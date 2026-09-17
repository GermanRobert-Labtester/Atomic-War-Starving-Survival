---
PLAN_ID: E1-3
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 3
STATUS: READY_FOR_EXECUTION_WHEN_RAILS_PASS
SOURCE_PLAN: "Plan 131 — Wasteland Information & Rumor Network"
SEQUENCE_FILENAME: "E1_planintegration[3].md"
PREVIOUS_FILENAME: "E1_planintegration[2].md"
NEXT_FILENAMES:
  - "E1_planintegration[4].md"
  - "E1_planintegration[5].md"
CATEGORY: LINK+INFORMATION_FLOW+PRESENTATION
PRIMARY_INTENT: "Create one bounded information-flow layer by integrating existing radio, gossip, caravan, world-event, knowledge, faction, economy, quest, and expedition authorities."
EXECUTION_STYLE: "Flagship integration plan"
PREMISE_VERIFICATION_REQUIRED: true
INTAKE_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
SECOND_INTEL_AUTHORITY_FORBIDDEN: true
RUNTIME_RISK: MEDIUM_HIGH
SAVE_RISK: MEDIUM
CONTENT_RISK: MEDIUM
SCOPE_RISK: HIGH
---

# E1 Plan Integration [3] — Wasteland Information Flow, Rumor Propagation, Intelligence, and Disinformation

> **Sequence rule:** this file is `E1_planintegration[3].md`.
> The next files are `E1_planintegration[4].md`, `E1_planintegration[5].md`, and so on.
> The bracketed sequence number remains immediately before `.md`.

## 0. Mission

This plan converts Plan 131 into an integration-grade information-flow programme. The design goal is a world
where events create knowledge unevenly: travelers carry news, radios transmit some reports faster than roads,
factions distort what they disclose, settlements amplify local stories, and the player may act on information
that is incomplete, stale, biased, verified, or false.

The implementation must not begin from the premise that ASHFALL has “zero information systems.” The source
plan itself names `MoralChoiceGossipRuntime`, radio distress work, faction radio content, caravans, weather
forecasting, faction systems, and location evolution. The correct architectural question is therefore not
“where do we put a brand-new rumor domain?” but:

**Which facts are already owned by existing authorities, what information products do they emit, and what
minimal cross-location propagation layer is actually missing?**

The primary deliverable is a single bounded information-flow rail that turns authoritative events into
time- and source-dependent observations. It must not become a second faction state machine, second economy,
second quest system, second expedition discovery database, second radio system, or second reputation system.
Rumors are representations of facts and claims; they do not own the underlying truth.

## 1. Core Design Thesis

The player should be able to distinguish three layers:

1. **World truth** — authoritative state owned by existing systems.
2. **Information state** — what a location, faction, caravan, radio channel, or player currently knows or claims.
3. **Presentation** — what the player is told and how confident/credible the source appears.

A rumor is not truth. It is a bounded claim about truth.

This distinction drives every implementation rule in this plan.

## 2. Source Plan Objectives Preserved

The source Plan 131 asks for:

- persistent rumor/information state;
- rumors generated from location, faction-war, economy, and expedition events;
- caravan-based propagation;
- radio interception;
- information hubs with bias and credibility;
- rumor mutation and truthfulness decay;
- deliberate disinformation;
- rumor verification;
- rumor-board UI;
- quest/economy/expedition consequences;
- deterministic save/load behavior;
- old-save compatibility;
- starter templates;
- headless selftests;
- data-integrity validation.

Those objectives remain, but the implementation sequence is reorganized to prevent a monolithic
`RumorSystem` from swallowing responsibilities already owned elsewhere.

## 3. Non-Negotiable Architecture Rules

- World truth always lives in the existing authoritative domain.
- Information-flow code stores claims, provenance, confidence, freshness, and propagation state only.
- Rumor mutation may alter what is claimed, never the underlying world.
- Reputation/standing changes are applied by the canonical faction/standing authority.
- Economy effects are applied by the canonical economy authority.
- Quest unlocks are applied by the canonical quest authority.
- Hidden locations are revealed through the canonical knowledge/reveal authority.
- Caravan movement remains owned by caravan/travel systems.
- Radio delivery remains owned by radio/intel systems.
- Gossip inside the shelter remains owned by existing gossip/social runtime.
- The information-flow layer may subscribe to events from these systems and publish observations back to them.
- No consumer may treat rumor data as canonical truth without explicit verification.
- No rumor propagation loop may run unbounded over every location every tick.
- No rumor mutation rule may rely on non-deterministic `System.Random`.
- No content template may reference an unresolved faction/location/event/item.
- No UI surface may invent confidence, expiry, rewards, or availability that the authority has not exposed.
- No infinite farming of rumor sales, verification rewards, or standing.
- No old rumor survives indefinitely unless explicitly marked persistent/legendary by design.

## 4. High-Level Execution Order

`premise audit → authority map → event-to-information contract → observation DTOs → propagation graph →
hub/source policies → caravan transport → radio transport → aging/decay → mutation → disinformation →
player knowledge store → verification → quest/economy/expedition adapters → broker/sale safeguards →
UI/read model → save/migration → content templates → integrity/selftests → scale/performance soak → tuning`

Do not build UI, disinformation, or economy consequences before the core provenance/authority contract is
stable.


---

## E1-3A — Premise verification and information-authority audit

**Goal:** Replace the source plan's 'zero information systems' assumption with a verified map of what already exists and what is genuinely missing.

### Required substeps

