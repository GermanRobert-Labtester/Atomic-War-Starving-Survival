# ASHFALL — WAVE 9 PART 2 IMPLEMENTATION UNBLOCKER CLOSEOUT REPORT

> **Status:** COMPLETE & VERIFIED
> **Date:** 2026-09-17
> **Verification Tier:** FAST (47/47 GATES PASSED CLEANLY)
> **Authority Decisions:** User Authorized C1 Option C, C2 Option B, C3 Findings Closure, D1 Content Tranche, D2 Option C, D3 Quarantine Batch Promotion

---

## 1. Executive Summary

Wave 9 Part 2 resolved the six surviving blocker packages from the Wave 9 ledger. All tasks were executed strictly in accordance with approved options and non-negotiable architectural rules (engine-free Core, authoritative data JSON, deterministic simulation, 0 disk deletion for tool files, targeted verification, full gate pass).

| Package | Approved Route | Status | Key Deliverables & Test Results |
|---|---|---|---|
| **C1 — Merchant Restock Priority** | Option C | **SEALED** | Pure priority scoring function `ComputeItemPriorityScore`, deterministic ordering `GetPrioritizedStock` in `ShelterBarterSystem`, display ordering in `ShelterBarterPanel`. Pinned stock & no-reroll invariants strictly preserved. `Plan147RestockPriorityTests` 6/6 PASS; host build 0/0. |
| **C2 — SignalTrust Availability** | Option B | **SEALED** | Formal retirement and tombstoming of dormant availability policy. Dial tuning operates on static authored frequencies; dynamic spawner retired. Math pins preserved in `SignalTrustTests` (21/21 PASS). `SIGNAL_TRUST_CONTRACT.md` §5 & `INTEGRATION_PLANS.md` updated. |
| **C3 — Radiation Balance Findings** | Signed Verdicts | **SEALED** | Formally closed F1–F8 in `BALANCE_SIM_radiation_exposure_20A.md`. F1 rate reduction DECLINED to preserve hardcore survival pressure. F2–F8 confirmed/mitigated/resolved. 0 data modifications. `Plan20ARadiationBalanceSweepTests` (9/9 PASS) & `Plan20BShieldingBalanceSweepTests` (12/12 PASS) preserved. |
| **D1 — Distress Content Tranche** | 12-Signal Tranche | **SEALED** | Authored follow-up signals with closed trigger grammar (`answered`, `rescue_success`, `expired`, `ambush_encountered`) across 12 high-traffic/moral/trap/false-flag signals in `radio_distress_signals.json`. `DistressFollowUpTests` 21/21 PASS, `DistressAudioCueTests` 10/10 PASS, data-integrity PASS 0 errors, audio-selftest 630/630 PASS. |
| **D2 — AI Tool Governance** | Option C | **SEALED** | Kept `.agents/` tracked as canonical. Untracked `.qwen/`, `.codex/`, `.cursor/` via `git rm --cached` without deleting any file on disk. Added directories to `.gitignore`. `scripts/ci/repo-hygiene-report.sh` verified safe. |
| **D3 — Quarantine Promotion** | Reinstatement | **SEALED** | Promoted `AutopsyProceduresCatalogTests.cs` from quarantine. Rematched tool fixture items to canonical catalog requirements (`medical_scissors`, `protective_rubber_gloves`, `field_surgical_kit`, etc.). Dropped Compile Remove. `AutopsyProceduresCatalogTests` 12/12 PASS, Medical suite 371/371 PASS, `KNOWN_DEBT.md` updated. |

---

## 2. Package Details

