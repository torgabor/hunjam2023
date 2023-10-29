using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameOverScreen _gameOverPanel;
    [SerializeField] private GameOverScreen _victoryPanel;
    public bool IsGameOver = false;
    public void GameOver(bool win)
    {
        IsGameOver = false;
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
