using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CementCircleData
{
    public float sizeShrinkValue;
    public float size;
    public Vector2 position;
}

public class MinigameCementController : MinigamePlayController
{
    [SerializeField] private Transform parent;
    [SerializeField] private MinigameCementCircle circlePrefab;
    [SerializeField] List<CementCircleData> cementCircleList = new List<CementCircleData>();

    [Header("Settings")]
    [SerializeField] private float startSize = 200f;

    private int circleLeft;

    public override void StartMinigame(MinigameSettingDataSO dataSetting)
    {
        base.StartMinigame(dataSetting);
        toolsAnimator.gameObject.SetActive(true);
        toolsAnimator.SetTrigger("Cungkil Start Cement");

        circleLeft = cementCircleList.Count;
        for (int i = 0; i < cementCircleList.Count; i++)
        {
            MinigameCementCircle cementCircle = Instantiate(circlePrefab, parent);
            cementCircle.SetData(cementCircleList[i].size, cementCircleList[i].sizeShrinkValue,CircleDestroyed);
            cementCircle.GetComponent<RectTransform>().anchoredPosition = cementCircleList[i].position;
        }
        if (isSkip)
        {
            isPlaying = false;
            EndGame();
        }
    }
    public void CircleDestroyed()
    {
        score += 100;
        circleLeft--;
        toolsAnimator.SetTrigger("Cungkil Cement");
        maskAnimator.SetTrigger("Mask Cement");
        if (circleLeft <= 0)
        {
            isPlaying = false;
            StartCoroutine(EndGame());
            
        }
    }
    public IEnumerator EndGame()
    {
        EndMinigameScene();
        Hide();
        yield return new WaitForSeconds(2);
        toolsAnimator.gameObject.SetActive(false);
        EndMinigame();
    }
    public override void SetSettings()
    {
        base.SetSettings();
        MinigameCementSettingSO setting = dataSetting as MinigameCementSettingSO;
        cementCircleList = setting.cementCircleDatas;
    }
    
}
