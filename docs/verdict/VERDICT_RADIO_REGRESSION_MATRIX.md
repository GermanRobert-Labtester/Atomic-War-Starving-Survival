# Verdict Radio Regression & Stability Matrix

> **Verification Suite:** `Ashfall.Core.Tests/Verdict/VerdictRadioExpansionTests.cs` (10 tests)
> **Regression Baseline:** 143 Verdict tests, 7,000+ total core tests, 5 host self-tests.

---

## 1. Automated Gate Verification Matrix

| Area | Verified Invariant | Test Method / Gate | Result |
|---|---|---|---|
| **Catalog Count** | Exactly 30 broadcasts parsed | `VerdictRadioExpansionTests.Catalog_Loads_All_30_Broadcasts` | **PASS** |
| **Identifier Hygiene** | All 30 IDs unique and start with `radio_verdict_` | `VerdictRadioExpansionTests.All_30_Broadcast_Ids_Are_Unique_And_Prefixed` | **PASS** |
| **Baseline Preservation** | All 13 original broadcasts preserved verbatim | `VerdictRadioExpansionTests.Baseline_13_Broadcasts_Preserved_Verbatim` | **PASS** |
| **Expansion Quality** | All 17 new broadcasts present; terseness budget <= 250 chars | `VerdictRadioExpansionTests.All_17_Plan94_New_Broadcasts_Present` | **PASS** |
| **Kind Taxonomy** | 5 telemetry, 4 maintenance, 3 census, 2 calibration, 2 anomaly, 1 emergency | `VerdictRadioExpansionTests.Plan94_Requested_Kind_Distribution_Matches` | **PASS** |
| **Frequency & Strength** | Valid bands (`99.0 MHz`, `88.5 MHz`) & levels (`S1`–`S4`) | `VerdictRadioExpansionTests.Frequency_And_Signal_Strength_Integrity` | **PASS** |
| **Timing & Gating** | Day 210 window, Culpable phase prerequisite, chronology | `VerdictRadioExpansionTests.DayTrigger_Semantics_And_Chronology` | **PASS** |
| **Save / Restore** | One-shot firing, fired IDs round-trip, no re-triggering | `VerdictRadioExpansionTests.OneShot_And_State_RoundTrip` | **PASS** |
| **Unified Radio** | 30 broadcasts registered into `UnifiedRadioBroadcast` catalog | `VerdictRadioExpansionTests.UnifiedRadioBroadcast_Catalog_Loads_Verdict_Broadcasts` | **PASS** |
| **Audio Cue Hygiene** | No un-registered audio cues authored | `VerdictRadioExpansionTests.AudioCueIntegrity_No_New_Broadcasts_Define_Dangling_Cues` | **PASS** |
| **Data Integrity Gate** | 0 errors across 208 catalogs | `godot --headless --path . -- --data-integrity-selftest` | **PASS** |
| **Content Utilization** | Clean boot, 0 orphaned catalogs | `godot --headless --path . -- --content-utilization-selftest` | **PASS** |
| **Scene Binding** | 22/22 scenes verified | `godot --headless --path . -- --scene-binding-selftest` | **PASS** |
| **Scene Lint** | 0 errors across 27 scenes | `python3 scripts/ci/scene-lint.py` | **PASS** |
| **Dotnet Host Build** | 0 errors, 0 warnings | `dotnet build Ashfall.csproj` | **PASS** |
