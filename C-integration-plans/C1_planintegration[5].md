# C1 — Flagship Integration Plan [5]: One Survivor Ledger — Fitness, Fatigue, Skill, Duty, Illness & Recovery

> **Output:** `C1_planintegration[5].md`
>
> **Source baseline:** Plan 24 — People Are Not Abstraction: Health, Fatigue, Skill, and Duty Are One Ledger
>
> **Wave:** Continuity Wave 2 — *The Bunker Machine* (closing plan)
>
> **Dependencies:** Plan 22 (meals and medicine payload), Plan 23 (light/heat/sleep loads), Plan 20 (dose/radiation cost), and Plan 16B authority repair before the skill/labour integration in 24B.
>
> **Mandatory execution order:** 24A → 24B → 24C.
>
> **Wave-level purpose:** make the survivor's body, schedule, skill, duty, illness, care, and death read as one continuous state machine instead of disconnected panels and systems.
>
> **Guardrails:** no new need kinds, no second morale track, no second stress model, no per-system skill setter proliferation, no scripted tragedy cascade that bypasses state, no UI-only fitness logic.

---

# 0. Mission

ASHFALL already models most of the variables that should make a survivor feel like a person rather than a row in a roster:

- hunger;
- thirst;
- fatigue;
- warmth;
- morale;
- health;
- hygiene;
- radiation dose;
- acute radiation syndrome state;
- afflictions;
- quarantine;
- sick-list severity;
- dependency and withdrawal;
- trauma;
- sleep assignment;
- last slept day;
- duty assignment;
- apprenticeship;
- skill progression and skill atrophy;
- trapping skill requirements;
- medical ward admission and discharge;
- caregiving;
- survivor fate;
- memorial outcome.

The integration failure is that these authorities rarely ask each other anything.

The current roster can reportedly assign a starving, exhausted, irradiated, quarantined survivor to a shift because duty validation has no condition input. A detailed shelter schedule can calculate a fatigue recovery modifier that only appears as a label. Trapping owns a skill multiplier and minimum skill gates that nobody feeds. Apprenticeship creates a second skill system instead of using campaign skill state. Ward admission, quarantine, discharge, death, rationing, morale, and duty remain local facts.

Plan 24 makes these systems share one survivor ledger without creating one giant survivor god-object.

The required integration shape is:

```text
                   ┌───────────────────────────┐
                   │    PERSISTED AUTHORITIES  │
                   │ needs / dose / disease    │
                   │ afflictions / sick list   │
                   │ dependency / trauma       │
                   │ sleep / skills / duties   │
                   └────────────┬──────────────┘
                                │
                                ▼
                 ┌─────────────────────────────┐
                 │ FITNESS FOR DUTY PROJECTION │
                 │ Fit / Impaired / Unfit /    │
                 │ Incapacitated + reasons     │
                 └────────────┬────────────────┘
                              │
              ┌───────────────┼─────────────────────┐
              ▼               ▼                     ▼
         DUTY ROSTER      EXPEDITIONS          COOK / WARD STAFF
              │               │                     │
              └───────────────┼─────────────────────┘
                              ▼
                     WORK / LOAD / CONSEQUENCE
                              │
                              ▼
                 ┌─────────────────────────────┐
                 │     NEEDS MODIFIER STACK    │
                 │ sleep / meal / cold / grief│
                 │ overwork / friction / care │
                 └────────────┬────────────────┘
                              │
                              ▼
                    SAME SURVIVOR NEED BARS
                              │
                              ▼
                 FITNESS RECALCULATED TOMORROW
```

The human lifecycle becomes:

```text
FIT
 ↓
IMPAIRED
 ↓
SICK / INJURED / QUARANTINED
 ↓
WARD / CARE / LIGHT DUTY
 ↓                    ↘
RECOVERY               DEATH
 ↓                      ↓
DUTY RETURN         SHIFT RELEASE
                       ↓
                 RATION RECALCULATION
                       ↓
                    GRIEF
                       ↓
                   MEMORIAL
```

---

# 1. Source-Evidence Interpretation

The supplied Plan 24 identifies three different continuity defects.

## 1.1 Duty validation does not know survivor condition

The existing assignment engine reportedly validates role existence, survivor row existence, generic assignability, and duplicate assignment—but has no dependency on needs, disease, radiation, afflictions, quarantine, or fatigue.

Therefore 24A must establish one condition projection before any downstream UI work.

## 1.2 Needs are internally complete but externally closed

`NeedsSystem` already owns the bars the player watches, yet external systems cannot reliably explain or modify those values. A schedule may compute sleep quality; cold may exist; meals may exist; grief may exist; politics may exist; none have a common attributable input seam.

Therefore 24B must add one deterministic modifier mechanism rather than individual bespoke hooks.

## 1.3 Patient and labour journeys are disconnected

Medical ward, sick list, disease, caregiving, fate, memorial, rationing, and roster all exist, but transitions do not reorganize labour or resource allocation.

Therefore 24C must treat admission, quarantine, discharge, recovery, and death as state transitions consumed by other systems.

---

# 2. Non-Negotiable Architectural Invariants

## INV-24.1 — One fitness verdict per survivor state

Duty roster, expedition party selection, cooking labour, ward staffing, and any other "can this survivor work?" consumer must use the same Core projection.

No panel may implement its own fitness rules.

## INV-24.2 — Fitness is derived, not persisted

Persist the authorities that determine fitness.

Recompute `FitnessVerdict` after load.

Do not serialize the verdict as a second source of truth.

## INV-24.3 — Warn vs block is deliberate

The game must preserve difficult choices.

- `Fit`: normal assignment.
- `Impaired`: assignable with explicit warning/cost.
- `Unfit`: normally blocked for demanding roles but may be eligible for explicitly authored light-duty roles.
- `Incapacitated`: cannot be assigned.

Exact level semantics must be documented and tested.

## INV-24.4 — Role requirements are authored data

Dose ceilings, fatigue ceilings, required skills, minimum health, maximum hours, and other role-specific conditions belong in role data.

Do not hardcode role names in `ValidateAssign`.

## INV-24.5 — Needs have one modifier stack

External systems contribute to existing need kinds.

Do not add parallel fatigue, morale, stress, hygiene, or warmth tracks.

## INV-24.6 — Modifier attribution is mandatory

Every external needs contribution must be explainable by a stable source ID.

A hidden modifier is considered incomplete integration.

## INV-24.7 — Modifier order is deterministic

Aggregation order must be stable, documented, and replay-safe.

## INV-24.8 — Skill authority is campaign-owned

No host session or panel may instantiate an independent `SkillProgressionSystem` for production.

## INV-24.9 — Worker identity is explicit

Production systems must know who is doing the work if skill, fatigue, health, or fitness can affect output.

## INV-24.10 — Admission/quarantine/death vacate labour obligations

A survivor who cannot continue work must release the duty assignment through an explicit integration event/path.

## INV-24.11 — Recovery is not binary

Discharge need not mean "Fit."

Recovery may produce an `Impaired` verdict and light-duty eligibility.

## INV-24.12 — Death propagates through existing systems

Death must:
- release duties;
- change ration accounting;
- affect grief/morale;
- record memorial outcome;
- emit briefing events.

No new death subsystem.

---

# 3. Definition of Done

Plan 24 is complete only when all are true:

- a single Core `FitnessVerdict` projection exists;
- duty roster consumes it;
- expedition party validation consumes it;
- kitchen/caregiving/ward staffing can consume it;
- dead/quarantined/incapacitated survivors cannot be assigned to impossible work;
- impaired survivors may be assigned where policy permits, with visible cost;
- role-specific limits are data-driven;
- `lastSleptDay` affects real fitness;
- quarantine/admission/death vacate duty;
- `NeedsSystem` accepts deterministic attributable external modifiers;
- sleep schedule affects fatigue;
- meals affect morale/health as designed by Plan 22;
- cold/darkness/hygiene pressure reach existing needs;
- grief reaches morale/fatigue;
- leadership/friction/ration conflict can reach output through needs;
- stress emitters call the existing dependency stress seam;
- apprenticeship uses campaign skill authority;
- trapping gets actual hunter skill;
- other production owners receive worker identity through one shared pattern;
- needs UI can explain top contributors;
- ward admission → recovery/light duty → full return or death is a real journey;
- sick survivors still consume rations through Plan 22's authority;
- caregiving consumes duty hours and fatigue;
- death reorganizes duty and ration obligations;
- all affected state survives save/load through its owning systems;
- 30-day fatigue/morale/mortality simulation is deterministic and playable;
- the Wave-2 closing verification suite passes.

---

# 4. Phase P0 — Evidence Freeze and Authority Map

## P0.1 Record current repository state

Capture:

```text
commit SHA
branch
dirty-file count
DutyRosterAssignmentEngine validation inputs
DutyRosterRow fields
NeedsSystem constructor/injected collaborators
NeedKind list
ShelterScheduleSystem consumers
SetHunterSkill callers
SkillProgressionSystem construction sites
SickListSystem owners/consumers
Disease quarantine event consumers
MedicalWard admission/discharge consumers
SurvivorFate / OnDied consumers
current survivor-related save sections
current live survivor panel authorities
```

Do not assume the cited revision remains current.

---

## P0.2 Read the complete authority set

Before implementation, read:

- `Assets/Ashfall.Core/DutyRoster/DutyRosterAssignmentEngine.cs`
- `DutyRosterSystem.cs`
- role definitions/catalogs
- `Survivors/NeedsSystem.cs`
- needs profiles/tuning data
- `ShelterScheduleSystem.cs`
- `ShelterThermalSystem.cs`
- power/light state from Plan 23
- `RadiationSystem`
- `DoseLedgerSystem`
- `AfflictionContracts.cs`
- `DiseaseSystem`
- `SickListSystem`
- `ChemicalDependencySystem`
- `CombatTraumaSystem`
- `GuiltInsomniaSystem`
- memorial/grief interfaces
- `SurvivorSocialCoordinator`
- `LeadershipSystem`
- `IdeologicalFrictionSystem`
- `RationConflictSystem`
- `TraumaBondSystem`
- `SkillProgressionSystem`
- `SkillAtrophySystem`
- `ApprenticeshipHostSession`
- `WildlifeTrappingSystem`
- greenhouse/workshop/foundry/medical/kitchen production owners
- `MedicalWardSystem`
- `DiagnosisKnowledgeStore`
- `CaregivingSystem`
- `SurvivorFateSystem`
- UI panels for roster, expedition, needs, ward, caregiving.

---

## P0.3 Publish survivor-authority ownership matrix

Create:

`docs/systems/SURVIVOR_STATE_AUTHORITY_MATRIX.md`

Minimum columns:

| Fact | Authority | Persisted? | Read by | Mutated by | Fitness input? | Needs modifier source? |
|---|---|---:|---|---|---:|---:|
| hunger | NeedsSystem | yes | UI/fitness | food/day tick | yes | no |
| fatigue | NeedsSystem | yes | UI/fitness | tick/modifiers | yes | yes |
| warmth | NeedsSystem | yes | UI/fitness | thermal/tick | yes | yes |
| morale | NeedsSystem | yes | UI/fitness | modifiers | yes | yes |
| health | NeedsSystem | yes | UI/fitness | treatment/affliction | yes | yes |
| hygiene | NeedsSystem | yes | UI/fitness | water/shelter | yes | yes |
| dose | Radiation/Dose | yes | fitness/medical | exposure/treatment | yes | no |
| quarantine | DiseaseSystem | yes | fitness | disease | yes | no |
| sick band | SickListSystem | yes | fitness/ward | medical | yes | no |
| dependency | ChemicalDependency | yes | fitness | medicine/stress | yes | yes |
| trauma | CombatTrauma | yes | fitness/social | combat | yes | yes |
| sleep assignment | schedule | yes/derived per source | needs | player | yes | yes |
| skill | SkillProgression | yes | production | apprenticeship/work | yes | no |
| duty | DutyRoster | yes | production | player/system | no | consequence |
| ward admission | MedicalWard | yes | fitness | medical | yes | no |

Use current source to fill actual ownership.

---

## P0.4 Reproduce baseline defects

Before repair, add or run tests proving:

1. starving survivor can be assigned to precision work;
2. quarantined survivor can remain assigned;
3. `fatigueRecoveryModifier` changes without changing fatigue recovery;
4. `SetHunterSkill` is never called in production;
5. apprenticeship skill authority is not reference-equal to campaign skill authority;
6. ward admission does not vacate duty.

Preserve as regression tests.

---

# TASK 24A — Fitness for Duty

# 24A.0 Goal

Create one Core projection that answers:

```text
Can this survivor do this work?
If yes, at what cost?
If no, why?
How many hours are reasonable?
Which needs or conditions are driving the verdict?
```

The same verdict must be usable by duty roster, expeditions, kitchen/caregiving, and medical staffing.

---

## 24A.1 Author `FitnessVerdict`

Create:

`Assets/Ashfall.Core/Survivors/FitnessForDutyModel.cs`

Recommended shape:

```csharp
public enum FitnessLevel
{
    Fit,
    Impaired,
    Unfit,
    Incapacitated
}

public sealed record FitnessVerdict(
    string SurvivorId,
    FitnessLevel Level,
    IReadOnlyList<string> BlockingReasons,
    IReadOnlyList<string> DegradedFactors,
    float RecommendedMaxHours,
    IReadOnlyList<NeedKind> AffectedNeeds);
```

Optional additions only if justified:
- numeric cost/risk multiplier;
- role-specific warnings;
- source IDs for attribution.

Do not include UI prose.

---

## 24A.2 Define stable reason IDs

Examples:

```text
dead
quarantined
unconscious
critical_health
severe_ars
severe_fatigue
sleep_deprived
starving
dehydrated
hypothermic
infectious
withdrawal
combat_trauma
skill_below_role_minimum
dose_above_role_limit
medical_restriction
```

Use snake_case or current project ID convention.

Reason IDs must be:
- stable;
- localizable;
- testable.

---

## 24A.3 Separate condition verdict from role suitability

Preferred model:

```text
base survivor fitness
+
role requirement evaluation
=
assignment verdict
```

Do not bake role-specific rules into generic survivor fitness.

Possible split:

```csharp
FitnessVerdict EvaluateSurvivor(string survivorId);
RoleFitnessVerdict EvaluateForRole(FitnessVerdict baseVerdict, RoleRequirements role);
```

Exact API may differ.

---

## 24A.4 Source fitness from existing authorities

Base fitness reads:

### Needs
- hunger;
- thirst;
- fatigue;
- warmth;
- morale where relevant;
- health;
- hygiene where relevant;
- alive state.

### Radiation
- cumulative/acute dose;
- current ARS phase;
- current treatment state if it affects work.

### Afflictions
- injury severity;
- diagnosed or undiagnosed status should not alter physical effect;
- explicit work restrictions if contracts already provide them.

