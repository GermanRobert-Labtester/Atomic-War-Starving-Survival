# ASHFALL — EXPANSION 3 & 4: COMPREHENSIVE IMPLEMENTATION PLAN

> **Status**: Planning complete — ready for implementation
> **Target**: ~60% reuse of existing infrastructure, ~40% new code
> **Owner**: Pi (C# systems, data, wiring) + Cursor (UI widgets, assets)

---

## I. EXISTING INFRASTRUCTURE AUDIT (What We Already Have)

### Exp 3: Locations, Loot, Quests
| What Exists | File | What It Does | What's Missing |
|-------------|------|-------------|----------------|
| `LocationDefinitionSO` | `Data/LocationDefinitionSO.cs` | Static def: danger, travel, baseRads | Procedural attributes, 20 new archetypes |
| `LocationCatalogSO` | `Data/LocationCatalogSO.cs` | Registry of all locations | 20 new entries |
| `LocationScavengingSystem` | `Core/LocationScavengingSystem.cs` | Roll loot from LocationDefinition | Variable item condition/contamination |
| `LootTableSO` | `Data/LootTableSO.cs` | Weighted loot entries | Condition/contamination modifiers |
| `VariableLootNode` | `Utilities/VariableLootNode.cs` | Scrap yield variance | Not extended to all item types |
| `ProceduralScavengeSystem` | `Core/ProceduralScavengeSystem.cs` | Procedural scavenging | Needs variable item output |
| `LocationEvolutionSystem` | `World/LocationEvolutionSystem.cs` | Our new system (Phase 17) | Already handles location state changes |
| `QuestlineSO` | `Survivors/QuestlineSO.cs` | Personal quest definition | Location-based multi-stage quests |
| `QuestRegistry` | `Quests/QuestRegistry.cs` | Quest registration | Dynamic quest activation |
| `QuestRuntime` | `Quests/QuestRuntime.cs` | Runtime quest state | Multi-stage tracking |
| `ItemDefinition` | `Inventory/ItemDefinition.cs` | Static item template | Dynamic condition/contamination fields |
| `ItemInstance` concept | Not yet | N/A | NEEDS CREATION |

### Exp 4: Warfare, Factions, Vehicles
| What Exists | File | What It Does | What's Missing |
|-------------|------|-------------|----------------|
| `HatchDefenseSystem` | `Shelter/HatchDefenseSystem.cs` | Raid resolution, upgrades, repel costs | Siege state, tactical commands, sentry guns |
| `HatchDefenseSystem.HatchUpgrades` | (partial class) | Hatch reinforcement tiers | Methane traps, decon flush, gunports |
| `RaidResolution` | (inner class) | Raid outcome data | Siege-specific resolution data |
| `FactionSO` | `Economy/FactionSO.cs` | Static faction definition | Reputation, tribute demands, alliances |
| `FactionPressureWiring` | `Core/FactionPressureWiring.cs` | Faction pressure tracking | Double agents, propaganda, defection |
| `FactionRaidPlanSystem` | `Core/FactionRaidPlanSystem.cs` | Raid planning | Pre-raid intelligence, signal jamming |
| `VehicleSystem` | `Core/VehicleSystem.cs` | Basic vehicle data + components | Biodiesel, breakdowns, ramming, winch, solar |
| `VehicleData` | (inner class) | Speed, cargo, fuel consumption | Armor plating, mounted weapons, medical bay |
| `VehicleStrandingSystem` | `Core/VehicleStrandingSystem.cs` | Vehicle breakdown events | Already handles breakdowns |
| `Encounter_Roadblock` | `Encounters/Encounter_Roadblock.cs` | Road ambush encounter | Needs expansion for vehicle combat |
| `Siege_Artillery` through `Siege_VehicleRam` | `Encounters/Siege_*.cs` | 6 siege encounter types | Needs tactical command integration |
| `CombatPerkSystem` | `Survivors/CombatPerkSystem.cs` | Combat milestone perks | Suppressor crafting, tactical gear perks |
| `Item_AmmoTypes` | `Inventory/Items/Item_AmmoTypes.cs` | Ammo types | Satchel charges, tear gas, smoke canisters |
| `FieldGearLoadoutSystem` | `Inventory/FieldGearLoadoutSystem.cs` | Face/body equipment | Tactical flashlights, NVG, body armor plates |

---

## II. EXPANSION 3: IMPLEMENTATION PLAN

### Phase 3A: ProceduralItemInstance System (1 new file)
**Goal**: Add dynamic condition, contamination, and purity to all scavenged items.

**New file**: `Assets/_Game/Inventory/ProceduralItemInstance.cs`
```csharp
// Extends ItemDefinition with runtime variability:
// - condition_pct (0..1): affects durability/effectiveness
// - contamination_pct (0..1): radiation/chemical/mold
// - purity_mult (0.5..1.5): calorie content or water purity
// - scrap_yield_roll: how many components from salvaging
// - expiration_state: Fresh/Expired/Degraded for medical/food
```

**Modify**: `LocationScavengingSystem.cs` — after rolling loot from LootTableSO, apply ProceduralItemInstance variance based on:
- Location danger level (higher danger = worse condition)
- Scavenger skill (higher skill = better rolls)
- World phase (later phases = more degraded items)

### Phase 3B: 20 Location Archetypes (data-only)
**Modify**: `Assets/StreamingAssets/Data/locations.json` — add 20 new entries with procedural parameters.

| # | id | Danger | Ambient Sv | Collapse Risk | Primary Loot |
|---|-----|--------|-----------|---------------|-------------|
| 1 | `loc_civil_defense_bunker` | 0.4 | 0.3 | 0.6 | medical, filters, rations |
| 2 | `loc_water_treatment_plant` | 0.6 | 0.5 | 0.4 | water_filters, valves, chemicals |
| 3 | `loc_highway_checkpoint` | 0.7 | 0.4 | 0.2 | ammo, fuel, military_gear |
| 4 | `loc_grain_silo` | 0.5 | 0.2 | 0.8 | wheat, rodent_scraps, sacks |
| 5 | `loc_substation_yard` | 0.6 | 0.3 | 0.3 | copper_wire, capacitors, electronics |
| 6 | `loc_regional_hospital` | 0.8 | 0.7 | 0.5 | antibiotics, surgical_kits, iodine |
| 7 | `loc_suburban_district` | 0.5 | 0.3 | 0.4 | cloth, scrap_metal, canned_food |
| 8 | `loc_train_yard` | 0.6 | 0.6 | 0.3 | steel_rails, tools, diesel_fuel |
| 9 | `loc_comm_array` | 0.4 | 0.2 | 0.1 | vacuum_tubes, wiring, radio_parts |
| 10 | `loc_ash_woodland` | 0.5 | 0.4 | 0.3 | firewood, game_scraps, resin |
| 11 | `loc_urban_pharmacy` | 0.4 | 0.3 | 0.2 | bandages, sedatives, antiseptic |
| 12 | `loc_missile_silo` | 0.9 | 0.9 | 0.7 | alloy_plates, rocket_fuel, heavy_armor |
| 13 | `loc_fuel_depot` | 0.7 | 0.4 | 0.5 | gasoline, kerosene, rubber_hose |
| 14 | `loc_metro_tunnel` | 0.6 | 0.3 | 0.6 | scrap_metal, clean_water, wiring |
| 15 | `loc_agricultural_coop` | 0.4 | 0.5 | 0.3 | fertilizer, seeds, hand_tools |
| 16 | `loc_basement_vault` | 0.5 | 0.2 | 0.4 | sealed_cans, waterproof_gear, keys |
| 17 | `loc_police_precinct` | 0.6 | 0.2 | 0.3 | riot_shields, shotgun_shells, badges |
| 18 | `loc_botanical_nursery` | 0.5 | 0.3 | 0.2 | medicinal_herbs, seeds, clean_water |
| 19 | `loc_evacuation_bus_depot` | 0.3 | 0.3 | 0.3 | spare_tires, batteries, scrap_cloth |
| 20 | `loc_coal_mine` | 0.7 | 0.2 | 0.8 | coal, drill_bits, miner_helmets |

### Phase 3C: Multi-Stage Questline Framework (2 new files)
**New file**: `Assets/_Game/Quests/DynamicQuestlineSystem.cs`
- Extends existing `QuestRegistry` / `QuestRuntime` pattern
- Tracks multi-stage quests with `QuestStage` struct
- Evaluates objective progress on expedition return / hourly tick
- Raises `OnQuestStageAdvanced`, `OnQuestCompleted`
- Support for `Discovery → Investigation → Crisis Choice → Resolution` pattern

**Data file**: `Assets/StreamingAssets/Data/dynamic_questlines.json` — 2 example questlines:
1. **The Dying Signal** (`quest_dying_signal`): 4 stages, 2 choice branches
2. **The Aquifer Contamination** (`quest_aquifer_contamination`): 3 stages, 2 choice branches

### Phase 3D: Loot Table Expansion (data-only)
**Modify**: `Assets/StreamingAssets/Data/items.json` — add dynamic attribute tags:
- `variable_condition` tag → condition rolled per instance
- `variable_contamination` tag → contamination rolled
- `variable_purity` tag → purity/calorie multiplier rolled
- `expirable` tag → Fresh/Expired/Degraded states
- `containered` tag → Volume + Integrity for water containers

### Phase 3E: UI Widgets for Cursor (3 new widgets)
1. **LocationDetailPanel** — Shows danger/radiation/collapse risk + loot preview for a selected location
2. **ItemConditionBadge** — Small badge on inventory items showing condition % and contamination status
3. **QuestlineProgressTracker** — Visual multi-stage quest tracker with stage icons and choice history

---

## III. EXPANSION 4: IMPLEMENTATION PLAN

### Phase 4A: Hatch Siege & Defense Tactics (#81–90) — Extend Existing

**Extend**: `HatchDefenseSystem.cs` + partial classes — add:
- `HatchDefenseState` struct with: `hatch_integrity_pct`, `reinforcement_tier` (1-3), `is_under_siege`, `breach_progress`
- `DeployMethaneTrap()` — consumes fuel, triggers explosion, causes indoor CO
- `DeployGunports()` — +cover bonus, -entry room safety
- `DeployTearGas()` — slows raiders, requires gas masks
- `TriggerControlledCollapse()` — seals tunnel permanently, loses room access
- `DeployBarbedWire()` — bleeds breaching forces
- `DeployAutoTurret()` — consumes power + ammo, automated defense
- `DeconFlushAttack()` — steam/chemical flush on occupied airlock
- `AssignSniperOverwatch()` — suppresses enemy mortar teams
- `IssueTacticalCommand(commandType)` — HoldTheLine, TacticalRetreat, SuppressiveFire

**New file**: `Assets/_Game/Shelter/HatchDefenseSystem.SiegeTactics.cs` (partial class)

### Phase 4B: Faction Intelligence & Espionage (#91–100) — Extend Existing

**Extend**: `FactionPressureWiring.cs` + `FactionRaidPlanSystem.cs`:
- `CovertRadioInterception()` — 24h advance notice of attacks/convoys
- `SendDoubleAgent(survivorId, factionId)` — charisma-based infiltration with betrayal risk
- `DemandTribute(factionId, resourceType, amount)` — recurring tribute system
- `CounterPropaganda(factionId)` — weakens faction hold, triggers uprisings
- `HostageNegotiation(prisonerId, demandType)` — exchange prisoners for resources
- `PlantSabotagedSupplies(factionId)` — weakens faction raiding power over time
- `InstigateDefection(factionId)` — persuade enemy soldiers to defect
- `DeployFakeSignalDecoy()` — redirects raids to empty ruins
- `EstablishInformantNetwork()` — alcohol/water bribes for early warnings
- `FormFactionAlliance(factionId)` — unlocks tech, incurs rival hostility

**New file**: `Assets/_Game/Factions/FactionIntelligenceSystem.cs`

### Phase 4C: Vehicle Reclamation & Motorized Expeditions (#101–110)

**Extend**: `VehicleSystem.cs` + `VehicleData`:
- Add `VehicleArmorPlating`, `MountedWeapon`, `MedicalBay`, `Winch`, `SolarArray` fields
- `RefineBiodiesel(organicWaste, tallow)` → crude biodiesel at chemical bench
- `VehicleBreakdownCheck()` → breakdown in dead zones, field repair or abandon
- `EquipHarpoonWinch()` → clear barricades, pull submerged crates
- `ConvertToMobileCommandPost()` → field surgery on expeditions
- `EquipSpikedBumper()` → smash through roadblocks
- `RefitSolarElectric()` → silent daytime travel, zero fuel
- `ScavengeMilitaryConvoy()` → multi-day timed expedition vs rival scavengers

**New AI actions**: `RefineBiodieselActionSO`, `RepairVehicleActionSO`, `MountWeaponActionSO`

**New items**: `item_biodiesel`, `item_armor_plate_steel`, `item_winch_kit`, `item_solar_panel_vehicle`

### Phase 4D: Advanced Tactical Scavenging & Combat (#111–120)

**New systems + items**:
- **Suppressor Crafting**: `CraftingSystem` recipe for improvised suppressors → item reduces noise
- **Tactical Flashlight/NVG**: `FieldGearLoadout` slot → removes darkness penalties
- **Satchel Charges**: `CraftingSystem` recipe → breaches reinforced doors
- **Thermal Insulation**: `FieldGearLoadout` → cold mitigation on winter expeditions
- **Adrenaline Injectors**: `ChemicalDependencySystem` already handles stimulants → add combat boost + crash
- **Cover Destructibility**: `Encounter_*` extend → cover HP degrades under fire
- **Fighting Retreats**: `ExpeditionSystem` → smoke bomb drop, partial loot retention
- **Tourniquet Application**: `MedicalSystem` → stop bleeding, temp limb mobility loss
- **Armor Plate Degradation**: `FieldGearLoadoutSystem` → plates absorb hits but shatter
- **Post-Battle Fatigue**: `CombatTraumaSystem` already handles → add mandatory rest hours

---

## IV. FILE MANIFEST — EXPANSION 3

### New C# Files (3)
```
Assets/_Game/Inventory/ProceduralItemInstance.cs
Assets/_Game/Quests/DynamicQuestlineSystem.cs
Assets/_Game/Shelter/HatchDefenseSystem.SiegeTactics.cs
```

### New JSON Files (2)
```
Assets/StreamingAssets/Data/locations_expansion3.json    (20 new location archetypes)
Assets/StreamingAssets/Data/dynamic_questlines.json       (2 multi-stage questlines)
```

### Modified Files (6)
```
Assets/_Game/Core/LocationScavengingSystem.cs   — apply ProceduralItemInstance variance
Assets/_Game/Data/LocationDefinitionSO.cs       — add procedural attribute fields
Assets/_Game/Data/LootTableSO.cs                — add condition/contamination modifiers
Assets/_Game/Quests/QuestRegistry.cs            — register dynamic questlines
Assets/_Game/Quests/QuestRuntime.cs             — multi-stage tracking
Assets/StreamingAssets/Data/items.json          — add dynamic attribute tags
```

### New Survivor Fields (0)
No new Survivor fields needed — item variance lives on ItemInstance, not Survivor.

### UI Widgets (3 for Cursor)
```
Assets/_Game/UI/LocationDetailPanel.cs + .uxml + .uss
Assets/_Game/UI/ItemConditionBadge.cs + .uxml + .uss
Assets/_Game/UI/QuestlineProgressTracker.cs + .uxml + .uss
```

---

## V. FILE MANIFEST — EXPANSION 4

### New C# Files (7)
```
Assets/_Game/Shelter/HatchDefenseSystem.SiegeTactics.cs
Assets/_Game/Factions/FactionIntelligenceSystem.cs
Assets/_Game/Core/VehicleMaintenanceSystem.cs
Assets/_Game/Core/BiodieselRefinerySystem.cs
Assets/_Game/Inventory/Items/Item_SatchelCharge.cs
Assets/_Game/AI/Actions/RefineBiodieselActionSO.cs
Assets/_Game/AI/Actions/RepairVehicleActionSO.cs
```

### New JSON Files (1)
```
Assets/StreamingAssets/Data/vehicle_configs.json
```

### Modified Files (8)
```
Assets/_Game/Shelter/HatchDefenseSystem.cs          — add siege state fields
Assets/_Game/Core/VehicleSystem.cs                  — extend VehicleData
Assets/_Game/Core/FactionPressureWiring.cs          — add intelligence hooks
Assets/_Game/Core/FactionRaidPlanSystem.cs          — add pre-raid intel
Assets/_Game/Core/ExpeditionSystem.cs               — vehicle integration
Assets/_Game/Encounters/Encounter_Roadblock.cs      — vehicle combat
Assets/_Game/Inventory/FieldGearLoadoutSystem.cs    — tactical gear slots
Assets/_Game/Medical/ChemicalDependencySystem.cs    — combat stimulant crash
```

### New Survivor Fields (~8)
```csharp
// #91: Double agent status
public bool IsDoubleAgent;
public string InfiltratedFactionId;
public float AgentDiscoveryRisk;

// #111-120: Tactical gear
public bool HasSuppressorEquipped;
public bool HasNVGEquipped;
public bool HasThermalInsulation;
public int ArmorPlateCount;
public float PostBattleFatigueHours;
```

### UI Widgets (5 for Cursor)
```
Assets/_Game/UI/SiegeStatusHUD.cs + .uxml + .uss
Assets/_Game/UI/FactionIntelligencePanel.cs + .uxml + .uss
Assets/_Game/UI/VehicleStatusPanel.cs + .uxml + .uss
Assets/_Game/UI/TacticalCommandBar.cs + .uxml + .uss
Assets/_Game/UI/QuestlineStageTracker.cs + .uxml + .uss
```

---

## VI. IMPLEMENTATION ORDER (Pi — C# Systems)

### Day 1: Expansion 3 Core (3 systems)
1. `ProceduralItemInstance.cs` — dynamic condition/contamination/purity
2. 20 location archetypes in `locations_expansion3.json`
3. Wire `LocationScavengingSystem` to apply variance

### Day 2: Expansion 3 Quests (1 system)
4. `DynamicQuestlineSystem.cs` — multi-stage quest engine
5. 2 questlines in `dynamic_questlines.json`
6. Wire to existing `QuestRegistry` + `EventRunner`

### Day 3: Expansion 4 Siege (extend existing)
7. `HatchDefenseSystem.SiegeTactics.cs` — 10 new tactical actions
8. Wire tactical commands to existing `HatchDefenseSystem`
9. Add `HatchDefenseState` struct + siege tick logic

### Day 4: Expansion 4 Factions (1 new system)
10. `FactionIntelligenceSystem.cs` — espionage, double agents, propaganda
11. Wire to `FactionPressureWiring` + `FactionRaidPlanSystem`

### Day 5: Expansion 4 Vehicles (2 new systems)
12. `VehicleMaintenanceSystem.cs` — repair, breakdowns, field modifications
13. `BiodieselRefinerySystem.cs` — organic waste → fuel conversion
14. Extend `VehicleSystem.cs` with new fields
15. New AI actions + items

### Day 6: Expansion 4 Combat (extend existing)
16. Suppressor/flashlight/NVG items + crafting recipes
17. Satchel charge item + breach mechanic
18. Cover destructibility in encounter system
19. Fighting retreat mechanic in ExpeditionSystem
20. Post-battle fatigue in CombatTraumaSystem

### Day 7: Wiring + Tests
21. GameBootstrap wiring for all new systems
22. 20+ EditMode tests
23. JSON data validation
24. Commit + update master plan

---

## VII. CURSOR UI HANDOFF (Expansions 3 & 4)

### Expansion 3 Widgets

#### 1. LocationDetailPanel
- **Shows**: Selected location's danger rating (0-5 skulls), ambient radiation (mSv/hr bar), structural collapse risk (%), faction owner icon, loot preview (3-5 items with drop chance %)
- **Data**: `LocationDefinitionSO` fields + `LocationEvolutionSystem.GetLocationState()`
- **UXML**: Modal panel with stat rows, color-coded danger indicators
- **Canva assets**: Skull icon, radiation symbol, collapse icon, faction icons (5)

#### 2. ItemConditionBadge
- **Shows**: On inventory items — condition % bar (green→yellow→red), contamination warning icon, "Expired" / "Degraded" text overlay
- **Data**: `ProceduralItemInstance` fields
- **UXML**: Small overlay badge on item slot
- **Canva assets**: Biohazard icon, clock/expired icon, condition bar gradient

#### 3. QuestlineProgressTracker
- **Shows**: Multi-stage quest with connected circles, current stage highlighted, completed stages checked, choice history tooltip
- **Data**: `DynamicQuestlineSystem` quest state
- **UXML**: Horizontal stage indicator with connecting lines
- **Canva assets**: Stage circle (active/completed/locked), checkmark icon

### Expansion 4 Widgets

#### 4. SiegeStatusHUD
- **Shows**: Hatch integrity bar (0-100%), reinforcement tier icon (wood/steel/composite), breach progress %, active tactical effects (gas deployed, gunports active, turret online), "ISSUE COMMAND" buttons
- **Data**: `HatchDefenseSystem.HatchDefenseState`
- **UXML**: Top-of-screen siege bar with command buttons
- **Canva assets**: Hatch icon, shield tier icons (3), explosion icon, gas mask icon

#### 5. FactionIntelligencePanel
- **Shows**: Faction standing bars (-100 to +100), active intel (incoming raids in 24h, convoy schedule), tribute demands, double agent status, alliance status
- **Data**: `FactionIntelligenceSystem` + `FactionPressureWiring`
- **UXML**: Side panel with faction tabs
- **Canva assets**: Faction emblem icons (5), spyglass icon, warning triangle

#### 6. VehicleStatusPanel
- **Shows**: Vehicle condition %, fuel gauge, cargo capacity bar, equipped modifications (winch, ram, solar, medical), breakdown risk indicator
- **Data**: `VehicleSystem.VehicleData` + `VehicleMaintenanceSystem`
- **UXML**: Dashboard-style panel with gauges
- **Canva assets**: Fuel gauge, cargo icon, wrench icon, modification slot icons (5)

#### 7. TacticalCommandBar
- **Shows**: During combat/siege — command buttons (Hold Line, Retreat, Suppressive Fire, Deploy Trap, Flush Airlock), each with cooldown indicator
- **Data**: `HatchDefenseSystem.SiegeTactics` commands
- **UXML**: Bottom-of-screen command bar, 5-6 buttons
- **Canva assets**: Command icons (shield, arrow-back, bullets, explosion, steam)

#### 8. QuestlineStageTracker
- **Shows**: Combined view of all active questlines with stage progress, objective checkboxes, and "DISPATCH EXPEDITION" button for location-based objectives
- **Data**: `QuestlineManager` active quests
- **UXML**: Scrollable quest list with expandable stages
- **Canva assets**: Quest marker icon, objective checkbox (incomplete/complete)

---

## VIII. CANVA ASSET REQUIREMENTS (Both Expansions)

| # | Asset Name | Size | Type | Used By |
|---|-----------|------|------|---------|
| 1 | `icon_skull_danger` | 24×24 | SVG | LocationDetailPanel |
| 2 | `icon_radiation_symbol` | 24×24 | SVG | LocationDetailPanel |
| 3 | `icon_collapse_warning` | 24×24 | SVG | LocationDetailPanel |
| 4 | `faction_icon_garrison` | 32×32 | SVG | LocationDetailPanel, FactionIntelligence |
| 5 | `faction_icon_militia` | 32×32 | SVG | ^ |
| 6 | `faction_icon_cult` | 32×32 | SVG | ^ |
| 7 | `faction_icon_warlord` | 32×32 | SVG | ^ |
| 8 | `faction_icon_scavenger` | 32×32 | SVG | ^ |
| 9 | `icon_biohazard` | 16×16 | SVG | ItemConditionBadge |
| 10 | `icon_expired` | 16×16 | SVG | ItemConditionBadge |
| 11 | `condition_bar_gradient` | 128×4 | PNG | ItemConditionBadge |
| 12 | `quest_stage_circle_active` | 32×32 | SVG | QuestlineProgressTracker |
| 13 | `quest_stage_circle_completed` | 32×32 | SVG | ^ |
| 14 | `quest_stage_circle_locked` | 32×32 | SVG | ^ |
| 15 | `icon_hatch_shield_wood` | 32×32 | SVG | SiegeStatusHUD |
| 16 | `icon_hatch_shield_steel` | 32×32 | SVG | ^ |
| 17 | `icon_hatch_shield_composite` | 32×32 | SVG | ^ |
| 18 | `icon_command_hold_line` | 32×32 | SVG | TacticalCommandBar |
| 19 | `icon_command_retreat` | 32×32 | SVG | ^ |
| 20 | `icon_command_suppressive` | 32×32 | SVG | ^ |
| 21 | `icon_command_trap` | 32×32 | SVG | ^ |
| 22 | `icon_command_flush` | 32×32 | SVG | ^ |
| 23 | `icon_fuel_gauge_bg` | 64×32 | PNG | VehicleStatusPanel |
| 24 | `icon_cargo` | 24×24 | SVG | VehicleStatusPanel |
| 25 | `icon_winch` | 24×24 | SVG | VehicleStatusPanel |
| 26 | `icon_solar_panel` | 24×24 | SVG | VehicleStatusPanel |
| 27 | `icon_spyglass` | 24×24 | SVG | FactionIntelligencePanel |
