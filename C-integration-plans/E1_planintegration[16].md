---
PLAN_ID: E1-16
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 16
STATUS: READY_FOR_EXECUTION_WHEN_RELATION_AND_INTERACTION_RAILS_PASS
SOURCE_PLAN: "Plan 182 — Relationship Decay & Drift System"
SEQUENCE_FILENAME: "E1_planintegration[16].md"
PREVIOUS_FILENAME: "E1_planintegration[15].md"
NEXT_FILENAMES:
  - "E1_planintegration[17].md"
  - "E1_planintegration[18].md"
CATEGORY: LINK+RELATIONSHIPS+SOCIAL_MEMORY+DRIFT+PRESENTATION
PRIMARY_INTENT: "Add gradual relationship maintenance, neglect drift, and reconciliation by applying deterministic temporal policies to canonical SurvivorRelations state using real interaction evidence, without creating a second relationship ledger, social scheduler, morale system, or memory system."
EXECUTION_STYLE: "Flagship integration plan"
PREMISE_VERIFICATION_REQUIRED: true
INTAKE_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
SECOND_RELATIONSHIP_LEDGER_FORBIDDEN: true
SECOND_SOCIAL_EVENT_BUS_FORBIDDEN: true
SECOND_MORALE_SYSTEM_FORBIDDEN: true
SECOND_MEMORY_SYSTEM_FORBIDDEN: true
RUNTIME_RISK: MEDIUM_HIGH
SAVE_RISK: HIGH
BALANCE_RISK: HIGH
PLAYER_FRICTION_RISK: HIGH
PERFORMANCE_RISK: MEDIUM
---

# E1 Plan Integration [16] — Relationship Decay, Social Drift, Maintenance, Reconciliation, and Long-Term Bond Dynamics

> **Sequence rule:** this file is `E1_planintegration[16].md`.
> The next files are `E1_planintegration[17].md`, `E1_planintegration[18].md`, and so on.
> The bracketed sequence number remains immediately before `.md`.

## 0. Mission

This plan converts Plan 182 into an implementation-grade relationship-drift programme.

The source plan identifies a real long-horizon social gap: `SurvivorRelationsSystem` already tracks affinity,
trust, resentment, grief, and bond type, but those values apparently change only through explicit events.
Once two survivors become close, the relationship can remain frozen indefinitely even if they are separated
for months and never share work, rooms, conversation, events, or support.

That problem should be solved without creating a second relationship authority.

The correct architecture is:

**canonical relationships own affinity/trust/resentment/bond identity; a decay/drift policy consumes elapsed
time plus verified interaction evidence and submits bounded relationship-change commands back to that same
authority.**

The drift layer may own:

- last meaningful-interaction markers where no canonical interaction-history authority exists;
- decay evaluation receipts/cursors;
- drift-warning state;
- reconciliation eligibility/provenance;
- bounded drift-history references;
- tuning definitions.

It must not own copied affinity, trust, resentment, grief, or bond type.

It also should not assume that “no interaction” means a relationship must steadily lose points every day.
Strong friendships, family bonds, shared history, grief, trust, and long separations can behave differently.
The system should model **maintenance pressure and relationship drift**, not a universal daily tax.

## 1. Source Intent Preserved

Plan 182 asks for:

- time-based relationship decay;
- different decay rates by bond type;
- interaction tracking;
- shared work and roommate effects;
- conversation/gift/shared-event effects;
- trust erosion;
- resentment changes;
- drift warnings/events;
- bond dissolution;
- reconciliation;
- UI warnings/history;
- quest/event hooks;
- deterministic behavior;
- old-save compatibility;
- integration with Duty, ShelterAssignment, IdeologicalFriction, Needs, and Psychology.

E1-16 preserves those goals but changes several assumptions:

- no RNG is required for ordinary deterministic decay;
- “resentment from neglect” is not automatic;
- interactions do not automatically grant affinity simply because two survivors shared a room or shift;
- family bonds may change form without “breaking at 0 affinity”;
- bond type is not inferred purely from affinity;
- relationship maintenance should rely on meaningful contact, not exploitably farming repeated micro-events;
- all relation-value mutations happen through `SurvivorRelationsSystem`.

## 2. Core Architecture Thesis

```text
Canonical interaction evidence
    |
    +--> social event / conversation
    +--> shared work
    +--> cohabitation / roommate context
    +--> gift
    +--> shared crisis/event
    +--> support/care
    +--> conflict
    +--> ideological friction
    +--> separation / absence
    |
    v
Relationship maintenance evaluator
    |
    +--> time since meaningful contact
    +--> relationship bond type
    +--> current affinity/trust/resentment
    +--> separation context
    +--> interaction diversity/quality
    +--> strong-bond protections
    +--> conflict modifiers
    |
    v
Drift policy decision
    |
    +--> no change
    +--> soft affinity drift
    +--> trust erosion
    +--> bond-state review
    +--> reconciliation opportunity
    +--> warning/event
    |
    v
SurvivorRelationsSystem
    |
    +--> applies canonical relationship changes
    +--> reclassifies bond if its own policy permits
```

The drift system is a policy consumer and scheduler. Relations remain the source of truth.

## 3. Non-Negotiable Rules

- `SurvivorRelationsSystem` owns affinity.
- `SurvivorRelationsSystem` owns trust.
- `SurvivorRelationsSystem` owns resentment.
- `SurvivorRelationsSystem` owns grief.
- `SurvivorRelationsSystem` owns bond type or delegates bond classification through its canonical policy.
- RelationshipDecay state must not copy current relationship values.
- Ordinary drift should be deterministic; RNG is unnecessary unless a later authored event explicitly needs it.
- Last-interaction tracking must use stable unordered pair IDs.
- Interactions are semantic events, not audio/UI events.
- Shared work does not automatically equal positive interaction.
- Being roommates does not automatically produce affinity each day.
- Conflict does not automatically reduce resentment merely because conflict occurred.
- Neglect does not automatically create resentment.
- Strong trust can persist through separation longer than surface affinity.
- Family bonds should not necessarily dissolve at affinity 0.
- Rival/enemy relationships require separate drift semantics; hostility may fade rather than invert to friendship.
- Grief should not decay through the same relation-maintenance formula unless Relations explicitly models it.
- Ideological friction is context from E1-7/Plan 148, not a direct hidden multiplier stored locally.
- Psychology/E1-15 may influence interaction/withdrawal context, but drift must not duplicate phobia, mood, or trauma state.
- No daily O(N²) full survivor-pair scan if population can grow materially.
- Only active/meaningful relationships require scheduled evaluation.
- Relationship changes are applied through canonical relation commands/events with provenance.
- Save/load cannot double-apply daily decay.
- Offline/wall-clock time never advances relationship decay.
- Time skip uses campaign time exactly once.
- Old saves do not receive retroactive months of relationship decay on first load.
- Relationship maintenance must not become a mandatory micromanagement chore.
- Player UI must show broad risk/reason, not hidden exact decay equations unless debug mode is enabled.
- Reconciliation is an opportunity/process, not an automatic +affinity button.
- The first release should support a small subset of bond types and interaction evidence before broad content expansion.

## 4. Acceptance Slices

### Slice A — Temporal inactivity tracking
Track meaningful contact and evaluate drift for a small set of existing positive bonds.

### Slice B — Bounded affinity/trust drift
Apply gradual canonical relation changes with strong-bond and family protections.

### Slice C — Meaningful maintenance evidence
Shared supportive events, conversations, cohabitation, and work interactions alter maintenance state only when
there is real semantic evidence.

### Slice D — Warnings and reconciliation
Surface “growing apart” and create real reconnection opportunities.

