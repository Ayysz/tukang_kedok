using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(
    fileName = "NewMinigameSetting",
    menuName = "Minigame/MinigameCementSetting"
)]
public class MinigameCementSettingSO : MinigameSettingDataSO
{
    public List<CementCircleData> cementCircleDatas = new List<CementCircleData>();
    public int perfectScore;
    public int greatScore;
    public int goodScore;
    public int badScore;
    public float perfectTime;
    public float greatTime;
    public float goodTime;

}
