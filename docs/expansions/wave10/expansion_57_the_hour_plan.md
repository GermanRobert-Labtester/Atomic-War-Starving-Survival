# ASHFALL — Expansion 57 Design Bible
# THE HOUR
### Wave 10 · Clocks, Bells, Water Clocks, Time Signals, Drift, Standard Time, and the Shelter's Shared Hour

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-24
**Domain owners touched:** `Ashfall.Core` (`ISimClock`, `IWallClock`, `DayRecord`), `Ashfall.Core.Shelter` (`ShelterScheduleSystem`), `Ashfall.Core.DutyRoster` (shifts), `Ashfall.Core.Narrative` (`TimekeepingHorologyCatalog`)
**Proposed host owner:** `TimeOfficeHostSession` (extends the shelter schedule and crafting surfaces)
**Existing save sections:** `shelter_schedule` (curfew, beds, schedule state), `duty_roster` (posts, shifts)
**Existing CLI verbs:** `--data-integrity-selftest`, `--content-utilization-selftest`, `--panel-bind-lifecycle-selftest` (no timekeeping-specific verb exists)
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already runs on time without ever asking what time it is. `ISimClock`
and `IWallClock` define the ticking contract; `DayRecord` counts the days;
`ShelterScheduleSystem` (`ShelterScheduleHostSession` with `SetCurfew`,
`SetEmergencyOverride`, `AssignBed`, `TickDay`, `TickHour`, `LoadCatalog`,
`Save`) assigns beds, curfews, and the daily shape of the shelter; the duty
roster divides the twenty-four hours into shifts. The rendering layer has
clocks in the interface, and everyone who has ever played a shift game knows
the hour is the skeleton under the whole simulation.

What does not exist: an hour that anyone maintains. There are no timepieces in
`items.json`. There is no clock registry, no bell discipline, no water clock
for the powerless weeks, no drift record, no timekeeper, no standard hour that
outposts agree to keep, no chronometer issued to a road crew, no way to answer
the simple question a shelter asks in its fourth winter: **is our clock still
right?**

The evidence sits in the narrative store waiting. Four authored trade logs —
`deadbeat_escapement_wear_logs.json` (5,832 B: `clock_mechanism_id`,
`escapement_type`, `daily_rate_drift_seconds`, `wear_depth_microns`),
`invar_pendulum_thermal_expansion.json` (5,909 B: `pendulum_assembly_id`,
`rod_material_alloy`, `thermal_expansion_coefficient_ppm_k`, `bob_mass_kg`),
`mainspring_fatigue_rupture_audits.json` (5,046 B: `spring_barrel_id`,
`spring_alloy_type`, `full_windup_torque_nm`, `failure_cycle_count`), and
`water_clock_orifice_silt_records.json` (5,226 B: `clepsydra_station_id`,
`orifice_material_type`, `nominal_flow_ml_min`, `viscosity_error_pct`) — are
read by exactly nothing. `TimekeepingHorologyCatalog` models every one of them
(`DeadbeatEscapementWearEntry`, `InvarPendulumThermalEntry`,
`MainspringFatigueRuptureEntry`, `ClepsydraWaterClockEntry`) and is referenced
by zero runtime files.

**The Hour** is the expansion about keeping time: the clocks the shelter owns,
the bells it rings, the drift it measures, the corrections it makes, and the
decision it eventually has to make — that a community which shares a meal and a
watch rotation also shares an hour.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| `ISimClock` / `IWallClock` | The ticking contract | Reads it; never replaces it |
| `ShelterScheduleSystem` | Curfew, beds, day shape | Extends with reliable time |
| 02 The Duty Roster (Exp 02) | Shifts, posts, fatigue | Schedules read time; no second roster |
| 56 The Calendar (Wave 9) | The year, observances | The hour only; no days, no festivals |
| 23 The Alarm (Wave 3) | Alarms, hazard signals | Time signals never sound as alarms |
| 41 The Quiet (Wave 6) | Sleep, noise, rest | Bell discipline respects quiet hours |
| 29 The Glass (Wave 4) | Lenses, metrology, instruments | Time only; no second metrology |
| 40 The Wheel (Wave 6) | Machines, machine maintenance | Clocks are instruments, not machines |
| 21 The Grid (Wave 2) | Power | Electric clocks draw through it |
| 34 The Long Road (Wave 5) | Travel, waystations | Issues chronometers to crews |
| 44 The Outpost (Wave 7) | Outposts | Offers the standard hour; never commands it |
| `NeedsSystem` | Morale, stress | Bells and light use `Modify` only |
| `ShelterWorkshopSystem` | Tool work | Clock bench extends it |
| `EquipmentConditionSystem` | Wear on gear | Timepieces register as tracked equipment |
| `StandingRecord` | Records | Files the drift book |

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The shelter runs on a clock nobody winds. Three timepieces disagree by eleven
minutes, the bell rings a shift late in winter, the water clock in the sump
nook has silted its orifice, and the road crews keep their own hours because
nobody has ever handed them a standard one. When the fourth winter makes the
sun useless for half the day, two hundred people discover that they have been
living in the same minute by luck.

**The Hour** is the expansion about time as infrastructure: the clock bench,
the bell frame, the water clock, the hourglasses, the time office, the standard
hour, the drift book, and the small daily ceremony of setting every clock in
the shelter from one source. It is about the least visible service in the
shelter and the one every other service depends on.

### 1.2 The five loops it adds

```
  Wind ──► Check ──► Correct ──► Ring ──► Record
    │        │          │          │         │
    ▼        ▼          ▼          ▼         ▼
  clocks   drift      noon mark  bells    drift book
  & sand   observed   & signal   & shifts & corrections
                                        │
                                        ▼
                          Issue ──► Keep ──► Teach
```

### 1.3 What the player manages

1. **The clocks.** Wind, set, compare, repair, and retire them.
2. **The drift.** Track each timepiece's error and correct it honestly.
3. **The standards.** The noon mark, the signal lamp, the standard hour.
4. **The bells.** Shift bells, meal bells, curfew, and the quiet rules.
5. **The water clock.** The powerless fallback and its orifice.
6. **The hourglasses.** Short intervals, kitchens, ward, smithy.
7. **The hours.** Who keeps time, and who is relieved.
8. **The chronometers.** Instruments issued to road crews and expeditions.
9. **The records.** The drift book and the corrections it proves.
10. **The outposts.** One hour offered, never imposed.

### 1.4 What it is not

- Not a replacement for `ISimClock`; the tick contract is untouched.
- Not a second schedule system; `ShelterScheduleSystem` keeps its authority.
- Not the calendar; days and observances stay with expansion 56.
- Not an alarm system; time signals never pretend to be hazard warnings.
- Not a sleep regime; bells obey the quiet-hour owners.
- Not a metrology system; distances and masses stay with expansion 29.
- Not a morale fountain; the hour's comforts are authored and bounded.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Clock/ISimClock.cs` | Tick contract | `LIVE` |
| `Assets/Ashfall.Core/IWallClock.cs` | Wall-clock contract | `LIVE` |
| `Assets/Ashfall.Core/Campaign/DayRecord.cs` | Day counting | `LIVE` |
| `Assets/Ashfall.Core/Shelter/ShelterScheduleSystem.cs` | Curfew, beds, day shape | `LIVE` |
| `src/Host/ShelterScheduleHostSession.cs` | Schedule host | `LIVE` |
| `Assets/Ashfall.Core/DutyRoster/*` | Shifts and posts | `LIVE` |
| `Assets/Ashfall.Core/EquipmentConditionSystem.cs` | Wear model | `LIVE` |
| `Assets/Ashfall.Core/Crafting/ShelterWorkshopSystem.cs` | Workshop bench | `LIVE` |
| `NeedsSystem` | Morale sink | `LIVE` |
| `StandingRecord` | Records | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Rows | Notes |
|---|---|---|
| `deadbeat_escapement_wear_logs.json` | 4+ entries, 5,832 B | drift and wear; unread |
| `invar_pendulum_thermal_expansion.json` | 4+ entries, 5,909 B | pendulum rods; unread |
| `mainspring_fatigue_rupture_audits.json` | 4+ entries, 5,046 B | springs; unread |
| `water_clock_orifice_silt_records.json` | 4+ entries, 5,226 B | clepsydras; unread |
| Clock, bell, signal, drift catalogs | **0** | confirmed absent |
| Timepiece items in `items.json` | **0** | confirmed absent |

### 2.3 Confirmed gaps

