using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Narrative;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// ExpeditionEncounterBridge + NarrativeEncounterState pending round-trip.
    /// </summary>
    public class ExpeditionEncounterBridgeTests
    {
        private static NarrativeEncounterSystem NewNarrative()
        {
            var sys = new NarrativeEncounterSystem();
            sys.RegisterEncounter(new EncounterDefinition
            {
                id = "enc_dead_letter_office",
                title = "The Dead Letter Office",
                description = "An overturned postal van.",
                category = "Discovery",
                baseWeight = 2.0f,
                stealthWeightMultiplier = 0.5f,
                speedWeightMultiplier = 1.5f,
                minDangerLevel = 0f,
                choices = new List<EncounterChoiceDefinition>
                {
                    new EncounterChoiceDefinition { choiceId = "read", text = "Read", moraleDelta = 1, guiltDelta = 0 }
                }
            });
            sys.RegisterEncounter(new EncounterDefinition
            {
                id = "enc_weather_station",
                title = "Automated Weather Station",
                description = "A pre-war weather station.",
                category = "Discovery",
                baseWeight = 3.0f,
                stealthWeightMultiplier = 0.5f,
                speedWeightMultiplier = 1.5f,
                minDangerLevel = 0f,
                choices = new List<EncounterChoiceDefinition>
                {
                    new EncounterChoiceDefinition { choiceId = "copy", text = "Copy", moraleDelta = 1, guiltDelta = 0 }
                }
            });
            return sys;
        }

        private static ExpeditionState NewState(string locationId = "loc_the_allotments", int dangerLevel = 2)
        {
            return new ExpeditionState
            {
                survivorId = "survivor_gunner_mikhail",
                locationId = locationId,
                displayName = "The Works Allotment Commune",
                stance = "Stealth",
                phase = (int)ExpeditionPhase.Outbound,
                encounterCount = 1,
                dangerLevel = dangerLevel
            };
        }

        [Fact]
        public void Bridge_DeterministicSelection_SameSeedSameSequence()
        {
            var narrative = NewNarrative();
            var rng1 = new SeededRng(42);
            var rng2 = new SeededRng(42);
            var bridge1 = new ExpeditionEncounterBridge(narrative, rng1);
            var bridge2 = new ExpeditionEncounterBridge(narrative, rng2);

            var ids1 = new List<string>();
            var ids2 = new List<string>();
            string? last1 = null;
            string? last2 = null;

            bridge1.OnSurfaced += dto => { last1 = dto.encounter_id; ids1.Add(dto.encounter_id ?? "bare"); };
            bridge2.OnSurfaced += dto => { last2 = dto.encounter_id; ids2.Add(dto.encounter_id ?? "bare"); };

            for (int i = 0; i < 10; i++)
            {
                bridge1.Surface(NewState());
                bridge2.Surface(NewState());
            }

            Assert.Equal(ids1.Count, ids2.Count);
            for (int i = 0; i < ids1.Count; i++)
                Assert.Equal(ids1[i], ids2[i]);
        }

        [Fact]
        public void Bridge_BareNotice_WhenNothingEligible()
        {
            var narrative = new NarrativeEncounterSystem();
            // Register an encounter that requires danger 10 — our state has danger 1.
            narrative.RegisterEncounter(new EncounterDefinition
            {
                id = "enc_hard",
                title = "Hard Encounter",
                description = "Needs danger 10.",
                category = "Hazard",
                baseWeight = 1f,
                minDangerLevel = 10f,
                choices = new List<EncounterChoiceDefinition>()
            });

            var bridge = new ExpeditionEncounterBridge(narrative, new SeededRng(1));
            ExpeditionEncounterBridge.EncounterSurfaced? surfaced = null;
            bridge.OnSurfaced += dto => surfaced = dto;
            bridge.Surface(NewState(dangerLevel: 1));

            Assert.NotNull(surfaced);
            Assert.Null(surfaced!.encounter_id);
            Assert.Equal("Encounter", surfaced!.title);
            Assert.False(string.IsNullOrEmpty(surfaced!.description));
            Assert.Empty(surfaced!.choices);
            Assert.Equal(false, surfaced!.resolved_at_lead);
        }

        [Fact]
        public void Bridge_ResolvedEncounter_ReturnsCatalogText()
        {
            var narrative = NewNarrative();
            var bridge = new ExpeditionEncounterBridge(narrative, new SeededRng(7));
            ExpeditionEncounterBridge.EncounterSurfaced? surfaced = null;
            bridge.OnSurfaced += dto => surfaced = dto;
            bridge.Surface(NewState(dangerLevel: 0));

            Assert.NotNull(surfaced);
            Assert.NotNull(surfaced!.encounter_id);
            Assert.Contains(surfaced!.encounter_id, new[] { "enc_dead_letter_office", "enc_weather_station" });
            Assert.NotEmpty(surfaced!.choices);
            Assert.Null(surfaced!.resolved_at_lead);
        }

        [Fact]
        public void Bridge_ResolveChoice_DelegatesToNarrative()
        {
            var narrative = NewNarrative();
            var bridge = new ExpeditionEncounterBridge(narrative, new SeededRng(7));
            ExpeditionEncounterBridge.EncounterSurfaced? surfaced = null;
            bridge.OnSurfaced += dto => surfaced = dto;
            bridge.Surface(NewState(dangerLevel: 0));

            Assert.NotNull(surfaced);
            string encId = surfaced!.encounter_id;
            string choiceId = surfaced!.choices[0].choiceId;

            bool ok = bridge.ResolveChoice(encId, choiceId, 40);
            Assert.True(ok);
            Assert.Equal(1, narrative.TotalResolved);
            Assert.Equal(40, narrative.State.history[0].day);
        }

        [Fact]
        public void NarrativeEncounterState_RoundTripsPending()
        {
            var sys = NewNarrative();
            sys.Resolve("enc_dead_letter_office", "read", "loc_the_allotments", 40);

            var state = sys.CaptureState();
            state.pending.Add(new PendingSurfacedEncounter
            {
                encounterId = "enc_weather_station",
                locationId = "loc_the_allotments",
                legIndex = 2,
                day = 41
            });

            var restored = new NarrativeEncounterSystem();
            restored.RestoreState(state);

            Assert.Equal(1, restored.TotalResolved);
            Assert.Single(restored.State.pending);
            Assert.Equal("enc_weather_station", restored.State.pending[0].encounterId);
            Assert.Equal(2, restored.State.pending[0].legIndex);
            Assert.Equal(41, restored.State.pending[0].day);
        }

        [Fact]
        public void NarrativeEncounterSystem_EnqueueAndClearPending()
        {
            var sys = NewNarrative();

            sys.EnqueuePending("enc_dead_letter_office", "loc_the_allotments", 1, 40);
            sys.EnqueuePending("enc_weather_station", "loc_denial_cut_substation", 3, 41);
            Assert.Equal(2, sys.State.pending.Count);

            sys.ClearPending("enc_dead_letter_office");

            Assert.Single(sys.State.pending);
            Assert.Equal("enc_weather_station", sys.State.pending[0].encounterId);
            Assert.Equal("loc_denial_cut_substation", sys.State.pending[0].locationId);
            Assert.Equal(3, sys.State.pending[0].legIndex);
            Assert.Equal(41, sys.State.pending[0].day);

            // Clearing an absent id is a no-op, not an error.
            sys.ClearPending("enc_not_present");
            Assert.Single(sys.State.pending);
        }

        [Fact]
        public void Bridge_ResolveChoice_ClearsPending()
        {
            var narrative = NewNarrative();
            var bridge = new ExpeditionEncounterBridge(narrative, new SeededRng(7));
            ExpeditionEncounterBridge.EncounterSurfaced? surfaced = null;
            bridge.OnSurfaced += dto =>
            {
                surfaced = dto;
                if (!string.IsNullOrEmpty(dto.encounter_id))
                    narrative.EnqueuePending(dto.encounter_id, dto.trigger.locationId, dto.trigger.encounterCount, 40);
            };

            bridge.Surface(NewState(dangerLevel: 0));

            Assert.NotNull(surfaced);
            Assert.NotNull(surfaced!.encounter_id);
            Assert.Single(narrative.State.pending);

            string encId = surfaced!.encounter_id;
            string choiceId = surfaced!.choices[0].choiceId;

            bool ok = bridge.ResolveChoice(encId, choiceId, 40);
            narrative.ClearPending(encId);   // host-side step mirrored by ExpeditionHostSession.EncounterApplyChoice

            Assert.True(ok);
            Assert.Empty(narrative.State.pending);
            Assert.Equal(1, narrative.TotalResolved);
        }

        [Fact]
        public void Bridge_ResolveChoice_ExplicitLocation_RecordsThatLocation()
        {
            var narrative = NewNarrative();
            var bridge = new ExpeditionEncounterBridge(narrative, new SeededRng(7));

            // Surface at location A, then resolve a *different* backlog row that
            // was recorded at location B. B must land in the history.
            bridge.Surface(NewState(locationId: "loc_the_allotments", dangerLevel: 0));
            narrative.EnqueuePending("enc_dead_letter_office", "loc_denial_cut_substation", 3, 41);

            bool ok = bridge.ResolveChoice("enc_dead_letter_office", "read", 41, "loc_denial_cut_substation");

            Assert.True(ok);
            Assert.Single(narrative.State.history);
            Assert.Equal("loc_denial_cut_substation", narrative.State.history[0].locationId);
            Assert.Equal(41, narrative.State.history[0].day);
        }

        [Fact]
        public void Bridge_ResolveChoice_BacklogRow_DoesNotStampNewestSurfaced()
        {
            var narrative = NewNarrative();
            var bridge = new ExpeditionEncounterBridge(narrative, new SeededRng(7));
            ExpeditionEncounterBridge.EncounterSurfaced? surfaced = null;
            bridge.OnSurfaced += dto => surfaced = dto;

            bridge.Surface(NewState(dangerLevel: 0));
            Assert.NotNull(surfaced);

            // Resolve some other encounter than the one currently surfaced.
            string other = surfaced!.encounter_id == "enc_dead_letter_office"
                ? "enc_weather_station"
                : "enc_dead_letter_office";
            string otherChoice = other == "enc_dead_letter_office" ? "read" : "copy";

            bool ok = bridge.ResolveChoice(other, otherChoice, 41, "loc_denial_cut_substation");

            Assert.True(ok);
            // The newest surfaced DTO was NOT the one decided, so it stays undecided.
            Assert.Null(surfaced!.resolved_at_lead);
            Assert.Null(surfaced!.encounter_record_resolution_id);
        }

        [Fact]
        public void Bridge_ResolveChoice_NoSurfaceAndNoLocation_ReturnsFalse()
        {
            var narrative = NewNarrative();
            var bridge = new ExpeditionEncounterBridge(narrative, new SeededRng(7));

            // Nothing surfaced and no explicit location: refuse rather than
            // invent an empty-string location in the history.
            Assert.False(bridge.ResolveChoice("enc_dead_letter_office", "read", 40));
            Assert.Empty(narrative.State.history);
        }

        [Fact]
        public void Bridge_Integration_OneTriggerEmitsOneDto()
        {
            var narrative = NewNarrative();
            var rng = new SeededRng(123);
            var bridge = new ExpeditionEncounterBridge(narrative, rng);

            var fakeEngine = new ExpeditionSystem();
            int triggered = 0;
            ExpeditionEncounterBridge.EncounterSurfaced? last = null;
            bridge.OnSurfaced += dto => { triggered++; last = dto; };

            fakeEngine.OnEncounterTriggered += s => bridge.Surface(s);
            fakeEngine.Start(new ExpeditionDefinition { id = "loc_x", displayName = "X", distanceTicks = 2, dangerLevel = 1, encounterChancePerTick = 1.0f }, "svy", 1, ExpeditionStance.Stealth);
            // Shared RNG: TickHours consumes stamina/loot rolls first, then bridge consumes selection.
            for (int t = 0; t < 10 && triggered == 0; t++)
                fakeEngine.TickHours(2f, rng);

            Assert.Equal(1, triggered);
            Assert.NotNull(last);
        }
    }
}
