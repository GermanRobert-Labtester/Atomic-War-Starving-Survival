# Archive Inks Regression Matrix

> **Verification Matrix:** Complete test suite mapping for Plan 78.

---

| # | Test Scenario | Subsystem | Expected Outcome | Status |
|---|---|---|---|---|
| 1 | **Catalog Load Count** | `ArchiveInkCatalogLoader` | Exactly 12 inks loaded from `archive_inks.json` | Verified |
| 2 | **Preserve Baseline Inks** | `ArchiveInkCatalogLoader` | `ink_iron_gall`, `ink_soot_lamp`, `ink_plant_dye` match exact baseline values | Verified |
| 3 | **Unique Ink IDs** | `ArchiveDeskSystem` | All 12 IDs are unique and prefixed with `ink_` | Verified |
| 4 | **Unique Display Names** | `ArchiveDeskSystem` | All 12 display names are non-empty and unique | Verified |
| 5 | **Ingredient Resolution** | `ArchiveInkCatalogLoader` | All `required_item_id` foreign keys resolve in `items.json` | Verified |
| 6 | **Positive Amount Bounds** | `ArchiveDeskSystem` | All `required_amount` are in range `[1, 5]` | Verified |
| 7 | **Numeric Bounds: Legibility**| `ArchiveDeskSystem` | All `legibility_score` values are in `[0.3, 1.0]` | Verified |
| 8 | **Numeric Bounds: Longevity** | `ArchiveDeskSystem` | All `archival_longevity_days` values are in `[50, 1000]` | Verified |
| 9 | **Numeric Bounds: Fade Rate** | `ArchiveDeskSystem` | All `fade_rate_per_day` values are in `[0.0005, 0.02]` | Verified |
| 10 | **No Duplicate Profiles** | `ArchiveDeskSystem` | No two inks share the same (legibility, longevity, fade, item, amount) tuple | Verified |
| 11 | **Pareto Anti-Dominance** | `ArchiveDeskSystem` | No ink dominates another across all quality and cost axes | Verified |
| 12 | **Inventory Payment Check**| `ArchiveDeskSystem` | Queuing job consumes exact `required_amount` from inventory | Verified |
| 13 | **Cancellation Refund** | `ArchiveDeskSystem` | Cancelling job refunds exact `required_amount` to inventory | Verified |
| 14 | **Legibility Propagation** | `ArchiveDeskSystem` | Completed job retains ink's authored `legibilityScore` | Verified |
| 15 | **Save State Round-Trip** | `ArchiveDeskSystem` | Queued jobs and completed transcriptions persist cleanly | Verified |
| 16 | **Host Session Loading** | `ArchiveDeskHostSession` | `LoadInkCatalog(dataDir)` populates all 12 inks into host | Verified |
| 17 | **UI Panel Rendering** | `ArchiveDeskPanel` | All 12 formulation cards render in UI scroll container | Verified |
