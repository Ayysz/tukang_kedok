using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum CameraType
{ 
    mainGame,
    MinigameHole,
    MinigameSculpt,
    MinigamePaint
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int proggress;
    public ClientData CurrentClientData;
    public ClientManager clientManager;
    public DialogueController playerDialogue;
    public TaskController taskAtasController;
    public CameraChangeController cameraChangeController;

    public MaskData MaskData;

    public List<MaskData> maskScoring = new List<MaskData>();
    public List<MinigamePlayController> minigameControllers = new List<MinigamePlayController>();

    public bool isMainGame;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(StartGameCoroutine());
    }

    public void UpdateTaskAtas()
    {
        taskAtasController.Show();
        ClientPeople cp = clientManager.currentClientPeople;
        taskAtasController.SetTaskLines(cp.GetMaskData().GetMaskDataSO().craftingTypes, cp.GetMaskData().currentProgress);
    }
    public void HideTaskAtas()
    {
        taskAtasController.Hide();
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
    public void GoToMinigame(CraftingType craftingType)
    {
        if (craftingType == CraftingType.Sculpting)
        {
            MaskData md = clientManager.currentClientPeople.GetMaskData();
            MinigameSculptDataSO setting = md.GetMaskDataSO().minigameSettingDatas[md.currentProgress] as MinigameSculptDataSO;
            minigameControllers[1].OnEndMinigame += MinigameClear;
            ChangeCamera(CameraType.MinigameSculpt);
            minigameControllers[1].StartMinigame(setting);
            isMainGame = false;


        }
        else if (craftingType == CraftingType.MakingHole)
        {

            MaskData md = clientManager.currentClientPeople.GetMaskData();
            MinigameHoleSettingSO holeSetting = md.GetMaskDataSO().minigameSettingDatas[md.currentProgress] as MinigameHoleSettingSO;
            minigameControllers[0].OnEndMinigame += MinigameClear;
            ChangeCamera(CameraType.MinigameHole);
            minigameControllers[0].StartMinigame(holeSetting);
            isMainGame = false;


        }
        else if (craftingType == CraftingType.Painting)
        {
            MaskData md = clientManager.currentClientPeople.GetMaskData();
            MinigameHoleSettingSO holeSetting = md.GetMaskDataSO().minigameSettingDatas[md.currentProgress] as MinigameHoleSettingSO;
            minigameControllers[2].OnEndMinigame += MinigameClear;
            ChangeCamera(CameraType.MinigamePaint);
            minigameControllers[2].StartMinigame(holeSetting);
            isMainGame = false;

        }
        else if (craftingType == CraftingType.Cement)
        {
            MaskData md = clientManager.currentClientPeople.GetMaskData();
            MinigameCementSettingSO holeSetting = md.GetMaskDataSO().minigameSettingDatas[md.currentProgress] as MinigameCementSettingSO;
            minigameControllers[3].OnEndMinigame += MinigameClear;
            ChangeCamera(CameraType.mainGame);
            minigameControllers[3].StartMinigame(holeSetting);
            isMainGame = false;
        }
    }
    public void MinigameClear()
    {
        ClientPeople cp = GameManager.Instance.clientManager.currentClientPeople;
        int curProgress = cp.GetMaskData().currentProgress;
        cp.AddProggress();
        ChangeCamera(CameraType.mainGame);
        isMainGame = true;
    }
    public void ChangeCamera(CameraType type)
    {
        if (type == CameraType.mainGame)
        {
            cameraChangeController.ChangeCameraActive(0);
        }
        else if (type == CameraType.MinigameHole)
        {
            cameraChangeController.ChangeCameraActive(1);
        }
        else if (type == CameraType.MinigameSculpt)
        {
            cameraChangeController.ChangeCameraActive(2);
        }
        else if (type == CameraType.MinigamePaint)
        {
            cameraChangeController.ChangeCameraActive(3);
        }
    }
    
}
