// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 32 — The Wild
// Subsystem    : Wildlife Harvest Quota Ledger (stateful owner over the signed
//                pure WildlifeHarvestQuotaEngine, DEC-86)
// ============================================================================
using System;
using System.Collections.Generic;

namespace Ashfall.Core.World
{
    /// <summary>
    /// Per-species seasonal harvest ledger record. It records what the shelter has
    /// taken and the last evaluated quota; it never stores the population itself
    /// (WildlifeMigrationSystem remains the population store).
    /// </summary>
    [Serializable]
    public sealed class SpeciesHarvestRecord
    {
        public string SpeciesId { get; set; } = string.Empty;
        public int HarvestTakenThisSeason { get; set; }
        public int LastMaxSafeQuota { get; set; }
        public int LastPopulationPermille { get; set; }
        public SpeciesPopulationBand LastPostHarvestBand { get; set; } = SpeciesPopulationBand.Stable;
    }

    /// <summary>Persisted harvest ledger, reset at each season boundary.</summary>
    [Serializable]
    public sealed class WildlifeHarvestState
    {
        public int SchemaVersion { get; set; } = 1;
        public int Season { get; set; }
        public Dictionary<string, SpeciesHarvestRecord> Species { get; set; } =
            new Dictionary<string, SpeciesHarvestRecord>(StringComparer.Ordinal);

        public WildlifeHarvestState Clone() => new WildlifeHarvestState
        {
            SchemaVersion = SchemaVersion,
            Season = Season,
            Species = new Dictionary<string, SpeciesHarvestRecord>(Species, StringComparer.Ordinal)
        };
    }

    /// <summary>Bounded read model of the harvest ledger.</summary>
    public struct WildlifeHarvestCensus
    {
        public int TrackedSpecies { get; }
        public int SpeciesAtRisk { get; }
        public int SpeciesOverQuota { get; }
        public int TotalHarvestTaken { get; }
        public int Season { get; }

        public WildlifeHarvestCensus(int trackedSpecies, int speciesAtRisk, int speciesOverQuota, int totalHarvestTaken, int season)
        {
            TrackedSpecies = trackedSpecies;
            SpeciesAtRisk = speciesAtRisk;
            SpeciesOverQuota = speciesOverQuota;
            TotalHarvestTaken = totalHarvestTaken;
            Season = season;
        }
    }

    /// <summary>
    /// Owns the mutable harvest ledger and delegates all quota/conflict/taming
    /// arithmetic to the signed pure <see cref="WildlifeHarvestQuotaEngine"/>.
    /// Populations, extinction flags, and trap sites remain with their canonical owners.
    /// </summary>
    public sealed class WildlifeHarvestLedger
    {
        private readonly WildlifeHarvestState _state;

        public WildlifeHarvestLedger(WildlifeHarvestState? state = null)
        {
            _state = state ?? new WildlifeHarvestState();
        }

        public IReadOnlyDictionary<string, SpeciesHarvestRecord> Species => _state.Species;
        public int Season => _state.Season;
        public int TrackedSpecies => _state.Species.Count;

        public WildlifeHarvestState CaptureState() => _state.Clone();

        public void RestoreState(WildlifeHarvestState? saved)
        {
            if (saved == null) return;
            if (saved.SchemaVersion > _state.SchemaVersion)
                throw new InvalidOperationException(
                    $"wildlife harvest schema {saved.SchemaVersion} is newer than supported {_state.SchemaVersion}.");

            _state.SchemaVersion = saved.SchemaVersion <= 0 ? _state.SchemaVersion : saved.SchemaVersion;
            _state.Season = saved.Season;
            _state.Species = saved.Species != null
                ? new Dictionary<string, SpeciesHarvestRecord>(saved.Species, StringComparer.Ordinal)
                : new Dictionary<string, SpeciesHarvestRecord>(StringComparer.Ordinal);
        }

        private SpeciesHarvestRecord Ensure(string speciesId)
        {
            if (string.IsNullOrWhiteSpace(speciesId)) throw new ArgumentException("species id required", nameof(speciesId));
            if (!_state.Species.TryGetValue(speciesId, out var record))
            {
                record = new SpeciesHarvestRecord { SpeciesId = speciesId };
                _state.Species[speciesId] = record;
            }
            return record;
        }

        /// <summary>Read-only quota evaluation. Never records a harvest.</summary>
        public HarvestQuotaResult EvaluateQuota(string speciesId, int populationPermille, int reproductionPermille, int requestedUnits)
        {
            var result = WildlifeHarvestQuotaEngine.EvaluateHarvestQuota(populationPermille, reproductionPermille, requestedUnits);
            var record = Ensure(speciesId);
            record.LastMaxSafeQuota = result.MaxSafeHarvestUnits;
            record.LastPopulationPermille = Math.Clamp(populationPermille, 0, 1000);
            record.LastPostHarvestBand = result.PostHarvestBand;
            return result;
        }

        /// <summary>
        /// Applies a harvest (even an over-hunt) against the ledger and returns the
        /// quota result so the caller sees the collapse risk it accepted.
        /// </summary>
        public HarvestQuotaResult ApplyHarvest(string speciesId, int populationPermille, int reproductionPermille, int requestedUnits)
        {
            var result = EvaluateQuota(speciesId, populationPermille, reproductionPermille, requestedUnits);
            if (requestedUnits > 0)
                Ensure(speciesId).HarvestTakenThisSeason += requestedUnits;
            return result;
        }

        /// <summary>Resets per-season harvest counters. Species records are retained.</summary>
        public void BeginSeason(int season)
        {
            _state.Season = season;
            foreach (var record in _state.Species.Values)
                record.HarvestTakenThisSeason = 0;
        }

        /// <summary>Read-only predator conflict posture.</summary>
        public PredatorConflictPosture EvaluatePredatorConflict(int predatorPopulationPermille, int proximityMetres, int shelterNoisePermille) =>
            WildlifeHarvestQuotaEngine.EvaluatePredatorConflict(predatorPopulationPermille, proximityMetres, shelterNoisePermille);

        /// <summary>Read-only deterministic taming readiness.</summary>
        public TamingReadinessResult EvaluateTamingReadiness(int animalHungerPermille, int trustExposurePermille, int speciesTamabilityPermille, int tamingSeed) =>
            WildlifeHarvestQuotaEngine.EvaluateTamingReadiness(animalHungerPermille, trustExposurePermille, speciesTamabilityPermille, tamingSeed);

        public bool TryGetSpecies(string speciesId, out SpeciesHarvestRecord record) =>
            _state.Species.TryGetValue(speciesId ?? string.Empty, out record!);

        public WildlifeHarvestCensus GetCensus()
        {
            int atRisk = 0, overQuota = 0, total = 0;
            foreach (var record in _state.Species.Values)
            {
                if (record.LastPostHarvestBand <= SpeciesPopulationBand.Depleted) atRisk++;
                if (record.LastMaxSafeQuota > 0 && record.HarvestTakenThisSeason > record.LastMaxSafeQuota) overQuota++;
                total += record.HarvestTakenThisSeason;
            }
            return new WildlifeHarvestCensus(_state.Species.Count, atRisk, overQuota, total, _state.Season);
        }

        public void Clear() => _state.Species.Clear();
    }
}
