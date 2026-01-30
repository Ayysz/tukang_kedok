using UnityEngine;
using DG.Tweening;

public class ClientManager : MonoBehaviour
{
    public static ClientManager Instance
    {
        get; private set;
    }
    [SerializeField] private Transform clientPeoplePos;
    [SerializeField] private Transform clientPeopleStopPos;
    public ClientPeople currentClientPeople;

    private void Awake()
    {
        Instance = this;
    }
    public void SpawnClientPeople(ClientData data)
    { 

        ClientPeople people = Instantiate(data.clientPeople,clientPeoplePos.transform.position,Quaternion.identity);
        currentClientPeople = people;
        people.SetDataFirstTime(data);
        people.transform.DOMove(clientPeopleStopPos.position,1f).SetEase(Ease.OutQuad).OnComplete(ClientStop);

    }
    public void ClientStop()
    {
        currentClientPeople.StartClient();
    }
}
