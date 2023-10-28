using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayAudioClips : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> clips;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(int index, float volume = 1)
    {
        audioSource.PlayOneShot(clips[index], volume);
    }
}