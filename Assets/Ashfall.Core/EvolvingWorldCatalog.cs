using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core
{
    // ── Wire shapes for world_evolution_seeds.json (the authority) ──────

    [Serializable]
    public sealed class EvolvingWorldSeedContainer
    {
        public int schema_version = 1;
        public string collection_id = string.Empty;
        public string description = string.Empty;
        public string shelter_sector_id = string.Empty;
        public List<string> scarcity_goods = new List<string>();
        public List<SectorSeedRecord> sectors = new List<SectorSeedRecord>();
        public List<PackSeedRecord> packs = new List<PackSeedRecord>();
        public List<LandmarkSeedRecord> landmarks = new List<LandmarkSeedRecord>();
        public List<LocationSeedRecord> location_seeds = new List<LocationSeedRecord>();
    }

    [Serializable]
    public sealed class SectorSeedRecord
    {
        public string sector_id = string.Empty;
        public List<string> neighbors = new List<string>();
    }

    [Serializable]
    public sealed class PackSeedRecord
    {
        public string pack_id = string.Empty;
        public string species_id = string.Empty;
        public string sector_id = string.Empty;
        public int population = 5;
    }

    [Serializable]
    public sealed class LandmarkSeedRecord
    {
        public string landmark_id = string.Empty;
        public string location_id = string.Empty;
        public float integrity = 100f;
    }

    [Serializable]
    public sealed class LocationSeedRecord
    {
        public string location_id = string.Empty;
        public string owner = "none";
        public float contamination;
        public List<string> threats = new List<string>();
    }

    /// <summary>Loads the seed catalog. Engine-agnostic: IFileIO + IJsonSerializer ports.</summary>
    public static class EvolvingWorldCatalogLoader
    {
        public const string DefaultFileName = "world_evolution_seeds.json";

        public static EvolvingWorldSeedContainer? Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return null;

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return null;

            string rawText = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(rawText))
                return null;

            return json.Deserialize<EvolvingWorldSeedContainer>(rawText);
        }
    }

    /// <summary>
    /// One-time deterministic seeding of the evolving-world systems from the
    /// seed catalog. Idempotent by design: every step checks the live state
    /// first, so calling this after a restored save is a no-op and a fresh
    /// boot always converges on the same starting world.
    /// </summary>
    public static class EvolvingWorldSeeder
    {
        public static void Seed(
            LocationEvolutionSystem locationEvolution,
            WildlifeMigrationSystem wildlife,
            LandmarkDegradationSystem landmarks,
            EvolvingWorldSeedContainer? catalog)
        {
            if (catalog == null) return;

            // Wildlife: sector graph first (migration needs it), then packs.
            if (wildlife != null)
            {
                wildlife.SetSectorAdjacency(catalog.sectors?
                    .Where(s => s != null)
                    .Select(s => (s.sector_id, new List<string>(s.neighbors ?? new List<string>())))
                    ?? Enumerable.Empty<(string, List<string>)>());

                if (wildlife.State.packs.Count == 0 && catalog.packs != null)
                {
                    foreach (var pack in catalog.packs)
                    {
                        if (pack == null || string.IsNullOrEmpty(pack.pack_id)) continue;
                        wildlife.RegisterPack(pack.pack_id, pack.species_id, pack.sector_id, pack.population);
                    }
                }
            }

            // Landmarks: register only when the ledger is empty.
            if (landmarks != null && landmarks.State.landmarks.Count == 0 && catalog.landmarks != null)
            {
                foreach (var lm in catalog.landmarks)
                {
                    if (lm == null || string.IsNullOrEmpty(lm.landmark_id)) continue;
                    landmarks.RegisterLandmark(lm.landmark_id, lm.location_id, lm.integrity);
                }
            }

            // Locations: seed a record only when the system has never seen it,
            // so a restored save (or a player-touched location) is never overwritten.
            if (locationEvolution != null && catalog.location_seeds != null)
            {
                foreach (var seed in catalog.location_seeds)
                {
                    if (seed == null || string.IsNullOrEmpty(seed.location_id)) continue;
                    if (locationEvolution.TryGetRecord(seed.location_id) != null) continue;

                    var record = locationEvolution.GetOrCreateRecord(seed.location_id);
                    if (record == null) continue;
                    record.currentOwner = string.IsNullOrEmpty(seed.owner) ? "none" : seed.owner;
                    record.contaminationLevel = seed.contamination;
                    if (seed.threats != null)
                    {
                        record.activeThreats.AddRange(
                            seed.threats.Where(t => !string.IsNullOrEmpty(t) && !record.activeThreats.Contains(t)));
                    }
                }
            }
        }

        /// <summary>Convenience: the sector the shelter draws its trapping density from.</summary>
        public static string ShelterSectorId(EvolvingWorldSeedContainer? catalog)
            => string.IsNullOrEmpty(catalog?.shelter_sector_id) ? string.Empty : catalog!.shelter_sector_id;

        /// <summary>Goods whose demand shifts with wildlife pressure (e.g. preserved protein).</summary>
        public static List<string> ScarcityGoods(EvolvingWorldSeedContainer? catalog)
            => catalog?.scarcity_goods?.Where(g => !string.IsNullOrEmpty(g)).ToList() ?? new List<string>();
    }
}
