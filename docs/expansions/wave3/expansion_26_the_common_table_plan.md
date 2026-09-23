# ASHFALL — Expansion 26 Design Bible
# THE COMMON TABLE
### Wave 3 · Cuisine, Rations, Preservation, Nutrition, Hunger, and Food Culture

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-20
**Domain owners touched:** `Ashfall.Core` (KitchenNutrition), `Ashfall.Core.Farming` (NutritionDiversity), `Ashfall.Core.Shelter` (FoodPreservation), `Ashfall.Core.Economy` (ResourceRationing), `Ashfall.Core.Survivors` (RationConflict, Desperation)
**Proposed host owner:** `CommonTableHostSession` (extends kitchen, preservation, and rationing surfaces)
**Existing save sections:** `kitchen_nutrition`, `food_preservation`, `rationing`, `ration_conflict`, `desperation`, `farming`
**Existing CLI verbs:** `--kitchen-nutrition-selftest`, `--food-preservation-selftest`, `--nutrition-diversity-selftest`, `--rationing-selftest`, `--ration-conflict-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already owns food the right way. `KitchenNutritionSystem` runs prep jobs with
assigned cooks, reserved inputs, progress, portions, and a stamped `cookQualityPermille`
that carries the cook's productivity into the result; it tracks pantry items, cellar
temperature, refrigeration, and a full serving log. `NutritionDiversitySystem` keeps
a rolling 14-day dietary-diversity log per survivor, tracks category coverage, and
reports deficiency state — while explicitly *not* applying consequences itself
(that goes through the canonical `NeedsSystem.Modify` surface). `FoodPreservationSystem`
tracks batch cohorts with freshness, tiers, spoilage, unpowered days, and active
curing jobs. `ResourceRationingSystem`, `RationConflictSystem` (perceived fairness,
resentment targets, confrontation and theft thresholds), and `DesperationSystem`
model the social pressure of scarcity. `CulinaryRationCatalog`,
`RefrigerationFermentationCatalog`, `FermentationYeastCatalog`,
`CryoPreservationCatalog`, and `BioFermentationEngine` add depth.

But the content is thin where the game is most personal: `food_preservation.json` is
5.2 KB, `nutrition_profiles.json` 3.2 KB, and the culinary catalogs are small. The
shelter has a kitchen but no cuisine, a ration system but no food culture, and a
nutrition tracker but almost no deficiency content.

**The Common Table** turns that machinery into the shelter's daily ritual: menus,
cooks, preservation, deficiencies, ration politics, food traditions, feasts, and
hunger — because in a survival game, the meal is the moment survival becomes visible.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin; **`PROPOSED`** new.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

A shelter is a machine for turning calories into tomorrow. The common table is where
that machine becomes a community — or does not.

**The Common Table** is the expansion about eating: what the shelter cooks, what it
saves, what it runs out of, and who eats first. It adds a menu and cuisine layer on
top of the live kitchen, a preservation economy of smoke, salt, cans, cellars, and
ferment, a deficiency layer that makes nutrition visible in bodies, a ration-policy
layer that makes fairness visible in faces, and a food culture that marks loss,
harvest, and survival at a shared table.

The expansion's hard rules are inherited from the live owners: the kitchen remains the
prep authority, nutrition diversity remains the coverage authority, preservation
remains the spoilage authority, and every consequence routes through `NeedsSystem`
and the medical pipeline. Nothing new is invented twice.

### 1.2 The five loops it adds

```
   Menu ──► Cook ──► Serve ──► Diversity ──► Bodies
     │        │         │          │            │
     ▼        ▼         ▼          ▼            ▼
   recipes, pantry, portions,  category     deficiencies,
   seasons  skill,    fairness  coverage     recovery,
            quality              14-day      appetite
     │                                    │
     ▼                                    ▼
   Preservation ◄── smoke, salt, can, cellar, ferment ◄── spoilage
     │
     ▼
   Culture ──► traditions, feasts, memories ──► morale, identity
```

### 1.3 What the player manages

1. **The menu.** Which recipes are cooked, from what, for whom, and how often.
   Variety affects morale; repetition grinds people down.
2. **The cook.** Skill, assigned cook, quality, fatigue, and the occasional inspired
   meal. `cookQualityPermille` already carries skill into the result.
3. **The pantry.** Stock, cellar temperature, refrigeration, and what is about to
   spoil first.
4. **Preservation.** Smoking, salting, canning, drying, fermenting, cellaring, and
   the power that refrigeration needs. Spoilage is the enemy.
5. **Nutrition.** The live 14-day diversity log; deficiencies show up as real
   symptoms and real recovery, never as a hidden bar.
6. **Rations.** Policy, portions, fairness, resentment, confrontation, and theft.
7. **Hunger.** Phases of scarcity with authored escalation, desperation, and the
   hard choices of a hungry winter.
8. **Culture.** Food traditions, feast days, food memories, and the shared table as
   identity.

### 1.4 What it is not

- Not a second kitchen or nutrition system. `KitchenNutritionSystem` and
  `NutritionDiversitySystem` remain owners.
- Not a second preservation system. `FoodPreservationSystem` remains the authority.
- Not a second rationing system. `ResourceRationingSystem` and `RationConflictSystem`
  remain owners.
- Not a cooking minigame. Recipes are selected and prepared, not diced.
- Not a second save authority.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/KitchenNutritionSystem.cs` | Prep jobs, cooks, pantry, cellar, refrigeration, portions, serving log | `LIVE` |
| `Assets/Ashfall.Core/Farming/NutritionDiversitySystem.cs` | 14-day diversity log, categories, deficiency state | `LIVE` |
| `Assets/Ashfall.Core/Shelter/FoodPreservationSystem.cs` | Batch cohorts, freshness, tiers, curing, spoilage | `LIVE` |
| `Assets/Ashfall.Core/Shelter/FoodPreservationCatalog.cs` | Preservation tiers | `LIVE` |
| `Assets/Ashfall.Core/Economy/ResourceRationingSystem.cs` | Rationing policy | `LIVE` |
| `Assets/Ashfall.Core/Survivors/RationConflictSystem.cs` | Fairness, resentment, confrontation, theft | `LIVE` |
| `Assets/Ashfall.Core/Survivors/DesperationSystem.cs` | Desperation events | `LIVE` |
| `Assets/Ashfall.Core/Narrative/CulinaryRationCatalog.cs` | Ration culture corpus | `LIVE` |
| `Assets/Ashfall.Core/Narrative/RefrigerationFermentationCatalog.cs` | Ferment and cold | `LIVE` |
| `Assets/Ashfall.Core/Shelter/BioFermentationEngine.cs` | Fermentation | `LIVE` |
| `src/Host/` kitchen/preservation/ration hosts | Host | `LIVE` |
| Kitchen, preservation, ration UI surfaces | UI | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `recipes.json` | **61 KB** | rich crafting/cooking recipe corpus |
| `food_preservation.json` | 5.2 KB | preservation tiers |
| `nutrition_profiles.json` | 3.2 KB | per-food nutrition |
| `culinary_ration_codex.json` | narrative | ration culture |
| `nutrition_profiles` consumers | live | diversity log |
| `pharma_recipes.json` | 12.9 KB | adjacent chemistry |

### 2.3 Confirmed gaps

- **GAP-26-1 — No menu system.** Recipes exist; no menu, meal plan, or cuisine
  variety with morale meaning.
- **GAP-26-2 — No deficiency content.** The diversity system reports deficiency
  state; almost no authored deficiencies, symptoms, or recovery exist.
- **GAP-26-3 — Preservation is a small tier list.** No methods catalog, no smokehouse
  or cannery content, no recipe-by-method matrix.
- **GAP-26-4 — No ration-policy depth.** Policies exist; no authored policy
  families, enforcement, or social consequences beyond fairness.
- **GAP-26-5 — No hunger arcs.** `DesperationSystem` exists; no authored phases,
  escalation, or famine content.
- **GAP-26-6 — No food culture.** No traditions, feast days, food memories, or
  shared-table identity.
- **GAP-26-7 — No cook skill content.** Cook quality carries into jobs; no authored
  skill progression, specialties, or signature dishes.
- **GAP-26-8 — No kitchen locations or NPCs.**
- **GAP-26-9 — Spoilage lacks a narrative face.** Cohorts exist; no authored pantry
  losses, salvage, or reuse.

### 2.4 Non-duplication statement

This expansion will **not** add a second kitchen, nutrition, preservation, rationing,
or desperation system. It extends `KitchenNutritionSystem`, `NutritionDiversitySystem`,
`FoodPreservationSystem`, `ResourceRationingSystem`, `RationConflictSystem`,
`DesperationSystem`, and the fermentation engines with data and additive subsystems.
It routes all nutrition consequences through `NeedsSystem.Modify` and all medical
consequences through `MedicalPipelineCoordinator`, exactly as the live contracts
require. It leaves production to the Deep Root expansion and public culture to the
Long Evening expansion.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — The meal is the moment survival becomes visible.** Players feel
scarcity at the table before they feel it in a bar. The expansion makes the table the
game's emotional anchor.

