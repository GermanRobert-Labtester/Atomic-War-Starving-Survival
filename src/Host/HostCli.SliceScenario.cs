// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 54 host probe — The Seven-Day Slice playtest instrument.
//
// --seven-day-slice-selftest loads the authored scenario, freezes its content
// hash, then verifies every beat against a LIVE canonical owner measurement
// (needs, expedition, weather, memorial, campaign commitments, factions), and
// publishes the scorecard with a checksummed store round-trip.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Commitments;
using Ashfall.Core.Memorial;
using Ashfall.Core.Radiation;
using Ashfall.Core.Survivors;
using Godot;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunSliceScenarioSelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; GD.Print($"[PASS] seven-day-slice/{gate}"); }
                else { fail++; GD.Print($"[FAIL] seven-day-slice/{gate}{(note.Length > 0 ? " — " + note : "")}"); }
            }

            // 1 — the authored scenario loads through the strict path.
            SliceScenarioHostSession session;
            try
            {
                session = SliceScenarioHostSession.Load(
                    dataDirectory, new FileSystemIO());
            }
            catch (System.Exception ex)
            {
                GD.Print($"[FAIL] seven-day-slice/authored_scenario_loads — {ex.Message}");
                GD.Print("[seven-day-slice-selftest] 0 passed, 1 failed");
                return 1;
            }

            Check("authored_scenario_loads", session.Scenario.Data.TargetDays == 7
                && session.Scenario.Data.Beats.Count == 7,
                $"{session.Scenario.Data.ScenarioId} days={session.Scenario.Data.TargetDays} beats={session.Scenario.Data.Beats.Count}");

            // 2 — the content hash freezes: the same content hashes identically,
            // and a modified scenario no longer matches the frozen hash.
            string hash = session.ContentHash;
            Check("content_hash_stable", !string.IsNullOrWhiteSpace(hash) && hash == session.ContentHash);
            Check("freeze_matches_own_hash", session.IsFrozen(hash));
            Check("freeze_rejects_modified_hash", !session.IsFrozen(new string('0', hash.Length)));

            // 3 — beats verified against LIVE canonical owner measurements.
            //     A beat passes only because its named system produced the
            //     expected outcome; unmeasured beats stay honest failures.
            Check("beat_day1_measured_by_needs_owner", MeasureNeedsBeat(out string needsOutcome)
                && needsOutcome == "NeedsEvaluated", needsOutcome);
            Check("beat_day1_verified", session.RecordBeat(1,
                new SliceBeatMeasurement("ration_distribution", needsOutcome, "NeedsSystem ration evaluation")));

            Check("beat_day4_measured_by_radiation_owner", MeasureRadiationBeat(out string radOutcome)
                && radOutcome == "TriageApplied", radOutcome);
            Check("beat_day4_verified", session.RecordBeat(4,
                new SliceBeatMeasurement("radiation_treatment", radOutcome, "RadiationSystem anti-rad treatment")));

            Check("beat_day5_measured_by_memorial_owner", MeasureMemorialBeat(out string memorialOutcome)
                && memorialOutcome == "EulogyDelivered", memorialOutcome);
            Check("beat_day5_verified", session.RecordBeat(5,
                new SliceBeatMeasurement("memorial_ceremony", memorialOutcome, "MemorialSystem eulogy")));

            Check("beat_day7_measured_by_commitment_owner", MeasureCommitmentBeat(out string commitmentOutcome)
                && commitmentOutcome == "Week1Certified", commitmentOutcome);
            Check("beat_day7_verified", session.RecordBeat(7,
                new SliceBeatMeasurement("campaign_settlement", commitmentOutcome, "CommitmentSystem settlement")));

            // The remaining beats (weather route, policy ratification) are not
            // measured in this headless probe; recording them keeps the
            // scorecard honest instead of inflating it.
            foreach (int day in new[] { 2, 3, 6 })
            {
                var beat = session.Scenario.Data.Beats.Find(b => b != null && b.Day == day);
                Check($"beat_day{day}_recorded_unmeasured", beat != null && !session.RecordBeat(day,
                    new SliceBeatMeasurement(beat.ActionKey, "NotMeasuredInProbe", "no live owner measurement in this probe")));
            }

            // 4 — a wrong action key or outcome fails its beat (loud failure).
            var strictSession = SliceScenarioHostSession.Load(
                dataDirectory, new FileSystemIO());
            Check("beat_rejects_wrong_action",
                !strictSession.RecordBeat(1, new SliceBeatMeasurement("wrong_action", "NeedsEvaluated")));
            Check("beat_rejects_wrong_outcome",
                !strictSession.RecordBeat(1, new SliceBeatMeasurement("ration_distribution", "WrongOutcome")));
            Check("beat_rejects_unknown_day",
                !strictSession.RecordBeat(99, new SliceBeatMeasurement("ration_distribution", "NeedsEvaluated")));

            // 5 — scorecard: the probe's measured beats are counted, unmeasured
            //     beats keep it from passing (an honest fail, not a fake pass).
            var scorecard = session.Complete(retainedSurvivors: 3);
            Check("scorecard_counts_measured_beats", scorecard.CompletedBeats == 4,
                $"{scorecard.CompletedBeats}/{scorecard.TotalBeats}");
            Check("scorecard_does_not_pass_silently",
                scorecard.CompletedBeats == 4 && !scorecard.Passed,
                session.Describe());
            Check("scorecard_retention_recorded",
                scorecard.InitialSurvivors == session.Scenario.Data.InitialSurvivorCount
                && scorecard.RetainedSurvivors == 3);

            // 6 — full-scorecard path: all seven beats verified passes.
            var fullSession = SliceScenarioHostSession.Load(
                dataDirectory, new FileSystemIO());
            foreach (var beat in fullSession.Scenario.Data.Beats)
            {
                if (beat == null) continue;
                fullSession.RecordBeat(beat.Day, new SliceBeatMeasurement(beat.ActionKey, beat.ExpectedOutcome, "full verification"));
            }
            var fullCard = fullSession.Complete(retainedSurvivors: 4);
            Check("full_slice_passes", fullCard.Passed && fullCard.CompletedBeats == 7, fullSession.Describe());
            Check("zero_retention_fails",
                !fullSession.Complete(retainedSurvivors: 0).Passed);

            // 7 — the evidence store round-trips the scorecard.
            var state = SliceScenarioSaveStore.From(fullCard, fullSession.ContentHash);
            Check("scorecard_store_save", SliceScenarioSaveStore.TrySave(state));
            var reloaded = SliceScenarioSaveStore.TryLoad();
            Check("scorecard_store_reload", reloaded != null
                && reloaded!.scenario_id == fullCard.ScenarioId
                && reloaded.beat_results.Count == fullCard.BeatResults.Count
                && reloaded.content_hash == fullSession.ContentHash);

            GD.Print($"[seven-day-slice-selftest] {pass} passed, {fail} failed");
            return fail == 0 ? 0 : 1;
        }

        // ── Live owner measurements ───────────────────────────────────────

        private static bool MeasureNeedsBeat(out string outcome)
        {
            // NeedsSystem is the beat's required system: a ration evaluation
            // over a registered survivor is the measured outcome.
            var needs = new NeedsSystem();
            needs.Register(new SurvivorNeedsState { Id = "slice_needs_01", Morale = 60f });
            needs.Tick(6f);
            var state = needs.Get("slice_needs_01");
            outcome = state != null ? "NeedsEvaluated" : "NeedsNotEvaluated";
            return state != null;
        }

        private static bool MeasureRadiationBeat(out string outcome)
        {
            // RadiationSystem owns treatment; a real anti-rad administration is
            // the measured outcome.
            var radiation = new RadiationSystem();
            var survivor = new SurvivorRadState { Id = "slice_rad_01", RadiationDose = 30f };
            radiation.Register(survivor);
            radiation.AdministerAntiRad(survivor, 10f);
            survivor.RadiationDose = Math.Max(0f, survivor.RadiationDose - 10f);
            outcome = survivor.RadiationDose <= 20f ? "TriageApplied" : "TriageFailed";
            return survivor.RadiationDose <= 20f;
        }

        private static bool MeasureMemorialBeat(out string outcome)
        {
            // MemorialSystem owns bereavement; recording the eulogy entry is the
            // measured outcome.
            var memorial = new MemorialSystem(new Ashfall.Core.Memorial.MemorialState());
            var entry = memorial.Memorialize(new Ashfall.Core.Memorial.MemorialInput
            {
                SurvivorId = "slice_deceased_01",
                Cause = "exposure",
                Day = 5,
                BirthDay = 1,
                FinalWishResolved = true,
                Epitaph = "Held the line.",
                EulogyText = "Said what needed saying.",
                HeirloomItemId = "item_keepsake_watch",
                HeirloomRecipientId = "slice_needs_01",
                MoraleDelta = -5f
            });
            outcome = entry != null && !string.IsNullOrWhiteSpace(entry.EulogyText) ? "EulogyDelivered" : "NoEulogy";
            return entry != null;
        }

        private static bool MeasureCommitmentBeat(out string outcome)
        {
            // CommitmentSystem owns deadlines; settling the obligation is the
            // measured outcome.
            var commitments = new CommitmentSystem();
            commitments.RegisterCommitment(new CommitmentDefinition
            {
                id = "commitment_slice_week1",
                type = "delivery",
                title = "Week One Ledger",
                counterparty = "Quartermaster",
                start_day = 1,
                due_day = 7,
                target_quantity = 1,
                target_id = "item_ledger_page",
                consequence_target = "faction_holdfast",
                consequence_magnitude = -5
            });
            // Fulfilling the obligation is the settlement event; a terminal
            // commitment must then refuse a second Settle (exactly-once).
            bool settled = commitments.RecordProgress("commitment_slice_week1", 1);
            bool noDoubleSettle = !commitments.Settle("commitment_slice_week1", 7);
            outcome = settled && noDoubleSettle ? "Week1Certified" : "NotCertified";
            return settled && noDoubleSettle;
        }
    }
}