1. Inspect `MoralChoiceGossipRuntime`, existing radio systems, distress-signal rails, signal triangulation/intel components, weather warning systems, caravan systems, location evolution, faction war, economy, quest, expedition, and standing/reputation code.
2. Classify each inspected system as truth authority, information producer, transport/channel, consumer, or presentation.
3. Record every existing information-like DTO, event, signal, reveal state, discovery state, forecast state, or gossip record.
4. Verify whether any existing knowledge ladder, intel coordinator, signal catalog, standing-memory, or player-discovery state can be reused.
5. Search for existing save sections that already persist knowledge, signal discovery, gossip, radio state, or location reveal state.
6. Create `docs/systems/INFORMATION_FLOW_AUTHORITY_MAP.md` with one row per fact and owner.
7. Mark the source plan's proposed new `RumorSystem` as provisional until the authority audit proves the missing responsibilities.
8. Identify which responsibilities are LINK work: subscribing to events, transporting observations, exposing read models, and wiring consumers.
9. Identify which responsibilities are genuinely new durable state: rumor/claim instances, provenance, per-node exposure, expiry, and verification state.
10. Run the duplicate-search receipt required by the intake policy and attach results to the plan metadata.
11. Set `PREMISE_VERIFIED_AT` to the audit commit and update the plan register.
12. Stop if a live information-flow authority already owns most of the proposed state; in that case produce an extension plan rather than a parallel system.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3B — Information model: truth references, claims, provenance, and observation state

**Goal:** Define data structures that represent knowledge claims without duplicating underlying world truth.

### Required substeps

1. Replace the overloaded concept of `truthfulness` with explicit fields for provenance, confidence, freshness, verification state, and optional distortion magnitude.
2. Define a stable `InformationClaim` or equivalent with ID, origin event reference, subject type, subject ID, source entity/channel, origin day/tick, received day/tick, confidence, expiry, tags, and payload variant.
3. Define a separate `ClaimPayload` shape for bounded subject-specific details such as quantity band, direction, location, faction attribution, urgency, or outcome.
4. Ensure claim payloads reference domain IDs instead of copying domain objects.
5. Define verification states such as Unverified, Corroborated, Verified, Disproved, Expired, and Superseded.
6. Define provenance chain entries for source → intermediary → recipient so mutation and credibility can be explained.
7. Define a compact immutable event fingerprint so multiple claims can refer to the same originating event without duplicating truth.
8. Define whether two variants are siblings of one source claim or independent claims about the same event.
9. Define deterministic claim IDs derived from seed/event/source where useful, while preserving uniqueness.
10. Define serialization DTOs separately from runtime objects if existing save conventions require it.
11. Add invariants for valid confidence range, valid subject IDs, monotonic receipt times, bounded provenance length, and no cycles in provenance.
12. Create fixtures for exact report, stale report, biased report, fabricated report, corroborated report, and superseded report.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3C — Event-to-information source contract

**Goal:** Create a narrow contract through which authoritative systems can emit information-worthy events without coupling to rumor internals.

### Required substeps

1. Define `IInformationSource`/`IRumorSource` as an adapter-facing contract, not something every domain system must implement directly if event subscriptions are cleaner.
2. Prefer subscribing to existing domain events over modifying authorities to know about rumors.
3. Define a normalized `InformationEvent` containing authoritative event ID, subject references, event kind, event time, locality, visibility class, and optional fact payload.
4. Create adapters for location-evolution events such as captured, ruined, cleared, opened, closed, or transformed.
5. Create adapters for faction-war events such as territory change, battle, ceasefire, treaty, raid, or mobilization where those events exist.
6. Create economy adapters for price shocks, shortage, surplus, route establishment/closure, and market disruption where authoritative events exist.
7. Create expedition adapters for discoveries, hazard encounters, recovered evidence, and resource finds subject to knowledge rules.
8. Create weather/intel adapters for warnings only if those systems do not already own dissemination.
9. Create player-caused event tagging so rumors can later attribute blame/credit without storing duplicate world state.
10. Filter events by information-worthiness before claim generation to avoid converting every tick into a rumor.
11. Add tests proving one authoritative event produces a stable information event and does not mutate the source system.
12. Document the rule for events that should never become rumors because they are private, instantaneous, low-value, or already directly visible.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3D — Propagation topology and information hubs

**Goal:** Define where information can travel using existing world topology, route connectivity, factions, and channels rather than an unrelated rumor graph.

### Required substeps

1. Model propagation endpoints as references to real locations, factions, caravans, radio channels, or player knowledge contexts.
2. Use the canonical map/route graph for physical travel adjacency.
3. Use canonical radio/intel coverage for broadcast adjacency.
4. Define hub metadata as policy only: amplification, suppression, source credibility, forwarding willingness, and channel capabilities.
5. Do not duplicate route distance, weather, territory, or faction hostility inside the information graph.
6. Define per-channel costs such as physical delay, radio delay, blackout, hostility filtering, or access requirements.
7. Define whether factions have virtual hub nodes only when they correspond to real communication capability.
8. Create deterministic route selection for claim propagation.
9. Define propagation fan-out caps and per-tick work budgets.
10. Define isolated locations as normal cases with zero incoming physical information unless another channel reaches them.
11. Add tests for connected settlements, isolated location, hostile boundary, radio-only link, caravan-only link, and multi-path duplicate arrival.
12. Ensure duplicate arrivals corroborate or refresh one claim rather than creating unbounded clones.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3E — Core information-flow runtime and deterministic scheduler

**Goal:** Implement the minimal durable runtime that owns claim instances, delivery queues, aging, and recipient exposure.

### Required substeps

