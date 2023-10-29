using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameOverScreen _gameOverPanel;
    [SerializeField] private GameOverScreen _victoryPanel;
    public void GameOver(bool win)
    {
        if (win)
        {
            _victoryPanel.ShowGameOverScreen();
        }
        else
        {
            _gameOverPanel.ShowGameOverScreen();
        }
    }
}
