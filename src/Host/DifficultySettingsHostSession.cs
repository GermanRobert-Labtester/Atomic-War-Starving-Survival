// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : DifficultySettingsSaveStore
// Core State : Ashfall.Core.Difficulty.DifficultySettingsState
// Host Caller: Main.DifficultySettings (SetupDifficultySettings / SaveDifficultySettings)
// Purpose    : Plan 181 — mutable runtime difficulty settings: active preset,
//              custom slider values, and the ironman campaign lock. The immutable
//              campaign identity selection stays in the checksummed campaign
//              header (XP-01); this section is the only runtime settings store.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Difficulty;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class DifficultySettingsSaveStore
    {
        public const string FileName = "difficulty_settings_save.json";
        public const string SectionName = "difficulty_settings";

        private static readonly SaveStore<DifficultySettingsState> s_store =
            SaveStoreHub.Checksummed<DifficultySettingsState>(FileName, nameof(DifficultySettingsSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(DifficultySettingsState state) => s_store.CaptureBare(state);
        public static DifficultySettingsState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(DifficultySettingsState state) => s_store.TrySave(state);
        public static DifficultySettingsState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Plan 181 host session. Wraps the signed pure-domain
    /// <see cref="DifficultySettingsSystem"/> over the campaign's existing
    /// <see cref="DifficultyPresetCatalog"/> instance. It is the settings/selection
    /// authority; it does not own a second catalog and does not bind scalar
    /// consumers itself — the canonical <c>DifficultyScalarsProvider</c> returned by
    /// <see cref="EffectiveProvider"/> is applied by <c>Main</c> to the systems that
    /// XP-01 already bound.
    /// </summary>
    public sealed class DifficultySettingsHostSession : HostSessionBase
    {
        /// <summary>The eight authored scalar lanes, in catalog order, exposed to the settings UI.</summary>
        public static readonly string[] ScalarNames =
        {
            "hunger_rate_mult",
            "thirst_rate_mult",
            "radiation_gain_mult",
            "disease_onset_mult",
            "hostile_encounter_mult",
            "market_price_mult",
            "equipment_decay_mult",
            "crisis_deadline_mult"
        };

        private readonly DifficultySettingsSystem _system;

        public DifficultySettingsSystem System => _system;
        public DifficultySettingsCensus Census => _system.GetCensus();
        public DifficultyScalarsProvider EffectiveProvider => _system.GetEffectiveProvider();
        public DifficultyScalars EffectiveScalars => _system.GetEffectiveScalars();
        public bool IsLocked => _system.IsLocked;
        public bool IsCustom => _system.IsCustom;
        public string ActivePresetId => _system.ActivePresetId;
        public IReadOnlyList<DifficultyPreset> Presets => _system.Catalog?.AllPresets ?? (IReadOnlyList<DifficultyPreset>)Array.Empty<DifficultyPreset>();

        public DifficultySettingsHostSession(DifficultyPresetCatalog catalog, DifficultySettingsState? state = null)
        {
            if (catalog == null) throw new ArgumentNullException(nameof(catalog));

            _system = new DifficultySettingsSystem(state ?? new DifficultySettingsState());
            _system.BindCatalog(catalog);

            _system.OnSettingsChanged += _ => RaiseStateChanged();
            _system.OnSettingsLocked += () => RaiseStateChanged();
        }

        public static DifficultySettingsHostSession Create(DifficultyPresetCatalog catalog, DifficultySettingsState? state = null) =>
            new DifficultySettingsHostSession(catalog, state);

        public bool SelectPreset(string presetId) => _system.SelectPreset(presetId);
        public bool SetCustomScalar(string scalarName, float value) => _system.SetCustomScalar(scalarName, value);

        /// <summary>Locks the settings for the rest of the campaign (ironman). Idempotent.</summary>
        public bool Lock()
        {
            _system.LockSettings();
            return _system.IsLocked;
        }

        public DifficultySettingsState CaptureState() => _system.CaptureState();
        public void RestoreState(DifficultySettingsState? state) => _system.RestoreState(state);

        /// <summary>Human-readable preset name for the active selection, or the custom label.</summary>
        public string ActiveDisplayName()
        {
            if (_system.IsCustom) return "Custom";
            return _system.Catalog != null && _system.Catalog.TryGet(_system.ActivePresetId, out var preset)
                ? preset.display_name
                : _system.ActivePresetId;
        }
    }
}
