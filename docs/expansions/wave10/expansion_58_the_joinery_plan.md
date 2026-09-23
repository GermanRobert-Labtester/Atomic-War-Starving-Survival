# ASHFALL — Expansion 58 Design Bible
# THE JOINERY
### Wave 10 · Timber, Sawing, Seasoning, Joints, Shoring, Dry Rot, Preservation, and the Frames That Hold

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-24
**Domain owners touched:** `Ashfall.Core.Narrative` (`TimberCarpentryCatalog`), `Ashfall.Core.Crafting` (`CraftingSystem`, `ShelterWorkshopSystem`), `Ashfall.Core` (`SubterraneanSystem` — shoring needs)
**Proposed host owner:** `JoineryHostSession` (extends the workshop and build surfaces)
**Existing save sections:** `crafting` (workbench jobs and workshop state), `subterranean` (mine and tunnel state)
**Existing CLI verbs:** `--data-integrity-selftest`, `--content-utilization-selftest`, `--panel-bind-lifecycle-selftest` (no joinery-specific verb exists)
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL builds things. `CraftingSystem` owns recipes and workbench jobs;
`ShelterWorkshopSystem` owns the bench and its condition; `SubterraneanSystem`
models the tunnels, adits, and mines whose roofs need holding;
`ShelterDecorSystem` places what people make; the crafting catalogs already
carry carpentry references in tool tables (`Wheelbarrow` — wood, iron; `Screen`
— wood, wire; `Pottery wheel` — wood, stone; `Mold` — wood, clay), and
expansion 31's kiln plan routes its wooden tools through carpentry. Everyone
consumes wood. Nobody owns it.

What does not exist: a sawpit, a seasoning yard, a joint, a peg, a shoring set,
a rot survey, a creosote retort, a moisture reading, a plan, or a record. There
are no timber items in `items.json` at all — no rough plank, no seasoned
board, no beam, no peg. The four authored trade logs sit in the narrative store
unread: `mortise_tenon_failure_reports.json` (5,246 B: `framing_assembly_id`,
`joint_geometry_type`, `peg_material_species`, `joint_load_kilonewtons`),
`timber_creosote_treatment_logs.json` (5,781 B: `treatment_retort_id`,
`wood_species_treated`, `retention_kg_m3`, `penetration_depth_mm`),
`timber_dry_rot_fruiting_records.json` (5,294 B: `infestation_site_id`,
`fungal_species_identified`, `timber_moisture_content_pct`,
`affected_area_sq_meters`), and `square_set_shoring_audits.json` (5,950 B:
`stope_location_id`, `timber_framing_system`, `measured_rock_pressure_mpa`,
`set_deflection_mm`). `TimberCarpentryCatalog` models all four
(`MortiseTenonFailureEntry`, `TimberCreosoteTreatmentEntry`,
`TimberDryRotFruitingEntry`, `SquareSetShoringEntry`) and is referenced by zero
runtime files.

**The Joinery** is the expansion about wood as a maintained material: the
sawpit, the drying stack, the joint, the peg, the shoring set, the rot survey,
the treated beam, and the drawings that let a shelter repair its own frames
instead of hoping they hold.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| 32 The Wild (Wave 5) | Forests, trees, wildlife | Buys standing timber; owns conversion onward |
| 31 The Kiln (Wave 4) | Fired earth, lime, concrete | Wood only; no second masonry |
| 18 The Underneath (Wave 2) | Tunnels, mines, collapses | Supplies and audits shoring sets |
| 14 Above the Ash (Wave 1) | Surface structures | Supplies framed parts; no second structures |
| 10 The Silent Foundry (Exp 10) | Metal tool heads | Wooden handles, hafts, and fittings |
| 40 The Wheel (Wave 6) | Machines | Wood components; no machine authority |
| 47 The Brigade (Wave 8) | Fire safety | Creosote and sawdust routes through it |
| 39 The Reagent (Wave 6) | Chemistry | Preservatives made by it; used here |
| `ShelterDecorSystem` | Placed decor | Supplies furniture blanks; no second decor |
| `CraftingSystem` | Recipes and jobs | Adds wood recipes; no second crafting |
| `EquipmentConditionSystem` | Tool wear | Edge tools register as tracked |
| 56 The Calendar (Wave 9) | Ceremonies | Roof-raisings and handovers as authored beats |
| 57 The Hour (Wave 10) | Clock cases | Supplies cases; no time authority |
| 33 The Weather (Wave 5) | Seasons, storms | Moisture and rot respond to weather |
| `StandingRecord` | Records | Files the drawings and repair log |

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The shelter's doorframes are green wood, its dormitory wall has a fungus
behind the paint, its mine sets are deflecting two millimetres more each
season, and the only person who knows how the roof trusses were joined is
seventy-one and has never drawn them. The sawpit has a dull blade, the
drying stacks are under a leaking lean-to, and the creosote retort next to the
foundry is one spark from a serious week.

**The Joinery** is the expansion about the material that holds the shelter
together and the trade that keeps it holding: felling agreements, sawing,
seasoning, joints, pegs, shoring, dry rot, preservation, drawings, and the
long, patient argument that a building made of wood is a building you
maintain, not a building you replace.

### 1.2 The five loops it adds

```
  Cut ──► Saw ──► Dry ──► Join ──► Hold
    │       │       │       │        │
    ▼       ▼       ▼       ▼        ▼
 woodlot  sawpit  stacks  joints   shoring
 plans    blades  moisture pegs    sets
                              │
                              ▼
                  Treat ──► Survey ──► Repair ─► Draw
```

### 1.3 What the player manages

1. **The woodlot.** Cutting plans, species requests, and rotations.
2. **The sawpit.** Blades, kerfs, pit crews, and sawing days.
3. **The stacks.** Seasoning, moisture, lean-to shelter, and grade.
4. **The bench.** Joints, pegs, fittings, doors, frames, and blanks.
5. **The shoring.** Sets, pressure, deflection, and audits.
6. **The rot.** Surveys, moisture, affected areas, and treatment.
7. **The retort.** Creosote runs, retention, penetration, and safety.
8. **The repairs.** Sills, frames, stairs, and the standing structure.
9. **The drawings.** Plans, patterns, and the taught set.
10. **The records.** Repair logs, failures, and lessons.

### 1.4 What it is not

