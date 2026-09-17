# C1 — Flagship Integration Plan [23]: Medical Afflictions → Quest, Work, Expedition & Combat Capability

> **Output:** `C1_planintegration[23].md`
>
> **Source baseline:** Plan 143 — Medical Afflictions → Quest & Work Bridge
>
> **Primary mission:** make survivor afflictions change what survivors can safely do, which authored quest routes are available, and how effectively they perform work, expeditions, and combat—without creating duplicate quest, duty, fitness, needs, or medical authorities.
>
> **Primary architectural rule:** affliction effects are a pure projection from canonical medical state. The bridge does not own affliction state, quest state, duty state, expedition state, combat state, or duplicated work-progress state.
>
> **Primary integration rule:** Plan 137 already introduces a needs→performance projection. Plan 143 must compose medical affliction penalties with that existing performance chain rather than creating an independent parallel work/combat multiplier path.
>
> **Mandatory execution order:** 143A authority audit + affliction capability contract → 143B duty/work + expedition/combat composition → 143C quest gating/unlocking/modification through canonical quest predicates → 143D UI, recovery, save/determinism, balance and CI hardening.
>
> **Critical scope rule:** the source proposes work-quality failures, medical side-effect chance, XP penalties, random work refusals, new quest events, new recovery traits, moral-outlook changes, caregiving bonds, stigma, mutations and special “push through” mechanics. These ship only if a current canonical authority already provides the seam. Otherwise they are explicitly deferred rather than being created inside Plan 143.
>
> **Guardrails:** no second fitness verdict; no second NeedsPerformance bridge; no saved derived modifier state; no quest list owned by medical code; no direct work-output mutation in UI; no random refusal inside affliction projection; no hidden dialogue mutation outside canonical quest/dialogue resolver; no direct expedition/combat penalty if Plan 137 already owns the domain multiplier pipeline; no invented treatment/trait system; no new tutorial engine; no unbounded multiplicative death spiral; no blocked mainline quest without an authored alternate path or explicit design approval.

---

# 0. Mission

ASHFALL already tracks medical afflictions through `MedicalPipelineCoordinator` and related systems.

The source baseline identifies a continuity break:

```text
AFFLICTION STATE
  broken leg
  radiation sickness
  combat trauma
  respiratory degeneration
  chemical dependency
        │
        ├── medical pipeline knows it
        │
        └── downstream gameplay mostly does not
```

The current result is mechanically flat:

```text
broken leg
→ still works at full rate

severe radiation sickness
→ still eligible for dangerous expedition

combat trauma
→ same combat/quest options

respiratory degeneration
→ same outdoor-duty behavior
```

At the same time, the repository already contains adjacent systems that must not be duplicated:
- Plan 24 survivor fitness / duty eligibility;
- Plan 137 needs→performance projection;
- `SomaticFlashbackSystem.workEfficiencyPenalty`;
- `DutyRosterSystem`;
- expedition readiness;
- tactical combat;
- canonical quest runtime;
- medical state and recovery;
- chemical dependency;
- current social/mental-health systems.

The target architecture is:

```text
CANONICAL MEDICAL STATE
       │
       ▼
AfflictionCapabilityProjection
       │
       ├── eligibility/exclusion reasons
       ├── work contribution
       ├── expedition readiness contribution
       ├── combat capability contribution
       └── quest predicate contribution
       │
       ▼
EXISTING COMPOSITION LAYERS
       │
       ├── Fitness / assignment eligibility
       ├── NeedsPerformance / throughput
       ├── Quest prerequisite resolver
       ├── Expedition readiness
       └── Tactical combat calculation
```

The bridge should answer:

> Given this survivor's active afflictions and their current severity, what constraints and modifier contributions should the existing systems consume?

It should **not** answer:
- whether the affliction exists;
- how it progresses;
- whether treatment succeeds;
- whether the quest is globally active;
- whether a work job succeeds;
- whether the survivor refuses;
- whether combat AI behaves differently.

Those remain existing authorities.

---

# 1. Source-Evidence Interpretation

## 1.1 The source correctly identifies the dead work penalty

`SomaticFlashbackSystem` already carries `workEfficiencyPenalty`, but the source reports that `DutyRosterSystem` does not consume it.

This is a strong signal that ASHFALL already has an intended modifier seam but not a finished composition path.

Plan 143 should repair that path rather than adding a second `AfflictionWorkBridge` that bypasses Plan 137.

## 1.2 Plan 137 changes the correct implementation shape

Plan 137 integrates:
- needs;
- work;
- expedition;
- combat
through one performance projection.

Therefore afflictions should likely become another **source contribution** to the same final domain performance pipeline.

## 1.3 Affliction gates should be declarative quest predicates

The source proposes:
- blocks;
- unlocks;
- modifies.

These map cleanly to quest prerequisites/branch predicates if the current quest runtime supports declarative conditions.

No medical-owned quest list is needed.

## 1.4 Quest modification is much more dangerous than gating

"Different dialogue/outcomes" can easily create a second narrative resolver.

143C must use:
- existing quest predicate;
- existing dialogue condition;
- existing choice/effect resolver.

## 1.5 Some source modifiers are speculative

Examples:
- radiation sickness reducing work quality by 50%;
- chemical dependency causing random refusals;
- combat trauma excluding all guard duty;
- recovered survivor gaining a new "Survivor" trait.

These are not repository facts.

Treat them as candidate content only after the real domain seams are verified.

## 1.6 Derived bridge state should not be persisted

The source proposes `AfflictionBridgeState` with active gates/modifiers and `CaptureState/RestoreState`.

That is unnecessary if:
- afflictions are already saved;
- gates/modifiers are pure functions.

The flagship explicitly removes redundant persistence unless a non-derivable one-shot quest state proves necessary.

---

# 2. Non-Negotiable Affliction Integration Invariants

