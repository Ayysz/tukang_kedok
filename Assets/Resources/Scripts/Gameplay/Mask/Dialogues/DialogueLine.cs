using UnityEngine;

[System.Serializable]
public class DialogueLine 
{
    public string SpeakerName;
    [TextArea(2, 4)]
    public string Text;
    public DialogueLine(string speakerName, string text)
    {
        SpeakerName = speakerName;
        Text = text;
    }
}
