# ASHFALL — Expansion 29 Design Bible
# THE GLASS
### Wave 4 · Glassworks, Optics, Lenses, Instruments, Glazing, and Vision

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-21
**Domain owners touched:** `Ashfall.Core.Shelter` (PrecisionOpticsEngine), `Ashfall.Core.Narrative` (OpticsGlassworksCatalog), `Ashfall.Core.World` (GeodeticSurveyEngine), `Ashfall.Core.Radiation`, `Ashfall.Core.Greenhouse`
**Proposed host owner:** `GlassworksHostSession` (extends the precision optics host)
**Existing save sections:** `precision_optics`
**Existing CLI verbs:** `--precision-optics-selftest`, `--data-integrity-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already owns precision optics. `PrecisionOpticsEngine` (14.9 KB) is the
live engine for grinding, fitting, and tuning optical elements, with a host
session (`PrecisionOpticsHostSession`), a save store (`PrecisionOpticsSaveStore`),
a panel, and a catalog loader. `precision_optics_catalog.json` (3.2 KB) feeds it
58 rows of precision optics data; `glassworks_recipes.json` (1.4 KB) is the
entire glassmaking recipe set. Narrative catalogs describe the trade in detail:
`OpticsGlassworksCatalog`, `pot_furnace_glass_melts.json` (6.1 KB),
`borosilicate_sight_glass_thermal_shock.json` (6 KB),
`optical_coating_rad_browning_reports.json` (5.4 KB), and
`ground_glass_joint_greasing_audits.json` (5.2 KB). Items include
`item_theodolite_brass_precision`, and systems as varied as
`GeodeticSurveyEngine`, `RadiationSystem`, `GreenhouseSystem`, `CvdDiamond`,
`PrecisionMetrologySystem`, and `PrecisionBroaching` all depend on glass and
optics in the fiction.

What does not exist: a glass furnace, batch and melt gameplay, blow and anneal
stages, eyewear and vision care, instrument assembly beyond precision optics,
window glazing as infrastructure, and mirrors as a light strategy.

**The Glass** turns the precision-optics engine into a full glass and vision
economy: sand to batch, batch to melt, melt to lens, lens to spectacles,
telescope, theodolite, and window — and the shelter learns that seeing clearly is
a survival capability.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

A shelter can lose more from bad eyesight than from bad weapons: the surveyor
misreads a theodolite, the watch misses a light on the ridge, the medic cannot
line up a syringe, and the child at the back of the class cannot see the board.

**The Glass** is the expansion about seeing: sand and cullet, furnaces and
annealing, lenses and frames, spectacles and telescopes, windows and daylight,
mirrors and signal. It extends the live precision-optics engine with real
glassmaking, adds vision care through the medical pipeline, wires instruments to
the survey and watch, and turns glazing into an infrastructure decision.

The expansion's hard rules follow the live owners: `PrecisionOpticsEngine`
remains the optical authority, `RadiationSystem` keeps its browning model,
`GreenhouseSystem` keeps its glazing contribution, `GeodeticSurveyEngine` keeps
survey, and all vision consequences route through the medical pipeline. No
second optics, radiation, greenhouse, or save system is created.

### 1.2 The five loops it adds

```
   Sand ──► Batch ──► Melt ──► Blow ──► Anneal ──► Glass
     │                                        │
     ▼                                        ▼
   Soda, lime,                            Lens blank ──► Grind ──► Lens
   cullet, salvage                                 │
                                                   ▼
                                     Spectacles ──► Vision care
                                                   │
                        Instruments ◄──────────────┤
                        (theodolite,              ▼
                         telescope,          Glazing ──► Light, heat,
                         microscope)         mirrors    signals, greenhouse
```

### 1.3 What the player manages

1. **Batch.** Sand, soda, lime, and cullet; purity and color.
2. **Furnace.** Heat, pots, melt time, and fuel; the furnace is a power and
   labor commitment.
3. **Forming.** Blowing, pressing, drawing, and casting; skill and failure.
4. **Annealing.** Controlled cooling; cracked glass is the tax on impatience.
5. **Optics.** Lens blanks, grinding, polishing, fitting, and prescription.
6. **Vision.** Eyesight, injuries, spectacles, and the real cost of not seeing.
7. **Instruments.** Theodolites, telescopes, microscopes, and labware.
8. **Glazing.** Windows, greenhouse panels, skylights, and daylight.
9. **Mirrors.** Light, signals, and thermal use.

### 1.4 What it is not

- Not a second precision-optics system. `PrecisionOpticsEngine` remains the
  authority for optical elements.
- Not a second radiation or greenhouse system. Their glass interactions stay in
  their owners.
- Not a magic-technology gate. Glass unlocks capability that already exists in
  fiction: seeing, measuring, surveying, and lighting.
- Not a second power generation system; furnaces consume the live power and fuel
  economies.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Shelter/PrecisionOpticsEngine.cs` | Grinding, fitting, tuning optics | `LIVE` |
| `src/Host/PrecisionOpticsHostSession.cs` | Host session | `LIVE` |
| `src/Host/PrecisionOpticsSaveStore.cs` | Save store | `LIVE` |
| `Assets/Ashfall.Core/Narrative/OpticsGlassworksCatalog.cs` | Glass and optics lore | `LIVE` |
| `Assets/Ashfall.Core/World/GeodeticSurveyEngine.cs` | Survey instruments | `LIVE` |
| `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` | Optical browning behavior | `LIVE` |
| `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs` | Glazing contribution | `LIVE` |
| `Assets/Ashfall.Core/Shelter/PrecisionMetrologySystem.cs` | Metrology | `LIVE` |
| `Assets/Ashfall.Core/Shelter/CvdDiamondSynthesisEngine.cs` | Adjacent precision materials | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `glassworks_recipes.json` | **1.4 KB** | very thin for a whole glassworks |
| `precision_optics_catalog.json` | 3.2 KB | 58 rows of optics data |
| `pot_furnace_glass_melts.json` | 6.1 KB | narrative |
| `borosilicate_sight_glass_thermal_shock.json` | 6 KB | narrative |
| `optical_coating_rad_browning_reports.json` | 5.4 KB | narrative |
| `ground_glass_joint_greasing_audits.json` | 5.2 KB | narrative |
| Batch/furnace/vision/instrument data | none | confirmed absent |

### 2.3 Confirmed gaps

- **GAP-29-1 — No glass furnace or melt gameplay.** Recipes exist; the industrial
  process does not.
- **GAP-29-2 — No anneal or failure model.** Glassmaking's defining risk is
  absent.
- **GAP-29-3 — No vision system.** Eyesight is not modeled; prescription does not
  exist.
- **GAP-29-4 — No instrument assembly.** Precision optics grinds lenses; nothing
  builds a telescope, microscope, or theodolite.
- **GAP-29-5 — No glazing content.** Windows and panels are fiction, not items.
- **GAP-29-6 — No mirror economy.** Light, signal, and thermal mirrors are absent.
- **GAP-29-7 — Furnace fuel and heat unmodeled.** No tie to the live power and
  fuel authorities beyond generic crafting.
- **GAP-29-8 — No eye injury or optical care through the medical pipeline.**
- **GAP-29-9 — No lens prescription or patient loop.**

### 2.4 Non-duplication statement

This expansion will **not** add a second optics, radiation, greenhouse, survey,
power, or medical system. It extends `PrecisionOpticsEngine` with glass batches,
products, and instrument assemblies; keeps radiation browning in
`RadiationSystem`; keeps greenhouse glazing in `GreenhouseSystem`; keeps survey
in `GeodeticSurveyEngine`; routes all vision consequences through the medical
pipeline and `NeedsSystem`; and adds state only as an additive sub-object of the
existing precision-optics save.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Seeing is capability.** A watch that cannot see, a surveyor who
misreads, a medic who misses: vision is a survival system.

**Pillar 2 — Glass is patient work.** Batch, melt, blow, anneal; every shortcut
cracks.

**Pillar 3 — One furnace unlocks many doors.** Spectacles, windows, instruments,
labware, and mirrors all come from the same pot.

**Pillar 4 — Light is morale.** Daylight through a pane, a lamp through a lens,
a mirror catching a sunset: glass changes how a shelter feels inside.

**Pillar 5 — Precision is a chain.** The theodolite needs the lens; the lens
needs the furnace; the furnace needs fuel; the chain is the game.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| First spectacles | Someone reads a label | Miracle |
| Furnace | Heat, danger, labor | Magic forge |
| Cracked pane | Work lost, patience | Punishment |
| Telescope | A first clear look | Awe overload |
| Eye injury | Care and caution | Gore |
| Glazing | Light entering a dark room | Luxury |

### 3.3 Content limits

- Eye injury and vision loss handled with restraint and utility.
- No real-world brands, military optics, or weapons optics.
- Furnace work is dangerous but never gore.
- Children's vision care is matter-of-fact, never sentimental.
- No glass-based superweapons or long-range targeting content.

