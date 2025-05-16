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
            float sfxVal = PlayerPrefs.GetFloat("SFX_VOLUME", 1f);
            sfxSlider.value = sfxVal;
            // 메뉴 열 때 즉시 반영
            SoundManager.Instance.SetSFXVolume(sfxVal);

            sfxSlider.onValueChanged.AddListener(val =>
            {
                SoundManager.Instance.SetSFXVolume(val);
                PlayerPrefs.SetFloat("SFX_VOLUME", val);
            });
        }

        // BGM 슬라이더
        if (GameObject.Find(BGM_SLIDER_NAME)?.TryGetComponent<Slider>(out var bgmSlider) == true)
        {
            float bgmVal = PlayerPrefs.GetFloat("BGM_VOLUME", 1f);
            bgmSlider.value = bgmVal;
            // 메뉴 열 때 즉시 반영
            SoundManager.Instance.SetBGMVolume(bgmVal);

            bgmSlider.onValueChanged.AddListener(val =>
            {
                SoundManager.Instance.SetBGMVolume(val);
                PlayerPrefs.SetFloat("BGM_VOLUME", val);
            });
        }

        // Master 슬라이더
        if (GameObject.Find(MASTER_SLIDER_NAME)?.TryGetComponent<Slider>(out var masterSlider) == true)
        {
            float masterVal = PlayerPrefs.GetFloat("MASTER_VOLUME", 1f);
            masterSlider.value = masterVal;
            // 메뉴 열 때 즉시 반영
            SoundManager.Instance.SetMasterVolume(masterVal);

            masterSlider.onValueChanged.AddListener(val =>
            {
                SoundManager.Instance.SetMasterVolume(val);
                PlayerPrefs.SetFloat("MASTER_VOLUME", val);
            });
        }
    }
}