### Slice E — Broader bond semantics
Rivalry decay, family dynamics, ideology context, psychology context, quests, and long-run content.

Do not start by applying a daily decay equation to every survivor pair.


---

## E1-16A — Premise verification and relationship-authority audit

**Goal:** Verify relation ownership, interaction sources, bond classification, time/calendar, housing, duty, social events, gifts, ideology, psychology, and save rails before creating drift state.

### Required substeps

1. Inspect `SurvivorRelationsSystem`, all relationship DTOs, bond classification, event APIs, relationship save sections, pair-key conventions, and downstream consumers.
2. Inspect `NeedsSystem`, `DutyRosterSystem`, `ShelterAssignmentSystem`, social-event/conversation systems, gift/item transfer events, shared encounter/event systems, E1-7 ideological friction integration, E1-15 psychology integration, family/romance systems, memory systems, and campaign calendar.
3. Verify whether affinity/trust/resentment/grief are independently mutable and whether relation commands already carry reason/provenance.
4. Verify whether bond type is a stored fact or derived from relation values.
5. Search for existing interaction timestamps, recent-contact caches, social history, cohabitation history, shared-duty history, or relation-decay prototypes.
6. Verify whether pair IDs are ordered or unordered and establish one canonical pair-key helper.
7. Verify population scale and maximum active relationship count.
8. Verify whether Plan 147 memory already records shared interactions that can be reused.
9. Create `docs/systems/RELATIONSHIP_DRIFT_AUTHORITY_MAP.md`.
10. Create intake duplicate-search evidence linking Plans 147, 148/E1-7, 150, 179/E1-15, family/romance, and social-event rails.
11. Set `PREMISE_VERIFIED_AT` to current HEAD.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16B — Relationship drift ADR

**Goal:** Define drift as a temporal policy over canonical relationship facts rather than a parallel relation system.

### Required substeps

1. Write an ADR comparing a dedicated `RelationshipDecaySystem`, an extension of `SurvivorRelationsSystem`, and a generic relationship-maintenance policy service.
2. Prefer placing relation-value mutation inside `SurvivorRelationsSystem` even if scheduling/evidence tracking is separate.
3. Define drift-owned state narrowly: last meaningful contact markers, next evaluation day/cursor, drift-warning state, reconciliation opportunity IDs, and bounded event receipts.
4. Explicitly exclude affinity, trust, resentment, grief, bond type, current morale, housing assignment, duty assignment, ideology, and psychology.
5. Define source interaction adapters.
6. Define evaluation cadence.
7. Define rollback/feature flag.
8. Require second-tool architecture review.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16C — Canonical pair identity

**Goal:** Guarantee all interaction/drift state addresses the same survivor pair independent of ordering.

### Required substeps

1. Define `RelationshipPairKey(minId,maxId)` or canonical equivalent.
2. Use canonical survivor stable IDs.
3. Reject self-pairs.
4. Handle missing/dead/departed survivor references.
5. Reuse Relations' pair-key implementation if it exists.
6. Use pair key in last-contact state, drift receipts, cooldowns, and reconciliation opportunities.
7. Add serialization and equality tests.
8. Add tests for A-B versus B-A.
9. Document no pair-key duplication across systems.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16D — Meaningful interaction evidence contract

**Goal:** Represent social maintenance evidence semantically without creating a second full interaction history.

### Required substeps

1. Define stable interaction evidence ID, pair key, semantic type, source system/event ID, day/time, valence, intensity band, maintenance weight/class, and optional context tags.
2. Do not persist raw affinity delta as the interaction itself.
3. Do not copy full event payload.
4. Define positive, neutral, negative, and mixed interactions.
5. Define maintenance-relevant versus incidental co-presence.
6. Use source provenance for dedupe.
7. Bound retained detail.
8. Add fixtures for conversation, support, shared work, cohabitation, gift, shared crisis, conflict, and ideological dispute.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16E — Interaction source adapter registry

