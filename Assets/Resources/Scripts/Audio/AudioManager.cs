using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public GlobalAudioList GlobalAudioList;
    public AudioSource musicSource;
    public AudioSource sfxSource;
    private void Awake()
    {
        Instance = this;   
    }
    public void PlaySfx(AudioClip clip)
    { 
        sfxSource.PlayOneShot(clip);
    }
   
}
