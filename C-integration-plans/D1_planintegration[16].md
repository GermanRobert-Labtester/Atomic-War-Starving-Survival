# D1 Flagship Integration Plan [16]
## Plan 193 — Chronic Conditions & Disabilities System

> **Purpose:** Replace ASHFALL's isolated chronic-illness flag and static impairment percentages with one
> authoritative, deterministic, save-aware chronic-condition and accommodation layer that models lasting
> physical, sensory, neurological, respiratory, and cognitive consequences without reducing survivors to
> blanket penalties or duplicating medical, needs, skill, combat, expedition, aging, or psychology state.
>
> **Primary source:** Plan 193 — Chronic Conditions & Disabilities System.
>
> **Core repository problem:** `RadiationSystem.cs` exposes only a boolean `HasChronicIllness`, while
> `MedicalPathologyCatalog.cs` can describe `FunctionalImpairmentPct`; neither provides a general runtime
> authority for persistent conditions, individualized functional effects, accommodations, treatment plans,
> assistive devices, or long-term management. Survivors therefore tend to heal completely or die, with no
> durable middle ground of living with lasting consequences.
>
> **Implementation posture:** medically conservative, deterministic, data-driven, accommodation-first,
> capability-oriented, save-safe, compatible with old saves, and integrated through typed adapters rather than
> by directly mutating every downstream subsystem.
>
> **Critical guardrail:** disability is not a synonym for global incompetence. Conditions should affect specific
> capabilities only where the game has a real mechanical dependency, and accommodations should restore access
> wherever plausible. Avoid generic "social penalty," "work penalty," or "combat penalty" bundles that turn
> disability into a universal debuff.

---

## 1. Source Problem Statement

The source establishes the current gap:

- `RadiationSystem.cs` can set `HasChronicIllness`;
- `MedicalPathologyCatalog.cs` can expose `FunctionalImpairmentPct`;
- no general `ChronicConditionSystem` exists;
- no stable condition instances exist;
- no long-term impairment state exists;
- no accommodation authority exists;
- no assistive-device assignment exists;
- no treatment/management plan exists;
- no persistent capability impact is shared across work, movement, learning, combat, crafting, or expedition.

The target architecture should therefore be:

```text
authoritative trigger
(injury / radiation / disease / age / congenital)
                 ↓
         ChronicConditionSystem
                 ↓
        condition instance
     ┌───────────┼────────────┐
     ↓           ↓            ↓
 severity    progression   diagnosis/visibility
     └───────────┼────────────┘
                 ↓
       functional-effect profile
                 ↓
       accommodation resolver
   ┌─────────────┼──────────────┐
   ↓             ↓              ↓
assistive     treatment      environment
device        plan           adaptation
   └─────────────┼──────────────┘
                 ↓
        effective capabilities
                 ↓
 typed read-only adapters to:
 Needs / Medical / Skills / Work / Combat / Expedition / UI
```

The chronic-condition layer owns persistent condition facts and accommodation assignment. It does not own
survivor health totals, treatment inventory, skill XP, combat math, expedition pathfinding, morale, or
relationship scores.

---

## 2. Flagship Success Criteria

The implementation is complete only when all of the following are true:

1. `ChronicConditionSystem.cs` exists with schema-versioned capture/restore.
2. Conditions have stable instance IDs and stable definition IDs.
3. Condition definitions are data-backed.
4. Disability/accommodation is represented through functional effects rather than one global impairment number.
5. `RadiationSystem.HasChronicIllness` migrates into the new authority without producing duplicate chronic state.
6. `FunctionalImpairmentPct` is either adapted into typed functional effects or explicitly documented as a
   pathology-side input rather than a second chronic-condition authority.
7. Injury, radiation, disease, and age triggers can request condition onset through typed contracts.
8. Congenital/start-of-life conditions are authored or deterministically initialized only where a canonical
   birth/survivor-generation path exists.
9. Onset cannot be rerolled by save/load.
10. Progression is deterministic under the same condition state/treatment inputs.
11. Stable conditions do not tick pointlessly.
12. Progressive conditions update only at meaningful intervals.
13. Treatments route through the medical pipeline.
14. Medication consumption routes through inventory/medical authorities.
15. Surgery uses canonical treatment/risk resolution.
16. Prosthetics and assistive devices are real inventory/equipment/accommodation references if implemented.
17. Environmental accommodations such as ramps are real shelter capability/upgrades, not invisible toggles.
18. Multiple conditions combine through an explicit capability resolver.
19. Combined functional modifiers are bounded and cannot accidentally collapse below zero.
20. Accommodations cannot improve a capability above its normal baseline unless a separate mechanic explicitly
    allows it.
21. Condition state persists through save/load.
22. Accommodation assignments persist through save/load.
23. Old saves with no chronic-condition block remain valid.
24. Old saves with `HasChronicIllness=true` migrate deterministically.
25. No condition is fabricated merely from age unless AgingSystem emits a real onset trigger.
26. UI displays condition, severity, management, functional impact, and accommodation state without duplicating
    business logic.
27. Headless simulation works with no UI.
28. `--chronic-condition-selftest` verifies onset, migration, progression, treatment, accommodation,
    capability resolution, and save round-trip.

---

## 3. Repository Reconnaissance Before Editing

Create:

`docs/chronic_conditions/CHRONIC_CONDITION_INTEGRATION_AUDIT.md`

Inspect at minimum:

- `Assets/Ashfall.Core/Radiation/RadiationSystem.cs`
- `Assets/Ashfall.Core/Narrative/MedicalPathologyCatalog.cs`
- `Assets/Ashfall.Core/Survivors/NeedsSystem.cs`
- `Assets/Ashfall.Core/MedicalPipeline/`
- injury/wound/trauma systems
- disease/infection systems
- aging system from Plan 176 if implemented
- psychology/mental-health systems from Plan 179 if implemented
- memory/cognitive-decline system from Plan 185 if implemented
- survivor skill progression
- work/task scheduling
- expedition movement/travel
- combat accuracy/damage/mobility adapters
- crafting/workshop systems
- shelter accessibility/upgrades
- inventory/equipment system
- prosthetic/assistive-device item definitions if any
- treatment/medication catalogs
- survivor relations/family
- save schema/migration
- event bus / deterministic RNG
- localization
- UI survivor detail
- achievement/quest/epilogue systems

Build an authority matrix:

| Concern | Canonical owner | Existing state | New system role |
|---|---|---|---|
| radiation dose | RadiationSystem | | consume trigger |
| wound severity | injury/medical | | consume trigger |
| disease | Disease/Medical | | consume trigger |
| age | AgingSystem | | consume trigger |
| skill XP | SkillProgression | | modifier adapter only |
| movement | expedition/movement | | capability adapter |
| treatment | MedicalPipeline | | condition plan input |
| item consumption | Inventory | | request only |
| shelter ramp | Shelter upgrades | | capability input |

Do not implement until duplicated chronic/impairment state is identified.

---

## 4. Scope Boundary

### In scope

- chronic-condition definitions;
- persistent condition instances;
- functional-effect profiles;
- severity;
- stable/progressive/manageable/curable trajectories;
- condition onset from authoritative triggers;
- accommodations;
- assistive devices;
- management plans;
- treatment integration;
- capability resolution;
- old-save migration;
- UI;
- journal/quest/achievement hooks;
- CI/selftests.

### Explicitly out of scope for first pass

- diagnosis of real-world medical conditions from player behavior;
- detailed real clinical protocols;
- arbitrary psychotherapy duplication;
- a second wound system;
- a second disease system;
- a second aging system;
- fully simulated rehab schedules;
- punitive "disability morale" defaults;
- automatic social stigma penalties;
- discrimination mechanics;
- cross-campaign disability profile persistence;
- general accessibility advocacy simulation.

The system should create durable consequences while remaining respectful and mechanically legible.

---

## 5. Condition Definition Contract

Create:

`Assets/StreamingAssets/Data/chronic_conditions.json`

Recommended definition:

```csharp
public sealed record ChronicConditionDefinition
{
    public string ConditionDefinitionId { get; init; }
    public string TitleKey { get; init; }
    public string DescriptionKey { get; init; }

    public string CategoryId { get; init; }
    public IReadOnlyList<string> CauseKinds { get; init; }

    public IReadOnlyList<ConditionSeverityDefinition> Severities { get; init; }
    public string ProgressionProfileId { get; init; }

    public IReadOnlyList<string> AccommodationProfileIds { get; init; }
    public IReadOnlyList<string> TreatmentProfileIds { get; init; }

    public IReadOnlyList<string> FunctionalEffectProfileIds { get; init; }
}
```

Condition name is localized data, not a free-form runtime string.

---

## 6. Condition Instance Contract

Recommended:

```csharp
public sealed record ChronicConditionInstance
{
    public string ConditionInstanceId { get; init; }
    public string SurvivorId { get; init; }
    public string ConditionDefinitionId { get; init; }

    public string SeverityId { get; init; }
    public int OnsetDay { get; init; }
    public string CauseKind { get; init; }
    public string? CauseSourceEventId { get; init; }

    public string CourseStateId { get; init; }
    public int ProgressionAccumulator { get; init; }

    public bool Diagnosed { get; init; }
    public int? DiagnosedDay { get; init; }

    public IReadOnlySet<string> ActiveTreatmentPlanIds { get; init; }
    public IReadOnlySet<string> ActiveAccommodationIds { get; init; }

    public bool Resolved { get; init; }
    public int? ResolvedDay { get; init; }
}
```

Do not store raw current capability multipliers inside every condition if they are derivable.

---

## 7. Condition Categories

Retain the source's seven broad categories:

- mobility;
- vision;
- hearing;
- physical;
- cognitive;
- respiratory;
- neurological.

These are content taxonomy.

They must not be used as blanket mechanics such as:
`if category == sensory -> social -30%`.

---

## 8. Specific Condition Target

The source requests 20+ definitions.

Do not author 20 placeholders before the capability/treatment contracts work.

Recommended first tranche:

### Mobility
- persistent limp;
- lower-limb mobility loss;
- chronic joint impairment.

### Vision
- reduced visual acuity;
- partial vision loss;
- blindness.

### Hearing
- hearing loss;
- deafness.

### Physical
- reduced grip/strength;
- chronic pain;
- organ damage.

### Cognitive
- attention/learning impairment;
- cognitive decline only if Plan 185 boundary is clear.

### Respiratory
- chronic respiratory damage;
- reduced respiratory capacity.

### Neurological
- tremor;
- seizure disorder;
- paralysis.

Expand only with real gameplay differentiation.

---

## 9. Terminology and Content Review

Avoid casual or outdated terms in UI/data.

Examples:
- use "hearing aid", not `hearing_aide`;
- avoid "wheelchair-bound"; use "wheelchair user" or "requires wheelchair for mobility";
- avoid "suffers from" as default phrasing;
- avoid "profound" as generic mechanical severity where not necessary.

Internal stable IDs can remain concise, but player-facing localization should be reviewed.

---

## 10. Severity Model

Source suggests:
- mild;
- moderate;
- severe;
- total/profound.

Use one shared severity vocabulary only if it works across definitions.

Some conditions may need:
- partial;
- complete;
- controlled;
- uncontrolled.

Prefer per-condition severity definitions with a normalized ordinal:

```text
severityRank: 0..3
```

UI label can differ.

---

## 11. Functional Effects Instead of Generic Disability DTO

The source proposes a separate `Disability` DTO.

Avoid duplicating the condition.

Recommended:
- `ChronicConditionInstance` is the medical/history state.
- `FunctionalEffect` is the gameplay consequence.
- `AccommodationAssignment` is the mitigation/access state.

A survivor can have one condition that produces several functional effects without creating a second disability
record describing the same fact.

---

## 12. Functional Capability Contract

Recommended typed capabilities:

```text
mobility_ground
mobility_stairs
travel_pace
fine_motor_control
gross_strength
visual_targeting
visual_reading
hearing_detection
spoken_communication
learning_attention
memory_recall
respiratory_endurance
pain_tolerance_task
combat_aim
crafting_precision
```

Only define capabilities consumed by real systems.

Avoid vague:
- `social_interaction`;
- `combat_effectiveness`;
- `work_speed`;

when more precise adapters are possible.

---

## 13. Capability Modifier Contract

Recommended:

```csharp
public sealed record FunctionalEffect
{
    public string CapabilityId { get; init; }
    public int ModifierBasisPoints { get; init; }
    public string EffectKind { get; init; }
    public string SourceConditionInstanceId { get; init; }
}
```

`10000 = baseline`.

Use fixed-point/basis points for deterministic arithmetic where project conventions support it.

---

## 14. Effective Capability Resolver

Create:

```csharp
public interface IFunctionalCapabilityResolver
{
    int GetEffectiveModifierBasisPoints(
        string survivorId,
        string capabilityId);
}
```

Pipeline:

```text
baseline
× condition effects
× treatment effects
× accommodation restoration
= final effective modifier
```

Clamp to configured safe range.

---

## 15. Multiplicative Combination Guardrail

The source says multiple conditions compound multiplicatively.

That is useful, but can collapse values excessively.

Example:
- 0.6 × 0.6 × 0.6 = 0.216.

Add:
- per-capability floor where gameplay requires;
- accommodation restoration;
- non-applicable-effect filtering.

Do not globally floor all capabilities to same value.

---

## 16. Accommodation Semantics

An accommodation is not a magical stat bonus.

It changes access/function in a specific context.

Examples:

- ramp resolves `mobility_stairs` barrier;
- cane improves walking stability;
- wheelchair can restore practical mobility on accessible surfaces;
- hearing aid improves hearing in supported contexts;
- sign-language communication removes spoken-hearing dependency when both sides/system supports;
- prosthetic can restore task capability;
- medication can reduce symptoms/progression.

Prefer capability-specific effects.

---

## 17. Accommodation Definition Contract

```csharp
public sealed record AccommodationDefinition
{
    public string AccommodationDefinitionId { get; init; }
    public string TitleKey { get; init; }
    public string AccommodationTypeId { get; init; }

    public IReadOnlyList<string> ApplicableConditionIds { get; init; }
    public IReadOnlyList<AccommodationEffect> Effects { get; init; }

    public IReadOnlyList<ItemCost> ProvisionCosts { get; init; }
    public string? RequiredShelterCapabilityId { get; init; }

    public string MaintenanceProfileId { get; init; }
}
```

---

## 18. Accommodation Instance/Assignment

```csharp
public sealed record AccommodationAssignment
{
    public string AssignmentId { get; init; }
    public string SurvivorId { get; init; }
    public string AccommodationDefinitionId { get; init; }

    public string? ItemInstanceId { get; init; }
    public string? ShelterCapabilityId { get; init; }

    public int ProvisionDay { get; init; }
    public string StatusId { get; init; }
    public int? LastMaintenanceDay { get; init; }
}
```

If assistive device is a real item, preserve its item instance ID.

---

## 19. Physical Accommodation Categories

Potential:

- cane;
- walker;
- wheelchair;
- ramp/accessibility upgrade;
- prosthetic limb;
- adapted workbench;
- handrail;
- seating/rest access.

Only include items/upgrades actually modeled.

Do not create invisible "wheelchair ramp" if shelter navigation has no accessibility distinction.

---

## 20. Communication Accommodation Categories

Potential:

- hearing aid;
- sign-language communication profile;
- written communication;
- visual alert system.

"Sign language interpreter" should not necessarily consume one survivor permanently.

If the game has no language/communication simulation, model communication access through a capability adapter or
defer the feature rather than inventing fake labor.

---

## 21. Medical/Therapeutic Accommodation Categories

Potential:

- long-term medication;
- respiratory support;
- pain management;
- physical therapy;
- occupational adaptation.

MedicalPipeline owns treatment.

ChronicConditionSystem records active management/adaptation state only.

---

## 22. Maintenance

Not every accommodation needs daily maintenance.

Source says per-day/week costs.

Prefer profiles:

```text
none
periodic_refill
periodic_repair
periodic_clinical_review
```

Example:
- cane: low/no routine maintenance;
- hearing device: battery/repair if such items exist;
- medication: scheduled consumption;
- wheelchair: periodic repair;
- prosthetic: occasional adjustment.

Avoid chore spam.

---

## 23. Maintenance Failure

If maintenance lapses:

- accommodation effectiveness may degrade;
- item may become unavailable;
- UI warns.

Do not instantly worsen the underlying condition unless medically justified by treatment profile.

Separate:
- device not working;
- condition progression.

---

## 24. Condition Cause Contract

Cause kinds:

- injury;
- radiation;
- disease;
- age;
- congenital.

Each onset request must include a real source event where applicable.

Example:

```csharp
public sealed record ChronicConditionOnsetRequest
{
    public string SurvivorId { get; init; }
    public string CauseKind { get; init; }
    public string SourceEventId { get; init; }
    public string TriggerProfileId { get; init; }
    public int TriggerSeverity { get; init; }
}
```

---

## 25. Injury Acquisition

Injury system decides:
- wound severity;
- body region;
- recovery outcome.

ChronicConditionSystem receives a post-acute consequence trigger.

Do not roll permanent disability independently at every combat hit.

Preferred:
```text
severe injury resolution
 -> chronic consequence candidate
```

---

## 26. Body Region Mapping

If wound system supports body regions:

- leg injury can lead to mobility condition;
- eye injury to vision condition;
- hand injury to fine-motor impairment.

If no body region exists:
- use generic injury consequence profiles.

Do not fabricate anatomical precision.

