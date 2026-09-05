# Plan 85 — Completion Report

## Summary

- **baseline damaged-map zone count:** 6 (repository truth; plan assumed 3 — delta rule §1.12 applied)
- **final zone count:** **12** (+6 new; all 6 original zones preserved)
- **new zones added:** `old_medical_quarter`, `court_district`, `pasture_valley`, `north_woods`, `university_quarter`, `metro_service_ring`
- **total fragment count:** 32 (15 existing + 17 new); all uniquely identified, all produced
- **hidden installations added/reconciled:** 12 total — 11 new wasteland-map nodes + routes, 12 new expedition destinations (incl. `loc_hidden_relay_bunker`, which had a node but no destination)
- **new item definitions added:** **0** (two broken pre-existing reward refs repaired: `generator_parts`→`mechanical_parts`, `heirloom_seeds`→`family_heirloom_seeds`)

## Runtime semantics discovered

- **fragment lifecycle:** Model B — discovery tokens registered into `WastelandMapState.RegisteredMapFragments` (permanent knowledge; not items; duplicates no-op)
- **completion rule:** derived — every catalog fragment of a zone registered; edge-triggered, idempotent
- **`revealed_items` meaning:** before Plan 85: unconsumed data. Now: installation destination `lootCategories` (guaranteed-eligible signature salvage via the seeded expedition loot loop; never a direct grant)
- **reveal authority:** `WastelandMapSystem.Discover` + `Unlock` (pre-existing fog-of-war/lock authority), invoked by `DamagedMapSystem` on the completion edge
- **destination integration model:** pre-authored `expeditions.json` destinations, gated in Core (`ExpeditionSystem.Start` → `IsDestinationLocked`) and in UI (`GetBlockReason` → "Map incomplete — location unidentified"); no runtime catalog mutation
- **loot-resolution boundary:** standard expedition Looting-phase rolls (`PerformLootRoll`, seeded `ISeededRng`); no unique one-time caches authored → no duplication surface (see `INSTALLATION_LOOT_PROVENANCE.md`)

## Existing-content preservation

- **original zone ids preserved:** all 6 (incl. unprefixed installation ids; `ResolveRevealNodeId` maps them onto the `loc_*` node namespace without renames)
- **old-save fixture result:** PASS (`OldSave_OriginalZoneProgress_LoadsAndPreserves_UnderExpandedCatalog`)
- **Plan 76 content reconciled:** 5 candidate concepts dropped/redirected to avoid duplicating `loc_municipal_archive`, `loc_seed_library_annex`, `loc_pump_station_nine`, the comms/relay cluster, and the pre-war medical cache (see `PLAN76_PLAN85_DESTINATION_RECONCILIATION.md`)
- **Plan 46 content reconciled:** fragments wired into 23 existing tables with location-type affinity; tables became live in the host (previously test-only)
- **Plan 47:** no map collectibles exist in `collectibles.json` — no reconciliation required, no semantics doc needed

## New zone matrix

| Zone | Frag | Area | Producers | Installation | Reward role | Consumer | Revisit behavior |
|---|---|---|---|---|---|---|---|
| old_medical_quarter | 3 | urban-medical | hospital, clinic, fire_station | Sealed Triage Annex | medical consumables (bounded) | dest → table_loot_hospital | standard loot loop |
| court_district | 3 | civic-records | police_station, archive, printworks | Evidence Sub-Basement | documents/records | dest → table_loot_police_station | standard loot loop |
| pasture_valley | 2 | agro-veterinary | veterinary_surgery, farm | Quarantine Barn | seeds + hand tools | dest → table_loot_veterinary_surgery | standard loot loop |
| north_woods | 3 | forestry | forestry_compound, hunting_cabin | Forestry Emergency Store | field kit/fuel | dest → table_loot_forestry_compound | standard loot loop |
| university_quarter | 3 | academic | school, observatory | Materials Research Sublevel | precision/research | dest → table_loot_observatory | standard loot loop |
| metro_service_ring | 3 | underground-transit | metro_station, power_substation | Electrical Maintenance Exchange | electrical | dest → table_loot_power_substation | standard loot loop |

## Acquisition

