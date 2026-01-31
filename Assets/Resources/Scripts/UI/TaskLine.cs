using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskLine : MonoBehaviour
{
    [SerializeField] private Sprite[] taskTypeIcon;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI taskText;
    [SerializeField] private GameObject clearedObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetTask(CraftingType type,bool cleared)
    {
        Sprite iconSprite = taskTypeIcon[(int)type];
        icon .sprite = iconSprite;
        taskText.text = type.ToString();
        clearedObject.SetActive(cleared);
    }

}
