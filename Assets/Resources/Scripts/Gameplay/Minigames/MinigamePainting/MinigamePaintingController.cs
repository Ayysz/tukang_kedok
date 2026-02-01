using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public class MinigamePaintingController : MinigamePlayController
{
    [SerializeField] private SandingPainter sandingPainter;
    [SerializeField] private SandingInputOld sandingInputOld;
    [SerializeField] private SandingProgression sandingProgression;
    [SerializeField] private Camera cam;
    [SerializeField] private Image icon;

    public override void StartMinigame(MinigameSettingDataSO dataSetting)
    {
        base.StartMinigame(dataSetting);
        GameObject go = GameManager.Instance.maskDisplay.GetProgression(GameManager.Instance.clientManager.currentClientPeople.GetMaskData().currentProgress);
        sandingPainter = go.GetComponent<SandingPainter>();
        sandingInputOld = go.GetComponent<SandingInputOld>();
        sandingProgression = go.GetComponent<SandingProgression>();
        RotateWithRightClick rotateWithRightClick = go.GetComponent<RotateWithRightClick>();
        sandingInputOld.isSanding = true;
        sandingInputOld.mainCamera = cam;
        sandingProgression.OnFinished += Finish;
    }
    public override void EndMinigame()
    {
        base.EndMinigame();
       
    }
    private void Update()
    {
        if (isPlaying)
        { 
            icon.transform.position = Input.mousePosition;
        }
    }
    public override void SpawnMask(MaskDisplay display, int progress)
    {
        base.SpawnMask(display, progress);
        GameObject go = maskDisplay.GetProgression(progress);
        sandingPainter = go.GetComponent<SandingPainter>();
        sandingInputOld = go.GetComponent<SandingInputOld>();
        sandingProgression = go.GetComponent<SandingProgression>();
        RotateWithRightClick rotateWithRightClick = go.GetComponent<RotateWithRightClick>();
        if (sandingInputOld != null)
        {
            sandingInputOld.isSanding = true;
            sandingInputOld.mainCamera = cam;
        }
        if (sandingProgression != null)
        {
            sandingProgression.OnFinished += Finish;
        }
        StartCoroutine(delayMainCamera());
    }
    private IEnumerator delayMainCamera()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Set MainCamera");
        sandingInputOld.mainCamera = cam;
    }
    public void Finish()
    {
        Debug.Log("Minigame Painting Finished");
        StartCoroutine(EndGame());
    }
    public IEnumerator EndGame()
    {
        sandingProgression.OnFinished -= Finish;
        EndMinigameScene();
        Hide();
        yield return new WaitForSeconds(2);
        EndMinigame();
    }

    public override void SetSettings()
    {
       

        
    }

}