### Disease
- quarantine;
- infectious state;
- outbreak restrictions.

### Sick list
- severity band;
- care priority.

### Dependency
- active withdrawal;
- impairment.

### Trauma
- combat trauma / panic restrictions if modeled.

Do not copy values into `DutyRosterRow`.

---

## 24A.5 `lastSleptDay` integration

The source plan identifies `lastSleptDay` as tracked but unread.

Use it as:
- an attribution source;
- an additional sleep-deprivation factor.

Do not derive all fatigue exclusively from day difference if Needs already owns fatigue.

Recommended:
- Needs fatigue remains primary;
- days since sleep can escalate warnings or cap hours;
- schedule/24B changes fatigue itself.

---

## 24A.6 Define fitness thresholds in data

Where thresholds are design-tunable, put them in data/tuning.

Examples:

```text
fitness.fatigue.impaired
fitness.fatigue.unfit
fitness.health.unfit
fitness.days_without_sleep.impaired
fitness.days_without_sleep.unfit
fitness.dose.default_hot_work_limit
```

Avoid magic constants inside evaluation branches.

Validate ranges.

---

## 24A.7 Author role requirements

Extend role data with fields such as:

```text
min_skill
skill_id
max_fatigue
min_health
max_dose
max_hours_if_impaired
allow_unfit
light_duty
requires_not_quarantined
precision_work
hazard_class
```

Only add fields the current role set needs.

Do not create a generic scripting language.

---

## 24A.8 Role requirement validation

`CatalogIntegrityValidator` must verify:

- skill IDs exist;
- min/max values are sane;
- referenced need kinds valid;
- hazard classes valid;
- light-duty role references valid;
- impossible combinations flagged.

Examples:
- `min_health > 100` => invalid;
- `max_fatigue < 0` => invalid;
- missing skill ID => invalid.

---

## 24A.9 Inject fitness collaborator into duty assignment

Use the existing Core-friendly injection pattern.

Example:

```csharp
Func<string, FitnessVerdict>? evaluateFitness
```

or a narrow interface.

Requirements:
- optional/fallback only for isolated tests if necessary;
- production always supplies campaign projection;
- roster remains engine-free.

---

## 24A.10 Extend assignment validation

Keep existing codes and add fitness-specific results.

Possible assignment result:

```text
Allowed
AllowedWithWarning
Blocked
```

Return:
- reason IDs;
- role requirement failures;
- recommended max hours;
- degradation preview.

Do not collapse to generic `cannot_assign`.

---

## 24A.11 Warn, do not over-block

`Impaired` should generally remain assignable.

Player confirmation should record that the warning was knowingly overridden.

Potential event:

```text
duty_assigned_impaired
```

if event vocabulary supports it.

The genre decision is:
- rest them;
- use them anyway;
- accept risk.

---

## 24A.12 Incapacitated blocking

Always block clearly where appropriate:

- dead;
- unconscious;
- quarantined when policy forbids work;
- medically incapacitated;
- other explicit hard blockers.

A corpse must never pass `ValidateAssign`.

---

## 24A.13 Unfit + light duty

Define light-duty roles in data.

Examples should come from actual game roles, not invented content.

Rules:
- reduced max hours;
- lower hazard;
- lower skill/precision demands;
- limited production contribution.

An `Unfit` survivor may be eligible for `light_duty=true` roles if data permits.

---

## 24A.14 Expedition party validation

Use identical fitness projection.

Expedition-specific role requirements may be stricter:

```text
travel load
dose ceiling
health minimum
fatigue ceiling
```

Do not create a second "expedition fitness" model.

Add cross-consumer test:
same survivor state → consistent reasons across roster and expedition, with role-specific differences only.

---

## 24A.15 Kitchen cook validation

Plan 22B cook assignment must consume fitness.

Examples:
- sick/exhausted cook may be impaired;
- quarantine blocks communal cooking if authored policy requires;
- low relevant skill may reduce quality via 24B worker-output integration.

Do not add kitchen-specific health checks.

---

## 24A.16 Medical/caregiving staff validation

Ward/caregiving staff use the same model.

Role data may require:
- minimum health;
- fatigue limit;
- medical skill;
- no quarantine/infectious status.

A sick nurse may be:
- blocked;
- allowed with warning;
depending on role data.

---

## 24A.17 Mid-shift invalidation

When survivor state crosses into a hard blocker:
- duty system receives invalidation;
- shift vacates;
- emit `duty_vacated`;
- downstream production notices missing worker.

Sources:
- quarantine started;
- ward admission;
- incapacitation;
- death.

Do not poll every frame if event-driven invalidation exists.

---

## 24A.18 UI condition display

Duty roster and expedition UI show:

```text
Name
Fitness level
Primary reason
Secondary reasons
Recommended max hours
Role-specific warnings
```

Color may support status, but words/numbers must carry meaning.

Accessibility requirement:
- no color-only status;
- keyboard navigation preserved;
- screen-reader/localization keys where project supports them.

---

## 24A.19 Warning confirmation

Assigning an impaired survivor requires explicit confirmation when UX conventions allow.

Record:
- survivor;
- role;
- warnings;
- day;
- override.

Do not add modal spam for minor warnings; group reasons.

---

## 24A.20 Fitness derivation after load

Save/load test:

```text
create persisted starving + irradiated + quarantined state
save
load
evaluate fitness
```

Expected:
- same verdict;
- no serialized verdict required.

---

## 24A.21 Determinism

Same state => identical:
- fitness level;
- reason order;
- max hours;
- affected needs.

Sort reason IDs deterministically.

---

## 24A.22 Precision-work consequence integration test

Source scenario:

```text
starving survivor
→ assign to precision work despite warning
→ work occurs
→ increased mistake/failure consequence
```

The consequence must use existing production/error mechanics where available.

Do not invent a generic random "mistake" subsystem solely for the test.

If no consumer exists:
- record degraded output multiplier in the assignment/work result;
- wire to nearest real producer.

---

## 24A.23 24A acceptance metrics

Record before/after:

```text
duty validation condition inputs: 0 → 1 shared fitness projection
incapacitated assignments accepted: baseline → 0
fitness consumers: roster + expedition + cook + ward/caregiving
role requirements in data: count
hardcoded role fitness rules: 0 target
```

### 24A DoD

The game can intentionally overwork an impaired survivor, but cannot accidentally assign a dead or quarantined/incapacitated survivor as though nothing happened.

---

# TASK 24B — Needs Modifier Stack, Sleep, Morale, Stress & Skill-to-Work

# 24B.0 Goal

Make the existing need bars respond to the systems that already surround them.

The target is not "more needs."

The target is one external modifier stack feeding existing needs, with blame/attribution.

---

## 24B.1 Design `NeedsModifierStack`

Create a Core type, e.g.:

`Assets/Ashfall.Core/Survivors/NeedsModifierStack.cs`

Recommended record:

```csharp
public sealed record NeedsModifier(
    string SourceId,
    NeedKind Need,
    float Amount,
    int StartDay,
    int EndDay,
    string? SurvivorId);
```

Alternative duration/window representation may follow project time conventions.

Required operations:

```text
Register / Add
Remove by source
GetActiveModifiers
AggregateFor(need, survivor, day)
GetTopContributors
```

---

## 24B.2 Derived vs persisted stack

Preferred:
- authoritative sources persist their own state;
- modifier stack is recomputed/registered from those sources after composition/load;
- transient timed modifiers may persist only if their source state cannot reconstruct them.

The source plan explicitly expects the stack to be derived.

