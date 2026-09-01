# Plan 22 — One Food Authority: Eating, Meds, and What the Pantry Actually Holds

> **Wave:** Continuity Wave 2 — *The Bunker Machine*
> **Depends on:** nothing to start; 21B shares its bill/recipe machinery. Feeds 23 (cold storage
> needs power) and 24 (cooks and meals are duty assignments).
>
> **Theme:** a survival game is judged on the moment the player clicks EAT. In ASHFALL that click
> draws from a **third stockpile**, on a **hardcoded item list**, subtracting a **hardcoded −30
> hunger** from **one survivor**, while the correct, data-driven, whole-crew food pipeline that
> already exists in Core is **never called**. Medicine is worse: consuming it removes it and
> applies nothing.

---

## Evidence Inventory (re-verified @ `ccac926e`)

### The good path exists and is uncalled

| Fact | Evidence |
|---|---|
| Core has a complete, data-driven consume API | `Assets/Ashfall.Core/Inventory/Inventory.cs:956–984 Consume(item, applyNeed, applyRadCleanse, applyIodine, applyContamination, therapeuticScale)` — applies authored `hungerRestore`, `thirstRestore`, `healthEffect`, `moraleEffect`, `radCleanse`, iodine blocking, and contamination dose |
| **The intended player route is a dead handler** | `src/Main.Inventory.cs:84 OnInventoryConsumeClicked(string itemId)` — `grep -rn "OnInventoryConsumeClicked" src/` returns **only the declaration**. The inventory panel's consume action is never connected to it |
| **The host wrapper that handler would call applies nothing** | `src/Host/InventoryHostSession.cs:303` — `Inventory.Consume(def, therapeuticScale: therapeuticScale)`; `applyNeed`, `applyRadCleanse`, `applyIodine`, `applyContamination` are all `null`, so the item is removed and **nothing happens**. The status line still reports success (`Main.Inventory.cs:88`) |
| The Core API works — only the host forgot | `Ashfall.Core.Tests/InventorySystemTests.cs:137` calls `inv.Consume(water, …)` **with** callbacks; the tests pass, the game doesn't consume |
| Core has a complete meal pipeline | `Assets/Ashfall.Core/KitchenNutritionSystem.cs` — `StartPrepJob` consumes a real bill from the shared inventory (`:114 TryConsumeBill`), `TickDay` advances prep and spoilage, `ServeMeal` (`:204`) decrements a portion, applies `_needs.Modify(...)` (`:221`), writes a `MealServingLog`, and raises `OnMealServed` |
| **Nothing serves meals** | `grep -rn "ServeMeal" src/` → only the host-session wrapper definition at `src/Host/KitchenNutritionHostSession.cs:49–51`. No panel, no UI handler, no day owner calls it |
| Cold storage is never configured | `SetRefrigeration` / `SetCellar` (`KitchenNutritionSystem.cs:93–100`) have **0 callers**, so `GetSpoilageDays()` (`:242–246`) always returns the `2`-day fallback no matter what the shelter has |

### The live path is a different, smaller game

| Fact | Evidence |
|---|---|
| The only EAT/DRINK buttons in the game | `src/Host/HoldfastTerminalPanel.cs:392,396` → `ConsumeFood()` / `ConsumeWater()` |
| They iterate a hardcoded item list | `HoldfastTerminalPanel.cs:212` — `string[] foodItems = { "canned_food", "ration_pack", "dried_meat", "mre" }` (ids not necessarily present in the authority; `mre`/`ration_pack` are literals in code) |
| They draw from the **trade ledger's** stock, not shelter inventory | `src/Host/HoldfastRuntimeSession.cs:233–236` — `Trade.GetHeld(itemId)` then `Trade.Inventory.RemoveItem(itemId, amount)` |
| The effect is a hardcoded constant | `:239` — `Survivors.Needs.Modify(PlayerSurvivorId, NeedKind.Hunger, -30f * amount)`, ignoring the item's authored `hungerRestore` (`canned_food` = 40, `canned_soup` = 30 per `items.json`) |
| It feeds **one** survivor | `PlayerSurvivorId` only; the crew is not involved, so "feed everyone" is unrepresentable through this path |
| A third stockpile also exists | `KitchenNutritionState.pantry` (`:13`) holds prepared meals with their own portions and spoilage, separate from inventory and from trade-held goods |
| The inventory UI is read-only about it | `src/UI/InventoryDetailPanel.cs:120` only prints `"Consume: available"` / `"not consumable"` — no action |
| Authored effects are abundant | `items.json` carries `hungerRestore`, `thirstRestore`, `healthEffect`, `moraleEffect`, `radCleanse`, `contamination`, `durability`, `radProtection` — and `ItemCatalogLoader.cs:360–370` maps the special types (`anti_rad`, `irradiated_water`, `contaminatedfood`) |
| Contaminated goods are produced but not punished | `DeepCoastHostSession.cs:245,275` yields `spoiled_canned_food` / `irradiated_water` with `contamination: 0.6f` — and the contamination callback that would apply the dose is the one passed as `null` |

