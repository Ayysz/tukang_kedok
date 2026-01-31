using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;
    [SerializeField] private List<GameObject> meshDisplays;

    [Header("Drag Settings")]
    [SerializeField] private bool returnIfNotPlaced = true;

    private Vector3 startPosition;
    private Vector3 dragOffset;
    private bool isDragging;
    private bool isPlaced;
    private Transform originalParent;

    private int activeIndex;
    Plane dragPlane;

    void Awake()
    {
        if (cam == null)
            cam = Camera.main;

        startPosition = transform.position;
        originalParent = transform.parent;


        PickRandomMesh();
        UpdateMeshDisplay();
    }

    //void Update()
    //{
    //    if (isDragging)
    //    {
    //        MoveWithMouse();
    //    }

    //    // Debug reset
    //    if (Input.GetKeyDown(KeyCode.R))
    //    {
    //        ResetLeaf();
    //    }
    //}

    //void OnMouseDown()
    //{
    //    if (isPlaced) return;

    //    isDragging = true;
    //    dragOffset = transform.position - GetMouseWorldPosition();
    //}

    //void OnMouseUp()
    //{
    //    isDragging = false;

    //    if (!isPlaced && returnIfNotPlaced)
    //    {
    //        ResetPositionOnly();
    //    }
    //}

    void Update()
    {
        HandleInput();

        if (isDragging)
        {
            MoveWithMouse();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetLeaf();
        }
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryStartDrag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopDrag();
        }
    }

    void TryStartDrag()
    {
        //Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        //if (Physics.Raycast(ray, out RaycastHit hit))
        //{
        //    // PENTING: cek apakah yang kena adalah LEAF INI
        //    if (hit.transform == transform)
        //    {
        //        if (isPlaced) return;

        //        isDragging = true;
        //        dragOffset = transform.position - GetMouseWorldPosition();
        //    }
        //}

        isDragging = true;

        dragPlane = new Plane(
            cam.transform.forward * -1,
            transform.position
        );

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            dragOffset = transform.position - hitPoint;
        }
    }

    void StopDrag()
    {
        isDragging = false;

        if (!isPlaced && returnIfNotPlaced)
        {
            ResetPositionOnly();
        }
    }

    // =========================
    // Movement
    // =========================

    void MoveWithMouse()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            transform.position = hitPoint + dragOffset;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = cam.WorldToScreenPoint(transform.position).z;
        return cam.ScreenToWorldPoint(mouse);
    }

    // =========================
    // Public API (dipanggil Slot)
    // =========================

    public void CancelDragAndPlace(Transform slot)
    {
        isDragging = false;
        isPlaced = true;

        transform.SetParent(slot);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void ResetLeaf()
    {
        isPlaced = false;
        PickRandomMesh();
        UpdateMeshDisplay();
        ResetPositionOnly();
    }

    // =========================
    // Internal helpers
    // =========================

    void ResetPositionOnly()
    {
        isDragging = false;
        dragOffset = Vector3.zero;

        transform.SetParent(originalParent);
        transform.position = startPosition;
    }

    void PickRandomMesh()
    {
        activeIndex = Random.Range(0, meshDisplays.Count);
    }

    void UpdateMeshDisplay()
    {
        for (int i = 0; i < meshDisplays.Count; i++)
        {
            meshDisplays[i].SetActive(i == activeIndex);
        }
    }

    // =========================
    // Trigger detection
    // =========================

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Tools")) return;

        Debug.Log("Leaf kena Tools");

        isDragging = false;

        ResetLeaf();
    }

}
