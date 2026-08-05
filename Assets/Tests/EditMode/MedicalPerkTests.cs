using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using InventoryClass = AtomicWar._Game.Inventory.Inventory;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompts #201–#205 — medical milestone perks.
    /// </summary>
    [TestFixture]
    public class MedicalPerkTests
    {
        private SkillProgressionSystem _progression;
        private MedicalPerkSystem _perks;
        private NeedsSystem _needs;
        private MedicalSystem _medical;
        private InventoryClass _inv;
        private Survivor _medic;
        private Survivor _patient;
        private List<Survivor> _survivors;

        [SetUp]
        public void SetUp()
        {
            _progression = new SkillProgressionSystem();
            _progression.RegisterDefaultPerks();
            _perks = new MedicalPerkSystem();
            _survivors = new List<Survivor>();
            _perks.Bind(_progression, () => _survivors);

            _needs = new NeedsSystem(ScriptableObject.CreateInstance<NeedsProfile>());
            _needs.TryDeferDeath = sv => _perks.TryEnterDeathsDoor(sv);

            _inv = new InventoryClass { Capacity = 80 };
            _medical = new MedicalSystem(_needs, _inv, shelter: null);
            foreach (var a in MedicalSystem.CreateDefaultAfflictions())
                _medical.RegisterAffliction(a);

            _medic = MakeSurvivor("sv_medic", "Medic");
            _patient = MakeSurvivor("sv_patient", "Patient");
            _survivors.Add(_medic);
            _survivors.Add(_patient);

            _medical.BindMedicalPerks(
                _perks,
                findSurvivor: id =>
                {
                    if (id == _medic.Id) return _medic;
                    if (id == _patient.Id) return _patient;
                    return null;
                },
                surgeryRng: new System.Random(1));
        }

        private static Survivor MakeSurvivor(string id, string name)
        {
            var sv = new Survivor
            {
                Id = id,
                DisplayName = name,
                State = SurvivorState.Idle,
                MedicalSkill = 0.8f
            };
            sv.Needs.Morale = 50f;
            sv.Needs.Health = 100f;
            sv.Needs.Fatigue = 10f;
            return sv;
        }

        private static ItemDefinition MakeItem(string id, ItemType type = ItemType.Medical)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.type = type;
            item.stackMax = 99;
            item.weight = 0.1f;
            return item;
        }

        private TreatmentRecipeSO MakeSurgicalRecipe(string targetAfflictionId)
        {
            var r = ScriptableObject.CreateInstance<TreatmentRecipeSO>();
            r.id = "treat_surgical_" + targetAfflictionId;
            r.displayName = "Surgical Care";
            r.targetAfflictionId = targetAfflictionId;
            r.baseTreatmentHours = 4f;
            r.requiresMedicalBed = false;
            r.requiresPatientRest = false;
            r.haltOnly = false;
            r.isSurgical = true;
            r.healthRestoreOnCure = 10f;
            r.ingredients = new List<TreatmentIngredient>();
            _medical.RegisterTreatment(r);
            return r;
        }

        // ── #201 Steady Hands ────────────────────────────────────────────

        [Test]
        public void SteadyHands_EarnedAfter3Phase2Cures()
        {
            Assert.IsFalse(_perks.Has(_medic, MedicalPerkSystem.SteadyHandsId));
            for (int i = 0; i < MedicalPerkSystem.Phase2CuresForSteadyHands - 1; i++)
                _perks.RecordPhase2Cure(_medic, AfflictionSO.Ids.Sepsis, isPhase2: true, 1);
            Assert.IsFalse(_perks.Has(_medic, MedicalPerkSystem.SteadyHandsId));

            _perks.RecordPhase2Cure(_medic, AfflictionSO.Ids.Sepsis, isPhase2: true, 1);
            Assert.IsTrue(_perks.Has(_medic, MedicalPerkSystem.SteadyHandsId));
        }

        [Test]
        public void SteadyHands_SepsisCountsEvenIfNotPhase2Flag()
        {
            for (int i = 0; i < MedicalPerkSystem.Phase2CuresForSteadyHands; i++)
                _perks.RecordPhase2Cure(_medic, AfflictionSO.Ids.Sepsis, isPhase2: false, 1);
            Assert.IsTrue(_perks.Has(_medic, MedicalPerkSystem.SteadyHandsId));
        }

        [Test]
        public void SteadyHands_SurgeryDuration_30PercentFaster()
        {
            var recipe = MakeSurgicalRecipe(AfflictionSO.Ids.GunshotWound);
            float baseHours = MedicalSystem.ComputeTreatmentHours(recipe, 0.8f, _medic, null);
            float withPerk = MedicalSystem.ComputeTreatmentHours(recipe, 0.8f, _medic, _perks);
            Assert.AreEqual(baseHours, withPerk, 0.001f, "No perk: same duration");

            for (int i = 0; i < MedicalPerkSystem.Phase2CuresForSteadyHands; i++)
                _perks.RecordPhase2Cure(_medic, AfflictionSO.Ids.RadBurns, isPhase2: true, 1);

            float steady = MedicalSystem.ComputeTreatmentHours(recipe, 0.8f, _medic, _perks);
            Assert.AreEqual(baseHours * MedicalPerkSystem.SteadyHandsSurgeryDurationMult, steady, 0.001f);
            Assert.AreEqual(0.70f, _perks.GetSurgeryDurationMultiplier(_medic), 0.001f);
        }

        [Test]
        public void SteadyHands_SurgeryBleedChance_ZeroWithPerk()
        {
            Assert.AreEqual(MedicalPerkSystem.BaseSurgeryBleedChance, _perks.GetSurgeryBleedChance(_medic), 0.001f);
            for (int i = 0; i < MedicalPerkSystem.Phase2CuresForSteadyHands; i++)
                _perks.RecordPhase2Cure(_medic, AfflictionSO.Ids.Sepsis, isPhase2: true, 1);
            Assert.AreEqual(0f, _perks.GetSurgeryBleedChance(_medic), 0.001f);
            Assert.IsFalse(_perks.RollSurgeryBleed(_medic, new System.Random(99)));
        }

        [Test]
        public void SteadyHands_CompleteSurgicalTreatment_NoBleedWithPerk()
        {
            for (int i = 0; i < MedicalPerkSystem.Phase2CuresForSteadyHands; i++)
                _perks.RecordPhase2Cure(_medic, AfflictionSO.Ids.Sepsis, isPhase2: true, 1);

            // Force-register gunshot and surgical recipe; complete via tick.
            _medical.Inflict(_patient, AfflictionSO.Ids.GunshotWound);
            var recipe = MakeSurgicalRecipe(AfflictionSO.Ids.GunshotWound);
            Assert.IsTrue(_medical.TryStartTreatment(_medic, _patient, recipe));
            _medical.Tick(new List<Survivor> { _patient }, 100f);

            Assert.IsFalse(_medical.HasAffliction(_patient, AfflictionSO.Ids.Bleeding),
                "Steady Hands must never inflict accidental Bleeding");
            Assert.IsFalse(_medical.HasAffliction(_patient, AfflictionSO.Ids.GunshotWound));
        }

        // ── #202 Triage Under Fire ───────────────────────────────────────

        [Test]
        public void TriageUnderFire_EarnedByBandagingDuringRaid()
        {
            Assert.IsFalse(_perks.Has(_medic, MedicalPerkSystem.TriageUnderFireId));
            _perks.RecordBandageDuringRaid(_medic, raidActive: false, 1);
            Assert.IsFalse(_perks.Has(_medic, MedicalPerkSystem.TriageUnderFireId));

            _perks.RecordBandageDuringRaid(_medic, raidActive: true, 1);
            Assert.IsTrue(_perks.Has(_medic, MedicalPerkSystem.TriageUnderFireId));
        }

        [Test]
        public void TriageUnderFire_UninterruptibleMedicalAction()
        {
            _perks.BeginMedicalAction(_medic);
            Assert.IsTrue(_perks.ShouldInterruptMedicalActionByDamage(_medic),
                "Without perk, damage interrupts medical action");

            _perks.RecordBandageDuringRaid(_medic, raidActive: true, 1);
            Assert.IsTrue(_perks.IsMedicalActionUninterruptible(_medic));
            Assert.IsFalse(_perks.ShouldInterruptMedicalActionByDamage(_medic),
                "Triage Under Fire: never interrupt medical action by damage");

            _perks.EndMedicalAction(_medic);
            Assert.IsFalse(_perks.ShouldInterruptMedicalActionByDamage(_medic));
        }

        [Test]
        public void TriageUnderFire_EmergencyHaltDuringRaidWindow_GrantsPerk()
        {
            var bandage = MakeItem(MedicalPerkSystem.BandageItemId);
            _inv.Add(bandage, 2);
            _medical.Inflict(_patient, AfflictionSO.Ids.GunshotWound);
            _medical.IsRaidWindowActive = () => true;

            Assert.IsTrue(_medical.TryEmergencyHalt(
                _patient, MedicalPerkSystem.BandageItemId, bandage, medic: _medic));
            Assert.IsTrue(_perks.Has(_medic, MedicalPerkSystem.TriageUnderFireId));
        }

        // ── #203 Radiologist ─────────────────────────────────────────────

        [Test]
        public void Radiologist_EarnedByTreatingManifestPatient()
        {
            _patient.PrognosisStage = PrognosisStage.Manifest;
            _patient.LatentDamage = 42f;
            _patient.OnsetTimer = 1.5f;

            Assert.IsFalse(_perks.Has(_medic, MedicalPerkSystem.RadiologistId));
            _perks.RecordManifestRadiationTreatment(_medic, _patient, 1);
            Assert.IsTrue(_perks.Has(_medic, MedicalPerkSystem.RadiologistId));
        }

        [Test]
        public void Radiologist_RevealLatentWithoutKit()
        {
            _patient.LatentDamage = 55f;
            _patient.OnsetTimer = 2.25f;
            _patient.PrognosisStage = PrognosisStage.Manifest;

            // Without perk, kit is required:
            bool kitTried = false;
            Assert.IsFalse(_perks.TryRevealLatentDamage(
                _medic, _patient, () => { kitTried = true; return false; },
                out _, out _));
            Assert.IsTrue(kitTried);

            _perks.RecordManifestRadiationTreatment(_medic, _patient, 1);
            Assert.IsTrue(_perks.HasRadiologist(_medic));

            kitTried = false;
            Assert.IsTrue(_perks.TryRevealLatentDamage(
                _medic, _patient, () => { kitTried = true; return false; },
                out float latent, out float onset));
            Assert.IsFalse(kitTried, "Radiologist must not consume a kit");
            Assert.AreEqual(55f, latent, 0.001f);
            Assert.AreEqual(2.25f, onset, 0.001f);
        }

        [Test]
        public void Radiologist_NonRadiologist_ConsumesKit()
        {
            _patient.LatentDamage = 10f;
            _patient.OnsetTimer = 3f;
            int kits = 1;
            Assert.IsTrue(_perks.TryRevealLatentDamage(
                _medic, _patient,
                () =>
                {
                    if (kits <= 0) return false;
                    kits--;
                    return true;
                },
                out float latent, out float onset));
            Assert.AreEqual(0, kits);
            Assert.AreEqual(10f, latent, 0.001f);
            Assert.AreEqual(3f, onset, 0.001f);
        }

        // ── #204 Anatomist ───────────────────────────────────────────────

        [Test]
        public void Anatomist_EarnedByAmputation_SuppressesPhantomPain()
        {
            var amp = new AmputationSystem();
            amp.Bind(
                findSurvivor: id => id == _patient.Id ? _patient : id == _medic.Id ? _medic : null,
                inflictAffliction: (sv, affId) => _medical.Inflict(sv, affId));
            amp.BindMedicalPerks(_perks, () => 1);

            // Register PhantomPain so inflictions during daily tick resolve cleanly.
            var phantom = ScriptableObject.CreateInstance<AfflictionSO>();
            phantom.id = AfflictionSO.Ids.PhantomPain;
            phantom.displayName = "Phantom Pain";
            _medical.RegisterAffliction(phantom);

            var tools = MakeItem(AmputationSystem.SurgicalToolsItemId, ItemType.Tool);
            var morphine = MakeItem(AmputationSystem.MorphineItemId);
            int toolsLeft = 1, morphLeft = 1;

            _medic.MedicalSkill = 1f;
            Assert.IsTrue(amp.PerformAmputation(
                _patient.Id, _medic.Id,
                countItem: id =>
                {
                    if (id == AmputationSystem.SurgicalToolsItemId) return toolsLeft;
                    if (id == AmputationSystem.MorphineItemId) return morphLeft;
                    return 0;
                },
                consumeItem: (id, n) =>
                {
                    if (id == AmputationSystem.SurgicalToolsItemId) { toolsLeft -= n; return true; }
                    if (id == AmputationSystem.MorphineItemId) { morphLeft -= n; return true; }
                    return false;
                }));

            Assert.IsTrue(_perks.Has(_medic, MedicalPerkSystem.AnatomistId));
            Assert.IsTrue(_perks.SuppressesPhantomPain(_patient.Id));
            Assert.AreEqual(0f, _perks.GetPhantomPainDailyChance(_patient.Id), 0.001f);

            // Daily tick must never inflict PhantomPain for clean amputee.
            for (int i = 0; i < 50; i++)
                amp.TickDaily(new List<Survivor> { _patient });
            Assert.IsFalse(_medical.HasAffliction(_patient, AfflictionSO.Ids.PhantomPain));
        }

        // ── #205 Paramedic ───────────────────────────────────────────────

        [Test]
        public void Paramedic_EarnedByComaRevive()
        {
            Assert.IsFalse(_perks.Has(_medic, MedicalPerkSystem.ParamedicId));
            _perks.RecordComaRevive(_medic, 1);
            Assert.IsTrue(_perks.Has(_medic, MedicalPerkSystem.ParamedicId));
        }

        [Test]
        public void Paramedic_DeathsDoor_DefersDeath_AndAdrenalineRevives()
        {
            _perks.RecordComaRevive(_medic, 1); // grant Paramedic
            Assert.IsTrue(_perks.HasParamedic(_medic));

            _patient.Needs.Health = 5f;
            _needs.Modify(_patient, NeedKind.Health, -10f); // → 0
            Assert.AreNotEqual(SurvivorState.Dead, _patient.State);
            Assert.IsTrue(_perks.IsOnDeathsDoor(_patient));
            Assert.AreEqual(0f, _patient.Needs.Health, 0.001f);
            Assert.AreEqual(MedicalPerkSystem.DeathsDoorHours,
                _perks.GetDeathsDoorHoursRemaining(_patient), 0.001f);

            // Non-paramedic cannot revive
            var other = MakeSurvivor("sv_other", "Other");
            _survivors.Add(other);
            Assert.IsFalse(_perks.TryAdministerAdrenaline(other, _patient, () => true));

            // Paramedic + adrenaline → 1 HP
            Assert.IsTrue(_perks.TryAdministerAdrenaline(_medic, _patient, () => true));
            Assert.IsFalse(_perks.IsOnDeathsDoor(_patient));
            Assert.AreEqual(MedicalPerkSystem.AdrenalineReviveHealth, _patient.Needs.Health, 0.001f);
            Assert.AreNotEqual(SurvivorState.Dead, _patient.State);
        }

        [Test]
        public void Paramedic_DeathsDoor_ExpiresToTrueDeath()
        {
            _perks.RecordComaRevive(_medic, 1);
            _patient.Needs.Health = 1f;
            _needs.Modify(_patient, NeedKind.Health, -5f);
            Assert.IsTrue(_perks.IsOnDeathsDoor(_patient));

            _perks.TickDeathsDoor(
                MedicalPerkSystem.DeathsDoorHours + 0.1f,
                _survivors,
                forceDeath: sv => _needs.ForceDeath(sv));

            Assert.IsFalse(_perks.IsOnDeathsDoor(_patient));
            Assert.AreEqual(SurvivorState.Dead, _patient.State);
        }

        [Test]
        public void Paramedic_WithoutColonyParamedic_DiesAtZero()
        {
            // No Paramedic granted
            _patient.Needs.Health = 1f;
            _needs.Modify(_patient, NeedKind.Health, -5f);
            Assert.AreEqual(SurvivorState.Dead, _patient.State);
            Assert.IsFalse(_perks.IsOnDeathsDoor(_patient));
        }

        // ── Save / load ──────────────────────────────────────────────────

        [Test]
        public void MedicalPerks_SaveLoad_RoundTrip()
        {
            for (int i = 0; i < 2; i++)
                _perks.RecordPhase2Cure(_medic, AfflictionSO.Ids.Sepsis, isPhase2: true, 1);
            _perks.RecordBandageDuringRaid(_medic, true, 1);
            _patient.PrognosisStage = PrognosisStage.Manifest;
            _perks.RecordManifestRadiationTreatment(_medic, _patient, 1);

            var save = _perks.CaptureState();
            var restored = new MedicalPerkSystem();
            restored.Bind(_progression, () => _survivors);
            restored.RestoreState(save);

            Assert.AreEqual(2, restored.GetCounters(_medic.Id).Phase2Cures);
            Assert.IsTrue(restored.Has(_medic, MedicalPerkSystem.TriageUnderFireId));
            Assert.IsTrue(restored.Has(_medic, MedicalPerkSystem.RadiologistId));
        }
    }
}
