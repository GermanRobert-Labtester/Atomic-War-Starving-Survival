# E1E implementation log — duplicate-topic clusters and live capabilities

**Date:** 2026-10-01
**Package:** `E1/PLAN-53/E1E-CAPABILITY-CLUSTERS`
**Status:** Complete for E1E; later E1 phases remain open.

## Delivered

- Added five reviewed clusters covering information flow, NPC episodic memory,
  shelter governance, needs-to-performance, and the food pipeline.
- Mapped each cluster to current Core, host, save-section, and test evidence;
  recorded distinct related systems and stale architecture-map entries for
  future documentation maintenance.
- Added a duplicate-search receipt format and extension-before-invention rule.
- Added deterministic candidate signals to the generated plan register and a
  generated capability-cluster report. Signals and reviewed membership never
  mutate plan status or merge plans.
- Added generator validation for member IDs, authority paths, save sections,
  claim IDs, and receipt fields. Added generator self-tests and a focused
  governance contract.
- No runtime, gameplay, save schema, UI, data catalog, plan body, or plan-status
  changes.

## Verification

- `python3 scripts/ci/generate-capability-clusters.py --write` — PASS; five
  reviewed clusters generated.
- `python3 scripts/ci/generate-capability-clusters.py --self-test` — PASS.
- `python3 scripts/ci/generate-capability-clusters.py --check` — PASS.
- `python3 scripts/ci/generate-plan-register.py --check` — PASS.
- `python3 scripts/ci/generate-docs-index.py` — PASS; 5,618 documents indexed.
- `python3 scripts/ci/generate-docs-index.py --check` — PASS; 5,618 documents
  verified.
- `bin/run-scoped-tests Ashfall.Core.Tests/Tooling/PlanGovernanceContractTests.cs`
  — PASS; 6/6.
- `git diff --check` on changed tracked package paths — PASS.

## Limits and handoff

The reviewed set is five seeded overlap examples, not a final human disposition
of every candidate signal in the corpus. The deterministic lexical section is a
search aid only. No commit was created; unrelated dirty worktree changes were
preserved; the full test suite was not run.

## Accepted E1E follow-ups — FULLY INTEGRATED (2026-10-01)

1. Reconciled the generated architecture map: added the live kitchen panel and
   food-loop selftest to `cooking`, added `NeedsPerformanceBridge` and its host
   projection under `survivors`, and kept NPC memory's current `None (GAP)` UI
   status. The independent review records why the earlier gap inference was
   incorrect.
2. Reviewed one additional candidate in each cluster and published five
   candidate-specific duplicate-search receipts. Added each stable plan ID and
   relation to the cluster registry/report/register without modifying source
   plan status or body.
3. Added five evidence-backed claims for rumor authority, NPC memory
   persistence, shelter governance, needs performance projection, and the
   kitchen nutrition route. `CLAIMS.json` now validates 33 claims.

Verification: capability-cluster self-test and claim validator PASS;
`ArchitectureTestMapGateTests` 7/7 PASS; `PlanGovernanceContractTests` 6/6
PASS; candidate host tests PASS (Rumor 7/7, NPC memory 9/9, governance 10/10,
needs performance 6/6, cooking 8/8). The first map-test attempt exposed an
ambiguous subsystem/status row and a route rendered in the route detail section;
the assertion now targets those generated locations directly. Generated report,
register, architecture map, and docs index checked after regeneration. No runtime,
gameplay, save schema, player UI, or plan-status changes.

## Suggested next small follow-ups

1. Teach `generate-capability-clusters.py` to validate candidate receipt
   headings and plan-ID references, not only receipt-file existence.
2. Run the existing `--food-loop-selftest` as a focused runtime proof for the
   kitchen route claim and record the result in its receipt.
3. Recheck Plan 24's stale implementation premise against the live modifier
   stack, performance bridge, and duty consumers, then amend only its receipt
   if the current relation needs refinement.
