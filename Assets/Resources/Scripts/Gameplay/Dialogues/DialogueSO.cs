using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewDialogue",
    menuName = "Game/Dialogue/Dialogue"
)]
public class DialogueSO : ScriptableObject
{
    public List<DialogueLine> lines;
}
