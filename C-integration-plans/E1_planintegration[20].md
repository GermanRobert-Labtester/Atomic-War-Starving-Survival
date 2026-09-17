---
PLAN_ID: E1-20
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 20
STATUS: READY_FOR_EXECUTION_WHEN_SKILL_CERTIFICATION_AND_DUTY_AUTHORITIES_PASS
SOURCE_PLAN: "Plan 195 — Survivor Specialization Roles"
SEQUENCE_FILENAME: "E1_planintegration[20].md"
PREVIOUS_FILENAME: "E1_planintegration[19].md"
NEXT_FILENAMES:
  - "E1_planintegration[21].md"
  - "E1_planintegration[22].md"
CATEGORY: LINK+SURVIVORS+ROLES+SKILLS+DUTY+IDENTITY
PRIMARY_INTENT: "Create formal survivor role appointments and specialist identity by projecting canonical skills, certifications, experience, duty eligibility, and authored responsibilities into typed role capabilities without introducing a second skill tree, duplicate XP ledger, hidden auto-action scheduler, or parallel survivor-stat authority."
EXECUTION_STYLE: "Flagship integration plan"
PREMISE_VERIFICATION_REQUIRED: true
INTAKE_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
SECOND_SKILL_SYSTEM_FORBIDDEN: true
SECOND_CERTIFICATION_SYSTEM_FORBIDDEN: true
SECOND_DUTY_SCHEDULER_FORBIDDEN: true
SECOND_AUTONOMY_ENGINE_FORBIDDEN: true
SECOND_STAT_MODIFIER_FRAMEWORK_FORBIDDEN: true
DUPLICATE_ROLE_XP_LEDGER_FORBIDDEN_BY_DEFAULT: true
RNG_FOR_MANUAL_ROLE_ASSIGNMENT_FORBIDDEN: true
RUNTIME_RISK: HIGH
SAVE_RISK: HIGH
BALANCE_RISK: VERY_HIGH
CHARACTER_IDENTITY_RISK: HIGH
MICROMANAGEMENT_RISK: HIGH
---

# E1 Plan Integration [20] — Survivor Specialization Roles, Appointments, Capability Adapters, Responsibilities, Mastery, and Identity

> **Sequence rule:** this file is `E1_planintegration[20].md`.
> The next files are `E1_planintegration[21].md`, `E1_planintegration[22].md`, and so on.
> The bracketed sequence number remains immediately before `.md`.

## 0. Mission

This plan converts Plan 195 into an implementation-grade survivor-specialization programme.

The source plan identifies a real character-identity gap. `DutyRosterSystem` assigns current work,
`SkillProgressionSystem` tracks competence, and `ApprenticeshipSystem` transfers skill knowledge, but there is
no durable formal appointment that lets the shelter say “this survivor is our medic,” “this survivor is our
chief engineer,” or “this survivor is our scout,” with clear eligibility, recognized responsibilities,
player-facing identity, and bounded specialist advantages.

That goal is valuable. The source implementation, however, risks creating three duplicate systems at once:

1. a second skill/progression ladder through role XP;
2. a second stat-modifier framework through generic `RoleBonus` percentages;
3. a second duty/autonomy scheduler through `auto_heal`, `auto_repair`, `auto_scout`, and similar hidden
   responsibilities.

E1-20 adopts a stricter architecture:

**skills and certifications prove competence; the role system formalizes appointment, specialization policy,
responsibility eligibility, and identity; canonical domain systems execute the actual work and own its
effects.**

A survivor does not become better at medicine because `SurvivorRoleSystem` secretly adds another generic
“medical +20%” layer. The role may unlock a typed medical capability or responsibility policy, while medical
actions continue to use canonical skill/certification/treatment calculations.

Likewise, “Medic auto-tends wounded” means the role contributes a high-priority duty/autonomy candidate when a
valid medical need exists. It does not reserve supplies, interrupt the survivor, consume medicine, and heal
the patient inside a role tick.

## 1. Source Intent Preserved

Plan 195 asks for:

- formal roles;
- skill requirements;
- eight or more role types;
- role-based bonuses;
- role responsibilities;
- five progression levels;
- role identity;
- role caps;
- training/apprenticeship integration;
- UI and role management;
- events and quests;
- old-save compatibility;
- deterministic behavior;
- integration with Duty, Skills, Needs, Combat, and Expedition;
- headless selftest.

