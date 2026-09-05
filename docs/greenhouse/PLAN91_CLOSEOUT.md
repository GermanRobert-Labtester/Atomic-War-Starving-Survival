# PLAN91 CLOSEOUT — Greenhouse Items Expansion

## Summary

`greenhouse_items.json` expanded to a **30-live-entry** greenhouse material
ecosystem: the 14 original live entries preserved byte-identical, 16 inert
stale parity copies removed (zero runtime change — their improved
definitions live in `items.json` and were already the global authority), and
16 new supply items added (tools ×4, soil amendments ×3, pest control ×3,
water management ×3, structural repair ×3). 4 crafting recipes and 3
scavenging-table bindings wire the additions into existing systems. **Zero
gameplay code was added** (pure data + tests + docs).

## Baseline (corrected)

Plan 91's audit said "14 entries". Reality: 30 on disk / 14 live — plans
36/47/50 had added 8 seed/crop pairs, later mirrored *with improvements*
into `items.json` (commit `8bb494b4`), leaving the greenhouse copies inert
(first-loaded-wins merge, `ItemCatalogLoader.SecondaryItemFiles`). Details:
`PLAN91_BASELINE.md`, `GREENHOUSE_ITEM_CATALOG_AUTHORITY.md`.

## Catalog authority

**Model A — merged global registry.** `items.json` loads first, then nine
secondary item files into one `ItemCatalog`; duplicate IDs silently skip.
Exact schema and the 19-value `ItemType` enum documented in
`GREENHOUSE_ITEM_CATALOG_AUTHORITY.md`. No loader/registry edits were
required (data-driven; plan §66: `NEW SYSTEM JUSTIFICATION: NOT REQUIRED`).

## Final roster (30 live)

Seeds/crops/production (14, preserved): `item_seed_mushroom`,
`item_seed_tuber`, `item_seed_grain`, `item_seed_wheat`, `item_planter_box`,
`item_grow_lamp`, `item_lead_glass_pane`, `item_blight_treatment`,
`item_grow_medium`, `crop_mushroom`, `crop_tuber`, `crop_grain`,
`crop_wheat`, `tainted_food`.

Supplies (16, new): `item_greenhouse_trowel` (Tool),
`item_greenhouse_pruning_shears` (Tool), `item_greenhouse_watering_can`
(Tool), `item_greenhouse_hand_cultivator` (Tool),
`item_greenhouse_compost` (Material), `item_greenhouse_ash_fertilizer`
(Material), `item_greenhouse_fish_emulsion` (Material),
`item_greenhouse_insecticidal_soap` (Material),
`item_greenhouse_sticky_traps` (Material), `item_greenhouse_pest_mesh`
(Material), `item_greenhouse_drip_kit` (Material),
`item_greenhouse_line_filter` (Filter),
`item_greenhouse_catchment_kit` (Material),
`item_greenhouse_glass_pane` (Material), `item_greenhouse_uv_sheeting`
(Material), `item_greenhouse_shade_cloth` (Material).

## Gap analysis