- Not a second masonry, concrete, or building-permanence system.
- Not a second crafting system; recipes stay with `CraftingSystem`.
- Not a forest authority; living trees and wildlife stay with expansion 32.
- Not a mine authority; tunnels and collapses stay with expansion 18.
- Not a fire system; sawdust and retort safety route through expansion 47.
- Not a decor system; placed furniture stays with `ShelterDecorSystem`.
- Not a metal system; tool heads come from the foundry.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` | Recipes, jobs | `LIVE` |
| `Assets/Ashfall.Core/Crafting/ShelterWorkshopSystem.cs` | Bench and condition | `LIVE` |
| `Assets/Ashfall.Core/SubterraneanSystem.cs` | Tunnels, mines, collapse | `LIVE` |
| `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs` | Placed decor | `LIVE` |
| `Assets/Ashfall.Core/EquipmentConditionSystem.cs` | Tool wear | `LIVE` |
| `Assets/Ashfall.Core/Inventory/Inventory.cs` | Items and stacks | `LIVE` |
| `Assets/Ashfall.Core/Farming/*` | Living wood | `LIVE` (boundary) |
| `NeedsSystem` | Morale sink | `LIVE` |
| `StandingRecord` | Records | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Rows | Notes |
|---|---|---|
| `mortise_tenon_failure_reports.json` | 4+ entries, 5,246 B | joints and loads; unread |
| `timber_creosote_treatment_logs.json` | 4+ entries, 5,781 B | retention, penetration; unread |
| `timber_dry_rot_fruiting_records.json` | 4+ entries, 5,294 B | moisture, area; unread |
| `square_set_shoring_audits.json` | 4+ entries, 5,950 B | pressure, deflection; unread |
| Timber, joint, stack, shoring catalogs | **0** | confirmed absent |
| Timber items in `items.json` | **0** | confirmed absent |

### 2.3 Confirmed gaps

- **GAP-58-1 — No timber exists as a graded material or item.**
- **GAP-58-2 — No sawing, blades, or sawpit work.**
- **GAP-58-3 — No seasoning, moisture, or drying stacks.**
- **GAP-58-4 — No joint catalog, pegs, or load knowledge.**
- **GAP-58-5 — No shoring sets, pressure, or deflection audits.**
- **GAP-58-6 — No dry rot detection, area, or treatment.**
- **GAP-58-7 — No preservation retort or safety practice.**
- **GAP-58-8 — No structural repair queue for wood members.**
- **GAP-58-9 — No drawings, patterns, or taught set.**
- **GAP-58-10 — Four authored carpentry logs are read by nothing.**

### 2.4 Non-duplication statement

This expansion adds **no** second crafting, masonry, forestry, mining,
fire, decor, or record system. It extends `CraftingSystem` and the workshop
with wood-family recipes and jobs, routes living trees through expansion 32,
shoring needs through expansion 18, fire safety through expansion 47,
preservatives through expansion 39, and files its drawings with
`StandingRecord`. All new state is additive inside the existing `crafting`
save owner. No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Wood is a season, not a purchase.** A board is cut in spring,
turned in summer, and used in winter, and the shelter plans in that rhythm.

**Pillar 2 — A joint is knowledge.** The difference between a shelter and a
pile of lumber is whether anyone remembers how the pieces were meant to meet.

**Pillar 3 — Rot is found by looking.** Dry rot does not announce itself; the
trade is the quarterly walk with a lamp and a knife.

**Pillar 4 — Shoring is certainty.** A timber set is not decoration; it is a
measured promise that the roof has another year.

**Pillar 5 — Draw it or lose it.** Plans belong to the shelter, and a drawing
is the cheapest way to make a skill survive its owner.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Sawpit | Teamwork, rhythm, sweat | Sawdust heroics |
| Seasoning | Patience, grades, marking | Magic wood |
| Joints | Fit, test, correction | Perfectionist drama |
| Shoring | Measurements, audits, certainty | Collapse spectacle |
| Dry rot | Survey, moisture, containment | Horror fungus |
| Creosote | Retort discipline, fire rules | Chemical thrill |
| Repairs | Sills, stairs, doors | Grand rebuilding |
| Drawings | Plans, patterns, teaching | Secret guild knowledge |

### 3.3 Content limits

- No real timber companies, forests, or species claims copied.
- No felling of sacred or protected trees; no tree violence for spectacle.
- No collapse deaths as entertainment; shoring failures are costly and rare.
- No poison treatment framing; preservatives are handled with plain rules.
- No second forest or wildlife system; the woodlot buys standing timber.
- No magical rot cures; moisture and airflow are the treatment.
- No new save section.

---

## 4. THE JOINERY WORLD

### 4.1 Interior rooms

- **`room_sawpit_works`** — the pit, the trestles, and the blade rack.
- **`room_drying_loft`** — the stacks, the vent, and the moisture board.
- **`room_joinery_bench`** — the bench, the chisels, the pegs, and the plans.
- **`room_pattern_store`** — drawings, templates, and the taught set.
- **`room_shoring_store`** — timber sets, wedges, and the audit board.
- **`room_rot_survey_post`** — the lamp, the knife, and the maps of walls.
- **`room_retort_yard`** — the creosote retort and its fire rules.
- **`room_repair_shop`** — sills, frames, doors, and the next job list.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_woodlot_line` | The Woodlot Line | 4 | Cutting plans and stakes |
| `loc_sawpit_camp` | The Sawpit Camp | 4 | Field sawing and breaks |
| `loc_stack_yard` | The Stack Yard | 3 | Seasoning in the open |
| `loc_charcoal_shed` | The Charcoal Shed | 5 | Retort and fuel boundary |
| `loc_old_joiner` | The Old Joiner | 6 | Ruined workshop, plans |
| `loc_beam_bridge` | The Beam Bridge | 5 | A wooden bridge needing care |
| `loc_landing_platform` | The Landing Stage | 5 | Hauling and loading |
| `loc_drift_mouth` | The Drift Mouth | 5 | Shoring at the mine entrance |
| `loc_rot_wall` | The Rot Wall | 6 | A standing teaching wall |
| `loc_mark_grove` | The Mark Grove | 4 | Trees marked for years ahead |

All locations must resolve in `locations.json` and pass the map loader gate.

### 4.3 The rhythm

Cut in spring, saw when the crew is dry, stack and turn through summer, survey
and treat in autumn, join and repair in winter when the bench is warm. The
year is the joinery's clock and the moisture board is its calendar.

---

## 5. MAIN STORYLINE — "THE JOINTS THAT HOLD"

### 5.1 Central conflict

**Tib Rennet** is the shelter's joiner and has never drawn a single plan
because there was never time to draw them. **Ulla Grove** runs the sawpit with
a blade that has been sharpened past its set. **Sol Eller** watches the stacks
and knows that half the shelter's timber went in green, which is why the
dormitory doors no longer close in summer. **Tild Marrow** audits the mine sets
and has been reporting two millimetres of deflection for three seasons to a
board nobody reads. **Della Nare** finds rot behind the dormitory's paint and
is told it is only damp.

Then a February storm takes the dormitory doorframe out of the wall, and the
shelter discovers what the joinery already knew: the doors that stopped closing
were a warning, the load paths run through the frames, and nobody can read the
roof because the roof was never drawn. The repair takes eleven days, one
drawing, and one hundred and forty pegs. By spring the shelter has a
seasoning yard, a drawn set, a survey route, and a rule that no timber goes
into a wall wet.

The expansion's question: **what does a shelter owe the frames that hold it
up?**

### 5.2 Theme (unspoken)

**A building is only as permanent as the people who still know how it was
joined.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_joiner_tib_rennet` | Tib Rennet | Joiner | Bench, joints, plans |
| `npc_sawyer_ulla_grove` | Ulla Grove | Sawyer | Sawpit and blades |
| `npc_seasoning_sol_eller` | Sol Eller | Seasoning | Stacks and moisture |
| `npc_shoring_tild_marrow` | Tild Marrow | Shoring auditor | Sets, pressure, audits |
| `npc_surveyor_della_nare` | Della Nare | Rot surveyor | Moisture and rot |
| `npc_framer_noll_yarrow` | Noll Yarrow | Framer | Frames and sills |
| `npc_apprentice_verne_lunt` | Verne Lunt | Apprentice | Pegs, patterns, drawings |
| `npc_woodlot_lorn_kest` | Lorn Kest | Woodlot liaison | Cutting plans with 32 |

### 5.4 Story beats (15)

1. **Green Wood.** The doors stop closing in summer.
2. **The Dull Blade.** Ulla's saw is past its set.
3. **The Measured Set.** Tild's deflection reports are filed.
4. **The Damp Wall.** Dry rot is found behind the paint.
5. **The Storm.** The doorframe leaves the wall.
6. **The Eleven Days.** The repair is drawn as it is done.
7. **The Pegs.** One hundred and forty pegs are cut and driven.
8. **The Stack.** A seasoning yard is laid out and covered.
9. **The Tests.** Joints are loaded and measured.
10. **The Retort.** A creosote run is made under fire rules.
11. **The Route.** The quarterly rot walk begins.
12. **The Plans.** The drawn set becomes the shelter's.
13. **The Teaching.** An apprentice learns the set by cutting it.
14. **The Set Replaced.** The mine's worst set is renewed.
15. **The Joints That Hold.** The shelter inspects its own frames.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Harvest | own woodlot / buy standing / salvage only | supply |
| Seasoning | full year / mill rule / use green | patience |
| Joints | pegged / nailed / mixed | craft |
| Shoring | audit quarterly / annually / on report | diligence |
| Rot | treat / replace / watch | method |
| Retort | full runs / limited / none | risk |
| Drawings | full set / key frames / memory | knowledge |
| Final | drawn trade / one master / shared bench | identity |

### 5.6 Endings (5 + fade)

1. **The Seasoned Yard** — the shelter's timber is dry, graded, and marked,
   and nobody builds wet again.
2. **The Drawn Set** — the frames are drawn, taught, and repaired by whoever
   is on the bench that winter.
3. **The Long Set** — every shoring set is audited and every audit is read,
   and the mine has decades left in it.
4. **The Dry Wall** — rot is routed along the survey walk, wall by wall, and
   the dormitory survives its hundredth winter.
5. **The Standing Frame** — the shelter builds nothing it cannot repair, and
   says so in one line on the plan.
6. **Fade** — a peg bag on a bench, a moisture board at the end of the day,
   and a wall whose newest plank is marked with the year it was cut.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_joinery_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_joinery_green_wood`, `quest_joinery_dull_blade`,
`quest_joinery_measured_set`, `quest_joinery_damp_wall`,
`quest_joinery_storm`, `quest_joinery_eleven_days`, `quest_joinery_pegs`,
`quest_joinery_stack`, `quest_joinery_tests`, `quest_joinery_retort`,
`quest_joinery_route`, `quest_joinery_plans`, `quest_joinery_teaching`,
`quest_joinery_set_replaced`, `quest_joinery_joints_that_hold`.

### 6.2 Side quests (30)

**Supply (5)**
- `quest_joinery_woodlot` — cutting plan agreed
- `quest_joinery_stakes` — trees marked
- `quest_joinery_haul` — hauling arranged
- `quest_joinery_salvage` — ruins timber recovered
- `quest_joinery_species` — species chosen

**Saw (5)**
- `quest_joinery_blade` — blade set and sharpened
- `quest_joinery_pit` — pit crew trained
- `quest_joinery_kerf` — kerf measured
- `quest_joinery_planks` — planks cut to size
- `quest_joinery_waste` — offcuts saved

**Dry (5)**
- `quest_joinery_stacks` — stacks laid
- `quest_joinery_cover` — lean-to repaired
- `quest_joinery_moisture` — moisture checked
- `quest_joinery_turn` — sticks turned
- `quest_joinery_grade` — grades marked

**Join (5)**
- `quest_joinery_mortise` — mortises cut
- `quest_joinery_tenon` — tenons fitted
- `quest_joinery_pegs` — pegs dried and driven
- `quest_joinery_tests` — joints load-tested
- `quest_joinery_doors` — doors hung true

**Hold (5)**
- `quest_joinery_survey` — quarterly walk
- `quest_joinery_rot` — rot area mapped
- `quest_joinery_treat` — affected wood treated
- `quest_joinery_sets` — shoring set renewed
- `quest_joinery_audit` — deflection audit read

**Draw and teach (5)**
- `quest_joinery_plan` — a frame drawn
- `quest_joinery_pattern` — a template kept
- `quest_joinery_set_book` — the drawn set bound
- `quest_joinery_apprentice` — a peg lesson taught
- `quest_joinery_lesson` — one lesson kept

### 6.3 Repeatable quests (8)

`quest_joinery_repeat_saw`, `quest_joinery_repeat_stack`,
`quest_joinery_repeat_survey`, `quest_joinery_repeat_audit`,
`quest_joinery_repeat_repair`, `quest_joinery_repeat_moisture`,
`quest_joinery_repeat_teach`, `quest_joinery_repeat_draw`.

### 6.4 Dynamic hooks

Live events (storms, tunnel events, mine ticks, workshop jobs, fire checks,
weather, decor placement, repair events) attach authored follow-ups through
existing seams. No new event bus.

### 6.5 Constraints

- Recipes and jobs stay with `CraftingSystem`.
- Bench condition stays with `ShelterWorkshopSystem`.
- Tunnels and collapses stay with expansion 18.
- Living trees stay with expansion 32.
- Decor stays with `ShelterDecorSystem`.
- Fire safety routes through expansion 47.
- Drawings file with `StandingRecord`.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `TimberYardSystem` (new, `Ashfall.Core.Crafting`)

**Owns:** logs, grades, species, stacks, moisture, and seasoning state.
**Consumes:** woodlot supply, weather, the drying loft. **Data:**
`timber_species.json`, `timber_grades.json`, `seasoning_stacks.json`.
**Rules:** green timber is honest but not structural; grades are visible and
marked; moisture is measured, never guessed; the yard plans in seasons.

### 7.2 `SawpitSystem` (new, thin, `Ashfall.Core.Crafting`)

**Owns:** sawing: blades, set, kerf, pit crews, and sawing days. **Consumes:**
logs, `EquipmentConditionSystem`, workshop. **Data:** `sawpit_blades.json`,
`saw_cuts.json`. **Rules:** a dull blade wastes a log; sawing is a crewed
rhythm, not a solo feat; offcuts are kept and sorted.

### 7.3 `JoinerySystem` (new, `Ashfall.Core.Crafting`)

**Owns:** joints (mortise, tenon, lap, scarf, dovetail), pegs, fittings,
doors, frames, and blanks. **Consumes:** seasoned stock, bench, plans.
**Data:** `joint_types.json`, `joinery_jobs.json`. **Rules:** joints are
tested before they are trusted; pegs are dried and sized; a fitting that
fails is remade, not forced.

### 7.4 `ShoringSystem` (extend `SubterraneanSystem`)

**Owns:** timber sets, wedges, pressure readings, deflection audits, and set
replacement. **Consumes:** the mine state and design loads. **Data:**
`shoring_sets.json`. **Rules:** every set has an audit date; deflection is
reported to the record and read by someone; a failing set is replaced before
it fails.

### 7.5 `DryRotSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** surveys, moisture readings, affected areas, fruiting records, and
containment. **Consumes:** weather, the survey route, treatment items.
**Data:** `dry_rot_sites.json`. **Rules:** rot is found by looking; treatment
is moisture and airflow first; the area is measured and the wall is drawn; the
survey is a walk, not an alarm.

### 7.6 `PreservationSystem` (new, thin, `Ashfall.Core.Crafting`)

**Owns:** creosote runs: retort, batch, retention, penetration, and storage.
**Consumes:** chemistry (39), fire safety (47), the retort yard. **Data:**
`preservation_runs.json`. **Rules:** the retort runs only under fire rules;
penetration is measured; treated timber is marked and never used in food or
sleep spaces without review.

### 7.7 `FrameRepairSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** structural repair jobs: sills, frames, stairs, lintels, and the
priority list. **Consumes:** the yard, the bench, the rot survey, shelter
state. **Data:** `frame_repairs.json`. **Rules:** repairs are ordered by load,
not by who complains; a temporary fix is labeled temporary; the job closes
with an inspection.

### 7.8 `JoineryPlanSystem` (new, thin, `Ashfall.Core.Narrative`)

**Owns:** drawings, patterns, the drawn set, and the taught lessons. **Data:**
`joinery_plans.json`. Files through `StandingRecord`. **Rules:** plans belong
to the shelter; a drawing names its joiner and its date; teaching cuts the
plan, not just reads it.

### 7.9 Systems explicitly not added

- No second crafting, masonry, forestry, mining, fire, decor, or record system.
- No magical rot cure, no treated-wood immunity, no instant seasoning.
- No collapse spectacle or shock deaths.
- No new RNG stream beyond the live tick and weather paths.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `timber_species.json` (new)

```json
{
  "schema_version": 1,
  "species": [
    {
      "species_id": "species_birch_straight",
      "display_name": "Straight Birch",
      "shrinkage_class": "medium",
      "season_days": 120,
      "structural": false,
      "uses": ["handles", "peg_stock", "cases"],
      "tags": ["common"]
    }
  ]
}
```

### 8.2 `timber_grades.json` (new)

Grades: green, air-dry, kiln-dry, structural, rejected, with rules.

### 8.3 `seasoning_stacks.json` (new)

Stacks: species, date, moisture curve, cover, turning rota, grade.

### 8.4 `sawpit_blades.json` (new)

Blades: tooth, set, kerf, condition, sharpening record.

### 8.5 `joint_types.json` (new)

Joints: geometry, load, material rule, peg species, failure note.

### 8.6 `shoring_sets.json` (new)

Sets: location, framing system, pressure, deflection, audit date.

### 8.7 `dry_rot_sites.json` (new)

Sites: wall, moisture, species, area, severity, treatment, review.

### 8.8 `preservation_runs.json` (new)

Runs: batch, species, retention, penetration, storage, fire check.

### 8.9 `frame_repairs.json` (new)

Repairs: member, load, cause, method, days, inspection, lesson.

### 8.10 `joinery_plans.json` (new)

Plans: frame, date, joiner, pattern, taught, revision.

### 8.11 Items

New items appended to `items.json`: `item_rough_plank`,
`item_seasoned_plank`, `item_timber_beam`, `item_peg_blank`,
`item_peg_dried`, `item_mortise_chisel`, `item_bow_saw_blade`,
`item_pit_saw_file`, `item_drawknife`, `item_plumb_bob`,
`item_timber_set`, `item_steel_wedge`, `item_creosote_batch`,
`item_rot_sample`, `item_moisture_gauge`, `item_joinery_plan_sheet`,
`item_pattern_template`, `item_sawdust_bag`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`crafting` remains the live save owner for bench and material state. Timber
yard, sawpit, joinery jobs, shoring audits (via the subterranean owner),
preservation runs, repair jobs, and plans are additive sub-objects. No new
save section.

### 9.2 State to persist

- Logs held, species, and grades.
- Stacks, moisture, and seasoning progress.
- Saw blades, condition, and sharpening.
- Joints made, tested, and their results.
- Shoring sets, pressure, deflection, and audit dates.
- Rot sites, moisture, area, and treatment.
- Preservation runs and storage.
- Repair jobs and inspections.
- Plans, patterns, and taught lessons.

### 9.3 Determinism

- Seasoning and moisture follow authored curves plus the live weather path.
- Sawing yield is determined by blade condition and species, not seeded noise.
- Joint tests are deterministic load comparisons.
- Rot progression is authored and moisture-driven; the same save trots the
  same walls.
- Shoring deflection is integer millimetres on the live mine tick.
- Paired replay hashes must match; no `System.Random`.

### 9.4 Migration

Legacy saves load with green stock, no stacks, and existing structures intact;
the rot survey starts with the walls already standing. Disabled rot state
defaults to a survey-due entry rather than a hidden infestation. The drawn set
starts empty.

### 9.5 Checksum

Invariant-culture floats for moisture, retention, and loads; integer day,
deflection, area, and count fields.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `JoineryPanel` (new) | Yard, bench, jobs | `JoineryHostSession` |
| `SawpitPanel` (new) | Blades and sawing | same |
| `SeasoningPanel` (new) | Stacks and moisture | same |
| `ShoringPanel` (new) | Sets and audits | same |
| `RotSurveyPanel` (new) | Sites and treatment | same |
| `PreservationPanel` (new) | Retort runs | same |
| `PlanStorePanel` (new) | The drawn set | same |
| `ShelterWorkshopPanel` (extend) | Wood-family jobs | existing |
| `EquipmentConditionPanel` (extend) | Edge tools | existing |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Rot severity is shown as words and numbers, never color alone.
- Retort actions state fire rules before confirmation.
- Repair priorities show their load reason, not a ranking.
- Keyboard/controller close/back preserved; focus maintained on refresh.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a pit saw's long pull, a chisel
seating a mortise, a peg driven home, a stack of boards settling, a moisture
knife tapping a sill. No cue is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `CraftingSystem` | Wood-family recipes and jobs |
| `ShelterWorkshopSystem` | Bench condition and jobs |
| `SubterraneanSystem` (Wave 2) | Shoring sets and audits |
| 32 The Wild (Wave 5) | Standing timber supply |
| 31 The Kiln (Wave 4) | Tool and material boundary |
| 14 Above the Ash (Wave 1) | Framed parts for surface works |
| 10 Silent Foundry (Exp 10) | Tool heads and steel wedges |
| 40 The Wheel (Wave 6) | Machine components |
| 47 The Brigade (Wave 8) | Retort and sawdust fire rules |
| 39 The Reagent (Wave 6) | Preservative feedstocks |
| 33 The Weather (Wave 5) | Moisture, storms, drying |
| `ShelterDecorSystem` | Furniture blanks and placement |
| `EquipmentConditionSystem` | Saw and chisel wear |
| 57 The Hour (Wave 10) | Clock cases and cases repair |
| 56 The Calendar (Wave 9) | Raising days and handovers |
| `StandingRecord` | Plans and repair logs |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm crafting, workshop, workshop condition,
subterranean, decor, equipment, wild, fire, chemistry, weather, and record
owners. Record file:line; change nothing.

**Phase 1 — Data + validators.** Author the ten catalogs; register validators
and scanner.

**Phase 2 — Pure Core.** `TimberYardSystem`, `SawpitSystem`, `JoinerySystem`,
`ShoringSystem`, `DryRotSystem`, `PreservationSystem`, `FrameRepairSystem`,
`JoineryPlanSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `JoineryHostSession`, focused selftest coverage,
fresh journey from green doors to the drawn set.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** Multi-season soak: seasoning time, blade wear, rot
spread, shoring audits, repair backlogs.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Species | 14 |
| Grades | 6 |
| Stacks | 10 |
| Blades | 8 |
| Joints | 12 |
| Shoring sets | 10 |
| Rot sites | 12 |
| Preservation runs | 8 |
| Repairs | 14 |
| Plans | 12 |
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
| Masonry overlap | High | Wood only |
| Forestry overlap | High | Buy standing timber |
| Mining overlap | High | Extend subterranean |
| Fire overlap | High | Route through 47 |
| Crafting fork | High | Extend live system |
| Collapse spectacle | High | Audits, not catastrophes |
| Determinism break | Low | Authored curves |
| Seasoning tedium | Medium | Background progress |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `timber_species.json` | 14 | 3,500 |
| `timber_grades.json` | 6 | 2,000 |
| `seasoning_stacks.json` | 10 | 2,500 |
| `sawpit_blades.json` | 8 | 2,000 |
| `joint_types.json` | 12 | 3,000 |
| `shoring_sets.json` | 10 | 2,500 |
| `dry_rot_sites.json` | 12 | 3,000 |
| `preservation_runs.json` | 8 | 2,000 |
| `frame_repairs.json` | 14 | 3,000 |
| `joinery_plans.json` | 12 | 3,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 18 | 3,000 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~58,000** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R58-1 | Masonry overlap | Med | High | Wood-only scope |
| R58-2 | Forestry overlap | Med | High | Supply boundary |
| R58-3 | Mining fork | Med | High | Extension only |
| R58-4 | Fire incident | Low | High | Retort rules |
| R58-5 | Crafting fork | Med | High | Live owner |
| R58-6 | Rot horror | Med | Medium | Survey tone |
| R58-7 | Determinism | Low | High | Authored curves |
| R58-8 | Tedium | Med | Medium | Background timers |
| R58-9 | Content overrun | Med | Medium | Budget |
| R58-10 | Plan secrecy | Low | Medium | Drawn set public |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Does the woodlot cut its own trees?** Recommended: no; standing timber
   is bought or salvaged through expansion 32, and the joinery owns conversion.
2. **Where do wet timbers live?** Recommended: additive inside `crafting`,
   with the stack state as a sub-object.
3. **Can treated timber be used in sleeping spaces?** Recommended: only after
   a documented review; the retort's products are marked.
4. **How often is the rot survey walked?** Recommended: quarterly, with an
   extra walk after storms.
5. **Who owns the drawn set when the joiner dies?** Recommended: the shelter,
   filed with `StandingRecord` and kept in the pattern store.

---

## 17. APPENDIX D — SPECIES TABLE

| # | Species | Shrinkage | Days | Structural | Uses |
|---|---|---|---|---|---|
| 1 | Straight Birch | medium | 120 | no | handles, pegs, cases |
| 2 | Ash | low | 150 | yes | tool hafts, frames |
| 3 | Oak | low | 240 | yes | sills, beams, sets |
| 4 | Elm | medium | 180 | yes | wet sills, hubs |
| 5 | Pine | high | 90 | no | crates, forms, sheathing |
| 6 | Larch | medium | 150 | yes | posts, bridges |
| 7 | Willow | high | 90 | no | baskets, floats, charcoal |
| 8 | Maple | medium | 150 | yes | benches, floors |
| 9 | Beech | high | 120 | no | pegs, mauls, blocks |
| 10 | Alder | medium | 120 | yes | piles, charcoal |
| 11 | Hazael | low | 180 | yes | long beams, drawn sets |
| 12 | Rowan | medium | 120 | no | handles, small work |
| 13 | Black Poplar | high | 90 | no | yard boards, patterns |
| 14 | Saltsedge | low | 200 | yes | outdoor frames, sills |

Fourteen species, each with a shrinkage class and a season, and the four
structural words in the table are the whole trade in miniature: oak and larch
and hazael and saltsedge get to hold roofs up, and everything else is honest
board. The tenth row, alder, exists mostly to feed the charcoal clamp in
expansion 31, which is how the joinery earns a second income on the days when
nobody needs a beam.

---

## 18. APPENDIX E — GRADES TABLE

| # | Grade | Moisture | Use | Visual rule |
|---|---|---|---|---|
| 1 | Green | >30% | rough work | drips when cut |
| 2 | Air-dry | 12–18% | indoor boards | turns freely |
| 3 | Kiln-dry | 8–12% | joinery, cases | light, true |
| 4 | Structural | <18% | bearers | straight grain |
| 5 | Rejected | any | firewood | rot, splits |

Five grades, each with a finger test and a use, and the fifth grade is not
waste: rejected timber goes to the fires and the charcoal clamp and the\drying loft's kindling box. The house rule the shelter adopts in the story is
simple: nobody has to guess what a board is for, because every board is
marked with its grade and the year it was cut.

---

## 19. APPENDIX F — SEASONING TABLE

| # | Stack | Species | Date | Moisture | Cover | Turn |
|---|---|---|---|---|---|---|
| 1 | Yard A | oak | spring 611 | 22% | lean-to | 30 d |
| 2 | Yard B | ash | spring 611 | 18% | lean-to | 30 d |
| 3 | Yard C | pine | summer 611 | 31% | tarp | 14 d |
| 4 | Loft | birch | autumn 611 | 15% | roof | none |
| 5 | Kiln | cases | winter 611 | 10% | shed | none |
| 6 | Yard D | elm | spring 612 | 26% | lean-to | 30 d |
| 7 | Yard E | larch | summer 612 | 20% | lean-to | 30 d |
| 8 | Loft | maple | autumn 612 | 14% | roof | none |
| 9 | Yard F | hazel | spring 613 | 24% | lean-to | 30 d |
| 10 | Rejected | mixed | ongoing | any | open | none |

Ten stacks and the year columns show the trade's real unit of planning: not
this week, this tree. The seventh row is the story's turning point — larch
seasoned under a proper lean-to is the first stack the shelter trusts with a
bridge, and the bridge holds for twenty years.

---

## 20. APPENDIX G — BLADE TABLE

| # | Blade | Tooth | Set | Kerf | Use | Condition |
|---|---|---|---|---|---|---|
| 1 | Pit saw long | 4 tpi | coarse | 4 mm | beams | 62 |
| 2 | Pit saw short | 6 tpi | coarse | 3 mm | planks | 71 |
| 3 | Bow saw | 10 tpi | medium | 2 mm | branches | 88 |
| 4 | Crosscut | 8 tpi | medium | 3 mm | logs | 66 |
| 5 | Ripsaw | 5 tpi | coarse | 4 mm | boards | 79 |
| 6 | Tenon saw | 12 tpi | fine | 1 mm | joints | 84 |
| 7 | Dovetail saw | 15 tpi | fine | 1 mm | fine joints | 90 |
| 8 | Frame saw | 6 tpi | medium | 3 mm | curves | 58 |

Eight blades and the first row's condition number, sixty-two, is the shelf
situation from the story's opening: the shelter's main pit saw is run past
its set and burning through logs. The blade table is also the handoff to
expansion 10, because a saw cannot be sharpened without a file, and a file
cannot be made without steel.

---

## 21. APPENDIX H — JOINT TABLE

| # | Joint | Geometry | Load | Material | Peg | Failure note |
|---|---|---|---|---|---|---|
| 1 | Mortise and tenon | framed | high | oak | oak | peg shears first |
| 2 | Lap | half | medium | mixed | oak | splits at shoulder |
| 3 | Scarf | long | high | oak | oak | slips if short |
| 4 | Dovetail | interlock | low | birch | none | wears in drawers |
| 5 | Dado | housed | medium | pine | none | cracks if dry |
| 6 | Bridle | forked | medium | ash | ash | pulls under racking |
| 7 | Housing | slot | low | mixed | peg | sags if loose |
| 8 | Peg-and-hole | pinned | low | any | oak | wobbles when wet |
| 9 | Splice | butted | high | oak | oak | needs plate |
| 10 | Birdsmouth | notched | medium | larch | none | splits at heel |
| 11 | Cog | engaged | high | oak | none | wears under load |
| 12 | Dowel | drilled | low | birch | birch | glue-dependent |

Twelve joints and the failure notes are the trade's memory written down. The
first row is the expansion's subject: the peg is supposed to be the part that
fails, because a peg is cheap to replace and a beam is not. A shelter that
understands its joints knows what is going to break before it breaks.

---

## 22. APPENDIX I — SHORING TABLE

| # | Set | Location | System | Pressure MPa | Deflection mm | Audit |
|---|---|---|---|---|---|---|
| 1 | Set A1 | drift mouth | square set | 1.4 | 2 | 90 d |
| 2 | Set A2 | gallery 1 | square set | 1.1 | 1 | 90 d |
| 3 | Set A3 | gallery 2 | post and cap | 0.9 | 0 | 90 d |
| 4 | Set B1 | adit gate | frame | 0.6 | 1 | 180 d |
| 5 | Set B2 | ramp | post and cap | 1.0 | 2 | 90 d |
| 6 | Set C1 | old stope | square set | 1.8 | 4 | 30 d |
| 7 | Set C2 | stope lip | chock | 1.2 | 2 | 90 d |
| 8 | Set D1 | water gallery | treated set | 0.8 | 1 | 90 d |
| 9 | Set D2 | sump gallery | treated set | 0.7 | 1 | 90 d |
| 10 | Set E1 | teaching drift | frame | 0.3 | 0 | 365 d |

Ten sets and the sixth row is the one Tild has been reporting for three
seasons: four millimetres of deflection in the old stope while the audit board
wasn't read. The tenth row is the expansion's teacher — a shallow teaching
drift at the mine mouth where apprentices learn to wedge a set without
standing under a real roof.

---

## 23. APPENDIX J — ROT SURVEY TABLE

| # | Site | Wall | Moisture | Species | Area m² | Treatment |
|---|---|---|---|---|---|---|
| 1 | Dormitory east | plaster | 24% | cellar fungus | 3 | dry, airflow |
| 2 | Dormitory north | plaster | 19% | surface mould | 1 | dry, paint |
| 3 | Kitchen store | render | 22% | cellar fungus | 2 | dry, replace |
| 4 | Mess hall | panelling | 17% | surface mould | 4 | dry, airflow |
| 5 | Stair core | timber | 26% | wet rot | 1 | replace treads |
| 6 | Loft edge | boards | 21% | dry rot | 2 | cut out, treat |
| 7 | Mine gate | timber | 28% | wet rot | 1 | replace post |
| 8 | Yard lean-to | boards | 23% | dry rot | 3 | cut out, replace |
| 9 | Bridge stringer | oak | 18% | none | 0 | watch list |
| 10 | Sill row west | oak | 16% | none | 0 | watch list |
| 11 | Pattern store | shelves | 20% | surface mould | 1 | dry, airflow |
| 12 | Retort shed | timber | 25% | wet rot | 2 | replace sill |

Twelve survey rows and the ninth is the one the story turns on: the bridge
stringer is dry now and goes on the watch list instead of the job list, which
is what a working survey looks like. The sixth row is the honest one about dry
rot — the fix is a knife, a saw, and a wider gap, and no chemical product in
the data acts as a miracle.

---

## 24. APPENDIX K — PRESERVATION TABLE

| # | Batch | Species | Retention kg/m³ | Penetration mm | Storage | Fire check |
|---|---|---|---|---|---|---|
| 1 | 001 | oak posts | 160 | 18 | shed | yes |
| 2 | 002 | larch rail | 140 | 15 | shed | yes |
| 3 | 003 | oak sills | 180 | 22 | shed | yes |
| 4 | 004 | alder sets | 120 | 12 | shed | yes |
| 5 | 005 | birch stakes | 90 | 8 | yard | yes |
| 6 | 006 | beech pegs | 0 | 0 | dry store | n/a |
| 7 | 007 | oak bridge | 200 | 26 | shed | yes |
| 8 | 008 | mixed fence | 110 | 10 | yard | yes |

Eight runs and the sixth row is deliberate: pegs are never treated, because a
peg should be dry and clean and replaceable, and a treated peg near food or
hands is worse than a worn one. Every other row carries a fire check because
the retort is a hot vessel in a wooden shelter, and the fire rules in
expansion 47 are read out before it lights.

---

## 25. APPENDIX L — REPAIR TABLE

| # | Member | Load | Cause | Method | Days | Inspection |
|---|---|---|---|---|---|---|
| 1 | Dormitory doorframe | medium | green wood | reframe | 11 | yes |
| 2 | Stair tread 4 | high | wet rot | replace | 1 | yes |
| 3 | Mess sill | medium | rot | splice | 2 | yes |
| 4 | Loft joist 7 | high | dry rot | sister | 2 | yes |
| 5 | Bridge stringer | high | wear | plate | 3 | yes |
| 6 | Mine set C1 | high | pressure | replace | 4 | yes |
| 7 | Pattern shelf | low | damp | rebuild | 1 | yes |
| 8 | Gate post | medium | rot | replace | 1 | yes |
| 9 | Roof truss 3 | high | split | brace | 5 | yes |
| 10 | Window lintel | medium | sag | prop, replace | 3 | yes |
| 11 | Yard lean-to rafter | low | actual break | replace | 1 | yes |
| 12 | Kitchen step | low | wear | replace | 1 | yes |
| 13 | Stair stringer | high | crack | reinforce | 4 | yes |
| 14 | Bell frame beam (57) | medium | flex | brace | 3 | yes |

Fourteen repairs and the first row is the storm's: eleven days for a doorframe
because the frame is load-bearing and the repair is drawn as it is done, and
the drawing becomes the pattern for every door in the shelter. The fourteenth
row runs to the time office, because a bell frame that flexes will not ring
the hour true, and the joining trades have always known that an instrument is
only as steady as the thing it hangs from.

---

## 26. APPENDIX M — PLAN TABLE

| # | Plan | Frame | Joiner | Pattern | Taught |
|---|---|---|---|---|---|
| 1 | J-01 | dormitory door | Tib | yes | Verne |
| 2 | J-02 | stair flight | Tib | yes | Verne |
| 3 | J-03 | roof truss A | Noll | yes | crew |
| 4 | J-04 | roof truss B | Noll | yes | crew |
| 5 | J-05 | mine set standard | Tild | yes | crew |
| 6 | J-06 | bridge splice | Tib | yes | crew |
| 7 | J-07 | pattern shelf | Verne | no | apprentice |
| 8 | J-08 | window frame | Verne | yes | apprentice |
| 9 | J-09 | gate frame | Noll | yes | crew |
| 10 | J-10 | bench build | Tib | yes | Verne |
| 11 | J-11 | drying rack | Sol | yes | crew |
| 12 | J-12 | teaching mortise | Tib | yes | children |

Twelve plans and the twelfth is the one the shelter keeps the longest: a
half-size mortise and tenon cut into a teaching block so that children can
feel how the building is held together. The drawn set's last page is
deliberately the smallest joke in the expansion, and it is also its argument —
a shelter that teaches its joints by hand outlives a shelter that keeps them
in a book.

---

## 27. APPENDIX N — WORKED YEAR

**Spring.** Lorn marks forty trees with the woodlot plan: oak for sills, ash
for handles, larch for the bridge, alder for the charcoal clamp. Ulla's crew
fells and hauls, and the pit saw runs every dry day while the new blade is
being set. Tib draws nothing yet, because the sawdust is in everything.

**Early summer.** The stacks go up under a rebuilt lean-to and Sol turns them
on a thirty-day rota, and the moisture board fills in with numbers that are
almost all embarrassingly high, because the shelter has been building with
green wood for years and now has the numbers to prove it.

**Late summer.** Tild audits all ten shoring sets and the old stope reads four
millimetres, which is beyond the report threshold she wrote two years ago and
which nobody has ever read. The audit is posted, read, and scheduled; the
stope set is renewed in autumn with seasoned oak instead of green pine.

**Autumn.** Della's first quarterly walk finds the dormitory's east wall at
twenty-four percent moisture with a fruiting body the size of a plate, and the
repair crew opens the plaster, cuts out three square metres, and rebuilds the
wall with an air gap and a drawing. The creosote retort runs its first batch
under fire rules and the shed gets a sill. The bridge stringer dries out and
goes on the watch list, which is the year's happiest entry.

**Winter.** The February storm takes the dormitory doorframe out of the wall,
and the repair takes eleven days, one drawing, one hundred and forty pegs, and
every warm hour the bench has. The shelter discovers that the doors that
stopped closing were warning it for two years, and that the warning was
written in the frames and not in anyone's notebook.

**Late winter.** The drawn set is bound and filed, the apprentice cuts the
teaching mortise, and the shelter's rule changes: no timber goes into a wall
wet, and every shoring audit is read aloud in the same meeting as the repair
list. The joinery's first full year ends with nothing dramatic except a new
habit, which is exactly how a trade like this survives.

---

## 28. APPENDIX O — VIGNETTES (TONE SAMPLE)

> Ulla sets the tooth with three passes of the file and tests the set against
> her thumbnail, and the pit saw goes back into the log and cuts straight for
> the first time in a month, and nobody makes a speech about it.

> Della lifts the plaster with a knife and the wall gives up a plate of
> orange fruiting bodies and a smell like wet cellar, and she marks the area
> on the wall plan with a pencil and keeps walking, because a survey is a
> walk and not a discovery.

> Tib draws the doorframe after it is fixed rather than before, because that
> is the only way he knows how, and the drawing becomes the pattern for every
> door in the building, and the pattern becomes the reason the next storm
> costs two days instead of eleven.

> Verne cuts the teaching mortise for the fourth time and finally gets the
> corners clean, and Tib says nothing but puts the block on the shelf with
> the plans, and that is how the shelter acquires its thirteenth plan.

---

## 29. APPENDIX P — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Green wood used | doors swell | dry, refit, mark |
| Dull blade | burnt planks | set, resharpen |
| Stack wet | rot arrives | rebuild cover |
| Joint loose | racking | re-peg, test |
| Set deflects | roof risk | renew set |
| Rot hidden | wall fails | survey route |
| Retort unsafe | fire risk | rules, extinguishers |
| Repair delayed | member fails | load priority |
| Plan missing | guesswork | draw it now |
| Audit unread | blindness | read it in meeting |

Every recovery is a habit the joinery already teaches, and the last row is
the expansion's sharpest lesson: Tild's accurate audit failed for two years
not because the measurement was wrong but because nobody read it. The shelter
learns to read the board before the board becomes a collapse.

---

## 31. APPENDIX Q — CONTENT REVIEW CHECKLIST

- [ ] No real forests, timber firms, or species claims are copied.
- [ ] Recipes and jobs stay with `CraftingSystem`.
- [ ] Living trees stay with expansion 32; only cut timber is owned here.
- [ ] Tunnels and collapses stay with expansion 18.
- [ ] Fire rules for the retort come from expansion 47.
- [ ] Preservative feedstocks come from expansion 39.
- [ ] Decor placement stays with `ShelterDecorSystem`.
- [ ] No magical rot cure; moisture and airflow are the treatment.
- [ ] Save additions are additive inside `crafting`.
- [ ] Determinism uses authored curves and live weather only.

---

## 32. APPENDIX R — GLOSSARY

- **Green timber** — wood above thirty percent moisture; honest, not structural.
- **Seasoning** — drying under cover with turning and measurement.
- **Kerf** — the width a saw cuts; wider kerf, more wasted board.
- **Set** — the outward bend of saw teeth that keeps a blade from binding.
- **Mortise and tenon** — a hole and a tongue, held by a peg.
- **Square set** — a mine frame of posts, caps, and braces.
- **Deflection** — how far a set has moved under load, in millimetres.
- **Dry rot** — wood decay that runs through damp enclosed timber.
- **Retention** — preservative held per cubic metre after treatment.
- **Drawn set** — the shelter's collected plans and patterns.

---

## 33. APPENDIX S — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `TimberYardSystem` | weather | species, stacks | forests |
| `SawpitSystem` | blocks | planks, waste | forests |
| `JoinerySystem` | stock, plans | joints, fittings | decor |
| `ShoringSystem` | mine state | sets, audits | tunnels |
| `DryRotSystem` | weather | sites, treatment | walls |
| `PreservationSystem` | chemistry | runs, storage | fire rules |
| `FrameRepairSystem` | survey | jobs, inspections | structures |
| `JoineryPlanSystem` | history | plans | nothing |
| `CraftingSystem` | nothing | recipes | wood |
| `SubterraneanSystem` | nothing | nothing | nothing |
| `ShelterDecorSystem` | nothing | nothing | nothing |
| `ShelterWorkshopSystem` | jobs | condition | wood |
| `EquipmentConditionSystem` | wear | condition | wood |
| `FireSystems` | nothing | rules | wood |
| `WildSystems` | nothing | nothing | nothing |
| `StandingRecord` | records | records | nothing |
| `NeedsSystem` | nothing | nothing | nothing |

---

## 34. APPENDIX T — DATA SCHEMA DETAIL (NEW CATALOGS)

**`timber_species.json`** — `species_id`, `display_name`, `shrinkage_class`,
`season_days`, `structural`, `uses[]`, `tags[]`.

**`timber_grades.json`** — `grade_id`, `moisture_range`, `use`, `visual_rule`,
`tags[]`.

**`seasoning_stacks.json`** — `stack_id`, `species_id`, `start_day`,
`moisture_pct`, `cover`, `turn_days`, `grade_id`, `tags[]`.

**`sawpit_blades.json`** — `blade_id`, `tooth`, `set_mm`, `kerf_mm`, `use`,
`condition`, `tags[]`.

**`joint_types.json`** — `joint_id`, `geometry`, `load_class`, `material_rule`,
`peg_species`, `failure_note`, `tags[]`.

**`shoring_sets.json`** — `set_id`, `location_id`, `framing_system`,
`pressure_mpa`, `deflection_mm`, `audit_days`, `tags[]`.

**`dry_rot_sites.json`** — `site_id`, `wall`, `moisture_pct`, `species`,
`area_sq_m`, `severity`, `treatment`, `review_day`, `tags[]`.

**`preservation_runs.json`** — `run_id`, `species_id`, `retention_kg_m3`,
`penetration_mm`, `storage`, `fire_checked`, `tags[]`.

**`frame_repairs.json`** — `repair_id`, `member`, `load_class`, `cause`,
`method`, `days`, `inspection`, `lesson`, `tags[]`.

**`joinery_plans.json`** — `plan_id`, `frame`, `joiner_id`, `pattern`,
`taught_to`, `revision`, `tags[]`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on
missing or duplicate IDs, invalid species, item, room, or location references,
or out-of-range numbers.

---

## 35. APPENDIX U — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Green stock held | patience debt | Yard |
| Mean moisture | readiness | Stacks |
| Blade condition | saw throughput | Sawpit |
| Joints tested | craft | Bench |
| Sets audited | safety | Shoring |
| Deflection trend | risk | Shoring |
| Rot area found | diligence | Survey |
| Retort penetration | treatment | Runs |
| Open repairs | backlog | Jobs |
| Plans drawn | knowledge | Plans |

Telemetry is diagnostic only; it never gates content, never ranks a joiner,
and never turns a deflection report into a score.

---

## 36. APPENDIX V — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive inside `crafting`.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling.
- [ ] Phase 6 content passes the review checklist in §31.
- [ ] Phase 7 soak shows a year seasoned, a wall surveyed and saved, a set
  renewed before it failed.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No collapse spectacle, rot horror, or secret-knowledge content exists.

---

## 37. APPENDIX W — OPEN QUESTIONS FOR REVIEW

1. Does the shelter fell its own trees, or is all standing timber purchased?
2. How much of the year's work can the yard hold before it needs a second
   loft?
3. Are rot surveys public walls or private reports to the build owner?
4. Does treated timber enter sleeping quarters at all?
5. Who reads the shoring board if the auditor is ill?
6. Can a repair be refused for load reasons, and who decides?
7. Do plans travel to outposts, or stay in the shelter's pattern store?
8. What happens to the drawn set when the joiner dies mid-job?

None of these may be decided unilaterally; each changes tone and balance.

---

## 38. APPENDIX X — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 14 Above the Ash | Framed parts for surface works |
| 1 | 15 The Deep Root | Orchard stakes, trellis, sheds |
| 2 | 18 The Underneath | Shoring sets and audits |
| 2 | 21 The Grid | Timber gasification feedstock |
| 3 | 25 The Iron Road | Sleepers, trestles, platforms |
| 4 | 31 The Kiln | Charcoal clamp, wooden tools |
| 5 | 32 The Wild | Standing timber and woodlot plans |
| 5 | 33 The Weather | Moisture, storms, drying |
| 5 | 34 The Long Road | Bridges, culverts, waystations |
| 6 | 39 The Reagent | Preservative feedstocks |
| 6 | 40 The Wheel | Wooden machine parts |
| 8 | 47 The Brigade | Retort and sawdust fire rules |
| 8 | 51 The Machine | Wooden jigs for repairs |
| 9 | 52 The Warm Ground | Timbers for the well pad |
| 9 | 55 The Quarter | Bunks, partitions, fixings |
| 10 | 57 The Hour | Clock cases and bell frames |

Each hook is additive. The Joinery can ship alone, and every other expansion
can ship without it.

---

## 39. APPENDIX Y — ENDING PROSE SKETCHES

**The Seasoned Yard.** The stacks are full, covered, turned, and marked, and
nobody builds wet again, and the yard's moisture board is read the way the
kitchen reads its stores.

**The Drawn Set.** The frames are drawn, taught, and repaired by whoever is on
the bench that winter, and the second storm costs two days instead of eleven.

**The Long Set.** Every shoring set is audited, every audit read, and the mine
has decades left in it, and the deflection board hangs in the meeting room.

**The Dry Wall.** Rot is routed along the survey walk, wall by wall, and the
dormitory survives its hundredth winter with a new sill and an air gap.

**The Standing Frame.** The shelter builds nothing it cannot repair, and says
so in one line at the top of every plan.

**Fade.** A peg bag on a bench, a moisture board at the end of the day, and a
wall whose newest plank is marked with the year it was cut.

---

## 40. APPENDIX Z — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Magic timber | false | moisture, grades |
| Instant seasoning | false | seasons |
| Rot horror | tone | survey tone |
| Collapse deaths | exploitation | audits first |
| Solo hero sawyer | fantasy | crews |
| Secret plans | gatekeeping | drawn set |
| Poison treatments | tone | plain rules |
| Masonry fork | duplication | wood only |
| Fire thrill | safety | read the rules |
| Infinite forest | no cost | supply plans |

The list exists because a woodworking system is easy to write as either a
fantasy of perfect craft or a disaster generator. The expansion's rule is
that a board is a season, a joint is a measurement, a set is an audit, and a
shelter survives because someone walks the walls with a knife four times a
year.

---

## 41. APPENDIX AA — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Species | 14 | 3,500 |
| Grades | 6 | 2,000 |
| Stacks | 10 | 2,500 |
| Blades | 8 | 2,000 |
| Joints | 12 | 3,000 |
| Shoring sets | 10 | 2,500 |
| Rot sites | 12 | 3,000 |
| Preservation runs | 8 | 2,000 |
| Repairs | 14 | 3,000 |
| Plans | 12 | 3,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 18 | 3,000 |
| Endings | 6 | 3,000 |
| **Total** | | **~58,000** |

---

## 42. APPENDIX AB — FIRST JOINERY YEAR

| Season | Focus | Milestone |
|---|---|---|
| Spring | plan | forty trees marked |
| Spring | saw | blade reset and resharpened |
| Summer | stacks | seasoning yard covered |
| Summer | survey | first rot walk |
| Autumn | retort | first creosote batch |
| Autumn | audit | sets audited and posted |
| Autumn | repair | east wall rebuilt |
| Winter | storm | doorframe lost |
| Winter | eleven days | frame refit and drawn |
| Winter | pegs | one hundred and forty set |
| Winter | set | stope set renewed |
| Winter | teaching | first apprentice mortise |

Twelve milestones in the order the shelter chose, and the shape is
characteristic of a material trade: planning in spring, patience in summer,
diligence in autumn, and the crisis in winter that turns the year's habits
into policy.

---

## 43. APPENDIX AC — CREW TABLE

| # | Crew | Work | Size | Season | Check |
|---|---|---|---|---|---|
| 1 | Felling crew | cuts and hauls | 4 | spring | stakes |
| 2 | Pit crew | saws and stacks | 4 | dry days | blade |
| 3 | Turning crew | turns stacks | 2 | monthly | moisture |
| 4 | Bench crew | joints and fittings | 3 | winter | test |
| 5 | Shoring crew | sets and audits | 3 | quarterly | board |
| 6 | Survey crew | rot walk | 2 | quarterly | map |
| 7 | Retort crew | treatment runs | 2 | autumn | fire check |
| 8 | Repair crew | structural jobs | 4 | as posted | inspection |
| 9 | Pattern crew | drawings and templates | 1 | winter | bound |
| 10 | Teaching crew | apprentices | 1 | winter | block |

Ten crews and the third is the one everyone forgets and the expansion
insists on: two people turning boards once a month is the difference between
a seasoning yard and a pile of wet lumber. The tenth row keeps the trade
alive past the current bench, which is the quiet purpose of the whole plan.

---

## 44. APPENDIX AD — TEACHING TABLE

| # | Lesson | Audience | Method | Record |
|---|---|---|---|---|
| 1 | Read a moisture board | stack crew | board | sheet |
| 2 | Feel a set blade | pit crew | thumbnail | none |
| 3 | Cut a mortise | apprentices | teaching block | plan |
| 4 | Fit a tenon | apprentices | bench | plan |
| 5 | Drive a peg | all | teaching block | none |
| 6 | Read a deflection | all | board | meeting |
| 7 | Walk a rot survey | survey crew | walls | map |
| 8 | Load a joint test | bench crew | rig | sheet |

Eight lessons and the fifth is the one that ends up in the children's
teaching block, because driving a peg is the first thing a shelter's youngest
residents can do that actually helps build the place they live in. The eighth
is the trade's honest limit: a joint test teaches you what will fail, and the
shelter writes the number down.

---

## 45. APPENDIX AE — JOINERY CHARTER

| Clause | Promise |
|---|---|
| Seasoned | No timber enters a wall wet |
| Graded | Every board is marked with its grade and year |
| Tested | Joints are loaded before they are trusted |
| Audited | Shoring sets are measured on schedule and read |
| Surveyed | The walls are walked four times a year |
| Treated | Preservatives run under fire rules and are marked |
| Repaired | Load comes before complaint in the repair queue |
| Drawn | What is built is drawn, and the drawing is the shelter's |
| Taught | The bench teaches the joints by hand |
| Kept | The frame outlives the people who built it |

The joinery charter is the expansion's first-class design object, kept in the
pattern store with the drawn set. Its last clause is the reason the trade
exists at all: every wooden building promises to outlast its builders, and a
joinery is the small institution that decides to keep that promise on purpose.

---

## 46. APPENDIX AF — BENCH SUCCESSION TABLE

| Role | First | Successor | Handover |
|---|---|---|---|
| Joiner | Tib | Verne | one drawn set |
| Sawyer | Ulla | pit hand | one blade set |
| Seasoning | Sol | turning crew | one full year |
| Shoring | Tild | mine crew | one audit cycle |
| Surveyor | Della | survey crew | one wall walk |
| Framer | Noll | bench hand | one roof |
| Woodlot | Lorn | wild liaison | one cutting plan |
| Apprentice | Verne | next recruit | one teaching block |

The succession table is measured in sets, walks, and roofs, and the joiner's
handover is one drawn set because a drawing can be handed to a person while a
memory cannot. The apprentice's last row is the smallest and the whole point:
the teaching block goes to whoever is next, and the block does not care whose
turn it is.

---

## 47. APPENDIX AG — REVIEW TABLE

| # | Question | Answer source | Action |
|---|---|---|---|
| 1 | How much green stock remains? | yard | stop building |
| 2 | Which blade wore fastest? | saw log | reorder |
| 3 | Which stack stayed wettest? | stacks | cover or move |
| 4 | Which joint failed a test? | bench | redo design |
| 5 | Which set moved most? | board | renew |
| 6 | Where did rot return? | survey | dry it again |
| 7 | Which run penetrated worst? | retort | replace batch |
| 8 | Which repair is open longest? | jobs | inspect |
| 9 | Which plan had to be redrawn? | store | bind it |
| 10 | What did the year teach? | crew | one lesson |

Ten questions asked once a year with the moisture board and the audit board
open, and the answers are supposed to be boring numbers that change one
habit each. The tenth is the only reflective one, and the joinery keeps it
because a trade that cannot say what it learned will relearn it the hard way.

---

## 48. APPENDIX AH — MOISTURE ARITHMETIC (WORKED)

A board dries by airflow and time, and the yard's model is deliberately
simple: moisture falls toward the seasoned floor at a rate set by the species
and the cover. For a stack of oak under a lean-to in summer,
`moisture(t) = 30% − (30% − 12%) · (1 − exp(−t / season_days))`, and the
practical reading is that oak takes a full 240-day season while pine reaches
air-dry in ninety. Cover matters because rain resets the exponential, which
is why the leaking lean-to in the story is worse than no cover: it keeps
undoing half the year's drying every time it rains.

The rot model is a threshold, not a clock. A site with moisture above
twenty-two percent and no airflow progresses; between eighteen and
twenty-two percent it is watched; below eighteen percent it is stable. That
is why Della's treatment column says dry and airflow first, and why the
clause that bans wet timber from walls is the entire expansion's cheapest
and most effective rule.

---

## 49. APPENDIX AI — TONE WATCHLIST

| Temptation | Why it fails | House rule |
|---|---|---|
| Master craftsman | fantasy | bench and crew |
| Perfect joints | false | test and correct |
| Rot as monster | genre | survey tone |
| Collapse as drama | exploitation | audits prevent |
| Hidden plans | gatekeeping | drawn set |
| Miracle preservative | false | retention numbers |
| Hero repair | spectacle | eleven quiet days |
| Infinite timber | no cost | supply plan |

The watchlist exists because woodwork invites romance. The expansion keeps
it physical: a blade that binds, a stack that got rained on, a peg that
sheared first exactly as designed, and a wall that dried out and went on the
watch list instead of the job list.

---

## 50. APPENDIX AJ — SMALL MISTAKES TABLE

| Mistake | Consequence | Lesson |
|---|---|---|
| Sawed without set | blade binds, burns | set the teeth |
| Stacked on bare ground | rot from below | raise on bearers |
| Skipped a turn | mould in the middle | calendar the rota |
| Pegged wet | loosens in summer | dry the peg |
| Forced a joint | cracked shoulder | remake it |
| Read deflection late | renew set urgently | read the board |

Six mistakes, each costing a board or a day, and the fifth is the one that
gives the story its chapter: a forced joint is a lie told to a building, and
the building tells it back in the storm.

---

## 51. APPENDIX AK — RETORT DRILL TABLE

| # | Step | Check | Who | Note |
|---|---|---|---|---|
| 1 | Clear yard | no sawdust within 3 m | crew | always |
| 2 | Water ready | two buckets | crew | always |
| 3 | Wind check | away from stacks | crew | daily |
| 4 | Fire watch | one person only watches | watch | no exceptions |
| 5 | Charge loaded | measured | crew | batch log |
| 6 | Vessel sealed | gasket checked | crew | leak = stop |
| 7 | Run watched | entire run | watch | no breaks |
| 8 | Cool before open | hand test | crew | never rush |
| 9 | Batch stored | marked shed | crew | food rule |
| 10 | Yard swept | complete | crew | fire rule |

Ten steps and the fourth is the one the fire owner insists on: during a retort
run, one person does nothing but watch. The tenth is the reason the shed has a
sweep rota, because a creosote retort in a shelter full of sawdust is a
manageable risk only as long as somebody is managing it.

---

## 52. APPENDIX AL — BRIDGE WATCH TABLE

| # | Member | Year | Moisture | Movement | Verdict |
|---|---|---|---|---|---|
| 1 | stringer east | 611 | 17% | none | watch |
| 2 | stringer west | 611 | 18% | none | watch |
| 3 | deck boards | 612 | 15% | one lift | refit |
| 4 | handrail | 612 | 14% | none | sound |
| 5 | abutment sill | 613 | 16% | none | sound |
| 6 | brace pair | 614 | 15% | none | sound |
| 7 | stringer east | 615 | 16% | none | sound |
| 8 | deck boards | 616 | 13% | none | sound |

The bridge watch table runs five years, and the story is the slowest and best
one in the expansion: two oak stringers that were eighteen percent moisture in
the first year and dry, stable, and unremarkable by the fifth. Nothing
happens to the bridge because the joinery owns it and reads its numbers, which
is what maintenance looks like when it works.

---

## 53. APPENDIX AM — BOARD MARK TABLE

| # | Mark | Meaning | Applied by | Kept |
|---|---|---|---|---|
| 1 | Year cut | season of felling | sawyer | paint |
| 2 | Species | kind of wood | yard | stamp |
| 3 | Grade | moisture class | seasoning | paint |
| 4 | Structural | load-rated | joiner | brand |
| 5 | Treated | preservative applied | retort crew | stripe |
| 6 | Food rule | never near food | retort crew | cross |
| 7 | Watch list | survey attention | surveyor | dot |
| 8 | Turned | season count | turn crew | tick |

The mark table is the expansion's insistence that a wooden shelter be
legible. Nobody should have to guess whether a plank is seasoned or whether
a beam has been treated, because a wrong guess is how a wall gets rebuilt
twice, and because a shelf that says year-cut and grade is a shelf that will
still be understood when nobody at the bench remembers cutting it.

---

## 54. APPENDIX AN — WEATHER RULE TABLE

| # | Weather | Yard | Stacks | Survey | Retort |
|---|---|---|---|---|---|
| 1 | clear | saw | turn | walk | run |
| 2 | rain | cover | covered | indoor only | run |
| 3 | storm | none | brace | indoor only | never |
| 4 | frost | saw | check cover | walk | run |
| 5 | thaw | none | turn, drain | walk wet walls | run |
| 6 | heat | saw early | shade, turn | walk early | run |
| 7 | fog | none | check mould | walk | run |
| 8 | wind | none | brace | walk | never |

The weather rule table is short because the trade's rules are short: turn
when it is dry, cover when it is wet, and never light the retort in a wind or
a storm. The fifth row is the one that saves the dormitory wall: a thaw is
when hidden rot shows itself, and the survey walk is scheduled around it.

---

## 55. CLOSING STATEMENT

ASHFALL's crafting layer already knows that a wheelbarrow has a wooden frame,
that a kiln screen is wooden, and that its mine tunnels need timber to stay
open. What the shelter has never had is the trade behind those wood parts:
the pit saw, the drying stack, the mortise, the peg, the shoring audit, the
rot survey, the creosote retort that four authored logs describe and no system
reads. The Joinery adds one material family properly — from the marked tree to
the drawn plan — and gives the shelter a way to keep the promise every wooden
building makes: that someone still knows how it was put together.

> Wave 10 note: this plan is one of five Wave 10 expansion bibles (57–61). Each
> is self-contained; none requires another to ship. The shared Wave 10 index
> lives at `docs/expansions/wave10/WAVE10_INDEX.md`. The safe pre-signature
> step is Phase 1 (data schemas and validators), which is additive and
> reversible. Evidence anchors: `Assets/Ashfall.Core/Narrative/TimberCarpentryCatalog.cs`
> (orphaned; `MortiseTenonFailureEntry`, `TimberCreosoteTreatmentEntry`,
> `TimberDryRotFruitingEntry`, `SquareSetShoringEntry`),
> `Assets/Ashfall.Core/Crafting/CraftingSystem.cs`,
> `Assets/Ashfall.Core/Crafting/ShelterWorkshopSystem.cs`,
> `Assets/Ashfall.Core/SubterraneanSystem.cs`,
> `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs`,
> `Assets/Ashfall.Core/EquipmentConditionSystem.cs`, and the narrative logs
> `mortise_tenon_failure_reports.json` (5,246 B),
> `timber_creosote_treatment_logs.json` (5,781 B),
> `timber_dry_rot_fruiting_records.json` (5,294 B),
> `square_set_shoring_audits.json` (5,950 B). Crafting catalogs already list
> wooden tool parts (Wheelbarrow, Screen, Pottery wheel, Mold) with no sourcing
> trade behind them.