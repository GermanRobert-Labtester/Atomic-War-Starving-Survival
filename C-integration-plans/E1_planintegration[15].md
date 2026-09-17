---
PLAN_ID: E1-15
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 15
STATUS: READY_FOR_EXECUTION_WHEN_PSYCHOLOGY_RAILS_PASS
SOURCE_PLAN: "Plan 179 — Unified Psychology & Phobia System"
SEQUENCE_FILENAME: "E1_planintegration[15].md"
PREVIOUS_FILENAME: "E1_planintegration[14].md"
NEXT_FILENAMES:
  - "E1_planintegration[16].md"
  - "E1_planintegration[17].md"
CATEGORY: LINK+PSYCHOLOGY+TRAUMA+PHOBIAS+THERAPY+PRESENTATION
PRIMARY_INTENT: "Create one longitudinal psychological integration layer that unifies evidence from six existing trauma systems into survivor-facing profiles, phobia/coping/recovery arcs, and authority-safe behavioral consequences without duplicating crisis, needs, relationships, autonomy, skills, dreams, aging, or survivor trait state."
EXECUTION_STYLE: "Flagship integration plan"
PREMISE_VERIFICATION_REQUIRED: true
INTAKE_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
SEVENTH_TRAUMA_ENGINE_FORBIDDEN: true
SECOND_MORALE_SYSTEM_FORBIDDEN: true
SECOND_RELATIONSHIP_SYSTEM_FORBIDDEN: true
SECOND_REFUSAL_ENGINE_FORBIDDEN: true
SECOND_PERSONALITY_STORE_FORBIDDEN: true
RUNTIME_RISK: HIGH
SAVE_RISK: HIGH
BALANCE_RISK: HIGH
NARRATIVE_RISK: HIGH
PLAYER_FRICTION_RISK: HIGH
---

# E1 Plan Integration [15] — Unified Psychology, Trauma Integration, Phobias, Coping, Therapy, Personality Development, and Long-Term Recovery

> **Sequence rule:** this file is `E1_planintegration[15].md`.
> The next files are `E1_planintegration[16].md`, `E1_planintegration[17].md`, and so on.
> The bracketed sequence number remains immediately before `.md`.

## 0. Mission

This plan converts Plan 179 into an implementation-grade psychology programme.

The source plan correctly identifies a fragmentation problem: six functional trauma-related systems already
exist, but they appear to operate as parallel specialists. Combat hypervigilance, somatic flashbacks,
guilt-driven insomnia, acute mental-health crises, location-linked psychological contamination, and phantom
memory triggers can all affect the same survivor without one coherent player-facing picture of what that
survivor is carrying, what triggers them, what helps, how recovery is progressing, or why certain behavior
changes over time.

The wrong solution is to create a large `PsychologicalProfileSystem` that becomes a seventh trauma engine and
starts owning stress, morale, relationships, task refusal, work penalties, sleep disruption, crisis severity,
personality, survivor goals, therapy skill, and dream logic itself.

E1-15 instead establishes a stricter architecture:

**existing trauma systems emit canonical psychological evidence; the psychology layer integrates that evidence
into longitudinal profile facts and treatment/recovery policy; canonical domain systems continue to resolve
the actual consequences.**

A phobia is therefore not “-20 work efficiency while triggered.” It is a persistent psychological condition
with trigger semantics. When a relevant context occurs, the psychology layer emits/returns a structured
constraint or response that Duty, Autonomy, Needs, Relations, MentalHealth, or another owner can consume.

Likewise, resilience is not a universal magic stat that silently modifies every mental-health system. It must
either be derived from canonical evidence or become a bounded psychology-owned construct with explicit,
audited consumers.

## 1. Source Intent Preserved

Plan 179 asks for:

- one psychological profile per survivor;
- integration with six existing trauma systems;
- phobia development;
- coping mechanisms;
- personality evolution;
- resilience/vulnerabilities;
- therapy/recovery arcs;
- deterministic psychology;
- events and quest hooks;
- UI and journal;
- old-save compatibility;
- integration with Needs, Relations, Skills, Dreams, and Aging;
- data-driven phobias/coping definitions;
- headless selftest.

E1-15 preserves all of those player-facing goals but applies authority boundaries and a staged rollout.

## 2. Core Architecture Thesis

