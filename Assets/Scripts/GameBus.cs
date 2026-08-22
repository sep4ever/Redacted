using System;

public static class GameBus
{
    public static event Action<Message> OnMessage;

    public static void SendMessage(Message message)
    {
        OnMessage?.Invoke(message);
    }
}