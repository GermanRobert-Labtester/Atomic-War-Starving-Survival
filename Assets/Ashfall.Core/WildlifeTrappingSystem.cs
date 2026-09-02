using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    [Serializable]
    public sealed class WildlifeTrappingState
    {
        public string systemId = WildlifeTrappingSystem.SystemId;
        public List<TrapSite> trapSites = new List<TrapSite>();
        public int totalCatch;
        public int totalToxicRemoved;
        public List<string> firstCatchLoggedSpeciesIds = new List<string>();
    }

    [Serializable]
    public sealed class TrapSite
    {
        public string siteId = string.Empty;
        public string assignedHunterId = string.Empty;
        public string baitType = string.Empty;
        public string trapType = "snare"; // snare, deadfall, cage, pit
        public string trapId = string.Empty; // Plan 36: catalog link
        public int setDay = -1;
        public int checkDay = -1;
        public int checkIntervalDays = 2;
        public int remainingDurability = -1; // -1 = legacy/untracked, >0 = operational, 0 = broken
        public bool isBroken; // Plan 36: trap cannot produce catches when true
        public bool hasCatch;
        public string catchSpecies = string.Empty;
        public string bycatchSpecies = string.Empty; // Plan 36 III: bycatch species if occurred
        public float carcassYield;
        public bool isToxic;
        public bool toxinRemoved;
        public bool isMeatProcessed;
        public bool hidePreserved;
    }

    /// <summary>
    /// Bait definition: each bait type attracts specific species with a weight bonus.
    /// </summary>
    [Serializable]
    public sealed class BaitProfile
    {
        public string baitId = string.Empty;
        public string displayName = string.Empty;
        public float catchBonusMultiplier = 1.0f; // multiplies base catch chance
        public float toxicReduction = 0.0f; // reduces toxic chance by this fraction
        public List<string> preferredSpecies = new List<string>();
        public int craftCostScrapMeat = 0;
        public int craftCostRoots = 0;
        public int craftCostChemicals = 0;
    }

    /// <summary>
    /// Quarry species definition with distinct yields, toxicity, and trap affinity.
    /// </summary>
    [Serializable]
    public sealed class QuarrySpecies
    {
        public string speciesId = string.Empty;
        public string displayName = string.Empty;
        public float baseYieldKg = 1.0f;
        public float toxicChance = 0.2f;
        public float hideYield = 0.0f;
        public string hideItemId = string.Empty;
        public string preferredTrapType = "snare";
        public List<string> attractedByBaitIds = new List<string>();
        public float minSkillLevel = 0.0f;
    }

    /// <summary>
    /// Plan 36: Immutable environment context for prey selection.
    /// Passed by the host to filter by season, migration presence, and abundance.
    /// WT-INT-01: Carries live WeatherSystem snapshot and per-hunter skill levels.
    /// </summary>
    public sealed class WildlifeSelectionContext
    {
        public static readonly WildlifeSelectionContext Default = new WildlifeSelectionContext();

        /// <summary>Current season window ID (e.g., "window_thaw"). Empty = unknown/all.</summary>
        public string SeasonWindowId { get; set; } = string.Empty;

        /// <summary>Current authoritative weather snapshot from WeatherSystem.</summary>
        public WeatherKind CurrentWeather { get; set; } = WeatherKind.Clear;

        /// <summary>Species IDs present in the current sector via migration.</summary>
        public HashSet<string> PresentMigrationSpecies { get; set; } = new HashSet<string>(StringComparer.Ordinal);

        /// <summary>Per-species abundance multiplier from seasonal calendar. Key = speciesId.</summary>
        public Dictionary<string, float> AbundanceFactors { get; set; } = new Dictionary<string, float>(StringComparer.Ordinal);

        /// <summary>Per-hunter normalized skill levels (0..100) from SkillProgressionSystem. Key = hunterId.</summary>
        public Dictionary<string, float> HunterSkillLevels { get; set; } = new Dictionary<string, float>(StringComparer.Ordinal);
    }

    public sealed class WildlifeTrappingSystem
    {
        public const string SystemId = "wildlife_trapping";
        private WildlifeTrappingState _state = new WildlifeTrappingState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private int _currentDay = 1;
        private float _hunterSkillLevel = 0.0f;
        private WildlifeSelectionContext _selectionContext = WildlifeSelectionContext.Default;

        // Bait and quarry catalogs
        private readonly Dictionary<string, BaitProfile> _baitCatalog = new Dictionary<string, BaitProfile>();
        private readonly Dictionary<string, QuarrySpecies> _quarryCatalog = new Dictionary<string, QuarrySpecies>();
        private readonly Dictionary<string, PreyDefinition> _preyDefinitionCatalog = new Dictionary<string, PreyDefinition>();
        private readonly Dictionary<string, TrapDefinition> _trapDefinitionCatalog = new Dictionary<string, TrapDefinition>();

        public WildlifeTrappingState State => _state;
        public event Action OnTrappingChanged;
        public event Action<string, string, string, bool> OnButcheryCompleted; // siteId, butcherId, species, isToxic
        public event Action<string, string> OnHidePreserved; // siteId, hideItemId
        /// <summary>WT-INT-01: Fired when a prey species is caught for the first time. Args: (speciesId, siteId, hunterId).</summary>
        public event Action<string, string, string>? OnNewSpeciesDiscovered;

        public WildlifeTrappingSystem(ISeededRng rng, ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
            InitializeDefaultProfiles();
        }

        /// <summary>
        /// Register bait and quarry profiles from the data authority.
        /// Call after construction if overriding defaults.
        /// </summary>
        public void RegisterBait(BaitProfile bait)
        {
            if (bait != null && !string.IsNullOrEmpty(bait.baitId))
                _baitCatalog[bait.baitId] = bait;
        }

        public void RegisterQuarry(QuarrySpecies species)
        {
            if (species != null && !string.IsNullOrEmpty(species.speciesId))
                _quarryCatalog[species.speciesId] = species;
        }

        /// <summary>Plan 36: Register prey definition with season/migration metadata.</summary>
        public void RegisterPreyDefinition(PreyDefinition prey)
        {
            if (prey != null && !string.IsNullOrEmpty(prey.speciesId))
                _preyDefinitionCatalog[prey.speciesId] = prey;
        }

        /// <summary>Plan 36 III: Register trap definition for bycatch/durability lookup.</summary>
        public void RegisterTrapDefinition(TrapDefinition trap)
        {
            if (trap != null && !string.IsNullOrEmpty(trap.trap_id))
                _trapDefinitionCatalog[trap.trap_id] = trap;
        }

        /// <summary>Set the global fallback hunter's skill level (0-100) for legacy/unassigned trap calculations.</summary>
        public void SetHunterSkill(float skillLevel)
        {
            _hunterSkillLevel = Math.Clamp(skillLevel, 0f, 100f);
        }

        /// <summary>Plan 36: Set the environment context for season/migration-aware prey selection.</summary>
        public void SetSelectionContext(WildlifeSelectionContext context)
        {
            _selectionContext = context ?? WildlifeSelectionContext.Default;
        }

        /// <summary>
        /// Pure static calculation of skill multiplier: 0 -> 0.5x, 50 -> 1.0x, 100 -> 1.5x.
        /// </summary>
        public static float SkillMultiplierFor(float skillLevel)
        {
            float clamped = Math.Clamp(skillLevel, 0f, 100f);
            return 0.5f + (clamped / 100f);
        }

        /// <summary>Get the effective catch chance multiplier from global fallback hunter skill.</summary>
        private float SkillMultiplier => SkillMultiplierFor(_hunterSkillLevel);

        /// <summary>
        /// Pure weather penalty mapping for all 22 WeatherKind values.
        /// Clear = 0.0, Rain = 0.3, FalloutStorm = 0.5, Blizzard = 0.8.
        /// </summary>
        public static float WeatherPenaltyFor(WeatherKind kind)
        {
            return kind switch
            {
                WeatherKind.Clear => 0.0f,
                WeatherKind.Overcast => 0.0f,
                WeatherKind.Silence => 0.0f,
                WeatherKind.FalseSpring => 0.0f,
                WeatherKind.SilentSpring => 0.0f,

                WeatherKind.Rain => 0.3f,
                WeatherKind.AlgaeBloom => 0.3f,

                WeatherKind.Ashfall => 0.4f,
                WeatherKind.BioFog => 0.4f,
                WeatherKind.ParticulateFog => 0.4f,
                WeatherKind.ThermalInversion => 0.4f,

                WeatherKind.FalloutStorm => 0.5f,
                WeatherKind.BlackRain => 0.5f,
                WeatherKind.BloodRain => 0.5f,
                WeatherKind.EMPStorm => 0.5f,
                WeatherKind.AshLightning => 0.5f,

                WeatherKind.Blizzard => 0.8f,
                WeatherKind.AcidSnow => 0.8f,
                WeatherKind.BlackSnow => 0.8f,
                WeatherKind.GlassStorm => 0.8f,
                WeatherKind.RadHail => 0.8f,
                WeatherKind.IceStorm => 0.8f,

                _ => 0.0f
            };
        }

        /// <summary>
        /// Pure weather multiplier calculation:
        /// 1 - clamp(weatherSensitivity, 0, 1) * clamp(weatherPenalty, 0, 1).
        /// </summary>
        public static float CalculateWeatherMultiplier(float weatherSensitivity, WeatherKind weather)
        {
            float sens = Math.Clamp(weatherSensitivity, 0f, 1f);
            float pen = Math.Clamp(WeatherPenaltyFor(weather), 0f, 1f);
            return 1.0f - (sens * pen);
        }

        /// <summary>
        /// Pure primary catch chance calculation.
        /// </summary>
        public static float CalculatePrimaryCatchChance(
            float densityMultiplier,
            float hunterSkillLevel,
            float baitMultiplier,
            float weatherSensitivity,
            WeatherKind weather)
        {
            float baseChance = BaseCatchChance * densityMultiplier;
            float skillMult = SkillMultiplierFor(hunterSkillLevel);
            float weatherMult = CalculateWeatherMultiplier(weatherSensitivity, weather);
            float rawChance = baseChance * skillMult * baitMultiplier * weatherMult;
            return Math.Clamp(rawChance, 0.05f, 0.95f);
        }

        /// <summary>
        /// Records a primary catch species if not previously caught. Returns true only on first catch.
        /// </summary>
        private bool TryRecordFirstCatch(string speciesId, string siteId, string hunterId)
        {
            if (string.IsNullOrEmpty(speciesId)) return false;
            if (_state.firstCatchLoggedSpeciesIds == null)
                _state.firstCatchLoggedSpeciesIds = new List<string>();

            for (int i = 0; i < _state.firstCatchLoggedSpeciesIds.Count; i++)
            {
                if (string.Equals(_state.firstCatchLoggedSpeciesIds[i], speciesId, StringComparison.Ordinal))
                    return false;
            }

            _state.firstCatchLoggedSpeciesIds.Add(speciesId);
            OnNewSpeciesDiscovered?.Invoke(speciesId, siteId ?? string.Empty, hunterId ?? string.Empty);
            return true;
        }

        private void InitializeDefaultProfiles()
        {
            // Default baits
            _baitCatalog["bait_scrap_meat"] = new BaitProfile
            {
                baitId = "bait_scrap_meat", displayName = "Scrap-Meat Bait",
                catchBonusMultiplier = 1.3f, toxicReduction = 0.0f,
                preferredSpecies = new List<string> { "rat", "fox" },
                craftCostScrapMeat = 1, craftCostRoots = 0, craftCostChemicals = 0
            };
            _baitCatalog["bait_grain_lure"] = new BaitProfile
            {
                baitId = "bait_grain_lure", displayName = "Grain Lure",
                catchBonusMultiplier = 1.5f, toxicReduction = 0.1f,
                preferredSpecies = new List<string> { "rabbit", "pheasant" },
                craftCostScrapMeat = 0, craftCostRoots = 2, craftCostChemicals = 0
            };
            _baitCatalog["bait_pheromone"] = new BaitProfile
            {
                baitId = "bait_pheromone", displayName = "Mutated-Beast Pheromone Lure",
                catchBonusMultiplier = 2.0f, toxicReduction = 0.0f,
                preferredSpecies = new List<string> { "molerat", "slag_beetle", "ash_crow" },
                craftCostScrapMeat = 2, craftCostRoots = 0, craftCostChemicals = 1
            };
            _baitCatalog["bait_fat_cake"] = new BaitProfile
            {
                baitId = "bait_fat_cake", displayName = "Rendered Fat Cake",
                catchBonusMultiplier = 1.8f, toxicReduction = 0.15f,
                preferredSpecies = new List<string> { "fox", "lynx", "wolf" },
                craftCostScrapMeat = 2, craftCostRoots = 0, craftCostChemicals = 0
            };
            _baitCatalog["bait_berry_mash"] = new BaitProfile
            {
                baitId = "bait_berry_mash", displayName = "Fermented Berry Mash",
                catchBonusMultiplier = 1.2f, toxicReduction = 0.2f,
                preferredSpecies = new List<string> { "rabbit", "pheasant", "deer" },
                craftCostScrapMeat = 0, craftCostRoots = 3, craftCostChemicals = 0
            };
            _baitCatalog["bait_salt_lick"] = new BaitProfile
            {
                baitId = "bait_salt_lick", displayName = "Mineral Salt Lick",
                catchBonusMultiplier = 1.6f, toxicReduction = 0.1f,
                preferredSpecies = new List<string> { "deer", "wolf", "boar" },
                craftCostScrapMeat = 0, craftCostRoots = 1, craftCostChemicals = 1
            };

            // Default quarry
            _quarryCatalog["rabbit"] = new QuarrySpecies
            {
                speciesId = "rabbit", displayName = "Ash Rabbit",
                baseYieldKg = 1.2f, toxicChance = 0.15f, hideYield = 0.3f,
                hideItemId = "leather_strap", preferredTrapType = "snare",
                attractedByBaitIds = new List<string> { "bait_grain_lure", "bait_berry_mash" },
                minSkillLevel = 0f
            };
            _quarryCatalog["rat"] = new QuarrySpecies
            {
                speciesId = "rat", displayName = "Irradiated Rat",
                baseYieldKg = 0.6f, toxicChance = 0.35f, hideYield = 0.0f,
                hideItemId = "", preferredTrapType = "snare",
                attractedByBaitIds = new List<string> { "bait_scrap_meat" },
                minSkillLevel = 0f
            };
            _quarryCatalog["fox"] = new QuarrySpecies
            {
                speciesId = "fox", displayName = "Barren Fox",
                baseYieldKg = 2.0f, toxicChance = 0.20f, hideYield = 0.5f,
                hideItemId = "leather_strap", preferredTrapType = "deadfall",
                attractedByBaitIds = new List<string> { "bait_scrap_meat", "bait_fat_cake" },
                minSkillLevel = 10f
            };
            _quarryCatalog["pheasant"] = new QuarrySpecies
            {
                speciesId = "pheasant", displayName = "Ash Pheasant",
                baseYieldKg = 1.5f, toxicChance = 0.10f, hideYield = 0.2f,
                hideItemId = "", preferredTrapType = "cage",
                attractedByBaitIds = new List<string> { "bait_grain_lure", "bait_berry_mash" },
                minSkillLevel = 5f
            };
            _quarryCatalog["molerat"] = new QuarrySpecies
            {
                speciesId = "molerat", displayName = "Tessarat Blind Mole-Rat",
                baseYieldKg = 3.0f, toxicChance = 0.25f, hideYield = 0.4f,
                hideItemId = "leather_strap", preferredTrapType = "pit",
                attractedByBaitIds = new List<string> { "bait_pheromone" },
                minSkillLevel = 20f
            };
            _quarryCatalog["slag_beetle"] = new QuarrySpecies
            {
                speciesId = "slag_beetle", displayName = "Titan Slag-Back Beetle",
                baseYieldKg = 4.0f, toxicChance = 0.40f, hideYield = 0.0f,
                hideItemId = "", preferredTrapType = "pit",
                attractedByBaitIds = new List<string> { "bait_pheromone", "bait_salt_lick" },
                minSkillLevel = 30f
            };
            _quarryCatalog["ash_crow"] = new QuarrySpecies
            {
                speciesId = "ash_crow", displayName = "Three-Eyed Sentry Crow",
                baseYieldKg = 0.8f, toxicChance = 0.15f, hideYield = 0.1f,
                hideItemId = "", preferredTrapType = "cage",
                attractedByBaitIds = new List<string> { "bait_pheromone", "bait_grain_lure" },
                minSkillLevel = 10f
            };
            _quarryCatalog["deer"] = new QuarrySpecies
            {
                speciesId = "deer", displayName = "Wasteland Mule Deer",
                baseYieldKg = 15.0f, toxicChance = 0.10f, hideYield = 2.0f,
                hideItemId = "leather_strap", preferredTrapType = "deadfall",
                attractedByBaitIds = new List<string> { "bait_berry_mash", "bait_salt_lick" },
                minSkillLevel = 40f
            };
            _quarryCatalog["wolf"] = new QuarrySpecies
            {
                speciesId = "wolf", displayName = "Two-Headed Steppe Wolf",
                baseYieldKg = 8.0f, toxicChance = 0.30f, hideYield = 1.5f,
                hideItemId = "leather_strap", preferredTrapType = "deadfall",
                attractedByBaitIds = new List<string> { "bait_fat_cake", "bait_salt_lick" },
                minSkillLevel = 50f
            };
            _quarryCatalog["boar"] = new QuarrySpecies
            {
                speciesId = "boar", displayName = "Razorback Boar",
                baseYieldKg = 12.0f, toxicChance = 0.25f, hideYield = 1.8f,
                hideItemId = "leather_strap", preferredTrapType = "pit",
                attractedByBaitIds = new List<string> { "bait_salt_lick", "bait_fat_cake" },
                minSkillLevel = 60f
            };
        }

        public ActionResult SetTrap(string siteId, string baitType, string hunterId, string trapType = "snare",
            string trapId = "", int checkIntervalDays = -1, int durabilityChecks = -1)
        {
            var existing = _state.trapSites.Find(s => s.siteId == siteId);
            int interval = checkIntervalDays > 0 ? checkIntervalDays : 2;
            if (existing != null)
            {
                if (!existing.hasCatch && existing.setDay > 0 && !existing.isBroken)
                    return ActionResult.Blocked("trap_active", "trapping.trap_active");
                existing.setDay = _currentDay;
                existing.checkDay = _currentDay + interval;
                existing.baitType = baitType;
                existing.trapType = trapType;
                existing.trapId = trapId ?? string.Empty;
                existing.checkIntervalDays = interval;
                existing.remainingDurability = durabilityChecks > 0 ? durabilityChecks : -1;
                existing.isBroken = false;
                existing.assignedHunterId = hunterId ?? string.Empty;
                existing.hasCatch = false;
            }
            else
            {
                _state.trapSites.Add(new TrapSite
                {
                    siteId = siteId, baitType = baitType, trapType = trapType,
                    trapId = trapId ?? string.Empty,
                    assignedHunterId = hunterId ?? string.Empty,
                    setDay = _currentDay, checkDay = _currentDay + interval,
                    checkIntervalDays = interval,
                    remainingDurability = durabilityChecks > 0 ? durabilityChecks : -1
                });
            }
            OnTrappingChanged?.Invoke();
            return ActionResult.Success("trapping.trap_set");
        }

        /// <summary>Baseline catch rate (Unity parity: 50%).</summary>
        public const float BaseCatchChance = 0.5f;

        /// <summary>
        /// Select a quarry species based on bait affinity, trap type, per-site hunter skill level,
        /// season, migration presence, and abundance. Returns the species ID or
        /// string.Empty if no eligible quarry.
        /// </summary>
        private string SelectQuarrySpecies(string baitType, string trapType, float hunterSkillLevel)
        {
            var candidates = new List<(string id, float weight)>();
            string seasonId = _selectionContext.SeasonWindowId;
            bool hasSeason = !string.IsNullOrEmpty(seasonId);
            bool hasMigration = _selectionContext.PresentMigrationSpecies.Count > 0;

            foreach (var kvp in _quarryCatalog)
            {
                var q = kvp.Value;
                if (hunterSkillLevel < q.minSkillLevel) continue;

                // Plan 36: season filter — prey with activeSeasons must include current season
                if (hasSeason && _preyDefinitionCatalog.TryGetValue(q.speciesId, out var preyDef)
                    && preyDef.activeSeasons.Count > 0)
                {
                    bool seasonMatch = false;
                    for (int i = 0; i < preyDef.activeSeasons.Count; i++)
                    {
                        if (string.Equals(preyDef.activeSeasons[i], seasonId, StringComparison.Ordinal))
                        { seasonMatch = true; break; }
                    }
                    if (!seasonMatch) continue;
                }

                // Plan 36: migration filter — prey with migrationSpeciesId must be present
                if (hasMigration && _preyDefinitionCatalog.TryGetValue(q.speciesId, out var preyDef2)
                    && !string.IsNullOrEmpty(preyDef2.migrationSpeciesId)
                    && !_selectionContext.PresentMigrationSpecies.Contains(preyDef2.migrationSpeciesId))
                    continue;

                float weight = 1.0f;

                // Bait affinity bonus
                if (q.attractedByBaitIds != null && q.attractedByBaitIds.Contains(baitType))
                    weight *= 2.5f;

                // Trap type affinity bonus
                if (q.preferredTrapType == trapType)
                    weight *= 1.8f;
                else if (trapType == "snare" && q.preferredTrapType != "snare")
                    weight *= 0.5f; // penalty for wrong trap type

                // Plan 36: seasonal abundance weighting
                if (_selectionContext.AbundanceFactors.TryGetValue(q.speciesId, out float abundance))
                    weight *= abundance;

                candidates.Add((q.speciesId, weight));
            }

            if (candidates.Count == 0)
                return "rabbit"; // fallback

            // Weighted random selection
            float totalWeight = 0f;
            foreach (var c in candidates) totalWeight += c.weight;
            float roll = (float)_rng.NextDouble() * totalWeight;
            float cumulative = 0f;
            foreach (var c in candidates)
            {
                cumulative += c.weight;
                if (roll <= cumulative) return c.id;
            }
            return candidates[candidates.Count - 1].id;
        }

        /// <summary>
        /// Baseline roll. <paramref name="densityMultiplier"/> scales the chance
        /// with live wildlife pressure — the sector pack population the migration
        /// system reports. 1.0 keeps the authored 50% rate; the result clamps to
        /// a believable band so empty ground still occasionally feeds a snare.
        /// WT-INT-01: Each site evaluates its assigned hunter skill and trap weather sensitivity.
        /// </summary>
        public ActionResult CheckTraps(float densityMultiplier = 1f)
        {
            int caught = 0;
            foreach (var site in _state.trapSites)
            {
                if (site.hasCatch || site.setDay < 0) continue;
                if (site.isBroken) continue; // Plan 36: broken traps produce no catches
                if (_currentDay < site.checkDay) continue;

                // Resolve per-site hunter skill
                float siteHunterSkill = _hunterSkillLevel; // legacy fallback
                if (!string.IsNullOrEmpty(site.assignedHunterId)
                    && _selectionContext.HunterSkillLevels != null
                    && _selectionContext.HunterSkillLevels.TryGetValue(site.assignedHunterId, out float projectedSkill))
                {
                    siteHunterSkill = projectedSkill;
                }

                // Resolve trap definition once per site
                TrapDefinition? trapDef = null;
                if (!string.IsNullOrEmpty(site.trapId))
                    _trapDefinitionCatalog.TryGetValue(site.trapId, out trapDef);

                float weatherSens = trapDef?.weatherSensitivity ?? 0f;

                // Apply bait bonus
                float baitMultiplier = 1.0f;
                float baitToxicReduction = 0.0f;
                if (!string.IsNullOrEmpty(site.baitType) && _baitCatalog.TryGetValue(site.baitType, out var bait))
                {
                    baitMultiplier = bait.catchBonusMultiplier;
                    baitToxicReduction = bait.toxicReduction;
                }

                float finalChance = CalculatePrimaryCatchChance(
                    densityMultiplier,
                    siteHunterSkill,
                    baitMultiplier,
                    weatherSens,
                    _selectionContext.CurrentWeather);

                if (_rng.NextDouble() < finalChance)
                {
                    // Select species based on bait, trap type, and per-site hunter skill
                    string speciesId = SelectQuarrySpecies(site.baitType, site.trapType, siteHunterSkill);
                    site.catchSpecies = speciesId;

                    // Get species data
                    if (_quarryCatalog.TryGetValue(speciesId, out var quarry))
                    {
                        site.carcassYield = quarry.baseYieldKg * (0.7f + (float)_rng.NextDouble() * 0.6f);
                        float toxicChance = Math.Max(0.01f, quarry.toxicChance - baitToxicReduction);
                        site.isToxic = _rng.NextDouble() < toxicChance;
                    }
                    else
                    {
                        site.carcassYield = 1f + (float)_rng.NextDouble() * 2f;
                        site.isToxic = _rng.NextDouble() < 0.2f;
                    }

                    site.hasCatch = true;
                    site.toxinRemoved = false;
                    site.isMeatProcessed = false;
                    site.hidePreserved = false;
                    site.bycatchSpecies = string.Empty;

                    // Plan 36 III: bycatch roll — deterministic, fixed RNG budget (unaffected by weather/skill)
                    if (trapDef != null
                        && trapDef.bycatchChance > 0f
                        && trapDef.bycatchSpecies != null && trapDef.bycatchSpecies.Count > 0
                        && _rng.NextDouble() < trapDef.bycatchChance)
                    {
                        // Select bycatch species excluding primary catch
                        float totalWeight = 0f;
                        for (int i = 0; i < trapDef.bycatchSpecies.Count; i++)
                        {
                            var bc = trapDef.bycatchSpecies[i];
                            if (bc != null && !string.IsNullOrEmpty(bc.speciesId)
                                && !string.Equals(bc.speciesId, site.catchSpecies, StringComparison.Ordinal))
                                totalWeight += bc.weight;
                        }
                        if (totalWeight > 0f)
                        {
                            float roll = (float)_rng.NextDouble() * totalWeight;
                            float cumulative = 0f;
                            for (int i = 0; i < trapDef.bycatchSpecies.Count; i++)
                            {
                                var bc = trapDef.bycatchSpecies[i];
                                if (bc == null || string.IsNullOrEmpty(bc.speciesId)
                                    || string.Equals(bc.speciesId, site.catchSpecies, StringComparison.Ordinal))
                                    continue;
                                cumulative += bc.weight;
                                if (roll <= cumulative)
                                {
                                    site.bycatchSpecies = bc.speciesId;
                                    break;
                                }
                            }
                        }
                    }

                    // WT-INT-01: First-catch discovery tracking (primary catch only)
                    TryRecordFirstCatch(speciesId, site.siteId, site.assignedHunterId);

                    caught++;
                    _state.totalCatch++;
                }

                // Update check day for next interval
                site.checkDay = _currentDay + site.checkIntervalDays;

                // Plan 36: decrement durability on every eligible check (catch or no-catch)
                if (site.remainingDurability > 0)
                {
                    site.remainingDurability--;
                    if (site.remainingDurability <= 0)
                        site.isBroken = true;
                }
            }
            OnTrappingChanged?.Invoke();
            return caught > 0
                ? ActionResult.Success("trapping.catch_found", new Dictionary<string, double> { { "caught", caught } })
                : ActionResult.Success("trapping.no_catch");
        }

        public ActionResult Butcher(string siteId, string butcherId = "")
        {
            var site = _state.trapSites.Find(s => s.siteId == siteId);
            if (site == null || !site.hasCatch)
                return ActionResult.Blocked("no_catch", "trapping.no_catch");
            if (site.isMeatProcessed)
                return ActionResult.Blocked("already_butchered", "trapping.already_butchered");

            site.isMeatProcessed = true;
            OnTrappingChanged?.Invoke();
            OnButcheryCompleted?.Invoke(siteId, butcherId ?? string.Empty, site.catchSpecies ?? string.Empty, site.isToxic);
            return ActionResult.Success("trapping.butchered",
                new Dictionary<string, double> { { "yield", site.carcassYield }, { "toxic", site.isToxic ? 1 : 0 } });
        }

        /// <summary>
        /// Preserve the hide from a trapped carcass. Requires the carcass to be butchered first.
        /// Returns the hide item ID and quantity for the caller to add to inventory.
        /// </summary>
        public ActionResult PreserveHide(string siteId, out string hideItemId, out float hideQuantity)
        {
            hideItemId = string.Empty;
            hideQuantity = 0f;

            var site = _state.trapSites.Find(s => s.siteId == siteId);
            if (site == null || !site.hasCatch)
                return ActionResult.Blocked("no_catch", "trapping.no_catch");
            if (!site.isMeatProcessed)
                return ActionResult.Blocked("not_butchered", "trapping.not_butchered");
            if (site.hidePreserved)
                return ActionResult.Blocked("already_preserved", "trapping.already_preserved");

            if (_quarryCatalog.TryGetValue(site.catchSpecies, out var quarry) && quarry.hideYield > 0f)
            {
                hideItemId = quarry.hideItemId;
                hideQuantity = quarry.hideYield * (0.8f + (float)_rng.NextDouble() * 0.4f);
            }

            site.hidePreserved = true;
            OnTrappingChanged?.Invoke();
            if (!string.IsNullOrEmpty(hideItemId))
                OnHidePreserved?.Invoke(siteId, hideItemId);
            return ActionResult.Success("trapping.hide_preserved");
        }

        /// <summary>Get the bait catalog for UI display.</summary>
        public IReadOnlyDictionary<string, BaitProfile> GetBaitCatalog() => _baitCatalog;

        /// <summary>Get the quarry catalog for UI display.</summary>
        public IReadOnlyDictionary<string, QuarrySpecies> GetQuarryCatalog() => _quarryCatalog;

        /// <summary>
        /// Plan 36: Roll disease risk for a caught species using deterministic RNG.
        /// Returns true if disease should be applied.
        /// </summary>
        public bool RollDiseaseRisk(float diseaseRisk)
        {
            if (diseaseRisk <= 0f) return false;
            return _rng.NextDouble() < diseaseRisk;
        }

        /// <summary>
        /// Plan 36: Roll contamination risk for a caught species using deterministic RNG.
        /// Returns true if contamination should be applied.
        /// </summary>
        public bool RollContaminationRisk(float contaminationRisk)
        {
            if (contaminationRisk <= 0f) return false;
            return _rng.NextDouble() < contaminationRisk;
        }

        /// <summary>
        /// Repair a broken or damaged trap. Restores durability to the catalog-defined value.
        /// Caller must have already consumed repair materials through the inventory authority.
        /// </summary>
        public ActionResult RepairTrap(string siteId, int restoreDurability)
        {
            var site = _state.trapSites.Find(s => s.siteId == siteId);
            if (site == null)
                return ActionResult.Blocked("no_trap", "trapping.no_trap");
            if (!site.isBroken && site.remainingDurability < 0)
                return ActionResult.Blocked("not_tracked", "trapping.durability_not_tracked");
            if (!site.isBroken && site.remainingDurability > 0)
                return ActionResult.Blocked("not_damaged", "trapping.not_damaged");

            site.remainingDurability = restoreDurability > 0 ? restoreDurability : 1;
            site.isBroken = false;
            OnTrappingChanged?.Invoke();
            return ActionResult.Success("trapping.trap_repaired");
        }

        public ActionResult RemoveToxin(string siteId)
        {
            var site = _state.trapSites.Find(s => s.siteId == siteId);
            if (site == null || !site.hasCatch)
                return ActionResult.Blocked("no_catch", "trapping.no_catch");
            if (!site.isToxic)
                return ActionResult.Blocked("not_toxic", "trapping.not_toxic");
            if (site.toxinRemoved)
                return ActionResult.Blocked("already_clean", "trapping.already_clean");

            site.toxinRemoved = true;
            _state.totalToxicRemoved++;
            OnTrappingChanged?.Invoke();
            return ActionResult.Success("trapping.toxin_removed");
        }

        /// <summary>
        /// Advance the day and auto-check eligible snares.
        /// <paramref name="densityMultiplier"/> carries live wildlife pressure
        /// (sector pack population) into the catch rolls; 1.0 is authored rate.
        /// </summary>
        public void TickDay(int day, float densityMultiplier = 1f)
        {
            _currentDay = day;
            CheckTraps(densityMultiplier);
        }

        public WildlifeTrappingState CaptureState() => CloneState(_state);

        public void RestoreState(WildlifeTrappingState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
            if (_state.firstCatchLoggedSpeciesIds == null)
                _state.firstCatchLoggedSpeciesIds = new List<string>();
        }

        private static WildlifeTrappingState CloneState(WildlifeTrappingState src)
        {
            if (src == null) return new WildlifeTrappingState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<WildlifeTrappingState>(json) ?? new WildlifeTrappingState();
        }
    }
}
