# Plans B74–B77 Authority Map

**Status:** Reconnaissance complete
**Date:** 2026-09-06
**Scope:** Geothermal ORC, armory calibration, aeroponic root ecology, and pneumatic dispatch

## 1. Current-source finding

Plans B74–B77 are new work in this checkout. No production Core system,
authoritative catalog, save section, host session, or player-facing panel exists
for these four plans.

The repository does contain older, unrelated narrative catalogs named for
geothermal steam and pneumatic equipment, plus an isolated geothermal aquifer
system. Those files are not the authority for the new gameplay systems:

- `Assets/Ashfall.Core/Narrative/SteamTurbinePowerCatalog.cs` is codex-style
  historical content.
- `Assets/Ashfall.Core/Narrative/PneumaticTubeDispatchCatalog.cs` is codex-style
  historical content.
- `Assets/Ashfall.Core/Shelter/GeothermalAquiferSystem.cs` owns drilling,
  strata crossing, casing, aquifer tapping, pressure, and the existing
  geothermal save state. It does not currently feed the canonical grid,
  thermal system, or water treatment.

B74–B77 therefore extend existing authorities through typed handoffs rather
than replacing them or relabeling the older catalogs.

## 2. Authority matrix

| Concern | Existing authority | B74–B77 responsibility | Boundary |
|---|---|---|---|
| Electrical generation, breaker state, load, battery, brownout | `PowerGridSystem` in `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` | B74 publishes a bounded generation contribution; B76 and B77 query delivered power through the grid | No second battery, breaker, or power simulation |
| Deep excavation and strata access | `ExcavationSystem`, `ExcavationHazardSystem`, and existing `GeothermalAquiferSystem` | B74 requires a commissioned well/stratum ID and consumes existing excavation readiness supplied by the host | No new room/collapse authority |
| Room identity and room power | `PowerGridRoom` and shelter room catalogs | New systems store stable room IDs and ask `PowerGridSystem.IsRoomPowered` | No private room registry |
| Shelter heating and condenser heat | `ShelterThermalSystem` in `Assets/Ashfall.Core/ShelterThermalSystem.cs` | B74 exposes recoverable condenser heat; host calls `SetGeneratorWasteHeat` once per day | B74 never edits room temperatures |
| Water treatment | `WaterTreatmentSystem` | B74 exposes optional water/heat capacity; host allocates through existing water APIs | No duplicate clean-water tank |
| Equipment condition | `EquipmentConditionSystem` | B75 references a stable equipment instance and applies service wear/repair through the existing system | No second weapon durability percentage |
| Combat resolution | `TacticalCombatSystem` and `WeaponConditionSystem` | B75 returns a bounded `WeaponCombatModifier` projection consumed by combat adapters | B75 does not resolve hits, damage, or ammo physics |
| Weapon identity | `EquipmentInstance.instanceId` and `WeaponEquipmentBridge` | B75 profile key is the canonical equipment instance ID | Missing profile derives a safe baseline; no random load initialization |
| Crop lifecycle and food inventory | `GreenhouseSystem`, `Inventory`, `KitchenNutritionSystem` | B76 owns chamber environment and cycle state; harvest grants canonical item IDs through `Inventory` | No duplicate greenhouse plot or food store |
| Pharmaceutical production | `PharmaLabSystem` nested in `CraftingSystem` | B76 outputs canonical medicinal-root ingredients that PharmaLab may consume | No `pharma_lab` save section |
| Item mass and atomic resource consumption | `Ashfall.Core.Inventory.Inventory` | B74/B75/B76/B77 use inventory bills/transactions for authored costs and outputs | No private maintenance stock |
| Assignments and specialist traits | `ShelterAssignmentSystem`, survivor/skill authorities, `DutyRosterSystem` | Hosts provide worker/trait modifiers to Core actions | B74–B77 do not create another roster |
| Room-to-room transfer | No current generic capsule transport authority | B77 owns only the in-transit capsule queue and uses a canonical inventory endpoint adapter | Source removal occurs on accepted dispatch; delivery is exactly once |
| Save envelope | `SaveSectionRegistry`, `SaveStoreHub`, `CampaignEnvelopeBuilder` | New sections use the existing registry/atomic campaign envelope pattern | Legacy missing sections restore safe defaults |
| Presentation | Godot `src/UI`, `src/Host` | Panels bind typed host sessions and forward commands | Core emits semantic events, never Godot/audio calls |