```text
Existing canonical systems
    |
    +--> CombatTraumaSystem
    +--> SomaticFlashbackSystem
    +--> GuiltInsomniaSystem
    +--> MentalHealthCrisisSystem
    +--> PsychologicalContaminationSystem
    +--> PhantomMemoryEngine
    +--> Needs / Relations / Memory / Aging / Autonomy
    |
    v
Psychological evidence adapter layer
    |
    +--> trauma observations
    +--> trigger exposures
    +--> recovery evidence
    +--> social/therapy evidence
    |
    v
Longitudinal PsychologicalProfile
    |
    +--> integrated trauma references
    +--> phobia identities/severity
    +--> coping strategy identities
    +--> resilience/recovery summary
    +--> personality-evolution descriptors
    +--> known/unknown/diagnosed state
    |
    v
Canonical consumers
    |
    +--> Duty/Autonomy refusal & preference policy
    +--> MentalHealth / Needs
    +--> Relations
    +--> SkillProgression
    +--> DreamSystem
    +--> Aging/Lifecycle
    +--> Quest/Event/Journal
    +--> UI
```

The profile is a longitudinal integration authority. It is not the source of truth for the six trauma systems
it summarizes.

## 3. Non-Negotiable Rules

- The six existing trauma systems remain canonical for their own specialized mechanics.
- `PsychologicalProfileSystem` does not reimplement their trigger/effect loops.
- `MentalHealthCrisisSystem` remains crisis/acuity authority.
- `NeedsSystem` remains owner of morale/stress/fatigue/sleep-related physiological state where applicable.
- `SurvivorRelationsSystem` remains owner of affinity/trust/resentment/grief.
- E1-6 Autonomy remains owner of survivor intent selection/refusal orchestration if implemented.
- Duty/roster authority owns assignment and final refusal/availability validation.
- `SkillProgressionSystem` owns psychology/therapy skill XP.
- `DreamSystem` owns dreams; psychology provides context only.
- `AgingSystem`/lifecycle owns age; psychology may consume age-context, not duplicate it.
- Survivor trait/personality authority remains canonical for base personality traits if one exists.
- Psychology may own **longitudinal adaptations/personality shifts** only when these are not already represented elsewhere.
- Phobia trigger state may constrain behavior but does not directly edit work speed, morale, or assignment.
- Therapy is a scheduled canonical activity/job, not a daily arbitrary severity subtraction.
- Therapy cannot guarantee complete recovery.
- Exposure to a phobic trigger does not automatically worsen phobia; repeated forced exposure can worsen,
  neutral exposure can do nothing, and structured therapeutic exposure may improve symptoms only under an
  explicit treatment model.
- “Substance use” is not automatically a coping benefit; if modeled it must integrate with actual substance,
  dependence, health, and inventory rails or remain deferred.
- “Denial” is not a generic coping buff; it may affect awareness/help-seeking if such systems exist.
- Social betrayal does not mechanically equal clinical social anxiety in one event.
- A single traumatic event may create vulnerability without guaranteeing a phobia.
- Phobia names and descriptions should be clinically respectful and avoid casual diagnostic overreach where
  the game uses simplified categories.
- Personality changes should be gradual, bounded, explainable, and not overwrite core identity every few days.
- Old saves do not receive fabricated trauma histories/phobias.
- Deterministic RNG is keyed and cannot be save-scummed.
- Psychological logs/history are bounded.
- The profile UI must summarize, not expose a spreadsheet of every hidden internal score.
- First release should implement 3–4 high-value phobias and 3–4 coping strategies before all 7/6 content.

## 4. Acceptance Slices

### Slice A — Integration profile
Read-only aggregation of the six trauma systems into one survivor psychological summary.

### Slice B — Phobia identity and trigger policy
Persistent phobia condition with canonical trigger/context integration.

### Slice C — Therapy and coping
Real scheduled treatment/support activities through existing systems.

### Slice D — Long-term personality/recovery
Bounded longitudinal changes with explicit provenance.

### Slice E — Dreams, aging, quests, broad content
Only after the base profile and treatment loop are accepted.

Do not build every proposed mechanic simultaneously.


---

## E1-15A — Premise verification and psychological-authority audit

