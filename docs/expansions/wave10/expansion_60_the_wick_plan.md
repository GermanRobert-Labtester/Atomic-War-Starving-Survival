# ASHFALL — Expansion 60 Design Bible
# THE WICK
### Wave 10 · Tallow, Beeswax, Wicks, Candles, Lamps, Lanterns, Light Rationing, and the Evening the Shelter Chooses

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-24
**Domain owners touched:** `Ashfall.Core.Narrative` (`CandleMakingWaxCatalog`), `Ashfall.Core.Crafting` (`CraftingSystem`, `ShelterWorkshopSystem`), `Ashfall.Core.Shelter` (`ShelterFireHazardSystem` — open-flame rules)
**Proposed host owner:** `WickHostSession` (extends the workshop, fire, and apiculture surfaces)
**Existing save sections:** `crafting` (workbench jobs and workshop state), `shelter_fire` (fire incidents and zones)
**Existing CLI verbs:** `--data-integrity-selftest`, `--content-utilization-selftest`, `--panel-bind-lifecycle-selftest` (no chandlery-specific verb exists)
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already knows that light costs something. `ShelterPowerSystem` and the
grid owners decide which rooms have electric light; expansion 41's night and
quiet owners have already proposed `NightLampSystem` for the hours after the
sun; expansion 39's product table lists wax and routes it to candles with the
plain note that without it "nights darker"; expansion 47 owns every open flame
in the shelter as a fire hazard, and its own plan observed that nobody has
counted the candles or walked the corridor at night. The apiculture loop is
live inside `GreenhouseState.apiculture` and already yields
`item_beeswax_block`. The horn worker in expansion 59 supplies translucent horn
for lantern panes.

What does not exist: a single candle, wick, lamp, oil flask, mould, dipping
frame, burn test, clarity grade, light plan, or dark-day ledger. Nothing in
`items.json` burns to make light. Five authored trade logs sit in the
narrative store unread: `tallow_rendering_vat_logs.json` (3,516 B:
`fat_source_animal`, `rendering_vat_id`, `yield_grams`),
`beeswax_clarification_records.json` (3,756 B: `wax_lot_source`,
`clarification_method`, `clarity_grade`), `wick_braiding_priming_reports.json`
(3,195 B: `wick_fibre_type`, `braid_ply_count`, `priming_wax_type`),
`candle_dip_mould_assays.json` (3,099 B: `candle_method`, `wax_blend_type`,
`burn_duration_hours`), and `beeswax_rendering_dipping_assays.json` (5,011 B:
`rendering_vat_id`, `beeswax_melting_point_celsius`,
`unadulterated_purity_pct`, `candle_burn_rate_grams_per_hour`).
`CandleMakingWaxCatalog` models all five (`TallowRenderingVatLog`,
`BeeswaxClarificationRecord`, `WickBraidingPrimingReport`,
`CandleDipMouldAssay`) and is referenced by zero runtime files.

**The Wick** is the expansion about the small burning thing that lets a shelter
have an evening: the rendering vat, the clarified wax, the braided wick, the
dipped candle, the oil lamp, the horn-paned lantern, the light plan, the
ration that keeps the ward lit, and the snuffer that ends the day on purpose.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| 21 The Grid (Wave 2) | Electric light and power | Firelight only; never grid authority |
| 39 The Reagent (Wave 6) | Chemistry, soap, wax products | Takes wax and tallow as feedstock |
| `ApicultureSystem` (live) | Bees and wax production | Consumes `item_beeswax_block` |
| 41 The Quiet (Wave 6) | Sleep, dark hours, rest | Lights the evening; obeys quiet rules |
| 47 The Brigade (Wave 8) | Open flames, fire zones, alarms | Every lamp is a fire-zone item |
| 23 The Alarm (Wave 3) | Hazard alarms | Lanterns never signal; no alarm light |
| 49 The Mirror (Wave 8) | Signal lamps and messages | Light, not message |
| 57 The Hour (Wave 10) | Time and candle marks | Candle marks for powerless time |
| 58 The Joinery (Wave 10) | Wood and frames | Lamp stands and lantern frames |
| 59 The Bone Shop (Wave 10) | Horn panes | Supplies panes to this trade |
| 26 The Common Table (Wave 3) | Food | Kitchen light only; no food authority |
| 55 The Quarter (Wave 9) | Shared living and fairness | Light rationing is explained there |
| 15 The Deep Root (Wave 1) | Livestock and tallow sources | Consumes tallow |
| 48 The Pastime (Wave 8) | Evenings, songs, games | Light for them; no performance owner |
| `ShelterFireHazardSystem` | Ignition, smoke, suppression | Registers lamps; never suppresses fire |
| `NeedsSystem` | Morale and stress | Comfort via `Modify` only |
| `StandingRecord` | Records | Files the dark-day ledger |

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The shelter's evenings end when the grid does. The ward sutures by a
flickering borrowed lamp; the mess hall has four candles and a rule about who
gets them; the beekeeper's wax goes to the apothecary and the tallow goes to
soap, and nobody ever put the two together with a wick; the corridors after
dark are walked by memory; and the fire brigade treats every open flame as a
risk with no inventory attached.

**The Wick** is the expansion about controlled fire as a household service: the
rendering vat, the clarified wax, the braided and primed wick, the dipped and
moulded candle, the oil lamp, the horn-paned lantern, the light plan that puts
the ward first, the ration that stays fair, and the snuffer that ends a day
deliberately instead of letting it gutter out.

### 1.2 The five loops it adds

```
  Render ──► Clarify ──► Wick ──► Shape ──► Burn
     │          │          │        │         │
     ▼          ▼          ▼        ▼         ▼
  tallow,   clarity     braid,   dip,     burn test,
  beeswax   grades      prime    mould    hours, grams
                                        │
                                        ▼
                          Light ──► Ration ──► Snuff ─► Record
```

### 1.3 What the player manages

1. **The fats.** Tallow and beeswax sources, lots, and purity.
2. **The vat.** Rendering, yield, smell, and fire rules.
3. **The clarity.** Filtering, grades, and storage.
4. **The wicks.** Fibre, ply, priming, and char behavior.
5. **The bench.** Dipping, moulding, blends, and burn tests.
6. **The lamps.** Oil lamps, reservoirs, trims, and smoke.
7. **The lanterns.** Frames, horn panes, and carriage rules.
8. **The plan.** Which rooms have light, and until when.
9. **The ration.** Fair shares when wax is short.
10. **The ledger.** Burn tests, counts, snuff times, and lessons.

### 1.4 What it is not

