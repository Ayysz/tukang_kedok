using UnityEngine;

public class SandingInputOld : MonoBehaviour
{
    public Camera mainCamera;
    public SandingPainter painter;
    public LayerMask sandingLayer;
    public bool isSanding;

    private void Start()
    {
        mainCamera = Camera.main;
    }
    void Update()
    {
        // Klik kiri mouse ditahan
        if (!isSanding || GameManager.Instance.isMainGame) return;
        
        if (Input.GetMouseButton(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 10f, sandingLayer))
            {
                painter.Paint(hit.textureCoord);
            }
        }
    }
}
