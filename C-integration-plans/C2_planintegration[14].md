# C2 — Flagship Integration Plan [14]: Goods Must Arrive

> **Source:** Plan 35 — Goods Must Arrive: The Production-to-Provisioning Chain
> **Wave:** Continuity Wave 5 — The Human Interface
> **Execution order:** **35A → 36A → 35B → 35C**
> **Depends on:** 22A single-consume authority; coordinates with 21B condition, 23A power, 24A labour/fitness, 31 semantic kinds, and 27A fixture fidelity.
> **Parallel sequencing:** Plan 136 hunting/cooking content lands after 35A. Plan 141 owns research-unlock content; 35C owns only the mechanical gate.
> **Guardrails:** no new producer system, no new resource currency, no rival panel framework, no silent overflow, no parallel port architecture where CombatHostPorts/InventoryBill already solve the pattern.

---

## 0. Mission

The repository already contains numerous production systems, but it has no universal definition of “delivered.” Greenhouse output reaches inventory; trapping can record a catch without creating goods; water exists as both treatment-plant litres and inventory `clean_water`; other producers use a mixture of ports, bills, direct inventory mutation, and local state.

This plan closes the production chain:

```text
inputs + labour + time + power + condition
→ producer
→ DeliveryBill
→ validated output sink
→ capacity/storage
→ inventory or bulk authority
→ spoilage/contamination/waste
→ use/trade/consumption
→ semantic events + UI
```

A producer is not complete because a timer ends or a panel says READY. It is complete only when output reaches an authoritative sink or returns an explicit typed refusal/waste result.

The program-level accounting invariant is:

```text
produced ≈ delivered + consumed + spoiled + wasted + refused
```

over a deterministic long-campaign soak, with explicit transform terms where items change identity.

---

## 1. Architectural Invariants

1. Every production-critical output port is mandatory and validated.
2. `InventoryBill` remains the canonical input/consumption vocabulary.
3. `DeliveryBill` becomes its output-side counterpart.
4. Every producer either delivers or returns a typed reason.
5. No output may remain only inside producer-local state.
6. Capacity failure is explicit; no silent clipping.
7. Water gets one documented authority/conversion model.
8. Required inventory/output dependencies are non-nullable.
9. Save/load must never double-consume inputs or double-deliver output.
10. Production events carry source-system attribution.
11. UI ETA/output/risk reads canonical Core calculations.
12. Producer registration with unbound required effects fails a gate.
13. Every non-automatic producer declares inputs, labour, duration, and output.
14. Power, fitness, and equipment condition come from their existing authorities.
15. Long-running producer actions use state-versioned command semantics.
16. Mass-balance reconciliation is a CI/soak artifact, not an informal QA note.

---

## 2. Dependency Graph

```text
22A single consume authority
          │
          ▼
35A delivery contract
          │
          ▼
36A producer-port enforcement
          │
          ▼
35B storage/spoilage
          │
          ▼
35C full production loop

21B condition ─────────────► quality / equipment degradation
23A power ─────────────────► producer halt + refrigeration
24A labour/fitness ────────► duty roster + operator quality
31 semantic kinds ─────────► delivery/refusal/failure attribution
27A fixture fidelity ──────► trustworthy producer census

Plan 136 hunting/cooking ──► AFTER 35A
Plan 141 research unlocks ─► content design outside 35C
```

Do not reorder this as 35A → 35B → 35C → 36. The enforcement gate lands immediately after 35A.

---

## 3. Baseline Capture

