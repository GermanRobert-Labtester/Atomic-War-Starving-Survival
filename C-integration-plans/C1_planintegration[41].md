# C1 — Flagship Integration Plan [41]: Survivor Exercise, Physical Conditioning, Training Adaptation & Deconditioning

> **Output:** `C1_planintegration[41].md`
>
> **Source baseline:** Plan 216 — Survivor Exercise & Physical Training System
>
> **Primary mission:** make survivor physical conditioning dynamic, trainable, degradable, and strategically relevant through structured exercise, recovery, deconditioning, and physical preparation—without replacing the existing authorities for traits, age, needs, fatigue, health, afflictions, injury, skills, work eligibility, expedition fitness, combat, carrying, schedules, or survivor capability.
>
> **Primary architectural rule:** `ExerciseSystem` owns **trainable physical conditioning state and training adaptation**: exercise exposure, conditioning dimensions, recovery debt, deconditioning state, routine adherence, training-session provenance, and conditioning-specific trend/history. It does not own congenital/permanent traits, age decline, hunger, fatigue, sleep, injury diagnosis, disease resistance, combat damage, combat skill, job proficiency, work assignment, expedition state, carrying rules, or final action performance.
>
> **Primary correction to the source plan:** the source proposes `FitnessProfile` values for cardio/strength/flexibility/endurance, separate `PhysicalAttribute` base/current/modifier fields, direct work/combat/expedition/health effects, exercise-created injury types, and a universal old-save baseline of 50. Several of those would duplicate existing authorities. The flagship therefore makes conditioning a **bounded capability contribution** consumed through existing capability/performance adapters. Traits and age define potential/constraints; Needs and medical state define current impairment/readiness; SkillProgression owns learned skills; Plan 137 / canonical capability projection owns final effective performance.
>
> **Primary capability rule:** exercise may improve trainable conditioning, but it must not become a parallel final-stat system. No system should separately read `strength`, `trait_strength`, `need_penalty`, and `fitness_modifier` and multiply them ad hoc. Consumers should query a canonical physical-capability projection or adapter with one documented stacking formula.
>
> **Primary health rule:** “disease resistance” is explicitly not a direct fitness stat unless the medical/disease system already exposes an evidence-backed conditioning input. General fitness can affect recovery/readiness only through a bounded medical adapter; ExerciseSystem never mutates disease probability directly.
>
> **Primary injury rule:** training can create **injury-risk observations**, but actual strains, sprains, fractures, overexertion, collapse, or other afflictions must be instantiated by the canonical injury/medical system. ExerciseSystem does not own injury DTOs or set work incapacity itself.
>
> **Primary fatigue rule:** exercise consumes real survivor time, energy, calories/hydration, and fatigue capacity through canonical Needs/schedule/work systems. A training session is not free progression. Training undertaken while hungry, exhausted, injured, ill, elderly, overheated, irradiated, or overworked must be restricted or produce lower benefit/higher canonical risk through explicit adapters.
>
> **Primary adaptation rule:** conditioning gain follows progressive overload, specificity, recovery, diminishing returns, and trainability caps. Repeating the same light workout forever cannot push every survivor to 100. Extreme training does not linearly accelerate growth. Overtraining can stall progress.
>
> **Primary deconditioning rule:** lack of structured exercise is not automatically “inactivity.” Physically demanding work, expeditions, combat, hauling, construction, and other real activity can contribute to maintenance where supported. Deconditioning should reflect **insufficient physical load over time**, not simply `daysSinceExercise`.
>
> **Primary age rule:** Plan 176 remains aging authority. Exercise may preserve function or slow deconditioning within age-appropriate potential, but must not erase age-related decline or make chronological aging mutable.
>
> **Primary scheduling rule:** training occupies canonical time slots/routines. It competes with work, sleep, treatment, caregiving, hobbies, guard duty, and emergencies. Auto-exercise is a scheduling preference/policy, not a hidden simulation toggle that grants free sessions.
>
> **Mandatory execution order:** 216A authority/capability audit → 216B conditioning-state contract → 216C routine schema and load model → 216D training-session scheduling/execution → 216E adaptation/progressive-overload/recovery → 216F deconditioning and activity-equivalence → 216G Needs/age/traits/medical integration → 216H SkillProgression/combat-drill integration → 216I work/expedition/combat/carrying capability adapters → 216J UI/autonomy/coaching and anti-chore design → 216K persistence/migration/determinism/idempotence → 216L balance/performance/30–400-day simulation and CI → 216M competitions/coaches/training camps as follow-on only after the base loop is stable.
>
> **Critical re-baseline rule:** before creating `ExerciseSystem.cs`, inspect `SkillProgressionSystem`, `TraitSystem`, `NeedsSystem`, age/Plan 176, medical/affliction/injury systems, Plan 137 Needs→Performance, Plan 143 Medical Afflictions→Capability, Plan 24 survivor eligibility/fitness authority, DutyRoster/scheduling, Plan 188 routines, Plan 161 hobbies/leisure, combat capability/damage formulas, ExpeditionSystem travel/carry/stamina logic, inventory/encumbrance, work-efficiency projection, survivor autonomy, room/facility/workstation systems, save ordering, semantic event bus, and current UI capability surfaces.
>
> **Guardrails:** no duplicate final physical attributes; no second injury database; no direct disease-resistance scalar; no direct work/combat/expedition multiplier applied independently by ExerciseSystem; no daily free morale buff for exercising; no “lastExerciseDay” deconditioning that ignores manual labor and expeditions; no daily streak as primary progression engine; no universal old-save fitness=50 unless proven parity-safe; no 100-point caps treated as biologically equal across all survivors; no training while medically ineligible; no infinite gains from identical sessions; no extreme-intensity farming; no free auto-exercise; no exercise that bypasses DutyRoster/time; no combat-drill skill duplication; no age reversal; no arbitrary fracture generation outside medicine; no per-frame training loop; no unseeded RNG; no `Guid.NewGuid`; no wall clock; no exercise-event spam; no quest incentives that reward harmful overtraining or pointless streak maintenance.

---

# 0. Mission

ASHFALL already models several factors that determine whether a survivor can perform physically, but not adaptation to repeated physical activity.

The source baseline identifies:
- `TraitSystem` as a determinant of inherent characteristics;
- `NeedsSystem` as a determinant of fatigue/hunger and current readiness;
- age/Plan 176 as a determinant of age-related physical decline;
- `SkillProgressionSystem` for learned skills;
- no exercise system;
- no conditioning profile;
- no structured routines;
- no deconditioning model;
- no physical improvement through repeated training.

Current shape:

```text
TRAITS + AGE + NEEDS + SKILLS + AFFLICTIONS
                │
                ▼
        current capability
                │
        largely static with respect
        to long-term exercise
```

Target shape:

```text
CANONICAL SURVIVOR STATE
      │
      ├── traits / age
      ├── needs / fatigue
      ├── nutrition / hydration
      ├── injuries / illness
      ├── skills
      ├── duties / routines
      └── recent physical workload
      │
      ▼
ExerciseSystem
      │
      ├── training exposure
      ├── conditioning dimensions
      ├── recovery readiness
      ├── progressive overload
      ├── activity equivalence
      ├── adaptation
      └── deconditioning
      │
      ▼
PhysicalConditioningContribution
      │
      ▼
CANONICAL CAPABILITY / PERFORMANCE
      ├────────► Duty/work
      ├────────► Expedition
      ├────────► Combat
      ├────────► Carrying/manual labor
      └────────► Medical readiness/recovery (bounded)
```

The exercise layer should answer:

> How conditioned is this survivor for sustained effort, force production, movement efficiency and mobility, what training load have they accumulated, are they recovered enough to train safely, and is their conditioning improving, stable or declining?

It should not answer:

> What is their final combat damage?
> Are they medically fit for duty?
> How hungry are they?
> How old are they?
> What is their combat skill?
> Can they carry 42.3 kg?
> Do they have a fracture?
> Which job are they assigned?

Those remain canonical systems.

---

# 1. Source-Evidence Interpretation

## 1.1 Exercise/conditioning is genuinely absent

The source reports zero Core matches for:
- `ExerciseSystem`;
- `PhysicalTraining`;
- `FitnessSystem`;
- `WorkoutSystem`;
- `TrainingRegimen`;
- `PhysicalConditioning`;
- `ExerciseRoutine`;
- `FitnessTraining`;
- `StrengthTraining`;
- `CardioSystem`.

A conditioning/adaptation authority is justified.

## 1.2 The source correctly identifies dynamic capability as missing

Survivors currently do not get fitter/weaker based on training/inactivity.

That is the real gap.

## 1.3 The source's `PhysicalAttribute` DTO risks parallel truth

Fields:
- baseValue;
- currentValue;
- modifier;
- cap
could duplicate:
- traits;
- age;
- Plan 137 performance;
- Plan 143 capability.

Use conditioning state only.

## 1.4 Four conditioning dimensions are reasonable if consumers exist

Candidate:
- aerobic capacity/cardio;
- strength;
- mobility/flexibility;
- muscular/work endurance.

But “cardio” and “endurance” can overlap.

An ADR must define each precisely.

## 1.5 Exercise type and physiological adaptation are not the same thing

A mixed routine may contribute to:
- multiple training loads.

Use data-driven load vectors.

## 1.6 Combat drills have two outputs

Potential:
1. physical conditioning;
2. combat skill practice.

Exercise owns #1.
SkillProgression owns #2.

## 1.7 Deconditioning must consider all physical activity

A survivor hauling rubble 8 hours/day is not sedentary merely because they skipped “workout.”

## 1.8 The old-save `50 all attributes` proposal is dangerous

If the old game implicitly assumes current capability without conditioning penalties, introducing 50/100 might unintentionally nerf everyone.

Migration should preserve pre-feature behavior.

Recommended:
- initialize a neutral **maintenance baseline** calibrated so effective capability is unchanged on old saves.

---

# 2. Non-Negotiable Exercise Invariants

## INV-216.1 — One traits authority

## INV-216.2 — One aging authority

## INV-216.3 — One needs/fatigue authority

## INV-216.4 — One medical/injury authority

## INV-216.5 — One skill authority

## INV-216.6 — One duty/schedule authority

## INV-216.7 — One final capability/performance projection

## INV-216.8 — Exercise owns conditioning only

## INV-216.9 — Conditioning is trainable, not innate trait truth

## INV-216.10 — Exercise gains require real time/load

## INV-216.11 — Recovery limits adaptation

## INV-216.12 — Training load is specific to adaptation dimensions

## INV-216.13 — Identical low load has diminishing benefit

## INV-216.14 — Physical work can contribute to maintenance

## INV-216.15 — Deconditioning uses physical-load deficit, not workout checkbox

## INV-216.16 — Training injuries are created by medical authority

## INV-216.17 — Combat drill skill gain is created by SkillProgression

## INV-216.18 — Fitness cannot erase age decline

## INV-216.19 — Fitness cannot bypass medical incapacity

## INV-216.20 — Exercise does not directly own work/combat/expedition results

## INV-216.21 — Old saves preserve effective capability

## INV-216.22 — Training sessions are exactly once

## INV-216.23 — No per-frame adaptation

## INV-216.24 — Headless behavior is deterministic

---

# 3. Definition of Done

Plan 216 closes only when:

- current survivor capability authorities are documented;
- Plan 137/143/24 integration seams are explicit;
- four conditioning dimensions have non-overlapping definitions;
- conditioning is separated from traits and skills;
- each routine produces a training-load vector;
- training consumes canonical time;
- Needs controls hunger/fatigue/hydration constraints;
- medical system controls eligibility and injury creation;
- age controls potential/recovery constraints;
- traits modify trainability/potential but do not directly duplicate conditioning state;
- adaptation uses progressive overload and diminishing returns;
- recovery debt can stall or reverse gains;
- physical work/expeditions/combat contribute to maintenance load where real;
- deconditioning is based on insufficient total physical stimulus;
- final work/combat/expedition effects pass through one capability projection;
- combat drill physical adaptation and combat skill XP are separated;
- no direct disease-resistance mutation exists without medical authority support;
- no automatic universal carrying bonus bypasses encumbrance rules;
- old-save migration preserves current effective capability;
- old saves do not receive a hidden -50% conditioning penalty;
- save/load preserves conditioning/load/recovery state exactly;
- no session benefits replay after restore;
- training cannot be repeated by save reload;
- auto-exercise schedules actual time;
- exercise UI does not require daily micromanagement;
- 30/120/180/400-day simulations demonstrate gain, plateau, detraining and age interaction;
- `--exercise-selftest` exists or equivalent;
- routine data passes equipment/facility/skill/content reachability.

