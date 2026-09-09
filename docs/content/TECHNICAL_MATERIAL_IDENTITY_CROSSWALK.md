# TECHNICAL MATERIAL IDENTITY CROSSWALK — Plan 158 Workstream B

Canonical identity audit for every authored equipment label across the two
corpora. Rule: only PROVEN canonical item ids may participate in item
inspection; unresolved labels remain visible provenance and never enter
integrity validation as invented `item_*` ids.

## Crosswalk verdicts

| Authored label space | Examples | Verdict | Canonical link |
|---|---|---|---|
| Hemp retting floors / crop origins | `MUSHROOM_CAVERN_RETTING_BED_01`, `BUNKER_GREENHOUSE_CANNABIS_SATIVA` | Content-only facility/process labels | Hemp fiber records → canonical `rope` (Hemp Rope 10 m, Component, trade 8) where the process output is rope stock |
| Wire-rope spool identifiers | `HOIST_SHAFT_MAIN_CABLE_01`, `BLAST_DOOR_COUNTERWEIGHT_CABLE` | Equipment labels (infrastructure) — no canonical wire-rope item exists | **None** (descriptive) |
| Manila hawser coils | `TUNNEL_FERRY_TOW_HAWSER_01`, `BARGE_WARPING_CAPSTAN_LINE` | Equipment labels (maritime gear) — no hawser item exists | **None** (descriptive) |
| Drive-line shafts | `CENTRAL_MACHINE_SHOP_MAIN_SHAFT`, `FORGE_TRIP_HAMMER_HEADER_LINE` | Infrastructure labels | **None** (descriptive; Plan 148 owns live machine state) |
| Neoprene mask models | `M17_SURVIVAL_RESPIRATOR_FACEPIECE`, `ARCTIC_PERIMETER_SENTRY_MASK`, … | **Historical/model designations — content-only labels.** No canonical item uses these designations | Gasket records → canonical `gas_mask` / `item_gas_mask_improved` (protective elastomer goods) by MATERIAL kinship, not by model identity |
| Aramid `armor_item_id` | `PASGT_BALLISTIC_VEST_MK2`, `BALLISTIC_COMBAT_HELMET_SHELL`, … | **Historical/model designations.** No canonical ballistic vest/helmet items exist in items.json | **None.** Registering these as item links fails closed (pinned by test) |
| Tire casing IDs | `MILITARY_TRUCK_TIRE_11R20`, `COMMUNAL_BICYCLE_INNER_TUBE_28IN` | Historical workshop labels. Vehicles are condition-modelled wholes (`vehicles.json`), no tire item parts | **None** (descriptive) |
| Film reels | `CIVIL_DEFENSE_REEL_35MM_104`, `MORALE_CINEMA_FEATURE_REEL_02` | Archive-reel labels — kinship with canonical film goods | Celluloid records → canonical `film_reel` / `photographic_film` |
| Elastomer goods | (polymer types: butyl, silicone, PVC, neoprene…) | Material chemistry | Selected gasket records → `protective_rubber_gloves`, `item_hermetic_hatch_silicone_gasket`, `rubber_hose` |

## Rules enforced

1. Every configured canonical link resolves in items.json (test:
   `CanonicalLinks_AllResolve_InItemAuthority`,
   `CanonicalLinks_NeverPointAtFakeItems`).
2. `TryRegisterCanonicalLink` fails closed for unresolved ids — an authored
   model designation can never masquerade as an item identity (test:
   `CanonicalLinks_RejectUnresolvedItems_FailClosed`).
3. No duplicate item identities were created; no rope/armor/tire items were
   authored to make narrative data resolve (Plan 158 §7.A1).

## Real-world designation note (Task J continuity)

Several authored model designations echo pre-war military nomenclature
(M17, CDV-715, MCU-2P, PASGT, 11R20). They are treated as in-world
archival designations of pre-Exchange equipment; no live faction, country,
war or person is referenced. Flagged for the standing tone sweep rather than
rewritten by this plan.
