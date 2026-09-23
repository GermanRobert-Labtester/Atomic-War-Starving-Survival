// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Governance;
using Godot;

namespace AtomicWar.GodotApp
{
    // ========================================================================
    // Plan 59 / Task 59A host probe — the retrospective's own gate.
    //
    // --standing-gates-selftest loads the 22-row standing-gate register and
    // measures the only column that matters: each rule must be enforced by a
    // real, critical, tier-compatible gate in docs/ci/CI_GATE_MANIFEST.json, or
    // carry a written, owned decision not to gate it. "Gates exist" is not
    // "gates run" — this probe is what makes that measurable.
    // ========================================================================
    public static partial class HostCli
    {
        public static int RunStandingGatesSelfTest(string dataDirectory, string repoRoot)
        {
            int pass = 0, fail = 0;

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; GD.Print($"[PASS] standing-gates/{gate}"); }
                else { fail++; GD.Print($"[FAIL] standing-gates/{gate}{(note.Length > 0 ? " — " + note : "")}"); }
            }

            var files = new FileSystemIO();

            // ── 1 — the register loads through the strict path.
            var session = new StandingGatesHostSession();
            bool registered = session.LoadRegister(dataDirectory, files);
            Check("standing_gate_register_loads", registered && session.IsLoaded,
                string.Join("; ", session.Errors));
            if (!registered)
            {
                GD.Print($"[standing-gates-selftest] {pass} passed, {fail} failed");
                return fail == 0 ? 0 : 1;
            }

            // ── 2 — the repo's own enforcement authority is readable.
            string manifestPath = files.Combine(
                files.Combine(repoRoot, "docs"), "ci");
            manifestPath = files.Combine(manifestPath, "CI_GATE_MANIFEST.json");
            bool manifestBound = session.LoadEnforcementManifest(manifestPath, files);
            Check("enforcement_manifest_reads", manifestBound,
                string.Join("; ", session.Errors));

            // ── 3 — 22 finding classes, one row each, every one owned.
            int bindingCount = session.Bindings.Count;
            Check("register_covers_twenty_two_findings", bindingCount == 22,
                $"rows={bindingCount}");
            bool everyRowOwned = true, everyRowGatedOrRuled = true;
            foreach (var binding in session.Bindings)
            {
                if (string.IsNullOrWhiteSpace(binding.Def.OwnerRole)) everyRowOwned = false;
                if (!binding.IsGated && string.IsNullOrWhiteSpace(binding.NonGateRule)) everyRowGatedOrRuled = false;
            }
            Check("every_gate_has_an_owner", everyRowOwned,
                "an unowned gate is the reason three were red while every plan claimed green (59A step 2)");
            Check("every_gate_is_gated_or_deliberately_unruled", everyRowGatedOrRuled,
                "59A step 6: write the decision not to gate it");

            // ── 4 — count the enforcement split before auditing it.
            int gated = 0, ruleOnly = 0;
            foreach (var binding in session.Bindings)
            {
                if (binding.IsGated) gated++;
                else ruleOnly++;
            }
            GD.Print($"[standing-gates-selftest] register: {session.Bindings.Count} rows — "
                + $"{gated} gated, {ruleOnly} deliberately rule-only");

            // ── 5 — audit: passing means enforced-by-a-real-gate or rule-only.
            var report = session.Audit();
            Check("audit_measured_all_enforced_gates",
                report.EvaluatedGates == session.Bindings.Count,
                $"evaluated={report.EvaluatedGates} rows={session.Bindings.Count}");
            Check("audit_reports_every_gate_it_evaluated",
                report.TotalGates == session.Bindings.Count && report.EnforcedGates == session.Bindings.Count,
                $"total={report.TotalGates} enforced={report.EnforcedGates}");

