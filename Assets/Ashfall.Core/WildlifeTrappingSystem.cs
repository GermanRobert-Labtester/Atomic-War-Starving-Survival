// SPDX-License-Identifier: MIT
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

        // Plan IV: shared event-delivery outbox. Unresolved external facts
        // (moral consequences, trap encounters, radio broadcasts) persist here
        // with stable identities until the destination authority accepts them.
        public List<WildlifeTrappingPendingEvent> pendingEvents = new List<WildlifeTrappingPendingEvent>();

        /// <summary>Monotonic persisted sequence for stable event identity.</summary>
        public int eventSequence;
        public int nextDeploymentSequence;

        // Deterministic replay positions. Zero means the injected RNG does
        // not expose a seekable position (legacy/custom test ports); the
        // seeded Core implementation captures all three streams.
        public int rngSeed;
        public ulong primaryRngState;
        public int encounterRngSeed;
        public ulong encounterRngState;
        public int incidentRngSeed;
        public ulong incidentRngState;
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
        public float bycatchYield; // Plan VI: independently resolved secondary carcass yield
        public bool bycatchToxic; // Plan VI: independently resolved secondary toxicity
        public float carcassYield;
        public bool isToxic;
        public bool toxinRemoved;
        public bool isMeatProcessed;
        public bool hidePreserved;
        public string diseaseId = string.Empty; // Tasks 5-8: resolved disease ID from catch
        public float contaminationDose; // Tasks 5-8: resolved contamination dose in rads
        public string bycatchDiseaseId = string.Empty; // Plan VI: resolved secondary disease ID
        public float bycatchContaminationDose; // Plan VI: resolved secondary contamination dose
        /// <summary>Resolved miss-only authored event awaiting host delivery.</summary>
        public string pendingNarrativeEvent = string.Empty;
        /// <summary>Deterministic persisted identity for one deployment cycle.</summary>
        public int deploymentSequence;
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
        // Plan IV Task 6: dedicated deterministic substream for the
        // trap-interference encounter roll. Forked once at construction so the
        // catch/disease/bycatch consumption order on the parent stream is
        // never perturbed by the encounter mechanic.
        private readonly ISeededRng _encounterRng;
        // Plan VI: flavor incidents use their own deterministic stream so
        // adding a miss-only narrative roll cannot reshuffle catch outcomes.
        private readonly ISeededRng _incidentRng;
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
        public event Action<ButcheryCompletedEvent>? OnButcheryCompletedDetailed;
        public event Action<string, string> OnHidePreserved; // siteId, hideItemId
        /// <summary>Plan 14E / C1.6: Fired when quarry preservation creates an eligible trophy recipe opportunity. Args: (speciesId, recipeId).</summary>
        public event Action<string, string>? OnTrophyReady;
        /// <summary>WT-INT-01: Fired when a prey species is caught for the first time. Args: (speciesId, siteId, hunterId).</summary>
        public event Action<string, string, string>? OnNewSpeciesDiscovered;
        /// <summary>Plan 36 III: Fired when secondary quarry (bycatch) is entangled alongside primary catch. Args: (siteId, trapId, primarySpecies, bycatchSpecies, day, hunterId).</summary>
        public event Action<string, string, string, string, int, string>? OnBycatchOccurred;
        /// <summary>Plan VI: complete bycatch state is committed before this event fires.</summary>
        public event Action<BycatchOccurredEvent>? OnBycatchResolved;
        public event Action<TrapLifecycleEvent>? OnTrapDeployed;
        public event Action<TrapLifecycleEvent>? OnTrapBroken;
        public event Action<TrapLifecycleEvent>? OnTrapRepaired;
        public event Action<TrapLifecycleEvent>? OnTrapRemoved;

        /// <summary>Plan IV: fired when a pending external fact is enqueued. Args: the pending event.</summary>
        public event Action<WildlifeTrappingPendingEvent>? OnPendingEventCreated;

        public WildlifeTrappingSystem(ISeededRng rng, ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
            // Plan IV Task 6: dedicated deterministic substream for the
            // trap-interference encounter roll, derived from the parent seed
            // (the ISeededRng port has no fork primitive — a stable seed
            // mix keeps the catch stream's consumption order unperturbed
            // while remaining fully seed-reproducible).
            _encounterRng = new SeededRng(DeriveEncounterStreamSeed(rng.Seed));
            _incidentRng = new SeededRng(DeriveIncidentStreamSeed(rng.Seed));
            InitializeDefaultProfiles();
        }

        /// <summary>Stable seed-mix (SplitMix64 finalizer, 32-bit) for the encounter substream.</summary>
        public static int DeriveEncounterStreamSeed(int parentSeed)
        {
            ulong z = unchecked((ulong)(uint)parentSeed + 0x9E3779B9UL);
            z ^= z >> 16;
            z *= unchecked(0xBF58476D1CE4E5B9UL);
            z ^= z >> 13;
            z *= unchecked(0x94D049BB133111EBUL);
            z ^= z >> 16;
            return unchecked((int)(uint)z);
        }

        /// <summary>Stable seed mix for miss-only narrative incidents.</summary>
        public static int DeriveIncidentStreamSeed(int parentSeed)
        {
            ulong z = unchecked((ulong)(uint)parentSeed + 0xD1B54A32D192ED03UL);
            z ^= z >> 17;
            z *= unchecked(0x9E3779B97F4A7C15UL);
            z ^= z >> 29;
            z *= unchecked(0xBF58476D1CE4E5B9UL);
            z ^= z >> 31;
            return unchecked((int)(uint)z);
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
        /// <summary>
        /// C2 / Plan 20C (§40) — optional shared weather-effects provider.
        /// When bound by the host, the per-kind trap penalty comes from the
        /// ONE weather-effects data table (penalty = 1 − trap_yield_multiplier)
        /// instead of this class's hardcoded curve — no duplicate weather
        /// tables (plan §40). Unbound keeps the legacy mapping byte-identical.
        /// </summary>
        public Func<WeatherKind, float>? WeatherPenaltyProvider { get; set; }

        public float EffectiveWeatherPenalty(WeatherKind weather)
            => WeatherPenaltyProvider != null
                ? Math.Clamp(WeatherPenaltyProvider(weather), 0f, 1f)
                : WeatherPenaltyFor(weather);

        public static float CalculatePrimaryCatchChance(
            float densityMultiplier,
            float hunterSkillLevel,
            float baitMultiplier,
            float weatherSensitivity,
            WeatherKind weather,
            float? weatherPenaltyOverride = null)
        {
            float baseChance = BaseCatchChance * densityMultiplier;
            float skillMult = SkillMultiplierFor(hunterSkillLevel);
            float pen = weatherPenaltyOverride.HasValue
                ? Math.Clamp(weatherPenaltyOverride.Value, 0f, 1f)
                : WeatherPenaltyFor(weather);
            float weatherMult = 1f - Math.Clamp(weatherSensitivity, 0f, 1f) * pen;
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
            CreatePendingEvent(
                WildlifeTrappingEventKinds.TrappingBroadcast,
                siteId, hunterId, speciesId,
                TrappingBroadcastIds.FirstCatch, 0f);
            OnNewSpeciesDiscovered?.Invoke(speciesId, siteId ?? string.Empty, hunterId ?? string.Empty);
            return true;
        }

        // ── Plan IV: shared event-delivery outbox ───────────────────────────

        /// <summary>
        /// Enqueue one unresolved external fact with a stable persisted
        /// identity. Deterministic: consumes no RNG; identity derives from the
        /// persisted monotonic sequence, never from Guid/hash/wall clock.
        /// Ordering across multiple facts in one check follows the explicit
        /// call order (bycatch → first catch → break; encounter on miss).
        /// </summary>
        private WildlifeTrappingPendingEvent CreatePendingEvent(
            string kind, string siteId, string survivorId, string speciesId,
            string payloadId, float weight, bool notify = true)
        {
            _state.eventSequence++;
            var ev = new WildlifeTrappingPendingEvent
            {
                eventId = $"wt_ev_{_state.eventSequence:D6}",
                sequence = _state.eventSequence,
                kind = kind ?? string.Empty,
                sourceTrapSiteId = siteId ?? string.Empty,
                survivorId = survivorId ?? string.Empty,
                speciesId = speciesId ?? string.Empty,
                payloadId = payloadId ?? string.Empty,
                weight = weight,
                day = _currentDay,
                status = WildlifeTrappingEventStatus.Pending
            };
            _state.pendingEvents.Add(ev);
            if (notify)
                OnPendingEventCreated?.Invoke(ev);
            return ev;
        }

        /// <summary>
        /// Pending events in persisted sequence order (delivery candidates).
        /// Delivered events are excluded — the destination authority owns them
        /// after acceptance.
        /// </summary>
        public List<WildlifeTrappingPendingEvent> GetPendingEvents()
        {
            var result = new List<WildlifeTrappingPendingEvent>();
            if (_state.pendingEvents == null) return result;
            for (int i = 0; i < _state.pendingEvents.Count; i++)
            {
                var ev = _state.pendingEvents[i];
                if (ev == null || !string.Equals(ev.status, WildlifeTrappingEventStatus.Pending, StringComparison.Ordinal))
                    continue;
                result.Add(ev);
            }
            result.Sort((a, b) => a.sequence.CompareTo(b.sequence));
            return result;
        }

        /// <summary>
        /// Acknowledge destination acceptance of one pending event. Returns
        /// false when the event is unknown or already delivered — exactly-once
        /// discipline: callers ack only after the destination accepted.
        /// </summary>
        public bool MarkEventDelivered(string eventId)
        {
            if (string.IsNullOrEmpty(eventId) || _state.pendingEvents == null) return false;
            for (int i = 0; i < _state.pendingEvents.Count; i++)
            {
                var ev = _state.pendingEvents[i];
                if (ev == null || !string.Equals(ev.eventId, eventId, StringComparison.Ordinal)) continue;
                if (!string.Equals(ev.status, WildlifeTrappingEventStatus.Pending, StringComparison.Ordinal))
                    return false;
                ev.status = WildlifeTrappingEventStatus.Delivered;
                if (string.Equals(ev.kind, WildlifeTrappingEventKinds.NarrativeIncident, StringComparison.Ordinal))
                {
                    for (int s = 0; s < _state.trapSites.Count; s++)
                    {
                        var site = _state.trapSites[s];
                        if (site != null
                            && string.Equals(site.siteId, ev.sourceTrapSiteId, StringComparison.Ordinal)
                            && string.Equals(site.pendingNarrativeEvent, ev.payloadId, StringComparison.Ordinal))
                        {
                            site.pendingNarrativeEvent = string.Empty;
                            break;
                        }
                    }
                }
                OnTrappingChanged?.Invoke();
                return true;
            }
            return false;
        }

        /// <summary>Count of pending (undelivered) external facts, by kind. Diagnostics.</summary>
        public int CountPendingEvents(string kind)
        {
            int n = 0;
            if (_state.pendingEvents == null) return 0;
            for (int i = 0; i < _state.pendingEvents.Count; i++)
            {
                var ev = _state.pendingEvents[i];
                if (ev != null && string.Equals(ev.kind, kind, StringComparison.Ordinal)
                    && string.Equals(ev.status, WildlifeTrappingEventStatus.Pending, StringComparison.Ordinal))
                    n++;
            }
            return n;
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

        /// <summary>
        /// Queries the single authoritative site-state rule used by deployment,
        /// host preflight, and the UI. A pending catch is intentionally
        /// replaceable; only a healthy, armed, catch-free trap is active.
        /// </summary>
        public bool CanSetTrapAtSite(string siteId, out string failureCode)
        {
            var existing = _state.trapSites.Find(s => s.siteId == siteId);
            if (existing != null && !existing.hasCatch && existing.setDay > 0 && !existing.isBroken)
            {
                failureCode = "trap_active";
                return false;
            }

            failureCode = string.Empty;
            return true;
        }

        public ActionResult SetTrap(string siteId, string baitType, string hunterId, string trapType = "snare",
            string trapId = "", int checkIntervalDays = -1, int durabilityChecks = -1)
        {
            var existing = _state.trapSites.Find(s => s.siteId == siteId);
            if (existing != null && !CanSetTrapAtSite(siteId, out string existingFailureCode))
                return ActionResult.Blocked(existingFailureCode, "trapping." + existingFailureCode);
            int interval = checkIntervalDays > 0 ? checkIntervalDays : 2;
            int deploymentSequence = ++_state.nextDeploymentSequence;
            if (existing != null)
            {
                existing.setDay = _currentDay;
                existing.checkDay = _currentDay + interval;
                existing.baitType = baitType;
                existing.trapType = trapType;
                existing.trapId = trapId ?? string.Empty;
                existing.checkIntervalDays = interval;
                existing.remainingDurability = durabilityChecks > 0 ? durabilityChecks : -1;
                existing.isBroken = false;
                existing.assignedHunterId = hunterId ?? string.Empty;
                existing.deploymentSequence = deploymentSequence;
                // Plan (flagship trapping tranche): a replacement begins clean —
                // the entire catch payload of the old trap is cleared, not just
                // the hasCatch flag. Stale catchSpecies/carcassYield/isToxic
                // etc. must not leak into the fresh trap's state.
                existing.hasCatch = false;
                existing.catchSpecies = string.Empty;
                existing.bycatchSpecies = string.Empty;
                existing.bycatchYield = 0f;
                existing.bycatchToxic = false;
                existing.carcassYield = 0f;
                existing.isToxic = false;
                existing.toxinRemoved = false;
                existing.isMeatProcessed = false;
                existing.hidePreserved = false;
                existing.diseaseId = string.Empty;
                existing.contaminationDose = 0f;
                existing.bycatchDiseaseId = string.Empty;
                existing.bycatchContaminationDose = 0f;
                existing.pendingNarrativeEvent = string.Empty;
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
                    remainingDurability = durabilityChecks > 0 ? durabilityChecks : -1,
                    deploymentSequence = deploymentSequence
                });
            }
            OnTrappingChanged?.Invoke();
            OnTrapDeployed?.Invoke(new TrapLifecycleEvent
            {
                siteId = siteId ?? string.Empty,
                trapId = trapId ?? string.Empty,
                trapType = trapType ?? string.Empty,
                isBroken = false,
                day = _currentDay
            });
            return ActionResult.Success("trapping.trap_set");
        }

        /// <summary>Baseline catch rate (Unity parity: 50%).</summary>
        public const float BaseCatchChance = 0.5f;

        /// <summary>
        /// Deterministic quarry-eligibility filter (Plan 36 / flagship trapping
        /// tranche). Returns the sorted species IDs that pass every independent
        /// gate for the given bait/trap/skill and the current selection
        /// context:
        ///   skill gate  — hunterSkillLevel >= minSkillLevel;
        ///   season gate — prey with activeSeasons must include SeasonWindowId
        ///                 (empty activeSeasons = year-round);
        ///   migration   — prey with migrationSpeciesId require that species
        ///                 in PresentMigrationSpecies. Empty presence excludes
        ///                 every migration-linked prey (migration-linked prey
        ///                 are simply absent when no pack stands in the
        ///                 sector); non-migration prey are unaffected.
        /// No weighting and no fallback here — callers get the exact filtered
        /// candidate set so tests can assert membership directly.
        /// </summary>
        public List<string> GetEligibleQuarryIds(string baitType, string trapType, float hunterSkillLevel,
            string trapId = "")
        {
            var eligible = new List<string>();
            string seasonId = _selectionContext.SeasonWindowId;
            bool hasSeason = !string.IsNullOrEmpty(seasonId);

            foreach (var kvp in _quarryCatalog)
            {
                var q = kvp.Value;
                if (hunterSkillLevel < q.minSkillLevel) continue;
                if (!IsCompatibleWithTrap(q.speciesId, trapType, trapId)) continue;

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

                // Plan 36 / flagship tranche: migration filter — a prey with a
                // migrationSpeciesId is only selectable when that species is
                // live in the sector. The old `hasMigration` guard skipped this
                // filter entirely when the presence set was empty, which made
                // migration-linked prey catchable on empty ground.
                if (_preyDefinitionCatalog.TryGetValue(q.speciesId, out var preyDef2)
                    && !string.IsNullOrEmpty(preyDef2.migrationSpeciesId)
                    && !_selectionContext.PresentMigrationSpecies.Contains(preyDef2.migrationSpeciesId))
                    continue;

                eligible.Add(q.speciesId);
            }

            eligible.Sort(StringComparer.Ordinal);
            return eligible;
        }

        /// <summary>
        /// Applies the authored trap compatibility matrix when the catalog is
        /// available. A resolved trap ID is authoritative; legacy callers that
        /// only have a trap type use the union of definitions of that type.
        /// An unregistered/default system keeps its historical open candidate
        /// behavior because it has no compatibility authority to consult.
        /// </summary>
        private bool IsCompatibleWithTrap(string preyId, string trapType, string trapId)
        {
            if (_trapDefinitionCatalog.Count == 0) return true;

            if (!string.IsNullOrEmpty(trapId)
                && _trapDefinitionCatalog.TryGetValue(trapId, out var exactTrap))
            {
                // Runtime-only/custom traps may omit the optional matrix. The
                // catalog validator rejects that shape for authored traps, but
                // omission here remains backward-compatible for callers that
                // use trap definitions solely for durability/bycatch.
                return exactTrap.compatiblePrey == null
                    || exactTrap.compatiblePrey.Count == 0
                    || exactTrap.compatiblePrey.Contains(preyId);
            }

            bool foundMatchingType = false;
            bool foundDeclaredCompatibility = false;
            foreach (var trap in _trapDefinitionCatalog.Values)
            {
                if (!string.Equals(trap.trapType, trapType, StringComparison.Ordinal)) continue;
                foundMatchingType = true;
                if (trap.compatiblePrey == null || trap.compatiblePrey.Count == 0) continue;
                foundDeclaredCompatibility = true;
                if (trap.compatiblePrey != null && trap.compatiblePrey.Contains(preyId))
                    return true;
            }

            // Preserve legacy synthetic callers that use a trap type not yet
            // represented in the loaded catalog; a real catalog type is gated.
            return !foundMatchingType || !foundDeclaredCompatibility;
        }

        /// <summary>
        /// Select a quarry species based on bait affinity, trap type, per-site hunter skill level,
        /// season, migration presence, and abundance. Returns the species ID or
        /// string.Empty if no eligible quarry.
        /// </summary>
        private string SelectQuarrySpecies(string baitType, string trapType, string trapId, float hunterSkillLevel)
        {
            var candidates = new List<(string id, float weight)>();

            foreach (string speciesId in GetEligibleQuarryIds(baitType, trapType, hunterSkillLevel, trapId))
            {
                var q = _quarryCatalog[speciesId];
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

                if (weight <= 0f) continue;

                candidates.Add((q.speciesId, weight));
            }
            if (candidates.Count == 0)
                return string.Empty;

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
                    _selectionContext.CurrentWeather,
                    EffectiveWeatherPenalty(_selectionContext.CurrentWeather));

                bool primaryCatchResolved = false;
                if (_rng.NextDouble() < finalChance)
                {
                    // Select species based on bait, trap type, and per-site hunter skill
                    string speciesId = SelectQuarrySpecies(site.baitType, site.trapType, site.trapId, siteHunterSkill);
                    if (string.IsNullOrEmpty(speciesId))
                    {
                        // No eligible quarry species available in current environment context
                        site.checkDay = _currentDay + site.checkIntervalDays;
                        if (site.remainingDurability > 0)
                        {
                            site.remainingDurability--;
                            if (site.remainingDurability <= 0)
                                MarkBroken(site);
                        }
                        continue;
                    }
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
                    site.bycatchYield = 0f;
                    site.bycatchToxic = false;
                    site.bycatchDiseaseId = string.Empty;
                    site.bycatchContaminationDose = 0f;
                    primaryCatchResolved = true;

                    // Plan VI: bycatch roll — deterministic and independent of
                    // the primary carcass. A candidate must resolve through a
                    // registered quarry/prey definition; unsupported legacy
                    // candidates cannot create a secondary catch.
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
                                && (_quarryCatalog.ContainsKey(bc.speciesId) || _preyDefinitionCatalog.ContainsKey(bc.speciesId))
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
                                    || (!_quarryCatalog.ContainsKey(bc.speciesId) && !_preyDefinitionCatalog.ContainsKey(bc.speciesId))
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

                    if (!string.IsNullOrEmpty(site.bycatchSpecies))
                    {
                        // Bycatch yield and toxicity use the secondary species
                        // definition. Each is resolved once before any event
                        // subscriber observes the state.
                        if (_quarryCatalog.TryGetValue(site.bycatchSpecies, out var bycatchQuarry))
                        {
                            site.bycatchYield = bycatchQuarry.baseYieldKg
                                * (0.7f + (float)_rng.NextDouble() * 0.6f);
                            float bycatchToxicChance = Math.Max(0.01f,
                                bycatchQuarry.toxicChance - baitToxicReduction);
                            site.bycatchToxic = _rng.NextDouble() < bycatchToxicChance;
                        }
                        else if (_preyDefinitionCatalog.TryGetValue(site.bycatchSpecies, out var bycatchPreyFallback))
                        {
                            site.bycatchYield = bycatchPreyFallback.baseYieldKg
                                * (0.7f + (float)_rng.NextDouble() * 0.6f);
                            float bycatchToxicChance = Math.Max(0.01f,
                                bycatchPreyFallback.toxicChance - baitToxicReduction);
                            site.bycatchToxic = _rng.NextDouble() < bycatchToxicChance;
                        }

                        if (_preyDefinitionCatalog.TryGetValue(site.bycatchSpecies, out var bycatchDef))
                        {
                            if (RollDiseaseRisk(bycatchDef.diseaseRisk))
                                site.bycatchDiseaseId = PreyDefinition.ResolveDiseaseId(bycatchDef);
                            if (RollContaminationRisk(bycatchDef.contaminationRisk))
                            {
                                site.bycatchContaminationDose = bycatchDef.contaminationDose > 0f
                                    ? bycatchDef.contaminationDose
                                    : PreyDefinition.FallbackContaminationDose;
                            }
                        }

                        // Preserve the existing compatibility event, then
                        // publish the complete typed result exactly once.
                        OnBycatchOccurred?.Invoke(site.siteId, site.trapId, site.catchSpecies,
                            site.bycatchSpecies, _currentDay, site.assignedHunterId);
                        OnBycatchResolved?.Invoke(new BycatchOccurredEvent
                        {
                            siteId = site.siteId,
                            trapId = site.trapId,
                            primarySpeciesId = site.catchSpecies,
                            bycatchSpeciesId = site.bycatchSpecies,
                            bycatchYield = site.bycatchYield,
                            bycatchToxic = site.bycatchToxic,
                            day = _currentDay,
                            hunterId = site.assignedHunterId
                        });

                        // Plan IV Task 7: rare-species bycatch feeds the radio
                        // layer. Common bycatch emits nothing. Reuses the
                        // bycatchDef resolved above (same species, same catalog).
                        if (bycatchDef != null && bycatchDef.isRareSpecies)
                        {
                            CreatePendingEvent(
                                WildlifeTrappingEventKinds.TrappingBroadcast,
                                site.siteId, site.assignedHunterId, site.bycatchSpecies,
                                TrappingBroadcastIds.RareBycatch, 0f);
                        }
                    }

                    // Plan 36 Closure II / Tasks 5-8: Resolve disease and contamination deterministically at catch time
                    site.diseaseId = string.Empty;
                    site.contaminationDose = 0f;
                    if (_preyDefinitionCatalog.TryGetValue(speciesId, out var preyDef))
                    {
                        if (RollDiseaseRisk(preyDef.diseaseRisk))
                        {
                            site.diseaseId = PreyDefinition.ResolveDiseaseId(preyDef);
                        }
                        if (RollContaminationRisk(preyDef.contaminationRisk))
                        {
                            site.contaminationDose = preyDef.contaminationDose > 0f
                                ? preyDef.contaminationDose
                                : 2.0f;
                        }
                    }

                    // WT-INT-01: First-catch discovery tracking (primary catch only).
                    // Plan IV Task 7: the first-catch fact also feeds the radio layer
                    // (created inside TryRecordFirstCatch, no RNG consumed).
                    TryRecordFirstCatch(speciesId, site.siteId, site.assignedHunterId);

                    caught++;
                    _state.totalCatch++;
                }

                // Update check day for next interval
                site.checkDay = _currentDay + site.checkIntervalDays;

                // Plan IV/VI: eligible miss — the primary catch roll failed
                // on an intact, deployed trap. Deterministic encounter roll on
                // dedicated streams; a successful catch never reaches this
                // path, and broken/legacy traps never enter the loop.
                if (!primaryCatchResolved)
                {
                    float encounterChance = trapDef?.trapEncounterChance ?? 0f;
                    if (encounterChance > 0f && _encounterRng.NextDouble() < encounterChance)
                        ResolveTrapInterference(site);

                    ResolveNarrativeIncident(site, trapDef);
                }

                // Plan 36: decrement durability on every eligible check (catch or no-catch)
                if (site.remainingDurability > 0)
                {
                    site.remainingDurability--;
                    if (site.remainingDurability <= 0)
                        MarkBroken(site);
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

            // Plan IV Task 5 / Plan VI: moral consequence is authored by the
            // PRIMARY prey only. Bycatch health is independent, but it never
            // creates a second morale action. Consumes no RNG.
            if (!string.IsNullOrEmpty(site.catchSpecies)
                && _preyDefinitionCatalog.TryGetValue(site.catchSpecies, out var moralPrey)
                && moralPrey.moralWeight > 0f)
            {
                CreatePendingEvent(
                    WildlifeTrappingEventKinds.MoralConsequence,
                    site.siteId,
                    butcherId ?? string.Empty,
                    site.catchSpecies,
                    TrappingMoralTier.ResolveQuestId(moralPrey.moralWeight),
                    moralPrey.moralWeight);
            }

            OnButcheryCompleted?.Invoke(siteId, butcherId ?? string.Empty, site.catchSpecies ?? string.Empty, site.isToxic);
            OnButcheryCompletedDetailed?.Invoke(new ButcheryCompletedEvent
            {
                actionId = $"butchery:{siteId}:{site.deploymentSequence}:{site.catchSpecies}:{butcherId ?? string.Empty}",
                siteId = siteId ?? string.Empty,
                butcherId = butcherId ?? string.Empty,
                primarySpeciesId = site.catchSpecies ?? string.Empty,
                bycatchSpeciesId = site.bycatchSpecies ?? string.Empty,
                primaryYield = site.carcassYield,
                bycatchYield = site.bycatchYield,
                totalYield = site.carcassYield + site.bycatchYield,
                isToxic = site.isToxic,
                bycatchToxic = site.bycatchToxic,
                setDay = site.setDay
            });
            return ActionResult.Success("trapping.butchered",
                new Dictionary<string, double>
                {
                    { "yield", site.carcassYield + site.bycatchYield },
                    { "primaryYield", site.carcassYield },
                    { "bycatchYield", site.bycatchYield },
                    { "toxic", site.isToxic ? 1 : 0 },
                    { "bycatchToxic", site.bycatchToxic ? 1 : 0 }
                });
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

            string trophyRecipe = GetTrophyRecipeForSpecies(site.catchSpecies);
            if (!string.IsNullOrEmpty(trophyRecipe))
                OnTrophyReady?.Invoke(site.catchSpecies, trophyRecipe);

            return ActionResult.Success("trapping.hide_preserved");
        }

        /// <summary>
        /// Plan 136: Connects butchered trap catch directly to player inventory,
        /// depositing raw meat and preserved hides without creating shadow stores.
        /// </summary>
        public ActionResult TransferCatchToInventory(string siteId, Inventory.Inventory inventory, string fallbackRawMeatId = "raw_meat")
        {
            if (inventory == null)
                return ActionResult.Blocked("null_inventory", "trapping.null_inventory");

            var site = _state.trapSites.Find(s => s.siteId == siteId);
            if (site == null || !site.hasCatch)
                return ActionResult.Blocked("no_catch", "trapping.no_catch");

            if (!site.isMeatProcessed)
            {
                var butcherRes = Butcher(siteId);
                if (!butcherRes.IsSuccess)
                    return butcherRes;
            }

            int meatCount = (int)Math.Max(1, Math.Round(site.carcassYield + site.bycatchYield));
            inventory.AddById(fallbackRawMeatId, meatCount);

            if (!site.hidePreserved)
            {
                PreserveHide(siteId, out string hideItemId, out float hideQuantity);
                if (!string.IsNullOrEmpty(hideItemId) && hideQuantity > 0f)
                {
                    int hideCount = (int)Math.Max(1, Math.Round(hideQuantity));
                    inventory.AddById(hideItemId, hideCount);
                }
            }

            return ActionResult.Success("trapping.catch_transferred",
                new Dictionary<string, double>
                {
                    { "meatTransferred", meatCount },
                    { "isToxic", site.isToxic ? 1 : 0 },
                    { "contaminationDose", site.contaminationDose }
                });
        }


        private static readonly Dictionary<string, string> s_trophyRecipes = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            { "wolf", "recipe_trophy_wolf_head" },
            { "deer", "recipe_trophy_deer_antlers" },
            { "boar", "recipe_trophy_boar_tusks" },
            { "fox", "recipe_trophy_fox_pelt" },
            { "slag_beetle", "recipe_trophy_beetle_carapace" },
            { "molerat", "recipe_trophy_molerat_skull" },
            { "ash_crow", "recipe_trophy_crow_feathers" },
            { "pheasant", "recipe_trophy_pheasant_plume" },
            { "ash_hound", "recipe_trophy_ash_hound_pelt" },
            { "species_ash_hound", "recipe_trophy_ash_hound_pelt" },
            { "dust_lynx", "recipe_trophy_gulden_wolf" },
            { "species_dust_lynx", "recipe_trophy_gulden_wolf" },
            { "iron_crow", "recipe_trophy_kestrel_wings" },
            { "species_iron_crow", "recipe_trophy_kestrel_wings" }
        };

        /// <summary>Plan 14E / C1.6: Get trophy recipe ID for quarry species.</summary>
        public string GetTrophyRecipeForSpecies(string speciesId)
        {
            if (string.IsNullOrEmpty(speciesId)) return string.Empty;
            return s_trophyRecipes.TryGetValue(speciesId, out var r) ? r : string.Empty;
        }

        /// <summary>Get the bait catalog for UI display.</summary>
        public IReadOnlyDictionary<string, BaitProfile> GetBaitCatalog() => _baitCatalog;

        /// <summary>Get the quarry catalog for UI display.</summary>
        public IReadOnlyDictionary<string, QuarrySpecies> GetQuarryCatalog() => _quarryCatalog;

        /// <summary>Plan 36 / Tasks 5-8: Get the trap definition catalog for runtime lookup and deployment proof.</summary>
        public IReadOnlyDictionary<string, TrapDefinition> GetTrapDefinitionCatalog() => _trapDefinitionCatalog;

        /// <summary>Plan 36 / Tasks 5-8: Get the prey definition catalog for runtime lookup and disease/contamination verification.</summary>
        public IReadOnlyDictionary<string, PreyDefinition> GetPreyDefinitionCatalog() => _preyDefinitionCatalog;

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
            OnTrapRepaired?.Invoke(new TrapLifecycleEvent
            {
                siteId = site.siteId,
                trapId = site.trapId,
                trapType = site.trapType,
                isBroken = false,
                day = _currentDay
            });
            return ActionResult.Success("trapping.trap_repaired");
        }

        /// <summary>Remove a trap site and its map presence. Catch outcomes are
        /// not exposed or copied anywhere; removal is a domain deletion.</summary>
        public ActionResult RemoveTrap(string siteId)
        {
            if (string.IsNullOrEmpty(siteId))
                return ActionResult.Blocked("no_trap", "trapping.no_trap");
            int index = _state.trapSites.FindIndex(s => s != null && s.siteId == siteId);
            if (index < 0)
                return ActionResult.Blocked("no_trap", "trapping.no_trap");

            var site = _state.trapSites[index];
            _state.trapSites.RemoveAt(index);
            OnTrappingChanged?.Invoke();
            OnTrapRemoved?.Invoke(new TrapLifecycleEvent
            {
                siteId = site.siteId,
                trapId = site.trapId,
                trapType = site.trapType,
                isBroken = site.isBroken,
                day = _currentDay
            });
            return ActionResult.Success("trapping.trap_removed");
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

        public WildlifeTrappingState CaptureState()
        {
            var captured = CloneState(_state);
            captured.rngSeed = _rng.Seed;
            captured.primaryRngState = _rng is SeededRng primary ? primary.PeekState() : 0UL;
            captured.encounterRngSeed = _encounterRng.Seed;
            captured.encounterRngState = _encounterRng is SeededRng encounter ? encounter.PeekState() : 0UL;
            captured.incidentRngSeed = _incidentRng.Seed;
            captured.incidentRngState = _incidentRng is SeededRng incident ? incident.PeekState() : 0UL;
            return captured;
        }

        public void RestoreState(WildlifeTrappingState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
            if (_state.trapSites == null)
                _state.trapSites = new List<TrapSite>();
            if (_rng is SeededRng primary && _state.primaryRngState != 0UL)
                primary.SeekState(_state.primaryRngState);
            if (_encounterRng is SeededRng encounter && _state.encounterRngState != 0UL)
                encounter.SeekState(_state.encounterRngState);
            if (_incidentRng is SeededRng incident && _state.incidentRngState != 0UL)
                incident.SeekState(_state.incidentRngState);
            if (_state.firstCatchLoggedSpeciesIds == null)
                _state.firstCatchLoggedSpeciesIds = new List<string>();

            // Plan IV §11.3: restore validation for the outbox. Drop null
            // entries, derive missing identities, normalize status, and keep
            // the monotonic sequence strictly ahead of every persisted event.
            if (_state.pendingEvents == null)
                _state.pendingEvents = new List<WildlifeTrappingPendingEvent>();
            int maxEventSequence = 0;
            for (int i = _state.pendingEvents.Count - 1; i >= 0; i--)
            {
                var ev = _state.pendingEvents[i];
                if (ev == null)
                {
                    _state.pendingEvents.RemoveAt(i);
                    continue;
                }
                ev.kind ??= string.Empty;
                ev.sourceTrapSiteId ??= string.Empty;
                ev.survivorId ??= string.Empty;
                ev.speciesId ??= string.Empty;
                ev.payloadId ??= string.Empty;
                ev.status = string.Equals(ev.status, WildlifeTrappingEventStatus.Delivered, StringComparison.Ordinal)
                    ? WildlifeTrappingEventStatus.Delivered
                    : WildlifeTrappingEventStatus.Pending;
                if (ev.sequence > maxEventSequence) maxEventSequence = ev.sequence;
                if (string.IsNullOrEmpty(ev.eventId))
                    ev.eventId = $"wt_ev_{ev.sequence:D6}";
            }
            if (_state.eventSequence < maxEventSequence)
                _state.eventSequence = maxEventSequence;

            int maxDeploymentSequence = _state.nextDeploymentSequence;
            if (_state.trapSites != null)
            {
                for (int i = 0; i < _state.trapSites.Count; i++)
                {
                    var site = _state.trapSites[i];
                    if (site == null) continue;

                    site.siteId ??= string.Empty;
                    site.assignedHunterId ??= string.Empty;
                    site.baitType ??= string.Empty;
                    site.trapType ??= string.Empty;
                    site.trapId ??= string.Empty;
                    site.catchSpecies ??= string.Empty;
                    site.bycatchSpecies ??= string.Empty;
                    site.diseaseId ??= string.Empty;
                    site.bycatchDiseaseId ??= string.Empty;
                    site.pendingNarrativeEvent ??= string.Empty;

                    if (string.IsNullOrEmpty(site.bycatchSpecies))
                    {
                        site.bycatchYield = 0f;
                        site.bycatchToxic = false;
                        site.bycatchDiseaseId = string.Empty;
                        site!.bycatchContaminationDose = 0f;
                    }

                    if (site.deploymentSequence <= 0)
                        site.deploymentSequence = ++maxDeploymentSequence;
                    else
                        maxDeploymentSequence = Math.Max(maxDeploymentSequence, site.deploymentSequence);

                    // Legacy untracked traps: if durability is 0 but not broken, and untracked (or missing), ensure -1
                    if (!site.isBroken && site.remainingDurability == 0 && string.IsNullOrEmpty(site.trapId))
                    {
                        site.remainingDurability = -1;
                    }
                    else if (site.remainingDurability < 0)
                    {
                        site.remainingDurability = -1;
                    }
                    else if (site.isBroken && site.remainingDurability > 0)
                    {
                        site.remainingDurability = 0;
                    }
                }
            }
            _state.nextDeploymentSequence = maxDeploymentSequence;

            // A legacy Plan VI save may contain the pending incident
            // projection but not the shared outbox entry. Rebuild that entry
            // without RNG or callbacks so restore cannot reroll or dispatch.
            for (int i = 0; i < _state.trapSites!.Count; i++)
            {
                var site = _state.trapSites[i];
                if (site == null || string.IsNullOrEmpty(site.pendingNarrativeEvent)) continue;
                bool hasPending = false;
                bool hasDelivered = false;
                for (int e = 0; e < _state.pendingEvents.Count; e++)
                {
                    var pending = _state.pendingEvents[e];
                    if (pending == null
                        || !string.Equals(pending.kind, WildlifeTrappingEventKinds.NarrativeIncident, StringComparison.Ordinal)
                        || !string.Equals(pending.sourceTrapSiteId, site.siteId, StringComparison.Ordinal)
                        || !string.Equals(pending.payloadId, site.pendingNarrativeEvent, StringComparison.Ordinal))
                        continue;
                    hasPending |= string.Equals(pending.status, WildlifeTrappingEventStatus.Pending, StringComparison.Ordinal);
                    hasDelivered |= string.Equals(pending.status, WildlifeTrappingEventStatus.Delivered, StringComparison.Ordinal);
                }
                if (hasDelivered && !hasPending)
                {
                    site.pendingNarrativeEvent = string.Empty;
                }
                else if (!hasPending)
                {
                    CreatePendingEvent(
                        WildlifeTrappingEventKinds.NarrativeIncident,
                        site.siteId,
                        site.assignedHunterId,
                        string.Empty,
                        site.pendingNarrativeEvent,
                        0f,
                        notify: false);
                }
            }
        }

        private static WildlifeTrappingState CloneState(WildlifeTrappingState src)
        {
            if (src == null) return new WildlifeTrappingState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<WildlifeTrappingState>(json) ?? new WildlifeTrappingState();
        }

        /// <summary>
        /// Plan IV Task 6: select and apply the deterministic interference
        /// outcome for one encounter hit. The trapping system owns the trap
        /// side effects (bait loss, tamper damage) as canonical domain
        /// mutations; the surfaced encounter definition owns the narrative
        /// presentation and the player's response (morale/guilt/flags via the
        /// encounter authority).
        /// </summary>
        private void ResolveTrapInterference(TrapSite site)
        {
            // Uniform deterministic selection across the three authored families.
            int pick = _encounterRng.Next(0, 3);
            string encounterId = pick switch
            {
                0 => TrapEncounterIds.BaitStolen,
                1 => TrapEncounterIds.Tampered,
                _ => TrapEncounterIds.StrangerDiscovery
            };

            switch (pick)
            {
                case 0:
                    // Bait stolen: canonical in-domain command. The trap
                    // continues operating unbaited (lower effective catch
                    // chance on subsequent checks).
                    site.baitType = string.Empty;
                    break;
                case 1:
                    // Trap tampered: canonical durability damage through the
                    // same break transition as wear exhaustion.
                    site.remainingDurability = 0;
                    MarkBroken(site);
                    break;
                default:
                    // Stranger discovery: no trap mutation; narrative +
                    // player response are owned by the encounter authority.
                    break;
            }

            CreatePendingEvent(
                WildlifeTrappingEventKinds.TrapEncounter,
                site.siteId,
                site.assignedHunterId,
                string.Empty,
                encounterId,
                0f);
        }

        /// <summary>
        /// Resolve one miss-only authored incident. The dedicated stream keeps
        /// atmospheric content from perturbing catch, bycatch, or health
        /// replay. Existing pending incidents are never rerolled or replaced.
        /// </summary>
        private void ResolveNarrativeIncident(TrapSite site, TrapDefinition? trapDef)
        {
            if (trapDef == null
                || string.IsNullOrEmpty(site.trapId)
                || site.isBroken
                || trapDef.narrativeIncidentChance <= 0f
                || !string.IsNullOrEmpty(site.pendingNarrativeEvent))
                return;

            if (_incidentRng.NextDouble() >= trapDef.narrativeIncidentChance)
                return;

            IReadOnlyList<string> candidates = trapDef.narrativeIncidentIds != null
                && trapDef.narrativeIncidentIds.Count > 0
                ? trapDef.narrativeIncidentIds
                : TrapNarrativeIncidentIds.Ordered;
            if (candidates.Count == 0) return;

            int pick = _incidentRng.Next(0, candidates.Count);
            string eventId = candidates[pick] ?? string.Empty;
            if (string.IsNullOrWhiteSpace(eventId)) return;

            site.pendingNarrativeEvent = eventId;
            CreatePendingEvent(
                WildlifeTrappingEventKinds.NarrativeIncident,
                site.siteId,
                site.assignedHunterId,
                string.Empty,
                eventId,
                0f);
        }

        /// <summary>Stable source identity for host-level narrative de-duplication.</summary>
        public static string BuildNarrativeIncidentSourceId(string siteId, string eventId)
            => $"wildlife-trap:{siteId ?? string.Empty}:incident:{eventId ?? string.Empty}";

        private void MarkBroken(TrapSite site)
        {
            if (site.isBroken) return;
            site.isBroken = true;
            site.remainingDurability = 0;
            // Plan IV Task 7: the break TRANSITION emits exactly one radio
            // fact. Already-broken traps re-entering checks emit nothing.
            CreatePendingEvent(
                WildlifeTrappingEventKinds.TrappingBroadcast,
                site.siteId,
                site.assignedHunterId,
                string.Empty,
                TrappingBroadcastIds.TrapBroken,
                0f);
            OnTrapBroken?.Invoke(new TrapLifecycleEvent
            {
                siteId = site.siteId,
                trapId = site.trapId,
                trapType = site.trapType,
                isBroken = true,
                day = _currentDay
            });
        }
    }
}
