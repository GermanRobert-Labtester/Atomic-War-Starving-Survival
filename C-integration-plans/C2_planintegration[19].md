# C2 — Flagship Integration Plan [19]: Relationship Effects, Pair History, and the Proven Inner-Life Loop

> **Deliverable:** `C2_planintegration[19].md`
> **Source scope:** Plan 44 — *Relations That Change Outcomes: Affinity With Consequences*
> **Wave:** Continuity Wave 6 — *The People In It* (closing plan)
> **Primary objective:** turn affinity from a displayed-only statistic into one canonical relationship-effect input consumed by duty, expeditions, caregiving, training, and production; add bounded per-pair history so every band change is explainable; then prove the entire Wave‑6 inner-life chain end to end with one seeded journey, one liveness report, and permanent port-contract coverage.
> **Required execution order inside Plan 44:** **44A → 44B → 44C**
> **Wave dependency context:** 40A identity, 41A memory/grief, 42A/B/C voice, 43A/B/C governance, 24A/24B fitness/modifier channels, 20A location/exposure, 31A/31B semantic events and click-through, 36A port contracts.
> **Scope discipline:** no new relationship system, no romance loop, no affection meters, no direct consumer reads of raw affinity, no unbounded pair logs, no colour-only relation state, and no “connected” arrow accepted unless 44C asserts it in a live journey.

---

# 0. Executive Intent

ASHFALL already computes social state:

- ideological friction changes pair affinity,
- trauma bonds change pair affinity,
- survivor relations store pair state,
- the social coordinator already bridges roster → social,
- the relations panel already displays a read model.

But the simulation currently stops at presentation.

The missing half is:

```text
social state
→ actual work/travel/care/training consequences
```

This plan closes that gap.

The desired architecture is:

```text
SurvivorRelationsSystem
        │
        ▼
RelationEffect query
        │
        ├─ authored band
        ├─ working modifier
        ├─ risk modifier
        ├─ separation modifier
        └─ note/reason key
        │
        ├────────► DutyRoster
        ├────────► Expedition estimate/runtime
        ├────────► Caregiving/medical
        ├────────► Apprenticeship
        └────────► Production-with-labour
                         │
                         ▼
                    state effects
                         │
                         ▼
                 semantic attribution
                         │
                         ▼
                   bounded pair history
                         │
                         ▼
                relations / journal / voice
                         │
                         ▼
                one inner-life journey proof
```

The flagship outcome is:

> **Two survivors who hate each other no longer work exactly like two survivors who trust each other, and every resulting penalty or benefit can be explained by one query, one event history, and one player-visible reason.**

---

# 1. Source Diagnosis

The source establishes:

- affinity has multiple live writers,
- pair state is persisted/displayed,
- no external mechanic consumes affinity,
- the reverse direction (social → roster/expedition/etc.) is missing,
- multiple suitable consumer channels already exist,
- one relation-adjacent panel remains decorative,
- family/cohabitation/mentor relations already exist in other systems,
- the system needs integration, not redesign.

Architectural reading:

```text
relationship state already exists;
the missing architecture is a single consumer API.
```

---

# 2. Program-Level Success Criteria

C2[19] closes only if:

1. All mechanical consumers use one relationship-effect query.
2. No consumer reads raw affinity directly.
3. Relationship bands are data-authored and stable.
4. Duty assignment preview includes relation effect.
5. Expedition estimate/runtime includes relation effect.
6. Caregiving uses relation effect where appropriate.
7. Apprenticeship/training uses relation effect.
8. Separation of bonded pairs can create attributable pressure.
9. Relation effects never double-count fatigue/fitness/grief attribution.
10. Pair-driven rolls are deterministic.
11. Every meaningful band change has at least one history reason.
12. Pair histories are bounded.
13. Confession/grudge content can write pair events.
14. Bereavement transitions pair state into memory rather than dangling live affinity.
15. Voice can consume pair history.
16. A single seeded inner-life journey asserts every major arrow.
17. Every arrow is declared under the port contract.
18. Wave-6 documentation and implemented-canon registry reflect actual liveness.
19. 100/200-day reports show relation effects are observed in real runtime.
20. No assignment strategy becomes trivially dominant due to relationship bonuses.

---

# 3. Architectural Invariants

## 3.1 One relationship authority

`SurvivorRelationsSystem` remains the owner of pair state.

## 3.2 Consumers use derived effect, never raw affinity

Forbidden:

```text
relations.GetAffinity(a,b)
```

from duty/expedition/care/training consumers.

Required:

```text
relations.EffectOf(a,b)
```

