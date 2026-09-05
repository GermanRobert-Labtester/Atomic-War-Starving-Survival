# Plan 98 Completion Report

## Baseline
- **standing_record_factions.json before:** 1 entry under `"actions"` wrapper
- **existing faction:** `faction_the_overlay` ("The Overlay")
- **Standing Record loader/consumer:** `CatalogLocator.LoadWrappedList<T>`, `LocationLayoutSystem`, `StandingRecordPanel`
- **trust ownership:** Authored integer represents starting baseline (`0`); mutable live reputation is owned by `SaveStoreHub` in `campaign.json`
- **alignment semantics:** Initial diplomatic posture (`conditional`, `neutral`)
- **wants semantics:** String array of desired material/commodity tokens, free of reserved entity prefixes
- **offers semantics:** String array of services, rights, and resource allocations
- **access_rule semantics:** Succinct descriptive jurisdictional condition string rendered on UI dossier cards
- **region authority:** Canonical macro-regions defined in `WASTELAND_REGION_ATLAS.md` (and `all_regions` sentinel)
- **badge authority:** `FactionIconCatalog.Resolve(id)` with safe fallback to `assets/ui/Icons/faction_icon_unknown.png`

---

## Identity Reconciliation
| Intended Concept | Proposed ID | Existing Collision | Final ID | Relationship to Global Faction | Decision |
|---|---|---|---|---|---|
| The Overlay | `faction_the_overlay` | Native baseline | `faction_the_overlay` | Canonical Standing Record Authority | Preserved byte-for-byte in position 0 |
| The Scale | `faction_the_scale` | None | `faction_the_scale` | Expansion-Local Utility Cartel | Added with Industrial Belt hydraulic identity |
| The Compact | `faction_the_compact` | None (Meridian Compact is historical lore) | `faction_the_compact` | Expansion-Local Deed Archive | Added with Dead Suburbs civic arbitration identity |
| The Underwrite | `faction_the_underwrite` | None | `faction_the_underwrite` | Expansion-Local Logistics Syndicate | Added with convoy escort and risk underwriting identity |
| The Cutters | `faction_the_cutters` | None | `faction_the_cutters` | Expansion-Local Corridor Maintenance | Added with The Cut alpine ice-road identity |
| The Fleet | `faction_the_fleet` | Black Flotilla (`faction_black_flotilla`) | `faction_the_fleet` | Distinct Civilian Maritime Cooperative | Disambiguated as working barge/dock operators on Deep Coast |
| The Rebuilders | `faction_the_rebuilders` | None (Silo Commune in gazetteer) | `faction_the_rebuilders` | Regional Agrarian Bloc | Added with Ash Flats communal seed and silo identity |
| The Garrison | `faction_the_garrison` | Fort Karkov Garrison in gazetteer | `faction_the_garrison` | Canonical Global Projection | Reconciled as Fort Karkov border sentry authority |

---