Add test:
- save contains source states, not duplicate aggregate modifier state;
- after load, aggregate equals pre-save aggregate.

---

## 24B.3 Deterministic aggregation

Sort modifiers by:
1. survivor scope;
2. `NeedKind`;
3. ordinal `SourceId`;
4. stable window key.

Aggregation should be deterministic regardless of registration order.

Test shuffled registration order.

---

## 24B.4 Preserve heat-source semantics deliberately

`NeedsSystem` already has `_isNearHeatSource`.

Choose one:

A. keep it as a special optimized collaborator and document it; or
B. migrate heat influence into modifier stack in one controlled change.

Do not accidentally double-apply warmth.

Add regression test against preexisting heat behavior.

---

## 24B.5 NeedsSystem seam

Add one external-modifier collaborator to `NeedsSystem`.

Possible:

```csharp
Func<string, NeedKind, int, float> getExternalModifier
```

or injected stack service.

Requirements:
- Core-only;
- deterministic;
- no UI access;
- no system-specific branches in NeedsSystem.

---

## 24B.6 Sleep assignment → fatigue recovery

Connect:

- `SleepAssignment`;
- schedule fatigue recovery modifier;
- curfew;
- emergency override;
- lighting/sleep quality.

Target flow:

```text
schedule state
→ sleep-quality modifier
→ NeedsModifierStack(Fatigue)
→ NeedsSystem tick
→ next-day fatigue
→ 24A fitness
```

---

## 24B.7 `lastSleptDay` write/read contract

When adequate sleep occurs:
- update `lastSleptDay`.

When sleep is interrupted/missed:
- do not update it;
- apply fatigue modifier;
- emit attributable event if supported.

Avoid two independent "did sleep" facts.

---

## 24B.8 Lighting and power impact sleep

Plan 23 provides actual lighting/power state.

Use:
- powered sleeping area;
- blackout/darkness;
- emergency lighting;
according to existing schedule/room rules.

Do not add a new electricity check inside NeedsSystem.

Shelter/power computes condition; modifier stack receives effect.

---

## 24B.9 Curfew/emergency override

Schedule policy can improve/degrade sleep.

Requirements:
- no UI-only percentage;
- modifier applies to fatigue recovery;
- event/attribution explains unusual sleep.

---

## 24B.10 Meals → morale/health

Use Plan 22 serving log/nutrition.

On meal served:
- hunger effect remains through consumption/meal system;
- meal quality contributes sanctioned morale/health modifiers;
- source ID includes meal/serving context.

Do not double-apply hunger via modifier stack if 22 already applies direct hunger restoration.

---

## 24B.11 Missed meal

A skipped meal should be distinct from base hunger decay if product data supports it.

Possible:
- event `meal_skipped`;
- morale penalty;
- grievance input.

Avoid stacking arbitrary additional hunger punishment on top of existing hunger need unless authored.

---

## 24B.12 Cold → fatigue/warmth/hygiene

Plan 23 thermal state contributes.

Potential existing need impacts:
- warmth;
- fatigue;
- hygiene if water/heat access constrains washing.

Keep effect data-driven.

Do not add `ColdStress` need kind.

---

## 24B.13 Water outage → hygiene

Needs already contains Hygiene.

Connect shelter water availability into hygiene decay/recovery.

Use existing shelter/water authority.

Avoid a second sanitation meter.

---

## 24B.14 Grief → morale/fatigue

Use:
- `DeathQuality`;
- memorial/grief sink;
- `TraumaBondSystem`;
- current guilt-insomnia pattern as architectural template.

On death:
- linked survivors receive attributable grief modifiers;
- duration/intensity comes from existing grief/memorial data where available;
- no universal hardcoded morale loss if relationship system can distinguish bonds.

---

## 24B.15 Guilt-insomnia compatibility

Ensure guilt and grief do not double-register the same semantic source.

Source IDs must distinguish:
- guilt;
- grief;
- trauma bond;
- sleep deprivation.

Add contributor attribution test.

---

## 24B.16 Leadership → needs/output

`LeadershipSystem` can register sanctioned morale/fatigue modifiers.

Do not compute production directly in leadership if needs is the intended bridge.

Example:
- effective leadership reduces morale decay/fatigue pressure;
- poor leadership increases it.

Use current data/rules.

---

## 24B.17 Ideological friction

Feed `IdeologicalFrictionSystem` through:
- morale;
- possibly fatigue/stress if authored.

Do not create a hidden production multiplier in parallel.

---

## 24B.18 Ration conflict

Plan 22 unequal service can generate grievance.

Ration conflict then contributes:
- morale;
- stress;
through existing social/dependency seams.

This closes:

```text
food inequality
→ social conflict
→ morale/stress
→ dependency / fitness / output
```

---

## 24B.19 Stress → dependency

Use existing:

`ChemicalDependencySystem.OnStressReported`

Stress sources:
- overwork;
- quarantine;
- grief;
- severe ration conflict;
- other current modeled events.

Do not create a second stress state in NeedsModifierStack.

Modifier stack may affect morale/fatigue; dependency gets stress events through its existing API.

---

## 24B.20 Overwork contribution

When actual duty hours exceed recommended max hours from 24A:
- fatigue modifier increases;
- stress event may fire;
- role output/mistake risk may degrade;
- attribution names role/shift.

Do not infer overwork only from role assignment; use actual hours if system tracks them.

---

## 24B.21 Fix apprenticeship skill authority

Remove:

```text
new SkillProgressionSystem()
```

from production `ApprenticeshipHostSession`.

Inject/reuse campaign-owned skill authority.

Required test:

```text
ReferenceEquals(apprenticeship.Skills, campaign.Skills) == true
```

Also test:
- apprenticeship gain appears in skill panel;
- save/load retains same campaign skill;
- atrophy affects same value.

---

## 24B.22 Skill atrophy visibility

Ensure `SkillAtrophySystem` and `SkillProgressionSystem` operate on the same campaign skill state or sanctioned shared aggregate.

The player must be able to see:
- gain;
- loss;
- current effective skill.

No duplicate skill snapshot in panel.

---

## 24B.23 Worker identity contract

Production owners must receive the worker identity through one shared pattern.

Recommended:
- work order / daily production context contains `survivorId`;
- producer asks campaign skill authority for relevant skill;
- producer optionally consumes fitness/work modifier.

Do not add one setter per producer.

---

## 24B.24 Wildlife trapping skill

Call existing `SetHunterSkill` using actual assigned hunter skill only if that setter remains current architecture.

Because source explicitly says no more per-system setters:
- treat trapping's existing setter as legacy-compatible;
- for new producers, use worker context/query.

Test:
- low skill => expected floor/multiplier;
- threshold gates respected;
- high skill => improved yield;
- no worker => explicit fallback/block according to system design.

---

## 24B.25 Greenhouse skill

Connect assigned worker skill through shared production context.

Use actual existing skill taxonomy.

No invented `greenhouse_skill` if skills are named differently.

---

## 24B.26 Workshop/foundry skill

Precision/technical production uses:
- assigned worker;
- relevant campaign skill;
- fitness;
- fatigue/impairment.

Degraded state should affect:
- speed;
- quality;
- failure risk;
according to existing mechanics.

---

## 24B.27 Medical skill

Ward procedures/caregiving use actual worker skill.

A medical worker's own fatigue/fitness affects procedure/care quality through existing treatment system where supported.

Do not create arbitrary roll if procedures have no probabilistic quality today; use duration/efficiency or warning metadata instead.

---

## 24B.28 Cooking skill

