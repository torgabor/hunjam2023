using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocustSounds : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> _flyClips;
    [SerializeField]
    private List<AudioClip> _deathClips;
    [SerializeField]
    private List<AudioClip> _hitClips;
    [SerializeField]
    private List<AudioClip> _attackClips;

    private AudioSource _flySource;
    private AudioSource _attackSource;
    private AudioSource _hitSource;
    private AudioSource _deathSource;

    // Start is called before the first frame update
    void Start()
    {
        var audioSources = GetComponents<AudioSource>();
        _flySource = audioSources[0];
        _attackSource = audioSources[0];
        _hitSource = audioSources[0];
        _deathSource = audioSources[0];

        _flySource.loop = true;
    }

    AudioClip GetRandomClip(List<AudioClip> clips)
    {
        return clips[Random.Range(0, clips.Count)];
    }

    public void PlayFly()
    {
        if (_flySource.isPlaying) return;
        _flySource.clip = GetRandomClip(_flyClips);
    }

    public void PlayWalk()
    {
        if (!_flySource.isPlaying) return;
        _flySource.Stop();
    }

    public void PlayAttack()
    {
        _attackSource.PlayOneShot(GetRandomClip(_attackClips));
    }

    public void PlayDie()
    {
        _deathSource.PlayOneShot(GetRandomClip(_deathClips));
    }

    public void PlayHit()
    {
        _hitSource.PlayOneShot(GetRandomClip(_hitClips));
    }
}