---

# 4. Phase P0 — Capability & Authority Audit

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
TraitSystem physical traits
Plan 176 age state
NeedsSystem hunger/fatigue/sleep/hydration
medical affliction/injury systems
Plan 143 capability bridge
Plan 137 performance cascade
Plan 24 survivor eligibility
SkillProgressionSystem
combat capability formulas
ExpeditionSystem stamina/travel/carry logic
DutyRoster/work efficiency
encumbrance/carrying capacity
Plan 188 routine scheduler
Plan 161 hobbies/leisure
survivor autonomy
facility/workstation/equipment requirements
semantic events
campaign clock
save order
survivor capability UI
```

## P0.2 Build physical-capability authority matrix

Create:

`docs/exercise/PHYSICAL_CONDITIONING_AUTHORITY_MATRIX.md`

Columns:

```text
fact
canonical owner
read API
write API
exercise role
persisted?
status
```

Rows:
- age;
- physical trait;
- hunger;
- fatigue;
- sleep;
- hydration;
- injury;
- illness;
- medical eligibility;
- cardio conditioning;
- strength conditioning;
- mobility;
- work endurance;
- combat skill;
- work skill;
- expedition skill;
- final physical capability;
- carrying;
- work efficiency;
- combat performance;
- expedition performance;
- exercise session;
- training load;
- recovery readiness;
- deconditioning.

## P0.3 Conditioning-vs-capability ADR

Create:

`docs/architecture/ADR_CONDITIONING_VS_PHYSICAL_CAPABILITY.md`

## P0.4 Training-vs-skill ADR

Create:

`ADR_EXERCISE_VS_SKILL_PROGRESSION.md`

## P0.5 Exercise-injury ADR

Create:

`ADR_EXERCISE_INJURY_AUTHORITY.md`

## P0.6 Old-save baseline ADR

Create:

`ADR_EXERCISE_OLD_SAVE_CAPABILITY_PARITY.md`

## P0.7 Baseline measurements

Record effective capability for representative survivors:
- young/healthy;
- older/healthy;
- hungry/fatigued;
- injured;
- high/low physical trait;
- skilled combatant.

These become migration parity oracles.

---

# TASK 216A — Conditioning State Contract

# 216A.0 Goal

Define trainable conditioning without duplicating base traits or final effective performance.

## 216A.1 Proposed owner

`Assets/Ashfall.Core/Survivors/ExerciseSystem.cs`

## 216A.2 Recommended DTO

```text
ConditioningProfile
  survivor_id
  aerobic_conditioning
  force_conditioning
  mobility_conditioning
  work_capacity_conditioning
  adaptation_state[]
  last_meaningful_load_day[]
  recent_load_ema[]
  recovery_debt
  detraining_stage[]
  training_age_days
  profile_revision
```

## 216A.3 Rename source attributes carefully

Source:
- cardio;
- strength;
- flexibility;
- endurance.

Recommended semantics:

### Aerobic conditioning
Sustained oxygen-dependent effort.

### Force conditioning
Trainable force-production capacity.

### Mobility conditioning
Range of motion / movement-quality capacity.

### Work-capacity conditioning
Repeated submaximal muscular effort / fatigue resistance.

## 216A.4 Avoid “overallFitness” as primary truth

Can derive UI summary.

## 216A.5 No `baseValue`

Base/potential belongs to:
- traits;
- age;
- physiology.

## 216A.6 No final `currentValue`

Conditioning value itself is current adaptation state.

Final action capability remains external.

## 216A.7 Scale

0–100 internal can work if:
- semantically calibrated;
- not treated as universal biological units.

## 216A.8 Neutral baseline

Define:

```text
conditioning_neutral
```

such that:
- old/current game behavior is preserved.

May be:
- 50;
- 60;
- 0 additive contribution.

Choose from capability formula, not aesthetics.

## 216A.9 Prefer normalized contribution

Example:

```text
conditioning_delta ∈ [-1, +1]
```

could be safer internally.

UI can render 0–100.

## 216A.10 Potential/cap

Derived from:
- age;
- traits;
- medical constraints.

Do not persist fixed cap if source state changes.

## 216A.11 Training age

Tracks cumulative adapted experience.

Not a skill.

## 216A.12 Trend

Derived:
- improving;
- stable;
- declining.

No need persisted if recomputable.

## 216A.13 History

Do not persist unbounded snapshot list.

Use:
- compact weekly/monthly samples;
- milestones.

## 216A.14 Last exercise day

Insufficient for deconditioning.

Store:
- last meaningful load per dimension.

## 216A.15 Recovery debt

Training-specific load stress, not duplicate fatigue.

## 216A.16 Restore

No adaptation replay.

### 216A DoD

Conditioning state represents only trainable adaptation, with all innate, age-related, acute, medical and final-capability facts remaining elsewhere.

---

# TASK 216B — Exercise Routine Data Authority

# 216B.0 Goal

Define exercise sessions as data-driven load prescriptions.

## 216B.1 Data file

`Assets/StreamingAssets/Data/exercise_routines.json`

## 216B.2 Routine DTO

Suggested:

```text
routine_id
display_key
routine_family
duration_options
intensity_band
load_vector
movement_tags[]
equipment_requirements[]
facility_requirements[]
skill_requirements[]
medical_exclusion_tags[]
need_thresholds[]
recovery_cost_profile
injury_risk_profile
combat_skill_practice_profile optional
group_size_range
coach_supported
```

## 216B.3 Routine families

Source candidates:
- cardio;
- strength;
- flexibility/mobility;
- endurance/work capacity;
- combat drill;
- mixed.

## 216B.4 Running

Load:
- aerobic;
- work capacity.

Requires:
- safe route/space.

## 216B.5 Cycling

Only if:
- bike/trainer equipment exists.

Do not invent inaccessible routine.

## 216B.6 Jumping

Could be:
- calisthenic/cardio.

## 216B.7 Weightlifting

Requires:
- weights or improvised load.

## 216B.8 Push-ups

No equipment.

## 216B.9 Pull-ups

Requires:
- safe bar/fixture.

## 216B.10 Stretching

Mobility.

## 216B.11 Yoga

Only if content tone fits.

Could be generic mobility routine.

## 216B.12 Long-duration activity

Work-capacity/aerobic.

## 216B.13 Combat drill

Physical load + SkillProgression practice.

## 216B.14 Mixed

Multiple load dimensions but lower specificity.

## 216B.15 Duration

Canonical game-time hours.

## 216B.16 Intensity

Suggested:
- recovery;
- light;
- moderate;
- hard;
- maximal.

Avoid casual “extreme” routine if medically reckless by default.

## 216B.17 Requirements

Must be real:
- equipment;
- facility;
- partner;
- coach;
- space.

## 216B.18 Prerequisite

Do not use “minimum fitness” circularly without reason.

Some harder routines require:
- medical clearance;
- movement skill;
- conditioning band.

## 216B.19 Benefits

Store:
- training load vector,
not direct `+2 strength`.

## 216B.20 Risk

Store:
- risk profile parameters.

Medical system creates outcome.

## 216B.21 15+ routines

Content target only.

Every routine must be reachable.

## 216B.22 Data integrity

Validate:
- equipment IDs;
- facility IDs;
- skill IDs;
- exclusion tags;
- load vector;
- localization.

### 216B DoD

Exercise routines describe time, intensity, specificity, requirements and load rather than directly writing final fitness attributes.

---

# TASK 216C — Training Session Scheduling & Execution

# 216C.0 Goal

Make exercise consume actual survivor time and readiness.

## 216C.1 Session intent

Suggested:

```text
TrainingSessionIntent
  session_id
  survivor_id
  routine_id
  scheduled_start
  planned_duration
  planned_intensity
  partner_ids[]
  coach_id optional
  source_schedule_id
```

## 216C.2 Scheduler authority

Use:
- DutyRoster;
- Plan 188 routine scheduler;
- activity/job scheduler.

## 216C.3 Exercise is an activity/job

It competes with:
- work;
- sleep;
- treatment;
- childcare;
- meals;
- leisure;
- guard duty.

## 216C.4 Auto-exercise

Source proposes bool.

Correct:
- scheduling preference.

Example:
- maintain conditioning;
- train 3×/week;
- exercise when idle.

## 216C.5 No hidden free auto tick

## 216C.6 Pre-session eligibility

Check:
- alive;
- conscious;
- not medically excluded;
- hunger;
- hydration;
- fatigue;
- current duty;
- required equipment/facility;
- partner/coach.

## 216C.7 Partial session

Possible:
- emergency interruption;
- fatigue;
- duty recall.

## 216C.8 Completion rate

Derived from actual time executed.

## 216C.9 Session output

Suggested:

```text
TrainingSessionResult
  session_id
  actual_duration
  effective_load_vector
  recovery_cost
  need_cost_refs[]
  injury_risk_observation optional
  skill_practice_event optional
  completion_reason
