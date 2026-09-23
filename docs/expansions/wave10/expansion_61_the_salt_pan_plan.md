# ASHFALL — Expansion 61 Design Bible
# THE SALT PAN
### Wave 10 · Veins, Brine, Pans, Boiling, Grades, Stock Salt, Saline, Road Salt, Treaty Deliveries, and the White Business

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-24
**Domain owners touched:** `Ashfall.Core.Foundry` (`SaltMineExtractionSystem`), `Ashfall.Core` (`ExpansionHubSave.saltMine`), `Ashfall.Core.Inventory`
**Proposed host owner:** `SaltPanHostSession` (extends the live salt-mine host in `SilentFoundryHostSession`)
**Existing save sections:** `saltMine` inside `ExpansionHubSave` (v6), foundry aggregate
**Existing CLI verbs:** `--data-integrity-selftest`, `--content-utilization-selftest`, `--panel-bind-lifecycle-selftest` (no salt-specific verb exists)
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already mines salt. `SaltMineExtractionSystem` is a 454-line live
system with veins (`SaltMineVeinState`), worker assignment (`AssignWorkers`),
drill and pump maintenance (`ReplaceDrill`, `RepairPump`), power
(`SetPower`), a daily tick (`TickDaily(day, rng)`), contamination and storage,
and treaty obligations (`TreatyDeliveryRecord`, `IsTreatyFulfilled`,
`GetDeliveryCount`). It is wired: `SilentFoundryHostSession` registers veins
(`:582`) and forwards treaty events (`OnTreatyDeliveryAccepted` /
`OnTreatyDeliveryMissed`, `:208–209`); `ExpansionHubSave` persists
`SaltMineState` as a v6 addition; and `BrineExtractionPanel` already renders
vein, workers, drill, pump, contamination, and storage labels. Expansion 39's
story already names the pans: Mirren "hauls salt from the pans and brine from
the deep well." The shelter's shelves already hold `item_preservation_salt`,
`item_rock_salt_sack`, `item_trade_salt_sack`, `item_medical_saline_salt`,
and `item_chem_clarity_salts`.

What does not exist: a single salt catalog file. The veins are registered from
code (`RegisterVein`), not authored into data; there are no pans, no grades, no
boiling schedule, no brine-well map, no livestock mineral salt, no road salt,
no saline dilution, no gallery records, and no ledger. The authored mine
inscriptions — `salt_mine_inscriptions.json` (5,036 B: `mine_gallery`,
`rock_medium`, `inscription_tool`, `recorder_identity`) — are read by nothing.

**The Salt Pan** is the expansion about the white business: the veins and
galleries, the brine wells, the evaporation pans, the boiling house, the
grades, the stocks that keep meat and herds alive, the saline the ward dilutes
for infusions, the road salt that keeps the winter pass open, the treaty
deliveries the foundry's accords require, and the gallery walls where the
miners have been writing their names for a hundred years.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| `SaltMineExtractionSystem` | Veins, workers, drills, pumps | Extends with data and pan work |
| 10 The Silent Foundry (Exp 10) | Foundry, accords, treaty deliveries | Reads and fulfills; no economy authority |
| 39 The Reagent (Wave 6) | Chemistry and feedstock use | Supplies clarity salts; no chemistry |
| 26 The Common Table (Wave 3) | Food and preservation | Supplies preservation salt |
| 38 The Ward (Wave 6) | Medicine, infusions | Supplies saline-grade salt; no medicine |
| 15 The Deep Root (Wave 1) | Livestock and herds | Supplies mineral and feed salt |
| 22 The Clean Flow (Wave 3) | Water treatment | Brine boundary; no water authority |
| 52 The Warm Ground (Wave 9) | Deep wells and geothermal | Brine heat only; no well authority |
| 34 The Long Road (Wave 5) | Transport and routes | Road salt is a supply, not a route |
| 45 The Envoy (Wave 7) | Diplomacy and treaties | Deliveries are obligations already live |
| 54 The Uninvited (Wave 9) | Pests and spoilage | Salt storage protects stores |
| 55 The Quarter (Wave 9) | Living and fairness | Work conditions reviewed there |
| `PowerGridSystem` | Power | Pumps and pans draw through it |
| `NeedsSystem` | Morale and stress | Work comfort via `Modify` only |
| `StandingRecord` | Records | Files the gallery and delivery ledgers |

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The shelter's salt works run on code. The veins are registered by the host,
the workers are assigned from a panel that already shows five labels, the
drill and pump are maintained when someone notices, and the treaty deliveries
go out because the foundry's accords say they must. There are no pans, no
boiling house, no grade book, no brine map, no mineral salt for the herd, no
road salt for the pass, and no one has ever read the names the miners carved
into the galleries.

**The Salt Pan** is the expansion about the oldest trade in the valley: the
white business. It turns the live salt-mine system into a full works — data
veins, brine wells, evaporation pans and boiling houses, five grades of salt,
stocks for food and herds and ward and road, treaty deliveries that are kept
honestly and missed honestly, and the gallery records that make a mine a place
with a history instead of a resource with a number.

### 1.2 The five loops it adds

```
  Mine ──► Brine ──► Pan ──► Grade ─► Issue
    │         │        │        │         │
    ▼         ▼        ▼        ▼         ▼
  veins    wells    evaporate  rock, pan, storage,
  galleries  pumps   boil      fine, feed, counts
  records            fuel      medical, road
                                   │
                                   ▼
                     Deliver ──► Record ──► Keep
```

### 1.3 What the player manages

1. **The veins.** Which galleries produce, at what grade and contamination.
2. **The workers.** Crews, shifts, and the mine's work conditions.
3. **The brine.** Wells, pumps, contamination, and the connection to water.
4. **The pans.** Evaporation area, weather, and the boiling house fuel.
5. **The grades.** Rock, pan, fine, feed, medical, road.
6. **The stocks.** Food, herds, ward, chemistry, and roads.
7. **The treaties.** Deliveries kept and missed, and their records.
8. **The road.** Winter salt for the pass and the rail.
9. **The galleries.** Inscriptions, marks, and the mine's history.
10. **The ledger.** Production, issues, deliveries, and lessons.

### 1.4 What it is not

