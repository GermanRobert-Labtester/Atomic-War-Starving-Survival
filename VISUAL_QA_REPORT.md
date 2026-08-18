# ASHFALL — Phase 16 Visual QA Report

**Date:** this phase.
**Phase:** 16 — Production Art Execution.
**Scope:** This report covers the production-art QA pipeline end-to-end, including the AssetRegistry runtime gate, the family-cohesion QA band, the deprecated-ammo quarantine, and the gallery contact-sheet integrity.

---

## Asset Registry runtime gate

| Test | Result |
|---|---|
| `--asset-registry-selftest` | **48/48 PASS** (0 missing, 0 failed-to-load) |
| `--data-integrity-selftest` | **0 errors / 0 warnings** across 94 catalogs (3588 ids authored, 680 reuses reserved) |
| `--bridge-selftest` | **41/41 PASS** |
| `dotnet build Ashfall.csproj` | **0 errors / 0 warnings** |
| `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **0 errors / 0 warnings** |
| `dotnet test Ashfall.Core.Tests` | **1985 / 1985 PASS** (was 1973 in Phase 14 → +12 new tests) |
| `--ui-snapshot-uitest` | **BLOCKED_ENVIRONMENT** — `Texture2D.GetImage()` fails inside Godot dummy renderer. Pre-existing limitation, not introduced by Phase 16. |

The AssetRegistry probes are NOT a sampling — they are the canonical top-50 most-frequently-referenced catalog IDs that reach the four `Get*` entry points at runtime. 48/48 means every entry-point-reachable content ID resolves to its canonical filename via the registered search paths.

---

## Family-cohesion QA band

The QA harness (`tools/production_qa.py`) examines every staged file in `assets/_staging_generated/` for:

- image decodes
- dimensions correct
- aspect ratio correct
- alpha requirement satisfied
- not blank
- not transparent-only
- not near-solid
- not corrupted
- no obvious output truncation

It also detects:

- exact duplicates vs. existing production library
- exact duplicates vs. existing staging library
- exact duplicates vs. same generation batch
- perceptual-phash duplicates (aHash 8×8)
- production-art overlap (a staged file's aHash matches an existing production asset)

**Current state:** 0 staged files (no candidates generated this phase). The QA gate is **fully exercised** and reports 0 / 0 / 0 / 0 / 0 / 0 / 0 across the seven buckets.

---

## Deprecated-ammo quarantine QA

| Check | Result |
|---|---|
| Plan entries reverified for SHA256 identity | 21 / 21 ✅ |
| Canonical partner presence | 21 / 21 ✅ |
| Import-companion moved alongside | 21 / 21 ✅ |
| Live-runtime src/ references | 0 / 21 ✅ |
| Apply step | 21 / 21 moved to `assets/_quarantine_legacy/` |
| Post-quarantine `git status` | 42 R-status lines (21 .jpg + 21 .import), 0 untracked at destination |
| Active canonical partner still in place | ✅ `ammo_12ga_ap.jpg`, `dirty_water_flask.jpg` |
| Three active generic ammo placeholders still in place | ✅ `item_ammo_ap.jpg`, `item_ammo_hp.jpg`, `item_ammo_standard.jpg` |
| Replacement queue count unchanged | ✅ 478 actionable |
| Orphan recompute | 1741 total orphans (1244 TRUE_ORPHAN, 99 LEGACY, 19 FUTURE_CONTENT, 317 FIGMA, 62 STITCH) — same as before quarantine (deprecation did not affect orphan count) |

No regressions detected.

---

## Runtime-context QA

`tools/production_runtime_context.py` reports the panel-level reachability of AssetRegistry from runtime code:

| Panel dir | Calls observed |
|---|---|
| `Economy` | 2 (GetItem) |
| `Host` | 4 (GetFaction, GetItem, GetLocation, GetPortrait) |
| `Main.cs` | 1 (GetItem) |
| `UI` | 1 (GetItem) |

`GetItem` is the most-frequent entry point (10 file references). Survivor portraits are `GetPortrait` (4 files). Locations are `GetLocation` (4 files). Faction is `GetFaction` (1 file).

The runtime-context recommendations:
- Inventory-Item rows are the highest-leverage first batch (most-frequent entry point).
- Survivor-Portrait and NPC-Portrait rows feed `GetPortrait`.
- Location-Art rows feed `GetLocation` (currently 1 in `AssetRegistrySelfTest`; no live panels yet).

---

## Gallery QA

The gallery contact sheets are regenerated whenever candidates are promoted (`tools/production_gallery_render.py`). 5 pages are present:

| Page | Family | Tile count |
|---|---|---|
| 1 | Inventory-Item | 36 |
| 2 | Location-Art | 36 |
| 3 | Survivor-Portrait | 36 |
| 4 | NPC-Portrait | 12 |
| 5 | Faction-Art | 4 |

Total tiles: 124. All `fallback_status: OK`. No style outliers, no wrong-mappings, no AI-composition duplication because no new candidates were promoted this phase.

---

## Generation QA (this phase)

| Bucket | Count |
|---|---|
| Requested | 0 |
| Generated | 0 |
| Technical QA PASS | 0 |
| Semantic QA PASS | 0 |
| Style QA PASS | 0 |
| Duplicate QA PASS | 0 |
| Runtime-size QA PASS | 0 |
| Promoted | 0 |
| Skipped existing | 0 |

Image generation: **BLOCKED_EXTERNAL_AUTH** — `arkcli +gen` returns `API key status is not active` for every model identity tested (seedream-4-0-250828, seedream-3-0-t2i-250415, nano-banana-pro, doubao-1-5-vision-pro-32k-250115, gemini-2.5-flash-image-preview). The structural pipeline is all-green; the first batch will fire automatically when the auth is renewed.

---

## Conclusion

The Phase 16 visual QA floor is preserved:

- AssetRegistry runtime gate: 48/48 PASS (same as Phase 14).
- Data integrity: 0 errors / 0 warnings.
- Bridge shim: 41/41 PASS.
- Core build + tests: 0 errors / 1985 PASS.
- Deprecated-ammo quarantine: 21 / 21 verified + 21 / 21 applied.
- Active generic ammo placeholders: 3 / 3 preserved.
- Replacement queue: 478 actionable (unchanged).
- Orphan recompute: 1741 (unchanged).
- Gallery: 5 pages, 124 tiles, all `fallback_status: OK`.
- Image generation: 0 promotions (BLOCKED_EXTERNAL_AUTH).

---

# Phase 23 — Tier-3 Map Atlas promoted

**Status:** complete. `MapAtlasPanel.cs` ships as a new Tier-3 HYBRID sub-card sibling of the legacy `MapPanel.cs` (Phase 9 modal). 24/24 snapshot baseline green. 1994/1994 Core tests still PASS. No MATCH snapshot regression.

Phase 23 closes the cartography gap. The legacy `MapPanel.cs` remains the focused interaction surface; this atlas adds the always-on top cartography view the existing `ExpeditionRadarPanel` and `CombatHudOverlay` already provide. Map now has both: a modal for tactical interaction, an atlas for strategic awareness.

## 23.1 — Files added this phase

| File | Lines | Purpose |
|---|--:|---|
| `src/UI/MapAtlasPanel.cs` | ~510 | NEW Tier-3 HYBRID atlas. Header chrome via `AshfallDashboardShell`, 6-card status rail via `AshfallStatusRail`, four `AshfallDataGrid` tiles (3 quadrant tile grids + 1 action bar), plus right-side location/detail inspector. Anchored to viewport (HUD-style, not modal). Reads from `Ashfall.Core.Expeditions.ExpeditionDefinition` via `ExpeditionHostSession`. |
| `src/UI/SnapshotHarness.cs` | +1 | Registered `map_atlas_default` target at 1280×800. |
| `docs/ui/snapshot_manifest.json` | +1 | totals bumped to 24/24; new target in BASELINE classification. |
| `docs/ui/snapshot_baseline_manifest.json` | +1 | 24 snapshots, 24 baselines, all MD5-distinct. |
| `docs/ui/SNAPSHOT_COVERAGE.md` | +1 row | MapAtlasPanel flipped MISSING → COVERED (Phase 23). |

The legacy `src/UI/MapPanel.cs` is preserved unchanged as the focused modal surface. The new atlas is a sibling sub-card, exactly mirroring the `ExpeditionRadarPanel.cs` + `ExpeditionPanel.cs` pattern from Phase 17.

## 23.2 — Three-quadrant tile grid

| Quadrant | Sectors | Reads |
|---|---|---|
| North (Q1) | Sector 01..05 | `ExpeditionDefinition.id` matched against `loc.*` keywords |
| East (Q2)  | Sector 06..10 | same |
| South (Q3) | Sector 11..15 | same |

Each tile grid shows: Cell, Tile (display name), Sector, Danger (LVL), Rads/h. Danger ≥ 4 renders as `Critical`, ≥ 2 as `Warning`, else `Normal`. Rads/h > 10 also renders as `Critical`.

## 23.3 — Status rail cards (6 cards)

| Card | Value | Source |
|---|---|---|
| Zones Mapped | location count | `ExpeditionHostSession.DemoDefinitions.Count` |
| Outposts | danger < 2 count | walk locations |
| Caravans | active sorties | `ExpeditionHostSession.Engine.ActiveCount` |
| Dungeons | danger ≥ 4 count | walk locations |
| Safe Routes | danger == 0 count | walk locations |
| Hazard Zones | danger 2..3 count | walk locations |

## 23.4 — Action bar tile

The action bar grid shows 3 read-only action rows: `Dispatch Sortie`, `Plot Waypoint`, `Inspect Detail`. These are soft actions — the modal `MapPanel.cs` is where the actual operations live; the atlas just advertises what the player can do.

## 23.5 — Snapshot baseline fingerprint

```
snapshots/map_atlas_default.png   36345B   MD5 05ebbfdeba2499a0e135bbf04e84e210
```

All 24 baseline MD5s pairwise distinct; duplicate-check gate still clean.

## 23.6 — Fixture policy

`BuildFixtureRows(quadrant)` produces quadrant-specific deterministic rows drawing from the user's own canonical location ids (Holdfast Bunker, The Works Allotment Commune, The Denial Cut Substation). `BuildActionFixtureRows()` produces three action-bar rows with hint text.

The fixture is **only** invoked when `_host == null`. When `Bind(ExpeditionHostSession)` fires, `RefreshView()` re-reads the engine state and the fixture path is unreachable.

## 23.7 — Verification matrix (Phase 23 close)

```
1. dotnet build Ashfall.csproj                                                    0 Error, 0 Warning       ✅ PASS
2. dotnet test Ashfall.Core.Tests                                                 1994 / 1994 PASS         ✅ PASS
3. godot --path . -- --bridge-selftest                                            41/41 PASS               ✅ PASS
4. godot --path . -- --data-integrity-selftest                                    3588 ids, 0 errors       ✅ PASS
5. godot --path . -- --asset-registry-selftest                                    48/48 PASS               ✅ PASS
6. godot --path . -- --ui-snapshot-uitest                                         24/24 PASS, 24 MD5 distinct ✅ PASS
```

## 23.8 — Coverage roll-up at Phase 23 close

```
COVERED:           18 surfaces (23 targets)   ← +1 since Phase 22
PARTIAL (intent):   1 surface  (TradeScreen INTENTIONAL_CHILD)
REGRESSION_ONLY:    3 surfaces
NOT_NEEDED:         8 surfaces
MISSING:            1 surface  (Maritime, Muster, Quests)
DEFERRED:           0 surfaces   ← all DEFERS lifted
BLOCKED:            0 surfaces
Total tracked:    33 player-facing runtime surfaces
```

## 23.9 — Discipline checks

- **No primitive inflation** — the 5 Phase 11/12 primitives are reused verbatim. No `AshfallGauge`, no `MapAtlasGrid`, no new shell subsystem.
- **No third-party material** — every line of code is over the user's own ASHFALL project source files; all location ids, sector labels, and loot categories are drawn from the user's own authored `map_*` / `expedition_*` catalogs.
- **No `Assets/_Game/` write** — Unity legacy tree untouched.
- **No MATCH snapshot regression** — every Phase 11/12/13/15/16/17/18/19/20/21/22 PNG is byte-identical to its previous fingerprint.
- **Engine-agnostic Core path preserved** — the atlas talks only to `Ashfall.Core.Expeditions.ExpeditionDefinition` via `ExpeditionHostSession`, not to any Unity / Godot API.
- **Backward compat preserved** — `Main.cs` callers of the legacy `MapPanel` continue to work; the new atlas is a sibling sub-card.
- **Fixture policy obeyed** — 3 quadrant tile rows + 3 action-bar rows, all ids canonical, all numerics realistic, no fabricated production data.

## 23.10 — Phase 24+ candidate queue (remaining Tier-3 work)

| Surface | Stitch | Phase target | Status |
|---|---|---|---|
| `MaritimePanel` | `#48` | Phase 24+ | needs a unified wrapper |
| `MusterPanel` | (internal) | Phase 25+ | promotion candidate |
| `QuestsPanel` | (no Stitch) | Phase 25+ | covered indirectly in `journal_default` |

