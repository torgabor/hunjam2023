using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgradeable : MonoBehaviour
{
    [SerializeField] private int _maxLvl;
    [SerializeField] private int _decayRate;
    private int _currentLvl;
    private int _currentPercentage;
    private PlantDestroyable _destroyable;
    public CropStateManager manager;
    public bool IsBeingWatered = false;
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
        get { return _currentPercentage; }
        set
        {
            _currentPercentage = Math.Clamp(value, 0, 100);
            if (_currentPercentage == 100 && CurrentLvl < _maxLvl)
            {
                CurrentLvl++;
                _currentPercentage = 0;
            }
            if (_currentPercentage == 0 && CurrentLvl > 0)
            {
                CurrentLvl--;
                _currentPercentage = 100;
            }
        }
    }
    void Start()
    {
        _currentLvl = 0;
        _currentPercentage = 0;
        _destroyable = GetComponent<PlantDestroyable>();
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
    private void LevelUp()
    {
        CurrentLvl++;
        manager.LevelChanged(this);
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
