using UnityEngine;

public class ToolButton : MonoBehaviour
{
    public CraftingType CraftingType;
    [SerializeField] private GameObject guide;
    [SerializeField] private Outline outline;
    public void Clicked()
    {
        Debug.Log("Clicked Tool " + CraftingType);
        //guide.gameObject.SetActive(false);
        ToggleGuide(false);
    }
    public void ToggleGuide(bool active)
    {
       // guide.SetActive(active);
        if (active)
        {
            outline.enabled = true;
        }
        else {
            outline.enabled = false;
        }
    }
    

}