**Reading:** three stocks, two consume implementations, one of them inert, one of them a
simplification, and a full authored effect table that nothing reads. This is the clearest example
in the game of *systems that each work and never meet*.

---

## Task 22A — Wire the effects: consumption must do what the data says

**Goal:** one authoritative consumption call in the host, using the existing Core API and the
authored per-item effects, for every item class (food, water, medicine, contaminants).

**Files:** `src/Host/InventoryHostSession.cs:286–310`, `src/Host/HoldfastRuntimeSession.cs:228–260`,
`src/Host/SurvivorsHostSession.cs`, `src/Host/HoldfastTerminalPanel.cs:208–235`,
`Assets/Ashfall.Core/Inventory/Inventory.cs` (read-only), `Ashfall.Core.Tests/Inventory*Tests.cs`.

### Substeps

1. **Failing test first**: consume `canned_food` and assert hunger changes by the **authored**
   40, not 30; consume `iodine_pills` and assert the iodine block flag is set; drink
   `irradiated_water` and assert a contamination dose is added. Mirror the existing
   `InventorySystemTests.cs:137` callback usage — the Core half of this is already proven.
2. **Reconnect the dead handler**: `src/Main.Inventory.cs:84 OnInventoryConsumeClicked` must be
   raised by the inventory panel's consume action (compare `_inventory.Remove` at `:80`, which *is*
   wired), so the route exists as well as works. Add a UI test asserting the panel button reaches
   the host — the reason this rotted is that no test clicked it.
3. **Give `Inventory.Consume` its callbacks** in `InventoryHostSession`: `applyNeed` → the
   survivor's `NeedsSystem.Modify`, `applyRadCleanse` → `RadiationSystem`/`DoseLedgerSystem`,
   `applyIodine` → the existing iodine-protection path, `applyContamination` → dose. All four
   parameters already exist — this is a call-site change, not an API change.
4. **Add the missing subject parameter**: consumption must name *who* consumes
   (`Consume(survivorId, itemId, scale)`); today the host has no way to express "Vasquez eats".
   Keep a backwards-compatible overload only if a selftest needs it, and mark it clearly.
5. **Delete the duplicate path**: remove `HoldfastRuntimeSession.ConsumeFood/ConsumeWater`'s
   hardcoded `-30f` arithmetic and make both delegate to the single authority. Keep the fallback
   members alive only for the documented headless-test case (`Survivors == null`, per H1's
   resolution note).
