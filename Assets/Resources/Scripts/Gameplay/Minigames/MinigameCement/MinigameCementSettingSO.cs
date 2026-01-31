using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(
    fileName = "NewMinigameSetting",
    menuName = "Minigame/MinigameCementSetting"
)]
public class MinigameCementSettingSO : MinigameSettingDataSO
{
    public List<CementCircleData> cementCircleDatas = new List<CementCircleData>();
    
}
