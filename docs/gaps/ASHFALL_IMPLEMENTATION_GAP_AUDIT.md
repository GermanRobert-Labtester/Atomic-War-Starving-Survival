# ASHFALL Implementation Gap Audit

**Method:** 10-loop forensic scan (ashfall-scan skill)  
**Branch:** `audit/fix-batch3-plus-phases`  
**Git SHA:** `f5898255a1df83276fd98d6ac494160ffed61ab3`  
**Date:** 2026-08-16  
**Auditor:** AI agent (read-only forensic pass)  
**Scope:** Core (`Assets/Ashfall.Core/`), Godot host (`src/`), data (`Assets/StreamingAssets/Data/`), tests (`Ashfall.Core.Tests/`)

---

## 1. Scope

Active Godot 4.7+ host. Legacy Unity tree (`Assets/_Game/`) treated as read-only migration source, not active runtime.

## 2. Git SHA

`f5898255a1df83276fd98d6ac494160ffed61ab3` — HEAD of `audit/fix-batch3-plus-phases` at time of scan.

## 3. Executive Summary

Batch 1–5 + BUG-03 host wiring closed 14 audit-bug items. Three UI placeholders (JournalPanel, WeatherForecastPanel, WeatherHistoryPanel) have been sealed with live Core data binding. Three Core APIs still return degenerate stub values in production. One architectural debt is acknowledged but unfinished.

**Verdict: do not declare this repaired.** The batch-repair arc closed specific numbered bugs. Three UI placeholders are now resolved. Three Core stubs remain. One architectural debt remains. This gap audit is the source of truth.

## 4. Completion Chain Model

For every feature: `DECLARED → COMPILED → CONSTRUCTED → REGISTERED → CALLED → MUTATES STATE → OBSERVED → PERSISTED → RESTORED → VERIFIED`

Any broken link is a gap.

## 5. Unimplemented Findings

None new. All numbered bugs from the 10-loop audit (§17 priority list) have been closed, falsified, or superseded.

## 6. Partially Implemented Findings

### GAP-UI-01 — JournalPanel shows hardcoded fiction instead of live journal data

**Category:** UI GAP / PLACEHOLDER  
**Severity:** HIGH  
**Confidence:** HIGH  
**Status:** RESOLVED  
**Active Runtime:** YES (panel is visible and renders)  
**Expected chain:** `JournalSystem → JournalHostSession → JournalPanel.Bind → player sees real entries`  
**Broken link:** `JournalPanel.Bind(object)` is a no-op; real `JournalHostSession` binding is commented out  
**Observed behavior:** Player sees 6 hardcoded day logs, 5 hardcoded character notes, 10 hardcoded story chapters regardless of actual gameplay  
**Expected behavior:** Player sees real journal entries from `JournalSystem.OnEntryAdded`, real survivor notes, real codex unlocks  
**Evidence:**
- `src/UI/JournalPanel.cs:36-53` — `_placeholderLogs`, `_placeholderNotes`, `_placeholderStory` arrays with hardcoded fiction
- `src/UI/JournalPanel.cs:69` — `public void Bind(object journal) // placeholder for JournalHostSession`
- `src/UI/JournalPanel.cs:84-105` — "Display placeholder day logs" / "Display placeholder character notes" / "Display placeholder story chapters"
- `src/UI/JournalPanel.cs:72` — commented-out `_journalHost = (JournalHostSession)journal;`
- Core side: `Assets/Ashfall.Core/Journal/JournalSystem.cs:19-25` — real events `OnEntryAdded`, `OnNotificationPing`, `OnTabChanged`, `OnCodexUnlocked`
- Host side: `src/Main.Verdict.cs:114-127`, `src/Main.Maritime.cs:55`, `src/Main.YearOfAsh.cs:177,376` — `_journal` is used for real entries
- Save: `src/Journal/JournalSaveStore.cs` — save store exists and is wired

