using UnityEngine;
using System.Collections.Generic;

public class MaskDisplay : MonoBehaviour
{
    public int progress;
    [SerializeField] List<GameObject> progressionDisplay;
    private int curProgIndex;

    public void DisplayMask(int progress)
    {
        progressionDisplay[curProgIndex].SetActive(false);
        curProgIndex = progress;
        progressionDisplay[curProgIndex].SetActive(true);
    }
}
