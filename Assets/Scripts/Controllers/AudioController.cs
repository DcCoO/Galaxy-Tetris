using UnityEngine;

public class AudioController : MonoBehaviour
{

    public static AudioController instance;
    public AudioClip scoreClip;
    public AudioClip loseClip;

    public AudioSource sfxSource;

    private void Awake()
    {
        instance = this;
    }

    public void ScoreSound()
    {
        if(sfxSource.isPlaying) sfxSource.Stop();
        sfxSource.clip = scoreClip;
        sfxSource.Play();
    }
    
    public void LoseSound()
    {
        if(sfxSource.isPlaying) sfxSource.Stop();
        sfxSource.clip = loseClip;
        sfxSource.Play();
    }
}