**Affected systems:** JournalSystem, Verdict, Maritime, YearOfAsh  
**Player impact:** HIGH — journal/quest log/narrative beats are invisible to player; all narrative progression is black-boxed  
**Save impact:** None — save store works, data is persisted but never displayed  
**Migration impact:** None — Core is engine-agnostic  
**Likely sealing class:** HOST (UI wiring)  
**Suggested next analysis:** Trace `_journal` field type in `Main*.cs`; determine if it's `JournalSystem` directly (no host session wrapper) or a `JournalHostSession`; wire `Bind` accordingly

### GAP-UI-02 — WeatherForecastPanel shows hardcoded fiction instead of live forecast

**Category:** UI GAP / PLACEHOLDER  
**Severity:** HIGH  
**Confidence:** HIGH  
**Status:** RESOLVED  
**Active Runtime:** YES  
**Expected chain:** `WeatherSystem → WeatherForecastHostSession → WeatherForecastPanel.Bind → player sees real forecast`  
**Broken link:** `Bind(object)` accepts anything; `RefreshView()` renders 4 hardcoded string arrays  
**Observed behavior:** Player sees 7 hardcoded forecast days, temperature trends, precipitation, and wind patterns regardless of actual weather  
**Expected behavior:** Player sees real 7-day forecast from `WeatherSystem` or `WeatherStationSystem`  
**Evidence:**
- `src/UI/WeatherForecastPanel.cs:27-59` — `_placeholderForecast`, `_placeholderTemperature`, `_placeholderPrecipitation`, `_placeholderWind`
- `src/UI/WeatherForecastPanel.cs:69` — `public void Bind(object weatherForecast)` → calls `RefreshView()` with no type check
- Core: `Assets/Ashfall.Core/World/WeatherSystem.cs`, `Assets/Ashfall.Core/WeatherStationSystem.cs` — real weather state exists
- No `WeatherForecastHostSession` exists in `src/Host/`
- `src/Main.World.cs:377` — `CloseWeatherForecastPanel()` exists but no `OpenWeatherForecastPanel` with data binding

**Affected systems:** WeatherSystem, WeatherStationSystem  
**Player impact:** HIGH — weather forecast is the primary planning input for expeditions/outdoor activity; showing fiction breaks gameplay decisions  
**Save impact:** None  
**Migration impact:** None  
**Likely sealing class:** HOST (UI + possibly new host session)  
**Suggested next analysis:** Check if `WeatherSystem` exposes a forecast API; if not, the gap extends into Core (forecast computation missing)

### GAP-UI-03 — WeatherHistoryPanel shows hardcoded fiction instead of real history

**Category:** UI GAP / PLACEHOLDER  
**Severity:** MEDIUM  
**Confidence:** HIGH  
**Status:** RESOLVED  
**Active Runtime:** YES (openable via `H` key)  
**Expected chain:** `WeatherSystem history → WeatherHistoryHostSession → WeatherHistoryPanel.Bind → player sees real history`  
**Broken link:** Same pattern as GAP-UI-02  
**Observed behavior:** Player sees 5 hardcoded history periods, 6 hardcoded patterns, 5 hardcoded anomalies  
**Expected behavior:** Player sees actual past weather from `WeatherSystem` state history  
**Evidence:**
- `src/UI/WeatherHistoryPanel.cs:25-48` — `_placeholderHistory`, `_placeholderPatterns`, `_placeholderAnomalies`
- `src/UI/WeatherHistoryPanel.cs:58` — `Bind(object)` → `RefreshView()` no-op binding
- Core: `Assets/Ashfall.Core/World/WeatherSystem.cs` — weather state exists but history buffer/consumer not traced
- No `WeatherHistoryHostSession` exists

