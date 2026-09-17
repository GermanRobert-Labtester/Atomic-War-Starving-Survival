# C1 — Flagship Integration Plan [10]: Goods Must Arrive — Production Delivery, Storage, Spoilage & Labour-Provisioning Integrity

> **Output:** `C1_planintegration[10].md`
>
> **Source baseline:** Plan 35 — Goods Must Arrive: The Production-to-Provisioning Chain
>
> **Wave:** Continuity Wave 5 — *The Human Interface*
>
> **Depends on:** Plan 22A single consume authority; Plan 24 fitness/labour; Plan 23 power/heat; Plan 21 equipment condition; Plan 31 semantic events; Plan 27 authority-backed test fixtures; Plan 36 producer-port gate.
>
> **Mandatory execution order:** 35A → 36A → 35B → 35C.
>
> **Coordination:** land 35A before parallel hunting/cooking expansion work so new content uses the delivery rails rather than inheriting today's trapping hole. Keep research unlock work separate: 35C defines the mechanical gate; content/tech-tree design belongs to the dedicated research plan.
>
> **Primary rule:** every producer must either deliver a real, persisted effect into an authoritative sink or return a typed, player-visible refusal. Silent production loss is forbidden.
>
> **Guardrails:** no new resource currency, no new producer subsystem, no rival bill framework, no panel framework rewrite, no hidden storage arithmetic, no producer-specific ad hoc delivery delegates when one shared sink contract can serve.

---

# 0. Mission

ASHFALL already has many producer systems:

- greenhouse;
- apiculture;
- trapping;
- water treatment;
- brine/desalination;
- pharma;
- foundry;
- excavation;
- salvage;
- kitchen;
- combat loot;
- potentially laundry/hygiene and other shelter production.

The core failure is not lack of production logic.

It is lack of one contract for the end of production.

Some chains work:

```text
greenhouse harvest
→ InventoryHost.Add(...)
→ item exists
```

and:

```text
combat outcome
→ explicit host ports
→ grantLoot / health / morale / ammo / progression
→ ValidatePorts()
```

Others stop at an intermediate state:

```text
trap catches quarry
→ hasCatch / catchSpecies / carcassYield
→ panel says "CATCH READY"
→ nothing enters inventory
```

Water is split between:
- treatment-plant litres;
- `clean_water` inventory items;

with an optional bridge and a dead ration hook.

Storage capacity exists in inventory state but producers do not negotiate with it.

Perishable/contaminated outputs have multiple local concepts but no universal delivery semantics.

Labour, time, power, operator skill, condition, and queue semantics differ producer by producer.

Plan 35 creates one complete production lifecycle:

```text
INPUT AUTHORITY
    │
    ▼
InventoryBill / required inputs
    │
    ▼
LABOUR + FITNESS + SKILL
    │
    ▼
POWER / HEAT / EQUIPMENT
    │
    ▼
PRODUCTION JOB
    │
    ▼
DeliveryBill
    │
    ▼
IOutputSink.Deliver(...)
    │
    ├── delivered
    ├── refused: storage_full
    ├── refused: weight_exceeded
    ├── spoiled / contaminated / waste
    └── explicit failure
    │
    ▼
AUTHORITATIVE INVENTORY / BUFFER / WORLD SINK
    │
    ▼
semantic resource / production event
    │
    ▼
briefing + panel + save + mass-balance test
```

The production equation becomes explicit:

```text
inputs
+ labour
+ time
+ power / environment
+ operator quality
+ equipment condition
=
outputs
+ waste
+ spoilage
+ refused delivery
```

Nothing disappears.

---

# 1. Source-Evidence Interpretation

## 1.1 The repository already contains the right two patterns

The source plan identifies:
- `InventoryBill` as the project's existing input vocabulary;
- combat host ports + `ValidatePorts()` as the project's strongest "required effect must be bound" pattern.

Therefore this plan should generalize those patterns rather than invent a parallel production architecture.

## 1.2 Trapping is the cleanest broken end-to-end chain

Trapping already computes:
- species;
- carcass yield;
- toxicity;
- processing state;
- hide state.

But the host reportedly has no inventory delivery dependency.

Therefore trapping is the first producer to convert and the best regression proof.

## 1.3 Water currently has two truths

Plant litres and inventory `clean_water` coexist.

The solution must be an authority decision, not another optional bridge.

## 1.4 Storage is currently not part of production

If inventory capacity/weight can reject goods but producer completion never asks, overproduction is unphysical.

35A adds typed delivery refusal; 35B makes capacity/spoilage meaningful.

## 1.5 Production inputs upstream can also be dead

Skill seams such as:
- pharma skill evaluator;
- trapping hunter skill;
must be verified while producer chains are audited.

A producer can have a perfect output sink but still be disconnected from its operator.

---

# 2. Non-Negotiable Production Invariants

## INV-35.1 — One delivery contract

Every inventory-producing system delivers through one common output abstraction.

## INV-35.2 — Required sinks are validated

A producer that requires an output sink cannot start production if the sink is unbound.