1. Create the runtime only after E1-3A proves it is not duplicating an existing authority.
2. Separate active claims from scheduled deliveries.
3. Use `ISeededRng` or the project's deterministic RNG service for all mutation, forwarding, and deliberate-disinformation choices.
4. Process propagation on discrete day/segment/tick boundaries compatible with existing world simulation cadence.
5. Use bounded queues and stable ordering to guarantee deterministic replay.
6. Prevent O(rumors × all_locations) scans by indexing pending deliveries by target/time and claims by subject/location.
7. Define claim deduplication by event fingerprint + payload lineage.
8. Define recipient exposure state separately from global claim existence.
9. Implement capture/restore of claims, queues, exposure state, and deterministic RNG state only where necessary.
10. Do not persist derived indexes if they can be rebuilt deterministically on restore.
11. Add determinism tests across multiple runs and save/reload boundaries.
12. Add long-run cap tests proving active-claim count and queue size stay within configured ceilings.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3F — Aging, freshness, expiry, and supersedence

**Goal:** Make information become stale in a way the player can reason about without arbitrary noise.

### Required substeps

1. Define freshness as time since originating event and time since recipient receipt.
2. Separate confidence decay from factual mutation; old information can be accurately remembered yet obsolete.
3. Define expiry windows by event class rather than one universal decay rate.
4. Allow authoritative follow-up events to supersede older claims about the same subject.
5. Mark expired claims instead of deleting immediately if a short history window is useful for player comprehension.
6. Prune expired claims deterministically under retention policy.
7. Prevent stale claims from continuing to affect economy, quests, or reveal logic after their allowed window.
8. Define persistent historical/legendary rumor category only if justified and explicitly capped.
9. Add tests for decay, expiry, supersedence, stale-but-still-visible UI, and post-expiry non-effect.
10. Expose freshness in the read model as exact age or qualitative band according to UI design.
11. Measure active rumor count, median age, and expired retention size.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3G — Bias, credibility, and deterministic mutation

**Goal:** Create bounded source-specific distortion that remains interpretable and testable.

### Required substeps

1. Define bias as policy over which facts are emphasized, suppressed, or framed, not as freeform random rewriting.
2. Define source credibility as a prior used by information consumers, not the same thing as factual truth.
3. Create a small mutation operator set: quantity band shift, urgency shift, attribution substitution among valid candidates, location imprecision, outcome exaggeration, and omission.
4. Prohibit mutation from inventing unresolved IDs.
5. Cap mutation depth by hop count and confidence floor.
6. Keep key identity facts immutable for event classes where mutation would make the claim meaningless.
7. Record mutation lineage so debugging can explain why a claim changed.
8. Use deterministic RNG keyed by claim lineage and hop.
9. Allow multiple biased variants of one event to coexist when sources differ materially.
10. Add tests for same-seed mutation equality, different-source variation, valid-ID preservation, bounded distortion, and no mutation after verified status if that is the chosen rule.
11. Document which fields may mutate for each template/event class.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3H — Deliberate disinformation and fabrication contract

**Goal:** Allow factions to spread false claims without granting the information system authority over faction strategy or world truth.

### Required substeps

1. Make deliberate disinformation originate from a faction/policy decision adapter, not from arbitrary rumor runtime chance.
2. Represent fabricated claims with explicit provenance and no authoritative world-event fingerprint or with a fabrication marker.
3. Define which faction AI/policy conditions can authorize a disinformation event.
4. Use canonical faction resources/capabilities if disinformation has costs.
5. Define target audience, objective, duration, and subject constraints.
6. Prevent fabricated claims from creating real world changes directly.
7. Allow the player to disprove fabricated claims through verification/corroboration.
8. Route discovered deception consequences to canonical standing/reputation/trust systems.
9. Add cooldowns and per-faction active-disinformation caps.
10. Add tests for fabrication creation, propagation, disproval, trust consequence, cooldown, and zero truth-state mutation.
11. Keep advanced propaganda campaigns out of the first slice unless a separate approved plan expands them.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3I — Caravan-as-news-carrier integration

**Goal:** Make caravans physically transport information between locations while remaining owned by the caravan system.

### Required substeps

1. Add a bounded manifest of claim references or knowledge tokens to caravan state only if the caravan authority legitimately carries them.
2. Prefer pickup/dropoff hooks at departure/arrival over per-tick rumor manipulation inside caravan movement.
3. At departure, sample eligible claims known at origin according to configured capacity and relevance.
4. At arrival, expose carried claims to the destination hub and/or player if present.
5. Do not deliver news when the caravan is destroyed, lost, rerouted, or never arrives.
6. Define whether a caravan can learn news at intermediate stops using existing travel semantics.
7. Deduplicate news already known at the destination.
8. Record caravan provenance hop for credibility/debugging.
9. Add tests for pickup, successful delivery, loss before delivery, reroute, repeated route, full news capacity, and deduplication.
10. Measure percentage of physical information deliveries attributable to caravans.
11. Do not let rumor cargo consume item inventory slots unless explicitly designed as a physical document item.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3J — Radio transmission and interception integration

**Goal:** Use existing radio/intel rails for fast information delivery and player interception rather than inventing a second broadcast stack.

### Required substeps

1. Audit existing radio tuner, signal catalog, distress signal, triangulation, and weather/intel broadcast systems.
2. Define which claim types can be broadcast and over which existing channels.
3. Use radio coverage, tuning, encryption/access, power, and interference rules from existing authorities.
4. Create an adapter that turns eligible faction/location information into broadcast payloads.
5. Create player interception exposure through the existing receiver/tuner path.
6. Do not create a separate 'radio receiver built' boolean if construction/power systems already own that capability.
7. Define broadcast delay and confidence based on source/channel, not physical distance alone.
8. Allow radio claims to corroborate caravan/traveler claims.
9. Prevent one broadcast from re-adding the same claim every tick.
10. Add tests for in-range intercept, out-of-range, unpowered receiver, hostile/encrypted channel where supported, repeated broadcast dedupe, and deterministic replay.
11. Measure radio vs physical propagation latency.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3K — Player knowledge store and information asymmetry

**Goal:** Track what the player has heard, how it was sourced, and whether it has been verified without turning player knowledge into world truth.

