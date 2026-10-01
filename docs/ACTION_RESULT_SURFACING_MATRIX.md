# Typed action-result surfacing matrix

Core owns stable failure codes; host/UI owns player-facing wording.

| Domain | Representative typed codes | Projection surface |
|---|---|---|
| Expedition | `missing_fuel`, `vehicle_unready`, `vehicle.refuel_failed`, `vehicle.repair_failed`, `missing_kit`, `unmapped`, `crossing_closed`, `route_blocked`, `unknown_target`, `stale_preview` | garage/expedition session and expedition panel (per-card block reason, disabled-button tooltip, and dispatch-refusal status line) |
| Shelter | `maintenance_dependencies_missing`, `research_required`, `maintenance_not_needed`, `missing_maintenance_item` | starting-level HUD and shelter maintenance surface |
| Medical | `missing_medicine`, `research_required`, `patient_unavailable`, `unknown_procedure`, `stale_preview`, `reservation_failed`, `treatment_rejected`, `clinic_no_power` | medical panel, journal, and medical host session |
| Economy | existing trade-session typed rejection/stance/fairness results; plain `MarketSystem` returns `NaN` for an unknown good | economy/trade presenters |
| Craft | `missing_recipe`, `unknown_recipe`, `research_locked`, `blueprint_locked`, `station_unavailable`, `insufficient_ingredients`, `inventory_full`, `result_restricted`, `moonshine_restricted`, `execute_failed`, `stale_preview` | crafting panel and workstation bench (pre-validated reason label plus runtime `REFUSED — <reason>` line) |
| Cook (kitchen) | `cook_fitness_blocked`, `fitness_blocked`, `job_active`, `insufficient_ingredients`, `no_job`, `no_meal`, `no_survivors`, `insufficient_portions` | kitchen nutrition panel (`Prep blocked: <code>` / `Cannot serve: <code>`) |
| Duty roster | `unknown_role`, `ineligible_underage`, `unknown_survivor`, `cannot_assign`, `fitness_blocked`, `fitness_warning_confirmation_required`, `duty_refused:<reason>`, `busy`, `already_assigned` | duty roster panel (fitness dialog, `Assignment blocked: <code>`, commit-failure line) |
| Expedition (vehicle prep) | `vehicle_unready`, `inventory_full`, `missing_kit`, `unknown_vehicle`, `stale_preview` (via typed `CommandResult`) | expedition panel `_dispatchStatusLabel` (`REFUEL REFUSED` / `TRACK GEAR REFUSED` line) |
| Archive desk | `already_unlocked`, `busy`, `insufficient_ink`, `no_job` | archive desk panel event-log label (host `LastEvent`) |
| Contractor roster | `insufficient_funds`, `not_active`, `not_available`, `offer_exists` | contractor roster panel event-log label (host `LastEvent`) |
| Apprenticeship | `chapter_not_open`, `unknown_child`, `mentor_unavailable`, `not_acceptable`, `busy` | apprenticeship panel `Last Event:` line (`LastEvent`) |
| Decontamination | `empty_queue`, `no_water`, `no_soap`, `no_chelator`, `no_filter`, `filter_installed`, `filter_exhausted`, `active_case`, `already_queued`, `not_in_progress`, `survivor_busy`, `unknown_protocol`, `invalid_gear`, `gear_not_found`, `no_active` | decontamination panel event-log label (host `LastEvent`) |
| Defense (perimeter) | `no_perimeter`, `sector_not_found`, `alarm_already_spent`, `alarm_not_spent` | defense grid panel detail (`LastEvent`) |
| Equipment maintenance | `item_exists`, `missing_part`, `missing_scrap` | equipment condition panel event-log label (`LastEvent`) |
| Shelter scheduling | `not_allowed` | shelter schedule panel `Last Event:` line (`LastEvent`) |
| Thermal / boiler | `already_burning`, `already_burst`, `not_burst`, `insufficient_fuel`, `insufficient_materials`, `no_blowtorch`, `no_radiator`, `room_exists`, `pipe_exists`, `not_frozen`, `stove_already_exists`, `storm_in_progress` | shelter thermal panel detail + frostbite card (`LastEvent`) |
| Relations (mediation) | `already_resolved`, `unknown_conflict` | survivor relations panel `Last Event:` line (`LastEvent`) |
| Morale (vinyl) | `not_owned`, `not_playing` | vinyl morale panel `Last Event:` line (`LastEvent`) |
| Sump flooding | `media_worn`, `no_cake`, `node_exists`, `no_pump`, `pump_exists`, `no_sludge`, `no_solids`, `no_tailings` | sump flooding panel event-log label (`LastEvent`) |
| Wildlife trapping | `no_catalog`, `no_inventory`, `unknown_trap`, `trap_active`, `insufficient_materials`, `commit_failed`, `repair_unavailable` | wildlife trapping panel detail (`Last Event:` line, host `LastEvent`; `CheckTraps` refusal) |
| Airlock security | `door_breached`, `incident_active`, `invalid_amount`, `invalid_decision`, `invalid_door_state`, `invalid_visitor`, `no_incident` | airlock security panel `Last Event:` line (`LastEvent`) |
| Pharma lab | `lab_busy`, `missing_inputs`, `not_processing`, `unbound` | pharma lab panel refusal label (`COMPOUND REFUSED — <prose>` / `ABORT REFUSED — <prose>`, shared `ActionRefusalText`) |
| Workshop / relic restoration | `cannot_start`, `missing_materials`, `transaction_failed`, `inventory_full`, `not_active`, `not_ready`, `workshop_busy`, `workshop_idle`, `already_complete`, `already_salvaged`, `missing_components`, `missing_source_item`, `no_research_unlock` | workshop panel refusal line at the top of the detail column (`START REFUSED`, `COLLECT REFUSED`, `ABORT REFUSED`, `OVERHAUL REFUSED`, `RESTORATION REFUSED`; shared `ActionRefusalText`) |
| Weather / cloud seeding | `already_installed`, `cloud_seeding.not_installed`, `cloud_seeding.cooldown_active`, `cloud_seeding.research_locked`, `cloud_seeding.insufficient_materials`, `cloud_seeding.illegal_weather_target` | weather forecast panel feedback line (`SEEDING REFUSED` / `INSTALL REFUSED`; shared `ActionRefusalText`) |