- Not a power or electric-light system; the grid keeps its authority.
- Not a chemistry system; soap, solvents, and refined wax stay with 39.
- Not an alarm or signal system; lamps never message.
- Not a sleep system; quiet hours belong to expansion 41.
- Not a fire system; hazards and suppression belong to expansion 47.
- Not a feast or festival system; the calendar keeps its evenings.
- Not a currency; candles are never money and never gambled.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` | Recipes and jobs | `LIVE` |
| `Assets/Ashfall.Core/Crafting/ShelterWorkshopSystem.cs` | Bench and condition | `LIVE` |
| `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` | Zones, ignition, suppression | `LIVE` |
| `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` | Bees and wax | `LIVE` |
| `Assets/Ashfall.Core/Shelter/ShelterPowerSystem.cs` | Electric light | `LIVE` (boundary) |
| `Assets/Ashfall.Core/NeedsSystem.cs` | Morale sink | `LIVE` |
| `Assets/Ashfall.Core/ShelterThermalSystem.cs` | Heat and fuel context | `LIVE` |
| `Assets/Ashfall.Core/EquipmentConditionSystem.cs` | Tool wear | `LIVE` |
| `StandingRecord` | Records | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Rows | Notes |
|---|---|---|
| `tallow_rendering_vat_logs.json` | 4+ entries, 3,516 B | yields; unread |
| `beeswax_clarification_records.json` | 4+ entries, 3,756 B | clarity grades; unread |
| `wick_braiding_priming_reports.json` | 4+ entries, 3,195 B | ply and priming; unread |
| `candle_dip_mould_assays.json` | 4+ entries, 3,099 B | methods, burn hours; unread |
| `beeswax_rendering_dipping_assays.json` | 4+ entries, 5,011 B | purity, burn rate; unread |
| Candle, lamp, light catalogs | **0** | confirmed absent |
| Candle, wick, or lamp items in `items.json` | **0** | `item_beeswax_block` and `item_signal_lamp_module` only |

### 2.3 Confirmed gaps

- **GAP-60-1 — No candle, wick, lamp, or lamp-oil item exists.**
- **GAP-60-2 — No tallow or wax rendering loop.**
- **GAP-60-3 — No clarification, clarity grades, or storage.**
- **GAP-60-4 — No wick fibre, ply, or priming practice.**
- **GAP-60-5 — No dipping, moulding, or blend standards.**
- **GAP-60-6 — No burn test, burn hours, or consumption rate.**
- **GAP-60-7 — No lamp catalog, reservoir, trim, or smoke rule.**
- **GAP-60-8 — No lantern frames or horn panes in use.**
- **GAP-60-9 — No light plan, ration, or fair-share rule.**
- **GAP-60-10 — Five authored chandlery logs are read by nothing.**

### 2.4 Non-duplication statement

This expansion adds **no** second power, chemistry, sleep, fire, alarm, signal,
food, morale, or record system. It consumes wax from the live apiculture loop
and tallow from livestock, buys refined products from 39, registers every
burning object with `ShelterFireHazardSystem`, obeys the quiet-hour owners,
ranks rooms by need rather than by status, and files its ledger with
`StandingRecord`. All new state is additive inside the existing `crafting`
save owner. No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Light is a service, not a luxury.** The ward gets light before the
hall, and the corridor gets light before the office.

**Pillar 2 — A candle is a measured thing.** Every candle has a burn test, a
rate, and an expected number of evenings, and the shelter plans with them.

**Pillar 3 — Fire kept is fire paid for.** Every burning thing has a place, a
stand, a snuffer, and a rule.

**Pillar 4 — Wax is a season.** Beeswax belongs to the bees, tallow to the
herd, and the light plan belongs to whoever is awake at two in the morning.

**Pillar 5 — The day should end on purpose.** The snuffer is the expansion's
small ceremony: light is put out, not left to gutter.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Rendering | Yield, smell, rota | Gross-out |
| Clarification | Grades, patience, storage | Alchemy |
| Wicks | Ply, priming, char test | Mysticism |
| Candles | Dip, mould, burn test | Romance props |
| Lamps | Reservoir, trim, smoke | Luxury interiors |
| Lanterns | Panes, carriage, corridors | Fairy lights |
| Ration | Fair shares, posted rules | Shame or status |
| Snuffer | End the day | Fireworks |

### 3.3 Content limits

- No real candle firms, monasteries, or brands copied.
- No candle-as-currency, wager, or market mechanics.
- No shame for using light or for failing to ration.
- No fire spectacle; the brigade's rules are absolute and dull.
- No sleep-discipline content; the quiet owners decide the hours.
- No mystical flame, ritual candle, or sacred-oil framing.
- No new save section.

---

## 4. THE WICK WORLD

### 4.1 Interior rooms

- **`room_rendering_yard`** — vats, fire rules, and the yield board.
- **`room_clarity_room`** — filters, settling shelves, and grade jars.
- **`room_wick_bench`** — fibre, braid, priming pot, and char tests.
- **`room_dip_bench`** — dipping frame, moulds, and the cooling rack.
- **`room_lamp_nook`** — reservoirs, trims, snuffers, and spare wicks.
- **`room_lantern_store`** — frames, horn panes, and carriage rules.
- **`room_light_plan_desk`** — the plan, the ration, and the dark-day ledger.
- **`room_evening_hall`** — the lit table and the reading corner.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_wax_meadow` | The Wax Meadow | 5 | Hives and forage |
| `loc_tallow_yard` | The Tallow Yard | 5 | Rendering in open air |
| `loc_lantern_walk` | The Lantern Walk | 3 | Corridor above ground |
| `loc_oilseed_press` | The Oilseed Press | 5 | Seed oil for lamps |
| `loc_snuffer_stone` | The Snuffer Stone | 4 | Outdoor lights and rules |
| `loc_dark_watch_post` | The Dark Post | 6 | Night watch boundary |
| `loc_pane_shed` | The Pane Shed | 4 | Horn panes waiting |
| `loc_old_chandlery` | The Old Chandlery | 6 | Ruined shop, moulds |
| `loc_lamp_repair_mark` | The Lamp Mark | 4 | Outdoor repair bench |
| `loc_reading_table` | The Reading Table | 3 | The most-wanted light |

All locations must resolve in `locations.json` and pass the map loader gate.

### 4.3 The rhythm

Render in the cool months, clarify and store in winter, wick and dip on the
short days, test every blend, and light the plan through the year. When the
grid fails, the plan becomes the shelter's calendar of hours.

---

## 5. MAIN STORYLINE — "THE LIGHT WE CARRY"

### 5.1 Central conflict

**Maud Farrow** renders tallow and has been told her vat is a smell; **Rilly
Kest** braids wicks by hand and cannot keep up with demand nobody has
counted; **Dora Pelt** knows the beeswax chain cold and watches the apothecary
take the clean wax while the shelter drips tallow candles that smoke; **Nim
Gale** walks the corridors at night and knows exactly which stairwells are
dark; **Ashe Corl** is asked to rank the rooms by light and refuses until
somebody writes the rule down; **Marlow Kest** burns candles in a test stand
and turns the shelter's opinions into numbers.

Then the grid fails for nine days in the shortest month. The ward runs out of
good light on the third night and operates by two borrowed lamps; the
corridors are dark enough that a nurse falls on the stairs; the ration is
improvised and unfair and the quarter argues about it in a way that nobody
enjoys. When the grid returns, the shelter finally accepts the light plan, the
rendering rota, the clarity grades, and the rule that the ward's lamp is lit
before anyone's reading table.

The expansion's question: **who gets to be awake in the dark?**

### 5.2 Theme (unspoken)

**A shelter's evenings are made of wax, patience, and a fair rule.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_chandler_maud_farrow` | Maud Farrow | Chandler | Bench, blends, counts |
| `npc_wickmaker_rilly_kest` | Rilly Kest | Wick braider | Fibre, ply, priming |
| `npc_renderer_dora_pelt` | Dora Pelt | Renderer | Vats, yield, fire rules |
| `npc_lamplighter_nim_gale` | Nim Gale | Lamplighter | Corridors, snuffing, trims |
| `npc_lightclerk_ashe_corl` | Ashe Corl | Light clerk | Plan, ration, dark ledger |
| `npc_burntester_marlow_kest` | Marlow Kest | Burn tester | Rates, hours, standards |
| `npc_fireliaison_verne_farrow` | Verne Farrow | Fire liaison | Zones, stands, rules |
| `npc_apprentice_tild_kest` | Tild Kest | Apprentice | Dips, counts, lessons |

### 5.4 Story beats (15)

1. **The Four Candles.** The mess hall has four and a rule.
2. **The Vat Smell.** Rendering is moved outdoors.
3. **The Clean Wax.** The apothecary's share is renegotiated.
4. **The Braid.** Rilly's ply count is written down.
5. **The Dip.** The first true candle comes off the frame.
6. **The Test.** Marlow's stand turns opinions into hours.
7. **The Lump.** A blend smokes and is pulled.
8. **The Pane.** Horn panes arrive from the bone shop.
9. **The Nine Days.** The grid fails in the shortest month.
10. **The Third Night.** The ward runs out of good light.
11. **The Stair.** A fall makes the dark a safety item.
12. **The Ration.** Ashe's plan is posted and argued.
13. **The Rule.** The ward's lamp is lit first.
14. **The Snuffer.** The snuffer stone becomes a habit.
15. **The Light We Carry.** Lanterns are carried, not stored.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Wax | beeswax first / tallow first / mixed | quality |
| Plan | ward first / fair share / equal rooms | priority |
| Ration | hours / candles / both | method |
| Lamps | oil / candle / both | supply |
| Lanterns | corridors / outdoors / both | safety |
| Snuffing | posted time / last out / no rule | discipline |
| Ledger | full / counts only / minimal | honesty |
| Final | light service / family wax / ward priority | identity |

### 5.6 Endings (5 + fade)

1. **The Long Evening Light** — the light plan holds through winter, and the
   hall's evening is longer than the sun's.
2. **The Wax Year** — beeswax candles are made in quantity, and the hives are
   treated as a light source, not a luxury.
3. **The Lantern Walk** — lit corridors and carried lanterns make the dark
   weeks ordinary and safe.
4. **The Fair Ration** — the posted rule survives every argument, and nobody
   hoards.
5. **The Ward First** — the ward's lamp is lit first every night, and the
   shelter treats that as the plan working.
