# D2 — Handoff

**Task:** Wave 8 Part 2, TASK D2 — Execute Recorded Decisions
**Status:** DONE
**Date:** 2026-09-17
**Next owner:** Foreman (ledger), plus the owners of the sibling candidates below.

## Outcome

- `SurvivorInspectionHostSession` + `SurvivorInspectionSnapshot` deleted after a
  fresh whole-tree zero-consumer proof; direct test fixture deleted; stale
  comment retargeted. Build 0/0; Survivors 227/227; Inventory 91/91.
- `DEBT-SURVIVOR-INSPECTION-ORPHAN` QUARANTINED → RETIRED/SEALED;
  `DEBT-186-INSPECTION-PROJECTION` annotated with the execution.
- New authoritative dead-data register for the 5 primary-wins distress rows;
  integrity gate emits exactly those 5 warnings.

## Sibling execution sweep (Phase 4 — candidates, NOT auto-executed)

| Debt row | State | Preconditions | Action needed |
|---|---|---|---|
| `DEBT-WORKTREE-DECLUTTER-2026-09-12` | QUARANTINED | tracked deletions + archive landing already recorded as done | foreman flip to RETIRED (confirm the archive is the intended final state) |
| `DEBT-TEST-QUARANTINE-2026-09-12` | QUARANTINED | 51 `Compile Remove` entries; next candidate `AutopsyProcedures` | Wave 9 Part 2 D3 bounded per-file promotion (per-file API/content proof) |
| `DEBT-GODOT-PARTIAL-REQUIRED` | ACCEPTED | documented source-generator contract (keep `partial`) | no execution — decision only |
| `DEBT-PLAN-SPRAWL` / `DEBT-RULEBOOK-SNAPSHOT` / `DEBT-PLANS170-199-PORTFOLIO` | ACCEPTED | process decisions | no execution |

## Remaining blockers

- The 5 expansion dead rows need **data-authority signature** to delete; default
  (register) is in force.

## Rollback

- Revert the commit; the deleted class is recoverable from git history. The
  register is additive documentation and safe to keep or drop.

## Verification rerun

```
dotnet build Ashfall.csproj
bash scripts/run_test.sh Ashfall.Core.Tests/Survivors
bash scripts/run_test.sh Ashfall.Core.Tests/Inventory
python3 scripts/ci/generate-architecture-map.py --check
python3 scripts/ci/generate-docs-index.py --check
godot --headless --path . -- --data-integrity-selftest
```