### Required substeps

1. Define player exposure/knowledge as references to claim IDs plus receipt/source/verification metadata.
2. Merge corroborating versions under a common event/subject view while preserving source differences.
3. Do not automatically reveal exact authoritative values because the player heard a rumor.
4. Define confidence presentation bands such as hearsay, plausible, corroborated, verified, disproved, and stale.
5. Define whether the player can forget/prune old claims in UI without deleting global history.
6. Persist player knowledge inside the information-flow save section or an existing knowledge section according to the authority audit.
7. Expose query APIs by current location, subject, faction, age, source, and verification state.
8. Add tests for hearing same rumor from multiple sources, conflicting claims, verification, save/load, and expiry.
9. Ensure headless mode maintains the same player-knowledge state transitions without UI.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3L — Rumor verification and corroboration

**Goal:** Give the player a costly way to turn uncertain claims into stronger knowledge using existing expedition, intel, or observation systems.

### Required substeps

1. Define verification targets from claim subject/event, not a generic 'verify rumor' minigame.
2. Route location verification through expedition/travel/observation authorities.
3. Route faction claims through corroborating contacts/radio/standing systems where appropriate.
4. Define verification cost in time, risk, resources, or opportunity rather than an arbitrary rumor currency.
5. On verification, compare against current authoritative world state or immutable event evidence depending on claim class.
6. Handle the case where the rumor was once true but is now stale.
7. Mark claim Verified, Disproved, or Superseded without rewriting its original payload.
8. Allow corroboration from independent sources to improve confidence without full verification.
9. Add tests for accurate rumor, false rumor, stale rumor, inaccessible target, conflicting sources, and verified result after save/load.
10. Measure verification usage and payoff.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3M — Quest integration: optional objectives and gated discovery

**Goal:** Allow rumors to create opportunities without making the quest system dependent on unverified claims as truth.

### Required substeps

1. Define an adapter from eligible claim states to optional quest hints/objectives.
2. Require the quest authority to own quest creation/progression.
3. Use rumor claim IDs as provenance so the quest can explain why the player knows about the objective.
4. Distinguish rumor-triggered optional objective from authoritative mandatory quest state.
5. Define what happens if the rumor expires, is disproved, or target state changes before acceptance.
6. Prevent duplicate quest farming from repeated versions of the same underlying event.
7. Add cooldown/deduplication keys based on event fingerprint and quest template.
8. Add tests for unlock, duplicate arrival, disproval before acceptance, stale rumor, and completion after verification.
9. Keep quest rewards canonical and independent of rumor runtime.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3N — Economy integration: information influences behavior, not price ownership

**Goal:** Allow market actors/pricing logic to react to information while keeping the economy authoritative.

### Required substeps

1. Identify the exact economy/pricing authority and event hooks.
2. Define which verified or high-confidence claims may influence local demand/supply expectations.
3. Do not let rumor data directly set prices.
4. Pass bounded signals such as perceived shortage risk, route risk, or expected scarcity into canonical economy policy.
5. Define duration and decay of informational effects.
6. Prevent fabricated claims from creating permanent economy state.
7. Allow deliberate misinformation to cause temporary market behavior only if economy design approves the signal.
8. Add anti-loop guards so price changes caused by rumor do not immediately generate the same rumor endlessly.
9. Add tests for shortage rumor, disproved shortage, expired effect, repeated rumor dedupe, and price restoration.
10. Measure magnitude and duration of rumor-induced price variance.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3O — Expedition and hidden-location integration

**Goal:** Use information to reveal or hint at destinations through the canonical knowledge/reveal system.

### Required substeps

1. Map eligible claims to location hints or expedition opportunities.
2. Use knowledge rung/reveal authority to decide whether a rumor yields vague region, approximate node, or exact destination.
3. Do not set hidden destination discovered merely because a rumor exists unless the acceptance rule allows it.
4. Allow verification expedition to upgrade knowledge.
5. Define false-location or imprecise-location outcomes without inventing invalid node IDs.
6. Add tests for vague hint, exact verified reveal, false lead, inaccessible site, duplicate discovery, and stale lead.
7. Ensure expedition dispatch still validates actual destination existence and reachability.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3P — Standing, trust, and source credibility consequences

**Goal:** Connect discovered deception and reliable reporting to existing social/faction standing without duplicating reputation state.

### Required substeps

1. Define when source credibility is local information metadata versus canonical faction/player trust.
2. Route standing changes through existing standing/reputation APIs.
3. Apply penalties for proven deliberate deception only when attribution is known.
4. Do not punish a faction merely because a rumor mutated through unrelated intermediaries.
5. Allow repeated accurate/false reports to affect source credibility metadata within bounded limits.
6. Define credibility recovery over time or through verified behavior.
7. Add tests for accurate source, accidental distortion, deliberate lie, unknown attribution, and repeated reports.
8. Keep rumor exposure itself from changing standing unless the player acts or verification creates a consequence.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3Q — Information broker role and exploit-safe selling

**Goal:** Support buying/selling intelligence as a bounded interaction using canonical economy/standing systems.

### Required substeps

1. Define sellable information eligibility by novelty, confidence, age, recipient knowledge, and subject relevance.
2. Require the recipient faction/location to not already know an equivalent claim.
3. Compute reward through canonical economy/reputation services.
4. Mark sale exposure so the same recipient cannot be farmed repeatedly.
5. Define whether selling unverified/false information risks later standing consequences.
6. Add per-claim and per-recipient cooldown/deduplication keys.
7. Prevent save-scumming exploits where deterministic payout can be repeated by reload if the canonical game has transaction journaling.
8. Add tests for first sale, repeated sale, sale to informed recipient, expired rumor, false rumor consequence, and multi-recipient sale if allowed.
9. Keep skill-tree specialization as a follow-on plan, not part of the foundational system.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3R — Rumor Board UI and read-model contract