or canonical equivalent.

## 3.3 Bands are authored

Thresholds are data/config, not scattered constants.

## 3.4 Relation effects travel through existing channels

- productivity/morale → Plan 24B modifier stack,
- readiness warnings → Plan 24A,
- expedition risk → existing estimate/runtime,
- caregiving → existing medical/care systems,
- production → Plan 35C labor/yield channel.

## 3.5 Relation effects are attributable

Every effect carries:

- band,
- cause category,
- note key,
- contributing history where appropriate.

## 3.6 History is bounded

No unbounded list per pair.

## 3.7 Bereavement changes representation

A dead partner is not treated as a still-live pair interaction.

## 3.8 Voice reads state/history

Voice may narrate/reflect a relationship.
It does not own it.

## 3.9 No new romance game loop

Parallel content plans may add romance/family narratives later.
This plan only provides shared mechanical relation consumption.

## 3.10 Every cross-system arrow is testable

If it is not asserted in 44C, it is not considered closed.

---

# 4. Dependency Graph

```text
40A identity
   │
41A memory/grief
   │
42A/B/C voice
   │
43A/B/C governance
   │
   └──────────────┐
                  ▼
            44A relation effects
                  │
                  ▼
            44B pair history
                  │
                  ▼
            44C journey + proof

24A/24B ───────► work/fitness/modifier channels
20A ───────────► expedition/location context
31A/31B ───────► semantic attribution + click-through
36A ───────────► declared seams
```

---

# 5. Baseline Capture

Before editing, record:

- total relationship pairs in representative save,
- affinity range,
- current friction constants,
- trauma-bond bonus,
- number of external raw-affinity readers,
- current duty preview fields,
- current expedition estimate fields,
- caregiving/mentor assignment APIs,
- current relation read-model shape,
- current pair-history availability,
- current relation panel route status,
- TraumaBondingCohortPanel liveness status.

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
python3 scripts/ci/generate-port-contract.py --check
godot --headless --path . -- --survivors-selftest
godot --headless --path . -- --expedition-selftest
bash scripts/ci/triad-drift-gate.sh
bash scripts/ci/verify-fast.sh
```

Capture duty/expedition/relations snapshots for a neutral pair and a hostile/bonded pair.

---

# 6. Workstream 44A — Read Affinity Where Work Is Decided

## Goal

Make pair state mechanically affect the two most frequent composition choices: shifts and expedition parties.

---

# 7. 44A Phase A — Define the Relation Effect Query

Add one canonical query.

Conceptual result:

```csharp
RelationEffect
{
    BandId,
    WorkingModifier,
    MoraleModifier,
    ErrorRiskModifier,
    ExpeditionRiskModifier,
    SeparationModifier,
    CaregivingModifier,
    TrainingModifier,
    NoteKey
}
```

Do not expose more fields than consumers need.

---

# 8. 44A Phase B — Relation Bands

Author bands in data/config.

Suggested source-inspired labels:

```text
hostile
strained
cordial
close
bonded
```

Use project-approved names/keys.

Each band defines:

- affinity min/max,
- working modifier,
- risk modifier,
- separation pressure,
- optional caregiving/training modifiers,
- note key.

---

# 9. 44A Phase C — No Bare Float Presentation

UI should show:

- band,
- reason,
- effect summary.

Do not surface raw affinity number as primary player-facing information.

Raw values may remain in diagnostics.

---

# 10. 44A Phase D — Raw-Affinity Source Gate

Add a source scan/architecture test.

Allow direct raw affinity only inside:

- relations authority,
- friction/bond writers,
- tests/diagnostics.

Fail external mechanical consumer reads.

---

# 11. 44A Phase E — Duty Roster Integration

When candidate co-assignment includes pairs:

```text
assignment
→ gather relevant pair effects
→ aggregate through documented rule
→ preview
→ apply through existing modifier/error channels
```

Do not create a parallel relation-only duty outcome engine.

---

# 12. 44A Phase F — Duty Preview

Before confirm, show:

- relevant pair band,
- productivity/morale/error consequence,
- note/reason.

Use 24A’s warn-don’t-block design.

Example:

```text
Strained pairing — higher mistake risk.
```

localized via key.

---

# 13. 44A Phase G — Pair Aggregation Rule

For teams larger than two, define deterministic aggregation.

Options:

- worst pair dominates certain risks,
- mean/weighted sum,
- capped additive modifiers.

Choose explicitly and balance-test.

Do not let O(n²) pair effects explode large crews.

---

# 14. 44A Phase H — Double-Counting Guard

Attribution must separate:

```text
fatigue
fitness
relationship strain
grief
environment
```

One outcome may have multiple contributors, but each effect term is distinct.

No same penalty applied twice through different stacks.

---

# 15. 44A Phase I — Expedition Composition Integration

Expedition party validation/estimate queries pair effects.

Inputs may alter:

- panic/failure risk,
- coordination term,
- encounter-handling risk,
- morale stress.

Do not block party formation solely because a pair is hostile unless another system independently makes it illegal.

---

# 16. 44A Phase J — Estimate Breakdown

Add relationship term to canonical expedition estimate.

Example:

```text
Base risk
+ terrain
+ weather
+ faction control
+ survivor readiness
+ relationship composition
= projected risk
```

UI reads the same calculation runtime uses.

---

# 17. 44A Phase K — Bereaved Party Context

Where grief state identifies a lost partner/close relation:

- relation query can expose bereavement context,
- expedition estimate may reflect stress/trigger risk through existing grief/fitness channels.

Avoid a second grief penalty if Plan 41A already applies one.

---

# 18. 44A Phase L — Separation Pressure

For bonded/close pairs:

```text
long expedition separation
or
repeated split shifts
```

may create morale/fatigue pressure.

Requirements:

- authored rate,
- deterministic,
- semantic event,
- visible attribution.

No hidden timer.

---

# 19. 44A Phase M — Caregiving Integration

Caregiving/staff assignment queries pair effect.

Potential outcomes:

- trusted caregiver improves cooperation/recovery,
- hostile caregiver worsens stress/compliance.

MedicalWard/Caregiving remains authority for recovery.

---

# 20. 44A Phase N — Apprenticeship Integration

Mentor/apprentice pairing reads:

- relation band,
- belief compatibility where 40A provides it,
- profession/skill prerequisites.

Training system owns skill transfer.

Relation effect modifies quality, not eligibility unless explicitly designed.

---

# 21. 44A Phase O — Production Integration

If Plan 35C production-with-labour is live:

```text
crew composition
→ RelationEffect aggregate
→ existing labour/yield modifier
```

No production-specific affinity formula.

---

# 22. 44A Phase P — TraumaBondingCohortPanel Decision

Choose one:

### Make live

- bind coordinator/read model,
- show bonded cohort reasoning,
- expose one existing meaningful action.

### Shelve/retire

- mark non-player-facing,
- remove false affordance.

Do not leave decorative.

---

# 23. 44A Phase Q — Semantic Events

Emit/report meaningful relation-driven outcomes.

Potential:

- relation_work_modifier_applied,
- bonded_pair_separated,
- relation_risk_modifier_applied.

Use actual Plan 31 taxonomy or semantic categories.

Avoid event spam for every minor internal calculation.

---

# 24. 44A Phase R — Deterministic Pair Rolls

Any mistake/panic roll affected by pair state:

- uses `ISeededRng`,
- stable stream IDs,
- stable pair ordering.

Canonical pair key:

```text
min(survivorA, survivorB) + max(...)
```

or equivalent.

---

# 25. 44A Phase S — Negative-Control Tests

For each relation consumer, prove unrelated systems do not change.

Examples:

```text
hostile duty pair
→ duty outcome changes
→ unrelated market price unchanged
```

This prevents accidental broad coupling.

---

# 26. 44A Phase T — 100-Day Assignment Replay

Use fixed assignment policy.

Run twice.

Assert digest identity for:

- relation bands,
- duty outcomes,
- expedition outcomes,
- modifier events.

---

# 27. 44A Docs

Create:

```text
docs/systems/RELATION_EFFECTS.md
```

Table:

| Band | Duty | Expedition | Caregiving | Training | Separation | UI note |
|---|---|---|---|---|---|---|

---

# 28. 44A Definition of Done

- [ ] one RelationEffect query,
- [ ] authored bands,
- [ ] no external raw-affinity reads,
- [ ] duty preview integration,
- [ ] deterministic team aggregation,
- [ ] double-counting guarded,
- [ ] expedition estimate/runtime term,
- [ ] separation pressure,
- [ ] caregiving integration,
- [ ] apprenticeship integration,
- [ ] production integration where applicable,
- [ ] trauma-bond panel disposition,
- [ ] semantic attribution,
- [ ] deterministic pair rolls,
- [ ] negative-control tests,
- [ ] 100-day replay deterministic,
- [ ] RELATION_EFFECTS.md.

---

# 29. Workstream 44B — Relations as Memory and Story

## Goal

Give each pair a bounded history so relation state is explainable, persistent, and reusable by narrative systems.

---

# 30. 44B Phase A — Pair History DTO

Create a small entry shape:

```text
PairRelationHistoryEntry
{
    event_id
    day
    cause_id
    kind
    delta
    resulting_band?
    source_owner
    note_key?
}
```

No presentation prose.

---

# 31. 44B Phase B — Canonical Pair Key

Store history under canonical ordered pair identity.

Avoid duplicated:

```text
A→B
B→A
```

records unless directional semantics genuinely require separate entries.

---

# 32. 44B Phase C — History Write Rule

Every meaningful affinity/band change writes a reason entry.

Potential sources:

- shared shift,
- trauma bond,
- ideological friction,
- ration conflict,
- caregiving,
- saved life,
- refused request,
- funeral/memorial,
- confession,
- policy consequence.

---

# 33. 44B Phase D — Explainability Invariant

Hard rule:

```text
band change
→ at least one traceable history cause
```

Add test.

Silent drift without history is a defect.

---

# 34. 44B Phase E — Bounded Retention

Define:

```text
N recent entries per pair
```

or a byte budget.

Older events are:

- summarized into standing record,
- aggregated by type,
- or archived through existing narrative record.

Coordinate Plan 39B retention.

---

# 35. 44B Phase F — Summary Rollup

When pruning:

- preserve net historical contribution,
- preserve landmark events,
- preserve last band transition.

Do not change current affinity merely because detailed history is capped.

---

# 36. 44B Phase G — Relations Panel “Why”

Panel shows top contributors:

```text
+ trauma bond
- ration dispute
- ideological friction
```

with day/source.

Click-through via Plan 31B where route exists.

---

# 37. 44B Phase H — `confession_secrets.json`

Wire as pair-event source.

Flow:

```text
secret disclosed
→ choice/outcome
→ forgiveness/grudge
→ relation delta
→ history entry
→ semantic event
```

No separate “secret relationship” system.

---

# 38. 44B Phase I — Confession Validation

Every confession definition validates:

- ids,
- outcome references,
- relation effect,
- localization keys,
- required trigger/context.

Content-utilization should move catalog to live/effect-produced.

---

# 39. 44B Phase J — Wall Carving Templates

Integrate `wall_carving_templates` through memorial/mourning/relationship context where appropriate.

They are artifacts of pair/family/mourning state, not independent mechanics.

---

# 40. 44B Phase K — Memorial Link

If survivor A mourns B:

- memorial event can write pair history/memory,
- surviving relation becomes bereavement/memory state,
- memorial UI may surface relationship significance.

---

# 41. 44B Phase L — Bereavement Transition

On death:

```text
live pair relation
→ surviving-person bereavement/memory relation
```

Do not keep dead survivor as active work/party pair.

Preserve history for:

- grief,
- memorial,
- voice,
- legacy/ending.

---

# 42. 44B Phase M — Family Mechanics

Use existing parent/child relationships.

Mechanical effects may include:

- prioritization,
- mourning weight,
- caregiving,
- duty/exposure considerations.

Do not design romance/family content loops here.

---

# 43. 44B Phase N — Voice Integration

42C can query pair history.

Voice eligibility may use:

- latest grievance,
- bond origin,
- bereavement,
- confession.

Voice never mutates relation history.

---

# 44. 44B Phase O — Standing Record Integration

Older bounded history rolls into the standing record where appropriate.

Do not invent another archival store.

---

# 45. 44B Phase P — Save Contract

Persist:

- current pair state,
- bounded recent history,
- required rollup/summary,
- bereavement state.

Round-trip and deterministic ordering required.

---

# 46. 44B Phase Q — Deterministic Event Ordering

If multiple relation events occur same day:

```text
phase
→ cause
→ canonical pair key
→ stable event id
```

or equivalent.

Pin deterministic serialization.

---

# 47. 44B Tests

- history write/read,
- explainability invariant,
- retention cap,
- summary rollup,
- relation-panel top reasons,
- confession→band change,
- wall carving/memorial artifact,
- bereavement transition,
- family mechanics handoff,
- voice history query,
- save round-trip,
- deterministic ordering.

---

# 48. 44B Definition of Done

- [ ] pair history DTO,
- [ ] canonical pair key,
- [ ] every band change gets reason,
- [ ] bounded retention,
- [ ] rollup preserves meaning,
- [ ] relations “why” UI,
- [ ] confession catalog live,
- [ ] carving/memorial integration,
- [ ] bereavement transition,
- [ ] family mechanical handoff,
- [ ] voice reads history,
- [ ] standing-record integration,
- [ ] save round-trip,
- [ ] deterministic history ordering.

---

# 49. Workstream 44C — Prove the Inner-Life Loop

## Goal

Turn the entire Wave‑6 social/identity stack into one asserted end-to-end journey, one liveness report, and one documentation closure.

---

# 50. 44C Phase A — Define the Acceptance Chain

Source chain:

```text
authored belief
→ friction drift
→ policy adopted
→ grievance voiced
→ death/grief
→ band change with history
→ duty/expedition outcome changed
→ ending input recorded
→ legacy/chronicle line
```

Where downstream plan numbers differ in current branch, bind to actual canonical systems.

---

# 51. 44C Phase B — `InnerLifeJourneySelfTest`

Create:

```text
src/Host/InnerLifeJourneySelfTest.cs
```

Expose a headless CLI verb.

Requirements:

- fixed seed,
- deterministic scripted sequence,
- no manual intervention,
- explicit assertion per arrow,
- machine-readable summary.

---

# 52. 44C Phase C — Journey Fixture

Construct a representative small cast.

Need at least:

- two conflicting belief profiles,
- one bonded/close pair,
- one leader/policy-capable survivor,
- one death/grief path,
- one duty/expedition assignment.

Use authored IDs.

No invented test-only personality truth that bypasses catalogs.

---

# 53. 44C Phase D — Assertion: Identity

Assert:

```text
authored belief loaded
```

not inferred.

---

# 54. 44C Phase E — Assertion: Friction

Assert:

```text
same relevant context
→ ideological friction
→ affinity/band change
```

with semantic event/history.

---

# 55. 44C Phase F — Assertion: Governance

Apply/observe a real policy decision through Plan 43.

Assert state changes and `causeId` exist.

---

# 56. 44C Phase G — Assertion: Voice

The affected survivor emits an eligible line through 42C.

Assert:

- correct subject type,
- causeId,
- knowledge eligibility,
- delivery sink,
- journal archive.

---

# 57. 44C Phase H — Assertion: Grief

Trigger a survivor death.

Assert:

- grief applied through real seam,
- relation memory/bereavement transition,
- semantic record.

---

# 58. 44C Phase I — Assertion: Pair History

Assert the resulting band change has a traceable pair-history entry.

---

# 59. 44C Phase J — Assertion: Mechanical Outcome

Use duty and/or expedition composition.

Assert relation effect changes preview/runtime outcome.

No direct raw-affinity injection.

---

# 60. 44C Phase K — Assertion: Ending/Legacy Input

Where current ending/chronicle architecture consumes social facts, assert the relevant record reaches that canonical sink.

If current plan numbering differs from source text, use actual implemented owner.

Do not add a new ending summary.

---

# 61. 44C Phase L — Port Contract Coverage

Every cross-system arrow gets a declared seam.

Port-contract report must include:

- identity registration,
- relation query consumer,
- voice delivery,
- grief sink,
- policy event,
- ending/record handoff.

Missing arrow → CI failure.

---

# 62. 44C Phase M — Runtime Liveness Report

Produce:

```text
artifacts/inner-life-report.json
```

For each arrow:

```text
declared?
bound?
executed?
observed in 100-day soak?
player-visible?
```

This is the closure evidence.

---

# 63. 44C Phase N — 100-Day Liveness Soak

Run representative autonomous/scripted campaign.

Track:

- friction events,
- relation band changes,
- voice lines,
- grievance events,
- grief events,
- mechanical relation modifiers,
- history writes.

No arrow may remain test-only.

---

# 64. 44C Phase O — 200-Day Balance Soak

With relation mechanics fully active, measure strategy dominance.

Compare policies:

- always bonded pairs,
- mixed pairs,
- rotate teams,
- minimize hostile pairs,
- random viable assignment.

No single relation policy should dominate every other objective.

---

# 65. 44C Phase P — Tune Existing Bands First

If “always pair bonded survivors” dominates:

- reduce relation modifier magnitude,
- increase opportunity cost,
- adjust separation pressure.

Do not add counter-mechanics just to offset overtuning.

---

# 66. 44C Phase Q — Documentation Truth Pass

Update:

- implemented-canon registry,
- atlas high-leverage/underconnected classifications,
- known issues,
- system docs,
- Wave‑6 ledger/index.

Every claim must now distinguish:

```text
implemented
bound
runtime-observed
player-visible
```

---

# 67. 44C Phase R — AGENTS Rules

Add explicit permanent rules:

```text
authored identity, not inferred
tags/properties, not generic ID lists
relationship consumers use EffectOf, not raw affinity
```

Retire invalid known-issue entries.

---

# 68. 44C Phase S — Parallel-Plan Coordination

Update roadmap notes for plans 132/144/147/148/150/154/159.

They must use:

- canonical identity,
- pair history,
- RelationEffect,
- voice channel,
- semantic event layer.

No parallel memory/affinity/voice frameworks.

---

# 69. 44C Phase T — Accessibility

All relation information has:

- text,
- icon/shape,
- keyboard navigation,
- localized labels.

No colour-only band meaning.

---

# 70. 44C Phase U — Snapshot Set

Capture representative states for:

- relations,
- duty roster,
- expedition dispatch,
- memorial,
- caregiving.

Include hostile/bonded/bereaved cases.

---

# 71. 44C Phase V — Release-Gate Integration

Run Wave‑5 release gate after inner-life journey is added.

Register journey as appropriate fast/nightly gate.

Expected summary must be mandatory.

---

# 72. 44C Definition of Done

- [ ] deterministic inner-life journey verb,
- [ ] one assertion per causal arrow,
- [ ] authored identity proven,
- [ ] friction proven,
- [ ] governance cause proven,
- [ ] grievance voice proven,
- [ ] grief proven,
- [ ] pair history proven,
- [ ] duty/expedition effect proven,
- [ ] ending/legacy handoff proven,
- [ ] port contract covers every arrow,
- [ ] runtime liveness report,
- [ ] 100-day observed coverage,
- [ ] 200-day balance sanity,
- [ ] docs corrected,
- [ ] AGENTS rules updated,
- [ ] parallel-plan coordination updated,
- [ ] accessibility pass,
- [ ] representative snapshots,
- [ ] release gate green.

---

# 73. Integrated Relationship Pipeline

```text
Authored survivor identity
        │
        ▼
