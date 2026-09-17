# C1 — Flagship Integration Plan [4]: One Food Authority — Eating, Meals, Medicine & Pantry Truth

> **Output:** `C1_planintegration[4].md`
>
> **Source baseline:** Plan 22 — One Food Authority: Eating, Meds, and What the Pantry Actually Holds
>
> **Wave:** Continuity Wave 2 — *The Bunker Machine*
>
> **Dependencies:** none required to start. Shares bill/recipe machinery with 21B. Feeds Plan 23 (cold storage ↔ power) and Plan 24 (cook/meal service ↔ duty roster).
>
> **Mandatory execution order:** 22A → 22B → 22C.
>
> **Primary architectural rule:** one stock of raw consumables, one host consumption authority, one authored effect table. Prepared meals may remain a prepared-portions state, but may not become an independent raw-resource authority.
>
> **Guardrails:** no new consume API in Core, no new raw-food stockpile, no parallel medicine subsystem, no invisible auto-feeding, no code-side hardcoded food item lists.

---

# 0. Mission

ASHFALL already contains most of the correct food and medicine mechanics. The defect is integration.

The current game reportedly contains:

- a full data-driven `Inventory.Consume(...)` API that applies authored hunger, thirst, health, morale, radiation, iodine, and contamination effects;
- a host wrapper that calls that API without the effect callbacks, causing items to disappear while applying no gameplay consequence;
- a disconnected inventory consume handler;
- a second live EAT/DRINK implementation in the holdfast terminal using a hardcoded item list, a hardcoded hunger delta, one survivor, and the trade ledger as a larder;
- a functioning kitchen pipeline that consumes real ingredient bills, advances prep jobs, tracks portions and spoilage, and can serve a meal into needs—but no live player path actually serves those meals;
- medicine items and medical systems that exist independently but are not joined through one consumption/treatment path.

This plan does not invent more systems. It connects the ones already present.

The final player-facing model must be:

```text
AUTHORED ITEM / RECIPE DATA
          │
          ▼
SHELTER INVENTORY  ─────────────┐
                                │
                    ┌───────────▼───────────┐
                    │ SINGLE CONSUME AUTHORITY │
                    │ subject + item + scale │
                    └───────────┬───────────┘
                                │
             ┌──────────────────┼───────────────────┐
             ▼                  ▼                   ▼
          NEEDS             RADIATION          CONTAMINATION
      hunger/thirst/etc.   iodine/cleanse        dose/events
             │                  │                   │
             └──────────────────┼───────────────────┘
                                ▼
                         DAY EVENT / AUDIO
```

Prepared food extends that model:

```text
SHELTER INVENTORY
      │
      ▼
TryConsumeBill(recipe ingredients)
      │
      ▼
KitchenNutritionSystem prep job
      │
      ▼
prepared meal portions + spoilage
      │
      ▼
ServeMeal(subject, meal)
      │
      ├── needs + morale
      ├── serving log
      ├── briefing event
      └── grievance / duty / medical-diet integrations
```

Medicine uses the same subject and stock authority:

```text
SHELTER INVENTORY
      │
      ▼
Consume(subject, medicalItem, therapeuticScale)
      │
      ├── health / rad cleanse / iodine window
      ├── contamination or adverse effects
      ├── dose/treatment ledger
      ├── dependency / withdrawal
      ├── triage policy
      └── exactly one medical cue
```

---

# 1. Source-Evidence Interpretation

The source plan identifies a continuity defect rather than a missing feature.

## 1.1 The correct Core API already exists

`Inventory.Consume(...)` already exposes callback seams for:

- needs modification;
- radiation cleanse;
- iodine protection;
- contamination;
- therapeutic scale.

Therefore, adding another Core consume API is explicitly out of scope.

## 1.2 The intended inventory route is disconnected

The existing host handler for inventory consumption exists but is not reached by the player-facing inventory panel. This is a wiring defect and must receive a click-path test.

## 1.3 The current host wrapper is semantically inert

The host calls `Inventory.Consume(...)` with the gameplay callbacks omitted. Removal succeeds, status feedback reports success, but the authored effects never reach the survivor.

This is especially dangerous because the UI can report a valid action while the simulation does not change.

## 1.4 The live terminal path is a duplicate game

The holdfast EAT/DRINK implementation reportedly:

- reads from trade-held goods;
- uses a hardcoded food ID list;
- applies `-30 hunger`;
- targets only `PlayerSurvivorId`.

That path must be removed or delegated to the single authority. Compatibility is not a justification for preserving duplicate gameplay logic.

## 1.5 The kitchen exists but is not the crew's eating path

`KitchenNutritionSystem` already owns:

- ingredient bill consumption;
- prep jobs;
- portions;
- spoilage;
- meal serving;
- needs application;
- serving log;
- `OnMealServed`.

Plan 22B is primarily activation and integration.

## 1.6 Medicine is already represented in authored data

Medical items have authored effect fields and the project already contains dose, sickness, treatment, dependency, and medical systems. The missing piece is one host-side treatment path and policy integration.

---

# 2. Non-Negotiable Invariants

## INV-22.1 — One raw stock authority

Raw food, water, medicine, and contaminated consumables are consumed from the shelter inventory.

The trade ledger is not the larder.

If trade-held goods and inventory counts overlap, resolve the ownership bug explicitly.

## INV-22.2 — One host consumption authority

Every direct consumable action must route through one subject-aware host method.

Required semantic signature:

```csharp
Consume(string survivorId, string itemId, float therapeuticScale)
```

Exact naming/types may follow project conventions.

## INV-22.3 — Authored effects are authoritative

No code-side replacement for:
- hunger restore;
- thirst restore;
- health effect;
- morale effect;
- rad cleanse;
- contamination;
- iodine effect;
- therapeutic effect scale.

The catalog value is the gameplay value unless a named system deliberately modifies it.

## INV-22.4 — Subject is explicit

Consumption cannot occur without identifying who consumed the item, except explicit headless/selftest fallback paths.

## INV-22.5 — Prepared meals are not a second raw-item stock

Kitchen pantry state may track prepared meal portions and spoilage because those are transformed outputs.

It must not become an independent authority for raw inventory counts.

## INV-22.6 — Crew feeding is atomic or explicitly partial

"Serve/feed all" must define its transaction semantics.

Preferred default:
- preflight quantity/portion availability;
- if insufficient, fail without partially consuming;
- offer per-survivor serving separately.

