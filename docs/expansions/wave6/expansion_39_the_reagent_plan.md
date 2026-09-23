# ASHFALL — Expansion 39 Design Bible
# THE REAGENT
### Wave 6 · Chemical Works, Acids, Chlorine, Plastics, Fermentation, and Lab Safety

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-23
**Domain owners touched:** `Ashfall.Core.Shelter` (ChlorAlkaliSynthesisEngine, PlasticPyrolysisSystem, FischerTropschSynthesisEngine, BioFermentationEngine), `Ashfall.Core` (PharmaLabSystem), MineralAcidSynthesisCatalog
**Proposed host owner:** `ReagentHostSession` (extends synthesis, lab, and storage surfaces)
**Existing save sections:** synthesis engine states, `PharmaLabState`, chemical inventories
**Existing CLI verbs:** `--chlor-alkali-selftest` (if present), `--data-integrity-selftest`, `--content-utilization-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already has chemical machinery. `ChlorAlkaliSynthesisEngine` defines
`ChlorAlkaliProcessDef` (`process_id`, `display_name`), `ChlorAlkaliFeedstockCost`
(`item_id`, `amount`), `ChlorAlkaliProcessStatus`, and `ChlorAlkaliHazardState`.
`PlasticPyrolysisSystem` defines `PyrolysisMachineDef` with
`construction_required_items`, `construction_labor_days`, `max_condition`,
`maintenance_interval_days`, and `maintenance_required_items`.
`FischerTropschSynthesisEngine` defines `FischerTropschBatchState`
(`batch_id`, `reactor_profile_id`, `progress_ticks`, `operator_skill`,
`feed_quality`, `process_variation`) and `SynthesisOutputBatch`.
`BioFermentationEngine` defines `BioFermentationPhase`,
`BioFermentationFeedstockCharge` (`item_id`, `units`), and
`BioFermentationState` with a `reactor_id` and an `Unbuilt` starting phase.
`PharmaLabSystem` defines `PharmaLabState` (`isProcessing`, `currentRecipeId`,
`assignedChemistId`, `progressHours`, `hoursRequired`, `currentPhase`,
`temperature`, `purity`, `contaminationRisk`, `reservedInputIds`) and
`PharmaPhase`.

The data is thin: `chlor_alkali_synthesis_catalog.json` is **1,617 bytes**,
`bio_fermentation_catalog.json` 3,332, `plastic_pyrolysis_catalog.json` 3,464,
`mineral_acid_synthesis_catalog.json` 4,677, and `chemical_syntheses.json`
7,001. There is no safety system, no catalyst system, no grade or purity
content, no storage segregation, no spill protocol, and no lab notebook.

**The Reagent** turns the back-room apparatus into a chemical works: feedstocks,
vessels, catalysts, grades, storage, safety, waste, and products the whole
shelter depends on. It extends the live synthesis engines and the pharma lab,
and it routes every hazard through the systems that already own burns, exposure,
and contamination.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| 19 The Bitter Air | Hazard agents, plumes, decontamination | Uses its decon and exposure routes |
| 22 The Clean Flow | Water, sanitation, waste water | Sends neutralized waste to its owners |
| 10 The Silent Foundry | Metal production and alloys | Supplies reagents, buys vessels |
| 29 The Glass | Glassware and optics | Orders lab glass, never makes it |
| 31 The Kiln | Ceramics, refractories, lime | Uses its vessels and linings |
| 35 The Habit | Dependency and narcotics handling | Produces precursors only through its rules |
| 21 The Grid | Power for reactors | Draws load through the grid owner |
| 38 The Ward | Medical care for burns and exposure | Routes injuries to it |
| 26 The Common Table | Food preservation chemistry | Supplies acids, salts, and cultures |

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The shelter has four machines it does not understand, a barrel of salvage, and
one person who has actually worked in a lab.

**The Reagent** is the expansion about industrial chemistry at the scale a
shelter can actually run: soap, bleach, acids, adhesives, plastics, solvents,
fuels, fertilizers, and the discipline that keeps them from killing the person
stirring the pot. It gives the works a room, a campaign, a grade standard, a
storage plan, a spill kit, and a notebook.

### 1.2 The five loops it adds

```
  Sort ──► Charge ──► React ──► Grade ─► Use / Store
    │        │          │         │          │
    ▼        ▼          ▼         ▼          ▼
  Salvage, Feedstock, Batch,   Purity,   Products,
  sorting  charges    run      labels    storage
                                            │
                                            ▼
                                     Waste ──► Neutralize ──► Dispose
```

### 1.3 What the player manages

1. **Feedstock.** Sorting salvage into usable chemical inputs.
2. **Vessels.** Reactors, stills, vats, and their condition.
3. **Catalysts.** Beds, regeneration, and poisoning.
4. **Batches.** Charge, run, monitor, and yield.
5. **Grades.** Purity, labeling, and fitness for use.
6. **Products.** Soap, bleach, acid, glue, plastic, solvent, fuel, culture.
7. **Storage.** Segregation, ventilation, and container discipline.
8. **Safety.** Goggles, aprons, wash, spill kits, and the rules.
9. **Waste.** Neutralization, disposal, and honest records.
10. **Notebook.** Recipes, observations, and what went wrong.

### 1.4 What it is not

- Not a weapons or poison program. Products are industrial and domestic; hazard
  agents stay with 19's owners and are never produced here.
- Not a party drug lab. Narcotic chemistry routes through the live pharma and
  dependency owners with their rules.
- Not a second chemistry engine. It extends the live synthesis engines.
- Not a dupe for soap, bleach, or acid from nothing; every output has a mass
  balance of real inputs.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Shelter/ChlorAlkaliSynthesisEngine.cs` | Chlorine and lye | `LIVE` |
| `Assets/Ashfall.Core/Shelter/PlasticPyrolysisSystem.cs` | Plastic recovery | `LIVE` |
| `Assets/Ashfall.Core/Shelter/FischerTropschSynthesisEngine.cs` | Fuel synthesis | `LIVE` |
| `Assets/Ashfall.Core/Shelter/BioFermentationEngine.cs` | Fermentation | `LIVE` |
| `Assets/Ashfall.Core/PharmaLabSystem.cs` | Lab recipes and purity | `LIVE` |
| `Assets/Ashfall.Core/Shelter/MineralAcidSynthesisCatalog.cs` | Acid content | `LIVE` |
| `Assets/Ashfall.Core/Shelter/SofcElectrochemistryEngine.cs` | Electrochemistry | `LIVE` |
| `Assets/Ashfall.Core/Shelter/CryogenicAirSeparationSystem.cs` | Gas separation | `LIVE` |
| `Assets/Ashfall.Core/Inventory/Inventory.cs` | Inputs and outputs | `LIVE` |
| `Assets/Ashfall.Core/Needs/NeedsSystem.cs` | Exposure consequences | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `chlor_alkali_synthesis_catalog.json` | **1,617 B** | thin |
| `bio_fermentation_catalog.json` | 3,332 B | thin |
| `plastic_pyrolysis_catalog.json` | 3,464 B | thin |
| `mineral_acid_synthesis_catalog.json` | 4,677 B | thin |
| `chemical_syntheses.json` | 7,001 B | thin |
| `pharma_recipes.json`, `narcotics.json` | small | owned elsewhere |
| Safety, storage, grades, catalysts, spill data | absent | confirmed none |

### 2.3 Confirmed gaps

- **GAP-39-1 — No feedstock sorting content.**
- **GAP-39-2 — No catalyst or regeneration content.**
- **GAP-39-3 — No reagent grade or purity standard.**
- **GAP-39-4 — No storage segregation or labeling.**
- **GAP-39-5 — No safety equipment or protocol content.**
- **GAP-39-6 — No spill or neutralization content.**
- **GAP-39-7 — No lab notebook or verification culture.**
- **GAP-39-8 — No product use chains beyond the engines' outputs.**
- **GAP-39-9 — No waste handling for chemical streams.**
- **GAP-39-10 — No chemical works room or staffing content.**

### 2.4 Non-duplication statement

This expansion will **not** add a second synthesis, lab, hazard, waste, or
medical system. It extends the live chlor-alkali, pyrolysis, Fischer-Tropsch,
fermentation, and pharma systems with content and campaign discipline, adds a
safety and waste layer that routes to existing owners, and stores its state in
additive sub-objects of the existing synthesis and lab stores. No new save
section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Chemistry is bookkeeping with fire.** Every input is counted and
every output is explained.

**Pillar 2 — Purity is trust.** A grade label is a promise the whole shelter
depends on.

**Pillar 3 — Safety is ordinary.** Goggles, wash, labels, and ventilation save
more people than any clever reaction.

**Pillar 4 — Waste is a result too.** If a process cannot explain its waste, it
is not done.

