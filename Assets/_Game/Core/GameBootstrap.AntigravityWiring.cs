using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Environment;
using AtomicWar._Game.World;
using AtomicWar._Game.Factions;
using AtomicWar._Game.Narrative;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Antigravity Expansion (Mechanics #41–80) — System construction,
    /// wiring, and tick registration for all 40 new mechanics across
    /// Sections V–VIII.
    ///
    /// Phase 16: #41–50 Physical & Mental Quirks
    /// Phase 17: #51–60 Environmental Narrative
    /// Phase 19: #71–80 Legacy & Long-Term Arcs
    /// (Phase 18 #61–70 are JSON events — no new C# systems)
    /// </summary>
    public partial class GameBootstrap
    {
        // ── Antigravity system accessors ──────────────────────────────

        public TinnitusSystem TinnitusSystem { get; private set; }
        public SleepwalkingSystem SleepwalkingSystem { get; private set; }
        public HoardingBehaviorSystem HoardingBehaviorSystem { get; private set; }
        public NerveDamageSystem NerveDamageSystem { get; private set; }
        public AshDriftBurialSystem AshDriftBurialSystem { get; private set; }
        public LocationEvolutionSystem LocationEvolutionSystem { get; private set; }
        public WildlifeMigrationSystem WildlifeMigrationSystem { get; private set; }
        public LandmarkDegradationSystem LandmarkDegradationSystem { get; private set; }
        public BunkerManifestoSystem BunkerManifestoSystem { get; private set; }
        public CulturalPreservationSystem CulturalPreservationSystem { get; private set; }
        public DeepAquiferProjectSystem DeepAquiferProjectSystem { get; private set; }
        public PeaceTreatySystem PeaceTreatySystem { get; private set; }

        /// <summary>
        /// Call during InitializeSystems after all core systems exist.
        /// </summary>
        private void InitAntigravitySystems()
        {
            InitAntigravityPhase16();
            InitAntigravityPhase17();
            InitAntigravityPhase19();
        }

        // ═══════════════════════════════════════════════════════════════
        // Phase 16: Physical & Mental Quirks (#41–50)
        // ═══════════════════════════════════════════════════════════════

        private void InitAntigravityPhase16()
        {
            // #42: Tinnitus System
            TinnitusSystem = new TinnitusSystem
            {
                ApplyMoraleDelta = (sv, d) =>
                {
                    if (sv?.Needs != null && NeedsSystem != null)
                        NeedsSystem.Modify(sv, NeedKind.Morale, d);
                },
                Rng = new System.Random(_worldSeed + 61)
            };
            _registry.RegisterPerSubstep("tinnitus",
                h => TickTinnitusSurvivors(h));
            _registry.Register<TinnitusSystem>(TinnitusSystem);

            // Wire tinnitus to explosion/raid events
            if (HatchDefenseSystem != null)
            {
                Action<RaidResolution> onRaid = (r) =>
                {
                    if (r != null && r.Launched)
                        TinnitusSystem.OnExplosionEvent(0.6f, Survivors);
                };
                HatchDefenseSystem.OnRaidResolved += onRaid;
                _subscriptions.Track(() =>
                    HatchDefenseSystem.OnRaidResolved -= onRaid);
            }

            // #43: Sleepwalking System
            SleepwalkingSystem = new SleepwalkingSystem
            {
                GetMissedNights = sv =>
                    SleepDeprivation?.GetOrCreate(sv.Id)?.MissedNights ?? 0,
                GetStressLevel = sv =>
                    sv.GuiltInsomniaSeverity,
                GetRoomIds = () =>
                {
                    var ids = new List<string>();
                    if (Shelter?.Rooms != null)
                        foreach (var room in Shelter.Rooms)
                            ids.Add(room.RoomId);
                    return ids;
                },
                IsRoomHazardous = roomId =>
                    roomId == "plant" || roomId == "deep_vault",
                IsHatchLocked = () => false, // Shelter has no IsHatchLocked API yet
                UnlockHatch = () => { },     // Shelter has no UnlockHatch API yet
                MoveFoodItems = (itemId, count) =>
                    Inventory?.RemoveById(itemId, count),
                Rng = new System.Random(_worldSeed + 63)
            };
            _registry.RegisterDaily("sleepwalking",
                d => SleepwalkingSystem.TickNightCheck(Survivors, d));
            _registry.Register<SleepwalkingSystem>(SleepwalkingSystem);

            // #44: Hoarding Behavior System
            HoardingBehaviorSystem = new HoardingBehaviorSystem
            {
                GetDaysStarved = sv =>
                {
                    // Count days where hunger was critical
                    return sv.Needs?.WasHungerCritical == true ? 5 : 0;
                },
                TryRemoveFromPantry = (itemId, count) =>
                    Inventory != null && Inventory.RemoveById(itemId, count),
                ApplyMoraleDelta = (sv, d) =>
                {
                    if (sv?.Needs != null && NeedsSystem != null)
                        NeedsSystem.Modify(sv, NeedKind.Morale, d);
                },
                AdjustAffinity = (a, b, d) =>
                    MentalBreakSystem?.Affinity?.Adjust(a, b, d),
                Rng = new System.Random(_worldSeed + 65)
            };
            _registry.RegisterDaily("hoardingBehavior",
                d => HoardingBehaviorSystem.TickDaily(Survivors, d));
            _registry.Register<HoardingBehaviorSystem>(HoardingBehaviorSystem);

            // #46: Nerve Damage System
            NerveDamageSystem = new NerveDamageSystem
            {
                GetDaysSinceLastWoundTreatment = sv =>
                {
                    // MedicalSystem has no DaysSinceLastTreatment API; placeholder
                    return 0f;
                },
                ApplyCraftingSpeedPenalty = (sv, penalty) =>
                {
                    // Placeholder — wired into CraftingSystem in UI layer
                },
                Rng = new System.Random(_worldSeed + 67)
            };
            _registry.RegisterPerSubstep("nerveDamage",
                h => TickNerveDamageSurvivors(h));
            _registry.Register<NerveDamageSystem>(NerveDamageSystem);
        }

        // ═══════════════════════════════════════════════════════════════
        // Phase 17: Environmental Narrative (#51–60)
        // ═══════════════════════════════════════════════════════════════

        private void InitAntigravityPhase17()
        {
            // #52: Ash Drift Burial System
            AshDriftBurialSystem = new AshDriftBurialSystem();
            _registry.RegisterPerSubstep("ashDriftBurial",
                h => AshDriftBurialSystem.Tick(h));
            _registry.Register<AshDriftBurialSystem>(AshDriftBurialSystem);

            // Wire ash storms to burial
            if (WeatherSystem != null)
            {
                // Ash storms trigger ash accumulation. Track the subscription for
                // explicit teardown — the EventBus is process-wide and is NOT
                // cleared on scene teardown (GameBootstrap.Lifecycle.cs), so an
                // untracked lambda would leak + accumulate one stale handler per
                // reload (each storm would then fire OnAshStorm N times).
                Action<WeatherKind> onWeatherForAshBurial = weather =>
                {
                    if (weather == WeatherKind.Ashfall ||
                        weather == WeatherKind.FalloutStorm)
                        AshDriftBurialSystem.OnAshStorm(0.8f);
                };
                EventBus.Subscribe(onWeatherForAshBurial);
                _subscriptions.Track(() => EventBus.Unsubscribe(onWeatherForAshBurial));
            }

            // #55: Location Evolution System
            LocationEvolutionSystem = new LocationEvolutionSystem();
            _registry.RegisterDaily("locationEvolution",
                d => LocationEvolutionSystem.Tick(d,
                    new System.Random(_worldSeed + 71)));
            _registry.Register<LocationEvolutionSystem>(
                LocationEvolutionSystem);

            // Register known locations
            if (_locationCatalog != null)
            {
                foreach (var loc in _locationCatalog.locations)
                    LocationEvolutionSystem.RegisterLocation(loc.id);
            }

            // #57: Wildlife Migration System
            WildlifeMigrationSystem = new WildlifeMigrationSystem();
            _registry.RegisterDaily("wildlifeMigration",
                d => WildlifeMigrationSystem.Tick(d,
                    WeatherSystem != null ? WeatherSystem.Current.ToString().ToLowerInvariant() : "clear",
                    WeatherSystem?.Current == WeatherKind.FalloutStorm,
                    new System.Random(_worldSeed + 73)));
            _registry.Register<WildlifeMigrationSystem>(
                WildlifeMigrationSystem);

            // Register expedition zones
            string[] zones = { "industrial", "suburban", "military",
                "forest", "swamp", "plains", "ruins" };
            foreach (var z in zones)
                WildlifeMigrationSystem.RegisterZone(z);

            // #58: Landmark Degradation System
            LandmarkDegradationSystem = new LandmarkDegradationSystem();
            _registry.RegisterDaily("landmarkDegradation",
                d => LandmarkDegradationSystem.Tick(d,
                    WeatherSystem?.Current == WeatherKind.BlackRain,
                    new System.Random(_worldSeed + 75)));
            _registry.Register<LandmarkDegradationSystem>(
                LandmarkDegradationSystem);

            if (_locationCatalog != null)
            {
                foreach (var loc in _locationCatalog.locations)
                    LandmarkDegradationSystem.RegisterLandmark(loc.id);
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // Phase 19: Legacy & Long-Term Arcs (#71–80)
        // ═══════════════════════════════════════════════════════════════

        private void InitAntigravityPhase19()
        {
            // #71: Bunker Manifesto System
            BunkerManifestoSystem = new BunkerManifestoSystem();
            _registry.RegisterPerSubstep("bunkerManifesto",
                h => TickBunkerManifestoSurvivors(h));
            _registry.Register<BunkerManifestoSystem>(BunkerManifestoSystem);

            // #73: Cultural Preservation System
            CulturalPreservationSystem = new CulturalPreservationSystem();
            _registry.RegisterEventDriven("culturalPreservation");
            _registry.Register<CulturalPreservationSystem>(
                CulturalPreservationSystem);

            // Wire artifact preservation to item acquisition
            if (Inventory != null)
            {
                Action<ItemDefinition, int> onItemAdded = (itemDef, amount) =>
                {
                    if (itemDef?.id == null) return;
                    // Cultural artifacts: books, records, artwork
                    if (itemDef.id.Contains("book") ||
                        itemDef.id.Contains("vinyl") ||
                        itemDef.id.Contains("art") ||
                        itemDef.id.Contains("painting") ||
                        itemDef.id.Contains("record"))
                    {
                        var sv = Survivors != null && Survivors.Count > 0
                            ? Survivors[0] : null;
                        CulturalPreservationSystem.PreserveArtifact(
                            itemDef.id, sv);
                    }
                };
                Inventory.OnItemAdded += onItemAdded;
                _subscriptions.Track(() =>
                    Inventory.OnItemAdded -= onItemAdded);
            }

            // #75: Deep Aquifer Project System
            DeepAquiferProjectSystem = new DeepAquiferProjectSystem();
            _registry.RegisterEventDriven("deepAquiferProject");
            _registry.Register<DeepAquiferProjectSystem>(
                DeepAquiferProjectSystem);

            // #78: Peace Treaty System
            PeaceTreatySystem = new PeaceTreatySystem();
            _registry.RegisterEventDriven("peaceTreaty");
            _registry.Register<PeaceTreatySystem>(PeaceTreatySystem);
        }

        // ── Antigravity tick helpers ──────────────────────────────────

        private void TickTinnitusSurvivors(float gameHours)
        {
            if (Survivors == null) return;
            bool raidWarning = HatchDefenseSystem?.LastResolution?.Launched ?? false;
            for (int i = 0; i < Survivors.Count; i++)
                TinnitusSystem?.Tick(Survivors[i], gameHours, raidWarning);
        }

        private void TickNerveDamageSurvivors(float gameHours)
        {
            if (Survivors == null) return;
            for (int i = 0; i < Survivors.Count; i++)
                NerveDamageSystem?.Tick(Survivors[i], gameHours);
        }

        private void TickBunkerManifestoSurvivors(float gameHours)
        {
            if (Survivors == null) return;
            for (int i = 0; i < Survivors.Count; i++)
                BunkerManifestoSystem?.TickSurvivor(Survivors[i], gameHours);
        }
    }
}