**Goal:** Consume interaction evidence from canonical systems rather than polling everything manually.

### Required substeps

1. Create source adapters for conversation/social events.
2. Create DutyRoster adapter for actual meaningful shared work where the work system can identify collaborative context.
3. Create ShelterAssignment adapter for cohabitation/proximity context without assuming affinity gain.
4. Create gift adapter from canonical item-giving/social event.
5. Create shared-event adapter for experiences involving both survivors.
6. Create conflict adapter.
7. Create ideological-friction adapter from E1-7 if enabled.
8. Create psychology/social-withdrawal context adapter only if E1-15 exposes it.
9. All adapters are read-only toward source systems.
10. Add contract tests for each adapter.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16F — Last meaningful contact state

**Goal:** Track when a relationship last received maintenance-relevant contact without storing a giant social log.

### Required substeps

1. Store pair key, last meaningful interaction day/time, last interaction class, and optionally last positive-contact day if required by policy.
2. Do not reset neglect on hostile or irrelevant co-presence unless policy says so.
3. Define contact semantics per bond type.
4. Update idempotently from evidence.
5. Use game time only.
6. Persist state where needed.
7. Add tests for positive, negative, neutral, duplicate, and out-of-order evidence.
8. Define initialization for newly formed bonds.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16G — Decay policy catalog

**Goal:** Define bond-specific maintenance policy in data without duplicating relation definitions.

### Required substeps

1. Define policy ID/bond type reference, grace period, evaluation interval, affinity drift policy, trust drift policy, resentment policy if any, strong-bond protection, separation modifier, reconnection threshold, warning threshold, floor/ceiling, and applicability tags.
2. Do not store relation values in the catalog.
3. Do not assume one universal daily rate.
4. Define policies for friend, lover/partner, family, mentor, rival only if those bond types truly exist.
5. Validate bond references.
6. Validate finite/nonnegative rates and ordered thresholds.
7. Version catalog semantics.
8. Start with friend + one close-bond type before all bond categories.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16H — Deterministic drift evaluation

**Goal:** Calculate drift from elapsed campaign time and current canonical relationship state without RNG.

### Required substeps

1. Query current relation state.
2. Compute elapsed time since meaningful maintenance.
3. Apply grace period.
4. Apply bond policy.
5. Apply current strong-bond/trust protection.
6. Apply separation/context modifiers only from canonical state.
7. Return a typed drift proposal.
8. Do not mutate Relations directly inside the calculator.
9. Use deterministic rounding.
10. Add golden tests at exact boundaries.
11. Prove call-order independence.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16I — Evaluation cadence and scheduler

**Goal:** Avoid per-frame or naive daily all-pair scans.

### Required substeps

1. Determine active relationship count.
2. Index only pairs with relevant bonds or nontrivial relation state.
3. Schedule next evaluation day per pair or process a compact dirty/due set.
4. Use campaign-day boundary/event scheduler.
5. Handle time skips by calculating elapsed time once rather than replaying every day if mathematically equivalent.
6. Do not evaluate unrelated neutral pairs.
7. Remove/archive pairs when survivors permanently leave/death policy says no further drift.
8. Add scale tests.
9. Document O(active relationships) or better cadence.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16J — Affinity drift adapter

**Goal:** Apply bounded neglect-related affinity changes through `SurvivorRelationsSystem` only.

### Required substeps

1. Define canonical reason code such as `relationship_neglect_drift`.
2. Submit relation change command with stable drift evaluation ID.
3. Clamp through Relations' own limits.
4. Do not directly set affinity field.
5. Do not decay below a policy floor if the bond semantics require retained history.
6. Use stronger protection for very strong established bonds if approved.
7. Add duplicate command/idempotency tests.
8. Add save/reload tests.
9. Measure long-term trajectories.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16K — Trust erosion policy