**Pillar 5 — The notebook is the lab's memory.** Observations beat recipes.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Reactions | Procedure and patience | Explosion spectacle |
| Burns | Fast care and review | Gore |
| Chlorine | Ventilation and rules | Gas warfare imagery |
| Acid | Labels and glassware | Splash horror |
| Fuel | Batch discipline | Fireworks |
| Waste | Neutralize and record | Dumping |
| Grades | Labels and trust | Alchemy |
| Notebook | Observation and honesty | Secret recipe mystique |

### 3.3 Content limits

- No weapons chemistry, no poison production, no explosives content.
- No production of the hazard agents owned by 19; the works never becomes a
  chemical weapons lab.
- No drug party content; narcotic production follows the live pharma rules.
- No graphic accident content; injuries are consequences with medical routing.
- No alchemical magic; every output is a function of real inputs, skill, and
  time.
- No child labor in the works; apprentices are 16+ and supervised, per the
  Wave 4 kiln contract.

---

## 4. THE REAGENT WORLD

### 4.1 Interior rooms

- **`room_works_main`** — vessels, benches, and the campaign board.
- **`room_works_still`** — the still and condenser.
- **`room_works_vats`** — fermentation vats and their smell.
- **`room_works_dry`** — a dry store for powders and salts.
- **`room_works_acids`** — the acid store: glass, sand, labels.
- **`room_works_fuel`** — the fuel room: steel, ground straps, ventilation.
- **`room_works_notebook`** — the desk with the lab ledger.
- **`room_works_scrub`** — wash station, eyewash, aprons.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_salvage_yard` | The Salvage Yard | 3 | Feedstock sorting |
| `loc_salt_pans` | The Salt Pans | 3 | Brine and salt |
| `loc_lime_quarry` | The Lime Quarry | 3 | Lime and alkali |
| `loc_fuel_bay` | The Fuel Bay | 4 | Fuel handling |
| `loc_waste_bed` | The Neutralization Bed | 3 | Waste treatment |
| `loc_vent_stack` | The Vent Stack | 3 | Safe exhaust |
| `loc_ash_pit` | The Ash Pit | 3 | Potash and ash |
| `loc_glass_delivery` | The Glass Delivery | 2 | Lab glassware |
| `loc_barrel_row` | The Barrel Row | 3 | Container store |
| `loc_works_muster` | The Works Muster | 2 | Shift briefings |

All locations require valid item references and scanner registration.

### 4.3 The campaign

One campaign at a time: a soap run, an acid run, a plastic run, a fuel run. Each
campaign has a charge, a run, a yield, a grade, and a waste record. The expansion's
clock is the batch.

---

## 5. MAIN STORYLINE — "SOMETHING THAT CLEANS"

### 5.1 Central conflict

The shelter is out of soap. **Dala Vint** knows how to make it and knows what
happens when the lye is wrong, the fire is too hot, or the storage is careless.
The works are a back room with four machines, no labels, and a bucket of acid
that everyone is afraid of. **Hesk** the safety officer wants rules before the
first accident, not after. **Osla** the soap maker wants the product to reach the
wash lines. **Gren** wants fuel for the generator and thinks the still is a
kind of cooking. **Mirren** hauls salt from the pans and brine from the deep well
and knows both are worth more than anyone says.

Then the acid bites someone, the chlorine alarm sounds, and the shelter learns
that a chemical works is a promise about containment. The expansion's question:
**can a shelter be trusted with strong things?**

### 5.2 Theme (unspoken)

**A society is measured by what it does with the dangerous things it needs.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_chemist_dala_vint` | Dala Vint | Chemist | Recipes, grades, notebook |
| `npc_lab_aide_pock` | Pock | Lab aide | Charges, cleaning, order |
| `npc_safety_officer_hesk` | Hesk | Safety officer | Labels, storage, spills |
| `npc_salt_hauler_mirren` | Mirren | Salt and brine hauler | Feedstock routes |
| `npc_soap_maker_osla` | Osla | Soap maker | Product use |
| `npc_stillman_gren` | Gren | Stillman | Fuel campaigns |
| `npc_dyer_tila` | Tila | Dyer | Dyes and mordants |
| `npc_apprentice_wex` | Wex | Apprentice | Learning, safety checks |

### 5.4 Story beats (15)

1. **The Empty Shelf.** Soap runs out and the wash line stops.
2. **The Recipe.** Dala writes the first campaign from memory.
3. **The Labels.** Hesk labels everything, including the bucket.
4. **The Charge.** Feedstock sorting becomes a job.
5. **The Run.** The first soap batch is made and judged.
6. **The Bite.** An acid splash sends someone to the ward.
7. **The Rules.** Storage segregation and eye protection become law.
8. **The Grade.** Purity standards are written and posted.
9. **The Chlorine.** A process alarm tests ventilation and evacuation.
10. **The Salt.** Mirren opens the brine route.
11. **The Fuel.** Gren runs a Fischer-Tropsch campaign under supervision.
12. **The Culture.** Fermentation vats produce acids and cultures.
13. **The Plastic.** Pyrolysis turns salvage into sheet and stock.
14. **The Waste.** The neutralization bed closes the loop.
15. **Something That Cleans.** The works becomes a department.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Scale | bench / works / two shifts | output vs. control |
| Grades | one grade / two / strict | simplicity vs. trust |
| Storage | mixed / segregated / vaulted | convenience vs. safety |
| Fuel | still / synthesis / both | supply vs. complexity |
| Waste | bed / burn / bury | closure vs. cost |
| Products | soap first / acid first / fuel first | daily life vs. industry |
| Apprentice | none / one / two supervised | growth vs. risk |
| Final | works as institution / practice / memory | identity |

### 5.6 Endings (5 + fade)

1. **The Clean Works** — labels, grades, storage, and spill kits make chemistry
   ordinary and safe.
2. **The Full Shelf** — soap, bleach, acid, glue, plastic, and fuel reach the
   whole shelter.
3. **The Careful Scale** — the works stays small and trustworthy, and nobody is
   ever hurt badly.
4. **The Hot Week** — a chlorine scare and a spill force a rebuild, and the
   works comes back stricter.
5. **The Empty Barrel** — a feedstock run fails, a campaign stops, and the
   shelter learns to plan around scarcity.
6. **Fade** — a bar of soap on a shelf with a label in careful handwriting.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_reagent_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_reagent_empty_shelf`, `quest_reagent_recipe`, `quest_reagent_labels`,
`quest_reagent_charge`, `quest_reagent_run`, `quest_reagent_bite`,
`quest_reagent_rules`, `quest_reagent_grade`, `quest_reagent_chlorine`,
`quest_reagent_salt`, `quest_reagent_fuel`, `quest_reagent_culture`,
`quest_reagent_plastic`, `quest_reagent_waste`, `quest_reagent_clean`.

### 6.2 Side quests (30)

**Feedstock (5)**
- `quest_reagent_sort` — sort salvage by class
- `quest_reagent_brine` — haul brine
- `quest_reagent_lime` — burn lime for alkali
- `quest_reagent_ash` — collect potash
- `quest_reagent_store` — inventory the dry store

**Acids (5)**
- `quest_reagent_acid_charge` — charge an acid run
- `quest_reagent_acid_glass` — glassware for acid
- `quest_reagent_acid_label` — label every container
- `quest_reagent_acid_spill` — practice the spill drill
- `quest_reagent_acid_use` — put the acid to work

**Chlorine (5)**
- `quest_reagent_chlor_charge` — chlor-alkali campaign
- `quest_reagent_chlor_vent` — test ventilation
- `quest_reagent_chlor_alarm` — drill the alarm
- `quest_reagent_chlor_bleach` — bleach for the wash line
- `quest_reagent_chlor_water` — chlorine in water treatment

**Plastics (5)**
- `quest_reagent_pyro_build` — build the pyrolysis machine
- `quest_reagent_pyro_feed` — feed sorted plastic
- `quest_reagent_pyro_maintain` — maintain the machine
- `quest_reagent_sheet` — press sheet stock
- `quest_reagent_forms` — make usable shapes

**Fermentation and fuel (5)**
- `quest_reagent_vat_build` — build a vat
- `quest_reagent_vat_culture` — start a culture
- `quest_reagent_vat_acid` — ferment to acid
- `quest_reagent_ft_build` — build the synthesis reactor
- `quest_reagent_ft_run` — run a fuel batch

**Safety (5)**
- `quest_reagent_goggles` — eye protection for all
- `quest_reagent_aprons` — aprons and boots
- `quest_reagent_eyewash` — install eyewash
- `quest_reagent_fire` — fire blanket and sand
- `quest_reagent_notebook` — keep the lab ledger

### 6.3 Repeatable quests (8)

`quest_reagent_repeat_soap`, `quest_reagent_repeat_bleach`,
`quest_reagent_repeat_charge`, `quest_reagent_repeat_run`,
`quest_reagent_repeat_grade`, `quest_reagent_repeat_waste`,
`quest_reagent_repeat_label`, `quest_reagent_repeat_restock`.

### 6.4 Dynamic hooks

Live events (salvage hauls, spill alarms, water quality, ward admissions, fuel
demand, embargo changes, power brownouts) attach authored follow-ups through
existing seams. No new event bus.

### 6.5 Constraints

- Synthesis stays with the live engines; this expansion adds campaigns and
  content, not reaction logic.
- Hazard agents stay with 19; narcotics with the live pharma and dependency
  owners.
- Burns and exposure route to `NeedsSystem`, medical, and decon owners.
- Waste routes to sanitation and water owners.
- No weapons, poisons, or explosives content.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `ChemicalWorksSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** the works rooms, vessels, campaigns, and campaign board.
**Consumes:** `ChlorAlkaliSynthesisEngine`, `PlasticPyrolysisSystem`,
`FischerTropschSynthesisEngine`, `BioFermentationEngine`, `PharmaLabSystem`,
`Inventory`, `PowerGridSystem`. **Data:** `reagent_works.json`,
`feedstock_sorts.json`. **Rules:** each campaign has a charge, a run, a yield,
and a waste record; the engines do the chemistry; the works does the schedule,
the room, and the counting.

