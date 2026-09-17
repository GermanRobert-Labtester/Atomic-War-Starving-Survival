---
PLAN_ID: E1-4
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 4
STATUS: READY_FOR_EXECUTION_WHEN_RAILS_PASS
SOURCE_PLAN: "Plan 136 — Wildlife Trapping → Food Pipeline & Cooking System"
SEQUENCE_FILENAME: "E1_planintegration[4].md"
PREVIOUS_FILENAME: "E1_planintegration[3].md"
NEXT_FILENAMES:
  - "E1_planintegration[5].md"
  - "E1_planintegration[6].md"
CATEGORY: LINK+FOOD_PIPELINE+PRODUCTION+PRESENTATION
PRIMARY_INTENT: "Close the trapping dead-end by integrating catch yields into canonical inventory/needs authorities, then add the smallest justified cooking layer."
EXECUTION_STYLE: "Flagship integration plan"
PREMISE_VERIFICATION_REQUIRED: true
INTAKE_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
TRAPPING_OUTPUT_MUST_REACH_INVENTORY: true
SECOND_INVENTORY_FORBIDDEN: true
SECOND_NEEDS_SYSTEM_FORBIDDEN: true
RUNTIME_RISK: MEDIUM_HIGH
SAVE_RISK: MEDIUM_HIGH
BALANCE_RISK: HIGH
CONTENT_RISK: MEDIUM
---

# E1 Plan Integration [4] — Wildlife Trapping, Inventory Delivery, Food Safety, Cooking, Nutrition, and Preservation Rails

> **Sequence rule:** this file is `E1_planintegration[4].md`.
> The next files are `E1_planintegration[5].md`, `E1_planintegration[6].md`, and so on.
> The bracketed sequence number stays immediately before `.md`.

## 0. Mission

This plan converts Plan 136 into an implementation-grade food-pipeline integration programme.

The first and highest-priority defect is not “ASHFALL lacks cooking.” It is that trapping is described as a
working gameplay system whose butchery/catch outputs do not reach the player-facing inventory/consumption
pipeline. A trapped animal that remains inside `TrapSite` state is authored breadth without consumption.
Therefore the first shippable objective is to make the existing trapping loop terminate in canonical item
ownership, nutrition, contamination, and player choice.

Only after that vertical slice is proven should a cooking layer be added. Cooking must not become a second
inventory, a second hunger system, a second radiation system, a second crafting scheduler, or a second skill
framework. It should be a transformation rail: authoritative ingredients enter, equipment/fuel/time rules are
resolved through existing systems where possible, canonical output items are produced, contamination or safety
metadata is transformed according to validated rules, and canonical needs/morale/health systems consume the
result.

The source plan proposes fixed radiation-removal percentages, cooking skill progression, equipment tiers,
spoilage, cooking events, quests, and fifteen recipes. Those are **design hypotheses**, not implementation
truth. E1-4 retains them as candidates but requires repository verification, domain ownership, balance review,
and evidence before they become permanent rules.

## 1. Source Evidence Preserved

The source plan establishes a concrete dead-end:

- `WildlifeTrappingSystem.cs` contains trapping/bait/quarry/butchery/hide behavior.
- Trapping output is not described as reaching `InventorySystem`.
- `KitchenNutritionSystem.cs` exists.
- `NeedsSystem` already owns hunger restoration.
- Item data already exists.
- The proposed cooking layer would transform raw game into consumable meals.
- The source proposes fuel, equipment, skill, contamination/radiation reduction, recipe data, spoilage,
  morale effects, UI, save/load, deterministic behavior, and CI selftests.

The flagship plan treats those as a dependency map rather than justification for one monolithic
`CookingSystem`.

## 2. Core Architecture Thesis

A complete food loop is:

```text
wildlife ecology/trapping
    -> catch resolution
    -> butchery yield
    -> canonical inventory ownership
    -> contamination/safety metadata
    -> optional preparation/cooking/preservation
    -> canonical inventory outputs
    -> canonical consumption
    -> hunger/nutrition/health/morale consequences
    -> waste/spoilage/byproducts
```

Each arrow is an integration seam. Each node must have one authority.

The new code should live at seams only where no existing generic production/crafting/kitchen authority already
owns the transformation.

## 3. Non-Negotiable Rules

- No catch is considered player-owned until canonical inventory accepts it.
- Trapping does not mutate inventory directly if the repository already has a host/service transaction boundary.
- Inventory transfer must be atomic or explicitly recoverable.
- Butchery yields come from canonical quarry/item data, not UI or recipe code.
- Contamination/radiation belongs to the canonical contamination/health/item-state authority.
- Cooking may transform contamination metadata only through a defined domain contract.
- No claim that ordinary cooking removes a specific percentage of radioactive contamination may ship without
  design and domain validation.
- Nutrition/hunger restoration belongs to canonical needs/nutrition systems.
- Cooking recipes define transformations; they do not directly heal hunger or morale.
- Fuel belongs to canonical inventory/power/resource systems.
- Equipment belongs to canonical shelter/workstation/facility authority.
- Skill belongs to an existing survivor skill/proficiency authority where one exists.
- Spoilage belongs to existing item decay/perishability authority where one exists.
- Cooking queue/scheduler must reuse existing production/job/commitment rails if compatible.
- Outputs are granted only on authoritative completion.
- Cancellation cannot duplicate ingredients.
- Save/reload cannot duplicate outputs or consume inputs twice.
- UI never owns ingredient reservation or cooking progress.
- Advanced recipe count is capped until the first five prove useful.
- Trapping → inventory integration can ship independently of the full cooking feature.

## 4. Delivery Strategy

The plan is divided into acceptance slices:

### Slice A — Dead-end repair
Trap → catch → butchery → inventory → raw food consumption.

### Slice B — Minimal preparation
One heat source, one fuel type, five recipes, deterministic output, no bespoke skill tree.

