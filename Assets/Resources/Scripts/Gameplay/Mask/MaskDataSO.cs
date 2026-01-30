using UnityEngine;

[CreateAssetMenu(
    fileName = "NewMask",
    menuName = "Game/MaskData"
)]
public class MaskDataSO : ScriptableObject
{
    public int id;
    public int maxProgress;
}
