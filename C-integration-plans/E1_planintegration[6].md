---
PLAN_ID: E1-6
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 6
STATUS: READY_FOR_EXECUTION_WHEN_RAILS_PASS
SOURCE_PLAN: "Plan 144 — Survivor Autonomy & Initiative"
SEQUENCE_FILENAME: "E1_planintegration[6].md"
PREVIOUS_FILENAME: "E1_planintegration[5].md"
NEXT_FILENAMES:
  - "E1_planintegration[7].md"
  - "E1_planintegration[8].md"
CATEGORY: LINK+SURVIVOR_AGENCY+DECISION_POLICY+PRESENTATION
PRIMARY_INTENT: "Give survivors legible initiative and refusal through a bounded decision/proposal layer that consumes canonical state and delegates effects to existing authorities."
EXECUTION_STYLE: "Flagship integration plan"
PREMISE_VERIFICATION_REQUIRED: true
INTAKE_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
PLAYER_OVERRIDE_POLICY_EXPLICIT: true
AUTONOMY_MUST_BE_EXPLAINABLE: true
RUNTIME_RISK: HIGH
SAVE_RISK: MEDIUM
BALANCE_RISK: HIGH
PLAYER_FRICTION_RISK: VERY_HIGH
---

# E1 Plan Integration [6] — Survivor Autonomy, Initiative, Refusal, Preferences, Goals, and Explainable Agency

> **Sequence rule:** this file is `E1_planintegration[6].md`.
> The next files are `E1_planintegration[7].md`, `E1_planintegration[8].md`, and so on.
> The bracketed sequence number remains immediately before `.md`.

## 0. Mission

This plan expands Plan 144 into an implementation-grade survivor-agency programme.

The desired player experience is strong: survivors should not feel like inert stat containers waiting for
orders. They should sometimes help one another, object to work, express preferences, pursue relationships,
take low-risk initiative, and surface personal goals. Those actions should make existing relationship,
needs, mental-health, duty, skill, and moral-choice data matter more often and more visibly.

The architectural risk is equally strong. A monolithic `SurvivorAutonomySystem` that directly changes hunger,
morale, relationships, duty assignments, skill XP, faction standing, projects, quests, and shelter bonuses
would become a second authority over almost every survivor system in the game. That would be difficult to
save deterministically, difficult to debug, and very likely to create actions the player experiences as
random sabotage.

E1-6 therefore uses a stricter model:

**canonical state → autonomy evaluation → proposed intent → authority validation → authoritative outcome →
explanation/history**

The autonomy layer may decide that a survivor *wants* to help, refuse, express, initiate, or pursue. It does
not get to bypass the systems that own food, work, relationships, morale, skills, projects, quests, or
standing.

## 1. Source Plan Intent Preserved

The source Plan 144 asks for five action families:

- Help
- Refuse
- Initiate
- Express
- Pursue

It also asks to use relationship, need, emotion, personality, and opportunity triggers; deterministic RNG;
cooldowns; logging; UI/journal surfaces; relationship/work consequences; skill integration; moral-choice
hooks; old-save compatibility; data templates; and a headless selftest.

Those objectives remain, but the implementation is reorganized around explainable decision policy,
authority-owned effects, and player-friction constraints.

## 2. Core Model

```text
Canonical survivor/world state
    |
    +--> Needs
    +--> Relations
    +--> Duty / commitments
    +--> Mental health
    +--> Traits / skills
    +--> Opportunity / free time
    +--> Recent events / memories
    |
    v
Autonomy evaluator
    |
    v
Candidate intents
    |
    +--> HelpIntent
    +--> RefusalIntent
    +--> ExpressionIntent
    +--> InitiativeIntent
    +--> PursuitIntent
    |
    v
Scoring / arbitration / cooldown / policy
    |
    v
Chosen intent
    |
    v
Canonical authority command/query
    |
    +--> accepted
    +--> rejected
    +--> deferred
    +--> player decision required
    |
    v
Outcome record + explanation + UI/journal
```

Autonomy owns **intent selection and recent-autonomy history**. It does not own the underlying effect.

## 3. Non-Negotiable Rules