## INV-143.1 — One medical authority

`MedicalPipelineCoordinator` and canonical affliction state remain source of truth.

## INV-143.2 — Affliction projection is pure

Same affliction state + rule data => same capability result.

No RNG.

## INV-143.3 — Derived modifiers are not saved

Persist afflictions, not their calculated output.

## INV-143.4 — Fitness owns hard availability

If Plan 24 already says survivor is:
- unfit;
- incapacitated;
then affliction bridge does not invent a contradictory parallel eligibility state.

## INV-143.5 — Plan 137 owns final throughput composition

Affliction work/combat/expedition contributions feed the same pipeline.

No second final multiplier.

## INV-143.6 — Quest runtime owns quest state

Medical projection only supplies predicates/conditions.

## INV-143.7 — Treatment recovery removes constraints automatically

When affliction clears/improves:
- query result updates;
- no stale bridge state.

## INV-143.8 — Quest blocks are scoped

Affliction may block:
- one survivor role;
- one route;
- one optional quest path.

It must not silently make the campaign unwinnable.

## INV-143.9 — Unlocks must be authored

No generic "affliction X always creates quest Y" unless a real quest definition exists.

## INV-143.10 — Work penalties are bounded

No single ordinary affliction reduces throughput to zero unless the fitness authority marks the survivor unavailable.

## INV-143.11 — Multiple afflictions compose once

No duplicate multiplier from:
- affliction bridge;
- SomaticFlashback;
- needs bridge;
- fitness.

## INV-143.12 — Work quality only if a real quality authority exists

Do not invent defects.

## INV-143.13 — Random refusal only if a real refusal/leadership system exists

No RNG inside capability projection.

## INV-143.14 — Combat trauma is not a universal combat ban

Use authored severity/fitness/mental-state contracts.

## INV-143.15 — Contagion exclusions use real infection-control rules

Food handling / close-contact restrictions come from medical/ward/isolation policy, not arbitrary flavor.

## INV-143.16 — Expedition gating uses canonical readiness

No direct hidden `if broken_leg` branch in expedition UI.

## INV-143.17 — UI reads the same projection

No parallel tooltip formula.

## INV-143.18 — Recovery traits are out of scope unless a trait authority already owns them

No one-off permanent bonuses in medical bridge.

---

# 3. Definition of Done

Plan 143 closes only when:

- all current affliction definitions and severity semantics are verified;
- Plan 24 fitness overlap is audited;
- Plan 137 performance composition seam is audited;
- `SomaticFlashbackSystem` work penalty is either integrated into the same projection or retired as duplicate/unwired state;
- one affliction capability projection exists if no current canonical projection already does;
- no derived bridge save section exists;
- affliction work modifiers are data-driven and bounded;
- hard duty exclusions use real duty categories and existing fitness/isolation rules;
- work output consumes affliction contribution exactly once;
- expedition readiness consumes affliction contribution exactly once;
- combat consumes affliction contribution exactly once if combat-effect dimensions are supported;
- work quality remains unchanged unless a real quality system exists;
- random refusal remains absent unless a real refusal authority exists;
- quest blocks/unlocks/modifiers use canonical quest predicates;
- all referenced quest IDs resolve;
- no critical quest is permanently blocked without alternate path;
- recovery removes quest blocks automatically;
- medical UI shows affliction consequences;
- duty UI shows exclusion/efficiency reason;
- expedition UI shows readiness consequence;
- combat UI shows impairment only if combat projection includes it;
- old saves require no bridge migration;
- affliction projection is deterministic/headless;
- multiple afflictions compose within floors;
- 30/120/180-day balance simulations prove treatment remains strategically valuable without creating irreversible shelter collapse;
- `--affliction-bridges-selftest` or equivalent named gate exists;
- data integrity validates affliction/rule/quest/duty IDs;
- content acceptance/reachability validates authored medical quest hooks;
- speculative source features are dispositioned explicitly.

---

# 4. Phase P0 — Authority, Catalog & Overlap Audit

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
affliction catalog path
affliction count
affliction severity fields
MedicalPipelineCoordinator APIs
FitnessVerdict inputs
NeedsPerformance projection API
SomaticFlashback work penalty
Trauma/mental health outputs
DutyRoster duty IDs
work-output calculation seam
Expedition readiness API
TacticalCombat performance seam
quest prerequisite/condition APIs
dialogue condition APIs
save sections
```

## P0.2 Verify affliction catalog

The source says `afflictions.json` — verify actual path and schema.

Generate:

`docs/medical/AFFLICTION_CATALOG_AUDIT.md`

Columns:

```text
affliction
severity model
work implication
duty exclusion candidate
expedition implication
combat implication
quest hook candidate
current consumer
```

## P0.3 Audit Plan 24 overlap

Questions:

```text
Does fitness already consume affliction state?
Which afflictions already make survivor unfit?
Does fitness expose reason codes?
Does duty roster already reject unfit survivors?
```

## P0.4 Audit Plan 137 overlap

Questions:

```text
Does final work multiplier accept external sources?
Does final combat multiplier accept external sources?
Does expedition preview consume same projection?
How are contributions explained?
```

## P0.5 Audit SomaticFlashback

Determine:
- active affliction?
- work penalty stored where?
- queried anywhere?
- should it become data-backed affliction modifier?

## P0.6 Audit duty taxonomy

Do not write:
- heavy labor;
- food handling;
- medical;
- guard
until exact duty IDs/types are verified.

## P0.7 Audit quest predicate model

Find supported condition forms:
- survivor state;
- tag;
- world flag;
- item;
- standing;
- medical predicate?
- arbitrary callback?

Prefer declarative data.

## P0.8 Baseline no-effect reproduction

Prove current:
- broken-leg-like affliction does not change work;
- severe medical state does not alter quest availability.

Keep as regression proof.

---

# TASK 143A — Affliction Capability Projection

# 143A.0 Goal

Create one engine-free projection from active afflictions into:
- availability/exclusions;
- performance contributions;
- quest predicate facts.

## 143A.1 Prefer extension over bridge duplication

If Plan 137 exposes pluggable performance sources:
- implement `IAfflictionPerformanceSource`.

If not:
- extend canonical performance projection.

Only create separate `AfflictionWorkBridge` if no cleaner seam exists.

## 143A.2 Suggested Core file

If new:

`Assets/Ashfall.Core/Medical/AfflictionCapabilityProjection.cs`

## 143A.3 Result DTO

Suggested:

```text
AfflictionCapabilityResult
  duty_exclusions[]
  work_multiplier
  combat_multiplier
  expedition_multiplier
  quest_facts[]
  reasons[]