### Slice C — Food safety and item-state integration
Preparation affects canonical contamination/safety state only where validated.

### Slice D — Production depth
Equipment differences, queue behavior, spoilage/preservation, morale/quality.

### Slice E — Content and presentation
Recipe book, fifteen recipes if justified, quests/events, UI refinement.

Do not begin Slice D/E before Slice A/B are accepted in runtime evidence.


---

## E1-4A — Premise audit and authority map

**Goal:** Verify every food-pipeline authority at current HEAD and identify exactly which seams are dead.

### Required substeps

1. Inspect `WildlifeTrappingSystem`, all trapping catalog/data files, inventory APIs, item DTOs, item-instance metadata, `KitchenNutritionSystem`, `NeedsSystem`, radiation/contamination/disease systems, crafting/production systems, workstation/equipment systems, survivor skill/proficiency systems, spoilage/perishability logic, morale/mental-health systems, quest/event systems, and save registry.
2. Search for existing cooking, food preparation, boiling, drying, smoking, canning, kitchen, recipe, crafting, or transformation logic before creating new classes.
3. Map ownership for catch state, butchery state, yield tables, inventory, item condition, contamination, nutrition, hunger, health, morale, fuel, equipment, skill, spoilage, recipe knowledge, and production scheduling.
4. Confirm the exact inventory mutation API and whether additions must go through host/session/service transactions.
5. Confirm whether item stacks can carry per-stack contamination/freshness metadata; if not, identify the existing item-instance model or a required prerequisite rail.
6. Verify whether `KitchenNutritionSystem` already has transformation or only consumption-related responsibility.
7. Verify whether food spoilage already exists globally; do not implement cooking-only spoilage if a canonical decay system exists.
8. Verify whether survivor skills already exist and can represent cooking proficiency.
9. Verify whether generic production jobs already support input reservation, duration, equipment, fuel, cancel, and output delivery.
10. Create `docs/systems/FOOD_PIPELINE_AUTHORITY_MAP.md`.
11. Create an intake duplicate-search receipt.
12. Record `PREMISE_VERIFIED_AT` and block every proposed field/mechanic with no valid authority.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4B — Catch-to-yield contract

**Goal:** Define exactly how a resolved trap catch becomes deterministic butchery yields without inventing inventory semantics inside trapping.

### Required substeps

1. Identify the canonical quarry/species record and all yield fields currently tracked by trapping.
2. Define a pure `ButcheryYield`/equivalent contract containing canonical item IDs and quantities plus item-state metadata references.
3. Separate catch resolution from butchery completion if the existing system already does so.
4. Define yield determinism from species, mass/quality, trap condition, butchery method, survivor skill if already canonical, and seeded RNG where appropriate.
5. Define hides, meat, organs, bones, fat, or other byproducts only if actual item catalogs support them.
6. Reject unknown output item IDs at data-integrity time.
7. Define contamination acquisition from environment/species through canonical contamination authority rather than arbitrary cooking code.
8. Define rounding rules and minimum/maximum yield.
9. Define failed/partial butchery behavior.
10. Add tests for common quarry, zero yield, multiple outputs, contaminated catch, and deterministic yield.
11. Ensure a single catch can reach butchery completion exactly once.
12. Create an event/transaction payload suitable for the inventory bridge.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4C — Atomic trapping → inventory delivery bridge

**Goal:** Close the highest-value dead end by transferring completed butchery yields into canonical inventory exactly once.

### Required substeps

1. Choose the host/service boundary that is allowed to mutate both trapping lifecycle state and inventory transaction state.
2. Do not make `WildlifeTrappingSystem` depend directly on a concrete inventory implementation if existing architecture uses ports/delegates/events.
3. Define `ButcheryCompleted` as an authoritative event or completion result carrying a stable operation ID.
4. Use inventory transaction APIs to add all accepted outputs.
5. Define destination storage when main inventory has multiple containers/sites.
6. Handle capacity failure explicitly: reject completion, leave pickup bundle, stage output, or use canonical overflow semantics—never delete yields silently.
7. Make delivery idempotent by operation ID.
8. Record transfer completion in save state if needed to prevent replay after reload.
9. Ensure hides/byproducts follow the same transaction rules.
10. Add tests for success, partial capacity, zero capacity, duplicate event, save/reload before delivery, save/reload after delivery, and inventory rejection.
11. Verify inventory totals before/after exactly equal accepted yield.
12. Ship this bridge before cooking work and measure whether trapping now creates usable food.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4D — Raw food item-state contract

**Goal:** Represent raw game as canonical inventory items with explicit nutrition, safety, contamination, freshness, and preparation eligibility.

### Required substeps

1. Inspect item schemas for nutrition, category, tags, contamination, condition, freshness, stackability, and affliction hooks.
2. Prefer catalog-backed static properties plus item-instance dynamic properties rather than one-off trapping DTO fields.
3. Define raw/cooked/prepared state as recipe output item identity or canonical item-state tag according to existing item architecture.
4. Define whether contamination is per item type, per stack, per instance, or external ledger reference.
5. Define raw consumption availability and consequences through canonical consumption rules.
6. Do not encode 'raw = low nutrition' if existing nutrition data says otherwise; balance values belong in data authority.
7. Define food-safety risk separately from radiation/contamination where disease systems distinguish them.
8. Define poisonous/toxic/inedible quarry tags.
9. Add validation that trapping output items are valid food/crafting items.
10. Add tests for raw edible, raw unsafe, contaminated, non-food hide, and stack merge rules with differing metadata.
11. Document how item-state survives transfer, split, merge, and save/load.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4E — Canonical consumption integration

**Goal:** Make raw and prepared trapping outputs affect hunger/nutrition through the same consumption authority as every other food.

### Required substeps