E1-20 preserves the player-facing objective while replacing duplicated state with explicit canonical
contracts.

## 2. Core Architecture Thesis

```text
Canonical survivor facts
    |
    +--> SkillProgression
    +--> Certification / qualification
    +--> traits / health / availability
    +--> Apprenticeship / education
    +--> historical completed actions
    |
    v
Role eligibility policy
    |
    v
Formal role appointment
    |
    +--> role identity/title
    +--> specialist capability-policy refs
    +--> responsibility-policy refs
    +--> mastery/readiness tier
    +--> appointment history
    |
    v
Canonical consumers
    |
    +--> DutyRoster / work assignment
    +--> E1-6 Autonomy
    +--> Medicine / Health
    +--> E1-17 Maintenance / Engineering
    +--> Expedition / Scouting
    +--> Research
    +--> Faction / diplomacy / E1-19 trade
    +--> Combat / Security
    +--> Apprenticeship / SkillProgression
```

The role system owns **appointment and specialization identity**. It does not own the underlying skill or
domain outcome.

## 3. Architectural Corrections to the Source Plan

### 3.1 Manual role assignment requires no RNG

If the player selects a qualified survivor for a role and the role cap/policy permits it, the assignment is
deterministic. `ISeededRng` is unnecessary.

### 3.2 Role XP should not duplicate SkillProgression

The source proposes separate `xpEarned` per role. This risks double progression for the same work.

Preferred hierarchy:

```text
skills/certifications + verified role-relevant experience
        ->
role mastery/readiness tier
```

If a role-specific experience ledger is truly needed, it must record verified role-action milestones rather
than create a free-standing XP economy that competes with SkillProgression.

### 3.3 Responsibilities are policy, not auto-actions

A responsibility like “auto-repair” should register a duty/autonomy response policy:

```text
breakdown event
-> role responsibility candidate
-> Duty/Autonomy validates availability, urgency, resource/tool requirements
-> canonical repair job
```

The role system must not directly consume parts or repair equipment.

### 3.4 Bonuses must use typed capability adapters

Generic bonus categories such as `speed_bonus`, `quality_bonus`, and `success_chance` can become uncontrolled
stacking. Each bonus must resolve to a canonical capability/policy with defined stacking semantics.

### 3.5 Leader morale aura is not a free passive effect

A role title should not create a constant nearby morale buff unless a real leadership/social interaction
authority models that effect. Leadership should preferably affect coordination, duty policies, mediation,
negotiation, or authored social actions.

### 3.6 Diplomat does not own trade profit or faction standing gains

E1-19 Market/Faction systems own trade and standing. A diplomat may improve negotiation capability or unlock
terms through canonical diplomacy policy, not directly multiply profit or standing.

### 3.7 Role identity must not erase character identity

“Medic” is an appointment/specialization, not the survivor’s entire identity. Names, traits, history,
relationships, ideology, trauma, and personal goals remain intact.

### 3.8 Role caps should be justified

A hard cap such as “only two medics” is artificial unless it represents formal posts, facility capacity,
organizational structure, or player-selected staffing policy. Skill eligibility can naturally limit
specialists. Caps should be optional, data-defined appointments rather than arbitrary class limits.

## 4. Non-Negotiable Rules

- SkillProgression owns skill levels and skill XP.
- Plan 180/certification authority owns certifications/qualifications.
- ApprenticeshipSystem owns mentor-apprentice skill transfer.
- DutyRoster owns work assignments.
- E1-6 Autonomy owns survivor initiative/refusal proposals where implemented.
- Medicine/Health owns treatment outcomes.
- E1-17 Maintenance/engineering authorities own repair/maintenance outcomes.
- Expedition owns expedition/travel/scouting outcomes.
- Research owns research progress/results.
- Faction/TradeStance/diplomacy owns standing and negotiation outcomes.
- E1-19 Market owns trade price/profit settlement.
- Combat/Security owns combat outcomes.
- Needs/MentalHealth owns morale/stress/fatigue.
- Role system owns formal role appointment, role eligibility policy, role title/identity, specialist
  capability references, responsibility-policy references, and role-specific appointment/mastery history
  only where not already canonical.