```

Only include supported dimensions.

## 143A.4 Do not include final domain multiplier

Plan 137 composes final result.

This result is one contribution.

## 143A.5 Rule data

Create/extend:

`affliction_capability_rules.json`

## 143A.6 Rule fields

Suggested:

```text
affliction_id
min_severity
max_severity
work_multiplier
combat_multiplier
expedition_multiplier
excluded_duty_tags[]
quest_fact_tags[]
reason_key
```

## 143A.7 Quest facts vs quest IDs

Prefer reusable facts:

```text
has_severe_mobility_affliction
has_contagious_affliction
has_severe_respiratory_affliction
has_dependency_affliction
```

Then quests declare predicates.

Avoid medical file owning quest IDs where possible.

## 143A.8 Direct quest mapping only when authored one-to-one

Some medical crisis quest may explicitly reference an affliction.

Then validate quest ID.

## 143A.9 Severity semantics

Use canonical medical severity.

Do not duplicate mild/moderate/severe derivation if medical system already has it.

## 143A.10 Missing severity

If affliction lacks graded severity:
- boolean active rule.

## 143A.11 Work multiplier neutral

No matching affliction:
- 1.0.

## 143A.12 Expedition multiplier neutral

1.0.

## 143A.13 Combat multiplier neutral

1.0.

## 143A.14 Hard exclusion

Only when:
- rule declares;
- duty tag exists;
- severity threshold satisfied.

## 143A.15 Composition with fitness

If fitness says unfit:
- exclusion reason comes from fitness first.

Affliction exclusion adds narrower duty restriction where survivor otherwise remains fit.

## 143A.16 Multiple afflictions

Candidate:
- multiplicative contribution with configured floor;
or:
- worst-of for domain.

Reuse Plan 137 composition strategy.

## 143A.17 No arbitrary per-affliction stacking formula

One shared method.

## 143A.18 SomaticFlashback migration

Move hardcoded `workEfficiencyPenalty` into:
- canonical projection contribution;
or:
- data-backed rule.

Retire duplicate field if safe.

## 143A.19 Chemical dependency

Do not add random refusal here.

If dependency state already affects fitness/performance:
- compose once.

## 143A.20 Contagion

Duty exclusions may rely on:
- isolation state;
- contagious flag.

Prefer ward/isolation authority over a static affliction list.

## 143A.21 Respiratory degeneration

Outdoor duty exclusion only if:
- outdoor duty tag exists;
- design validates.

## 143A.22 Broken-limb examples

Map to actual affliction IDs, not invented names.

## 143A.23 Pure query

No tick.

## 143A.24 No persistence

No `AfflictionBridgeState`.

## 143A.25 Recovery

Affliction cleared:
- output returns neutral automatically.

## 143A.26 Partial recovery

Severity drops:
- contribution improves.

## 143A.27 Reason trace

For every non-neutral effect:

```text
affliction_id
severity
rule_id
domain
contribution
reason_key
```

## 143A.28 Invalid rule ID

Integrity fail.

## 143A.29 Invalid duty tag

Integrity fail.

## 143A.30 Invalid quest fact

Generated registry validation.

## 143A.31 Unit tests

Per affliction class/severity.

## 143A.32 Combination tests

2–4 concurrent afflictions.

## 143A.33 Floor tests

No output below domain floor unless fitness excludes survivor.

## 143A.34 Monotonic tests

Worse severity does not improve capability.

## 143A.35 Recovery tests

Treatment/recovery improves output.

## 143A.36 Determinism

Same state => same result.

## 143A.37 Generated matrix

Create:

`docs/medical/AFFLICTION_CAPABILITY_MATRIX.md`

### 143A DoD

Afflictions expose one pure, bounded, explainable capability contribution that composes with existing survivor-performance and fitness systems.

---

# TASK 143B — Duty, Work, Expedition & Combat Integration

# 143B.0 Goal

Make operational systems consume affliction capability exactly once.

---

# 143B-W — Duty / Work

## 143B.W1 Find final work-output seam

Use same seam as Plan 137.

## 143B.W2 Compose needs + affliction

Final conceptual:

```text
base
× skill
× facility/tool
× needs contribution
× affliction contribution
× trauma/social contribution
→ floor/clamp
```

Exact order documented.

## 143B.W3 No duplicate somatic penalty

Somatic flashback contribution appears once.

## 143B.W4 Duty exclusion

Duty assignment asks:
- fitness;
- affliction exclusion.

## 143B.W5 Rejection reason

Typed:
- unfit;
- excluded_by_affliction;
- isolated;
- already_assigned;
etc.

## 143B.W6 No silent auto-removal without policy

If survivor develops affliction mid-shift:
- existing reassignment policy decides.

Document:
- immediate stop;
- finish shift;
- next tick re-evaluation.

## 143B.W7 Work speed/output

Apply one throughput dimension.

## 143B.W8 Work quality

Source proposes:
- defect chance;
- medical side effects;
- teaching XP.

Default:
- DEFER unless a canonical quality/error system already exists.

## 143B.W9 Heavy duty

Use actual duty tags only.

## 143B.W10 Food handling

Contagion exclusion only through infection-control policy.

## 143B.W11 Medical duty

Do not automatically ban contagious survivor from medical duty if current ward PPE/isolation rules allow.

Use authored policy.

## 143B.W12 Duty preview

UI shows:
- expected multiplier;
- exclusion reason.

## 143B.W13 Mid-shift recovery

If system supports recalculation:
- next authoritative tick updates.

## 143B.W14 Work tests

- neutral;
- one affliction;
- multiple;
- recovery;
- exclusion;
- isolated patient.

---

# 143B-E — Expedition

## 143B.E1 Reuse canonical readiness

Plan 24 fitness + Plan 137 expedition performance.

## 143B.E2 Affliction hard block

Only where:
- severe state;
- expedition duty category;
- medical design says unavailable.

## 143B.E3 Affliction soft penalty

If survivor remains eligible:
- expedition performance contribution.

## 143B.E4 Broken mobility

Use real mobility-related affliction ID.

## 143B.E5 Severe radiation/respiratory state

Only block if fitness/readiness contract says so.

## 143B.E6 Preview parity

Dispatch preview shows same result actual expedition uses.

## 143B.E7 Recovery before dispatch

Treatment updates preview.

## 143B.E8 No expedition mutation in medical UI

UI only reads.

## 143B.E9 Expedition tests

- healthy;
- mobility-afflicted;
- respiratory-afflicted;
- severe vs moderate;
- recovery.

---

# 143B-C — Combat

## 143B.C1 Reuse Plan 137 combat slice

Affliction contribution feeds final combat performance.

## 143B.C2 Do not duplicate TacticalCombat checks

No direct `if afflictionId == ...` branches throughout combat.

## 143B.C3 Combat dimensions

Only existing:
- accuracy;
- initiative/AP;
- damage
as supported.

## 143B.C4 Combat trauma

Do not universally ban combat.

Use:
- fitness;
- severity;
- mental-state authority.

## 143B.C5 Broken arm/limb

Only if actual affliction + combat mechanic supports meaningful penalty.

## 143B.C6 Incapacitated

Fitness excludes.

## 143B.C7 AI parity

Same projection for survivor combat actors.

## 143B.C8 Combat tests

- neutral;
- affliction;
- severe;
- recovery;
- no double needs+affliction application.

### 143B DoD

Duty, work, expedition and combat consume one affliction contribution through existing fitness/performance authorities with no duplicate math.

---

# TASK 143C — Quest Blocking, Unlocking & Conditional Modification

# 143C.0 Goal

Make authored quests react to survivor medical state through the canonical prerequisite/branch system.

## 143C.1 Quest authority audit result

Document:
- quest availability owner;
- predicate API;
- branch condition API.

## 143C.2 Prefer quest-side predicates

Example:

```text
requires:
  survivor_capability:
    not_tags: [severe_mobility_affliction]