```

## 216C.10 No direct `benefitsGained` stored as arbitrary dict

Adaptation occurs after load/recovery processing.

## 216C.11 Fatigue cost

NeedsSystem owns fatigue.

Exercise emits:
- exertion workload.

## 216C.12 Hunger/calories

If calorie expenditure model exists:
- emit workload to Needs.

Do not invent exercise calories if no metabolic model.

## 216C.13 Hydration

Same.

## 216C.14 Interrupted session

No full reward.

## 216C.15 Emergency

Exercise yields immediately if emergency schedule says so.

## 216C.16 Session exactly once

Stable ID.

## 216C.17 Headless

No UI dependencies.

### 216C DoD

Training is a real scheduled activity whose completed workload, costs, interruptions and eligibility come from canonical time, Needs, medical and scheduling systems.

---

# TASK 216D — Training Load, Specificity & Progressive Overload

# 216D.0 Goal

Make exercise improvement require an appropriate stimulus rather than session count.

## 216D.1 Load vector

Example:

```text
[aerobic, force, mobility, work_capacity]
```

Normalized.

## 216D.2 Effective load

Depends on:
- routine;
- duration;
- intensity;
- completion;
- current conditioning;
- equipment quality if relevant.

## 216D.3 Specificity

Running:
- high aerobic;
- moderate work capacity;
- low force.

Strength:
- high force;
- some work capacity.

Mobility:
- high mobility.

## 216D.4 Progressive overload

Adaptation requires:
- effective load above maintenance threshold.

## 216D.5 Underload

Maintains or provides minimal gain.

## 216D.6 Productive load

Improves.

## 216D.7 Excessive load

Higher recovery debt/injury risk;
not proportionally higher gain.

## 216D.8 Diminishing returns

As conditioning rises:
- same routine produces less adaptation.

## 216D.9 Plateau

Expected.

## 216D.10 New stimulus

Increase:
- duration;
- intensity;
- resistance;
- complexity
within safe limits.

## 216D.11 No linear `session = +1`

## 216D.12 Trait trainability

Traits may modify:
- response;
- potential.

## 216D.13 Age

Modifies:
- recovery;
- potential;
- adaptation rate.

## 216D.14 Illness/injury

Can suppress effective adaptation.

## 216D.15 Nutrition

If supported:
- poor nutrition limits adaptation.

## 216D.16 Load history

Use rolling:
- acute;
- chronic
training load.

Avoid excessive complexity if not needed.

## 216D.17 MVP model

Could use:
- 7-day recent load;
- 28-day chronic load;
- recovery debt.

## 216D.18 No sports-science overfitting

Game-readable, not clinical simulator.

### 216D DoD

Conditioning improves through specific, recoverable, progressively challenging physical load with diminishing returns and plateau behavior.

---

# TASK 216E — Recovery, Overtraining & Readiness

# 216E.0 Goal

Prevent nonstop training from being optimal.

## 216E.1 Recovery debt

Training-specific adaptation stress.

## 216E.2 Distinct from Needs fatigue

Needs fatigue:
- acute tiredness.

Recovery debt:
- accumulated training stress.

## 216E.3 Inputs

- load;
- intensity;
- age;
- sleep quality;
- nutrition;
- injury/illness;
- recent sessions.

## 216E.4 Recovery

Occurs with:
- time;
- adequate sleep/rest;
- nutrition
through canonical sources.

## 216E.5 Overreaching

Moderate temporary overload:
- reduced next-session quality.

## 216E.6 Overtraining

Sustained excess:
- stalled adaptation;
- elevated medical risk;
- performance contribution reduction.

Use restrained game semantics.

## 216E.7 No new disease

Overtraining may expose:
- fatigue/stress context.

Medical system decides actual conditions.

## 216E.8 Rest day

Can be productive.

## 216E.9 Light recovery session

Possible.

## 216E.10 Injury recovery

Medical authority.

Exercise can:
- be restricted;
- use rehab routine
if medicine supports.

## 216E.11 Rehab

Do not implement physical therapy unless medical system supports.

## 216E.12 Readiness indicator

Derived:
- ready;
- caution;
- needs recovery;
- medically blocked.

## 216E.13 Auto-scheduler

Should respect readiness.

## 216E.14 No streak punishment

A rest day should not “break progress” mechanically.

### 216E DoD

Training gains depend on recovery, while excessive training creates bounded readiness costs rather than rewarding maximum daily exercise.

---

# TASK 216F — Deconditioning & Activity Equivalence

# 216F.0 Goal

Model decline from insufficient physical stimulus without penalizing physically active survivors.

## 216F.1 Reject source rule

```text
fitness decays because lastExerciseDay is old
```

Insufficient.

## 216F.2 Activity sources

Potential:
- structured exercise;
- heavy manual work;
- hauling;
- construction;
- expedition travel;
- combat;
- scouting;
- physically demanding chores.

## 216F.3 Activity workload adapter

Canonical systems may emit:

```text
PhysicalLoadObservation
```

## 216F.4 Exercise consumes same load model

## 216F.5 Maintenance threshold

Each dimension needs:
- minimum rolling stimulus.

## 216F.6 Deconditioning delay

Short inactivity:
- little/no loss.

## 216F.7 Longer inactivity

Gradual decline.

## 216F.8 Bed rest/injury

Can accelerate detraining.

## 216F.9 Age

May accelerate decline/reduce reacquisition.

## 216F.10 Re-training

Previously conditioned survivor may regain faster only if model intentionally supports training history.

Optional:
- training-age adaptation memory.

## 216F.11 Dimension-specific decay

Aerobic may decay differently from force/mobility.

Use simplified profiles.

## 216F.12 No daily journal event

## 216F.13 Significant threshold

Notify:
- meaningful drop only.

## 216F.14 Work cannot train everything

Heavy lifting:
- force/work capacity;
not necessarily mobility/aerobic fully.

## 216F.15 Expedition walking

Aerobic/work capacity.

## 216F.16 Sedentary shelter role

Requires deliberate exercise to maintain.

### 216F DoD

Conditioning declines from sustained lack of relevant physical load, while real work and travel correctly contribute to maintenance.

---

# TASK 216G — TraitSystem Integration

# 216G.0 Goal

Use survivor traits as trainability/potential modifiers without turning traits into duplicate fitness.

## 216G.1 Audit physical traits

Do not assume IDs.

## 216G.2 Potential roles

Traits can modify:
- starting neutral conditioning;
- adaptation rate;
- recovery;
- potential;
- injury-risk context.

## 216G.3 “Athletic”

If real:
- better trainability/potential.

## 216G.4 “Strong”

If trait exists:
- base physical capability remains trait-owned.

Exercise force conditioning adds bounded contribution.

## 216G.5 “Flexible”

Same.

## 216G.6 Poor constitution

Medical/trait authority.

## 216G.7 No trait copied into fitness state

## 216G.8 No trait mutation from exercise

Unless another plan allows traits to evolve.

## 216G.9 Trait cap

Do not permanently prevent improvement with crude 0–100 cap unless design requires.

Use potential curve.

## 216G.10 Explainability

UI distinguishes:
- innate predisposition;
- trainable conditioning.

### 216G DoD

Traits shape physical potential and response to training while conditioning remains a separate learned physiological adaptation.

---

# TASK 216H — Age / Plan 176 Integration

# 216H.0 Goal

Let exercise preserve function across age without erasing aging.

## 216H.1 Plan 176 owns chronological age

## 216H.2 Age affects

Potential:
- recovery speed;
- injury risk;
- adaptation ceiling;
- detraining rate.

## 216H.3 No age reversal

## 216H.4 Older survivor

Can improve from sedentary baseline.

## 216H.5 High conditioning older survivor

Can outperform:
- unconditioned younger survivor
within bounded domains.

## 216H.6 Age-related decline

Still applies to base capacity.

## 216H.7 Conditioning preservation

Exercise may:
- offset some deconditioning;
not:
- cancel base aging.

## 216H.8 Training prescription

Older/medically vulnerable survivor:
- lower safe load.

## 216H.9 UI

Show:
- age context;
not deterministic “too old to train.”

## 216H.10 Long-horizon aging sim

Mandatory.

### 216H DoD

Exercise interacts plausibly with aging by preserving and improving trainable capacity without modifying chronological age or nullifying age-related constraints.

---

# TASK 216I — Needs, Nutrition, Sleep & Hydration Integration

# 216I.0 Goal

Make training readiness depend on real survival state.

## 216I.1 NeedsSystem owns

- hunger;
- fatigue;
- sleep;
- hydration if present.

## 216I.2 Eligibility thresholds

Example:
- severe hunger → block hard session;
- extreme fatigue → block;
- dehydration → block.

Use real scale.

## 216I.3 Mild deficit

Reduces:
- session completion;
- adaptation;
- recovery.

## 216I.4 Training cost

Emit exertion.

## 216I.5 No duplicate fatigue bar

## 216I.6 Post-session fatigue

NeedsSystem applies.

## 216I.7 Meal timing

Do not add sports nutrition minigame by default.

## 216I.8 Protein/macros

Out of scope unless nutrition system supports.

## 216I.9 Sleep

Poor sleep:
- lower recovery.

## 216I.10 Stress

Could reduce adherence/recovery through mental-health system.

## 216I.11 Illness

Medical system.

## 216I.12 Plan 137 double-count guard

If Needs already lowers performance:
- Exercise capability adapter must not reapply hunger/fatigue penalties.

### 216I DoD

Training consumes and depends on canonical survivor needs while exercise state never duplicates hunger, fatigue, sleep or their performance penalties.

---

# TASK 216J — Medical Eligibility & Exercise Injury Integration

# 216J.0 Goal

Create meaningful training risk through canonical medical systems.

## 216J.1 Medical eligibility query

Suggested:

```text
GetExerciseEligibility(survivorId, routineTags, intensity)
```

## 216J.2 Inputs

- fractures;
- strains;
- wounds;
- fever;
- radiation sickness;
- cardiovascular condition if modeled;
- severe malnutrition;
- pregnancy if modeled;
- age vulnerability.

Only real afflictions.

## 216J.3 Blocked routine

UI explains.

## 216J.4 Modified routine

Potential:
- lower intensity;
- mobility only.

## 216J.5 Injury risk observation

Exercise calculates:
- load/exposure context.

Medical/injury authority resolves:
- whether injury occurs;
- injury type/severity.

## 216J.6 No `injuryOccurred` as authoritative saved exercise field

Session may reference:
- medical_event_id.

## 216J.7 Source injury types

- strain;
- sprain;
- fracture;
- overexertion.

Ship only those supported by medical catalog.

## 216J.8 Overexertion

Could be:
- fatigue collapse;
- heat event
depending medical system.

## 216J.9 Flexibility “reduces injury risk”

Make bounded input to medical risk model.

Not universal immunity.

## 216J.10 Training through injury

Blocked/limited.

## 216J.11 Reinjury

Medical authority.

## 216J.12 Rehab

Follow-on if medical treatment system supports.

## 216J.13 Exactly-once medical event

Stable session risk ID.

### 216J DoD

Exercise can expose survivors to training injuries, but all diagnosis, severity, incapacity and treatment remain canonical medical state.

---

# TASK 216K — SkillProgression & Combat Drill Integration

# 216K.0 Goal

Separate physical conditioning from learned combat skill.

## 216K.1 Combat drill routine

Creates:
- physical load;
- practice observation.

## 216K.2 Exercise owns

- conditioning load.

## 216K.3 SkillProgression owns

- combat skill XP/progression.

## 216K.4 No local combat-skill field

## 216K.5 Practice quality

Can depend on:
- partner;
- coach;
- equipment;
- completion.

## 216K.6 Partner

Must be available.

## 216K.7 Sparring injury

Medical/combat training adapter.

## 216K.8 No live combat damage

Use training/sparring path.

## 216K.9 Weapon drills

Only if safe training equipment/rules exist.

## 216K.10 Strength routine

Does not grant combat skill.

## 216K.11 Endurance routine

Does not grant weapon skill.

## 216K.12 Skill XP caps

Canonical anti-farm.

## 216K.13 Training partners and relations

Optional social event;
not automatic relationship boost every session.

### 216K DoD

Combat drills improve conditioning and learned combat skill through two explicit authorities without conflating physical fitness with fighting proficiency.

---

# TASK 216L — Work & DutyRoster Capability Integration

# 216L.0 Goal

Make conditioning matter to physical work through the canonical performance projection.

## 216L.1 Identify work categories

Examples:
- heavy labor;
- hauling;
- maintenance;
- farming;
- medical;
- research;
- guard.

## 216L.2 Physical-demand tags

Duty/job definitions should declare:
- force demand;
- aerobic demand;
- work-capacity demand;
- mobility demand.

## 216L.3 Conditioning contribution

Project to:
- effectiveness/readiness
through Plan 137/24.

## 216L.4 No universal work bonus

High strength should not improve:
- research;
- radio analysis.

## 216L.5 Heavy labor

May benefit force/work capacity.

## 216L.6 Long shift

Work capacity can help fatigue resistance,
but shift rules remain DutyRoster/Needs.

## 216L.7 No longer-work-shift permission automatically

Source proposes high endurance → longer shifts.

Correct:
- may reduce fatigue accumulation within bounds;
- DutyRoster still owns shift length/safety.

## 216L.8 Physically demanding work

Emits maintenance training load.

## 216L.9 Overwork

Can increase recovery debt rather than count as beneficial training indefinitely.

## 216L.10 Eligibility

Medical/Plan24 first.

## 216L.11 Capability trace

Debug:
- base;
- conditioning;
- needs;
- medical;
- skill.

### 216L DoD

Conditioning improves only physically relevant duties through one canonical performance stack, while demanding work itself contributes realistic physical load.

---

# TASK 216M — Expedition Integration

# 216M.0 Goal

Make physical conditioning relevant to travel without duplicating expedition state.

## 216M.1 ExpeditionSystem owns

- route;
- travel;
- hazard;
- supplies;
- encounters;
- survival outcome.

## 216M.2 Conditioning context

Suggested:

```text
ExpeditionPhysicalConditioningContext
  aerobic
  work_capacity
  force
  mobility
  readiness
