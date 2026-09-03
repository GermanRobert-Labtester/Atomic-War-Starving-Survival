using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// F1–F4 — the structured resolution payload. Narrative Core returns the
    /// full consequence metadata so the Host never re-reads choice JSON.
    /// </summary>
    public class NarrativeEncounterResolutionResultTests
    {
        private static EncounterDefinition Def(
            string id, string choiceId, int morale = 0, int guilt = 0,
            string grantItemId = "", int grantQty = 0, string journal = "",
            string discover = "", string flag = "", bool depletes = false)
        {
            return new EncounterDefinition
            {
                id = id,
                title = id,
                category = "Discovery",
                baseWeight = 1f,
                choices = new List<EncounterChoiceDefinition>
                {
                    new EncounterChoiceDefinition
                    {
                        choiceId = choiceId,
                        text = "Go",
                        moraleDelta = morale,
                        guiltDelta = guilt,
                        grantItemId = grantItemId,
                        grantItemQuantity = grantQty,
                        journalUnlockId = journal,
                        discoverLocationId = discover,
                        setWorldFlag = flag,
                        depletesOnResolve = depletes
                    }
                }
            };
        }

        [Fact]
        public void ItemGrant_MetadataReturned_Positive()
        {
            var sys = new NarrativeEncounterSystem();
            sys.RegisterEncounter(Def("micro_crashed_truck", "search_truck_cargo",
                grantItemId: "canned_food", grantQty: 2, depletes: true));

            var r = sys.TryResolve("micro_crashed_truck", "search_truck_cargo", "loc_x", 4);

            Assert.NotNull(r);
            Assert.Equal("canned_food", r!.GrantItemId);
            Assert.Equal(2, r.GrantItemQuantity);
            Assert.True(r.DepletesEncounter);
        }

        [Fact]
        public void ItemOffering_MetadataReturned_Negative()
        {
            var sys = new NarrativeEncounterSystem();
            sys.RegisterEncounter(Def("micro_shrine", "add_shrine_offering",
                grantItemId: "canned_food", grantQty: -1));

            var r = sys.TryResolve("micro_shrine", "add_shrine_offering", "loc_x", 5);

            Assert.NotNull(r);
            Assert.Equal("canned_food", r!.GrantItemId);
            Assert.Equal(-1, r.GrantItemQuantity);
            Assert.False(r.DepletesEncounter);
        }

        [Fact]
        public void NoItemEffect_ZeroQuantityEmptyId()
        {
            var sys = new NarrativeEncounterSystem();
            sys.RegisterEncounter(Def("micro_roadside_memorial", "leave_memorial", morale: 1));

            var r = sys.TryResolve("micro_roadside_memorial", "leave_memorial", "loc_x", 1);

            Assert.NotNull(r);
            Assert.Equal(string.Empty, r!.GrantItemId);
            Assert.Equal(0, r.GrantItemQuantity);
        }

        [Fact]
        public void JournalAndLocation_MetadataReturned()
        {
            var sys = new NarrativeEncounterSystem();
            sys.RegisterEncounter(Def("micro_observation_post", "read_grid_references",
                journal: "micro_observation_post_grid", discover: "rural_gas_station"));

            var r = sys.TryResolve("micro_observation_post", "read_grid_references", "loc_x", 6);

            Assert.NotNull(r);
            Assert.Equal("micro_observation_post_grid", r!.JournalUnlockId);
            Assert.Equal("rural_gas_station", r.DiscoverLocationId);
        }

        [Fact]
        public void WorldFlag_MetadataReturned()
        {
            var sys = new NarrativeEncounterSystem();
            sys.RegisterEncounter(Def("micro_abandoned_generator", "mark_generator",
                flag: "micro_generator_marked"));

            var r = sys.TryResolve("micro_abandoned_generator", "mark_generator", "loc_x", 2);

            Assert.NotNull(r);
            Assert.Equal("micro_generator_marked", r!.SetWorldFlagId);
        }

        [Fact]
        public void ResolutionId_DeterministicText_NotRuntimeHash()
        {
            var sys = new NarrativeEncounterSystem();
            sys.RegisterEncounter(Def("micro_a", "c1", grantItemId: "cloth", grantQty: 1));

            var r1 = sys.TryResolve("micro_a", "c1", "loc_x", 3);
            var r2 = sys.TryResolve("micro_a", "c1", "loc_x", 3);

            Assert.NotNull(r1);
            Assert.NotNull(r2);
            Assert.Equal("micro_a:c1:3:loc_x", r1!.ResolutionId);
            Assert.Equal(r1.ResolutionId, r2!.ResolutionId);
        }

        [Fact]
        public void SameEncounterSameChoice_DeterministicPayload()
        {
            // Same catalog + same input ⇒ byte-identical consequence payloads,
            // independent of resolution order or day (plan §20/§46).
            Func<NarrativeEncounterResolutionResult?> resolve = () =>
            {
                var sys = new NarrativeEncounterSystem();
                sys.RegisterEncounter(Def("micro_b", "c2", morale: 2, guilt: 1,
                    grantItemId: "fuel", grantQty: 2, journal: "micro_b_journal",
                    discover: "government_bunker", depletes: true));
                return sys.TryResolve("micro_b", "c2", "loc_y", 9);
            };

            var a = resolve();
            var b = resolve();

            Assert.NotNull(a);
            Assert.NotNull(b);
            Assert.Equal(a!.EncounterId, b!.EncounterId);
            Assert.Equal(a.ChoiceId, b.ChoiceId);
            Assert.Equal(a.MoraleDelta, b.MoraleDelta);
            Assert.Equal(a.GuiltDelta, b.GuiltDelta);
            Assert.Equal(a.DepletesEncounter, b.DepletesEncounter);
            Assert.Equal(a.GrantItemId, b.GrantItemId);
            Assert.Equal(a.GrantItemQuantity, b.GrantItemQuantity);
            Assert.Equal(a.JournalUnlockId, b.JournalUnlockId);
            Assert.Equal(a.DiscoverLocationId, b.DiscoverLocationId);
        }
    }
}