### 7.2 `FeedstockSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** salvage sorting, feedstock grades, and charge readiness. **Consumes:**
`Inventory`, salvage tables, `CryogenicAirSeparationSystem` (gases).
**Data:** `feedstock_grades.json`. **Rules:** each charge declares its inputs
and their grades; contaminated feedstock is detected and diverted, never
silently consumed.

### 7.3 `CatalystSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** catalyst beds, regeneration, poisoning, and replacement.
**Consumes:** live engines' process definitions, `Inventory`.
**Data:** `catalysts.json`. **Rules:** a catalyst has a life, a condition, and a
failure mode; poisoned beds are regenerated or replaced with recorded cost.

### 7.4 `ReagentGradeSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** grade standards, purity checks, and labels. **Consumes:**
`PharmaLabState.purity`, engine outputs, `Inventory`.
**Data:** `reagent_grades.json`. **Rules:** outputs carry a grade; a grade
mismatch blocks uses that require higher purity; mislabeling is a reviewable
incident.

### 7.5 `ChemicalSafetySystem` (new, `Ashfall.Core.Shelter`)

**Owns:** safety equipment, storage segregation, ventilation rules, and drill
content. **Consumes:** `ShelterAtmosphereSystem` (air quality),
`VentilationSystem`, `NeedsSystem` (burns, exposure), medical pipeline,
`ShelterFireHazardSystem`. **Data:** `chemical_storage.json`,
`spill_protocols.json`. **Rules:** every store has a segregation plan; every
worker has protection; spills have procedures and drills; injuries route to the
ward with a cause attached.

### 7.6 `ChemicalWasteSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** neutralization, disposal routes, and waste records. **Consumes:**
`SanitationSystem`, `WaterTreatmentSystem` (Wave 3), `SumpFloodingSystem`.
**Data:** `waste_streams.json`. **Rules:** every stream is neutralized or
stored; nothing is dumped; the record is part of the campaign.

### 7.7 `LabNotebookSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** recipe observations, failure notes, and verification.
**Consumes:** `ArchiveDeskSystem` (Wave 4), `LibraryManualCatalogLoader`,
`DiagnosisKnowledgeStore` patterns. **Data:** `lab_notebook_fields.json`.
**Rules:** the notebook records what was done and what happened; recipes improve
only through recorded observation; the notebook is the works' institutional
memory.

### 7.8 Systems explicitly not added

- No second chemistry, hazard, waste, or medical system.
- No weapons, poisons, explosives, or CBRN production.
- No drug party or contraband loop.
- No new currency.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `reagent_works.json` (new)

```json
{
  "schema_version": 1,
  "campaigns": [
    {
      "campaign_id": "campaign_soap_first",
      "display_name": "First Soap Run",
      "engine": "chlor_alkali",
      "charge": [{"item_id": "item_fat_scrap", "amount": 6}],
      "run_hours": 8,
      "yield_item": "item_soap_bar",
      "yield_amount": 12,
      "grade": "household",
      "waste_stream": "alkaline_wash",
      "tags": ["works", "daily"]
    }
  ]
}
```

### 8.2 `feedstock_sorts.json` (new)

Sorts: class, source, grade, hazard, divert rule.

### 8.3 `feedstock_grades.json` (new)

Grades: name, purity band, allowed uses, price effect.

### 8.4 `catalysts.json` (new)

Catalysts: bed, life, condition, regeneration, poisoning, replacement.

### 8.5 `reagent_grades.json` (new)

Grade standards: name, check, label, blocked uses.

### 8.6 `chemical_storage.json` (new)

Storage: room, class, segregation, ventilation, container, label.

### 8.7 `spill_protocols.json` (new)

Spills: class, procedure, kit, neutralizer, disposal, drill.

### 8.8 `waste_streams.json` (new)

Streams: source, character, treatment, route, record.

### 8.9 `lab_notebook_fields.json` (new)

Fields: date, campaign, charge, observation, result, grade, waste, follow-up.

### 8.10 Extensions

Extend `chlor_alkali_synthesis_catalog.json`,
`bio_fermentation_catalog.json`, `plastic_pyrolysis_catalog.json`,
`mineral_acid_synthesis_catalog.json`, and `chemical_syntheses.json` with
authored processes, feedstocks, and products.

### 8.11 Items

New items appended to `items.json`: `item_soap_bar`, `item_lye_can`,
`item_bleach_jug`, `item_acid_carboy`, `item_solvent_tin`, `item_glue_pot`,
`item_plastic_sheet`, `item_fuel_canister`, `item_catalyst_bed`,
`item_goggles`, `item_rubber_apron`, `item_wash_bottle`,
`item_neutralizer_sack`, `item_fire_blanket`, `item_label_roll`,
`item_lab_notebook`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

The live synthesis engine states and `PharmaLabState` remain the save owners.
New sub-objects (works, campaigns, feedstocks, catalysts, grades, storage,
safety, waste, notebook) are additive inside them. No new save section.

### 9.2 State to persist

- Works rooms and vessel condition.
- Campaign progress and yields.
- Feedstock sorting and grade stock.
- Catalyst beds and regeneration history.
- Grade labels and purity records.
- Storage assignments and segregation plans.
- Safety equipment, drills, and incidents.
- Waste streams and neutralization records.
- Notebook entries.

### 9.3 Determinism

- Reactions remain deterministic in the live engines; campaigns add scheduling
  and records, not chemistry.
- Batch yields derive from charge, skill, condition, and the live engine's
  process outputs.
- Accidents derive from records, condition, and protection, using the live
  seeded paths only where the game already uses them.
- Paired replay hashes must match; no wall-clock or `System.Random`.

### 9.4 Migration

Legacy saves load with existing engine state and lab state untouched; no works,
campaign, grade, or storage state exists until started. Existing process
definitions keep working; authored catalogs add to them.

### 9.5 Checksum

Invariant-culture floats; integer batch, hour, and unit counts.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `WorksPanel` (new) | Vessels, campaigns, condition | `ReagentHostSession` |
| `FeedstockPanel` (new) | Sorting, grades, stock | same |
| `CatalystPanel` (new) | Beds, regeneration, poisoning | same |
| `GradePanel` (new) | Standards and labels | same |
| `StoragePanel` (new) | Segregation and containers | same |
| `SafetyPanel` (new) | Equipment, drills, incidents | same |
| `WastePanel` (new) | Streams and treatment | same |
| `NotebookPanel` (new) | Recipes and observations | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Labels and grades are legible, high-contrast, and never color-only.
- Alarms have visual and text equivalents; nothing is audio-only.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Storage panes show segregation as a map with clear icons and text.
- Incident records are readable and reviewable in full.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a glass stopper, a bubbling vat, a
vent fan, a label being written, a wash bucket, a still ticking as it cools. No
cue is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `ChlorAlkaliSynthesisEngine` | Chlorine and lye campaigns |
| `PlasticPyrolysisSystem` | Plastic recovery campaigns |
| `FischerTropschSynthesisEngine` | Fuel campaigns |
| `BioFermentationEngine` | Culture and acid campaigns |
| `PharmaLabSystem` | Lab recipes and purity |
| `MineralAcidSynthesisCatalog` | Acid content |
| `SofcElectrochemistryEngine` | Electrochemistry |
| `CryogenicAirSeparationSystem` | Gases |
| `Inventory` | Inputs and outputs |
| `PowerGridSystem` (Wave 2) | Reactor load |
| `VentilationSystem` | Air handling |
| `ShelterAtmosphereSystem` | Air quality |
| `NeedsSystem` | Burns and exposure |
| `MedicalPipelineCoordinator` | Injury routing |
| `DecontaminationSystem` (Wave 2) | Exposure cleanup |
| `WaterTreatmentSystem` (Wave 3) | Process water and waste |
| `SanitationSystem` (Wave 3) | Waste routes |
| `ShelterFireHazardSystem` (Wave 3) | Fire safety |
| `ArchiveDeskSystem` (Wave 4) | Notebook archive |
| `EpilogueChronicleBuilder` | Works milestones |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm the four synthesis engines, pharma lab,
mineral acid catalog, ventilation, atmosphere, and inventory owners. Record
file:line; change nothing.

