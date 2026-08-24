using System;

public static class GameBus
{
    public static event Action<Message> OnMessage;
    public static event Action<GameEffect> OnEffectRequested;

    public static void SendMessage(Message message)
    {
        OnMessage?.Invoke(message);
    }
    public static void RequestEffect(GameEffect effect)
    {
        OnEffectRequested?.Invoke(effect);
    }
}