using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Factions;
using AtomicWar._Game.Endgame;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Phases 9-10 — Wasteland Faction Arcs + Endgame Branching Refinement.
    ///
    /// Phase 9:
    ///   GarrisonConscriptionSystem  → Every 15 days from day 20
    ///   AshSignCultSystem          → Every 10 days from day 15
    ///   ScavengerRefugeSystem      → Every 20 days from day 10
    ///   GrainExchangeBlackMarket   → Trade relationship tracking (Phase 10)
    ///   TollmanFavorSystem         → Favor-based passage (Phase 10)
    ///
    /// Phase 10:
    ///   EndgameBranchingRefinement → Weighted path scoring
    ///   RadioBlackoutMystery       → Silent frequency investigation
    ///   MedicalQuarantine           → Hospital ethical dilemma
    ///   StrandedConvoy             → Salvage/rescue/walk-away
    ///   OrphanCare                 → Child integration
    /// </summary>
    public partial class GameBootstrap
    {
        // ── Phase 9-10 system accessors ────────────────────────────────

        public GarrisonConscriptionSystem GarrisonConscription { get; private set; }
        public AshSignCultSystem AshSignCult { get; private set; }
        public ScavengerRefugeSystem ScavengerRefuge { get; private set; }

        /// <summary>
        /// Call during InitializeSystems, after faction systems exist.
        /// </summary>
        private void InitPhases9to10Wiring()
        {
            InitPhase9Systems();
            WirePhase10EndgameRefinement();
        }

        // ═══════════════════════════════════════════════════════════════
        // Phase 9: Faction Arc Systems
        // ═══════════════════════════════════════════════════════════════

        private void InitPhase9Systems()
        {
            // ── Garrison Conscription ──────────────────────────────────
            GarrisonConscription = new GarrisonConscriptionSystem();
            _registry.RegisterDaily("garrisonConscription",
                d => GarrisonConscription.Tick(d, Survivors,
                    new System.Random(_worldSeed + 51)));
            _registry.Register<GarrisonConscriptionSystem>(GarrisonConscription);

            // Wire conscription events
            GarrisonConscription.OnConscriptionDemand += (food, survivors) =>
            {
                GameLog.Log($"[Garrison] Conscription demand: {food} food, " +
                    $"{survivors} young survivor(s) for military service.");
            };
            GarrisonConscription.OnPunitiveRaidTriggered += () =>
            {
                GameLog.Log("[Garrison] Punitive raid incoming — refused conscription.");
                // Trigger a raid via HatchDefenseSystem
                HatchDefenseSystem?.ForceRaid("garrison_punitive");
            };

            // ── Ash Sign Cult ──────────────────────────────────────────
            AshSignCult = new AshSignCultSystem();
            _registry.RegisterDaily("ashSignCult",
                d => AshSignCult.Tick(d, Survivors,
                    new System.Random(_worldSeed + 53)));
            _registry.Register<AshSignCultSystem>(AshSignCult);

            AshSignCult.OnRitualOffered += (sv) =>
            {
                GameLog.Log($"[Cult] Ritual offered to {sv.DisplayName}: " +
                    "24h in irradiated hotspot for herbal remedy.");
            };

            // ── Scavenger Refuge ───────────────────────────────────────
            ScavengerRefuge = new ScavengerRefugeSystem();
            _registry.RegisterDaily("scavengerRefuge",
                d => ScavengerRefuge.Tick(d,
                    new System.Random(_worldSeed + 55),
                    () => Shelter?.GetAvailableBedCount() ?? 4));
            _registry.Register<ScavengerRefugeSystem>(ScavengerRefuge);

            ScavengerRefuge.OnRefugeesArrived += (count) =>
            {
                GameLog.Log($"[Refuge] {count} desperate civilians " +
                    "seeking shelter at the bunker hatch.");
            };
        }

        // ═══════════════════════════════════════════════════════════════
        // Phase 10: Endgame Branching Refinement
        // ═══════════════════════════════════════════════════════════════

        private void WirePhase10EndgameRefinement()
        {
            // Endgame paths are already weighted by EndgameEngine.Evaluate()
            // which checks accumulated campaign state. The expansion variables
            // (MoralBranchDirection, FactionStandings, SurvivorSurvivorship,
            // RelicsRestored, FinalWishesCompleted) are read directly from
            // Survivor fields and SaveSystem world flags at evaluation time.
            //
            // Future work: add a weighted scoring pass that checks:
            //   - TheBroadcast: high compassion + knowledge + relics
            //   - TheUnifier: high faction standings + leader alive
            //   - BuriedAlive: low survivors + low faction standings
            //   - Migration: high survivors + compassion
            //   - MAD Protocol: high numbed + low faction standings
        }
    }
}