```

## 216M.3 Aerobic

May reduce:
- exertion cost;
- travel fatigue
within bounded range.

## 216M.4 Work capacity

May improve:
- repeated walking/hauling tolerance.

## 216M.5 Force

May affect:
- physical obstacle/haul tasks,
not general travel speed.

## 216M.6 Mobility

May affect:
- certain traversal/injury contexts.

## 216M.7 No direct “fitness = route success”

## 216M.8 Carrying

Encumbrance system owns.

Force conditioning can contribute bounded capacity if architecture supports.

## 216M.9 No huge backpack scaling

## 216M.10 Expedition activity

Returns:
- physical load observation
for deconditioning maintenance.

## 216M.11 Exhausted expedition return

Needs owns fatigue.

Exercise recovery debt may also increase due to sustained workload.

## 216M.12 Expedition injury

Medical/expedition authority.

## 216M.13 Selection UI

Can show:
- conditioning suitability.

### 216M DoD

Expedition systems consume bounded conditioning context and return real travel workload, while route success, carrying and hazard outcomes remain canonical.

---

# TASK 216N — Combat Integration

# 216N.0 Goal

Make conditioning relevant in combat without creating a second combat-stat stack.

## 216N.1 Combat authority

Audit exact system.

## 216N.2 Conditioning should not directly write

- damage;
- accuracy;
- armor;
- health.

## 216N.3 Force conditioning

Possible bounded inputs:
- melee force;
- grapple;
- recoil handling only if combat model supports.

## 216N.4 Aerobic/work capacity

Possible:
- combat fatigue/recovery.

## 216N.5 Mobility

Possible:
- movement/evasion context.

## 216N.6 No universal flat combat bonus

## 216N.7 Combat skill remains separate

## 216N.8 Weapon handling

Skill/equipment.

## 216N.9 Injury risk

Medical/combat.

## 216N.10 Combat activity

Generates physical load.

But high-risk combat is not optimal training.

## 216N.11 Anti-farm

Fighting weak enemies cannot be best fitness routine.

Cap/discount uncontrolled combat load.

## 216N.12 Plan 137/143

Final combat eligibility/effectiveness combines:
- conditioning;
- needs;
- afflictions;
- skill;
- equipment.

One trace.

### 216N DoD

Conditioning contributes to physically relevant combat capabilities through the canonical combat/capability pipeline without duplicating skill, damage or injury systems.

---

# TASK 216O — Health & Disease Integration

# 216O.0 Goal

Avoid unsupported medical claims while still allowing conditioning to matter where canonical systems can consume it.

## 216O.1 Reject direct source rule

```text
high endurance → disease resistance
```

unless DiseaseSystem has a specific compatible input.

## 216O.2 Conditioning may support

If medical authority permits:
- recovery reserve;
- exertional tolerance;
- rehabilitation;
- cardiovascular resilience.

## 216O.3 Disease infection probability

Do not modify by default.

## 216O.4 Immune function

No generic stat.

## 216O.5 Injury recovery

Could be small modifier if medical model supports.

## 216O.6 Overtraining

May worsen readiness/stress,
not directly create infection.

## 216O.7 Malnutrition

Needs/medical.

## 216O.8 Age

Medical/aging.

## 216O.9 Health effect matrix

Create:

`EXERCISE_MEDICAL_EFFECT_MATRIX.md`

Every effect:
- supported;
- bounded;
- source API.

## 216O.10 Unsupported claims

DEFER.

### 216O DoD

Exercise integrates only with medical outcomes that have a real supported interface, avoiding a vague universal “health” or disease-resistance multiplier.

---

# TASK 216P — Autonomy, Motivation & Training Preferences

# 216P.0 Goal

Let survivors participate in exercise without turning the player into a mandatory daily fitness scheduler.

## 216P.1 Plan 144 autonomy

Audit willingness/refusal.

## 216P.2 Exercise assignment

Can be:
- ordered;
- suggested;
- self-selected
depending governance.

## 216P.3 Survivor preference

May derive from:
- traits;
- hobbies;
- role;
- relationship;
- schedule.

## 216P.4 Plan 161 hobbies

Physical hobbies may:
- count as light exercise;
- improve adherence.

No duplicate hobby state.

## 216P.5 Motivation

Do not create new motivation meter by default.

Use:
- autonomy;
- morale;
- traits.

## 216P.6 Refusal

Possible if:
- exhausted;
- injured;
- dislikes routine;
- autonomy policy.

## 216P.7 Player override

Governance/work policy.

## 216P.8 Auto plan

Recommended presets:
- maintain;
- improve gently;
- combat conditioning;
- expedition prep;
- recovery.

## 216P.9 Preset schedules

Use canonical scheduler.

## 216P.10 No mandatory streak

## 216P.11 Training partner

Optional.

## 216P.12 Partner compatibility

Relations can influence adherence,
but no automatic daily relation gain.

## 216P.13 Coach

Follow-on/canonical skill role if supported.

### 216P DoD

Exercise can be scheduled through survivor preferences and reusable plans, respecting autonomy and eliminating the need for daily manual workout assignment.

---

# TASK 216Q — Facilities, Equipment & Shelter Constraints

# 216Q.0 Goal

Ground physical training in actual shelter space and equipment without requiring a full gym system.

## 216Q.1 Bodyweight routines

Require:
- minimal/no equipment.

## 216Q.2 Running

Requires:
- safe space;
- exterior route;
or
- indoor corridor policy.

## 216Q.3 Weather/radiation

Outdoor exercise blocked/modified by:
- canonical hazards.

## 216Q.4 Weights

Use real items/facility if available.

## 216Q.5 Improvised weights

Could be recipe/facility content.

## 216Q.6 Pull-up bar

Facility/fixture if needed.

## 216Q.7 Training mat

Optional.

## 216Q.8 Combat drill area

Could require:
- safe training space.

## 216Q.9 Shelter space

Do not add new room system.

Use facility/workstation tags.

## 216Q.10 Power

Most routines should not require power.

## 216Q.11 Equipment wear

Only if equipment-condition system models training equipment.

## 216Q.12 Facility capacity

Group sessions bounded by space.

## 216Q.13 No 15 routines if 12 are unreachable

### 216Q DoD

Training routines bind to real shelter space, hazard exposure and equipment requirements while low-tech bodyweight options keep the system viable.

---

# TASK 216R — UI, Progress, Logs & Anti-Chore Design

# 216R.0 Goal

Make conditioning understandable and strategic without presenting a spreadsheet of mandatory daily workouts.

## 216R.1 Survivor panel

Show:
- conditioning summary;
- trend;
- readiness;
- recent load;
- recommended focus.

## 216R.2 Dimensions

Use semantic labels.

## 216R.3 Exact scores

Optional advanced tooltip.

## 216R.4 Overall fitness

Derived UI summary only.

## 216R.5 Routine panel

Show:
- duration;
- intensity;
- physical focus;
- requirements;
- expected load;
- recovery demand;
- medical restrictions.

## 216R.6 Training plan

Preset schedule.

## 216R.7 Session history

Bounded.

## 216R.8 Progress graph

Weekly samples.

## 216R.9 Deconditioning

Show:
- “aerobic conditioning declining”
only at meaningful thresholds.

## 216R.10 Alerts

High value only:
- medically blocked;
- training injury;
- major milestone;
- significant deconditioning;
- overtraining risk.

## 216R.11 No every-session popup

## 216R.12 Tutorial

First completed structured session.

## 216R.13 Capability explanation

Example:

```text
Expedition readiness:
+ conditioning
- fatigue
- ankle sprain
+ survival skill
```

Use canonical projection trace.

## 216R.14 Accessibility

- no color-only trend;
- keyboard/controller;
- no hover-only details;
- text scaling;
- screen-reader.

## 216R.15 Automation

“Maintain fitness” should be enough for casual players.

## 216R.16 No body-shaming language

Use:
- conditioning;
- readiness;
- deconditioned.

## 216R.17 No BMI/body-shape system

Out of scope.

### 216R DoD

Players can understand physical development, plan training and automate routine maintenance without daily fitness micromanagement.

---

# TASK 216S — Persistence, Migration & Restore

# 216S.0 Goal

Persist conditioning state while preserving old-save capability parity.

## 216S.1 Persist

Suggested:

```text
schema_version
feature_activation_day
conditioning_profiles[]
recent_training_load[]
recovery_debt[]
scheduled_training_refs[]
processed_session_ids[]
training_sequence
weekly_history_samples[]
```

## 216S.2 Do not persist duplicate

- traits;
- age;
- Needs;
- injuries;
- medical eligibility;
- combat skill;
- work skill;
- final work performance;
- expedition state;
- combat state.

## 216S.3 Old-save strategy

Do **not** blindly set 50.

Calibrate neutral profile so:

```text
effective_capability_after_migration
≈ effective_capability_before_feature
```

for baseline survivor fixtures.

## 216S.4 Possible neutral profile

If consumer formula is multiplicative around 1.0:
- initialize conditioning contribution = 1.0.

UI maps that to:
- “baseline conditioned.”

## 216S.5 Existing physically active survivors

Do not infer high fitness retroactively unless reliable activity history exists.

Default:
- parity baseline.

## 216S.6 New campaigns

Can initialize from:
- age;
- traits;
- survivor background
through deterministic profile generation.

## 216S.7 Restore ordering

After:
- survivors;
- traits;
- age;
- needs;
- medical;
- skills
and before:
- capability consumers
or two-phase reconcile.

## 216S.8 Missing survivor

Drop/archive profile safely.

## 216S.9 Changed routine definition

Past session retains:
- resolved load/result summary.

## 216S.10 Recompute trend

Do not persist if unnecessary.

## 216S.11 Session replay

Processed session IDs prevent duplicate adaptation.

## 216S.12 Interrupted save

Session state transactionally persisted.

## 216S.13 Scheduled job restore

Use scheduler authority.

## 216S.14 History compaction

Weekly/monthly.

### 216S DoD

Exercise persistence contains only conditioning/load/recovery facts, old saves retain pre-feature capability, and restore cannot replay completed training gains.

---

# TASK 216T — Determinism & Exploit Prevention

# 216T.0 Goal

Prevent reload rerolls, workout spam, streak farming and unsafe extreme-training optimization.

## 216T.1 Session ID stable

## 216T.2 Load deterministic

From:
- routine;
- duration;
- intensity;
- completion;
- survivor context.

## 216T.3 Adaptation deterministic

No RNG needed for normal gains.

## 216T.4 Injury risk

If stochastic:
- canonical medical seeded RNG
with session risk ID.

## 216T.5 Reload

Same injury/no injury.

## 216T.6 Session replay

No duplicate gain.

## 216T.7 Pause/resume

Actual time only.

## 216T.8 Cancel/restart

Cannot repeatedly harvest partial-session benefits.

## 216T.9 Same low workout

Plateaus.

## 216T.10 Max-intensity spam

Recovery debt/injury risk prevents optimal abuse.

## 216T.11 Streak

Cosmetic/statistical only.

No giant compounding bonus.

## 216T.12 Work-as-training

Bounded.

Player cannot create useless hauling loop for free fitness if no real work output.

Use:
- productive activity events;
- caps.

## 216T.13 Combat-as-training

Bounded.

## 216T.14 Expedition walking farm

Normal activity contributes maintenance;
not infinite rapid gain.

## 216T.15 Schedule exploit

Cannot train while simultaneously:
- working;
- sleeping;
- hospitalized.

## 216T.16 Equipment duplication

Training does not consume/create exercise gear unless canonical system says.

## 216T.17 Old-save neutral profile

Not exploitable by repeated migration.

## 216T.18 No GUID

## 216T.19 No wall clock

### 216T DoD

Physical development comes from real completed and recoverable activity, not reloads, duplicated time, streaks, low-load spam or overlapping schedules.

---

# TASK 216U — Performance & Scaling

# 216U.0 Goal

Support large survivor rosters without expensive per-frame physiology simulation.

## 216U.1 Event-driven sessions

## 216U.2 Daily adaptation boundary

Acceptable.

## 216U.3 No per-frame conditioning updates

## 216U.4 Lazy deconditioning

Compute from:
- last meaningful load;
- rolling history
when queried/day boundary.

## 216U.5 Active scheduled sessions

O(active sessions).

## 216U.6 Profiles

O(survivors) daily acceptable at normal scale.

## 216U.7 Large shelter benchmark

20 / 50 / 100 survivors.

## 216U.8 Routine catalog

Pre-index by:
- family;
- equipment;
- facility;
- eligibility.

## 216U.9 Capability queries

O(1) profile read.

## 216U.10 History bounded

## 216U.11 Activity load ingestion

Coalesce daily workload.

## 216U.12 UI

Load details on demand.

## 216U.13 State-size budget

Set explicit.

### 216U DoD

Conditioning cost scales with survivors and completed physical activities rather than frames, item count or pairwise interactions.

---

# TASK 216V — Long-Horizon Balance & Simulation

# 216V.0 Goal

Prove exercise creates visible but bounded development without becoming mandatory optimization.

## 216V.1 30-day no-exercise parity

Sedentary survivor.

Expected:
- mild/no early decline depending baseline;
- no sudden nerf.

## 216V.2 30-day moderate training

Expected:
- visible early improvement;
- manageable fatigue.

## 216V.3 30-day extreme training

Expected:
- recovery debt;
- plateau;
- medical risk.

Not 3× progress.

## 216V.4 120-day structured program

Observe:
- progressive overload;
- plateau;
- routine changes.

## 216V.5 120-day physical-worker scenario

Heavy labor maintains some dimensions without gym.

## 216V.6 120-day sedentary specialist

Requires deliberate exercise for maintenance.

## 216V.7 180-day expedition specialist

Travel load preserves aerobic/work capacity.

## 216V.8 180-day combat survivor

Combat load does not become best training source.

## 216V.9 180-day injury/recovery scenario

Medical restrictions;
detraining;
safe return.

## 216V.10 400-day young survivor

Gain → plateau → maintenance → detraining → retraining.

## 216V.11 400-day aging survivor

Exercise helps preserve capacity but age still matters.

## 216V.12 Starved survivor

Training blocked/reduced.

## 216V.13 Exhausted survivor

Training blocked/reduced.

## 216V.14 Fully rested survivor

Expected adaptation.

## 216V.15 High physical trait

Higher potential/response if designed.

## 216V.16 Low physical trait

Can still improve.

## 216V.17 Mobility-only routine

Does not grant major strength.

## 216V.18 Strength-only routine

Does not max cardio.

## 216V.19 Mixed routine

Balanced but less specific.

## 216V.20 Combat drill

Physical conditioning + skill XP exactly once.

## 216V.21 Old save

Exact capability parity fixture.

## 216V.22 Auto-maintain

Works without player micromanagement.

## 216V.23 Many survivors

Scheduler/performance stable.

---

# TASK 216W — Testing & CI

# 216W.0 Goal

Make conditioning ownership, capability stacking, medical risk, persistence and balance continuously verifiable.

## 216W.1 Data integrity

Validate:
- routine IDs;
- load vectors;
- duration/intensity;
- equipment refs;
- facility refs;
- skill refs;
- exclusion tags;
- localization;
- coach/partner requirements.

## 216W.2 Selftest

Create:

```text
--exercise-selftest
```

## 216W.3 Selftest scenarios

At least:

1. old-save capability parity;
2. no-exercise baseline;
3. bodyweight routine;
4. equipment-gated routine;
5. scheduled session consumes time;
6. interrupted session;
7. hunger restriction;
8. fatigue restriction;
9. medical block;
10. adaptation;
11. progressive overload;
12. plateau;
13. overtraining;
14. recovery;
15. deconditioning;
16. manual work maintenance;
17. expedition maintenance;
18. combat drill skill separation;
19. medical injury handoff;
20. age interaction;
21. trait interaction;
22. save/load;
23. session idempotence;
24. headless.

## 216W.4 Source-scan authority gate

Detect:
- duplicate trait/base physical attributes;
- local injury state;
- direct work multiplier;
- direct combat multiplier;
- direct expedition multiplier;
- local disease-resistance scalar;
- duplicate combat skill;
- direct shift extension;
- per-frame loop;
- unseeded RNG.

## 216W.5 Content acceptance

Routine ladder:

```text
DISCOVERED
LOADED
REGISTERED
REQUIREMENTS_RESOLVED
SESSION_SCHEDULED
LOAD_PRODUCED
ADAPTATION_OBSERVED
CAPABILITY_CONSUMER_OBSERVED
```

## 216W.6 Dead-routine gate

No routine:
- impossible requirements;
- no load;
- no useful consumer.

## 216W.7 Dead-effect gate

Every conditioning effect has:
- canonical consumer.

## 216W.8 Capability single-stack gate

No consumer applies:
- conditioning twice.

## 216W.9 Medical-authority gate

All exercise injury events resolve through medical system.

## 216W.10 Skill-authority gate

Combat drill XP only SkillProgression.

## 216W.11 Old-save parity gate

Representative fixtures within tolerance.

## 216W.12 Deterministic golden fixtures

Fixed:
- survivor;
- routine;
- duration;
- needs;
- age;
- traits
→ exact load/adaptation.

## 216W.13 Anti-farm gate

## 216W.14 Long-horizon state-size gate

## 216W.15 Performance benchmark

## 216W.16 Generated docs

Create:
- `EXERCISE_ARCHITECTURE.md`;
- `PHYSICAL_CONDITIONING_AUTHORITY_MATRIX.md`;
- `EXERCISE_ROUTINE_MATRIX.md`;
- `EXERCISE_LOAD_MATRIX.md`;
- `EXERCISE_CAPABILITY_CONSUMER_MATRIX.md`;
- `EXERCISE_MEDICAL_EFFECT_MATRIX.md`;
- `EXERCISE_DECONDITIONING_MATRIX.md`;
- `EXERCISE_MIGRATION_MATRIX.md`;
- `EXERCISE_BALANCE_REPORT.md`;
- `ADR_CONDITIONING_VS_PHYSICAL_CAPABILITY.md`;
- `ADR_EXERCISE_VS_SKILL_PROGRESSION.md`;
- `ADR_EXERCISE_INJURY_AUTHORITY.md`;
- `ADR_EXERCISE_OLD_SAVE_CAPABILITY_PARITY.md`.

### 216W DoD

Every conditioning change can be traced from real physical load through recovery/adaptation into one canonical capability projection, with medical and skill consequences delegated exactly once.

---

# TASK 216X — Exercise Events, Milestones & Quest Hooks

# 216X.0 Goal

Expose meaningful development without rewarding repetitive workout bookkeeping.

## 216X.1 Semantic events

Candidate:

```text
training_session_completed
conditioning_band_changed
conditioning_milestone_reached
training_overload_detected
exercise_injury_event
conditioning_deconditioned
training_plan_established
```

## 216X.2 No event for every +0.1

## 216X.3 Source narrative names

- The Workout;
- The Improvement;
- The Injury;
- The Decline;
- The Milestone;
- The Competition;
- The Routine;
- The Deconditioning.

Treat as authored narrative candidates.

## 216X.4 “The Workout”

Routine event feed, not journal every time.

## 216X.5 “The Improvement”

Only meaningful band/milestone.

## 216X.6 “The Injury”

Medical event may already own narrative.

Deduplicate.

## 216X.7 “The Decline/Deconditioning”

Use only significant functional loss.

## 216X.8 “The Competition”

Follow-on event requiring:
- participants;
- rules;
- reward.

## 216X.9 “The Routine”

Could be tutorial/planning milestone.

## 216X.10 Plan 171 hook surface

Expose:
- conditioning band;
- training program;
- expedition prep;
- rehab completion;
- overtraining risk.

## 216X.11 Source quests

- Athlete 90+;
- Trainer 50 sessions;
- Marathon 30-day streak;
- Strong 90+;
- Flexible 90+;
- Endurer 90+;
- Coach train 10 survivors.

Treat as backlog.

## 216X.12 Reject session-count grind by default

## 216X.13 Reject mandatory 30-day streak

Rest should be healthy.

## 216X.14 Attribute-90 goals

Could encourage unsafe optimization.

Prefer:
- prepare survivor for specific expedition;
- restore deconditioned survivor;
- maintain team readiness through winter;
- complete safe combat-conditioning program.

## 216X.15 Coach

Only when canonical role/skill support exists.

### 216X DoD

Exercise narrative content rewards meaningful readiness and development rather than raw session counts, arbitrary 90-point bars or unhealthy streak behavior.

---

# TASK 216Y — Advanced Coaches, Competitions, Rehabilitation & Training Camps: Explicit Follow-On

# 216Y.0 Goal

Keep the base conditioning system focused.

## 216Y.1 Coach role

Use:
- SkillProgression;
- Plan 195 specialization roles.

## 216Y.2 Coach effects

Potential:
- better routine selection;
- adherence;
- safe load;
not free gains.

## 216Y.3 Competitions

Need:
- event system;
- participants;
- rules.

Follow-on.

## 216Y.4 Training camp

Could be:
- temporary schedule/facility program.

## 216Y.5 External training service

Trade/service system.

## 216Y.6 Exercise trading

Source phrase:
- “trade training services.”

Reframe:
- hire coach;
- attend training;
- teach settlement
through services/skills/diplomacy.

## 216Y.7 Rehabilitation

Medical/therapy plan.

## 216Y.8 Prosthetic adaptation

Medical/amputation follow-on.

## 216Y.9 Children/elderly routines

Age-specific content only with appropriate systems.

## 216Y.10 Team sports

Leisure/social follow-on.

## 216Y.11 Famous achievements

Plan 162/archive.

## 216Y.12 Body composition

Out of scope.

### 216Y DoD

Coaches, competitions, camps and rehabilitation remain compositional follow-ons over conditioning, SkillProgression, medicine, events and service systems rather than expanding ExerciseSystem into a universal physiology simulator.

---

# 5. Core Conditioning Lifecycle

```text
SURVIVOR SCHEDULE / PHYSICAL ACTIVITY
              │
              ▼
      PhysicalLoadObservation
              │
              ├── exercise
              ├── work
              ├── expedition
              └── combat
              │
              ▼
       LOAD + RECOVERY
              │
              ▼
       ADAPTATION MODEL
              │
              ├── improve
              ├── maintain
              ├── plateau
              ├── overreach
              └── decondition
              │
              ▼