- No role-local copy of skill levels.
- No role-local base survivor stats.
- No role-local inventory or resource ledger.
- No role-local duty queue.
- No role-local healing/repair/research/combat resolver.
- No role-local generic success-chance engine.
- Manual assignment is deterministic.
- Auto-assignment, if supported, is a deterministic recommendation/assignment policy unless explicitly using
  an existing autonomous staffing authority.
- Role bonuses must name a canonical consumer.
- Every role responsibility must hand off to a canonical action/job.
- Resource costs are consumed by the canonical action, not by role triggering.
- Role progression cannot award duplicate SkillProgression XP for the same action.
- Role promotion cannot happen twice after save/load.
- Role titles do not create arbitrary universal morale effects.
- Role changes are allowed unless specific institutional policy prevents them.
- Losing eligibility does not silently delete historical mastery.
- A survivor can remain a former medic even if no longer medically fit for duty; current appointment and
  historical expertise are distinct.
- Old saves begin with no formal appointments unless a conservative migration can infer an existing explicit
  profession/certification.
- Do not automatically assign every old survivor a role based on highest skill.
- Role responsibilities are opt-in/controllable by player policy.
- Critical emergency roles must respect survivor availability, health, consent/governance, and resource
  constraints.
- Multiple role holding must be an explicit design decision; do not assume one or unlimited roles.
- First release should prove 4–5 roles before all eight.
- Role UI should reduce, not increase, duty-management friction.

## 5. Acceptance Slices

### Slice A — Appointment identity
Assign a qualified survivor as Medic/Engineer/Scout/Leader with no mechanical duplication.

### Slice B — Typed capability integration
One role unlocks or modifies a real canonical action through a typed adapter.

### Slice C — Responsibility policy
One emergency condition proposes a real canonical duty/action.

### Slice D — Mastery/readiness
Role mastery derives from skills/certification/relevant verified experience.

### Slice E — Broader role family
Scientist, Diplomat, Technician, Enforcer, quests, dialogue, synergies only after the first vertical slices.

Do not start by authoring eight fully automated role engines.


---

## E1-20A — Premise verification and role-authority audit

**Goal:** Verify skill, certification, apprenticeship, duty, autonomy, medicine, repair, research, expedition, diplomacy, combat, identity, save, and UI authorities before creating role state.

### Required substeps

1. Inspect `DutyRosterSystem`, `ApprenticeshipSystem`, `SkillProgressionSystem`, `SurvivorLifecycle`, Plan 180 certification implementation, education/training systems, E1-6 Autonomy, medical/treatment, E1-17 maintenance/repair, ExpeditionSystem, ResearchSystem, Market/Faction/TradeStance, combat/security, dialogue, survivor traits/personality, needs, journal, quest, and save registry.
2. Search for profession, job title, class, certification, occupation, specialist, expertise, leadership, medic, engineer, scientist, diplomat, enforcer, technician, scout, and role-related tags.
3. Determine whether survivors already have background professions/titles.
4. Determine whether capability modifiers use a typed modifier framework.
5. Determine whether duties can be assigned by policy/priority rather than direct mutation.
6. Determine whether completed work/action provenance exists for mastery derivation.
7. Determine whether certifications already gate surgery, engineering, research, or other specialist actions.
8. Determine whether dialogue can query survivor tags/roles.
9. Map every proposed role requirement, bonus, responsibility, and progression field to one owner.
10. Create `docs/systems/SURVIVOR_ROLE_AUTHORITY_MAP.md`.
11. Create intake duplicate-search evidence linking Plans 154, 180, 188, 193, E1-6, E1-15, E1-17, E1-19, and current skill/certification rails.
12. Set `PREMISE_VERIFIED_AT` to current HEAD.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20B — Role ownership ADR

**Goal:** Define the role system as formal appointment/specialization identity and prevent a second skill/duty framework.

### Required substeps

