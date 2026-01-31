using UnityEngine;

[System.Serializable]
public class MaskData
{
    public int maskID;
    public int currentProgress;
    public bool isCompleted;
    public int MaxProgress()
    { 
        return GetMaskDataSO().maxProgress;
    }
    public MaskData(int maskID, int progress)
    {
        this.maskID = maskID;
        this.currentProgress = progress;
        isCompleted = false;
    }
    public MaskDataSO GetMaskDataSO()
    {
        return MaskDatabase.Instance.GetMask(maskID);
    }
    public void AddProggress()
    {
        currentProgress++;
        if (currentProgress >= MaxProgress())
        {
            isCompleted = true;
        }
    }

}

public enum CraftingType
{ 
    Sculpting,
    MakingHole,
    Painting,
    Cement
}