1. Identify the one API responsible for consuming an inventory item and applying needs effects.
2. Ensure trapping-origin food requires no special-case consumption path.
3. Map nutrition/hunger values from item/recipe data into `KitchenNutritionSystem`/`NeedsSystem` through existing interfaces.
4. Ensure inventory removal and needs restoration are one atomic or recoverable transaction.
5. Route food-safety/radiation consequences through canonical health/contamination systems.
6. Define morale/taste effects through existing morale authority if supported.
7. Add tests for eating raw safe meat, raw risky meat, contaminated meat, prepared meal, and non-food item rejection.
8. Add save/reload test around consumption to prevent duplicate restoration.
9. Measure hunger restored per unit of trapping effort for balancing.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4F — Cooking ownership ADR and minimal system boundary

**Goal:** Decide whether a new CookingSystem is necessary or whether generic production/kitchen rails should be extended.

### Required substeps

1. Write an ADR comparing three options: new `CookingSystem`, extension of generic production/crafting, or a thin cooking facade over existing production jobs.
2. Score each option on state ownership, input reservation, scheduler reuse, equipment/fuel reuse, save complexity, UI compatibility, testing, and duplication risk.
3. Default to the smallest new durable authority.
4. Define what the cooking layer owns: recipe selection, job identity, preparation-specific outcome policy, and recipe knowledge only if no existing owners exist.
5. Explicitly exclude inventory, hunger, contamination truth, fuel quantities, equipment condition, survivor core stats, and morale state.
6. Define the first-slice scope as five recipes and one or two equipment paths.
7. Define a rollback path that disables new cooking jobs without corrupting ingredient/output state.
8. Have a second tool review the ADR against the authority map before implementation.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4G — Recipe schema and transformation contract

**Goal:** Define recipes as data-driven transformations with explicit authoritative inputs, outputs, equipment, time, and optional safety effects.

### Required substeps

1. Define recipe ID, input item requirements, output items, duration, equipment tags, fuel requirement, batch size, skill requirement if canonical, quality policy, contamination transformation policy, spoilage/freshness policy, and localization keys.
2. Support multiple inputs and multiple outputs/byproducts.
3. Use item IDs that resolve against the canonical item catalog.
4. Use equipment tags that resolve against canonical facility/workstation definitions.
5. Use fuel item/resource IDs that resolve through canonical inventory/resource systems.
6. Separate nutrition values from recipe mechanics if output item definitions already own nutrition.
7. Separate contamination transformation from recipe text; use a typed policy with validated bounds.
8. Define cancellation/refund policy metadata only if production rails support it.
9. Define recipe unlock/source metadata through existing knowledge/skill/journal authority.
10. Create schema versioning and catalog loader only if existing generic recipe loader cannot be reused.
11. Add integrity validation for duplicate IDs, missing items, invalid equipment, invalid fuel, negative durations, impossible yields, and invalid contamination policies.
12. Author five first-slice recipes before expanding to fifteen.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4H — Ingredient reservation and transactional cooking start

**Goal:** Ensure starting a cook cannot duplicate, lose, or double-reserve ingredients.

### Required substeps

1. Use canonical inventory reservation/transaction APIs where available.
2. Validate recipe knowledge, equipment availability, equipment condition, fuel, ingredients, worker eligibility, and destination capacity before commitment.
3. Reserve or consume inputs according to the existing production convention.
4. Assign a stable cooking operation ID.
5. Record exact input stacks/quantities if metadata matters.
6. Prevent the same ingredients from backing two active jobs.
7. Define how stack splits preserve contamination/freshness metadata.
8. Define cancellation before and after partial progress.
9. Define what happens when destination capacity becomes unavailable before completion.
10. Add tests for valid start, no fuel, no equipment, missing ingredient, duplicate reservation, metadata-bearing stack split, and concurrent starts.
11. Make start deterministic and idempotent where external calls can retry.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4I — Time progression, worker assignment, and deterministic completion

**Goal:** Run cooking through canonical time/job scheduling rather than UI timers.

### Required substeps

1. Use existing day/minute/segment scheduler semantics.
2. Define whether a survivor must be actively assigned or only initiates the job.
3. Use canonical duty/fitness rules if worker time is consumed.
4. Use equipment availability/occupancy authority to prevent impossible parallel use.
5. Use canonical fuel consumption timing.
6. Define interruption behavior for power loss, evacuation, injury, worker reassignment, or equipment destruction.
7. Define deterministic success/failure outcomes using seeded RNG only if the design truly needs stochastic cooking.
8. Prefer deterministic recipe completion over random failure in the first slice unless failure creates clear player value.
9. Emit completion/failure/cancellation events.
10. Add tests for time advance, pause/interruption, save/reload mid-job, deterministic completion, and concurrent equipment use.
11. Ensure headless and UI-driven runs produce identical state.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4J — Equipment integration and capability tags

**Goal:** Use real shelter/workstation equipment to constrain recipes without inventing a parallel equipment hierarchy.

### Required substeps

1. Audit current stove, boiler, fire, oven, heater, workshop, kitchen, and power/fuel facilities.
2. Map proposed improvised stove, boiler, oven, and industrial cooker to existing entities or explicitly approved new equipment content.
3. Prefer capability tags such as `heat`, `boil`, `bake`, `bulk_cook` over hard-coded class checks.
4. Define equipment throughput, occupancy, condition, and power/fuel dependencies through canonical owners.
5. Do not duplicate upgrade progression if shelter construction already owns upgrades.
6. Define recipe compatibility with capability requirements.
7. Add tests for missing capability, damaged equipment, unpowered equipment, fuel-required equipment, and compatible alternatives.
8. Start with existing equipment where possible; defer industrial tiers until the food loop is accepted.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4K — Fuel and energy accounting

**Goal:** Make food preparation consume real resources through canonical fuel/power systems.

