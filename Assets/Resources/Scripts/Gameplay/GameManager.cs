using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int proggress;
    public ClientData CurrentClientData;
    public ClientManager clientManager;
    public DialogueController playerDialogue;

    public MaskData MaskData;

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
        CurrentClientData = ClientDatabase.Instance.GetClient(proggress);
        clientManager.SpawnClientPeople(CurrentClientData);
        

    }
    public void SetPlayerDialogue(DialogueSO dialogue,Action action)
    {
        playerDialogue.SetDialogue(dialogue, action);
    }
    
}
