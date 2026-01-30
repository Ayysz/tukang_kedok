using System;
using UnityEngine;

public class MinigamePlayController : UIManager
{
    public Action OnStartMinigame;
    public Action OnEndMinigame;

    private void OnDestroy()
    {
        OnStartMinigame = null;
        OnEndMinigame = null;
    }
    public virtual void StartMinigame()
    {
        OnStartMinigame?.Invoke();
    }
    public virtual void EndMinigame()
    {
        OnEndMinigame?.Invoke();
    }
}
