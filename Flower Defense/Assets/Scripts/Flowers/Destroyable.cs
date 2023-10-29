using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public int MaxHp;
    private int _hp;
    [HideInInspector] public StateManager stateManager;
    private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _deathClips;

    public virtual int Hp
    {
        get { return _hp; }
        set
        {
            _hp = Math.Clamp(value, 0, MaxHp);
            if (_hp == 0)
            {
                OnHpDown();
            }

            if (stateManager != null)
            {
                stateManager.HpChanged(this);
            }
        }
    }

    protected virtual void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.clip = _deathClips[UnityEngine.Random.Range(0, _deathClips.Count)];
        _audioSource.volume = 1f;
        _audioSource.pitch = UnityEngine.Random.Range(0.75f, 1.25f);
        Hp = MaxHp;
    }

    protected virtual void OnHpDown()
    {
        Die();
    }

    private void Die()
    {
        _audioSource.Play();
        Destroy(gameObject);
    }
}