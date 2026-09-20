using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Typewriter/Message")]
public class Message : ScriptableObject
{
    public Option[] Options = Array.Empty<Option>();
    [TextArea(5, 10)]
    public string Text; 

    public Message(string text, Option[] options = null)
    {
        Text = text;

        if (options != null)
        {
            Options = options;
        }
    }
}

//Хардкодить смену дня это, конечно, так себе решение, но для геймджема пойдёт.
[Serializable]
public class Option
{
    public GameEffect optionEffect;
    public Action OnChosen;
    public string OptionDescription;
    public Message NextMessage;
    public int dayCount;
    public bool triggerDayChange;

    public void Choose()
    {
        OnChosen?.Invoke();
        GameBus.RequestEffect(optionEffect);
        if (NextMessage != null)
            GameBus.SendMessage(NextMessage);
        
        if (triggerDayChange)
            GameBus.TriggerDayChange(dayCount);
    }

    public Option(string optionDesc, Action onOptionChosen = null, Message nextMessage = null)
    {
        OptionDescription = optionDesc;
        OnChosen = onOptionChosen;
        NextMessage = nextMessage;
    }
}