PhysicalConditioningContribution
              │
              ▼
    CANONICAL CAPABILITY STACK
```

---

# 6. Conditioning vs Traits Contract

Traits:
- innate/persistent predisposition.

Conditioning:
- trainable adaptation.

---

# 7. Conditioning vs Age Contract

Age:
- baseline physiological change.

Conditioning:
- adaptation within age-related constraints.

---

# 8. Conditioning vs Needs Contract

Needs:
- acute readiness.

Conditioning:
- long-term capacity.

---

# 9. Conditioning vs Skill Contract

Skill:
- learned technique/knowledge.

Conditioning:
- physical adaptation.

---

# 10. Conditioning vs Medical Contract

Medical:
- injury/illness and eligibility.

Conditioning:
- not a diagnosis.

---

# 11. Conditioning vs Final Capability Contract

Consumers do not read fitness ad hoc.

They query:
- canonical projection.

---

# 12. Aerobic Contract

Aerobic:
- sustained exertion capacity.

Not:
- raw movement speed.

---

# 13. Force Contract

Force conditioning:
- trainable force contribution.

Not:
- universal damage multiplier.

---

# 14. Mobility Contract

Mobility:
- movement range/quality.

Not:
- dodge chance by itself.

---

# 15. Work-Capacity Contract

Work capacity:
- repeated effort/fatigue resistance.

Not:
- permission for unsafe longer shifts.

---

# 16. Training Session Contract

Session:
- scheduled time + actual physical load.

Not:
- instant stat increment.

---

# 17. Progressive Overload Contract

Repeated stimulus must exceed:
- current maintenance challenge
for adaptation.

---

# 18. Recovery Contract

Training + no recovery:
- does not maximize gains.

---

# 19. Deconditioning Contract

Deconditioning:
- insufficient physical load.

Not:
- no explicit workout checkbox.

---

# 20. Physical Work Contract

Work can maintain/train relevant dimensions.

It also creates fatigue/recovery load.

---

# 21. Expedition Contract

Travel contributes load.
Conditioning contributes capability.

No duplicate stamina system.

---

# 22. Combat Contract

Combat contributes limited load.
Conditioning contributes physical readiness.

Skill/combat outcome remains external.

---

# 23. Medical Injury Contract

Exercise emits risk context.
Medical system owns injury.

---

# 24. Combat Drill Contract

Exercise:
- physical load.

SkillProgression:
- combat XP.

---

# 25. Auto-Exercise Contract

Auto mode:
- schedule preference.

Not:
- automatic free training tick.

---

# 26. Streak Contract

Streak:
- analytics/cosmetic.

Not:
- mandatory adaptation multiplier.

---

# 27. Health Contract

No generic disease immunity.

Only explicitly supported medical consumers.

---

# 28. Old-Save Contract

Migration preserves:
- old effective capability.

No arbitrary hidden nerf.

---

# 29. Persistence Matrix

| Fact | Owner |
|---|---|
| physical traits | TraitSystem |
| age | Plan 176 |
| hunger/fatigue/sleep | NeedsSystem |
| medical injuries | medical |
| combat skill | SkillProgression |
| job skill | SkillProgression |
| duty/schedule | DutyRoster / routines |
| conditioning | ExerciseSystem |
| training load | ExerciseSystem |
| recovery debt | ExerciseSystem |
| deconditioning | ExerciseSystem |
| final work capability | Plan 137/24 |
| final expedition capability | Expedition/capability adapter |
| final combat capability | combat/capability adapter |
| carrying rules | inventory/encumbrance |
| injury result | medical |
| disease risk | DiseaseSystem |

---

# 30. Old-Save Migration Matrix

Old save:

```text
exercise schema absent
```

Migration:
1. create conditioning profile at parity baseline;
2. initialize load/recovery history neutral;
3. no synthetic past sessions;
4. no synthetic streaks;
5. no retroactive deconditioning;
6. preserve effective capability within tolerance.

---

# 31. Exactly-Once Identity

Examples:

```text
training:<survivor>:<schedule_job>:<sequence>
physical-load:<source-system>:<source-event>
adaptation:<survivor>:<day>:<dimension>
exercise-risk:<session>
skill-practice:<session>:<skill>
```

---

# 32. Failure Injection Matrix

## N216.1 Old save initializes fitness 50 and loses 25% work efficiency
Expected: migration parity gate fails.

## N216.2 ExerciseSystem directly multiplies combat damage
Expected: capability authority gate fails.

## N216.3 Combat drill increments local combat skill
Expected: skill authority gate fails.

## N216.4 Exercise creates its own fracture record
Expected: medical authority gate fails.

## N216.5 Survivor doing 8 hours heavy labor deconditions because no workout
Expected: activity-equivalence gate fails.

## N216.6 Auto-exercise gives daily progress without scheduled time
Expected: scheduler gate fails.

## N216.7 Same push-up session gives +1 force forever
Expected: progressive-overload gate fails.

## N216.8 Extreme daily training is optimal
Expected: recovery/overtraining gate fails.

## N216.9 Conditioning directly lowers infection chance with no DiseaseSystem input
Expected: medical-effect gate fails.

## N216.10 Survivor trains while hospitalized and simultaneously working
Expected: schedule/medical eligibility gate fails.

## N216.11 Reload rerolls exercise injury
Expected: deterministic risk gate fails.

## N216.12 Rest day destroys streak and applies fitness penalty
Expected: anti-chore design gate fails.

---

# 33. Determinism Contract

Same:

```text
conditioning profile
+ physical load
+ recovery state
+ age
+ traits
+ Needs state
+ medical eligibility
+ routine policy
```

must yield same:
- effective load;
- recovery cost;
- adaptation;
- deconditioning.

Medical RNG, if used for injury, is seeded and owned by medical authority.

---

# 34. Long-Horizon Metrics

Track:

```text
sessions scheduled
sessions completed
sessions interrupted
training hours
physical load by source
conditioning by dimension
adaptation events
plateaus
overtraining periods
deconditioning events
manual-work maintenance
expedition-load contribution
combat-load contribution
medical training injuries
combat skill practice
old-save parity delta
auto-plan compliance
state bytes
daily processing time
```

---

# 35. Balance Guardrails

Exercise should:
- reward planning;
- support specialists;
- create development.

It should not:
- become mandatory daily maintenance;
- overshadow traits/skills;
- make every survivor a super-athlete.

---

# 36. Gain Guardrails

Early gains:
- visible.

Late gains:
- slower.

Plateau:
- expected.

---

# 37. Deconditioning Guardrails

A few rest days:
- fine.

Long inactivity:
- gradual loss.

---

# 38. Overtraining Guardrails

Harder ≠ always better.

Recovery matters.

---

# 39. Age Guardrails

Exercise:
- improves/preserves.

Age:
- still matters.

---

# 40. Work Guardrails

Physical jobs:
- partially self-maintaining.

Sedentary roles:
- benefit more from formal exercise.

---

# 41. Combat Guardrails

Conditioning:
- bounded contributor.

Combat skill/equipment:
- remain decisive.

---

# 42. Expedition Guardrails

Fitness helps:
- stamina/readiness.

It does not negate:
- weather;
- injury;
- hunger;
- radiation;
- poor equipment.

---

# 43. UI Acceptance

## Survivor
- conditioning;
- readiness;
- trend.

## Routine
- load;
- time;
- requirements;
- recovery.

## Plan
- maintain/improve/prepare.

## History
- weekly/milestones.

---

# 44. Accessibility

- trend not color-only;
- keyboard/controller;
- no hover-only values;
- text scale;
- screen-reader;
- semantic intensity labels.

---

# 45. Localization

Routine:
- localization keys.

No localized strings in simulation state.

---

# 46. Content Acceptance

Routine ladder:

```text
DISCOVERED
LOADED
REGISTERED
REQUIREMENTS_BOUND
SCHEDULE_PATH_REACHED
SESSION_EXECUTED
LOAD_RECORDED
ADAPTATION_RECORDED
CAPABILITY_CONSUMER_REACHED
```

---

# 47. Reachability

Every routine:
- deterministic fixture;
- requirements fixture;
- load vector;
- at least one meaningful adaptation.

---

# 48. Performance Guardrails

- event/day-boundary;
- no frame loop;
- O(survivors + active sessions);
- bounded history;
- cached routine indexes.

---

# 49. CI / Gate Set

Recommended:

```text
exercise_authority_matrix
exercise_conditioning_vs_capability
exercise_skill_authority
exercise_medical_authority
exercise_old_save_capability_parity
exercise_schedule_time_integrity
exercise_activity_equivalence
exercise_progressive_overload
exercise_recovery_overtraining
exercise_deconditioning
exercise_consumer_single_stack
exercise_antifarm
exercise_determinism
exercise_long_horizon
exercise_large_roster_performance
exercise_ui_access
```

---

# 50. Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --exercise-selftest
godot --headless --path . -- --real-campaign-journey-selftest
bash scripts/ci/content-acceptance-gate.sh
bash scripts/ci/verify-fast.sh
```

