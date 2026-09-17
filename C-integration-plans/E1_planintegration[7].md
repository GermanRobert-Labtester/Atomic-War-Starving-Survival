---
PLAN_ID: E1-7
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 7
STATUS: READY_FOR_EXECUTION_WHEN_RAILS_PASS
SOURCE_PLAN: "Plan 148 — Ideological Friction → Events & Quests"
SEQUENCE_FILENAME: "E1_planintegration[7].md"
PREVIOUS_FILENAME: "E1_planintegration[6].md"
NEXT_FILENAMES:
  - "E1_planintegration[8].md"
  - "E1_planintegration[9].md"
CATEGORY: LINK+SOCIAL_EVENTS+BELIEF_CONFLICT+QUESTS+PRESENTATION
PRIMARY_INTENT: "Turn existing ideological-friction signals into explainable social events, mediation choices, bounded belief evolution, and quest hooks without duplicating relations, autonomy, governance, faction, or moral-choice authority."
EXECUTION_STYLE: "Flagship integration plan"
PREMISE_VERIFICATION_REQUIRED: true
INTAKE_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
BELIEF_CHANGE_REQUIRES_EXPLICIT_MODEL: true
PLAYER_MEDIATION_EXPLICIT: true
MASS_FACTION_SPLIT_GATED: true
RUNTIME_RISK: HIGH
SAVE_RISK: MEDIUM_HIGH
NARRATIVE_RISK: HIGH
PLAYER_FRICTION_RISK: HIGH
---

# E1 Plan Integration [7] — Ideological Friction, Confrontation Events, Mediation, Belief Evolution, and Shelter Schisms

> **Sequence rule:** this file is `E1_planintegration[7].md`.
> The next files are `E1_planintegration[8].md`, `E1_planintegration[9].md`, and so on.
> The bracketed sequence number remains immediately before `.md`.

## 0. Mission

This plan expands Plan 148 into an implementation-grade ideological-friction integration programme.

The source plan identifies a valuable disconnected rail: `IdeologicalFrictionSystem` already computes belief
conflict and relationship/sleep consequences, and exposes events such as `OnFrictionDetected` and
`OnAffinityChanged`, but the resulting friction does not become player-facing narrative, mediation,
conversion pressure, shelter-level polarization, or quests.

The correct fix is not to create an independent ideological simulator that directly owns relationships,
morale, belief identity, faction membership, duties, crises, quests, and shelter efficiency. The feature
should instead translate existing friction facts into **candidate social situations**, allow survivors and
the player to respond through existing authorities, and record bounded narrative consequences.

The core flow is:

```text
canonical belief + relation + duty + need state
        |
        v
IdeologicalFrictionSystem detects sustained incompatibility
        |
        v
Friction-event evaluator builds candidate event
        |
        +--> confrontation
        +--> mediation/debate
        +--> persuasion/conversion attempt
        +--> personal crisis/questioning
        +--> group polarization warning
        |
        v
player/survivor choices
        |
        v
canonical authorities resolve:
relations / morale / autonomy / duty / moral choice / quests / governance
        |
        v
bounded history + UI/journal + future eligibility
```

The foundational release should emphasize confrontations, mediation, belief expression, gradual personal
change, and small-group tensions. Shelter-wide ideological factions, suppression policies, purges,
rebellion, and mass schisms are higher-order governance features and must be gated behind proven individual
and small-group rails.

## 1. Source Intent Preserved

Plan 148 asks for:

- ideological confrontation events;
- conversion attempts;
- ideological splits;
- friction-triggered quests;
- belief-specific event content;
- belief shifts;
- informal factions;
- mediation choices;
- journal/UI surfaces;
- deterministic event selection;
- cooldowns;
- old-save compatibility;
- data-driven event templates;
- cross-system integration with relations, moral choice, duty roster, mental health, and shelter systems.

E1-7 preserves those goals but restructures them into authority-safe slices and introduces explicit
constraints around belief mutation, coercion, mass group formation, and repetitive event generation.

## 2. Core Architecture Thesis

There are four distinct concepts:

1. **Belief identity** — survivor worldview/profile; owned by the canonical survivor/belief model.
2. **Friction state** — incompatibility and accumulated tension between beliefs/people; owned by friction and
   relationship authorities.
3. **Narrative event state** — which confrontation/mediation situation is currently active; owned by the
   friction-event layer.
