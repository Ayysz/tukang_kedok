using UnityEngine;


public enum VisualEffectCondition
{
    None, RandomRotation
}
public class VisualEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private float _duration;
    public string EffectName;
    [SerializeField] private VisualEffectCondition visualEffectCondition;

    public virtual void Setup()
    {
        if (_particleSystem != null)
        {
            _particleSystem.Play();
            //Debug.Log($"VisualEffect {EffectName} started at position {transform.position}");
        }
        else
        {
            //Debug.LogError($"ParticleSystem is null for VisualEffect {EffectName}");
        }
        if (visualEffectCondition == VisualEffectCondition.RandomRotation)
        {
            transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        }
        Invoke(nameof(Deactivate), _duration);
    }
    public virtual void Deactivate()
    {
        if (_particleSystem != null)
        {
            _particleSystem.Stop();
        }
        CancelInvoke(nameof(Deactivate));
        ObjectPooler.EnqueueObject(this, EffectName);
    }
}