Use exact current command names if renamed.

---

# 51. Recommended Commit Breakdown

```text
216A-1 capability authority audit
216A-2 conditioning-vs-capability ADR
216A-3 exercise-vs-skill ADR
216A-4 exercise-injury ADR
216A-5 old-save parity ADR
216A-6 ConditioningProfile DTO/state
216A-7 neutral baseline calibration
216A-8 docs/tests

216B-1 exercise routine schema
216B-2 routine loader
216B-3 load vectors
216B-4 requirements/facility/equipment refs
216B-5 medical exclusions
216B-6 risk profiles
216B-7 routine reachability
216B-8 docs

216C-1 training session intent
216C-2 scheduler integration
216C-3 auto-exercise policy
216C-4 eligibility
216C-5 partial/interrupted sessions
216C-6 Needs exertion output
216C-7 exactly-once session
216C-8 tests/docs

216D-1 effective load calculation
216D-2 specificity
216D-3 progressive overload
216D-4 maintenance/productive/excessive bands
216D-5 diminishing returns
216D-6 plateau
216D-7 trait/age modifiers
216D-8 tests/docs

216E-1 recovery debt
216E-2 recovery integration
216E-3 overreach
216E-4 overtraining
216E-5 readiness projection
216E-6 rest/recovery session
216E-7 scheduler readiness
216E-8 tests/docs

216F-1 PhysicalLoadObservation
216F-2 work-load adapter
216F-3 expedition-load adapter
216F-4 combat-load adapter
216F-5 dimension maintenance thresholds
216F-6 deconditioning profiles
216F-7 retraining/training-age decision
216F-8 tests/docs

216G-1 TraitSystem audit
216G-2 trainability modifiers
216G-3 potential modifiers
216G-4 recovery/risk modifiers
216G-5 no-trait-duplication gate
216G-6 UI explainability
216G-7 tests/docs

216H-1 Plan-176 age adapter
216H-2 age recovery
216H-3 age potential
216H-4 age deconditioning
216H-5 preservation without age reversal
216H-6 400-day aging fixture
216H-7 docs/tests

216I-1 Needs eligibility
216I-2 fatigue/hunger/hydration thresholds
216I-3 exertion costs
216I-4 sleep/recovery
216I-5 stress integration
216I-6 Plan-137 double-count guard
216I-7 tests/docs

216J-1 medical eligibility API
216J-2 routine exclusion tags
216J-3 exercise injury-risk observation
216J-4 medical injury resolver
216J-5 supported injury catalog mapping
216J-6 re-injury/training restriction
216J-7 exactly-once risk result
216J-8 tests/docs

216K-1 combat-drill load
216K-2 SkillProgression practice event
216K-3 sparring/partner requirements
216K-4 no-local-combat-skill gate
216K-5 skill anti-farm
216K-6 tests/docs

216L-1 job physical-demand tags
216L-2 Plan-137/24 conditioning adapter
216L-3 heavy-labor contribution
216L-4 shift-fatigue integration
216L-5 work-as-load observation
216L-6 capability trace
216L-7 tests/docs

216M-1 expedition conditioning context
216M-2 aerobic/work-capacity effect
216M-3 force/mobility task hooks
216M-4 encumbrance boundary
216M-5 expedition physical-load return
216M-6 selection UI
216M-7 tests/docs

216N-1 combat conditioning context
216N-2 physical-domain bounded effects
216N-3 no direct damage/accuracy gate
216N-4 combat workload return
216N-5 anti-combat-training-farm
216N-6 Plan-137/143 integration
216N-7 tests/docs

216O-1 medical-effect audit
216O-2 recovery/readiness supported effects
216O-3 no-disease-immunity regression
216O-4 medical-effect matrix
216O-5 unsupported effects defer
216O-6 tests/docs

216P-1 autonomy integration
216P-2 exercise preferences
216P-3 Plan-161 physical hobbies
216P-4 maintain/improve/combat/expedition presets
216P-5 refusal/override
216P-6 training partner behavior
216P-7 tests/docs

216Q-1 facility/equipment audit
216Q-2 bodyweight routines
216Q-3 safe running space/weather/radiation
216Q-4 weights/improvised load
216Q-5 combat training area
216Q-6 capacity
216Q-7 reachability tests/docs

216R-1 fitness/conditioning panel
216R-2 readiness/trend
216R-3 routine detail
216R-4 training plan automation
216R-5 weekly history/progress
216R-6 alert budget
216R-7 accessibility/localization
216R-8 tutorial/snapshots

216S-1 persistence schema
216S-2 old-save neutral migration
216S-3 parity oracle tests
216S-4 restore ordering
216S-5 missing survivor/routine migration
216S-6 session replay prevention
216S-7 history compaction
216S-8 docs/tests

216T-1 stable session IDs
216T-2 deterministic load/adaptation
216T-3 medical RNG handoff
216T-4 partial-session anti-farm
216T-5 low-load/max-intensity anti-farm
216T-6 work/combat/expedition load caps
216T-7 schedule overlap guard
216T-8 exploit tests

216U-1 event-driven scheduler
216U-2 lazy deconditioning
216U-3 routine indexes
216U-4 20-survivor benchmark
216U-5 50-survivor benchmark
216U-6 100-survivor benchmark
216U-7 state-size gate

216V-1 30-day parity
216V-2 30-day moderate
216V-3 30-day extreme
216V-4 120-day structured
216V-5 120-day physical worker
216V-6 120-day sedentary specialist
216V-7 180-day expedition/combat/injury
216V-8 400-day young/aging
216V-9 balance report

216W-1 selftest
216W-2 source-scan authority gates
216W-3 content acceptance
216W-4 failure fixtures
216W-5 deterministic goldens
216W-6 long-horizon/performance gates
216W-7 final ship/no-ship report

216X-1 semantic exercise events
216X-2 Plan-171 hook surface
216X-3 narrative event budget
216X-4 anti-grind quest review

216Y-1 coach/competition/rehab/training-camp follow-on disposition
```

---

# 52. Risk Register

## R216.1 Fitness duplicates physical capability

Mitigation:
- conditioning-only state;
- canonical capability projection.

## R216.2 New system nerfs old saves

Mitigation:
- capability parity migration oracle.

## R216.3 Daily exercise becomes mandatory chore

Mitigation:
- work/activity maintenance;
- auto-maintain plans;
- no streak bonuses.

## R216.4 Extreme training dominates

Mitigation:
- recovery debt;
- diminishing returns;
- medical risk.