**Goal:** Present current information clearly at the player's location without exposing hidden truth or creating fake authority.

### Required substeps

1. Reuse an existing information/radio/intel panel if one already provides the appropriate surface; otherwise justify a new routed panel through intake.
2. Create a read model grouped by subject/event with source variants underneath.
3. Display source, age, confidence band, verification state, and affected location/faction where known.
4. Do not display exact hidden truthfulness values unless the design explicitly wants gamey percentages.
5. Show conflicting reports side by side rather than silently choosing one.
6. Expose actions only when authoritative commands exist: verify, track, sell/share, open map hint, open quest, inspect source.
7. Represent expired/disproved claims distinctly.
8. Provide keyboard navigation and standard panel wrapping/scroll behavior.
9. Add snapshot tests for exact report, conflicting variants, disinformation, expired rumor, and verified claim.
10. Add integration tests for each actionable button.
11. Measure information-board open rate and action conversion.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3S — Static templates and `information_networks.json`

**Goal:** Author a bounded starter corpus whose data drives phrasing/mutation policy without owning world facts.

### Required substeps

1. Define schema for information hub policy and rumor templates separately if mixing them would create ambiguous ownership.
2. Create 20 starter templates covering location change, faction movement, battle/treaty, shortage/surplus, route change, expedition discovery, threat, weather warning, and player-caused event categories.
3. Use template IDs and subject placeholders that resolve through canonical catalogs.
4. Define allowed mutation operators per template.
5. Define expiry class, default confidence/source policy, and eligible channels.
6. Do not hard-code narrative text that claims exact truth when the payload may be mutated.
7. Validate all referenced location/faction/event IDs.
8. Add localization-ready text keys if the project uses localization catalogs.
9. Add data-integrity fixtures for missing subject, invalid operator, invalid confidence, unknown channel, and duplicate template ID.
10. Keep future propaganda/encryption/black-market content outside the first 20 templates unless rails already exist.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3T — Save/load, migration, and old-save compatibility

**Goal:** Persist bounded information state safely and guarantee existing saves load with an empty/default information section.

### Required substeps

1. Register the information-flow save section through the canonical save registry only after confirming no existing knowledge section should own it.
2. Version the section from first release.
3. Persist active claims, bounded exposure state, pending deliveries if necessary, verification state, and deterministic scheduler/RNG state where required.
4. Do not persist derived indexes.
5. Define default empty state for old saves.
6. Add migration fixture from no-section save.
7. Add round-trip tests with conflicting claims, pending caravan delivery, radio delivery, verified/disproved states, and expired-history retention.
8. Add checksum/corruption tests.
9. Ensure restore does not replay already-consumed deliveries.
10. Ensure save/reload does not duplicate quest/economy consequences.
11. Document section ownership and lifecycle.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3U — Headless selftest and integrity validation

**Goal:** Make information flow verifiable in CI without UI or manual play.

### Required substeps

1. Create `--information-flow-selftest` only if the project selftest verb conventions support it.
2. Build a deterministic miniature topology with source location, connected hub, isolated hub, caravan path, radio path, and hostile boundary.
3. Generate one authoritative event.
4. Verify initial claim creation.
5. Verify physical propagation delay.
6. Verify radio propagation delay.
7. Verify mutation remains deterministic.
8. Verify isolated location receives nothing.
9. Verify hostility/suppression policy blocks or delays according to rules.
10. Verify expiry and supersedence.
11. Verify save/reload mid-propagation.
12. Verify player exposure and verification transition.
13. Verify one downstream adapter such as quest hint or knowledge reveal without mutating world truth incorrectly.
14. Add catalog validation to the existing data-integrity selftest.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3V — Scale, performance, and retention soak

**Goal:** Prove rumor propagation remains bounded under long campaigns and many event sources.

### Required substeps

1. Define budgets for active claims, pending deliveries, provenance length, per-location visible claims, and per-day propagation work.
2. Create a 180-day simulation with representative locations, factions, caravans, radios, and event rates.
3. Measure active claim count, scheduled deliveries, allocations, tick duration, save section size, and GC behavior where available.
4. Assert expired claims are pruned and duplicate arrivals do not explode state.
5. Measure worst-case fan-out event.
6. Test repeated faction-war/economy events without infinite feedback loops.
7. Test 20 starter templates plus synthetic stress templates.
8. Profile subject/location indexes and queue operations.
9. Add regression thresholds to tests or benchmark docs.
10. Block content expansion if propagation cost scales superlinearly with total historical rumor count.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3W — Feedback-loop and exploit audit

**Goal:** Prevent rumor-driven consumers from recursively generating endless rumors or rewards.

### Required substeps

1. Map every downstream adapter that can itself generate world events.
2. Add event-origin metadata so rumor-induced reactions can be distinguished from original events.
3. Define loop suppression for economy price rumor → price change → new rumor → price change.
4. Define loop suppression for quest unlock/completion events that should not regenerate equivalent rumors indefinitely.
5. Define loop suppression for standing changes caused by discovered disinformation.
6. Define broker reward deduplication across save/load.
7. Define verification reward limits.
8. Add tests for recursive economy loop, recursive quest loop, repeated radio rebroadcast, repeated caravan circuit, repeated sale, and save/reload exploit.
9. Document accepted intentional feedback loops separately from forbidden self-amplification.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3X — Cross-system acceptance scenarios

**Goal:** Prove the feature is valuable as information asymmetry rather than just a persistent rumor database.

### Required substeps

