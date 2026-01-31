using System;
using UnityEngine;
using UnityEngine.UI;

public class MinigameSculptCircle : MonoBehaviour
{
    [SerializeField] private float targetSize = 50;
    [SerializeField] private float perfectThreshold = 5f;
    [SerializeField] private float GreatThreshold = 10f;
    [SerializeField] private float shrinkingSpeed;

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image image;

    [SerializeField] private bool isShrinking;
    [SerializeField] float curSize = 50;

    public Action OnPerfect;
    public Action OnGreat;
    public Action OnGood;
    public Action OnFailed;

    public MinigameSculptTarget target;
    private void Update()
    {
        if (isShrinking)
        {
            //curSize -= Time.deltaTime*shrinkingSpeed;
            // rectTransform.sizeDelta = new Vector2(curSize, curSize);
            //if (curSize <= 10f)
            //{
            //    Failed();
            //}
            rectTransform.sizeDelta -= new Vector2(shrinkingSpeed * Time.deltaTime, shrinkingSpeed * Time.deltaTime);
            curSize = rectTransform.sizeDelta.x;
            if (curSize <= 30f)
            {
                Failed();
            }
        }
    }
    public void SetAction(Action Fail,Action Good,Action Great,Action Perfect)
    {
        OnFailed += Fail;
        OnGood += Good;
        OnGreat += Great;
        OnPerfect += Perfect;
    }
    private void OnDestroy()
    {
        OnFailed = null;
        OnGood = null;
        OnGreat = null;
        OnPerfect = null;
    }
    public void Set(float Speed, float Size,float GreatThreshold,float PerfectThreshold,float targetSize)
    {
        shrinkingSpeed = Speed;
        this.GreatThreshold = GreatThreshold;
        this.perfectThreshold = PerfectThreshold;
        rectTransform.sizeDelta = new Vector2(Size, Size);
        this.targetSize = targetSize;
        curSize = Size;
        isShrinking = true;
    }
    public void Failed()
    {
        OnFailed?.Invoke();
        OnFailed = null;
        isShrinking = false;
        image.color = Color.red;
        
    }
    public void Perfect()
    {
        OnPerfect?.Invoke();
        OnPerfect = null;
        isShrinking = false;
        image.color = Color.green;
    }
    public void Great()
    {
        OnGreat?.Invoke();
        OnGreat = null;
        isShrinking = false;
        image.color = Color.blue;
    }
    public void Good()
    {
        OnGood?.Invoke();
        OnGood = null;
        isShrinking = false;
        image.color = Color.yellow;
    }
    public void Clicked()
    {
        float size = curSize - targetSize;
        if (size > perfectThreshold || size < -perfectThreshold)
        {
            Perfect();

        } else
        {
            if (size > GreatThreshold || size < -GreatThreshold)
            {
                Great();
            }
            else
            {
                Good();
            }
        }
       
    }
}