---

## 4. THE GLASSWORKS WORLD

### 4.1 Interior rooms

- **`room_furnace_hall`** — the pot furnace, glory holes, and heat.
- **`room_batch_room`** — sand, soda, lime, cullet, and scales.
- **`room_annealing_room`** — slow cooling and stacked ware.
- **`room_lens_bench`** — grinding, polishing, and inspection.
- **`room_optical_fitting`** — frames, lenses, and prescription.
- **`room_instrument_shop`** — assembly and calibration.
- **`room_glazing_store`** — panes, putty, and frames.
- **`room_dark_room`** — optical testing and light work.

### 4.2 Exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_sand_quarry` | The Sand Quarry | 4 | Silica sand |
| `loc_soda_flats` | The Soda Flats | 5 | Soda ash source |
| `loc_cullet_field` | The Cullet Field | 4 | Salvage glass |
| `loc_old_lab` | The Old Lab | 6 | Labware, lenses, instruments |
| `loc_observatory_ruins` | The Observatory | 6 | Telescope parts and mirrors |
| `loc_window_works` | The Window Works | 5 | Pane salvage and frames |
| `loc_hothouse` | The Hot House | 4 | Furnace fuel and heat |
| `loc_survey_hill` | The Survey Hill | 3 | Instrument testing |
| `loc_signal_point` | The Signal Point | 4 | Mirror signaling |
| `loc_glass_beach` | The Glass Beach | 4 | Sea glass and drift cullet |

All locations require valid item references and scanner registration.

### 4.3 The glass day

Batch in the morning, melt through the day, form when the gather is ready,
anneal overnight, grind in the lens bench, and test in the evening light. The
furnace never really stops; the shop's rhythm is the furnace's rhythm.

---

## 5. MAIN STORYLINE — "WHAT THE GLASS SHOWED"

### 5.1 Central conflict

The shelter's surveyor, **Emeric**, cannot read his own instrument anymore. The
watch keeps reporting lights on the ridge that nobody else sees. **Sadie Lorne**,
who ground lenses for a living before the Exchange, has one pair of working
spectacles and eleven people who need them. **Corvin Ashe** the glassblower has a
furnace site, sand, and no soda. And **Noll** in the clinic has labware made from
salvaged jars because the shelter has no borosilicate.

When a theodolite reading misplaces a water source and a salvage team walks two
extra days for nothing, the shelter decides that glass is not decoration. It is
measurement, vision, and light — and it can all come from one furnace if the
shelter is willing to feed it.

The expansion's question: **how much of the shelter's survival depends on
looking clearly at the world?**

### 5.2 Theme (unspoken)

**Glass is how a shelter stops guessing.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_glassblower_corvin_ashe` | Corvin Ashe | Glassblower | Furnace, batch, and forming |
| `npc_optician_sadie_lorne` | Sadie Lorne | Optician | Lenses, fitting, and vision |
| `npc_surveyor_emeric` | Emeric | Surveyor | Theodolite, maps, and accuracy |
| `npc_labtech_noll` | Noll | Lab technician | Labware, testing, and medicine |
| `npc_watch_berrin` | Berrin | Watch lead | Telescopes and ridge lights |
| `npc_child_ines` | Ines | Child | The back of the class |
| `npc_trader_pike` | Pike | Salvage trader | Cullet, frames, and instruments |
| `npc_medic_orla` | Orla | Medic | Eye injuries and care |

### 5.4 Story beats (15)

1. **The Misread.** A theodolite error wastes a salvage run.
2. **The Back of the Class.** A child cannot see the board.
3. **The Last Spectacles.** One pair, eleven people.
4. **The Furnace Site.** Corvin picks the site and the risk.
5. **The Batch.** Sand, soda, lime, cullet; the first melt is wrong.
6. **The Crack.** The first anneal fails and the lesson is learned.
7. **The First Lens.** A blank is ground and polished.
8. **The Fitting.** The first prescription is issued.
9. **The Lab.** Borosilicate matters to the clinic.
10. **The Window.** A pane goes into the common room.
11. **The Telescope.** The watch can see the ridge.
12. **The Mirror.** Signal and light.
13. **The Survey.** The theodolite redraws the water map.
14. **The Eyes.** A round of vision care finds more than expected.
15. **What the Glass Showed.** Final disposition of the works.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Furnace scale | small / full / none | capability vs. cost |
| Gas vs. electric | fuel / power | infrastructure |
| Fittings | for need / for all / trade | fairness |
| Labware | priority / later / substitute | medicine vs. other |
| Glazing | shelter first / greenhouse first / both | light vs. food |
| Mirrors | signal / light / thermal | defense vs. comfort |
| Prescription policy | tested / self-reported / queue | rigor vs. speed |
| Final | works as institution / as workshop / as memory | identity |

### 5.6 Endings (5 + fade)

1. **The Clear Works** — spectacles, windows, instruments, and labware; the
   shelter sees and measures everything it owns.
2. **The Lens Bench** — optics become the shelter's export.
3. **The Lit Shelter** — glazing and mirrors change daily life.
4. **The Surveyed Valley** — the theodolite rewrites the region's maps.
5. **The Cracked Pot** — the furnace fails and the shelter returns to salvage.
6. **Fade** — the one pair of spectacles continues to circulate.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_glass_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_glass_misread`, `quest_glass_back_of_class`, `quest_glass_last_spectacles`,
`quest_glass_furnace_site`, `quest_glass_batch`, `quest_glass_crack`,
`quest_glass_first_lens`, `quest_glass_fitting`, `quest_glass_lab`,
`quest_glass_window`, `quest_glass_telescope`, `quest_glass_mirror`,
`quest_glass_survey`, `quest_glass_the_eyes`, `quest_glass_what_it_showed`.

### 6.2 Side quests (30)

**Batch and furnace (5)**
- `quest_glass_sand_run` — source clean sand
- `quest_glass_soda` — find or make soda ash
- `quest_glass_cullet` — gather and sort cullet
- `quest_glass_fuel` — feed the furnace
- `quest_glass_pot_care` — maintain the furnace pot

**Forming (5)**
- `quest_glass_first_blow` — first gather and blow
- `quest_glass_press` — press a simple shape
- `quest_glass_draw` — draw tubing
- `quest_glass_anneal` — learn controlled cooling
- `quest_glass_color` — color batch

**Optics (5)**
- `quest_glass_lens_blank` — cast a lens blank
- `quest_glass_grind` — grind and polish
- `quest_glass_prescription` — test and prescribe
- `quest_glass_frames` — make or salvage frames
- `quest_glass_lens_repair` — repair a cracked lens

**Instruments (5)**
- `quest_glass_theodolite` — build or rebuild the theodolite
- `quest_glass_telescope_build` — a telescope for the watch
- `quest_glass_microscope` — microscopy for the clinic
- `quest_glass_labware` — borosilicate glassware
- `quest_glass_calibrate` — calibrate instruments

**Glazing (5)**
- `quest_glass_window_common` — pane for the common room
- `quest_glass_greenhouse` — panels for the greenhouse
- `quest_glass_skylight` — daylight from above
- `quest_glass_putty` — make glazing putty
- `quest_glass_storm_patch` — repair storm damage

**Light and mirrors (5)**
- `quest_glass_mirror_make` — make a mirror
- `quest_glass_signal` — signal mirror protocol
- `quest_glass_lamp_lens` — a lens for lamps
- `quest_glass_sun_catch` — solar concentration
- `quest_glass_dark_room` — a dark room for testing

### 6.3 Repeatable quests (8)

`quest_glass_repeat_melt`, `quest_glass_repeat_grind`,
`quest_glass_repeat_anneal`, `quest_glass_repeat_fit`,
`quest_glass_repeat_glaze`, `quest_glass_repeat_calibrate`,
`quest_glass_repeat_salvage`, `quest_glass_repeat_signal`.

### 6.4 Dynamic hooks

Live events (survey readings, radiation browning, greenhouse glazing state,
watch reports, injuries, class attendance) attach authored follow-ups through
existing seams. No new event bus.

### 6.5 Constraints

