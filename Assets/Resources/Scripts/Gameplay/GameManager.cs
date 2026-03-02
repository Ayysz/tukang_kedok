using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


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
    public FinalScoreManager finalScoreManager;
    public MaskData MaskData;

    public List<MaskData> maskScoring = new List<MaskData>();
    public List<MinigamePlayController> minigameControllers = new List<MinigamePlayController>();
    [SerializeField] Animator camAnimator;
    public DialogueController mainDialogue;
    public Animator maskAnimator;
    public Transform maskParent;
    public MaskDisplay maskDisplay;
    public bool isMainGame;

    public ToolManager toolManager;
    public GameObject starBackground;

    [SerializeField] private AudioClip cameraChangeClip;

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
        toolManager.SelectTool(cp.GetMaskData().GetMaskDataSO().craftingTypes[cp.GetMaskData().currentProgress]);
    }
    public void CompletedAMask(Action action)
    {
        //nanti di refactor jangan lupa
        ResultManager.Instance.Show(clientManager.currentClientPeople.GetData().clientName, clientManager.currentClientPeople.GetMaskData().score, clientManager.currentClientPeople.GetMaskData(), action);


        if (clientManager.currentClientPeople.GetMaskData().IsSuccess())
        {
            clientManager.currentClientPeople.State2();
        }
        else
        {
            clientManager.currentClientPeople.State3();
        }
        starBackground.gameObject.SetActive(true);
        maskAnimator.SetTrigger("Mask Done");
        camAnimator.SetTrigger("ChangeCustomer");

        StartCoroutine(AfterAMaskDelat());
        taskAtasController.Hide();

    }

    public void CameraChange()
    {
        camAnimator.SetTrigger("Change");
        AudioManager.Instance.PlaySfx(cameraChangeClip);

    }
    public void CameraFinalScore()
    {
        camAnimator.SetTrigger("Final");
        starBackground.gameObject.SetActive(true);
        AudioManager.Instance.PlaySfx(cameraChangeClip);
        finalScoreManager.ShowFinalScore();


    }
    private IEnumerator AfterAMaskDelat()
    {
        yield return new WaitForSeconds(2f);
        foreach (Transform t in maskParent)
        {
            Destroy(t.gameObject);
        }

    }
    public void AfterOkay()
    {
        camAnimator.SetTrigger("Change");
        AudioManager.Instance.PlaySfx(cameraChangeClip);



    }
    public virtual void SpawnMask(MaskDisplay display, int progress)
    {
        MaskDisplay md = Instantiate(display, maskParent.transform.position, maskParent.transform.rotation, maskParent);
        md.DisplayMask(progress);
        maskDisplay = md;
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
        starBackground.gameObject.SetActive(false);
        clientManager.ClientPeopleOut(ClientDoneAddProggress);
    }
    public void AddScore(MaskData data)
    {
        maskScoring.Add(data);
    }
    public void ClientDoneAddProggress()
    {
        proggress++;
        if (proggress >= 3)
        {
            Debug.Log("Win Game");
            StartCoroutine(WinDelay());
        }
        else
        {
            clientManager.DestroyCurrentPeople();
            CurrentClientData = ClientDatabase.Instance.GetClient(proggress);
            clientManager.SpawnClientPeople(CurrentClientData);
        }
    }
    private IEnumerator WinDelay()
    {
        yield return new WaitForSeconds(2f);
        // All Score Display!
        CameraFinalScore();

        //SceneManager.LoadScene("Main Menu");
    }
    public void SetPlayerDialogue(DialogueSO dialogue, Action action)
    {
        playerDialogue.SetDialogue(dialogue, action);
    }
    public void GoToMinigame(CraftingType craftingType)
    {
        if (isMainGame)
        {
            if (craftingType == CraftingType.Sculpting)
            {
                isMainGame = false;
                MaskData md = clientManager.currentClientPeople.GetMaskData();
                MinigameSculptDataSO setting = md.GetMaskDataSO().minigameSettingDatas[md.currentProgress] as MinigameSculptDataSO;
                minigameControllers[1].OnEndMinigame += MinigameClear;
                minigameControllers[1].StartMinigame(setting);



            }
            else if (craftingType == CraftingType.MakingHole)
            {
                isMainGame = false;

                MaskData md = clientManager.currentClientPeople.GetMaskData();
                MinigameHoleSettingSO holeSetting = md.GetMaskDataSO().minigameSettingDatas[md.currentProgress] as MinigameHoleSettingSO;
                minigameControllers[0].OnEndMinigame += MinigameClear;
                minigameControllers[0].StartMinigame(holeSetting);


            }
            else if (craftingType == CraftingType.Painting)
            {
                isMainGame = false;

                MaskData md = clientManager.currentClientPeople.GetMaskData();
                MinigamePaintSettingSO paintSetting = md.GetMaskDataSO().minigameSettingDatas[md.currentProgress] as MinigamePaintSettingSO;
                minigameControllers[2].OnEndMinigame += MinigameClear;
                minigameControllers[2].StartMinigame(paintSetting);

            }
            else if (craftingType == CraftingType.Cement)
            {
                isMainGame = false;
                MaskData md = clientManager.currentClientPeople.GetMaskData();
                MinigameCementSettingSO holeSetting = md.GetMaskDataSO().minigameSettingDatas[md.currentProgress] as MinigameCementSettingSO;
                minigameControllers[3].OnEndMinigame += MinigameClear;
                minigameControllers[3].StartMinigame(holeSetting);
            }
        }

    }
    public void MinigameClear()
    {
        ClientPeople cp = GameManager.Instance.clientManager.currentClientPeople;
        int curProgress = cp.GetMaskData().currentProgress;
        cp.AddProggress();
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
