using System.Collections;
using UnityEngine;
using UnityEngine.UI;


    public class UIVisualHandler : MonoBehaviour
    {
        [SerializeField] private Image[] renderers;
        [SerializeField] private Material[] materials;

        public void SetUp()
        {

            materials = new Material[renderers.Length];
            foreach (Image renderer in renderers)
            {
                renderer.material = new Material(renderer.material);
            }
            for (int i = 0; i < renderers.Length; i++)
            {
                materials[i] = renderers[i].material;
            }
        }

        void Start()
        {
            SetUp();
        }
        public void SetShineColor(Color color)
        {
            foreach (Material renderer in materials)
            {
                renderer.SetColor("_ShineColor", color);
            }
        }

        public Image[] GetRenderers()
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

            while (true)
            {
                float strength = 0;
                while (strength < 1)
                {
                    strength += Time.deltaTime;
                    foreach (Material renderer in materials)
                    {
                        renderer.SetFloat("_ShineLocation", strength);
                    }
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForSeconds(2f);

            }
        }
        public void Shake()
        {
            foreach (Material renderer in materials)
            {
                renderer.EnableKeyword("SHAKEUV_ON");
            }
        }
        public void StopShake()
        {
            foreach (Material renderer in materials)
            {
                renderer.DisableKeyword("SHAKEUV_ON");
            }
        }
        Coroutine burnCoroutine;
        public void Burn()
        {
            burnCoroutine = StartCoroutine(BurnCoroutine());
        }
        public void StopBurn()
        {
            StopCoroutine(burnCoroutine);
        }
        private IEnumerator BurnCoroutine()
        {
            float fadeAmount = 0;
            while (fadeAmount < 1)
            {
                fadeAmount += Time.deltaTime;
                foreach (Material renderer in materials)
                {
                    renderer.SetFloat("_FadeAmount", fadeAmount);
                }
                yield return new WaitForEndOfFrame();
            }
        }
        private IEnumerator WavingCoroutine()
        {
            Debug.Log("WavingCoroutine");
            float strength = 25f;
            foreach (Material renderer in materials)
            {
                renderer.SetFloat("_WaveStrength", strength);
            }
            yield return new WaitForSeconds(1f);
            while (strength > 0)
            {
                strength -= Time.deltaTime * 25f;
                foreach (Material renderer in materials)
                {
                    renderer.SetFloat("_WaveStrength", strength);
                }
                yield return new WaitForEndOfFrame();
            }
            strength = 0;
            foreach (Material renderer in materials)
            {
                renderer.SetFloat("_WaveStrength", strength);
            }
        }
    }

