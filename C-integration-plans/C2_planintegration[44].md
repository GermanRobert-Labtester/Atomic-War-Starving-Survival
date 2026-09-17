# C2 — Flagship Integration Plan [44]: Shelter Resource Rationing, Priority Allocation, Scarcity Crisis Management, and Fairness-Aware Consumption Control

> **Deliverable:** `C2_planintegration[44].md`
> **Source scope:** Plan 215 — *Shelter Resource Rationing & Crisis Management*
> **Primary objective:** create a deterministic shelter rationing and scarcity-management layer that lets the player define consumption policy under shortage, allocate scarce resources by explicit priority rules, detect and manage resource crises, authorize emergency protocols, surface unequal consequences, and coordinate food/water/medical/fuel allocation across canonical resource-consumption systems without creating parallel inventory balances, duplicate need calculations, duplicate kitchen/water/power/medical pipelines, or a second leadership/governance authority.
> **Required execution order:** **215A Foundation / Rationing Policy Contract → 215B Protocols, Priority Allocation, Crisis Lifecycle, Enforcement & UI → 215C Cross-System Integration, Save/CI, Exploit Control, Long-Run Scarcity Balance, and Closure**
> **Hard dependencies:** Plan 22 One Food Authority; `NeedsSystem`; `Inventory`; `KitchenNutritionSystem`; `WaterTreatmentSystem`; `PowerGridSystem`; `MedicalPipelineCoordinator`; `DutyRosterSystem`; `LeadershipSystem`; Plan 211 `InternalCommunicationSystem`; canonical survivor roster/age/role/health/work-state providers; morale/social/health consequence sinks; Plan 31 semantic events; Plan 36 port contracts; Plan 39 save durability; Plan 55 retention; Plan 184 accessibility.
> **Critical ownership correction:** `ResourceRationingSystem` owns **policy and allocation constraints**, not resource truth. Inventory/resource producers remain authoritative for quantities; Needs/Kitchen/Water/Power/Medical systems remain authoritative for actual consumption and physiological/operational consequences.
> **Critical membership correction:** priority groups are primarily **rules/projections**, not manually persisted survivor lists. “Children,” “medics,” “workers,” “leadership,” etc. must be recomputed from canonical survivor facts at allocation time, with explicit override support only where design requires it.
> **Critical crisis correction:** crisis declaration/resolution must use **threshold hysteresis and minimum-duration/cooldown rules** so a resource oscillating around one threshold does not repeatedly declare and resolve a crisis.
> **Scope discipline:** no second resource ledger, no duplicate food/water consumption loop, no local survivor health simulation, no leadership privilege baked into code, no percentage-share model that can allocate more than available stock, no persistent priority membership that becomes stale when roles/age/health change, no random ration violations without a real access/behavior trigger, no repeated morale penalty every tick, no automatic starvation deaths calculated locally, and no UI action that changes resource state outside canonical transaction APIs.

---

# 0. Executive Intent

ASHFALL already has systems that answer:

```text
How much food exists?
How much clean water exists?
Who is hungry/thirsty?
How much nutrition is served?
How much power/fuel exists?
How many medical supplies remain?
Who is working?
Who is leading?
```

What is missing is a coherent answer to:

```text
What should happen when there is not enough for everyone?
```

Without rationing, scarcity collapses into:

```text
resource exists
→ consume normally
→ resource reaches zero
→ failure
```

The target is:

```text
resource pressure
      │
      ▼
ResourceRationingSystem
      │
      ├─ protocol selection
      ├─ per-resource ration level
      ├─ eligibility / exemptions
      ├─ priority allocation policy
      ├─ crisis declaration
      ├─ emergency responses
      └─ enforcement policy
      │
      ▼
canonical allocation requests
      │
 ┌────┼─────────┬──────────┬───────────┐
 ▼    ▼         ▼          ▼           ▼
Food Water   Medical     Fuel/Power   Other
 │    │         │          │
 ▼    ▼         ▼          ▼
canonical consumption / production systems
      │
      ▼
actual delivered amount
      │
      ├─ Needs/health consequences
      ├─ morale/social reaction
      ├─ violation/dispute events
      └─ crisis state changes
```

The strongest product outcome is:

> **When the shelter enters scarcity, the player can explicitly decide what to cut, what to preserve, and who receives priority. The game then applies those decisions through the real consumption systems, shows exactly who received less and why, surfaces health/morale/social consequences, and lets the player escalate or relax policy as reserves change—without ever inventing a second set of resource numbers.**

---

# 1. Source Diagnosis

The source establishes:

- `DutyRosterSystem.cs` has a single `mutationRationProtocol` boolean,
- resources are consumed from shared pools without comprehensive prioritization,
- no comprehensive rationing system exists,
- seven ration levels are required:
  - Full,
  - Three-quarter,
  - Half,
  - Third,
  - Quarter,
  - Minimal,
  - None,
- seven named priority groups are required:
  - Essential Personnel,
  - Children,
  - Elderly,
  - Medics,
  - Leadership,
  - Workers,
  - General,
- six crisis types are required:
  - Food Shortage,
  - Water Shortage,
  - Medical Shortage,
  - Fuel Shortage,
  - General Scarcity,
  - Multiple Shortage,
- crisis response must include:
  - declaration,
  - rationing activation,
  - priority allocation,
  - emergency measures,
  - resolution,
- rationing must affect consumption,
- violations must be detectable and penalized,
- morale/health consequences must exist,
- 10+ protocol templates are required,
- UI must include:
  - rationing panel,
  - protocol detail,
  - priority panel,
  - crisis panel,
  - resource status,
  - violation log,
- integrations are required with:
  - Needs,
  - Inventory,
  - Kitchen/Nutrition,
  - Water Treatment,
  - Power Grid,
  - Medical Pipeline,
  - Leadership,
  - Internal Communication,
- old saves get no active rationing,
- deterministic behavior and `--resource-rationing-selftest` are required.

The source also contains architectural ambiguities that must be normalized.

### 1.1 `dailyAllocation` cannot be universal

Food, water, medicine, and fuel have different units and consumers.

Therefore:

```text
RationLevel
```

should encode:

```text
fraction of baseline demand
minimum reserve / exception policy
```

and leave domain-specific units to each consumer.

### 1.2 Priority percentages are not the same as individual allocations

A group share such as:

```text
Children 25%
```

is ambiguous if group size changes.

Prefer:

```text
priority weight / minimum guarantee / tier
```

and let the allocation solver distribute constrained stock.

### 1.3 Groups overlap

A survivor can be:

```text
child
+ patient
+ worker?
```

or:

```text
medic
+ leadership
+ essential personnel
```

Therefore priority must resolve through a deterministic overlap policy rather than summing all group percentages blindly.

### 1.4 `General Scarcity` and `Multiple Shortage` overlap

Define:

```text
GeneralScarcity
= broad low-reserve state across multiple resources

MultipleShortage
= severe/critical simultaneous shortage of >= configured N resources
```

No duplicate crisis for the same condition unless intentionally layered.

### 1.5 Ration violation requires behavior/access

The system must not randomly declare:

```text
survivor stole extra food
```

just because rationing is active.

Violation must come from:

- explicit theft/access event,
- unauthorized consumption,
- black-market/internal conflict action,
- scripted/autonomy behavior.

---

# 2. Program-Level Success Criteria

C2[44] closes only when all of the following are true.

1. `ResourceRationingSystem.cs` exists.
2. It has versioned `CaptureState/RestoreState`.
3. Existing resource quantities remain owned by Inventory/domain systems.
4. Existing physiological need state remains owned by Needs/health systems.
5. Existing `mutationRationProtocol` is migrated, deprecated, or turned into a compatibility projection.
6. Seven source ration levels exist.
7. Ration-level fractions are data-driven and validated.
8. “Minimal” has an explicit configured range/value per resource domain rather than an ambiguous 10–15% runtime random choice.
9. Seven source priority-group concepts exist.
10. Group membership derives from canonical survivor facts.
11. Overlapping groups resolve deterministically.
12. Player-authored/manual exemptions are explicit.
13. Priority configuration cannot allocate more than available stock.
14. Per-resource allocation is computed from demand and stock.
15. No consumer receives a negative allocation.
16. Full rationing reproduces baseline behavior.
17. None rationing means no ordinary allocation unless an explicit emergency exception applies.
18. Food rationing integrates with Plan 22/KitchenNutrition rather than multiplying hunger directly.
19. Water rationing integrates with WaterTreatment/Needs through canonical delivery.
20. Medical rationing prioritizes treatment demand rather than “daily medicine per survivor.”
21. Fuel/power rationing integrates with PowerGrid load shedding/reserve policy.
22. Six source crisis types are supported.
23. Crisis declaration uses stock/demand horizons or thresholds from canonical resource forecasts.
24. Crisis resolution uses hysteresis.
25. Crisis severity is derived from objective resource pressure.
26. Auto-ration mode can propose/activate protocols according to player setting.
27. Manual mode never silently overrides player policy except a clearly defined hard safety rule.
28. Leadership authorization is required where configured.
29. A dead/invalid authorizer does not leave policy locked forever.
30. Crisis protocol changes are idempotent.
31. Ration violations require an actual unauthorized-consumption event.
32. Violation consequences route through canonical systems.
33. Rationing consequences are bounded/event-based, not re-applied every tick.
34. Health effects derive from actual unmet needs/treatment denial.
35. Morale/social effects distinguish fair, unfair, severe, and discriminatory allocation contexts.
36. Old saves load with no active rationing.
37. Old `mutationRationProtocol` state is migrated deterministically.
38. No-rationing play remains valid.
39. Full multi-resource crisis remains valid.
40. 10+ protocol templates load and validate.
41. UI shows requested vs allocated vs delivered amounts.
42. UI shows priority reasons and overlap resolution.
43. UI shows crisis trigger/resolution reason.
44. Headless behavior matches UI behavior.
45. Same seed + same resource state + same policy produces the same allocation/crisis history.
46. `--resource-rationing-selftest` passes.

---

# 3. Architectural Invariants

## 3.1 Resource truth remains external

`ResourceRationingSystem` never stores authoritative:

```text
food stock
water stock
medicine stock
fuel stock
```

It reads snapshots.

## 3.2 Rationing is policy

It answers:

```text
How much of normal demand is authorized?
Who should receive scarce stock first?
What reserve should be protected?
```

## 3.3 Consumption systems execute

Food, water, medical, power systems execute actual resource transactions.

