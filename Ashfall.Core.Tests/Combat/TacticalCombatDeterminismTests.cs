using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;

namespace Ashfall.Core.Tests.Combat
{
    public class TacticalCombatDeterminismTests
    {
        public TacticalCombatDeterminismTests()
        {
            CombatCatalog.SeedDefaults();
        }

        private static List<CombatantState> CreateRoster(int count = 2)
        {
            var roster = new List<CombatantState>();
            for (int i = 0; i < count; i++)
            {
                roster.Add(new CombatantState
                {
                    Id = "p_" + i,
                    Name = "Survivor " + i,
                    SurvivorId = "survivor_" + i,
                    IsPlayer = true,
                    Health = 100,
                    MaxHealth = 100,
                    ArmorRating = 0.3f,
                    CoverRating = 0.4f
                });
            }
            return roster;
        }

        private static List<WeaponInstanceState> CreateWeapons(int count = 2)
        {
            var list = new List<WeaponInstanceState>();
            for (int i = 0; i < count; i++)
            {
                list.Add(new WeaponInstanceState
                {
                    InstanceId = "w_inst_" + i,
                    WeaponId = "weapon_assault_rifle",
                    OwnerSurvivorId = "survivor_" + i,
                    ConditionPct = 0.85f,
                    AmmoId = "ammo_556",
                    AmmoRemaining = 40
                });
            }
            return list;
        }

        [Fact]
        public void B3_005_SameSeedReplayProducesIdenticalEventSequence()
        {
            // Two combat systems with identical initial state, seed, and command sequence
            int seed = 777123;

            var sys1 = new TacticalCombatSystem();
            sys1.BeginEncounter("enc_replay", "exp_1", "loc_alpha", "Alpha Ruins", 1, seed, CreateRoster(), CreateWeapons(), enemyCount: 2, enemyHealth: 30);

            var sys2 = new TacticalCombatSystem();
            sys2.BeginEncounter("enc_replay", "exp_1", "loc_alpha", "Alpha Ruins", 1, seed, CreateRoster(), CreateWeapons(), enemyCount: 2, enemyHealth: 30);

            var rng1 = new SeededRng(seed);
            var rng2 = new SeededRng(seed);

            var events1 = sys1.ResolveToEnd(rng1, maxTurns: 20);
            var events2 = sys2.ResolveToEnd(rng2, maxTurns: 20);

            Assert.Equal(events1.Count, events2.Count);
            for (int i = 0; i < events1.Count; i++)
            {
                Assert.Equal(events1[i].Kind, events2[i].Kind);
                Assert.Equal(events1[i].Turn, events2[i].Turn);
                Assert.Equal(events1[i].Detail, events2[i].Detail);
            }
            Assert.Equal(sys1.State.Resolved, sys2.State.Resolved);
            Assert.Equal(sys1.State.Phase, sys2.State.Phase);
            Assert.Equal(sys1.State.OutcomeText, sys2.State.OutcomeText);
        }

        [Fact]
        public void B3_007_WeaponConditionConversionPinned()
        {
            // Check conversion accuracy and clamping
            Assert.Equal(0.75f, WeaponEquipmentBridge.ConditionToCombatPct(75f), 4);
            Assert.Equal(1.0f, WeaponEquipmentBridge.ConditionToCombatPct(120f), 4);
            Assert.Equal(0.0f, WeaponEquipmentBridge.ConditionToCombatPct(-10f), 4);

            Assert.Equal(75f, WeaponEquipmentBridge.CombatPctToEquipmentCondition(0.75f), 4);
            Assert.Equal(100f, WeaponEquipmentBridge.CombatPctToEquipmentCondition(1.5f), 4);
            Assert.Equal(0f, WeaponEquipmentBridge.CombatPctToEquipmentCondition(-0.2f), 4);

            // Round trip
            float initial = 83.5f;
            float combatVal = WeaponEquipmentBridge.ConditionToCombatPct(initial);
            float restored = WeaponEquipmentBridge.CombatPctToEquipmentCondition(combatVal);
            Assert.Equal(initial, restored, 2);
        }

        [Fact]
        public void B3_009_CombatDoctrineCapability_PureProjectionFromResearch()
        {
            var unresearched = CombatDoctrineCapability.FromResearch(k => false);
            Assert.False(unresearched.HasCombatTraining);
            Assert.Equal(0f, unresearched.AccuracyBonus);
            Assert.Equal(0f, unresearched.TacticalMobilityBonus);

            var researched = CombatDoctrineCapability.FromResearch(k => k == "knowledge_combat_training" || k == "knowledge_fortified_chokepoints");
            Assert.True(researched.HasCombatTraining);
            Assert.Equal(0.05f, researched.AccuracyBonus, 3);
            Assert.Equal(0.05f, researched.TacticalMobilityBonus, 3);
            Assert.True(researched.HasFortifiedChokepoints);
            Assert.Equal(0.20f, researched.BarrierIntegrityBonus, 3);

            // Verify sys applies doctrine
            var sys = new TacticalCombatSystem();
            sys.DoctrineCapability = researched;
            Assert.Same(researched, sys.DoctrineCapability);
        }