Plan 22B meals use assigned cook.

Skill may affect:
- prep time;
- nutrition/quality;
- waste;
according to existing kitchen fields.

Do not let skill bypass ingredient costs.

---

## 24B.29 Needs attribution UI

Needs panel shows top contributors.

Example:

```text
Fatigue +2.0/day
• No sleep for 3 days
• Night shift
• Cold sleeping room

Morale -1.5/day
• Ration dispute
• Death of bonded survivor
+ Warm meal
```

Use current localization/layout conventions.

---

## 24B.30 Contributor ranking

Ranking rules:
- largest absolute effect first;
- deterministic tie order by source ID;
- cap number displayed;
- aggregate "other" if needed.

Do not expose raw internal object names.

---

## 24B.31 30-day balance simulation

Run at least:

### Policy A — humane rotation
- respect recommended max hours;
- regular sleep;
- meal priority.

### Policy B — aggressive production
- frequent impaired assignments;
- shortened rest.

### Policy C — crisis
- power/heat loss;
- illness/grief.

Track:

```text
fatigue by survivor
health
morale
hygiene
work hours
duty vacancies
mistake/output modifiers
dependency stress events
deaths
recovery
```

Acceptance:
- fatigue creates rotation decisions;
- aggressive policy yields benefit plus meaningful cost;
- system does not automatically death-spiral under reasonable baseline.

---

## 24B.32 Modifier-stack tests

Required:
- aggregation;
- registration-order independence;
- expiration/window;
- contributor attribution;
- survivor scoping;
- load re-derivation;
- no duplicate source after rebind/load;
- no heat double application.

---

## 24B.33 Skill-to-yield tests

Per connected producer:
- same worker state + same skill => deterministic output;
- low vs high skill produces expected difference;
- atrophy changes output next day;
- apprenticeship gain changes output;
- save/load preserves skill effect.

### 24B DoD

Sleep, meal quality, cold, hygiene, grief, leadership/friction/ration conflict, and overwork visibly affect the same need bars, each with a named source; campaign skill state reaches actual labour output.

---

# TASK 24C — Illness, Injury, Recovery, Care & Death as Roster Events

# 24C.0 Goal

Turn the medical journey into a shelter-wide operational journey.

Admission, quarantine, recovery, discharge, death, and mourning must reorganize labour, rations, caregiving, morale, and briefing history.

---

## 24C.1 Publish current patient journey map

Create:

`docs/systems/SURVIVOR_JOURNEY_OWNERSHIP.md`

Map current stages:

```text
symptom / affliction
→ diagnosis knowledge
→ sick-list classification
→ ward admission
→ patient bed
→ treatment/procedure
→ caregiving
→ discharge
→ recovery restriction
→ full duty
```

Death branch:

```text
condition worsens
→ survivor fate / OnDied
→ duty release
→ ration recalculation
→ memorial
→ grief
→ briefing
```

Mark every current seam that stops inside a panel.

---

## 24C.2 Admission → duty vacated

On ward admission:
- 24A fitness becomes at least `Unfit`/appropriate state;
- current duty assignment removed;
- emit `duty_vacated`;
- production owner sees vacancy.

No separate scripted production penalty.

---

## 24C.3 Quarantine → duty vacated

On quarantine start:
- block incompatible roles;
- vacate existing duty;
- retain explicitly authored isolated/light duties only if role data permits.

On quarantine end:
- re-evaluate fitness;
- do not automatically restore old shift unless design explicitly supports auto-return.

---

## 24C.4 Ward admission idempotence

Repeated admission event must not:
- vacate twice;
- duplicate journal event;
- duplicate bed reservation.

Add idempotency test.

---

## 24C.5 Discharge → impaired recovery

Discharge should derive a recovery state from affliction/treatment outcome.

Possible:
- `Impaired`;
- reduced recommended max hours;
- restricted role set.

Do not set generic `Fit` on discharge.

---

## 24C.6 Author recovery ramps

Per relevant affliction/treatment data:

```text
recovery_days
max_hours_by_day
restricted_roles
health/fatigue modifiers
```

Use data only where current medical model requires it.

Avoid overengineering every affliction.

---

## 24C.7 Light-duty role set

Use 24A role data.

Characteristics:
- reduced hours;
- low hazard;
- low physical/precision burden;
- useful but limited output.

Do not invent roles purely for mechanics if existing roles can be marked light duty.

---

## 24C.8 Light-duty transition tests

Journey:

```text
ward discharge
→ Unfit/Impaired
→ light-duty eligible
→ several days recovery
→ Fit
→ normal role eligible
```

Same save/load midpoint produces same trajectory.

---

## 24C.9 Sick survivors consume rations

Use Plan 22 single consumption authority.

Required:
- sick-list members remain included in ration planning;
- convalescent diet only where authored;
- treatment status does not create free food.

---

## 24C.10 Ration re-split on admission/death

Admission:
- survivor still eats unless medical policy says otherwise.

Death:
- remove survivor from future ration requirement;
- do not reclaim food already consumed that day;
- recalculate next allocation deterministically.

---

## 24C.11 Unequal sick-list ration grievance

If scarcity results in unequal feeding:
- feed existing `RationConflictSystem`;
- patient priority may be a policy input;
- do not create a medical grievance system.

---

## 24C.12 Caregiving is labour

Caregiving assignment consumes:
- survivor duty hours;
- fitness eligibility;
- relevant skill.

Caregiver gets:
- fatigue;
- stress where appropriate;
- possible morale/relationship effects through existing systems.

---

## 24C.13 Caregiving competes with production

A survivor assigned as caregiver cannot simultaneously contribute full hours to incompatible production roles.

Use duty roster conflict rules.

No hidden "free nurse."

---

## 24C.14 Caregiver fatigue

Actual care hours contribute to 24B modifier stack.

Attribution:

```text
caregiving:<patient_id>
```

or stable equivalent.

---

## 24C.15 Diagnosis knowledge honesty

`DiagnosisKnowledgeStore` decides what the player knows.

Physical effects occur whether diagnosed or not.

UI:
- known facts shown;
- unknown cause remains uncertain;
- do not reveal hidden affliction because fitness model knows it internally.

Fitness reasons exposed to UI should respect diagnosis knowledge where necessary.

Example:
Core reason may be `medical_restriction`;
UI may display `unexplained weakness` until diagnosis.

---

## 24C.16 Separate mechanical truth from player knowledge

Maintain:
- full internal fitness evaluation;
- filtered presentation.

Do not weaken actual effects because diagnosis is unknown.

---

## 24C.17 Autopsy knowledge seam

If current autopsy chain reveals cause-of-death/affliction knowledge:
- memorial/medical records update knowledge;
- no retroactive gameplay correction.

Use existing `AutopsySystem`.

---

## 24C.18 Death event integration

Use existing:
- `SurvivorFateSystem.OnSurvivorFate`;
- `NeedsSystem.OnDied`;
- current host subscription.

Central death handler must orchestrate downstream notifications without becoming a second death authority.

---

## 24C.19 Death → duty release

On death:
- remove duty assignment;
- cancel incompatible pending work ownership;
- emit vacancy event.

Ensure idempotence across both fate and needs death events if both can fire.

Prefer one canonical death integration handler.

---

## 24C.20 Death → ration recalculation

Next ration plan excludes deceased survivor.

Tests:
- N survivors before;
- death;
- N-1 requirement after;
- current-day accounting stable.

---

## 24C.21 Death → memorial

Write:
- survivor;
- death quality;
- memorial outcome;
- epitaph/wall record if current system supports it.