## R216.5 Injury system duplicated

Mitigation:
- medical injury-risk handoff.

## R216.6 Combat skill duplicated

Mitigation:
- SkillProgression practice event.

## R216.7 Deconditioning punishes physical workers

Mitigation:
- unified physical-load observations.

## R216.8 Exercise overpowers aging

Mitigation:
- age-owned potential/base decline.

## R216.9 Fitness multiplies every consumer independently

Mitigation:
- one capability adapter;
- single-stack CI gate.

## R216.10 Routine content is unreachable

Mitigation:
- equipment/facility/content acceptance gates.

---

# 53. Acceptance Checklist

## P0

- [ ] TraitSystem audited
- [ ] Plan 176 aging audited
- [ ] NeedsSystem audited
- [ ] medical/affliction/injury audited
- [ ] Plan 143 capability bridge audited
- [ ] Plan 137 performance cascade audited
- [ ] Plan 24 eligibility audited
- [ ] SkillProgressionSystem audited
- [ ] combat formulas audited
- [ ] ExpeditionSystem audited
- [ ] DutyRoster/work efficiency audited
- [ ] encumbrance/carrying audited
- [ ] Plan 188 routines audited
- [ ] Plan 161 hobbies audited
- [ ] survivor autonomy audited
- [ ] facility/equipment systems audited
- [ ] semantic events audited
- [ ] campaign clock audited
- [ ] save order audited
- [ ] capability UI audited
- [ ] authority matrix published
- [ ] conditioning-vs-capability ADR
- [ ] exercise-vs-skill ADR
- [ ] exercise injury ADR
- [ ] old-save parity ADR
- [ ] representative baseline oracle recorded

## 216A — Conditioning State

- [ ] ConditioningProfile created
- [ ] aerobic conditioning
- [ ] force conditioning
- [ ] mobility conditioning
- [ ] work-capacity conditioning
- [ ] no primary overallFitness truth
- [ ] no duplicate baseValue
- [ ] no final currentValue duplicate
- [ ] scale calibrated
- [ ] neutral baseline defined
- [ ] old-save parity-safe
- [ ] potential derived from canonical sources
- [ ] training age
- [ ] trend derived
- [ ] bounded history
- [ ] meaningful load timestamp per dimension
- [ ] recovery debt distinct from Needs fatigue
- [ ] restore no replay

## 216B — Routine Data

- [ ] versioned routine file
- [ ] stable routine IDs
- [ ] localization keys
- [ ] routine families
- [ ] duration options
- [ ] intensity bands
- [ ] load vector
- [ ] movement tags
- [ ] equipment requirements
- [ ] facility requirements
- [ ] skill requirements
- [ ] medical exclusion tags
- [ ] Needs thresholds
- [ ] recovery profile
- [ ] injury-risk profile
- [ ] combat practice profile optional
- [ ] group/coach metadata
- [ ] running reachable
- [ ] cycling only if equipment real
- [ ] bodyweight routines reachable
- [ ] weightlifting real requirements
- [ ] pull-up facility real
- [ ] mobility routine
- [ ] combat drill split
- [ ] mixed routine specific
- [ ] direct +attribute benefits removed
- [ ] 15+ not quota
- [ ] integrity

## 216C — Scheduling

- [ ] session intent
- [ ] canonical scheduler
- [ ] training consumes time
- [ ] conflicts with work
- [ ] conflicts with sleep
- [ ] conflicts with treatment
- [ ] conflicts with caregiving
- [ ] conflicts with guard duty
- [ ] auto-exercise is scheduling policy
- [ ] no free auto tick
- [ ] alive/conscious check
- [ ] medical eligibility
- [ ] hunger threshold
- [ ] hydration threshold
- [ ] fatigue threshold
- [ ] equipment/facility
- [ ] partner/coach
- [ ] partial session
- [ ] completion from actual time
- [ ] effective load result
- [ ] recovery cost
- [ ] Needs cost refs
- [ ] injury risk ref
- [ ] skill practice ref
- [ ] no arbitrary benefits dict
- [ ] exertion to Needs
- [ ] interruption no full reward
- [ ] emergency precedence
- [ ] session exactly once
- [ ] headless

## 216D — Load/Adaptation

- [ ] load vector normalized
- [ ] effective load formula
- [ ] specificity
- [ ] progressive overload
- [ ] underload maintenance
- [ ] productive load
- [ ] excessive load
- [ ] diminishing returns
- [ ] plateau
- [ ] new stimulus
- [ ] no session-count gain
- [ ] trait trainability
- [ ] age adaptation
- [ ] injury/illness suppression
- [ ] nutrition if supported
- [ ] rolling load
- [ ] simplified MVP
- [ ] no sports-science overcomplexity

## 216E — Recovery

- [ ] recovery debt
- [ ] distinct from fatigue
- [ ] load/intensity inputs
- [ ] age input
- [ ] sleep input
- [ ] nutrition input
- [ ] medical input
- [ ] overreach
- [ ] overtraining
- [ ] no new disease
- [ ] rest useful
- [ ] recovery session
- [ ] injury recovery medical-owned
- [ ] rehab deferred unless real
- [ ] readiness states
- [ ] auto scheduler respects readiness
- [ ] no streak punishment

## 216F — Deconditioning

- [ ] lastExerciseDay-only rule rejected
- [ ] structured exercise load
- [ ] heavy work load
- [ ] hauling load
- [ ] construction load
- [ ] expedition load
- [ ] combat load
- [ ] chore load where real
- [ ] PhysicalLoadObservation
- [ ] maintenance thresholds
- [ ] inactivity delay
- [ ] gradual loss
- [ ] bed-rest acceleration
- [ ] age effect
- [ ] retraining decision
- [ ] dimension-specific decay
- [ ] no journal spam
- [ ] threshold alerts
- [ ] work specificity
- [ ] expedition specificity
- [ ] sedentary role distinction

## 216G — Traits

- [ ] physical traits audited
- [ ] trainability modifier
- [ ] potential modifier
- [ ] recovery modifier
- [ ] risk modifier only if justified
- [ ] athletic trait only if real
- [ ] strong trait remains base capability
- [ ] no trait copy
- [ ] no trait mutation
- [ ] potential curve not crude arbitrary cap
- [ ] UI innate vs trained

## 216H — Age

- [ ] Plan 176 remains age owner
- [ ] age recovery modifier
- [ ] age risk modifier
- [ ] age adaptation modifier
- [ ] age detraining modifier
- [ ] no age reversal
- [ ] older survivors can improve
- [ ] conditioning can beat unconditioned younger in bounded domains
- [ ] base age decline remains
- [ ] exercise preserves but does not cancel age
- [ ] safe prescription
- [ ] age UI non-deterministic language
- [ ] 400-day aging sim

## 216I — Needs

- [ ] Needs owns hunger
- [ ] Needs owns fatigue
- [ ] Needs owns sleep
- [ ] Needs owns hydration if present
- [ ] severe hunger blocks hard session
- [ ] severe fatigue blocks
- [ ] dehydration blocks if modeled
- [ ] mild deficits reduce session quality
- [ ] exertion emitted
- [ ] no duplicate fatigue
- [ ] post-session Needs update
- [ ] no sports nutrition minigame
- [ ] no unsupported macro model
- [ ] poor sleep reduces recovery
- [ ] stress integration
- [ ] illness medical-owned
- [ ] Plan-137 double-count guard

## 216J — Medical

- [ ] eligibility query
- [ ] real affliction inputs only
- [ ] blocked-routine explanation
- [ ] modified routine if supported
- [ ] risk observation
- [ ] medical resolver
- [ ] no authoritative local injury bool
- [ ] supported strain/sprain/fracture mapping only
- [ ] overexertion mapping real
- [ ] mobility injury-risk contribution bounded
- [ ] injured training restricted
- [ ] reinjury medical-owned
- [ ] rehab deferred/bridged
- [ ] risk result exactly once

## 216K — Skill

- [ ] combat drill physical load
- [ ] combat drill practice event
- [ ] no local combat skill
- [ ] partner availability
- [ ] coach availability
- [ ] sparring medical risk
- [ ] no live combat damage
- [ ] weapon drill only if safe path
- [ ] strength routine no combat XP
- [ ] endurance routine no weapon XP
- [ ] SkillProgression anti-farm
- [ ] relation effect not daily auto boost

## 216L — Work

- [ ] physical job tags
- [ ] force demand
- [ ] aerobic demand
- [ ] work-capacity demand
- [ ] mobility demand
- [ ] Plan-137/24 projection
- [ ] no universal work bonus
- [ ] heavy labor benefit
- [ ] long-shift semantics bounded
- [ ] no automatic shift-extension permission
- [ ] physical work returns load
- [ ] overwork adds recovery stress
- [ ] medical eligibility first
- [ ] capability trace

## 216M — Expedition

- [ ] ExpeditionSystem remains owner
- [ ] conditioning context
- [ ] aerobic effect bounded
- [ ] work-capacity effect bounded
- [ ] force only relevant tasks
- [ ] mobility only relevant tasks
- [ ] no direct route success
- [ ] encumbrance owns carry rules
- [ ] no huge carry bonus
- [ ] expedition returns load
- [ ] Needs owns return fatigue
- [ ] sustained workload can add recovery debt
- [ ] expedition injury external
- [ ] selection UI

## 216N — Combat

- [ ] combat authority audited
- [ ] no direct damage write
- [ ] no direct accuracy write
- [ ] no armor/health write
- [ ] force context bounded
- [ ] aerobic/work-capacity combat fatigue
- [ ] mobility context bounded
- [ ] no universal flat combat bonus
- [ ] combat skill separate
- [ ] weapon handling skill/equipment
- [ ] injury medical-owned
- [ ] combat activity load
- [ ] anti-combat-training farm
- [ ] Plan-137/143 one-stack trace

## 216O — Health

- [ ] no direct disease-resistance scalar
- [ ] medical-supported recovery effects only
- [ ] no infection-probability change by default
- [ ] no generic immune stat
- [ ] injury recovery only if supported
- [ ] overtraining readiness not infection
- [ ] malnutrition external
- [ ] age external
- [ ] medical effect matrix
- [ ] unsupported claims deferred

## 216P — Autonomy/Preferences

- [ ] Plan 144 audited
- [ ] exercise can be ordered/suggested/self-selected
- [ ] preference derives from existing systems
- [ ] Plan-161 physical hobbies reused
- [ ] no motivation meter
- [ ] refusal supported
- [ ] governance override
- [ ] maintain preset
- [ ] gentle improvement preset
- [ ] combat conditioning preset
- [ ] expedition prep preset
- [ ] recovery preset
- [ ] canonical schedule
- [ ] no streak requirement
- [ ] partners optional
- [ ] partner relations contextual
- [ ] coach follow-on if unsupported

## 216Q — Facilities

- [ ] bodyweight no-equipment path
- [ ] running safe-space requirement
- [ ] weather blocks outdoors when relevant
- [ ] radiation blocks outdoors when relevant
- [ ] real weight equipment
- [ ] improvised weight path if authored
- [ ] pull-up fixture
- [ ] training mat optional
- [ ] combat drill area
- [ ] shelter space uses existing facility tags
- [ ] no new room system
- [ ] power not required unnecessarily
- [ ] equipment wear only if supported
- [ ] facility capacity
- [ ] unreachable routines removed

## 216R — UI

- [ ] conditioning summary
- [ ] trend
- [ ] readiness
- [ ] recent load
- [ ] recommended focus
- [ ] semantic dimensions
- [ ] exact scores optional
- [ ] overall derived only
- [ ] routine duration
- [ ] intensity
- [ ] focus
- [ ] requirements
- [ ] recovery demand
- [ ] restrictions
- [ ] training presets
- [ ] bounded history
- [ ] weekly graph
- [ ] deconditioning threshold alert
- [ ] high-value alert budget
- [ ] no every-session popup
- [ ] tutorial
- [ ] capability explanation
- [ ] no color-only
- [ ] keyboard/controller
- [ ] no hover-only
- [ ] text scale
- [ ] screen-reader
- [ ] automation sufficient for casual play
- [ ] neutral non-shaming terminology
- [ ] no body-composition system

## 216S — Persistence

