using UnityEngine;

public class RotateWithRightClick : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotateSpeed = 0.3f;

    [Header("Axis Control")]
    public bool rotateX = true;
    public bool rotateY = true;
    public bool rotateZ = false;

    Vector3 lastMousePosition;
    public bool isCanRotate;

    void Update()
    {
        // Saat klik kanan pertama kali ditekan
        if (!isCanRotate || GameManager.Instance.isMainGame) return;
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePosition = Input.mousePosition;
        }

        // Saat klik kanan ditahan
        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;

            float rotX = rotateX ? -delta.y * rotateSpeed : 0f;
            float rotY = rotateY ? delta.x * rotateSpeed : 0f;
            float rotZ = rotateZ ? delta.x * rotateSpeed : 0f;

            transform.Rotate(rotX, rotY, rotZ, Space.World);

            lastMousePosition = Input.mousePosition;
        }
    }

}
