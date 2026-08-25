using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

public enum EndingType
{
    Execution,
    Hanged,
    Fired,
    GoodEnding
}

public class GameManager : MonoBehaviour
{
    public static event Action<string> OnSceneLoadRequested;
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
            ChooseEnding();
        }
    }

    private void ChooseEnding()
    {
        var endingParameters = new Dictionary<EndingType, int>
        {
            { EndingType.Execution, supervisorDiscontent },
            { EndingType.Hanged, peopleDiscontent },
            { EndingType.Fired, postInterest }
        };

        int max = endingParameters.Values.Max();

        if (max < 3)
        {
            LoadEnding(EndingType.GoodEnding);
            return;
        }

        var endings = endingParameters
            .Where(x => x.Value == max)
            .Select(x => x.Key)
            .ToList();

        EndingType end = endings[UnityEngine.Random.Range(0, endings.Count)];

        LoadEnding(end);
    }

    private void LoadEnding(EndingType end)
    {
        switch (end)
        {
            case EndingType.Execution:
                LoadSceneAsync("ExecutionEnd");
                break;
            case EndingType.Hanged:
                LoadSceneAsync("HangedEnd");
                break;
            case EndingType.Fired:
                LoadSceneAsync("FiredEnd");
                break;
            case EndingType.GoodEnding:
                LoadSceneAsync("GoodEnd");
                break;
        }
    }

    public void LoadSceneAsync(string sceneName)
    {
        OnSceneLoadRequested.Invoke(sceneName);
    }
    public void StartLoadSceneCoroutine(string sceneName)
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