- Optical elements are produced through `PrecisionOpticsEngine`.
- Radiation browning stays in `RadiationSystem`.
- Greenhouse glazing stays in `GreenhouseSystem`.
- Survey instruments stay in `GeodeticSurveyEngine`.
- Vision consequences route through the medical pipeline and `NeedsSystem`.
- Furnaces consume real fuel and power; no free heat.
- No glass item may be created without batch, melt, and anneal.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `GlassworksSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** batch recipes, melt state, forming stages, annealing, and product
output. **Consumes:** `Inventory`, `PowerGridSystem`, fuel, `CraftingSystem`,
`SkillProgressionSystem`. **Data:** `glass_batches.json`, `glass_products.json`,
extended `glassworks_recipes.json`. **Rules:** every product passes batch, melt,
form, and anneal; failure modes are authored and visible; quality is
deterministic from inputs and skill.

### 7.2 `VisionSystem` (new, `Ashfall.Core.Medical`)

**Owns:** eyesight, eye injuries, prescriptions, and spectacle fitting.
**Consumes:** `MedicalPipelineCoordinator`, `NeedsSystem`, `PrecisionOpticsEngine`,
`GlassworksSystem`. **Data:** `optical_prescriptions.json`,
`vision_conditions.json`. **Rules:** vision loss is a real capability change;
spectacles restore function; injuries are treated through the medical pipeline;
no hidden vision stat.

### 7.3 `InstrumentSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** instrument assembly, calibration, and instrument capability.
**Consumes:** `PrecisionOpticsEngine`, `GlassworksSystem`, `GeodeticSurveyEngine`
(survey results), `PrecisionMetrologySystem`. **Data:** `instrument_catalog.json`.
**Rules:** instruments are assembled from real parts; calibration drifts;
capability is expressed in the systems that own the outcome (survey, watch,
clinic), never in a duplicate stat.

### 7.4 `GlazingSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** windows, panels, skylights, putty, and daylight contribution.
**Consumes:** `ShelterAtmosphereSystem` (lighting quality),
`GreenhouseSystem` (panels), `Inventory`. **Data:** `glazing_upgrades.json`.
**Rules:** glazing changes shelter lighting and greenhouse output through their
live owners; storm damage is real; no direct mood writes that bypass
`ShelterAtmosphereSystem`.

### 7.5 `MirrorSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** mirrors for signal, light, and thermal concentration. **Consumes:**
`GlassworksSystem`, `SignalTriangulationSystem`/watch communications, lamp
systems. **Data:** `mirror_catalog.json`. **Rules:** mirrors are physical
objects with condition; signal effect routes through the live communication
path; no new signal system.

### 7.6 Systems explicitly not added

- No second precision-optics, radiation, greenhouse, survey, or power system.
- No glass superweapons or targeting optics.
- No new currency.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `glass_batches.json` (new)

```json
{
  "schema_version": 1,
  "batches": [
    {
      "batch_id": "batch_soda_lime",
      "display_name": "Soda-Lime Batch",
      "sand": 60,
      "soda": 18,
      "lime": 12,
      "cullet": 10,
      "melt_temp": "high",
      "hours": 8,
      "product_grade": "common",
      "tags": ["window", "container"]
    }
  ]
}
```

### 8.2 `glass_products.json` (new)

Products: bottles, jars, panes, tubing, lens blanks, rods, mirrors, labware,
with grade and use.

### 8.3 `optical_prescriptions.json` (new)

Prescription rows: strength bands, lens type, fitting minutes, and capability
effect.

### 8.4 `vision_conditions.json` (new)

Condition rows: cause, onset, severity bands, treatment, and spectacles
requirement.

### 8.5 `instrument_catalog.json` (new)

Instrument rows: parts, assembly hours, calibration, capability provided, and
consumer system.

### 8.6 `glazing_upgrades.json` (new)

Upgrade rows: pane count, putty, frame, hours, lighting contribution, storm
resistance, and greenhouse effect.

### 8.7 `mirror_catalog.json` (new)

Mirror rows: size, coating, quality, use, condition, and signal strength.

### 8.8 `glassworks_recipes.json` (extend)

Expand from a thin set to a full recipe table matching the new batch and product
schemas.

### 8.9 `precision_optics_catalog.json` (extend)

Extend with lens blanks, frames, and prescription elements compatible with the
live engine.

### 8.10 Items

New items appended to `items.json`: `item_silica_sand`, `item_soda_ash`,
`item_cullet`, `item_glass_pane`, `item_glass_bottle`, `item_lens_blank`,
`item_spectacles`, `item_telescope`, `item_theodolite_parts`,
`item_microscope`, `item_mirror`, `item_lab_beaker`, `item_glazing_putty`,
`item_blowpipe`, `item_annealing_rack`, `item_eye_chart`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`PrecisionOpticsSaveStore` remains the live save owner. New sub-objects (batches,
products, prescriptions, conditions, instruments, glazing, mirrors) are additive
inside it. No new save section.

### 9.2 State to persist

- Batch and melt state.
- Forming and anneal jobs.
- Lens and prescription records.
- Vision conditions and fittings.
- Instrument assembly and calibration.
- Glazing installations and damage.
- Mirror condition.

### 9.3 Determinism

- Melt, anneal, and grind outcomes are deterministic given inputs and skill.
- Failure states are authored probabilities applied through the live seeded path
  where randomness already exists; otherwise deterministic thresholds.
- Paired replay hashes must match.

### 9.4 Migration

Legacy saves load with existing precision optics state untouched; no batch,
vision, instrument, glazing, or mirror state exists until started. Existing
lenses and theodolite items keep working.

### 9.5 Checksum

Invariant-culture floats; integer permille for quality and calibration.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `GlassworksPanel` (new) | Batch, melt, form, anneal | `GlassworksHostSession` |
| `LensBenchPanel` (new) | Grind, polish, fit | same |
| `VisionPanel` (new) | Eyesight, prescriptions, spectacles | same |
| `InstrumentPanel` (new) | Assembly and calibration | same |
| `GlazingPanel` (new) | Windows, panels, daylight | same |
| `MirrorPanel` (new) | Signal, light, thermal | same |
| `PrecisionOpticsPanel` (extend) | Existing optics work | existing |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Vision is shown as capability and prescription, never a hidden stat.
- Furnace risk is stated before the melt starts.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Icon and color meaning always has a text label.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a furnace roar, a gather turning, a
glass crack, a grind wheel, a pane set into a frame, a mirror catching light. No
cue is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `PrecisionOpticsEngine` | Lens and element production |
| `RadiationSystem` | Browing behavior unchanged |
| `GreenhouseSystem` | Glazing contribution |
| `GeodeticSurveyEngine` | Instrument capability and survey |
| `PrecisionMetrologySystem` | Calibration reference |
| `MedicalPipelineCoordinator` | Eye injuries and conditions |
| `NeedsSystem` | Fatigue, morale, and capability effects |
| `ShelterAtmosphereSystem` | Lighting quality |
| `PowerGridSystem` | Furnace power |
| `CraftingSystem` | Batch and product recipes |
| `Inventory` | Glass goods |
| `TradingSystem` | Glass and instrument trade |
| `SignalTriangulationSystem` | Mirror signaling |
| `EpilogueChronicleBuilder` | Works milestones |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm `PrecisionOpticsEngine`, host, save store,
panel, catalog loader, `glassworks_recipes.json` size, and the narrative glass
catalogs. Record file:line; change nothing.

**Phase 1 — Data + validators.** Author batches, products, prescriptions,
conditions, instruments, glazing, mirrors; extend glassworks and optics catalogs;
append items. Register validators and scanner.