**Pillar 2 — Variety is health.** Monotony is not a debuff; it is a deficiency. The
live 14-day diversity log is the right model, and the expansion gives it symptoms.

**Pillar 3 — Preservation is time.** Smoking, salting, and canning are how a shelter
borrows from a good month to survive a bad one. Spoilage is the interest on that loan.

**Pillar 4 — Fairness is felt.** Ration policy is judged, resented, and sometimes
violated. The expansion never treats fairness as a hidden multiplier.

**Pillar 5 — Food is memory.** A meal can mark a harvest, a death, a return, or a
founding. The table is where the shelter says what it has survived.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| A good meal | Steam, a full tin, quiet eating | Feast spectacle |
| A bad meal | Grey mash eaten without comment | Gross-out |
| A deficiency | A symptom noticed, a diagnosis made | Medical horror |
| Preservation | Smoke, salt, jars, patience | Cooking show |
| Rationing | A queue, a scale, a look | Dickensian misery |
| A feast | Shared food, a named reason | Carnival |

### 3.3 Content limits

- Deprivation is depicted with restraint and dignity; no misery tourism.
- No real-world cuisine, brand, or dish is copied wholesale.
- Deficiency symptoms are fictionalized and practical, never medical horror.
- No shaming of survivors for appetite, weight, or eating.
- Child nutrition is handled carefully and without sentimentality.

---

## 4. THE COMMON TABLE WORLD

### 4.1 Interior rooms

- **`room_kitchen_main`** — ranges, pots, prep tables, and the cook's corner.
- **`room_bakery`** — oven, proofing, and the smell of grain.
- **`room_cellar`** — cool storage, racks, and rotation.
- **`room_smokehouse`** — smoke, hooks, and patience.
- **`room_cannery`** — jars, lids, pressure, and boiling water.
- **`room_mill`** — grain milling and flour.
- **`room_ice_house`** — winter ice and summer use.
- **`room_common_table`** — the long table where the shelter eats together.

### 4.2 Exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_market_stalls` | The Stalls | 4 | Food trade and haggling |
| `loc_salt_pan` | The Salt Pans | 5 | Salt for preservation |
| `loc_smoke_yard` | The Smoke Yard | 4 | Outdoor smoking and drying |
| `loc_ice_field` | The Ice Field | 5 | Winter ice harvest |
| `loc_cannery_ruins` | The Cannery | 6 | Jars, lids, and equipment |
| `loc_spice_walk` | The Spice Walk | 4 | Herbs, salt, and flavor |
| `loc_grain_store` | The Grain Store | 5 | Bulk grain and pests |
| `loc_hunger_marker` | The Marker | 3 | A memorial to a hungry year |
| `loc_herb_walk` | The Herb Walk | 3 | Culinary and medicinal herbs |
| `loc_canteen_wreck` | The Canteen | 5 | Pre-war food service salvage |

All locations require valid item references and scanner registration.

### 4.3 The meal loop

The shelter eats in a rhythm: pantry review, menu selection, prep, service, and
aftermath (diversity log, morale, resentment). The live systems own each step; the
expansion authors what the steps mean and what they cost.

---

## 5. MAIN STORYLINE — "THE TABLE WE SET"

### 5.1 Central conflict

The shelter's cook, **Ottilie Frayne**, notices that people are tired in a way that
sleep does not fix. The pantry is full — the same three foods every day. The diversity
log shows a falling category coverage, and two survivors are showing early symptoms
of something that is not an infection.

At the same time, the ration board is under pressure. The administrator wants to
stretch the stores through winter by cutting portions; the workers want more; a
family with a child on half-rations is already stealing. And in the cannery ruins
there is a pre-war pressure canner that would let the shelter preserve the autumn
surplus — if the jars, lids, and seals can be found and the fuel budgeted.

The expansion's question: **what does a shelter owe its people at the table, and who
decides when the food runs short?**

### 5.2 Theme (unspoken)

**You can survive on the same meal for a year. You may not survive as the same people.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_cook_ottilie_frayne` | Ottilie Frayne | Cook | Menus, variety, and dignity at the table |
| `npc_administrator_marek` | Marek Dowd | Administrator | Portions, winter, and the arithmetic |
| `npc_dietitian_tam` | Tam Orr | Dietitian | Diversity log, deficiencies, recovery |
| `npc_preserver_berek` | Berek Halm | Preserver | Smoke, salt, cans, and the cellar |
| `npc_baker_yla` | Yla Brant | Baker | Grain, oven, and the daily loaf |
| `npc_steward_koval` | Koval | Steward | Ration board, scales, and fairness |
| `npc_child_ration_pim` | Pim | Child | The human face of the ration |
| `npc_trader_venn` | Venn | Food trader | Spices, salt, and prices |

### 5.4 Story beats (15)

1. **The Tiredness.** Symptoms appear; the diversity log is consulted.
2. **The Same Three Foods.** The menu is audited; variety is proposed.
3. **The Portion Board.** Marek proposes cuts; the shelter reacts.
4. **The Steal.** A ration theft is discovered.
5. **The Cannery.** The pressure canner is located; jars are needed.
6. **The Salt Run.** Preservation salt must be sourced.
7. **The Smokehouse.** Meat is smoked before it spoils.
8. **The Deficiency.** A diagnosis is made; recovery begins.
9. **The Menu.** A varied menu is written and defended.
10. **The Cellar.** Rotation is fixed; spoilage drops.
11. **The Feast.** An autumn surplus is marked with a shared meal.
12. **The Hungry Week.** A shortage tests the policy and the peace.
13. **The Recovery.** Bodies and trust are rebuilt.
14. **The Reckoning.** The cost of the winter and the fairness of the table.
15. **The Table We Set.** Final disposition of food policy and culture.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Variety | invest / minimal / monotony | health vs. stock |
| Portions | full / cut / tiered | fairness vs. winter |
| Theft | punish / forgive / reform | order vs. trust |
| Preservation | expand / basic / none | future vs. now |
| Ration policy | equal / worker / child-first / needs | fairness model |
| Feast | hold / cancel / small | culture vs. stock |
| Trade | sell surplus / store / share | market vs. reserve |
| Final | table as right / table as reward / table as ration | identity |

### 5.6 Endings (5 + fade)

1. **The Full Table** — variety and preservation secure the shelter; the meal is culture.
2. **The Stretched Pot** — rations hold the shelter together through winter, barely.
3. **The Empty Cellar** — preservation failed; the hungry season is real and costly.
4. **The Fair Scale** — a ration policy everyone accepts becomes an institution.
5. **The Shared Loaf** — the surplus is shared with a neighbor; standing and debt both rise.
6. **Fade** — the same three foods continue; nothing gets worse for a while.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_table_`. Schema follows `year_of_ash_quests.json`.

### 6.1 Main questline (15)

`quest_table_tiredness`, `quest_table_same_three`, `quest_table_portion_board`,
`quest_table_the_steal`, `quest_table_cannery`, `quest_table_salt_run`,
`quest_table_smokehouse`, `quest_table_deficiency`, `quest_table_menu`,
`quest_table_cellar`, `quest_table_feast`, `quest_table_hungry_week`,
`quest_table_recovery`, `quest_table_reckoning`, `quest_table_table_we_set`.

### 6.2 Side quests (30)

**Menu and cooking (5)**
- `quest_table_menu_write` — write the week's menu
- `quest_table_recipe_variety` — add a new recipe
- `quest_table_cook_skill` — train a cook
- `quest_table_signature` — perfect a shelter dish
- `quest_table_pantry_audit` — audit and rotate

**Nutrition (5)**
- `quest_table_diversity_log` — keep the log current
- `quest_table_category_gap` — fill a missing category
- `quest_table_deficiency_recover` — treatment and recovery
- `quest_table_child_nutrition` — children's needs
- `quest_table_work_diet` — heavy-work nutrition

**Preservation (5)**
- `quest_table_smoke_batch` — smoke a batch
- `quest_table_salt_batch` — salt a batch
- `quest_table_can_batch` — can a batch
- `quest_table_dry_batch` — dry a batch
- `quest_table_ferment_batch` — ferment a batch

**Storage (5)**
- `quest_table_cellar_cool` — cool the cellar
- `quest_table_spoilage_salvage` — salvage what can be saved
- `quest_table_ice_harvest` — harvest winter ice
- `quest_table_pest_control` — protect the grain store
- `quest_table_rotation` — fix stock rotation

**Rations (5)**
- `quest_table_policy_write` — write the ration policy
- `quest_table_fairness_hearing` — hear grievances
- `quest_table_scale_calibrate` — calibrate the scales
- `quest_table_special_need` — a medical ration
- `quest_table_theft_case` — a theft case

**Culture (5)**
- `quest_table_tradition` — establish a food tradition
- `quest_table_feast_plan` — plan a feast
- `quest_table_food_memory` — record a remembered meal
- `quest_table_shared_table` — make the table communal
- `quest_table_hunger_marker` — mark a hungry year

### 6.3 Repeatable quests (8)

`quest_table_repeat_cook`, `quest_table_repeat_serve`,
`quest_table_repeat_preserve`, `quest_table_repeat_audit`,
`quest_table_repeat_ration`, `quest_table_repeat_feast`,
`quest_table_repeat_market`, `quest_table_repeat_log`.

