using System.IO;
using Newtonsoft.Json;

namespace Melatonin_AP_Client
{
    public class ConnectionInfo
    {
        public ConnectionInfo(string server, string slot, string password)
        {
            Server = server;
            Slot = slot;
            Password = password;
        }

        public string Server { get; set; }
        public string Slot { get; set; }
        public string Password { get; set; }
    }

    public static class ConnectionInfoHandler
    {
        private const string Path = "./ArchipelagoSaves/" + "connection_info.json";
        public static string savedServer;
        public static string savedSlot;
        public static string savedPassword;

        public static void Save(string server, string slot, string password)
        {
            savedServer = server;
            savedSlot = slot;
            savedPassword = password;
            if (!Directory.Exists("./ArchipelagoSaves/"))
                Directory.CreateDirectory("./ArchipelagoSaves/");
            var connectionInfo = new ConnectionInfo(server, slot, password);
            var text = JsonConvert.SerializeObject(connectionInfo);
            File.WriteAllText(Path, text);
        }

        public static bool Load()
        {
            if (!File.Exists(Path))
                Save(savedServer, savedSlot, savedPassword);
            var json = File.ReadAllText(Path);
            var connectionInfo = JsonConvert.DeserializeObject<ConnectionInfo>(json);
            if (connectionInfo == null) 
                return false;
            savedServer = connectionInfo.Server;
            savedSlot = connectionInfo.Slot;
            savedPassword = connectionInfo.Password;
            return true;
        }
    }
}
