using UnityEngine;
using DG.Tweening;
using System.Collections;

public class ClientPeople : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private DialogueController dialogue;

    [SerializeField] private MaskData maskData;
    ClientData data;

    public void SetDialogue(DialogueController dc)
    {
        dialogue = dc;
    }
    public void SetDataFirstTime(ClientData data)
    {
        this.data = data;
    }
    public void StartClient()
    {
        dialogue.SetDialogue(data.startDialogue,DoneStart);
    }
    public void AddProggress()
    {
        maskData.AddProggress();

        if (maskData.isCompleted)
        {
            Debug.Log("Complete");
            dialogue.SetDialogue(data.doneDialogue, DoneEnd);
            GameManager.Instance.AfterOkay();
            GameManager.Instance.HideTaskAtas();
        }
        else
        {
            GameManager.Instance.UpdateTaskAtas();
        }
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
    public void DoneEnd()
    {
        // Animasi Client Done 
        //Camera Zoom in
        // Animasi Idle
        //GameManager.Instance.ClientDone();
        GameManager.Instance.AddScore(maskData);
        StartCoroutine(DoneEndDelay());
    }
    private IEnumerator DoneEndDelay()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.ClientDone();

    }
    public void AfterFirstMCDialogue()
    {
        Debug.Log("FirstMCDialogueDone");
        GameManager.Instance.UpdateTaskAtas();
        GameManager.Instance.AfterOkay();
    }
}
