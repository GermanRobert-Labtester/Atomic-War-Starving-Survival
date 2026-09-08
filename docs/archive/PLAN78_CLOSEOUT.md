# Plan 78 — Archive Inks Expansion — Closeout

**Status: COMPLETE (verification + one-entry balance repair).** The 12-ink catalog was already
implemented and committed (`7738facc` wave); this pass verified the full acceptance matrix against
the actual runtime formula and repaired the one genuine dominance defect.

## Summary

`archive_inks.json` verified at **12 inks** (3 original parity-frozen + 9 additions shipped by the
plan36–47–50 wave). One shipped addition (`ink_lampblack`) was strictly dominated by
`ink_soot_lamp` on the same ingredient at the same amount — rebalanced into a distinct niche.

## Baseline

Original 3 (`ink_iron_gall`, `ink_soot_lamp`, `ink_plant_dye`) — parity-frozen, byte-identical to
the 3-ink baseline commit (`bf591a99`). Catalog at execution time: 12 (target already met; per §1.1
repository truth wins — no additions required).

## Schema & runtime formula (the load-bearing finding)

- Fields: `ink_id`, `display_name`, `legibility_score`, `archival_longevity_days`,
  `fade_rate_per_day`, `required_item_id`, `required_amount`. Root: `{schema_version, collection_id, inks[]}`.
- **`legibility_score` is the only mechanically consumed quality** — it is stamped onto the
  transcription job at queue time (`ArchiveDeskSystem.QueueTranscription`).
- **`fade_rate_per_day` and `archival_longevity_days` are player-facing comparison metadata**
  (displayed in `ArchiveDeskPanel`); no document-decay system consumes them yet. The plan's
  §11–13 "do not infer" rule was applied: the hypothetical linear-fade curve analysis was used
  only as a coherence check, not as gameplay truth.
- Ingredient consumption is atomic (availability checked before roster state; refund on cancel —
  pinned by `Runtime_ArchiveDeskQueuesAndConsumesCorrectIngredientAmount`).
- Save contract: queue state stores ink ID + legibility snapshot by reference; catalog definitions
  are not serialized. Old saves resolve the original 3 IDs unchanged.

## Final roster (12)

| ink_id | leg | longevity d | fade/d | ingredient ×amount | niche |
|---|---:|---:|---:|---|---|
| ink_iron_gall | 0.90 | 500 | 0.0008 | charcoal ×2 | best all-round archival standard |
| ink_archival_carbon | 0.95 | 600 | 0.0010 | charcoal ×3 | highest legibility + longevity; premium cost |
| ink_chemical_marker | 0.80 | 400 | 0.0030 | chemical_solvent ×1 | high-contrast salvage path |
| ink_diluted_toner | 0.75 | 350 | 0.0030 | empty_toner_cartridge ×1 | office-salvage path |
| **ink_lampblack** | **0.72** | **120** | **0.0060** | charcoal ×1 | **brightest cost-1 ink; impermanent (repaired)** |
| ink_soot_lamp | 0.70 | 300 | 0.0015 | charcoal ×1 | durable budget charcoal path |
| ink_sepia | 0.70 | 280 | 0.0040 | organic_residue ×1 | organic-salvage path |
| ink_plant_dye | 0.60 | 200 | 0.0020 | cloth ×1 | cloth path (original) |
| ink_mineral_oxide | 0.60 | 220 | 0.0050 | scrap_metal ×2 | metal-salvage path |
| ink_improvised_pigment | 0.55 | 180 | 0.0060 | mineral_chunk ×2 | mineral path |
| ink_berry_juice | 0.50 | 150 | 0.0070 | berries ×2 | vegetation path |
| ink_blood_emergency | 0.40 | 100 | 0.0100 | blood_sample ×1 | intentional emergency floor |

## Dominance audit

- **Repaired:** `ink_lampblack` (0.65/250d/0.004) was strictly dominated by `ink_soot_lamp`
  (0.70/300d/0.0015) on the same `charcoal ×1` — same-cost dominance per §15/§61. Rebalanced to
  0.72/120d/0.006: brightest amount-1 ink but fastest-fading standard-tier option — a real
  legibility-now vs durability trade. Fade/longevity coherence holds (0.006/day reaches zero ≈ day
  120 = declared longevity).
- Remaining 8 dominated pairs are all **cross-ingredient path niches**: each ink is the best
  available option *conditional on possessing its ingredient* (charcoal may be exhausted while
  cloth/berries/mineral salvage is not). Zero same-ingredient dominance remains (script-verified).
- No identical profiles; no universal best (archival carbon pays 3× charcoal for its quality).

## Ingredients

All 9 distinct required items resolve (`charcoal`, `cloth`, `berries`, `chemical_solvent`,
`empty_toner_cartridge`, `mineral_chunk`, `blood_sample`, `organic_residue`, `scrap_metal`) —
zero new items needed (§48/§49 satisfied; item-creation gate unused). Amounts 1–3 within the
test-pinned 1–5 range. Acquisition: charcoal (crafting/scavenge), cloth/berries/scrap_metal
(scavenging tables/trapping/apiary), chemical_solvent (chemical plant/printworks tables),
blood_sample (medical), mineral_chunk/organic_residue/empty_toner_cartridge (salvage paths).

## Validation

| Command | Result |
|---|---|
| `dotnet test --filter ArchiveInks + ArchiveDesk` | **23/23 PASS** (catalog shape, parity, ranges, ingredient refs, no-dup, no-dominance, runtime consumption/refund, save round-trip) |
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **9461/9461 PASS** |
| `godot --headless -- --data-integrity-selftest` | **PASS** — 0 findings, 298 catalogs |
| `godot --headless -- --content-utilization-selftest` | **PASS** |
| `godot --headless -- --real-campaign-journey-selftest` | **PASS** |
| `dotnet build Ashfall.csproj` | **PASS** — 0 errors |

No dedicated `--archive-selftest` verb exists; none was invented (§73).

## Deferred

- Mechanical fade/longevity consumption (document-decay system) — the fields are coherent authored
  metadata awaiting that owner; Plan 51/17 territory.
- Plan 55 multi-ingredient ink recipes; Plan 47 collectible classification for sealed stock;
  Plan 46 pigment-table placement (current acquisition paths suffice).