        [Fact]
        public void B3_010_TraumaAndMoraleConsequencesAppliedExactlyOnce()
        {
            int traumaCount = 0;
            float moraleTotal = 0f;
            var ports = new CombatHostPorts(
                applyMoraleDelta: (id, m) => moraleTotal += m,
                raiseTrauma: (id, tag, s) => traumaCount++);

            var sys = new TacticalCombatSystem(null, ports);
            sys.BeginEncounter("enc_trauma", "exp_1", "loc_alpha", "Alpha Ruins", 1, 1234, CreateRoster(1), CreateWeapons(1), enemyCount: 1, enemyHealth: 10);

            var rng = new SeededRng(1234);
            sys.ResolveToEnd(rng, maxTurns: 10);

            Assert.True(sys.State.Resolved);
            Assert.NotNull(sys.State.Aftermath);
            Assert.True(sys.State.Aftermath.IsApplied);
            Assert.StartsWith("cres_", sys.State.ResolutionId);

            float initialMorale = moraleTotal;
            int initialTrauma = traumaCount;

            // Subsequent checks or calls to BuildAndApplyAftermath must not duplicate
            sys.BuildAndApplyAftermath("Won", +5f);
            Assert.Equal(initialMorale, moraleTotal);
            Assert.Equal(initialTrauma, traumaCount);

            // Restore from save must be silent and not re-apply aftermath
            var saved = sys.CaptureState();
            var sys2 = new TacticalCombatSystem(null, ports);
            sys2.RestoreState(saved);

            Assert.Equal(initialMorale, moraleTotal);
            Assert.Equal(initialTrauma, traumaCount);
            Assert.NotNull(sys2.State.Aftermath);
            Assert.True(sys2.State.Aftermath.IsApplied);
            Assert.Equal(sys.State.ResolutionId, sys2.State.ResolutionId);
        }

        [Fact]
        public void B3_011_WeaponWearTrackedInAftermathAndPreservedAcrossSave()
        {
            var sys = new TacticalCombatSystem();
            var weapons = CreateWeapons(1);
            weapons[0].ConditionPct = 0.80f;

            sys.BeginEncounter("enc_wear", "exp_1", "loc_alpha", "Alpha Ruins", 1, 4567, CreateRoster(1), weapons, enemyCount: 1, enemyHealth: 15);

            // Mid-combat degradation
            WeaponConditionSystem.Degrade(sys.State.Weapons[0], 0.05f); // 0.80 -> 0.75
            Assert.Equal(0.75f, sys.State.Weapons[0].ConditionPct, 2);

            // Save mid-encounter
            var saved = sys.CaptureState();
            Assert.Equal(0.80f, saved.GetBoundWeaponStartCondition("w_inst_0"), 2);

            // Restore into fresh system
            var sys2 = new TacticalCombatSystem();
            sys2.RestoreState(saved);
            Assert.Equal(0.80f, sys2.GetBoundWeaponStartCondition("w_inst_0"), 2);
            Assert.Equal(0.75f, sys2.State.Weapons[0].ConditionPct, 2);

            // Resolve combat
            sys2.BuildAndApplyAftermath("Won", +5f);
            Assert.NotNull(sys2.State.Aftermath);
            Assert.Single(sys2.State.Aftermath.WeaponWear);

            var wear = sys2.State.Aftermath.WeaponWear[0];
            Assert.Equal("w_inst_0", wear.InstanceId);
            Assert.Equal(0.80f, wear.StartConditionPct, 2);
            Assert.Equal(0.75f, wear.FinalConditionPct, 2);
            Assert.Equal(0.05f, wear.WearDeltaPct, 2);
        }

        [Fact]
        public void B3_012_AmmoDeductedDuringAction_AftermathSummarizesWithoutDoubleCharge()
        {
            int ammoCount = 50;
            var ports = new CombatHostPorts(
                consumeAmmo: (id, n) => { ammoCount -= n; return ammoCount; });

            var sys = new TacticalCombatSystem(null, ports);
            sys.BeginEncounter("enc_ammo", "exp_1", "loc_alpha", "Alpha Ruins", 1, 9999, CreateRoster(1), CreateWeapons(1), enemyCount: 1, enemyHealth: 30);

            var rng = new SeededRng(9999);
            string targetId = sys.State.Combatants[1].Id;

            int ammoBefore = ammoCount;
            var fireRes = sys.PlayerFire(targetId, rng);
            Assert.True(fireRes.Success);
            Assert.True(ammoCount < ammoBefore); // ammo consumed at action time

            int ammoAfterFire = ammoCount;
            sys.BuildAndApplyAftermath("Won", +5f);

            // Aftermath creation must NOT mutate ammo stock
            Assert.Equal(ammoAfterFire, ammoCount);
            Assert.NotEmpty(sys.State.Aftermath.AmmoSpent);
        }

