// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Plan 181 — Difficulty Settings System host wiring.
// The signed pure-domain DifficultySettingsSystem (DEC-174) is the settings
// authority for preset selection, the eight custom slider lanes, and the
// ironman/campaign lock. It shares the campaign's single DifficultyPresetCatalog
// instance owned by XP-01; this host never loads a second catalog and never
// rebinds scalar consumers itself. It only routes the effective
// DifficultyScalarsProvider into the existing XP-01 `_difficultyScalars` field.
// ============================================================================

using System;
using Godot;
using Ashfall.Core.Difficulty;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private DifficultySettingsHostSession? _difficultySettings;
        private bool _difficultySettingsDirty;

        public DifficultySettingsHostSession? DifficultySettings => _difficultySettings;
        public bool DifficultySettingsDirty => _difficultySettingsDirty;

        /// <summary>
        /// Creates the settings session over the campaign's existing catalog. A
        /// saved runtime settings section wins; otherwise the session starts from
        /// the campaign header's identity preset, unlocked and not custom, so a
        /// legacy v1 campaign and a fresh campaign behave identically.
        /// </summary>
        public void SetupDifficultySettings()
        {
            if (_difficultySettings != null) return;
            if (!EnsureDifficultyAuthority(out _)) return;

            var catalog = EnsureDifficultyCatalog();
            var saved = DifficultySettingsSaveStore.TryLoad();

            DifficultySettingsState seed = saved ?? new DifficultySettingsState
            {
                ActivePresetId = string.IsNullOrWhiteSpace(_difficultyPresetId)
                    ? DifficultyScalarsProvider.Legacy.PresetId
                    : _difficultyPresetId
            };

            _difficultySettings = DifficultySettingsHostSession.Create(catalog, seed);
            _difficultySettings.StateChanged += () => _difficultySettingsDirty = true;
        }

        /// <summary>
        /// Pushes the settings authority's effective scalars into the XP-01 binding
        /// field. Consumers read that field on each calculation, so a mid-campaign
        /// preset or slider change takes effect without rebinding, and a malformed
        /// restored custom set falls back to the header preset instead of crashing.
        /// </summary>
        public void ApplyDifficultySettingsToScalars()
        {
            SetupDifficultySettings();
            if (_difficultySettings == null) return;

            try
            {
                _difficultyScalars = _difficultySettings.EffectiveProvider;
            }
            catch (Exception ex)
            {
                GD.PrintErr("[Ashfall Godot] Difficulty settings scalars rejected, keeping the header preset: " + ex.Message);
            }
        }

        /// <summary>
        /// Keeps the settings authority's preset aligned with the campaign header
        /// selection. An explicit custom configuration is never overwritten.
        /// </summary>
        public void SyncDifficultySettingsPreset(string presetId)
        {
            if (_difficultySettings == null) return;
            if (_difficultySettings.IsCustom) return;
            if (!string.IsNullOrWhiteSpace(presetId))
                _difficultySettings.SelectPreset(presetId);
        }

        /// <summary>Selects an authored preset at runtime. Refused once the campaign is locked.</summary>
        public bool SelectDifficultyPreset(string presetId)
        {
            SetupDifficultySettings();
            if (_difficultySettings == null) return false;
            if (!_difficultySettings.SelectPreset(presetId)) return false;
            ApplyDifficultySettingsToScalars();
            return true;
        }

        /// <summary>Sets one custom slider lane; switches to custom mode. Refused once locked.</summary>
        public bool SetDifficultyCustomScalar(string scalarName, float value)
        {
            SetupDifficultySettings();
            if (_difficultySettings == null) return false;
            if (!_difficultySettings.SetCustomScalar(scalarName, value)) return false;
            ApplyDifficultySettingsToScalars();
            return true;
        }

        /// <summary>Locks difficulty for the rest of the campaign (ironman). Idempotent.</summary>
        public bool LockDifficultySettings()
        {
            SetupDifficultySettings();
            if (_difficultySettings == null) return false;
            if (!_difficultySettings.Lock()) return false;
            ApplyDifficultySettingsToScalars();
            return true;
        }

        public void SaveDifficultySettings()
        {
            if (_difficultySettings == null) return;
            var state = _difficultySettings.CaptureState();
            DifficultySettingsSaveStore.TrySave(state);
            if (CaptureSection(DifficultySettingsSaveStore.SectionName, DifficultySettingsSaveStore.TryCapturePersisted(state)))
                _difficultySettingsDirty = false;
        }

        public DifficultySettingsCensus GetDifficultySettingsCensus() =>
            _difficultySettings?.Census ?? default;

        /// <summary>Human-readable active selection for the HUD/settings panel.</summary>
        public string DifficultyActiveDisplayName() =>
            _difficultySettings?.ActiveDisplayName()
            ?? (_difficultyScalars != null ? _difficultyScalars.PresetId : DifficultyScalarsProvider.Legacy.PresetId);

        public void FlushDifficultySettingsIfDirty()
        {
            if (_difficultySettingsDirty)
                SaveDifficultySettings();
        }

        public void ResetDifficultySettings()
        {
            _difficultySettings = null;
            _difficultySettingsDirty = false;
        }
    }
}