## INV-35.3 — Delivery is typed and attributable

Every delivered output carries:
- item;
- amount;
- reason;
- source producer;
- day;
and optional batch/job identity.

## INV-35.4 — Refusal is explicit

Capacity/weight/storage refusal returns a typed result.

No silent truncation.

## INV-35.5 — Input and output use symmetric vocabulary

Inputs use `InventoryBill`/equivalent.

Outputs use `DeliveryBill`.

Both are explicit and testable.

## INV-35.6 — Water has one documented authority

Plant litres and inventory items cannot both independently represent the same water quantity.

## INV-35.7 — No double delivery

Save/load, double click, stale panel, or repeated callback cannot duplicate a completed batch.

## INV-35.8 — Mass balance closes

Over a deterministic long run:

```text
produced
≈ delivered
+ consumed
+ spoiled
+ waste
+ refused/retained
```

within documented rounding.

## INV-35.9 — Storage affects producer decisions

If goods cannot fit:
- job completion must resolve explicitly;
- UI must explain;
- player must have a remedy.

## INV-35.10 — Spoilage transforms state

Perishable goods become:
- spoiled goods;
- waste;
- other authored outputs.

They do not vanish silently.

## INV-35.11 — Contamination persists as state

Dirty/irradiated goods retain contamination until:
- decontaminated;
- consumed;
- discarded;
- transformed.

## INV-35.12 — Labour comes from roster authority

Long-running producers use duty assignments, not hidden "worker" variables.

## INV-35.13 — Producer quality uses shared worker/equipment inputs

Skill, fitness, cleanliness, and equipment condition contribute through shared seams.

## INV-35.14 — Production jobs are steerable

Long-running work supports:
- start;
- progress;
- cancel;
- pause/reassign/prioritize where architecture allows.

## INV-35.15 — Optimistic concurrency prevents duplicate output

Stale UI state cannot execute the same completion twice.

---

# 3. Definition of Done

Plan 35 is complete only when:

- `IOutputSink` exists;
- `DeliveryBill` exists;
- producer port validation exists;
- required unbound producer effects fail construction/selftest;
- trapping outputs meat/hide/etc. into real inventory;
- toxicity/processing state affects delivered item/state correctly;
- water authority ADR exists;
- water host dependency is non-null where required;
- dead `ConsumeRation` hook is wired or deleted;
- producer audit table covers all live producers;
- skill/operator seams are verified;
- production delivery emits semantic events;
- mid-production save cannot double-deliver;
- 200-day mass balance closes;
- storage authority is documented;
- producer delivery respects capacity and weight;
- cellar/fridge/pantry affect shelf life;
- perishables transform into spoiled/waste goods through one path;
- contamination is persisted and decontaminable;
- cold-chain blackout causes attributable loss;
- storage loss causes are authored/visible;
- inventory/shelter UI exposes fill and upcoming perishables;
- balance sim proves no unrepresentable overflow;
- every producer declares inputs + labour + duration + outputs;
- duty vacancy stops relevant production;
- fitness and skill alter quality/yield through shared seams;
- power/heat can halt production;
- queue/action semantics use state versioning;
- stale-version double-click cannot duplicate delivery;
- recipe knowledge gates are mechanical and delegated to existing research authority;
- producer panels read a consistent production read model;
- content utilization sees real `EFFECT_PRODUCED` from producer catalogs;
- Plan 36 gate makes unbound producer chains impossible to regress.

---

# 4. Phase P0 — Production Chain Inventory

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty-file count
producer systems
producer host sessions
input bill APIs
output mutation sites
inventory Add/Remove call sites
nullable inventory dependencies
skill-evaluator bindings
power bindings
duty/worker bindings
save sections
production events
capacity/weight checks
perishable paths
contamination paths
```

## P0.2 Build producer chain matrix

Create:

`docs/systems/PRODUCTION_CHAIN_MATRIX.md`

Columns:

```text
producer_id
system
host
input bill
labour role
power/heat
skill input
condition input
duration/job model
output item(s)
output sink
capacity checked
spoilage path
event emitted
save section
test
status
```

Statuses:
- COMPLETE
- OUTPUT_UNBOUND
- INPUT_UNBOUND
- SKILL_UNBOUND
- POWER_UNBOUND
- STORAGE_UNAWARE
- SAVE_RISK
- DUPLICATE_AUTHORITY
- NEEDS_DECISION

## P0.3 Enumerate live producers

At minimum:
- greenhouse;
- apiculture;
- wildlife trapping;
- water treatment;
- brine/desal;
- pharma;
- foundry;
- excavation;
- deep-coast salvage;
- kitchen;
- combat loot.

Also inspect:
- laundry/hygiene;
- fermentation;
- workshop;
- munitions;
- waystation production;
if present.

## P0.4 Reproduce broken trapping chain

Test before fix:

```text
trap catches
→ butchery completes
→ carcassYield > 0
→ inventory count unchanged
```

Preserve as regression.

## P0.5 Reproduce unbound-sink no-op

Construct a producer with missing sink.

Expected post-fix:
- construction/validation failure with named missing effect.

---

# TASK 35A — Shared Delivery Contract and Producer-Port Validation

# 35A.0 Goal

Create one answer to:

> "This producer made something. Where does it go?"

## 35A.1 Define `DeliveryBill`

Create `Assets/Ashfall.Core/Production/DeliveryBill.cs`.

Recommended:

```csharp
public sealed record DeliveryBill(
    string ItemId,
    float Amount,
    string Reason,
    string SourceSystemId,
    int Day,
    string? BatchId = null,
    string? SubjectId = null);
