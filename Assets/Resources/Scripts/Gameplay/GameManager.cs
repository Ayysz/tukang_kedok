using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int proggress;
    public ClientData CurrentClientData;
    public ClientManager clientManager;

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
}
