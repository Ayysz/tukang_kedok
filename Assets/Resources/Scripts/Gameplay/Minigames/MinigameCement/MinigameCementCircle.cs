using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
public class MinigameCementCircle : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image image;

    [SerializeField] private float startSize;
    [SerializeField] private float curSize;
    [SerializeField] private float sizeShrinkValue;
    private float targetSize;
    private bool isShrink;
    bool isActive;

    public Action OnDestroyed;

    public void SetData(float startSize,float sizeShrinkValue,Action action)
    {
        OnDestroyed += action;
        this.startSize = startSize;
        this.curSize = startSize;
        this.sizeShrinkValue = sizeShrinkValue;
        targetSize = startSize;
        isActive = true;
    }
    public void Shrink()
    {
        targetSize -= sizeShrinkValue;
    }
    private void Update()
    {
        if (!isActive) return;
        if (curSize > targetSize)
        {
            curSize -= Time.deltaTime;
            rectTransform.sizeDelta = new Vector2(curSize, curSize);
            if (curSize < 0)
            {
                OnDestroyed?.Invoke();
                OnDestroyed = null;
                isActive = false;
            }
        }
    }

}
