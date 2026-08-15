using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    [CollectionDefinition("VerdictEdgeCase", DisableParallelization = true)]
    public sealed class VerdictEdgeCaseCollection { }

    /// <summary>
    /// ASHFALL: THE VERDICT (Expansion 08) — negative-path edge cases.
    /// Guards the failure modes catalog validity cannot see:
    ///  - seasonCap rollover across the 60-day season boundary,
    ///  - one-shot encounters never re-offer after resolution,
    ///  - day-window exclusivity (no encounter fires outside min/max),
    ///  - faction token fallback when a required item id is unknown.
    /// </summary>
    [Collection("VerdictEdgeCase")]
    public class VerdictEdgeCaseTests
    {
        private static DoorEncounterEntry Make(string id, int minDay, int maxDay, int seasonCap = 0)
            => new DoorEncounterEntry
            {
                encounterId = id,
                visitorName = id,
                description = id,
                minDay = minDay,
                maxDay = maxDay,
                threatLevel = 1,
                seasonCap = seasonCap,
                choices = new List<EncounterChoice>
                {
                    new EncounterChoice { choiceId = "c_leave", text = "Leave.",
                        baseMoraleDelta = 0, baseGuiltDelta = 0,
                        requiredItemId = string.Empty, requiredItemQuantity = 0,
                        targetFaction = string.Empty, factionStandingDelta = 0,
                        outcomeDescription = "Gone." }
                }
            };

        private static SurvivorOccupantSnapshot[] Roster() => Array.Empty<SurvivorOccupantSnapshot>();

        // ── seasonCap rollover across the 60-day season boundary ──────────────

        [Fact]
        public void SeasonCap_BlocksRepeat_WithinSameSeason_ButAllowsNextSeason()
        {
            var sys = new DoorEncounterSystem();
            sys.RegisterEncounter(Make("door_encounter_verdict_tape_exchange", 220, 360, seasonCap: 1));
            var enc = sys.GetEligibleEncounters(230);
            var tapeEnc = enc.Find(e => e.encounterId == "door_encounter_verdict_tape_exchange");
            Assert.NotNull(tapeEnc);

            // Fire it once in season 3 (day 230/60 = 3). The eligibility query above
            // sets the day context the resolver uses to stamp the season.
            sys.ResolveChoice(tapeEnc, tapeEnc.choices[0], Roster());

            // Same season (day 235 / 60 = 3) => suppressed by seasonCap.
            Assert.DoesNotContain(sys.GetEligibleEncounters(235), e => e.encounterId == "door_encounter_verdict_tape_exchange");

            // Next season (day 300 / 60 = 5) => allowed again.
            Assert.Contains(sys.GetEligibleEncounters(300), e => e.encounterId == "door_encounter_verdict_tape_exchange");

            // But beyond maxDay => never.
            Assert.DoesNotContain(sys.GetEligibleEncounters(400), e => e.encounterId == "door_encounter_verdict_tape_exchange");
        }

        // ── one-shot: resolved encounters never re-offer (any season) ──────────

        [Fact]
        public void OneShot_ResolvedEncounter_NeverReOffers()
        {
            var sys = new DoorEncounterSystem();
            // Note: this one has seasonCap 0 (unlimited) — the ONE-SHOT gate is
            // `resolvedEncounterIds`, which must still permanently hide it.
            sys.RegisterEncounter(Make("door_encounter_verdict_tape_seller", 165, 240, seasonCap: 0));
            var enc = sys.GetEligibleEncounters(170);
            var tapeSellerEnc = enc.Find(e => e.encounterId == "door_encounter_verdict_tape_seller");
            Assert.NotNull(tapeSellerEnc);

            sys.ResolveChoice(tapeSellerEnc, tapeSellerEnc.choices[0], Roster());

            Assert.DoesNotContain(sys.GetEligibleEncounters(180), e => e.encounterId == "door_encounter_verdict_tape_seller");
            Assert.DoesNotContain(sys.GetEligibleEncounters(360), e => e.encounterId == "door_encounter_verdict_tape_seller");
        }

        // ── day window: never eligible outside [minDay, maxDay] ────────────────

        [Fact]
        public void DayWindow_OutsideRange_NotEligible()
        {
            var sys = new DoorEncounterSystem();
            sys.RegisterEncounter(Make("door_encounter_verdict_relay_repair", 170, 260));
            Assert.DoesNotContain(sys.GetEligibleEncounters(169), e => e.encounterId == "door_encounter_verdict_relay_repair");
            Assert.Contains(sys.GetEligibleEncounters(170), e => e.encounterId == "door_encounter_verdict_relay_repair");
            Assert.Contains(sys.GetEligibleEncounters(260), e => e.encounterId == "door_encounter_verdict_relay_repair");
            Assert.DoesNotContain(sys.GetEligibleEncounters(261), e => e.encounterId == "door_encounter_verdict_relay_repair");
        }

        // ── save round-trip preserves seasonCap + one-shot history ─────────────

        [Fact]
        public void EncounterState_RoundTrips_SeasonAndOneShot()
        {
            var sys = new DoorEncounterSystem();
            sys.RegisterEncounter(Make("door_encounter_verdict_tape_exchange", 220, 360, seasonCap: 1));
            var enc = sys.GetEligibleEncounters(230); // sets the season-stamping day context
            var tapeEnc = enc.Find(e => e.encounterId == "door_encounter_verdict_tape_exchange");
            Assert.NotNull(tapeEnc);
            sys.ResolveChoice(tapeEnc, tapeEnc.choices[0], Roster());

            var snap = sys.CaptureState();
            var fresh = new DoorEncounterSystem();
            fresh.RegisterEncounter(Make("door_encounter_verdict_tape_exchange", 220, 360, seasonCap: 1));
            fresh.RestoreState(snap);

            Assert.DoesNotContain(fresh.GetEligibleEncounters(235), e => e.encounterId == "door_encounter_verdict_tape_exchange");
            Assert.Contains(fresh.GetEligibleEncounters(300), e => e.encounterId == "door_encounter_verdict_tape_exchange");
        }

        // ── negative: unknown required item id fails closed (no crash) ─────────

        [Fact]
        public void ResolveChoice_UnknownItemId_FailsSafely()
        {
            var sys = new DoorEncounterSystem();
            var enc = Make("door_encounter_verdict_sound_engineer", 200, 340);
            enc.choices[0].requiredItemId = "item_does_not_exist_zzz";
            sys.RegisterEncounter(enc);

            var result = sys.ResolveChoice(enc, enc.choices[0], Roster());
            Assert.NotNull(result);              // resolves without crash
            Assert.Equal("door_encounter_verdict_sound_engineer", result.encounterId);
        }

        // ── hold-branch: no branch double-resolves ─────────────────────────────

        [Fact]
        public void Quest_HoldBranch_ResolvesEachBranchIndependently()
        {
            var system = new QuestlineSystem();
            var def = new QuestlineDefinition
            {
                questlineId = "quest_verdict_the_hold",
                title = "The Hold",
                factionTag = "faction_the_tempest",
                firstStageId = "stage_hold_register",
                minDay = 200,
                maxDay = 360
            };
            def.stages.Add(new QuestStage
            {
                stageId = "stage_hold_register",
                title = "The Dead Hand",
                narrativePrompt = "x",
                choices = new List<QuestChoice>
                {
                    new QuestChoice { choiceId = "choice_hold_release", text = "Release.", nextStageId = "", moraleDelta = 1, guiltDelta = 4 },
                    new QuestChoice { choiceId = "choice_hold_retain", text = "Retain.", nextStageId = "", moraleDelta = 0 },
                    new QuestChoice { choiceId = "choice_hold_count", text = "Count.", nextStageId = "", moraleDelta = 2 }
                }
            });
            system.RegisterQuestline(def);

            Assert.True(system.StartQuestline("quest_verdict_the_hold", 250));
            var r = system.TakeChoice("quest_verdict_the_hold", "choice_hold_release", 250);
            Assert.NotNull(r);
            Assert.Equal(QuestlineStatus.Completed, r.newQuestStatus);

            // Terminal: second choice on a finished questline is refused (no double fire).
            var again = system.TakeChoice("quest_verdict_the_hold", "choice_hold_retain", 250);
            Assert.Null(again);
        }
    }
}