```

or current schema.

## 143C.3 Do not create medical-owned active gate list

Quest system evaluates current facts.

## 143C.4 Block semantics

A quest can be blocked because:
- selected survivor is medically ineligible;
- a specific actor is incapacitated;
- contagious-state policy forbids participation.

## 143C.5 Global quest vs survivor assignment

Distinguish:

```text
QUEST unavailable globally
vs
QUEST available but survivor X cannot be assigned
```

Prefer the second when possible.

## 143C.6 Avoid key-quest softlock

Mainline/critical quest:
- alternate survivor;
- delayed route;
- treatment path;
or explicit authored failure branch.

## 143C.7 Unlock semantics

Medical state can make a quest **eligible**.

Example:
- dependency treatment quest;
- anti-rad supply quest;
- filtration upgrade quest.

Only if quest exists.

## 143C.8 No dynamic quest invention

Bridge cannot create a quest definition.

## 143C.9 Quest fact registry

Expose stable facts.

## 143C.10 Multiple afflictions

Quest predicates can require:
- count;
- severity band;
- specific tags.

No hardcoded bridge switch.

## 143C.11 Modify semantics

Dialogue/outcome modification goes through:
- canonical branch predicate;
- dialogue condition.

## 143C.12 No hidden quest-result override

Medical bridge never changes quest reward directly.

## 143C.13 Push-through option

Only if quest/choice system already supports:
- alternate choice;
- medical consequence effect.

Otherwise DEFER.

## 143C.14 Seek-help-first option

Can be an authored branch if:
- treatment route exists;
- quest delay/commitment system supports.

## 143C.15 Affliction worsens after push-through

Only through canonical medical effect API.

No quest-side mutation of raw affliction state.

## 143C.16 Quest event hooks

Source examples:
- Sick Leader;
- Quarantine;
- Medical Emergency;
- Healer's Dilemma;
- Plague;
- Wounded Warrior;
- Addiction.

Treat as **content backlog**, not architecture acceptance requirement.

## 143C.17 Existing quest reuse first

Audit whether similar medical quests already exist.

## 143C.18 Minimum authored test set

Use 6–10 real quests/scenarios after predicate seam lands.

## 143C.19 Quest reference integrity

All quest IDs resolve.

## 143C.20 Recovery unblock

Treat/recover:
- predicate evaluates true next query.

## 143C.21 No stale cached gate

Quest UI refresh on relevant medical event.

## 143C.22 Quest journal

Only log:
- quest blocked/unblocked if player-facing/important.

Do not spam every predicate re-evaluation.

## 143C.23 Reachability

Medical quest hooks must be:
- triggerable;
- completable;
- not pathological.

## 143C.24 Mainline reachability simulation

Inject severe afflictions.

Assert campaign remains viable.

## 143C.25 Quest-modification snapshots/tests

Same quest:
- healthy survivor branch;
- afflicted survivor branch;
- recovered branch.

### 143C DoD

Medical state participates in the same quest prerequisite and branch machinery as every other campaign fact, with no medical-owned quest runtime.

---

# TASK 143D — UI, Recovery, Persistence, Balance & CI

# 143D.0 Goal

Make affliction consequences visible, automatically recoverable, save-safe, deterministic and balanced over long campaigns.

## 143D.1 Medical panel

Each active affliction may show:

```text
work effect
duty exclusions
expedition restriction
combat effect
quest implications
```

Only supported effects.

## 143D.2 Duty roster

Show:
- final expected efficiency;
- affliction contribution;
- exclusion reason.

## 143D.3 Quest panel

Show:
- blocked reason;
- required recovery/treatment path where authored.

## 143D.4 Expedition panel

Show:
- readiness reason;
- performance penalty.

## 143D.5 Combat UI

Compact impairment reason if supported.

## 143D.6 Tooltip uses one projection

No duplicate formulas.

## 143D.7 UI refresh event

Medical state changed:
- invalidate/recompute read model.

## 143D.8 No polling

No frame-by-frame full affliction scan.

## 143D.9 Recovery

When affliction:
- clears;
- severity improves;
- isolation ends,

all downstream constraints update automatically.

## 143D.10 No permanent recovery trait by default

Source proposes "Survivor" trait.

Disposition:
- DEFER unless canonical trait system + balance design already supports.

## 143D.11 Moral-outlook change

DEFER to narrative/social system.

## 143D.12 Caregiver bonds

DEFER to Plan-44/social systems unless current medical event already feeds them.

## 143D.13 Medical journal

Use existing journal.

Only semantic milestones:
- major duty restriction;
- quest unlock;
- recovery.

## 143D.14 Tutorial

Only existing tutorial framework.

## 143D.15 Old save

No new bridge state.

Restore afflictions:
- derive effects.

## 143D.16 No `CaptureState/RestoreState` for projection

Assert source scan.

## 143D.17 Quest state persistence

Quest system owns any already-started medical quest.

## 143D.18 Mid-affliction save

Reload:
- same constraints.

## 143D.19 Mid-recovery save

Reload:
- same severity;
- same output.

## 143D.20 Determinism

Same affliction state:
- same modifier/gates.

## 143D.21 Headless

Works without UI.

## 143D.22 Data integrity

Validate:
- affliction IDs;
- duty tags;
- quest facts;
- quest IDs;
- severity bands;
- multiplier ranges.

## 143D.23 Selftest

Create/keep:

```text
--affliction-bridges-selftest
```

## 143D.24 Selftest cases

At least:
1. no affliction;
2. one work modifier;
3. hard duty exclusion;
4. expedition restriction;
5. combat contribution;
6. quest block;
7. quest unlock;
8. multiple afflictions;
9. treatment/recovery;
10. old-save-derived parity.

## 143D.25 30-day simulation

Measure:
- afflicted workers;
- output loss;
- treatment recovery;
- blocked quest count.

## 143D.26 120-day simulation

Track:
- average affliction load;
- work efficiency;
- expedition availability;
- quest availability.

## 143D.27 180-day stress simulation

Harsh medical load.

Assert:
- campaign remains recoverable;
- critical quests have alternate path.

## 143D.28 Death-spiral guard

Scenario:

```text
injury
→ reduced work
→ fewer medical resources
→ worsening condition
```

Must have recovery path.

## 143D.29 Max work penalty

Source suggests max 50%.

Treat as balance starting point.

Use configured floor.

## 143D.30 Multiple affliction floor

No near-zero output unless fitness marks unavailable.

## 143D.31 Treatment value

Telemetry should show:
- restoring capability has measurable benefit.

## 143D.32 No over-prioritization exploit

Do not make one treatment always dominate all shelter priorities.

## 143D.33 Performance benchmark

Projection:
- O(active afflictions for survivor);
- no catalog parsing per query.

## 143D.34 Allocation budget

Avoid per-frame/per-query heap churn.

## 143D.35 Content acceptance

Quest gate data reaches:
- QUERIED;
- EFFECT_PRODUCED when gameplay route changes.

## 143D.36 Reachability

Medical quest unlocks/blocks are exercised in synthetic campaigns.

## 143D.37 Source-scan authority gate

Fail:
- direct quest mutation inside bridge;
- duplicate work formula;
- saved modifier state;
- random refusal logic.

## 143D.38 Docs

Create/update:
- `AFFLICTION_CAPABILITY_ARCHITECTURE.md`;
- `AFFLICTION_CAPABILITY_MATRIX.md`;
- `AFFLICTION_QUEST_PREDICATES.md`;
- `AFFLICTION_SCOPE_DISPOSITION.md`.

### 143D DoD

Affliction effects are visible, derived, deterministic, recoverable and integrated through the same gameplay authorities used by healthy survivors.

---

# 5. Affliction Capability State Model

Conceptual:

```text
MEDICAL AFFLICTION
   │
   ├── severity
   ├── contagion/isolation
   └── state
        │
        ▼
