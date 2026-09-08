# Plan 152 — Vehicle Customization & Mobile Base

## Goal

Transform vehicles from simple transport into customizable mobile bases that serve as expedition headquarters, mobile shelters, and strategic assets. Currently `ExpeditionVehicleSystem.cs` (193 lines) handles vehicle speed, cargo, breakdown, and fuel — but vehicles are purely functional transport with no customization, no mobile shelter capability, and no strategic value beyond getting to destinations. This plan adds vehicle customization, mobile base functionality, and vehicle-based gameplay.

## Why

**Repository evidence:** `ExpeditionVehicleSystem.cs` tracks vehicle profiles (speed multiplier, cargo capacity, breakdown chance, fuel consumption). `ExpeditionVehicleProfile` defines vehicle types. But vehicles have no customization (armor, upgrades, modules), no mobile shelter function (can't sleep in them, can't use as base), and no strategic value beyond transport. The gameplay gaps agent confirmed: "No vehicle-as-home mechanic." No matches for `vehicle.*home`, `mobile.*shelter`, `convoy.*base`, or `caravan.*home` in Core.

**What is missing:** Vehicles are tools, not assets. Players can't customize them, can't live in them, can't use them strategically. A convoy of trucks is just transport — it can't serve as a mobile base for extended expeditions, can't be fortified against raids, can't house survivors overnight.

**Why existing plans don't solve it:** Plan 60 (vehicle expansion) adds more vehicle types but not customization. Plan 10 (combat/expedition depth) adds vehicles to combat but not mobile base mechanics. Plan 133 (expedition consequences) connects discoveries to world state but doesn't add vehicle functionality. No plan addresses vehicle customization or mobile base mechanics.

**Player value:** Creates strategic depth (customize vehicles for specific missions), adds immersion (vehicles feel like real assets), enables new gameplay (mobile expeditions, convoy raids, vehicle-based shelter), and generates emergent stories (a customized convoy becomes a legend).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs` — vehicle logistics
- `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` — expedition mechanics
- `Assets/StreamingAssets/Data/vehicles.json` — vehicle definitions
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` — combat (vehicle combat)
- NEW: `Assets/Ashfall.Core/Vehicles/VehicleCustomizationSystem.cs`
- NEW: `Assets/StreamingAssets/Data/vehicle_modules.json`

## Main Task 1 — Foundation / System Contract

1. Create `VehicleCustomizationSystem.cs` in `Assets/Ashfall.Core/Vehicles/`
2. Define `VehicleModule` DTO: `moduleId`, `moduleType` (armor/cargo/living/weapon/utility), `name`, `effects` (list of modifiers), `cost` (resources), `installationTime` (days), `prerequisites` (list of research/tech)
3. Define `VehicleInstance` DTO: `vehicleId`, `profileId`, `modules` (list of installed module IDs), `condition` (0-100), `fuel` (0-100), `customName` (string), `ownerId` (survivor/faction)
4. Define `VehicleCustomizationState` DTO: list of vehicle instances, list of available modules, list of vehicle blueprints
5. Implement `CaptureState/RestoreState` with schema versioning
6. Define module categories:
   - **Armor**: reinforced hull, spike strips, bulletproof glass (defense bonus)
   - **Cargo**: extended bed, roof rack, trailer (capacity bonus)
   - **Living**: bunk beds, kitchenette, water tank (mobile shelter)
   - **Weapon**: mounted gun, ram, smoke launcher (combat bonus)
   - **Utility**: winch, crane, radio, solar panels (special capabilities)
7. Define vehicle customization rules:
   - Each vehicle has module slots (2-6 depending on size)
   - Modules cost resources and time to install
   - Some modules require research (Plan 141 integration)
   - Modules can be removed/replaced
   - Vehicle condition affects module effectiveness
8. Define mobile base mechanics:
   - Vehicles with living modules can serve as mobile shelter
   - Survivors can sleep in vehicle (restores fatigue)
   - Vehicle can store supplies (extended cargo)
   - Vehicle can serve as expedition base (return point, resupply)
9. Add deterministic seeding: vehicle outcomes use `ISeededRng`
10. Wire into `GameBootstrap`: `SetupVehicleCustomization`, `TickVehicles`, `SaveVehicleCustomization`
11. Create `VehicleModuleCatalogLoader` for module definitions
12. Implement vehicle condition: vehicles degrade with use, need repair
13. Add vehicle fuel: vehicles consume fuel, need refueling
14. Create UI hook: vehicle panel showing customization, condition, fuel

## Main Task 2 — Implementation / Modules / Mobile Base / Combat

1. Implement vehicle customization:
   - Player selects vehicle to customize
   - Choose modules from available list
   - Pay resource cost and wait installation time
   - Module installed, vehicle stats updated
   - Can remove/replace modules (partial refund)
2. Implement armor modules:
   - Reinforced hull: +20% damage resistance
   - Spike strips: damage attackers in melee
   - Bulletproof glass: -50% incoming damage
   - Smoke screen: escape capability
   - Armor adds weight, reduces speed
3. Implement cargo modules:
   - Extended bed: +50% cargo capacity
   - Roof rack: +25% cargo, -10% speed
   - Trailer: +100% cargo, -20% speed, breakdown risk
   - Refrigerated: preserve food longer
4. Implement living modules:
   - Bunk beds: 2-4 survivors can sleep
   - Kitchenette: cook food on expedition
   - Water tank: carry extra water
   - Medical bay: treat injuries on expedition
   - Living modules make vehicle a mobile shelter
