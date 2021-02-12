using UnityEngine;

public class AudioController : SingletonMonoBehaviour<AudioController>
{
    public AudioClip scoreClip;
    public AudioClip loseClip;

    public AudioSource sfxSource;

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
