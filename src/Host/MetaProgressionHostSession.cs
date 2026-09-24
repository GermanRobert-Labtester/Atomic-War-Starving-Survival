// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : MetaProgressionSaveStore
// Core State : Ashfall.Core.Endgame.MetaProgressionSaveState
// Host Caller: Main.MetaProgression (SetupMetaProgression / SaveMetaProgression)
// Purpose    : Plan 175 — Meta progression & cross-run profile store:
//              prestige scoring, crests, insignias, and New Game+ starting boons.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Endgame;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class MetaProgressionSaveStore
    {
        public const string FileName = "meta_progression_save.json";
        public const string SectionName = "meta_progression";

        private static readonly SaveStore<MetaProgressionSaveState> s_store =
            SaveStoreHub.Checksummed<MetaProgressionSaveState>(FileName, nameof(MetaProgressionSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(MetaProgressionSaveState state) => s_store.CaptureBare(state);
        public static MetaProgressionSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(MetaProgressionSaveState state) => s_store.TrySave(state);
        public static MetaProgressionSaveState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Host session manager for Plan 175 (Meta Progression & Cross-Run Profile Store).
    /// Binds catalog data from meta_unlockables.json, evaluates run history facts from
    /// CrossRunProfileStore, calculates prestige, unlocks meta cosmetic crests and NG+ boons,
    /// and persists state across campaigns.
    /// </summary>
    public sealed class MetaProgressionHostSession : HostSessionBase
    {
        private readonly MetaProgressionSystem _system;
        private string _lastEvent = string.Empty;

        public MetaProgressionSystem System => _system;
        public string LastEvent => _lastEvent;

        public MetaProgressionCensus Census => _system.GetCensus();
        public int TotalPrestige => _system.TotalPrestigeEarned;
        public IReadOnlyCollection<string> UnlockedIds => _system.UnlockedIds;
        public IReadOnlyCollection<string> ActiveNgPlusBoons => _system.ActiveNgPlusBoons;

        /// <summary>Authored starting grants for every active NG+ boon.</summary>
        public IReadOnlyList<MetaGrantDef> GetActiveNgPlusGrants() => _system.GetActiveNgPlusGrants();

        /// <summary>Authored starting grants for one boon id.</summary>
        public IReadOnlyList<MetaGrantDef> GetGrantsForBoon(string id) => _system.GetGrantsForBoon(id);

        public MetaProgressionHostSession(string? dataDir = null, MetaProgressionSystem? system = null)
        {
            _system = system ?? new MetaProgressionSystem();

            _system.OnItemUnlocked += itemId =>
            {
                _lastEvent = $"Unlocked meta progression reward: {itemId}";
                RaiseStateChanged();
            };

            if (!string.IsNullOrEmpty(dataDir))
            {
                LoadCatalog(dataDir);
            }
        }

        public static MetaProgressionHostSession Create(string dataDir, MetaProgressionSystem? system = null)
        {
            return new MetaProgressionHostSession(dataDir, system);
        }

        public void LoadCatalog(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;
            string path = Path.Combine(dataDir, "meta_unlockables.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                _system.LoadCatalog(json);
                _lastEvent = $"Loaded {_system.GetAllUnlockables().Count} meta unlockable definitions.";
                RaiseStateChanged();
            }
        }

        public int EvaluateProgress(
            IReadOnlyCollection<string>? completedAchievements = null,
            IReadOnlyCollection<string>? completedEndings = null)
        {
            return _system.EvaluateProgress(completedAchievements, completedEndings);
        }

        public void RecordCampaignCompletion(
            CampaignCompletionHistory? history,
            IReadOnlyCollection<string>? achievements = null,
            IReadOnlyCollection<string>? endings = null)
        {
            if (history != null)
            {
                _system.ProfileStore.Record(history);
            }
            int prestige = _system.EvaluateProgress(achievements, endings);
            _lastEvent = $"Campaign completed; total prestige evaluated to {prestige}.";
            RaiseStateChanged();
        }

        public bool SetNgPlusBoonActive(string id, bool active)
        {
            return _system.SetNgPlusBoonActive(id, active);
        }

        public bool IsUnlocked(string id)
        {
            return _system.IsUnlocked(id);
        }

        public bool IsBoonActive(string id)
        {
            return _system.IsBoonActive(id);
        }

        public MetaProgressionSaveState CaptureState() => _system.CaptureState();

        public void RestoreState(MetaProgressionSaveState? state) => _system.RestoreState(state);
    }
}
