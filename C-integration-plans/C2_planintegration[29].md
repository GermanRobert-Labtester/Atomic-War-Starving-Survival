# C2 — Flagship Integration Plan [29]: Clothing Warmth, Layered Cold Protection, Wetness, and Nuclear-Winter Gear Progression

> **Deliverable:** `C2_planintegration[29].md`
> **Source scope:** Plan 142 — *Clothing & Warmth Gear Progression*
> **Primary objective:** make equipped clothing materially reduce cold exposure and warmth decay by projecting item-authored insulation/cold-resistance data, equipment slots, condition, wetness, and environment into the canonical survivor warmth/needs calculation—without creating a duplicate durability ledger or a second equipment authority.
> **Required execution order:** **142A Foundation/System Contract → 142B Clothing Content, Layering, Wetness & Progression → 142C Integration, Save/CI, Balance, and Player-Facing Closure**
> **Hard dependencies:** `NeedsSystem`, `Inventory.Equip()`, equipment/loadout authority, `EquipmentConditionSystem`, item catalog, Plan 20/135 weather/exposure inputs, expedition/travel cold-zone logic, trapping/crafting/research/economy systems where present, Plan 21 condition authority, Plan 25 localization, Plan 31 semantic events, Plan 39 save durability.
> **Scope discipline:** no second durability field as runtime truth, no clothing-warmth save state that can be derived from equipment/item state, no hardcoded item-ID lists where tags/properties suffice, no weather-owned wetness duplicated here, no clothing that grants permanent 100% cold immunity, no layering exploit beyond configured slots/caps, and no UI number that disagrees with the exact modifier used by `NeedsSystem`.

---

# 0. Executive Intent

ASHFALL already has:

- survivor warmth need,
- equipped item handling,
- item definitions,
- radiation protection aggregation,
- equipment durability/condition,
- weather and cold pressure,
- expedition risk,
- crafting/research/trade content.

The missing connection is simple but strategically important:

```text
equipped clothing
→ has no effect on warmth decay
```

That undermines one of the game’s central survival loops.

The desired architecture is:

```text
equipped clothing items
        │
        ├─ authored insulation
        ├─ authored cold resistance
        ├─ slot/layer class
        ├─ condition
        └─ wetness
        │
        ▼
ClothingWarmthSystem
        │
        ▼
WarmthProtectionSnapshot
        │
        ├─ effective insulation
        ├─ effective cold resistance
        ├─ wetness penalty
        ├─ overheating pressure
        └─ explanation terms
        │
        ▼
NeedsSystem.ApplyWarmth()
        │
        ├─ environment/cold load
        ├─ shelter heat source
        └─ clothing mitigation
        │
        ▼
survivor warmth outcome
```

The product-level outcome is:

> **A survivor wearing a maintained, dry, layered cold-weather outfit loses warmth meaningfully slower than an unprotected survivor, while wetness, damaged gear, heavy layering, and cold intensity preserve real tradeoffs.**

---

# 1. Source Diagnosis

The source establishes:

- `NeedsSystem.ApplyWarmth()` currently checks only shelter-level heat proximity,
- equipped clothing is ignored,
- equipment condition already exists,
- `Inventory.GetTotalRadProtection()` provides a precedent for equipment-derived protection,
- cold survival has no gear progression,
- the plan proposes:
  - warmth bonus,
  - cold resistance,
  - wetness,
  - durability interaction,
  - 4-layer structure,
  - 20 clothing profiles,
  - expedition cold checks,
  - crafting/trade/research hooks,
  - tutorial/tooltips,
  - deterministic headless validation.

The source also proposes `ClothingWarmthState` containing equipped warmth and durability.

That should be corrected architecturally:

```text
equipped clothing + item warmth profile + EquipmentConditionSystem + wetness
→ derived warmth projection
```

not:

```text
duplicate persisted warmth/durability truth
```

The system only needs to persist state that is not derivable elsewhere, primarily wetness if no canonical equipment wetness owner already exists.

---

# 2. Program-Level Success Criteria

C2[29] closes only when:

1. Equipped clothing affects survivor warmth loss.
2. Warmth protection is calculated from real equipped items.
3. Item warmth values are authored in data, not hardcoded in C#.
4. Clothing condition modifies warmth through the canonical condition authority.
5. Wet clothing loses insulation through one explicit wetness state.
6. Drying uses real heat/environment state.
7. Layering follows actual equipment-slot rules.
8. Layering has a hard cap or diminishing-return model.
9. Excess insulation can create overheating/fatigue pressure where appropriate.
10. No clothing combination creates unconditional cold immunity.
11. Shelter heat and clothing mitigation combine through one canonical formula.
12. Expedition/cold-zone checks use the same warmth projection as needs.
13. UI uses the same calculation as runtime.
14. Old saves load without a new duplicate equipment state requirement.
15. Save/load preserves only genuinely stateful wetness/cooldowns where needed.
16. Clothing repair goes through the existing condition/repair system.
17. Clothing crafting/trade/research uses canonical recipes/items/economy.
18. 20 clothing warmth profiles validate against the real item catalog.
19. Headless CI proves dry/wet/damaged/layered edge cases.
20. Balance testing proves cold remains dangerous but manageable.

---

# 3. Architectural Invariants

## 3.1 Item catalog owns static warmth properties

Each clothing item definition owns:

- warmth contribution,
- cold resistance,
- layer/slot category,
- wetness sensitivity,
- optional mobility/work modifiers.

## 3.2 Equipment system owns what is worn

