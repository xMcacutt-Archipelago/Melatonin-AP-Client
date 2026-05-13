using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace Melatonin_AP_Client;

public class Dispatcher : MonoBehaviour
{
    private static readonly ConcurrentQueue<Action> ActionQueue = new();
    public static void Run(Action action) { ActionQueue.Enqueue(action); }
    private void Update() { while (ActionQueue.TryDequeue(out var action)) action.Invoke(); }
}