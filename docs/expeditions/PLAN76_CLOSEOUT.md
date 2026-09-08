# Plan 76 — Expedition Destinations — Closeout

**Status: COMPLETE (verification pass).** The expansion was already implemented and committed
(`1426438e` "plan76: expedition destination catalog validation, loot-table migration, and balance
sim" + follow-on `ef67d600`). This pass verified the full acceptance matrix against repository
truth. **Zero data or code changes required.**

## Summary

The plan's stated baseline of "2 destinations" was stale: repository truth at Plan 76 execution
time was already a **55-destination catalog** — the expansion target was superseded by repository
truth and then exceeded (55 > 15), exactly as the plan's own §1.1 ("repository truth overrides the
planning grammar") directs. The prior implementation completed the plan's three numbered
sub-efforts: 76.0 catalog validation + loot-reference repair, 76.1 Plan 46 scavenging-table
binding (49 tables, renewable/one-time depletion models), 76.2 deterministic balance simulation
(200 seeded runs × 53 destinations, two-pass determinism proof), 76.3 owner-approved balance trims.

## Baseline

- `expeditions.json` root: `{schema_version, expeditions[]}` — 55 destination records.
- Original planned pair `loc_the_allotments` (Works Allotment Commune) and
  `loc_denial_cut_substation` (Denial Cut Substation) present and intact.
- Baseline gates (this pass): suite 9461/9461, data-integrity 0 findings, build clean,
  expedition-selftest PASS (40 checks), content-utilization PASS.

## Schema (actual)

`{id, displayName, distanceTicks, dangerLevel, encounterChancePerTick, baseStaminaDrainPerHour,
scavenging_table_id, lootCategories[], requiresDiscovery?}` — Plan 46 Case B/C is live:
`scavenging_table_id` binds the authoritative weighted table while `lootCategories` remains as
item-ID flavor metadata. `requiresDiscovery` (2 destinations) is the §43/§44 availability seam.

## Location authority

54/55 destination IDs are canonical `locations.json` world-site IDs (the exception is a derived
sub-site with its own cross-catalog identity). No duplicate `loc_*` identity exists.

## Destination roster

55 destinations spanning near (2–5 ticks, 32), midrange (6–9, 15), and far (10+, 8) bands;
danger 2–10 spread across 9 levels; every planned family covered (urban, industrial, military,
scientific, wilderness, settlements-as-trade under Plan 43 social rules). Parity: all 53
destinations shared with the Plan 76 commit are byte-identical except two intentional later
additions (`requiresDiscovery` on rural gas station / government bunker, plus the Forestry
Compound record).

## Distance / Danger / Stamina

All values within runtime ranges; no monotonic ladder; stamina 1.2–4.0/hour tiered with
distance/danger bands. Highest cumulative encounter pressure: The Dead Hand Core (0.998),
Arcology Sector 4 (0.995), Silent Observatory (0.985) — deliberate endgame gatekeeping.

## Loot authority

- All 55 `scavenging_table_id` refs resolve against `scavenging_tables.json` (Plan 46 authority).
- `lootCategories` validated through `ExpeditionLootValidator` + the canonical multi-catalog
  `ItemCatalogLoader` (items.json **plus** expansion catalogs incl. `crossing_items.json`).
  Two Crossing-catalog refs (`item_crossing_traded_salt`, `item_hydro_baron_queue_chit`) are
  canonical items outside items.json — not orphans.
- Active regression gates: `Plan76DestinationLootReferenceTests` (5/5),
  `Plan76BalanceSimulationTests`, `Plan32ExpeditionDestinationWiringTests` (19/19 combined).

## Balance findings

- No strictly dominated destination on same-table axes except 11 pairs that are dominated
  **numerically only** — each retains a unique cross-catalog hook (cassette hidden caches,
  distress/quest sites, settlement trade, narrative locations), satisfying §52's
  "no unique content hook" escape clause. Numeric dominance is a documented balance observation,
  not a defect requiring data churn against a shipped, simulated catalog.

## Micro-locations / Weather

Plan 49/48 seams operate through `micro_locations.json` and the weather/travel systems; the 55
stable destination IDs are the binding surface. No destination-embedded bindings exist (per §36/§38).

## Determinism / Save

`--expedition-selftest` PASS (40 checks: dispatch, travel, looting, vehicle gates, 21
micro-location gates); balance sim is two-pass seeded-deterministic by construction; discovery
flag on old saves is additive-safe.

## Validation

| Command | Result |
|---|---|
| `godot --headless -- --expedition-selftest` | **PASS** — 40/40 |
| `godot --headless -- --data-integrity-selftest` | **PASS** — 0 findings, 298 catalogs |
| `godot --headless -- --content-utilization-selftest` | **PASS** — CI gate |
| `dotnet test --filter Plan76 + Plan32ExpeditionDestination` | **19/19 PASS** |
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **9461/9461 PASS** |
| `dotnet build Ashfall.csproj` | **PASS** — 0 errors |

## Deferred

None outstanding for Plan 76 scope. Downstream consumers (Plans 49/48/58/50/59) bind through the
55 stable destination IDs as designed.
