using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] GameObject GameOverScreenElements;
    [SerializeField] string mainMenuSceneName;
    private PauseMenu _pauseMenu;

    void Start()
    {
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
        GameOverScreenElements.SetActive(false);
        _pauseMenu= FindObjectOfType<PauseMenu>();
    }

    public void ShowGameOverScreen()
    {
        _pauseMenu.PauseUnpauseGame();
        GameOverScreenElements.SetActive(true);
    }

    public void Retry()
    {
        GameOverScreenElements.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
