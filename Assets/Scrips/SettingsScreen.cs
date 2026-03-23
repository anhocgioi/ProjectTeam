using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsScreen : MonoBehaviour
{
    [Header("Audio UI")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Audio Source")]
    [SerializeField] private AudioSource bgmSource; // 🎵 NHẠC NỀN

    [Header("Resolution UI")]
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    [Header("Settings")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private float currentBgmVolume = 1f;
    private float currentSfxVolume = 1f;

    private Resolution[] resolutions;

    private const string PREF_BGM_VOLUME = "Audio_BGMVolume";
    private const string PREF_SFX_VOLUME = "Audio_SFXVolume";

    void Start()
    {
        // 🎵 Load volume
        currentBgmVolume = PlayerPrefs.GetFloat(PREF_BGM_VOLUME, 1f);
        currentSfxVolume = PlayerPrefs.GetFloat(PREF_SFX_VOLUME, 1f);

        // 🎵 ÁP DỤNG NGAY CHO NHẠC
        if (bgmSource != null)
        {
            bgmSource.volume = currentBgmVolume;
        }

        // 🎚️ Slider
        if (bgmSlider != null)
        {
            bgmSlider.value = currentBgmVolume;
            bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = currentSfxVolume;
            sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        }

        // 🖥️ Resolution
        resolutions = Screen.resolutions;

        if (resolutionDropdown != null)
        {
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();
            int currentIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentIndex;
            resolutionDropdown.RefreshShownValue();

            resolutionDropdown.onValueChanged.AddListener(SetResolution);
        }

        // 🖥️ Fullscreen
        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = Screen.fullScreen;
            fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        }
    }

    // 🎵 BGM (NHẠC NỀN)
    public void SetBgmVolume(float value)
    {
        currentBgmVolume = Mathf.Clamp01(value);

        if (bgmSource != null)
            bgmSource.volume = currentBgmVolume; // 🔥 CHỈNH NHẠC NỀN

        PlayerPrefs.SetFloat(PREF_BGM_VOLUME, currentBgmVolume);
        PlayerPrefs.Save();
    }

    // 🔊 SFX (sau này bạn tách riêng)
    public void SetSfxVolume(float value)
    {
        currentSfxVolume = Mathf.Clamp01(value);

        PlayerPrefs.SetFloat(PREF_SFX_VOLUME, currentSfxVolume);
        PlayerPrefs.Save();
    }

    // 🖥️ Resolution
    public void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    // 🖥️ Fullscreen
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    // 🔙 Exit
    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}