---

## 27. Radiation Acquisition

Migrate:

```text
RadiationSystem.HasChronicIllness
```

Target:
```text
RadiationSystem emits radiation chronic-condition request
```

Radiation remains dose authority.

ChronicConditionSystem selects/instantiates a definition based on validated radiation consequence profile.

---

## 28. Radiation Flag Compatibility

During transition:

```csharp
HasChronicIllness
    => ChronicConditionSystem.HasCauseOrTag(survivorId, "radiation_chronic");
```

Deprecate mutable boolean.

No two independent truths.

---

## 29. Radiation Migration

For old save:
- `HasChronicIllness=false` → no migration.
- `true` → create one deterministic generic radiation-chronic condition compatible with available data.
- onset day unknown → use migration day or known radiation threshold event if saved.
- do not invent multiple organ-specific conditions.

Record migration provenance.

---

## 30. Disease Acquisition

Disease system/MedicalPipeline can emit:

```text
severe disease resolved with sequela risk
```

Condition onset may depend on:
- disease type;
- severity;
- duration;
- treatment outcome.

No generic every-disease roll.

---

## 31. Age Acquisition

AgingSystem owns age thresholds/risk.

It emits:
```text
AgeRelatedConditionRiskEvent
```

ChronicConditionSystem handles condition instance.

Do not poll age and independently create conditions every day.

---

## 32. Congenital Conditions

Only support if survivor birth/generation pipeline can author them coherently.

For shelter-born children:
- Plan 183 may emit a deterministic start-life condition fact.

For generated adult survivors:
- Plan 174/backstory may author congenital condition as part of initial state.

Do not infer from parent conditions automatically.

---

## 33. Inheritance Guardrail

The source says congenital "rare, from parent conditions."

This is too simplistic.

Do not implement direct parent-condition inheritance without an explicit genetics plan.

If congenital conditions exist:
- authored/randomized independently under scenario rules;
- future genetics system may add validated inheritance later.

---

## 34. Onset RNG

Use `ISeededRng` only where probability is necessary.

Seed:
```text
campaignSeed
+ survivorId
+ sourceEventId
+ triggerProfileId
```

Persist result.

Save/load cannot reroll.

---

## 35. Deterministic Trigger Thresholds First

Prefer deterministic onset where the source event already crosses a clear threshold.

Example:
- complete loss of limb from medical resolution → resulting mobility condition;
- confirmed severe radiation syndrome → chronic radiation condition.

Use RNG for uncertain long-term sequelae, not obvious facts.

---

## 36. Diagnosis vs Existence

Separate:

```text
condition exists
condition diagnosed/known
```

Player may see:
- persistent symptom;
- undiagnosed functional issue;
- later formal diagnosis.

Only implement hidden diagnosis if MedicalPipeline already supports diagnosis uncertainty.

Otherwise condition can be known immediately.

---

## 37. Diagnosis Event

Source event:
"The Diagnosis."

Use canonical medical assessment.

Diagnosis may unlock:
- condition name;
- progression expectation;
- treatment options.

Do not gate accommodations that are obvious/needed behind arbitrary diagnosis if that harms gameplay.

---

## 38. Course States

Recommended:

```text
stable
progressive
managed
improving
resolved
```

`permanent` is not mutually exclusive with `managed`.

Avoid a single bool being the only course model.

---

## 39. Progression Profile

```csharp
public sealed record ConditionProgressionProfile
{
    public string ProfileId { get; init; }
    public string ProgressionKind { get; init; }
    public int EvaluationIntervalDays { get; init; }
    public int BaseProgressionPoints { get; init; }

    public IReadOnlyList<string> TreatmentModifiers { get; init; }
    public IReadOnlyList<string> HealthContextModifiers { get; init; }
}
```

Do not store a raw arbitrary `progressionRate per day` if progression is event/band based.

---

## 40. Stable Conditions

Stable:
- no daily random check;
- no silent worsening.

Examples:
- established vision loss;
- stable mobility loss.

Management may improve function without changing underlying condition.

---

## 41. Progressive Conditions

Evaluate on configured intervals.

Inputs:
- current severity;
- treatment adherence;
- health context;
- trigger-specific factors.

Use stable deterministic progression.

No progression notification every day.

---

## 42. Managed Conditions

Managed means:
- progression slowed/paused;
- symptoms/function improved;
- treatment/accommodation active.

Managed does not imply cured.

---

## 43. Curable Conditions

Only definitions explicitly marked curable may resolve.

MedicalPipeline executes cure/treatment resolution.

ChronicConditionSystem marks condition resolved after authoritative result.

Do not allow generic surgery button to cure blindness/paralysis/etc.

---

## 44. Surgery Boundary

Source says surgery can cure some conditions.

Require:
- condition-specific surgical treatment profile;
- real medical capability;
- resources;
- surgeon/skill if applicable;
- canonical risk resolution.

No universal "surgery" action.

---

## 45. Medication Management

Medication profile defines:

- item/medication ID;
- schedule;
- symptom effect;
- progression effect;
- missed-dose behavior.

Inventory/MedicalPipeline consumes medication.

Condition system reads treatment status.

---

## 46. Therapy Management

Therapy may mean:
- physical rehabilitation;
- occupational adaptation;
- respiratory therapy.

Use a generic therapy session contract only if medical scheduler supports it.

Do not conflate psychological therapy from Plan 179 with physical rehabilitation.

---

## 47. Prosthetic Management

Prosthetic is preferably:
- a real assistive device item;
- assigned to survivor;
- may require fitting/maintenance.

It modifies specific capabilities.

It does not delete amputation/history condition.

---

## 48. Wheelchair Semantics

A wheelchair should not mean:
`movement speed = 0.4 everywhere`.

Better:
- accessible flat shelter mobility restored substantially;
- stairs/rough terrain may remain constrained;
- expedition path depends on terrain/accessibility;
- combat use depends on specific encounter system.

This requires contextual capability queries.

---

## 49. Contextual Capability Query

Recommended:

```csharp
GetEffectiveCapability(
    survivorId,
    capabilityId,
    CapabilityContext context);
```

Context may include:
- shelter;
- stairs;
- expedition terrain;
- workshop;
- combat posture.

Do not overbuild contexts before real consumers exist.

---

## 50. Blindness / Vision Loss Semantics

Avoid blanket:
- work -50%;
- social -30%;
- crafting -50%;
- combat -80%.

Instead:
- visual targeting affected;
- reading may need accommodation;
- certain crafting tasks requiring sight affected;
- nonvisual tasks unaffected;
- accessible communication may remain normal.

This is a central design principle.

---

## 51. Deafness / Hearing Loss Semantics

Do not impose generic relationship penalties.

Potential effects:
- auditory detection;
- spoken communication without accommodation;
- radio/audio-only cues.

Accommodations:
- sign language;
- visual alerts;
- hearing device.

Relationships remain normal if communication access exists.

---

## 52. Reduced Strength Semantics

Affects:
- heavy carry;
- heavy work;
- some melee tasks.

Does not necessarily affect:
- learning;
- social;
- precise seated crafting;
- leadership.

Use task capability requirements.

---

## 53. Chronic Pain Semantics

Possible effects:
- endurance;
- concentration under flare;
- rest/medical needs.

Avoid permanent universal productivity penalty.

If flare mechanics do not exist, use bounded stable modifier + management profile.

---

## 54. Cognitive Conditions Boundary

Plan 185 owns memory decay.

Plan 179 may own psychological conditions.

ChronicConditionSystem should not create overlapping cognitive authorities.

Possible integration:
- persistent neurological condition references a cognitive functional-effect profile;
- memory trajectory still owned by Plan 185 where implemented.

Document exactly who owns what.

---

## 55. Seizure Conditions

If seizures are modeled:
- do not simulate medical detail beyond gameplay abstraction;
- use rare episode events through MedicalPipeline/Needs;
- management medication may reduce episode risk.

No per-second seizure RNG.

---

## 56. Respiratory Conditions

Potential impacts:
- expedition endurance;
- heavy labor;
- severe contamination exposure sensitivity.

Do not necessarily affect office/reading/crafting tasks.

Accommodation/treatment:
- medication;
- rest pacing;
- respiratory support if item/system exists.

---

## 57. Capability Registry

Create:

`FunctionalCapabilityRegistry`

Each capability defines:
- ID;
- description;
- consumers;
- baseline 10000;
- legal min/max.

This prevents magic strings across systems.

---

## 58. Task Capability Requirements

Work tasks should declare required capabilities where useful.

Example:

```text
heavy_carry:
  gross_strength
  mobility_ground

precision_repair:
  fine_motor_control
  visual_close_work
```

Do not hardcode condition names into every task.

---

## 59. Work-Speed Integration

Task scheduler computes effective duration from capability requirements.

ConditionSystem does not directly alter all `work_speed`.

