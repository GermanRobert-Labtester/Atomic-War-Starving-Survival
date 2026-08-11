using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
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
        public void TreatmentWithMorphine_RecordsBloodToxicityChemUse()
        {
            // Mirrors GameBootstrap.WireMedicalAddictionHooks: treatment ingredients → BloodToxicity.
            var bandage = MakeItem("bandage");
            var morphine = MakeItem("morphine");
            var inv = new Inventory { Capacity = 20, MaxWeight = 100f };
            inv.Add(bandage, 1);
            inv.Add(morphine, 2);

            var med = MakeMedical(inv);
            var blood = new BloodToxicitySystem();
            med.GetCurrentDay = () => 3;
            med.OnTreatmentItemConsumed = (sv, itemId, day) =>
            {
                if (sv != null)
                    blood.RecordChemUse(sv.Id, itemId);
            };

            // Pain recipe: primary bandage + secondary morphine (itemId-only on secondary).
            var recipe = ScriptableObject.CreateInstance<TreatmentRecipeSO>();
            recipe.id = "treat_gunshot_pain";
            recipe.displayName = "Morphine dressing";
            recipe.targetAfflictionId = AfflictionSO.Ids.GunshotWound;
            recipe.baseTreatmentHours = 1f;
            recipe.requiresMedicalBed = false;
            recipe.requiresPatientRest = false;
            recipe.ingredients = new List<TreatmentIngredient>
            {
                new TreatmentIngredient { item = bandage, itemId = "bandage", amount = 1 },
                new TreatmentIngredient { item = null, itemId = "morphine", amount = 1 }
            };
            med.RegisterTreatment(recipe);

            var medic = MakeSurvivor("sv_medic");
            medic.MedicalSkill = 0.2f; // low skill: secondary morphine not spared
            var patient = MakeSurvivor("sv_chem_patient");
            Assert.IsTrue(med.Inflict(patient, AfflictionSO.Ids.GunshotWound));
            Assert.IsTrue(med.TryStartTreatment(medic, patient, recipe));

            Assert.IsTrue(blood.States.ContainsKey(patient.Id),
                "Morphine in a treatment recipe must record blood toxicity chem use.");
            Assert.AreEqual(1, blood.States[patient.Id].chemUsageCount);
            Assert.AreEqual(1, inv.CountById("morphine"), "One morphine unit should be consumed.");
            Assert.AreEqual(0, inv.CountById("bandage"));

            Object.DestroyImmediate(bandage);
            Object.DestroyImmediate(morphine);
            Object.DestroyImmediate(recipe);
        }

        [Test]
        public void TreatmentWithItemIdOnlyAntiRad_RecordsBloodToxicity()
        {
            var antiRad = MakeItem("anti_rad");
            var inv = new Inventory { Capacity = 10, MaxWeight = 50f };
            inv.Add(antiRad, 1);

            var med = MakeMedical(inv);
            var blood = new BloodToxicitySystem();
            med.OnTreatmentItemConsumed = (sv, itemId, day) =>
            {
                if (sv != null) blood.RecordChemUse(sv.Id, itemId);
            };

            // Affliction that can be "treated" with anti_rad only (itemId, no item ref).
            var recipe = ScriptableObject.CreateInstance<TreatmentRecipeSO>();
            recipe.id = "treat_rad_anti";
            recipe.targetAfflictionId = AfflictionSO.Ids.GunshotWound;
            recipe.baseTreatmentHours = 0.5f;
            recipe.requiresMedicalBed = false;
            recipe.requiresPatientRest = false;
            recipe.ingredients = new List<TreatmentIngredient>
            {
                new TreatmentIngredient { item = null, itemId = "anti_rad", amount = 1 }
            };
            med.RegisterTreatment(recipe);

            var medic = MakeSurvivor("sv_m");
            var patient = MakeSurvivor("sv_p");
            med.Inflict(patient, AfflictionSO.Ids.GunshotWound);
            Assert.IsTrue(med.TryStartTreatment(medic, patient, recipe));
            Assert.AreEqual(1, blood.States[patient.Id].chemUsageCount);
            Assert.AreEqual(0, inv.CountById("anti_rad"));

            Object.DestroyImmediate(antiRad);
            Object.DestroyImmediate(recipe);
        }

        [Test]
        public void AmputationMorphine_RecordsBloodToxicityChemUse()
        {
            var amp = new AmputationSystem();
            var patient = MakeSurvivor("sv_amp_patient");
            var surgeon = MakeSurvivor("sv_surgeon");
            surgeon.MedicalSkill = 0.9f;
            patient.Needs.Health = 80f;
            surgeon.Needs.Fatigue = 10f;
            surgeon.Needs.Morale = 70f;
            patient.Needs.Morale = 70f;

            amp.Bind(
                id => id == patient.Id ? patient : id == surgeon.Id ? surgeon : null,
                (sv, affId) => { });
            amp.BindMedicalPerks(null, getDay: () => 4);

            var blood = new BloodToxicitySystem();
            amp.OnChemConsumed = (sv, itemId, day) =>
            {
                if (sv != null) blood.RecordChemUse(sv.Id, itemId);
            };

            int tools = 1, morph = 1;
            Assert.IsTrue(amp.PerformAmputation(
                patient.Id,
                surgeon.Id,
                id => id == AmputationSystem.SurgicalToolsItemId ? tools
                    : id == AmputationSystem.MorphineItemId ? morph : 0,
                (id, n) =>
                {
                    if (id == AmputationSystem.SurgicalToolsItemId) { tools -= n; return true; }
                    if (id == AmputationSystem.MorphineItemId) { morph -= n; return true; }
                    return false;
                }));

            Assert.AreEqual(0, morph);
            Assert.AreEqual(1, blood.States[patient.Id].chemUsageCount,
                "Amputation morphine must count as a blood-toxicity chem use on the patient.");
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
        public void AdministerAdrenalineAction_RevivesDeathsDoorPatient_AndConsumesEpiPen()
        {
            // H-6d: MedicalPerkSystem.TryAdministerAdrenaline existed but no AI action
            // could ever call it — a Death's-Door survivor could only ever die.
            var progression = new SkillProgressionSystem();
            progression.RegisterDefaultPerks();
            var perks = new MedicalPerkSystem();
            var medic = MakeSurvivor("sv_medic");
            var patient = MakeSurvivor("sv_patient");
            var survivors = new List<Survivor> { medic, patient };
            perks.Bind(progression, () => survivors);
            perks.RecordComaRevive(medic, 1); // grants Paramedic
            Assert.IsTrue(perks.HasParamedic(medic));
            Assert.IsTrue(perks.TryEnterDeathsDoor(patient));

            var inv = new Inventory { Capacity = 10 };
            var epiPen = MakeItem(AdministerAdrenalineActionSO.AdrenalineItemId);
            inv.Add(epiPen, 1);

            var action = ScriptableObject.CreateInstance<AdministerAdrenalineActionSO>();
            var ctxNoPatient = new AIContext(medic)
            {
                MedicalPerks = perks,
                Inventory = inv,
                GetSurvivors = () => new List<Survivor> { medic }
            };
            Assert.AreEqual(0f, action.EvaluateRaw(ctxNoPatient));

            var ctx = new AIContext(medic)
            {
                MedicalPerks = perks,
                Inventory = inv,
                GetSurvivors = () => survivors
            };
            Assert.Greater(action.EvaluateRaw(ctx), 0.9f);

            action.Execute(ctx);

            Assert.IsFalse(perks.IsOnDeathsDoor(patient));
            Assert.AreEqual(MedicalPerkSystem.AdrenalineReviveHealth, patient.Needs.Health, Eps);
            Assert.AreEqual(0, inv.CountById(AdministerAdrenalineActionSO.AdrenalineItemId));

            Object.DestroyImmediate(action);
            Object.DestroyImmediate(epiPen);
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
        public void StaminaCap_HoldsFatigueUp_AndNeverRestsBelowTheFloor()
        {
            var med = MakeMedical();
            var patient = MakeSurvivor();

            // Fully rested when the needle goes in.
            patient.Needs.Fatigue = 0f;
            med.Inflict(patient, AfflictionSO.Ids.BloodLoss);

            med.Tick(new List<Survivor> { patient }, 1f);

            // blood_loss carries staminaCap 30 — stamina is capped at 30%, i.e. the
            // survivor sits at 70 fatigue or worse. They are functional, never rested.
            Assert.That(patient.Needs.Fatigue, Is.EqualTo(70f).Within(Eps),
                "A stamina cap of 30% must hold Fatigue at or above 70");

            // Sleeping it off does not beat the cap.
            patient.Needs.Fatigue = 5f;
            med.Tick(new List<Survivor> { patient }, 1f);
            Assert.That(patient.Needs.Fatigue, Is.EqualTo(70f).Within(Eps),
                "Rest cannot push Fatigue below the floor while the affliction is active");
        }

        [Test]
        public void StaminaCap_IsHarsherForTheWorseAffliction()
        {
            var med = MakeMedical();
            var donor = MakeSurvivor("sv_donor");
            var poisoned = MakeSurvivor("sv_poisoned");

            med.Inflict(donor, AfflictionSO.Ids.BloodLoss);   // staminaCap 30
            med.Inflict(poisoned, AfflictionSO.Ids.Botulism); // staminaCap 20

            med.Tick(new List<Survivor> { donor, poisoned }, 1f);

            Assert.That(poisoned.Needs.Fatigue, Is.GreaterThan(donor.Needs.Fatigue),
                "Botulism caps stamina lower than blood loss, so it must exhaust harder");
        }

        [Test]
        public void Amputation_TakesTheSepsisOffWithTheLimb()
        {
            var med = MakeMedical();
            var patient = MakeSurvivor("sv_patient");
            var surgeon = MakeSurvivor("sv_surgeon");
            surgeon.MedicalSkill = 1f;

            Assert.IsTrue(med.Inflict(patient, AfflictionSO.Ids.Sepsis));

            var amp = new AmputationSystem();
            amp.Bind(
                findSurvivor: id => id == patient.Id ? patient : id == surgeon.Id ? surgeon : null,
                inflictAffliction: (sv, affId) => med.Inflict(sv, affId),
                cureAffliction: (sv, affId, medic) => med.CureOutright(sv, affId, medic));

            int tools = 1, morphine = 1;
            Assert.IsTrue(amp.PerformAmputation(
                patient.Id, surgeon.Id,
                countItem: id =>
                {
                    if (id == AmputationSystem.SurgicalToolsItemId) return tools;
                    if (id == AmputationSystem.MorphineItemId) return morphine;
                    return 0;
                },
                consumeItem: (id, n) =>
                {
                    if (id == AmputationSystem.SurgicalToolsItemId) { tools -= n; return true; }
                    if (id == AmputationSystem.MorphineItemId) { morphine -= n; return true; }
                    return false;
                }));

            Assert.IsTrue(amp.IsAmputee(patient.Id), "Patient must carry the amputee disability");
            Assert.IsFalse(med.HasAffliction(patient, AfflictionSO.Ids.Sepsis),
                "Amputation removes the septic limb — the sepsis must go with it, or the "
                + "patient paid a limb and 30 health for nothing.");
        }

        [Test]
        public void CureOutright_LeavesOtherAfflictionsAndUnknownIdsAlone()
        {
            var med = MakeMedical();
            var patient = MakeSurvivor();
            med.Inflict(patient, AfflictionSO.Ids.Sepsis);
            med.Inflict(patient, AfflictionSO.Ids.BrokenBone);

            Assert.IsFalse(med.CureOutright(patient, AfflictionSO.Ids.Coma),
                "Curing an affliction the survivor does not have reports failure");

            Assert.IsTrue(med.CureOutright(patient, AfflictionSO.Ids.Sepsis));
            Assert.IsFalse(med.HasAffliction(patient, AfflictionSO.Ids.Sepsis));
            Assert.IsTrue(med.HasAffliction(patient, AfflictionSO.Ids.BrokenBone),
                "Unrelated afflictions survive the procedure");
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

        [Test]
        public void HighSkillTreatment_LowRoll_SparesSecondaryIngredient()
        {
            var bandage = MakeItem("bandage");
            var tweezers = MakeItem("tweezers", ItemType.Tool);
            var inv = new Inventory { Capacity = 20, MaxWeight = 100f };
            inv.Add(bandage, 1);
            inv.Add(tweezers, 1);

            var bedSO = ScriptableObject.CreateInstance<MedicalBedModuleSO>();
            bedSO.ModuleId = MedicalSystem.MedicalBedModuleId;
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance(bedSO, level: 1));

            var med = MakeMedical(inv, shelter);
            // Roll 0.1 < skill(0.9) * 0.4 (0.36): secondary ingredient (tweezers) is spared.
            med.BindMedicalPerks(null, null, new FixedRandom(0.1));
            var recipe = MedicalSystem.CreateGunshotFullRecipe(bandage, tweezers);
            med.RegisterTreatment(recipe);

            var medic = MakeSurvivor("sv_medic");
            medic.MedicalSkill = 0.9f;
            var patient = MakeSurvivor("sv_patient");
            med.Inflict(patient, AfflictionSO.Ids.GunshotWound);

            Assert.IsTrue(med.TryStartTreatment(medic, patient, recipe));
            Assert.AreEqual(1, inv.Count(tweezers), "A low sparing roll must leave the secondary ingredient uneaten.");
            Assert.AreEqual(0, inv.Count(bandage), "The primary ingredient is always consumed regardless of the roll.");

            Object.DestroyImmediate(bandage);
            Object.DestroyImmediate(tweezers);
            Object.DestroyImmediate(bedSO);
            Object.DestroyImmediate(recipe);
        }

        [Test]
        public void HighSkillTreatment_HighRoll_ConsumesSecondaryIngredient()
        {
            var bandage = MakeItem("bandage");
            var tweezers = MakeItem("tweezers", ItemType.Tool);
            var inv = new Inventory { Capacity = 20, MaxWeight = 100f };
            inv.Add(bandage, 1);
            inv.Add(tweezers, 1);

            var bedSO = ScriptableObject.CreateInstance<MedicalBedModuleSO>();
            bedSO.ModuleId = MedicalSystem.MedicalBedModuleId;
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance(bedSO, level: 1));

            var med = MakeMedical(inv, shelter);
            // Roll 0.9 >= skill(0.9) * 0.4 (0.36): secondary ingredient (tweezers) is consumed as normal.
            med.BindMedicalPerks(null, null, new FixedRandom(0.9));
            var recipe = MedicalSystem.CreateGunshotFullRecipe(bandage, tweezers);
            med.RegisterTreatment(recipe);

            var medic = MakeSurvivor("sv_medic");
            medic.MedicalSkill = 0.9f;
            var patient = MakeSurvivor("sv_patient");
            med.Inflict(patient, AfflictionSO.Ids.GunshotWound);

            Assert.IsTrue(med.TryStartTreatment(medic, patient, recipe));
            Assert.AreEqual(0, inv.Count(tweezers), "A high sparing roll must consume the secondary ingredient normally.");

            Object.DestroyImmediate(bandage);
            Object.DestroyImmediate(tweezers);
            Object.DestroyImmediate(bedSO);
            Object.DestroyImmediate(recipe);
        }

        private sealed class FixedRandom : System.Random
        {
            private readonly double _value;
            public FixedRandom(double value) { _value = value; }
            public override double NextDouble() => _value;
            public override int Next() => 0;
            public override int Next(int maxValue) => 0;
            public override int Next(int minValue, int maxValue) => minValue;
        }
    }
}
