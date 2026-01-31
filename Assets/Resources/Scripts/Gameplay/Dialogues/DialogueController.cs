using System;
using System.Collections;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private DialogueBubble dialogueBubble;
    [SerializeField] private int dialogueIndex;
    [SerializeField] private DialogueSO currentDialogueSO;

    public Action OnEndDialogue;

    
    public void SetDialogue(DialogueSO dialogueSO,Action action)
    {
        OnEndDialogue = action;
        dialogueIndex = 0;
        currentDialogueSO = dialogueSO;
        dialogueBubble.gameObject.SetActive(true);
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
                dialogueBubble.gameObject.SetActive(false);
                dialogueIndex = 0;
                StartCoroutine(EndDelay());

            }
        }
        else { 
            dialogueBubble.gameObject.SetActive(false);
            dialogueIndex = 0;
            StartCoroutine(EndDelay());


        }

    }
    private IEnumerator EndDelay()
    {
        yield return new WaitForSeconds(0.2f);
        OnEndDialogue?.Invoke();
        OnEndDialogue = null;
    }

    public void ShowDialogue(string name, string dialogue)
    {
        dialogueBubble.DisplayText(name, dialogue);
    }
}