This preserves specificity.

---

## 60. Crafting Integration

Crafting may query:
- fine motor;
- visual precision;
- endurance.

Only tasks that depend on those capabilities are affected.

Do not apply blanket crafting-quality penalty to all disabilities.

---

## 61. Skill Learning Integration

SkillProgression queries:
- learning_attention;
- reading_access if relevant.

Accommodation can restore access.

No generic reduced learning for all chronic conditions.

---

## 62. Combat Integration

Combat adapters may query:
- visual targeting;
- hearing detection;
- mobility;
- fine motor;
- respiratory endurance.

CombatSystem owns formulas.

ChronicConditionSystem provides modifiers only.

---

## 63. Expedition Integration

Expedition system may query:
- travel pace;
- terrain accessibility;
- endurance;
- assistive device compatibility.

Do not globally forbid disabled survivors from expeditions.

Route/terrain/transport can change access.

---

## 64. Expedition Accommodation

Examples:
- vehicle transport;
- accessible route;
- companion assistance;
- wheelchair-suitable route.

Only implement where travel system supports these.

Otherwise use clear restrictions/alternatives, not invisible penalties.

---

## 65. Needs Integration

Conditions may alter needs:

- pain management;
- rest;
- medication;
- respiratory support.

NeedsSystem remains authority.

ConditionSystem exposes active need-profile modifiers.

---

## 66. No Automatic Morale Penalty

Do not make every disability reduce morale.

If survivor struggles due to:
- unmanaged pain;
- inaccessible environment;
- acute decline;

Needs/MentalHealth can receive contextual effects.

Well-accommodated survivors should not carry a permanent "sad because disabled" debuff.

---

## 67. Social Interaction Guardrail

The source proposes social penalties.

Reject blanket social penalties.

A communication barrier may affect specific interactions until accommodation exists.

RelationsSystem should not reduce trust/friendship merely because a survivor is disabled.

---

## 68. Accessibility as Shelter Capability

Environmental access should be represented by real shelter upgrades/capabilities:

- ramp;
- accessible route;
- adapted workstation;
- visual alarm;
- quiet treatment space.

If shelter pathing does not model stairs/doors, keep v1 to equipment/treatment accommodations.

---

## 69. Accommodation Assignment Flow

```text
select survivor/condition
 -> list applicable accommodations
 -> validate item/facility/treatment
 -> provision transaction
 -> create assignment
 -> recalculate effective capabilities
 -> emit AccommodationProvided
```

No direct UI stat mutation.

---

## 70. Accommodation Removal

On removal/failure:
- assignment status changes;
- device returned/consumed according to inventory rules;
- capability recalculated.

Underlying condition remains.

---

## 71. Accommodation Effectiveness

Source suggests 0–100.

Use data-backed restoration amounts.

Example:
- cane may partially improve stability;
- adapted ramp may fully remove stair barrier at that location.

Do not reduce all accommodations to one generic percentage.

---

## 72. Treatment vs Accommodation

Keep distinct:

### Treatment
Targets symptoms/course.

### Accommodation
Changes access/function/environment.

Example:
- medication reduces tremor severity;
- adapted tools improve fine-motor task access.

Both can coexist.

---

## 73. Adaptation Event

Source event:
"The Adaptation."

This should not mean "the disability stops mattering."

It may mean:
- successful accommodation;
- survivor learns alternative method;
- work role adapts;
- assistive device proficiency.

Represent as a positive contextual milestone, not erasure.

---

## 74. Adaptation Mechanics

Optional:
- after sustained successful use, accommodation reliability/effect may improve modestly;
- or unlock adapted task preference.

Only if the project has mastery/adaptation architecture.

V1 can keep adaptation narrative-only.

---

## 75. Condition Onset Event

Source:
"The Injury."

Better taxonomy:
- condition onset from injury;
- condition onset from radiation;
- condition onset from disease;
- age-related onset.

UI notification should name cause only if known.

---

## 76. Progression Event

Notify only on meaningful severity/course change.

Do not notify daily progression accumulator.

Example:
"Respiratory damage worsened from moderate to severe."

---

## 77. Treatment Event

Record:
- treatment started;
- treatment outcome;
- management stabilized.

Medical journal/pipeline may already store clinical events.

Avoid duplicate logs; chronic system can link to treatment event ID.

---

## 78. Cure/Resolution Event

If condition resolves:
- persist historical condition record;
- mark resolved;
- remove active functional effects;
- retain provenance.

Do not delete history.

---

## 79. Condition History

Per condition retain only major events:

- onset;
- diagnosis;
- severity change;
- accommodation;
- major treatment;
- resolution.

Do not store every daily pill.

---

## 80. Condition Log UI

Survivor detail can show:

```text
Day 43 — Persistent leg impairment began after combat injury.
Day 45 — Cane provided.
Day 76 — Adapted workshop station installed.
```

Only if events actually exist.

Use stable references/localization.

---

## 81. Survivor Detail Projection

Recommended:

```csharp
public sealed record ChronicConditionPanelModel
{
    public string SurvivorId { get; init; }
    public IReadOnlyList<ConditionView> ActiveConditions { get; init; }
    public IReadOnlyList<AccommodationView> Accommodations { get; init; }
    public IReadOnlyList<FunctionalImpactView> FunctionalImpacts { get; init; }
    public IReadOnlyList<ManagementView> ManagementPlans { get; init; }
}
```

UI receives effective impacts, not raw formulas.

---

## 82. Condition Detail

Show:

- condition name;
- severity/course;
- onset cause/day;
- affected capabilities;
- current treatment;
- accommodations;
- next management need;
- prognosis only if system supports it.

Avoid medical claims not backed by data.

---

## 83. Functional Impact Display

Prefer:

```text
Travel pace: moderately reduced
Heavy lifting: reduced
Reading: unaffected
Precision work: unaffected
```

over:
`Overall effectiveness: 63%`.

This is clearer and less reductive.

---

## 84. Accommodation Panel

Show:
- available options;
- required items/facility;
- expected capability change;
- maintenance;
- whether already provided.

No hidden resource drain.

---

## 85. Accessibility of UI

Support:
- keyboard/controller;
- text scaling;
- severity not color-only;
- concise descriptive labels;
- no critical data only in icons;
- screen-reader-friendly text where framework supports;
- no flashing seizure-risk effects.

Given the subject matter, UI accessibility is especially important.

---

## 86. Tutorial

First chronic-condition onset:

Explain:
- condition may be stable or progressive;
- accommodations can restore function/access;
- treatment and accommodation are different;
- survivor capabilities remain individual.

Avoid framing:
"this survivor is now permanently worse."

---

## 87. Tooltips

Condition tooltip:
- affected functions;
- active management;
- progression status.

Accommodation tooltip:
- what access/function it changes;
- maintenance.

No opaque "effectiveness 72%" without explanation.

---

## 88. Quest Hooks

Source hooks:

- The Doctor;
- The Caregiver;
- The Survivor;
- The Cure;
- The Management;
- The Adaptation;
- The Care.

Export facts/events.

QuestSystem owns progress/rewards.

---

## 89. Quest Language Guardrail

Avoid quests that treat disabled survivors as inspirational props.

"The Survivor — survivor with disability completes major task" can be reframed around access/adaptation rather
than "despite disability."

Use neutral, character-focused writing.

---

## 90. Achievement Integration

Plan 149 may observe:
- condition effectively managed;
- accommodation provided;
- accessible shelter upgrades;
- long-term stable management.

No achievement should reward intentionally causing disability.

---

## 91. Epilogue Integration

Plan 145 may consume:
- survivor lived with chronic condition;
- accommodation/adapted role;
- long-term treatment stability;
- major achievements.

Do not make disability automatically tragic or heroic.

Export structural facts only.

---

## 92. Backstory Integration

Plan 174 may initialize an authored/pre-existing condition.

Rules:
- same condition system;
- stable onset cause may be `pre_campaign`/congenital;
- no separate backstory impairment state.

Starting accommodations may be authored too.

---

## 93. Aging Integration

Plan 176 can emit age-related onset candidates.

AgingSystem remains age authority.

ChronicConditionSystem owns the resulting condition once created.

---

## 94. Child Development Integration

Plan 183 can consume functional capabilities for age-appropriate support.

Do not assume a child with disability follows a different chronological development stage.

Education/access accommodations should remain available.

---

## 95. Psychology Boundary

Plan 179 owns psychological conditions.

ChronicConditionSystem may coexist with:
- pain-related stress;
- adaptation;
- treatment burden.

It must not duplicate depression/anxiety/PTSD diagnoses.

---

## 96. Memory/Cognitive Boundary

Plan 185 owns memory decay trajectory if implemented.

ChronicConditionSystem may provide:
- neurological cause;
- functional access profile.

One system must own memory metric.

Document the adapter.

---

## 97. Radiation Mutation Boundary

