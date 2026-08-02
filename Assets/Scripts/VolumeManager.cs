using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField]Slider seSlider;

    void Start()
    {
        audioMixer.GetFloat("SE", out float seVolume);
        seSlider.value = seVolume;
    }
    public void SetSE(float volume)
    {
        audioMixer.SetFloat("SE", volume);
    }
}
