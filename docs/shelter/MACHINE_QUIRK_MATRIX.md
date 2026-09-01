# Plan 29 — Machine Quirk Matrix (Task 29B, §29B.5–§29B.23)

> Generated from `Assets/StreamingAssets/Data/shelter_machine_identities.json` on
> 2026-09-01. **7 machines, 20 quirks (12 diagnostic + 8 personality),
> 11 glitch events (6 harmless once + 5 real-fault continuous with cooldown).**
> Every diagnostic threshold equals a threshold the owning system acts on
> (pinned by `ShelterMachineTellTests`, which cross-checks against real system instances).

## 1. Machine roster

| Machine id | Name / nickname | Room | Condition owner | Condition key |
|---|---|---|---|---|
| `machine_hepa_stack` | "The Lung" — HEPA Filtration Stack | `room_filtration` | StartingLevelSystem.airFilterHealthPercent (+ radon, weather, duty) | `hepa.filter_health` |
| `machine_foundry_cupola` | The Silent Foundry | `room_foundry` | SilentFoundrySystem 5 facility components | `foundry.average_condition` |
| `machine_generator` | Main Generator & Battery Bank | `room_main` | PowerGridSystem (FuelUnits / BatteryReserveWh / IsBrownout) | `power.fuel_units` |
| `machine_ventilation_plant` | Exhaust Plant & Ducting | `room_filtration` | VentilationSystem (exhaustFilterSaturation / ductIntegrity) | `ventilation.filter_saturation` |
| `machine_water_still` | "The Still" — Brine Still & Filter Bank | `room_water_pump` | WaterTreatmentSystem (filterIntegrity / ReplaceFilter) | `water.filter_integrity` |
| `machine_boiler` | Shelter Boiler | `room_main` | ShelterThermalSystem (boilerFuelLevel / boilerActive) | `thermal.boiler_fuel` |
| `machine_airlock_machinery` | Airlock Machinery | `room_airlock` | AirlockSecuritySystem (door state / pending incidents) | `airlock.incident_active` |

**Generator/boiler room binding (§7.3):** `machine_generator` and `machine_boiler` both
bind to `room_main` ("Main Vault"), the power/thermal vault documented in
`duty_roster_locations.json` ("main_vault") and `room_bp_01` (pre-war engineering record).

## 2. Condition bands (projection only — owner keeps the state)

| Band | Range (condition 0–100) | HEPA | Foundry | Generator fuel | Generator battery | Ventilation saturation | Water filter | Boiler fuel | Airlock |
|---|---|---|---|---|---|---|---|---|
| Healthy | ≥ 70 | owner quiet | owner has no safety warnings | owner full generation | owner >10% reserve | media clear | membranes healthy | tank full | no incident |
| Worn | 50–69 | wearing; no owner warning | some floors near | partial generation risk | load warning | loaded | nearly spent | low | — |
| Service Due | < 50 | **owner warns** | below owner floors | — | — | — | — | — | — |
| Critical | < 25 | radon inflow active | owner critical floors | — | brownout imminent | — | — | — | — |
| Failed | 0 | air-quality floor | plant damage floor | tank empty | reserve empty | — | — | cut out | incident active |

**Note:** fuel/battery quantities are absolute values mapped to the 0–100 display band;
the tell thresholds match the owner's actual degradation points.

## 3. Quirk roster (20 records: 12 diagnostic + 8 personality)