This must remain the same death record consumed by Plan 19 epilogue continuity.

---

## 24C.22 Death → grief modifiers

Use 24B grief path.

Factors may include:
- trauma bond;
- relationship;
- death quality;
- memorial/funeral response.

Do not apply duplicate global morale penalty plus bond penalty unless authored.

---

## 24C.23 Optional mourning action

Add an authored optional response using existing event/duty/action framework.

Tradeoff:
- morale/grief benefit;
- labour hours lost.

No forced modal.

No scripted tragedy content is required.

---

## 24C.24 Mourning duty/time accounting

If mourning consumes a day/shift:
- duty availability decreases;
- production owners see lost hours;
- event `mourned` recorded.

Do not alter simulation day separately.

---

## 24C.25 Briefing events

Emit canonical state events:

- `admitted`
- `discharged`
- `quarantined`
- `duty_vacated`
- `died`
- `mourned`

Use existing 17A vocabulary/catalog rules.

Event payloads:
- survivor;
- reason/outcome;
- day;
- related duty/ward where useful.

---

## 24C.26 Audio parity

Reuse current medical audio family.

Requirements:
- quarantine seal cue once;
- quarantine clear once;
- survivor death cue once;
- no double-fire from both panel and host;
- 16C subscription lifecycle remains green.

---

## 24C.27 Save/load mid-illness

Journey:

```text
affliction
→ sick list
→ ward admission
→ duty vacated
→ save
→ load
```

Assert:
- patient still admitted;
- duty still vacant;
- fitness same;
- treatment state same;
- ration requirement same;
- briefing not duplicated.

---

## 24C.28 Save/load mid-recovery

Journey:
- discharged impaired;
- recovery day 2 of N;
- light duty assigned;
- save/load;
- same remaining restrictions.

---

## 24C.29 Save/load after death

Assert:
- survivor absent/dead per canonical roster model;
- duty released;
- memorial present once;
- ration count updated;
- grief modifiers reconstruct;
- death event not duplicated.

---

## 24C.30 30-day illness determinism

Script:
- seeded disease/injury timeline;
- treatment policy;
- duty policy;
- ration policy.

Same seed/choices => identical:
- admissions;
- duty vacancies;
- recovery;
- deaths;
- memorial outcomes;
- survivor need curves.

---

## 24C.31 Panel snapshots

Capture changed states:

### Duty roster
- fit;
- impaired warning;
- incapacitated block;
- light duty.

### Medical ward
- admitted;
- recovering;
- discharge eligible.

### Caregiving
- caregiver assigned;
- caregiver impaired/exhausted;
- no eligible caregiver.

Only intentional UI changes update goldens.

---

## 24C.32 Documentation truth corrections

Update `AGENTS.md` stale claims identified by source only after verifying current source.

Specifically re-check:
- H5 Utility AI fork status;
- H11 JournalSystem test status.

Do not blindly copy source-plan claims if repository has changed.

---

## 24C.33 Wave-close verification

Run full Plan 24 suite and full Wave-2 gates.

### 24C DoD

Illness and death change the shift table, ration calculation, care load, morale, memorial state, and briefing history through real authorities.

---

# 5. Cross-Task Dependency Graph

```text
16B campaign authority fix
         │
         └──────────────► apprenticeship / skill ownership
                                │
20 dose/radiation ───────► 24A FITNESS VERDICT
22 meals/medicine ───────►        │
23 heat/light/sleep ─────►        │
                                  ▼
                           24B MODIFIER STACK
                                  │
                ┌─────────────────┼─────────────────┐
                ▼                 ▼                 ▼
            sleep/fatigue      morale/stress     skill→work
                │                 │                 │
                └─────────────────┼─────────────────┘
                                  ▼
                           24C SURVIVOR JOURNEY
                                  │
                    admission / recovery / death
                                  │
                                  ▼
                      duty + rations + memorial
```

Strict plan-local order:

```text
24A
 ↓
24B
 ↓
24C
```

---

# 6. Wave-2 Overall Sequence

Use the source plan's sequence unless current file conflicts require local rebasing:

```text
20A
→ 20B
→ 22A
→ 21A
→ 23A
→ 22B
→ 21B
→ 23B
→ 24A
→ 24B
→ 21C
→ 22C
→ 20C
→ 23C
→ 24C
```

Highest-value short-capacity set:
- 22A;
- 24A;
- 23A.

---

# 7. Integration Contracts

## 7.1 Fitness model contract

Reads:
- needs;
- dose;
- affliction;
- disease;
- sick list;
- dependency;
- trauma;
- sleep history.

Outputs:
- derived verdict;
- reasons;
- hours;
- affected needs.

Does not mutate.

---

## 7.2 Duty roster contract

Owns:
- assignment;
- role;
- shift obligations.

Consumes:
- fitness;
- role requirements.

Emits:
- assignment;
- vacancy.

Does not own health state.

---

## 7.3 Needs contract

Owns:
- need values.

Consumes:
- deterministic modifiers.

Does not know:
- grief system class;
- kitchen class;
- power class;
- leadership class.

Only modifier sources know their own semantics.

---

## 7.4 Skill contract

`SkillProgressionSystem` is one campaign authority.

Consumers query it.

Apprenticeship mutates it.

Atrophy mutates/adjusts it through sanctioned coordinator.

Panels display it.

---

## 7.5 Medical journey contract

Ward owns:
- admission;
- beds;
- procedures.

Sick list owns:
- sickness severity/prioritization.

Disease owns:
- quarantine.

Duty roster reacts.

Needs reacts through modifier stack.

Memorial owns remembrance after death.

---

# 8. State Transition Table

| Transition | Fitness | Duty | Needs modifiers | Rations | Briefing |
|---|---|---|---|---|---|
| normal → tired | Fit/Impaired | remains | fatigue source | unchanged | optional |
| tired → exhausted | Impaired/Unfit | warn/vacate per role | fatigue | unchanged | attributable |
| disease → quarantine | Incapacitated/role-blocked | vacate | stress | unchanged | quarantined |
| ward admission | Unfit | vacate | illness/care | unchanged | admitted |
| discharge | Impaired | light duty | recovery | unchanged | discharged |
| full recovery | Fit | normal eligible | recovery ends | unchanged | optional |
| death | Incapacitated/dead | release | grief to others | remove next allocation | died |
| mourning chosen | survivors vary | hours lost | grief benefit | unchanged | mourned |

Use actual source semantics during implementation.

---

# 9. Failure Injection Matrix

## N24.1 Dead survivor assignment
Expected: blocked.

## N24.2 Quarantined survivor assignment
Expected: blocked for incompatible work; no hidden override.

## N24.3 Impaired survivor assignment
Expected: warning, explicit confirmation, consequence preview.

## N24.4 Role skill below minimum
Expected: role-specific block/warning from data.

## N24.5 Three days without sleep
Expected: fitness degraded; fatigue contributor visible.

## N24.6 Modifier registration order shuffled
Expected: identical need result.

## N24.7 Load causes duplicate modifier
Expected: no doubled effect.

## N24.8 Apprenticeship uses duplicate skill system
Expected: identity test fails.

## N24.9 No worker assigned to trapping
Expected: explicit fallback/block according to current producer semantics; never silent permanent 0.5× from missing wiring.

## N24.10 Ward admission while on shift
Expected: shift vacated once.

## N24.11 Death fires through two events
Expected: one duty release, one memorial, one ration recalculation, one death cue.

## N24.12 Undiagnosed affliction
Expected: mechanics apply; UI does not reveal hidden diagnosis.

