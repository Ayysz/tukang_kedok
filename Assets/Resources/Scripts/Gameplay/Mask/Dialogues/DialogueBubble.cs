using TMPro;
using UnityEngine;

public class DialogueBubble : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    public void DisplayText(string name,string dialogue)
    {
        nameText.text = name;
        dialogueText.text = dialogue;
    }
}
