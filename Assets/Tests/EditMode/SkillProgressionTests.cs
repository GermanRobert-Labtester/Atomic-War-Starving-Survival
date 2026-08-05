using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompts #179–#181: action-driven XP, dormant perks, stress epiphany.
    /// </summary>
    [TestFixture]
    public class SkillProgressionTests
    {
        private SkillProgressionSystem _sys;
        private Survivor _elena;
        private Survivor _marcus;

        [SetUp]
        public void SetUp()
        {
            _sys = new SkillProgressionSystem();
            _sys.RegisterDefaultPerks();

            _elena = new Survivor
            {
                Id = "sv_elena",
                DisplayName = "Elena",
                MedicalSkill = 0.70f,
                CraftingSkill = 0.25f,
                ScienceSkill = 0.40f,
                ExpertDisciplineId = "medical"
            };
            _marcus = new Survivor
            {
                Id = "sv_marcus",
                DisplayName = "Marcus",
                MedicalSkill = 0.30f,
                CraftingSkill = 0.65f,
                ScienceSkill = 0.25f,
                ExpertDisciplineId = "crafting"
            };
        }

        // ─── Prompt #179: XP + perk assignment ───────────────────────────

        [Test]
        public void RecordAction_AccumulatesHiddenXp()
        {
            _sys.RecordAction(_elena, "medical", 5f, currentDay: 1);
            _sys.RecordAction(_elena, "medical", 5f, currentDay: 1);
            Assert.AreEqual(10f, _sys.GetXp(_elena.Id, "medical"), 0.001f);
        }

        [Test]
        public void RecordAction_CrossingThreshold_AwardsPerk()
        {
            bool earned = false;
            string earnedId = null;
            _sys.OnPerkEarned += (sv, perk) =>
            {
                if (perk != null && perk.id == "perk_field_dressing")
                {
                    earned = true;
                    earnedId = perk.id;
                }
            };

            // field dressing threshold = 50
            for (int i = 0; i < 10; i++)
                _sys.RecordAction(_elena, "medical", 5f, currentDay: 1);

            Assert.IsTrue(earned, "Should earn field dressing perk at 50 XP.");
            Assert.IsTrue(_sys.HasActivePerk(_elena.Id, "perk_field_dressing"));
            Assert.AreEqual("perk_field_dressing", earnedId);
            Assert.Greater(_elena.ProgressionMedicalBonus, 0f);
            Assert.Greater(_elena.EffectiveMedicalSkill, _elena.MedicalSkill);
        }

        [Test]
        public void ExpertPerk_OnlyEarnedOnMatchingTrack_AndOnlyOnce()
        {
            // Marcus is crafting expert — cannot earn medical expert.
            for (int i = 0; i < 30; i++)
                _sys.RecordAction(_marcus, "medical", 5f, currentDay: 1);

            Assert.IsTrue(_sys.HasActivePerk(_marcus.Id, "perk_field_dressing"),
                "Non-expert medical perk still available.");
            Assert.IsFalse(_sys.HasActivePerk(_marcus.Id, "perk_steady_hands"),
                "Expert medical perk blocked for crafting expert.");
            Assert.IsFalse(_sys.HasEarnedExpertPerk(_marcus.Id));

            // Marcus earns crafting expert at 120 XP.
            for (int i = 0; i < 30; i++)
                _sys.RecordAction(_marcus, "crafting", 5f, currentDay: 2);

            Assert.IsTrue(_sys.HasActivePerk(_marcus.Id, "perk_workshop_sense"));
            Assert.IsTrue(_sys.HasEarnedExpertPerk(_marcus.Id));

            // Even with more medical XP, cannot earn second expert.
            for (int i = 0; i < 30; i++)
                _sys.RecordAction(_marcus, "medical", 5f, currentDay: 3);
            Assert.IsFalse(_sys.HasActivePerk(_marcus.Id, "perk_steady_hands"));
        }

        [Test]
        public void Elena_EarnsMedicalExpert_NotCraftingExpert()
        {
            for (int i = 0; i < 30; i++)
                _sys.RecordAction(_elena, "medical", 5f, currentDay: 1);

            Assert.IsTrue(_sys.HasActivePerk(_elena.Id, "perk_steady_hands"));
            Assert.IsTrue(_sys.HasEarnedExpertPerk(_elena.Id));

            for (int i = 0; i < 30; i++)
                _sys.RecordAction(_elena, "crafting", 5f, currentDay: 2);

            Assert.IsTrue(_sys.HasActivePerk(_elena.Id, "perk_rough_repairs"));
            Assert.IsFalse(_sys.HasActivePerk(_elena.Id, "perk_workshop_sense"),
                "Second expert track blocked.");
        }

        [Test]
        public void EmptyDiscipline_OrZeroXp_IsNoOp()
        {
            _sys.RecordAction(_elena, null, 5f, 1);
            _sys.RecordAction(_elena, "medical", 0f, 1);
            _sys.RecordAction(null, "medical", 5f, 1);
            Assert.AreEqual(0f, _sys.GetXp(_elena.Id, "medical"));
        }

        // ─── Prompt #180: dormant after 14 days ──────────────────────────

        [Test]
        public void TickDaily_After14DaysUnused_PerkBecomesDormant()
        {
            for (int i = 0; i < 10; i++)
                _sys.RecordAction(_elena, "medical", 5f, currentDay: 1);

            Assert.IsTrue(_sys.HasActivePerk(_elena.Id, "perk_field_dressing"));
            float bonusWhileActive = _elena.ProgressionMedicalBonus;
            Assert.Greater(bonusWhileActive, 0f);

            bool wentDormant = false;
            _sys.OnPerkDormant += (sv, perk) =>
            {
                if (perk != null && perk.id == "perk_field_dressing")
                    wentDormant = true;
            };

            var list = new List<Survivor> { _elena };
            // Day 1 used; day 15 = 14 days later → dormant.
            _sys.TickDaily(15, list);

            Assert.IsTrue(wentDormant);
            Assert.IsFalse(_sys.HasActivePerk(_elena.Id, "perk_field_dressing"));
            Assert.IsTrue(_sys.HasDormantPerk(_elena.Id, "perk_field_dressing"));
            Assert.AreEqual(0f, _elena.ProgressionMedicalBonus, 0.001f,
                "Dormant perk loses mechanical benefit.");
            Assert.AreEqual(_elena.MedicalSkill, _elena.EffectiveMedicalSkill, 0.001f);
        }

        [Test]
        public void RecordAction_ReactivatesDormantPerk()
        {
            for (int i = 0; i < 10; i++)
                _sys.RecordAction(_elena, "medical", 5f, currentDay: 1);

            _sys.TickDaily(15, new List<Survivor> { _elena });
            Assert.IsTrue(_sys.HasDormantPerk(_elena.Id, "perk_field_dressing"));

            bool reactivated = false;
            _sys.OnPerkReactivated += (sv, perk) =>
            {
                if (perk != null && perk.id == "perk_field_dressing")
                    reactivated = true;
            };

            _sys.RecordAction(_elena, "medical", 5f, currentDay: 16);
            Assert.IsTrue(reactivated);
            Assert.IsTrue(_sys.HasActivePerk(_elena.Id, "perk_field_dressing"));
            Assert.IsFalse(_sys.HasDormantPerk(_elena.Id, "perk_field_dressing"));
            Assert.Greater(_elena.ProgressionMedicalBonus, 0f);
        }

        [Test]
        public void TickDaily_PracticingKeepsPerkActive()
        {
            for (int i = 0; i < 10; i++)
                _sys.RecordAction(_elena, "medical", 5f, currentDay: 1);

            // Practice every few days — never hits 14 unused.
            for (int day = 5; day <= 40; day += 5)
            {
                _sys.RecordAction(_elena, "medical", 1f, currentDay: day);
                _sys.TickDaily(day, new List<Survivor> { _elena });
            }

            Assert.IsTrue(_sys.HasActivePerk(_elena.Id, "perk_field_dressing"));
            Assert.IsFalse(_sys.HasDormantPerk(_elena.Id, "perk_field_dressing"));
        }

        // ─── Prompt #181: stress epiphany ────────────────────────────────

        [Test]
        public void StressEpiphany_AtLowMorale_InstantMasteryAndMoraleRestore()
        {
            _elena.Needs.Morale = 5f;
            _elena.Needs.Health = 80f;

            bool epiphany = false;
            _sys.OnEpiphany += (sv, perk) => epiphany = true;

            // Deterministic: rng always rolls 0 → always within 5%.
            var rng = new System.Random(0);
            // Force NextDouble < 0.05 by using a custom seed that works... 
            // System.Random(0).NextDouble() first call is ~0.72 — not reliable.
            // Use a stub random via multiple attempts with forced low morale.
            // Better: inject by looping with FixedRandom below.

            var forced = new FixedRandom(0.01); // always 1%
            _sys.RecordAction(_elena, "medical", 5f, currentDay: 1, rng: forced);

            Assert.IsTrue(epiphany, "Epiphany should fire at low morale with roll < 5%.");
            Assert.AreEqual(100f, _elena.Needs.Morale, 0.001f, "Epiphany cures low morale.");
            Assert.IsTrue(_sys.HasActivePerk(_elena.Id, "perk_steady_hands")
                          || _sys.HasActivePerk(_elena.Id, "perk_field_dressing"),
                "Epiphany should grant medical mastery perks.");
            Assert.GreaterOrEqual(_sys.GetXp(_elena.Id, "medical"), 50f);
        }

        [Test]
        public void StressEpiphany_HealthySurvivor_DoesNotFire()
        {
            _elena.Needs.Morale = 50f;
            _elena.Needs.Health = 80f;

            bool epiphany = false;
            _sys.OnEpiphany += (sv, perk) => epiphany = true;

            var forced = new FixedRandom(0.01);
            _sys.RecordAction(_elena, "medical", 5f, currentDay: 1, rng: forced);

            Assert.IsFalse(epiphany);
            Assert.AreEqual(5f, _sys.GetXp(_elena.Id, "medical"), 0.001f);
        }

        [Test]
        public void StressEpiphany_LowHealth_AlsoQualifies()
        {
            _elena.Needs.Morale = 50f;
            _elena.Needs.Health = 10f;

            bool epiphany = false;
            _sys.OnEpiphany += (sv, perk) => epiphany = true;

            _sys.RecordAction(_elena, "medical", 5f, currentDay: 1, rng: new FixedRandom(0.01));
            Assert.IsTrue(epiphany);
            Assert.AreEqual(100f, _elena.Needs.Morale, 0.001f);
        }

        // ─── Save / load ─────────────────────────────────────────────────

        [Test]
        public void CaptureRestore_PreservesXpPerksAndDormant()
        {
            for (int i = 0; i < 10; i++)
                _sys.RecordAction(_elena, "medical", 5f, currentDay: 1);
            _sys.TickDaily(15, new List<Survivor> { _elena });

            var save = _sys.CaptureState();
            var restored = new SkillProgressionSystem();
            restored.RegisterDefaultPerks();
            restored.RestoreState(save, new List<Survivor> { _elena });

            Assert.AreEqual(50f, restored.GetXp(_elena.Id, "medical"), 0.001f);
            Assert.IsTrue(restored.HasDormantPerk(_elena.Id, "perk_field_dressing"));
            Assert.AreEqual(0f, _elena.ProgressionMedicalBonus, 0.001f);
        }

        /// <summary>RNG that always returns a fixed NextDouble.</summary>
        private sealed class FixedRandom : System.Random
        {
            private readonly double _value;
            public FixedRandom(double value) { _value = value; }
            public override double NextDouble() => _value;
        }
    }
}