## N24.13 Save/load during quarantine
Expected: survivor remains unavailable; no duplicate vacancy event.

## N24.14 Mourning action
Expected: labour cost + morale/grief benefit once.

---

# 10. Determinism Contract

Same:

```text
campaign seed
+ survivor state
+ duty decisions
+ meal decisions
+ treatment decisions
+ sleep schedule
+ power/thermal state
+ save/load points
```

must yield identical:

```text
fitness verdicts
modifier aggregates
need curves
skill values
production outputs
admissions
recoveries
deaths
memorial outcomes
briefing events
```

No:
- clock-based fitness randomness;
- hash-based role randomness;
- UI-open-time modifiers;
- unordered dictionary-dependent contributor selection.

---

# 11. Performance Guardrails

Fitness may be queried often by UI and assignment validation.

Requirements:
- no catalog reparsing per query;
- no full save-state traversal;
- no expensive allocation-heavy LINQ in hot UI refresh if current project avoids it;
- role requirements preloaded;
- derived verdict can be cached only within a stable state/version boundary if necessary, never persisted as authority.

Needs modifier aggregation:
- scale O(active modifiers for survivor), not O(all systems × all survivors × all catalog data);
- expired modifiers pruned or ignored efficiently;
- no per-frame recomputation if needs tick is day/hour based.

---

# 12. Attribution Model

Every visible needs change should answer:

```text
what changed?
how much?
why?
for how long?
```

Example source IDs:

```text
sleep:no_sleep_3_days
duty:night_shift
thermal:room_cold
meal:quality_good
grief:survivor_<id>
leadership:poor
ration_conflict:unequal_service
recovery:affliction_<id>
caregiving:patient_<id>
```

Use stable IDs and localization mapping.

Do not encode player-facing prose inside Core.

---

# 13. Save/Persistence Strategy

Persist authorities, not projections.

## Persist
- needs;
- dose;
- disease/quarantine;
- afflictions;
- sick-list state;
- dependency;
- trauma;
- sleep schedule/history;
- skill;
- duty;
- ward state;
- fate/memorial.

## Derive
- fitness verdict;
- role suitability;
- active needs modifier aggregate;
- UI contributor summary.

Transient modifier windows may need persisted source state where source cannot reconstruct them.

---

# 14. UI Acceptance Criteria

## Duty roster

Show:
- fitness level;
- reasons;
- role warning;
- max recommended hours;
- skill requirement;
- assignment confirmation.

## Expedition picker

Same fitness vocabulary.

Do not invent separate colors/terms.

## Needs panel

Show:
- current value;
- trend if existing;
- top contributors;
- contributor magnitude;
- source-friendly label.

## Ward

Show:
- patient state;
- duty impact;
- recovery restriction;
- light-duty eligibility where relevant.

## Caregiving

Show:
- assigned carer;
- carer fitness;
- hours;
- fatigue consequence.

Accessibility:
- text + color;
- keyboard navigation;
- consistent reason localization.

---

# 15. Balance Acceptance

## 15.1 Fitness thresholds

Too strict:
- constant blocked assignments;
- player cannot choose sacrifice.

Too loose:
- fitness becomes decorative.

Target:
- impaired work is common in crisis;
- incapacitated is hard block;
- light duty creates middle ground.

---

## 15.2 Fatigue

Target:
- rotation decisions by mid-game;
- sleep quality matters;
- emergency overwork can buy short-term output;
- recovery is possible.

Avoid:
- permanent death spiral from one bad night;
- trivial full reset each sleep.

---

## 15.3 Morale

Morale should reflect:
- meals;
- grief;
- leadership;
- friction;
- ration inequality.

Do not let decor or one positive source completely erase structural crisis effects.

---

## 15.4 Skill

Skill must be:
- visible;
- produced by apprenticeship/work;
- vulnerable to atrophy if authored;
- materially relevant to output.

Do not make low skill equal zero productivity unless role data requires it.

---

## 15.5 Care burden

A ward patient should cost:
- bed/supplies through medical system;
- food;
- caregiver hours;
- indirect production loss.

But recovery should produce value compared with abandonment/death.

---

# 16. 30-Day Telemetry KPIs

Capture:

```text
survivors by fitness level per day
average fatigue
max fatigue
sleep hours/quality
impaired assignments
blocked assignments
duty vacancies
production hours lost
meal quality
morale
grief modifiers
ration conflicts
dependency stress events
skill gains/losses
yield by skill band
ward admissions
quarantines
caregiving hours
recoveries
light-duty days
deaths
```

Compare policies.

---

# 17. Static / CI Gates

Recommended:

1. no production `new SkillProgressionSystem()` outside composition authority;
2. duty assignment engine has configured fitness provider in production;
3. role requirement IDs validate;
4. no second needs/morale/stress state introduced;
5. modifier source IDs deterministic/nonempty;
6. no per-producer new skill setter pattern added;
7. death integration idempotence test;
8. fitness projection not serialized as authority.

Use existing repository gate idioms.

---

# 18. Test Pyramid

## Tier 1 — Core unit

- fitness boundaries;
- role suitability;
- modifier stack;
- recovery ramp;
- contributor order.

## Tier 2 — Host

- authority injection;
- skill identity;
- event wiring;
- admission/death duty release.

## Tier 3 — Integration

- assignment → overwork → fatigue;
- meal → morale;
- cold → fatigue/warmth;
- apprenticeship → skill → yield;
- ward → light duty → recovery;
- death → duty/rations/memorial/grief.

## Tier 4 — UI

- roster warning;
- expedition warning;
- needs contributors;
- ward/caregiving state.

## Tier 5 — Persistence

- save/load per journey.

## Tier 6 — Telemetry

- 30-day deterministic policy runs.

---

# 19. Verification Commands

Run per task and at wave close:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --survivors-selftest
bash scripts/ci/triad-drift-gate.sh
ashfall-telemetry-playtest
ashfall-ui-access
ashfall-snapshot-diff
bash scripts/ci/verify-fast.sh
```

Expected:
- all tests pass;
- 0 integrity errors;
- bridge exits 0;
- survivors selftest passes;
- accessibility/snapshot checks accepted;
- telemetry artifact recorded;
- fast verification fully green.

---

# 20. Recommended Commit Breakdown

```text
24A-1 baseline tests + authority matrix
24A-2 FitnessForDutyModel + thresholds
24A-3 role requirement data + validator
24A-4 duty roster integration
24A-5 expedition/kitchen/ward consumers + UI
24A-6 mid-shift invalidation + persistence/determinism

24B-1 NeedsModifierStack + deterministic aggregation
24B-2 NeedsSystem seam + sleep/schedule
24B-3 thermal/hygiene + meal/grief modifiers
24B-4 leadership/friction/ration/stress integration
24B-5 skill authority fix
24B-6 worker identity + trapping/producer skill links
24B-7 attribution UI + 30-day balance