## 3. Required seam changes

### 3.1 Power contribution

`PowerGridSystem` currently exposes mutable base `GenerationWatts` but has no
source contribution API. B74 will add a deterministic, keyed contribution map
whose sum is included in the existing generation query. The base generator
remains intact, and removing/restoring a B74 source is idempotent by source ID.
The host, not the geothermal Core system, owns the call that publishes the
current output.

### 3.2 Waste heat

`ShelterThermalSystem.SetGeneratorWasteHeat(float, bool)` already provides the
correct semantic seam. B74 will expose condenser heat and the host will pass
the allocated value once before the thermal day tick. No second thermal room
state will be added.

### 3.3 Equipment/combat

The existing `WeaponEquipmentBridge` already converts the one canonical
equipment condition into combat state. B75 adds a per-instance calibration
profile and a pure modifier projection. Combat adapters may combine that
projection with the existing stance/condition modifiers exactly once.

### 3.4 Agriculture and downstream production

Greenhouse remains the baseline soil/plot authority. B76 owns only aeroponic
chambers and their living root cycles. A completed harvest calls the shared
inventory output path; Kitchen and Pharma remain consumers of those item IDs.

### 3.5 Pneumatic inventory endpoints

The existing `Inventory` is a single shelter container rather than a
location-aware multi-container store. B77 will use an explicit endpoint
adapter with source/destination inventory references. Dispatch removes cargo
atomically from the source; arrival grants it to the destination exactly once.
The capsule state stores the cargo ledger needed for reload conservation.

## 4. Data and identity rules

New authoritative catalogs live in `Assets/StreamingAssets/Data/`:

- `geothermal_strata_catalog.json`
- `ballistics_workbench_catalog.json`
- `aeroponics_nutrient_catalog.json`
- `pneumatic_network_catalog.json`

Every runtime entity has a stable ID. All persisted values are campaign-state
values, not wall-clock timestamps. Milestone randomness uses `ISeededRng` and
is consumed only by explicit operating, firing, environmental, or dispatch
transitions. Restore never rerolls a resolved leak, failure, disease, jam,
harvest, dispatch, or delivery.

## 5. Known plan divergences

1. Existing `GeothermalAquiferSystem` and `ShelterThermalSystem` are retained.
   B74 is an ORC/loop integration layer and does not rewrite their drilling or
   room-temperature logic.
2. `WaterTreatmentSystem` stores water internally while greenhouse watering
   still uses item inventory. B76 will not silently merge those models; its
   chamber water input is an explicit adapter seam.
3. `DutyRosterSystem` is a narrative chart/assignment authority, not a generic
   labor scheduler. Specialist bonuses are injected as bounded modifiers
   rather than adding private worker assignments.
4. There is no existing generic room inventory graph. B77 therefore introduces
   a narrow endpoint contract, not a second global inventory authority.
5. The current worktree has extensive unrelated uncommitted changes. No
   existing changes are reset, reformatted, or folded into this wave.

## 6. Verification targets

The wave must prove:

- catalog load and content-utilization registration;
- Core engine purity and deterministic milestone behavior;
- exact capture/restore with absent-section defaults;
- canonical power contribution and waste-heat handoff;
- equipment-instance modifier projection without duplicate wear;
- canonical harvest inventory output;
- pneumatic source/in-transit/destination conservation;
- focused tests, full xUnit, data integrity, content utilization, and Godot
  host build/headless checks.