## 3.4 Need consequences come from delivered amount

Do not directly set:

```text
hunger += rationPenalty
```

if Kitchen/Needs already computes intake deficiency.

## 3.5 Priority groups are dynamic projections

A medic ceases to be “active medic” if:

- duty changes,
- survivor dies,
- incapacity prevents work,

unless policy intentionally still prioritizes them.

## 3.6 Overlap is explicit

Each survivor receives one effective priority result per resource allocation event.

## 3.7 Crisis state uses hysteresis

Example:

```text
declare below 3 days supply
resolve above 5 days supply
```

not one identical threshold.

## 3.8 Scarcity is demand-relative

“10 food units” can be abundant for 2 survivors and disastrous for 20.

Prefer:

```text
days_of_supply
```

or domain-specific coverage metrics.

## 3.9 No invisible discrimination

If leadership receives more than general population:

- UI must show that policy,
- consequences may react.

## 3.10 Enforcement does not invent crimes

Violation detection consumes real unauthorized-consumption events.

---

# 4. Ownership Matrix

| Concern | Authority |
|---|---|
| Inventory stock | `Inventory` / canonical resource owner |
| Food consumption | Plan 22 + KitchenNutrition |
| Hunger/needs | `NeedsSystem` |
| Water production/availability | `WaterTreatmentSystem` |
| Power/fuel | `PowerGridSystem` |
| Medical supplies/treatment | `MedicalPipelineCoordinator` |
| Duties/roles | `DutyRosterSystem` |
| Current leader | `LeadershipSystem` |
| Internal announcement | Plan 211 |
| Rationing policy | `ResourceRationingSystem` |
| Crisis state | `ResourceRationingSystem` |
| Allocation priority policy | `ResourceRationingSystem` |
| Health consequence | health/needs owner |
| Morale/social consequence | morale/relations owner |
| Unauthorized consumption fact | owning behavior/inventory/event system |

---

# 5. Workstream 215A — Foundation / Rationing Policy Contract

## Goal

Create a normalized, deterministic policy model for ration levels, priorities, crisis conditions, authorization, and allocation requests while preserving all existing resource/need authorities.

---

# 6. 215A Phase A — Audit Existing Rationing Touchpoints

Search:

```text
mutationRationProtocol
ration
food allocation
water allocation
medical reserve
fuel reserve
priority
consume
TryConsume
daily ration
```

Classify:

```text
LIVE
PARTIAL
LEGACY
UI_ONLY
DEAD
PLAN_ONLY
```

---

# 7. 215A Phase B — Audit `DutyRosterSystem`

Inspect exact semantics of:

```text
mutationRationProtocol
```

Determine whether it currently:

- changes work assignments,
- toggles mutation-specific rationing,
- affects food consumption,
- is unused.

Do not delete until compatibility path is proven.

---

# 8. 215A Phase C — Compatibility Strategy

Possible outcomes:

### If unused

Deprecate and remove after migration/test.

### If used as generic rationing toggle

Map:

```text
true
→ one compatibility rationing policy/template
```

then retire old bool.

### If mutation-specific

Keep as separate domain rule and do not conflate with Plan 215.

---

# 9. 215A Phase D — Create `ResourceRationingSystem`

Path:

```text
Assets/Ashfall.Core/Economy/ResourceRationingSystem.cs
```

Responsibilities:

- own active rationing protocols,
- own policy priority rules,
- evaluate scarcity/crisis triggers,
- issue allocation decisions/constraints,
- track protocol lifecycle,
- track crisis lifecycle,
- consume violation facts,
- emit consequence intents,
- expose read models,
- capture/restore owned state.

---

# 10. 215A Phase E — Static vs Runtime Split

Static:

```text
RationingProtocolDefinition
RationLevelDefinition
PriorityRuleDefinition
CrisisDefinition
CrisisResponseDefinition
RationingConsequenceProfile
```

Runtime:

```text
ActiveRationingProtocol
ResourceRationPolicy
CrisisRecord
RationingPolicyChange
RationViolationRecord
ResourceRationingState
```

---

# 11. 215A Phase F — Ration Level Vocabulary

Preserve exact source seven:

```text
Full
ThreeQuarter
Half
Third
Quarter
Minimal
None
```

---

# 12. 215A Phase G — Fraction Mapping

Default:

```text
Full = 1.00
ThreeQuarter = 0.75
Half = 0.50
Third = 0.333333...
Quarter = 0.25
Minimal = domain-defined minimum
None = 0.00
```

Do not randomly choose 10–15% for Minimal.

---

# 13. 215A Phase H — Minimal Ration

Data:

```text
resource_id / resource_domain
minimal_fraction
minimum_absolute_delivery optional
```

Examples:

- water survival minimum differs from calories,
- medicine may not support “minimal daily dose.”

---

# 14. 215A Phase I — Resource Domains

Create typed classification:

```text
Food
Water
Medical
Fuel
Power
OtherConsumable
```

Resource IDs map to domain.

---

# 15. 215A Phase J — Ration Policy Per Resource

Recommended:

```text
ResourceRationPolicy
{
    resource_id_or_domain
    ration_level
    baseline_fraction
    reserve_policy
    priority_scheme_id
    effective_day
}
```

---

# 16. 215A Phase K — Baseline Demand

Each domain exposes:

```text
IRationableDemandProvider
```

Returns canonical demand for allocation window.

Examples:

```text
Food
→ kcal/meal demand

Water
→ liters-equivalent demand

Medical
→ treatment request quantities

Fuel
→ generator/device demand
```

Rationing does not invent units.

---

# 17. 215A Phase L — Allocation Window

Default:

```text
daily
```

for survivor consumables.

Power/fuel may be:

```text
tick/hour/day
```

through domain adapter.

The policy engine should support domain-owned windows.

---

# 18. 215A Phase M — Priority Group Vocabulary

Preserve source seven:

```text
EssentialPersonnel
Children
Elderly
Medics
Leadership
Workers
General
```

---

# 19. 215A Phase N — Group Definitions

Static rules define membership queries.

Examples:

```text
Children
→ life stage == child

Elderly
→ canonical age/life-stage predicate

Medics
→ active medical duty / qualified medic

Leadership
→ current leader / delegated leader if policy includes

Workers
→ active duty

General
→ fallback
```

---

# 20. 215A Phase O — Essential Personnel

Do not hardcode profession list.

Use:

```text
critical-duty tag
```

from duty/role system.

Possible:

- medic,
- engineer,
- security,
- water operator,
- power operator.

---

# 21. 215A Phase P — Group Membership Projection

At allocation event:

```text
survivor state
+ duty state
+ life stage
+ health state
+ leadership state
→ matching groups
```

---

# 22. 215A Phase Q — Overlap Resolution

Choose one explicit policy.

Recommended:

```text
highest-priority matching group wins
```

with tie:

```text
most-specific rule
→ stable group ID
```

Alternative additive weights only if carefully bounded.

---

# 23. 215A Phase R — Why Highest Priority Wins

Prevents:

```text
leader + medic + worker + essential
→ 4x ration
```

---

# 24. 215A Phase S — Priority Levels

Source:

```text
1–5
1 = highest
```

Keep internal.

Allow multiple groups same priority.

---

# 25. 215A Phase T — Allocation Percentage Correction

The source proposes group `allocationPercentage`.

Use one of:

```text
minimum guarantee %
priority weight
maximum share %
```

but do not treat it as literal fixed fraction of total stock without population normalization.

Recommended baseline:

```text
priority_weight
minimum_fraction_of_baseline
```

---

# 26. 215A Phase U — Weighted Allocation Solver

For a resource:

1. compute each consumer baseline demand,
2. apply protocol ration fraction,
3. compute effective priority,
4. guarantee configured minimums,
5. allocate remaining stock in priority order/weighted shares,
6. cap at requested amount,
7. return shortages.

---

# 27. 215A Phase V — No Overallocation

Invariant:

```text
sum(allocations) <= available resource
```

within numeric tolerance.

---

# 28. 215A Phase W — No Negative Allocation

Invariant:

```text
allocation >= 0
```

---

# 29. 215A Phase X — Reserve Policy

Some resources may have protected reserve.

Example:

```text
medicine reserved for severe treatment
fuel reserved for life-support power
```

Rationing policy can declare reserve intent.

Canonical domain system enforces.

---

# 30. 215A Phase Y — Emergency Overrides

Examples:

```text
medical emergency override
dehydration emergency
critical heating emergency
```

These must be explicit domain rules.

Do not silently bypass player policy.

---

# 31. 215A Phase Z — `RationingProtocolDefinition`

Recommended:

```text
protocol_id
name_key
protocol_type
resource_policies
priority_scheme_id
activation_conditions optional
authorization_policy
duration_policy
communication_profile
consequence_profile
```

---

# 32. 215A Phase AA — Protocol Types

Preserve source:

```text
Standard
Tight
Emergency
Crisis
Triage
```

---

# 33. 215A Phase AB — Protocol Semantics

Suggested:

```text
Standard
→ mild conservation

Tight
→ substantial reduction

Emergency
→ survival-focused

Crisis
→ multi-resource emergency

Triage
→ medical/critical-service prioritization
```

Do not bind one exact ration fraction to entire protocol; per-resource policies differ.

---

# 34. 215A Phase AC — Protocol Lifecycle

Use:

```text
Draft
PendingAuthorization
Active
Suspended
Expired
Lifted
Superseded
```

Source only requires active/suspended/expired; richer lifecycle avoids ambiguity.

---

# 35. 215A Phase AD — Authorization

`authorizedBy` must be:

- valid survivor,
- current authorized leader/deputy/governance actor,
- alive/present as policy requires.

---

# 36. 215A Phase AE — Authorization Port

Use:

```text
IRationingAuthorityPolicy
```

Backed by Leadership/Plan 159 if needed.

---

# 37. 215A Phase AF — Emergency Auto-Activation

If:

```text
auto_ration_on_shortage = true
```

system may auto-activate a configured emergency template.

But:

- action is logged,
- UI announces,
- player can review,
- authority policy determines whether automatic emergency power is valid.

---

# 38. 215A Phase AG — Manual Mode

If auto-ration off:

- crisis can be declared,
- protocol remains proposal/pending player authorization.

Critical domain systems may still enforce unavoidable physical shortage because stock is actually absent.

---

# 39. 215A Phase AH — Crisis Types

Preserve six:

```text
FoodShortage
WaterShortage
MedicalShortage
FuelShortage
GeneralScarcity
MultipleShortage
```

