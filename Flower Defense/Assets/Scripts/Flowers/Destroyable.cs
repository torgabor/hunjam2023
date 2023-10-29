using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public int MaxHp;
    private int _hp;
    [HideInInspector] public StateManager stateManager;
    private AudioSource _deathAudioSource;
    private AudioSource _damageAudioSource;
    [SerializeField] private List<AudioClip> _deathClips;
    [SerializeField] private List<AudioClip> _damageClips;

    public virtual int Hp
    {
        get { return _hp; }
        set
        {
            _hp = Math.Clamp(value, 0, MaxHp);
            if(value<0 && _damageAudioSource!=null)
            {
                _damageAudioSource.Play();
            }
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
        if(_deathClips.Count > 0) {
            AddAudioSource(_deathAudioSource, _deathClips, 1, (0.75f, 1.25f));
        }
        if (_damageClips.Count > 0)
        {
            AddAudioSource(_damageAudioSource, _damageClips, 1, (0.75f, 1.25f));
        }
        Hp = MaxHp;
    }

    private void AddAudioSource(AudioSource audioSource, List<AudioClip> clips,float volume, (float,float)pitch)
    {
        audioSource  = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clips[UnityEngine.Random.Range(0, clips.Count)];
        audioSource.volume = volume;
        audioSource.pitch = UnityEngine.Random.Range(pitch.Item1, pitch.Item2);
    }
    protected virtual void OnHpDown()
    {
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        if (_deathAudioSource != null)
        {
            _deathAudioSource.Play();
        }
        while (_deathAudioSource!=null && _deathAudioSource.isPlaying)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
}