4. **Consequences** — relation, morale, duty, moral, mental-health, quest, or governance changes; owned by
   their canonical systems.

The event layer may decide *what situation is presented*. It must not directly become the source of truth for
the other three domains.

## 3. Non-Negotiable Rules

- `IdeologicalFrictionSystem` remains the canonical source of friction detection unless the premise audit
  proves otherwise.
- `SurvivorRelationsSystem` remains the canonical owner of affinity/trust/resentment/grief.
- Belief identity changes only through an explicit belief-profile authority and validated transition model.
- A conversion event cannot simply assign `target.belief = source.belief`.
- Moral-choice outcomes remain owned by the moral-choice authority.
- Duty changes remain owned by duty/roster authority.
- Mental-health consequences remain owned by mental-health/crisis authority.
- Quest progress remains owned by quest authority.
- Shelter-wide governance/suppression remains owned by leadership/governance/policy rails.
- An ideological event cannot directly apply arbitrary +20/-30 affinity or -10 morale from JSON.
- Outcome templates reference canonical consequence-policy IDs or commands.
- Event eligibility is state-driven and deterministic; base probability is secondary.
- The same conflict pair/event family must not spam repeatedly.
- Event content must distinguish disagreement, persuasion, coercion, identity crisis, discrimination,
  and collective action rather than flattening all conflict into “conversion.”
- Belief change should be gradual and uncommon unless the game explicitly models sudden shifts.
- The player cannot “convert” a survivor through one button unless the narrative/agency model explicitly
  supports that type of decision.
- Survivors retain agency; mediation choices do not guarantee obedience or belief change.
- No religious/nonreligious or political belief is mechanically treated as inherently “correct” by the core
  system. Templates may depict biased characters, but authority-level scoring remains symmetrical.
- Mass faction formation is deferred until individual event distribution, autonomy, and governance rails are
  stable.
- Deterministic RNG uses `ISeededRng` or the repository's accepted RNG service.
- Old saves default to empty event/cooldown state without fabricating old ideological history.
- Event logs and pair histories are bounded.

## 4. Acceptance Slices

### Slice A — Friction events
Subscribe to friction signals and surface confrontation/complaint/debate events.

### Slice B — Mediation and survivor agency
Player/survivor responses resolve through canonical relation/moral-choice/autonomy systems.

### Slice C — Gradual belief reconsideration
Longitudinal belief-confidence or openness model if and only if a canonical belief authority supports it.

### Slice D — Quest chains
Friction creates optional authored objectives through quest rails.

### Slice E — Group polarization
Informal blocs, demands, shelter-wide schisms, suppression, rebellion — only after governance rails pass.

Do not implement Slice E as part of the first shippable vertical slice.


---

## E1-7A — Premise verification and ideological-authority audit

**Goal:** Verify the actual belief, friction, relationship, autonomy, moral, duty, quest, governance, and mental-health rails before adding event state.

### Required substeps

1. Inspect `IdeologicalFrictionSystem`, all belief-profile DTOs/catalogs, `SurvivorRelationsSystem`, duty roster, autonomy/initiative systems, mental-health/crisis systems, moral-choice system, quest system, leadership/governance/policy systems, journal/event framework, and save sections.
2. Verify the 11 conflict pairs and whether friction is pairwise, room/shift-based, value-axis-based, or profile-ID-based.
3. Verify whether `OnFrictionDetected` and `OnAffinityChanged` truly have no subscribers at current HEAD.
4. Search for existing social confrontation, debate, persuasion, belief-change, faction-bloc, ritual, conversion, or schism code.
5. Verify whether survivor beliefs are mutable today and which authority owns mutation.
6. Verify whether belief strength/confidence/openness is represented or only a categorical profile ID.
7. Verify whether autonomy from E1-6 or equivalent can initiate expressions/refusals that should be reused.
8. Verify whether moral-choice actions already expose mediation/side-taking consequence adapters.
9. Map every proposed Plan 148 outcome to a canonical owner.
10. Create `docs/systems/IDEOLOGICAL_FRICTION_AUTHORITY_MAP.md`.
11. Create intake duplicate-search evidence and plan-register links to Plan 12/30/144 and any live overlapping systems.
12. Set `PREMISE_VERIFIED_AT` to current HEAD.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7B — Friction-event ADR and scope boundary

**Goal:** Define the event layer as a consumer of friction and producer of social situations rather than a second ideology simulator.