5. Implement weapon modules:
   - Mounted gun: vehicle combat bonus
   - Ram: charge enemies, damage structures
   - Smoke launcher: defensive capability
   - Flamethrower: area damage (controversial)
   - Weapons attract attention (hostile encounters)
6. Implement utility modules:
   - Winch: recover stuck vehicles, move obstacles
   - Crane: load/unload heavy cargo
   - Radio: communicate with shelter, call for help
   - Solar panels: generate power, charge devices
   - GPS: improved navigation, reduced getting lost
7. Implement mobile base mechanics:
   - Vehicle with living modules serves as mobile shelter
   - Expedition can establish base camp (vehicle stays, survivors explore)
   - Base camp provides return point, resupply, rest
   - Base camp can be fortified (defense bonus)
   - Base camp can be abandoned (vehicle returns to shelter)
8. Implement vehicle combat:
   - Vehicles with weapons participate in combat
   - Vehicle combat uses different rules (speed, armor, weapons)
   - Vehicle can ram enemies, shoot, or flee
   - Vehicle damage affects condition, can be destroyed
   - Survivors inside vehicle protected by armor
9. Create vehicle events:
   - "The Custom Build" — survivor customizes vehicle for specific mission
   - "The Convoy Raid" — enemies attack vehicle convoy
   - "The Breakdown" — vehicle breaks down mid-expedition
   - "The Mobile Base" — establish base camp in remote location
   - "The Race" — vehicle race for morale and bragging rights
   - "The Rescue" — vehicle winsch rescues stranded survivors
   - "The Upgrade" — research unlocks new vehicle modules
10. Add vehicle quest hooks:
    - "The Dream Machine" — build the ultimate custom vehicle
    - "The Convoy" — escort supply convoy through dangerous territory
    - "The Expedition" — extended mobile base expedition
    - "The Chase" — pursue or flee from hostile vehicles
    - "The Salvage" — recover valuable vehicle from wreck
11. Implement vehicle inheritance:
    - Vehicles persist between expeditions
    - Customizations persist (modules stay installed)
    - Vehicles can be passed to successors (Plan 140 integration)
    - Famous vehicles gain reputation bonuses
12. Add UI: vehicle customization panel with module selection
13. Create vehicle journal: automatic log of vehicle events
14. Implement vehicle tutorial: first customization explains system
15. Create 20 vehicle modules in data file

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `ExpeditionVehicleSystem`: customization affects vehicle stats
2. Connect to `ExpeditionSystem`: mobile base mechanics integrate
3. Integrate with `TacticalCombatSystem`: vehicle combat rules
4. Connect to `ResearchSystem` (Plan 141): research unlocks modules
5. Wire into `ShelterDefenseSystem` (Plan 138): vehicles defend shelter
6. Connect to `NeedsSystem`: living modules restore fatigue
7. Implement old-save compatibility: existing saves get empty customization state
8. Add deterministic seeding: vehicle outcomes use `ISeededRng`
9. Create exploit prevention: modules cost resources, can't be infinite
10. Add tests: customization, mobile base, combat, save round-trip
11. Verify catalog integrity: all module IDs resolve
12. Test edge cases: no modules (stock vehicle), all modules (max customization)
13. Verify headless behavior: customization processes correctly without UI
14. Add data-integrity-selftest: module definitions validate against vehicle/research catalogs
15. Create `--vehicle-customization-selftest` verb for CI validation

## State / System Interaction Model

```text
Vehicle acquired (purchased, found, built)
├─ Customization
│  ├─ Select modules (armor/cargo/living/weapon/utility)
│  ├─ Pay resource cost
│  ├─ Wait installation time
│  └─ Module installed, stats updated
├─ Mobile base (if living modules)
│  ├─ Survivors sleep in vehicle (fatigue restored)
│  ├─ Vehicle stores supplies (extended cargo)
│  ├─ Base camp established (expedition return point)
│  └─ Base camp fortified (defense bonus)
├─ Vehicle combat
│  ├─ Weapons participate in combat
│  ├─ Armor protects survivors
│  ├─ Vehicle can ram, shoot, flee
│  └─ Damage affects condition
├─ Vehicle maintenance
│  ├─ Condition degrades with use
│  ├─ Fuel consumed during travel
│  ├─ Repairs restore condition
│  └─ Refueling at settlements
└─ Vehicle legacy
   ├─ Customizations persist
   ├─ Vehicles pass to successors
   └─ Famous vehicles gain reputation
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --vehicle-customization-selftest
```

## Risk

**MEDIUM** — Vehicle customization complexity can overwhelm players if too many modules exist. Risk of vehicles becoming overpowered with all modules. Mitigation: limit module slots (2-6), make modules expensive, add weight/speed trade-offs, and require research for advanced modules.

## Definition of Done

- `VehicleCustomizationSystem.cs` exists with full `CaptureState/RestoreState`
- 5 module categories implemented (armor, cargo, living, weapon, utility)
- Vehicle customization functional (install/remove/replace modules)
- Mobile base mechanics working (sleep, store, base camp)
- Vehicle combat integrated
- Vehicle maintenance (condition, fuel, repair)
- Vehicle events and quest hooks
- Save/load round-trip tested
- Deterministic vehicle outcomes verified
- Old saves load without error
- 20 vehicle modules in data authority
- UI panel shows vehicle customization
- Cross-system integration (expedition, combat, research, shelter defense, needs)

## Follow-On Opportunities

- Vehicle racing (competitions for morale)
- Vehicle trading (buy/sell customized vehicles)
- Vehicle specialization (survivors become mechanics)
- Vehicle legacy (famous vehicles remembered in epilogue)
- Vehicle mutations (radiation-exposed vehicles gain quirks)
