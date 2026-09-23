// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Endgame
{
    [Serializable]
    public sealed class MetaUnlockableDef
    {
        public string id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string category { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int required_prestige { get; set; }
        public string? required_ending_id { get; set; }
        public string? required_achievement_id { get; set; }
    }

    [Serializable]
    public sealed class MetaUnlockablesCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<MetaUnlockableDef> unlockables { get; set; } = new();
    }

    [Serializable]
    public sealed class MetaProgressionSaveState
    {
        public int schema_version { get; set; } = 1;
        public int totalPrestigeEarned { get; set; }
        public List<string> unlockedIds { get; set; } = new();
        public List<string> activeNgPlusBoons { get; set; } = new();
    }

    /// <summary>
    /// C3 Plan 175: Meta Progression System.
    /// Pure domain authority managing profile prestige, unlockables (crests, insignias, NG+ boons),
    /// and cross-run milestones. Coordinates with <see cref="CrossRunProfileStore"/> and
    /// <see cref="CampaignCompletionHistory"/> without mutating run history or duplicating save state.
    /// </summary>
    public sealed class MetaProgressionSystem
    {
        public const int CurrentSchemaVersion = 1;

        private readonly MetaProgressionSaveState _state;
        private readonly Dictionary<string, MetaUnlockableDef> _catalog = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _unlockedIds = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _activeNgPlusBoons = new(StringComparer.OrdinalIgnoreCase);
        private CrossRunProfileStore _profileStore;

        public CrossRunProfileStore ProfileStore => _profileStore;
        public int TotalPrestigeEarned => _state.totalPrestigeEarned;
        public IReadOnlyCollection<string> UnlockedIds => _unlockedIds;
        public IReadOnlyCollection<string> ActiveNgPlusBoons => _activeNgPlusBoons;

        public event Action<string>? OnItemUnlocked;

        public MetaProgressionSystem(CrossRunProfileStore? profileStore = null, MetaProgressionSaveState? state = null)
        {
            _profileStore = profileStore ?? new CrossRunProfileStore();
            _state = state ?? new MetaProgressionSaveState();
            SyncStateFromCollections();
        }

        public void BindProfileStore(CrossRunProfileStore store)
        {
            _profileStore = store ?? throw new ArgumentNullException(nameof(store));
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;

            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var cat = JsonSerializer.Deserialize<MetaUnlockablesCatalog>(json, options);
                if (cat?.unlockables == null) return;

                foreach (var item in cat.unlockables)
                {
                    if (!string.IsNullOrWhiteSpace(item.id))
                    {
                        _catalog[item.id] = item;
                    }
                }
            }
            catch (Exception)
            {
                // Catalog parse fallback; strictness enforced by data integrity gate
            }
        }

        public IReadOnlyCollection<MetaUnlockableDef> GetAllUnlockables() => _catalog.Values;

        public MetaUnlockableDef? GetUnlockable(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;
            return _catalog.TryGetValue(id, out var def) ? def : null;
        }

        public bool IsUnlocked(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            return _unlockedIds.Contains(id);
        }

        public bool IsBoonActive(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            return _activeNgPlusBoons.Contains(id);
        }

        public bool SetNgPlusBoonActive(string id, bool active)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            if (!_unlockedIds.Contains(id)) return false;
            if (!_catalog.TryGetValue(id, out var def) || !string.Equals(def.category, "ng_plus_boon", StringComparison.OrdinalIgnoreCase))
                return false;

            if (active)
            {
                _activeNgPlusBoons.Add(id);
            }
            else
            {
                _activeNgPlusBoons.Remove(id);
            }

            SyncStateToCollections();
            return true;
        }

        /// <summary>
        /// Evaluates run records from <see cref="CrossRunProfileStore"/> and optional achievements / endings
        /// to update total prestige score and unlock applicable items.
        /// </summary>
        public int EvaluateProgress(
            IReadOnlyCollection<string>? completedAchievementIds = null,
            IReadOnlyCollection<string>? completedEndingIds = null)
        {
            int prestige = 0;
            var achievedEndings = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (completedEndingIds != null)
            {
                foreach (var e in completedEndingIds)
                {
                    if (!string.IsNullOrWhiteSpace(e)) achievedEndings.Add(e);
                }
            }

            // Calculate prestige from profile run history
            foreach (var run in _profileStore.Runs)
            {
                if (!string.IsNullOrWhiteSpace(run.EndingId))
                {
                    achievedEndings.Add(run.EndingId);
                }

                // Days survived: 2 prestige per day
                prestige += Math.Max(0, run.DaysSurvived) * 2;

                // Living survivors: 5 prestige per survivor
                prestige += Math.Max(0, run.LivingDwellers) * 5;

                // Major milestones achieved
                if (run.GrandTreatySigned) prestige += 50;
                if (run.TempestDecommissioned) prestige += 50;
                if (run.DebtLedgersBurned) prestige += 50;
                if (run.ChildrenSurvived) prestige += 50;
                if (run.VelSecretExposed) prestige += 50;
            }

            _state.totalPrestigeEarned = Math.Max(_state.totalPrestigeEarned, prestige);

            var achievementsSet = completedAchievementIds != null
                ? new HashSet<string>(completedAchievementIds, StringComparer.OrdinalIgnoreCase)
                : new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Unlock eligible items
            foreach (var item in _catalog.Values)
            {
                if (_unlockedIds.Contains(item.id)) continue;

                if (_state.totalPrestigeEarned < item.required_prestige)
                    continue;

                if (!string.IsNullOrWhiteSpace(item.required_ending_id) && !achievedEndings.Contains(item.required_ending_id))
                    continue;

                if (!string.IsNullOrWhiteSpace(item.required_achievement_id) && !achievementsSet.Contains(item.required_achievement_id))
                    continue;

                _unlockedIds.Add(item.id);
                OnItemUnlocked?.Invoke(item.id);
            }

            SyncStateToCollections();
            return _state.totalPrestigeEarned;
        }

        public MetaProgressionCensus GetCensus()
        {
            return new MetaProgressionCensus(
                _catalog.Count,
                _unlockedIds.Count,
                _activeNgPlusBoons.Count,
                _state.totalPrestigeEarned,
                _profileStore.TotalRunsCompleted);
        }

        public MetaProgressionSaveState CaptureState()
        {
            SyncStateToCollections();
            return new MetaProgressionSaveState
            {
                schema_version = CurrentSchemaVersion,
                totalPrestigeEarned = _state.totalPrestigeEarned,
                unlockedIds = new List<string>(_unlockedIds),
                activeNgPlusBoons = new List<string>(_activeNgPlusBoons)
            };
        }

        public void RestoreState(MetaProgressionSaveState? state)
        {
            if (state == null) return;

            _state.schema_version = state.schema_version;
            _state.totalPrestigeEarned = Math.Max(0, state.totalPrestigeEarned);
            _unlockedIds.Clear();
            _activeNgPlusBoons.Clear();

            if (state.unlockedIds != null)
            {
                foreach (var id in state.unlockedIds)
                {
                    if (!string.IsNullOrWhiteSpace(id)) _unlockedIds.Add(id);
                }
            }

            if (state.activeNgPlusBoons != null)
            {
                foreach (var id in state.activeNgPlusBoons)
                {
                    if (!string.IsNullOrWhiteSpace(id)) _activeNgPlusBoons.Add(id);
                }
            }

            SyncStateToCollections();
        }

        private void SyncStateFromCollections()
        {
            _unlockedIds.Clear();
            if (_state.unlockedIds != null)
            {
                foreach (var id in _state.unlockedIds)
                {
                    if (!string.IsNullOrWhiteSpace(id)) _unlockedIds.Add(id);
                }
            }

            _activeNgPlusBoons.Clear();
            if (_state.activeNgPlusBoons != null)
            {
                foreach (var id in _state.activeNgPlusBoons)
                {
                    if (!string.IsNullOrWhiteSpace(id)) _activeNgPlusBoons.Add(id);
                }
            }
        }

        private void SyncStateToCollections()
        {
            _state.unlockedIds = new List<string>(_unlockedIds);
            _state.activeNgPlusBoons = new List<string>(_activeNgPlusBoons);
        }
    }

    /// <summary>
    /// Read-only census of live meta-progression state (Plan 175). Exposed for
    /// the architecture scanner and the host self-test probe.
    /// </summary>
    public struct MetaProgressionCensus
    {
        public int TotalCatalogItems { get; }
        public int TotalUnlocked { get; }
        public int ActiveBoons { get; }
        public int PrestigeScore { get; }
        public int RunsRecorded { get; }

        public MetaProgressionCensus(
            int totalCatalogItems,
            int totalUnlocked,
            int activeBoons,
            int prestigeScore,
            int runsRecorded)
        {
            TotalCatalogItems = totalCatalogItems;
            TotalUnlocked = totalUnlocked;
            ActiveBoons = activeBoons;
            PrestigeScore = prestigeScore;
            RunsRecorded = runsRecorded;
        }
    }
}
