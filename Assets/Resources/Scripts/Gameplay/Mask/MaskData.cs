using UnityEngine;

[System.Serializable]
public class MaskData
{
    public int maskID;
    public string progress;
    public MaskData(int maskID, string progress)
    {
        this.maskID = maskID;
        this.progress = progress;
    }
    /*public MaskDataSO GetMaskDataSO()
    { 
    }*/

}