### 6.4 Dynamic hooks

Live systems emit prep, serving, diversity, spoilage, rationing, conflict, and
desperation events. The generator attaches authored follow-ups without a new event
bus.

### 6.5 Constraints

- Nutrition consequences route only through `NeedsSystem.Modify`.
- Preservation spoilage stays in `FoodPreservationSystem`.
- Ration fairness stays in `RationConflictSystem`.
- No food may be created without production or trade.
- No recipe may grant medical effects outside the medical pipeline.
- No feast may be free; it consumes real stores.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `MenuSystem` (new, `Ashfall.Core`)

**Owns:** menus, meal plans, cuisine variety, repetition fatigue, and morale effects.
**Consumes:** `KitchenNutritionSystem`, `Inventory`, `NutritionDiversitySystem`.
**Data:** `menus.json`.
**Rules:** a menu is a real plan with inputs and portions; variety is tracked across
the live 14-day window; monotony produces authored fatigue that routes through
`NeedsSystem`.

### 7.2 `DeficiencySystem` (new, `Ashfall.Core.Farming`)

**Owns:** authored deficiencies, symptoms, diagnosis, treatment, and recovery.
**Consumes:** `NutritionDiversitySystem` coverage, `NeedsSystem`,
`MedicalPipelineCoordinator`. **Data:** `deficiency_profiles.json`.
**Rules:** deficiencies are noticed as symptoms and confirmed by a dietitian;
recovery needs real food and time; no hidden bars.

### 7.3 `PreservationMethodSystem` (extend `FoodPreservationSystem`)

**Owns:** methods (smoke, salt, can, dry, ferment, cellar, ice) and their inputs,
capacity, skill, and failure modes. **Consumes:** live cohorts and curing jobs,
`Inventory`, `PowerGridSystem`. **Data:** `preservation_methods.json`.
**Rules:** each method has a real cost and a quality outcome; failure spoils the
batch; methods combine into a preservation economy.

### 7.4 `RationPolicySystem` (extend `ResourceRationingSystem`)

**Owns:** authored policy families, enforcement, special rations, and hearings.
**Consumes:** `ResourceRationingSystem`, `RationConflictSystem`, `NeedsSystem`,
`GuiltInsomniaSystem`. **Data:** `ration_policies.json`.
**Rules:** policy is written, argued, and judged; enforcement costs trust; special
rations require medical justification.

### 7.5 `HungerPhaseSystem` (new, `Ashfall.Core.Survivors`)

**Owns:** authored famine phases, escalation, desperation triggers, and recovery.
**Consumes:** `DesperationSystem`, `NeedsSystem`, `RationConflictSystem`,
`Inventory`. **Data:** `hunger_phases.json`.
**Rules:** hunger escalates through authored phases with warnings; desperation is
visible before it is dangerous; recovery takes longer than deprivation.

### 7.6 `FoodCultureSystem` (new, thin, `Ashfall.Core.Culture`)

**Owns:** food traditions, feast days, food memories, and shared-table identity.
**Consumes:** `CultureCreationSystem` (Wave 2), `MemorialSystem`, `NeedsSystem`.
**Data:** `food_traditions.json`, `food_memories.json`.
**Rules:** culture is authored, not procedurally generated; feasts consume real
stores; memories persist into the chronicle.

### 7.7 `CulinarySkillSystem` (new, thin, `Ashfall.Core.Survivors`)

**Owns:** cook skill progression, specialties, and signature dishes. **Consumes:**
`SkillProgressionSystem`, `KitchenNutritionSystem`. **Data:** `kitchen_roles.json`.
**Rules:** skill raises quality and unlocks recipes; specialties are earned; no chef
is irreplaceable, but losing one hurts.

### 7.8 Systems explicitly not added

- No second kitchen, nutrition, preservation, rationing, or culture system.
- No cooking minigame.
- No new currency or resource.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `menus.json` (new)

```json
{
  "schema_version": 1,
  "menus": [
    {
      "menu_id": "menu_staple_week",
      "display_name": "Staple Week",
      "meal_slots": 21,
      "required_categories": ["staple", "protein"],
      "optional_categories": ["vegetable", "fat"],
      "servings_per_day": 3,
      "variety_target": 3,
      "morale_baseline": 2.0,
      "repetition_penalty": 0.5,
      "tags": ["baseline", "survivable", "dull"]
    }
  ]
}
```

### 8.2 `deficiency_profiles.json` (new)

Deficiency rows: name, missing category, onset days, symptoms, diagnosis, treatment,
recovery days, and severity bands.

### 8.3 `preservation_methods.json` (new)

Method rows: name, inputs, capacity, hours, skill, fuel/power, quality bands,
failure mode, and output tiers.

### 8.4 `ration_policies.json` (new)

Policy rows: name, portions, priority rule, enforcement, special allowances, and
social effects.

### 8.5 `hunger_phases.json` (new)

Phase rows: name, trigger, portions, morale effect, desperation risk, and recovery
condition.

### 8.6 `food_traditions.json` (new)

Tradition rows: name, trigger, menu, participants, morale effect, and chronicle line.

### 8.7 `food_memories.json` (new)

Memory rows: dish, person, era, and morale/chronicle effect.

### 8.8 `kitchen_roles.json` (new)

Role rows: cook, baker, preserver, dietitian, steward, with skill and duties.

### 8.9 `food_preservation.json` (extend)

Existing tier schema preserved; new tiers and qualities.

### 8.10 `nutrition_profiles.json` (extend)

New food rows with the existing nutrition fields.

### 8.11 `recipes.json` (extend)

New shelter recipes using the existing recipe schema, tagged by meal, category, and
preservation method.

### 8.12 Items

New items appended to `items.json`: `item_preserving_salt`, `item_smoke_wood`,
`item_canning_jar`, `item_jar_lid`, `item_drying_rack`, `item_ice_block`,
`item_yeast_culture`, `item_spice_pouch`, `item_cook_apron`,
`item_ration_scale`, `item_menu_slate`, `item_feast_cloth`,
`item_grain_sack`, `item_seal_wax`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

Existing stores: kitchen nutrition, food preservation, rationing, ration conflict,
desperation, farming. New sub-objects are additive. No new save section.

### 9.2 State to persist

- Menus and repetition history.
- Deficiency state and recovery.
- Preservation method jobs and outcomes.
- Ration policy and hearings.
- Hunger phase and desperation.
- Food traditions and memories.
- Cook skills and specialties.

### 9.3 Determinism

- Spoilage, preservation success, and deficiency onset are deterministic given rates.
- Cook quality is already stamped per job; no new randomness.
- Paired replay hashes must match.

### 9.4 Migration

Legacy saves load with no menus, no deficiencies, no methods, no policies, no phases,
and no traditions. Existing kitchen, preservation, and ration state is untouched.

### 9.5 Checksum

Invariant-culture floats; integer-permille for quality and freshness.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `KitchenPanel` (extend) | Prep jobs, cooks, pantry, servings | `CommonTableHostSession` |
| `MenuPanel` (new) | Menus, variety, repetition | same |
| `NutritionPanel` (new) | Diversity log, categories, deficiencies | same |
| `PreservationPanel` (new) | Methods, batches, freshness, spoilage | same |
| `RationPanel` (new) | Policy, portions, fairness, hearings | same |
| `HungerPanel` (new) | Phase, desperation, recovery | same |
| `FoodCulturePanel` (new) | Traditions, feasts, memories | same |
| `CookPanel` (new) | Cook skills and specialties | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Nutrition status is shown as observed symptoms and categories, never a hidden bar.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Ration cuts state exactly who loses what.
- Spoilage and shortage warnings precede the loss.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a ladle, a lid sealing, a scale, an oven
door, a full table, an empty pot. No cue is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `KitchenNutritionSystem` | Menus, cooks, pantry extended |
| `NutritionDiversitySystem` | Deficiencies and recovery |
| `FoodPreservationSystem` | Methods and quality extended |
| `ResourceRationingSystem` | Policies and enforcement extended |
| `RationConflictSystem` | Fairness consequences |
| `DesperationSystem` | Hunger phases |
| `NeedsSystem` | Sole nutrition and morale consequence surface |
| `MedicalPipelineCoordinator` | Deficiency care |
| `SkillProgressionSystem` | Cook skills |
| `CultureCreationSystem` | Food culture and memories |
| `MemorialSystem` | Hunger markers |
| `Inventory` | Food, salt, jars, fuel |
| `PowerGridSystem` | Refrigeration and canning |
| `TradingSystem` | Food trade |
| `Farming` / `GreenhouseSystem` | Production inputs |
| `EpilogueChronicleBuilder` | Food milestones |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm `KitchenNutritionSystem`,
`NutritionDiversitySystem`, `FoodPreservationSystem`, `ResourceRationingSystem`,
`RationConflictSystem`, `DesperationSystem`, fermentation engines, and hosts. Record
file:line; change nothing.

**Phase 1 — Data + validators.** Extend recipes, preservation, and nutrition; author
menus, deficiencies, methods, policies, phases, traditions, memories, roles. Register
validators and scanner.