Plan 172 owns mutations.

A mutation can trigger or coexist with chronic functional condition.

Do not encode mutation appearance/body state here.

---

## 98. Medical Pathology Integration

`FunctionalImpairmentPct` may become:

- an acute pathology severity input;
- a trigger to chronic consequence evaluation;
- or a compatibility field mapped to one functional profile.

Do not persist both pathology impairment and chronic impairment as separate final multipliers without explicit
composition.

---

## 99. Pathology-to-Chronic Handoff

Recommended:

```text
acute pathology resolves
 -> MedicalPipeline computes sequela outcome
 -> ChronicConditionOnsetRequest
 -> chronic system creates lasting state
```

This is the seam between acute and chronic care.

---

## 100. Acute vs Chronic Distinction

Acute:
- wound;
- infection;
- temporary pain;
- temporary respiratory issue.

Chronic:
- long-term/persistent state after acute phase;
- ongoing functional/access/management implications.

Do not promote every injury to chronic status.

---

## 101. Condition Acquisition Probability

For uncertain sequelae:

```text
risk =
  trigger severity
  + duration
  + treatment outcome
  + survivor context
```

Use data profile + deterministic seed.

No arbitrary global 10% roll.

---

## 102. Onset Idempotency

Stable key:

```text
chronic_onset:<survivorId>:<sourceEventId>:<conditionDefinitionId>
```

Same acute event cannot create duplicate condition instance.

---

## 103. Duplicate Condition Policy

A survivor may not need two identical stable conditions.

Definition can specify:
- merge severity;
- coexist;
- reject duplicate.

Example:
- repeated hearing damage may worsen existing hearing-loss condition rather than create two.

---

## 104. Bilateral/Laterality

Do not add left/right body-side complexity unless medical/injury system already tracks it.

V1 can model functional outcome at sufficient abstraction.

Avoid fake anatomical detail.

---

## 105. Condition Progression Interval

Use day/week event, not per-frame.

Examples:
- daily for unstable respiratory issue;
- weekly for chronic degenerative process;
- no tick for stable impairment.

Store next evaluation day if needed.

---

## 106. Progression Idempotency

Each interval has stable evaluation ID:

```text
condition_progress:<instanceId>:<evaluationDay>
```

Reload cannot reroll.

---

## 107. Treatment Adherence

If medication schedule exists:

MedicalPipeline reports adherence/compliance facts.

Do not scan inventory and assume possession means treatment taken.

This distinction prevents free management.

---

## 108. Resource Scarcity

If treatment unavailable:

- condition may remain stable/unmanaged;
- progressive condition may worsen according to profile;
- accommodations can still provide access.

Do not punish every missed dose with immediate severity increase.

---

## 109. Accommodation Without Cure

A well-accommodated survivor can have near-normal access for specific tasks while condition remains unchanged.

This is desirable.

UI must communicate:
- underlying condition;
- effective function with accommodation.

---

## 110. Accommodation Overlap

Two accommodations may target same capability.

Resolver must define stacking:

- best applicable;
- multiplicative restoration;
- capped additive restoration.

Avoid >100% accidental overshoot.

---

## 111. Accommodation Compatibility

Some pairs incompatible:
- two devices occupying same equipment slot;
- alternative mobility aids.

Definition includes exclusive-group ID if needed.

Validator enforces.

---

## 112. Accommodation Item Durability

If device is a real item with condition:
- degraded item may reduce effectiveness.

Item system remains durability authority.

Chronic system reads status.

---

## 113. Shelter Access Graph — Optional

If shelter has navigation graph/stairs:

- ramps/lifts can modify accessible edges;
- mobility capability determines traversability.

This is powerful but should be separate phase.

Do not fake it with work-speed penalties if navigation system cannot model access.

---

## 114. Accessible Workstation

Adapted workstation can remove:
- standing requirement;
- fine-motor barrier where appropriate.

Work/crafting task declares environment capability.

This creates strategic shelter accommodation.

---

## 115. Visual Alerts

If alarms are audio-only:
- visual alert accommodation can provide equal warning.

This is a better hearing-related mechanic than arbitrary social penalties.

Only implement if alarm system exposes modality.

---

## 116. Communication Alternatives

If game has conversation actions:
- sign/written communication can preserve interaction access.

Do not block relationships because spoken hearing is reduced.

If no communication modality system exists, keep hearing effects to detection-specific mechanics.

---

## 117. Combat Accessibility

Do not assume all conditions make combat impossible.

Examples:
- deaf survivor may fight normally but auditory detection differs;
- wheelchair user may fight in accessible static defense position;
- vision loss may affect ranged aiming but not every defensive/support role.

Combat system should query specific capabilities.

---

## 118. Expedition Accessibility

Allow route-level compatibility:

```text
terrain_rough
stairs
vehicle_available
distance
```

A mobility condition may restrict some routes, not all expeditions.

If route system lacks this, use travel pace/endurance only.

---

## 119. Skill Learning Accessibility

Blindness should not universally reduce learning.

Reading-heavy curriculum may require:
- accessible material;
- teacher.

Audio/oral curriculum may be unaffected.

Again: capability/context, not category-wide penalty.

---

## 120. Crafting Accessibility

Fine-motor tremor may affect precision recipes.

Reduced mobility may not affect seated electronics repair if workstation accessible.

Recipe/task capabilities should decide.

---

## 121. Social Accessibility

Communication accommodations can restore interaction.

Do not encode disability as charisma loss.

This is non-negotiable architecture/design guidance.

---

## 122. Needs / Care Load

Some chronic conditions create periodic medical-care load.

Represent:
- treatment schedule;
- maintenance;
- rest needs.

Do not create a global "disabled survivor requires X caregiver hours" rule.

---

## 123. Caregiver Integration

Only survivors who actually need assistance should generate care tasks.

Examples:
- total mobility support;
- severe neurological episodes.

Care tasks route through scheduler.

No blanket caregiver assignment.

---

## 124. Independent Living

Well-accommodated survivor may require no caregiver.

The game should reflect that.

This prevents the system from turning accommodations into permanent labor tax.

---

## 125. Condition Privacy / Knowledge

If game has secret medical info:
- diagnosis visibility may matter.

Otherwise, survivor detail can show condition openly.

Do not add privacy mechanics in v1 without supporting systems.

---

## 126. Notification Policy

Notify:
- new chronic condition;
- meaningful progression;
- accommodation provided/fails;
- treatment stabilizes/resolves.

Do not notify:
- every modifier recalculation;
- every maintenance tick.

---

## 127. Journal Integration

Journal significant:
- onset after major injury;
- adaptation/accommodation milestone;
- cure/resolution;
- major accessible achievement if narratively relevant.

Medical log may hold detailed progression.

Avoid duplicate logs.

---

## 128. Archive Integration

ShelterArchive may retain:
- major adaptation;
- pioneering prosthetic/medical treatment;
- notable survivor legacy.

Condition system exports facts only.

---

## 129. Modding Integration

If Plan 165 exists, candidate public contracts:

- new condition definitions;
- progression profiles;
- accommodation definitions;
- treatment-profile references.

Mods cannot inject executable modifier formulas.

All capabilities/rule kinds must be registered.

---

## 130. Modded Condition Save Safety

If mod removed:
- save may contain condition ID no longer defined;
- Plan 165 compatibility reports risk.

Do not silently delete a survivor's condition.

Historical/inert placeholder may be possible only under save-compatibility policy.

---

## 131. Old-Save Migration — No Conditions

If save predates system and radiation flag absent/false:

- empty chronic state;
- no accommodations;
- no retroactive age/disease/injury conditions;
- no notification flood.

Future events begin normal tracking.

---

## 132. Old-Save Migration — Radiation Flag

If `HasChronicIllness=true`:

1. create deterministic migrated condition;
2. cause = radiation;
3. onset provenance = legacy migration;
4. choose conservative severity based on any saved radiation context;
5. if no context, use baseline configured severity;
6. mark diagnosis known if old UI already exposed chronic illness;
7. do not roll again;
8. persist immediately.

Test exact round-trip.

---

## 133. Old-Save `FunctionalImpairmentPct`

Do not automatically convert every pathology impairment into chronic state.

Only migrate if:
- pathology is persisted as chronic/long-term;
- medical authority says sequela remains.

Otherwise it may have been acute/temporary.

---

## 134. Migration Provenance

Condition instance can store:

```text
migration_radiation_flag
migration_existing_pathology
migration_authored
```

Useful for diagnostics.

Not normally shown to player.

---

## 135. Save Schema

Persist:

- condition instances;
- severity/course;
- onset/cause;
- diagnosis state;
- active treatment references;
- accommodation assignments;
- progression accumulator/next check;
- resolved history if retention policy includes it;
- idempotency markers.

Do not persist duplicated skill/work/combat modifiers.

---

## 136. Save/Load During Treatment