1. Scenario 1: a distant settlement is raided, caravan-delivered news arrives late, and the player can prepare before direct contact.
2. Scenario 2: radio delivers a faster but biased faction report that conflicts with caravan testimony.
3. Scenario 3: a shortage rumor temporarily affects player decisions while the authoritative market remains owned by economy.
4. Scenario 4: a hidden expedition destination is hinted at, then verified through travel.
5. Scenario 5: a faction deliberately spreads a false report, the player disproves it, and canonical standing/trust consequences apply.
6. Scenario 6: an isolated settlement remains informationally dark until a route or radio channel opens.
7. Scenario 7: the player sells novel intelligence once, then cannot farm the same recipient.
8. Scenario 8: old saves load with no rumor state and begin receiving new information normally.
9. Scenario 9: save/reload in mid-propagation yields identical final claims.
10. Scenario 10: 180-day soak stays within retention/performance budgets.
11. Record player-facing acceptance evidence for each scenario before broadening content.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3Y — Documentation, content handoff, and extension rules

**Goal:** Leave a clear contract for future refugee-news, propaganda, encryption, information-black-market, and broker-specialization expansions.

### Required substeps

1. Write `docs/systems/INFORMATION_FLOW.md` with truth-vs-claim architecture, event adapters, channels, save ownership, and consumer rules.
2. Document the authority map and extension points.
3. Document how to add a new information source event without editing the core runtime.
4. Document how to add a channel without duplicating route/radio authority.
5. Document how to add a template and allowed mutation operators.
6. Document how a consumer should react to claims without treating them as truth.
7. Add explicit forbidden patterns: direct price writes, direct quest state writes, direct hidden-location truth writes, direct standing writes.
8. Register follow-on plans for propaganda campaigns, encrypted communications, refugee information, broker specialization, or black-market systems only after core acceptance metrics pass.
9. Update plan register status and supersedence/merge links for overlapping rumor/intel proposals.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

## E1-3Z — Release gate and closure

**Goal:** Ship a small, deterministic information-flow slice and prevent expansion until connection, clarity, and performance are proven.

### Required substeps

1. Run .NET build/test and full game build.
2. Run data-integrity selftest.
3. Run `--information-flow-selftest`.
4. Run save round-trip and migration suites.
5. Run determinism suite across multiple seeds and save/reload boundaries.
6. Run feedback-loop/exploit suite.
7. Run 180-day performance/retention soak.
8. Verify UI authority and actionable-button wiring.
9. Verify at least four source domains produce claims through adapters.
10. Verify caravan and radio channels both work where rails exist.
11. Verify at least one quest/knowledge consumer and one economy/standing consumer without direct authority violation.
12. Verify 20 starter templates validate.
13. Capture metrics and residual blockers.
14. Do not mark DONE if the system is merely generating rumors; it must demonstrate information asymmetry affecting player decisions through existing authorities.

### Integration guardrails

- World truth remains owned by the source domain.
- The information layer owns only claims, provenance, exposure, confidence/freshness, and propagation.
- No consumer may directly mutate its domain by writing rumor state into canonical fields.
- Every cross-system mutation goes through the canonical API/command/event boundary.
- Every persistent collection has an explicit retention rule.
- Every RNG decision is deterministic and reproducible.
- Every UI action names an authoritative command.
- Every save change has old-save compatibility.

### Negative tests

- A rumor changes the underlying world truth.
- The same delivery is consumed twice after save/reload.
- A duplicate arrival creates an unbounded clone.
- An invalid subject ID propagates.
- A stale/expired claim still drives an irreversible consumer effect.
- A faction is blamed for mutation introduced by an unrelated intermediary.
- A radio/caravan claim appears when the transport/channel could not have delivered it.
- A repeated sale or verification can be farmed indefinitely.

### Acceptance evidence

- [ ] Targeted deterministic tests pass.
- [ ] Save/load or no-save rationale is covered.
- [ ] Runtime/headless evidence exists.
- [ ] Authority map remains correct.
- [ ] Metric or measurable acceptance property is recorded.
- [ ] Plan metadata and premise SHA are updated.

---

# 5. Canonical Information Model

## 5.1 World Truth vs Claim

```text
Authoritative domain event
    |
    v
InformationEvent adapter
    |
    v
Claim generation
    |
    +--> claim variant A (source: caravan, slower, higher credibility)
    |
    +--> claim variant B (source: faction radio, faster, biased)
    |
    +--> claim variant C (fabricated/disinformation)
    |
    v
Propagation / exposure
    |
    v
Player/location/faction knowledge
    |
    +--> verify / corroborate / ignore / sell / act
    |
    v
Canonical consumer commands
```

The claim layer never edits the source event.

## 5.2 Suggested Runtime Claim Shape

```yaml
claim_id: "info_..."
origin_event_id: "evt_..."
subject:
  type: "faction|location|event|economy|expedition"
  id: "..."
origin_location_id: "..."
origin_tick: 1234
source:
  entity_type: "settlement|faction|caravan|radio|player|unknown"
  entity_id: "..."
  channel: "traveler|caravan|radio|contact|direct"
received_tick: 1242
confidence: 0.68
verification: "UNVERIFIED"
freshness_class: "RECENT"
payload:
  template_id: "rumor_location_raided"
  fields:
    severity_band: "high"
    attribution_faction_id: "..."
lineage:
  parent_claim_id: "..."
  hop_count: 2
flags:
  fabricated: false
  corroborated: true
expiry_tick: 1400
```

Do not hard-code this exact schema if the repository already has compatible knowledge DTOs. Preserve the
contract, not the spelling.

---

# 6. Information Channels

