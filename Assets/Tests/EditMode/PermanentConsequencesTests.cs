using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class PermanentConsequencesTests
    {
        private NeedsSystem _needsSystem;
        private MedicalSystem _medicalSystem;
        private NeedsProfile _profile;

        [SetUp]
        public void SetUp()
        {
            _profile = ScriptableObject.CreateInstance<NeedsProfile>();
            _needsSystem = new NeedsSystem(_profile);
            _medicalSystem = new MedicalSystem(_needsSystem);

            var defaultAfflictions = MedicalSystem.CreateDefaultAfflictions();
            for (int i = 0; i < defaultAfflictions.Count; i++)
            {
                _medicalSystem.RegisterAffliction(defaultAfflictions[i]);
            }
        }

        [Test]
        public void CuringSepsis_After75Hours_GrantsScarredLungs_AndCapsHealthAt75()
        {
            var survivor = new Survivor
            {
                Id = "survivor_sepsis_test",
                DisplayName = "Maria",
                State = SurvivorState.Idle
            };
            survivor.Needs.Health = 50f;

            // Inflict Sepsis
            bool inflicted = _medicalSystem.Inflict(survivor, AfflictionSO.Ids.Sepsis);
            Assert.IsTrue(inflicted, "Sepsis should be inflicted.");

            // Set active hours to 75 so survivor has spent >72h in critical Sepsis state
            var activeList = _medicalSystem.GetActive(survivor);
            Assert.AreEqual(1, activeList.Count);
            activeList[0].HoursActive = 75f;
            activeList[0].ProgressionHalted = true;

            // Register a treatment recipe for Sepsis
            var recipe = ScriptableObject.CreateInstance<TreatmentRecipeSO>();
            recipe.id = "cure_sepsis_test";
            recipe.targetAfflictionId = AfflictionSO.Ids.Sepsis;
            recipe.baseTreatmentHours = 1f;
            recipe.healthRestoreOnCure = 50f;
            _medicalSystem.RegisterTreatment(recipe);

            var list = new List<Survivor> { survivor };
            survivor.Needs.Health = 100f;

            // Start and complete treatment
            bool started = _medicalSystem.TryStartTreatment(survivor, survivor, recipe);
            Assert.IsTrue(started, "Treatment should start.");

            // Tick treatment completion (recipe is 1 hour, tick 1.5 hours)
            _medicalSystem.Tick(list, 1.5f);

            // Assert Sepsis is cured
            Assert.IsFalse(_medicalSystem.HasAffliction(survivor, AfflictionSO.Ids.Sepsis), "Sepsis should be cured.");

            // Assert survivor received ScarredLungs disability
            Assert.IsTrue(survivor.HasDisability(DisabilitySO.Ids.ScarredLungs), "Survivor should receive ScarredLungs disability after surviving Sepsis >72 hours.");
            Assert.AreEqual(75f, survivor.MaxHealthCap, "Survivor's max health cap should be 75.");

            // Try modifying health above 75 (e.g. restore to 100)
            _needsSystem.Modify(survivor, NeedKind.Health, 100f);
            Assert.AreEqual(75f, survivor.Needs.Health, "Health should be permanently capped at 75.");
        }

        [Test]
        public void LimpDisability_DoublesExpeditionStaminaDrain()
        {
            var survivor = new Survivor
            {
                Id = "survivor_limp_test",
                DisplayName = "Alex",
                State = SurvivorState.Idle
            };
            survivor.DisabilityIds.Add(DisabilitySO.Ids.Limp);

            var expSystem = new ExpeditionSystem(null, null, null);
            var loc = ScriptableObject.CreateInstance<LocationDefinitionSO>();
            loc.id = "scavenge_site_1";
            loc.travelHours = 10f;

            bool started = expSystem.StartExpedition(survivor, loc);
            Assert.IsTrue(started, "Expedition should start.");

            var expState = expSystem.GetExpeditionBySurvivor(survivor.Id);
            Assert.IsNotNull(expState, "Expedition state should exist.");

            // Tick 1 hour of expedition
            expSystem.Tick(1f);

            // Base stamina drain = 5f/hr. With Limp, stamina drain should be 10f/hr (stamina drops to 90)
            Assert.AreEqual(90f, expState.Stamina, "Limp should double stamina drain during expedition.");
        }

        [Test]
        public void TremorsDisability_ReducesActionSpeedBy50Percent()
        {
            var medic = new Survivor
            {
                Id = "medic_tremors_test",
                DisplayName = "Doc",
                MedicalSkill = 0.5f
            };
            medic.DisabilityIds.Add(DisabilitySO.Ids.Tremors);

            var recipe = ScriptableObject.CreateInstance<TreatmentRecipeSO>();
            recipe.id = "test_recipe";
            recipe.baseTreatmentHours = 4f;

            float normalHours = MedicalSystem.ComputeTreatmentHours(recipe, 0.5f, null);
            float tremorsHours = MedicalSystem.ComputeTreatmentHours(recipe, 0.5f, medic);

            Assert.AreEqual(normalHours * 2.0f, tremorsHours, "Tremors disability should double treatment duration (reducing action speed by 50%).");
        }
    }
}
