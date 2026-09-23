// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 38 / C1[11] host probe.
//
// --commitments-selftest proves, headlessly, that the commitment authority
// loads its authored catalog, evaluates the warning ladder, routes a missed
// obligation's consequence, settles a met obligation, and round-trips its
// captured state — the exact seams the host session and day owner use.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Commitments;
using Godot;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunCommitmentsSelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; GD.Print($"[PASS] commitments/{gate}"); }
                else { fail++; GD.Print($"[FAIL] commitments/{gate}{(note.Length > 0 ? " — " + note : "")}"); }
            }

            var io = CatalogPath.CreateFileIOForDataDir(dataDirectory);

            // 1 — authored catalog loads through the strict loader.
            var catalog = CommitmentCatalogLoader.Load(
                dataDirectory, io, new SystemTextJsonSerializer());
            Check("catalog_loads_without_errors", !catalog.HasErrors,
                string.Join("; ", catalog.Errors));
            Check("catalog_has_definitions", catalog.Commitments.Count >= 3,
                $"got {catalog.Commitments.Count}");

            var system = new CommitmentSystem();
            for (int i = 0; i < catalog.Commitments.Count; i++) system.RegisterCommitment(catalog.Commitments[i]);

            // 2 — warning ladder emits on the configured lead day.
            int warnings = 0;
            system.OnWarningIssued += _ => warnings++;
            var first = catalog.Commitments[0];
            int warningDay = first.due_day - first.warning_lead_days;
            var dayEvents = new List<DayStateChangeEvent>();
            system.TickDay(warningDay, dayEvents);
            Check("warning_emits_once", warnings == 1, $"warnings={warnings}");
            Check("warning_event_visible", dayEvents.Exists(e => e.Kind == "obligation_warning"));
            system.TickDay(warningDay, dayEvents);
            Check("warning_not_repeated", warnings == 1, $"warnings={warnings}");

            // 3 — missed obligation fires exactly once and routes its consequence.
            int missed = 0;
            var routed = new List<(string cls, string target, int magnitude)>();
            system.OnCommitmentMissed += _ => missed++;
            system.OnConsequenceRouted += (cls, target, magnitude) => routed.Add((cls, target, magnitude));
            dayEvents.Clear();
            system.TickDay(first.due_day + 1, dayEvents);
            Check("miss_emits_once", missed == 1, $"missed={missed}");
            Check("miss_event_visible", dayEvents.Exists(e => e.Kind == "obligation_missed"));
            Check("consequence_routed", routed.Count == 1,
                "expected exactly one consequence for the missed obligation");
            Check("consequence_payload_matches",
                routed.Count == 1
                && routed[0].cls == first.consequence_class
                && routed[0].target == first.consequence_target
                && routed[0].magnitude == first.consequence_magnitude);

            // 4 — terminal state is exactly-once: a later tick cannot re-fire.
            system.TickDay(first.due_day + 2, dayEvents);
            Check("missed_is_terminal", missed == 1 && routed.Count == 1);

            // 5 — met via partial payment/settlement; progress is bounded.
            var second = catalog.Commitments[1];
            bool met = false;
            system.OnCommitmentMet += _ => met = true;
            system.RecordProgress(second.id, second.target_quantity);
            Check("met_on_full_progress", met);
            Check("met_status_read_model",
                system.GetCommitment(second.id, second.due_day)?.Status == CommitmentStatus.Met);
            bool progressAfterTerminal = system.RecordProgress(second.id, 1);
            Check("met_is_terminal", !progressAfterTerminal);

            // 6 — save/restore round-trip parity over the terminal ledger.
            var captured = system.CaptureState();
            var restored = new CommitmentSystem();
            for (int i = 0; i < catalog.Commitments.Count; i++) restored.RegisterCommitment(catalog.Commitments[i]);
            restored.RestoreState(captured);
            bool parity = true;
            for (int i = 0; i < catalog.Commitments.Count; i++)
            {
                string id = catalog.Commitments[i].id;
                var before = system.GetCommitment(id, first.due_day + 2);
                var after = restored.GetCommitment(id, first.due_day + 2);
                if (before == null || after == null
                    || before.Status != after.Status
                    || before.CurrentQuantity != after.CurrentQuantity
                    || before.DaysRemaining != after.DaysRemaining)
                {
                    parity = false;
                    break;
                }
            }
            Check("save_restore_parity", parity);

            GD.Print($"[commitments-selftest] {pass} passed, {fail} failed");
            return fail == 0 ? 0 : 1;
        }
    }
}
