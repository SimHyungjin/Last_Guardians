using UnityEngine;
using UnityEngine.UI;

public class RuntimeSoundUIBinder : MonoBehaviour
{
    private const string SFX_SLIDER_NAME = "SFX_Slider";
    private const string BGM_SLIDER_NAME = "BGM_Slider";
    private const string MASTER_SLIDER_NAME = "Master_Slider";

    private void Start()
    {
        // SFX 슬라이더
        if (GameObject.Find(SFX_SLIDER_NAME)?.TryGetComponent<Slider>(out var sfxSlider) == true)
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFX_VOLUME", 1f);
            sfxSlider.onValueChanged.AddListener(val =>
            {
                SoundManager.Instance.SetSFXVolume(val);
                PlayerPrefs.SetFloat("SFX_VOLUME", val);
            });
        }

        // BGM 슬라이더
        if (GameObject.Find(BGM_SLIDER_NAME)?.TryGetComponent<Slider>(out var bgmSlider) == true)
        {
            bgmSlider.value = PlayerPrefs.GetFloat("BGM_VOLUME", 1f);
            bgmSlider.onValueChanged.AddListener(val =>
            {
                SoundManager.Instance.SetBGMVolume(val);
                PlayerPrefs.SetFloat("BGM_VOLUME", val);
            });
        }

        // Master 슬라이더
        if (GameObject.Find(MASTER_SLIDER_NAME)?.TryGetComponent<Slider>(out var masterSlider) == true)
        {
            masterSlider.value = PlayerPrefs.GetFloat("MASTER_VOLUME", 1f);
            masterSlider.onValueChanged.AddListener(val =>
            {
                SoundManager.Instance.SetMasterVolume(val);
                PlayerPrefs.SetFloat("MASTER_VOLUME", val);
            });
        }
    }
}