### Required substeps

1. Write an ADR comparing event-only adapter, full ideological event system, and extension of an existing social-event framework.
2. Define event-owned state: active event IDs, participant/context references, event-stage state, cooldown/dedupe keys, bounded event history, and optional group-polarization markers if later approved.
3. Explicitly exclude relation values, morale, belief identity, duty schedules, mental-health state, faction standing, shelter resources, and quest progress.
4. Define which events auto-resolve and which require player/survivor choice.
5. Define how survivor autonomy can initiate or refuse participation.
6. Define feature flags per family: confrontation, mediation, persuasion, belief-questioning, quest hook, group split.
7. Define rollback: disable new event generation while preserving resolved history and current canonical state.
8. Require second-tool review before code.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7C — Ideological event DTO and reason-trace model

**Goal:** Represent friction situations as deterministic, inspectable event instances.

### Required substeps

1. Define event ID, template ID, event family, participant IDs, belief/profile references, source friction record/context, generated day/tick, stage, available choices, expiry, cooldown key, status, and outcome provenance.
2. Represent trigger evidence with reason traces such as conflict_pair, affinity_low, sustained_days, shared_room, shared_shift, recent_moral_choice, grief/stress modifier, or positive_cross-belief_affinity.
3. Do not copy full survivor state.
4. Define event statuses: Candidate, Selected, Presented, AwaitingResponse, Resolving, Resolved, Expired, Cancelled.
5. Define deterministic event IDs.
6. Define stable participant ordering for pair events.
7. Bound reason-trace length.
8. Add fixtures for confrontation, debate, persuasion, identity-questioning, mediation, and group-warning.
9. Add validation for missing survivors/beliefs/context.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7D — Trigger windows and sustained-friction accumulation

**Goal:** Trigger events from persistent state and threshold crossings rather than daily random checks over every pair.

### Required substeps

1. Subscribe to friction detection/affinity changes if canonical events are stable.
2. Maintain or derive sustained-friction duration only if not already available.
3. Define trigger windows for conflict severity, positive cross-belief affinity, repeated co-assignment, recent moral disputes, and group composition.
4. Use hysteresis so a pair near a threshold does not repeatedly enter/exit event eligibility.
5. Generate confrontation candidates when high friction is sustained.
6. Generate mediation candidates when conflict affects duties/housing or reaches an accepted threshold.
7. Generate persuasion/understanding candidates from sustained positive contact, not simply affinity > 30.
8. Generate identity-questioning events from internal experiences/memories if the belief model supports them.
9. Generate group-polarization warnings only after several distinct pair conflicts, not merely population counts.
10. Add tests for threshold crossing, oscillation, sustained duration, reset, and unrelated belief pairs.
11. Avoid all-pairs scanning each simulation tick.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7E — Event scoring and deterministic arbitration

**Goal:** Select which ideological situation surfaces without allowing every eligible pair to fire simultaneously.

### Required substeps

1. Assign base priority by event family.
2. Score using friction severity, duration, participant relevance, recent event history, duty/housing impact, current shelter crisis load, narrative diversity, and cooldown.
3. Use deterministic RNG only for tie-breaking/variety among similarly suitable templates.
4. Cap active ideological events globally and per survivor.
5. Prevent the same survivor from being in incompatible simultaneous ideological scenes.
6. Suppress low-value events during critical shelter emergencies unless the event is directly relevant.
7. Record score components in debug traces.
8. Add same-seed determinism tests.
9. Add tests proving severe unresolved conflict outranks cosmetic debate.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7F — Pair/context cooldowns and repetition control

**Goal:** Prevent ideological conflict from becoming repetitive log spam.

### Required substeps

1. Define cooldown by participant pair + conflict pair + event family.
2. Define template-specific repeat limits.
3. Use a recent-theme diversity penalty.
4. Allow major state changes to reopen eligibility before nominal cooldown when justified.
5. Do not suppress canonical relationship/crisis consequences simply because narrative event is cooling down.
6. Persist cooldown expiry if it spans saves.
7. Use game time, not wall clock.
8. Add tests for same pair, different pair, different conflict theme, major state change, save/load, and expiry.
9. Track repetition rate as a tuning metric.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7G — Confrontation event pipeline

**Goal:** Create arguments and disagreements that expose conflict without hard-coding direct relation/morale arithmetic in the event layer.

### Required substeps