Before implementation, capture:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --survivors-selftest
godot --headless --path . -- --content-utilization-selftest
bash scripts/ci/verify-fast.sh
```

Publish a producer census:

| Producer | Inputs | Outputs | Sink bound? | Capacity? | Event? | Save state? | Skill? | Labour? | Power? |
|---|---|---|---:|---:|---:|---:|---:|---:|---:|
| greenhouse | | | | | | | | | |
| apiculture | | | | | | | | | |
| trapping | | | | | | | | | |
| water treatment | | | | | | | | | |
| brine/desal | | | | | | | | | |
| pharma | | | | | | | | | |
| foundry | | | | | | | | | |
| excavation | | | | | | | | | |
| deep-coast salvage | | | | | | | | | |
| kitchen | | | | | | | | | |
| hygiene/laundry if present | | | | | | | | | |

Re-measure current code rather than trusting historical counts.

---

# PART I — 35A: A Delivery Contract Every Producer Implements

## 4. Objective

Create one repository-wide contract for “this producer created output and attempted to give it to the campaign.”

Combat already establishes the correct precedent: explicit bound ports plus validation of required effects. Reuse that discipline.

---

## 5. ProducerPorts

Create/reuse a producer-side port record. Conceptually:

```csharp
ProducerPorts
{
    deliverItem,
    consumeInput,
    recordWaste,
    emitEffect,
    ...
}
```

Keep the surface narrow.

### Required behaviors

- each port is typed;
- required ports are declared by producer capability;
- `UnboundRequiredEffects` lists missing production-critical effects;
- `ValidatePorts()` emits a deterministic diagnostic;
- campaign composition fails loudly when required ports are missing;
- previews/tests use explicit fake sinks rather than null.

Do not turn ProducerPorts into a service locator.

---

## 6. DeliveryBill

Create a symmetric output bill.

Minimum semantic fields:

```text
item_id
amount
reason
source_system_id
day
```

Only add additional fields when existing mechanics need them:

```text
batch_id
quality
contamination
source_location
```

Use the repository's actual naming and serialization conventions.

### Validation

Reject:

- missing item id,
- unknown item id,
- non-positive amount unless the system explicitly supports signed deltas,
- NaN/infinite quantities,
- invalid source ids.

---

## 7. IOutputSink

Define/reuse:

```csharp
DeliveryResult Deliver(DeliveryBill bill);
```

The inventory-backed sink performs authoritative mutation.

Typed results should distinguish:

```text
delivered
storage_full
weight_exceeded
invalid_item
invalid_amount
sink_unavailable
```

Never convert a refusal into silent loss.

---

## 8. Fix Wildlife Trapping First

Write the failing integration test before changing the host.

### Failing case

```text
register fresh trap
→ catch quarry
→ butcher
→ carcass yield exists
→ inventory count remains unchanged
```

This should reproduce the current break.

### Correct path

```text
catch
→ processing/butchery
→ calculate meat/hide/secondary yield
→ construct DeliveryBill(s)
→ deliver to sink
→ update producer state only after transaction outcome is known
→ emit semantic result
```

### Preserve authored state

Respect:

- `isToxic`,
- toxin removal state,
- meat processed state,
- hide preservation,
- quarry hide yield,
- any existing secondary yield.

Do not invent new item content.

### UI

Replace a purely local `CATCH READY` success signal with authoritative results:

```text
Delivered: 4 meat, 1 hide
Refused: 2 meat — storage full
```

The panel reads a result/read model; it does not mutate inventory.

---

## 9. Water Authority ADR

Write the ADR before touching implementation.

Recommended model from the source:

```text
WaterTreatmentSystem litres = bulk water authority
Inventory clean_water = packaged/portable water
```

This requires explicit conversion actions.

### ADR must specify

- canonical bulk unit;
- inventory unit conversion;
- rounding policy;
- packaging/container requirement if any;
- plant capacity;
- draw/package action;
- pour/return action if supported;
- save representation;
- expedition consumption semantics;
- needs/ration consumption semantics;
- UI display.

### Prohibited architecture

Do not keep two independent numbers synchronized in the background.

If packaged water is distinct, it is a transaction:

```text
bulk litres
→ package action
→ clean_water items
```

and the reverse is explicit if supported.

---

## 10. Resolve ConsumeRation

Re-verify whether `ConsumeRation(float needed)` remains caller-free.

Choose one outcome:

### Wire

If it is the legitimate day-consumption seam:

- call it from the authoritative day/needs owner;
- route through 22A;
- emit `consumed_rations` or canonical equivalent;
- test exactly once per day.

### Delete

If it duplicates the needs system:

- remove it;
- remove stale tests/docs;
- ensure no future contributor can infer that it is a live hook.

Dead hooks are architectural misinformation.

---

## 11. Make Required Sinks Non-Nullable

A producer that fundamentally outputs inventory goods must not accept:

```text
InventoryHostSession? = null
```

as a normal running configuration.

Change composition to:

```text
mandatory sink
```

with explicit test/preview substitutes.

Construction-time failure is preferable to a runtime no-op.

---

## 12. Producer Audit Table

Create a committed producer-delivery matrix:

| Producer | Output(s) | Sink | Required? | Capacity checked? | Delivery event? | Save-idempotent? | Test |
|---|---|---|---:|---:|---:|---:|---|

Include every producer discoverable from:

- host setup,
- producer catalogs,
- inventory mutations,
- output/bill APIs.

Do not rely on content-utilization alone; that scan cannot detect “output nowhere” bugs.

---

## 13. Verify Upstream Quality Seams

Audit:

- `PharmaLabSystem.BindSkillEvaluator`,
- `WildlifeTrappingSystem.SetHunterSkill`,
- equivalent producer skill callbacks.

For each:

```text
Setup owner
→ binding
→ live survivor/roster source
→ producer calculation
```

Add a test proving a changed live skill value changes expected output/quality when designed.

---

## 14. Delivery Events

Use Plan 31 semantic vocabulary.

At minimum represent:

```text
production_delivered
resource_delta
delivery_refused
production_waste
```

Event attribution includes:

- producer/system id,
- item id,
- quantity,
- reason,
- day,
- batch id where available.

Avoid duplicate “resource delta” emissions from both producer and inventory unless ownership explicitly differentiates them.

---

## 15. Exactly-Once Save/Restore

Critical tests:

### Mid-production

```text
start batch
→ partial progress
→ save
→ load
→ resume
→ complete
→ one delivery
```

### Post-delivery

```text
complete
→ deliver
→ save
→ load
→ no repeat delivery
```

Persist enough state to distinguish:

- pending,
- completed-undelivered if such a state is intentionally possible,
- delivered,
- refused/blocked.

---

## 16. 200-Day Mass-Balance Harness

Track per item or transformation family:

```text
opening
produced
delivered
consumed
spoiled
wasted
refused
converted_in
converted_out
closing
```

Inventory reconciliation:

```text
opening
+ delivered
+ converted_in
- consumed
- spoiled
- wasted
- converted_out
= closing
```

Producer reconciliation:

```text
produced
= delivered + refused + waste + internal_use
```

Define explicit tolerances only for floating-point bulk resources.

Any unexplained delta fails.

---

## 17. 35A Test Matrix

- trapping meat delivery;
- trapping hide delivery;
- toxic catch processing;
- preservation branch;
- capacity refusal;
- weight refusal;
- invalid item;
- unbound required output port;
- missing mandatory sink at composition;
- greenhouse regression;
- pharma regression;
- foundry regression;
- brine/salvage regression;
- water packaging conversion;
- skill seam live;
- delivery semantic event;
- mid-production save idempotence;
- post-delivery save idempotence;
- 200-day mass balance.

---

## 18. 35A Definition of Done

- [ ] DeliveryBill exists;
- [ ] IOutputSink exists;
- [ ] ProducerPorts validate required effects;
- [ ] trapping yields real goods;
- [ ] water ADR approved;
- [ ] water conversion authoritative;
- [ ] dead ration hook wired or removed;
- [ ] mandatory sinks non-nullable;
- [ ] producer matrix complete;
- [ ] quality seams verified;
- [ ] events attributable;
- [ ] save/load exactly once;
- [ ] 200-day balance closes.

---

# PART II — 36A INTERLOCK

## 19. Why 36A Lands Immediately

35A establishes the contract. 36A prevents regression.

Do not proceed to 35B while producer ports can still be unbound silently.

The enforcement gate should fail when:

- a registered producer requires `deliverItem` but no sink is bound;
- a producer requires input consumption but no consume port is bound;
- a declared waste/effect path is production-critical but absent;
- a host session creates a production-capable system without binding required ports;
- a producer catalog declares output that has no valid delivery path.

Prefer deterministic, human-readable diagnostics:

```text
producer=wildlife_trapping
missing_required_port=deliver_item
```

The gate becomes part of `verify-fast` or the appropriate central CI suite.

---

# PART III — 35B: Storage, Spoilage, and the Physical Larder

## 20. Objective

Output delivery is only meaningful if storage is finite, perishable goods can decay, contamination persists, and cold-chain failures are visible.

---

## 21. Storage ADR

The source identifies existing `capacity` and `maxWeight` fields but no producer enforcement.

Choose one authority model and document it.

The ADR must answer:

- whether capacity is global, per-room, or hybrid;
- whether weight is authoritative;
- whether slot count matters;
- whether volume exists;
- how room assignment works;
- what happens to unassigned goods;
- what output refusal looks like.

Do not add multiple capacity dimensions without a demonstrated need.

---

## 22. Delivery Preflight

The sink checks capacity before mutation.

Conceptual transaction:

```text
validate bill
→ evaluate capacity
→ commit full delivery OR return typed refusal
```

If partial delivery is supported, it must be explicit:

```text
accepted_amount
refused_amount
```

and mass balance records both.

No silent truncation.

---

## 23. Room-Scale Storage

Connect real shelter spaces:

- pantry,
- cellar,
- refrigeration,
- medical cold storage where applicable.

Storage location affects only mechanics that genuinely depend on it, principally shelf life/temperature/contamination.

Use 22B and 23A rather than adding duplicate refrigerator state.

---

## 24. One Perishable Path

Items with perishable/shelf-life metadata must decay through one authority.

Persist:

- remaining life or age,
- storage condition inputs,
- contamination state where needed.

Spoilage must become a real inventory transformation when a spoiled item definition exists:

```text
fresh item
→ spoiled_* item
```

Do not simply delete inventory.

---

## 25. Cold Chain

Pipeline:

```text
PowerGridSystem
→ refrigeration power
→ thermal-mass grace
→ storage temperature state
→ spoilage multiplier
```

A blackout should not ruin food instantly.

A sufficiently long outage should produce an attributable loss.

Use the same thermal/refrigeration calculations in UI estimates and simulation.

---

## 26. Contamination as Persistent State

Dirty/irradiated goods should not “reroll” contamination each use.

Persist the state.

Where `DecontaminationSystem` supports processing:

```text
contaminated good
+ resources/power/labour
→ decontaminated state/item
+ waste/dose consequences
```

All conversions enter mass balance.

---

## 27. Authored Loss Causes

If storage losses can come from:

- vermin,
- infestation,
- environmental contamination,
- room damage,

they must come from an authored source/system and emit an event.

Do not apply invisible daily loss percentages.

---

## 28. Storage Decision Pressure

Make overproduction a real decision:

- preserve now;
- trade;
- consume;
- ration differently;
- expand storage;
- accept spoilage risk;
- discard deliberately.

The system should never punish the player with an unrepresentable overflow.

---

## 29. Storage Read Model

Existing inventory/shelter surfaces should be able to display:

```text
capacity used
weight used
room allocation
refrigeration state
top perishables
next expected spoilage
contamination warnings
recent refused deliveries
```

No panel-side shelf-life arithmetic.

---

## 30. Extended Mass Balance

Add terms:

```text
spoilage
decontamination transforms
preservation transforms
waste
refused
```

Every transform records input and output.

A preserved food conversion is not “loss.”

---

## 31. 35B Balance Sweep

Sweep:

```text
production rate
× storage capacity
× consumption rate
× refrigeration availability
× blackout duration/frequency
× shelf life
```

Acceptance:

- storage upgrades matter;
- stockpiling has limits;
- early production is still useful;
- moderate blackouts are survivable;
- losses are warned and attributable;
- overflow never silently disappears.

---

## 32. 35B Tests

- full-capacity refusal;
- max-weight refusal;
- partial delivery if supported;
- pantry shelf life;
- cellar shelf life;
- refrigeration shelf life;
- blackout grace;
- spoilage transform;
- contaminated item persistence;
- decontamination conversion;
- save/load remaining life;
- canonical next-loss estimate;
- full/near-full/ruined storage snapshots;
- mass balance including spoilage.

---

## 33. 35B Definition of Done

- [ ] one storage authority documented;
- [ ] producers respect capacity;
- [ ] room storage affects life where appropriate;
- [ ] one perishability path;
- [ ] spoiled goods represented;
- [ ] contamination persists;
- [ ] decontamination uses real machinery;
- [ ] cold chain depends on power;
- [ ] losses are authored and attributable;
- [ ] UI shows pressure;
- [ ] balance sweep passes;
- [ ] mass balance closes with storage terms.

---

# PART IV — 35C: Labour, Inputs, Time, and Player Steering

## 34. Objective

Every producer becomes a traceable production decision rather than a faucet.

---

## 35. Production Triple

For each producer author:

```text
input bill
+ labour role/hours
+ duration
→ output bill(s)
```

Also declare:

- automatic/manual,
- power need,
- required equipment/condition,
- skill/fitness influence,
- failure/degradation branches.

Anything with no labour term must explicitly declare itself automatic.

---

## 36. Duty Roster as Labour Authority

Use `DutyRosterSystem`.

Do not invent per-producer worker ownership.

Expected chain:

```text
roster assigns survivor
→ producer reads assignment
→ illness/fitness changes availability
→ batch pauses/degrades according to authored rule
```

Plan 24 owns human fitness.

---

## 37. Operator Fitness and Skill

Use shared evaluator seams.

Avoid:

```text
foundry skill penalty = custom formula
pharma skill penalty = unrelated custom formula
```

when a normalized verdict can be shared.

Quality/yield should be explainable from:

- skill,
- fitness,
- input quality,
- equipment condition.

---

## 38. Power and Environment

Plan 23A is power authority.

Unpowered producer:

```text
halts
OR
degrades
```

according to authored semantics.

Emit an attributable event.

Do not let a panel claim “paused” while Core continues production.

---

## 39. Versioned Batch Commands

Adopt the strongest repository interaction pattern:

```text
Preview
Execute
Cancel
stateVersion
expectedStateVersion
```

for producers that currently use bare actions.

Long-running producers should expose where relevant:

- start,
- pause,
- resume,
- cancel,
- reprioritize.

Stale commands fail typed validation.

---

## 40. Double-Click Defense

Critical journey:

```text
UI sends collect/start
→ state changes version
→ stale duplicate arrives
→ expectedStateVersion mismatch
→ reject
→ one delivery
```

Add an end-to-end test.

Exactly-once output must not depend on UI debouncing.

---

## 41. Quality Inheritance

Batch outcome can inherit:

```text
operator skill
operator fitness
input cleanliness
equipment condition
environmental state
```

Use Plan 21B for condition.

Quality must not be a second hidden currency.

Prefer item state/tier fields already supported.

---

## 42. Knowledge Gate

35C defines only:

```text
recipe unlocked?
```

through the existing Knowledge/Research authority.

Plan 141 owns which research unlocks which content.

Do not duplicate research design.

---

## 43. Failure Warnings

Production failures must be legible before they become punitive where possible.

Examples:

- foundry incident risk;
- contamination risk;
- batch ruin risk;
- power-loss interruption.

Route through Plan 31 semantic events/briefing.

Each failure should have a recoverable cost/path unless deliberately terminal and reviewed.

---

## 44. Producer Steering

For long-running producers expose meaningful control:

```text
pause
resume
cancel
reassign labour
reprioritize
```

Do not add automation that removes the production decision.

---

## 45. Shared Producer Read-Model Semantics

No new panel framework.

Define/reuse a common read-model vocabulary:

```text
inputs
input availability
labour assignment
operator fitness
power state
progress
ETA
expected output
quality/risk
storage availability
state version
```

Existing panels can render differently but should answer the same questions.

---

## 46. 35C Tests

- input gating;
- labour gating;
- worker becomes unfit;
- skill changes quality;
- condition changes quality;
- unpowered halt;
- environmental interruption;
- stale-version rejection;
- pause/resume;
- cancel;
- reprioritize;
- knowledge lock;
- failure warning;
- double-click no duplicate;
- active batch save/load;
- complete batch save/load;
- exactly-once DeliveryBill.

---

## 47. 35C Definition of Done

- [ ] every producer declares production triple;
- [ ] automatic producers explicit;
- [ ] labour comes from roster;
- [ ] fitness/skill seam live;
- [ ] power respected;
- [ ] condition participates where authored;
- [ ] batch commands versioned;
- [ ] stale actions rejected;
- [ ] knowledge gate mechanical only;
- [ ] failures warn;
- [ ] long-running jobs steerable;
- [ ] common read-model semantics;
- [ ] double delivery impossible.

---

# PART V — CROSS-CUTTING HARDENING

## 48. Save/Load Restore Order

Recommended conceptual order:

```text
catalogs
→ inventory/bulk resources
→ shelter/storage state
→ labour/roster
→ power/environment
→ producer runtimes
→ active batches
→ UI
```

Use the existing composition root; do not invent a second restore pipeline.

Loading must not emit historical delivery events as new deliveries.

---

## 49. Producer Ownership Table

At closure publish:

| Fact | Authority |
|---|---|
| input consumption | 22A consume authority |
| output transaction | DeliveryBill/IOutputSink |
| inventory ownership | Inventory |
| bulk water | water ADR authority |
| packaged water | Inventory |
| producer progress | producer system |
| labour assignment | DutyRosterSystem |
| fitness | Plan 24 authority |
| power | PowerGridSystem |
| equipment condition | Plan 21B authority |
| storage capacity | 35B chosen authority |
| refrigeration | 22B + 23A |
| contamination/decon | item state + DecontaminationSystem |
| semantic production event kinds | Plan 31 authority |

---

## 50. No-Double-Delivery State Machine

Prefer an explicit lifecycle:

```text
Idle
→ Reserved/Started
→ InProgress
→ CompletedPendingDelivery
→ Delivered
```

Only keep `CompletedPendingDelivery` if genuinely needed.

If the producer can atomically complete and deliver, simpler is better.

Persistence must distinguish delivered state.

---

## 51. Delivery Transaction Rules

A delivery transaction must be:

- validated;
- capacity-checked;
- atomic or explicitly partial;
- idempotent per batch/delivery ID if retries are possible;
- event-attributed.

If output delivery fails because storage is full, producer behavior must be defined:

- hold output pending,
- leave it at site,
- waste it,
- require player collection.

Do not choose a universal behavior if producers differ physically; author it per producer.

---

## 52. Performance

Avoid:

- per-frame spoilage scans over all items;
- rebuilding producer matrices each tick;
- allocation-heavy DeliveryBill conversions;
- repeated catalog lookups by linear scan.

Prefer:

- daily/hourly cadence consistent with existing systems;
- indexed item definitions;
- stable producer registration;
- batched spoilage advancement.

Measure 200-day soak runtime before/after.

---

## 53. Semantic Event Discipline

Suggested semantic concepts:

```text
production_started
production_paused
production_resumed
production_cancelled
production_delivered
delivery_refused
resource_delta
production_waste
storage_near_capacity
spoilage
contamination
production_halted
batch_degraded
```

Do not introduce raw string event kinds if Plan 31 already canonicalizes vocabulary.

---

## 54. UI Decision Questions

Every producer panel should answer:

```text
What are we making?
What inputs are missing?
Who is working?
Are they fit?
Is there power?
How long remains?
What output is expected?
What could go wrong?
Is there room to receive it?
What happened to the previous batch?
```

Every storage surface should answer:

```text
How full are we?
What spoils next?
Is refrigeration live?
What is contaminated?
What delivery was refused?
```

---

## 55. Failure Modes

### Producer internal yield exists but inventory unchanged
Fix: mandatory output sink.

### Sink null in production
Fix: non-null composition contract.

### Capacity clips silently
Fix: typed refusal/partial result.

### Water counts drift
Fix: ADR + explicit conversion.

### Save/load repeats output
Fix: persisted state/version/idempotence.

### Skill evaluator exists but no caller
Fix: setup binding + integration test.

### Producer runs without labour
Fix: duty roster.

### Producer runs without power
Fix: Plan 23 authority.

### Spoilage deletes item invisibly
Fix: explicit spoiled good/waste event.

### UI ETA disagrees with sim
Fix: Core read model.

### Double-click duplicates output
Fix: optimistic concurrency plus idempotent completion.

---

## 56. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| item counts shift economy | High | Medium | baseline + balance sweep |
| water ADR save migration | Medium | High | migration matrix + round-trip |
| non-null sinks break previews | Medium | Low | explicit fake sink |
| storage scope explosion | Medium | Medium | ADR before code |
| spoilage feels unfair | Medium | Medium | warning + canonical estimates |
| transforms break mass balance | Medium | Medium | conversion ledger |
| unbound quality seams | Medium | Medium | producer audit |
| duplicate delivery | Medium | Critical | stateVersion + idempotence |
| Plan 136 lands before rails | Medium | Medium | dependency gate |
| Plan 141 scope bleed | Low | Low | mechanical-only unlock gate |
| soak becomes expensive | Low–Med | Medium | batched cadence + perf measurement |

---

## 57. Commit Strategy

```text
C2[14].1  baseline + producer census
C2[14].2  DeliveryBill + DeliveryResult
C2[14].3  IOutputSink + ProducerPorts validation
C2[14].4  wildlife trapping delivery
C2[14].5  water ADR + conversion
C2[14].6  required sink hardening + dead hook resolution
C2[14].7  producer audit + skill seams + events
C2[14].8  save idempotence + 200-day mass balance
           └─ 35A GATE
