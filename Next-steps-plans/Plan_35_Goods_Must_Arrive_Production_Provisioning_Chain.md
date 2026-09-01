# Plan 35 — Goods Must Arrive: The Production-to-Provisioning Chain

> **Wave:** Continuity Wave 5 — *The Human Interface* (Plans 35–39)
> **Predecessors:** [W1](Wave1_Continuity_Audit_INDEX.md) story · [W2](Wave2_Continuity_Audit_INDEX.md)
> physics · [W3](Wave3_Continuity_Audit_INDEX.md) ship · [W4](Wave4_Continuity_Audit_INDEX.md) world.
> **Depends on:** 22A (one consume authority) and 36 (the port contract that proves this plan's work).
>
> **Theme:** Wave 2 made eating and dosing real; this plan asks the obvious next question —
> **does anything the player produces ever arrive?** Greenhouse harvest does (`GreenhouseHostSession.cs:242
> InventoryHost?.Add(...)`). Trapping does not: a snare "catches" a quarry, the site records
> `hasCatch`/`catchSpecies`, the panel prints `CATCH READY (species)`, and no item is ever created.
> Water exists twice — as litres inside the treatment plant and as `clean_water` units in the
> inventory — with a bridge that is optional and a ration hook that has no caller. The game has a
> dozen producers and no shared definition of "delivered".

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Producer | Chain status | Evidence |
|---|---|---|---|
| 1 | Greenhouse / apiculture | **works** | `src/Host/GreenhouseHostSession.cs:235–247` — harvest → `InventoryHost?.Add(harvest.yieldItemId, totalAmount)`, with pollination bonus; honey/wax path at `:306` |
| 2 | **Wildlife trapping** | **broken — yields never become goods** | `Assets/Ashfall.Core/WildlifeTrappingSystem.cs:326–349` sets `site.hasCatch`, `site.catchSpecies`, `carcassYield`, `isToxic`, `isMeatProcessed`, `hidePreserved`; `:387` raises `OnButcheryCompleted`; `:409` reads `quarry.hideYield`. `grep -nE "Inventory\|Add\(" src/Host/WildlifeTrappingHostSession.cs` → **no inventory at all** (constructed as `new WildlifeTrappingHostSession(new WildlifeTrappingSystem(rng, log))`, `PanelBindLifecycleSelfTest.cs:432`). The only reader of `catchSpecies` is a panel string: `src/UI/WildlifeTrappingPanel.cs:102` |
| 3 | **Water treatment** | **two authorities, one optional bridge, one dead hook** | `WaterTreatmentSystem.cs:114,135,161 AddWater/RemoveWater/GetWater(WaterType)` (litres) coexists with inventory `clean_water` items (starting supplies add 12, `InventoryHostSession.cs:77`); the host session's inventory dependency is **nullable** (`WaterTreatmentHostSession.cs:16–21 InventoryHostSession? inventoryHost = null`); and `:471 ConsumeRation(float needed)` has **0 callers** anywhere in the repo |
| 4 | Pharma lab | **good shape, unverified end** | `PharmaLabSystem.cs:71,84` takes `Inventory.Inventory` by constructor and `:131` builds an `InventoryBill` — the right pattern; `:92 BindSkillEvaluator(Func<string,float>)` exists and (per Wave 4's 24B finding on `SetHunterSkill`) must be checked for a caller |
| 5 | Silent Foundry | **port-based, host-bound** | `SilentFoundrySystem.cs:78` "Inventory ports (wired by host…)" and `:162 BindInventory(...)` called from `src/Foundry/SilentFoundryHostSession.cs:134` — the model to generalise |
| 6 | Brine/desal, deep-coast salvage, excavation | **mixed** | `BrineWaterSystem`, `District8DeepCoastSystem` produce `VariableLootNode` bills (`DeepCoastHostSession.cs:245–275` incl. `DegradedItemId`), consumed via `TryConsumeBill` — the bill API is the project's own lingua franca |
| 7 | Combat | **the best evidence that the pattern works** | `src/Host/CombatHostSession.cs:145–153` binds eight explicit ports (damage, heal, morale, ammo, item, trauma, **`grantLoot`**, survived-marking) and `:161 ValidatePorts()` logs *"any production-required combat effects still unbound… An empty list means every health, morale, inventory, and progression effect reaches a real consumer"* |
| 8 | Kitchen | **correct** | `KitchenNutritionSystem.cs:114 TryConsumeBill(inputRequirements)` from the real inventory → pantry portions → `_needs.Modify` on serve (Wave 2's 22B wires the serving call) |
| 9 | Storage/capacity is not part of the chain | inventory has `capacity`/`maxWeight` (`Inventory.cs:CaptureState`), and `ShelterDecor`/`Excavation`/room systems exist, but no producer consults available storage before "producing" |
| 10 | Content scan can't see this class of bug | `artifacts/content-utilization.json` reports `EFFECT_PRODUCED 4` of 411 catalogs and `Actionable Priorities: 0,0,0,0,0` — a producer whose output goes nowhere is, to the gate, fully "consumed" |

**Reading:** the game already invented the right answer twice — `InventoryBill` and
`CombatHostPorts.ValidatePorts()`. Plan 35 applies the combat pattern to every producer; Plan 36
makes it a gate so it can never silently regress.

**Coordination:** the parallel `Plan_136_Wildlife_Trapping_Food_Pipeline_Cooking.md` designs new
hunting/cooking content. This plan supplies the rails it needs (a delivery contract) and should run
**first**, or 136's content will land in the same hole trapping is already in.

---

## Task 35A — A delivery contract every producer implements

**Goal:** one shape for "I made a thing, here is where it goes", with an explicit unbound-effect
check copied from combat — so no producer can quietly discard output again.

**Files:** new `Assets/Ashfall.Core/Production/IOutputSink.cs` (+ `DeliveryBill`,
`ProducerPorts.ValidatePorts()`), `WildlifeTrappingSystem.cs` +
`src/Host/WildlifeTrappingHostSession.cs`, `WaterTreatmentSystem.cs` +
`src/Host/WaterTreatmentHostSession.cs`, `PharmaLabSystem.cs`, `BrineWaterSystem.cs`,
`GreenhouseHostSession.cs`, `SilentFoundryHostSession.cs`, `Ashfall.Core.Tests/ProducerDeliveryTests.cs`
(new).

### Substeps

1. **Codify what combat already does**: `ProducerPorts { deliverItem, consumeInput, applyNeed,
   recordWaste, … }` + `UnboundRequiredEffects` + a `ValidatePorts()` that lists any effect a
   producer requires with no bound sink. Copy the comment discipline from
   `CombatHostSession.cs:157–165` — the wording there is already the project's best statement of
   this rule.
2. **Define `IOutputSink.Deliver(DeliveryBill)`** where a bill is `{itemId, amount, reason,
   sourceSystemId, day}` — the same vocabulary `TryConsumeBill` already uses for inputs, so
   production and consumption are symmetric.
3. **Refuse-with-reason instead of vanishing**: when there is no storage/weight capacity (row 9),
   delivery returns a typed failure (`storage_full`, `weight_exceeded`) that the panel and the
   briefing surface — never a silent truncation, never negative-space overflow.
4. **Fix trapping first** (clearest broken chain): butchery must deliver meat/hide/tallow via the
   sink, honouring `isToxic` → `toxinRemoved` processing (leather/`PolymerTextileCatalog` exist) and
   `hidePreserved` state; the panel then reports what arrived, not "CATCH READY".
5. **Resolve the water duplication** with a decision, not a bridge hack: either the treatment plant
   is the authority and bottled `clean_water` is a *packaging* output (recommended: litres ↔ items
   via explicit `draw`/`pour` actions, one conversion, both directions auditable), or litres are a
   buffer with a documented capacity. Write the ADR before coding.
6. **Wire or delete `ConsumeRation`** (`WaterTreatmentSystem.cs:471`, 0 callers): if the plant should
   eat crew water, call it from the day owner and emit `consumed_rations` (31); if not, delete it.
   Dead hooks are how "handled" gets assumed.
7. **Make the nullable inventory non-nullable** on host sessions that cannot function without it
   (`WaterTreatmentHostSession.cs:16`), so a missing sink is a construction error rather than a
   runtime no-op — the same lesson as Wave 2's null-callback `Consume`.
8. **Audit every producer in a table** (greenhouse, apiculture, trapping, water, brine, pharma,
   foundry, excavation, salvage, kitchen, laundry/hygiene if present): `output item | sink bound? |
   capacity respected? | event emitted? | test?`. That table is the task's deliverable as much as
   the code is.
9. **Verify the skill seam while it's in hand**: confirm `PharmaLabSystem.BindSkillEvaluator(:92)`
   and `WildlifeTrappingSystem.SetHunterSkill(:110)` are called by the day owner (Wave 4's 24B
   step 11) — a producer with a dead quality input is a chain with a broken link upstream.
10. **Emit `resource_delta` / `production_delivered` events** (31's vocabulary) so delivery is
    visible in the briefing with its source system — production you can't see is production you
    can't manage.
11. **Save/restore proof**: delivered quantities must be reconstructible from persisted state, and
    a mid-production save must not double-deliver on load (the classic repeat-on-restore bug).
12. **Tests**: per-producer delivery, capacity refusal, no-double-delivery, unbound-port detection
    (a producer registered without a sink must fail), and one soak test running 200 days asserting
    produced ≈ delivered + consumed + spoiled (the mass-balance assertion — the one that would have
    caught all of this at once).
13. **Run the five-step verification checklist** + `--data-integrity-selftest`.

**DoD:** every producer either delivers or explains itself, and a 200-day mass balance closes.

---

## Task 35B — Storage, spoilage, and the physical limits of a larder

**Goal:** make capacity, contamination, and decay first-class so "we produced a lot" can still be a
bad news line.

**Files:** `Assets/Ashfall.Core/Inventory/Inventory.cs` (capacity/weight), `ItemDefinitions.cs`
(`perishable`, `contamination`), `RefrigerationFermentationCatalog.cs` (exists, has `spoil` text),
`ShelterThermalSystem.cs`, `PowerGridSystem.cs` (23), `KitchenNutritionSystem.cs`,
`DecontaminationSystem.cs`, new `docs/systems/STORAGE.md`.

### Substeps

1. **State the model**: what limits storage — slots, weight, volume, or rooms? Pick one authority and
   write it down; the code currently carries `capacity` and `maxWeight` with no producer checking
   either.
2. **Room-scale storage**: connect cellar/fridge/pantry (kitchen's `hasCellar`/`hasRefrigeration`,
   wired from shelter rooms by 22B/23A) so *where* you store things changes how long they last.
3. **Perishables in the authority**: items with `perishable`/shelf-life data must spoil through one
   path that delivers `spoiled_*` items (they exist: `spoiled_canned_food`, `spoiled_blood_bag`) into
   inventory as real, disposable goods rather than vanishing.
4. **Contamination is a state, not a reroll**: irradiated/dirty goods must be decontaminable where
   the machinery exists (`DecontaminationSystem`) with the dose/waste bookkeeping visible.
5. **Cold chain**: refrigeration depends on power (23A step 8) with a grace window; a blackout must
   produce a *loss the player can see and attribute*.
6. **Vermin and loss**: apiculture/pest/landmark-decay systems exist; storage losses should come
   from authored causes with events, not an invisible percentage.
7. **Capacity pressure must reach decisions**: overproduction is only interesting when it costs
   something (Spoilage risk, guarding stores, trading quickly, rationing early).
8. **Show it**: the shelter/inventory surfaces report fill %, top perishables, and next expected
   loss — using the same Core functions the sim uses (no second arithmetic path, per Wave 2's rule).
9. **Mass balance again**: extend 35A step 12's assertion to include spoilage and waste terms.
10. **Balance sweep**: `ashfall-balance-sim` over production rates vs storage, checking the player is
    never punished by an unrepresentable overflow.
11. **Tests**: capacity refusal, per-category shelf life, cold-chain failure, decontamination
    conversion, save round-trip of perishables with remaining life.
12. **Run the checklist** + snapshots of full/near-full/ruined storage states.

**DoD:** larder limits are real, legible, and consequential.

---

## Task 35C — Labour, inputs, and the full loop: production is a choice, not a faucet

**Goal:** every producer must consume labour (duty roster), inputs (bills), and time, and must be
steerable — so production is decisions all the way down.

**Files:** all producer host sessions, `DutyRosterSystem.cs` (+ 24A's fitness verdict),
`CraftingSystem.cs:336–337` (replace hardcoded medical-id list with tags/types),
`ApprenticeshipSystem.cs`, `KnowledgeSystem`/`ResearchSystem`, `PowerGridSystem.cs`,
producer catalogs (`foundry_production.json`, `greenhouse_items.json`, pharma recipes),
`ShelterScheduleSystem.cs`.

### Substeps

1. **Declare the production triple for every system in data**: input bill + labour (role, hours) +
   duration → output(s), with failure/degradation branches. Anything lacking a labour term is either
   genuinely automatic (say so in the data) or a gap this task closes.
2. **Pull labour from the roster** (24's `duty_roster` owner already consumes assignments) rather
   than a per-system "who worked here" parameter; production must stop when the shift is vacated by
   illness (24C).
3. **Apply the fitness verdict**: an unfit operator degrades yield/batch quality through the same
   `BindSkillEvaluator`-style seam (35A step 9), not a bespoke penalty per producer.
4. **Respect power and heat** (23A): producers declare draw; unpowered rooms halt them with an
   attributable event rather than silently producing anyway.
5. **Batch and queue semantics**: unify "start/progress/cancel" verbs — water treatment already has
   `Preview*/Execute*/Cancel*` with `stateVersion` optimistic concurrency (`:180,219,325,340,365`),
   which is the strongest pattern in the codebase for player-triggered actions. Adopt it for the
   producers still using bare `ActionResult` calls, so double-clicks and stale panels cannot
   duplicate output.
6. **Quality tiers from the chain** (crafting, pharma, foundry, cooking): a batch inherits operator
   skill, input cleanliness, and equipment condition (21) — this is where "the same recipe, worse
   result" becomes legible.
7. **Knowledge gating**: research/knowledge unlocks *which* recipes are possible, not merely faster —
   coordinate with the parallel `Plan_141_Research_Downstream_Unlocks_Bridge` and keep this plan to
   the mechanical gate, leaving content design to it.
8. **Failure modes with warnings**: foundry incidents, batch ruin, contamination spread — each with a
   pre-warning in the briefing (31) and a recoverable cost.
9. **Steerability**: pause/reassign/prioritise must exist for the long-running producers, so
   production is managed rather than observed.
10. **UI consistency**: all producer panels should read from one read-model shape (inputs, labour,
    ETA, output, failure risk) so the player transfers knowledge between screens — the current 164
    UI files each invent their own layout language for the same concept.
11. **Tests**: labour gating, stale-version rejection (`expectedStateVersion` mismatch), unpowered
    halt, quality inheritance, queue steering, and one integration test proving a batch cannot
    deliver twice under a double click.
12. **Run the checklist** + `--expansions-selftest` + `verify-fast.sh`.

**DoD:** nothing is produced without inputs, labour, time, and a decision — and every batch can be
traced to all four.

---

## Cross-Task Dependencies

```
22A (single consume authority) ──► 35A (delivery contract mirrors it) ──► 36 (port gate)
   ├── 24A (fitness) ──► 35C step 3        ├── 23A (watts) ──► 35C step 4, 35B step 5
   ├── 21B (condition) ──► 35C step 6      ├── 31A (kinds) ──► 35A step 10, 35C step 8
   └── 27A (fixture fidelity) ──► 35A step 8's table is only trustworthy on real data
   Plan_136 (parallel: hunting/cooking content) ◄── runs AFTER 35A, on the rails
   Plan_141 (parallel: research unlocks)        ◄── 35C step 7 defines the gate only
```

**Execution order:** 35A → 36A → 35B → 35C (35A's contract is meaningless until 36 makes it
enforceable, so interleave 36A immediately after 35A).

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. godot --headless --path . -- --survivors-selftest             # needs/dose after delivery
7. godot --headless --path . -- --content-utilization-selftest   # producer catalogs: EFFECT_PRODUCED
8. mass-balance soak: 200 days, produced ≈ delivered + consumed + spoiled + refused
9. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Core | Host | Data | UI | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|---|
| 35A | 1 new + 3 | 3 | 0–1 | 2 | 12–16 | Medium | MEDIUM (item counts shift balance) |
| 35B | 2–3 | 2 | 1–2 | 2 | 10–14 | Medium | MEDIUM (perishables surprise players — warn first) |
| 35C | 4–6 | 4 | 2–3 | 3 | 14–18 | **High** | MEDIUM–HIGH (touches every producer) |

**Guardrails:** no new producer system, no new resource currency, no new panel framework, and no
capacity model invented mid-task — pick one and write the ADR. Where combat already solved a problem
(`ValidatePorts`, explicit port records), copy it instead of designing a rival.
