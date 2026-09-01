// SPDX-License-Identifier: MIT
// Plan 12B \u2014 friction, ration-conflict, escalation, graffiti. Behavioral
// coverage pinning:
//
//   1. IdeologicalFrictionSystem.ConflictGroups closed-set pin for the
//      4 NEW belief profile ids (each new id must appear once AND be
//      paired with every faction it is in conflict with, otherwise
//      AreInConflict returns false silently and the bunker stays quiet
//      when it should bristle).
//
//   2. Data authority present in events.json + bunker_graffiti_postings.json
//      with mechanical hooks (choices, conditions, setWorldFlag keys,
//      resolve to existing flag_* ids).
//
//   3. Closed-set pin on graffiti/trigger-world-flag ids and the new
//      setWorldFlag values used by Plan 12B, so accidental renaming
//      breaks here rather than in a distant consumer.
//
//   4. End-to-end behavior: registering two survivors in Plan 12B's
//      paired belief ids surfaces OnFrictionDetected and a
//      sleep-quality penalty below 1.0.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests
{
    public class Plan12BFrictionTests : CatalogTestBase
    {
        private static string DataDir => DataDirectory;

        // \u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500 1. ConflictGroups closed-set \u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500

        // Documented Plan 12B belief set. The test fails if any of these
        // is missing from the published ConflictGroups dictionary.
        private static readonly string[] Plan12BBeliefIds = new[]
        {
            "belief_ration_collectivist",
            "belief_every_soul_alone",
            "belief_faith_in_rebuild",
            "belief_ash_nihilist"
        };

        [Fact]
        public void IdeologicalFrictionSystem_HasPlan12BBeliefIds_InConflictGroups()
        {
            foreach (var id in Plan12BBeliefIds)
            {
                Assert.True(
                    IdeologicalFrictionSystem.ConflictGroups.ContainsKey(id),
                    "Plan 12B belief id '" + id + "' missing from IdeologicalFrictionSystem.ConflictGroups");
            }
        }

        [Fact]
        public void IdeologicalFrictionSystem_Plan12BPairs_AreReciprocal_Friction()
        {
            // Each Plan 12B belief must have at least one partner to be in
            // conflict with AND must be the partner of every belief in
            // its pair array. AreInConflict is symmetric.
            var sys = new IdeologicalFrictionSystem();
            foreach (var id in Plan12BBeliefIds)
            {
                var partners = IdeologicalFrictionSystem.ConflictGroups[id];
                foreach (var p in partners)
                {
                    // The AreInConflict matrix must return true on both directions.
                    Assert.True(
                        PrivateInvoke.AreInConflict(id, p),
                        "AreInConflict(" + id + ", " + p + ") returned false; the published pair is one-way.");
                    Assert.True(
                        PrivateInvoke.AreInConflict(p, id),
                        "AreInConflict(" + p + ", " + id + ") returned false; the published pair is one-way.");
                }
            }
        }

        [Fact]
        public void IdeologicalFrictionSystem_Plan12BPair_NoSelfConflict()
        {
            // Every Plan 12B belief must NOT be in conflict with itself.
            foreach (var id in Plan12BBeliefIds)
            {
                Assert.False(PrivateInvoke.AreInConflict(id, id),
                    "Plan 12B belief " + id + " is in conflict with itself.");
            }
        }

        [Fact]
        public void IdeologicalFrictionSystem_Plan12BPair_ProducesSleepPenalty_OnTick()
        {
            // End-to-end: register a belief pair survivor pair, run a
            // single GetRoommateCompatibilityMultiplier call, assert the
            // OnFrictionDetected event fired AND the multiplier < 1.0.
            var sys = new IdeologicalFrictionSystem();
            string resenter = "sv_ration_collectivist";
            string target = "sv_every_soul_alone";

            int fired = 0;
            float lastPenalty = 1f;
            sys.OnFrictionDetected += (a, b, penalty) => { fired++; lastPenalty = penalty; };

            sys.RegisterBelief(resenter, "belief_ration_collectivist");
            sys.RegisterBelief(target, "belief_every_soul_alone");
            float mult = sys.GetRoommateCompatibilityMultiplier(resenter, target);

            Assert.True(fired >= 1, "OnFrictionDetected did not fire on a known-conflict belief pair");
            Assert.True(mult < 1f, "Compatibility multiplier should be < 1.0 for a Plan 12B conflict pair");
        }

        [Fact]
        public void IdeologicalFrictionSystem_Plan12BSynergy_OnMatchingBeliefs()
        {
            // Same belief should NOT count as conflict; the system grants
            // a synergy bonus across the bed.
            var sys = new IdeologicalFrictionSystem();

            string s1 = "sv_ration_collectivist_a";
            string s2 = "sv_ration_collectivist_b";
            sys.RegisterBelief(s1, "belief_ration_collectivist");
            sys.RegisterBelief(s2, "belief_ration_collectivist");

            float mult = sys.GetRoommateCompatibilityMultiplier(s1, s2);
            Assert.True(mult > 1f, "Same belief pair should yield a synergy multiplier > 1.0; got " + mult);
        }

        // \u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500 2. Data authority \u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500

        [Fact]
        public void Graffiti_Catalog_Exists_And_AllPostingsMeetSchema()
        {
            string path = Path.Combine(DataDir, "bunker_graffiti_postings.json");
            Assert.True(File.Exists(path), "bunker_graffiti_postings.json must exist (Plan 12B authored fresh)");
            using var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(path));
            var root = doc.RootElement;
            Assert.True(root.TryGetProperty("postings", out var postings));
            Assert.Equal(System.Text.Json.JsonValueKind.Array, postings.ValueKind);
            var ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (var p in postings.EnumerateArray())
            {
                foreach (var key in new[] { "id", "title", "text", "triggerWorldFlag" })
                {
                    Assert.True(p.TryGetProperty(key, out var _), "graffiti posting missing required field '" + key + "'");
                }
                var id = p.GetProperty("id").GetString();
                Assert.StartsWith("graffiti_", id);
                Assert.True(ids.Add(id), "duplicate graffiti id " + id);
                var flag = p.GetProperty("triggerWorldFlag").GetString();
                // The project's world flags ride a few accepted prefixes
                // (Originally flag_, but events.json in the wider project
                // also uses friction_/ration_/escalation_). The catalog
                // integrator handles them all (see CatalogIntegrityRules).
                Assert.True(
                    flag.StartsWith("flag_", StringComparison.Ordinal)
                    || flag.StartsWith("friction_", StringComparison.Ordinal)
                    || flag.StartsWith("ration_", StringComparison.Ordinal)
                    || flag.StartsWith("escalation_", StringComparison.Ordinal),
                    "graffiti trigger flag '" + flag + "' must start with one of the canonical prefixes");
            }
            // At least the 10 documented extras.
            Assert.True(postings.GetArrayLength() >= 10,
                "Plan 12B promised \u226510 graffiti postings; got " + postings.GetArrayLength());
        }

        [Fact]
        public void Events_Plan12B_Bundle_IsAuthoredAndWired()
        {
            var ids = LoadAllEventIds();
            // The 10 + 6 + 4 = 20 Plan 12B event ids. Each pair (the chosen
            // set must equal the public IDs the plan documents).
            string[] required = {
                // 10 friction events
                "friction_snoring_feud",
                "friction_stolen_keepsake",
                "friction_forbidden_radio_listening",
                "friction_work_shirker_accusation",
                "friction_stealing_sleep_rotation",
                "friction_ash_sermon_quiet_hours",
                "friction_inherited_walkout_threat",
                "friction_two_sides_of_chalk",
                "friction_ration_observation_overheard",
                "friction_ration_stolen_dry_rations",
                // 6 ration events
                "ration_uneven_scoop_third_day",
                "ration_hoarded_tins_first_sign",
                "ration_feast_day_demand",
                "ration_sick_gets_more_dispute",
                "ration_theft_with_evidence",
                "ration_resentment_kindling",
                // 4 escalation events
                "escalation_walkout",
                "escalation_sabotage",
                "escalation_challenge_to_leadership",
                "escalation_walkout_reconsidered"
            };
            foreach (var id in required)
                Assert.True(ids.Contains(id), "Plan 12B event '" + id + "' missing from events.json");
        }

        [Fact]
        public void Events_Plan12B_AllEvents_HaveChoicesAndAtLeastOneEffect()
        {
            var doc = System.Text.Json.JsonDocument.Parse(
                File.ReadAllText(Path.Combine(DataDir, "events.json")))
                .RootElement.GetProperty("events");
            string[] plan12bIds = {
                "friction_snoring_feud","friction_stolen_keepsake","friction_forbidden_radio_listening",
                "friction_work_shirker_accusation","friction_stealing_sleep_rotation",
                "friction_ash_sermon_quiet_hours","friction_inherited_walkout_threat",
                "friction_two_sides_of_chalk","friction_ration_observation_overheard",
                "friction_ration_stolen_dry_rations",
                "ration_uneven_scoop_third_day","ration_hoarded_tins_first_sign",
                "ration_feast_day_demand","ration_sick_gets_more_dispute",
                "ration_theft_with_evidence","ration_resentment_kindling",
                "escalation_walkout","escalation_sabotage","escalation_challenge_to_leadership",
                "escalation_walkout_reconsidered"
            };
            foreach (var id in plan12bIds)
            {
                var found = false;
                foreach (var e in doc.EnumerateArray())
                {
                    if (e.GetProperty("id").GetString() != id) continue;
                    found = true;
                    Assert.True(e.TryGetProperty("choices", out var choices));
                    Assert.True(choices.GetArrayLength() >= 1,
                        "Plan 12B event '" + id + "' must offer at least one choice");
                    foreach (var choice in choices.EnumerateArray())
                    {
                        Assert.True(choice.TryGetProperty("effects", out var effects));
                        Assert.True(effects.GetArrayLength() >= 1,
                            "Plan 12B event '" + id + "' choice must declare at least one effect");
                    }
                }
                Assert.True(found, "event " + id + " not found in events.json during scan");
            }
        }

        // \u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500 3. World flag closed-set pin \u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500

        // Allowed setWorldFlag values Plan 12B introduces. Add to this list
        // if a future iteration in the same plan introduces a new flag id.
        private static readonly HashSet<string> AllowedPlan12BWorldFlagValues =
            new HashSet<string>(StringComparer.Ordinal);

        [Fact]
        public void Events_Plan12B_SetWorldFlags_AreClosedAndDocumented()
        {
            // Complete closed set of world flags Plan 12B introduced.
            var allowed = new HashSet<string>(StringComparer.Ordinal)
            {
                "friction_snoring_feud_mediated",
                "friction_snoring_feud_swapped",
                "friction_stolen_keepsake",
                "friction_stolen_keepsake_left",
                "friction_forbidden_radio_listening",
                "friction_radio_dial_argument",
                "friction_work_shirker_accusation",
                "friction_shirker_amended",
                "friction_visitor_assigned",
                "friction_second_bunk_permitted",
                "friction_ash_cant_banned",
                "friction_ash_cant_admitted",
                "friction_walkout_council_held",
                "friction_chalk_pen_compromise",
                "friction_chalk_dispute_pending",
                "friction_ration_observation_discreet",
                "friction_ration_observation_open",
                "faction_stores_locked",
                "ration_scoop_rotated",
                "ration_double_lead_ack",
                "ration_first_tin_logged",
                "ration_tin_shared_response",
                "ration_hoarded_tins_first_sign",
                "ration_feast_honored",
                "ration_feast_deferred",
                "ration_medic_extra_canon",
                "ration_medic_extra_audit",
                "ration_theft_second_bunk_named",
                "ration_double_tally_week",
                "ration_resentment_mediated",
                "ration_resentment_allowed",
                "escalation_walkout",
                "escalation_walkout_reconsidered",
                "escalation_sabotage_investigated",
                "escalation_challenge_acknowledged",
                "escalation_leadership_stepdown",
                "escalation_walkout_welcomed",
                "friction_stolen_keepsake_after",
                "escalation_walkout_conditions",
                "post_walkout_reconsider",
                "friction_walkout_conditions_set",
                "friction_walkout_letter_written"
            };
            var doc = System.Text.Json.JsonDocument.Parse(
                File.ReadAllText(Path.Combine(DataDir, "events.json")))
                .RootElement.GetProperty("events");
            foreach (var ev in doc.EnumerateArray())
            {
                // Only Pin 12B-authored events (those whose id starts with
                // friction_/ration_/escalation_ + matches our set). Other
                // events use their own flag vocabulary and should not be
                // gated by Plan 12B.
                // Only Scan events whose id is part of Plan 12B authored
                // output (the events_Plan12B_Bundle required list). Other
                // events use their own flag vocabulary that the project
                // owns but Plan 12B is not its gatekeeper.
                var eid = ev.GetProperty("id").GetString();
                if (eid == null) continue;
                if (!Plan12BEventIds.Contains(eid)) continue;
                if (!ev.TryGetProperty("choices", out var choices)) continue;
                foreach (var c in choices.EnumerateArray())
                {
                    if (!c.TryGetProperty("effects", out var effects)) continue;
                    foreach (var eff in effects.EnumerateArray())
                    {
                        if (!eff.TryGetProperty("setWorldFlag", out var flag)) continue;
                        var v = flag.GetString();
                        if (!string.IsNullOrEmpty(v) && !allowed.Contains(v))
                        {
                            Assert.True(false,
                                "Unknown Plan 12B setWorldFlag '" + v + "' on event id="
                                + eid + " \u2014 add to Plan12BFrictionTests.allowed allowlist (or rename to a documented flag).");
                        }
                    }
                }
            }
        }

        [Fact]
        public void Grazziti_Posting_Triggers_Each_Resolve_ToAnEventSettingThatFlag()
        {
            // Each graffiti posting's triggerWorldFlag must be set by at
            // least one event in events.json, otherwise it's a dangling UI
            // hook with no narrative chain.
            var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(
                Path.Combine(DataDir, "bunker_graffiti_postings.json")));
            var postings = doc.RootElement.GetProperty("postings");
            var flagsSet = new HashSet<string>(StringComparer.Ordinal);
            foreach (var ev in System.Text.Json.JsonDocument.Parse(
                File.ReadAllText(Path.Combine(DataDir, "events.json")))
                .RootElement.GetProperty("events").EnumerateArray())
            {
                if (!ev.TryGetProperty("choices", out var choices)) continue;
                foreach (var c in choices.EnumerateArray())
                {
                    if (!c.TryGetProperty("effects", out var effects)) continue;
                    foreach (var eff in effects.EnumerateArray())
                    {
                        if (eff.TryGetProperty("setWorldFlag", out var f))
                        {
                            var v = f.GetString();
                            if (!string.IsNullOrEmpty(v)) flagsSet.Add(v);
                        }
                    }
                }
            }
            foreach (var p in postings.EnumerateArray())
            {
                var flag = p.GetProperty("triggerWorldFlag").GetString();
                Assert.True(flagsSet.Contains(flag),
                    "graffiti posting " + p.GetProperty("id").GetString()
                    + " triggers '" + flag + "' but no event sets that flag.");
            }
        }

        // \u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500 helpers \u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500

        // Documented Plan 12B event ids. The test fails if any of these
        // is missing from events.json OR if a closed-set world-flag id
        // is introduced in these events without adding it to the allowlist.
        private static readonly HashSet<string> Plan12BEventIds = new HashSet<string>(StringComparer.Ordinal)
        {
            "friction_snoring_feud",
            "friction_stolen_keepsake",
            "friction_forbidden_radio_listening",
            "friction_work_shirker_accusation",
            "friction_stealing_sleep_rotation",
            "friction_ash_sermon_quiet_hours",
            "friction_inherited_walkout_threat",
            "friction_two_sides_of_chalk",
            "friction_ration_observation_overheard",
            "friction_ration_stolen_dry_rations",
            "ration_uneven_scoop_third_day",
            "ration_hoarded_tins_first_sign",
            "ration_feast_day_demand",
            "ration_sick_gets_more_dispute",
            "ration_theft_with_evidence",
            "ration_resentment_kindling",
            "escalation_walkout",
            "escalation_sabotage",
            "escalation_challenge_to_leadership",
            "escalation_walkout_reconsidered"
        };

        private static bool StartCheck(string id, string prefix, params string[] ownIds)
        {
            if (!id.StartsWith(prefix, StringComparison.Ordinal)) return false;
            foreach (var own in ownIds)
                if (id == own) return true;
            return false;
        }

        private static HashSet<string> LoadAllEventIds()
        {
            var ids = new HashSet<string>(StringComparer.Ordinal);
            var doc = System.Text.Json.JsonDocument.Parse(
                File.ReadAllText(Path.Combine(DataDir, "events.json")))
                .RootElement.GetProperty("events");
            foreach (var ev in doc.EnumerateArray())
            {
                var s = ev.GetProperty("id").GetString();
                if (!string.IsNullOrEmpty(s)) ids.Add(s);
            }
            return ids;
        }

        private static class PrivateInvoke
        {
            // AreInConflict is private and static; we reflect into it so we
            // can pin the matrix directly without simulating full ticks.
            public static bool AreInConflict(string a, string b)
            {
                var m = typeof(IdeologicalFrictionSystem).GetMethod(
                    "AreInConflict",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                if (m == null) throw new InvalidOperationException("AreInConflict method not found");
                return (bool)m.Invoke(null, new object[] { a, b })!;
            }
        }
    }
}
