using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixingManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider fxSlider;
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        SetMasterVolume(masterSlider.value);
        SetFXVolume(fxSlider.value);
        SetMusicVolume(musicSlider.value);
    }

    public void SetMasterVolume(float level)
    {
        level = Mathf.Clamp(level, 0.0001f, 1f);
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
    }

    public void SetFXVolume(float level)
    {
        level = Mathf.Clamp(level, 0.0001f, 1f);
        audioMixer.SetFloat("fxVolume", Mathf.Log10(level) * 20f);
    }

    public void SetMusicVolume(float level)
    {
        level = Mathf.Clamp(level, 0.0001f, 1f);
        audioMixer.SetFloat("musikVolume", Mathf.Log10(level) * 20f);
    }
}
