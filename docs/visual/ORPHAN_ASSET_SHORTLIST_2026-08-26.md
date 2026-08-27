# ASHFALL — Orphan Asset Review Shortlist (Non-Destructive)

**Date:** 2026-08-26
**Scope:** 10 curated orphan asset candidates identified across `assets/art/`, `assets/sprites/`, and `assets/ui/Screens/` with zero catalog or runtime code references.
**Policy:** **Strictly Non-Destructive.** Reporting only for human review; no assets or `.import` files were moved or deleted.

---

## 1. Summary of Orphan Landscape

- **Total on-disk visual assets in `assets/`:** ~2,335 image files.
- **Unreferenced Assets:** ~1,877 files (comprising legacy art assets, unmapped item/faction variants, Stitch UI ideation concepts, and deprecated ammo files).
- **Runtime Impact:** Zero runtime impact. Active systems resolve canonical assets via [`src/Host/AssetRegistry.cs`](../../src/Host/AssetRegistry.cs) and procedural fallbacks.

---

## 2. Curated 10-Candidate Human Review Shortlist

| # | File Path | File Size | Asset Category | Review Rationale & Context | Suggested Review Action |
|---|---|---|---|---|---|
| 1 | [`assets/art/ammo_deprecated_cal_12ga.png`](../../assets/art/ammo_deprecated_cal_12ga.png) | 208.4 KB | Deprecated Ammo Caliber | Legacy shotgun shell artwork from pre-unification inventory. The live catalog uses `ammo_12ga` (`assets/art/ammo_12ga.png`). | Candidate for retirement to legacy archive. |
| 2 | [`assets/art/ammo_deprecated_cal_16ga.png`](../../assets/art/ammo_deprecated_cal_16ga.png) | 239.5 KB | Deprecated Ammo Caliber | Legacy 16-gauge ammo art with no active weapon or recipe consumer in `items.json`. | Candidate for retirement to legacy archive. |
| 3 | [`assets/art/item_resin_adhesive.png`](../../assets/art/item_resin_adhesive.png) | 1.2 KB | Unindexed Crafting Item | Crafting adhesive artwork not authored into `items.json` or `recipes.json`. | Candidate for future crafting expansion item ID assignment. |
| 4 | [`assets/art/item_lead_plate.png`](../../assets/art/item_lead_plate.png) | 1.1 KB | Unindexed Material Item | Radiation shielding lead plate sprite not referenced by current shelter upgrade recipes. | Candidate for future bunker fortification recipe integration. |
| 5 | [`assets/sprites/Factions/faction_central_garrison_remnants.png`](../../assets/sprites/Factions/faction_central_garrison_remnants.png) | 0.6 KB | Unreferenced Faction Emblem | Faction insignia variant for garrison remnants; live catalog uses `iron_garrison` / `faction_garrison`. | Candidate for alias mapping in `AssetRegistry.cs`. |
| 6 | [`assets/sprites/Factions/faction_cultists_of_the_glow.png`](../../assets/sprites/Factions/faction_cultists_of_the_glow.png) | 0.5 KB | Unreferenced Faction Emblem | Radiation cult insignia; live narrative catalogs reference `cult_of_ash_sign` and `faction_ash_sign`. | Candidate for alias mapping in `AssetRegistry.cs`. |
| 7 | [`assets/sprites/Items/aa_batteries_package_10.png`](../../assets/sprites/Items/aa_batteries_package_10.png) | 0.4 KB | Unindexed Item Packaging Variant | 10-pack battery sprite; live catalog indexes single battery item `battery_aa` / `scrap_electronics`. | Potential bulk salvage or container reward item. |
| 8 | [`assets/sprites/Items/accelerant_full.png`](../../assets/sprites/Items/accelerant_full.png) | 0.4 KB | Unindexed Item State Variant | Full canister accelerant sprite; current catalog references `item_accelerant`. | Candidate for item condition or fuel state representation. |
| 9 | [`assets/ui/Screens/01_ashfall_-_subterranean_mining_geological_excavation_terminal.png`](../../assets/ui/Screens/01_ashfall_-_subterranean_mining_geological_excavation_terminal.png) | 83.8 KB | UI Screen Ideation Mockup | Stitch design concept export for mining excavation terminal (reference library output). | Retain in design reference archive; do not load in runtime. |
| 10 | [`assets/ui/Screens/02_ashfall_-_long-range_radio_intercept_morse_decryption_array.png`](../../assets/ui/Screens/02_ashfall_-_long-range_radio_intercept_morse_decryption_array.png) | 88.2 KB | UI Screen Ideation Mockup | Stitch design concept export for radio decryption array (reference library output). | Retain in design reference archive; do not load in runtime. |

---

## 3. Human Review Instructions

1. **Review Action Only**: Do not delete files directly.
2. **Catalog Mapping Candidates** (Items #3, #4, #7, #8): Can be assigned new catalog IDs in `Assets/StreamingAssets/Data/items.json` or recipes in `recipes.json`.
3. **Alias Candidates** (Items #5, #6): Can be added to `AssetRegistry.ItemIdAliases` or `FactionAliases` in `src/Host/AssetRegistry.cs`.
4. **Design Reference Artifacts** (Items #9, #10): Retain under `assets/ui/Screens/` as reference benchmarks for Godot panel themes.
