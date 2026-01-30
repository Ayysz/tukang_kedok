using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int proggress;
    public ClientData CurrentClientData;
    public ClientManager clientManager;
    public DialogueController playerDialogue;

    public MaskData MaskData;

    public List<MaskData> maskScoring = new List<MaskData>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(StartGameCoroutine());
    }
    private IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSeconds(1);
        StartGame();
    }

    public void StartGame()
    { 
        Debug.Log("Game Started!");
        proggress = 0;
        CurrentClientData = ClientDatabase.Instance.GetClient(proggress);
        clientManager.SpawnClientPeople(CurrentClientData);
        

    }
    public void ClientDone()
    {
        clientManager.ClientPeopleOut(ClientDoneAddProggress);
    }
    public void AddScore(MaskData data)
    {
        maskScoring.Add(data);
    }
    public void ClientDoneAddProggress()
    {
        proggress++;
        if (proggress >= 4)
        {
            Debug.Log("Win Game");
        }
        else {
            clientManager.DestroyCurrentPeople();
            CurrentClientData = ClientDatabase.Instance.GetClient(proggress);
            clientManager.SpawnClientPeople(CurrentClientData);
        }
    }
    public void SetPlayerDialogue(DialogueSO dialogue,Action action)
    {
        playerDialogue.SetDialogue(dialogue, action);
    }
    
}
