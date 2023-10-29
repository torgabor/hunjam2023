using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgradeable : MonoBehaviour
{
    [SerializeField] private int _maxLvl;
    [SerializeField] private int _decayRate;
    [SerializeField] private int _currentLvl;
    [SerializeField] private int _currentProgress;
    private PlantDestroyable _destroyable;
    public StateManager manager;
    public bool IsBeingWatered = false;

    public Sprite[] SpriteByLevel;
    public int[] ThresholdByLevel;
    private SpriteRenderer Renderer;

    public int CurrentLvl
    {
        get { return _currentLvl; }
        set
        {
            _currentLvl = Math.Clamp(value, 0, _maxLvl);
            LeveLChanged();
        }
    }

    public int CurrentPercentage
    {
        get { return _currentProgress; }
        set
        {
            _currentProgress = Math.Clamp(value, 0, ThresholdByLevel[CurrentLvl]);
            if (_currentProgress == ThresholdByLevel[CurrentLvl] && CurrentLvl < _maxLvl)
            {
                CurrentLvl++;
                _currentProgress = 0;

            }
            else if (_currentProgress == 0 && CurrentLvl > 0)
            {
                CurrentLvl--;
                _currentProgress = 100;
            }
        }
    }

    private void Awake()
    {
        _currentLvl = 0;
        _currentProgress = 0;
        _destroyable = GetComponent<PlantDestroyable>();
        Renderer = GetComponentInChildren<SpriteRenderer>();
        ChangeLevelSprite();
    }

    void Start()
    {
        StartCoroutine(DrainWater());
    }

    public void Water(int amount)
    {
        if (_destroyable.Hp < _destroyable.MaxHp)
        {
            _destroyable.Hp += amount;
        }
        else
        {
            CurrentPercentage += amount;

        }
    }

    private void LeveLChanged()
    {
        if (manager != null)
        {
            manager.LevelChanged(this);
        }

        ChangeLevelSprite();
    }

    private void ChangeLevelSprite()
    {
        if (SpriteByLevel != null && SpriteByLevel.Length > CurrentLvl)
        {
            Renderer.sprite = SpriteByLevel[CurrentLvl];
        }
    }

    private IEnumerator DrainWater()
    {
        while (true)
        {
            if (!IsBeingWatered)
            {
                _destroyable.Hp -= _decayRate;
            }
            yield return new WaitForSeconds(1);
        }

    }

}