**Goal:** Verify the six trauma systems, survivor personality/trait state, Needs, Relations, Autonomy, Duty, Skills, Dreams, Aging, therapy/medical rails, journal, and save architecture before adding profile state.

### Required substeps

1. Inspect `CombatTraumaSystem`, `SomaticFlashbackSystem`, `GuiltInsomniaSystem`, `MentalHealthCrisisSystem`, `PsychologicalContaminationSystem`, `PhantomMemoryEngine`, survivor aggregate/components, Needs, Relations, Autonomy, duty roster, memory, skills, medical/therapy actions, DreamSystem/Plan 177, AgingSystem/Plan 176, journal, quest, and save registry.
2. Document exactly what each trauma system owns, persists, emits, and modifies today.
3. Identify duplicated psychological concepts already present across the six systems.
4. Verify whether survivors already have personality traits, temperament, resilience, stress tolerance, coping tags, or therapy states.
5. Verify whether there is any generic affliction/diagnosis framework suitable for phobias.
6. Verify whether work/duty validation can accept structured psychological constraints.
7. Verify whether survivor autonomy can express avoidance/help-seeking.
8. Search for `PhobiaSystem`, `PsychologicalProfile`, `PersonalityChange`, `TraumaProfile`, counseling, therapy, fear, avoidance, panic, resilience, coping, and related terms.
9. Create `docs/systems/PSYCHOLOGY_AUTHORITY_MAP.md`.
10. Create intake duplicate-search evidence linking Plans 147, 148, 177, 176, 144/E1-6, and medical systems.
11. Set `PREMISE_VERIFIED_AT` to current HEAD.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15B — Psychology integration ADR

**Goal:** Define the psychological profile as an integration authority and prevent duplication of six trauma domains.

### Required substeps

1. Write an ADR comparing a new `PsychologicalProfileSystem`, extending survivor component store, a read-model-only aggregator, and a hybrid profile + phobia/recovery policy service.
2. Define psychology-owned state narrowly: phobia identities/severity if no canonical affliction owner exists, coping identities, therapy/recovery plan state if not owned by medical jobs, longitudinal adaptation/personality-shift records, evidence references, and bounded profile history.
3. Explicitly exclude combat trauma state, somatic flashback state, guilt insomnia state, crisis acuity, psychological contamination state, phantom-memory state, needs/morale, relationship values, duty assignments, skill XP, dream state, and age.
4. Define adapters from all six trauma systems.
5. Define profile read model separate from persistent state.
6. Define feature flags for phobias, coping, therapy, personality evolution, dreams, and aging.
7. Require second-tool architecture review before code.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15C — Psychological evidence contract

**Goal:** Normalize observations from heterogeneous trauma systems without flattening them into one universal trauma score.

### Required substeps

1. Define `PsychologicalEvidence` with stable evidence ID, survivor ID, source system, source event/record ID, category, context tags, severity band, onset day, resolved/active state if source supplies it, and optional target/location/object references.
2. Do not copy whole source-system DTOs.
3. Do not convert every system into a single `traumaSeverity` number.
4. Define source-specific categories such as combat_hypervigilance, somatic_flashback, guilt_insomnia, crisis_episode, contamination_trigger, phantom_memory.
5. Define evidence provenance and idempotency.
6. Define whether evidence is persisted as references or reconstructed from source history.
7. Bound retained evidence.
8. Add validation for missing survivor/source/event.
9. Add fixtures for each of the six systems.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15D — Six-system adapter layer

**Goal:** Consume existing trauma signals without changing their core mechanics.

### Required substeps

1. Create one thin adapter per source system or one registry-based adapter if project architecture prefers it.
2. CombatTrauma adapter reports verified hypervigilance/false-alarm/trauma milestones.
3. SomaticFlashback adapter reports flashback episodes/trigger classes.
4. GuiltInsomnia adapter reports guilt/sleep-disruption evidence.
5. MentalHealthCrisis adapter reports crisis episode/acuity transitions.
6. PsychologicalContamination adapter reports location-linked psychological contamination evidence.
7. PhantomMemory adapter reports background/item-trigger memory evidence.
8. Adapters must be read-only toward source state.
9. Use stable event IDs to prevent duplicate evidence.
10. Add contract tests for each adapter.
11. Ensure source systems behave identically when profile integration is disabled.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15E — Profile DTO minimalism