- Survivor needs remain owned by `NeedsSystem` or the canonical survivor aggregate.
- Relationships remain owned by `SurvivorRelationsSystem` or its canonical successor.
- Duty assignment and work availability remain owned by duty/roster systems.
- Mental-health state remains owned by mental-health/crisis systems.
- Skill XP and progression remain owned by the skill authority.
- Inventory/food remains owned by inventory and consumption systems.
- Faction standing remains owned by faction/standing systems.
- Quests remain owned by quest systems.
- Projects/jobs remain owned by production/task systems.
- Autonomy never directly grants “+5 morale,” “+5 affinity,” or “+10% efficiency” as hidden state.
- Every autonomous action has a reason trace that can be inspected in debug/test surfaces.
- Probability is not a substitute for motivation. Candidate generation should be driven by meaningful state.
- Survivors cannot autonomously spend rare/critical resources unless an explicit policy permits it.
- Survivors cannot autonomously start high-risk expeditions/combat missions in the foundational slice.
- Refusal must distinguish unsafe, impossible, value-conflicting, exhausted, and relationship-based causes.
- Player override rules must be explicit and authority-owned.
- Autonomous help cannot steal or consume another survivor's critical ration without canonical transfer rules.
- Cooldowns are per action family and context, not one arbitrary global silence timer.
- Save/reload cannot reroll a decision that was already committed.
- Determinism uses `ISeededRng` or the existing deterministic RNG rail.
- Autonomy evaluation must be bounded; no O(all survivors × all survivors × all actions) explosion each tick.
- No action template may reference missing trait, need, relationship, task, or event IDs.
- Old saves default to safe empty autonomy history/preferences without synthetic past actions.

## 4. Acceptance Slices

### Slice A — Explainable expressions and low-risk help
Preferences, complaints/praise, comfort suggestions, and help proposals that use existing authorities.

### Slice B — Duty preference and refusal
Survivors can object or refuse under explicit safety/need/relationship thresholds.

### Slice C — Low-risk initiative
Practice, social interaction, maintenance assistance, or hobbies that consume free-time capacity.

### Slice D — Personal goals
Persistent bounded goals with progress delegated to canonical systems.

### Slice E — Narrative depth
Quest hooks, journals, event templates, leadership/rebellion follow-ons.

Do not begin mass refusal, autonomous expedition decisions, or shelter-wide leadership before Slice A/B are
accepted in playtests.


---

## E1-6A — Premise verification and survivor-agency authority audit

**Goal:** Verify what existing survivor systems already initiate, constrain, or react autonomously before introducing a new decision layer.

### Required substeps

1. Inspect survivor aggregate/state, `SurvivorRelationsSystem`, `NeedsSystem`, `MentalHealthCrisisSystem`, `PhantomMemoryEngine`, ration conflicts, duty roster, assignments, commitment/task systems, skill progression, moral choice, faction opinions, memory, traits, dialogue/social coordinators, and any existing AI/utility/behavior code.
2. Search for spontaneous or scheduled survivor actions beyond the examples named in the source plan.
3. Identify whether any existing Utility AI, action scheduler, social coordinator, behavior tree, or NPC intent framework should host survivor initiative.
4. Map every proposed autonomy effect to its canonical owner.
5. Separate 'decision to attempt' from 'effect if accepted'.
6. Verify how free time, busy state, sleep, sickness, expedition absence, incapacitation, and duty assignment are represented.
7. Verify whether survivor preferences already exist as traits, affinities, skills, profession, memories, or narrative tags.
8. Verify whether tasks have danger/essentiality/target-worker metadata sufficient for refusal reasoning.
9. Create `docs/systems/SURVIVOR_AUTONOMY_AUTHORITY_MAP.md`.
10. Create intake duplicate-search evidence.
11. Classify the new layer as LINK/POLICY unless it genuinely owns persistent goals/preferences not represented elsewhere.
12. Set `PREMISE_VERIFIED_AT` to current HEAD and stop if an existing canonical autonomy/utility authority already owns candidate selection.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6B — Autonomy ADR and ownership boundary

**Goal:** Define exactly what autonomy owns and what it may only propose to other authorities.

### Required substeps

1. Write an ADR comparing a standalone `SurvivorAutonomySystem`, extension of existing Utility AI/social coordinator, and a thin policy evaluator over canonical systems.
2. Define autonomy-owned state as candidate policy configuration, bounded cooldowns, persistent personal-goal references if needed, preference overrides if not canonical, and recent outcome history.
3. Explicitly exclude needs, relationships, morale, skill XP, inventory, duty assignments, faction standing, quest progress, and project state.
4. Define command/request boundaries for each action family.
5. Define when an action resolves automatically versus requiring player response.
6. Define whether player overrides are commands to duty/policy authorities rather than autonomy state mutation.
7. Define debug reason traces.
8. Define feature flags per action family so refusal/initiation can be disabled independently.
9. Require second-tool review before code.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6C — Intent DTOs and reason-trace contract

**Goal:** Represent survivor motivation as inspectable intent objects rather than opaque random effects.

### Required substeps

1. Define common intent fields: stable intent ID, actor ID, action family, target/context ID, generated tick/day, expiry, trigger facts, utility score, policy modifiers, RNG tie-break seed, status, and outcome reference.
2. Define typed intents for Help, Refuse, Express, Initiate, and Pursue rather than one stringly-typed action blob if repository conventions support it.
3. Represent trigger facts as references/snapshots needed for explanation, not duplicated live survivor state.
4. Define `ReasonTrace` entries such as fatigue_high, affinity_high, resentment_high, dangerous_task, free_time_available, recent_grief, preferred_duty, mentor_opportunity.
5. Define intent states: Candidate, Selected, Proposed, AwaitingPlayer, Accepted, Rejected, Deferred, Expired, Resolved.
6. Define deterministic stable IDs.
7. Bound reason-trace length.
8. Add validation for missing actor/target/context IDs.
9. Add fixtures for multi-trigger help, safety refusal, preference expression, goal pursuit, and rejected candidate.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6D — Candidate generation pipeline

