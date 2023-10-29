using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] Slider mainVolumeSlider;
    [SerializeField] AudioMixer audioMixer;

    public void SetMainVolume()
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(mainVolumeSlider.value)*20);
    }
}