1. Write an ADR comparing `SurvivorRoleSystem`, extension of survivor trait/component store, and a role registry plus appointment service.
2. Define role-owned facts: active appointment, role definition ID, appointment day/provenance, role policy settings, responsibility opt-ins, mastery/readiness tier if approved, and bounded role history.
3. Explicitly exclude skill values, certification state, duty assignments, action progress, health, morale, relationship values, inventory, research progress, combat stats, and faction standing.
4. Define typed capability adapters.
5. Define responsibility handoff protocol.
6. Define role change/removal semantics.
7. Define one-role versus multi-role policy.
8. Require second-tool architecture review.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20C — Role-definition catalog schema

**Goal:** Define roles as data-driven appointments that reference canonical skills, certifications, capabilities, responsibilities, and presentation.

### Required substeps

1. Define role ID, localization keys, category, eligibility policy, required skill/certification refs, capability-policy refs, responsibility-policy refs, mastery-policy ref, appointment-cap policy ref, UI/icon tags, dialogue tags, and feature flags.
2. Do not store free-form generic numeric bonuses unless the target capability has a typed consumer.
3. Do not embed auto-action implementation.
4. Do not embed SkillProgression XP thresholds blindly.
5. Validate all referenced skills/certifications/capabilities/responsibilities.
6. Version catalog semantics.
7. Start with 4–5 roles.
8. Add integrity tests for duplicates/unresolved references/cycles.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20D — Canonical survivor-role assignment record

**Goal:** Persist formal appointment identity with minimal state.

### Required substeps

1. Define survivor ID, role ID, appointment ID, appointed day, appointed-by/provenance, status, mastery/readiness tier if stored, responsibility policy overrides, and historical role refs.
2. Do not copy role name or skill levels.
3. Do not copy active duty.
4. Use stable appointment ID.
5. Define Active, Suspended, Former, Acting/Interim only if each has real behavior.
6. Add serialization/round-trip tests.
7. Support multiple appointments only if policy approves.
8. Keep history bounded.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20E — Role eligibility policy

**Goal:** Evaluate qualification from canonical skills, certifications, health/availability, and authored requirements.

### Required substeps

1. Read SkillProgression levels.
2. Read certification/qualification state.
3. Read required traits/background only if canonical.
4. Use health/availability only for current service eligibility, not historical expertise.
5. Return blocker/reason list.
6. Do not mutate survivor.
7. Do not roll RNG.
8. Add exact-boundary tests.
9. Define whether waived/interim appointment is possible under emergency policy.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20F — Manual assignment transaction

**Goal:** Assign a qualified survivor deterministically and idempotently.

### Required substeps

1. Validate survivor exists.
2. Validate role definition.
3. Validate eligibility.
4. Validate appointment-cap/institutional slot.
5. Validate conflicts with existing appointments.
6. Create stable assignment operation ID.
7. Commit appointment once.
8. Emit role-assigned event after commit.
9. Do not auto-change duty.
10. Add double-click/reload/idempotency tests.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20G — Role removal, resignation, suspension, and reassignment

**Goal:** Separate formal appointment lifecycle from competence/history.

### Required substeps

1. Allow player/system to remove or suspend appointment under policy.
2. Do not delete earned skills/certifications.
3. Preserve former-role history.
4. Handle survivor death/departure.
5. Handle temporary incapacity.
6. Handle changing institutional cap.
7. Release responsibility policies.
8. Do not cancel unrelated active jobs automatically unless Duty handles role-loss constraints.
9. Add lifecycle tests.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20H — One-role versus multi-role decision

**Goal:** Make role multiplicity explicit instead of accidental.

### Required substeps

1. Assess character identity, UI complexity, staffing strategy, and overlap among Medic/Scientist/Leader/Diplomat roles.
2. Recommended default: one primary formal appointment plus optional secondary qualification tags, unless repository design favors multiple appointments.
3. Do not make every skilled survivor hold five simultaneous roles.
4. Allow acting/interim role during shortage if useful.
5. Define conflict rules.
6. Add assignment-conflict tests.
7. Document player-facing semantics.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20I — Appointment-cap policy

**Goal:** Replace arbitrary role caps with organizational slots where they create value.

### Required substeps

