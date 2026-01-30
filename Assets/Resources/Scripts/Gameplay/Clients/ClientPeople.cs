using UnityEngine;

public class ClientPeople : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private DialogueController dialogue;
    ClientData data;
    public void SetDataFirstTime(ClientData data)
    {
        this.data = data;
    }
    public void StartClient()
    {
        dialogue.SetDialogue(data.startDialogue,DoneStart);
    }
    public void DoneStart()
    {
        Debug.Log("Done Start!");
    }
}