friction / bond / grievance / care events
        │
        ▼
SurvivorRelationsSystem
        │
        ├─ current affinity
        ├─ band
        └─ bounded history
        │
        ▼
RelationEffect
        │
        ├─ duty
        ├─ expedition
        ├─ care
        ├─ training
        └─ production
        │
        ▼
semantic outcome events
        │
        ├─ briefing
        ├─ journal
        ├─ voice
        └─ pair-history reasons
```

---

# 74. Raw Affinity Boundary

Raw affinity is an internal state value.

Allowed readers:

- relation authority,
- ideological friction,
- trauma bond,
- diagnostics/tests.

External consumer:

```text
must use RelationEffect
```

This prevents threshold/formula drift.

---

# 75. Relation Band Contract

Each band definition must specify:

```text
id
min_affinity
max_affinity
working_modifier
risk_modifier
separation_modifier
caregiving_modifier
training_modifier
note_key
```

Not every modifier has to be non-neutral.

---

# 76. Aggregation Contract

For groups:

```text
members
→ canonical pair list
→ EffectOf each pair
→ deterministic aggregate
```

Document aggregation by channel.

Duty may use a different aggregation than expedition risk if justified, but both consume the same pair effects.

---

# 77. Attribution Contract

Every relation-derived mechanical effect should answer:

```text
Which pair?
Which band?
Which history/cause?
Which channel?
What magnitude?
```

This supports briefing/debug/UI.

---

# 78. History Retention Contract

Every pair has:

```text
recent bounded entries
+ optional summarized standing record
```

No unbounded per-pair log.

---

# 79. Bereavement Contract

When one survivor dies:

- active pair mechanical participation ends,
- surviving survivor gains bereavement/memory relation,
- history remains,
- grief/memorial/voice can read it.

No dangling live work-effect pair to a dead survivor.

---

# 80. Voice Contract

Voice can read:

- current band,
- known history,
- grievance,
- bereavement.

Voice cannot:

- alter band,
- create grievance,
- invent refusal.

---

# 81. Save Contract

Persist:

- pair state,
- bounded pair history,
- bereavement/memory state,
- any required summary.

Deterministic ordering by canonical pair key.

---

# 82. Port Contract Alignment

Every relation-effect consumer is a declared seam.

Healthy port report:

```text
RELATION_EFFECT_MISSING_CONSUMERS=0
```

or equivalent.

---

# 83. Content Utilization Contract

Revived catalogs such as:

- confession secrets,
- wall carvings

must become:

```text
loaded
→ event produced
→ state changed
→ player observed
```

not merely referenced.

---

# 84. Balance Contract

Relations should create tradeoffs, not one optimal roster strategy.

Bound magnitudes.

Examples:

- bonded pairs help coordination,
- separating them costs morale,
- concentrating all bonded pairs may weaken other shifts,
- hostile pairs are risky but not always unusable.

---

# 85. Failure Modes

## Consumer reads raw affinity

Fail query-only test/source gate.

## Band thresholds scattered

Move to one authored table.

## Duty preview disagrees with runtime

Use same EffectOf + aggregation.

## Expedition risk double-counts grief

Separate attribution channels.

## Band changes without history

Fail explainability invariant.

## Pair history grows forever

Enforce retention.

## Death leaves active pair mechanics

Transition to bereavement state.

## Voice creates a relation consequence

Move consequence into relations/governance state first.

## TraumaBondingCohortPanel remains decorative

Bind or shelve.

## Journey passes via test-only seam

Require runtime observed evidence.

## 200-day soak makes bonded pairing always optimal

Tune bands, not add counter-system.

---

# 86. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| assignment balance shifts too far | Medium | High | 200-day strategy sweep |
| pair aggregation over-amplifies | Medium | High | capped authored aggregation |
| raw-affinity drift | Medium | Medium | source gate |
| history save growth | High | Medium | bounded retention |
| grief double-counting | Medium | High | attribution matrix |
| relation UI becomes stat sheet | Medium | Medium | bands/reasons only |
| parallel plans duplicate relationship state | Medium | High | roadmap coordination |
| journey becomes fixture-only | Medium | High | runtime liveness report |
| revived catalogs still no-op | Medium | Medium | effect-produced utilization |
| dead-survivor pair references linger | Medium | High | bereavement transition tests |

---

# 87. Commit Strategy

## C2[19].1 — baseline + RELATION_EFFECTS ADR

## C2[19].2 — RelationEffect query + band data

## C2[19].3 — raw-affinity consumer gate

## C2[19].4 — duty preview/runtime integration

## C2[19].5 — expedition estimate/runtime integration

## C2[19].6 — separation/caregiving/training integration

## C2[19].7 — production + trauma-bond panel disposition

## C2[19].8 — attribution/determinism/negative controls

### Gate: 44A complete

## C2[19].9 — pair history model + bounded retention

## C2[19].10 — explainability invariant + relations UI

## C2[19].11 — confession secrets integration

## C2[19].12 — wall carvings/memorial integration

## C2[19].13 — bereavement/family handoffs

## C2[19].14 — voice/standing-record/save integration

### Gate: 44B complete

## C2[19].15 — InnerLifeJourneySelfTest

## C2[19].16 — port-contract arrow declarations

## C2[19].17 — inner-life report + 100-day liveness

## C2[19].18 — 200-day balance sweep

## C2[19].19 — docs/AGENTS/roadmap truth pass

## C2[19].20 — accessibility/snapshots/release gate

### Gate: 44C complete

## C2[19].21 — Wave‑6 closure

---

# 88. Verification Checklist

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
python3 scripts/ci/generate-port-contract.py --check
godot --headless --path . -- --survivors-selftest
godot --headless --path . -- --expedition-selftest
bash scripts/ci/triad-drift-gate.sh
bash scripts/ci/verify-fast.sh
```