**Phase 2 — Pure Core.** `MenuSystem`, `DeficiencySystem`,
`PreservationMethodSystem`, `RationPolicySystem`, `HungerPhaseSystem`,
`FoodCultureSystem`, `CulinarySkillSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip, determinism.

**Phase 4 — Host + CLI.** `CommonTableHostSession`, extended selftest verbs, fresh journey.

**Phase 5 — UI.** Extended and new surfaces with lifecycle and accessibility.

**Phase 6 — Content.** Locations, rooms, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 180/360-day soak including harvest, winter, and famine.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Menus | 15 |
| Recipes | 40 new |
| Deficiency profiles | 12 |
| Preservation methods | 10 |
| Ration policies | 10 |
| Hunger phases | 8 |
| Food traditions | 15 |
| Food memories | 20 |
| Kitchen roles | 6 |
| Nutrition profiles | 30 new |
| Locations | 10 |
| Rooms | 8 |
| NPCs | 8 |
| Main quests | 15 |
| Side quests | 30 |
| Repeatable | 8 |
| Items | 14 |
| Endings | 5 + fade |
| Prose estimate | 60,000–75,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Second kitchen/nutrition system | Critical | Extend live owners |
| Consequence bypasses `NeedsSystem` | Critical | Route through `Modify` |
| Monotony becomes a hidden stat | High | Symptoms and log visibility |
| Preservation trivializes scarcity | High | Real inputs and failure |
| Ration policy unfeeling | High | Hearts and hearings |
| Famine too punishing | Medium | Authored phases and recovery |
| Determinism break | Low | Host-forked RNG only where live |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `menus.json` | 15 | 4,000 |
| `recipes.json` | +40 | 8,000 |
| `deficiency_profiles.json` | 12 | 3,500 |
| `preservation_methods.json` | 10 | 3,000 |
| `ration_policies.json` | 10 | 3,000 |
| `hunger_phases.json` | 8 | 2,500 |
| `food_traditions.json` | 15 | 3,500 |
| `food_memories.json` | 20 | 3,500 |
| `kitchen_roles.json` | 6 | 1,500 |
| `nutrition_profiles.json` | +30 | 4,000 |
| `food_preservation.json` | +10 | 2,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 14 | 2,000 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~69,000** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R26-1 | Second kitchen system | Low | Critical | Extend owners |
| R26-2 | Needs bypass | Low | Critical | Route through `Modify` |
| R26-3 | Hidden monotony stat | Med | High | Visible log and symptoms |
| R26-4 | Preservation trivial | Med | High | Inputs, failure, capacity |
| R26-5 | Ration policy cold | Med | High | Hearings and hearts |
| R26-6 | Famine too hard | Med | Med | Phases and recovery |
| R26-7 | Determinism | Low | High | Live seeded paths |
| R26-8 | Content overrun | Med | Med | Budget §13 |
| R26-9 | Food culture shallow | Med | Med | Authored traditions |
| R26-10 | Deficiency horror | Low | Med | Practical, dignified prose |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Can monotony be fully avoided?** Recommended: no; variety is a real cost, and
   the best a shelter can do is rotate intelligently.
2. **Do deficiencies require a dietitian to diagnose?** Recommended: yes; otherwise
   symptoms are visible but the cause is uncertain.
3. **Can the player force a ration policy?** Recommended: yes, with enforcement costs
   and hearings.
4. **Does a feast consume stores that could stretch winter?** Recommended: yes; that
   is the decision.
5. **Can a food memory become a tradition?** Recommended: yes, after repetition.

---

## 17. APPENDIX D — MENU TABLE (15 MENUS)

| # | Menu | Slots | Required | Optional | Variety | Morale | Penalty |
|---|---|---|---|---|---|---|---|
| 1 | Staple Week | 21 | staple, protein | vegetable | 3 | 2.0 | 0.5 |
| 2 | Harvest Table | 21 | staple, vegetable | fruit, fat | 6 | 5.0 | 0.2 |
| 3 | Winter Thin | 21 | staple | none | 2 | 0.5 | 0.8 |
| 4 | Worker's Plate | 21 | staple, protein, fat | vegetable | 4 | 3.0 | 0.4 |
| 5 | Clinic Diet | 21 | soft, protein | fruit | 4 | 2.5 | 0.3 |
| 6 | Child's Table | 21 | staple, protein, fruit | vegetable | 5 | 4.0 | 0.3 |
| 7 | Forager's Week | 21 | variety | none | 7 | 4.5 | 0.2 |
| 8 | Preserved Only | 21 | preserved | none | 3 | 1.5 | 0.7 |
| 9 | Fast Day | 7 | none | none | 0 | −1.0 | n/a |
| 10 | Feast Table | 3 | festive | festive | 8 | 10.0 | 0 |
| 11 | Recovery Diet | 21 | soft, protein, fruit | vegetable | 5 | 3.0 | 0.2 |
| 12 | Night Watch Menu | 7 | portable | none | 3 | 1.5 | 0.4 |
| 13 | Trade Week | 21 | staple | variety | 4 | 2.5 | 0.4 |
| 14 | Ration Board Menu | 21 | tiered | none | 2 | 0 | 0.9 |
| 15 | The Long Table | 21 | staple, protein, vegetable, fruit | fat | 8 | 6.0 | 0.1 |

The menu is the expansion's daily decision. A dull menu is survivable; a varied one
is healthier and happier; a feast is a deliberate spend. Monotony is authored,
tracked, and felt, never hidden.

---

## 18. APPENDIX E — DEFICIENCY TABLE (12 DEFICIENCIES)

| # | Deficiency | Missing category | Onset days | Symptoms | Treatment | Recovery |
|---|---|---|---|---|---|---|
| 1 | Scurvy-analogue | fruit/vegetable | 30 | bleeding gums, fatigue | citrus, greens | 21 |
| 2 | Night blindness | fruit/vegetable | 40 | dim vision | liver, greens | 30 |
| 3 | Weak bones | fat/vegetable | 60 | fractures | fish, sun | 45 |
| 4 | Nerve ache | variety | 50 | numbness | grain variety | 40 |
| 5 | Anemia | protein | 35 | pallor, fatigue | meat, legumes | 25 |
| 6 | Wasting | calories | 14 | weight loss | full rations | 40 |
| 7 | Gut trouble | fiber | 21 | stomach pain | fiber foods | 20 |
| 8 | Slow healing | protein | 30 | wounds linger | meat, rest | 30 |
| 9 | Cold intolerance | fat | 28 | shivering | fat, warmth | 20 |
| 10 | Confusion | variety | 45 | memory slips | varied diet | 35 |
| 11 | Swollen gut | protein | 25 | swelling | protein | 30 |
| 12 | Bone ache | minerals | 55 | pain, weakness | minerals, dairy | 45 |

Deficiencies are noticed as symptoms and confirmed by a dietitian. Recovery needs
real food, not a pill, and a shelter that ignores the diversity log pays in bodies.

---

## 19. APPENDIX F — PRESERVATION METHOD TABLE (10 METHODS)

| # | Method | Inputs | Capacity | Hours | Skill | Fuel/Power | Failure |
|---|---|---|---|---|---|---|---|
| 1 | Smoke | meat, smoke wood | 40 kg | 12 | 2 | low | sour |
| 2 | Salt | meat, salt | 60 kg | 24 | 1 | none | rot |
| 3 | Can | any, jar, seal | 30 jars | 6 | 3 | high | botulism-analogue |
| 4 | Dry | fruit, veg | 50 kg | 20 | 1 | none | mold |
| 5 | Ferment | veg, culture | 40 kg | 96 | 2 | none | spoilage |
| 6 | Cellar | any | 200 kg | 0 | 1 | cool | frost/rot |
| 7 | Ice | any | 100 kg | 0 | 1 | ice | melt |
| 8 | Pickle | veg, salt, vinegar | 45 kg | 48 | 2 | none | softness |
| 9 | Render | fat | 20 kg | 8 | 2 | med | rancid |
| 10 | Cure-Smoke | meat, salt, smoke | 35 kg | 36 | 4 | med | mixed |

Every method costs something: time, fuel, salt, jars, or power. The expansion's
intended economy is that a shelter which preserves in the good months eats in the bad
ones, and one that does not learns why the smokehouse existed.

---

## 20. APPENDIX G — RATION POLICY TABLE (10 POLICIES)

| # | Policy | Portions | Priority | Enforcement | Special | Social effect |
|---|---|---|---|---|---|---|
| 1 | Equal | standard | none | trust | medical | fair, fragile |
| 2 | Worker-Heavy | standard+ | workers | scales | medical | resented |
| 3 | Child-First | standard | children | staff | child needs | respected |
| 4 | Needs-Based | varied | assessed | board | medical | fair, slow |
| 5 | Tiered | tiered | needs | scales | medical | contentious |
| 6 | Medical-First | standard | sick | staff | medical | humane |
| 7 | Rotation | standard | rotating | trust | medical | clever |
| 8 | Strict Winter | cut | none | guards | medical | hard |
| 9 | Harvest Bounty | generous | none | trust | none | high |
| 10 | Scarcity Rule | minimal | essential | guards | medical | dangerous |

Policy is written, argued, and judged. Every policy produces winners and losers, and
the expansion makes both visible in the ration board and the mess hall.

---

## 21. APPENDIX H — HUNGER PHASE TABLE (8 PHASES)

| # | Phase | Trigger | Portions | Morale | Desperation | Recovery |
|---|---|---|---|---|---|---|
| 1 | Full | stock > 60 days | full | 0 | none | n/a |
| 2 | Stretched | stock 30–60 | full | −1 | none | restock |
| 3 | Careful | stock 14–30 | cut 10% | −2 | low | restock |
| 4 | Thin | stock 7–14 | cut 25% | −4 | med | restock |
| 5 | Hungry | stock < 7 | cut 40% | −7 | high | harvest |
| 6 | Famine | no restock 7 days | cut 60% | −12 | very high | harvest |
| 7 | Crisis | no restock 14 days | minimum | −18 | extreme | aid, harvest |
| 8 | Recovery | restock begins | step up | +2/phase | falling | time |

Hunger escalates through authored phases, with visible warnings and a recovery path.
The expansion never makes starvation a surprise, and recovery always takes longer
than deprivation because that is true.

---

## 22. APPENDIX I — FOOD TRADITION TABLE (15 TRADITIONS)

| # | Tradition | Trigger | Menu | Participants | Morale | Chronicle |
|---|---|---|---|---|---|---|
| 1 | Founding Meal | founding day | Feast Table | all | +10 | "They ate together." |
| 2 | Harvest Table | first harvest | Harvest Table | all | +8 | "The first grain." |
| 3 | Long Night Meal | winter solstice | Winter Thin | all | +5 | "They shared the dark." |
| 4 | Names Meal | memorial | simple | all | +3 | "The empty seats." |
| 5 | Return Meal | expedition home | Worker's Plate | all | +6 | "They came back." |
| 6 | New Arrival Meal | intake | Child's Table | all | +4 | "A place was set." |
| 7 | Trade Meal | treaty | Trade Week | partners | +5 | "They broke bread." |
| 8 | Builder's Meal | project done | Worker's Plate | workers | +4 | "The beam held." |
| 9 | Children's Meal | school year | Child's Table | children | +5 | "The young ate first." |
| 10 | Quiet Meal | after a death | simple | family | +2 | "They ate in silence." |
| 11 | Thaw Meal | first melt | Harvest Table | all | +6 | "The water moved." |
| 12 | Marker Meal | hunger marker | Winter Thin | all | +3 | "They remembered the thin year." |
| 13 | Guard's Meal | long watch | Night Watch Menu | guards | +3 | "Someone kept watch." |
| 14 | Clinic Meal | recovery | Recovery Diet | patients | +4 | "They ate to heal." |
| 15 | Last Meal | terminal care | chosen dish | family | +5 | "They chose the dish." |

Traditions are authored and earned. They give the shelter a food culture and give the
chronicle a warm line to remember, which is the expansion's quiet payoff.

---

## 23. APPENDIX J — FOOD MEMORY TABLE (20 MEMORIES)

| # | Memory | Dish | Era | Effect |
|---|---|---|---|---|
| 1 | First Loaf | bread | founding | morale |
| 2 | The Last Tin | peaches | pre-war | grief |
| 3 | Mother's Stew | stew | pre-war | comfort |
| 4 | The Birthday Cake | grain cake | founding | joy |
| 5 | Thin Soup | broth | hungry year | grief |
| 6 | The Feast | roast | festival | joy |
| 7 | Hospital Rice | rice | pre-war | calm |
| 8 | River Fish | fish | post-war | comfort |
| 9 | The Shared Loaf | bread | neighbor | pride |
| 10 | Salt Meat | cured | post-war | comfort |
| 11 | The Missing Plate | none | mourning | grief |
| 12 | The Child's Cup | milk | founding | joy |
| 13 | The Trade Meal | spice | treaty | pride |
| 14 | Cold Ash Bread | bread | winter | grief |
| 15 | The Garden Salad | greens | harvest | joy |
| 16 | The Guard's Tin | tinned | watch | comfort |
| 17 | The Clinic Broth | broth | illness | calm |
| 18 | The Last Meal | chosen | terminal | grief |
| 19 | The First Harvest | grain | harvest | pride |
| 20 | The Empty Table | none | famine | grief |

Food memories persist into the chronicle and can be offered as menu requests. They
are how the shelter says that a meal was not only calories.

---

## 24. APPENDIX K — KITCHEN ROLE TABLE (6 ROLES)

| # | Role | Skill | Duties | Fatigue | Risk |
|---|---|---|---|---|---|
| 1 | Cook | cooking | prep jobs | high | burns |
| 2 | Baker | baking | bread, oven | high | burns |
| 3 | Preserver | preservation | smoke, salt, can | med | cuts, smoke |
| 4 | Dietitian | knowledge | diversity log | low | none |
| 5 | Steward | organization | rations, scales | med | conflict |
| 6 | Cellar Keeper | logistics | rotation, cool | med | falls |

Kitchen work is hot, repetitive, and constant. The expansion gives it the same
respect as the foundry: a shelter runs on its cooks as surely as on its smiths.

---

## 25. APPENDIX L — RECIPE EXPANSION TABLE (40 RECIPES)

| # | Recipe | Meal | Category | Inputs | Preservation |
|---|---|---|---|---|---|
| 1 | Ash Grain Porridge | breakfast | staple | grain, water | none |
| 2 | Rye Loaf | breakfast | staple | rye, yeast | none |
| 3 | Hard Biscuit | travel | staple | flour | dry |
| 4 | Root Hash | main | staple, vegetable | tubers, fat | none |
| 5 | Salt Pork | main | protein | pork, salt | salt |
| 6 | Smoked Fish | main | protein | fish, smoke | smoke |
| 7 | Bean Stew | main | protein, fiber | beans, water | none |
| 8 | Mushroom Soup | main | vegetable | mushrooms | none |
| 9 | Grain Mash | main | staple | grain | none |
| 10 | Picked Greens | side | vegetable | greens, salt | pickle |
| 11 | Cellar Cabbage | side | vegetable | cabbage | ferment |
| 12 | Dried Fruit | snack | fruit | fruit | dry |
| 13 | Berry Preserve | snack | fruit | berries, sugar | can |
| 14 | Honey Cake | treat | festive | flour, honey | none |
| 15 | Seed Bread | breakfast | staple | seeds, flour | none |
| 16 | Fish Stew | main | protein | fish, water | none |
| 17 | Nut Loaf | main | protein, fat | nuts, grain | none |
| 18 | Fat Cakes | main | fat | tallow, flour | none |
| 19 | Bone Broth | main | protein | bones, water | none |
| 20 | Herb Tea | drink | herb | herbs | dry |
| 21 | Grain Coffee | drink | substitute | roasted grain | dry |
| 22 | Cellar Pickle | side | vegetable | cucumber, salt | pickle |
| 23 | Smoked Sausage | main | protein | meat, salt | cure-smoke |
| 24 | Canned Vegetables | side | preserved | veg, jar | can |
| 25 | Canned Meat | main | preserved | meat, jar | can |
| 26 | Milk Porridge | breakfast | dairy | milk, grain | none |
| 27 | Goat Cheese | side | dairy | milk | ferment |
| 28 | Egg Tin | breakfast | protein | eggs, salt | can |
| 29 | Garden Salad | side | vegetable | greens | none |
| 30 | Festival Roast | festive | festive | meat, root | none |
| 31 | Memorial Bread | memorial | staple | flour, water | none |
| 32 | Clinic Broth | clinic | soft | broth, herb | none |
| 33 | Recovery Pudding | clinic | soft, fruit | grain, fruit | can |
| 34 | Worker's Bar | field | staple, fat | grain, fat | dry |
| 35 | Watch Ration | field | portable | biscuit, meat | dry |
| 36 | Trade Travel Cake | field | staple | flour, honey | dry |
| 37 | Pickled Egg | side | protein | eggs, vinegar | pickle |
| 38 | Salt Fish | main | protein | fish, salt | salt |
| 39 | Root Cellar Stew | main | staple | roots, bones | none |
| 40 | The Long Table Stew | festive | festive | everything | none |

Each recipe carries a meal slot, category tags, inputs, and a preservation method
where applicable. The live recipe schema is preserved; the expansion only adds rows
and tags.

---

## 26. APPENDIX M — NUTRITION PROFILE EXPANSION TABLE (30 FOODS)

| # | Food | Cal | Protein | Vit C | Micro | Fat | Fiber |
|---|---|---|---|---|---|---|---|
| 1 | Grain | 1.0 | 0.3 | 0.0 | 0.2 | 0.1 | 0.4 |
| 2 | Rye | 1.0 | 0.4 | 0.0 | 0.3 | 0.1 | 0.6 |
| 3 | Tuber | 1.0 | 0.2 | 0.2 | 0.2 | 0.0 | 0.4 |
| 4 | Root | 0.8 | 0.2 | 0.3 | 0.3 | 0.0 | 0.5 |
| 5 | Greens | 0.3 | 0.3 | 0.9 | 0.6 | 0.0 | 0.5 |
| 6 | Cabbage | 0.3 | 0.2 | 0.7 | 0.4 | 0.0 | 0.5 |
| 7 | Mushroom | 0.3 | 0.7 | 0.1 | 0.6 | 0.1 | 0.3 |
| 8 | Bean | 0.9 | 0.8 | 0.1 | 0.4 | 0.1 | 0.8 |
| 9 | Pea | 0.8 | 0.7 | 0.3 | 0.3 | 0.0 | 0.7 |
| 10 | Grain Sprout | 0.6 | 0.5 | 0.6 | 0.5 | 0.1 | 0.4 |
| 11 | Fish | 0.7 | 1.0 | 0.1 | 0.7 | 0.4 | 0.0 |
| 12 | Salt Fish | 0.7 | 1.0 | 0.0 | 0.7 | 0.4 | 0.0 |
| 13 | Smoked Fish | 0.7 | 1.0 | 0.0 | 0.6 | 0.5 | 0.0 |
| 14 | Pork | 0.9 | 0.9 | 0.0 | 0.4 | 0.8 | 0.0 |
| 15 | Salt Pork | 0.9 | 0.9 | 0.0 | 0.3 | 0.9 | 0.0 |
| 16 | Goat Meat | 0.8 | 1.0 | 0.0 | 0.4 | 0.4 | 0.0 |
| 17 | Goat Milk | 0.6 | 0.6 | 0.2 | 0.8 | 0.5 | 0.0 |
| 18 | Goat Cheese | 0.8 | 0.8 | 0.0 | 0.9 | 0.7 | 0.0 |
| 19 | Egg | 0.7 | 1.0 | 0.0 | 0.7 | 0.6 | 0.0 |
| 20 | Berry | 0.5 | 0.1 | 1.0 | 0.5 | 0.0 | 0.4 |
| 21 | Dried Fruit | 0.9 | 0.1 | 0.4 | 0.5 | 0.0 | 0.5 |
| 22 | Preserve | 1.0 | 0.1 | 0.2 | 0.3 | 0.0 | 0.2 |
| 23 | Honey | 1.0 | 0.0 | 0.0 | 0.1 | 0.0 | 0.0 |
| 24 | Nut | 1.0 | 0.6 | 0.0 | 0.7 | 0.9 | 0.5 |
| 25 | Tallow | 1.0 | 0.0 | 0.0 | 0.1 | 1.0 | 0.0 |
| 26 | Herb | 0.1 | 0.1 | 0.3 | 0.5 | 0.0 | 0.2 |
| 27 | Salt | 0.0 | 0.0 | 0.0 | 0.1 | 0.0 | 0.0 |
| 28 | Broth | 0.3 | 0.4 | 0.1 | 0.4 | 0.2 | 0.0 |
| 29 | Algae | 0.4 | 0.6 | 0.3 | 0.8 | 0.1 | 0.3 |
| 30 | Fungus | 0.4 | 0.7 | 0.1 | 0.7 | 0.1 | 0.4 |

Nutrition profiles are the diversity log's raw data. A shelter that cannot cover
categories in 14 days will see a deficiency, and the table is how it plans around
that.

---

## 27. APPENDIX N — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_table_tiredness` | 3 | Symptoms noticed; log consulted |
| `quest_table_same_three` | 4 | Audit the menu; argue variety |
| `quest_table_portion_board` | 4 | Propose cuts; hear the room |
| `quest_table_the_steal` | 4 | A theft; punish, forgive, or reform |
| `quest_table_cannery` | 5 | Locate canner; find jars and seals |
| `quest_table_salt_run` | 4 | Source preservation salt |
| `quest_table_smokehouse` | 4 | Build and run a smoke batch |
| `quest_table_deficiency` | 5 | Diagnose; treat; recover |
| `quest_table_menu` | 5 | Write a varied menu; defend it |
| `quest_table_cellar` | 4 | Fix rotation; cut spoilage |
| `quest_table_feast` | 5 | Plan and hold a feast |
| `quest_table_hungry_week` | 6 | A shortage tests policy and peace |
| `quest_table_recovery` | 4 | Rebuild bodies and trust |
| `quest_table_reckoning` | 5 | Cost the winter; judge the table |
| `quest_table_table_we_set` | 3 | Final disposition; epilogue |

