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

    [Header("Movement Settings")]
    public float MoveSpeed = 100f;
    public float MaxMoveSpeed = 500f;
    public float AccelerationInterval = 2f;
    public float MinimumSafeZoneWidth = 50f;
    public float MaximumSafeZoneWidth= 200f;

    // Threshold used to determine if pointer "reached" a point
    [SerializeField]
    private float ReachThreshold = 0.1f;
    private Camera MainCamera;

    //private float direction = 1f;
    private RectTransform pointerTransform;
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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pointerTransform = GetComponent<RectTransform>();
        targetPosition = PointB.position;
        RandomSafeZone();
        UtilizeSafeZoneWidth();
        if (!MainCamera)
        {
            MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }
        
        MainCamera.enabled = true;
        ShakeCamera.enabled = false;

        if (shakeOnStart)
        {
            StartCoroutine(PlayCameraShake());
        }

        UpdateTextCombo();
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
        }
        else
        {
            SafeZone.sizeDelta = new Vector2(Random.Range(MinimumSafeZoneWidth, MaximumSafeZoneWidth), SafeZone.sizeDelta.y);
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
            isStopped = true;
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
            if (Input.GetKeyDown(KeyCode.Space))
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

    void AddSpeedBar()
    {
        MoveSpeed += AccelerationInterval;
        // Clamp MoveSpeed so it does not exceed MaxMoveSpeed
        MoveSpeed = Mathf.Min(MoveSpeed, MaxMoveSpeed);
    }

    void RestartGame()
    {
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
        bool IsSuccess = RectTransformUtility.RectangleContainsScreenPoint(SafeZone, pointerTransform.position, null);
        if (IsSuccess)
        {
            if (CurrentCombo < MaxCombo)
            {
                RandomSafeZone();
                CurrentCombo += 1;
                UpdateTextCombo();
            }
        }
        else
        {
            ShakeTheCamera();
        }
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
        if (!ShakeCamera.enabled)
        {
            StartCoroutine(PlayCameraShake());
        }
    }

}


