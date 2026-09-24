// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : AgingSaveStore
// Core State : Ashfall.Core.Survivors.AgingState
// Host Caller: Main.Aging (SetupAging / SaveAging)
// Purpose    : Plan 176 — Aging & elderly survivor system: chronological age
//              progression, life stages, retirement, elder mentorship, and milestones.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class AgingSaveStore
    {
        public const string FileName = "aging_save.json";
        public const string SectionName = "aging";

        private static readonly SaveStore<AgingState> s_store =
            SaveStoreHub.Checksummed<AgingState>(FileName, nameof(AgingSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(AgingState state) => s_store.CaptureBare(state);
        public static AgingState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(AgingState state) => s_store.TrySave(state);
        public static AgingState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Host session manager for Plan 176 (Aging & Elderly Survivor System).
    /// Binds life stages and milestones from life_stages.json, evaluates age profiles,
    /// manages dignified retirement, provides elder mentorship checks, and saves/restores state.
    /// </summary>
    public sealed class AgingHostSession : HostSessionBase
    {
        private readonly AgingSystem _system;
        private string _lastEvent = string.Empty;

        public AgingSystem System => _system;
        public string LastEvent => _lastEvent;
        public AgingCensus Census => _system.GetCensus();
        public int TrackedSurvivorCount => _system.TrackedSurvivorCount;
        public int RetiredSurvivorCount => _system.RetiredSurvivorCount;
        public int DaysPerYear => _system.DaysPerYear;
        public int MinRetirementAgeYears => _system.MinRetirementAgeYears;
        public IReadOnlyList<LifeStageDef> LifeStages => _system.GetAllLifeStages();
        public IReadOnlyList<AgingMilestoneDef> Milestones => _system.GetAllMilestones();

        public AgingHostSession(
            string? dataDir = null,
            AgingSystem? system = null)
        {
            _system = system ?? new AgingSystem();

            if (!string.IsNullOrEmpty(dataDir))
            {
                LoadCatalogs(dataDir);
            }

            _system.OnSurvivorRetired += (id, day) =>
            {
                _lastEvent = $"Survivor '{id}' retired on day {day}.";
                RaiseStateChanged();
            };
            _system.OnMilestoneReached += (id, age, label) =>
            {
                _lastEvent = $"Survivor '{id}' reached milestone age {age}: {label}.";
                RaiseStateChanged();
            };
            _system.OnStageTransitioned += (id, oldStage, newStage) =>
            {
                _lastEvent = $"Survivor '{id}' transitioned from {oldStage} to {newStage}.";
                RaiseStateChanged();
            };
        }

        public static AgingHostSession Create(
            string? dataDir = null,
            AgingSystem? system = null)
        {
            return new AgingHostSession(dataDir, system);
        }

        public void LoadCatalogs(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;

            try
            {
                var result = LifeStagesCatalogLoader.Load(dataDir);
                if (result.Success && result.Catalog != null)
                {
                    _system.BindValidatedCatalog(result.Catalog);
                    _lastEvent = $"Loaded {result.Catalog.life_stages.Count} life stages and {result.Catalog.milestones.Count} milestones.";
                    RaiseStateChanged();
                }
                else if (result.Errors.Count > 0)
                {
                    _lastEvent = $"Failed to load life stages catalog: {string.Join("; ", result.Errors)}";
                }
            }
            catch (Exception ex)
            {
                _lastEvent = $"Exception loading life stages catalog: {ex.Message}";
            }
        }

        public SurvivorAgingRecord RegisterSurvivor(string survivorId, int baseAgeYears, int joinedDay)
        {
            var record = _system.RegisterSurvivor(survivorId, baseAgeYears, joinedDay);
            RaiseStateChanged();
            return record;
        }

        public SurvivorAgeProfile EvaluateSurvivor(string survivorId, int currentDay)
        {
            return _system.EvaluateSurvivor(survivorId, currentDay);
        }

        public bool IsRetired(string survivorId) => _system.IsRetired(survivorId);

        public bool RetireSurvivor(string survivorId, int currentDay)
        {
            return _system.RetireSurvivor(survivorId, currentDay);
        }

        public void AdvanceDay(int day, IEnumerable<string>? livingSurvivorIds = null)
        {
            _system.TickDay(day, livingSurvivorIds);
            var census = _system.GetCensus();
            _lastEvent = $"Day {day} aging simulation advanced. Tracked: {census.TotalTrackedSurvivors}, Retired: {census.RetiredSurvivors}, Elderly: {census.ElderlySurvivors}.";
            RaiseStateChanged();
        }

        public bool HasLivingElderMentor(int currentDay, IEnumerable<string> livingSurvivorIds)
        {
            return _system.HasLivingElderMentor(currentDay, livingSurvivorIds);
        }

        public AgingState CaptureState() => _system.CaptureState();

        public void RestoreState(AgingState? state)
        {
            if (state == null) return;
            _system.RestoreState(state);
            _lastEvent = $"Restored aging state with {state.Records?.Count ?? 0} survivor records.";
            RaiseStateChanged();
        }
    }
}
