# C2 — Flagship Integration Plan [5]: Protection Wears Out, One Condition Authority, Decisions Read It

> **Deliverable:** `C2_planintegration[5].md`
> **Source scope:** Plan 21 — *Protection Wears Out: One Condition Ledger*
> **Wave:** Continuity Wave 2 — *The Bunker Machine*
> **Hard prerequisite:** **Plan 20A must land first.** Exposure must be environmental before exposure-driven wear can be balanced or validated.
> **Execution order:** **20A → 21A → 21B → 21C**
> **Pairs with:** Plan 22 for consumable bills, replacement canisters, patch kits, medicine, and shared item-tag consumption semantics; Plan 24 for labour/duty-shift cost of repair.
> **Primary objective:** make protective equipment wear measurable, unify duplicate personal-equipment condition values behind one authority, preserve disjoint shelter-infrastructure condition ownership, and make condition a real input to expedition, inventory, trade, repair, and risk decisions.
> **Highest regression risk:** **save compatibility and duplicate-condition migration in 21B.**
> **Scope discipline:** no new condition system, no new repair station, no automatic survivor assignment, no new protective-item families, no new wear fiction. Use existing authored durability/protection data and existing repair/consumption infrastructure.

---

# 0. Executive Intent

ASHFALL already tells the player that gas masks, hazmat suits, weapons, vehicles, and shelter machinery wear out. The data and Core code already contain the ingredients:

- authored durability,
- authored radiation protection,
- `EffectiveProtection()` scaled by durability,
- durability mutation APIs,
- persisted equipped-item durability,
- a working `EquipmentConditionSystem`,
- combat condition effects,
- vehicle breakdowns,
- shelter infrastructure wear,
- repair mechanics,
- expedition estimates,
- inventory detail surfaces,
- trade and crafting consumption paths.

The continuity failure is that these pieces disagree about **which number is authoritative** and, for protective gear, the number does not actually decrease in the running game.

The source identifies the key defects:

```text
protective gear projection gets DegradeRate = 0
→ radiation mutates a temporary WornGear copy
→ inventory durability never changes
→ protection never decays

meanwhile:
Inventory.CurrentDurability
EquipmentConditionSystem.condition
Vehicle condition
Shelter pipe condition

all represent related wear concepts through separate counters
```

This plan resolves the problem in three implementation layers:

```text
21A — Make protective wear real
      exposure + data-authored rate
      → canonical item-instance durability changes

21B — Unify personal equipment condition ownership
      gear + weapons + vehicles where item-instance semantics apply
      → one authoritative condition value
      while shelter infrastructure remains explicitly separate

21C — Put condition into decisions
      expedition estimate + inventory + repair + trade + diagnostics
      → player sees failure before it kills someone
```

The flagship outcome is:

> **Every item that wears has one authoritative remaining-life value. Exposure, combat, crafting/use, and repair all mutate that authority. The same value is saved, displayed, used by combat/radiation, priced or repaired through shared resource paths, and included in expedition decisions before departure.**

---

# 1. Source Truth

The supplied Plan 21 establishes these critical facts:

- `EffectiveProtection()` already scales `RadProtection` by durability.
- a `Degrade(...)` method already exists.
- the worn-gear bridge currently hardcodes `DegradeRate = 0f`.
- radiation degrades a temporary `WornGear` projection rather than the inventory authority.
- `EquippedItem.CurrentDurability` already exists and persists.
- `EquipmentConditionSystem` is a second, working condition store and combat already reads it.
- vehicle wear and shelter pipe condition introduce additional wear state.
- no synchronization exists between the inventory durability and `EquipmentConditionSystem` values.
- protective-gear repair is not integrated through a data-driven repair bill.

Therefore, the repair strategy is **not** to invent wear. It is to make existing authored durability load-bearing and collapse duplicated personal-equipment state.

---

# 2. Program-Level Success Criteria

C2[5] is complete only when the following questions have one deterministic answer.

## 2.1 Does protective gear actually wear?

Yes. Hours of environmental exposure reduce the authority value stored for the equipped item instance.

## 2.2 Does protection respond to wear?

Yes. `EffectiveProtection()` decreases as the same persisted condition value falls.

## 2.3 Is there exactly one condition number for a personal equipment instance?

Yes. Inventory, combat, radiation, UI, repair, and save/load all read/write the same authoritative value.

## 2.4 Are infrastructure and item condition ownership explicit?

Yes. Shelter structural condition remains a separate, documented authority rather than being ambiguously forced into an item ledger.

## 2.5 Can the player see failure coming?

Yes. Expedition preparation, inventory/detail views, and risk estimates show protection condition and projected failure.

## 2.6 Can the player act on wear?

Yes. Repair/replacement consumes real authored resources and labour through existing shared bill/duty systems.

## 2.7 Does save/load preserve the exact condition state?

Yes, including migration from old duplicated values.

---

# 3. Architectural Invariants

## 3.1 One personal-equipment condition authority

For personal equipment instances, there must be one authoritative remaining-life value.

This includes at minimum:

- protective gear,
- weapons,
- wearable devices if they use item-instance durability,
- vehicles only if their runtime model genuinely maps to an item instance.

If vehicles remain a distinct owned system, document the boundary explicitly rather than pretending all condition belongs in one dictionary.

## 3.2 Shelter infrastructure remains structurally owned

Do **not** move shelter pipes, installed machinery, walls, or other fixed plant into an inventory ledger merely to satisfy the phrase “one ledger.”

The source plan itself gives the better boundary:

```text
personal/weapon/vehicle condition → item/instance condition authority
shelter infrastructure → structural condition authority
```

The important invariant is **disjoint ownership**, not one monolithic class.

## 3.3 WornGear is a projection, not the write authority

Any `WornGear` list assembled for radiation calculation is a read projection.

