using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Upgradeable : MonoBehaviour
{
    [SerializeField] private int _maxLvl;
    [SerializeField] private int _decayRate;
    [SerializeField] private float _decayInterval=1f;
    [SerializeField] private int _currentLvl;
    [SerializeField] private int _currentProgress;
    private PlantDestroyable _destroyable;
    public StateManager manager;
    public bool IsBeingWatered = false;

    public Sprite[] SpriteByLevel;
    public int[] ThresholdByLevel;

    [SerializeField] private float[] _cooldownPerLevel;
    [SerializeField] private float[] _projectileSpeedPerLevel;
    [SerializeField] private int[] _projectileDamagePerLevel;
    
    private AudioSource _upgradeAudioSource;
    [SerializeField] private List<AudioClip> _upgradeClips;

    private SpriteRenderer Renderer;

    public int CurrentLvl
    {
        get { return _currentLvl; }
        set
        {
            if(_upgradeAudioSource!=null && _currentLvl<value)
            {
                _upgradeAudioSource.Play();
            }
            _currentLvl = Math.Clamp(value, 0, _maxLvl);
            LeveLChanged();
        }
    }

    public int CurrentProgress
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
        if (_projectileSpeedPerLevel.Length > 0 && _cooldownPerLevel.Length > 0 && _projectileSpeedPerLevel.Length > 0)
        {
            _destroyable.ChangeLevel(_projectileDamagePerLevel[CurrentLvl], _cooldownPerLevel[CurrentLvl], _projectileSpeedPerLevel[CurrentLvl]);
        }
        if(_upgradeClips.Count> 0)
        {
            AddAudioSource(_upgradeAudioSource, _upgradeClips, 1, (0.75f, 1.25f));
        }

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
            CurrentProgress += amount;

        }
    }

    private void LeveLChanged()
    {
        if (manager != null)
        {
            manager.LevelChanged(this);
        }
        if (_projectileSpeedPerLevel.Length > 0 && _cooldownPerLevel.Length > 0 && _projectileSpeedPerLevel.Length > 0)
        {
            _destroyable.ChangeLevel(_projectileDamagePerLevel[CurrentLvl], _cooldownPerLevel[CurrentLvl], _projectileSpeedPerLevel[CurrentLvl]);
        }
        ChangeLevelSprite();
    }
    private void AddAudioSource(AudioSource audioSource, List<AudioClip> clips, float volume, (float, float) pitch)
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clips[UnityEngine.Random.Range(0, clips.Count)];
        audioSource.volume = volume;
        audioSource.pitch = UnityEngine.Random.Range(pitch.Item1, pitch.Item2);
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
            yield return new WaitForSeconds(_decayInterval);
        }

    }

}
