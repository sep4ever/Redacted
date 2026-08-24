using System.Collections;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup dayOverlay;
    [SerializeField] private CanvasGroup gameCanvasGroup;
    [SerializeField] private TMP_Text dayCountText;
    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private float loadingDelay = 1f;

    private void OnEnable()
    {
        GameBus.OnDayChange += PlayDayChangeAnim;
    }

    private void OnDisable()
    {
        GameBus.OnDayChange -= PlayDayChangeAnim;
    }

    private void PlayDayChangeAnim(int dayCount)
    {
        StartCoroutine(DayChangeCoroutine(dayCount));
    }

    IEnumerator DayChangeCoroutine(int dayCount)
    {
        gameCanvasGroup.alpha = 0;
        dayCountText.text = $"День {dayCount}.";
        float t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            dayOverlay.alpha = t;
            yield return null;
        }
        t = 1f;
        yield return new WaitForSeconds(loadingDelay);

        while (t >= 0f)
        {
            t -= Time.deltaTime * fadeSpeed;
            dayOverlay.alpha = t;
            yield return null;
        }
        t = 0f;
        dayOverlay.alpha = t;
        gameCanvasGroup.alpha = 1;
    }
}