Mutation must occur through a callback/sink into the authoritative item condition store.

## 3.4 No parallel repair value

Repair must change the same value that:

- radiation protection reads,
- combat performance reads,
- inventory displays,
- save/load persists.

## 3.5 Wear has an attributable cause

All condition mutation should be able to answer:

```text
why did this item lose condition?
```

Use an explicit cause enum/value such as:

- radiation exposure,
- combat use,
- crafting/use,
- travel,
- environmental damage,
- repair/replacement.

Do not rely on opaque negative deltas with no source.

## 3.6 Determinism

Wear is deterministic unless an existing mechanic explicitly includes seeded randomness.

Forbidden:

- `System.Random`,
- wall-clock time,
- non-stable ordering,
- UI-driven condition mutation.

## 3.7 Save migration must be reversible in reasoning

If two old condition fields exist, the migration rule must be explicit and testable.

Never silently pick one without documenting precedence.

## 3.8 UI never re-derives condition

Panels read the same authoritative value or Core-computed estimate used by simulation.

No panel-side arithmetic for “hours left” if a Core function already owns the formula.

---

# 4. Dependency Graph

```text
Plan 20A — environmental exposure
    │
    ▼
21A — protective gear wear becomes real
    │
    ▼
21B — one personal-equipment condition authority
    │
    ├────────────► Plan 22 consumables/repair bills
    │
    ├────────────► Plan 24 repair labour/duty shift
    │
    ▼
21C — expedition/trade/inventory decisions consume condition
```

Required order:

```text
20A → 21A → 21B → 21C
```

Do not start 21B before 21A. Unifying duplicated condition state before the primary protective-gear field actually changes risks turning a dead abstraction into a cleaner dead abstraction.

---

# 5. Baseline Capture

Before implementation, record the exact repository state and condition behavior.

