// Universidad del Valle de Guatemala
// Daniel Garcia, 14152
// Programacion de plataformas moviles y juegos

using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

//Todo lo contenido en este script es del tutorial oficial de Unity:
// https://unity3d.com/learn/tutorials/topics/scripting/events-creating-simple-messaging-system
//La unica diferencia es que añadi un parametro tipo float al sistema, asi puedo pasar floats entre scripts.



[System.Serializable]
public class ThisEvent : UnityEvent<int>
{
}

public class EventManager : MonoBehaviour
{

    private Dictionary<string, ThisEvent> eventDictionary;

    private static EventManager eventManagerArgs;

    public static EventManager instance
    {
        get
        {
            if (!eventManagerArgs)
            {
                eventManagerArgs = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManagerArgs)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManagerArgs.Init();
                }
            }

            return eventManagerArgs;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, ThisEvent>();
        }
    }

    public static void startListening(string eventName, UnityAction<int> listener)
    {
        ThisEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new ThisEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void stopListening(string eventName, UnityAction<int> listener)
    {
        if (eventManagerArgs == null) return;
        ThisEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void triggerEvent(string eventName, int value)
    {
        ThisEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(value);
        }
    }
}