            bool anyUnbound = false, anyNotBlocking = false, anyTierMismatch = false;
            foreach (var status in session.Statuses)
            {
                switch (status.Verdict)
                {
                    case StandingGateVerdict.Unbound: anyUnbound = true; break;
                    case StandingGateVerdict.NotBlocking: anyNotBlocking = true; break;
                    case StandingGateVerdict.TierMismatch: anyTierMismatch = true; break;
                }
            }
            Check("no_standing_gate_names_a_nonexistent_gate", !anyUnbound,
                session.DescribeViolations(StandingGateVerdict.Unbound));
            Check("no_standing_gate_binds_an_informational_gate", !anyNotBlocking,
                session.DescribeViolations(StandingGateVerdict.NotBlocking));
            Check("no_standing_gate_has_a_tier_mismatch", !anyTierMismatch,
                session.DescribeViolations(StandingGateVerdict.TierMismatch));
            Check("all_standing_gates_pass", report.AllGatesPassing,
                session.DescribeViolations());

            // ── 6 — self-proofs: the register's own gates can fail.
            bool ownerlessRejected = StandingGatesHostSession.BuildOwnerlessRegister(dataDirectory, files);
            bool unrulledRejected = StandingGatesHostSession.BuildUnruledRegister(dataDirectory, files);
            bool emptyRejected = StandingGatesHostSession.BuildEmptyRegister(dataDirectory, files);
            bool unknownTierRejected = StandingGatesHostSession.BuildUnknownTierRegister(dataDirectory, files);
            Check("ownerless_gate_rejected", ownerlessRejected,
                "an unowned gate is exactly the failure this plan closes (59A step 2)");
            Check("unwritten_non_gate_decision_rejected", unrulledRejected,
                "59A step 6: a decision not to gate must be written");
            Check("empty_register_rejected", emptyRejected);
            Check("unknown_tier_rejected", unknownTierRejected,
                "a tier is never silently defaulted (59A step 1)");

            // ── 7 — self-proof: a reference to a nonexistent CI gate is measured,
            //       not silently green.
            var unboundProbe = new StandingGatesHostSession();
            unboundProbe.LoadRegister(dataDirectory, files);
            unboundProbe.LoadEnforcementManifest(manifestPath, files);
            unboundProbe.AuditWithEnforcementOverride(
                "gate_port_contract_uncalled", "a_gate_id_that_does_not_exist");
            bool unboundDetected = unboundProbe.CountOf(StandingGateVerdict.Unbound) > 0;
            Check("nonexistent_enforcement_ref_detected", unboundDetected,
                "a standing gate naming a gate that is not in the manifest must be reported unbound");

            // ── 8 — a tier-mismatched binding is a measured violation, not silent.
            var mismatched = new StandingGatesHostSession();
            mismatched.LoadRegister(dataDirectory, files);
            mismatched.LoadEnforcementManifest(manifestPath, files);
            var mismatchReport = mismatched.AuditWithTierOverride(
                StandingGatesHostSession.RequiredClassification(GateTier.PerPush),
                "full");
            Check("per_push_rule_needs_fast_gate",
                mismatched.CountOf(StandingGateVerdict.TierMismatch) > 0
                || mismatched.Statuses.Count == 0,
                $"per-push rules now require 'full', which no fast-gated per-push rule can satisfy — "
                + $"{mismatched.CountOf(StandingGateVerdict.TierMismatch)} mismatch(es) detected");
            Check("tier_override_still_reports_all_rows",
                mismatched.Statuses.Count == session.Bindings.Count,
                $"{mismatched.Statuses.Count} vs {session.Bindings.Count}");

            // ── 9 — the register is deterministic: two audits agree.
            var again = new StandingGatesHostSession();
            again.LoadRegister(dataDirectory, files);
            again.LoadEnforcementManifest(manifestPath, files);
            var second = again.Audit();
            Check("audit_is_deterministic",
                second.PassingGates == report.PassingGates
                && second.EvaluatedGates == report.EvaluatedGates,
                $"{report.PassingGates}/{report.EvaluatedGates} vs {second.PassingGates}/{second.EvaluatedGates}");

            GD.Print($"[standing-gates-selftest] {pass} passed, {fail} failed");
            GD.Print($"[standing-gates-selftest] {session.SummaryLine()}");
            GD.Print($"[standing-gates-selftest] audit: {report.PassingGates}/{report.EvaluatedGates} passing "
                + $"(total={report.TotalGates}, enforced={report.EnforcedGates})");
            foreach (var status in session.Statuses)
                GD.Print($"[standing-gates-selftest]   {status.Describe()}");
            return fail == 0 ? 0 : 1;
        }
    }
}