        [Fact]
        public void B3_016_MidEncounterSaveParity_ContinuationProducesIdenticalOutcome()
        {
            int seed = 444333;
            // Run system 1 uninterrupted to turn 3
            var sys1 = new TacticalCombatSystem();
            sys1.BeginEncounter("enc_cont", "exp_1", "loc_alpha", "Alpha Ruins", 1, seed, CreateRoster(2), CreateWeapons(2), enemyCount: 3, enemyHealth: 40);

            var rng1 = new SeededRng(seed);
            string enemyId1 = sys1.State.Combatants[2].Id;
            sys1.PlayerFire(enemyId1, rng1);
            sys1.EndTurn(rng1);

            // Mid-encounter save of sys1
            var midSave = sys1.CaptureState();

            // Continue sys1 uninterrupted with deterministic turn 2 RNG
            var rngTurn2_A = new SeededRng(seed + 1);
            sys1.PlayerFire(enemyId1, rngTurn2_A);
            sys1.EndTurn(rngTurn2_A);

            // Create sys2 from mid-encounter save and continue with identical seed/commands
            var sys2 = new TacticalCombatSystem();
            sys2.RestoreState(midSave);
            var rngTurn2_B = new SeededRng(seed + 1);
            string enemyId2 = sys2.State.Combatants[2].Id;
            sys2.PlayerFire(enemyId2, rngTurn2_B);
            sys2.EndTurn(rngTurn2_B);

            Assert.Equal(sys1.State.Turn, sys2.State.Turn);
            Assert.Equal(sys1.State.Combatants.Count, sys2.State.Combatants.Count);
            for (int i = 0; i < sys1.State.Combatants.Count; i++)
            {
                Assert.Equal(sys1.State.Combatants[i].Health, sys2.State.Combatants[i].Health, 2);
                Assert.Equal(sys1.State.Combatants[i].IsDowned, sys2.State.Combatants[i].IsDowned);
            }
        }

        [Fact]
        public void B3_018_ActionPreflight_ExplainsUnavailableActionReasons()
        {
            var sys = new TacticalCombatSystem();
            sys.BeginEncounter("enc_pf", "exp_1", "loc_alpha", "Alpha Ruins", 1, 1111, CreateRoster(1), CreateWeapons(1), enemyCount: 1, enemyHealth: 20);

            // Valid target preflight
            string targetId = sys.State.Combatants[1].Id;
            var pf1 = sys.EvaluateFire(targetId);
            Assert.True(pf1.CanExecute);
            Assert.Empty(pf1.Reason);

            // Invalid target preflight
            var pfInvalid = sys.EvaluateFire("non_existent_target");
            Assert.False(pfInvalid.CanExecute);
            Assert.Contains("target", pfInvalid.Reason, StringComparison.OrdinalIgnoreCase);

            // Jam weapon
            sys.State.Weapons[0].IsJammed = true;
            var pfJammed = sys.EvaluateFire(targetId);
            Assert.False(pfJammed.CanExecute);
            Assert.Contains("jammed", pfJammed.Reason, StringComparison.OrdinalIgnoreCase);

            // Clear jam preflight should now be legal
            var pfClear = sys.EvaluateClearJam("survivor_0");
            Assert.True(pfClear.CanExecute);

            // Once unjammed, Clear jam should explain not jammed
            sys.State.Weapons[0].IsJammed = false;
            var pfNotJammed = sys.EvaluateClearJam("survivor_0");
            Assert.False(pfNotJammed.CanExecute);
            Assert.Contains("not jammed", pfNotJammed.Reason, StringComparison.OrdinalIgnoreCase);

            // Retreat allowed in Hold stance
            var pfRetreat = sys.EvaluateRetreat();
            Assert.True(pfRetreat.CanExecute);

            // Switch to LastStand -> retreat blocked
            sys.SetStance(TacticalStance.LastStand);
            var pfRetreatLastStand = sys.EvaluateRetreat();
            Assert.False(pfRetreatLastStand.CanExecute);
            Assert.Contains("Last Stand", pfRetreatLastStand.Reason, StringComparison.OrdinalIgnoreCase);

            // Resolve encounter -> all actions blocked
            sys.BuildAndApplyAftermath("Won", +5f);
            sys.State.Resolved = true;
            Assert.False(sys.EvaluateFire(targetId).CanExecute);
            Assert.False(sys.EvaluateSuppress().CanExecute);
            Assert.False(sys.EvaluateEndTurn().CanExecute);
        }
    }
}