`ClothingWarmthSystem` never owns equipped-item identity.

## 3.3 EquipmentConditionSystem owns durability

Do not persist a second `durability` inside warmth state.

## 3.4 ClothingWarmthSystem owns projection only

It may own:

- calculation,
- wetness status if no canonical owner exists,
- read-model/explanation.

## 3.5 NeedsSystem owns survivor warmth need

Clothing system never writes warmth directly except through the established mitigation input seam.

## 3.6 Weather/environment owns external cold/wet pressure

Clothing reacts to rain/snow/cold.
It does not invent the weather.

## 3.7 Wetness is state, not a permanent item mutation unless intentionally designed

Wetness can dry.
Condition damage is separate.

## 3.8 UI and runtime share one formula

No duplicated tooltip math.

## 3.9 Item IDs are not generic behavior switches

Use item tags/properties/slots.

## 3.10 Protection remains bounded

No 100% cold elimination through stacking.

---

# 4. Dependency Graph

```text
item catalog
   │
   ├─ warmth profile
   ├─ layer class
   └─ wet sensitivity
   │
   ▼
Inventory / equipment
   │
   ├────────────► EquipmentConditionSystem
   │
   └────────────► wetness state
                        │
                        ▼
               ClothingWarmthSystem
                        │
                        ▼
              WarmthProtectionSnapshot
                        │
          ┌─────────────┼──────────────┐
          ▼             ▼              ▼
       NeedsSystem   Expedition      UI/tooltips
          │
          ▼
 survivor warmth / cold survival
```

Supporting systems:

```text
weather → wetting/cold load
heat source → drying/need mitigation
crafting/research → gear progression
trapping → hide material
trade/economy → acquisition
```

---

# 5. Baseline Capture

Before implementation, record:

- `NeedsSystem.ApplyWarmth()` current formula,
- current warmth-decay constants,
- current heat-source behavior,
- equipment slots,
- `Inventory.Equip()` contract,
- radiation-protection aggregation precedent,
- current item fields/tags,
- condition/durability query API,
- repair APIs,
- expedition cold-zone/readiness checks,
- crafting recipe schema,
- research gating,
- item trade/scavenge paths.

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/verify-fast.sh
```

Record baseline warmth loss for:

```text
no clothing
near heat
outside cold
```

for a fixed survivor/test environment.

---

# 6. Workstream 142A — Foundation / System Contract

## Goal

Create one pure clothing-protection projection from equipped gear and feed it into the canonical warmth calculation.

---

# 7. 142A Phase A — Define `ClothingWarmthProfile`

Prefer static authored fields such as:

```text
item_id
base_warmth
cold_resistance
layer_class
wet_penalty
overheat_weight
movement_penalty
work_bonus tags if relevant
```

Do not include live durability.

Live condition is queried separately.

---

# 8. 142A Phase B — Define Layer Classes

Initial vocabulary:

```text
Inner
Mid
Outer
Head
Hands
Feet
```

If the equipment system already has canonical slots, map directly to them.

Do not invent a parallel slot system.

---

# 9. 142A Phase C — Define `WarmthProtectionSnapshot`

Runtime derived read model:

```text
survivor_id
equipped_warmth_total
effective_warmth
cold_resistance
wetness_penalty
condition_penalty
layering_penalty
overheating_pressure
final_decay_multiplier
explanation_terms
```

This is not save authority.

---

# 10. 142A Phase D — `ClothingWarmthSystem`

Create:

```text
Assets/Ashfall.Core/Inventory/ClothingWarmthSystem.cs
```

Responsibilities:

- inspect equipped items,
- load static profiles,
- read condition,
- read wetness,
- compute aggregate protection,
- expose deterministic query APIs.

---

# 11. 142A Phase E — `GetWarmthProtection(survivorId)`

Prefer one rich query over multiple drifting helpers.

Example:

```csharp
WarmthProtectionSnapshot GetWarmthProtection(string survivorId)
```

`GetTotalWarmthBonus()` may remain a convenience projection.

---

# 12. 142A Phase F — No Persisted Equipped-Warmth Map

Reject a redundant save state like:

```text
survivor → equipped clothing warmth
```

because it is derivable.

Persist only non-derivable wetness state if needed.

---

# 13. 142A Phase G — Wetness State Ownership

If no existing item-environment state exists, create minimal:

```text
ClothingWetnessState
{
    survivor/item/slot key
    wetness 0..1
    last_wet_day/tick
}
```

Do not duplicate durability.

---

# 14. 142A Phase H — Wetness Persistence

Capture/restore:

- wetness only,
- deterministic ordering,
- schema version.

Old saves:

```text
missing wetness section
→ all equipped clothing dry
```

---

# 15. 142A Phase I — Warmth Formula

Define canonical formula conceptually:

```text
environmental cold load
× clothing decay multiplier
× wetness multiplier
× condition modifier
× other canonical modifiers
```

Avoid applying both:

```text
warmth bonus
and
cold resistance
```

as two overlapping reductions unless mathematically intended.

---

# 16. 142A Phase J — Separate Insulation and Cold Resistance

Recommended interpretation:

## Insulation

Reduces routine warmth decay.

## Cold resistance

Reduces environment severity/exposure term.

This preserves meaning and avoids double counting.

---

# 17. 142A Phase K — Condition Scaling

Condition source:

```text
EquipmentConditionSystem
```

Example curve:

```text
100% condition → 100% protection
50% condition → configurable reduced protection
0% condition → minimal/zero protection
```

Do not assume a strictly linear 1:1 curve without balance evidence.

---

# 18. 142A Phase L — Tier Labels

Source tiers:

```text
Tier 0 — no clothing
Tier 1 — basic
Tier 2 — warm
Tier 3 — insulated
Tier 4 — arctic
```

Use labels as presentation/readiness bands.

Do not make tier the underlying protection truth.

---

# 19. 142A Phase M — Initial Protection Bands

Source starting targets:

```text
Tier 1 ~10% decay reduction
Tier 2 ~30%
Tier 3 ~50%
Tier 4 ~70%
```

Treat as balance targets, not constants buried in code.

---

# 20. 142A Phase N — Shelter Heat Combination

Current heat-source check must become one term in the final warmth calculation.

Example:

```text
environment load
→ shelter/heat mitigation
→ clothing mitigation
→ warmth change
```

Do not stack independent hardcoded “near heat = safe” and “clothing = safe” booleans.

---

# 21. 142A Phase O — Needs Integration Seam

`NeedsSystem.ApplyWarmth()` should accept/query:

```text
WarmthProtectionSnapshot
```

or a small derived multiplier interface.

Avoid direct item scanning inside NeedsSystem.

---

# 22. 142A Phase P — Environment Input

Warmth system should accept/query canonical:

- ambient temperature,
- weather/cold severity,
- indoors/outdoors,
- shelter heat state.

Do not duplicate environmental simulation.

---

# 23. 142A Phase Q — Wetting Events

Wetness sources may include:

```text
rain
snow
flood/water exposure
sweat from heavy exertion
```

Every source comes from canonical events/state.

---

# 24. 142A Phase R — Drying

Drying rate may depend on:

- near heat source,
- shelter warmth,
- ventilation,
- elapsed time.

No instant dry just because UI opened.

---

# 25. 142A Phase S — Sweat Wetness

Sweating should be driven by:

- high exertion,
- excessive insulation,
- warm environment.

Do not add a complex thermoregulation simulator unless needed.

A bounded overheat/sweat projection is enough.

---

# 26. 142A Phase T — Overheating Pressure

Too much insulation in warm/active contexts may add:

- fatigue,
- work inefficiency,
- wetness risk.

Use existing fatigue/work modifier channels.

---

# 27. 142A Phase U — Determinism

Warmth protection is pure from:

```text
equipment
condition
wetness
environment
```

No RNG.

---

# 28. 142A Phase V — Catalog Loader

Create:

```text
ClothingWarmthCatalogLoader
```

or integrate into canonical item loader if item schema can own these fields directly.

Prefer avoiding a second catalog if `items.json` can safely carry warmth properties.

---

# 29. 142A Phase W — Single Data Authority Decision

Choose one:

## Preferred

Add warmth fields directly to `items.json`.

## Alternate

Separate `clothing_warmth_profiles.json` keyed by item ID.

Do not maintain both.

---

# 30. 142A Phase X — Catalog Integrity

Validate:

- item exists,
- item is wearable,
- slot/layer valid,
- warmth bounds,
- resistance 0..1,
- wet penalty 0..1,
- no duplicate profile.

---

# 31. 142A Phase Y — Diagnostics

Headless diagnostic:

```text
CLOTHING_WARMTH_PROFILES
EQUIPPED_WARMTH_ITEMS
UNKNOWN_WARMTH_ITEMS
MAX_EFFECTIVE_REDUCTION
```

---

# 32. 142A Tests

- no clothing,
- one item,
- layered items,
- damaged item,
- wet item,
- near heat,
- cold outside,
- old-save dry default,
- deterministic same-state calculation,
- unknown item profile validation.

---

# 33. 142A Definition of Done

- [ ] ClothingWarmthSystem,
- [ ] profile schema,
- [ ] one data authority,
- [ ] layer mapping,
- [ ] derived protection snapshot,
- [ ] no duplicate equipped-warmth persistence,
- [ ] no duplicate durability state,
- [ ] wetness ownership,
- [ ] NeedsSystem seam,
- [ ] shelter heat combination,
- [ ] environmental input,
- [ ] deterministic formula,
- [ ] catalog validation,
- [ ] diagnostics,
- [ ] old-save default.

---

# 34. Workstream 142B — Clothing Content, Layering, Wetness & Progression

## Goal

Author a meaningful 20-item cold-weather progression and wire acquisition, repair, crafting, trade, expedition readiness, and player understanding.

---

# 35. 142B Phase A — 20-Item Content Budget

Source requires 20 profiles.

Build across:

- basic,
- warm,
- insulated,
- arctic,
- hybrid/special.

Use actual existing item IDs where possible.

Do not invent duplicates if equivalent clothing already exists.

---

# 36. 142B Phase B — Item Audit First

Before adding items, scan `items.json`.

Classify existing wearable clothing:

```text
already suitable
needs warmth metadata
missing required progression niche
```

Prefer enriching existing items.

---

# 37. 142B Phase C — Basic Tier

Source examples:

- ragged coat,
- wool scarf,
- leather gloves.

Target purpose:

```text
early-game partial mitigation
```

not endgame protection.

---

# 38. 142B Phase D — Warm Tier

Examples:

- winter coat,
- fur-lined boots,
- thermal underwear.

Target:

```text
meaningful winter survival
```

with moderate acquisition cost.

---

# 39. 142B Phase E — Insulated Tier

Examples:

- cold-weather hazmat suit,
- heated vest,
- insulated gloves.

Require rarer materials/research/power if heated gear genuinely consumes power.

Do not grant “heated” bonus for free if battery/power system exists.

---

# 40. 142B Phase F — Arctic Tier

Examples:

- arctic survival suit,
- heated boots,
- thermal balaclava.

Endgame/rare.

Still capped below complete immunity.

---

# 41. 142B Phase G — Radiation/Cold Hybrid

Source proposes:

```text
+warmth +rad protection
```

Use item’s existing rad-protection property.

Balance weight/condition/scarcity.

---

# 42. 142B Phase H — Layering Rules

Map to actual equipment slots.

Source’s “maximum 4 layers” should become:

```text
maximum compatible thermal stack
```

rather than literally allowing 4 arbitrary items in one slot.

---

# 43. 142B Phase I — Layer Compatibility

Examples:

```text
inner + mid + outer + accessories
```

Reject:

- two outer suits in one slot,
- incompatible full-body suit + coat if equipment model disallows it.

---

# 44. 142B Phase J — Diminishing Returns

To prevent additive overflow:

```text
effective insulation = capped / diminishing formula
```

Source example totaling +107 but capping at tier 4 is a useful warning.

Do not sum raw bonuses indefinitely.

---

# 45. 142B Phase K — Cold Resistance Cap

Set a hard cap below 1.0.

Suggested product target:

```text
~0.70 maximum routine decay reduction
```

before other context.

Exact cap from balance tests.

---

# 46. 142B Phase L — Condition Degradation

Clothing degradation occurs through `EquipmentConditionSystem`.

Potential causes:

- daily wear,
- expedition wear,
- severe weather,
- combat damage if equipment system supports it.

No warmth-system durability tick.

---

# 47. 142B Phase M — Repair

Repair uses existing repair/workshop/recipe path.

Source examples:

- sewing kit,
- scrap.

Validate actual materials.

---

# 48. 142B Phase N — Upgrade Path

If item upgrades exist:

```text
basic → insulated variant
```

through research/workshop.

Do not create a separate clothing upgrade subsystem unless necessary.

---

# 49. 142B Phase O — Wetness from Rain/Snow

Source baseline:

```text
~50% warmth effectiveness loss when soaked
```

Treat as data/config.

Different materials may vary.

---

# 50. 142B Phase P — Sweat Wetness

Heavy work + excess insulation can wet inner layers.

Keep bounded and understandable.

---

# 51. 142B Phase Q — Drying

Near heat:

```text
drying rate increases
```

Avoid fully drying all clothing instantly.

---

# 52. 142B Phase R — Hypothermia Risk

Do not create a new hypothermia health system if one already exists or another plan owns it.

Minimum contract:

```text
critical warmth
+ severe cold/wetness
→ canonical cold-injury/health warning
```

---

# 53. 142B Phase S — Frozen Fingers

Glove absence under severe cold may apply dexterity/work penalty through work modifier stack.

No item-ID switch.
Use:

```text
hand protection property
```

---

# 54. 142B Phase T — Overheated Event

Excess layers in warm/high-work condition can produce:

- fatigue,
- wetness.

Do not overuse modal events.
Prefer status/briefing if recurrent.

---

# 55. 142B Phase U — “Soaked to the Bone”

State-driven warning when:

```text
high wetness + severe cold
```

This is a warning/event projection, not a separate mechanic.

---

# 56. 142B Phase V — Crafting Progression

Potential sources:

```text
rags
leather
animal hides
synthetics
advanced components
```

Use actual item IDs and recipes.

---

# 57. 142B Phase W — Trapping Integration

Animal hides become clothing material only if WildlifeTrappingSystem produces canonical hide items.

No direct trap→clothing bypass.

---

# 58. 142B Phase X — Research Integration

Advanced insulation unlocks via canonical ResearchSystem.

No hidden clothing-only tech tree.

---

# 59. 142B Phase Y — Trade Integration

Warm clothing is normal trade inventory.

Faction standing may affect availability through existing economy/faction rules.

---

# 60. 142B Phase Z — Expedition Scavenging

Cold-weather gear may appear in expedition loot tables.

Use data-authoring.

---

# 61. 142B Phase AA — Cold-Zone Expedition Readiness

Expedition preview can show:

```text
insulation band
cold resistance
wetness vulnerability
gear condition
```

Use same `WarmthProtectionSnapshot`.

---

# 62. 142B Phase AB — Cold-Zone Requirements

Do not hard block by arbitrary item ID.

Requirement may be:

```text
minimum thermal readiness band
```

Player may attempt anyway if expedition design permits, with warning/risk.

---

# 63. 142B Phase AC — Combat/Mobility Tradeoff

Heavy cold gear may impose movement/encumbrance penalty only through canonical equipment/combat modifier channels.

Do not implement duplicate mobility math.

---

# 64. 142B Phase AD — Work Modifier

Specialized gloves/clothes may help work if item properties justify it.

Use general tag/property-based modifiers.

---

# 65. 142B Phase AE — Clothing Panel / Survivor Panel

Prefer integrating into survivor/equipment panel rather than inventing a redundant standalone panel unless UI architecture needs one.

Show:

- equipped layers,
- effective warmth,
- cold resistance,
- wetness,
- condition,
- overheat risk.

---

# 66. 142B Phase AF — Tooltips

Each clothing item shows:

```text
warmth
cold resistance
slot/layer
condition
wet penalty
special requirements
```

Values come from same data authority.

---

# 67. 142B Phase AG — Tutorial

First significant cold exposure with insufficient clothing may trigger guidance.

Tutorial explains:

```text
environment cold
+ shelter heat
+ clothing
+ wetness
+ condition
```

Do not dump all mechanics before they matter.

---

# 68. 142B Phase AH — Clothing Journal

Source asks for automatic log of acquisition/upgrades.

Avoid logging every ordinary clothing pickup if journal noise is high.

Log only:

- first high-tier gear,
- major upgrade,
- winter-prep milestone,
- unique/heirloom clothing.

---

# 69. 142B Phase AI — Quest Hooks

Source concepts:

```text
The Tailor
Winter Preparation
The Arctic Expedition
Clothing Drive
```

Use canonical quest runtime.

---

# 70. 142B Phase AJ — 20-Profile Coverage Matrix

Generate:

| Item | Tier | Slot | Base warmth | Cold resist | Wet penalty | Condition scaling | Acquisition |
|---|---|---|---:|---:|---:|---|---|

---

# 71. 142B Phase AK — Content Utilization

100-day cold-weather run.

Report:

```text
profiles loaded
profiles equipped
profiles crafted
profiles traded
profiles scavenged
profiles never used
```

---

# 72. 142B Definition of Done

- [ ] existing-clothing audit,
- [ ] 20 profiles total,
- [ ] balanced tier spread,
- [ ] layering/slot compatibility,
- [ ] diminishing returns/cap,
- [ ] condition degradation through existing system,
- [ ] repair path,
- [ ] wetness/drying,
- [ ] overheat/sweat pressure,
- [ ] crafting,
- [ ] trapping material path,
- [ ] research,
- [ ] trade,
- [ ] expedition readiness,
- [ ] mobility/work tradeoffs,
- [ ] UI/tooltips,
- [ ] tutorial,
- [ ] quest hooks,
- [ ] utilization report.

---

# 73. Workstream 142C — Integration / Consequences / Validation

## Goal

Prove clothing warmth modifies the real warmth loop, remains deterministic and save-safe, and creates meaningful cold-survival progression without trivializing winter.

---

# 74. 142C Phase A — NeedsSystem Integration

`NeedsSystem.ApplyWarmth()` must consume the canonical clothing projection.

Test matrix:

```text
same environment
different clothing
→ different warmth decay
```

---

# 75. 142C Phase B — No-Clothing Baseline

Source requirement:

```text
no clothing → full decay
```

Confirm this remains true absent shelter heat/other modifiers.

---

# 76. 142C Phase C — Arctic-Gear Ceiling

Source target:

```text
~70% reduction
```

Assert cap.

Even max gear under extreme cold should not create impossible immunity if the environment is sufficiently severe.

---

# 77. 142C Phase D — Shelter + Clothing Combination

Test:

```text
no clothing + no heat
clothing + no heat
no clothing + heat
clothing + heat
```

Ensure formula is monotonic and bounded.

---

# 78. 142C Phase E — Condition Integration

Same outfit at:

```text
100%
50%
near broken
```

produces lower protection according to configured curve.

---

# 79. 142C Phase F — Wetness Integration

Same outfit:

```text
dry
damp
soaked
```

produces lower protection.

---

# 80. 142C Phase G — Drying Integration

Move/keep survivor near heat.

Wetness decreases over time.

Save/load mid-drying preserves state.

---

# 81. 142C Phase H — Expedition Integration

Cold-zone readiness preview and runtime use identical query.

No preview/runtime drift.

---

# 82. 142C Phase I — Inventory Equip Integration

Equip/unequip:

```text
immediately changes warmth projection
```

No stale cached protection after gear swap.

---

# 83. 142C Phase J — Condition Change Integration

Damage/repair:

```text
immediately changes protection
```

No separate re-registration required.

---

# 84. 142C Phase K — Old Save Compatibility

Old save with equipped clothing but no wetness state:

```text
loads dry
```

Warmth protection derives correctly from existing equipment.

No migration needed for equipped-warmth cache.

---

# 85. 142C Phase L — Save Round Trip

Persist wetness if stateful.

Round trip:

```text
wet item
→ save
→ load
→ same wetness
→ same protection
```

---

# 86. 142C Phase M — Determinism

Same state:

```text
same protection snapshot
same warmth delta
```

across runs.

---

# 87. 142C Phase N — Layer Exploit Tests

Attempt:

- duplicate incompatible layers,
- multiple outer suits,
- equip/unequip spam,
- zero-durability gear,
- over-cap accessories.

Assert no uncapped stacking.

---

# 88. 142C Phase O — Permanent-Gear Exploit

Source says durability prevents permanence.

More precise rule:

```text
protection tracks condition
```

If repair economy makes maintenance possible, clothing can remain useful indefinitely at a resource cost.
Do not intentionally force eventual destruction if condition system supports repair.

---

# 89. 142C Phase P — Heated Gear Power/Battery Check

If any heated clothing is retained:

- require actual energy source,
- deplete through canonical battery/power item state.

Otherwise rename/reframe as insulated gear.

No free powered clothing.

---

# 90. 142C Phase Q — Weather Integration

Rain/snow/cold values come from Weather/WeatherCascade.

No clothing-owned weather table.

---

# 91. 142C Phase R — Nuclear Winter Scenario

Run representative nuclear-winter scenario with:

```text
no gear
basic
warm
insulated
arctic
```

Measure:

- warmth decay,
- survival time,
- fuel use,
- drying burden,
- repair burden.

---

# 92. 142C Phase S — Balance Goal

Cold protection should:

- reward preparation,
- reduce but not erase fuel need,
- make wetness dangerous,
- make condition maintenance relevant,
- preserve expedition tradeoffs.

---

# 93. 142C Phase T — Gear Progression Sweep

Compare acquisition timing.

Ensure high-tier clothing cannot be trivially acquired before cold pressure matters unless intentionally rewarded.

---

# 94. 142C Phase U — Weight/Encumbrance Check

If inventory/equipment encumbrance exists:

- high-tier gear contributes accurately.

Do not double-penalize with both weight and arbitrary mobility modifier unless intended.

---

# 95. 142C Phase V — Work/Combat Modifier Integrity

Any gloves/heavy clothing effects route through canonical modifier stack.

No direct special-case code in crafting/combat.

---

# 96. 142C Phase W — UI Runtime Parity Test

Displayed:

```text
warmth bonus
resistance
wetness effect
condition effect
```

must match exact runtime calculation.

Snapshot/tooltips validate.

---

# 97. 142C Phase X — Accessibility

UI:

- text labels,
- no color-only warmth tier,
- icons with labels,
- keyboard/controller support if applicable,
- localized.

---

# 98. 142C Phase Y — Semantic Events

Significant events:

```text
clothing_soaked
cold_protection_insufficient
arctic_gear_equipped
critical_warmth_warning
```

Do not spam per-tick events.

---

# 99. 142C Phase Z — `--clothing-warmth-selftest`

Required scenarios:

1. no clothing,
2. one basic item,
3. full compatible layering,
4. max cap,
5. damaged gear,
6. soaked gear,
7. drying,
8. heat source combination,
9. equip/unequip refresh,
10. old save,
11. save/load wetness,
12. cold expedition readiness.

---

# 100. 142C Phase AA — Catalog Integrity

Validate:

- all 20 item IDs exist,
- wearable slots valid,
- warmth values bounded,
- resistance bounded,
- wet penalties bounded,
- no duplicate profiles,
- no profile for non-wearable item unless explicitly justified.

---

# 101. 142C Phase AB — Deliberate Failure Proof

Break:

- missing item ID,
- invalid resistance >1,
- incompatible layer,
- runtime/UI parity.

Assert selftest/data gate fails.

---

# 102. 142C Phase AC — 100-Day Cold Soak

Run fixed weather/campaign seed.

Compare cohorts:

```text
unprotected
basic
warm
insulated
arctic
```

Record:

- average warmth,
- critical-warmth events,
- hypothermia/cold-health events if canonical,
- fuel consumption,
- repairs,
- clothing condition,
- wetness time.

---

# 103. 142C Phase AD — Extreme Wet-Cold Soak

Repeated precipitation + cold.

Assert:

- wetness matters,
- drying matters,
- arctic gear is not magical,
- no runaway permanent wetness if heat available.

---

# 104. 142C Phase AE — Same-State Replay

No RNG should be involved in the protection formula.

Assert exact equality.

---

# 105. 142C Phase AF — Player Test Scenarios

Manual/structured:

1. first cold exposure with poor gear,
2. outfit upgrade,
3. wet expedition return,
4. drying near heat,
5. arctic expedition.

Observe clarity and progression value.

---

# 106. 142C Phase AG — Documentation

Create:

```text
docs/systems/CLOTHING_WARMTH.md
```

Include:

- authority boundaries,
- formula,
- layer/slot rules,
- wetness,
- condition,
- expedition readiness,
- adding clothing profiles,
- save behavior.

---

# 107. 142C Definition of Done

- [ ] NeedsSystem integration,
- [ ] Inventory equip integration,
- [ ] EquipmentCondition integration,
- [ ] expedition readiness,
- [ ] trapping/research/crafting/trade integration,
- [ ] old-save support,
- [ ] wetness save/load,
- [ ] deterministic formula,
- [ ] no layer exploit,
- [ ] heated-gear power integrity,
- [ ] weather integration,
- [ ] nuclear-winter balance,
- [ ] progression sweep,
- [ ] encumbrance/modifier integrity,
- [ ] UI/runtime parity,
- [ ] accessibility,
- [ ] semantic events,
- [ ] selftest,
- [ ] deliberate failure proof,
- [ ] 100-day cold soak,
- [ ] extreme wet-cold soak,
- [ ] playtests,
- [ ] docs.

---

# 108. Integrated Clothing-Warmth Pipeline

```text
items.json / warmth profile
      │
      ├─ base warmth
      ├─ cold resistance
      ├─ layer/slot
      └─ wet sensitivity
      │
      ▼
