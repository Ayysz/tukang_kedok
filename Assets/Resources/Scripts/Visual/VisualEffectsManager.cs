using UnityEngine;


public class VisualEffectsManager : MonoBehaviour
{
    [SerializeField] private VisualEffect[] _visualEffects;

    private void Start()
    {
        //Debug.Log($"VisualEffectsManager: Setting up {_visualEffects.Length} visual effects");
        ObjectPooler.Startup();
        foreach (var visualEffect in _visualEffects)
        {
            if (visualEffect != null)
            {
                //Debug.Log($"Setting up pool for: {visualEffect.EffectName}");
                ObjectPooler.SetupPool(visualEffect, 4, visualEffect.EffectName);
            }
            else
            {
                Debug.LogError("VisualEffect is null in VisualEffectsManager");
            }
        }
    }
}

