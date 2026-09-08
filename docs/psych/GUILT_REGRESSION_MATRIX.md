# Plan 66 — Guilt Sources Expansion: Regression Matrix

**Document Version:** 1.0.0
**Authority:** `Assets/StreamingAssets/Data/guilt_sources.json` (40 triggers)
**System:** `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs`
**Test Suite:** `Ashfall.Core.Tests/GuiltSourcesPlan66CatalogTests.cs` (8/8 passing), `Ashfall.Core.Tests/GuiltInsomniaSystemTests.cs` (15/15 passing)

---

## 1. 20 Regression Scenarios & Verification Matrix

| # | Regression Scenario / Contract Invariant | Expected Behavior & Boundary Condition | Verification Target / Method | Status |
|---|---|---|---|---|
| **1** | **Catalog Size & Schema Version** | Catalog contains exactly 40 items; `schema_version` is integer `1`. | `Catalog_LoadsAndHasExactly40GuiltSources` | PASS |
| **2** | **Baseline 20 Preservation** | Original 20 choice patterns, titles, and severities preserved in exact order and values. | `Catalog_LoadsAndHasExactly40GuiltSources` | PASS |
| **3** | **Plan 66 Category Distribution** | Exactly 20 new sources across 8 categories: 4 Resource, 3 Shelter, 3 Expedition, 3 Combat, 3 Social, 2 Medical, 1 Scavenging, 1 Leadership. | `Catalog_CategoryDistribution_MatchesPlan66Specification` | PASS |
| **4** | **Choice Pattern Uniqueness** | 40 distinct `choice_pattern` strings; 0 duplicate patterns across entire catalog. | `Catalog_AllChoicePatternsAndTitles_AreUniqueAndValid` | PASS |
| **5** | **Title Uniqueness** | 40 distinct `title` strings; 0 duplicate titles. | `Catalog_AllChoicePatternsAndTitles_AreUniqueAndValid` | PASS |
| **6** | **Title Word Count Constraints** | Every title contains between 2 and 5 words inclusive. | `Catalog_AllChoicePatternsAndTitles_AreUniqueAndValid` | PASS |
| **7** | **Description Phrasing & `{name}`** | All 20 new entries contain non-empty descriptions with `{name}` placeholder for survivor name interpolation. | `Catalog_AllChoicePatternsAndTitles_AreUniqueAndValid` | PASS |
| **8** | **Restrained Non-Moralizing Tone** | Zero moralistic rhetoric ("evil", "wrong"); descriptions anchor on sensory, physical, and procedural details. | Narrative review & catalog audit | PASS |
| **9** | **Severity Range Integrity** | All severity values strictly within `0.10` to `1.00` inclusive. | `Catalog_AllChoicePatternsAndTitles_AreUniqueAndValid` | PASS |
| **10** | **Severity Calibration Distribution** | Balanced across minor/moderate (10), moderate/high (13), and severe/devastating (17); not top-heavy. | `Catalog_SeverityDistribution_IsWellCalibrated` | PASS |
| **11** | **Deterministic Severity Accumulation** | `GuiltInsomniaSystem.RecordGuilt` adds exact authored severity to survivor state without drift. | `GuiltInsomniaSystem_All20NewSources_AccumulateSeverityDeterministically` | PASS |
| **12** | **Record Event Dispatch** | `OnGuiltRecorded` fires with correct survivor ID, sourceId, and severity for all new triggers. | `GuiltInsomniaSystem_All20NewSources_AccumulateSeverityDeterministically` | PASS |
| **13** | **Critical Insomnia Threshold** | Sources with severity >= `0.70` trigger `OnGuiltInsomniaCritical` event. | `GuiltInsomniaSystem_HighSeveritySources_TriggerCriticalInsomniaThreshold` | PASS |
| **14** | **Severity Capping at 1.0** | Multiple stacking sources correctly clamp total `insomniaSeverity` to `1.00`. | `RecordGuilt_MultipleSources_CapsAt1` | PASS |
| **15** | **Sedative Mitigation** | `ApplySedative` provides 12 hours of relief and reduces severity by `0.40`. | `ApplySedative_ReducesSeverity` | PASS |
| **16** | **Interpersonal Dialogue Relief** | `ResolveGuiltThroughDialogue` removes the most recent guilt record and updates insomnia severity. | `GuiltInsomniaSystem_ExpiryAndDialogueResolution_OperateCorrectly` | PASS |
| **17** | **30-Day Natural Expiry** | Guilt records older than 30 game days expire during daily tick. | `GuiltInsomniaSystem_ExpiryAndDialogueResolution_OperateCorrectly` | PASS |
| **18** | **Sleep Quality Penalty Scaling** | Insomnia scales sleep quality penalty by `0.50` per severity point (minimum `0.10`). | `SleepQuality_LowerWithGuilt` | PASS |
| **19** | **Save/Load Full Round-Trip** | `GuiltInsomniaSaveState` preserves all survivors, guilt records, day stamps, and severities. | `GuiltInsomniaSystem_SaveLoad_FullRoundTrip_PreservesGuiltRecords` | PASS |
| **20** | **Global Project Invariants** | Zero engine coupling in Core (`Ashfall.Core`), 100% deterministic simulation, JSON data authority. | `--data-integrity-selftest` & CI gates | PASS |

---

## 2. Risk Mitigation Summary

1. **Risk 1: Duplicate Choice Patterns** — Mitigated by automated uniqueness assertion across all 40 patterns (`Catalog_AllChoicePatternsAndTitles_AreUniqueAndValid`).
2. **Risk 2: Dead Trigger Entries** — Mitigated by `GUILT_EMITTER_MAP.md` assigning every pattern to an existing or scheduled gameplay system.
3. **Risk 3: Severity Inflation** — Mitigated by `Catalog_SeverityDistribution_IsWellCalibrated` verifying balanced spread across bands.
4. **Risk 4: Moralizing Prose** — Mitigated by strict authoring guidelines focusing on physical memory, recorded dialogue, and sensory artifacts.
5. **Risk 5: Save Corruption** — Mitigated by `GuiltInsomniaSystem_SaveLoad_FullRoundTrip_PreservesGuiltRecords` testing multi-survivor state round-trips.