- **GAP-57-1 — No timepiece exists as an item or entity.**
- **GAP-57-2 — No clock registry, ownership, or winding.**
- **GAP-57-3 — No drift measurement, record, or correction.**
- **GAP-57-4 — No bell discipline or time signals.**
- **GAP-57-5 — No water clock or powerless timekeeping.**
- **GAP-57-6 — No hourglasses or short-interval instruments.**
- **GAP-57-7 — No timekeeper role, hour duty, or relief.**
- **GAP-57-8 — No standard hour shared with outposts or roads.**
- **GAP-57-9 — No chronometer issue for expeditions.**
- **GAP-57-10 — Four authored horology logs are read by nothing.**

### 2.4 Non-duplication statement

This expansion adds **no** second clock contract, schedule, roster, alarm,
metrology, power, morale, or save system. It reads `ISimClock`/`IWallClock`,
extends `ShelterScheduleSystem` with time data, routes shifts through the duty
roster unchanged, keeps signals out of the alarm owner's territory, keeps
physical measurement with expansion 29, and files its records with
`StandingRecord`. All new state is additive inside the existing
`shelter_schedule` save owner or the workshop's condition path. No new save
section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Time is a shared service.** One shelter, one hour, or the shifts
drift apart and the kitchen serves cold food to a watch that already ate.

**Pillar 2 — A clock is a promise you can hear.** The bell is the moment the
promise is kept, and everyone hears whether it was late.

**Pillar 3 — Drift is honest work.** Every clock is wrong; the trade is
knowing by how much and correcting without shame.

**Pillar 4 — The powerless hours still count.** When the grid falls, sand and
water keep time, and the shelter keeps its shape.

**Pillar 5 — The hour is given, not imposed.** Outposts are offered the
standard hour and may refuse it without insult.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Clock bench | Small tools, patient repair | Genius-inventor fantasy |
| Bells | Signals, relief, meals | Command authority |
| Drift | Logged error, correction | Punishment for being wrong |
| Water clock | Silt, orifice, patience | Mystic water ritual |
| Night hours | Quiet discipline | Sleep shaming |
| Chronometers | Issued, signed, returned | Prestige trophies |
| Standard hour | Offered and agreed | Enforced uniformity |
| Records | Ledgers, corrections | Surveillance timestamps |

### 3.3 Content limits

- No real clockmakers, brands, or institutions copied.
- No time police; no punishment for a late bell or a missed winding.
- No surveillance reading of the records; the drift book is not a spy log.
- No sleep shaming; quiet-hour rules belong to expansion 41.
- No alarm confusion; a time signal is never a hazard alert.
- No second roster; shift authority stays with expansion 02.
- No new save section.

---

## 4. THE HOUR WORLD

### 4.1 Interior rooms

- **`room_time_office`** — the standard mark, the drift book, and the clock registry.
- **`room_clock_bench`** — the repair bench, springs, pallets, and oil.
- **`room_bell_frame`** — the frame, rope, clapper, and the bell schedule.
- **`room_clepsydra_nook`** — the water clock, basin, and orifice tools.
- **`room_glass_store`** — sandglasses, spare bulbs, and sand grades.
- **`room_schedule_desk`** — where the day's shape is read and posted.
- **`room_pendulum_gallery`** — the shelter's best clock and its case.
- **`room_standard_mark`** — the indoor noon mark for bad weather.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_noon_mark` | The Noon Mark | 6 | Sun line and correction |
| `loc_bell_arch` | The Bell Arch | 3 | Surface bell and rope |
| `loc_sand_beds` | The Sand Beds | 3 | Hourglass sand |
| `loc_clock_wreck` | The Clock Wreck | 6 | Pre-war workshop ruin |
| `loc_rail_time_post` | The Rail Time Post | 7 | Road and rail hour |
| `loc_outpost_hour_stone` | The Hour Stone | 7 | Outpost agreement |
| `loc_clepsydra_fall` | The Drip Fall | 5 | Spring for the water clock |
| `loc_signal_ridge` | The Signal Ridge | 3 | Noon signal lamp |
| `loc_weight_pit` | The Weight Pit | 5 | Clock weights and stones |
| `loc_watchmaker_row` | The Watchmaker Row | 6 | Ruined shop row, parts |

All locations must resolve in `locations.json` and pass the map loader gate.

### 4.3 The rhythm

Wind at dawn, compare at noon, correct in the afternoon, ring the shift bells,
log the drift at the end of the day, and settle the standard hour once a
season. The unit of play is the day inside the hour.

---

## 5. MAIN STORYLINE — "THE HOUR WE ALL KEEP"

### 5.1 Central conflict

**Adem Rusk** keeps the drift book and has nobody to hand it to. **Cyra
Linney** can repair a deadbeat escapement with a file and a lamp but cannot
convince the shelter that a clock is worth a workbench. **Harl Senn** rings the
bells and has been quietly adding fifteen seconds to each ring because the
bell becomes the shelter's real time and no one checks the book. **Orlo Zorn**
builds a water clock in a sump nook because the grid will fall again and the
shelter must be able to wake a shift without electricity. **Juna Serle** keeps
the schedule and needs one hour she can trust. **Fain Pike** wants to offer the
hour to the outposts and is told repeatedly that outposts do not take orders.

Then the winter when the mainspring ruptures during a storm: the bells ring
late, the second shift sleeps through the meal, the ward's medicine schedule
shifts by an hour, and the shelter learns what a broken clock actually costs.
The repair takes nine days. On the tenth, the noon mark is checked in front of
witnesses, every clock in the shelter is set from one source, and the bell
rings on time while people stop what they are doing to listen to it. The
shelter's real decision is not who fixes the clock. It is whether the hour
belongs to everyone or to the person holding the key.

The expansion's question: **who decides what time it is?**

### 5.2 Theme (unspoken)

**A community that shares an hour has agreed to live in the same day.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_timekeeper_adem_rusk` | Adem Rusk | Timekeeper | Drift book and registry |
| `npc_watchmaker_cyra_linney` | Cyra Linney | Watchmaker | Bench, escapements, repairs |
| `npc_bell_harl_senn` | Harl Senn | Bell keeper | Rings, schedule, rope |
| `npc_clepsydra_orlo_zorn` | Orlo Zorn | Water-clock builder | Clepsydra and fallback |
| `npc_schedule_juna_serle` | Juna Serle | Schedule clerk | The day's shape |
| `npc_glass_bree_farrow` | Bree Farrow | Glass and sand | Hourglasses, bulbs |
| `npc_signals_fain_pike` | Fain Pike | Signals | Noon lamp and outposts |
| `npc_apprentice_dace_orren` | Dace Orren | Apprentice | Winding, cleaning, records |

### 5.4 Story beats (15)

1. **Three Clocks.** The shelter's timepieces disagree by eleven minutes.
2. **The Book.** Adem opens the drift record nobody has read.
3. **The Bench.** Cyra asks for a bench and gets a corner.
4. **The Noon Mark.** The sun line is re-cut and checked.
5. **The Bell.** Harl is caught adding seconds and admits why.
6. **The Set.** All clocks are set from one source for the first time.
7. **The Fall.** The grid drops; Orlo's water clock carries the shift.
8. **The Silt.** The clepsydra orifice clogs and the night loses an hour.
9. **The Issue.** Chronometers are signed out to a road crew.
10. **The Stone.** An outpost accepts the hour — and asks who set it.
11. **The Rupture.** The mainspring breaks in a storm.
12. **The Nine Days.** The bench repairs what cannot be replaced.
13. **The Witnesses.** The standard hour is checked and posted.
14. **The Quiet Hours.** Bells are re-shaped around the sleep owners' rules.
15. **The Hour We All Keep.** The shelter winds its clocks on a rota.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Standard | noon mark / signals / one good clock | source |
| Winding | rota / keeper / two keepers | duty |
| Bells | full schedule / shift only / silent | sound |
| Corrections | daily / weekly / at drift limit | discipline |
| Powerless | water / sand / candle marks | fallback |
| Outposts | offered / invited to share / free to refuse | agreement |
| Records | full drift book / correction only / minimal | honesty |
| Final | standard hour / keeper office / open rota | authority |

### 5.6 Endings (5 + fade)

1. **The Standard Hour** — every outpost and road keeps the shelter's hour
   because it earned the right to offer it.
2. **The Open Bench** — clockwork becomes a taught trade, and the bench never
   empties.
3. **The Water Hour** — the powerless fallback becomes the shelter's honest
   backup, tested every season.
4. **The Quiet Bell** — fewer bells, better kept, and the shelter hears the
   ones that matter.
5. **The Keeper's Hour** — one keeper holds the hour with a rota and a book,
   and the office outlives the keeper.