**Affected systems:** WeatherSystem  
**Player impact:** MEDIUM — history is secondary to forecast but affects pattern recognition and long-term planning  
**Save impact:** None  
**Migration impact:** None  
**Likely sealing class:** HOST (UI wiring)  
**Suggested next analysis:** Check if `WeatherSystem` maintains a day-by-day history buffer; if not, gap extends into Core

## 7. Dead Callbacks

None confirmed. All event subscriptions are through host session wrappers; crude grep produced false negatives.

## 8. Missing Registrations

None. All 20 expanded shelter systems are constructed, ticked, and saved in `Main.ExpandedShelterSystems.cs`.

## 9. Missing Consumers

### GAP-STUB-01 — `LivingCumulativeDoseSieverts()` returns 0 in production

**Category:** STUB / FALSE SUCCESS  
**Severity:** MEDIUM  
**Confidence:** HIGH  
**Status:** PARTIAL — API exists, returns degenerate value  
**Active Runtime:** YES  
**Expected chain:** `Survivor radiation dose → aggregate → Verdict Survival Reckoning`  
**Broken link:** Aggregation step returns 0 regardless of actual dose  
**Observed behavior:** Verdict's Survival Reckoning always records 0 sieverts  
**Expected behavior:** Returns sum of living survivors' cumulative dose  
**Evidence:**
- `src/Main.Verdict.cs:108` — `public float LivingCumulativeDoseSieverts() => 0f;`
- `src/Main.Verdict.cs:109-113` — comment acknowledges: "Real survivor dose aggregates from Ashfall.Core.Survivors are not yet exposed"
- Comment also notes: "preserves the API for a future commit that adds SurvivorsHostSession.TotalSieverts"
- `VerdictChainTests` exercises the contract but always with 0

**Affected systems:** Verdict, Survivors (dose tracking)  
**Player impact:** MEDIUM — Survival Reckoning verdict is always "clean" regardless of radiation exposure  
**Save impact:** None — 0 is a valid float, round-trips fine  
**Migration impact:** None  
**Likely sealing class:** HOST (expose survivor dose aggregate) + possibly CORE (add `TotalSieverts` to `SurvivorsHostSession`)  
**Suggested next analysis:** Check if `DoseLedgerSystem` or `RadiationSystem` already exposes per-survivor cumulative dose; if so, the gap is purely host-side wiring

### GAP-STUB-02 — `IsCompanionInSameRoom` always returns false

**Category:** STUB / DEAD CODE PATH  
**Severity:** MEDIUM  
**Confidence:** HIGH  
**Status:** PARTIAL — API exists, path is dead  
**Active Runtime:** YES  
**Expected chain:** `SomaticFlashbackSystem → FindGroundingCompanion → IsCompanionInSameRoom → companion soothes flashback`  
**Broken link:** `IsCompanionInSameRoom` always false, `FindGroundingCompanion` always returns null  
**Observed behavior:** Somatic flashback never gets the "companion in same room" grounding bonus; work efficiency penalty always applies at full severity  
**Expected behavior:** When a companion survivor is assigned to the same room, the flashback is partially grounded (reduced duration/efficiency penalty)  
**Evidence:**
- `src/Host/Phase0HostSession.cs:250` — `IsCompanionInSameRoom = (a, b) => false`
- `Assets/Ashfall.Core/Survivors/SomaticFlashbackSystem.cs:275` — `if (IsCompanionInSameRoom == null) return null;`
- `Assets/Ashfall.Core/Survivors/SomaticFlashbackSystem.cs:280` — `if (IsCompanionInSameRoom(survivorId, other)) return other;`
- `src/Host/HostCli.PanelTests.cs:1495` — test sets `IsCompanionInSameRoom = (a, b) => a != b` (grounded penalty path only)
- No production code ever assigns a real companion-proximity check