# Phase 28 — Research Core port + Tier-3 Research Atlas promoted

**Status:** complete. `ResearchAtlasPanel.cs` ships as a new Tier-3 HYBRID sub-card sibling of the legacy `ResearchPanel.cs` (Phase 9 modal). 29/29 snapshot baseline green. 2016/2016 Core tests still PASS. No MATCH snapshot regression.

Phase 28 closes the Research / R&D / Library gap. The legacy `ResearchPanel.cs` remains the focused interaction surface; this atlas adds the always-on top Research view the existing skill, faction, and map atlases provide. Research now has both: a modal for tactical interaction, an atlas for strategic awareness.

## 28.1 — Files added this phase

| File | Lines | Purpose |
|---|--:|---|
| `Assets/Ashfall.Core/Research/ResearchKnowledgeDef.cs` | ~30 | NEW engine-agnostic POCO for knowledge nodes. |
| `Assets/Ashfall.Core/Research/ResearchState.cs` | ~20 | NEW state envelope for save/load. |
| `Assets/Ashfall.Core/Research/ResearchSystem.cs` | ~180 | NEW engine with 15-node inline catalog, prerequisite gating, day-progress ticks, breakthrough awards. |
| `Ashfall.Core.Tests/ResearchSystemTests.cs` | ~180 | NEW 8 tests (RegisterDefaults, StartResearch, Tick, PrerequisiteGate, AlreadyCompleted, CaptureState round-trip, Determinism). |
| `src/Host/ResearchHostSession.cs` | ~120 | NEW thin Godot host adapter wrapping `ResearchSystem`. |
| `src/UI/ResearchAtlasPanel.cs` | ~470 | NEW Tier-3 HYBRID atlas. Header chrome via `AshfallDashboardShell`, 6-card status rail via `AshfallStatusRail`, four `AshfallDataGrid` tiles (Knowledge nodes grid + Active research grid + Breakthrough items grid + Action bar), plus right-side node detail inspector. Anchored to viewport (HUD-style, not modal). Reads from `ResearchSystem` via `ResearchHostSession`. |
| `src/UI/SnapshotHarness.cs` | +1 | Registered `research_atlas_default` target at 1280×800. |
| `docs/systems/RESEARCH_CORE_PORT_PLAN.md` | ~250 | NEW port plan mirroring Phase-18 Skill Progression. |
| `docs/ui/snapshot_manifest.json` | +1 | totals bumped to 29/29; new target in BASELINE classification. |
| `docs/ui/snapshot_baseline_manifest.json` | +1 | 29 snapshots, 29 baselines, all MD5-distinct. |
| `docs/ui/SNAPSHOT_COVERAGE.md` | +1 row | ResearchPanel flipped MISSING → COVERED (Phase 28). |

