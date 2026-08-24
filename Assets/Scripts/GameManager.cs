using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EventType
{
    SupervisorDiscontent,
    PeopleDiscontent,
    PostInterest,
    None
}

[Serializable]
public struct GameEffect
{
    public EventType eventType;
    public int value;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private Message firstMessage;

    [SerializeField] private int supervisorDiscontent;   
    [SerializeField] private int peopleDiscontent;
    [SerializeField] private int postInterest;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreateIfNeeded()
    {
        if (Instance != null)
            return;

        GameObject manager = new GameObject("GameManager");
        manager.AddComponent<GameManager>();
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

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

    private void CheckGameEnded(int dayCount)
    {
        if (dayCount >= 8)
        {
            LoadSceneAsync("EndCutscene");
        }
    }

    private void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!sceneLoad.isDone)
        {
            yield return null;
        }
    } 

    private void OnEnable()
    {
        GameBus.OnEffectRequested += ApplyGameEffect;
        GameBus.OnDayChange += CheckGameEnded;
    }

    private void OnDisable()
    {
        GameBus.OnEffectRequested -= ApplyGameEffect;
        GameBus.OnDayChange -= CheckGameEnded;
    }
}
