using UnityEngine;

public class ClientPeople : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private DialogueController dialogue;

    [SerializeField] private MaskData maskData;
    ClientData data;
    public void SetDataFirstTime(ClientData data)
    {
        this.data = data;
    }
    public void StartClient()
    {
        dialogue.SetDialogue(data.startDialogue,DoneStart);
    }
    public MaskData GetMaskData()
    {
        return maskData;
    }
    public void DoneStart()
    {
        Debug.Log("Done Start!");
        maskData = new MaskData(data.maskDataSO.id, 0);
        GameManager.Instance.playerDialogue.SetDialogue(data.startMCDialogue,AfterFirstMCDialogue);
    }
    public void AfterFirstMCDialogue()
    {
        Debug.Log("FirstMCDialogueDone");
    }
}