```

Use integral quantities where the inventory model requires them.

## 35A.2 Define typed delivery result

Result codes:

```text
Delivered
Partial
RefusedStorageFull
RefusedWeightExceeded
RefusedCategory
UnknownItem
InvalidAmount
SinkUnavailable
DuplicateBatch
```

Return requested, accepted, remaining, and reason code.

## 35A.3 Define `IOutputSink`

```csharp
public interface IOutputSink
{
    DeliveryResult Deliver(DeliveryBill bill);
}
```

Keep the base interface narrow.

## 35A.4 Inventory output sink adapter

The authoritative inventory adapter must:
- validate item IDs;
- enforce capacity/weight;
- preserve supported item metadata;
- mutate inventory exactly once;
- return typed result.

## 35A.5 Item-state metadata

Support only metadata current producers need:
- contamination;
- quality/condition if existing;
- provenance/source;
- toxicity/processed state where applicable.

Avoid a generic property bag if typed item-state contracts already exist.

## 35A.6 Multi-output batch

Create a `DeliveryBatch`/equivalent with:
- stable batch ID;
- lines;
- source;
- day.

Choose and document:
- atomic delivery;
- or explicit partial delivery.

## 35A.7 Producer ports

Generalize the combat pattern:

```text
ProducerPorts
  deliverItem
  consumeInput
  applyNeed
  recordWaste
  getSkill
  getPower