---

## 28. APPENDIX O — NPC DOSSIERS (BRIEF)

**Ottilie Frayne** — cook. Believes a meal is a promise the shelter makes to itself.
Practical, exacting, and quietly furious that variety is treated as a luxury when it
is medicine. The expansion's heart.

**Marek Dowd** — administrator. Does the winter arithmetic and hates it. Cuts
portions because someone must, and takes the blame because someone must.

**Tam Orr** — dietitian. Keeps the diversity log, diagnoses deficiencies, and has the
least popular job in the shelter: telling people what they are not eating.

**Berek Halm** — preserver. Smoke, salt, jars, and patience. Believes a full cellar
is a moral position and runs the smokehouse accordingly.

**Yla Brant** — baker. Owns the oven and the daily loaf, which is the only meal
everyone eats together. Her bread is the shelter's clock.

**Koval** — steward. Keeps the ration board, calibrates the scales, and hears every
grievance. Fair by method, tired by nature.

**Pim** — child. On the ration list, and the reason the ration list is not abstract.

**Venn** — food trader. Spices, salt, and prices. Brings flavor into a shelter that
has forgotten it exists.

---

## 29. APPENDIX P — LOCATION DETAIL

- **The Stalls** — food trade; haggling, scales, and a smell that travels.
- **The Salt Pans** — salt for curing; wind, sun, and iodine questions.
- **The Smoke Yard** — hooks, smoke, and meat that will last a season.
- **The Ice Field** — winter harvest; ice blocks and a saw.
- **The Cannery** — pre-war jars, lids, and a pressure canner that still seals.
- **The Spice Walk** — herbs; salt and flavor, the two things that make monotony bearable.
- **The Grain Store** — bulk grain, a mill, and pests.
- **The Marker** — a memorial to a hungry year; a stone and a list.
- **The Herb Walk** — culinary and medicinal herbs, side by side.
- **The Canteen** — pre-war food service salvage; trays, urns, and menus.

