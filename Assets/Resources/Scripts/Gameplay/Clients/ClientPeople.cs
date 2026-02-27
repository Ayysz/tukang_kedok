using UnityEngine;
using DG.Tweening;
using System.Collections;

public class ClientPeople : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private DialogueController dialogue;

    [SerializeField] private MaskData maskData;

    [SerializeField] private AudioClip walk;
    [SerializeField] private GameObject[] objectState;
    ClientData data;

    public void SetDialogue(DialogueController dc)
    {
        dialogue = dc;
    }
    public void SetDataFirstTime(ClientData data)
    {
        this.data = data;
        AudioManager.Instance.PlaySfx(walk);
        State1();
    }
    public ClientData GetData()
    {
        return data;
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
            if (data.doneClip.Length > 0)
            {
                for (int i = 0; i < data.doneClip.Length; i++)
                {
                    AudioManager.Instance.PlaySfx(data.doneClip[i]);
                }
            }
            GameManager.Instance.CompletedAMask(()=> {
                dialogue.SetDialogue(data.doneDialogue, DoneEnd);
                // GameManager.Instance.AfterOkay();
                GameManager.Instance.HideTaskAtas();
                GameManager.Instance.CameraChange();
            });
        }
        else
        {
            GameManager.Instance.UpdateTaskAtas();
        }
    }
    public void State1()
    {
        objectState[0].SetActive(true);
        objectState[1].SetActive(false);
    }
    public void State2()
    {
        objectState[1].SetActive(true);
        objectState[0].SetActive(false);
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
        ClientPeople cp = GameManager.Instance.clientManager.currentClientPeople;
        GameManager.Instance.SpawnMask(cp.GetMaskData().GetMaskDataSO().maskDisplayPrefab, cp.GetMaskData().currentProgress);
    }
}