### 2.1 C1 — Merchant Restock Priority (Option C)
- **Problem:** Restock priority was specified to order inventory displays based on demand/scarcity, but without rerolling or altering stock amounts during active stays.
- **Solution:** Implemented pure static priority scorer `ComputeItemPriorityScore` and deterministic ordering method `GetPrioritizedStock` in `Assets/Ashfall.Core/Economy/ShelterBarterSystem.cs`. Tied stock items break stably on ordinal item ID.
- **UI Integration:** Updated `src/UI/ShelterBarterPanel.cs` line 680 to consume `_barterSystem.GetPrioritizedStock(caravan)` for display iteration.
- **Verification:**
  - `Ashfall.Core.Tests/Economy/Plan147RestockPriorityTests.cs`: 6/6 PASSED (pure function, custom modifier, tie-break, stock count preservation, same-day no-reroll, next-arrival re-evaluation).
  - `Ashfall.Core.Tests/Economy/ShelterBarterSystemPlan54Tests.cs`: 6/6 PASSED.
  - `Ashfall.Core.Tests/Narrative/ContrabandBarterRouteTests.cs`: 11/11 PASSED.
  - `Ashfall.Core.Tests/UI/ShelterBarterPanelRouteTests.cs`: 3/3 PASSED.
  - `dotnet build Ashfall.csproj`: 0 warnings, 0 errors.

### 2.2 C2 — SignalTrust Availability Consumer (Option B)
- **Problem:** `SignalTrustAvailability` was tested in isolation but remained dormant because ASHFALL's radio tuning is an analog rotary dial over static authored frequencies (`RadioTuner.EvaluateFrequency`), not a dynamic candidate spawner.
- **Solution:** Formally retired the requirement for an availability consumer. Tombstoned the status in `docs/radio/SIGNAL_TRUST_CONTRACT.md` §5 and updated `INTEGRATION_PLANS.md`. Retained unit test math pins in `SignalTrustTests.cs`.
- **Verification:**
  - `Ashfall.Core.Tests/Radio/SignalTrustTests.cs`: 21/21 PASSED.

### 2.3 C3 — Radiation Balance Findings F1–F8 (Signed Verdicts)
- **Problem:** Balance sim report `BALANCE_SIM_radiation_exposure_20A.md` had left findings F1–F8 open for foreman decision.
- **Solution:** Recorded official signed foreman closure in `docs/balance/BALANCE_SIM_radiation_exposure_20A.md`:
  - F1 (Surface lethality): DECLINED proposal to re-scale outdoor rates. Surface hostility is core to the hardcore survival identity; long excursions rely on Plan 21 gear, Plan 50 vehicle shielding, Plan 20B shelter infrastructure, and Plan 22C anti-rad medication.
  - F2 (Ashfall weather alignment): RESOLVED in 20A (+45 mSv/h).
  - F3 (Ceiling attenuation): RESOLVED in 20B multi-component shielding model.
  - F4 (Intact shelter radon floor): RESOLVED in 20B (1.44 mSv/day indoor floor).
  - F5–F8 (Shielding invariants): CONFIRMED design invariants.
  - Data Modifications: 0.
- **Verification:**
  - `Ashfall.Core.Tests/Radiation/Plan20ARadiationBalanceSweepTests.cs`: 9/9 PASSED.
  - `Ashfall.Core.Tests/Shelter/Plan20BShieldingBalanceSweepTests.cs`: 12/12 PASSED.