Test:

1. progressive condition;
2. treatment started;
3. medication consumed;
4. save;
5. reload;
6. next progression evaluation.

Expected:
- no duplicate medication consumption;
- no reroll;
- same severity/course.

---

## 137. Save/Load During Accommodation Provision

If device assignment is transaction-based:

- provision commits once;
- reload does not consume second item;
- assignment resolves same item instance;
- effective capability restored once.

---

## 138. Save/Load During Surgery

Surgery/treatment result belongs to MedicalPipeline.

Persist treatment episode/seed before outcome if multi-step.

ChronicConditionSystem consumes final result once.

---

## 139. Stable IDs

Condition:

```text
condition:<survivorId>:<sourceEventId>:<definitionId>
```

Accommodation:

```text
accommodation:<survivorId>:<definitionId>:<sequence>
```

Progression event:

```text
condition_progress:<conditionInstanceId>:<evaluationKey>
```

---

## 140. Data Catalog Structure

Suggested `chronic_conditions.json`:

```json
{
  "schemaVersion": 1,
  "categories": [],
  "conditions": [],
  "severityProfiles": [],
  "progressionProfiles": [],
  "functionalEffectProfiles": [],
  "accommodations": [],
  "maintenanceProfiles": [],
  "treatmentBindings": []
}
```

---

## 141. Catalog Validation

Validate:

- unique condition IDs;
- category IDs;
- severity ordering;
- effect capability IDs;
- modifier bounds;
- progression interval;
- accommodation references;
- treatment references;
- item IDs;
- shelter capability IDs;
- localization keys;
- exclusive groups;
- no invalid cure/profile combination.

---

## 142. 20+ Condition Authoring Gate

Every condition definition must answer:

1. What authoritative cause can create it?
2. Which specific capabilities does it affect?
3. Is it stable/progressive?
4. What treatment can manage it?
5. Which accommodations are meaningful?
6. Does it overlap another plan/system?
7. Is there at least one gameplay consumer?
8. Is UI language appropriate?

Reject filler definitions.

---

## 143. Condition Coverage Report

Generate:

`docs/chronic_conditions/CHRONIC_CONDITION_COVERAGE.md`

| Condition | Cause | Functional effects | Treatment | Accommodation | Trigger fixture |
|---|---|---|---|---|---|
| | | | | | |

Flag:
- unreachable definitions;
- definitions with no consumer;
- definitions with generic global penalties.

---

## 144. Capability Coverage Report

Generate:

`docs/chronic_conditions/FUNCTIONAL_CAPABILITY_COVERAGE.md`

| Capability | Consumers | Conditions affecting | Accommodations restoring | Tests |
|---|---|---|---|---|
| | | | | |

This prevents dead capability metadata.

---

## 145. Accommodation Coverage Report

Generate:

`docs/chronic_conditions/ACCOMMODATION_COVERAGE.md`

For each accommodation:
- real item/facility?
- condition applicability?
- maintenance?
- capability effect?
- UI?
- test?

No decorative-only accommodations pretending to matter.

---

## 146. Progression Calibration

Generate:

`docs/chronic_conditions/CONDITION_PROGRESSION_CALIBRATION.md`

Simulate:
- untreated;
- partially managed;
- well managed.

Measure:
- time to severity changes;
- resource consumption;
- effective capability.

Reject rapid unavoidable deterioration that makes conditions a death spiral.

---

## 147. Accommodation Balance

Accommodation should meaningfully restore access.

Targets:
- low-cost aids provide useful partial restoration;
- expensive prosthetics/adaptations provide stronger restoration;
- no accommodation becomes mandatory for every survivor;
- maintenance does not dominate resource economy.

---

## 148. Treatment Burden Balance

Long-term management should be strategic, not repetitive.

Prefer:
- scheduled resource consumption;
- periodic maintenance;
- batch medical planning.

Avoid dozens of daily confirmation clicks.

---

## 149. Multiple Conditions

Resolver example:

Survivor:
- reduced vision;
- respiratory damage.

Task:
- precision shooting.

Relevant:
- visual targeting;
- respiratory endurance.

Task:
- writing.

Relevant:
- visual reading, maybe none if accommodated.

Do not multiply all condition modifiers into every action.

---

## 150. Compound Effects Test

Test 3–5 conditions on one survivor.

Ensure:
- no NaN/negative;
- contextual tasks use only relevant capabilities;
- accommodation restoration capped;
- UI explains major contributors.

---

## 151. Functional Effect Explainability

Developer projection:

```text
visual_targeting:
 baseline 100%
 partial_vision_loss x 70%
 corrective_access x 110% restoration capped to 95%
 final 95%
```

Player UI can simplify.

This is critical for debugging.

---

## 152. Healthy Survivor Parity

A survivor with no conditions/accommodations must behave identically to pre-Plan-193 baseline.

Regression compare:
- work;
- skills;
- combat;
- expedition;
- crafting.

No global hook drift.

---

## 153. Fully Accommodated Parity

Where an accommodation is defined as full access:

- affected task may return to baseline;
- condition remains visible/history preserved.

This is expected, not an exploit.

---

## 154. No-Accommodation Edge Case

Survivor remains usable in unaffected roles.

UI should surface:
- which tasks are impaired;
- alternative roles.

Do not hard-disable entire survivor.

---

## 155. Bedridden / Total Mobility Edge Case

If a condition genuinely prevents independent mobility:

- medical/rest tasks;
- communication;
- decision-making;
- certain seated/bed activities may remain possible.

Do not equate mobility loss with total incapacity.

---

## 156. Severe Vision Loss Edge Case

Allow:
- nonvisual roles;
- adapted tasks;
- communication;
- leadership if system allows.

Combat targeting may be restricted.

Again, task-specific.

---

## 157. Deaf Survivor Edge Case

With communication accommodation:
- normal relationships/interactions;
- auditory detection still context-specific.

No permanent social debuff.

---

## 158. Chronic Pain Edge Case

Pain management can improve endurance/concentration.

Do not model pain as one universal percentage.

If no flare system exists, keep stable bounded profile.

---

## 159. Respiratory Damage Edge Case

Heavy travel/physical work affected.

Sedentary tasks may be normal.

Medical management can stabilize.

Test expedition pace separately.

---

## 160. Neurological Condition Edge Case

Tremor:
- precision tasks.

Seizure disorder:
- episodic risk/medical management.

Paralysis:
- mobility/access.

Do not bundle these under one neurological global modifier.

---

## 161. Condition Death Relationship

A chronic condition may contribute to death only through:
- MedicalPipeline/health system;
- terminal complication event.

ChronicConditionSystem itself should not arbitrarily kill survivor on severity tick unless it is the designated
medical authority (unlikely).

Use handoff.

---

## 162. Condition Resolution on Death

When survivor dies:
- active conditions become historical/inactive;
- accommodations released/returned according to item rules;
- medical schedule cancelled;
- epilogue/archive can still reference.

Do not delete condition history prematurely.

---

## 163. Accommodation Item Reclamation

On death/departure:
- reusable cane/wheelchair/prosthetic handling follows inventory/equipment policy;
- personal fitted devices may or may not be reusable.

Do not duplicate items.

---

## 164. Departure / Retirement

If survivor leaves:
- cancel local treatment schedules;
- return shelter-owned devices if appropriate;
- preserve historical condition state if survivor archive needs it.

External simulation not required.

---

## 165. UI Snapshot Cases

Capture:

1. no conditions;
2. one stable mobility condition;
3. progressive respiratory condition;
4. fully accommodated hearing loss;
5. multiple conditions;
6. treatment missing resource;
7. condition improved/stabilized;
8. resolved condition history;
9. high text scale;
10. keyboard/controller focus.

---

## 166. Structured Diagnostics

Logs:

```text
ChronicConditionOnset survivor=<id> condition=<id> source=<event>
ChronicConditionProgressed instance=<id> from=<severity> to=<severity>
ChronicConditionManaged instance=<id> course=<state>
AccommodationAssigned survivor=<id> accommodation=<id>
AccommodationMaintenanceChanged assignment=<id> status=<state>
ChronicConditionResolved instance=<id>
```

Avoid routine daily logs.

---

## 167. Dedicated `--chronic-condition-selftest`

Selftest should:

1. load catalog;
2. verify all conditions/capabilities/accommodations;
3. create healthy survivor and verify baseline parity;
4. trigger injury-based condition;
5. replay onset and verify idempotency;
6. verify specific functional effect;
7. assign accommodation;
8. verify contextual restoration;
9. start treatment;
10. progress condition deterministically;
11. save/reload;
12. verify no duplicate treatment/resource use;
13. test multiple conditions;
14. test radiation legacy migration;
15. test `HasChronicIllness` compatibility projection;
16. test resolved condition;
17. verify UI projection contains no formula logic;
18. verify headless consumers;
19. exit non-zero on mismatch.

