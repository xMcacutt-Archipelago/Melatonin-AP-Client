using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Colors;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Archipelago.MultiClient.Net.MessageLog.Parts;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Packets;
using Melatonin_AP_Client.Hooks;
using UnityEngine;
using Random = System.Random;

namespace Melatonin_AP_Client
{
    public class ArchipelagoHandler : MonoBehaviour
    {
        private ArchipelagoSession? Session { get; set; }

        private string? Server { get; set; }
        public string? Slot { get; set; }
        private string? Password { get; set; }
        public string? seed;
        public bool IsConnected => Session?.Socket.Connected ?? false;

        public event System.Action? OnConnected;
        public event Action<string>? OnConnectionFailed;
        public event System.Action? OnDisconnected;

        private ConcurrentQueue<long> _locationsToCheck = new ConcurrentQueue<long>();
        private readonly Random _random = new Random();

        public volatile bool connectionFinished;
        public volatile bool connectionSucceeded;
        private readonly bool _queueBreak = false;

        private HashSet<int> starIds = new HashSet<int>();
        public int starCount => starIds.Count;

        private readonly string[] _deathMessages =
        {
            "had a skill issue (died)",
        };

        public void CreateSession(string server, string slot, string password)
        {
            Server = server;
            Slot = slot;
            Password = password;
            _locationsToCheck = new ConcurrentQueue<long>();
            starIds = new HashSet<int>();
            Session = ArchipelagoSessionFactory.CreateSession(Server);
            Session.MessageLog.OnMessageReceived += OnMessageReceived;
            Session.Socket.ErrorReceived += OnError;
            Session.Socket.SocketClosed += OnSocketClosed;
            Session.Items.ItemReceived += ItemReceived;
        }

        public IEnumerator ConnectRoutine()
        {
            APConsole.Instance.Log($"Logging in to {Server} as {Slot}...");
            var connectTask = Session!.ConnectAsync();

            yield return new WaitUntil(() => connectTask.IsCompleted);

            if (connectTask.Exception != null)
            {
                APConsole.Instance.Log(connectTask.Exception.ToString());
                yield break;
            }
            
            seed = connectTask.Result.SeedName;
            
            var loginTask = Session?.LoginAsync(
                PluginMain.GameName,
                Slot,
                ItemsHandlingFlags.AllItems,
                new System.Version(0, 6, 5), 
                Array.Empty<string>(),
                password: Password
            );

            yield return new WaitUntil(() => loginTask.IsCompleted);
            if (loginTask.Exception != null)
            {
                APConsole.Instance.Log(loginTask.Exception.ToString());
                yield break;
            }

            
            if (loginTask.Result.Successful)
            {
                APConsole.Instance.Log($"Success! Connected to {Server}");
                var successResult = (LoginSuccessful)loginTask.Result;
                PluginMain.SlotData = new SlotData(successResult.SlotData);

                PluginMain.ArchipelagoHandler.StartCoroutine(RunCheckQueue());
                connectionSucceeded = true;
                connectionFinished = true;
                OnConnected?.Invoke();
                yield break;
            }

            connectionSucceeded = false;
            connectionFinished = true;
            if (loginTask.Result != null)
            {
                var failure = (LoginFailure)loginTask.Result;
                var errorMessage = $"Failed to Connect to {Server} as {Slot}:";
                errorMessage = failure.Errors.Aggregate(errorMessage, (current, error) => current + $"\n    {error}");
                errorMessage =
                    failure.ErrorCodes.Aggregate(errorMessage, (current, error) => current + $"\n    {error}");
                OnConnectionFailed?.Invoke(errorMessage);
                APConsole.Instance.Log(errorMessage);
            }

            APConsole.Instance.Log("Attempting reconnect...");
        }

        public void Connect()
        {
            StartCoroutine(ConnectRoutine());
        }

        public void Disconnect()
        {
            if (Session == null)
                return;
            StopAllCoroutines();
            Session.Socket.DisconnectAsync();
            Session = null;
            APConsole.Instance.Log("Disconnected from Archipelago");
        }

        private void OnError(Exception ex, string message)
        {
            APConsole.Instance.Log($"Socket error: {message} - {ex.Message}");
        }

        private void OnSocketClosed(string reason)
        {
            StopAllCoroutines();
            APConsole.Instance.Log($"Socket closed: {reason}");
            OnDisconnected?.Invoke();
        }

        private void ItemReceived(ReceivedItemsHelper helper)
        {
            try
            {
                while (helper.Any())
                {
                    helper.DequeueItem();
                    starIds.Add(helper.Index);
                    PluginMain.logger.LogInfo(helper.Index);
                }
                TotalBoxPatches.SetStars(starCount);
                UnlockHandler.CheckUnlocks(starCount);
            }
            catch (Exception ex)
            {
                APConsole.Instance.Log($"ItemReceived Error: {ex}");
                throw;
            }
        }
        
        public void Release()
        {
            Session?.SetGoalAchieved();
            Session?.SetClientState(ArchipelagoClientState.ClientGoal);
        }

        public void CheckLocations(long[] ids)
        {
            ids.ToList().ForEach(id => _locationsToCheck.Enqueue(id));
        }

        public void CheckLocation(long id)
        {
            _locationsToCheck.Enqueue(id);
        }

        private IEnumerator RunCheckQueue()
        {
            while (true)
            {
                if (_locationsToCheck.TryDequeue(out var locationId))
                {
                    Session?.Locations.CompleteLocationChecks(locationId);
                    APConsole.Instance.DebugLog($"Sent location check: {locationId}");
                }

                if (_queueBreak)
                    yield break;
                yield return new WaitForSeconds(0.1f);
            }
        }

        public bool IsLocationChecked(long id)
        {
            return Session != null && Session.Locations.AllLocationsChecked.Contains(id);
        }

        public int CountLocationsCheckedInRange(long start, long end)
        {
            return Session != null ? Session.Locations.AllLocationsChecked.Count(loc => loc >= start && loc < end) : 0;
        }

        public int CountLocationsCheckedInRange(long start, long end, long delta)
        {
            return Session != null
                ? Session.Locations.AllLocationsChecked.Count(loc =>
                    loc >= start && loc < end && loc % delta == start % delta)
                : 0;
        }

        public void UpdateTags(List<string> tags)
        {
            var packet = new ConnectUpdatePacket
            {
                Tags = tags.ToArray(),
                ItemsHandling = ItemsHandlingFlags.AllItems
            };
            Session?.Socket.SendPacket(packet);
        }

        private void OnMessageReceived(LogMessage message)
        {
            string messageStr;
            if (message.Parts.Any(x => x.Type == MessagePartType.Player) &&
                PluginMain.FilterLog != null &&
                PluginMain.FilterLog.Value &&
                !message.Parts.Any(x => x.Text.Contains(Session!.Players.GetPlayerName(Session.ConnectionInfo.Slot))))
                return;
            if (message.Parts.Length == 1)
            {
                messageStr = message.Parts[0].Text;
            }
            else
            {
                var builder = new StringBuilder();
                foreach (var part in message.Parts)
                {
                    builder.Append($"{part.Text}");
                }

                messageStr = builder.ToString();
            }

            APConsole.Instance.Log(messageStr);
        }

        public ScoutedItemInfo? TryScoutLocation(long locationId)
        {
            return Session?.Locations.ScoutLocationsAsync(locationId)?.Result?.Values.First();
        }
    }
}