- Not a foundry or economy system; accords and deliveries stay with them.
- Not a chemistry system; clarity and reagent salts stay with 39.
- Not a food system; preservation salt is a supply to the common table.
- Not a medical system; the ward owns every infusion and dose.
- Not a water system; brine separation stays with the clean-flow owners.
- Not a new currency, price, or market for salt.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` | Veins, workers, drills, pumps, deliveries | `LIVE` |
| `src/Foundry/SilentFoundryHostSession.cs` | Host, `RegisterVein` `:582`, treaty events `:208–209` | `LIVE` |
| `src/UI/BrineExtractionPanel.cs` | Vein/worker/drill/pump/contamination/storage labels | `LIVE` |
| `Assets/Ashfall.Core/ExpansionHubSave.cs` | `saltMine` v6 persistence | `LIVE` |
| `Assets/Ashfall.Core/Foundry/BrineWaterSystem.cs` | Brine separation | `LIVE` (boundary) |
| `PowerGridSystem` | Pump and boiling power | `LIVE` |
| `NeedsSystem` | Morale sink | `LIVE` |
| `StandingRecord` | Records | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Rows | Notes |
|---|---|---|
| `salt_mine_inscriptions.json` | 4+ entries, 5,036 B | gallery names; unread |
| `brine_pickling_barrel_spoilage.json` | 4+ entries | preservation link; unread |
| `artesian_well_contamination_logs.json` | 4+ entries | brine/water link; unread |
| Salt, pan, grade, brine catalogs | **0** | confirmed absent |
| Salt items | 5 | `item_preservation_salt`, `item_rock_salt_sack`, `item_trade_salt_sack`, `item_medical_saline_salt`, `item_chem_clarity_salts` |

### 2.3 Confirmed gaps

- **GAP-61-1 — Veins are registered from code, not authored data.**
- **GAP-61-2 — No evaporation pans or boiling house.**
- **GAP-61-3 — No brine-well map or pump practice.**
- **GAP-61-4 — No grades or grade book for the five salt uses.**
- **GAP-61-5 — No livestock mineral or feed salt.**
- **GAP-61-6 — No road salt or winter pass supply.**
- **GAP-61-7 — No medical saline dilution practice or supply line.**
- **GAP-61-8 — No stock, damp, or caking rules for storage.**
- **GAP-61-9 — No gallery records, names, or mine history.**
- **GAP-61-10 — The authored mine inscriptions are read by nothing.**

### 2.4 Non-duplication statement

This expansion adds **no** second foundry, economy, chemistry, food, medical,
water, power, transport, or record system. It extends the live
`SaltMineExtractionSystem` with authored data, adds pan and grade practice on
top of it, supplies the food, herd, ward, chemistry, and road owners, fulfills
the foundry's existing treaty obligations without pricing anything, draws its
power through the grid, and files its records with `StandingRecord`. All new
state is additive inside `ExpansionHubSave.saltMine`. No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Salt is the oldest promise.** Every winter the valley survives
because somebody kept the meat, the herd, the ward, and the road salted.

**Pillar 2 — A mine is a place, not a number.** Galleries have names, and the
people who worked them wrote on the walls.

**Pillar 3 — Grades are honesty.** Rock salt for the road, pan salt for the
barrel, fine salt for the table, feed salt for the herd, medical salt for the
ward, and never the wrong one for the wrong job.

**Pillar 4 — Deliveries are kept or missed, never fudged.** A treaty delivery
is counted, recorded, and either made or missed with a reason.

**Pillar 5 — The pans belong to the weather.** Evaporation is free on a dry
day and expensive on a wet one, and the boiling house is for the wet ones.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Mine | Crews, galleries, lamps | Danger spectacle |
| Brine | Wells, pumps, tests | Alchemy |
| Pans | Weather, raking, patience | Free-money fantasy |
| Boiling | Fuel, shifts, steam | Industrial awe |
| Grades | Honest sorting | Purity snobbery |
| Deliveries | Counts, reasons, records | Smuggling drama |
| Road salt | Pass, rail, winter | Traffic systems |
| Galleries | Names, marks, records | Treasure maps |

### 3.3 Content limits

- No real salt companies, mines, or place names copied.
- No prices, currencies, or market mechanics for salt.
- No hazardous-gas spectacle, no collapse deaths as entertainment.
- No smelting or chemistry beyond what the live owners provide.
- No medicinal authority; the ward owns doses and infusions.
- No treasure, hoard, or artifact framing in the galleries.
- No new save section.

---

## 4. THE SALT PAN WORLD

### 4.1 Interior rooms

- **`room_salt_mine_office`** — vein books, crew boards, and the grade book.
- **`room_brine_pump_room`** — pumps, lines, and test jars.
- **`room_evaporation_hall`** — shallow pans and the raking floor.
- **`room_boiling_house`** — kettles, fuel, and the steam run.
- **`room_salt_grade_floor`** — sieves, bins, and grade marks.
- **`room_salt_store`** — dry bins, damp check, and the issue desk.
- **`room_gallery_records`** — inscriptions, rubbings, and the ledger.
- **`room_crew_room`** — shifts, clothes, and the wash after.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_salt_pans` | The Pans | 5 | Solar evaporation |
| `loc_brine_well` | The Brine Well | 5 | Deep well and pump |
| `loc_mine_adit_salt` | The Salt Adit | 5 | Mine entrance |
| `loc_inscription_wall` | The Inscription Wall | 7 | Names and history |
| `loc_haul_road` | The Haul Road | 5 | Mine to shop |
| `loc_pass_gate` | The Pass Gate | 6 | Winter road salt |
| `loc_rail_salt_siding` | The Rail Siding | 5 | Delivery loading |
| `loc_dry_lake` | The Dry Lake | 6 | Old pan-beds |
| `loc_stock_barn` | The Stock Barn | 4 | Herd mineral salt |
| `loc_white_marker` | The White Marker | 4 | Road and survivor lore |

All locations must resolve in `locations.json` and pass the map loader gate.

### 4.3 The rhythm

Mine and pump through the year; evaporate in the dry season; boil in the wet;
grade and store in autumn; deliver on treaty days; salt the pass when winter
closes it. The works are the only trade in the shelter whose best days are
decided by the sky.

---

## 5. MAIN STORYLINE — "THE WHITE BUSINESS"

### 5.1 Central conflict

**Sedge Farrow** came up in the pans and knows the dryness of every month;
**Ottiline Kest** runs the boiling house and watches the fuel take a third of
the salt's value in a wet year; **Tansy Farrow** keeps the vein books and has
registered every gallery except the one the old crew called the Long Room;
**Dora Kest** runs the grade floor and finds rock salt in the fine bins and
fine in the feed — a sorting failure that nobody has ever written down; **Juna
Kest** keeps the ward's saline supply and has been diluting medical salt by
taste; **Ashe Farrow** keeps the delivery ledger and has to tell the foundry
that this year's treaty salt is two barrels short; **Weft Kest** walks the
gallery wall and copies the names; **Della Kest** runs the road salt and knows
the pass closes on the first bad week of winter.

Then the dry season fails. The pans produce half of a normal year, the treaty
delivery is short, the ward's saline runs low, and the pass closes with the
shelter's road salt still in the store because nobody prioritised it. The
shelter gets through the winter on boiled brine, borrowed barrels, and a
decision it should have made in autumn: the grade book, the priority list, and
the pan plan.

The expansion's question: **what does a shelter owe the winter before it
arrives?**

### 5.2 Theme (unspoken)

**Salt is how a shelter makes a promise to its own future.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_panman_sedge_farrow` | Sedge Farrow | Pan man | Pans, weather, raking |
| `npc_boiler_ottiline_kest` | Ottiline Kest | Boiling house | Fuel, kettles, shifts |
| `npc_veinbook_tansy_farrow` | Tansy Farrow | Vein keeper | Books, galleries, plans |
| `npc_grade_dora_kest` | Dora Kest | Grade mistress | Sieves, bins, honesty |
| `npc_saline_juna_kest` | Juna Kest | Saline clerk | Ward supply, dilution |
| `npc_ledger_ashe_farrow` | Ashe Farrow | Delivery clerk | Treaties, counts, reasons |
| `npc_gallery_weft_kest` | Weft Kest | Gallery recorder | Names, rubbings, history |
| `npc_road_della_kest` | Della Kest | Road salt | Pass, rail, winter |

### 5.4 Story beats (15)

1. **The Code Veins.** The host's veins have no books behind them.
2. **The Long Room.** A gallery nobody registered is found.
3. **The Pans.** The dry season starts late.
4. **The Boil.** Ottiline fires the kettles and counts fuel.
5. **The Bins.** Dora finds rock salt in the fine grade.
6. **The Book.** The grade book is written.
7. **The Well.** Brine contamination is tested and posted.
8. **The Dilution.** Juna's taste-mixed saline is measured.
9. **The Shortfall.** The dry season ends half short.
10. **The Ledger.** Ashe writes the short treaty delivery.
11. **The Pass.** The road closes with salt still in store.
12. **The Winter.** Boiled brine carries the shelters that ran dry.
13. **The Priority.** The pan plan and issue list are agreed.
14. **The Names.** The gallery records are bound and filed.
15. **The White Business.** Salt becomes a planned trade.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Pans | expand / maintain / boil instead | capital |
| Grades | five strict / three / loose | honesty |
| Deliveries | treaty first / shelter first / split | obligation |
| Road | full / half / none | risk |
| Saline | measured / tasted / borrowed | care |
| Galleries | record / seal / preserve names | memory |
| Fuel | own wood / trade / solar only | cost |
| Final | pan works / mine works / dry store | identity |

### 5.6 Endings (5 + fade)

1. **The Dry Store** — five grades, full bins, a priority list, and a winter
   that does not touch the reserve.
2. **The Treaty Kept** — deliveries are made in full for three years, and the
   foundry's accords cite the salt works by name.
3. **The Pans Full** — evaporation expansion makes the boiling house a wet-year
   backup, and fuel cost drops out of the salt's price forever.
4. **The White Road** — the pass and the rail siding are salted every winter,
   and the valley's worst weeks stop cutting the shelter off.
5. **The Gallery Kept** — the names are copied, bound, and filed, and the mine
   is a place with a history rather than a hole with a number.
6. **Fade** — a rake left in a dry pan, a bin of fine salt with a grade mark,
   and a gallery wall where a new name has been cut beside a very old one.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_salt_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_salt_code_veins`, `quest_salt_long_room`, `quest_salt_pans`,
`quest_salt_boil`, `quest_salt_bins`, `quest_salt_book`, `quest_salt_well`,
`quest_salt_dilution`, `quest_salt_shortfall`, `quest_salt_ledger`,
`quest_salt_pass`, `quest_salt_winter`, `quest_salt_priority`,
`quest_salt_names`, `quest_salt_white_business`.

### 6.2 Side quests (30)

**Mine (5)**
- `quest_salt_veins` — veins authored
- `quest_salt_workers` — crews assigned
- `quest_salt_drill` — drill replaced
- `quest_salt_pump` — pump repaired
- `quest_salt_lamp_round` — gallery lamps checked

**Brine (5)**
- `quest_salt_wells` — wells mapped
- `quest_salt_test` — brine tested
- `quest_salt_contamination` — contamination posted
- `quest_salt_lines` — lines maintained
- `quest_salt_separate` — brine separated from water

**Pan (5)**
- `quest_salt_rake` — raking rota
- `quest_salt_dry_days` — dry days used
- `quest_salt_cover` — rain covers kept
- `quest_salt_fuel` — fuel gathered
- `quest_salt_kettle` — kettle run

**Grade (5)**
- `quest_salt_sieve` — sieves checked
- `quest_salt_rock` — rock grade binned
- `quest_salt_fine` — fine grade binned
- `quest_salt_feed` — feed grade binned
- `quest_salt_medical` — medical grade held