---

## 168. Unit Test Matrix

### Catalog
- valid;
- duplicate ID;
- invalid capability;
- invalid item;
- invalid treatment;
- invalid accommodation.

### Onset
- injury;
- radiation;
- disease;
- age;
- congenital/authored;
- duplicate event.

### Progression
- stable;
- progressive;
- managed;
- improving;
- resolved.

### Capability
- one condition;
- multiple;
- irrelevant capability unaffected;
- accommodation;
- cap/floor.

### Management
- medication;
- therapy;
- device;
- maintenance;
- surgery resolution.

### Migration
- no old condition;
- radiation false;
- radiation true;
- existing pathology case.

### Persistence
- active;
- managed;
- accommodation;
- resolved.

---

## 169. Golden Scenarios

1. Severe leg injury leads to persistent mobility impairment.
2. Cane improves flat-ground mobility.
3. Ramp removes shelter stair access barrier if path system supports it.
4. Hearing loss with visual/sign accommodation preserves interaction.
5. Partial vision loss affects ranged targeting but not unrelated role.
6. Respiratory damage slows long expedition but not desk-like work.
7. Tremor affects precision crafting but not social task.
8. Progressive condition stabilized by treatment.
9. Surgery resolves one specifically curable condition.
10. Old radiation flag migrates to one condition.
11. Survivor with three conditions remains usable in unaffected roles.
12. Fully accommodated survivor reaches near-baseline in intended task.

---

## 170. Property / Fuzz Testing

Properties:

- modifier values remain bounded;
- condition instance IDs unique;
- same source onset event produces at most one instance;
- stable conditions do not progress spontaneously;
- resolved conditions emit no active effects;
- accommodation cannot apply to invalid condition/capability;
- same state produces same capability result;
- no condition changes canonical skill XP/health directly.

---

## 171. Progression Fuzz

Randomize valid:
- severity;
- management;
- progression profile;
- evaluation intervals.

Assert:
- severity never jumps outside defined states;
- no backward progression unless profile explicitly permits improvement;
- same seed/result deterministic;
- managed profile matches bounds.

---

## 172. Performance Budget

The system is low-frequency.

Requirements:
- no per-frame condition scan;
- stable conditions require no scheduled work;
- progressive conditions indexed by next evaluation day;
- capability resolver caches until relevant state changes;
- accommodation changes invalidate only affected survivor.

100 survivors with multiple conditions must remain cheap.

---

## 173. Save Footprint

Persist only condition/accommodation state.

Do not store per-task calculated modifiers.

Resolved historical conditions may be compacted if long campaigns create many records.

---

## 174. Resolved Condition Retention

Keep:
- condition ID;
- onset;
- cause;
- resolution day;
- notable treatment.

Drop:
- obsolete progression accumulator;
- expired maintenance details.

Archive/epilogue can still reference.

---

## 175. Implementation Phase A — Authority Audit

Tasks:

1. map radiation flag;
2. map pathology impairment;
3. map injury/disease/age triggers;
4. map downstream capability consumers;
5. map treatment/inventory;
6. document boundaries.

Exit:
no ambiguity about who owns chronic facts.

---

## 176. Implementation Phase B — Condition Core

Tasks:

1. condition DTO/state;
2. stable IDs;
3. catalog;
4. loader;
5. validator;
6. onset request;
7. capture/restore;
8. no-condition parity tests.

Exit:
one condition can exist persistently without downstream effects.

---

## 177. Implementation Phase C — Capability Resolver

Tasks:

1. capability registry;
2. functional effects;
3. combination rules;
4. caps/floors;
5. contextual queries;
6. explainability diagnostics;
7. work/skill/combat/expedition adapters.

Exit:
specific conditions affect only relevant mechanics.

---

## 178. Implementation Phase D — Accommodations

Tasks:

1. definition/assignment;
2. item/facility bindings;
3. provision transaction;
4. restoration effects;
5. maintenance profiles;
6. removal/failure;
7. tests.

Exit:
access can be restored through meaningful accommodations.

---

## 179. Implementation Phase E — Management/Progression

Tasks:

1. progression profiles;
2. evaluation schedule;
3. medication binding;
4. therapy binding;
5. surgery resolution;
6. managed/stable/improving states;
7. notifications;
8. tests.

Exit:
long-term conditions can be managed without daily spam.

---

## 180. Implementation Phase F — Trigger Integrations

Tasks:

1. injury;
2. radiation;
3. disease;
4. age;
5. authored/congenital initialization;
6. source-event idempotency;
7. tests.

Exit:
real gameplay consequences create chronic state.

---

## 181. Implementation Phase G — Legacy Migration

Tasks:

1. migrate `HasChronicIllness`;
2. compatibility projection;
3. review `FunctionalImpairmentPct`;
4. no-condition old saves;
5. migration provenance;
6. fixtures.

Exit:
old campaigns load without duplicate or fabricated state.

---

## 182. Implementation Phase H — UI

Tasks:

1. survivor condition list;
2. condition detail;
3. functional impact;
4. accommodation panel;
5. treatment/management link;
6. history;
7. accessibility;
8. snapshots.

Exit:
UI contains no chronic-condition formulas.

---

## 183. Implementation Phase I — Narrative Hooks

Tasks:

1. onset/progression/adaptation events;
2. journal;
3. quest facts;
4. achievement facts;
5. archive;
6. epilogue facts;
7. respectful language review.

Exit:
conditions contribute character depth without stereotype.

---

## 184. Implementation Phase J — 20+ Definitions

Only after foundation is stable:

1. author first 8–10 high-confidence conditions;
2. validate mechanical differentiation;
3. validate treatments/accommodations;
4. expand to 20+;
5. coverage report;
6. remove filler/overlap.

Exit:
20+ meaningful, reachable definitions.

---

## 185. Implementation Phase K — CI Hardening

Tasks:

1. selftest;
2. data integrity;
3. fuzz;
4. progression calibration;
5. accommodation coverage;
6. multiple-condition stress;
7. old-save migration;
8. headless parity;
9. UI snapshots;
10. full regression.

Exit:
system is deterministic and supportable.

---

## 186. Exact File Plan

Expected new/modified files, adjusted to repository conventions:

### Core
- `Assets/Ashfall.Core/Medical/ChronicConditionSystem.cs`
- `Assets/Ashfall.Core/Medical/ChronicConditionState.cs`
- `Assets/Ashfall.Core/Medical/ChronicConditionInstance.cs`
- `Assets/Ashfall.Core/Medical/ChronicConditionDefinition.cs`
- `Assets/Ashfall.Core/Medical/FunctionalCapabilityRegistry.cs`
- `Assets/Ashfall.Core/Medical/FunctionalCapabilityResolver.cs`
- `Assets/Ashfall.Core/Medical/AccommodationDefinition.cs`
- `Assets/Ashfall.Core/Medical/AccommodationAssignment.cs`
- `Assets/Ashfall.Core/Medical/ChronicConditionEvents.cs`

### Data
- `Assets/StreamingAssets/Data/chronic_conditions.json`

### Tests
- `Ashfall.Core.Tests/Medical/ChronicConditionSystemTests.cs`
- `Ashfall.Core.Tests/Medical/ChronicConditionMigrationTests.cs`
- `Ashfall.Core.Tests/Medical/FunctionalCapabilityResolverTests.cs`
- `Ashfall.Core.Tests/Medical/AccommodationTests.cs`
- `Ashfall.Core.Tests/Medical/ChronicConditionIntegrationTests.cs`

### Docs
- `docs/chronic_conditions/CHRONIC_CONDITION_INTEGRATION_AUDIT.md`
- `docs/chronic_conditions/CHRONIC_CONDITION_COVERAGE.md`
- `docs/chronic_conditions/FUNCTIONAL_CAPABILITY_COVERAGE.md`
- `docs/chronic_conditions/ACCOMMODATION_COVERAGE.md`
- `docs/chronic_conditions/CONDITION_PROGRESSION_CALIBRATION.md`
- `docs/chronic_conditions/CHRONIC_CONDITION_MIGRATION.md`

---

## 187. Bootstrap / Composition Root

Recommended order:

```text
load chronic condition catalog
validate capabilities/accommodations/treatments
construct MedicalPipeline
construct Radiation/Disease/Injury/Aging authorities
construct ChronicConditionSystem
restore survivor/medical state
restore ChronicConditionState
wire onset/treatment/accommodation events
construct capability adapters
bind UI projection
```

Restore order must avoid evaluating downstream work/combat modifiers before chronic state is restored.

---

## 188. Event Contracts

Recommended:

```csharp
ChronicConditionOnsetRequested
ChronicConditionAdded
ChronicConditionDiagnosed
ChronicConditionSeverityChanged
ChronicConditionManaged
AccommodationProvided
AccommodationStatusChanged
ChronicConditionResolved
```