**Goal:** Model trust separately and more conservatively than surface closeness.

### Required substeps

1. Verify trust semantics in Relations.
2. Define longer grace period/slower rate than affinity by default only if supported by design.
3. Do not erode trust merely because two survivors sleep in different rooms.
4. Consider trust decay only after prolonged absence, unresolved abandonment, broken commitments, or meaningful separation if those contexts exist.
5. Use canonical relation command.
6. Do not drive trust to zero automatically in every friendship.
7. Add tests for long separation, regular contact, betrayal context, family bond, and save/load.
8. Show reason trace.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16L — Resentment boundary

**Goal:** Reject automatic resentment growth from simple neglect unless evidence supports a grievance.

### Required substeps

1. Verify resentment semantics.
2. Treat ordinary drifting apart as reduced affinity/relationship salience, not hostility.
3. Increase resentment only from authored grievance contexts: ignored request, abandonment, favoritism, broken promise, hostile conflict, coercion, or similar canonical events.
4. Relationship drift may keep unresolved resentment from decaying or surface a grievance, but should not invent one.
5. Use Relations authority.
6. Add negative test proving no-contact alone does not necessarily create resentment.
7. Add tests with genuine abandonment/grievance.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16M — Grief boundary

**Goal:** Keep grief outside ordinary social-maintenance decay unless the canonical relation system explicitly couples them.

### Required substeps

1. Audit grief ownership and decay.
2. Do not reduce grief because survivors did not interact before a death.
3. Do not treat grief as a living relationship maintenance value.
4. Allow prior relationship history to influence grief only through existing relation/mental-health policy.
5. Add boundary tests.
6. Document exclusion.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16N — Bond classification and dissolution policy

**Goal:** Define how canonical bond types change without assuming `affinity == 0 -> bond deleted`.

### Required substeps

1. Audit bond derivation/storage semantics.
2. For friendship, allow close_friend -> friend -> acquaintance/no special bond if canonical policy supports tiers.
3. For romantic/partner bonds, require relationship-specific breakup/commitment rules rather than passive affinity threshold alone.
4. For family, preserve kinship identity even if affection/trust deteriorates.
5. For mentor bonds, allow active mentorship to end while history remains.
6. For rivalry, hostility may fade toward neutrality.
7. Use Relations/family/romance authorities for reclassification.
8. Add tests for each supported bond category.
9. Do not erase historical relationship provenance.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16O — Strong-bond memory protection

**Goal:** Allow deep bonds to be more stable without creating a separate permanent-bond stat.

### Required substeps

1. Use current affinity/trust/bond duration/shared-history summary if canonical systems expose them.
2. Define diminishing decay for established bonds.
3. Do not make high affinity completely immune forever unless bond policy says so.
4. Consider E1-147 memory/shared-event history only through a read-only summary.
5. Keep calculation deterministic.
6. Add tests for new friend versus ten-year/long-campaign close friend abstraction.
7. Document cap/floor behavior.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16P — Cohabitation/proximity policy

**Goal:** Treat roommates as increased opportunity for interaction, not automatic daily affinity.

### Required substeps

1. Audit room assignment and social-event generation.
2. Use cohabitation as decay-protection context or interaction-opportunity multiplier.
3. Require actual positive/neutral social interaction for affinity gain where possible.
4. Allow hostile roommates to worsen conflict through existing social systems, not a positive roommate bonus.
5. Define separation transition when roommates move apart.
6. Add tests for friendly roommates, hostile roommates, no conversation, room separation, and overcrowding.
7. Keep Housing authority canonical.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16Q — Shared-work policy

**Goal:** Treat shared duties as interaction opportunities with context-dependent valence.

### Required substeps

