using UnityEngine;
using DG.Tweening;
using TMPro;


public enum MinigamesScoreEffectType
{
    BAD,
    GOOD,
    GREAT,
    PERFECT
}
public class MinigamesScoreEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreValueText;

    public void SetText(MinigamesScoreEffectType type)
    {
        switch (type)
        {
            case MinigamesScoreEffectType.BAD:
                scoreValueText.text = "Bad";
                scoreValueText.color = Color.red;
                scoreValueText.fontSize = 72;
                break;
            case MinigamesScoreEffectType.GOOD:
                scoreValueText.text = "Good";
                scoreValueText.color = Color.yellow;
                scoreValueText.fontSize = 72;
                break;
            case MinigamesScoreEffectType.GREAT:
                scoreValueText.text = "Great";
                scoreValueText.color = Color.green;
                scoreValueText.fontSize = 96;
                break;
            case MinigamesScoreEffectType.PERFECT:
                scoreValueText.text = "Perfect";
                scoreValueText.color = Color.aquamarine;
                scoreValueText.fontSize = 128;
                break;
        }
        PlayScoreEffectAnimation(type);
    }
    public void PlayScoreEffectAnimation(MinigamesScoreEffectType type)
    {
        // Reset scale and alpha
        scoreValueText.transform.localScale = Vector3.one;
        scoreValueText.alpha = 1f;

        // Animation parameters based on type
        float scaleAmount;
        float duration;
        float fadeDelay;
        float fadeDuration;

        switch (type)
        {
            case MinigamesScoreEffectType.BAD:
                scaleAmount = 1.1f;
                duration = 0.3f;
                fadeDelay = 0.2f;
                fadeDuration = 0.2f;
                break;
            case MinigamesScoreEffectType.GOOD:
                scaleAmount = 1.2f;
                duration = 0.35f;
                fadeDelay = 0.25f;
                fadeDuration = 0.25f;
                break;
            case MinigamesScoreEffectType.GREAT:
                scaleAmount = 1.35f;
                duration = 0.4f;
                fadeDelay = 0.3f;
                fadeDuration = 0.3f;
                break;
            case MinigamesScoreEffectType.PERFECT:
                scaleAmount = 1.5f;
                duration = 0.45f;
                fadeDelay = 0.35f;
                fadeDuration = 0.35f;
                break;
            default:
                scaleAmount = 1.1f;
                duration = 0.3f;
                fadeDelay = 0.2f;
                fadeDuration = 0.2f;
                break;
        }

        // Scale up and then back to normal
        scoreValueText.transform.DOScale(scaleAmount, duration / 2)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                scoreValueText.transform.DOScale(1f, duration / 2)
                    .SetEase(Ease.InBack);
            });

        // Fade out after delay
        scoreValueText.DOFade(0f, fadeDuration)
            .SetDelay(fadeDelay + duration)
            .SetEase(Ease.InQuad);
    }
}