---

## 30. APPENDIX Q — SPOILAGE AND PRESERVATION MODEL

| Condition | Freshness loss/day | Warning | Action |
|---|---|---|---|
| Ambient cool | 4% | 30% | eat first |
| Ambient warm | 12% | 50% | preserve now |
| Cellar | 1% | 15% | rotate |
| Ice house | 0.5% | 10% | refill ice |
| Refrigerated | 0.3% | 5% | power check |
| Smoked | 0.2% | 10% | hang dry |
| Salted | 0.1% | 5% | keep dry |
| Canned | 0.05% | 5% | seal check |
| Fermented | 0.2% | 10% | watch surface |
| Dried | 0.1% | 5% | keep dry |

The model is authored so that preserving is always worth it and always costs
something. A shelter that preserves in autumn eats in winter, and a shelter that
does not learns the difference in February.

---

## 31. APPENDIX R — DIVERSITY AND DEFICIENCY MODEL

The live system keeps a 14-day rolling window. The expansion gives it meaning:

| Category coverage | State | Consequences |
|---|---|---|
| 6+ categories | excellent | morale bonus |
| 4–5 | good | none |
| 3 | fair | slow recovery |
| 2 | poor | deficiency risk |
| 1 | monotony | deficiency likely |
| 0 | starvation | crisis |

Deficiency onset is deterministic given coverage and days. The first symptom is
subtle; the diagnosis requires a dietitian; recovery requires both time and the
missing category. This is the expansion's central act of honesty: nutrition is not a
number, it is a story that plays out in a body.

---

## 32. APPENDIX S — WORKED 360-DAY FOOD SCENARIO

**Days 1–30.** The tiredness appears. Coverage is 2 categories. The dietitian
diagnoses an early deficiency. The menu is rewritten to rotate three staples and a
vegetable.

**Days 31–90.** The cannery is reached; jars and seals are sourced. The salt run
succeeds. The smokehouse is built; the first batch cures. Coverage rises to 4.

**Days 91–150.** The cellar rotation is fixed and spoilage halves. A theft is caught;
the shelter chooses forgiveness and a reform to the ration board. The first feast
marks the harvest.

**Days 151–220.** Winter thins the menu to preserved foods; coverage falls to 3; the
phase advances to Careful. The kitchen stretches stocks with broth and grain.

**Days 221–300.** A hungry week tests everything: the ration policy holds, the
desperation stays visible, and one deficiency recurs. Recovery begins with the first
greens.

**Days 301–360.** The table is reset. Traditions are declared. The chronicle records
the dishes and the year. The epilogue reads the menu and the marker.

---

## 33. APPENDIX T — VIGNETTE (TONE SAMPLE)

> Ottilie stands at the pot with a wooden spoon and tastes the broth, and then adds
> salt, and then tastes it again, and the second taste is the one that decides
> whether the shelter has a good night or only a night.

> At the long table, Pim counts the bowls because counting bowls is a job a child can
> do, and the count is right, and she puts the extra bowl at the empty place because
> the shelter started doing that two winters ago and never stopped.