1. Define confrontation presentation and participant statements from belief-specific templates.
2. Generate player response options such as hear both sides, support A's requested policy, support B's requested policy, redirect to rules, ask for compromise, or decline involvement where appropriate.
3. Map each response to canonical social/moral/governance consequence policies.
4. Let survivor agency/autonomy determine acceptance of mediation/compromise where applicable.
5. Do not guarantee successful mediation from one skill check without canonical skill/policy support.
6. Resolve relationship effects through relations authority.
7. Resolve morale effects through morale/mental-health authority.
8. Record provenance and event outcome.
9. Add tests for side-taking, neutral mediation, participant refusal, no-valid-choice fallback, and duplicate resolution guard.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7H — Debate and structured mediation events

**Goal:** Support lower-risk ideological disagreement as conversation rather than always escalating to hostility.

### Required substeps

1. Define debate eligibility for conflicting beliefs with adequate trust/affinity and low immediate danger.
2. Let neutral/high-trust survivors volunteer as mediator if E1-6-style autonomy exists.
3. Use existing social/leadership skills for moderation if available.
4. Separate debate outcome from belief conversion.
5. Allow outcomes such as mutual understanding, unchanged disagreement, increased irritation, new personal question, or future quest hook.
6. Route relation/morale effects through canonical authorities.
7. Add cooldown and diversity handling.
8. Add tests for constructive debate, failed debate, absent mediator, participant withdrawal, and no belief mutation.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7I — Persuasion and conversion-attempt contract

**Goal:** Represent attempts to influence belief as consent- and state-aware social interactions rather than deterministic belief assignment.

### Required substeps

1. Audit whether the game has persuasion, influence, ideology confidence, openness, memory, or identity mechanics.
2. Define persuasion as an event intent with source, target, topic, and argument context.
3. Require target agency: accept discussion, reject discussion, engage, counter-argue, or defer.
4. Do not change belief category directly from a single attempt unless the canonical belief model explicitly permits sudden conversion.
5. Record exposure/argument history only if it has a bounded owner.
6. Allow successful persuasion to affect openness/confidence or create a future reconsideration flag before categorical change.
7. Allow counter-persuasion without infinite ping-pong.
8. Add cooldowns and anti-harassment rules.
9. Add tests for refusal to engage, constructive exchange, repeated attempts, target agency, and no direct belief mutation.
10. Keep the source plan's success/fail affinity numbers as rejected hard-coded hypotheses.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7J — Belief confidence/openness model

**Goal:** Introduce gradual belief evolution only if the current categorical profile model cannot express the desired narrative honestly.

### Required substeps

1. First verify whether existing traits/memories already represent certainty, doubt, zeal, openness, or identity crisis.
2. If needed, define bounded confidence/openness metadata under the canonical belief authority, not event state.
3. Keep confidence separate from relation affinity.
4. Define causes of change: repeated experiences, contradictions, trusted relationships, trauma, moral outcomes, rituals, evidence, or personal goals.
5. Use data-driven bounded deltas with provenance.
6. Do not map ideology change to one universal rationality score.
7. Define thresholds for questioning and possible profile transition.
8. Require explicit transition rules between compatible profile states.
9. Add save/migration tests.
10. Allow the first release to ship without mutable belief confidence if the data model is not ready.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7K — Belief transition and identity-change safeguards

**Goal:** Make categorical belief shifts rare, explainable, reversible only where narrative supports it, and owned by the belief authority.

### Required substeps

1. Define transition eligibility from accumulated canonical evidence rather than one event outcome.
2. Require a stable transition event/decision ID.
3. Record previous belief, new belief, reasons, and day for historical/journal purposes.
4. Recompute ideological friction through the canonical friction system after transition.
5. Do not directly rewrite relationship history.
6. Define how previous-group reactions are generated as separate events, not automatic blanket penalties.
7. Prevent repeated oscillation between profiles with cooldown/hysteresis.
8. Add tests for eligible transition, ineligible transition, transition after save/load, friction recalculation, and oscillation prevention.
9. Keep irreversible/permanent language out unless the game's belief model explicitly requires it.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7L — Belief-specific content templates

**Goal:** Use belief-pair-specific authored content without putting gameplay arithmetic into narrative data.

### Required substeps