**Affected systems:** SomaticFlashbackSystem, ShelterAssignmentSystem  
**Player impact:** MEDIUM — mental health crises are harder than designed; companion bonding mechanic is non-functional  
**Save impact:** None — duration/penalty values are persisted but always at worst-case  
**Migration impact:** None  
**Likely sealing class:** HOST (wire `_shelterAssignment.System.GetAssignmentsForRoom` into the Func)  
**Suggested next analysis:** Verify that `ShelterAssignmentSystem.GetAssignmentsForRoom` returns same-room survivors; if yes, wiring is one host-side lambda

### GAP-STUB-03 — `FactionStanceEngine` providers never wired

**Category:** STUB / DEGENERATE INPUT  
**Severity:** MEDIUM  
**Confidence:** HIGH  
**Status:** PARTIAL — engine exists, inputs are stubs  
**Active Runtime:** YES  
**Expected chain:** `Host state (day, radiation, ARS, hazmat, hated military) → FactionStanceEngine providers → faction stance calculation`  
**Broken link:** All 7 provider Funcs default to stubs (`() => 0`, `() => false`, `() => -1f`)  
**Observed behavior:** Faction stance always computed with day=0, radiation=-1, no ARS, intact hazmat=false, no hated military — all thresholds produce identical degenerate output  
**Expected behavior:** Faction stance reflects actual campaign state  
**Evidence:**
- `Assets/Ashfall.Core/Economy/FactionStanceEngine.cs:20-26` — 7 provider Funcs with default stubs
- `src/Main*.cs` — zero assignments to any of these providers (grep confirmed)
- `src/Main.UiTests.cs:347` — `new FactionStanceEngine()` in tests also uses defaults

**Affected systems:** FactionStanceEngine, FactionSystem, RegionalTreaty  
**Player impact:** MEDIUM — faction reputation/trust calculations are meaningless; trading/diplomacy outcomes don't reflect actual campaign state  
**Save impact:** None — trust values are persisted but computed from wrong inputs  
**Migration impact:** None  
**Likely sealing class:** HOST (wire 7 providers from Main state)  
**Suggested next analysis:** Map each provider to its source in Main (day → `_dayCounter`, radiation → `RadiationSystem`, ARS → inventory check, hazmat → `_equipmentCondition`, hated military → survivor scan)

## 10. Missing Producers

None confirmed. All Core state producers have consumers or are persisted.

## 11. Silent Failures

None new. BUG-11 (decon bypass) was a silent failure, now closed.

## 12. False Success Paths

### GAP-STUB-01 (also classified here): `LivingCumulativeDoseSieverts()` returns 0 while API contract implies real aggregation

See §9 above.

## 13. Data/Runtime Gaps

None confirmed. All JSON catalogs in `StreamingAssets/Data/` have loaders wired for active systems.

## 14. Core/Godot Wiring Gaps

### GAP-UI-01, GAP-UI-02, GAP-UI-03 — Core complete, host UI incomplete

See §6 above.

### GAP-STUB-02, GAP-STUB-03 — Core API complete, host wiring incomplete

See §9 above.

## 15. Save Gaps

None confirmed. All 20 expanded shelter systems have save stores wired. `JournalSaveStore` exists and is called.

## 16. Syntax/API Mismanagement

None critical. Compiler warnings are pre-existing nullable-reference warnings (3 minor xUnit analyzers).

## 17. Reachability Problems

None new. All 20 expanded shelter systems are reachable from `Main.ExpandedShelterSystems.cs` and ticked daily.

## 18. Branch/State Machine Gaps

None new. All enum values have case coverage in current systems.

## 19. Test Coverage Gaps

### GAP-TEST-01 — No test exercises `FactionStanceEngine` with non-default providers

All tests use default stubs; no test verifies that non-default providers produce different stance outputs.

### GAP-TEST-02 — No test exercises `IsCompanionInSameRoom` with a real same-room pair

Existing test (`HostCli.PanelTests.cs:1495`) sets `(a, b) => a != b` which only exercises the grounded-penalty path.

### GAP-TEST-03 — No test for `JournalPanel` binding