---

# 40. 215A Phase AI — Crisis Severity

Preserve:

```text
Mild
Moderate
Severe
Critical
```

---

# 41. 215A Phase AJ — Scarcity Metric

Preferred:

```text
days_of_supply
```

when meaningful.

Formula:

```text
available usable stock / projected baseline demand per day
```

Domain can provide another metric if necessary.

---

# 42. 215A Phase AK — Crisis Thresholds

Example data:

```text
mild_enter
moderate_enter
severe_enter
critical_enter

resolve_threshold
```

Do not hardcode one global threshold.

---

# 43. 215A Phase AL — Hysteresis

Example:

```text
Food crisis enters < 5 days supply
resolves > 7 days supply
```

This prevents chatter.

---

# 44. 215A Phase AM — Minimum Active Duration

Optional:

```text
min_crisis_days
```

prevents declare/resolve same day unless resource emergency disappears completely through major resupply and policy allows immediate closure.

---

# 45. 215A Phase AN — Crisis Cooldown

After resolution:

- same crisis can reopen if threshold truly crosses again,
- optional cooldown only prevents notification spam, not state truth.

---

# 46. 215A Phase AO — General Scarcity

Define as:

```text
>= N resources below mild/moderate pressure threshold
```

but not necessarily critical.

---

# 47. 215A Phase AP — Multiple Shortage

Define as:

```text
>= 3 resources at severe/critical shortage
```

matching source intent.

---

# 48. 215A Phase AQ — Crisis Aggregation

A `MultipleShortage` crisis may coexist with individual crises if UI handles hierarchy.

Recommended:

- individual crises remain authoritative,
- multiple shortage is derived aggregate crisis.

Avoid double consequences.

---

# 49. 215A Phase AR — `CrisisRecord`

Fields:

```text
crisis_id
crisis_type
resource_refs
severity
triggered_day
trigger_reason
active_protocol_refs
response_action_refs
status
resolved_day optional
resolution_reason optional
```

---

# 50. 215A Phase AS — Crisis Response Actions

Typed:

```text
ActivateRationing
TightenRationing
ReserveCriticalStock
SeekTrade
LaunchExpedition
IncreaseProduction
ReduceNonEssentialLoad
SuspendRecipe/Process
RequestAid
CommunicationBriefing
```

Only actions with real consumers.

---

# 51. 215A Phase AT — No Fake Emergency Action

If no system can execute:

```text
RequestAid
```

do not ship it as clickable.

Can remain future data only if clearly disabled.

---

# 52. 215A Phase AU — Crisis Resolution

Requires:

- resource metric above exit threshold,
- unresolved hard deficits cleared,
- minimum-duration policy if configured.

---

# 53. 215A Phase AV — Crisis Reclassification

Severity can escalate/de-escalate without opening new crisis ID.

Emit:

```text
rationing_escalated
crisis_severity_changed
```

---

# 54. 215A Phase AW — Rationing Event Record

Source asks:

```text
RationingEvent
```

Prefer semantic event history with structured refs.

Runtime record only if needed for UI/history.

Fields:

```text
event_id
event_type
day
resource_refs
protocol_ref
crisis_ref
actor_ref
reason_code
severity
consequence_refs
```

---

# 55. 215A Phase AX — Event Types

Preserve:

```text
RationingStarted
RationingEscalated
RationingLifted
ResourceDepleted
PriorityDispute
CrisisDeclared
CrisisResolved
RationViolation
```

---

# 56. 215A Phase AY — Determinism

Allocation math should be deterministic without RNG.

RNG only for:

- survivor behavioral violation attempt if canonical autonomy emits uncertainty,
- narrative reaction selection,
- tie-breakers if unavoidable.

---

# 57. 215A Phase AZ — RNG Stream

Use:

```text
resource_rationing
```

Stable keys by:

```text
campaign seed
day
event type
survivor/resource ID
```

---

# 58. 215A Phase BA — State Persistence

Persist:

- active/suspended protocols,
- policy overrides,
- active crises,
- authorization state,
- violation records needed for unresolved consequences,
- idempotency keys,
- schema version.

Do not persist:

- duplicate stock,
- duplicate survivor need values,
- derived group membership,
- derived days-of-supply if recomputable.

---

# 59. 215A Phase BB — Old-Save Compatibility

Exact source baseline:

```text
no active rationing
```

Also:

- no active crisis records,
- no priority overrides.

---

# 60. 215A Phase BC — Legacy Bool Migration

If old `mutationRationProtocol` is live:

- preserve behavior through explicit migration mapping,
- document result.

No silent loss.

---

# 61. 215A Phase BD — Composition Root

Source asks:

```text
SetupResourceRationing
TickResourceRationing
SaveResourceRationing
```

Follow Plan 28 composition architecture.

Do not reintroduce manual setup drift.

---

# 62. 215A Phase BE — Tick Model

Prefer:

```text
day-start / meal / treatment / power-allocation events
```

not one giant generic tick.

---

# 63. 215A Phase BF — Semantic Events

Candidate kinds:

```text
rationing_protocol_proposed
rationing_protocol_activated
rationing_protocol_changed
rationing_protocol_lifted
resource_crisis_declared
resource_crisis_severity_changed
resource_crisis_resolved
ration_allocation_shortfall
ration_violation_detected
ration_priority_dispute
```

---

# 64. 215A Phase BG — Port Contract

Mandatory:

- resource stock snapshots,
- demand providers,
- survivor roster,
- consequence sinks,
- save.

Conditional:

- leadership authorization,
- communication,
- expedition/trade response actions,
- autonomy/violation producer.

---

# 65. 215A Phase BH — Diagnostics

Expose:

```text
RATION_PROTOCOLS_ACTIVE
RATION_RESOURCES_ACTIVE
RATION_CRISIS_COUNT
RATION_CRITICAL_CRISIS_COUNT
RATION_UNMET_DEMAND_TOTAL
RATION_PRIORITY_OVERRIDES
RATION_VIOLATIONS
RATION_REQUIRED_PORTS_MISSING
```

---

# 66. 215A Tests

- seven ration levels,
- fraction mapping,
- minimal validation,
- group projection,
- overlapping groups,
- allocation <= stock,
- allocation <= demand,
- reserve handling,
- crisis entry,
- crisis hysteresis,
- multi-shortage aggregation,
- authorization,
- old-save,
- legacy bool migration.

---

# 67. 215A Definition of Done

- [ ] `ResourceRationingSystem`,
- [ ] resource-domain adapters,
- [ ] 7 ration levels,
- [ ] dynamic priority groups,
- [ ] deterministic overlap policy,
- [ ] allocation solver,
- [ ] reserve policy,
- [ ] 5 protocol types,
- [ ] 6 crisis types,
- [ ] 4 severities,
- [ ] hysteresis,
- [ ] crisis responses,
- [ ] authorization,
- [ ] deterministic processing,
- [ ] save migration,
- [ ] legacy bool handling,
- [ ] events,
- [ ] ports,
- [ ] diagnostics.

---

# 68. Workstream 215B — Protocol Template Corpus

## Goal

Create reusable protocols representing different scarcity strategies without hardcoding one morally “correct” allocation.

---

# 69. 215B Phase A — Data File

Create:

```text
Assets/StreamingAssets/Data/rationing_protocols.json
```

---

# 70. 215B Phase B — Minimum Count

Source:

```text
10+ protocol templates
```

Recommended initial target:

```text
12 templates
```

---

# 71. 215B Phase C — Template 1: Standard Conservation

- mild reductions,
- broad fairness,
- modest reserves.

---

# 72. 215B Phase D — Template 2: Tight Food Rations

- food-focused,
- water unchanged,
- health-sensitive exceptions.

---

# 73. 215B Phase E — Template 3: Water Emergency

- water survival minimum,
- sanitation/production interactions explicit,
- critical medical exceptions.

---

# 74. 215B Phase F — Template 4: Medical Triage

- treatment priority,
- no generic “medicine per survivor.”

Priority driven by:

- treatment urgency,
- prognosis,
- medical policy.

---

# 75. 215B Phase G — Template 5: Fuel Conservation

- reduce nonessential power load,
- preserve heat/water/medical systems.

Uses PowerGrid.

---

# 76. 215B Phase H — Template 6: Worker Priority

- active critical workers receive higher nutritional/water minimum.

Potential moral/social consequences.

---

# 77. 215B Phase I — Template 7: Children First

- children prioritized.

Do not assume automatically morally approved by all survivors.

---

# 78. 215B Phase J — Template 8: Medical Staff Priority

- medics receive operational minimums.

---

# 79. 215B Phase K — Template 9: Leadership Continuity

Source includes leadership priority.

Make explicit/controversial:

- leader gets protected minimum,
- not necessarily more than children/medics.

---

# 80. 215B Phase L — Template 10: Equal Shares

- everyone same fraction of baseline demand,
- except hard medical/survival exceptions.

---

# 81. 215B Phase M — Template 11: Crisis Survival

- minimal across most resources,
- critical-role reserves,
- emergency-only.

---

# 82. 215B Phase N — Template 12: Multi-Resource Triage

- coordinated food/water/medical/fuel emergency profile.

---

# 83. 215B Phase O — Template Fields

```text
protocol_id
name_key
description_key
protocol_type
resource_policies
priority_scheme
reserve_rules
authorization_rule
default_duration
crisis_compatibility
communication_key
moral_context_tags
```

---

# 84. 215B Phase P — No Moral Labeling

Do not name templates:

```text
good rationing
evil rationing
```

Use descriptive policy names.

---

# 85. 215B Phase Q — Template Integrity

Validate:

- every resource ref,
- group ref,
- fraction,
- reserve,
- crisis type,
- localization key.

---

# 86. Workstream 215B — Allocation Engine

## Goal

Translate policy into actual per-consumer authorized quantities while preserving each domain’s baseline demand.

---

# 87. 215B Phase R — Allocation Request

Create:

```text
RationAllocationRequest
{
    resource_id
    allocation_window
    available_amount
    consumer_demands[]
    reserve_requirement
    active_policy
}
```

---

# 88. 215B Phase S — Consumer Demand

```text
consumer_id
consumer_type
baseline_requested
minimum_safe optional
priority_context
domain_context
```

---

# 89. 215B Phase T — Consumer Types

Examples:

```text
Survivor
Treatment
PowerLoad
ProductionProcess
ShelterService
```

This avoids forcing fuel/medicine into survivor-only allocation.

---