1. Create template families for the major verified conflict pairs.
2. Use localization-ready dialogue keys and participant-role placeholders.
3. Define required belief pair, context tags, eligible event family, presentation lines, allowed choice-policy IDs, cooldown class, and narrative tags.
4. Do not include raw affinity/morale/stat deltas.
5. Provide multiple tonal variants: hostile confrontation, weary disagreement, intellectual debate, practical conflict, private doubt.
6. Ensure symmetric content quality across belief perspectives.
7. Validate all belief IDs and choice-policy IDs.
8. Start with a smaller set per pair before expanding to 25+ events.
9. Add content diversity tests or report coverage by conflict pair.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7M — Relationship consequences adapter

**Goal:** Route ideological social outcomes through canonical relation APIs.

### Required substeps

1. Define consequence policies such as supported_by_player, publicly_sided_against, mediated_constructively, pressured_to_convert, respected_boundary, ignored_concern, or reconciled.
2. Map policies to canonical relationship commands/events.
3. Do not directly change affinity/trust/resentment inside event code.
4. Use provenance IDs to prevent double application.
5. Apply pair-specific and participant-specific outcomes only when authoritative relation rules permit.
6. Add tests for support, rejection, mediation, pressure, duplicate event, and save/reload.
7. Measure whether ideological events make relation state more meaningful rather than merely noisier.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7N — Morale and mental-health consequences

**Goal:** Use friction events as social stressors/supports without duplicating mental-health calculations.

### Required substeps

1. Identify canonical stress/morale/grief/crisis APIs.
2. Emit bounded social stress/support events after resolved ideological situations.
3. Do not hard-code shelter-wide morale penalties in event templates.
4. Allow repeated ideological harassment/conflict to increase crisis risk only through mental-health policy.
5. Allow successful mediation to reduce stress if canonical systems support it.
6. Ensure critical crises remain under mental-health authority.
7. Add tests for stressor event, supportive resolution, repeated conflict, crisis threshold interaction, and duplicate suppression.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7O — Duty roster and housing consequences

**Goal:** Let ideological friction influence assignment recommendations and constraints through canonical duty/housing systems.

### Required substeps

1. Audit whether bunks/rooms/shifts are mutable via a canonical housing/duty authority.
2. Expose ideological incompatibility as one input to assignment scoring or warnings.
3. Do not let event state directly move survivors between rooms or shifts.
4. Allow the player to separate conflicting survivors through normal assignment commands.
5. Define opportunity cost/efficiency consequence through canonical scheduling/capacity systems.
6. Create mediation choices that suggest rather than secretly execute housing changes.
7. Add tests for incompatible pair warning, reassignment, capacity failure, work-essential pairing, and save/load.
8. Keep blanket 'efficiency penalty' out of ideological event code.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7P — Moral-choice integration

**Goal:** Let player responses to ideological disputes contribute to moral/policy history without turning every disagreement into a global morality score.

### Required substeps

1. Audit moral-choice bands/categories and decision-record API.
2. Map only genuinely moral/policy choices into the moral-choice system.
3. Examples: coercive suppression, discrimination, protection of expression, forced participation, punishment for belief, equal accommodation.
4. Do not map ordinary debate preference to morality automatically.
5. Use stable choice provenance.
6. Let moral authority determine future consequences.
7. Add tests for morally relevant choice, neutral social choice, duplicate decision, and save/load.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7Q — Autonomy integration

**Goal:** Reuse survivor autonomy for initiating, rejecting, or responding to ideological events.

### Required substeps

1. Allow autonomy to express objection, request mediation, attempt persuasion, refuse participation, volunteer as mediator, or seek support.
2. Do not duplicate autonomy scoring inside ideological event code.
3. Use event context as an opportunity fed into the autonomy layer.
4. Honor survivor refusal to participate where policy allows.
5. Prevent event system from forcing an autonomous action outcome.
6. Add tests with autonomy enabled and disabled.
7. Ensure deterministic cross-system ordering so the same seed yields the same participants/response.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7R — Quest hook contract

**Goal:** Create optional friction-driven quest objectives through the canonical quest system.

### Required substeps

1. Define quest triggers from resolved or sustained friction records rather than raw affinity thresholds alone.
2. Use event/participant/belief provenance IDs.
3. Quest authority owns quest lifecycle.
4. Implement source concepts such as Debate, Conversion, Schism, Heretic/Seeker identity crisis, Mediator, or Peacemaker only where content fits current narrative tone.
5. Treat 'Crusade/purge' content as high-risk escalation requiring governance/violence rails; defer from first slice.
6. Prevent duplicate quest spawning from repeated event variants.
7. Allow quests to expire or become invalid if participants die/leave or beliefs change.
8. Add tests for eligibility, duplicate guard, invalidation, clean resolution, and save/load.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7S — Identity-questioning and personal crisis events

