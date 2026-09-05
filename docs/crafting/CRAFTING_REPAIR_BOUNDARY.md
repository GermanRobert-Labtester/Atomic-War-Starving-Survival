# Crafting / Repair Boundary (Plan 55)

## Rule

Crafting is a **conversion system**. Wherever a mutable-condition entity or
installed infrastructure exists, crafting may produce *components, consumables,
or replacement parts* — never a repaired/pristine entity that bypasses the
condition authority.

## Audit of repair-adjacent concepts

| Concept | Condition authority | Plan 55 disposition |
|---|---|---|
| Weapon condition | `EquipmentConditionSystem` → projected into combat via `WeaponEquipmentBridge` | untouched; reload recipes produce ammunition, not condition |
| Tool overhaul | `ShelterWorkshopSystem` (ToolOverhaul jobs consume `machine_oil` + `mechanical_parts` bills directly; machine ToolingHealth/Calibration) | no "tool repair kit" item authored — it would duplicate the direct bill and bypass `requiresTools` semantics |
| Generator / shelter infrastructure | Shelter room builds + `SkyLayerArmorCatalog` consume raw material bills directly | no service-kit items authored — no consumer; avoids double-charging (§55D.10) |
| Frozen pipes | `thaw_frozen_pipe` zero-result sink consumes `item_thermal_paste` + `fuel_1l` + `item_epoxy_injector` | untouched |
| Vehicles | `ExpeditionVehicleSystem.Repair(vehicleId, amount)` — takes a float; host consumes no items | **no vehicle-component recipes**; per §55D.12 substitution rule, the mechanical niche remains with `craft_engine` (existing) and shelter mechanical material sinks |
| Crafting station wear | `CraftingStation.Degrade/Repair` inside `CraftingSystem` (5f/craft; refund-on-overflow repairs) | untouched |

## Why no vehicle recipes (Risk 21)

The vehicle repair path exposes no component-consumption concept: repair is a
bare condition float mutation with inventory consumption left entirely to the
caller, and no caller consumes items today. Authoring gaskets/patch kits/fuel
filters would create outputs with no consumer (orphan outputs, §6.4) or would
require a Plan-55-only vehicle-repair framework (forbidden, §12). Documented
as deferred work: "vehicle-specific service kits after vehicle repair
consumption is mature" (§16).
