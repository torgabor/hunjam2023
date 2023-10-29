using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject PauseMenuElements;
    [SerializeField] string mainMenuSceneName;
    private GameManager _gameManager;
    public bool GamePaused = false;

    private void Start()
    {
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
        _gameManager=FindObjectOfType<GameManager>();
        GamePaused = false;
        ShowHidePauseMenuElements();
    }

    void ShowHidePauseMenuElements()
    {
        PauseMenuElements.SetActive(GamePaused);
    }

    public void PauseUnpauseGame()
    {
        GamePaused = !GamePaused;

        if (GamePaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    void Update()
    {
        if (!_gameManager.IsGameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpauseGame();
            ShowHidePauseMenuElements();
        }
    }

    public void Continue()
    {
        PauseUnpauseGame();
        ShowHidePauseMenuElements();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