If partial service is a deliberate social mechanic, it must be explicit and grievance-producing, never accidental.

## INV-22.7 — Exactly one gameplay effect and one cue per action

One click:
- one inventory decrement;
- one authored effect application;
- one event;
- one appropriate audio cue.

No duplicate callbacks or duplicated cues.

## INV-22.8 — Medical treatment is logged

A medical item cannot affect radiation/health/dependency without producing a treatment record or equivalent authoritative history.

## INV-22.9 — Preservation state comes from shelter infrastructure

Cellar/refrigeration state may not be arbitrary defaults disconnected from shelter state.

## INV-22.10 — No auto-feeding

Food remains a player/system decision governed by inventory, meal portions, duty assignments, and explicit service actions.

---

# 3. Definition of Done

Plan 22 closes when all are true:

- inventory consume button reaches the host;
- direct food/water/medicine consumption routes through one subject-aware host method;
- authored effects are applied;
- contaminated consumables apply contamination;
- iodine applies a real protection window;
- radiation treatments affect the authoritative radiation/dose system;
- holdfast terminal no longer contains its own hunger arithmetic;
- holdfast terminal no longer owns a code-side edible ID list;
- shelter inventory—not the trade ledger—is the source stock;
- "feed all" can target all living survivors and fails honestly on shortage;
- kitchen prep and serving are reachable from the UI;
- prepared meals produce better value than raw emergency rations without becoming mandatory;
- spoilage reflects cellar/refrigeration state;
- cook assignment is compatible with Plan 24's duty authority;
- unequal service can feed the existing ration-conflict/grievance system;
- medical items have explicit windows/limits/policies through existing systems;
- treatment history is visible in the medical panel from the authoritative ledger;
- save/load preserves consumable, kitchen, treatment, and timeline state;
- exactly one cue fires per consumption/treatment;
- 10-day feeding policy replay is deterministic;
- all required build/selftest/CI gates pass.

---

# 4. Phase P0 — Baseline, Inventory and Ownership Freeze

## P0.1 Record repository baseline

Capture:

```text
commit SHA
branch
dirty-file count
Core test count
current inventory consumer call sites
current HoldfastRuntimeSession consume call sites
current ServeMeal call sites
current StartPrepJob call sites
current SetCellar call sites
current SetRefrigeration call sites
current medical item IDs
current trade-held vs shelter-inventory ownership paths
```

Do not use source-plan line numbers as permanent truth without rechecking current source.

## P0.2 Read the complete relevant files

Before editing, read:

- `Assets/Ashfall.Core/Inventory/Inventory.cs`
- item definition/catalog models and loaders
- `src/Host/InventoryHostSession.cs`
- `src/Main.Inventory.cs`
- inventory panel classes and action registration
- `src/Host/HoldfastRuntimeSession.cs`
- `src/Host/HoldfastTerminalPanel.cs`
- `Assets/Ashfall.Core/KitchenNutritionSystem.cs`
- `src/Host/KitchenNutritionHostSession.cs`
- `src/UI/KitchenNutritionPanel.cs`
- shelter room/infrastructure state
- power/refrigeration-related code that Plan 23 will own
- `DutyRosterSystem`
- `RationConflictSystem`
- survivor social coordinator
- `DoseLedgerSystem`
- radiation system
- iodine protection implementation
- contamination system
- `ChemicalDependencySystem`
- `SickListSystem`
- medical treatment catalog/contracts
- `SaveSectionRegistry`
- all relevant save DTOs.

## P0.3 Build a consumable authority table

Generate a machine-readable artifact:

```text
item_id
item_type
is_consumable
hunger_restore
thirst_restore
health_effect
morale_effect
rad_cleanse
iodine_effect
contamination
durability
rad_protection
current_direct_consume_route
stock_source
subject_required
audio_cue
medical_class
notes
```

Use current catalog data.

This table becomes test input and drift evidence.

## P0.4 Build a stock ownership matrix

Required columns:

| Stock-like state | Owns raw items? | Owns prepared items? | Player-visible? | Mutation owner | Persisted? | Verdict |
|---|---:|---:|---:|---|---:|---|
| shelter inventory | yes | maybe references outputs | yes | Inventory | yes | authority |
| trade ledger held | no larder semantics | no | trade UI | trade | yes | ledger |
| kitchen pantry | no raw authority | yes, portions | kitchen | KitchenNutritionSystem | yes | transformed-state authority |

If current source contradicts this model, document before altering.

## P0.5 Reproduce three failures before changing code

Create or extend tests proving:

1. inventory consume removes item but authored needs effect is not applied;
2. holdfast terminal uses hardcoded hunger arithmetic;
3. contaminated consumable does not apply contamination through the live host path.

Preserve these as regression tests after repair.

---

# TASK 22A — One Direct Consumption Authority

# 22A.0 Goal

Any edible, drinkable, therapeutic, or contaminated inventory item consumed by a survivor must:

1. come from shelter inventory;
2. identify the survivor;
3. invoke the existing Core consume API;
4. apply all authored effects through real systems;
5. emit one attributable event;
6. play one cue;
7. persist the resulting state.

---

## 22A.1 Write the failing effect tests first

Minimum test cases:

### Food
Consume `canned_food`.

Assert:
- inventory decremented exactly once;
- hunger changes by authored value, not hardcoded 30;
- subject survivor changed;
- other survivor did not change;
- event records subject/item;
- one food cue if project has one.

### Iodine
Consume `iodine_pills`.

Assert:
- item decremented;
- iodine protection state is active;
- ledger/timeline records treatment;
- window duration matches authoritative rule;
- cue fires once.

### Contaminated water
Consume `irradiated_water`.

Assert:
- thirst effect applies if authored;
- contamination dose applies;
- item decremented;
- `contaminated_meal` or appropriate event emitted;
- no duplicate contamination callbacks.

### Anti-rad medicine
Consume one current catalog anti-rad item.

Assert:
- authored radiation-cleanse path is reached;
- ledger records treatment;
- inventory decremented once.

---

## 22A.2 Reconnect the inventory panel consume action

Trace the existing remove action from UI to host and mirror its route semantics.

Required click path:

```text
InventoryDetailPanel / Inventory UI
        │
        ▼
consume action signal
        │
        ▼
Main.OnInventoryConsumeClicked(subject, item)
        │
        ▼
InventoryHostSession.Consume(...)
        │
        ▼
Core Inventory.Consume(...)
```