24C-1 survivor journey ownership doc + failing journeys
24C-2 admission/quarantine duty release
24C-3 discharge/recovery/light duty
24C-4 caregiving labour + ration integration
24C-5 death/ration/memorial/grief integration
24C-6 briefing/audio/UI snapshots + docs truth + wave close
```

Avoid mixing:
- unrelated survivor content;
- new need kinds;
- new role catalog redesign;
- broad UI restyling;
- Wave-3 narrative features.

---

# 21. Risk Register

## R24.1 Fitness over-blocking

Risk:
- roster becomes frustrating.

Mitigation:
- preserve `Impaired` warnings;
- light-duty roles;
- telemetry assignment rejection rate.

---

## R24.2 Balance-wide needs regression

24B affects every survivor.

Mitigation:
- source-by-source tests;
- one modifier stack;
- 30-day policy comparison;
- no unrelated tuning until wiring is correct.

---

## R24.3 Double modifier application

Risk:
- heat/sleep/grief applied from old and new paths.

Mitigation:
- explicit source IDs;
- duplicate registration gate;
- migration tests.

---

## R24.4 Skill authority drift

Risk:
- panel/apprenticeship/production read different instances.

Mitigation:
- reference identity assertions;
- composition-root gate.

---

## R24.5 Producer API sprawl

Risk:
- each producer gets unique skill/fatigue setters.

Mitigation:
- worker context;
- query shared skill/fitness authority;
- trapping existing setter is the only legacy exception.

---

## R24.6 Death double-fire

Risk:
- fate and needs death events both trigger consequences.

Mitigation:
- canonical idempotent death integration key;
- one memorial/duty release/ration recalculation test.

---

## R24.7 Diagnosis leak

Risk:
- fitness UI reveals hidden disease.

Mitigation:
- mechanical truth vs presentation filtering.

---

## R24.8 Recovery saves drift

Risk:
- discharge/load resets restrictions.

Mitigation:
- persist source affliction/recovery state;
- derive fitness after load.

---

# 22. Final Acceptance Checklist

## 24A — Fitness

- [ ] authority map published
- [ ] baseline impossible-assignment tests captured
- [ ] FitnessLevel enum implemented
- [ ] FitnessVerdict implemented
- [ ] stable reason IDs
- [ ] needs inputs wired
- [ ] radiation/ARS inputs wired
- [ ] affliction inputs wired
- [ ] quarantine inputs wired
- [ ] sick-list inputs wired
- [ ] dependency/trauma inputs wired
- [ ] lastSleptDay meaningful
- [ ] thresholds data-driven
- [ ] role requirements data-driven
- [ ] role catalog validation
- [ ] duty roster consumes shared fitness
- [ ] impaired assignment warning path
- [ ] incapacitated hard block
- [ ] light-duty support
- [ ] expedition uses same verdict
- [ ] kitchen/ward/caregiving consume same verdict
- [ ] mid-shift invalidation emits duty_vacated
- [ ] UI reasons accessible
- [ ] load re-derivation passes
- [ ] deterministic reason ordering
- [ ] precision-work consequence integration passes

## 24B — Needs + skill

- [ ] NeedsModifierStack implemented
- [ ] stack derived rather than duplicated in save
- [ ] deterministic aggregation
- [ ] heat semantics preserved without duplication
- [ ] NeedsSystem consumes one external seam
- [ ] sleep modifies fatigue
- [ ] lastSleptDay updated by real sleep
- [ ] lighting/power affects sleep
- [ ] curfew/emergency behavior real
- [ ] meal quality affects sanctioned needs
- [ ] missed-meal behavior explicit
- [ ] cold/water outages affect existing needs
- [ ] grief affects morale/fatigue
- [ ] guilt/grief sources do not double count
- [ ] leadership reaches needs/output
- [ ] ideological friction reaches needs/output
- [ ] ration conflict reaches needs/stress
- [ ] stress calls existing dependency API
- [ ] overwork produces fatigue/stress
- [ ] apprenticeship uses campaign skill authority
- [ ] skill atrophy visible in same authority
- [ ] worker identity context established
- [ ] trapping receives actual skill
- [ ] greenhouse/workshop/medical/cooking skill seams implemented where existing systems support them
- [ ] needs panel shows top contributors
- [ ] modifier tests pass
- [ ] skill→yield tests pass
- [ ] 30-day balance telemetry accepted

## 24C — Journey

- [ ] survivor journey map published
- [ ] ward admission vacates duty
- [ ] quarantine vacates incompatible duty
- [ ] admission idempotent
- [ ] discharge returns impaired where appropriate
- [ ] recovery ramps data-driven where needed
- [ ] light-duty transitions work
- [ ] sick survivors remain in ration accounting
- [ ] death recalculates future rations
- [ ] unequal feeding uses existing grievance system
- [ ] caregiving consumes labour
- [ ] caregiving creates fatigue
- [ ] diagnosis knowledge controls presentation
- [ ] hidden diagnosis does not remove mechanical effects
- [ ] autopsy seam preserved
- [ ] canonical death integration used
- [ ] death releases duty once
- [ ] death creates memorial once
- [ ] death creates grief through 24B
- [ ] optional mourning has labour/morale tradeoff
- [ ] briefing events emitted
- [ ] medical audio exactly once
- [ ] save/load mid-illness passes
- [ ] save/load mid-recovery passes
- [ ] save/load after death passes
- [ ] 30-day illness determinism passes
- [ ] changed panel snapshots accepted
- [ ] stale docs claims reverified before correction
- [ ] full wave-close verification green

---

# 23. Ship / No-Ship Gate

**SHIP** only if:

```text
fitness_authorities == 1
AND duty_roster_uses_fitness == true
AND expedition_uses_same_fitness == true
AND incapacitated_assignments_accepted == 0
AND impaired_warning_path == true
AND role_requirements_data_driven == true
AND needs_external_modifier_stacks == 1
AND duplicate_need_tracks == 0
AND modifier_attribution_available == true
AND modifier_aggregation_deterministic == true
AND production_skill_authorities == 1
AND apprenticeship_campaign_skill_identity == true
AND trapping_skill_live == true
AND ward_admission_vacates_duty == true
AND quarantine_vacates_duty == true
AND death_releases_duty_once == true
AND death_recalculates_rations == true
AND death_memorial_once == true
AND save_load_survivor_journeys == pass
AND telemetry_30_day == accepted
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 24. Implementer Handoff

1. Land 24A before 24B.
2. Do not add condition fields to duty rows as copied state; derive from authorities.
3. Keep UI text out of Core verdicts.
4. Preserve the ability to intentionally exploit impaired survivors.
5. Put role limits in data.
6. Use one needs modifier stack, not one hook per system.
7. Make every modifier attributable.
8. Reuse existing stress/dependency pathways.
9. Eliminate duplicate skill construction before skill reaches labour.
10. Pass worker identity through a shared work context rather than per-system setters.
11. Treat ward admission, quarantine, and death as labour transitions.
12. Keep diagnosis knowledge separate from mechanical truth.
13. Make death integration idempotent.
14. Persist sources, derive projections.
15. Close Wave 2 with deterministic 30-day telemetry and the full CI suite.

---

# 25. Final Outcome

When this plan is complete, ASHFALL no longer treats the roster and the body as separate facts.

A survivor who has not slept, has been underfed, is cold, irradiated, grieving, withdrawing, or quarantined carries those facts into duty assignment. The player may still choose to spend an impaired person on a dangerous shift—but the game states the cost and remembers the consequence.

Sleep schedules alter actual fatigue instead of a label. Meals, cold, grief, leadership, ration conflict, and overwork move the existing need bars through one deterministic, attributable modifier system. Skills gained through apprenticeship and lost through atrophy are the same skills that determine actual work.

Ward admission vacates a shift. Recovery returns someone gradually, often through light duty. Caregiving consumes another survivor's time and stamina. Death releases obligations, changes ration math, creates grief, and writes the memorial that later continuity systems can read.

The result is not a new survivor simulator. It is the existing survivor, duty, needs, medical, social, skill, shelter, and memorial systems finally behaving as one human ledger.
