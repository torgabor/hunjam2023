using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveSimply : MonoBehaviour
{
    public Transform Target;
    public float Speed;
    private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _flyClips;

    void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.loop = true;
        _audioSource.clip = PlayAudioClips.GetRandomClip(_flyClips);
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null && Vector3.Distance(transform.position, Target.transform.position) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.position, Speed * Time.deltaTime);
            if (!_audioSource.isPlaying)
            {
                //_audioSource.Play();
            }
        }
        else if (_audioSource.isPlaying)
        {
            //_audioSource.Stop();
        }
    }
}
