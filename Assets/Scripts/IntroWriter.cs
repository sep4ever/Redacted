using UnityEngine;

public class IntroWriter : MonoBehaviour
{
    [SerializeField] private Message startingMessage;
    [SerializeField] private bool sendAtStart = true;
    private void Start()
    {
        if (sendAtStart)
            GameBus.SendMessage(startingMessage);
    }

    public void SendMessage()
    {
        GameBus.SendMessage(startingMessage);
    }

}