1. Define cap policy as unlimited, shelter-post count, facility-dependent posts, governance-defined posts, or scenario limit.
2. Do not cap medics solely to prevent 'all medics' if qualification already provides cost.
3. Facility capacity may create Chief Medic/Lead Engineer posts while ordinary qualified practitioners remain possible.
4. Show cap source.
5. Add tests for unlimited, one-post, expanded-facility, reduced-capacity, and emergency interim roles.
6. Keep cap data-driven.
7. Do not delete appointments silently when cap shrinks.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20J — Skill/certification requirement integration

**Goal:** Use canonical competence as the foundation for specialization.

### Required substeps

1. Map each role to real skill IDs.
2. Map high-risk responsibilities to certifications where required.
3. Do not create fake skill IDs merely to fill role requirements.
4. Prefer multiple requirement bands/policies over hard-coded source examples such as medical 50/first aid 30 until actual scales are verified.
5. Support alternative qualification routes only if design needs them.
6. Add catalog validation.
7. Add tests for qualified/unqualified/certified/expired or revoked certification if supported.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20K — No duplicate role XP architecture

**Goal:** Replace independent role XP with mastery derived from canonical evidence wherever possible.

### Required substeps

1. Audit whether SkillProgression/action history can support mastery.
2. Preferred inputs: relevant skill bands, certifications, number/quality of verified completed specialist actions, apprenticeship/teaching milestones, crisis successes, and tenure.
3. Do not add generic `roleXpEarned` if it merely mirrors action skill XP.
4. If compact role-experience counters are required, store verified milestone counts/points with provenance and no independent generic XP rewards.
5. Define migration/versioning.
6. Add duplicate-action tests.
7. Require ADR amendment before creating a standalone role XP ledger.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20L — Role mastery/readiness tiers

**Goal:** Retain the novice-to-master identity goal without building a second arbitrary progression tree.

### Required substeps

1. Define tier names only after checking game tone; source's novice/apprentice/journeyman/expert/master may conflict with formal apprenticeship terminology.
2. Consider Trainee, Qualified, Experienced, Senior, Master or role-specific labels.
3. Derive tier from mastery policy.
4. Tier unlocks typed responsibilities/capabilities, not generic percentage bundles.
5. Do not demote historical mastery solely because temporary skill debuff occurs.
6. Differentiate current readiness from historical mastery.
7. Add boundary tests.
8. Show source of tier in UI.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20M — Verified role-experience evidence

**Goal:** Record only specialist actions that actually completed under canonical systems.

### Required substeps

1. Define stable evidence ID and source action/job ID.
2. Medicine emits completed treatment/surgery evidence.
3. Maintenance emits completed repair/diagnostic evidence.
4. Expedition emits scouting/navigation evidence.
5. Research emits completed analysis/project contribution.
6. Diplomacy emits resolved negotiation/mediation evidence.
7. Combat/Security emits verified defensive/training evidence.
8. Do not count canceled actions.
9. Deduplicate by source operation ID.
10. Bound history/aggregate counters.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20N — Typed capability adapter contract

**Goal:** Apply role specialization only through canonical capability hooks with explicit stacking rules.

### Required substeps

1. Define capability IDs such as medical_triage_priority, advanced_repair_planning, expedition_recon_bonus_policy, research_specialist_policy, negotiation_specialist_policy, security_response_policy.
2. Each capability identifies owning system.
3. Define whether role unlocks eligibility, reduces time, improves quality, increases information, or changes priority.
4. Avoid global flat multipliers where possible.
5. Define stacking with skill/certification/tool/facility bonuses.
6. Add owner-boundary tests.
7. Reject unknown capability refs at data-integrity check.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20O — Modifier stacking policy

**Goal:** Prevent roles from turning high-skill survivors into runaway multiplicative power stacks.

### Required substeps

1. Audit existing modifier framework.
2. Define additive/multiplicative order once in canonical consumer.
3. Cap specialist contribution.
4. Do not apply role multiplier and then another identical certification/skill multiplier to the same concept without review.
5. Prefer unlock/quality/priority effects over pure speed bonuses.
6. Create combination matrix for skill + certification + role + tool + facility.
7. Add max-stack tests.
8. Measure high-tier role power.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20P — Role responsibility contract

**Goal:** Represent responsibilities as trigger-to-candidate policies, not self-executing effects.

### Required substeps