**Goal:** Design a profile that summarizes longitudinal psychology without copying every source state.

### Required substeps

1. Define survivor ID, phobia refs, coping refs, longitudinal vulnerability tags, recovery-plan reference/state where owned, resilience representation if accepted, personality-evolution records, diagnosis/knowledge state if appropriate, and bounded evidence/history references.
2. Do not duplicate active crisis acuity.
3. Do not duplicate current morale/stress.
4. Do not duplicate current relationship values.
5. Do not copy every trauma system's current state.
6. Define deterministic stable ordering.
7. Define schema version.
8. Define caps for history/evidence.
9. Add serialization and integrity tests.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15F — Resilience model decision

**Goal:** Decide whether `resilience` should be a persisted psychology-owned score, a derived read model, or omitted.

### Required substeps

1. Audit existing resilience/stress-tolerance/personality traits.
2. Prefer deriving resilience from stable canonical facts if possible.
3. If persisted, define explicit meaning, range, sources of change, consumers, and caps.
4. Do not let resilience silently scale every trauma system.
5. Require each consumer to opt in.
6. Separate resilience from current morale.
7. Separate resilience from personality.
8. Add boundary and migration tests.
9. Document why the concept adds player value.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15G — Vulnerability model

**Goal:** Represent trigger sensitivities as typed longitudinal facts rather than duplicated event rules.

### Required substeps

1. Define vulnerability ID/category, source evidence, trigger tags, sensitivity/severity, visibility/diagnosis state, onset, and resolution/recovery behavior.
2. Allow a phobia to reference a vulnerability but do not require every vulnerability to become a diagnosis.
3. Keep source-system trigger logic canonical where it already exists.
4. Use vulnerability tags to make cross-system triggers explainable.
5. Add tests for combat/noise/location/social/darkness contexts.
6. Bound vulnerability count.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15H — Phobia taxonomy and content gate

**Goal:** Use a small clinically respectful set of game abstractions with explicit trigger semantics.

### Required substeps

1. Audit the proposed seven phobias against actual game contexts and canonical trigger data.
2. Do not ship acrophobia if there are no meaningful height contexts.
3. Do not ship thalassophobia if deep-water/maritime contexts cannot identify relevant exposure.
4. Do not ship social phobia solely because betrayal exists; require a broader social-anxiety trigger/avoidance model.
5. Do not ship blood phobia if combat/injury presentation has no canonical blood/injury context tag.
6. Define radiation fear as fear of contamination/radiation contexts, not actual radiation sensitivity.
7. Start with 3–4 phobias that have real trigger consumers.
8. Add content review for terminology.
9. Expand only after behavior is observable.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15I — Phobia definition schema

**Goal:** Create data-driven phobia identities with typed triggers, development policy, progression/recovery policy, and canonical consumers.

### Required substeps

1. Define phobia ID, localization keys, trigger tags/context query, development-policy ID, initial-severity range/policy, manifestation/knowledge state, avoidance-policy ID, distress-policy ID, therapy-policy ID, compatibility/exclusion, and presentation tags.
2. Do not put arbitrary work/morale deltas directly in JSON unless consumed by canonical policies.
3. Define severity as psychology-owned only if it has consistent semantics.
4. Validate trigger tags.
5. Validate policy IDs.
6. Validate all UI/localization keys.
7. Version catalog semantics.
8. Add integrity tests.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15J — Phobia-development decision model

**Goal:** Develop phobias from longitudinal evidence through deterministic, bounded policy rather than one event = one diagnosis.

### Required substeps

1. Build eligible phobia candidates from evidence patterns.
2. Use trauma severity, repetition, context specificity, survivor predisposition, recovery state, and accepted resilience inputs.
3. Allow no-result outcome.
4. Use deterministic keyed RNG if probability is retained.
5. Persist decision outcome to prevent rerolls.
6. Define minimum evidence requirements by phobia.
7. Do not infer diagnosis from one arbitrary betrayal/accident unless content policy explicitly allows it.
8. Apply compatibility/cap rules.
9. Add distribution tests.
10. Add save-scum/no-reroll tests.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15K — Phobia trigger detection

**Goal:** Detect relevant contexts through canonical environment/duty/event tags rather than bespoke per-phobia polling.

### Required substeps