**Phase 1 — Data + validators.** Author the nine catalogs; extend the five thin
process catalogs; append items; register validators and scanner.

**Phase 2 — Pure Core.** `ChemicalWorksSystem`, `FeedstockSystem`,
`CatalystSystem`, `ReagentGradeSystem`, `ChemicalSafetySystem`,
`ChemicalWasteSystem`, `LabNotebookSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `ReagentHostSession`, selftest coverage, fresh
journey from empty shelf to clean works.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 360-day soak: soap runs, a spill, a chlorine drill, and a
feedstock shortage.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Campaigns | 30 |
| Feedstock sorts | 12 |
| Feedstock grades | 6 |
| Catalysts | 10 |
| Grade standards | 8 |
| Storage classes | 10 |
| Spill protocols | 8 |
| Waste streams | 8 |
| Notebook fields | 12 |
| Extended processes | 40 |
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
| Weapons chemistry | Critical | Content limits §3.3 |
| Second chemistry system | Critical | Extend live engines |
| Dupe outputs | High | Mass balance per campaign |
| Safety as decoration | High | Drills and incidents |
| Waste ignored | Medium | Streams with records |
| Hazard overlap with 19 | Medium | Boundary §0.1 |
| Determinism break | Low | Live seeded paths |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `reagent_works.json` | 30 | 6,000 |
| `feedstock_sorts.json` | 12 | 2,500 |
| `feedstock_grades.json` | 6 | 1,500 |
| `catalysts.json` | 10 | 2,500 |
| `reagent_grades.json` | 8 | 2,000 |
| `chemical_storage.json` | 10 | 2,500 |
| `spill_protocols.json` | 8 | 2,000 |
| `waste_streams.json` | 8 | 2,000 |
| `lab_notebook_fields.json` | 12 | 2,000 |
| Process extensions | 40 | 10,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 16 | 2,500 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~64,000** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R39-1 | Weapons chemistry | Low | Critical | Content limits |
| R39-2 | Second chemistry system | Low | Critical | Live engines |
| R39-3 | Dupe without inputs | Med | High | Mass balance |
| R39-4 | Safety theater | Med | High | Drills and records |
| R39-5 | Waste dumping | Med | High | Streams and records |
| R39-6 | Hazard overlap | Med | High | Boundary contract |
| R39-7 | Alchemical tone | Med | Med | Real inputs and grades |
| R39-8 | Determinism | Low | High | Live paths |
| R39-9 | Content overrun | Med | Med | Budget §13 |
| R39-10 | Apprentice misuse | Low | Med | 16+ supervised |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **How many grades until a mismatch becomes a real failure?** Recommended: a
   small ladder, with mismatches producing weak results, never disasters.
2. **Can the works ever produce a chemical weapon?** Recommended: no,
   unconditionally, as a hard content rule.
3. **Are accidents preventable by perfect play?** Recommended: reduced to rare,
   never eliminated; care lowers risk and never guarantees.
4. **Does the works ever sell reagents outside?** Recommended: yes, through the
   live trade owners, with safety labels honored.
5. **Are apprentices permitted in the acid store?** Recommended: only with
   supervision and never alone.

---

## 17. APPENDIX D — CAMPAIGN TABLE (30 CAMPAIGNS)

| # | Campaign | Engine | Charge | Yield | Grade |
|---|---|---|---|---|---|
| 1 | First soap | chlor-alkali | fat, lye | soap bars | household |
| 2 | Wash soap | chlor-alkali | fat, lye, ash | soap bars | laundry |
| 3 | Hard soap | chlor-alkali | fat, brine | soap blocks | household |
| 4 | Lye stock | chlor-alkali | ash, lime | lye cans | technical |
| 5 | Bleach jug | chlor-alkali | chlorine, lye | bleach | disinfectant |
| 6 | Water chlorine | chlor-alkali | gas feed | chlorine dose | treated |
| 7 | Salt split | chlor-alkali | brine | chlorine, lye | technical |
| 8 | Acid charge one | mineral acid | sulfur, water | acid carboy | technical |
| 9 | Acid charge two | mineral acid | salt, acid | acid stock | technical |
| 10 | Pickling acid | mineral acid | acid, water | pickle bath | works |
| 11 | Battery acid | mineral acid | acid, glass | battery fill | technical |
| 12 | Solvent run | pyrolysis | plastic, heat | solvent tins | technical |
| 13 | Plastic sheet | pyrolysis | sorted plastic | sheet stock | utility |
| 14 | Plastic forms | pyrolysis | sheet, press | shape stock | utility |
| 15 | Tar recovery | pyrolysis | mixed plastic | tar, oil | crude |
| 16 | Glue cook | fermentation | bones, water | glue pots | works |
| 17 | Vinegar vat | fermentation | mash, culture | vinegar | food |
| 18 | Lactic vat | fermentation | milk, culture | acid stock | food |
| 19 | Culture start | fermentation | mash, seed | cultures | works |
| 20 | Dye bath | fermentation | plants, mordant | dye | textile |
| 21 | Fuel batch one | Fischer-Tropsch | gas, catalyst | fuel cans | engine |
| 22 | Fuel batch two | Fischer-Tropsch | coal, water | fuel cans | engine |
| 23 | Lamp fuel | Fischer-Tropsch | oil, catalyst | lamp oil | light |
| 24 | Lubricant | Fischer-Tropsch | oil, wax | grease tins | machine |
| 25 | Rubber batch | pyrolysis | latex scrap | rubber stock | works |
| 26 | Coating batch | lab | resin, solvent | coating | protective |
| 27 | Tablet batch | pharma lab | powder, binder | tablets | medical |
| 28 | Salve batch | pharma lab | fat, herb | salve | medical |
| 29 | Antiseptic | chlor-alkali | chlorine, water | antiseptic | medical |
| 30 | Fertilizer mix | mineral acid | ash, acid | fertilizer | field |

Campaigns are the works' calendar and its vocabulary. Each row is a real chain
from salvage to shelf, and each one has a waste stream attached, because the
shelter's chemistry is only as clean as its worst byproduct.

---

## 18. APPENDIX E — FEEDSTOCK SORT TABLE (12 SORTS)

| # | Sort | Source | Grade | Hazard | Divert if |
|---|---|---|---|---|---|
| 1 | Clean plastic | salvage | high | none | mixed layers |
| 2 | Mixed plastic | salvage | low | fumes | contaminated |
| 3 | Battery cells | salvage | technical | acid, metal | cracked |
| 4 | Salt | pans | high | none | wet |
| 5 | Brine | deep well | raw | none | silt-heavy |
| 6 | Ash | fires | raw | dust | wet |
| 7 | Lime | quarry | technical | caustic | underburned |
| 8 | Fat and tallow | kitchen | high | rancid | spoiled |
| 9 | Bones | kitchen | raw | bio | old |
| 10 | Sulfur | salvage | technical | fumes | wet |
| 11 | Oils | salvage | raw | flammable | water |
| 12 | Plant matter | garden | raw | none | moldy |

Sorting is the works' first quality gate and its first safety gate. The divert
column is the rule that keeps a bad jug out of a good campaign.

---

## 19. APPENDIX F — CATALYST TABLE

| Catalyst | Use | Life | Condition | Regeneration | Poisoning |
|---|---|---|---|---|---|
| Iron bed | FT fuel | 200 h | pores | hydrogen wash | sulfur |
| Cobalt bed | FT fuel | 300 h | surface | steam | water |
| Zinc bed | acid | 150 h | crust | wash | chlorides |
| Copper bed | chlor-alkali | 250 h | plates | re-plate | impurities |
| Nickel bed | hydrogen | 200 h | surface | reduc | carbon |
| Clay bed | cracking | 100 h | coking | burn | metals |
| Yeast bed | fermentation | 40 h | viability | new seed | infection |
| Enzyme bed | fermentation | 60 h | strength | cool | heat |
| Lime bed | gas scrub | 80 h | water | re-burn | saturation |
| Char bed | filters | 40 h | pores | bake | oil |

Catalysts are the works' slow capital. Each bed has a life, a way to come back,
and a way to die, and the table makes the difference legible instead of magical.

---

## 20. APPENDIX G — STORAGE CLASS TABLE

| Class | Examples | Room | Separation | Container |
|---|---|---|---|---|
| Acids | mineral acid | acid store | sand, shelf | glass carboy |
| Bases | lye, lime | dry store | separate shelf | sealed can |
| Chlorine | bleach | cool store | vent | sealed jug |
| Solvents | solvent | fuel room | grounded | metal tin |
| Fuels | fuel, oil | fuel bay | bonded | steel can |
| Oxidizers | nitrates | dry store | far shelf | sealed jar |
| Powders | ash, lime | dry store | dry only | sack |
| Cultures | vats | vats room | warm | vat |
| Medical | salves | clinic | locked | jar |
| Waste | sludges | waste bed | far | sealed bin |

The storage table is the works' most load-bearing document. Almost every serious
accident in a chemical room is a storage disagreement, and almost every fix is
the same boring shelf.

---

## 21. APPENDIX H — SPILL PROTOCOL TABLE

| Class | Immediate | Kit | Neutralizer | Disposal | Drill |
|---|---|---|---|---|---|
| Acid | dilute, flush | sand, lime | lime | bed | monthly |
| Base | dilute, flush | sand, acid | weak acid | bed | monthly |
| Chlorine | vent, leave | masks | none | disperse | quarterly |
| Solvent | absorb, no flame | sawdust | none | burn | quarterly |
| Fuel | contain, cover | clay | none | burn | quarterly |
| Powder | damp, sweep | water | none | bed | monthly |
| Culture | kill, wash | bleach | heat | bed | monthly |
| Mixed | evacuate, call | full kit | per class | bed | quarterly |

Every spill class has an immediate action, a kit, and a disposal. The drill
column is the part shelters skip and the part that decides whether a spill is an
afternoon or an evacuation.

---

## 22. APPENDIX I — WASTE STREAM TABLE

| Stream | Source | Character | Treatment | Route |
|---|---|---|---|---|
| Alkaline wash | soap | caustic | neutralize | drainage |
| Acid wash | acid runs | acidic | lime bed | drainage |
| Salt water | chlor-alkali | saline | dilute | brine bed |
| Tar | pyrolysis | heavy | store | burn pit |
| Solvent slops | pyrolysis | flammable | condense | fuel room |
| Vat dregs | fermentation | organic | compost | garden |
| Catalyst dust | beds | metal | bag | sealed store |
| Filter sludge | scrub | mixed | dry | sealed store |

The waste table is the expansion's proof of honesty. A campaign that cannot say
where its waste goes is a campaign the works is not allowed to run.

---

## 23. APPENDIX J — NOTEBOOK FIELD TABLE

| Field | Required | Example | Purpose |
|---|---|---|---|
| Date | yes | day 214 | record |
| Campaign | yes | first soap | trace |
| Charge | yes | 6 fat, 2 lye | mass balance |
| Conditions | yes | warm, stirred 2h | repeat |
| Observation | yes | trace hardened | learning |
| Yield | yes | 11 bars | output |
| Grade | yes | household | trust |
| Waste | yes | alkaline wash | closure |
| Incident | if any | splash, minor | safety |
| Follow-up | optional | stir longer | improve |
| Verifier | yes | Hesk | honesty |
| Notes | optional | color pale | flavor |

The notebook is the works' memory and its conscience. The verifier field exists
because the shelter decided early that a chemical record is a promise, and
promises are signed.

---

## 24. APPENDIX K — PROCESS EXTENSION TABLE (40 PROCESSES)

| # | Process | Catalog | Input | Output |
|---|---|---|---|---|
| 1 | Soap saponify | chlor-alkali | fat, lye | soap |
| 2 | Hard soap | chlor-alkali | soap, brine | soap bars |
| 3 | Laundry soap | chlor-alkali | fat, ash | soap |
| 4 | Lye leach | chlor-alkali | ash, water | lye |
| 5 | Lime slake | chlor-alkali | lime, water | slaked lime |
| 6 | Chlorine split | chlor-alkali | brine, power | chlorine |
| 7 | Bleach make | chlor-alkali | chlorine, lye | bleach |
| 8 | Water dose | chlor-alkali | chlorine | treated water |
| 9 | Hydrochloric | mineral acid | salt, acid | acid |
| 10 | Sulfuric | mineral acid | sulfur, water | acid |
| 11 | Nitric | mineral acid | nitrate, acid | acid |
| 12 | Battery fill | mineral acid | acid, water | electrolyte |
| 13 | Pickling | mineral acid | acid, water | pickle bath |
| 14 | Plastic melt | pyrolysis | plastic | sheet |
| 15 | Plastic crack | pyrolysis | plastic, heat | solvent |
| 16 | Tar draw | pyrolysis | mixed | tar |
| 17 | Wax recover | pyrolysis | plastic | wax |
| 18 | Rubber reclaim | pyrolysis | latex | rubber stock |
| 19 | Glue cook | fermentation | bone, water | glue |
| 20 | Vinegar | fermentation | mash | vinegar |
| 21 | Lactic | fermentation | milk | acid |
| 22 | Culture grow | fermentation | seed | culture |
| 23 | Dye steep | fermentation | plants | dye |
| 24 | Mordant mix | fermentation | salts | mordant |
| 25 | FT fuel | synthesis | gas, catalyst | fuel |
| 26 | FT wax | synthesis | gas, catalyst | wax |
| 27 | Oil refine | synthesis | oil | lamp oil |
| 28 | Grease make | synthesis | oil, wax | grease |
| 29 | Coat mix | lab | resin, solvent | coating |
| 30 | Tablet press | lab | powder, binder | tablets |
| 31 | Salve melt | lab | fat, herb | salve |
| 32 | Tincture | lab | herb, spirit | tincture |
| 33 | Antiseptic | lab | chlorine, water | antiseptic |
| 34 | Fertilizer | lab | ash, acid | fertilizer |
| 35 | Gas wash | synthesis | gas, lime | clean gas |
| 36 | Salt crystal | pans | brine, sun | salt |
| 37 | Brine pump | well | water, salt | brine |
| 38 | Filter char | kiln | wood | char bed |
| 39 | Ash leach | works | ash, water | potash |
| 40 | Soap mill | works | soap, scent | milled soap |

Forty processes give the works enough content to be a real department. The table
is also proof that nothing appears from nothing: every row has an input, an
output, and a place in the shelter's ordinary life.

---

## 25. APPENDIX L — PRODUCT USE CHAIN TABLE

| Product | Used by | Wave | Without it |
|---|---|---|---|
| Soap | wash line | 22, 27 | hygiene fails |
| Bleach | sanitation | 22 | disinfection slows |
| Lye | soap, drains | 22 | maintenance stalls |
| Acid | works, batteries | 10, 39 | metal work stops |
| Solvent | workshops | 10, 40 | repairs slow |
| Glue | carpentry | 27, 31 | builds slow |
| Plastic sheet | glazing, cover | 05, 29 | crops suffer |
| Fuel | generator, lamps | 21 | power drops |
| Grease | machines | 40 | wear rises |
| Dye | cloth | 27 | cloth plain |
| Vinegar | kitchen | 26 | preservation shrinks |
| Fertilizer | fields | 15 | yields drop |
| Salve | ward | 38 | comfort drops |
| Antiseptic | ward | 22, 38 | infection rises |
| Coating | optics | 29 | lenses suffer |
| Wax | candles | 21 | nights darker |

Sixteen products, sixteen dependencies. The table is the expansion's argument
that chemistry is not a hobby room but a foundation, and that the shelter notices
it most on the day the soap shelf is empty.

---

## 26. APPENDIX M — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_reagent_empty_shelf` | 4 | Soap runs out |
| `quest_reagent_recipe` | 4 | First campaign written |
| `quest_reagent_labels` | 4 | Everything labeled |
| `quest_reagent_charge` | 4 | Sorting becomes a job |
| `quest_reagent_run` | 5 | Soap produced and judged |
| `quest_reagent_bite` | 4 | Injury and response |
| `quest_reagent_rules` | 5 | Storage and protection law |
| `quest_reagent_grade` | 4 | Standards posted |
| `quest_reagent_chlorine` | 5 | Alarm and drill |
| `quest_reagent_salt` | 4 | Brine route open |
| `quest_reagent_fuel` | 5 | Supervised fuel run |
| `quest_reagent_culture` | 4 | Vats producing |
| `quest_reagent_plastic` | 4 | Sheet stock made |
| `quest_reagent_waste` | 5 | Bed closes the loop |
| `quest_reagent_clean` | 3 | Final disposition |

