using UnityEngine;
using DG.Tweening;
using System;

public class ClientManager : MonoBehaviour
{
    public static ClientManager Instance
    {
        get; private set;
    }
    [SerializeField] private Transform clientPeoplePos;
    [SerializeField] private Transform clientPeopleStopPos;
    public ClientPeople currentClientPeople;
    Action OnClientPeopleOut;

    private void Awake()
    {
        Instance = this;
    }
    public void DestroyCurrentPeople()
    {
        Destroy(currentClientPeople.gameObject);
    }
    public void SpawnClientPeople(ClientData data)
    { 
        ClientPeople people = Instantiate(data.clientPeople,clientPeoplePos.transform.position,Quaternion.identity);
        currentClientPeople = people;
        people.SetDialogue(GameManager.Instance.mainDialogue);
        people.SetDataFirstTime(data);
        people.transform.DOMove(clientPeopleStopPos.position,1f).SetEase(Ease.OutQuad).OnComplete(ClientStop);
    }
    public void ClientPeopleOut(Action action)
    {
        OnClientPeopleOut = action;
        currentClientPeople.transform.DOMove(clientPeoplePos.position, 1f).SetEase(Ease.OutQuad).OnComplete(ClientDone);
    }
    public void ClientDone()
    {
        OnClientPeopleOut.Invoke();
        OnClientPeopleOut = null;
    }
    public void ClientStop()
    {
        currentClientPeople.StartClient();
        
    }
}
