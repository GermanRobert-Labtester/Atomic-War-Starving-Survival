# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
# Plan 53 E1E follow-ups — map truth, receipts, capability claims

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

**STATUS: APPROVED BY USER**
**Package:** `E1/PLAN-53/E1E-FOLLOWUPS`
**Approval:** User authorized all three listed E1E follow-ups on 2026-10-01.

## Outcome

1. Reconcile the generated architecture map with current evidence: wire the
   live KitchenNutritionPanel cooking route, include NeedsPerformanceBridge as
   a survivor-needs projection, and retain the real NPC-memory UI gap while
   correcting the earlier review's false inference.
2. Select and independently review one additional plan candidate per E1E
   cluster; publish five completed duplicate-search receipts and include the
   confirmed cluster relations in the registry/report/register.
3. Add evidence-backed domain claims to `CLAIMS.json`, link them to clusters,
   and pass the existing claim validator.

## Non-goals

No runtime/gameplay implementation, new UI, new save section, plan status/body
changes, plan merge, or E1F work. A capability claim describes only behavior
proved by current sources and tests. NPC memory remains without a player panel;
NeedsPerformanceBridge remains a projection under persisted survivor state.

## Acceptance

- The architecture-map generator records the kitchen route and needs projection
  without inventing independent owners; generated map check passes.
- NPC-memory UI gap is documented as current and evidence-based.
- Five new candidate receipts contain the required search, authority, route,
  save, test, relation, conclusion, action, and date fields.
- Each additional candidate resolves uniquely in the plan register and appears
  under its reviewed cluster without status mutation.
- New claims have existing evidence/test paths and pass
  `verify-capability-claims.py --check`; cluster claim IDs resolve.
- Capability report, plan register, docs index, and focused governance and
  architecture-map tests pass.

## Owned files

`scripts/ci/generate-architecture-map.py`,
`scripts/ci/generate-capability-clusters.py`,
`Ashfall.Core.Tests/Tooling/PlanGovernanceContractTests.cs`,
`Ashfall.Core.Tests/Tooling/ArchitectureTestMapGateTests.cs`,
`docs/architecture/ARCHITECTURE_TEST_MAP.md`, `docs/architecture/CLAIMS.json`,
`docs/roadmap/e1/E1E_CAPABILITY_CLUSTERS.json`,
`docs/roadmap/CAPABILITY_CLUSTERS.md`,
`docs/roadmap/e1/E1E_INDEPENDENT_REVIEW.md`,
`docs/roadmap/e1/receipts/`, the E1E implementation log, docs index,
plan register, worktree claim, `.ai/state.md`, and `INTEGRATION_PLANS.md`.

## Verification

- `bash scripts/ci/generate-architecture-map.sh --check`
- `python3 scripts/ci/verify-capability-claims.py --check`
- `python3 scripts/ci/generate-capability-clusters.py --check`
- `python3 scripts/ci/generate-plan-register.py --check`
- `python3 scripts/ci/generate-docs-index.py --check`
- `bin/run-scoped-tests Ashfall.Core.Tests/Tooling/ArchitectureTestMapGateTests.cs`
- `bin/run-scoped-tests Ashfall.Core.Tests/Tooling/PlanGovernanceContractTests.cs`
- `git diff --check` on this package's tracked and new files.