1. Audit DutyRoster and job outcome events.
2. Do not grant affinity just because pair IDs appear on the same task.
3. Use successful collaboration/support/conflict evidence where available.
4. If only co-assignment data exists, use it as reduced neglect rather than positive affinity.
5. Allow dangerous/shared hardship events to generate stronger maintenance evidence through Event/Relations.
6. Add tests for cooperative success, routine co-assignment, conflict at work, separate duties, and save/load.
7. Keep duty assignments canonical.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16R — Conversation and social-event integration

**Goal:** Use explicit social interaction as the strongest ordinary maintenance evidence.

### Required substeps

1. Audit social/conversation event authority.
2. Map positive conversation, shared leisure, emotional support, disagreement, and conflict to semantic evidence.
3. Use stable source event IDs.
4. Allow conversation to reset maintenance timer.
5. Do not automatically restore fixed affinity every conversation.
6. Route explicit relation gains/losses through the social event's existing relation policies.
7. Drift layer should avoid double-applying gains.
8. Add dedupe tests.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16S — Gift integration

**Goal:** Prevent gift spam from becoming a relationship-maintenance exploit.

### Required substeps

1. Audit gift transaction/event system.
2. Use gift as maintenance evidence only when socially meaningful.
3. Cap/diminish repeated gifts in short windows.
4. Do not compute affinity solely from market value.
5. Respect recipient preferences/relationship context where existing systems support it.
6. Let canonical gift/social system own explicit affinity change.
7. Drift system only updates maintenance context unless no owner exists.
8. Add tests for repeated cheap gifts, valuable unwanted gift, meaningful gift, and duplicate transaction.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16T — Shared-event and hardship bonding

**Goal:** Use real shared experiences as longitudinal relationship maintenance evidence.

### Required substeps

1. Audit event participant lists.
2. Classify supportive shared outcome, neutral co-presence, conflict, rescue, loss, crisis, expedition survival, or communal success.
3. Use canonical event result to determine valence.
4. Do not award positive bonding for every shared disaster automatically.
5. Use source event provenance.
6. Allow strong positive events to reset drift and Relations to handle explicit affinity/trust changes.
7. Add tests for rescue, failed expedition, shared loss, and conflict.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16U — Ideological friction integration

**Goal:** Use E1-7 ideological conflict as one contextual contributor, not a hidden drift multiplier applied twice.

### Required substeps

1. Consume resolved/sustained friction evidence.
2. If E1-7 already changes relationships, do not apply the same effect again.
3. Drift policy may shorten maintenance grace period or increase risk of bond reclassification only if explicit and justified.
4. Successful mediation may count as meaningful contact.
5. Use provenance to dedupe.
6. Add tests for ongoing disagreement, respectful debate, mediated conflict, and duplicate relation consequence prevention.
7. Keep ideology state external.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16V — Psychology integration

**Goal:** Allow withdrawal, phobia, coping, therapy, and trauma context to shape interaction opportunity without making drift a mental-health engine.

### Required substeps

1. Consume E1-15 read-only profile/context where implemented.
2. Psychological withdrawal may reduce interaction opportunities through Autonomy, not directly increase decay rate secretly.
3. Supportive relationships may become recovery opportunities.
4. Relationship loss/drift may emit a stressor to MentalHealth through canonical policy.
5. Do not copy profile state.
6. Do not diagnose psychology from relationship decay.
7. Add tests with psychology enabled/disabled.
8. Use provenance to prevent double stressors.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16W — Needs/morale consequence boundary

**Goal:** Route meaningful relationship loss through canonical Needs/MentalHealth, not a daily neglect penalty.

### Required substeps

1. Define only significant milestones as stressor/support events.
2. Examples: close friendship downgrades, breakup, reconciliation, major abandonment.
3. Do not apply morale penalty for every tiny affinity drift.
4. Use canonical consequence policies.
5. Use stable event IDs.
6. Add tests for warning-only drift, bond downgrade, reconciliation, and dedupe.
7. Measure event frequency.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16X — Autonomy and help/cooperation boundary

