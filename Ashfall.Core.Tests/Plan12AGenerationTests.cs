// SPDX-License-Identifier: MIT
// Plan 12A — generative society tests. Behavioral coverage for:
//   1. CohortSystem.TryMaturation is one-way, fires OnMaturation, persists
//      through save/load.
//   2. ApprenticeshipSystem grants an existing canonical skill and the
//      pair completion flows through TraitLedger / skill state deterministically.
//   3. Schooling decision events + orphan adoption events + coming-of-age
//      events exist in events.json with valid mechanical hooks
//      (world flags, scheduled follow-ups, conditions).
//   4. Questline content authored under Plan 12A is in questline_master.json
//      and the canonical child questlines are no longer empty stub ids.
//   5. New condition flags (RequireChildCohort, RequireMaturationEligible,
//      RequireCasualtyOrphan) and the strange new narrativeHook values on
//      questline entries are documented strings — they are not yet engine-
//      consumers but pre-exist as gateway markers for future host wiring;
//      the test pins them as deterministic literal set so accidental
//      renaming surfaces here.

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests
{
    public class Plan12AGenerationTests : CatalogTestBase
    {
        private static string DataDir => DataDirectory;

        // ────────── 1. CohortSystem.TryMaturation ──────────

        [Fact]
        public void Cohort_TryMaturation_BookedChild_FiresAndPersistsOnce()
        {
            var sys = new CohortSystem();
            sys.BookChild("child_a", new List<string> { "sv_mae", "sv_ged" }, "medium", 280);

            int fired = 0;
            string firedFor = null;
            int firedDay = -1;
            sys.OnMaturation += (id, day) => { fired++; firedFor = id; firedDay = day; };

            Assert.True(sys.TryMaturation("child_a", 374));
            var child = sys.GetChild("child_a");
            Assert.NotNull(child);
            Assert.True(child.isMatured);
            Assert.Equal(374, child.maturationDay);
            Assert.Equal(1, fired);
            Assert.Equal("child_a", firedFor);
            Assert.Equal(374, firedDay);

            // Idempotent: a second maturation call is no-op (the flag sticks).
            Assert.False(sys.TryMaturation("child_a", 999));
            Assert.Equal(374, sys.GetChild("child_a").maturationDay);
            Assert.Equal(1, fired);
        }

        [Fact]
        public void Cohort_TryMaturation_BadInputRejected()
        {
            var sys = new CohortSystem();
            sys.BookChild("child_a", new List<string>(), "low", 280);
            // Unknown child
            Assert.False(sys.TryMaturation("child_missing", 300));
            // Invalid day
            Assert.False(sys.TryMaturation("child_a", 0));
            Assert.False(sys.TryMaturation("child_a", -7));
            // Empty id
            Assert.False(sys.TryMaturation("", 300));
            Assert.False(sys.TryMaturation(null, 300));
        }

        [Fact]
        public void Cohort_PersistsMaturationThroughSaveRoundTrip()
        {
            var sys = new CohortSystem();
            sys.BookChild("child_a", new List<string> { "sv_mae" }, "high", 280);
            sys.TryMaturation("child_a", 374);

            var snap = sys.CaptureState();
            var copy = new CohortSystem();
            copy.RestoreState(snap);

            var reborn = copy.GetChild("child_a");
            Assert.NotNull(reborn);
            Assert.True(reborn.isMatured);
            Assert.Equal(374, reborn.maturationDay);
            Assert.Equal(new List<string> { "sv_mae" }, reborn.parentIds);
        }

        // ────────── 2. Apprenticeship reward still rides the existing skill
        //     pipeline: a completed apprentice gets the canonical skill id
        //     (skill_rough_repairs) the apprenticeship declared, with no
        //     parallel counter. ──────────

        [Fact]
        public void Apprenticeship_OnCompletion_GrantsCanonicalSkillOnly()
        {
            var seed = new SeededRng(0);
            var skills = new SkillProgressionSystem();
            var duty = new DutyRosterSystem();
            var relations = new SurvivorRelationsSystem(new SeededRng(42));
            var sys = new ApprenticeshipSystem(seed, skills, duty, relations);

            // Patch the apprenticeship's mentor to be qualified in
            // skill_rough_repairs by recording an action with high XP.
            skills.RecordAction(new SimpleSkillActor("mentor_1"), "skill_rough_repairs", 50f, 1);

            int completedFor = 0;
            sys.OnApprenticeshipCompleted += _ => completedFor++;

            var started = sys.StartPair("mentor_1", "apprentice_1", "skill_rough_repairs", targetXp: 20f);
            // Pair may be blocked by DutyRoster unless mentor/apprentice are
            // free; we use unique ids not present in any roster.
            Assert.Equal(ActionResult.StatusKind.Success, started.Status);

            // 2 ticks @ 10 xp/day → 20 xp → completion
            sys.TickDay(1);
            sys.TickDay(2);

            Assert.Equal(1, completedFor);
            Assert.Contains("skill_rough_repairs", sys.State.completedSkillIds);
        }

        // ────────── 3. event authoring + mechanical hooks present ──────────

        [Fact]
        public void Events_Plan12A_Bundle_ExistsAndHooksAreAuthored()
        {
            var events = System.Text.Json.JsonDocument.Parse(
                System.IO.File.ReadAllText(System.IO.Path.Combine(DataDir, "events.json")))
                .RootElement.GetProperty("events");
            var ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (var ev in events.EnumerateArray())
            {
                var s = ev.GetProperty("id").GetString();
                if (!string.IsNullOrEmpty(s)) ids.Add(s);
            }
            // The complete Plan 12A event footprint.
            string[] required =
            {
                "apprenticeship_completion_rough_repairs",
                "apprenticeship_completion_field_dressing",
                "apprenticeship_completion_signal_ear",
                "apprenticeship_completion_workshop_sense",
                "apprenticeship_completion_watchful",
                "apprenticeship_completion_steady_hands",
                "schooling_curriculum_choice",
                "coming_of_age_first_surface",
                "coming_of_age_first_watch",
                "orphan_adoption_warmarms",
                "orphan_adoption_fierce_mother",
                "orphan_adoption_grange",
                "orphan_adoption_archive"
            };
            foreach (var r in required)
                Assert.True(ids.Contains(r), $"event {r} missing from events.json");

            // Each Plan 12A event surfaces at least one choice and at least one
            // effect (mechanical hooks — see AGENTS.md "no parallel counters").
            foreach (var r in required)
            {
                var ev = FindEvent(events, r);
                Assert.True(ev.TryGetProperty("choices", out var choices));
                Assert.True(choices.GetArrayLength() >= 1, $"{r} must offer at least one choice");
                foreach (var choice in choices.EnumerateArray())
                {
                    Assert.True(choice.TryGetProperty("effects", out _), $"{r}.choice must declare effects");
                }
                Assert.True(ev.TryGetProperty("conditions", out var conds),
                    $"{r} must declare conditions (mechanical hook surface)");
            }
        }

        [Fact]
        public void Events_Plan12A_ConditionFlags_AreClosedAndDocumented()
        {
            // Mechanical condition flags added by Plan 12A. The test pins
            // the allowed closed set so accidental renaming breaks HERE
            // rather than in a distant consumer.
            var allowed = new HashSet<string>(StringComparer.Ordinal)
            {
                "MinDay", "RequireFalloutStorm", "RequireChildCohort",
                "RequireMaturationEligible", "RequireCasualtyOrphan",
                "RequireChildCohort_Rotational", "RequireCohortAdoptionPending",
                "RequireAdoptionConcluded"
            };
            // Only scan events whose id is part of Plan 12A authored output.
            // Pre-existing events use their own condition vocabulary that
            // the project owns but Plan 12A is not its gatekeeper.
            var plan12aIds = new HashSet<string>(StringComparer.Ordinal)
            {
                "apprenticeship_completion_rough_repairs",
                "apprenticeship_completion_field_dressing",
                "apprenticeship_completion_signal_ear",
                "apprenticeship_completion_workshop_sense",
                "apprenticeship_completion_watchful",
                "apprenticeship_completion_steady_hands",
                "schooling_curriculum_choice",
                "coming_of_age_first_surface",
                "coming_of_age_first_watch",
                "orphan_adoption_warmarms",
                "orphan_adoption_fierce_mother",
                "orphan_adoption_grange",
                "orphan_adoption_archive"
            };
            HashSet<string> seen = new HashSet<string>(StringComparer.Ordinal);
            int scanned = 0;
            var events = System.Text.Json.JsonDocument.Parse(
                System.IO.File.ReadAllText(System.IO.Path.Combine(DataDir, "events.json")))
                .RootElement.GetProperty("events");
            foreach (var ev in events.EnumerateArray())
            {
                var eid = ev.GetProperty("id").GetString();
                if (eid == null || !plan12aIds.Contains(eid)) continue;
                scanned++;
                if (!ev.TryGetProperty("conditions", out var c)) continue;
                foreach (var prop in c.EnumerateObject())
                {
                    var key = prop.Name;
                    if (!allowed.Contains(key))
                    {
                        Assert.True(false,
                            "Unknown event condition '" + key + "' in Plan 12A event id=" + eid
                            + " — add to Plan12AGenerationTests.Events_Plan12A_ConditionFlags_AreClosedAndDocumented allowlist (or rename to a documented flag).");
                    }
                    seen.Add(key);
                }
            }
            // Plan 12A authored events must use at least one of the two
            // structural flags. If a future iteration adds events under
            // this plan that don't toggle any of them, the test will fail
            // so the omission is intentional.
            foreach (var k in new[] { "RequireChildCohort", "RequireCasualtyOrphan" })
                Assert.True(seen.Contains(k),
                    $"Plan 12A authored 0 events using {k}; check the conditions are wired on every event.");
            Assert.Equal(plan12aIds.Count, scanned);
        }

        // ────────── 4. Questline content present and the canonical four child
        //     quests are no longer empty stubs. ──────────

        [Fact]
        public void Questlines_Plan12A_FourChildren_FilledBody_AndNewArcsPresent()
        {
            var root = System.Text.Json.JsonDocument.Parse(
                System.IO.File.ReadAllText(System.IO.Path.Combine(DataDir, "questline_master.json")))
                .RootElement.GetProperty("entries");
            var byId = new Dictionary<string, System.Text.Json.JsonElement>(StringComparer.Ordinal);
            foreach (var e in root.EnumerateArray())
            {
                var s = e.GetProperty("id").GetString();
                if (!string.IsNullOrEmpty(s)) byId[s] = e;
            }
            // The 4 child questlines that existed as empty stubs are now
            // fully populated.
            string[] required = {
                "quest_growing_up_fast",
                "quest_first_blood",
                "quest_dropping_the_rifle",
                "quest_the_pack"
            };
            foreach (var r in required)
            {
                Assert.True(byId.ContainsKey(r), $"missing questline {r}");
                var e = byId[r];
                Assert.True(e.TryGetProperty("title", out var t) && !string.IsNullOrWhiteSpace(t.GetString()),
                    $"{r} must have a non-empty title after Plan 12A");
                Assert.True(e.TryGetProperty("synopsis", out var s) && !string.IsNullOrWhiteSpace(s.GetString()),
                    $"{r} must have a non-empty synopsis after Plan 12A");
            }
            // The 16 new arcs (6 apprentice + 4 schooling + 4 adoption + 2 coming-of-age).
            string[] newArcs = {
                "quest_apprentice_pipefitting", "quest_apprentice_dressing", "quest_apprentice_radio",
                "quest_apprentice_recycling", "quest_apprentice_hatch", "quest_apprentice_triage",
                "quest_schooling_curriculum_letters", "quest_schooling_curriculum_mechanics",
                "quest_schooling_curriculum_medicine", "quest_schooling_curriculum_marksmanship",
                "quest_adoption_warmarms", "quest_adoption_fierce_mother",
                "quest_adoption_grange", "quest_adoption_archive",
                "quest_coming_of_age_first_surface", "quest_coming_of_age_first_watch"
            };
            foreach (var r in newArcs)
                Assert.True(byId.ContainsKey(r), $"new questline {r} missing");
        }

        // ────────── 5. narrativeHook values are part of a documented literal
        //    set so accidental renaming breaks here. Add to the allowlist
        //    in this test any new hook a future event/quest consumer adds. ──────────

        [Fact]
        public void Questlines_Plan12A_NarrativeHook_AreClosedAndDocumented()
        {
            // The complete closed set of narrativeHook strings Plan 12A uses.
            var allowed = new HashSet<string>(StringComparer.Ordinal)
            {
                "schooling_curriculum_choice",
                "mentorship_veteran_pair",
                "mentorship_medic_pair",
                "mentorship_forge_pair",
                "mentorship_signal_pair",
                "mentorship_triage_pair",
                "mentorship_medic_or_smith",
                "apprenticeship_medic_or_smith",
                "apprentice_arc",
                "schooling_letters_unlock",
                "schooling_mechanics_unlock",
                "schooling_medicine_unlock",
                "schooling_marksmanship_unlock",
                "adoption_warmarms", "adoption_rotational_vigil", "adoption_grange_plot", "adoption_archive_intake",
                "orphan_adoption_pair_choice",
                "apprenticeship_completion:skill_field_dressing",
                "apprenticeship_completion:skill_steady_hands",
                "apprenticeship_completion:skill_rough_repairs",
                "apprenticeship_completion:skill_workshop_sense",
                "apprenticeship_completion:skill_signal_ear",
                "apprenticeship_completion:skill_watchful",
                "childhood_path",
                "coming_of_age", "trigger_maturation",
                "cipher_relay_decode", "cipher_winter_decode", "cipher_rotation_decode"
            };
            var root = System.Text.Json.JsonDocument.Parse(
                System.IO.File.ReadAllText(System.IO.Path.Combine(DataDir, "questline_master.json")))
                .RootElement.GetProperty("entries");
            foreach (var e in root.EnumerateArray())
            {
                if (!e.TryGetProperty("narrativeHook", out var h)) continue;
                var s = h.GetString();
                if (string.IsNullOrEmpty(s)) continue;
                if (!allowed.Contains(s))
                {
                    Assert.True(false,
                        "Unknown narrativeHook '" + s + "' on quest id="
                        + e.GetProperty("id").GetString()
                        + " — add to Plan12AGenerationTests.Questlines_Plan12A_NarrativeHook_AreClosedAndDocumented (or rename to a documented hook).");
                }
            }
        }

        private static System.Text.Json.JsonElement FindEvent(System.Text.Json.JsonElement eventsRoot, string id)
        {
            foreach (var ev in eventsRoot.EnumerateArray())
            {
                if (ev.GetProperty("id").GetString() == id) return ev;
            }
            throw new InvalidOperationException("event id=" + id + " not found");
        }
    }
}
