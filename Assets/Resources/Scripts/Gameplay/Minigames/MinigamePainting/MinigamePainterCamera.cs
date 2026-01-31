using UnityEngine;

public class MinigamePainterCamera : MonoBehaviour
{
    public Camera cam;

    public static MinigamePainterCamera Instance;

    private void Awake()
    {
        Instance = this;
    }
}
