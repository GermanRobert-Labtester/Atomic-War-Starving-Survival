using System.Collections.Generic;
using AtomicWar._Game.Settings;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI.MainMenu
{
    /// <summary>
    /// Binds the settings dialog's three controls to <see cref="SettingsManager"/>.
    ///
    /// Every change applies immediately — there is no Apply button and no
    /// pending state to reconcile, so what the player hears and sees is always
    /// what is stored. Confirming the dialog only flushes PlayerPrefs.
    /// </summary>
    public sealed partial class MainMenuController
    {
        private VisualElement _settingsBody;
        private Slider _volumeSlider;
        private Label _volumeValue;
        private Toggle _fullscreenToggle;
        private DropdownField _resolutionDropdown;

        private List<Vector2Int> _resolutions;

        /// <summary>
        /// Guards the change callbacks while the controls are being populated
        /// from stored values, so seeding the UI does not write back settings
        /// the player never touched.
        /// </summary>
        private bool _suppressSettingsCallbacks;

        private void BindSettings()
        {
            _settingsBody = _root.Q<VisualElement>("settings-body");
            _volumeSlider = _root.Q<Slider>("setting-volume");
            _volumeValue = _root.Q<Label>("setting-volume-value");
            _fullscreenToggle = _root.Q<Toggle>("setting-fullscreen");
            _resolutionDropdown = _root.Q<DropdownField>("setting-resolution");

            _resolutions = SettingsManager.AvailableResolutions();
            if (_resolutionDropdown != null)
            {
                _resolutionDropdown.choices = BuildResolutionChoices(_resolutions);
            }

            _volumeSlider?.RegisterValueChangedCallback(OnVolumeChanged);
            _fullscreenToggle?.RegisterValueChangedCallback(OnFullscreenChanged);
            _resolutionDropdown?.RegisterValueChangedCallback(OnResolutionChanged);
        }

        private void UnbindSettings()
        {
            _volumeSlider?.UnregisterValueChangedCallback(OnVolumeChanged);
            _fullscreenToggle?.UnregisterValueChangedCallback(OnFullscreenChanged);
            _resolutionDropdown?.UnregisterValueChangedCallback(OnResolutionChanged);

            _settingsBody = null;
            _volumeSlider = null;
            _volumeValue = null;
            _fullscreenToggle = null;
            _resolutionDropdown = null;
        }

        /// <summary>
        /// The start screen is the first scene, so nothing has created a
        /// SettingsManager yet. It makes itself persistent in Awake, so the
        /// one created here is the same instance the gameplay scene uses.
        /// </summary>
        private static SettingsManager Settings()
        {
            if (SettingsManager.Instance != null) return SettingsManager.Instance;
            var go = new GameObject("SettingsManager");
            return go.AddComponent<SettingsManager>();
        }

        /// <summary>Seed the controls from stored settings, without echoing back.</summary>
        private void RefreshSettingsControls()
        {
            SettingsManager settings = Settings();

            _suppressSettingsCallbacks = true;
            try
            {
                if (_volumeSlider != null) _volumeSlider.value = settings.MasterVolume;
                UpdateVolumeValueLabel(settings.MasterVolume);

                if (_fullscreenToggle != null) _fullscreenToggle.value = settings.Fullscreen;

                if (_resolutionDropdown != null)
                {
                    int index = IndexOfResolution(settings.ResolutionWidth, settings.ResolutionHeight);
                    _resolutionDropdown.index = index;
                }
            }
            finally
            {
                _suppressSettingsCallbacks = false;
            }
        }

        private void OnVolumeChanged(ChangeEvent<float> evt)
        {
            if (_suppressSettingsCallbacks) return;
            Settings().SetMasterVolume(evt.newValue);
            UpdateVolumeValueLabel(evt.newValue);
        }

        private void OnFullscreenChanged(ChangeEvent<bool> evt)
        {
            if (_suppressSettingsCallbacks) return;
            Settings().SetFullscreen(evt.newValue);
        }

        private void OnResolutionChanged(ChangeEvent<string> evt)
        {
            if (_suppressSettingsCallbacks) return;

            int index = _resolutionDropdown.index;
            if (_resolutions == null || index < 0 || index >= _resolutions.Count) return;

            Vector2Int size = _resolutions[index];
            Settings().SetResolution(size.x, size.y);
        }

        private void UpdateVolumeValueLabel(float value)
        {
            if (_volumeValue == null) return;
            _volumeValue.text = Mathf.RoundToInt(value * 100f) + "%";
        }

        /// <summary>
        /// SettingsManager already persists on every setter; this is the
        /// belt-and-braces flush when the player confirms, so a hard kill after
        /// the dialog closes cannot lose the change.
        /// </summary>
        private void PersistSettings() => Settings().Save();

        private static List<string> BuildResolutionChoices(List<Vector2Int> resolutions)
        {
            var choices = new List<string>(resolutions.Count);
            foreach (Vector2Int size in resolutions)
            {
                choices.Add($"{size.x} × {size.y}");
            }
            return choices;
        }

        /// <summary>
        /// Index of the stored size, or the largest available as a fallback —
        /// a stored resolution can disappear when a monitor is unplugged, and
        /// leaving the dropdown blank would look broken.
        /// </summary>
        private int IndexOfResolution(int width, int height)
        {
            if (_resolutions == null || _resolutions.Count == 0) return -1;

            for (int i = 0; i < _resolutions.Count; i++)
            {
                if (_resolutions[i].x == width && _resolutions[i].y == height) return i;
            }
            return _resolutions.Count - 1;
        }
    }
}
