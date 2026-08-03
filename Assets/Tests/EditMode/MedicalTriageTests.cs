using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Medical triage pipeline: afflictions, progression, treatments, immune collapse.
    /// </summary>
    [TestFixture]
    public class MedicalTriageTests
    {
        private const float Eps = 1e-3f;

        private NeedsProfile _profile;
        private NeedsSystem _needs;
        private List<AfflictionSO> _defs;

        [SetUp]
        public void SetUp()
        {
            _profile = ScriptableObject.CreateInstance<NeedsProfile>();
            _profile.hungerPerHour = 0f;
            _profile.thirstPerHour = 0f;
            _profile.fatiguePerHour = 0f;
            _profile.warmthLossPerHourInCold = 0f;
            _profile.hungerCritical = 100f;
            _profile.thirstCritical = 100f;
            _profile.warmthCritical = 0f;
            _profile.moraleLossPerHourWhileCritical = 0f;
            _needs = new NeedsSystem(_profile);
            _defs = MedicalSystem.CreateDefaultAfflictions();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_profile);
            if (_defs != null)
            {
                for (int i = 0; i < _defs.Count; i++)
                    Object.DestroyImmediate(_defs[i]);
            }
        }

        private MedicalSystem MakeMedical(Inventory inv = null, Shelter shelter = null)
        {
            var med = new MedicalSystem(_needs, inv, shelter);
            for (int i = 0; i < _defs.Count; i++)
                med.RegisterAffliction(_defs[i]);
            return med;
        }

        private static Survivor MakeSurvivor(string id = "sv_patient")
        {
            return new Survivor { Id = id, DisplayName = id, MedicalSkill = 0.3f };
        }

        private static ItemDefinition MakeItem(string id, ItemType type = ItemType.Medical)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.type = type;
            item.stackMax = 10;
            item.weight = 0.1f;
            return item;
        }

        [Test]
        public void GunshotWound_WithoutBandage_BecomesSepsisAfter48Hours()
        {
            var med = MakeMedical();
            var patient = MakeSurvivor();
            Assert.IsTrue(med.Inflict(patient, AfflictionSO.Ids.GunshotWound));
            Assert.IsTrue(med.HasAffliction(patient, AfflictionSO.Ids.GunshotWound));
            Assert.IsFalse(med.HasAffliction(patient, AfflictionSO.Ids.Sepsis));

            // Just under 48h: still gunshot
            med.Tick(new List<Survivor> { patient }, 47.9f);
            Assert.IsTrue(med.HasAffliction(patient, AfflictionSO.Ids.GunshotWound));
            Assert.IsFalse(med.HasAffliction(patient, AfflictionSO.Ids.Sepsis));

            // Cross 48h
            med.Tick(new List<Survivor> { patient }, 0.2f);
            Assert.IsFalse(med.HasAffliction(patient, AfflictionSO.Ids.GunshotWound),
                "Gunshot should progress away after 48h untreated");
            Assert.IsTrue(med.HasAffliction(patient, AfflictionSO.Ids.Sepsis),
                "Untreated gunshot must become sepsis after 48 hours");
        }

        [Test]
        public void Bandage_HaltsGunshotProgression()
        {
            var bandage = MakeItem("bandage");
            var inv = new Inventory { Capacity = 10, MaxWeight = 50f };
            inv.Add(bandage, 1);

            var med = MakeMedical(inv);
            var patient = MakeSurvivor();
            Assert.IsTrue(med.Inflict(patient, AfflictionSO.Ids.GunshotWound));

            Assert.IsTrue(med.TryEmergencyHalt(patient, "bandage", bandage));
            Assert.That(inv.Count(bandage), Is.EqualTo(0));

            var active = med.GetActive(patient);
            Assert.That(active.Count, Is.EqualTo(1));
            Assert.IsTrue(active[0].ProgressionHalted);

            // Far past 48h — must NOT become sepsis
            med.Tick(new List<Survivor> { patient }, 100f);
            Assert.IsTrue(med.HasAffliction(patient, AfflictionSO.Ids.GunshotWound));
            Assert.IsFalse(med.HasAffliction(patient, AfflictionSO.Ids.Sepsis));

            Object.DestroyImmediate(bandage);
        }

        [Test]
        public void Affliction_DrainsHealth_NoAffliction_NoMedicalDrain()
        {
            var med = MakeMedical();
            var healthy = MakeSurvivor("sv_ok");
            var wounded = MakeSurvivor("sv_hurt");
            healthy.Needs.Health = 100f;
            wounded.Needs.Health = 100f;

            med.Inflict(wounded, AfflictionSO.Ids.GunshotWound);

            float h0 = healthy.Needs.Health;
            float w0 = wounded.Needs.Health;
            med.Tick(new List<Survivor> { healthy, wounded }, 2f);

            Assert.That(healthy.Needs.Health, Is.EqualTo(h0).Within(Eps),
                "Health without afflictions must not be drained by MedicalSystem");
            Assert.That(wounded.Needs.Health, Is.LessThan(w0 - Eps),
                "Active affliction must drain Health");
        }

        [Test]
        public void Infection_WithLatentDamage_IsThreeTimesMoreLethal()
        {
            var med = MakeMedical();
            var normal = MakeSurvivor("sv_a");
            var compromised = MakeSurvivor("sv_b");
            compromised.LatentDamage = MedicalSystem.ImmuneCollapseLatentThreshold + 1f;

            var def = med.GetAfflictionDef(AfflictionSO.Ids.BacterialInfection);
            Assert.IsNotNull(def);
            Assert.IsTrue(def.isInfection);

            float baseL = med.EffectiveLethality(normal, def);
            float hotL = med.EffectiveLethality(compromised, def);
            Assert.That(hotL, Is.EqualTo(baseL * MedicalSystem.ImmuneCollapseLethalityFactor).Within(Eps));

            // Drain rate reflects the multiplier
            normal.Needs.Health = 100f;
            compromised.Needs.Health = 100f;
            med.Inflict(normal, AfflictionSO.Ids.BacterialInfection);
            med.Inflict(compromised, AfflictionSO.Ids.BacterialInfection);
            med.Tick(new List<Survivor> { normal, compromised }, 1f);

            float normalLoss = 100f - normal.Needs.Health;
            float hotLoss = 100f - compromised.Needs.Health;
            Assert.That(hotLoss, Is.EqualTo(normalLoss * MedicalSystem.ImmuneCollapseLethalityFactor).Within(0.05f));
        }

        [Test]
        public void FullGunshotTreatment_RequiresMedicalBed_AndCures()
        {
            var bandage = MakeItem("bandage");
            var tweezers = MakeItem("tweezers", ItemType.Tool);
            var inv = new Inventory { Capacity = 20, MaxWeight = 100f };
            inv.Add(bandage, 2);
            inv.Add(tweezers, 1);

            var bedSO = ScriptableObject.CreateInstance<MedicalBedModuleSO>();
            bedSO.ModuleId = MedicalSystem.MedicalBedModuleId;
            bedSO.DisplayName = "Medical Bed";
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance(bedSO, level: 1));

            var med = MakeMedical(inv, shelter);
            var recipe = MedicalSystem.CreateGunshotFullRecipe(bandage, tweezers);
            med.RegisterTreatment(recipe);

            var medic = MakeSurvivor("sv_medic");
            medic.MedicalSkill = 0.9f;
            var patient = MakeSurvivor("sv_patient");
            med.Inflict(patient, AfflictionSO.Ids.GunshotWound);

            Assert.IsTrue(med.TryStartTreatment(medic, patient, recipe));
            Assert.IsTrue(med.GetActive(patient)[0].IsTreating);
            Assert.IsTrue(med.GetActive(patient)[0].ProgressionHalted);

            float hours = MedicalSystem.ComputeTreatmentHours(recipe, medic.MedicalSkill);
            med.Tick(new List<Survivor> { patient }, hours + 0.1f);

            Assert.IsFalse(med.HasAffliction(patient, AfflictionSO.Ids.GunshotWound),
                "Full treatment must remove gunshot wound");

            Object.DestroyImmediate(bandage);
            Object.DestroyImmediate(tweezers);
            Object.DestroyImmediate(bedSO);
            Object.DestroyImmediate(recipe);
        }

        [Test]
        public void HighMedicalSkill_TreatsFasterThanUnskilled()
        {
            var recipe = ScriptableObject.CreateInstance<TreatmentRecipeSO>();
            recipe.id = "t";
            recipe.baseTreatmentHours = 4f;
            float slow = MedicalSystem.ComputeTreatmentHours(recipe, medicalSkill: 0f);
            float fast = MedicalSystem.ComputeTreatmentHours(recipe, medicalSkill: 1f);
            Assert.That(fast, Is.LessThan(slow));
            Assert.That(fast, Is.EqualTo(2f).Within(Eps)); // 0.5 * 4
            Object.DestroyImmediate(recipe);
        }

        [Test]
        public void TreatPatientAction_ScoresWhenSomeoneIsWounded()
        {
            var med = MakeMedical();
            var medic = MakeSurvivor("sv_medic");
            medic.MedicalSkill = 0.8f;
            var patient = MakeSurvivor("sv_patient");
            med.Inflict(patient, AfflictionSO.Ids.GunshotWound);

            var action = ScriptableObject.CreateInstance<TreatPatientActionSO>();
            var ctxEmpty = new AIContext(medic)
            {
                MedicalSystem = med,
                GetSurvivors = () => new List<Survivor> { medic }
            };
            // Only medic, no affliction on self
            float scoreAlone = action.EvaluateRaw(ctxEmpty);

            var ctx = new AIContext(medic)
            {
                MedicalSystem = med,
                GetSurvivors = () => new List<Survivor> { medic, patient }
            };
            float scoreWithPatient = action.EvaluateRaw(ctx);
            Assert.That(scoreWithPatient, Is.GreaterThan(scoreAlone));
            Assert.That(scoreWithPatient, Is.GreaterThan(0f));

            Object.DestroyImmediate(action);
        }

        [Test]
        public void RadBurns_ProgressToSepsis_ThenThreatenDeath()
        {
            var med = MakeMedical();
            var patient = MakeSurvivor();
            // Enough health to survive pre-progression rad-burn drain (2.5/hr × 36h)
            // so the cascade RadBurns → Sepsis can be observed.
            patient.Needs.Health = 100f;
            med.Inflict(patient, AfflictionSO.Ids.RadBurns);

            med.Tick(new List<Survivor> { patient }, 36.1f);
            Assert.IsFalse(med.HasAffliction(patient, AfflictionSO.Ids.RadBurns),
                "Rad burns should progress away after 36h untreated");
            Assert.IsTrue(med.HasAffliction(patient, AfflictionSO.Ids.Sepsis),
                "Untreated rad burns must become sepsis");

            // Sepsis drains hard
            float before = patient.Needs.Health;
            med.Tick(new List<Survivor> { patient }, 2f);
            Assert.That(patient.Needs.Health, Is.LessThan(before));
        }

        [Test]
        public void MedicalState_RoundTripsThroughSave()
        {
            var med = MakeMedical();
            var patient = MakeSurvivor();
            med.Inflict(patient, AfflictionSO.Ids.BrokenBone);
            med.GetActive(patient)[0].HoursActive = 12f;

            var save = med.CaptureState();
            var med2 = MakeMedical();
            med2.RestoreState(save);

            Assert.IsTrue(med2.HasAffliction(patient, AfflictionSO.Ids.BrokenBone));
            Assert.That(med2.GetActive(patient)[0].HoursActive, Is.EqualTo(12f).Within(Eps));
        }
    }
}
