using UnityEngine;
using UnityEngine.Playables;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private PlayableAsset loadingAnim;
    [SerializeField] private PlayableAsset startingAnim;
    private PlayableDirector playableDirector;
    private string sceneName;

    private void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    void OnEnable()
    {
        GameManager.OnSceneLoadRequested += ShowAnimation;
    }

    public void LoadScene()
    {
        GameManager.Instance.StartLoadSceneCoroutine(sceneName);
    }

    public void ShowAnimation(string sceneName)
    {
        this.sceneName = sceneName;
        playableDirector.Play(loadingAnim);
    }

    void OnDisable()
    {
        GameManager.OnSceneLoadRequested -= ShowAnimation;
    }
}
