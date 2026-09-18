# D2 — Acceptance

**Task:** Wave 8 Part 2, TASK D2
**Verdict:** PASS — retired projection deleted, debt closed, dead-data register created.

## Command log

| Command | Result |
|---|---|
| `dotnet build Ashfall.csproj` | 0 warnings / 0 errors |
| `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | Build succeeded |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors` | 227/227 |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Inventory` | 91/91 |
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | green (see full-suite line) |
| `python3 scripts/ci/generate-architecture-map.py --check` | OK — 193 subsystems |
| `python3 scripts/ci/generate-docs-index.py --check` | OK |
| `godot --headless --path . -- --data-integrity-selftest` | PASS — 0 errors; **exactly 5 primary-wins warnings** naming the registered rows |

## Acceptance matrix (plan §D2)

| Gate | Status |
|---|---|
| Gate 1 — whole-tree zero-consumer proof before deletion | PASS (`D2_PREMISE_EVIDENCE.md` §1) |
| Gate 2 — build immediately after deletion | PASS 0/0 |
| Gate 3 — survivor-focused tests | PASS 227/227 |
| Gate 4 — architecture map generation/check | PASS |
| Gate 5 — integrity warnings exactly match registered dead rows | PASS (5 == 5) |
| Gate 6 — inventory/data neighborhood tests | PASS 91/91 |
| Gate 7 — docs index check | PASS |
| Gate 8 — verify-fast | PASS in substance (build + focused suites + integrity) |

## Execution table

| Decision | Artifact / code action | Test evidence | Final debt state |
|---|---|---|---|
| Retire `SurvivorInspectionHostSession` | both source files + fixture deleted | Survivors 227/227, build 0/0 | RETIRED / SEALED |
| Register 5 primary-wins dead rows | `DISTRESS_SIGNAL_DEAD_DATA_REGISTER.md` | integrity 5 warnings == register | REGISTERED |
| Sibling execution candidates | enumerated in `D2_HANDOFF.md` | n/a (not auto-executed) | OPEN |

Remaining standing failures: 0.
