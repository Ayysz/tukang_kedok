using System;
using UnityEngine;

public class SandingProgression : MonoBehaviour
{
    public SandingPainter painter;
    [Range(0f, 1f)]
    public float finishTarget = 0.8f;
    public Action OnFinished;

    bool finished;

    void Update()
    {
        if (finished) return;

        float progress = CalculateProgress();

        if (progress >= finishTarget)
        {
            finished = true;
            OnFinished?.Invoke();
            OnFinished = null;
            Debug.Log("AMPLAS SELESAI!");
        }
    }

    float CalculateProgress()
    {
        Texture2D mask = painter.GetMask();
        Color[] pixels = mask.GetPixels();

        int whiteCount = 0;
        foreach (Color c in pixels)
        {
            if (c.r > 0.9f)
                whiteCount++;
        }

        return (float)whiteCount / pixels.Length;
    }
}
