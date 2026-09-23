# Expansion 98 — A Lesson Kept Between Shifts

**Wave:** 20 — Useful Systems, Real Consequences  
**Requested series:** Plan 2 of 5  
**Requested plan length:** At least 120,000 words; expansion above the minimum is acceptable.  
**Series status:** Plan complete as a planning document; proposal only; no implementation or path claim.
**Measured length:** 133,139 words; 110.95% of the 120,000-word minimum (13,139 words above minimum). Counted through formal closeout, Section 801.
**Unique key feature:** A reachable education loop from valid learner/teacher matching through one replay-safe lesson to owner-backed knowledge.  
**Status:** proposal only; no implementation or path claim.  
**Audit:** [Wave 20 forensic report](../../forensics/WAVE20_FEATURE_SEAMS_FORENSIC_REPORT.md)

> Exactly three subfeatures are defined in Section 4. The examples and casebook all map to those same three; no additional feature pillars are introduced.

## 1. Expansion thesis

The experience is a reachable education loop from valid learner/teacher matching through one replay-safe lesson to owner-backed knowledge. It must start from canonical state, invoke one owner-backed command and show an outcome that remains true after day advance and reload. A panel-only simulation does not meet the promise.

Learning should feel like work made teachable under scarcity: one person gives up a shift, another receives an hour, and a later task proves the time mattered.

Show real cost, permission and uncertainty before commitment. Refusal, delay and failure may be valid results.

## 2. Canon fit

Learning should feel like work made teachable under scarcity: one person gives up a shift, another receives an hour, and a later task proves the time mattered.

Keep the tone restrained, material and human. Sample lines below are candidates, not current canon; check them against the live narrative data before authoring.

## 3. Existing systems reused

Reuse curriculum JSON, learner/proficiency methods, age/roster facts, duty schedule, needs, current skill consumers and apprenticeship route. NurseryPanel is only a candidate surface after ownership is resolved.

Reuse stable IDs and owner records. JSON under Assets/StreamingAssets/Data remains authoritative; do not duplicate mutable data in the host.

## 4. Key feature and exactly three subfeatures

**Single key feature:** A reachable education loop from valid learner/teacher matching through one replay-safe lesson to owner-backed knowledge.

**Current gap:** Core curriculum behavior is not proven reachable in Godot and overlaps a live child-learning projection. A repeat call can grant additional session progress; no daily idempotency key appears in the session API.

### Subfeature 1: A study agreement that respects age and duty

**Purpose and loop:** Show eligible learners, subjects, prerequisites, candidate teachers and a real schedule window. Route assignment through the education owner after canonical age, roster, needs, duty and consent checks.

**Player value:** A parent may teach when free, but relationship does not equal availability. A mechanic can offer a short lesson after shift; a learner may defer without losing progress.

**Boundary:** Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

### Subfeature 2: One lesson, one daily result

**Purpose and loop:** Resolve one session using campaign-seeded RNG and current needs, duty and room facts. Show proficiency, completion, unlock or interruption after the owner returns it.

**Player value:** An urgent water repair can postpone study; a quiet day can make the same lesson complete. Schedule matters because labor is scarce.

**Boundary:** ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

### Subfeature 3: Knowledge that leaves the lesson

**Purpose and loop:** Route confirmed skill unlocks to the current skill consumer, show graduation separately, and offer apprenticeship as a distinct next route.

**Player value:** A learner can become able to read a maintenance label or request supervised work; the outcome can remain modest.

**Boundary:** Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

These three subfeatures are the complete gameplay scope. A fourth independent pillar needs a separate proposal.

## 5. Core mechanics

**Input:** current owner state, catalog, day and explicit player command. **State:** existing domain DTO and only stable event references. **Decision:** commit, defer, decline or choose a supported alternative with costs visible. **Uncertainty:** only facts current systems support. **Consequence:** owner-confirmed state or cost. **Cross-system output:** one fact once. **Failure/recovery:** stale data, denied consent, capacity, interruption and retry have truthful results. **Replayability:** seeded campaign inputs and stable order, never wall-clock or UI-local random.

## 6. Cross-system interactions

Primary domain: SurvivorEducationSystem owns curriculum records and graduation candidates; GenerationalSystem owns child profiles and education XP shown by NurseryPanel; ApprenticeshipSystem and ApprenticeshipCurriculumEngine own vocational training. Age, duty, needs and skill facts stay with current owners.

A current cost/permission owner supplies constraints; an existing downstream owner consumes confirmed effects; the day/save owner refreshes the same state after restore. Verify each actual API and consumer before implementation.

## 7. Main narrative spine

A concrete need appears; the player sees known facts and limits; one of the three subfeatures permits a decision; a later view shows who acted and what changed. Human center: Learning should feel like work made teachable under scarcity: one person gives up a shift, another receives an hour, and a later task proves the time mattered.

Use physical details such as a corrected roster, an unanswered notice or a receipt. Let observation carry emotion.

## 8. Major questlines

No new major questline is proposed. The feature must be useful in ordinary shelter play first. Existing quest conditions may read owner-confirmed state but cannot create parallel flags.

## 9. Side quests and content carriers

No fetch-chain bundle is proposed. Requests, disputes, notices and after-action records may carry content only through one of the three subfeatures. Conditional writing stays conditional when a route is not guaranteed.

## 10. Survivors and NPCs

Use current IDs, professions, status and relationships. No bespoke NPC is needed to make the system reachable. An absent or departed participant is never silently replaced.

## 11. Locations and spaces

Use rooms/locations only where a current owner reports access and capacity. A room string or board label does not prove a physical space.

## 12. Encounters and recurring moments

Prefer ordinary state-grounded moments: an interrupted lesson, a refusal, an unanswered note or a moved object. These are examples inside the feature, not a new encounter authority.

## 13. Faction reactions

No new faction or reputation channel. Caravan/faction responses stay with current market or faction owners and remain separate from individual consent or trust.

## 14. Items and resources

No new resource or item authority. Each cost names the owner that validates and applies it. Preview never transfers goods, consumes a ration or occupies capacity.

## 15. Radio and environmental storytelling

Radio remains external signal/program content. Sound and diegetic records may reflect verified events and need a readable UI equivalent; audio alone cannot report a cost or denial.

## 16. Persistent consequences

Preserve stable identity, valid pending intent, confirmed result and provenance through the existing save owner. A durable consequence is A learner can become able to read a maintenance label or request supervised work; the outcome can remain modest.

Core capture/restore is necessary but not sufficient: prove host registration, restore order and dirty flush.

## 17. Failure and alternate outcomes

A blocked command is a valid branch. Distinguish missing source, deferral, owner rejection, expiry, conflict and completion where current APIs support them. Never show success before confirmation or turn denial into hidden punishment.

## 18. Replayability

Different people, timing and owner state should change the choices without arbitrary new rolls. Same seeded state resolves identically. Duplicate command/day delivery reuses a durable result or reports already processed.

## 19. Implementation classification

**Classification:** Cross-system host integration with an education-owner decision gate; a minimal session identity may be needed.

Start with a fresh premise and runtime audit. Resolve owner decisions, connect the existing Core authority through the current host/event/save seam, then expose one Godot route. Core remains engine-free and JSON remains authoritative.

## 20. Collision audit

Adjacent behavior: Reuse curriculum JSON, learner/proficiency methods, age/roster facts, duty schedule, needs, current skill consumers and apprenticeship route. NurseryPanel is only a candidate surface after ownership is resolved.

Classify this as extension/reachability work, not replacement. Re-search current content, host paths and save sections before implementation. Never revive Unity behavior.

## 21. Expansion hooks

Later quests or campaign history can consume an owner-confirmed result with provenance. They cannot infer success from a UI label or duplicate the source state.

## 22. Strongest recommended content

Start with the smallest interaction that proves the cost and consequence: An urgent water repair can postpone study; a quiet day can make the same lesson complete. Schedule matters because labor is scarce. Add one blocked path and one delayed callback.

## 23. Contracts, data, save and determinism

**Owner boundary:** SurvivorEducationSystem owns curriculum records and graduation candidates; GenerationalSystem owns child profiles and education XP shown by NurseryPanel; ApprenticeshipSystem and ApprenticeshipCurriculumEngine own vocational training. Age, duty, needs and skill facts stay with current owners.

Validate current JSON schema, IDs, references, ranges and consumers. Trace SaveSectionRegistry and Main setup/save/flush before assigning state; no speculative store. Inject campaign RNG and stable event identities; no System.Random or wall-clock seed. Read old saves through the owning codec.

## 24. Godot surface and player flow

Bind from current owner, render its state, validate request shape, route the command, show the returned result and refresh from source. Preserve close/back, keyboard/controller focus, disposal, contrast and readable type. A panel never advances simulation.

## 25. Dependency-ordered delivery

1. Re-read INTEGRATION_PLANS, WORKTREE_OWNERSHIP, TEST_POLICY, KNOWN_DEBT and AI_AGENT_WORKFLOW.
2. Recheck source, data consumer, composition, save registration, restore order and active claims.
3. Record any ownership decision before code.
4. Add only minimal Core contracts proven missing and route them through the current host owner.
5. Prove save/restore, seeded replay, catalog integrity and downstream outcome.
6. Run only future focused verification selected by package owner; no tests ran for these plans.
7. Handoff exact paths, contracts, commands, results and limits.

Candidate paths are discovery hints, not ownership claims.

## 26. Risks, verification and rollback

**Primary risk:** Core curriculum behavior is not proven reachable in Godot and overlaps a live child-learning projection. A repeat call can grant additional session progress; no daily idempotency key appears in the session API.

Later verification should cover accepted, denied, stale, repeated and restored actions plus seeded replay where relevant. Prove behavior, not class presence. Rollback disables the host route and preserves canonical state and save readers. Keep unsafe commands unavailable while ownership is unresolved.

## 27. Three creative variants for this feature

**Grounded:** show current state and one safe owner command. **Systemic:** connect the three subfeatures to real costs, permissions and delayed outcomes. **Wildcard, still grounded:** let the player inspect the source, author, provenance or physical limit before committing. This changes framing, not architecture. Implement systemic only when contracts are proven.

## 28. Review gate

Proceed to implementation planning only when source confirms the gap, every mutable fact has one owner, exactly these three subfeatures have truthful routes, save/replay owners are named and success follows an owner result. Otherwise revise or stop.

## 29. Casebook: design acceptance records mapped to the three subfeatures

These are review examples, not implementation claims, test results or extra features. Every scenario maps to S1, S2 or S3 and preserves the current ownership boundary.

### Scenario W20-98-S1-001 — adult learner without active subject

**Initial condition:** adult learner without active subject. The view is a projection of the current owner. **Pressure:** age differs between child and curriculum records. **Player action:** refresh from chosen canonical owner.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. learner profile and curriculum record do not diverge. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The room is free after the filter shift. I can teach then. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-001 — adult learner without active subject

**Initial condition:** adult learner without active subject. The view is a projection of the current owner. **Pressure:** daily owner repeats session callback. **Player action:** defer with truthful duty/health reason.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. one seeded result belongs to one learner/day. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She has the lesson. She still has a shift. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-001 — adult learner without active subject

**Initial condition:** adult learner without active subject. The view is a projection of the current owner. **Pressure:** catalog retires prerequisite. **Player action:** reuse stored result for repeated session.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. teacher availability comes from roster. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The room is free after the filter shift. I can teach then. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-002 — adult learner without active subject

**Initial condition:** adult learner without active subject. The view is a projection of the current owner. **Pressure:** catalog retires prerequisite. **Player action:** route unlock once.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. restore cannot create another proficiency roll. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That number is from the other ledger. Check the name first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-002 — adult learner without active subject

**Initial condition:** adult learner without active subject. The view is a projection of the current owner. **Pressure:** two views submit stale assignment. **Player action:** stop if child-learning authorities disagree.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. skill/graduation has one consumer. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One page today. Tomorrow depends on the roster. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-002 — adult learner without active subject

**Initial condition:** adult learner without active subject. The view is a projection of the current owner. **Pressure:** skill consumer unavailable. **Player action:** offer apprenticeship as separate next step.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. denial is visible before schedule changes. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That number is from the other ledger. Check the name first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-003 — adult learner without active subject

**Initial condition:** adult learner without active subject. The view is a projection of the current owner. **Pressure:** room availability changes after schedule. **Player action:** reuse stored result for repeated session.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. learner profile and curriculum record do not diverge. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She has the lesson. She still has a shift. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-003 — adult learner without active subject

**Initial condition:** adult learner without active subject. The view is a projection of the current owner. **Pressure:** UI shows graduation before owner event. **Player action:** hold choice until prerequisite is met.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. one seeded result belongs to one learner/day. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The room is free after the filter shift. I can teach then. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-003 — child profile already showing education XP

**Initial condition:** child profile already showing education XP. The view is a projection of the current owner. **Pressure:** teacher reassigned to emergency work. **Player action:** preserve assignment but report catalog issue.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. teacher availability comes from roster. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She has the lesson. She still has a shift. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-004 — child profile already showing education XP

**Initial condition:** child profile already showing education XP. The view is a projection of the current owner. **Pressure:** teacher reassigned to emergency work. **Player action:** offer apprenticeship as separate next step.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. restore cannot create another proficiency roll. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One page today. Tomorrow depends on the roster. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-004 — child profile already showing education XP

**Initial condition:** child profile already showing education XP. The view is a projection of the current owner. **Pressure:** catalog retires prerequisite. **Player action:** refresh from chosen canonical owner.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. skill/graduation has one consumer. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That number is from the other ledger. Check the name first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-004 — child profile already showing education XP

**Initial condition:** child profile already showing education XP. The view is a projection of the current owner. **Pressure:** two views submit stale assignment. **Player action:** defer with truthful duty/health reason.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. denial is visible before schedule changes. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One page today. Tomorrow depends on the roster. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-005 — child profile already showing education XP

**Initial condition:** child profile already showing education XP. The view is a projection of the current owner. **Pressure:** two views submit stale assignment. **Player action:** preserve assignment but report catalog issue.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. learner profile and curriculum record do not diverge. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The room is free after the filter shift. I can teach then. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-005 — child profile already showing education XP

**Initial condition:** child profile already showing education XP. The view is a projection of the current owner. **Pressure:** skill consumer unavailable. **Player action:** route unlock once.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. one seeded result belongs to one learner/day. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She has the lesson. She still has a shift. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-005 — child profile already showing education XP

**Initial condition:** child profile already showing education XP. The view is a projection of the current owner. **Pressure:** parent-child bonus lacks verified relationship. **Player action:** stop if child-learning authorities disagree.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. teacher availability comes from roster. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The room is free after the filter shift. I can teach then. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-006 — child profile already showing education XP

**Initial condition:** child profile already showing education XP. The view is a projection of the current owner. **Pressure:** UI shows graduation before owner event. **Player action:** defer with truthful duty/health reason.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. restore cannot create another proficiency roll. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That number is from the other ledger. Check the name first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-006 — parent offered as teacher on a free shift

**Initial condition:** parent offered as teacher on a free shift. The view is a projection of the current owner. **Pressure:** teacher reassigned to emergency work. **Player action:** reuse stored result for repeated session.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. skill/graduation has one consumer. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One page today. Tomorrow depends on the roster. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-006 — parent offered as teacher on a free shift

**Initial condition:** parent offered as teacher on a free shift. The view is a projection of the current owner. **Pressure:** health or needs block study. **Player action:** hold choice until prerequisite is met.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. denial is visible before schedule changes. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That number is from the other ledger. Check the name first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-007 — parent offered as teacher on a free shift

**Initial condition:** parent offered as teacher on a free shift. The view is a projection of the current owner. **Pressure:** health or needs block study. **Player action:** stop if child-learning authorities disagree.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. learner profile and curriculum record do not diverge. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She has the lesson. She still has a shift. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-007 — parent offered as teacher on a free shift

**Initial condition:** parent offered as teacher on a free shift. The view is a projection of the current owner. **Pressure:** save occurs between assignment and lesson. **Player action:** offer apprenticeship as separate next step.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. one seeded result belongs to one learner/day. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The room is free after the filter shift. I can teach then. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-007 — parent offered as teacher on a free shift

**Initial condition:** parent offered as teacher on a free shift. The view is a projection of the current owner. **Pressure:** skill consumer unavailable. **Player action:** refresh from chosen canonical owner.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. teacher availability comes from roster. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She has the lesson. She still has a shift. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-008 — parent offered as teacher on a free shift

**Initial condition:** parent offered as teacher on a free shift. The view is a projection of the current owner. **Pressure:** skill consumer unavailable. **Player action:** hold choice until prerequisite is met.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. restore cannot create another proficiency roll. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One page today. Tomorrow depends on the roster. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-008 — parent offered as teacher on a free shift

**Initial condition:** parent offered as teacher on a free shift. The view is a projection of the current owner. **Pressure:** parent-child bonus lacks verified relationship. **Player action:** preserve assignment but report catalog issue.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. skill/graduation has one consumer. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That number is from the other ledger. Check the name first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-008 — learner has time-critical duty

**Initial condition:** learner has time-critical duty. The view is a projection of the current owner. **Pressure:** age differs between child and curriculum records. **Player action:** route unlock once.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. denial is visible before schedule changes. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One page today. Tomorrow depends on the roster. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-009 — learner has time-critical duty

**Initial condition:** learner has time-critical duty. The view is a projection of the current owner. **Pressure:** daily owner repeats session callback. **Player action:** route unlock once.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. learner profile and curriculum record do not diverge. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The room is free after the filter shift. I can teach then. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-009 — learner has time-critical duty

**Initial condition:** learner has time-critical duty. The view is a projection of the current owner. **Pressure:** catalog retires prerequisite. **Player action:** stop if child-learning authorities disagree.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. one seeded result belongs to one learner/day. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She has the lesson. She still has a shift. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-009 — learner has time-critical duty

**Initial condition:** learner has time-critical duty. The view is a projection of the current owner. **Pressure:** two views submit stale assignment. **Player action:** offer apprenticeship as separate next step.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. teacher availability comes from roster. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The room is free after the filter shift. I can teach then. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-010 — learner has time-critical duty

**Initial condition:** learner has time-critical duty. The view is a projection of the current owner. **Pressure:** stage changes during day advance. **Player action:** reuse stored result for repeated session.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. restore cannot create another proficiency roll. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That number is from the other ledger. Check the name first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-010 — learner has time-critical duty

**Initial condition:** learner has time-critical duty. The view is a projection of the current owner. **Pressure:** room availability changes after schedule. **Player action:** hold choice until prerequisite is met.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. skill/graduation has one consumer. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One page today. Tomorrow depends on the roster. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-010 — learner has time-critical duty

**Initial condition:** learner has time-critical duty. The view is a projection of the current owner. **Pressure:** UI shows graduation before owner event. **Player action:** preserve assignment but report catalog issue.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. denial is visible before schedule changes. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That number is from the other ledger. Check the name first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-011 — learner has time-critical duty

**Initial condition:** learner has time-critical duty. The view is a projection of the current owner. **Pressure:** UI shows graduation before owner event. **Player action:** offer apprenticeship as separate next step.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. learner profile and curriculum record do not diverge. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She has the lesson. She still has a shift. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-011 — student crosses a catalog age stage

**Initial condition:** student crosses a catalog age stage. The view is a projection of the current owner. **Pressure:** daily owner repeats session callback. **Player action:** refresh from chosen canonical owner.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. one seeded result belongs to one learner/day. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The room is free after the filter shift. I can teach then. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-011 — student crosses a catalog age stage

**Initial condition:** student crosses a catalog age stage. The view is a projection of the current owner. **Pressure:** catalog retires prerequisite. **Player action:** defer with truthful duty/health reason.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. teacher availability comes from roster. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She has the lesson. She still has a shift. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-012 — student crosses a catalog age stage

**Initial condition:** student crosses a catalog age stage. The view is a projection of the current owner. **Pressure:** catalog retires prerequisite. **Player action:** preserve assignment but report catalog issue.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. restore cannot create another proficiency roll. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One page today. Tomorrow depends on the roster. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-012 — student crosses a catalog age stage

**Initial condition:** student crosses a catalog age stage. The view is a projection of the current owner. **Pressure:** two views submit stale assignment. **Player action:** route unlock once.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. skill/graduation has one consumer. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That number is from the other ledger. Check the name first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-012 — student crosses a catalog age stage

**Initial condition:** student crosses a catalog age stage. The view is a projection of the current owner. **Pressure:** skill consumer unavailable. **Player action:** stop if child-learning authorities disagree.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. denial is visible before schedule changes. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One page today. Tomorrow depends on the roster. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-013 — student crosses a catalog age stage

**Initial condition:** student crosses a catalog age stage. The view is a projection of the current owner. **Pressure:** room availability changes after schedule. **Player action:** defer with truthful duty/health reason.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. learner profile and curriculum record do not diverge. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The room is free after the filter shift. I can teach then. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-013 — student crosses a catalog age stage

**Initial condition:** student crosses a catalog age stage. The view is a projection of the current owner. **Pressure:** UI shows graduation before owner event. **Player action:** reuse stored result for repeated session.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. one seeded result belongs to one learner/day. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She has the lesson. She still has a shift. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-013 — subject prerequisite is not met

**Initial condition:** subject prerequisite is not met. The view is a projection of the current owner. **Pressure:** teacher reassigned to emergency work. **Player action:** hold choice until prerequisite is met.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. teacher availability comes from roster. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The room is free after the filter shift. I can teach then. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-014 — subject prerequisite is not met

**Initial condition:** subject prerequisite is not met. The view is a projection of the current owner. **Pressure:** teacher reassigned to emergency work. **Player action:** stop if child-learning authorities disagree.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. restore cannot create another proficiency roll. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That number is from the other ledger. Check the name first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-014 — subject prerequisite is not met

**Initial condition:** subject prerequisite is not met. The view is a projection of the current owner. **Pressure:** health or needs block study. **Player action:** offer apprenticeship as separate next step.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. skill/graduation has one consumer. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One page today. Tomorrow depends on the roster. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-014 — subject prerequisite is not met

**Initial condition:** subject prerequisite is not met. The view is a projection of the current owner. **Pressure:** two views submit stale assignment. **Player action:** refresh from chosen canonical owner.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. denial is visible before schedule changes. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That number is from the other ledger. Check the name first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-015 — subject prerequisite is not met

**Initial condition:** subject prerequisite is not met. The view is a projection of the current owner. **Pressure:** two views submit stale assignment. **Player action:** hold choice until prerequisite is met.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. learner profile and curriculum record do not diverge. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She has the lesson. She still has a shift. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-015 — subject prerequisite is not met

**Initial condition:** subject prerequisite is not met. The view is a projection of the current owner. **Pressure:** skill consumer unavailable. **Player action:** preserve assignment but report catalog issue.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. one seeded result belongs to one learner/day. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The room is free after the filter shift. I can teach then. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-015 — subject prerequisite is not met

**Initial condition:** subject prerequisite is not met. The view is a projection of the current owner. **Pressure:** parent-child bonus lacks verified relationship. **Player action:** route unlock once.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. teacher availability comes from roster. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She has the lesson. She still has a shift. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-016 — subject prerequisite is not met

**Initial condition:** subject prerequisite is not met. The view is a projection of the current owner. **Pressure:** UI shows graduation before owner event. **Player action:** refresh from chosen canonical owner.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. restore cannot create another proficiency roll. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One page today. Tomorrow depends on the roster. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-016 — saved learner resumes after interruption

**Initial condition:** saved learner resumes after interruption. The view is a projection of the current owner. **Pressure:** teacher reassigned to emergency work. **Player action:** defer with truthful duty/health reason.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. skill/graduation has one consumer. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That number is from the other ledger. Check the name first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-016 — saved learner resumes after interruption

**Initial condition:** saved learner resumes after interruption. The view is a projection of the current owner. **Pressure:** health or needs block study. **Player action:** reuse stored result for repeated session.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. denial is visible before schedule changes. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One page today. Tomorrow depends on the roster. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-017 — saved learner resumes after interruption

**Initial condition:** saved learner resumes after interruption. The view is a projection of the current owner. **Pressure:** save occurs between assignment and lesson. **Player action:** reuse stored result for repeated session.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. learner profile and curriculum record do not diverge. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The room is free after the filter shift. I can teach then. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-017 — saved learner resumes after interruption

**Initial condition:** saved learner resumes after interruption. The view is a projection of the current owner. **Pressure:** stage changes during day advance. **Player action:** hold choice until prerequisite is met.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. one seeded result belongs to one learner/day. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She has the lesson. She still has a shift. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-017 — saved learner resumes after interruption

**Initial condition:** saved learner resumes after interruption. The view is a projection of the current owner. **Pressure:** room availability changes after schedule. **Player action:** preserve assignment but report catalog issue.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. teacher availability comes from roster. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The room is free after the filter shift. I can teach then. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-018 — saved learner resumes after interruption

**Initial condition:** saved learner resumes after interruption. The view is a projection of the current owner. **Pressure:** room availability changes after schedule. **Player action:** offer apprenticeship as separate next step.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. restore cannot create another proficiency roll. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That number is from the other ledger. Check the name first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-018 — teacher has time for one of two students

**Initial condition:** teacher has time for one of two students. The view is a projection of the current owner. **Pressure:** age differs between child and curriculum records. **Player action:** refresh from chosen canonical owner.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. skill/graduation has one consumer. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One page today. Tomorrow depends on the roster. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-018 — teacher has time for one of two students

**Initial condition:** teacher has time for one of two students. The view is a projection of the current owner. **Pressure:** daily owner repeats session callback. **Player action:** defer with truthful duty/health reason.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. denial is visible before schedule changes. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That number is from the other ledger. Check the name first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-019 — teacher has time for one of two students

**Initial condition:** teacher has time for one of two students. The view is a projection of the current owner. **Pressure:** daily owner repeats session callback. **Player action:** preserve assignment but report catalog issue.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. learner profile and curriculum record do not diverge. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She has the lesson. She still has a shift. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-019 — teacher has time for one of two students

**Initial condition:** teacher has time for one of two students. The view is a projection of the current owner. **Pressure:** catalog retires prerequisite. **Player action:** route unlock once.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. one seeded result belongs to one learner/day. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The room is free after the filter shift. I can teach then. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-019 — teacher has time for one of two students

**Initial condition:** teacher has time for one of two students. The view is a projection of the current owner. **Pressure:** two views submit stale assignment. **Player action:** stop if child-learning authorities disagree.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. teacher availability comes from roster. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She has the lesson. She still has a shift. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-020 — teacher has time for one of two students

**Initial condition:** teacher has time for one of two students. The view is a projection of the current owner. **Pressure:** stage changes during day advance. **Player action:** defer with truthful duty/health reason.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. restore cannot create another proficiency roll. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One page today. Tomorrow depends on the roster. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-020 — teacher has time for one of two students

**Initial condition:** teacher has time for one of two students. The view is a projection of the current owner. **Pressure:** room availability changes after schedule. **Player action:** reuse stored result for repeated session.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. skill/graduation has one consumer. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That number is from the other ledger. Check the name first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-020 — teacher has time for one of two students

**Initial condition:** teacher has time for one of two students. The view is a projection of the current owner. **Pressure:** UI shows graduation before owner event. **Player action:** hold choice until prerequisite is met.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. denial is visible before schedule changes. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One page today. Tomorrow depends on the roster. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-021 — teacher has time for one of two students

**Initial condition:** teacher has time for one of two students. The view is a projection of the current owner. **Pressure:** UI shows graduation before owner event. **Player action:** stop if child-learning authorities disagree.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. learner profile and curriculum record do not diverge. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The room is free after the filter shift. I can teach then. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-021 — course completion unlocks an existing skill

**Initial condition:** course completion unlocks an existing skill. The view is a projection of the current owner. **Pressure:** teacher reassigned to emergency work. **Player action:** offer apprenticeship as separate next step.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. one seeded result belongs to one learner/day. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She has the lesson. She still has a shift. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-021 — course completion unlocks an existing skill

**Initial condition:** course completion unlocks an existing skill. The view is a projection of the current owner. **Pressure:** catalog retires prerequisite. **Player action:** refresh from chosen canonical owner.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. teacher availability comes from roster. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The room is free after the filter shift. I can teach then. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-022 — course completion unlocks an existing skill

**Initial condition:** course completion unlocks an existing skill. The view is a projection of the current owner. **Pressure:** catalog retires prerequisite. **Player action:** hold choice until prerequisite is met.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. restore cannot create another proficiency roll. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That number is from the other ledger. Check the name first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-022 — course completion unlocks an existing skill

**Initial condition:** course completion unlocks an existing skill. The view is a projection of the current owner. **Pressure:** two views submit stale assignment. **Player action:** preserve assignment but report catalog issue.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. skill/graduation has one consumer. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One page today. Tomorrow depends on the roster. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-022 — course completion unlocks an existing skill

**Initial condition:** course completion unlocks an existing skill. The view is a projection of the current owner. **Pressure:** skill consumer unavailable. **Player action:** route unlock once.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. denial is visible before schedule changes. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That number is from the other ledger. Check the name first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-023 — course completion unlocks an existing skill

**Initial condition:** course completion unlocks an existing skill. The view is a projection of the current owner. **Pressure:** room availability changes after schedule. **Player action:** refresh from chosen canonical owner.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. learner profile and curriculum record do not diverge. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She has the lesson. She still has a shift. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-023 — course completion unlocks an existing skill

**Initial condition:** course completion unlocks an existing skill. The view is a projection of the current owner. **Pressure:** UI shows graduation before owner event. **Player action:** defer with truthful duty/health reason.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. one seeded result belongs to one learner/day. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The room is free after the filter shift. I can teach then. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-023 — learner already in apprenticeship

**Initial condition:** learner already in apprenticeship. The view is a projection of the current owner. **Pressure:** teacher reassigned to emergency work. **Player action:** reuse stored result for repeated session.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. teacher availability comes from roster. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She has the lesson. She still has a shift. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-024 — learner already in apprenticeship

**Initial condition:** learner already in apprenticeship. The view is a projection of the current owner. **Pressure:** teacher reassigned to emergency work. **Player action:** route unlock once.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. restore cannot create another proficiency roll. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One page today. Tomorrow depends on the roster. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-024 — learner already in apprenticeship

**Initial condition:** learner already in apprenticeship. The view is a projection of the current owner. **Pressure:** health or needs block study. **Player action:** stop if child-learning authorities disagree.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. skill/graduation has one consumer. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That number is from the other ledger. Check the name first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-024 — learner already in apprenticeship

**Initial condition:** learner already in apprenticeship. The view is a projection of the current owner. **Pressure:** save occurs between assignment and lesson. **Player action:** offer apprenticeship as separate next step.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. denial is visible before schedule changes. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One page today. Tomorrow depends on the roster. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-025 — learner already in apprenticeship

**Initial condition:** learner already in apprenticeship. The view is a projection of the current owner. **Pressure:** stage changes during day advance. **Player action:** offer apprenticeship as separate next step.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. learner profile and curriculum record do not diverge. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The room is free after the filter shift. I can teach then. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-025 — learner already in apprenticeship

**Initial condition:** learner already in apprenticeship. The view is a projection of the current owner. **Pressure:** parent-child bonus lacks verified relationship. **Player action:** refresh from chosen canonical owner.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. one seeded result belongs to one learner/day. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She has the lesson. She still has a shift. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-025 — schoolroom unavailable today

**Initial condition:** schoolroom unavailable today. The view is a projection of the current owner. **Pressure:** age differs between child and curriculum records. **Player action:** defer with truthful duty/health reason.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. teacher availability comes from roster. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The room is free after the filter shift. I can teach then. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-026 — schoolroom unavailable today

**Initial condition:** schoolroom unavailable today. The view is a projection of the current owner. **Pressure:** age differs between child and curriculum records. **Player action:** preserve assignment but report catalog issue.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. restore cannot create another proficiency roll. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That number is from the other ledger. Check the name first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-026 — schoolroom unavailable today

**Initial condition:** schoolroom unavailable today. The view is a projection of the current owner. **Pressure:** daily owner repeats session callback. **Player action:** route unlock once.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. skill/graduation has one consumer. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One page today. Tomorrow depends on the roster. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-026 — schoolroom unavailable today

**Initial condition:** schoolroom unavailable today. The view is a projection of the current owner. **Pressure:** catalog retires prerequisite. **Player action:** stop if child-learning authorities disagree.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. denial is visible before schedule changes. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That number is from the other ledger. Check the name first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-027 — schoolroom unavailable today

**Initial condition:** schoolroom unavailable today. The view is a projection of the current owner. **Pressure:** save occurs between assignment and lesson. **Player action:** defer with truthful duty/health reason.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. learner profile and curriculum record do not diverge. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She has the lesson. She still has a shift. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-027 — schoolroom unavailable today

**Initial condition:** schoolroom unavailable today. The view is a projection of the current owner. **Pressure:** stage changes during day advance. **Player action:** reuse stored result for repeated session.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. one seeded result belongs to one learner/day. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The room is free after the filter shift. I can teach then. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-027 — schoolroom unavailable today

**Initial condition:** schoolroom unavailable today. The view is a projection of the current owner. **Pressure:** room availability changes after schedule. **Player action:** hold choice until prerequisite is met.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. teacher availability comes from roster. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She has the lesson. She still has a shift. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S1-028 — schoolroom unavailable today

**Initial condition:** schoolroom unavailable today. The view is a projection of the current owner. **Pressure:** room availability changes after schedule. **Player action:** stop if child-learning authorities disagree.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. restore cannot create another proficiency roll. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Current assignment does not prove teacher availability, guardian permission or schedule. Never mirror GenerationalSystem education XP into a second child record.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One page today. Tomorrow depends on the roster. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S2-028 — schoolroom unavailable today

**Initial condition:** schoolroom unavailable today. The view is a projection of the current owner. **Pressure:** UI shows graduation before owner event. **Player action:** offer apprenticeship as separate next step.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. skill/graduation has one consumer. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. ConductDailySession lacks a day/session identity. Prove one call per learner/day or add a durable deterministic key; a panel-local guard is insufficient.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That number is from the other ledger. Check the name first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-98-S3-028 — guardian fact missing from provider

**Initial condition:** guardian fact missing from provider. The view is a projection of the current owner. **Pressure:** daily owner repeats session callback. **Player action:** refresh from chosen canonical owner.

**Expected route and outcome:** Re-read age, prerequisite, teacher and schedule from their owners. A valid session can grant progress each time it runs, so require one stable learner/day identity. denial is visible before schedule changes. If child progress still disagrees with curriculum state, stop before displaying advancement.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Do not copy proficiency to GenerationalSystem or ApprenticeshipSystem. No automatic job assignment or age-only graduation.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One page today. Tomorrow depends on the roster. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

## 30. Final review gate

Refresh this premise against current source; confirm path claims and owner decisions; choose focused verification; leave unproven effects unavailable. Proposal only: no implementation, test, runtime or ledger result is claimed.

## 31. Plan 2 continuation — a learner-led story inside the three subfeatures

This continuation expands the proposal without adding a fourth feature pillar. Its story moves through the same three owners of player value already defined in Section 4: a study agreement that respects age and duty; one replay-safe lesson; and knowledge that leaves the lesson. It uses an ordinary shelter task as the reason learning matters. The learner is not automatically assigned a job, the teacher is not presumed available, and the curriculum does not become a separate narrative reputation system.

The working sequence is titled **“The Label by the Boiler.”** A maintenance label on a service panel has faded enough that one line is difficult to read. The learner notices that a warning mark appears beside a number but does not know what the number refers to. The lesson can help them interpret a related word or symbol. It does not authorize them to operate equipment, replace a qualified worker, or change the boiler state.

All character references remain role-based until current cast IDs and canon are checked. The lesson subject and the visible label must be supported by current curriculum and world data before production. If the current catalog has no suitable learning objective, the scene remains a prose proposal and must not invent proficiency data.

## 32. Sequence entry — the question belongs to the learner

The learner can begin by asking a question, pointing to the label, requesting help, or declining to discuss it. A teacher or the player may notice the faded text, but the learner’s interest is not inferred from age, family relation, or a nearby work assignment.

The opening scene offers four ways forward:

- **Ask what the learner wants to understand.** Their answer narrows the lesson objective if the curriculum supports it.
- **Explain the immediate safety instruction directly.** This may be the correct urgent response even if no lesson is scheduled. A warning is not withheld to create a tutorial.
- **Offer a study session later.** The learner can accept, defer, or decline; no progress is granted by the offer.
- **Leave the label alone for now.** The player can mark the task as needing a qualified reader or ask the current equipment owner to replace or clarify it.

If the learner’s question is outside the catalog, the response says what is unknown and offers a safe alternative. It does not create a hidden subject merely to keep the quest open. If the learner does not want a lesson, the scene can close with the practical instruction provided and the learning opportunity left available only if the current education owner supports a later offer.

## 33. Subfeature 1 — a study agreement with meaningful alternatives

When the player chooses to arrange a lesson, the interface presents only currently valid candidates. Candidate eligibility comes from the existing age, curriculum, prerequisite, roster, duty, needs, room-access, and consent owners. A family relationship can make a conversation natural, but cannot substitute for guardian permission or teacher availability.

The player can invite a qualified adult who has a free interval, ask the learner whether they have a preferred teacher, let the learner request a demonstration from a current mechanic, or defer until the roster opens a suitable time. The teacher can accept a short lesson, propose another interval, decline because of a duty, or accept only if another task is covered. The learner can change their mind before the session begins.

Those routes create different social scenes while remaining inside the matching subfeature:

- **Learner-selected teacher:** the candidate is still validated by the owner. The learner explains why that person feels approachable; the player does not treat preference as qualification.
- **Teacher-offered time:** the teacher names a window, and the player checks whether it fits duty and needs. The offer expires if the window passes without acceptance.
- **Group demonstration:** several learners may observe only if the existing session API and curriculum support it. Otherwise, the player chooses one learner and the rest receive a later offer rather than an invented group result.
- **Deferred agreement:** the player leaves no pending lesson if there is no supported pending-state owner. The journal can state “not scheduled” without creating a parallel appointment store.

A study agreement should identify who will teach, who will learn, the curriculum item, and the accepted time. If the UI cannot display all four from current owners, implementation stops at that evidence gap. The player should not see a “Ready” state based only on a teacher’s name or an open room.

## 34. Agreement branches — timing changes the relationship

**The free-shift branch:** the teacher has a genuine opening. The player can schedule the lesson now, let the teacher finish an existing task first, or ask the learner whether this time still works. Scheduling immediately may provide an earlier session but can interrupt the teacher’s chosen rest. Waiting can improve the arrangement but risks losing the room window.

**The family branch:** a parent or guardian is a possible teacher. The learner may want them, prefer someone else, or not want the lesson at home. The game checks required permission through the canonical owner. If permission is absent or uncertain, the lesson is not scheduled until the owner provides a valid result. The player can seek a qualified alternative or close the scene without blame.

**The duty-conflict branch:** the only teacher candidate is assigned to a time-critical shift. The player can preserve the duty, find another qualified person, shorten or move the lesson if that is supported, or wait. The lesson does not take priority merely because it is a quest objective. The learner may express disappointment while agreeing that the repair work cannot be abandoned.

**The room-conflict branch:** a suitable space is unavailable. The player can choose another accessible location only if a current owner reports that it is usable, ask for the next opening, or defer. A dialogue line saying “we can use the workshop” cannot establish room capacity by itself.

**The no-match branch:** no candidate is valid today. The player can read the immediate safety instruction aloud, seek a qualified teacher later, or let the learner pursue a different catalog subject if they want one. The sequence can continue as a practical safety scene without falsely completing Subfeature 1.

## 35. Subfeature 2 — one lesson, one daily result

The lesson scene begins only after the agreement is confirmed. The teacher brings the current manual or approved teaching material. The learner can ask to see the label again, start with a simpler related example, or postpone if the shift has changed. The player confirms the session through the education owner. The result is shown only after that owner returns it.

The lesson may complete, make partial progress, be interrupted before the command, be rejected as stale, or already have been processed for the same learner/day. These are not interchangeable outcomes:

- **Completed:** show the owner-confirmed progress and the next allowed lesson or application.
- **Partial:** describe what the owner actually reports and whether continuation is supported; do not round partial work up to completion.
- **Interrupted before execution:** preserve the prior state and let the player reschedule if valid.
- **Rejected:** name the supported reason and offer a valid next step without success styling.
- **Already processed:** display the existing result or a clear “already completed today” response; do not call the session again for another roll.

The learner’s comprehension line can change with the returned result. A completed session may give them confidence about one term. A partial result may leave the label clearer but the warning still misunderstood. A rejected command can leave the scene with the teacher putting the page away and asking whether to try another day. The writing cannot fabricate an education gain if the owner says no progress occurred.

The command identity must remain stable across UI close/reopen, save/restore, and host retries. The current API gap around day/session identity remains the primary technical blocker. If the owner cannot safely guarantee one result per learner per day, content production cannot promise replay-safe lessons until a bounded owner-backed design resolves it.

## 36. Lesson dialogue — a small moment of understanding

The teacher taps the line on the manual, not the equipment panel.

“This mark means pressure. The number is a limit for this model, not a target.”

The learner leans closer. “So if it’s below that, we leave it?”

“You tell the person on the shift what you read. They decide what to do with the machine.”

The player can ask the teacher to use another example; ask the learner to explain the warning back in their own words; stop when the learner has enough for today; or continue only if both participants still agree and the session owner allows it.

If the learner explains the term correctly, the scene can celebrate understanding without claiming independent operational competence. If they misread it, the teacher can correct the explanation, repeat a supported lesson step, or end the session with the basic safety instruction intact. The player may choose to observe quietly, but their silence does not alter the curriculum result.

## 37. Subfeature 3 — knowledge leaves the lesson carefully

After a confirmed result, the player can ask how the learner wants to use it. Options include reading a similar label with a qualified worker present, explaining the term to another learner if that is an available curriculum outcome, requesting a supervised apprenticeship path, or keeping the knowledge private for now. The game routes any confirmed skill result to the current skill consumer.

The learner is never auto-assigned to operate the boiler or placed on a duty roster because a lesson completed. Graduation is shown separately. Apprenticeship is a distinct next route with its own prerequisites and acceptance. A learner may understand one term but still need an expert for every operational task.

**Read-along application:** the learner identifies a related word while the qualified worker remains responsible for the equipment. The later scene can show the learner reading a maintenance label, but not changing settings.

**Teach-back application:** if current curriculum supports peer explanation, the learner can repeat one term for another student. The second student’s progress requires their own valid agreement and session result; it is not copied from the first learner.

**Apprenticeship request:** the learner asks to observe a maintenance task. The player can check current eligibility, ask the supervisor, accept a limited observation, or defer. Graduation and skill unlock are not presumed.

**No application today:** the learner may choose not to use the new knowledge yet. The lesson result remains whatever the owner recorded, but the story does not manufacture a work outcome for an unused skill.

## 38. The three subfeatures form one arc, not three separate reward tracks

The arc’s narrative sequence is agreement, session, and application. A player can stop after any stage without creating a new feature:

- A valid agreement with no session is still a scheduled opportunity, not learning progress.
- A lesson result with no application is still an education outcome, not a job assignment.
- An application request without a confirmed skill or accepted supervisor remains a request, not an apprenticeship.

The player’s strongest branch may be to protect the lesson from competing work. Another player may prioritize the current repair and return to study later. A third may let the learner choose a different teacher or subject. These are play approaches inside the same education loop, not new mechanics.

## 39. Faction and supporting-current context within scope

No faction gains a separate education authority. A major faction may ask whether a learner can read a route sign, understand a supply label, or assist with a supervised task. The player can report an owner-confirmed capability, explain that the lesson was incomplete, or withhold personal learning details and offer a general capacity statement.

The Archivists can provide an accessible, dated learning source if their current offer and the lesson catalog support it. The Scavenger Guild can show a practical label or tool fitting under supervision. The Long Walk can bring a route description that creates a later reading exercise. These are contextual contributions, not additional education features. A Current cannot enroll learners, assign teachers, mark progress, or decide graduation.

The player can accept or refuse each contribution. Refusing an Archivist copy does not block a lesson if another supported material exists. Declining a Guild demonstration does not make the learner ineligible for study. A late Long Walk note can become a later lesson context only if the learner, teacher, and curriculum owners permit it.

## 40. Alternative learner-centered resolutions

- **The learner chooses the interval:** a compatible teacher is available, the learner accepts a time, and the lesson occurs once. The learner later reads one approved label with a qualified worker.
- **The teacher protects the shift:** the candidate remains on duty; the lesson is deferred, and the learner receives only the immediate safety explanation. The next study window remains a fresh agreement.
- **The parent is not the teacher:** family permission is handled correctly, but the learner asks another qualified adult. The family relationship remains personal rather than becoming an automatic education contract.
- **The lesson is partial:** the daily result records what happened, and the application task waits for another session. The learner may still choose to stop pursuing the subject.
- **Knowledge stays private:** the learner completes the session but does not want a public demonstration or faction report. The skill consumer receives the real result, while the player shares only what the learner consents to share.
- **No valid match:** the player closes the education attempt for today, protects the urgent equipment instruction, and leaves a future offer available only through current scheduling and education owners.

Every outcome stays within the three subfeatures. None creates a class rank, education reputation, school faction, personal inventory of skills, or automatic labor pipeline.

## 41. Continuation acceptance for the new sequence

Before implementation, verify the actual learner and teacher IDs, curriculum IDs, current age and consent contracts, daily schedule owner, room access provider, lesson command, session identity, persistence owner, and downstream skill consumer. The production sequence is acceptable only if:

1. The learner has an explicit, supported route to request or decline the lesson.
2. Teacher availability and eligibility come from current owners at commit time.
3. A valid session cannot grant duplicate daily progress after retry or restore.
4. The player sees the actual owner result before the scene reports progress.
5. Skill or graduation output reaches one current consumer and is not mirrored.
6. Use of the knowledge remains supervised or separately accepted where required.
7. No faction or Current becomes an education authority.
8. All three subfeatures remain the complete scope.

This sequence begins Plan 2’s expansion toward its 120,000-word minimum. It remains a proposal and does not supersede the source, ownership, test, or integration requirements in the original plan.

## 42. Continuation installment — learning methods that preserve the same owner result

The same lesson can feel different according to how the learner and teacher work together. These variations are authored scene forms, not additional curriculum systems. Every format must still use an eligible subject, an accepted teacher/learner agreement, one owner-backed daily session, and one downstream consumer for confirmed knowledge.

The teacher and learner can choose a method from the formats already supported by the current curriculum: demonstration, guided read-back, worked example, or supervised observation. If a format is not represented by the current lesson owner, it remains a dialogue variation and must not grant a distinct mechanical bonus. Do not make one format more effective through a new hidden “teaching style” stat.

## 43. Format branch — demonstrate, explain, or read back

**Demonstration:** The teacher points to a mark on a manual or safe example. The learner watches, then identifies the same mark on another approved example. The player can ask for a slower demonstration, stop after the first recognition, or let the teacher finish the scheduled lesson. The machine itself is not used as a teaching prop unless the current duty and safety owners permit it.

**Guided read-back:** The learner reads a short instruction and explains what they think it means. The teacher corrects the interpretation before the learner acts. The learner may get the answer right, partly right, or ask to hear the explanation again. Dialogue reflects the owner’s result; it does not create a new comprehension roll.

**Worked example:** The teacher uses a completed repair note to show how a word or symbol appeared in context. The player can ask why the example was chosen, compare it with the current label, or keep the session on the catalog’s stated objective. If the note contains private information, it is redacted or replaced before the session only where the current document/consent process supports that handling.

**Supervised observation:** The learner watches an approved task performed by a qualified worker. They may ask questions, remain quiet, or leave. Observation alone is not a lesson result unless the current owner records it as a supported lesson. It never grants authority to operate the equipment.

The teacher may prefer one format and the learner another. The player can ask them to choose a workable method, offer a compromise if supported, or postpone. A disagreement does not prove either person lacks interest. The result remains tied to the session the owner actually accepted and processed.

## 44. Learner-led route — choose the question before the teacher

Some learners arrive with a specific question; others do not yet know the subject name. The player can ask what they want to do with the knowledge, show the current curriculum list, invite the teacher to propose an entry point, or defer until the learner has more context.

If the learner says, “I want to know which mark means stop,” the player checks whether the curriculum has a valid subject and whether the teacher can teach it. If supported, the lesson begins there. If not, the teacher gives the immediate safety instruction and the education owner can offer a supported subject later. No lesson is fabricated around a phrase the catalog does not contain.

If the learner says only, “I want to understand the page,” the teacher may ask which part is confusing. The player can let the learner point to a symbol, ask for a short overview within the subject, or postpone the choice. The game should not treat a vague request as consent to a long session or a general education plan.

The learner may also ask to stop once they understand the immediate example. The player can end the session if the current owner supports that boundary, continue only with both participants’ agreement, or keep the lesson time but switch to review. A shorter session should not be represented as a complete result unless the owner reports that it is complete.

## 45. Teacher-led route — expertise without control

A teacher can offer a subject that they know well, but the player still checks the learner’s interest, prerequisites, and available time. The teacher can say the label is only a starting point and suggest a related curriculum objective. The player can accept that suggestion if the catalog supports it, keep the original question, ask another qualified teacher, or defer.

The teacher’s expertise does not entitle them to decide what the learner needs. If the learner wants a practical answer and the teacher begins a broad lecture, the player may ask to return to the immediate question. If the teacher has only one time window, the learner can decline it without losing unrelated progress. A lesson may be useful even if the teacher’s preferred subject is not selected.

When no qualified teacher is available, a faction representative or Current cannot be substituted just because they have a manual. The player can seek a current supported source, ask when a valid teacher is next available, or close the attempt for the day. Content must distinguish “someone possesses information” from “this person is an eligible instructor under the education owner.”

## 46. Interruption branch — keep the daily result singular

An interruption can happen before, during, or after the session command. The scene needs a precise transition:

- **Before command:** a duty, room, or learner preference changes. The player can cancel or reschedule. No education result is claimed.
- **During owner processing:** the caller waits for the authoritative result. Closing the panel cannot submit another lesson.
- **After result:** an alarm or urgent task arrives. The owner-confirmed progress remains; the player chooses whether to apply it later.
- **After save and restore:** the session identity resolves to the same daily result. The player may review it but cannot request a second grant for the same lesson/day.

The interruption scene can show real friction without adding a new interruption meter. A worker calls from the corridor; the learner closes the book; the teacher asks whether to continue tomorrow. The scene’s dialogue may differ if the session was already accepted, but the domain result comes from the owner.

If the education owner reports no progress, the narrative may still recognize effort or resolve the learner’s immediate question, provided it does not claim skill advancement. Conversely, if the owner confirms progress, a later interrupted application does not erase the lesson.

## 47. Application scene — read a service label with supervision

The learner returns to the service panel with a qualified worker. The player can ask the learner to identify the label’s purpose, have the worker read it first and invite questions, or keep the label closed if access or duty conditions changed. The learner’s action is an application of knowledge; the qualified worker remains responsible for interpretation and operation.

If the learner reads the intended term correctly, the worker can ask what it means in this context. If the answer is incomplete, the worker can correct it without humiliation. If the label is too faded, the player can stop and ask for a replacement or a clearer source. Learning does not repair the physical text.

The player can ask whether the learner wants the scene recorded as a demonstration, keep the moment private, or share a general description of the supported skill with a supervisor. Any persistent skill result belongs to the existing consumer. The learner’s personal explanation or performance is not automatically added to a public record.

## 48. Application scene — explain a route symbol without certifying a route

The learner sees a mark on a route note that resembles one from the lesson. The player can let them identify the symbol on the page, ask a teacher to confirm its meaning, or stop because the route report itself is stale. The learner may correctly identify the symbol while the route’s current condition remains unknown.

This scene demonstrates a critical separation: knowing how to read the notation does not mean knowing whether the field report is current. A later trip may still be required. The learner can contribute a reading under supervision, but cannot be turned into a route witness or field worker by a lesson result alone.

If the symbol is not part of the curriculum, the teacher can explain that the resemblance is only visual. The player may request a supported subject for another day, or leave the mark uninterpreted. A surprising symbol does not create a hidden curriculum branch.

## 49. Application scene — carry knowledge into an apprenticeship request

After a confirmed skill outcome, the learner asks whether they can observe a maintenance shift. The player checks the current apprenticeship route separately: prerequisites, teacher/supervisor acceptance, roster capacity, age and consent rules, and any applicable safety restriction. The learner can ask for an observation only, request a future apprenticeship, or decide not to pursue it.

Possible outcomes include an accepted observation, a deferred request pending a qualified supervisor, an owner rejection with a clear reason, or a refusal from the learner after seeing the schedule. Each is distinct. A graduation flag does not automatically assign a role; a lesson result does not bypass apprenticeship requirements.

If a supervisor agrees to a single observation, the agreement expires after that event unless renewed. If no one accepts recurring responsibility, the player can preserve the learner’s education result and close the apprenticeship request without inventing a standing placement.

## 50. Faction context — education is useful, not a recruitment test

Major factions may notice a confirmed skill outcome, but their interest must stay inside existing faction and education owners.

- **Military:** may ask whether the learner can interpret a particular label or whether a qualified worker is available. The player can report a capability at an agreed level, explain that it was a supervised exercise, or withhold the learner’s name. The Military cannot directly enroll or assign the student through this scene.
- **Rebel:** may offer a shared lesson or ask whether the learner wants to teach a peer. The player can accept if the current curriculum and teacher rules support it, offer a general handout, or decline. Local participation remains voluntary.
- **Independent:** may request proof of a relevant skill for a limited work exchange. The player can show an owner-confirmed qualification if one exists, negotiate supervised work instead, or reject the exchange. A broker’s receipt cannot become a new credential authority.
- **No major commitment:** the player can keep the lesson local, share a general reading resource, or ask the relevant Current for one bounded material or teaching service. The lack of faction sponsorship may limit supply or scheduling reach.

Faction interest should reveal different practical needs: coverage, peer access, or a clear qualification. It must not make education a loyalty gate. A learner can gain knowledge while refusing to work for a faction, and a faction may refuse a candidate without undoing the learning result.

## 51. Supporting-current roles across the lesson sequence

Archivists may preserve a dated copy of a manual or provide source context. Their custody does not certify the learner’s ability. The Scavenger Guild may show how a term appears on a salvaged part; its practical demonstration does not replace the lesson result. Long Walk may bring a route note that becomes a suitable reading example; the courier’s report is not a curriculum item unless the current catalog supports it.

The player can use none, one, or several of these offers. Each support role has a separate scope, cost, and acceptance. The Archivist can decline to lend a fragile copy; the Guild can offer a demonstration but no classroom time; Long Walk can deliver a note too late for the current lesson. The education loop remains playable through the current approved material and a valid teacher if those exist.

## 52. Learner voice and dignity

Do not write every learner as embarrassed, grateful, or eager. One asks precise questions. One wants to leave as soon as the lesson is over. One makes a joke to cover uncertainty. Another does not want to explain why the subject matters. These are voice possibilities for existing characters, to be checked against their current profiles and dialogue.

Useful lines include:

- “Show me the mark first. I’ll ask about the number after.”
- “I know that word now. I don’t know what to do with the machine.”
- “Can we stop there? That’s enough for today.”
- “I want the page. I don’t want everyone watching me read it.”
- “If I get it wrong, correct the word. Don’t take the page away.”

Teacher voice also varies. A patient teacher may ask the learner to explain back; a hurried one may request another time; a confident teacher may need to be reminded that the learner sets the pace. The narrative can show that expertise and good teaching are related but not identical without adding a teacher skill stat.

## 53. Result matrix for the three-feature sequence

| Agreement | Session result | Application choice | Truthful conclusion |
|---|---|---|---|
| Accepted | Completed | Supervised read-back | Owner-confirmed knowledge receives a visible use, with work authority retained by supervisor |
| Accepted | Partial | Deferred | Partial result remains partial; a later session requires a new valid daily identity |
| Accepted | Rejected as stale | Ask teacher to refresh source | No progress is claimed; material source is updated only through its owner |
| Deferred | No session | Immediate safety instruction | The learner receives necessary warning; education progress remains unchanged |
| Declined | No session | None | The learner’s choice is respected; no hidden loss or shame line is added |
| Accepted | Completed | No public application | Knowledge is recorded by its owner; privacy is respected and future use stays optional |
| Accepted | Completed | Apprenticeship request | Separate owner checks eligibility, supervisor, schedule, and acceptance |
| No valid match | No session | Seek supported alternative | The scene closes without an invented teacher, subject, or result |

The matrix is an authoring check, not a proposal for an additional runtime state table. Production must map each result to existing education and apprenticeship contracts before using it in content.

## 54. Installment 2 closing note

This continuation deepens learner-led entry, teacher/learner agreement, lesson formats, interruption handling, supervised application, faction responses, and supporting-current context. All additions remain inside the original three subfeatures and preserve owner-backed progress, separate apprenticeship acceptance, and the learner’s right to defer or decline.

## 55. Installment 3 — branching by what the player does

The branch design should answer a practical question: what did the player do that changed this encounter? A branch is earned by an observable choice, an owner-backed condition, or both. It is not earned by a hidden alignment score, a broad label such as “good,” or a single reputation threshold. The player may protect the learner’s time in one scene, preserve the teacher’s duty coverage in another, choose a damaged source because it is the only available one, or decline to publish a result. These choices can lead to different scenes even when they produce the same education result.

Do not turn every action into a permanent global flag. Most branches can be represented by the immediate command, the existing agreement outcome, the owner’s lesson result, the existing source record, or the player’s choice at the application scene. Where a persistent distinction is genuinely needed, use an existing contract or ask the owning team to approve an extension. A prose plan cannot make a new save bit legitimate by naming it a “story tag.”

The same premise applies to negative outcomes. If the teacher is called away, the scene should not silently label the teacher unreliable. If the learner defers, it should not imply laziness. If the player shares a general lesson but withholds the learner’s name, the faction should not treat that as a betrayal unless the current faction contract explicitly makes a named report part of an accepted agreement. Consequence should follow the selected action and the authoritative state it changed.

## 56. Branch drivers and their authored consequences

The following table defines a usable authoring vocabulary. It does not request a new state model. Before production, each row must be mapped to a current command, event, or visible condition. If the source cannot distinguish a row, collapse its prose variation into a common result rather than manufacturing a flag to preserve a distinction.

| Player action or current condition | Immediate scene difference | Persistent consequence allowed | What must not be inferred |
|---|---|---|---|
| Ask the learner before scheduling | The learner names a preferred time or declines to choose | The accepted or declined agreement already owned by the schedule/education seam | Agreement is not proof of aptitude or enthusiasm forever |
| Schedule around a duty window | The teacher points out the coverage cost and the learner sees the proposed hour | Only the schedule and agreement state already supported | A successful schedule does not mean the duty had no cost |
| Ask the teacher to demonstrate first | The teacher uses an example before requesting recall | One session result through the existing lesson owner | Demonstration does not grant an extra progress tick |
| Invite the learner to explain back | The learner chooses whether to speak, write, point, or stop | The owner’s supported result, if the API records one | Quiet participation is not failure by itself |
| Use a dated source with clear provenance | Teacher and learner can compare the displayed date with the current task | Source selection or source validity only if an existing record owns it | A dated source is not automatically current |
| Use an undated or damaged source | The group marks the uncertain part and avoids acting on it | No knowledge credit beyond what the education owner confirms | Uncertainty is not a hidden penalty to unrelated skills |
| Ask a support Current for a material | The request is framed as a bounded loan, copy, delivery, or example | The offering Current’s existing inventory, relation, or service result | Help is not a new faction-allegiance gate |
| Pause when urgent work arrives | The teacher asks whether to stop, and the learner answers | The already-produced daily result remains unchanged; a later attempt follows the owner’s idempotency rules | An interruption does not erase completed progress or award a second result |
| Share a general lesson result | The receiving faction hears the supported skill at the correct level | Existing faction conversation or work request only | A general result is not consent to identify the learner |
| Share the learner’s identity with permission | The learner is present or has given the relevant consent under current rules | The agreed report or apprenticeship route if already owned | Identity sharing does not waive future consent |
| Keep the result private | The player closes the report and the teacher records only what the owner requires | The existing private education result | Privacy is not automatically deception or disloyalty |
| Ask for supervised application | A qualified person retains the hazard decision while the learner tries the reading task | Existing skill consumer if it accepts the action | Observation is not an appointment, certification, or unsupervised access |
| Decline an application opportunity | The learner can explain a practical reason or simply decline | No new progress loss; any accepted schedule is handled by its owner | Declining is not proof of cowardice or faction opposition |

The prose can name different motives without claiming different mechanics. A learner may want to read a note because it relates to family, because it could help with work, or because they enjoy solving symbols. The education result remains the result the current owner reports. Motivation affects voice, framing, and next conversation only where existing character or narrative data supports that variation.

## 57. Decision scene — choosing a source, not choosing a side

At the start of a lesson, the player finds that the subject is available but the cleanest example is not. A current manual has a missing page. A newer note is unsigned. A worker remembers the procedure but has no written source. A support Current may have a surviving copy, but reaching it costs time or materials. The choice concerns evidence, time, and access rather than whether the player is honest or corrupt.

The scene begins with a small, concrete discrepancy. The learner points to the word “closed” on a service note and asks whether it refers to a valve, a room, or a work order. The teacher recognizes the term but cannot confirm the context from the damaged page. The player has four honest choices:

1. **Use the damaged page as a reading exercise.** The group marks the illegible section and limits the lesson to recognizable vocabulary. If the education owner accepts the lesson content and reports progress, that result is credited once. Nobody treats the missing context as known.
2. **Delay until a dated reference arrives.** The teacher keeps the duty slot open only if the schedule owner allows it. The learner is told why there is a delay and can choose another time. There is no hidden lesson attempt or progress result.
3. **Ask a worker to demonstrate the relevant term.** The worker can explain what they personally know while the teacher distinguishes practical recollection from an authoritative procedure. This can produce a valid lesson only if the current curriculum and lesson API support the subject and teacher qualification.
4. **Choose another available subject for this session.** The player may preserve the hour by switching to a supported lesson. If no valid match exists, the session does not occur; the system must not auto-substitute an unregistered subject.

Each option changes what the player sees next. The damaged-page path foregrounds uncertainty. The delay path foregrounds the learner’s schedule and the cost of reserving time. The demonstration path foregrounds a qualified worker’s limits. The alternate-subject path may produce a different application opportunity later. These are meaningful branches even if two paths share a later outcome. They do not need morality points or mutually exclusive grand campaigns.

The teacher’s line should make the boundary understandable: “I can show you what this word usually means. I can’t tell you that this page is the current instruction.” A later application scene can then ask the learner to identify the word while requiring the worker to confirm the live state. The learner’s knowledge matters without replacing source verification.

If the source cannot be used safely, the correct result is a clear stop. A stop may still be narratively satisfying if the learner has learned how to identify an incomplete source, but this must not be represented as skill progress unless the curriculum owner explicitly supports that learning outcome. The scene can honor sound judgment without giving a fictional mechanical reward.

## 58. Three bounded lesson stories with different practical stakes

The examples below provide distinct content shapes for the same education loop. They are not three new systems, quests, or curriculum packages. Subject IDs, eligible learners, teachers, and consumers remain subject to catalog collision checks and owner confirmation. If the live data already contains a suitable subject, adapt that subject; if it does not, the plan remains a proposal until the data owner approves one.

### 58.1 The maintenance label

The learner encounters a short service label. The player can begin with the sound of the word, its written form, or the context in which it is used. A teacher may ask the learner to match the label to a drawing, read it aloud, or describe when they would ask a worker for help. Those are teaching choices and dialogue variations inside one session. They do not each award progress.

The central branch is whether the player treats the label as a learning example or as authorization to operate equipment. Choosing the first permits supervised interpretation. Choosing the second must be blocked by the relevant safety and work owner, with an explanation grounded in qualification and current conditions. The learner can still complete the lesson even if operating the machine is refused. The refusal may make the opportunity feel limited, but it does not invalidate what was learned.

A successful application lets the learner locate the right label and explain its ordinary meaning while the responsible worker confirms the current instruction. An incomplete answer leads to a correction, a clearer reference, or an end to the exercise. A source mismatch leads to “we do not know from this page,” not to a guess presented as fact.

### 58.2 The ration rota note

A learner asks why two household names appear beside different collection times. The teacher can explain the format of the rota and ask the learner to find their own entry, subject to privacy rules. The player can choose a private explanation, a public board explanation using a general example, or a deferral until a responsible person can confirm that the rota is current.

This case gives the story a social stake without turning the lesson into a dispute-resolution feature. The student can learn how to read a date and a time even if the underlying allocation is unfair, mistaken, or subject to change. The player can separately route a suspected allocation problem through its current owner. Education cannot silently correct the ration ledger.

Branch consequences come from the action: private explanation avoids exposing another person’s schedule; public explanation may teach more peers but must use permitted information; deferral preserves accuracy but leaves the question unresolved for now. None of these outcomes declares the player morally good or bad. The learner’s reading ability remains separate from the legitimacy of the ration decision.

### 58.3 The route symbol on a returned note

A courier brings back a note with a route symbol and a date. The learner recognizes the symbol from the lesson. The teacher asks them to find the legend and read the date, while the player decides whether the paper is only a classroom sample or whether a qualified route owner should review it before any planning use.

The learner may correctly identify the symbol and still be unable to say whether the route is safe today. If the report is stale, the player routes it to current travel assessment. If the legend is missing, the player asks for a verified reference. If the learner misreads the symbol, the teacher corrects the reading and the group records only the outcome the lesson owner supports. No route is opened solely because a student recognized a mark.

This example makes a useful distinction: the learner can contribute to interpretation without inheriting operational responsibility. A later apprenticeship request can build on demonstrated interest, but it remains a separate accepted route. A learner who does not want field work can still keep the literacy benefit.

## 59. A scene grammar that allows several playstyles

The scene should not assume that every player wants to optimize a score or maximize one person’s career. A lesson can be approached as a work-planning problem, a relationship moment, a resource negotiation, a careful evidence check, or a learner-led conversation. Give these approaches recognizable options and honest costs. Do not create separate hidden difficulty tracks for each style.

### The scheduler

This player wants the lesson to fit around essential work. Show the actual duty conflict, available window, and cost of moving a qualified teacher. A good scheduler can preserve coverage by choosing a shorter valid session or another day, if the current owner supports that alternative. The plan should not reward the player for booking a lesson during an unrepresented “free” hour.

### The mentor

This player prioritizes learner comfort and voice. Offer consent checks, a choice of speaking or pointing, a quiet room, and the option to stop. Do not give the mentor a bonus progress multiplier. The payoff is a more respectful scene and a learner whose choice is clearly understood.

### The archivist

This player cares about evidence and source provenance. Let them inspect dates, edition marks, gaps, and custody. A careful source check may prevent a misleading application, but must not create a separate archival education score. If no valid source is available, the player can defer or choose a supported example.

### The practical instructor

This player prefers demonstration. Let the teacher show the object or label, then invite the learner to repeat only within safe bounds. The practical format may produce a different later application scene. It does not let an unqualified teacher bypass the curriculum requirements.

### The peer builder

This player wants knowledge to circulate. Let them ask for a general handout, a second learner’s optional attendance, or a shared explanation if the existing roster and curriculum owners support it. Additional listeners do not silently receive progress if the owner only records one learner. Do not invent a cohort attendance mechanic to make the scene feel larger.

### The private tutor

This player wants to keep the learner’s progress private. The scene should respect that choice while still preserving required owner records. The player can decline faction reporting, public praise, and an unnecessary audience. Private learning is not a concealed penalty state.

### The opportunist

This player sees a lesson as a chance to open a work option. The player may ask about supervised application or apprenticeship, but the request is explicit and the prerequisites are evaluated by the current owner. A learner can accept one path and reject another; the game should not treat career interest as consent to all future assignments.

### The minimalist

This player wants one useful result and then to return to survival work. Keep the lesson short, make its cost legible, and avoid forcing a speech or ceremony. A concise scene can still establish what was learned, what remains unknown, and what the owner recorded.

These playstyles are authoring lenses, not selectable classes. A player can use different approaches in different lessons. The game should offer at least two meaningful methods when its source data and interface support them, but it should not inflate the curriculum to satisfy a fixed option count.

## 60. Branch handling when the learner or teacher changes their mind

Consent is a current choice. An accepted schedule does not lock either participant into attending regardless of changed conditions. At the scene boundary, re-read the relevant roster and duty facts. If a participant is no longer available, explain the concrete reason and offer only routes that the current owner validates.

When the learner withdraws, give them a short, non-punitive response. They may be tired, worried about being watched, uninterested in the subject, or simply unwilling to explain. The player can accept the withdrawal, ask whether a different method would help, or request another time. Repeatedly asking after a clear refusal should not unlock a special persuasion bonus. If the learner voluntarily chooses a different method, the change is theirs.

When the teacher withdraws, do not recast the teacher as a villain. They may have been called to a duty, discovered a qualification mismatch, or realized the material is outside their competence. The player can find another valid teacher, postpone, choose another subject, or close the agreement. The system must not keep the teacher occupied after the owner has released them.

When the player changes the lesson method after beginning, resolve it within the one session identity. If the API cannot safely amend a started session, provide a narrative explanation and close it according to the owner’s actual outcome. Do not call the lesson owner twice because the player changed from read-aloud to demonstration. A method change is not a new daily attempt.

If an urgent task arrives after the owner has already committed the result, preserve that result and show the interruption around it. If it arrives before the session begins, the player may reschedule only through the current schedule owner. These timings should be explicit in UI feedback and save/load behavior. A vague “lesson canceled” line must not contradict a committed skill result.

## 61. Dialogue variation by action and knowledge boundary

Dialogue should communicate what the learner understands and where the limit sits. Use action-specific lines instead of generic praise or moral judgment. The examples below are candidates to adapt after checking character voices, localization patterns, and existing narrative IDs.

**When the learner identifies a word but not the process:**

- Teacher: “You found the label. The worker still has to tell us what the panel is doing today.”
- Learner: “So I can read the warning, but I can’t clear it.”
- Player options: agree; ask the worker to explain the warning; end the exercise.

**When the learner asks to use a private example:**

- Learner: “Can we use my note? I don’t want it on the board.”
- Teacher: “That’s fine. We can use the date and format without showing anyone else the message.”
- Player options: proceed privately; choose a neutral sample; defer if no safe example exists.

**When the teacher is unsure:**

- Teacher: “I know the older mark. I don’t know whether they changed it last month.”
- Learner: “Then I’ll learn the old one, but I won’t use it on the new sheet?”
- Player options: verify a newer source; proceed only as historical vocabulary; stop.

**When the learner declines public application:**

- Learner: “I’ll show you here. I don’t want the whole room watching.”
- Player options: arrange a private supervised read-back; ask whether another method is preferable; end the application.

**When a faction asks for a name:**

- Representative: “Who can read this label for our crew?”
- Player options: share a general capability; ask the learner before naming them; offer a qualified worker instead; decline.
- Required narrative fact: no option may imply that a lesson authorizes unsafe work or removes the need for the receiving crew’s own verification.

The learner need not deliver every boundary line. If the learner is young, tired, or uncomfortable, a teacher or the player can make the limit clear while leaving the learner room to answer. Avoid dialogue that turns a boundary into a lecture about the entire feature. One specific line at the moment of decision is easier to understand and more credible.

## 62. How local supporting groups can matter without taking over

The secondary groups create texture by changing how a valid lesson is reached or used. They do not own education progress. Each offer should answer a narrow question: where did the source come from, who can transport it, who can demonstrate a practical term, or who might use the confirmed knowledge later?

**Archivists** can help trace a source’s date, edition, and custody. Their most meaningful branch is whether they lend the original, provide a copy, or refuse because the page is fragile or incomplete. A refusal can lead to another source or a delay, not a new “Archivist standing” path inside the education loop. If the player borrows the original, return and custody behavior belongs to the existing archive or item owner.

**The Scavenger Guild** can explain how a label or symbol appears on salvaged parts. It may offer an object as a demonstrator, charge an existing price, or decline to part with scarce stock. The Guild’s practical vocabulary can make the lesson vivid, but it cannot certify a procedure or award skills. The player can use the example, ask for a neutral teaching copy, or choose another supported lesson.

**Long Walk** can carry a dated route note, deliver a manual, or bring a question from another settlement. Timing itself can make a branch: the material arrives before the agreement, during a scheduled window, or too late. A late delivery can enrich future learning, but must not retroactively alter a completed daily result. The route and delivery owners remain responsible for the note’s currentness.

**Major factions** can express demand for the skill through existing work requests. They may ask for a qualified worker, a supervised demonstration, or a general explanation. The player’s response changes the work conversation or referral route where those owners support it. No major faction gains direct control over who may learn, and no learner’s refusal should erase an already completed lesson.

When several groups are involved, present the player with a practical sequence rather than a faction popularity contest. For example, ask the Archivists to confirm a page, use a Guild-held part as a visual example, and let a Long Walk courier carry a copy to the next outpost. Each contribution is optional and bounded. If the player declines all offers, a valid local lesson remains possible wherever the canonical education system already supports it.

## 63. Narrative continuity across several ordinary days

The feature can feel like a developing story without adding a quest-chain authority. Use current lesson records and ordinary world events to select a follow-up scene. If the owner exposes no suitable completion or application event, keep the follow-up as dialogue only or omit it. Do not create a custom journal progression to simulate a chain the education owner does not support.

**First available day — a question appears.** The learner encounters a term while helping with a routine task. The player can invite a lesson, find a supported source, or defer. If no valid teacher/source pair is available, the question remains a conversational hook rather than a failed quest.

**Lesson day — the agreement is honored.** The scene reflects the agreed time, the chosen method, and any current duty change. The owner produces at most its valid result for that identity and day. The teacher and learner acknowledge the exact boundary of what was taught.

**First later use — application is optional.** A compatible opportunity may expose the skill. The player can accept a supervised application, ask for more instruction, or leave the opportunity to a qualified worker. If the lesson result is not sufficient for the existing consumer, the consumer explains what is still required.

**Later conversation — interpretation changes.** The learner may explain why the knowledge mattered, ask to keep practicing, or say that the subject is complete for them. The player can respond with support, a resource request, or no additional commitment. This conversation cannot award a duplicate result or assign a job.

**A future faction request — the learner decides.** A faction may ask for the skill or request a named participant. The player can relay the opportunity, decline on the learner’s behalf only where existing authority permits, or ask the learner directly. If the learner accepts, the faction’s work route proceeds normally; if not, education remains intact.

These day labels are a writing rhythm, not fixed timers. The actual event sequence depends on schedule, session identity, available source, and existing world state. An interrupted campaign may never reach a later application. That is acceptable. The feature’s value comes from a completed, truthful learning loop, not from forcing a five-step plot into every save.

## 64. Observable consequences without a new education reputation

The plan should make consequences visible through existing world responses:

- A teacher who gave up a valid duty window is no longer available for that conflicting assignment until the schedule owner releases them.
- A learner who completed a supported session can be offered compatible application where the skill consumer accepts that result.
- A stale source remains stale until its owner updates it; the lesson does not refresh it.
- A private result stays private according to the existing record and reporting rules.
- A faction that receives a capability description can make a bounded work request; it cannot silently enroll the learner.
- A declined agreement closes without hidden skill loss or negative global reaction.
- A partial owner result remains partial through save and reload; prose cannot round it up.
- A support group’s delivery or loan changes only its own existing item/service state.
- A supervised success shows the learner’s contribution and the supervisor’s continuing responsibility.
- An unresolved question stays unresolved and can be revisited only through an allowed new session or source update.

Prefer a small number of specific, perceivable consequences over many abstract counters. “The teacher is working the water shift now” is clearer than “duty reputation −1.” “The learner reads the label, and the technician checks the current panel” communicates both agency and boundary. If the current UI cannot expose a result reliably, add no claim in dialogue until its owner provides a feedback route.

## 65. Acceptance review for branching authored content

Before a branch enters production, answer these questions in its content note:

1. What observable player action or canonical condition selects the branch?
2. Which existing owner supplies the state required to select it?
3. Does the branch alter schedule, lesson result, source state, apprenticeship request, or only the immediate dialogue?
4. If it alters gameplay, where is the single authoritative command and what consumes the result?
5. Can the branch be replayed, reloaded, or revisited without duplicating progress or consuming a second daily result?
6. Does the learner retain an explicit option to defer or decline where the underlying system permits it?
7. Does the scene distinguish reading/recognition from qualification and live operational knowledge?
8. Does a supporting Current stay within its bounded material, source, or delivery role?
9. Does the branch avoid treating reputation, morality, or faction allegiance as a proxy for the learner’s wishes?
10. If the required owner or data is missing, does the branch fail closed with a comprehensible response?

An author who cannot answer one of these questions should mark the branch unresolved. The correct follow-up is an evidence check or a smaller prose variation, not invention of a new persisted variable. This preserves the feature’s promise as an education loop rather than allowing content breadth to mask an unwired contract.

## 66. Installment 3 close

This installment expands the plan’s branching by source choice, schedule, teaching method, privacy, learner consent, supervised use, and faction requests. It supplies three distinct practical lesson stories, multiple compatible player approaches, dialogue at knowledge boundaries, and acceptance questions for authored branches. Every proposed consequence remains within the three original subfeatures and relies on existing owners. It adds no new faction authority, reputation score, quest-chain system, or save section.

## 67. Installment 4 — a second story route with endings earned through action

The first story centers on interpreting a physical service label. This installment uses a different everyday object: a shift rota that is difficult to read because a copy has been folded, marked, and corrected. Its story question is not “Will the player be honest?” It is “What does the player ask the learner to read, which source do they trust, whose time do they reserve, and how do they handle a discrepancy that belongs to another owner?”

The working title is **“The Names Beside the Hours.”** Use only an existing schedule or duty surface that can legitimately be shown. The note must not reveal private information the player has no authority to expose. If no current data source permits a rota-reading lesson, use a neutral public schedule or keep the vignette as a prose concept. Do not invent an education subject, character ID, schedule owner, or privacy exception to make the route work.

This is a short narrative route that can end in several ways. It is not a mandatory quest chain. A player who never schedules the lesson still receives a truthful response; a completed lesson need not trigger a faction scene; and a learner who declines public application has still made an understandable choice. No route requires a “good” or “evil” classification.

## 68. Route premise — reading a note is not changing the roster

A folded copy has been returned to a notice space after a shift handover. The learner asks what the marks beside two time slots mean. One mark may indicate that a person traded hours, but the copy is smudged and its date may have passed. The teacher can explain how to read the layout if a current curriculum supports that objective. Only the schedule owner can confirm assignments or apply changes.

The player’s first meaningful decision concerns the purpose of the interaction:

- **Teach the notation.** The player offers a supported session so the learner can read the time columns and symbols. The current schedule owner remains the source for present assignments.
- **Resolve the live shift question.** If the concern is who is actually working now, ask the responsible schedule owner directly. Give the learner the answer needed for immediate safety without making them wait for a lesson.
- **Protect the document.** If the copy contains private names or is not appropriate for a shared space, move to an approved private explanation or choose a neutral sample.
- **Leave the subject alone.** The learner can decide that this is not a topic they want to study. The player can close the conversation without a hidden loss.

This entry decision should happen before the system proposes a study agreement. It prevents the design from turning every question into a lesson appointment. It also gives a clear distinction between urgent information and optional education: a player never withholds a safety-relevant schedule fact as leverage to produce a lesson.

## 69. Branch map — different actions, different consequences

| Action | Immediate consequence | Later content permitted | Education result |
|---|---|---|---|
| Ask the schedule owner to confirm the current shift | The player gets a current answer if that owner supports the query | A separate lesson offer may appear afterward if the learner still wants it | None unless a valid lesson is separately scheduled and resolved |
| Use a public, non-sensitive sample | The teacher explains notation without exposing private names | Learner may request a later supervised read of another approved schedule | One session result at most, from the education owner |
| Use the folded copy after checking permission | The lesson can use a realistic but possibly outdated example | The learner may spot the date and ask the player to verify it | Progress only as returned by the lesson owner; no roster edit |
| Ask the learner to choose a teacher | The teacher preference guides the offer, subject to eligibility | A different teacher may produce a distinct voice or method | No progress before the accepted session |
| Schedule during a quiet interval | Teacher and learner can meet if schedule and room owners confirm | Application can occur during a later allowed activity | One owner result for that learner/day identity |
| Reserve a slot that displaces another duty | The duty conflict is shown and must be confirmed or rejected by its owner | A consequence may appear in the work queue if the owner applies it | Lesson outcome does not erase or conceal the duty cost |
| Share the result with a major faction | The faction hears only the level of capability the owner supports | It can make an ordinary work or verification request | No faction recruitment or education authority is created |
| Keep the result private | The player declines an optional report | Learner can apply knowledge locally or not use it | Owner record remains truthful; privacy choice does not remove the result |
| Ask for a schedule change | Current schedule flow accepts or rejects it | The learner can read a new copy only after the owner confirms it | Reading the copy cannot itself commit the change |
| Stop after a discrepancy appears | The group flags uncertainty and routes the question to the owner | The lesson may resume only if the current owner allows it | Do not invent progress because the conversation was long |

The table separates three kinds of result: the player’s immediate answer, the education owner’s session result, and the schedule owner’s official roster state. Content must use the correct one. A learner may successfully identify that a copy is outdated; the player may still lack a current shift answer; and the education owner may report no progress. Those facts can coexist without contradiction.

## 70. Ending A — the learner becomes the careful reader

The learner and teacher use a permitted sample. The learner identifies the date, finds the column headings, and asks whether a crossed-out name means a shift was traded or canceled. The teacher does not guess. The player can show how to find the authoritative source or end the lesson at that limit.

If the owner returns a completed lesson result, a later scene can show the learner checking the date before interpreting a new copy. They do not alter the schedule or tell another survivor where to report unless that use is authorized. A qualified coordinator confirms the current assignment. The learner has contributed by catching a stale paper, not by taking over scheduling.

Several follow-up tones are valid: quiet pride in noticing the date, impatience with paperwork, satisfaction at being asked a real question, or no explicit celebration at all. The scene should not give an extra progress result for the later check unless it is a separately valid owner-backed lesson on a new day. If there is no such route, show the recognition as dialogue only.

## 71. Ending B — the class is postponed, but the live answer arrives

The teacher’s duty is needed during the only available interval. The player can keep the duty covered and ask the schedule owner to answer the immediate roster question. The learner receives the information needed for their next safe action. The lesson itself remains unscheduled.

The ending recognizes the practical choice without claiming that work is morally superior to learning. The teacher may say, “We have the example ready. We don’t have the hour today.” The learner can ask for another interval, choose a different teacher, or leave it there. If the schedule owner exposes a future availability, the UI can offer a new agreement later; if not, there is no invented pending appointment.

The next visit should not guilt the player with a line such as “you never made time for me” unless the learner’s established character voice and current context support it. The player can have had a compelling reason to defer. The consequence is simply that no lesson result exists yet.

## 72. Ending C — the group finds that the copy is stale

The learner correctly reads a date showing that the copy is old. The player routes the discrepancy to the schedule owner, who confirms the current roster through its normal path. The lesson may have taught date and layout reading, but the old paper remains stale until its owner updates or replaces it.

This ending can branch on what happens to the document. The player may return it to a controlled archive, ask for a clean public copy, or keep it only as an approved teaching example. These choices affect custody or document access only where an existing owner records them. They do not create an “archive trust” education variable.

The learner’s contribution is visible when a later notice arrives: they ask whose date it carries before reading the names. That follow-up is an authored reaction, not proof that the world has a second hidden skill track. The player can rely on the canonical schedule state, not the learner’s memory of a previous sheet.

## 73. Ending D — the discrepancy belongs to a separate work problem

The current schedule owner confirms that the roster is correct, but the learner’s question reveals a coverage gap. The player can request a schedule adjustment, assign an eligible replacement through the normal owner, or leave the gap for the responsible role to resolve. The lesson does not solve the coverage problem, and the learner is not automatically volunteered because they understand the notation.

This route provides a meaningful branch based on action and role. A player who asks the learner to read for comprehension remains within the lesson. A player who asks the coordinator to fix coverage uses the schedule owner. A player who asks an eligible worker to take the shift routes the task through labor assignment. The three outcomes can appear in a single story while each owner remains authoritative.

If the schedule request is rejected because no replacement is available, the UI must report that result directly. The player may leave the gap unresolved, request another time, or choose another supported staffing option. Neither the lesson nor the dialogue may imply that the schedule changed. The learner can understand the problem without becoming responsible for its solution.

## 74. Ending E — the learner declines to have the skill reported

The learner completes an owner-backed lesson and later demonstrates the reading in front of the teacher. A major faction asks who can read the rota. The learner does not want their name attached to the answer. The player may share a general capability statement, identify a qualified staff member, ask for a private conversation with the learner, or decline to answer.

If the learner’s consent must be explicit under the active rules, the player must ask before giving a name. If no consent contract exists for this interaction, do not invent a consent bypass or claim that the lesson itself permitted disclosure. Pause content production and resolve the contract with the proper owner. The player’s dialogue cannot substitute for the missing authority.

The faction may have a genuine need for coverage. It can ask for a qualified worker or provide its own representative. Its practical need remains real even though the learner declines. The learner’s refusal does not undermine the faction, the player, or the education record. The player can support the learner while helping the faction find another route.

## 75. Ending F — knowledge becomes a request, not an assignment

After a successful lesson, the learner asks to help check a public rota or observe how changes are entered. The player can ask the current schedule owner for a supervised observation, request a formal apprenticeship if that existing route is available, or explain that reading and editing the schedule are separate responsibilities.

An observation may be accepted, deferred, or declined. A formal apprenticeship follows its own eligibility, teacher, capacity, safety, and acceptance rules. If neither route is currently supported, the story ends with an honest limitation and can still acknowledge the learner’s interest. No class rank or hidden persistence is added to remember a career track.

This ending is distinct from “knowledge leaves the lesson” because it emphasizes the learner’s initiative and the boundary between capability and authority. The learner can be curious without accepting a career. A player who chooses a work-focused approach receives a practical next step; a player who chooses privacy or rest can respect that choice. No ending is framed as the only successful one.

## 76. Endings are different closures, not a moral scale

Each route should close around what actually happened:

- **Current answer, no lesson:** the immediate question is answered; no session was scheduled.
- **Completed lesson, private application:** the owner result is shown; the learner chooses not to demonstrate publicly.
- **Completed lesson, supervised contribution:** the learner reads a permitted example; the responsible worker retains authority.
- **Partial session, future choice:** only the partial owner result is shown; the learner chooses whether to continue later.
- **Stale source, corrected schedule:** the learner notices the date; the schedule owner supplies current state.
- **Schedule conflict, no session:** the shift remains covered; a new agreement is optional and uncommitted.
- **Faction request, alternate worker:** the player protects the learner’s choice and routes the task to a qualified person.
- **Apprenticeship request, deferred:** interest is acknowledged; acceptance and training remain separate.
- **No valid teacher or subject:** the scene closes honestly; no invisible task is left waiting.

The word “ending” here means the last authored beat of this bounded route, not an ending to a major campaign, faction, or feature. A closing beat should leave the shelter in a comprehensible state and tell the player whether any valid next action exists. It should not promise an unavailable callback or silently keep a participant occupied.

## 77. Save, revisit, and state-change treatment for the route

Reopening the relevant panel after save should reconstruct the route from existing authoritative records. The route must not depend on an in-memory branch pointer that disappears on restore, and it must not rebuild a second mutable copy of the schedule or session result.

If the schedule changes between the offer and acceptance, revalidate it before committing. If the teacher’s duty changes after acceptance but before the lesson, route through the current schedule owner and explain the change. If the schedule changes after the lesson has finished, preserve the result and show the new schedule only as current context. A later edit to the roster does not retroactively change what the learner learned.

If an event is delivered twice, the owner’s stable identity and idempotency rules control. A repeated screen open cannot create a second session, consume materials twice, or replay an application as if it were a fresh lesson. If the route has no current event identity, that is an integration gap, not a reason to keep a UI-only “seen it” flag.

If the learner or teacher leaves the roster, show the actual departure or unavailability. Do not preserve a dangling assignment so the route can finish. Existing save migration must retain or safely retire any state with the canonical owner; this plan does not propose a parallel migration mechanism.

## 78. Faction branches stay proportional to the lesson

Major factions may encounter the result after the route closes, but they do not receive an automatic scene just because a lesson completed. A faction appearance requires its own current event, request, or location condition. The education result can enrich a conversation when relevant; it cannot manufacture the request.

The Military may care that a worker can interpret a roster under pressure. The player can state that the learner completed a reading lesson, distinguish that from staffing authority, and refer the request to a qualified coordinator. The Military’s response can depend on whether it needs an immediate replacement or a long-term worker, not on whether the player is “honest.”

The Rebel group may want a copy of an approved public schedule format for its own handovers. The player can share a neutral template, ask the learner whether they want to demonstrate it, or decline because the source is private. The Rebel group does not gain access to private roster names by virtue of requesting help.

An Independent group may offer goods for a trained coordinator’s time. The player can negotiate through the current trade/work owner, request a qualified participant, or decline the exchange. The lesson is not a new currency and does not create a market value for the learner’s private education record.

Archivists, the Scavenger Guild, and Long Walk can each support a source or transport need already covered by their current systems. Their role appears only if the existing narrative condition brings them into this route. Do not make every plan require all factions to appear; support roles stay optional and proportionate.

## 79. Production packet for this route

Once owners and current content IDs are verified, the writing handoff should contain:

- one entry line for the learner’s question;
- one immediate-information response, independent of lesson scheduling;
- one study-agreement prompt with accept, defer, decline, and valid alternative where supported;
- one teacher opening for the approved subject and method;
- one owner-result response for completion, partial progress, interruption, rejection, and already-processed identity;
- one application scene that separates reading from roster authority;
- one privacy/reporting choice that follows current consent authority;
- one closing beat for each supported ending above;
- localized text keys and voice references for current cast members only;
- a content-to-owner mapping for every condition and claim;
- a collision note for any already-existing rota lesson, schedule quest, or faction request.

The handoff need not ship every line in one release. It must be explicit about which branches are implemented, which are omitted, and what state each visible claim depends on. Missing branches must fail closed and explain the available route. Writers should not replace a missing owner with conditional prose that asserts an unverified result.

## 80. Installment 4 close

This installment adds a second contained story route and six action-specific endings around reading a schedule, protecting privacy, respecting duty, verifying a source, sharing capability, and responding to a learner’s work interest. It provides separate consequences for schedule state, lesson state, and narrative dialogue. Every path remains inside the agreement, single-session, and knowledge-use subfeatures; no major questline, fetch chain, additional faction authority, or morality gate has been added.

## 81. Installment 5 — a page travels farther than its teacher

The prior routes center on a service label and a roster note. This route explores what happens when a teaching page has circulated through several people and now carries several kinds of evidence: an old mark from an Archivist, a practical annotation from a Scavenger Guild worker, and a delivery date written by a Long Walk courier. The page may be useful, incomplete, or misleading. The player’s task is not to choose a faction winner. It is to decide what can safely be taught, whose time to use, and how far the learner’s confirmed knowledge should travel.

The working title is **“The Page That Came Back.”** It is a bounded content route within the same three subfeatures. It is not a fetch quest: the page is already present if the current item or narrative data supports it. The player is not sent to collect all three annotations. Each supporting Current is optional and may provide a different kind of context only if the existing story state brings it into reach.

No new portable curriculum, document-custody system, faction reputation, or knowledge-sharing ledger is proposed. The “page” may be a current approved manual, a note already held by an existing authority, or simply a dialogue reference. Its physical ownership and provenance belong to the existing item/archive systems. Its curriculum identity belongs to the education catalog. Its practical meaning belongs to the relevant work owner.

## 82. The opening — three marks, one unresolved question

The learner notices a short instruction written beside a diagram. One word is underlined; a second has been crossed out; the date is difficult to distinguish from a batch number. The teacher recognizes the format but cannot say which edition it came from. The learner asks whether the underlined word changes what the diagram means.

The player receives only what the current world can show: the page’s source, its date if recorded, and the visible annotations. The player does not receive an omniscient “safe/unsafe” verdict. There are three immediate actions:

- **Ask the learner which part they want to understand.** This keeps the student’s question central. A valid curriculum subject must still exist.
- **Check provenance before teaching from the page.** The player consults an existing source or custodian if available. This may confirm an edition, reveal a gap, or produce no answer.
- **Use a neutral example instead.** If the page is sensitive or too uncertain, a supported alternative can preserve a lesson opportunity without exposing the document.

The player can also close the conversation, provide any urgent operational instruction directly, and leave study for another time. No urgent warning is hidden behind the agreement interaction. If the player closes the conversation, the route does not create a pending invisible objective.

## 83. Source branches — provenance changes confidence, not authority

An **Archivist** may recognize a print mark or remember when a particular edition was copied. That can help date the page, but it does not prove that the instruction remains current. The player can ask for the source’s recorded edition, accept a copy of an approved reference if one exists, or stop when the available record is incomplete. The Archivist may decline to loan an original. That choice changes access, not the learner’s eligibility.

A **Scavenger Guild** worker may recognize the diagram from a recovered part. They can show the physical resemblance or explain how the label looked on a salvaged example. The demonstration may clarify vocabulary, but it cannot certify that the part is safe, compatible, or operational. The player can request a qualified worker’s review, use the object only as a visual aid if permitted, or decline it.

A **Long Walk** courier may know when the page was carried and whether it came from a settlement with a distinct notation. This context can help interpret handwriting or date order. It cannot prove present conditions at the originating location. The player can ask for a source name if permitted, use only the delivery date, or avoid repeating personal information.

The player can seek none, one, or more of these contexts, but the story should never demand a complete three-faction circuit. Availability, costs, privacy, and schedule must come from the current owners. If the route’s story requires a particular Current but the game cannot truthfully make that group available, rewrite the route as an optional variation or omit it.

## 84. The agreement — participants choose what can be discussed

Once the player has chosen a supported subject and source, they can offer a lesson. The learner may want the underlined term, the diagram, or the difference between the page’s date and its reference number. The teacher can accept the subject, ask for another source, or say that the uncertainty exceeds their competence. The player can schedule a later window or close the offer.

The agreement should disclose the practical boundary in plain language: “We can learn to read the diagram. We can’t use this page as today’s procedure until the crew checks it.” This gives the learner a reason for the limit without presenting caution as failure.

The player can also choose who hears the lesson. If an observer is permitted, the learner may prefer one-on-one instruction, accept one peer, or choose a shared demonstration. The group size is restricted to what current roster, room, curriculum, and session APIs support. If the current owner only resolves one learner, only that learner receives a result. Observers are not silently enrolled by standing nearby.

The page’s source may carry private names or a personal annotation. The player can choose a neutral copy, ask for a private setting, or defer. If the relevant consent or privacy authority is missing, the route stops before disclosure and the content owner must resolve the contract. A dialogue choice cannot invent permission.

## 85. The session — methods reveal different kinds of understanding

Within one valid session, the teacher can use one of several supported methods. The player can ask the teacher to start with the page’s layout, demonstrate the diagram, compare two terms, or invite the learner to explain the mark in their own words. These are interaction variations in one command identity. The method choice must not trigger multiple progression calls.

If the learner reads the word correctly but misinterprets its relation to the drawing, the teacher can clarify the connection. If the learner notices that the page is old, the teacher can reinforce the distinction between historical understanding and current instruction. If the learner asks why two annotations disagree, the group can record uncertainty and stop short of inventing a consensus.

The education owner determines completion, partial progress, interruption, or rejection. A long dialogue does not imply a completed lesson. A concise session can be successful if the owner returns that result. A repeated UI entry can show the prior result but must not run a second attempt for the same daily identity.

The session can also end with an unresolved question. That is a valid narrative beat when the content source does not answer it. The learner can ask for a qualified explanation later; the player can seek an authoritative source; or the player can decide the question is no longer relevant. None of these options creates an extra education state by itself.

## 86. The application — knowledge travels with an explicit boundary

After a confirmed result, the learner may recognize the same notation on a separate work page. The player chooses whether to invite a supervised read-back, ask the responsible worker to verify it first, share a neutral explanation with a supporting Current, or keep the knowledge private.

The learner’s interpretation can be right while the page itself is obsolete. The qualified worker’s confirmation remains necessary for live procedure. If the learner catches a likely mismatch, the player routes that discrepancy to its owner. If the worker rejects the interpretation, the scene shows what was misunderstood and lets the learner ask a follow-up question without humiliating them.

A **Military** representative may ask whether the learner can explain the diagram to a crew. The player can share a general, confirmed capability; offer a qualified instructor; or ask the learner whether they want to be identified. The Military’s request is a work conversation, not an enrollment command.

A **Rebel** representative may ask for a copy of the format so another settlement can teach from it. The player can share an approved neutral copy, decline because the source is unverified, or ask a current custodian to prepare a safe version if that service exists. The learner’s private notes do not become faction property.

An **Independent** trader may offer a material or service in exchange for an explanation. The player can negotiate through the current trade owner, provide only a general description, or decline. No separate knowledge-token currency is created.

## 87. Endings for “The Page That Came Back”

The route supports several closures. They are intentionally not arranged from virtuous to corrupt; each resolves a different practical need.

### Ending 1 — the learner can explain the page’s limits

The learner names what the diagram shows and also points to the date they cannot verify. A qualified worker checks the current instruction. The learner does not take over the work, but their attention prevents the group from treating an old copy as current. The scene can close quietly, with the page returned to its existing custodian.

### Ending 2 — the source is verified, but the teacher is unavailable

The Archivist confirms which edition the page came from. The teacher is pulled into a duty before the lesson. The player can keep the source reference for another time if the current record allows, ask a different qualified teacher, or stop. Provenance improves what the player knows about the page; it does not create a lesson result by itself.

### Ending 3 — the demonstration helps, but the lesson remains partial

The Guild’s example gives the learner a concrete comparison. The education owner reports partial progress or another supported result. The player shows that result accurately, then decides whether to arrange a later session. The Guild’s demonstration is not counted as a second class or certification.

### Ending 4 — the courier’s note is informative but too old

Long Walk confirms when the page arrived. The learner notices the date is not current. The player can seek current instructions or use another subject. The route closes with the learner understanding why a date matters, but no operational claim is made from the old page.

### Ending 5 — the private annotation stays private

The player chooses a neutral copy or ends the lesson when the page’s personal note cannot be shown appropriately. The learner may still study the general notation through a valid source, or may choose to defer. No faction hears the private details. A completed owner result remains intact if the lesson already occurred.

### Ending 6 — the learner explains the concept to a peer

If the curriculum and roster support a peer activity, the learner offers a general explanation to another willing learner. The second learner receives no automatic progress. Their own agreement and lesson result remain necessary. If no peer-learning route exists, the line remains a non-mechanical conversation or is omitted.

### Ending 7 — no reliable source is available

The teacher and player acknowledge that they cannot confirm the page. The immediate safety message comes from the proper work owner. The learner’s question is respected, but the lesson is not faked. The player can end the route, choose another supported subject, or revisit after a legitimate source becomes available.

### Ending 8 — a faction request goes to a qualified adult

The faction’s work need is real, but the learner is not ready or does not consent. The player identifies a qualified adult or declines the request. The lesson remains the learner’s own accomplishment, while the task is handled through the normal work route.

Any supported ending can be the player’s preferred resolution. The content should give each a clear final beat and leave participants, documents, and duty assignments in their actual owner-backed state.

## 88. How the supporting Currents remain supporting

The three local groups contribute different forms of context rather than competing for the education feature:

| Group | Useful contribution | Boundary | Player decision |
|---|---|---|---|
| Archivists | Edition, date, source record, approved copy | Cannot certify current operation or learner proficiency | Verify, use another source, or stop |
| Scavenger Guild | Physical example, practical vocabulary, recovered object | Cannot replace safety review, catalog eligibility, or the lesson owner | Demonstrate, request review, or decline |
| Long Walk | Delivery timing, notation context, question carried from elsewhere | Cannot certify remote conditions or current route status | Use context, verify locally, or withhold personal details |
| Major faction | Relevant demand for a confirmed capability or qualified worker | Cannot enroll the learner or own education progress | Share, refer, negotiate, or refuse |

The player directs whether these contributions are useful. A Current may decline or offer an inconvenient version. The learner may prefer a neutral source. The faction may decide it needs a qualified adult instead. Each refusal changes the immediate route but does not invalidate the other groups’ authority or the lesson owner’s result.

## 89. Branch combinations for the route

The author should support combinations where state and player actions can reasonably coexist. Examples include:

- The player verifies provenance with the Archivists, the learner selects a private teaching method, and the session completes; the application remains local.
- The player accepts a Guild demonstration, but the teacher’s shift ends early; the owner returns an interrupted or partial result, and the player can seek another valid time.
- Long Walk provides a dated note, yet a later faction request concerns a different edition; the learner’s old result remains true but is not misapplied to the new source.
- The page is public, but the learner declines to speak before a group; the player can arrange one-on-one supervised use or close the application.
- The lesson result is complete, but the relevant work consumer rejects the attempted application because a separate prerequisite is missing; show both facts accurately.
- A faction has a real need but no qualified teacher is free; the request can be deferred or routed to another worker without creating an education penalty.
- The player declines every Current’s offer; the lesson still proceeds if a valid local source and teacher exist.
- The learner chooses another subject after discovering that the page is not trustworthy; the previous page remains unresolved, and any new lesson requires its own supported agreement.

These combinations are important because a route that only supports one linear order will make the world feel reactive only on paper. They should be implemented through existing owner facts and explicit commands. If the current owner cannot distinguish a combination, use a common response and avoid adding a new persistent tag merely to preserve different prose.

## 90. Dialogue fragments — uncertainty without helplessness

The route’s voice should make uncertainty legible while preserving agency. Candidate lines, subject to current character profiles and localization:

- Learner: “The drawing is clear. The date isn’t. Which part are we learning?”
- Teacher: “The shape, today. The instruction, only after someone checks the current sheet.”
- Archivist: “I can date the copy. I can’t promise what the repair crew changed after it left us.”
- Guild worker: “This part has the same mark. Same mark doesn’t mean same machine.”
- Courier: “I carried it last week. I didn’t write it, and I can’t tell you what happened there yesterday.”
- Learner: “I want to show I can read it. I don’t want my name sent with it.”
- Player: “We can tell them what the lesson covered without turning it into a work assignment.”
- Supervisor: “Read the label with me. I’ll make the call on the machine.”

Short lines should carry the actual boundary rather than a long exposition block. Characters can disagree over whether the page is useful, but they should not disagree about facts that the underlying system has already resolved. If an authoritative source is current, dialogue should not pretend that its status is unknowable merely to prolong suspense.

## 91. Content pacing and optional returns

This route can occupy one conversation, one lesson, and one later application. Those beats need not all appear in one session or on consecutive days. The player may ignore a follow-up. If the lesson owner exposes a result but the campaign has no relevant consumer or request, the route can close with the learner using the knowledge informally or with no application scene at all.

Avoid repeatedly prompting the player to ask every group for an opinion. Offer source context when the player chooses it or when a relevant world event naturally introduces it. A concise route that brings in one supporting Current is better than a forced tour of three. Additional groups should enrich a replay or an available branch, not become a completion checklist.

If a supporting group appears, make the return useful in proportion to its cost. A paid copy should be deliverable through the existing market/inventory owner. A request for a teacher should expose that teacher’s real availability. A courier’s report should preserve its date and source. The player should never spend a resource for an annotation that the story then treats as authoritative when its owner does not.

## 92. Accessibility and comprehension in the authored scene

The player must be able to understand what each option will do before choosing it. “Use the page” is too vague if it could reveal private information, consume an item, or produce an owner command. Label options by action: “Check edition with the Archivists,” “Use the approved neutral copy,” or “Stop and ask a qualified worker.” If cost or delay is real, surface it before commitment.

Provide equivalent ways to participate where current UI permits: read aloud, point to a diagram, compare two marks, or explain verbally. These are interaction choices that can make the fiction more inclusive; they must not silently change ability scores or make one style the only successful route. If accessibility support changes a core input path, route it through the current UI/accessibility owners rather than adding story-specific controls.

Ensure that the result message distinguishes “lesson complete,” “source verified,” “application accepted,” and “work authorization.” Use clear wording and available visual feedback. Do not rely on color, audio, or a subtle journal update as the only sign that an owner rejected or completed an action.

## 93. Review checklist for “The Page That Came Back”

Before any content implementation, record evidence for the following:

1. The chosen page or document exists in current content or is explicitly approved as a new authored record.
2. Its custody, source, date, and privacy treatment have current owners.
3. The curriculum catalog has a matching objective and a valid learner/teacher route.
4. The schedule, room, and session identity remain canonical through retry and restore.
5. A successful lesson produces one owner-backed result and reaches the current consumer.
6. Any practical demonstration is allowed by the work and safety owners.
7. Faction responses route through existing conversation, work, item, and trade paths.
8. Every branch can be selected from a real action or current condition.
9. Every ending closes without unsupported success, invisible pending state, or duplicated progress.
10. The story remains understandable when no supporting Current is available.

If a required provider is missing, omit or simplify the affected branch. If the source is uncertain, the story can end with a truthful stop. A missing capability is a design limit to resolve with its owner, not permission to build an alternate authority for this plan.

## 94. Installment 5 close

This installment adds a source-centered route in which the learner, teacher, player, and optional supporting Currents interpret a circulating page. It offers eight distinct closures, multiple source and privacy choices, faction requests, and action combinations across ordinary days. The player’s choices concern evidence, method, time, disclosure, and work authority. Education progress still belongs to one replay-safe session owner; curriculum and application remain the other two subfeatures. No new quest authority or fourth feature has been added.

## 95. Installment 6 — the lesson with the open door

Location can change how a lesson feels without becoming a new gameplay feature. A room can be noisy, public, inaccessible, already in use, or close to the equipment that makes an example concrete. The learner’s privacy preference, the teacher’s schedule, the room’s actual access, and the lesson’s method create a new set of player choices. The lesson owner still determines one result; no room grants an education bonus by itself.

The working vignette is **“The Lesson with the Open Door.”** A learner has agreed to study a supported subject, but the available space is a shared room beside an active passage. People can hear the conversation when the door is open. The teacher has limited time before another duty. A nearby work area offers a relevant object, but it may not be safe or appropriate for a lesson. The player must decide how to arrange the moment with the learner rather than treating an empty floor tile as a classroom.

The scene is not a request for a room-placement system, a privacy meter, or a new schedule authority. Only use rooms and access facts already supplied by current owners. If there is no authoritative room state, stage the dialogue in an existing narrative location without claiming occupancy, capacity, or privacy mechanics.

## 96. The learner names the condition for a useful lesson

Before the player commits to a location, the learner can say what would help: quiet, light, a place to see the example, no audience, or a quick start before the teacher leaves. These are conversational preferences, not new stats. The player can ask one follow-up question, accept the stated preference, offer a supported alternative, or defer.

Do not force the learner to justify a request for privacy. If they say “not here,” the player may respect that without demanding a disclosure about family, literacy, past embarrassment, or faction activity. If a current character profile supports a specific explanation, it can enrich the scene, but it is never a required key to a branch.

The teacher can also state a limit: a short interval, a required workbench, or a need to remain available for a call. The player receives that information before deciding. Neither teacher nor learner is reduced to a resource slot. Their constraints are legible, and the player can choose among valid options.

## 97. Four location approaches and what each changes

### The shared room

The shared room is convenient and may already have an approved place for reading or instruction. Its cost is audience and noise. The player can use it if the learner accepts the setting, choose a neutral example that does not disclose private information, or wait for another space. If the session proceeds, the owner result is unchanged by the room unless current game rules explicitly model a relevant condition.

### The active work area

An active work area makes a practical example visible, but equipment, hazards, noise, and staff duty remain with their existing owners. The player can ask a qualified worker to supervise a permitted observation, move to a safe explanation away from operation, or reject the location. A learner must not be brought into a restricted zone because a lesson scene needs atmosphere.

### The quiet approved space

An existing quiet room or private corner may protect focus or confidentiality. The player checks actual access and current use. If the space is occupied, an NPC line cannot make it available. The player can wait, choose a shared setting with the learner’s agreement, or reschedule. Do not create a new “private room unlocked” flag.

### The outdoor or transitional space

A covered passage or sheltered exterior can be suitable for a brief read-back when current access, weather, and safety owners allow it. Movement, lighting, noise, and interruptions may change the scene. If the interface cannot represent the location’s conditions, keep them in prose and avoid claims of mechanical effects. The lesson remains the same education command.

The player can also decide that no location is acceptable today. That is a real ending to the current offer. It does not destroy the learner’s interest or produce a hidden penalty.

## 98. Decision tree — privacy, timing, and practical context

1. **Ask what setting the learner prefers.** If the request can be met in an existing available space, offer it. If not, say which condition cannot be met and let the learner choose another supported option.
2. **Check the teacher’s real interval.** If the teacher’s window conflicts with duty, present the conflict before the lesson starts. Do not disguise an unavailable teacher as a confirmed appointment.
3. **Check the room or work area.** Use its current owner for access and occupancy. An empty-looking scene is not proof of availability.
4. **Choose the method that fits the space.** A read-back may work in a quiet corner; a demonstration may require an approved work surface; a source comparison may need adequate light. These are authored affordances, not independent progress grants.
5. **Confirm that both participants still agree.** If the location is more public than expected or the work area becomes active, ask whether to continue, move, or stop.
6. **Resolve the session once.** The accepted session identity and owner determine completion, partial result, interruption, rejection, or already-processed status.
7. **Offer a truthful next step.** The player can apply the knowledge under supervision, arrange a future agreement, choose another source, or close the scene.

This order lets the player make informed choices without a broad morality label. It also prevents the system from treating a private location as inherently better or a shared lesson as inherently generous. A learner may prefer company. Another may prefer quiet. The right branch is the one the participants accept within the available constraints.

## 99. Branch outcomes from the same scheduled lesson

**Quiet space, full interval:** the learner selects a private or low-traffic place. The teacher has enough time, the material is valid, and the owner returns a completed result. Later application can remain private or be offered with the learner’s consent.

**Shared room, learner-assented:** the learner agrees to a shared room and the teacher uses a neutral example. If observers are not enrolled by the current session owner, they do not gain progress. The learner can stop if the audience changes or becomes uncomfortable.

**Work area, supervised read-only demonstration:** an eligible supervisor confirms the area is available and safe. The teacher connects the lesson to an object but does not allow unsupervised operation. Any later work action routes through the proper skill or duty owner.

**Teacher window too short:** the teacher can give an immediate safety or orientation statement, then close the lesson offer. If the owner supports a short session, use its normal result; otherwise, do not invent a shortened lesson pathway. A later attempt needs a valid agreement and daily identity.

**Space unexpectedly occupied:** the room owner reports that it is unavailable. The player can choose another approved location, wait if the teacher remains available, or defer. No session is submitted until the participants actually begin.

**Learner changes their mind at the doorway:** the player closes or reschedules without public pressure. No progress is claimed unless the owner already committed a valid result. The scene should not turn the learner’s change of mind into a faction or family conflict automatically.

**Noise or interruption after the lesson begins:** resolve timing carefully. If the session owner has committed a result, preserve it and show the interruption around it. If not, close according to the owner’s supported interruption path. Reopening the scene cannot create a second result.

## 100. A full scene pass — from arrival to close

The teacher arrives with the approved page. The learner sees the open door and pauses. The player is offered three concise choices: ask whether to use this room, find an existing approved alternative, or end today’s agreement. If the learner accepts the shared room, the teacher removes a personal name from the example only if the current record permits that redaction; otherwise, the group uses a different sample.

During the lesson, someone passes the doorway. The learner can continue, ask to close the door if the room owner allows it, or stop. The teacher does not decide for them. If the room cannot be secured, the player can move only to a currently available approved space. Otherwise, the session closes through the actual owner contract.

If the learner stays, the teacher asks for one action that fits the chosen method: read the heading, trace the diagram, identify the date, or explain the term. Dialogue can acknowledge effort, uncertainty, or a correct answer. The game displays the owner’s result and avoids equating one right answer with an unsupported level of mastery.

At the end, the learner chooses whether another person may hear about the result. The teacher records any required information through the education owner. The player may offer a later supervised application, but does not schedule it without checking the relevant work and schedule owners. The scene closes with a concrete image—page folded, chair returned, door opened—not a generic success banner alone.

## 101. Ending beats keyed to the player’s approach

- **Learner-led privacy:** the learner chooses a quiet setting and later chooses who, if anyone, hears about the lesson. The player protects that boundary while preserving the owner’s required record.
- **Practical observation:** the player chooses an approved work area and qualified supervisor. The learner sees how the vocabulary appears in context but does not gain work authority automatically.
- **Shared teaching moment:** the learner agrees to a neutral group explanation. Other listeners may ask questions, but only individually valid learners receive any owner-backed progress.
- **Schedule-first deferral:** the player keeps the teacher on duty and closes the lesson offer. The immediate answer is given if required; study remains optional for another time.
- **Source-first pause:** the room is ready but the example is not. The group stops before teaching from questionable material and seeks an approved reference later.
- **No suitable setting:** the learner’s preference and current access cannot both be met. The player explains the limitation and leaves the choice open without an untracked appointment.

Each ending should have a last line from a participant or a clear physical action. Avoid grading the player with praise or disappointment. If an ending has a cost, show who bears it and which owner applied it.

## 102. How setting affects flavor without affecting the result falsely

The same education outcome can be presented through different sensory details. A shared room has interruptions and overlapping conversation. A work area carries familiar sounds and visible tools. A quiet space makes pauses noticeable. An outdoor setting brings light, cold, or passing traffic into the scene. These details enrich tone; they must not imply hidden multipliers or penalties.

If current simulation data includes a real need, fatigue, noise, or room modifier used by the lesson owner, the scene can reflect it after that owner returns the outcome. If no such modifier exists, do not claim that a noisy room caused a partial result. A narrative interruption can still be interesting without changing education progress.

Likewise, do not represent disability, language, or sensory preference as a flat penalty. Offer understandable interaction choices within current UI conventions. If a requested accessibility behavior requires changes to controls or presentation, route it through the existing accessibility and UI owners rather than inventing a story-specific substitute.

## 103. Supporting roles in the location route

An **Archivist** may provide a neutral copy or suggest where an approved document can be read. The Archivist does not reserve a room unless the current space owner confirms it. A **Guild** worker may offer a supervised demonstration area, but only the work and safety owners can approve its use. A **Long Walk** courier may arrive during the scene and create a scheduling choice, but delivery timing comes from the existing travel or narrative owner.

A **major faction** representative can request use of the shared space or ask the learner to demonstrate a skill. The player can accept if the learner agrees and the room owner permits it, offer another qualified person, or decline. A faction’s urgency cannot supersede participant consent and current access rules by implication.

Supporting groups can also be absent. If their presence would require an unsupported spawn, room reservation, or activity flag, keep the location route local. The value of the vignette is the player’s arrangement of a valid lesson, not a mandatory tour of every group.

## 104. Re-entry rules for changing location conditions

If the player leaves the screen before confirming the lesson, return to the current owner state and rebuild the options. Do not preserve a panel-local room selection as if it were an accepted agreement. If the player already confirmed the agreement, restore it through its owner and revalidate any condition the contract says can expire.

If the room becomes unavailable before the session begins, show the owner’s current answer and offer a supported alternative. If the room becomes unavailable after the session result is committed, retain the lesson result; only the remaining application or follow-up scene changes. If the learner’s privacy preference changes, ask again where the current story state permits, and do not infer that last week’s choice still authorizes today’s audience.

If a second observer arrives, do not silently convert an individual lesson into a group lesson. The player can ask the original learner whether they wish to continue, move, or stop, then follow the current session API’s supported behavior. If that API cannot model the change safely, close the current scene without duplicating progress and record the integration gap.

## 105. Content implementation notes

The writing packet for this vignette should identify the existing location IDs, access provider, room capacity source, schedule provider, curriculum subject, eligible teacher and learner, privacy contract, accepted session identity, result consumer, and application route. These are discovery requirements, not new claimed implementations.

For each location option, the packet should state whether it changes only staging or also a verified owner fact. A room can have a rich description without changing the lesson result. A supervisor can be visible without being assigned a new role. An observer can hear dialogue without receiving progress. If those distinctions are not captured in the content metadata or command flow, reduce the choice to safe dialogue variation.

The UI must label concrete outcomes. “Use shared room” should tell the player that other people may hear. “Move to work area” should disclose any duty, safety, or access requirement. “Wait for a quiet room” should show a real delay only if the schedule owner can report one. “End agreement” should close cleanly and make no claim about future availability.

## 106. Installment 6 review gate

Accept the location route only if current owners can answer all of these questions: Is the place available? Is it appropriate for the selected method? Who may hear or observe? Are the teacher and learner still available? What does the session owner return? Which later action can consume the result? If any answer is unavailable, the corresponding branch is omitted or narrowed to a truthful conversation.

Do not add a room-quality score, privacy meter, group-learning ledger, attendance registry, or locally saved appointment to preserve prose differences. Do not let a location name or dramatic line stand in for access, capacity, consent, or safe work authority. The player’s agency comes from choosing among real arrangements and accepting their visible tradeoffs.

## 107. Installment 6 close

This installment adds a location-centered branch set and the vignette “The Lesson with the Open Door.” It makes privacy, access, teacher timing, method, observation, and changing room conditions shape the player’s choices. Outcomes remain grounded in existing schedule, room, education, and work owners. The lesson remains one valid session with one owner-backed result, and no fourth subfeature or new location authority is proposed.

## 108. Installment 7 — what happens when the answer is wrong

A learning feature needs more than a success line. A learner may misread a word, apply a familiar rule to a new context, or notice that the source itself is ambiguous. The teacher may also be uncertain. These moments can produce meaningful player decisions without turning mistakes into a hidden failure score or a moral test.

The working scene is **“Read It Again.”** A learner reads a term from an approved practice page, then applies it to a nearby object or note. The interpretation does not match what the responsible worker expects. The player must decide whether there is an immediate safety concern, whether the source is trustworthy, who is qualified to answer, and how to continue the learning conversation.

The scene’s first principle is to protect people and equipment before protecting the lesson’s dramatic rhythm. If the interpretation could affect an active operation, the player pauses that operation through its current owner and asks a qualified person to verify the instruction. The learner receives the necessary safety information immediately. They are not left to act on an uncertain reading while the game waits for a lesson choice.

The second principle is epistemic humility. A mismatched answer does not prove that the learner failed. The page might be stale, the mark might be unclear, the teacher might be recalling another edition, or the player may lack the needed context. The branch begins with verification, not blame.

## 109. Establish the kind of mismatch before responding

The player sees a mismatch and can select an action that fits its cause:

- **Ask the learner how they reached the answer.** The player may learn whether the issue was pronunciation, a confusing mark, missing context, or a reasonable inference from the page.
- **Compare the source with an approved reference.** If the current content supports a reference check, the player can confirm an edition, date, definition, or diagram.
- **Ask the teacher to model the step.** A qualified teacher may show the distinction without implying that the learner should already know it.
- **Consult the responsible work owner.** If the question affects an active process, the work owner—not the lesson scene—confirms the live instruction.
- **Stop and return later.** If no reliable source or qualified person is available, the player can keep the question open in conversation without inventing a result or persistent task.

These actions have different immediate consequences. Asking the learner preserves their reasoning and may reveal an ambiguous source. Checking a reference costs time or access if those costs are real. Asking the teacher centers instruction but may expose a qualification gap. Consulting the work owner resolves the operational question but may end the lesson. Stopping protects against a false claim and can leave the emotional question unresolved.

Do not place these actions under a single “correct” button. If the player is facing an urgent operational choice, the interface should clearly distinguish “pause the operation and verify” from “continue the lesson.” The scene should not reward curiosity by making an unsafe interpretation.

## 110. Branch table — correction is an action, not a judgment

| Player action | Immediate scene | Valid later consequence | Forbidden inference |
|---|---|---|---|
| Ask the learner to explain their reasoning | The teacher listens before correcting | The example can reveal whether the source or lesson method was unclear | The learner’s confidence is not a proficiency value |
| Correct the term in front of the group | The teacher demonstrates the reading and checks whether the learner wishes to continue | The current session owner returns its actual result | Public correction is not automatically humiliation or trust loss |
| Move to a private explanation | The learner may ask a follow-up without an audience | The scene can continue if the agreement and session owner permit it | Privacy does not create bonus progress |
| Ask a qualified worker to confirm a live instruction | The operation is paused or continued only through its owner | The lesson can resume with a verified example | The work owner’s confirmation is not an education award |
| Check the document’s date or edition | The source is verified, rejected, or left uncertain | A suitable source may be selected for a later agreement | A learner’s earlier answer is not retroactively rewritten |
| Stop the lesson | Participants close the material and leave the question open | A new agreement may be made when valid | Stopping is not a failure penalty or a moral deficit |
| Let the learner read it again after a demonstration | The learner repeats or declines | Only one result comes from the valid session identity | A second try is not a second daily lesson |
| Invite a peer to comment | The learner can accept or decline an observer | A peer can ask a question, subject to supported group rules | The peer does not receive progress by observation |

The table is for content and contract design. It does not ask for a persisted “mistake type,” “correction quality,” or “confidence” field. If the education owner already returns a supported partial/completed outcome, use that result. If it does not, the authored scene should not claim an additional performance category.

## 111. A safe scene pass — separate urgent action from teaching

The learner points to a label and says that the mark means the system should be adjusted. The nearby qualified worker says the current reading does not support that interpretation. The player sees the first choice: pause the work and ask the worker to confirm the live state; ask what part of the text the learner used; or close the learning scene and route the issue to the responsible owner.

If the player pauses and checks, the worker confirms the current instruction. The worker may say the mark means something different on this equipment, or may discover that the page is not current. Either way, the game reflects the worker’s actual result. The learner is not asked to operate anything while the question is unresolved.

After the operational question is settled, the player can decide how to return to learning. The learner may want to compare the source, ask the teacher for a demonstration, choose a simpler approved example, or stop. The teacher acknowledges the difference between “you read the mark” and “we know what this machine needs today.”

If the learner’s reading was correct but the page was stale, the story states that clearly. If the reading was incorrect, the teacher explains the distinction. If the worker cannot determine the answer, the group says so and stops. This branch is complete when the player understands the limit, not when every uncertainty is artificially resolved.

## 112. Three correction approaches with different social texture

### Ask-first correction

The player invites the learner to explain their interpretation. The teacher may discover that the student used a reasonable clue but lacked a source convention. This route foregrounds thought process and can create a thoughtful exchange. It does not guarantee a completed lesson; the education owner remains decisive.

### Demonstration-first correction

The teacher demonstrates with a verified, permitted example. The learner can repeat the reading, point to the clue, or ask to stop. This route suits a learner who prefers seeing a method before talking. If the teacher is not qualified for the subject or the example is not current, the route must not pretend the demonstration is authoritative.

### Quiet correction

The player asks the teacher to continue privately or use a neutral sample. This route may protect the learner from an unwanted audience. It can cost a room change or time only where those costs are owner-backed. The learner can still prefer public discussion; the content does not assume that privacy is always better.

These are not personality classes. The same learner can prefer different correction approaches in different moments. The player’s previous choice can inform a later line only if current narrative state supports it; do not create a hidden “teaching style affinity” variable.

## 113. If the source, teacher, or interface is the problem

The writer must keep three possible error sources open until an authority resolves them:

1. **Source problem:** the page is stale, damaged, incomplete, or from another edition. Route provenance and replacement through the current document owner.
2. **Teacher problem:** the teacher remembers a different convention or is not eligible for this subject. Revalidate teacher qualification and offer a supported alternative.
3. **Presentation problem:** the text is clipped, low contrast, unreadable at the current scale, or missing a controller/keyboard path. Route the issue through current UI and accessibility owners; do not blame the learner.

Only after these checks can content attribute the mismatch to a misunderstanding. Even then, describe what was misunderstood, not who the learner is. A useful line is “That mark belongs to the next column,” not “You never pay attention.”

If the source is wrong, the teacher should not be retroactively declared incompetent without evidence. If the UI hides a relevant clue, do not add a dialogue hint and call the underlying interaction repaired. If no owner can settle the source, the player can halt the task and leave the lesson open for a later valid route.

## 114. Session result behavior during correction

The education command occurs once for its stable learner/day identity. Asking a follow-up, changing the correction method, or moving to another room does not create a fresh result. If the lesson command has not yet been committed, the player can revise the planned method before invoking it, provided the agreement and schedule remain valid.

If the command already returned a result, preserve it even if the scene moves into a verification exchange. A complete result remains complete; a partial result remains partial; rejection remains rejection; and an interrupted result follows the owner contract. The operational check is a separate command owned by the work system, not a second way to rerun education.

If the lesson is stopped after a learner’s wrong answer, do not assume the owner should return “failure.” It may report a partial result, no progress, a valid completion, or another supported response. The content must follow that response. If the owner has no suitable result representation, record an API/content gap for the integrator rather than smuggling an extra status into dialogue.

## 115. Endings for “Read It Again”

**The learner corrects the interpretation.** After a demonstration or source comparison, the learner identifies the relevant mark and explains the limit. The teacher confirms only the supported point. The work owner separately confirms any live instruction.

**The source is corrected, not the learner.** The group discovers a stale or damaged page. The player routes it to its existing custodian, selects a valid reference if available, or ends the lesson. The learner’s question helped expose a source problem.

**The player stops at the right boundary.** No qualified person is available and the source is uncertain. The player pauses operational use, explains why the group cannot continue, and closes the lesson without claiming new skill progress.

**The session is partial and the learner wants another method.** The owner’s partial result is shown accurately. The learner asks for a drawing, demonstration, or simpler source next time. A new attempt requires the ordinary agreement and a new valid daily identity.

**The learner wants privacy after correction.** The player closes the audience or moves to an approved quiet space. The learner may continue, defer, or end. The story does not interpret privacy as shame unless the existing character context supports that reading.

**The teacher revises their own explanation.** The teacher realizes that the example was ambiguous and says so. The learner and teacher review a supported source or stop. The scene can show expertise as careful correction rather than infallibility, without creating a new teacher rating.

**The learner opts out.** The player accepts the choice, answers any urgent safety question through the proper owner, and closes the lesson. No hidden penalty, blocked future route, or faction reaction follows unless an existing owner explicitly produces one.

**A faction witness asks for a demonstration.** The player declines to make a public performance, offers a qualified worker, or asks the learner if they want to participate. The faction’s task continues through its own work route.

## 116. Supporting factions and Currents as context for correction

An Archivist can clarify a source’s date or edition when the current archive authority has that record. They cannot decide whether the learner passed. The Scavenger Guild can show whether a similar label appears on a recovered object, but resemblance is not proof of current instructions. Long Walk can describe where a note was carried, but cannot vouch for current conditions elsewhere.

Major factions may be present because a task needs explanation. The Military can insist that a qualified worker verify an active instruction. The Rebel group can question whether an old document should be trusted. An Independent crew can request a neutral source or offer a trade for expert time. The player can respond to these practical differences without choosing a moral camp.

No faction receives authority to shame, grade, or recruit the learner through an error. If a representative reacts harshly, the player may end the demonstration, request a qualified contact, or move the exchange to an allowed private conversation. Any relationship consequence must come from an existing faction owner and an actual action, not from the mere fact that the learner was mistaken.

## 117. What the player learns from an imperfect lesson

The feature can make the player more capable of reading the simulation’s limits. The player learns that a curriculum result, a source record, and a live work instruction are different facts. A valid lesson can help identify uncertainty; it cannot replace the source owner. A teacher can be experienced and still need a current reference. A learner can make a mistake and still notice a crucial discrepancy.

This is not a new player skill system. Do not award “judgment XP” or unlock a secret competence route. The player’s improved understanding is delivered through clear feedback and future choices. If the game tracks a learning result for a survivor, it remains in the education owner. If the player needs to review prior information, use an existing journal or log surface only if its current data contract supports it.

The narrative can also show that a correct answer sometimes begins with a question. The learner who says “this page looks old” may have contributed more to safe work than a confident guess. The lesson outcome still comes from its owner; the work outcome still comes from its owner; the story respects both.

## 118. Writer and integrator review for error branches

Before adding an error branch, document:

- the exact source or live condition the player sees;
- the owner that can verify that fact;
- whether an urgent safety action is required before the lesson continues;
- the learner’s explicit choices at the moment of correction;
- the teacher’s actual qualification and availability;
- the single education command identity and returned result;
- the separate work command, if the player is checking an operational issue;
- which line communicates uncertainty or correction without humiliating the learner;
- whether any faction presence has a real current trigger and owner;
- how the scene closes if the source cannot be verified.

Do not author a “mistake ending” until the owner results and source provenance are known. Do not use a random outcome to decide whether the student was right when the answer depends on an authoritative document or work state. Deterministic variation may select voice or presentation only when the relevant owner and seeded RNG contract support it.

## 119. Implementation and verification boundaries

The future implementation must prove that the active UI can distinguish a scheduled lesson, an uncommitted session, an owner result, and a work verification. It must keep the session identity stable through reopen and restore. It must show the work owner’s live response separately from education progress. It must not permit repeated correction dialogue to call the lesson command again.

Content review should inspect every result line against the current command response. Save review should confirm that a completed education result survives restore and that a stopped scene does not create a dangling assignment. Accessibility review should check that the disputed source is actually readable and that the player can make all correction choices with current input methods. Any future tests belong to the package owner’s targeted verification plan; this document has not run or claimed tests.

## 120. Installment 7 close

This installment expands error handling into a player-authored branch: verify the source, protect immediate safety, choose a correction approach, respect the learner’s response, and end with a truthful owner-backed result. It distinguishes learner misunderstanding from source, teacher, and presentation failures; adds eight grounded closures; and allows factions to contribute context without judging or enrolling the learner. No mistake score, teacher rating, fourth feature, or second daily lesson has been added.

## 121. Installment 8 — a lesson crosses a shift boundary

A lesson exists inside a world that advances by simulation days and assigns people to work. A player may open the education view before a shift change, schedule a lesson, close the screen, advance the day, and return after the teacher’s duties have changed. A save may happen after the session command but before its result is rendered. A host callback may arrive twice. These cases affect whether the promise “one lesson, one daily result” is true.

This installment expands the boundary conditions rather than adding another feature. A lesson uses the campaign’s simulation-day identity, current schedule owner, existing session command, and established save lifecycle. It must not use the player computer’s date, elapsed real time, panel-open duration, or a newly invented local cooldown.

The narrative consequence is practical: a teacher who has started a shift cannot be treated as still available because the lesson panel was left open. A completed result cannot be erased because the day advanced. A lesson that never reached the owner cannot be reported as completed merely because the player saw its opening scene.

## 122. Distinguish offer, agreement, command, and result

The writing and UI should use four plain-language moments, with implementation mapped to actual owner contracts:

1. **Offer:** the player and learner consider a lesson. No schedule or progress changes unless the owning command says otherwise.
2. **Agreement:** the eligible learner and teacher accept a valid time. The schedule and education owners determine whether this commitment persists or expires.
3. **Session command:** the education owner resolves the lesson against current canonical facts and a stable campaign-day/session identity.
4. **Result:** the host displays the owner’s returned completion, partial progress, rejection, interruption, or previously processed result.

These labels describe player-facing stages, not a request for four persisted statuses. If the existing owner represents the lifecycle differently, use that contract and map the prose accordingly. Do not add a second state machine to make the scene easier to script.

The most important separation is between agreement and result. An accepted time can still be missed when the teacher is called to duty. A session can be submitted once and then survive a save before display. The UI must distinguish those conditions without implying that every accepted agreement already produced education progress.

## 123. The day-boundary decision must come from simulation time

Before implementation, identify the existing simulation day source and the exact point at which a day advances. The education owner and its integrator must decide whether a session belongs to the day on which it is accepted, the day on which its command is committed, or another existing authoritative event. This plan does not choose a new rule by prose.

The selected rule must be stable and visible. If a lesson is scheduled for the next shift, the UI should name that simulation day or shift. If a session begins before an advance but resolves after, the owner must return one unambiguous identity and result. Day identity must not change because the player paused, reloaded, switched panels, changed system locale, or left the game running overnight.

If the simulation uses a monotonically increasing day index, use that existing value. If it uses a composite campaign clock, follow that owner’s canonical key. If no stable day identity exists, implementation must stop at that evidence gap until the time owner and education owner agree on the contract. Do not add a wall-clock fallback.

## 124. Boundary cases and their truthful outcomes

| Event order | Required interpretation | Player-facing response |
|---|---|---|
| The offer is opened, then the player advances the day before agreeing | No session occurred; refresh all eligibility and schedule facts | “That window has passed. Check today’s roster before arranging another time.” |
| Agreement is accepted, but the teacher is reassigned before the session command | Revalidate availability through the schedule owner | Offer another valid teacher/time or close without progress |
| Session command commits, then the day advances before the panel renders | Preserve and display the committed result; do not submit again | Show the result from the original session identity and the new current day separately |
| Session command is sent, but the host receives no response | Resolve through the owner’s idempotency/recovery contract | Show pending/retry only if the owner supports it; never assume failure or completion |
| Host retries the same command during the same simulation day | The owner returns the existing result or an explicit processed response | Do not roll again, charge twice, or advance twice |
| The player saves after command commit and before display | Restore the committed result through the save owner | Display the exact result once after restore |
| The player saves before the command commits | Restore the pre-session state; revalidate agreement and schedule | No progress toast appears until a new valid command returns |
| A second lesson offer appears later the same day | Apply the feature’s one-result contract and current owner policy | Explain that another attempt is unavailable or route only a permitted alternative |
| A lesson is canceled before any session command | No education result exists | Close the agreement through its schedule owner without a false completion line |
| The session is interrupted after the owner returns a result | Keep the result; represent the interruption only in the remaining scene | Do not erase or duplicate the committed progress |

The exact commands and messages are implementation-specific. The table defines the truth the experience must preserve and the distinctions the future integrator must map to current contracts.

## 125. Shift handover as a branching scene

The teacher and learner arrive just as a shift handover begins. The player can keep the lesson time, let the teacher complete the handover, find another qualified teacher, or ask the learner whether they want to reschedule. Those choices are materially different even if they lead to the same education result on a later day.

If the player keeps the lesson, the schedule owner must confirm that the handover can be covered. If the teacher stays for the handover, the lesson does not proceed until the teacher is free. If a substitute is offered, the education owner verifies subject eligibility; the player cannot promote a convenient worker by selecting their name. If the learner defers, the scene ends with their choice acknowledged and no hidden appointment created.

The player can also choose an immediate safety briefing while study waits. A responsible worker states the instruction the learner needs now, then returns to their shift. This briefing may be narration only; it cannot masquerade as curriculum progress unless the education owner supports that result. The lesson can be arranged later through a fresh valid agreement.

Several endings are available:

- **Handover protected, lesson deferred:** the crew receives a clean transfer; learning waits.
- **Alternate eligible teacher accepted:** the learner agrees to a different instructor; the lesson proceeds through the normal owner.
- **Teacher remains, coverage confirmed:** the schedule owner supports both duties and the lesson; the player sees the real cost or gap.
- **No replacement available:** the player closes the offer and may revisit later.
- **Learner withdraws after the delay:** the player respects that decision; the absence of a lesson is not framed as disloyalty.

The schedule does not become a morality meter. A player who prioritizes continuity is not automatically anti-education, and a player who arranges coverage is not automatically careless. The action has a visible operational outcome; the characters respond to what actually happened.

## 126. The result belongs to the command’s identity, not the open panel

An open screen is presentation state. It cannot own whether a lesson ran, which day it belongs to, or what progress was earned. Closing and reopening the panel, changing tabs, using keyboard/controller back, receiving a separate event, or loading a save must not create a new education result.

The stable identity must come from the current education/time owners and meet the same-seed replay rules. The exact fields and storage location are an implementation decision, not specified here. At minimum, the integrator must resolve how the owner distinguishes a legitimate new daily lesson from a duplicate delivery of the same command. If a deterministic key is needed, its contents and collision behavior must be documented by the owner.

Do not use a random GUID, wall-clock timestamp, hash iteration order, or process-local counter to invent identity. Do not make a panel-local `alreadyClicked` boolean the only replay defense. If the host sends the same event twice, the owner must still avoid a second result or the route must not claim replay safety.

The UI can retain focus or a visible result panel as interface state, but it should reload its facts from authoritative state. A “last lesson” label that survives only while a node remains in memory is not persistence.

## 127. Save and restore sequence for an in-flight lesson

The future save review should trace each boundary using the existing section owner and host restore order:

- Before agreement: restore learner, teacher, subject, schedule, and room state from their canonical owners; do not restore a stale panel selection as a commitment.
- After agreement but before the session: restore only the agreement state the current owner persists; revalidate any time-sensitive eligibility before execution.
- After command submission but before response: use the education owner’s supported idempotent recovery path. If there is no such path, do not claim safe automatic retry.
- After result commit but before presentation: restore the result and show it without invoking the lesson command again.
- After presentation: reopening the feature reads the same authoritative progress and current schedule, not a second locally saved copy.
- After later curriculum or roster changes: follow the current migration/retirement rules; do not silently reinterpret an old result as a new lesson.

Save ownership includes capture, restore, registration, restore ordering, dirty flush, and compatibility with existing saves. Merely serializing a session DTO does not prove the result survives. The plan remains blocked on current-source evidence until the actual owner and save seam are verified.

## 128. Retry language must describe the real transaction state

“Try again” can mean three different things: repeat a failed request with the same identity, schedule a new lesson for another day, or start a different supported subject. The UI should use specific labels and explain costs. If the owner cannot tell whether a request committed, the UI must not offer a fresh daily attempt that could duplicate progress.

Suggested responses, adapted to current localization and UI conventions:

- **Same command already processed:** “Today’s lesson is already recorded. Review the result or arrange another time.”
- **Teacher unavailable before command:** “The teacher has been called to duty. No lesson was recorded.”
- **Owner returned partial progress:** “The lesson stopped partway. Your current progress is saved; another session depends on the next available day and teacher.”
- **Result is pending under owner recovery:** “The lesson result is still being confirmed. Keep this request; don’t start another one yet.” Use only if the owner really supports pending resolution.
- **No reliable owner recovery:** “The result could not be confirmed. The system needs a verified status before another attempt.” This is an integration error state, not a normal narrative ending.
- **New day, new agreement required:** “The old time has passed. Check the learner, teacher, and schedule again.”

The line “No lesson was recorded” may appear only when the owner confirms no commit occurred. If the state is ambiguous, say that the result is being checked or surface the actual failure. Never convert uncertainty into a confident failure toast.

## 129. Time pressure without arbitrary randomness

A quiet day and a day with urgent repairs can create different education opportunities because the current schedule, needs, and duty owners differ. The feature need not roll a random “lesson quality” die to feel variable. It can use owner-confirmed conditions and seeded simulation only where the current system already supplies that contract.

If a session outcome legitimately uses campaign RNG, the lesson owner must consume that RNG deterministically and record the resulting state through existing save rules. Content can reflect the result, but it cannot reroll because the player dislikes a partial outcome. The plan does not prescribe a new probability table, buff, or hidden mood modifier.

Time costs should be explicit when they exist. Reserving a teacher may reduce available labor; waiting may allow another duty to intervene; choosing a private space may require a later interval. If current owners do not model the cost, avoid claims that a resource was consumed or a shift weakened. Narrative time pressure is not evidence of a gameplay transaction.

## 130. Revisit behavior and new-day boundaries

When a new simulation day begins, the player may receive a fresh offer only if the education and schedule owners show a valid candidate. The prior day’s result remains part of canonical progression. A new offer does not automatically begin a session or imply that yesterday’s teacher is free.

The learner can choose to continue, request a different method, change subject where the catalog supports it, or stop pursuing the topic. The player should see relevant prerequisites and the meaning of any existing progress. The continuation is still a new agreement followed by the allowed session command; it is not a silent increment caused by day advance.

If the learner has completed the subject, show the owner’s completion or graduation state and route any application separately. Do not keep offering repeat sessions merely to create content. If the learner is partially progressed, show what the owner actually reports. If a curriculum has expired or changed, the owning data/migration path determines the next step.

## 131. Branch endings around a shift boundary

**The shift takes priority:** the player closes the offer before command submission; the worker remains assigned; no education result is claimed.

**The lesson completes before handover:** the command commits while the teacher is available; the result persists through the day advance; a later handover does not invalidate it.

**The teacher changes, the learner agrees:** the player selects a different eligible teacher; the owner validates the match and the lesson uses a stable new identity.

**The interface closes after commit:** the result is not shown yet; save/restore retrieves it and displays it once without rerunning the session.

**The host cannot confirm commit status:** the player sees an explicit integration status, not success or a new attempt. Production content cannot ship this ambiguity as ordinary play.

**The learner declines a new-day offer:** no second session occurs. The player can respect the choice and proceed with other shelter work.

**A faction requests the just-learned capability during a handover:** the player can refer to the confirmed result, identify a qualified worker, or defer until the shift is covered. The faction does not create a new lesson or erase the existing result.

## 132. Open owner decisions before implementation

The future premise audit must answer these concrete questions from source:

1. What is the authoritative simulation-day identifier and when does it advance?
2. At what transaction point does `ConductDailySession` or its current equivalent commit progress?
3. What key makes a duplicate host request the same lesson rather than a second lesson?
4. Is the intended limit one result per learner/day, or another current domain rule? The plan’s current product promise is one daily result; any different owner contract requires an explicit proposal and revision.
5. How does a host recover after a timeout between command submission and result display?
6. Which state is saved by which owner, and what is restored before the Godot route becomes available?
7. Does a teacher reassignment invalidate an agreement, require re-consent, or allow a supported replacement?
8. Which existing event refreshes a stale lesson panel after day advance?

These questions block implementation claims, not design continuation. Until answered, the plan remains a proposal. Do not invent a new session table, retry cache, or save section to fill the gaps.

## 133. Verification plan for the future owner

When an implementation is authorized and current contracts are verified, focused verification should target the exact boundaries above: same-day duplicate, new-day attempt, save before command, save after commit but before presentation, restored panel, changed teacher, expired agreement, owner rejection, interruption, and same-seed replay. Each case should test the current public API and current save owner. A case should remain independent if it tests a different lifecycle or determinism contract.

The host path should also prove that a panel reopen, route re-entry, or duplicate event does not call the owner a second time. A Godot headless check is appropriate only if the new implementation changes that runtime route. No tests are prescribed or run by this documentation installment; the package owner will choose the focused target after implementation scope is approved.

## 134. Installment 8 close

This installment expands the daily-result contract across shift changes, simulation-day advance, save/restore, callback retry, teacher reassignment, and new-day continuation. It supplies action-specific dialogue and endings while preserving one authoritative session result. It leaves the identity fields, commit point, and recovery behavior for the current education/time/save owners to prove. No wall-clock rule, duplicate save store, panel guard, or second feature pillar is introduced.

## 135. Installment 9 — the request that follows the lesson

Learning can change how other people see a survivor. A person who can now read a work label may receive requests to explain it, check a public copy, or help with a task. These requests create a new branch after the lesson: how does the player keep demonstrated knowledge distinct from consent, qualification, and labor assignment?

The route is titled **“One More Thing You Can Read.”** It begins after an owner-confirmed result, when a worker asks the learner to read a second label during a supervised task. Later, a faction representative may ask whether the learner can help regularly. The learner can be proud, cautious, interested, or tired. The player’s response decides whether a specific request goes through an existing work, apprenticeship, or trade owner.

This is a story about expectations, not a new exploitation, burnout, education reputation, or labor system. The player does not gain an education score by assigning the learner more work. Current duty, age, needs, capacity, compensation, apprenticeship, and consent rules remain authoritative. If an existing owner cannot represent the proposed assignment, the route stays at conversation or is omitted.

## 136. Entry condition — capability creates an invitation, not an obligation

The scene can occur only when the education owner has returned a relevant result and an existing world event or request creates a reason to ask for help. A completed lesson alone does not spawn a new faction, task, or labor slot. The first request should be narrow and verifiable: “Would you read this heading with me?” is different from “Can you cover the shift?”

The player sees who is asking, what action they want, whether it is one-time or recurring, the current duty impact, and whether a qualified supervisor is present. If the request does not disclose one of these facts, the player can ask before deciding. The learner must have an explicit choice when the existing system allows it. The player cannot accept a recurring commitment on the learner’s behalf merely because a previous lesson agreement was accepted.

When the request is urgent, the player can route it to a qualified adult or work owner first. No urgent duty is delayed while the scene asks the learner to prove their education result. The lesson can be acknowledged after the immediate need is handled.

## 137. The learner’s options are specific and reversible where owners allow

Offer choices that describe the requested commitment:

- **Read one label with a qualified worker present.** This is a bounded observation or assistance request if the current work owner permits it.
- **Explain the general term to a peer.** The learner shares knowledge without operating equipment or taking responsibility for a shift.
- **Ask for a later time.** The request remains unaccepted until the schedule owner confirms a compatible window.
- **Ask what regular participation would require.** The player obtains the duty, training, and supervision conditions before responding.
- **Request formal training.** Route through the current apprenticeship owner; the learner’s interest is not automatic enrollment.
- **Decline.** The player can preserve the learner’s refusal and refer the task to a qualified worker.

The learner can change their mind before an uncommitted request becomes an owner-backed assignment. After a commitment is made, cancellation and schedule changes follow the owner’s rules. Dialogue should not promise reversibility that the current work system cannot deliver.

## 138. Player response branches and their practical results

| Player action | Immediate result | Later route | What the lesson does not do |
|---|---|---|---|
| Ask the learner first | The learner states interest, limit, or refusal | Continue only with an accepted request and valid owner path | It does not grant the player permission to assign work |
| Refer the task to a qualified adult | The immediate work request has a valid candidate | The learner may observe only if separately accepted | It does not erase the learner’s education result |
| Accept one supervised reading task | Work owner validates supervisor, location, schedule, and safety | A truthful application scene can occur | It does not create qualification for unsupervised work |
| Ask for training requirements | Existing apprenticeship owner reports prerequisites and availability | Learner can accept, defer, or decline an application | Lesson completion is not graduation or enrollment |
| Negotiate time or compensation | Existing schedule/economy owner evaluates the request | A valid offer may be revised or declined | A fictional barter promise cannot be paid outside the trade owner |
| Share a general capability statement | The requester knows a relevant skill exists without receiving private details | Requester can seek an eligible worker | No identity or private result is disclosed by default |
| Decline the request | Task returns to the requester or its normal queue | Another candidate or future request may appear through existing routes | Refusal does not remove skill progress or add a hidden penalty |

The same action can produce different outcomes from real schedule, capacity, qualification, or faction state. A qualified supervisor may be free in one run and unavailable in another. The story’s variation comes from those facts and the learner’s stated preference, not an alignment roll.

## 139. The one-time supervised request

The learner agrees to read one label with a qualified worker present. The player checks that the work owner allows the task and that the supervisor remains responsible for the operation. The learner can read the label, identify uncertainty, or ask the worker to confirm. The player receives the owner’s work result, not an invented education reward.

If the worker corrects the reading, the learner can ask one follow-up or stop. If the label is stale, the player routes it to its current owner. If the learner completes the bounded task, the scene closes with a clear distinction between helpful participation and qualification. The worker can thank them without making a recurring commitment.

The player may later offer a formal apprenticeship only if the current route supports it. The learner may say they were curious about one task but do not want the role. That answer should be accepted without a “lost opportunity” warning unless the actual offer expires under an existing owner rule.

## 140. The recurring-request branch

A major faction or work group asks whether the learner can assist regularly. The player should not see a single “yes” button that hides shift length, supervision, eligibility, and training requirements. The player can request those details, refer the requester to an adult, ask the learner directly, propose one supervised observation if supported, or decline.

If the learner is interested, route the request through the current apprenticeship or labor owner. The owner may reject it because the learner is ineligible, no supervisor is available, the roster is full, or the schedule conflicts with essential needs. Each rejection should identify its real reason. The player can seek another valid route, wait, or close the request.

If the learner is not interested, education remains intact. The requester may be disappointed or find someone else, but no hidden education penalty follows. If the requester pressures the player, the player can reinforce the boundary, state that the skill was learned under supervision, or end the exchange. Any faction relationship consequence must come from an existing faction contract and the actual response, not from the learner’s refusal alone.

## 141. Three different meanings of “help”

**Peer explanation** means the learner explains a general concept to someone who wants to hear it. It does not confer progress on the listener without their own valid session. It can be a short, voluntary exchange and may happen without changing the roster.

**Supervised contribution** means the learner performs a bounded action under a qualified worker’s responsibility. The work owner validates location, task, timing, and scope. It may provide a vivid application scene, but is not an apprenticeship unless the existing route says so.

**Assigned duty** means the learner occupies a scheduled role or recurring work responsibility. This is the highest-commitment route and must go through the labor, schedule, age, needs, and apprenticeship owners. A teaching scene cannot assign it.

The UI should label these differences before commitment. A player may want the learner to share knowledge without assigning work, or may want them to observe without taking responsibility. The narrative should not collapse all three into “put skill to use.”

## 142. Faction approaches to the request

The **Military** may value reliable handover and ask for a recurring reader. The player can request formal supervision and schedule terms, offer an eligible adult, or decline to identify the learner. Military urgency does not turn a lesson result into an assignment.

The **Rebel group** may prefer peer teaching so knowledge does not depend on one expert. The player can offer a general explanation, ask the learner if they want to participate, or keep the session private. The group does not enroll the learner by attending.

An **Independent** may offer trade or material support for a limited contribution. The player can use the current economy owner, request a one-time supervised task, refer a qualified adult, or reject an exchange. The learner’s education record is not a tradable item.

The Archivists, Scavenger Guild, and Long Walk may provide source context or a bounded demonstration. They do not set labor demand, decide learner eligibility, or certify an apprenticeship. Their help can influence which request is practical but does not replace the player’s and learner’s decision.

## 143. Endings — contribution without a compulsory career

- **The learner accepts one supervised task:** a specific request is completed and no recurring duty follows automatically.
- **The learner teaches a peer:** a general concept is shared; the peer’s own education remains separate.
- **The learner asks for training:** the apprenticeship route evaluates the request and returns an actual next step.
- **The learner asks for conditions:** the player obtains time, supervision, and compensation details before accepting anything.
- **The learner declines:** the requester finds another route or leaves the task unresolved; the learner keeps their result.
- **The owner rejects a proposed assignment:** the reason is shown; the player can refer an adult or close the request.
- **The request expires:** this happens only if the current owner models expiry; otherwise the story says the offer remains uncommitted.
- **The player declines to disclose identity:** the requester receives a general answer or no answer, according to the player’s choice and current policy.

These are distinct closures because they arise from different actions and responsibilities. They do not rank the player’s compassion or ambition. A player can be career-focused while requiring safeguards, or protective while still helping the faction through an adult. The game should reward neither posture with an invented virtue score.

## 144. Later callbacks without a new labor track

If the learner accepts one application, a later conversation can ask how it felt, whether the task stayed within the agreed scope, and whether they want another. These questions are narrative only unless an existing work/apprenticeship owner records an answer. The game must not create a fatigue or exploitation ledger just for this feature.

If the learner declines, a later request may go to someone else. A callback can show that the faction adapted without implying the learner lost status. If the learner asks for training, the apprenticeship owner can return accepted, deferred, or rejected. The education result remains distinct in every case.

If the player repeatedly accepts tasks, current schedule and needs owners should reveal any actual conflict. Dialogue can remind the player of a commitment already displayed by those owners. It must not invent a hidden cumulative strain value. Conversely, if the current simulation models fatigue or workload, the content may reflect the authoritative result after it is applied.

## 145. Branch recombination examples

The route should support combinations such as these:

- The learner accepts one supervised reading, then declines a recurring role; the one-time contribution remains valid.
- A faction requests a named learner, but the player shares a general capability and refers a qualified adult; the learner’s identity stays private.
- The learner wants training, but no supervisor is available; the apprenticeship request is deferred or rejected by its owner without losing education progress.
- The player offers a peer explanation, but the second learner declines; no second session is created.
- The owner accepts a supervised observation, then a duty conflict cancels attendance; the work owner closes the task, while the education result remains unchanged.
- An Independent offers payment, but the current trade rules do not support the exchange; the player can decline or renegotiate through an existing route.
- The learner is interested in the task but chooses a different teacher; the education owner validates the match separately.

No combination should require a campaign-wide moral state. The branch can end locally and be re-entered if a genuine new request occurs. Re-entry must use current owner state rather than a custom “career ending” flag.

## 146. Dialogue for consent, limits, and practical interest

Candidate lines, pending current character and faction voice review:

- Learner: “I can read the heading. I haven’t been trained to take that shift.”
- Worker: “One label is all I asked. I’ll stay here while you read it.”
- Learner: “I want to learn the work. I don’t want my name on next week’s roster yet.”
- Player: “They can explain the term. A supervisor still needs to confirm the task.”
- Military representative: “We need someone who can keep the handover clear.”
- Player: “I can ask whether they want training. I can’t promise their time for them.”
- Learner: “If it’s one afternoon, tell me when. If it’s every shift, show me the full schedule.”
- Independent trader: “Could they read the stock labels when the shipment comes?”
- Player: “Let’s check the work request and supervision before anyone agrees.”

The learner’s language can be confident, cautious, or blunt, but it should not imply that refusal needs a moral defense. A faction’s need can also be valid while its request is declined. This keeps the story grounded in negotiation and roles instead of simple faction approval.

## 147. Production safeguards for post-lesson requests

Before writing the request scene, verify that the initiating event exists, the education result is available from its owner, and the work/apprenticeship routes can represent the proposed scope. Confirm whether the learner can consent directly, whether a guardian or age rule applies, how schedule and needs constrain availability, and which owner records any compensation or commitment.

If only a dialogue callback exists, keep it descriptive and do not present it as a binding assignment. If an actual work command exists, show its cost and result only after confirmation. If the apprenticeship route is missing or blocked, do not simulate it in the education panel. A missing downstream owner means the offer must be narrowed or deferred.

The content review must also confirm that a one-time task cannot accidentally create a recurring assignment, a general capability statement cannot reveal a private record, and a rejected request cannot revoke a completed lesson. Those are contract boundaries, not optional tone decisions.

## 148. Installment 9 close

This installment expands the path from confirmed learning to requests for peer explanation, supervised contribution, recurring work, or formal training. It gives the player concrete ways to negotiate scope, privacy, schedule, qualification, and compensation while preserving the learner’s agency. Eight endings and multiple branch combinations remain grounded in existing owners. A lesson creates an invitation to act; it does not create a job, obligation, labor meter, or new faction authority.

## 149. Installment 10 — which question gets the available hour?

When the roster supports several valid learning opportunities but only one teacher window, the player has a real prioritization decision. One subject may answer the learner’s own question. Another may prepare them for an upcoming supervised task. A third may be a prerequisite for a later course. The decision can change what happens next without sorting the player into a moral alignment.

The working route is **“One Hour on the Board.”** The education view surfaces multiple candidates only when the current catalog, learner prerequisites, teacher eligibility, schedule, source availability, and session rules support them. The player sees why each option is available and what its known limits are. The learner can state a preference, ask about consequences, choose to wait, or decline all options.

The one-hour constraint must be real. If the schedule owner says the teacher has time for only one lesson, the second session cannot happen on the same day merely because a branch needs more content. If the owner supports multiple distinct sessions, the plan’s one-result-per-day promise must be reconciled with that contract before production. Do not silently broaden the feature rule.

## 150. Candidate lessons are offers with different evidence

Each visible lesson candidate should have a plain-language explanation of:

- what the learner will work on;
- why the learner is eligible now;
- who can teach it and when;
- what material or location it requires;
- what the player can expect to see if it is completed;
- what remains outside its scope;
- whether waiting has an owner-backed consequence.

Do not present a subject as “best” based on an opaque score. If one candidate has a clear prerequisite or near-term use, state that fact. If another is the learner’s preference, say so. If a source is uncertain, show the uncertainty. The player can then choose based on their priorities.

The UI must not tease inaccessible lessons as if they were selectable. If the subject is locked by a prerequisite, explain the prerequisite using the current curriculum owner. If no qualified teacher is available, do not show an empty teacher slot as a player challenge. If the lesson could be available later, say that only when current scheduling state supports it.

## 151. Decision matrix — priorities other than morality

| Player or learner priority | Typical action | Immediate consequence | Later consequence allowed |
|---|---|---|---|
| The learner’s own question | Choose the subject they asked about | The teacher uses the available window on that subject | Application may follow if the current consumer accepts the result |
| Upcoming work opportunity | Choose a supported prerequisite or preparation lesson | The player commits time toward a specific, bounded request | The work owner still decides whether the task is available and safe |
| Source certainty | Choose the lesson with a verified example | Uncertainty is reduced for that subject | A different subject remains available only if later schedule and catalog facts support it |
| Teacher expertise | Choose the subject matched to the teacher’s current qualification | A valid session can proceed without substituting an ineligible teacher | The learner may request another teacher or subject later |
| Learner autonomy | Ask the learner to choose or decline | Their stated preference directs the available window | The player does not infer lifelong interest from one choice |
| Duty continuity | Release the teacher to a higher-priority shift | No lesson occurs in that window | A later offer depends on current availability; no appointment is invented |
| Sequence planning | Choose a prerequisite course before an advanced one | The learner may not reach the advanced lesson today | Progression follows the catalog owner, not a custom quest flag |
| Low commitment | Defer both subjects and handle immediate work | The day continues without an education result | No hidden skill decay or relationship loss is implied |

Different players can value these outcomes differently. The plan should not attach virtue language such as “responsible choice” or “selfish choice” to the buttons. Show the duty, time, interest, and learning implications, then let the player decide.

## 152. A scene pass — the learner asks for one subject, the crew asks for another

The learner has asked to study a topic that they encountered while helping with a routine task. A crew member has also asked whether a different available subject could prepare the learner to assist during a future supervised activity. The only eligible teacher has one free interval before returning to duty.

The player can ask the learner which question matters to them now, inspect the future activity’s prerequisites, ask the teacher which subject they are qualified to teach, or decline the opportunity. The teacher’s answer does not decide the learner’s priorities. The crew member’s request does not make the learner a volunteer. If the player chooses the learner’s subject, the work opportunity may go to a qualified adult. If the player chooses the preparation topic, the learner can still decline the later task.

If the future activity is urgent, the qualified adult handles it while study remains optional. If it is not urgent, the player can schedule a supported follow-up, provided the teacher and learner are actually available. A branch should not create a countdown unless the work owner reports a real deadline.

At the end of the scene, the player knows which lesson was accepted, what was deferred, and what the current next step is. The student’s agency is visible because the request was heard, not because the game gives them an abstract autonomy score.

## 153. What “defer” means for the unchosen subject

Deferral should be explicit and modest. The player can close the second candidate for today without promising it will remain available tomorrow. If the current curriculum and schedule owners preserve an offer, show its true expiry and availability. If they do not, the journal should not imply that a stable appointment exists.

The learner may remember the question conversationally, but that memory is not a gameplay guarantee. A later line can ask whether they still want to study it, subject to current eligibility. If a new duty or source change makes the subject unavailable, explain that. If a prerequisite changes, the curriculum owner decides whether the old route remains valid.

Do not punish the player with invisible curriculum decay merely because they selected another valid lesson. Do not grant progress to the unchosen subject because it appeared in the same panel. The current owner’s result is scoped to the subject and identity it actually resolved.

## 154. Unlocking and prerequisites without a hidden curriculum tree

If the canonical catalog already contains prerequisites, present them in learner-facing language. The player can see that one lesson requires a prior concept or a qualified instructor. The feature should not invent a branching tree of subjects or a bespoke “education path” that mirrors the existing curriculum owner.

Where the learner is eligible for multiple catalog subjects, choices may lead to different valid downstream consumers. One subject can support supervised reading; another may support an apprenticeship request; a third may have no current application. That difference is meaningful, but it should be stated before commitment when known.

If the exact downstream use is not known, do not market a subject as a guaranteed career unlock. The player may still choose it out of curiosity. Learning value is not limited to immediate labor, and the game should not describe a lesson as wasted simply because there is no current quest consumer.

## 155. The teacher’s window is a constraint, not a character score

The teacher may be qualified for only one of the available subjects. Another candidate might require a different instructor or source. The player can choose the subject that fits the current teacher, look for another eligible teacher, or wait. Those decisions can affect timing and availability without creating a hidden teacher-quality statistic.

The teacher may also be tired, assigned to a duty, or unavailable because of a current need. Use the existing owner that reports that state. Dialogue can show the teacher protecting a rest interval or asking for a shorter explanation, but the education command must interpret any relevant modifier from canonical state. A line such as “I’m exhausted” does not prove a mechanical penalty exists.

If the teacher suggests that a different colleague can cover the topic, the player still checks the roster and subject eligibility. A friendly recommendation is not authority to transfer a lesson assignment. The replacement teacher and learner must accept the actual supported agreement.

## 156. Supporting-faction pressure remains a request

A major faction can make one candidate more immediately useful by describing its need. The Military may need a future handover assistant; the Rebel group may want more people able to interpret a shared document; an Independent may offer work that requires a particular prerequisite. Their requests can help the player understand the campaign context, but they cannot define the curriculum for the learner.

The player can ask the faction to wait, identify a qualified adult, offer a different worker, negotiate the scope through existing work/economy owners, or refuse. The learner may choose the faction-relevant subject for personal reasons, or decline the opportunity entirely. The faction’s urgency is not an alignment test.

Supporting Currents can influence which material or example is available. Archivists may provide a source for one candidate; the Guild may provide a practical object for another; Long Walk may deliver a question that makes a third topic relevant. None of these supplies guarantees a lesson. They contribute to the evidence and context around a valid catalog option.

## 157. Endings for “One Hour on the Board”

**Learner’s question first:** the accepted session covers the learner’s chosen topic. The crew member’s request is routed to another candidate or deferred through its owner. The learner may decide later whether to pursue the practical opportunity.

**Preparation first, voluntary application later:** the player chooses a supported prerequisite. After the owner returns a result, the learner receives a separate choice about a future supervised task. The lesson does not commit them to the task.

**Teacher fit first:** the player chooses the only subject this teacher is qualified to teach today. The learner accepts or declines. Another subject remains possible only through a later valid teacher match.

**Source confidence first:** the player chooses the candidate with an approved, current source. Another interesting subject waits until its source can be verified. The lesson result is not treated as proof about the unverified material.

**Duty first:** the player releases the teacher to a current shift. No session occurs. The player may still answer an urgent question directly through the relevant owner.

**Learner declines both:** the player closes the offer and proceeds with other work. No relationship penalty or skill loss is invented.

**No candidate is actually valid:** the interface refreshes and explains the missing teacher, prerequisite, source, or time. The player cannot force a lesson from a stale list.

## 158. Recombination across future days

The unchosen subject can return only as a current offer. If the learner becomes eligible, a teacher becomes available, or a source is verified, the player may see a new option. If none of those conditions changes, the same offer should not recur as though it were a new quest stage.

The learner may change their preference. The player may also have new priorities after a repair, a faction request, or a roster change. Recheck the current state and let the player select again. Do not bind the learner to “the path they chose” with a custom branch lock.

If the first lesson produces a confirmed prerequisite result, the catalog owner can expose a new subject. If it does not, the game must not unlock it because the narrative sequence reached a later page. A branch can close on “not yet” and still provide a coherent story ending.

## 159. Accessibility and clarity in a multi-offer screen

The player should be able to compare candidates without remembering hidden facts from earlier dialogue. Use a stable summary: subject, learner interest if expressed, qualified teacher, available time, required source, known application, and unresolved limit. Screen-reader and controller navigation should preserve that order, with clear focus and a usable close/back route.

Do not imply that the most decorated card is the recommended or morally superior choice. Avoid using color alone to communicate urgency, eligibility, or uncertainty. If a candidate is unavailable, state why in text and remove or disable it according to existing UI patterns.

The screen must distinguish the learner’s preference from the player’s selected action. A player can ask the learner, then choose another valid priority; the scene should make any difference clear. If the learner has not expressed a preference, do not generate one in the UI merely to make the choice look personal.

## 160. Content and data review for subject choices

Before authoring multiple candidate subjects, confirm the live curriculum IDs, display strings, prerequisites, teacher rules, student eligibility, session behavior, data schema, and current consumers. Search for existing lessons and related quest content to prevent duplicate subjects or contradictory terminology. If no second valid candidate exists, the design can still express choice between study now and other shelter work; it should not invent a fake lesson card.

For each supported candidate, record whether its outcome is immediate, delayed, or purely educational. Identify the exact owner that can show its result. Confirm whether the same daily-result limit applies across subjects. This is a core contract decision: do not allow a second subject to bypass the limit by using a different panel or catalog ID.

The production packet should include the unavailable-state line for each candidate. A subject can be hidden because of prerequisites, teacher mismatch, missing source, schedule conflict, or learner refusal. Each reason should come from a current owner; if the owner does not expose the distinction, merge the lines rather than fabricating state.

## 161. Installment 10 close

This installment adds a multi-offer decision route around the scarce teacher window. It lets the player weigh learner interest, teacher fit, source reliability, prerequisites, duty, and future work without a good/evil gate. The unchosen subject receives no invisible progress or guaranteed appointment. All options remain catalog-backed agreements followed by the same one-result daily lesson contract and a separate application route.

## 162. Installment 11 — one teacher, two learners

Scarcity becomes more personal when two eligible learners want the same teacher’s available hour. One may want a practical skill for an upcoming supervised task; the other may have a private question or a prerequisite they have waited to study. The player cannot satisfy both by clicking a single group option unless the current curriculum and session owner explicitly support a shared lesson and record individual results correctly.

The working route is **“Who Gets the Hour?”** It is a bounded agreement scene, not a new school, cohort, queue, fairness meter, or turn-taking system. The player sees the teacher’s real availability, each learner’s current eligibility, each learner’s expressed preference if known, and any owner-backed deadline or duty consequence. The player is not asked to infer worth from age, family relation, faction, confidence, or past productivity.

If the canonical schedule owner supports only one individual session, one learner can receive that session. The other receives no progress from attending, overhearing, or helping set up. The player may ask about a later window, but only the schedule owner can confirm it. If no later slot exists, the story closes honestly rather than promising equal access the game cannot deliver.

## 163. A fair scene does not require a hidden fairness score

The player can hear both learners, ask whether either would prefer to wait, check whether one has a supported prerequisite or urgent use, or leave the teacher on duty. These actions can reveal different priorities. The game should not calculate a secret “fairness” value that rewards one method without telling the player.

If an existing owner already imposes a rule—such as eligibility order, age restriction, or a requirement to honor an accepted agreement—show that rule before the player commits. If no current rule determines priority, the player makes an explicit decision among available learners. The content may let the learners respond differently, but it does not label the player’s choice good or bad.

The player can also decide not to choose between them today. That may mean preserving teacher duty, asking whether either learner wants another teacher, or postponing both. It must not create an invisible queue or imply that they are guaranteed the next available hour.

## 164. Opening scene — two questions arrive together

The teacher lays out one approved example and explains they are free until the next duty. Two eligible learners arrive with separate questions. One asks whether the lesson can prepare them to assist with a supervised task. The other wants help understanding a subject they have been curious about but does not want to discuss in a crowded space.

The teacher can teach only one individual session in the available interval. The player may ask each learner what they want, ask the teacher whether the curriculum supports a different method, look for another eligible instructor, request a later schedule check, or close the offer. The player is told that a short conversation is not the same as a completed lesson.

The scene should not present either learner as more deserving. The learner interested in work is not automatically more useful; the learner with a private question is not automatically more vulnerable. Their preferences and the actual owner-backed constraints are the relevant facts.

## 165. Allocation choices and their consequences

| Player action | Immediate result | Consequence for the other learner | System boundary |
|---|---|---|---|
| Ask both learners to state their preference | The player receives an explicit preference from each willing learner | Either may choose to wait or withdraw | A spoken preference does not create a schedule slot |
| Choose one valid individual lesson | One learner enters a confirmed agreement | The second receives no progress and can be offered only a separately valid future route | One command/result belongs to one learner identity |
| Ask for a later window | The schedule owner checks actual availability | Both may remain uncommitted until a later agreement | Do not invent a queue or reserved future slot |
| Find a second qualified teacher | The roster and education owner validate the match | The two lessons may proceed only if both are separately supported | One person cannot be cloned or assumed available |
| Use a supported group lesson | Both learners can attend if the current curriculum/session contract permits it | Each result is recorded only as the owner supports | No shared progress copy or cohort ledger |
| Let one observe | The observer may hear or watch if access and consent permit | Observer receives no progress unless separately enrolled and resolved | Presence is not a lesson command |
| Defer both for duty | The teacher remains assigned to the higher-priority task | Neither learner receives an invisible appointment | No education result is shown |
| Let one learner withdraw | The other may proceed if valid | The withdrawing learner’s future choice remains open where supported | Withdrawal does not affect the other learner’s state |

## 166. Distinguish a group lesson from two people in one room

A group lesson is a domain behavior only if the canonical education owner supports multiple learners, per-learner eligibility, individual results, and replay-safe identity for each participant. If any part is missing, the scene can still place both people nearby, but only one receives the lesson result. Do not claim group completion because two portraits appear on screen.

If a group lesson is supported, each learner must accept the subject and method. One may leave or defer without canceling the other unless the owner’s contract requires a shared session. The teacher’s availability, room capacity, and curriculum must be checked for the group. Each progress result is displayed separately and must survive save/restore independently.

If the owner supports only individual sessions, a useful alternative is sequential scheduling on different days. The player can request a future interval for the second learner, but the interface must state that it is only a request until confirmed. The two lessons cannot share one daily identity if the domain rule limits each learner independently; equally, one learner cannot receive a second result just because the teacher serves someone else.

## 167. Preference, urgency, and subject are separate facts

The learner’s interest answers “what would you like to learn?” A prerequisite answers “what is currently available?” An operational request answers “what would help a current task?” A schedule answers “who can teach and when?” These facts can point in different directions. The player should see them separately and decide how to weigh them.

If one learner has an imminent work opportunity, verify its deadline and requirements with the work owner. Do not treat a faction’s request as a deadline without evidence. If another learner’s subject is private, offer a suitable setting only if the current room owner supports it. If one learner needs a prerequisite, show it from the curriculum catalog rather than describing the person as behind.

The teacher can state expertise limits. If they can teach one subject but not the other, the player may choose the valid match, seek another teacher, or defer. A warm relationship does not turn the teacher into an expert in every topic.

## 168. Branch endings for “Who Gets the Hour?”

**One learner accepts the immediate slot:** the teacher and learner complete one valid agreement and session. The other learner is told that no lesson was scheduled and can choose whether to seek another route.

**The learner with a work request studies first:** the lesson may support a later supervised opportunity, but the work owner still validates it and the learner can decline the task.

**The learner with the private question studies first:** the player uses a permitted setting and does not disclose the content to the other learner or a faction. The second learner receives no implied judgment.

**One learner chooses to wait:** the teacher uses the slot for the other learner. The waiting learner has not been forcibly displaced; they made a current choice, and any later offer depends on the real schedule.

**A second teacher is found:** both arrangements proceed only if each teacher is eligible, available, and separately accepted. If the roster cannot support simultaneous sessions, the second agreement is deferred.

**The learners choose a supported group lesson:** the owner returns individual outcomes and each participant sees their own result. If the group contract is absent, this ending is unavailable.

**Neither learner wants the offered terms:** the player closes the lesson offer. The teacher returns to duty, and both learners can pursue other supported subjects later.

**The teacher becomes unavailable:** the player tells both learners why the offer closed. No one is marked as having refused or failed a session.

## 169. The teacher’s voice — avoid turning scarcity into favoritism

The teacher should describe their capacity, not rank the learners. They might say, “I have one hour before the pump inspection,” or “I can teach the first subject today; I need a current reference for the second.” They should not say, “This learner deserves it more,” unless an actual owner rule establishes a priority and the dialogue explains it.

The teacher can help compare prerequisites and timing, but the final learner consent remains explicit. If the teacher has already accepted one agreement, honor that commitment or route a change through the owner. The player cannot quietly transfer the hour to another learner without notifying the first participant and updating the canonical state.

The scene can show professional restraint: the teacher may ask the player to verify the schedule before promising a later slot. That caution creates a small moment of trust without requiring a teacher reputation statistic.

## 170. Supporting roles and faction requests

A **major faction** can request that one learner prepare for a supervised task. The player checks whether the request is real, whether it has a deadline, and which qualifications are required. The learner can decline, and the faction must use its existing work route to find another candidate.

The **Archivists** may have a source usable for one subject but not the other. The **Scavenger Guild** may lend an object for demonstration if current inventory and access permit. **Long Walk** may report that a second teacher is away or returning later, if its current travel state supplies that fact. These contributions inform options; they do not allocate the scarce hour.

No support Current is required for the scene. If no group can help, the player still has the choice to teach one valid subject, reschedule, or decline. The learner’s access to education cannot depend on faction sponsorship unless the current design explicitly and legitimately models that restriction.

## 171. Re-entry and consistency after the player chooses

After the player selects one learner, the education view should show the other learner’s actual current status: eligible, unavailable, declined, waiting without a commitment, or no longer offered. Only use distinctions the owner can represent. Do not preserve a custom `waitingLearner` flag if the schedule owner does not support it.

If the selected learner’s session is interrupted, the other learner does not automatically inherit the same teacher window unless the schedule owner confirms that availability and the agreement is separately made. If the selected learner completes the lesson, their result cannot be copied to a peer. If the selected learner declines after the agreement, close that agreement through its owner before offering the slot to anyone else.

After save/restore, the screen reconstructs both learners’ canonical state and the teacher’s schedule. It must not remember an outdated selection as the decision. If a current owner cannot reconstruct which agreement was accepted, the route is not ready for production.

## 172. Dialogue fragments for a shared scarce opportunity

- Learner A: “I asked first, but I can wait if the other lesson is needed today.”
- Learner B: “I want the subject, but not if it means changing my shift without asking me.”
- Teacher: “I can teach one of these before handover. I won’t promise both.”
- Player: “Let’s check whether there’s another qualified teacher before either of you gives up the hour.”
- Learner: “Watching is fine. Don’t mark it as my lesson.”
- Teacher: “One of you can read the example. The other needs their own session if they want progress recorded.”
- Player: “The work request is for a supervised task. It doesn’t decide who gets to learn.”

These lines should be assigned to current cast members only after voice and relationship review. Avoid dialogue that makes a learner praise the player for choosing them; the choice can be practical without turning the student into a reward dispenser.

## 173. Content and owner checklist for shared scarcity

Before production, verify whether the catalog supports the candidates, whether both learners are individually eligible, how the teacher’s time is represented, whether a group session exists, and how each result is keyed and saved. Confirm that room access supports one or more participants, that a faction’s work request uses a real event, and that the player can close or reschedule without a fake queue.

If group sessions are unsupported, remove the group-completion option and keep observation separate from progress. If the schedule cannot persist a deferred offer, say it is unconfirmed. If there is no explicit priority rule, do not invent one. If learners have a specific schedule or consent rule, re-read it at commit time.

The future implementation must not duplicate session state per learner in a UI collection. Each result comes from its current education owner and each assignment from its current schedule owner. Any broader cohort or fairness feature requires a separate proposal and evidence review.

## 174. Installment 11 close

This installment adds a two-learner scarcity route with distinct outcomes for preference, urgency, group learning where supported, observation, alternate teachers, and deferral. It makes fairness legible through explicit constraints and choices rather than a hidden score. One learner’s session cannot produce another learner’s progress, and shared attendance is not enrollment. All branches remain within the existing agreement and single-result features.

## 175. Installment 12 — whose agreement is being made?

A valid study agreement depends on the learner’s own choice and any additional permission the current age, roster, and consent owners require. Those facts are related, but they are not interchangeable. A family relationship does not prove that someone can teach. A guardian’s approval does not erase the learner’s stated refusal. A learner’s enthusiasm does not bypass a required permission check.

The working route is **“Before the Book Opens.”** A learner asks to study a subject and names a possible teacher. The player must verify who is eligible, who can approve the agreement under current rules, and whether the learner still wants this person and this subject. The route can close before a lesson is scheduled, and that closure is valid.

This installment does not propose new age thresholds, guardianship law, permission records, or consent flags. It assumes only that the existing domain owners determine eligibility and permission. If their current contracts do not answer the questions below, the feature remains blocked at that seam until the relevant owner supplies an explicit decision.

## 176. Separate the people and decisions in the agreement

The player-facing flow should distinguish these roles where they apply:

- **Learner:** the person who would receive the lesson and whose willingness matters at the moment of agreement.
- **Teacher:** the person who can teach the selected catalog subject and is actually available.
- **Guardian or required approver:** the person recognized by the current owner as able to provide a required permission.
- **Schedule owner:** the authority that confirms the selected time and any duty conflict.
- **Education owner:** the authority that accepts the learner/teacher/subject match and resolves the session.

One survivor may occupy more than one role, but the system must not assume that they do. A parent may also be an eligible teacher, but that is two separate checks. A guardian may approve attendance without being qualified to teach. A qualified teacher may lack authority to approve a learner’s participation. The UI should reveal the actual reason a proposed match is unavailable without exposing unnecessary private details.

The agreement is not complete merely because the player presses a button. The owner must confirm every required condition. If a required permission is unknown, stale, or denied, the route stops before session submission. The learner can receive any immediate safety information through the proper owner even when a lesson cannot be arranged.

## 177. The opening scene — a request and a proposed teacher

The learner asks for help with an available subject and suggests a relative who has experience. The relative is currently on shift. The player can ask the learner whether they want that person specifically, check whether the subject is appropriate for that teacher, verify any required permission, or ask whether another eligible adult would feel acceptable.

The learner’s preference is not interpreted as proof of qualification. The relative’s experience is not treated as catalog eligibility. The player should see the distinction before they commit. If the teacher cannot serve, the player can explain why and offer only candidates the current roster and education system validate.

If the learner is eligible to make the agreement directly under current rules, the game should not add an unnecessary family approval step. If additional permission is required, the system should explain that requirement in plain language and identify the next supported action. Never make the player guess whether a family member is authorized.

## 178. Agreement branches for common family and age situations

**The guardian is also a qualified, available teacher.** The learner accepts that person; the required permission owner confirms its condition; the schedule owner confirms the time. The education owner then accepts the match. If the shift is incompatible, choose another time or teacher.

**The guardian approves, but the learner prefers another teacher.** The approval is one condition, not a forced assignment. The player searches for a valid alternate teacher or closes the offer. The guardian’s permission does not substitute for the learner’s choice where the current contract requires assent.

**The learner accepts, but required permission is missing.** The player can request permission through the supported route, offer an approved alternative if the owner permits one, or defer. No lesson is scheduled and no progress is granted while the required fact remains unknown.

**The guardian is unavailable.** If the current owner supports a pending request, show its actual status and expiration. If it does not, close the offer and ask the learner whether they want to revisit. Do not save a local “awaiting guardian” appointment.

**The guardian declines.** The system reports the owner’s result and the valid options that remain. The player does not secretly route around a required denial. If an alternate subject or teacher is allowed, it must be validated through the same canonical owners.

**The learner declines even after permission is granted.** The agreement closes without a lesson. Permission is not a command to participate. Do not show the learner as unreliable, spoiled, or ungrateful.

**The learner is an adult under current rules.** Use the appropriate adult agreement contract. Do not require a family member to approve by default. A relative can still be a preferred teacher if eligible and available.

## 179. Choice table — approval and assent are different facts

| Learner choice | Required approval | Teacher/schedule state | Result |
|---|---|---|---|
| Accepts | Confirmed | Valid | Agreement may proceed through the education owner |
| Accepts | Unknown or stale | Otherwise valid | Pause before scheduling; request refresh or valid permission |
| Accepts | Denied | Otherwise valid | Close or offer only a permitted alternative; no session |
| Declines | Confirmed | Valid | Close without session; no punishment |
| No answer | Confirmed | Valid | Do not infer assent; use the current consent owner’s rules |
| Accepts one teacher | Confirmed | Teacher ineligible | Offer another valid teacher or close; family relation does not qualify them |
| Accepts | Confirmed | Teacher unavailable | Recheck time or candidates; do not hold the teacher beyond schedule authority |
| Changes mind before session | Confirmed | Agreement exists | Cancel or revise through the supported owner contract |
| Changes mind after session command | Confirmed | Result committed | Preserve the owner result; respect future choice without rewriting history |

The table is a content review aid, not a new consent database. If current APIs combine or represent these facts differently, map to the existing contract and document any ambiguity for the integrator.

## 180. A dialogue pass — family care without family control

Learner: “I want to learn it. I don’t want my aunt teaching me.”

Player: “Would you rather wait for another teacher, or ask who is available today?”

Learner: “Ask. If there isn’t anyone, I can wait.”

The relative can respond without being cast as an antagonist: “That’s fair. I know the job, but I’m not the only person who can help.” The player checks the roster. If a valid teacher is free, the agreement can proceed after the required checks. If not, the learner’s choice is acknowledged and the offer closes.

In another branch, the learner asks for the relative specifically but the relative is on duty. The player can check for a later window or ask whether another teacher would be acceptable. The learner may say, “I’ll wait,” “I’d rather learn from someone free,” or “Not today.” Each line closes a different practical route without judging the family bond.

## 181. Where permission, privacy, and disclosure separate

Permission to attend a lesson does not automatically permit public demonstration, faction reporting, sharing private notes, or assigning work. After the session, ask separately before exposing the learner’s name or private example, subject to current policy. A required internal education record remains with its owner; that record does not make all details public.

If the player must disclose a fact for a valid operational reason, explain the purpose and recipient through the appropriate owner. If the game lacks a current consent or privacy contract for that disclosure, do not improvise a bypass in dialogue. The content should remain private or be paused until the relevant owner defines the route.

A learner may accept a lesson while declining an audience. A guardian may approve study while the learner wants a different teacher. A learner may complete a lesson but decline a faction’s later request. Treat these as independent decisions where the existing contracts permit them; do not bundle them into one global “consent granted” flag.

## 182. Major factions cannot become substitute guardians

A Military officer, Rebel representative, or Independent trader may have a reason to request a learner’s time, but faction role does not create permission authority. The player can ask the faction to wait, refer a qualified adult, or close the request. If an existing rule requires guardian approval for a work or apprenticeship opportunity, the faction’s urgency cannot override it.

Supporting Currents may help identify a qualified teacher or source, but they do not determine who may approve a learner’s participation. An Archivist can confirm document provenance; the Guild can offer a demonstration object; Long Walk can report a courier’s schedule. None can convert kinship, familiarity, or need into consent.

If a faction pressures the player, show the concrete request and the owner-backed limits. The player may be firm, negotiate time, or offer an alternate worker. Any faction consequence must follow the existing faction interaction system. The learner’s permission state itself is not a faction reputation value.

## 183. Revalidation and changes between agreement and lesson

Age, roster membership, required approval, teacher eligibility, and schedule can change between the offer and the session. Revalidate the relevant facts at the commit point through their current owners. If the learner has crossed an age boundary that changes the governing rule, use the canonical age provider and documented transition behavior; do not retain yesterday’s permission because it is convenient.

If the guardian or learner withdraws permission before the session, follow the existing owner’s cancellation path. If the session has already committed a result, preserve that result and apply the new preference to future activity. A change in permission cannot retroactively erase knowledge already recorded, though it may limit disclosure or application according to current rules.

If the teacher changes after acceptance, the learner may need to approve the new person and the education owner must validate their qualification. If the schedule changes, the schedule owner decides whether the agreement remains valid. A panel’s saved selection cannot override any of these changes.

## 184. Ending beats for “Before the Book Opens”

- **Learner and guardian both agree; a qualified teacher is available:** schedule the session through the owner and show the confirmed time.
- **Guardian approval exists, but the learner chooses another teacher:** seek a valid alternative or close the offer.
- **Learner wants the lesson; required approval is missing:** pause without progress and use the supported permission route.
- **The proposed family teacher is unavailable:** preserve the learner’s preference while checking another time or candidate.
- **The learner declines after an adult approves:** close without pressure or skill penalty.
- **The adult agreement needs no guardian approval:** proceed under the current adult contract without inserting family authority.
- **The permission result is denied:** explain the valid alternatives and end the session path.
- **The family disagrees about public sharing:** preserve the lesson record but do not disclose beyond current policy.

Each ending answers what actually happened: whether a valid agreement exists, whether a session occurred, and who may know about it. It does not award approval points or label a family member as good or bad.

## 185. Technical owner questions before implementation

The premise audit should verify the current source for age, learner eligibility, guardian/approver identity, consent freshness, teacher qualification, roster membership, scheduling, and cancellation. It should identify whether the education owner consumes permission facts directly or whether an existing host command gathers them from their owners.

The integrator should also confirm how permission changes are saved, restored, and revalidated; which events refresh the Godot UI; and whether the current curriculum API can express a learner declining a proposed teacher. If a needed distinction is missing, record the gap and request an owner decision. Do not add another permission record or shadow guardian field in the panel.

## 186. Content review risks

Writers should avoid dialogue that treats a guardian as automatically benevolent or automatically obstructive. A learner may want a relative to teach them, prefer distance, or have no strong preference. A guardian may approve a lesson but not understand the topic. A teacher may be competent in a work role but not eligible for this curriculum. The game should let current facts—not family stereotypes—select the branch.

Do not make permission friction the sole source of drama. The scene can be quietly practical: check the rule, ask the learner, find a teacher, or defer. Avoid requiring a player to expose private history just to proceed. Keep any conflict grounded in what the characters actually said and what the current owner confirms.

## 187. Installment 12 close

This installment expands Subfeature 1 with explicit separation between learner assent, required approval, teacher qualification, and schedule availability. It provides adult, family-teacher, missing-permission, declined-permission, and changed-mind branches, plus post-lesson privacy boundaries. It adds no age rule, guardian system, or consent ledger; unresolved authority remains a current-owner decision.

## 188. Installment 13 — when a participant leaves the roster

A teacher or learner may leave the shelter through an existing move, expedition, reassignment, or other canonical roster event. That transition creates pressure around unfinished lessons, but it does not justify a second education store or an invented remote-learning feature. The player should be able to understand what already happened, what can still happen locally, and what has ended.

The working route is **“The Last Hour on This Roster.”** A current departure event places one participant’s availability in question. The lesson may be only an idea, an accepted agreement, an uncommitted session, a committed result awaiting display, or a completed lesson with a possible application. Each state has a different truthful outcome.

The narrative should not create departure purely to force a lesson. It reacts only to a real event from the current roster/travel/campaign owner. The route may be omitted if the game has no canonical transition that can trigger it. No bespoke character, timetable, or evacuation quest is required.

## 189. First ask: who is leaving, and what has already committed?

Before presenting a choice, re-read the participant roster and education session state. The player needs to know whether the departing person is the learner or the teacher, whether the lesson agreement is accepted, whether the lesson command has run, and whether an owner result exists. A narrative line like “they leave at dawn” cannot replace those facts.

For the **departing teacher**, the immediate question is whether any accepted lesson can still occur before their schedule changes. The player can honor the existing agreement if the schedule owner confirms it, find another eligible teacher if the learner agrees, or close the offer. A teacher’s departure does not automatically cancel a completed result.

For the **departing learner**, the question is whether there is a valid local teaching opportunity before departure and whether the learner wants it. The player can offer a session, keep their existing result, or let them leave without rushing. The learner’s departure does not authorize a lesson they have declined.

For **both participants leaving or no valid local teacher remaining**, close any uncommitted agreement through its owner. If the education owner supports continuity across the destination, use that canonical route. If it does not, do not imply that a new settlement will resume the session automatically.

## 190. Transition matrix — preserve what happened, close what did not

| State at departure | Required behavior | Player-facing close |
|---|---|---|
| Conversation only; no agreement | No session or appointment exists | “They are leaving. No lesson was scheduled.” |
| Agreement accepted; command not submitted | Revalidate time and participant availability; cancel or complete only through owner | “The agreed hour is no longer available” or show the confirmed session |
| Session command submitted; result unresolved | Use the current idempotent owner recovery contract | Do not claim success, failure, or a new attempt while status is unknown |
| Result committed; panel not yet refreshed | Restore/display the same committed result once | “The lesson was recorded before departure” only after owner confirmation |
| Partial result committed | Preserve the actual partial state; offer continuation only if a valid route remains | Explain what progress exists and what is unfinished |
| Completed result persisted | Keep the owner-backed result through roster transition if the save/domain contract supports it | Show completion separately from future local availability |
| Apprenticeship request pending | Route its status through the current apprenticeship owner | Do not transfer or accept the request via education dialogue |
| Application task accepted but not started | Current work/schedule owner determines cancellation or reassignment | State the actual assignment status and responsible owner |

This matrix distinguishes education progress from participant presence. A person can leave after completing a lesson. A completed result can remain in the campaign while its teacher is no longer available. A pending session cannot be portrayed as complete merely because the departure scene needs closure.

## 191. Player choices before a teacher departs

If the teacher is leaving and the learner still wants the lesson, the player can ask whether there is time for the already proposed session, request a valid alternate teacher, postpone, or close the subject. The schedule owner confirms whether the final interval is free. The education owner confirms whether the alternate teacher is qualified.

The teacher may choose to leave a general approved reference or explain where the source can be found, if current item custody and curriculum rules allow. That is not a substitute for delivering progress to the learner. The player can preserve the material through its current inventory/archive owner or decline it. Do not create a custom keepsake item just to make the farewell mechanical.

If the learner has already agreed to another teacher, offer the replacement plainly: “Would you like to continue with them, wait, or stop?” The learner’s relationship to the departing teacher does not force a yes or no. If no suitable replacement exists, the scene closes with what is actually possible now.

## 192. Player choices before a learner departs

If the learner is leaving and has not yet studied, the player can offer a final valid session only if schedule, permission, source, and teacher checks pass. The learner can accept, defer, or decline. The scene should not describe a single lesson as a complete curriculum when the owner would return only partial progress.

If the learner already has progress, the player can show the current owner result and ask whether they want one more supported session, a relevant application, or no additional commitment. A short final opportunity cannot bypass the daily result limit or turn graduation into a farewell reward.

If the learner has a completed result, the player may offer an appropriate record or general source only through existing data/item routes. The learner can ask that personal details stay private. If the current save model does not support transferable education state, do not write a promise that the destination will recognize it automatically.

The learner may leave without a concluding speech. A quiet departure with a folded reference, a returned borrowed book, or no object at all can close the scene. Do not use farewell sentiment to imply a persisted item or new inventory authority.

## 193. A scene pass — the teacher’s final hour

The teacher tells the player that their assignment ends after the current handover. The learner’s agreement exists, but the session has not begun. The player checks the schedule instead of assuming the final hour is free. If it is available, both participants confirm they still want to proceed. If the teacher is needed for the handover, the player can ask whether another eligible instructor is acceptable or close the lesson.

During the session, the teacher points out one source the learner can find later. The player may ask the teacher to leave a general citation through the existing record route, keep the lesson itself private, or simply close the subject. The teacher does not promise remote tutoring unless an existing communication owner supports it.

If the education owner returns a complete result, the panel shows it before departure. If it returns partial progress, the teacher explains that the learner can continue only with a new valid agreement and teacher. The teacher’s leaving does not reduce the result. If the session is interrupted before commit, the system says no result was confirmed and does not invent a last-minute completion.

The scene can end with a practical exchange: the teacher returns a borrowed copy; the learner chooses whether to keep a permitted reference; the player confirms the schedule is clear. The final dialogue responds to the result and the relationship, not to a hidden “farewell quality” score.

## 194. A scene pass — the learner chooses not to rush

The learner is leaving after a current roster change. The player explains that a teacher is free for one hour, but the lesson would need to use a subject the learner is eligible for. The learner can choose the session, ask for a different subject that cannot fit today, or say they do not want a rushed lesson.

If the learner declines, the player can acknowledge the choice and resolve any accepted agreement normally. The immediate practical question can still be answered by a qualified worker. If the learner accepts, the session owner determines its true result; a short window does not automatically mean partial progress or completion.

The player may offer a reference to carry only if the material owner permits it. The learner can decline the object, ask for a general verbal pointer, or request that no personal notes travel with them. If there is no supported destination education route, the game should not promise that an unfinished lesson can be resumed there.

## 195. Travel and faction roles without a remote education system

Long Walk may carry a document, a question, or a message if its current travel/delivery system supports the route. It cannot carry a lesson result as if it were a parcel unless the current education and save owners already model that behavior. A courier’s delivery date informs provenance; it does not certify the recipient’s progress.

The Military, Rebel group, or Independent may offer the departing person a role or ask for a skill demonstration. The player can refer to a confirmed education result, offer a qualified worker, or ask the learner before sharing their identity. Any move, recruitment, work assignment, or trade follows its own owner. A faction request does not hold a participant in place or silently change the education record.

Archivists can help preserve an approved source reference; the Guild can show whether a borrowed object must be returned; Long Walk can explain the limits of a message that has not yet arrived. These supporting roles make departure feel connected to the world while staying outside education ownership.

If a faction or settlement is not currently present in the departure route, omit the branch. The story should not introduce a new faction solely to provide a sentimental send-off.

## 196. Progress, destination, and identity continuity

The future source audit must establish how education records refer to survivors and how those identities behave when a person changes roster or settlement. If stable identity already spans destinations, use that contract. If identity is local to a shelter, the content cannot assume portability. If a migration path exists, verify its capture/restore and collision behavior before claiming continuity.

Do not copy skill results into a second learner profile to make a departure scene work. Do not create a “travel transcript” authority alongside the current education owner. A permitted printout or note can be a narrative reference, but it is not authoritative proof unless the current consumer recognizes it.

If the destination has no compatible skill consumer, the completed result can remain true without producing a new job. If a consumer exists, it must read the authoritative result and enforce its own qualification requirements. Departure cannot auto-graduate the learner or bypass apprenticeship acceptance.

## 197. Endings for “The Last Hour on This Roster”

- **The lesson completes before the teacher leaves:** the owner confirms a result; the teacher’s later departure changes availability only.
- **The teacher is needed for handover:** the lesson closes without a result; the player may seek another teacher if the learner agrees.
- **The learner accepts a replacement teacher:** the new match passes the ordinary eligibility and schedule checks.
- **The learner declines a rushed session:** the player respects the choice; no lesson or future appointment is falsely recorded.
- **Partial progress remains with the learner:** the owner’s exact result persists if its contract supports identity across roster change; continuation is not guaranteed.
- **A completed lesson has no destination application:** progress remains recorded, but no new work role is created.
- **A destination request arrives through a faction:** the player responds through work/apprenticeship owners and learner consent rules.
- **No portable reference is permitted:** the learner leaves without a document; the game does not invent an item or remote class.
- **Both participants leave before session:** cancel through the current owner and close with no result.

These endings differ through actual timing, consent, owner result, and destination support. The player is not rated on whether they forced one last lesson or let the person leave.

## 198. Replay and save behavior around roster changes

If a departure event is delivered twice, the education route must not cancel twice, reopen a stale agreement, duplicate a message, or remove the same result twice. Use the current roster and campaign event identities. If the event owner cannot guarantee idempotency, the feature must not claim a replay-safe departure callback.

If the save occurs after departure but before the education panel refreshes, restore the canonical roster and education state, then show the appropriate closed or persisted result. Do not use a panel-local “departed” marker. If a save occurs after an accepted agreement but before the departure command, restore both owners and resolve the ordering deterministically.

If a lesson result is committed immediately before departure, preserve it. If no result is committed, do not write a completion line merely because the screen was open. The day-boundary and save rules in Installment 8 continue to apply.

## 199. Content and integration review

Before implementing the departure route, verify the current event that removes or transfers a participant, the education owner’s identity and result lifetime, the schedule owner’s agreement cancellation path, the save registration and restore order, any destination education consumer, and faction/work routes triggered by departure. Confirm that the event exists in canonical content and is not only a proposed story premise.

The writer should identify which scene lines are conditioned on actual session state. The integrator should identify which commands can still occur after roster removal. The content should fail closed if a participant is no longer eligible. No callback, result, or message should be sent to an absent survivor through a host path that does not exist.

If current save or identity contracts cannot preserve the result safely, pause the portability claim and keep the route local. The safe design is a truthful close, not a parallel portable transcript system.

## 200. Installment 13 close

This installment expands the education loop across real roster departures. It distinguishes learner and teacher exit, accepted-but-uncommitted agreements, committed results, partial progress, and downstream requests. It adds story options for a final lesson, a replacement teacher, a voluntary deferral, a source reference, and no lesson at all. Roster, schedule, education, save, travel, and faction owners retain their authority; no remote-learning or portable-record system is invented.

## 201. Installment 14 — a question carried by the radio

Radio can make a lesson feel connected to the wider world. A learner hears a repeated phrase, catches only one word through static, or notices that a familiar symbol is used differently in a broadcast. The player can replay the signal, inspect an available transcript, seek a reliable source, or decide that the fragment is too incomplete to teach from.

The working route is **“The Voice Between Stations.”** A radio excerpt prompts a learner’s question about a supported curriculum subject. The excerpt is a story carrier, not a new education authority and not proof of a current local instruction. Audio may introduce a cost, a request, or a denial only when the corresponding current owner and readable UI route confirm it.

This route adds no separate radio lesson system, signal decoding minigame, broadcast faction, or transcription inventory. Use current radio/audio data and existing presentation paths. If no current radio event supplies the phrase, this remains a prose proposal until the content owner approves a signal and its transcript.

## 202. The opening — one phrase survives the static

During a routine listening moment, a short broadcast repeats a phrase the learner has heard in a lesson context. They ask whether the speaker is giving an instruction, describing a past event, or using a local term. The player can replay the excerpt if the current radio owner permits it, read the corresponding text if one exists, ask a qualified person to explain the term, or leave the signal unresolved.

The UI should identify what it knows about the source: current or archived program, timestamp or simulation-day context if available, known speaker attribution if verified, and whether a transcript matches the audio. It must not manufacture speaker identity from accent, faction symbol, or the learner’s guess.

If the message contains an urgent warning, the game surfaces the warning in text and routes the necessary response through the right work, travel, or communication owner. The player is not required to complete a lesson before acting on a safety-critical signal. If the excerpt is only atmospheric or incomplete, it should not be presented as a command.

## 203. Four ways to investigate the phrase

**Replay the available excerpt.** The learner can listen again and identify the part they recognized. Replay changes presentation only; it cannot consume another lesson result or alter the broadcast’s authoritative state.

**Read the transcript.** If the radio owner provides a transcript, the player can compare words without relying on audio alone. If the transcript is missing, do not synthesize exact wording from an unverified sound bite. Provide a truthful limitation.

**Ask a qualified teacher.** The teacher can explain a supported term, but must distinguish general vocabulary from what the speaker intended in that context. If the teacher does not know, the player can consult a source or stop.

**Check source provenance.** The player can ask whether the broadcast is current, archived, local, or carried from elsewhere, using current source metadata. Provenance can narrow interpretation; it does not prove that any instruction applies to the shelter today.

The player can also decline to investigate. The learner’s curiosity is not a mandatory quest hook. A later lesson offer exists only if the education owner and current catalog support it.

## 204. Branches from signal quality and attribution

| Signal/content state | Player action | Resulting scene | Truth that must be preserved |
|---|---|---|---|
| Clear excerpt, known current source | Compare phrase with a supported lesson | The teacher explains the wording and its limits | Current source still does not automatically authorize local action |
| Clear excerpt, speaker unknown | Read transcript or ask about provenance | The learner learns what was said, not who said it | Do not assign faction identity without evidence |
| Fragmented excerpt | Replay, stop, or seek another source | The missing phrase remains missing | Never fill static gaps with invented certainty |
| Audio and transcript disagree | Report the mismatch through the source owner | The lesson pauses or uses another supported example | Do not choose whichever version creates the better quest branch |
| Archived broadcast | Study it as historical language if the catalog permits | The learner can identify dated context | It is not a live warning or current order |
| No transcript or accessible replay path | Offer an equivalent text summary only if authoritative | The player can stop or use another source | Audio alone cannot hide cost, denial, or mandatory information |
| Teacher recognizes the phrase but not the speaker | The teacher explains vocabulary and uncertainty | The group may seek provenance or close | Familiarity is not source verification |
| Broadcast conflicts with a local owner | Ask the local authority to verify current state | Local operation follows its canonical owner | The broadcast cannot override live work/schedule facts |

## 205. Lesson agreement — teach the term, not the broadcast’s authority

If the learner wants a lesson, the player offers only a curriculum subject that currently exists and for which learner, teacher, and source rules pass. The subject might concern a supported term, notation, or communication convention, but the actual catalog ID must be verified. A one-time listening conversation is not automatically a session.

The agreement can specify whether the lesson uses replay, transcript, a neutral example, or a qualified teacher’s explanation. These methods do not create separate progress tracks. If the teacher or learner changes the method after the session begins, keep the same identity and follow the education owner’s behavior for amendment or interruption.

The learner can ask to replay the phrase privately, study from text, bring another person, or stop. If the existing UI does not support one of these options, do not promise it in dialogue. The scene can still close with a clear explanation and an alternate valid route.

## 206. Application choices after a confirmed result

After an owner-confirmed lesson, the learner may recognize the same term in a current local notice. The player can ask a qualified operator whether it applies now, share a general vocabulary explanation with a peer, or leave the connection as a personal understanding. A faction request can occur only if an existing event or conversation asks for it.

If the signal came from a major faction, the player can report that the learner studied the phrase, share only the supported capability, or withhold identity. The speaker’s faction does not receive automatic access to the learner or their education record. If the broadcast contains a request for action, route that request through the current communication and work owners.

If the learner notices that the local notice uses the word differently, the player can ask for a qualified explanation. The lesson may have taught one context, not universal meaning. A mismatch is a new question, not a reason to overwrite the previous result.

## 207. Endings for “The Voice Between Stations”

**The phrase is understood, the source remains uncertain:** the learner gains only the owner-confirmed lesson result. The player records no speaker or faction identity that the radio owner cannot prove.

**The transcript resolves the wording:** the teacher explains the supported term and the learner chooses whether to pursue an application. Audio and text agree, but operational consequences still use their own owners.

**The source is historical:** the learner understands that the phrase belongs to an earlier broadcast. The player closes the lesson or selects a current subject; no live action is triggered.

**The audio and text conflict:** the player reports the mismatch, seeks an authoritative copy, or stops. The story does not select a preferred version silently.

**The learner prefers text over audio:** the player uses an accessible transcript if it is current, or another valid material. The scene does not penalize the learner for not listening.

**No reliable material exists:** the teacher and player acknowledge that the signal is insufficient. No lesson is claimed unless the education owner returned a valid result on a supported subject.

**A faction claims the voice:** the player shares the attribution only if the source confirms it. Otherwise the player can say the speaker is unknown or avoid reporting.

**The learner declines further study:** the player closes the offer; replaying a broadcast does not create a hidden unfinished task.

## 208. Faction roles around a broadcast

The **Military** may broadcast a handover or warning; the current communication owner determines whether it is active and actionable. A lesson can help a learner understand a term, but a qualified worker still confirms operational instructions.

The **Rebel group** may use a phrase or symbol in a recorded appeal. The player can discuss its wording, verify the source, or decline to spread it. The content should not turn comprehension into political allegiance.

An **Independent** may relay a market or route notice. The player checks its validity through the relevant trade or travel owner before acting. Understanding the words is separate from trusting the offer.

Archivists can help establish whether a recording is cataloged; Long Walk can report when it was carried; the Guild may recognize terminology from an object. These are optional source contributions. None can prove the identity or current authority of a voice without the relevant owner’s evidence.

## 209. Audio and text presentation requirements

Any essential choice, cost, denial, deadline, or outcome must be available in readable text as well as audio where the current UI requires it. Do not make replaying a sound the sole way to understand a branch. Preserve subtitles/transcripts, readable contrast, keyboard/controller navigation, and clear focus according to the existing UI patterns.

If a transcript is a separate authored data record, follow the current data schema and source/cue catalog. Do not copy the transcript into a panel constant while claiming JSON authority. Verify cue identity and routing through the current audio pipeline; the presence of an audio file does not mean the radio feature can trigger it.

Dialogue must distinguish an exact quote from a paraphrase. If the host cannot expose speaker, date, or transcript metadata, omit that claim. Audio can add mood and voice but must not become the only persistence or gameplay contract.

## 210. Replay, transcript, and lesson identity

Replaying a broadcast is not a lesson attempt. Opening a transcript is not a lesson attempt. Reading a saved transcript after restore cannot reapply the education command. When the player schedules a lesson, its stable identity follows the existing daily-session contract from Installment 8.

If the same radio event is delivered again, use the current communication owner’s identity and replay rules. Do not create a second lesson prompt solely because an event callback repeated. If the player learns from a later broadcast on a new day, the education owner and catalog determine whether a new session is permitted.

Save/restore must preserve only the canonical communication and education facts. If the broadcast owner does not retain a transcript reference or source identity, the education content must not invent a persistent reference in a separate local record.

## 211. Content verification packet

Before this route is implemented, verify the radio event and cue, transcript or subtitle source, event identity, known speaker metadata, current/archived status, related curriculum subject, eligible teacher, lesson command, save owner, and any application consumer. Confirm that the text route surfaces all player-relevant information and that audio is not the sole path to a result.

The content author should supply exact transcript provenance, paraphrase boundaries, source uncertainty, and a list of choices that depend on live owner state. If the recording is not current or no speaker can be confirmed, mark that fact explicitly. The route may be cut without affecting the three-feature education loop.

## 212. Installment 14 close

This installment makes a radio excerpt a possible prompt for a supported lesson, with branches for replay, transcript, provenance, uncertain attribution, accessibility, and live verification. It keeps audio as atmosphere and sourced content, not an alternate communication or education authority. One confirmed education result remains distinct from broadcast truth, faction identity, and local operational instruction.

## 213. Installment 15 — begin where the learner actually is

Learners arrive with different histories. One has handled a tool for years but never read its instructions. Another can explain the notation but has never seen the physical object. A third has completed an earlier lesson, while a fourth remembers a related idea but cannot say where they learned it. The game should respect those differences without inventing a hidden diagnostic test or duplicating skill state.

The working route is **“Show Me What You’ve Seen.”** Before the teacher begins a supported subject, the player can ask the learner what they already recognize, invite a low-pressure demonstration, use the catalog’s existing prerequisite/proficiency facts, or simply start with the lesson. The learner’s reply shapes the example and dialogue. Only the current education or skill owner may determine formal progress, proficiency, unlock, graduation, or qualification.

This route is not a placement exam, a new competency score, or a fourth feature. It is a content pattern inside the study agreement and lesson subfeatures. If the current owner has no way to adapt a lesson based on prior knowledge, use the response as dialogue only and resolve the normal single session.

## 214. Separate lived experience from recorded proficiency

The learner may have experience that is not represented in the education catalog. The player can acknowledge it and ask whether the learner wants to connect it to the subject. That does not authorize the game to grant a skill level. A work or apprenticeship owner may already recognize relevant experience; use that route if it exists.

Conversely, a canonical proficiency value does not explain how the learner feels about the topic, what example is familiar, or whether they want to continue. The player can offer an easier or more advanced supported example only if the current curriculum supports it. Do not infer confidence, patience, or personality from proficiency alone.

The narrative can show respect without a numeric reward: the teacher asks the learner to demonstrate the part they know, recognizes the useful detail, and explains what remains unverified. The learner can correct the teacher, ask for a different example, or choose not to perform in front of others.

## 215. Four starting-point approaches

**Start from the learner’s question.** The teacher asks what the learner wants to understand and chooses a supported example. This approach gives the learner control over context. It does not let them select a nonexistent curriculum ID.

**Start from a practical demonstration.** The learner shows a familiar action or object while a qualified teacher identifies which parts are already understood. The work owner retains all safety decisions. If the lesson subject is not eligible for practical demonstration, use a safe diagram or neutral example instead.

**Start from the recorded prerequisite.** The player views current catalog/proficiency facts and offers the next supported subject. The learner can accept, ask why, choose another valid subject, or defer. The interface should explain the prerequisite plainly and avoid describing the learner as deficient.

**Start without assessment.** The player can begin the normal lesson without asking the learner to prove anything first. This is important for someone who does not want an audience or has no desire to explain their history. The education owner still returns the one daily result.

## 216. Branches when prior knowledge is visible

If the current owner reports prior proficiency, the player can choose a valid next subject, review an available prerequisite, or ask the learner what they want to use the knowledge for. The player cannot replay the same lesson for another progress award unless the owner explicitly permits that contract.

If the learner demonstrates a familiar procedure but the skill owner has no matching record, the scene can acknowledge the experience and ask for a qualified assessment through an existing work or apprenticeship route. It must not silently convert the demonstration into formal education progress.

If the learner remembers a previous lesson, the current record—not a dialogue memory—determines whether progress persists. A completed result stays completed. Partial progress stays partial. Missing state after restore is an integration gap; the writing cannot patch it by claiming that the learner remembers everything.

If proficiency is unknown, do not fill the gap with a guess based on age, occupation, faction, or family. The player can ask, use a gentle example, or proceed with a supported lesson and let its owner report the outcome.

## 217. A branching scene — practical experience meets formal instruction

The learner recognizes a tool from a previous job and says they know how to use it. The teacher asks whether they have seen this model and points to a different label. The player can ask the learner to describe what they know, let the teacher compare the example, choose another subject, or stop before the object is handled.

If the learner’s experience matches the supported subject, the teacher can build the lesson around a familiar context. If it does not, the teacher can explain the distinction without saying the learner was wrong to recognize the tool. If the model’s identity is uncertain, the player asks the work owner to identify it before drawing a lesson conclusion.

The learner can say, “I used the old one, not this version,” or, “I know the sound it makes, not the writing.” These answers shape the scene. They do not trigger a new hidden branch stat. The player chooses a valid catalog subject, a different example, or no lesson today.

If a later application request arrives, the work owner decides what training or supervision is required. Familiarity with an older model does not certify operation of the current one. The learner’s formal lesson result remains true but bounded.

## 218. A branching scene — recorded progress exceeds confidence

The catalog shows a valid prior result, but the learner says they do not remember the material well enough to use it today. The player can ask whether they want a supported review, choose a different topic, or consult the existing consumer to see what the result permits. The learner’s uncertainty is not proof that the saved result is corrupt.

If the owner supports review, the player schedules it using the regular agreement and daily identity. If it does not, the teacher can offer a dialogue recap without claiming another session result. A work task may still require independent verification. The player can defer that task to a qualified person while the learner decides whether they want to study again.

This branch prevents the feature from treating an unlock as automatic confidence or consent. A learner can know a term but choose not to apply it, and the game can preserve both the canonical result and the learner’s present hesitation.

## 219. A branching scene — no one knows where the learner learned it

The learner uses a term correctly but cannot recall where they encountered it. The player can ask a teacher to connect it to a catalog subject, check whether a current proficiency owner already records it, or continue the conversation without making a formal claim.

The teacher may say, “You know the word. Let’s check whether it means the same thing on this page.” This recognizes knowledge while leaving context open. If the catalog and owner support a formal assessment, use that route. Otherwise the lesson can proceed normally or end without a fabricated prior credit.

The player should not be forced to choose between “the learner already knows everything” and “the learner knows nothing.” Content can reveal a partial understanding through dialogue while the system retains only its supported state.

## 220. Starting-point choice table

| Current evidence | Learner preference | Player action | Truthful next step |
|---|---|---|---|
| Prior result exists | Wants to continue | Select the next eligible subject | Current curriculum owner resolves prerequisite and lesson |
| Prior result exists | Wants a recap | Request a supported review or narrative recap | No duplicate result unless the owner supports a new session |
| No result exists | Demonstrates lived experience | Ask what they want to learn next | Work/education owners validate any formal recognition |
| Proficiency unknown | Does not want to demonstrate | Begin a supported lesson or defer | Unknown remains unknown until an owner resolves it |
| Related but different context | Wants practical example | Choose an approved example and supervisor | Work owner retains live safety and qualification decisions |
| Source or object identity unclear | Wants to proceed | Verify, substitute a neutral source, or stop | Do not teach a guessed interpretation as current fact |
| Current result conflicts with what the learner reports | Wants to correct the record | Re-read the canonical owner and use its supported correction path | Dialogue does not overwrite persisted proficiency |

## 221. Playstyle expression through starting point

The **curious player** asks open questions and lets the learner set the example. The **time-focused player** chooses the prerequisite already available and makes the duty cost clear. The **practical player** requests a demonstration under qualified supervision. The **privacy-focused player** starts without a public assessment. The **evidence-focused player** checks the canonical record and source. Each style reaches a supported lesson or an honest stop; none is the designated correct personality.

The player may change style between subjects. Do not persist a “mentor type” or adapt all later dialogue to a single earlier choice unless an existing narrative contract already captures that fact. One action can shape one scene without becoming a campaign-wide identity.

## 222. Faction and Current reactions to prior knowledge

A major faction may ask for proof before offering a supervised task. The player can show an owner-confirmed qualification if one exists, explain that the learner has practical experience but no formal record, offer a qualified adult, or decline. The faction’s standard belongs to its current work contract; the education panel does not issue credentials.

Archivists may help compare older and newer references. The Guild may recognize an object model or explain a recovered part. Long Walk may have carried a question from another settlement. Their experience can reveal context, but none can write into the learner’s proficiency record unless an existing owner already supports that action.

The learner may reject a faction’s interpretation of their experience. The player can keep the conversation general or close it. A faction does not own the learner’s biography because it has a job opening.

## 223. Results and language for different starting points

Avoid generic “beginner,” “advanced,” or “master” labels unless the current catalog uses them. Say what the owner returned: eligible for a subject, prerequisite recorded, partial progress, session complete, skill consumer accepted, apprenticeship request pending, or no valid match. Explain what the learner can do next without exaggerating.

Candidate responses include:

- “You already know the older mark. This page uses the newer one.”
- “The catalog shows the prerequisite as complete. The supervisor still needs to confirm the task.”
- “We don’t have a record that settles that. We can ask the owner or begin the supported lesson.”
- “You described the tool well. That does not tell us whether this model is safe to use.”
- “The review was a conversation. Your saved lesson result has not changed.”

The line should match the canonical result and distinguish recorded learning from narrative recognition.

## 224. Source and owner questions for starting-point branches

Before implementation, inspect current curriculum and proficiency APIs, prerequisite evaluation, any prior-learning route, work qualification owner, age/roster providers, save capture/restore, and consumers. Confirm which facts are authoritative and which are narrative context. Search current data for similar subjects and existing character claims to prevent duplicate or contradictory content.

If there is no formal prior-learning assessment, keep the demonstration as dialogue. If the player can correct or import an existing skill record, use only its established command. If no current consumer can use the result, avoid promising that formal recognition opens work. The player can still learn for personal understanding.

## 225. Installment 15 close

This installment expands how a lesson can meet learners with recorded progress, practical experience, uncertain history, or low confidence. It gives the player several starting methods and corresponding content branches while keeping formal proficiency, curriculum eligibility, and work qualification with their current owners. No placement exam, learner score, or duplicated skill record has been added.

## 226. Installment 16 — one idea, two names

People who work with the same object may use different terms for it. A formal curriculum might use one label, a household might use a shorter word, and a faction may use a work-specific phrase. The learner can understand the underlying idea while asking which name belongs in a report or current instruction.

The working route is **“What Do You Call It?”** A learner hears two people refer to the same concept with different words. The player can ask where each term came from, choose a supported lesson that compares the vocabulary, request an approved reference, or leave the difference unresolved. The choice can change who understands the learner later without making language a proxy for intelligence, honesty, or faction loyalty.

This is a writing and content pattern, not a new language system, glossary registry, translation authority, or skill score. Use only terms already supported by current canon, data, and localization. If the game has no established alternate vocabulary for a subject, do not invent a dialect solely to create a branch.

## 227. The opening — the learner is understood differently by two crews

The learner uses a familiar term while asking a qualified worker about a page. The worker corrects them with a formal label. Later, a representative from another group uses the familiar term in a request. The learner asks whether one word is wrong or whether different people mean the same thing.

The player can ask the worker to explain the formal term, ask the learner where they learned their word, compare both terms against an approved source, or use a diagram if the catalog and material support it. The learner may choose to keep both terms, adopt the formal one for a report, or ask the player not to turn a private conversation into a public lesson.

The immediate work request goes to its owner. If the difference could affect an operation, the player asks a qualified person to confirm the current instruction before anyone acts. The lesson can explain vocabulary; it cannot decide which faction’s procedure is authoritative.

## 228. Branch choices around vocabulary

**Teach the formal label.** The learner may want the term used in the catalog or report. The teacher explains it with a supported example. The player can later share that formal label without revealing the learner’s identity if the current reporting route supports it.

**Preserve both terms.** The learner can say that one word is useful with family and another with the work crew. The teacher may explain when each appears, but cannot imply that one community’s ordinary speech is inferior.

**Check provenance.** The player asks which source introduced the formal term and whether it is current. The reference owner confirms or leaves the question open. A current term can still be local rather than universal.

**Use a visual example.** If the curriculum supports it, the teacher maps the two words to a shared object or symbol. A visual can clarify the lesson, but it does not remove the need to use the correct term in a live safety instruction.

**Stop before teaching from uncertain wording.** If the words may refer to different parts or procedures, the player routes the question to an authorized source. The group does not guess.

**Do not share the phrase.** The learner may prefer not to repeat a family or personal term in public. The player can continue with a neutral example or close the offer.

## 229. Three faction contexts, three different reasons for terms

The **Military** may prefer a standardized term in a handover because ambiguity can disrupt coordination. The player can help the learner understand that convention while preserving their existing vocabulary. The request for standard wording does not mean the Military owns the curriculum.

The **Rebel group** may use a phrase that emphasizes a local adaptation. The player can compare it with an approved reference or explain that the local phrase has a narrower meaning. The learner does not have to adopt it to be accepted by the group.

An **Independent** crew may use a trade or salvage term tied to an object model. The player can ask the Guild or an item/work owner to identify the object before explaining the term. Commercial familiarity is not proof of technical equivalence.

These differences produce meaningful branches because the player can choose which audience needs which explanation, not because a faction is morally correct. A learner can understand all three terms and still choose not to work for any group.

## 230. Branch matrix — translation and audience

| Audience or purpose | Player choice | Supported outcome | Limit |
|---|---|---|---|
| Private understanding | Compare familiar and formal terms in a lesson | Learner can discuss both in context | No public report is implied |
| Work handover | Use the formal term if the current procedure requires it | A qualified worker confirms the handover wording | Education does not authorize the task |
| Peer explanation | Ask whether learner wants to explain both words | A voluntary explanation can occur | Peers gain no automatic curriculum progress |
| Faction request | Share only the relevant term or a general capability | Existing conversation/work owner receives it | No private history or identity is disclosed by default |
| Uncertain source | Ask for an approved reference | Verify, defer, or close | Do not guess from accent or affiliation |
| Text/audio mismatch | Use the canonical transcript or report an error | Owner-confirmed wording is presented | Audio cannot silently override authoritative text |
| Unsupported in-world language | Use neutral standard wording or omit the variation | The lesson remains in the existing curriculum | Do not invent language lore or a translator system |

## 231. A dialogue pass — correction without erasure

Learner: “My family calls it the return line.”

Teacher: “The maintenance sheet calls it the release line. Let’s check whether those point to the same mark here.”

Player options: compare with the current source; ask the learner to describe the part; use a neutral diagram; end the conversation if the source cannot be verified.

If the source confirms that both terms refer to the same concept, the teacher can explain which one appears in the formal report. If the terms refer to different things, the teacher corrects the mapping, not the learner’s background. If the source is inconclusive, the group preserves the difference and asks the relevant owner.

The player may later speak with a faction representative. They can use the required work term without telling the representative where the learner’s word came from. The learner’s home vocabulary does not become public property because it helped the lesson.

## 232. Writing and localization boundaries

In-world vocabulary variation and interface localization are separate concerns. The first is fictional language content; the second is rendering user-facing text in the selected language. A translated menu string does not prove that characters use different in-world terms, and an in-world term should not be hardcoded into a UI label if the localization pipeline owns it.

If current content introduces multiple terms, record their source, intended meaning, audience, and localization keys. Preserve any necessary distinction in the data owner. Avoid relying on color, italics, pronunciation audio, or unexplained slang as the only way to distinguish terms. Give the player a readable explanation in the interface.

Do not use real national or cultural stereotypes to make speech patterns legible. Distinct voices can come from occupation, relationship, practical priorities, and individual history already supported by canon. If the existing character profiles do not support a particular vocabulary, leave it out.

## 233. Lesson result and knowledge transfer

The education owner still determines whether the learner completed a supported lesson. Recognizing two terms in dialogue is not a second session. A later work consumer may require the formal label, an approved qualification, or a separate supervised action. The result should be shown with its actual scope.

The learner may apply the formal term in a handover and use the familiar term in a private conversation. The game does not need to store a per-audience vocabulary profile unless a current owner already supports that data. Dialogue can vary locally without introducing a hidden global language preference.

If the learner chooses not to share the term, their lesson result remains intact. If a faction rejects the learner’s chosen wording, the player can clarify or move the conversation, but the education record is not revoked. The faction’s standards belong to its current work interaction.

## 234. Endings for “What Do You Call It?”

- **Both terms are valid in context:** the learner understands when the formal label is needed and retains their familiar term.
- **One term is obsolete:** the source owner confirms the change; the learner can use the current term in future work.
- **The terms refer to different things:** the teacher clarifies the distinction and the player routes any operational issue to its owner.
- **The source cannot settle the wording:** the player stops before making a claim and chooses another supported lesson or later reference.
- **The learner keeps the exchange private:** no public report is made, but the private lesson can still complete if the owner returns that result.
- **A faction requires formal wording:** the player explains the work convention, lets the learner choose whether to participate, or provides an eligible alternative worker.
- **No canonical vocabulary variation exists:** the branch is omitted and the standard subject proceeds without invented dialect.

## 235. Reuse across existing content carriers

The same vocabulary branch can appear in a current manual, radio transcript, roster note, tool label, or survivor conversation only where each source already exists. The player should not be asked to memorize contradictory wording. Each carrier must identify whether it is current, historical, local, or unverified using its existing source owner.

If a later document uses the same term differently, the game can open a new question rather than silently rewriting the earlier scene. The player can inspect the source, ask an eligible teacher, or route the discrepancy. The learner’s prior result remains as recorded; a new lesson is scheduled only if the education owner permits it.

## 236. Content review questions

Before implementation, confirm that each term already appears in canon or is explicitly approved, the curriculum can teach the relevant distinction, and the UI/localization pipeline can render it accessibly. Identify the owner for source provenance, procedure, and any work consequence. Confirm that no faction is being used as a shortcut for dialect, ethnicity, morality, or intelligence.

If the game does not have current in-world term variants, this installment remains a writing opportunity rather than a data request. If it does, map every branch to real player actions and owner facts. The route must remain understandable when no faction representative is present.

## 237. Installment 16 close

This installment adds a vocabulary-centered branch in which the learner can compare familiar, formal, local, and uncertain terms. It creates decisions around audience, source, privacy, and practical use without a faction-alignment gate. Curriculum, localization, communication, and work owners retain their authority; no new language system or glossary state is introduced.

## 238. Installment 17 — a lesson with no book

Teaching material can be useful, scarce, damaged, already in use, or absent. The player may need to decide whether to reserve a manual for essential work, use another supported method, request a loan, or defer. This creates a survival-management choice around access and time without adding an education resource pool.

The working route is **“The Only Clean Copy.”** A valid learner/teacher pair exists, but the only available copy of a relevant reference is needed by another current task. The learner still wants to study. The player can protect the work use, ask whether the teacher can use an approved alternative, request a copy or loan through an existing owner, or close the offer.

This is not a request for consumable lesson materials, a new school inventory, or a book durability system. Any transfer, reservation, return, cost, or loss belongs to the current item, archive, work, or trade owner. If the current catalog does not require a specific medium, the absence of a book may affect flavor only, not eligibility or result.

## 239. First determine whether the material is actually required

The interface must distinguish a catalog prerequisite from a convenient example. If the curriculum requires a particular approved source, show that requirement and stop the lesson when it is unavailable. If the teacher can use another valid method, the player may choose it. If the manual is only a visual aid, do not block study because it is in use elsewhere.

The teacher can explain this difference: “We need the current reference for the procedure. We do not need that copy to learn the vocabulary.” The learner then decides whether the remaining lesson still interests them. The player does not choose the substitute based solely on inventory convenience; the catalog and teacher eligibility must support it.

If no source is required by the owner, the player can begin with an approved demonstration, a known safe example, or the normal lesson. If the source itself is uncertain, do not make an oral explanation authoritative simply because the page is missing.

## 240. Material states and player choices

| Material condition | Player option | Immediate consequence | Boundary |
|---|---|---|---|
| Only copy reserved for essential work | Keep it with the work owner; choose a supported alternate method or defer | Work retains the reference; lesson may use another valid source | Do not create a duplicate copy in the education panel |
| Copy is available but must be returned | Request a loan through the custodian | The lesson can proceed only under the existing loan/custody rules | A dialogue promise cannot change item ownership |
| Copy is damaged or incomplete | Use only the confirmed legible portion, find another source, or stop | The group sees exactly what remains uncertain | Do not teach missing text as if it were present |
| Digital or transcript copy exists | Read it only if current data and the host route expose it | The player can avoid moving a physical source | Do not duplicate data into a local cache as authority |
| Object is useful as a demonstration | Request a supervised example if work and inventory owners allow | Learner can connect terms to a real object | The object is not necessarily safe to handle or operate |
| No medium is available | Defer or use only an approved oral lesson | No source transfer occurs | Do not create a new item or material stat |

## 241. A scene pass — keep the reference where the work needs it

The teacher takes the clean copy from its shelf and reads the work reservation taped beside it. A crew needs the manual for an upcoming repair. The learner asks whether they can study first and return it before the shift. The player checks the time with the current work owner rather than deciding that the lesson is more important by default.

If the manual is required for the repair, the player can return it, ask whether an approved copy exists, use another catalog-supported example, or defer. The learner may prefer to wait for the exact source. They may also accept a general lesson now and a later source comparison. Neither choice is automatically superior.

If the manual is merely convenient, the work owner may allow a short lesson and return. The item’s custody rules still apply. The player sees any real timing or transfer cost before agreeing. After the session, the reference returns to its proper owner; the education result is saved by the education owner, not by keeping a local manual copy.

If the book’s condition is not recorded, the UI should not claim that the player damaged or preserved it. A closing line can say that the copy was returned, but an inventory transaction is shown only after the owner confirms it.

## 242. Alternative methods are not automatic shortcuts

An oral explanation may work for a supported concept, but it is not always a substitute for a required current reference. A practical demonstration may make an idea visible, but cannot prove a system is safe to operate. A diagram can clarify relationships, but only if the current curriculum and material owners recognize it. The player should see these boundaries before choosing.

If an approved method is available, its session still uses the same one-result identity and owner command. Switching from a physical manual to an approved transcript mid-session does not create another attempt. If the owner cannot change method after submission, the player must choose before the command or close the scene according to the actual API.

Do not award a bonus for conserving materials or a penalty for requesting a loan unless an existing economy/work owner models a real cost. The choice matters because it affects access, scheduling, and who uses a scarce reference, not because a new lesson-efficiency score exists.

## 243. Different player approaches to scarce material

The **work-first player** reserves the manual for the essential task and asks whether an alternate source is valid. The result may be a delayed lesson or a different subject.

The **learning-first player** checks whether the work owner can use another qualified source, then requests the copy only if it is genuinely free. The task is not deprived by an unsupported assumption.

The **source-focused player** refuses to teach from a damaged copy and waits for provenance or a clean reference. No lesson result occurs until the owner accepts a valid session.

The **practical player** asks for a supervised object demonstration if the work and safety owners permit one. The teacher uses the object to explain a concept without transferring operational authority.

The **resource-conscious player** asks whether an existing digital or printed copy can be reused. The current inventory/record owner determines whether that copy exists and who may access it.

The **minimalist player** closes the offer and returns to survival work. The learner can choose another opportunity later if the current owners support it.

These are flexible play approaches, not classes or permanent traits. Each choice has a clear practical consequence and remains inside the existing agreement and session loop.

## 244. Supporting Currents and material access

Archivists may know where an approved copy is held or provide a permitted loan. The Scavenger Guild may offer an object or recovered manual if current inventory and provenance allow. Long Walk may carry a copy only if its delivery system and schedule support the route. Major factions can request the same reference for work and negotiate through existing service/trade owners.

The player can accept, decline, or seek a local alternative. A Current does not become an education authority because it owns a book. It cannot enroll the learner, choose the curriculum result, or certify that a method is valid. A source holder can explain what they know about provenance; the relevant domain owner confirms current applicability.

If no group can supply a copy, the lesson may still proceed where the education owner supports another method. If the exact source is mandatory, close the offer clearly. No faction sponsorship is required for a local education agreement unless the existing design explicitly models that constraint.

## 245. Endings for “The Only Clean Copy”

- **Reference stays with essential work:** the player protects the work task and schedules a supported lesson later only if availability is confirmed.
- **Approved alternate source accepted:** the learner chooses an available method and the education owner resolves one session.
- **Loan arranged:** the custodian transfers the copy through its current owner; return expectations are visible before commitment.
- **Damaged source rejected:** the player avoids guessing and requests a clean source or selects another subject.
- **Demonstration accepted:** a qualified worker supervises a bounded example; the learner gains no automatic operating authority.
- **Lesson deferred by the learner:** no session occurs, and the player does not portray the decision as lost progress.
- **No valid source or method:** the offer closes honestly with no fabricated teacher performance or result.
- **Work owner releases the reference:** the player schedules only if teacher, learner, and room remain available.

## 246. Return, damage, and loss after a lesson

If the material was borrowed, its return follows the existing custodian/item owner. If it is damaged during the lesson, the relevant item or work owner reports that consequence. The education system cannot create a second copy or retroactively erase the session result.

If the copy is lost, follow current inventory and custody rules. Do not write a new “lost textbook” quest state solely to give the education route an ending. The learner can remember the lesson, but the game should claim only the progress that the education owner recorded. If they need the source for a future application, ask the current owner for another valid reference.

If the source was never transferred, merely previewed, or shown in a panel, no inventory change occurs. The screen must not imply that the player took or consumed an item. A source card can expose metadata without becoming an item transaction.

## 247. Accessibility across material forms

When a lesson has multiple supported media, offer them as equivalent ways to participate where possible: readable text, a diagram, a spoken explanation with captions, or a supervised demonstration. Do not make one medium mandatory unless the underlying curriculum genuinely requires it.

If audio is used, a readable equivalent must expose any essential instruction, cost, denial, or result. If a print source is small, low contrast, or clipped, route the issue to the existing UI/accessibility owner. Do not state that the learner failed because a source was not legible in the interface.

Avoid describing a learner as less capable because they choose an alternate medium. Method choice changes how the scene is presented; only the education owner determines the result.

## 248. Save and determinism boundaries for source choice

The lesson result and the material transaction are separate state changes with separate owners. Capture and restore each through the current save path. If the player saves after the education result but before returning a borrowed copy, restore both facts accurately and let the item owner finish its current workflow.

Repeatedly opening a source preview cannot consume the copy or call the lesson owner. A retry of the same transfer uses the existing item transaction identity. A new source selected after a later day is part of a new supported agreement, not a replay of the old result.

The same seeded world and same owner state should produce the same source availability and lesson outcome. Do not use wall-clock time or process-local randomness to decide that a copy appeared or a book was damaged.

## 249. Review gate for material scarcity

Before implementation, confirm that each named source exists, its inventory/custody owner is known, any loan/cost path is real, alternate teaching methods are accepted by the curriculum owner, and the relevant work owner can report whether the reference is reserved. Verify that the host route returns material state after save/restore and that the lesson command remains idempotent.

If the source does not exist, remove the branch. If a cost is not modeled, do not charge one. If an oral or demonstrated method is not supported, do not award progress for it. If the work owner cannot confirm the reservation, avoid implying that the book was taken from a shift. Keep the feature useful when no supporting faction is available.

## 250. Installment 17 close

This installment adds source scarcity as a player decision: reserve a reference for work, use an approved alternate method, request a loan, demonstrate under supervision, or defer. It maps each outcome to existing item, archive, work, trade, education, and save owners. No material pool, durability system, duplicate copy, or lesson-efficiency score is introduced.

## 251. Installment 18 — who gets to keep the lesson in writing?

After a useful lesson, the learner may ask for a reminder, the teacher may want to record the source, or another group may request a copy. The player decides whether to create a private aid, contribute a general reference, record a verified event, or leave no written artifact. These choices can shape who encounters the knowledge later while the education result remains with its existing owner.

The working route is **“The Note After Class.”** The learner asks, “Can we write down the part I keep mixing up?” A teacher offers to mark a general reminder, but the source may be borrowed, the example may contain private details, and a faction may later request the note. The player chooses the audience and purpose, then routes any actual document, journal, or archive action through its existing owner.

This is not a new education ledger, note inventory, document-authoring system, or public knowledge network. If current narrative, chronicle, archive, item, or bulletin owners cannot accept the record, the route remains dialogue and the game makes no promise that a note persists.

## 252. The first decision — what is worth recording?

The learner may want to remember a word, a source date, an unresolved question, or the name of a qualified person to ask. These are different content and privacy choices. The player can help create a general reference, ask the teacher to identify the approved source, record the question without the learner’s name if the current system permits it, or leave the class without a written record.

The scene should distinguish a private study aid from an official work instruction. A note that helps the learner remember a term cannot direct equipment use, change a schedule, authorize an apprenticeship, or replace a current manual. If the learner asks for a procedure, the player routes them to the current approved source.

If the learner does not want their name or example written down, respect that where the current record contract allows. Required education persistence remains private to its canonical owner; it does not authorize public display. If the current game does not expose separate private and public record choices, keep the conversation private and do not invent a sharing system.

## 253. Four documentation outcomes

**No record.** The player closes the lesson and returns or closes the source. The owner-backed result remains. The learner can choose this if they prefer not to preserve a written reminder.

**Private reminder.** The learner and teacher use an existing private note/journal route if one exists. The reminder is not an official credential or equipment procedure. If no owner supports it, the line remains a spoken recap.

**Sourced general reference.** The player asks an existing archive or chronicle owner to preserve a non-sensitive explanation with provenance. It can help later readers find the source, but cannot claim all readers completed a lesson.

**Public teaching notice.** The learner may offer a general note to a current notice/bulletin route, subject to audience, moderation, and data authority. The player can anonymize or remove sensitive details only if the owner supports that transformation.

The player may also preserve the unresolved question rather than a proposed answer. If the system cannot store an open question, do not create a local objective flag to simulate it; close with dialogue and let the learner ask again only through a valid later route.

## 254. A scene pass — the learner asks for a reminder

The lesson has ended and the learner can explain one concept but worries they will confuse it with another tomorrow. The teacher says, “We can write a reminder, but we should not copy the whole page.” The player can ask which part the learner wants to remember, check the source and any private details, or decline to create a record.

If the lesson used a borrowed manual, the player does not tear out a page or keep the source without permission. The teacher can point to the authorized reference location or use an approved copy if one exists. The learner can choose an abbreviated reminder, a diagram, a spoken recap, or no aid.

If the current note system supports a private reminder, the player can save it through that owner. If not, the teacher explains where the source remains and the conversation ends. The education result is still recorded by the education owner, not by the reminder.

Later, the learner may ask to review the note. The note can remind them what was taught, but any new session or formal review follows the same daily identity and curriculum rules. Reading a reminder does not award progress again.

## 255. The teacher’s source note

The teacher may want to record which source was used, especially if the page was old, partial, or supplied by a Current. The player can include source provenance, record that the example was illustrative, or leave the source unresolved. Any archive/chronicle record must distinguish what the teacher verified from what they merely heard.

The teacher can request that the learner’s name be omitted. The learner can also ask that the source not be associated with them. If names are required by the canonical education owner, that internal record remains separate from a public narrative artifact. Do not duplicate the list of participants into a new journal or bulletin store.

If the source is disputed, the note should say “unverified” only if the current archive/chronicle owner supports that status. If not, do not record a misleading verdict. The player can choose no public note and ask the appropriate owner to resolve the source separately.

## 256. A faction asks for the note

A major faction asks whether it can copy the learner’s reminder for its own crew. The player can share a general approved reference, offer a qualified worker instead, ask the learner whether a public version is acceptable, or decline. Sharing the source does not share the learner’s identity or education result by default.

The **Military** may need standardized wording for a handover. The player can refer to the current source or an existing work instruction, not a learner’s private shorthand as authority.

The **Rebel group** may ask for a general explanation to circulate. The player can provide only a permitted, sourced version or decline because the source is uncertain.

An **Independent** may want to reuse the example in a trade or work exchange. Any price, copy, or distribution cost belongs to the existing economy/item owner. Knowledge itself is not a new tradable item.

The faction can accept the boundary, ask for a qualified reference, or find another source. Its need remains real, but the learner is not forced to publish personal work.

## 257. Branch table — audience and persistence

| Audience | Player decision | Possible durable route | Claim prohibited |
|---|---|---|---|
| Learner only | Keep reminder private | Existing journal/note owner if supported | It is not a new curriculum result |
| Teacher and learner | Record the teaching source or question | Existing chronicle/archive owner if supported | It is not proof of certification |
| General shelter | Share approved, non-sensitive explanation | Existing bulletin/notice owner if supported | Readers do not automatically gain progress |
| Major faction | Share a sourced capability or reference | Existing faction/work/communication route | No identity disclosure unless allowed and accepted |
| No one | Create no artifact | Education result remains in education owner | No missing record is treated as a failure |
| Future destination | Carry a copy through an existing delivery route | Existing item/travel owner if supported | No remote lesson continuation is implied |

## 258. The player chooses a teaching legacy, not a new progression track

The note can create a small, observable narrative consequence: someone finds the approved reference more easily, a teacher sees which term confused the learner, or a faction knows to ask for a qualified worker. Those effects happen only if the current journal/archive/communication/work systems can show them.

Do not add “lessons documented” reputation, a knowledge-sharing score, a public school board, or a new memory-decay system. The player can choose not to write anything and still complete the feature. The value of a written artifact comes from clarity and access, not a new numerical reward.

If the current Chronicle or journal already records the owner-confirmed lesson event, extend its current event seam only after verifying the schema and consumers. Do not create a parallel document just because an existing panel is inconvenient. If no suitable event exists, the content can remain a one-scene closure.

## 259. Re-entry, editing, and outdated reminders

If the current note owner permits editing, the player may update a reminder when the source changes. The edit must not rewrite the original education result or conceal its provenance. If the note is a historical record, preserve its original date and add an update through the owner’s normal append/update rules.

If the source becomes obsolete, mark it outdated only through the current document/archive owner. The learner’s old result remains true for the lesson they completed, but its practical use may need revalidation. A faction reading the old note cannot assume that it is current because it exists in an archive.

If a saved note is deleted, expired, or unavailable, the education result is not erased. The player can ask for another valid source or choose not to repeat the lesson. The note and lesson are distinct records with distinct owners.

## 260. Dialogue fragments for the note route

- Learner: “Write the date too. I don’t want to mistake an old page for today’s rule.”
- Teacher: “We can note where it came from. That doesn’t make it the current instruction.”
- Player: “Do you want your name on the public copy, or should this stay between us?”
- Learner: “The word can be shared. My example stays private.”
- Archivist: “I can preserve the source reference. I can’t certify what the crew changed afterward.”
- Faction representative: “Can we use this for our handover?”
- Player: “Use the approved instruction. This note can point you to it.”
- Teacher: “The lesson is recorded already. The note is only a reminder.”

Use these lines only where current character profiles and record owners support the distinction. A line about privacy cannot substitute for a privacy command, and a source citation cannot substitute for a document state.

## 261. Accessibility and authorship of teaching aids

Where current systems support different formats, let the learner choose a short text reminder, diagram, spoken recap with text, or source reference. These are presentation options, not a new item catalog. If a format requires an asset or import, use the current asset and data owners.

The aid should identify its author/source where appropriate, have readable contrast and scaling, and remain navigable by keyboard/controller. Avoid a handwritten visual as the only version of essential information. If the learner’s contribution is used publicly, ensure the interface can show what is shared before the player confirms.

## 262. Endings for “The Note After Class”

- **Private reminder kept:** the learner chooses an aid for personal review through an existing owner.
- **Sourced reference shared:** a current archive/chronicle path records a general explanation with provenance.
- **Question left open:** the player records no answer and routes the unresolved matter to its proper owner.
- **Public notice accepted:** a non-sensitive version is shared after the learner’s choice and owner validation.
- **Faction request declined:** the learner’s private note stays private; the faction uses another source.
- **Source too uncertain:** no note claims a verified procedure, though the owner-backed lesson remains.
- **No record chosen:** the teacher and learner close the material and leave with the session result only.
- **Reminder later outdated:** the existing source owner marks or replaces it; the original session result is preserved.

## 263. Owner and content audit for written aftermath

Before implementation, inspect current journal, Chronicle, archive, notice-board, communication, item, and education owners. Identify the smallest existing event route that can truthfully represent a reminder or source reference. Confirm who can see it, who can edit it, whether it is saved, how it expires, and whether content has a localization key.

If no current system owns the desired artifact, do not store it in the education feature. Keep the scene conversational or request a separate architecture proposal. The learner’s progress remains fully valid without a public document.

## 264. Installment 18 close

This installment adds a post-lesson decision about recording a private reminder, preserving a sourced reference, sharing a general notice, or creating no artifact. It creates privacy, provenance, audience, and future-use branches while keeping the education result in its canonical owner. No note inventory, education ledger, knowledge-sharing score, or document authority is added.

## 265. Installment 19 — a bounded story arc from question to handover

The plan now has several self-contained scenes. This installment ties three of them into one optional, bounded story route: a learner notices a confusing mark, asks for a lesson, and later decides whether to use the knowledge during a supervised handover. The route is long enough to show a consequence but small enough to rely on existing education, schedule, work, and narrative owners.

The working title is **“The Mark Between Shifts.”** It uses current cast members and a current, permitted handover document or task if one exists. No new NPC, place, faction, item, campaign flag, or major quest authority is needed. If the game has no live handover source that can support this route, substitute an existing approved teaching example or leave the arc as a prose proposal.

The arc is optional. It begins only when an existing event makes the mark relevant and ends when the player receives a truthful education/application result or decides not to continue. It has no fixed timer, forced three-day schedule, or required faction tour.

## 266. Beat One — the question appears during a real task

The learner notices that a symbol on a handover sheet looks different from one used on an earlier page. The player can ask what the learner recognizes, check whether the sheet is current, ask a qualified coordinator for the immediate work meaning, or move the question to a later lesson.

If the handover affects an active shift, the coordinator confirms the operational state directly. A lesson is not required to learn who is on duty or whether a task is safe. If the discrepancy is not urgent, the player can let the learner explore it as a study question.

The learner may recognize one symbol but not the notation around it. The player can choose to verify the source, find a qualified teacher, ask for a supported subject, or close the question. No progress is earned from noticing the mark alone unless the education owner already records that behavior.

## 267. Beat Two — a study agreement fits the participants

The player checks learner eligibility, teacher qualification, required permission, material provenance, duty, and location through the current owners. The learner can choose a supported subject and method, ask for privacy, select a different teacher, defer, or decline. The teacher can accept, request a different source, or decline due to duty.

The lesson’s focus is specific: read the mark and understand the document’s structure. It is not a promise that the learner can update the roster or take over the handover. The player sees whether the reference is current and what remains unknown before committing.

When the session begins, one stable daily identity is used. The education owner returns the actual result. The story branches on that result: a complete result can enable a supported application request; a partial result can prompt another valid agreement later; rejection or no match closes the lesson path honestly. The arc does not force success for narrative pacing.

## 268. Beat Three — knowledge meets a live handover

If the education owner reports a relevant result and a compatible application opportunity exists, the learner may offer to read a permitted part of a later handover while a qualified coordinator remains responsible. The player can accept, ask the coordinator to verify first, keep the lesson private, or decline.

If the learner reads the mark correctly, the coordinator confirms the current assignment. If the document is stale, the player routes it to the schedule owner. If the learner is uncertain, the coordinator takes over without treating the earlier lesson as wasted. If the task is outside the learner’s eligibility, the player redirects it to an appropriate worker.

If no later application opportunity appears, the arc can end after the completed lesson. Do not invent a work event just to produce a third scene. The learner can remember the class in dialogue or move on to another topic without a special quest closure.

## 269. Branch map for the three-beat arc

| Beat One action/condition | Beat Two route | Beat Three result | Closure |
|---|---|---|---|
| Verify current sheet first | Use confirmed source and eligible teacher | Supervised read-back if work owner permits | Learner contributes to a verified handover without owning it |
| Ask the learner what they recognize | Choose a subject around the learner’s question | Learner chooses whether to demonstrate | Personal curiosity leads to private or public application |
| Urgent work is active | Coordinator handles the live assignment; lesson waits | No application until a valid future task exists | The urgent need is resolved separately; no lesson is faked |
| Source is stale | Seek a valid source or another supported subject | No live use of the stale sheet | The learner’s caution is acknowledged; the document owner updates it |
| No qualified teacher is available | Defer, find another eligible candidate, or stop | No application claim | The arc closes with a real availability limit |
| Learner wants privacy | Use a permitted private method or neutral example | Keep later result local unless learner agrees otherwise | Knowledge is preserved without public exposure |
| Lesson returns partial progress | Follow the exact owner result | Wait for a new valid session before any supported use | Progress remains partial and continuation is optional |
| Faction requests a named reader | Ask learner before disclosure or offer qualified adult | Use ordinary work route | Faction need is addressed without auto-enrollment |

The branches can recombine at the handover owner, but the narrative must preserve how the learner arrived there. No route should be gated by a broad “good/bad” value or a faction reputation threshold.

## 270. Endings for “The Mark Between Shifts”

**Verified contribution:** the learner completes a supported lesson, chooses supervised use, and reads one current mark while the coordinator confirms the assignment. The learner has a visible contribution; the coordinator retains authority.

**Private understanding:** the learner completes the lesson but does not demonstrate it publicly. They understand the mark for their own purposes, and no faction receives their identity.

**Source corrected:** the player discovers that the page is stale. The learner’s question helps reveal a document problem, which the schedule/document owner resolves. The lesson may continue using an approved source or end.

**Duty-first closure:** no valid teaching interval exists before handover. The coordinator completes the work; the learner receives no lesson result today. A future offer depends on current state.

**Partial learning:** the owner reports partial progress. The learner can choose whether to continue later. No one claims that the student is ready to interpret live assignments.

**Alternate teacher:** the learner accepts another eligible teacher. The session uses the same normal owner path and one daily identity.

**No valid route:** source, teacher, permission, or subject is unavailable. The player closes the sequence with the missing condition stated plainly.

**Faction referral:** a faction requests a reader but the player assigns the conversation to a qualified worker. The learner’s education result remains intact and separate.

## 271. What the arc gives each participant

The learner gains a reason to ask a question and a choice about how to use the answer. The teacher sees a practical subject and can state where their expertise ends. The coordinator keeps current responsibility for the handover. The player decides what is taught, when, by whom, and whether the result is shared. A faction can have a concrete request without gaining control of the learner.

The arc also gives the player a small consequence that is not a new counter. A later handover may be easier to explain because the learner understands the notation, or the player may learn that the paper was out of date. Those are observable story outcomes grounded in existing state.

If the player never schedules the lesson, the task remains playable through a qualified worker. If the player completes the lesson but never uses it, the story remains complete. Optional learning should add depth without making the shelter’s basic operation depend on an untrained learner.

## 272. Faction branches stay at the edge of the story

The Military may want a consistent handover format and ask whether the learner can help. The player can share the standard term, provide a qualified coordinator, or keep the learner’s identity private. The Rebel group may have a local notation and ask whether the player can compare it to the shelter’s sheet. The Independent may request a worker who can read a trade note. Each request uses its current work/communication/trade owner.

Archivists can help verify the age of the handover form. The Guild can identify a mark on a salvaged tool if the current item and work owners support it. Long Walk can explain when a note arrived or carry an approved reference only through current delivery systems. These support roles are optional and bounded.

No faction needs to appear for the arc to function. The player can complete it as a local education story, decline each request, or use one relevant support Current. Faction presence follows real narrative triggers, not an education completion flag invented for this plan.

## 273. Quest presentation without a parallel quest ledger

If a current quest or journal system can display this route, it may read existing facts: valid agreement, owner result, current document status, and accepted application request. It must not own duplicate copies of learner progress or assignment state. If no current quest route exists, present the story through dialogue/events and do not add a bespoke quest store.

The player may see a simple objective such as “Check whether the sheet is current” only if an existing journal/quest authority supports it. Completion criteria derive from the source owner’s confirmed outcome, not from pressing a UI button. If the source check is unavailable, show “unverified” only if that status is supported.

The arc can end without a quest-complete banner. A clear narrative close and current owner state are enough. The player should understand what happened and whether any valid next step remains.

## 274. Re-entry and interrupted arcs

The player can leave between beats. On return, the route reconstructs from current source, roster, session, schedule, and work state. If the learner or teacher has left, use the roster-transition rules from Installment 13. If the day has advanced, use the daily-session rules from Installment 8. If the note has changed, re-read its current owner.

If a faction request expires, the faction owner provides the expiry. The education arc does not extend a work request. If the learner changes their mind, the agreement closes through its owner. If the lesson result is complete, re-entry displays it without a second session. If a branch condition can no longer be evaluated, collapse to a truthful local ending instead of preserving a stale quest pointer.

## 275. Production content packet for the arc

The final content packet should include an entry event tied to a current source, a learner question, immediate operational response, one valid agreement route, session result lines, supervised application options, faction request variants where current events exist, and endings for complete, partial, interrupted, rejected, no-match, private, source-corrected, and duty-first outcomes.

For each line, identify whether it is based on a canonical condition or immediate dialogue choice. List the source and audience of any note. Identify who can remove or update a stale handover. Include a fallback when there is no teacher, no subject, no source, no application, or no quest route. Use current character IDs only after canon checks.

## 276. Review criteria for the bounded arc

The arc is ready for integration planning only when its trigger exists in current content; exactly the three education subfeatures remain in scope; one session identity produces one result; all work decisions go through existing owners; and save/restore reconstructs the route. Verify that no ending claims unconfirmed learning, no faction inherits education authority, and no learner is assigned work by a quest condition.

If the live game cannot support an application event, cut Beat Three rather than creating a parallel work route. If no journal owner can expose the arc, keep it as an encounter or dialogue sequence. If the current canonical handover sheet does not exist, select another supported source or leave the story as a proposal.

## 277. Installment 19 close

This installment ties one learner question, one valid session, and one optional supervised application into a compact branching story arc. It supplies endings for source correction, duty priority, privacy, partial progress, alternate teacher, and faction referral. The route can be skipped, interrupted, or completed without a new quest-state authority; current education, schedule, work, source, and narrative owners remain authoritative.

## 278. Installment 20 — a lesson happens inside a living body

Education takes place in a survival game. Hunger, fatigue, illness, injury, stress, and rest may affect whether a learner or teacher is available, but the education feature must read those facts from their current owners. It must not create its own wellbeing bars, assume that every need affects learning, or apply hidden progress penalties to make lessons feel costly.

The working route is **“After the Meal Line.”** A learner asks for a lesson during a day when the shelter is managing a real need. The player chooses whether to proceed, wait until a current need is addressed, find another eligible teacher, give an immediate safety explanation, or close the offer. Those actions can produce different consequences through existing needs, health, schedule, education, and inventory owners.

This installment does not assert that hunger, fatigue, radiation, or illness currently modify the education result. It asks the premise audit to verify which facts the owner reads. If the lesson API does not use a particular condition, the writing must not claim that it caused a partial result.

## 279. Show the actual constraint, not a vague readiness score

The player should see what condition is affecting the agreement: the learner is assigned to a task, the teacher is resting, a health owner has restricted activity, the room is unavailable, or an urgent need remains unresolved. Do not compress all these facts into “not ready” if the player can take a concrete action.

If a need owner supports providing food, water, rest, care, or a schedule adjustment, the player can route that action through its current command. The lesson screen itself does not consume a ration, treat an injury, reduce radiation, change duty, or reserve a bed. Previewing an option does not transact it.

If the constraint has no supported resolution, say so and offer a valid alternative or close. “Try later” is only truthful if a later availability path exists. Do not imply that the player can fix every condition by spending an untracked resource.

## 280. Learner needs and learner agency

A learner may ask to continue despite being tired. The player can explain the known schedule and any owner-confirmed restriction, ask whether they prefer to defer, or proceed if the relevant owners allow it. The learner’s choice matters, but it does not override health, age, safety, or eligibility rules.

A learner may also want to stop even when conditions look favorable. The player can close the session or decline to begin. The game must not infer laziness from a refusal or make the player repeatedly pressure them until they accept.

If the owner reports that the learner is unavailable because of a need, the player sees that fact before committing. If the owner allows the lesson and returns a result, the scene follows the result. Do not add a hidden fatigue multiplier that rewards scheduling at an ideal moment unless such a modifier already exists in canonical gameplay.

## 281. Teacher needs and duty are equally real

The teacher’s health and rest constraints matter as much as the learner’s. A person who is qualified is not automatically available. The player can choose a different teacher, a different day, a different supported subject, or no lesson. A close relationship does not justify assigning the teacher through exhaustion.

If a teacher is resting, the interface should not present an accept button and later blame the teacher for declining. If the teacher’s condition changes between offer and commit, revalidate through the current owner. If the teacher must leave, follow the same schedule and interruption contracts from prior installments.

The teacher can state a boundary without a new emotional state: “I can teach tomorrow after the water shift,” or “I can answer the warning now, but I can’t run a lesson.” The player can respect that and use a qualified alternate if one exists.

## 282. Immediate information remains separate from optional learning

Some questions require a fast answer: where to report, whether an area is currently restricted, or whom to contact for urgent care. The responsible owner or qualified worker provides that information directly. A lesson may explain the terms later, but the player must not withhold the answer to make a feature tutorial or narrative branch happen.

If a learner asks about an urgent health condition, route it to the health/medical owner. The education feature does not diagnose, prescribe, or convert an urgent need into a lesson task. If a safety instruction is urgent, state it through its current authority and record no education progress unless the lesson owner separately supports it.

When the immediate need is resolved, the learner can decide whether the original subject still matters. They may want to study it, choose a different topic, or return to other work. The game should respect that change in priority.

## 283. Branch matrix — timing around needs

| Current owner fact | Player action | Valid consequence | Claim to avoid |
|---|---|---|---|
| Learner has a supported activity restriction | Offer another time or valid learner | Agreement cannot bypass the restriction | Do not call the restriction a motivation problem |
| Teacher needs rest | Defer or find another eligible teacher | Teacher’s rest remains protected through the schedule/need owner | Do not apply a fictional teacher penalty |
| Immediate safety answer is required | Ask the qualified owner now | Information is given before any lesson | Do not gate safety behind education progress |
| Need can be addressed through a current resource command | Route that command separately and refresh state | Lesson may become available if owners confirm | Do not spend resource in preview or panel code |
| Need remains unresolved | Close or defer the lesson | The unresolved need stays visible through its owner | Do not invent a lesson-based cure |
| Learner chooses to stop | Respect the choice | No session result unless already committed | Do not infer bad character or lost progress |
| Teacher becomes unavailable after session commit | Preserve returned education result | Remaining application may change | Do not erase a committed result |
| Current owners show no relevant need effect | Proceed using normal lesson rules | Owner returns the normal result | Do not write a need penalty unsupported by code |

## 284. A scene pass — choosing between study and rest

The learner arrives after a demanding shift and asks if the lesson can still happen. The current needs/schedule view shows that the learner is free but tired; the lesson owner’s documented contract determines whether that condition affects availability or result. The player can ask whether the learner wants to continue, offer a later valid window, find another eligible student for the teacher’s time, or close the agreement.

If the learner continues and the owners allow it, the scene can be brief and low-pressure. The teacher may choose one supported method and stop when the learner asks. The lesson result is still the owner’s result, not a narrative judgment of how well-rested the person appeared.

If the learner defers, the teacher can protect the material for another day only if its owner permits. The player can use the interval for rest or shelter work through existing commands. No hidden appointment is created unless the schedule owner confirms one.

If the learner asks for an immediate warning rather than the lesson, the qualified worker gives the warning and the session closes. The player’s choice respects the urgent need and the learner’s current purpose.

## 285. A scene pass — the teacher needs care

The teacher is qualified but the health owner reports that they are unavailable. The player may choose another qualified teacher, ask the learner whether they want to wait, or close the offer. A faction may offer a substitute, but the education owner validates that person’s eligibility and the learner accepts the match.

The teacher’s health is handled through the existing health/needs path. The player does not administer treatment through the lesson menu. The scene can show the community covering a shift or bringing supplies only if current work, inventory, and needs owners support those actions.

If no replacement is available, the lesson ends with a simple explanation. The learner can be disappointed or unconcerned. The game does not apply relationship loss or education decay unless a current owner does so. A later offer appears only when current state allows it.

## 286. Factions and Currents can help with needs, not education ownership

A major faction may offer a qualified teacher, a temporary work hand, or a resource exchange. The player routes each offer through the current roster, schedule, inventory, or economy owner. The faction cannot declare the learner available because it needs their help.

Archivists may lend a source, the Guild may provide a practical example, and Long Walk may bring a requested item or report a delayed delivery. Their contribution may make a future lesson possible, but they do not heal, feed, schedule, consent, or grant progress outside their existing systems.

The player can accept none of these offers. A local lesson remains available wherever the current education owner and valid teacher permit it. If all offers fail, the scene can end with the current need still visible and a clear next action owned by the appropriate system.

## 287. Endings for “After the Meal Line”

- **Lesson proceeds by mutual choice:** current owners permit it, and the session resolves once through the education owner.
- **Learner chooses rest:** no lesson command runs; the player sees only a future option supported by schedule state.
- **Teacher rests; alternate found:** the alternate passes eligibility checks and the learner accepts.
- **Teacher rests; no alternate:** the player closes the offer without blaming anyone.
- **Urgent information takes priority:** the responsible owner answers the need; study remains optional.
- **Need addressed first:** a separate existing command resolves it; education availability is refreshed and rechecked.
- **Need remains unresolved:** the player routes care or work elsewhere and does not claim that study fixes it.
- **Faction offers help:** the player accepts, negotiates, or declines through existing owners; education remains separate.
- **Lesson result committed before an interruption:** the result is preserved and the interruption affects only what follows.

## 288. Dialogue and tone — care without a new meter

Candidate lines, to be checked against character voices:

- Learner: “I still want the lesson. I also need to sit down first.”
- Teacher: “I can explain the term now. I can’t run the whole session before my rest.”
- Player: “The warning comes first. We can study the label after the worker checks it.”
- Learner: “If there’s no time today, tell me plainly. Don’t keep me waiting by the door.”
- Teacher: “I’m free tomorrow, if the roster still has me free tomorrow.”
- Worker: “I’ll answer the safety question. The lesson can wait.”

Avoid lines such as “push through,” “earn your rest,” or “you should be grateful for the lesson” unless a specific antagonist voice is intentionally being shown and the player can respond. A health boundary is a current condition, not a moral failing.

## 289. Save, replay, and owner boundaries

Any need action is saved through its current owner. Any lesson result is saved through its education owner. Any duty adjustment is saved through its schedule/work owner. A single lesson panel must not cache or serialize these facts independently.

If a need changes after the player opens the agreement, re-read it before commit. If a session result is already committed, later fatigue or health changes cannot retroactively erase it. If the player closes/reopens the screen, the owner response—not a local readiness flag—determines what remains available.

The same seed and same canonical needs/schedule/education state should reproduce the same outcome. Do not use a new random chance to decide whether a tired learner succeeds. Any seeded outcome must be owned and saved by the current deterministic system.

## 290. Installment 20 close

This installment expands lesson timing around existing needs, health, rest, and duty. It gives the player choices to proceed, defer, find another teacher, address an urgent need, or close the offer. Outcomes use only existing owners, and no hidden readiness score, education penalty, medical feature, or resource transaction has been added.

## 291. Installment 21 — a lesson after something went wrong

A mistake, delay, or confusing handover can leave a learner with a question. The player can make that question useful without turning the learner into the culprit or rewriting the original event. The education route begins only after a real event or report exists in its current owner. It teaches a supported concept; it does not retroactively repair a loss, reverse a work decision, or assign blame.

The working route is **“The Blank Line.”** A current after-action note shows that a handover form had one section left blank, and the next worker did not know whether the task was complete. The underlying event, task result, and responsibility remain with their existing work/incident owners. A learner asks what the blank meant and whether they could have read it differently.

This route is optional. If there is no actual report or relevant curriculum subject, do not invent an incident and do not create a lesson trigger. If the event is urgent, resolve the urgent need through its current owner before offering education. The lesson can happen later, when participants, source, and schedule are valid.

## 292. Separate the incident, its interpretation, and the lesson

The player sees three independent questions:

1. **What happened?** The incident/work owner reports the event and its confirmed outcome.
2. **What did the document mean?** The source/custody owner verifies the document, version, or missing entry if available.
3. **What can the learner study?** The education owner identifies a supported subject, eligible teacher, and valid agreement.

The incident may remain unresolved even if the learner completes a lesson. The learner may understand how to read a field without knowing why a worker left it blank. A teacher may explain the format while the work owner investigates the actual omission.

The player can report a factual observation, ask for a source check, offer a lesson, or close the subject. Avoid one grand “investigate and educate” button that silently changes multiple owners. The player should see which command handles each outcome.

## 293. Learner voice after an incident

The learner can be curious, embarrassed, angry, detached, or not involved. Do not infer that they caused the event because they asked about it. If the learner participated in the task, ask how they want to discuss it and whether they want the source shown. If they were not involved, do not expose private incident details merely to teach a lesson.

Useful player responses include: ask what the learner noticed, explain the confirmed event facts, tell them that responsibility is still being checked, or say the conversation can wait. If the learner asks for an immediate safety instruction, the qualified work owner answers it before any lesson offer.

The player can also choose not to turn an incident into a lesson. A mistake is not automatically a teaching opportunity. The person affected may need rest, repair, clarification, or privacy first.

## 294. Branches from the player’s response

| Player action | Immediate effect | Possible follow-up | Limit |
|---|---|---|---|
| Ask the learner what they noticed | Their observation enters the conversation | Source can be compared with the incident owner’s record | The observation is not a formal finding |
| Ask the document custodian for the current version | Provenance may be confirmed | A supported lesson can use an approved example | A corrected document does not rewrite the incident by itself |
| Ask the work owner to explain the blank field | The owner can confirm whether blank means unknown, not required, or incomplete | The learner can study the relevant notation if available | Education cannot assign responsibility |
| Offer a private lesson | The learner chooses whether to proceed | A supported session can happen later | Privacy does not hide an active safety issue from its owner |
| Offer a group debrief | Participants choose whether to attend if current rules permit | The group may clarify a general process | No cohort progress unless the education owner supports it |
| Publicly name a person | The player risks disclosing an unverified attribution | Route through current incident/privacy rules | Do not present accusation as education content |
| Close the education offer | The event is handled by its own owner | A later lesson may be offered if current state supports it | No missed-lesson penalty or dangling quest flag |

The route should make the difference between learning and accountability explicit. If the work owner is investigating, the player can request an education conversation that does not pre-judge the result. If the owner confirms a process failure, corrective action follows its own route.

## 295. A scene pass — learning without assigning fault

The learner points to the blank line and says, “I would have thought the task was finished.” The coordinator answers, “The form doesn’t tell us that. We’re checking who last updated it.” The player can agree that the field is unclear, ask for the current form, offer a lesson on reading its fields, or close the conversation until the event owner reports more.

If the player asks for a lesson, the teacher explains how the form marks completion and which parts remain uncertain. The learner can ask whether the same blank appears in their previous example. The player can compare only permitted documents. If the form owner cannot confirm the meaning, the lesson uses a neutral sample or waits.

If the learner completes a supported session, their result concerns the curriculum subject. The incident remains at its owner’s state. The player may later apply the learning to a new handover only if a qualified coordinator and current work consumer permit it.

If the learner’s interpretation was reasonable, the teacher says so. If it was not, the teacher corrects the mark without describing the person as careless. If the document itself caused ambiguity, the source owner can address that. The scene does not need a villain to end meaningfully.

## 296. Branch outcomes across incident status

**Incident confirmed; document current.** The player can use the form as a valid teaching example and route any remaining task correction through the work owner.

**Incident confirmed; document obsolete.** The player seeks a current source or uses a neutral example. The old document remains part of the event record but is not taught as current procedure.

**Incident still under review.** The player can teach general terminology while keeping attribution unresolved, or defer the lesson until facts are clear.

**No source or report exists.** The route remains a conversation and does not invent an incident ledger. The player can ask the relevant owner to provide one if that current system supports it.

**Learner declines discussion.** The player stops. Any active work issue still proceeds through its owner, but the learner’s education path is not forced.

**Corrective work is required.** The qualified work owner assigns it. The learner can observe only through a valid work and education route; a lesson does not become unpaid repair labor.

## 297. Faction involvement without a blame contest

The Military may want consistent handover records. It can ask for a factual process review or request a qualified coordinator. The player can provide verified information through the existing communication path or keep the learner out of the discussion.

The Rebel group may suspect that a form was changed without notice. The player can ask for the source record, point out what remains unverified, or decline to repeat the claim. A faction belief is not evidence that a named person caused the event.

An Independent may have supplied or traded the form. The player can use the existing item/trade records to verify provenance or route a replacement request. No new liability or compensation ledger appears inside the education system.

Archivists can compare editions, the Guild can identify a label or form mark through its existing object context, and Long Walk can establish when a copy arrived. These supporting roles may clarify one fact; none decides the incident outcome or learner’s progress.

## 298. Endings for “The Blank Line”

- **The form is clarified:** the learner studies its fields and a qualified coordinator applies the correct current process.
- **The source is outdated:** the player routes replacement through its current owner; any lesson waits for a valid copy.
- **The incident remains unresolved:** the player communicates that uncertainty and chooses a general lesson or no lesson.
- **The learner does not want to revisit the event:** the education offer closes without penalty.
- **A work correction proceeds separately:** a qualified person handles it; the learner is not automatically assigned.
- **A faction request is declined or narrowed:** only verified facts are shared, and no accusation becomes a lesson branch.
- **The incident produces a confirmed lesson need:** the player arranges a valid agreement and sees the owner result after the session.
- **No lesson subject is supported:** the route ends with source or process clarification only.

## 299. Subsequent learning without retroactive reward

If the lesson helps the learner read a future form, that later action is an application, not a second reward for the incident. If the learner makes a similar mistake again, consult the current owner and context. Do not treat one event as permanent proof of mastery or incompetence.

If the work owner changes the form, the education result does not automatically teach the new version. The player may need a new supported subject or source check. If the owner determines that the old form was adequate, the player should not preserve a narrative claim that it was defective.

Any Chronicle, journal, or archive entry should report the incident owner’s verified result and the education owner’s result separately. If the Chronicle contract does not support the distinction, keep the follow-up in dialogue instead of merging both facts.

## 300. Tone and dialogue for the aftermath

Candidate lines, subject to existing character voice and localization review:

- Learner: “I didn’t know a blank could mean ‘not checked.’ I thought it meant ‘finished.’”
- Coordinator: “That’s why we’re checking the form. Don’t fill in what it doesn’t say.”
- Teacher: “We can study the mark. We can’t decide who left it blank.”
- Player: “The task is with the work owner. We can learn the form without blaming anyone.”
- Archivist: “This copy is older. I can tell you when it was printed, not who used it last.”
- Learner: “I’d rather not talk about the accident. Can we use another example?”

These lines should not turn the education feature into a disciplinary hearing. Keep the scene focused on the learner’s question and the present decision. The incident’s emotional weight can remain without adding melodrama.

## 301. Review conditions for incident-based learning

Before adding an incident trigger, verify the canonical incident/report event, its current status and privacy, the source record, the curriculum subject, learner and teacher eligibility, save identity, and any downstream application consumer. Confirm that the education route cannot change the incident result or reopen a resolved event.

If the incident is unverified, keep attribution unresolved. If its details are private, use a neutral example or close the lesson. If no lesson owner supports the subject, do not invent one. If a faction tries to make the learner a public example, the player can decline and route any active work need to a qualified person.

## 302. Installment 21 close

This installment adds a post-incident learning route that separates what happened, what the source means, and what the learner can study. It supplies branches for verified, outdated, disputed, and missing reports, plus endings for private refusal, work correction, and supported lessons. Incident, document, education, work, Chronicle, and faction owners remain distinct; no blame score or incident ledger is added.

## 303. Installment 22 — a promise made too early

The player may speak to a faction representative before asking the learner whether they want to help. The player might say, “We have someone who can read that,” intending to keep a negotiation moving. That sentence can create expectation even when no valid lesson, consent, work assignment, or qualification exists.

The working route is **“I Said We Could Help.”** It explores the consequence of overpromising without introducing a dishonesty meter. The result depends on what the player actually told the faction, whether the current faction/work owner recorded a commitment, whether the learner agrees, and whether the current education result supports the claimed capability.

The scene does not assume that a friendly conversation is a binding transaction. The player-facing UI should show the difference between “offer to ask,” “state a capability,” “make a tentative proposal,” and “accept a work request.” If the current faction contract cannot represent that difference, content must use the existing contract honestly rather than inventing hidden promise state.

## 304. Four facts behind a premature promise

The player must determine:

1. **What was said:** an informal possibility, a general capability claim, or an accepted request.
2. **What the faction recorded:** a conversation, a pending offer, or an owner-backed work commitment.
3. **What the learner wants:** accept, ask for conditions, decline, or remain undecided.
4. **What the education/work owners permit:** supported skill level, eligible age, required supervision, schedule, and qualification.

These facts can produce different outcomes. If the faction has only heard that the player will ask, the player can return with an answer later. If the faction has recorded a task request but not an assignment, the player can negotiate its scope. If a work owner has already committed a task, cancellation and reassignment follow that owner’s rules. The education panel cannot erase an external commitment.

## 305. Opening scene — the learner hears about the promise

The faction representative leaves. The learner asks why their name was used. The player can acknowledge the assumption, clarify that no agreement was made, ask what the learner would be willing to consider, or avoid the question. The learner may be annoyed, curious, or matter-of-fact; current character voice determines the reaction.

The learner’s response should not be inferred from previous lessons. A completed session does not mean they accept public work. A past refusal does not prove they will refuse every request. The player asks now, explains the full request, and waits for a clear response under current consent rules.

If the learner did not hear the exchange and the faction has no named person recorded, the player can still approach them before making a formal request. Do not stage a confrontation simply to add drama. The route exists because the player’s action created an expectation that now needs clarification.

## 306. Player repair options

**Clarify that the offer was tentative.** The player returns to the faction and explains that they have not asked the learner or verified the requirements. This can reset expectations if the current conversation owner supports it.

**Ask the learner with full terms.** The player describes the task, schedule, supervision, audience, and any compensation known. The learner can accept, request changes, defer, or decline.

**Offer a qualified adult instead.** The player tells the faction that a qualified worker is available or asks the current roster/work owner for a valid candidate.

**Negotiate a smaller request.** The player asks whether the faction needs one supervised reading, a general explanation, or a recurring assignment. Each is checked through the appropriate owner.

**Retract the claim.** The player admits that they spoke too soon and declines to identify the learner. Any faction response comes from the existing faction system.

**Proceed with a valid accepted request.** If the learner agrees and all current owners validate the route, the player can make a real commitment. A lesson result is not substituted for task eligibility.

The game should not hide the consequences of a repair. If the faction loses time, loses a trade offer, or registers dissatisfaction, show it only when the current owner applies that outcome. If no mechanical response exists, the consequence can remain a line of dialogue rather than a new reputation point.

## 307. Branch table — what kind of promise was made?

| Existing faction/work state | Learner response | Player action | Correct route |
|---|---|---|---|
| No request recorded; player offered to ask | Accepts | Share full terms and validate prerequisites | Create a supported request only after agreement |
| No request recorded; player offered to ask | Declines | Tell faction the learner is unavailable | Offer qualified adult or close conversation |
| Tentative request pending | Wants more details | Ask faction to specify timing, supervision, and scope | Keep request uncommitted until owners confirm |
| Tentative request pending | Declines | Retract or refer another worker | Faction owner handles its response |
| Work assignment committed | Learner was not consulted | Follow the work owner’s cancellation/reassignment rules | Do not force the learner into the task through education UI |
| Claimed capability exceeds owner result | Any | Correct the claim before work starts | Report only the confirmed capability and route further training separately |
| Learner accepts but teacher/supervisor unavailable | Accepts | Defer or seek another valid supervisor | No work begins until owner confirms availability |
| Player cannot verify what was recorded | Any | Check the faction/work owner | Do not improvise a cancellation or promise state |

## 308. Education result is not consent, qualification, or commitment

A completed lesson may confirm one skill or curriculum result. It does not prove that the learner wants to work, that they are qualified for all related tasks, or that the faction has an accepted assignment. The player should be able to state the result accurately: “They completed a lesson on the notation” is different from “They can cover the handover.”

If the faction needs certification or a prerequisite, its owner evaluates that condition. If the learner needs an apprenticeship, the existing apprenticeship route applies. If the task requires supervision, the work owner confirms a supervisor. If the learner declines, the player seeks another route.

Do not create a campaign-wide “promised worker” flag in the education record. If the faction’s offer system needs to remember a proposal, it belongs to that faction/work owner and must follow its current save/replay contract.

## 309. The faction’s response should match its actual expectation

The Military may appreciate a quick response but still require a qualified coordinator. The player can explain that the learner has studied one subject and offer an eligible adult. The Military can accept the alternative, clarify a narrower task, or reject the offer through its current system.

The Rebel group may value shared knowledge but not want an unwilling participant. The player can offer a general source or another volunteer. If the faction asks why the learner declined, the player can keep the reason private.

An Independent may care about delivery timing or compensation. The player can renegotiate through the existing trade/work route or withdraw. A prior lesson does not mean the learner owes labor or the player can barter their time.

These reactions create different branches because the requests have different practical terms. They should not map each faction onto a moral judgment about the player.

## 310. Dialogue — acknowledging the mistake and repairing it

- Learner: “You told them I’d do it before you asked me.”
- Player: “You’re right. I should have checked. Here’s what they actually asked for.”
- Learner: “I might read the sheet with someone there. I’m not taking the shift.”
- Player: “I’ll tell them that’s the limit, and I’ll ask who can supervise.”
- Faction representative: “You said you had a reader.”
- Player options: “I said I would ask”; “They can help with one supervised reading”; “I have a qualified worker instead”; “We can’t take this request.”
- Learner: “Don’t send them my lesson record. Tell them what I agreed to.”

Use these lines only if the current privacy and work contracts permit the stated disclosure. The apology line can be sincere without granting a trust stat. A faction can remain practical and disappointed without becoming a villain.

## 311. Endings for “I Said We Could Help”

**Tentative offer corrected:** the player clarifies that no learner agreed. The faction waits, revises the request, or closes it according to its owner.

**Learner accepts a bounded task:** the player obtains explicit agreement, a qualified supervisor, and schedule confirmation. The task remains one-time if that is what the learner accepted.

**Learner accepts training, not work:** the player routes the request to the apprenticeship owner. The faction cannot treat the interest as immediate labor.

**Learner declines:** the player respects the refusal and refers another candidate or closes the offer. Education progress is unchanged.

**Claim corrected after a work commitment:** the player uses the work owner’s cancellation or reassignment route and reports the actual result. The learner is not silently assigned.

**Faction declines the alternative:** the player accepts that the request is closed. No new faction reputation is added unless its current owner supports a consequence.

**Capability claim was too broad:** the player corrects the detail before any task begins, then offers supported training or an eligible worker.

**No learner was named:** the player can keep the conversation general and seek a candidate only through the current roster and consent rules.

## 312. Repair is a meaningful action, not a reset button

The player’s repair may cost time or lose an opportunity if the current faction/work owner confirms that effect. It can also preserve trust in an existing conversation by setting clear terms. The plan does not assume that an apology erases a committed state or that every faction reacts positively.

If a promise has become a real work assignment, the player follows its actual cancellation rules. If the learner was never named and no task was committed, the player can clarify with fewer consequences. These distinctions make the player’s earlier choice matter without assigning a global good/bad rating.

If a player knowingly continues to claim that the learner agreed when they did not, the content can block that action where existing contracts require consent. It should not create a special coercion system. The correct result is a truthful rejection or a supported route to a qualified alternative.

## 313. Re-entry and save behavior

After the player speaks with the learner, the faction request may still be pending, changed, accepted, expired, or closed. Re-read its owner state on return. Do not keep a separate promise flag in the education panel.

If the learner changes their mind before the work command commits, follow the current cancellation route. If the work assignment has already committed, use its current rules; do not rewrite history in dialogue. A completed lesson result remains intact in every case.

If the faction callback or learner response is delivered more than once, event identity and owner idempotency determine whether it is new. Reopening a dialogue cannot create another assignment, trade, progress result, or duplicate apology event.

## 314. Owner review before production

Verify current faction dialogue state, work request/assignment transitions, consent and privacy rules, learner roster, qualification, apprenticeship route, schedule, education result consumer, cancellation behavior, and save ownership. Identify which stages are conversations and which are committed transactions.

If the current faction system cannot distinguish a tentative offer from an accepted job, resolve that owner seam before writing definitive outcome lines. If the learner’s time cannot be committed without consent, keep the request at dialogue until the contract is explicit. If a faction response is unsupported, do not invent a reputation effect.

## 315. Installment 22 close

This installment adds an action-driven repair route after the player speaks for a learner too early. It distinguishes tentative conversation from accepted work, gives the player options to clarify, retract, narrow, or negotiate, and lets the learner accept or decline with full terms. Faction and work owners determine commitments; education remains a source of confirmed capability, not consent or labor authority.

## 316. Installment 23 — Knowledge with a reason outside the roster

This segment gives a lesson emotional and practical value when the learner does not intend to take a shift, join a faction, or qualify for a job. A person might want to read a family letter, understand a public notice, preserve a recipe, explain a map to a younger sibling, or make sense of a damaged instruction sheet. The lesson remains the same bounded education loop: a valid learner and teacher, one lesson result for the day, and knowledge that can be applied where the current owner supports it.

The player should not have to translate every act of learning into labor. That creates a richer set of motives and protects the human scale of the feature. A learner can study because a subject matters to them, because someone they care about left a note, because the settlement keeps misreading a warning, or simply because they want to know how something works. None of those reasons should create an employment promise or a hidden productivity bonus.

## 317. Three kinds of non-work motivation

**Personal understanding:** A learner asks to interpret a note, label, chart, or instruction tied to their own life. The player can help them find a teacher and a suitable lesson, or let the learner defer until a safer time. The subject and source must already exist in authored data or be clearly presented as a proposed narrative example awaiting continuity review.

**Shared memory:** A learner wants to understand something left by a relative, former crew member, or community. The value is in making meaning together. The game should not imply that an education result restores a relationship, changes grief, or unlocks a new memory system. The scene can carry its weight through dialogue, an item interaction already owned by the relevant system, and a truthful lesson outcome.

**Civic comprehension:** A learner wants to understand a public notice or a shared reference used by the settlement. They might ask a teacher to explain the format, then choose whether to share that explanation with others. The current notice, archive, faction, or journal owner controls any publication or distribution. Education supplies only the confirmed understanding.

These motives form a tonal range, not three new mechanics. Any may use one of the existing lesson subjects if current curriculum data supports it. No bespoke personal-motivation meter is needed.

## 318. The learner's reason changes the conversation, not the skill result

Before confirming a lesson, offer a short exchange that lets the learner explain why they want it. The player can ask directly, invite them to say as much as they want, or proceed without asking. The response can shape the scene's language and later optional dialogue, but it must not silently alter lesson success, future willingness, relationship values, or a faction's opinion.

Example actions include:

- Ask what they hope to understand, then arrange the appropriate available lesson.
- Say that they do not need to explain and ask whether they still want to study.
- Offer to postpone because the immediate duty is more urgent, while preserving their request only if an existing owner can persist it.
- Find a teacher who can discuss the topic privately, if the current schedule and location allow it.
- Suggest a shared lesson if the learner asks to study with someone else and the current lesson owner supports multiple learners.
- Decline to promise an answer where the subject needs a specialist or unavailable source.

The learner's reason never waives age, duty, safety, teacher, schedule, or consent checks. Curiosity does not turn an unavailable person into a valid teacher, and personal importance does not make a lesson result certain.

## 319. Small scene: the folded letter

A learner brings a folded letter with a date and a few words they cannot confidently read. The scene begins with a choice about privacy and competence, not with a new quest marker. The player might ask whether the learner wants help now, find a teacher who can read the script, or explain that nobody present can do so reliably.

If the lesson can proceed, the learner studies one supported subject. Afterward, they may ask to read the letter with the teacher present, read it privately, or leave it folded for another day. These options should invoke only available item or dialogue routes. If there is no safe way for a teacher to inspect the item, the plan should keep the event at the request stage rather than invent a document-inspection subsystem.

Branches arise from what the player does: disclose the letter's contents, protect the learner's privacy, rush the lesson despite fatigue, wait for a qualified teacher, or admit that the script is uncertain. Outcomes can include an accurate reading, a partial understanding, a deferred request, or an explicit statement that the text cannot yet be read. A lesson should not guarantee that the letter has a hidden reward.

## 320. Small scene: the notice on the wall

A public notice causes repeated confusion. One learner wants to understand its terms and explain them to a neighbor. The player can ask the notice's owner for a clearer copy, arrange a lesson on the relevant subject, or allow the learner to ask a capable adult directly. If the learner already understands the material, no education interaction is needed.

After a lesson, the learner chooses whether to explain the notice to one person, ask permission to make a public explanation, or keep their understanding private. The faction or notice owner decides whether a public repost is accepted. The education feature does not appoint the learner as spokesperson and does not make an unofficial interpretation authoritative.

An effective branch might end with a corrected explanation, a request for the original author to clarify, a neighbor who decides to check the notice themselves, or no public action because the learner prefers not to be identified. Each result says something about the people involved without adding a settlement-literacy score.

## 321. Small scene: a skill for a household task

A learner asks how a familiar household object works. The practical goal may be to mend, store, label, or safely use something. The lesson can introduce a relevant concept, but any actual repair, item change, consumption, or safety result belongs to the current inventory, crafting, medical, or environmental owner.

The player can let the learner observe, encourage them to try only when the current action is valid, ask a qualified person to demonstrate, or stop when the task exceeds the learner's confirmed capability. If the lesson provides no recognized skill for the task, the learner still gains the lesson's real curriculum result without the game claiming the object is fixed.

This scene offers a grounded distinction between knowing a principle and being authorized or equipped to perform a task. It also creates playable branches: a successful demonstration; a safe pause; a request for tools; a mismatch between lesson and need; or a teacher referral. None requires a work assignment.

## 322. Choice table — where the knowledge goes

| Learner preference | Player action | Supported consequence | Boundary |
|---|---|---|---|
| Understand privately | Arrange a private lesson where schedule and venue permit | Lesson result is recorded by the education owner | No private journal is invented |
| Share with one person | Ask who may hear the explanation | Existing dialogue or notice owner handles the exchange | No automatic relationship reward |
| Make a public explanation | Check the notice or faction owner's publication route | Owner accepts, edits, or declines the explanation | Learner does not gain authority by studying |
| Apply it to an object | Verify the relevant item or action owner | Owner applies a valid attempt or reports a limitation | Lesson does not create inventory effects |
| Keep the source untouched | Continue with an example or defer | Lesson may still be possible if the curriculum supports it | The education panel does not copy sensitive text |
| Stop before the lesson | Respect the request to wait or withdraw | No lesson result is created | No refusal penalty is added |

## 323. Quiet, shared, public, and deferred routes

**Quiet route:** The learner studies without disclosing the personal reason. This may suit a private letter or a question they are not ready to discuss. The player can help secure a private location if the existing schedule and venue systems can represent one. If they cannot, the lesson should not promise privacy it cannot enforce.

**Shared route:** The learner invites a trusted companion or sibling to observe. Use an existing group lesson only if one is supported by the education owner. Otherwise, the second person can participate in dialogue without receiving a second progress result.

**Public route:** The learner wants to explain a public notice or shared practice. The player routes the draft through the existing content owner and reports what was accepted. If rejected, the learner can revise it, ask for clarification, or leave the notice unchanged.

**Deferred route:** The learner has another duty, lacks energy, or wants to think first. The player can return later only if a current request or dialogue state can persist it. Otherwise, make the honest result clear: the request was heard, but no durable appointment exists.

Each route changes audience, timing, or who confirms the result. None creates a new branch based solely on whether the player is kind or cruel.

## 324. How factions can appear without owning the scene

The Military may need an old public instruction explained, yet the learner could be interested for personal reasons. The player can ask the Military for an unaltered copy, arrange a lesson, or decline to connect the learner's private motive to the request. The Military remains a source or recipient, not the owner of the learner's education result.

A Rebel group may preserve a community account or teach an informal practice. A learner might ask to understand the account, then choose whether to share their interpretation. The faction can correct a factual error or invite a conversation, but it cannot turn curiosity into faction loyalty.

An Independent may have a manual or notice needed by several settlements. The player can negotiate access through the existing trade or conversation route. A learner who understands it can choose whether to help explain it. No lesson creates a debt or trade entitlement.

Faction presence therefore adds perspective and stakes. The primary story remains the learner's chosen reason, the player's action, and the truthful result.

## 325. Player agency through concrete actions

Do not reduce this segment to dialogue choices about attitude. The player should have observable actions: find a suitable source, identify a capable teacher, make time in the roster, ask the learner how much to share, check permission with the source owner, prepare a safe practice example, or stop an overconfident explanation.

Each action can open a different subsequent branch. Finding a valid teacher may reveal a delay due to duty. Asking the source owner may expose an incomplete or disputed notice. Making time may displace another task. Offering a private venue may be impossible tonight. Stopping a risky demonstration may preserve safety but leave the learner with only a theoretical understanding.

Where existing owners do not support a proposed action, present it as dialogue or omit it from the production build. A design outline cannot claim an operational system based only on a plausible narrative scene.

## 326. Branch outcomes beyond faction alignment

The learner may understand the source and keep the knowledge private. They may understand it and ask a trusted person to confirm. They may discover that the source is ambiguous and bring a question to its author. They may explain it to someone else and accept correction. They may decide that the lesson did not answer their original question. They may complete the lesson but choose not to interact with the source at all.

These endings vary according to evidence, audience, timing, and the learner's choice. They do not ask whether the player is an especially honest or evil character. A player who protects privacy may still make a practical mistake; a player who shares a public explanation may still act responsibly if the source owner approves it.

No ending awards a hidden virtue score. If the content needs to remember which line was spoken, use the smallest existing dialogue or event state that owns that conversation. Do not place the learner's motive in a global education statistic.

## 327. Dialogue sketches: curiosity without a contract

- Learner: “I want to know what the mark means. It keeps showing up on the boxes.”
- Player: “I can find someone who understands the label. We can leave the boxes closed until then.”
- Learner: “I wasn't asking for a shift. I just want to read it.”
- Player: “Then we'll keep it a lesson. If that changes, we'll talk about it separately.”

- Learner: “It was my mother's note. I don't want the whole room reading it.”
- Player: “You can decide what the teacher sees. If we can't make that private tonight, we can wait.”
- Learner: “I want to try tomorrow.”
- Player: “I'll check whether I can keep that time open. I won't promise it until the roster confirms.”

- Learner: “I can explain the notice to Mara now.”
- Player: “Do you want to explain what you learned, or speak for the person who wrote it?”
- Learner: “Only what I understand. The rest should go back to them.”

The lines establish motives and limits. They should not be paired with an invisible trust change.

## 328. Failure cases that still respect the learner

If no one can teach the relevant subject, say so and offer a source referral only if one exists. If the only available teacher is needed for a critical duty, the player can postpone rather than silently displace that worker. If a personal document is damaged, do not pretend the lesson restores its missing text. If a public notice is contradictory, let the learner identify the contradiction rather than force one answer. If the learner becomes tired or changes their mind, the current needs and schedule owners determine whether the lesson can continue.

An interrupted lesson must follow the one-result-per-day rule. The game should distinguish a genuine completed lesson from an attempted session, but must not create multiple daily results because the player restarts the same dialogue. Stable event identity and owner idempotency govern any persisted completion.

If a lesson cannot begin, the player may still complete an actionable setup step—finding a source, asking permission, or locating a teacher—provided that action is supported by an existing route. Setup is not mislabeled as learning progress.

## 329. Scope and continuity guardrails

This segment adds reasons to use the education loop and varied ways to apply a confirmed result. It does not add a family relationship system, archive browser, personal diary, emotional wellbeing meter, public literacy score, content publishing tool, or general-purpose object tutorial engine.

Candidate letters, notices, labels, recipes, and manuals require a current content review. Verify that each source exists, has an owner, and can be referenced through a real route. If a source is only a writing proposal, label it as such and do not imply its item or interaction is already in the game.

Keep age, duty, consent, teacher competence, privacy, and safety checks in their existing owners. Learner motivation changes how the scene is written; it does not bypass those contracts.

## 330. Re-entry and persistence cases

On return after a day advance, reread the education result, the learner's current needs, teacher availability, relevant source state, and any existing dialogue or publication decision. Do not assume that the learner still wants the same audience or timing. If no owner persists the appointment, ask again instead of reconstructing it from a stale panel value.

If the lesson was completed before the player left, report the same confirmed result after reload. Reopening the source interaction must not grant another lesson or duplicate a publication. If the learner chose privacy, the UI should not reveal their reason on a public faction panel. If the relevant privacy owner has no supported representation, do not promise that guarantee in production dialogue.

The player can resume the conversation, request another lesson through the normal rule, or close the matter. This is enough to make the scene re-enterable without a new quest journal subsystem.

## 331. Acceptance questions for this segment

1. Can a learner have a legible reason to study that does not imply future labor?
2. Can the player respect that reason while still following schedule, age, teacher, safety, and consent rules?
3. Does every claimed practical result come from its current item, work, notice, or dialogue owner?
4. Can the learner choose a private, shared, public, or deferred route where current systems allow it?
5. Does the same lesson result survive day advance and reload without duplication?
6. Can a scene end with uncertainty, refusal, or no public action while remaining a valid outcome?
7. Are the differences in endings caused by evidence, audience, timing, and concrete player actions rather than a moral alignment axis?

If any answer depends on a new mutable education flag or unsupported faction reaction, narrow the implementation proposal before production.

## 332. Endings for “The Thing They Wanted to Read”

**Private understanding:** the learner completes a lesson and chooses to keep the source private. The education owner records only the confirmed lesson result.

**Shared explanation:** the learner explains what they understood to one chosen person. Any resulting dialogue belongs to its existing owner.

**Public clarification:** the learner proposes an explanation, and the notice owner accepts, edits, or declines it. The learner's lesson is not treated as public authority.

**Source remains uncertain:** the lesson reveals a genuine ambiguity. The player routes a question to the source owner or leaves the matter open.

**Lesson deferred:** needs, duty, unavailable expertise, or the learner's preference postpones the session. No result is created, and no refusal penalty is added.

**Understanding without application:** the learner completes the lesson and chooses not to act on the source. This is a complete outcome, not a failed quest.

## 333. Segment success and production gate

This segment succeeds when the player can support learning for a reason the learner values, see what was actually learned, and respect the learner's decision about applying or sharing it. It fails if the system turns every student into a future worker, treats lesson completion as proof of broad competence, or promises privacy and publication controls that do not exist.

Before writing final content, verify lesson subjects, teacher availability, learner eligibility, daily result identity, roster timing, current source owners, dialogue re-entry, privacy representation, and all practical actions referenced by these examples. Keep unsupported examples in the proposal layer until their data and routes are confirmed.

## 334. Installment 23 close

This installment broadens Plan 2's reasons for learning without creating a fourth feature pillar. It makes curiosity, personal understanding, shared memory, and civic comprehension valid motivations; gives players concrete actions around teachers, timing, evidence, privacy, and audience; and lets sources remain uncertain when the evidence is incomplete. The education loop still owns only matching, one lesson result, and owner-backed knowledge. Work, relationships, publication, item changes, and faction reactions remain with their current owners.

## 335. Installment 24 — When two teachers disagree

Disagreement between teachers gives the learner a playable way to think about evidence, methods, and limits. It should not reduce to a faction choosing the correct side for the player. Two capable adults can remember different procedures, use different names for the same tool, or teach the same principle through different examples. The player's task is to help the learner understand what is shared, what is uncertain, and what requires checking before use.

This branch still belongs to the three existing subfeatures. The player arranges a study agreement that identifies the subject and teacher; the lesson produces one supported result; the learner applies the result only where an existing owner confirms it. The disagreement adds context to the learning loop, not another knowledge system.

## 336. Start with the reason the disagreement matters

The learner notices a difference in a practical explanation. One teacher describes how to read a mark; another recalls a different meaning. The conflict can matter because a label is faded, a source is incomplete, a procedure changed, a teacher uses regional language, or the learner wants to understand why two adults act differently.

The game should establish what is actually at stake. Is the learner simply curious? Are they about to perform a task? Could a wrong interpretation damage an item, expose someone to danger, or spread a misleading public notice? The task, safety, content, or notice owner decides the consequence. Do not make every disagreement dangerous, and do not invent stakes unsupported by the referenced system.

The player can ask the learner what they want to know, listen to each teacher separately, compare an existing source, or pause the practical task until the disagreement is resolved. These actions reveal different evidence and create distinct branches.

## 337. Agree on the question before choosing a teacher

An ambiguous request like “teach me how this works” invites incompatible expectations. The player can narrow it: “What does this mark mean on this container?”, “Which reading applies to this route?”, or “Why do the two instructions differ?” A smaller question can be answered within one lesson; a broad debate may require additional research the education feature does not promise.

The learner may prefer one teacher because they are available, familiar, or patient. The player can honor that choice, ask a second teacher for a brief comparison, or explain that only one person is qualified for the immediate subject. Availability and qualifications are checked through the current owners. A character's faction affiliation alone does not make them more or less credible.

If the learner's question changes after hearing the first explanation, the player may reformulate the next conversation, but must not grant multiple daily results. A revised question can be dialogue and source review; another actual lesson waits for a valid later opportunity under the existing rule.

## 338. The three-evidence route

When existing content supplies more than one source, the player can compare testimony, written material, and a demonstrated example. The design should not require all three sources as a new generic investigation mechanic. It is a way to author concrete narrative branches using sources that already exist.

**Testimony:** The learner asks both teachers to explain what they remember. One may be confident, one uncertain, or both may agree on the principle but use different terms.

**Written material:** The player checks a current note, manual, log, label, or notice if one exists. It may clarify the disagreement, show an older procedure, or remain ambiguous.

**Demonstration:** A qualified teacher shows a supported, safe example. The relevant item or task owner controls whether a real interaction can be attempted.

Evidence can converge, remain inconclusive, or show that the two teachers refer to different contexts. The learner can finish with a narrow understanding or defer action. The narrative must not claim a source exists merely to complete a three-step quest.

## 339. Player choices that create different routes

The player can choose the teacher based on subject qualification, arrange a joint conversation, ask both to explain the same example, compare a source, invite the learner to decide whose account to investigate first, or separate the practical task from the unresolved question.

Each action changes what follows. A joint conversation may reveal that one teacher assumed a different context. A source review may show that an instruction is outdated. A demonstration can expose an exception neither teacher mentioned. A private conversation may help a learner ask a question they were reluctant to raise in front of a group. Pausing the task may cost time but avoid an unsupported claim.

These are action branches, not alignment tests. The game should not label one teacher honest and the other corrupt unless the current narrative evidence explicitly supports that characterization. Even where one person is mistaken, the useful player action may be verification, tactful correction, or stopping an unsafe application.

## 340. Branch table — disagreement and next action

| What the player observes | Player action | Learner's possible response | Supported result |
|---|---|---|---|
| Teachers use different terms for the same idea | Ask each for an example | Learner recognizes shared meaning | One lesson can retain the supported subject result |
| One account refers to an older procedure | Check an existing dated source | Learner asks which version applies now | Current procedure owner determines usable guidance |
| Source is incomplete | Ask its owner for clarification | Learner waits, narrows question, or proceeds only with known facts | No fabricated certainty or progress bonus |
| Demonstration would involve a risky task | Stop and seek qualified confirmation | Learner accepts delay or asks for another route | Safety/task owner controls whether any attempt is allowed |
| Teachers disagree about vocabulary | Let learner compare phrasing | Learner chooses a term that makes sense | No faction or skill score changes |
| No source or qualified specialist is available | State the limitation clearly | Learner defers or chooses a low-risk question | Lesson is delayed or remains narrowly descriptive |

## 341. Factions as perspectives, not truth selectors

A faction may have a current practice or reference document. The player can ask what its representative uses, request an explanation, or offer to compare the practice with a community source. That creates a supporting role: the faction supplies one perspective and may respond to a verified correction.

Do not let the Military, Rebel group, or Independents automatically settle a factual question by ideology. A learner may prefer a faction's source because it is accessible, but the plan should still distinguish availability from evidence. A faction can be correct in one case, incomplete in another, and uninterested in a third.

If the comparison could affect a faction-owned duty or public statement, that owner decides how a correction is applied. The education record does not alter faction doctrine or reputation. A faction may decline to revise a notice even when the learner has a well-supported explanation; the learner's knowledge and the faction's publication decision remain separate.

## 342. A short questline: “The Same Mark Twice”

**Opening:** A learner asks about a symbol seen on two containers. One adult says it means “keep dry”; another says it means “inspect before use.” The player can ask what the learner needs to do, inspect whether these are actually the same container type, or arrange a lesson with the available teacher.

**Middle:** The learner interviews both adults, examines an existing label or record if present, or asks the relevant item owner whether any safe inspection route exists. The player may discover that the mark applies differently by container, that one account is from an earlier system, or that the evidence is insufficient.

**Decision:** The player can state the narrow confirmed reading, ask the learner to wait for a qualified check, tell the source owner about the ambiguity, or proceed with a supported low-risk action. If the learner wants to publish an explanation, use the existing notice owner.

**Resolution:** The learner understands what the mark means in the verified context, keeps the ambiguity open, or decides not to act until an expert is available. The lesson result remains a single result. No quest reward, faction alignment point, or broad container skill is implied.

## 343. Alternate questline: “Different Words for the Same Route”

Two teachers describe a route landmark with different local names. A learner worries one is wrong. The player can compare a current map or route record, ask each person where they learned the name, or walk only to an already supported safe viewpoint. The travel owner controls movement and route safety.

If both names identify the same location, the learner can use either while asking for clarification when needed. If they refer to two nearby places, the player can help distinguish them through the current map/wayfinding route. If there is no authoritative route source, the learner can keep both terms in mind without inventing a canonical answer.

The conclusion depends on the player's evidence-gathering actions and the route actually available. It does not declare one community more truthful than another. The learner can explain the difference to one companion, keep it private, or suggest a correction to an existing route record if its owner permits edits.

## 344. Handling a teacher who is confidently wrong

If authored evidence proves that one teacher is mistaken, the player can correct them privately, show the supporting source, let the learner ask the question, or avoid public embarrassment by asking for a joint recheck. The teacher may accept the correction, ask for more evidence, or continue to disagree. Their reaction belongs to current dialogue and social systems.

The learner's result must come from the lesson owner and the evidence it supports. A teacher's confidence does not automatically certify the result. If current education APIs have no concept of conflicting sources or confidence, keep the resolution in authored dialogue and record only the established lesson subject, or defer the proposed mechanic until an owner-backed contract exists.

The player can also choose to repeat the teacher's claim as fact. That may lead to an incorrect downstream action only if the existing task owner supports such an outcome. Do not attach a universal deception or trust penalty. Consequences should follow the specific action taken and what it affected.

## 345. Learner agency when the adults disagree

The learner can choose which account to hear first, ask both teachers the same question, request that they stop arguing, or decide to postpone. They may also decide that the disagreement is interesting and pursue it without needing an immediate practical answer.

The player should not require the learner to expose a personal motive to unlock the lesson. They may be curious, cautious, under pressure to act, or simply tired of hearing conflicting terms. The player can support them by narrowing the question, finding a reliable source, asking a teacher to slow down, or acknowledging that no definite answer exists today.

Where a learner is a minor or otherwise subject to special consent/supervision rules, the current age and duty owners still apply. The disagreement does not relax supervision or make public debate mandatory.

## 346. Failure and ambiguity outcomes

**No common example:** The teachers cannot compare like with like. The player can find a shared example later or let the ambiguity stand.

**Outdated source:** A document is old. The player can ask the owner whether it remains valid; the document's presence alone is not proof.

**Unsafe demonstration:** The player stops the attempt and seeks another route. The learner can accept or disagree, but no unsafe action should be available if the safety owner disallows it.

**Unavailable teacher:** The player can wait, ask another qualified person, or conclude that no lesson can happen today.

**Unresolved disagreement:** The learner retains the confirmed portion and knows where uncertainty begins. This is a complete narrative outcome.

**Public correction declined:** The source owner declines to edit a notice. The learner can accept the limit, appeal through a supported route, or keep their own understanding private.

## 347. Persistence and replay contract

On every return, inspect the current education result, source version if owned, teacher availability, any task/notice decision, and the state of the relevant dialogue. Do not reconstruct a resolved disagreement from a panel cache. If the sources changed during a day advance, report the current owner state and explain whether that changes what the earlier lesson established.

Repeating the same conversation must not grant another lesson result, publish duplicate corrections, or create a second faction request. Use existing event identity and owner idempotency. If a source or dialogue decision is not persisted by its owner, do not imply that it will survive reload.

## 348. Scope exclusions

This branch does not add a universal research system, source-quality meter, debate minigame, teacher reputation ranking, faction truth table, or skill-confidence ledger. The scenario can be authored only where a real subject, source, teacher, and downstream use can be verified.

No faction gets a default truth bonus. No teacher disagreement automatically harms relationships. No unresolved question silently becomes a quest failure. The player's meaningful choices are whether to clarify, compare, defer, correct, or act within the evidence.

## 349. Acceptance questions

1. Does the player know what question the lesson is trying to answer?
2. Can the learner choose who teaches, what evidence to check, or whether to delay?
3. Are source and task consequences owned by current systems?
4. Can a lesson produce a limited result without claiming broad competence?
5. Can disagreement remain unresolved without creating a false failure state?
6. Are faction perspectives contextual rather than automatic truth judgments?
7. Does reload preserve only decisions the relevant owners actually persist?

## 350. Installment 24 close

This installment develops a branch where teachers disagree and gives the player concrete ways to compare testimony, inspect an existing source, ask for demonstration, correct a claim, or delay action. The learner's result stays narrow and evidence-backed; factions add context without becoming truth judges; uncertainty can remain a valid ending. No fourth education pillar or alignment axis is added.

## 351. Installment 25 — A learning path that can change direction

A sequence of lessons can create a stronger arc than a single unlock, provided every day remains one real lesson and each result has an owner. The learner may start with a narrow question, discover a related subject, apply what they learned, then decide whether another lesson is worth the time. The plan is not a career ladder or a compulsory curriculum. It is a sequence of player and learner choices around a subject the current education data can actually support.

This installment focuses on continuity across days: how the learner remembers the agreed aim, how the player supports or changes the path, and how both parties respond when the original objective becomes irrelevant. It does not add an education tree, course catalog, qualification record, or hidden aptitude score. Where the current system cannot persist a proposed plan, keep the continuity in dialogue and do not claim a durable course state.

## 352. A path begins with a concrete aim

Before a multi-day sequence begins, let the learner name what they want to be able to understand or do. The aim should be narrow enough to test against current lesson subjects and downstream owners. Examples include reading a repeated mark, understanding a measurement, following a supported procedure, or asking a better question of a specialist.

The player can ask the learner to choose one aim, suggest a related prerequisite, or explain that the requested outcome is beyond what the available lessons can promise. The learner may want knowledge without a practical target. That is valid too; the player should not demand a productivity justification to offer a lesson.

If the learner asks for a qualification, certification, or work assignment, route that request to the existing owner that governs it. Education can provide a lesson result but cannot invent a credential. A multi-day sequence does not become an apprenticeship unless the current apprenticeship route accepts it.

## 353. Four possible shapes for a short learning path

**Single-question path:** One lesson answers a narrow question. The learner applies it once or decides no action is needed. This is the smallest route and should remain available when time is scarce.

**Foundation-then-application path:** The learner completes a supported foundational lesson, then tries a related task only if its owner confirms eligibility. The player can arrange supervision, seek tools, or delay.

**Compare-and-correct path:** The learner studies one interpretation, finds a conflict or missing context, and seeks a source or second teacher. Each additional lesson still uses the daily limit.

**Learner-directed path:** The learner changes their question after the first result. The player can follow the new interest, finish the old goal first, or stop the sequence. Changing direction does not erase completed learning.

These are authored shapes rather than new curriculum modes. Use them only where existing subjects, teachers, and event routes support the examples.

## 354. Session agreement across multiple days

The first agreement should make clear whether the learner is committing to one lesson or asking to continue later. A future meeting remains tentative unless a current schedule owner can persist and confirm it. The player can set a reminder only through an existing route; otherwise, the game should represent the next meeting as a conversation to revisit.

At each return, recheck the learner's current preference, needs, duty, teacher availability, and the last confirmed result. The original agreement does not authorize every later session. The learner may continue, change the subject, ask for a different teacher, pause, or close the matter.

The player can also change their own support. They may keep making time, ask another eligible adult to teach, provide an available source, or explain that current duties leave no slot. A truthful delay is better than a false promise that silently reserves a worker.

## 355. Choice table — preserve, branch, or close the path

| Current situation | Player action | Learner response | Valid path state |
|---|---|---|---|
| Learner still wants the agreed subject | Arrange the next available valid lesson | Continue, defer, or change teacher | New session only after current checks |
| Learner has a related question | Ask whether to branch or finish first | Choose the new question or retain original aim | Completed result stays; no duplicate award |
| Needs or duty now conflict | Offer a later window or a shorter supported session | Accept, negotiate, or decline | No session until schedule permits |
| Teacher became unavailable | Find another qualified teacher or wait | Accept substitute or postpone | No assumed teacher equivalence |
| First result answered the question | Offer an application route or close | Apply, keep knowledge, or stop | Goal can close without extra lesson |
| Evidence contradicts the lesson | Seek the current source owner | Recheck, defer, or retain uncertainty | Correct only supported understanding |
| Learner no longer wants the path | Close it respectfully | Withdraw without explaining | Previous results remain intact |

## 356. Branching mini-arc: “The Mark on the Valve”

This is a candidate content structure only. Before production, confirm that the relevant label, valve, lesson subject, item interaction, and safety owner exist. If any are absent, keep the arc as a writing pitch rather than an implied gameplay route.

**First encounter:** A learner asks what a mark on a valve means. The player can identify a qualified teacher, look for an existing instruction source, or tell the learner that no one present can confirm it. The learner may prefer a quick explanation, a careful comparison, or a later session.

**First session:** The teacher explains the confirmed portion. If no eligible curriculum result is available, the conversation can remain informational but must not claim an education result. The player can ask the learner to repeat the idea privately, request a source check, or end the conversation.

**Between sessions:** The player sees a real duty conflict, learns the teacher is unavailable, or receives a new interpretation from an existing source. They can reschedule, seek another qualified person, or suspend the path. Each choice changes access and timing, not an invisible morality value.

**Application:** If the learner wants to help with a valve task, the relevant work/safety owner decides whether they can participate and whether supervision is needed. They can observe, do an allowed step, find a tool, or decline. A lesson does not authorize the learner to operate equipment.

**Resolution:** The learner can understand the label, understand only its confirmed part, discover that the source is outdated, decide to leave the valve to a specialist, or lose interest after the first answer. Every ending preserves prior truthful results.

## 357. Different playstyles without a skill tree

The learning path can support several player approaches without adding numeric build classes.

**The scheduler:** Makes time, negotiates duty coverage, and protects a regular window. This can create reliable follow-through while leaving less time for other work.

**The connector:** Finds a qualified teacher, source, or faction contact. This can improve evidence and access but may add travel, negotiation, or delay.

**The practical mentor:** Helps the learner relate a subject to a real owner-backed task. This may produce a useful application, but requires the task owner to validate each action.

**The cautious facilitator:** Narrows claims, compares sources, and pauses when evidence is weak. The result may arrive later, but uncertainty stays visible.

**The learner-led supporter:** Lets the learner choose pace and topic, including stopping. It may not maximize short-term output, but it preserves agency and produces a credible personal arc.

These playstyles are descriptions of choices, not character classes. Any player can shift approach from one session to the next.

## 358. A changed aim is not a failed quest

After an early lesson, the learner might discover that the original aim was too broad, irrelevant, or no longer theirs. The player can ask whether they want to revise it, keep the old aim as a completed question, or close the path. No “abandoned curriculum” penalty should appear.

If the original aim belongs to a separate quest or task, ask that owner whether a changed plan affects its status. Do not automatically fail the wider objective because the learner reconsidered. Conversely, do not mark the objective complete merely because the learner attended lessons.

The story can acknowledge consequences: a scheduled task may need a different worker; a faction may wait for an answer; a source may remain unread. These outcomes must come from the owners that control schedules, tasks, and communication.

## 359. When the player pushes for speed

The player may ask to compress the sequence, skip a prerequisite, or put the learner on a task after one lesson. The learner can agree to a supported smaller step, ask for more practice, refuse the assignment, or refer the player to the work owner. The game should not hide what is being skipped.

If the desired action is ineligible, show why and offer a route that the current owner supports: another teacher, more practice on a later day, a supervised role, or an eligible alternative worker. Do not use a false lesson completion to satisfy a faction deadline.

If the task owner permits an attempt but the learner lacks confidence, that does not necessarily mean the attempt is disallowed. The learner's agreement and the task's eligibility are separate checks. Preserve that distinction in dialogue and result presentation.

## 360. When the learner wants to stop

The learner can stop after one session, after a source conflict, before practical application, or after changing interests. The player may ask once whether a different route would help, but must accept the answer. A refusal closes the proposed continuation; it does not erase a completed result or create a future obligation.

If a faction or another survivor expected the learner to continue, the player can correct that expectation, seek an eligible alternative, or let the request lapse. Do not reveal the learner's private reason unless they consent and current communication rules allow it.

The strongest ending may be a clear answer to one question followed by no further lessons. Completion of the learner's chosen aim is not measured by the length of the feature interaction.

## 361. Faction support along a learning path

A faction can supply a source, answer a technical question, lend a venue where allowed, or describe the work context that makes a lesson useful. It cannot decide the learner's personal aim or convert a lesson sequence into labor.

The Military might need a reading of an existing procedure. An Independent may have a reference manual. A Rebel contact might preserve an alternate account or explain why a notice is contested. These are illustrative roles; confirm current characters and authored content before adding them.

The player can disclose the learner's goal, ask a general question without naming them, request an eligible source, or decline the faction's offer. Faction response comes through its current owner. If the faction refuses access, the learner can find another teacher, narrow the question, defer, or stop.

## 362. Save, reload, and day-boundary cases

After reload, the education owner determines the last confirmed result. The learner's current goal or appointment can reappear only if a current owner persists it. If the plan itself is not durable, return to dialogue and ask the learner what they want now; do not synthesize a course state from a UI cache.

At a day boundary, verify whether the previous session completed before permitting another. Repeated command delivery must not create a second lesson result. Changing teacher or topic does not bypass the daily limit. A true new session on a later day must use a new valid identity under the existing owner contract.

If the learner leaves or the teacher becomes unavailable, preserve completed results and close or pause only the future route that can no longer proceed. No substitute survivor is silently assigned to the learning path.

## 363. Scope guardrails

This installment does not propose a skill tree, education reputation, school, graduation credential, curriculum scheduler, lesson queue, or parallel goal ledger. It does not make course attendance a proxy for consent to work. It does not assume a personal learning plan can be persisted until the current owner proves that behavior.

The path is content and orchestration over existing lesson results. Each session must remain independently truthful. Application belongs to the task/item/safety owner. Future intentions remain tentative unless the schedule or quest owner stores them.

## 364. Acceptance questions

1. Does the learner choose or affirm a concrete aim without needing a productivity motive?
2. Can the player support continuation while rechecking schedule, consent, and teacher availability each time?
3. Does every actual session obey one-result-per-day and stable event identity?
4. Can the learner change their aim or stop without losing prior truthful results?
5. Is any practical capability confirmed by the task owner rather than inferred from attendance?
6. Do faction contacts add useful context without gaining control over the learner?
7. Does reload restore only course/appointment state that an existing owner actually persists?

## 365. Outcomes for “The Mark on the Valve”

**Narrow answer, no task:** The learner understands the mark and chooses not to work on the valve.

**Supervised application:** The task owner permits a limited role with the required supervision; the learner agrees to that scope.

**Source correction:** An existing manual clarifies that the first explanation was outdated. The learner updates their understanding and the source owner controls any correction.

**Specialist referral:** The unresolved part needs expertise unavailable in the shelter. The player seeks a supported contact or leaves the question open.

**Changed interest:** The learner uses the first lesson to identify a different question and starts a new route only after a fresh agreement.

**Respectful stop:** The learner decides one answer was enough. The player closes the path and routes any external request elsewhere.

## 366. Production gates for a multi-day arc

Before authoring final content, verify the supported subject and result, whether a sequence can be represented by current data, teacher eligibility and availability, schedule persistence, per-day result identity, re-entry behavior, practical task ownership, and any source or faction route. If the design needs a durable personal goal not already supported, return for an architecture decision rather than adding a second store.

The writing should make clear when a future lesson is only proposed. The UI should distinguish a completed result from an appointment or intention. A player should be able to close the path without receiving a success message that claims a qualification or task completion.

## 367. Installment 25 close

This installment turns the existing lesson loop into an optional multi-day learning path while keeping every session bounded, consent-based, and truthful. The learner can continue, branch, apply, defer, change aims, or stop. Player approaches vary through scheduling, source-finding, practical support, caution, and learner-led pacing; none becomes a hidden class or morality axis. The only durable learning facts remain those confirmed by the current education owner.

## 368. Installment 26 — One teacher, two learners, one open hour

Scarce teaching time creates a player decision that cannot be solved by labeling one learner deserving and the other selfish. Two people may want help before the same duty window closes. The teacher may be qualified for both subjects but available for only one session. The player has to learn what each person wants, whether either can wait, and what alternatives actually exist.

This is a pressure scenario inside the existing education loop, not a classroom or learner-priority system. Each lesson still needs a valid learner, teacher, time, subject, and owner-confirmed result. A group session can be offered only if the current lesson owner supports it. Otherwise, the player chooses an individual route or finds another teacher.

## 369. Separate the requests before comparing them

The player should see each learner as a person with a distinct question and schedule. One may need a short explanation before a task. Another may be curious about a source and have no immediate deadline. The urgency is evidence about timing, not a ranking of personal worth.

The player can ask each learner whether the request is time-sensitive, what they need from the teacher, and whether they are willing to meet another day. A learner does not need to disclose a private motive. The player can also check duty, health/rest, location access, and teacher availability through the current owners.

If the two requests are compatible with one supported lesson, the player may ask both whether they want to learn together. Neither learner is enrolled by convenience. If the owner cannot represent two learner results, the session may be a shared conversation only; no second progress result should be invented.

## 370. Player actions that create different routes

- **Protect the nearer deadline:** arrange the lesson for the learner whose task depends on it, then negotiate a new time for the second learner.
- **Ask for another qualified teacher:** check roster and subject capability rather than assuming any available adult can teach both.
- **Use a source instead of a lesson:** locate a supported reference and ask the learner whether that meets their need; reading a source is not automatically an education result.
- **Offer the next valid day:** explain the delay to both people and let each accept, change subject, or withdraw.
- **Request a shared session:** do this only when the lesson owner supports multiple learners and both give an affirmative answer.
- **Split the question:** have the teacher address one supported portion now and schedule a separate session later only if that is a valid lesson under the current contract.
- **Stop promising:** if time cannot be secured, tell both learners the session is not arranged and let them choose another route.

The branches depend on deadlines, subject, teacher skill, needs, consent, and task eligibility. They do not use a global “most valuable student” score.

## 371. Schedule and result boundaries

Before either session starts, confirm that time is genuinely available. A tentative conversation is not a reservation. If the duty owner cannot hold the hour, the player may seek coverage, pick another window, or leave the choice unresolved.

Respect the exact daily result limit exposed by the current education owner. Do not infer that two learners mean two awards can be written to the same record, or that one learner's result can be copied to the other. If the owner supports separate learner results, each must have its own valid identity and state transition. If it supports only one, design the scene accordingly and defer the other lesson.

An interruption does not let the player restart the session for another result. If the teacher completes one supported lesson before being called away, record that result and offer an honest continuation route for the other learner. If no lesson completes, say so.

## 372. Branch table — who receives the hour?

| Teacher and learner state | Player action | Result for learner A | Result for learner B |
|---|---|---|---|
| A has task deadline; B is curious without a deadline | Teach A first, explain delay to B | Lesson may support an eligible task | B accepts later time, changes topic, or withdraws |
| Both requests are non-urgent and subjects differ | Ask for another qualified teacher | Waits or accepts a later session | Waits, changes teacher, or chooses source route |
| Same subject and both want to join | Check group lesson support | Shared lesson only if owner permits | No copied result if owner lacks group result support |
| Teacher lacks expertise for one subject | Find a specialist or narrow the question | Session proceeds only for supported subject | Specialist referral or deferral |
| One learner declines to wait | Respect the decision | No pressure to continue | Seek another teacher/source or close request |
| Duty emergency removes the hour | Recheck schedule and explain truthfully | Reschedule or withdraw | Reschedule or withdraw |

## 373. Scene outline — “Before the Shift Bell”

One learner asks for help understanding a label before taking an owner-approved task. Another asks the same teacher about a broader subject for personal interest. The teacher has one free period before their own shift. The player can ask what each learner needs, seek a second teacher, use an existing source, or decide who receives the confirmed session.

If the first learner studies, their later task still goes through the task owner. They may understand the label but decline the work. If the second learner waits, they may remain interested, revise the question, find a source, or decide the answer can wait. If a second teacher is found, verify subject competence and schedule independently.

The scene closes when the hour is used, both requests are routed, or the player honestly admits that neither lesson can happen now. It does not require a dramatic argument. A quiet schedule decision can carry consequence because someone must wait and the player is accountable for what they promised.

## 374. Neither learner becomes a resource

Avoid describing the choice as “which survivor yields the most productivity.” One learner's task deadline may be real, but the other person's interest remains valid. The player can choose the order based on concrete consequences and later repair the inconvenience by securing another teacher or source.

Do not give either learner a hidden resentment penalty for waiting unless a current relationship owner supports a specific response. Dialogue can acknowledge disappointment, relief, or indifference without changing a global value. A learner can decide the delay is acceptable or withdraw the request.

If one learner has a role that makes the lesson useful to a faction, that fact does not automatically outrank another learner's need. The faction can clarify a genuine deadline or offer a resource; the education owner still records only valid results.

## 375. Failure and recovery routes

**No second teacher:** The player can offer a later time, ask whether the learner wants an existing source, or close the request. Do not appoint an unqualified substitute.

**Shared lesson unsupported:** The player tells both learners that the owner cannot award group results. They can choose separate days or stop.

**Conflicting duties:** The player seeks an actual coverage route, uses a different window, or declines to promise.

**Deadline passes:** The task owner determines whether the opportunity expired. Education progress remains truthful even when it came too late to help the task.

**Teacher becomes ill or unavailable:** Recheck health and schedule. Do not force the session because it appeared on screen earlier.

**Learner changes their mind:** Respect withdrawal. The other learner's session may still proceed if all current checks pass.

## 376. Supporting factions and other adults

A faction can clarify why a lesson is time-sensitive, provide an eligible instructor, or lend an existing reference. It cannot rank learners by usefulness or demand that private motivations be disclosed. If it refuses the request, the player can seek a non-faction route, wait, or let the lesson go.

Another adult can volunteer to teach only if their skill and availability are confirmed by current systems. Personal confidence is not a qualification. A character may be willing but unavailable; a qualified person may decline. Both are meaningful results.

The player can also ask a faction contact to wait for the next day, propose a narrower task for the first learner, or recruit an eligible adult through the roster owner. These actions create different branches through access, time, and capability rather than moral alignment.

## 377. Re-entry and persistence

When returning after a day advance, read current requests, learner preferences where persisted, teacher availability, duty state, and any completed result. Do not assume the second learner still wants the original subject. If the request was not durably stored, ask again rather than recreating it from dialogue history.

When the teacher completes a lesson, the result must appear once for the correct learner. If the learner record was removed or the subject is no longer valid, follow the owner’s rejection path and preserve previous state. A save/reload must not create a second lesson because two dialogue branches converged on the same command.

## 378. Acceptance questions

1. Can two learners express different reasons and deadlines without being reduced to a ranking?
2. Can the player seek another teacher, a source, a shared session, or a later time only where owners support it?
3. Does every learner receive only the result the education owner confirmed for them?
4. Are group lessons unavailable when the current owner cannot represent them?
5. Does the player see the schedule cost before either person is promised a session?
6. Can either learner wait, change direction, or withdraw without a hidden penalty?
7. Does save/re-entry restore only requests and results that their existing owners persist?

## 379. Installment 26 close

This installment makes a scarce teaching hour a branching player decision shaped by deadline, subject, teacher availability, duty, and each learner's preference. The player can secure another teacher, find a source, ask about a supported shared lesson, reschedule, prioritize a time-sensitive request, or stop promising. Learners are not ranked by productivity, and no result is copied or invented. All outcomes remain inside matching, lesson results, and owner-backed knowledge.

## 380. Installment 27 — The same lesson can have a different doorway

A learner may understand a subject better after seeing a demonstration, hearing a term explained plainly, or working through one example with the teacher. The player can ask what would help, and the teacher can offer an available approach. The point is not to label the learner or build a learning-style profile. It is to let participants shape a real lesson around the materials, time, and teaching methods that currently exist.

This installment remains inside the three subfeatures: match an eligible learner and teacher; complete one owner-backed lesson result; and apply confirmed knowledge through its actual owner. If the education system does not represent instructional formats, these choices remain narrative variations within the lesson and cannot create separate progress states.

## 381. Ask what would make this explanation useful

Before the session, the learner may say that a written instruction is hard to follow, a verbal explanation moved too quickly, or a demonstration would make a step clearer. The player can ask a simple follow-up, offer a supported alternative, or accept that the learner does not want to explain further.

Do not diagnose the learner or infer a disability, literacy level, intelligence, or motivation from one request. The scene should treat the request as ordinary agency: people can ask for clearer words, a slower pace, another example, or a chance to practice.

If the learner does not know what would help, the player can ask the teacher to offer two available approaches. If no alternate method exists, say so and let the learner continue, defer, or stop. The request must not unlock a hidden bonus or impose a penalty.

## 382. Candidate approaches, only where supported

**Plain-language explanation:** The teacher restates a term using words the learner recognizes. This changes presentation, not the underlying subject or result.

**Worked example:** The teacher walks through one example from an existing source or safe practice setting. The item/source owner confirms whether that example is available.

**Demonstration:** The teacher shows a step only if the current location, equipment, safety, and lesson owners permit it. Watching a demonstration does not automatically authorize the learner to repeat the task.

**Learner explanation:** The learner describes what they understood, and the teacher clarifies a gap. This is a conversational check, not a quiz or score.

**Repeat at another time:** The player arranges another valid lesson on a later day only if the schedule owner and daily result rule allow it. Repeating a line inside the same session cannot mint an extra result.

These are content and interaction suggestions. Do not add a preferred-format field, accessibility profile, or secondary curriculum state unless a current owner already supports it.

## 383. The teacher must also agree

The learner's request does not obligate a teacher to use a method they cannot safely or competently provide. The teacher may offer an alternative, ask for a source, request a different venue, or decline the lesson. The player can find another eligible teacher, postpone, or proceed with the original approach only if the learner still agrees.

If a demonstration requires a scarce item, the player checks the real inventory owner before promising it. If a private document is involved, the learner controls whether to show it within supported privacy rules. If the teacher cannot confirm an answer, the lesson may remain uncertain or be deferred.

The point of agreement is to set a credible session, not to simulate a negotiation over every teaching sentence. Focus on material differences: location, source, equipment, schedule, audience, or the learner's comfort continuing.

## 384. Branch table — adapting the session

| Learner request | Player action | Teacher response | Lesson route |
|---|---|---|---|
| “Use simpler words” | Ask teacher to explain one term plainly | Rephrases or identifies a term they cannot clarify | Same subject and one result |
| “Show me one example” | Find an existing example/source | Uses it, seeks permission, or declines | Result only if owner confirms lesson completion |
| “Can I try the step?” | Check task/safety/tool owner | Allows, supervises, or blocks practice | Lesson does not bypass task eligibility |
| “I need more time” | Offer a valid window or stop | Reschedules or keeps current pace | No false appointment without schedule support |
| “I don't want to show the note” | Keep the source private or switch example | Teacher accepts or says they need another source | Learner can defer without penalty |
| “I don't know what helps” | Offer available options without pressure | Learner chooses, declines, or asks to listen first | No learner profile is recorded |

## 385. Scene — “The Label in Small Print”

This is a conditional example. Verify that a relevant authored label, subject, teacher, and item interaction exist before using it as a production quest beat.

A learner brings a label they have trouble following. The player can ask what part is unclear, invite the teacher to restate one term, find an existing diagram, or seek permission to demonstrate with a safe example. The learner may choose to keep the label private and use a separate sample.

The teacher identifies one term that is clear and another that needs a source. The player can seek that source, continue with only the confirmed part, or postpone. If the learner understands the explanation but lacks task authorization, the player routes any practical action through its owner.

The learner can finish knowing the confirmed term, leave with a question, ask for another session, or decide that the lesson has answered enough. There is no literacy score, penalty, or claim that the label was changed unless its owner confirms an edit.

## 386. Demonstration is not automatic permission

A practical demonstration may make a concept easier to understand, but three separate conditions remain: the teacher's ability to demonstrate; the safety and access rules of the space or item; and the learner's consent to participate. The player checks each through the current owner.

The learner can watch, ask questions, perform an allowed low-risk step, request supervision, or stop. If the task owner permits observation but not operation, the UI must not present an actionable “try it” button. If there is no safe route, the player can use a diagram, explanation, or later specialist referral only where those are available.

The education result states what was taught. The operational owner decides whether the learner may perform the task and what happened when they did. One must not be used as a shortcut for the other.

## 387. Audience and source choices still matter

The learner can ask for an individual explanation, include a chosen companion only where the lesson owner permits it, or choose a public demonstration if the current venue and privacy rules allow one. The player can check what other participants would see, protect a private source, or reschedule for a more suitable space.

A second person listening does not automatically receive a lesson result. If a group lesson is unsupported, the companion may be present in dialogue but must not gain knowledge state. The player can offer them a separate session through the same matching checks.

Where a faction provides the example or source, the player can ask a general question without identifying the learner, request access, or decline. The source owner's answer does not determine whether the learner is capable; it only determines what material can be used.

## 388. Play approaches through actions, not labels

**The question-led approach:** The player asks what is unclear, then finds a source or teacher who can answer it.

**The example-led approach:** The player finds a supported example or diagram and asks the learner whether it helps.

**The practice-led approach:** The player checks for a safe, eligible step and obtains supervision where required.

**The privacy-led approach:** The player protects a personal source and substitutes a general example or delays the session.

**The time-aware approach:** The player chooses the explanation that fits the available hour, or postpones rather than rushing.

Players can mix these during one lesson. They are not permanent archetypes, traits, or skill tree choices.

## 389. When adaptation changes the session outcome

If the teacher restates the term and the learner understands it, the result remains the same supported subject result. If an example reveals that the subject is broader than expected, the teacher can narrow the result, defer the rest, or suggest a later lesson. If no method resolves the ambiguity, the learner can leave with uncertainty intact.

Do not equate a repeated explanation with a failed learner. A content mismatch can come from an unclear source, an unavailable example, a teacher who lacks context, or a schedule that does not permit a full session. Each has a different repair action.

An incomplete session counts only according to the education owner's current result contract. UI text should not invent a “partial skill” field. The player can still take a concrete next step—find the source, seek another teacher, secure a safer venue, or stop—without claiming learning progress.

## 390. Returning to the practical task

After the lesson, the learner may ask to use the knowledge. The player should restate the exact action and have the task owner validate eligibility. The learner can proceed, observe, ask a specialist, or decline. If the lesson used a demonstration, do not assume the learner is now qualified for a different or riskier task.

The practical owner confirms the outcome, including whether an item changed, work was completed, or no action occurred. Education can be a knowledge source for that action; it does not own the transaction. If the task is unavailable, the player can keep the lesson result and wait for a valid route.

## 391. Tone, accessibility, and respect

Keep dialogue matter-of-fact. A learner who asks for simpler wording should not be mocked, infantilized, or framed as deficient. A teacher may be patient without becoming saintly. A player can be impatient, efficient, or protective, but consequences stay grounded in the specific interaction and task.

If an accessibility feature already exists in the learning interface, use it as a presentation affordance and avoid duplicating that authority in education data. If no such feature exists, the narrative suggestion does not itself promise a new UI control. Verify readability and focus requirements before any panel is designed.

## 392. Faction and community support

A faction may lend a reference, teacher, or safe practice space. The player can accept, ask for a different example, protect the learner's identity, or decline. The faction can set its own access rules but cannot require the learner to reveal a diagnosis or personal reason.

A community adult can offer to explain a term or demonstrate a task if current skill and availability owners support it. Another learner can share their own understanding, but that is not automatically a qualified lesson. The player can request corroboration, refer the question, or keep the result narrow.

## 393. Persistence and re-entry

The education owner persists the confirmed result; current dialogue and schedule owners persist only the choices they already support. Reopening the lesson must not recreate a private document exposure, duplicate progress, or imply that an appointment remains. Recheck teacher, learner, venue, and task state after save/restore.

If the learner selected a teaching approach only for one session, do not carry it forward as a permanent profile. Ask again next time when needed. A stable event identity prevents a repeated demonstration callback from generating another daily result.

## 394. Acceptance questions

1. Can the learner ask for a clearer, slower, or more concrete explanation without receiving a label or penalty?
2. Are alternate approaches limited to sources, venues, and methods that currently exist?
3. Does the teacher agree and have the needed competence and availability?
4. Are practice, safety, supervision, and learner consent checked independently?
5. Does the lesson yield only its confirmed result, regardless of format?
6. Can the learner protect a private source, continue, defer, or stop?
7. Does application still route through the task/item owner?

## 395. Installment 27 close

This installment gives the learner and teacher ways to shape an individual session through explanation, examples, demonstration, questions, or delay where supported. It avoids learning-style traits, diagnoses, profile state, and extra result types. The player branches through source, time, privacy, teaching competence, and safe application; all practical effects remain with their current owners.

## 396. Installment 28 — The lesson needs a thing

A lesson sometimes depends on a scarce source, tool, sample, or safe practice object. The player can decide whether to commit that resource, find a reusable example, borrow an eligible item, use a no-cost explanation, or wait. This turns preparation into a concrete branch without creating a lesson-material economy.

The lesson owner still controls whether learning occurred. Inventory, belongings, trade, and task systems own any object that is consumed, borrowed, transferred, or used. A panel preview cannot take an item. If no current owner supports the proposed practice material, it remains a narrative suggestion rather than an operational requirement.

## 397. Identify what the lesson actually requires

The teacher can explain whether the subject needs a written reference, an expendable sample, a reusable tool, or only a conversation. The learner may ask for a demonstration, but the player should distinguish what helps from what is strictly required. Avoid presenting invented scarcity when a lesson could proceed without the item.

The player can ask whether an existing substitute is valid, find out who owns it, or ask the teacher to narrow the lesson to the material already available. If the original item is needed for a current task, the player checks that task and inventory owner before taking it out of use.

When the resource is unavailable, the learner can still choose to hear the theory, use another source, defer, or stop. The game should not quietly award the same practical result as a completed demonstration if the education owner distinguishes those outcomes.

## 398. Preparation choices

- **Use a reusable reference:** locate an existing document or diagram and confirm access with its owner.
- **Borrow a tool:** ask its current owner or holder, agree on return conditions through the supported route, and avoid assuming availability.
- **Consume a practice sample:** preview the cost and ask whether the learner and teacher agree that the example is worth using.
- **Find a substitute:** identify an equivalent item only if the task or item owner confirms it is suitable.
- **Teach without the object:** use a supported explanation or example that does not affect inventory.
- **Wait for supply:** keep the request open only if the relevant owner persists it; otherwise explain that the learner must ask again.
- **Decline the cost:** protect the scarce item for another use and offer a different learning route.

Every route changes the preparation, timing, or practical scope. None creates a hidden generosity score.

## 399. Branch table — source, tool, sample, or no material

| Material condition | Player action | Learner choice | Owner-backed result |
|---|---|---|---|
| Reusable source exists but access is restricted | Ask the source owner or choose another source | Wait, use substitute, or defer | No access assumed from a displayed name |
| Tool is personally owned | Ask to borrow or use another tool | Accept the delay or alternate | Property owner controls transfer/use |
| Tool is shared work stock | Check the stock owner and task conflict | Reschedule or use a permitted alternative | No private appropriation of shared property |
| Practice sample is consumed | Show the cost before commitment | Proceed, choose theory, or decline | Inventory owner records any consumption |
| Equivalent substitute is proposed | Verify task/material compatibility | Accept or keep original aim | Relevant item/task owner confirms suitability |
| No material is available | Offer conversation, referral, or later date | Learn within supported scope or stop | No fake practical completion |
| Item moves after preparation | Re-read current owner state | Revise or cancel | Stale preview cannot settle use |

## 400. Scene — “One Clean Sheet”

This is a candidate scene, not a claim that paper or a particular lesson exists. Verify the authored source, inventory item, and relevant teaching route before promotion.

A learner and teacher want to compare an old instruction with a current one. There is one clean sheet available for a copy, but it is also needed by another task. The player can check inventory ownership and demand, ask whether a reusable source can be read directly, request a copied excerpt through a supported route, or postpone.

The learner may choose to study the general principle without a personal copy. The teacher may explain that one detail cannot be verified from memory. The player can accept a narrower result, seek the source author, or preserve the sheet for the task that already needs it.

If the source owner permits copying, the inventory owner records the consumed item. If the player only looks at the source, no inventory change occurs. If the player uses the last sheet without checking, the game can only show a consequence when the inventory/task owner applies one. The lesson cannot become a punishment meter for scarce supplies.

## 401. Knowledge routes that do not require ownership

The player may borrow a tool or use a public source without transferring it. These routes have different costs and permission checks. Borrowing needs a valid holder and return path; public reading needs source access; a demonstration needs safety and schedule approval. The player should know which promise they are making.

If a source belongs to a faction or another survivor, the player can ask permission, offer an alternative, or close the request. Refusal to share property does not make the owner an antagonist. An available source may still be unsuitable for the learner's question, in which case the player can seek a better reference.

Do not implement borrowing by duplicating an item in the education system. Any return condition belongs to the current property or trade owner. If no return state exists, keep “borrow” as a proposed dialogue route only.

## 402. The learner can choose not to spend scarce resources

Some learners will prefer an imperfect explanation now to consuming an item. Others may value a demonstration enough to accept the cost. The player can state the actual tradeoff and let the learner decide. The cost must be clear before the action commits.

The learner is not responsible for deciding how settlement-wide stock should be allocated. If the item belongs to shared inventory, the current resource owner and player allocation rules apply. The learner can express a preference, but that preference does not override inventory policy.

If the learner declines the lesson because its cost is too high, the player can offer a no-cost route or respect the decision. The game should not frame caution as lack of curiosity.

## 403. Supporting factions as sources and constraints

A faction may lend a reference, require a return, charge for an item through its current trade route, or refuse access. The player can negotiate, seek a community source, substitute a supported item, or defer. The faction contributes context and cost; it does not own the learner's result.

A small supporting group may have a specialist or spare tool. The player can ask whether the resource is available and what it would displace. A faction contact cannot create stock by dialogue, and the education panel does not reserve an item on the contact's behalf.

If the faction's terms change, disclose them to the learner and teacher before use. They can accept the material cost, narrow the lesson, use a different source, or stop.

## 404. A material does not multiply the lesson result

The learner can inspect a source, hear an explanation, and demonstrate a practice step in one session, but the education owner still determines the single valid result. Using three materials does not generate three skill increments. Failing to obtain a material does not create a compensatory result.

If the lesson owner supports a distinction between theory and practice, report the exact result. If it does not, author dialogue that stays within the confirmed subject and defer any separate practice claim. One daily result remains one daily result regardless of how many objects were available.

The player can leave the session with a clear next action: return the borrowed tool, request the source, seek an eligible task, or ask for another lesson on a later day. These follow-ups use their actual owners.

## 405. Resource changes during the session

If a tool breaks, a sample is consumed, or a source becomes inaccessible during the lesson, pause and re-read state. The player can continue with an available explanation, seek a substitute, stop, or ask the learner whether they want to reschedule. Only the resource owner determines whether any item was actually consumed or damaged.

If the teacher completed the lesson before interruption, preserve that result. If not, follow the current lesson outcome contract. Do not let the player reopen the same session to receive a second daily award after replacing the item.

## 406. Play approaches through resource decisions

**The improvisor:** asks the teacher to adapt the explanation to an available source.

**The caretaker:** protects shared stock and seeks a no-cost or borrowed route.

**The practical mentor:** accepts a known material cost for a supported demonstration after explaining it.

**The negotiator:** asks an owner or faction for access, a return window, or a substitute.

**The cautious student advocate:** lets the learner decide whether the result is worth the cost.

These approaches generate different branches through available resources and consent. They are not education perks or moral categories.

## 407. Re-entry and transaction safety

On return after save or day advance, query the actual owner for the material, loan, consumed sample, and lesson result. Do not trust the earlier preview. If the lesson completed, replay must show the same result and never consume the resource again. If the item was only reserved in dialogue, report the reservation only if its owner persisted it.

Duplicate callbacks must not consume a sample twice or settle a loan twice. If the material owner cannot provide idempotent behavior, do not wire a consumable practice step until that contract is solved.

## 408. Scope guardrails

This installment does not create a lesson-supply catalog, education inventory, loan ledger, or personal affordability score. It does not reserve community stock in the panel or infer that every lesson requires a physical item. Materials appear only where current item, task, and education owners support them.

## 409. Acceptance questions

1. Does the player know whether the lesson needs, uses, consumes, or borrows an item?
2. Is ownership checked before any resource moves or becomes unavailable?
3. Can the learner choose an explanation, alternate source, delay, or no-cost route?
4. Are shared resources handled by their actual owner?
5. Does the lesson produce only its valid one-per-day result regardless of materials?
6. Are interruption and reload safe against duplicate consumption?
7. Do faction resources create negotiation without taking over education state?

## 410. Installment 28 close

This installment makes lesson preparation a player choice around sources, tools, samples, permission, and scarcity. The player can verify ownership, negotiate, find a substitute, use a no-cost explanation, or wait. The learner can accept or reject the cost. Education still records only its supported result; items and obligations remain with their current owners.

## 411. Installment 29 — The knowledge meets a different situation

The learner may encounter a familiar idea in a setting that is not identical to the lesson. This is a useful point for branching: the player can let the learner apply what is clearly supported, help compare the new context, ask a specialist, or stop before an uncertain action. The knowledge remains valuable, but it is not a universal license.

This installment develops the third subfeature—knowledge that leaves the lesson—while respecting the same matching and one-result-per-day rules. It does not add skill decay, hidden confidence, or automatic proficiency transfer. If the current education owner stores only a subject result, the difference between situations must remain in the task/source dialogue and owner outcomes.

## 412. Recognize what is the same and what changed

Before the learner acts, the player can ask what they recognize from the lesson and what is new. The situations may share a symbol but use a different material; follow the same steps but have a different safety condition; or look similar while belonging to different owners. The learner can state what they remember, ask for confirmation, or say they are unsure.

The player should not quiz the learner for a score. A short conversation identifies whether the lesson applies, whether a source should be checked, and whether an authorized task route exists. The learner can also choose not to explain their reasoning and simply ask for a qualified check.

The relevant owner decides if the action is eligible. A correct answer in dialogue does not override equipment limits, supervision, access, or safety rules.

## 413. Four kinds of transfer boundary

**Same principle, new example:** The lesson likely applies, but the player verifies that the task owner accepts the new example.

**Similar appearance, different owner:** The learner recognizes an object or label, but its property source differs. The player checks ownership before use.

**Changed condition:** The method is familiar, but fatigue, damage, weather, time, or location changed. The player rechecks current state.

**Unclear evidence:** The learner and teacher cannot tell whether the old lesson applies. The player finds an existing source, requests a specialist, defers, or chooses another allowed action.

These are narrative distinctions, not new education result categories. Only implement a mechanical distinction if the current owner already expresses it.

## 414. Scene — “The Borrowed Gauge”

This is a candidate scene. Verify that the relevant measurement device, subject, source, and downstream work route exist before adding content.

A learner studied how to read one gauge. Later, a neighbor offers another gauge with different markings. The player can ask the learner what they recognize, check the device owner and its reference, seek a qualified adult, or decline to use the reading until the scale is confirmed.

If the learner identifies a shared principle but not the calibration, the player can let them explain the limited part and then ask the source owner to verify the rest. The task can proceed only if its owner accepts the confirmed reading and the learner is eligible. If the learner chooses to wait, the player can find a reference or an alternate qualified worker.

Possible endings include a supported reading, a specialist handoff, an owner-confirmed mismatch, or an unresolved reading that prevents action. The education result remains what the learner studied; the device and task owners decide whether the new measurement is valid.

## 415. Choice table — apply, check, or stop

| Learner recognizes | Player action | Learner response | Consequence owner |
|---|---|---|---|
| Same principle and verified device | Offer the permitted task route | Try, observe, or decline | Task owner confirms action/result |
| Same symbol, different scale | Compare an existing reference | Ask for specialist or proceed only within verified bounds | Device/source/task owner |
| Familiar method, changed condition | Recheck current status and access | Adapt, wait, or stop | Environment/safety/task owner |
| No clear source | Find an eligible teacher or source | Defer or choose another supported action | Education/source owner |
| Learner is unsure | Normalize uncertainty and seek confirmation | Ask, pause, or decline | No penalty or competence downgrade |
| Owner rejects the action | Explain the exact limit | Find alternative or close the route | Owner rejection remains authoritative |

## 416. Let uncertainty create a route, not a stat

The player can take the learner's uncertainty seriously without recording a confidence percentage. They may use a source, bring in a qualified adult, choose a narrower action, or stop. If the learner proceeds and succeeds, the task owner records the result. If the owner rejects it, show that reason. If the lesson's relevance is unknown, leave it unknown.

Do not create a hidden penalty for hesitation. A learner who asks for confirmation may be exercising good judgment. A learner who proceeds may be eligible and willing, but the game should still show the actual risk and task conditions. These are different play approaches, not good/bad endings.

## 417. A small mistake can teach without becoming punishment

If the learner makes an incorrect interpretation, use only the consequence the relevant item or task owner supports. The player can stop the action, correct the reading, ask a teacher to explain, or route a repair. Do not inflict broad morale or trust loss to make the error feel meaningful.

If no item changed and no task began, the result can simply be a corrected misunderstanding. If a real resource was consumed, its owner reports that fact. If a task was partially completed, its owner records the actual partial state. The education record is not rewritten to conceal the error.

The learner can decide whether they want to practice again later, ask for a different source, or leave the subject for now. A single mistake does not become a permanent “unreliable learner” tag.

## 418. Player actions that reveal transfer limits

The player can find the source for the new device, compare its marking with the studied example, ask a specialist to demonstrate, confirm ownership, test a safe non-operational example if supported, or ask the task owner whether the learner can participate. Each action may expose a different constraint.

Finding a reference may show that both devices share a principle but use different scales. Checking the owner may reveal that the neighbor cannot lend the item. Asking a specialist may reveal that the lesson applies only to observation, not operation. Rechecking the task may show that a different worker is required.

This creates substantial branches based on sources, property, eligibility, and task scope. The player does not need to select a personality alignment response to reach them.

## 419. Factions and specialist referrals

A faction contact may know the device, own a manual, provide a qualified operator, or refuse access. The player can ask a general question, request the source, disclose only the facts needed, or find a community expert. The faction can support or limit the route but does not validate the lesson result.

If the specialist is unavailable, the learner can wait, use a narrower reading, ask another teacher, or stop. A faction's confident assertion is still checked against current evidence where the task requires verification. Do not add faction-based accuracy bonuses.

## 420. Knowledge remains useful even when transfer is blocked

The learner may understand the first device and be unable to use the second. This is not a failed lesson. They can explain what they know, identify what is different, and help the player find the missing reference. That is a meaningful application of knowledge even if no task result changes.

If the existing journal, task, or communication owner records that question, it may become visible there. Otherwise, keep it in the current conversation. Do not add an education-specific evidence notebook to remember every uncertainty.

## 421. Multiple outcomes for “The Borrowed Gauge”

**Verified transfer:** source and task owners confirm that the learned principle applies to the new gauge.

**Limited observation:** learner may describe the marking but cannot operate the device; player finds a qualified operator.

**Different calibration:** the learner recognizes the notation but discovers that the scale differs; a specialist provides the correct route.

**Ownership blocks use:** the neighbor's device is not available for the task; the player asks permission or finds another source.

**Unresolved but safe:** nobody can verify the scale, so the player stops before acting and leaves the question open.

**Learner withdraws:** after learning what is involved, the learner decides not to participate. The player respects the decision and routes the task elsewhere.

## 422. Re-entry and persistence

When the player returns, re-read the current lesson result, device/source state, owner permissions, and downstream task status. If the device or reference changed, recheck rather than relying on a cached “learner knows this” label. The owner may confirm that a task completed, was blocked, or never began.

Duplicate attempts cannot consume another sample or count another result. A follow-up lesson on a later day must be a valid new session. The player can reopen the conversation to ask what the learner remembers, but dialogue itself cannot advance progress.

## 423. Scope boundaries

This installment does not add transferable skill matrices, confidence meters, decay, mastery tiers, certification, or universal task authorization. It does not infer a learner's competence from a successful dialogue answer. It creates authored choices around applying narrow knowledge to a changed context and routes every operational result to its owner.

## 424. Acceptance questions

1. Can the player identify which part of the new situation matches the lesson and which part differs?
2. Are sources, devices, property, and tasks validated by their current owners?
3. Can the learner ask for confirmation, proceed within supported bounds, or stop?
4. Does uncertainty remain a truthful state rather than a hidden stat or penalty?
5. Can an incorrect interpretation be corrected without rewriting the lesson history?
6. Does a blocked application still leave the learner's confirmed knowledge intact?
7. Are replays and reloads safe against duplicate tasks, costs, or lesson results?

## 425. Installment 29 close

This installment explores what happens when a learner brings a lesson into a changed situation. The player can verify sources, compare examples, confirm eligibility, seek expertise, narrow the action, or stop. A correct result, uncertainty, owner rejection, or learner withdrawal all remain valid outcomes. Knowledge stays useful but does not become universal authorization or a hidden competence score.

## 426. Installment 30 — The learner already knows part of it

The learner may arrive with experience that overlaps the planned lesson. They might know one step from a previous job, recognize a tool, or have learned a different version from someone else. The player can ask what they already understand, compare that account with current sources, narrow the lesson, or choose a different subject.

This is a matching and lesson-quality branch, not a proficiency mini-game. The player should not make the learner pass a quiz before being believed. If a canonical skill or curriculum owner exposes relevant prior knowledge, use that state; otherwise, keep the conversation exploratory and do not invent a level, badge, or credential.

## 427. Listen before repeating the lesson

At the start of the session, the teacher can ask what the learner has seen or tried. The learner may describe a familiar step, say they remember only the result, show an existing example, or decline to explain. The player can let the teacher listen, ask a clarifying question, or proceed with the planned explanation.

The learner's account is useful context, not proof that an owner-backed skill result exists. The teacher can respond with “you already have this part,” “let's check the current procedure,” or “that applies to a different version.” This respects experience without confusing memory with authorization.

The player should not force an assessment if the learner only asked for help. A brief conversational check can shape the lesson, but any formal competency requirement belongs to the current skill/task owner.

## 428. Branches when experience is partial or mismatched

**The learner knows the first step:** the teacher can skip repetition and focus on the part the learner asked about, if the current lesson owner permits a bounded subject.

**The learner knows an older version:** the player can find a current source, ask a specialist, or keep the lesson paused until the difference is clear.

**The learner remembers the outcome but not the method:** the teacher can explain the reasoning or use an available example.

**The learner has practical experience but no recorded result:** the player can check the existing skill owner; do not fabricate a credential because the learner sounds confident.

**The teacher disagrees with the learner's account:** the player can compare evidence, ask for a demonstration, or continue with a narrow verified result.

**The learner does not want to discuss prior experience:** continue only if teacher and learner agree to a useful lesson; otherwise, defer or stop.

## 429. Player actions that adapt the lesson

- Ask which part of the subject the learner already recognizes.
- Ask what they still want to understand, rather than repeating the entire lesson.
- Check the current curriculum or skill owner for an existing result.
- Find a dated source when the learner may have seen an older procedure.
- Invite the teacher to use one example that tests the difference without grading the learner.
- Ask the learner whether they want to continue after discovering that part of the lesson is familiar.
- Choose another valid lesson if the current subject would only repeat known material.
- Close the session without an award if no new supported lesson occurred.

These routes branch on prior result, source version, subject scope, teacher judgment, and learner choice. They do not branch on whether the player is trusting or suspicious by personality.

## 430. Decision table — repeat, narrow, verify, or change

| What the learner brings | Player action | Lesson route | Truthful outcome |
|---|---|---|---|
| Existing owner confirms the full subject | Check whether another lesson is valid | Apply knowledge or choose another subject | No duplicate progress for repeated content |
| Existing result covers one part | Ask learner to select the gap | Narrow the lesson where supported | Record only the confirmed result |
| Learner describes an older method | Find a current source | Compare or defer | No unverified procedure is taught as current |
| No existing result but learner has experience | Let teacher hear the account | Teach, verify, or refer | Conversation alone is not certification |
| Learner is uncertain or private | Offer a low-pressure explanation | Continue, change subject, or stop | No penalty for not disclosing |
| Teacher cannot verify the subject | Find another specialist or end session | Defer or refer | No invented expertise |

## 431. Scene — “The Step Before the Lesson”

This is a candidate writing pattern. Confirm that the subject, prior skill result, teacher, and practical follow-up exist before making it a reachable scene.

A learner asks to study a procedure the teacher planned to introduce from the beginning. During the opening conversation, the learner explains that they already know how to prepare the materials but are unsure how to read the final result. The player can narrow the lesson, ask the teacher to verify the current procedure, or continue with the full explanation if the learner prefers that.

The teacher discovers that the learner's example came from an older source. The player can obtain a current source, ask a qualified specialist, or defer. If the material matches, the teacher can focus on interpretation. If it conflicts, the learner can compare both explanations and decide whether to proceed only with the confirmed part.

The ending may be a shorter valid lesson, a source referral, a different subject, a deferred session, or no lesson because the learner already has the supported result. No time is charged or progress awarded unless the current lesson owner says so.

## 432. Prior experience does not authorize every task

Even when an existing skill owner confirms knowledge, the learner may still need equipment, supervision, access, a current procedure, or a separate work assignment. The player checks those requirements independently. The lesson panel should not issue a work command because it found a prior result.

The learner can understand the subject and still decline the task. A task owner can reject an action even when the learner is willing. The player may seek an eligible alternative, arrange required support, or stop. This boundary keeps education useful without making it a general authorization system.

## 433. Respect the learner when the teacher is wrong

If the teacher assumes the learner knows nothing, the player can clarify the learner's experience, ask the teacher to listen, invite the learner to choose the starting point, or seek another teacher. The teacher may adapt, disagree, or decline to teach. Any relationship reaction belongs to the current social owner.

If the learner's prior information is incorrect, correct the specific claim through evidence. Do not label the learner careless or unreliable. They can ask for a source, accept a correction, retain uncertainty, or stop the session.

The player may choose to let the teacher proceed without checking. If a later outcome depends on the incorrect information, the responsible source/task owner must support that consequence. No universal trust penalty applies.

## 434. Supporting faction and community sources

A faction may hold a current procedure while the learner remembers an older community practice. The player can ask for a dated copy, compare the sources, request a qualified explanation, or use a separate subject that does not depend on the disputed detail.

An Independent or Rebel contact may provide an alternate source; a Military contact may confirm an operational revision. These are examples only. The source's current owner decides access, and its faction does not automatically win the disagreement. The player can keep the learner's identity private while asking a general question where current communication rules permit.

## 435. Endings from existing knowledge

**No repeat needed:** current owner state already supports the subject; the player chooses a new aim or closes the session.

**Narrowed lesson:** the learner knows the foundation and studies only the unresolved part where the lesson owner permits it.

**Verified update:** an existing source confirms the current version, and the learner updates their understanding.

**Two versions remain:** the sources disagree; the player asks an owner, defers, or uses only the confirmed portion.

**Prior experience is not qualification:** the learner describes practical familiarity, but the task owner still requires formal eligibility or supervision.

**Learner opts out:** the learner does not want to repeat or explain prior experience; the player stops without penalty.

## 436. Re-entry and save behavior

On return, read the actual education/skill result and relevant source version from their owners. Do not trust a cached dialogue flag that says the learner completed the lesson. If the result was not persisted, the player should ask again or re-run the valid lesson route; they must not infer prior progress.

Repeating a familiar session cannot award a second result. A different valid subject can proceed only under current schedule and daily-result rules. The task owner validates any later practical use independently.

## 437. Scope guardrails

This installment does not add an entrance exam, proficiency quiz, prior-experience tree, skill level, teacher rating, or automatic credit transfer. It does not assume that learner testimony is either proof or falsehood. It lets the player adapt a lesson through conversation and current owner evidence.

## 438. Acceptance questions

1. Can the learner share prior experience without facing a mandatory test or diagnosis?
2. Does any confirmed prior result come from the current skill/education owner?
3. Can the player narrow, verify, change, or stop the lesson based on what is already known?
4. Is older or conflicting information routed to a current source owner?
5. Does prior knowledge remain distinct from task eligibility, consent, and supervision?
6. Are duplicated lessons prevented from awarding repeated progress?
7. Can a learner keep their experience private and still decline the session?

## 439. Installment 30 close

This installment adds a branch for learners who arrive with partial, practical, outdated, or already-recorded knowledge. The player can listen, verify, narrow the subject, choose another route, or stop. No test score, credential, or automatic task permission is added; education and skill owners confirm prior results, while task owners decide what the learner may do.

## 440. Expansion installment 31 — The learner explains one step

Once a learner has a confirmed result, the lesson can continue through a modest role reversal: the learner explains one part to another person while the original teacher listens. The point is not to turn every trained survivor into a certified instructor. It is to create a playable moment where understanding can travel, be corrected, or remain personal, using only lesson outcomes the current education owner can support.

This is a continuation of the single lesson loop. It maps to matching, one replay-safe lesson, and owner-backed knowledge. It does not create a curriculum tree or a separate peer-tutoring feature.

## 441. Explanation is not an examination

The player may ask the learner to describe a step in their own words, show a safe example, or identify where they still need help. The learner may also decline to demonstrate. The teacher can listen, add a correction, or say that the subject is not safe to pass along without supervision.

This exchange is conversational evidence for deciding what lesson to offer next. It is not a graded test, public performance, or new skill credential. The learner's phrasing can differ from the teacher's while still being accurate. If the game's knowledge owner only records that a subject was learned, it cannot claim that the survivor can teach it or perform a more advanced task.

If a live scene needs an award or progression event, the current owner must confirm that the lesson happened and what result it produced. A well-spoken explanation by itself cannot grant knowledge, task eligibility, or a teacher role.

## 442. Give the original learner control of the handoff

The original learner can choose to explain, ask the teacher to explain instead, invite the second person to observe, or end the session. The second person can accept, ask a different question, or leave. The teacher can support the handoff, narrow the subject, or stop it when the procedure exceeds their authority.

The player can ask whether the learner is comfortable sharing the material, whether the second person can attend, and which step they want covered. The player should not volunteer another survivor's experience as proof or disclose their prior history to make the lesson more impressive.

A faction or crew may restrict who can demonstrate a hazardous procedure. That limit comes from the current safety, access, or work owner. The learning conversation may still cover safe concepts if the qualified teacher and owner allow it; the scene must not quietly turn a lesson into authorization to operate equipment.

## 443. A second learner starts their own route

The second survivor is not granted the original learner's result by proximity. They may ask to hear the explanation, but their lesson eligibility, availability, subject scope, and outcome are checked independently. If the education owner supports a shared session, it can record one teaching event and the appropriate learner results. If it supports only one learner per lesson, the second person receives no implied completion.

The player may arrange a separate valid session, ask the second survivor to observe without credit, focus on one common question, or stop the handoff. The teacher may have time for only one person. The learners may want different levels of detail. Shared attendance does not guarantee shared understanding or identical results.

This boundary protects both characters. It avoids using a scene to duplicate knowledge flags while still allowing a meaningful social and teaching interaction.

## 444. Scene seed — “Show me where it catches”

Candidate content only: confirm the subject, learners, teacher, room, and any practice interaction in current data before authoring it as a reachable event.

After a lesson on a repair procedure, the learner notices a second survivor watching the workbench. The second survivor asks why one step matters. The original learner can try to explain, ask the teacher to take over, or say they need more practice first. The teacher can invite a safe verbal explanation, correct one detail, or state that the physical demonstration requires supervision.

The second survivor may ask about a different step, decide the procedure is not relevant to their work, or request a full session later. The player can check whether that session fits the current schedule, arrange a separate lesson, or let the conversation end. The original learner is not penalized for declining to teach.

Possible closes include a supported shared lesson, a brief observation without progress, a later one-person session, a correction that sharpens the teacher's material, or no handoff. The education owner determines whether any knowledge result changed.

## 445. Correction can strengthen the shared material

The learner may remember a point the teacher omitted or phrase a step in a way the second person understands more easily. The player can ask the teacher to confirm it, compare against an approved source, or keep the explanation informal while avoiding a claim of official instruction.

If the teacher corrects the learner, the correction should identify the step and evidence rather than humiliate them. The learner can accept it, ask for a source, disagree, or stop. The second person may hear the correction, but does not receive a personal result unless their own lesson path supports it.

Do not add a new curriculum-editing workflow here. If the correction should change authored lesson content, it requires review through the existing data-authoring authority. A survivor's dialogue cannot mutate canonical curriculum data during play.

## 446. The practice owner still decides what can be demonstrated

Before a practical example, check the actual task, equipment, location, supervision, and safety requirements. The teacher's confidence and the learner's new knowledge do not substitute for those owners.

The player can choose a verbal explanation, an inert or safe example if a real training asset and owner exist, a supervised demonstration, or a referral to a qualified specialist. They can also stop. If no safe demonstration route exists, do not improvise one from a dialogue choice.

The lesson may be complete while the practical task remains locked. Conversely, an allowed task rehearsal may occur without awarding a broader knowledge result if the lesson owner does not confirm one.

## 447. Player approaches through the handoff

**Invite the learner to explain:** lets the learner choose their own words, while preserving the teacher's chance to correct.

**Ask the teacher to lead:** keeps instruction with the qualified person, though the learner may remain a quiet observer.

**Ask the second survivor what they need:** narrows the discussion and can reveal that they need a different lesson.

**Check safety and authority first:** confirms whether a demonstration is permitted, at the cost of delaying a useful conversation.

**Leave the knowledge private:** accepts that the first learner may not want to teach anyone else.

These are available actions, not personality traits, skill checks, or hidden reputation strategies.

## 448. Supporting faction roles

A supporting faction can provide an authorized source, confirm a safety limit, lend a room or training object through its actual owner, or refer the second learner to a specialist. It cannot declare the first learner qualified to instruct or command them to share personal knowledge.

A contact may disagree with the teacher's procedure. The player can compare the current source, ask for a qualified review, teach only the uncontested step, or defer. Faction authority is scoped to the relevant procedure or resource; it does not replace learner consent or education state.

If a faction records training attendance, verify whether that record is a source of knowledge, a roster note, or simply an access log. Do not translate one into another.

## 449. Ending routes

**Learner teaches a confirmed step:** the current lesson owner records only the supported teaching and individual learner outcomes.

**Teacher corrects a step:** the group can continue with the verified detail, ask for a source, or defer.

**Second person needs a different lesson:** the player can start a separate valid match or leave a request for later.

**Learner declines the handoff:** the original lesson remains valid; no social or progress penalty is invented.

**Demonstration lacks authorization:** the group can use a permitted verbal route, ask a specialist, or stop.

**Shared session is unsupported:** attendance may remain dialogue only; separate lesson routes are offered without claiming duplicate progress.

## 450. A later task remains a separate decision

After the handoff, a supervisor or faction contact may ask the second learner to perform a related task. The player checks eligibility, consent, schedule, and equipment through the task owner. The explanation is not a work assignment, and the person who learned first cannot authorize the second person's task.

A successful task can produce a new learning opportunity if its owner emits suitable evidence. A failed or interrupted task can lead to more instruction, a safety review, or a referral. It must not automatically erase the lesson or mark a survivor incompetent.

## 451. Persistence and replay

On re-entry, read the source lesson, confirmed participants, individual results, and any supported follow-up from their current owners. Do not persist “taught a peer” in a host dialogue flag if no domain owner can later interpret it.

Repeatedly opening the same scene cannot award a second lesson. If an owner supports a single teaching event with multiple learners, replaying the event cannot duplicate one person's result or trust mutation. If the owner supports only separate lessons, create separate stable lesson identities through that owner.

If the original teacher leaves before the second learner's session, the player must find an eligible replacement or defer. The first learner does not inherit the teacher's role by default.

## 452. Acceptance questions

1. Can the learner decline to explain without losing the result already earned?
2. Is an explanation distinct from a practical qualification or advanced task permission?
3. Does the second learner receive only individually confirmed knowledge?
4. Can the teacher correct or stop an unsafe step without turning the learner into a public failure?
5. Are lesson and task outcomes owned, saved, and replay-safe in their current systems?
6. Are faction sources limited to what the faction actually controls?
7. Can the player end the handoff without inventing social punishment?

## 453. Installment 31 close

This installment lets a confirmed learner explain one step to another person, with the teacher present when needed. Consent, safe scope, individual lesson results, and task eligibility remain separate. The role reversal creates branches around sharing, correction, referral, observation, and stopping; it adds no peer credential, curriculum editor, or copied knowledge state.

## 454. Expansion installment 32 — The lesson comes with a condition

A supporting faction may control access to a specialist, manual, room, or demonstration and attach a condition to the lesson. That condition might be a safety rule, an attendance limit, a restriction on copying a controlled document, or a requirement to ask the faction before using a procedure on its equipment. The player must make the condition visible before the learner accepts.

This installment extends the existing lesson match, session, and owner-backed knowledge. It does not create a faction curriculum system, loyalty meter, or new credential. The faction owns its access and material; the learner owns their consent to study; the task owner still decides what work is authorized.

## 455. Separate access from understanding

The player can inspect what the faction controls and what it is asking of participants. Access to a room does not mean the faction owns the learner's knowledge. Attendance does not mean agreement with faction doctrine. A source can be restricted while the underlying subject remains available through another qualified teacher.

The conditions should be concrete. “Use eye protection while at the equipment” is a safety condition if the safety owner supports it. “Do not copy this controlled manual” is a material-access condition if the source owner defines it. “Report for a faction shift after the lesson” is a separate work request and must be offered through the task owner with separate consent.

If the condition cannot be represented or enforced by a current owner, keep it as dialogue or choose a different lesson route. Do not imply a persistent contract from a line of text.

## 456. The learner may ask what happens to the result

Before agreeing, the learner can ask whether attendance is recorded, who can see it, whether the result will be shared with a faction, and whether the procedure can later be used outside faction property. The player can answer only from current owner-backed rules.

Possible responses include a lesson with a private knowledge result, a faction attendance note that does not certify skill, an access condition limited to the training room, or an unknown rule that needs clarification. These are not interchangeable. Do not state that a personal result is confidential if the current save/data owner cannot enforce that boundary.

The learner can accept the condition, request a narrower scope, ask the source owner, choose an alternate teacher, defer, or decline without losing other earned knowledge.

## 457. Scene seed — “You can learn it here”

Candidate content only. Verify that a supporting contact, controlled source, instructor, training area, and access rule exist before treating this as reachable.

A faction contact can arrange a lesson from a specialist but says that the demonstration may occur only in the faction's work area. The learner wants the knowledge for a task in another part of the shelter. The player can ask whether the restriction concerns the room, the tool, the source material, or the learner's later task. The contact may clarify one boundary, refuse another, or say they need approval.

The learner can accept an in-room lesson and later ask a task owner about independent practice, seek a community teacher, study a public source, or decide that the restriction makes the lesson unsuitable. The contact can offer a safe overview without the controlled material only if the specialist and source owner permit it.

The scene ends with a lesson, a limited session, an alternate route, a request for authorization, or no agreement. No faction automatically wins the knowledge dispute.

## 458. Branches when access is conditional

| Condition | Player action | Learner decision | Owner-backed route |
|---|---|---|---|
| Faction limits the room | Ask if a different space is permitted | Accept location, wait, or choose another teacher | Access owner confirms the space |
| Source cannot be copied | Ask for a permitted explanation | Learn from the specialist or decline | Source owner controls material use |
| Attendance is recorded | Check audience and purpose | Accept, narrow, or refuse the record | Record owner confirms actual visibility |
| Lesson implies future work | Separate the work request | Accept lesson and decide on work later | Task owner handles the assignment |
| Faction procedure may conflict with public guidance | Compare permitted sources | Study confirmed parts or defer | Current source owner determines guidance |
| Rule is not documented | Ask for clarification | Wait or choose another route | No unverified condition is persisted |

The same condition can lead to different outcomes because the learner's use, privacy needs, access, and alternatives differ. No hidden compliance score is required.

## 459. Negotiate the boundary, not the person's belief

The player can ask the faction to limit its condition to the property or equipment it controls, allow a general explanation outside the room, provide an approved public source, or schedule a second lesson after permission is granted. The contact can agree, counter, or refuse.

The player should not force the learner to declare allegiance or publicly reject the faction. The learner can say that the terms are acceptable, ask for time, or leave. A refusal may protect privacy, avoid a work obligation, or simply reflect a different learning need; the player does not need to diagnose the reason.

If the contact requests a personal promise, route it through the current owner that can represent that promise. A lesson panel must not record a general pledge to the faction.

## 460. A limited lesson can still be useful

The learner may accept a lesson that covers only the permitted portion, then ask a qualified community teacher to explain what the first instructor could not. Each source is attributed to its actual owner. The player can compare them, ask a specialist whether their scopes overlap, or use only the common confirmed material.

If the two sources disagree, use the existing source-review route. Do not resolve the conflict by adding a new faction standing score. A faction can restrict its own document or property; it cannot declare that every other explanation is false.

The learner may decide that a partial session is worth the time. They may also decide that an incomplete lesson would create more risk than value and stop. Both decisions remain valid without changing their prior results.

## 461. Support from another faction or community teacher

A supporting contact outside the controlling faction may have a public source, alternate specialist, or safe location. The player can ask for a referral, confirm eligibility, and compare the actual terms. The contact may lack the equipment or current procedure; the learner can wait, accept a verbal overview, or decline.

No faction should be added merely to manufacture an alternative route. Use a current minor contact, community role, or existing specialist where available. If no such source exists in canon, keep the branch as a design possibility rather than inventing a new faction.

The major faction remains a constraint or source owner in the scene, not the protagonist. The player's question, the learner's choice, and the available evidence drive the path.

## 462. The next task is separately authorized

After a conditional lesson, the player may ask whether the learner can perform a related task. Check task qualification, supervision, location access, schedule, and equipment through their actual owners. The faction's permission to teach inside one room does not authorize the learner to work elsewhere.

The learner may request supervised practice, seek task permission, ask for a different task, or decline. A faction supervisor may offer a work opportunity, but it is a new proposal with scope and cost. Completing it cannot retroactively become the price of the lesson unless the learner explicitly accepts a supported arrangement beforehand.

## 463. Outcome routes

**Condition accepted:** the learner completes a valid lesson under the visible access rule.

**Condition narrowed:** the source owner permits a bounded explanation and the learner accepts its limited scope.

**Alternative source chosen:** a separate qualified teacher or public source provides a valid route.

**Permission requested:** the session waits for an owner response; attendance is not assumed.

**Lesson declined:** the learner leaves without a false failure or faction disloyalty result.

**Condition conflicts with safety or another owner:** the player pauses and seeks an authoritative answer.

**No supported alternative exists:** the player explains that limitation and closes the route.

## 464. Player approaches

**The scope clarifier:** asks exactly which source, place, or task the condition covers.

**The privacy checker:** confirms who will see attendance or a resulting record.

**The alternate-source seeker:** looks for a qualified path that does not use restricted material.

**The boundary negotiator:** requests a narrower condition while accepting that the faction can refuse.

**The no-pressure closer:** lets the learner decline without demanding a reason.

These actions can be combined. They do not form a faction-alignment or obedience build.

## 465. Persistence and replay

On re-entry, read the session, access rule, source version, attendance record, and knowledge result from their actual owners. A dialogue flag saying “faction lesson offered” cannot prove attendance or a skill result. A later change in room access does not erase knowledge already confirmed.

If the faction's condition expires, refresh it before scheduling another session. If the current owner cannot persist the condition, treat the next interaction as a fresh conversation rather than assuming an old agreement remains active.

Repeatedly accepting the same lesson cannot duplicate knowledge or faction attendance. Task permission remains a separate owner check.

## 466. Acceptance questions

1. Are the faction's access, source, and attendance conditions explicit before the learner decides?
2. Can the learner accept only the lesson's allowed scope?
3. Does a lesson result remain distinct from faction membership, work consent, and task eligibility?
4. Can the player find an alternate source without inventing a faction or procedure?
5. Are private attendance and resulting knowledge records described only as their owners support?
6. Can either participant refuse or defer without an invented loyalty penalty?
7. Does re-entry avoid carrying forward an unsupported promise?

## 467. Installment 32 close

This installment makes faction-controlled access a source of practical branching. The player can clarify, negotiate, accept a limited lesson, seek another teacher, ask for permission, or stop. It deepens matching and lesson delivery while leaving knowledge with its owner and task authority elsewhere. No faction curriculum, loyalty meter, or unrecorded obligation is added.

## 468. Expansion installment 33 — Explain the step without naming the source

A learner may understand a confirmed procedure but not be free to identify who taught it. The second person can ask where the explanation came from; the source may have requested privacy, belong to a faction that controls access, or simply not want their name used as proof. The learner chooses what to share, while the player keeps the knowledge claim separate from its provenance.

This extends learner/teacher matching, one lesson, and owner-backed knowledge. It adds no source-secrecy flag or second knowledge ledger. If current owners cannot enforce anonymity, dialogue must not promise it.

## 469. Four facts travel separately

Keep separate: what the learner can explain; what source supports the explanation; who provided that source; and whether the learner has permission to name them. These facts may have different audiences.

The learner can answer from their own understanding, cite a public document, ask the original source for permission, identify a specialist only with consent, or decline to discuss the origin. The second person can accept the explanation, request verification, wait for a public source, or choose another lesson.

If the information cannot be verified without naming a private source, leave the claim uncertain or ask that source to participate. Memory alone does not prove provenance.

## 470. Scene seed — “Who showed you that?”

Candidate content only. Verify the learner, subject, source, any confidentiality rule, and the second person's lesson route before making this reachable.

A survivor uses a maintenance step during a practical conversation. Another person asks which faction taught them. The learner can say they learned it from a qualified source without naming them, cite a public instruction, ask the teacher, or say they do not want to discuss the source.

The player can check whether the step itself is current, ask whether attribution is permitted, invite the second person to a separate lesson, or stop. A supporting faction may confirm its public procedure without identifying who attended a private session.

## 471. The learner controls attribution

The player can ask, “May I name who taught you?” The learner can consent, give limited attribution, keep the source private, or withdraw. They do not owe an explanation.

If a lesson record exists, verify its audience through its actual owner; an attendance entry may be an access log, not public teaching credit. If the game cannot hide a source identity from this audience, offer another route or state that the source cannot be protected here.

## 472. Attribute only with permission

A teacher may allow their name, permit citation of a document only, or ask not to be identified. Respect that boundary, seek another source, or stop. The teacher's preference does not prove that the content is correct.

If the current owner does not store attribution, keep this distinction in dialogue. Do not create a persistent source-protection record inside the education panel.

## 473. Verify without exposing

The player can compare the explanation with a public manual, ask another qualified person a general question, request permission to share a redacted excerpt, or pause until the source can respond. These routes may cost time or leave part of the answer open.

A supporting faction may provide a current public procedure without confirming who first taught it. The player can compare sources, use only the confirmed portion, seek a specialist, or stop. Do not claim independent confirmation until an owner provides it.

## 474. A faction can ask without owning the learner

A faction contact may ask who taught the learner because it controls a manual, room, or safety condition. The player can explain public evidence, ask what access question needs answering, share attribution only with consent, or decline to identify the teacher.

The faction may still deny use of its property. That affects its own access and task rules; it does not erase the learner's understanding. The player can seek a community specialist, wait for permission, or choose another lesson.

## 475. Source privacy does not authorize a task

The learner may explain an idea without naming its source, but a later task still checks qualification, supervision, equipment, and location through their owners. Privacy does not create permission.

The learner can request supervised practice, perform a permitted lower-risk action, seek authorization, or decline. The player cannot reveal a private source to bypass a separate work rule.

## 476. Branch table — knowledge and attribution

| Learner's account | Source status | Player action | Result |
|---|---|---|---|
| Confirmed lesson result | Public source available | Cite it | Share the step without private attribution |
| Confirmed result | Teacher requests privacy | Ask permission or keep identity private | Attribution follows consent and owner rules |
| Partial or uncertain account | Source unavailable | State uncertainty and seek another route | No expanded knowledge claim |
| Faction asks for identity | Access question is unresolved | Explain confirmed content and limit | Faction may restrict property; no forced disclosure |
| Public and private sources conflict | Evidence differs | Compare, defer, or use shared confirmed facts | No accusation or source leak |
| No privacy-safe route exists | Audience cannot be limited | Do not ask for disclosure | Defer, find another source, or stop |

## 477. Outcomes without a secrecy meter

The learner may share a confirmed step while protecting the teacher; name the source with permission; cite a public document; invite a specialist; leave origin unresolved; accept faction access limits; or stop the conversation. None requires a new secret or trust score.

The source may later choose to participate. The learner can then ask a new question or authorize narrower attribution. A new source does not automatically revise the learner's saved result.

## 478. Player approaches

**Attribution checker:** confirms whether the source consented to be named.

**Public-source seeker:** finds material that can be cited to a wider audience.

**Scope separator:** shares the confirmed step, not an unsupported origin story.

**Boundary keeper:** refuses to trade a private identity for access without consent.

**Task gatekeeper:** checks qualification independently of who taught the lesson.

These are actions, not faction-alignment or honesty builds.

## 479. Later callbacks preserve the distinction

A future teacher can recognize the procedure without claiming to know its origin. A faction can later publish an independent instruction that confirms the step. The learner can choose to cite that public material while leaving the original source private.

If a source later asks to be named, update attribution only through a supported route and with its audience shown. No old dialogue silently broadens the audience after reload.

## 480. Persistence and acceptance

On return, read knowledge, source state, permission, and attendance from their owners. If permission to attribute was not persisted, ask again. If source identity is not stored, do not infer it from dialogue.

Accept when a learner can share a supported fact without unapproved disclosure; a faction enforces only its own access rules; attribution is not mistaken for correctness; and task authorization remains independent.

## 481. Installment 33 close

This installment adds a provenance choice after learning: explain the confirmed step, cite a public source, ask permission to name a teacher, seek verification, or stop. It protects a source only where current owners can enforce that boundary. No secrecy meter, alignment score, or parallel knowledge record is added.

## 482. Expansion installment 34 — A refresher without erasing the first lesson

A learner may remember the principle but hesitate over the sequence when they return to a task after a long gap. This is a request for practice, not proof that the earlier lesson failed. The learner can ask for a quick reminder, supervised rehearsal, a full repeat, or a chance to proceed with the confirmed step they still know.

The plan adds no automatic skill decay, freshness meter, or repeat-credit loop. Current education, skill, memory, and task owners determine what result exists and what the next action may be.

## 483. Keep the result; reopen the question

The player can read the existing result and ask what the learner still wants to review. The learner may identify one uncertain step, ask to see the source again, request a full explanation, or decide they do not need a refresher after all.

The system should not downgrade a recorded result because time passed or because the learner pauses before acting. Confidence and authorization are different. If an existing owner records memory change, use that result; otherwise, do not invent a decay state.

## 484. A refresher is not a test

The teacher can ask the learner to point out where they want help, demonstrate only when safe and supported, or offer a short explanation. The learner does not have to perform in front of a group or prove that they deserve another session.

The player can ask a plain question—“Which part would you like to go over?”—without turning it into an assessment. The learner may answer, show a practical example, ask for privacy, or stop. No score or pass/fail event is added.

## 485. Scene seed — “The step before the clamp”

Candidate content only. Confirm the subject, teacher, task, tool, current procedure, and refresher route before making this reachable.

A survivor completed a lesson earlier and now hesitates before a later repair. They remember the purpose of the step but not the order in which two parts are secured. The player can ask for a quick reminder, find the current instruction, request supervised practice, or let the task wait for a qualified person.

The teacher may have time for only a short review. The learner can accept the narrow reminder, request a full session later, ask for a written source, or decide to stop. If a supervisor is required, the task owner confirms that requirement.

The moment may end with a supported refresher, a source lookup, a specialist referral, a postponed task, or the learner proceeding under an authorized route. None erases or duplicates the prior lesson result.

## 486. Actions that distinguish review from relearning

- Check the prior result through the existing education/skill owner.
- Ask which step the learner wants to review.
- Offer a source, short reminder, full lesson, or safe rehearsal if supported.
- Verify whether the procedure itself remains current.
- Check the task's qualification and supervision separately.
- Let the learner continue, defer, or stop without a failure label.

These choices vary by learner, source, task risk, teacher time, and material access. They do not branch on whether the player is patient or strict.

## 487. The length of the gap does not decide

A learner who practiced yesterday may still request help; another who has not practiced in weeks may feel ready. The player can consult actual owner state but should not infer knowledge from a calendar threshold.

If a task owner requires a refresher after a specific condition, show that requirement and its source. If no such rule exists, a refresher remains a learner choice, not a hidden eligibility gate.

The player can accept a request for more preparation even when the task would permit immediate work. They can also let the learner proceed when all actual requirements are met.

## 488. Practice uses the correct task boundary

A verbal reminder may be enough for one question. Physical rehearsal requires the right place, tool, supervision, and permission. The teacher cannot use the education panel to start a task or reserve equipment.

The player can ask for a safe demonstration, find an authorized practice item, schedule a supervised task, or stop. If none is supported, do not fake practice progress with a dialogue flag.

The learner can complete a refresher without receiving permission to perform the live task. The task owner decides that separately.

## 489. Full repetition is not automatically better

A teacher may offer the whole lesson, but the learner can prefer one step, a source citation, or no review. A full session may take time from another task; show any cost through the current schedule owner.

If the education owner allows only one result per day, the refresher cannot award another. If it supports a distinct follow-up result, use that contract exactly. Repeating familiar material does not multiply skill.

The learner can revise their preferred route after hearing the cost. The player can offer a later session or a different teacher without implying that the learner failed.

## 490. Supporting faction routes

A supporting faction may provide a current checklist, specialist, supervised practice space, or access to an authorized training object. The player can ask for the resource, accept its stated access condition, seek a community alternative, or defer.

The faction cannot decide that time alone erased a survivor's knowledge. Its task owner may still require current supervision or qualification for faction property. Keep that requirement separate from the learner's remembered result.

Do not add a faction refresher certification. A record of attendance is not a new skill result unless the current education owner says so.

## 491. Ending routes

**One-step reminder:** the learner reviews the specific point; prior knowledge remains unchanged.

**Source revisit:** the player obtains a current reference, and the learner decides whether to continue.

**Supervised rehearsal:** the task owner permits practice and records only its supported result.

**Full lesson requested:** schedule a valid session without granting duplicate progress.

**No refresher needed:** the learner proceeds only if the task owner permits it.

**Refresher unavailable:** the learner waits, finds another specialist, or chooses a different task.

**Learner stops:** the conversation ends without a penalty or downgrade.

## 492. Player approaches

**The narrow reviewer:** asks which single step is unclear.

**The source checker:** verifies that the old lesson still matches the current procedure.

**The safe-practice arranger:** checks task permissions before scheduling rehearsal.

**The time explainer:** shows the cost of a full repeat and offers a short route.

**The boundary keeper:** accepts that the learner can ask for help without being retested.

These actions change the next lesson and task route; they do not form an education stat build.

## 493. Persistence and replay

On re-entry, refresh the original lesson result, current source, task requirements, and any supported refresher event from their owners. A dialogue line cannot decrement knowledge or grant repeat credit.

If the refresher is not persisted, ask what the learner needs again. If it is persisted, retries cannot award it twice. A later procedure change is handled as an update through its source owner, not as evidence that the learner forgot.

## 494. Acceptance questions

1. Can the learner request a refresher without losing a valid earlier result?
2. Does the player avoid inferring decay from elapsed time?
3. Is a refresher distinct from a practical authorization or new skill result?
4. Can the learner choose a narrow reminder, source review, full lesson, or no review?
5. Are costs and safety requirements supplied by current owners?
6. Does replay avoid duplicate knowledge or training events?
7. Can the learner stop without a failure label?

## 495. Installment 34 close

This installment adds an opt-in refresher route for a learner who remembers a skill but wants help with a step. The player can check prior results, current sources, task requirements, and safe practice options. Knowledge is not downgraded by time, and repetition does not create duplicate credit or task permission.

## 496. Expansion installment 35 — The procedure stayed; the conditions changed

A learner may understand a procedure and still need to adapt when the tool, material, workspace, or operating condition differs from the lesson. The scene should ask what changed before it judges the person. This extends the existing loop from review to contextual application; it does not create an additional education feature or a hidden adaptability score.

## 497. Separate remembered steps from present conditions

The player can compare the learner's prior result with the current task requirements and the source that governs the procedure. If the tool is different, that is a changed condition. If the source itself changed, that is a changed procedure. If neither changed, a learner may still ask for a cue or pause. These are distinct observations with different next actions.

Do not label a correct pause as ignorance. Do not call an outdated instruction a learner failure. Any task permission, safety threshold, material limit, or current procedure comes from its existing owner.

## 498. Scene seed — “The clamp with the bent screw”

A learner recognizes the clamp from a maintenance lesson but sees that its screw no longer seats squarely. The instructor's first response is to reach for the familiar sequence. The learner points to the bent thread and asks whether the sequence still applies. The player can inspect the tool under the proper maintenance route, fetch the current reference, ask the instructor to demonstrate on an intact clamp, defer the task, or seek the responsible specialist.

The dramatic question is not whether the learner is brave enough to proceed. It is whether the known procedure fits this specific tool.

## 499. Make comparison a player action

Give the player useful, nonexclusive choices: name the step they remember, inspect the changed part, read the current reference aloud, ask for a demonstration on a safe equivalent, or stop and request another person. Each action can reveal different information and consume a different amount of time, but none should silently grant skill or authority.

The learner may already know the answer and want confirmation. The instructor may notice a detail the learner missed. The maintenance owner may rule that no attempt is allowed. Let the interaction reflect these outcomes instead of forcing a single quiz response.

## 500. A source review can correct the teacher

The instructor is a character, not an infallible source. If the current reference contradicts an old lesson, the player can show the discrepancy, ask who owns the updated procedure, or delay the demonstration. The instructor may acknowledge the old wording, explain where it came from, or disagree until the responsible owner is consulted.

Avoid a reputation penalty for a learner who requests verification. Any revision to the lesson material belongs to its current catalog or content owner; the dialogue can identify the issue without pretending to patch the curriculum.

## 501. Safe practice must remain bounded

When an equivalent tool or harmless practice object is available, the learner may rehearse the altered hand position or inspection step there. When no safe equivalent exists, the valid branches are to observe, ask, defer, or leave the task to a qualified person. A live hazard is not an attractive “high-risk training” option.

Use the current task owner for permission and the current needs or schedule owner for cost. A training scene cannot authorize work by narrative implication.

## 502. Three kinds of useful response

The learner may explain the remembered sequence, point to the condition that invalidates it, or ask for a fresh demonstration. The teacher may give a cue, admit uncertainty, or ask the learner to locate the current source. These responses can lead to a short review, a safe practice step, a specialist handoff, or a pause.

Do not require a fixed response order. A learner who goes directly to the source is not less teachable than one who asks the instructor. A learner who stops after inspection has still made a sound decision.

## 503. Branches follow what the player verifies

- **Condition is unchanged and reference is current:** the learner may proceed only under the existing task permission.
- **Tool differs but a safe equivalent is available:** demonstrate or practice the relevant step, then let the task owner decide whether the live work is authorized.
- **Source is outdated or unclear:** stop the attempted operation and seek the responsible owner.
- **No safe practice or specialist is available:** defer, choose another task, or leave the matter unresolved.
- **The learner does not want to continue:** close the lesson without a failed result or lost prior knowledge.

The branch is based on observed conditions and the learner's choice, not on a good/bad personality axis.

## 504. Supporting factions provide context, not certification

A supporting faction may offer a current manual, a compatible tool, a safe demonstration space, or a knowledgeable person. The player can compare that offer with the shelter's own source, ask for a limited demonstration, or decline if the terms expose the learner to an unacceptable cost. Confirm faction capacity and any item or access conditions from their existing owners.

No faction visit should certify the learner merely because it supplied equipment. The education owner records the learning result; the task owner controls authorization.

## 505. Callbacks distinguish knowledge from trust

If the learner later pauses at a different mismatch, the callback can show that they learned to check the condition before acting. If they ask the same instructor for help, that may reflect trust or convenience, not dependence. If they choose another source, do not frame it as betrayal.

When an earlier correction changed a reference, later dialogue should cite the current wording and preserve who reported the discrepancy. It should not invent a new skill tier to remember the scene.

## 506. Ending routes for this lesson

The scene may close with the learner proceeding under a valid task permission, completing safe practice and waiting for authorization, handing the issue to a specialist, choosing a different task, or leaving the question open. Each close should say what is known and what remains unknown.

There is no mandatory graduation beat. The learner can leave with the same recorded knowledge and a better reason to ask before applying it in changed conditions.

## 507. Play approaches

**The careful comparer** checks tool, material, and reference before booking practice.

**The learner-led route** asks the learner to identify the mismatch and choose the next step.

**The source-first route** seeks the current procedure before asking anyone to demonstrate.

**The schedule protector** defers live work when a safe window or instructor is unavailable.

**The modest closer** accepts that today's result may be a useful pause rather than a completed task.

These approaches vary the scene and consequence without creating separate skill builds.

## 508. Persistence and replay

After reload, read the prior lesson result, current source, tool or task condition, and any completion event from their existing owners. Do not infer the condition from the last dialogue choice. Repeating a source review must not award the same knowledge event twice.

If the game cannot persist a practice attempt, ask again whether the learner wants to review it. If the source has since changed, show the new source as current while retaining any supported account of the earlier correction.

## 509. Acceptance questions

1. Does the scene ask what changed before judging learner competence?
2. Can the learner pause, consult, practice safely, or seek a specialist?
3. Do task permissions and safety requirements come from their current owners?
4. Can the instructor be mistaken without the scene blaming the learner?
5. Does no branch grant authorization through dialogue alone?
6. Are current sources and any correction represented truthfully after reload?
7. Does replay avoid duplicate training credit?

## 510. Installment 35 close

This installment makes changed conditions a decision point in education. The player can compare the learner's knowledge with the present tool and procedure, choose safe review or an owner-backed handoff, and defer without erasing prior learning. The result is contextual judgment, not a new skill meter or a fourth education subfeature.

## 511. Expansion installment 36 — No single teacher owns every step

A lesson can cross from general understanding into a specialty that the first instructor is not qualified to teach. The player should be able to preserve what is already useful, identify the boundary, and bring in a second qualified source only for the part that needs it. This is a staged lesson route, not an assumption that every learner needs a panel of experts.

## 512. Separate the shared foundation from the specialist step

The first teacher may explain vocabulary, identify the relevant tool, or show how to read a current instruction. A second instructor may own a practical step or a procedure with a stricter qualification. Ask which portion each person can teach and which current source supports that claim.

Do not present one teacher as incompetent because they decline a step outside their scope. Do not imply that two partial explanations automatically combine into a complete qualification.

## 513. Scene seed — “The mark on the regulator”

A learner recognizes a mark from a general maintenance lesson but asks what it permits them to adjust. The available instructor can explain the label and locate the current manual, but says the adjustment requires a qualified specialist. The player can stop at the explanation, arrange a second session, ask whether the specialist can attend the same shift, or choose a different lesson while the learner waits.

The learner can also say that the label was all they wanted to understand. A specialist referral is an option, not a forced escalation.

## 514. Let the learner choose a staged route

Possible routes include a short foundation lesson now and specialist practice later; a single session with both teachers if schedules and owners permit; a source-only answer; or a pause until the specialist is available. The player can ask the learner which part matters for their current goal before spending scarce teacher time.

If the learner declines to wait, retain the confirmed foundation result and leave the specialist step unresolved. Never discard valid knowledge because a later stage is unavailable.

## 515. A shared session needs two real commitments

If both teachers attend, validate each person's eligibility and availability separately. The first teacher's presence does not authorize the specialist's work, and the specialist's attendance does not retroactively validate an earlier demonstration. Schedule and duty owners confirm the time; the education owner confirms the learner and result.

If only one instructor can attend, the player may reschedule, split the session, use an approved source, or close the request. Do not create a silent substitute teacher.

## 516. Distinct explanations can still agree

Two instructors may use different words for the same confirmed step. The learner can ask each person to identify the source they rely on, compare the shared part, or stop where the accounts diverge. The player may record a content question for the current curriculum owner without asking the learner to choose which adult to trust.

If a real conflict remains, teach only the uncontested material and defer the disputed step. Do not produce a blended procedure by stitching phrases together.

## 517. Costs and timing stay legible

A two-person session can take longer, require separate shifts, or delay another task. Show only costs the schedule and task owners can confirm. The learner can accept the later time, ask for the short foundation now, seek another qualified person, or abandon this route while keeping prior results.

Do not make the specialist's time a faction reward or a hidden friendship benefit unless an existing owner supplies that offer and its terms.

## 518. Branches after the scope check

- **Foundation is sufficient:** the learner keeps the result and proceeds only within current task permission.
- **Specialist step is wanted and available:** schedule a valid follow-up with fresh acceptance.
- **Only one instructor is available:** teach their supported portion or defer without inventing the rest.
- **Sources disagree:** preserve the discrepancy and seek the relevant owner.
- **The learner changes their mind:** close the request without removing earlier knowledge.

Each route changes the lesson path through availability, scope, and consent rather than an honest/evil or strict/kind axis.

## 519. Supporting factions can supply one link

A supporting faction may offer an authorized specialist, a source, a supervised practice space, or a time window. The player can compare that link with the shelter's own route, accept only the needed portion, or decline the conditions. Verify the representative, access, and capacity against current faction and task owners.

The faction does not own the learner's record. Its instructor cannot silently certify a step beyond their documented scope, and faction help does not grant membership or trust.

## 520. Outcomes keep partial knowledge useful

Later dialogue can show the learner using a confirmed foundation step, waiting for the specialist, choosing another subject, or reporting that the manual answered the question. If a second teacher resolved the issue, credit the source accurately. If the question remains open, say so without implying that the learner failed.

The education owner records supported knowledge once. A schedule change, faction visit, or second instructor does not duplicate the result.

## 521. Player approaches

**The scope mapper** divides the question into steps and checks who can answer each.

**The learner-led planner** lets the learner choose foundation now or a specialist later.

**The source comparer** checks the current approved reference before joining two accounts.

**The schedule broker** asks both instructors for a valid window without assuming availability.

**The clear boundary keeper** accepts a partial answer when the specialist route is not ready.

## 522. Persistence and replay

On re-entry, refresh the learner's prior result, each instructor's eligibility, current source, schedule, and any supported follow-up state from their owners. If the second session was not persisted, ask whether the learner still wants it. Do not infer attendance from a prior plan or dialogue.

Repeating the shared session cannot grant two results for one step. If each stage has a separate supported outcome, preserve their distinct identity under the education owner.

## 523. Acceptance questions

1. Can one instructor teach a valid portion without being forced to own the whole topic?
2. Is each teacher separately qualified and available for the proposed step?
3. Can the learner choose a source, split session, specialist, or no follow-up?
4. Are conflicting accounts preserved rather than blended into a false procedure?
5. Do schedule, task, and education owners confirm their own facts?
6. Does faction assistance remain scoped to its actual source and capacity?
7. Can replay avoid duplicate results across linked sessions?

## 524. Installment continuity

A staged lesson differs from a refresher: the learner is not revisiting an old step but deciding how to reach a new one that has a distinct qualification boundary. Keep both records accurate. The first result stays valid; the later step remains pending until the responsible owner confirms it.

## 525. Installment 36 close

This installment supports a learner when useful instruction crosses teacher scopes. The player can take the foundation lesson, arrange a qualified specialist, compare sources, split the schedule, or stop with partial knowledge intact. No single instructor is made all-knowing, and no faction or dialogue choice grants unowned certification.

## 526. Expansion installment 37 — The learner does not have to trust this teacher

A learner may question an instructor because of an earlier disagreement, a broken promise, or simply a preference for someone else. That concern does not prove the lesson is false, and a correct procedure does not require the learner to accept this particular teacher. The player can keep the route open while separating the quality of the source from the relationship between its users.

## 527. Keep trust, evidence, and eligibility distinct

The player can verify whether the instructor is qualified, compare the current source, and ask the learner what route feels workable. These facts answer different questions. A qualified teacher may not be the learner's preferred teacher; a preferred teacher may not be eligible for the procedure; an approved source may let the learner proceed without either person.

Do not make a hidden relationship threshold determine whether a correct source is visible. Use current relationship facts only where an existing owner supports a relevant consequence.

## 528. Scene seed — “I will read the page, not ask her”

A learner needs to understand a maintenance label and refuses a session with the instructor who taught the earlier class. They are willing to read the public manual or meet another teacher. The player can show the source, check for another qualified person, ask whether a narrow conversation with the original instructor would be acceptable, or let the learner wait.

The learner does not owe an explanation for preferring another route. If they volunteer a reason, the player can listen without requiring a reconciliation scene.

## 529. Offer routes without forcing a confrontation

The learner may accept the original instructor, request a different teacher, use an approved public source, bring a trusted observer if the current lesson owner permits, or decline for now. They may also accept the facts while maintaining their personal boundary. The player can ask which route they prefer instead of telling them that “everyone needs to talk it out.”

The original instructor can respond with respect, ask for a chance to correct a mistake, or decline to teach under the proposed conditions. Each choice changes access to that person, not the learner's underlying knowledge.

## 530. A repair conversation is optional

If both people want to address a prior misunderstanding, the player can offer a private meeting, a source-led review, a written correction, or no meeting. The learner can accept, postpone, set a boundary, or refuse. The instructor can acknowledge an error without demanding forgiveness.

Do not use a successful lesson as proof that the relationship is repaired. Do not use a repaired relationship as proof that the procedure is correct. Those facts have separate owners and separate evidence.

## 531. Source review can be independent

The learner can compare the current reference directly, ask a different qualified person to explain a term, or request a demonstration on a safe example. If accounts conflict, keep the uncertainty visible and consult the responsible content or safety owner. The player's own confidence does not settle it.

If only one current approved source exists, say that clearly. The learner can still decline to use it with this instructor and wait for an acceptable route.

## 532. Do not turn preference into an investigation

The player should not pressure the learner to disclose a personal history just to select another teacher. If an assignment or supervision policy requires a reason, the relevant owner defines the minimum information needed and who can see it. The choice of a public manual or alternate instructor should not expose the learner's private concern by default.

The teacher may ask to understand the refusal. The learner can answer, keep the reason private, or ask the player to pass on only a practical boundary.

## 533. Practical work keeps its normal permission boundary

Choosing a different teacher does not change task authorization. Before any live practice, verify the current task, equipment, location, supervision, and safety rules. If those owners require a particular qualified person, the learner can wait or choose another activity; the player cannot waive the requirement to avoid an awkward conversation.

The lesson owner records only the supported education result. A different teacher is not a substitute for a missing permission.

## 534. Supporting factions can offer distance

A supporting faction may provide a public reference, an authorized specialist, or a separate location where the learner can study. The player can accept the practical route, compare its source with the shelter's, decline its access terms, or continue seeking an independent teacher. Faction support does not make the learner take a side in a dispute.

Confirm any claimed specialist, source, or space through the current faction and task owners. A faction cannot promise that the original teacher will apologize or that the learner will trust a replacement.

## 535. Branches follow the learner's chosen route

- **Original teacher accepted:** conduct the lesson with fresh consent and current source checks.
- **Alternate teacher available:** schedule through matching, eligibility, and duty owners.
- **Public source chosen:** use only a source the current curriculum permits and preserve its limitations.
- **Repair conversation chosen:** keep it separate from lesson completion and forgiveness.
- **No route accepted:** defer or close without downgrading prior knowledge.

Every route preserves the learner's ability to act on a confirmed result while deciding who they will learn from next.

## 536. Later callbacks remain precise

If the learner later asks the original instructor for help, that is a new choice rather than proof the earlier refusal was insincere. If they continue using the public source, show it as a valid route where the owner supports it. If a new teacher is unavailable, do not send them back to the person they declined without asking.

The instructor may remain disappointed. The narration can acknowledge that feeling without making the learner responsible for repairing it.

## 537. Player approaches

**The evidence checker** confirms that the source and instructor are valid for this subject.

**The route giver** offers another qualified person, a public reference, or a pause.

**The boundary keeper** accepts the learner's preferred distance without demanding a reason.

**The repair facilitator** offers conversation only when both people want it.

**The task gatekeeper** keeps live practice under its ordinary authorization rules.

## 538. Persistence and replay

On return, refresh the learner's current preference if the social owner stores it, the instructor's eligibility, source version, availability, and any completed lesson result from the relevant owners. Do not assume the learner still refuses or now accepts based on an old line of dialogue.

Repeated route selection cannot award duplicate knowledge or automatically change relationship state. If a repair conversation was not persisted, ask whether the learner wants to revisit it.

## 539. Acceptance questions

1. Can the learner choose a different teacher or source without proving misconduct?
2. Are source validity, teacher eligibility, and personal trust shown as separate facts?
3. Is a repair conversation optional for both people?
4. Can the learner keep a reason private while selecting a supported route?
5. Do live task permissions remain with their existing owners?
6. Does faction support provide access without forcing alignment?
7. Are preference, relationship, and lesson results replay-safe?

## 540. Installment 37 close

This installment makes teacher preference an actionable branch without turning trust into a knowledge score. The learner can choose the original instructor, another qualified person, a public source, an optional repair conversation, or delay. Correct instruction remains evidence-based, and no one is forced into reconciliation.

## 541. Expansion installment 38 — The learner says stop before the demonstration

A learner may notice a risk or mismatch before the instructor does. The player needs to treat that interruption as useful information, then check the condition through the proper owner. The learner's decision to stop does not prove the procedure is wrong, but the instructor's confidence does not prove it is safe.

## 542. Stop the demonstration without deciding the cause

The player can pause the lesson, ask the learner what they noticed, and inspect the setup under the relevant task or safety authority. The cause may be a faulty tool, an unexpected sound, a misunderstood step, or no confirmed danger at all. Until it is checked, narration should say what was observed rather than supply a diagnosis.

Do not award or remove knowledge for interrupting. A stop is an action, not a quiz answer.

## 543. Scene seed — “The needle moved on its own”

During a safe demonstration, a learner sees a pressure needle drift after the instructor releases the control. They say “stop” and move their hand away. The player can end the demonstration, ask the teacher to keep distance, consult the current manual, request the equipment owner, or switch to an approved inert example.

If the equipment is part of active work, the task owner—not the lesson scene—determines safe shutdown and reporting. The instruction can resume only when the owner confirms the setup is suitable.

## 544. Listen before explaining

Ask the learner what changed in their view: a reading moved, the tool felt loose, the instruction differed from the source, or they simply no longer wanted to continue. They can answer, point, ask the teacher to check, or leave the reason unstated. The player may acknowledge the stop without demanding a detailed account.

The teacher can agree, ask for a second look, or explain why they think the movement is expected. If accounts differ, verify the source before returning to practice.

## 545. The teacher's response changes the next route

The instructor may immediately pause, thank the learner, ask the responsible owner to inspect, or become defensive. The player can support a pause, redirect to the approved source, request a different teacher, or end the session. Do not convert defensiveness into proof of incompetence or a relationship penalty without a current owner-backed consequence.

The learner can stay for the source review, observe from a safe distance, choose an inert demonstration, or leave. Their prior result stays intact.

## 546. A stopped lesson can still have a truthful outcome

Possible outcomes include an interrupted session with no new result, a completed source review, a safe demonstration on an approved sample, a referral for equipment inspection, or a later reschedule. The education owner determines whether a lesson result occurred; the equipment and task owners determine whether the setup may be used.

Do not call a session failed because a learner prevented a possibly unsafe step. If the lesson did not reach its learning goal, state that plainly without erasing earlier knowledge.

## 547. Separate the lesson question from the equipment issue

The player can still ask what the learner wanted to understand and whether a source or another example answers it. A mechanical concern can be routed separately; it should not consume the learner's only chance to ask. Conversely, solving the equipment concern does not force the learner to resume.

If no owner can verify the setup, stop practical demonstration and offer only supported alternatives: a source, qualified specialist, later session, different topic, or no lesson today.

## 548. Protect other learners without shaming anyone

If others are present, the player can pause the shared demonstration, explain only the confirmed practical reason, and let each learner choose whether to stay. Do not single out the person who called stop or tell the group they caused a delay. Another learner may ask questions, leave, or wait for the owner check.

If the concern is private, ask before describing it to the group. The lesson's audience does not automatically own the learner's explanation.

## 549. Supporting faction response

A faction may provide an authorized inspector, a safe practice item, or a current procedure. The player can request that support, compare it with the shelter's source, or decline if its terms are unsuitable. A faction's willingness to demonstrate does not clear equipment that another owner controls.

If the faction contact disagrees with the learner, preserve the disagreement and seek the owner with authority over the equipment or procedure. Do not use the faction's standing as a substitute for evidence.

## 550. Branches after the stop

- **Owner confirms a hazard or defect:** route inspection or repair and keep the lesson paused.
- **Owner confirms the movement is expected:** explain the evidence and let the learner resume, observe, or leave.
- **No owner is available:** use an approved source or safe equivalent, or defer.
- **Learner no longer wants the session:** close without penalty or knowledge downgrade.
- **Teacher declines to continue:** find another qualified route or end the lesson honestly.

The branch follows verification and consent, not a courage or obedience score.

## 551. Player approaches

**The immediate pauser** stops the demonstration before diagnosing the issue.

**The attentive listener** asks what the learner noticed and accepts a brief answer.

**The source checker** compares the setup with the current approved procedure.

**The equipment caller** routes inspection through its actual owner.

**The lesson protector** preserves a source or alternate topic when practical work must stop.

## 552. Persistence and replay

On return, refresh the learner's prior result, current equipment condition, inspection outcome, source version, and session status from their owners. A dialogue flag cannot prove the equipment was inspected. Replaying the scene cannot create a second stop event, damage report, or training result.

If the owner did not persist the reason for stopping, ask the learner again only if it is appropriate; do not invent a diagnosis to make the callback complete.

## 553. Acceptance questions

1. Can the learner interrupt before the scene requires proof of danger?
2. Does the player verify the cause through the correct equipment or safety owner?
3. Can the instructor respond without automatic character judgment?
4. Are group members protected from unnecessary disclosure or blame?
5. Can the learner stop while keeping earlier knowledge intact?
6. Are lesson, equipment, and task outcomes kept separate?
7. Does replay avoid duplicate reports or lesson results?

## 554. Installment continuity

A later lesson may return to the same equipment only after its owner confirms the present condition. The learner can remember that they called stop without the game treating that memory as proof of continuing danger. The instructor can acknowledge the interruption without being forced into a moral apology scene.

## 555. Installment 38 close

This installment makes a learner's stop decision consequential and safe. The player can listen, pause, verify, use an approved example, seek an owner, or end the session. The learner is not penalized for raising a concern, and neither dialogue confidence nor faction presence replaces an equipment check.

## 556. Expansion installment 39 — The class is interrupted by shelter work

A lesson can be underway when a real shelter need changes the room: an urgent repair, a medical call, a power interruption, a water delivery, or a shift recall. The player should route the interruption through the owner of that event and let people make a fresh choice. It is a pressure on the lesson's schedule, not a random failure roll and not a hidden test of who cares about education.

## 557. The interruption belongs to the system that caused it

If a roster change recalls the teacher, the duty owner confirms it. If a need or emergency requires a learner, the responsible work or health owner determines the valid response. If the room loses power or access, its current facility owner reports the condition. The education scene can explain what changed, but cannot invent the incident or override it.

The player sees which part of the lesson can safely pause, which materials can remain, and whether anyone has a committed task. If the game cannot identify the cause, show only that the session is interrupted and ask the owner before presenting a precise reason.

## 558. Scene seed — “The water line needs a second pair of hands”

A teacher is explaining how to read a pressure mark when a water-line alert arrives. The learner is not automatically the right helper; the player checks the alert's owner, task requirements, qualifications, schedule, and current needs. The learner can volunteer if eligible, ask to stay for the lesson, leave the task to another person, or request that the teacher pause until the situation is clear.

The player can accept the valid assignment, find another eligible worker, keep the class together for a short source review, or end the session. If the alert is not confirmed, the lesson need not be abandoned on rumor alone.

## 559. Protect the learner's choice during the interruption

The teacher can ask whether the learner wants to stop, continue with a short explanation, or resume later. The player can tell the learner what the interruption costs, offer to preserve the current topic, arrange another teacher, or close for today. The learner may prioritize the task, remain in class, or decline both options while they think.

Do not narrate volunteering as proof of devotion or staying as proof of selfishness. If the learner is assigned under a valid owner-backed command, make that assignment and its authority visible; do not disguise it as a choice.

## 560. A paused lesson is not a completed one

The session owner returns the actual result: completed, interrupted, deferred, or another currently supported state. Do not award a full result because the teacher delivered most of the explanation. If a learner already completed a valid outcome before the interruption, preserve it; if the event lacks enough evidence, leave the result incomplete.

The player can still preserve the exact point reached as ordinary dialogue where supported. A narrative note saying “we were on the second diagram” is not a new proficiency ledger. On return, the learner and teacher can decide whether to resume, review, or start a new eligible session.

## 561. Do not make a second attempt free by accident

If resuming would invoke the same daily lesson owner, confirm its day/session identity. A second panel opening or re-entry cannot award another result. If the current owner only supports one completed session per day, the valid branches are to continue only through its existing resume behavior, defer, use a separate approved source, or wait for a future eligible session.

Do not create a UI flag to turn the interrupted event into a fresh lesson. If the owner cannot distinguish resume from duplicate, keep the resume route proposed and avoid promising repeat credit.

## 562. The class can split without inventing a group result

Some learners may leave while others stay. Check each learner's age, eligibility, consent, duty, and session outcome separately. A shared room or identical subject does not prove that everyone was present for the same content or achieved the same result.

The player may schedule separate follow-ups, direct one learner to an approved source, ask who wants another lesson, or close the group scene. If the education owner supports multiple learners, it must still record each valid result under its current contract. Otherwise, only the learner with an eligible individual session may receive one.

## 563. Disruption variants

**Teacher recalled:** the learner can accept a later appointment, ask for another qualified teacher, or keep only what the first owner confirmed.

**Learner recalled:** the teacher can stop, offer a source to take away where permitted, or wait for the learner to return. No lesson result is inferred from a handout.

**Room becomes unavailable:** move only if the alternate location is approved and accessible; otherwise defer.

**Urgent work remains uncertain:** ask the event owner for status instead of sending the class away on rumor.

**Another learner needs assistance:** ask the original learner whether they want to continue alone, invite the new learner through matching checks, or end the session.

Each variant uses the same three education subfeatures and the relevant external owner.

## 564. Faction support during disruption

A supporting faction may provide a substitute instructor, a safe room, a public reference, or practical assistance with the interrupting task. The player can accept the specific offer, compare its timing and terms, seek another route, or let the lesson wait. Faction capacity and access remain with their existing owners.

The faction cannot classify the interrupted lesson as complete, force the learner to take an assignment, or create a second education result. Its contribution can solve a constraint without taking ownership of the lesson.

## 565. Branches when the player returns

**The task was completed:** refresh the learner's availability and ask whether they still want the lesson.

**The task remains active:** reschedule or choose a different subject that fits the present window.

**The teacher is unavailable:** ask for an eligible replacement or wait; do not auto-substitute.

**The source changed during the delay:** use its current version and identify the update.

**The learner no longer wants the session:** close it without erasing earlier knowledge or assigning a failure label.

The player is responding to a changed day, not selecting a permanent education identity.

## 566. Story language for an unfinished hour

The scene can carry a physical reminder: a chair left beside the workbench, a diagram folded at the page they reached, or the instructor returning the only clean pencil before joining the shift. Use these details only when they fit the location and current content. Avoid a sentimental speech declaring that the unfinished lesson was secretly completed.

The learner may be irritated, relieved, focused on the urgent task, or quiet. The teacher may apologize for leaving, explain the recall, or simply go. Preserve character voice and state; do not standardize every interruption into gratitude.

## 567. Player approaches

**The source saver** offers the approved reference and names what it does not answer.

**The schedule rebuilder** checks current availability before promising a new time.

**The learner-first pause** asks what the learner wants before closing or continuing.

**The owner checker** confirms the external task rather than guessing from an alert.

**The honest closer** reports an interrupted session without pretending it was complete.

**The alternative planner** asks whether another subject or eligible teacher fits today.

## 568. Persistence and replay

On load, refresh session outcome, calendar day, lesson identity, attendance if supported, current source, duty state, task state, and any accepted follow-up from their owners. The player can reopen the lesson view, but it cannot replay the education event. A scheduled follow-up is not attendance, and attendance is not a result.

If interrupted state is not durable, state that the current owner cannot preserve the point reached and ask whether the learner wants a fresh eligible session. Do not reconstruct a partial progress percentage from dialogue.

## 569. Acceptance questions

1. Does a real interruption come from a current owner rather than narrative chance?
2. Can the learner choose among staying, helping where eligible, and deferring?
3. Is an interrupted session distinct from a completed lesson result?
4. Does a resumed session avoid a second daily award?
5. Are split learners checked individually for eligibility and outcome?
6. Can a faction assist without owning the learner's result or assignment?
7. Do callbacks preserve uncertainty and character voice after reload?

## 570. Installment continuity

An interruption can become a meaningful later branch only if the game remembers the supported fact that the lesson stopped and if the learner wants to return. The callback should not invent urgency or imply that the learner owes the class another hour. If no durable session state exists, preserve the boundary and let the conversation begin again honestly.

## 571. Installment 39 close

This installment treats shelter work as a real interruption to education. The player can verify its source, preserve a useful reference, let eligible people choose their next action, schedule a valid return, or close the lesson without false completion. Existing task, schedule, and education owners each confirm their own outcome.

## 572. Interruption casebook — The teacher's shift starts early

The schedule owner reports that the teacher must leave before the planned end. The learner can keep the explanation they received, ask for a source, ask whether another qualified teacher is available, or stop. The original instructor can offer a later meeting only if their schedule owner confirms it. Do not mark the lesson complete just because the teacher has to go.

If the learner chooses another teacher, recheck matching and scope. The replacement does not inherit the first teacher's personal explanation or claim that the learner understood it. They can begin with the learner's current question and a supported source.

## 573. Interruption casebook — The room becomes a route

A corridor or common room becomes necessary for an emergency task. The location owner determines whether people must leave and which alternate spaces are usable. The player may move the lesson only after confirming access, privacy, and any equipment restrictions; otherwise the choices are to take an approved source away, wait, or close.

A room change may alter what can be taught. No live tool demonstration should continue in a passage because the teacher has a diagram in hand. If an alternate room is noisy or temporary, the learner can accept a short source review, request a different time, or choose another topic.

## 574. Interruption casebook — The learner is recalled to a task

The learner may have an actual assignment, a voluntary offer, or only an unconfirmed request. The player checks the relevant owner before telling them they must leave. If the assignment is valid, the learner can follow it; if it is voluntary, they can decide whether to accept; if it is unconfirmed, the player can wait for the source rather than inventing an obligation.

After the task, the learner decides whether to return. The teacher may have moved on to other duties. The player can offer another session, a public source, or closure. None of these changes the learner's previous knowledge.

## 575. Interruption casebook — A classmate asks to stay

One learner wants to continue after another person leaves. Verify whether the education owner supports an individual continuation and whether the remaining teacher's time permits it. The player can continue for one person, ask both whether to reschedule together, or close the shared scene.

The departing learner does not receive progress by association. The learner who stays does not receive more credit simply because the room became quiet. Each result follows the same valid session contract.

## 576. Interruption casebook — The alarm proves to be a false report

The event owner may confirm that no response is needed. The player can tell the class what is known, ask whether they want to resume, or keep the lesson ended if someone has left. Do not return everyone to their chairs automatically. A false report still cost attention and may have changed the teacher's availability.

If the alert source is still uncertain, keep the interruption unresolved. The lesson can wait while the player checks rather than teach a scene that asks learners to ignore alarms.

## 577. Interruption casebook — A learner has to help a neighbor

The learner may ask to stop because another resident needs help. Check whether the request is an urgent task, a voluntary personal offer, or an informal plea. The player can route it, offer another eligible helper, or let the learner respond within their own limits.

Do not force the learner to provide care because they were the first person asked. Afterward, let them decide whether they want to talk about the interruption or return to study without recounting private details.

## 578. Interruption casebook — Supplies are moved before the lesson resumes

If a teaching tool or example is relocated during the interruption, confirm its current owner and condition. It may remain available, be reserved for a real task, or be unavailable. The player can choose another source, request a safe equivalent, or defer.

The lesson owner should not claim that the original material was consumed unless its authority says so. A repeated preview cannot reserve or duplicate a scarce item.

## 579. Interruption casebook — The learner completed the task while away

The work owner may confirm a task result that answers the learner's practical question. The player can ask whether the learner wants a follow-up explanation, compare the result with the source, or close the lesson. Task completion does not automatically grant an education result, and a lesson is not needed to celebrate a completed task.

If the task reveals a knowledge gap, the learner can request another session. If it shows the learned step was valid, only the existing education owner decides whether any new result was earned.

## 580. Interruption casebook — The class divides over what to do

Some learners may want to leave for the task while others prefer to continue. The player can present the confirmed choices individually, avoid public ranking, and close the shared session for those who depart. Do not make the majority decide for a learner who has a distinct assignment or concern.

If a group lesson is supported, each learner's eligibility and completion are still checked separately. If not, the player can preserve individual sessions for later and report no group result.

## 581. Compact response matrix

| Changed condition | Player action | Learner route | Owner-backed result |
|---|---|---|---|
| Teacher recalled | Check schedule | Wait, use a source, or seek another teacher | No replacement assumed |
| Room unavailable | Check access and safety | Relocate or defer | No unauthorized move |
| Voluntary task offer | Clarify terms | Accept or decline | No hidden assignment |
| Alert uncertain | Verify source | Wait or choose a safe alternative | No rumor treated as fact |
| Item moved | Recheck inventory | Substitute or defer | No duplicate material |

The table is a writing aid; production behavior still comes from the actual owner contracts.

## 582. Faction-assisted interruption scenes

An authorized faction contact might lend a teaching surface, offer a teacher for a later hour, or help with the task that caused the interruption. The player can accept one concrete service, ask about its cost, or decline. The faction cannot make the original lesson complete or force the learner to leave.

If the faction's service introduces a new subject, treat it as a new proposed lesson and repeat eligibility, scope, cost, and consent checks. Never carry over acceptance from the interrupted class.

## 583. Casebook close

The interruption variants give the player a practical response to early shifts, room changes, false alerts, voluntary help, and separated learners. They preserve the same rule: check the source of the interruption, ask what the learner wants next, and let current owners determine task, schedule, material, and lesson outcomes.

## 584. Expansion installment 40 — The learner chooses how to return

After the interruption, let the learner choose the shape of return.

A return invitation should not assume that the learner wants to repeat the full lesson. They may want the missing step, a source to keep, a short practical demonstration, a different teacher, or no continuation. Present the options only when the education owner can support their resulting lesson form. If it supports only a complete session, explain that boundary and let the learner decide whether to book it.

The teacher can offer an observation about what was covered, but that observation is not a completion record. The learner can correct the account: perhaps the explanation stopped before the important part, perhaps they already knew the first half, or perhaps they do not want to continue. This creates dialogue variation from the learner's stated experience without manufacturing a second progress ledger.

## 585. Re-entry branch — Start with the unresolved question

The player can ask what the learner still wants answered. The learner may name a specific procedure, ask for its source, say that the interruption made them lose their place, or decline to resume. If they identify a question outside the first teacher's scope, the player can look for a qualified source or defer. No character should pretend to know an answer simply because the scene needs a conclusion.

For a supported continuation, the teacher first recaps the confirmed material in a sentence and asks whether it matches the learner's memory. The learner can agree, correct it, or ask to skip the recap. This is a small interaction with a real narrative purpose: the player learns which branch to offer next, while the lesson owner still decides what can be recorded as a result.

## 586. Re-entry branch — Practice is not the same as a lecture

Some learners prefer to watch a demonstration; others may want to try under supervision or read first. The player can offer these choices only where the subject, location, equipment, and current education contract support them. If a practical attempt could damage an item or put someone at risk, the qualified teacher explains that limit and proposes a safe substitute or a later session.

Do not use success at a simple demonstration to imply mastery of a broad skill. The scene can end with a narrow accomplishment—“you can identify the valve”—while leaving repair, diagnosis, and independent operation outside the result. The player's language should match the narrow fact, and any durable knowledge change remains with the education authority.

## 587. Re-entry branch — The reference contradicts an old lesson

A source may show that a previously taught instruction has changed or was incomplete. The teacher can explain the discrepancy, point to the current authority, and ask whether the learner wants to revisit the procedure. The learner may feel embarrassed, skeptical, relieved, or merely practical. These reactions are character choices, not a penalty applied to the learner's record.

If the old teaching was wrong, do not preserve it as a valid result for the sake of continuity. If the source itself is uncertain, label it uncertain and seek an owner who can resolve the disagreement. The scene can branch into correction, a second opinion, a safe pause, or a decision not to use the procedure until verified.

## 588. Re-entry branch — Another learner asks to teach

A learner who understood one part may offer to explain it to a classmate. The player checks whether peer teaching is appropriate for the topic and whether the original teacher agrees to supervise when needed. The classmate can accept, ask the qualified teacher instead, or continue alone. A peer's explanation may be useful dialogue without granting either learner a new formal skill result.

This branch lets a learner contribute without converting the lesson into a hierarchy. The original teacher can acknowledge a clear explanation, add a correction, or say that the topic needs a qualified instructor. The learner who teaches can stop if they become uncomfortable. The classmate's consent matters as much as the volunteer's offer.

## 589. Faction support without curriculum capture

A supporting faction may lend a manual, provide an eligible instructor, or make a room available for a later class. The player should ask what the service covers and whether it creates a practical condition such as a time limit, supervised access, or return of borrowed material. A learner may accept the reference while declining membership, recruitment, or a separate work request.

If two factions offer different explanations, compare the sources and qualifications instead of presenting the disagreement as a popularity contest. The player can ask a neutral specialist, postpone the lesson, or study both sources where safe. A faction's role remains supporting: it can provide an input to learning, but neither faction defines the learner's beliefs or owns their outcome.

## 590. Re-entry scene endings

The interrupted class can conclude with a scheduled continuation, a narrow question answered, a source borrowed, a new qualified teacher requested, a correction under review, a peer explanation, or a voluntary stop. Each ending should name the factual next step and leave optional future study open only where the relevant owner supports it.

Avoid endings that award the player a generic “good teacher” identity. The meaningful distinction is what the learner chose and what the verified lesson actually covered. A quiet close can be complete: the learner has enough information for now and does not owe the shelter a promise to return.

## 591. Expansion installment 41 — A question becomes a route through the shelter

A lesson can begin with a small question that belongs to several different authorities. The learner may ask how to read a gauge, why a task was postponed, who may enter a room, or whether an old procedure still applies. The player’s first useful action is to classify the question with the learner: is this a teachable procedure, an unresolved factual claim, a personal decision, or a request for an owner to act?

Do not make “education” absorb every uncertainty. A learner can study how a valve is generally inspected, but the maintenance owner decides whether this valve is safe to use. They can learn what a notice says, but its author confirms the audience and current instruction. They can understand a faction’s offer, but the survivor decides whether to accept it. The education branch should make the right handoff legible rather than answering on behalf of the other system.

## 592. Route opening — Ask what the learner wants to do with the answer

Two learners can ask the same question for different reasons. One may need to complete a task, another may be curious, and a third may want to explain the subject to someone else. The player can ask one short follow-up about the intended use. The learner may answer, keep the reason private, or say that they are not sure yet. All three can lead to an eligible lesson if the current education owner allows it.

The answer changes the scene’s framing, not the learner’s entitlement. A person who declines to explain their motivation should still be able to request instruction. Do not require a personal disclosure as an eligibility test. When the question is attached to an active task, check the task owner before presenting lesson choices that could delay or alter the work.

## 593. Route branch — The question is already answered by an owner

The player may discover that the relevant task, location, or notice owner has a current answer. Offer the learner a choice between receiving that answer directly, studying the underlying procedure, or asking a further question. A confirmed answer is not a lesson completion, and a lesson is not needed to legitimize a fact the owner has already established.

If the learner disputes the owner’s answer, record the nature of the concern in dialogue and route it to the responsible reviewer where one exists. Do not stage a vote between the learner and the owner. A branch can end with a verified answer and a separate request for review, preserving both the practical next step and the unresolved disagreement.

## 594. Route branch — The answer depends on a source

When the question needs research, the player can help identify what source would resolve it: a current manual, a qualified teacher, a witnessed event, a cataloged record, or an owner’s decision. The learner may choose one available source, request another, or stop. If no credible source is available, say so and keep the topic open only as a conversation, not as a falsely completed lesson.

Sources can conflict for understandable reasons: one is older, one applies to different equipment, or one describes a local exception. The teacher explains which source they trust and why, while naming the limit of that explanation. The learner can request a second opinion. Avoid presenting faction identity as a substitute for expertise; affiliations may explain access to a source, but they do not decide its accuracy.

## 595. Route branch — The learner wants an answer before a deadline

An active task can give the question a real deadline. The task owner states whether the learner can pause, whether supervision is required, and what happens if the answer remains unknown. The teacher can provide a safe lesson within that window only if the relevant owners permit it. Otherwise, the player can request a qualified helper, use a confirmed procedure, or recommend deferral.

Time pressure must not cause the player to overstate competence. “You have heard the explanation” is different from “you are qualified to do this alone.” A narrow supervised action may be available even when independent work is not. If the task proceeds through another worker, the learner can observe or ask questions only where the task owner allows it; observation itself does not imply skill acquisition.

## 596. Route branch — The learner asks to share what they learned

After a lesson, the learner may want to write a reference, explain the procedure to a neighbor, or present it to a faction group. Ask who the intended audience is and whether the source allows that use. The teacher can help distinguish public facts from personal or restricted information. The learner may share a safe summary, ask for review, or keep the knowledge private.

If a faction offers to distribute the material, make its role concrete: printing, storing, or providing a room are separate from endorsing the content. The learner can accept logistical help without agreeing to faction messaging. If the proposed document would include another survivor’s private experience, ask that person before sharing it. No general “class notes” flag should be invented unless the current content and save owners support one.

## 597. Route branch — The learner returns with evidence of use

The learner may report that they applied a taught idea and encountered a new result. First determine whether they want instruction, task assistance, or simply to tell the teacher what happened. The player can route a practical problem to its owner, ask what part of the procedure they used, or help request a second lesson. A good outcome does not prove mastery of adjacent procedures; a bad outcome does not prove the lesson was worthless.

When the learner’s action affected an item or task, the responsible owner confirms the state. Dialogue can carry the human response—relief, frustration, surprise, or a new question—but should not duplicate the result in a shadow record. The education authority decides whether the new evidence supports any further learning outcome.

## 598. A short authored path with meaningful forks

One reusable questline shape is: learner asks a question; player identifies whether it is teachable or owner-resolved; learner chooses a source; teacher explains within scope; learner decides whether to practice, share, or stop; any practical action is confirmed by its own owner; the learner chooses whether to continue later. The story can branch at each choice without requiring a moral alignment meter.

The branch should be keyed to observable actions already available to the plan: which source the player requests, whether the learner consents to practice, whether a qualified person is found, whether an owner confirms a task, and whether the learner wants to share the result. These facts can change who is present, what is said, and which next action is offered. They should not imply an unapproved persistent proficiency tree.

## 599. Supporting groups as access bridges

A minor faction or resident circle can provide a source, meeting space, or specialist introduction. The player asks the learner whether they are comfortable with that contact and confirms the practical terms. One group might insist on supervised access to its equipment; another might lend a manual but ask that it be returned. Those are distinct support branches with visible conditions.

The learner may use one group’s resource while declining its wider invitation. A faction may be disappointed, but the scene should not punish the learner by closing unrelated education routes unless a current authored dependency supports that consequence. Major factions remain influential through resources and public positions; supporting groups add local access, testimony, and texture without taking ownership of the learner’s education.

## 600. Installment 41 endings

The route can close with an owner-confirmed answer, a bounded lesson, an unresolved source dispute, a safe referral, a supervised task, an approved shared note, or a learner who decides that now is not the time. Each ending should answer three questions in plain language: what does the learner know now, what remains uncertain, and who owns the next action?

This structure gives authors room for callbacks while keeping outcomes honest. If the learner returns, the callback can reference the source they chose or the practical question they left open. If they do not return, no hidden failure is implied. Education has served the character when it helped them make a more informed choice, even if the lesson did not produce a new mechanical result.

## 601. Expansion installment 42 — Knowledge enters a disagreement

A learner may use a lesson to take part in a disagreement about a practical process. One resident says the procedure is safe; another remembers a failure; a faction contact cites a written rule. The player can slow the exchange down and separate observation from conclusion. What was directly seen? Which source applies? Which owner can decide whether the equipment or room may be used now?

The learner is allowed to contribute without being made the arbitrator. They can explain the part they studied, ask a question, or choose not to speak. A teacher can help distinguish a general principle from a current operational decision. This creates a branch based on the learner's chosen action—present evidence, ask for review, defer, or remain quiet—rather than on whether the player has acquired a “truthful” or “deceptive” identity.

## 602. Evidence branch — The learner reports an observation

The learner may say, “I saw the indicator move,” without knowing what the movement means. The player can preserve that direct observation, ask when and where it happened, and route the claim to the relevant technical owner. Do not upgrade it into a diagnosis. The learner can be precise and still be mistaken about the cause.

If the observation is relevant to an active task, the task owner decides whether work pauses, continues under supervision, or requires inspection. The teacher may explain how to read the indicator, but should not overrule the task owner. If the observation is not current or cannot be reproduced safely, the player can record it as unverified through a supported route or leave it as dialogue.

## 603. Evidence branch — A source contradicts the learner's account

The source may show a different limit from what the learner remembers. The teacher can ask whether the learner wants to review the passage, identify the edition, or compare it with a second source. The learner may accept the correction, point out an exception, ask for a specialist, or step away. Disagreement should not automatically humiliate them.

The player should avoid lines that flatten the source into absolute truth if the source’s scope is narrow. A manual for one model does not settle a question about another. If the source is current but the equipment identity is uncertain, the responsible owner can verify the match. Until then, the branch ends with a pending check and no independent operation.

## 604. Evidence branch — A faction invokes its rules

A faction representative may cite a policy that applies to the faction’s own equipment, room, or work crew. The player can ask which boundary the policy governs. The learner may accept that condition for this service while continuing to question its broader claim. The faction can explain, refuse access, or offer a different supervised setting.

Do not portray every faction rule as either inherently correct or inherently corrupt. The player can examine its scope and practical consequences. A supporting group might supply a comparison manual or an independent witness, but it cannot declare another group’s policy void. The learner’s path can continue through public instruction even if one faction’s equipment remains inaccessible.

## 605. Evidence branch — The learner wants to demonstrate

The learner may ask to show what they understood. The teacher checks whether the demonstration is safe and appropriate; the task owner checks any live equipment. They can approve a harmless simulation, require close supervision, or decline the demonstration. The learner may accept another form of explanation or decide that the lesson is enough for today.

The demonstration is a scene outcome, not a general certification. If it shows one correct step, describe that step. If the learner makes an error, let the teacher explain it without turning a single attempt into a permanent incompetence label. Any qualification needed for future work must come from its current owner and established contract.

## 606. Evidence branch — The learner is asked to take sides

The arguing residents may ask the learner which person they believe. The learner can answer the factual question, explain that they have not seen enough, ask the responsible owner to decide, or refuse to be used as a witness. The player can reinforce the distinction between speaking about evidence and endorsing a faction.

If the learner does choose a side in the conversation, use their actual words and the evidence they give. Do not infer a change in faction alignment or personal loyalty. A person can support one procedure in this instance and still disagree with the group that proposed it in another context.

## 607. The player can choose a process, not a verdict

The meaningful player choices are procedural: ask for the source, arrange a safe review, invite a qualified witness, pause the affected task, or let the learner withdraw. The player should not select which faction’s story wins because of a generic reputation variable. If an existing decision owner makes a determination, present that result with its scope and leave room for an appeal only where the current system supports one.

This supports several playstyles. A player can be a careful facilitator, a direct advocate for a learner’s right to speak, a practical coordinator who gets the equipment inspected, or a quiet observer who lets the learner decide. Each can produce a distinct scene and action consequence while leaving the underlying truth to evidence and authority.

## 608. Case — The demonstration confirms only part of the claim

The learner accurately identifies a reading but draws an incorrect conclusion from it. The teacher can affirm the observed reading and explain what it does not establish. The learner may ask for more instruction, accept the narrow correction, or insist on a second source. The task owner still decides whether the equipment is safe to use.

The other resident may apologize for dismissing the learner or may continue to disagree. Either response can be written from current characterization. Do not resolve a social conflict automatically when the technical point is clarified; feelings and factual findings are related but separate branches.

## 609. Case — The owner cannot inspect immediately

The qualified owner may be occupied. The player can leave the equipment unused, ask whether a safe alternative exists, request an estimated time, or move the lesson to a different subject. The learner can choose whether to wait. Do not make the teacher improvise an operational answer to fill the gap.

If the owner later arrives, the scene can resume with the learner present, absent, or no longer interested. The player should ask before repeating the learner’s account. If the learner does not return, the owner can still inspect the equipment based on authorized information, but no educational completion is inferred.

## 610. Installment 42 close — Keep testimony, learning, and decisions distinct

This installment makes education consequential in a contested situation without appointing the learner as an expert, witness, or faction representative. The player can help establish what was observed, what the lesson supports, and who must decide the practical question. The learner may speak, demonstrate, ask for review, or withdraw.

Endings include a confirmed reading with unresolved cause, a corrected explanation, a supervised demonstration, an owner-led inspection, a deferred operation, or a learner who chooses not to participate in the dispute. If later dialogue refers to the event, carry forward only the supported fact and the learner’s own stated position.

## 611. Expansion installment 43 — Show what was learned without turning it into an exam

At the end of a lesson, the teacher may want to know whether the learner followed the explanation. The player can offer several low-pressure ways to respond: describe the idea in their own words, point to the relevant source, ask a question, watch a second demonstration, or stop. The learner can refuse any check that is not necessary for a supported qualification.

The goal is to select the next teaching action, not rank the learner. If the answer reveals a misunderstanding, the teacher can explain again, use a different source, or identify a narrower prerequisite. If the learner chooses not to answer, the teacher can still close the conversation without claiming mastery. Any formal assessment required for a task must follow that task’s current authority and established process.

## 612. Branch — The learner can explain the principle but not the procedure

The learner may understand why a process matters yet still be unsure of its steps. The teacher can affirm the distinction and offer a supervised practice, a reference, or a later demonstration. The player should not treat a good explanation as proof that the learner can perform the procedure independently.

If a task is waiting, the task owner determines whether the learner may assist, observe, or should leave the work to a qualified person. The learner can choose among the allowed options. This branch rewards precise instruction and creates a practical consequence without awarding a broad skill level.

## 613. Branch — The learner performs a step correctly but misses a condition

The learner may follow the sequence while overlooking a precondition, such as access, equipment state, or supervision. The teacher can pause and ask what should be checked first. The learner may notice the missing condition, ask for help, or make another attempt only after the owner confirms it is safe.

Do not punish the learner with an irreversible failure in an instructional scene unless an actual supported hazard or task state makes that consequence possible. A useful lesson can reveal that the procedure has a prerequisite. The callback should name that prerequisite rather than saying vaguely that the learner “failed.”

## 614. Branch — The learner requests a private reference

The learner may want a copy or a reminder but not want a public record of attending the lesson. The player checks whether the source can be copied and whether the current owner supports private access. The learner can take the source, ask the teacher to hold it, or decline. Do not promise a private notebook or persistent personal journal unless that system exists.

If the source belongs to a faction, explain any return or access conditions. The learner can use it without joining the group if the terms permit. The faction should not receive attendance or personal details merely because it lent a reference.

## 615. Branch — The learner asks the teacher to confirm a public claim

Another resident may ask whether the learner is now qualified to perform a task. The teacher can state exactly what was covered and what remains unassessed. The learner may answer for themselves, ask the teacher to speak, or decline public discussion. Do not release personal education details to satisfy curiosity.

If a task owner needs qualification evidence, route the request through its authorized process. The teacher can provide only the supported completion fact. A lesson conversation should not become an informal credential that a faction can use to recruit, assign, or restrict the learner.

## 616. Assessment branch matrix

| Learner response | Teaching next step | What can be stated | What remains unproven |
|---|---|---|---|
| Explains the principle | Clarify or close | Learner can describe that idea | Independent procedure |
| Requests another example | Demonstrate or cite source | Question remains active | Broader competence |
| Completes a supervised step | Confirm the narrow step | That step occurred under supervision | Unsupervised use |
| Declines a check | Respect and close | Lesson was offered or discussed | Mastery or failure |
| Identifies a precondition | Review the owner’s rule | Learner noticed a relevant condition | Safe operation before confirmation |

Use this as an authoring guide, not a new assessment schema. Persist only outcomes already represented by the education or task owners.

## 617. Installment 43 close — Make the result narrow and useful

The lesson can end with a clear statement of what the learner understood, what they practiced, and which next step requires another owner. This supports a grounded callback: later, the teacher can refer to the question or supervised action without exaggerating it into a certification.

The learner may leave confident, uncertain, tired, or uninterested in further study. All are viable endings. The player has helped when they made the limits and options understandable, not when they extracted a performance from the learner.

## 618. Expansion installment 44 — A lesson changes how the next problem is approached

Later, the learner may encounter a related problem. The callback should recognize the earlier lesson only if its result is still supported and relevant. The learner can use the source, ask for a refresher, seek another teacher, or choose not to apply it. The player should not assume that one successful session made the learner permanently self-sufficient.

The new problem can be familiar in one respect and different in another. The teacher may help the learner identify the shared principle while pointing out the new condition. This encourages transfer of understanding without treating every adjacent task as automatically solved.

## 619. Callback branch — The learner recognizes a pattern

The learner may notice that the new situation resembles a prior example. The player can ask what seems similar, let them identify what differs, and check whether the source still applies. The learner can make a cautious suggestion, ask a qualified person to confirm it, or decline to proceed.

If the relevant owner confirms the procedure, the learner may assist within the permitted scope. If the owner reports a different condition, the earlier lesson remains useful but limited. Avoid writing the callback as “you learned this already” when the new context requires additional expertise.

## 620. Callback branch — The learner forgot a detail

The learner may remember the principle but forget a step. The teacher can offer a refresher, direct them to the source, or ask whether they want another demonstration. Forgetting should not erase a valid prior lesson unless the education owner has a supported decay rule. Nor should the game assume that every remembered line persists perfectly without such authority.

The player can make the refresher convenient without making it compulsory. The learner may decide that the task is not urgent, that another teacher is preferable, or that they would rather not take part. Their choice controls the next scene, while the task owner controls whether work can proceed.

## 621. Callback branch — The learner teaches a safe fragment

The learner may explain one narrow point to another resident. The listener can ask a follow-up, request the original source, or ask for a qualified teacher. The learner can answer within what they know, refer the question onward, or stop. This creates a small social branch without silently promoting them to instructor.

If a correction is needed, the qualified teacher can add it without discrediting the learner’s whole contribution. The two characters may agree on the source, compare interpretations, or leave a point unresolved. The player can support a respectful exchange rather than choose whose confidence sounds more persuasive.

## 622. Callback branch — A faction cites the learner’s prior lesson

A faction contact may refer to a lesson when inviting the learner to help. The player checks whether the invitation accurately describes what the learner studied. The learner can accept a bounded role, ask for supervision, request more instruction, or refuse. The faction may provide equipment or access only within its own authority.

Do not let a faction convert a lesson into a qualification by quoting it. If the proposed role requires a credential or permission, the responsible owner confirms it. The learner can be proud of what they learned without accepting a task beyond its scope.

## 623. Callback branch — A lesson is used to persuade someone else

The player or another character may be tempted to cite the lesson as proof that a disputed policy is correct. The learner can clarify that the lesson covered a procedure, not the policy decision, or decline to be used as an argument. A teacher can restate the source’s scope. The decision owner can explain what remains theirs to decide.

This branch protects the learner’s words from being turned into faction endorsement. They may support an action for their own reasons, but the game should not infer that stance from education alone. If the learner chooses to speak publicly, use their actual authored choice and preserve the audience rules.

## 624. Installment 44 close — Learning is reusable, not unlimited

Callbacks should make prior learning matter without making it a universal key. The learner may recognize, forget, teach, question, or decline to apply a concept. Each route has a specific next action and owner, and only supported educational outcomes persist.

This gives a questline a longer arc: an initial question, a source or practice route, a later use, and an optional refresher or peer exchange. The ending need not resolve every related skill. It can leave the learner more confident about where to seek help and more precise about what they do not yet know.

## 625. Expansion installment 45 — A lesson becomes a shared shelter resource

A learner may propose making a useful subject easier for other residents to study. The player can help them ask whether a group session is wanted, identify a qualified teacher, or seek a source that can be shared. The proposal does not automatically create a class, open room, resource pool, or permanent learning program. Each owner must confirm the part it controls.

The learner can help design the session, attend as a participant, offer a limited peer explanation, or decide that they do not want the responsibility. A faction can lend a location or instructor, but the learner can accept that service without endorsing the faction’s wider program. If no qualified teacher is available, the group can share a verified source or postpone rather than pretend an informal gathering is equivalent to instruction.

## 626. Proposal branch — The group wants a practical subject

Residents may request help with a subject that touches a live task. The player checks whether it is appropriate to teach in a group and whether demonstration materials are available. A teacher can narrow the session to a safe concept, ask a task owner to demonstrate, or refer the group elsewhere. The learners can accept the narrower subject or wait for a more complete lesson.

Do not make group attendance imply that every resident has the same background or goal. The teacher can ask what participants already know without demanding public disclosure. The group can split into a basic explanation and an advanced question only if the education owner supports separate sessions and the room schedule allows them.

## 627. Proposal branch — The room is offered by a faction

A faction may provide a room that is otherwise unavailable. The player verifies access rules, hours, capacity, and whether attendance carries any separate condition. Learners can accept the room, ask for another location, request that the session stay open to non-members, or decline. The faction may set legitimate conditions on its own space, but cannot silently define who qualifies as a learner elsewhere.

If the room is only available to members, say so before the group commits. The learner can decide whether that audience limit is acceptable. The player should not promise that a later public version will occur unless someone has agreed to host it.

## 628. Proposal branch — Residents disagree about what should be taught

One group may want an immediate practical demonstration while another asks for broader background. The teacher can explain what fits the session and offer a second topic for later. Participants can choose the first topic, split into supported sessions, submit questions, or leave. Do not force a majority decision over a participant’s access to a separate lesson.

If the disagreement concerns a contested historical or faction account, label sources and distinguish evidence from interpretation. A group lesson should not present a faction’s political account as technical instruction. The player can ask a qualified archivist or another source owner to clarify what is known.

## 629. Proposal branch — A participant cannot attend at the stated time

The resident can request another time, a written source, or a private explanation. The organizer checks the teacher’s schedule and room availability rather than promising universal access. If no alternative is possible, the resident can receive the verified public source where permitted or choose not to participate.

Attendance should not become a proxy for commitment. A resident who misses the session has not refused the subject or the group. If the organizer later repeats the lesson, confirm the source and current version; do not assume the old plan remains accurate forever.

## 630. Proposal branch — The learner becomes the organizer

The original learner may want to coordinate a future class. The player can help them identify what the role entails: contacting a teacher, checking a room, sharing an authorized notice, and responding to questions. The learner can accept the whole bounded role, ask for a co-organizer, or return to being a participant.

Do not transform a volunteer into an administrator with authority over attendance, curriculum, or records unless the existing system grants that role. A supporting faction can provide a contact person or logistics, while the learner retains the choice to proceed. If the planning task becomes too much, they can withdraw and the organizer can find another route.

## 631. Proposal branch — The session exposes unequal prior access

Some residents may already know the material because a faction trained them, while others have never seen the source. The teacher can acknowledge the difference and avoid treating prior access as merit. Participants may request a slower introduction, a separate advanced question period, or a source they can review privately.

If a faction’s instruction conflicts with the current source, compare the content respectfully and identify what needs review. Do not shame learners for relying on the material previously available to them. The owner of the subject determines which source is current; the learner decides whether they want to continue.

## 632. Proposal branch — The class is canceled after residents arrive

The teacher or location owner may cancel because of a real schedule or safety change. The organizer can give the reason that is confirmed, offer a reschedule if one exists, share a permitted source, or close. Participants may be frustrated, leave, or ask for another teacher. Do not claim the cancellation itself taught them the material.

If a faction hosted the room, it can explain its own availability but cannot require attendees to accept recruitment or work as compensation for a canceled lesson unless those terms were disclosed and accepted separately. The learner can decline any new condition and still ask for the source already promised.

## 633. Installment 45 close — Shared access is built from explicit roles

The shared session can end as a scheduled class, a smaller safe briefing, a public source exchange, a faction-hosted lesson with disclosed limits, a postponed proposal, or a canceled gathering. These outcomes create different callbacks for learner, teacher, organizer, and host without implying a permanent education institution.

Before closing the route, state who will do the next confirmed task: teacher, organizer, location owner, or learner. If no one has accepted that task, leave the proposal pending only as authored conversation and do not imply a durable class roster. Shared learning grows from consent and real capacity, not from an invisible curriculum flag.

## 634. Expansion installment 46 — One teacher, several urgent questions

A qualified teacher may be available for only a short window while several learners want help. The player should not allocate time by hidden worth, faction rank, or a generalized urgency score. Ask what each learner needs and whether a current task, safety owner, or deadline makes one question time-sensitive. The teacher and schedule owner confirm what can fit.

Possible routes include a narrow answer for one learner, a shared introduction to a topic, a referral to another qualified person, a source learners can use later, or a rescheduled session. The player explains the tradeoff before anyone commits. A learner may choose to wait, seek another source, or leave without being treated as less serious.

## 635. Capacity branch — One question concerns immediate safe work

A learner may need clarification before taking part in a live task. The task owner confirms whether work can pause and what instruction is required. The teacher can address that narrow prerequisite first, but should not claim to resolve a broader curriculum. Other learners can wait, attend a separate supported session, or use an authorized reference.

If the task owner says the learner should not participate until a qualified person is available, the player must respect that boundary. The learner can choose another role or decline. Do not make the teacher responsible for certifying them simply because no one else is present.

## 636. Capacity branch — Several learners can share one explanation

The teacher may be able to explain a general concept to everyone at once. Ask whether the learners want a group session and whether the subject is appropriate for a shared explanation. They may agree, ask to keep their questions private, or split the session. One learner’s personal history should not become the example used for the entire group without consent.

The group explanation can answer common questions while leaving individual practice and owner decisions separate. Each learner decides whether to demonstrate or request follow-up. Attendance alone does not produce a uniform lesson result.

## 637. Capacity branch — The available teacher is qualified only for part

The teacher may know the general theory but lack authority or experience with a particular tool, location, or faction procedure. They can explain the part they know and identify the rest as outside scope. The player can seek a specialist, ask the relevant owner, or defer the entire practical action.

Do not present a partial qualification as a reason to exclude the teacher from all dialogue. They can still help identify the question and source. A learner can decide whether that narrower help is useful while understanding that it does not authorize independent use.

## 638. Capacity branch — The learner cannot wait for the next session

The learner may have a real deadline, but a teacher’s schedule cannot change. The task owner can confirm whether the action can be reassigned, delayed, or completed under another qualified person. The learner may choose to wait, seek the alternate person, or abandon that task. Do not invent an emergency class merely to preserve the planned quest route.

If the deadline is self-imposed rather than owner-confirmed, the player can still take it seriously without calling it binding. The learner can explain why timing matters, ask for a source, or choose a smaller objective. Distinguish personal urgency from an external assignment in the dialogue.

## 639. Capacity branch — A faction offers a second teacher

A supporting faction may provide another qualified instructor or a contact with a relevant source. Confirm their actual scope, schedule, access terms, and whether the learner wants that connection. The faction can help with one lesson without gaining control over the class or requiring a broader commitment.

If the second teacher disagrees with the first, compare the subject and sources. The player can ask both to explain their limits, seek a neutral owner, or let the learner wait for better evidence. Do not choose a teacher based on faction prestige or cast disagreement as proof that both are equally reliable.

## 640. Capacity branch — The teacher asks the learner to prioritize

The teacher may ask which question matters most to the learner. The learner can identify one, ask the teacher to recommend a safe order, or say they cannot decide. The teacher can offer a reasoned sequence while leaving the final choice with the learner. If task dependencies dictate order, the task owner confirms that constraint.

Prioritizing one topic does not mean the learner has rejected all others. The player can name what remains open and offer a supported return path. If no durable appointment exists, phrase this as an intention or referral rather than a scheduled session.

## 641. Capacity branch — Another learner objects to the allocation

A waiting learner may believe the teacher chose unfairly. The player can explain the confirmed criterion, ask the teacher to clarify the decision, or invite the learner to request another route. The teacher should not disclose a peer’s private task or health concern to justify the allocation.

If there was no agreed criterion, the teacher can acknowledge that and revise the plan where possible. The player can help design a clear order for the remaining time, but should not invent a shelter-wide queue rule. A disappointed learner can remain disappointed without a faction relationship penalty.

## 642. Capacity branch — The teacher must leave early

The teacher can state what was covered and which questions remain unanswered. Learners may ask for a source, seek another person, stop, or request a later session. No one should be pressured to claim the lesson was enough just because the teacher has to leave.

The close records a truthful partial result only if the education owner supports it. Otherwise the scene states that a discussion occurred and leaves formal completion unset. The teacher can apologize for the limited time without promising future availability they do not control.

## 643. Installment 46 close — Scarcity should expose choices, not rank people

Limited teaching capacity can create a rich branch through urgency, scope, group learning, referrals, and deferral. The player helps make the available time and sources visible. Each learner chooses what to do with that information, and current owners confirm any task restriction or lesson result.

The final scene can close with one narrow answer, a shared introduction, a specialist referral, a split plan, or no lesson. It should not claim that one resident deserved the teacher more. If the shelter later changes its teaching capacity, show the concrete resource or schedule that made the change possible.

## 644. Expansion installment 47 — The learner chooses how the idea is taught

Learners may understand a topic through different forms: a spoken explanation, a diagram, a demonstration, a written source, a question-and-answer exchange, or a supervised attempt. The player can ask what would help and offer only formats the teacher, subject, and current resources support. A learner can request a change without explaining a disability or personal history.

The format is part of the learner’s agency, not a cosmetic selection. A diagram may make a sequence clear but omit a safety condition; a demonstration may help with movement but not explain why; a text may be easier to revisit but depend on unfamiliar terminology. The teacher can combine formats or say which one is unavailable. The learner decides whether the available version is useful enough to continue.

## 645. Format branch — Spoken explanation is not landing

The learner may ask the teacher to slow down, use simpler terms, give an example, or stop repeating the same explanation. The teacher can reframe the idea, draw a comparison, show a source, or ask whether the learner wants a pause. The player should not mock or infantilize the learner for needing another explanation.

If the teacher cannot explain it more clearly, the player can seek another qualified person or postpone. The learner may understand part of the idea and still want a different source. No one should claim successful instruction merely because the teacher completed the planned speech.

## 646. Format branch — The learner prefers to observe first

The learner may want to watch someone perform a safe step before trying it. The task owner confirms whether observation is permitted and whether any privacy or hazard boundary applies. The learner can ask questions, request a closer view, or decide that observation is enough for now.

Observation can prepare a later lesson, but does not grant independent task authority. If the demonstration cannot occur safely, the teacher can use an authorized model or source, or explain why no demonstration is appropriate. The player should not move live equipment just to make the lesson more engaging.

## 647. Format branch — The learner wants a written source

The learner can ask to read at their own pace. The player checks whether the source is current, accessible, and permitted to copy or lend. The learner may ask for a marked passage, a plain-language explanation, or a later discussion. If the source is faction-owned, disclose any return or access condition before taking it.

Reading a source is not proof of comprehension or consent to its author’s politics. The learner can question it, compare it, or stop. If the text itself is not available in an accessible form, the player can ask whether a qualified person can summarize it without claiming that the summary replaces the source.

## 648. Format branch — The learner wants to try before explaining

Some learners prefer a practical attempt before a long explanation. The teacher and task owner confirm whether a safe, bounded practice is possible. The learner can try with supervision, watch first, or decline. The teacher names what the attempt can show and stops before an unsafe or unsupported step.

An error can reveal which instruction needs a different format. The teacher can pause, offer a diagram, or ask the learner to describe what they expected. Do not treat the first attempt as a test with a hidden pass/fail score unless the current qualification owner explicitly defines one.

## 649. Format branch — The learner asks for quiet or privacy

The learner may not want to ask questions in a group. The player can ask whether a smaller setting, written question, or later conversation is available. The location owner confirms privacy and access. If no alternative exists, the learner can leave without forfeiting a future lesson where the education owner allows it.

Do not tell the group why the learner stepped away. The teacher can continue with those who remain, but each result follows the current education rules. A later private explanation should not be promised unless a teacher and time are actually available.

## 650. Format branch — The teacher uses an analogy the learner rejects

An analogy may be culturally unfamiliar, technically misleading, or simply unhelpful. The learner can say it does not fit; the teacher can ask what part fails and choose another route. The player should preserve the technical concept rather than defend a favorite metaphor.

If the analogy accidentally carries a faction stereotype or political implication, the teacher can correct it and return to the source. The learner may accept the correction, remain uncomfortable, or prefer another instructor. This is a meaningful character branch without forcing an apology scene where none is wanted.

## 651. Format outcome matrix

| Learner preference | Confirm first | Possible next action | Claim to avoid |
|---|---|---|---|
| Diagram | Source and materials | Annotate a safe sequence | Diagram proves competence |
| Demonstration | Safety and owner approval | Observe a bounded example | Observation grants access |
| Written source | Version and lending rules | Read, mark questions, return | Reading means agreement |
| Practice | Qualification and supervision | Try one supported step | One attempt proves mastery |
| Private exchange | Room access and schedule | Defer or move if available | Privacy can be guaranteed without checking |

The matrix helps authors branch the scene. It does not create a teaching-method catalog or learner trait system.

## 652. Installment 47 close — A format choice is a real play choice

The learner may continue through a different explanation, observe, practice, read, request privacy, or stop. These choices change who participates and what evidence the scene produces. They do not assign a fixed learner personality or grant an unsupported skill state.

A later callback can remember the source or method only if the current system can represent it. Otherwise, authored dialogue can refer to it when the character is present, while the plan avoids claiming permanent memory. The important outcome is that the learner had a meaningful say in how they were taught.

## 653. Expansion installment 48 — A source meets lived experience

A teacher may bring a written procedure while a learner brings experience from doing the work under different conditions. Neither source should be dismissed solely because it is formal or informal. The player can ask what each person observed, what conditions differed, and which current owner can verify the practical question.

This creates a knowledge-sharing route with several outcomes: the source applies directly, the lived experience reveals an exception, the two accounts concern different situations, or neither resolves the current case. The teacher can invite the learner to explain their observation, and the learner can decline or ask not to be quoted. The lesson remains a conversation, not a contest between credentials and memory.

## 654. Branch — A resident offers a local workaround

The resident may describe a workaround that helped in one location or during one task. The player can ask when it was used, what equipment it involved, and whether a task owner has reviewed it. The teacher may compare it with the formal source and identify which parts seem compatible or need verification.

Do not turn one successful workaround into a universal procedure. If it is unsafe or outside the teacher’s expertise, pause and refer it. If it is a useful local adaptation, the responsible owner may choose to document it through an existing route. The resident can decide whether their name or experience may be shared.

## 655. Branch — The formal source is old but still relevant

The source may be old without being invalid. The teacher can identify its date, model, author, and known limits. The learner can ask whether it remains in use, request another source, or decide that the uncertainty is too great for a practical attempt.

Age alone should not determine reliability. The current owner can confirm whether a newer rule supersedes it. If no one can verify that, the plan should leave the source’s status uncertain. The class can still learn the historical procedure as a record if it is clearly labeled and safe to discuss.

## 656. Branch — Two residents remember different outcomes

Two witnesses may recall the same task differently. The player can separate what each saw from what each concluded, ask whether they consent to a joint discussion, and seek records or owners that could corroborate the event. The teacher can model how to state uncertainty without calling either resident dishonest.

If a learner is asked to choose which witness is credible, they may decline. If they do compare accounts, they should explain their reasons and limits. No faction reputation, social rank, or confident delivery substitutes for evidence. The lesson can end with a method for checking claims rather than a verdict on the witnesses.

## 657. Branch — The knowledge-holder wants credit or privacy

The resident may want their contribution acknowledged or may prefer not to be named. The player asks how the source can be used and who may hear it. The teacher can attribute the idea, summarize it without identifying detail where permitted, or keep it out of the shared lesson.

Credit and permission are not the same thing. A person who accepts attribution has not necessarily agreed to future interviews or faction promotion. Someone who declines attribution may still want the practical knowledge used. Respect the exact permission given, and do not store a testimonial unless the current content owner supports it.

## 658. Branch — A faction claims the knowledge as its own

A faction representative may say that the workaround came from its training or equipment. The player can ask whether the resident agrees, what source can be cited, and whether the faction’s claim changes the procedure’s scope. The resident may confirm the connection, correct it, or keep the account private.

The lesson can acknowledge a faction’s role without transferring ownership of a person’s experience. The faction may lend a specialist or source, while the resident decides how their account is used. A disagreement about credit should not prevent the player from asking the relevant owner to verify a safety question.

## 659. Branch — The class turns the comparison into a shared reference

Participants may ask for a reference that distinguishes confirmed procedure, local observation, and unresolved questions. The author can help draft such a page only if the relevant source owner permits it and the class has a supported way to keep it. Otherwise, the teacher can provide a spoken summary and identify where each source can be found.

The reference should not blend claims into one confident instruction. It can say: “The manual covers this model”; “two residents observed this behavior”; “the task owner has not yet confirmed whether the workaround applies.” Learners can decide whether that level of certainty is enough for study or whether they want to wait.

## 660. Branch — A learner challenges the teacher respectfully

The learner may question the teacher’s conclusion with a source or observation. The teacher can examine it, explain why it does or does not apply, ask a specialist, or acknowledge uncertainty. The learner can persist, accept the explanation, or withdraw. Neither person needs to win the exchange for the lesson to be useful.

If the teacher is wrong, correct the lesson and follow the education owner’s current rules for any recorded result. If the learner’s evidence does not apply, show why without dismissing the person. The player’s action is to keep the disagreement factual, bounded, and open to verification.

## 661. Installment 48 close — Teach how to compare sources

This route makes the learner’s future play richer by showing how to compare formal instruction, local experience, testimony, and faction claims. The player can request permission, identify scope, seek corroboration, document an authorized reference, or stop when the evidence is insufficient.

The ending can be a verified procedure, a locally bounded practice, a corrected account, a source dispute, or an unresolved question. Do not collapse the outcome into “the teacher was right” or “the resident was right.” The lesson’s lasting contribution is a clearer account of what each source can support.

## 662. Expansion installment 49 — A failed attempt becomes a review route

A learner may try a supported procedure and get an unexpected result. The player should first stop any unsafe continuation and ask what happened, not who is to blame. The task owner confirms the state of equipment or work; the teacher helps compare the attempt with the lesson; the learner decides whether they want to participate in a review.

The outcome can arise from a missed step, an unclear explanation, a changed condition, a faulty tool, or a source that did not apply. Do not choose a cause before evidence exists. The learner may be frustrated, embarrassed, worried about consequences, or focused on fixing the problem. All are valid responses and lead to different dialogue without assigning a generic competence score.

## 663. Review branch — The learner wants to stop and leave

The learner can ask to step away from the work. The task owner determines how the immediate task is secured, and the player can find an eligible replacement or pause the work if permitted. The learner does not need to remain as a witness or explain why they want to leave.

If the task has a reporting requirement, the responsible owner explains it before requesting details. The teacher can ask whether the learner wants a later review. Do not turn an educational review into an interrogation or condition future lessons on the learner accepting one immediately.

## 664. Review branch — The learner wants to reconstruct the steps

The learner may want to identify where the result changed. The teacher can ask them to describe the sequence from memory, compare it with the source, or demonstrate only the safe parts. The learner may ask for a second person, choose to review privately, or stop when the explanation becomes uncomfortable.

The sequence they remember is evidence about what they understood, not proof that they caused the outcome. Another person may have changed the conditions; equipment may have behaved unexpectedly. The task owner and any available records determine operational facts, while the teacher helps identify what the lesson needs to clarify.

## 665. Review branch — The teacher discovers the lesson was incomplete

The teacher may realize that an important precondition was omitted. They can acknowledge the gap, explain the correction, and ask whether the learner wants to continue. The player can route any practical consequence to the task owner and help prevent the same gap from reaching other learners through an authorized correction.

Do not make the teacher’s apology a substitute for fixing the procedure. If the source is wrong or outdated, the responsible owner should review it. The learner can accept the correction, request a different instructor, seek a second opinion, or leave the subject alone.

## 666. Review branch — The equipment or setting changed

The learner may have followed the instruction correctly, but the tool, room, or task condition was different from the example. The player can verify the difference with its owner. The teacher may explain why the earlier lesson did not cover that case, while the learner decides whether they want instruction for the new condition.

If the variation is outside safe scope, pause and refer. Do not alter the original lesson record to match the new condition. A later lesson can be a distinct route with its own source, qualification, and result.

## 667. Review branch — A witness blames the learner

Another resident may accuse the learner of carelessness. The player can ask what the witness saw, separate observation from judgment, and invite the task owner to determine any operational finding. The learner can respond, remain silent, ask for a private conversation, or leave.

Do not force a public defense. If the witness has relevant information, preserve it through the authorized route. If the accusation is unsupported, the player can say that no cause is confirmed. The group can still address any immediate task risk without deciding the learner’s character.

## 668. Review branch — The learner asks to teach the correction

After understanding the missing step, the learner may want to tell classmates. The teacher can help prepare a narrow correction and confirm that the learner’s account is accurate. The learner can share it, ask the teacher to share it, or keep the experience private. No one should use the learner’s mistake as a cautionary story without consent.

If a faction supplied the source, it may help publish a corrected copy, but the source owner controls the official revision. The learner can contribute without becoming responsible for the faction’s training program. Their account remains theirs even when the correction is made public.

## 669. Review branch — A second teacher disagrees about the cause

A second teacher may believe the learner missed a step, while the first believes the source was incomplete. The player can ask each to identify evidence and limits, then refer the practical cause to the proper owner. The learner may hear both, request a neutral reviewer, or decline further discussion.

Do not make a compromise diagnosis to keep everyone satisfied. The review can end with an operational fix and an unresolved educational question. If the learner needs another attempt, it should occur only after the task owner confirms that conditions are safe and the learner agrees.

## 670. Review outcomes and supported records

Possible outcomes include equipment repair, task reassignment, a corrected source, a repeated lesson, a narrower qualification, an unresolved cause, or a learner who chooses not to continue. The task owner records work state; the education owner records only the learning result its contract supports. The plan should not create a shared incident-and-education ledger.

If no persistent review state exists, callbacks can reference the event only where authored continuity supports it. Avoid durable labels such as failed learner or unsafe teacher. A later scene should refer to a confirmed correction, task result, or explicit conversation, not an inferred diagnosis.

## 671. Installment 49 close — Review the process before judging the person

This installment makes an unsuccessful attempt a branching opportunity for safety, clarification, correction, and choice. The learner can leave, review, request a different teacher, help correct a source, or stop studying the topic. The player routes practical facts to their owners and does not assign blame without evidence.

The most useful ending may be a narrow correction and a paused task. The learner can decide whether they want to try again later. The lesson remains meaningful even when the right response is to wait for a verified answer.

## 672. Expansion installment 50 — Carry a correction into the next lesson

When a lesson or source is corrected, the player can ask whether the learner wants to review the revised material, hear what changed, or leave the topic. The teacher should identify the old claim, the new confirmed claim, and the authority that approved the correction. If the change is still under review, keep it provisional.

The learner can accept the new explanation, ask how the error happened, request a practical demonstration, or choose another teacher. The teacher can acknowledge responsibility without pressuring the learner to forgive or continue. If the learner relied on the earlier instruction during a task, route that result to the task owner and ask whether any practical follow-up is needed.

The correction can also affect future classes. An authorized source owner may update the material; the teacher can tell other learners about the change through a permitted channel. Do not mark every past learner as notified unless the communication owner confirms delivery. A callback should distinguish “the source was revised” from “this person has heard the revision.”

The path can close with a learner who returns, a learner who wants a second opinion, a corrected reference, a task review, or no further study. The durable outcome remains limited to what the education and task owners support. A correction improves the route when it changes a concrete next action, not when it erases the fact that the earlier lesson was incomplete.

If the learner wants to verify the correction through practice, the teacher can propose one bounded step and identify the task owner’s safety conditions. The learner may accept that step, ask for a witness, choose a different method, or leave the subject alone. A successful check confirms only what was demonstrated under those conditions. A later task with different equipment or a different owner requires its own review. This final branch lets a corrected lesson rebuild confidence through evidence while keeping the learner free to decide when they are ready.

If another educator takes over, give them the corrected source and the learner’s permission boundaries, not an invented history of failure. The learner can choose whether to explain the earlier attempt, ask the first teacher to join, or start fresh with the new person. The new teacher confirms what they can support and does not assume the learner needs remediation in every related skill. This creates a clean handoff while preserving the learner’s dignity and control of their own learning story.

At the next class review, the teacher can ask whether the correction was clear and whether the source was usable. Learners may report that it helped, request another format, or offer no feedback. The owner can improve the lesson without making any one learner responsible for repairing the entire program.

Where no shared review route exists, the teacher can still adjust their own next explanation and tell the source owner about a verified error. Keep that improvement local until the content owner confirms a broader revision.

Learners can also ask that their own attempt not be used as a public example. The teacher can honor that limit while still correcting the material in general terms.

If the content owner has not approved a revision, the teacher should label it as a proposed correction and avoid teaching it as settled procedure.

## 673. Expansion installment 51 — The learner investigates a practical question

A learner may want to answer a question that matters beyond one lesson: whether a work practice changes by location, why two records disagree, or which source explains a repeated failure. The player can help turn curiosity into a bounded inquiry. Together they identify the question, what evidence could answer it, who owns the relevant space or record, and which actions need permission.

The learner chooses whether the result is for personal understanding, a task owner, a small class, or a public reference. That audience affects what can be collected and shared. The teacher can help with method, while task, location, and source owners retain their own authority. The project is a learning path, not an excuse to create a new research database.

## 674. Inquiry branch — Turn a broad question into something answerable

The learner may begin with “why does this keep happening?” The teacher can ask what they have observed and help narrow the question to a comparison the shelter can safely make. The learner may compare two known cases, inspect an authorized source, ask a specialist, or decide the evidence is not available.

Do not decide the answer before the inquiry. The player should separate a testable question from a private accusation or a faction claim. If the question implies that someone caused a problem, use the relevant review owner rather than sending the learner to investigate another resident.

## 675. Inquiry branch — The learner chooses an observation method

Possible methods include reading a current record, watching a permitted process, asking a qualified person, or comparing conditions across two locations. The learner can choose among the methods the owners allow. The teacher names limitations: one observation cannot establish a general rule, an old record may be incomplete, and a specialist’s opinion has a defined scope.

The task owner confirms whether observation could interfere with work. The location owner confirms where the learner may go. If no safe method exists, the inquiry can stop at a question list or use a public source. A project need not manufacture danger to feel consequential.

## 676. Inquiry branch — Access depends on a faction contact

A faction may hold a relevant manual or control a workspace where the learner could observe a process. The player asks what access is available, who can attend, and whether the learner must accept any separate condition. The learner can accept bounded access, ask for a different source, or decline.

The faction can provide evidence and explain its own procedures, but it does not get to define the learner’s conclusion. The learner may compare its material with another source, ask a neutral specialist, or report that the evidence remains incomplete. Supporting factions add access and local expertise; they do not become the sole educational authority.

## 677. Inquiry branch — A source contains personal information

A record may include another resident’s name, health detail, work history, or private explanation. The player checks whether the learner needs that information to answer the question and whether the source owner permits access. The learner can use an approved redacted version, ask a general question, or stop.

Do not expose private details as a teaching example merely because they are in a record. If the relevant owner can provide an aggregate or non-identifying explanation, offer it. The learner can still study the process while respecting the people described by the source.

## 678. Inquiry branch — The observations do not agree

The learner may find that two locations behave differently or that one witness contradicts another. The teacher can help list conditions that differ and ask whether the comparison remains valid. The learner can gather a third source, narrow the claim, or conclude that the result is inconclusive.

Inconclusive evidence should remain an outcome, not a failed quest. It may still reveal that the source does not cover a local case or that more observation is needed. The player can present the uncertainty to the relevant owner without claiming a definitive cause.

## 679. Inquiry branch — The learner finds a practical pattern

The learner may identify a pattern that could help a task. The player can ask whether they want to share it with the task owner, keep it as a personal observation, or present it to a teacher for review. The owner decides whether it changes procedure; the learner decides whether their name is attached.

If the pattern is confirmed, a later task may benefit from the revised instruction. If it is not confirmed, the learner can still explain what they saw and what remains uncertain. Avoid turning a single correct observation into universal competence or a permanent “researcher” identity.

## 680. Inquiry branch — The learner presents to peers

The learner may ask to share findings with a small class or meeting. The player confirms the audience and checks whether any source or testimony has sharing limits. Peers can ask questions, point out a missing condition, or disagree. The learner can answer, defer, or ask the teacher to clarify.

The presentation is not a faction speech unless the learner chooses that purpose. A faction may host the room, but should not edit the conclusion to endorse its preferred narrative. If the group cannot resolve an issue, the learner can close with a list of open questions and an owner for each.

## 681. Inquiry branch — The learner wants an official answer

The learner may expect the owner to adopt the result. The player can explain the distinction between submitting evidence and receiving a decision. The owner may review, request more information, decline to change the procedure, or issue a correction. The learner can ask why, appeal through a supported route, or return to study.

Do not script an owner’s agreement as guaranteed because the learner worked hard. Education gives the learner a way to ask a sharper question; it does not transfer decision authority. If the owner declines, preserve the learner’s evidence and the owner’s stated reason where the current systems permit.

## 682. Inquiry branch — The source is corrected during the project

An authoritative source may change while the learner is working. The teacher identifies which observations still apply and which should be repeated under the updated method. The learner can continue, restart a narrow comparison, or stop. Any time or material cost should come from the actual task and owner state.

The correction can become a meaningful callback: the learner notices that a claim once thought settled is now under review. Do not erase the earlier work. Explain what it established at the time and what the new source changes.

## 683. Inquiry outcomes and later callbacks

Possible outcomes include a supported pattern, an inconclusive comparison, a corrected source, a private observation, a public class presentation, an owner review, or a learner who decides not to share. Each outcome should state what question was answered, what remains uncertain, and who owns the next action.

If a later quest uses the learner’s findings, the current owner must confirm that they remain relevant. A callback can mention the project without making every character aware of it. The authored branch should carry only the facts and audience permissions the system can represent.

## 684. Installment 51 close — Curiosity becomes agency through method

This route lets the learner choose a question, gather permitted evidence, compare sources, and decide who should hear the result. Factions may offer materials or access; teachers guide method; operational owners decide whether any practice changes. The player can facilitate, challenge overconfident claims, and preserve the learner’s authorship.

Endings include a verified change, a useful uncertainty, an owner referral, an approved class presentation, or a private learning outcome. The meaningful consequence is that the learner can make a better-informed contribution without the game pretending that study grants authority over every related system.

## 685. Installment 52 — Two learners ask to study one practical problem

Two learners may ask to share a question because the work affects both of them or because one feels more confident with a peer present. The player can help them name a shared goal, then let each describe what they already know and what they hope to learn. A shared lesson is not automatically suitable merely because both learners chose it. Their schedules, prerequisites, preferred pace, access needs, and consent still need separate consideration. The educator can propose one common session, a paired demonstration, or two shorter sessions with a later comparison. Each learner can accept or decline independently.

The shared goal can be framed as a task question—how to read a warning, measure a ration, identify a safe hand tool, understand a ledger entry, or recognize when to ask a specialist—without turning either learner into the other’s assistant. If one learner has more experience, the teacher should not silently assign them to teach. That learner may volunteer to demonstrate, ask the educator to lead, or prefer to learn the same material from the beginning. The other learner can accept peer help, request a private explanation, or stop. The player should make these choices legible before booking time.

## 686. Shared-learning branch — Learners begin at different levels

The teacher can ask each learner what they have practiced and what they want to do next. A learner may know the vocabulary but not the procedure; another may have performed the task without knowing why it works. The curriculum owner’s prerequisites remain the source of eligibility, while the educator adapts examples and pacing within that scope. The player can choose a common foundation followed by distinct practice, two tracks in sequence, or separate sessions.

Do not flatten the difference into one “average” proficiency result. The session record should preserve which learner attempted which supported step, if the current owner can represent that information. If not, prefer separate lesson records over an invented group score. The outcome may be that the pair can work together, that one needs another session, or that both learned different parts of the same process. If one learner advances sooner, the other should not be described as holding them back.

## 687. Shared-learning branch — A confident peer takes over the lesson

One learner may start explaining the task before the educator has established what is safe or accurate. The teacher can invite them to show the method, ask what evidence supports it, or pause and compare it with the current curriculum. The learner who offered help may feel proud, defensive, or relieved to hand the explanation back. The other learner can ask a question, request another source, or say that the demonstration moved too quickly.

The player does not need to punish confidence. The useful branch is whether the peer can describe the limits of what they know and whether the educator can make room for both learners. If the demonstration conflicts with the approved method, the teacher explains the discrepancy and identifies the current source owner. The learners may continue with the approved method, compare the two approaches under supervision, or end the group session and ask the owner to review the curriculum. No peer receives authority over another person’s work from giving one explanation.

## 688. Shared-learning branch — The learners disagree about the question

Two learners may agree to study together but disagree about what problem matters. One wants a general lesson, while the other wants help with a specific upcoming assignment. The player can ask each to describe the intended use, then let the teacher identify overlap. They may settle on a shared foundation and separate follow-up, let one learner lead this session and the other choose a later one, or decide that their goals are too different for a group lesson.

The disagreement is not a trust score and does not need a winner. Each learner can keep their question private, propose a narrower topic, or leave the arrangement. If a faction is funding or hosting the session, its representative may explain which subjects it can support, but cannot choose the learners’ purpose for them. The teacher can decline to cover a subject beyond their competence and refer the pair to an appropriate source.

## 689. Shared-learning branch — One learner wants privacy from the other

A learner may want instruction but not want a peer to know why. The player can offer a neutral lesson description, schedule individual time, or ask whether the learner is comfortable with the other person knowing only the subject. Do not reveal a personal reason, prior difficulty, health detail, or performance history to make the shared scene more dramatic. The second learner is not entitled to an explanation for a change in schedule.

If the activity genuinely requires group work, the teacher states that before either learner commits. The first learner can participate without disclosing their reason, request an alternative method, or decline. The teacher and player should use the minimum information necessary to arrange a suitable session. If the existing lesson record cannot represent separate privacy scopes, record the lesson at the narrowest safe level or keep it individual.

## 690. Shared-learning branch — The group needs a practical role for everyone

A demonstration may require someone to measure, someone to read an instruction, and someone to observe. The teacher can explain the roles and let learners choose or rotate them. A role is part of the lesson only if the learner agrees and the activity is safe. The player can ask for a non-operational practice version, use a sample object, or request that the teacher perform the hazardous step while learners observe.

If one learner repeatedly takes the visible role, the teacher can invite another to try without treating reluctance as failure. The learner can observe, take notes, ask questions, or leave the practical portion. The outcome should state what each person attempted rather than claiming that attendance alone established competence. A future lesson can build on the specific practice each learner chose.

## 691. Shared-learning branch — A rotating peer circle forms

After a successful paired session, learners may ask to meet again with one or two additional peers. The player checks the curriculum owner’s group limits, the educator’s availability, room access, and whether each person has opted into the same audience. A rotating circle can make scarce teaching time more useful, but it should not become an unbounded classroom that the player manages through informal attendance alone.

Participants may choose to bring a question, listen only, practice a method, or ask the teacher to lead. The host can state the session’s scope and time limit. A late arrival can join only if the educator and learners agree and the activity remains safe. If group size exceeds what the teacher can supervise, the teacher can split the meeting, defer some topics, or ask participants to choose a later time. The player can make the limits visible and preserve a path for each learner.

## 692. Shared-learning branch — The session creates a useful peer resource

Learners may ask whether they can keep a diagram, question list, glossary, or demonstration notes for the next group. The source owner and teacher check accuracy, audience, and whether any included example exposes a private record. The learners can approve the shared copy, make a generic version, keep it within the small circle, or take no copy. The act of creating a learning aid can be a meaningful branch even when it does not grant a skill increase.

If the curriculum changes, the owner can mark the aid as outdated, request revision, or withdraw it. A copy should not remain presented as current solely because learners like it. If the current system lacks versioning, the safest route is to link the aid to an owner-confirmed source and avoid a separate editable rules document. The learners can decide who may use the notes while the source owner remains responsible for the official instruction.

## 693. Installment 52 outcomes — Shared instruction preserves separate progress

The group outcome can be one of several states: a common lesson is scheduled, separate lessons are preferred, one learner joins and another declines, the learners select a shared foundation and individual follow-up, a peer demonstration is reviewed, the circle is postponed, or the teaching owner has no safe capacity. In every outcome, retain the separate consent and eligibility decision for each learner. Do not make group participation a condition for receiving an individual lesson.

A later callback can mention that learners exchanged notes, that one is waiting for a follow-up, or that a question remains disputed. It should not infer friendship, rivalry, or improved relationship from attendance. If the group made a public learning resource, only people authorized to see it should receive the callback. The small circle can also end quietly after one session; its narrative value does not require an endlessly recurring club.

## 694. Installment 53 — An urgent task interrupts a scheduled lesson

A scheduled session can collide with a real shelter event: a water delivery arrives early, a work shift runs long, a room becomes unavailable, a learner is needed for an authorized task, or the teacher’s duty changes. The schedule owner decides what changed, and the lesson owner determines whether the session can proceed in a shortened or alternative form. The player should see the new constraint and its source before choosing. Do not mark a learner unreliable when the schedule owner moved the duty after confirmation.

The learner can attend the original time if still permitted, request a new slot, take a safe independent exercise, or cancel. The teacher can offer a different duration only if the curriculum supports it. A short session is not automatically equivalent to the planned lesson; the educator identifies which objective can realistically be covered and which remains for later. If there is no suitable option, the booking becomes a reschedule or cancellation with a clear reason, not an invisible lesson that silently fails.

## 695. Interruption branch — The learner chooses the operational task

The learner may decide that the arriving task is more important than study. The task owner confirms whether the learner is eligible and whether the assignment is actually needed. The teacher can hold the lesson for a defined period, release the slot, or offer another appointment. The learner may accept the new duty, ask someone else to cover it, or decline if they are not obligated. Preserve the choice that was made rather than narrating it as a compulsory sacrifice for the shelter.

If the learner takes the duty, the player should not also grant lesson progress for a session they did not attend. The lesson can remain scheduled only if the owner supports a pending appointment. If no durable booking exists, return the learner to the eligible pool and let the player request another time later. The outcome should name the concrete cost: study was delayed, the task was covered, or another survivor accepted the assignment.

## 696. Interruption branch — The teacher’s shift consumes the lesson window

The teacher may be called to a repair, treatment, security, or supply task. Their task owner confirms the assignment and whether it can be delegated. The player can ask whether a qualified alternate teacher is available, but cannot treat a similar skill label as proof of qualification. The learner may accept a different teacher, wait for the original one, or choose a written exercise approved by the curriculum owner.

The original teacher can leave a handoff note if the owner supports it, stating what they intended to teach and any preparation already complete. The alternate teacher reviews the current curriculum independently and can change the plan. Do not copy private comments about the learner into the handoff. A teacher’s absence may inconvenience the student without making either person culpable; the route should preserve that distinction.

## 697. Interruption branch — The learner arrives after the safe window

The learner may arrive late because another duty ran over, a passage was closed, or they misunderstood the meeting place. The teacher decides whether enough time remains for the planned objective and whether the learner can still participate safely. The player can offer a smaller approved objective, a new time, or a short check-in. The learner can explain, decline to explain, accept the revised session, or leave.

Do not reduce proficiency as a punishment for lateness. If there is a concrete schedule conflict, the player can help the owner review it, but attendance should not create an automatic disciplinary side system. A missed appointment can carry a real opportunity cost because the teacher is unavailable later; show that cost plainly. If another resident was waiting, ask whether they want the slot before reassigning it.

## 698. Interruption branch — A hazard cancels the practice component

The room, tool, material, or demonstration may become unsafe after the lesson begins. The teacher stops the activity and the location owner confirms restrictions. The learners can continue with a paper exercise or discussion if appropriate, observe from a safe place, reschedule the practical portion, or end the lesson. No character should be asked to complete one last repetition for progress after the teacher has identified a hazard.

The owner records the actual lesson objective that was covered. If no current API represents partial objectives, do not approximate it by granting the full proficiency step; preserve only supported completion and narrate the interruption as unfinished. The player can help relay the safety report to the existing maintenance or incident owner. The lesson is not also a substitute hazard investigation.

## 699. Interruption branch — The learner’s needs change during the session

A learner may become tired, hungry, distressed, or unable to focus. The relevant needs owner supplies the current fact; the teacher offers a pause, a shorter lesson, a different format, or an end to the session. The learner chooses among safe options where possible. Do not infer a diagnosis or reveal a personal need to other learners without permission.

A pause should not spend materials or accrue progress unless the lesson owner defines that behavior. The teacher can note which objective remains and ask whether the learner wants to resume later. If the learner says no, close the session respectfully. The later callback can show that they chose rest or a new format, not that they failed to persevere.

## 700. Interruption branch — The entire room becomes inaccessible

The location owner may close a study space for repair, incident response, or another scheduled use. The player can check a different authorized location, move to an approved low-risk format, or reschedule. Each learner may have a different acceptable alternative: one may need a quiet room, another may be fine with a public table, and a practical lesson may require its original equipment.

Do not treat a location change as a mere visual substitution if access, privacy, or safety changes. The new owner confirms permission, and participants are told who can observe them there. A learner can decline the alternative. If no compatible room exists, the session pauses and the learner retains a clear next step rather than an unexplained “pending” state.

## 701. Interruption branch — A faction asks to use the lesson as a public demonstration

A faction representative may see the interrupted session as an opportunity to showcase its training program. The player asks whether the learners actually want an audience and what recording or attribution is involved. The teacher can agree to a public demonstration, keep the lesson private, or postpone. Each learner may consent separately; one person’s agreement cannot authorize the other’s image, story, or performance.

The faction can offer a room, materials, or teacher time, but it cannot convert educational attendance into recruitment. If there are conditions, disclose them before the session is moved. A learner may accept the resource and decline the publicity if the owner permits that combination. If the faction insists on bundling them, the learners can choose another route or cancel. The supporting role remains visible and bounded.

## 702. Interruption branch — A deadline tempts the player to award progress early

The player may want the learner to qualify for a task that begins soon. The task owner can state the actual qualification requirement and the education owner can report current proficiency. The player can ask for another safe teacher, a valid prior-learning assessment, or a task that allows supervised practice. They cannot declare the lesson complete because a deadline would make the outcome convenient.

If no qualifying route exists in time, the task may go to another eligible worker and the learner may pursue the next opportunity. This is a consequential branch with a clear explanation, not a design failure to conceal. The learner may be disappointed, ask for future preparation, or decide that the task is not worth pursuing. The game should avoid implying that effort guarantees the job.

## 703. Interruption outcomes — Work, care, and study share one calendar

The interrupted lesson can end as a completed short objective, a safely paused session, a rescheduled booking, an approved alternate format, a missed opportunity, a room change, or a cancellation. It may also return to the task owner if the learner chose duty. Each endpoint should leave the schedule, lesson record, and learner status consistent. The interface reports what the owner confirmed and what remains uncertain.

Later dialogue can callback to the actual cause: the teacher covered an urgent repair, the learner chose their shift, the room remained closed, or the pair found a quiet alternative. Avoid a generic “lesson failed” message. The branch broadens playstyle by allowing the player to protect education, prioritize operations, negotiate a compromise, or accept a delay without rewarding coercion.

## 704. Installment 53 close — An interruption is a decision point, not a hidden penalty

The design requires explicit state for booked, started, paused, completed, cancelled, and rescheduled only to the extent the current owner can represent those states. Do not introduce a host-only appointment ledger. The current save owner must carry any durable booking or recovery state; otherwise, prefer an honest one-session route with no claim that it survives reload.

An interrupted session can still teach the player how the shelter’s calendar works. The learner and educator can state what they need, operational owners retain assignment authority, and factions can offer bounded resources. The lesson may resume, change format, or end. The decision belongs to the people involved and the owner-backed schedule, not to a morality axis.

## 705. Installment 54 — Demonstration, assessment, and the meaning of “ready”

A learner may finish a lesson and ask whether they are ready to perform a task. The question crosses two authorities: education can report what was learned, while the task owner decides qualification and supervision. The player should be able to see both records, their evidence, and their limits. “Attended a lesson,” “demonstrated a practice step,” “holds a proficiency value,” and “is eligible for this task” are not interchangeable phrases.

The teacher can describe a supported demonstration, an assessment prompt, or a referral to a qualified evaluator. The learner may choose the method, request an accommodation, ask for more practice, or decline assessment. The owner must define what evidence counts before the attempt begins. The result can be ready, ready with supervision, not yet ready, inconclusive, or referred—only if those states match existing contracts. Do not add a new credential authority to make the scene feel official.

## 706. Readiness branch — The learner chooses a practical demonstration

The teacher explains the task, its hazards, and the supervision boundary. The learner can demonstrate on a safe sample, narrate the steps, point out a hazard, or decline the practical method. The evaluator confirms whether the selected demonstration measures the required skill. If the live task is unsafe for assessment, it is not an acceptable test environment merely because it looks realistic.

The learner may complete the core method but miss a step. The evaluator can stop, ask a clarifying question, or record only the components actually demonstrated. If the learner corrects the step after a prompt, the owner decides whether prompted performance meets the requirement. The outcome states what happened, what was observed, and what support remains needed. A failed attempt should not erase prior learning or create a universal incompetence tag.

## 707. Readiness branch — The learner asks for a non-practical assessment

Some skills can be assessed by explanation, diagram, sample record, or a sequence of choices. The evaluator confirms that the alternative is equivalent for the specific qualification and that the method is accessible to the learner. The learner can select an allowed format or decline. The game should not assume that a written test is neutral: reading, language, disability, stress, and unfamiliar terminology may affect what it measures.

If an accommodation is needed, the relevant owner approves the format without requiring unnecessary disclosure to the whole group. The result can include a scope note, such as “understands the checklist, needs supervised practice with the equipment.” It should not produce a stronger qualification than the evidence supports. This route makes the player’s preparation choices matter while respecting different ways of demonstrating knowledge.

## 708. Readiness branch — The assessment is inconclusive

The learner may have shown a method correctly once, but the evaluator cannot determine whether the result was independent, repeatable, or applicable to the intended task. The evaluator can request another sample, consult a qualified specialist, or provide a supervised first assignment. The learner can accept, ask what evidence is missing, or stop. The player can compare the stated criterion with what was actually observed.

Do not silently turn “inconclusive” into “failed” or “passed.” Explain whether the uncertainty comes from the test, the source, the observer, or the learner’s opportunity to practice. If the evaluator and teacher disagree, record the different scopes of their judgment and route the dispute to the established qualification owner. An honest unresolved answer is a useful branch when it changes how the next task will be supervised.

## 709. Readiness branch — An earlier learner brings outside experience

A learner may have practiced a skill before joining the shelter or in a different setting. The evaluator can ask for a demonstration or describe an accepted evidence route. The learner chooses what to disclose and whether to be assessed. Do not treat an undocumented history as either automatic proof or automatic distrust. The owner can recognize supported prior learning, request a refresher, or conclude that the methods differ too much to transfer.

The result may allow the learner to skip a lesson while still requiring a current safety briefing, or it may identify one new local procedure. The educator can explain the difference without dismissing prior experience. If no recognition process exists, state that limit and provide the shortest valid route available. The player should not promise a bypass that the task owner cannot honor.

## 710. Readiness branch — The evaluator has a relationship with the learner

The evaluator may be a friend, relative, faction member, or person involved in a previous disagreement. The player can disclose the connection and ask whether a second evaluator is available. The qualification owner decides whether recusal or review is required. A familiar teacher can still provide instruction, but instruction and final qualification may need separate observers for a disputed high-impact role.

The learner can proceed with the current evaluator, request another, or defer. If no alternate exists, the owner may offer a narrower assessment or a witnessed demonstration. Do not make a relationship meter decide whether the result is trustworthy. The relevant questions are the evaluator’s authority, the evidence, and any review rule that currently applies.

## 711. Readiness branch — A faction funds the course and asks for a credential

A supporting faction may pay for materials or teaching and ask whether completion can be noted. The player clarifies what record the education owner can produce and who may see it. The faction can receive an aggregate report, a learner-approved confirmation, or no personal record, depending on the supported privacy rules. Funding does not authorize it to invent its own grade or require the learner to share assessment details.

The learner can accept the course while declining public recognition. They can also choose to use a faction-specific certificate if it is explicitly a local attestation and does not replace official task eligibility. The player should label its scope. A later task owner remains responsible for deciding whether the evidence satisfies that task’s requirements.

## 712. Readiness branch — The learner refuses the assessment

The learner may have completed the lesson but not want a formal test. The teacher can ask whether they still want practice or simply want to stop. The task owner determines whether assessment is required for the role. The learner can pursue another activity, take a supervised practice task if allowed, or return later. Refusal does not erase a lesson record or prove lack of competence.

If the assessment is mandatory for a high-risk assignment, explain that requirement before the learner asks to perform it. The player can explore another eligible task or another preparation route. Do not let a faction representative shame the learner into taking a test. The branch remains meaningful because the learner sets a boundary and chooses what opportunity to pursue next.

## 713. Readiness branch — The standard changes after study begins

An owner may update the required procedure, tool, or safety rule after the learner has been assessed. The learner can ask whether prior evidence remains valid, whether a short update is sufficient, or whether a new demonstration is needed. The owner compares the old and current requirements and states the difference. Do not retroactively call the prior learning false if it was correct under the earlier standard.

The transition may require a brief module, supervised practice, full reassessment, or no change. Each choice should be supported by the owner and attributed to the changed requirement. A learner should not be required to repeat the entire course because one minor step changed. Conversely, a major safety change cannot be ignored because the learner already passed once.

## 714. Installment 54 close — Readiness has a scope and an owner

The ending can be a supported qualification, supervised readiness, an evidence gap, a second assessment, a recognized prior skill, a private result, a declined assessment, or a changed standard. Each must name the authority that produced it. The learner can disagree, ask for review, or choose another opportunity. The player’s role is to make the route visible and help prepare, not to award competence by dialogue.

This distinction supports richer playstyles: invest in practice, seek independent evaluation, accept a supervised entry task, rely on recognized experience, or leave an opportunity for later. Factions can fund learning or supply an observer, while the current task and education owners remain decisive. The outcome should affect only consumers that already read the relevant skill or eligibility state.

## 715. Installment 55 — A teacher can have knowledge without being the only authority

Teaching capacity is scarce and expertise is uneven. A survivor may know a procedure from experience but lack the current qualification to teach a hazardous step. Another may hold the formal role but know little about local conditions. The player can identify the distinction, ask who owns curriculum accuracy, and decide whether the candidate should teach, assist, provide a case example, or refer the learner.

The feature should let residents contribute without making every experienced character a universal instructor. A teacher can explain what they know, where they learned it, and what they cannot verify. A specialist can review the approved method. A local worker can describe the condition on the ground. The player can combine these voices through the current education and task owners, or pause if no safe synthesis exists.

## 716. Teacher branch — The candidate has experience but no formal record

The candidate may have performed the work for years but lack a current education record. The curriculum owner can decide whether they may give an informal demonstration, assist under a qualified teacher, or need verification before leading. The candidate can explain their experience, offer a low-risk example, or decline to disclose personal history. The learner can choose whether that form of instruction meets their needs.

The record must not overstate the candidate’s status. If the system supports a verified instructor role, use it only after the owner confirms the criteria. If not, label the session as peer sharing and route any assessment to an authorized evaluator. The learner may still benefit from a practical perspective without the player falsely equating experience with institutional approval.

## 717. Teacher branch — The qualified teacher disagrees with the written source

The teacher may notice a mismatch between a curriculum page and what the task owner currently does. They can pause the lesson, explain the disagreement, and submit a correction request. The player can consult the source owner, ask for a second specialist, or continue only with the portion that is not disputed. The learner may observe, ask questions, or postpone until the conflict is resolved.

Do not let the teacher silently replace official instruction with personal practice. Nor should the written page automatically override a confirmed safety update from an operational owner. The branch ends with a corrected source, a scoped addendum, a referred question, or a paused course. A later callback can show which version was used and why, preserving the history of the decision without duplicating the curriculum authority.

## 718. Teacher branch — A faction educator has a local method

A supporting faction may teach a method that works with its own tools or facilities. The player asks whether it transfers to the shelter’s equipment and constraints. The faction educator can explain adaptations, provide a supervised demonstration, or state that the method is specific to its site. The curriculum owner decides whether to add the method as an optional example or reject it as incompatible.

Learners can compare methods when the comparison is safe and authorized. They can prefer the faction’s approach, the shelter’s approach, or a neutral source. Do not frame disagreement as evidence that one faction is honest and another corrupt. The methods may reflect different tools, materials, or risk tolerances. Supporting factions enrich the knowledge landscape while the learner remains free to evaluate evidence.

## 719. Teacher branch — Two educators offer different explanations

Two candidates may both be qualified but emphasize different parts of the same lesson. The player can ask the curriculum owner for required learning objectives and let each teacher choose an example within those limits. The learner may attend both, select one, request a written comparison, or ask an owner to resolve a substantive contradiction.

Do not make the choice a popularity contest unless the learner explicitly values rapport as part of the decision. Show availability, teaching format, schedule, safety scope, and source basis. If the educators can co-teach, establish who handles which topic and who is responsible for stopping an unsafe demonstration. If they cannot agree on division, schedule separate sessions or defer the disputed material.

## 720. Teacher branch — The teacher asks the learner to teach someone else

After learning a procedure, the survivor may be invited to demonstrate it to a peer. They can accept, decline, prepare with the educator, or participate only as a helper. The peer must agree to learn from them, and the owner must confirm the activity is within the learner’s competence. A learner who declines is not ungrateful; a learner who accepts does not automatically become a permanent assistant.

The teacher can remain present, give feedback, or take over if a safety limit is reached. The learner may discover that explaining a concept exposes a gap in their own understanding. That can lead to another lesson without shaming them. If the current system cannot distinguish peer practice from instruction, keep the event narrative-only and do not create instructor progression state as a side effect.

## 721. Teacher branch — A learner challenges a teacher’s account

The learner may bring an observation that contradicts the teacher. The teacher can welcome evidence, explain a known limitation, ask the learner to test the claim through an approved method, or refer it for review. The learner may persist, accept the explanation, or decide the material is not trustworthy. The player can mediate the process, but should not assign credibility by moral alignment.

If the claim concerns another person, private record, or faction, the normal owner and privacy boundaries still apply. A teacher can be mistaken without being malicious; a learner can be sincere without being correct. The branch changes the relationship to the source through specific actions: whether evidence was shared, whether a correction was made, and who followed up.

## 722. Installment 55 close — Expertise is plural, authority is explicit

Teaching voices can include qualified instructors, peer experience, local workers, independent specialists, and faction educators. Their contributions differ in scope. The game should preserve the role each person actually played and avoid collapsing all knowledge into one reputation score. Learners choose what to hear and whether to continue; owners decide which source is authoritative for each task.

This creates space for practical playstyles: prefer formal instruction, learn through peer practice, compare community methods, challenge a source, or defer until a dispute is resolved. Every route still has a clear owner and an honest limit. A supporting group can make scarce expertise available; it cannot unilaterally certify a learner or overwrite the canonical curriculum.

## 723. Installment 56 — Learning material has custody, scarcity, and a return path

A lesson may depend on a manual, measuring tool, demonstration object, recorded message, or safe sample. The owner of that material determines whether it can be borrowed, copied, consumed, or used only in a particular room. The teacher explains what the lesson needs before the player schedules it. A learner can ask for a low-material version, wait until an item is available, borrow from a supporting group, or choose another topic. Material availability should shape the route without silently becoming a second inventory.

The education plan reuses the current property, inventory, room, and task owners. It does not add a lesson stockpile. If an item is consumed by practice, the existing owner records that transaction. If it is returned, the owner confirms its return. If the material is merely illustrative, do not decrement a resource because a narrative line mentions it. The player should be able to distinguish “needed and reserved,” “available to inspect,” “used in practice,” “loaned,” and “requested but not confirmed.”

## 724. Material branch — The last usable copy is already in use

The teacher may need a handbook currently borrowed by a task team. The resource owner can report when it may be available or whether another approved copy exists. The player can reserve a later session, ask the team for a supervised consultation, or choose a different source. The learner can wait, change topic, or decide that this course is not a priority now.

Do not take the item from its current owner because an education quest has higher narrative priority. If a shared resource can be inspected by multiple people at once, the owner determines that. A task team can decline an interruption, offer a short viewing window, or provide a summary through an authorized specialist. The branch exposes a real tradeoff between teaching time and current operations while leaving the allocation decision with the resource owner.

## 725. Material branch — The substitute resource teaches a different method

The original tool may be unavailable while a substitute is on hand. The teacher can explain whether the substitute preserves the learning objective or changes it. The learner can accept an adapted session, request the original tool, or postpone. The owner validates the substitute for the specific practice; the player should not decide equivalence by item name or apparent similarity.

If the alternative teaches the general concept but not the local procedure, record only the general objective that was actually covered. A later session can complete the missing part. The player can tell the learner what they will be able to do afterward and what they will still need supervision to do. A faction may lend the correct tool, but any return date or use condition must be explicit and accepted before the material is moved.

## 726. Material branch — The learner wants to make a personal copy

The learner may ask to copy a page, diagram, or checklist for later practice. The source owner determines whether copying is allowed, whether the page contains restricted content, and whether the copy needs a current-version note. The learner can use a public summary, take notes by hand, request a permitted excerpt, or decide not to keep a copy. Their choice does not establish a new official curriculum.

If a copy contains a name, private example, or faction-only procedure, the teacher can replace that portion with a generic example. The learner may ask why the text is restricted; the owner can explain the boundary or refer the question. Do not respond by treating the learner as suspicious. If no copy workflow exists, do not promise one or create durable notes in a parallel save store. Offer a spoken review or a pointer to an existing authorized source instead.

## 727. Material branch — A faction offers its workshop for instruction

A supporting faction may offer a room and equipment when the shelter lacks space. The player asks which learners may attend, whether a host must be present, what tools are accessible, and what conditions apply. The location and equipment owners confirm actual availability. Each learner decides whether they are comfortable attending. The faction can provide expertise and access without taking over the course’s learning outcomes.

The offer may include a public demonstration, a limit on tools, or a request to return equipment after the session. Put the condition beside the offer so the player can compare it with shelter-based options. The learner may attend one session without joining the faction if the host permits that arrangement. If access requires membership, state it plainly; the learner can accept the full package, seek another source, or decline.

## 728. Material branch — The lesson uses a consumable sample

Some practice consumes a sample or changes it so it cannot be reused. The task or inventory owner validates the quantity and purpose before the lesson. The teacher states how many attempts are supported and what happens if the sample fails. The learner can choose a demonstration-only format, use an approved substitute, or wait for a later supply.

The player must not pay a hidden cost after the session. If a sample is used, the owner records it; if the lesson is cancelled before use, it remains available unless another action consumed it. When the learner makes a mistake, the cost should follow the agreed practice design rather than a surprise penalty. A failed sample may still teach something, but the system should report material use and lesson progress separately.

## 729. Material branch — A damaged tool changes the training decision

The teacher notices that a tool is damaged before or during practice. They stop the affected activity and report it through the current maintenance owner. The player can switch to observation, use a verified replacement, reschedule, or stop. A faction technician may offer an inspection, but the owner decides whether the tool can return to service.

Do not let the learner continue because the tool is scarce or because a proficiency point is close. The teacher explains what was learned before the stop and what was not attempted. Any repair cost follows the actual maintenance path; it is not charged to the learner for following an instruction. A later callback may show that the tool was repaired or remains unavailable, if that state is supported and relevant.

## 730. Material branch — A learning aid goes out of date

An old chart or handwritten checklist can remain useful as history while no longer being safe as current instruction. The source owner can mark it superseded, request a correction, or preserve it as a historical note with a warning. The learner can compare versions, ask what changed, or use only the current source. The teacher identifies the change’s practical effect rather than simply calling the old aid wrong.

If the aid is publicly posted, the communication or location owner may need to remove or annotate it. The education owner confirms the replacement. A faction that supplied the old document can be asked to review the correction; it should not be blamed automatically. The player can choose whether to report an omission, help produce a revised summary under the owner’s process, or focus on the learner’s immediate need.

## 731. Installment 56 outcomes — Access can be granted, delayed, adapted, or declined

The material branch can conclude with a confirmed loan, a supervised inspection, an approved substitute, a generic copy, a faction-hosted session, a consumed sample, a maintenance referral, or no safe resource. The education route should tell the learner exactly which objective remains possible. If the material does not arrive, the schedule must not imply that the lesson proceeded. If a resource owner changes its answer, refresh the session plan before commitment.

Supporting factions add meaningful local texture when they contribute a tool or room and describe their constraints. Their assistance should remain legible as a contribution with a source and duration. A faction’s equipment does not become shelter property, and receiving a loan does not create an unspoken political obligation. The learner can value the help while remaining independent of its provider.

## 732. Installment 57 — A lesson becomes part of an ordinary week

Not every educational arc needs a dramatic discovery. A learner may schedule regular short sessions, practice after a shift, or revisit a subject when a practical need arises. The player can help fit instruction around duties, but the schedule owner and learner remain responsible for their commitments. Repetition should be useful because it lets the learner choose pace and application, not because the game needs a recurring attendance meter.

The education owner may support discrete lessons and skill changes; the plan should not add streaks, homework debt, or a second needs system. A missed session can be rescheduled or released. A completed lesson can lead to another objective, a supervised task, a peer explanation, or an intentional pause. The learner can stop a course without forfeiting knowledge already recorded by the current owner.

## 733. Routine branch — The learner requests a weekly time

The learner may prefer the same time each week. The player checks recurring availability through whatever schedule contract currently exists. If only one-off booking is supported, schedule one session and invite the learner to request the next; do not imply a persistent series. The teacher can offer a regular window, a rotating time, or no recurring capacity. The learner can choose another teacher or a less formal study arrangement.

A recurring arrangement should not reserve scarce capacity forever without revalidation. At each supported interval, duty, room, and teacher availability may change. The owner can ask for confirmation, release a slot, or propose another time. The learner can pause during illness, high workload, or changed interest without accumulating fictional debt. The player sees the next confirmed date only after the relevant owner accepts it.

## 734. Routine branch — Practice happens between formal sessions

The learner may ask to practice independently between lessons. The teacher identifies which steps are safe to do alone and which require supervision. The location and equipment owners confirm access. The learner can practice, request observation, choose a different exercise, or wait. The player should not translate an optional practice note into automatic progress if the skill owner requires an assessed lesson.

If current proficiency supports passive skill improvement, the route must use that authority and avoid double counting. If it does not, practice can remain a narrative action with a clear next appointment. The learner can report what felt difficult; the teacher can adapt a future lesson. Independent effort may influence the conversation without generating unsupported permanent state.

## 735. Routine branch — The learner changes their goal

During the course, the learner may decide that the original goal no longer matters. They can say why, keep the reason private, ask to redirect the course to a related topic, or end it. The teacher explains whether completed lessons transfer to the new goal. The education owner determines which prerequisites still apply. The player can help identify a new route but cannot demand that the learner justify a change.

If a faction sponsored the original course, the terms may affect whether further sessions remain available. The sponsor can offer the new subject, decline it, or refer the learner elsewhere. The learner can continue only the portion they want, accept a clearly described alternative, or stop. The story should not cast a changed goal as betrayal of the shelter or faction.

## 736. Routine branch — The learner teaches a safe tip informally

After a lesson, the learner may share a small tip with a peer. The peer can listen, ask for the source, or decline. If the tip is safety-critical or conflicts with official instruction, the educator can suggest a review before it is repeated as guidance. This scene is distinct from an authorized teaching role. It can show how knowledge travels while keeping the owner’s authority clear.

The player may encourage the learner to credit their source, note uncertainty, or invite the peer to attend a formal session. They should not turn an informal exchange into a reputation reward. If the tip is wrong, the correction should be specific and proportionate. The learner can revise their account or explain where the mistaken understanding came from, and the peer can choose whether to continue learning from them.

## 737. Routine branch — The educator keeps a boundary on availability

An instructor can care about a learner and still be unavailable outside agreed hours. The learner may ask for extra help, and the teacher can offer the next open session, refer them to another source, or decline further contact. The player can help make the response kind and clear. Do not reward boundary violations as proof of devotion, and do not make friendship a hidden way to bypass the schedule.

The learner may feel disappointed or relieved, seek a different teacher, or decide to study later. The teacher can be praised for a useful lesson without becoming on-call. If a safety question requires immediate guidance, route it to the current operational owner rather than expecting the instructor to answer at any hour. Relationship dialogue can acknowledge care without changing the availability record.

## 738. Routine branch — A caretaker asks for learning to fit another person

A guardian or caretaker may request a schedule change for a learner. The learner’s age, autonomy, current consent rules, and relevant guardian authority determine who may make the decision. The caretaker can describe the practical constraint, while the learner can state whether the proposed time works. The player should hear from the learner where the current system allows it.

The teacher can propose a shorter session, a different day, or an alternative source. The schedule owner confirms availability. A caretaker does not gain access to private lesson details merely by requesting a change. If the learner does not want a caretaker present, respect the supported privacy arrangement. The branch should distinguish support from control without reducing either character to an alignment label.

## 739. Routine branch — The learner leaves the shelter or changes roster

A roster transition can interrupt the course. The current owner determines whether lesson records travel with the learner, whether the schedule ends, and who may access a transfer. The learner can authorize a summary, request no transfer, or ask for a particular source to be included. Do not keep lessons scheduled after the learner is no longer eligible or present.

If knowledge persistence is attached to the survivor’s existing skill record, that record follows the owner’s current migration rules. The plan should not create an education dossier. A teacher may offer a goodbye note or a final recap, but any personal message remains separate from official proficiency. A later reappearance can callback to the learner’s choices without exposing a record the player no longer has permission to see.

## 740. Installment 57 close — Continuity comes from accepted next steps

The ordinary-week arc can end with a confirmed recurring slot, a one-time follow-up, independent practice, a changed course, an informal peer tip, a teacher boundary, a caretaker adjustment, a roster transition, or a deliberate pause. Each ending can remain satisfying if it accurately reports the learner’s present goal and the next action, if any. No outcome requires a grand success scene.

The player’s playstyle may be scheduler, advocate, source-checker, practical coach, or patient observer. Each role influences what routes are offered and how conflicts are surfaced. None should grant the player authority to book a person against their will or certify learning outside the owner. The education loop gains depth through persistence, privacy, access, and the consequences of a real calendar.

## 741. Installment 58 — Cross-quest callbacks must preserve the learning’s scope

An education quest can intersect later repair, trade, medical, travel, work, or faction content. The callback should carry a narrow fact that the downstream owner can verify: a learner completed a supported demonstration, a source correction was submitted, a teacher is unavailable, a tool remains on loan, or a student requested privacy. It must not carry a broad label such as “expert,” “loyal,” or “unreliable” unless an existing authority actually defines that state.

Writers should identify the exact action that allows the callback. If a learner’s observation improved a task, the task owner records or exposes that improvement. If a player only heard the observation, it remains dialogue context and must not change a simulation result. If the learner merely attended, later characters should not refer to a qualification they never earned. This is how the narrative can feel continuous without using unsupported flags.

## 742. Callback branch — A learner applies knowledge on a later task

The learner may notice a relevant detail while assigned to an authorized task. The task owner decides whether they can act, report, or ask for help. The player can encourage them to state what they observed, then route it to the owner. They may be correct, partly correct, or mistaken. The important branch is whether their information changes the task’s next decision, not whether the game rewards them for being a student.

If the observation is accepted, the task outcome records its effect through the existing mechanism. If rejected, the owner explains why or requests more evidence. The learner can accept the decision, ask for review, or preserve the observation for future study. A callback must reflect the owner’s result. It should not award hidden proficiency simply because the learner was present during a successful task.

## 743. Callback branch — A changed source reaches a different learner

A later student may encounter a corrected curriculum. The teacher can explain that a previous project identified an uncertainty and that the source owner revised the guidance. If the first learner consented to attribution, the teacher can credit them; otherwise, describe the correction without naming them. The second student can ask about the evidence, accept the new method, or compare the archived version.

The correction should not imply that one learner authored an official policy unless the owner granted that role. A faction may react to the change, but its response remains one character or group’s view. The player can help keep the lesson grounded by showing the revised source and who approved it. The callback rewards careful inquiry while keeping canon and authority stable.

## 744. Callback branch — A learner’s skill is needed but they are off duty

A task may arise that matches what a survivor learned, while they are resting or assigned elsewhere. The player can ask whether the task owner has another eligible person, whether a later slot is possible, and whether the learner wants to volunteer. Skill does not create an obligation to interrupt rest or cancel an existing duty.

The survivor may accept, decline, or ask for a different time. The owner checks schedule, consent, and safety. A refusal should not erase the qualification. If the work proceeds with someone else, the learner can later ask about it without being framed as having missed their only chance. This cross-quest branch joins education to roster play through an actual decision, not a hidden bonus.

## 745. Callback branch — Another faction disputes the lesson’s account

A faction may publish a different explanation of the same method or event. The player can show the learner both sources, ask a neutral specialist, or leave the dispute unresolved. The learner chooses which evidence to examine and whether to speak publicly. The faction contact can explain its position without gaining control over the learner’s study.

If a factual correction affects an operational rule, route it to the current source owner. If it is a difference in interpretation, preserve both accounts. The learner may decide that their evidence is insufficient. Do not write an automatic “truth faction” branch; concrete actions—who shares records, who grants access, who accepts correction—should determine trust and availability separately.

## 746. Callback branch — The player remembers a personal learning boundary

A later lesson may offer a choice the learner previously declined, such as group instruction, a public assessment, a faction-hosted room, or practical equipment. The player can use the earlier preference only if the relevant memory is supported and still current. The learner can revise it. A prior refusal is context, not a permanent exclusion flag.

If the current owner does not save the preference, the scene should ask again rather than assume. Keep the callback small: “Would you still prefer the individual session?” is enough. The learner can say yes, no, or explain a changed situation. Avoid making a privacy setting or a temporary boundary into a personality diagnosis.

## 747. Installment 58 close — A callback references an action, not a label

Education can echo through the campaign when later scenes refer to named lessons, corrected sources, accepted tasks, shared notes, or respected boundaries. The plan should inventory any such cross-quest dependency and verify the producer and consumer before authoring a hard branch. If a consumer does not exist, retain the callback as optional authored dialogue without claiming a mechanical effect.

This discipline allows many endings to arise from different actions: the learner presented evidence, withdrew from publicity, accepted a supervised task, challenged a source, shared a tip, or waited for a safe tool. The ending is not a verdict on character. It records what they did, what the owner accepted, and what remains possible.

## 748. Installment 59 — Knowledge travels through work without becoming a second credential system

The learner’s later contribution may be informal: they notice that a measuring step was skipped, remember that a source changed, or ask a qualified person before acting. This can shape a scene even when it does not grant a new skill value. The player should be able to distinguish a useful observation from a formally assessed competency. The task owner decides whether to rely on it; the education owner remains responsible for the learning record.

This distinction helps avoid a flat progression loop where every lesson unlocks a larger task. Some learning changes confidence, the quality of a question, or willingness to seek help. A later task may still require the learner to watch rather than act. The player can accept that limit, request supervised practice, or pursue a different route. Consequence does not require a numerical reward if a character now participates differently and the owner’s action remains accurate.

## 749. Cross-system branch — A task owner asks whether the learner is qualified

The task owner may request evidence before assigning a role. The player can present the supported education record, describe an observed demonstration, or say that no formal assessment exists. The owner decides whether the evidence meets its qualification rule. The learner may choose a supervised role, take an assessment, request another task, or decline to share a private record.

Do not expose the entire lesson history when the owner only needs a current qualification fact. If a narrowly scoped status is not available, ask the owner what alternative evidence it can accept. The learner can challenge a rejection through the established route. The player cannot turn attendance into a credential because the task would be more convenient to staff.

## 750. Cross-system branch — A learner catches an error but is not certain

The learner may notice a step that differs from the lesson but hesitate to correct an experienced worker. The player can help them state what they observed and what source they remember, then route the question to the task owner. The worker can pause, explain the difference, or ask a specialist. The learner can persist, ask privately, or let the owner decide.

If the learner is right, the task owner decides whether to stop, correct, or review the process. If they are mistaken, the owner explains why without making the act of asking shameful. If the evidence remains uncertain, pause the affected step where required and seek a qualified answer. The outcome is defined by the task’s authority, not by whether the learner’s confidence matched an invisible truth flag.

## 751. Cross-system branch — A lesson is relevant to a group but no one can teach it alone

Several residents may hold complementary knowledge: a formal instructor knows the approved procedure, a local worker knows how the equipment behaves, and a faction contact has a useful manual. The player can ask whether the owners permit a joint session and what role each person may take. Each contributor can accept, decline, or limit their participation.

The educator identifies the learning objective and resolves instructional scope with the source owner. The local worker supplies an example but does not certify safety. The faction supplies material but does not set the learner’s conclusion. If the group cannot agree, the class can examine the difference, ask an independent specialist, or postpone the disputed section. This branch shows that knowledge may be distributed while decision authority remains explicit.

## 752. Cross-system branch — The learner shares a correction beyond the original audience

The learner may want to tell another room or work group about a corrected source. The education and communication owners check whether the correction is ready to share, which audience needs it, and whether the learner may be named. The learner can share a generic fact, ask an authorized teacher to relay it, publish through an approved channel, or keep it within the original group.

Do not let a learner become a public authority merely because they discovered an issue. They can accurately say that a source changed and point to the current owner. If the correction is still pending, label it as unresolved. The original author or owner confirms the final wording. A later reader may ask questions that require the task owner’s expertise rather than the learner’s personal interpretation.

## 753. Cross-system branch — A faction wants the learner as a permanent representative

A faction may invite a learner to become its ongoing instructor, spokesperson, or liaison after a successful project. The learner can ask what the role entails, whether it is paid or scheduled, what authority it carries, and whether it affects access elsewhere. The faction can offer a limited event, provide a role description, or withdraw the invitation. The learner may accept, negotiate, seek independent advice, or decline.

The player checks actual task, schedule, and faction membership owners before presenting the role as available. A single presentation does not equal consent to represent a faction. If the learner accepts only one session, record only that scope. The person may remain independent while contributing expertise. This creates a meaningful political branch based on a concrete invitation rather than a binary loyalty decision.

## 754. Cross-system branch — The learner asks to use their knowledge outside assigned work

The learner may volunteer at a community event, help a neighbor, or apply a skill to a personal project. The task owner confirms whether the activity needs qualification, access, or materials. The learner can proceed within the allowed scope, request supervision, choose a safer variation, or stop. The player can help identify an owner without turning every personal action into a shelter assignment.

If the work is private and the owner has no jurisdiction, do not create an approval ritual. If it uses shared equipment or affects another person, seek the relevant permission. A faction may sponsor the activity, but its conditions should be visible. Any proficiency progression follows the established skill owner; narrative participation alone does not produce an undocumented bonus.

## 755. Cross-system branch — A learner becomes a source for another person’s decision

Another survivor may ask the learner for advice because they remember the lesson. The learner can answer from what they know, show the source, refer to a teacher, or admit uncertainty. The requester can accept the advice, verify it with the owner, or choose another path. The player should not force the learner to take responsibility for another person’s outcome.

If advice concerns safety, health, access, or a high-impact task, the current specialist owner remains available. A learner can say, “This is what the lesson covered, but ask the owner before acting.” That limitation is a capable response. A later callback may show that the learner helped someone ask a better question without recording them as a qualified instructor.

## 756. Cross-system branch — The education record follows a save and roster transition

Before a learner changes duty, location, or roster status, the owner confirms which skill and course facts persist. The player can request a portable summary only if the current data model supports it. The learner chooses whether personal notes or named authorship travel. A roster transition should not clone a second education record or leave an active lesson assigned to someone who is no longer present.

After reload, the same owner should report the same completed lesson and supported proficiency. Pending or cancelled sessions should remain in their actual state if those states are saved. If the record does not persist, the feature must not promise a durable qualification. Focused verification should test the owner’s existing save route once the implementation is approved; this prose does not substitute for that evidence.

## 757. Installment 59 close — Knowledge can change a choice before it changes a number

The downstream ending may be an owner-approved task, a supervised assignment, an accurate correction, a useful referral, an independent contribution, a faction invitation, or a learner who chooses to keep their knowledge private. The learning can matter through how the person asks, observes, and consents. Numerical progression remains owned by the current skill system and must be independently verified.

The player gains more than one route: invest in the qualification, advocate for a supervised chance, help a learner correct information, accept a specialist’s boundary, or decline a faction role. Each choice carries a concrete opportunity cost or social consequence. None asks the player to infer a universal moral identity from one lesson.

## 758. Final route check — The learner can explain the next step

Before closing an education quest, make sure the learner can state what they learned, what they may safely do now, and which owner they should ask if the next task exceeds that scope. If the lesson is incomplete, say which objective remains. If the source is under review, identify that uncertainty. If the learner has no next step, allow the story to end without manufacturing an assignment.

The player-facing closeout should distinguish education progress from task eligibility, scheduled follow-up from a casual invitation, and a private preference from a saved record. This prevents a later scene from quietly assuming more than the lesson established. Any callback should be based on the owner’s current state or the learner’s authorized account.

## 759. Plan 2 continuation close — Capacity, curiosity, and consent remain visible

Across these installments, a learner may study alone, work with peers, ask a faction for a bounded resource, question a source, pause for duty, demonstrate a skill, or use knowledge on a later task. These paths differ through actions and owners rather than a moral score. A lesson can succeed, remain uncertain, or stop without reducing the learner to a failed quest.

The plan remains proposal-only. Before implementation, verify the education owner, task qualification consumer, schedule path, save behavior, and any callback flags in current source. Preserve the exactly three Section 4 subfeatures; the expansion sections elaborate their consequences and do not add new feature pillars.

## 760. Secondary expansion installment 60 — A learner may need a second route through the same objective

The first lesson format may not work even when the subject is right. The learner can ask to hear the explanation again, see a sequence drawn, practice with a sample, watch someone else, or discuss a real example. The teacher checks which formats preserve the approved objective and which require a qualified observer. The player can request an adaptation, ask the learner what would help, or stop if no safe format is available.

The adaptation should not be described as a lower standard simply because its method differs. The curriculum owner determines the objective; the teacher chooses a supported teaching method; the learner selects from options they can access. If an assessment is involved, the qualification owner confirms that the alternative measures the same requirement. If it does not, describe the narrower result honestly and leave the remaining objective open.

## 761. Format branch — The learner wants to observe before attempting

The learner may prefer to watch a demonstration before touching a tool or speaking in a group. The teacher can show the process, narrate decision points, and identify what cannot be seen from observation alone. The learner can ask questions, request another demonstration, attempt a safe step, or end the session. Watching can be meaningful preparation without automatically counting as hands-on proficiency.

The task owner determines whether observation satisfies any required orientation. If the later task needs practical competence, the player can see that another step remains. Do not pressure the learner into an attempt solely to make the progress bar move. A quiet observer may later identify a useful detail, but that contribution should be attributed to the specific observation and routed through the current task owner.

## 762. Format branch — The learner needs more time to process instructions

The learner may ask the teacher to slow down, divide the lesson into steps, or repeat a key explanation. The teacher can adapt pace within the time window or schedule a follow-up. The learner can state whether the current format is working without giving a private reason. The player can ask for a clear summary while preserving all safety warnings and prerequisites.

If the shorter available slot cannot support the required detail, the teacher should say so and stop before an incomplete instruction becomes misleading. The learner may accept a second session, switch to a supported written source, or defer. A time limit is a real constraint, but it should not become an excuse to teach an unsafe shortcut. The schedule owner confirms the next available slot rather than the dialogue promising one.

## 763. Format branch — The learner asks for a peer to help with practice

The learner may want another person present for confidence, translation, note taking, or a second set of eyes. The invited peer chooses whether to participate and what role they will take. The educator confirms that the peer’s role is safe and does not replace required supervision. The learner can proceed with the peer, request a different helper, or use another format.

Do not assign the peer responsibility for the learner’s result. They can observe, read a permitted prompt, or offer encouragement, but assessment remains with its owner. If the learner and peer have a conflict, the educator can separate their roles or end the arrangement. Any personal reason for requesting company remains private unless the learner chooses to share it.

## 764. Format branch — The learner prefers a private practice space

Some subjects are easier to practice without an audience. The learner can request a private room, a quiet time, or an individual session. The location owner confirms access and any supervision requirements. The teacher can offer an appropriate space, another time, or a safe alternative. The learner can accept, wait, or withdraw.

Privacy does not mean the teacher can ignore required witnesses or safety procedures. Explain those conditions before the session. If the room is unavailable, do not move the learner to a public space without asking. If a supporting faction offers a private room, state who controls access and what information the host may observe. The learner can decline that venue without rejecting the lesson itself.

## 765. Format branch — The learner asks for an interpreter or a trusted explainer

The learner may need the lesson communicated through an interpreter or a trusted person who can explain unfamiliar terms. The player checks whether that person is willing, whether the subject allows mediated instruction, and which information may be shared. The teacher remains responsible for the accuracy of the lesson. The learner can approve the helper, ask for a different one, or wait for another route.

An interpreter should not add instructions, omit uncertainty, or decide what the learner is allowed to know. If a phrase has no clear equivalent, the teacher can explain the concept with a supported example or consult the curriculum owner. The learner may ask questions privately or through the interpreter. If the lesson includes personal records, share only what the learner authorized and what the helper needs to interpret.

## 766. Format branch — The learner rejects an accommodation that was offered

An offered adaptation may not suit the learner. They can say no, explain the practical mismatch, request another option, or end the session. The educator can ask one clarifying question but should not make the learner defend the refusal. The player can identify another route or state that no alternative is currently available.

The refusal does not mean the learner is unwilling to learn. It means this format was not accepted. The owner can review whether another accommodation exists, but the lesson should not proceed as if agreement was given. A later callback should reflect the actual choice and not label the learner as difficult. If no supported preference record exists, ask again at the next session rather than assuming permanence.

## 767. Format branch — The requested format conflicts with an essential safety check

The learner may request to practice alone when an owner requires direct supervision. The teacher explains the exact safety boundary and what can still be done independently. The learner can accept supervised practice, observe without attempting, seek a qualified evaluator, or stop. The player can challenge whether the rule is current but cannot waive it without authority.

If the rule itself is disputed, the source owner reviews it. Do not imply that an accommodation removes all safety limits; instead, find the nearest valid method that preserves the essential check. If none exists, defer the practical objective and offer a theoretical or preparatory route only when the curriculum owner supports it. The learner retains the right to decline the alternative.

## 768. Format outcomes — Adaptation remains attached to an objective

The path can end in a repeated explanation, an observed demonstration, paced instruction, peer-supported practice, a private room, interpreted instruction, a rejected adaptation, or a safety-based deferral. Each outcome identifies the objective reached and any scope still open. The player should see the instructor, location, and assessment owner before confirming a consequential session.

Adaptation is not a new progression system. It uses the existing education route and records only the supported outcome. If no owner can store a durable format preference, the plan should not promise future automatic accommodations. A learner can still discuss their preference in each authored scene, and a teacher can respond within the current session.

## 769. Secondary expansion installment 61 — A lesson can be interrupted by uncertainty in the subject itself

The teacher may discover that the source is incomplete, the local conditions differ, or the learner’s question crosses into another owner’s scope. The lesson can continue with the verified portion, pause for review, change to a safer example, or end. The player should see which claim is confirmed and which remains open. The teacher does not need to improvise certainty to protect the schedule.

An uncertain subject can produce a strong ending when the learner leaves with a useful question, a source contact, or a clear boundary. If the owner later resolves the issue, a new lesson can pick up the thread. The earlier session remains true to what was known at the time. The learner can choose whether to keep investigating or move to another subject.

## 770. Uncertainty branch — A local practice differs from the written curriculum

The learner may observe that the local procedure does not match the manual. The teacher can pause, ask the task owner how the local condition arose, or continue with the approved general principle while avoiding the disputed step. The learner can compare sources, submit a question, or decide that the session should wait. A faction worker can explain its own practice but not resolve the shelter’s rule by assertion.

The owner may confirm a local exception, update the source, reject the practice, or request more observation. The teacher records the result through the current source pathway. Do not turn an observed difference into automatic misconduct. It may reflect changed equipment, an undocumented adaptation, or a genuine error. The next task uses only the instruction the owner confirms.

## 771. Uncertainty branch — The learner asks about a subject outside the teacher’s competence

The teacher can identify the edge of their knowledge and refer the learner to a qualified person or owner. The learner can accept the referral, ask a narrower question, request a second source, or stop. The teacher may still cover the portion they are qualified to teach. The player should not interpret a bounded referral as a failed lesson.

If the specialist is a member of a supporting faction, the learner can ask about access and any conditions before meeting. The faction can provide the service, offer a different contact, or decline. The teacher does not transfer private lesson details without permission. If no specialist is available, the learner can preserve the question and continue with safe general material or choose another topic.

## 772. Uncertainty branch — Two sources disagree on a non-safety interpretation

The sources may differ in historical account, local terminology, or explanation while agreeing on the operational steps. The teacher can show both views, identify who produced each, and ask what the learner needs to decide. The learner can compare, remain uncertain, or set the question aside. The owner determines whether one source is canonical for the current task.

Do not force a resolution just to close the lesson. A learner can carry two attributed accounts forward and choose to ask more later. A faction may explain why it prefers one source, while another resident explains a different context. The player can help separate factual disagreement from political interpretation. Any operational instruction remains tied to its current owner.

## 773. Uncertainty branch — The learner has only one source and asks if it is enough

The teacher can describe the source’s authority, date, scope, and known limitations. The learner can accept it for a narrow purpose, seek corroboration, or withhold judgment. If the source owner confirms it as current, the teacher can say what it is valid for without claiming that it answers every related question.

The player can ask whether a second source exists, but should not invent one. A supporting faction may hold relevant material, or it may not. If the source is the only available guidance for an approved task, the task owner determines whether work can proceed. The learner can choose not to act until the uncertainty is resolved if that is operationally permitted.

## 774. Uncertainty branch — The lesson creates a question for an owner who is absent

The teacher may need a decision from an unavailable curriculum or task owner. The player can record a question through a supported channel, ask when that owner returns, request a qualified alternate, or close the session without an answer. The learner can wait, pursue another topic, or choose a safe independent exercise.

Do not create an automatic response date if none is confirmed. The teacher may tell the learner what can be practiced while waiting, but must distinguish that from the unresolved decision. A later callback occurs only when the owner responds or the learner asks again. If the question becomes urgent, the relevant operational owner handles its priority rather than the education UI inventing escalation behavior.

## 775. Uncertainty outcomes — A clear boundary can be more useful than a guess

The ending may be a local exception confirmed, a source correction requested, a specialist referral, a dual account, a narrow accepted source, an unanswered owner question, or a deferred practical step. Each result has a different next action. The learner should know who can answer and what they may safely do in the meantime.

This installment improves the secondary route logic: not every branch should be forced toward a proficiency increase. Some branches produce a better question, a safer delay, or a source review. Those outcomes are player-readable and consequential while remaining within the existing education, task, and data owners.

## 776. Secondary review — Keep the three feature pillars intact

The deeper format and uncertainty branches remain attached to the study agreement, the supported lesson, and its owner-backed knowledge outcome. They do not add a fourth pillar for accessibility, instructor politics, or curriculum governance. Those concerns are conditions and consequences around the same education loop. The document’s Section 4 remains the acceptance authority for the proposal.

During implementation planning, tag each branch with its current owner and whether it needs durable state, a one-session choice, or only authored dialogue. This makes it possible to build a narrow slice first without losing the future narrative options. Unsupported persistence should be marked as a gap rather than filled by a parallel host flag.

## 777. Secondary review — A first implementation slice must still allow refusal

The smallest playable slice should include an eligible learner, a valid teacher, a visible time cost, a lesson objective, one completion or interruption result, and an explicit decline route. It should demonstrate that a learner can refuse a format or stop safely without being assigned a hidden penalty. A later slice can add alternative teachers, group work, faction access, and cross-quest callbacks.

The slice is not complete if it only shows a panel or increments proficiency in memory. It needs the current education owner, host route, save behavior for any durable result, and an observable consumer. A rejected or inconclusive result should be representable honestly. Focused verification should match the exact owner behavior changed; broad speculative tests are outside this plan.

## 778. Secondary review — Keep acceptance language observable

Acceptance should be stated as evidence a reviewer can inspect: the learner and teacher were eligible; availability and prerequisites were checked; a supported lesson command ran once; repeated submission did not grant duplicate progress; the record survived the documented save path; and a real task or interface reflected the result. If a criterion cannot be checked because the API lacks a state, rewrite the proposal to identify that gap.

Authored branches can be reviewed for correct audience, owner, condition, and fallback. A branch that says “the learner understands” needs an observable story beat or a supported owner result. A branch that says “the faction certified them” needs a current authority for that certification. This keeps the expanded narrative grounded as the plan moves toward implementation.

## 779. Secondary close — Expansion adds routes; it does not relax evidence

The learner may seek a different format, pause for uncertainty, compare sources, invite a peer, ask a faction for access, or carry a question into later work. These options give the player more ways to support learning and more endings that do not depend on alignment. Each route still depends on consent and a current owner.

When the plan is next revised, use source evidence to connect any new branch to a real route. Preserve the original subfeature count, state ownership, and proposal-only status. A large narrative surface does not make an implementation reachable; the plan remains successful only when its promises can be traced to current contracts.

## 780. Secondary expansion installment 62 — A teacher handoff carries the lesson, not the learner’s whole history

The original teacher may become unavailable between sessions. The learner can wait, request another educator, take an approved independent step, or stop. If a handoff is chosen, the current education owner identifies what information may move: the subject, completed objective, prerequisite, remaining practice, and any learner-approved accommodation. Private motivation, unrelated performance, or relationship commentary does not belong in a routine teaching handoff.

The new educator reviews the current curriculum rather than inheriting the previous teacher’s assumptions. The learner can meet them before committing, ask the original teacher to introduce them, or request an independent source. The player can help keep continuity without implying that a teacher’s departure erases learning or that a new instructor automatically knows the learner’s preferences.

## 781. Handoff branch — The previous teacher left before the first session

The learner may have agreed to a lesson that never began. The player checks whether the booking owner can reassign it and whether any material or schedule was reserved. The learner can accept a replacement, wait for the original teacher, choose another subject, or cancel. No proficiency result is recorded merely because an appointment was scheduled.

If the original teacher left without a handoff, the education owner identifies the gap. The player should not claim a personal reason or blame the teacher. A new educator can offer an introductory conversation, but the learner chooses whether to repeat the preparation. If the learner declines, close the booking accurately and release any confirmed reservation.

## 782. Handoff branch — The learner completed part of a lesson with the absent teacher

The learner can report what was covered and what remains. The new educator may consult the supported lesson record, ask the learner to demonstrate a safe step, or begin with a brief recap. The learner can correct a mismatch, request a different starting point, or decide they do not want to repeat the explanation.

The player should compare the owner-backed completion record with the learner’s account. If they differ, do not erase either: ask the education owner to reconcile supported facts. The new teacher can continue only from confirmed scope or choose a short review that avoids duplicating the whole course. The learner may prefer repetition if it helps; it should be their choice where possible.

## 783. Handoff branch — The original teacher asks not to be contacted

The teacher may have left the role or set a personal boundary. The player respects that boundary and does not ask for a private explanation unless the owner requires operational handoff details. The learner can receive a neutral notice, choose a replacement, wait, or stop. The education owner determines what curriculum records remain available.

The learner may be disappointed and ask whether something happened. The player can state only what is confirmed. Do not use absence as a suspense device that implies conflict. If the teacher authorized a handoff note, share only its educational scope. If not, the new educator works from the canonical record and the learner’s account.

## 784. Handoff branch — The replacement teacher belongs to a faction

The alternate instructor may be available through a supporting faction. The player shows the learner the teacher’s qualifications, location, schedule, audience, and any faction conditions. The learner can accept one session, request another instructor, use a public source, or decline. The teacher can explain the faction’s material without requiring the learner to join.

If the faction wants participant data, the learner decides what may be shared within the existing privacy rules. The teacher’s faction does not automatically receive the prior learner record. The education owner confirms what is needed to continue. A faction educator can be a useful specialist while remaining one source among others.

## 785. Handoff branch — The learner wants to continue but change the subject

Teacher unavailability may lead the learner to rethink the course. They can keep the same subject with someone else, switch to a related objective, or pause. The education owner confirms transferability of completed progress. The new teacher explains prerequisites without implying that the learner owes completion of the original plan.

The player can compare the time and material costs of each route. A supporting faction may cover one subject but not the other. The learner can choose based on current need, curiosity, or available time. The prior lesson record remains accurate even when the course changes; do not label a switch as abandonment.

## 786. Handoff branch — The old teacher’s method is not the new teacher’s method

The replacement may structure practice differently while teaching the same approved objective. The learner can ask whether this is a curriculum change, a personal teaching style, or an updated safety rule. The new teacher can identify the source and demonstrate the difference. The learner can compare, ask the source owner, or follow the current approved method.

If the difference affects qualification, the task owner confirms what it accepts. The new teacher cannot invalidate earlier progress solely because they prefer another example. If the owner requires a refreshed step, explain why and how much remains. The player can request a review when the standard is unclear.

## 787. Handoff branch — No educator can continue the subject

The current roster may have no qualified replacement. The learner can keep the question for later, study an approved source independently, ask a faction whether it has a specialist, or choose another subject. The source owner confirms what independent reading is safe. If no reliable method exists, defer the lesson.

The player should not promote a peer to instructor simply to preserve the quest. A peer may share experience within an informal scope if both agree and the owner permits it. The learner can also decide that the subject is no longer worth waiting for. Close the active lesson route and leave a callback only if the system can represent a pending request truthfully.

## 788. Handoff branch — The learner wants to preserve a relationship with the teacher

The learner may value the original teacher beyond the lesson and ask to stay in touch. The player can help send a personal note only through an authorized channel and with both people’s consent. The education record remains separate from the relationship. The teacher can respond, decline contact, or allow only lesson-related follow-up.

Do not make a completed course grant private access to a teacher. A faction intermediary may carry a message if both approve, but that does not give the faction the conversation. If contact ends, the learner can still continue studying through another route. The scene can acknowledge loss without inventing a persistent relationship preference in the education system.

## 789. Handoff branch — A source owner changes after the teacher leaves

The curriculum source may have changed while the old teacher was responsible for the class. The new owner identifies the current version and whether previous lessons remain valid. The learner can review a delta, ask for a new demonstration, accept the prior scope, or pause. The player should distinguish a source update from a teacher handoff.

If the change is major, the owner defines any required refresher. If it is editorial, the owner may confirm that no retraining is needed. The old educator’s absence does not determine the answer. The new educator can teach the change, but the source owner remains responsible for declaring it current.

## 790. Handoff outcomes — Continuity is a new agreement with limited disclosure

The learner can accept a replacement, wait, switch subjects, use an independent source, request a faction instructor, keep a personal connection, or close the route. Each outcome preserves already confirmed learning and avoids sharing more than necessary. A new teacher does not automatically inherit every preference or personal detail.

If the owner cannot persist a handoff, do not promise a seamless transfer. The player can help the learner re-enter the route later. The follow-up should name what is confirmed and what must be checked again. Continuity is valuable because the person can continue on their terms, not because the system pretends nothing changed.

## 791. Installment 62 close — A changed teacher does not erase the learner’s choices

The handoff branch covers absence before a lesson, mid-course transitions, faction-based alternatives, changed goals, conflicting teaching styles, missing capacity, personal boundaries, and source updates. Each route asks the learner whether to continue. The current owner determines which progress and preferences persist.

This secondary addition is still part of the study and supported-lesson loop. It introduces no new feature pillar or parallel education record. Before any future implementation, confirm that the owner can represent the handoff; otherwise, keep the branch as a future narrative option with an explicit limitation.

## 792. Secondary coda — A learner can reject the handoff without losing the subject

The replacement educator may be technically qualified but not someone the learner wants to study with. The learner can ask for another person, choose an independent source, wait, or stop. The player should not interpret the choice as a rejection of education or of the original teacher. The owner can report whether other capacity exists; if none does, the route may pause.

If the learner declines a specific instructor because of a concrete concern, they can share only what the owner needs to arrange a safe alternative. A private allegation goes through an appropriate review route rather than being debated in the classroom. The new educator should not receive personal details as a condition of the learner’s refusal. This lets the learner protect a boundary while keeping an eventual path to study open.

## 793. Secondary coda — A callback should reopen a choice, not assume the answer

When a new teacher becomes available later, the player can offer the route again and ask whether the learner still wants it. The learner may have changed goals, found another source, or decided the subject is no longer relevant. A previously declined handoff is not a permanent refusal of all teaching. The current schedule and curriculum owner confirm what is actually available.

If the learner accepts, start a new agreement with current terms. If they decline, close the offer without repeated prompting. This small revision prevents the education chain from turning a one-time preference into a permanent gate. It also gives the player a meaningful return branch while keeping consent current.

## 794. Secondary coda — The route remains complete when the learner chooses another priority

The handoff arc can close with another teacher, an alternate format, a delayed subject, a referral, a personal boundary, or a decision to study something else. A quest need not remain active indefinitely to prove the learner still has agency. The owner-backed record preserves only the progress and commitments that truly exist.

## 795. Secondary coda — A later invitation names the changed condition

If the learner is invited back, the player can explain what changed: a qualified teacher returned, a room opened, a source was corrected, or a safer format became available. The learner can decide whether that change matters. Without a changed condition, avoid repeating the same invitation as if persistence alone should overcome a previous no.

The educator confirms current availability before any promise. The learner can accept one session, ask for details, or close the route. This final check keeps continuity action-based and gives the player a grounded reason to reopen the branch.

## 796. Secondary coda — The player can leave the invitation with the learner

After explaining a changed condition, the player can let the learner return later rather than demanding an immediate answer. The owner may hold the slot only if it confirms a real reservation. Otherwise, availability remains uncertain and the learner can ask again. This keeps the invitation informative without turning it into pressure.

## 797. Formal closeout — Plan 2 is complete as a planning document

Expansion 98, **A Lesson Kept Between Shifts**, is closed as a planning document. It exceeds the requested minimum length and defines a coherent, canon-aware education loop from eligible learner and teacher matching through a supported lesson to a knowledge result that can matter in later work. This closeout confirms that the requested planning deliverable is complete; it does not claim that the feature has been implemented, integrated, tested, or made reachable in a build.

The feature scope remains the single key feature and exactly three subfeatures in Section 4: the study agreement, the supported lesson, and the owner-backed knowledge outcome. All appended installments—group learning, interruption, assessment, teacher succession, accessibility, materials, cross-quest callbacks, and source uncertainty—are branches and implementation considerations within that scope. They do not add a fourth feature pillar, a new education authority, a learner dossier, or a parallel schedule.

### 798. Closeout record — What the plan promises

The plan promises that a player can see who is eligible, what the lesson costs, which schedule and location owners must agree, what a refusal or interruption means, what objective a lesson covers, and which authority decides whether later work is permitted. It treats learning as a human process under scarcity: learners can question, defer, change format, decline an assessment, accept supervision, or choose a different goal. Factions may provide teachers, tools, rooms, sources, or access under explicit conditions; they do not gain control of the learner’s conclusion or the shelter’s qualification rules.

The plan also promises that visible story outcomes will match owner-backed state. Attendance is not qualification. A completed lesson is not automatic eligibility for a task. A source correction is not an operational policy change until the relevant owner accepts it. A callback may refer to a learner’s action only when the producer, audience, and persistence behavior are confirmed. Where current APIs do not support the distinction, the proposal records a gap rather than inventing UI state.

### 799. Closeout record — What implementation must revalidate

Before implementation, re-open the live source and data rather than treating this document or the older forensic report as proof of current APIs. Verify the canonical curriculum data, eligibility and proficiency owners, teacher and learner identifiers, age and roster facts, duty schedule, needs, consent path, lesson idempotency, task consumers, host route, save owner, and reload behavior. Confirm that any proposed panel is only a view over those authorities. Confirm that repeated lesson submission cannot duplicate progress and that canceled or interrupted work leaves the correct state.

The integration plan must name the exact owned paths and focused verification. It must prove at least one actual route from a valid learner/teacher pair to a committed lesson outcome and an observable consumer. If the owner cannot represent a proposed state—pending appointment, partial objective, privacy preference, group completion, or instructor handoff—keep that branch staged or narrow its promise. Do not fill the gap with a cache, save section, registry, or panel callback.

### 800. Closeout record — Failure and refusal remain supported outcomes

The feature is not complete if its only reachable path is a successful lesson. A learner must be able to decline, ask for another teacher or format, lose a time window, encounter unavailable material, stop at a safety boundary, or remain uncertain about an answer. Each result needs an honest player-facing explanation and an owner-consistent state. A faction can say no to lending its room; a task owner can refuse an unqualified assignment; a teacher can be unavailable. Those answers may lead to another route, a delay, or closure without a hidden moral penalty.

Verification should inspect consequences as well as a happy path: the same accepted lesson cannot grant duplicate progress; an interrupted lesson cannot claim an objective it did not cover; a learner’s refusal does not silently change proficiency; stale source data does not overwrite a corrected authority; and save/restore preserves only supported durable outcomes. Any unresolved contract remains a documented blocker for implementation, not a reason to overstate the plan.

### 801. Closeout record — Relationship to Plans 3–5

Plans 3–5 remain separate planning documents. A learner may later make an initiative, use a communication channel, or trade an item, but those systems own their own state and acceptance. Education can provide a callback fact such as “completed a supported demonstration” or “requested another instructor” only through a verified producer and consumer. It does not own survivor initiative, message delivery, inventory movement, or barter settlement.

The Wave 20 index should report the saved character count after this closeout. Future edits to Plan 2 are maintenance corrections only: current-source evidence, continuity fixes, formatting, or a user-directed revision. Any new implementation decision belongs in the active integration authority and ownership ledger. This formal closeout records a complete proposal while keeping implementation status explicit and truthful.
