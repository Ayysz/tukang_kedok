using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class TargetCircleData
{
    public float delay;
    public SculptCircleData data;
}
[System.Serializable]
public class SculptCircleData
{
    public Vector2 position;
    public float delay = 0;
    public float Size = 50;
    public float perfectThreshold = 5f;
    public float GreatThreshold = 10f;
    public float shrinkingSpeed;
    public float targetSize;
}

public class MinigameSculptController : MinigamePlayController
{
    public List<TargetCircleData> targetCircleDatas = new List<TargetCircleData>();
    public MinigameSculptTarget prefab;
    [SerializeField] private Transform parent;
    public float areaX;
    public float areaY;

    public float PerfectScore = 12;
    public float GreatScore = 8;
    public float GoodScore = 5;
    public float failScore = 1;
    public int circleCount;
    private int curCircleCount;
    [SerializeField]private float totalScore = 0;

    [SerializeField] private AudioClip pop;
    [SerializeField] private AudioClip popFail;




    public override void StartMinigame(MinigameSettingDataSO dataSetting)
    {
        base.StartMinigame(dataSetting);
        toolsAnimator.gameObject.SetActive(true);
        toolsAnimator.SetTrigger("Hammer Start");
       
        circleCount = targetCircleDatas.Count;
        totalScore = 0;
        curCircleCount = 0;
        for (int i = 0; i < targetCircleDatas.Count; i++)
        {
            StartCoroutine(SpawnTargetCircle(targetCircleDatas[i].delay, targetCircleDatas[i]));
        }
        if (isSkip)
        {
            isPlaying = false;
            WinGame();
        }

    }
    public void AddCircleCount()
    {
        if (isPlaying)
        {
            curCircleCount++;
            if (curCircleCount >= circleCount)
            {
                WinGame();
            }
        }
    }
    public void WinGame()
    {
        Debug.Log("Sculpt Win");
        isPlaying = false;
        
        EndMinigameScene();
        foreach (Transform t in parent)
        {
            Destroy(t.gameObject);
        }
        StartCoroutine(WinDelay());
    }
    public IEnumerator WinDelay()
    {
        yield return new WaitForSeconds(2f);
        toolsAnimator.gameObject.SetActive(false);
        EndMinigame();
    }
    public void Fail()
    {
        AudioManager.Instance.PlaySfx(popFail);
        AddCircleCount();
        totalScore += failScore;
        Debug.Log("Fail");
        toolsAnimator.SetTrigger("Hammer Slow");
        maskAnimator.SetTrigger("Mask Hammer Slow");

    }
    public void Good()
    {
        AudioManager.Instance.PlaySfx(popFail);
        AddCircleCount();
        totalScore += GoodScore;
        Debug.Log("Good");
        toolsAnimator.SetTrigger("Hammer Slow");
        maskAnimator.SetTrigger("Mask Hammer Slow");


    }
    public void Great()
    {
        AudioManager.Instance.PlaySfx(pop);
        AddCircleCount();
        totalScore += GreatScore;
        Debug.Log("Great");
        toolsAnimator.SetTrigger("Hammer Slow");
        maskAnimator.SetTrigger("Mask Hammer Slow");

    }
    public void Perfect()
    {
        AudioManager.Instance.PlaySfx(pop);
        AddCircleCount();
        totalScore += PerfectScore;
        Debug.Log("Perfect");
        toolsAnimator.SetTrigger("Hammer Hard");
        maskAnimator.SetTrigger("Mask Hammer Hard");
    }
    public IEnumerator SpawnTargetCircle(float delay,TargetCircleData tcd)
    {
        yield return new WaitForSeconds(delay);
        MinigameSculptTarget target = Instantiate(prefab, parent);
        float x = 0;
        float y = 0;
        if (tcd.data.position.x == 0 && tcd.data.position.y == 0)
        {
            x = Random.Range(-areaX, areaX);
            y = Random.Range(-areaY, areaY);
        }
        else 
        { 
            x = tcd.data.position.x;
            y = tcd.data.position.y;
        }
           
        target.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
        target.SetAction(Fail, Perfect, Great, Good);
        target.Set(tcd.data.delay, tcd.data.shrinkingSpeed, tcd.data.Size, tcd.data.GreatThreshold, tcd.data.perfectThreshold,tcd.data.targetSize);

    }
    public override void SetSettings()
    {
        base.SetSettings();
        MinigameSculptDataSO sculptSetting = dataSetting as MinigameSculptDataSO;
        targetCircleDatas = sculptSetting.targetCircleDatas;
        failScore = sculptSetting.failScore;
        GoodScore = sculptSetting.GoodScore;
        GreatScore = sculptSetting.GreatScore;
        PerfectScore = sculptSetting.PerfectScore;
        // dataSetting

    }
}
