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
                    Debug.Log("Klik object interactable: " + hit.collider.name);
                    ToolButton toolButton = hit.collider.GetComponent<ToolButton>();
                }
            }
        }
    }
}