**Phase 2 — Pure Core.** `GlassworksSystem`, `VisionSystem`, `InstrumentSystem`,
`GlazingSystem`, `MirrorSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `GlassworksHostSession`, selftest coverage, fresh
journey.

**Phase 5 — UI.** New and extended panels with lifecycle and accessibility.

**Phase 6 — Content.** Locations, rooms, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 360-day soak including furnace failures, storm damage,
and vision care outcomes.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Batches | 15 |
| Glass products | 40 |
| Prescriptions | 10 |
| Vision conditions | 10 |
| Instruments | 15 |
| Glazing upgrades | 12 |
| Mirrors | 8 |
| Recipes | 50 |
| Items | 16 |
| Locations | 10 |
| Rooms | 8 |
| NPCs | 8 |
| Main quests | 15 |
| Side quests | 30 |
| Repeatable | 8 |
| Endings | 5 + fade |
| Prose estimate | 55,000–70,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Second optics system | Critical | Extend `PrecisionOpticsEngine` |
| Radiation/greenhouse duplication | Critical | Live owners keep their glass |
| Vision as hidden stat | High | Capability and prescriptions |
| Furnace trivialized | High | Fuel, time, failure |
| Glass superweapons | High | Explicit exclusion |
| Eye injury gore | High | Restraint review |
| Determinism break | Low | Live seeded paths |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `glass_batches.json` | 15 | 3,500 |
| `glass_products.json` | 40 | 8,000 |
| `optical_prescriptions.json` | 10 | 2,000 |
| `vision_conditions.json` | 10 | 2,500 |
| `instrument_catalog.json` | 15 | 4,000 |
| `glazing_upgrades.json` | 12 | 3,000 |
| `mirror_catalog.json` | 8 | 2,000 |
| `glassworks_recipes.json` | +50 | 10,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 16 | 2,500 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~66,000** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R29-1 | Second optics system | Low | Critical | One optics owner |
| R29-2 | Radiation/greenhouse drift | Low | Critical | Live owners |
| R29-3 | Hidden vision stat | Med | High | Prescriptions visible |
| R29-4 | Furnace too easy | Med | High | Fuel and failure |
| R29-5 | Superweapons | Low | High | Exclusion rule |
| R29-6 | Eye gore | Low | High | Restraint review |
| R29-7 | Determinism | Low | High | Seeded paths |
| R29-8 | Content overrun | Med | Med | Budget §13 |
| R29-9 | Instrument capability vague | Med | Med | Consumer-system results |
| R29-10 | Glazing effects duplicate | Med | Med | Route through owners |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Does the furnace require continuous power or fuel only?** Recommended: fuel
   with optional electric boost, routing through the live power economy.
2. **Can spectacles be traded?** Recommended: yes, with prescription mismatch
   consequences.
3. **Does the telescope change watch range in the live watch model?** Recommended:
   yes, through the consumer system's own capability input.
4. **Is borosilicate gated behind a skill or a recipe?** Recommended: skill.
5. **Do greenhouse panels belong to `GreenhouseSystem` or `GlazingSystem`?**
   Recommended: greenhouse owns the effect; glazing owns the object.

---

## 17. APPENDIX D — GLASS BATCH TABLE (15 BATCHES)

| # | Batch | Sand | Soda | Lime | Cullet | Grade | Use |
|---|---|---|---|---|---|---|---|
| 1 | Common Green | 60 | 18 | 12 | 10 | common | bottles, jars |
| 2 | Common Clear | 65 | 20 | 10 | 5 | common | windows |
| 3 | Window Batch | 62 | 18 | 12 | 8 | common | panes |
| 4 | Bottle Batch | 60 | 20 | 10 | 10 | common | storage |
| 5 | Container Hard | 58 | 18 | 14 | 10 | durable | jars, pressure |
| 6 | Tubing Batch | 64 | 18 | 10 | 8 | technical | labware |
| 7 | Borosilicate | 70 | 12 | 4 | 14 | technical | lab, optics |
| 8 | Optical Crown | 68 | 16 | 6 | 10 | optical | lenses |
| 9 | Optical Flint | 66 | 14 | 4 | 16 | optical | lenses |
| 10 | Tempered Batch | 62 | 18 | 12 | 8 | strong | doors, windows |
| 11 | Colored Amber | 58 | 18 | 12 | 12 | decorative | morale, trade |
| 12 | Colored Blue | 58 | 18 | 12 | 12 | decorative | morale, trade |
| 13 | Mirror Batch | 66 | 16 | 8 | 10 | optical | mirrors |
| 14 | Heat-Resistant | 64 | 16 | 8 | 12 | technical | furnace glass |
| 15 | Salvage Remelt | 20 | 10 | 6 | 64 | mixed | containers |

Batch purity decides the product line. A shelter that never sorts its cullet
makes bottles; one that learns to sort makes lenses.

---

## 18. APPENDIX E — GLASS PRODUCT TABLE (40 PRODUCTS)

| # | Product | Grade | Use | Hours |
|---|---|---|---|---|
| 1 | Bottle | common | storage | 1 |
| 2 | Jar | common | preservation | 1 |
| 3 | Wide Jar | common | preservation | 2 |
| 4 | Carboy | common | liquid | 3 |
| 5 | Window Pane | common | glazing | 2 |
| 6 | Small Pane | common | repair | 2 |
| 7 | Skylight Panel | common | light | 3 |
| 8 | Greenhouse Panel | common | growing | 3 |
| 9 | Door Glass | strong | safety | 3 |
| 10 | Lamp Glass | common | lighting | 1 |
| 11 | Lantern Globe | common | lighting | 2 |
| 12 | Tubing | technical | labware | 2 |
| 13 | Rod | technical | labware | 1 |
| 14 | Beaker | technical | clinic | 2 |
| 15 | Flask | technical | clinic | 2 |
| 16 | Vial | technical | medicine | 1 |
| 17 | Ampoule | technical | medicine | 2 |
| 18 | Funnel | technical | clinic | 1 |
| 19 | Retort | technical | chemistry | 3 |
| 20 | Condenser | technical | chemistry | 4 |
| 21 | Thermometer Case | technical | instruments | 2 |
| 22 | Lens Blank | optical | grinding | 2 |
| 23 | Small Lens | optical | spectacles | 1 |
| 24 | Spectacle Pair | optical | vision | 3 |
| 25 | Monocle | optical | vision | 2 |
| 26 | Magnifier | optical | bench work | 2 |
| 27 | Telescope Tube | optical | watch | 5 |
| 28 | Telescope Mirror | optical | astronomy | 6 |
| 29 | Theodolite Glass | optical | survey | 4 |
| 30 | Microscope Slide | technical | clinic | 1 |
| 31 | Microscope Lens | optical | clinic | 4 |
| 32 | Signal Mirror | optical | signaling | 3 |
| 33 | Wall Mirror | common | morale | 3 |
| 34 | Shaving Mirror | common | hygiene | 2 |
| 35 | Concentrator | optical | heat | 4 |
| 36 | Prism | optical | instruments | 3 |
| 37 | Sight Glass | technical | furnaces | 3 |
| 38 | Gauge Glass | technical | pressure | 3 |
| 39 | Bead Set | decorative | trade | 1 |
| 40 | Glass Tile | common | decoration | 2 |

Every product is a real object with a real recipe path. The list matters because
it is the shelter's menu of seeing, storing, treating, and measuring.

---

## 19. APPENDIX F — PRESCRIPTION TABLE (10 PRESCRIPTIONS)

| # | Prescription | Strength | Lens | Fitting | Effect |
|---|---|---|---|---|---|
| 1 | Reading Weak | 1 | crown | 1h | near text |
| 2 | Reading Strong | 2 | crown | 1h | small text |
| 3 | Distance Weak | 1 | crown | 1h | far detail |
| 4 | Distance Strong | 2 | flint | 2h | survey work |
| 5 | Work Close | 2 | crown | 2h | bench work |
| 6 | Watch Far | 3 | flint | 2h | ridge sighting |
| 7 | Astigmatic | 2 | ground | 3h | general |
| 8 | Injury Correct | 2 | special | 3h | vision restored |
| 9 | Child Reading | 1 | light | 1h | school |
| 10 | Aged General | 3 | bifocal | 4h | daily life |

Spectacles are fitted, not dispensed. A shelter that tests before fitting gets
useful glasses; one that hands out lenses gets headaches and a class that still
cannot see.

---

## 20. APPENDIX G — VISION CONDITION TABLE (10 CONDITIONS)

| # | Condition | Cause | Onset | Severity | Treatment |
|---|---|---|---|---|---|
| 1 | Strain | fine work | days | mild | rest, light |
| 2 | Age Nearsighted | age | months | progressive | spectacles |
| 3 | Age Farsighted | age | months | progressive | spectacles |
| 4 | Astigmatism | congenital | early | fixed | ground lenses |
| 5 | Dust Injury | environment | sudden | variable | irrigation, care |
| 6 | Chemical Burn | hazard | sudden | severe | urgent care |
| 7 | Radiation Eye | exposure | delayed | variable | shielding, care |
| 8 | Infection | hygiene | days | variable | medicine |
| 9 | Snow Blindness | glare | sudden | temporary | dark, rest |
| 10 | Traumatic Loss | injury | sudden | permanent | adaptation |

Vision conditions route through the medical pipeline like any other injury or
illness. Spectacles restore function; they never cure a condition. That
distinction is the expansion's honesty rule.

---

## 21. APPENDIX H — INSTRUMENT TABLE (15 INSTRUMENTS)

| # | Instrument | Parts | Assembly | Calibration | Consumer |
|---|---|---|---|---|---|
| 1 | Theodolite | glass, brass, base | 12h | 4h | survey |
| 2 | Level | glass, tube | 6h | 2h | construction |
| 3 | Telescope | tube, lens, mount | 10h | 3h | watch |
| 4 | Spotting Scope | tube, lens | 8h | 2h | watch |
| 5 | Microscope | lens, stage | 12h | 4h | clinic |
| 6 | Barometer | glass, metal | 8h | 3h | weather |
| 7 | Thermometer | glass, fluid | 4h | 2h | clinic, kitchen |
| 8 | Hygrometer | glass, hair | 5h | 2h | greenhouse |
| 9 | Spectroscope | prism, tube | 8h | 3h | chemistry |
| 10 | Magnifier Bench | lens, arm | 6h | 2h | lens bench |
| 11 | Survey Chain | metal, glass | 3h | 1h | survey |
| 12 | Sextant | mirror, frame | 10h | 4h | navigation |
| 13 | Signal Mirror Kit | mirror, case | 2h | 1h | signaling |
| 14 | Test Gauge | glass, metal | 6h | 3h | metrology |
| 15 | Sun Glass | lens, shade | 2h | 1h | observation |

Instruments are assembled from real parts and calibrated against the live
metrology reference. Their capability is expressed in the systems that consume
them, never in a floating bonus.

---

## 22. APPENDIX I — GLAZING UPGRADE TABLE (12 UPGRADES)

| # | Upgrade | Panes | Putty | Frame | Hours | Effect |
|---|---|---|---|---|---|---|
| 1 | Common Window | 1 | 1 | wood | 3 | lighting |
| 2 | Double Pane | 2 | 2 | wood | 5 | insulation |
| 3 | Skylight | 1 | 2 | wood | 6 | daylight |
| 4 | Greenhouse Panel | 1 | 1 | wood | 3 | growing |
| 5 | Greenhouse Row | 4 | 4 | wood | 10 | growing |
| 6 | Door Light | 1 | 1 | metal | 4 | safety |
| 7 | Corridor Light | 2 | 2 | wood | 6 | lighting |
| 8 | Clinic Window | 1 | 1 | wood | 4 | care morale |
| 9 | School Window | 2 | 2 | wood | 6 | education |
| 10 | Storm Shutter | 0 | 0 | wood | 3 | protection |
| 11 | Glass Door | 2 | 2 | metal | 8 | visibility |
| 12 | Observation Bay | 3 | 3 | metal | 10 | watch |

Glazing effects route through `ShelterAtmosphereSystem.LightingQuality` and
`GreenhouseSystem`; the expansion owns the object and the work, not the effect.

---

## 23. APPENDIX J — MIRROR TABLE (8 MIRRORS)

| # | Mirror | Size | Coating | Quality | Use |
|---|---|---|---|---|---|
| 1 | Pocket Signal | small | silver | good | signaling |
| 2 | Wall Mirror | med | tin | fair | morale, hygiene |
| 3 | Shaving Mirror | small | tin | fair | hygiene |
| 4 | Lamp Reflector | med | silver | good | lighting |
| 5 | Concentrator | large | silver | good | heat |
| 6 | Solar Cooker | large | silver | fair | cooking |
| 7 | Heliograph | med | silver | good | long signal |
| 8 | Observation Mirror | large | silver | good | watch |

Mirrors are the quiet multiplier: they extend light and signal without a wire,
and they fail visibly when the coating goes.

---

## 24. APPENDIX K — GLASS RECIPE TABLE (50 RECIPES — REPRESENTATIVE)

| # | Recipe | Inputs | Station | Hours | Output |
|---|---|---|---|---|---|
| 1 | Mix Common Batch | sand, soda, lime | batch room | 2 | batch |
| 2 | Mix Optical Batch | sand, soda, lime, cullet | batch room | 3 | optical batch |
| 3 | Melt Batch | batch, fuel | furnace | 8 | molten glass |
| 4 | Gather | molten glass | blowpipe | 1 | gather |
| 5 | Blow Bottle | gather | blowpipe | 2 | bottle |
| 6 | Blow Jar | gather | blowpipe | 2 | jar |
| 7 | Blow Globe | gather | blowpipe | 3 | globe |
| 8 | Press Pane | gather | press | 3 | pane |
| 9 | Draw Tubing | gather | draw bench | 4 | tubing |
| 10 | Draw Rod | gather | draw bench | 3 | rod |
| 11 | Cast Blank | molten glass | mold | 4 | lens blank |
| 12 | Cast Panel | molten glass | mold | 3 | panel |
| 13 | Anneal Ware | formed glass | annealing rack | 12 | annealed glass |
| 14 | Temper Pane | pane | temper oven | 4 | strong pane |
| 15 | Grind Blank | blank, grit | lens bench | 6 | ground lens |
| 16 | Polish Lens | ground lens, polish | lens bench | 4 | lens |
| 17 | Fit Spectacles | lens, frame | fitting bench | 2 | spectacles |
| 18 | Silver Mirror | glass, silver | coating bench | 5 | mirror |
| 19 | Tin Mirror | glass, tin | coating bench | 4 | mirror |
| 20 | Assemble Theodolite | glass, brass | instrument shop | 12 | theodolite |
| 21 | Assemble Telescope | tube, lens | instrument shop | 10 | telescope |
| 22 | Assemble Microscope | lens, stage | instrument shop | 12 | microscope |
| 23 | Calibrate Instrument | instrument | test bench | 4 | calibrated |
| 24 | Make Beaker | tubing | lamp bench | 2 | beaker |
| 25 | Make Flask | tubing | lamp bench | 2 | flask |
| 26 | Make Vial | tubing | lamp bench | 1 | vial |
| 27 | Make Ampoule | tubing | lamp bench | 2 | ampoule |
| 28 | Make Slide | pane | lamp bench | 1 | slide |
| 29 | Glaze Window | pane, putty | build site | 3 | window |
| 30 | Glaze Panel | panel, putty | greenhouse | 3 | panel |
| 31 | Fit Door Light | pane, frame | build site | 4 | door light |
| 32 | Make Putty | lime, oil | batch room | 3 | putty |
| 33 | Make Grit | sand, water | batch room | 2 | grit |
| 34 | Make Polish | ash, oil | batch room | 2 | polish |
| 35 | Sort Cullet | cullet | batch room | 2 | sorted cullet |
| 36 | Color Batch | batch, oxide | batch room | 2 | colored batch |
| 37 | Repane Window | pane, putty | build site | 2 | repaired |
| 38 | Repair Lens | lens, polish | lens bench | 3 | repaired lens |
| 39 | Recoat Mirror | mirror, silver | coating bench | 4 | recoat |
| 40 | Build Annealing Rack | wood, metal | shop | 4 | rack |
| 41 | Build Blowpipe | metal, wood | shop | 3 | blowpipe |
| 42 | Build Furnace Pot | clay, sand | kiln | 8 | pot |
| 43 | Rebuild Furnace | brick, pot | build site | 24 | furnace |
| 44 | Make Eye Chart | board, paint | clinic | 2 | chart |
| 45 | Fit Child Glasses | lens, frame | fitting | 2 | child specs |
| 46 | Make Prism | blank | lens bench | 4 | prism |
| 47 | Make Gauge Glass | tubing | lamp bench | 2 | gauge |
| 48 | Make Sight Glass | tubing | lamp bench | 2 | sight glass |
| 49 | Make Bead Set | gather, oxide | lamp bench | 3 | beads |
| 50 | Make Tile | molten glass | mold | 3 | tile |

All recipes route through live crafting and the furnace's own state. The furnace
pot (recipe 42) and furnace rebuild (recipe 43) are the long-cycle work that
makes glass rare.

---

## 25. APPENDIX L — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_glass_misread` | 4 | Wrong reading; wasted run |
| `quest_glass_back_of_class` | 4 | Child cannot see; test |
| `quest_glass_last_spectacles` | 5 | One pair, eleven people |
| `quest_glass_furnace_site` | 4 | Site, fuel, and risk |
| `quest_glass_batch` | 5 | Sand, soda, lime, first melt |
| `quest_glass_crack` | 4 | Failed anneal; learn cooling |
| `quest_glass_first_lens` | 5 | Blank, grind, polish |
| `quest_glass_fitting` | 4 | Test and issue |
| `quest_glass_lab` | 4 | Borosilicate for the clinic |
| `quest_glass_window` | 4 | Pane for the common room |
| `quest_glass_telescope` | 5 | Watch gains range |
| `quest_glass_mirror` | 4 | Signal and light |
| `quest_glass_survey` | 5 | Redraw the water map |
| `quest_glass_the_eyes` | 4 | Vision round; more than expected |
| `quest_glass_what_it_showed` | 3 | Final disposition |

