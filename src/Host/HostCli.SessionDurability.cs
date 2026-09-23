// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 39 / C2[16] host probe.
//
// --session-durability-selftest proves, headlessly, the durability audit read
// model the host wires over the canonical save pipeline: slot capacity and
// isolation, interrupted-write auditing, backup recovery notification, soak
// sampling and stability verdicts, and capture/restore parity.
// ============================================================================
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Save;
using Godot;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunSessionDurabilitySelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; GD.Print($"[PASS] session-durability/{gate}"); }
                else { fail++; GD.Print($"[FAIL] session-durability/{gate}{(note.Length > 0 ? " — " + note : "")}"); }
            }

            var manager = new SessionDurabilityManager();

            // 1 — slot registration + capacity isolation.
            manager.State.MaxSlots = 2;
            Check("slot_registration", manager.RegisterOrUpdateSlot("slot_01", "Campaign Alpha", 10, 5, "chk_alpha"));
            Check("slot_registration_second", manager.RegisterOrUpdateSlot("slot_02", "Campaign Beta", 40, 8, "chk_beta"));
            Check("slot_capacity_enforced", !manager.RegisterOrUpdateSlot("slot_03", "Overflow", 1, 1, "chk_c"));
            Check("slot_isolation",
                manager.GetSlot("slot_01")!.Checksum == "chk_alpha"
                && manager.GetSlot("slot_02")!.Checksum == "chk_beta");

            // 2 — interrupted write is audited through the seam.
            string? reportedSlot = null;
            string? reportedReason = null;
            manager.CorruptionDetectedSeam = (slotId, reason) => { reportedSlot = slotId; reportedReason = reason; };
            Check("interrupted_write_recorded", manager.RecordInterruptedWrite("slot_01", "Power loss during flush"));
            Check("corruption_seam_fired", reportedSlot == "slot_01" && reportedReason == "Power loss during flush");
            Check("corrupt_flag_set", manager.GetSlot("slot_01")!.IsCorrupt);

            // 3 — recovery without backup is declined; with backup it restores.
            bool? restoredNotification = null;
            manager.BackupRestoredSeam = (slotId, recovered) => restoredNotification = recovered;
            Check("recovery_without_backup_declined", !manager.TryRecoverBackup("slot_01", out _));
            Check("decline_seam_fired", restoredNotification == false);
            Check("backup_created", manager.CreateBackup("slot_01", "chk_healthy"));
            Check("recovery_with_backup", manager.TryRecoverBackup("slot_01", out string recovered));
            Check("recovery_checksum", recovered == "chk_healthy" && !manager.GetSlot("slot_01")!.IsCorrupt);
            Check("restore_seam_fired", restoredNotification == true);

            // 4 — checksum verification failure funnels into the audit.
            Check("checksum_mismatch_detected", !manager.ValidateSlotChecksum("slot_02", "not_the_checksum"));
            Check("checksum_mismatch_audited", manager.GetSlot("slot_02")!.IsCorrupt);

            // 5 — soak sampling: stable series then an out-of-bounds sample.
            var stable = new SessionDurabilityManager();
            for (int day = 1; day <= 10; day++)
                stable.RecordDayAdvance(day, 120f + day, 2_000_000L + (day * 1000L));
            var stableReport = stable.EvaluateSoakStability();
            Check("soak_stable_series", stableReport.IsMonotonicallyStable, stableReport.StabilityVerdict);
            Check("soak_sample_count", stableReport.TotalDaysSampled == 10);
            stable.RecordDayAdvance(11, 9_000f, 2_100_000L);
            var unstableReport = stable.EvaluateSoakStability();
            Check("soak_instability_detected", !unstableReport.IsMonotonicallyStable, unstableReport.StabilityVerdict);

            // 6 — capture/restore parity over slots + soak + corruption audit.
            var captured = stable.CaptureState();
            var restoredManager = new SessionDurabilityManager();
            restoredManager.RestoreState(captured);
            Check("capture_restore_slot_count", restoredManager.Slots.Count == stable.Slots.Count);
            Check("capture_restore_samples", restoredManager.SoakSamples.Count == stable.SoakSamples.Count);
            Check("capture_restore_verdict",
                restoredManager.EvaluateSoakStability().IsMonotonicallyStable
                == unstableReport.IsMonotonicallyStable);

            GD.Print($"[session-durability-selftest] {pass} passed, {fail} failed");
            return fail == 0 ? 0 : 1;
        }
    }
}
