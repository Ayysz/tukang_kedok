using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public class MinigamePaintingController : MinigamePlayController
{
    [SerializeField] private SandingPainter sandingPainter;
    [SerializeField] private SandingInputOld sandingInputOld;
    [SerializeField] private SandingProgression sandingProgression;
    [SerializeField] private Camera cam;
    [SerializeField] private Image icon;
    [SerializeField] private int scoringMeasurement = 1000;
    [SerializeField] private float scoreTarget = 0.8f;
    [SerializeField] private ParticleSystem hitEffectDusty;



    public override void StartMinigame(MinigameSettingDataSO dataSetting)
    {
        base.StartMinigame(dataSetting);
       
        GameObject go = GameManager.Instance.maskDisplay.GetProgression(GameManager.Instance.clientManager.currentClientPeople.GetMaskData().currentProgress);
        sandingPainter = go.GetComponent<SandingPainter>();
        sandingInputOld = go.GetComponent<SandingInputOld>();
        sandingProgression = go.GetComponent<SandingProgression>();
        sandingInputOld.SetIcon(icon.rectTransform);
        RotateWithRightClick rotateWithRightClick = go.GetComponent<RotateWithRightClick>();
        Debug.Log("StartMinigame Sanding");
        scoreText.text = ClientManager.Instance.currentClientPeople.GetMaskData().score.ToString();

        if (sandingInputOld != null)
        {
            sandingInputOld.isSanding = true;
            sandingInputOld.mainCamera = cam;
        }
        if (sandingProgression != null)
        {
            sandingProgression.OnFinished += Finish;
            sandingProgression.OnSanding += (float progress) =>
            {
                score = Mathf.RoundToInt(progress * scoringMeasurement);
                Debug.Log("Sanding : " + " Score: " + score);
                int tscore = ClientManager.Instance.currentClientPeople.GetMaskData().score;
                scoreText.text = (score + tscore).ToString();
                hitEffectDusty.transform.position = sandingPainter.GetHitEffectPosition();
            };
        }
        if (isSkip)
        {
            isPlaying = false;
            Finish();
        }
    }
    public override void EndMinigame()
    {
        base.EndMinigame();

    }
    private void Update()
    {
        if (isPlaying)
        {
            icon.transform.position = Input.mousePosition;
        }
    }
    public override void SpawnMask(MaskDisplay display, int progress)
    {
        base.SpawnMask(display, progress);
        GameObject go = maskDisplay.GetProgression(progress);
        sandingPainter = go.GetComponent<SandingPainter>();
        sandingInputOld = go.GetComponent<SandingInputOld>();
        sandingProgression = go.GetComponent<SandingProgression>();
        Debug.Log("Spawn Mask Sanding");
        RotateWithRightClick rotateWithRightClick = go.GetComponent<RotateWithRightClick>();
        if (sandingInputOld != null)
        {
            sandingInputOld.isSanding = true;
            sandingInputOld.mainCamera = cam;
        }
        if (sandingProgression != null)
        {
            sandingProgression.OnFinished += Finish;
            sandingProgression.OnSanding += (float progress) =>
            {
                score = Mathf.RoundToInt(progress * scoringMeasurement);
                Debug.Log("Sanding : " + " Score: " + score);
                scoreText.text = score.ToString();
            };
        }
        StartCoroutine(delayMainCamera());
    }
    private IEnumerator delayMainCamera()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Set MainCamera");
        if (sandingInputOld != null)
        {
            sandingInputOld.mainCamera = cam;
        }
    }
    public void Finish()
    {
        Debug.Log("Minigame Painting Finished");
        AudioManager.Instance.PlaySfx(AudioManager.Instance.GlobalAudioList.perfectSfx);
        ImpactEffect(MinigamesScoreEffectType.PERFECT, icon.rectTransform);
        StartCoroutine(EndGame());
    }
    public IEnumerator EndGame()
    {
        sandingProgression.OnFinished -= Finish;
        score = Mathf.RoundToInt(scoreTarget * scoringMeasurement);
        EndMinigameScene();
        int tscore = ClientManager.Instance.currentClientPeople.GetMaskData().score;
        UpdateScore(score);
        yield return new WaitForSeconds(2);
        EndMinigame();
        Hide();
    }

    public override void SetSettings()
    {
        MinigamePaintSettingSO minigamePaintSettingSO = dataSetting as MinigamePaintSettingSO;
        scoringMeasurement = minigamePaintSettingSO.scoringMeasurement;
        scoreTarget = minigamePaintSettingSO.scoreTarget;

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
        }
        else
        {
            // fallback: just use rectTransform.position
            effect.transform.position = pos.position;
        }
    }
}