equipped items
      │
      ├────────────► condition authority
      └────────────► wetness state
                         │
                         ▼
                ClothingWarmthSystem
                         │
                         ▼
              WarmthProtectionSnapshot
                         │
              ┌──────────┴──────────┐
              ▼                     ▼
        NeedsSystem              Expedition
              │                     │
              └──────────┬──────────┘
                         ▼
                    player-facing UI
```

---

# 109. Static Item Data Contract

Warmth profile fields are static content.

They should not be duplicated in save state.

---

# 110. Equipment Ownership Contract

Inventory/equipment tells the warmth system what is worn.

Warmth system never equips/unequips.

---

# 111. Condition Ownership Contract

`EquipmentConditionSystem` remains the only runtime condition authority.

Warmth reads condition.

---

# 112. Wetness Ownership Contract

If introduced here, wetness is the only new persistent per-item/per-slot state.

Future generalized equipment environment-state plans may absorb it.

---

# 113. Warmth Need Contract

`NeedsSystem` remains owner of current Warmth need.

Clothing only supplies mitigation terms.

---

# 114. Formula Contract

All terms are explicit and independently attributable.

Example explanation:

```text
Environmental cold: -2.0/day
Shelter heat: ×0.75
Clothing insulation: ×0.55
Wetness: ×1.35
Final: -1.11/day
```

Numbers illustrative only.

---

# 115. Tier Contract

Tier labels are derived from effective protection.

No item stores “Tier 4” as behavior unless authored for UI only.

---

# 116. Layer Contract

Layering uses actual slots/compatibility.

No arbitrary four-item stacking.

---

# 117. Cap Contract

Protection remains bounded.

High-tier gear improves survival but cannot reduce all cold effects to zero.

---

# 118. Wetness Contract

Wetness reduces effective insulation, not item durability directly unless separate wear rule applies.

---

# 119. Drying Contract

Drying is time/environment dependent.

No UI-triggered instant state reset.

---

# 120. Heated Gear Contract

“Heated” means an actual energy consumer if energy mechanics exist.

Otherwise do not use powered semantics.

---

# 121. Expedition Contract

Expedition preview/runtime uses the exact same protection query.

---

# 122. Crafting Contract

Crafting outputs normal items with warmth metadata.

No clothing-specific hidden inventory.

---

# 123. Trade Contract

Trade uses normal item economy.

No special warmth-shop subsystem.

---

# 124. Research Contract

Research unlocks recipes/items, not a parallel warmth progression state.

---

# 125. Save Contract

Persist only non-derivable wetness state/cooldowns.

Do not persist:

```text
total warmth bonus
effective cold resistance
equipped warmth map
durability copy
```

---

# 126. Old Save Contract

Old saves automatically gain clothing warmth based on existing equipped items.

This is a feature addition, not a save incompatibility.

---

# 127. UI Contract

Displayed warmth protection is a projection from `WarmthProtectionSnapshot`.

No separate tooltip formula.

---

# 128. Content Acceptance Contract

20 profiles progress through:

```text
AUTHORED
→ LOADS
→ EQUIPPABLE
→ PROTECTION_PRODUCED
→ PLAYER_VISIBLE
```

---

# 129. Balance Contract

Cold gear should create a real choice between:

- fuel/heat investment,
- clothing acquisition,
- repair,
- expedition timing,
- drying,
- encumbrance.

No single axis should erase all others.

---

# 130. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| warmth state duplicates durability/equipment | Medium | High | projection-only design |
| additive layering exceeds 100% | High | High | diminishing returns + cap |
| arctic gear trivializes winter | Medium | High | 70%-ish cap + extreme-cold soak |
| wetness becomes permanent nuisance | Medium | Medium | drying/recovery tests |
| heated gear has free power | Medium | Medium | battery/power contract |
| old saves unexpectedly become overpowered | Medium | Medium | balance legacy fixture |
| UI value differs from runtime | Medium | High | shared snapshot/query |
| clothing content duplicates existing items | Medium | Medium | item audit first |
| condition scaling double-applied | Medium | High | one condition term |
| expedition uses separate gear requirement math | Medium | High | same protection query |
| glove/work special cases become ID hacks | Medium | Medium | tags/properties |
| journal/tutorial spam | Medium | Low | significance gating |

---

# 131. Commit Strategy

## 142A — Foundation

### C2[29].1 — baseline + clothing-warmth ADR

### C2[29].2 — profile schema + data-authority decision

### C2[29].3 — WarmthProtectionSnapshot + ClothingWarmthSystem

### C2[29].4 — condition integration

### C2[29].5 — wetness/drying state

### C2[29].6 — NeedsSystem integration

### C2[29].7 — catalog integrity/diagnostics

### Gate: 142A complete

---

## 142B — Content / Progression

### C2[29].8 — existing-clothing audit

### C2[29].9 — basic/warm profiles

### C2[29].10 — insulated/arctic/hybrid profiles

### C2[29].11 — layering/caps/overheat

### C2[29].12 — repair/crafting/research

### C2[29].13 — trapping/trade/scavenge

### C2[29].14 — expedition/combat/work modifiers

### C2[29].15 — UI/tutorial/tooltips/quests

### C2[29].16 — content-utilization report

### Gate: 142B complete

---

## 142C — Closure

### C2[29].17 — save/old-save matrix

### C2[29].18 — equip/condition/wetness runtime refresh

### C2[29].19 — exploit/cap/heated-gear tests

### C2[29].20 — weather/nuclear-winter integration

### C2[29].21 — selftest + deliberate failure proof

### C2[29].22 — 100-day cohort cold soak

### C2[29].23 — extreme wet-cold soak

### C2[29].24 — UI/accessibility parity

### C2[29].25 — docs/playtest/release closure

### Gate: 142C complete

---

# 132. Verification Checklist

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --clothing-warmth-selftest
bash scripts/ci/verify-fast.sh
```