# 90. 215B Phase U — Allocation Result

```text
consumer_id
requested
authorized
shortfall
effective_priority
reason_codes
```

---

# 91. 215B Phase V — Full Mode Parity

With:

```text
Full
no reserve
no shortage
```

result must equal baseline requests.

---

# 92. 215B Phase W — Scarcity Allocation

If stock insufficient:

- never exceed stock,
- never allocate more than request,
- resolve priority deterministically.

---

# 93. 215B Phase X — Fraction Then Priority

Recommended:

1. compute policy target:
   ```text
   requested × ration fraction
   ```
2. guarantee minimums,
3. distribute stock by effective priority,
4. record shortfalls.

---

# 94. 215B Phase Y — Fairness Tie

Within same effective priority:

- proportional to authorized target,
- or equal fraction of demand.

Use deterministic algorithm.

---

# 95. 215B Phase Z — Remainder Handling

Discrete units can create leftovers.

Use stable round-robin by sorted consumer ID or largest remainder method.

No RNG.

---

# 96. 215B Phase AA — Unit Precision

Respect domain precision.

Do not allocate fractional inventory units if resource is discrete.

---

# 97. 215B Phase AB — Shortfall Record

Shortfall is a fact delivered to domain system/UI.

Do not automatically translate into health damage locally.

---

# 98. Workstream 215B — Food Rationing

## Goal

Integrate with Plan 22/KitchenNutrition while keeping one food authority.

---

# 99. 215B Phase AC — Meal Demand

KitchenNutrition supplies:

- planned nutrition per survivor,
- dietary/medical minimum if applicable.

---

# 100. 215B Phase AD — Ration Fraction

Rationing authorizes reduced meal target.

Kitchen executes recipe/resource consumption.

---

# 101. 215B Phase AE — Nutritional Consequence

Needs/health sees actual consumed nutrition.

No duplicate starvation calculation.

---

# 102. 215B Phase AF — Meal-Level UI

Show:

```text
normal target
ration target
actual served
shortfall
```

---

# 103. 215B Phase AG — Food Quality vs Quantity

Do not conflate:

- calorie ration,
- nutrition quality,
- contaminated food.

Existing systems own quality.

---

# 104. Workstream 215B — Water Rationing

## Goal

Allocate clean-water delivery without duplicating treatment/needs.

---

# 105. 215B Phase AH — Water Supply

WaterTreatment exposes:

- clean stock,
- production forecast,
- reserve.

---

# 106. 215B Phase AI — Survivor Water Demand

Needs/domain adapter exposes baseline.

---

# 107. 215B Phase AJ — Operational Water

If water is also needed for:

- medicine,
- sanitation,
- cooking,

model separate consumers.

Priority policy decides.

---

# 108. 215B Phase AK — No Double Consumption

Kitchen water usage and survivor drinking must reference same canonical pool.

---

# 109. Workstream 215B — Medical Rationing

## Goal

Implement triage correctly for treatment demand rather than generic daily medicine shares.

---

# 110. 215B Phase AL — Treatment Requests

MedicalPipeline exposes:

```text
treatment ID
patient
resource requirements
urgency
expected benefit
time window
```

---

# 111. 215B Phase AM — Medical Priority

Possible factors:

- immediate life threat,
- infection control,
- treatment window,
- patient policy group,
- medic judgment.

Do not simply prioritize leaders.

---

# 112. 215B Phase AN — Triage Transparency

UI shows:

```text
Treatment A receives antibiotics
Treatment B delayed
```

and why.

---

# 113. 215B Phase AO — Health Outcome

Medical system owns outcome.

Rationing records denied/partial supply.

---

# 114. Workstream 215B — Fuel / Power Rationing

## Goal

Turn fuel shortage into explicit load-shedding/reserve policy.

---

# 115. 215B Phase AP — PowerGrid Integration

PowerGrid exposes:

- generation,
- fuel demand,
- load priorities,
- storage.

---

# 116. 215B Phase AQ — Rationing as Load Policy

Fuel rationing can:

- reduce generator runtime,
- reserve fuel,
- disable nonessential loads.

PowerGrid executes.

---

# 117. 215B Phase AR — Essential Loads

Examples:

- heat,
- water,
- medical,
- life safety.

Reuse Plan 23 power priority where available.

Do not create a second power priority hierarchy.

---

# 118. 215B Phase AS — Plan 23 Boundary

If Plan 23 already owns:

```text
power tiers / cascade
```

ResourceRationingSystem requests:

```text
conservation severity / fuel reserve
```

and consumes PowerGrid result.

---

# 119. Workstream 215B — Priority Group Management

## Goal

Let player shape policy while preserving dynamic membership.

---

# 120. 215B Phase AT — Default Schemes

Data profiles:

```text
Equal
EssentialFirst
ChildrenFirst
MedicalFirst
WorkersFirst
LeadershipContinuity
Custom
```

---

# 121. 215B Phase AU — Custom Scheme

Player edits:

- group priority ordering,
- minimum guarantees,
- resource applicability.

Not member lists by default.

---

# 122. 215B Phase AV — Manual Survivor Override

Optional:

```text
survivor-specific priority override
```

for cases:

- severe illness,
- pregnancy if such system exists,
- critical mission preparation,
- punishment/denial if morally allowed.

Must be explicit and visible.

---

# 123. 215B Phase AW — Override Expiry

Manual override should have:

- duration,
- condition,
- or explicit indefinite marker.

Avoid forgotten permanent exceptional status.

---

# 124. 215B Phase AX — Dynamic Group Changes

If survivor becomes medic:

- next allocation reflects new group.

No save migration required.

---

# 125. 215B Phase AY — Dead/Departed Survivors

Automatically absent from membership.

---

# 126. 215B Phase AZ — Priority Explainability

For every allocation:

```text
"75% ration; Medical Staff priority; critical duty"
```

---

# 127. Workstream 215B — Crisis Lifecycle

## Goal

Make crisis state understandable, stable, and useful for strategic response.

---

# 128. 215B Phase BA — Crisis Detection Schedule

At:

- day start,
- major stock change,
- major population change,
- production outage.

No per-frame check.

---

# 129. 215B Phase BB — Projected Demand

Use:

- survivor count,
- current needs,
- treatment queue,
- power demand.

Avoid static threshold only.

---

# 130. 215B Phase BC — Crisis Declaration

Creates one stable crisis ID.

---

# 131. 215B Phase BD — Crisis Severity Change

Same ID escalates/de-escalates.

---

# 132. 215B Phase BE — Protocol Suggestion

Crisis can recommend:

- template,
- projected duration,
- effect.

Auto-ration setting may activate.

---

# 133. 215B Phase BF — Crisis Response Panel

Actions link to actual systems:

- ration,
- trade,
- expedition,
- production,
- power reduction.

---

# 134. 215B Phase BG — Crisis Resolution

Requires exit threshold.

Records:

- duration,
- worst severity,
- resources affected,
- unmet demand,
- major consequences.

---

# 135. 215B Phase BH — Crisis Recurrence

New crisis ID if later re-enters after genuine resolution.

---

# 136. Workstream 215B — Enforcement / Violations

## Goal

Make rule-breaking emerge from actual behavior rather than RNG-only flavor.

---

# 137. 215B Phase BI — Unauthorized Consumption Event

Define:

```text
RationViolationCandidate
{
    survivor_id
    resource_id
    amount
    source_action_id
    access_context
    day
}
```

---

# 138. 215B Phase BJ — Violation Producers

Potential:

- theft system,
- survivor autonomy,
- kitchen access,
- black market,
- conflict event.

If none exist yet:

- baseline violation system only validates/records explicit test/scripted events,
- do not fabricate random theft.

---

# 139. 215B Phase BK — Violation Validation

Compare:

```text
authorized allocation
vs
actual unauthorized extra consumption
```

---

# 140. 215B Phase BL — Violation Consequences

Route to:

- inventory,
- relations,
- morale,
- governance/discipline,
- conflict.

No local punishment stat.

---

# 141. 215B Phase BM — Enforcement Policy

Possible:

```text
None
Warning
Restitution
DutyPenalty
Detention
LeadershipReview
```

Only options supported by canonical systems.

---

# 142. 215B Phase BN — No Automatic Penalty

Player/governance may choose response.

Some violations can be sympathetic:

- starving child,
- sick survivor.

---

# 143. 215B Phase BO — Dispute Event

Priority dispute can be triggered by:

- persistent unequal allocation,
- relationship conflict,
- moral choice.

Do not roll random dispute daily.

---

# 144. Workstream 215B — Consequences

## Goal

Let the actual delivered shortage create consequences while policy context shapes social reaction.

---

# 145. 215B Phase BP — Health

Actual unmet food/water/medical needs flow into canonical health.

---

# 146. 215B Phase BQ — Morale

Possible event-based effects:

- rationing begins,
- rationing tightens,
- perceived unfairness,
- crisis resolved,
- equal treatment,
- protected vulnerable group.

Bound and reason-coded.

---

# 147. 215B Phase BR — Fairness Assessment

Derived:

```text
allocation policy
+ group impacts
+ survivor values/relations
+ transparency
```

Do not create universal “fairness score” unless another system owns it.

---

# 148. 215B Phase BS — Unequal Distribution

UI and social events must make unequal outcomes visible.

---

# 149. 215B Phase BT — Leadership Responsibility

Leadership authorizes policy.

Political consequences route to Leadership/legitimacy if Plan 208 supports.

---

# 150. 215B Phase BU — Death

If survivor dies due to prolonged unmet needs:

- canonical health/fate decides death,
- rationing history supplies cause context.

---

# 151. Workstream 215B — UI

## Goal

Make scarcity decisions comprehensible without turning them into spreadsheet work.

---

# 152. 215B Phase BV — Rationing Dashboard

Show:

```text
Resource
Stock
Days of supply
Current ration
Projected shortfall
Priority scheme
Crisis state
```

---

# 153. 215B Phase BW — Protocol Panel

Show:

- active protocol,
- resources affected,
- ration levels,
- priority scheme,
- reserve rules,
- authorized by,
- start/end,
- projected impact.

---

# 154. 215B Phase BX — Protocol Comparison

Before activation:

```text
current
vs
proposed
```

Show projected:

- stock lifetime,
- unmet demand,
- high-risk groups.

---

# 155. 215B Phase BY — Priority Panel

Show rules rather than only static member list.

Example:

```text
1. Children
2. Medical Staff
3. Essential Personnel
4. Workers
5. General
```

