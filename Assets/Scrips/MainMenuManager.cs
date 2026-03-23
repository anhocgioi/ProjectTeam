using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private const string PREF_BGM_VOLUME = "Audio_BGMVolume";
    private const string PREF_SFX_VOLUME = "Audio_SFXVolume";

    [Header("Menu Audio")]
    [SerializeField] private AudioClip menuBgmClip;
    [SerializeField] private AudioClip buttonClickClip;
    [SerializeField] private AudioClip buttonHoverClip;
    [SerializeField] private bool playBgmOnStart = true;
    [SerializeField] private bool loopBgm = true;

    [SerializeField] private float sceneSwitchDelaySeconds = 0.08f;
    [SerializeField] private float hoverCooldownSeconds = 0.1f;

    private float bgmVolume = 1f;
    private float sfxVolume = 1f;

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    private bool isSwitchingScene;
    private bool globalListenersAdded;
    private float lastHoverPlayTime;

    private void Awake()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        bgmSource.spatialBlend = 0f;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.spatialBlend = 0f;
    }

    private void Start()
    {
        bgmVolume = PlayerPrefs.GetFloat(PREF_BGM_VOLUME, 1f);
        sfxVolume = PlayerPrefs.GetFloat(PREF_SFX_VOLUME, 1f);

        if (playBgmOnStart && menuBgmClip != null)
        {
            bgmSource.clip = menuBgmClip;
            bgmSource.volume = bgmVolume;
            bgmSource.loop = loopBgm;
            bgmSource.Play();
        }

        if (!globalListenersAdded)
        {
            AddGlobalButtonClickSfx();
            globalListenersAdded = true;
        }

        AddGlobalButtonHoverSfx();
    }

    private void PlayClick()
    {
        if (buttonClickClip == null) return;
        sfxSource.PlayOneShot(buttonClickClip, sfxVolume);
    }

    public void PlayHover()
    {
        if (isSwitchingScene) return;
        AudioClip clip = buttonHoverClip != null ? buttonHoverClip : buttonClickClip;
        if (clip == null) return;
        if (Time.unscaledTime - lastHoverPlayTime < hoverCooldownSeconds) return;
        lastHoverPlayTime = Time.unscaledTime;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    private void AddGlobalButtonHoverSfx()
    {
        Button[] buttons = FindObjectsOfType<Button>(true);
        foreach (Button b in buttons)
        {
            if (b == null) continue;
            if (b.GetComponent<MenuButtonHoverSound>() != null) continue;
            b.gameObject.AddComponent<MenuButtonHoverSound>();
        }
    }

    private void AddGlobalButtonClickSfx()
    {
        if (buttonClickClip == null) return;
        Button[] buttons = FindObjectsOfType<Button>(true);
        foreach (Button b in buttons)
        {
            if (b == null) continue;
            b.onClick.AddListener(PlayClick);
        }
    }

    // ================= SINGLE PLAYER =================
    public void OpenSinglePlayer()
    {
        // Gán chế độ 1 người chơi vào GameDataManager trước khi load cảnh
        if (GameDataManager.instance != null)
        {
            GameDataManager.instance.isSinglePlayer = true;
            Debug.Log("Chế độ: Đấu với máy (Single Player)");
        }

        LoadSceneSafe("CharacterSelect");
    }

    // ================= MULTIPLAYER =================
    public void OpenMultiPlayer()
    {
        // Gán chế độ 2 người chơi vào GameDataManager
        if (GameDataManager.instance != null)
        {
            GameDataManager.instance.isSinglePlayer = false;
            Debug.Log("Chế độ: 2 Người chơi (Multiplayer)");
        }

        LoadSceneSafe("CharacterSelect");
    }

    public void OpenSettings()
    {
        LoadSceneSafe("Settings");
    }

    public void OpenHowToPlay()
    {
        LoadSceneSafe("HowToPlay");
    }

    public void ExitGame()
    {
        StartCoroutine(QuitAfterDelay());
    }

    private void LoadSceneSafe(string sceneName)
    {
        if (isSwitchingScene) return;
        isSwitchingScene = true;
        PlayClick(); // Phát âm thanh click ngay khi bấm
        StartCoroutine(LoadSceneAfterDelay(sceneName));
    }

    private IEnumerator LoadSceneAfterDelay(string sceneName)
    {
        float waitTime = sceneSwitchDelaySeconds;
        if (buttonClickClip != null)
            waitTime = Mathf.Max(waitTime, buttonClickClip.length + 0.02f);

        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator QuitAfterDelay()
    {
        float waitTime = sceneSwitchDelaySeconds;
        if (buttonClickClip != null)
            waitTime = Mathf.Max(waitTime, buttonClickClip.length + 0.02f);

        yield return new WaitForSeconds(waitTime);
        Application.Quit();
    }
}