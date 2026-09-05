# Standing Record Wants & Offers Contract

## 1. Schema & Technical Specification

| Field Property | Specification | Implementation Evidence |
|---|---|---|
| **JSON Field Names** | `wants`, `offers` | `Assets/StreamingAssets/Data/standing_record_factions.json` |
| **C# Data Type** | `string[]` | `StandingRecordFactionEntryDto` / `HoldfastFactionDef` |
| **Deserializer** | `SystemTextJsonSerializer` via `CatalogLocator.LoadWrappedList<T>` | Preserves order; null-coalesced to empty array if omitted |
| **Unknown Value Handling** | Silently displayed in UI dossier strips | Presentation tokens rendered via UI formatting helpers |
| **Prefix Validation** | Scanned by `CatalogIntegrityValidator.cs` | Tokens must NOT start with reserved entity prefixes (e.g. `crop_`, `quest_`, `item_`) unless they resolve to active catalog records. |
| **Ordering Semantics** | Preserved from JSON declaration | Primary desire/offer displayed first in UI cards |
| **Duplicates Policy** | Forbidden within the same faction; differentiated across factions | Enforced by `StandingRecordFactionExpansionTests.TradeProfiles_AreDifferentiated` |
| **Gameplay Impact** | Presentation & economic flavor | Drives NPC dialogue context, trade panel demand tags, and contract requirements; does not alter raw barter math without active hooks. |

---

## 2. Wants Mapping Matrix

| Faction ID | Authored Wants Tokens | Resource Domain | Provenance / Circulation in World | Validator Result |
|---|---|---|---|---|
| `faction_the_overlay` | `brass_fittings`, `sr_stencil_pot`, `lamp_oil` | Cadastral & Survey Supplies | Found in administrative ruins, metal caches, and workshops | PASS |
| `faction_the_scale` | `brass_valve_bodies`, `filter_charcoal`, `pipe_sealant` | Hydraulic & Pipeline Maintenance | Salvaged from pumping stations and chemical refineries | PASS |
| `faction_the_compact` | `parchment_rolls`, `iron_gall_ink`, `surveyor_transit_glass` | Archival & Recordkeeping | Sourced from civic halls, libraries, and optical ruins | PASS |
| `faction_the_underwrite` | `hardened_plate_carriers`, `heavy_machine_oil`, `sealed_logistics_manifests` | Security & Convoy Maintenance | Procured from military motor pools and freight depots | PASS |
| `faction_the_cutters` | `hardened_ice_spikes`, `black_coal_briquettes`, `steel_winch_cable` | Winter Engineering & Haulage | Mined from rail coal yards, quarry stores, and cable winches | PASS |
| `faction_the_fleet` | `tarred_hemp_rigging`, `marine_pitch_caulk`, `copper_hull_nails` | Maritime Maintenance & Caulk | Gathered from coastal shipyards, chandlers, and boathouses | PASS |
| `faction_the_rebuilders` | `viable_heirloom_seeds`, `refractory_kiln_bricks`, `sterilized_field_dressings` | Agriculture & Kiln Infrastructure | Collected from agricultural vaults, pottery ruins, and clinics | PASS |
| `faction_the_garrison` | `smokeless_powder_kegs`, `machined_rifle_extractors`, `preserved_medical_plasma` | Munitions & Checkpoint Defense | Stockpiled at arsenal ruins, military armories, and aid stations | PASS |

---

## 3. Offers Mapping Matrix

| Faction ID | Authored Offers Tokens | Functional Service / Benefit | Consumer Subsystem | Realizability Status |
|---|---|---|---|---|
| `faction_the_overlay` | `cadastral_keys`, `travel_correction_on_named_sites` | Map correction and cadastral unlock chits | Wasteland Map / UI Dossier | Realizable / Displayed |
| `faction_the_scale` | `potable_ration_quota`, `sluice_transit_clearance`, `flow_rate_telemetry` | Clean water chits, gate passage, pipeline telemetry | Shelter Water Grid / Expeditions | Realizable / Displayed |
| `faction_the_compact` | `cadastral_boundary_deeds`, `arbitration_records`, `survey_marker_waypoints` | Land deed proofs, dispute mediation, waypoints | Wasteland Gazetteer / Navigation | Realizable / Displayed |
| `faction_the_underwrite` | `convoy_underwriting`, `armed_escort_vouchers`, `depot_fuel_draws` | Caravan escort guarantees, fuel depot access | Caravan Trade / Expeditions | Realizable / Displayed |
| `faction_the_cutters` | `cleared_corridor_passage`, `heavy_haulage_sledges`, `span_waystation_shelter` | Ice-road passage, haulage equipment, waystation shelter | Rail / Expedition Transit | Realizable / Displayed |
| `faction_the_fleet` | `barge_ferry_berth`, `coastal_salvage_tolls`, `tide_table_nav_charts` | Coastal ferry berth, salvage permissions, tide charts | Coastal Navigation / Trade | Realizable / Displayed |
| `faction_the_rebuilders` | `staple_grain_bushels`, `soil_rotation_almanac`, `communal_silo_storage` | Food rations, crop rotation data, grain storage access | Greenhouse / Food Storage | Realizable / Displayed |
| `faction_the_garrison` | `checkpoint_transit_chits`, `sentry_perimeter_watch`, `tactical_hazard_briefings` | Checkpoint transit chits, sentry warnings, scout data | Border Checkpoints / Travel Map | Realizable / Displayed |
