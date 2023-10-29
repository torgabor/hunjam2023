using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] GameObject GameOverScreenElements;
    [SerializeField] string mainMenuSceneName;

    void Start()
    {
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
        GameOverScreenElements.SetActive(false);
    }

    public void ShowVictoryScreen()
    {
        Destroy(FindObjectOfType<PauseMenu>().gameObject);
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