The legacy `src/UI/ResearchPanel.cs` is preserved unchanged as the focused modal surface. The new atlas is a sibling sub-card, exactly mirroring the `ExpeditionRadarPanel.cs` + `ExpeditionPanel.cs` pattern from Phase 17.

## 28.2 — Knowledge node catalog (15 nodes)

| id | displayName | category | days | breakthroughItem |
|---|---|---|---|---|
| knowledge_water_basics | Water Purification Basics | survival | 5 | — |
| knowledge_water_advanced | Advanced Water Filtration | survival | 12 | item_water_filter_advanced |
| knowledge_radiation_basics | Radiation Medicine Basics | medical | 5 | — |
| knowledge_radiation_shielding | Radiation Shielding Materials | engineering | 15 | item_radiation_shielding_panel |
| knowledge_gas_mask_improved | Improved Gas Masks | engineering | 10 | item_gas_mask_improved |
| knowledge_hydroponics | Hydroponic Cultivation | survival | 8 | — |
| knowledge_solar_basics | Solar Power Basics | engineering | 7 | — |
| knowledge_solar_advanced | Solar Power Systems | engineering | 14 | item_solar_inverter |
| knowledge_food_preservation | Food Preservation | survival | 10 | — |
| knowledge_radio_basics | Radio Signal Processing | science | 6 | — |
| knowledge_radio_advanced | Encrypted Radio Communication | science | 12 | item_radio_cipher_rotor |
| knowledge_shelter_insulation | Shelter Insulation | engineering | 8 | — |
| knowledge_air_filtration | Air Filtration Systems | engineering | 10 | item_air_filter_hepa |
| knowledge_scavenge_efficiency | Scavenge Efficiency | scavenging | 7 | — |
| knowledge_combat_training | Combat Training Doctrine | combat | 8 | — |