Every new resource-backed path validates before mutation. Shelter service,
scheduled medical reservation/consumption, expedition launch, and existing
trade transactions use the owning inventory/ledger authority. The UI does not
derive a capability from a string or calculate a replacement price.

## Host `LastEvent` convention (T10)

Host sessions that expose a <c>LastEvent</c> string and bind a panel that
renders it **must** assign `LastEvent` on the failure branch as well as the
success branch. A host that assigns it only inside `if (result.IsSuccess)`
leaves the previous success sentence on screen after a refusal, which reads as
a silent no-op to the player. The panels the sweep pinned:

archive desk, contractor roster, apprenticeship, decontamination, defense,
equipment condition, shelter scheduling, thermal/boiler, relations, morale,
sump flooding, wildlife trapping (incl. `CheckTraps`), airlock security, radio
program production, amphibious draisine, CVD diamond reactor, solid-oxide fuel
cell, acoustic sound-ranging, and low-background metrology.

The last five host sessions already produced a failure `LastEvent`, but their
panels did not render it; the sweep added the `Last event:` line to their detail
column (and the low-background metrology string commands now publish
`LastEvent` before `RaiseStateChanged`).

## Direct-system panels (T10)

When a panel binds a Core system directly (no host session / no `LastEvent`
seam), it captures the returned `ActionResult` and renders the refusal itself
through the shared `AtomicWar.GodotApp.UI.ActionRefusalText` formatter
(`WorkshopPanel`, `PharmaLabPanel`, `WeatherForecastPanel`). Unknown failure
codes fall back to a de-underscored readable form, so no refusal is dropped.