6. **Fade** — a bell rope moving in the dark, a drift book open to a corrected
   page, and a child asking why the noon mark is cut into the floor.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_hour_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_hour_three_clocks`, `quest_hour_book`, `quest_hour_bench`,
`quest_hour_noon_mark`, `quest_hour_bell`, `quest_hour_set`, `quest_hour_fall`,
`quest_hour_silt`, `quest_hour_issue`, `quest_hour_stone`,
`quest_hour_rupture`, `quest_hour_nine_days`, `quest_hour_witnesses`,
`quest_hour_quiet_hours`, `quest_hour_hour_we_all_keep`.

### 6.2 Side quests (30)

**Registry (5)**
- `quest_hour_registry` — every clock counted
- `quest_hour_wind_rota` — winding rota written
- `quest_hour_cases` — cases and keys
- `quest_hour_positions` — placement by room
- `quest_hour_survey` — accuracy survey

**The bench (5)**
- `quest_hour_spring` — mainspring replaced
- `quest_hour_pallet` — pallet filed
- `quest_hour_oil` — clock oil made
- `quest_hour_weights` — weights matched
- `quest_hour_case_repair` — case repaired

**The hour (5)**
- `quest_hour_noon_check` — noon checked
- `quest_hour_signal` — signal lamp lit
- `quest_hour_correction` — corrections made
- `quest_hour_drift` — drift measured
- `quest_hour_post` — standard hour posted

**Bells (5)**
- `quest_hour_bell_rope` — rope replaced
- `quest_hour_clapper` — clapper tightened
- `quest_hour_schedule` — schedule agreed
- `quest_hour_quiet` — quiet hours respected
- `quest_hour_meal_bell` — meal bell kept

**The powerless (5)**
- `quest_hour_clepsydra` — water clock built
- `quest_hour_orifice` — orifice cleared
- `quest_hour_sand` — sand graded
- `quest_hour_glass` — bulbs blown
- `quest_hour_candle_mark` — candle marks kept

**Records and roads (5)**
- `quest_hour_drift_book` — drift book kept
- `quest_hour_chronometer` — chronometer issued
- `quest_hour_return` — instrument returned
- `quest_hour_outpost` — hour offered
- `quest_hour_lesson` — lesson taught

### 6.3 Repeatable quests (8)

`quest_hour_repeat_wind`, `quest_hour_repeat_check`,
`quest_hour_repeat_ring`, `quest_hour_repeat_record`,
`quest_hour_repeat_orifice`, `quest_hour_repeat_issue`,
`quest_hour_repeat_teach`, `quest_hour_repeat_relief`.

### 6.4 Dynamic hooks

Live events (`TickHour`, `TickDay`, curfew changes, shift events, grid state,
weather, expeditions, outpost messages) attach authored follow-ups through
existing seams. No new event bus.

### 6.5 Constraints

- The tick contract is read, never modified.
- Schedules stay with `ShelterScheduleSystem`; shifts with the roster.
- Morale and stress use `NeedsSystem.Modify` only.
- Bells respect the sleep owners' quiet hours.
- Physical measurement stays with expansion 29.
- Records file with `StandingRecord`.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `TimekeepingSystem` (new, `Ashfall.Core`)

**Owns:** the clock registry, the standard hour, drift measurement, and
corrections. **Consumes:** `ISimClock`/`IWallClock` reads, schedule state,
workshop condition. **Data:** `clock_registry.json`, `time_standards.json`.
**Rules:** every clock has an owner and a known error; the standard hour has
one source; corrections are recorded, never hidden.

### 7.2 `ClockworksSystem` (new, thin, `Ashfall.Core.Crafting`)

**Owns:** the clock bench: springs, pallets, gears, pivots, oils, casework,
and repair jobs. **Consumes:** `ShelterWorkshopSystem`, `EquipmentConditionSystem`,
inventory. **Data:** `clock_parts.json`, `clock_repairs.json`. **Rules:** a
repair has a real cause and a real part; a broken clock is a schedule risk, not
a failure screen; work is patient and measured in days.

### 7.3 `ClepsydraSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** water clocks: basins, orifices, flow rates, silt, and the powerless
fallback. **Consumes:** water supply (read-only quantities), weather (freezing).
**Data:** `clepsydra_stations.json`. **Rules:** the fallback is tested every
season; an uncleared orifice is a known error, not a mystery; no power needed.

### 7.4 `HourglassSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** short intervals: sandglasses, bulbs, sand grades, and the intervals
used by the ward, kitchen, and forge. **Consumes:** glass store, sand beds.
**Data:** `hourglasses.json`. **Rules:** short instruments are for intervals,
not schedules; a cracked bulb is replaced, never glued.

### 7.5 `BellSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** bells: the frame, rope, clapper, ring schedule, and audibility.
**Consumes:** schedule state, quiet hours (41), hazard alarms (23, separate).
**Data:** `bell_schedules.json`. **Rules:** bells mark work, meals, and rest;
they never imitate or replace hazard alarms; the bell keeper's additions are
recorded or removed.

### 7.6 `TimeSignalSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** the noon mark, the signal lamp, and the daily public time check.
**Consumes:** weather, the signal owner's lamps (49 heliograph boundary).
**Data:** `time_signals.json`. **Rules:** the check is public and witnessed;
weather cancels without blame; signals are time, never messages.

### 7.7 `ChronometerIssueSystem` (new, thin, `Ashfall.Core.World`)

**Owns:** instruments issued to road crews, expeditions, and outposts:
sign-out, condition, return, and the lost-instrument record. **Consumes:**
expedition and outpost owners. **Data:** `chronometer_issue.json`. **Rules:**
an issued instrument is a responsibility, not a reward; loss is recorded
factually; the standard hour is offered, never enforced.

### 7.8 `TimeRecordSystem` (new, thin, `Ashfall.Core.Narrative`)

**Owns:** the drift book, corrections, seasonal accuracy reports, and the
lessons of the hour. **Data:** `time_records.json`. Files through
`StandingRecord`. **Rules:** records are honest and dull; the drift book is not
a surveillance log; the year's accuracy is read aloud once.

### 7.9 Systems explicitly not added

- No second clock, schedule, roster, alarm, metrology, power, or morale system.
- No time enforcement, penalties, or surveillance.
- No sleep regime; quiet hours remain with their owner.
- No new RNG stream beyond the live tick path.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `clock_registry.json` (new)

```json
{
  "schema_version": 1,
  "clocks": [
    {
      "clock_id": "clock_pendulum_gallery_01",
      "display_name": "The Gallery Clock",
      "kind": "pendulum",
      "room_id": "room_pendulum_gallery",
      "owner_id": "npc_timekeeper_adem_rusk",
      "rated_drift_seconds_per_day": 0.4,
      "winding_interval_days": 8,
      "condition": 92
    }
  ]
}
```

### 8.2 `time_standards.json` (new)

Standards: source, room, method, witness rule, posting board, revision.

### 8.3 `clock_parts.json` (new)

Parts: spring, pallet, gear, pivot, weight, oil, case, glass, key, tools.

### 8.4 `clock_repairs.json` (new)

Repairs: symptom, cause, part, days, skill, result, lesson.

### 8.5 `clepsydra_stations.json` (new)

Clepsydras: station, basin, orifice, flow, silt, service interval.

### 8.6 `hourglasses.json` (new)

Hourglasses: interval, bulb, sand grade, user, replacement.

### 8.7 `bell_schedules.json` (new)

Bells: time, kind, ring count, quiet exception, keeper.

### 8.8 `time_signals.json` (new)

Signals: noonday, weather cancel, lamp, witness, posted correction.

### 8.9 `chronometer_issue.json` (new)

Issues: instrument, crew, sign-out, condition, return, loss note.

### 8.10 `time_records.json` (new)

Records: day, clock, error, correction, source, keeper, lesson.

### 8.11 Items

New items appended to `items.json`: `item_clock_spring`,
`item_clock_gear`, `item_clock_pallet`, `item_pendulum_rod`,
`item_clock_weight`, `item_clock_oil`, `item_watch_glass`,
`item_hourglass_sand`, `item_hourglass_bulb`, `item_clepsydra_basin`,
`item_clepsydra_orifice`, `item_bell_rope`, `item_bell_clapper`,
`item_chronometer_case`, `item_drift_book`, `item_time_signal_lamp`,
`item_winding_key`, `item_noon_mark_stone`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`shelter_schedule` remains the live save owner. The clock registry, drift
book, clepsydra state, bell state, issued instruments, and records are additive
sub-objects inside it. No new save section.

### 9.2 State to persist

- Clocks owned, room, owner, winding interval, and condition.
- Drift per clock and the last correction.
- The standard source and posted hour.
- Clepsydra basins, orifices, and service state.
- Hourglass inventory and replacements.
- Bell schedule and quiet exceptions.
- Issued chronometers and returns.
- The drift book and seasonal reports.

### 9.3 Determinism

- Time reads come from the live tick path only; no wall-clock reads in Core.
- Drift is computed from authored rates plus determined wear, not seeded
  noise; the same saves produce the same drift.
- Bell timing and corrections are integer seconds; culture-invariant.
- Weather cancels are determined by the live weather path.
- Paired replay hashes must match; no `System.Random`.