---

## 27. APPENDIX N — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_reagent_sort` | 4 | Sorting table running |
| `quest_reagent_brine` | 3 | Brine hauled |
| `quest_reagent_lime` | 4 | Lime burned |
| `quest_reagent_ash` | 3 | Potash collected |
| `quest_reagent_store` | 3 | Dry store counted |
| `quest_reagent_acid_charge` | 4 | Acid run charged |
| `quest_reagent_acid_glass` | 3 | Glassware stocked |
| `quest_reagent_acid_label` | 3 | Labels applied |
| `quest_reagent_acid_spill` | 4 | Spill drilled |
| `quest_reagent_acid_use` | 3 | Acid put to work |
| `quest_reagent_chlor_charge` | 4 | Campaign run |
| `quest_reagent_chlor_vent` | 3 | Vent tested |
| `quest_reagent_chlor_alarm` | 4 | Alarm drilled |
| `quest_reagent_chlor_bleach` | 3 | Bleach delivered |
| `quest_reagent_chlor_water` | 3 | Water dosed |
| `quest_reagent_pyro_build` | 5 | Machine built |
| `quest_reagent_pyro_feed` | 4 | Feed sorted |
| `quest_reagent_pyro_maintain` | 3 | Machine maintained |
| `quest_reagent_sheet` | 3 | Sheet pressed |
| `quest_reagent_forms` | 4 | Shapes made |
| `quest_reagent_vat_build` | 4 | Vat built |
| `quest_reagent_vat_culture` | 3 | Culture started |
| `quest_reagent_vat_acid` | 3 | Acid fermented |
| `quest_reagent_ft_build` | 5 | Reactor built |
| `quest_reagent_ft_run` | 4 | Fuel batch run |
| `quest_reagent_goggles` | 3 | Eye protection issued |
| `quest_reagent_aprons` | 3 | Aprons and boots |
| `quest_reagent_eyewash` | 4 | Eyewash installed |
| `quest_reagent_fire` | 3 | Blanket and sand |
| `quest_reagent_notebook` | 4 | Ledger kept |