## Final Faction Matrix
| ID | Display Name | Alignment | Home Region | Initial Trust | Wants | Offers | Access Rule | Badge |
|---|---|---|---|---:|---|---|---|---|
| `faction_the_overlay` | The Overlay | `conditional` | `all_regions` | 0 | `brass_fittings`, `sr_stencil_pot`, `lamp_oil` | `cadastral_keys`, `travel_correction_on_named_sites` | Acknowledge ground marks; no unauthorized survey tampering. | `""` (fallback) |
| `faction_the_scale` | The Scale | `conditional` | `industrial_belt` | 0 | `brass_valve_bodies`, `filter_charcoal`, `pipe_sealant` | `potable_ration_quota`, `sluice_transit_clearance`, `flow_rate_telemetry` | Maintain conduit integrity; unmetered water draw is prohibited. | `""` (fallback) |
| `faction_the_compact` | The Compact | `neutral` | `dead_suburbs` | 0 | `parchment_rolls`, `iron_gall_ink`, `surveyor_transit_glass` | `cadastral_boundary_deeds`, `arbitration_records`, `survey_marker_waypoints` | Abide by recorded deed boundaries; arbitration outcomes are binding. | `""` (fallback) |
| `faction_the_underwrite` | The Underwrite | `conditional` | `industrial_belt` | 0 | `hardened_plate_carriers`, `heavy_machine_oil`, `sealed_logistics_manifests` | `convoy_underwriting`, `armed_escort_vouchers`, `depot_fuel_draws` | Settle transit premiums upfront; breach of escort voiding forfeits coverage. | `""` (fallback) |
| `faction_the_cutters` | The Cutters | `conditional` | `the_cut` | 0 | `hardened_ice_spikes`, `black_coal_briquettes`, `steel_winch_cable` | `cleared_corridor_passage`, `heavy_haulage_sledges`, `span_waystation_shelter` | Yield roadbed to clearing crews; waystation haulage chits required. | `""` (fallback) |
| `faction_the_fleet` | The Fleet | `conditional` | `deep_coast` | 0 | `tarred_hemp_rigging`, `marine_pitch_caulk`, `copper_hull_nails` | `barge_ferry_berth`, `coastal_salvage_tolls`, `tide_table_nav_charts` | Honor estuary berthing chits; uncaulked or unmanifested hulls turned away. | `""` (fallback) |
| `faction_the_rebuilders` | The Rebuilders | `neutral` | `ash_flats` | 0 | `viable_heirloom_seeds`, `refractory_kiln_bricks`, `sterilized_field_dressings` | `staple_grain_bushels`, `soil_rotation_almanac`, `communal_silo_storage` | Respect fallow rotation schedules; seed grain reserves are inviolate. | `""` (fallback) |
| `faction_the_garrison` | The Garrison | `conditional` | `ash_flats` | 0 | `smokeless_powder_kegs`, `machined_rifle_extractors`, `preserved_medical_plasma` | `checkpoint_transit_chits`, `sentry_perimeter_watch`, `tactical_hazard_briefings` | Halt for sentry inspection; armed entry without transit vouchers strictly barred. | `""` (fallback) |

---

## Cross-Plan Integrations
- **Plan 44 territory:** `supported_after_landed_plan` (The Scale, The Cutters, and The Fleet mapped to key regional chokepoints and transit hubs)
- **Plan 45 patrols:** `defer` (Garrison patrols covered by existing Fort Karkov encounter tables; civilian road crews deferred)
- **Plan 43 settlements:** `already_supported` (Fort Karkov and Silo Commune map directly in `wasteland_settlement_gazetteer.json`)
- **Plan 92 dialogue:** `already_supported` (Overheard lines and faction voice lines integrate via `faction_war_dialogue.json`)
- **Plan 89 epilogues:** `already_supported` (Epilogue matrix safely projects live faction standing without code changes)

---

## Save Compatibility
- **new game:** PASS (all 8 factions initialized with default trust 0)
- **old save fixture:** PASS (missing factions defaulted without corrupting existing progress)
- **trust mutation round-trip:** PASS (mutations preserved across serialization cycles)
- **access threshold restore:** PASS (tier gates evaluate correctly from restored values)

---

## Validation
- **data integrity:** PASS (`godot --headless --path . -- --data-integrity-selftest`: 0 errors across 216 catalogs)
- **tests:** PASS (`dotnet test Ashfall.Core.Tests`: 7,460 passed, 0 failed)
- **build:** PASS (`dotnet build Ashfall.csproj`: 0 errors, 0 warnings)
- **content utilization:** PASS (`godot --headless --path . -- --content-utilization-selftest`: CI gate PASS)
- **exported content/badges:** PASS (`assets/ui/Icons/faction_icon_unknown.png` verified)

---

## Deviations
- **Prefix Collision Prevention:** Shifted candidate want token `crop_rotation_almanac` to `soil_rotation_almanac` to prevent unintended entity validation against Greenhouse `crop_` prefixes in `CatalogIntegrityValidator.cs`.
- **Existing Test Modernization:** Updated `LocationLayoutSystemTests.cs:253` from strict `Assert.Single(factions)` to targeted resolution of `faction_the_overlay`, preserving test coverage while accommodating catalog expansion.

---

## Follow-ups
- **Emblem Artwork:** Author dedicated vector/PNG badges for the 7 new factions and wire into `FactionIconCatalog._systemsIdsToIcon`.
- **Expanded Dialogue Sets:** Expand overheard banter and site conversations in `faction_war_dialogue.json` for The Scale, The Cutters, and The Underwrite.
- **Dynamic Territory Capture:** Once Plan 44 lands, formally wire territorial jurisdiction nodes to The Scale, The Cutters, and The Fleet.
