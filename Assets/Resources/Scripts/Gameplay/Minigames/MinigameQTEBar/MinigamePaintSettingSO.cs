using UnityEngine;

[CreateAssetMenu(
    fileName = "NewMinigameSetting",
    menuName = "Minigame/MinigamePaintSetting"
)]
public class MinigamePaintSettingSO : MinigameSettingDataSO
{
    public int scoringMeasurement = 1000;
    public float scoreTarget = 0.8f;

}