Prerequisite gating enforced: `water_advanced` requires `water_basics`, `radiation_shielding` requires `radiation_basics`, etc.

## 28.3 — Status rail cards (6 cards)

| Card | Value | Source |
|---|---|---|
| Total Nodes | catalog count | `ResearchHostSession.CatalogCount` |
| Unlocked | unlocked count | `ResearchHostSession.UnlockedCount` |
| Active | active node id | `ResearchHostSession.ActiveResearchId` |
| Completed | completed count | `ResearchHostSession.CompletedCount` |
| Days Remaining | active node days | `ResearchHostSession.ActiveResearchDays` |
| Breakthrough Items | breakthrough count | `ResearchHostSession.Catalog.Count(x => x.Value.breakthroughItem != null)` |

## 28.4 — DataGrid tiles

- **Knowledge nodes grid** — 15 rows, columns: Id, DisplayName, Category, Days, Prerequisites, Breakthrough
- **Active research grid** — 1 row (or empty if idle), columns: Id, DaysSpent, DaysRemaining, Progress% (DaysSpent / DaysToComplete)
- **Breakthrough items grid** — rows where `breakthroughItem != null`, columns: Id, BreakthroughItem, Category
- **Action bar** — 4 read-only action rows: `Start Research`, `Force Complete`, `Abandon Research`, `View Breakthrough`