---

## 26. APPENDIX M — NPC DOSSIERS (BRIEF)

**Corvin Ashe** — glassblower. Worked a furnace before the Exchange and treats
heat with respect. Measures his day by how much glass survived the night.

**Sadie Lorne** — optician. Ground lenses for a living; keeps the last working
pair of spectacles in a case and lends them by hand. Believes eyesight is
infrastructure.

**Emeric** — surveyor. Has drawn the region's water and routes from memory and
wants the theodolite working before he loses the rest of his sight.

**Noll** — lab technician. Runs the clinic's tests with salvaged jars and a
borosilicate wish list. Quietly furious about substitutes.

**Berrin** — watch lead. Reports lights nobody else can see until the telescope
proves her right.

**Ines** — child. Sits at the back, cannot see the board, and says nothing
because she assumes that is normal.

**Pike** — salvage trader. Sells cullet, frames, and instruments, and knows
exactly which shelter is desperate for glass.

**Orla** — medic. Treats eye injuries and dust burns and is the person who first
notices that more of the shelter needs glasses than admits it.

---

## 27. APPENDIX N — LOCATION DETAIL

- **The Sand Quarry** — sand with the wrong grit and the right grit, and the
  sorting that decides it.
- **The Soda Flats** — soda ash by the shore; wind, water, and haulage.
- **The Cullet Field** — salvage glass; color sorting is the whole skill.
- **The Old Lab** — labware, lenses, and instruments under broken benches.
- **The Observatory** — telescope parts, mirrors, and a stair that may not hold.
- **The Window Works** — panes, frames, and putty; a storm-damaged floor.
- **The Hot House** — fuel and heat; the furnace's supply line.
- **The Survey Hill** — instrument testing with a clear horizon.
- **The Signal Point** — mirror signaling with a sightline to the ridge.
- **The Glass Beach** — drift cullet polished by water; the easiest salvage.