and current resolved membership preview.

---

# 156. 215B Phase BZ — Overlap Display

For survivor matching several groups:

```text
Mara — Medic, Worker, Essential → effective: Essential Personnel
```

---

# 157. 215B Phase CA — Resource Status

Show:

```text
available
reserved
baseline demand
authorized demand
actual delivered
days of supply
```

No duplicate resource numbers.

---

# 158. 215B Phase CB — Crisis Panel

Show:

- crisis type,
- severity,
- entry metric,
- exit metric,
- active responses,
- projected days to depletion,
- recommended actions.

---

# 159. 215B Phase CC — Violation Log

Only real violations.

Show:

- survivor,
- resource,
- unauthorized amount,
- context,
- response.

---

# 160. 215B Phase CD — No Precision Theater

If forecast uncertain:

- use bands/ranges.

Do not display false exact “4.327 days food” if demand is volatile.

---

# 161. 215B Phase CE — Actionable Summary

Top of dashboard:

```text
Water: CRITICAL — 1.8–2.2 days
Food: SEVERE — 4 days
Medical: Stable
Recommended: Water Emergency protocol
```

---

# 162. 215B Phase CF — Tutorial

First shortage explains:

- ration level,
- priority,
- days of supply,
- consequences,
- crisis resolution threshold.

---

# 163. 215B Phase CG — Tooltips

Hover + focus/details.

Plan 37/184.

---

# 164. 215B Phase CH — Large Text

2× text.

Tables reflow/scroll.

---

# 165. 215B Phase CI — No Color-Only Severity

Use labels/icons.

---

# 166. 215B Phase CJ — Screen Reader

Row announces:

```text
Water, critical shortage, half ration, 2 days supply.
```

---

# 167. 215B Phase CK — Cognitive Load Mode

Compact:

```text
What is low?
What is rationed?
Who is prioritized?
What should I do next?
```

Advanced solver details hidden under expand.

---

# 168. Workstream 215B — Source Events

Preserve:

```text
The Rationing
The Crisis
The Escalation
The Relief
The Depletion
The Dispute
The Violation
The Resolution
```

Emit as semantic/narrative events.

---

# 169. 215B Phase CL — The Rationing

Meaningful protocol activation.

---

# 170. 215B Phase CM — The Crisis

New crisis declared.

---

# 171. 215B Phase CN — The Escalation

Severity/protocol tightened.

---

# 172. 215B Phase CO — The Relief

Ration policy relaxed/lifted.

---

# 173. 215B Phase CP — The Depletion

Canonical stock reaches unavailable state.

---

# 174. 215B Phase CQ — The Dispute

Actual social/political dispute.

---

# 175. 215B Phase CR — The Violation

Validated unauthorized consumption.

---

# 176. 215B Phase CS — The Resolution

Crisis resolved.

---

# 177. Workstream 215B — Quest / Achievement Hooks

## Goal

Preserve source goals without incentivizing artificial crisis creation.

---

# 178. 215B Phase CT — Ownership

Plan 149/shared quest runtime owns achievement/quest state.

Rationing emits facts.

---

# 179. 215B Phase CU — The Quartermaster

Source:

```text
manage 10 rationing protocols
```

Define as:

- 10 distinct meaningful protocol activations/changes,
- no rapid toggle farming.

---

# 180. 215B Phase CV — The Crisis Manager

Resolve 5 genuine resource crises.

Player cannot create crisis by dumping stock purely for achievement without canonical consequences; achievement itself should not encourage destructive farming.

---

# 181. 215B Phase CW — The Fair Dealer

Zero validated ration violations for 100 days while rationing active.

Not 100 ordinary non-ration days.

---

# 182. 215B Phase CX — The Prioritizer

Maintain active priority allocation for 50 days with no catastrophic unresolved allocation failure.

Define precisely.

---

# 183. 215B Phase CY — The Survivor

Survive critical 3+ resource shortage.

Use MultipleShortage critical crisis.

---

# 184. 215B Phase CZ — The Strategist

“Prevent 5 crises” needs a counterfactual.

Do not infer impossible counterfactual casually.

Better:

```text
resolve 5 pre-crisis warnings before declaration
```

using explicit `ScarcityWarning` states.

---

# 185. 215B Phase DA — The Leader

10 difficult rationing decisions.

Define meaningful decision kinds:

- priority change,
- triage,
- emergency reserve release,
- severe protocol activation,
- dispute resolution.

No UI-click count.

---

# 186. 215B Definition of Done

- [ ] 10+ templates,
- [ ] recommended 12 protocols,
- [ ] allocation engine,
- [ ] food integration,
- [ ] water integration,
- [ ] medical triage,
- [ ] power/fuel conservation,
- [ ] dynamic group rules,
- [ ] custom priority,
- [ ] manual overrides,
- [ ] crisis lifecycle,
- [ ] enforcement,
- [ ] violation validation,
- [ ] social/health consequence routing,
- [ ] rationing dashboard,
- [ ] protocol/priority/crisis/resource/violation panels,
- [ ] source events,
- [ ] source hooks,
- [ ] tutorial/tooltips,
- [ ] accessibility.

---

# 187. Workstream 215C — NeedsSystem Integration

## Goal

Ensure rationing changes delivered consumption rather than directly mutating needs.

---

# 188. 215C Phase A — Needs Consumption Path Audit

Identify:

```text
food
water
sleep? not rationable
warmth? not direct resource
```

exact consumption handoffs.

---

# 189. 215C Phase B — Authorized Intake

Needs receives:

```text
actual consumed amount
```

from food/water domain.

No direct ration modifier on need decay unless architecture already expresses intake this way.

---

# 190. 215C Phase C — Double-Penalty Test

Half food ration must not:

```text
halve calories
+
also apply separate hunger penalty
```

unless second penalty is intentional social effect.

---

# 191. 215C Phase D — Minimum Survival

Needs/health defines physiological consequences.

Rationing only labels policy tier.

---

# 192. Workstream 215C — Inventory Integration

## Goal

Use inventory as source of resource availability and transaction truth.

---

# 193. 215C Phase E — Resource Snapshot

Inventory adapter returns:

- usable stock,
- reserved stock,
- inaccessible stock if modeled.

---

# 194. 215C Phase F — Reserved Stock

Rationing reserve policy must not duplicate item reservation.

Use canonical reservation mechanism if available.

---

# 195. 215C Phase G — Depletion Event

Inventory/domain emits resource depleted.

Rationing records crisis context.

---

# 196. 215C Phase H — Stock Changes

Major acquisition/consumption triggers crisis recalculation.

---

# 197. Workstream 215C — KitchenNutrition Integration

## Goal

Make rationing produce smaller/changed meals through one food pipeline.

---

# 198. 215C Phase I — Meal Plan Adapter

Rationing passes:

- authorized nutrition quantity by survivor/group,
- reserve constraints.

Kitchen chooses feasible recipes.

---

# 199. 215C Phase J — Scarcity Recipe Selection

If kitchen already owns substitution:

- reuse.

Rationing does not pick ingredient recipes itself.

---

# 200. 215C Phase K — Shared Meal Fairness

Kitchen actual delivered portions feed rationing history.

---

# 201. Workstream 215C — WaterTreatment Integration

## Goal

Coordinate drinking/operational water under scarcity.

---

# 202. 215C Phase L — Clean vs Contaminated Water

Rationing should not treat unsafe water as equivalent clean stock.

Water system supplies usable quantity/quality.

---

# 203. 215C Phase M — Production Forecast

Crisis days-of-supply can include expected treatment production only if forecast is authoritative enough.

---

# 204. Workstream 215C — PowerGrid Integration

## Goal

Reuse existing power dependency and load tiers.

---

# 205. 215C Phase N — Fuel Shortage

Rationing requests:

```text
fuel conservation mode
```

---

# 206. 215C Phase O — Power Load Priority

Plan 23 remains owner.

No second load order.

---

# 207. 215C Phase P — Critical Services

If rationing prioritizes essential personnel but power is out:

- consequences arise naturally.

No artificial compensation.

---

# 208. Workstream 215C — MedicalPipeline Integration

## Goal

Apply triage to real treatment demand.

---

# 209. 215C Phase Q — Treatment Queue

Medical pipeline exposes resource demand.

---

# 210. 215C Phase R — Allocation Decision

Rationing can:

- authorize,
- partially authorize if treatment supports partial,
- defer.

---

# 211. 215C Phase S — No Unsafe Partial Treatment

If treatment requires full dose/course:

- domain marks indivisible/minimum quantity.

Allocation solver respects.

---

# 212. 215C Phase T — Treatment Failure

Medical system owns clinical result.

---

# 213. Workstream 215C — DutyRoster Integration

## Goal

Use real worker/essential-personnel roles and reconcile legacy toggle.

---

# 214. 215C Phase U — Duty Tags

Duty system exposes:

```text
active duty
critical duty
medical duty
security duty
engineering duty
```

---

# 215. 215C Phase V — Worker Membership

Derived from active assignment.

---

# 216. 215C Phase W — Essential Personnel Membership

Derived from duty criticality.

---

# 217. 215C Phase X — Legacy Toggle

Document exact migration.

No dual behavior.

---

# 218. Workstream 215C — Leadership Integration

## Goal

Make rationing a political decision without duplicating leadership.

---

# 219. 215C Phase Y — Authorization

LeadershipSystem answers:

- authorized leader/deputy.

---

# 220. 215C Phase Z — Leadership Vacancy

If no leader:

- emergency policy may permit acting authority,
- otherwise player/system can use predefined emergency protocol.

No deadlock while people starve.

---

# 221. 215C Phase AA — Political Consequences

Plan 208 legitimacy may consume:

- harsh rationing,
- unequal favoritism,
- successful crisis response.

But only through public/known events.

---

# 222. Workstream 215C — InternalCommunication Integration

## Goal

Make rationing legible to survivors and avoid hidden policy changes.

---

# 223. 215C Phase AB — Plan 211 Announcement

On:

- protocol activation,
- escalation,
- priority change,
- relief,
- crisis resolution.

---

# 224. 215C Phase AC — Announcement Content

Include:

```text
what is rationed
why
who is prioritized
expected duration if known
```

---

# 225. 215C Phase AD — Transparency Effects

If communication system models trust/transparency:

- clear explanation can affect reaction.

Rationing does not calculate communication trust itself.

---

# 226. Workstream 215C — Save / Load / Idempotency

## Goal

Guarantee policy, crisis, allocation, and consequences survive save/load without double application.

