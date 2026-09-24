// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 184 — Accessibility Options System host wiring.
// Pure domain AccessibilitySettingsSystem governs visual, hearing, motor, and
// cognitive profiles, font scaling, high contrast, and colorblind modes.
// Bridges with UserSettingsStore as the single engine-level user preference authority.
// ============================================================================

using System;
using Godot;
using Ashfall.Core.Accessibility;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private AccessibilitySettingsHostSession? _accessibilitySettings;
        private bool _accessibilitySettingsDirty;

        public AccessibilitySettingsHostSession? AccessibilitySettings => _accessibilitySettings;

        public void SetupAccessibilitySettings()
        {
            if (_accessibilitySettings != null) return;

            var saved = AccessibilitySettingsSaveStore.TryLoad();
            _accessibilitySettings = AccessibilitySettingsHostSession.Create(saved);
            _accessibilitySettings.StateChanged += () => _accessibilitySettingsDirty = true;

            // Load authored accessibility profiles catalog
            string catalogPath = CatalogPath.ResolveCatalog("accessibility_profiles.json");
            var catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (catalogIo.FileExists(catalogPath))
            {
                _accessibilitySettings.LoadCatalog(catalogIo.ReadAllText(catalogPath));
            }
        }

        public bool ApplyAccessibilityProfile(string profileId)
        {
            SetupAccessibilitySettings();
            return _accessibilitySettings!.ApplyProfile(profileId);
        }

        public void SetAccessibilityColorblindMode(string mode)
        {
            SetupAccessibilitySettings();
            _accessibilitySettings!.SetColorblindMode(mode);
        }

        public void SetAccessibilityFontScale(float scale)
        {
            SetupAccessibilitySettings();
            _accessibilitySettings!.SetFontScale(scale);
        }

        public void SetAccessibilityHighContrast(bool enabled)
        {
            SetupAccessibilitySettings();
            _accessibilitySettings!.SetHighContrast(enabled);
        }

        public void SetAccessibilityReducedMotion(bool enabled)
        {
            SetupAccessibilitySettings();
            _accessibilitySettings!.SetReducedMotion(enabled);
        }

        public AccessibilityCensus GetAccessibilityCensus() =>
            _accessibilitySettings?.Census ?? default;

        public void SaveAccessibilitySettings()
        {
            if (_accessibilitySettings == null) return;
            var state = _accessibilitySettings.System.CaptureState();
            AccessibilitySettingsSaveStore.TrySave(state);
            if (CaptureSection(
                    AccessibilitySettingsSaveStore.SectionName,
                    AccessibilitySettingsSaveStore.TryCapturePersisted(state)))
            {
                _accessibilitySettingsDirty = false;
            }
        }

        public void FlushAccessibilitySettingsIfDirty()
        {
            if (_accessibilitySettingsDirty)
            {
                SaveAccessibilitySettings();
            }
        }

        public void ResetAccessibilitySettings()
        {
            _accessibilitySettings = null;
            _accessibilitySettingsDirty = false;
        }
    }
}