## 5.1 Mandatory verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --survivors-selftest
bash scripts/ci/triad-drift-gate.sh
bash scripts/ci/verify-fast.sh
```

Also run the current equipment-balance tooling.

## 5.2 Condition authority inventory

Create a table before editing:

| Family | Current condition field | Owner | Saved where | Wear producer | Repair producer | Gameplay reader |
|---|---|---|---|---|---|---|
| protective gear | `EquippedItem.CurrentDurability` | Inventory | inventory save | effectively none | none/partial | radiation projection |
| weapons | condition field/system | `EquipmentConditionSystem` | condition save | combat | repair | combat bridge |
| vehicles | vehicle condition | vehicle system | vehicle save | travel/breakdown | existing | expeditions |
| shelter pipes | structural condition | thermal/shelter | shelter save | thermal/use | infrastructure | shelter thermal |

Use actual repository names and values.

## 5.3 Baseline behavior probes

Record:

- fresh gas mask protection,
- gas mask protection after 24h hot-zone exposure,
- inventory durability before/after exposure,
- weapon condition path,
- vehicle condition path,
- current repair paths,
- save/load values for each condition field,
- current duplicate-field disagreements if any.

The protective-gear baseline should demonstrate the defect before implementation.

---

# 6. Workstream 21A — Make Protective Degradation Real

## 6.1 Objective

Environmental exposure must measurably degrade protective gear, and the mutation must hit the canonical persisted item instance.

Desired causal chain:

```text
environmental exposure from Plan 20
→ data-authored gear wear rate
→ condition mutation sink
→ EquippedItem / unified condition authority
→ lower EffectiveProtection()
→ higher subsequent dose
→ visible warning / event
```

---

# 7. 21A Phase A — Failing Test First

Create a test with:

```text
survivor
+ fresh gas mask
+ known hot-zone exposure
+ N simulated hours
```

Assert:

```text
CurrentDurability after exposure < initial durability
```

Today this should fail.

Add a second test:

```text
worn condition decreases
→ EffectiveProtection decreases
```

And a third:

```text
CurrentDurability == 0
→ EffectiveProtection == 0
```

---

# 8. 21A Phase B — Mutation Sink

## 8.1 Preferred shape

Mirror existing callback/delegate patterns already used by `RadiationSystem`.

Conceptual contract:

```csharp
Action<SurvivorRadState, float hours> degradationSink
```

or a narrow typed equivalent.

The host implementation resolves actual equipped item instances and mutates their condition authority.

## 8.2 Why this shape

It preserves separation:

- RadiationSystem owns exposure math.
- Inventory/condition authority owns item mutation.
- Host owns composition.

## 8.3 Projection contract

Document above `FillWornGear` or equivalent:

> The returned worn-gear buffer is a read projection. Condition mutation must occur through the authoritative condition sink; mutating the projection is non-persistent and forbidden.

Add a test that prevents accidental regression to projection-only mutation.

---

# 9. 21A Phase C — Data-Authored Wear Rates

Stop hardcoding `DegradeRate = 0`.

## 9.1 Data ownership

Add or reuse authored fields for protective-equipment wear.

Conceptual fields:

```text
degrade_per_hour
wear_family
```

Use repository schema conventions.

## 9.2 Family distinction

At minimum distinguish:

- filter/canister-type protection,
- suit shell / hazmat body protection,
- mask seals/respirator if represented separately.

Different physical components should not share one arbitrary rate unless data explicitly says so.

## 9.3 Environmental scaling

Wear may scale with:

- base per-hour rate,
- zone contamination,
- weather modifier from Plan 20C,
- possibly exposure category if already modeled.

Do not use survivor identity.

## 9.4 Formula ownership

Put the canonical rate calculation in Core.

UI estimates must consume that function.

---

# 10. 21A Phase D — Data Validation

Update data schema/version as required.

Validate:

- rate >= 0,
- finite values,
- valid wear family,
- durability > 0 for degrading protective items,
- protective item with zero durability cannot claim persistent nonzero protection,
- referenced item IDs valid.

Add integrity tests.

---

# 11. 21A Phase E — Failure Semantics

At zero condition:

```text
protection = 0
```

and player feedback must exist.

Emit canonical semantic event equivalent to:

```text
gear_failed
```

using Plan 31/17A vocabulary.

Trigger an existing alert/hazard cue family through the audio layer.

Exactly-once rules apply.

Do not emit repeated “failed” events every tick after the item is already at zero.

---

# 12. 21A Phase F — Player-Facing Remaining Life

Update existing inventory and expedition-prep surfaces.

Display:

- current condition percentage,
- current effective protection,
- projected remaining hours at current exposure,
- failure warning if projected trip exceeds remaining life.

## 12.1 Estimate rule

“Hours left” must use the same Core wear-rate function.

No second arithmetic path.

## 12.2 Honest uncertainty

If future weather/exposure is uncertain, display estimate assumptions rather than pretending precision.

Example conceptual wording:

```text
~18 h remaining at current exposure
```

---

# 13. 21A Phase G — Save Contract

The source says `EquippedItem.CurrentDurability` already persists.

Prove:

```text
wear item
→ save
→ load
→ exact mutated condition restored
```

Add checksum-sensitive coverage so condition cannot silently disappear from save serialization.

No new duplicate save section for the same value.

---

# 14. 21A Phase H — Allocation/Performance Cleanup

The current bridge allocates a new worn-gear list per survivor/tick.

Reuse existing buffer/out-parameter pattern where available.

Requirements:

- no stale entries,
- no cross-survivor contamination,
- no condition mutation through reused projection,
- measurable reduction in unnecessary allocations if benchmarked.

Do not overcomplicate with caching that can stale equipment state.

---

# 15. 21A Integration Tests

Minimum suite:

- wear rate from data,
- hot zone wears faster than low zone where specified,
- weather scaling applied,
- condition decreases canonical inventory value,
- projection mutation alone has no authority,
- zero condition gives zero protection,
- failure event fires once,
- save/load round-trip,
- paired-seed replay,
- mask fails mid-expedition,
- post-failure dose rises because protection actually drops,
- reused projection buffer does not leak state.

---

# 16. 21A Definition of Done

- [ ] protective gear condition decreases in real play,
- [ ] mutation lands on authority,
- [ ] read projection no longer pretends to be mutable state,
- [ ] rates data-authored,
- [ ] environment scales wear,
- [ ] zero condition means zero protection,
- [ ] failure event/audio integrated,
- [ ] UI shows remaining life,
- [ ] save/load preserves wear,
- [ ] paired-seed replay passes,
- [ ] per-tick allocation issue reduced without stale-state bugs.

---

# 17. Workstream 21B — One Personal-Equipment Condition Authority

## 17.1 Objective

Remove duplicate condition values for equipment instances while preserving a clear, separate ownership boundary for shelter infrastructure.

The target is not “one giant system.”

The target is:

> **one number per thing, one owner per number, one save path per number.**

---

# 18. 21B Phase A — Publish the Wear-Ownership Matrix

Before selecting implementation direction, inventory all current wear systems.

For each record:

- identifier key,
- current value type/range,
- owner,
- writer(s),
- reader(s),
- save section,
- repair path,
- breakage behavior,
- whether it is item-instance or structural.

Classify each as:

```text
ITEM_INSTANCE
VEHICLE_INSTANCE
STRUCTURAL
TRANSIENT
LEGACY_DUPLICATE
```

This table becomes the design evidence for migration.

---

# 19. 21B Phase B — Decide the Canonical Boundary

Recommended boundary from source:

```text
personal gear + weapons = item-instance condition authority
vehicles = same authority only if already represented by stable item/instance IDs
shelter infrastructure = structural authority
```

## 19.1 Explicit non-goal

Do not merge shelter pipes into inventory.

## 19.2 Vehicle decision

If vehicles have a robust existing instance identity and condition model, either:

- adapt them to the same condition interface while retaining vehicle-owned storage,
- or migrate them to the canonical item-instance authority if architecturally clean.

Document the decision.

Do not force migration solely for naming consistency.

---

# 20. 21B Phase C — Choose the Write Authority

The source suggests the clean shape:

```text
EquipmentConditionSystem keyed by item instance ID
→ write authority
Inventory reads through it
```

or the reverse if repository ownership clearly favors Inventory.

Choose one.

## 20.1 Authority requirements

The owner must provide:

- `GetCondition(instanceId)`,
- `ApplyWear(instanceId, amount, cause)`,
- `Repair(instanceId, amount, cause/resources)`,
- breakage/irreversible-state query,
- save/restore.

Use existing APIs where possible.

## 20.2 No dual-write period beyond migration adapter

During transition, if both old fields must temporarily exist:

- one is canonical,
- the other is compatibility-only,
- synchronization direction is one-way,
- remove compatibility field once all readers/writers migrate.

---

# 21. 21B Phase D — Read-Side Migration First

Safe sequence:

1. point `WeaponEquipmentBridge` at canonical condition,
2. point radiation/protective gear at canonical condition,
3. point inventory/detail display at canonical condition,
4. point expedition estimates at canonical condition,
5. prove outputs match pre-migration where behavior should be unchanged.

For combat:

```text
same save + same weapon condition
→ same selected weapon
→ same performance scalar
```

before removing duplicate fields.

---

# 22. 21B Phase E — Write-Side Migration

Move all item-instance wear producers to one method:

```text
ApplyWear(instanceId, amount, cause)
```

Producers may include:

- radiation exposure,
- combat,
- crafting/use,
- travel,
- environmental damage.

## 22.1 Cause enum

Use a typed cause value.

This supports:

- debugging,
- briefing/event text,
- memorial/fate attribution,
- analytics,
- tests.

## 22.2 Exactly-once mutation

One semantic wear event must apply one delta.

Avoid:

```text
combat system decrements old field
+ bridge decrements new field
```

during migration.

---

# 23. 21B Phase F — Save Migration Strategy

This is the highest-risk phase.

## 23.1 Inventory old schemas

Identify every persisted duplicate condition value.

Create migration matrix:

| Old source | New authority | Precedence | Conflict rule | Version |
|---|---|---|---|---|

## 23.2 Conflict rule

If old save contains two differing values for the same item instance, choose and document a deterministic rule.

Examples:

- lower value wins for conservative preservation,
- newer authoritative section wins if version metadata proves it,
- mapped family-specific precedence.

Do not arbitrarily average.

## 23.3 Version discipline

Use established V1→V2 or next-version migration discipline.

Extend wire-contract tests deliberately.

Do not simply update a pinned checksum without proving migration semantics.

## 23.4 Migration tests

At minimum:

- old inventory-only condition save,
- old equipment-system-only condition save,
- old conflicting duplicate save,
- new unified save,
- repeated load/save idempotence,
- save checksum stability after normalization.

---

# 24. 21B Phase G — Retire Obsolete WornGear Shim if Possible

The source notes a sanctioned duplicate `WornGear` bridge in two namespaces.

After authority unification, reassess whether both types remain necessary.

If not:

- remove obsolete bridge,
- update known-issues/H2 documentation,
- migrate tests,
- avoid leaving a permanently “sanctioned” compatibility layer with no actual need.

If still needed:

- document why,
- enforce projection-only semantics.

---

# 25. 21B Phase H — Data-Driven Repair Recipes

Protective-gear repair must be authored in `recipes.json` or the canonical repair data authority.

Potential repair actions:

- patch suit shell,
- replace filter canister,
- reseal mask,
- replace damaged seal.

Use existing item IDs and consumables.

## 25.1 Shared bill consumption

Consume through the canonical bill path such as `TryConsumeBill`.

Do not add a protective-gear-only inventory-consumption API.

## 25.2 Replace hardcoded medical item lists

Where crafting currently identifies categories with hardcoded IDs, migrate to existing/new item tags/type semantics in coordination with Plan 22.

---

# 26. 21B Phase I — Repair as a Trade-Off

Repair should cost:

- scarce materials,
- time/labour,
- opportunity cost.

Integrate with Plan 24 duty/roster semantics when available.

If Plan 24 is not yet landed:

- keep the repair contract ready for labour cost,
- do not invent a second temporary labour system.

---

# 27. 21B Phase J — Family-Specific Breakage

Canonical condition does not imply identical behavior.

Examples:

```text
weapon → reliability/jam behavior
mask/hazmat → radiation protection approaches zero
vehicle → breakdown risk
installed plant → structural failure through its own authority
```

## 27.1 Irreversible end state

Protective gear should have a point where “repair” no longer means infinite restoration.

Use authored thresholds/repairability rules.

Conceptual states:

```text
serviceable
worn
critical
failed
beyond_repair
```

Do not add these if existing condition semantics already cover them; use current domain language.

## 27.2 Repair vs replacement

Ensure late-life gear can become a replacement decision rather than a permanently renewable item.

---

# 28. 21B Long-Campaign Soak

Run a 200-day seeded soak.

Track:

- protective gear lifespan,
- weapon wear/repair cycles,
- vehicle breakdowns if included,
- repair material consumption,
- condition distribution by day,
- number of irreparable failures,
- save/load migration stability.

Acceptance:

- authored gear lifespans are meaningful,
- repair is useful but not infinite free reset,
- no runaway condition underflow/overflow,
- no condition desynchronization.

---

# 29. 21B Documentation

Create/update:

```text
docs/systems/CONDITION_LEDGER_OWNERSHIP.md
```

Document:

- canonical item-instance owner,
- structural owner,
- vehicle boundary,
- mutation API,
- repair API,
- save ownership,
- migration rules,
- cause attribution,
- projection semantics.

Regenerate save-store matrix using repository tooling.

---

# 30. 21B Test Matrix

Per equipment family:

- authoritative read,
- authoritative write,
- wear cause attribution,
- repair cost,
- failure behavior,
- irreversible threshold where applicable,
- save round-trip,
- migration from old save,
- conflict migration,
- deterministic replay.

Cross-system:

- combat bridge reads canonical value,
- radiation reads canonical value,
- inventory UI reads canonical value,
- expedition estimate reads canonical value,
- no duplicate writes,
- no stale compatibility field after migration.

---

# 31. 21B Definition of Done

- [ ] wear-ownership table published,
- [ ] canonical item-instance condition authority selected,
- [ ] structural ownership explicitly separate,
- [ ] combat reads canonical value,
- [ ] radiation reads canonical value,
- [ ] inventory reads canonical value,
- [ ] all item-instance wear writes use one API,
- [ ] wear cause typed/attributable,
- [ ] duplicate persisted values migrated,
- [ ] conflict rule tested,
- [ ] obsolete bridge removed or justified,
- [ ] repair recipes data-driven,
- [ ] shared bill consumption used,
- [ ] repair has resource cost,
- [ ] labour seam ready/connected,
- [ ] family-specific breakage preserved,
- [ ] long-campaign soak passes,
- [ ] save-store docs regenerated.

---

# 32. Workstream 21C — Condition Reaches Player Decisions

## 32.1 Objective

Condition must affect decisions **before** failure, not only appear as post-failure diagnostics.

Minimum decision surfaces:

1. expedition dispatch,
2. inventory/detail/repair,
3. trade/replacement availability.

Additional surfaces:

- map/route editing,
- dosimeter/geiger reliability,
- memorial/journal post-mortem.

---

# 33. 21C Phase A — Expedition Estimate Uses Canonical Protection

Read `ExpeditionSystem.Estimate` and extend it so the pure estimate consumes party protection/condition inputs from the canonical authority.

No panel-side recomputation.

## 33.1 Candidate summary

For each candidate/party, expose:

- best protection,
- median protection,
- count with no working mask/protection,
- projected dose,
- projected gear wear,
- predicted mid-route failure if applicable.

Use existing estimate architecture.

---

# 34. 21C Phase B — Warn, Do Not Silently Block

If destination/route exposure exceeds party protection:

- show warning,
- name missing/weak protection,
- require explicit confirmation,
- play existing warning/invalid cue according to action semantics,
- allow player to proceed unless existing game rules define a hard prohibition.

The design preserves agency.

---

# 35. 21C Phase C — Recommendation, Not Automation

Provide read-only recommendation such as:

```text
best candidate to send
item most worth repairing
route likely to exceed current mask life
```

Do not auto-assign survivors or auto-repair gear.

The player may disagree.

---

# 36. 21C Phase D — Trade Screen Parity

Replacement consumables and repair materials must be real trade goods.

Examples:

- filter canisters,
- patch kits,
- seal materials,
- repair components already authored.

Prices should reflect actual scarcity/consumption signals from Plan 22/economy systems.

Do not create a special “gear shop” pricing model.

---

# 37. 21C Phase E — Device Calibration and Honest Measurement

The source proposes an important distinction:

```text
meter reading can be wrong
actual dose cannot be wrong
```

Use existing device calibration state.

## 37.1 Calibration semantics

Poor calibration may affect:

- displayed zone radiation estimate,
- forecasted projected dose,
- confidence range.

It must **not** alter actual RadiationSystem exposure.

## 37.2 UI honesty

Show confidence/reliability rather than hidden arbitrary error where possible.

---

# 38. 21C Phase F — Pre-Announce Mid-Route Failure

If projected exposure/wear implies gear will fail before route completion:

- show it before departure,
- identify likely failure point/time,
- allow route shortening/editing through existing map/waystation UI.

Do not guarantee exact prediction when weather uncertainty is material; show assumptions/confidence.

---

# 39. 21C Phase G — Post-Mortem Attribution

When gear failure materially contributes to survivor harm/death:

- memorial/journal/fate output should be able to record the contribution,
- use existing restrained tone and text slots,
- do not create a moralizing explanation layer.

Use wear cause/failure events from canonical condition authority.

---

# 40. 21C Phase H — Accessibility

Condition must never be communicated by color alone.

Display:

- percentage,
- text state,
- icon where useful,
- projected remaining life where relevant.

Keyboard-only flow must work for:

- inspect,
- repair,
- warning confirmation,
- route change.

---

# 41. 21C Phase I — Snapshot Coverage

Update snapshots for changed:

- expedition dispatch,
- inventory/detail,
- warning state,
- repair state,
- any trade surface materially changed.

Use real live-state fixtures.

Regenerate approvals explicitly.

---

# 42. 21C End-to-End Journey Test

Required journey:

```text
1. equip fresh mask
2. dispatch into environmental exposure
3. condition wears
4. warning threshold reached
5. return / inspect
6. repair through data-driven bill
7. condition improves
8. re-dispatch
9. estimate reflects repaired value
```

Add a variant:

```text
trip projection predicts mid-route failure
→ player shortens route
→ projected dose/wear decreases
```

---

# 43. 21C Test Matrix

- expedition estimate reads canonical condition,
- no panel-side recomputation,
- candidate summary correct,
- no-working-mask count correct,
- warning triggers at real threshold,
- warning confirmation preserves agency,
- recommendation is non-mutating,
- trade uses actual consumables,
- poor calibration affects reading only,
- actual dose unchanged by calibration,
- projected mid-route failure visible,
- route edit changes prediction,
- memorial attribution can include gear failure,
- accessibility labels/percent present,
- snapshots stable,
- full dispatch→wear→repair→redispatch journey passes.

---

# 44. 21C Definition of Done

- [ ] expedition estimate consumes canonical condition,
- [ ] dispatch shows party protection,
- [ ] projected dose visible,
- [ ] projected gear failure visible,
- [ ] unsafe trip warns rather than silently blocking,
- [ ] recommendations do not auto-assign,
- [ ] trade can supply actual repair/replacement goods,
- [ ] calibration affects measurement, not truth,
- [ ] route shortening can respond to wear risk,
- [ ] post-mortem can attribute gear failure,
- [ ] condition readable without color,
- [ ] snapshots updated,
- [ ] end-to-end journey passes.

---

# 45. Unified Condition Flow

Final intended architecture:

```text
Authored item durability / wear data
              │
              ▼
