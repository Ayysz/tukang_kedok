using System;
using System.Collections;
using UnityEngine;

public class MinigamePlayController : UIManager
{
    public Action OnStartMinigame;
    public Action OnEndMinigame;

    public bool isPlaying = false;
    public MinigameSettingDataSO dataSetting;
    public Transform maskParent;
    protected MaskDisplay maskDisplay;
    public MaskDisplay GetMaskDisplay()
    {
        return maskDisplay;
    }
    private void OnDestroy()
    {
        OnStartMinigame = null;
        OnEndMinigame = null;
    }
    public virtual void SpawnMask(MaskDisplay display,int progress)
    {
        MaskDisplay md = Instantiate(display, maskParent.transform.position, maskParent.transform.rotation, maskParent);
        md.DisplayMask(progress);
        maskDisplay = md;
    }
    public virtual void StartMinigame(MinigameSettingDataSO dataSetting)
    {
        Show();
        ClientPeople cp = GameManager.Instance.clientManager.currentClientPeople;
        SpawnMask(cp.GetMaskData().GetMaskDataSO().maskDisplayPrefab, cp.GetMaskData().currentProgress);
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
        ClearMask();
       
    }
    public virtual void EndMinigameScene()
    {
        StartCoroutine(EndMinigameSceneDelay());
    }
    private IEnumerator EndMinigameSceneDelay()
    {
        ClearMask();
        yield return new WaitForSeconds(0.5f);
        ClientPeople cp = GameManager.Instance.clientManager.currentClientPeople;
        if (!cp.GetMaskData().isCompleted)
        { 
            SpawnMask(cp.GetMaskData().GetMaskDataSO().maskDisplayPrefab, cp.GetMaskData().currentProgress+1);
        
        }

    }
    public void ClearMask()
    {
        foreach (Transform t in maskParent)
        {
            Destroy(t.gameObject);
        }
    }
}
