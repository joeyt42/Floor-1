using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip earlyRoundMusic;
    public AudioClip lateRoundMusic;
    private bool switched = false;

    public void UpdateMusic(int currentRound)
    {
        if (currentRound <= 10 && audioSource.clip != earlyRoundMusic)
        {
            audioSource.clip = earlyRoundMusic;
            audioSource.loop = true;
            audioSource.Play();
        } else if (currentRound >= 11 && !switched)
        {
            audioSource.clip = lateRoundMusic;
            audioSource.loop = true;
            audioSource.Play();
            switched = true;
        }
    }
}
