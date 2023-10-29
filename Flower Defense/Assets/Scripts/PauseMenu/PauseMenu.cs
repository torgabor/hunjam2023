using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject PauseMenuElements;
    [SerializeField] string mainMenuSceneName;

    public bool GamePaused = false;

    private void Start()
    {
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowHidePauseMenuElements();
            PauseUnpauseGame();
        }
    }

    public void Continue()
    {
        ShowHidePauseMenuElements();
        PauseUnpauseGame();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
