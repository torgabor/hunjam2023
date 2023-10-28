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

    /// <summary>
    /// Play sound by index
    /// </summary>
    /// <param name="index"></param>
    public void PlaySound(int index)
    {
        audioSource.PlayOneShot(clips[index]);
    }
}