If current UI has no survivor selection:
- use the current explicit player/selected survivor from live state;
- do not fallback to a literal survivor ID.

Add a UI interaction test that clicks the action.

The test must fail if the signal becomes disconnected again.

---

## 22A.3 Make the host consume method subject-aware

Preferred host contract:

```csharp
public ActionResult Consume(
    string survivorId,
    string itemId,
    float therapeuticScale)
```

Requirements:

- validate survivor exists and is alive;
- validate item exists;
- validate item is consumable;
- validate inventory has quantity;
- validate therapeutic scale;
- call existing Core consume API;
- wire callbacks;
- commit exactly once.

A backwards-compatible overload is allowed only if:
- used by an explicit selftest/headless path;
- clearly named/documented;
- never used by player UI.

---

## 22A.4 Wire `applyNeed`

Map Core need-effect callbacks to the authoritative survivor needs system.

Requirements:

- subject ID passed through;
- hunger/thirst/health/morale mapping follows current Core API semantics;
- clamping remains owned by needs system;
- no UI-side arithmetic.

Tests:
- lower/upper need bounds;
- invalid survivor;
- dead survivor if consumption should be blocked;
- multiple authored effects on one item.

---

## 22A.5 Wire `applyRadCleanse`

Resolve the exact radiation authority.

Rules:
- do not mutate only a UI display;
- treatment must affect the same dose state used by survival consequences;
- treatment entry must be recorded in dose/treatment ledger.

If radiation cleanse is gradual:
- `Inventory.Consume` callback should schedule/register treatment rather than instantly mutate the entire dose, according to current system design.

---

## 22A.6 Wire `applyIodine`

Use existing iodine protection.

Requirements:
- protection window starts at the same campaign clock used by exposure;
- repeated dosing obeys the medical rule from 22C;
- protection state persists;
- expired window does not silently remain active.

22A must wire the seam. 22C deepens policy and limits.

---

## 22A.7 Wire `applyContamination`

Contamination authored on an item must reach the same contamination/dose authority used by environmental exposure.

Cases:
- contaminated food;
- irradiated water;
- other catalog-tagged contaminated consumables.

Do not special-case individual IDs if item definitions already identify the effect.

Test:
- uncontaminated item => no dose;
- contaminated item => exact authored dose or sanctioned conversion.

---

## 22A.8 Therapeutic scale contract

`therapeuticScale` must have explicit meaning.

Document:
- accepted range;
- which effects it scales;
- whether hunger/thirst are scaled;
- whether medical effects only are scaled;
- whether contamination scales;
- whether dose is rounded/clamped.

Prefer Core's existing semantics.

Validate:
- NaN;
- infinity;
- negative;
- zero;
- unusually high values.

Do not let a UI arbitrary float create an exploit.

---

## 22A.9 Delete hardcoded terminal food arithmetic

Remove direct code such as:

```text
Needs.Modify(PlayerSurvivorId, Hunger, -30f * amount)
```

Replace terminal EAT/DRINK with delegation to the single host consumption authority.

Terminal responsibilities become:
- select subject;
- find candidate item from live catalog + stock;
- invoke one consume call;
- present result.

Terminal must not own gameplay effect values.

---

## 22A.10 Kill the code-side food ID list

Replace static lists such as:

```text
canned_food
ration_pack
dried_meat
mre
```

with catalog-driven filtering.

Recommended query semantics:

```text
inventory items
∩ consumable definitions
∩ edible/drinkable type or authored effect predicate
ordered by explicit preference
```

Preference ordering should be:
- data-driven if current catalog supports it;
- otherwise stable generic ranking such as lowest-value emergency food first, documented in host/UI logic.

Do not infer edibility from substring matching.

---

## 22A.11 Authored-data drift audit

For each former literal ID:
- does it exist?
- is it correctly typed?
- does it have authored effects?
- is it still intended?

Fix:
- data if the item should exist;
- code references if obsolete;
- tests so unknown IDs fail catalog integrity.

Do not create dummy catalog entries merely to preserve stale terminal literals.

---

## 22A.12 Correct the stock source

Terminal direct consumption must pull from shelter inventory.

Explicitly remove:

```text
Trade.GetHeld(...)
Trade.Inventory.RemoveItem(...)
```

from the food consumption path.

If trade purchase currently does not transfer goods into shelter inventory:
- trace trade settlement;
- add/repair the transfer at the trade transaction boundary;
- do not make EAT aware of trade-held goods.

Document the ownership seam in `docs/systems/`.

---

## 22A.13 Trade/inventory double-count audit

Test a purchase:

```text
before purchase:
trade held = X
shelter inventory = Y

after purchase:
seller/buyer ledger updated
shelter inventory changed once
no duplicated quantity exists in both stores as spendable food
```

The exact trade semantics may differ, but raw consumables cannot be independently consumable from both stores.

---

## 22A.14 Subject selection

Every direct consume UI must know who consumes.

Accepted sources:
- currently selected survivor;
- explicit dropdown/roster selection;
- player survivor if the terminal is deliberately personal.

Not accepted:
- first roster entry;
- fallback literal ID;
- hidden global subject.

Display the subject in feedback:

```text
Vasquez ate Canned Food.
```

or current localization equivalent.

---

## 22A.15 Feed-all preflight

Add a crew-feed command that can target all living survivors.

Before mutation:
1. collect eligible living survivors;
2. calculate required item count/servings;
3. verify available stock;
4. if insufficient and operation is atomic, return detailed shortage;
5. otherwise execute exactly once per survivor.

Example failure data:

```text
requested survivors: 6
available servings: 4
shortfall: 2
```

Do not silently feed one survivor.

---

## 22A.16 Partial-feed policy

If product design allows partial feeding:
- require explicit player choice;
- select recipients;
- emit unequal-service data;
- feed `RationConflictSystem` in 22B.

Do not let partial execution occur because a loop ran out of items mid-way.

---

## 22A.17 Day-event integration

Emit canonical events after successful application:

- `ate`
- `drank`
- `med_taken`
- `contaminated_meal`

Event payload should include where supported:

```text
day
survivor_id
item_id
quantity
effect_summary
source_surface
```

Events are observations, not authority.

---

## 22A.18 Audio integration

Route current cues through one successful action result.

Examples from source:
- `action_water_pour`
- `action_pill_bottle`
- `action_injection`

Requirements:
- cue fires after committed consumption;
- blocked action => no success cue;
- no double cue from both panel and survivor session;
- replay does not duplicate cue due to event subscription defects.