**Goal:** Let changing relationships influence willingness to help through existing autonomy/social systems.

### Required substeps

1. Expose current canonical relation values and drift-status context to E1-6 Autonomy if it consumes relations.
2. Do not implement a separate cooperation probability inside drift system.
3. Do not directly refuse tasks because trust eroded.
4. Let Autonomy/Duty decide behavior.
5. Add tests for high trust, eroded trust, reconciliation, and disabled autonomy.
6. Document dependency direction.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16Y — Drift warning state

**Goal:** Warn before meaningful bond downgrade without persisting duplicate relation values.

### Required substeps

1. Define warning based on time-since-contact + canonical relation trajectory/policy.
2. Store warning receipt/state only to prevent spam.
3. Do not expose exact hidden decay rate by default.
4. Show actionable reason: little recent contact, prolonged separation, unresolved conflict.
5. Clear/update warning after meaningful interaction or relation change.
6. Add tests for warning, recovery, repeated warning cooldown, and bond downgrade.
7. Use UI read model.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16Z — Drift milestone events

**Goal:** Emit narrative events only at meaningful state transitions.

### Required substeps

1. Define `RelationshipAtRisk`, `BondDowngraded`, `GrewApart`, `TrustErodedSignificantly`, `Reconnected`, `RivalryFaded` as appropriate.
2. Use canonical relation transition result.
3. Do not emit event for every daily drift adjustment.
4. Use stable event IDs.
5. Journal/Quest/UI subscribe.
6. Add save/reload dedupe tests.
7. Keep event volume bounded.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16AA — Reconciliation opportunity model

**Goal:** Create real opportunities for former friends/strained bonds to reconnect without instant restoration.

### Required substeps

1. Define eligibility from current canonical relation state, prior bond history, absence of severe unresolved grievance, and recent positive contact.
2. Generate opportunity through social/autonomy/event system.
3. Do not directly restore affinity from drift layer.
4. Allow no reconciliation outcome.
5. Allow partial reconnection.
6. Respect survivor agency.
7. Use stable opportunity ID.
8. Add tests for eligible former friends, active hostility, one-sided refusal, successful partial reconnection, and save/load.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16AB — Reconnection consequence contract

**Goal:** Route reconciliation outcomes through Relations and MentalHealth.

### Required substeps

1. Canonical social event resolves conversation/action.
2. Relations owns affinity/trust/resentment changes.
3. MentalHealth may receive support/closure event.
4. Bond classification updates through canonical relation policy.
5. Drift timer/contact state updates from the real interaction.
6. Do not reset relationship to old peak automatically.
7. Add tests for partial, full, failed, and repeated reconciliation.
8. Preserve historical bond record.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16AC — Relationship memory and historical salience boundary

**Goal:** Reuse Plan 147/shared-memory history if available without making decay own full social memory.

### Required substeps

1. Audit per-NPC memory implementation.
2. Read summaries such as major shared events or past bond milestones where useful.
3. Do not duplicate memory entries.
4. Use history to protect deep bonds or inform reconciliation only through bounded policy.
5. Define behavior if memory system absent.
6. Add tests with/without memory integration.
7. Keep relationship current state in Relations.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16AD — Old-save migration

**Goal:** Introduce maintenance prospectively without punishing players for interactions that were never tracked.

### Required substeps

1. Do not calculate `lastInteractionDay = campaign start` and immediately apply months of decay.
2. For existing relationships, initialize maintenance anchor to migration/load day or a conservative recent-contact baseline.
3. Preserve all canonical relation values.
4. Create no retroactive drift events.
5. Default warning receipts empty.
6. Version decay state.
7. Add fixtures for early/late saves, many close bonds, rivals, family, romance, and missing relation entries.
8. Make migration idempotent.
9. Begin decay prospectively.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16AE — Save contract and day-cursor integrity

**Goal:** Persist only tracking/receipt state necessary to avoid double application.

### Required substeps

