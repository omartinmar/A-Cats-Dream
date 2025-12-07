using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSet : MonoBehaviour
{

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;


    public void SetMusicVolume(){

        float volume = musicSlider.value;
        mixer.SetFloat("MusicParameter", Mathf.Log10(volume)*20);
    }

        public void SetSFXVolume(){

        float volume = musicSlider.value;
        mixer.SetFloat("SFXParameter", Mathf.Log10(volume)*20);

    }

}