**Goal:** Generate plausible actions from canonical state without rolling every possible action for every survivor.

### Required substeps

1. Evaluate only eligible survivors: alive, present, conscious, not in incompatible activity, and within the simulation context.
2. Use event-driven or dirty-flag triggers where possible instead of scanning every relationship every tick.
3. Generate Help candidates from visible unmet need + relationship/opportunity conditions.
4. Generate Refusal candidates at assignment/command validation time where possible rather than daily random refusal rolls.
5. Generate Express candidates from threshold crossings or witnessed events.
6. Generate Initiative candidates only when free-time/opportunity budgets exist.
7. Generate Pursuit candidates from persistent goals and available actions.
8. Cap candidate count per survivor/day.
9. Deduplicate candidates by actor/action/context.
10. Expire stale candidates deterministically.
11. Add performance tests around large shelters.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6E — Scoring, arbitration, and deterministic selection

**Goal:** Choose among plausible intents using explainable scores and deterministic tie-breaking.

### Required substeps

1. Define base utility by action family as data, not hard-coded magic percentages.
2. Apply trigger severity, relationship strength, need urgency, trait/personality, recent-history, and opportunity modifiers.
3. Apply policy blocks before RNG.
4. Use deterministic RNG only for near-ties or probabilistic expression frequency, not as the primary motivation engine.
5. Normalize or cap scores to prevent one trait from dominating all behavior.
6. Select at most a bounded number of intents per survivor/time window.
7. Prevent conflicting intents such as help target while simultaneously refusing all contact if mental state blocks it.
8. Record score components in debug traces.
9. Add same-seed determinism tests and changed-state sensitivity tests.
10. Add tests proving severe conditions predictably dominate weak personality modifiers.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6F — Cooldown and action-budget model

**Goal:** Prevent spam without making survivors artificially silent.

### Required substeps

1. Define action-family cooldowns separately for Help, Refuse, Express, Initiate, and Pursue.
2. Define context cooldowns such as repeated complaint about same condition or repeated help offer to same target.
3. Define daily/segment autonomy budget per survivor.
4. Allow severe safety/need refusal to bypass cosmetic cooldowns.
5. Do not let cooldown suppress canonical crises or emergency behavior.
6. Persist only cooldowns needed across save/load.
7. Use monotonic game time, not wall clock.
8. Add tests for repeated trigger, different target, severe override, save/load, and expiry.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6G — Help intent: comfort and social support

**Goal:** Implement low-risk spontaneous support by delegating actual social/morale effects to canonical authorities.

### Required substeps

1. Define eligibility for comfort based on target grief/stress/morale state if exposed canonically.
2. Require relationship/trust/affinity threshold or trait/opportunity rules.
3. Use a social interaction command/event to resolve whether target accepts.
4. Let relationship/morale systems own resulting changes.
5. Define refusal/rejection of comfort where personality/context supports it.
6. Add cooldowns to avoid repeated morale farming.
7. Record the action and outcome for UI/journal.
8. Add tests for high-affinity comfort, rejected comfort, no target need, cooldown, and save/reload.
9. Do not hard-code +5 morale in autonomy.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6H — Help intent: work assistance

**Goal:** Let survivors volunteer for bounded assistance without bypassing roster, capacity, or task ownership.

### Required substeps

1. Identify tasks/jobs that can accept an assistant through canonical work APIs.
2. Define overloaded coworker or under-resourced task signals.
3. Check helper availability, fitness, skill, and conflicting assignments.
4. Submit an assist proposal to duty/job authority.
5. Let the job system calculate throughput benefit.
6. Do not apply a blanket pair work-speed bonus from autonomy.
7. Define duration and cancellation.
8. Add tests for accepted assist, unavailable helper, incompatible task, helper becoming unfit, and job completion.
9. Measure micromanagement reduction.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6I — Help intent: resource sharing

**Goal:** Allow survivor-initiated sharing only through canonical inventory/ration transfer and protection policies.

### Required substeps

1. Define what resources may be autonomously shared in the first slice—prefer low-risk personal rations or social-use items if the inventory model supports ownership.
2. Do not autonomously transfer critical medicine, ammunition, quest items, rare equipment, or shelter reserves without explicit policy.
3. Verify giver actually owns/controls the resource.
4. Use canonical transfer/consumption transaction.
5. Check giver need so altruism cannot repeatedly starve the helper accidentally unless the design explicitly allows sacrifice.
6. Define target acceptance.
7. Record outcome through needs/relations systems.
8. Add conservation tests and cooldowns.
9. Do not implement '-5 hunger for both' as an autonomy-side arithmetic shortcut.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6J — Mentoring and skill assistance