### Required substeps

1. Identify canonical fuel items/resources and energy authority.
2. Define per-recipe or per-duration fuel demand using data, not UI.
3. Support fuel alternatives only if resource systems already model them.
4. Reserve/consume fuel transactionally with the cooking operation.
5. Define behavior on fuel exhaustion mid-job.
6. Prevent zero-cost cancellation/restart exploits.
7. Add tests for exact fuel cost, insufficient fuel, alternative fuel, interruption, cancellation, and save/reload.
8. Measure fuel-to-nutrition tradeoff for balance.
9. Do not introduce a cooking-only fuel balance.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4L — Food safety and contamination transformation

**Goal:** Integrate preparation with canonical contamination/disease rules while avoiding unsupported claims that heat simply deletes radiation.

### Required substeps

1. Audit how radiation contamination, radionuclide exposure, biological contamination, disease risk, toxins, and item contamination are represented.
2. Separate biological pathogen reduction from radioactive contamination handling.
3. Do not ship fixed 50/80/90% 'radiation removal' values as physical truth without game-design justification and explicit data semantics.
4. If the game intentionally abstracts decontamination, name the mechanic accurately as item contamination reduction and document it as an abstraction.
5. Allow boiling/leaching or trimming to transfer contamination into discarded liquid/waste only if the item/waste model supports it.
6. Define preparation policies per contaminant class.
7. Ensure preparation cannot make intrinsically toxic/poisonous food safe unless explicitly supported.
8. Route foodborne illness risk through canonical disease/affliction system.
9. Add tests for biological contamination reduction, radioactive contamination policy, toxic meat, no-treatment recipe, and invalid policy.
10. Require data-driven bounded transformations and validation.
11. Document gameplay abstraction separately from real-world scientific claims.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4M — Nutrition quality and hunger restoration

**Goal:** Make prepared outputs nutritionally distinct through canonical item/nutrition data, not through hidden cooking multipliers.

### Required substeps

1. Determine whether nutrition belongs to output item definitions, recipe quality, portion size, or `KitchenNutritionSystem`.
2. Prefer output items with explicit nutrition profiles.
3. Define calorie/hunger restoration, protein or micronutrient abstractions only to the level already used by the game.
4. Do not multiply nutrition by survivor cooking skill unless the nutrition authority explicitly supports quality modifiers.
5. Use quality/morale/taste as separate dimensions from caloric value.
6. Define burned/overcooked outputs as real item variants if they need different consumption effects.
7. Add tests comparing raw, cooked, preserved, burned, and contaminated variants.
8. Balance trapping yields against other food sources to avoid making trapping dominant.
9. Measure net hunger restored per labour-hour and fuel unit.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4N — Cooking proficiency integration

**Goal:** Use an existing skill/proficiency framework if present and keep progression bounded.

### Required substeps

1. Audit survivor skills, professions, traits, XP, and recipe knowledge.
2. Do not create a cooking-specific skill system if a generic proficiency authority exists.
3. Define what cooking proficiency may affect: throughput, failure chance, fuel efficiency, quality, or recipe access.
4. Do not let one skill simultaneously reduce time, increase nutrition, remove more contamination, and improve morale without balance evidence.
5. Define XP award on authoritative job completion only.
6. Prevent XP farming through trivial zero-cost recipes.
7. Cap progression and make thresholds data-driven.
8. Add tests for XP award, unlock, cap, no award on cancel, no duplicate award after reload, and deterministic quality effect.
9. Allow first slice to ship with no skill progression if the base loop is clearer without it.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4O — Recipe knowledge and discovery

**Goal:** Connect recipe availability to canonical knowledge/journal/skill systems instead of storing a second discovery ledger.

### Required substeps

1. Identify existing recipe/codex/journal/unlock authority.
2. Represent known recipes there or provide a thin adapter.
3. Define starter recipes, learned recipes, quest-learned recipes, and experimental discovery only if supported.
4. Ensure discovering a recipe does not directly create ingredients/equipment.
5. Use stable recipe IDs for save state.
6. Add tests for unknown recipe, learned recipe, old-save defaults, duplicate discovery, and save/load.
7. Keep experimentation/random discovery as follow-on if no established discovery rail exists.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4P — Spoilage, freshness, and preservation integration

**Goal:** Make cooked and preserved foods participate in one global perishability model.

### Required substeps

1. Audit existing spoilage, item age, storage temperature, refrigeration, preservation, and decay systems.
2. Extend the canonical system rather than adding cooking-only spoil timers.
3. Define shelf-life metadata on output items or preservation classes.
4. Define how cooking resets, transforms, or preserves freshness.
5. Define dried/jerky outputs using canonical preservation tags.
6. Define spoiled-food consumption consequences through disease/affliction authority.
7. Add tests for raw spoilage, cooked spoilage, dried preservation, refrigerated storage if supported, save/reload age, and stack merge rules.
8. Ensure item age advances in headless mode.
9. Keep 3–7 day and 30-day numbers as tunable data rather than code constants.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4Q — Cooking quality, burnt outputs, and morale

**Goal:** Add quality outcomes only after the deterministic transformation loop is stable.

### Required substeps

1. Define quality tiers only if they create meaningful choices.
2. Prefer quality driven by recipe, equipment, worker proficiency, interruptions, and timing—not arbitrary critical-hit randomness.
3. Represent burnt/failed food as canonical output item or quality state.
4. Route morale/taste consequences through canonical morale/mental-health APIs.
5. Define excellent meal bonuses with caps and cooldowns.
6. Prevent morale farming from repeated cheap meals.
7. Add tests for normal, excellent, burnt, interrupted, morale cooldown, and save/reload.
8. Keep critical-success events disabled in the first slice unless evidence shows they improve play.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4R — Cooking UI authority contract

