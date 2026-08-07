using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Settings
{
    /// <summary>
    /// Persists player preferences (volume, resolution, accessibility) across sessions.
    /// Uses PlayerPrefs for simplicity; swap to JSON file later if needed.
    ///
    /// Master volume is applied through <see cref="AudioListener.volume"/> rather
    /// than an AudioMixer group: nothing in the project routes through a mixer
    /// yet, so a mixer group would control nothing, whereas the listener scales
    /// every AudioSource in the scene automatically.
    /// </summary>
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager Instance { get; private set; }

        [Header("Audio")]
        [Range(0f, 1f)] public float MasterVolume = 1f;

        // Persisted but currently inert: no audio is routed into per-category
        // buses yet, so the main menu deliberately does not expose these --
        // showing a slider that does nothing would be worse than hiding it.
        [Range(0f, 1f)] public float MusicVolume = 0.7f;
        [Range(0f, 1f)] public float SFXVolume = 1f;

        [Header("Display")]
        public int ResolutionWidth;
        public int ResolutionHeight;
        public bool Fullscreen = true;
        public int TargetFPS = 60;

        /// <summary>
        /// Which fullscreen style to use while <see cref="Fullscreen"/> is true.
        /// Borderless by default, matching the project's own fullscreenMode.
        /// Kept separate from the bool so there is a single source of truth for
        /// "is the player in fullscreen" rather than two fields to keep in sync.
        /// </summary>
        public FullScreenMode FullscreenStyle = FullScreenMode.FullScreenWindow;

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
        private const string PP_ResWidth = "ash_res_width";
        private const string PP_ResHeight = "ash_res_height";

        /// <summary>The screen mode implied by the current settings.</summary>
        public FullScreenMode CurrentFullScreenMode =>
            Fullscreen ? FullscreenStyle : FullScreenMode.Windowed;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            // DontDestroyOnLoad is a play-mode-only operation; calling it from an
            // EditMode test logs an error and aborts the rest of Awake.
            if (Application.isPlaying) DontDestroyOnLoad(gameObject);
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

            // Default to whatever the player is already running at rather than a
            // hardcoded 1920x1080 -- forcing an unsaved default would resize a
            // smaller display on first launch.
            ResolutionWidth = PlayerPrefs.GetInt(PP_ResWidth, Screen.currentResolution.width);
            ResolutionHeight = PlayerPrefs.GetInt(PP_ResHeight, Screen.currentResolution.height);

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
            PlayerPrefs.SetInt(PP_ResWidth, ResolutionWidth);
            PlayerPrefs.SetInt(PP_ResHeight, ResolutionHeight);
            PlayerPrefs.Save();
        }

        public void Apply()
        {
            AudioListener.volume = MasterVolume;
            Application.targetFrameRate = TargetFPS;
            ApplyDisplay();
        }

        /// <summary>
        /// Push resolution + screen mode to the display. Split out of
        /// <see cref="Apply"/> so the no-op guard stays readable.
        /// </summary>
        private void ApplyDisplay()
        {
            FullScreenMode mode = CurrentFullScreenMode;

            // A zero/negative size means "never configured" -- change only the
            // window mode and leave the player's current resolution alone.
            if (ResolutionWidth <= 0 || ResolutionHeight <= 0)
            {
                if (Screen.fullScreenMode != mode) Screen.fullScreenMode = mode;
                return;
            }

            // SetResolution triggers a real mode switch, so skip it when nothing
            // would change. (In the Editor Screen.width is the Game view size,
            // so this rarely short-circuits there -- harmless, since the Editor
            // ignores the resolution change anyway.)
            if (Screen.width == ResolutionWidth
                && Screen.height == ResolutionHeight
                && Screen.fullScreenMode == mode)
            {
                return;
            }

            Screen.SetResolution(ResolutionWidth, ResolutionHeight, mode);
        }

        /// <summary>A-12: Persist settings on application quit.</summary>
        private void OnApplicationQuit()
        {
            Save();
        }

        public void SetVolume(float master, float music, float sfx)
        {
            MasterVolume = Mathf.Clamp01(master);
            MusicVolume = Mathf.Clamp01(music);
            SFXVolume = Mathf.Clamp01(sfx);
            Apply();
            Save();
        }

        /// <summary>Set master volume alone (what the main-menu slider drives).</summary>
        public void SetMasterVolume(float master)
        {
            MasterVolume = Mathf.Clamp01(master);
            Apply();
            Save();
        }

        public void SetFullscreen(bool fullscreen)
        {
            Fullscreen = fullscreen;
            Apply();
            Save();
        }

        /// <summary>Set and persist the target resolution. Ignores non-positive sizes.</summary>
        public void SetResolution(int width, int height)
        {
            if (width <= 0 || height <= 0)
            {
                Debug.LogWarning(
                    $"[SettingsManager] Ignoring invalid resolution {width}x{height}.");
                return;
            }
            ResolutionWidth = width;
            ResolutionHeight = height;
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

        /// <summary>
        /// Distinct width x height options for a resolution picker, smallest
        /// first. <see cref="Screen.resolutions"/> lists one entry per refresh
        /// rate, so the same size appears several times; the menu only cares
        /// about the size.
        /// </summary>
        public static List<Vector2Int> AvailableResolutions()
        {
            var seen = new HashSet<Vector2Int>();
            var result = new List<Vector2Int>();

            foreach (Resolution resolution in Screen.resolutions)
            {
                var size = new Vector2Int(resolution.width, resolution.height);
                if (seen.Add(size)) result.Add(size);
            }

            // Screen.resolutions is empty on some headless/batchmode setups.
            if (result.Count == 0)
            {
                result.Add(new Vector2Int(
                    Screen.currentResolution.width, Screen.currentResolution.height));
            }

            result.Sort((a, b) => a.x != b.x ? a.x.CompareTo(b.x) : a.y.CompareTo(b.y));
            return result;
        }
    }
}
