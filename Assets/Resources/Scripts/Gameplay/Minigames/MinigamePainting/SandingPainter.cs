using UnityEngine;

public class SandingPainter : MonoBehaviour
{
    [Header("Renderer Target")]
    public Renderer targetRenderer;

    [Header("Brush Settings")]
    public int textureSize = 1024;
    public float brushSize = 20f;
    public float brushStrength = 0.05f;

    Texture2D maskTexture;
    Material runtimeMaterial;

    void Start()
    {
        // Duplikat material supaya tidak mengubah asset asli
        runtimeMaterial = targetRenderer.material;

        // Buat mask texture kosong
        maskTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        maskTexture.wrapMode = TextureWrapMode.Clamp;

        ClearMask();

        runtimeMaterial.SetTexture("_MaskTex", maskTexture);
    }

    public void Paint(Vector2 uv)
    {
        int centerX = (int)(uv.x * textureSize);
        int centerY = (int)(uv.y * textureSize);
        int radius = Mathf.RoundToInt(brushSize);

        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                int px = centerX + x;
                int py = centerY + y;

                if (px < 0 || py < 0 || px >= textureSize || py >= textureSize)
                    continue;

                float dist = Mathf.Sqrt(x * x + y * y);
                if (dist > radius) continue;

                Color current = maskTexture.GetPixel(px, py);
                float strength = brushStrength * (1f - dist / radius);

                current.r = Mathf.Clamp01(current.r + strength);
                maskTexture.SetPixel(px, py, current);
            }
        }

        maskTexture.Apply();
    }

    public void ClearMask()
    {
        Color[] colors = new Color[textureSize * textureSize];
        for (int i = 0; i < colors.Length; i++)
            colors[i] = Color.black;

        maskTexture.SetPixels(colors);
        maskTexture.Apply();
    }

    public Texture2D GetMask()
    {
        return maskTexture;
    }
}