**Goal:** Connect spontaneous mentoring to existing skill/proficiency rails without inventing XP transfer arithmetic.

### Required substeps

1. Audit mentor/training systems and skill XP authority.
2. Define mentor eligibility from real skill thresholds/traits.
3. Define learner eligibility and free-time/task context.
4. Create mentor-session request through canonical training/skill APIs.
5. Let skill authority determine XP, duration, caps, and prerequisites.
6. Prevent infinite reciprocal mentoring loops.
7. Add cooldown and daily training budget.
8. Record mentor relationship effects through relations authority.
9. Add tests for valid mentor, same-level invalid, unavailable learner, repeated session, save/reload, and XP idempotency.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6K — Refusal classification and duty-validation integration

**Goal:** Make refusal a reasoned validation outcome at assignment time rather than a random daily strike.

### Required substeps

1. Define refusal causes: physically unable, critically exhausted, unsafe/dangerous, trauma-linked, moral/value conflict, relationship conflict, non-essential low-morale objection, policy/protest.
2. Separate hard refusal from soft objection and preference warning.
3. Hook refusal evaluation into duty assignment/command validation.
4. Use task metadata for danger, essentiality, target coworker, location, and trauma tags.
5. Return structured refusal result with reason trace.
6. Define player options: accept refusal, negotiate/reassign, override if policy allows.
7. Do not let autonomy itself assign penalties; delegate override consequences to canonical morale/relations/governance systems.
8. Add tests for each refusal class.
9. Ensure essential life-saving tasks have explicit policy rather than hidden exemption.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6L — Override, coercion, consent, and consequence policy

**Goal:** Define player authority over refusals in a way that is mechanically clear and narratively consistent.

### Required substeps

1. Audit consent/leadership/governance systems if present.
2. Define which refusals may be overridden and under what policy.
3. Define hard blocks for physical incapacity or impossible tasks.
4. Define coercive override as a canonical command carrying provenance.
5. Route morale, resentment, trust, grievance, discipline, or leadership consequences through owning systems.
6. Record repeated coercion for future behavior if memory/governance rails support it.
7. Prevent override loops where the player repeatedly retries to reroll acceptance.
8. Add tests for allowed override, prohibited override, accepted refusal, repeated coercion, and save/load.
9. Expose consequences before confirmation in UI where feasible.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6M — Duty preferences and expressed work affinity

**Goal:** Let survivors express preferred/avoided duties without directly applying hidden efficiency modifiers.

### Required substeps

1. Audit profession, skill, trait, memory, and duty preference data.
2. Define preference profile as derived from canonical facts where possible.
3. Allow explicit persistent preference only when it is a genuine character fact not otherwise represented.
4. Emit preference expression when assignment or free-time context makes it relevant.
5. Show preferred/avoided duties in roster UI.
6. Let duty/work authority decide any efficiency effect from skill/fit.
7. Do not add autonomy-only +efficiency bonuses.
8. Add tests for preferred duty, disliked duty, conflicting need, changed skill, and roster presentation.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6N — Expression system: praise, complaint, suggestion, reaction

**Goal:** Make survivors voice state changes and opinions without turning expressions into hidden global buffs/debuffs.

### Required substeps

1. Generate expressions from meaningful threshold crossings or witnessed events.
2. Define categories: praise, complaint, suggestion, concern, gratitude, objection, grief, fear, approval, disapproval.
3. Use existing dialogue/voice/journal presentation rails.
4. Treat expression as information first; any social consequence must go through canonical social systems.
5. Prevent repeated identical complaint spam using context cooldown.
6. Let unresolved complaints remain visible as conditions rather than repetitive messages.
7. Add tests for threshold crossing, repeated unchanged state, resolved condition, conflicting traits, and save/load history.
8. Keep global morale effects minimal and authority-owned.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6O — Faction opinions and moral-expression boundary

**Goal:** Allow survivors to express faction/moral opinions without directly changing faction standing merely by speaking.

### Required substeps

1. Audit survivor ideology/moral memory/faction preference state.
2. Represent opinions as survivor expression or preference state.
3. Do not mutate external faction standing because a survivor voiced an opinion.
4. If the player acts on the opinion, route consequences through moral-choice/faction systems.
5. Allow internal relations changes between survivors with opposing views only through relations authority.
6. Create quest/dialogue hooks if accepted.
7. Add tests proving expressions alone do not alter external faction standing.
8. Document the difference between internal opinion and external diplomacy.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6P — Low-risk initiative: self-practice and maintenance

**Goal:** Let survivors use free time productively through existing activity/job systems.

### Required substeps