Also run repository-canonical equivalents of:

```text
item/profile integrity check
old-save fixture load
same-state warmth replay
100-day nuclear-winter clothing cohort
wet-cold stress soak
survivor/equipment panel snapshot-accessibility check
```

---

# 133. Flagship Definition of Done

## 142A — Foundation

- [ ] ClothingWarmthSystem,
- [ ] single static data authority,
- [ ] layer/slot vocabulary,
- [ ] derived protection snapshot,
- [ ] no duplicate durability state,
- [ ] minimal wetness state,
- [ ] NeedsSystem integration,
- [ ] shelter/environment combination,
- [ ] deterministic formula,
- [ ] old-save dry default,
- [ ] integrity tests,
- [ ] diagnostics.

## 142B — Content / Progression

- [ ] 20 valid profiles,
- [ ] existing items reused where appropriate,
- [ ] basic/warm/insulated/arctic progression,
- [ ] radiation/cold hybrid where appropriate,
- [ ] layering compatibility,
- [ ] diminishing returns/cap,
- [ ] durability degradation via canonical system,
- [ ] repair,
- [ ] wetness/drying,
- [ ] overheat/sweat pressure,
- [ ] crafting,
- [ ] trapping materials,
- [ ] research,
- [ ] trade/scavenging,
- [ ] expedition readiness,
- [ ] combat/work tradeoffs,
- [ ] UI/tooltips,
- [ ] tutorial/quests,
- [ ] utilization.

