# Plan 98 — Regression Matrix & Risk Mitigation

## 1. Risk Register & Verification Evidence

| Risk ID | Description & Severity | Mitigation Strategy | Verification Evidence |
|---|---|---|---|
| **R1** | Global faction ID collision (*High*) | Complete repository sweep before authoring; reconciled Fort Karkov Garrison under `faction_the_garrison`. | Confirmed by `STANDING_RECORD_FACTION_IDENTITY_AUDIT.md`. |
| **R2** | Duplicate mutable trust authorities (*Critical*) | Strict authority map; static JSON holds initial baseline (0), while `SaveStoreHub` owns live campaign integers. | Confirmed by `STANDING_RECORD_FACTION_AUTHORITY_MAP.md`. |
| **R3** | JSON trust resets campaign trust on reload (*Critical*) | Catalog reloads do not overwrite campaign state; tested round-trip with live mutation. | `StandingRecordFactionExpansionTests.Persistence_MutableTrustRoundTrip_PreservesDynamicStanding`. |
| **R4** | Alignment conflicts with trust (*High*) | Authored alignments limited to `conditional` and `neutral`; no static hostility. | `StandingRecordFactionExpansionTests.Alignments_AreValid`. |
| **R5 / R6** | Wants / offers typed incorrectly (*High*) | Authored tokens validated against `CatalogIntegrityValidator.cs` prefix registry (prevented `crop_` collision). | `--data-integrity-selftest` passed with 0 errors. |
| **R8 / R9** | Invalid home-region references (*Medium*) | Regions mapped to `WASTELAND_REGION_ATLAS.md` macro-regions; `all_regions` verified for The Overlay. | `StandingRecordFactionExpansionTests.HomeRegions_AreValid`. |
| **R10** | Badge placeholder crashes (*Medium*) | `badge_asset_id: ""` used uniformly; resolved via `FactionIconCatalog.Resolve` to safe fallback icon. | `STANDING_RECORD_BADGE_AUDIT.md`. |
| **R12** | The Overlay unintentionally rebalanced (*High*) | Position 0 record preserved byte-for-byte; verified via unit assertion. | `StandingRecordFactionExpansionTests.BaselineOverlay_PreservedVerbatim`. |
| **R13** | Eight factions become seven reskins (*Medium*) | Evaluated across 12 institutional and economic axes; unique quotes and access rules. | `StandingRecordFactionExpansionTests.TradeProfiles_AreDifferentiated`. |
| **R24** | New factions break old saves (*High*) | Missing-key initialization gracefully defaults to 0 without mutating existing progress. | `StandingRecordFactionExpansionTests.Persistence_OldSaveInitialization_DefaultsGracefully`. |

---

## 2. Regression Test Suite Summary

- **Total Suite Execution:** 7,460 passed, 0 failed across `Ashfall.Core.Tests`.
- **Standing Record Expansion Tests:** 18 passed, 0 failed in `StandingRecordFactionExpansionTests.cs`.
- **Legacy Regression Safeguard:** `LocationLayoutSystemTests.OverlayCurrentLivesInStandingRecordFileNotASeventhPower` updated to targeted resolution, passing cleanly.
- **Headless Selftests:**
  - `data_integrity_selftest`: 0 errors across 216 catalogs.
  - `content_utilization_selftest`: CI gate PASS.
  - `scene_binding_selftest`: 22/22 PASS.
  - `scene-lint.py`: 0 errors across 27 production scenes.