```

Only required ports belong to each producer.

## 35A.8 Required effect declaration

Each producer declares which ports are REQUIRED versus optional.

No implicit assumptions.

## 35A.9 `UnboundRequiredEffects`

Expose a deterministic list of missing required effects.

## 35A.10 `ValidatePorts()`

Return/log:
- producer ID;
- missing effect;
- expected binding.

Use combat's validation discipline.

## 35A.11 Construction enforcement

Required output sinks become non-null constructor dependencies.

Do not accept `null` and silently continue.

## 35A.12 Fix trapping first

At butchery completion:
- compute meat;
- hide;
- tallow/secondary output if authored;
- toxicity state;
- processing state;
- preservation state.

Create delivery batch.

## 35A.13 Toxic catch handling

Never transform toxic catch into safe meat without the real toxin-removal step.

## 35A.14 Hide handling

Honor:
- hide yield;
- preservation;
- leather/textile conversion path.

## 35A.15 Trapping panel

The panel reports:
- catch state;
- butchery state;
- delivered goods;
- refused goods;
- waste.

`CATCH READY` is no longer terminal success.

## 35A.16 Delivery events

Successful output emits Plan-31 semantic events:
- `production_delivered`;
- `resource_delta`;
or canonical equivalents.

## 35A.17 Water authority ADR

Create `docs/architecture/ADR_WATER_AUTHORITY.md`.

Recommended authority:
- treatment plant owns litres as a process buffer;
- inventory owns packaged portable `clean_water`;
- explicit draw/bottle conversion;
- optional pour-back only if supported.

## 35A.18 Water conversion constant

One authored/configured `litres_per_clean_water_item`.

No repeated magic conversion.

## 35A.19 Draw transaction

```text
validate litres
→ preview inventory acceptance
→ decrement litres
→ deliver clean_water
```

If delivery fails, litres remain/rollback.

## 35A.20 Pour transaction

If supported:

```text
consume clean_water
→ validate plant capacity
→ add litres
```

No duplication.

## 35A.21 Dual-authority prohibition

The same physical water may exist in one form at a time.

## 35A.22 Water host dependency

Make required inventory/output dependency non-null.

## 35A.23 `ConsumeRation` decision

Audit the zero-caller hook.

If intended:
- wire to day owner / Plan-22 consume authority.

If not:
- delete and document.

## 35A.24 Greenhouse migration

Move direct inventory add through common output sink while preserving:
- yield;
- pollination bonus.

## 35A.25 Apiculture migration

Honey/wax use same delivery path.

## 35A.26 Pharma audit

Verify:
- input bill;
- output delivery;
- skill evaluator caller;
- storage refusal.

Adapt to sink without unnecessary formula rewrite.

## 35A.27 Foundry audit

Retain explicit port architecture and integrate common delivery semantics.

## 35A.28 Brine/desal audit

Inputs use bills; outputs use sink.

## 35A.29 Deep-coast salvage audit

Preserve:
- contamination;
- degraded item metadata;
- loot provenance.

## 35A.30 Excavation audit

Map every material output and waste path.

## 35A.31 Kitchen boundary

Prepared portions may remain the sanctioned kitchen pantry state from Plan 22.

Document why that is not a duplicate raw-stock authority.

## 35A.32 Combat compatibility

Only item/loot output uses the shared sink.

Combat-specific health/morale/trauma ports remain specialized.

## 35A.33 Skill seam validation

Audit:
- `PharmaLabSystem.BindSkillEvaluator`;
- `WildlifeTrappingSystem.SetHunterSkill`;
- all producer skill inputs.

Assert campaign authority supplies the value.

## 35A.34 Power seam inventory

Record all power-dependent producers for 35C.

## 35A.35 Delivery refusal events

Storage refusal emits an attributable semantic event.

## 35A.36 Event idempotence

A completed batch cannot emit the same delivery event twice.

## 35A.37 Stable batch identity

Persist:
- batch ID;
- progress;
- completion;
- delivered flag.

## 35A.38 Mid-production save

Save at partial progress, load, finish, deliver exactly once.

## 35A.39 Completed-before-refresh save edge

If inventory delivery happened but UI refresh did not:
- save/load must not redeliver.

## 35A.40 Test mass-balance ledger

Track:
- produced;
- delivered;
- refused;
- transformed;
- consumed;
- spoiled;
- waste.

Developer/test instrumentation only.

## 35A.41 200-day soak

Run deterministic authority-backed simulation.

Assert balance per tracked resource within documented rounding.

## 35A.42 Content-utilization proof

A completed producer recipe/catalog entry reaches `EFFECT_PRODUCED`.

## 35A.43 Per-producer delivery tests

For each live producer:
- valid delivery;
- capacity refusal;
- missing sink;
- save/load;
- event.

## 35A.44 Plan 36 interlock

Immediately land 36A's producer-port gate after this contract.

### 35A DoD

Every live producer either delivers to an authoritative sink or returns a typed failure. No output vanishes.

---

# TASK 35B — Storage, Spoilage, Contamination & Physical Limits

# 35B.0 Goal

Make storage the physical destination of production, not an infinite list.

## 35B.1 Storage ADR

Create `docs/architecture/ADR_STORAGE_AUTHORITY.md`.

Choose one current authority:
- slots;
- weight;
- capacity;
- room modifiers.

Do not invent volume mid-plan unless data already supports it.

## 35B.2 Layered model

Prefer existing inventory capacity/maxWeight plus shelter room modifiers.

## 35B.3 Shared capacity query

Expose one function:

```text
CanAccept(itemId, amount, itemState)
```

Used by:
- output sink;
- producer preview;
- inventory UI.

## 35B.4 Completion/reservation policy

Document whether storage is:
- reserved at job start;
- checked at completion.

Avoid indefinite phantom reservations.

## 35B.5 Refused output fate

Per producer choose explicitly:
- remain pending;
- stay at producer;
- become waste;
- partial delivery.

No silent loss.

## 35B.6 Room-scale storage

Map:
- pantry;
- cellar;
- refrigerator;
- other real storage rooms.

## 35B.7 Kitchen/shelter integration

Use actual `hasCellar` / refrigeration state from Plans 22/23.

## 35B.8 Perishable contract

Use authored:
- perishable;
- shelf life;
- spoil output;
- storage modifiers.

## 35B.9 One spoilage authority

Do not run independent kitchen and inventory decay clocks for the same goods.

## 35B.10 Persistent spoilage state

Choose deterministic:
- expiry day;
- remaining life;
- produced day + modifiers.

## 35B.11 Spoiled conversion

Expired items become:
- `spoiled_*`;
- waste;
- other authored result.

They do not disappear.

## 35B.12 Spoiled goods remain real

They may be:
- disposed;
- risky;
- processed;
per authored data.

## 35B.13 Cold chain

Refrigeration depends on Plan-23 power.

## 35B.14 Grace window

Short outages use authored thermal grace.

## 35B.15 Cold-chain warnings/events

Emit:
- refrigeration lost;
- spoilage risk;
- goods spoiled.

Include causal attribution.

## 35B.16 Persistent contamination

Contamination is an item-state fact, not rerolled.

## 35B.17 Decontamination

Use existing `DecontaminationSystem`.

## 35B.18 Decon mass balance

Dirty input → clean output + waste/dose bookkeeping.

## 35B.19 Authored vermin/loss

Only real systems may cause storage losses.

No hidden percentage.

## 35B.20 Loss attribution

Show:
- item;
- amount;
- storage;
- cause.

## 35B.21 Capacity-pressure decisions

Player remedies:
- stop/pause production;
- consume;
- preserve;
- trade;
- expand storage.

## 35B.22 Inventory fill read model

Core supplies current/max/fill percentage.

## 35B.23 Perishable read model

Core supplies:
- next expiries;
- remaining life;
- expected loss;
- storage modifier.

## 35B.24 Storage snapshots

Capture:
- healthy;
- near-full;
- full;
- cold-chain risk;
- ruined.

## 35B.25 Event routing

Refusal/loss lines route to:
- storage;
- power;
- producer;
as appropriate.

## 35B.26 Save round-trip

Persist:
- expiry;
- contamination;
- storage assignment.

## 35B.27 Deterministic spoilage order

Tie-break stably.

## 35B.28 Extended mass balance

Include spoilage/waste and water conversion.

## 35B.29 Balance simulation

Scenarios:
- high greenhouse output;
- trapping surge;
- small larder;
- expanded larder;
- blackout.

Ensure warning precedes unrepresentable loss.

## 35B.30 Capacity tests

Test:
- exact fit;
- one-over;
- weight limit;
- category limit;
- partial policy.

## 35B.31 Spoilage tests

Test:
- normal;
- cellar;
- fridge;
- blackout grace;
- expiry.

## 35B.32 Contamination tests

Test:
- persistence;
- decon;
- save/load.

### 35B DoD

Storage capacity, shelf life, contamination and cold chain are real, visible constraints.

---

# TASK 35C — Labour, Inputs, Power, Time, Quality & Steering

# 35C.0 Goal

Make every non-automatic producer a managed job requiring inputs, labour, time, and environmental capability.

## 35C.1 Production triple/data contract

Each producer declares:

```text
input bill
labour role/hours
duration
power/heat
equipment
output
failure/degradation
automatic?
```

## 35C.2 Automatic producer declaration

Anything without labour must explicitly be marked automatic/passive.

## 35C.3 Duty roster authority

Long-running producers pull assigned workers from Plan 24 roster.

## 35C.4 Vacancy propagation

Illness, quarantine, death, reassignment:
- pauses or degrades production;
- no ghost labour.

## 35C.5 Fitness verdict

Use Plan 24A:
- fit;
- impaired;
- unfit;
- incapacitated.

## 35C.6 Shared effective-worker quality

Expose:
- skill;
- fitness;
- fatigue;
- relevant modifiers.

Producer formulas consume it.

## 35C.7 Power draw

Every powered producer declares real watts and queries Plan 23.

## 35C.8 Heat/environment

Use shelter thermal/environment authority.

## 35C.9 Attributable halt

Power/heat loss emits `production_blocked`/equivalent.

## 35C.10 Input consumption timing

Document:
- reserve/start;
- progressive;
- completion.

## 35C.11 Cancellation semantics

Return unconsumed inputs; convert irreversible process loss to waste.

## 35C.12 Optimistic concurrency

Use `stateVersion` / `expectedStateVersion`.

Actions:
- Preview;
- Start;
- Pause;
- Resume;
- Cancel;
- Prioritize;
- Complete/Collect.

## 35C.13 Double-click protection

Same stale version cannot mutate twice.

## 35C.14 Stable job identity

Persist:
- ID;
- recipe;
- owner;
- start;
- progress;
- version;
- delivered flag.

## 35C.15 Queue semantics

Use common:
- FIFO;
- priority;
- pause;
- cancel.

## 35C.16 Queue read model

Expose:
- active;
- queued;
- ETA;
- worker;
- power;
- inputs;
- storage;
- risk.

## 35C.17 Quality inheritance

Use:
- operator skill;
- fitness;
- input cleanliness;
- equipment condition;
- environment.

## 35C.18 Quality result

Represent through existing:
- quantity;
- item state;
- degraded item;
- failure/waste.

No new quality currency.

## 35C.19 Contamination propagation

Dirty input can affect output through existing contamination state.

## 35C.20 Equipment condition

Use Plan 21 for machine/tool condition.

## 35C.21 Production wear

Jobs apply wear through Plan 21 authority.

## 35C.22 Knowledge gate

Recipe start checks existing research/knowledge authority.

## 35C.23 Remove hardcoded medical recipe ID list

Replace with tags/types/recipe metadata.

## 35C.24 Research-plan boundary

This plan provides mechanical unlock enforcement only.

## 35C.25 Failure modes

Use actual authored failures:
- foundry incident;
- pharma ruin;
- contamination spread;
- equipment overheat.

## 35C.26 Pre-warning

Predictable elevated risk is surfaced before failure.

## 35C.27 Recoverable cost

Failures produce bounded:
- waste;
- time loss;
- degraded output;
- condition damage;
- contamination.

## 35C.28 Steering

Long jobs allow reasonable:
- pause;
- reprioritize;
- reassignment.

## 35C.29 Worker reassignment

New worker influences subsequent progress without duplicating labour.

## 35C.30 Shared production read model

Provide:

```text
producer_id
job_id
status
inputs
inputs_ready
worker
fitness
skill
power_required
power_available
environment
progress
eta
output_preview
capacity_status
failure_risk
actions
state_version
```

## 35C.31 Panel consistency

Every producer screen answers:
- what is needed;
- who works;
- how long;
- what comes out;
- whether it fits;
- why blocked;
- risk;
- actions.

## 35C.32 No UI-side formulas

ETA/capacity/risk/quality come from Core/read model.

## 35C.33 Production semantic events

Use canonical event kinds:
- started;
- paused;
- blocked;
- delivered;
- failed.

## 35C.34 No progress heartbeat spam

Only meaningful thresholds/transitions emit player events.

## 35C.35 Queue save/load

Persist order, progress, worker reference, version, delivered status.

## 35C.36 Worker-death save edge

Job remains unstaffed after load.

## 35C.37 Power-outage save edge

Job remains halted/risk state preserved.

## 35C.38 Quality determinism

Same authoritative inputs => same result, except explicitly seeded existing RNG.

## 35C.39 Labour-gating tests

No eligible worker => no progress.

## 35C.40 Unpowered tests

No watts => no progress/output.

## 35C.41 Stale-version tests

No inventory/labour/event side effect on rejected stale action.

## 35C.42 Queue steering tests

Pause/resume/cancel/reassign/prioritize.

## 35C.43 No-double-delivery integration

Two identical completion requests => one delivery.

## 35C.44 Expansion selftest

Run current expansion integrity path.

### 35C DoD

Every batch is traceable to inputs, worker, time, power/environment and final output, and cannot silently disappear or duplicate.

---

# 5. Cross-Task Dependency Graph

```text
22A consume authority
      │
      ▼