## 142C — Integration

- [ ] no-clothing baseline,
- [ ] arctic cap,
- [ ] shelter + clothing matrix,
- [ ] condition scaling,
- [ ] wetness scaling,
- [ ] drying,
- [ ] equip refresh,
- [ ] old-save compatibility,
- [ ] save/load wetness,
- [ ] deterministic equality,
- [ ] layer exploit blocked,
- [ ] heated gear powered or renamed,
- [ ] weather integration,
- [ ] nuclear-winter balance,
- [ ] progression timing,
- [ ] UI/runtime parity,
- [ ] accessibility,
- [ ] selftest,
- [ ] failure proof,
- [ ] 100-day soak,
- [ ] wet-cold soak,
- [ ] playtest,
- [ ] docs.

## Global

- [ ] no duplicate equipment authority,
- [ ] no duplicate condition authority,
- [ ] no hardcoded clothing-ID behavior lists,
- [ ] no uncapped insulation stacking,
- [ ] no permanent 100% cold immunity,
- [ ] no UI/runtime formula drift,
- [ ] full verification green.

---

# 134. Closure Report Template

```markdown
## C2[29] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- Clothing items:
- Wearable items:
- Warmth-aware items before:
- NeedsSystem clothing references before:
- Warmth decay baseline:
- Condition authority:
- Equipment slots:

### 142A — Foundation
- Profile authority:
- Profiles:
- Layer mapping:
- ClothingWarmthSystem:
- Wetness state:
- Needs integration:
- Old save:
- Integrity failures:
- Result:

### 142B — Progression
- Basic:
- Warm:
- Insulated:
- Arctic:
- Hybrid:
- Layer cap:
- Wetness:
- Repair:
- Crafting:
- Trapping:
- Research:
- Trade:
- Expedition:
- UI:
- Tutorial:
- Unused profiles:
- Result:

### 142C — Integration
- No-clothing decay:
- Basic decay:
- Warm decay:
- Insulated decay:
- Arctic decay:
- Max reduction:
- Wet penalty:
- Damaged penalty:
- Drying:
- Old-save load:
- Save/load wetness:
- Layer exploit failures:
- Heated gear power:
- 100-day soak:
- Wet-cold soak:
- UI parity:
- Result:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Clothing warmth selftest:
- Old-save fixtures:
- Same-state replay:
- Cold cohort soak:
- Wet-cold soak:
- Verify fast:

### Final Metrics
- CLOTHING_WARMTH_PROFILES:
- PROFILES_EQUIPPED_IN_SOAK:
- UNKNOWN_PROFILE_ITEMS:
- MAX_WARMTH_DECAY_REDUCTION:
- WETNESS_STATE_COUNT:
- UI_RUNTIME_MISMATCHES:
- OLD_SAVE_FAILURES:
- LAYER_CAP_VIOLATIONS:
- COLD_CRITICAL_EVENTS_NO_GEAR:
- COLD_CRITICAL_EVENTS_ARCTIC:

### Remaining Debt
- Clothing content:
- Wetness:
- Overheating:
- Expedition:
- Crafting:
- UI:
```