| Quirk id | Machine | Kind | Trigger | Comparison | Text cue | Severity | Maintenance action |
|---|---|---|---|---|---|---|---|
| `machine_quirk_hepa_intake_whistle` | `machine_hepa_stack` | diagnostic | hepa.filter_health < 50 | — | "The intake whistles on the draw — the filters are loaded." | warning | Service (ServiceAirFilter) or replace the HEPA cartridge (ReplaceAirFilter). The owner raises its air-hazard warning at exactly this floor. |
| `machine_quirk_hepa_storm_cough` | `machine_hepa_stack` | diagnostic | hepa.filter_health < 70 | — | "The intake coughs dust after the storm — the filters are eating ashfall." | warning | Service or replace the cartridge; hazard weather doubles the clog rate in the owner. |
| `machine_quirk_hepa_housing_tick` | `machine_hepa_stack` | personality | — (stable) | — | "The housings tick as they cool after the evening watch. Always twice." | info | none |
| `machine_quirk_foundry_tuyere_knock` | `machine_foundry_cupola` | diagnostic | foundry.hearth_tuyeres < 35 | — | "The tuyeres knock in threes before a pour — hearth brick is going." | warning | Rebuild the hearth tuyeres (StartRepair(HearthTuyeres), firebrick). The owner warns of breakout risk below 35. |
| `machine_quirk_foundry_exhaust_whine` | `machine_foundry_cupola` | diagnostic | foundry.safety_exhaust < 30 | — | "The stack whines on the up-draft. The exhaust is losing pull." | warning | Repair the safety exhaust (StartRepair(SafetyExhaust)). The owner warns fumes concentrate below 30. |
| `machine_quirk_generator_fuel_cough` | `machine_generator` | diagnostic | power.fuel_units < 20 | — | "The generator coughs twice before it catches — the tank is under a day." | warning | Add fuel (PowerGridSystem.AddFuel); partial generation halves the bus when the tank runs dry. |
| `machine_quirk_generator_battery_dip` | `machine_generator` | diagnostic | power.battery_reserve < 10 | — | "The lights dip when heavy plant starts. The reserve is thin." | warning | Rebalance load and priorities or add fuel (PowerGridSystem). Brownout trips follow a thin reserve. |
| `machine_quirk_ventilation_loaded_rattle` | `machine_ventilation_plant` | diagnostic | ventilation.filter_saturation above 80 | — | "The exhaust leg rattles as the differential builds — the media is loaded." | warning | Service or replace the exhaust filter (VentilationSystem.ServiceFilter / ReplaceFilter); the owner logs smoke hazard above 60. |
| `machine_quirk_water_ro_choke` | `machine_water_still` | diagnostic | water.filter_integrity < 40 | — | "The still runs long and draws slow — the membranes are nearly spent." | warning | Replace the water filter (WaterTreatmentSystem.ReplaceFilter); the owner blocks reverse osmosis when integrity cannot cover the draw. |
| `machine_quirk_boiler_cutout` | `machine_boiler` | diagnostic | thermal.boiler_fuel < 15 | — | "The boiler sputters near the bottom of its tank — it will cut out soon." | warning | Fuel and restart the boiler (SetBoilerActive); the owner shuts it down dry at zero. |
| `machine_quirk_airlock_seal_drag` | `machine_airlock_machinery` | diagnostic | airlock.incident_active above 0.5 | — | "The door drags on its seal mid-cycle — the owner has a pending incident." | warning | Resolve the pending incident through the owner and fit a new gasket. |
| `machine_quirk_boiler_jacket_tick` | `machine_boiler` | personality | — (stable) | — | "The jacket ticks as the fire settles. It has done that since it was a range." | info | none |
| `machine_quirk_hepa_radon_hum` | `machine_hepa_stack` | diagnostic | hepa.radon_bqm3 above 50 | — | "The intake hum changes pitch when the radon spikes — the stack is pulling harder against a heavier mix." | warning | Increase filter duty cycles (StartingLevelSystem); the owner warns at this radon floor. |
| `machine_quirk_foundry_heat_shimmer` | `machine_foundry_cupola` | personality | — (stable) | — | "The air above the cupola shimmers even when the furnace is cold. It is the residual heat leaving the lining, and it takes longer than it should." | info | none |
| `machine_quirk_foundry_vibration_tune` | `machine_foundry_cupola` | personality | — (stable) | — | "The floor vibration has a pitch. It is lower when the tuyeres are good and rises when the brick goes. The crew feel it before they hear it." | info | none |
| `machine_quirk_generator_brownout_flicker` | `machine_generator` | diagnostic | power.battery_reserve < 5 | — | "The panel flickers on the brownout leg. The generator is running but the bus is not clean." | warning | Balance load and re-seat breakers (PowerGridSystem); the owner logs brownout below this reserve. |
| `machine_quirk_generator_vibration_tick` | `machine_generator` | personality | — (stable) | — | "The generator ticks on its mount at idle — a metronome that nobody tuned because it has always been there." | info | none |
| `machine_quirk_ventilation_soot_smell` | `machine_ventilation_plant` | diagnostic | ventilation.filter_saturation above 60 | — | "The ventilation leg carries a soot smell before the gauge moves. The filter is loading and the exhaust is not clearing." | warning | Service or replace the exhaust filter and clear the duct (VentilationSystem). |
| `machine_quirk_water_distillation_hum` | `machine_water_still` | personality | — (stable) | — | "The still hums at a frequency that is not in the catalogue. It changes with the water chemistry and the crew have stopped noticing it." | info | none |
| `machine_quirk_airlock_machinery_grind` | `machine_airlock_machinery` | personality | — (stable) | — | "The airlock machinery grinds once on the up-stroke and seats clean. It is the seal bedding in, and it has done it every day since the hatch was last opened." | info | none |

**Threshold provenance (§1.5):
- HEPA 50 = owner's `airHazardWarning` floor; 70 = storm-cough band (+4/day degrade); radon 50 = owner radon warning floor
- Foundry 35/30/25 = `SilentFoundrySystem.GetSafetyWarnings()` exact floors
- Generator 20 = `fuelNeed ≈ 19.2/day` at default 800W; owner halves generation below this; battery 5 = brownout imminent
- Ventilation 60 = soot-smell pre-warning (projection); 80 = saturation loaded band
- Water 40 = pre-warning (owner blocks RO when integrity < draw-dependent need)
- Boiler 15 = pre-warning (owner cuts dry at 0; 15 fuel ≈ 30-day lead time at 0.5/day)
- Airlock 0.5 = owner's `HasPendingIncident` threshold

