using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    private Dictionary<string, List<Action<Data>>> _eventListeners = new Dictionary<string, List<Action<Data>>>();

    public static void RemoveEventListener(string eventName, Action<Data> functionToCall)
    {
        print("RemoveEvent");
        Instance._eventListeners[eventName].Remove(functionToCall);
    }

    public static void AddEventListener(string eventName, Action<Data> functionToCall)
    {
        print("AddEvent");
        //EventObserver observer = new EventObserver(subject, eventName, functionToCall);
        if (Instance._eventListeners.ContainsKey(eventName))
        {
            Instance._eventListeners[eventName].Add(functionToCall);
            print("attached");
        }
        Instance._eventListeners.Add(eventName, new List<Action<Data>>(){functionToCall});
    }

    public static void Dispatch(string eventName, Data data)
    {
        Instance.CleanEventsFromNull(eventName);
        List<Action<Data>> functions = new List<Action<Data>>(Instance._eventListeners[eventName]);
        print("Dispatch");
        foreach (Action<Data> functionToCall in functions)
        {
            functionToCall(data);
        }
    }

    private void CleanEventsFromNull(string eventName)
    {
        Instance._eventListeners[eventName].RemoveAll(item => item == null);
    }
}