> In the cellar, Berek turns a jar to check the seal, and the seal holds, and he says
> nothing because nobody is there to hear it.

---

## 34. APPENDIX U — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Spoilage | stock loss | preserve earlier, cool |
| Deficiency | illness | missing category, time |
| Monotony | morale, health | menu change, trade |
| Ration theft | trust loss | reform, hearing |
| Preservation failure | batch lost | redo, method fix |
| Cellar failure | large loss | repair, redistribute |
| Hungry phase | desperation | aid, harvest, hunt |
| Famine | deaths, crisis | aid, trade, emergency |
| Policy collapse | unrest | rewrite, enforce |
| Feast overspend | winter risk | ration, trade |

No failure is a game over. Every failure has a recovery path, and every recovery
costs food, time, or trust. The deepest failure is a shelter that stops caring what
the food tastes like.

---

## 35. APPENDIX V — CONTENT REVIEW CHECKLIST

- [ ] `KitchenNutritionSystem` remains the kitchen authority.
- [ ] `NutritionDiversitySystem` remains the coverage authority.
- [ ] `FoodPreservationSystem` remains the spoilage authority.
- [ ] Ration fairness stays in `RationConflictSystem`.
- [ ] All nutrition consequences route through `NeedsSystem.Modify`.
- [ ] No hidden nutrition or monotony bar exists.
- [ ] Preservation methods have real inputs and failure.
- [ ] Deficiencies are practical, not horror.
- [ ] Feasts consume real stores.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses live seeded paths only.

---

## 36. APPENDIX W — GLOSSARY

- **Menu** — an authored meal plan with slots, categories, and variety.
- **Repetition** — tracked monotony from the same foods.
- **Coverage** — category count in the 14-day window.
- **Deficiency** — an authored condition from missing categories.
- **Method** — smoke, salt, can, dry, ferment, cellar, or ice.
- **Cohort** — a batch of food with freshness.
- **Policy** — the written ration rule.
- **Phase** — an authored hunger stage with warnings.
- **Tradition** — a recurring food ritual with a chronicle line.
- **Memory** — a dish tied to a person or era.

---

## 37. APPENDIX X — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `KitchenNutritionSystem` | pantry | prep, servings | nutrition state |
| `NutritionDiversitySystem` | profiles | log | needs |
| `FoodPreservationSystem` | cohorts | freshness | inventory |
| `ResourceRationingSystem` | stock | policy | fairness |
| `RationConflictSystem` | fairness | resentment | needs |
| `DesperationSystem` | scarcity | desperation | stock |
| `NeedsSystem` | consequences | morale | — |
| `MedicalPipelineCoordinator` | deficiencies | treatment | — |
| `SkillProgressionSystem` | skills | cook xp | — |
| `CultureCreationSystem` | traditions | culture | — |
| `MemorialSystem` | deaths | markers | — |
| `Inventory` | food | transfers | — |
| `PowerGridSystem` | cold | — | — |
| `TradingSystem` | food | trade | — |
| `EpilogueChronicleBuilder` | milestones | chronicle | — |

---

## 38. APPENDIX Y — DATA SCHEMA DETAIL (NEW CATALOGS)

**`menus.json`** — `menu_id`, `display_name`, `meal_slots`, `required_categories[]`,
`optional_categories[]`, `servings_per_day`, `variety_target`, `morale_baseline`,
`repetition_penalty`, `tags`.

**`deficiency_profiles.json`** — `deficiency_id`, `display_name`, `missing_category`,
`onset_days`, `symptoms[]`, `diagnosis_skill`, `treatment[]`, `recovery_days`,
`severity_bands[]`, `tags`.

**`preservation_methods.json`** — `method_id`, `display_name`, `inputs[]`,
`capacity`, `hours`, `skill`, `fuel_or_power`, `quality_bands[]`, `failure_mode`,
`output_tiers[]`, `tags`.

**`ration_policies.json`** — `policy_id`, `display_name`, `portions`, `priority_rule`,
`enforcement`, `special_allowances[]`, `social_effects[]`, `tags`.

**`hunger_phases.json`** — `phase_id`, `display_name`, `trigger`, `portions`,
`morale_effect`, `desperation_risk`, `recovery_condition`, `tags`.

**`food_traditions.json`** — `tradition_id`, `display_name`, `trigger`, `menu_id`,
`participants`, `morale_effect`, `chronicle_line`, `tags`.

**`food_memories.json`** — `memory_id`, `display_name`, `dish`, `person_id`,
`era`, `effect[]`, `tags`.

**`kitchen_roles.json`** — `role_id`, `display_name`, `required_skill`,
`duties[]`, `fatigue`, `risk`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on missing or
duplicate IDs, invalid item/menu references, or out-of-range numbers.

---

## 39. APPENDIX Z — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Category coverage average | nutrition health | NutritionDiversitySystem |
| Deficiency cases | consequence load | DeficiencySystem |
| Spoilage per week | preservation quality | FoodPreservationSystem |
| Preservation batches | future security | PreservationMethodSystem |
| Ration fairness score | social pressure | RationConflictSystem |
| Theft and confrontation | policy stress | RationConflictSystem |
| Hunger phase | scarcity | HungerPhaseSystem |
| Feast frequency | culture | FoodCultureSystem |
| Menu variety | monotony | MenuSystem |

Telemetry is diagnostic only; it never gates content and never becomes a hidden
score. It exists so the team can tell whether the table feels like survival or like
bookkeeping.

---

## 40. APPENDIX AA — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] All nutrition consequences route through `NeedsSystem.Modify`.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the review checklist in §35.
- [ ] Phase 7 soak shows harvest, winter, deficiency, and recovery over a year.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No parallel kitchen, nutrition, preservation, or ration system exists.

---

## 41. APPENDIX AB — OPEN QUESTIONS FOR REVIEW

1. Should monotony be visible as a morale modifier or only as a deficiency risk?
2. Should a feast be cancellable after stores are committed?
3. Should ration theft ever be a capital matter? (Recommended: no.)
4. Should deficiency symptoms be hidden until diagnosed?
5. Should food memories be player-selectable as menu requests?
6. Should preservation skill be per-survivor or per-kitchen?
7. Should the cellar require continuous power or just a cool room?
8. Should traditions decay if not practised?

None of these may be decided unilaterally; each changes the expansion's balance and
tone.

---

## 42. APPENDIX AC — CLOSING VIGNETTE

> The menu slate by the kitchen door says STAPLE, GREENS, FISH, and someone has
> rubbed out FISH and written EGGS, and Ottilie leaves it, because eggs are better
> and the person who wrote it was right.

> At the long table the bowls are counted, the extra bowl is set at the empty place,
> and the shelter eats together, which is the one thing that has not changed in any
> of the winters so far.

> In the cellar, the jars are lined up with the newest at the back, and the newest
> at the back means the oldest gets eaten first, and that small rule is the whole
> difference between a full cellar in spring and an empty one in February.

---

## 44. APPENDIX AD — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_table_menu_write` | 3 | Draft, cost, post |
| `quest_table_recipe_variety` | 4 | Find, test, teach, serve |
| `quest_table_cook_skill` | 4 | Train, practice, assess, certify |
| `quest_table_signature` | 5 | Choose, perfect, name, share |
| `quest_table_pantry_audit` | 3 | Count, date, rotate |
| `quest_table_diversity_log` | 3 | Record, review, plan |
| `quest_table_category_gap` | 4 | Identify, source, cook, verify |
| `quest_table_deficiency_recover` | 5 | Diagnose, treat, feed, recover |
| `quest_table_child_nutrition` | 3 | Assess, plan, serve |
| `quest_table_work_diet` | 3 | Assess load, plan, serve |
| `quest_table_smoke_batch` | 4 | Prep, smoke, hang, store |
| `quest_table_salt_batch` | 4 | Prep, salt, dry, store |
| `quest_table_can_batch` | 5 | Prep, pack, seal, test |
| `quest_table_dry_batch` | 4 | Prep, dry, turn, store |
| `quest_table_ferment_batch` | 4 | Pack, culture, watch, jar |
| `quest_table_cellar_cool` | 4 | Dig, vent, insulate, stock |
| `quest_table_spoilage_salvage` | 3 | Sort, reprocess, discard |
| `quest_table_ice_harvest` | 4 | Cut, haul, pack, cover |
| `quest_table_pest_control` | 3 | Find, trap, seal, clean |
| `quest_table_rotation` | 3 | Mark, order, enforce |
| `quest_table_policy_write` | 4 | Draft, debate, post, enforce |
| `quest_table_fairness_hearing` | 3 | Listen, weigh, answer |
| `quest_table_scale_calibrate` | 3 | Check, adjust, certify |
| `quest_table_special_need` | 3 | Assess, approve, deliver |
| `quest_table_theft_case` | 4 | Investigate, judge, repair |
| `quest_table_tradition` | 4 | Choose, cook, repeat, declare |
| `quest_table_feast_plan` | 4 | Menu, stores, invite, execute |
| `quest_table_food_memory` | 3 | Interview, cook, record |
| `quest_table_shared_table` | 3 | Arrange, schedule, hold |
| `quest_table_hunger_marker` | 3 | Choose, carve, place |

