using UnityEngine;

public class ToolButton : MonoBehaviour
{
    public CraftingType CraftingType;
    public void Clicked()
    {
        Debug.Log("Clicked Tool " + CraftingType);
    }
    

}
