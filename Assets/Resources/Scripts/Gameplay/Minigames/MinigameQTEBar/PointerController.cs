using UnityEngine;

public class PointerController : MonoBehaviour
{
    public Transform PointA;
    public Transform PointB;
    public RectTransform SafeZone;
    public float MoveSpeed = 100f;
    public float MaxMoveSpeed = 500f;
    public float AccelerationInterval = 2f;

    private float direction = 1f;
    private RectTransform pointerTransform;
    private Vector3 targetPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pointerTransform = GetComponent<RectTransform>();
        targetPosition = PointB.position;
        
    }

    // Update is called once per frame
    void Update()
    {

        pointerTransform.position = Vector3.MoveTowards(pointerTransform.position, targetPosition, MoveSpeed * Time.deltaTime);
        
        bool AchivedPositionA = ReachPosition(Vector3.Distance(pointerTransform.position, PointA.position), 0.1f);
        bool AchivedPositionB = ReachPosition(Vector3.Distance(pointerTransform.position, PointB.position), 0.1f);
        if (AchivedPositionA)
        {
            targetPosition = PointB.position;
            direction = 1f;
            AddSpeed();
        }else if (AchivedPositionB)
        {
            targetPosition = PointA.position;
            direction = -1f;
            AddSpeed();
        }
        
        // Check for input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckSuccess();
        }
    }

    void AddSpeed()
    {
        MoveSpeed += AccelerationInterval;
        MoveSpeed = Mathf.Max(MoveSpeed, MaxMoveSpeed);
        Debug.Log("New Speed: " + MoveSpeed);
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
            Debug.Log("Success!");
        }
        else
        {
            Debug.Log("Fail!");
        }
    }
}
