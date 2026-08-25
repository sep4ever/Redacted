using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//ИМХО отдельный struct читаемее.
[System.Serializable]
public struct TypingCharacter
{
    public char Character;
    public float CharDelay;
}

public class Typewriter : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private TypingCharacter[] typingCharacters;
    [SerializeField] private Button[] buttons;
    [SerializeField] private InputAction nextMessageAction;
    [SerializeField] private AudioClip typingSFX;
    private AudioSource typewriterAudioSource;
    private Dictionary<char, float> characterDelayDict = new Dictionary<char, float>();
    private Message currentMessage;

    private void OnEnable()
    {
        GameBus.OnMessage += Type;
    }

    private void OnDisable()
    {
        GameBus.OnMessage -= Type;
    }

    private void Awake()
    {
        foreach (TypingCharacter typingCharacter in typingCharacters)
        {
            characterDelayDict.Add(typingCharacter.Character, typingCharacter.CharDelay);
        }
        nextMessageAction = FindAnyObjectByType<PlayerInput>().actions["Next"];
        typewriterAudioSource = GetComponent<AudioSource>();
    }

    private bool IsTyping()
    {
        return typingCoroutine != null;
    }

    private void Update()
    {
        if (UIManager.AnimationIsPlaying())
            return;

        if (nextMessageAction.WasPressedThisFrame() && IsTyping())
        {
            text.text = currentMessage.Text;
            text.maxVisibleCharacters = currentMessage.Text.Length;
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
            return;
        }
    }

    private Coroutine typingCoroutine;
    public void Type(Message message)
    {
        currentMessage = message;
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
        }

        for (int i = 0; i < message.Options.Length; i++)
        {
            Button button = buttons[i];
            Option option = message.Options[i];

            button.gameObject.SetActive(true);
            button.GetComponentInChildren<TMP_Text>().text = option.OptionDescription;
            button.onClick.AddListener(option.Choose);
        }
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypingCoroutine(message.Text));
    }

    private IEnumerator TypingCoroutine(string textToType)
    {
        text.text = textToType;
        text.maxVisibleCharacters = 0;
        while (text.maxVisibleCharacters < textToType.Length)
        {
            if (UIManager.AnimationIsPlaying())
            {
                yield return null;
                continue;
            }
            text.maxVisibleCharacters++;
            char currentChar = textToType[text.maxVisibleCharacters - 1];
            PlayTypingSound();
            yield return new WaitForSeconds(GetCharDelay(currentChar));
        }
        text.maxVisibleCharacters = textToType.Length;
        typingCoroutine = null;
    }

    private void PlayTypingSound()
    {
        if (typewriterAudioSource != null && typingSFX != null)
        {
            float pitch = Random.Range(0.8f, 1.1f);
            typewriterAudioSource.pitch = pitch;
            typewriterAudioSource.PlayOneShot(typingSFX);
        }
    }

    private float GetCharDelay(char character)
    {
        if (characterDelayDict.TryGetValue(character, out float delay))
            return delay;
        else
            return 0.05f;
    }
}