### 2.4 D1 — Distress Content Tranche
- **Problem:** Distress follow-up mechanism was complete (scheduler, save codec, trigger grammar, audio projection), but 20 primary signals lacked follow-up transmissions.
- **Solution:** Authored rich, restrained follow-up transmissions with closed trigger grammar across 12 high-traffic signals in `Assets/StreamingAssets/Data/radio_distress_signals.json`:
  1. `freq_distress_217_4` (Checkpoint Kilo): `answered`, `rescue_success`
  2. `freq_distress_148_2` (Civilian Bunker 4-East — raider trap): `ambush_encountered`
  3. `freq_distress_55_1` (The Pianist's Last Broadcast): `answered`, `expired`
  4. `freq_distress_401_9` (Military Convoy Echo-7): `answered`, `rescue_success`
  5. `freq_distress_203_1` (Water Treatment Worker): `answered`, `rescue_success`
  6. `freq_distress_311_5` (Stranded Expedition Group): `answered`, `rescue_success`
  7. `freq_distress_410_7` (Kidnap Setup — raider trap): `ambush_encountered`
  8. `freq_distress_288_1` (Faction Tactical Bait — warlord trap): `ambush_encountered`
  9. `freq_distress_333_6` (Impersonated Settlement — militia false flag): `ambush_encountered`
  10. `freq_distress_478_2` (Impersonated Medical Evacuation — raider false flag): `ambush_encountered`
  11. `freq_distress_812_5` (Petar in School — moral choice): `answered`, `rescue_success`, `expired`
  12. `freq_distress_867_9` (Siblings in Clinic — moral choice): `answered`, `rescue_success`, `expired`
- **Verification:**
  - `Ashfall.Core.Tests/Radio/DistressFollowUpTests.cs`: 21/21 PASSED.
  - `Ashfall.Core.Tests/Radio/DistressAudioCueTests.cs`: 10/10 PASSED.
  - `Ashfall.Core.Tests/Radio/DistressSignalTasks912ReplayTests.cs`: 5/5 PASSED.
  - `Ashfall.Core.Tests/Radio/` suite: 323/323 PASSED.
  - `godot --headless --path . -- --data-integrity-selftest`: PASS (0 errors across 333 catalogs).
  - `godot --headless --path . -- --audio-selftest`: PASS (630/630 assertions passed).

### 2.5 D2 — AI Tool Governance (Option C)
- **Problem:** Three AI directories (`.qwen/`, `.codex/`, `.cursor/`) were tracked in git with 178 files (~4MB) while `.agents/` represents the canonical rule/skill tree.
- **Solution:** Executed Option C:
  - Kept `.agents/` tracked as canonical.
  - Untracked `.qwen/`, `.codex/`, and `.cursor/` via `git rm -r --cached` (0 files deleted from disk).
  - Added `.qwen/`, `.codex/`, and `.cursor/` to `.gitignore`.
- **Verification:**
  - `scripts/ci/repo-hygiene-report.sh` confirmed all three directories ignored and 0 tracked files, files intact on disk.

### 2.6 D3 — Test Quarantine Promotion Continuation
- **Problem:** `Ashfall.Core.Tests/Medical/AutopsyProceduresCatalogTests.cs` was quarantined in `Twin_ASHFall/quarantine/` due to tool reference mismatches with updated medical catalogs.
- **Solution:** Restored `AutopsyProceduresCatalogTests.cs`, dropped `<Compile Remove="Medical/AutopsyProceduresCatalogTests.cs" />` from `Ashfall.Core.Tests.csproj`, rematched legacy test fixtures (`scalpel`, `forceps`, `bandage`) to canonical catalog requirements (`medical_scissors`, `protective_rubber_gloves`, `field_surgical_kit`, `clean_water`, `sterilised_bandage`). Updated `KNOWN_DEBT.md`.
- **Verification:**
  - `Ashfall.Core.Tests/Medical/AutopsyProceduresCatalogTests.cs`: 12/12 PASSED.
  - `Ashfall.Core.Tests/Medical/` suite: 371/371 PASSED (38 test classes).

---

## 3. Comprehensive Verification Summary

- **Fast Verification Suite:**
  `bash scripts/ci/verify-fast.sh`
  **Result:** ✅ **ALL 47 GATES PASSED CLEANLY (192.60s)**
- **Build Status:**
  - `Ashfall.Core`: 0 warnings, 0 errors
  - `Ashfall.Core.Tests`: 0 warnings, 0 errors
  - `Ashfall.csproj` (Godot Host): 0 warnings, 0 errors
- **Data Integrity:**
  - `godot --headless --path . -- --data-integrity-selftest`: 0 errors across 333 catalogs.
- **Audio Integrity:**
  - `godot --headless --path . -- --audio-selftest`: 630/630 passed.