6. **Fade** — a snuffer set down on stone, a lamp being carried down a
   corridor, and a table where four people read by one flame.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_wick_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_wick_four_candles`, `quest_wick_vat_smell`, `quest_wick_clean_wax`,
`quest_wick_braid`, `quest_wick_dip`, `quest_wick_test`, `quest_wick_lump`,
`quest_wick_pane`, `quest_wick_nine_days`, `quest_wick_third_night`,
`quest_wick_stair`, `quest_wick_ration`, `quest_wick_rule`,
`quest_wick_snuffer`, `quest_wick_light_we_carry`.

### 6.2 Side quests (30)

**Fats (5)**
- `quest_wick_tallow` — tallow collected
- `quest_wick_beeswax` — wax lot traded
- `quest_wick_lots` — lots marked
- `quest_wick_purity` — purity checked
- `quest_wick_store` — cool store kept

**Render (5)**
- `quest_wick_render` — vat run
- `quest_wick_yield` — yield weighed
- `quest_wick_fire_rules` — rules read
- `quest_wick_rota` — rota posted
- `quest_wick_filter` — first filtering

**Clarify (5)**
- `quest_wick_settle` — settling done
- `quest_wick_grade` — grades assigned
- `quest_wick_bleach` — first clarity
- `quest_wick_jar` — storage jars filled
- `quest_wick_spoil` — spoiled lot pulled

**Wick and shape (5)**
- `quest_wick_fibre` — fibre chosen
- `quest_wick_ply` — ply count tested
- `quest_wick_prime` — priming done
- `quest_wick_dipping` — dips made
- `quest_wick_mould` — moulds poured

**Light (5)**
- `quest_wick_burn_test` — burn test run
- `quest_wick_lamp` — lamp trimmed
- `quest_wick_lantern` — lantern assembled
- `quest_wick_plan` — light plan posted
- `quest_wick_dark_ledger` — dark days recorded

**Ration and care (5)**
- `quest_wick_ward_lamp` — ward lamp first
- `quest_wick_reading` — reading light table
- `quest_wick_snuffer` — snuffing habit
- `quest_wick_fairness` — quarter review
- `quest_wick_lesson` — lesson kept

### 6.3 Repeatable quests (8)

`quest_wick_repeat_render`, `quest_wick_repeat_wick`,
`quest_wick_repeat_dip`, `quest_wick_repeat_test`,
`quest_wick_repeat_light`, `quest_wick_repeat_snuff`,
`quest_wick_repeat_count`, `quest_wick_repeat_teach`.

### 6.4 Dynamic hooks

Live events (grid failures, fire checks, apiculture wax yields, livestock
slaughter, ward demand, quiet hours, feast evenings, storm darkness) attach
authored follow-ups through existing seams. No new event bus.

### 6.5 Constraints

- Power and electric light stay with the grid owners.
- Chemistry and refined wax stay with expansion 39.
- Fire zones, ignition, and suppression stay with expansion 47.
- Quiet hours stay with expansion 41.
- Morale and stress use `NeedsSystem.Modify` only.
- No currency, wager, or market mechanics for light.
- Records file with `StandingRecord`.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `WaxLotSystem` (new, thin, `Ashfall.Core.Crafting`)

**Owns:** lots, sources, purity, marking, and cool storage for tallow and
beeswax. **Consumes:** livestock tallow, apiculture wax, trade lots. **Data:**
`wax_lots.json`. **Rules:** every lot has a source and a purity reading; beeswax
and tallow are stored separately; a spoiled lot is pulled and recorded.

### 7.2 `RenderingSystem` (new, `Ashfall.Core.Crafting`)

**Owns:** rendering vats, melt, yield, smell, and fire rules. **Consumes:**
lots, fuel, `ShelterFireHazardSystem`. **Data:** `rendering_runs.json`.
**Rules:** the vat runs outdoors or under the fire rules; yield is weighed and
recorded; a run that smells past the corridor line is moved.

### 7.3 `ClarificationSystem` (new, thin, `Ashfall.Core.Crafting`)

**Owns:** settling, filtering, clarity grades, jarring, and spoilage. **Data:**
`clarification_records.json`. **Rules:** clarity is graded and visible;
"clean enough for a ward" is a higher grade than "clean enough for a corridor";
spoilage is checked on a rota.

### 7.4 `WickSystem` (new, thin, `Ashfall.Core.Crafting`)

**Owns:** fibre, braid ply, priming, and char tests. **Data:**
`wick_specs.json`. **Rules:** ply count is specified per candle size; every
wick batch is char-tested; a wick that curls is remade.

### 7.5 `CandleBenchSystem` (new, `Ashfall.Core.Crafting`)

**Owns:** dips, moulds, blends, cooling, counts, and burn tests. **Consumes:**
wax, wicks, bench. **Data:** `candle_blends.json`, `burn_tests.json`.
**Rules:** every blend is burn-tested; burn rate is recorded in grams per hour;
a smoking blend is reformulated, never shipped.

### 7.6 `LampSystem` (new, thin, `Ashfall.Core.Crafting`)

**Owns:** lamps, reservoirs, trims, oil, smoke, and snuffers. **Consumes:**
oilseed press, bowl makers, lamp repair. **Data:** `lamp_catalog.json`,
`lamp_oil.json`. **Rules:** lamps are trimmed and filled on a rota; a smoking
lamp is pulled and cleaned; every lamp has a stand and a snuffer.

### 7.7 `LanternSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** lantern frames, horn panes from expansion 59, carriage rules, and
outdoor use. **Consumes:** joinery frames, bone-shop panes. **Data:**
`lanterns.json`. **Rules:** a lantern is carried by the handle, never set
down on wood; panes are replaced, not repaired with wax; outdoor use follows
the fire rules.

### 7.8 `LightPlanSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** the plan, the ration, the priority list, the dark-day ledger, and
the fair-share rule. **Consumes:** room needs, ward demand, quiet hours,
population. **Data:** `light_plan.json`, `light_records.json`. **Rules:** the
ward is always first; the corridor outranks the office; the ration is posted
before it is enforced; nobody is shamed for needing light.

### 7.9 Systems explicitly not added

- No second power, chemistry, sleep, fire, alarm, signal, or food system.
- No candle currency, wager, or trade mechanics.
- No magical flame, sacred oil, or ritual candle.
- No light-quality tier that shames a room's residents.
- No new RNG stream beyond the live tick, weather, and demand paths.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `wax_lots.json` (new)

```json
{
  "schema_version": 1,
  "lots": [
    {
      "lot_id": "lot_beeswax_greenhouse_611",
      "kind": "beeswax",
      "source": "apiculture",
      "purity_pct": 94,
      "grade": "candle",
      "stored_room": "room_clarity_room",
      "marked": true,
      "tags": ["live_source"]
    }
  ]
}
```

### 8.2 `rendering_runs.json` (new)

Runs: lot, vat, melt point, yield, location, fire check.

### 8.3 `clarification_records.json` (new)

Records: lot, method, clarity grade, jar, spoilage check.

### 8.4 `wick_specs.json` (new)

Specs: fibre, ply, size, priming wax, char result, candles-per-hank.

### 8.5 `candle_blends.json` (new)

Blends: wax ratio, wick spec, burn hours, smoke, drip, use.

### 8.6 `burn_tests.json` (new)

Tests: blend, grams per hour, hours, smoke, verdict, note.

### 8.7 `lamp_catalog.json` (new)

Lamps: reservoir, wick size, trim, brightness, smoke, stand.

### 8.8 `lamp_oil.json` (new)

Oils: source, press, yield, smoke, burn, storage.

### 8.9 `lanterns.json` (new)

Lanterns: frame, pane, brightness, carriage rule, location.

### 8.10 `light_plan.json` / `light_records.json` (new)

Plan: room, priority, hours, ration, review. Records: day, grid state,
lights lit, snuff time, note, lesson.

### 8.11 Items

New items appended to `items.json`: `item_tallow_lot`,
`item_beeswax_lot`, `item_clarified_wax`, `item_wick_hank`,
`item_primed_wick`, `item_candle_dipped`, `item_candle_moulded`,
`item_candle_mould`, `item_dipping_frame`, `item_burn_test_stand`,
`item_oil_lamp`, `item_lamp_oil_flask`, `item_lamp_wicks`,
`item_lantern_frame`, `item_lantern_horn_pane`, `item_candle_snuffer`,
`item_clarity_jar`, `item_light_plan_sheet`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`crafting` remains the live save owner. Lots, runs, clarity jars, wick
batches, candles, lamps, lanterns, the plan, and the ledger are additive
sub-objects. Fire-zone registrations ride `shelter_fire`. No new save section.

### 9.2 State to persist

- Lots held, kind, purity, and storage.
- Rendering runs, yields, and fire checks.
- Clarity grades and spoilage.
- Wick batches and char tests.
- Candle batches, blends, counts, and burn tests.
- Lamps, reservoirs, oil, and trim state.
- Lanterns and panes.
- The light plan, ration, and dark-day ledger.

### 9.3 Determinism

- Wax and tallow yields follow live apiculture and herd outputs.
- Rendering yield is authored per lot and volume.
- Burn hours derive from authored grams-per-hour rates, not noise.
- The light plan is a deterministic priority sort with authored tie-breaks.
- Grid failure lighting follows the live grid and weather paths.
- Paired replay hashes must match; no `System.Random`.

### 9.4 Migration

Legacy saves load with four candles and no plan; the ledger starts empty and
the ward keeps its borrowed lamps. Grid-failure state loads with the plan
unposted and the default priority (ward, corridor, kitchen, hall) active.
No lot grants are retroactive.

### 9.5 Checksum

