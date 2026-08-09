using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Decoupled, background-simulated Expedition Engine.
    /// Manages node-based travel ticks, stance selection, stamina drain,
    /// psychological encounter auto-resolution, and push-your-luck looting.
    /// </summary>
    public partial class ExpeditionSystem
    {
        public const float BaseStaminaDrainPerHour = 5f;
        public const float MaxCarryingCapacityDefault = 30f;

        /// <summary>
        /// Base AcuteDoseWindow added to the survivor when caught in the flashpoint,
        /// before duration scaling (see <see cref="GetFlashpointDoseScale"/>).
        /// </summary>
        public const float FlashpointAcuteDoseSpike = 30f;

        /// <summary>Expedition hours at which the flashpoint dose spike is unscaled (×1).</summary>
        public const float FlashpointDoseReferenceHours = 24f;

        /// <summary>Lower bound on the flashpoint dose duration scale (a survivor caught minutes out).</summary>
        public const float MinFlashpointDoseScale = 0.5f;

        /// <summary>Upper bound on the flashpoint dose duration scale (a long deep-field run).</summary>
        public const float MaxFlashpointDoseScale = 2.0f;

        /// <summary>Default shelter delay (ticks) for the Cautious flashpoint behavior.</summary>
        public const int DefaultCautiousShelterDelayTicks = 18; // 12-24 hour range; midpoint

        /// <summary>Return speed multiplier for the Paranoid sprint-home-empty-handed behavior.</summary>
        public const float ParanoidSprintMultiplier = 2.0f;

        /// <summary>Return speed divisor for the Fatalist slow-walk behavior.</summary>
        public const float FatalistSlowWalkDivisor = 2.0f;

        // Hatch-dilemma consequence magnitudes (Prompt #26 follow-up).
        // Tuned for designer iteration; log a one-liner on each apply so
        // the values can be re-tuned from the Editor console.
        /// <summary>Bunker contamination (rads/hr) added when the player picks "let them in".</summary>
        public const float LetThemInContaminationRadsPerHour = 50f;
        /// <summary>Smaller bunker contamination (rads/hr) added when the player picks "force decon" (small spill during strip-and-decon).</summary>
        public const float ForceDeconContaminationRadsPerHour = 10f;
        /// <summary>Morale hit applied to every OTHER living survivor when the player picks "deny entry".</summary>
        public const float DenyEntryMoralePenaltyForOtherSurvivors = 20f;

        private readonly RadiationSystem _radSystem;
        private readonly Inventory.Inventory _inventory;
        private readonly ItemCatalogSO _itemCatalog;
        private readonly WeatherSystem _weatherSystem;
        private readonly RadiationKnowledgeMap _knowledgeMap;
        private readonly System.Random _rng;
        private readonly MedicalSystem _medicalSystem;
        private readonly Shelter.Shelter _shelter;
        private readonly IReadOnlyList<Survivor> _survivors;
        private GeneratedMap _generatedMap;
        private SabotagedCacheSystem _sabotagedCaches;
        private NeedsSystem _needsSystem;
        private BicycleSystem _bicycleSystem;
        private FloodedNodeSystem _floodedNodeSystem;
        private RiverNodeSystem _riverNodeSystem;
        private BloodToxicitySystem _bloodToxicity;
        private Func<string, bool> _hasItem;
        private Func<string, int> _countItem;
        private Func<string, int, bool> _consumeItem;
        private ItemDefinition _deserterStandRifle;

        private readonly List<ExpeditionState> _activeExpeditions = new List<ExpeditionState>();
        private readonly List<EncounterSO> _encounterPool = new List<EncounterSO>();

        // Prompts #182–#188 — combat milestone perks (optional wiring).
        private CombatPerkSystem _combatPerks;
        private Func<int> _getDay;
        private InterpersonalAffinity _affinity;
        private Func<IReadOnlyList<Survivor>> _getAllSurvivors;

        // Structured caliber combat + faction ammo loot (Item_AmmoTypes).
        private Item_AmmoTypes _ammoTypes;
        private Func<string, ItemDefinition> _ammoItemFactory;
        private Func<string, ItemDefinition> _worldItemFactory;
        private bool _worldLootEnabled = true;

        // Prompts #206–#210 — expedition / wasteland milestone perks.
        private ExpeditionPerkSystem _expeditionPerks;
        private NoiseSystem _noiseSystem;
        private Func<bool> _isStormActive;
        private ItemDefinition _foragerRoots;
        private ItemDefinition _foragerBerries;
        // Prompts #220–#224 — latent expert traits on expeditions.
        private PersonalQuestSystem _personalQuests;
        private ItemDefinition _apexMeat;

        // REPROMOTE-Encounter-001 — class tracker for roadblocks (map tag / SO id).
        private Encounter_Roadblock _classRoadblock;

        // REPROMOTE-MapHazard-001 — carnivorous swamp flora on looting arrival.
        private MapHazard_VenusTrap _venusTrap;

        // Prompt #902 — frozen survivor on snowfield/frozen-lake nodes.
        private MapHazard_FrozenSurvivor _frozenSurvivor;

        // REPROMOTE-Item-001 — keycard doors on secure / keycard_door-tagged nodes.
        private Item_Keycards _keycards;

        // Prompt #67 — mutated flora / fauna rolled per looting tick.
        private MutatedEcosystemSystem _ecosystem;

        // Prompts #901/#903/#904 — narrative encounter trackers. NarrativeEncounters
        // registered the EncounterSOs, but nothing bound the classes that hold their
        // outcomes, so the encounters printed their text and did nothing.
        private Encounter_DeadLetterOffice _deadLetterOffice;
        private Encounter_WeatherStation _weatherStation;
        private Encounter_Pianist _pianist;

        // Host hooks the narrative outcomes need. Both optional: an unbound host
        // still resolves every other part of the choice.
        private Action<string, float> _modifyFactionTrust;
        private Action<string, bool> _setWorldFlag;

        public IReadOnlyList<ExpeditionState> ActiveExpeditions => _activeExpeditions;
        public IReadOnlyList<EncounterSO> EncounterPool => _encounterPool;
        public GeneratedMap GeneratedMap => _generatedMap;
        public SabotagedCacheSystem SabotagedCaches => _sabotagedCaches;

        /// <summary>
        /// REPROMOTE-Encounter-001 — wire live <see cref="Encounter_Roadblock"/> class tracker.
        /// Dispatched from psychology resolve when the SO id or map node is tagged roadblock.
        /// </summary>
        public void BindClassRoadblock(Encounter_Roadblock roadblock) =>
            _classRoadblock = roadblock;

        /// <summary>
        /// REPROMOTE-MapHazard-001 — wire VenusTrap for swamp-tagged looting nodes.
        /// </summary>
        public void BindVenusTrap(MapHazard_VenusTrap venusTrap) =>
            _venusTrap = venusTrap;

        /// <summary>Prompt #902 — wire frozen survivor for snowfield/frozen-lake nodes.</summary>
        public void BindFrozenSurvivor(MapHazard_FrozenSurvivor frozenSurvivor) =>
            _frozenSurvivor = frozenSurvivor;

        /// <summary>
        /// REPROMOTE-Item-001 — wire keycard tracker for secure-door looting nodes.
        /// </summary>
        public void BindKeycards(Item_Keycards keycards) =>
            _keycards = keycards;

        /// <summary>
        /// Prompt #67 — wire the mutated ecosystem so its stage actually reaches the
        /// map. <see cref="MutatedEcosystemSystem.RollEcosystemEncounter"/> documents
        /// this call site, but nothing bound it: the system advanced its mutation
        /// stage daily and saved it while flora and fauna could never be encountered.
        /// </summary>
        public void BindEcosystem(MutatedEcosystemSystem ecosystem) =>
            _ecosystem = ecosystem;

        /// <summary>Prompt #901 — wire the postal van so its choices resolve.</summary>
        public void BindDeadLetterOffice(Encounter_DeadLetterOffice office) =>
            _deadLetterOffice = office;

        /// <summary>Prompt #903 — wire the weather station so its choices resolve.</summary>
        public void BindWeatherStation(Encounter_WeatherStation station) =>
            _weatherStation = station;

        /// <summary>Prompt #904 — wire Matej so his choices resolve.</summary>
        public void BindPianist(Encounter_Pianist pianist) =>
            _pianist = pianist;

        /// <summary>
        /// Host hook for faction standing changes (delivering the bound letter).
        /// </summary>
        public void BindFactionTrustWriter(Action<string, float> modifyTrust) =>
            _modifyFactionTrust = modifyTrust;

        /// <summary>
        /// Host hook for world flags the narrative outcomes set (forecast boost,
        /// word of the bunker spreading).
        /// </summary>
        public void BindWorldFlagWriter(Action<string, bool> setWorldFlag) =>
            _setWorldFlag = setWorldFlag;

        // Events
        public event Action<ExpeditionState> OnExpeditionStarted;
        public event Action<ExpeditionState> OnExpeditionTick;
        public event Action<ExpeditionState, EncounterSO> OnEncounterTriggered;
        public event Action<ExpeditionState, EncounterSO, EventChoice> OnEncounterResolved;
        public event Action<ExpeditionState, List<ItemDefinition>> OnExpeditionCompleted;
        public event Action<ExpeditionState, string> OnExpeditionFailed;

        // Day-30 Flashpoint intercept events
        public event Action<ExpeditionState> OnFlashpointIntercepted;
        public event Action<ExpeditionState> OnHatchDilemmaReady;
        public event Action<ExpeditionState> OnHatchDilemmaResolved;

        /// <summary>Prompt #12 — UXO landmine detonated under the scavenger.</summary>
        public event Action<ExpeditionState, string> OnUxoDetonated;

        /// <summary>Prompt #13 — medical loot discarded after spotting a poisoned cache.</summary>
        public event Action<ExpeditionState, string> OnSabotagedCacheDetected;

        /// <summary>Prompt #13 — medical loot swapped for poisoned iodine (undetected).</summary>
        public event Action<ExpeditionState> OnSabotagedCachePlanted;

        /// <summary>Prompt #15 — Deserter's Stand narrative beat resolved (no combat).</summary>
        public event Action<ExpeditionState, string> OnDesertersStandResolved;

        /// <summary>Optional wiring for expedition simulation (weather, map knowledge, medical, etc.).</summary>
        public sealed class Config
        {
            public WeatherSystem WeatherSystem;
            public RadiationKnowledgeMap KnowledgeMap;
            public MedicalSystem MedicalSystem;
            public Shelter.Shelter Shelter;
            public IReadOnlyList<Survivor> Survivors;
            public int Seed = 42;

            /// <summary>
            /// Populate the built-in encounter pool on construction (default true).
            /// Set false when a test needs a deterministic expedition: random
            /// encounters can resolve to "flee", which flips the expedition to
            /// Inbound mid-travel and prevents arrival entirely.
            /// </summary>
            public bool CreateDefaultEncounters = true;
        }

        /// <summary>Precomputed path request from map UI (travel hours + hazard snapshot).</summary>
        public struct PathRequest
        {
            public string NodeId;
            public float TravelHours;
            public float TrueRad;
            public float DangerLevel;
            public string DisplayName;
            public ExpeditionStance Stance;
            public float MaxLootCapacity;
        }

        public ExpeditionSystem(
            RadiationSystem radSystem,
            Inventory.Inventory inventory,
            ItemCatalogSO itemCatalog,
            Config config = null)
        {
            config ??= new Config();
            _radSystem = radSystem;
            _inventory = inventory;
            _itemCatalog = itemCatalog;
            _weatherSystem = config.WeatherSystem;
            _knowledgeMap = config.KnowledgeMap;
            _medicalSystem = config.MedicalSystem;
            _shelter = config.Shelter;
            _survivors = config.Survivors;
            _rng = new System.Random(config.Seed);

            if (config.CreateDefaultEncounters)
                CreateDefaultEncounters();

            // Subscribe to the typed intercept signal published by the
            // FlashpointChoreographer's EMP step.
            //
            // NOT idempotent across instances: EventBus dedupes by delegate
            // equality (Target + Method), so a delegate bound to a *different*
            // ExpeditionSystem instance is a distinct handler and is added
            // alongside the existing one. Every instance that subscribes here
            // must be paired with UnsubscribeAll() or it leaks into the static
            // bus for the rest of the process. GameBootstrap.OnDestroy does
            // this; tests that construct an ExpeditionSystem directly must too.
            EventBus.Subscribe<FlashpointInterceptSignal>(HandleFlashpointIntercept);
            EventBus.Subscribe<HatchDilemmaResolvedSignal>(HandleHatchDilemmaResolved);
        }

        /// <summary>Compact overload used by most EditMode tests (core deps + seed only).</summary>
        public ExpeditionSystem(
            RadiationSystem radSystem,
            Inventory.Inventory inventory,
            ItemCatalogSO itemCatalog,
            int seed)
            : this(radSystem, inventory, itemCatalog, new Config { Seed = seed })
        {
        }

        /// <summary>
        /// H-2: Unsubscribe all EventBus handlers this ExpeditionSystem
        /// registered. Call this when the ExpeditionSystem is replaced
        /// (e.g. a "new game" flow or a test fixture). Idempotent.
        /// </summary>
        public void UnsubscribeAll()
        {
            EventBus.Unsubscribe<FlashpointInterceptSignal>(HandleFlashpointIntercept);
            EventBus.Unsubscribe<HatchDilemmaResolvedSignal>(HandleHatchDilemmaResolved);
        }

        /// <summary>Inject Prompt #13 sabotaged-cache system (optional; safe to skip in tests).</summary>
        public void SetNeedsSystem(NeedsSystem system)
        {
            _needsSystem = system;
        }

        public void SetSabotagedCacheSystem(SabotagedCacheSystem system)
        {
            _sabotagedCaches = system;
        }

        /// <summary>Inject Prompt #68 bicycle logistics (optional; safe to skip in tests).</summary>
        public void SetBicycleSystem(BicycleSystem system)
        {
            _bicycleSystem = system;
        }

        /// <summary>Inject Prompt #69 flooded-node hazards (optional; safe to skip in tests).</summary>
        public void SetFloodedNodeSystem(FloodedNodeSystem system)
        {
            _floodedNodeSystem = system;
        }

        /// <summary>Inject Prompt #569 river crossings / bridges (optional; safe to skip in tests).</summary>
        public void SetRiverNodeSystem(RiverNodeSystem system)
        {
            _riverNodeSystem = system;
        }

        /// <summary>Inject Prompt #551 blood toxicity bite retaliation (optional).</summary>
        public void SetBloodToxicitySystem(BloodToxicitySystem system)
        {
            _bloodToxicity = system;
        }

        /// <summary>
        /// Inventory query for expedition gear (bicycle, pump). Injected by
        /// GameBootstrap so ExpeditionSystem stays free of Inventory write APIs.
        /// </summary>
        public void SetHasItem(Func<string, bool> hasItem)
        {
            _hasItem = hasItem;
        }

        /// <summary>
        /// Inventory count/consume for river blockade tolls (fuel units).
        /// Optional — without these, blockades force boat/wade instead of auto-pay.
        /// </summary>
        public void SetItemHandlers(Func<string, int> countItem, Func<string, int, bool> consumeItem)
        {
            _countItem = countItem;
            _consumeItem = consumeItem;
        }

        /// <summary>Last river crossing method applied on expedition start (tests / UI).</summary>
        public string LastRiverCrossingMethod { get; private set; }

        /// <summary>One-shot rad from last river wade on expedition start.</summary>
        public float LastRiverWadeRad { get; private set; }

        /// <summary>Fuel units paid for last river blockade toll (0 if none).</summary>
        public int LastRiverTollPaid { get; private set; }

        /// <summary>Poison damage dealt to last biting attacker via blood toxicity.</summary>
        public float LastBiteRetaliationDamage { get; private set; }

        /// <summary>Last engaged combat shot via Item_AmmoTypes.ResolveHit (0 if none).</summary>
        public float LastCombatShotDamage { get; private set; }

        /// <summary>Ammo item id consumed on last engaged combat (null if none).</summary>
        public string LastCombatAmmoId { get; private set; }

        /// <summary>Morale delta applied by last combat ResolveHit (may be 0).</summary>
        public float LastCombatMoraleDelta { get; private set; }

        /// <summary>Health delta applied by last combat ResolveHit (negative = damage).</summary>
        public float LastCombatHealthDelta { get; private set; }

        /// <summary>Formatted encounter-log line for last combat shot (null if none).</summary>
        public string LastCombatLogLine { get; private set; }

        /// <summary>True when last shot took JHP/soft armor penalty.</summary>
        public bool LastCombatArmorPenalty { get; private set; }

        /// <summary>Ammo loot ids injected on last military/rebel loot roll.</summary>
        public List<string> LastFactionAmmoLootIds { get; } = new List<string>();

        /// <summary>World-item / attachment ids injected on last faction gear loot roll.</summary>
        public List<string> LastFactionWorldLootIds { get; } = new List<string>();

        /// <summary>Item id used as river blockade toll currency.</summary>
        public const string RiverTollFuelItemId = "fuel";

        /// <summary>
        /// Prompts #182–#188 — combat milestone perks (stealth kills, flees,
        /// confined fights, human kills, Looter's Reflex on flee).
        /// </summary>
        public void BindCombatPerks(
            CombatPerkSystem combatPerks,
            Func<int> getDay = null,
            InterpersonalAffinity affinity = null,
            Func<IReadOnlyList<Survivor>> getAllSurvivors = null)
        {
            _combatPerks = combatPerks;
            _getDay = getDay;
            _affinity = affinity;
            _getAllSurvivors = getAllSurvivors;
        }

        /// <summary>
        /// Wire structured caliber combat (ResolveHit) + faction ammo loot injection.
        /// <paramref name="ammoItemFactory"/> builds ItemDefinitions for rolled ammo ids
        /// (host typically looks up the item catalog or synthesizes a stackable Weapon).
        /// </summary>
        public void BindAmmoTypes(Item_AmmoTypes ammoTypes, Func<string, ItemDefinition> ammoItemFactory = null)
        {
            _ammoTypes = ammoTypes;
            _ammoItemFactory = ammoItemFactory;
        }

        /// <summary>
        /// Wire faction world-item / attachment loot (attachments extremely rare loose).
        /// </summary>
        public void BindWorldLoot(Func<string, ItemDefinition> worldItemFactory = null, bool enabled = true)
        {
            _worldItemFactory = worldItemFactory;
            _worldLootEnabled = enabled;
        }

        /// <summary>
        /// Prompts #206–#210 — expedition milestone perks (Pack Mule, Light Step,
        /// Urban Pathfinder, Night Terror, Forager).
        /// </summary>
        public void BindExpeditionPerks(
            ExpeditionPerkSystem expeditionPerks,
            Func<int> getDay = null,
            NoiseSystem noiseSystem = null,
            Func<bool> isStormActive = null)
        {
            _expeditionPerks = expeditionPerks;
            if (getDay != null) _getDay = getDay;
            _noiseSystem = noiseSystem;
            _isStormActive = isStormActive;
            EnsureForagerFoodItems();
        }

        /// <summary>
        /// Prompts #220–#224 — Warlord/Peacekeeper/Juggernaut/Apex/Survivalist
        /// expedition hooks (carry, stealth, meat, alone stamina).
        /// </summary>
        public void BindPersonalQuests(PersonalQuestSystem personalQuests) =>
            _personalQuests = personalQuests;

        /// <summary>Optional override for Forager wild-food item definitions.</summary>
        public void SetForagerFoodItems(ItemDefinition roots, ItemDefinition berries)
        {
            if (roots != null) _foragerRoots = roots;
            if (berries != null) _foragerBerries = berries;
        }

        public ExpeditionPerkSystem ExpeditionPerks => _expeditionPerks;

        public void SetEncounterPool(IEnumerable<EncounterSO> encounters)
        {
            _encounterPool.Clear();
            if (encounters != null)
            {
                _encounterPool.AddRange(encounters);
            }
            if (_encounterPool.Count == 0)
            {
                CreateDefaultEncounters();
            }
        }

        /// <summary>
        /// Append or replace a single encounter by id (Prompt #47 — radio intel inject).
        /// Does not wipe the default pool.
        /// </summary>
        public void AddEncounter(EncounterSO encounter)
        {
            if (encounter == null || string.IsNullOrEmpty(encounter.id)) return;
            for (int i = 0; i < _encounterPool.Count; i++)
            {
                if (_encounterPool[i] != null && _encounterPool[i].id == encounter.id)
                {
                    _encounterPool[i] = encounter;
                    return;
                }
            }
            _encounterPool.Add(encounter);
        }

        /// <summary>True if an encounter with the given id is currently in the pool.</summary>
        public bool HasEncounter(string encounterId)
        {
            if (string.IsNullOrEmpty(encounterId)) return false;
            for (int i = 0; i < _encounterPool.Count; i++)
            {
                if (_encounterPool[i] != null && _encounterPool[i].id == encounterId)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Weighted pick among pool entries eligible for the given location.
        /// Pure selection (no RNG chance-to-fire). Null if nothing qualifies.
        /// forceOnArrival encounters are excluded — they only fire via
        /// <see cref="TryFireForcedLocationEncounter"/> so they cannot re-roll.
        /// </summary>
        public EncounterSO PickEncounter(string locationId, ExpeditionStance stance, float dangerLevel)
        {
            float totalWeight = 0f;
            var valid = new List<EncounterSO>();
            var weights = new List<float>();

            for (int i = 0; i < _encounterPool.Count; i++)
            {
                var enc = _encounterPool[i];
                if (enc == null) continue;
                // Location-scripted beats (Safe Haven ambush/cache) must not
                // reappear in the random encounter roll after force-fire.
                if (enc.forceOnArrival) continue;
                float w = enc.GetEffectiveWeight(stance, dangerLevel, locationId);
                if (w <= 0f) continue;
                valid.Add(enc);
                weights.Add(w);
                totalWeight += w;
            }

            if (valid.Count == 0 || totalWeight <= 0f) return null;

            double roll = _rng.NextDouble() * totalWeight;
            float accum = 0f;
            for (int i = 0; i < valid.Count; i++)
            {
                accum += weights[i];
                if (roll <= accum) return valid[i];
            }
            return valid[valid.Count - 1];
        }

        /// <summary>Optional override for the Deserter's Stand service rifle loot.</summary>
        public void SetDeserterStandRifle(ItemDefinition rifle)
        {
            if (rifle != null) _deserterStandRifle = rifle;
        }

        /// <summary>Inject proc-gen wasteland map (visit/reveal on arrival).</summary>
        public void SetGeneratedMap(GeneratedMap map)
        {
            _generatedMap = map;
        }

        /// <summary>
        /// Start an expedition for a survivor to a target location node.
        /// Returns false if survivor is invalid, dead, or already on an expedition.
        /// Travel hours are multiplied by current weather (blizzards ×2).
        /// </summary>
        /// <summary>
        /// When true, all StartExpedition* paths return false (Prompt #48 —
        /// hatch Buried/Frozen). Expedition UI should treat this as disabled.
        /// </summary>
        public bool HatchBlocksExpeditions { get; set; }

        /// <summary>False while the hatch is sealed — drives map/expedition UI disable.</summary>
        public bool IsExpeditionUiEnabled => !HatchBlocksExpeditions;

    }
}