---

## 22A.19 Save/load attribution

After consumption:
- inventory count persists;
- survivor needs persist;
- radiation/iodine/contamination persist;
- treatment/event history persists where designed.

Add round-trip tests including "who consumed it."

---

## 22A.20 Deterministic 10-day feeding policy

Script a policy, e.g.:

```text
day 1–10:
feed selected survivor when hunger threshold crossed
drink when thirst threshold crossed
use no medicine unless triggered
```

Same seed + same inventory + same choices => identical:
- consumed items;
- survivor needs;
- contamination;
- event history;
- ending inventory.

---

## 22A.21 22A CI/source gates

Add targeted checks:

- no hardcoded `-30` hunger logic in terminal consume path;
- no static edible ID list in terminal;
- no trade-ledger removal in direct EAT/DRINK path;
- inventory consume action is registered;
- player direct consume route resolves to subject-aware host method.

### 22A DoD

One consume authority, one raw stock, authored effects, explicit subject, correct callbacks, exactly one cue/event, deterministic persistence.

---

# TASK 22B — Make the Kitchen the Crew's Table

# 22B.0 Goal

Cooking must turn real ingredients into real prepared portions, those portions must spoil according to shelter preservation, and serving must be the principal way to feed the crew efficiently.

Raw tins remain a fallback.

---

## 22B.1 Read and map the full kitchen state machine

Document:

```text
recipe selected
→ bill validation
→ TryConsumeBill
→ prep job
→ day ticks
→ prepared portions
→ pantry/spoilage
→ ServeMeal
→ needs
→ MealServingLog
→ OnMealServed
```

Confirm actual API names and state fields from current source.

---

## 22B.2 Wire `StartPrepJob`

The kitchen panel's existing SELECT/recipe flow must reach `StartPrepJob`.

Required UI states:
- recipe selectable;
- ingredient bill visible;
- missing ingredients explained;
- job duration visible;
- existing job conflict explained;
- action result shown.

Do not duplicate bill arithmetic in the panel.

---

## 22B.3 Share bill machinery with 21B

If Plan 21B is implementing shared `TryConsumeBill`/recipe semantics:
- use the same bill validator/consumer;
- avoid separate kitchen-specific quantity logic;
- coordinate file ownership/commit sequencing.

Add contract tests:
- one bill;
- same inventory decrement semantics;
- no double consumption.

---

## 22B.4 Wire `ServeMeal` per survivor

Kitchen panel must expose:
- prepared meal;
- portions remaining;
- selected survivor;
- serve action.

Call existing host/session wrapper and Core method.

Assertions:
- one portion decremented;
- correct survivor needs change;
- serving log appended;
- `OnMealServed` emitted exactly once.

---

## 22B.5 Serve-all action

Add:

```text
Serve all living survivors
```

Preflight:
- eligible survivor count;
- available portions;
- optionally ration policy.

Failure:
- no silent partial service.

If explicit partial service is offered:
- recipients displayed before confirmation;
- unserved survivors feed social grievance logic.

---

## 22B.6 Make cooked meals meaningfully better than raw emergency food

Use existing recipe/meal nutrition value.

Define a documented transformation from nutrition into:
- hunger restoration;
- morale benefit.

Prefer existing Core fields/methods.

Do not invent arbitrary panel-side bonuses.

Balance criteria:
- cooked meal yields a measurable advantage;
- fuel/time/cook-duty cost means the advantage is not free;
- raw tins remain viable in emergencies;
- player is not forced into kitchen every day under normal baseline.

---

## 22B.7 Preservation authority: cellar

At setup/load:
- inspect shelter room/infrastructure authority;
- if root cellar is built/functional, call `SetCellar(true)`;
- otherwise false.

Do not hardcode true.

Persist source state in the shelter authority, not kitchen duplicate flags unless kitchen state intentionally caches derived status.

---

## 22B.8 Preservation authority: refrigeration

Plan 23 will own powered refrigeration.

Plan 22 must establish the seam:

```text
shelter refrigeration installed
AND refrigeration currently powered
→ KitchenNutritionSystem.SetRefrigeration(true)
```

Until Plan 23 lands:
- do not pretend powered refrigeration exists;
- cellar can be wired now;
- add explicit integration test marked for Plan 23 activation.

---

## 22B.9 Recompute preservation when infrastructure changes

When room/power state changes:
- kitchen preservation state updates;
- spoilage projection refreshes.

Avoid one-time setup-only wiring if shelter state can change.

---

## 22B.10 Spoilage visibility

Kitchen UI must display:

```text
meal name
portions
days until spoilage
preservation source
```

Example:
- `4 portions — spoil in 2 days`
- `4 portions — refrigerated — spoil in 5 days`

Use current localization conventions.

---

## 22B.11 Spoilage event integration

When portions spoil:
- decrement/replace state according to existing kitchen logic;
- emit canonical spoilage event;
- briefing explains loss;
- no duplicate message after load.

---

## 22B.12 Ingredient-source neutrality

Recipes consume catalog items regardless of whether they originated from:
- greenhouse;
- wildlife trapping;
- scavenging;
- trade transfer;
- starting inventory.

Kitchen must not branch on origin system.

The inventory item ID/type is the seam.

---

## 22B.13 Cook duty seam

Plan 24 will own duty assignment.

Plan 22 defines prerequisite behavior:

```text
prep job requires assigned cook capacity / cook token
```

Do not add an invisible auto-cook fallback.

Until Plan 24 is active:
- use existing duty-roster API if already available;
- otherwise create a clearly temporary integration adapter that fails closed rather than auto-cooking.

---

## 22B.14 Prep progression ownership

Confirm whether prep advances:
- on daily tick;
- by work hours;
- by cook duty completion.

Use existing Core semantics.

Do not tick kitchen jobs from both kitchen session and duty owner.

Add single-tick ownership test.

---

## 22B.15 Unequal-serving grievance link

Use existing `RationConflictSystem` / social coordinator.

Trigger only when:
- there are eligible hungry/living survivors;
- some receive a meal;
- others do not;
- inequality meets current grievance rules.

Do not create a second grievance model.

Payload should include served/unserved IDs where architecture supports it.

---

## 22B.16 Convalescent/medical diet seam

Read current caregiving/convalescence data.

If authored meal/diet rules already exist:
- allow kitchen service to satisfy them;
- use same serving log;
- expose medical priority recommendation.

