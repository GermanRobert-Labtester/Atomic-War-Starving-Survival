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
    ///   WireHUD()           → … → WireExpansion4HudController() (lives in InitWorld.cs)
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
        [Tooltip("Paint pump for the four overlays above. Lives beside " +
                 "DiegeticHudController on the HUD; falls back to a HUD-hierarchy " +
                 "lookup when unassigned.")]
        [SerializeField] private Expansion4HudController    _expansion4HudController;

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
                var rooms = Shelter.Rooms;
                if (rooms != null)
                    foreach (var room in rooms)
                        StructuralEntropySystem.RegisterRoom(room);
            }

            // Atmosphere changes can accelerate carbonation.
            if (AtmosphereSystem != null)
            {
                AtmosphereSystem.OnAtmosphereChanged += StructuralEntropySystem.OnAtmosphereChanged;
                _subscriptions.Track(() => AtmosphereSystem.OnAtmosphereChanged -= StructuralEntropySystem.OnAtmosphereChanged);
            }

            // Surface spalling events to the event bus for diary/radio reactions.
            Action<SpallingEvent> onSpalling = spallingEvt =>
            {
                if (EventRunner != null)
                    EventRunner.RaiseSpallingOccurred(spallingEvt.RoomId);
            };
            StructuralEntropySystem.OnSpallingEventBus += onSpalling;
            _subscriptions.Track(() => StructuralEntropySystem.OnSpallingEventBus -= onSpalling);

            // 2. Lethe Protocol ---------------------------------------------------
            LetheProtocolSystem = new LetheProtocolSystem(CreateSaltedRng(_worldSeed, "lethe"));
            LetheProtocolSystem.BindDependencies(NeedsSystem, MentalBreakSystem);

            // Hook reservoir depletion into water-purifier volume events.
            if (WaterEconomySystem != null)
            {
                Action<float, IReadOnlyList<Survivor>> onWaterPurified = (volumeLitres, survivors) =>
                    LetheProtocolSystem.ConsumeAmnesticDose(volumeLitres, survivors ?? Survivors);
                WaterEconomySystem.OnWaterPurified += onWaterPurified;
                _subscriptions.Track(() => WaterEconomySystem.OnWaterPurified -= onWaterPurified);
            }

            // Expose Waking Sickness to the event bus.
            Action<WakingSicknessEvent> onWakingSickness = evt =>
            {
                if (EventRunner != null)
                    EventRunner.RaiseWakingSicknessStarted(evt.ReservoirLevel, evt.AffectedCount);
            };
            LetheProtocolSystem.OnWakingSicknessEventBus += onWakingSickness;
            _subscriptions.Track(() => LetheProtocolSystem.OnWakingSicknessEventBus -= onWakingSickness);

            // 3. Ozone Scourge ----------------------------------------------------
            // OzoneScourgeSystem reads WeatherSystem directly via its constructor.
            OzoneScourgeSystem = new OzoneScourgeSystem(WeatherSystem);
            OzoneScourgeSystem.BindNeedsSystem(NeedsSystem);

            // 4. Generational Psychology ------------------------------------------
            GenerationalPsychologySystem = new GenerationalPsychologySystem();
            GenerationalPsychologySystem.BindDependencies(NeedsSystem, MentalBreakSystem);

            // Agoraphobic panic → diary entry for narrative texture.
            Action<Survivor> onAgoraphobicPanic = sv =>
            {
                if (SurvivorDiaries != null && sv != null)
                    SurvivorDiaries.RecordEntry(sv, "Panic. The sky. I cannot.");
            };
            GenerationalPsychologySystem.OnAgoraphobicPanicAttack += onAgoraphobicPanic;
            _subscriptions.Track(() => GenerationalPsychologySystem.OnAgoraphobicPanicAttack -= onAgoraphobicPanic);
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
