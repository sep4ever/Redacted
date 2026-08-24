using System;
using UnityEngine;

public class IntroWriter : MonoBehaviour
{
    [SerializeField] private Message startingMessage;
    void Start()
    {
        GameBus.SendMessage(startingMessage);
    }

}
