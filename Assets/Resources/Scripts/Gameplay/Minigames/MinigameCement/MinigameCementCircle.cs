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
    private float speed = 10f;
    bool isActive;

    public Action<MinigameCementCircle> OnDestroyed;

    [SerializeField] private AudioClip clip;

    public void SetData(float startSize,float sizeShrinkValue,Action<MinigameCementCircle> action)
    {
        OnDestroyed += action;
        this.startSize = startSize;
        this.curSize = startSize;
        this.sizeShrinkValue = sizeShrinkValue;
        targetSize = startSize;
        isActive = true;
        speed = sizeShrinkValue*5f;
        rectTransform.sizeDelta = new Vector2(curSize, curSize);
    }
    public void Shrink()
    {
        targetSize -= sizeShrinkValue;
        AudioManager.Instance.PlaySfx(clip);
    }
    private void Update()
    {
        if (!isActive) return;
        if (curSize > targetSize)
        {
            curSize -= Time.deltaTime *speed;
            rectTransform.sizeDelta = new Vector2(curSize, curSize);
            if (curSize <= 0)
            {
                OnDestroyed?.Invoke(this);
                OnDestroyed = null;
                isActive = false;
            }
        }
    }

}
