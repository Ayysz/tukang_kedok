using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

//[CreateAssetMenu(fileName = "qte_ar", menuName = "Scriptable Objects/qte_ar")]
public class MiniGameQTEBarController : MinigamePlayController
{
    [Header("Bar Settings")]
    public Transform PointA;
    public Transform PointB;
    public RectTransform SafeZone;
    public Camera ShakeCamera;
    public float MaxCombo = 3;
    [SerializeField] private TextMeshProUGUI ComboText;
    [SerializeField] private TextMeshProUGUI performText;
    [SerializeField] private float GreatValue = 0f;
    [SerializeField] private float GoodValue = 100f;

    [Header("Movement Settings")]
    public float MoveSpeed = 100f;
    public float MaxMoveSpeed = 500f;
    public float AccelerationInterval = 2f;
    public float MinimumSafeZoneWidth = 50f;
    public float MaximumSafeZoneWidth = 200f;

    // Threshold used to determine if pointer "reached" a point
    [SerializeField]
    private float ReachThreshold = 0.1f;
    [SerializeField] private Camera MainCamera;

    //private float direction = 1f;
    [SerializeField] private RectTransform pointerTransform;
    private Vector3 targetPosition;

    // Flag to ensure AddSpeed runs only once per reached point
    private bool speedAddedThisStop = false;
    private bool isStopped = false;

    // Fields \\
    [Tooltip("If a CameraShake is not assigned, all cameras with a CameraShake component are targeted.")]
    public CameraShake camShake;
    public bool shakeOnStart = true;

    [Header("Shake Settings")]
    public float duration = 0.5f;
    public float posMagnitude = 0.25f;
    public float dirMagnitude = 0.25f;
    public float rotMagnitude = 0.5f;
    public float noiseFrequency = 5f;

    [Header("3D Settings")]
    public bool falloff = true;
    public float minFalloffDist = 20f;
    public float maxFalloffDist = 70f;

    float CurrentCombo = 0;
    bool isShaking = false;

    [SerializeField] private AudioClip sfx;
    MinigameHoleSettingSO holeSettingSO;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {

    }
    public override void StartMinigame(MinigameSettingDataSO dataSetting)
    {
        base.StartMinigame(dataSetting);

    }
    public override void GameStart()
    {
        base.GameStart();
        performText.text = "";
        RestartGame();
        targetPosition = PointB.position;
        RandomSafeZone();
        UtilizeSafeZoneWidth();
        toolsAnimator.gameObject.SetActive(true);
        toolsAnimator.SetTrigger("Cungkil Start Pahat");
        UpdateScore(0);
        MainCamera.enabled = true;
        // ShakeCamera.enabled = false;

        if (shakeOnStart)
        {
            // StartCoroutine(PlayCameraShake());
        }

        UpdateTextCombo();
        if (isSkip)

        {

            EndGame();
        }
    }
    public override void SetSettings()
    {
        MinigameHoleSettingSO data = dataSetting as MinigameHoleSettingSO;
        holeSettingSO = data;
        MoveSpeed = data.MoveSpeed;
        MaxMoveSpeed = data.MaxMoveSpeed;
        AccelerationInterval = data.AccelerationInterval;
        MinimumSafeZoneWidth = data.MinimumSafeZoneWidth;
        MaximumSafeZoneWidth = data.MaximumSafeZoneWidth;
        MaxCombo = data.maxCombo;
    }


    public void Shake()
    {
        CameraShake.ShakeProperties shake = new CameraShake.ShakeProperties(
            duration,
            posMagnitude,
            dirMagnitude,
            rotMagnitude,
            noiseFrequency,
            transform.position,
            falloff,
            minFalloffDist,
            maxFalloffDist);

        if (camShake)
        {
            camShake.Shake(shake);
        }
        else
        {
            CameraShake.ShakeAll(shake);
        }
    }


