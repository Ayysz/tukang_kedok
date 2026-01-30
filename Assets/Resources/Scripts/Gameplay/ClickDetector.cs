using UnityEngine;

public class ClickDetector : MonoBehaviour
{
    [SerializeField] private Camera cam;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // klik kiri
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Tools"))
                {
                    ToolButton toolButton = hit.collider.GetComponent<ToolButton>();
                    toolButton.Clicked();
                    ClientPeople cp = GameManager.Instance.clientManager.currentClientPeople;
                    int curProgress = cp.GetMaskData().currentProgress;
                    if (cp.GetMaskData().GetMaskDataSO().craftingTypes[curProgress] == toolButton.CraftingType)
                    {
                        Debug.Log("Painting Clicked, Add Proggress");
                        cp.AddProggress();
                    }
                  
                }
            }
        }
    }
}