1. Define responsibility ID, source event/query, role eligibility, minimum mastery, urgency, priority, opt-in policy, canonical action/job factory ID, resource/tool prerequisite query, and suppression/cooldown rules.
2. Do not define `actionTaken` as executable role-local code.
3. Do not consume resources on trigger.
4. Do not directly mutate patient/equipment/research/security state.
5. Return duty/autonomy candidate with reason trace.
6. Add schema/integration tests.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20Q — DutyRoster integration

**Goal:** Let formal roles improve staffing policy without replacing duty assignments.

### Required substeps

1. Expose role tags/capabilities to DutyRoster.
2. Allow duty eligibility/preference to use role identity.
3. Responsibility event may propose a high-priority duty.
4. DutyRoster remains final assignment owner.
5. Do not force role holder off an existing critical task without scheduler policy.
6. Allow player lock/manual override.
7. Add tests for preferred assignment, unavailable specialist, competing emergencies, and roleless fallback.
8. Keep current no-role behavior valid.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20R — Autonomy integration

**Goal:** Let role identity inform survivor initiative through E1-6 instead of creating role-local autonomous actions.

### Required substeps

1. Role responsibilities become candidate intents.
2. Autonomy evaluates survivor willingness/context where enabled.
3. Medic may propose tending a patient.
4. Engineer may propose inspecting/repairing equipment.
5. Leader may propose mediation/coordination.
6. Do not create a second intent scoring loop.
7. Player override/governance remains external.
8. Add tests with autonomy enabled/disabled.
9. Use reason traces.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20S — Medic role vertical slice

**Goal:** Implement one formal medical specialization end-to-end through canonical medicine and duty systems.

### Required substeps

1. Verify medical skills/certifications.
2. Assign Medic role.
3. Expose medical triage/treatment responsibility candidate.
4. Canonical medical system validates patient, supplies, facility, treatment, and outcome.
5. Role may unlock or modestly improve a typed medical capability only if not already represented by certification.
6. Do not directly add +20% healing globally.
7. Do not consume supplies in role code.
8. Record completed specialist evidence.
9. Add save/load tests.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20T — Engineer role vertical slice

**Goal:** Integrate Engineer with E1-17 maintenance and E1-9 construction without duplicating repair/crafting.

### Required substeps

1. Verify repair/construction skills and certifications.
2. Role may prioritize diagnostics, maintenance planning, complex repair eligibility, or bounded workmanship policy.
3. E1-17 owns repair jobs/outcome.
4. E1-9 owns construction/upgrade.
5. SkillProgression owns skill growth.
6. Do not directly restore condition.
7. Do not create repair materials.
8. Record completed engineering evidence.
9. Add breakdown-response tests.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20U — Scout role vertical slice

**Goal:** Integrate Scout with Expedition/world knowledge without duplicating travel or threat systems.

### Required substeps

1. Verify navigation/stealth/perception/scouting skills.
2. Role may unlock recon task, improve information quality, route-risk estimate, detection policy, or expedition preparation.
3. Expedition owns travel speed and encounters.
4. Do not grant a flat +25% expedition speed without validating travel semantics.
5. E1-3 may consume scouting intel.
6. Record completed recon evidence.
7. Add expedition start/return tests.
8. Keep source world truth hidden appropriately.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20V — Leader role vertical slice

**Goal:** Define leadership through coordination/decision responsibilities rather than a passive morale aura.

### Required substeps

1. Audit governance, duty priority, mediation, social, faction, autonomy, and morale systems.
2. Possible role capabilities: emergency coordination policy, meeting/mediation eligibility, duty reassignment recommendation, negotiation support, leadership-specific dialogue.
3. Needs/MentalHealth owns morale consequences.
4. Relations owns relationship consequences.
5. Do not emit continuous nearby morale bonus.
6. Define formal leader post versus generic leadership skill.
7. Add tests for crisis coordination and absent leader.
8. Keep authoritarian/control mechanics elsewhere.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20W — Technician role boundary

**Goal:** Differentiate Technician from Engineer so both roles create distinct decisions.

### Required substeps

1. Audit electrical/electronic/radio/power/terminal/technical-operation systems.
2. Technician should focus on operation, diagnostics, electronics, power controls, radio, sensors, or complex devices.
3. Engineer focuses on mechanical/structural/maintenance/construction where possible.
4. Do not duplicate capabilities between roles without reason.
5. Use certifications if dangerous equipment requires them.
6. Add overlap matrix.
7. Feature-gate Technician if no distinct consumer exists.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20X — Scientist role boundary

