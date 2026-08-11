using NUnit.Framework;
using AtomicWar._Game.Quests;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class QuestRegistryTests
    {
        [Test]
        public void RegistryContainsAllSevenNewQuests()
        {
            var reg = new QuestRegistry();
            Assert.IsNotNull(reg.Get(QuestRegistry.IdGarrisonLastOrder));
            Assert.IsNotNull(reg.Get(QuestRegistry.IdMilitiaGrainWar));
            Assert.IsNotNull(reg.Get(QuestRegistry.IdCultGlowCommunion));
            Assert.IsNotNull(reg.Get(QuestRegistry.IdElenaTriage));
            Assert.IsNotNull(reg.Get(QuestRegistry.IdMechanicHighwayHeart));
            Assert.IsNotNull(reg.Get(QuestRegistry.IdChildSoldierRifle));
            Assert.IsNotNull(reg.Get(QuestRegistry.IdDeepWell));
        }

        [Test]
        public void StartAdvancesToInProgress()
        {
            var reg = new QuestRegistry();
            reg.Start(QuestRegistry.IdDeepWell, 0);
            var s = reg.Get(QuestRegistry.IdDeepWell).State;
            Assert.AreEqual(QuestStatus.InProgress, s.Status);
            Assert.AreEqual(1, s.Stage);
        }
    }

    [TestFixture]
    public class GarrisonLastOrderTests
    {
        [Test]
        public void DestroyChoiceAppliesFactionTrustAndCompletes()
        {
            float garrisonTrust = 0, militiaTrust = 0, survivorTrust = 0;
            var reg = new QuestRegistry
            {
            };
            var q = new Quest_GarrisonLastOrder
            {
                AddFactionTrust = (f, d) => { if (f == "faction_garrison") garrisonTrust += d; },
                SubtractFactionTrust = (f, d) => { if (f == "faction_militia") militiaTrust += d; if (f == "faction_survivors") survivorTrust += d; },
                MarkLocationDestroyed = (l, k) => { },
                BroadcastRadioMessage = (f, m, c) => { },
                RecordMoralEntry = (t) => { }
            };
            q.Start(0);
            q.ResolveDestroy();
            Assert.AreEqual(QuestStatus.Success, q.State.Status);
            Assert.AreEqual(30f, garrisonTrust, 0.001f);
            Assert.AreEqual(-25f, militiaTrust, 0.001f);
            Assert.AreEqual(-15f, survivorTrust, 0.001f);
        }

        [Test]
        public void RefuseMarksGarrisonHostile()
        {
            float garrisonTrust = 0;
            var q = new Quest_GarrisonLastOrder
            {
                AddFactionTrust = (f, d) => { },
                SubtractFactionTrust = (f, d) => { if (f == "faction_garrison") garrisonTrust += d; },
                RecordMoralEntry = (t) => { }
            };
            q.Start(0);
            q.ResolveRefuse();
            Assert.AreEqual(QuestStatus.Success, q.State.Status);
            Assert.AreEqual(-40f, garrisonTrust, 0.001f);
        }
    }

    [TestFixture]
    public class MilitiaGrainWarTests
    {
        [Test]
        public void DiversionRefuseFailsQuest()
        {
            float militiaTrust = 0;
            var q = new Quest_MilitiaGrainWar
            {
                SubtractFactionTrust = (f, d) => { if (f == "faction_upland_militia") militiaTrust += d; },
                RecordMoralEntry = (t) => { }
            };
            q.Start(0);
            q.ResolveDiversionRefuse();
            Assert.AreEqual(QuestStatus.Failure, q.State.Status);
            Assert.AreEqual(-25f, militiaTrust, 0.001f);
        }
    }

    [TestFixture]
    public class ElenaTriageTests
        {
        [Test]
        public void FiveSuccessesCompleteAndGrantFieldTriage()
            {
            string perk = null;
            var q = new Quest_ElenaTriage
            {
                GrantPerk = (sv, id, n) => perk = id,
                ApplyMorale = (sv, m) => { },
                RecordMoralEntry = (t) => { }
            };
            q.Start(0);
            for (int i = 0; i < 5; i++) q.RecordTreatmentSuccess("sv_elena");
            Assert.AreEqual(QuestStatus.Success, q.State.Status);
            Assert.AreEqual("perk_field_triage", perk);
        }

        [Test]
        public void ThreeDeathsFailAndAfflictGuilt()
        {
            string aff = null;
            var q = new Quest_ElenaTriage
            {
                GrantPerk = (sv, id, n) => { },
                ApplyMorale = (sv, m) => { },
                AddAffliction = (sv, id) => aff = id,
                RecordMoralEntry = (t) => { }
            };
            q.Start(0);
            for (int i = 0; i < 3; i++) q.RecordPatientDiedUnderCare("sv_elena");
            Assert.AreEqual(QuestStatus.Failure, q.State.Status);
            Assert.AreEqual("affliction_survivors_guilt", aff);
        }
    }

    [TestFixture]
    public class MechanicHighwayHeartTests
    {
        [Test]
        public void FailedExtractionReducesDurability()
        {
            string given = null;
            var q = new Quest_MechanicHighwayHeart
            {
                GiveItem = (sv, id, n) => given = id,
                RecordMoralEntry = (t) => { }
            };
            q.Start(0);
            q.Advance(); // stage 1 -> 2
            q.Advance(); // stage 2 -> 3 (extraction)
            // Force a low roll: skill 0.9, roll 0.1 -> success first
            var rng = new System.Random(0); // deterministic
            q.ResolveAttemptExtraction(0.0f, rng);
            Assert.IsNotNull(given);
            // After failed attempt durability is reduced.
            Assert.LessOrEqual(q.GetProgress("engine_durability"), 100f);
        }
    }

    [TestFixture]
    public class ChildSoldierRifleTests
    {
        [Test]
        public void RaidDuringQuestLocksIt()
        {
            var q = new Quest_ChildSoldierRifle
            {
                TakeItem = (sv, id, n) => { },
                GrantPerk = (sv, id, n) => { },
                RecordMoralEntry = (t) => { }
            };
            q.Start(0);
            q.OnRaidDuringQuest();
            Assert.AreEqual(QuestStatus.Locked, q.State.Status);
        }

        [Test]
        public void ThreeTalkDaysAdvanceToStageTwo()
        {
            var q = new Quest_ChildSoldierRifle
            {
                TakeItem = (sv, id, n) => { },
                GrantPerk = (sv, id, n) => { },
                RecordMoralEntry = (t) => { }
            };
            q.Start(0);
            for (int i = 0; i < 3; i++) q.RecordTalkDay("sv_therapist");
            Assert.AreEqual(2, q.State.Stage);
        }
    }

    [TestFixture]
    public class DeepWellTests
    {
        [Test]
        public void EightExcavationDaysAdvanceToCompletion()
        {
            var q = new Quest_DeepWell
            {
                RecordMoralEntry = (t) => { }
            };
            q.Start(0);
            for (int d = 0; d < 8; d++) q.RecordExcavationDay();
            // Stage should advance to 5 (the final stage triggers Complete).
            Assert.AreEqual(QuestStatus.Success, q.State.Status);
        }
    }
}