1. Define a whitelist of low-risk self-initiated activities for first release.
2. Examples may include skill practice, reading/training, cleaning/maintenance assist, exercise, journaling, hobby activity, or social visit if supporting authorities exist.
3. Check free time, fitness, resource cost, equipment, and task availability.
4. Submit activity through canonical job/activity scheduler.
5. Do not let autonomy spawn shelter upgrades or consume rare resources directly.
6. Let skill/morale/maintenance authorities resolve benefits.
7. Add tests for free time, occupied survivor, missing equipment, no resource cost, cancellation by new duty, and save/reload.
8. Measure reduced idle-time micromanagement.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6Q — Initiated social activities

**Goal:** Allow survivors to organize small social actions without creating shelter-wide bonuses from nowhere.

### Required substeps

1. Audit social event/mental-health systems.
2. Define small activities such as conversation, shared meal invitation, memorial visit, card/game session, or group rest where existing content supports them.
3. Check participants' availability and relationship context.
4. Use canonical event/social scheduler.
5. Let morale/relations systems resolve effects.
6. Apply participant caps and cooldowns.
7. Prevent event loops where one event triggers another indefinitely.
8. Add tests for accepted invite, declined participant, unavailable participant, repeated event, and social consequence ownership.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6R — Personal goal model

**Goal:** Add bounded survivor goals only where they provide longitudinal agency beyond one-off actions.

### Required substeps

1. Define goal ID, actor ID, goal type, source, start day, target references, progress query, completion condition, blocked condition, expiry/abandonment policy, and narrative state.
2. Keep progress derived from canonical systems whenever possible.
3. Do not store duplicate skill XP, relation values, inventory quantities, or quest progress inside the goal.
4. First goal types should be small: improve a skill, repair a relationship, recover from trauma, complete a personal project, mentor someone.
5. Define how goals are selected—traits/memories/events—not arbitrary random assignment.
6. Cap active goals per survivor to one primary and perhaps one minor goal.
7. Allow goals to be abandoned or replaced with reason trace.
8. Add save/load and determinism tests.
9. Keep goal completion rewards narrative/authority-owned; no generic trait bonus from autonomy.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6S — Goal pursuit scheduler

**Goal:** Use free-time opportunities to advance goals through real actions rather than ticking an abstract progress bar.

### Required substeps

1. Map each goal type to one or more canonical activities.
2. Generate pursuit candidate only when an eligible activity exists.
3. Submit activity through canonical task/social/skill/therapy systems.
4. Query completion from source authority after outcome.
5. Do not increment goal progress merely because an autonomy tick occurred.
6. Handle interruption and blocked goals.
7. Record attempt and outcome.
8. Add tests for activity progress, blocked goal, interrupted activity, completion, abandonment, and no duplicate completion after reload.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6T — Personality/trait modifiers

**Goal:** Use existing character traits to bias intent selection while avoiding rigid stereotypes and runaway modifiers.

### Required substeps

1. Audit actual trait catalog before adding Independent/Social/Stubborn/Ambitious/Anxious labels.
2. Map only existing traits unless new traits pass content intake.
3. Use small bounded modifiers to utility scores.
4. Do not make a trait guarantee an action.
5. Allow state severity to outweigh personality.
6. Prevent stacking dozens of modifiers from exceeding score caps.
7. Expose contributing traits in debug reason trace.
8. Add tests for modifier direction, cap, severe-state dominance, and no missing trait references.
9. Keep the source plan's +20/-10 numbers as tunable hypotheses, not hard-coded facts.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6U — Relationship feedback and reciprocity

**Goal:** Let resolved autonomous interactions feed relationships through canonical social consequence APIs.

### Required substeps

1. Map accepted help, rejected help, coercive override, mentoring, social invitation, and goal support to existing relation consequence events.
2. Do not directly add affinity/trust/resentment in autonomy code.
3. Use context-specific consequence strengths from relations/balance data.
4. Prevent one autonomous event from double-applying because both initiator and target handlers observe it.
5. Record provenance IDs.
6. Add tests for reciprocal relationship update, rejection, override, duplicate event, and save/reload.
7. Measure whether autonomy meaningfully activates otherwise-unused relationship data.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6V — Mental-health and crisis boundary

**Goal:** Use mental-health state as motivation and constraint without replacing crisis authority.

### Required substeps

1. Read crisis/trauma/stress/grief APIs.
2. Generate withdrawal, complaint, refusal, comfort-seeking, or therapy-seeking candidates where supported.
3. Do not resolve crises inside autonomy.
4. Do not allow normal autonomy cooldowns to suppress urgent crisis behavior.
5. Define safety priority so suicidal, psychotic, incapacitated, or otherwise critical states remain under dedicated crisis logic.
6. Route supportive actions to mental-health APIs.
7. Add tests for grief comfort, trauma-linked refusal, crisis priority, cooldown bypass, and recovery state.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6W — Moral-choice integration

**Goal:** Allow survivor agency to surface moral disagreement without duplicating moral-choice resolution.

### Required substeps

