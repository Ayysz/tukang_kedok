using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewMinigameSetting",
    menuName = "Minigame/MinigameSculptSetting"
)]
public class MinigameSculptDataSO : MinigameSettingDataSO
{
    public List<TargetCircleData> targetCircleDatas;
    public float PerfectScore = 12;
    public float GreatScore = 8;
    public float GoodScore = 5;
    public float failScore = 1;
}