**Goal:** Model a survivor questioning beliefs as an individual narrative arc rather than an automatic conversion attempt.

### Required substeps

1. Generate eligibility from belief confidence/openness if it exists, recent contradictory experiences, trusted relationships, trauma, moral events, or personal goals.
2. Present options such as listen, encourage reflection, reinforce current tradition, refer to another survivor, or leave them space.
3. Do not force a target belief.
4. Route mental-health consequences through canonical authority.
5. Route future belief-transition eligibility through belief authority.
6. Allow quest/journal hooks.
7. Add tests for no-change outcome, deeper questioning, reaffirmation, and transition eligibility.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7T — Group polarization detector

**Goal:** Detect meaningful shelter-level blocs without immediately creating a new faction system.

### Required substeps

1. Compute group polarization from verified belief distribution, repeated cross-group friction, within-group affinity, and repeated event history.
2. Do not form a faction merely because three survivors share a belief.
3. Require sustained social coherence and opposition.
4. Represent early result as a `PolarizationWarning`/group condition, not a full durable faction authority.
5. Cap group analysis frequency.
6. Use canonical survivor groups/relations if an existing group system exists.
7. Add tests for 3-vs-3 with no friction, 3-vs-3 with sustained conflict, mixed friendships across groups, leader absence, and decay after mediation.
8. Keep group membership derived where practical.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7U — Informal ideological bloc model

**Goal:** Only if group polarization proves useful, represent informal blocs as bounded social groups rather than external factions.

### Required substeps

1. Write a separate mini-ADR before enabling durable bloc state.
2. Define bloc ID, belief theme, member references, spokesperson/leader reference, demands/issues, cohesion, and status only if no canonical social-group authority can represent it.
3. Keep external faction diplomacy separate.
4. Do not grant automatic morale bonuses to recognized blocs.
5. Do not create resource ownership inside blocs.
6. Let group demands become event/quest/policy requests.
7. Define formation, membership change, dissolution, and retention.
8. Add tests for formation, mixed-belief departure, leader change, dissolution, and save/load.
9. Feature-flag this layer independently.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7V — Bloc demands and governance handoff

**Goal:** Route shelter-wide ideological demands through governance/policy authority rather than resolving them in the friction system.

### Required substeps

1. Examples may include service/ritual access, quiet hours, duty exemptions, housing preferences, resource-distribution policy, or speech/meeting rules where the game models them.
2. Validate each demand against actual shelter systems.
3. Create policy proposal/quest/event references.
4. Player responses go through governance/moral-choice authorities.
5. Record group reaction through relations/morale systems.
6. Do not directly modify shelter efficiency.
7. Add tests for valid demand, impossible demand, rejected demand, accepted policy, and no-governance fallback.
8. Defer demands that require nonexistent systems.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7W — Suppression, coercion, and discrimination boundary

**Goal:** Treat coercive shelter responses as explicit governance choices with consequences, not convenient event-resolution buttons.

### Required substeps

1. Require governance/leadership authority before implementing suppression.
2. Define actions such as prohibit meeting, force reassignment, punish harassment, ban coercive conversion, or discriminate against a belief only through policy commands.
3. Separate neutral safety enforcement from belief suppression.
4. Route moral/relations/morale consequences through canonical systems.
5. Prevent a single 'Suppress faction' option from directly writing resentment and efficiency values.
6. Require clear player-facing consequences.
7. Add tests for legal/available policy, unavailable policy, enforcement, consequence provenance, and save/load.
8. Keep violent purge/rebellion scope deferred.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7X — Violence/escalation safety gate

**Goal:** Prevent ideological friction from jumping directly from disagreement to violence without existing conflict/combat/crisis rails.

### Required substeps

1. Define escalation levels: disagreement, heated confrontation, harassment, refusal, threat, physical altercation, organized violence.
2. Map only levels already supported by social/conflict systems.
3. Use existing conflict/violence authority for physical altercations.
4. Do not create a second combat resolver.
5. Require repeated severe conditions and failed interventions before high escalation where appropriate.
6. Add cooldown and de-escalation paths.
7. Add tests for no-direct-jump, de-escalation, canonical conflict handoff, and duplicate prevention.
8. Defer organized ideological violence to a separate approved plan.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7Y — Ideological tension UI and read model

