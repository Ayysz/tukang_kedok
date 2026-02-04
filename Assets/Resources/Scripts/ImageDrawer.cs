using UnityEngine;

public class ImageDrawer : MonoBehaviour
{
    private Texture2D Image;
    public int res = 1024;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.Image = new Texture2D(res, res, TextureFormat.RGBA32, false);
        GetComponent<Renderer>().material.SetTexture("_BaseMap",this.Image);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 256; i++)
        { 
            var x = Random.Range(0, res);
            var y = Random.Range(0, res);
            this.Image.SetPixel(x, y, Random.ColorHSV(0f,1f,0f,1f,0f,1f,0f,1f));
        }
        this.Image.Apply();
    }
}
