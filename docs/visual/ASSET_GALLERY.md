# ASHFALL — Visual Asset Gallery

**Date:** this turn (Phase 14).
**Authority source:** [`docs/visual/visual_asset_manifest.json`](visual_asset_manifest.json) (regenerated via `scripts/audit_assets.py`).
**Authoritative coverage classification:** [`docs/visual/VISUAL_ASSET_AUDIT.md`](VISUAL_ASSET_AUDIT.md).

This gallery is a human-browsable index of the active `assets/` tree grouped by the asset families the runtime actually references. It exists to give future agent runs a single jumping-off page when they need to (a) find the canonical asset for an id, (b) identify orphans, or (c) rerun the audit script.

## Gallery roll-up

```
Active assets/ tree (Godot-native)           2,335 files
Legacy Assets/ tree (under Unity keepalive)  3,512 files
Sum                                          5,847 image assets
Catalogued reference ids (from catalogs)     1,114
Missing in tree (post-Wiring-Matrix)           517
Resolved-via-alias                                5
Exact-dup groups (MD5-identical)                182
Perceptual-dup groups (aHash)                    82
Orphan assets (no catalog reference)          1,687
Fallback (asset resolves but degrades visual)   522
```

## How to read the gallery

The gallery is **Dictionary-shaped** but **probe-shaped**: each category lists the canonical IDs the runtime expects, then shows the on-disk row that the master resolution chain produces. Workers can diff the two lists to spot missing assets.

```text
Category: Item
├── id                       exists?  resolved-via                     MD5
├── iodine_pills             ✔        art/iodine_pills.jpg             a81c..
├── geiger_counter           ✔        art/geiger_counter.jpg           3ebe..
├── mercury_vials_main       ❌        (procedural MakeItemIcon)        → queue  ┐
├── sterling_pen             ❌        (procedural MakeItemIcon)        → queue  │ pipeline
├── encrypted_drive          ✔        art/item_encrypted_drive.jpg     0caf..   │ work
└── cigarette_pack_sealed    ✔        art/cigarette_pack_sealed.jpg    81d9..   ┘
```

## Per-category snapshots

The active tree splits into nine families. Counts taken from the Phase 14 audit run (12:24, before this turn).

| Category | Path root | Files | All-active-tree share | Catalog coverage |
|---|---|---:|---:|---|
| Item | `assets/art/`, `assets/sprites/Items/` | 1,757 | 75.3% | 78.5% of catalog item IDs resolve |
| Item/Ammo | `assets/sprites/Items/` (subtree) | 173 | 7.4% | 41.0% (lowest of all families) |
| Item/Medical | `assets/sprites/Items/medical/` | 49 | 2.1% | 79.5% |
| Character/Portrait | `assets/sprites/Portraits/` | 100 | 4.3% | 65.0% (recent addition) |
| Location | `assets/sprites/Locations/` | 47 | 2.0% | 51.1% |
| Faction | `assets/ui/FactionEmblems/`, `assets/sprites/Factions/` | 118 | 5.1% | 18.2% (most missing) |
| Environment/Weather | `assets/sprites/Weather/` | 15 | 0.6% | 26.7% |
| UI/Chrome | `assets/ui/Screens/` | 62 | 2.7% | n/a (Stitch source-of-truth) |
| UI/Icon | `assets/ui/Icons/`, `assets/ui/Textures/` | 219 | 9.4% | (UI chrome, no catalog test) |

`Active-tree share` is the percent of all 2,335 active images. Each row plus its prefix-additions sum to ~100%.

## Manifest + queue maintenance

Asset discrepancies fall into three registry buckets, each with a file of record:

| Bucket | File of record | Purpose |
|---|---|---|
| Wiring matrix | `docs/visual/WIRING_MATRIX.md` + `.json` | Survey of content→asset mapping for every catalog id (1114 rows). |
| Replacement queue | `docs/visual/ASSET_REPLACEMENT_QUEUE.md` | Hand-fixable missing-asset list — pulls from the wiring matrix and orders by visual surface importance. |
| Fallback documentation | `docs/visual/FALLBACK_VISUAL_ASSETS.md` | Explains when the runtime falls back to placeholder texture and which IDs hit that path. |
| Duplicates | `docs/visual/DUPLICATE_VISUAL_ASSETS.md` | Tracks exact-dup MD5 groups and perceptual aHash groups so cleanup is convergent. |
| Orphans | `docs/visual/ORPHAN_VISUAL_ASSETS.md` | Assets in the active tree with no catalog reference — candidates for archive. |

## How to extend the gallery

1. **Add a new content ID**: `data_dir/items.json` (+ every expansion that loads it). Re-run `scripts/audit_assets.py`; the new ID will appear in the wiring matrix and (if missing) in the replacement queue.
2. **Add a new asset file**: drop the file under `assets/<category>/`. Re-run the script; the wiring matrix is regenerated.
3. **Add a new category**: edit `scripts/audit_assets.py` `categorize()` function. Re-run. Updates the manifest + the gallery roll-up in this doc.
4. **Resolve an alias**: edit `AssetRegistry.ItemIdAliases` in `src/Host/AssetRegistry.cs`. Re-run `--asset-registry-selftest` and `scripts/audit_assets.py`. Wiring matrix updates the resolved-via count.
5. **Hand-fill an asset**: drop the file under the canonical path; re-run wiring script; the existing queue entry should disappear from the marching column.

## Procedure for replacement work

```
1. Open docs/visual/ASSET_REPLACEMENT_QUEUE.md.
2. Pick the highest-impact missing ID for the surface you are working on.
3. Add the asset under the asset-registry canonical path.
4. Re-run --asset-registry-selftest to confirm resolution.
5. Re-run scripts/audit_assets.py and update docs/visual/visual_asset_manifest.json.
6. Run the relevant snapshot target to ensure no visual regression.
7. Refresh docs/visual/_trace_phase13_baseline.json if rank-order shifts.
```

## Snapshot Gallery (Phase 28 baseline)

Each snapshot target is a deterministic 1280×800 PNG rendered by the Godot snapshot harness. The MD5 is the byte-level fingerprint of the PNG file itself (not the raw texture).