C2[14].9  Plan 36A port enforcement
           └─ 36A GATE
C2[14].10 storage ADR + capacity transaction
C2[14].11 room storage + perishables
C2[14].12 contamination + cold chain
C2[14].13 storage UI/read model + balance
           └─ 35B GATE
C2[14].14 production triples + roster
C2[14].15 fitness/power/condition quality
C2[14].16 stateVersion batch semantics
C2[14].17 knowledge/failure/steering
C2[14].18 shared read model + duplicate-delivery defense
           └─ 35C GATE
C2[14].19 integrated closure
```

Keep commits independently testable where practical.

---

## 58. Verification Checklist

Run after each major gate and at final closure:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --survivors-selftest
godot --headless --path . -- --content-utilization-selftest
bash scripts/ci/verify-fast.sh
```

Also run repository-supported:

```text
--expansions-selftest
ashfall-balance-sim
200-day mass-balance soak
```

Final acceptance requires unexplained accounting delta = 0 within declared numeric tolerance.

---

## 59. Flagship Definition of Done

### 35A
- [ ] delivery contract exists;
- [ ] producer ports validated;
- [ ] trapping output reaches inventory;
- [ ] water authority/conversion documented;
- [ ] dead ration hook resolved;
- [ ] production-critical sinks mandatory;
- [ ] producer audit complete;
- [ ] quality seams live;
- [ ] delivery/refusal events attributable;
- [ ] no double delivery on load;
- [ ] 200-day producer balance closes.

