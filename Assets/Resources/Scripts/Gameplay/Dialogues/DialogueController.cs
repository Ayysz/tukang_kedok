using System;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private DialogueBubble dialogueBubble;
    [SerializeField] private int dialogueIndex;
    [SerializeField] private DialogueSO currentDialogueSO;

    public Action OnEndDialogue;

    
    public void SetDialogue(DialogueSO dialogueSO,Action action)
    {
        dialogueBubble.gameObject.SetActive(true);
        OnEndDialogue = action;
        currentDialogueSO = dialogueSO;
        dialogueIndex = 0;
        ShowDialogue(dialogueSO.lines[dialogueIndex].SpeakerName, dialogueSO.lines[dialogueIndex].Text);
    }
    public void NextDialogue()
    {
        if (dialogueIndex < currentDialogueSO.lines.Count)
        {
            dialogueIndex++;
            if (dialogueIndex < currentDialogueSO.lines.Count)
            {
                ShowDialogue(currentDialogueSO.lines[dialogueIndex].SpeakerName, currentDialogueSO.lines[dialogueIndex].Text);
            }
            else
            {
                OnEndDialogue?.Invoke();
                OnEndDialogue = null;
                dialogueBubble.gameObject.SetActive(false);

            }
        }
        else { 
            OnEndDialogue?.Invoke();
            OnEndDialogue = null;
            dialogueBubble.gameObject.SetActive(false);

        }

    }

    public void ShowDialogue(string name, string dialogue)
    {
        dialogueBubble.DisplayText(name, dialogue);
    }
}