**Issue (5)**
- `quest_salt_food` — food salt issued
- `quest_salt_herd` — herd salt issued
- `quest_salt_ward` — ward salt issued
- `quest_salt_chem` — chemistry salt issued
- `quest_salt_road` — road salt issued

**Deliver and record (5)**
- `quest_salt_delivery` — treaty delivery made
- `quest_salt_missed` — shortfall recorded
- `quest_salt_gallery_mark` — a name copied
- `quest_salt_ledger_entry` — ledger kept
- `quest_salt_lesson` — lesson kept

### 6.3 Repeatable quests (8)

`quest_salt_repeat_pump`, `quest_salt_repeat_rake`,
`quest_salt_repeat_grade`, `quest_salt_repeat_issue`,
`quest_salt_repeat_deliver`, `quest_salt_repeat_test`,
`quest_salt_repeat_count`, `quest_salt_repeat_teach`.

### 6.4 Dynamic hooks

Live events (treaty delivery events, mine ticks, pump and drill condition,
weather, power, ward demand, herd tick, road state) attach authored follow-ups
through existing seams. No new event bus.

### 6.5 Constraints

- The live salt system keeps its authority; this plan extends it.
- Treaty obligations are read and fulfilled, never repriced.
- Chemistry, food, medical, and water owners keep their authority.
- Power draws through the grid.
- Morale and stress use `NeedsSystem.Modify` only.
- Records file with `StandingRecord`.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `SaltVeinSystem` (extend `SaltMineExtractionSystem`)

**Owns:** authored veins: gallery, depth, grade, contamination, yield, and
unlock conditions. **Consumes:** the live mine tick. **Data:**
`salt_veins.json`. **Rules:** veins come from data, not code; every gallery has
a name; a vein's grade and contamination are visible before workers are
assigned; unlocking is earned.

### 7.2 `BrineWellSystem` (new, thin, `Ashfall.Core.Foundry`)

**Owns:** brine wells, lines, pumps, contamination, and separation. **Consumes:**
`BrineWaterSystem`, power, water owners. **Data:** `brine_wells.json`.
**Rules:** each well has a tested contamination band; brine never enters the
potable line; a well that fails its test is rested, not hidden.

### 7.3 `PanSystem` (new, `Ashfall.Core.Foundry`)

**Owns:** evaporation pans, raking, weather use, covers, and yield. **Consumes:**
brine, weather, labor. **Data:** `evaporation_pans.json`. **Rules:** pans
belong to the weather; covers are honest; a wet season routes to the boiling
house rather than wasting brine.

### 7.4 `BoilingHouseSystem` (new, thin, `Ashfall.Core.Foundry`)

**Owns:** kettles, fuel, shifts, and steam. **Consumes:** fuel from expansion
21/31, power, fire rules. **Data:** `boiling_runs.json`. **Rules:** fuel is
counted against yield; the boiling house is a wet-year tool, not the default;
every run records its fuel cost.

### 7.5 `SaltGradeSystem` (new, `Ashfall.Core.Foundry`)

**Owns:** sieving, bins, grade marks, and cross-grade contamination checks.
**Data:** `salt_grades.json`, `grade_bins.json`. **Rules:** five grades and no
loose sorting; a bin found with the wrong grade is corrected and recorded;
grade marks are visible.

### 7.6 `SaltIssueSystem` (new, thin, `Ashfall.Core.Inventory`)

**Owns:** issue to food, herds, ward, chemistry, and roads, plus the reserve.
**Consumes:** requesters' demand, the grade bins, the priority list. **Data:**
`salt_issues.json`. **Rules:** the reserve is issued only by an explicit
priority decision; every issue is counted against a requester; no grade is
issued to a use it was not graded for.

### 7.7 `SaltDeliverySystem` (new, thin, `Ashfall.Core.Foundry`)

**Owns:** delivery scheduling against the live treaty records, shortfall
reasons, and the delivery ledger. **Consumes:** `TreatyDeliveryRecord` events.
**Data:** `salt_deliveries.json`. **Rules:** deliveries are counted and either
made or missed; a shortfall is recorded with its reason; nothing is repriced,
smuggled, or fudged.

### 7.8 `GalleryRecordSystem` (new, thin, `Ashfall.Core.Narrative`)

**Owns:** gallery names, inscriptions, rubbings, names, and the mine's
history. **Data:** `mine_gallery_records.json`. Files through `StandingRecord`.
**Rules:** names are copied faithfully; the wall is never rewritten; the
ledger separates the mine's business from the miners' names.

### 7.9 Systems explicitly not added

- No second foundry, economy, chemistry, food, medical, water, power, or
  transport system.
- No salt prices, currencies, or markets.
- No gas, collapse, or disaster spectacle.
- No treasure or artifact framing in galleries.
- No new RNG stream beyond the live mine tick and weather paths.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `salt_veins.json` (new)

```json
{
  "schema_version": 1,
  "veins": [
    {
      "vein_id": "vein_long_room",
      "gallery_name": "The Long Room",
      "depth_m": 96,
      "grade": "rock",
      "contamination_band": "low",
      "yield_per_shift": 14,
      "unlock_treaty_count": 2,
      "tags": ["historic"]
    }
  ]
}
```

### 8.2 `brine_wells.json` (new)

Wells: well, depth, pump, contamination band, test interval, rest rule.

### 8.3 `evaporation_pans.json` (new)

Pans: area, depth, brine load, dry yield, cover, rake rota.

### 8.4 `boiling_runs.json` (new)

Runs: kettle, brine, fuel, hours, yield, cost note.

### 8.5 `salt_grades.json` (new)

Grades: rock, pan, fine, feed, medical; use, rule, bin, check.

### 8.6 `grade_bins.json` (new)

Bins: grade, count, damp, caking, mark, verified.

### 8.7 `salt_issues.json` (new)

Issues: requester, grade, quantity, day, reserve flag, note.

### 8.8 `salt_deliveries.json` (new)

Deliveries: treaty, due day, quantity, made, shortfall, reason.

### 8.9 `mine_gallery_records.json` (new)

Records: gallery, inscription, medium, tool, recorder, name, year.

### 8.10 `salt_store.json` (new)

Store: bin, damp reading, caking, reserve floor, review.

### 8.11 Items

New items appended to `items.json`: `item_rock_salt_chunk`,
`item_pan_salt`, `item_fine_salt`, `item_feed_salt_block`,
`item_saline_salt_jar`, `item_clarity_salt_sack`,
`item_road_salt_measure`, `item_brine_bucket`, `item_brine_test_jar`,
`item_evaporation_pan`, `item_salt_rake`, `item_salt_sieve`,
`item_grade_mark_stamp`, `item_gallery_rubbing`, `item_gallery_chisel`,
`item_salt_ledger`, `item_delivery_barrel`, `item_store_damp_card`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`ExpansionHubSave.saltMine` remains the live save owner. Vein books, pan
state, boiling runs, grade bins, issues, deliveries, gallery records, and the
store are additive sub-objects. No new save section.

### 9.2 State to persist

- Authored veins registered, unlocked, and worked.
- Workers per vein and shift state.
- Well tests, pump condition, and rest periods.
- Pan loads, covers, and dry yield.
- Boiling runs and fuel cost.
- Grade bins, counts, and corrections.
- Issues per requester and the reserve floor.
- Deliveries made and shortfalls with reasons.
- Gallery records and rubbings.
- Store damp and caking state.

### 9.3 Determinism

- Vein yields follow the live `TickDaily(day, rng)` path.
- Pan yield is authored per dry day and weather state; no seeded noise.
- Boiling yield and fuel cost are authored per kettle and brine load.
- Grade checks are deterministic comparisons on bin contents.
- Delivery counting follows the live treaty events exactly.
- Paired replay hashes must match; no `System.Random` outside the live path.

### 9.4 Migration

Legacy saves load with the pre-existing code veins intact, empty books, and a
store holding the current salt items. Pan and boiling state start empty. The
reserve floor defaults to the authored minimum so an old save cannot be
retroactively overdrawn.

### 9.5 Checksum

