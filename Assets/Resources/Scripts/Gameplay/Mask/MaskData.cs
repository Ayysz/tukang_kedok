using UnityEngine;

[System.Serializable]
public class MaskData
{
    public int maskID;
    public int currentProgress;
    public int MaxProgress()
    { 
        return GetMaskDataSO().maxProgress;
    }
    public MaskData(int maskID, int progress)
    {
        this.maskID = maskID;
        this.currentProgress = progress;
    }
    public MaskDataSO GetMaskDataSO()
    {
        return MaskDatabase.Instance.GetMask(maskID);
    }

}

public enum CraftingType
{ 
    Sculpting,
    MakingHole,
    Painting,
}
