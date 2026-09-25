# ASHFALL — UI Asset Fallback Audit (2026-09-25)

Scope: which UI art resolves to real assets and which silently resolves through
fallback chains, ahead of the first texture pass. Evidence: `artifacts/asset_registry.json`
(regenerated from data + disk), direct file checks, and
`godot --headless --path . -- --asset-registry-selftest`.

## Verdict

- **No hard failures**: `--asset-registry-selftest` PASS — `checked=55 passed=55
  missing=0 load-failed=0 probe-failures=0`.
- **1,746 registered ids: 509 loaded (29.15%), 1,237 fallback** — every fallback
  resolves to a real placeholder (`res://assets/ui/Icons/icon_placeholder.png`),
  so nothing renders as a broken/blank texture.
- **Update 2026-09-25 (emblem/portrait waves)**: faction category is now
  **98/98 loaded (0 fallback)** — 18 new emblems rendered and mapped through
  `FactionIconCatalog`, 2 alias ids mapped to shipped art, and the host self-test
  now loads all 68 mapped emblems end-to-end (`[AssetRegistrySelfTest] Faction
  emblems: 68/68 resolve through the icon catalog`). Portraits are mid-wave
  (138/267, rate-limited); after the wave the registry total stands at
  **1,746 ids: 634 loaded (36.3%), 1,112 fallback** and will be refreshed in the
  wave close-out. The untextured-UI remainder is unchanged: locations 331/385,
  items 652/996.

## UI chrome chains (all resolve)

| Chain | Primary on disk | Result |
|---|---|---|
| Panel frame (`MakePanel`) | `assets/ui/Textures/frame_9slice.png` ✔ | 9-slice frame used |
| Panel frame fallback | `panel_bg_9slice.png` ✔ | spare |
| Buttons (`MakeButton`) | `btn_default/hover/pressed/disabled.png` all ✔ | raster button family used |
| Helpers/flat fallback | theme `AshfallUiTheme` flat styleboxes | consistent dark chrome |
| Fonts | `assets/fonts/BarlowCondensed-*.ttf`, `ShareTechMono-Regular.ttf` ✔ | load confirmed in selftest |

No chrome element depends on a missing file; the fallback chains are redundancy,
not active debt.

## Content coverage by category (registry truth)

| Category | Total | Loaded | Fallback | Player exposure |
|---|---|---|---|---|
| Items | 996 | 344 (34.5%) | **652** | Inventory, market, crafting, foundry rows |
| Portraits | 267 | 110 (41.2%) | **157** | Survivor list/detail, relationships |
| Locations | 385 | 54 (14.0%) | **331** | Map/atlas, expedition routes, waypoints |
| Factions | 98 | 1 (1.0%) | **97** | Faction matrix, stance panels, radio |

Every faction except one currently shows the placeholder emblem — the
highest-visibility gap per asset.

## Actionable production batches (fallback ids, descending)

- **Items (652)** — largest by prefix: `item_collectible_*` (40),
  `item_document_*` (33), `item_foundry_*` (29), `item_decor_*` (23),
  `item_greenhouse_*` (16), `item_seed_*` (11), `item_metallurgy_*` (11),
  `item_fermentation_*` (8), `item_chem_*` (7), `cassette_station_*` (6).
  Batch by prefix so one art pass completes a whole gameplay domain.
- **Factions (97)** — e.g. `faction_archivists`, `faction_lamplighters`,
  `faction_quiet_house`, `faction_grain_exchange`, `faction_sun_seekers`,
  `faction_osteophages`, `faction_the_tally`, `faction_undertow`,
  `faction_cold_count`, `faction_deserter_coalition`.
- **Locations (331)** — e.g. `loc_veterinary_surgery`, `loc_school_gymnasium`,
  `loc_cider_press`, `loc_terrace_pumphouse`, `loc_ration_queue_plaza`,
  `loc_municipal_archive`, `loc_printworks`, `loc_department_store`.
- **Portraits (157)** — named survivor portraits (`alex_raymond`, `jamie_chen`,
  `taylor_morgan`, …) plus archetype portraits.

## Recommended order for the texture pass

1. **Factions** — 97 items, one per faction; unlocks the entire political/radio
   layer's readability after a single art session.
2. **Portraits** — 157; the survivor journey is the emotional spine of the game.
3. **Locations** — 331; the map/atlas surfaces look sparse without them.
4. **Items** — 652, batched by prefix; highest count but least individually
   critical, and the placeholder reads acceptably at row sizes.

## Regeneration / verification

- Registry source of truth: regenerate via its generator (see
  `artifacts/asset_registry.md` provenance) and re-run
  `--asset-registry-selftest` after each art tranche.
- Fallbacks are silent by design at runtime; this audit is the visibility
  mechanism, so no panel needs to change when art lands.