CAPABILITY CONTRIBUTION
   │
   ├── can/cannot perform duty
   ├── work efficiency
   ├── expedition readiness
   ├── combat effectiveness
   └── quest facts
```

No extra persisted bridge state.

---

# 6. Hard Exclusion vs Soft Penalty Contract

## Hard exclusion

Use when survivor **cannot safely/physically** perform domain role.

Owned in composition with:
- fitness;
- isolation;
- duty policy.

## Soft penalty

Use when survivor remains eligible but performs worse.

Feeds Plan 137.

Do not use both automatically for same affliction severity.

---

# 7. Quest Block vs Assignment Block Contract

Prefer:

```text
quest remains available
but Survivor A cannot be assigned
```

over:

```text
quest disappears
```

unless quest specifically requires that person.

This avoids medical softlocks.

---

# 8. Quest Unlock Contract

Medical state can expose a predicate:

```text
has_severe_radiation_affliction = true
```

The quest authority decides whether a matching authored quest becomes available.

---

# 9. Work Composition Contract

Final work:

```text
base progress
× skill
× facility/tool
× needs
× affliction
× trauma/social
→ floor
```

No extra calculation in duty UI.

---

# 10. Expedition Composition Contract

```text
fitness eligibility
+ affliction exclusions
+ needs performance
+ affliction performance
→ existing team aggregation
→ route preview
```

Same value used at runtime.

---

# 11. Combat Composition Contract

Only if real combat dimensions exist:

```text
base combat
× skill/equipment
× needs contribution
× affliction contribution
× trauma/other
→ clamp
```

No special medical combat engine.

---

# 12. Somatic Flashback Integration Contract

Current hardcoded work penalty must end in one of three states:

```text
A. moved into data-backed affliction contribution
B. adapted into canonical performance source
C. deleted as dead duplicate
```

Never remain unwired.

---

# 13. Contagion Duty Contract

Contagious disease restrictions derive from:
- isolation;
- contact rules;
- PPE/policy if existing.

Not a blanket hardcoded food/medical ban.

---

# 14. Dependency Contract

Chemical dependency is already deeper than a simple affliction flag.

Plan 143 should not flatten it.

Use:
- dependency authority;
- capability contribution only when real state says impaired.

---

# 15. Quest Predicate Contract

Allowed predicate classes may include:

```text
has_affliction
affliction_severity_at_least
has_affliction_tag
survivor_capability_tag
survivor_isolated
survivor_fit_for_role
```

Use actual quest schema.

---

# 16. UI Explainability Contract

For a survivor:

```text
Work efficiency: 72%
  Fatigue: -12%
  Broken leg: -18%