**Goal:** Create or extend a kitchen/cooking surface whose controls are projections of canonical job and inventory state.

### Required substeps

1. Audit existing kitchen, production, inventory, and workstation panels.
2. Reuse a production/kitchen panel if possible.
3. Build a read model for known recipes, ingredient availability, reserved ingredients, equipment capability, fuel, worker assignment, duration, output, and blocking reason.
4. Every Start action calls the authoritative start command.
5. Every Cancel action calls the authoritative cancel command.
6. UI never directly removes ingredients, adds outputs, advances time, or grants XP.
7. Show contamination/safety transformation using game terminology without presenting unsupported real-world guarantees.
8. Show nutrition, morale, shelf life, and fuel cost from authoritative data.
9. Support keyboard navigation, filtering, recipe book, and queue inspection.
10. Add snapshot tests for ready recipe, missing ingredient, missing fuel, unavailable equipment, active job, completed job, and contaminated input.
11. Add interaction tests for start/cancel/select equipment.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4S — Starter recipe corpus: five first, fifteen only after acceptance

**Goal:** Author a small recipe set that exercises distinct rails rather than maximizing content count.

### Required substeps

1. First slice recipes: one roasted meat, one boiled meat/stew, one preserved/dried meat, one mixed meal, and one high-contamination handling recipe if validated.
2. Ensure each recipe has a distinct decision: fuel, time, equipment, shelf life, safety, morale, or nutrition.
3. Validate all item IDs and equipment/fuel requirements.
4. Use localization-ready names/descriptions.
5. Add recipe-specific tests where transformations are nontrivial.
6. Run player-value review before authoring recipes 6–15.
7. Only expand to fifteen if the first five produce meaningful differentiated choices.
8. Advanced candidates may include herb roast, spiced stew, pie, bulk ration, long-cook detox abstraction, soup, broth, preserved strips, and feast meal where ingredients exist.
9. Do not invent ingredient catalogs solely to satisfy recipe count.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4T — Kitchen events and accidents

**Goal:** Integrate cooking-specific events through existing event/affliction systems with bounded frequency.

### Required substeps

1. Audit event framework and kitchen/work accident handling.
2. Represent kitchen accident as a normal event/affliction consequence.
3. Define eligibility, cooldown, severity, and prevention factors.
4. Route injuries through canonical health authority.
5. Route morale events through canonical morale authority.
6. Do not use random accidents to punish routine cooking excessively.
7. Add tests for cooldown, deterministic seeded selection, injury application, and no duplicate event on reload.
8. Keep event count minimal until the core loop is accepted.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4U — Quest hooks and narrative integration

**Goal:** Expose food preparation as a quest capability without making quests own cooking state.

### Required substeps

1. Define quest predicates such as recipe known, meal prepared, quantity produced, quality reached, contamination reduced, or feast delivered.
2. Quest authority subscribes to cooking completion events or queries canonical state.
3. Implement source-plan quest concepts only if they fit current narrative rails: 'The Last Chef', 'Feast or Famine', 'Toxic Harvest'.
4. Ensure quest rewards use canonical systems.
5. Prevent quest completion from replaying on save/load.
6. Add tests for event-driven progress, duplicate completion guard, cancelled cook, and recipe-unlock reward.
7. Keep quest authoring separate from foundational food-pipeline release if needed.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4V — Old-save migration and unresolved trap catches

**Goal:** Load existing campaigns safely without duplicating historical catches or silently gifting speculative inventory.

### Required substeps

1. Default missing cooking state to empty.
2. Determine whether existing trap state can distinguish an unclaimed completed catch from an already-resolved historical catch.
3. Do not retroactively transfer every historical catch unless the save schema proves it remains genuinely unclaimed.
4. If safe, migrate only explicit pending-butchered/claimable outputs using stable IDs.
5. If ambiguous, leave legacy state untouched and document the compatibility limitation.
6. Version the new transfer marker/operation IDs.
7. Add migration tests for no trapping state, active trap, pending catch, completed but undelivered catch, and ambiguous legacy record.
8. Ensure migration is idempotent.
9. Ensure old saves can continue trapping and receive new post-migration inventory deliveries.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4W — Save contract and job persistence

**Goal:** Persist cooking jobs only at the canonical production/cooking authority boundary.

### Required substeps

1. Register a new save section only if existing production save state cannot represent cooking jobs.
2. Persist operation IDs, recipe ID, reserved input references/quantities, equipment reference, worker reference, progress, deterministic outcome seed/state, and completion/delivery marker as needed.
3. Do not persist inventory copies, needs values, fuel balances, or derived output previews.
4. Define section version and default empty state.
5. Add round-trip tests for queued, active, interrupted, completed-undelivered, cancelled, and failed jobs.
6. Ensure restore cannot re-consume inputs or re-grant outputs.
7. Add checksum/corruption fixtures.
8. Document ownership in save-model docs.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4X — Exploit and conservation audit

**Goal:** Prove the entire food pipeline conserves items/resources and cannot be farmed through retries, cancellation, reload, or stack manipulation.

### Required substeps

1. Write conservation assertions for catch yield → inventory.
2. Write conservation assertions for cooking inputs/fuel → outputs/byproducts.
3. Test cancellation before/after reservation.
4. Test save/reload before completion and after completion.
5. Test double-click/retry of Start.
6. Test duplicate butchery completion event.
7. Test stack split/merge with contamination/freshness metadata.
8. Test output capacity failure.
9. Test XP and morale duplicate-award prevention.
10. Test quest/event duplicate-award prevention.
11. Test zero-cost recipe loops.
12. Create a fuzz/property test over random valid recipes and inventory states if existing test infrastructure supports it.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4Y — Performance and long-run food-pipeline soak

**Goal:** Ensure cooking jobs, spoilage, and trapping delivery remain bounded over a full campaign.

### Required substeps

