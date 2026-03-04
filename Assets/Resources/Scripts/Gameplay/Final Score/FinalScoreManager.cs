using UnityEngine;

public class FinalScoreManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private FinalScoreDisplay[] finalScoreDisplays;
    [SerializeField] private FinalScoreUI FinalScoreUI;

    public void ShowFinalScore()
    {
        FinalScoreUI.Show();
        for (int i = 0; i < GameManager.Instance.maskScoring.Count; i++)
        {
            finalScoreDisplays[i].gameObject.SetActive(true);   
            finalScoreDisplays[i].SetData(GameManager.Instance.maskScoring[i]);
        }
    }
}
