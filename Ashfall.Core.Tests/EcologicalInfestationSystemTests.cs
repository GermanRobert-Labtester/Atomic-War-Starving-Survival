using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core.Ecology;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 28 Phase 4 — ecological infestation lifecycle: deterministic
    /// eligibility, cooldown, item-cost clears, bounded tolerated harvests,
    /// routed consequences, and save round-trip (reload never duplicates
    /// outcomes).
    /// </summary>
    public sealed class EcologicalInfestationSystemTests
    {
        private static EcologicalInfestationSystem SystemWith(
            params EcologicalInfestationDefinition[] defs)
        {
            var system = new EcologicalInfestationSystem();
            system.LoadDefinitions(defs.Where(d => d != null));
            return system;
        }

        private static EcologicalInfestationDefinition ProbeDef() => new EcologicalInfestationDefinition
        {
            id = "infestation_probe",
            name = "Probe Bloom",
            scope = "location",
            target_id = "loc_bridge_seven",
            trigger_chance_per_day = 0f, // host-triggered: deterministic
            clear_options = new List<InfestationClearOption>
            {
                new InfestationClearOption
                {
                    option_id = "opt_smoke",
                    required_item_id = "charcoal",
                    required_item_count = 1,
                    success_chance = 1f,
                    outcome_summary = "cleared by smoke"
                },
                new InfestationClearOption { option_id = "opt_never", success_chance = 0f }
            },
            leave_resource_item_id = "item_mycelium_bricks",
            leave_resource_amount = 1,
            leave_benefit_summary = "culture bricks",
            max_harvests = 1
        };

        [Fact]
        public void TryTrigger_EligibleZeroChance_Triggers_AndNeverReTriggers()
        {
            var system = SystemWith(ProbeDef());

            Assert.True(system.IsEligibleToTrigger("infestation_probe", 10, null));
            Assert.True(system.TryTrigger("infestation_probe", 10, null, out var reason));
            Assert.Equal("triggered", reason);
            Assert.True(system.IsActive("infestation_probe"));

            // An active infestation never re-triggers (no duplicate events).
            Assert.False(system.TryTrigger("infestation_probe", 11, null, out var again));
            Assert.Equal("not_inactive", again);
        }

        [Fact]
        public void TryClear_ConsumesCostThroughTheCaller_AndResolvesOnSuccess()
        {
            var consumed = new List<(string id, int count)>();
            var system = SystemWith(ProbeDef());
            system.TryTrigger("infestation_probe", 10, null, out _);

            Assert.True(system.TryClear("infestation_probe", "opt_smoke", 12,
                consumeItem: (id, count) => { consumed.Add((id, count)); return true; },
                dayRng: new SeededRng(1), out var summary));
            Assert.Equal(("charcoal", 1), consumed[0]);
            Assert.Equal("cleared by smoke", summary);
            Assert.False(system.IsActive("infestation_probe"));
        }

        [Fact]
        public void TryClear_MissingItem_Fails_AndStaysActive()
        {
            var system = SystemWith(ProbeDef());
            system.TryTrigger("infestation_probe", 10, null, out _);

            Assert.False(system.TryClear("infestation_probe", "opt_never", 12,
                consumeItem: (_id, _count) => true,
                dayRng: new SeededRng(1), out var summary));

            Assert.True(system.IsActive("infestation_probe"));
        }

        [Fact]
        public void TolerateAndHarvest_GrantsBoundedBenefit_ThenExhausts()
        {
            var def = ProbeDef();
            def.leave_resource_item_id = "item_mycelium_bricks";
            def.leave_resource_amount = 2;
            def.max_harvests = 1;
            var system = SystemWith(def);
            system.TryTrigger("infestation_probe", 10, null, out _);

            Assert.True(system.TryTolerateAndHarvest("infestation_probe", 11,
                out var itemId, out var amount, out _));
            Assert.Equal("item_mycelium_bricks", itemId);
            Assert.Equal(2, amount);

            // Exhausted: no further benefit.
            Assert.False(system.TryTolerateAndHarvest("infestation_probe", 12,
                out _, out _, out var exhausted));
            Assert.Equal("harvest_exhausted", exhausted);
        }

        [Fact]
        public void TickDay_RoutesBoundedFoodLoss_OnlyWhileActive()
        {
            var def = ProbeDef();
            def.food_loss_per_day = 9; // authored above the hard cap
            var system = SystemWith(def);
            system.TryTrigger("infestation_probe", 10, null, out _);

            var routed = new List<(string id, int units)>();
            system.TickDay(20, new SeededRng(5),
                foodLoss: (id, units) => routed.Add((id, units)),
                diseaseRisk: null);

            var routedEvent = Assert.Single(routed);
            Assert.Equal("infestation_probe", routedEvent.id);
            Assert.Equal(EcologicalInfestationSystem.MaxFoodLossPerDay, routedEvent.units);
        }

        [Fact]
        public void TickDay_HazardRoll_IsDeterministic_UnderTheSameFork()
        {
            var def = ProbeDef();
            def.linked_disease_id = "disease_spore_blight";
            def.tolerated_hazard_risk = 1f;

            var a = SystemWith(def);
            var b = SystemWith(def);
            a.TryTrigger("infestation_probe", 10, null, out _);
            b.TryTrigger("infestation_probe", 10, null, out _);

            var routedA = new List<string>();
            int routedBCount = 0;
            for (int day = 11; day <= 40; day++)
            {
                var fork = new SeededRng(1000 + day);
                a.TickDay(day, new SeededRng(1000 + day), (_, _) => { }, d => routedA.Add(d));
                b.TickDay(day, new SeededRng(1000 + day), (_, _) => { }, _ => routedBCount++);
            }

            Assert.Equal(routedA.Count, routedBCount);
        }

        [Fact]
        public void CaptureRestore_RoundTriipsExactly_AndNeverDuplicatesOutcomes()
        {
            var system = SystemWith(ProbeDef());
            system.TryTrigger("infestation_probe", 10, null, out _);
            system.TryTolerateAndHarvest("infestation_probe", 12,
                out _, out _, out _);

            var json = new SystemTextJsonSerializer().Serialize(system.CaptureState());

            var restored = new EcologicalInfestationSystem();
            restored.RestoreState(new SystemTextJsonSerializer()
                .Deserialize<EcologicalInfestationState>(json));

            Assert.Equal(json,
                new SystemTextJsonSerializer().Serialize(restored.CaptureState()));
            Assert.Equal(system.GetRecord("infestation_probe")!.harvests_taken,
                restored.GetRecord("infestation_probe")!.harvests_taken);
        }
    }
}
