using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private int perfectScore;
    [SerializeField] private int greatScore;
    [SerializeField] private int goodScore;
    [SerializeField] private int badScore;
    [SerializeField] private float perfectTime;
    [SerializeField] private float greatTime;
    [SerializeField] private float goodTime;
    [SerializeField] private TextMeshProUGUI performText;


    private int circleLeft;

    float currentTime;


    public override void StartMinigame(MinigameSettingDataSO dataSetting)
    {
        base.StartMinigame(dataSetting);

    }
    private void Update()
    {
        if (isPlaying)
        {
            currentTime += Time.deltaTime;
        }
    }
    public override void GameStart()
    {
        base.GameStart();
        currentTime = 0;
        toolsAnimator.gameObject.SetActive(true);
        toolsAnimator.SetTrigger("Cungkil Start Cement");
        score = 0;
        scoreText.text = ClientManager.Instance.currentClientPeople.GetMaskData().score.ToString();
        performText.text = "";
        circleLeft = cementCircleList.Count;
        for (int i = 0; i < cementCircleList.Count; i++)
        {
            MinigameCementCircle cementCircle = Instantiate(circlePrefab, parent);
            cementCircle.SetData(cementCircleList[i].size, cementCircleList[i].sizeShrinkValue, CircleDestroyed);
            cementCircle.GetComponent<RectTransform>().anchoredPosition = cementCircleList[i].position;
        }
        if (isSkip)
        {
            isPlaying = false;
            EndGame();
        }
    }
    public void CircleDestroyed(MinigameCementCircle circle)
    {
       
        circleLeft--;
        toolsAnimator.SetTrigger("Cungkil Cement");
        maskAnimator.SetTrigger("Mask Cement");
        int tscore = ClientManager.Instance.currentClientPeople.GetMaskData().score;
        if (circleLeft <= 0)
        {
            isPlaying = false;
            StartCoroutine(EndGame());
        }
        if (currentTime <= perfectTime)
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.GlobalAudioList.perfectSfx);
            score += perfectScore;
            UpdateScore(score,perfectScore);
            ImpactEffect(MinigamesScoreEffectType.PERFECT, circle.GetComponent<RectTransform>());
        }
        else if (currentTime <= greatTime)
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.GlobalAudioList.greatSfx);
            score += greatScore;
            UpdateScore(score,greatScore);
            ImpactEffect(MinigamesScoreEffectType.GREAT, circle.GetComponent<RectTransform>());
        }
        else if (currentTime <= goodTime)
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.GlobalAudioList.goodSfx);
            score += goodScore;
            UpdateScore(score,goodScore);
            ImpactEffect(MinigamesScoreEffectType.GOOD, circle.GetComponent<RectTransform>());
        }
        else
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.GlobalAudioList.badSfx);
            score += badScore;
            UpdateScore(score,badScore);
            ImpactEffect(MinigamesScoreEffectType.BAD, circle.GetComponent<RectTransform>());
        }
    }
    private void ImpactEffect(MinigamesScoreEffectType type, RectTransform pos)
    {
        VisualEffect effect = null;
        if (type == MinigamesScoreEffectType.PERFECT)
        {
            effect = ObjectPooler.DequeueObject<VisualEffect>("PerfectEffect");
        }
        else if (type == MinigamesScoreEffectType.GREAT)
        {
            effect = ObjectPooler.DequeueObject<VisualEffect>("GreatEffect");
        }
        else if (type == MinigamesScoreEffectType.GOOD)
        {
            effect = ObjectPooler.DequeueObject<VisualEffect>("GoodEffect");
        }
        else if (type == MinigamesScoreEffectType.BAD)
        {
            effect = ObjectPooler.DequeueObject<VisualEffect>("BadEffect");
        }
        effect.Setup();
        // Convert UI position to world position so the effect is visible to the camera
        Canvas canvas = GetComponentInParent<Canvas>();
        Camera uiCamera = null;
        if (canvas != null)
        {
            uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
        }

        Vector3 worldPosition;
        // Use the center of the rectTransform in screen space
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(uiCamera, pos.position);

        // Convert screen point to world point in the main camera
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Vector3 effectWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, mainCamera.nearClipPlane + 1f));
            effect.transform.position = effectWorldPos;
            PerformText(type, pos);
        }
        else
        {
            // fallback: just use rectTransform.position
            effect.transform.position = pos.position;
            PerformText(type, pos);
        }
    }
    public void PerformText(MinigamesScoreEffectType type, RectTransform pos)
    {
        performText.rectTransform.position = pos.position; // Move perform text to the position of the circle
        if (type == MinigamesScoreEffectType.PERFECT)
        {
            performText.color = Color.aquamarine;
            performText.text = "PERFECT";
            performText.fontSize = 125;
            performText.transform.DOScale(1.5f, 0.5f).OnComplete(() => performText.transform.DOScale(1f, 0.5f));
        }
        else if (type == MinigamesScoreEffectType.GREAT)
        {
            performText.color = Color.green;
            performText.text = "GREAT";
            performText.fontSize = 115;

            performText.transform.DOScale(1.4f, 0.5f).OnComplete(() => performText.transform.DOScale(1f, 0.5f));

        }
        else if (type == MinigamesScoreEffectType.GOOD)
        {
            performText.color = Color.yellow;
            performText.text = "GOOD";
            performText.fontSize = 96;
            performText.transform.DOScale(1.25f, 0.5f).OnComplete(() => performText.transform.DOScale(1f, 0.5f));

        }
        else if (type == MinigamesScoreEffectType.BAD)
        {
            performText.color = Color.red;
            performText.text = "BAD";
            performText.fontSize = 80;

            performText.transform.DOScale(1.1f, 0.5f).OnComplete(() => performText.transform.DOScale(1f, 0.5f));

        }
    }
    public IEnumerator EndGame()
    {
        EndMinigameScene();
        yield return new WaitForSeconds(2);
        Hide();
        toolsAnimator.gameObject.SetActive(false);
        EndMinigame();
    }
    public override void SetSettings()
    {
        base.SetSettings();
        MinigameCementSettingSO setting = dataSetting as MinigameCementSettingSO;
        cementCircleList = setting.cementCircleDatas;
        perfectScore = setting.perfectScore;
        greatScore = setting.greatScore;
        goodScore = setting.goodScore;
        badScore = setting.badScore;
        perfectTime = setting.perfectTime;
        greatTime = setting.greatTime;
        goodTime = setting.goodTime;
    }

}