**Goal:** Integrate Scientist with ResearchSystem without creating a second research multiplier.

### Required substeps

1. Audit research contribution/analysis systems.
2. Role may unlock specialist project eligibility, experiment supervision, sample analysis, or bounded research contribution policy.
3. ResearchSystem owns research progress and results.
4. Do not add independent role research points.
5. Do not apply generic +25% research after existing skill/facility modifiers without stacking review.
6. Record completed project/analysis evidence.
7. Add tests.
8. Feature-gate if researcher identity already exists elsewhere.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20Y — Diplomat role boundary

**Goal:** Integrate Diplomat with faction negotiation, E1-19 trade agreements, and mediation without directly modifying profit/standing.

### Required substeps

1. Audit faction negotiation/TradeStance/standing.
2. Role may unlock negotiation options, improve concession policy, provide better information, or satisfy representative requirements.
3. E1-19 Market owns trade settlement/profit.
4. Faction authority owns standing changes.
5. Do not apply +20% trade profit.
6. Do not apply +15% standing gain globally.
7. Record resolved negotiation evidence.
8. Add trade-route agreement tests.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20Z — Enforcer role boundary

**Goal:** Integrate Enforcer with security/combat/training without becoming a generic combat class.

### Required substeps

1. Audit combat, shelter defense, guard duty, discipline/governance, and training systems.
2. Role may unlock security response priority, guard-training responsibility, defensive coordination, or specialized equipment certification.
3. Combat owns actual effectiveness/damage.
4. Do not add universal +20% combat multiplier without a canonical typed policy.
5. Governance owns punishment/order decisions.
6. Do not make Enforcer automatically intimidate survivors.
7. Record completed security evidence.
8. Add raid/emergency tests.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20AA — Medical responsibility policy

**Goal:** Implement `tend wounded` as a canonical response candidate.

### Required substeps

1. Trigger from canonical medical-emergency/injury event.
2. Find active eligible Medic(s).
3. Check Duty/Autonomy availability.
4. Canonical medical job validates treatment/supplies/facility.
5. Role policy sets priority/preference only.
6. Patient treatment outcome remains medical authority.
7. Prevent duplicate response to same patient/event.
8. Add multi-medic and no-medic tests.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20AB — Engineering responsibility policy

**Goal:** Implement breakdown response as a maintenance-job candidate.

### Required substeps

1. Trigger from E1-17 significant component failure/service alert.
2. Find Engineer/Technician candidates by capability.
3. Respect existing maintenance queue.
4. Do not consume parts on trigger.
5. Do not override player-locked priorities silently.
6. Use stable failure/proposal ID.
7. Add multiple-failure and unavailable-engineer tests.
8. Keep E1-17 action authoritative.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20AC — Scout responsibility policy

**Goal:** Implement expedition recon preparation as an optional pre-departure responsibility.

### Required substeps

1. Trigger from expedition planning or threat-intelligence gap.
2. Scout may propose route reconnaissance, map review, threat brief, or scouting assignment.
3. Expedition owns departure/travel.
4. E1-3 owns resulting information.
5. Do not auto-send Scout away without assignment policy.
6. Add tests with/without scout.
7. Prevent repeated prep farming.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20AD — Leader responsibility policy

**Goal:** Implement leadership response through real meetings/coordination/mediation.

### Required substeps

1. Trigger only on meaningful shelter-level need such as unresolved duty crisis, morale incident, conflict, or governance event.
2. Leader proposes canonical meeting/mediation/coordination action.
3. Do not direct-set morale.
4. Do not direct-set relationships.
5. Do not automatically override player orders.
6. Add cooldown and significance threshold.
7. Add tests for low morale versus actual actionable leadership event.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20AE — Scientist/Technician responsibility policies

**Goal:** Use queued canonical research/technical events without auto-generating work.

### Required substeps