---

## 28. APPENDIX O — NPC DOSSIERS (BRIEF)

**Dala Vint** — chemist. Reads the notebook before the recipe and the label
before the jar. Believes a shelter's chemistry is a public trust kept in private
habits.

**Pock** — lab aide. Cleans, charges, and counts, and has caught more bad
batches at the sorting table than any instrument.

**Hesk** — safety officer. Labels everything, drills everyone, and has never
once been thanked for an accident that did not happen.

**Mirren** — salt and brine hauler. Knows the pans, the well, and the route, and
negotiates for the works like a supplier with pride.

**Osla** — soap maker. Wants a bar that works in cold water and a shelf that is
never empty; tests every batch on her own hands first.

**Gren** — stillman. Treats the fuel room like a kitchen and has learned the
hard way that it is not one; now writes every batch down.

**Tila** — dyer. Uses the vats for color and argues that beauty is infrastructure
for morale, which is a sentence she says exactly once a week.

**Wex** — apprentice. Sixteen, supervised, and faster at labels than anyone;
keeps the spill kit by the door without being told.

---

## 29. APPENDIX P — LOCATION DETAIL

- **The Salvage Yard** — where the works begins, sorting what the world left.
- **The Salt Pans** — brine, sun, and patience.
- **The Lime Quarry** — the oldest chemistry in the world, still working.
- **The Fuel Bay** — steel, grounds, and ventilation, and no shortcuts.
- **The Neutralization Bed** — where the works proves it is honest.
- **The Vent Stack** — the pipe that keeps the shelter breathing.
- **The Ash Pit** — potash from every fire the shelter has ever burned.
- **The Glass Delivery** — carboys and bottles, counted twice.
- **The Barrel Row** — containers with labels in careful handwriting.
- **The Works Muster** — the shift brief where the day's campaign is read out.

---

## 30. APPENDIX Q — MASS BALANCE TABLE (WORKED EXAMPLE)

| Campaign | Input | Kg | Output | Kg | Waste | Kg |
|---|---|---|---|---|---|---|
| First soap | fat | 6 | soap | 11 bars | wash | 40 L |
| First soap | lye | 2 | glycerin | 0.4 | wash | — |
| Bleach | chlorine | 1 | bleach | 12 L | gas | trace |
| Bleach | lye | 2 | — | — | wash | 8 L |
| Acid | sulfur | 3 | acid | 9 L | fumes | vented |
| Acid | water | 8 | — | — | wash | 2 L |
| Fuel | gas | 20 | fuel | 12 L | tar | 4 |
| Fuel | catalyst | 0.1 | wax | 2 | dust | 0.2 |
| Vinegar | mash | 10 | vinegar | 9 L | dregs | 3 |
| Fertilizer | ash | 8 | mix | 10 | dust | 1 |

The table is the works' discipline in one page. Every campaign must be able to
fill in these columns before it runs, and the numbers are what the notebook
verifies afterwards.

---

## 31. APPENDIX R — WORKED 360-DAY WORKS SCENARIO

**Days 1–20.** Soap shelf empty; Dala writes the first recipe; the works is a
back room with an unlabeled bucket.

**Days 21–50.** Labels applied to every container; the bucket is identified and
properly stored; Pock sets up a sorting table and diverts three bad lots.

**Days 51–80.** First soap run succeeds; the wash line restarts; Osla tests the
bars and requests a harder recipe.

**Days 81–110.** Acid spill in the morning shift: one splash, one ward visit,
one blameless review, and the eyewash station installed within the week.

**Days 111–140.** Storage segregation becomes law; Hesk drills the spill
protocol twice; the works runs its first chlorine alarm drill.

**Days 141–170.** Brine route opens from the deep well; the chargers are
balanced; grade standards posted beside every shelf.

**Days 171–200.** Pyrolysis machine built and maintained; the first sheet stock
reaches the greenhouse and the glaziers.

**Days 201–230.** Culture vats start; vinegar and lactic acid enter the kitchen;
Tila's first dye bath reaches the thread works.

**Days 231–260.** Fischer-Tropsch reactor built; Gren runs one supervised batch;
the fuel bay gets bonded flooring and better ventilation.

**Days 261–290.** A catalyst bed is poisoned by sulfur; the failure is traced,
the bed regenerated, and the sorting rule tightened.

**Days 291–320.** Waste bed closes the loop; the first full waste record is
published; the works is audited by its own notebook and passes.

**Days 321–360.** Year review: forty processes, one injury, one alarm, zero
dumps, and a shelf that is not empty.

---

## 32. APPENDIX S — VIGNETTES (TONE SAMPLE)

> Dala holds the jar up to the lamp and reads the label twice, once to check the
> word and once to check the handwriting, because the handwriting is the part
> that tells her whether the person who wrote it was in a hurry.

> Hesk walks the barrel row on a Tuesday and finds one acid jug among the
> bases, and he does not shout, he just moves it and writes the date and the
> name in the margin of the drill sheet, and the next drill has a new line.

> Osla takes the first bar to the wash line and scrubs her own hands with it in
> cold water, and it makes poor lather, and she asks for a harder recipe without
> any drama, and gets one.

> Gren writes down every can he fills now, and the notebook entry is short and
> bad-tempered and honest, and Dala reads it and adds one line: stove was hot.

---

## 33. APPENDIX T — ACCIDENT AND INJURY PROTOCOL

| Step | Action | Owner |
|---|---|---|
| First | Stop the process | nearest worker |
| Second | Move the person to wash | safety |
| Third | Call the ward | runner |
| Fourth | Record the cause | notebook |
| Fifth | Clean the scene | safety |
| Sixth | Neutralize the spill | waste |
| Seventh | Review without blame | works |
| Eighth | Change one step | works |
| Ninth | Drill the change | safety |
| Tenth | Thank the reporter | Dala |

The protocol is the expansion's answer to the temptation to treat accidents as
drama. Every injury is a system telling the shelter which step is missing, and
the response is always the same: care first, then the step.

---

## 34. APPENDIX U — SAFETY EQUIPMENT TABLE

| Equipment | Where | Checked | Replaced |
|---|---|---|---|
| Goggles | every bench | weekly | scratched |
| Face shield | acid bench | weekly | cracked |
| Rubber apron | acid, base | weekly | worn |
| Boots | works floor | weekly | split |
| Gloves | charging | daily | torn |
| Eyewash | scrub | weekly | water stale |
| Wash basin | scrub | daily | dirty |
| Fire blanket | fuel | monthly | used |
| Sand bucket | acid | monthly | damp |
| Neutralizer | spill kit | monthly | spent |
| Labels | shelves | weekly | torn |
| Vent fan | works | weekly | seized |

Safety equipment is checked on a schedule because care that depends on memory
is not care. The table's last column is the one that matters: everything wears
out, including the things that keep people whole.

---

