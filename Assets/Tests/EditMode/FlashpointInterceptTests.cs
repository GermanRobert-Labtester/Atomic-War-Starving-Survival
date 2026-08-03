using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Day-30 "caught outside" flashpoint protocol (Prompt #26).
    ///
    /// Verifies the full chain: EMP fires → ExpeditionSystem severs comms →
    /// trait-driven behavior resolves → arrival triggers the hatch dilemma.
    ///
    /// The spec's headline test: "Seed a Paranoid survivor on an expedition
    /// 10 ticks away. Trigger Day 30 Flashpoint. Assert UI goes dark,
    /// inventory is emptied, ETA is halved, and they arrive with a Trauma
    /// Affliction." That lives in <see cref="ParanoidSurvivor_OnFlashpoint_DropsAllLootAndHalvesEta"/>.
    /// </summary>
    [TestFixture]
    public class FlashpointInterceptTests
    {
        private const float Eps = 1e-3f;

        private NeedsProfile _needsProfile;
        private NeedsSystem _needsSystem;
        private RadiationSystem _radSystem;
        private Inventory _inventory;
        private ItemCatalogSO _itemCatalog;
        private ItemDefinition _foodItem;
        private ItemDefinition _waterItem;
        private LocationDefinitionSO _location;
        private Shelter _shelter;
        private MedicalSystem _medicalSystem;
        private ExpeditionSystem _expeditionSystem;

        [SetUp]
        public void SetUp()
        {
            // Clear the static EventBus between tests so subscribers from
            // other test fixtures don't leak in.
            EventBus.Clear();

            _needsProfile = ScriptableObject.CreateInstance<NeedsProfile>();
            _needsProfile.hungerPerHour = 1f;
            _needsProfile.thirstPerHour = 1f;
            _needsProfile.fatiguePerHour = 0.5f;

            _needsSystem = new NeedsSystem(_needsProfile, sv => true);
            _radSystem = new RadiationSystem(_needsSystem);
            _inventory = new Inventory { Capacity = 50, MaxWeight = 200f };

            _foodItem = ScriptableObject.CreateInstance<ItemDefinition>();
            _foodItem.id = "canned_food";
            _foodItem.displayName = "Canned Food";
            _foodItem.weight = 0.5f;

            _waterItem = ScriptableObject.CreateInstance<ItemDefinition>();
            _waterItem.id = "clean_water";
            _waterItem.displayName = "Clean Water";
            _waterItem.weight = 1.0f;

            _itemCatalog = ScriptableObject.CreateInstance<ItemCatalogSO>();
            _itemCatalog.items = new List<ItemDefinition> { _foodItem, _waterItem };

            _location = ScriptableObject.CreateInstance<LocationDefinitionSO>();
            _location.id = "ruined_suburb";
            _location.displayName = "Ruined Suburb";
            _location.travelHours = 10f; // 10-tick round trip per the spec
            _location.baseRadsPerHour = 10f;
            _location.dangerLevel = 1f;

            _shelter = new Shelter();

            // Register the broken-bone affliction so the intercept can inflict it.
            _medicalSystem = new MedicalSystem(_needsSystem, _inventory, _shelter);
            foreach (var aff in MedicalSystem.CreateDefaultAfflictions())
            {
                _medicalSystem.RegisterAffliction(aff);
            }

            _expeditionSystem = new ExpeditionSystem(
                _radSystem, _inventory, _itemCatalog,
                weatherSystem: null, knowledgeMap: null,
                medicalSystem: _medicalSystem, seed: 42);
        }

        [TearDown]
        public void TearDown()
        {
            EventBus.Clear();
        }

        // -------------------------------------------------------------------
        // Helpers
        // -------------------------------------------------------------------

        private Survivor MakeSurvivor(string id, RiskBiasTrait trait, float acuteDose = 0f)
        {
            var sv = new Survivor
            {
                Id = id,
                DisplayName = id,
                RiskBias = trait,
                AcuteDoseWindow = acuteDose
            };
            _needsSystem.Register(sv);
            _radSystem.Register(sv);
            return sv;
        }

        private void PublishIntercept(EmpResult result = default)
        {
            var active = _expeditionSystem.ActiveExpeditions;
            var snapshot = new List<ExpeditionState>(active.Count);
            for (int i = 0; i < active.Count; i++)
            {
                snapshot.Add(active[i]);
            }
            EventBus.Raise(new FlashpointInterceptSignal(result, snapshot));
        }

        // -------------------------------------------------------------------
        // Headline test (Prompt #26)
        // -------------------------------------------------------------------

        [Test]
        public void ParanoidSurvivor_OnFlashpoint_DropsAllLootAndHalvesEta()
        {
            var survivor = MakeSurvivor("sv_paranoid", RiskBiasTrait.Paranoid);
            bool started = _expeditionSystem.StartExpedition(survivor, _location, ExpeditionStance.Stealth);
            Assert.IsTrue(started);

            var exp = _expeditionSystem.GetExpeditionBySurvivor(survivor.Id);
            // Add loot so we can verify it gets dropped.
            exp.TryAddLoot(_foodItem);
            exp.TryAddLoot(_waterItem);
            Assert.AreEqual(2, exp.CollectedLoot.Count, "Setup: survivor should carry 2 loot items.");
            // Simulate mid-trip: 6 ticks outbound, 4 inbound remaining.
            exp.TravelTicksCompleted = 6;
            float preInterceptEta = exp.TravelTicksCompleted;

            float preAcute = survivor.AcuteDoseWindow;
            float preMorale = survivor.Needs.Morale;
            int preBrokenBoneCount = _medicalSystem.GetActive(survivor).Count;

            // Trigger the Day-30 flashpoint intercept.
            PublishIntercept();

            // 1. UI goes dark: isCommsSevered = true
            Assert.IsTrue(exp.isCommsSevered,
                "After the intercept, the expedition's comms should be severed.");

            // 2. Trauma affliction: broken_bone
            var active = _medicalSystem.GetActive(survivor);
            bool hasBrokenBone = false;
            for (int i = 0; i < active.Count; i++)
            {
                if (active[i].AfflictionId == AfflictionSO.Ids.BrokenBone) { hasBrokenBone = true; break; }
            }
            Assert.IsTrue(hasBrokenBone,
                "After the intercept, the survivor should be inflicted with a trauma affliction (broken_bone).");
            Assert.Greater(_medicalSystem.GetActive(survivor).Count, preBrokenBoneCount,
                "Affliction count should grow after the intercept.");

            // 3. Acute-dose spike (+30)
            Assert.AreEqual(preAcute + ExpeditionSystem.FlashpointAcuteDoseSpike, survivor.AcuteDoseWindow, Eps,
                "AcuteDoseWindow should spike by the configured amount on intercept.");

            // 4. Inventory emptied: Paranoid drops all loot
            Assert.AreEqual(0, exp.CollectedLoot.Count,
                "Paranoid survivor should drop ALL loot on the intercept.");
            Assert.AreEqual(0f, exp.CurrentWeight, Eps);

            // 5. ETA halved
            Assert.AreEqual(Mathf.RoundToInt(preInterceptEta * 0.5f), exp.TravelTicksCompleted,
                "Paranoid sprint should halve the remaining return ETA.");

            // 6. Trait behavior recorded
            Assert.AreEqual(FlashpointBehavior.ParanoidSprint, exp.flashpointBehavior,
                "Paranoid survivor should be tagged with ParanoidSprint behavior.");
            Assert.AreEqual(ExpeditionSystem.ParanoidSprintMultiplier, exp.returnSpeedMultiplier, Eps,
                "Paranoid survivor should have a sprint speed multiplier.");

            // 7. Morale NOT hit by the intercept itself (the EMP step applies
            //    the global morale hit; the intercept is a separate signal).
            Assert.AreEqual(preMorale, survivor.Needs.Morale, Eps);
        }

        // -------------------------------------------------------------------
        // Trait variants
        // -------------------------------------------------------------------

        [Test]
        public void RecklessSurvivor_OnFlashpoint_KeepsLootAndNormalSpeed()
        {
            var survivor = MakeSurvivor("sv_reckless", RiskBiasTrait.Reckless);
            _expeditionSystem.StartExpedition(survivor, _location, ExpeditionStance.Stealth);
            var exp = _expeditionSystem.GetExpeditionBySurvivor(survivor.Id);
            exp.TryAddLoot(_foodItem);
            exp.TryAddLoot(_waterItem);
            exp.TravelTicksCompleted = 8;

            PublishIntercept();

            Assert.IsTrue(exp.isCommsSevered);
            Assert.AreEqual(2, exp.CollectedLoot.Count,
                "Reckless survivor should keep all loot on the intercept.");
            Assert.AreEqual(FlashpointBehavior.RecklessPushThrough, exp.flashpointBehavior);
            Assert.AreEqual(1f, exp.returnSpeedMultiplier, Eps,
                "Reckless survivor should retain the default return speed multiplier.");
            Assert.AreEqual(1f, exp.returnSpeedDivisor, Eps);
        }

        [Test]
        public void CautiousSurvivor_OnFlashpoint_PausesInShelterAndGainsRadiationAnxiety()
        {
            var survivor = MakeSurvivor("sv_cautious", RiskBiasTrait.Cautious);
            _expeditionSystem.StartExpedition(survivor, _location, ExpeditionStance.Stealth);
            var exp = _expeditionSystem.GetExpeditionBySurvivor(survivor.Id);
            exp.TryAddLoot(_foodItem);
            exp.TravelTicksCompleted = 5;

            Assert.IsFalse(survivor.HasRadiationAnxietyStatus, "Setup: survivor should not start anxious.");

            PublishIntercept();

            Assert.IsTrue(exp.isCommsSevered);
            Assert.AreEqual(FlashpointBehavior.CautiousShelter, exp.flashpointBehavior);
            Assert.AreEqual(ExpeditionSystem.DefaultCautiousShelterDelayTicks, exp.shelterDelayTicksRemaining,
                "Cautious survivor should have a shelter delay set to the configured default.");
            Assert.IsTrue(survivor.HasRadiationAnxietyStatus,
                "Cautious survivor should gain the RadiationAnxiety status on the intercept.");
            Assert.AreEqual(1, exp.CollectedLoot.Count,
                "Cautious survivor should keep their loot (they took shelter, not fled).");
        }

        [Test]
        public void FatalistSurvivor_OnFlashpoint_HalvesSpeedAndGainsNumbness()
        {
            var survivor = MakeSurvivor("sv_fatalist", RiskBiasTrait.Fatalist);
            _expeditionSystem.StartExpedition(survivor, _location, ExpeditionStance.Stealth);
            var exp = _expeditionSystem.GetExpeditionBySurvivor(survivor.Id);
            exp.TryAddLoot(_foodItem);
            exp.TryAddLoot(_waterItem);
            exp.TravelTicksCompleted = 7;

            Assert.IsFalse(survivor.IsNumb, "Setup: survivor should not start numb.");

            PublishIntercept();

            Assert.IsTrue(exp.isCommsSevered);
            Assert.AreEqual(FlashpointBehavior.FatalistNumbWalk, exp.flashpointBehavior);
            Assert.AreEqual(ExpeditionSystem.FatalistSlowWalkDivisor, exp.returnSpeedDivisor, Eps,
                "Fatalist survivor should have a slow-walk divisor.");
            Assert.IsTrue(survivor.IsNumb,
                "Fatalist survivor should gain the Numb status on the intercept.");
            Assert.AreEqual(2, exp.CollectedLoot.Count,
                "Fatalist survivor should keep their loot (they walked through it).");
        }

        // -------------------------------------------------------------------
        // Comms-severed UI semantics
        // -------------------------------------------------------------------

        [Test]
        public void IsCommsSevered_DefaultsToFalse_BeforeIntercept()
        {
            var survivor = MakeSurvivor("sv_neutral", RiskBiasTrait.Realist);
            _expeditionSystem.StartExpedition(survivor, _location, ExpeditionStance.Stealth);
            var exp = _expeditionSystem.GetExpeditionBySurvivor(survivor.Id);

            Assert.IsFalse(exp.isCommsSevered, "Comms should be intact before the flashpoint.");
        }

        [Test]
        public void Idempotency_DoubleIntercept_DoesNotApplyTwice()
        {
            var survivor = MakeSurvivor("sv_dup", RiskBiasTrait.Paranoid);
            _expeditionSystem.StartExpedition(survivor, _location, ExpeditionStance.Stealth);
            var exp = _expeditionSystem.GetExpeditionBySurvivor(survivor.Id);
            exp.TryAddLoot(_foodItem);
            exp.TravelTicksCompleted = 5;

            float preAcute = survivor.AcuteDoseWindow;

            PublishIntercept();
            float postFirstAcute = survivor.AcuteDoseWindow;
            Assert.AreEqual(preAcute + ExpeditionSystem.FlashpointAcuteDoseSpike, postFirstAcute, Eps);

            // Re-publish: should be a no-op (comms already severed).
            PublishIntercept();
            Assert.AreEqual(postFirstAcute, survivor.AcuteDoseWindow, Eps,
                "Re-publishing the intercept must not double-apply the dose spike.");
        }

        // -------------------------------------------------------------------
        // Hatch dilemma
        // -------------------------------------------------------------------

        [Test]
        public void CommsSeveredExpedition_OnReturn_RaisesHatchDilemmaSignal_InsteadOfCompleting()
        {
            var survivor = MakeSurvivor("sv_hatch", RiskBiasTrait.Realist);
            _expeditionSystem.StartExpedition(survivor, _location, ExpeditionStance.Speed);
            var exp = _expeditionSystem.GetExpeditionBySurvivor(survivor.Id);
            // Sever comms and put the expedition 1 tick from arrival.
            PublishIntercept();
            exp.TravelTicksCompleted = 1;

            HatchDilemmaReadySignal captured = default;
            bool fired = false;
            EventBus.Subscribe<HatchDilemmaReadySignal>(s => { captured = s; fired = true; });

            // Tick to arrival.
            _expeditionSystem.Tick(1f);

            Assert.IsTrue(fired, "HatchDilemmaReadySignal should fire on arrival.");
            Assert.AreEqual(exp.ExpeditionId, captured.Expedition.ExpeditionId);
            Assert.IsTrue(captured.SurvivorIsAlive);
            Assert.AreEqual(ExpeditionPhase.AtHatchDilemma, captured.Expedition.Phase,
                "The expedition should be in the AtHatchDilemma phase, not Completed.");
            Assert.IsTrue(_expeditionSystem.IsOnExpedition(survivor.Id),
                "The expedition should remain in the active list until the player resolves the dilemma.");
        }

        [Test]
        public void LetThemIn_Choice_CompletesExpeditionAndKeepsSurvivorAlive()
        {
            var survivor = MakeSurvivor("sv_letin", RiskBiasTrait.Realist);
            _expeditionSystem.StartExpedition(survivor, _location, ExpeditionStance.Speed);
            var exp = _expeditionSystem.GetExpeditionBySurvivor(survivor.Id);
            PublishIntercept();
            exp.TravelTicksCompleted = 1;
            _expeditionSystem.Tick(1f); // arrival → AtHatchDilemma

            _expeditionSystem.ApplyHatchDilemmaChoice(
                exp.ExpeditionId,
                HatchDilemmaResolvedSignal.Resolution.LetThemIn);

            Assert.IsFalse(_expeditionSystem.IsOnExpedition(survivor.Id),
                "Let-them-in should remove the expedition from the active list.");
            Assert.IsTrue(survivor.IsAlive,
                "Let-them-in must not kill the survivor.");
            Assert.AreEqual(SurvivorState.Idle, survivor.State,
                "Let-them-in should return the survivor to the Idle state in the bunker.");
        }

        [Test]
        public void ForceDecon_Choice_DamagesSurvivorButKeepsThemAlive()
        {
            var survivor = MakeSurvivor("sv_decon", RiskBiasTrait.Realist);
            float preHealth = survivor.Needs.Health;
            float preMorale = survivor.Needs.Morale;
            _expeditionSystem.StartExpedition(survivor, _location, ExpeditionStance.Speed);
            var exp = _expeditionSystem.GetExpeditionBySurvivor(survivor.Id);
            PublishIntercept();
            exp.TravelTicksCompleted = 1;
            _expeditionSystem.Tick(1f);

            _expeditionSystem.ApplyHatchDilemmaChoice(
                exp.ExpeditionId,
                HatchDilemmaResolvedSignal.Resolution.ForceDeconOutside);

            Assert.IsTrue(survivor.IsAlive);
            Assert.Less(survivor.Needs.Morale, preMorale,
                "Force-decon should drop the survivor's morale.");
            // The exposed rad path may or may not have dropped health (depending
            // on the shelter shielding + exposed rad), but the choice must
            // not have killed the survivor.
        }

        [Test]
        public void DenyEntry_Choice_KillsSurvivor()
        {
            var survivor = MakeSurvivor("sv_deny", RiskBiasTrait.Realist);
            _expeditionSystem.StartExpedition(survivor, _location, ExpeditionStance.Speed);
            var exp = _expeditionSystem.GetExpeditionBySurvivor(survivor.Id);
            PublishIntercept();
            exp.TravelTicksCompleted = 1;
            _expeditionSystem.Tick(1f);

            _expeditionSystem.ApplyHatchDilemmaChoice(
                exp.ExpeditionId,
                HatchDilemmaResolvedSignal.Resolution.DenyEntry);

            Assert.IsFalse(survivor.IsAlive,
                "Deny-entry should kill the survivor outside the hatch.");
            Assert.AreEqual(SurvivorState.Dead, survivor.State);
            Assert.IsFalse(_expeditionSystem.IsOnExpedition(survivor.Id));
        }

        [Test]
        public void NonCommsSeveredExpedition_StillCompletesNormally_WithoutHatchDilemma()
        {
            // Baseline regression: without a flashpoint intercept, an
            // expedition that reaches the shelter completes as before.
            var survivor = MakeSurvivor("sv_normal", RiskBiasTrait.Realist);
            _expeditionSystem.StartExpedition(survivor, _location, ExpeditionStance.Speed);
            var exp = _expeditionSystem.GetExpeditionBySurvivor(survivor.Id);
            // Force the expedition into the Inbound phase, 1 tick from arrival.
            exp.Phase = ExpeditionPhase.Inbound;
            exp.TravelTicksCompleted = 1;

            bool fired = false;
            EventBus.Subscribe<HatchDilemmaReadySignal>(_ => fired = true);

            _expeditionSystem.Tick(1f);

            Assert.IsFalse(fired, "Non-severed expeditions must NOT raise the hatch dilemma signal.");
            Assert.IsFalse(_expeditionSystem.IsOnExpedition(survivor.Id),
                "Non-severed expeditions should complete and be removed.");
        }
    }
}
