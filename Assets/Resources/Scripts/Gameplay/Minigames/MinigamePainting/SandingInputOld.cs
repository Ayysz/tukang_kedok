using UnityEngine;
using UnityEngine.UI;

public class SandingInputOld : MonoBehaviour
{
    public Camera mainCamera;
    public SandingPainter painter;
    public LayerMask sandingLayer;
    public bool isSanding;

    [SerializeField] private AudioClip amplasSound;
    [SerializeField] private float soundDelay = 1f;
    [SerializeField] private int effectDelay = 0;

    RectTransform icon;
    [SerializeField] private ParticleSystem dustyEffect;

    public void SetIcon(RectTransform data)
    {
        icon = data;

    }

    private void Start()
    {
        mainCamera = Camera.main;
    }
    public void SetDustyEffect(ParticleSystem val)
    {
        dustyEffect = val;
    }
    void Update()
    {
        // Klik kiri mouse ditahan
        if (!isSanding || GameManager.Instance.isMainGame) return;

        if (Input.GetMouseButton(0))
        {
            soundDelay -= Time.deltaTime;
            if (soundDelay <= 0)
            {
                AudioManager.Instance.PlaySfx(amplasSound);
                soundDelay = 1f;
                effectDelay++;
                if (effectDelay > 3)
                {
                    effectDelay = 0;
                    ImpactEffect(MinigamesScoreEffectType.PERFECT, icon);
                    AudioManager.Instance.PlaySfx(AudioManager.Instance.GlobalAudioList.perfectSfx);
                }
            }
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 10f, sandingLayer))
            {
                painter.Paint(hit.textureCoord);
                if (dustyEffect != null && !dustyEffect.isPlaying)
                {
                    dustyEffect.Play();
                }
                dustyEffect.transform.position = hit.point;
            }
        }
        if (Input.GetMouseButtonUp(0))
        { 
            dustyEffect.Stop();
        }
    }
    private void ImpactEffect(MinigamesScoreEffectType type, RectTransform pos)
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
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(uiCamera, pos.position);

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
            effect.transform.position = pos.position;
        }
    }
}