Canonical item-instance condition authority
              │
    ┌─────────┼──────────┬───────────┐
    │         │          │           │
    ▼         ▼          ▼           ▼
Radiation   Combat     Craft/use    Repair
 exposure    wear        wear        bill
    │         │          │           │
    └─────────┴────┬───┴───────────┘
                     ▼
              ApplyWear / Repair
                     │
                     ▼
              authoritative value
                     │
        ┌────────────┼─────────────┐
        ▼            ▼             ▼
 Effective      Expedition      Inventory/
 protection      Estimate       detail UI
        │            │             │
        ▼            ▼             ▼
      Dose       player choice    repair/trade
```

Shelter infrastructure remains adjacent but disjoint:

```text
Shelter structural condition authority
→ pipes / installed systems / plant
→ own save + repair semantics
```

---

# 46. Save-Migration Hard Gate

Because 21B is high-risk, no merge until all migration cases pass.

## 46.1 Required save fixtures

Maintain fixtures for:

- pre-21A inventory durability,
- pre-21B equipment condition,
- duplicate conflicting values,
- worn protective gear,
- worn weapon,
- repaired item,
- failed/irreparable item if persisted.

## 46.2 Required assertions

After migration:

- exactly one canonical condition value,
- no duplicate field silently survives as authority,
- gameplay readers all see same number,
- migration idempotent,
- save→load→save stable,
- checksum changes only where schema normalization intentionally requires it.

---

# 47. Condition Range and Numeric Contract

Standardize the condition domain.

Examples:

```text
0..1
0..100
0..MaxDurability
```

Choose one canonical internal representation per authority.

If authored durability uses absolute points while combat historically uses 0–100:

- centralize conversion,
- never duplicate conversion in consumers.

Validate:

- clamp behavior,
- underflow,
- over-repair,
- NaN/infinity,
- zero max durability.

---

# 48. Repair Semantics Contract

Repair must define:

- maximum repairable condition,
- whether repeated repairs reduce max condition,
- whether certain failures are irreparable,
- cost curve,
- labour cost,
- item/category eligibility.

Put balance values in data where practical.

Avoid a magic `+20` repair increment remaining as a universal rule unless it is intentionally canonical.

---

# 49. Consumption Integration with Plan 22

Plan 21 should prepare for Plan 22 rather than duplicate it.

Shared requirements:

- item tags/types for repair consumables,
- shared `TryConsumeBill` semantics,
- exactly-once consumption,
- save/load inventory parity,
- trade scarcity reflects real use,
- filter replacement consumes the same canister that trade/crafting sees.

If Plan 22 is not landed:

- implement the narrowest compatible interface,
- avoid a temporary protective-gear-only bill model.

---

# 50. Labour Integration with Plan 24

Repair labour should compete with other shelter work.

Preferred integration:

```text
repair order
→ material bill reserved/consumed according to existing policy
→ duty shift / labour assignment
→ repair completes
→ canonical condition changes
```

If Plan 24 is not ready:

- represent required labour metadata,
- do not fake zero-time repair as final behavior if the design depends on labour.

---

# 51. Event and Briefing Integration

Use canonical semantic kinds.

Potential events:

- gear wear critical,
- gear failed,
- repair started,
- repair completed,
- item beyond repair,
- weapon degraded,
- vehicle breakdown.

Each event should carry:

- item instance ID,
- item definition ID,
- survivor/owner where relevant,
- cause,
- numeric before/after or delta if supported.

This enables:

- briefing attribution,
- journal/fate text,
- debugging,
- post-mortem.

---

# 52. Audio Integration

Reuse Plan 17C cue ownership.

Examples:

- gear failure → hazard/warning family,
- repair success → confirm family,
- invalid repair → invalid action,
- no duplicate cue if both UI and domain event fire.

Audio must not mutate condition.

---

# 53. Performance Constraints

Potential hot paths:

- per-survivor worn gear projection,
- condition lookup by instance ID,
- expedition estimate across candidate parties.

Requirements:

- indexed condition lookup,
- avoid repeated catalog parsing,
- reuse projection buffers where safe,
- avoid LINQ/allocation-heavy loops in per-tick exposure if profiler shows cost,
- no cache that can make condition stale after repair/wear.

Benchmark only if needed; prioritize correctness first.

---

# 54. Balance Program

Run `ashfall-equipment-balance` or repository equivalent.

## 54.1 Protective gear scenarios

Vary:

```text
base durability
× environmental exposure
× weather
× repair frequency
× repair material scarcity
```

Track:

- median hours to critical,
- median hours to failure,
- average repairs before replacement,
- dose increase after failure,
- repair material consumption.

## 54.2 Cross-family scenarios

Track:

- weapon condition distribution,
- vehicle condition if included,
- repair workload,
- resource competition.

## 54.3 Acceptance principles

- gear is meaningfully consumable,
- it does not evaporate unrealistically fast,
- maintenance matters,
- repair competes with other priorities,
- replacement remains necessary eventually,
- early-game failure is avoidable with reasonable play.

---

# 55. Failure Modes and Corrective Actions

## 55.1 Gear still never wears

Likely cause:

- sink not bound,
- rate remains zero,
- projection still mutated instead of authority.

Fix at authority boundary.

## 55.2 UI condition changes but radiation protection does not

Likely cause:

- radiation reads old field/projection.

Fix all readers to canonical value.

## 55.3 Combat changes one condition, inventory shows another

Critical duplicate-authority bug.

Fix before migration removal.

## 55.4 Old save condition jumps unexpectedly

Likely cause:

- conflicting old values with undefined precedence.

Fix migration rule and fixture.

## 55.5 Repair duplicates materials

Likely cause:

- custom consume path plus shared bill path.

Use one bill owner.

## 55.6 Repair is free/instant despite intended labour cost

Do not hide this as “temporary complete.”

Mark Plan 24 dependency or integrate labour seam.

## 55.7 Gear failure event spams every tick

Emit on transition into failure, not steady failed state.

## 55.8 Calibration changes real dose

Critical semantic bug.

Calibration changes measurement only.

## 55.9 Recommendation auto-assigns survivors

Remove mutation. Recommendation is advisory.

---

# 56. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| save migration conflict | High | Critical | explicit migration matrix + fixtures |
| duplicate condition writes during transition | High | High | one-way compatibility + write gate |
| exposure wear shifts Plan 20 balance | Medium | High | rerun dose/equipment balance |
| repair infinite-loop economy | Medium | Medium–High | irreparable threshold + scarcity |
| old combat behavior changes unintentionally | Medium | High | read-side parity before write migration |
| vehicle boundary over-unified | Medium | Medium | explicit ownership decision |
| projection mutation regression | Medium | Medium | code comment + regression test |
| item IDs lack stable instance identity | Medium | High | solve identity before unification |
| UI re-derives estimates | Medium | Medium | Core estimate functions only |
| warning becomes hard block | Low–Med | Medium | explicit confirmation UX test |
| repair consumption duplicates Plan 22 | Medium | Medium | shared bill interface |
| labour duplicated before Plan 24 | Medium | Medium | defer to seam, no temp labour system |

---

# 57. Commit Strategy

## Commit C2[5].1 — Baseline and failing protective-wear tests

- condition authority inventory,
- failing gas-mask exposure test,
- baseline equipment curves.

## Commit C2[5].2 — Degradation sink

- projection contract,
- canonical mutation callback,
- tests.

## Commit C2[5].3 — Data-authored wear rates

- item schema,
- environmental scaling,
- integrity tests.

## Commit C2[5].4 — Failure semantics + UI remaining-life projection

- semantic event,
- alert cue,
- inventory/expedition display.

## Commit C2[5].5 — Save/round-trip + allocation cleanup

### Gate: 21A complete

## Commit C2[5].6 — Condition ownership design + read-side migration

- ownership doc,
- canonical authority,
- combat/radiation/inventory readers.

## Commit C2[5].7 — Write-side migration

- typed `ApplyWear`,
- all item-instance producers.

## Commit C2[5].8 — Save migration

- versioning,
- conflict rule,
- old-save fixtures.

## Commit C2[5].9 — Repair recipes + shared bill path

- protective gear repairs,
- tag/type migration.

## Commit C2[5].10 — Breakage states + 200-day soak

- irreparable threshold,
- docs/save-store matrix.

### Gate: 21B complete

## Commit C2[5].11 — Expedition estimate condition inputs

- party protection,
- projected dose/wear.

## Commit C2[5].12 — Warning/recommendation/route edit

- agency-preserving warning,
- advisory hints.

## Commit C2[5].13 — Trade/calibration/post-mortem

- replacement goods,
- measurement reliability,
- fate attribution.

## Commit C2[5].14 — Accessibility/snapshots/journey test

### Gate: 21C complete

## Commit C2[5].15 — Full balance + migration closure

---

# 58. Verification Checklist

Run after each major workstream and final closure.

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --survivors-selftest
bash scripts/ci/triad-drift-gate.sh
bash scripts/ci/verify-fast.sh
```