1. Define common trigger-context interface.
2. Examples: enclosed_space, darkness, deep_water, height, visible_injury, radiation_zone, crowded_social_context.
3. Use authoritative location/task/event tags.
4. Do not infer trigger from audio alone.
5. Evaluate only when survivor enters/receives relevant context.
6. Use hysteresis/cooldown to prevent rapid retriggering.
7. Record trigger event/provenance.
8. Add tests for entry, exit, repeated context, false-positive context, save/load.
9. Avoid per-frame scanning of all phobias.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15L — Triggered-phobia response contract

**Goal:** Return structured psychological constraints/responses rather than directly applying generic penalties.

### Required substeps

1. Define response classes such as distress, hesitation, avoidance_preference, soft_refusal_candidate, hard_incapacity only where clinically/gameplay justified, panic/crisis escalation request, and support-seeking.
2. Pass distress to MentalHealth/Needs.
3. Pass avoidance/refusal to Autonomy/Duty validation.
4. Pass social response to Relations/Autonomy.
5. Do not directly reduce work efficiency in psychology code.
6. Define severity and context thresholds.
7. Add tests for mild, moderate, severe, supported survivor, emergency duty, and recovery.
8. Use player-facing reason traces.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15M — Exposure and phobia-severity progression

**Goal:** Replace the source plan's blanket 'repeated exposure increases severity' rule with context-aware progression.

### Required substeps

1. Differentiate uncontrolled traumatic exposure, ordinary safe exposure, avoidance, supportive exposure, and structured therapeutic exposure.
2. Uncontrolled re-traumatization may worsen severity.
3. Ordinary exposure may be neutral.
4. Therapeutic exposure may reduce fear only under a real treatment plan.
5. Avoidance may preserve or worsen long-term vulnerability depending on model, but do not simulate clinical treatment casually.
6. Define bounded severity updates.
7. Use stable provenance.
8. Add tests for each exposure class.
9. Do not update severity every tick inside a trigger zone.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15N — Coping mechanism taxonomy

**Goal:** Separate healthy coping, neutral coping, maladaptive coping, and dependency-prone behavior.

### Required substeps

1. Audit available activities: meditation/quiet rest, exercise, social interaction, creative work, substances, journaling, work, rituals, hobbies.
2. Only include mechanisms backed by real activity/resource systems.
3. Classify mechanism type.
4. Do not assume every coping strategy is effective for every survivor.
5. Do not treat substance use as a free stress-reduction button.
6. Do not use denial as a generic positive effect.
7. Start with 3–4 supported coping mechanisms.
8. Define content tone.
9. Expand to six after integration.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15O — Coping mechanism schema

**Goal:** Represent learned coping identities, preferences, effectiveness context, and side-effect policies without duplicating activity state.

### Required substeps

1. Define coping ID, localization keys, activity/capability reference, eligibility, learning source, context effectiveness policy, contraindication/side-effect policy, and visibility.
2. Keep current activity owned by job/autonomy/social systems.
3. Keep substance inventory/dependence owned elsewhere.
4. Do not persist generic `effectiveness: 0-100` unless meaning and update policy are explicit.
5. Allow survivor-specific mastery/preference only if needed.
6. Add schema validation.
7. Add save/load tests.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15P — Coping acquisition and learning

**Goal:** Let survivors acquire coping strategies through therapy, experience, peers, or existing habits.

### Required substeps

1. Define deterministic acquisition conditions.
2. Use therapy-session outcomes, successful activity history, peer mentoring, or background traits.
3. Do not grant a coping mechanism randomly every day.
4. Use stable acquisition IDs.
5. Route peer learning through social/skill authority.
6. Add tests for therapy learning, experience learning, duplicate learning, incompatible coping, and save/load.
7. Keep acquisition bounded.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15Q — Coping action integration

**Goal:** Use canonical activities to execute coping rather than ticking a profile effect.

### Required substeps

1. Meditation/quiet time uses activity/free-time scheduling.
2. Exercise uses physical activity/Needs.
3. Socializing uses social/autonomy/relations.
4. Creative work uses activity/inventory if materials are required.
5. Substance use requires actual item/consumption/dependence rails or remains disabled.
6. Each action emits a coping-outcome event.
7. MentalHealth/Needs resolve relief or side effects.
8. Add tests for valid action, unavailable context, interruption, no resource, and save/load.
9. Do not directly set stress in PsychologySystem.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15R — Therapy ownership ADR