If no authored medical diet exists:
- document "not supported" and do not invent a nutrition subsystem in Plan 22.

---

## 22B.17 Kitchen pantry semantics

Clarify prepared-meal state:

Allowed:
- recipe/meal ID;
- prepared portions;
- prepared day;
- spoil day;
- quality/nutrition;
- preparation metadata.

Forbidden:
- duplicate raw ingredient counts;
- direct purchase into pantry;
- arbitrary consumable item ownership separate from shelter inventory.

Add invariant tests.

---

## 22B.18 Kitchen save round-trip

Persist and restore:
- active prep jobs;
- job progress;
- prepared meal portions;
- spoilage timestamps/counters;
- preservation flags if legitimately stateful;
- serving log.

After load:
- no duplicate serving log entries;
- no reset spoilage clock;
- no duplicated ingredient bill.

---

## 22B.19 Kitchen snapshot/interaction test

Capture representative states:

1. no recipe selected;
2. recipe selected + ingredients available;
3. missing ingredients;
4. prep in progress;
5. prepared portions available;
6. insufficient portions for crew;
7. cellar active;
8. refrigerated state once Plan 23 lands.

Only update snapshots for intentional UI changes.

---

## 22B.20 First-10-day telemetry funnel

Track:

```text
food stock
raw consumptions
meals prepared
meals served
spoiled portions
hunger distribution
morale delta from meals
cook-duty usage
shortage failures
grievances
```

Acceptance:
- kitchen becomes relevant early;
- raw food is a valid fallback;
- prep does not trivialize hunger;
- spoilage pressure is noticeable but not punitive.

### 22B DoD

Kitchen prep and serving are live; meals use real ingredients, real subjects, real serving logs, real spoilage, and real shelter/duty integrations.

---

# TASK 22C — Medicine Is a Decision, Not Inventory Deletion

# 22C.0 Goal

Medical consumables must use the same inventory/subject authority as food and water but feed the correct medical systems, with explicit time windows, limits, priority policy, side effects, and treatment history.

---

## 22C.1 Enumerate current medical consumables

Build a current-data table before changing rules.

Required columns:

```text
item_id
item_type
health_effect
rad_cleanse
morale_effect
iodine_effect
dependency_class
treatment_class
affliction_targets
contraindications
expiry/spoilage
cue
```

Potential source examples:
- iodine pills;
- anti-rad;
- rad-away;
- bandage;
- morphine/analgesic;
- antibiotics;
- chelation agents.

Do not assume every example exists under that exact ID.

---

## 22C.2 Classify each medical item

Use current data authority to assign or derive:

- prophylaxis;
- radiation clearance;
- wound care;
- infection treatment;
- analgesia;
- chelation/detox;
- other authored class.

Avoid branching on raw string IDs when a treatment class/type can be authoritative.

---

## 22C.3 Data-driven treatment rules

Where current systems already contain rule data, use it.

Where a rule is absent but required by existing medical mechanics, add data—not scattered constants—for:

- prophylaxis window;
- dose ceiling;
- clearance duration/curve;
- cooldown;
- dependency liability;
- taper requirement;
- contraindication;
- treatment quality multiplier.

Keep new fields minimal and schema-validated.

---

## 22C.4 Iodine window

Wire `applyIodine` into the authoritative protection state.

Policy must define:
- start time;
- duration;
- repeat dose behavior;
- campaign/survivor ceiling;
- what happens when ceiling reached;
- persistence.

Use the same campaign clock as exposure.

Test:
- dose before exposure;
- dose during active window;
- expiry;
- repeated dose;
- ceiling reached;
- save/load mid-window.

---

## 22C.5 Radiation-clearing treatment

Use `applyRadCleanse`.

If current model supports a curve:
- treatment registers a rate/duration;
- daily dose ledger applies it deterministically.

Do not delete accumulated dose instantly unless existing authority explicitly models that.

Tests:
- baseline without treatment;
- treatment curve;
- repeated treatment;
- maximum/minimum dose clamping;
- save/load midpoint.

---

## 22C.6 Treatment record in dose ledger

Every radiation-affecting treatment writes a record with fields such as:

```text
day/time
survivor_id
item_id
treatment_class
therapeutic_scale
pre_treatment_dose
post/expected effect
window/cooldown
```

Use existing ledger schema where possible.

Do not create a second medical-history store.

---

## 22C.7 Wound/infection treatment

Bandages/antibiotics must route through current medical/affliction systems.

Rules:
- item removal occurs only if treatment accepted;
- wrong treatment returns a specific block reason;
- authored health effect may coexist with affliction treatment if current Core API supports it.

Do not allow a bandage to silently be consumed with no target/effect.

---

## 22C.8 Sick-list / triage priority

`SickListSystem` and ward reservations inform who should receive scarce treatment first.

Implement:
- recommendation/order;
- "treat highest priority" action if appropriate;
- explicit per-survivor override.

Do not auto-spend medicine without player/system policy consent.

---

## 22C.9 Dependency integration

For dependency-risk medicines:
- successful use informs `ChemicalDependencySystem`;
- repeated use updates dependency state;
- withdrawal/stress uses the same campaign clock;
- taper/detox items feed existing relapse/taper mechanics.

Do not implement a new addiction state in inventory or medical panel.

---

## 22C.10 Stress/withdrawal seam

Confirm Plan 09 relapse rules and current `OnStressReported` flow.

Add integration test:

```text
dependent survivor
→ medication withheld / taper condition
→ stress event
→ withdrawal/relapse rule
→ persisted state
```

Use actual project semantics.

---

## 22C.11 Prevention > treatment balance ordering

Balance should preserve strategic hierarchy:

```text
avoid exposure
> shielding/decontamination
> prophylaxis
> post-exposure treatment
```

Treatments may mitigate but must not make exposure prevention irrelevant.

Run comparative simulation if current balance tool supports radiation.

---

## 22C.12 Distinct blocked reasons

Replace generic "cannot consume" for medical decision failures with structured reasons.

Examples:
- no dose remaining;
- treatment ceiling reached;
- wrong treatment class;
- no applicable affliction;
- contraindicated;
- expired;
- survivor unavailable;
- dependency/taper restriction;
- insufficient therapeutic requirement.

Use localized message keys rather than hardcoded prose if project architecture supports it.

---

## 22C.13 Pharma expiry/spoilage seam

If pharma expiry is already modeled:
- display and enforce it;
- expired stock uses current item-expiry semantics.

