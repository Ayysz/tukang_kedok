using UnityEngine;

public class BillboardScript : MonoBehaviour
{
    Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