Invariant-culture floats for yield and contamination; integer counts, depth,
and days.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `SaltPanPanel` (new) | Pans, boiling, grades | `SaltPanHostSession` |
| `VeinBookPanel` (new) | Veins and galleries | same |
| `BrineWellPanel` (new) | Wells and tests | same |
| `GradeFloorPanel` (new) | Bins and corrections | same |
| `SaltIssuePanel` (new) | Issues and reserve | same |
| `SaltDeliveryPanel` (new) | Treaties and shortfalls | same |
| `GalleryRecordPanel` (new) | Names and rubbings | same |
| `BrineExtractionPanel` (extend) | Live mine state | existing |
| `SilentFoundryPanel` (extend) | Salt works ribbon | existing |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Grade and contamination are shown as words and numbers, never color alone.
- Shortfalls are always shown with their reason.
- The reserve floor is visible before any issue is confirmed.
- Keyboard/controller close/back preserved; focus maintained on refresh.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a pump starting against the head
pressure, a rake dragging across a dry pan, a kettle settling, a sieve
shaking, a barrel lid being set. No cue is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `SaltMineExtractionSystem` | Extended by veins and pans |
| `SilentFoundryHostSession` | Host surface and treaty events |
| `BrineExtractionPanel` | Extended UI |
| `ExpansionHubSave.saltMine` | Save owner |
| 10 Silent Foundry (Exp 10) | Accords and deliveries |
| 39 The Reagent (Wave 6) | Clarity salts |
| 26 The Common Table (Wave 3) | Preservation salt |
| 38 The Ward (Wave 6) | Saline supply |
| 15 The Deep Root (Wave 1) | Herd mineral salt |
| 22 The Clean Flow (Wave 3) | Brine separation |
| 52 The Warm Ground (Wave 9) | Deep well heat boundary |
| 34 The Long Road (Wave 5) | Haul road and pass |
| 45 The Envoy (Wave 7) | Treaty context |
| 54 The Uninvited (Wave 9) | Store protection |
| 55 The Quarter (Wave 9) | Work conditions |
| `PowerGridSystem` | Pumps and kettles |
| `WeatherSystem` (Wave 5) | Dry days and covers |
| `NeedsSystem` | Work comfort via `Modify` |
| `StandingRecord` | Gallery and delivery ledgers |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.0 Wiring note for the promotion package

The Salt Pan is the only Wave 10 plan that extends a live host path rather
than adding a new one. Any package that claims it must name
`SilentFoundryHostSession` and `ExpansionHubSave.saltMine` explicitly in
`WORKTREE_OWNERSHIP.md`, because both are shared seams: the host is the
foundry's, the save version is the hub's, and the vein registration move is an
additive change inside someone else's file. No other Wave 10 plan needs a
shared-seam claim.

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm the live salt system, host wiring,
save v6, brine panel, foundry accords, power, weather, ward, herd, and record
owners. Record file:line; change nothing.

**Phase 1 — Data + validators.** Author the catalogs and register them with
`CatalogIntegrityValidator` and `ContentUtilizationScanner`; move vein
registration from code to data while keeping legacy code veins working.

**Wiring note (foreman-critical).** This is the only Wave 10 plan that touches
an already-live host path. `SilentFoundryHostSession` registers veins from
code at `:582`; the data migration must register authored `salt_veins.json`
entries through the same `RegisterVein` call and must keep every legacy
code-registered vein loadable, because an existing save cannot lose a working
vein. The order is: load authored veins, then legacy veins, deduplicating by
`vein_id` with authored data winning; a save with no salt catalog bound keeps
the exact legacy behavior. A focused test must prove both registration paths
and that paired replay hashes match.

**Phase 2 — Pure Core.** `SaltVeinSystem`, `BrineWellSystem`, `PanSystem`,
`BoilingHouseSystem`, `SaltGradeSystem`, `SaltIssueSystem`,
`SaltDeliverySystem`, `GalleryRecordSystem`.

**Phase 3 — Persistence.** Additive sub-objects inside `saltMine`, migration,
round-trip, determinism.

**Phase 4 — Host + CLI.** `SaltPanHostSession`, focused selftest coverage,
fresh journey from code veins to the planned pan year.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** Multi-year soak: dry years, wet years, delivery
shortfalls, reserve floors, road-salt winters.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Veins | 12 |
| Wells | 8 |
| Pans | 10 |
| Boiling runs | 8 |
| Grades | 5 |
| Bins | 10 |
| Issues | 16 |
| Deliveries | 10 |
| Gallery records | 14 |
| Store rows | 10 |
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
| Economy drift | High | No prices, count only |
| Foundry overlap | High | Extend live owner |
| Chemistry overlap | Medium | Supply only |
| Medical overlap | High | Ward owns doses |
| Water overlap | High | Separation boundary |
| Mine spectacle | High | Plain work tone |
| Determinism break | Low | Live tick only |
| Data migration risk | Medium | Legacy code veins kept |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `salt_veins.json` | 12 | 3,000 |
| `brine_wells.json` | 8 | 2,000 |
| `evaporation_pans.json` | 10 | 2,500 |
| `boiling_runs.json` | 8 | 2,000 |
| `salt_grades.json` | 5 | 2,000 |
| `grade_bins.json` | 10 | 2,500 |
| `salt_issues.json` | 16 | 3,000 |
| `salt_deliveries.json` | 10 | 2,500 |
| `mine_gallery_records.json` | 14 | 3,000 |
| `salt_store.json` | 10 | 2,500 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 18 | 3,000 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~58,500** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R61-1 | Economy creep | Med | High | No prices |
| R61-2 | Foundry fork | Med | High | Extension |
| R61-3 | Chemistry fork | Low | Medium | Supply |
| R61-4 | Ward fork | Med | High | Saline only |
| R61-5 | Water fork | Med | High | Separation |
| R61-6 | Mine spectacle | Med | High | Plain tone |
| R61-7 | Determinism | Low | High | Live tick |
| R61-8 | Legacy veins | Med | Medium | Keep registered |
| R61-9 | Fixed content | Low | Medium | Data-first |
| R61-10 | Content overrun | Med | Medium | Budget |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Do code-registered veins stay?** Recommended: yes, registered as legacy
   data entries on load; no save loses a working vein.
2. **Who sets the reserve floor?** Recommended: the issue clerk with the store
   keeper and one witness, reviewed each autumn.
3. **How is a shortfall handled?** Recommended: recorded with a reason and
   carried forward; never hidden and never repriced.
4. **Does road salt outrank a treaty delivery?** Recommended: no standing rule;
   the priority is decided each autumn and written down before winter.
5. **Are gallery names public?** Recommended: yes, copied and filed; the wall
   is not owned by the works.

---

## 17. APPENDIX D — VEIN TABLE

| # | Vein | Gallery | Depth m | Grade | Contamination | Yield/shift | Unlock |
|---|---|---|---|---|---|---|---|
| 1 | V-01 | The First Room | 40 | rock | low | 18 | open |
| 2 | V-02 | The Second Room | 55 | pan | low | 16 | open |
| 3 | V-03 | The Long Room | 96 | rock | low | 14 | 2 treaties |
| 4 | V-04 | The Wet Room | 60 | pan | medium | 12 | 1 treaty |
| 5 | V-05 | The High Room | 35 | rock | low | 10 | open |
| 6 | V-06 | The Deep Room | 120 | rock | high | 9 | 3 treaties |
| 7 | V-07 | The Narrow Room | 70 | fine | low | 8 | 2 treaties |
| 8 | V-08 | The White Room | 82 | fine | low | 7 | 4 treaties |
| 9 | V-09 | The Old Room | 45 | pan | medium | 6 | sealed |
| 10 | V-10 | The New Room | 130 | rock | high | 11 | 5 treaties |
| 11 | V-11 | The Gallery of Names | 58 | rock | low | 5 | open |
| 12 | V-12 | The Cold Room | 100 | feed | low | 8 | 3 treaties |

Twelve veins and the third is the story's discovery: the Long Room was worked
for decades and never registered, and its low-contamination rock makes it the
mine's best site. The eleventh exists for the gallery records rather than its
yield — five shifts a month is not a business, but the names on its wall are
the reason the works has a history at all. The ninth row is sealed, with the
old crew's reason written on the door.

---

## 18. APPENDIX E — WELL TABLE

| # | Well | Depth m | Pump | Contamination | Test | Rest rule |
|---|---|---|---|---|---|---|
| 1 | W-01 | 60 | lift | low | 30 d | none |
| 2 | W-02 | 75 | lift | low | 30 d | none |
| 3 | W-03 | 90 | piston | medium | 14 d | 7 d if high |
| 4 | W-04 | 110 | piston | high | 7 d | 14 d if high |
| 5 | W-05 | 100 | lift | medium | 14 d | 7 d |
| 6 | W-06 | 88 | piston | low | 30 d | none |
| 7 | W-07 | 120 | piston | high | 7 d | 21 d |
| 8 | W-08 | 70 | lift | low | 30 d | none |

Eight wells and the fourth is the one that keeps getting high readings: a
hundred and ten metres down, the brine carries so much mineral that the grade
floor has to blend it rather than run it straight. The rest rules are the
works' safety habits — a well that tests high is rested and re-tested before
it is worked again, and the rest is recorded like any other maintenance.

---

## 19. APPENDIX F — PAN TABLE