If not modeled:
- do not clone kitchen spoilage into medicine automatically;
- document the gap and only add it if current medical data explicitly expects expiry.

The source plan requests pharma spoilage, but implementation must remain grounded in existing authority.

---

## 22C.14 Medical-panel treatment timeline

Add a per-survivor view reading the authoritative ledger.

Display where available:
- item/treatment;
- taken time/day;
- remaining protection window;
- cumulative ceiling usage;
- current clearance treatment;
- dependency/taper status;
- block reason/recommendation.

No panel-local memory.

---

## 22C.15 Medical audio

Use:
- `action_pill_bottle`;
- `action_injection`;
- other existing cues.

Cue mapping should be treatment-class/item-data driven where possible.

One successful treatment => one cue.

Blocked treatment => no success cue.

---

## 22C.16 Medical day events

Successful actions feed:
- `med_taken`;
- treatment-specific existing vocabulary if available.

Adverse outcomes may feed:
- contamination;
- withdrawal;
- failed-treatment events if current event system supports them.

Do not manufacture event IDs without catalog registration.

---

## 22C.17 Treatment save round-trip

Persist:
- inventory decrement;
- applied needs;
- active iodine window;
- clearance treatment;
- dose ledger record;
- dependency state;
- treatment history/timeline source.

After load:
- remaining window/curve must match elapsed campaign time semantics;
- no duplicate treatment entry.

---

## 22C.18 One integration test per medical class

Minimum:
- prophylaxis;
- radiation cleanse;
- bandage/wound;
- antibiotic/infection if supported;
- analgesic/dependency if supported;
- chelation/detox if supported.

Each test must traverse:
UI/host seam where practical → single consume authority → target medical authority.

---

## 22C.19 Medical data-integrity validation

Validate:
- treatment item references;
- treatment class;
- affliction IDs;
- window/ceiling values;
- dependency class IDs;
- taper recipe IDs;
- cue IDs if cataloged.

Run `--data-integrity-selftest`.

### 22C DoD

Medicine uses the same stock and subject authority as all consumables, but its authored effects are mediated by the correct treatment, dose, dependency, and triage systems.

---

# 5. Cross-Task Dependency Graph

```text
                       ┌──────────────────────┐
                       │  21B bill/recipe     │
                       │  consumption seam    │
                       └──────────┬───────────┘
                                  │
                                  ▼
22A SINGLE CONSUME AUTHORITY ────────────────┐
      │                                      │
      ├────────► 17A day events              │
      ├────────► 17C one cue per act         │
      │                                      │
      ▼                                      ▼
22B KITCHEN / CREW TABLE ───────────────► 22C MEDICINE
      │                                      │
      ├────────► 23 powered refrigeration    ├──► dose ledger
      ├────────► 24 cook duty                ├──► sick list / triage
      └────────► ration grievances           └──► dependency / taper
```

Execution is strict:

```text
22A
 ↓
22B
 ↓
22C
```

Do not let 22B or 22C create alternate consumption paths while 22A remains unresolved.

---

# 6. Integration Contracts

## 6.1 Inventory contract

Inventory owns:
- raw consumable quantities;
- removal/consumption transaction.

Inventory does not own:
- survivor needs;
- radiation history;
- dependency;
- kitchen serving policy.

---

## 6.2 Survivor needs contract

Needs owns:
- hunger;
- thirst;
- health/morale deltas where current architecture routes them.

Consume authority passes the subject and authored effect.

---

## 6.3 Trade contract

Trade owns:
- transaction ledger;
- merchant stock;
- purchase/sale accounting.

Purchased consumables must cross into shelter inventory before being edible.

---

## 6.4 Kitchen contract

Kitchen owns:
- prep jobs;
- transformed meal portions;
- spoilage;
- meal service logs.

Kitchen does not own raw food authority.

---

## 6.5 Medical contract

Medical/dose systems own:
- treatment applicability;
- radiation cleanse/protection;
- affliction treatment;
- treatment timeline;
- dependency consequences.

Inventory only owns the consumable transaction.

---

## 6.6 Duty contract

Duty roster owns:
- who is assigned to cooking;
- availability/capacity.

Kitchen consumes that capacity; it does not create workers.

---

## 6.7 Shelter/power contract

Shelter/power owns:
- root cellar availability;
- refrigeration installation;
- refrigeration power.

Kitchen consumes a boolean/read model for spoilage calculation.

---

# 7. Transaction Semantics

Consumption must be failure-safe.

Recommended operation:

```text
1. validate subject
2. validate item/meal
3. validate stock/portion
4. validate treatment applicability/policy
5. compute effects
6. commit inventory/portion decrement
7. apply gameplay effects
8. record ledger/log
9. emit event
10. play cue
11. refresh UI
```

If the existing Core consume API commits before callback application, ensure callback failures cannot leave partially applied state.

Prefer callbacks that cannot fail after validation.

Where external authority can reject:
- preflight before consuming item;
- or add host-level transactional compensation only if repository patterns already support it.

Do not introduce a general transaction framework in Plan 22.

---

# 8. Failure Injection Matrix

## N22.1 Unknown item
Expected:
- blocked;
- no inventory change;
- no effect;
- no event;
- no cue.

## N22.2 Non-consumable item
Expected:
- blocked with specific reason.

## N22.3 Missing survivor
Expected:
- blocked;
- no fallback subject.

## N22.4 Dead survivor
Expected:
- current policy enforced; normally blocked.

## N22.5 Insufficient inventory
Expected:
- no partial direct consume.

## N22.6 Six survivors, four portions, serve all
Expected:
- honest shortage;
- no silent one-survivor result.

## N22.7 Contaminated item
Expected:
- authored positive hydration/nutrition may apply;
- authored contamination also applies.

## N22.8 Iodine ceiling reached
Expected:
- treatment blocked or sanctioned behavior;
- item not silently deleted.

## N22.9 Wrong medicine for affliction
Expected:
- distinct blocked result.

## N22.10 Kitchen prep without ingredients
Expected:
- bill rejected;
- no partial ingredient loss.

## N22.11 Kitchen prep without cook
Expected:
- blocked when duty requirement is active.

## N22.12 Refrigeration installed but power lost
Expected:
- refrigeration effect deactivates;
- spoilage projection updates.

## N22.13 Save/load mid-prep
Expected:
- job resumes exactly once.

## N22.14 Save/load mid-iodine window
Expected:
- remaining window correct.