Invariant-culture floats for purity, yield, and burn rates; integer counts,
hours, and grades.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `WickPanel` (new) | Bench, lots, counts | `WickHostSession` |
| `RenderingPanel` (new) | Vats and fire rules | same |
| `ClarityPanel` (new) | Jars and grades | same |
| `CandleTestPanel` (new) | Burn tests and blends | same |
| `LampPanel` (new) | Lamps, oil, trims | same |
| `LanternPanel` (new) | Frames and panes | same |
| `LightPlanPanel` (new) | Plan and ration | same |
| `DarkLedgerPanel` (new) | Dark-day record | same |
| `ShelterFirePanel` (extend) | Burning objects | existing |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Priorities are shown with their reason, never as a rank of people.
- Ration text is posted before enforcement and never shames a room.
- Burn rates are shown as numbers and words, never color alone.
- Keyboard/controller close/back preserved; focus maintained on refresh.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a wick catching, a lamp glass being
set down, a snuffer closing, a dip frame lifted from the pot. No cue is
required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `CraftingSystem` | Candle-family recipes |
| `ShelterWorkshopSystem` | Bench condition |
| `ShelterFireHazardSystem` | Burning object zones |
| `ApicultureSystem` | Beeswax supply |
| 39 The Reagent (Wave 6) | Refined wax, soap, solvents |
| 21 The Grid (Wave 2) | Electric light and failure |
| 41 The Quiet (Wave 6) | Quiet hours and dark rooms |
| 47 The Brigade (Wave 8) | Fire rules, stands, suppression |
| 23 The Alarm (Wave 3) | Signal separation |
| 49 The Mirror (Wave 8) | Signal lamp separation |
| 57 The Hour (Wave 10) | Candle marks and snuff times |
| 58 The Joinery (Wave 10) | Frames and stands |
| 59 The Bone Shop (Wave 10) | Horn panes |
| 26 The Common Table (Wave 3) | Kitchen light boundary |
| 55 The Quarter (Wave 9) | Fairness review |
| 15 The Deep Root (Wave 1) | Tallow sources |
| `NeedsSystem` | Comfort via `Modify` |
| `StandingRecord` | Dark-day ledger |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm crafting, workshop, fire, apiculture,
grid, quiet, chemistry, needs, and record owners. Record file:line; change
nothing.

**Phase 1 — Data + validators.** Author the catalogs; register validators
and scanner.

