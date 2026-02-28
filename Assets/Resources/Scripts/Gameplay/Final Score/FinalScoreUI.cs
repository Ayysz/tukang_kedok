using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalScoreUI : UIManager
{
    public void Continue()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