---

# 227. 215C Phase AE — Save Matrix

Test:

```text
no rationing
protocol draft
pending authorization
active protocol
suspended protocol
indefinite protocol
date-expiring protocol
crisis warning
crisis active
severity escalation
crisis resolved
priority override
violation pending
violation resolved
```

---

# 228. 215C Phase AF — Allocation Idempotency

Allocation event keyed by:

```text
domain + allocation window + consumer + policy version
```

No double consumption authorization.

---

# 229. 215C Phase AG — Crisis Declaration Idempotency

Same threshold crossing:

- one crisis.

---

# 230. 215C Phase AH — Consequence Idempotency

Policy-start morale effect once.

Crisis escalation effect once per transition.

---

# 231. 215C Phase AI — Protocol Versioning

Changing active protocol creates:

```text
policy_version
```

so allocations use exact version.

---

# 232. 215C Phase AJ — Old Save

No active rationing.

If old bool mapping exists:

- deterministic compatibility migration.

---

# 233. Workstream 215C — Exploit Prevention

## Goal

Prevent policy toggling, stock manipulation, priority cycling, and violation/achievement farming.

---

# 234. 215C Phase AK — Toggle Farming

Rapid:

```text
activate
lift
activate
```

does not repeatedly grant events/rewards.

Use transition cooldown/history.

---

# 235. 215C Phase AL — Crisis Achievement Farming

Dumping stock intentionally may still create real crisis, but achievement criteria should not reward obviously synthetic loops.

Use distinct resource-crisis IDs + minimum duration/impact.

---

# 236. 215C Phase AM — Priority Cycling

Changing group ordering repeatedly:

- no repeated morale reward,
- may create instability if actually implemented.

---

# 237. 215C Phase AN — Leadership Favoritism Exploit

Cannot use group overlap to stack multipliers.

---

# 238. 215C Phase AO — Reserve Duplication

Reserved stock remains same inventory stock.

Do not copy into separate reserve pool.

---

# 239. 215C Phase AP — Forecast Exploit

Projected production is not spendable stock.

Allocation uses current usable stock plus explicit domain reservations.

---

# 240. 215C Phase AQ — Negative Demand Exploit

Clamp/reject invalid demand.

---

# 241. 215C Phase AR — Zero-Population Edge

No survivor consumption.

No divide by zero days-of-supply.

---

# 242. 215C Phase AS — Huge Population

Allocation scales.

No integer overflow.

---

# 243. 215C Phase AT — Violation Farming

Same unauthorized action ID:

- one violation.

---

# 244. 215C Phase AU — Meal Rounding Exploit

Discrete leftovers allocated deterministically.

No reload changing who gets extra unit.

---

# 245. Workstream 215C — Edge Cases

## Goal

Prove policy remains coherent under sparse stock, zero stock, multiple crises, changing roster, and contradictory priorities.

---

# 246. 215C Phase AV — No Rationing

Must reproduce current baseline.

---

# 247. 215C Phase AW — Full Rationing Level

Same as normal allocation when enough stock.

---

# 248. 215C Phase AX — None Level

No ordinary allocation.

Emergency exceptions explicit.

---

# 249. 215C Phase AY — Zero Stock

Allocation all zero.

Crisis critical if demand exists.

No divide by zero.

---

# 250. 215C Phase AZ — Infinite/Very Large Stock

No crisis.

Full protocol parity.

---

# 251. 215C Phase BA — All Resources Low

Individual crises + derived multiple shortage.

No duplicate health/morale consequences.

---

# 252. 215C Phase BB — One Resource Recovers

Aggregate severity recalculates.

---

# 253. 215C Phase BC — Population Doubles

Days-of-supply drops.

Crisis may open without stock quantity changing.

Correct.

---

# 254. 215C Phase BD — Population Drops

Crisis may resolve as demand falls.

Record resolution reason.

---

# 255. 215C Phase BE — Role Change Mid-Day

Membership changes next allocation window unless domain supports immediate recalculation.

---

# 256. 215C Phase BF — Leader Dies Mid-Protocol

Active protocol remains until policy says otherwise.

Authorization history preserved.

New changes require current authority.

---

# 257. 215C Phase BG — Priority Tie

Stable deterministic resolution.

---

# 258. 215C Phase BH — Everyone High Priority

Solver still respects stock.

Priority does not create resources.

---

# 259. 215C Phase BI — Medical Indivisible Demand

One full treatment may outrank partial distribution.

Domain rule.

---

# 260. 215C Phase BJ — Power / Food Simultaneous Crisis

Cross-domain policy should not create circular dependency.

---

# 261. Workstream 215C — Determinism

## Goal

Guarantee identical allocation and crisis outcomes from identical state.

---

# 262. 215C Phase BK — Stable Consumer Ordering

Sort by stable consumer ID after priority.

---

# 263. 215C Phase BL — Decimal Precision

Use deterministic decimal/fixed arithmetic where resource system does.

Avoid platform-dependent float edge behavior for discrete allocation.

---

# 264. 215C Phase BM — Same-State Allocation Digest

Digest:

```text
policy version
resource
consumer
requested
authorized
shortfall
priority
```

---

# 265. 215C Phase BN — Same-Seed Crisis Digest

Includes:

```text
crisis IDs
severity transitions
protocol changes
violations/reactions
```

---

# 266. Workstream 215C — Data Integrity

## Goal

Reject broken rationing data and cross-system references before runtime.

---

# 267. 215C Phase BO — Protocol Validation

Validate:

- unique protocol ID,
- valid type,
- valid resource/domain,
- valid ration level,
- valid fraction,
- valid priority scheme,
- valid authorization,
- valid localization.

---

# 268. 215C Phase BP — Priority Scheme Validation

Validate:

- unique group IDs,
- priority range 1–5,
- no negative weights,
- fallback General exists,
- overlap policy exists.

---

# 269. 215C Phase BQ — Crisis Validation

Validate:

- enter thresholds,
- exit threshold/hysteresis,
- severity ordering,
- resource refs,
- response action refs.

---

# 270. 215C Phase BR — Hysteresis Integrity

Fail if:

```text
resolve threshold <= entry threshold
```

for metrics where higher is safer.

---

# 271. 215C Phase BS — Allocation Integrity

Property tests:

```text
0 <= allocated_i <= requested_i
sum allocated <= available
```

---

# 272. 215C Phase BT — Domain Adapter Integrity

Every rationed domain has:

- stock provider,
- demand provider,
- consumption executor/adapter.

---

# 273. 215C Phase BU — No Duplicate Resource Authority Scan

Flag new persistent fields:

```text
foodStock
waterStock
medicineStock
fuelStock
```

inside rationing state.

---

# 274. Workstream 215C — `--resource-rationing-selftest`

Required scenarios:

1. full ration,
2. three-quarter,
3. half,
4. third,
5. quarter,
6. minimal,
7. none,
8. essential-personnel group,
9. children group,
10. elderly group,
11. medics group,
12. leadership group,
13. workers group,
14. general group,
15. overlapping-group survivor,
16. food shortage,
17. water shortage,
18. medical shortage,
19. fuel shortage,
20. general scarcity,
21. multiple shortage,
22. crisis escalation,
23. hysteresis prevents chatter,
24. crisis resolution,
25. auto-ration,
26. manual authorization,
27. allocation <= stock,
28. reserve protection,
29. food integration,
30. water integration,
31. medical triage,
32. power conservation,
33. violation event,
34. no fabricated violation,
35. save/load,
36. old save,
37. legacy toggle migration,
38. no rationing,
39. all resources critical,
40. same-state deterministic replay.

---

# 275. Workstream 215C — Deliberate Failure Proof

Break:

- invalid resource ID,
- invalid ration level,
- fraction > 1,
- negative fraction,
- missing General fallback,
- invalid priority 0/6,
- bad hysteresis,
- allocation exceeding stock,
- duplicate protocol ID,
- missing domain adapter.

Assert correct gate fails.

---

# 276. Workstream 215C — 200-Day Scarcity Soak

## Goal

Prove rationing changes strategic survival without dominating ordinary play.

---

# 277. 215C Phase BV — Strategy Profiles

Run:

```text
never_ration
late_reactive
early_conservation
children_first
essential_first
equal_shares
medical_triage
aggressive_multi_resource
```

---

# 278. 215C Phase BW — Metrics

Record:

```text
days of supply by resource
crises declared
critical crisis days
rationed survivor-days
unmet food demand
unmet water demand
deferred treatments
fuel saved
morale events
health outcomes
violations
deaths with shortage context
```

---

# 279. 215C Phase BX — Compare No-Ration Baseline

Rationing should:

- extend resource horizon,
- redistribute suffering,
- create meaningful tradeoffs.

It should not magically reduce total physical demand without consequence.

---

# 280. 215C Phase BY — Early Conservation

Potential:

- lower short-term morale/needs,
- longer reserve duration,
- fewer catastrophic zero-stock days.

---

# 281. 215C Phase BZ — Late Reactive

Potential:

- higher early comfort,
- sharper late crisis.

---

# 282. 215C Phase CA — Priority Tradeoff

Compare:

- equal,
- children,
- workers,
- medics.

Measure:

- production,
- mortality,
- morale,
- long-run recovery.

No one policy should dominate all conditions.

---

# 283. 215C Phase CB — Leadership Priority Audit

Ensure:

```text
Leadership priority
```

is not mechanically best by default.

It is a political choice.

---

# 284. 215C Phase CC — Minimal Ration Calibration

Minimal must not accidentally be:

- healthier than intended,
- equivalent to death sentence immediately.

Tune per domain.

---

# 285. 215C Phase CD — Medical Triage Fairness

Review whether treatment prioritization creates impossible/irrational outcomes.

---

# 286. 215C Phase CE — Crisis Duration

Crises should remain long enough to matter but resolve promptly after genuine recovery.

Hysteresis tuned.

---

# 287. 215C Phase CF — Notification Budget

Measure:

```text
rationing notifications/day
crisis notifications/day
```

Only transitions, not every shortfall.

---

# 288. 215C Phase CG — Spreadsheet Risk Mitigation

Source risk notes spreadsheet management.

Mitigation:

- protocol presets,
- recommended actions,
- days-of-supply,
- compact consequences,
- explainable priorities,
- auto-ration option.

---

# 289. Workstream 215C — Performance

## Goal

Keep allocation computation bounded even in large shelters.

---

# 290. 215C Phase CH — Complexity

Typical:

```text
O(consumers log consumers)
```

per rationed domain/window due sorting.

Acceptable.

---

# 291. 215C Phase CI — Cache Dynamic Membership

Cache within allocation window.

Invalidate on:

- duty change,
- life-stage change,
- leader change,
- survivor join/leave/death.

---

# 292. 215C Phase CJ — Avoid Daily Full History Scan

Only active protocols/crises.

---

# 293. 215C Phase CK — Crisis Metrics Cache

Domain providers may cache forecasts.

Rationing does not recalculate expensive production graphs unnecessarily.

---

# 294. 215C Phase CL — Stress Fixture

Test:

```text
100 survivors
10 resource classes
7 groups
5 simultaneous crises
```

Measure:

- day tick,
- allocation,
- UI read-model build,
- save/load.

---

# 295. Workstream 215C — Accessibility

## Goal

Make difficult allocation decisions understandable under Plan 184.

---

# 296. 215C Phase CM — Large Text

Dashboard and tables support 2× text.

---

# 297. 215C Phase CN — No Color-Only Scarcity

Severity uses:

- text,
- icon,
- shape.

---

# 298. 215C Phase CO — Screen Reader

Priority row announces:

```text
Children, priority 1, guaranteed 75 percent food ration.
```

---

# 299. 215C Phase CP — Keyboard / Controller

All protocol creation/edit/authorization actions accessible.

---

# 300. 215C Phase CQ — Cognitive Load Reduction

Default compact view emphasizes:

```text
resource at risk
days remaining
current ration
who gets priority
recommended action
```

---

# 301. Workstream 215C — Retention / Archive

## Goal

Keep famous scarcity crises without bloating saves with every allocation row.

---

# 302. 215C Phase CR — Plan 55 Retention

Persist fully:

- active protocols,
- active crises,
- unresolved violations,
- recent policy history.

Roll up:

- old daily allocations,
- resolved minor shortfalls.

---

# 303. 215C Phase CS — Crisis Summary

For resolved crisis retain:

```text
type
start/end
peak severity
protocols used
unmet demand totals
major consequences
```

---

# 304. 215C Phase CT — Plan 162 Archive

Landmark:

- major famine,
- famous water crisis,
- controversial triage,
- successful conservation campaign.

Not every shortage.

---

# 305. Workstream 215C — Human Playtest

## Goal

Verify rationing feels like leadership under scarcity rather than arbitrary punishment or spreadsheet optimization.

---

# 306. 215C Phase CU — Playtest Questions

```text
Could the player tell why a crisis was declared?
Could they predict what a protocol would change?
Did priorities feel understandable?
Did the system show who received less?
Did rationing extend resources in an intuitively plausible way?
Did health consequences come from actual deprivation?
Did political/social reactions reflect unequal policy?
Was there a meaningful reason to choose one protocol over another?
```

---

# 307. 215C Phase CV — One-Decision Test

Present:

```text
2 days water
5 days food
1 antibiotic course
fuel for 3 cold nights
```

Player should be able to identify meaningful policy choices within one panel.

---

# 308. 215C Phase CW — Fairness Test

Run:

- equal shares,
- children first,
- essential workers first.

Human review should understand why survivors disagree.

---

# 309. 215C Phase CX — Recovery Test

After major resupply:

- crisis should de-escalate/resolve,
- rationing should not remain silently active forever,
- UI should recommend relief.

---

# 310. Documentation

Create:

```text
docs/systems/RESOURCE_RATIONING_AND_SCARCITY.md
```

Include:

- ownership boundaries,
- resource-domain adapters,
- ration levels,
- dynamic priority groups,
- overlap rules,
- allocation algorithm,
- crisis metrics/hysteresis,
- violations,
- authorization,
- save/idempotency,
- balance.

---

# 311. Data Authoring Guide

Create:

```text
docs/content/RATIONING_PROTOCOL_AUTHORING.md
```

Checklist:

```text
1. choose protocol type
2. define affected resource domains
3. define ration fractions
4. define reserve policy
5. define priority scheme
6. define authorization
7. define crisis compatibility
8. define communication text
9. validate domain adapters
10. add targeted selftest
```

---

# 312. Integrated Rationing Pipeline

```text
canonical stock + canonical demand
              │
              ▼
      scarcity assessment
              │
              ▼
 ResourceRationingSystem
              │
       ┌──────┼───────────┐
       ▼      ▼           ▼
 protocol  priority     crisis
       │      │           │
       └──────┼───────────┘
              ▼
       allocation policy
              │
              ▼
   domain allocation adapters
       │      │      │      │
       ▼      ▼      ▼      ▼
     Food   Water  Medical  Power
       │      │      │      │
       ▼      ▼      ▼      ▼
 canonical consumption/execution
              │
              ▼
      delivered / denied facts
              │
     ┌────────┼──────────┐
     ▼        ▼          ▼
   Needs    Health     Morale/
                        Social
```

---

# 313. Rationing Authority Contract

`ResourceRationingSystem` owns:

- active rationing policy,
- ration levels,
- priority rules,
- scarcity crisis state,
- protocol authorization/history,
- allocation decisions,
- violation records.

---

# 314. Inventory Authority Contract

Inventory/resource systems own:

- quantity,
- item identity,
- reservation/transaction truth.

No duplicate stock.

---

# 315. Needs Authority Contract

Needs owns:

- hunger,
- thirst,
- physiological response.

Rationing supplies delivered amount context.

---

# 316. Food Authority Contract

Plan 22/Kitchen owns:

- food consumption,
- meal construction,
- nutrition.

Rationing caps/allocates.

---

# 317. Water Authority Contract

WaterTreatment owns:

- production,
- cleanliness,
- availability.

Rationing allocates usable supply.

---

# 318. Medical Authority Contract

MedicalPipeline owns:

- treatment necessity,
- clinical outcome,
- resource consumption.

Rationing triages resource authorization.

---

# 319. Power Authority Contract

Plan 23/PowerGrid owns:

- generation,
- load priority,
- battery/fuel behavior.

Rationing requests conservation/reserve policy.

---

# 320. Duty Authority Contract

DutyRoster owns:

- current role/duty.

Priority groups query it.

---

# 321. Leadership Authority Contract

Leadership owns:

- authorized political actor.

Rationing records authorization.

---

# 322. Communication Authority Contract

Plan 211 owns internal communication delivery.

Rationing supplies announcements.

---

# 323. Priority Contract

Priority groups are policy projections.

Do not persist stale member lists as authority.

---

# 324. Overlap Contract

One effective priority result per consumer/resource allocation.

No stacked group multipliers.

---

# 325. Crisis Contract

Crisis is demand-relative and hysteresis-protected.

---

# 326. Violation Contract

Violation requires real unauthorized consumption.

No random crime fabrication.

---

# 327. Consequence Contract

Health/morale/social effects route to canonical owners.

---

# 328. Save Contract

Persist policy/crisis/history/idempotency.

Do not duplicate stock/needs/group membership.

---

# 329. Old-Save Contract

No active rationing by default.

Legacy toggle migration explicit.

---

# 330. Determinism Contract

Same:

```text
resource stock
demand
survivor roles
policy
seed
```

→ same allocation/crisis history.

---

# 331. Attention Contract

Notify transitions and decisions, not every routine allocation.

---

# 332. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| duplicate stock ledger | Medium | Critical | read-only stock adapter |
| direct need penalties double-count deprivation | High | Critical | actual-delivery integration |
| static group membership becomes stale | High | High | dynamic projection |
| overlapping groups multiply rations | High | High | highest-priority resolution |
| group percentages overallocate stock | High | Critical | bounded solver |
| crisis threshold chatters | High | Medium | hysteresis |
| medical rationing modeled as daily shares | Medium | High | treatment-request adapter |
| power priorities duplicated | Medium | High | Plan 23 handoff |
| random violations feel arbitrary | High | High | real behavior events only |
| leadership privilege hardcoded | Medium | Medium | data-driven priority |
| ration toggling farms rewards | Medium | High | transition idempotency |
| old save loses existing toggle behavior | Medium | Medium | explicit migration |
| rationing becomes spreadsheet-heavy | High | Medium | presets + actionable UI |
| multiple crises double-apply consequences | Medium | High | aggregate crisis derivation |

---

# 333. Commit Strategy

## 215A — Foundation

### C2[44].1 — baseline + rationing ownership ADR

### C2[44].2 — legacy `mutationRationProtocol` audit/migration

### C2[44].3 — ration levels / resource domains

### C2[44].4 — dynamic priority-group rules

### C2[44].5 — allocation solver / overlap policy

### C2[44].6 — protocol lifecycle / authorization

### C2[44].7 — crisis types / severity / hysteresis

### C2[44].8 — crisis response / event contracts

### C2[44].9 — save/versioning / old-save behavior

### C2[44].10 — composition/ports/diagnostics

### Gate: 215A complete

---

## 215B — Protocols / Runtime / UI

### C2[44].11 — 12 protocol templates

### C2[44].12 — food allocation adapter

### C2[44].13 — water allocation adapter

### C2[44].14 — medical triage adapter

### C2[44].15 — fuel/power conservation adapter

### C2[44].16 — custom priority / overrides

### C2[44].17 — crisis lifecycle/runtime

### C2[44].18 — enforcement/violation intake

### C2[44].19 — consequence routing

### C2[44].20 — rationing dashboard/protocol panel

### C2[44].21 — priority/crisis/resource/violation UI

### C2[44].22 — events/hooks/tutorial/accessibility

### Gate: 215B complete

---

## 215C — Integration / Validation

### C2[44].23 — Needs integration

### C2[44].24 — Inventory / Kitchen integration

### C2[44].25 — Water / Medical integration

### C2[44].26 — Power / Duty integration

### C2[44].27 — Leadership / InternalCommunication

### C2[44].28 — save-load/idempotency matrix

### C2[44].29 — exploit prevention

### C2[44].30 — edge cases

### C2[44].31 — data-integrity / solver properties

### C2[44].32 — `--resource-rationing-selftest`

### C2[44].33 — deliberate failure proof

### C2[44].34 — 200-day scarcity soak

### C2[44].35 — strategy/balance comparison

### C2[44].36 — performance/accessibility/retention

### C2[44].37 — human playtest/docs/release closure

### Gate: 215C complete

---

# 334. Verification Checklist