**Phase 2 — Pure Core.** `WaxLotSystem`, `RenderingSystem`,
`ClarificationSystem`, `WickSystem`, `CandleBenchSystem`, `LampSystem`,
`LanternSystem`, `LightPlanSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `WickHostSession`, focused selftest coverage, fresh
journey from four candles to the posted plan.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** Multi-season soak: wax yield, burn consumption, plan
compliance, dark-day behavior, ration fairness.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Lots | 12 |
| Rendering runs | 8 |
| Clarification records | 8 |
| Wick specs | 10 |
| Blends | 12 |
| Burn tests | 14 |
| Lamps | 8 |
| Oils | 6 |
| Lanterns | 6 |
| Plan/ledger rows | 24 |
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
| Power overlap | High | Firelight only |
| Chemistry overlap | High | Feedstock boundary |
| Fire incident | High | Zone registration |
| Sleep overlap | High | Quiet owners rule |
| Ration shame | High | Posted, reason-first |
| Candle currency | High | No market mechanics |
| Determinism break | Low | Authored rates |
| Tone romance | Medium | Measurement first |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `wax_lots.json` | 12 | 3,000 |
| `rendering_runs.json` | 8 | 2,000 |
| `clarification_records.json` | 8 | 2,000 |
| `wick_specs.json` | 10 | 2,500 |
| `candle_blends.json` | 12 | 3,000 |
| `burn_tests.json` | 14 | 3,000 |
| `lamp_catalog.json` | 8 | 2,000 |
| `lamp_oil.json` | 6 | 2,000 |
| `lanterns.json` | 6 | 2,000 |
| `light_plan.json` | 24 | 3,500 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 18 | 3,000 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~56,500** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R60-1 | Power fork | Med | High | Firelight scope |
| R60-2 | Chemistry fork | Med | High | Feedstock only |
| R60-3 | Fire incident | Low | High | Zones, stands |
| R60-4 | Sleep conflict | Med | High | Owner rules |
| R60-5 | Ration shame | Med | High | Reason-first |
| R60-6 | Currency creep | Low | High | No market |
| R60-7 | Determinism | Low | High | Authored |
| R60-8 | Tone romance | Med | Medium | Numbers first |
| R60-9 | Smell gross-out | Low | Medium | Plain work |
| R60-10 | Content overrun | Med | Medium | Budget |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Who owns the light plan?** Recommended: the light clerk with a posted
   rule; no room may unilaterally hold light.
2. **Is light rationed in hours or in candles?** Recommended: hours for rooms,
   counts for ward stock, with both posted.
3. **Does the ward get a permanent lamp?** Recommended: yes; it is the plan's
   first priority and the story's moral center.
4. **May families keep private wax?** Recommended: yes, from their own
   permitted lots, with no resale.
5. **What happens to the ration when the wax runs out?** Recommended: the plan
   reduces hours in reverse priority order; nobody's reading table is cut
   before the corridors are dimmed.

---

## 17. APPENDIX D — LOT TABLE

| # | Lot | Kind | Source | Purity | Grade | Store |
|---|---|---|---|---|---|---|
| 1 | B-611-a | beeswax | apiary | 94% | candle | clarity room |
| 2 | B-611-b | beeswax | apiary | 91% | candle | clarity room |
| 3 | B-612-a | beeswax | apiary | 96% | ward | clarity room |
| 4 | T-611-a | tallow | herd | 85% | corridor | cool store |
| 5 | T-611-b | tallow | herd | 78% | render | cool store |
| 6 | T-612-a | tallow | hunt | 82% | corridor | cool store |
| 7 | T-612-b | tallow | hunt | 74% | render | cool store |
| 8 | M-612-a | mixed | trade | 88% | hall | cool store |
| 9 | B-613-a | beeswax | apiary | 97% | ward | clarity room |
| 10 | T-613-a | tallow | herd | 86% | candle | cool store |
| 11 | S-613-a | seed oil | press | 92% | lamp | oil store |
| 12 | S-613-b | seed oil | press | 88% | lamp | oil store |

Twelve lots and the third row is the one the light plan cares about: a
ninety-six percent beeswax lot is the only material the shelter will burn
beside a patient, and the apothecary's claim on the clean wax becomes the
story's first real negotiation. The eleventh and twelfth rows feed the lamps,
which matters because oil is the bridge between beeswax scarcity and corridor
safety.

---

## 18. APPENDIX E — RENDERING TABLE

| # | Run | Lot | Vat | Melt °C | Yield g | Location | Fire check |
|---|---|---|---|---|---|---|---|
| 1 | R-611-1 | T-611-a | vat 1 | 48 | 4,200 | yard | yes |
| 2 | R-611-2 | T-611-b | vat 1 | 52 | 3,600 | yard | yes |
| 3 | R-612-1 | T-612-a | vat 2 | 47 | 3,900 | yard | yes |
| 4 | R-612-2 | T-612-b | vat 2 | 55 | 3,100 | yard | yes |
| 5 | R-612-3 | B-612-a | vat 3 | 63 | 2,800 | clarity room | yes |
| 6 | R-613-1 | T-613-a | vat 1 | 49 | 4,400 | yard | yes |
| 7 | R-613-2 | B-613-a | vat 3 | 62 | 3,000 | clarity room | yes |
| 8 | R-613-3 | M-612-a | vat 2 | 51 | 3,300 | yard | yes |

Eight runs and the fifth is the one that moves indoors: beeswax renders at a
higher temperature in a clean room because the apothecary and the ward both
depend on its purity. Every row carries a fire check because a rendering vat
is a pot of hot fat over a flame, which the fire owner treats as exactly what
it is.

---

## 19. APPENDIX F — CLARITY TABLE

| # | Grade | Look | Use | Check |
|---|---|---|---|---|
| 1 | Ward | pale, clear | ward lamps | weekly |
| 2 | Candle | light haze | candles | monthly |
| 3 | Hall | amber | hall dips | monthly |
| 4 | Corridor | dark | corridor lamps | quarterly |
| 5 | Render | cloudy | soap (39) | quarterly |
| 6 | Pulled | spoiled | none | immediately |

Six grades and the first is the plan's highest standard: a ward grade wax is
the cleanest thing the shop makes and it exists because a lamp beside a
patient should not smoke. The sixth row is the shop's honesty — a pulled lot
is recorded, and the record shows how the lot spoiled so the rota can change.

---

## 20. APPENDIX G — WICK TABLE

| # | Spec | Fibre | Ply | Size | Prime | Char result |
|---|---|---|---|---|---|---|
| 1 | W-1 | flax | 2 | thin | wax | curls |
| 2 | W-2 | flax | 3 | thin | wax | clean |
| 3 | W-3 | flax | 3 | medium | wax | clean |
| 4 | W-4 | hemp | 2 | medium | tallow | smokes |
| 5 | W-5 | hemp | 3 | medium | tallow | clean |
| 6 | W-6 | cotton | 2 | thin | wax | clean |
| 7 | W-7 | cotton | 4 | thick | wax | clean |
| 8 | W-8 | flax | 4 | thick | wax | clean |
| 9 | W-9 | hemp | 4 | thick | tallow | clean |
| 10 | W-10 | mixed | 3 | medium | wax | uneven |

Ten specs and the first is a failure the shop keeps on the wall: a two-ply
flax wick curls into the wax and drowns its own flame, and the fix is not
talent but a third ply. The tenth row documents a mixed-fibre wick that burns
unevenly so nobody tries it again, which is what a spec sheet is for.

---

## 21. APPENDIX H — BLEND TABLE

| # | Blend | Ratio | Wick | Burn h | Smoke | Use |
|---|---|---|---|---|---|---|
| 1 | B-1 tallow plain | 100/0 | W-5 | 6.5 | low | corridor |
| 2 | B-2 tallow firm | 95/5 | W-5 | 7.0 | low | corridor |
| 3 | B-3 beeswax plain | 0/100 | W-3 | 9.0 | none | ward |
| 4 | B-4 beeswax firm | 10/90 | W-3 | 8.5 | none | ward |
| 5 | B-5 half | 50/50 | W-3 | 8.0 | low | hall |
| 6 | B-6 half slow | 50/50 | W-7 | 10.5 | low | hall |
| 7 | B-7 tallow soft | 100/0 | W-8 | 8.0 | medium | store |
| 8 | B-8 beeswax slow | 5/95 | W-7 | 11.0 | none | reading |
| 9 | B-9 lamp oil | oil | W-2 | 14.0 | low | lamps |
| 10 | B-10 lantern | 30/70 | W-3 | 9.5 | none | lanterns |
| 11 | B-11 smoke test | 100/0 | W-4 | 5.0 | high | pulled |
| 12 | B-12 emergency | any | W-5 | 4.0 | any | dark days |

Twelve blends and the eighth is the reading table's candle: a slow beeswax
burn that gives eleven hours of steady light. The eleventh row is the blend
the shop burns once on purpose to teach the difference between a candle and a
smoke source, and the twelfth is the dark-day blend that exists for exactly
the nine nights the story's grid fails.

---

## 22. APPENDIX I — BURN TEST TABLE

| # | Blend | g/hour | Hours | Smoke | Verdict | Note |
|---|---|---|---|---|---|---|
| 1 | B-1 | 8.4 | 6.5 | low | pass | corridor stock |
| 2 | B-2 | 7.9 | 7.0 | low | pass | standard |
| 3 | B-3 | 6.2 | 9.0 | none | pass | ward stock |
| 4 | B-4 | 6.8 | 8.5 | none | pass | ward stock |
| 5 | B-5 | 7.1 | 8.0 | low | pass | hall |
| 6 | B-6 | 5.4 | 10.5 | low | pass | hall slow |
| 7 | B-7 | 9.8 | 8.0 | medium | review | store only |
| 8 | B-8 | 5.1 | 11.0 | none | pass | reading |
| 9 | B-9 | 22.0 | 14.0 | low | pass | lamp |
| 10 | B-10 | 5.9 | 9.5 | none | pass | lantern |
| 11 | B-11 | 11.2 | 5.0 | high | fail | pulled |
| 12 | B-12 | 10.0 | 4.0 | any | pass | emergency |
| 13 | B-8 variant | 4.8 | 11.5 | none | pass | better | 
| 14 | B-3 variant | 6.0 | 9.5 | none | pass | better |

Fourteen tests and the last two rows show the trade improving its own
standards across a year, half a gram at a time. The eleventh is the only
failure and it stays in the table on purpose: a record that hides its failures
cannot be used to plan a winter.

---

## 23. APPENDIX J — LAMP TABLE

| # | Lamp | Reservoir | Wick | Brightness | Smoke | Stand |
|---|---|---|---|---|---|---|
| 1 | Ward lamp | 200 ml | W-2 | high | low | bracket |
| 2 | Corridor lamp | 300 ml | W-2 | medium | low | hook |
| 3 | Hall lamp | 500 ml | W-2 | high | low | pedestal |
| 4 | Desk lamp | 100 ml | W-2 | medium | low | base |
| 5 | Kitchen lamp | 250 ml | W-2 | high | medium | bracket |
| 6 | Store lamp | 150 ml | W-2 | low | low | hook |
| 7 | Guest lamp | 120 ml | W-2 | medium | low | base |
| 8 | Repair lamp | 180 ml | W-2 | high | low | arm |

Eight lamps and the first is the plan's priority: the ward's two-hundred-
millilitre lamp burns through a night shift and is filled before anything else
draws oil. Every lamp has a stand and a snuffer, and the fire liaison signs the
stand list quarterly.

---

## 24. APPENDIX K — OIL TABLE

| # | Oil | Source | Press | Yield | Smoke | Storage |
|---|---|---|---|---|---|---|
| 1 | Seed oil A | oilseed | press 1 | 1.2 L/batch | low | tin |
| 2 | Seed oil B | oilseed | press 1 | 1.0 L/batch | low | tin |
| 3 | Fish oil | river catch | press 2 | 0.8 L/batch | medium | sealed |
| 4 | Tallow oil | herd | render | 0.9 L/batch | medium | tin |
| 5 | Mixed lamp | mixed | press 2 | 1.1 L/batch | low | tin |
| 6 | Emergency | any | any | any | any | flask |

Six oils and the third is the one that gets a rule: fish oil lights but it
smells, so it burns outdoors or in the store and never beside a patient. The
sixth row is the stub flask kept in the dark-watch post, because the person
on the stairs at night should never be carrying an empty lamp.

---

## 25. APPENDIX L — LANTERN TABLE

| # | Lantern | Frame | Pane | Brightness | Carry rule | Location |
|---|---|---|---|---|---|---|
| 1 | Corridor lantern | oak | horn A | medium | handle only | stair core |
| 2 | Walk lantern | larch | horn A | medium | handle only | lantern walk |
| 3 | Gate lantern | oak | horn B | high | hung | guest gate (56) |
| 4 | Ward lantern | oak | horn A | high | carried, set high | ward |
| 5 | Night watch lantern | larch | horn B | medium | handle only | dark post |
| 6 | Store lantern | softwood | horn B | low | hung | cellar |

Six lanterns and the fourth is the one the ward treats as equipment: a
carried lantern with a horn pane from the bone shop that will not shatter
when a nurse sets it down in a hurry. Every carriage rule says handle only
because the fire owner's first question about any lantern is where the person
puts it when their hands are full.

---

## 26. APPENDIX M — LIGHT PLAN TABLE

| # | Room | Priority | Hours | Ration | Review |
|---|---|---|---|---|---|
| 1 | Ward | 1 | all night | lamp + candles | weekly |
| 2 | Stair core | 2 | all night | 2 lamps | weekly |
| 3 | Corridors | 3 | evening | 4 lamps | monthly |
| 4 | Kitchen | 4 | pre-dawn | 1 lamp | monthly |
| 5 | Hall | 5 | evening | 3 candles | monthly |
| 6 | Workshop | 6 | as needed | 1 lamp | monthly |
| 7 | Office | 7 | working hours | 1 candle | quarterly |
| 8 | Guest room | 8 | evening | 1 candle | quarterly |
| 9 | Reading table | 9 | evening | 1 slow candle | monthly |
| 10 | Store | 10 | as needed | 1 lantern | quarterly |

Ten rooms and the plan's order is the expansion's moral argument written as a
table: the ward first, the stairs second, then the corridors that keep people
safe, and only then the rooms where people would like to be. The ninth row is
the shelter's favorite and the first thing cut in a hard winter, which is
what makes it precious.

---

## 27. APPENDIX N — DARK LEDGER TABLE

| # | Day | Grid | Lit | Snuffed | Note | Lesson |
|---|---|---|---|---|---|---|
| 1 | 40 | on | 18 | 23:00 | plan posted | — |
| 2 | 41 | on | 18 | 23:00 | plan holds | — |
| 3 | 55 | down | 12 | 02:00 | ward kept lit | ward lamp priority |
| 4 | 56 | down | 10 | 01:30 | stair fall | stair rank raised |
| 5 | 57 | down | 9 | 01:00 | ration tight | emergency blend |
| 6 | 58 | down | 9 | 00:30 | oil low | seed oil pressed |
| 7 | 59 | down | 11 | 23:30 | ward operating | plan works |
| 8 | 60 | down | 12 | 23:00 | stable | plan repeats |
| 9 | 61 | back | 20 | 23:00 | relit | review called |
| 10 | 62 | back | 20 | 23:00 | plan revised | stair rule kept |

The tenth row is the year's best entry: the stair fall raised the stair core
from fourth to second priority, and the change stays in the plan after the
emergency ends. This is what a light plan is for — not to ration prettily, but
to turn one bad night into a permanent rule that keeps the next person on
their feet.

---

## 28. APPENDIX O — VIGNETTES (TONE SAMPLE)

> Rilly braids three plies together and primes the finished wick in a pan of
> warm wax, and the wick goes stiff and straight, and the candle it will feed
> is going to burn for nine hours, which she knows because Marlow burned
> eleven just like it in the test stand.

> The vat runs outdoors on a cold morning and the yard smells like a kitchen
> incident, and Dora keeps the fire watch standing beside it doing nothing
> but watching, which is the most important job in the shop.

> On the third night of the outage, the ward's lamp is the only one in the
> hall that is not being argued about, because the plan says the ward comes
> first and the plan was posted before anyone needed it.

> Nim walks the corridor with a lantern and puts it down only on stone, and
> at the end of the round she sets it on the snuffer stone and closes the
> snuffer and the day ends on purpose, which is the small ceremony the shop
> never planned to create.

---

## 29. APPENDIX P — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Waxy tallow | smoky light | re-render, grade |
| Poor clarit | lamp smokes | settle again |
| Two-ply wick | drowns flame | add ply |
| Smoking blend | air quality | pull, reform |
| Dry reservoir | flame guttering | fill rota |
| Untrimmed wick | soot | trim round |
| Unfair ration | quarter friction | post the rule |
| Dark stairs | injury risk | raise priority |
| Spark in store | fire risk | fire rules |
| Hoarded wax | shortage | ledger counts |

Every recovery is a rule the shop already contains, and the last row is the
expansion's anti-hoarding principle: candles are counted, not traded, and the
count is posted where anyone can read it.

---

## 31. APPENDIX Q — GLOSSARY

- **Lot** — a marked batch of tallow or wax with a source and purity.
- **Render** — melting fat to separate usable tallow from tissue.
- **Clarity grade** — how clean a wax is, and what it may be used for.
- **Ply** — the number of strands braided into a wick.
- **Prime** — stiffening a wick in warm wax so it stands straight.
- **Dip** — building a candle by repeated dipping in molten wax.
- **Burn rate** — grams of wax consumed per hour of flame.
- **Light plan** — the posted priority list of rooms and hours.
- **Dark ledger** — the record of grid-failure nights and their lessons.
- **Snuffer** — the small cone that ends a flame on purpose.

---

## 32. APPENDIX R — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `WaxLotSystem` | wax, tallow | lots | animals |
| `RenderingSystem` | fire rules | runs, yield | fire state |
| `ClarificationSystem` | lots | grades, jars | chemistry |
| `WickSystem` | fibre | specs | candles |
| `CandleBenchSystem` | wax, wicks | candles, tests | light plan |
| `LampSystem` | oil | lamps, trims | grid |
| `LanternSystem` | panes | lanterns | fire rules |
| `LightPlanSystem` | needs | plan, ration | rooms |
| `ApicultureSystem` | nothing | nothing | nothing |
| `ShelterFireHazardSystem` | zones | zones | nothing |
| `ShelterPowerSystem` | nothing | nothing | nothing |
| `QuietHoursSystem` | nothing | nothing | nothing |
| `NeedsSystem` | nothing | nothing | nothing |
| `CraftingSystem` | nothing | recipes | wax |
| `StandingRecord` | records | records | nothing |

---

## 33. APPENDIX S — DATA SCHEMA DETAIL (NEW CATALOGS)

**`wax_lots.json`** — `lot_id`, `kind`, `source`, `purity_pct`, `grade`,
`stored_room`, `marked`, `tags[]`.

**`rendering_runs.json`** — `run_id`, `lot_id`, `vat`, `melt_celsius`,
`yield_grams`, `location`, `fire_checked`, `tags[]`.

**`clarification_records.json`** — `record_id`, `lot_id`, `method`, `grade`,
`jar`, `spoilage_check`, `tags[]`.

**`wick_specs.json`** — `spec_id`, `fibre`, `ply`, `size`, `prime`,
`char_result`, `per_hank`, `tags[]`.

**`candle_blends.json`** — `blend_id`, `ratio`, `wick_spec`, `burn_hours`,
`smoke`, `use`, `tags[]`.

**`burn_tests.json`** — `test_id`, `blend_id`, `grams_per_hour`, `hours`,
`smoke`, `verdict`, `note`, `tags[]`.

**`lamp_catalog.json`** — `lamp_id`, `reservoir_ml`, `wick_spec`, `brightness`,
`smoke`, `stand`, `tags[]`.

**`lamp_oil.json`** — `oil_id`, `source`, `press`, `yield_litres`, `smoke`,
`storage`, `tags[]`.

**`lanterns.json`** — `lantern_id`, `frame`, `pane`, `brightness`,
`carry_rule`, `location`, `tags[]`.

**`light_plan.json`** — `room_id`, `priority`, `hours`, `ration`, `review`,
`tags[]`.

**`light_records.json`** — `record_id`, `day`, `grid_state`, `lights_lit`,
`snuff_time`, `note`, `lesson`, `tags[]`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on
missing or duplicate IDs, invalid room, lot, item, or source references, or
out-of-range numbers. **No lot may reference a human source; the validator
rejects it.**

---

## 34. APPENDIX T — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Wax in store | reserve | Lots |
| Beeswax share | quality | Lots |
| Render yield | vat health | Runs |
| Clarity grades | practice | Jars |
| Wicks per hank | supply | Wick |
| Burn rate mean | economy | Tests |
| Smoke failures | quality | Tests |
| Lamps lit | plan | Ledger |
| Snuff times | habit | Ledger |
| Ward lamp continuity | priority | Ledger |

Telemetry is diagnostic only; it never gates content, never ranks a room, and
never turns a dark night into a personal failing.

---

## 35. APPENDIX U — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive inside `crafting` and
  `shelter_fire`.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling.
- [ ] Phase 6 content passes the review checklist in §29.
- [ ] Phase 7 soak shows a wax year, a nine-day grid failure, and a fair
  ration that is posted before it is enforced.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No currency, wager, shame, or ritual-flame content exists.

---

## 36. APPENDIX V — OPEN QUESTIONS FOR REVIEW

1. Who may move a room's priority, and must the reason be recorded?
2. Does the ward's lamp burn all night in every season, or only in crisis?
3. Are family candles drawn from the common store or from private lots?
4. How is fairness reviewed when the ration cuts someone's evening work?
5. Do lanterns leave the shelter, and under what carriage rule?
6. Is there a winter minimum of wax, and who counts it?
7. What happens to a room that hoards candles after the plan is posted?
8. Who keeps the dark ledger when the light clerk sleeps?

None of these may be decided unilaterally; each changes tone and balance.

---

## 37. APPENDIX W — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 15 The Deep Root | Tallow from the herd |
| 2 | 21 The Grid | Failures and the light plan |
| 3 | 23 The Alarm | Signal separation |
| 3 | 26 The Common Table | Kitchen light boundary |
| 4 | 29 The Glass | Lamp chimneys and bulb glass |
| 5 | 32 The Wild | Fish oil and hunt tallow |
| 6 | 39 The Reagent | Refined wax and soap |
| 6 | 41 The Quiet | Quiet hours and dark rooms |
| 8 | 47 The Brigade | Fire rules and stands |
| 8 | 48 The Pastime | Lit evenings |
| 8 | 49 The Mirror | Signal lamp separation |
| 8 | 50 The Vault | Pre-war lamp collections |
| 9 | 54 The Uninvited | Candle storage versus vermin |
| 9 | 55 The Quarter | Ration fairness review |
| 10 | 57 The Hour | Candle marks and snuff times |
| 10 | 58 The Joinery | Holds and stands |
| 10 | 59 The Bone Shop | Horn panes |

Each hook is additive. The Wick can ship alone, and every other expansion can
ship without it.

---

## 38. APPENDIX X — ENDING PROSE SKETCHES

**The Long Evening Light.** The plan holds through winter, and the hall's
sixth hour is light the sun never gave.

**The Wax Year.** Beeswax candles are made in quantity, and the hives are
treated as a light source, not a luxury.

**The Lantern Walk.** Lit corridors and carried lanterns make the dark weeks
ordinary and safe, and the stairs are never dark again.

**The Fair Ration.** The posted rule survives every argument, nobody hoards,
and the count is read aloud beside the meal bell.

**The Ward First.** The ward's lamp is lit first every night, and the shelter
treats that as the plan working.

**Fade.** A snuffer set down on stone, a lamp carried down a corridor, and a
table where four people read by one flame.

---

## 39. APPENDIX Y — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Candle currency | economy break | counts, no trade |
| Shame ration | cruelty | reason-first rule |
| Better people's light | status bias | ward and stairs first |
| Mystic flame | genre drift | measured burn |
| Endless wax | no cost | lots and yields |
| Fire spectacle | safety | brigade rules |
| Sleep policing | harm | quiet owners rule |
| Luxury interiors | tone | plain lamps |
| Hero blend | fantasy | test improvements |
| Secret formula | gatekeeping | spec sheets |

The list exists because light is seductive to write as luxury or as regimen.
The expansion's rule is that light is measured in grams per hour, rationed by
room need, posted before it is enforced, and put out with a snuffer so the day
ends on purpose.

---

## 40. APPENDIX Z — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Lots | 12 | 3,000 |
| Rendering runs | 8 | 2,000 |
| Clarification | 8 | 2,000 |
| Wick specs | 10 | 2,500 |
| Blends | 12 | 3,000 |
| Burn tests | 14 | 3,000 |
| Lamps | 8 | 2,000 |
| Oils | 6 | 2,000 |
| Lanterns | 6 | 2,000 |
| Plan and ledger | 24 | 3,500 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 18 | 3,000 |
| Endings | 6 | 3,000 |
| **Total** | | **~56,500** |

---

## 41. APPENDIX AA — FIRST WICK YEAR

| Season | Focus | Milestone |
|---|---|---|
| Spring | lots | apiculture wax and herd tallow marked |
| Spring | vat | rendering moved outdoors |
| Summer | wicks | ply specs written |
| Summer | dips | first true candle off the frame |
| Autumn | tests | burn stand built and used |
| Autumn | panes | horn panes arrive |
| Autumn | lamps | oil pressed and lamps trimmed |
| Winter | outage | nine dark days |
| Winter | ward | third-night shortage |
| Winter | stair | fall raises priority |
| Winter | plan | ration posted |
| Winter | rule | ward lamp lit first |

Twelve milestones and the shape is the plan's argument: a trade built through
the bright seasons so that the dark ones are survivable. The third-night row
is the crisis that makes the shelter care, and the eleventh is the rule that
outlasts the crisis.

---

## 42. APPENDIX AB — CREW TABLE

| # | Crew | Work | Size | Season | Check |
|---|---|---|---|---|---|
| 1 | Render crew | vats, fire watch | 2 | cool months | rules |
| 2 | Clarity crew | settle, grade, jar | 2 | winter | grades |
| 3 | Wick crew | braid, prime, char | 2 | short days | specs |
| 4 | Dip crew | dips, moulds, cooling | 3 | short days | counts |
| 5 | Test crew | burn tests | 1 | any | stand |
| 6 | Lamp crew | trim, fill, clean | 2 | any | round |
| 7 | Lantern crew | frames, panes | 1 | any | carriage |
| 8 | Light crew | plan, ration, ledger | 2 | any | posting |
| 9 | Snuffer crew | end of day | 1 | nightly | round |
| 10 | Teaching crew | apprentices | 1 | winter | sample |

Ten crews and the ninth is one person walking the corridors at the end of the
day with a snuffer, which the shelter eventually calls "the last round" and
treats as the day's closing. The tenth row keeps the bench's knowledge alive,
and the fifth is the only crew of one that the plan insists on as a separate
job, because whoever measures the light must not also be making it.

---

## 43. APPENDIX AC — TEACHING TABLE

| # | Lesson | Audience | Method | Record |
|---|---|---|---|---|
| 1 | Render safely | vat crew | walkthrough | rules |
| 2 | Grade a lot | clarity crew | jar samples | sheet |
| 3 | Braid three ply | apprentices | hands | spec |
| 4 | Dip level | dip crew | frame | counts |
| 5 | Burn test | test crew | stand | ledger |
| 6 | Trim a lamp | all | lamp | round |
| 7 | Read the plan | all | board | none |
| 8 | Snuff safely | all | last round | none |

Eight lessons and the eighth is the one the children learn first, because a
snuffer is a small cone and a flame is the most dangerous ordinary thing in
the shelter. The third is the trade's skill lesson: three ply, wax-primed,
straight in the stand, and the apprentice braids until their fingers stop
arguing.

---

## 44. APPENDIX AD — WICK CHARTER

| Clause | Promise |
|---|---|
| Measured | Every lot, batch, and candle carries a number |
| Tested | No blend is issued before it burns in the stand |
| Kept | Every flame has a stand, a snuffer, and a place |
| Fair | The ration is posted before it is enforced |
| Ordered | The ward is first, the stairs second, the rest after |
| Quiet | The plan obeys the shelter's quiet hours |
| Clean | Ward-grade wax burns beside patients and nowhere else |
| Counted | Nothing is hoarded, and the count is public |
| Taught | The bench teaches by hands, not by preference |
| Ended | The day is snuffed, not left to gutter |

The wick charter is the expansion's first-class design object, posted beside
the light plan where the ration is read. Its last clause is the one the
shelter keeps longest: a day that ends on purpose is a day the shelter chose,
and choosing is what separates an evening from a wait.

---

## 45. APPENDIX AE — BENCH SUCCESSION TABLE

| Role | First | Successor | Handover |
|---|---|---|---|
| Chandler | Maud | Tild | one wax year |
| Wick maker | Rilly | wick hand | one spec cycle |
| Renderer | Dora | render hand | one vat season |
| Lamplighter | Nim | last-round hand | one round month |
| Light clerk | Ashe | office hand | one winter plan |
| Burn tester | Marlow | test hand | one test cycle |
| Fire liaison | Verne | brigade hand | one stand list |
| Apprentice | Tild | next recruit | one dip batch |

The succession table is measured in years and rounds, and the chandler's
handover takes a full wax year because a light plan inherited in autumn has
never seen a winter. The lamplighter's row is the one the shelter notices
most: when the last-round hand changes, the corridors get brighter or darker
for a week, and everyone knows before the plan says so.

---

## 46. APPENDIX AF — REVIEW TABLE

| # | Question | Answer source | Action |
|---|---|---|---|
| 1 | Which lot burned best? | ledger | buy more |
| 2 | Which blend smoked? | tests | reform |
| 3 | Which wick drowned? | specs | drop |
| 4 | Which room ran out first? | plan | re-rank |
| 5 | Did the ward stay lit? | ledger | keep priority |
| 6 | Where was the dark worst? | rounds | add lamp |
| 7 | Was the ration fair? | quarter | revise |
| 8 | Which lamp smoked? | rounds | clean |
| 9 | What did the winter teach? | crew | one lesson |
| 10 | What is next winter's plan? | meeting | post it |

Ten questions asked once a year with the plan and the dark ledger open, and
the answers are supposed to be numbers that change one route. The tenth is
the only anticipatory one, and the plan is reposted every autumn so that
everyone walks into the dark season already knowing what the rule will be.

---

## 47. APPENDIX AG — BURN ARITHMETIC (WORKED)

A candle's life is division. A two-hundred-gram dipped candle at a burn rate
of six point two grams per hour lasts thirty-two hours, which is four evenings
at eight hours or one long ward night and a breakfast. The light clerk
maintains a simple projection: `nights = stock_grams / (rate × hours_per_night)`,
and the plan's rule is that the ward reserve is never included in that
calculation — the ward lamp's wax is held separate, and the plan works with
what is left. When the ninth-dark-day scenario is worked, the shelter's
corridor stock at eight grams per hour and four hours per evening gives
eleven nights, which is why the corridor survives and the reading table does
not.

Oil is the same arithmetic with different numbers: a three-hundred-millilitre
corridor lamp at twenty-two grams per hour lasts about twelve hours of
burning, and a trimmed wick extends that by a fifth. The plan's hidden skill
is trimming, which is why the lamplighter's round exists and why the burn
tests record soot as carefully as hours: a guttering, sooted lamp is a lamp
that eats oil and delivers darkness.

---

## 48. APPENDIX AH — TONE WATCHLIST

| Temptation | Why it fails | House rule |
|---|---|---|
| Candle hoarder | economic creep | public count |
| Romantic glow | tone | measured hours |
| Snob grade wax | status bias | need-based use |
| Pyre spectacle | safety | fire rules |
| Light as reward | unfairness | plan, not prize |
| Darkness as punishment | cruelty | priority, not penalty |
| Sacred lamp | genre drift | service only |
| Secret formula | gatekeeping | spec sheets |

The watchlist exists because warm light is the easiest emotional shorthand in
games. The expansion keeps the flame small and accountable: grams, hours,
trims, counts, and a posted rule that puts the ward first and the reading
table last, and lets the reader earn the eleventh hour anyway.

---

## 49. APPENDIX AI — SMALL MISTAKES TABLE

| Mistake | Consequence | Lesson |
|---|---|---|
| Unfiltered wax | smoky lamp | settle first |
| Over-primed wick | drips | prime lightly |
| Dipped too fast | thin shell | slow the lift |
| Mould too cold | cracked candle | warm the mould |
| Untrimmed wick | soot | trim each round |
| Forgot the stand | fire report | stand list |

Six mistakes, each costing a candle or a complaint, and the fifth is the one
the fire liaison writes into the rules: an untrimmed wick does not just soot a
ceiling, it turns a safe lamp into a lazy one, and laziness is how the
shelter's fire record gets its second incident.

---

## 50. APPENDIX AJ — WAX ECONOMY TABLE

| # | Item | In | Out | Balance | Note |
|---|---|---|---|---|---|
| 1 | Beeswax | 3.0 kg | 0 | 3.0 | apiculture |
| 2 | Beeswax to ward | 0 | 0.8 kg | 2.2 | priority |
| 3 | Beeswax to candles | 0 | 1.6 kg | 0.6 | reserve |
| 4 | Tallow | 4.2 kg | 0 | 4.2 | herd |
| 5 | Tallow to soap | 0 | 0.9 kg | 3.3 | reagent (39) |
| 6 | Tallow to candles | 0 | 2.0 kg | 1.3 | corridor |
| 7 | Seed oil | 2.2 L | 0 | 2.2 | press |
| 8 | Oil to lamps | 0 | 1.4 L | 0.8 | reserve |
| 9 | Candles made | 0 | 96 | 96 | count |
| 10 | Candles burned | 74 | 0 | 22 | winter |

The economy table is the expansion's quiet conversation with expansion 39:
the reagent works takes nine hundred grams of tallow every month for soap and
antiseptic, the ward takes clean beeswax, and the corridors get what is left.
The numbers are authored so that the shelter is always slightly short, which
is what makes the plan and the ration meaningful, and the tenth row's
twenty-two candles at year end is the reserve the plan protects.

---

## 51. APPENDIX AK — LAST ROUND TABLE

| # | Stop | Lamp | Action | Snuff | Note |
|---|---|---|---|---|---|
| 1 | Ward | ward lamp | check, fill | keep lit | all night |
| 2 | Stair core | 2 lamps | trim, check | keep lit | all night |
| 3 | Corridor east | lamp | trim | snuff | posted |
| 4 | Corridor west | lamp | trim | snuff | posted |
| 5 | Kitchen | lamp | check | snuff | pre-dawn relight |
| 6 | Hall | 3 candles | count | snuff | reading last |
| 7 | Workshop | lamp | clean | snuff | tools down |
| 8 | Store | lantern | check | snuff | door locked |
| 9 | Guest room | candle | none | guest snuffs | candle left |
| 10 | Snuffer stone | — | set down | closed | day ends |

The last round is the expansion's daily ceremony and the tenth row is its
ending: the snuffer is set down on the stone, the flame is closed, and the
round is over. The seventh row is the practical one — a workshop lamp that is
snuffed keeps the bench from becoming a fire story — and the ninth is the
polite one, because a guest's candle is a guest's choice.

---

## 52. APPENDIX AL — STORE COUNT TABLE

| # | Store | Candles | Lamps | Oil | Wick | Verified |
|---|---|---|---|---|---|---|
| 1 | Ward | 6 | 2 | 1.1 L | 20 | weekly |
| 2 | Corridor | 0 | 4 | 1.8 L | 0 | weekly |
| 3 | Hall | 12 | 1 | 0.3 L | 0 | monthly |
| 4 | Kitchen | 2 | 1 | 0.5 L | 4 | monthly |
| 5 | Workshop | 1 | 1 | 0.4 L | 2 | monthly |
| 6 | Reserve | 22 | 0 | 0.8 L | 30 | quarterly |
| 7 | Dark post | 2 | 1 | 0.3 L | 2 | weekly |
| 8 | Emergency | 4 | 0 | 0.2 L | 4 | quarterly |

The count table is posted monthly beside the plan so that nobody has to guess
whether the shelter is short, and the eighth row is the emergency stub the
store keeper keeps after learning that the worst nights are not the ones the
shelter sees coming. The first row's weekly verification is the ward's own
rule, because a nurse who cannot find a candle at three in the morning has
learned something about the plan that the plan should already know.

---

## 53. APPENDIX AM — WORKED DARK WEEK

**Day one.** The grid drops at dusk on the shortest day of the year. The plan
is already posted, so the corridors take four lamps and the ward takes its two
plus candles, and the hall gets three candles and no argument.

**Day three.** The ward uses its last ward-grade candle and Ashe moves two
beeswax candles from the hall to the ward, and the hall reads by one lamp for
the rest of the outage. Nobody complains, because the plan already said whose
light would be cut first.

**Day five.** The corridor lamp oil runs low and Nim's trim round stretches the
remaining oil by a fifth. Rilly braids wicks by the ward lamp's light and Dora
renders an emergency tallow batch in the yard under fire watch.

**Day seven.** A nurse falls on the stair between two corridor lamps and the
stair core is raised to second priority the same night. Two lamps move from the
hall to the stairs, and the hall stops reading entirely.

**Day nine.** The grid returns. The plan's persisted states are checked the
same night: the ward lamp is still lit, the reserve count is eleven candles,
the dark ledger is complete, and the ration is reposted with the stair rule
written in.

**After.** The shelter's reward for nine hard nights is not a refund: it is a
permanent change to a single line of the plan, which is what the expansion
means by learning from the dark.

---

## 54. APPENDIX AN — APICULTURE EXCHANGE TABLE

| # | Season | Hive wax g | To candles | To ward | Note |
|---|---|---|---|---|---|
| 1 | spring | 600 | 300 | 300 | first draw |
| 2 | summer | 900 | 500 | 400 | main flow |
| 3 | autumn | 700 | 400 | 300 | storage |
| 4 | winter | 200 | 100 | 100 | maintenance |
| 5 | poor year | 300 | 150 | 150 | ration |
| 6 | good year | 1,200 | 700 | 500 | reserve |

The apiculture exchange is the smallest table in the expansion and the one
that ties it to the oldest live loop in the shelter: the bees were already
making wax before anyone thought to burn it. The sixth row's good year is when
the reserve grows, and the fifth is when the plan's ration becomes visible
without anyone having done anything wrong.

---

## 55. APPENDIX AO — CORRIDOR LAMP ROUND TABLE

| # | Corridor | Lamp | Reach | Dark gap | Fix |
|---|---|---|---|---|---|
| 1 | upper east | 1 | 22 m | door recess | hook moved |
| 2 | upper west | 1 | 20 m | stair head | second hook |
| 3 | stair core | 2 | full | none | priority 2 |
| 4 | lower east | 1 | 18 m | store turn | lantern |
| 5 | lower west | 1 | 19 m | bunk turn | nothing needed |
| 6 | mess run | 1 | 25 m | none | keep |
| 7 | workshop run | 1 | 17 m | bench corner | repair lamp |
| 8 | guest run | 1 | 15 m | none | keep |

The corridor round exists because the light plan ranks rooms but people walk
routes, and Nim's rounds found every dark gap on the list. The first row's
moved hook and the fourth's lantern are the whole value of doing a walk with
a plan in hand: the numbers said the corridor was covered, and the feet said
otherwise.

---

## 56. APPENDIX AP — SNUFF ROUND TIMES

| # | Season | Last light | Snuff | Reading | Note |
|---|---|---|---|---|---|
| 1 | winter | 22:00 | 23:00 | 1 hour | longest |
| 2 | spring | 21:30 | 23:30 | 2 hours | outside work |
| 3 | summer | 22:30 | 00:00 | 1.5 hours | late sun |
| 4 | autumn | 21:00 | 23:00 | 2 hours | prep |
| 5 | outage | any | keep ward | none | plan rules |
| 6 | feast (56) | 23:00 | 00:30 | songs | calendar |

Six snuff times and the third is the one the shelter argues about every year:
a summer evening wants a later light and the morning shift wants an earlier
one, and the resolution is a posted time rather than a mood. The fifth row
delegates entirely to the plan, which is what makes an outage survivable: the
dark days have a rule already written for them.

---

## 57. CLOSING STATEMENT

ASHFALL already counted the empty extinguisher racks and the dark corridors;
expansion 41 already proposed a night lamp, expansion 39 already routed wax to
candles, and the live apiary already produces the block. What the shelter never
had is the trade in between: the vat, the clarity grade, the braid, the dip,
the burn test, the lamp trim, the horn pane, the plan, the ration, and the
snuffer. The Wick gives the shelter its evenings back as a service — measured,
fair, and put out on purpose — and answers a question that survival games
usually skip: light is not decoration. It is the thing that decides who is
awake, who is safe, and who gets to read at the end of the day.

> Wave 10 note: this plan is one of five Wave 10 expansion bibles (57–61). Each
> is self-contained; none requires another to ship. The shared Wave 10 index
> lives at `docs/expansions/wave10/WAVE10_INDEX.md`. The safe pre-signature
> step is Phase 1 (data schemas and validators), which is additive and
> reversible. Evidence anchors:
> `Assets/Ashfall.Core/Narrative/CandleMakingWaxCatalog.cs` (orphaned;
> `TallowRenderingVatLog`, `BeeswaxClarificationRecord`,
> `WickBraidingPrimingReport`, `CandleDipMouldAssay`),
> `Assets/Ashfall.Core/Crafting/CraftingSystem.cs`,
> `Assets/Ashfall.Core/Crafting/ShelterWorkshopSystem.cs`,
> `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs`,
> `Assets/Ashfall.Core/Greenhouse/ApicultureSystem.cs` (live, wax),
> `item_beeswax_block` in `items.json`, and the narrative logs
> `tallow_rendering_vat_logs.json` (3,516 B),
> `beeswax_clarification_records.json` (3,756 B),
> `wick_braiding_priming_reports.json` (3,195 B),
> `candle_dip_mould_assays.json` (3,099 B),
> `beeswax_rendering_dipping_assays.json` (5,011 B). Expansion 39's product
> table already routes wax to candles with the note "nights darker".