1. Run a 180-day headless simulation with repeated trapping, butchery, inventory transfer, cooking, spoilage, consumption, and cancellation.
2. Measure active job count, completed history, inventory stack growth, perishable item update cost, save size, allocations, and tick time.
3. Ensure completed cooking jobs are retired under retention policy.
4. Ensure stale transfer markers are bounded or compacted.
5. Ensure spoilage does not scan irrelevant non-food inventory inefficiently.
6. Measure median/p95 cooking tick cost.
7. Add practical regression thresholds.
8. Block recipe/content expansion if job or perishability cost scales with total historical operations.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

## E1-4Z — Release gate, metrics, and closure

**Goal:** Ship a connected food pipeline whose first success criterion is that trapping finally feeds people.

### Required substeps

1. Run .NET build/test and game build.
2. Run data-integrity selftest.
3. Run trapping/food/cooking selftest if introduced.
4. Run inventory conservation suite.
5. Run save/migration suite.
6. Run deterministic job suite.
7. Run contamination/safety transformation tests.
8. Run consumption/needs integration tests.
9. Run UI authority tests.
10. Run 180-day soak.
11. Verify five first-slice recipes before approving fifteen.
12. Capture player-value metrics.
13. Update authority map, docs, plan register, and handoff.
14. Mark DONE only when trap catches reach inventory exactly once, food can be consumed through canonical needs, and cooking adds decisions without duplicating authorities.

### Integration invariants

- Inventory owns item quantities.
- Needs/nutrition owns hunger restoration.
- Health/contamination owns physiological consequences.
- Equipment/workstations own capability and occupancy.
- Fuel/power authorities own energy resources.
- Cooking owns only preparation-specific transformation/job state not already owned elsewhere.
- All transfers and completions are idempotent.
- All random outcomes use deterministic RNG.
- All persistent collections have retention rules.

### Negative tests

- The same catch can be transferred twice.
- Cooking can duplicate ingredients or fuel.
- Cancellation can create net-positive items.
- Reload can grant output twice or restore hunger twice.
- UI can start a job without the authority validating resources.
- A recipe can reference an invalid item/equipment/fuel ID.
- A contaminated stack can merge and silently lose metadata.
- Cooking-specific state can disagree with canonical inventory/needs state.

### Acceptance evidence

- [ ] Targeted tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless/runtime behavior is proven.
- [ ] Conservation invariants pass.
- [ ] Authority map remains correct.
- [ ] Metric or balance evidence is captured.
- [ ] Premise SHA and plan metadata are updated.

---

# 5. Canonical Food-Pipeline Authority Matrix

| Fact | Canonical owner | Cooking/trapping stores? | Rule |
|---|---|---:|---|
| Active trap state | Wildlife trapping | Yes | Existing authority |
| Catch identity | Wildlife trapping | Yes | Stable catch/operation ID |
| Butchery yield definition | Quarry/item data + trapping resolver | Result only | No inventory mutation inside data |
| Item quantity | Inventory | No duplicate | Canonical |
| Item metadata | Item instance/contamination/freshness authority | Reference/transform | Preserve on split/merge |
| Hunger | Needs | No | Consumption command applies effects |
| Nutrition profile | Item/nutrition data | No duplicate | Recipe outputs reference it |
| Radiation/contamination | Canonical contamination/item-state authority | Transform policy only | No second ledger |
| Disease risk | Disease/affliction authority | No | Preparation may reduce risk through policy |
| Fuel quantity | Inventory/resource system | No | Transactional consumption |
| Power | Power grid/facility authority | No | Query capability |
| Equipment | Shelter/workstation authority | Reference only | Capability tags |
| Worker fitness | Survivor/needs/fitness authority | No | Query |
| Skill/proficiency | Survivor skill authority | Reference/query | Do not clone |
| Recipe knowledge | Knowledge/journal/skill authority | Reference/query | One source |
| Cooking progress | Cooking/generic production job | Yes if genuinely unique | Minimal durable state |
| Output delivery | Inventory transaction | Delivery marker only | Idempotent |
| Spoilage | Item decay/perishability | No duplicate | One global clock |
| Morale | Mental-health/morale authority | No | Apply through API |
| Quest progress | Quest authority | No | Completion event only |

Any row whose canonical owner cannot be identified is a blocker.

---

# 6. Catch Delivery Transaction

Recommended logical transaction:

```text
Trap catch resolved
    |
    v
Butchery operation created
    |
    v
Butchery completes -> deterministic yield bundle
    |
    v
Host integration boundary
    |
    +--> validate operation not previously delivered
    +--> validate item IDs/metadata
    +--> inventory transaction
    |       +--> accepted all
    |       +--> accepted canonical overflow behavior
    |       `--> rejected with explicit reason
    |
    v
mark delivery result by operation ID
```

Never mark the catch delivered before inventory accepts the transaction.

---

# 7. Suggested Recipe Contract

```yaml
id: cook_roasted_game_meat
schema_version: 1

inputs:
  - item_id: raw_game_meat
    quantity: 2

outputs:
  - item_id: roasted_game_meat
    quantity: 2

equipment:
  required_capabilities:
    - heat

fuel:
  options:
    - item_id: firewood
      quantity: 1

duration_minutes: 90
batch_size: 1

worker:
  proficiency: cooking
  minimum_level: 0

safety_transform:
  biological:
    mode: reduce_risk
    magnitude: 0.8
  radiological:
    mode: none

freshness:
  output_age_policy: prepared_now

quality:
  enabled: false

knowledge:
  unlock_id: recipe_roasted_game_meat
