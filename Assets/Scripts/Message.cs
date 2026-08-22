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
    public UnityAction OnChoosen;
    public string OptionDescription;

    public void Choose()
    {
        OnChoosen?.Invoke();
    }

    public Option(string optionDesc, UnityAction onOptionChoosen = null)
    {
        OptionDescription = optionDesc;
        OnChoosen = onOptionChoosen;
    }
}
