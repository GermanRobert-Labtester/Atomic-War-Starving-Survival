using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Settings;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Covers the PlayerPrefs round-trip behind the main menu's Settings
    /// dialog, with particular attention to the resolution keys, which were
    /// previously declared as fields but never persisted or applied.
    ///
    /// PlayerPrefs is machine-wide, so the fixture snapshots every key it
    /// touches and restores it afterwards rather than deleting: a developer
    /// running the suite should not lose their own audio/display settings.
    /// </summary>
    [TestFixture]
    public class SettingsManagerPersistenceTests
    {
        private static readonly string[] TouchedKeys =
        {
            "ash_master_vol", "ash_music_vol", "ash_sfx_vol",
            "ash_fullscreen", "ash_colorblind", "ash_text_scale",
            "ash_res_width", "ash_res_height"
        };

        private readonly Dictionary<string, string> _savedStrings = new Dictionary<string, string>();
        private GameObject _host;
        private SettingsManager _settings;

        [SetUp]
        public void SetUp()
        {
            // Snapshot as strings: PlayerPrefs has no type-agnostic getter, but
            // GetString round-trips ints/floats well enough to restore them.
            _savedStrings.Clear();
            foreach (string key in TouchedKeys)
            {
                if (PlayerPrefs.HasKey(key)) _savedStrings[key] = PlayerPrefs.GetString(key, null);
                PlayerPrefs.DeleteKey(key);
            }

            _host = new GameObject(nameof(SettingsManagerPersistenceTests));
            _settings = _host.AddComponent<SettingsManager>();
        }

        [TearDown]
        public void TearDown()
        {
            if (_host != null) Object.DestroyImmediate(_host);

            foreach (string key in TouchedKeys) PlayerPrefs.DeleteKey(key);
            foreach (KeyValuePair<string, string> saved in _savedStrings)
            {
                if (saved.Value != null) PlayerPrefs.SetString(saved.Key, saved.Value);
            }
            PlayerPrefs.Save();
        }

        // -------------------------------------------------------------
        // Resolution (the gap this change closes)
        // -------------------------------------------------------------

        [Test]
        public void SetResolution_ThenLoad_RestoresTheSavedSize()
        {
            _settings.SetResolution(1600, 900);

            _settings.ResolutionWidth = 1;
            _settings.ResolutionHeight = 1;
            _settings.Load();

            Assert.That(_settings.ResolutionWidth, Is.EqualTo(1600));
            Assert.That(_settings.ResolutionHeight, Is.EqualTo(900));
        }

        [Test]
        public void SetResolution_WithNonPositiveSize_LeavesTheExistingSizeUnchanged()
        {
            _settings.SetResolution(1280, 720);

            LogAssert_ExpectInvalidResolutionWarning();
            _settings.SetResolution(0, -5);

            Assert.That(_settings.ResolutionWidth, Is.EqualTo(1280));
            Assert.That(_settings.ResolutionHeight, Is.EqualTo(720));
        }

        [Test]
        public void Load_WithNoStoredResolution_DefaultsToTheCurrentDisplaySize()
        {
            // Never force an unsaved default onto the player's display.
            _settings.Load();

            Assert.That(_settings.ResolutionWidth, Is.EqualTo(Screen.currentResolution.width));
            Assert.That(_settings.ResolutionHeight, Is.EqualTo(Screen.currentResolution.height));
        }

        // -------------------------------------------------------------
        // Volume + fullscreen
        // -------------------------------------------------------------

        [Test]
        public void SetMasterVolume_ThenLoad_RestoresTheSavedVolume()
        {
            _settings.SetMasterVolume(0.35f);

            _settings.MasterVolume = 1f;
            _settings.Load();

            Assert.That(_settings.MasterVolume, Is.EqualTo(0.35f).Within(0.0001f));
        }

        [Test]
        public void SetMasterVolume_Always_AppliesToTheAudioListener()
        {
            _settings.SetMasterVolume(0.5f);

            Assert.That(AudioListener.volume, Is.EqualTo(0.5f).Within(0.0001f));
        }

        [Test]
        public void SetMasterVolume_OutOfRange_ClampsToZeroOne()
        {
            _settings.SetMasterVolume(2.5f);
            Assert.That(_settings.MasterVolume, Is.EqualTo(1f).Within(0.0001f));

            _settings.SetMasterVolume(-1f);
            Assert.That(_settings.MasterVolume, Is.EqualTo(0f).Within(0.0001f));
        }

        [Test]
        public void SetFullscreen_ThenLoad_RestoresTheSavedFlag()
        {
            _settings.SetFullscreen(false);

            _settings.Fullscreen = true;
            _settings.Load();

            Assert.That(_settings.Fullscreen, Is.False);
        }

        [Test]
        public void CurrentFullScreenMode_WhenWindowed_IsWindowed()
        {
            _settings.Fullscreen = false;

            Assert.That(_settings.CurrentFullScreenMode, Is.EqualTo(FullScreenMode.Windowed));
        }

        [Test]
        public void CurrentFullScreenMode_WhenFullscreen_UsesTheConfiguredStyle()
        {
            _settings.Fullscreen = true;
            _settings.FullscreenStyle = FullScreenMode.ExclusiveFullScreen;

            Assert.That(_settings.CurrentFullScreenMode,
                Is.EqualTo(FullScreenMode.ExclusiveFullScreen));
        }

        // -------------------------------------------------------------
        // Resolution options
        // -------------------------------------------------------------

        [Test]
        public void AvailableResolutions_Always_ReturnsDistinctAscendingSizes()
        {
            List<Vector2Int> sizes = SettingsManager.AvailableResolutions();

            Assert.That(sizes, Is.Not.Empty, "Must always offer at least the current display size.");
            Assert.That(sizes, Is.Unique, "Screen.resolutions repeats sizes per refresh rate.");

            for (int i = 1; i < sizes.Count; i++)
            {
                bool ascending = sizes[i].x > sizes[i - 1].x
                    || (sizes[i].x == sizes[i - 1].x && sizes[i].y >= sizes[i - 1].y);
                Assert.That(ascending, Is.True, $"Not ascending at index {i}: {sizes[i - 1]} -> {sizes[i]}");
            }
        }

        /// <summary>
        /// SetResolution logs a warning for invalid input; the test runner
        /// treats unexpected log output as a failure, so declare it.
        /// </summary>
        private static void LogAssert_ExpectInvalidResolutionWarning()
        {
            UnityEngine.TestTools.LogAssert.Expect(LogType.Warning,
                new System.Text.RegularExpressions.Regex("Ignoring invalid resolution"));
        }
    }
}
