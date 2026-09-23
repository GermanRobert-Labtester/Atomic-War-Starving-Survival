# Unblock Residuals — Plans 24 + 31 Integration Plan

**Package:** `UNBLOCK-RESIDUALS-PLANS-24-31`
**Claim:** `claim-unblock-residuals-plans-24-31-2026-09-23`
**Date:** 2026-09-23
**Status:** COMPLETE (see the ledger entry in `INTEGRATION_PLANS.md`)
**Role:** Integrator (user-authorized unblock & full integration)

---

## 1. Executive Summary & Objective

This integration package resolves and seals the two long-standing blocked residuals:
1. **Plan 31 residual (D11-B):** The decision-blocked closed-section routing contract where non-heartbeat unhandled day-event kinds route to their designated semantic sections (e.g. `Warnings` for `SemanticKind.Hazard`) rather than dumping everything into `GenericSectionTitle` ("System Activity"). `Plan31BriefingRouteTests.GenericNonHeartbeatEvent_BecomesActionable` has been the single red test across Batches 1–6 of the unblock program (`Campaign 256/257`). With D11-B ratified and the test assertion aligned with the semantic section contract, `Plan31BriefingRouteTests` is 6/6 PASS and the entire Campaign suite reaches 257/257 PASS (0 red).
2. **Plan 24 residual (Environment-blocked snapshot rebaseline):** Plan 24 core implementation is CLOSED (ward staffing option b sealed, affliction recovery ramp option ii sealed). The sole residual—golden UI snapshot rebaseline—was blocked due to headless environment restrictions (SubViewport requires a real display/GPU). This package formally ratifies the L-P24R procedure (`docs/plans/unblockers/UNBLOCK-04_LEDGER_REGISTER_CENSUS_QUARANTINE_TRUTH.md` §5.11), establishes the exact execution protocol for the first renderer-capable session, and unblocks the procedural roadblock in the governance ledger without fabricating golden snapshots in headless mode.

---

## 2. Plan 31 Residual (D11-B) Integration

### 2.1 Premise & Root Cause Analysis
- `DailyBriefingReportBuilder` default switch case was updated under D11-A (`DEC-23`) to route non-heartbeat unhandled events to their semantic sections via `DayEventVocabulary.SectionTitleFor(semantic)` instead of unconditionally adding them to `GenericSectionTitle` ("System Activity").
- In `Assets/Ashfall.Core/Campaign/DayEventVocabulary.cs`, `"sanitation_spill"` is classified as `SemanticKind.Hazard`, which maps to section title `"Warnings"`.
- In `Ashfall.Core.Tests/Campaign/Plan31BriefingRouteTests.cs`, the test `GenericNonHeartbeatEvent_BecomesActionable` was asserting:
  ```csharp
  var entry = Assert.Single(report.Sections, s => s.Title == DayEventVocabulary.GenericSectionTitle).Entries[0];
  ```
- Because `"sanitation_spill"` was correctly placed into `"Warnings"`, the test failed with `Assert.Single() Failure: The collection did not contain any matching items`.
- `UNBLOCK-03_SEMANTIC_VOICE_STRING_FREEZE_D11_D22_PLAN424649.md` §5.1.2 explicitly documented the migration:
  `| Plan31BriefingRouteTests (2 reads) | generic title lookups | read the routed title for the fixture kind |`

### 2.2 Changes Applied
- Updated `Ashfall.Core.Tests/Campaign/Plan31BriefingRouteTests.cs` to resolve the expected section title via `DayEventVocabulary.SectionTitleFor(DayEventVocabulary.GetSemanticKind("sanitation_spill"))`.
- Confirmed that `UnknownKind_IsInformational_NotADeadAffordance` still tests fallback to `DayEventVocabulary.GenericSectionTitle` for unmapped synthetic event kinds with `IsActionable = false`.
- Recorded Decision `DEC-303` (D11-B closed-section routing ratification).

### 2.3 Verification Evidence
- `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/Plan31BriefingRouteTests.cs` → **6/6 PASS** (19 ms).
- `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/DayEventVocabularyTests.cs` → **8/8 PASS** (27 ms).
- `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/` → **257/257 PASS** (860 ms, 0 failed).

---

## 3. Plan 24 Residual (Environment-Blocked Snapshot Rebaseline) Integration

### 3.1 Premise & Status
- `docs/plans/PLAN_24_CLOSEOUT.md` confirms:
  - Both product/architecture signatures were granted by the user on 2026-09-18 (ward staffing via option b, recovery ramp via option ii).
  - Production logic, data schemas, save/load journey parity tests (3/3), and 30-day simulation tests are 100% verified and passing.
  - Item 12 ("Snapshot review") remained ENVIRONMENT-BLOCKED because SubViewport rendering requires a real display/GPU, and no headless test runner can execute visual snapshot diffs without artifacts.

### 3.2 L-P24R Protocol Adoption
The formal procedure defined in `UNBLOCK-04` §5.11 is now incorporated as an authoritative addendum to `docs/plans/PLAN_24_CLOSEOUT.md`:
1. **Inventory Review:** Review the documented render changes in `PLAN_24_CLOSEOUT.md`:
   - Survivor Detail: "Top active need contributors" / "Recent need contributors" populated by attributed sources.
   - Duty Roster: Duty-Hours row (committed/recommended + OVERWORKED indicator), ASSIGNMENT buttons, impaired-warning dialog.
   - Cenotaph: Memorial status line (recorded souls + vigil pending) and live vigil button.
2. **Harness Confirmation:** Use the canonical repository snapshot harness (`--snapshot-selftest` or dedicated Godot rendering harness).
3. **Execution on Renderer-Capable Session:** Render panels at 1920x1080 resolution on an environment with a physical or virtual GPU display (X11/Wayland with active OpenGL/Vulkan context).
4. **Diff Verification:** Confirm any diffs match the authorized inventory changes. If matched, rebaseline the golden PNGs; if mismatched, log a regression defect.
5. **Closeout Recording:** Record run timestamp, host display info, MATCH/DIFF counts, and update `PLAN_24_CLOSEOUT.md` item 12 to CLOSED.
6. **Headless Invariant:** Under no circumstances should headless sessions fabricate snapshot passes or bypass verification.

---

## 4. Worktree Hygiene & Repository Verification

- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` → 0 Errors, 0 Warnings.
- `dotnet build Ashfall.csproj` → 0 Errors, 0 Warnings.
- `python3 scripts/ci/agent-fast-verify.py` → 10/10 gates PASS.
- `python3 scripts/ci/sync-agent-rulebooks.py --check` → 13/13 rulebooks in sync.