- [ ] schema version
- [ ] activation day
- [ ] conditioning profiles
- [ ] recent load
- [ ] recovery debt
- [ ] scheduled refs external
- [ ] processed session IDs
- [ ] training sequence
- [ ] weekly samples
- [ ] no trait duplicate
- [ ] no age duplicate
- [ ] no Needs duplicate
- [ ] no injury duplicate
- [ ] no medical eligibility duplicate
- [ ] no skill duplicate
- [ ] no work performance duplicate
- [ ] no expedition/combat state duplicate
- [ ] old save neutral parity
- [ ] not blindly 50
- [ ] active survivors not retroactively inferred
- [ ] new-game generation deterministic
- [ ] restore order
- [ ] missing survivor safe
- [ ] changed routine retains resolved result
- [ ] trend recomputable
- [ ] no session replay
- [ ] interrupted session transactional
- [ ] scheduler restore canonical
- [ ] history compacted

## 216T — Determinism/Exploit

- [ ] stable session IDs
- [ ] deterministic load
- [ ] deterministic adaptation
- [ ] medical RNG canonical
- [ ] reload same risk result
- [ ] session replay blocked
- [ ] partial time only
- [ ] cancel/restart no farm
- [ ] low load plateau
- [ ] max-intensity abuse blocked
- [ ] streak cosmetic
- [ ] work loop exploit blocked
- [ ] combat fitness farm blocked
- [ ] expedition walking bounded
- [ ] no schedule overlap
- [ ] no gear duplication
- [ ] migration profile not reinitializable
- [ ] no GUID
- [ ] no wall clock

## 216U — Performance

- [ ] event-driven sessions
- [ ] day-boundary adaptation
- [ ] no per-frame
- [ ] lazy deconditioning
- [ ] O(active sessions)
- [ ] O(survivors) daily bounded
- [ ] 20 survivor benchmark
- [ ] 50 survivor benchmark
- [ ] 100 survivor benchmark
- [ ] routine indexes
- [ ] O(1) capability profile read
- [ ] bounded history
- [ ] daily physical-load coalescing
- [ ] UI on demand
- [ ] state-size budget

## 216V/W — Simulation/CI

- [ ] 30-day no-exercise parity
- [ ] 30-day moderate training
- [ ] 30-day extreme training
- [ ] 120-day structured
- [ ] 120-day physical worker
- [ ] 120-day sedentary specialist
- [ ] 180-day expedition specialist
- [ ] 180-day combat survivor
- [ ] 180-day injury/recovery
- [ ] 400-day young survivor
- [ ] 400-day aging survivor
- [ ] starved survivor
- [ ] exhausted survivor
- [ ] rested survivor
- [ ] high physical trait
- [ ] low physical trait
- [ ] mobility specificity
- [ ] strength specificity
- [ ] mixed specificity
- [ ] combat drill dual output
- [ ] old-save parity
- [ ] auto-maintain
- [ ] many-survivor stability
- [ ] data integrity
- [ ] exercise selftest
- [ ] source-scan authority gates
- [ ] content acceptance
- [ ] dead-routine gate
- [ ] dead-effect gate
- [ ] single-stack capability gate
- [ ] medical-authority gate
- [ ] skill-authority gate
- [ ] old-save parity gate
- [ ] deterministic goldens
- [ ] anti-farm
- [ ] state-size
- [ ] performance
- [ ] generated docs
- [ ] verify-fast

## 216X/Y — Narrative/Follow-On

- [ ] bounded exercise semantic events
- [ ] no every-0.1 improvement event
- [ ] source narrative names treated as content
- [ ] injury event deduplicated with medical
- [ ] decline only significant
- [ ] competition follow-on
- [ ] Plan 171 owns quests
- [ ] session-count grind rejected
- [ ] mandatory streak rejected
- [ ] raw 90-point goals reviewed
- [ ] capability/readiness quests preferred
- [ ] coach only with real role/skill
- [ ] coach uses SkillProgression/Plan195
- [ ] competition system separate
- [ ] training camp separate
- [ ] external service uses trade/service
- [ ] “exercise trading” reframed
- [ ] rehab medical-owned
- [ ] prosthetic adaptation follow-on
- [ ] age-specific programs use age system
- [ ] team sports leisure/social
- [ ] famous achievement archive
- [ ] body composition out of scope

---

# 54. Ship / No-Ship Gate

**SHIP** only if:

```text
trait_authorities == 1
AND aging_authorities == 1
AND needs_fatigue_authorities == 1
AND medical_injury_authorities == 1
AND skill_progression_authorities == 1
AND schedule_duty_authorities == 1
AND final_capability_projection_authorities == 1
AND exercise_duplicate_trait_state == 0
AND exercise_duplicate_age_state == 0
AND exercise_duplicate_needs_state == 0
AND exercise_duplicate_medical_injury_state == 0
AND exercise_duplicate_combat_skill_state == 0
AND exercise_direct_work_efficiency_multiplier == false
AND exercise_direct_combat_damage_or_accuracy_multiplier == false
AND exercise_direct_expedition_success_multiplier == false
AND exercise_direct_disease_resistance_scalar == false
AND exercise_direct_shift_length_override == false
AND last_exercise_day_only_deconditioning == false
AND physical_work_ignored_for_conditioning_maintenance == false
AND auto_exercise_without_scheduled_time == false
AND identical_low_load_infinite_gain == false
AND extreme_training_linear_gain == false
AND rest_day_streak_penalty_affects_conditioning == false
AND medically_ineligible_training_sessions == 0
AND simultaneous_work_sleep_training_sessions == 0
AND old_save_effective_capability_regressions == 0
AND duplicate_session_adaptation_replays == 0
AND unseeded_exercise_rng == 0
AND per_frame_exercise_processing == 0
AND dead_exercise_routines == 0
AND dead_conditioning_effects == 0
AND exercise_old_save_capability_parity == pass
AND exercise_save_roundtrip == pass
AND exercise_progressive_overload == pass
AND exercise_recovery_overtraining == pass
AND exercise_activity_equivalence == pass
AND exercise_deconditioning == pass
AND exercise_medical_authority == pass
AND exercise_skill_authority == pass
AND exercise_consumer_single_stack == pass
AND exercise_determinism == pass
AND exercise_antifarm == pass
AND exercise_30_day_balance == pass
AND exercise_120_day_balance == pass
AND exercise_180_day_balance == pass
AND exercise_400_day_soak == pass
AND exercise_large_roster_performance == pass
AND exercise_selftest == pass
AND data_integrity_selftest == pass
AND content_acceptance_gate == pass
AND ui_access == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 55. Implementer Handoff

1. Audit Traits, aging, Needs, medical injury/affliction state, SkillProgression, Plan 137, Plan 143, Plan 24, DutyRoster, Expedition and combat before writing ExerciseSystem.
2. Define ExerciseSystem as the owner of **conditioning and training adaptation only**.
3. Do not create a second final strength/endurance/current-capability model.
4. Write an ADR defining aerobic, force, mobility and work-capacity conditioning so the dimensions have clear gameplay meaning.
5. Calibrate a neutral conditioning baseline that preserves old-save effective capability. Do not default to 50 merely because the UI uses 0–100.
6. Define routines by training-load vectors rather than direct `+attribute` rewards.
7. Make every session consume actual scheduled time.
8. Treat auto-exercise as a scheduling preference, not free progression.
9. Check hunger, fatigue, hydration, medical eligibility, equipment, facility and partner requirements before session execution.
10. Give partial benefits only for actual completed time/load.
11. Emit exertion to Needs; do not maintain a duplicate fatigue bar.
12. Implement progressive overload, specificity, diminishing returns and plateau behavior.
13. Keep recovery debt separate from acute fatigue and use sleep/nutrition/age/medical state to govern recovery.
14. Make sustained extreme training stall gains and raise canonical medical risk rather than becoming optimal.
15. Replace `daysSinceExercise` deconditioning with rolling physical-load maintenance.
16. Feed physical workload from heavy labor, expeditions and combat where appropriate.
17. Ensure physical work does not train unrelated dimensions.
18. Let age influence adaptation/recovery/potential without allowing exercise to reverse aging.
19. Let traits influence trainability/potential without duplicating trait state.
20. Send exercise injury-risk observations to the medical system and store only medical event references.
21. Reject direct generic disease-resistance bonuses unless DiseaseSystem explicitly supports a conditioning input.
22. Split combat drills into physical training load and SkillProgression practice events.
23. Route work/combat/expedition effects through one canonical capability projection and prove conditioning is applied only once.
24. Keep carrying/encumbrance authority external and bound any force-conditioning contribution.
25. Use Plan 144/Plan 188 for autonomy and recurring training plans.
26. Provide bodyweight/no-equipment routines so the feature works in low-resource shelters.
27. Add weather/radiation restrictions to outdoor training through canonical hazard systems.
28. Build UI around readiness, trend, routine focus and maintain/improve presets—not daily streak pressure.
29. Persist only conditioning, load, recovery and exercise-specific history; do not copy traits, injuries, Needs or skill state.
30. Prevent session replay, cancel/restart farming, low-load spam, combat farming, hauling loops and overlapping schedules.
31. Run 30/120/180/400-day simulations including physical workers, sedentary specialists, expeditions, injuries, starvation, aging, no-exercise parity and extreme-training abuse.
32. Reject every routine whose equipment/facility requirements or capability consumers are unreachable.
33. Close only when survivors can become fitter, plateau, maintain, overtrain, decondition and regain conditioning through believable activity without exercise becoming a second capability engine.

---

# 56. Final Outcome

When this plan is complete, ASHFALL's survivors will no longer have physically static lives.

A survivor who spends months doing radio analysis in a shelter can gradually lose some conditioning if they never exercise.

A mechanic hauling generators and working heavy repairs may maintain substantial force and work capacity even without a formal workout routine.

An expedition specialist who regularly travels long distances can preserve aerobic and work-capacity conditioning through the work they actually do.

A survivor preparing for a difficult expedition can spend several weeks on a structured conditioning plan and arrive better prepared.

But none of those survivors become stronger because a generic `fitness += 1` timer fired.

Training consumes time.

It competes with work, sleep, guard duty, medicine and the rest of shelter life.

It creates fatigue through Needs.
It creates training stress that must recover.
It can expose a survivor to injury risk that the medical system actually resolves.

Physical development therefore becomes a management decision.

The player can choose to spend scarce time preserving a sedentary specialist's conditioning. They can prepare a combat team before a dangerous operation. They can let a manual worker's job provide much of the load they need. They can reduce training after an injury. They can deliberately maintain an older survivor's functional capacity.

Exercise also stops being a simplistic stat ladder.

Running primarily improves sustained aerobic capacity.
Strength work primarily improves trainable force.
Mobility work improves movement quality.
Long-duration work improves repeated-effort tolerance.
Mixed sessions give broad but less specific adaptation.

Combat drills can improve both physical conditioning and combat technique—but through separate systems.

The SkillProgression system still owns combat skill.

The medical system still owns injury.

Traits still describe inherent differences.
Age still changes physiological potential.
Needs still control immediate fatigue and hunger.
Plan 137 and Plan 143 still determine whether someone can actually perform.
Combat and Expedition still own their own outcomes.

Conditioning becomes one missing layer between them: what repeated physical activity has done to the survivor over time.

That layer can improve.

It can plateau.

It can deteriorate.

It can recover.

And because physically demanding work and travel contribute real training load, the player is not forced into a daily gym minigame simply to stop every survivor from collapsing.

The UI can offer a simple “Maintain conditioning” plan for ordinary management, while players who care about optimization can build specialized programs for expedition crews, guards or heavy laborers.

An older survivor may never have the same raw potential as a young athlete, but consistent training can preserve far more function than inactivity.

A naturally strong survivor still benefits from conditioning but does not become automatically skilled in combat.

A hungry, exhausted survivor cannot safely brute-force a maximal workout for free progression.

A well-rested survivor cannot repeat the same easy routine forever and reach 100.

That makes physical development dynamic without turning it into a separate RPG stat sheet.

ASHFALL gains a credible long-term conditioning loop:

activity creates load,
load plus recovery creates adaptation,
insufficient load creates deconditioning,
and conditioning feeds one canonical physical-capability model.

Survivors now change physically because of how they live—not merely because of the traits they spawned with.
