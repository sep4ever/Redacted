using System;
using UnityEngine;

public enum EventType
{
    SupervisorDiscontent,
    PeopleDiscontent,
    PostInterest
}

[Serializable]
public struct GameEffect
{
    public EventType eventType;
    public int value;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private Message firstMessage;

    [SerializeField] private int supervisorDiscontent;   
    [SerializeField] private int peopleDiscontent;
    [SerializeField] private int postInterest;

    private void Awake()
    {
        GameBus.SendMessage(firstMessage);
    }

    private void ApplyGameEffect(GameEffect gameEffect)
    {
        switch (gameEffect.eventType)
        {
            case EventType.SupervisorDiscontent:
                supervisorDiscontent += gameEffect.value;
                break;
            case EventType.PeopleDiscontent:
                peopleDiscontent += gameEffect.value;
                break;
            case EventType.PostInterest:
                postInterest += gameEffect.value;
                break;
            default:
                break;
        }
    }

    private void OnEnable()
    {
        GameBus.OnEffectRequested += ApplyGameEffect;
    }

    private void OnDisable()
    {
        GameBus.OnEffectRequested -= ApplyGameEffect;
    }
}
