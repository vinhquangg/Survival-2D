using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sounds;



    void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume") && PlayerPrefs.HasKey("SoundsVolume"))
        {
            loadMusic();
            loadSounds();
        }
        else
        {
            setSoundVolume();
            SetMusicVolume();
        }
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    private void loadMusic()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        SetMusicVolume();
    }

    public void setSoundVolume()
    {
        float volume = sounds.value;

        audioMixer.SetFloat("Sounds", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SoundsVolume", volume);
    }

    private void loadSounds()
    {
        sounds.value = PlayerPrefs.GetFloat("SoundsVolume");
        setSoundVolume();
    }

}//AudioSettings