**Goal:** Expose belief distribution, active friction, recent events, and mediation opportunities without creating a fake management console.

### Required substeps

1. Audit survivor, relationship, shelter, journal, and social overview panels.
2. Prefer extending an existing social/shelter surface.
3. Show belief profiles only at the level the player is canonically allowed to know.
4. Show current tension pairs/groups, sustained duration, recent events, and actionable mediation/housing/duty options.
5. Do not expose exact hidden event probabilities.
6. Show choice consequences only where canonical systems can forecast them.
7. Link participants to survivor details.
8. Show unresolved group polarization as a warning rather than a new faction panel in the first slice.
9. Add snapshot tests for no friction, one pair, multiple pairs, questioning survivor, active event, and polarization warning.
10. Add interaction tests for mediation choices.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7Z — Journal and historical record

**Goal:** Record meaningful ideological events and belief transitions without flooding the chronicle.

### Required substeps

1. Use canonical journal/chronicle authority if available.
2. Record first confrontation, major mediation, persuasion boundary crossing, belief transition, major group polarization, quest outcome, and governance decision.
3. Keep routine recurring disagreement in bounded recent-history only.
4. Use stable event IDs for dedupe.
5. Summarize repeated conflicts over the same issue.
6. Add retention caps.
7. Add tests for duplicate resolution, repeated issue summary, belief transition, and save/load.
8. Allow E1-5 legacy to consume only major ideological records in future campaigns if approved.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7AA — Ideological event catalog

**Goal:** Create data-driven narrative templates that reference consequence policies rather than cross-system deltas.

### Required substeps

1. Define template ID, family, required belief pair/tags, context tags, participant roles, eligibility rules, cooldown class, choice-policy IDs, dialogue/localization keys, journal importance, and escalation class.
2. Validate all belief/profile IDs.
3. Validate all choice/consequence policy IDs.
4. Prohibit direct affinity, morale, standing, duty-efficiency, or shelter-stat changes in template data.
5. Start with a small representative corpus, then expand toward 25 events after diversity testing.
6. Ensure each major conflict pair gets more than one tone/outcome.
7. Add duplicate ID, unknown belief, invalid family, invalid policy, impossible participant role, and escalation-level validation tests.
8. Document authoring guidelines that avoid caricaturing one belief profile.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7AB — Save model and old-save compatibility

**Goal:** Persist only active event workflow, cooldowns, bounded history, and any approved belief-evolution metadata.

### Required substeps

1. Determine whether an existing social-event save section can own active events.
2. Persist event IDs/stage/participants/context, awaiting-player choice, cooldown expiries, bounded recent history, and group-polarization state only if durable.
3. Persist belief confidence/openness only under the canonical belief authority.
4. Do not persist copied relation/morale/duty values.
5. Default old saves to no active friction event and empty cooldown/history.
6. Define schema version.
7. Add migration fixture.
8. Add round-trip tests for active confrontation, awaiting mediation, persuasion event, cooldown, polarization warning, and no-history.
9. Ensure restore does not reroll the selected event/template.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7AC — Exploit, deadlock, and repetition audit

**Goal:** Prevent ideological events from being farmed, spammed, or used to bypass normal systems.

### Required substeps

1. Test save/reload around event selection to prevent reroll.
2. Test repeated side-taking does not farm relation/moral effects.
3. Test conversion attempts cannot be spammed every day.
4. Test repeated housing shuffle does not bypass capacity/duty rules.
5. Test quest spawn dedupe.
6. Test belief transition cannot oscillate rapidly.
7. Test polarization does not remain permanently active after conditions resolve.
8. Test all survivors in one belief profile produces no manufactured conflict.
9. Test extreme friction does not create multiple simultaneous events involving same survivor.
10. Test event history remains bounded.
11. Add fuzz/property tests over random belief/affinity distributions if infrastructure supports it.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7AD — Performance and long-run event-distribution soak

**Goal:** Prove ideological event generation remains bounded and diverse over a full campaign.

### Required substeps