Run source-required commands:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --resource-rationing-selftest
```

Also run repository-canonical equivalents of:

```text
rationing single-resource-authority audit
allocation conservation/property tests
priority overlap test
crisis hysteresis test
old-save legacy-toggle migration fixture
food/water/medical/power adapter parity tests
same-state ration allocation digest replay
200-day scarcity strategy soak
large-shelter rationing performance test
rationing UI accessibility/runtime parity
```

---

# 335. `--resource-rationing-selftest` Acceptance Matrix

| Scenario | Expected |
|---|---|
| Full | baseline demand |
| Three-quarter | 75% target |
| Half | 50% target |
| Third | 33% target |
| Quarter | 25% target |
| Minimal | domain-defined survival minimum |
| None | zero ordinary allocation |
| Essential | dynamic membership |
| Children | canonical life-stage |
| Elderly | canonical life-stage |
| Medics | active role |
| Leadership | current leadership |
| Workers | active duty |
| General | fallback |
| Overlap | one effective priority |
| Food crisis | canonical stock/demand |
| Water crisis | canonical stock/demand |
| Medical crisis | treatment demand |
| Fuel crisis | power/fuel demand |
| General scarcity | multi-resource pressure |
| Multiple shortage | >= configured severe resources |
| Hysteresis | no threshold chatter |
| Auto ration | policy activates once |
| Manual mode | no silent activation |
| Violation | real unauthorized event |
| No violation source | no fabricated crime |
| Save/load | exact policy/crisis |
| Old save | no active rationing |
| Legacy bool | migrated explicitly |
| All resources low | bounded aggregate |
| Same state | same digest |

---

# 336. Flagship Definition of Done — Foundation

- [ ] `ResourceRationingSystem.cs`,
- [ ] `CaptureState/RestoreState`,
- [ ] 7 ration levels,
- [ ] 7 priority-group concepts,
- [ ] 6 crisis types,
- [ ] 4 severity levels,
- [ ] dynamic group membership,
- [ ] overlap policy,
- [ ] bounded allocation solver,
- [ ] reserve policy,
- [ ] authorization,
- [ ] crisis hysteresis,
- [ ] deterministic state,
- [ ] old-save compatibility,
- [ ] legacy bool migration,
- [ ] semantic events,
- [ ] ports,
- [ ] diagnostics.

---

# 337. Flagship Definition of Done — Runtime / UI

- [ ] protocol activation/suspension/lift,
- [ ] per-resource ration policy,
- [ ] food rationing,
- [ ] water rationing,
- [ ] medical triage,
- [ ] fuel/power conservation,
- [ ] priority schemes,
- [ ] custom overrides,
- [ ] crisis declaration/escalation/resolution,
- [ ] crisis response actions,
- [ ] violation intake,
- [ ] consequence routing,
- [ ] 10+ protocol templates,
- [ ] recommended 12 templates,
- [ ] dashboard,
- [ ] protocol detail,
- [ ] priority panel,
- [ ] crisis panel,
- [ ] resource status,
- [ ] violation log,
- [ ] source events/hooks,
- [ ] tutorial/tooltips,
- [ ] accessibility.

---

# 338. Flagship Definition of Done — Integration / Validation

- [ ] NeedsSystem,
- [ ] Inventory,
- [ ] KitchenNutritionSystem,
- [ ] WaterTreatmentSystem,
- [ ] PowerGridSystem,
- [ ] MedicalPipelineCoordinator,
- [ ] DutyRosterSystem,
- [ ] LeadershipSystem,
- [ ] InternalCommunicationSystem,
- [ ] morale/health/social sinks,
- [ ] no duplicate resource truth,
- [ ] no duplicate need effects,
- [ ] save/load lifecycle,
- [ ] allocation/crisis/consequence idempotency,
- [ ] anti-toggle/priority/violation farming,
- [ ] no-rationing/full-crisis edges,
- [ ] zero-stock / zero-population edges,
- [ ] simultaneous crises,
- [ ] deterministic replay,
- [ ] data integrity,
- [ ] property tests,
- [ ] deliberate failure fixtures,
- [ ] selftest,
- [ ] 200-day soak,
- [ ] strategy comparisons,
- [ ] performance,
- [ ] accessibility,
- [ ] retention/archive,
- [ ] human playtest,
- [ ] docs.

---

# 339. Global Definition of Done

- [ ] no duplicate food/water/medicine/fuel ledger,
- [ ] no duplicate hunger/thirst/health simulation,
- [ ] no duplicated power priority hierarchy,
- [ ] no stale persisted group membership,
- [ ] no stacked priority multipliers,
- [ ] no allocation above stock,
- [ ] no random fabricated violations,
- [ ] no crisis threshold chatter,
- [ ] no repeated morale/health consequences per tick,
- [ ] no old-save regression,
- [ ] no spreadsheet-only UX,
- [ ] full verification green.

---

# 340. Closure Report Template

```markdown
## C2[44] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- DutyRoster mutationRationProtocol semantics:
- Food authority:
- Needs authority:
- Inventory authority:
- Water authority:
- Power authority:
- Medical authority:
- Leadership authorization:
- Communication:
- Existing ration-related code:

### 215A — Foundation
- ResourceRationingSystem:
- Ration levels:
- Resource domains:
- Priority groups:
- Dynamic membership:
- Overlap policy:
- Allocation algorithm:
- Reserve policy:
- Protocol types:
- Crisis types:
- Hysteresis:
- Authorization:
- Save schema:
- Legacy migration:
- Missing ports:
- Result:

### 215B — Runtime / UI
- Protocol templates:
- Food adapter:
- Water adapter:
- Medical adapter:
- Power adapter:
- Active protocols:
- Crisis declarations:
- Escalations:
- Resolutions:
- Violations:
- UI:
- Events/hooks:
- Result:

### 215C — Integration
- Needs:
- Inventory:
- Kitchen:
- Water:
- Power:
- Medical:
- DutyRoster:
- Leadership:
- InternalCommunication:
- Morale/health:
- Duplicate stock findings:
- Double need penalty findings:
- Result:

### Balance / Soak
- 200-day crises:
- Critical crisis days:
- Food shortfall:
- Water shortfall:
- Deferred treatments:
- Fuel conserved:
- Rationed survivor-days:
- Violations:
- Morale events:
- Shortage-linked deaths:
- Best/worst strategy:
- Dominant priority policy:
- Notification rate:
- Spreadsheet-UX findings:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Rationing selftest:
- Resource-authority audit:
- Allocation property tests:
- Overlap test:
- Hysteresis test:
- Old-save migration:
- Same-state digest:
- 200-day soak:
- Performance:
- Accessibility:
- Result:

### Final Metrics
- RATION_PROTOCOLS_ACTIVE:
- RATION_CRISES_TOTAL:
- RATION_CRITICAL_CRISIS_DAYS:
- RATION_UNMET_FOOD:
- RATION_UNMET_WATER:
- RATION_DEFERRED_TREATMENTS:
- RATION_FUEL_RESERVED:
- RATION_VIOLATIONS:
- RATION_PRIORITY_OVERLAP_ERRORS:
- RATION_ALLOCATION_OVERFLOW_ERRORS:
- RATION_DUPLICATE_RESOURCE_AUTHORITY_ERRORS:
- RATION_DOUBLE_CONSEQUENCE_ERRORS:
- RATION_CRISIS_CHATTER_EVENTS:
- REQUIRED_PORTS_MISSING:

### Remaining Debt
- Protocol content:
- Violation producers:
- Governance depth:
- Forecasting:
- Trade/expedition response actions:
- UI:
```

---

# 341. Final Execution Directive

Execute Plan 215 as a **scarcity policy and allocation layer over canonical resource, need, medical, power, duty, and leadership systems**.

The critical sequence is:

```text
audit the existing resource/consumption paths
→ establish ResourceRationingSystem as policy authority only
→ convert the legacy ration toggle safely
→ define seven ration levels and dynamic priority rules
→ implement one bounded allocation solver
→ connect domain-specific demand/stock adapters
→ add crisis detection with demand-relative thresholds and hysteresis
→ authorize protocols through real leadership
→ execute food/water/medical/power allocations through canonical owners
→ derive health consequences from actual unmet demand
→ surface social/political consequences from explicit policy choices
→ validate violations only from real unauthorized consumption
→ prove deterministic save/load and long-run scarcity balance
```

Do not create parallel food, water, medicine, or fuel balances.

Do not write hunger/thirst/health penalties directly when actual reduced consumption already produces them.

Do not persist “medics,” “children,” “workers,” or “leadership” membership as static survivor lists.

Do not stack multiple priority groups into multiplicative ration bonuses.

Do not use one crisis threshold for both declaration and resolution.

Do not invent ration theft simply because the system needs a violation event.

The strongest authority rule is:

> **`ResourceRationingSystem` owns what the shelter authorizes and prioritizes under scarcity; the resource-producing and consuming systems remain the sole authorities for what physically exists, what is actually consumed, and what physiological or operational result follows.**

The strongest allocation rule is:

> **Rationing can redistribute and defer scarcity, but it can never create resources. Every authorized allocation must remain bounded by both canonical demand and canonical available stock.**

The strongest crisis rule is:

> **A shortage is defined relative to projected demand and must cross a real entry threshold to begin and a safer exit threshold to resolve, preventing crisis-state chatter around one cutoff.**

The flagship acceptance scenario is:

> **Start a seeded 20-survivor shelter with 4 days of food, 2 days of clean water, one critical antibiotic course, and fuel for 3 cold nights. Activate a multi-resource emergency protocol authorized by the canonical shelter leader. Resolve current survivor roles dynamically so a medic who is also an essential worker receives one effective priority rather than stacked bonuses; children, elderly survivors, workers, leadership, and general survivors resolve according to the configured scheme. Allocate food and water without exceeding actual Inventory/WaterTreatment stock, send reduced meal/water quantities through Kitchen/Needs, reserve the antibiotic for one canonical high-urgency treatment through MedicalPipeline, and request fuel conservation through PowerGrid rather than duplicating load priority. Save/load before the next allocation window and verify identical authorized quantities and remainder assignment. Increase water above the entry threshold but below the higher resolution threshold: the water crisis must remain active. Increase it above the exit threshold: resolve exactly once. Then switch to Equal Shares and prove group overlap cannot create extra resources. Finally inject one real unauthorized food-consumption event and verify exactly one violation is recorded; without such an event, no random violation may appear. Run `--resource-rationing-selftest`, allocation conservation/property tests, the old-save legacy-toggle fixture, and the 200-day scarcity strategy soak with zero duplicate resource authorities and zero double-applied deprivation effects.**
