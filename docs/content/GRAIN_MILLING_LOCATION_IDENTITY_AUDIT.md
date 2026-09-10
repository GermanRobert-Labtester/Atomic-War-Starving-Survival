# Grain Milling Location Identity Audit & Equipment Mapping

## 1. Directive & Scope

In accordance with **Plan 157 §7 (Workstream B)**, process-specific equipment identifiers (such as `PRIMARY_BURR_RUNNER_PAIR_01`, `CENTRIFUGAL_FORCE_REEL_02`, `DEEP_STORAGE_SILO_ALPHA_01`) represent industrial machinery labels, NOT canonical game engine location IDs (`loc_*`).

**Rule**: Never create a new world map location solely to satisfy an authored equipment label. Instead, map equipment identifiers to existing canonical locations and shelter rooms based on verified world lore, industrial purpose, and geographic context.

---

## 2. Equipment Identifier to Canonical Location Mapping

| Equipment Identifier | Catalog Family | Canonical Location ID | Location Display Name | Faction / Custodian | Map State | Mapping Rationale |
|---|---|---|---|---|---|---|
| `PRIMARY_BURR_RUNNER_PAIR_01` | Millstone Dressing | `loc_grain_silo` | Old Militia Grain Silo | Grain Exchange Neutral Hub | Discovered / Map | Main grist mill under the leaning silo structure. |
| `RESERVE_GRIST_MILL_RUNNER_02` | Millstone Dressing | `loc_quarry` | High Scarp Slate Quarry | Independent Quarrymen | Expedition Exploration | Quarry workshops extracting mill hones and dressing stones. |
| `COMMISSARY_FLOUR_MILL_01` | Millstone Dressing | `loc_grain_silo` | Old Militia Grain Silo | Militia / Commercial Traders | Discovered / Map | Commissary wholemeal milling plant at the trade exchange. |
| `WATER_DRIVEN_GRIST_MILL_03` | Millstone Dressing | `loc_quarry` | High Scarp Slate Quarry | Independent Hermits | Expedition Exploration | Hydro-powered mill in the wet lower slate drainage tunnels. |
| `PRECISION_SEMOLINA_STONE_04` | Millstone Dressing | `room_workshop` | Shelter Workshop | Holdfast Survivors | Starting Shelter | Shelter precision machining bench and stone tentering rig. |
| `AUTOMATED_HOPPER_FEED_STONE` | Millstone Dressing | `loc_settlement_silo_burrow` | New Ceres Silo Collective | Ceres Agrarian Commune | Expedition Exploration | Mechanized grain intake hopper at the New Ceres silo base. |
| `DUST_FREE_ENCLOSED_MILL_02` | Millstone Dressing | `room_workshop` | Shelter Workshop | Holdfast Survivors | Starting Shelter | Enclosed grinding vat fitted with leather sweepers. |
| `HEAVY_FOUNDRY_MILL_RUNNER_01` | Millstone Dressing | `room_foundry` | Iron Foundry | Holdfast Survivors | Built Shelter Room | Thermal shrink-fit hoop assembly requiring foundry heat. |
| `PRIMARY_PATENT_FLOUR_BOLTER_01` | Bolting Silk | `room_common_mess_hall` | Shelter Mess Hall / Kitchen | Holdfast Survivors | Starting Shelter | Sifting chest used for daily shelter flatbread rations. |
| `CENTRIFUGAL_FORCE_REEL_02` | Bolting Silk | `loc_grain_silo` | Old Militia Grain Silo | Grain Exchange Traders | Discovered / Map | High-throughput centrifugal reel sifting trade flour. |
| `STORED_FLOUR_SIFTER_UNIT_03` | Bolting Silk | `loc_grain_silo` | Old Militia Grain Silo | Grain Exchange Traders | Discovered / Map | Humid basement sifter affected by storage mites. |
| `DRY_CLIMATE_PLANSIFTER_01` | Bolting Silk | `room_workshop` | Shelter Workshop | Holdfast Survivors | Starting Shelter | Plansifter adapted to dry subterranean bunker air. |
| `FREE_SWINGING_PLANSIFTER_BAY` | Bolting Silk | `room_workshop` | Shelter Workshop | Holdfast Survivors | Starting Shelter | Suspended twelve-sieve swinging bay under mechanical test. |
| `MIDDLINGS_PURIFIER_DECK_04` | Bolting Silk | `loc_settlement_silo_burrow` | New Ceres Silo Collective | Ceres Agrarian Commune | Expedition Exploration | Aspiration semolina deck used for commune pasta production. |
| `MODERNIZED_SIFTING_CHEST_01` | Bolting Silk | `room_workshop` | Shelter Workshop | Holdfast Survivors | Starting Shelter | Upgraded synthetic polyamide monofilament test bench. |
| `TERMINAL_BRAN_DUSTER_UNIT` | Bolting Silk | `loc_grain_silo` | Old Militia Grain Silo | Grain Exchange Traders | Discovered / Map | High-velocity scouring unit recovering adherent flour. |
| `DEEP_STORAGE_SILO_ALPHA_01` | Silo Weevil | `loc_grain_silo` | Old Militia Grain Silo | Militia Quartermasters | Discovered / Map | Primary deep grain bin afflicted by Sitophilus weevils. |
| `REINFORCED_CONCRETE_SILO_03` | Silo Weevil | `loc_settlement_silo_burrow` | New Ceres Silo Collective | Ceres Agrarian Commune | Expedition Exploration | Concrete silo bin suffering cold-wall condensation crusting. |
| `HERMETIC_STEEL_SILO_CELL_04` | Silo Weevil | `loc_agricultural_outpost` | Agricultural Outpost | Outpost Farmers | Expedition Exploration | Modern hermetic steel cell using CO2 gas asphyxiation. |
| `EMERGENCY_RESERVE_SILO_02` | Silo Weevil | `loc_grain_silo` | Old Militia Grain Silo | Grain Exchange Neutral Hub | Discovered / Map | Reserve bin that suffered biological 48°C hotspot spike. |
| `CENTRAL_COMMISSARY_GRAIN_ELEVATOR` | Silo Weevil | `loc_settlement_silo_burrow` | New Ceres Silo Collective | Ceres Agrarian Commune | Expedition Exploration | Bucket elevator treated with diatomaceous earth desiccant. |
| `CONICAL_DISCHARGE_BIN_05` | Silo Weevil | `loc_grain_silo` | Old Militia Grain Silo | Grain Exchange Neutral Hub | Discovered / Map | Steep steel discharge bin affected by rye awn rat-holing. |
| `MAIN_STORAGE_BATTERY_SILO_06` | Silo Weevil | `loc_settlement_silo_burrow` | New Ceres Silo Collective | Ceres Agrarian Commune | Expedition Exploration | Battery silo undergoing pneumatic winter turnover. |
| `PRIMARY_TEMPERING_SILO_01` | Tempering Assay | `room_greenhouse` | Shelter Greenhouse | Holdfast Botanist | Starting Shelter | Shelter seed and grain hydration conditioning barrel. |
| `SECONDARY_REST_BIN_BAY_02` | Tempering Assay | `loc_settlement_silo_burrow` | New Ceres Silo Collective | Ceres Agrarian Commune | Expedition Exploration | Pine timber rest bins for grain moisture equilibration. |
| `HYDROTHERMAL_SCOURER_SKID` | Tempering Assay | `room_workshop` | Shelter Workshop | Holdfast Survivors | Starting Shelter | Compact 52°C water scouring skid for seed decontamination. |
| `PRECISION_MILLING_BIN_03` | Tempering Assay | `loc_agricultural_outpost` | Agricultural Outpost | Outpost Technicians | Expedition Exploration | Calibrated durum wheat tempering bin optimizing roller energy. |
| `EXPERIMENTAL_WET_MILL_CELL` | Tempering Assay | `room_workshop` | Shelter Workshop | Holdfast Survivors | Starting Shelter | R&D wet milling rig that experienced fluting choke. |
| `ENTOLETER_IMPACT_STATION` | Tempering Assay | `loc_settlement_silo_burrow` | New Ceres Silo Collective | Ceres Agrarian Commune | Expedition Exploration | High-speed pin mill for non-chemical insect egg destruction. |
| `CONTINUOUS_INTENSIVE_DAMPENER` | Tempering Assay | `loc_grain_silo` | Old Militia Grain Silo | Grain Exchange Millwrights | Discovered / Map | Industrial spray dampener retrofitted into central mill. |

---

## 3. Location Identity Reconciliation

- **`loc_grain_silo` (Old Militia Grain Silo)**: Canonical hub of wasteland grain trade. The lore explicitly establishes it handles 80% of the district's grain via hand-cranked scales and neutral barter. It naturally hosts 10 of the catalog's major industrial milling and storage units.
- **`loc_settlement_silo_burrow` (New Ceres Silo Collective)**: An agrarian commune inside three concrete silos. It logically hosts the community's cooperative storage, aspiration purifiers, and biological control systems (8 units).
- **`loc_agricultural_outpost` (Agricultural Outpost)**: Pre-war agricultural research facility housing specialized hermetic steel silos and durum conditioning bins (2 units).
- **`loc_quarry` (High Scarp Slate Quarry)**: Subterranean stone quarry where stonemasons dress millstones and salvage heavy timber bearings (2 units).
- **Shelter Facilities (`room_workshop`, `room_common_mess_hall`, `room_greenhouse`, `room_foundry`)**: The player's holdfast workshops and kitchens host the remaining 8 portable, experimental, or shelter-scale milling devices.