## 35. APPENDIX V — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Wrong charge | bad yield | re-sort, re-run |
| Unlabeled jar | risk | label, inspect, record |
| Acid splash | injury | wash, ward, review |
| Chlorine leak | evacuation | vent, mask, drill |
| Poisoned bed | poor yield | regenerate, trace |
| Grade mismatch | blocked use | re-grade, re-label |
| Fuel fire | damage | blanket, sand, rebuild |
| Waste spill | contamination | neutralize, record |
| Vat infection | lost batch | kill, clean, reseed |
| Feedstock shortage | idle works | route, substitute, wait |

Nothing in the works fails silently: every failure has a record, a cause, and a
next step, which is the difference between a chemistry department and a room
full of accidents.

---

## 36. APPENDIX W — CONTENT REVIEW CHECKLIST

- [ ] No weapons, poisons, or explosives content exists.
- [ ] Hazard agents remain with 19's owners.
- [ ] Narcotics follow the live pharma and dependency rules.
- [ ] `ChlorAlkaliSynthesisEngine` remains the chlorine and lye authority.
- [ ] `PlasticPyrolysisSystem` remains the plastic authority.
- [ ] `FischerTropschSynthesisEngine` remains the fuel authority.
- [ ] `BioFermentationEngine` remains the fermentation authority.
- [ ] Every output has a real input (mass balance).
- [ ] Every stream has a waste record.
- [ ] Apprentices are 16+ and supervised.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses live seeded paths only.

---

## 37. APPENDIX X — GLOSSARY

- **Campaign** — one planned chemical run from charge to shelf.
- **Charge** — the counted inputs for a run.
- **Grade** — the purity standard and its label.
- **Bed** — a catalyst charge with a life and a condition.
- **Segregation** — keeping incompatible classes apart.
- **Neutralization** — treating a waste stream before disposal.
- **Notebook** — the verified record of what was done and what happened.
- **Vent** — the route that keeps the room breathable.
- **Mass balance** — inputs minus outputs equals waste, always.
- **Blameless review** — finding the missing step, never the guilty person.

---

## 38. APPENDIX Y — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `ChemicalWorksSystem` | engines | campaigns, yields | reaction logic |
| `ChlorAlkaliSynthesisEngine` | feedstock | process state | safety policy |
| `PlasticPyrolysisSystem` | plastic | machine state | grades |
| `FischerTropschSynthesisEngine` | gas | batch state | storage |
| `BioFermentationEngine` | culture | vat state | kitchen state |
| `PharmaLabSystem` | recipes | lab state | dependency |
| `FeedstockSystem` | salvage | sorts, grades | charges |
| `CatalystSystem` | beds | condition | yields |
| `ReagentGradeSystem` | purity | labels | reactions |
| `ChemicalSafetySystem` | stores | rules, drills | injuries |
| `ChemicalWasteSystem` | streams | treatment | sanitation policy |
| `LabNotebookSystem` | campaigns | entries | engine state |
| `VentilationSystem` | airflow | vents | chemistry |
| `NeedsSystem` | burns | needs | medical care |
| `WaterTreatmentSystem` | chemicals | dosing | chemistry |
| `SanitationSystem` | waste | routing | treatment |
| `EpilogueChronicleBuilder` | milestones | chronicle | works state |

---

## 39. APPENDIX Z — DATA SCHEMA DETAIL (NEW CATALOGS)

**`reagent_works.json`** — `campaign_id`, `display_name`, `engine`, `charge[]`,
`run_hours`, `yield_item`, `yield_amount`, `grade`, `waste_stream`, `tags`.

**`feedstock_sorts.json`** — `sort_id`, `display_name`, `source`, `grade`,
`hazard`, `divert_if`, `tags`.

**`feedstock_grades.json`** — `grade_id`, `display_name`, `purity_band`,
`allowed_uses[]`, `price_effect`, `tags`.

**`catalysts.json`** — `catalyst_id`, `display_name`, `use`, `life_hours`,
`condition`, `regeneration`, `poisoning`, `tags`.

**`reagent_grades.json`** — `grade_id`, `display_name`, `check`, `label`,
`blocked_uses[]`, `tags`.

**`chemical_storage.json`** — `class_id`, `display_name`, `examples[]`, `room`,
`separation`, `container`, `tags`.

**`spill_protocols.json`** — `class_id`, `display_name`, `immediate`, `kit[]`,
`neutralizer`, `disposal`, `drill`, `tags`.

**`waste_streams.json`** — `stream_id`, `display_name`, `source`, `character`,
`treatment`, `route`, `tags`.

**`lab_notebook_fields.json`** — `field_id`, `display_name`, `required`,
`example`, `purpose`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on missing
or duplicate IDs, invalid references, or out-of-range numbers.

---

## 40. APPENDIX AA — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Batches completed | output | Works |
| Grade pass rate | quality | Grades |
| Waste records filed | honesty | Waste |
| Spill response time | safety | Safety |
| Drills held | readiness | Safety |
| Catalyst life used | capital | Catalysts |
| Feedstock diverted | quality gate | Feedstock |
| Injuries per year | safety trend | Ward |
| Label audit failures | discipline | Storage |
| Notebook verification rate | learning | Notebook |

Telemetry is diagnostic only; it never gates content and never becomes a score
against a worker.

---

## 41. APPENDIX AB — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] The five thin process catalogs are extended with real content.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] The five synthesis and lab authorities remain untouched.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the review checklist in §36.
- [ ] Phase 7 soak shows campaigns, a spill, a drill, and a shortage.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No weapons, poison, or explosive content exists.

---

## 42. APPENDIX AC — OPEN QUESTIONS FOR REVIEW

1. Should grade mismatches block a use outright or produce a weak result?
2. How many notices does a mislabel earn before it becomes a policy matter?
3. Is the waste bed a place the player builds once or maintains forever?
4. Should the works sell reagents outside, and who sets the prices?
5. Are fuel campaigns gated behind the reactor build or available earlier?
6. Does the notebook ever become a public archive, and at what delay?
7. Should a chlorine alarm evacuate the whole shelter or the works wing?
8. How much of the works can run at night with limited staff?

None of these may be decided unilaterally; each changes tone and balance.

---

## 43. APPENDIX AD — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 15 The Deep Root | Fertilizer and safe sprays |
| 1 | 16 The Rebuilt Body | Salves and antiseptics |
| 2 | 17 The Long Evening | Dyes, inks, and pigments |
| 2 | 18 The Underneath | Vents and sealed stores |
| 2 | 19 The Bitter Air | Hazard boundary and decon |
| 2 | 21 The Grid | Reactor load and fuel |
| 3 | 22 The Clean Flow | Chlorine, bleach, water dosing |
| 3 | 23 The Alarm | Spill and chlorine alarms |
| 3 | 24 The Long Goodbye | Comfort salves |
| 3 | 26 The Common Table | Vinegar, cultures, preserving |
| 4 | 27 The Thread | Dyes, mordants, laundering |
| 4 | 28 The Lesson | Apprentice training |
| 4 | 29 The Glass | Carboys, lenses, coatings |
| 4 | 30 The Press | Notebooks, labels, records |
| 4 | 31 The Kiln | Vessels, linings, neutralizers |
| 5 | 32 The Wild | Plant dyes and remedies |
| 5 | 33 The Weather | Campaign scheduling |
| 5 | 34 The Long Road | Incoming feedstock |
| 5 | 35 The Habit | Precursors under live rules |
| 5 | 36 The Watch | Hazard watches and drills |
| 6 | 38 The Ward | Burns, exposure, antiseptics |

Each hook is additive. The Reagent can ship alone, and every other expansion
can ship without it.

---

## 44. APPENDIX AE — ENDING PROSE SKETCHES

**The Clean Works.** Labels, grades, segregation, and drills make the chemical
room the safest place in the shelter, and nobody ever notices, which is the
point.

**The Full Shelf.** Soap, bleach, acid, glue, plastic, fuel, and dye reach every
workshop and wash line, and the shelter stops noticing chemistry because
chemistry is everywhere.

**The Careful Scale.** The works stays small, slow, and trustworthy, and its
notebook has more entries than it has accidents, which is the whole ambition.

**The Hot Week.** A chlorine scare and a spill force a rebuild, and the works
returns with better venting, better storage, and a deeper respect for ordinary
safety.

**The Empty Barrel.** A feedstock run fails and a campaign stops, and the
shelter learns that a chemical works is a supply chain before it is a science.

**Fade.** A bar of soap on a shelf with a label in careful handwriting, and the
wash line running, and nobody thinking about it at all.

---

## 45. APPENDIX AF — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Explosion spectacle | tone | procedure and records |
| Alchemy recipes | dishonesty | inputs and mass balance |
| Unlabeled jars | unsafe | labels as law |
| Safety skipped | cruelty | drills and equipment |
| Waste dumped | dishonest | streams and treatment |
| Weapons chemistry | harmful | hard prohibition |
| Drug party loop | tone | live rules only |
| Free reagents | economy break | real inputs |
| Child labor | exploitation | 16+ supervised |
| Purity ignored | trust break | grades and labels |

