# D2 — Change Matrix

| # | Path | Change | Reason | Verification |
|---|---|---|---|---|
| 1 | `Assets/Ashfall.Core/Survivors/SurvivorInspectionHostSession.cs` | **deleted** | zero-consumer retired duplicate projection (re-proved) | build 0/0 |
| 2 | `Ashfall.Core.Tests/Survivors/SurvivorInspectionHostSessionTests.cs` | **deleted** | direct fixture for the deleted projection | build 0/0; suite green |
| 3 | `Ashfall.Core.Tests/BalanceFedLoopTelemetryTests.cs` | comment retargeted (`… in SurvivorInspection` → `… 10 per unit`) | remove a stale reference to the deleted type | build 0/0 |
| 4 | `docs/radio/DISTRESS_SIGNAL_DEAD_DATA_REGISTER.md` | **new** | single authoritative register for the 5 primary-wins rows | integrity gate emits exactly those 5 |
| 5 | `docs/radio/DISTRESS_SIGNAL_STAGE_CONTRACT.md` | §5 links the register | keep the contract pointing at one authority | docs index check |
| 6 | `KNOWN_DEBT.md` | `DEBT-SURVIVOR-INSPECTION-ORPHAN` QUARANTINED → RETIRED/SEALED; `DEBT-186` execution note | close debt truthfully | n/a |
| 7 | `docs/gaps/PARTIAL_PLANS_VERIFIED_AUDIT.md` | P10 → SEALED; D2 section → DELETED | prevent rediscovery | docs index check |
| 8 | `docs/plans/wave8_part2/D2_{PREMISE_EVIDENCE,CHANGE_MATRIX,ACCEPTANCE,HANDOFF}.md` | **new** | required deliverables | n/a |

## Non-changes

- No production behavior change beyond removing the dead projection.
- The five expansion dead rows were **not** deleted (data authority not signed).
- No JSON comments inserted (not schema-permitted).
- `SurvivorDetailPanel` / `ItemInspectionModel` untouched (they remain the live
  read surfaces).