---

## 28. APPENDIX O — FURNACE MODEL

| State | Heat | Fuel/hour | Output | Risk |
|---|---|---|---|---|
| Cold | none | 0 | none | none |
| Warm-up | rising | 2 | none | pot crack |
| Melting | high | 3 | molten | burn |
| Working | high | 2 | gather | burn |
| Holding | med | 1 | ready | waste |
| Cooling | falling | 0 | annealing | crack |

The furnace's schedule is the shelter's schedule. It cannot be switched off and
on without cost, and a cold furnace is a week of work before the first pane.

---

## 29. APPENDIX P — VISION MODEL

| Vision state | Reading | Distance | Work | Treatment |
|---|---|---|---|---|
| Clear | full | full | full | none |
| Strained | slow | full | reduced | rest, light |
| Corrected | full | full | full | spectacles |
| Impaired | reduced | reduced | reduced | care, lenses |
| Injured | none/partial | none/partial | none | medical care |
| Lost | none | none | adapted | training, tools |

Vision changes capability, not identity. A survivor who loses sight adapts to
roles and tools; the expansion never treats disability as deletion.

---

## 30. APPENDIX Q — WORKED 360-DAY GLASS SCENARIO

**Days 1–30.** Emeric's misread; Ines's test; site chosen. Sand and cullet
stocked; soda sourced through trade.

**Days 31–60.** First melt is cloudy; first anneal cracks. Corvin rebuilds the
pot and changes the cooling schedule.

**Days 61–90.** First pane; first lens blank; first pair of spectacles fitted to
Ines. Emeric reads his own notes.

**Days 91–150.** Borosilicate batch succeeds; clinic labware replaced. Window
installed in the common room; lighting quality rises.

**Days 151–220.** Telescope assembled; the watch confirms the ridge lights;
signal mirror protocol established.

**Days 221–300.** Second melt of optical glass; theodolite rebuilt and
calibrated; survey redraws the water map.

**Days 301–360.** Vision round finds fourteen people needing correction; nine
fitted. Glass works established as a permanent shelter trade.

---

## 31. APPENDIX R — VIGNETTE (TONE SAMPLE)

> Corvin opens the glory hole and the gather glows, and he turns the pipe and
the glass moves like something alive, and he does not hurry because hurrying is
how the pot cracks.

> Sadie holds the lens up to the window and looks through it, and then hands it
to Ines, who reads the letters on the board from the back of the room for the
first time, and nobody says anything because there is nothing to say.

> The telescope sits on the ridge in the evening, and Berrin finds the lights
again, and this time there are two of them, and she writes the bearing in the
log.

---

## 32. APPENDIX S — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Cloudy melt | product downgrade | re-sort, re-melt |
| Cracked anneal | ware lost | correct cooling |
| Pot failure | furnace down | rebuild, use reserve |
| Fuel shortage | furnace cold | fuel run, power boost |
| Lens grinding error | blank lost | regrind, recast |
| Frame shortage | no fitting | salvage, make |
| Storm damage | panes lost | repane, shutter |
| Mirror tarnish | signal weak | recoat |
| Eye injury | capability loss | medical care |
| Instrument drift | wrong readings | recalibrate |

No failure is a game over. Glass fails visibly and recovers slowly, which is
what makes the works feel real.

---

## 33. APPENDIX T — CONTENT REVIEW CHECKLIST

- [ ] `PrecisionOpticsEngine` remains the optical authority.
- [ ] Radiation browning stays in `RadiationSystem`.
- [ ] Greenhouse glazing effects stay in `GreenhouseSystem`.
- [ ] Survey capability stays in `GeodeticSurveyEngine`.
- [ ] Vision routes through the medical pipeline and `NeedsSystem`.
- [ ] No hidden vision stat exists.
- [ ] Furnace has real fuel, heat, and failure.
- [ ] No glass superweapon or targeting content exists.
- [ ] Eye injury prose is restrained.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses live seeded paths only.

---

## 34. APPENDIX U — GLOSSARY

- **Batch** — the sand, soda, lime, and cullet mix.
- **Cullet** — scrap glass used in a melt.
- **Gather** — molten glass on the pipe.
- **Anneal** — controlled cooling.
- **Blank** — a cast lens before grinding.
- **Crown / flint** — optical glass types.
- **Prescription** — a measured lens correction.
- **Calibration** — adjusting an instrument against a reference.
- **Glazing** — fitting panes to frames.
- **Heliograph** — mirror signal.

---

## 35. APPENDIX V — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `GlassworksSystem` | batch, fuel | melt, products | optics |
| `PrecisionOpticsEngine` | blanks | lenses, fittings | vision |
| `VisionSystem` | lenses | prescriptions | medical |
| `InstrumentSystem` | parts | assemblies | survey |
| `GlazingSystem` | panes | installations | atmosphere |
| `MirrorSystem` | mirrors | conditions | signals |
| `RadiationSystem` | glass | browning | optics |
| `GreenhouseSystem` | panels | growth | glazing |
| `GeodeticSurveyEngine` | instruments | survey | optics |
| `MedicalPipelineCoordinator` | injuries | treatment | vision |
| `ShelterAtmosphereSystem` | glazing | lighting | glazing |
| `PowerGridSystem` | furnace | power | glass |
| `CraftingSystem` | recipes | outputs | — |
| `Inventory` | goods | stock | — |
| `EpilogueChronicleBuilder` | milestones | chronicle | — |

---

## 36. APPENDIX W — DATA SCHEMA DETAIL (NEW CATALOGS)

**`glass_batches.json`** — `batch_id`, `display_name`, `sand`, `soda`, `lime`,
`cullet`, `melt_temp`, `hours`, `product_grade`, `tags`.

**`glass_products.json`** — `product_id`, `display_name`, `grade`, `use`,
`hours`, `quality_bands[]`, `tags`.

**`optical_prescriptions.json`** — `prescription_id`, `display_name`,
`strength`, `lens_type`, `fitting_minutes`, `effect`, `tags`.

**`vision_conditions.json`** — `condition_id`, `display_name`, `cause`, `onset`,
`severity_bands[]`, `treatment[]`, `spectacles_required`, `tags`.

**`instrument_catalog.json`** — `instrument_id`, `display_name`, `parts[]`,
`assembly_hours`, `calibration_hours`, `consumer_system`, `tags`.

**`glazing_upgrades.json`** — `upgrade_id`, `display_name`, `panes`, `putty`,
`frame`, `hours`, `lighting`, `storm_resistance`, `greenhouse_effect`, `tags`.

**`mirror_catalog.json`** — `mirror_id`, `display_name`, `size`, `coating`,
`quality`, `use`, `condition`, `signal_strength`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on missing
or duplicate IDs, invalid references, or out-of-range numbers.

---

## 37. APPENDIX X — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Melt success rate | furnace skill | GlassworksSystem |
| Anneal failure rate | process quality | GlassworksSystem |
| Lenses produced | optics output | PrecisionOpticsEngine |
| Spectacles fitted | vision care | VisionSystem |
| Uncorrected vision | capability gap | VisionSystem |
| Instruments working | capability | InstrumentSystem |
| Glazing coverage | light and growing | GlazingSystem |
| Storm pane loss | fragility | GlazingSystem |
| Mirror condition | signal readiness | MirrorSystem |
| Survey corrections | map accuracy | GeodeticSurveyEngine |

Telemetry is diagnostic only; it never gates content and never becomes a hidden
score.

---

## 38. APPENDIX Y — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] Vision consequences route only through the medical pipeline.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the review checklist in §33.
- [ ] Phase 7 soak shows furnace failure, storm damage, and vision outcomes.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No parallel optics, radiation, greenhouse, survey, or vision system exists.

---

## 39. APPENDIX Z — OPEN QUESTIONS FOR REVIEW

1. Does melted glass spoil if the furnace cools with the pot full?
2. Can spectacles be repaired, or only remade?
3. Should the telescope change watch detection in the live watch model, and at
   what range?
4. Should colored glass be morale content or purely decorative trade goods?
5. Does glazing count toward greenhouse output through the live greenhouse
   system, or only toward lighting?
6. Should instruments be lootable and repairable by raiders?
7. Should the furnace be a shelter room or an exterior works?
8. Should vision conditions ever be authored to a named survivor at fresh start?

None of these may be decided unilaterally; each changes balance and tone.

---

