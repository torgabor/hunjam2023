using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgradeable : MonoBehaviour
{
    [SerializeField] private int _maxLvl;
    [SerializeField] private int _decayRate;
    private int _currentLvl;
    private int _currentProgress;
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
                LeveLChanged();

            }
            else if (_currentProgress == 0 && CurrentLvl > 0)
            {
                CurrentLvl--;
                _currentProgress = ThresholdByLevel[CurrentLvl];
                LeveLChanged();

            }
        }
    }
    void Start()
    {
        _currentLvl = 0;
        _currentProgress = 0;
        _destroyable = GetComponent<PlantDestroyable>();
        StartCoroutine(DrainWater());
        Renderer = GetComponentInChildren<SpriteRenderer>();
        ChanngeLevelSprite();
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

        ChanngeLevelSprite();
    }

    private void ChanngeLevelSprite()
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
