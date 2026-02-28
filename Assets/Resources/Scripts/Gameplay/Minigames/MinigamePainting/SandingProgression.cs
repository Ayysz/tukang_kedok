using System;
using UnityEngine;

public class SandingProgression : MonoBehaviour
{
    public SandingPainter painter;
    [Range(0f, 1f)]
    public float finishTarget = 0.8f;

    public Action OnFinished;
    public Action<float> OnSanding;
    [SerializeField] bool finished;

    float proggress = -1;

    void Update()
    {
        if (finished) return;

        float progress = CalculateProgress();
        if (proggress != progress)
        {
            proggress = progress;
            OnSanding?.Invoke(progress);
            if (progress >= finishTarget)
            {
                OnSanding?.Invoke(progress); // Pastikan progress mencapai 100% saat selesai
                OnFinished?.Invoke();
                finished = true;
                Debug.Log("AMPLAS SELESAI!");
                OnFinished = null;
                OnSanding = null;
            }
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