1. Identify moral-choice events survivors witness or are affected by.
2. Generate expressions, objections, refusal, or support from canonical moral memory/traits.
3. Do not resolve player moral choices automatically.
4. Allow survivor objection to become a decision input or consequence.
5. Route relationship/morale effects through canonical systems.
6. Create quest/event hooks only after base action acceptance.
7. Add tests for witnessed choice, uninvolved survivor, conflicting moral positions, and no duplicate moral resolution.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6X — Player communication and predictability

**Goal:** Make autonomy understandable enough that friction feels character-driven rather than random.

### Required substeps

1. Expose current major preferences, stressors, and likely refusal reasons in survivor UI before assignment where appropriate.
2. Show structured reason after an autonomous action.
3. Distinguish 'couldn't', 'wouldn't', and 'preferred not to'.
4. Show cooldown/recent-history only in debug or subtle UI unless player value justifies full exposure.
5. Provide tutorial on first expression/help/refusal, not a giant one-time autonomy tutorial.
6. Allow the player to inspect why an override carries consequences.
7. Add UI tests for clear reason labels.
8. Run playtest prompts asking whether players could predict the refusal/help action before it happened.
9. Do not expose exact hidden probability percentages unless the design intentionally uses them.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6Y — Autonomy read model and survivor panel integration

**Goal:** Present recent agency through existing survivor UI rather than creating an isolated management console.

### Required substeps

1. Audit current survivor detail, roster, relationship, journal, and activity panels.
2. Add recent autonomous actions to survivor detail or activity history.
3. Add current goal/preferences only if persistent and player-relevant.
4. Add action status such as proposed, accepted, rejected, overridden, completed.
5. Link target survivor/task where navigation supports it.
6. Display authority-generated consequence summary.
7. Do not let the UI mutate outcomes directly except through canonical player decision commands.
8. Add snapshot tests for help, refusal, expression, active goal, and no-history state.
9. Ensure history is bounded.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6Z — Autonomy journal and retention

**Goal:** Record meaningful survivor-initiated moments without producing hundreds of low-value log lines.

### Required substeps

1. Classify actions as journal-worthy, recent-history-only, or debug-only.
2. Keep routine micro-actions out of the global chronicle.
3. Record first-time, relationship-changing, refusal/override, goal completion, major initiative, and narratively significant actions.
4. Use canonical journal/chronicle authority if present.
5. Store provenance/action IDs for dedupe.
6. Apply retention caps to recent autonomy history.
7. Summarize repeated complaints/preferences instead of logging each tick.
8. Add tests for dedupe, pruning, save/load, and deterministic ordering.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6AA — Action template catalog

**Goal:** Create a data-driven template layer for candidate policies and presentation without encoding cross-system effects in JSON.

### Required substeps

1. Define template ID, action family, eligibility tags, trigger references, score weights, cooldown class, target rules, required authority capability, presentation key, and risk class.
2. Do not let templates directly specify arbitrary changes to morale, relations, skills, standing, or inventory.
3. Reference consequence policy IDs owned by canonical adapters.
4. Validate trait, need, task, relationship, event, and target tags.
5. Start with a small accepted corpus before authoring 20.
6. Ensure templates are localization-ready.
7. Add invalid ID, invalid action family, missing capability, negative cooldown, impossible target rule, and duplicate ID tests.
8. Expand toward 20 only after behavior distribution is healthy.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6AB — Save contract and old-save compatibility

**Goal:** Persist the minimum autonomy state needed to prevent rerolls and preserve goals/history.

### Required substeps

1. Determine whether existing survivor save sections can own preferences/goals/cooldowns before adding a new section.
2. Persist stable active intent only when it spans save boundaries or awaits player decision.
3. Persist cooldowns, active personal goals, bounded recent outcome history, and deterministic selection state if required.
4. Do not persist canonical need/relation/task snapshots as autonomy truth.
5. Default old saves to no active intents, empty history, and derived/default preferences.
6. Define schema version.
7. Add migration fixture from no autonomy state.
8. Add round-trip tests for awaiting-player refusal, active goal, cooldown, recent action, and no-history.
9. Ensure restore does not reroll an already-selected intent.
10. Ensure stale intents whose context no longer exists resolve safely.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6AC — Exploit and pathological-state audit

**Goal:** Prevent autonomy from causing resource duplication, permanent shelter lockout, or retry-based manipulation.

### Required substeps

1. Test player save/reload around refusal to ensure outcome does not reroll.
2. Test repeated assignment attempts do not bypass a hard refusal unless policy/context changes.
3. Test all survivors exhausted and determine emergency/essential-task fallback policy.
4. Test all survivors resent one another without permanently deadlocking shelter operation.
5. Test repeated help does not farm morale/relations.
6. Test repeated mentoring does not farm XP.
7. Test resource sharing conserves inventory.
8. Test initiative cannot spend restricted resources.
9. Test goal completion cannot award twice.
10. Test action history remains bounded.
11. Add adversarial fuzz over random needs/relations/assignments if infrastructure supports it.
12. Document emergency fail-safe behavior without making autonomy meaningless.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6AD — Performance and scheduling soak

