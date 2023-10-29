using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameManager gameManager;
    public LocustSpawner spawner;
    public Wave[] waves;
    // public Wave alma;
    public int waveIndex;
    public TextMeshProUGUI text;

    public IEnumerator StartWave()
    {
        spawner.spawning = false;
        float fadeDuration = 0.5f;
        
        float timeElapsed = 0f;
        text.text = $"Wave {waveIndex + 1}";
        text.enabled = true;
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timeElapsed / fadeDuration);
            text.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);
        timeElapsed = 0f;
        
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timeElapsed / fadeDuration);
            text.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }
        text.enabled = false;
        spawner.ApplyWave(waves[waveIndex]);
        spawner.spawning = true;

    }

    public void Start()
    {
        text.enabled = false;
        StartCoroutine(StartWave());
    }

    public void Update()
    {
        if (spawner.spawning && spawner.WaveEnded())
        {
            if (waveIndex < waves.Length-1)
            {
                waveIndex++;
                StartCoroutine(StartWave());
            }
            else
            {   
                gameManager.GameOver(true);
            }
        }
    }
}
