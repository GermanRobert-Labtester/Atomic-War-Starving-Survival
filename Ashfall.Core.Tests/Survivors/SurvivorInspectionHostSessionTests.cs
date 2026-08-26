using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radiation;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public class SurvivorInspectionHostSessionTests
    {
        private static (SurvivorInspectionHostSession host,
            NeedsSystem needs, RadiationSystem radiation,
            Ashfall.Core.Inventory.Inventory inventory,
            SurvivorNeedsState ns, SurvivorRadState rs)
            MakeRig(int foodUnits = 10, int waterUnits = 10, int bandages = 5,
                int iodide = 3, int antiRad = 2)
        {
            var ns = new SurvivorNeedsState { Id = "elena_vasquez", Hunger = 80, Thirst = 80, Health = 90 };
            var needs = new NeedsSystem();
            needs.Register(ns);
            var rs = new SurvivorRadState { Id = "elena_vasquez", RadiationDose = 25 };
            var radiation = new RadiationSystem(
                exposureContext: _ => new ExposureContext(),
                applyNeed: (_, _, _) => { });
            radiation.Register(rs);
            var inv = new Ashfall.Core.Inventory.Inventory { Capacity = 20, MaxWeight = 200f };
            void AddMany(string id, int n, ItemType type)
            {
                if (n <= 0) return;
                var def = new ItemDefinition
                {
                    id = id,
                    displayName = id,
                    stackMax = 100,
                    weight = 0.1f,
                    type = type
                };
                inv.Add(def, n);
            }
            AddMany("canned_food", foodUnits, ItemType.Food);
            AddMany("clean_water", waterUnits, ItemType.Water);
            AddMany("bandage", bandages, ItemType.Medical);
            AddMany("potassium_iodide", iodide, ItemType.Iodine);
            AddMany("anti_rad", antiRad, ItemType.AntiRad);
            var host = new SurvivorInspectionHostSession(needs, radiation, inv,
                resolveNeeds: id => id == ns.Id ? ns : null,
                resolveRad: id => id == rs.Id ? rs : null);
            return (host, needs, radiation, inv, ns, rs);
        }

        [Fact]
        public void Inspect_ReturnsLiveSnapshot()
        {
            var (host, _, _, _, _, _) = MakeRig();
            var s = host.Inspect("elena_vasquez");
            Assert.True(s.IsAlive);
            Assert.Equal(80f, s.Hunger);
            Assert.Equal(80f, s.Thirst);
            Assert.Equal(90f, s.Health);
            Assert.Equal(25f, s.RadiationDose);
        }

        [Fact]
        public void Inspect_UnknownSurvivor_ReturnsEmpty()
        {
            var (host, _, _, _, _, _) = MakeRig();
            var s = host.Inspect("ghost");
            Assert.False(s.IsAlive);
        }

        [Fact]
        public void Feed_ReducesHungerAndConsumesItem()
        {
            var (host, _, _, _, ns, _) = MakeRig();
            var result = host.Feed("elena_vasquez", 2);
            Assert.True(result.Succeeded);
            Assert.Equal(2, result.IntDeltas["food_consumed"]);
            Assert.True(ns.Hunger < 80f);
        }

        [Fact]
        public void Feed_NoInventory_FailsWithStableCode()
        {
            var (host, _, _, _, _, _) = MakeRig(foodUnits: 0);
            var result = host.Feed("elena_vasquez", 2);
            Assert.False(result.Succeeded);
            Assert.Equal("insufficient_food", result.ReasonCode);
        }

        [Fact]
        public void Feed_UnknownSurvivor_Fails()
        {
            var (host, _, _, _, _, _) = MakeRig();
            var result = host.Feed("ghost", 1);
            Assert.False(result.Succeeded);
            // Either missing_survivor_id or survivor_unavailable — both are stable failure codes.
            Assert.Contains(result.ReasonCode,
                new[] { "missing_survivor_id", "survivor_unavailable" });
        }

        [Fact]
        public void Feed_DeadSurvivor_Fails()
        {
            var (host, _, _, _, ns, _) = MakeRig();
            ns.IsAlive = false;
            ns.IsDead = true;
            var result = host.Feed("elena_vasquez", 1);
            Assert.False(result.Succeeded);
            Assert.Equal("survivor_unavailable", result.ReasonCode);
        }

        [Fact]
        public void Drink_ConsumesWater()
        {
            var (host, _, _, inv, ns, _) = MakeRig(waterUnits: 5);
            int before = inv.CountById("clean_water");
            var result = host.Drink("elena_vasquez", 3);
            Assert.True(result.Succeeded);
            Assert.Equal(before - 3, inv.CountById("clean_water"));
            Assert.True(ns.Thirst < 80f);
        }

        [Fact]
        public void Rest_ReducesFatigue()
        {
            var (host, _, _, _, ns, _) = MakeRig();
            ns.Fatigue = 50f;
            var result = host.AssignRest("elena_vasquez", 4f);
            Assert.True(result.Succeeded);
            Assert.Equal(4f, result.FloatDeltas["rest_hours"]);
            Assert.True(ns.Fatigue < 50f);
        }

        [Fact]
        public void Rest_RejectsOutOfRangeHours()
        {
            var (host, _, _, _, _, _) = MakeRig();
            Assert.False(host.AssignRest("elena_vasquez", 0f).Succeeded);
            Assert.False(host.AssignRest("elena_vasquez", 24f).Succeeded);
        }

        [Fact]
        public void Bandage_ConsumesBandagesAndRestoresHealth()
        {
            var (host, _, _, inv, ns, _) = MakeRig(bandages: 4);
            ns.Health = 70f;
            var result = host.Bandage("elena_vasquez", 2);
            Assert.True(result.Succeeded);
            Assert.Equal(2, result.IntDeltas["bandages_used"]);
            Assert.Equal(2, inv.CountById("bandage"));
            Assert.True(ns.Health > 70f);
        }

        [Fact]
        public void TakeIodide_ConsumesExactlyOnePill()
        {
            var (host, _, _, inv, _, rs) = MakeRig(iodide: 1);
            int before = inv.CountById("potassium_iodide");
            float beforeTimer = rs.IodineProtectionTimer;
            var result = host.TakeIodide("elena_vasquez", 1);
            Assert.True(result.Succeeded);
            Assert.Equal(1, result.IntDeltas["pills_consumed"]);
            Assert.Equal(before - 1, inv.CountById("potassium_iodide"));
            Assert.True(rs.IodineProtectionTimer >= beforeTimer);
        }

        [Fact]
        public void TakeIodide_NoPills_Fails()
        {
            var (host, _, _, _, _, _) = MakeRig(iodide: 0);
            var result = host.TakeIodide("elena_vasquez", 1);
            Assert.False(result.Succeeded);
            Assert.Equal("insufficient_iodide", result.ReasonCode);
        }

        [Fact]
        public void TakeAntiRad_ConsumesDoseAndReducesRadiation()
        {
            var (host, _, _, inv, _, rs) = MakeRig(antiRad: 2);
            int before = inv.CountById("anti_rad");
            float beforeDose = rs.RadiationDose;
            var result = host.TakeAntiRad("elena_vasquez", 1);
            Assert.True(result.Succeeded);
            Assert.Equal(1, result.IntDeltas["doses_consumed"]);
            Assert.Equal(before - 1, inv.CountById("anti_rad"));
            Assert.True(rs.RadiationDose < beforeDose);
        }

        [Fact]
        public void Speak_BumpsMorale()
        {
            var (host, _, _, _, ns, _) = MakeRig();
            float before = ns.Morale;
            var result = host.Speak("elena_vasquez", 5f);
            Assert.True(result.Succeeded);
            Assert.True(ns.Morale > before);
        }

        [Fact]
        public void AtomicCommands_RejectEmptyOrNegativeInputs()
        {
            var (host, _, _, _, _, _) = MakeRig();
            Assert.False(host.Feed("elena_vasquez", 0).Succeeded);
            Assert.False(host.Drink("elena_vasquez", -1).Succeeded);
            Assert.False(host.Bandage("elena_vasquez", 0).Succeeded);
            Assert.False(host.TakeIodide("elena_vasquez", -1).Succeeded);
        }

        [Fact]
        public void OnCommandApplied_FiresPerCommand()
        {
            var (host, _, _, _, _, _) = MakeRig();
            var fired = new List<string>();
            host.OnCommandApplied += s => fired.Add(s);
            host.Feed("elena_vasquez", 1);
            host.Speak("elena_vasquez");
            Assert.Equal(2, fired.Count);
            Assert.Contains("feed:elena_vasquez", fired);
            Assert.Contains("speak:elena_vasquez", fired);
        }
    }
}