| # | Pan | Area m² | Depth mm | Load L | Dry yield kg | Cover | Rake |
|---|---|---|---|---|---|---|---|
| 1 | P-1 | 40 | 120 | 4,800 | 145 | boards | 2×daily |
| 2 | P-2 | 40 | 120 | 4,800 | 145 | boards | 2×daily |
| 3 | P-3 | 30 | 100 | 3,000 | 95 | boards | daily |
| 4 | P-4 | 30 | 100 | 3,000 | 95 | boards | daily |
| 5 | P-5 | 25 | 90 | 2,250 | 70 | tarp | daily |
| 6 | P-6 | 25 | 90 | 2,250 | 70 | tarp | daily |
| 7 | P-7 | 20 | 80 | 1,600 | 50 | tarp | 2×daily |
| 8 | P-8 | 20 | 80 | 1,600 | 50 | tarp | 2×daily |
| 9 | P-9 | 15 | 70 | 1,050 | 32 | none | daily |
| 10 | P-10 | 15 | 70 | 1,050 | 32 | none | daily |

Ten pans and the sums are the story's shortfall: a full dry season yields
about seven hundred and eighty-four kilograms of pan salt, and the shelter's
five uses need over a thousand. The covers matter as much as the area — a
single uncovered rain event costs a pan its whole load — and the ninth and
tenth pans are deliberately uncovered so that a missed cover costs little.

---

## 20. APPENDIX G — BOILING TABLE

| # | Run | Kettle | Brine L | Fuel | Hours | Yield kg | Cost note |
|---|---|---|---|---|---|---|---|
| 1 | B-1 | kettle 1 | 600 | wood 0.4 m³ | 8 | 75 | wet year |
| 2 | B-2 | kettle 1 | 600 | wood 0.4 m³ | 8 | 75 | wet year |
| 3 | B-3 | kettle 2 | 400 | coal 60 kg | 6 | 50 | coal route |
| 4 | B-4 | kettle 2 | 400 | coal 60 kg | 6 | 50 | coal route |
| 5 | B-5 | kettle 1 | 800 | wood 0.5 m³ | 10 | 100 | emergency |
| 6 | B-6 | kettle 3 | 300 | gas 20 kg | 4 | 38 | grid down |
| 7 | B-7 | kettle 1 | 600 | wood 0.4 m³ | 8 | 74 | improved |
| 8 | B-8 | kettle 1 | 600 | wood 0.4 m³ | 8 | 76 | improved |

The boiling table is the works' honesty about cost: boiling a litre of brine
takes more fuel than the salt is worth in a good year, which is why the pans
exist and why the kettles only run in the wet season. The seventh and eighth
runs show a small improvement from better fire management — a recovered
kilogram per run — and that improvement is the expansion's economic argument:
skill, not price.

---

## 21. APPENDIX H — GRADE TABLE

| # | Grade | Source | Use | Rule | Check |
|---|---|---|---|---|---|
| 1 | Rock | mine, coarse | road, stock, curing | never food | weekly |
| 2 | Pan | pans, medium | barrels, pickling | food after wash | weekly |
| 3 | Fine | pans, sieved | table, kitchen | only food | weekly |
| 4 | Feed | mine, mineral | herd, poultry | fixed block | monthly |
| 5 | Medical | fine, cleanest | saline, ward | ward only | each batch |

Five grades and the first rule is the one the story's sorting failure broke:
rock salt never goes into food, and the day Dora finds rock salt in the fine
bins, the grade book gets its reason to exist. The fifth row is the smallest
and strictest — medical salt comes from the cleanest fine lot and goes nowhere
else, and its batch is held by the ward's clerk.

---

## 22. APPENDIX I — BIN TABLE

| # | Bin | Grade | Count kg | Damp | Caking | Verified |
|---|---|---|---|---|---|---|
| 1 | F-1 | fine | 120 | dry | none | weekly |
| 2 | F-2 | fine | 95 | dry | light | weekly |
| 3 | P-1 | pan | 210 | dry | none | weekly |
| 4 | P-2 | pan | 180 | light | none | weekly |
| 5 | R-1 | rock | 340 | light | heavy | monthly |
| 6 | R-2 | rock | 300 | wet | heavy | monthly |
| 7 | D-1 | feed | 90 | dry | none | monthly |
| 8 | D-2 | feed | 85 | dry | none | monthly |
| 9 | M-1 | medical | 12 | dry | none | each batch |
| 10 | X-1 | mixed | 0 | — | — | pulled |

The tenth bin is the lesson bin: mixed salt is pulled, recorded, and re-graded
rather than used, and the pulled bin's history is read at the year review. The
sixth row's wet rock salt is not a problem because rock salt is allowed to be
wet — it is going to the road anyway — which is exactly the kind of small
freedom a five-grade system buys.

---

## 23. APPENDIX J — ISSUE TABLE

| # | Requester | Grade | Quantity kg | Day | Reserve | Note |
|---|---|---|---|---|---|---|
| 1 | common table | pan | 40 | 20 | no | pickling |
| 2 | common table | fine | 8 | 20 | no | table |
| 3 | herd | feed | 30 | 22 | no | blocks |
| 4 | ward | medical | 2 | 25 | no | saline |
| 5 | reagent | fine | 6 | 28 | no | clarity |
| 6 | road | rock | 120 | 30 | no | pass |
| 7 | common table | pan | 35 | 45 | no | meat |
| 8 | rail siding | rock | 60 | 50 | no | delivery |
| 9 | common table | pan | 12 | 70 | yes | short year |
| 10 | ward | medical | 1 | 72 | yes | short year |

Ten issues and the ninth and tenth are the reserve being drawn: in the short
year the table gets twelve kilograms instead of forty and the ward gets one
instead of two, and both draws are recorded so the autumn review can see how
close the works came. The reserve is not a hiding place; it is a number that
only moves with a decision and a witness.

---

## 24. APPENDIX K — DELIVERY TABLE

| # | Treaty | Due | Quantity | Made | Shortfall | Reason |
|---|---|---|---|---|---|---|
| 1 | D-1 | day 120 | 40 barrels | 40 | 0 | full |
| 2 | D-2 | day 210 | 60 barrels | 60 | 0 | full |
| 3 | D-3 | day 300 | 30 barrels | 26 | 4 | dry season |
| 4 | D-4 | day 330 | 50 barrels | 50 | 0 | full |
| 5 | D-5 | day 60 | 20 barrels | 20 | 0 | full |
| 6 | D-6 | day 150 | 25 barrels | 25 | 0 | full |
| 7 | D-7 | day 240 | 35 barrels | 35 | 0 | full |
| 8 | D-8 | day 320 | 45 barrels | 40 | 5 | pan failure |
| 9 | D-9 | day 355 | 15 barrels | 15 | 0 | full |
| 10 | D-10 | day 90 | 30 barrels | 0 | 30 | pass closed |

The delivery table is the expansion's treaty honesty in one page: two
shortfalls in a bad year, both recorded with their reasons, neither hidden and
neither repriced. The tenth row is the one the works disputes — the pass was
closed, which is a reason and not an excuse — and the record of that dispute
is kept, because a ledger that only contains successes is a ledger nobody
needs.

---

## 25. APPENDIX L — GALLERY TABLE

| # | Gallery | Inscription | Medium | Tool | Recorder | Year |
|---|---|---|---|---|---|---|
| 1 | The First Room | first shift | rock | chisel | crew | 508 |
| 2 | The Second Room | twenty barrels | rock | chisel | foreman | 514 |
| 3 | The Long Room | eleven names | rock | knife | unknown | 540s |
| 4 | The Wet Room | pump broke | rock | chalk | crew | 552 |
| 5 | The High Room | a child | rock | nail | mother | 561 |
| 6 | The Deep Room | do not enter | rock | chisel | foreman | 570 |
| 7 | The Narrow Room | the good seam | rock | knife | crew | 578 |
| 8 | The White Room | the day it rained | rock | chisel | crew | 584 |
| 9 | The Old Room | the flood | rock | chalk | crew | 590 |
| 10 | The Gallery of Names | forty names | rock | chisel | many | ongoing |
| 11 | The Cold Room | salt is cold | rock | knife | crew | 601 |
| 12 | The New Room | started late | rock | chisel | crew | 611 |
| 13 | The Adit | welcome | rock | chisel | works | 611 |
| 14 | The Inscription Wall | copied | rock | rubbing | Weft | 612 |

Fourteen records and the fifth is the smallest and heaviest: one word and a
year, cut into rock by a mother whose child is not otherwise in any shelter
record. The tenth row is the gallery the story ends on — forty names copied
and filed, some from before the bunker was dug — and the fourteenth is the
rubbing that makes the mine's history part of the shelter's records without
moving a single stone.

---

## 26. APPENDIX M — STORE TABLE

| # | Bin | Damp reading | Caking | Reserve floor kg | Review |
|---|---|---|---|---|---|
| 1 | F-1 | dry | none | 60 | weekly |
| 2 | P-1 | dry | none | 120 | weekly |
| 3 | R-1 | light | heavy | 200 | monthly |
| 4 | D-1 | dry | none | 50 | monthly |
| 5 | M-1 | dry | none | 6 | each batch |
| 6 | Reserve pan | dry | none | 80 | monthly |
| 7 | Reserve rock | light | heavy | 100 | monthly |
| 8 | Emergency | dry | none | 40 | quarterly |