**Goal:** Define therapy as a scheduled treatment/social/skill activity, not a profile-only severity decrement.

### Required substeps

1. Audit medical/treatment, job scheduler, social interaction, skills, and facilities.
2. Define therapist qualification source.
3. Define patient consent/availability.
4. Define therapy location/capability if required.
5. Define session duration/resource/time.
6. Define trust/relationship requirements.
7. Define therapy-plan ownership.
8. Define how sessions request changes to phobia/recovery state.
9. Require second-tool review if therapy crosses medical and social authorities.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15S — Therapy session transaction

**Goal:** Schedule and resolve one therapy session safely through canonical activity systems.

### Required substeps

1. Validate therapist and patient.
2. Validate skill/qualification.
3. Validate trust/consent.
4. Validate time/location/resource.
5. Reserve both participants.
6. Create stable session ID.
7. Advance through job/activity scheduler.
8. Resolve session outcome once.
9. Apply psychology-owned recovery update and canonical mental-health/relations consequences through adapters.
10. Grant skill XP through SkillProgression.
11. Add interruption/save/reload/idempotency tests.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15T — Therapy effectiveness model

**Goal:** Make therapy useful but gradual and non-guaranteed.

### Required substeps

1. Use phobia/recovery policy, session quality, therapist skill, trust, patient willingness, recent stressors, and session consistency.
2. Do not use one fixed severity reduction for every session.
3. Cap per-session change.
4. Allow no-change sessions.
5. Allow setbacks only with clear context and low frequency.
6. Keep clinical realism abstract but avoid instant cures.
7. Add deterministic or keyed-outcome tests.
8. Measure sessions-to-improvement distribution.
9. Balance time opportunity cost.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15U — Recovery-arc state machine

**Goal:** Represent longitudinal recovery without implying a single linear path for every survivor.

### Required substeps

1. Define states such as Unassessed, ActiveSymptoms, Stabilizing, EngagedInRecovery, Improving, Managed, RelapseRisk, Relapsed only if each changes behavior/UI.
2. Do not call 'Recovered' a permanent immunity state.
3. Allow recurrence from new trauma.
4. Keep active crisis state in MentalHealthCrisisSystem.
5. Define transitions from evidence/session outcomes.
6. Persist only profile-owned recovery state.
7. Add transition/save tests.
8. Keep state count small.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15V — Psychological assessment and diagnosis

**Goal:** Make detailed profile information an earned/observed read model where the game supports uncertainty.

### Required substeps

1. Audit whether player normally knows all survivor mental-health state.
2. Define what is always visible versus assessment-revealed.
3. Therapy/medical assessment may reveal phobia, vulnerability, coping, or trigger detail.
4. Do not hide information necessary for fair task assignment if gameplay requires it.
5. Do not expose every hidden source-system value.
6. Add tests for unassessed, assessed, changing state, and old-save defaults.
7. Use canonical knowledge/diagnosis authority if one exists.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15W — Personality-evolution ownership decision

**Goal:** Determine whether long-term personality changes belong in psychology or the existing survivor trait/personality authority.

### Required substeps

1. Audit base traits, temperament, ideology, preferences, autonomy modifiers, and relation traits.
2. Separate core identity traits from acquired longitudinal adaptations.
3. Prefer `PersonalityAdaptation`/experience-derived modifiers rather than rewriting foundational personality labels.
4. Define provenance and bounded magnitude.
5. Define reversibility/permanence by adaptation type.
6. Do not overwrite a survivor into aggressive/cautious with one combat event.
7. Require repeated evidence/milestones.
8. Add tests for gradual change and no-op when evidence is insufficient.
9. Document consumer systems.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15X — Personality adaptation model

**Goal:** Implement gradual experience-derived tendencies only through explicit canonical consumers.

### Required substeps

1. Examples may include cautious_under_fire, protective_after_loss, socially_withdrawn, seeks_support, conflict_avoidant, vigilant, resilient_reframing.
2. Do not make every adaptation a stat buff/debuff.
3. Use traits/preferences/autonomy policy as consumers.
4. Cap active adaptations.
5. Define conflict resolution.
6. Allow later evolution/replacement only with strong evidence.
7. Add save/load tests.
8. Add reason-trace support for behavior affected by adaptations.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15Y — Autonomy and refusal integration