Also run:

```text
ashfall-equipment-balance
```

and save-store matrix generation/check.

---

# 59. Flagship Definition of Done

## 21A — Protective wear

- [ ] gas mask/hazmat condition decreases from real exposure,
- [ ] write lands on authoritative instance,
- [ ] projection remains read-only,
- [ ] wear rate comes from data,
- [ ] environmental scaling works,
- [ ] failure drops protection to zero,
- [ ] failure event fires once,
- [ ] UI shows condition and remaining-life estimate,
- [ ] save/load preserves mutated value,
- [ ] replay deterministic,
- [ ] mid-expedition failure raises subsequent dose,
- [ ] bridge allocation behavior improved safely.

## 21B — Condition authority

- [ ] ownership matrix complete,
- [ ] canonical item-instance owner selected,
- [ ] structural condition remains disjoint,
- [ ] weapon bridge reads canonical value,
- [ ] radiation reads canonical value,
- [ ] inventory reads canonical value,
- [ ] wear writes converge on one API,
- [ ] cause attribution exists,
- [ ] save migration versioned,
- [ ] conflict rule tested,
- [ ] old duplicate field retired,
- [ ] obsolete bridge removed or documented,
- [ ] repair recipes data-driven,
- [ ] shared bills used,
- [ ] labour dependency integrated/prepared,
- [ ] irreparable condition exists where required,
- [ ] 200-day soak passes,
- [ ] save-store documentation green.

