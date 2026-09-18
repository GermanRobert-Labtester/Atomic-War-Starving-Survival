# D3 — Change Matrix

**Task:** Wave 8 Part 2, TASK D3
**Nature:** documentation/classification only — **no production or test code changed**.

| # | Path | Change | Reason | Verification |
|---|---|---|---|---|
| 1 | `docs/ui/UI_NODE_DIAGNOSTICS_AND_LEAK_TRIAGE.md` | new §4 "Shutdown Resource/RID Warnings — Classified Signatures" + §5 best practices renumber | record the exact benign signature and the distinguisher from a real leak | docs index check |
| 2 | `docs/plans/PLAN_24_CLOSEOUT.md` | the "routed to D3" bullet → classified disposition | close the routed debt with evidence | docs index check |
| 3 | `docs/plans/wave8_part2/D3_{PREMISE_EVIDENCE,CHANGE_MATRIX,ACCEPTANCE,HANDOFF}.md` | new | required deliverables | n/a |
| 4 | `WORKTREE_OWNERSHIP.md` / `INTEGRATION_PLANS.md` | D3 claim row + note | governance | n/a |

## Explicit non-changes

- No `src/UI/**`, `src/Host/**`, `Assets/Ashfall.Core/**` change.
- No global exit-time cleanup added (forbidden).
- No rendering-policy or disposal change → visible output provably unchanged, so
  no snapshot re-baseline is required (Gate 7 is trivially satisfied).

## Why no lifetime repair

The owner-lifetime repair path (Phase 2) is **not** triggered: telemetry is flat
and the real lifecycle path is clean. The remaining signature is the
diagnostic-harness specimen lifetime, which D3 permits documenting as benign
with evidence.