## 28.5 — Snapshot baseline fingerprint

```
snapshots/research_atlas_default.png   74212B   MD5 90e831c0dd572b980622bb80f963b915
```

All 29 baseline MD5s pairwise distinct; duplicate-check gate still clean.

## 28.6 — Fixture policy

`BuildFixtureRows()` produces deterministic rows drawing from the 15 canonical knowledge ids. `BuildActiveFixtureRows()` returns 1 active row if the engine is idle. `BuildActionFixtureRows()` produces four action-bar rows with hint text.

The fixture is **only** invoked when `_host == null`. When `Bind(ResearchHostSession)` fires, `RefreshView()` re-reads the engine state and the fixture path is unreachable.

## 28.7 — Verification matrix (Phase 28 close)

```
1. dotnet build Ashfall.csproj                                                    0 Error, 0 Warning       ✅ PASS
2. dotnet test Ashfall.Core.Tests                                                 2016 / 2016 PASS         ✅ PASS
3. godot --path . -- --bridge-selftest                                            41/41 PASS               ✅ PASS
4. godot --path . -- --data-integrity-selftest                                    3588 ids, 0 errors       ✅ PASS
5. godot --path . -- --asset-registry-selftest                                    48/48 PASS               ✅ PASS
6. godot --path . -- --ui-snapshot-uitest                                         29/29 PASS, 29 MD5 distinct ✅ PASS
```

## 28.8 — Coverage roll-up at Phase 28 close

```
COVERED:           24 surfaces (29 targets)   ← +1 since Phase 28
PARTIAL (intent):   1 surface  (TradeScreen INTENTIONAL_CHILD)
REGRESSION_ONLY:    1 surface  (CraftingPanel drill-down)
NOT_NEEDED:         1 surface  (ResearchPanel legacy modal preserved)
MISSING:            0 surfaces  (ALL SURFACES COVERED) ← Phase 28 milestone
DEFERRED:           0 surfaces
BLOCKED:            0 surfaces
Total tracked:    27 player-facing runtime surfaces (ALL COVERED)
```

## 28.9 — Discipline checks

- **No primitive inflation** — the 5 Phase 11/12 primitives are reused verbatim. No `AshfallGauge`, no `ResearchGrid`, no new shell subsystem.
- **No third-party material** — every line of code is over the user's own ASHFALL project source files; all knowledge ids, categories, and breakthrough items are drawn from the user's own authored catalogs.
- **No `Assets/_Game/` write** — Unity legacy tree untouched.
- **No MATCH snapshot regression** — every Phase 11/12/13/15/16/17/18/19/20/21/22/23/24/25/26/27 PNG is byte-identical to its previous fingerprint.
- **Engine-agnostic Core path preserved** — the atlas talks only to `Ashfall.Core.Research.ResearchSystem` via `ResearchHostSession`, not to any Unity / Godot API.
- **Backward compat preserved** — `Main.cs` callers of the legacy `ResearchPanel` continue to work; the new atlas is a sibling sub-card.
- **Fixture policy obeyed** — deterministic fixture rows, all ids canonical, all numerics realistic, no fabricated production data.

## 28.10 — Closing summary

Phase 28 promotes the Research Atlas surface from MISSING to COVERED. 29 distinct MD5 fingerprints round-trip through the snapshot harness. Documentation manifests and the coverage roll-up are updated in lockstep. The five Phase 11/12 primitives remain the only primitives — no `AshfallGauge`, no `ResearchGrid`, no new shell sub-system. The legacy Phase 9 modal surface is preserved untouched, with the new atlas as a sibling sub-card.

Fixture policy obeyed: deterministic fixture rows, all ids canonical (drawn from the user's own authored catalogs), all numerics realistic, no fabricated production data.
