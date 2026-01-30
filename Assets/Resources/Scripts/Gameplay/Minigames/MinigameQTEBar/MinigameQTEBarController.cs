using UnityEngine;

//[CreateAssetMenu(fileName = "qte_ar", menuName = "Scriptable Objects/qte_ar")]
public class MiniGameQTEBarController : MinigamePlayController
{
    //[SerializeField] QTEBarMover barMover;
    //[SerializeField] QTEJudge judge;
    //[SerializeField] QTEInput input;

    bool isPlaying;

    public override void StartMinigame()
    {
        base.StartMinigame();

        isPlaying = true;
        //barMover.StartMove();
    }

}