35A delivery authority
      │
      ▼
36A producer-port enforcement
      │
      ▼
35B storage / spoilage / contamination
      │
      ▼
35C labour / power / queues / quality
```

Support:

```text
24A fitness ─────────────► worker eligibility/quality
23A power/thermal ───────► cold chain and powered production
21B condition ───────────► machine quality/wear
31A semantic events ─────► delivery/block/failure visibility
27A fixture fidelity ────► trustworthy soak/matrix
Plan 136 content ────────► runs on 35A rails
Plan 141 research ───────► consumes 35C gate
```

---

# 6. Production Contract

Every producer must answer:

```text
input?
worker?
duration?
power/heat?
skill?
equipment?
output?
sink?
capacity?
event?
save?
```

Any unanswered field is a flagged gap.

---

# 7. Delivery Result Contract

Canonical outcomes:

```text
SUCCESS
PARTIAL
REFUSED_STORAGE_FULL
REFUSED_WEIGHT
REFUSED_CATEGORY
UNKNOWN_ITEM
INVALID_AMOUNT
DUPLICATE_BATCH
SINK_UNAVAILABLE
```

Carry requested, accepted, remaining and source/batch identity.

---

# 8. Transaction Semantics

1. verify batch not delivered;
2. verify ports bound;
3. compute outputs;
4. preview capacity;
5. apply atomic/partial policy;
6. deliver;
7. record refusal/waste;
8. mark batch closed;
9. emit event;
10. persist;
11. refresh UI.

No mutation twice on retry.

---

# 9. Water Authority Contract

Recommended lifecycle:

```text
source water
→ treatment
→ clean litre buffer
→ package/draw
→ clean_water item
→ Plan-22 consumption
```

One conversion factor.

---

# 10. Storage Contract

The same `CanAccept` result powers:
- producer preview;
- producer completion;
- inventory UI.

No arithmetic drift.

---

# 11. Spoilage Contract

```text
produced
→ delivered
→ stored
→ shelf-life countdown
→ warning
→ spoiled conversion
→ disposal/secondary processing
```

---

# 12. Contamination Contract

```text
contaminated goods
→ inventory state
→ decontamination OR consumption/disposal
```

No reroll.

---

# 13. Labour Contract

Roster owns worker assignment.

Producer queries it.

No duplicate worker registry.

---

# 14. Power Contract

Producer data declares draw and environmental need.

Grid/thermal authority decides availability.

---

# 15. Quality Contract

Quality is attributable to:
- skill;
- fitness;
- inputs;
- condition;
- environment.

No hidden producer-only duplicate threshold tables.

---

# 16. Mass-Balance Contract

For a resource:

```text
opening + produced + imported
=
closing + consumed + exported + spoiled + waste + destroyed
```

Pending/refused output must remain represented.

---

# 17. 200-Day Soak

Use:
- authority-backed fixture;
- deterministic seed;
- multiple producers;
- outages;
- storage pressure;
- spoilage;
- illness;
- cancellation.

Fail on unexplained resource delta or duplicate batch.

---

# 18. Failure Injection Matrix

## N35.1 Trapping sink missing
ValidatePorts fails.

## N35.2 Full inventory on butchery
Typed refusal, no vanished meat.

## N35.3 Water draw cannot fit
No litre loss.

## N35.4 Load after delivered batch
No duplicate output.

## N35.5 Missing skill binding
Readiness/port gate fails.

## N35.6 Cold-chain outage
Grace, warning, then attributable spoilage.

## N35.7 Decontamination
Clean goods + waste; balance closes.

## N35.8 Worker admitted
Job pauses.

## N35.9 Power lost
Job halts.

## N35.10 Double click
One mutation/event.

## N35.11 UI says capacity available but sink rejects
Shared read model prevents mismatch.

---

# 19. Persistence Matrix

| State | Authority | Persist |
|---|---|---:|
| definitions | catalogs | no |
| active batch | producer | yes |
| progress | producer | yes |
| worker assignment | duty + job ref | yes |
| batch version | producer | yes |
| delivered flag | producer | yes |
| goods | inventory | yes |
| contamination | item state | yes |
| spoilage age/expiry | storage/inventory | yes |
| storage room state | shelter | yes |
| power | grid | yes |
| condition | equipment | yes |
| derived ETA/risk | read model | no |

---

# 20. UI Acceptance

Producer panels show:
- inputs;
- worker;
- fitness/skill;
- power/heat;
- progress/ETA;
- output;
- storage;
- quality/risk;
- block reason;
- actions.

Storage panels show:
- fill;
- top perishables;
- upcoming loss;
- refrigeration state.

---

# 21. Semantic Event Acceptance

Use Plan-31 canonical forms for:
- resource delta;
- delivered production;
- blocked production;
- failure;
- spoilage;
- cold-chain loss;
- decontamination.

No heartbeat noise.

---

# 22. Accessibility

No color-only:
- storage capacity;
- risk;
- block state.

Actions keyboard accessible.

---

# 23. Performance Guardrails

Avoid:
- full inventory scan every frame;
- catalog parse per delivery;
- mass-balance logic in release hot path.

Use indexed item defs and event-driven read models.

---

# 24. CI / Static Gates

Plan 36 must make these enforceable:

```text
every registered producer has required ports
every output producer has delivery test
every stateful producer has round-trip
every live producer panel binds campaign authority
every producer catalog reaches runtime effect evidence
```

---

# 25. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --survivors-selftest
godot --headless --path . -- --content-utilization-selftest
godot --headless --path . -- --expansions-selftest
bash scripts/ci/verify-fast.sh
```