| Target | Size | MD5 | Phase | Notes |
|---|---|---|---|---|
| medical_default | 31,052B | `3105c142a18ff31bb321bb7a6396fcbb` | 11 | Medical panel baseline |
| shelter_default | 29,645B | `c5ec30542bf17869a582884150bad904` | 11 | Shelter panel baseline |
| journal_default | 38,451B | `5cc5d3ec3491153275924ce790a659a9` | 11 | Journal panel baseline |
| inventory_default | 32,114B | `927414663f2622c17a2bf0f6d03e8348` | 11 | Inventory panel baseline |
| survivors_default | 35,881B | `53e85d2f913c5ed5e6a0f150957018b8` | 11 | Survivors panel baseline |
| radio_default | 31,440B | `ca0a9c4b22498927b026213de43c9b61` | 11 | Radio panel baseline |
| weather_default | 29,665B | `b3cc997e91c6ab8ebef350597c175779` | 11 | Weather panel baseline |
| verdict_default | 38,777B | `12fe77f9a6f26fd2db4fdc9c196a42d9` | 11 | Verdict panel baseline |
| trade_default | 41,221B | `6d258bce2f2537fac179bbc33fb255e2` | 12 | Trade panel baseline (INTENTIONAL_CHILD) |
| survival_workstation_default | 72,472B | `1d996b69b6d069ef3cc2259aaf06add6` | 12 | Survival Workstation baseline |
| caravan_barter_default | 45,172B | `d51acfb40126fd25ffb1825e6a78da4c` | 12 | Caravan Barter Ledger baseline |
| shelter_hud_default | 39,071B | `90a79f391088ad542ca82b73803f667a` | 13 | Shelter HUD baseline |
| faction_matrix_default | 47,339B | `b7187f5078f0c2e86298b3b0cfc3d894` | 13 | Faction Matrix baseline |
| dose_ledger_default | 48,912B | `a126bba9cd1b263b38873cd8654ff393` | 13 | Dose Ledger baseline |
| verdict_dashboard_default | 53,455B | `d3a540ff8f505ce8a823b736dc1f3897` | 13 | Verdict Dashboard baseline |
| weather_dashboard_default | 36,114B | `f31ebdd415188fda9aa3d332c74e93f6` | 13 | Weather Dashboard baseline |
| greenhouse_default | 43,205B | `a3762014aff52e9afdfdb776409d8b34` | 15 | Greenhouse panel baseline (Tier-A4) |
| silent_foundry_default | 58,173B | `af4db26d58e025d823ba5dcee9c97763` | 16 | Silent Foundry panel baseline (Tier-A1) |
| expedition_radar_default | 58,058B | `3cc6fd463b5ceae8d5fbc873445f7f65` | 17 | Expedition Radar panel baseline (Tier-A5) |
| skill_matrix_default | 98,739B | `76057f2be71cdf3640169982f3f90907` | 19 | Skill Matrix panel baseline |
| duty_roster_default | 57,279B | `df4ac41f22fa9f010acdcf2f92fc8220` | 20 | Duty Roster panel baseline |
| factions_narrative_default | 61,160B | `6b828ede820e9af7a7f97a12214ad1db` | 21 | Factions Narrative panel baseline |
| combat_hud_default | 44,965B | `330e7beadbc18e4821f6ad21fe3a4926` | 22 | Combat HUD Overlay baseline (HUD-style) |
| map_atlas_default | 36,345B | `05ebbfdeba2499a0e135bbf04e84e210` | 23 | Map Atlas panel baseline (Tier-3) |
| maritime_atlas_default | 49,172B | `1402d5a3dd7b23cc56aaef562bce4872` | 24 | Maritime Atlas panel baseline (Tier-3) |
| muster_atlas_default | 76,235B | `54603018036464dbf6eea757113b9532` | 25 | Muster Atlas panel baseline (Tier-3) |
| quests_atlas_default | 51,845B | `ee47c0d978fc26b13e28d6428601117a` | 26 | Quests Atlas panel baseline (Tier-3) |
| standing_record_atlas_default | 77,717B | `96da620bfcad4289011eb6905a31e4f9` | 27 | Standing Record Atlas panel baseline (Tier-3) |
| research_atlas_default | 74,212B | `90e831c0dd572b980622bb80f963b915` | 28 | Research Atlas panel baseline (Tier-3) |

The Stitch inventory file lives at `/home/robertsrff/stitch_export_17640704459929707404/screen_manifest.json` — but that file is not under git. The mapping table lives in [`../PHASE13_DATA_AVAILABILITY.md`](../ui/PHASE13_DATA_AVAILABILITY.md) row "Stitch ref id" + per-target `stitch_reference_id` in [`snapshot_manifest.json`](../ui/snapshot_manifest.json).

## What is not in the gallery

- **3D models**, sound, animations — none of those Phase 4/Phase 5 mod assets are present.
- **Procedurally generated icons** — Badge icons via `MakeBadgeIcon` are runtime-generated, not file assets.
- **Programmatic monospaced numbers** — many Phase 13 panel body cells draw typography-only ("— no profile bound —", "0 mSv", etc.) so the gallery catalog can't survey them.
- **TileMap / Atlas terrains** — not present in this game's repository; atlas mappings live in `MapPanel` references, not files.

## Phase 14 baseline (date stamp)

This gallery is committed alongside `visual_asset_manifest.json` and `VISUAL_ASSET_AUDIT.md` at this turn. Any regeneration of `scripts/audit_assets.py` is treated as Phase 14+ evidence: do not regenerate without updating both documents together. They stand together as the canonical Phase 14 visual-asset picture.