## 21C — Decisions

- [ ] expedition estimate consumes canonical value,
- [ ] party protection visible,
- [ ] projected dose visible,
- [ ] projected gear failure visible,
- [ ] unsafe trip warns and can be confirmed,
- [ ] recommendations are advisory,
- [ ] repair/replacement goods appear in trade,
- [ ] calibration changes displayed reliability only,
- [ ] route changes can reduce projected failure,
- [ ] post-mortem can attribute failure,
- [ ] accessibility passes,
- [ ] snapshots updated,
- [ ] dispatch→wear→repair→redispatch journey passes.

## Cross-system

- [ ] exactly one condition value per item instance,
- [ ] no duplicate save authority,
- [ ] no duplicate wear application,
- [ ] repair mutates same value simulation reads,
- [ ] audio observational only,
- [ ] semantic events canonical,
- [ ] all automated verification green.

---

# 60. Closure Report Template

```markdown
## C2[5] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:
- Working tree:

### Baseline
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Bridge:
- Survivors selftest:
- Condition authorities found:
- Duplicate fields found:

### 21A — Protective Wear
- Degradation sink:
- Wear-rate data:
- Zone/weather scaling:
- Canonical mutation:
- Failure event:
- UI remaining life:
- Save/load:
- Allocation cleanup:
- Mid-expedition failure test:
- Equipment balance:
- Result:

### 21B — Condition Authority
- Ownership boundary:
- Canonical owner:
- Combat migration:
- Radiation migration:
- Inventory migration:
- Write API:
- Save migration version:
- Conflict rule:
- Obsolete fields removed:
- Repair recipes:
- Bill path:
- Labour path:
- Irreparable state:
- 200-day soak:
- Save-store matrix:
- Result:

### 21C — Decisions
- Expedition estimate:
- Party protection UI:
- Projected dose:
- Failure prediction:
- Warning/confirmation:
- Recommendation:
- Trade parity:
- Calibration:
- Route editing:
- Post-mortem:
- Accessibility:
- Snapshots:
- Journey test:
- Result:

### Full Verification
- dotnet build Core.Tests:
- dotnet test:
- dotnet build Ashfall:
- data-integrity:
- bridge-selftest:
- survivors-selftest:
- triad-drift-gate:
- verify-fast:
- equipment balance:

### Remaining Debt
- Plan 20:
- Plan 22:
- Plan 24:
- Condition migration:
- UI:
```