1. Run 180-day headless simulation with representative belief distributions and relationship changes.
2. Measure friction checks, eligible candidates, selected events, active events, template diversity, repetition rate, allocations, tick cost, save size, and journal size.
3. Prevent all-pairs scans every frame.
4. Use threshold/event-driven evaluation.
5. Record median/p95 event-evaluation time.
6. Measure events per survivor/week.
7. Measure same-pair repeat interval.
8. Measure group-polarization false positives.
9. Block content expansion if event rate or repetition is unhealthy.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7AE — Narrative fairness and content review

**Goal:** Ensure the system models conflict without encoding one worldview as mechanically superior by default.

### Required substeps

1. Review every belief pair for symmetrical access to constructive and destructive outcomes.
2. Ensure template tone distinguishes character bias from system truth.
3. Check that mediation does not always reward one side's assumptions.
4. Check that persuasion success is not tied to one ideology being objectively 'correct'.
5. Check that suppression/discrimination consequences arise from policy/behavior, not belief identity alone.
6. Review belief-specific dialogue for caricature/repetition.
7. Use second-tool content review on the initial corpus.
8. Document intentional asymmetries only when grounded in specific character behavior or world events.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7AF — Quest-chain implementation slice

**Goal:** Implement a small number of high-value friction quest chains after base events are stable.

### Required substeps

1. Prioritize `The Debate`, `The Peacemaker`, `The Seeker`, and `The Schism` precursor over violent purge content.
2. Define each quest's eligibility from stable event/group records.
3. Use canonical quest state and choice systems.
4. Create failure/expiry paths if participants die/leave/beliefs change.
5. Use existing dialogue/event assets where possible.
6. Add end-to-end tests from friction trigger to quest resolution.
7. Measure quest trigger frequency.
8. Do not ship every source-plan quest simply to meet a count.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7AG — Group schism release gate

**Goal:** Require explicit governance readiness before persistent shelter-wide ideological factions become active.

### Required substeps

1. Verify individual friction events are accepted in playtests.
2. Verify E1-6-style survivor autonomy and refusal rails are stable.
3. Verify governance/leadership/policy authority exists.
4. Verify group membership can be represented without duplicating survivor state.
5. Verify demands can route through real shelter systems.
6. Verify mass refusal/violence have fail-safe rules.
7. Run a dedicated ADR for durable ideological blocs.
8. Only then enable bloc formation/suppression/schism mechanics.
9. Otherwise keep polarization as read-only warning/event context.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the selected template or outcome opportunity.
- The same pair farms repeated relationship/moral rewards.
- A 3-vs-3 belief distribution forms a faction despite no sustained friction.
- Suppression writes shelter efficiency or resentment directly.
- One survivor appears in incompatible simultaneous ideological events.
- Journal/history grows without bound.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/load no-reroll behavior passes.
- [ ] Template integrity passes.
- [ ] Event distribution/repetition is measured.
- [ ] Player-facing event reasons and choices are clear.
- [ ] Narrative fairness review completed.

---

## E1-7AH — Headless selftest and data-integrity validation

**Goal:** Build a deterministic mini-shelter proving friction can become an event and resolve through canonical systems.

### Required substeps

1. Create two survivors with a verified conflicting belief pair.
2. Set relation/friction state to produce an eligible confrontation.
3. Run event evaluation and assert deterministic template selection.
4. Present/resolve one mediation choice.
5. Verify relationship/morale consequences are applied through canonical adapters.
6. Create a second high-trust cross-belief pair and verify debate/persuasion eligibility without forced conversion.
7. Save/reload an awaiting-player event and verify no reroll.
8. Advance cooldown and verify eligibility behavior.
9. Validate ideological event catalog.
10. Assert event code did not directly mutate protected relation/morale/duty fields.
11. Create `--friction-events-selftest` if conventions support it.

### Ideology-event invariants

- The event layer presents situations; canonical systems resolve consequences.
- Belief identity changes only through the canonical belief authority.
- No JSON template writes relationship, morale, duty, standing, or shelter stats directly.
- Player/survivor choices are explicit and provenance-tracked.
- Repeated conflict is cooldown- and diversity-controlled.
- Mass group/suppression mechanics remain gated behind governance readiness.
- Save/reload cannot reroll a selected event.
- Belief-specific content does not encode one worldview as system-level truth by default.

### Negative tests

- A friction event directly mutates a protected canonical field.
- One conversion attempt changes categorical belief without transition policy.
- Reload changes the

<!-- Deliverable capped to remain within the requested 50–90k character envelope. -->
