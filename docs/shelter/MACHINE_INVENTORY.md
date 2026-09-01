# Plan 29 — Machine Inventory (Phase 0, §3.4)

> Machines that actually exist in runtime code/data. Blueprint-codex-only machines are
> listed in §3 as lore candidates. Verified 2026-09-01.

## 1. Runtime machine roster (player-repeated, condition-relevant)

| # | Machine | Technical name | Room | Runtime owner |
|---|---|---|---|---|
| 1 | HEPA air filtration stack | Air Filtration & HEPA Bay | `room_filtration` | `StartingLevelSystem` |
| 2 | Ventilation exhaust filter + blower/ducts | (no dedicated room; ducts/valves to foundry/generator/kitchen/medical) | shelter-wide | `VentilationSystem` |
| 3 | Water treatment vessel | Filtration/RO/distillation unit | (near water stores) | `WaterTreatmentSystem` |
| 4 | Water pump | Pump Station | `room_water_pump` (power draw only) | `PowerGridSystem` (breaker); pump hardware **unmodeled** |
| 5 | Generator + battery/inverter | Main generation | (no spatial room; `room_main` "Main Vault" only in ShelterSchedule test) | `PowerGridSystem` |
| 6 | Boiler + radiator loop + pipes | Shelter heating | shelter-wide (`ThermalRoomNode`s) | `ShelterThermalSystem` |
| 7 | Silent Foundry facility (cupola) | The Silent Foundry | `room_foundry` | `SilentFoundrySystem` |
| 8 | Airlock door machinery | Airlock Hatch | `room_airlock` | `AirlockSecuritySystem` |
| 9 | Greenhouse environmental equipment | Grow lights etc. | `room_greenhouse` | `GreenhouseSystem` (no condition fields — **unmodeled**) |
| 10 | Radio tuner station | 142.850 MHz Tuner | `room_radio_tuner` | StartingLevel room record only (**narrative**) |

## 2. Save owners (per machine)

| Machine | Save section/store |
|---|---|
| HEPA stack | starting-level section (`airFilterHealthPercent`, `filterSparesCount`, `mechanicalScrapCount`) |
| Ventilation | ventilation section (`exhaustFilterSaturation`, `ductIntegrity`, valves, sources) |
| Water treatment | `water_treatment` (`filterIntegrity`, `filterReplacements`, tanks) |
| Power (gen/battery/pump breaker/lighting) | `power_grid` (`PowerGridSaveStore`) |
| Boiler/pipes | `shelter_thermal` (`ShelterThermalSaveStore`) |
| Foundry | `silent_foundry` (`SilentFoundrySaveStore`) — includes 5 component conditions |
| Airlock | `airlock_security` |
| Gear/weapons (NOT shelter machines) | equipment-condition section (`EquipmentConditionSystem`) |

## 3. Lore-only machines (blueprint codex `room_bp_*` + glitch logs)

Diesel Generator & Alternator Vault, Central Ventilation & Blower Station, Deep Artesian
Well Pump, Brine Distillation & RO Unit, Main DC Battery & Inverter Vault, mercury-arc
rectifier, loom, reloading bench, brewery cellar, etc. — 24 blueprint rooms with
`chief_engineer_note` diagnostic voice, `maintenance_cycle_days`, and
`catastrophic_failure_mode`. These are **canonical texture for nicknames/quirks** but
have no runtime condition. Glitch log codes (`ENG-FL-088-STEAM` etc.) supply the
maintenance-log voice.

⚠ `bunker_maintenance_glitches.json` repair kits reference ~80 item ids that do **not**
exist in `items.json` (see `SHELTER_WEAR_PROVENANCE.md` §5). Before any glitch becomes
mechanically actionable (Task 29B), its kit must be remapped to existing items or the
items added via the normal data pipeline.

## 4. Machine→room binding gaps (Plan 29 §6.1)

- Generator has **no spatial room** in any roster (blueprint lore places it in its own
  vault). Task 29B machine-identity records should assign machines to canonical rooms;
  the generator needs a room decision (likely a non-player-facing service space or the
  filtration/technical end of the corridor).
- Water pump exists only as a power-grid room id; WaterTreatmentSystem does not
  reference it.
- VentilationSystem's "generator" valve implies a generator location that no room id
  captures.