### 9.4 Migration

Legacy saves load with no clocks registered; the schedule keeps working and
the drift book starts empty. A save with a broken grid loads the water clock
in a serviceable state. The standard hour defaults to the shelter's own case.

### 9.5 Checksum

Invariant-culture floats for seconds and rates; integer day, hour, count, and
condition fields.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `TimeOfficePanel` (new) | Registry, drift, standard hour | `TimeOfficeHostSession` |
| `ClockBenchPanel` (new) | Repair jobs and parts | same |
| `BellSchedulePanel` (new) | Bells and quiet rules | same |
| `ClepsydraPanel` (new) | Water clock and services | same |
| `ChronometerPanel` (new) | Issues and returns | same |
| `ShelterSchedulePanel` (extend) | Live schedule with real time | existing |
| `EquipmentConditionPanel` (extend) | Timepieces as tracked items | existing |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- No penalty text for late bells or missed windings.
- Drift is presented as numbers and words, never color-only.
- Quiet hours are stated before any bell change is confirmed.
- Keyboard/controller close/back preserved; focus maintained on refresh.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a key turned in a case, a spring
releasing, a bell rope creaking, a drop of water striking a basin, sand
turning in glass. No cue is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `ISimClock` / `IWallClock` | Read-only time contract |
| `ShelterScheduleSystem` | Reliable time for curfew and beds |
| `DutyRosterSystem` (Exp 02) | Shifts and hour duties |
| 56 The Calendar (Wave 9) | Day boundary and observances |
| 23 The Alarm (Wave 3) | Keep time signals separate |
| 41 The Quiet (Wave 6) | Quiet hours and bell rules |
| 29 The Glass (Wave 4) | Hourglass bulbs and instrument source |
| 40 The Wheel (Wave 6) | Machine boundary |
| 21 The Grid (Wave 2) | Electric clock power |
| 34 The Long Road (Wave 5) | Chronometer issue |
| 44 The Outpost (Wave 7) | Standard hour offer |
| `NeedsSystem` | Comfort and morale via `Modify` |
| `ShelterWorkshopSystem` | Clock bench |
| `EquipmentConditionSystem` | Timepiece condition |
| `StandingRecord` | Drift book filing |
| 49 The Mirror (Wave 8) | Signal lamp boundary |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm clock contracts, schedule owner, host
session, roster, workshop, needs, record, alarm, quiet, glass, and outpost
owners. Record file:line; change nothing.

**Phase 1 — Data + validators.** Author the ten catalogs; register validators
and scanner.