| Channel | Speed | Range | Trust | Dependencies | Typical failure |
|---|---:|---:|---:|---|---|
| Direct observation | Immediate | Local | High | Player presence | None beyond perception |
| Traveler | Slow | Route-based | Medium | Reachability | Never arrives |
| Caravan | Slow/medium | Route-based | Medium-high | Caravan survival | Destroyed/rerouted |
| Faction contact | Medium | Network-based | Variable | Standing/access | Suppression/bias |
| Radio broadcast | Fast | Coverage-based | Variable | Power/tuning/range | Interference/encryption |
| Distress signal | Fast | Radio-based | Contextual | Existing distress rails | Emergency-only |
| Shelter gossip | Local | Shelter | Variable | Existing gossip runtime | No cross-location reach |

The first release should support only channels whose underlying authorities are already real enough to test.

---

# 7. Mutation Policy

Mutation must be bounded and legible.

Recommended operators:

1. **Quantity band shift** — “a few” → “many,” never arbitrary impossible counts.
2. **Urgency shift** — routine → serious, or serious → catastrophic within template limits.
3. **Location imprecision** — exact node → nearby region, not nonexistent location.
4. **Attribution substitution** — only among valid plausible factions if the template allows it.
5. **Outcome exaggeration** — damaged → destroyed, pushed back → routed, within configured bounds.
6. **Omission** — drop one optional detail.
7. **Compression** — multiple details summarized into one vague claim.

Forbidden mutation:

- inventing unknown IDs;
- changing immutable event identity when the template requires it;
- fabricating player actions unless the claim is explicitly deliberate disinformation;
- changing canonical save/world state;
- stacking mutations beyond configured hop/credibility bounds.

---

# 8. Truthfulness and Confidence

Avoid exposing one magical `truthfulness` scalar as the entire simulation.

Use separate concepts:

- **Factual accuracy:** comparison between claim payload and authoritative event/state.
- **Source credibility:** historical prior attached to source/channel.
- **Confidence:** recipient-facing belief estimate based on source, corroboration, age, and mutation.
- **Freshness:** whether the information still describes current conditions.
- **Verification:** whether a canonical observation confirmed or disproved it.
- **Fabrication:** whether it originated without a matching authoritative event.

A claim can be factually accurate but stale.
A claim can be false but come from a usually credible source.
A claim can be low-confidence but later verified.
A claim can be deliberately fabricated yet accidentally match later events; provenance still matters.

---

# 9. Retention and Caps

Recommended first-slice budgets should be explicit configuration, not comments:

- maximum active claims globally;
- maximum pending deliveries;
- maximum visible claims per location;
- maximum source variants per event;
- maximum provenance hop count;
- maximum retained expired/disproved claims per player/location;
- maximum faction disinformation campaigns;
- maximum sellable novelty window;
- maximum propagation work per simulation step.

When a cap is reached, prune by deterministic priority:

1. expired and disproved;
2. oldest low-value duplicates;
3. superseded variants;
4. low-confidence low-relevance claims;
5. never prune active unresolved commitments/quest-critical information without an explicit consumer handoff.

---

# 10. Feedback-Loop Rules

The information layer connects many systems, so recursive loops are a primary risk.

### Economy loop guard

`economy event → rumor → economy perception effect → price change`

The resulting price change must not automatically produce the same shortage rumor again unless a separate
authoritative threshold event occurs and deduplication permits it.

### Quest loop guard

`rumor → optional quest → quest completion event`

Quest completion may generate a new world event, but must not regenerate the same initial rumor identity.

### Standing loop guard

`disinformation → verification → standing penalty`

The standing penalty may be newsworthy later, but must use a distinct event type and cooldown.

### Radio rebroadcast guard

Broadcast exposure must be idempotent by claim/channel/recipient window.

### Caravan circuit guard

A caravan repeatedly traveling A ↔ B must not duplicate the same known claim indefinitely.

---

# 11. Player-Facing Scenarios

## Scenario A — Slow truth, fast bias

A settlement is raided. The authoritative location/faction systems resolve the event. A caravan departs with
a relatively accurate claim. A faction radio channel broadcasts a faster version blaming a rival. The player
hears the radio first, then the caravan version later. The UI shows source conflict rather than selecting a
hidden “correct” answer.

## Scenario B — Stale but once-true shortage

A real shortage occurs and propagates slowly. By the time the player reaches the market, the shortage has
ended. The rumor is historically accurate but stale. The economy does not remain permanently modified just
because an old claim exists.

## Scenario C — False lead and verification

A faction fabricates a report of a weapons cache. The player can accept the risk and verify through an
expedition. The claim is disproved, the world remains unchanged, and the appropriate trust/standing
consequence goes through canonical systems.

## Scenario D — Isolated settlement

A physically isolated settlement with no radio access receives no distant rumors. Opening a route or restoring
radio coverage changes the information topology without adding a special-case rumor rule.

## Scenario E — Information broker

The player hears a novel, recent, corroborated claim and sells it to a faction that does not know it. The
transaction occurs once. Repeating the same sale or reloading cannot duplicate the reward.

---

# 12. Starter Template Categories

The initial 20 templates should span, for example:

1. location raided;
2. location captured;
3. location ruined;
4. location reopened;
5. faction battle won/lost;
6. faction mobilization;
7. treaty signed;
8. treaty broken;
9. trade route opened;
10. trade route closed;
11. food shortage;
12. medicine shortage;
13. fuel shortage;
14. unusual surplus;
15. resource discovery;
16. hazardous discovery;
17. expedition disappeared;
18. weather warning;
19. player-caused rescue/attack consequence;
20. deliberate faction disinformation.

Each template declares:

- eligible source events;
- allowed channels;
- expiry class;
- mutable fields;
- forbidden mutations;
- default credibility policy;
- optional localization keys;
- subject-resolution rules.

---

# 13. Suggested Query API

The information layer should expose queries, not mutable public collections:

```text
GetClaimsAtLocation(locationId, filters)
GetClaimsAboutSubject(subjectType, subjectId, filters)
GetPlayerKnownClaims(filters)
GetClaimVariants(eventId)
GetClaimProvenance(claimId)
GetVerificationOptions(claimId)
GetSellEligibility(claimId, recipientId)
GetCorroborationSummary(claimId)
```

Consumer commands remain outside the information system:

```text
QuestAuthority.TryUnlockFromInformation(...)
EconomyAuthority.ApplyPerceptionSignal(...)
KnowledgeAuthority.TryRevealHint(...)
StandingAuthority.ApplyDeceptionConsequence(...)
ExpeditionAuthority.TryCreateVerificationObjective(...)
```

Exact API names should follow repository conventions.

---

# 14. Save Section Contract

Persist only what must survive:

- active claim records;
- bounded retained history;
- recipient/player exposure;
- pending deliveries whose transport semantics require persistence;
- verification/disproval state;
- disinformation campaign references if this layer owns them;
- deterministic scheduler/RNG state when required.

Do not persist:

- resolved domain truth;
- current prices;
- faction standing;
- route openness;
- weather;
- location control;
- quest progress;
- expedition state;
- derived indexes;
- UI sorting/filter state unless UI conventions already persist it separately.

---

# 15. Test Matrix

## Determinism

- same seed + same events → same claims;
- same seed + same save/reload point → same final state;
- stable ordering regardless of dictionary iteration;
- mutation and fan-out deterministic.

## Propagation

- caravan delivery;
- caravan lost before delivery;
- radio in range;
- radio out of range;
- hostile boundary;
- isolated node;
- duplicate multi-path arrival;
- corroboration.

## Mutation

- valid allowed operator;
- immutable field preserved;
- bounded hop count;
- invalid attribution prevented;
- fabricated marker preserved.

## Aging

- freshness bands;
- expiry;
- supersedence;
- pruning;
- no consumer effect after expiry where prohibited.

## Consumers

- quest hint;
- economy perception;
- location reveal hint;
- standing consequence;
- broker sale;
- verification objective.

## Save

- old save empty default;
- round-trip;
- pending delivery;
- mid-mutation state;
- verified/disproved;
- no duplicate consequence after reload.

## Exploit

- repeated sale;
- repeated verification;
- radio rebroadcast;
- caravan circuit;
- price-rumor loop;
- quest-rumor loop.

---

# 16. Performance Targets

The exact thresholds should be benchmarked against current project scale, but the implementation should target:

- O(1)-average claim lookup by ID;
- indexed lookup by subject and recipient/location;
- priority/timing queue for scheduled deliveries;
- no scan of all locations for every claim on every tick;
- no scan of all claims for every location on every tick;
- bounded mutation work per delivery;
- bounded provenance;
- deterministic pruning;
- save size proportional to configured active/retained caps rather than campaign age.

Record median and p95 tick cost under the stress soak.

---

# 17. Rollback Strategy

If the information-flow layer causes late regressions:

1. disable new propagation while preserving save compatibility;
2. continue loading and presenting already-known claims if safe;
3. disable individual downstream adapters independently;
4. disable broker sales independently;
5. disable disinformation independently;
6. keep canonical world systems unaffected;
7. never rewrite world state to “undo” rumors;
8. preserve old-save empty/default migration;
9. leave the save section reader in place until all compatible saves are migrated or retired.

This modular rollback is one of the reasons adapters must remain separate from the core claim runtime.

---

# 18. Metrics

Track at minimum:

- authoritative events eligible for information generation;
- claims generated;
- claims delivered by caravan;
- claims delivered by radio;
- claims blocked by topology/hostility;
- average propagation latency by channel;
- active claim count;
- duplicate arrival suppression count;
- average claim age;
- expired/pruned count;
- corroboration rate;
- verification rate;
- disproval rate;
- disinformation exposure rate;
- information-board open rate;
- quest hints created from information;
- location hints revealed;
- broker sales and blocked duplicate sales;
- rumor-induced economy signal duration;
- standing consequences from proven deception;
- median/p95 tick time;
- save-section size.

The most important player-value metric is not “rumors generated.” It is how often information changes a
meaningful decision before direct observation.

---

# 19. Follow-On Scope Gates

Do not begin these until the first slice passes acceptance:

## Encrypted communications

Requires real radio access/encryption authority and a reason to create decryption gameplay.

## Propaganda campaigns

Requires faction strategy/policy ownership and bounded campaign resources.

## Information black markets

Requires stable broker transactions, recipient-knowledge queries, and economy safeguards.

## Refugee information

Requires refugee/travel population rails and should reuse the same carrier interface as caravans/travelers.

## Broker specialization

Requires player/survivor skill progression rails and proven baseline demand for information trading.

These are separate plans, not “while we are here” additions.

---

# 20. Completion Checklist

- [ ] Premise audit completed at current HEAD.
- [ ] Existing information/radio/gossip/knowledge authorities mapped.
- [ ] New durable information state justified.
- [ ] World truth is never duplicated in rumor state.
- [ ] Claim/provenance model is bounded and deterministic.
- [ ] Event adapters exist for approved source domains.
- [ ] Physical propagation uses canonical routes/caravans.
- [ ] Radio propagation uses canonical radio/intel rails.
- [ ] Aging, expiry, and supersedence work.
- [ ] Bias and mutation are bounded by template rules.
- [ ] Deliberate disinformation cannot change truth directly.
- [ ] Player knowledge stores claims, not world truth.
- [ ] Verification distinguishes false from stale.
- [ ] Quest integration uses quest authority.
- [ ] Economy integration uses economy authority.
- [ ] Expedition/location reveal uses knowledge/expedition authority.
- [ ] Standing consequences use standing authority.
- [ ] Broker selling is novelty- and cooldown-gated.
- [ ] Rumor Boa

<!-- Deliverable capped to remain inside the requested 50–90k character envelope. -->