---

# 135. Final Execution Directive

Execute Plan 142 as a **gear-derived cold-mitigation layer over the existing equipment, condition, needs, weather, expedition, and crafting authorities**.

The critical sequence is:

```text
author warmth properties on wearable items
→ read the actual equipped set
→ read real condition
→ track only genuinely new wetness state
→ compute one bounded protection snapshot
→ feed that snapshot into NeedsSystem
→ reuse the same snapshot for expedition readiness and UI
→ author a 20-item progression
→ prove cold remains dangerous but manageable in long winter soaks
```

Do not save a duplicate equipped-warmth map.

Do not copy durability into the warmth system.

Do not hardcode item IDs for generic protection behavior.

Do not let additive layering exceed the cap.

Do not label gear “heated” unless it consumes actual energy.

The strongest authority rule is:

> **Warmth protection is derived from the clothing the survivor is actually wearing, the condition that equipment actually has, and the wetness/environment the world actually provides.**

The strongest balance rule is:

> **Clothing must make winter survivable without making shelter heat, fuel, drying, repair, or expedition timing irrelevant.**

The strongest UI rule is:

> **The number shown to the player must be the exact protection value used by `NeedsSystem` and expedition readiness—not a parallel estimate.**

The flagship acceptance scenario is:

> **Place the same survivor outside in a fixed nuclear-winter environment with five configurations: no clothing, basic clothing, warm clothing, damaged insulated clothing, and dry full arctic gear. Record the exact warmth-loss curve, then soak the arctic set, save/load, wet it in precipitation, return to a heated shelter, dry it, repair one damaged piece, and send the survivor back outside. Every gear swap, condition change, wetness change, and heat-source change must update the same protection query immediately, the old save must load without duplicate clothing state, and the maximum configuration must improve survival substantially without eliminating cold risk.**