Plus:
- producer-port gate;
- 200-day mass-balance soak;
- storage snapshots;
- balance simulation.

---

# 26. Recommended Commit Breakdown

```text
35A-1 producer matrix + failing trapping test
35A-2 DeliveryBill / IOutputSink / DeliveryResult
35A-3 ProducerPorts / ValidatePorts
35A-4 trapping delivery
35A-5 water ADR + conversion
35A-6 greenhouse/apiculture/pharma/foundry
35A-7 remaining producers/events/save idempotence
35A-8 mass-balance soak

36A gate lands here

35B-1 storage ADR / capacity
35B-2 room storage
35B-3 perishables
35B-4 cold chain
35B-5 contamination/decon
35B-6 storage UI/balance

35C-1 job/labour contract
35C-2 fitness/skill/power/thermal
35C-3 stateVersion/queue
35C-4 quality/condition
35C-5 knowledge gate
35C-6 failure/steering
35C-7 read model/UI
35C-8 persistence/double-click tests
```

---

# 27. Risk Register

## R35.1 Balance shifts
Use mass-balance + balance sim.

## R35.2 Sink overgeneralization
Limit shared sink to delivery semantics, keep domain effects specialized.

## R35.3 Water duplication
ADR + transactional conversion.

## R35.4 Stuck full-storage jobs
Explicit blocked state/cancel/remedy.

