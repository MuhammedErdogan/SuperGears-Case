using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    private static readonly Dictionary<Delegate, Delegate> EventDictionary = new();

    private static void AddListener(Delegate eventType, Delegate listener)
    {
        if (EventDictionary.ContainsKey(eventType))
        {
            EventDictionary[eventType] = Delegate.Combine(EventDictionary[eventType], listener);
        }
        else
        {
            EventDictionary[eventType] = listener;
        }
    }

    private static void RemoveListener(Delegate eventType, Delegate listener)
    {
        if (!EventDictionary.ContainsKey(eventType))
        {
            return;
        }

        EventDictionary[eventType] = Delegate.Remove(EventDictionary[eventType], listener);
    }

    private static void InvokeEvent(Delegate eventType, params object[] parameter)
    {
        if (!EventDictionary.TryGetValue(eventType, out var thisEvent))
        {
            return;
        }

        var callbacks = thisEvent.GetInvocationList();

        foreach (var callback in callbacks)
        {
            callback?.DynamicInvoke(parameter);
        }
    }

    #region Overloads
    public static void StartListening(Action eventType, Action listener)
    {
        AddListener(eventType, listener);
    }

    public static void StartListening<T>(Action<T> eventType, Action<T> listener)
    {
        AddListener(eventType, listener);
    }

    public static void StartListening<T1, T2>(Action<T1, T2> eventType, Action<T1, T2> listener)
    {
        AddListener(eventType, listener);
    }

    public static void StartListening<T1, T2, T3>(Action<T1, T2, T3> eventType, Action<T1, T2, T3> listener)
    {
        AddListener(eventType, listener);
    }

    public static void StartListening<T1, T2, T3, T4>(Action<T1, T2, T3, T4> eventType, Action<T1, T2, T3, T4> listener)
    {
        AddListener(eventType, listener);
    }

    public static void StartListening<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> eventType, Action<T1, T2, T3, T4, T5> listener)
    {
        AddListener(eventType, listener);
    }

    public static void StopListening(Action eventType, Action listener)
    {
        RemoveListener(eventType, listener);
    }

    public static void StopListening<T>(Action<T> eventType, Action<T> listener)
    {
        RemoveListener(eventType, listener);
    }

    public static void StopListening<T1, T2>(Action<T1, T2> eventType, Action<T1, T2> listener)
    {
        RemoveListener(eventType, listener);
    }

    public static void StopListening<T1, T2, T3>(Action<T1, T2, T3> eventType, Action<T1, T2, T3> listener)
    {
        RemoveListener(eventType, listener);
    }

    public static void StopListening<T1, T2, T3, T4>(Action<T1, T2, T3, T4> eventType, Action<T1, T2, T3, T4> listener)
    {
        RemoveListener(eventType, listener);
    }

    public static void StopListening<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> eventType, Action<T1, T2, T3, T4, T5> listener)
    {
        RemoveListener(eventType, listener);
    }

    public static void TriggerEvent(Action eventType)
    {
        InvokeEvent(eventType);
    }

    public static void TriggerEvent<T>(Action<T> eventType, T parameter)
    {
        InvokeEvent(eventType, parameter);
    }

    public static void TriggerEvent<T1, T2>(Action<T1, T2> eventType, T1 parameter1, T2 parameter2)
    {
        InvokeEvent(eventType, parameter1, parameter2);
    }

    public static void TriggerEvent<T1, T2, T3>(Action<T1, T2, T3> eventType, T1 parameter1, T2 parameter2, T3 parameter3)
    {
        InvokeEvent(eventType, parameter1, parameter2, parameter3);
    }

    public static void TriggerEvent<T1, T2, T3, T4>(Action<T1, T2, T3, T4> eventType, T1 parameter1, T2 parameter2, T3 parameter3, T4 parameter4)
    {
        InvokeEvent(eventType, parameter1, parameter2, parameter3, parameter4);
    }

    public static void TriggerEvent<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> eventType, T1 parameter1, T2 parameter2, T3 parameter3, T4 parameter4, T5 parameter5)
    {
        InvokeEvent(eventType, parameter1, parameter2, parameter3, parameter4, parameter5);
    }
    #endregion

    #region Actions

    public static readonly Action GameStarted = () => { };
    public static readonly Action GameFinished = () => { };
    public static readonly Action GamePaused = () => { };
    public static readonly Action GameResumed = () => { };
    public static readonly Action GameFailed = () => { };
    public static readonly Action GameRevived = () => { };

    public static readonly Action ConnectionLost = () => { };
    public static readonly Action Connected = () => { };

    public static readonly Action CarInitialized = () => { };
    public static readonly Action<float> NotifySpeed = (float _) => { };
    public static readonly Action<Rigidbody> OnCarMove = (Rigidbody _) => { };

    #endregion
}