Also run repository-canonical equivalents of:

```text
inner-life journey selftest
100-day inner-life liveness soak
200-day relation-balance soak
ashfall-narrative-check
ashfall-narrative-continuity
ashfall-dialog-graph-lint
release gate
snapshot diff
```

---

# 89. Flagship Definition of Done

## 44A — Mechanical Consumers

- [ ] one RelationEffect API,
- [ ] authored relation bands,
- [ ] no external raw-affinity reads,
- [ ] duty preview/runtime consumes relation effects,
- [ ] expedition estimate/runtime consumes relation effects,
- [ ] separation pressure,
- [ ] caregiving integration,
- [ ] apprenticeship integration,
- [ ] production integration where available,
- [ ] trauma-bond panel decision,
- [ ] attribution separates fatigue/grief/relations,
- [ ] deterministic pair rolls,
- [ ] negative controls,
- [ ] 100-day replay deterministic.

## 44B — Pair History

- [ ] canonical pair-history model,
- [ ] every band change explainable,
- [ ] bounded retention,
- [ ] standing-record rollup,
- [ ] relations “why” surface,
- [ ] confession secrets live,
- [ ] wall carvings/memorial live,
- [ ] bereavement transition,
- [ ] family mechanical handoff,
- [ ] voice reads history,
- [ ] save round-trip,
- [ ] deterministic ordering.

