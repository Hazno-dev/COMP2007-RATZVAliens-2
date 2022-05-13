using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class VolumeChanger : MonoBehaviour
{
    [Header("Volume Definitions")]
    public AudioSource AudioSource_;
    public Slider VolumeSlider;
    public int amountToDivide = 6;
    // Start is called before the first frame update
    void Start()
    {
        if (AudioSource_ != null) InvokeRepeating("UpdateVolume", 0f, 0.2f);
        if (VolumeSlider != null) StartCoroutine(LateStart());
        Debug.Log(AudioManager.ReturnVolume());
    }

    //Set a new value to the audios volume
    void UpdateVolume()
    {
        AudioSource_.volume = AudioManager.ReturnVolume() / amountToDivide;
    }

    //A reference to the static audiomanager, updates current volume
    public void ChangeVolume()
    {
        if (VolumeSlider != null) AudioManager.ChangeVolume(VolumeSlider.value);
        Debug.Log(AudioManager.volume);
    }

    //Wait a few seconds before setting the current sound instance to the static volume
    IEnumerator LateStart() 
    {
        yield return new WaitForSeconds(0.01f);
        Debug.Log(AudioManager.volume + "Returning");
        VolumeSlider.value = AudioManager.ReturnVolume();
    }

    //Sound instance is played
    public void PlaySound()
    {
        AudioSource_.Play();
    }
}