**Goal:** Prove the autonomy layer remains cheap and bounded as survivor count and relationship density grow.

### Required substeps

1. Benchmark candidate generation at realistic survivor counts and a synthetic high-count stress case.
2. Measure evaluation count, candidate count, selected intent count, allocations, tick time, and save size.
3. Prevent all-pairs relation scans every simulation tick.
4. Use indexes/events to focus on changed relationships/needs/opportunities.
5. Cap reason traces and histories.
6. Run a 180-day headless simulation with help/refusal/expression/initiative/goal behavior.
7. Check action distribution for spam or starvation.
8. Record median/p95 autonomy tick cost.
9. Add regression thresholds.
10. Block content expansion if complexity grows superlinearly with relationship count.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6AE — Player-friction and behavioral distribution tuning

**Goal:** Validate that autonomy creates personality and reduced micromanagement rather than arbitrary obstruction.

### Required substeps

1. Track action-family frequencies per survivor/week.
2. Track refusal rate by essential/non-essential task.
3. Track override rate.
4. Track help acceptance/rejection.
5. Track initiative completion/cancellation.
6. Track goal completion/abandonment.
7. Track player response time to autonomy prompts.
8. Track repeated identical expressions.
9. Run playtests with autonomy off vs Slice A/B enabled.
10. Ask whether players could understand why actions happened.
11. Set tuning targets from evidence rather than fixed source-plan 20/10/5/15/10% daily probabilities.
12. Ensure refusal frequency stays low enough for agency without constant workflow interruption.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6AF — Quest/event hooks

**Goal:** Add authored scenarios only after base agency produces reliable, understandable outcomes.

### Required substeps

1. Implement quest hooks such as Mediator, Project, Refusal, and Goal as consumers of existing autonomy outcome events.
2. Quest system owns quest state.
3. Use stable action/goal provenance.
4. Do not make quests required to resolve ordinary autonomy.
5. Prevent duplicate spawning from repeated events/reload.
6. Add tests for eligibility, duplicate guard, expired action, completed goal, and clean save/load.
7. Keep first release content modest.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6AG — Leadership, rebellion, and mass-action follow-on gate

**Goal:** Explicitly defer shelter-wide autonomous governance until individual agency rails are proven.

### Required substeps

1. Define leadership, strikes, rebellion, coordinated refusal, autonomous expedition leadership, and political blocs as separate follow-on scope.
2. Require governance/consent/leadership rails before coordinated mass actions.
3. Do not implement 'The Strike' as a shelter-paralyzing random event in the foundational slice.
4. Use individual refusal data as future input, not immediate mass-state ownership.
5. Require satisfiability/fail-safe tests before any mass refusal feature.
6. Document follow-on criteria: individual action explainability, stable refusal policy, player-friction metrics, and authority integration.
7. Keep current plan limited to individual/small-group agency.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6AH — Headless selftest and data-integrity validation

**Goal:** Create a deterministic scenario proving agency decisions reach canonical authorities without UI.

### Required substeps

1. Create two survivors with known affinity, one stressed target, one duty conflict, one available help action, and one refusal condition.
2. Run autonomy evaluation.
3. Assert deterministic candidate/selection result.
4. Resolve one help through social authority.
5. Resolve one refusal through duty validation.
6. Save/reload before an awaiting-player decision and verify no reroll.
7. Advance cooldown and verify eligibility restoration.
8. Create one personal goal and advance it through a real canonical activity.
9. Assert no direct changes to protected canonical fields from autonomy code.
10. Validate action templates against trait/need/task catalogs.
11. Add `--survivor-autonomy-selftest` if project selftest conventions support it.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

## E1-6AI — Release gate and closure

**Goal:** Ship survivor agency only when it is explainable, bounded, deterministic, and authority-safe.

### Required substeps

1. Run .NET build/test and game build.
2. Run data-integrity selftest.
3. Run survivor-autonomy selftest.
4. Run save/migration suite.
5. Run deterministic no-reroll tests.
6. Run duty refusal/override policy tests.
7. Run relationship/help/mentoring/resource-conservation tests.
8. Run goal lifecycle tests.
9. Run 180-day autonomy soak.
10. Run UI reason-trace tests.
11. Capture player-friction metrics.
12. Verify at least one meaningful action from Help, Refuse, Express, Initiate, and Pursue while keeping high-risk actions gated.
13. Update authority map, ADR, docs, plan register, and handoff.
14. Mark DONE only if survivors feel more agentic without becoming unpredictable second controllers of the shelter.

### Agency invariants

- Autonomy selects/proposes intent; canonical systems resolve effects.
- Every action has an inspectable reason trace.
- No autonomous action bypasses resource, duty, relation, skill, or health validation.
- Critical/high-risk actions require explicit policy.
- Save/reload cannot reroll committed decisions.
- Cooldowns and histories are bounded.
- Randomness is deterministic and subordinate to meaningful state.
- Player-facing friction is measured, not assumed acceptable.