The store table's sixth and seventh rows are the reserve proper, and the
eighth is the emergency stub that survived the pass-closure winter: forty
kilograms of rock salt held for the worst week, never issued without a
recorded decision. The caking column is not cosmetic — heavy cake is a sign
the damp has won, and a caked bin is broken down and re-binned before it
becomes a wasted ton.

---

## 27. APPENDIX N — WORKED PAN YEAR

**Spring.** Tansy registers the Long Room and the works' books finally match
its galleries. Well W-04 tests high and is rested for two weeks. The pans are
repaired, boards are replaced on the four covered pans, and the first loads
are drawn from the brine wells.

**Early summer.** The dry season starts late, which is the year's first bad
sign. Six pans evaporate well in the first week and then a rain event costs
two uncovered pans their whole loads, which is exactly what the uncovered
pans are for.

**Late summer.** The dry season closes early, a third short of the pan plan.
The boiling house is fired on wood for three days and produces a hundred and
seventy-five kilograms for nearly a cord and a half of fuel, and Ottiline's
cost note makes the shortfall concrete: the kettle rescued the schedule at a
price the works can only pay once.

**Autumn.** Dora finds rock salt in the fine bins, corrects it, and writes the
grade book's first correction. The reserve is counted and is twelve kilograms
under the floor. The treaty delivery D-3 goes out four barrels short with a
dry-season reason, and Ashe files it the same day.

**Winter.** The pass closes in the first week and the road salt is still in
the store, so the works carries the blame for a decision nobody made. The
shelter gets through on the reserve floor, boiled brine, and borrowed barrels
from the rail siding. The ward's saline is diluted by weight for the first
time instead of by taste.

**Late winter.** The autumn review turns the season into policy: the pan plan
is posted, the road salt is issued before the first frost, the grade book is
owed weekly, and the reserve floor rises by forty kilograms. The gallery
names are copied and filed, and the works' first full planned year ends with a
worse harvest and a better institution, which is the trade's whole point.

---

## 28. APPENDIX O — VIGNETTES (TONE SAMPLE)

> Sedge walks the pans at dawn and drags the rake once through the first pan
> and holds up a hand of pink-grey crystals, and the day has not decided
> whether it will be dry, which is the sort of gamble the works takes
> every morning of the season.

> Ottiline feeds the kettle for three hours and the steam finds the roof
> and the salt crusts on the beam overhead, and she enters the fuel cost on
> the run sheet because a boiling run that is not costed is a lie the works
> tells itself.

> Juna measures the medical salt by weight on a brass balance and mixes the
> saline to the marked line, and the ward's clerk signs the batch, and the
> taste-testing habit dies quietly in one afternoon of arithmetic.

> Weft kneels at the gallery wall with rice paper and a soft pencil and
> copies forty names, some cut deep and some barely scratched, and the
> rubbing goes into the ledger beside the delivery records, because the mine
> is a place where people lived and not only a place where salt comes from.

---

## 29. APPENDIX P — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Late dry season | pan shortfall | boil, ration |
| Rain on pans | lost loads | covers, spares |
| High well test | brine unfit | rest, blend |
| Drill worn | slow cuts | replace |
| Pump dry | brine stops | repair |
| Grade mix | wrong use risk | pull, re-bin |
| Reserve drawn | tight winter | plan, record |
| Delivery short | treaty strain | record, carry |
| Pass closed early | road short | issue before frost |
| Names lost | history lost | rubbing, filing |

Every recovery is a habit the works already teaches, and the ninth row is the
one the shelter learns the hard way: the pass does not care that the salt was
in the store, only that it was not on the road, and the fix is a calendar
entry instead of a principle.

---

## 31. APPENDIX Q — GLOSSARY

- **Vein** — a working face in the mine with a grade and a contamination band.
- **Gallery** — a named passage; the mine's addressable rooms.
- **Brine** — mineral water pumped from wells or drawn from the mine.
- **Pan** — a shallow bed where sun and wind evaporate brine.
- **Kettle** — the boiling house vessel used in wet seasons.
- **Grade** — rock, pan, fine, feed, medical; each with its own use.
- **Reserve floor** — the kilograms the works will not issue without a decision.
- **Treaty delivery** — the salt obligation the foundry's accords carry.
- **Shortfall** — a delivery missed or reduced, recorded with its reason.
- **Rubbing** — a paper copy of a gallery inscription.

---

## 32. APPENDIX R — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `SaltVeinSystem` | mine tick | veins, unlocks | galleries |
| `BrineWellSystem` | water owners | tests, rests | potable line |
| `PanSystem` | weather | pan loads, yield | weather |
| `BoilingHouseSystem` | fuel stock | runs, cost notes | fuel ledger |
| `SaltGradeSystem` | bins | grades, corrections | uses |
| `SaltIssueSystem` | demand | issues, reserve | requesters |
| `SaltDeliverySystem` | treaty events | deliveries, reasons | accords |
| `GalleryRecordSystem` | inscriptions | records | nothing |
| `SaltMineExtractionSystem` | nothing | nothing | nothing |
| `SilentFoundryHostSession` | events | events | nothing |
| `PowerGridSystem` | draw | power | salt |
| `WeatherSystem` | nothing | nothing | nothing |
| `NeedsSystem` | nothing | nothing | nothing |
| `Inventory` | issues | stock | salt |
| `StandingRecord` | records | records | nothing |

---

## 33. APPENDIX S — DATA SCHEMA DETAIL (NEW CATALOGS)

**`salt_veins.json`** — `vein_id`, `gallery_name`, `depth_m`, `grade`,
`contamination_band`, `yield_per_shift`, `unlock_treaty_count`, `tags[]`.

**`brine_wells.json`** — `well_id`, `depth_m`, `pump`, `contamination_band`,
`test_days`, `rest_rule`, `tags[]`.

**`evaporation_pans.json`** — `pan_id`, `area_sq_m`, `depth_mm`,
`load_litres`, `dry_yield_kg`, `cover`, `rake_rota`, `tags[]`.

**`boiling_runs.json`** — `run_id`, `kettle`, `brine_litres`, `fuel`,
`hours`, `yield_kg`, `cost_note`, `tags[]`.

**`salt_grades.json`** — `grade_id`, `source`, `use`, `rule`, `check`,
`tags[]`.

**`grade_bins.json`** — `bin_id`, `grade_id`, `count_kg`, `damp`, `caking`,
`verified`, `tags[]`.

**`salt_issues.json`** — `issue_id`, `requester`, `grade_id`, `quantity_kg`,
`day`, `reserve`, `note`, `tags[]`.

**`salt_deliveries.json`** — `delivery_id`, `treaty_id`, `due_day`,
`quantity`, `made`, `shortfall`, `reason`, `tags[]`.

**`mine_gallery_records.json`** — `record_id`, `gallery`, `inscription`,
`medium`, `tool`, `recorder_id`, `year`, `tags[]`.

**`salt_store.json`** — `bin_id`, `damp_reading`, `caking`, `reserve_floor_kg`,
`review`, `tags[]`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on
missing or duplicate IDs, invalid vein, bin, treaty, or requester references,
or out-of-range numbers. **No catalog may carry a price field; the validator
rejects one.**

---

## 34. APPENDIX T — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Vein yield | production | Mine |
| Contamination mean | safety | Wells |
| Pan yield per season | weather skill | Pans |
| Boiling cost per kg | honesty | Kettles |
| Grade corrections | sorting quality | Grade floor |
| Reserve level | readiness | Store |
| Issues by requester | priority | Issues |
| Deliveries made | treaties | Ledger |
| Shortfall reasons | learning | Ledger |
| Names recorded | history | Gallery |

Telemetry is diagnostic only; it never gates content, never ranks a crew, and
never turns a shortfall into a penalty.

---

## 35. APPENDIX U — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored, registered, and wired to the live
  `RegisterVein` path; legacy code veins still load.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive inside `saltMine`.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling.
- [ ] Phase 6 content passes the review checklist in §29.
- [ ] Phase 7 soak shows a dry year, a wet year, a shortfall, and a pass
  winter with salt on the road.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No prices, currencies, mineshaft spectacle, or treasure framing exists.

---

## 36. APPENDIX V — OPEN QUESTIONS FOR REVIEW

1. Does a legacy code vein get a gallery name when it is registered from data?
2. Can a shortfall be carried as credit to the next delivery, or is it simply
   recorded?
3. Who may draw the reserve, and how many signatures does it take?
4. Does road salt compete with treaty salt in a hard winter, and who decides?
5. Are gallery rubbings public in the shelter, or kept by the works?
6. Can a vein be closed for good, and who may order it?
7. Does the ward blend its own saline, or does the works deliver solution?
8. What happens when a well tests high for a season and the pans need brine?

None of these may be decided unilaterally; each changes tone and balance.

---