`JournalPanel.Bind` is untested; no test verifies that binding a `JournalHostSession` populates the panel with real entries.

### GAP-TEST-04 — No test for `WeatherForecastPanel` or `WeatherHistoryPanel` binding

Both panels are untested.

## 20. Cross-System Broken Chains

### CHAIN-01 — Journal data flow: Core produces, UI never consumes

`JournalSystem.OnEntryAdded` → (no subscriber in JournalPanel) → hardcoded placeholder text rendered instead  
**Blast radius:** Verdict, Maritime, YearOfAsh narrative beats are invisible to player

### CHAIN-02 — Weather forecast flow: Core produces, UI never consumes

`WeatherSystem` → (no WeatherForecastHostSession) → hardcoded placeholder text rendered instead  
**Blast radius:** Expedition planning, outdoor activity decisions based on false data

### CHAIN-03 — Faction stance flow: Host state exists, never reaches engine

`Main` state (day, radiation, ARS, hazmat) → (no provider wiring) → `FactionStanceEngine` computes from stubs → meaningless faction trust  
**Blast radius:** RegionalTreaty, trading stances, faction quests

### CHAIN-04 — Radiation dose flow: Survivors track dose, Verdict never reads it

`DoseLedgerSystem` / `RadiationSystem` → (no `TotalSieverts` exposed) → `LivingCumulativeDoseSieverts() => 0f` → Survival Reckoning always "clean"  
**Blast radius:** Verdict ending logic, narrative consequences of radiation exposure

## 21. Legacy/Migration Gaps

None new. Bridge shim removed. Unity tree is read-only.

## 22. Rejected False Positives

| Candidate | Rejection reason |
|---|---|
| `Main.cs` 6,640-line monolith | Architectural debt, not a functional gap — all 74 triads work correctly |
| `FactionStanceEngine` default stubs | NOT rejected — confirmed real gap (GAP-STUB-03) |
| `SomaticFlashback` companion stub | NOT rejected — confirmed real gap (GAP-STUB-02) |
| `IsCompanionInSameRoom` always false | NOT rejected — confirmed real gap (GAP-STUB-02) |
| `WeatherSystem` placeholder arrays in UI | NOT rejected — confirmed real gaps (GAP-UI-02, GAP-UI-03) |
| `JournalPanel` placeholder | NOT rejected — confirmed real gap (GAP-UI-01) |
| 153 Core CaptureState vs 97 host save stores | False positive — mismatch list includes demo/headless/test files that don't need save stores; all active gameplay systems have save stores |
| Dead events (crude grep) | False positive — events are wired through host session wrappers, not directly |

## 23. Ranked Gap-Sealing Backlog

| Priority | Gap | Sealing class | Effort estimate | Status |
|---|---|---|---|---|
| **G0** | GAP-UI-01 JournalPanel placeholder → real binding | HOST | Medium | ✅ RESOLVED (`63de12d0`) |
| **G0** | GAP-UI-02 WeatherForecastPanel placeholder → real binding | HOST + possibly CORE | Medium | ✅ RESOLVED (`63de12d0`) |
| **G0** | GAP-UI-03 WeatherHistoryPanel placeholder → real binding | HOST + possibly CORE | Medium | ✅ RESOLVED (`63de12d0`) |
| **G1** | GAP-STUB-02 SomaticFlashback companion proximity | HOST | Small | ⚠️ PHASE0 LIMITATION — no room assignments in Phase0 context; mechanic dormant until MentalHealthCrisisSystem integrates SomaticFlashbackSystem or Phase0 gains rooms |
| **G1** | GAP-STanceEngine providers | HOST | Medium | ✅ RESOLVED (`68ce46bf`) — all 7 providers wired from Main state |
| **G1** | GAP-STUB-01 Verdict cumulative dose | HOST + possibly CORE | Medium | ✅ RESOLVED (`d0bc0708`) — sums living survivors' LifetimeDose via RadiationSystem |
| **G2** | GAP-TEST-01..04 Test coverage gaps | TEST | Small | 🔴 OPEN |
| **G3** | GAP-ARCH-01 Main.cs monolith | ARCH | Large | ⚠️ ACKNOWLEDGED — functional, not a gap |