The list exists because chemistry is the most dangerous ordinary thing a shelter
does, and the expansion's rule is that the drama belongs in the discipline and
never in the accident.

---

## 46. APPENDIX AG — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Campaigns | 30 | 6,000 |
| Feedstock sorts | 12 | 2,500 |
| Feedstock grades | 6 | 1,500 |
| Catalysts | 10 | 2,500 |
| Grade standards | 8 | 2,000 |
| Storage classes | 10 | 2,500 |
| Spill protocols | 8 | 2,000 |
| Waste streams | 8 | 2,000 |
| Notebook fields | 12 | 2,000 |
| Process extensions | 40 | 10,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 16 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~64,000** |

---

## 47. APPENDIX AH — FIRST YEAR OF THE WORKS

| Month | Focus | Milestone |
|---|---|---|
| 1 | Empty shelf | first recipe |
| 2 | Labels | every container named |
| 3 | Sorting | divert rules live |
| 4 | Soap | wash line restarts |
| 5 | Acid | glassware and rules |
| 6 | Segregation | storage law |
| 7 | Grades | standards posted |
| 8 | Chlorine drill | alarm tested |
| 9 | Brine | salt route open |
| 10 | Pyrolysis | sheet stock made |
| 11 | Fuel | first supervised batch |
| 12 | Waste bed | loop closed |

A year of the works is a year of small disciplines, and the shelter that keeps
them ends the year with a clean room and a quiet conscience.

---

## 48. APPENDIX AI — CHEMICAL TRUST COVENANT

| Clause | Promise |
|---|---|
| Containment | Nothing escapes its container |
| Honesty | Every stream has a record |
| Protection | Every worker has equipment |
| Labels | Every jar is named |
| Grades | Every promise is checked |
| Training | Every task has a trained person |
| Review | Every accident changes a step |
| Prohibition | No weapons, no poisons, no explosives |
| Waste | Nothing is dumped |
| Neighbors | Water, air, and food are protected first |

The covenant is the works' reason for existing and its condition of operation. A
shelter's civilization is measured by what it does with the dangerous things it
needs, and this table is the measurement.

---

## 49. APPENDIX AJ — WORKS SHIFT TABLE

| Shift | Hours | Staff | Campaigns | Safety lead |
|---|---|---|---|---|
| Morning | 06–14 | chemist, aide | acid, soap | Hesk |
| Afternoon | 14–22 | aide, stillman | fuel, vats | Gren |
| Night | 22–06 | duty only | none | watch tie |
| Campaign day | 08–16 | full | one campaign | Hesk |
| Cool day | 08–16 | aide | cleaning | Pock |
| Vat check | split | anyone | culture care | Tila |
| Spill drill | monthly | all | none | Hesk |
| Rest | post-campaign | all | none | roster |

The works keeps ordinary hours. Night is for watching vats and reading
instruments, never for starting a campaign, which is the same rule the ward uses
for surgery and the shelter uses for everything dangerous.

---

## 50. APPENDIX AK — ACID HANDLING RULES TABLE

| Rule | Reason | Check |
|---|---|---|
| Glass only | acid eats metal | label |
| Sand tray | breakage catch | visual |
| Apron and goggles | splash | pre-run |
| Water first, acid slow | heat control | procedure |
| Open window | fumes | visual |
| Never mix | reactions | label |
| Rinse everything | residue | after |
| Neutralizer near | response | weekly |
| Eye wash near | injury | weekly |
| Write it down | review | notebook |

Ten rules, all boring, all load-bearing. The table is written to be read aloud at
the muster, because rules that live only in a manual are rules the shelter has
decided not to follow.

---

## 51. APPENDIX AL — WORKS ROOM TABLE

| Room | Vessels | Vent | Power | Water | Access |
|---|---|---|---|---|---|
| Main | reactors | fan | med | yes | staff |
| Still | column | fan | med | yes | staff |
| Vats | vats | open | none | yes | staff |
| Dry | none | pass | none | no | staff |
| Acids | storage | fan | none | yes | trained |
| Fuel | storage | fan | none | no | trained |
| Notebook | none | pass | lamp | no | staff |
| Scrub | sinks | pass | none | yes | all |

Rooms are defined by what they hold and what they need. The access column shows
where training matters and where anyone may walk in, which is how the works
keeps its doors open without pretending its hazards are ordinary.

---

## 52. APPENDIX AM — LABEL SPECIMEN TABLE

| Field | Example | Size | Ink |
|---|---|---|---|
| Name | Sulfuric acid | large | black |
| Class | Acid | large | red edge |
| Grade | Technical | med | black |
| Date | Day 214 | small | black |
| Source | Works run 12 | small | black |
| Hazard | Corrosive | large | red |
| First aid | Water, 15 min | med | black |
| Keeper | D. Vint | small | black |

A label is the smallest document the works produces and the one that saves the
most people. The specimen table is included so the implementation can render
labels consistently and so the player can read one across a room.

---

## 53. APPENDIX AN — PRODUCT QUALITY CHECK TABLE

| Product | Check | Pass | Fail action |
|---|---|---|---|
| Soap | lather, feel | white, firm | re-mill |
| Bleach | smell, strip | sharp, clear | dilute, re-run |
| Acid | clear, fizz test | clear | filter, re-run |
| Glue | set test | hard set | re-cook |
| Plastic sheet | flex, thickness | even | re-press |
| Fuel | flame test | steady | re-distill |
| Grease | spread test | smooth | re-blend |
| Vinegar | taste, smell | sharp | re-vat |
| Dye | swatch test | even | re-steep |
| Fertilizer | mix test | crumbly | re-mix |

Quality checks are performed by the people who will use the product, not only
by the works. The fail column always names a real remedy, because the point of
checking is to fix a batch rather than to condemn it.

---

## 54. APPENDIX AO — FEEDSTOCK SEASON TABLE

| Season | Salt | Ash | Fat | Plastic | Plants |
|---|---|---|---|---|---|
| Spring | pans open | low | low | salvage | early |
| Summer | high | low | med | salvage | peak |
| Autumn | med | med | high | median | harvest |
| Winter | low | high | med | salvage | stored |
| Ash | covered | high | med | salvage | greenhouse |
| Storm | none | low | low | none | none |

The works has seasons the same way the fields do. The table is how the shelter
learns to make soap in autumn when the fat is plentiful and stock the shelf for
the winter when it is not.

---

## 55. APPENDIX AP — WORKS CYCLE TABLE

| Cycle | Duration | Output | Rest |
|---|---|---|---|
| Short | 4–8h | one product | overnight |
| Standard | 8–16h | full campaign | next day |
| Long | 2–5 days | fuel, acid | rota |
| Continuous | weeks | vats, cultures | daily check |
| Cold | months | aging stock | none |

Cycles give the works its calendar. The rest column is deliberate: even the
long campaigns leave the machines cold and the people rested, because a works
that runs forever is a works that eventually runs wrong.

---

## 56. CLOSING STATEMENT

ASHFALL already has chlor-alkali, pyrolysis, fuel synthesis, fermentation, and a
pharma lab, and it already owns the systems that handle burns, exposure,
contamination, and waste. What it lacks is the department around them: sorting,
catalysts, grades, labels, storage, drills, waste records, and a notebook. The
Reagent adds that department without adding a second chemistry system or a
single weapon. It adds a bar of soap with a label in careful handwriting, a
vetted charge, a spill kit by the door, and a shelter that is trustworthy with
strong things.

> Wave 6 note: this plan is one of five Wave 6 expansion bibles (37–41). Each is
> self-contained; none requires another to ship. The shared Wave 6 index lives at
> `docs/expansions/wave6/WAVE6_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible. Evidence
> anchors: `ChlorAlkaliSynthesisEngine` (`ChlorAlkaliProcessDef`,
> `ChlorAlkaliFeedstockCost`, `ChlorAlkaliProcessStatus`,
> `ChlorAlkaliHazardState`), `PlasticPyrolysisSystem` (`PyrolysisMachineDef`
> with construction, condition, and maintenance fields),
> `FischerTropschSynthesisEngine` (`FischerTropschBatchState` with
> `operator_skill`, `feed_quality`, `process_variation`,
> `SynthesisOutputBatch`), `BioFermentationEngine` (`BioFermentationPhase`,
> `BioFermentationFeedstockCharge`, `BioFermentationState`), `PharmaLabState`
> (`currentPhase`, `temperature`, `purity`, `contaminationRisk`), and the thin
> catalogs `chlor_alkali_synthesis_catalog.json` (1,617 B),
> `bio_fermentation_catalog.json` (3,332 B), `plastic_pyrolysis_catalog.json`
> (3,464 B), `mineral_acid_synthesis_catalog.json` (4,677 B), and
> `chemical_syntheses.json` (7,001 B).