## R35.5 Spoilage surprise
Warnings + remaining-life UI + grace.

## R35.6 Quality formula sprawl
Shared worker/equipment factor seams.

## R35.7 State-version scope
Migrate long-running producers first.

## R35.8 Transformations confuse balance
Track transformation categories and units.

---

# 28. Acceptance Checklist

## 35A

- [ ] producer chain matrix complete
- [ ] trapping regression captured
- [ ] DeliveryBill defined
- [ ] typed DeliveryResult
- [ ] IOutputSink
- [ ] inventory sink adapter
- [ ] multi-output batch semantics
- [ ] ProducerPorts
- [ ] required effects
- [ ] UnboundRequiredEffects
- [ ] ValidatePorts
- [ ] constructor enforcement
- [ ] trapping meat delivered
- [ ] hide/secondary output correct
- [ ] toxicity/processing respected
- [ ] trapping panel shows delivery
- [ ] delivery events
- [ ] water ADR
- [ ] one conversion factor
- [ ] transactional draw
- [ ] optional pour decision
- [ ] no dual water authority
- [ ] required water dependency non-null
- [ ] ConsumeRation wired/deleted
- [ ] greenhouse migrated
- [ ] apiculture migrated
- [ ] pharma audited
- [ ] foundry audited
- [ ] brine/desal audited
- [ ] salvage audited
- [ ] excavation audited
- [ ] kitchen boundary documented
- [ ] skill seams live
- [ ] power seams inventoried
- [ ] delivery event idempotent
- [ ] stable batch identity
- [ ] mid-production round-trip
- [ ] completed-before-refresh edge
- [ ] mass-balance ledger
- [ ] 200-day balance closes
- [ ] producer catalogs EFFECT_PRODUCED
- [ ] Plan 36 gate consumes producer registry

## 35B

