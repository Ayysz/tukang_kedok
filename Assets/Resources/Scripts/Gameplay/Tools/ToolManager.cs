using UnityEngine;
using System.Collections.Generic;

public class ToolManager : MonoBehaviour
{
    [SerializeField] private List<ToolButton> toolButton = new List<ToolButton>();

    public void SelectTool(CraftingType craftingType)
    {
        foreach (ToolButton button in toolButton)
        {
            if (button.CraftingType == craftingType)
            {
                button.ToggleGuide(true);
            }
            else
            {
                button.ToggleGuide(false);
            }
        }
    }



}
