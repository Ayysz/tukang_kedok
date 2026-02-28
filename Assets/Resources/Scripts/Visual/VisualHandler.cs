using UnityEngine;
using System.Collections;
using System.Collections.Generic;


    public class VisualHandler : MonoBehaviour
    {
        [SerializeField] private Renderer[] renderers;
        [SerializeField] private Material[] materials;

        [SerializeField] private Transform targetTransform;
        public void SetUp()
        {
            if (targetTransform != null)
            {
                renderers = targetTransform.GetComponentsInChildren<Renderer>();
                materials = new Material[renderers.Length];
                for (int i = 0; i < renderers.Length; i++)
                {
                    materials[i] = renderers[i].material;
                }
            }
            else
            {
                renderers = GetComponentsInChildren<Renderer>();
                materials = new Material[renderers.Length];
                for (int i = 0; i < renderers.Length; i++)
                {
                    materials[i] = renderers[i].material;
                }
            }
        }

        void Start()
        {
            SetUp();
        }

        public Renderer[] GetRenderers()
        {
            return renderers;
        }
        public void Waving()
        {
            Debug.Log("Waving");
            wavingCoroutine = StartCoroutine(WavingCoroutine());
        }
        public void Shining()
        {
            Debug.Log("Shining");
            shiningCoroutine = StartCoroutine(ShiningCoroutine());
        }
        Coroutine shiningCoroutine;
        Coroutine wavingCoroutine;
        private IEnumerator ShiningCoroutine()
        {
            Debug.Log("ShiningCoroutine");
            float strength = 0;
            while (strength < 1)
            {
                strength += Time.deltaTime ;
                foreach (Renderer renderer in renderers)
                {
                    renderer.material.SetFloat("_ShineLocation", strength);
                }
                yield return new WaitForEndOfFrame();
            }

        }
        private IEnumerator WavingCoroutine()
        {
            Debug.Log("WavingCoroutine");
            float strength = 25f;
            foreach (Renderer renderer in renderers)
            {
                renderer.material.SetFloat("_WaveStrength", strength);
            }
            yield return new WaitForSeconds(1f);
            while (strength > 0)
            {
                strength -= Time.deltaTime * 25f;
                foreach (Renderer renderer in renderers)
                {
                    renderer.material.SetFloat("_WaveStrength", strength);
                }
                yield return new WaitForEndOfFrame();
            }
            strength = 0;
            foreach (Renderer renderer in renderers)
            {
                renderer.material.SetFloat("_WaveStrength", strength);
            }
        }
    }