1. Scientist candidate responds to eligible research analysis/project supervision.
2. Technician candidate responds to complex system diagnostics/operation.
3. Canonical Research/Maintenance/Power/Radio systems validate work.
4. Do not automatically start work when resources/priority disallow it.
5. Use stable proposal IDs.
6. Add disabled/no-resource tests.
7. Keep responsibilities opt-in.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20AF — Diplomat responsibility policy

**Goal:** Use negotiation opportunities from canonical faction/trade systems.

### Required substeps

1. Trigger from explicit negotiation/renewal/mediation opportunity.
2. Diplomat can be recommended/assigned as representative.
3. Do not negotiate automatically without player/autonomy policy.
4. Faction/Market resolves terms.
5. Do not spend currency/goods in role code.
6. Add E1-19 agreement-renewal tests.
7. Keep political consequences external.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20AG — Enforcer responsibility policy

**Goal:** Use security emergencies through Duty/Defense rather than a hidden auto-combat path.

### Required substeps

1. Trigger from canonical security alert/attack.
2. Role contributes guard-response candidate.
3. Defense/Combat decides deployment/outcome.
4. Do not teleport survivor into combat.
5. Respect injuries/equipment/current duty.
6. Use stable threat event ID.
7. Add tests for attack, unavailable enforcer, multiple guards, and no-role fallback.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20AH — Responsibility resource-consumption boundary

**Goal:** Ensure role triggers never consume supplies twice.

### Required substeps

1. Medical supplies consumed by treatment action.
2. Repair parts consumed by maintenance action.
3. Research materials consumed by research action.
4. Trade concessions consumed by negotiation/economy action.
5. Ammunition/equipment consumed by combat authority.
6. Role responsibility stores no resource balance.
7. Add conservation tests for each first-release responsibility.
8. Add cancel/retry tests.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20AI — Responsibility priority and conflict resolution

**Goal:** Prevent specialists from being yanked between simultaneous emergencies by independent role triggers.

### Required substeps

1. Use Duty/Autonomy priority system.
2. Define emergency/urgent/routine responsibility classes.
3. Respect current critical work.
4. Allow manual lock.
5. Do not queue duplicate proposals for same event.
6. Resolve one survivor qualifying for multiple roles.
7. Expose blocked reason.
8. Add simultaneous medical+repair+raid scenario tests.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20AJ — Role readiness versus mastery

**Goal:** Separate long-term expertise from current capacity to serve.

### Required substeps

1. Mastery is durable role experience/qualification.
2. Readiness reflects current health, fatigue, stress, equipment, certification validity, availability, and context through canonical queries.
3. A Master Medic can be unavailable due to illness without losing mastery.
4. A former Engineer can retain mastery but not hold active appointment.
5. Do not store duplicate fatigue/health in role state.
6. Add readiness tests.
7. Show both concepts clearly.

### Role-system invariants

- SkillProgression and certifications remain the sole competence authorities.
- Role state owns appointment/specialization identity, not copied skill or stat state.
- Responsibilities propose canonical jobs/actions instead of executing domain effects.
- Capabilities are typed and owned by their consuming systems.
- Manual role assignment and mastery are deterministic by default.
- Role progression does not duplicate skill XP.
- Resources are consumed only by canonical actions.
- Role identity remains one facet of a larger survivor character model.

### Negative tests

- Role state stores copied skill levels or certification DTOs.
- Role responsibility directly heals, repairs, researches, trades, or fights.
- Role action consumes resources once in role code and again in domain action.
- Reload grants mastery twice from the same completed action.
- Manual role assignment calls RNG.
- Leader role emits a free passive morale aura every tick.
- Diplomat multiplies trade profit or standing directly.
- All eight roles are shipped despite overlapping/no canonical consumers.

### Acceptance evidence

- [ ] Authority-boundary tests pass.
- [ ] Eligibility/assignment/idempotency tests pass.
- [ ] Capability stacking tests pass.
- [ ] Responsibility handoff/resource-conservation tests pass.
- [ ] Mastery evidence cannot duplicate.
- [ ] Old-save migration is prospective.
- [ ] Power-budget/micromanagement/performance evidence is captured.

---

## E1-20AK — Training-to-role pipeline

**Goal:** Use education/apprenticeship/skills to make survivors eligible rather than adding role-training XP.

### Requi

<!-- Deliverable capped to remain within the requested 50–90k character envelope. -->
