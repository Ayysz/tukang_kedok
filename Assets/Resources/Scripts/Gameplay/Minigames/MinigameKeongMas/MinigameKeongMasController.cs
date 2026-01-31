using UnityEngine;

public class MinigameKeongMasController : MinigamePlayController
{
    [Header("Setup")]
    public Camera mainCamera;
    public LayerMask maskLayer;

    GameObject currentLeaf;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryPlaceLeaf();
        }
    }

    void TryPlaceLeaf()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, maskLayer))
        {
            if (currentLeaf != null)
            {
                currentLeaf.transform.position = hit.point;
                
                currentLeaf.transform.rotation = Quaternion.LookRotation(hit.normal);

                currentLeaf = null;
            }
        }
    }

    public void SetLeaf(GameObject leaf)
    {
        currentLeaf = leaf;
    }
}
