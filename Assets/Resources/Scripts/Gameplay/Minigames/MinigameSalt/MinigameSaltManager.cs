using UnityEngine;
using UnityEngine.UI;

public class MinigameSaltManager : MonoBehaviour
{
    public RectTransform cursor;
    public RectTransform target;
    [SerializeField] private Image targetImage;

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
        float radius = target.sizeDelta.x * 0.5f;
        float distance = Vector2.Distance(
            cursor.position,
            target.position
        );

        if (distance <= radius)
        {
            targetImage.GetComponent<Image>().color = GetColorFromEnum(ColorType.SAFE);
        }
        else
        {
            targetImage.GetComponent<Image>().color = GetColorFromEnum(ColorType.DANGER);
        }
    }
}
