# Plan 137 — Save Compatibility & Schema Invariant Analysis

## 1. Executive Summary
Plan 137 preserves **Invariant 3 (Cross-Host Save Compatibility)** in full. It introduces **ZERO** new custom save files, unversioned JSON blobs, or alterations to existing save envelope schemas.

---

## 2. Save Envelope & Store Analysis

### 2.1 Static Authority vs. Dynamic State
- **Static Catalogs:** `expansion_survivor_fields.json`, `expansion_item_tags.json`, `deep_lore_survivor_fields.json`, and `antigravity_survivor_fields.json` are read-only static streaming asset catalogs loaded into memory at startup. They are never serialized into campaign save files.
- **Survivor Definitions:** `SurvivorRosterSystem` continues to save survivor instances with existing IDs, health, morale, and work assignments via `SurvivorSaveStore`.
- **Enrichment Projection:** Enrichment data is projected dynamically at runtime via `SurvivorEnrichmentService.GetView(survivorId)` using the static catalog and the survivor's persistent ID.

### 2.2 Keepsake Discovery Persistence
- When a keepsake is recognized by a survivor upon arrival in inventory, the event is recorded in the settlement's knowledge ledger:
  `keepsake_recognized:{survivorId}:{itemId}`
- This string key is saved inside the existing, checksum-protected `JournalSaveStore` (`KnowledgeLedger` sub-structure).
- Existing save files seamlessly load without knowledge of new keys; old saves simply have an empty keepsake discovery set.
- New saves containing keepsake discoveries remain 100% backward- and forward-compatible.

### 2.3 Verification Matrix
| Store | Schema Changed? | Checksum Impact | Migration Codec Required? |
|---|---|---|---|
| `SurvivorSaveStore` | NO | None (unmodified DTO) | None |
| `JournalSaveStore` | NO | Checksum covers knowledge keys automatically | None |
| `InventorySaveStore` | NO | Unmodified items and counts | None |
| `WorldSaveStore` | NO | Unmodified | None |

---

## 3. Conclusion
Plan 137 achieves zero save-breaking changes, requires no migration codecs, and introduces zero unversioned state.
