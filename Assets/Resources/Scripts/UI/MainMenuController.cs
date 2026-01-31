using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject Credit;

    public void ShowCredit()
    { 
        Credit.SetActive(true);
    }
    public void HideCredit()
    {
        Credit.SetActive(false);
    }
    public void StartShop()
    {
        SceneManager.LoadScene("GameplayScene 1");
    }
    public void QuitGame()
    { 
        Application.Quit();
    }
}
