using UnityEngine;

[CreateAssetMenu(fileName = "NewClient",menuName ="Data/ClientData")]
public class ClientData : ScriptableObject
{
    public int id;
    public string clientName;
    public string clientDescription;
    public ClientPeople clientPeople;
    public DialogueSO startDialogue;
    public DialogueSO doneDialogue;
}
