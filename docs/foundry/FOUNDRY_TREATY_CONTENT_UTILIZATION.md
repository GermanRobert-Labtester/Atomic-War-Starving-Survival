# Foundry Treaty Content Utilization Report

**Tool:** `godot --headless --path . -- --content-utilization-selftest`
**CI Gate:** PASS

---

## 1. Catalog Utilization Profile

- **Catalog:** `Assets/StreamingAssets/Data/foundry_treaty_consequences.json`
- **Total Rows Authored:** 15
- **Referenced Treaties in Catalog:** 8 distinct treaties
  - All 8 treaties resolve in `foundry_accords.json` (100% utilization rate).
- **Referenced Factions in Catalog:** 5 distinct factions
  - `faction_silent_foundry`, `faction_the_fleet`, `faction_ash_sign`, `faction_central_garrison`, `faction_the_scale`
  - All 5 factions resolve in `factions.json` / `foundry_faction.json` (100% resolution).
- **Referenced Market Goods in Catalog:** 8 distinct goods
  - `coal`, `fuel`, `clean_water`, `canned_food`, `water_filter`, `scrap_metal`, `item_foundry_brine_pipe`, `item_foundry_ice_anchor`
  - All 8 goods resolve in `economy_goods.json` and item definitions (100% resolution).

---

## 2. CI Verification Evidence

```
[Ashfall Godot] Initializing ASHFALL: Atomic War - Starving Survival...
Catalog Boot Report:
  Total: 29 (Required: 13, Optional: 16, DevOnly: 0)
  Success: 29
  Warnings: 0
  Errors: 0

DATA_INTEGRITY_SELFTEST PASS — 0 findings (11104 ids authored, 3917 reuses reserved) — 0 errors, 0 warnings across 215 catalogs
CONTENT_UTILIZATION_SELFTEST PASS — CI gate: PASS
```

Zero orphaned entries, zero dangling foreign keys, and zero unreferenced catalog rows.
