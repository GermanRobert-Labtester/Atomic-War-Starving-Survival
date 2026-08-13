using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.Survivors;
using InventoryClass = AtomicWar._Game.Inventory.Inventory;
using Ashfall.Core.Journal;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompts #182–#188 — combat milestone perks earned through bloodshed,
    /// jams, flees, traps, and wasteland ambushes.
    /// </summary>
    [TestFixture]
    public class CombatPerkTests
    {
        private SkillProgressionSystem _progression;
        private CombatPerkSystem _perks;
        private Survivor _sv;
        private Survivor _empath;

        [SetUp]
        public void SetUp()
        {
            _progression = new SkillProgressionSystem();
            _progression.RegisterDefaultPerks();
            _perks = new CombatPerkSystem();
            _perks.Bind(_progression);

            _sv = MakeSurvivor("sv_raider", "Raider");
            _empath = MakeSurvivor("sv_empath", "Soft");
            _empath.RiskBias = RiskBiasTrait.Empath;
        }

        private static Survivor MakeSurvivor(string id, string name)
        {
            var sv = new Survivor
            {
                Id = id,
                DisplayName = name,
                State = SurvivorState.Idle
            };
            sv.Needs.Morale = 70f;
            sv.Needs.Health = 100f;
            return sv;
        }

        // ── #182 Tap-Rack-Bang ───────────────────────────────────────────

        [Test]
        public void TapRackBang_EarnedAfterThreeJams_ClearsInOneTick()
        {
            Assert.AreEqual(CombatPerkSystem.DefaultJamClearTicks, _perks.GetJamClearTicks(_sv));

            _perks.RecordWeaponJamSurvived(_sv, 1);
            _perks.RecordWeaponJamSurvived(_sv, 1);
            Assert.IsFalse(_perks.Has(_sv, CombatPerkSystem.TapRackBangId));

            _perks.RecordWeaponJamSurvived(_sv, 1);
            Assert.IsTrue(_perks.Has(_sv, CombatPerkSystem.TapRackBangId));
            Assert.AreEqual(CombatPerkSystem.TapRackBangJamClearTicks, _perks.GetJamClearTicks(_sv));
            Assert.AreEqual(3, _perks.GetCounters(_sv.Id).JamsSurvived);
        }

        [Test]
        public void WeaponMaintenance_StartJam_RespectsClearTicks()
        {
            var maint = new WeaponMaintenanceSystem();
            maint.Fire("rifle"); // degrade a bit
            // Force below jam threshold
            for (int i = 0; i < 40; i++) maint.Fire("rifle");

            Assert.IsTrue(maint.CanJam("rifle"));
            Assert.IsTrue(maint.TryJam("rifle", clearTicks: 1, chanceWhenEligible: 1f));
            Assert.IsTrue(maint.IsJammed("rifle"));
            Assert.AreEqual(1, maint.GetJamTicksRemaining("rifle"));
            Assert.IsTrue(maint.TickJamClear("rifle"), "1-tick jam clears on first tick");
            Assert.IsFalse(maint.IsJammed("rifle"));
        }

        // ── #183 Cold Bore ───────────────────────────────────────────────

        [Test]
        public void ColdBore_EarnedOnStealthKill_FirstShotCritsAtFiftyPercent()
        {
            _perks.RecordStealthKill(_sv, 2);
            Assert.IsTrue(_perks.Has(_sv, CombatPerkSystem.ColdBoreId));

            // Fixed RNG: NextDouble always 0.0 → always crit when chance > 0
            var alwaysLow = new FixedRandom(0.0);
            Assert.IsTrue(_perks.RollFirstShotCrit(_sv, "enc_a", alwaysLow, baseCritChance: 0f));

            // Second shot same encounter: no Cold Bore bonus → base 0 → no crit
            Assert.IsFalse(_perks.RollFirstShotCrit(_sv, "enc_a", alwaysLow, baseCritChance: 0f));

            // New encounter: first shot again
            Assert.IsTrue(_perks.RollFirstShotCrit(_sv, "enc_b", alwaysLow, baseCritChance: 0f));
        }

        [Test]
        public void ColdBore_WithoutPerk_NoFirstShotBonus()
        {
            var alwaysLow = new FixedRandom(0.0);
            // base 0, no perk → never crit even with "low" roll under threshold 0
            // NextDouble 0.0 < 0.0 is false
            Assert.IsFalse(_perks.RollFirstShotCrit(_sv, "enc_x", alwaysLow, baseCritChance: 0f));
        }

        // ── #184 Suppressing Fire ────────────────────────────────────────

        [Test]
        public void SuppressingFire_EarnedAfterFiftyAmmo_HaltsRaidTwoHours()
        {
            Assert.IsFalse(_perks.CanUseSuppressingFire(_sv));
            _perks.RecordAmmoExpended(_sv, 49, 3);
            Assert.IsFalse(_perks.CanUseSuppressingFire(_sv));
            _perks.RecordAmmoExpended(_sv, 1, 3);
            Assert.IsTrue(_perks.CanUseSuppressingFire(_sv));
            Assert.IsTrue(_perks.Has(_sv, CombatPerkSystem.SuppressingFireId));

            var hatch = new HatchDefenseSystem(rng: new System.Random(1));
            hatch.ApplySuppressingFireHalt(CombatPerkSystem.SuppressingFireHaltHours);
            Assert.IsTrue(hatch.IsRaidHalted);
            Assert.AreEqual(2f, hatch.RaidHaltHoursRemaining, 0.001f);

            hatch.TickRaidHalt(1f);
            Assert.AreEqual(1f, hatch.RaidHaltHoursRemaining, 0.001f);
            hatch.TickRaidHalt(1.5f);
            Assert.IsFalse(hatch.IsRaidHalted);
        }

        [Test]
        public void SuppressingFireAction_RequiresPerkAndAmmo()
        {
            var action = ScriptableObject.CreateInstance<SuppressingFireActionSO>();
            var inv = new InventoryClass();
            var ammo = ScriptableObject.CreateInstance<ItemDefinition>();
            ammo.id = "handgun_ammo";
            ammo.displayName = "Handgun Ammo";
            ammo.stackMax = 100;
            ammo.type = ItemType.Weapon;
            inv.Add(ammo, 10);

            var hatch = new HatchDefenseSystem(rng: new System.Random(2));
            hatch.SecurityOverride = 10f; // low security → threat

            var ctx = new AIContext
            {
                Survivor = _sv,
                Inventory = inv,
                CombatPerks = _perks,
                HatchDefense = hatch,
                CurrentDay = HatchDefenseSystem.RaidUnlockDay + 1,
                RaidThreatLevel = 0.5f
            };

            Assert.AreEqual(0f, action.EvaluateRaw(ctx), "No perk → score 0");

            _perks.RecordAmmoExpended(_sv, 50, 1);
            Assert.Greater(action.EvaluateRaw(ctx), 0f, "With perk + ammo + threat → scores");

            action.Execute(ctx);
            Assert.IsTrue(hatch.IsRaidHalted);
            Assert.AreEqual(5, SuppressingFireActionSO.CountAmmo(inv), "Spent 5 of 10 rounds");
        }

        // ── #185 Close Quarters ──────────────────────────────────────────

        [Test]
        public void CloseQuarters_EarnedAfterThreeConfined_DoublesDamage()
        {
            Assert.AreEqual(1f, _perks.GetCloseQuartersDamageMultiplier(_sv, true));

            _perks.RecordConfinedEncounterSurvived(_sv, 1);
            _perks.RecordConfinedEncounterSurvived(_sv, 1);
            _perks.RecordConfinedEncounterSurvived(_sv, 1);
            Assert.IsTrue(_perks.Has(_sv, CombatPerkSystem.CloseQuartersId));
            Assert.AreEqual(
                CombatPerkSystem.CloseQuartersDamageMultiplier,
                _perks.GetCloseQuartersDamageMultiplier(_sv, confinedOrBreach: true),
                0.001f);
            Assert.AreEqual(1f, _perks.GetCloseQuartersDamageMultiplier(_sv, confinedOrBreach: false));
        }

        [Test]
        public void CloseQuarters_TagHelpers_DetectUrbanSubwayConfined()
        {
            var tags = new List<string> { "urban", "scavenge" };
            Assert.IsTrue(CombatPerkSystem.IsUrbanOrSubwayTags(tags));
            Assert.IsTrue(CombatPerkSystem.IsConfinedNodeTags(tags));

            var subway = new List<string> { "subway" };
            Assert.IsTrue(CombatPerkSystem.IsUrbanOrSubwayTags(subway));

            var open = new List<string> { "rural" };
            Assert.IsFalse(CombatPerkSystem.IsUrbanOrSubwayTags(open));
            Assert.IsFalse(CombatPerkSystem.IsConfinedNodeTags(open));

            var confined = new List<string> { "confined_space" };
            Assert.IsTrue(CombatPerkSystem.IsConfinedNodeTags(confined));
            Assert.IsFalse(CombatPerkSystem.IsUrbanOrSubwayTags(confined));
        }

        // ── #186 Trap Setter ─────────────────────────────────────────────

        [Test]
        public void TrapSetter_TenTraps_NoMisfireDoubleDamagePerfectDisarm()
        {
            var traps = new PerimeterTrapSystem();
            traps.BindCombatPerks(_perks, id => id == _sv.Id ? _sv : null);
            traps.SetRng(new System.Random(99));

            for (int i = 0; i < 9; i++)
                traps.DeployTrap(PerimeterTrapSystem.BearTrapItemId, _sv, 1, currentDay: 1);
            Assert.IsFalse(_perks.Has(_sv, CombatPerkSystem.TrapSetterId));

            traps.DeployTrap(PerimeterTrapSystem.BearTrapItemId, _sv, 1, currentDay: 1);
            Assert.IsTrue(_perks.Has(_sv, CombatPerkSystem.TrapSetterId));
            Assert.AreEqual(0f, traps.GetMisfireChance(), 0.001f);
            Assert.IsFalse(traps.RollPrematureMisfire());

            float dmg = traps.GetTrapDamageAgainstRaiders();
            // 10 bear traps * base * 2x
            Assert.AreEqual(
                PerimeterTrapSystem.BaseTrapDamage * 10 * CombatPerkSystem.TrapSetterDamageMultiplier,
                dmg, 0.001f);

            Assert.IsTrue(traps.TryDisarmWastelandTrap(_sv));
            Assert.AreEqual(1f, _perks.GetDisarmSuccessRate(_sv), 0.001f);
        }

        // ── #187 Looter's Reflex ─────────────────────────────────────────

        [Test]
        public void LootersReflex_ThreeFlees_KeepsMostValuableItem()
        {
            _perks.RecordFlee(_sv, 1);
            _perks.RecordFlee(_sv, 1);
            Assert.IsFalse(_perks.Has(_sv, CombatPerkSystem.LootersReflexId));
            _perks.RecordFlee(_sv, 1);
            Assert.IsTrue(_perks.Has(_sv, CombatPerkSystem.LootersReflexId));

            // trade values: 1, 50, 10 → keep index 1
            int retainedIdx = -1;
            _perks.OnLootRetainedOnFlee += (sv, idx) => retainedIdx = idx;
            var drop = _perks.ComputeFleeDropIndices(
                _sv, 3,
                i => i == 0 ? 1f : i == 1 ? 50f : 10f,
                i => 1f);
            Assert.AreEqual(2, drop.Count);
            Assert.IsFalse(drop.Contains(1), "Most valuable index retained");
            Assert.IsTrue(drop.Contains(0));
            Assert.IsTrue(drop.Contains(2));
            Assert.AreEqual(1, retainedIdx, "OnLootRetainedOnFlee fires with keep index");
        }

        [Test]
        public void LootersReflex_WithoutPerk_DropsFractionalFromEnd()
        {
            var drop = _perks.ComputeFleeDropIndices(
                _sv, 4,
                i => 1f, i => 1f,
                defaultDropFraction: 0.5f);
            // ceil(4 * 0.5) = 2 from end → indices 3, 2
            Assert.AreEqual(2, drop.Count);
            Assert.AreEqual(3, drop[0]);
            Assert.AreEqual(2, drop[1]);
        }

        [Test]
        public void FindMostValuableOrHeavy_PrefersValueThenWeight()
        {
            // equal value → heavier wins
            int idx = CombatPerkSystem.FindMostValuableOrHeavyIndex(
                3,
                i => 10f,
                i => i == 2 ? 9f : 1f);
            Assert.AreEqual(2, idx);
        }

        // ── #188 Desensitized ────────────────────────────────────────────

        [Test]
        public void Desensitized_FiveKills_RemovesMoralePenalty_DropsEmpathAffinity()
        {
            var affinity = new InterpersonalAffinity();
            affinity.Set(_sv.Id, _empath.Id, 20f);
            var all = new List<Survivor> { _sv, _empath };

            for (int i = 0; i < 4; i++)
                _perks.RecordHumanKill(_sv, currentDay: 5, allSurvivors: all, affinity: affinity);
            Assert.IsFalse(_perks.Has(_sv, CombatPerkSystem.DesensitizedId));

            float moraleBefore = _sv.Needs.Morale;
            float applied = _perks.ApplyHumanKillMorale(_sv);
            Assert.Less(applied, 0f, "Still takes kill morale before perk");
            Assert.Less(_sv.Needs.Morale, moraleBefore);

            _perks.RecordHumanKill(_sv, currentDay: 5, allSurvivors: all, affinity: affinity);
            Assert.IsTrue(_perks.Has(_sv, CombatPerkSystem.DesensitizedId));
            Assert.AreEqual(
                20f + CombatPerkSystem.DesensitizedEmpathAffinityDrop,
                affinity.Get(_sv.Id, _empath.Id),
                0.001f);

            _sv.Needs.Morale = 70f;
            Assert.AreEqual(0f, _perks.ApplyHumanKillMorale(_sv));
            Assert.AreEqual(70f, _sv.Needs.Morale, 0.001f);
            Assert.IsTrue(_perks.IsImmuneToKillMorale(_sv));
            Assert.IsTrue(_perks.IsImmuneToCorpseMorale(_sv));
        }

        // ── Save / load ──────────────────────────────────────────────────

        [Test]
        public void CombatPerkCounters_RoundTripSave()
        {
            _perks.RecordWeaponJamSurvived(_sv, 1);
            _perks.RecordWeaponJamSurvived(_sv, 1);
            _perks.RecordAmmoExpended(_sv, 12, 1);
            _perks.RecordFlee(_sv, 1);

            var save = _perks.CaptureState();
            var restored = new CombatPerkSystem();
            restored.Bind(_progression);
            restored.RestoreState(save);

            var c = restored.GetCounters(_sv.Id);
            Assert.AreEqual(2, c.JamsSurvived);
            Assert.AreEqual(12, c.AmmoExpended);
            Assert.AreEqual(1, c.Flees);
        }

        [Test]
        public void MilestonePerks_NotEarnableViaXpGrind()
        {
            // Even huge combat XP must not grant Tap-Rack-Bang (threshold 999999).
            for (int i = 0; i < 200; i++)
                _progression.RecordAction(_sv, "combat", 100f, currentDay: 1);

            Assert.IsFalse(_progression.HasActivePerk(_sv.Id, CombatPerkSystem.TapRackBangId));
            Assert.IsFalse(_progression.HasActivePerk(_sv.Id, CombatPerkSystem.ColdBoreId));
            Assert.IsFalse(_progression.HasActivePerk(_sv.Id, CombatPerkSystem.DesensitizedId));
        }

        /// <summary>Deterministic Random that always returns a fixed NextDouble.</summary>
        private sealed class FixedRandom : System.Random
        {
            private readonly double _value;
            public FixedRandom(double value) => _value = value;
            public override double NextDouble() => _value;
            public override int Next() => 0;
            public override int Next(int maxValue) => 0;
            public override int Next(int minValue, int maxValue) => minValue;
        }
    }
}
