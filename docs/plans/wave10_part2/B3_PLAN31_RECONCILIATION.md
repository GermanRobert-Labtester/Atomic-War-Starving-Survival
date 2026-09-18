# Plan 31 (C1[8]) Reconciliation Report: "The Event Layer Speaks"

**Package:** Wave 10 Part 2 — Task B3\
**Plan Authority:** `C-integration-plans/C1_planintegration[8].md` (Plan 31)\
**Status:** **SEALED-ELSEWHERE / VERIFIED-RESOLVED (Case C)**\
**Date:** 2026-09-17\
**Integrator:** Antigravity\

---

## 1. Executive Summary

Plan 31 ("The Event Layer Speaks: Semantic Day Events, Navigable Briefings & Replayable Diagnostics") establishes three authoritative axes:
1. **Semantic Event Layer:** Semantic day-event classification, loud failure on unmapped events, static derived classification without persistent bloat.
2. **Navigable Briefings:** Briefing entries provide typed route metadata mapping domain facts to canonical UI panel routes, guarded against invalid routes.
3. **Replayable Diagnostics:** Day telemetry records capturing owner durations, semantic event traces, and reproducible diagnostic logs.

Per Wave 10 Part 2 §2.2 **Case C**, current source at `HEAD` already satisfies all three axes completely:
- **Axis 1 (Semantic Kinds)** was fully sealed in **Wave 9 Task B1** (`DayEventVocabulary.cs`, `EVENT_SEMANTIC_PARITY_MATRIX.md`, `DayEventVocabularyTests.cs`).
- **Axis 2 (Navigable Briefings)** was fully sealed in **Wave 10 Part 1 Task C1** (`BriefingRouteMap.cs`, `DailyBriefingReportBuilder.cs`, `Main.Campaign.cs`, `Plan31BriefingRouteTests.cs`).
- **Axis 3 (Replayable Diagnostics)** was fully sealed in **Wave 10 Part 1 Task C1** (`DayRecord.cs`, `DayRecordBuilder.cs`, `Main.DayRecord.cs`, `DayRecordTests.cs`, `docs/telemetry/DAY_RECORD.md`).

Zero additional code churn is required or permitted. Plan 31 / C1[8] is **100% SEALED**.

---

## 2. Clause Reconciliation Matrix

| Clause / Axis | Historical Requirement | Wave 9 B1 Deliverable | Existing Source at HEAD | Wave 10 Part 2 Delta | Final Status | Sealing Evidence |
|---|---|---|---|---|---|---|
| **31A.1 Semantic Kind Representation** | Typed semantic classification for all campaign day events | `DayEventVocabulary.cs` (`DayEventSemanticKind` enum) | `Assets/Ashfall.Core/Campaign/DayEventVocabulary.cs` | None (0 churn) | **SEALED** | 100% totality gate in `DayEventVocabularyTests.cs` |
| **31A.2 Totality & Loud Failure** | Every registered event ID maps to a semantic kind; unmapped events fail loud | Implemented in `DayEventVocabulary.GetSemanticKind()` | Throws `KeyNotFoundException` / `ArgumentException` on unmapped IDs | None (0 churn) | **SEALED** | `DayEventVocabularyTests.cs` (all 47+ catalog events mapped) |
| **31A.3 Parity Matrix** | Exhaustive documentation of event IDs, emitters, and semantic mappings | Created `EVENT_SEMANTIC_PARITY_MATRIX.md` | `docs/campaign/EVENT_SEMANTIC_PARITY_MATRIX.md` | None (0 churn) | **SEALED** | CI gate 25 (`verify-event-semantic-parity.py`) passes cleanly |
| **31B.1 Route Target Contract** | Briefing entries carry typed `RouteTarget` strings pointing to canonical panels | Deferred in Wave 9 per generic title contract | `BriefingRouteMap.cs` maps `DayEventSemanticKind` to UI route strings | None (0 churn) | **SEALED** | `Plan31BriefingRouteTests.cs` (6 tests PASS) |
| **31B.2 Briefing Entry Enrichment** | `DailyBriefingEntry` stores `RouteTarget` without mutating display text | Model unextended in Wave 9 | `DailyBriefingReportBuilder.cs` exposes `DailyBriefingEntry.RouteTarget` | None (0 churn) | **SEALED** | `DailyBriefingReportBuilder.cs` lines 32–48 |
| **31B.3 Host Route Liveness Guard** | Host validates route target against `PanelRegistry` before navigation | Not present in Wave 9 | `src/Main.Campaign.cs` checks `PanelRegistryBootstrap.IsRegistered(routeId)` | None (0 churn) | **SEALED** | `Main.Campaign.cs` `OnBriefingRouteRequested` |
| **31C.1 Day Telemetry Schema** | Structured schema capturing day number, elapsed time, and per-owner execution duration | Not present in Wave 9 | `Assets/Ashfall.Core/Campaign/DayRecord.cs` | None (0 churn) | **SEALED** | `DayRecordTests.cs` (4 tests PASS) |
| **31C.2 Owner Timing Monotonicity** | Accurate per-owner timing during `AdvanceDay` | Not present in Wave 9 | `CampaignDayCoordinator.cs` records stopwatch duration in `DayOwnerReport` | None (0 churn) | **SEALED** | `CampaignDayCoordinator.cs` lines 145–170 |
| **31C.3 Opt-in Replay Trace Writer** | Diagnostic trace output without polluting default stdout | Not present in Wave 9 | `src/Main.DayRecord.cs` writes JSONL when `ASHFALL_DAY_RECORD=1` | None (0 churn) | **SEALED** | `docs/telemetry/DAY_RECORD.md` specification |

---

## 3. Verification Summary

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/Plan31BriefingRouteTests.cs` — 6/6 PASS.
2. `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/DayRecordTests.cs` — 4/4 PASS.
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/DayEventVocabularyTests.cs` — 8/8 PASS.
4. Fast CI Gate 25 (`verify-event-semantic-parity.py`) — PASS.
5. All 48 Fast CI Gates (`scripts/ci/verify-fast.sh`) — PASS.

**Conclusion:** Plan 31 (C1[8]) is definitively closed as **SEALED-ELSEWHERE / VERIFIED-RESOLVED**.
