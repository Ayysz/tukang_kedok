using System;
using UnityEngine;

public class MinigamePlayController : UIManager
{
    public Action OnStartMinigame;
    public Action OnEndMinigame;

    public bool isPlaying = false;
    public MinigameSettingDataSO dataSetting;

    private void OnDestroy()
    {
        OnStartMinigame = null;
        OnEndMinigame = null;
    }
    public virtual void StartMinigame(MinigameSettingDataSO dataSetting)
    {
        Show();
        this.dataSetting = dataSetting;
        isPlaying = true;
        SetSettings();
        OnStartMinigame?.Invoke();
        OnStartMinigame = null;
    }
    public virtual void SetSettings()
    {
        Debug.Log("SetSettings");
    }
    public virtual void EndMinigame()
    {
        Hide();
        Debug.Log("EndMinigame");
        isPlaying = false;
        OnEndMinigame?.Invoke();
        OnEndMinigame = null;
    }
}