```

This is an example contract, not a demand for these exact fields. Reuse generic recipe/production schemas if
they already exist.

---

# 8. Food Safety Design Rule

Game systems often collapse several hazards into one “contamination” number. That is convenient but can
produce misleading mechanics if cooking is treated as a universal decontaminator.

E1-4 therefore requires explicit hazard classes where the repository supports them:

- biological pathogens;
- surface chemical contamination;
- radioactive particulate contamination;
- internal radionuclide burden;
- toxins/poisons;
- spoilage-derived risk.

A heat treatment can plausibly reduce some biological hazards. It does not automatically make radioactive
material disappear. If ASHFALL intentionally abstracts boiling, trimming, draining, or discard as reducing an
item-contamination gameplay value, the data should say exactly which abstract hazard is reduced and what waste
or trade-off occurs.

The user-facing game may remain simplified. The architecture should still avoid encoding a false universal
law such as “well cooked = 80% radiation removed.”

---

# 9. Minimal First-Slice Recipes

The first five should cover distinct decisions:

## 1. Roasted game meat

- simplest heat transformation;
- moderate fuel;
- short-medium time;
- no special preservation;
- establishes basic raw → cooked loop.

## 2. Boiled meat or simple stew

- requires boiling capability and water if water is a real resource;
- may reduce biological/surface contamination according to validated policy;
- may produce broth/output byproduct;
- longer time.

## 3. Dried/jerky meat

- long duration;
- preservation benefit;
- lower immediate throughput;
- tests spoilage integration.

## 4. Mixed survival stew

- multiple ingredients;
- tests transactional reservations and multi-input recipes;
- higher morale/quality potential.

## 5. High-contamination handling recipe

- only if contamination rails can model it honestly;
- trades time/fuel/yield/waste for reduced item contamination;
- never promises universal radiological safety.

Recipes 6–15 are gated on acceptance metrics.

---

# 10. Cooking Job State Machine

```text
AVAILABLE_RECIPE
      |
      v
VALIDATING
      |
      +--> REJECTED
      |
      v
RESERVED
      |
      +--> CANCELLED
      |
      v
ACTIVE
      |
      +--> INTERRUPTED
      |       +--> ACTIVE
      |       +--> CANCELLED
      |       `--> FAILED
      |
      v
COMPLETED_PENDING_DELIVERY
      |
      +--> DELIVERY_BLOCKED
      |       `--> COMPLETED_PENDING_DELIVERY
      |
      v
DELIVERED
      |
      v
RETIRED
```

Key rules:

- inputs are not available to other jobs once reserved;
- output is not granted before completion;
- delivery is idempotent;
- job history is bounded;
- reload resumes one state, not two;
- UI only commands transitions.

---

# 11. Consumption Transaction

```text
Player selects food item
    |
    v
Consumption authority validates
    |
    +--> is edible?
    +--> quantity available?
    +--> survivor can consume?
    +--> item state / contamination / spoilage?
    |
    v
Inventory removal transaction
    |
    v
Needs/nutrition effects
    |
    +--> hunger
    +--> nutrition
    +--> hydration if modeled
    |
    v
Health/affliction consequences
    |
    +--> foodborne risk
    +--> contamination exposure
    |
    v
Morale/taste consequences
```

No trapping- or cooking-specific code should bypass this pipeline.

---

# 12. Old-Save Compatibility Matrix

| Legacy state | Expected behavior |
|---|---|
| No cooking section | Empty/default cooking state |
| Active traps, no catch | Continue normally |
| Catch pending | Preserve pending state |
| Butchery active | Resume under existing trapping semantics |
| Butchery completed with explicit undelivered marker | Deliver once if migration proves ownership |
| Historical completed catch with ambiguous delivery | Do not invent retroactive grant |
| Existing raw meat inventory | Remains valid under new consumption/cooking rules |
| Existing food with no freshness metadata | Apply canonical migration/default |
| Existing contaminated item | Preserve contamination semantics |

Migration must prefer not gifting items over speculative duplication.

---

# 13. Conservation Equations

For every butchery operation:

```text
resolved_yield
= accepted_inventory_output
+ canonical_overflow/staged_output
+ explicit_discard/loss
```

For every cooking operation:

```text
reserved_inputs + reserved_fuel
= consumed_inputs + returned_inputs + explicit_loss
```

and:

```text
recipe_outputs
= delivered_outputs + canonical_staged_outputs + explicit_waste/loss
```

These are logical conservation equations. Recipes may transform mass/quantities according to design, but the
transaction accounting must explain every input and output.

---

# 14. Exploit Matrix

| Exploit | Guard |
|---|---|
| Double-click Start | Stable request/job ID + reservation |
| Reload before completion | Persist progress and reserved inputs |
| Reload after completion | Delivery marker |
| Cancel after output roll | Output generated only at authoritative completion |
| Cancel/refund fuel repeatedly | Phase-aware refund policy |
| Butchery event replay | Operation delivery ID |
| Stack split resets contamination | Metadata-preserving split |
| Stack merge removes freshness | Merge compatibility rules |
| Cheap recipe XP farming | XP eligibility/caps |
| Meal morale farming | Cooldown/diminishing return |
| Quest completion replay | Event/quest idempotency |
| Capacity failure deletes output | Canonical staging/blocked delivery |
| Zero-fuel recipe loop | Explicit recipe resource policy |

---

# 15. Balance Metrics

Track:

- trap success → usable inventory conversion rate;
- average edible yield per trap-day;
- raw food consumption rate;
- prepared food consumption rate;
- hunger restored per trap-day;
- hunger restored per labour-hour;
- hunger restored per unit fuel;
- spoilage loss percentage;
- preparation usage rate;
- recipe diversity used per campaign;
- cooking queue utilization;
- number of blocked cooks by cause;
- contamination/safety incidents;
- foodborne affliction rate;
- morale gained from meals;
- preservation adoption;
- cooking proficiency progression rate if enabled;
- first five recipe pick distribution;
- inventory overflow incidents from butchery;
- transfer/conservation test failures;
- cooking tick median/p95 cost.

The first success metric is not “fifteen recipes authored.” It is “trapping catches reliably become player-
usable food through canonical inventory and needs.”

---

# 16. Headless Selftest Scenario

A compact deterministic selftest should:

1. create a known trap site and quarry outcome;
2. resolve one catch;
3. complete butchery;
4. assert exact raw meat/hide yield;
5. deliver to inventory exactly once;
6. save/reload and assert no duplicate delivery;
7. start one simple recipe;
8. verify ingredient/fuel reservation;
9. advance time headlessly;
10. complete and deliver prepared output;
11. consume prepared food;
12. assert canonical hunger restoration;
13. assert contamination/safety rule;
14. run spoilage time if available;
15. verify no duplicate XP/morale/quest effects.

The selftest should fail loudly on unresolved item IDs or authority bypass.

---

# 17. Performance Requirements

The feature should avoid:

- per-frame scanning of all recipes;
- per-frame scanning of all inventory for every cooking job;
- one timer node/object per food stack where a centralized decay index exists;
- retaining every completed cook forever;
- reparsing recipe JSON on every UI open;
- recomputing all recipe availability repeatedly when only one inventory/equipment dependency changed.

Prefer:

- catalog loaded once;
- indexed recipes by ingredient/capability;
- event-driven/dirty recomputation for UI availability;
- bounded job list;
- canonical perishability scheduler;
- deterministic compact save DTOs.

---

# 18. UI Read Model

Recommended sections:

### Available recipes
- known/unknown;
- output;
- ingredient sufficiency;
- fuel sufficiency;
- equipment capability;
- duration;
- worker requirement;
- block reason.

### Active jobs
- recipe;
- assigned worker;
- equipment;
- progress;
- reserved inputs/fuel;
- interrupt/block state;
- cancel availability.

### Food safety
- player-facing game hazard labels;
- preparation effect;
- no unsupported absolute safety claims.

### Output
- nutrition profile;
- shelf life;
- morale/quality if applicable;
- amount.

The panel should never display values calculated independently from the same data the command authority uses.

---

# 19. Follow-On Opportunities Gated by E1-4

## Fermentation/preservation

Admit only after canonical spoilage and recipe rails are stable.

## Cooking competitions

Admit only after meal quality/morale and social event rails prove useful.

## Recipe trading

Admit only after information/economy/knowledge ownership is clear.

## Cooking specialization

Admit only after generic skill/proficiency integration works.

## Advanced food poisoning

Admit only if disease/affliction rails can represent preparation-specific risk without duplication.

## Bulk institutional cooking

Admit only if large-population throughput becomes a measured bottleneck.

---

# 20. Verification Matrix

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --cooking-system-selftest
bash scripts/ci/verify-fast.sh
```

