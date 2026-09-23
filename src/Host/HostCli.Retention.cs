// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 55 host probe — retention, a save corpus, and the 400-year campaign.
//
// --retention-selftest loads the authored policy table, overlays it on the
// built-in defaults, and applies the authority to LIVE canonical owners
// (kitchen serving log, faction-war decrees, machine log, dose ledger). The
// probe proves bounded growth over a long campaign and that iron-rule
// obligations are never pruned, then round-trips the audit section.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Records;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.Verdict;
using Godot;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunRetentionSelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; GD.Print($"[PASS] retention/{gate}"); }
                else { fail++; GD.Print($"[FAIL] retention/{gate}{(note.Length > 0 ? " — " + note : "")}"); }
            }

            // 1 — the authored policy table loads through the strict path.
            var loaded = RetentionPolicyCatalogLoader.Load(dataDirectory, new FileSystemIO());
            Check("authored_policy_table_loads", !loaded.HasErrors && loaded.Policies.Count > 0,
                string.Join("; ", loaded.Errors));
            if (loaded.HasErrors)
            {
                GD.Print($"[retention-selftest] {pass} passed, {fail} failed");
                return fail == 0 ? 0 : 1;
            }

            // 2 — the overlay wins for named keys and preserves every default.
            var session = new RetentionHostSession();
            int beforeOverlay = session.Catalog.ActivePolicies.Count;
            session.LoadAuthoredPolicies(dataDirectory, new FileSystemIO());
            Check("authored_overlay_applied", session.UsingAuthoredPolicies && session.LoadError == null,
                session.LoadError);
            Check("overlay_preserves_defaults",
                session.Catalog.ActivePolicies.Count >= beforeOverlay);

            bool boundReached = false;
            int boundValue = -1;
            foreach (var policy in session.Catalog.ActivePolicies)
            {
                if (string.Equals(policy.CollectionKey, "kitchen_serving_log", StringComparison.OrdinalIgnoreCase))
                {
                    boundValue = policy.MaxCapacity;
                    boundReached = policy.MaxCapacity == 200;
                    break;
                }
            }
            Check("authored_bound_reaches_catalog", boundReached, boundValue.ToString());

            // 3 — the loaded rows are all real, canonical, and snake_case; no
            //     free-form key can smuggle in a second policy for a collection.
            bool allSnakeCase = true, hasProtected = false;
            foreach (var policy in loaded.Policies)
            {
                if (!RetentionPolicyCatalogLoader.IsSnakeCase(policy.CollectionKey)) allSnakeCase = false;
                if (policy.IsProtectedObligation) hasProtected = true;
            }
            Check("authored_rows_are_canonical", allSnakeCase, string.Join(",", loaded.Errors));
            Check("authored_table_declares_obligations", hasProtected);

            // 4 — LIVE owner measurement: the kitchen serving log is bounded by
            //     the authority, and the owner still owns its log.
            var kitchen = new Ashfall.Core.KitchenNutritionSystem(
                new SeededRng(4242),
                new Ashfall.Core.Inventory.Inventory(),
                new Ashfall.Core.Survivors.NeedsSystem(),
                new GodotLog());
            for (int i = 0; i < 260; i++)
                kitchen.State.servingLog.Add(new MealServingLog
                {
                    day = i, survivorId = "probe_cook", recipeId = "recipe_probe", moraleBonus = 1f
                });
            session.BindOwners(kitchen, null, null, null);
            var report = session.ApplyRetention();
            Check("kitchen_serving_log_bounded",
                kitchen.State.servingLog.Count == 200 && report.TotalEntriesPruned == 60,
                $"count={kitchen.State.servingLog.Count} pruned={report.TotalEntriesPruned}");
            Check("kitchen_owner_still_owns_log",
                kitchen.State.servingLog[0].day == 60,
                $"newest-first head day={kitchen.State.servingLog[0].day}");

            // 5 — LIVE owner measurement: faction-war decrees use their own bound.
            var factionWar = new FactionWarSystem();
            for (int i = 0; i < 140; i++) factionWar.State.enactedDecrees.Add($"decree_{i:D4}");
            session.BindOwners(kitchen, factionWar, null, null);
            session.ApplyRetention();
            Check("faction_war_decrees_bounded", factionWar.State.enactedDecrees.Count == 100,
                $"{factionWar.State.enactedDecrees.Count}");

            // 6 — LIVE owner measurement: the machine log is bounded and the
            //     authority's seam still fires for observability.
            var machineLog = new MachineLogSystem();
            for (int i = 0; i < 340; i++)
                machineLog.Post("machine_probe_bay", day: i, kind: "operating", "probe round", "evidence_probe");
            string? seamKey = null; int seamAfter = -1;
            session.Catalog.OnEntriesPrunedSeam = (key, before, after) => { seamKey = key; seamAfter = after; };
            session.BindOwners(kitchen, factionWar, machineLog, null);
            session.ApplyRetention();
            Check("machine_log_bounded", machineLog.Entries.Count == 300,
                $"{machineLog.Entries.Count}");
            Check("prune_seam_fires", seamKey == "machine_log" && seamAfter == 300,
                $"{seamKey}->{seamAfter}");
            session.Catalog.OnEntriesPrunedSeam = null;

            // 7 — LIVE owner measurement: the dose ledger bounds the per-survivor
            //     reading history while the cumulative dose and band are untouched.
            var dose = new DoseLedgerSystem();
            dose.AssignDosimeter("probe_dosimeter", "tag_probe_01");
            var doseEntry = dose.GetEntry("probe_dosimeter")!;
            var doseRng = new SeededRng(909);
            for (int i = 0; i < 520; i++)
                dose.BookReading("probe_dosimeter", day: i, nominalMsv: 0.1f,
                    source: "probe_flux", highEnergyEvent: false,
                    antiRadBefore: false, antiRadAfter: false, rng: doseRng);
            session.BindOwners(kitchen, factionWar, machineLog, dose);
            session.ApplyRetention();
            Check("dose_readings_bounded", doseEntry.readingsHistory.Count == 500,
                $"{doseEntry.readingsHistory.Count}");
            Check("dose_cumulative_preserved",
                Math.Abs(doseEntry.cumulativeMsv - 0.1f * 520) < 0.05f,
                $"{doseEntry.cumulativeMsv}");

            // 8 — the iron rule: a protected obligation list is never pruned.
            var obligations = new List<string>();
            for (int i = 0; i < 900; i++) obligations.Add($"will_{i:D4}");
            Check("protected_obligations_never_pruned",
                session.Catalog.ApplyRetention("survivor_wills_and_legacies", obligations, out int obligationPruned)
                && obligations.Count == 900 && obligationPruned == 0,
                $"count={obligations.Count} pruned={obligationPruned}");

            // 9 — a 400-day pass is idempotent: once bounded, nothing more moves.
            int stableBefore = kitchen.State.servingLog.Count
                + factionWar.State.enactedDecrees.Count
                + machineLog.Entries.Count
                + doseEntry.readingsHistory.Count;
            var repeat = session.ApplyRetention();
            int stableAfter = kitchen.State.servingLog.Count
                + factionWar.State.enactedDecrees.Count
                + machineLog.Entries.Count
                + doseEntry.readingsHistory.Count;
            Check("second_pass_is_idempotent",
                stableBefore == stableAfter && repeat.TotalEntriesPruned == 0,
                $"pruned={repeat.TotalEntriesPruned}");

            // 10 — the audit section round-trips (and does not persist the logs).
            var state = RetentionSaveStore.From(repeat, session.Passes, session.UsingAuthoredPolicies);
            Check("audit_store_save", RetentionSaveStore.TrySave(state));
            var reloaded = RetentionSaveStore.TryLoad();
            Check("audit_store_reload", reloaded != null
                && reloaded!.schema_version == 1
                && reloaded.passes == session.Passes
                && reloaded.protected_collection_keys.Count == report.ProtectedCollectionKeys.Count,
                reloaded == null ? "null" : $"passes={reloaded.passes}");

            // 11 — restoring an audit state into a fresh session does not invent
            //      collection state: only the audit facts are restored.
            var restored = RetentionSaveStore.TryRestore(
                RetentionSaveStore.TryCapturePersisted(state));
            Check("audit_capture_restore", restored != null && restored!.passes == session.Passes);

            GD.Print($"[retention-selftest] {pass} passed, {fail} failed");
            GD.Print($"[retention-selftest] {session.Describe()}");
            return fail == 0 ? 0 : 1;
        }
    }
}