---

## 45. APPENDIX AE — FOOD TRADE AND MARKET MODEL

| Good | Source | Price band | Spoilage | Demand |
|---|---|---|---|---|
| Salt | salt pans | low | none | very high |
| Spice | spice walk | high | low | high |
| Grain | fields | low | slow | very high |
| Preserved meat | smokehouse | med | very slow | high |
| Fresh greens | greenhouse | med | fast | high |
| Fruit | orchard | high | fast | high |
| Honey | hives | high | none | med |
| Milk | herd | med | fast | high |
| Cheese | cellar | med | slow | high |
| Eggs | coops | med | med | high |
| Fish | river | low | fast | high |
| Jar goods | cannery | med | very slow | high |

Food trade is where the shelter's table meets the region. A shelter that produces a
surplus can trade for variety, and a shelter that trades away its seed grain has made
a choice it will remember in spring.

---

## 46. APPENDIX AF — KITCHEN WORK MODE L

| Task | Staff hours | Inputs | Output | Fatigue |
|---|---|---|---|---|
| Porridge | 2 | grain, water | 40 portions | low |
| Bread | 6 | flour, yeast | 60 loaves | high |
| Stew | 4 | mixed | 60 portions | med |
| Smoke batch | 12 | meat, wood | 40 kg | high |
| Salt batch | 24 | meat, salt | 60 kg | low |
| Can batch | 6 | food, jars | 30 jars | high |
| Dry batch | 20 | food | 50 kg | low |
| Ferment | 96 | veg, culture | 40 kg | low |
| Serve | 2 | portions | meals | med |
| Clean | 2 | water, soap | hygiene | med |

The kitchen is a real workplace with real hours. The expansion's intent is that
a shelter plans its cooking the way it plans its shifts, because both are labor.

---

## 47. APPENDIX AG — MEAL SERVICE AND FAIRNESS MODEL

| Stage | Action | Failure |
|---|---|---|
| Count | count eaters | miscount, shortage |
| Portion | weigh or scoop | unfairness |
| Serve | queue or table | resentment |
| Special | medical, child | dispute |
| Seconds | remainder | favoritism |
| Clean | wash up | hygiene risk |
| Log | record | lost data |

Fairness is felt at the scoop. The live `RationConflictSystem` tracks perceived
fairness and resentment; the expansion makes the moments that feed those numbers
visible, which is why a steward with a scale matters more than a slogan.

---

## 48. APPENDIX AH — REGIONAL FOOD MAP

| Settlement | Surplus | Deficit | Trade |
|---|---|---|---|
| The shelter | preserved goods | fruit, spice | sells jars |
| Market Town | variety | preservation | sells spice |
| Fog Ridge Camp | fish | grain | sells fish |
| Spring Village | greens | meat | sells greens |
| Foundry Enclave | tools | food | buys food |
| Deep Bunker | stores | fresh | trades preserved |
| River Flotilla | fish, transport | grain | moves food |
| Coal Stage | fuel | food | barters fuel |

No settlement feeds itself in every category. Trade is how a table becomes varied,
and it is why the rail line and the road both matter to the menu.

---

## 49. APPENDIX AI — LORE: THE CANTEEN
The pre-war world ate in canteens and kitchens, and the shelter inherited its habits.
The fiction:

- **The Canteen** was a municipal food service site; its trays, urns, and menu
  boards are the shelter's kitchen equipment.
- **The Cannery** was a seasonal preservation plant; its jars and lids are the
  shelter's preservation backbone.
- **The Salt Pans** were an industrial salt works; the shelter's curing depends on
  them.
- **The Grain Store** was a co-operative silo; its mill still turns.
- **The Marker** was a memorial to a famine year before the Exchange, and the
  shelter added its own hungry year to it.

No real company, agency, or cuisine is copied. The Canteen is fictional and exists to
explain why the wasteland's food knowledge is partially recoverable.

---

## 50. APPENDIX AJ — WORKED FOOD ECONOMY

| Month | Production | Consumption | Preserved | Coverage |
|---|---|---|---|---|
| Spring | low | normal | 10% | 3 |
| Summer | high | normal | 30% | 5 |
| Autumn | peak | normal | 45% | 6 |
| Winter | none | normal | drawn down | 3 |
| Hungry | none | cut | drawn down | 2 |

The intent of the economy is that autumn is the whole year. A shelter that preserves
in autumn eats in winter; a shelter that feasts in autumn and preserves nothing eats
what it saved. The expansion makes this arithmetic visible and personal.

---

## 51. APPENDIX AK — MEASUREMENT AND REVIEW CADENCE

| Gate | Question | Evidence |
|---|---|---|
| Premise | Do the live owners still match? | file:line audit |
| Data | Are all rows valid and reachable? | integrity + scanner |
| Core | Are the systems pure and deterministic? | unit tests |
| Persistence | Does legacy load neutral? | round-trip tests |
| Host | Is every command real? | selftest + journey |
| UI | Is nutrition honest? | lifecycle + a11y tests |
| Tone | Is deprivation dignified? | content review |
| Balance | Is the table tense and warm? | long soak |

This expansion is where the game is most intimate, and the tone gate is as important
as the balance gate. A hungry shelter should feel real, never like a punishment.

---

## 53. APPENDIX AL — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Hidden nutrition bar | removes agency | visible symptoms and log |
| Monotony as pure debuff | punishes without teaching | deficiency risk and morale |
| Preservation as free food | breaks scarcity | inputs, capacity, failure |
| Ration policy without faces | feels mechanical | hearings and resentment |
| Famine as a wipe | removes choice | phased warnings and off-ramps |
| Feast as pure bonus | undermines tension | real store cost |
| Cook skill as a number | removes personhood | specialties and signature dishes |
| Food culture as decoration | feels hollow | traditions with chronicle lines |
| Spoilage as invisible | unfair | warnings before loss |
| Medical effects from recipes | bypasses pipeline | route through treatment |

The list exists because this expansion is easy to get subtly wrong: it can become a
second economy, a hidden debuff system, or a misery simulator. The live authority
splits exist to prevent exactly that, and every implementation phase must honor them.

---

## 54. APPENDIX AM — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Menus | 15 | 4,000 |
| Recipes | 40 | 8,000 |
| Deficiencies | 12 | 3,500 |
| Preservation methods | 10 | 3,000 |
| Ration policies | 10 | 3,000 |
| Hunger phases | 8 | 2,500 |
| Traditions | 15 | 3,500 |
| Memories | 20 | 3,500 |
| Kitchen roles | 6 | 1,500 |
| Nutrition profiles | 30 | 4,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 14 | 2,000 |
| Endings | 6 | 3,000 |
| **Total** | | **~66,500** |

This budget keeps the expansion at the target while leaving room for the prose that
makes a meal feel like an event rather than a transaction.

---

## 55. APPENDIX AN — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | Children's rations, growth nutrition, first solid food |
| 1 | 13 The Faithful | Fasting, feast days, food offerings, harvest rites |
| 1 | 14 Above the Ash | Airlift food, airdrop rations, flight rations |
| 1 | 15 The Deep Root | Production inputs; the table consumes what the fields grow |
| 1 | 16 The Rebuilt Body | Recovery diets, textured food, appetite with implants |
| 2 | 17 The Long Evening | Feasts, communal meals, food as performance |
| 2 | 18 The Underneath | Cellar geology, cave fungus food, deep storage |
| 2 | 19 The Bitter Air | Quarantine rations, safe food handling, outbreak diets |
| 2 | 20 The Quiet Hand | Poison, theft, ration informants |
| 2 | 21 The Grid | Refrigeration, canning power, cold chain |
| 3 | 22 The Clean Flow | Wash water, kitchen hygiene, food safety |
| 3 | 23 The Alarm | Kitchen fire, food store collapse |
| 3 | 24 The Long Goodbye | Last meals, comfort food, terminal appetite |
| 3 | 25 The Iron Road | Grain freight, food towns, market schedules |

Each hook is additive. The Common Table can ship alone, and every other expansion can
ship without it.

---

## 56. CLOSING STATEMENT

ASHFALL already owns food production, kitchen prep, nutrition diversity, preservation
cohorts, rationing, and resentment with real systems and honest authority splits.
What it lacks is the table: a menu, a cook with a specialty, a cellar that rotates, a
smokehouse, a canning run, a deficiency noticed before it is a crisis, a ration policy
everyone argues about, and a shared meal that marks what the shelter has survived. The
Common Table adds that world without adding a second kitchen or a hidden nutrition
bar. It adds a full pot, a fair scale, a remembered dish, and the daily proof that the
shelter is still a place where people eat together.

> Wave 3 note: this plan is one of five Wave 3 expansion bibles (22–26). Each is
> self-contained; none requires another to ship. The shared Wave 3 index lives at
> `docs/expansions/wave3/WAVE3_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible. Evidence anchors
> used throughout: `KitchenNutritionSystem`, `NutritionDiversitySystem`,
> `FoodPreservationSystem`, `ResourceRationingSystem`, `RationConflictSystem`,
> `DesperationSystem`, the fermentation engines, and the live kitchen, preservation,
> and ration save sections.

---