## 44C — End-to-End Proof

- [ ] inner-life journey verb,
- [ ] identity arrow asserted,
- [ ] friction arrow asserted,
- [ ] governance arrow asserted,
- [ ] voice arrow asserted,
- [ ] grief arrow asserted,
- [ ] pair-history arrow asserted,
- [ ] duty/expedition outcome arrow asserted,
- [ ] ending/legacy handoff asserted,
- [ ] every arrow in port contract,
- [ ] runtime liveness report,
- [ ] no test-only arrows,
- [ ] 100-day coverage,
- [ ] 200-day balance sanity,
- [ ] docs/registry updated,
- [ ] AGENTS rules updated,
- [ ] parallel-plan coordination published,
- [ ] accessibility/snapshots,
- [ ] release gate green.

## Global

- [ ] no new relationship system,
- [ ] no affection meter,
- [ ] no romance loop,
- [ ] no unbounded pair logs,
- [ ] no colour-only bands,
- [ ] no raw affinity reads outside authority,
- [ ] no unevidenced causal arrow,
- [ ] full verification green.

---

# 90. Closure Report Template

```markdown
## C2[19] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- Relation pairs:
- Raw-affinity external readers:
- Relation bands:
- Duty relation terms:
- Expedition relation terms:
- Pair history entries:
- Live relation panels:
- Port seams:

### 44A — Effects
- RelationEffect API:
- Band data:
- Raw-affinity gate:
- Duty preview:
- Duty runtime:
- Expedition estimate:
- Expedition runtime:
- Separation:
- Caregiving:
- Apprenticeship:
- Production:
- Trauma-bond panel:
- Attribution:
- Determinism:
- Result:

### 44B — History
- History model:
- Retention:
- Explainability failures:
- Confession events:
- Carving/memorial events:
- Bereavement:
- Family handoff:
- Voice history reads:
- Save round-trip:
- Result:

### 44C — Inner-Life Proof
- Journey seed:
- Identity arrow:
- Friction arrow:
- Governance arrow:
- Voice arrow:
- Grief arrow:
- Pair-history arrow:
- Duty/expedition arrow:
- Ending/legacy arrow:
- Port-contract missing arrows:
- 100-day observed arrows:
- 200-day balance:
- Docs:
- Accessibility:
- Release gate:
- Result:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Bridge:
- Port contract:
- Survivors selftest:
- Expedition selftest:
- Triad gate:
- Inner-life journey:
- Narrative checks:
- Verify fast:
- Release gate:

### Final Metrics
- RAW_AFFINITY_EXTERNAL_READERS:
- RELATION_EFFECT_CONSUMERS:
- BAND_CHANGES_WITHOUT_HISTORY:
- PAIR_HISTORY_MAX_LENGTH:
- INNER_LIFE_ARROWS_DECLARED:
- INNER_LIFE_ARROWS_BOUND:
- INNER_LIFE_ARROWS_RUNTIME_OBSERVED:
- INNER_LIFE_ARROWS_PLAYER_VISIBLE:
- DOMINANT_ASSIGNMENT_STRATEGY_DELTA:

### Remaining Debt
- Relations:
- History:
- Family:
- Voice:
- Governance:
- Balance:
```

