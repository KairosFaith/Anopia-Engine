using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Core
{
    //event bag
    static Dictionary<string, GameEvent> _eventBag = new Dictionary<string, GameEvent>();

    //register event
    public static void SubscribeEvent(string eventName, GameEvent eventObj)
    {
        GameEvent existing = null;
        if(_eventBag.TryGetValue(eventName, out existing))
        {
            //subscribe
            existing += eventObj;
        }
        else
        {
            //assigning
            existing = eventObj;
        }
        _eventBag[eventName] = existing;
    }
    //unregister event
    public static void UnsubscribeEvent(string eventName, GameEvent eventObj)
    {
        if(_eventBag.TryGetValue(eventName, out GameEvent existing))
        {
            existing -= eventObj;
            if(existing==null)
            {
                _eventBag.Remove(eventName);
            }
            else
            {
                _eventBag[eventName] = existing;
            }
        }
    }
    //clear all events
    public static void ClearAllEvents()
    {
        _eventBag.Clear();
    }
    //broadcast event
    public static void BroadcastEvent(string eventName, object sender, params object[] args)
    {
        if(_eventBag.TryGetValue(eventName, out GameEvent existing))
        {
            existing(sender, args);
        }
    }
}

public delegate void GameEvent(object sender, object[] args);