**Goal:** Use E1-6-style survivor agency for avoidance, help-seeking, coping, and refusal without duplicating autonomy scoring.

### Required substeps

1. Expose psychology context/opportunities to Autonomy.
2. Phobia may contribute a structured refusal reason during duty validation.
3. Autonomy may propose therapy, coping, seeking support, leaving a trigger context, or asking for reassignment.
4. Do not create a psychology-side action scheduler.
5. Player override/coercion uses governance/duty policy.
6. Route consequences through MentalHealth/Relations.
7. Add tests with autonomy enabled/disabled.
8. Ensure no duplicate refusal logic.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15Z — Duty and work preference integration

**Goal:** Make psychological constraints visible in assignment validation rather than hidden work penalties.

### Required substeps

1. Expose trigger conflicts to duty authority.
2. Show avoidance/preferences before assignment where fair.
3. Severe active phobia may cause soft/hard refusal according to policy.
4. Emergency task override policy remains external.
5. Do not globally reduce work performance for unrelated duties.
6. Add tests for relevant task, irrelevant task, emergency context, successful coping/support, and save/load.
7. Use clear reason text.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15AA — Needs and morale integration

**Goal:** Route distress/support outcomes through canonical Needs/MentalHealth state.

### Required substeps

1. Define psychological stressor/support event policies.
2. Triggered phobia may emit distress event.
3. Successful coping/therapy may emit support/recovery event.
4. Do not set morale directly.
5. Do not duplicate sleep effects already owned by GuiltInsomnia.
6. Prevent one source event from being counted twice via source system + profile.
7. Use provenance IDs.
8. Add tests for dedupe and bounded effects.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15AB — Relations integration

**Goal:** Make support, withdrawal, conflict, therapy trust, and caregiver burden relationship-aware through canonical relations.

### Required substeps

1. Use Relations to query trust/affinity for therapy/support eligibility.
2. Submit social outcome events rather than direct deltas.
3. Do not make a phobia automatically reduce all relationships.
4. Allow supportive relationships to create coping/recovery opportunities.
5. Prevent double application when source trauma system already emitted a relation effect.
6. Add provenance-aware tests.
7. Use E1-7 ideological/social systems only where relevant.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15AC — SkillProgression integration

**Goal:** Use existing skill authority for therapist competence and survivor learning.

### Required substeps

1. Identify psychology/medical/social skill IDs or closest canonical competence.
2. Do not invent a new therapy XP ledger.
3. Grant XP only from real completed sessions/training.
4. Define qualification thresholds through skill/profession data.
5. Add tests for qualified/unqualified therapist, XP once, mentoring, save/load.
6. Defer certification specialization to follow-on.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15AD — DreamSystem integration

**Goal:** Let dreams consume psychological context without making dreams a recovery authority.

### Required substeps

1. Audit Plan 177/live DreamSystem contract.
2. Expose active vulnerability/phobia/trauma evidence tags as read-only context.
3. DreamSystem owns dream selection/content.
4. Dream outcome may emit a psychological evidence/support/stressor event if designed.
5. Do not let one dream directly cure a phobia.
6. Use stable provenance.
7. Add tests with DreamSystem absent/disabled and enabled.
8. Keep dependency optional.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15AE — Aging/lifecycle integration

**Goal:** Use age/life stage as one contextual factor, not a universal resilience modifier.

### Required substeps

1. Audit Plan 176/live AgingSystem.
2. Define whether age influences recovery, vulnerability, sleep, or coping through existing design.
3. Do not hard-code older = less resilient or younger = more adaptable.
4. Use data-driven/contextual policy.
5. Allow experience accumulated over time to affect coping/resilience separately from chronological age.
6. Add tests across life stages only where canonical age mechanics exist.
7. Feature-gate if AgingSystem is absent.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15AF — MentalHealthCrisis integration

**Goal:** Keep acute crises under the existing crisis pipeline while allowing profile context to influence prevention/recovery.

### Required substeps

