using UnityEngine;

public class ToolButton : MonoBehaviour
{
    public CraftingType CraftingType;
    [SerializeField] private GameObject guide;
    public void Clicked()
    {
        Debug.Log("Clicked Tool " + CraftingType);
        guide.gameObject.SetActive(false);
    }
    public void ToggleGuide(bool active)
    {
        guide.SetActive(active);
    }
    

}
