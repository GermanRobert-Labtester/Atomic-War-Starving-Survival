using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;
using Random = System.Random;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompt #17 — Radio Interception / Wiretapping: antenna gate, raid plans,
    /// warn / scavenge / do-nothing choices, intercept log, save/load.
    /// </summary>
    [TestFixture]
    public class FactionRaidInterceptTests
    {
        private const float Eps = 1e-3f;

        private List<FactionSO> _factions;
        private List<Object> _toDestroy;
        private WorldPhase _phase;
        private int _day;
        private bool _antennaUp;

        /// <summary>RNG that always schedules a plan on daily chance.</summary>
        private sealed class AlwaysScheduleRng : Random
        {
            public override double NextDouble() => 0.0;
            public override int Next(int minValue, int maxValue) => minValue;
        }

        /// <summary>RNG that never schedules.</summary>
        private sealed class NeverScheduleRng : Random
        {
            public override double NextDouble() => 0.99;
        }

        [SetUp]
        public void SetUp()
        {
            _phase = WorldPhase.NuclearWinter;
            _day = 40;
            _antennaUp = true;
            _factions = DynamicEconomySystem.CreateDefaultFactions();
            _toDestroy = new List<Object>();
            for (int i = 0; i < _factions.Count; i++)
                _toDestroy.Add(_factions[i]);
        }

        [TearDown]
        public void TearDown()
        {
            if (_toDestroy == null) return;
            for (int i = 0; i < _toDestroy.Count; i++)
            {
                if (_toDestroy[i] != null)
                    Object.DestroyImmediate(_toDestroy[i]);
            }
            _toDestroy = null;
            _factions = null;
        }

        private DynamicEconomySystem MakeEconomy()
        {
            var eco = new DynamicEconomySystem(() => _phase, null, new Random(1));
            eco.SetDayProvider(() => _day);
            for (int i = 0; i < _factions.Count; i++)
                eco.RegisterFaction(_factions[i]);
            return eco;
        }

        private FactionRadioInterceptSystem MakeRadio(DynamicEconomySystem eco)
        {
            var radio = new FactionRadioInterceptSystem();
            radio.Bind(eco, () => _day);
            return radio;
        }

        private FactionRaidPlanSystem MakePlans(
            DynamicEconomySystem eco,
            FactionRadioInterceptSystem radio,
            GeneratedMap map = null,
            Random rng = null,
            RadiationSystem radiation = null)
        {
            var sys = new FactionRaidPlanSystem(rng ?? new Random(7));
            sys.Bind(
                eco,
                radio,
                getDay: () => _day,
                isAntennaOperational: () => _antennaUp,
                map: map,
                radiation: radiation);
            return sys;
        }

        private static GeneratedMap MakeTinyMap()
        {
            var map = new GeneratedMap { Seed = 99 };
            map.Nodes.Add(new MapNode
            {
                NodeId = GeneratedMap.ShelterNodeId,
                DisplayName = "Shelter",
                Ring = DangerRing.Shelter,
                TrueRad = 0f
            });
            map.Nodes.Add(new MapNode
            {
                NodeId = "ring_road_a",
                DisplayName = "Ring Road",
                Ring = DangerRing.Suburbs,
                TrueRad = 12f
            });
            map.Nodes.Add(new MapNode
            {
                NodeId = "outskirts_b",
                DisplayName = "Outskirts",
                Ring = DangerRing.CityOutskirts,
                TrueRad = 40f
            });
            return map;
        }

        private static Survivor MakeSurvivor(string id = "s1")
        {
            var s = new Survivor
            {
                Id = id,
                DisplayName = id,
                State = SurvivorState.Idle,
                RadiationDose = 5f,
                LifetimeRadiationExposure = 5f
            };
            s.Needs.Hunger = 50f;
            s.Needs.Thirst = 50f;
            s.Needs.Fatigue = 50f;
            s.Needs.Warmth = 50f;
            s.Needs.Morale = 50f;
            s.Needs.Health = 80f;
            return s;
        }

        [Test]
        public void SchedulePlan_WithAntenna_PresentsInterceptAndRaidPlanLog()
        {
            var eco = MakeEconomy();
            var radio = MakeRadio(eco);
            var sys = MakePlans(eco, radio);
            FactionRadioInterceptSystem.InterceptEntry last = null;
            radio.OnIntercept += e => last = e;

            FactionRaidPlan offered = null;
            GameEvent offeredEv = null;
            sys.OnInterceptOffered += (p, ev) =>
            {
                offered = p;
                offeredEv = ev;
            };

            var plan = sys.SchedulePlan(
                FactionSO.Ids.MilitaryRemnants,
                FactionSO.Ids.ScavengerCamp,
                scheduleDay: _day,
                leadDays: 2);

            Assert.IsNotNull(plan);
            Assert.That(plan.FireDay, Is.EqualTo(_day + 2));
            Assert.IsTrue(plan.InterceptPresented);
            Assert.IsNotNull(last);
            Assert.That(last.Kind, Is.EqualTo(nameof(FactionRadioInterceptSystem.InterceptKind.RaidPlan)));
            Assert.That(last.Message, Does.Contain("Wiretap").IgnoreCase);
            Assert.That(last.Message, Does.Contain("MILBAND").Or.Contain("Military").IgnoreCase);
            Assert.IsNotNull(offered);
            Assert.IsNotNull(offeredEv);
            Assert.That(offeredEv.choices.Count, Is.EqualTo(3));
            Assert.That(offeredEv.choices.Exists(c => c.ChoiceId == FactionRaidPlanSystem.ChoiceDoNothing));
            Assert.That(offeredEv.choices.Exists(c => c.ChoiceId == FactionRaidPlanSystem.ChoiceWarnTarget));
            Assert.That(offeredEv.choices.Exists(c => c.ChoiceId == FactionRaidPlanSystem.ChoiceScavenge));

            Object.DestroyImmediate(offeredEv);
            radio.Unbind();
        }

        [Test]
        public void SchedulePlan_WithoutAntenna_SilentNoIntercept()
        {
            _antennaUp = false;
            var eco = MakeEconomy();
            var radio = MakeRadio(eco);
            var sys = MakePlans(eco, radio);
            int intercepts = 0;
            radio.OnIntercept += _ => intercepts++;
            int offered = 0;
            sys.OnInterceptOffered += (_, __) => offered++;

            var plan = sys.SchedulePlan(
                FactionSO.Ids.ScavengerCamp,
                FactionSO.Ids.DoomsdayPreppers,
                scheduleDay: _day);

            Assert.IsNotNull(plan);
            Assert.IsFalse(plan.InterceptPresented);
            Assert.That(intercepts, Is.EqualTo(0));
            Assert.That(offered, Is.EqualTo(0));
            Assert.IsNull(sys.ActiveInterceptPlan);

            radio.Unbind();
        }

        [Test]
        public void WarnTarget_BoostsTargetTrust_AngersAttacker_CancelsRaid()
        {
            var eco = MakeEconomy();
            eco.SetTrust(FactionSO.Ids.ScavengerCamp, 0f);
            eco.SetTrust(FactionSO.Ids.MilitaryRemnants, 0f);
            var radio = MakeRadio(eco);
            var sys = MakePlans(eco, radio);

            var plan = sys.SchedulePlan(
                FactionSO.Ids.MilitaryRemnants,
                FactionSO.Ids.ScavengerCamp,
                scheduleDay: _day,
                leadDays: 2);

            Assert.IsTrue(sys.ApplyChoice(plan.Id, FactionRaidPlanSystem.ChoiceWarnTarget));
            Assert.IsTrue(plan.Resolved);
            Assert.IsFalse(plan.PlanSucceeded);
            Assert.IsTrue(plan.PlayerWarned);
            Assert.That(eco.GetTrust(FactionSO.Ids.ScavengerCamp),
                Is.EqualTo(FactionRaidPlanSystem.WarnTargetTrustDelta).Within(Eps));
            Assert.That(eco.GetTrust(FactionSO.Ids.MilitaryRemnants),
                Is.EqualTo(FactionRaidPlanSystem.AttackerAngerTrustDelta).Within(Eps));

            // Fire day must not re-resolve.
            _day = plan.FireDay;
            sys.TickDay(_day);
            Assert.IsTrue(plan.Resolved);
            Assert.IsFalse(plan.PlanSucceeded);

            radio.Unbind();
        }

        [Test]
        public void ScavengeBattlefield_SpawnsHighRadLootNode_Claimable()
        {
            var eco = MakeEconomy();
            var radio = MakeRadio(eco);
            var map = MakeTinyMap();
            var needsProfile = ScriptableObject.CreateInstance<NeedsProfile>();
            _toDestroy.Add(needsProfile);
            var needs = new NeedsSystem(needsProfile);
            var radiation = new RadiationSystem(needs);
            var scav = MakeSurvivor();
            needs.Register(scav);
            radiation.Register(scav);

            var sys = MakePlans(eco, radio, map, radiation: radiation);

            var plan = sys.SchedulePlan(
                FactionSO.Ids.ScavengerCamp,
                FactionSO.Ids.DoomsdayPreppers,
                scheduleDay: _day,
                leadDays: 0); // fire immediately on scavenge resolve

            Assert.IsTrue(sys.ApplyChoice(plan.Id, FactionRaidPlanSystem.ChoiceScavenge));
            Assert.IsTrue(plan.Resolved);
            Assert.IsTrue(plan.PlanSucceeded);
            Assert.IsTrue(plan.PlayerScavenged);
            Assert.IsTrue(plan.BattlefieldLootReady);
            Assert.That(plan.BattlefieldNodeId, Is.Not.Empty);
            Assert.IsTrue(sys.HasBattlefieldLootAt(plan.BattlefieldNodeId));

            var node = map.GetNode(plan.BattlefieldNodeId);
            Assert.IsNotNull(node);
            Assert.That(node.TrueRad, Is.GreaterThanOrEqualTo(FactionRaidPlanSystem.BattlefieldRadFloor));
            Assert.IsTrue(node.IsRevealed);

            var inv = new Inventory { Capacity = 50, MaxWeight = 200f };
            var weapon = FactionRaidPlanSystem.CreateDefaultWeaponDef();
            var scrap = FactionRaidPlanSystem.CreateDefaultScrapDef();
            _toDestroy.Add(weapon);
            _toDestroy.Add(scrap);

            float radBefore = scav.RadiationDose;
            float lifeBefore = scav.LifetimeRadiationExposure;
            Assert.IsTrue(sys.TryClaimBattlefieldLoot(
                plan.BattlefieldNodeId, scav, inv, weapon, scrap, radDoseOnClaim: 12f));
            Assert.That(inv.Count(weapon), Is.EqualTo(FactionRaidPlanSystem.BattlefieldWeaponLoot));
            Assert.That(inv.Count(scrap), Is.EqualTo(FactionRaidPlanSystem.BattlefieldScrapLoot));
            // MISC-007 — claim spike goes through RadiationSystem.Expose.
            Assert.That(scav.RadiationDose, Is.EqualTo(radBefore + 12f).Within(Eps));
            Assert.That(scav.LifetimeRadiationExposure, Is.EqualTo(lifeBefore + 12f).Within(Eps));
            Assert.IsTrue(plan.BattlefieldLootClaimed);
            Assert.IsFalse(sys.HasBattlefieldLootAt(plan.BattlefieldNodeId));
            // Second claim fails.
            Assert.IsFalse(sys.TryClaimBattlefieldLoot(plan.BattlefieldNodeId, scav, inv, weapon, scrap));

            radio.Unbind();
        }

        [Test]
        public void DoNothing_OnFireDay_PlanSucceeds()
        {
            var eco = MakeEconomy();
            var radio = MakeRadio(eco);
            var sys = MakePlans(eco, radio);

            var plan = sys.SchedulePlan(
                FactionSO.Ids.DoomsdayPreppers,
                FactionSO.Ids.MilitaryRemnants,
                scheduleDay: _day,
                leadDays: 1);

            Assert.IsTrue(sys.ApplyChoice(plan.Id, FactionRaidPlanSystem.ChoiceDoNothing));
            Assert.IsFalse(plan.Resolved, "Do-nothing waits for fire day");

            _day = plan.FireDay;
            sys.TickDay(_day);
            Assert.IsTrue(plan.Resolved);
            Assert.IsTrue(plan.PlanSucceeded);

            radio.Unbind();
        }

        [Test]
        public void SilentPlan_WithoutAntenna_StillResolvesOnFireDay()
        {
            _antennaUp = false;
            var eco = MakeEconomy();
            var radio = MakeRadio(eco);
            var sys = MakePlans(eco, radio);

            var plan = sys.SchedulePlan(
                FactionSO.Ids.CultOfTheGlow,
                FactionSO.Ids.ScavengerCamp,
                scheduleDay: _day,
                leadDays: 1);

            Assert.IsFalse(plan.InterceptPresented);
            _day = plan.FireDay;
            sys.TickDay(_day);
            Assert.IsTrue(plan.Resolved);
            Assert.IsTrue(plan.PlanSucceeded);

            radio.Unbind();
        }

        [Test]
        public void TickDay_BeforeMinDay_NeverAutoSchedules()
        {
            _day = 10;
            var eco = MakeEconomy();
            var radio = MakeRadio(eco);
            var sys = MakePlans(eco, radio, rng: new AlwaysScheduleRng());

            sys.TickDay(_day);
            Assert.That(sys.Plans.Count, Is.EqualTo(0));
            radio.Unbind();
        }

        [Test]
        public void TickDay_AfterMinDay_WithAlwaysRng_SchedulesPlan()
        {
            _day = FactionRaidPlanSystem.MinDayForPlans;
            var eco = MakeEconomy();
            var radio = MakeRadio(eco);
            var sys = MakePlans(eco, radio, rng: new AlwaysScheduleRng());

            sys.TickDay(_day);
            Assert.That(sys.Plans.Count, Is.EqualTo(1));
            Assert.IsTrue(sys.Plans[0].InterceptPresented);
            radio.Unbind();
        }

        [Test]
        public void CultAttacker_InterceptMentionsGlowbandChannel()
        {
            var eco = MakeEconomy();
            var radio = MakeRadio(eco);
            var sys = MakePlans(eco, radio);
            FactionRadioInterceptSystem.InterceptEntry last = null;
            radio.OnIntercept += e => last = e;

            sys.SchedulePlan(
                FactionSO.Ids.CultOfTheGlow,
                FactionSO.Ids.MilitaryRemnants,
                scheduleDay: _day);

            Assert.IsNotNull(last);
            Assert.That(last.Message, Does.Contain("GLOWBAND").IgnoreCase
                .Or.Contain("CH-13"));
            radio.Unbind();
        }

        [Test]
        public void SaveLoad_RoundTripsPendingPlanAndBattlefield()
        {
            var eco = MakeEconomy();
            var radio = MakeRadio(eco);
            var map = MakeTinyMap();
            var sys = MakePlans(eco, radio, map);

            var plan = sys.SchedulePlan(
                FactionSO.Ids.MilitaryRemnants,
                FactionSO.Ids.ScavengerCamp,
                scheduleDay: _day,
                leadDays: 0);
            sys.ApplyChoice(plan.Id, FactionRaidPlanSystem.ChoiceScavenge);
            Assert.IsTrue(plan.BattlefieldLootReady);
            string nodeId = plan.BattlefieldNodeId;

            var snap = sys.CaptureState();
            var sys2 = MakePlans(eco, radio, map);
            sys2.RestoreState(snap);

            Assert.That(sys2.Plans.Count, Is.EqualTo(1));
            var p2 = sys2.Plans[0];
            Assert.That(p2.Id, Is.EqualTo(plan.Id));
            Assert.IsTrue(p2.Resolved);
            Assert.IsTrue(p2.PlayerScavenged);
            Assert.That(p2.BattlefieldNodeId, Is.EqualTo(nodeId));
            Assert.IsTrue(sys2.HasBattlefieldLootAt(nodeId));

            radio.Unbind();
        }

        [Test]
        public void ApplyChoiceFromEvent_UsesEventIdPlanLink()
        {
            var eco = MakeEconomy();
            var radio = MakeRadio(eco);
            var sys = MakePlans(eco, radio);

            var plan = sys.SchedulePlan(
                FactionSO.Ids.ScavengerCamp,
                FactionSO.Ids.MilitaryRemnants,
                scheduleDay: _day,
                leadDays: 3);
            var ev = sys.CreateInterceptEvent(plan);
            _toDestroy.Add(ev);

            var warn = ev.choices.Find(c => c.ChoiceId == FactionRaidPlanSystem.ChoiceWarnTarget);
            Assert.IsTrue(sys.ApplyChoiceFromEvent(ev, warn));
            Assert.IsTrue(plan.PlayerWarned);
            Assert.IsTrue(plan.Resolved);

            radio.Unbind();
        }

        [Test]
        public void AntennaDead_AfterEmp_CannotHearNewPlans()
        {
            var eco = MakeEconomy();
            var radio = MakeRadio(eco);
            var sys = MakePlans(eco, radio);

            _antennaUp = true;
            var heard = sys.SchedulePlan(
                FactionSO.Ids.MilitaryRemnants,
                FactionSO.Ids.DoomsdayPreppers,
                scheduleDay: _day);
            Assert.IsTrue(heard.InterceptPresented);

            // Resolve so a new plan can be scheduled
            sys.ApplyChoice(heard.Id, FactionRaidPlanSystem.ChoiceWarnTarget);

            _antennaUp = false; // EMP-killed radio
            var silent = sys.SchedulePlan(
                FactionSO.Ids.ScavengerCamp,
                FactionSO.Ids.MilitaryRemnants,
                scheduleDay: _day);
            Assert.IsFalse(silent.InterceptPresented);

            radio.Unbind();
        }
    }
}