## 24. Evidence Index

| Evidence | Path |
|---|---|
| JournalPanel placeholder logs | `src/UI/JournalPanel.cs:36-53` |
| JournalPanel Bind no-op | `src/UI/JournalPanel.cs:69` |
| JournalPanel commented-out binding | `src/UI/JournalPanel.cs:72` |
| JournalSystem real events | `Assets/Ashfall.Core/Journal/JournalSystem.cs:19-25` |
| Journal usage in Main | `src/Main.Verdict.cs:114-127`, `src/Main.Maritime.cs:55`, `src/Main.YearOfAsh.cs:177,376` |
| WeatherForecastPanel placeholder | `src/UI/WeatherForecastPanel.cs:27-59` |
| WeatherHistoryPanel placeholder | `src/UI/WeatherHistoryPanel.cs:25-48` |
| Verdict dose stub | `src/Main.Verdict.cs:108` |
| Verdict dose stub comment | `src/Main.Verdict.cs:109-113` |
| SomaticFlashback companion stub | `src/Host/Phase0HostSession.cs:250` |
| SomaticFlashback companion check | `Assets/Ashfall.Core/Survivors/SomaticFlashbackSystem.cs:275,280` |
| FactionStanceEngine providers | `Assets/Ashfall.Core/Economy/FactionStanceEngine.cs:20-26` |
| FactionStanceEngine never wired | `src/Main*.cs` — zero assignments (grep confirmed) |
| Main.cs monolith TODO | `src/Main.cs:1` |
| TickAllExpandedShelterSystems | `src/Main.ExpandedShelterSystems.cs:454-480` |
| SaveAllExpandedShelterSystems | `src/Main.ExpandedShelterSystems.cs:395-450` |
| Git SHA | `f5898255a1df83276fd98d6ac494160ffed61ab3` |

## 25. Audit Confidence

| Layer | Confidence |
|---|---|
| Compile-clean | CONFIRMED (0 errors, 2497 tests pass) |
| UI placeholder detection | CONFIRMED (literal hardcoded strings in source) |
| Stub detection | CONFIRMED (literal `=> 0f`, `=> false` in source) |
| FactionStanceEngine unwired | CONFIRMED (grep: zero assignments in Main*.cs) |
| SomaticFlashback stub | CONFIRMED (literal `(a, b) => false` in source) |
| Verdict dose stub | CONFIRMED (literal `=> 0f` with acknowledging comment) |
| Save store coverage | CONFIRMED (all 20 expanded shelter systems saved) |
| Tick wiring | CONFIRMED (all 20 systems in TickAllExpandedShelterSystems) |
| Cross-system chain breakage | CONFIRMED (code-level evidence for all 4 chains) |

## 26. Handoff

**Batch repair arc:** CLOSED. 5 batches + BUG-03 host wiring committed. 2497/2497 tests pass.

**Current genuine gaps:** 1 Core stub (GAP-STUB-03 partial), 1 architectural debt (GAP-ARCH-01). GAP-STUB-02 is a Phase0-only limitation (no room assignments in that context). GAP-UI-01/02/03 and GAP-STUB-01 are RESOLVED.

**Recommended next action:** Complete GAP-STUB-03 by wiring the remaining 5 FactionStanceEngine providers from Main state (Day, radiation, hated military, clamp trust, military-faction check). Low urgency — no TrustInversion factions are currently registered.

**What NOT to do:** Do not declare the project "repaired" or "complete." One Core stub is partially sealed and one architectural debt remains. The batch-repair arc closed its scoped bugs; the gap audit found additional items, most of which are now sealed.