1. Persist pair maintenance anchor/last meaningful contact where not derivable.
2. Persist last evaluated day or per-pair next evaluation marker.
3. Persist emitted warning/milestone receipts where needed.
4. Persist reconciliation opportunity IDs if active.
5. Do not persist copied relation values.
6. Use schema version.
7. Restore relations before validating pair tracking.
8. Handle deleted survivors/bonds.
9. Add boundary save/load tests.
10. Ensure day tick cannot apply twice.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16AF — Time-skip semantics

**Goal:** Apply elapsed drift exactly once across multi-day skips.

### Required substeps

1. Audit CampaignCalendar time advancement.
2. If policy is linear/piecewise deterministic, integrate elapsed duration mathematically instead of looping every day.
3. Respect interaction events inside the skipped interval if simulation still generates them.
4. Do not use wall-clock elapsed time.
5. Handle crossing grace/warning/bond thresholds.
6. Emit at most the appropriate milestone transitions once.
7. Add 1-day, 7-day, 30-day, and 100-day skip tests.
8. Compare stepped versus bulk-equivalent results.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16AG — No-RNG baseline

**Goal:** Remove unnecessary seeded randomness from ordinary relationship decay.

### Required substeps

1. Define decay as deterministic from canonical relation/context/time.
2. Do not use `ISeededRng` merely because the source plan mentions it.
3. Only authored reconciliation/social events may use their own deterministic event RNG if already supported.
4. Add test proving same inputs always produce same drift proposal.
5. Document zero-RNG policy.
6. Prevent RNG-call-order changes to unrelated systems.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes through canonical social/Relations systems.
- [ ] Old-save migration is prospective.
- [ ] Performance and player-burden metrics are captured.

---

## E1-16AH — Bond-type policy specifics

**Goal:** Define distinct semantics for each actually supported relationship category.

### Required substeps

1. Friends: closeness can fade through neglect.
2. Close friends: longer grace/strong-history protection.
3. Partners/lovers: passive drift may signal strain but breakup follows romance/commitment authority.
4. Family: kinship persists; affection/trust can change.
5. Mentor/mentee: active mentorship may end while respect/history persists.
6. Rivals: hostility may fade with separation rather than accumulate.
7. Enemies if present: no-contact may reduce salience/resentment only through relation policy.
8. Validate each against actual bond catalog.
9. Do not author policy for nonexistent bond types.
10. Add type-specific tests.

### Relationship-drift invariants

- `SurvivorRelationsSystem` remains the sole owner of affinity, trust, resentment, grief, and bond truth.
- Drift tracks time/evidence and proposes canonical relation changes; it does not maintain copied relation values.
- Ordinary drift is deterministic and does not require RNG.
- No-contact alone cannot automatically turn affection into hostility.
- Shared room/work is opportunity/context, not guaranteed positive affinity.
- Bond categories have distinct semantics; family identity is not erased by low affinity.
- Save/load/time skip applies each drift interval exactly once.
- Maintenance must remain strategically meaningful without becoming routine micromanagement.

### Negative tests

- RelationshipDecayState stores current affinity/trust/resentment.
- A roommate bonus grants affinity every day with no meaningful interaction.
- A shared-work assignment automatically grants positive affinity.
- Neglect alone increases resentment for every pair.
- Family bond disappears because affinity reaches zero.
- Reload at day boundary applies drift twice.
- A 30-day time skip differs from equivalent stepped evaluation without intervening events.
- The scheduler evaluates all possible survivor pairs every frame/day unnecessarily.

### Acceptance evidence

- [ ] Relations authority-boundary tests pass.
- [ ] Interaction evidence/dedupe tests pass.
- [ ] Time-skip/save idempotency tests pass.
- [ ] Bond-specific trajectory tests pass.
- [ ] Reconciliation routes

<!-- Deliverable capped to remain within the requested 50–90k character envelope. -->