## N22.15 Duplicate UI click/event subscription
Expected:
- one decrement only; existing panel lifecycle gates should catch double invoke.

---

# 9. Balance Acceptance

## 9.1 Direct food

Check authored hunger restore distribution.

Identify:
- weakest food;
- median food;
- strongest food.

Ensure direct raw consumption remains:
- understandable;
- useful;
- less efficient than cooked meals on average.

---

## 9.2 Water

Ensure authored thirst values produce sane daily usage.

Contaminated water must create a meaningful survival tradeoff rather than being strictly unusable or consequence-free.

---

## 9.3 Meals

Measure:

```text
ingredient value
fuel/time cost
cook-duty cost
portion count
hunger restored per ingredient
morale benefit
spoilage risk
```

Cooked meals should be a strategy, not a free multiplier.

---

## 9.4 Medicine

Measure:
- prevention effectiveness;
- treatment effectiveness;
- scarcity;
- dose ceiling;
- dependency cost;
- recovery curve.

Treatment should not dominate shielding/decontamination/prevention.

---

## 9.5 Ten-day telemetry

Run first-10-day simulation and compare before/after.

Record:
- hunger failures;
- thirst failures;
- food consumed;
- meals prepared/served;
- food wasted;
- morale;
- contaminated consumptions;
- medicine uses;
- survival outcomes.

Expect early-game curves to change because the old `-30` path is being removed. Treat that as a balance migration, not automatically a regression.

---

# 10. Performance and Allocation Guardrails

Direct consumption is low-frequency, but avoid avoidable churn.

- no catalog reload per click;
- no inventory-wide string scan every frame;
- edible-item query occurs on interaction/refresh, not `_Process`;
- cache catalog metadata where existing architecture already does;
- no repeated session construction;
- no duplicate serving-log serialization;
- no per-day reparsing of recipes.

Prepared-spoilage loops should be O(number of prepared meal entries), not O(all catalog items × meals).

---

# 11. Persistence Matrix

| State | Authority | Save requirement |
|---|---|---|
| raw item count | inventory | required |
| survivor hunger/thirst/etc. | needs | required |
| contamination dose | dose/contamination | required |
| iodine window | radiation/medical state | required |
| radiation treatment | dose ledger/treatment | required |
| prepared meal portions | kitchen | required |
| prep job progress | kitchen | required |
| spoilage timer | kitchen | required |
| serving log | kitchen | required |
| dependency state | dependency system | required |
| treatment history | ledger | required |
| grievance from unequal serving | social system | required if system persists |

Add save-roundtrip tests across authority boundaries, not just each DTO in isolation.

---

# 12. UI Acceptance Criteria

## Inventory panel

Must show:
- consumable status;
- actual consume button;
- selected survivor;
- authored effect summary where appropriate;
- success/failure reason.

---

## Holdfast terminal

Must:
- delegate to single consume authority;
- select from actual inventory;
- avoid hardcoded IDs;
- offer feed-all/serve-all semantics as designed;
- show shortage honestly.

---

## Kitchen panel

Must show:
- recipe;
- ingredient bill;
- prep status;
- portions;
- spoilage days;
- preservation status;
- selected survivor;
- serve/serve-all;
- shortage;
- cook-duty dependency.

---

## Medical panel

Must show:
- selected survivor;
- applicable treatment;
- treatment history/timeline;
- active iodine/clearance window;
- ceiling/limit usage;
- specific blocked reason;
- triage priority/recommendation.

---

# 13. Static / CI Gates

Recommended targeted gates:

1. no hardcoded terminal food ID array;
2. no direct terminal `Needs.Modify(... Hunger ...)` consume logic;
3. no direct terminal trade-ledger item removal for EAT/DRINK;
4. player inventory consume action registered;
5. direct consumption requires subject;
6. medical consumables reference valid treatment metadata where required;
7. kitchen raw ingredient IDs resolve;
8. no duplicate raw pantry stock authority introduced.

Integrate using existing source-scan/test conventions rather than new tooling stacks.

---

# 14. Test Pyramid

## Tier 1 — Core unit tests

- authored effect application;
- kitchen prep/serve/spoilage;
- medical rule calculations.

## Tier 2 — Host tests

- callback wiring;
- subject propagation;
- trade/inventory ownership;
- audio/event exactly once.

## Tier 3 — UI interaction tests

- inventory consume button;
- kitchen prep;
- serve meal;
- medical treatment.

## Tier 4 — Persistence journeys

- consume → save → load;
- prep → save → load → serve;
- iodine/treatment → save → load.

## Tier 5 — Simulation

- deterministic 10-day feeding policy;
- first-10-day telemetry;
- medical balance probes.

---

# 15. Verification Commands

Run after each task, with full suite at plan close:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --survivors-selftest
ashfall-telemetry-playtest
bash scripts/ci/triad-drift-gate.sh
bash scripts/ci/verify-fast.sh
```

Expected:
- Core tests pass;
- host build 0 errors and project-required warning threshold;
- integrity selftest 0 errors;
- bridge exits 0;
- survivors needs/dose probes pass;
- telemetry report stored as acceptance evidence;
- fast CI fully green.

---

# 16. Recommended Commit/Lane Breakdown

Keep high-risk ownership changes reviewable.

```text
22A-1 failing live-consume tests + consumable inventory table
22A-2 subject-aware InventoryHostSession callback wiring
22A-3 inventory-panel action wiring
22A-4 terminal duplicate removal + catalog-driven edible selection
22A-5 stock authority / trade transfer correction
22A-6 feed-all + events/audio + persistence/determinism

22B-1 kitchen StartPrepJob/ServeMeal UI wiring
22B-2 cellar preservation + Plan 23 refrigeration seam
22B-3 meal quality + serving log + serve-all
22B-4 grievance/duty seams + save/snapshot/telemetry

