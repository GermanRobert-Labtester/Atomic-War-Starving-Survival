using UnityEngine;

namespace AtomicWar._Game.Settings
{
    /// <summary>
    /// Persists player preferences (volume, resolution, accessibility) across sessions.
    /// Uses PlayerPrefs for simplicity; swap to JSON file later if needed.
    /// </summary>
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager Instance { get; private set; }

        [Header("Audio")]
        [Range(0f, 1f)] public float MasterVolume = 1f;
        [Range(0f, 1f)] public float MusicVolume = 0.7f;
        [Range(0f, 1f)] public float SFXVolume = 1f;

        [Header("Display")]
        public int ResolutionWidth = 1920;
        public int ResolutionHeight = 1080;
        public bool Fullscreen = true;
        public int TargetFPS = 60;

        [Header("Accessibility")]
        public bool ColorblindMode = false;
        public float TextScale = 1f;
        public bool ReduceMotion = false;

        private const string PP_MasterVol = "ash_master_vol";
        private const string PP_MusicVol = "ash_music_vol";
        private const string PP_SFXVol = "ash_sfx_vol";
        private const string PP_Fullscreen = "ash_fullscreen";
        private const string PP_Colorblind = "ash_colorblind";
        private const string PP_TextScale = "ash_text_scale";

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Load();
        }

        public void Load()
        {
            MasterVolume = PlayerPrefs.GetFloat(PP_MasterVol, 1f);
            MusicVolume = PlayerPrefs.GetFloat(PP_MusicVol, 0.7f);
            SFXVolume = PlayerPrefs.GetFloat(PP_SFXVol, 1f);
            Fullscreen = PlayerPrefs.GetInt(PP_Fullscreen, 1) == 1;
            ColorblindMode = PlayerPrefs.GetInt(PP_Colorblind, 0) == 1;
            TextScale = PlayerPrefs.GetFloat(PP_TextScale, 1f);
            Apply();
        }

        public void Save()
        {
            PlayerPrefs.SetFloat(PP_MasterVol, MasterVolume);
            PlayerPrefs.SetFloat(PP_MusicVol, MusicVolume);
            PlayerPrefs.SetFloat(PP_SFXVol, SFXVolume);
            PlayerPrefs.SetInt(PP_Fullscreen, Fullscreen ? 1 : 0);
            PlayerPrefs.SetInt(PP_Colorblind, ColorblindMode ? 1 : 0);
            PlayerPrefs.SetFloat(PP_TextScale, TextScale);
            PlayerPrefs.Save();
        }

        public void Apply()
        {
            AudioListener.volume = MasterVolume;
            Screen.fullScreen = Fullscreen;
            Application.targetFrameRate = TargetFPS;
        }

        public void SetVolume(float master, float music, float sfx)
        {
            MasterVolume = Mathf.Clamp01(master);
            MusicVolume = Mathf.Clamp01(music);
            SFXVolume = Mathf.Clamp01(sfx);
            Apply();
            Save();
        }

        public void SetFullscreen(bool fullscreen)
        {
            Fullscreen = fullscreen;
            Apply();
            Save();
        }

        public void SetColorblindMode(bool enabled)
        {
            ColorblindMode = enabled;
            Save();
        }

        public void SetTextScale(float scale)
        {
            TextScale = Mathf.Clamp(scale, 0.5f, 2f);
            Save();
        }
    }
}