## 40. APPENDIX AA — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | Child vision care, school windows |
| 1 | 13 The Faithful | Stained glass, light in ritual |
| 1 | 14 Above the Ash | Optics for aircraft, sight glass |
| 1 | 15 The Deep Root | Greenhouse glazing, pollution sensors |
| 1 | 16 The Rebuilt Body | Optical prosthetics, sensor housings |
| 2 | 17 The Long Evening | Mirrors, lamps, performance light |
| 2 | 18 The Underneath | Lamp glass, subterranean lighting |
| 2 | 19 The Bitter Air | Mask lenses, decon sight glass |
| 2 | 20 The Quiet Hand | Signal mirrors, optical surveillance |
| 2 | 21 The Grid | Furnace power, lamp glass |
| 3 | 22 The Clean Flow | Lab glass, water testing |
| 3 | 23 The Alarm | Sight glass, fire-safe glazing |
| 3 | 24 The Long Goodbye | Reading glasses for the old |
| 3 | 25 The Iron Road | Signal glass, rail lamps |
| 3 | 26 The Common Table | Storage jars, lamp glass |
| 4 | 27 The Thread | Buttons, beads, loom parts |
| 4 | 28 The Lesson | Spectacles for pupils, charts |
| 4 | 30 The Press | Ink, glass rollers, lantern slides |
| 4 | 31 The Kiln | Furnace brick, pots, crucibles |

Each hook is additive. The Glass can ship alone, and every other expansion can
ship without it.

---

## 41. APPENDIX AB — CLOSING VIGNETTE

> At the end of the melt the furnace hums and the anneal rack fills with gray
> shapes that are not yet anything, and Corvin closes the door and goes to bed
> while the glass decides whether it will be a window.

> Ines reads from the back of the class, and Miren stops mid-sentence, and then
> continues, because stopping would be embarrassing and continuing is the
> kindness.

> Emeric sets the theodolite on the tripod and turns it until the crosshair
> lands on the survey mark, and the mark is where he thought it was, and for the
> first time in a year the map on the wall and the ground outside agree.

---

## 42. APPENDIX AC — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Glass as mana | wrong fiction | batch, fuel, craft |
| Vision as a meter | removes capability | prescriptions and outcomes |
| Furnace as a button | no craft | states, heat, failure |
| Lenses as loot only | no production | blanks, grinding, fitting |
| Instruments as flat bonuses | vague | consumer-system capability |
| Glazing as decoration | underuse | light, growing, morale |
| Mirrors as toys | underuse | signal, light, heat |
| Storm damage as punishment | unfair | warning, shutters, repair |
| Eye loss as deletion | dehumanizing | adaptation and tools |
| Optics as weapons | tone break | explicit exclusion |

The list exists because glass sits at the intersection of crafting, medicine,
architecture, and communication. Without the live owners it would sprawl; with
them it becomes the quiet capability the shelter builds once and uses forever.

---

## 43. APPENDIX AD — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Batches | 15 | 3,500 |
| Products | 40 | 8,000 |
| Prescriptions | 10 | 2,000 |
| Vision conditions | 10 | 2,500 |
| Instruments | 15 | 4,000 |
| Glazing upgrades | 12 | 3,000 |
| Mirrors | 8 | 2,000 |
| Recipes | 50 | 10,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 16 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~66,000** |

---