### Negative tests

- Autonomy directly changes a protected canonical field.
- The same action awards effects twice after reload.
- Repeated assignment retries reroll a refusal.
- A help action creates or destroys inventory.
- A survivor initiates an unavailable/high-risk task without policy.
- An expression directly changes external faction standing.
- All survivors can permanently deadlock essential shelter work with no defined policy.
- Action history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Reason traces are readable.
- [ ] Authority adapter tests pass.
- [ ] Save/load behavior is covered.
- [ ] Headless behavior matches interactive behavior.
- [ ] Performance/action distribution remains bounded.
- [ ] Player-friction metric is recorded.

---

# 5. Canonical Autonomy Authority Matrix

| Fact / effect | Canonical owner | Autonomy role |
|---|---|---|
| Hunger/fatigue/warmth | Needs | Read trigger only |
| Morale/stress/crisis | Mental-health/morale | Read trigger; submit support/consequence events |
| Affinity/trust/resentment/grief | Relations | Read trigger; submit interaction outcomes |
| Duty assignment | Duty roster | Validate/propose/refuse |
| Task danger/essentiality | Task/duty definition | Read |
| Skill XP | Skill progression | Request training/mentoring activity |
| Inventory/food | Inventory/consumption | Request transfer/consume where policy permits |
| Faction standing | Faction authority | Never direct-write |
| Moral choice | Moral-choice authority | Express/object/react only |
| Quest progress | Quest authority | Outcome event/provenance |
| Personal goal | Autonomy or existing goal authority | Own only if no canonical goal system exists |
| Project/job state | Production/task authority | Submit proposal |
| Dialogue/expression | Dialogue/social presentation | Request/surface |
| Recent autonomy history | Autonomy | Own bounded history |
| Cooldown | Autonomy | Own |
| Reason trace | Autonomy | Own/debug/presentation |
| Player override | Duty/governance authority | Autonomy records outcome only |

---

# 6. Recommended Intent Contract

```yaml
intent_id: "aut_..."
actor_id: "survivor_anna"
family: "REFUSE"
context:
  task_id: "duty_night_patrol"
  target_id: null
generated_day: 42
expires_day: 43

reasons:
  - code: "fatigue_critical"
    source_authority: "Needs"
    severity: 0.93
  - code: "task_danger_high"
    source_authority: "Duty"
    severity: 0.80
  - code: "recent_trauma_match"
    source_authority: "MentalHealth"
    severity: 0.71

score:
  base: 0.20
  state: 0.68
  trait: 0.05
  recent_history: -0.02
  final: 0.91

policy:
  classification: "SOFT_REFUSAL"
  override_allowed: true

status: "AWAITING_PLAYER"
```

This shape is illustrative. Reuse current DTO conventions.

---

# 7. Refusal Taxonomy

## Physically impossible

Examples: unconscious, severe injury, absent from shelter.

Result: hard reject from canonical duty validation. This is not personality autonomy.

## Critical need refusal

Examples: extreme fatigue, hunger, hypothermia risk.

Result: policy-driven hard/soft refusal depending on task essentiality.

## Danger/fear refusal

Examples: recent trauma matching expedition/combat context.

Result: soft/hard refusal according to mental-health and leadership policy.

## Relationship refusal

Examples: will not work paired with a deeply resented survivor.

Result: suggest reassignment; override may carry relationship consequence.

## Moral/value refusal

Examples: task violates a survivor's established moral stance.

Result: explicit objection/refusal; player choice may matter.

## Preference objection

Examples: dislikes duty but remains capable.

Result: expression/warning, not necessarily refusal.

## Protest/collective action

Deferred to governance/rebellion follow-on.

---

# 8. Player Override Contract

Override should never mean “ignore autonomy boolean.”

```text
Refusal returned by duty validation
      |
      +--> Accept refusal
      |      -> assignment cancelled/reassigned
      |
      +--> Negotiate / choose alternative
      |      -> canonical new assignment attempt
      |
      +--> Override (if policy allows)
             -> governance/duty command
             -> canonical consequences
             -> assignment becomes active or still fails
             -> autonomy records outcome
```

The consequence may include resentment, morale, trust, grievance, or discipline, but those values are owned by
their systems.

---

# 9. Candidate Generation Strategy

Avoid daily dice rolls over every possible behavior.

Preferred triggers:

- Need threshold crossed.
- Relationship threshold crossed.
- A relevant event was witnessed.
- Duty assignment attempted.
- Free-time window opened.
- Goal became actionable.
- Target became distressed.
- Survivor gained/changed skill.
- Project/task became understaffed.
- Player made moral choice.
- Memorial/death/grief event occurred.

Each trigger produces a small bounded candidate set.

---

# 10. Explainability Requirements

For every selected action, debugging should answer:

1. Why this survivor?
2. Why now?
3. Why this action family?
4. Why this tar

<!-- Deliverable capped to remain within the requested 50–90k character envelope. -->