1. Psychology may provide vulnerability/context inputs to crisis eligibility only through documented adapter.
2. Crisis system owns acuity and acute resolution.
3. Therapy/coping may reduce risk via canonical policies.
4. Profile records crisis evidence for longitudinal arcs.
5. Do not mirror crisis acuity.
6. Do not suppress crisis because profile says 'recovering'.
7. Add tests for crisis onset, recovery, repeated crisis, profile evidence, and no duplicate effects.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15AG — CombatTrauma integration

**Goal:** Use combat-trauma events as evidence and avoid reimplementing hypervigilance/false alarms.

### Required substeps

1. Consume stable trauma milestones/episodes.
2. Allow repeated combat trauma to contribute to specific vulnerabilities/phobia eligibility.
3. Do not duplicate combat trauma decay/progression.
4. Do not duplicate false-alarm behavior.
5. Use canonical combat/duty context tags.
6. Add tests for evidence ingestion, phobia candidate generation, and disabled profile parity.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15AH — SomaticFlashback integration

**Goal:** Use somatic flashbacks as evidence while preserving their existing trigger/effect mechanics.

### Required substeps

1. Consume flashback episode and trigger class.
2. Map repeated trigger patterns to vulnerability context only where justified.
3. Do not duplicate flashback effects.
4. Do not infer a phobia from every flashback.
5. Allow therapy/coping context to influence source system only through explicit supported API.
6. Add tests for repeated noise triggers, unrelated phobia, and no duplicate distress.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15AI — GuiltInsomnia integration

**Goal:** Treat guilt-linked sleep disruption as evidence while keeping sleep effects canonical.

### Required substeps

1. Consume guilt/insomnia milestones.
2. Do not duplicate sleep penalties in profile.
3. Allow profile to surface guilt vulnerability/recovery context.
4. Therapy/coping may route support to the existing guilt/sleep authority.
5. Add tests for active insomnia, resolved state, profile summary, and no double sleep effect.
6. Keep sleep ownership in Needs/GuiltInsomnia.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15AJ — PsychologicalContamination integration

**Goal:** Use location-linked psychological contamination as evidence without conflating it with radiation/biological contamination.

### Required substeps

1. Preserve its existing semantics.
2. Consume location/context trigger evidence.
3. Map recurring contaminated-location fear to relevant vulnerability/phobia eligibility only when tags align.
4. Do not create a generic 'radiation phobia' merely because the system name contains contamination.
5. Use actual event context.
6. Add tests for maritime/location triggers, radiation context mismatch, and dedupe.
7. Keep source state authoritative.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer.
- Reload rerolls phobia development.
- One traumatic event guarantees a diagnosis with no policy.
- Repeated ordinary exposure automatically worsens phobia every tick.
- Therapy directly grants skill XP outside SkillProgression.
- Psychology duplicates E1-6 refusal/autonomy scoring.
- Old-save migration invents historical therapy/personality events.

### Acceptance evidence

- [ ] Six-source adapter tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Determinism/save tests pass.
- [ ] Therapy/coping integration passes.
- [ ] No duplicate effects from source+profile.
- [ ] Clinical/tone review passes.
- [ ] Performance/usability evidence is captured.

---

## E1-15AK — PhantomMemory integration

**Goal:** Use background/item-triggered memories as longitudinal context without duplicating memory ownership.

### Required substeps

1. Consume stable phantom-memory trigger events.
2. Keep narrative memory content in PhantomMemoryEngine.
3. Allow repeated memory themes to contribute to vulnerability/coping/personality adaptation.
4. Do not store duplicate memory text in profile.
5. Add tests for repeated item trigger, resolved/ignored memory, profile evidence, and save/load.
6. Use stable memory IDs.

### Psychology integration invariants

- The six existing trauma systems remain authoritative for their own mechanics.
- Psychology integrates longitudinal evidence and owns only profile-specific facts.
- Needs, Relations, Duty, Autonomy, Skills, Dreams, and Aging remain canonical.
- Phobias produce structured constraints/responses rather than direct generic penalties.
- Therapy is a real scheduled activity and cannot be farmed by reload/retry.
- Development decisions are deterministic and provenance-tracked.
- Old saves receive no fabricated biography.
- Profile/history size and player-facing complexity are bounded.

### Negative tests

- PsychologicalProfile duplicates crisis acuity or current morale.
- A phobia directly subtracts work speed instead of using a canonical consumer

<!-- Deliverable capped to remain within the requested 50–90k character envelope. -->
