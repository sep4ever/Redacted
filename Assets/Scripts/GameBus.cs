using System;
using UnityEngine;
public static class GameBus
{
    public static event Action<Message> OnMessage;
    public static event Action<GameEffect> OnEffectRequested;
    public static event Action<int> OnDayChange;

    public static void SendMessage(Message message)
    {
        Debug.Log("Sent a message. Filling: " + message.Text);
        OnMessage?.Invoke(message);
    }
    public static void RequestEffect(GameEffect effect)
    {
        OnEffectRequested?.Invoke(effect);
    }

    public static void TriggerDayChange(int dayCount)
    {
        OnDayChange.Invoke(dayCount);
    }
}