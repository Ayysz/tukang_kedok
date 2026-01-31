using System.Collections.Generic;
using UnityEngine;

public class CameraChangeController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private List<Camera> camList = new List<Camera>();
    GameObject currentCamera;
    public void ActivateCamera(int id)
    {
        camList[id].gameObject.SetActive(true);
        currentCamera = camList[id].gameObject;
    }
    public void DisactivateCamera(int id)
    {

        camList[id].gameObject.SetActive(false);
    }
    public void ChangeCameraActive(int id)
    {
        if (currentCamera != null)
        {
            currentCamera.SetActive(false);
        }
        ActivateCamera(id);
    }
}   
