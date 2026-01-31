using UnityEngine;

public class MinigameSaltBorder : MonoBehaviour
{
    [Header("References")]
    public RectTransform arena;
    public RectTransform target;

    [Header("Movement")]
    public float moveInterval = 1.5f;
    public float moveSpeed = 5f;

    [Header("Shrink")]
    public float shrinkSpeed = 30f;
    public float minShrinkRange = -5f;
    public float maxShrinkRange = 10f;
    public float minSize = 40f;
    public float maxSize = 100f;

    Vector2 targetPos;
    float timer;

    void Start()
    {
        PickRandomPosition();
    }

    void Update()
    {
        Move();
        Shrink();
    }

    void Move()
    {
        timer += Time.deltaTime;
        if (timer >= moveInterval)
        {
            PickRandomPosition();
            timer = 0f;
        }

        target.anchoredPosition = Vector2.Lerp(
            target.anchoredPosition,
            targetPos,
            Time.deltaTime * moveSpeed
        );
    }

    void Shrink()
    {
        float currentSize = target.sizeDelta.x;

        float shrink = shrinkSpeed * Time.deltaTime;
        float noise = Random.Range(-2f, 2f);

        float newSize = currentSize - shrink + noise;
        newSize = Mathf.Clamp(newSize, minSize, maxSize);

        target.sizeDelta = Vector2.one * newSize;
    }

    void PickRandomPosition()
    {
        Vector2 halfArena = arena.rect.size / 2f;
        Vector2 halfTarget = target.sizeDelta / 2f;

        float x = Random.Range(
            -halfArena.x + halfTarget.x,
             halfArena.x - halfTarget.x
        );

        float y = Random.Range(
            -halfArena.y + halfTarget.y,
             halfArena.y - halfTarget.y
        );

        targetPos = new Vector2(x, y);
    }
}