Typed IDs only.

No reflection-driven event dispatch from JSON.

---

## 189. No Generic Per-Frame Tick

The source proposes `TickChronicConditions`.

Prefer:
- progression scheduler/day event;
- treatment event;
- accommodation event;
- onset event.

A generic frame tick is unnecessary.

---

## 190. Condition Management Settings

If player settings exist, use high-level automation:

```text
Long-term treatment:
[ Manual approval ]
[ Maintain if resources available ]
```

Do not expose obscure per-condition toggles unless needed.

Automation must respect medical resource reserves.

---

## 191. Medication Reserve Safety

Auto-management should not consume:
- last emergency medicine;
- quest-critical item;
- incompatible medication.

MedicalPipeline owns reserve policy.

ConditionSystem requests treatment continuation.

---

## 192. Surgery Risk Transparency

If surgery is available:
- show risk;
- success/failure outcomes;
- resources;
- surgeon requirement.

Do not hide condition cure chance behind lore text.

---

## 193. Condition Exploit Prevention

The source says conditions cannot be gamed.

Specific checks:

- save/load cannot reroll onset;
- treatment outcome seed stable;
- accommodation item cannot be assigned to two survivors simultaneously;
- remove/reassign does not duplicate item;
- repeated onset event does not stack duplicate condition;
- repeatedly withholding/restarting treatment cannot farm adaptation rewards;
- cure event fires once.

---

## 194. Achievement Exploit Prevention

If achievements observe:
- accommodations;
- management;

count unique valid condition/assignment IDs.

Do not count repeatedly toggling same device.

---

## 195. Quest Exploit Prevention

"The Care — maintain 20 accommodations" should mean:
- 20 valid maintenance completions;
- not toggling assignment 20 times.

QuestSystem defines exact criteria.

---

## 196. Respectful Narrative Review

Before ship, review all 20+ definitions and event text for:

- disability-as-tragedy clichés;
- "overcoming" framing;
- infantilization;
- automatic social rejection;
- magical cure framing;
- outdated terminology;
- implication that accommodation makes survivor "normal";
- implication that disability erases usefulness.

The system should represent adaptation and access, not moral judgment.

---

## 197. Content Design Principle: Access > Penalty

When possible, design a condition as:

```text
task has requirement
condition changes access/function
accommodation changes access
```

rather than:

```text
survivor receives -30% to everything
```

This principle should appear in code review checklist.

---

## 198. Condition Creation Review Checklist

For every new definition:

- [ ] real trigger?
- [ ] specific functional effects?
- [ ] unaffected capabilities remain unaffected?
- [ ] meaningful accommodation?
- [ ] realistic management path?
- [ ] no duplicate system ownership?
- [ ] respectful language?
- [ ] deterministic?
- [ ] test fixture?
- [ ] save-safe?

---

## 199. Release Gate

Release fails if:

- `HasChronicIllness` remains independently mutable;
- `FunctionalImpairmentPct` double-applies with chronic modifiers;
- a condition applies a generic social penalty without explicit communication/access rationale;
- a condition changes all work/combat/crafting equally without task-capability mapping;
- accommodation can exceed baseline unintentionally;
- old-save radiation migration duplicates conditions;
- progression rerolls on reload;
- treatment consumes resources twice;
- one device can be assigned twice;
- healthy survivor baseline changes;
- selftest or coverage validation fails.

The risk is medium not because persistence is complex, but because cross-system modifiers can become incoherent
and the content can become punitive if boundaries are weak.

---

## 200. Definition of Done — Flagship

### Authority
- [ ] `ChronicConditionSystem.cs`
- [ ] schema-versioned state
- [ ] stable condition instances
- [ ] `HasChronicIllness` migrated/projected
- [ ] pathology impairment boundary documented

### Content
- [ ] seven source categories
- [ ] 20+ meaningful conditions
- [ ] severity/course profiles
- [ ] treatment bindings
- [ ] accommodations
- [ ] localization
- [ ] respectful language review

### Onset
- [ ] injury
- [ ] radiation
- [ ] disease
- [ ] age
- [ ] authored/congenital initialization where supported
- [ ] deterministic onset
- [ ] source-event idempotency

### Functional Effects
- [ ] capability registry
- [ ] context-specific modifiers
- [ ] multiplicative composition
- [ ] caps/floors
- [ ] healthy parity
- [ ] no blanket social penalty

### Accommodations
- [ ] devices
- [ ] shelter adaptations where real
- [ ] communication access
- [ ] maintenance
- [ ] item/facility validation
- [ ] effective restoration

### Management
- [ ] medication
- [ ] therapy/rehab where supported
- [ ] condition-specific surgery
- [ ] prosthetics
- [ ] stable/progressive/managed/resolved
- [ ] periodic evaluation

### Integrations
- [ ] MedicalPipeline
- [ ] Radiation
- [ ] Disease
- [ ] Injury
- [ ] Aging
- [ ] Needs
- [ ] SkillProgression
- [ ] Work/Crafting
- [ ] Combat
- [ ] Expedition
- [ ] Journal/Quest/Achievement/Epilogue

### Validation
- [ ] old-save migration
- [ ] save round-trip
- [ ] multiple conditions
- [ ] accommodation overlap
- [ ] treatment idempotency
- [ ] selftest
- [ ] data integrity
- [ ] headless

---

## 201. Follow-On Task 193-A — Adaptive Work Roles

Goal:
allow scheduler to recommend tasks based on effective capabilities and accommodations.

Requirements:
- no discrimination/auto-exclusion by condition label;
- capability-based matching;
- player override where safe.

Output:
better role assignment, not a separate disability job system.

---

## 202. Follow-On Task 193-B — Shelter Accessibility Audit

Goal:
make physical shelter layout meaningfully accessible.

Requires:
- navigation graph;
- stairs/door widths or abstract access edges;
- ramps/lifts/adapted routes.

Do not implement until shelter topology supports it.

---

## 203. Follow-On Task 193-C — Prosthetics & Assistive Device Crafting

Goal:
add durable craftable assistive devices.

Requires:
- item definitions;
- workshop recipes;
- fitting/assignment;
- repair.

ChronicConditionSystem supplies requirements/effects.

---

## 204. Follow-On Task 193-D — Rehabilitation & Adaptation

Goal:
support longer-term functional adaptation.

Inputs:
- therapy;
- practice;
- assistive device use.

Outputs:
- improved accommodation effectiveness;
- adapted task preference.

No miraculous cure.

---

## 205. Follow-On Task 193-E — Long-Term Condition Narrative Pack

Goal:
author grounded events around management/access without stereotyping.

Examples:
- device repair;
- adapted workspace;
- treatment shortage;
- role transition;
- peer support.

Quest/Event systems own narrative flow.

---

## 206. Follow-On Task 193-F — Disability Legacy Integration

Goal:
allow Plan 145 to reference meaningful survivor life facts:
- accommodated role;
- leadership;
- long-term treatment stability;
- major achievements.

No automatic tragedy/hero framing.

---

## 207. Follow-On Task 193-G — Medical Accessibility Policy

Goal:
add shelter-level policies:
- prioritize assistive-device maintenance;
- accessible housing/workstation assignment;
- long-term medication reserve.

Only after policy infrastructure exists.

---

## 208. Final Guardrails

- No second health authority.
- No second wound authority.
- No second disease authority.
- No second aging authority.
- No second psychology authority.
- No duplicate cognitive-memory authority.
- No per-frame condition scan.
- No `System.Random`.
- No save/load reroll.
- No globally punitive disability multiplier.
- No generic social penalty for disability.
- No "wheelchair = globally slow" simplification.
- No "blindness = bad at everything" simplification.
- No direct parent-condition genetic inheritance.
- No universal surgery cure button.
- No treatment resource duplication.
- No assistive-device duplication.
- No condition disappearance when accommodation works.
- No condition worsening merely because device maintenance lapses unless treatment profile justifies it.
- No retroactive old-save condition generation except explicit migrated facts.
- No 20-condition filler quota.
- No player-facing pseudo-medical precision unsupported by game data.
- No moral framing that treats disability as failure.
- No achievement design rewarding intentionally causing disability.
- No mandatory caregiver tax for all disabled survivors.
- No condition label hardcoded into every task; use capabilities.

When complete, Plan 193 should create the middle ground that ASHFALL currently lacks: survivors can live with
lasting consequences, adapt, receive treatment, use assistive devices, change how they perform particular tasks,
and remain fully meaningful members of the shelter. The system will make serious injuries and illnesses matter
without making disability synonymous with uselessness.

The flagship proof is architectural: one persistent condition authority, typed functional capabilities,
contextual accommodations, deterministic progression, clean acute-to-chronic handoffs, migration of the
existing radiation flag, and downstream systems that ask "what can this survivor do in this context?" rather
than "does this survivor have a disability?".