- **fragments with scavenging producers:** 32/32 (zero dead fragments; test-pinned)
- **fragments with alternate producers:** 0 quest/trader dependencies (no missable-fragment hard-locks)
- **unreachable/deferred fragments:** **zero**

## Expedition/location integration

- **Plan 76 destinations wired:** 12 of 12 installations (minimum was 3)
- **other world-location reveals:** all 12 installations also appear on the wasteland map (Locked → Discovered on reveal)
- **duplicate concepts avoided:** 5 (documented in reconciliation doc)

## Rewards

- **fixed caches:** none authored (would require a new claimed-state loot system — non-goal)
- **procedural caches:** all 12, via the existing seeded expedition loop with themed Plan 46 tables
- **unique rewards:** none (all rewards are pre-existing multi-producer items)
- **newly added items:** 0
- **economy outliers and mitigations:** no new ammo/medicine/fuel jackpots; medical set spread across 5 common consumables; military zones untouched (pre-existing)

## Persistence/determinism

- **partial-progress save tests:** PASS (old-save fixture + round-trip)
- **completion/reveal save tests:** PASS (capture/restore; no re-fire)
- **loot-state save tests:** existing expedition save flow unchanged (checksummed); site loot stateless by design
- **reload-reroll tests:** n/a by construction (stateless site loot; seeded streams)
- **old-save migration result:** clean (added field with default; unknown fragment ids inert)

## Verification

```text
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj : PASS (0 errors)
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj  : PASS (6866/6866)
dotnet build Ashfall.csproj                               : PASS (0 errors, 0 warnings)
--data-integrity-selftest                                 : PASS (0 findings, 208 catalogs)
--bridge-selftest                                         : PASS
--content-utilization-selftest                            : PASS (CI gate green)
--cartography-selftest                                    : 2 pre-existing failures only (≥60 nodes/≥200 routes thresholds; 20/44 now vs 9/22 baseline) — predates Plan 85, recorded, not normalized
```

## Deviations from the source plan (repository-truth driven)

1. **Baseline was 6 zones, not 3** → added 6 (delta rule), not 9.
2. **Core code was required beyond pure data** — the catalog had no loader, system, producers, completion, or reveal at all (§7.1's exact "missing live seam" case). Added: `DamagedMapCatalog`, `DamagedMapSystem`, `WastelandMapState.RegisteredMapFragments`, `map_fragment_id` scavenging channel, `ExpeditionSystem` hook/gate. Justified per seam in the authority map.
3. **All 12 installations wired to expedition destinations** (plan minimum 3) — uniform, data-driven, zero marginal cost per zone.
4. **No unique one-time caches** — building persistent site-loot claim state would be a new loot system (forbidden §14); rewards use existing multi-producer items, making duplication impossible by construction.
5. **Reveal visibility:** hidden installations use the plan-sanctioned locked-node pattern (visible as locked markers, §85D.3); full marker hiding would require changing `ResolveNodeStatus`/view semantics for existing locked nodes — deferred.
6. **Zone-concept substitutions:** Municipal Archive Vault, Cooperative Root Reserve, Upland Weather Array, Bonded Marine Warehouse, Hardened Signal Annex were replaced after Plan 76/23 concept reconciliation; substitutions preserve area-type and reward diversity.
7. **Two pre-existing data bugs fixed** in original zones (`generator_parts`, `heirloom_seeds` did not resolve).
8. **Host wiring side-effect:** Plan 46 scavenging tables are now bound to the live expedition engine (were test-only); destinations without a table keep the legacy fallback.

## Deferred follow-ons

- Cartography selftest aspirational thresholds (≥60 nodes / ≥200 routes) — catalog grew 9→20/22→44; reaching the thresholds needs a broader world-map content pass outside Plan 85.
- Persistent per-site depletion/claimed-cache state (would enable true unique caches) — new loot-state authority, explicitly out of scope.
- Dedicated damaged-map UI panel (zone progress list, fragment log) — plan §85C.8 forbids new UI subsystems in this plan; dispatch-block reason + map markers ship instead.
- `depletion_model` fields on scavenging tables remain data-only (pre-existing gap, recorded).
- Multi-zone regional mysteries, cartographer NPC, and the other §18 follow-ons remain deferred by design.
