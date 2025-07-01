using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource Music;
    [SerializeField] private AudioSource Sounds;

    [Header("Audio Clip")]
    public AudioClip backgroud;

    public static AudioManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            if (instance.backgroud == this.backgroud)
            {
                Destroy(gameObject); 
            }
            else
            {
                Destroy(instance.gameObject);
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }


        Music.clip = backgroud;
        Music.loop = true;
        Music.Play();
    }

    private void Start()
    {

    }
    public void playSFX(AudioClip clip)
    {
        if (Sounds != null && clip != null)
        {
            Sounds.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Sounds or clip is null");
        }
    }
}