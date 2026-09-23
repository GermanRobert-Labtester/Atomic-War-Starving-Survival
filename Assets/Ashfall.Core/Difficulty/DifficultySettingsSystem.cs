// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 181 — Difficulty Settings System
// Pure domain authority for campaign difficulty preset selection, customizable
// difficulty scalars (sliders), ironman/campaign lock enforcement, and save/restore.
// Connects with DifficultyPresetCatalog and DifficultyDirector.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Ashfall.Core.Difficulty
{
    [Serializable]
    public sealed class DifficultySettingsState
    {
        public int SchemaVersion { get; set; } = 1;
        public string ActivePresetId { get; set; } = "difficulty_standard";
        public bool IsLocked { get; set; }
        public bool IsCustom { get; set; }
        public DifficultyScalars CustomScalars { get; set; } = DifficultyScalars.Legacy();
    }

    /// <summary>
    /// Plan 181 — Coordinates campaign difficulty presets, granular slider customizations,
    /// and mid-campaign modification locking without bypassing DifficultyPresetCatalog authority.
    /// </summary>
    public sealed class DifficultySettingsSystem
    {
        private readonly DifficultySettingsState _state;
        private DifficultyPresetCatalog _catalog;

        public event Action<DifficultyScalars>? OnSettingsChanged;
        public event Action? OnSettingsLocked;

        public string ActivePresetId => _state.ActivePresetId;
        public bool IsLocked => _state.IsLocked;
        public bool IsCustom => _state.IsCustom;
        public DifficultyPresetCatalog Catalog => _catalog;

        public DifficultySettingsSystem()
        {
            _state = new DifficultySettingsState();
            _catalog = new DifficultyPresetCatalog();
        }

        public DifficultySettingsSystem(DifficultySettingsState state, DifficultyPresetCatalog? catalog = null)
        {
            _state = state ?? new DifficultySettingsState();
            _catalog = catalog ?? new DifficultyPresetCatalog();
        }

        // ── Catalog Loading ────────────────────────────────────────────────

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                var loaded = DifficultyPresetCatalogLoader.LoadFromJson(json);
                if (loaded != null)
                {
                    _catalog = loaded;
                    _catalog.Index();
                    if (string.IsNullOrWhiteSpace(_state.ActivePresetId) && !string.IsNullOrWhiteSpace(_catalog.default_preset_id))
                    {
                        _state.ActivePresetId = _catalog.default_preset_id;
                    }
                }
            }
            catch (Exception) { /* malformed catalog falls back to built-in defaults; authoring errors are enforced by the data-integrity gate */ }
        }

        // ── Preset Selection & Sliders ─────────────────────────────────────

        /// <summary>
        /// Selects an authored preset from the catalog.
        /// Fails if settings are locked.
        /// </summary>
        public bool SelectPreset(string presetId)
        {
            if (_state.IsLocked) return false;
            if (string.IsNullOrWhiteSpace(presetId)) return false;

            if (!_catalog.TryGet(presetId, out var preset))
                return false;

            _state.ActivePresetId = preset.id;
            _state.IsCustom = false;
            _state.CustomScalars = preset.scalars.Clone();

            OnSettingsChanged?.Invoke(GetEffectiveScalars());
            return true;
        }

        /// <summary>
        /// Sets a custom difficulty scalar value within safe bounds [0.25, 2.5].
        /// Fails if settings are locked.
        /// </summary>
        public bool SetCustomScalar(string scalarName, float value)
        {
            if (_state.IsLocked) return false;
            if (string.IsNullOrWhiteSpace(scalarName)) return false;

            float clamped = Math.Clamp(value, 0.25f, 2.5f);
            bool updated = false;

            switch (scalarName.ToLowerInvariant())
            {
                case "hunger_rate_mult":
                case "hunger":
                    _state.CustomScalars.hunger_rate_mult = clamped;
                    updated = true;
                    break;
                case "thirst_rate_mult":
                case "thirst":
                    _state.CustomScalars.thirst_rate_mult = clamped;
                    updated = true;
                    break;
                case "radiation_gain_mult":
                case "radiation":
                    _state.CustomScalars.radiation_gain_mult = clamped;
                    updated = true;
                    break;
                case "disease_onset_mult":
                case "disease":
                    _state.CustomScalars.disease_onset_mult = clamped;
                    updated = true;
                    break;
                case "hostile_encounter_mult":
                case "hostile":
                case "raids":
                    _state.CustomScalars.hostile_encounter_mult = clamped;
                    updated = true;
                    break;
                case "market_price_mult":
                case "market":
                case "economy":
                    _state.CustomScalars.market_price_mult = clamped;
                    updated = true;
                    break;
                case "equipment_decay_mult":
                case "equipment":
                    _state.CustomScalars.equipment_decay_mult = clamped;
                    updated = true;
                    break;
                case "crisis_deadline_mult":
                case "crisis":
                    _state.CustomScalars.crisis_deadline_mult = clamped;
                    updated = true;
                    break;
            }

            if (updated)
            {
                _state.IsCustom = true;
                _state.ActivePresetId = "difficulty_custom";
                OnSettingsChanged?.Invoke(GetEffectiveScalars());
            }

            return updated;
        }

        /// <summary>
        /// Permanently locks difficulty settings for the current campaign (ironman style).
        /// </summary>
        public void LockSettings()
        {
            if (!_state.IsLocked)
            {
                _state.IsLocked = true;
                OnSettingsLocked?.Invoke();
            }
        }

        /// <summary>
        /// Returns the current active scalar multipliers.
        /// </summary>
        public DifficultyScalars GetEffectiveScalars()
        {
            if (_state.IsCustom && _state.CustomScalars != null)
                return _state.CustomScalars.Clone();

            if (_catalog.TryGet(_state.ActivePresetId, out var preset))
                return preset.scalars.Clone();

            return DifficultyScalars.Legacy();
        }

        // ── Save / Restore ─────────────────────────────────────────────────

        public DifficultySettingsState CaptureState()
        {
            return new DifficultySettingsState
            {
                SchemaVersion = _state.SchemaVersion,
                ActivePresetId = _state.ActivePresetId,
                IsLocked = _state.IsLocked,
                IsCustom = _state.IsCustom,
                CustomScalars = _state.CustomScalars?.Clone() ?? DifficultyScalars.Legacy()
            };
        }

        public void RestoreState(DifficultySettingsState? saved)
        {
            if (saved == null) return;
            _state.SchemaVersion = saved.SchemaVersion;
            _state.ActivePresetId = string.IsNullOrWhiteSpace(saved.ActivePresetId) ? "difficulty_standard" : saved.ActivePresetId;
            _state.IsLocked = saved.IsLocked;
            _state.IsCustom = saved.IsCustom;
            _state.CustomScalars = saved.CustomScalars?.Clone() ?? DifficultyScalars.Legacy();
        }
    }
}