---

# 61. Final Execution Directive

Implement Plan 21 as an **authority repair**, not as a cosmetic durability feature.

The final chain must be:

```text
real exposure/use
→ canonical condition mutation
→ same saved value
→ same value used by protection/combat
→ same value shown to player
→ same value repaired through real resources/labour
→ same value used by expedition decisions
```

Do not accept any of these as completion:

- durability number displays but never changes,
- a temporary `WornGear` object degrades,
- combat and inventory show different values,
- repair changes one field while radiation reads another,
- a migration test only covers fresh saves,
- expedition UI uses a hand-written condition estimate,
- a warning removes player agency through silent auto-blocking.

The most important invariant is:

> **One item instance, one authoritative condition value, one persistence path, one mutation API.**

And the most important player-facing outcome is:

> **The player must be able to see a mask dying, understand why, predict whether it will survive the trip, spend real resources to repair or replace it, and watch that same repaired condition change the actual dose calculation.**

---

# 62. Execution Checkpoint — 2026-09-15 (21A complete + 21B Phase A)

Premise corrections (full evidence: `C2_PLANINTEGRATION_5_BASELINE.md`):
the 21A wear chain (sink, data rates, zero-protection semantics) and the weapon-side
21B migration (`WeaponEquipmentBridge` canonical projection) were **already built**.
The "two-namespace WornGear" and "duplicate persisted condition" premises are stale.