**Phase 2 — Pure Core.** `TimekeepingSystem`, `ClockworksSystem`,
`ClepsydraSystem`, `HourglassSystem`, `BellSystem`, `TimeSignalSystem`,
`ChronometerIssueSystem`, `TimeRecordSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `TimeOfficeHostSession`, focused selftest coverage,
fresh journey from the three-clock survey to the standard hour.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** Multi-season soak: winding load, drift, bell quiet
compliance, chronometer loss, powerless days.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Clocks | 14 |
| Standards | 6 |
| Parts | 18 |
| Repairs | 14 |
| Clepsydras | 6 |
| Hourglasses | 8 |
| Bell schedules | 12 |
| Signals | 8 |
| Chronometer issues | 10 |
| Records | 20 |
| Items | 18 |
| Locations | 10 |
| Rooms | 8 |
| NPCs | 8 |
| Main quests | 15 |
| Side quests | 30 |
| Repeatable | 8 |
| Endings | 5 + fade |
| Prose estimate | 60,000–75,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Tick contract drift | High | Read-only contract |
| Schedule duplication | High | Extend live owner |
| Alarm confusion | High | Separate signal kinds |
| Sleep shaming | High | Quiet-hour owners rule |
| Metrology overlap | Medium | Time only |
| Power overlap | Medium | Draw through grid |
| Determinism break | Low | Integer seconds |
| Tone perfectionism | Medium | Small tools, real work |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `clock_registry.json` | 14 | 3,500 |
| `time_standards.json` | 6 | 2,000 |
| `clock_parts.json` | 18 | 3,000 |
| `clock_repairs.json` | 14 | 3,000 |
| `clepsydra_stations.json` | 6 | 2,000 |
| `hourglasses.json` | 8 | 2,000 |
| `bell_schedules.json` | 12 | 2,500 |
| `time_signals.json` | 8 | 2,000 |
| `chronometer_issue.json` | 10 | 2,500 |
| `time_records.json` | 20 | 3,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 18 | 3,000 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~57,000** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R57-1 | Tick contract change | Low | High | Read-only |
| R57-2 | Schedule fork | Med | High | Extend owner |
| R57-3 | Alarm confusion | Med | High | Kind separation |
| R57-4 | Quiet-hour breach | Med | High | Owner rules |
| R57-5 | Metrology overlap | Low | Medium | Time scope |
| R57-6 | Grid overlap | Low | Medium | Drawn power |
| R57-7 | Determinism | Low | High | Integer time |
| R57-8 | Content overrun | Med | Medium | Budget |
| R57-9 | Tone didacticism | Med | Medium | Small stakes |
| R57-10 | Ornamental clocks | Med | Low | One owner each |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Where does drift state live?** Recommended: additive inside
   `shelter_schedule`, with the registry owned by the new timekeeper system.
2. **Does the standard hour touch outposts?** Recommended: offered only, with
   the offer recorded and refusals honored.
3. **Are bell changes player-confirmed?** Recommended: yes; quiet-hour owners
   are consulted first.
4. **Do chronometers affect expedition timing?** Recommended: only as an
   authored modifier to existing travel estimates; no new travel math.
5. **Who signs the drift book?** Recommended: the keeper and one witness each
   season, as a practice rather than a governance rule.

---

## 17. APPENDIX D — CLOCK REGISTRY TABLE

| # | Clock | Kind | Room | Owner | Drift s/day | Wind | Condition |
|---|---|---|---|---|---|---|---|
| 1 | The Gallery Clock | pendulum | gallery | Adem | 0.4 | 8 d | 92 |
| 2 | The Office Wall Clock | weight | time office | Adem | 1.1 | 1 d | 74 |
| 3 | The Kitchen Clock | spring | kitchen | cook | 3.2 | 2 d | 61 |
| 4 | The Ward Clock | electric | ward | nurse | 0.0 | grid | 88 |
| 5 | The Workshop Clock | spring | workshop | Cyra | 2.6 | 2 d | 55 |
| 6 | The Mess Clock | weight | mess | Juna | 1.8 | 1 d | 70 |
| 7 | The Watchroom Clock | spring | watchroom | watch | 4.0 | 2 d | 42 |
| 8 | The Nursery Clock | spring | nursery | caregiver | 2.2 | 1 d | 80 |
| 9 | The Sump Nook Clepsydra | water | clepsydra nook | Orlo | 0.0 | water | 66 |
| 10 | The Bell Frame Regulator | pendulum | bell frame | Harl | 0.8 | 8 d | 85 |
| 11 | The Guest Clock | spring | guest room | Brun (56) | 5.1 | 2 d | 38 |
| 12 | The Lab Clock | electric | lab | chemist (39) | 0.0 | grid | 79 |
| 13 | The Rail Shed Clock | weight | rail shed | road crew | 2.9 | 1 d | 58 |
| 14 | The Keeper's Pocket Watch | spring | carried | Adem | 6.4 | 1 d | 34 |

Fourteen timepieces and the table already tells the expansion's story: the
pocket watch has the worst drift and the oldest hands, the electric clocks
are perfect until the grid drops, and the kitchen has been quietly cooking to
a clock that is three seconds slow every day and twenty minutes wrong by
winter. The registry makes every one of those errors known and ownable, and
the first gift the time office gives the shelter is not accuracy — it is the
honest list.

---

## 18. APPENDIX E — TIME STANDARDS TABLE

| # | Standard | Source | Room | Method | Witness |
|---|---|---|---|---|---|
| 1 | The Noon Mark | sun | standard mark | shadow line | keeper |
| 2 | The Signal Lamp | noon mark | signal ridge | lamp flash | watcher |
| 3 | The Gallery Clock | pendulum | gallery | regulation | keeper |
| 4 | The Rail Hour | desk clock | rail shed | posted sheet | clerk |
| 5 | The Outpost Hour | signal lamp | hour stone | agreement | envoy |
| 6 | The Ward Hour | quiet standard | ward | nurse check | nurse |

Six standards, one of which is the sun and cannot be argued with. The fifth
row is the expansion's diplomacy: the outpost hour is the shelter's hour only
because the outpost said yes, and the agreement is written down where both
parties can read it.

---

## 19. APPENDIX F — PARTS TABLE

| # | Part | Material | Source | Used for | Wear |
|---|---|---|---|---|---|
| 1 | Mainspring | spring steel | foundry (10) | drive | fatigue |
| 2 | Hairspring | fine alloy | salvage | regulation | bending |
| 3 | Pallet | hardened steel | bench | escapement | faces |
| 4 | Escape wheel | brass | foundry | beat | teeth |
| 5 | Gear wheel | brass | foundry | train | arbor wear |
| 6 | Pivot | steel | bench | bearings | ovality |
| 7 | Bushing | brass | bench | pivot seats | wallow |
| 8 | Pendulum rod | invar alloy | salvage | regulation | thermal |
| 9 | Pendulum bob | brass | foundry | regulation | scratches |
| 10 | Weight | iron, stone | weight pit | drive | rope wear |
| 11 | Line | hemp, wire | cordage (158) | drive | stretch |
| 12 | Bell rope | hemp | cordage | ringing | fray |
| 13 | Clapper | iron | foundry | ringing | seat |
| 14 | Glass | borosilicate | glassworks (29) | cases | cracks |
| 15 | Case wood | timber | joinery (58) | protection | dry rot |
| 16 | Oil | fine lubricant | chemistry (39) | pivots | gum |
| 17 | Sand | graded silica | sand beds | hourglasses | damp |
| 18 | Bulb | glass | glassworks | hourglasses | cracks |

Eighteen parts with sources that cross the whole shelter, which is the
quiet argument of this table: a clock cannot be maintained by one trade. The
fourteenth row runs to expansion 29 and the fifteenth to expansion 58, and the
sixteenth to the reagent works, which makes the time office a consumer of
three other plans in the most ordinary way possible.

---

## 20. APPENDIX G — REPAIR TABLE

| # | Symptom | Cause | Part | Days | Result |
|---|---|---|---|---|---|
| 1 | Loses 5 min/day | mainspring fatigue | mainspring | 3 | steady |
| 2 | Gains 3 min/day | pallet worn | pallet | 2 | steady |
| 3 | Stops at same hour | gear tooth | gear | 2 | runs |
| 4 | Runs fast in heat | pendulum rod | invar rod | 4 | stable |
| 5 | Grinding noise | dry pivot | oil | 1 | quiet |
| 6 | Weight hits floor | rope stretch | line | 1 | corrected |
| 7 | Case door loose | hinge | case | 1 | secure |
| 8 | Glass cracked | impact | glass | 2 | replaced |
| 9 | Bell dull | clapper seat | clapper | 2 | clear |
| 10 | Bell late | rope slip | rope | 1 | timed |
| 11 | Clepsydra slow | orifice silt | orifice | 1 | cleared |
| 12 | Clepsydra fast | basin leak | basin | 2 | sealed |
| 13 | Hourglass damp | sand clumped | sand | 1 | regraded |
| 14 | Watch fogged | seal | case seal | 1 | clean |

Fourteen repairs, every one small, patient, and reversible. The eleventh row
is the expansion's favorite because it costs nothing but attention: the water
clock that saved the shelter during the grid failure was stopped by a thumb's
worth of silt, and the fix is a wire and an afternoon.

---

## 21. APPENDIX H — CLEPSYDRA TABLE

| # | Station | Basin | Orifice | Flow ml/min | Service | Error |
|---|---|---|---|---|---|---|
| 1 | Sump nook | ceramic | bronze needle | 60 | 14 d | 2% |
| 2 | Ward fallback | glass | sapphire chip | 30 | 10 d | 1% |
| 3 | Kitchen fallback | ceramic | bronze needle | 90 | 14 d | 4% |
| 4 | Bell fallback | ceramic | steel pin | 45 | 21 d | 3% |
| 5 | Outpost hour | clay | bored stone | 40 | 30 d | 6% |
| 6 | Teaching station | glass | removable chip | 20 | 7 d | 2% |

Six clepsydras, and the sixth exists so children can watch the water climb
and understand what the bell means. The outpost row's six percent error is
honest — a clay basin bored with a stone in an outpost is not a scientific
instrument, and the standard hour is offered with the error printed beside it.

---

## 22. APPENDIX I — HOURGLASS TABLE

| # | Interval | Bulb | Sand grade | Used by | Replace |
|---|---|---|---|---|---|
| 1 | 1 minute | small | fine | ward pulse check | 30 d |
| 2 | 3 minutes | small | fine | cooking eggs | 60 d |
| 3 | 5 minutes | medium | fine | forge quench | 45 d |
| 4 | 10 minutes | medium | medium | kitchen proof | 45 d |
| 5 | 15 minutes | medium | medium | ward rounds | 90 d |
| 6 | 30 minutes | large | coarse | watch relief | 120 d |
| 7 | 1 hour | large | coarse | clepsydra check | 150 d |
| 8 | 2 hours | large | coarse | kiln soak (31) | 180 d |

Eight intervals, and the eighth pairs with the kiln in expansion 31, which is
the kind of cross-trade habit this expansion celebrates: a potter needs time
the way a nurse does, and both need it in a bulb that will not rust.

---

## 23. APPENDIX J — BELL SCHEDULE TABLE

| # | Time | Kind | Rings | Quiet exception | Keeper |
|---|---|---|---|---|---|
| 1 | 05:30 | wake | 3 | ward exempt | Harl |
| 2 | 06:00 | shift 1 | 2 | none | Harl |
| 3 | 08:00 | meal | 2 | none | Harl |
| 4 | 12:00 | noon | 1 | none | signal |
| 5 | 14:00 | shift 2 | 2 | none | Harl |
| 6 | 18:00 | meal | 2 | none | Harl |
| 7 | 22:00 | shift 3 | 1 | soft | Harl |
| 8 | 23:00 | curfew | 1 | soft | Harl |
| 9 | anytime | hazard | alarm | separate (23) | alarm |
| 10 | anytime | quiet | 0 | all | all |

The ninth row is the expansion's most important line: a bell never rings a
hazard and an alarm never rings a meal. The tenth row is the promise that
silence is a schedule too, and that the quiet-hour owners in expansion 41 can
turn the bell off without asking the keeper's permission.

---

## 24. APPENDIX K — SIGNAL TABLE

| # | Signal | Weather | Lamp | Witness | Posting |
|---|---|---|---|---|---|
| 1 | Noonday flash | clear | mirror white | any | correction board |
| 2 | Noonday muted | cloud | none | keeper | check by clock |
| 3 | Noonday cancelled | storm | none | keeper | note only |
| 4 | Correction posted | any | none | keeper+1 | desk board |
| 5 | Standard revised | any | none | season meeting | both boards |
| 6 | Outpost offer | clear | mirror white | envoy | hour stone |
| 7 | Outpost accept | clear | mirror green | both | hour stone |
| 8 | Outpost refuse | any | none | envoy | note only |

Eight signals with no words at all, which is the boundary with expansion 49:
the lamp flashes the hour and nothing else. The eighth row honors a refusal
with a note instead of a grudge, and the stone keeps no record of who said no.

---

## 25. APPENDIX L — CHRONOMETER ISSUE TABLE

| # | Instrument | Crew | Sign-out | Condition | Return |
|---|---|---|---|---|---|
| 1 | Chronometer 01 | rail survey | day 612 | 88 | day 619 |
| 2 | Chronometer 02 | road crew | day 640 | 74 | day 651 |
| 3 | Watch 03 | ridge outpost | season | 66 | season |
| 4 | Watch 04 | river crew | day 700 | 59 | day 712 |
| 5 | Watch 05 | salvage team | day 720 | 81 | day 729 |
| 6 | Watch 06 | hunting camp (32) | day 745 | 70 | day 752 |
| 7 | Watch 07 | neighbor settlement | day 770 | 64 | day 781 |
| 8 | Watch 08 | ward transfer | day 790 | 77 | day 790 |
| 9 | Watch 09 | lost | day 800 | 55 | not returned |
| 10 | Watch 10 | replacement | day 810 | 100 | day 818 |

The ninth row is the expansion's honesty test: an instrument is lost, and the
record says lost rather than stolen, and the crew pays for its replacement in
work rather than in blame. The eighth row is the smallest and kindest: a watch
issued to a ward transfer so the nurse's timeline survives the journey.

---

## 26. APPENDIX M — DRIFT RECORD TABLE

| # | Day | Clock | Error s | Correction | Source | Keeper |
|---|---|---|---|---|---|---|
| 1 | 401 | office | +22 | −22 | noon | Adem |
| 2 | 401 | kitchen | −88 | +88 | office | Adem |
| 3 | 402 | workshop | −61 | +61 | office | Cyra |
| 4 | 403 | watchroom | −140 | +140 | office | watch |
| 5 | 404 | guest | −201 | +201 | office | Dace |
| 6 | 405 | gallery | +2 | 0 | noon | Adem |
| 7 | 410 | rail shed | −64 | +64 | office | clerk |
| 8 | 420 | watch | lost | none | office | Adem |
| 9 | 430 | kitchen | −12 | +12 | noon | Dace |
| 10 | 431 | all | 0 | set | noon | witness |

The tenth row is the year's best entry: every clock in the shelter set from
one source on one afternoon, in front of witnesses, with the correction
written down. The table reads like bookkeeping because it is bookkeeping, and
the expansion's argument is that shared time is kept by boring records and
not by brilliant people.

---

## 27. APPENDIX N — DRIFT ARITHMETIC (WORKED)

A pendulum's daily rate error comes from three authored sources, never from
noise. Let `D` be the error in seconds per day:

- Thermal term: `T = k·(temp − 20 °C)` where `k` is the authored rod coefficient
  in ppm/K scaled to seconds.
- Wear term: `W = w·wear_depth_microns`, authored per escapement type.
- Set term: `S`, the error introduced when the clock was last set.

Then `D = T + W + S`, rounded to integer seconds, and the daily correction is
`−D` applied at the checked interval. A clock checked daily never accumulates
more than one day of error; a clock checked weekly can be wrong by a minute
and a quarter; a pocket watch checked monthly is decoration by the fourth
month. This is why the registry records a `winding_interval_days` and a
`checked_interval_days` separately, and why the drift book's most common
correction is not a repair — it is a habit.

For the water clock the arithmetic is flow:
`flow = nominal_flow_ml_min · (1 − silt_fraction) · (1 − viscosity_error_pct/100)`,
and the emptied basin's hours are `capacity_ml / flow / 60`. An orifice clogged
to a tenth of its flow makes a nine-hour basin last ninety hours, which is how
a night shift loses an hour without anyone touching a clock.

---

## 28. APPENDIX O — VIGNETTES (TONE SAMPLE)

> Cyra holds the pallet up to the lamp and files it three strokes, then
> tests, then two more, and the escaping wheel ticks like a small polite
> animal, and she says the clock is fine, and the clock is fine.

> Orlo sets the bronze needle in the basin at dawn and the first drop falls
> the way it has fallen for four hundred years, and the shelter's second
> shift is called by water while the grid is dark, and nobody thanks the
> basin, and Orlo does not mind.

> At noon the shelter stops, which is to say the people in the yard look up at
> the ridge, and the lamp flashes once, and Adem crosses the correction off
> the board, and the afternoon is eleven seconds luckier than the morning.

> The outpost says no to the standard hour, politely, in the first winter,
> and says yes in the second, and says nothing at all in the third because by
> then the hour stone is simply a stone where the hour lives.

---

## 29. APPENDIX P — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Clock unset | shifts drift | noon check |
| Spring broken | bell late | bench repair |
| Rope slipped | bell late | retie, log |
| Orifice silted | night loses hour | wire clear |
| Sand damp | intervals wrong | regrade |
| Watch lost | crew without hour | issue replacement |
| Grid down | electric clocks stop | clepsydra |
| Standard unclear | arguments | witnesses, posting |
| Bell overuse | quiet broken | schedule cut |
| Records lapsed | drift hidden | weekly book |

Every recovery is a habit the time office already teaches, and the last row is
the expansion's quiet thesis: a shelter does not lose time all at once. It
loses it in the weeks when nobody wrote anything down.

---

## 31. APPENDIX Q — CONTENT REVIEW CHECKLIST

- [ ] No real clockmakers, brands, or observatories are copied.
- [ ] The tick contract is read, never modified.
- [ ] Schedule authority stays with `ShelterScheduleSystem`.
- [ ] Shift authority stays with the duty roster.
- [ ] Time signals never imitate hazard alarms.
- [ ] Quiet hours obey expansion 41's rules, not the bell keeper's.
- [ ] Physical measurement stays with expansion 29.
- [ ] Drift is honest, logged, and never punished.
- [ ] Save additions are additive inside `shelter_schedule`.
- [ ] Determinism uses the live tick path only; no wall-clock reads in Core.

---

## 32. APPENDIX R — GLOSSARY

- **Drift** — how wrong a clock is, in seconds per day.
- **Standard hour** — the one source the shelter sets from.
- **Noon mark** — the sun-cut line used for correction.
- **Clepsydra** — a water clock; the powerless fallback.
- **Regulator** — the shelter's most accurate clock, the bell's reference.
- **Chronometer** — a timepiece issued to a crew or outpost.
- **Hour duty** — the rotation of winding and checking.
- **Correction** — the measured adjustment written in the book.
- **Ring schedule** — which bells sound when, and which do not.
- **Quiet exception** — a bell or hour the sleep owners exempt.

---

## 33. APPENDIX S — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `TimekeepingSystem` | tick, registry | standard, drift | schedule |
| `ClockworksSystem` | workshop | repairs | inventory totals |
| `ClepsydraSystem` | water | flow, silt | water ledger |
| `HourglassSystem` | glass | intervals | glass batches |
| `BellSystem` | schedule | ring state | alarms |
| `TimeSignalSystem` | weather | checks | messages |
| `ChronometerIssueSystem` | crews | issues | expeditions |
| `TimeRecordSystem` | history | drift book | nothing |
| `ShelterScheduleSystem` | standard | curfew | clocks |
| `DutyRosterSystem` | hours | shifts | clocks |
| `NeedsSystem` | nothing | nothing | nothing |
| `EquipmentConditionSystem` | wear | condition | time |
| `ShelterWorkshopSystem` | jobs | bench | time |
| `AlarmSystem` | nothing | alarms | bells |
| `QuietHoursSystem` | nothing | quiet | bells |
| `StandingRecord` | records | records | nothing |
| `GlassworksSystem` | orders | glass | time |
| `PowerGridSystem` | draw | power | clocks |

---

## 34. APPENDIX T — DATA SCHEMA DETAIL (NEW CATALOGS)

**`clock_registry.json`** — `clock_id`, `display_name`, `kind`, `room_id`,
`owner_id`, `rated_drift_seconds_per_day`, `winding_interval_days`,
`condition`, `tags[]`.

**`time_standards.json`** — `standard_id`, `source`, `room_id`, `method`,
`witness_rule`, `posting_board`, `revision`, `tags[]`.

**`clock_parts.json`** — `part_id`, `material`, `source_tag`, `used_for`,
`wear_kind`, `tags[]`.

**`clock_repairs.json`** — `repair_id`, `symptom`, `cause`, `part_id`, `days`,
`result`, `lesson`, `tags[]`.

**`clepsydra_stations.json`** — `station_id`, `basin`, `orifice`, `flow_ml_min`,
`service_days`, `error_pct`, `room_id`, `tags[]`.

**`hourglasses.json`** — `glass_id`, `interval_minutes`, `bulb`, `sand_grade`,
`used_by`, `replace_days`, `tags[]`.

**`bell_schedules.json`** — `bell_id`, `time`, `kind`, `rings`,
`quiet_exception`, `keeper_id`, `tags[]`.

**`time_signals.json`** — `signal_id`, `weather_rule`, `lamp`, `witness_rule`,
`posting`, `tags[]`.

**`chronometer_issue.json`** — `issue_id`, `instrument_id`, `crew`, `sign_out`,
`condition`, `return`, `loss_note`, `tags[]`.

**`time_records.json`** — `record_id`, `day`, `clock_id`, `error_seconds`,
`correction`, `source`, `keeper_id`, `lesson`, `tags[]`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on
missing or duplicate IDs, invalid room, item, or owner references, or
out-of-range numbers.

---

## 35. APPENDIX U — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Registry coverage | ownership | Registry |
| Winding compliance | habit | Schedule |
| Mean absolute drift | accuracy | Records |
| Corrections made | honesty | Records |
| Bells on time | trust | Bell |
| Quiet exceptions | care | Bell |
| Clepsydra service | readiness | Clepsydra |
| Hourglass breaks | supply | Hourglass |
| Instruments issued | reach | Issue |
| Instruments returned | responsibility | Issue |

Telemetry is diagnostic only; it never gates content, never ranks a keeper,
and never becomes a performance score for a bell.

---

## 36. APPENDIX V — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive inside `shelter_schedule`.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling.
- [ ] Phase 6 content passes the review checklist in §31.
- [ ] Phase 7 soak shows a season kept, a spring broken and repaired, a
  powerless day carried by water.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No surveillance, penalty, or sleep-shaming content exists.

---

## 37. APPENDIX W — OPEN QUESTIONS FOR REVIEW

1. Does the standard hour need a fallback if the noon mark is clouded for a
   month?
2. Can any resident own a private watch, or are all timepieces registered?
3. Who may change the bell schedule, and must the sleep owners agree?
4. Does an outpost that refuses the hour still receive chronometer repairs?
5. Are drift records public, or kept by the keeper and read on request?
6. What happens when the keeper and the regulator disagree?
7. May the bell be rung for a death, or is that the memorial owner's sound?
8. Does the shelter ever adopt an outpost's hour instead of its own?

None of these may be decided unilaterally; each changes tone and balance.

---

## 38. APPENDIX X — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 2 | 02 Duty Roster | Hour duties and shift bells |
| 2 | 21 The Grid | Electric clocks and outages |
| 3 | 23 The Alarm | Signal separation |
| 4 | 29 The Glass | Bulbs and instrument glass |
| 5 | 34 The Long Road | Chronometer issue |
| 6 | 40 The Wheel | Machine boundary |
| 6 | 41 The Quiet | Quiet hours and soft bells |
| 7 | 44 The Outpost | Standard hour offer |
| 8 | 49 The Mirror | Signal lamp boundary |
| 8 | 51 The Machine | Timed maintenance |
| 9 | 52 The Warm Ground | Steam clocks? No — heat only |
| 9 | 56 The Calendar | Day boundary, observances |
| 10 | 58 The Joinery | Clock cases |
| 10 | 60 The Wick | Candle marks for powerless hours |

Each hook is additive. The Hour can ship alone, and every other expansion can
ship without it.

---

## 39. APPENDIX Y — ENDING PROSE SKETCHES

**The Standard Hour.** Every outpost and road keeps the shelter's hour because
it earned the right to offer it, and the offer is renewed each season with the
error printed beside it.

**The Open Bench.** Clockwork becomes a taught trade; the bench never empties;
children learn the pallet before they learn the lathe.

**The Water Hour.** The powerless fallback is tested every season and trusted
by everyone, and the shelter's second shift is called by water in the dark.

**The Quiet Bell.** Fewer bells, better kept, and the two that sound are the
two everyone needed anyway.

**The Keeper's Hour.** One keeper holds the hour with a rota and a book, and
the office outlives the keeper because the book does not belong to a person.

**Fade.** A bell rope moving in the dark, a drift book open to a corrected
page, and a child asking why the noon mark is cut into the floor.

---

## 40. APPENDIX Z — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Genius clockmaker | tone fantasy | patient trade |
| Time police | cruelty | no penalties |
| Alarm bells | safety risk | kind separation |
| Silent records | dishonesty | drift book |
| Perpetual accuracy | false | drift is normal |
| Sleep shaming | harm | quiet owners rule |
| Enforced hour | coercion | offered agreement |
| Metrology fork | duplication | time only |
| Power fork | duplication | draw through grid |
| Ornament clocks | waste | one owner each |

The list exists because time as a game system is easy to write as either a
punishment schedule or a genius fantasy. The expansion's rule is that the
clock is small, the error is normal, the book is honest, and the bell is a
promise the shelter hears and keeps.

---

## 41. APPENDIX AA — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Clocks | 14 | 3,500 |
| Standards | 6 | 2,000 |
| Parts | 18 | 3,000 |
| Repairs | 14 | 3,000 |
| Clepsydras | 6 | 2,000 |
| Hourglasses | 8 | 2,000 |
| Bells | 12 | 2,500 |
| Signals | 8 | 2,000 |
| Issues | 10 | 2,500 |
| Records | 20 | 3,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 18 | 3,000 |
| Endings | 6 | 3,000 |
| **Total** | | **~57,000** |

---

## 42. APPENDIX AB — FIRST STANDARD YEAR

| Season | Focus | Milestone |
|---|---|---|
| Spring | registry | fourteen clocks listed |
| Spring | noon mark | sun line re-cut |
| Summer | bench | first mainspring replaced |
| Summer | signals | noon lamp flashing |
| Autumn | bells | schedule agreed |
| Autumn | clepsydra | fallback built |
| Autumn | issue | first chronometer signed out |
| Winter | rupture | spring breaks in a storm |
| Winter | nine days | bench repair completed |
| Winter | witnesses | all clocks set from one source |
| Winter | stone | outpost accepts the hour |
| Winter | rota | winding becomes a duty |

Twelve milestones in the order the shelter chose, and the shape is
characteristic of a trade that only becomes visible when it fails: registry
first, accuracy second, agreement third, and the crisis that makes everyone
care in the fourth season.

---

## 43. APPENDIX AC — HOUR DUTY TABLE

| # | Duty | When | Who | Check |
|---|---|---|---|---|
| 1 | Wind gallery | every 8 days | keeper | turns counted |
| 2 | Wind office | daily | keeper | weight height |
| 3 | Check kitchen | daily | cook | against office |
| 4 | Check watchroom | daily | watch | against office |
| 5 | Noon read | clear days | keeper | mark line |
| 6 | Signal flash | clear days | signals | lamp ready |
| 7 | Corrections | daily | keeper | written |
| 8 | Orifice rub | every 14 days | Orlo | flow tested |
| 9 | Sand regrade | monthly | Bree | interval tested |
| 10 | Bell check | weekly | Harl | rope, clapper |
| 11 | Book review | weekly | keeper | errors read |
| 12 | Relief | as posted | roster | handover note |

Twelve duties, almost all of them minutes long, which is the point: shared
time is maintained by a hundred small habits, not by one great instrument.
The twelfth row hands the hour between people without losing it, and the
handover note is one line long.

---

## 44. APPENDIX AD — TEACHING TABLE

| # | Lesson | Audience | Method | Record |
|---|---|---|---|---|
| 1 | What a minute is | children | hourglass | none |
| 2 | Reading a clock | children | kitchen clock | none |
| 3 | Why noon matters | all | noon mark | board |
| 4 | Winding safely | rota | bench | sheet |
| 5 | Drift and error | clerks | book | sheet |
| 6 | Bell discipline | ringers | frame | sheet |
| 7 | Water clock care | fallback crew | basin | sheet |
| 8 | Instrument care | crews | issue desk | signed |

Eight lessons and the first two are for children, because a shelter that
raises a generation which cannot read a clock has quietly lost the trade it
spent a year rebuilding. The eighth row is signed, which is how instrument
care stops being advice and becomes a habit.

---

## 45. APPENDIX AE — POWERLESS WEEK TABLE

| Day | Grid | Time source | Bell | Note |
|---|---|---|---|---|
| 1 | down | clepsydra | water watch | quiet shift |
| 2 | down | clepsydra | rope manual | short rings |
| 3 | down | clepsydra + sand | manual | corrected twice |
| 4 | down | clepsydra | manual | erosion error 4% |
| 5 | down | clepsydra | manual | orifice cleaned |
| 6 | down | clepsydra | manual | flow stable |
| 7 | back | regulator | schedule | reset in one hour |

The powerless week is the expansion's proof of purpose: seven days without a
grid, one water clock, and a shelter that still ate, slept, and worked on
schedule. The seventh row is the reward — reversing a week of manual bells
takes an hour because the registry and the book already knew what everything
was supposed to say.

---

## 46. APPENDIX AF — TIME OFFICE CHARTER

| Clause | Promise |
|---|---|
| One hour | The shelter keeps one standard hour, and the standard is public |
| Owned | Every clock has an owner, a room, and a known error |
| Wound | Clocks are wound on a rota and the rota is posted |
| Corrected | Errors are measured, corrected, and written down |
| Sounded | Bells mark work, meals, and rest, and never imitate alarms |
| Quiet | The bell yields to the shelter's quiet hours |
| Powered | Electric clocks drink from the grid and never claim its authority |
| Watered | The fallback is tested every season and works when nothing else does |
| Issued | Instruments travel with crews and return with their records |
| Offered | Outposts may take the hour or refuse it without insult |

The time office charter is the expansion's first-class design object, kept
beside the registry where the winding rota is posted. Its first clause is the
one that matters most: the standard is public, which means it belongs to
everyone who hears the bell.

---

## 47. APPENDIX AG — KEEPER SUCCESSION TABLE

| Role | First | Successor | Handover |
|---|---|---|---|
| Timekeeper | Adem | Dace | one full year |
| Watchmaker | Cyra | bench apprentice | one bench season |
| Bell keeper | Harl | rota ringers | one schedule cycle |
| Water clock | Orlo | fallback crew | one powerless test |
| Schedule clerk | Juna | office aide | one quarter |
| Glass and sand | Bree | glassworks hand | one bulb batch |
| Signals | Fain | ridge watcher | one clear month |
| Apprentice | Dace | next recruit | one winding rota |

The table is measured in seasons rather than days, and the timekeeper's
handover takes a full year because a year contains every error the calendar
can produce. The apprentice's last row is the smallest and most important:
the person who learns the winding rota is the person who will keep the hour
after the person who taught them has stopped coming to the office.

---

## 48. APPENDIX AH — RECORD AND REVIEW TABLE

| # | Question | Answer source | Action |
|---|---|---|---|
| 1 | Which clock drifted worst? | book | repair or retire |
| 2 | Which clock was best? | book | trust for standard |
| 3 | How many windings missed? | rota | adjust interval |
| 4 | How many bells late? | bell log | check rope, keeper |
| 5 | How many quiet exceptions? | bell log | talk to sleep owners |
| 6 | Did the fallback work? | powerless test | service orifice |
| 7 | Which instrument was lost? | issue sheet | replace, no blame |
| 8 | Did an outpost accept? | stone | renew or leave |
| 9 | What did the year teach? | keeper | one lesson |
| 10 | What is next year's standard? | meeting | revise and post |

Ten review questions asked once a year with the drift book open, and the
answers are supposed to be dull: which spring tired, which rope stretched,
which outpost said yes. The tenth question is the only ambitious one, because
revising the standard is how the office stays alive rather than becoming an
inherited set of habits.

---

## 49. APPENDIX AI — CALENDAR OF TIME WORK

| Month | Work | Owner | Note |
|---|---|---|---|
| 1 | Wind and check | keeper | daily |
| 2 | Bench survey | Cyra | condition |
| 3 | Noon marks | Adem | weather |
| 4 | Orifice service | Orlo | quarterly |
| 5 | Sand regrade | Bree | damp season |
| 6 | Bell rope | Harl | wear |
| 7 | Instrument issue | desk | road season |
| 8 | Return inspection | desk | after roads |
| 9 | Standard review | meeting | seasonal |
| 10 | Powerless test | fallback crew | annual |
| 11 | Book audit | keeper + witness | annual |
| 12 | Lesson teaching | keeper | winter |

Twelve months and every row is a task someone can finish in an afternoon,
which is the expansion's defense of small institutions: a shelter does not
need a great clock. It needs twelve afternoons a year and a book that says
what happened during them.

---

## 50. APPENDIX AJ — TONE WATCHLIST

| Temptation | Why it fails | House rule |
|---|---|---|
| The brilliant repair | fantasy | patient work |
| The infallible clock | false | drift is normal |
| The punished lateness | cruelty | record, no blame |
| The all-seeing record | surveillance | drift only |
| The military bell | authority | signals of work and rest |
| The compulsory hour | coercion | offered, agreed |
| The mystical water | genre drift | a basin and an orifice |
| The clock as treasure | loot framing | a service, not a prize |

The watchlist exists because timepieces are seductive props. The expansion
keeps them small: a spring, a pallet, a rope, a drop of water, and a book.
The shelter's hour is not kept by the instrument. It is kept by the people
who agreed to wind it.

---

## 52. APPENDIX AK — BELL RINGING TABLE

| # | Ring | Method | Count | Sound reaches | Note |
|---|---|---|---|---|---|
| 1 | Wake | rope, 3 pulls | 3 | all corridors | soft at night |
| 2 | Shift | rope, 2 pulls | 2 | shift rooms | standard |
| 3 | Meal | rope, 2 pulls | 2 | mess | standard |
| 4 | Noon | one pull | 1 | yard, ridge | with lamp |
| 5 | Curfew | one pull | 1 | residential | muffled |
| 6 | Relief | hand bell | 1 | watchroom | carried |
| 7 | Storm | none | 0 | — | alarm owner rings |
| 8 | Death | none | 0 | — | memorial owner decides |
| 9 | Quiet | none | 0 | — | all |

Nine rows and the last three are the discipline the expansion insists on:
the bell does not ring storms, does not ring deaths, and does not ring into
quiet hours. Those three silences protect the bell from becoming an alarm, a
funeral notice, or a nuisance, and keeping the bell narrow is what makes the
shelter believe it when it does ring.

---

## 53. APPENDIX AL — WATCH AND RELIEF TABLE

| # | Watch | Hours | Relief signal | Handover |
|---|---|---|---|---|
| 1 | First | 06–14 | shift bell | wind, check |
| 2 | Second | 14–22 | shift bell | wind, check |
| 3 | Third | 22–06 | soft bell | wind, check |
| 4 | Relief | any | hand bell | note only |
| 5 | Emergency | any | roster call | keeper keeps hour |

The third watch gets the soft bell and the note-only handover, and the fifth
row is the expansion's boundary with the watch owners: in an emergency the
watch command takes the building and the time office keeps the hour, which is
the only job it should never give away.

---

## 54. APPENDIX AM — STANDARD HOUR AGREEMENT TABLE

| # | Party | Offer | Response | Record | Ridicule |
|---|---|---|---|---|---|
| 1 | Holdfast | first | accepted | stone | none |
| 2 | Ridge outpost | second | accepted | stone | none |
| 3 | River crew | second | accepted | sheet | none |
| 4 | Neighbor settlement | third | refused | note | never |
| 5 | Waystation | third | accepted | sheet | none |
| 6 | Rail town | fourth | accepted | stone | none |

Six offers and one refusal, and the fourth row's rule is the entire diplomacy
of time: a refusal is written as a note with no commentary, kept without
ridicule, and renewed next season without pressure. The hour is offered the
way a shelter offers bread — because it is real and because the other party
may already have their own.

---

## 55. APPENDIX AN — FIRST WINTER TABLE

| Week | Event | Clock state | Shelter response | Record |
|---|---|---|---|---|
| 1 | cold snap | office −4 min | extra check | book |
| 2 | storm | spring breaks | manual bell | book |
| 3 | grid down | electric stop | clepsydra | book |
| 4 | repair | spring fitted | bench | book |
| 5 | noon clear | five clocks set | witness | book |
| 6 | outpost answer | hour accepted | stone | book |

The six weeks of the first winter are the expansion's compressed story: cold,
breakage, outage, repair, correction, agreement. The table is included as
authoring guidance because it shows where the drama actually lives — not in
the instrument but in the fifth week, when five clocks are set from one
source in front of witnesses and the shelter quietly becomes a place with an
hour.

---

## 56. APPENDIX AO — SMALL MISTAKES TABLE

| Mistake | Consequence | Lesson |
|---|---|---|
| Wound one clock wrong | it runs an hour off | wind by the book |
| Set from the kitchen | kitchen error spreads | set from the standard |
| Skipped a noon check | drift hidden a week | measure in weather |
| Ignored a soft bell | night shift slept late | relief matters |
| Cleaned orifice with oil | flow slowed | use a wire |
| Regraded sand with salt | sand clumped | dry it first |

Six small mistakes, each costing minutes rather than disasters, and the
lesson column is intentionally unheroic: use a wire, dry the sand, follow the
book. The expansion's tone rule for repair prose is that the shelter's
instruments are kept by habits, and habits are learned by making the small
mistake once and writing it down.

---

## 57. CLOSING STATEMENT

ASHFALL already runs on a tick and a schedule, and it has never once asked
whether its hour is true. `ISimClock`, `IWallClock`, `DayRecord`, and
`ShelterScheduleSystem` give the shelter a heartbeat and a curfew; four
authored horology logs and an orphaned `TimekeepingHorologyCatalog` give it the
trade that keeps the heartbeat honest. The Hour adds the clocks, the bench,
the bells, the water clock, the drift book, the standard hour, and the small
daily work of agreeing what time it is. It adds no power over anyone's day: a
bell is a promise, not a command, and the record of an error is kept the way a
good shelter keeps everything — plainly, and on purpose.

> Wave 10 note: this plan is one of five Wave 10 expansion bibles (57–61). Each
> is self-contained; none requires another to ship. The shared Wave 10 index
> lives at `docs/expansions/wave10/WAVE10_INDEX.md`. The safe pre-signature
> step is Phase 1 (data schemas and validators), which is additive and
> reversible. Evidence anchors: `Assets/Ashfall.Core/Clock/ISimClock.cs`,
> `Assets/Ashfall.Core/IWallClock.cs`, `Assets/Ashfall.Core/Campaign/DayRecord.cs`,
> `Assets/Ashfall.Core/Shelter/ShelterScheduleSystem.cs`,
> `src/Host/ShelterScheduleHostSession.cs` (`SetCurfew`, `SetEmergencyOverride`,
> `AssignBed`, `LoadCatalog`, `TickDay`, `TickHour`, `Save`),
> `Assets/Ashfall.Core/EquipmentConditionSystem.cs`,
> `Assets/Ashfall.Core/Crafting/ShelterWorkshopSystem.cs`,
> `Assets/Ashfall.Core/Narrative/TimekeepingHorologyCatalog.cs` (orphaned;
> `DeadbeatEscapementWearEntry`, `InvarPendulumThermalEntry`,
> `MainspringFatigueRuptureEntry`, `ClepsydraWaterClockEntry`), and the
> narrative logs `deadbeat_escapement_wear_logs.json` (5,832 B),
> `invar_pendulum_thermal_expansion.json` (5,909 B),
> `mainspring_fatigue_rupture_audits.json` (5,046 B),
> `water_clock_orifice_silt_records.json` (5,226 B).