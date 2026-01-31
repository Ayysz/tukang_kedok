using UnityEngine;

[System.Serializable]
public class MaskData
{
    public int maskID;
    public int currentProgress;
    public int score;
    public bool isCompleted;
    public int MaxProgress()
    { 
        return GetMaskDataSO().maxProgress;
    }
    public MaskData(int maskID, int progress)
    {
        this.maskID = maskID;
        this.currentProgress = progress;
        score = 0;
        isCompleted = false;
    }
    public void AddScore(int s)
    {
        score += s;
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