    void UtilizeSafeZoneWidth()
    {
        if (MinimumSafeZoneWidth < pointerTransform.sizeDelta.x)
        {
            SafeZone.sizeDelta = new Vector2(pointerTransform.sizeDelta.x, SafeZone.sizeDelta.y);
            float length = SafeZone.rect.xMax - SafeZone.rect.xMin;

            GreatValue = length * 0.05f;
            Debug.Log("Great Value : " + GreatValue);
        }
        else
        {
            SafeZone.sizeDelta = new Vector2(Random.Range(MinimumSafeZoneWidth, MaximumSafeZoneWidth), SafeZone.sizeDelta.y);
            float length = SafeZone.rect.xMax - SafeZone.rect.xMin;

            GreatValue = length * 0.05f;
            Debug.Log("Great Value : " + GreatValue);

        }
    }

    void RandomSafeZone()
    {
        SafeZone.position = new Vector3(Random.Range(PointA.position.x, PointB.position.x), pointerTransform.position.y, pointerTransform.position.z);
        UtilizeSafeZoneWidth();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying) return;
        if (!isStopped)
        {
            pointerTransform.position = Vector3.MoveTowards(pointerTransform.position, targetPosition, MoveSpeed * Time.deltaTime);

            float distanceToA = Vector3.Distance(pointerTransform.position, PointA.position);
            float distanceToB = Vector3.Distance(pointerTransform.position, PointB.position);

            bool AchivedPositionA = ReachPosition(distanceToA, ReachThreshold);
            bool AchivedPositionB = ReachPosition(distanceToB, ReachThreshold);

            if (AchivedPositionA && !speedAddedThisStop)
            {
                targetPosition = PointB.position;
                //direction = 1f;
                AddSpeedBar();
                speedAddedThisStop = true;
            }
            else if (AchivedPositionB && !speedAddedThisStop)
            {
                targetPosition = PointA.position;
                //direction = -1f;
                AddSpeedBar();
                speedAddedThisStop = true;
            }

            // Reset the flag when pointer moves away from both points so AddSpeed can run again next time a point is reached
            if (distanceToA > ReachThreshold && distanceToB > ReachThreshold)
            {
                speedAddedThisStop = false;
            }
        }

