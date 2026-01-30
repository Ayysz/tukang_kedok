using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Client
{ 
    public int id;
    public int score;

    public Client(int id)
    {
        this.id = id;
        this.score = 0;
    }
    public ClientData GetClientData()
    {
        return ClientDatabase.Instance.GetClient(id);
    }
}


public class ClientDatabase : MonoBehaviour
{
    public static ClientDatabase Instance;

    public List<ClientData> clients = new List<ClientData>();

    public Dictionary<int, ClientData> clientDictionary = new Dictionary<int, ClientData>();
    void Awake()
    {
        Instance = this;
        Initialize();
    }
    public void Initialize()
    {
        for (int i = 0; i < clients.Count; i++)
        {
            clientDictionary.Add(clients[i].id, clients[i]);
        }
    }
    public ClientData GetClient(int id)
    { 
        return clientDictionary[id];
    }
}