- [ ] storage ADR
- [ ] one capacity authority
- [ ] shared capacity query
- [ ] reservation policy
- [ ] refused-output fate documented
- [ ] room storage mapped
- [ ] cellar/fridge real
- [ ] perishable authority
- [ ] one spoilage scheduler
- [ ] spoilage state persisted
- [ ] spoiled goods produced
- [ ] no vanish-on-spoil
- [ ] cold chain on real power
- [ ] grace window
- [ ] cold-chain events
- [ ] contamination persists
- [ ] decon real
- [ ] decon balance closes
- [ ] authored storage loss only
- [ ] loss attribution
- [ ] capacity pressure steerable
- [ ] fill read model
- [ ] perishable read model
- [ ] snapshots
- [ ] save round-trip
- [ ] deterministic spoilage
- [ ] extended mass balance
- [ ] balance sim
- [ ] capacity tests
- [ ] spoilage tests
- [ ] contamination tests

## 35C

- [ ] production job contract
- [ ] automatic producers explicit
- [ ] roster labour authority
- [ ] vacancy pauses work
- [ ] fitness applied
- [ ] shared worker-quality seam
- [ ] power real
- [ ] thermal real
- [ ] block event
- [ ] input timing documented
- [ ] cancellation semantics
- [ ] optimistic concurrency
- [ ] double-click protection
- [ ] stable job identity
- [ ] queue semantics
- [ ] queue read model
- [ ] quality inheritance
- [ ] no quality currency
- [ ] contamination propagation
- [ ] equipment condition/wear
- [ ] research gate
- [ ] hardcoded medical list removed
- [ ] research content boundary
- [ ] authored failures
- [ ] pre-warning
- [ ] recoverable failures
- [ ] long-job steering
- [ ] reassignment safe
- [ ] shared producer read model
- [ ] UI consistency
- [ ] no UI arithmetic fork
- [ ] semantic production events
- [ ] no heartbeat spam
- [ ] queue save/load
- [ ] worker-death edge
- [ ] power-outage edge
- [ ] quality determinism
- [ ] labour gate tests
- [ ] unpowered tests
- [ ] stale-version tests
- [ ] steering tests
- [ ] no-double-delivery integration
- [ ] expansions selftest

---

# 29. Ship / No-Ship Gate

**SHIP** only if:

```text
production_delivery_contracts == 1
AND output_producers_without_sink == 0
AND unbound_required_effects == 0
AND trapping_outputs_reach_inventory == true
AND water_authorities == 1
AND dead_water_ration_hook == 0
AND producer_chain_matrix_complete == true
AND producer_skill_seams_live == true
AND duplicate_batch_deliveries == 0
AND mass_balance_200_day == pass
AND capacity_refusals_silent == 0
AND storage_authorities == 1
AND perishable_spoilage_paths == 1
AND cold_chain_depends_on_power == true
AND contamination_persists == true
AND decontamination_mass_balance == pass
AND nonautomatic_producers_without_labour == 0
AND powered_producers_ignore_grid == 0
AND stale_state_actions_duplicate_output == 0
AND producer_panels_use_shared_read_semantics == true
AND producer_catalog_EFFECT_PRODUCED == true
AND producer_port_gate == pass
AND content_utilization_selftest == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 30. Implementer Handoff

1. Inventory every live producer and current output mutation.
2. Copy combat's `ValidatePorts()` discipline.
3. Mirror `InventoryBill` with one output vocabulary.
4. Fix trapping first.
5. Preserve toxicity/processing semantics.
6. Decide water authority in an ADR before coding.
7. Make required sinks non-null.
8. Wire or delete dead hooks.
9. Migrate working producers only enough to gain common validation/capacity/events.
10. Add stable batch IDs before save/load work.
11. Make the 200-day mass-balance soak a release criterion.
12. Interleave Plan 36A immediately after 35A.
13. Choose one storage authority.
14. Transform spoiled goods rather than deleting them.
15. Reuse contamination/decon systems.
16. Use roster, fitness, power, thermal, and condition authorities.
17. Standardize state-versioned long-job actions.
18. Keep research content design in its dedicated plan.
19. Ship only when every produced good can be traced from inputs and labour to a real sink, persistence, and briefing.

---

# 31. Final Outcome

When this plan is complete, production in ASHFALL becomes physically and economically real.

A trap no longer "catches" an animal only for a panel label. Butchery produces concrete meat and hides, or the game explains exactly why delivery failed. Greenhouse harvests, pharma batches, foundry runs, salvage hauls, and water production all terminate through one validated delivery contract.

Water stops existing as two unrelated truths. Treatment litres and packaged inventory water are connected by one auditable conversion and one authority decision.

Storage becomes part of the simulation. Full shelves can block production. Refrigeration buys time but depends on watts. Blackouts have grace and then visible consequences. Perishables become spoiled goods rather than disappearing. Contamination follows the goods until a real process removes it.

Production stops being a faucet. Jobs consume inputs, labour, time, power, equipment condition, and operator capability. Illness can vacate a shift and pause a batch. Stale UI commands cannot duplicate goods. Long jobs can be paused, cancelled, reassigned, and prioritized, while every panel uses the same semantics for inputs, worker, ETA, output, capacity, and risk.

A deterministic 200-day mass-balance test can finally answer where goods came from and where they went.

The result is not another crafting layer. It is the existing production economy finally closing its own loops.