Heavy labor: unavailable
  Severe mobility impairment

Expedition: available with penalty
  Travel speed: -20%
```

Values illustrative only.

---

# 17. Recovery Contract

Treatment is valuable because it:
- improves medical state;
- automatically removes derived operational penalties.

No bridge event needed to "clear" a modifier.

---

# 18. Persistence Matrix

| Fact | Persist owner |
|---|---|
| affliction | medical |
| severity | medical |
| isolation | medical/ward |
| work modifier | derived |
| quest predicate facts | derived |
| duty exclusion | derived |
| expedition restriction | derived |
| combat contribution | derived |
| quest active/completed | quest |
| work progress | production |
| expedition state | expedition |

---

# 19. Speculative Feature Disposition

Create `AFFLICTION_SCOPE_DISPOSITION.md` for:

```text
work quality / defects
medical side-effect chance
teaching XP reduction
random work refusal
Sick Leader event
Quarantine event
Medical Emergency event
Pushing Through event
Healer's Dilemma
Plague quest
Wounded Warrior
Addiction quest
Survivor recovery trait
moral outlook change
caregiving bond
stigma
affliction mutation
specialization/research
```

Status:
- IMPLEMENT via existing authority;
- DEFER;
- REJECT.

---

# 20. Failure Injection Matrix

## N143.1 Broken-leg rule changes final work twice
Expected: no-double-count gate fails.

## N143.2 Bridge saves derived work modifier
Expected: architecture gate fails.

## N143.3 Quest bridge directly marks quest active
Expected: quest-authority gate fails.

## N143.4 Medical UI computes separate multiplier
Expected: source-scan fail.

## N143.5 Contagious survivor blocked from all medical duty despite PPE policy
Expected: duty-policy integration test fails.

## N143.6 Recovery leaves quest blocked
Expected: pure projection/predicate refresh fails.

## N143.7 Multiple afflictions reduce output below floor while survivor remains fit
Expected: bound test fails.

## N143.8 Random work refusal inside projection
Expected: determinism/source-scan fails.

## N143.9 Critical quest has no alternate survivor/path
Expected: quest reachability gate fails.

## N143.10 Expedition preview says available but runtime rejects
Expected: parity test fails.

## N143.11 SomaticFlashback penalty remains dead/unconsumed
Expected: integration gate fails.

## N143.12 Old save requires missing bridge section
Expected: migration test fails.

---

# 21. Determinism Contract

Same:

```text
afflictions
+ severity
+ isolation
+ rule data
+ current fitness/needs state
```

must yield same:

```text
capability contribution
duty exclusions
quest facts
domain performance contribution
```

No seed required.

---

# 22. Long-Horizon Metrics

Track:

```text
afflicted survivor-days
avg work penalty from afflictions
hard duty exclusion-days
expedition unavailable-days
combat impairment-days
quests blocked
quests unlocked
recovery time
output restored by treatment
critical quest softlock incidents
```

---

# 23. Balance Guardrails

Afflictions should:
- make treatment strategically valuable;
- force reassignment;
- alter quest planning.

They should not:
- permanently bench half the shelter;
- make one injury unrecoverable;
- create automatic resource collapse.

---

# 24. Content Acceptance

Rules/quest hooks:

```text
DISCOVERED
LOADED
REGISTERED
QUERIED
SELECTED
EFFECT_PRODUCED
```

Gameplay hooks need effect proof.

---

# 25. Reachability Contract

For every authored affliction quest hook:

```text
can affliction occur?
can quest predicate become true?
can player see quest?
can quest complete?
can recovery remove block?
```

---

# 26. Performance Budget

Capability projection:
- proportional to active afflictions;
- indexed rules;
- no full catalog scan;
- no heap-heavy LINQ in hot path.

---

# 27. Accessibility

All restrictions:
- text + icon;
- no color-only severity;
- keyboard/tooltips;
- text-scale safe.

---

# 28. CI / Gate Set

Recommended:

```text
affliction_capability_integrity
affliction_no_duplicate_performance
affliction_duty_exclusion
affliction_expedition_parity
affliction_combat_integration
affliction_quest_predicates
affliction_quest_reachability
affliction_recovery
affliction_determinism
affliction_balance
affliction_ui_access
```

---

# 29. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --medical-selftest
godot --headless --path . -- --survivors-selftest
godot --headless --path . -- --affliction-bridges-selftest
godot --headless --path . -- --real-campaign-journey-selftest
bash scripts/ci/content-acceptance-gate.sh
bash scripts/ci/verify-fast.sh
```