22C-1 medical consumable authority table + rule data
22C-2 iodine/radiation callback providers + dose ledger treatment records
22C-3 affliction/triage/dependency integration
22C-4 medical timeline UI + audio + persistence + class integration tests
```

Avoid mixing:
- unrelated item rebalance;
- new recipe content;
- large UI redesign;
- Plan 23 power implementation;
- Plan 24 full duty-roster redesign.

---

# 17. Risk Register

## R22.1 Early-game balance shifts

Removing hardcoded `-30` and using authored values will alter hunger curves.

Mitigation:
- telemetry before/after;
- balance authored data only after correctness is restored.

## R22.2 Double-consumption

Wiring the dead inventory handler while leaving terminal duplicate logic can cause two paths to mutate.

Mitigation:
- delete/delegate duplicate before enabling broad UI;
- one-click integration test.

## R22.3 Trade ledger semantic break

Moving EAT away from `Trade.GetHeld` may expose purchase-transfer bugs.

Mitigation:
- explicit stock-ownership test;
- fix transfer boundary, not consume path.

## R22.4 Callback partial failure

Inventory may decrement before external effect rejects.

Mitigation:
- preflight medical applicability and subject validity;
- callbacks should be non-failing after transaction begins.

## R22.5 Prepared-pantry duplication

Developers may copy raw ingredients into kitchen state.

Mitigation:
- invariant tests;
- pantry limited to prepared portions.

## R22.6 Refrigeration dependency unfinished

Plan 23 not yet landed.

Mitigation:
- wire cellar immediately;
- establish powered-refrigeration adapter contract;
- fail closed until real power signal exists.

## R22.7 Duty dependency unfinished

Plan 24 may not yet own cook assignments.

Mitigation:
- use current duty APIs where present;
- no invisible permanent auto-cook behavior.

## R22.8 Medicine complexity expansion

Adding real-world pharmacology beyond existing game systems could balloon scope.

Mitigation:
- implement only rules required by current authored medical systems/data;
- new medical content remains out of scope.

---

# 18. Final Acceptance Checklist

## 22A — Direct consumption

- [ ] pre-fix failures reproduced
- [ ] consumable catalog table generated
- [ ] stock ownership matrix generated
- [ ] inventory consume click wired
- [ ] host consume requires subject
- [ ] `applyNeed` wired
- [ ] `applyRadCleanse` wired
- [ ] `applyIodine` wired
- [ ] `applyContamination` wired
- [ ] therapeutic scale validated/documented
- [ ] terminal hardcoded hunger arithmetic removed
- [ ] terminal static food ID list removed
- [ ] direct EAT/DRINK uses shelter inventory
- [ ] trade/inventory double-count audit passes
- [ ] no literal fallback survivor
- [ ] feed-all preflight implemented
- [ ] partial-feed policy explicit
- [ ] events emitted
- [ ] exactly one cue per successful action
- [ ] save/load attribution passes
- [ ] 10-day deterministic feeding policy passes
- [ ] 22A source gates pass

## 22B — Kitchen

- [ ] kitchen state machine documented
- [ ] StartPrepJob wired from UI
- [ ] shared bill machinery used
- [ ] ServeMeal wired per survivor
- [ ] serve-all added
- [ ] cooked meal advantage uses authoritative nutrition
- [ ] cellar state wired from shelter
- [ ] powered-refrigeration seam established
- [ ] preservation updates when infrastructure changes
- [ ] spoilage visible
- [ ] spoilage event feeds briefing
- [ ] ingredients origin-neutral
- [ ] cook-duty seam established
- [ ] one prep tick owner
- [ ] unequal-service grievance link works
- [ ] medical/convalescent diet integration only where authored
- [ ] pantry stores prepared portions only
- [ ] kitchen save round-trip passes
- [ ] panel interaction/snapshot coverage updated
- [ ] 10-day kitchen telemetry accepted

## 22C — Medicine

- [ ] medical item table generated
- [ ] each supported class has authoritative rule
- [ ] iodine window/ceiling implemented
- [ ] radiation clearance path deterministic
- [ ] treatment records written to dose ledger
- [ ] wound/infection items target real affliction systems
- [ ] sick-list/triage policy integrated
- [ ] dependency system receives relevant usage
- [ ] withdrawal/stress integration tested
- [ ] prevention remains strategically stronger than treatment
- [ ] medical failures have distinct reasons
- [ ] pharma expiry only implemented if supported by authority
- [ ] medical panel timeline reads ledger
- [ ] one treatment cue exactly once
- [ ] treatment events emitted
- [ ] medical state round-trips
- [ ] one integration test per supported class
- [ ] data-integrity validation passes

---

# 19. Ship / No-Ship Gate

**SHIP** only if:

```text
raw_consumable_stock_authorities == 1
AND direct_consume_host_authorities == 1
AND terminal_hardcoded_food_list == 0
AND terminal_hardcoded_hunger_effect == 0
AND player_consume_route_connected == true
AND contaminated_consumable_effect_reachable == true
AND iodine_effect_reachable == true
AND rad_cleanse_effect_reachable == true
AND serve_meal_player_route_reachable == true
AND kitchen_raw_stock_duplication == 0
AND medicine_treatment_log_authoritative == true
AND duplicate_audio_per_action == 0
AND deterministic_10_day_policy == pass
AND save_load_consumption_journeys == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 20. Implementer Handoff

1. Fix 22A before touching meal/medical embellishment.
2. Do not modify the Core consume API unless current source proves the cited seam no longer exists.
3. Move player consumption onto shelter inventory.
4. Treat any trade-ledger dependence as an ownership defect.
5. Make the survivor subject explicit.
6. Use authored effect fields; never reintroduce code-side hunger constants.
7. Keep the kitchen pantry limited to prepared/transformed food.
8. Wire cellar now; wire powered refrigeration through Plan 23's authority.
9. Use duty roster as the cook authority; Plan 24 may deepen it.
10. Keep medicine on the same consume transaction while letting medical systems own treatment rules.
11. Preflight treatment applicability before item removal.
12. Emit events and audio only after successful commit.
13. Run persistence journeys across authority boundaries.
14. Expect early-game balance to move; correct data only after integration correctness is proven.
15. Close with first-10-day telemetry and the full CI suite.

---

# 21. Final Outcome

When this plan is complete, the question "What happens when I click EAT?" has one answer.

The item comes from the shelter inventory. The game knows which survivor consumed it. The item's authored effects—not a terminal constant—reach needs, radiation, iodine, contamination, medical, and dependency systems through the existing seams. The action is logged and audible exactly once.

The kitchen becomes the crew's real table: real ingredients become prepared portions, cook labor matters, preservation changes spoilage, and service can feed the whole crew or visibly fail.

Medicine becomes a scarce, attributable treatment decision instead of an inventory deletion. Its protection windows, dose effects, triage, dependency consequences, and treatment history all come from the systems ASHFALL already has.

The result is not a new food game. It is the existing food, kitchen, survivor, medical, trade, duty, shelter, and event systems finally behaving as one bunker machine.
