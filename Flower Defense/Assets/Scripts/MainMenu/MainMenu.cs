using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] string gameStartSceneName;
    [SerializeField] GameObject InstructionsPage, OptionsPage, CreditsPage;

    private void Start()
    {
        ReturnToMainMenu();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameStartSceneName);
    }

    public void Instructions()
    {
        InstructionsPage.SetActive(true);
    }

    public void Options()
    {
        OptionsPage.SetActive(true);
    }

    public void Credits()
    {
        CreditsPage.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        InstructionsPage.SetActive(false);
        OptionsPage.SetActive(false);
        CreditsPage.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