## 37. APPENDIX W — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 15 The Deep Root | Herd mineral blocks |
| 2 | 21 The Grid | Pump and kettle power |
| 3 | 22 The Clean Flow | Brine separation |
| 3 | 26 The Common Table | Preservation and pickling |
| 4 | 31 The Kiln | Firebrick and evaporating pans |
| 5 | 33 The Weather | Dry days, covers, storms |
| 5 | 34 The Long Road | Haul road and pass |
| 6 | 38 The Ward | Saline stock |
| 6 | 39 The Reagent | Clarity salts |
| 6 | 40 The Wheel | Pumps and haulage |
| 6 | 41 The Quiet | Shift noise boundary |
| 7 | 44 The Outpost | Salt as a shared supply |
| 7 | 45 The Envoy | Treaty context |
| 8 | 47 The Brigade | Boiling house fire rules |
| 8 | 50 The Vault | Gallery records copied |
| 9 | 54 The Uninvited | Salt storage protection |
| 9 | 55 The Quarter | Work conditions |
| 10 | 58 The Joinery | Barrel staves and store bins |
| 10 | 60 The Wick | Lamp light for the night shift |

Each hook is additive. The Salt Pan can ship alone, and every other expansion
can ship without it.

---

## 38. APPENDIX X — ENDING PROSE SKETCHES

**The Dry Store.** Five grades, full bins, a priority list, and a winter that
does not touch the reserve floor.

**The Treaty Kept.** Deliveries are made in full for three years, and the
works' name appears in the foundry's minutes without a shortfall beside it.

**The Pans Full.** Evaporation expansion makes the boiling house a wet-year
backup, and fuel cost drops out of every kilogram's story.

**The White Road.** The pass and the rail siding are salted before the first
frost, and the valley's worst weeks stop cutting the shelter off.

**The Gallery Kept.** The names are copied, bound, and filed, and the mine is
a place with a history rather than a hole with a number.

**Fade.** A rake left in a dry pan, a bin of fine salt with a grade mark, and
a gallery wall where a new name has been cut beside a very old one.

---

## 39. APPENDIX Y — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Salt price | economy creep | counts only |
| Smuggling plot | tone | deliveries recorded |
| Gas spectacle | disaster porn | plain work |
| Treasure gallery | loot framing | names and records |
| Free evaporation | no cost | weather leads |
| Purity snobbery | status | grades by use |
| Hero driller | fantasy | crews |
| Hidden shortfall | dishonesty | reason recorded |
| Road as system | scope | supply only |
| Ward diagnosis | medical creep | supply only |

The list exists because a salt works is easy to write as either an economy
lever or a disaster setting. The expansion's rule is that salt is counted,
graded, and delivered, that a shortfall has a reason, and that the mine's best
room is a wall with forty names on it.

---

## 40. APPENDIX Z — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Veins | 12 | 3,000 |
| Wells | 8 | 2,000 |
| Pans | 10 | 2,500 |
| Boiling runs | 8 | 2,000 |
| Grades | 5 | 2,000 |
| Bins | 10 | 2,500 |
| Issues | 16 | 3,000 |
| Deliveries | 10 | 2,500 |
| Gallery records | 14 | 3,000 |
| Store rows | 10 | 2,500 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 18 | 3,000 |
| Endings | 6 | 3,000 |
| **Total** | | **~58,500** |

---

## 41. APPENDIX AA — FIRST PAN YEAR

| Month | Focus | Milestone |
|---|---|---|
| 1 | books | four veins registered |
| 2 | vein | Long Room discovered |
| 3 | wells | W-04 rested |
| 4 | pans | covers repaired |
| 5 | dry start | late season noted |
| 6 | rain | two pans lost |
| 7 | boil | first costed run |
| 8 | grade | first correction written |
| 9 | shortfall | D-3 delivered short |
| 10 | pass | closes early |
| 11 | winter | reserve drawn |
| 12 | review | pan plan posted |

Twelve months and the shape is the trade's whole lesson: the works can survive
a bad year, and it cannot survive a bad year twice if the first one is not
written down. The twelfth row's posted plan is the expansion's real product,
and it is made of paper rather than salt.

---

## 42. APPENDIX AB — CREW TABLE

| # | Crew | Work | Size | Season | Check |
|---|---|---|---|---|---|
| 1 | Mining crew | faces, ore | 6 | year | lamp round |
| 2 | Haul crew | trains, bins | 3 | year | count |
| 3 | Pump crew | wells, lines | 2 | year | test |
| 4 | Pan crew | rake, cover | 4 | dry season | weather |
| 5 | Boil crew | kettles, fuel | 3 | wet season | cost |
| 6 | Grade crew | sieve, bin, mark | 3 | autumn | book |
| 7 | Issue crew | all | 2 | any | bin |
| 8 | Delivery crew | barrels, roads | 4 | treaty days | ledger |
| 9 | Road crew | pass, siding | 3 | winter | issue |
| 10 | Record crew | gallery, ledger | 1 | any | rubbing |

Ten crews and the tenth is the smallest and the one the expansion closes on: a
single recorder with paper and a soft pencil, copying names off a wall because
the mine's history belongs to the shelter and not to the salt.

---

## 43. APPENDIX AC — TEACHING TABLE

| # | Lesson | Audience | Method | Record |
|---|---|---|---|---|
| 1 | Rake a pan | pan crew | pans | sheet |
| 2 | Read weather | pan crew | sky, almanac | sheet |
| 3 | Fire a kettle | boil crew | house | cost sheet |
| 4 | Grade a load | grade crew | sieves | grade book |
| 5 | Test brine | pump crew | jars | test sheet |
| 6 | Count an issue | issue crew | bins | ledger |
| 7 | Copy a name | record crew | wall | rubbing |
| 8 | Write a shortfall | delivery crew | ledger | ledger |

Eight lessons and the eighth is the one nobody wants to teach: writing down a
shortfall with its reason is a skill, and the works trains it like any other
because a ledger that only records successes cannot be planned against.

---

## 44. APPENDIX AD — SALT WORKS CHARTER

| Clause | Promise |
|---|---|
| Booked | Every vein and gallery has a name and a record |
| Tested | Every well is tested on schedule and rested when high |
| Weathered | Pans belong to the weather; covers are honest |
| Costed | Every boiling run records its fuel |
| Graded | Five grades, no loose sorting, corrections recorded |
| Reserved | A reserve floor exists and moves only by decision |
| Delivered | Deliveries are made or missed, and shortfalls carry reasons |
| Roaded | Road salt is issued before the frost, not after the pass closes |
| Named | The gallery names are copied and filed |
| Kept | The winter is planned before it arrives |

The salt works charter is the expansion's first-class design object, posted in
the mine office beside the vein books. Its last clause is the whole trade: salt
is the shelter's way of making a promise to a season that has not happened yet,
and the works is the small institution that keeps that promise on paper before
it keeps it in barrels.

---

## 45. APPENDIX AE — WORKS SUCCESSION TABLE

| Role | First | Successor | Handover |
|---|---|---|---|
| Pan man | Sedge | pan hand | one dry season |
| Boiling house | Ottiline | kettle hand | one wet season |
| Vein keeper | Tansy | office hand | one vein cycle |
| Grade mistress | Dora | grade hand | one bin year |
| Saline clerk | Juna | ward clerk | one batch cycle |
| Delivery clerk | Ashe | dock hand | one treaty year |
| Gallery recorder | Weft | record hand | one rubbing |
| Road salt | Della | haul hand | one winter |

The succession table is measured in seasons and winters, and the pan man's
handover takes a full dry season because the pans teach weather rather than
rules. The gallery recorder's row is the shortest and the one the expansion
treats as most durable: teaching someone to copy a name takes an afternoon,
and the first rubbing they bring back is filed beside a hundred years of work.

---

## 46. APPENDIX AF — REVIEW TABLE

| # | Question | Answer source | Action |
|---|---|---|---|
| 1 | Was the dry season used fully? | pans | adjust rota |
| 2 | What did boiling cost? | runs | raise covers |
| 3 | Which grade was miscounted? | grade book | retrain |
| 4 | Was the reserve drawn? | issues | raise floor |
| 5 | Which delivery was short? | ledger | plan next |
| 6 | Did the pass close early? | road log | issue earlier |
| 7 | Which well tested high? | well log | rest more |
| 8 | Which vein yielded best? | mine | assign crew |
| 9 | What did the year teach? | crew | one lesson |
| 10 | What is next year's pan plan? | meeting | post it |

Ten questions asked once a year with the vein books and the ledger open, and
the answers are supposed to be numbers that change one habit each. The tenth
is posted in autumn, and the shelter walks into winter already knowing what
the salt is for.

---

## 47. APPENDIX AG — SALT ARITHMETIC (WORKED)

Evaporation is area times weather. A pan of forty square metres loaded with
four thousand eight hundred litres yields about a hundred and forty-five
kilograms of pan salt in a good dry week, and the whole works' ten pans in a
full dry season yield about seven hundred and eighty-four kilograms. The
shelter's five uses need roughly a thousand and forty: two hundred and forty
for food preservation, a hundred and eighty for the herd, twenty for the ward,
sixty for the reagent works, three hundred and sixty for the roads and rail,
and a hundred and eighty for the treaties. The gap is the shortfall, and the
boiling house can close about half of it at a fuel cost of roughly half a
kilogram of fuel per kilogram of salt, which is why the works' improvement arc
is measured in recovered kilograms and covered pans rather than in a new
mine.