The 14 live entries covered seeds, clean/tainted yields, and three pieces of
production equipment — but **no hand tools, no soil amendments beyond
sterile substrate, no water management, no structural repair beyond the
leaded pane, and a single pest treatment**. The plan's proposed roster was
adjusted per audit (plan §32/33): "irradiated compost" → Screened Compost
(§55 contamination rule; Rot Farmers' Compost Yard is canon),
copper tape → Sticky Trap Sheets, neem oil → Insecticidal Mesh (regional
plausibility, §54), full rainwater collector → inventory-scale Catchment
Kit (§28), and pest control uses three distinct action modes
(wash/capture/barrier) instead of three sprays (§34).

## Global ID audit

All 16 new IDs globally unique across the 747-ID namespace; no semantic
duplicates (shears vs medical scissors, mesh vs faraday mesh, drip-line
filter vs air/RO/potable filters, plain pane vs leaded pane — all distinct).
Full table: `GREENHOUSE_ITEM_GLOBAL_ID_AUDIT.md`.

## Physical balance

Stacks: hand tools 1, kits 2, bulk supplies 8–15. Weights 0.15–2.6 kg
(pane heaviest, justified above the draft 2.0 cap per plan §29 — baseline
lead pane is 5.0). Trade spread 2–18, no uniform band; drip kit (18) and
catchment kit (14) lead as high-utility infrastructure; ash (2) and compost
(3) anchor the cheap-consumables floor. Matrix: `GREENHOUSE_ITEM_ROLE_MATRIX.md`.

## Acquisition

Every addition has ≥1 path; 4 craftable, 3 scavengeable, all tradeable.
No unreachable content. Matrix: `GREENHOUSE_ITEM_ACQUISITION_MATRIX.md`.

## Crafting (4 bindings)

`craft_greenhouse_trowel`, `craft_greenhouse_watering_can`,
`craft_greenhouse_drip_kit`, `craft_greenhouse_catchment_kit` — workbench,
proven staple ingredients, no arbitrage (output ≥ 75% of ingredient trade
cost + labor). `GREENHOUSE_CRAFTING_BINDINGS.md`.

## Scavenging (3 bindings)

`table_loot_greenhouse` += glass pane (uncommon), UV sheeting (uncommon),
drip kit (rare) — weighted-schema entries only; Plan 76 destinations route
through this authority. `GREENHOUSE_SCAVENGING_BINDINGS.md`.

## Live consumer status

LIVE_CONSUMED today: the 12 seeds (Plant) + `item_blight_treatment`
(TreatBlight). All 16 new supplies are inventory/crafting/scavenging/trade
content with explicit `FUTURE_GREENHOUSE_HOOK` documentation — **no
description claims a greenhouse effect no system delivers** (enforced by
`GreenhouseFile_NewSuppliesClaimNoConsumableEffectFields`). Plans 22/71
remain the authorities for production/power effects; neither was touched.

## Save / inventory

No save code touched. Static definitions stay load-time data (plan §64);
inventory instances of new IDs serialize through the existing item-id path
(Model A registry resolution), pinned by the registry tests.

## Validation (exact)

| Command | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | PASS — 0 errors, 0 warnings |
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **PASS — 7003/7003** (incl. 21 new Plan 91 tests) |
| `dotnet build Ashfall.csproj` | PASS — 0 errors, 0 warnings |
| `godot --headless --path . -- --data-integrity-selftest` | **PASS — 0 errors, 208 catalogs** |
| `godot --headless --path . -- --greenhouse-selftest` | PASS — 24/24 |
| `godot --headless --path . -- --content-utilization-selftest` | CI gate PASS |
| `godot --headless --path . -- --asset-registry-selftest` | PASS |

(Phase 0 baseline had 3 pre-existing muster-ending test failures and a
13-finding integrity FAIL; both were fixed by a concurrent agent's muster
work during execution — not claimed by Plan 91. See
`PLAN91_REGRESSION_MATRIX.md`.)

## Deferred

- Plan 22 greenhouse production consuming fertilizer/pest/repair items
  (requires greenhouse runtime mechanics — do not fake in data).
- Plan 71 room-power interactions (pumps/lamps/irrigation controls).
- Plan 55 deeper crafting chains (drip tubing, repair kits) on stable IDs.
- Seasonal crop expansion (§78.6) — existing seed coverage audited as
  sufficient (12 cultivars across staple/greens/root/legume/herb/oil/algae).
- Dedicated pest/fertility systems (§78.7) — item presence alone does not
  justify them.

## Definition-of-Done note

DoD item "final total of exactly 30" is met as **30 live entries**; the
on-disk 30 → 30 reconciliation (16 dead copies out, 16 supplies in) is the
documented resolution of the plan's stale 14-entry audit.
