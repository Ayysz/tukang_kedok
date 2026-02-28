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
    public Action OnClicked;

    public MinigameSculptTarget target;
    [SerializeField] private MinigamesScoreEffect minigamesScoreEffect;
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
    public void SetAction(Action Fail, Action Good, Action Great, Action Perfect)
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
        OnClicked = null;
    }
    public void SetOnClicked(Action Clicked)
    {
        OnClicked += Clicked;
    }
    public void Set(float Speed, float Size, float GreatThreshold, float PerfectThreshold, float targetSize)
    {
        shrinkingSpeed = Speed;
        this.GreatThreshold = GreatThreshold;
        this.perfectThreshold = PerfectThreshold;
        rectTransform.sizeDelta = new Vector2(Size, Size);
        this.targetSize = targetSize;
        curSize = Size;
        isShrinking = true;
        minigamesScoreEffect.gameObject.SetActive(false);
    }
    public void Failed()
    {
        OnFailed?.Invoke();
        OnFailed = null;
        isShrinking = false;
        image.color = Color.red;
        image.raycastTarget = false;
        image.enabled = false;
        minigamesScoreEffect.gameObject.SetActive(true);
        minigamesScoreEffect.SetText(MinigamesScoreEffectType.BAD);
        OnClicked?.Invoke();
    }
    public void Perfect()
    {
        OnPerfect?.Invoke();
        OnPerfect = null;
        isShrinking = false;
        image.color = Color.green;
        image.enabled = false;

        minigamesScoreEffect.gameObject.SetActive(true);
        minigamesScoreEffect.SetText(MinigamesScoreEffectType.PERFECT);
    }
    public void Great()
    {
        OnGreat?.Invoke();
        OnGreat = null;
        isShrinking = false;
        image.color = Color.blue;
        image.enabled = false;

        minigamesScoreEffect.gameObject.SetActive(true);
        minigamesScoreEffect.SetText(MinigamesScoreEffectType.GREAT);
    }
    public void Good()
    {
        OnGood?.Invoke();
        OnGood = null;
        isShrinking = false;
        image.color = Color.yellow;
        image.enabled = false;

        minigamesScoreEffect.gameObject.SetActive(true);
        minigamesScoreEffect.SetText(MinigamesScoreEffectType.GOOD);
    }
    public void Clicked()
    {
        if (!isShrinking) return;
        //float diff = Mathf.Abs(curSize - targetSize);

        if (curSize > targetSize)
        {
            Perfect();
            ImpactEffect(MinigamesScoreEffectType.PERFECT);
        }
        else { 
            float diff = targetSize - curSize;
            Debug.Log(diff + " " + perfectThreshold);
            if (diff <= perfectThreshold)
            {
                Great();
                ImpactEffect(MinigamesScoreEffectType.GREAT);
            }
            else if (diff <= GreatThreshold)
            {
                Good();
                ImpactEffect(MinigamesScoreEffectType.GOOD);
            }
            else {
                Failed();
                ImpactEffect(MinigamesScoreEffectType.BAD);
            }
        }

        image.raycastTarget = false;
        OnClicked?.Invoke();
        
    }
    private void ImpactEffect(MinigamesScoreEffectType type)
    {
        VisualEffect effect = null;
        if (type == MinigamesScoreEffectType.PERFECT)
        {
            effect = ObjectPooler.DequeueObject<VisualEffect>("PerfectEffect");
        }
        else if (type == MinigamesScoreEffectType.GREAT)
        {
            effect = ObjectPooler.DequeueObject<VisualEffect>("GreatEffect");
        }
        else if (type == MinigamesScoreEffectType.GOOD)
        {
            effect = ObjectPooler.DequeueObject<VisualEffect>("GoodEffect");
        }
        else if (type == MinigamesScoreEffectType.BAD)
        {
            effect = ObjectPooler.DequeueObject<VisualEffect>("BadEffect");
        }
        effect.Setup();
        // Convert UI position to world position so the effect is visible to the camera
        Canvas canvas = GetComponentInParent<Canvas>();
        Camera uiCamera = null;
        if (canvas != null)
        {
            uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
        }

        Vector3 worldPosition;
        // Use the center of the rectTransform in screen space
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(uiCamera, rectTransform.position);

        // Convert screen point to world point in the main camera
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Vector3 effectWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, mainCamera.nearClipPlane + 1f));
            effect.transform.position = effectWorldPos;
        }
        else
        {
            // fallback: just use rectTransform.position
            effect.transform.position = rectTransform.position;
        }
    }
}
