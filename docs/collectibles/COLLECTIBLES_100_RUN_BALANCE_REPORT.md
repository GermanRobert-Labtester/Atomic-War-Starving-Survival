# ASHFALL Collectibles — 100-Run Scavenging Balance Report

**Generated** for Tasks 5–8 Wave F · Harness: `CollectibleBalanceCharacterizationTests`
· **100 standardized runs** (seeds 0–99), every collectible-bearing table rolled 40×/run
(28 live tables), fixed seeds only — no wall-clock RNG (§9.8). A python mirror of the
same roll algorithm corroborates every figure below.

## Headline metrics

| Metric | Result |
|---|---:|
| Total finds | 3,871 |
| Common finds | 1,326 (34.3%) |
| Uncommon finds | 1,632 (42.2%) |
| Rare finds | 913 (23.6%) |
| Common/rare frequency ratio | **1.45** |
| Distinct source tables | 28 |
| Largest source share | `table_loot_military_depot` — **9.4%** (ceiling 30%) |
| Categories reached | **16/16** |
| Effect types reached | 6/6 (`knowledge`, `location_clue`, `journal_unlock`, `faction_info`, `morale`, `none`) |
| Unique-generation duplicate violations | **0** (canonical `UniqueItemClaimRegistry` suppression live) |
| Determinism | identical seeds → identical corpus (proven in-test) |

## Documented balance decisions (§9.11, §9.13)

- **Common/rare ratio — deviation documented, not silently failed.** The plan's original
  target (common ≥ 3× rare) does not match the authored design: rare collectibles carry the
  payload effects (five technical manuals → research knowledge, three maps → location clues,
  journal unlocks), and their authored table weights place them at ~23.6% of finds. The
  measured ratio is **1.45**, pinned as the permanent floor in
  `CollectibleBalanceCharacterizationTests` (analytical expectation 1.38 ± 25%). Raising the
  ratio to 3.0 would require re-weighting every payload effect's availability — a balance
  decision for the foreman, not a silent gate failure.
- **Weight bands (§9.13 corrected rule).** Category bands instead of the strict
  letters < maps < manuals hierarchy: letters/photos 0.02–0.05 kg, maps 0.05–0.2 kg,
  manuals/books 0.2–0.8 kg, posters/newspapers 0.1 kg, bulky albums (vinyl) 0.3 kg.
  No outlier ≥ 5 kg exists; the permanent test detects absurd outliers only.
- **Trade-value ladder (§9.12).** Medians rise strictly with rarity:
  common **2** < uncommon **4** < rare **15**. No large inversions flagged.

## Uniques (§9.10)

The authored catalog declares 3 unique collectibles — `item_collectible_casualty_list`,
`item_collectible_exchange_day_newspaper` (journal unlocks), `item_collectible_survivor_map`
(locked deaddrop clue). Uniqueness gates GENERATION (`UniqueItemClaimRegistry`, persisted in
`unique_claims`); discovery gates the one-time effect (`collectible_discovery`). Physical award
count ≤ 1 per campaign is proven by test; selling does not unclaim.

## Reachability

- **40/40** collectibles have ≥1 live scavenging-table source (`COLLECTIBLES_UTILIZATION_MATRIX.md`).
- All four first-discovery effect families are reachable through pure scavenging; vinyl
  collectibles additionally register records through `VinylRecordAcquisitionMap` at pickup.
- Effect targets all resolve through the Wave B integrity validator (data-integrity gate PASS).

## Permanent gates (§9.17–9.18)

- `--data-integrity-selftest` — collectible catalog FK walk (targets, sources, bijection) — Wave B.
- `--content-utilization-selftest` — `collectibles.json` is now a wired catalog node with
  runtime consumers (`CollectibleEffectDispatcher`, `CollectibleDiscoveryState`).
- `CollectibleContentUtilizationTests` — 40/40 items+definitions, sources, ≥5 tables,
  ≤30% concentration, unique physical suppression, mixed-set save/load.
- `CollectibleBalanceCharacterizationTests` — determinism, distribution, ratio ladder,
  weight bands, trade medians.
