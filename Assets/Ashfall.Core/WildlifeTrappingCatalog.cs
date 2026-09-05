using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core
{
    /// <summary>
    /// Trap definition from the wildlife trapping catalog.
    /// Design intent only — the existing WildlifeTrappingSystem handles catch resolution.
    /// </summary>
    [Serializable]
    public sealed class TrapDefinition
    {
        public string trap_id = string.Empty;
        public string displayName = string.Empty;
        public string description = string.Empty;
        public string trapType = "snare";
        public List<TrapSetupCost> setupCosts = new List<TrapSetupCost>();
        public int checkIntervalDays = 2;
        public int durabilityChecks = 8;
        public float baseCatchModifier = 1.0f;
        public List<string> compatiblePrey = new List<string>();
        public bool requiresWater = false;
        public float weatherSensitivity = 0.0f;
        public float bycatchChance = 0f; // Plan 36 III: probability of bycatch on successful catch
        public List<BycatchCandidate> bycatchSpecies = new List<BycatchCandidate>(); // Plan 36 III: weighted bycatch pool
    }

    [Serializable]
    public sealed class TrapSetupCost
    {
        public string itemId = string.Empty;
        public int amount = 1;
    }

    /// <summary>
    /// Plan 36 III: Weighted bycatch candidate for trap definitions.
    /// </summary>
    [Serializable]
    public sealed class BycatchCandidate
    {
        public string speciesId = string.Empty;
        public float weight = 1.0f;
    }

    /// <summary>
    /// Prey definition from the wildlife trapping catalog.
    /// Maps to QuarrySpecies for registration with WildlifeTrappingSystem.
    /// Includes ecology fields for seasonal/migration integration.
    /// </summary>
    [Serializable]
    public sealed class PreyDefinition
    {
        public string speciesId = string.Empty;
        public string displayName = string.Empty;
        public string description = string.Empty;
        public float baseYieldKg = 1.0f;
        public float toxicChance = 0.2f;
        public float hideYield = 0.0f;
        public string hideItemId = string.Empty;
        public string preferredTrapType = "snare";
        public List<string> attractedByBaitIds = new List<string>();
        public float minSkillLevel = 0.0f;
        public string migrationSpeciesId = string.Empty;
        public List<string> activeSeasons = new List<string>();
        public float diseaseRisk = 0.1f;
        public float contaminationRisk = 0.05f;
        public string diseaseId = string.Empty; // Plan 36 Closure II: per-species disease mapping
        public float contaminationDose = 0f; // Plan 36 Closure II: explicit contamination dose in rads

        public const string FallbackDiseaseId = "disease_zoonotic_flu";

        /// <summary>
        /// Resolve disease ID: explicit per-species mapping wins, otherwise tier fallback.
        /// Low risk (≤0.1) → no disease; medium/high (>0.1) → fallback wildlife disease ("disease_zoonotic_flu").
        /// </summary>
        public string ResolveDiseaseId() => ResolveDiseaseId(this);

        public static string ResolveDiseaseId(PreyDefinition? prey)
        {
            if (prey == null) return string.Empty;
            if (!string.IsNullOrEmpty(prey.diseaseId))
                return prey.diseaseId;
            if (prey.diseaseRisk <= 0.1f)
                return string.Empty;
            return FallbackDiseaseId;
        }

        public static string ResolveDiseaseId(float diseaseRisk, string? explicitDiseaseId = null)
        {
            if (!string.IsNullOrEmpty(explicitDiseaseId))
                return explicitDiseaseId;
            if (diseaseRisk <= 0.1f)
                return string.Empty;
            return FallbackDiseaseId;
        }
    }

    [Serializable]
    internal sealed class WildlifeTrappingCatalogFileRaw
    {
        public int schema_version = 1;
        public List<TrapDefinition> traps = new List<TrapDefinition>();
        public List<PreyDefinition> prey = new List<PreyDefinition>();
        public List<BaitProfile> baits = new List<BaitProfile>();
    }

    /// <summary>
    /// Loader for wildlife_trapping_catalog.json.
    /// Engine-agnostic: IFileIO + IJsonSerializer ports.
    /// Missing file returns null (silent-empty).
    /// </summary>
    public static class WildlifeTrappingCatalogLoader
    {
        public const string FileName = "wildlife_trapping_catalog.json";

        public static WildlifeTrappingCatalog? Load(
            string dataDir, IFileIO fileIO, IJsonSerializer json, ILog? log = null)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir)) return null;
            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path)) return null;
            try
            {
                string raw = fileIO.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(raw)) return null;
                var file = json.Deserialize<WildlifeTrappingCatalogFileRaw>(raw);
                if (file == null) return null;
                return new WildlifeTrappingCatalog(file.traps, file.prey, file.baits);
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn(path, "WildlifeTrappingCatalogFileRaw", ex);
                return null;
            }
        }
    }

    /// <summary>
    /// Loaded wildlife trapping catalog with trap definitions, prey definitions, and bait profiles.
    /// Provides registration with WildlifeTrappingSystem.
    /// </summary>
    public sealed class WildlifeTrappingCatalog
    {
        private readonly Dictionary<string, TrapDefinition> _traps = new Dictionary<string, TrapDefinition>();
        private readonly Dictionary<string, PreyDefinition> _prey = new Dictionary<string, PreyDefinition>();
        private readonly Dictionary<string, BaitProfile> _baits = new Dictionary<string, BaitProfile>();

        public WildlifeTrappingCatalog(
            List<TrapDefinition>? traps, List<PreyDefinition>? prey, List<BaitProfile>? baits)
        {
            if (traps != null)
                foreach (var t in traps)
                    if (t != null && !string.IsNullOrEmpty(t.trap_id))
                        _traps[t.trap_id] = t;
            if (prey != null)
                foreach (var p in prey)
                    if (p != null && !string.IsNullOrEmpty(p.speciesId))
                        _prey[p.speciesId] = p;
            if (baits != null)
                foreach (var b in baits)
                    if (b != null && !string.IsNullOrEmpty(b.baitId))
                        _baits[b.baitId] = b;
        }

        public IReadOnlyDictionary<string, TrapDefinition> Traps => _traps;
        public IReadOnlyDictionary<string, PreyDefinition> Prey => _prey;
        public IReadOnlyDictionary<string, BaitProfile> Baits => _baits;

        /// <summary>
        /// Register all trap, prey, and bait entries with the WildlifeTrappingSystem.
        /// Prey are converted to QuarrySpecies and also registered as PreyDefinitions
        /// for season/migration filtering. Baits and trap definitions are registered directly.
        /// </summary>
        public void RegisterWith(WildlifeTrappingSystem system)
        {
            if (system == null) return;
            // Plan 36 III: register trap definitions for bycatch/durability lookup
            foreach (var t in _traps.Values)
                system.RegisterTrapDefinition(t);
            foreach (var p in _prey.Values)
            {
                system.RegisterQuarry(new QuarrySpecies
                {
                    speciesId = p.speciesId,
                    displayName = p.displayName,
                    baseYieldKg = p.baseYieldKg,
                    toxicChance = p.toxicChance,
                    hideYield = p.hideYield,
                    hideItemId = p.hideItemId,
                    preferredTrapType = p.preferredTrapType,
                    attractedByBaitIds = new List<string>(p.attractedByBaitIds),
                    minSkillLevel = p.minSkillLevel
                });
                // Plan 36: also register prey definition for season/migration filtering
                system.RegisterPreyDefinition(p);
            }
            foreach (var b in _baits.Values)
            {
                system.RegisterBait(b);
            }
        }
    }
}