## 45. APPENDIX AE — MAIN QUESTLINE STAGE DETAIL (SIDE QUESTS)

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_glass_sand_run` | 3 | Locate, test, haul |
| `quest_glass_soda` | 4 | Find, burn, store |
| `quest_glass_cullet` | 3 | Gather, sort, weigh |
| `quest_glass_fuel` | 3 | Cut, haul, feed |
| `quest_glass_pot_care` | 3 | Inspect, patch, replace |
| `quest_glass_first_blow` | 4 | Gather, blow, anneal |
| `quest_glass_press` | 3 | Heat, press, cool |
| `quest_glass_draw` | 4 | Heat, draw, cut |
| `quest_glass_anneal` | 4 | Schedule, hold, inspect |
| `quest_glass_color` | 4 | Source, mix, melt |
| `quest_glass_lens_blank` | 4 | Cast, cool, inspect |
| `quest_glass_grind` | 5 | Rough, fine, polish |
| `quest_glass_prescription` | 4 | Chart, test, record |
| `quest_glass_frames` | 3 | Salvage, fit, adjust |
| `quest_glass_lens_repair` | 3 | Assess, polish, refit |
| `quest_glass_theodolite` | 5 | Parts, assemble, calibrate |
| `quest_glass_telescope_build` | 4 | Parts, assemble, test |
| `quest_glass_microscope` | 4 | Parts, assemble, test |
| `quest_glass_labware` | 4 | Batch, blow, anneal |
| `quest_glass_calibrate` | 3 | Reference, adjust, record |
| `quest_glass_window_common` | 3 | Make, fit, seal |
| `quest_glass_greenhouse` | 4 | Make, fit, verify |
| `quest_glass_skylight` | 4 | Measure, frame, fit |
| `quest_glass_putty` | 3 | Mix, test, store |
| `quest_glass_storm_patch` | 3 | Assess, cut, fit |
| `quest_glass_mirror_make` | 4 | Cast, coat, cure |
| `quest_glass_signal` | 4 | Protocol, train, test |
| `quest_glass_lamp_lens` | 3 | Grind, fit, light |
| `quest_glass_sun_catch` | 4 | Cast, coat, aim |
| `quest_glass_dark_room` | 3 | Build, darken, test |

---

## 46. APPENDIX AF — GLASS ECONOMY MODEL

| Flow | Input | Output | Loss |
|---|---|---|---|
| Batch | sand, soda, lime | batch | mixing |
| Melt | batch, fuel | molten | pot, dregs |
| Form | molten | ware | breakage |
| Anneal | ware, heat | finished | cracks |
| Grind | blank, grit | lens | errors |
| Fit | lens, frame | spectacles | mis-fit |
| Glaze | pane, putty | window | breakage |
| Coat | glass, silver | mirror | tarnish |
| Salvage | cullet | batch | sorting |

The economy is a funnel: every stage loses some of what the last stage made. Glass
is valuable precisely because the shelter cannot fake the middle steps.

---

## 47. APPENDIX AG — FUEL AND HEAT TABLE

| Fuel | Heat | Hours | Source | Cost |
|---|---|---|---|---|
| Coal | high | 8 | Coal Stage | trade |
| Charcoal | high | 7 | kiln | labor |
| Wood | med | 5 | forest | labor |
| Dried dung | low | 4 | herd | gather |
| Oil | high | 6 | trade | expensive |
| Electric boost | variable | 8 | grid | power |
| Waste heat | low | 12 | foundry | scheduling |

Fuel is the furnace's food, and it competes with the kitchen, the foundry, and
the shelter's heat. The expansion makes that competition explicit rather than
hidden in a crafting cost.

---

## 48. APPENDIX AH — LENS BENCH TABLE

| Stage | Grit | Hours | Removal | Check |
|---|---|---|---|---|
| Roughing | coarse | 3 | fast | shape |
| Fine grinding | medium | 2 | moderate | curve |
| Smoothing | fine | 2 | slow | surface |
| Polishing | polish | 2 | minimal | clarity |
| Edge fitting | tool | 1 | edge | fit |
| Inspection | lamp | 1 | none | power |

The lens bench is the shelter's patience made visible: six stages, each with a
way to fail, and a final inspection that decides whether someone will see.

---

## 49. APPENDIX AI — STORM AND REPAIR MODEL

| Weather | Pane risk | Effect | Mitigation |
|---|---|---|---|
| Calm | none | none | none |
| Wind | low | seal wear | shutters |
| Storm | medium | cracks | shutters, spares |
| Hail | high | breakage | shutters, thick glass |
| Frost | medium | frame damage | putty, fit |
| Ash fall | low | opacity | cleaning |
| Freeze-thaw | high | frame shift | re-putty |
| Dust storm | medium | abrasion | covers |

Glazing is infrastructure and behaves like it: it degrades, it fails in weather,
and it costs labor to keep. The expansion's rule is that the shelter never gets
light for free.

---

## 50. APPENDIX AJ — REGIONAL GLASS MAP

| Settlement | Strength | Need | Trade |
|---|---|---|---|
| The shelter | furnace, lenses | sand, soda | sells panes, spectacles |
| Market Town | trade glass | production | moves goods |
| Fog Ridge Camp | salvage lenses | frames | sells salvage |
| Spring Village | no glass | windows | buys panes |
| Foundry Enclave | metal frames | optics | trades hardware |
| Deep Bunker | instruments | repairs | lends tools |
| River Flotilla | drift cullet | furnace | sells cullet |
| Coal Stage | fuel | glassware | barters heat |

Glass travels farther than almost any other shelter product because everyone
needs to see, store, and measure.

---

## 51. APPENDIX AK — LORE: THE GLASS TRADITION

The fiction:

- **The Old Lab** was a regional testing station; its glassware was built to
  survive decades and mostly did.
- **The Observatory** tracked weather and sky; its mirror survived, its mount
  did not.
- **The Window Works** cut and fitted panes for the valley; the shelter inherited
  its frames.
- **The Glass Beach** is drift cullet from a pre-war works, rounded by water and
  sorted by color.
- **The Hot House** fed the furnace of the old works and feeds the new one.

No real company, observatory, or laboratory is copied. The tradition is generic
and local.

---

## 52. APPENDIX AL — VISION CARE WORKED SCENARIO

**Day 1.** Exam finds fourteen people who need correction and seven who did not
know they did.

**Day 4.** Two pairs are fitted from the first lens run; both are reading
strengths because the class needs them first.

**Day 12.** Emeric is fitted for a strong distance prescription; his reading of
the theodolite improves on the spot.

**Day 20.** A dust injury is treated; the worker is warned about glare and given
shaded lenses.

**Day 40.** Four more pairs are fitted; the queue drops to five. Two people
decline glasses because they are afraid of breaking them, which is a real
answer.

**Day 90.** The vision round is complete. The shelter's average work
productivity rises, and nobody calls it a buff; they call it seeing.

---

## 53. APPENDIX AM — ENDING PROSE SKETCHES

**The Clear Works.** The furnace runs, the lens bench is staffed, the windows
are glazed, and the shelter can see, measure, store, and treat what it has.

**The Lens Bench.** Spectacles leave the shelter in hand-sewn cases and come
back as grain, fuel, and news.

**The Lit Shelter.** Daylight moves through the corridors and the lamps carry
farther, and the dark months are less dark.

**The Surveyed Valley.** The theodolite redraws the water map, and the salvage
teams stop walking to empty places.

**The Cracked Pot.** The furnace fails in winter and the works goes cold, and
the last pair of spectacles is handed around like a sentence.

**Fade.** One pair of spectacles continues to circulate, and the shelter manages
the way it always has, one clear look at a time.

---

## 54. APPENDIX AN — IMPLEMENTATION NOTES

- Glass products should be items in `items.json` with grade metadata, not a
  separate product ledger.
- Furnace state should be a room-level object registered with the shelter, so
  save/load includes it naturally.
- Vision should be a survivor attribute consumed by the medical pipeline and the
  work model, not a parallel health track.
- Prescriptions should reference lens items and measured strengths, so fitting
  is a real assembly step.
- Instrument calibration should read the live metrology reference; no independent
  calibration clock.
- Glazing state should be walkable, damageable, and repairable through existing
  build and weather systems.
- The observatory and old lab should be authored with scavenging tables, not
  hand-placed loot, so they integrate with the live loot model.
- The furnace must never be placeable before the shelter can feed it, or the
  works becomes a dead room.

---

## 55. APPENDIX AO — SENSITIVE TOPICS TABLE

| Topic | Risk | Handling |
|---|---|---|
| Vision loss | Disability | Adaptation, tools, roles |
| Eye injury | Gore | Clinical, restrained |
| Child spectacles | Sentiment | Matter-of-fact |
| Workplace eye risk | Blame | Safety instruction, prevention |
| Radiation eye damage | Fear | Careful, factual |
| Supply denial | Inequality | Policy debate |
| Cosmetic vs. need | Frivolity | Needs first, clearly |
| Trade of spectacles | Dependency | Authored fairness |
| Mirror signaling | Surveillance fear | Distinct from spying |
| Blind survivor | Deletion fear | Fully playable adaptation |

The expansion's contract is that sight is capability, not humanity. A survivor
who loses vision keeps their place, their name, and their work.

---

## 56. APPENDIX AP — MEASUREMENT AND REVIEW CADENCE

| Gate | Question | Evidence |
|---|---|---|
| Premise | Do live owners still match? | file:line audit |
| Data | Are all rows valid and reachable? | integrity + scanner |
| Core | Are systems pure and deterministic? | unit tests |
| Persistence | Does legacy load neutral? | round-trip tests |
| Host | Is every command real? | selftest + journey |
| UI | Is vision honest? | lifecycle + a11y tests |
| Tone | Is impairment dignified? | content review |
| Balance | Is the furnace worth feeding? | 360-day soak |

---

## 57. APPENDIX AQ — GLASS STORAGE AND PRESERVATION MODEL

| Vessel | Capacity | Seal | Use | Durability |
|---|---|---|---|---|
| Bottle | 1 L | cork | liquid | 60 |
| Jar | 1 L | lid | preserved food | 70 |
| Wide Jar | 2 L | lid | bulk preserve | 70 |
| Carboy | 10 L | stopper | water, oil | 80 |
| Vial | 10 mL | stopper | medicine | 90 |
| Ampoule | 5 mL | sealed | sterile dose | 95 |
| Beaker | 500 mL | none | clinic | 85 |
| Flask | 250 mL | stopper | chemistry | 85 |

Glass is why the shelter can preserve, dose, and test. Every jar is a season of
food; every ampoule is a dose that stays clean. The expansion ties glass output
to the preservation and medical economies through real items and real seals,
never through abstract capacity.

---

## 58. APPENDIX AR — LIGHT AND SIGNAL MODEL

| Source | Strength | Range | Consumer |
|---|---|---|---|
| Window daylight | med | room | atmosphere |
| Skylight | high | hall | atmosphere |
| Lamp lens | high | corridor | lighting |
| Lantern globe | med | camp | night work |
| Signal mirror | high | ridge | watch |
| Heliograph | high | valley | long signal |
| Concentrator | high | bench | heat, cooking |
| Observation mirror | med | watch | sightline |

Light travels through glass objects and lands in the systems that already own
the outcome: atmosphere comfort, watch communication, and cooking heat. The
expansion contributes objects and limits, not new light physics.

---

## 59. APPENDIX AS — LENS AND GLASS QUALITY BAND TABLE

| Band | Clarity | Strength | Rejection | Use |
|---|---|---|---|---|
| Reject | cloudy | wrong | discarded | practice |
| Rough | hazy | approximate | high | coarse work |
| Common | clear | accurate | medium | windows, jars |
| Good | clear | precise | low | instruments |
| Fine | crisp | exact | very low | optics, lab |
| Optical | flawless | measured | almost none | precision |
| Damaged | cracked | n/a | repair | salvage |

Quality is deterministic from batch purity, curve, and grinding skill. The shelter
can use every band somewhere; the point is knowing which band it made.

---

## 60. APPENDIX AT — CLINIC GLASS INTERFACE TABLE

| Clinic need | Glass product | Source | Priority |
|---|---|---|---|
| Sample jars | Beaker, flask | glassworks | high |
| Medicine vials | Vial, ampoule | glassworks | high |
| Slides | Slide | glassworks | medium |
| Measuring | Graduated flask | glassworks | high |
| Sterilization | Heat-resistant flask | refractory + glass | high |
| Eye care | Lens, shades | optics | high |
| Chart | Eye chart | press | medium |
| Storage | Glass jar | glassworks | medium |
| Testing | Prism, instrument | optics | medium |
| Waste | Glass container | glassworks | low |

The clinic is the glassworks' most demanding customer: it needs clean, heat-safe,
measurable glass, and none of it can be improvised reliably from salvage.

---

## 61. APPENDIX AU — FIRST-YEAR WORK SCHEDULE

| Week | Focus | Milestone |
|---|---|---|
| 1–2 | Site and sand | batch room ready |
| 3–4 | First melt | glass exists |
| 5–6 | Anneal lessons | ware survives |
| 7–8 | First pane | glazing begins |
| 9–10 | Lens blank | optics begins |
| 11–12 | First fitting | vision restored |
| 13–16 | Borosilicate | labware replaced |
| 17–20 | Instrument parts | theodolite rebuilt |
| 21–24 | Telescope | watch extended |
| 25–28 | Mirror work | signal ready |
| 29–32 | Survey | map corrected |
| 33–36 | Vision round | many fitted |

The first year is deliberately paced around real products rather than a tech
tree: each fortnight the works makes one thing the shelter did not have before.

---

## 62. CLOSING STATEMENT

ASHFALL already grinds precision optics, measures with instruments, models
radiation browning on glass, and grows food behind glazing. What it lacks is the
works: sand to batch, batch to melt, melt to pane and lens, and the vision care
that lets a surveyor read his own instrument. The Glass adds that world without
adding a second optics, radiation, or greenhouse system. It adds a first pair of
spectacles, a window full of daylight, a telescope on the ridge, and the moment
the shelter stops guessing about what it can see.

> Wave 4 note: this plan is one of five Wave 4 expansion bibles (27–31). Each is
> self-contained; none requires another to ship. The shared Wave 4 index lives at
> `docs/expansions/wave4/WAVE4_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible. Evidence
> anchors: `PrecisionOpticsEngine`, `PrecisionOpticsHostSession`,
> `PrecisionOpticsSaveStore`, `glassworks_recipes.json` (1.4 KB),
> `precision_optics_catalog.json` (3.2 KB), `pot_furnace_glass_melts.json` (6.1 KB),
> `borosilicate_sight_glass_thermal_shock.json` (6 KB), and
> `optical_coating_rad_browning_reports.json` (5.4 KB).