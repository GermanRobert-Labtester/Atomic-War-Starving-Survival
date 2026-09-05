# Installation Reveal Matrix (Plan 85)

## Reveal chain (§85D.1) — fully wired, no magical handoff

```
final fragment registered (PerformLootRoll → RegisterFragment)
  → DamagedMapSystem.IsZoneComplete (derived, set-based)
  → edge-triggered OnZoneCompleted (once per completing registration; idempotent)
  → WastelandMapSystem.Discover(loc_<installation>) + Unlock(loc_<installation>)
      [authoritative fog-of-war + lock authority; both idempotent]
  → OnInstallationRevealed
  → world-map marker: Locked → Discovered (WastelandMapView, unchanged)
  → expedition destination (expeditions.json) passes ExpeditionSystem.Start gate
     (DamagedMap.IsDestinationLocked == false)
  → dispatch through the normal expedition loop (travel → Looting → loot rolls)
```

Before reveal, `ExpeditionHostSession.GetBlockReason` reports **"Map incomplete — location unidentified"** for installation destinations; `Start` refuses them in Core (headless-safe).

## Per-zone integration

| Zone | Installation id | Map node | Node state pre-reveal | Destination | Loot owner |
|---|---|---|---|---|---|
| industrial_district | `underground_fuel_depot` | `loc_underground_fuel_depot` (new) | locked, undiscoverable | `loc_underground_fuel_depot` (new) | table_loot_tank_farm |
| suburban_heights | `municipal_seed_vault` | `loc_municipal_seed_vault` (new) | locked, undiscoverable | `loc_municipal_seed_vault` (new) | table_loot_farm |
| military_corridor | `blacksite_armory_7` | `loc_blacksite_armory_7` (new) | locked, undiscoverable | `loc_blacksite_armory_7` (new) | table_loot_military_depot |
| crater_ground_zero | `loc_excavation_command_vault` | existing id, node new | locked, undiscoverable | `loc_excavation_command_vault` (new) | table_loot_dead_hand_core |
| deep_coast_shelf | `loc_deaddrop_command_shelter` | existing id, node new | locked, undiscoverable | `loc_deaddrop_command_shelter` (new) | table_loot_relay_mast |
| high_scarp_ridgeline | `loc_hidden_relay_bunker` | **pre-existing node** | danger "medium" (pre-existing; NOT changed — see note) | `loc_hidden_relay_bunker` (new) | table_loot_relay_mast |
| old_medical_quarter | `loc_sealed_triage_annex` | new | locked, undiscoverable | new | table_loot_hospital |
| court_district | `loc_evidence_sub_basement` | new | locked, undiscoverable | new | table_loot_police_station |
| pasture_valley | `loc_quarantine_barn` | new | locked, undiscoverable | new | table_loot_veterinary_surgery |
| north_woods | `loc_forestry_emergency_store` | new | locked, undiscoverable | new | table_loot_forestry_compound |
| university_quarter | `loc_materials_research_sublevel` | new | locked, undiscoverable | new | table_loot_observatory |
| metro_service_ring | `loc_electrical_maintenance_exchange` | new | locked, undiscoverable | new | table_loot_power_substation |

**All 12 zones** are expedition-reachable after reveal (plan required ≥3). Destinations are pre-authored in the data catalog — no runtime catalog mutation (§85D.3).

## Namespace mapping (§1.5, §7.4)

`DamagedMapSystem.ResolveRevealNodeId`: exact node-id match first, otherwise `loc_` + installation id. The three unprefixed original ids (`underground_fuel_depot`, `municipal_seed_vault`, `blacksite_armory_7`) keep their stable zone-catalog ids and map onto `loc_*` map nodes; nothing was renamed.

## Pre-existing exception (recorded, not normalized)

`loc_hidden_relay_bunker` shipped as a discoverable, non-locked map node before Plan 85 (its fiction — a bunker scouted from the ridge — predates the hidden-node pattern). Changing its danger to `locked` would retro-lock it for existing saves that had already discovered it (ResolveNodeStatus checks Locked before Discovered), so it was left as-is. It gains the missing expedition destination and the same loot path as every other installation.

## Save behavior

Reveal state lives in `WastelandMapState.Discovered`/`Unlocked` (existing `wasteland_map` section). Fragment registration + reveal persist together; reload cannot re-fire completion (pinned by tests). Catalog expansion against an old save initializes new zones undiscovered (pinned by test).
