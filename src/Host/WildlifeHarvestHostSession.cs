// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : WildlifeHarvestSaveStore
// Core State : Ashfall.Core.World.WildlifeHarvestState
// Host Caller: Main.WildlifeHarvest
// Purpose    : Expansion 32 — per-species seasonal harvest ledger and quota
//              evaluation. Populations/extinction remain with their owners.
// ============================================================================

using System;
using Ashfall.Core.Save;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public static class WildlifeHarvestSaveStore
    {
        public const string FileName = "wildlife_harvest_save.json";
        public const string SectionName = "wildlife_harvest";

        private static readonly SaveStore<WildlifeHarvestState> s_store =
            SaveStoreHub.Checksummed<WildlifeHarvestState>(FileName, nameof(WildlifeHarvestSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(WildlifeHarvestState state) => s_store.CaptureBare(state);
        public static WildlifeHarvestState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(WildlifeHarvestState state) => s_store.TrySave(state);
        public static WildlifeHarvestState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Expansion 32 host session. Wraps the stateful
    /// <see cref="WildlifeHarvestLedger"/> over the signed pure
    /// <see cref="WildlifeHarvestQuotaEngine"/>. Population state, extinction flags,
    /// and trap sites stay with WildlifeMigrationSystem / WildlifeEcosystemSystem /
    /// WildlifeTrappingSystem.
    /// </summary>
    public sealed class WildlifeHarvestHostSession : HostSessionBase
    {
        private readonly WildlifeHarvestLedger _ledger;

        public WildlifeHarvestLedger Ledger => _ledger;
        public WildlifeHarvestCensus Census => _ledger.GetCensus();
        public int Season => _ledger.Season;
        public string LastEvent { get; private set; } = string.Empty;

        public WildlifeHarvestHostSession(WildlifeHarvestState? state = null)
        {
            _ledger = new WildlifeHarvestLedger(state);
        }

        public static WildlifeHarvestHostSession Create(WildlifeHarvestState? state = null) =>
            new WildlifeHarvestHostSession(state);

        public HarvestQuotaResult EvaluateQuota(string speciesId, int populationPermille, int reproductionPermille, int requestedUnits) =>
            _ledger.EvaluateQuota(speciesId, populationPermille, reproductionPermille, requestedUnits);

        public HarvestQuotaResult ApplyHarvest(string speciesId, int populationPermille, int reproductionPermille, int requestedUnits)
        {
            var result = _ledger.ApplyHarvest(speciesId, populationPermille, reproductionPermille, requestedUnits);
            LastEvent = result.IsWithinQuota
                ? $"Harvested {requestedUnits} of '{speciesId}' within the safe quota ({result.MaxSafeHarvestUnits})."
                : $"Over-hunted '{speciesId}': {requestedUnits} requested, {result.MaxSafeHarvestUnits} safe (collapse risk {result.OverhuntCollapseRiskPermille}\u2030).";
            RaiseStateChanged();
            return result;
        }

        public void BeginSeason(int season)
        {
            _ledger.BeginSeason(season);
            LastEvent = $"Wildlife harvest ledger opened season {season}.";
            RaiseStateChanged();
        }

        public PredatorConflictPosture EvaluatePredatorConflict(int predatorPopulationPermille, int proximityMetres, int shelterNoisePermille) =>
            _ledger.EvaluatePredatorConflict(predatorPopulationPermille, proximityMetres, shelterNoisePermille);

        public TamingReadinessResult EvaluateTamingReadiness(int animalHungerPermille, int trustExposurePermille, int speciesTamabilityPermille, int tamingSeed) =>
            _ledger.EvaluateTamingReadiness(animalHungerPermille, trustExposurePermille, speciesTamabilityPermille, tamingSeed);

        public WildlifeHarvestState CaptureState() => _ledger.CaptureState();
        public void RestoreState(WildlifeHarvestState? state) => _ledger.RestoreState(state);
        public void Clear() => _ledger.Clear();
    }
}
