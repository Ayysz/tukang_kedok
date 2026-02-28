using UnityEngine;

[CreateAssetMenu(
    fileName = "NewMinigameSetting",
    menuName = "Minigame/MiniameHoleSetting"
)]
public class MinigameHoleSettingSO : MinigameSettingDataSO
{
    public int maxCombo = 3;
    public float MoveSpeed = 100f;
    public float MaxMoveSpeed = 500f;
    public float AccelerationInterval = 2f;
    public float MinimumSafeZoneWidth = 50f;
    public float MaximumSafeZoneWidth = 200f;

    public int perfectScore = 100;
    public int greatScore = 70;
    public int goodScore = 40;
    public int failScore = 10;

}