Targeted suites should include:

- `TrappingInventoryBridgeTests`
- `ButcheryYieldTests`
- `FoodItemStateTests`
- `FoodConsumptionTests`
- `CookingRecipeValidationTests`
- `CookingReservationTests`
- `CookingLifecycleTests`
- `CookingEquipmentTests`
- `CookingFuelTests`
- `CookingSafetyTransformTests`
- `CookingNutritionTests`
- `CookingSkillTests`
- `FoodSpoilageTests`
- `CookingUiAuthorityTests`
- `CookingSaveMigrationTests`
- `CookingConservationTests`
- `CookingDeterminismTests`
- `CookingPerformanceTests`

---

# 21. Completion Checklist

- [ ] Authority audit completed at current HEAD.
- [ ] Trapping output dead-end reproduced in a test.
- [ ] Butchery yields resolve to canonical item IDs.
- [ ] Completed catches reach inventory exactly once.
- [ ] Capacity failure is explicit.
- [ ] Raw trapping food can be consumed through canonical needs.
- [ ] Item contamination/freshness metadata survives transfer.
- [ ] Cooking ownership ADR accepted.
- [ ] Recipe schema validates.
- [ ] Five first-slice recipes implemented and differentiated.
- [ ] Ingredient/fuel reservation is transactional.
- [ ] Cooking progress uses canonical time/job rails.
- [ ] Equipment capability uses canonical facilities.
- [ ] Food-safety transforms are domain-valid and data-driven.
- [ ] No universal unsupported “cooking removes radiation” rule exists.
- [ ] Nutrition remains owned by canonical item/nutrition systems.
- [ ] Skill/proficiency reuses existing framework or is explicitly deferred.
- [ ] Spoilage reuses canonical perishability.
- [ ] UI commands authoritative jobs.
- [ ] Old saves load safely.
- [ ] Ambiguous legacy catches are not gifted twice.
- [ ] Save/reload cannot duplicate inputs, outputs, XP, morale, or quest progress.
- [ ] Conservation/exploit suite passes.
- [ ] Headless selftest passes.
- [ ] 180-day soak remains bounded.
- [ ] Balance metrics show trapping is a viable but not dominant food source.
- [ ] Recipes 6–15 are gated on first-slice acceptance.
- [ ] `E1_planintegration[5].md` is the next sequence filename.

---

# 22. Final Directive

This plan is successful when the food loop becomes connected, not when the cooking feature becomes large.

The project already paid the complexity cost of trapping. The immediate obligation is to make the result of
that system arrive in the player's real inventory and interact with the real needs model. Cooking is valuable
only after that seam is closed and only insofar as it creates meaningful trade-offs among time, fuel,
equipment, safety, shelf life, nutrition, and morale.

If a proposed cooking feature requires its own food inventory, its own hunger numbers, its own radiation
ledger, its own workstation state, or its own skill framework, stop and extend the canonical owner instead.

The strongest E1-4 outcome may be smaller than Plan 136's original feature list: one exact trapping bridge,
one reliable consumption path, five high-value recipes, one real kitchen surface, deterministic saves, and a
food pipeline that finally reaches the player.