Use exact current command names if renamed.

---

# 30. Recommended Commit Breakdown

```text
143A-1 medical/fitness/performance/quest authority audit
143A-2 affliction capability rule schema
143A-3 pure projection
143A-4 SomaticFlashback migration
143A-5 duty/quest fact registries
143A-6 bounds/composition tests
143A-7 generated matrices
143A-8 selftest/docs

143B-1 duty exclusion integration
143B-2 work performance composition
143B-3 Plan-137 no-double-count tests
143B-4 expedition readiness/performance
143B-5 dispatch preview parity
143B-6 combat contribution
143B-7 AI/headless integration
143B-8 operational integration tests

143C-1 quest predicate adapter
143C-2 quest block semantics
143C-3 quest unlock semantics
143C-4 dialogue/branch predicates
143C-5 recovery/unblock refresh
143C-6 authored scenario set
143C-7 mainline reachability
143C-8 quest docs/tests

143D-1 medical/duty/quest/expedition UI
143D-2 old-save/derived-state tests
143D-3 deterministic fingerprints
143D-4 30-day balance
143D-5 120-day balance
143D-6 180-day softlock/death-spiral stress
143D-7 CI gates/failure fixtures
143D-8 scope disposition/final ship report
```

---

# 31. Risk Register

## R143.1 Duplicates Plan 137 work penalties

Mitigation:
- affliction as contribution source;
- no second final multiplier.

## R143.2 Medical quest gating softlocks campaign

Mitigation:
- assignment block before global quest block;
- alternate paths;
- reachability tests.

## R143.3 Hard exclusions over-bench survivors

Mitigation:
- use fitness/isolation first;
- bounded rules.

## R143.4 SomaticFlashback remains duplicate state

Mitigation:
- migrate/adapter/delete.

## R143.5 Work-quality scope balloons

Mitigation:
- defer unless existing quality authority.

## R143.6 Quest modification creates new narrative engine

Mitigation:
- canonical predicates/dialogue branches only.

## R143.7 Treatment becomes mandatory for every minor affliction

Mitigation:
- severity-based soft penalties;
- long-horizon balance.

## R143.8 UI presents stale effects

Mitigation:
- pure query + medical-state refresh.

---

# 32. Acceptance Checklist

## P0

- [ ] affliction catalog verified
- [ ] affliction count captured
- [ ] severity model documented
- [ ] MedicalPipelineCoordinator APIs audited
- [ ] Plan 24 fitness overlap audited
- [ ] Plan 137 performance seam audited
- [ ] SomaticFlashback penalty audited
- [ ] dependency overlap audited
- [ ] duty taxonomy verified
- [ ] work output seam verified
- [ ] expedition readiness seam verified
- [ ] combat performance seam verified
- [ ] quest predicate/branch APIs verified
- [ ] save sections audited
- [ ] baseline no-effect reproduced

## 143A

- [ ] existing projection extended before new bridge
- [ ] capability projection pure
- [ ] result DTO contains only supported dimensions
- [ ] no final multiplier duplication
- [ ] rules in data
- [ ] quest facts preferred over quest IDs
- [ ] direct quest mapping only when authored
- [ ] canonical severity used
- [ ] boolean affliction fallback
- [ ] neutral work/combat/expedition = 1.0
- [ ] hard exclusions data-driven
- [ ] fitness precedence
- [ ] multiple-affliction composition matches Plan 137
- [ ] one shared stacking method
- [ ] SomaticFlashback migrated
- [ ] random dependency refusal absent
- [ ] contagion uses isolation policy
- [ ] outdoor/mobility rules use real tags
- [ ] pure query/no tick
- [ ] no bridge persistence
- [ ] recovery automatic
- [ ] severity improvement automatic
- [ ] reason trace
- [ ] invalid IDs fail
- [ ] unit tests
- [ ] combination tests
- [ ] floors
- [ ] monotonic
- [ ] recovery
- [ ] determinism
- [ ] generated matrix

## 143B — Work

- [ ] final output seam used
- [ ] needs+affliction composed once
- [ ] SomaticFlashback once
- [ ] duty exclusion via canonical assignment
- [ ] typed exclusion reason
- [ ] mid-shift policy documented
- [ ] one throughput dimension
- [ ] work quality deferred unless real
- [ ] duty tags real
- [ ] contagion policy grounded
- [ ] medical duty policy grounded
- [ ] duty preview
- [ ] recovery refresh
- [ ] work tests

## 143B — Expedition

- [ ] canonical readiness reused
- [ ] hard blocks only if supported
- [ ] soft penalty only when eligible
- [ ] mobility affliction real
- [ ] severe radiation/respiratory rules grounded
- [ ] preview parity
- [ ] recovery updates preview
- [ ] no medical UI mutation
- [ ] expedition tests

## 143B — Combat

- [ ] Plan 137 combat slice reused
- [ ] no scattered direct affliction checks
- [ ] only real combat dimensions
- [ ] combat trauma not universal ban
- [ ] broken limb rules grounded
- [ ] fitness handles incapacitated
- [ ] AI parity
- [ ] combat tests
- [ ] no double needs/affliction

## 143C

