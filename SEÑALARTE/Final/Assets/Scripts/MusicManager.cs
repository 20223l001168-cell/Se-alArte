using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CambiarMusica(AudioClip nuevaMusica)
    {
        if (audioSource.clip == nuevaMusica)
            return;

        audioSource.Stop();
        audioSource.clip = nuevaMusica;
        audioSource.Play();
    }

    public void DetenerMusica()
    {
        audioSource.Stop();
    }

    public void ReanudarMusica()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }
}