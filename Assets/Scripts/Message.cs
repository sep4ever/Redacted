using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Message
{
    public Option[] Options = Array.Empty<Option>();
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

[Serializable]
public class Option
{
    public Action OnChosen;
    public string OptionDescription;

    public void Choose()
    {
        OnChosen?.Invoke();
    }

    public Option(string optionDesc, Action onOptionChosen = null)
    {
        OptionDescription = optionDesc;
        OnChosen = onOptionChosen;
    }
}
