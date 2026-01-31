using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MinigameSaltManager : MinigamePlayController
{
    public RectTransform cursor;
    public RectTransform target;
    [SerializeField] private Image targetImage;
    [SerializeField] private Slider progressBar;
    [SerializeField] private float MaxprogressBar = 100;
    [SerializeField] private float MultiplerAddProggerBar = 10f;
    [SerializeField] private float MultiplerSubProggerBar = 15f;
    [SerializeField] private MinigameSaltBorder border;
    
    private bool isWin = false;

    private void Start()
    {
        if (progressBar == null)
        {
            progressBar = GetComponentInChildren<Slider>();
        }
        if (progressBar != null)
        {
            progressBar.interactable = false;
            progressBar.maxValue = MaxprogressBar;
        }
    }
    public enum ColorType
    {
        SAFE,
        DANGER
    }

    public ColorType colorChoice;

    public Color GetColorType(ColorType type)
    {
        return GetColorFromEnum(type);
    }

    public static Color GetColorFromEnum(ColorType type)
    {
        switch (type)
        {
            case ColorType.SAFE:
                return Color.green;
            case ColorType.DANGER:
                return Color.red;
            default:
                return Color.white;
        }
    }

    void Update()
    {
        if (isWin) return;

        if (progressBar.value >= MaxprogressBar)
        {
            isWin = true;
            border.SetWin();
        }
        changeColorCircle();
    }

    void changeColorCircle()
    {
        float radius = target.sizeDelta.x * 0.5f;
        float distance = Vector2.Distance(
            cursor.position,
            target.position
        );

        if (distance <= radius)
        {
            targetImage.GetComponent<Image>().color = GetColorFromEnum(ColorType.SAFE);
            progressBar.value += MultiplerAddProggerBar * Time.deltaTime;
        }
        else
        {
            targetImage.GetComponent<Image>().color = GetColorFromEnum(ColorType.DANGER);
            progressBar.value -= MultiplerSubProggerBar * Time.deltaTime;
        }
    }
}