- [ ] quest authority documented
- [ ] quest-side predicates preferred
- [ ] no medical-owned gate state
- [ ] survivor assignment vs global block distinguished
- [ ] mainline softlock prevention
- [ ] unlocks require real quest
- [ ] no dynamic quest invention
- [ ] quest facts stable
- [ ] multi-affliction predicates
- [ ] dialogue/branch modifiers canonical
- [ ] no reward override
- [ ] push-through only if choice/effect supports
- [ ] seek-help-first only if timing supports
- [ ] worsening through medical API
- [ ] source event ideas treated as content backlog
- [ ] existing medical quests audited
- [ ] 6–10 real scenarios
- [ ] quest IDs validated
- [ ] recovery unblocks
- [ ] UI refreshes
- [ ] journal bounded
- [ ] reachability
- [ ] mainline viability
- [ ] branch tests

## 143D

- [ ] medical panel effects
- [ ] duty roster effects
- [ ] quest panel reasons
- [ ] expedition reasons
- [ ] combat UI compact
- [ ] one tooltip projection
- [ ] event-driven refresh
- [ ] no per-frame scan
- [ ] recovery updates all consumers
- [ ] recovery trait deferred unless real
- [ ] moral-outlook deferred
- [ ] caregiving bond deferred
- [ ] journal semantic only
- [ ] tutorial only if framework exists
- [ ] old save no bridge state
- [ ] no CaptureState/RestoreState for derived projection
- [ ] quest persistence owned by quest
- [ ] mid-affliction save
- [ ] mid-recovery save
- [ ] determinism
- [ ] headless
- [ ] integrity
- [ ] selftest
- [ ] 30-day sim
- [ ] 120-day sim
- [ ] 180-day sim
- [ ] death-spiral guard
- [ ] configured work floor
- [ ] multi-affliction floor
- [ ] treatment value visible
- [ ] no universal treatment priority exploit
- [ ] performance benchmark
- [ ] allocation budget
- [ ] content acceptance
- [ ] reachability
- [ ] source-scan authority gate
- [ ] docs complete

---

# 33. Ship / No-Ship Gate

**SHIP** only if:

```text
medical_affliction_authorities == 1
AND affliction_capability_projections == 1
AND duplicate_final_work_multiplier_paths == 0
AND duplicate_final_expedition_multiplier_paths == 0
AND duplicate_final_combat_multiplier_paths == 0
AND derived_affliction_bridge_save_sections == 0
AND somatic_flashback_unconsumed_penalty == false
AND duty_exclusions_without_real_duty_tag == 0
AND quest_gates_owned_by_medical_bridge == 0
AND mainline_quest_medical_softlocks == 0
AND recovered_afflictions_leave_stale_blocks == 0
AND random_refusal_inside_projection == false
AND work_quality_system_invented_by_plan143 == false
AND treatment_recovery_improves_capability == true
AND affliction_old_save == pass
AND affliction_determinism == pass
AND affliction_headless == pass
AND affliction_quest_reachability == pass
AND affliction_balance_30_day == pass
AND affliction_balance_120_day == pass
AND affliction_balance_180_day == pass
AND affliction_bridges_selftest == pass
AND data_integrity_selftest == pass
AND content_acceptance_gate == pass
AND ui_access == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 34. Implementer Handoff

1. Re-read the real affliction catalog and severity semantics first.
2. Audit Plan 24 fitness and Plan 137 performance before creating any new bridge class.
3. Treat affliction effects as one contribution source, not a second final performance system.
4. Migrate the SomaticFlashback work penalty into that one composition path.
5. Keep hard duty exclusions separate from soft throughput penalties.
6. Use real duty tags and real isolation/infection-control policy.
7. Let expedition and combat consume the same canonical performance composition used elsewhere.
8. Prefer quest-side predicates over medical-owned quest maps.
9. Distinguish “quest unavailable” from “this survivor cannot be assigned.”
10. Never let a medical block silently softlock a critical quest.
11. Make medical quest unlocks authored and reference-validated.
12. Route quest modifications through canonical dialogue/choice predicates.
13. Do not invent work defects, side-effect chance, random refusals, recovery traits, mutations, or new social systems inside Plan 143.
14. Do not persist derived gates/modifiers.
15. Recovery should remove restrictions automatically by recomputation.
16. Show the same reasons in medical, duty, quest and expedition UI.
17. Add old-save, multi-affliction and recovery fixtures.
18. Run 30/120/180-day productivity/quest-viability simulations.
19. Close only when treatment has measurable strategic value without turning affliction into an unrecoverable shelter death spiral.

---

# 35. Final Outcome

When this plan is complete, an affliction stops being a medical label that lives only inside the health pipeline.

A survivor with a mobility injury can still exist in the same canonical medical system, but now the rest of ASHFALL understands what that state means. Fitness can decide whether the survivor is available at all. The affliction capability projection can say that heavy duty is unavailable and that ordinary work is slower. Plan 137 then composes that contribution with hunger, fatigue, skill and trauma through one final work-performance path.

Expeditions behave the same way. A severely impaired survivor can be blocked by the existing readiness rules; a moderately impaired one may still travel with a visible penalty. The dispatch preview and the actual expedition use the same calculation.

Combat also remains one system. Where a real combat-performance dimension exists, the affliction contribution enters the same performance pipeline rather than creating a medical combat rules engine.

Quests become medically aware without becoming medical-owned. A quest can require a fit survivor, unlock when a real affliction is present, or expose a different dialogue branch through canonical predicates. Recovery removes the block automatically because there is no duplicated bridge state to clear.

The player sees these consequences before making decisions. Medical detail explains what the affliction affects. Duty assignment shows why output is reduced or a role is unavailable. Expedition planning shows readiness. Quest UI explains why a route is delayed and what recovery path can restore it.

Treatment therefore gains strategic value for the right reason:

not because medicine awards a bonus,

but because restoring a person restores what that person can actually do.