The delivery arithmetic is integer barrels and reasons. A treaty of forty
barrels is forty barrels; a dry season makes thirty-six, and the ledger reads
thirty-six made, four short, reason dry season, carried forward. That is the
expansion's whole economic design: no prices, no negotiation, no fudging, and
a record a stranger could audit.

---

## 48. APPENDIX AH — TONE WATCHLIST

| Temptation | Why it fails | House rule |
|---|---|---|
| Salt barons | economy creep | no prices |
| Smuggler plot | tone | recorded deliveries |
| Deadly mine | disaster porn | plain work |
| Treasure wall | loot framing | names and records |
| Magic brine | alchemy | tested bands |
| Free wealth | no scarcity | weather leads |
| Ward vivisection | medical creep | saline supply only |
| Hero pan man | fantasy | crew credit |

The watchlist exists because mines and salt carry centuries of narrative
baggage. The expansion keeps them as bookkeeping with weather: veins with
grades, pans with covers, kettles with fuel costs, barrels counted, and names
copied off a wall.

---

## 49. APPENDIX AI — SMALL MISTAKES TABLE

| Mistake | Consequence | Lesson |
|---|---|---|
| Missed a cover | lost pan load | watch the sky |
| Raked wet salt | caking | wait for crust |
| Overfueled kettle | cost overrun | measure fuel |
| Crossed grades | wrong use | sieve, verify |
| Tasted saline | dose uneven | weigh it |
| Salted the road late | pass closes | issue at frost |

Six mistakes, each costing a load or a week, and the fifth is the one that
goes into the ward's practice: a medical salt measured by taste is a supply
line pretending to be a habit, and the works and the ward fix it together with
a balance and a marked line.

---

## 50. APPENDIX AJ — TREATY CONTEXT TABLE

| # | Treaty | Partner | Salt Use | Delivery | Note |
|---|---|---|---|---|---|
| 1 | D-1 | ridge outpost | food, road | 40 barrels | full |
| 2 | D-2 | river crews | fish curing | 60 barrels | full |
| 3 | D-3 | rail siding | freight, road | 30 barrels | short |
| 4 | D-4 | neighbor settlement | herds | 50 barrels | full |
| 5 | D-5 | waystation | food | 20 barrels | full |
| 6 | D-6 | hunting camp | hides, meat | 25 barrels | full |
| 7 | D-7 | lowland farms | herds | 35 barrels | full |
| 8 | D-8 | lake folk | fish | 45 barrels | short |
| 9 | D-9 | teaching post | table salt | 15 barrels | full |
| 10 | D-10 | pass keepers | road | 30 barrels | missed |

The treaty table is the expansion's external face, and its two shortfalls and
one miss are the whole point: the shelter's salt promises are visible, counted,
and occasionally unmet, and the record lives beside the reasons. The tenth row
is the one the works disputes — the pass was closed before the delivery — and
the dispute is part of the ledger rather than an embarrassment to be erased.

---

## 51. APPENDIX AK — WINTER ROAD TABLE

| # | Week | Road state | Salt issued kg | Note |
|---|---|---|---|---|
| 1 | pre-frost | open | 80 | issued early |
| 2 | first snow | open | 120 | pass |
| 3 | freeze | icy | 140 | rail siding |
| 4 | storm | closed 2 d | 0 | no delivery |
| 5 | thaw | muddy | 60 | haul road |
| 6 | refreeze | icy | 100 | pass again |
| 7 | hard freeze | risky | 40 | emergency only |
| 8 | spring thaw | open | 20 | cleanup |

Eight winter weeks and the first row is the lesson from the bad year: the salt
is issued before the frost, because waiting for the first snowfall means the
road that carries the salt is already the road that needs it. The eighth row's
spring cleanup is the habit that keeps the pass usable into the muddy season,
and it is the least dramatic line in the whole expansion.

---

## 52. APPENDIX AL — RESERVE DECISION TABLE

| # | Draw | Amount kg | Reason | Witness | Replaced |
|---|---|---|---|---|---|
| 1 | 1 | 12 | short year table | store keeper | spring |
| 2 | 1 | 4 | ward saline | ward clerk | spring |
| 3 | 2 | 40 | pass closed | works foreman | summer |
| 4 | 2 | 10 | treaty D-10 | delivery clerk | summer |
| 5 | 3 | 0 | none | — | — |
| 6 | 4 | 0 | none | — | — |

The reserve decision table is deliberately dull, and its fifth and sixth rows
are the best entries in the ledger: two years in a row with no draw, which is
what a reserve is for. The first four rows prove the rule works under pressure
— every draw has a reason, a witness, and a replacement season, and the
replacement column is a promise rather than an accounting trick.

---

## 53. APPENDIX AM — MINE OFFICE TABLE

| # | Book | Kept by | Reviewed | Content |
|---|---|---|---|---|
| 1 | Vein book | Tansy | monthly | faces, grades, yields |
| 2 | Crew board | Tansy | weekly | shift and count |
| 3 | Well log | pump crew | monthly | tests, rests |
| 4 | Pan plan | Sedge | weekly | loads, weather |
| 5 | Boil cost book | Ottiline | per run | fuel and yield |
| 6 | Grade book | Dora | weekly | bins and corrections |
| 7 | Issue ledger | works clerk | daily | requesters, reserve |
| 8 | Delivery ledger | Ashe | per treaty | made, short, reason |
| 9 | Gallery ledger | Weft | as found | names and rubbings |
| 10 | Store book | store keeper | weekly | damp, caking, floor |

Ten books and none of them is a price list, which is the expansion's economic
design in a single table: the works knows everything about salt except what it
is worth. The ninth row is the one the shelter reads aloud at the year review,
and the third is the one that has prevented more trouble than any other — a
well rested on schedule is a well that does not fail in February.

---

## 54. APPENDIX AN — GRADE MARK TABLE

| # | Mark | Stamp | Applies to | Verified | Note |
|---|---|---|---|---|---|
| 1 | R | square stamp | rock bins | monthly | road and stock only |
| 2 | P | round stamp | pan bins | weekly | wash before food |
| 3 | F | fine stamp | fine bins | weekly | table and kitchen |
| 4 | D | block stamp | feed bins | monthly | herd and poultry |
| 5 | M | cross stamp | medical jar | each batch | ward only |
| 6 | X | pull stamp | pulled bin | as pulled | never issued |

The grade mark table is the works' last line of defense against its own
busiest day: a stamp takes a second, a bin without a mark takes a stranger ten
minutes and a guess, and the pulled mark exists because the only mistake worse
than mixing grades is hiding the mix. The fifth row's cross stamp is held by
the ward's clerk, which the works agreed to without argument.

---

## 55. CLOSING STATEMENT

ASHFALL already has a live salt mine with veins, crews, pumps, contamination,
treaty deliveries, a save version, a host surface, and a panel that shows six
labels — and no single line of authored salt data. The Salt Pan gives the white
business its books: data veins and gallery names, wells and tests, pans and
boiling houses, five honest grades, issue lists with a visible reserve, treaty
deliveries that are made or missed with a reason, road salt for the pass, and
the inscriptions that turn a mine into a place. It adds no price and no
permission, only a promise the shelter can keep every winter: the meat is
salted, the herd is salted, the ward is supplied, and the pass stays open.

> Wave 10 note: this plan is one of five Wave 10 expansion bibles (57–61). Each
> is self-contained; none requires another to ship. The shared Wave 10 index
> lives at `docs/expansions/wave10/WAVE10_INDEX.md`. The safe pre-signature
> step is Phase 1 (data schemas and validators), which is additive and
> reversible. Evidence anchors:
> `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` (454 lines;
> `SaltMineVeinState`, `TreatyDeliveryRecord`, `SaltMineState`; `RegisterVein`,
> `UnlockVein`, `AssignWorkers`, `TickDaily`, `ReplaceDrill`, `RepairPump`,
> `SetPower`, `IsTreatyFulfilled`, `GetDeliveryCount`, `CaptureState`,
> `RestoreState`), `src/Foundry/SilentFoundryHostSession.cs` (`RegisterVein`
> `:582`; `OnTreatyDeliveryAccepted`/`OnTreatyDeliveryMissed` `:208–209`),
> `src/UI/BrineExtractionPanel.cs`, `Assets/Ashfall.Core/ExpansionHubSave.cs`
> (`saltMine` v6, lines 54–55, 350–433), `Assets/Ashfall.Core/Foundry/BrineWaterSystem.cs`,
> items `item_preservation_salt`, `item_rock_salt_sack`, `item_trade_salt_sack`,
> `item_medical_saline_salt`, `item_chem_clarity_salts`, and the narrative log
> `salt_mine_inscriptions.json` (5,036 B). No salt catalog file exists today;
> veins are code-registered.