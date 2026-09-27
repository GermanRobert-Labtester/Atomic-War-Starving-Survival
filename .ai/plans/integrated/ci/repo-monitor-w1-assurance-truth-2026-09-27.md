# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

## REPO-MONITOR-W1-ASSURANCE-TRUTH

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

> STATUS: APPROVED BY USER — 2026-09-27

## Outcome and boundaries

Implement the first package of the user-provided Deep Repository Monitoring & Assurance Plan: release-workflow parity, tracked repository-size growth, actual C# compile-set reachability, and test-only production-like source detection. Revalidate the older PR #70 measurements against the current tree. Do not change gameplay, migrate Unity sources, run full suites, auto-upgrade dependencies, accept the bloated branch as a cleaned size baseline, or commit volatile monitoring output.

Only edit new monitoring code and stable policy, the canonical gate manifest/release workflow and script, the fast CI workflow where needed, and focused monitor tests. Treat the unrelated dirty Core, `src/`, generated maps, and governance ledgers as other agents' work.

## Acceptance

1. Release policy is machine-readable in the existing canonical gate manifest; a focused parity check fails if the hosted release path omits or weakens any required gate, and the tag workflow invokes the canonical release command.
2. Repository-size monitoring measures tracked Git blobs and branch/PR growth without reading large prose bodies. New unapproved oversized Markdown and catastrophic generated growth fail; the existing bloated tree is reported without being ratified as a cleaned baseline.
3. C# source reachability uses evaluated production/test MSBuild compile items. Every tracked out-of-root source receives an explicit custody class, and newly unclassified or test-only production-like source fails.
4. All monitors emit bounded JSON tied to a commit and document the measured signal, confidence, false-positive/false-negative limits, threshold, cadence, owner, and remediation. At least one positive, negative, and baseline/false-positive fixture is tested per monitor. CI uploads output from an ignored report directory.
5. Run focused Go tests, manifest validation, a host build only if changed code requires one, and static workflow checks. Do not run full Core/release suites in this task.

## Signal contract

| Monitor | Authority and signal | Threshold / cadence / owner | Known evidence limit and remediation |
|---|---|---|---|
| Release parity | Required gate IDs in `CI_GATE_MANIFEST.json` against blocking tag-workflow execution and the canonical release script | No omitted or non-blocking required gate; PRs and tags; release integrator | Static workflow checks cannot prove hosted tools or exports actually succeed. Run the release policy on tags and inspect the gate artifact; fix missing workflow/script wiring, not a second checklist. |
| Repository size | Raw Git tree blob sizes for the proposed commit against its first-parent base, including new Markdown size and total tracked-blob growth | New Markdown >2 MiB needs an explicit exception; total net growth >100 MiB warns and >1 GiB fails; PRs; repository integrator | LFS pointers measure pointer bytes, not fetched payload; untracked local files are excluded; simultaneous removals can offset large additions in a net-growth budget. Keep LFS inventory separate and inspect added blobs before archiving. |
| Compile reachability | MSBuild-evaluated production/test `Compile` items crossed with tracked `.cs` files | No newly unclassified source; PRs; tooling integrator | Generated post-build source or dynamic loads are not proof of gameplay reachability. Classify or migrate a source only after reviewing its actual owner. |
| Test-only production source | Test-compiled minus production-compiled sources, excluding test fixtures | No new production-like test-only source beyond a named shrink-only debt baseline; PRs; tooling integrator | A source path alone cannot prove it should ship. Investigate its custody; do not add legacy source to production merely to make the gate green. |

## Execution log

Phase 0 — premise and worktree check: Complete. The local branch is newer than the supplied PR #70 checkpoint; unrelated worktree edits are present. Go tooling is under `tools/gotools`; C# compile graphs are evaluated by MSBuild rather than inferred from names.

Phase 1 — size and compile monitors: Implemented in `tools/gotools/pkg/monitor` with exact-path debt policy in `docs/ci/MONITORING_POLICY.json`. All 14 focused Go tests and `go vet` pass. The canonical runner's `repository_size_budget` gate passes. Local compile verification currently fails closed because two tracked `src/Host/*Mercenary*.cs` files have been deleted by concurrent work without a commit; it reports `tracked_source_missing_from_worktree`, not unclassified source debt. No change to those paths was made.

Phase 2 — release parity: Complete. The tag workflow invokes the canonical release script, runs the full Core and save-support gates through the manifest, exports Linux and Windows, and fails on missing artifacts. The Go static checker recognizes the required-gate manifest metadata and rejects missing, conditional, comment-only, or nonblocking release/export steps. No tag, release, or export command was run.

Phase 3 — focused verification: The existing manifest-drift xUnit class passed 7/7 through `scripts/run_test.sh`. Both Go packages pass their focused tests, `go vet`, and command builds. Manifest validation reports 63 total / 59 fast gates; the canonical runner passes the release-parity and size gates. The compile command writes a fail-closed local report for two tracked Mercenary source files deleted by unrelated concurrent work. This dirty checkout is not evidence that the clean tag/PR workflow fails. CI will run the compile monitor on its checked-out commit; no unrelated source was restored or exempted. JSON reports remain under ignored `build/reports/monitoring/`. Static release checks cannot prove that a hosted export succeeds; full Core/release/export runs were intentionally not executed.