---

# 91. Final Execution Directive

Execute Plan 44 as the Wave‑6 closure that makes relations consequential and provable.

The critical sequence is:

```text
derive one relation-effect query
→ feed existing work/travel/care/training channels
→ show those effects before confirmation
→ record every meaningful pair change with a bounded reason history
→ transition death into bereavement/memory
→ let voice/journal consume that history
→ build one seeded inner-life journey
→ assert every cross-system arrow
→ publish liveness evidence
```

Do not redesign affinity.

Do not expose raw affinity directly to consumers.

Do not create another relationship ledger.

Do not let pair history grow without limit.

Do not call a cross-system loop “integrated” because each class exists separately.

The strongest mechanical rule is:

> **Every consumer gets relationship meaning through one `RelationEffect` query, never through raw affinity math.**

The strongest explainability rule is:

> **Every meaningful band change has a bounded, persistent, player-discoverable reason.**

The strongest closure rule is:

> **Every arrow in the inner-life loop must be declared, bound, runtime-observed, and asserted by one seeded journey before Wave 6 is considered complete.**

The flagship acceptance scenario is:

> **Start with authored conflicting beliefs, let friction change the pair, enact a policy that creates a grievance, hear the grievance, kill one member of a meaningful pair, observe grief/bereavement history, then assign the survivor to duty or an expedition and verify the relation state changes the actual preview/runtime outcome — all in one deterministic journey with a traceable cause chain.**
