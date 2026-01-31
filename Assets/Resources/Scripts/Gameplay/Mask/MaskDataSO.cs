using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewMask",
    menuName = "Game/MaskData"
)]
public class MaskDataSO : ScriptableObject
{
    public int id;
    public int maxProgress;
    public List<CraftingType> craftingTypes = new List<CraftingType>();
    public List<MinigameSettingDataSO> minigameSettingDatas = new List<MinigameSettingDataSO>();
    public MaskDisplay maskDisplayPrefab;
}