## 4. Glitch events (11 records: 6 harmless once + 5 real-fault continuous)

| Glitch id | Machine | Kind | Condition | Trigger | Repeat | Cooldown | Title |
|---|---|---|---|---|---|---|---|
| `glitch_21_phantom_draft` | `machine_airlock_machinery` | harmless | — | none | once | 0d | The Draft That Isn't |
| `glitch_22_repeating_relay_click` | `machine_generator` | harmless | — | none | once | 0d | The Relay That Counts Twice |
| `glitch_23_old_intercom_burst` | `machine_airlock_machinery` | harmless | — | none | once | 0d | The Old Intercom Speaks |
| `glitch_24_seal_cycles` | `machine_airlock_machinery` | real_fault | `airlock.incident_active` | above 0.5 | continuous | 7d | The Seal Cycles Itself |
| `glitch_25_ground_loop` | `machine_generator` | real_fault | `power.battery_reserve` | below 10 | continuous | 3d | The Ground Loop, Again |
| `glitch_26_stuck_damper` | `machine_ventilation_plant` | real_fault | `ventilation.filter_saturation` | above 80 | continuous | 5d | Damper Off Its Seat |
| `glitch_27_pressure_flutter` | `machine_water_still` | real_fault | `water.filter_integrity` | below 30 | continuous | 5d | Flutter At The Still |
| `glitch_28_boiler_cutout` | `machine_boiler` | real_fault | `thermal.boiler_fuel` | below 10 | continuous | 2d | The Boiler Cuts Out |
| `glitch_29_boiler_sigh` | `machine_boiler` | harmless | — | none | once | 0d | The Boiler Sighs |
| `glitch_30_generator_hum_drop` | `machine_generator` | harmless | — | none | once | 0d | The Hum Drops |
| `glitch_31_water_still_gurgle` | `machine_water_still` | harmless | — | none | once | 0d | The Still Gurgles |

**One-shot persistence:** harmless glitches journal via `KnowledgeKeys.GlitchNoted(id)` +
`JournalSystem.UnlockGlitchNoted/IsGlitchNoted`. Old saves default un-noted and reveal
once; the journal key makes it idempotent.

**Real-fault kits (all existing items, no new ids):
- `glitch_24`: `item_hermetic_hatch_silicone_gasket`
- `glitch_25`: `copper_wire_10m_of_10m`, `scrap_electronic`
- `glitch_26`: `filter_pack`, `scrap_metal`
- `glitch_27`: `water_filter`
- `glitch_28`: `charcoal`

## 5. Context gates (§29B.8)

`MachineConditionReadings` carries `HazardWeather` from the real `WeatherKind.Current`.
Contextual quirks gate on it. **No hidden random conditions.** The HEPA storm-cough
(`hazard_weather`) already maps to real mechanics (+4/day HEPA degrade in the owner).

## 6. Styling rules (§29B.9)

| Class | Records | Styling rule (enforced by `Validate()`) |
|---|---|---|
| diagnostic | 12 records | severity warning/critical; must name a real maintenance action |
| personality | 8 records (housing_tick, jacket_tick, heat_shimmer, vibration_tune, brownout_flicker, soot_smell, distillation_hum, airlock_grind) | severity **info** only; must not bind a condition; must not gate on context |
| harmless glitch | 6 records | no condition gate; no severity; never styled as service warning |
| real-fault glitch | 5 records | condition gate + resolution text + kit list; cooldown ≥ 0 |

## 7. Panel surfacing (§29B.16–29B.18)

- **GameDashboardPanel**: new `_machineTellLabel` row below filter spares. Color-coded:
  - `Critical`/`FAULT` → `DesignTheme.Critical`
  - `WORN`/`RATTLE`/`CHOKE`/`COUGH` → `DesignTheme.Entropy`
  - `NOMINAL` → `DesignTheme.Pale`
- **SilentFoundryPanel**: tell line reserved for Phase 4 extension (foundry quirks evaluated
  against live `SilentFoundrySystem` state; wiring follows the same pattern as the dashboard).

## 8. Host daily pass (§29B.12–29B.13)

`Main.TickMachineGlitchEvents(day)` runs from `TickShelterRoomHistoryMilestones`:
- Evaluates all glitch events against live `MachineConditionReadings`
- Journals one-shots via `JournalSystem.UnlockGlitchNoted(id)`
- Continuous events re-fire every day (cooldown is caller-paced; host logs/tick owns it)

## 9. What Phase 8 deliberately does NOT do

- No new persisted condition anywhere (tells are projections of existing system state).
- No new save section (glitch one-shots live in journal knowledge keys; continuous state
  is session-scoped in the host — wear store deferred to Phase 5 per §29C.4).
- No nickname for the foundry (canon name is already the survivor name).
- No machine-panel scene changes beyond the dashboard tell row (SilentFoundryPanel wiring
  follows in Phase 4 extension).
