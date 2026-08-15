using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Factions;
using AtomicWar._Game.Endgame;
using AtomicWar._Game.Utilities;

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
            _registry.RegisterDaily("garrison_conscription",
                d => GarrisonConscription.Tick(d, Survivors,
                    new System.Random(_worldSeed + 51)));
            _registry.Register<GarrisonConscriptionSystem>(GarrisonConscription);

            // Wire conscription events
            Action<int, int> onConscriptionDemand = (food, survivorsCount) =>
            {
                GameLog.Log($"[Garrison] Conscription demand: {food} food, " +
                    $"{survivorsCount} young survivor(s) for military service.");
            };
            GarrisonConscription.OnConscriptionDemand += onConscriptionDemand;
            _subscriptions.Track(() =>
                GarrisonConscription.OnConscriptionDemand -= onConscriptionDemand);

            Action onPunitiveRaid = () =>
            {
                GameLog.Log("[Garrison] Punitive raid incoming — refused conscription.");
                HatchDefenseSystem?.OpenRaidWindow();
            };
            GarrisonConscription.OnPunitiveRaidTriggered += onPunitiveRaid;
            _subscriptions.Track(() =>
                GarrisonConscription.OnPunitiveRaidTriggered -= onPunitiveRaid);

            // ── Ash Sign Cult ──────────────────────────────────────────
            AshSignCult = new AshSignCultSystem();
            _registry.RegisterDaily("ash_sign_cult",
                d => AshSignCult.Tick(d, Survivors,
                    new System.Random(_worldSeed + 53)));
            _registry.Register<AshSignCultSystem>(AshSignCult);

            Action<Survivor> onRitualOffered = (sv) =>
            {
                GameLog.Log($"[Cult] Ritual offered to {sv.DisplayName}: " +
                    "24h in irradiated hotspot for herbal remedy.");
            };
            AshSignCult.OnRitualOffered += onRitualOffered;
            _subscriptions.Track(() =>
                AshSignCult.OnRitualOffered -= onRitualOffered);

            // ── Scavenger Refuge ───────────────────────────────────────
            ScavengerRefuge = new ScavengerRefugeSystem();
            _registry.RegisterDaily("scavenger_refuge",
                d => ScavengerRefuge.Tick(d,
                    new System.Random(_worldSeed + 55),
                    () => 4));
            _registry.Register<ScavengerRefugeSystem>(ScavengerRefuge);

            Action<int> onRefugeesArrived = (count) =>
            {
                GameLog.Log($"[Refuge] {count} desperate civilians " +
                    "seeking shelter at the bunker hatch.");
            };
            ScavengerRefuge.OnRefugeesArrived += onRefugeesArrived;
            _subscriptions.Track(() =>
                ScavengerRefuge.OnRefugeesArrived -= onRefugeesArrived);
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
