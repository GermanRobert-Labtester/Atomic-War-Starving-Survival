using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompts #211–#213 — social / leadership milestone perks.
    /// </summary>
    [TestFixture]
    public class SocialPerkTests
    {
        private SkillProgressionSystem _progression;
        private SocialPerkSystem _perks;
        private MentalBreakSystem _mental;
        private Survivor _sv;
        private Survivor _other;
        private List<Survivor> _survivors;
        private MentalBreakSO _violent;

        [SetUp]
        public void SetUp()
        {
            _progression = new SkillProgressionSystem();
            _progression.RegisterDefaultPerks();
            _perks = new SocialPerkSystem();
            _perks.Bind(_progression);

            _violent = ScriptableObject.CreateInstance<MentalBreakSO>();
            _violent.id = MentalBreakSO.Ids.ViolentParanoia;
            _violent.displayName = "Violent Paranoia";
            _violent.cureHours = 48f;
            _violent.comfortItemCureAmount = 24f;
            _violent.requiresMedicalBed = true;
            _violent.sabotageChancePerTick = 0.1f;

            _mental = new MentalBreakSystem();
            _mental.RegisterBreak(_violent);

            _sv = MakeSurvivor("sv_leader", "Leader");
            _other = MakeSurvivor("sv_broken", "Broken");
            _survivors = new List<Survivor> { _sv, _other };
        }

        [TearDown]
        public void TearDown()
        {
            if (_violent != null) Object.DestroyImmediate(_violent);
        }

        private static Survivor MakeSurvivor(string id, string name)
        {
            var sv = new Survivor
            {
                Id = id,
                DisplayName = name,
                State = SurvivorState.Idle,
                CurrentRoomId = "stores"
            };
            sv.Needs.Morale = 70f;
            sv.Needs.Health = 100f;
            sv.Needs.Fatigue = 10f;
            return sv;
        }

        // ── #211 De-Escalator ────────────────────────────────────────────

        [Test]
        public void DeEscalator_EarnedAfterOnePeacefulDeEscalation()
        {
            Assert.IsFalse(_perks.Has(_sv, SocialPerkSystem.DeEscalatorId));
            _perks.RecordPeacefulDeEscalation(_sv, 1);
            Assert.IsTrue(_perks.Has(_sv, SocialPerkSystem.DeEscalatorId));
            Assert.AreEqual(1, _perks.GetCounters(_sv.Id).PeacefulDeEscalations);
        }

        [Test]
        public void TalkDown_RequiresPerk_AndInstantlyCuresViolentParanoia()
        {
            _other.currentMentalBreakId = MentalBreakSO.Ids.ViolentParanoia;
            _other.mentalBreakCureProgress = 0f;

            // Without perk: no cure.
            Assert.IsFalse(_perks.TryTalkDown(_sv, _other, _mental, 1));
            Assert.IsTrue(_other.HasMentalBreak);

            _perks.RecordPeacefulDeEscalation(_sv, 1);
            Assert.IsTrue(_perks.HasDeEscalator(_sv));

            Assert.IsTrue(_perks.TryTalkDown(_sv, _other, _mental, 1));
            Assert.IsFalse(_other.HasMentalBreak);
            Assert.IsNull(_other.currentMentalBreakId);
        }

        [Test]
        public void TalkDown_IgnoresNonViolentBreaks()
        {
            _perks.RecordPeacefulDeEscalation(_sv, 1);
            _other.currentMentalBreakId = MentalBreakSO.Ids.BingeEater;
            Assert.IsFalse(_perks.TryTalkDown(_sv, _other, _mental, 1));
            Assert.IsTrue(_other.HasMentalBreak);
        }

        [Test]
        public void TalkDownAction_ScoresOnlyWithPerkAndViolentTarget()
        {
            var action = ScriptableObject.CreateInstance<TalkDownActionSO>();
            try
            {
                var ctx = new AIContext(_sv)
                {
                    SocialPerks = _perks,
                    MentalBreak = _mental,
                    GetSurvivors = () => _survivors
                };

                Assert.AreEqual(0f, action.EvaluateRaw(ctx), 0.001f);

                _perks.RecordPeacefulDeEscalation(_sv, 1);
                Assert.AreEqual(0f, action.EvaluateRaw(ctx), 0.001f,
                    "Perk alone without violent target → 0");

                _other.currentMentalBreakId = MentalBreakSO.Ids.ViolentParanoia;
                Assert.AreEqual(1f, action.EvaluateRaw(ctx), 0.001f);

                action.Execute(ctx);
                Assert.IsFalse(_other.HasMentalBreak);
            }
            finally
            {
                Object.DestroyImmediate(action);
            }
        }

        [Test]
        public void ComfortCure_OfViolentParanoia_EarnsDeEscalator()
        {
            _other.currentMentalBreakId = MentalBreakSO.Ids.ViolentParanoia;
            _other.mentalBreakCureProgress = 30f; // one comfort (24) finishes 48h cure
            _mental.ComfortCureHandler = (sv, br) => true;

            var action = ScriptableObject.CreateInstance<MentalBreakComfortActionSO>();
            try
            {
                var ctx = new AIContext(_sv)
                {
                    SocialPerks = _perks,
                    MentalBreak = _mental,
                    GetSurvivors = () => _survivors,
                    CurrentDay = 3
                };
                action.Execute(ctx);
                Assert.IsFalse(_other.HasMentalBreak);
                Assert.IsTrue(_perks.HasDeEscalator(_sv));
            }
            finally
            {
                Object.DestroyImmediate(action);
            }
        }

        // ── #212 Quartermaster ───────────────────────────────────────────

        [Test]
        public void Quartermaster_EarnedAfterHauling100Items()
        {
            Assert.IsFalse(_perks.Has(_sv, SocialPerkSystem.QuartermasterId));
            _perks.RecordItemsHauledCount(_sv, SocialPerkSystem.ItemsHauledForQuartermaster - 1, 1);
            Assert.IsFalse(_perks.Has(_sv, SocialPerkSystem.QuartermasterId));
            _perks.RecordItemsHauledCount(_sv, 1, 1);
            Assert.IsTrue(_perks.Has(_sv, SocialPerkSystem.QuartermasterId));
            Assert.AreEqual(100, _perks.GetCounters(_sv.Id).ItemsHauled);
        }

        [Test]
        public void Quartermaster_WeightKgMapsToItems()
        {
            _perks.RecordItemsHauled(_sv, 20.7f, 1);
            Assert.AreEqual(20, _perks.GetCounters(_sv.Id).ItemsHauled);
            _perks.RecordItemsHauled(_sv, 0.4f, 1); // fractional still counts ≥1
            Assert.AreEqual(21, _perks.GetCounters(_sv.Id).ItemsHauled);
        }

        [Test]
        public void Quartermaster_HalvesDegradationInSameRoom()
        {
            _sv.CurrentRoomId = "stores";
            Assert.AreEqual(1f, _perks.GetItemDegradationMultiplier("stores", _survivors), 0.001f);

            _perks.RecordItemsHauledCount(_sv, 100, 1);
            Assert.IsTrue(_perks.HasQuartermaster(_sv));
            Assert.AreEqual(SocialPerkSystem.QuartermasterDegradationMult,
                _perks.GetItemDegradationMultiplier("stores", _survivors), 0.001f);
            Assert.AreEqual(1f, _perks.GetItemDegradationMultiplier("quarters", _survivors), 0.001f);
        }

        [Test]
        public void Quartermaster_WeaponRustUsesRateMult()
        {
            var weapons = new WeaponMaintenanceSystem();
            weapons.Fire("rifle_1"); // ensure entry exists
            // Reset to full then rust
            weapons.OilWeapon("rifle_1");
            float full = weapons.GetDurability("rifle_1");

            weapons.TickRust("rifle_1", 10f, humidity: 0.9f, rateMult: 1f);
            float normal = weapons.GetDurability("rifle_1");
            float normalLoss = full - normal;

            weapons.OilWeapon("rifle_1");
            weapons.TickRust("rifle_1", 10f, humidity: 0.9f,
                rateMult: SocialPerkSystem.QuartermasterDegradationMult);
            float half = weapons.GetDurability("rifle_1");
            float halfLoss = full - half;

            Assert.AreEqual(normalLoss * 0.5f, halfLoss, 0.01f);
        }

        [Test]
        public void HaulLootAction_RecordsItemsTowardQuartermaster()
        {
            var haul = new InternalHaulingSystem();
            haul.DumpLootInAirlock(50f);
            var action = ScriptableObject.CreateInstance<HaulLootActionSO>();
            try
            {
                var ctx = new AIContext(_sv)
                {
                    HaulingSystem = haul,
                    SocialPerks = _perks,
                    CurrentDay = 2
                };
                action.Execute(ctx);
                // 20 kg/hour → 20 items
                Assert.AreEqual(20, _perks.GetCounters(_sv.Id).ItemsHauled);
            }
            finally
            {
                Object.DestroyImmediate(action);
            }
        }

        // ── #213 Taskmaster ──────────────────────────────────────────────

        [Test]
        public void Taskmaster_EarnedAfter14HighMoraleDays()
        {
            _sv.Needs.Morale = 91f;
            for (int i = 0; i < SocialPerkSystem.HighMoraleDaysForTaskmaster - 1; i++)
            {
                _perks.TickDailyMorale(_survivors, i + 1);
                Assert.IsFalse(_perks.Has(_sv, SocialPerkSystem.TaskmasterId));
            }
            _perks.TickDailyMorale(_survivors, 14);
            Assert.IsTrue(_perks.Has(_sv, SocialPerkSystem.TaskmasterId));
            Assert.AreEqual(14, _perks.GetCounters(_sv.Id).HighMoraleDays);
        }

        [Test]
        public void Taskmaster_StreakResetsBelowThreshold()
        {
            _sv.Needs.Morale = 95f;
            for (int i = 0; i < 10; i++)
                _perks.TickDailyMorale(_survivors, i + 1);
            Assert.AreEqual(10, _perks.GetCounters(_sv.Id).HighMoraleDays);

            _sv.Needs.Morale = 90f; // not strictly > 90
            _perks.TickDailyMorale(_survivors, 11);
            Assert.AreEqual(0, _perks.GetCounters(_sv.Id).HighMoraleDays);
            Assert.IsFalse(_perks.HasTaskmaster(_sv));
        }

        [Test]
        public void Taskmaster_PacingAura_SameAndAdjacentRooms()
        {
            _perks.RecordItemsHauledCount(_sv, 0, 0); // no-op ensure map
            // Force grant via high morale
            _sv.Needs.Morale = 99f;
            for (int d = 0; d < 14; d++)
                _perks.TickDailyMorale(_survivors, d + 1);
            Assert.IsTrue(_perks.HasTaskmaster(_sv));

            _sv.CurrentRoomId = "workshop";
            _other.CurrentRoomId = "workshop";
            Assert.AreEqual(SocialPerkSystem.TaskmasterActionSpeedMult,
                _perks.GetPacingAuraMultiplier(_other, _survivors, areRoomsAdjacent: null), 0.001f);

            _other.CurrentRoomId = "corridor";
            Assert.AreEqual(1f,
                _perks.GetPacingAuraMultiplier(_other, _survivors, areRoomsAdjacent: null), 0.001f,
                "Different room without adjacency → no aura");

            bool Adjacent(string a, string b) =>
                (a == "workshop" && b == "corridor") || (a == "corridor" && b == "workshop");
            Assert.AreEqual(SocialPerkSystem.TaskmasterActionSpeedMult,
                _perks.GetPacingAuraMultiplier(_other, _survivors, Adjacent), 0.001f);
        }

        [Test]
        public void Taskmaster_NoAuraWithoutPerk()
        {
            _sv.CurrentRoomId = "workshop";
            _other.CurrentRoomId = "workshop";
            Assert.AreEqual(1f,
                _perks.GetPacingAuraMultiplier(_other, _survivors, null), 0.001f);
        }

        // ── Catalog + save ───────────────────────────────────────────────

        [Test]
        public void RegisterSocialPerks_AreInCatalog()
        {
            Assert.IsNotNull(_progression.GetPerk(SocialPerkSystem.DeEscalatorId));
            Assert.IsNotNull(_progression.GetPerk(SocialPerkSystem.QuartermasterId));
            Assert.IsNotNull(_progression.GetPerk(SocialPerkSystem.TaskmasterId));
        }

        [Test]
        public void SocialPerks_SaveLoad_RoundTrip()
        {
            _perks.RecordPeacefulDeEscalation(_sv, 1);
            _perks.RecordItemsHauledCount(_sv, 55, 2);
            _sv.Needs.Morale = 95f;
            for (int i = 0; i < 7; i++)
                _perks.TickDailyMorale(_survivors, i + 1);

            var save = _perks.CaptureState();
            var progression2 = new SkillProgressionSystem();
            progression2.RegisterDefaultPerks();
            progression2.TryGrantPerk(_sv, SocialPerkSystem.DeEscalatorId, 1);

            var restored = new SocialPerkSystem();
            restored.Bind(progression2);
            restored.RestoreState(save);

            Assert.AreEqual(1, restored.GetCounters(_sv.Id).PeacefulDeEscalations);
            Assert.AreEqual(55, restored.GetCounters(_sv.Id).ItemsHauled);
            Assert.AreEqual(7, restored.GetCounters(_sv.Id).HighMoraleDays);
            Assert.IsTrue(restored.HasDeEscalator(_sv));
        }
    }
}
