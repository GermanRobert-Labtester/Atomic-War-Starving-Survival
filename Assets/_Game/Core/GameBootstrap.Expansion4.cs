using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansion IV — Chronos Decay &amp; Lethe Protocol wiring.
    ///
    /// Call order in GameBootstrap:
    ///   InitializeSystems() → … → InitExpansion4Systems()  (after InitMedicalSystems)
    ///   WireHUD()           → … → WireExpansion4Hud()       (at end of WireHUD)
    ///   DailyTick()         → … → DailyTickExpansion4()     (alongside other daily ticks)
    /// </summary>
    public partial class GameBootstrap
    {
        // ----------------------------------------------------------------
        // Expansion IV system properties
        // ----------------------------------------------------------------

        /// <summary>Expansion IV Ch.38.1 — rebar corrosion + spalling simulation.</summary>
        public StructuralEntropySystem StructuralEntropySystem { get; private set; }

        /// <summary>Expansion IV Ch.39 — amnestic reservoir depletion + Waking Sickness.</summary>
        public LetheProtocolSystem LetheProtocolSystem { get; private set; }

        /// <summary>Expansion IV Ch.40 — UV exposure damage when ozone layer is depleted.</summary>
        public OzoneScourgeSystem OzoneScourgeSystem { get; private set; }

        /// <summary>Expansion IV Ch.38.2 — generational cohort psychology (Bunker-Born).</summary>
        public GenerationalPsychologySystem GenerationalPsychologySystem { get; private set; }

        // ----------------------------------------------------------------
        // Inspector-wired overlay components (assign in Unity Inspector)
        // These live on the HUD or a dedicated Expansion IV overlay canvas.
        // ----------------------------------------------------------------
        [Header("Expansion IV Overlays")]
        [SerializeField] private StructuralStressWireframe _structuralWireframe;
        [SerializeField] private LetheDripGauge            _letheDripGauge;
        [SerializeField] private OzoneScourgeOverlay        _ozoneOverlayComp;
        [SerializeField] private MemoryFlashVignette        _memoryFlashComp;

        // Cached delegate guard — prevents double-subscribe across load-game restores.
        private Action<ComingOfAgeEvent> _onComingOfAgeHud;

        // ----------------------------------------------------------------
        // Initialisation
        // ----------------------------------------------------------------

        /// <summary>
        /// Constructs all four Expansion IV plain-C# systems and wires their
        /// dependencies into the existing foundation systems.
        /// Called from InitializeSystems() after InitMedicalSystems().
        /// </summary>
        private void InitExpansion4Systems()
        {
            // 1. Structural Entropy -----------------------------------------------
            StructuralEntropySystem = new StructuralEntropySystem();
            StructuralEntropySystem.BindDependencies(() => Survivors, NeedsSystem);

            // Register all shelter rooms that already exist at init time.
            if (Shelter != null)
            {
                var rooms = Shelter.GetAllRooms();
                if (rooms != null)
                    foreach (var room in rooms)
                        StructuralEntropySystem.RegisterRoom(room);
            }

            // Atmosphere changes can accelerate carbonation.
            if (AtmosphereSystem != null)
                AtmosphereSystem.OnAtmosphereChanged += StructuralEntropySystem.OnAtmosphereChanged;

            // Surface spalling events to the event bus for diary/radio reactions.
            StructuralEntropySystem.OnSpallingEventBus += spallingEvt =>
            {
                if (EventRunner != null)
                    EventRunner.RaiseSpallingOccurred(spallingEvt.RoomId);
            };

            // 2. Lethe Protocol ---------------------------------------------------
            LetheProtocolSystem = new LetheProtocolSystem(CreateSaltedRng(_worldSeed, "lethe"));
            LetheProtocolSystem.BindDependencies(NeedsSystem, MentalBreakSystem);

            // Hook reservoir depletion into water-purifier volume events.
            if (WaterEconomySystem != null)
                WaterEconomySystem.OnWaterPurified += (volumeLitres, survivors) =>
                    LetheProtocolSystem.ConsumeAmnesticDose(volumeLitres, survivors ?? Survivors);

            // Expose Waking Sickness to the event bus.
            LetheProtocolSystem.OnWakingSicknessEventBus += evt =>
            {
                if (EventRunner != null)
                    EventRunner.RaiseWakingSicknessStarted(evt.ReservoirLevel, evt.AffectedCount);
            };

            // 3. Ozone Scourge ----------------------------------------------------
            // OzoneScourgeSystem reads WeatherSystem directly via its constructor.
            OzoneScourgeSystem = new OzoneScourgeSystem(WeatherSystem);
            OzoneScourgeSystem.BindNeedsSystem(NeedsSystem);

            // 4. Generational Psychology ------------------------------------------
            GenerationalPsychologySystem = new GenerationalPsychologySystem();
            GenerationalPsychologySystem.BindDependencies(NeedsSystem, MentalBreakSystem);

            // Agoraphobic panic → diary entry for narrative texture.
            GenerationalPsychologySystem.OnAgoraphobicPanicAttack += sv =>
            {
                if (SurvivorDiaries != null && sv != null)
                    SurvivorDiaries.RecordEntry(sv, "Panic. The sky. I cannot.");
            };
        }

        // ----------------------------------------------------------------
        // HUD wiring
        // ----------------------------------------------------------------

        /// <summary>
        /// Wires the Expansion4HudController to all four systems.
        /// Called at the end of WireHUD() — after EnsureDiegeticHud() has run.
        /// </summary>
        private void WireExpansion4Hud()
        {
            // The Expansion4HudController is a sibling MonoBehaviour on the
            // same GameObject as DiegeticHudController. Fetch it from the
            // scene; it is guaranteed to exist once the HUD prefab is spawned.
            var exp4Ctrl = FindObjectOfType<Expansion4HudController>();
            if (exp4Ctrl == null)
            {
                // Not a hard error — the overlay prefab may not be in the scene yet
                // (e.g. stripped test builds). Log and continue gracefully.
                Debug.LogWarning("[Expansion4] Expansion4HudController not found in scene. " +
                                 "Attach it to the HUD GameObject to enable Expansion IV overlays.");
                return;
            }

            exp4Ctrl.Bind(
                entropySystem:  StructuralEntropySystem,
                letheSystem:    LetheProtocolSystem,
                ozoneSystem:    OzoneScourgeSystem,
                genSystem:      GenerationalPsychologySystem,
                getSurvivors:   () => Survivors,
                wireframe:      _structuralWireframe,
                dripGauge:      _letheDripGauge,
                ozoneComp:      _ozoneOverlayComp,
                flashComp:      _memoryFlashComp);

            // Subscribe the HUD to the coming-of-age event so the event footer
            // in the Generational panel is driven by an event, not just polling.
            // Unsubscribe any prior subscription first (hot-reload / load-game safety).
            if (_onComingOfAgeHud != null)
                GenerationalPsychologySystem.OnComingOfAgeEventBus -= _onComingOfAgeHud;

            _onComingOfAgeHud = evt =>
            {
                // GenerationalPsychologySystem.LastComingOfAgeEvent is already set
                // at this point; the controller's next Update() will pick it up.
                // Optionally also push a diegetic event panel notification here.
                if (_hud != null)
                    _hud.PushEventText(
                        $"{evt.DisplayName} has come of age. " +
                        "They have never known sunlight.");
            };
            GenerationalPsychologySystem.OnComingOfAgeEventBus += _onComingOfAgeHud;
        }

        // ----------------------------------------------------------------
        // Daily tick
        // ----------------------------------------------------------------

        /// <summary>
        /// Drives all Expansion IV systems that require a daily cadence.
        /// Call from the existing daily-tick site in GameBootstrap.TickSystems.cs.
        /// </summary>
        public void DailyTickExpansion4(int currentDay)
        {
            // Structural entropy — corrosion accumulates each game-day.
            // Pass 24 game-hours as the tick unit (one full day).
            StructuralEntropySystem?.Tick(24f);

            // Generational psychology — age advancement and coming-of-age checks.
            GenerationalPsychologySystem?.DailyTick(currentDay, Survivors);

            // Lethe & Ozone don't have daily ticks; they run off water consumption
            // and weather-change events respectively.
        }
    }
}
