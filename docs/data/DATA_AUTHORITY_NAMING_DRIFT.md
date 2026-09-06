# ASHFALL — Data Authority Naming Drift Inventory & Migration Notes
## Plans 02–09 Flagship Residual Hygiene Audit

**Date:** 2026-09-06
**Status:** Canonical Audit Baseline
**Policy:** Never bulk-rename stable JSON keys without backwards compatibility. Authoritative snake_case is standard for all new data; legacy camelCase is supported via loader aliases or explicit normalization.

---

### 1. Catalog Key Drift Classification

| Catalog Family | Wire Key Example | Canonical Target | Loader Handling | Classification | Action / Safety Policy |
|---|---|---|---|---|---|
| items.json | displayName | display_name | Supported in ItemDefinition via JSON serializer property mapping | DTO/property naming compatibility | Retain dual-read in loader; new catalogs emit canonical snake_case. |
| items.json | maxStack | max_stack | Read by ItemCatalogLoader | Legacy-compatible key | Do not mass-rename; preserves compatibility with external tools and saves. |
| locations.json | dangerLevel | danger_level | LocationCatalogLoader supports both dangerLevel and danger_level | Loader alias | Dual-fallback implemented in ExpeditionCatalogLoader. |
| locations.json | scavengeTableId | scavenge_table_id | Read by WastelandMapSystem / ExpeditionSystem | Legacy-compatible key | Stable wire format preserved. |
| relic_recipes.json | relic_id, research_unlock_id | relic_id, research_unlock_id | Fully canonical snake_case | Canonical | 0 drift; all 16 research unlocks statically verified in research_knowledge.json. |
| vinyl_record_archive.json | record_id, daily_morale_modifier | record_id, daily_morale_modifier | Loaded by VinylRecordCatalog and LoadVinylRecordCatalog | Canonical | 0 drift; all 30 records conform to snake_case schema. |
| disease_catalog.json | id, display_name, tell, treatments | snake_case throughout | Loaded by DiseaseCatalog | Canonical | Schema version 3, zero naming drift across 15 diseases. |
| survivor_letters_lost_kin.json | letter_id, pigeonhole_number | snake_case throughout | Loaded by SurvivorLetterCatalog | Canonical | Schema version 1, zero naming drift across 25 letters. |
| cassette_sets.json | set_id, total_parts | snake_case throughout | Loaded by CassetteSetCatalog | Canonical | Schema version 1, zero naming drift across cassette sets. |

---

### 2. Migration Invariants

1. **Root Catalogs:** All root catalogs carry schema_version validated by CatalogIntegrityValidator.
2. **Deterministic Deserialization:** Loaders must never throw on valid legacy fields; they map either exact snake_case or canonical aliases.
3. **No Shadow Catalogs:** Never resolve naming drift by creating a second catalog file.