6. **Kill the literal item list**: `HoldfastTerminalPanel.cs:212`'s `{"canned_food","ration_pack",
   "dried_meat","mre"}` becomes "the first edible item in *actual* stock, ordered by preference",
   resolved from the catalog by `ItemType` — never a code-side id list. Flag any of those four ids
   that don't exist in `items.json` as authored-data drift and fix the data or delete the name.
7. **Fix the stock question explicitly**: EAT must consume from the **shelter inventory**, the same
   store the day loop decrements and the panels display. Trade-held goods are a *ledger balance*,
   not a larder — if `Trade.GetHeld` and inventory counts overlap, that is a real bug worth its own
   commit and note in `docs/systems/`.
8. **Verification of who is fed**: the terminal's crew-feeding must be able to feed N survivors and
   fail honestly when there isn't enough for everyone (currently it silently feeds one).
9. **Emit day events**: `ate`, `drank`, `med_taken`, `contaminated_meal` (17A vocabulary) so the
   briefing can explain a hunger or dose change instead of reporting a bare level.
10. **Play the existing cues**: `action_water_pour`, `action_pill_bottle`, `action_injection` are
   already registered and only fire from `SurvivorsHostSession` probes — route them through the new
   single authority so every consumption path is audible exactly once (pairs with 17C).
11. **Therapeutic scale honesty**: `therapeuticScale` already threads through `Consume`; expose it
    as an explicit parameter (dose by body mass, treatment quality) rather than a default of 1,
    and document what scales in the panel.
12. **Tests**: per-item effect table test (asserts authored numbers), who-ate-it save round-trip,
    contamination path, iodine window expiry, and one determinism test for a scripted 10-day
    feeding policy.
13. **Run the five-step verification checklist.**

**DoD:** one consume call, authored effects, correct stock, correct subject, audible, and
attributable.

---

## Task 22B — Make the kitchen the crew's table

**Goal:** connect the meal pipeline the player can see — prep jobs, portions, spoilage, service —
so a cooked meal is better than eating a tin standing up, and cooking is a real duty.

**Files:** `src/UI/KitchenNutritionPanel.cs`, `src/Host/KitchenNutritionHostSession.cs`,
`src/Main.ShelterBatch3.cs:99–110`, `src/Main.ExpandedShelterSystems.cs:248`,
`DutyRosterSystem` (cook assignment), `recipes.json`, `greenhouse`/`wildlife` outputs (read),
`SaveSectionRegistry`.

### Substeps

1. **Call the existing API**: wire `ServeMeal` from the kitchen panel (per survivor, per meal) and
   `StartPrepJob` from the SELECT flow already present at `KitchenNutritionPanel.cs:230`.
2. **Configure preservation from the shelter, not from nowhere**: `SetCellar`/`SetRefrigeration`
   must be driven by shelter state at setup time (a root cellar is a built room; refrigeration is a
   powered room → **this is Plan 23's first customer**). Until 23 lands, wire cellar status from the
   shelter room catalog so spoilage is not permanently the 2-day fallback.
3. **Meal quality must beat raw rations**: cooked meals should apply the kitchen's `nutrition`
   score (`KitchenNutritionSystem.cs:238` already records it) to hunger **and** morale, so cooking
   is worth fuel, time, and duty slots — verify the delta is meaningful but not mandatory.
4. **Serving is a duty**: cooks come from the duty roster (24), and a meal only appears if someone
   was assigned; no invisible automatic catering.
5. **Ingredients must come from the real chains**: greenhouse crops, trapping quarry, kitchen
   foraging, and scavenged cans all resolve through the same catalog types — no special-casing
   `canned_food` (as 22A step 5 requires).
6. **Spoilage is a pressure valve, not a punishment**: show "N portions spoil in D days" on the
   panel, and let the player act (serve more, preserve more, trade). Spoilage events feed 17A.
7. **Feed the whole crew explicitly**: a "serve all living survivors" action with an honest failure
   line when there are 6 mouths and 4 portions. The current silent one-survivor behaviour is the
   single most confusing thing in the terminal.
8. **Rations conflict seam**: `RationConflictSystem` / grievances already exist in the social
   coordinator; unequal serving (some fed, some not) should raise a grievance there — that is a
   free, high-value link between the food and social systems.
9. **Medical overlap**: `kitchen_nutrition` should serve sick-list and convalescent diets where
   authored (caregiving/convalescence systems exist), otherwise the ward and the pantry stay
   unrelated.
10. **Persistence**: verify `KitchenNutritionState` (jobs, pantry, serving log, flags) round-trips
    through the campaign envelope, and that the serving log is the same data the journal/briefing
    reads.
11. **Tests**: prep→serve→needs delta per authored recipe, spoilage windows with/without cellar and
    refrigeration, insufficient-food failure path, grievance raised on unequal service, save
    round-trip, snapshot for the panel.
12. **Balance**: run `ashfall-telemetry-playtest` for the first 10 days and confirm meal access is
    an early pressure rather than a background constant.
13. **Run the checklist.**

**DoD:** the kitchen is where the crew eats; raw tins are the fallback; spoilage and cook duty are
real pressures.

---

## Task 22C — Medicine is a decision, not a inventory deletion

**Goal:** make iodine, anti-rad, chelation, and bandages behave like scarce medicine with windows,
limits, side effects, and dependency — every one of those systems already exists and is unlinked.

**Files:** `Assets/Ashfall.Core/Inventory/Inventory.cs:956–984`, `ChemicalDependencySystem.cs`,
`DoseLedgerSystem.cs`, `SickListSystem.cs`, `MedicalTreatmentCatalog.cs`, `AfflictionContracts.cs`,
`VoluntaryRegisterSystem` (dose limits), `medical`/`pharma` data JSON, `src/UI/MedicalPanel.cs`,
`src/Host/SurvivorsHostSession.cs` (existing `action_pill_bottle` / `action_injection` sites).

### Substeps

1. **Enumerate the medical item classes** in the authority (`iodine_pills`, `anti_rad`,
   `rad_away`, `bandage`, `morphine`, `antibiotics`, chelation agents) and their authored
   `healthEffect` / `radCleanse` / `moraleEffect` values — write the table before changing rules.
2. **Give each class a rule, in data**: prophylaxis window (iodine blocks for N hours, with a
   per-campaign dose ceiling — thyroid saturation is the real constraint), clearance curve
   (anti-rad/chelation strips dose over time, not instantly), rebound/withdrawal (see
   `ChemicalDependencySystem`, whose relapse rules landed in Plan 09 9B), and contraindication with
   the affliction pipeline.
3. **Consume through 22A's single authority** — the `applyIodine` / `applyRadCleanse` callbacks are
   already the intended seam; implement the missing host-side providers, don't invent new ones.
4. **Tie to the dose ledger**: `DoseLedgerSystem` already records exposure; treatment must write a
   *treatment* record so the ledger explains both what you took and what it cost.
5. **Tie to triage**: `SickListSystem` bands and the ward's reservations should decide who gets
   treatment first — that is the game's signature cruelty and it currently has no supply side.
6. **Dependency and withdrawal** must bite on the same clock as the relapse rules table
   (`OnStressReported` landed in Plan 09 9B): morphine/analgesic dependency raising stress, tapering
   via the detox items/`taper recipe` authored in Plan 09 9B.
7. **Placebo honesty**: treatments must never be strictly better than prevention (decontamination,
   shielding from 20B); state the ordering in the panel text and check it in the balance sim.
8. **Fail with information**: no doses left, wrong item for the affliction, or an expired supply
   (kitchen-style spoilage applies to pharma too) each produce a distinct, authored message — the
   current `ActionResult.Blocked("cannot_consume", …)` string is not enough for medicine.
9. **UI**: the medical panel gains a per-survivor treatment timeline (taken, window remaining,
   ceiling used) reading from the ledger, not a second memory.
10. **Audio**: reuse `action_pill_bottle` / `action_injection` exactly once per treatment (they are
    already wired for probes in `SurvivorsHostSession`).
11. **Tests**: window + ceiling, clearance curve determinism, withdrawal trigger on stress,
    triage-priority treatment, save round-trip of treatment records, one integration test per class.
12. **Run the checklist** + `--data-integrity-selftest`.

**DoD:** medicine has windows, ceilings, side effects, and a triage policy — and all four are
legible.

---

## Cross-Task Dependencies

```
22A (single consume authority) ──► 22B (meals use it) ──► 22C (meds use it)
        │                                │                      │
        ├──► 17C (one cue per act)       ├──► 23 (cold storage needs power)
        └──► 17A (ate/drank/taken events)└──► 24 (cook is a duty; rations ↔ grievances)
   21B shares TryConsumeBill + recipes ◄──┘
```

**Execution order:** 22A → 22B → 22C. 22A is a prerequisite for everything: it is the one call the
other two (and Plan 21's repair bills) must go through.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. godot --headless --path . -- --survivors-selftest             # needs/dose probes
7. ashfall-telemetry-playtest (first-10-days hunger/morale funnel)
8. bash scripts/ci/triad-drift-gate.sh
9. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Core | Host | Data | UI | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|---|
| 22A | 0 (API exists) | 4 | 0–1 (stray ids) | 1 | 8–12 | Low–Med | MEDIUM (needs curves shift early game) |
| 22B | 0–1 | 2 | 1 (recipes) | 1 | 10–13 | Medium | LOW |
| 22C | 1–2 | 2 | 2 | 1 | 12–15 | Medium–High | MEDIUM |

**Guardrails:** no new consume API, no new food stocks, no new medicine subsystem, no auto-feeding.
Prefer deleting the duplicate path to preserving it "for compatibility" — the duplicate is the bug.