        GetInput();
        if (CurrentCombo >= MaxCombo)
        {
            StartCoroutine(EndGame());
            Debug.Log("Success End");
            isStopped = true;
            isPlaying = false;
        }
    }

    void UpdateTextCombo()
    {
        ComboText.text = "Max Combo: " + CurrentCombo.ToString();
    }

    void GetInput()
    {
        if (!isStopped)
        {
            // Check for input
            if (Input.GetMouseButtonDown(0))
            {
                CheckSuccess();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RandomSafeZone();
            RestartGame();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RestartGame();
        }
    }
    public void Perform(MinigamesScoreEffectType type)
    {
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

    void AddSpeedBar()
    {
        MoveSpeed += AccelerationInterval;
        // Clamp MoveSpeed so it does not exceed MaxMoveSpeed
        MoveSpeed = Mathf.Min(MoveSpeed, MaxMoveSpeed);
    }

    void RestartGame()
    {
        CurrentCombo = 0;
        if (isStopped)
        {
            isStopped = false;
        }
    }

    bool ReachPosition(float distance, float limit)
    {
        return distance < limit;
    }

    void CheckSuccess()
    {
        // Get pointer position in screen space
        Vector2 pointerScreenPos = RectTransformUtility.WorldToScreenPoint(null, pointerTransform.position);

        // Get SafeZone rect in screen space
        Vector2 safeZoneScreenPos = RectTransformUtility.WorldToScreenPoint(null, SafeZone.position);
        float safeZoneWidth = SafeZone.rect.width * SafeZone.lossyScale.x;
        float safeZoneHeight = SafeZone.rect.height * SafeZone.lossyScale.y;
        Rect safeZoneRect = new Rect(
            safeZoneScreenPos.x - safeZoneWidth * 0.5f,
            safeZoneScreenPos.y - safeZoneHeight * 0.5f,
            safeZoneWidth,
            safeZoneHeight
        );

        bool isInside = safeZoneRect.Contains(pointerScreenPos);

        float distanceToEdge = 0f;
        if (!isInside)
        {
            // Calculate the closest point on the safezone rect to the pointer
            float clampedX = Mathf.Clamp(pointerScreenPos.x, safeZoneRect.xMin, safeZoneRect.xMax);
            float clampedY = Mathf.Clamp(pointerScreenPos.y, safeZoneRect.yMin, safeZoneRect.yMax);
            Vector2 closestPoint = new Vector2(clampedX, clampedY);
            distanceToEdge = Vector2.Distance(pointerScreenPos, closestPoint);
        }
        else
        {
            // If inside, distance to center of the safezone
            Vector2 safeZoneCenter = new Vector2(safeZoneRect.center.x, safeZoneRect.center.y);
            distanceToEdge = Vector2.Distance(pointerScreenPos, safeZoneCenter);

        }

        // Judge result
        string result = "Bad";
        if (isInside)
        {
            if (distanceToEdge <= GreatValue)
            {
                result = "Perfect";
                score += holeSettingSO.perfectScore;
                UpdateScore(score);
                ImpactEffect(MinigamesScoreEffectType.PERFECT);
                Perform(MinigamesScoreEffectType.PERFECT);
                AudioManager.Instance.PlaySfx(AudioManager.Instance.GlobalAudioList.perfectSfx);
            }
            else if (distanceToEdge <= GoodValue)
            {

                result = "Great";
                score += holeSettingSO.greatScore;
                UpdateScore(score);
                ImpactEffect(MinigamesScoreEffectType.GREAT);
                Perform(MinigamesScoreEffectType.GREAT);

                AudioManager.Instance.PlaySfx(AudioManager.Instance.GlobalAudioList.greatSfx);
            }
            else
            {
                result = "Great";
                score += holeSettingSO.greatScore;
                UpdateScore(score);
                ImpactEffect(MinigamesScoreEffectType.GREAT);
                Perform(MinigamesScoreEffectType.GREAT);

                AudioManager.Instance.PlaySfx(AudioManager.Instance.GlobalAudioList.greatSfx);

            }
        }
        else
        {
            if (distanceToEdge <= GoodValue)
            {
                result = "Near Miss";
                score += holeSettingSO.goodScore;
                UpdateScore(score);
                ImpactEffect(MinigamesScoreEffectType.GOOD);
                Perform(MinigamesScoreEffectType.GOOD);

                AudioManager.Instance.PlaySfx(AudioManager.Instance.GlobalAudioList.goodSfx);


            }
            else
            {
                result = "Bad";
                score += holeSettingSO.failScore;
                UpdateScore(score);
                ImpactEffect(MinigamesScoreEffectType.BAD);
                Perform(MinigamesScoreEffectType.BAD);

                AudioManager.Instance.PlaySfx(AudioManager.Instance.GlobalAudioList.badSfx);
            }
        }

        Debug.Log($"Result: {result}, Distance: {distanceToEdge}");

        if (isInside)
        {
            if (CurrentCombo < MaxCombo)
            {
                toolsAnimator.SetTrigger("Cungkil Pahat");
                maskAnimator.SetTrigger("Mask Cungkil");
                RandomSafeZone();
                CurrentCombo += 1;

                UpdateTextCombo();
                AudioManager.Instance.PlaySfx(sfx);
            }
        }
        else
        {
            //ShakeTheCamera();
            if (CurrentCombo < MaxCombo)
            {
                toolsAnimator.SetTrigger("Cungkil Pahat");
                maskAnimator.SetTrigger("Mask Cungkil");
                RandomSafeZone();
                CurrentCombo += 1;
                UpdateTextCombo();
                AudioManager.Instance.PlaySfx(sfx);
                Debug.Log("Failed");
            }
        }
    }
    private void ImpactEffect(MinigamesScoreEffectType type)
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
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(uiCamera, pointerTransform.position);

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
            effect.transform.position = pointerTransform.position;
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

    IEnumerator PlayCameraShake()
    {
        if (isShaking) yield break;
        isShaking = true;

        MainCamera.enabled = false;
        ShakeCamera.enabled = true;

        Shake();

        yield return new WaitForSeconds(duration);

        ShakeCamera.enabled = false;
        MainCamera.enabled = true;

        isShaking = false;
    }
    void ShakeTheCamera()
    {
        /*if (!ShakeCamera.enabled)
        {
            StartCoroutine(PlayCameraShake());
        }*/
    }

}


