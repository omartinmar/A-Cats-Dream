using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    public AudioClip backgroundMusic;




private void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }
}
