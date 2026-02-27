using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class MinigamePlayController : UIManager
{
    public Action OnStartMinigame;
    public Action OnEndMinigame;

    public bool isPlaying = false;
    public MinigameSettingDataSO dataSetting;
    public Transform maskParent;
    protected MaskDisplay maskDisplay;
    public Animator toolsAnimator;
    public Animator maskAnimator;
    [SerializeField] protected TextMeshProUGUI scoreText;
    [SerializeField] private string title;
    [TextArea]
    [SerializeField] private string instruction;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI instructionText;

    private Coroutine scoreTweenCoroutine;

    public int score;
    public bool isSkip;
    public MaskDisplay GetMaskDisplay()
    {
        return maskDisplay;
    }


    private void OnDestroy()
    {
        OnStartMinigame = null;
        OnEndMinigame = null;
    }
    public virtual void SpawnMask(MaskDisplay display, int progress)
    {
        MaskDisplay md = Instantiate(display, maskParent.transform.position, maskParent.transform.rotation, maskParent);
        md.DisplayMask(progress);
        maskDisplay = md;
    }
    public virtual void StartMinigame(MinigameSettingDataSO dataSetting)
    {
        Show();
        this.dataSetting = dataSetting;
        SetSettings();
        ShowInstruction();
      
    }
    public virtual void ShowInstruction()
    { 
        startPanel.SetActive(true);
        gamePanel.SetActive(false);
        titleText.text = title;
        instructionText.text = instruction;
       // StartCoroutine(InstructionIenumerator());
    }
    public virtual void GameStart()
    {
        isPlaying = true;
        OnStartMinigame?.Invoke();
        OnStartMinigame = null;
        startPanel.SetActive(false);
        gamePanel.SetActive(true);
    }
    private IEnumerator InstructionIenumerator()
    {
      
        yield return new WaitForSeconds(2f);
        GameStart();
    }
    public virtual void SetSettings()
    {
        Debug.Log("SetSettings");
    }
    public virtual void EndMinigame()
    {
        Hide();
        GameManager.Instance.clientManager.currentClientPeople.GetMaskData().AddScore(score);
        score = 0;
        Debug.Log("EndMinigame");
        isPlaying = false;
        OnEndMinigame?.Invoke();
        OnEndMinigame = null;
        //ClearMask();

    }
    public void UpdateScore(int score)
    {
        if (scoreTweenCoroutine != null)
        {
            StopCoroutine(scoreTweenCoroutine);
        }
        scoreTweenCoroutine = StartCoroutine(TweenScore(this.score, score, 0.5f));
    }

    private IEnumerator TweenScore(int from, int to, float duration)
    {
        float elapsed = 0f;
        int startScore = from;
        int endScore = to;
        Vector3 originalScale = scoreText.transform.localScale;
        Vector3 popupScale = originalScale * 1.3f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            int currentScore = Mathf.RoundToInt(Mathf.Lerp(startScore, endScore, t));
            scoreText.text = currentScore.ToString();

            // Tween scale for popup effect
            float scaleT = Mathf.Sin(t * Mathf.PI); // Ease in-out popup
            scoreText.transform.localScale = Vector3.Lerp(originalScale, popupScale, scaleT);

            yield return null;
        }
        this.score = to;
        scoreText.text = to.ToString();
        scoreText.transform.localScale = originalScale;
        scoreTweenCoroutine = null;
    }
    public virtual void EndMinigameScene()
    {
        StartCoroutine(EndMinigameSceneDelay());
    }
    private IEnumerator EndMinigameSceneDelay()
    {
        ClearMask();
        yield return new WaitForSeconds(0.1f);
        ClientPeople cp = GameManager.Instance.clientManager.currentClientPeople;
        if (!cp.GetMaskData().isCompleted)
        {
            SpawnMask(cp.GetMaskData().GetMaskDataSO().maskDisplayPrefab, cp.GetMaskData().currentProgress + 1);
            GameManager.Instance.maskDisplay = maskDisplay;
        }
        else
        {
            SpawnMask(cp.GetMaskData().GetMaskDataSO().maskDisplayPrefab, cp.GetMaskData().currentProgress);
            GameManager.Instance.maskDisplay = maskDisplay;

            maskAnimator.SetTrigger("Mask Done");
            yield return new WaitForSeconds(2f);
            ClearMask();
        }

    }
    public void ClearMask()
    {
        foreach (Transform t in maskParent)
        {
            Destroy(t.gameObject);
        }
    }
}