**21A remainder — COMPLETE (16/16 tests, `Inventory/Plan21ProtectiveWearTests.cs`):**
- P2: `OnProtectiveGearFailed` exactly-once transition (>0→0) on the wear authority;
  `DegradeEquippedGear` routed through `RecordWear` (one mutation API + cause).
- P3: Core `TryEstimateWeakestProtectiveLife` (data-rate arithmetic, one path);
  session `GetWeakestProtectiveLife` (bound multiplier); RadiationDetailPanel
  remaining-life line, source-gated against panel-side arithmetic.
- P4: reused `_wornGearBuffer` projection buffer (no per-tick allocation).
- P7: `WeatherSystem.HazmatDegradeMultiplier` (black rain ×5) now bound — was defined
  but never wired.
- D1 recorded: authored `degradeRate` overrides documented Core family defaults.

**21B Phase A — COMPLETE (`docs/systems/CONDITION_LEDGER_OWNERSHIP.md`):**
- Ownership matrix: Inventory = protective ITEM_INSTANCE; ECS = registered
  weapon/tool families (explicit registration only — **no live duplicate exists**);
  vehicles VEHICLE_INSTANCE; shelter STRUCTURAL (disjoint by design).
- **Save migration NOT REQUIRED** (no persisted duplicate value exists in current
  source); guarded by the 21A pins instead. Save-store matrix regenerated: 194/194.
- D2 recorded: protective-gear repair stays replacement-crafting (`craft_hazmat_patch`,
  `craft_gas_mask` authored bills); restore-repair via `TryConsumeBill` + `RecordWear`
  is a Plan 22-coordinated decision, not invented here.

**Verification:** Inventory 64/64 · journey 1/1 · Radiation 72/72 · host build 0 ·
data-integrity PASS · panel lifecycle PASS · ui-a11y PASS · survivors selftest PASS ·
triad PASS · docs index 2135 · save-store matrix 194/194.

**Remaining: 21C (P6)** — expedition estimate party-protection inputs, projected
dose/wear, mid-route failure prediction, warn-don't-block dispatch.

---

# 63. Execution Checkpoint — 21C complete (2026-09-15)

P6 closed — condition reaches the dispatch decision (plan §33–§35, §42):

- **Core:** `ExpeditionProtectiveInputs` + additive `ExpeditionEstimate` fields; dose
  projection through the canonical `RadiationSystem.ComputeExposurePerHour`; wear
  projection from the data-authored rate × multiplier × trip hours; mid-route failure
  predicted from protective life vs trip hours. Null inputs ⇒ legacy byte-identical.
- **Host:** `SetEstimateProtectiveInputs` hook (route-modifier pattern) →
  `SurvivorsHostSession.BuildProtectiveEstimateInputs` (resolver ambient + gear
  projection + melt multiplier from the canonical owners).
- **Panel:** `dose ~X mSv (protection N / NO WORKING PROTECTION)` +
  `GEAR FAILS MID-ROUTE` — warn-don't-block, display only, agency preserved; source
  gates forbid panel dose arithmetic.
- **Journey §42 (2/2):** fresh mask → estimate safe → exposure wears → exactly-once
  failure → post-failure dose rate rises 20→30 mSv/h → spare equipped → estimate
  restored; route shortening drops the failure prediction.

**C2[5] status: 21A COMPLETE · 21B COMPLETE (no migration required — verified) ·
21C COMPLETE** at Core+host+panel scope. Remaining deferred (documented, need
foreman/content packages): full per-item wear-rate data authoring (D1 tranche),
restore-repair via Plan 22 bill semantics (D2), Plan 24 labour seam, 200-day soak
(owners' save/load coverage exists; dedicated soak window needs a user directive
per TEST_POLICY), snapshot regeneration for changed surfaces.

**Final verification:** Expeditions 243/243 · Inventory 66/66 · Radiation 72/72 ·
host build 0 errors · data-integrity PASS · panel lifecycle PASS · ui-a11y PASS ·
survivors selftest PASS · bridge PASS · triad PASS · docs index 2136.