### 36A
- [ ] every registered producer has required ports;
- [ ] missing port fails CI/selftest;
- [ ] diagnostics name producer and missing effect.

### 35B
- [ ] storage model ADR;
- [ ] capacity enforced before delivery;
- [ ] room storage meaningful;
- [ ] perishables use one path;
- [ ] spoiled goods/waste represented;
- [ ] contamination persists;
- [ ] cold chain depends on power;
- [ ] storage losses attributable;
- [ ] capacity pressure visible;
- [ ] mass balance includes spoilage/waste/refusal.

### 35C
- [ ] every producer declares inputs;
- [ ] labour;
- [ ] duration;
- [ ] outputs;
- [ ] automatic status explicit;
- [ ] duty roster is labour authority;
- [ ] fitness/skill real;
- [ ] power real;
- [ ] equipment condition integrated where authored;
- [ ] state-version commands;
- [ ] stale commands rejected;
- [ ] research gate mechanical only;
- [ ] failure warnings;
- [ ] long-running jobs steerable;
- [ ] common producer read-model semantics;
- [ ] double delivery impossible.

---

## 60. Closure Report Template

```markdown
# C2[14] Closure Report

## Repository
- Start commit:
- End commit:
- Branch:
- Working tree:

## Baseline
- Producers discovered:
- Producers with real output sinks:
- Missing output sinks:
- Nullable critical sinks:
- Water authorities:
- Unbound quality seams:
- 200-day unexplained delta:

## 35A
- DeliveryBill:
- IOutputSink:
- ProducerPorts:
- trapping:
- water ADR:
- ConsumeRation:
- mandatory dependencies:
- producer audit:
- skill seams:
- semantic events:
- save idempotence:
- mass balance:
- result:

## 36A
- gate:
- missing-port diagnostics:
- CI:
- result:

## 35B
- storage ADR:
- capacity:
- rooms:
- perishables:
- contamination:
- refrigeration:
- losses:
- UI:
- balance:
- mass balance:
- result:

## 35C
- production triples:
- roster:
- fitness/skill:
- power:
- condition:
- stateVersion:
- steering:
- knowledge:
- warnings:
- read model:
- duplicate-delivery test:
- result:

## Verification
- Core build:
- Core tests:
- Host build:
- data integrity:
- bridge:
- survivors:
- content utilization:
- expansions:
- verify-fast:
- balance:
- mass-balance soak:

## Final Metrics
- producer count:
- missing required ports:
- delivery refusals:
- spoiled:
- wasted:
- duplicate deliveries:
- unexplained item delta:
```

---

## 61. Final Execution Directive

Implement Plan 35 as a transaction-and-accounting repair, not a content expansion.

The critical sequence is:

```text
reuse combat port discipline
→ define output bill/sink
→ make required ports mandatory
→ fix trapping
→ resolve water authority
→ audit all producers
→ prove exactly-once save/load
→ enforce with 36A
→ make storage finite
→ make spoilage/contamination physical
→ make batches depend on inputs, labour, time, power, and condition
→ close the 200-day mass balance
```

Do not declare success because a producer is “consumed” by content tooling or because its panel displays completion.

The defining rule is:

> **Every producer must either deliver through a validated output port or explain exactly why it could not.**

The systemic accounting rule is:

> **Goods may be consumed, spoiled, wasted, transformed, or refused — but they must never simply disappear.**
