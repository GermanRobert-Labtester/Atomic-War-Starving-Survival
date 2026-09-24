// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : AccessibilitySettingsSaveStore
// Core State : Ashfall.Core.Accessibility.AccessibilitySettingsState
// Host Caller: Main.AccessibilitySettings
// Purpose    : Plan 184 — Accessibility Options System host session & persistence.
// ============================================================================

using System;
using Ashfall.Core.Accessibility;
using Ashfall.Core.Save;
using AtomicWar.GodotApp.Settings;

namespace AtomicWar.GodotApp
{
    public static class AccessibilitySettingsSaveStore
    {
        public const string FileName = "accessibility_settings_save.json";
        public const string SectionName = "accessibility_settings";

        private static readonly SaveStore<AccessibilitySettingsState> s_store =
            SaveStoreHub.Checksummed<AccessibilitySettingsState>(FileName, nameof(AccessibilitySettingsSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(AccessibilitySettingsState state) => s_store.CaptureBare(state);
        public static AccessibilitySettingsState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(AccessibilitySettingsState state) => s_store.TrySave(state);
        public static AccessibilitySettingsState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Plan 184 host session. Wraps <see cref="AccessibilitySettingsSystem"/>.
    /// Governs visual, hearing, motor, and cognitive accessibility profiles,
    /// typography scaling, high contrast, colorblind filters, and custom assists.
    /// Synchronizes changes with <see cref="UserSettingsStore"/> when active.
    /// </summary>
    public sealed class AccessibilitySettingsHostSession : HostSessionBase
    {
        private readonly AccessibilitySettingsSystem _system;

        public AccessibilitySettingsSystem System => _system;
        public AccessibilityCensus Census => _system.GetCensus();
        public string LastEvent { get; private set; } = string.Empty;

        public AccessibilitySettingsHostSession(AccessibilitySettingsState? state = null)
        {
            _system = new AccessibilitySettingsSystem(state ?? new AccessibilitySettingsState());
            _system.OnSettingsChanged += OnSettingsChangedHandler;
        }

        private void OnSettingsChangedHandler(AccessibilitySettingsState state)
        {
            LastEvent = $"Accessibility settings changed: Profile '{state.ActiveProfileId}', HighContrast={state.HighContrast}, FontScale={state.FontScale:0.00}.";
            RaiseStateChanged();
        }

        public static AccessibilitySettingsHostSession Create(AccessibilitySettingsState? state = null) =>
            new AccessibilitySettingsHostSession(state);

        public void LoadCatalog(string json)
        {
            _system.LoadCatalog(json);
            LastEvent = $"Loaded {_system.GetAllProfiles().Count} accessibility profiles.";
            RaiseStateChanged();
        }

        public bool ApplyProfile(string profileId)
        {
            bool applied = _system.ApplyProfile(profileId);
            if (applied)
            {
                LastEvent = $"Applied accessibility profile '{profileId}'.";
                SyncToUserSettings();
            }
            return applied;
        }

        public void SetColorblindMode(string mode)
        {
            _system.SetColorblindMode(mode);
            SyncToUserSettings();
        }

        public void SetFontScale(float scale)
        {
            _system.SetFontScale(scale);
            SyncToUserSettings();
        }

        public void SetHighContrast(bool enabled)
        {
            _system.SetHighContrast(enabled);
            SyncToUserSettings();
        }

        public void SetReducedMotion(bool enabled)
        {
            _system.SetReducedMotion(enabled);
            SyncToUserSettings();
        }

        public void SetVisualAudioAlerts(bool enabled) => _system.SetVisualAudioAlerts(enabled);
        public void SetSubtitleSize(string size) => _system.SetSubtitleSize(size);
        public void SetCognitiveLoadReduction(bool enabled) => _system.SetCognitiveLoadReduction(enabled);
        public void SetAutoWalk(bool enabled) => _system.SetAutoWalk(enabled);
        public void SetAimAssist(bool enabled) => _system.SetAimAssist(enabled);

        private void SyncToUserSettings()
        {
            // Single authority bridge: if UserSettingsStore is initialized, sync relevant flags
            try
            {
                var cur = UserSettingsStore.Current;
                if (cur != null)
                {
                    cur.HighContrast = _system.HighContrast;
                    cur.ReducedMotion = _system.ReducedMotion;
                    cur.LargeFonts = _system.FontScale > 1.1f;
                    if (!string.IsNullOrWhiteSpace(_system.ColorblindMode))
                    {
                        cur.ColorblindMode = _system.ColorblindMode;
                    }
                }
            }
            catch
            {
                // Engine/unit test resilience
            }
        }
    }
}
