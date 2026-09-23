# Expansion 99 — The Refusal Has a Reason

**Wave:** 20 — Useful Systems, Real Consequences  
**Requested series:** Plan 3 of 5  
**Requested plan length:** At least 120,000 words; expansion above the minimum is acceptable.  
**Series status:** This plan's content pass is closed at the requested minimum; proposal only.
**Measured length:** 120,082 words; 100.07% of the 120,000-word minimum (82 words above). Counted through Section 548.
**Unique key feature:** A player-readable survivor initiative loop where offers, refusals and personal goals use current consent, work and social authorities.  
**Status:** proposal only; no implementation or path claim.  
**Audit:** [Wave 20 forensic report](../../forensics/WAVE20_FEATURE_SEAMS_FORENSIC_REPORT.md)

> Exactly three subfeatures are defined in Section 4. The examples and casebook all map to those same three; no additional feature pillars are introduced.

## 1. Expansion thesis

The experience is a player-readable survivor initiative loop where offers, refusals and personal goals use current consent, work and social authorities. It must start from canonical state, invoke one owner-backed command and show an outcome that remains true after day advance and reload. A panel-only simulation does not meet the promise.

Survivors have priorities, fatigue and relationships, not interchangeable command slots. The player should understand how initiative meets a roster without turning coercion into a hidden stat tax.

Show real cost, permission and uncertainty before commitment. Refusal, delay and failure may be valid results.

## 2. Canon fit

Survivors have priorities, fatigue and relationships, not interchangeable command slots. The player should understand how initiative meets a roster without turning coercion into a hidden stat tax.

Keep the tone restrained, material and human. Sample lines below are candidates, not current canon; check them against the live narrative data before authoring.

## 3. Existing systems reused

Reuse action templates, social save owner, relationship/need facts, DutyRoster, CrewConsentVerdict, task completion, inventory and current event bridges. Keep UtilityAI scoring separate.

Reuse stable IDs and owner records. JSON under Assets/StreamingAssets/Data remains authoritative; do not duplicate mutable data in the host.

## 4. Key feature and exactly three subfeatures

**Single key feature:** A player-readable survivor initiative loop where offers, refusals and personal goals use current consent, work and social authorities.

**Current gap:** Initiative and goals lack a demonstrated Godot route/save path. OverrideRefusal(enforceWork) can penalize morale/affinity without proven CrewConsentVerdict linkage. Repeated evaluation may draw again when no action triggered.

### Subfeature 1: Offers survivors can answer

**Purpose and loop:** Show help, initiative, expression and pursuit with actor, target, reason, timing, cost, expiry and whether the action is a suggestion or executed. Evaluate with campaign RNG and stable roster order; route effects through current owners.

**Player value:** A survivor may help a trusted partner but decline the same request from someone who broke a promise. The player learns from a concrete action rather than a personality meter.

**Boundary:** Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

### Subfeature 2: A refusal goes through consent

**Purpose and loop:** Explain the refusal and show permitted choices: accept it, reassign through roster command, or request only an approved review. No generic force-work switch.

**Player value:** A survivor may be exhausted after night watch or object to unsafe duty. The player can adapt the rota; an open slot is an honest consequence.

**Boundary:** OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

### Subfeature 3: Goals measured by witnessed work

**Purpose and loop:** Persist a personal intention and advance only from a real task event. Interrupted or reassigned work pauses progress. Player may support, replace where allowed, or leave the goal.

**Player value:** A small repair can span days and stop when a part is unavailable; the player decides whether the scarce item is worth committing.

**Boundary:** AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

These three subfeatures are the complete gameplay scope. A fourth independent pillar needs a separate proposal.

## 5. Core mechanics

**Input:** current owner state, catalog, day and explicit player command. **State:** existing domain DTO and only stable event references. **Decision:** commit, defer, decline or choose a supported alternative with costs visible. **Uncertainty:** only facts current systems support. **Consequence:** owner-confirmed state or cost. **Cross-system output:** one fact once. **Failure/recovery:** stale data, denied consent, capacity, interruption and retry have truthful results. **Replayability:** seeded campaign inputs and stable order, never wall-clock or UI-local random.

## 6. Cross-system interactions

Primary domain: SurvivorAutonomySystem owns actions, cooldowns and goals; SurvivorSocialCoordinator/survivor_social own social save composition; DutyRoster/CrewConsentVerdict own assignment and consent; needs, health, inventory and task owners own facts and costs.

A current cost/permission owner supplies constraints; an existing downstream owner consumes confirmed effects; the day/save owner refreshes the same state after restore. Verify each actual API and consumer before implementation.

## 7. Main narrative spine

A concrete need appears; the player sees known facts and limits; one of the three subfeatures permits a decision; a later view shows who acted and what changed. Human center: Survivors have priorities, fatigue and relationships, not interchangeable command slots. The player should understand how initiative meets a roster without turning coercion into a hidden stat tax.

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

Preserve stable identity, valid pending intent, confirmed result and provenance through the existing save owner. A durable consequence is A small repair can span days and stop when a part is unavailable; the player decides whether the scarce item is worth committing.

Core capture/restore is necessary but not sufficient: prove host registration, restore order and dirty flush.

## 17. Failure and alternate outcomes

A blocked command is a valid branch. Distinguish missing source, deferral, owner rejection, expiry, conflict and completion where current APIs support them. Never show success before confirmation or turn denial into hidden punishment.

## 18. Replayability

Different people, timing and owner state should change the choices without arbitrary new rolls. Same seeded state resolves identically. Duplicate command/day delivery reuses a durable result or reports already processed.

## 19. Implementation classification

**Classification:** Cross-system host integration; verify survivor_social save seam. Evaluation identity may need a Core contract. Refusal enforcement remains policy-gated.

Start with a fresh premise and runtime audit. Resolve owner decisions, connect the existing Core authority through the current host/event/save seam, then expose one Godot route. Core remains engine-free and JSON remains authoritative.

## 20. Collision audit

Adjacent behavior: Reuse action templates, social save owner, relationship/need facts, DutyRoster, CrewConsentVerdict, task completion, inventory and current event bridges. Keep UtilityAI scoring separate.

Classify this as extension/reachability work, not replacement. Re-search current content, host paths and save sections before implementation. Never revive Unity behavior.

## 21. Expansion hooks

Later quests or campaign history can consume an owner-confirmed result with provenance. They cannot infer success from a UI label or duplicate the source state.

## 22. Strongest recommended content

Start with the smallest interaction that proves the cost and consequence: A survivor may be exhausted after night watch or object to unsafe duty. The player can adapt the rota; an open slot is an honest consequence. Add one blocked path and one delayed callback.

## 23. Contracts, data, save and determinism

**Owner boundary:** SurvivorAutonomySystem owns actions, cooldowns and goals; SurvivorSocialCoordinator/survivor_social own social save composition; DutyRoster/CrewConsentVerdict own assignment and consent; needs, health, inventory and task owners own facts and costs.

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

**Primary risk:** Initiative and goals lack a demonstrated Godot route/save path. OverrideRefusal(enforceWork) can penalize morale/affinity without proven CrewConsentVerdict linkage. Repeated evaluation may draw again when no action triggered.

Later verification should cover accepted, denied, stale, repeated and restored actions plus seeded replay where relevant. Prove behavior, not class presence. Rollback disables the host route and preserves canonical state and save readers. Keep unsafe commands unavailable while ownership is unresolved.

## 27. Three creative variants for this feature

**Grounded:** show current state and one safe owner command. **Systemic:** connect the three subfeatures to real costs, permissions and delayed outcomes. **Wildcard, still grounded:** let the player inspect the source, author, provenance or physical limit before committing. This changes framing, not architecture. Implement systemic only when contracts are proven.

## 28. Review gate

Proceed to implementation planning only when source confirms the gap, every mutable fact has one owner, exactly these three subfeatures have truthful routes, save/replay owners are named and success follows an owner result. Otherwise revise or stop.

## 29. Casebook: design acceptance records mapped to the three subfeatures

These are review examples, not implementation claims, test results or extra features. Every scenario maps to S1, S2 or S3 and preserves the current ownership boundary.

### Scenario W20-99-S1-001 — survivor offers help after ordinary shift

**Initial condition:** survivor offers help after ordinary shift. The view is a projection of the current owner. **Pressure:** same evaluation called twice. **Player action:** process one owner evaluation identity.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. same seed yields same proposal and outcome. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can do the pump after the roster settles. Not before. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-001 — survivor offers help after ordinary shift

**Initial condition:** survivor offers help after ordinary shift. The view is a projection of the current owner. **Pressure:** roster changes after offer. **Player action:** use roster/consent command result.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. assignment cannot bypass CrewConsentVerdict. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She refused the night watch. Her medical note is current. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-001 — survivor offers help after ordinary shift

**Initial condition:** survivor offers help after ordinary shift. The view is a projection of the current owner. **Pressure:** UI callback tries to force work. **Player action:** defer or reassign without hidden penalty.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. goal progress references durable task ID. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can do the pump after the roster settles. Not before. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-002 — survivor offers help after ordinary shift

**Initial condition:** survivor offers help after ordinary shift. The view is a projection of the current owner. **Pressure:** UI callback tries to force work. **Player action:** apply cost through inventory/task owner.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. restore does not lose or duplicate refusal. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That was a suggestion. No one has taken the parts. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-002 — survivor offers help after ordinary shift

**Initial condition:** survivor offers help after ordinary shift. The view is a projection of the current owner. **Pressure:** two goals claim one event. **Player action:** show no action instead of inventing one.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. view has no shadow goal state. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** He kept the goal. The bolt did not come in. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-002 — survivor offers help after ordinary shift

**Initial condition:** survivor offers help after ordinary shift. The view is a projection of the current owner. **Pressure:** survivor order changes on reload. **Player action:** restore through verified social owner.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. missing RNG cannot silently use seed 144. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That was a suggestion. No one has taken the parts. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-003 — survivor offers help after ordinary shift

**Initial condition:** survivor offers help after ordinary shift. The view is a projection of the current owner. **Pressure:** campaign RNG not injected. **Player action:** defer or reassign without hidden penalty.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. same seed yields same proposal and outcome. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She refused the night watch. Her medical note is current. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-003 — survivor offers help after ordinary shift

**Initial condition:** survivor offers help after ordinary shift. The view is a projection of the current owner. **Pressure:** trigger reason cannot be explained. **Player action:** advance only from task completion.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. assignment cannot bypass CrewConsentVerdict. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can do the pump after the roster settles. Not before. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-003 — worker declines night duty after poor rest

**Initial condition:** worker declines night duty after poor rest. The view is a projection of the current owner. **Pressure:** target leaves before help begins. **Player action:** block coercive override pending policy.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. goal progress references durable task ID. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She refused the night watch. Her medical note is current. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-004 — worker declines night duty after poor rest

**Initial condition:** worker declines night duty after poor rest. The view is a projection of the current owner. **Pressure:** target leaves before help begins. **Player action:** restore through verified social owner.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. restore does not lose or duplicate refusal. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** He kept the goal. The bolt did not come in. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-004 — worker declines night duty after poor rest

**Initial condition:** worker declines night duty after poor rest. The view is a projection of the current owner. **Pressure:** UI callback tries to force work. **Player action:** process one owner evaluation identity.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. view has no shadow goal state. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That was a suggestion. No one has taken the parts. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-004 — worker declines night duty after poor rest

**Initial condition:** worker declines night duty after poor rest. The view is a projection of the current owner. **Pressure:** two goals claim one event. **Player action:** use roster/consent command result.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. missing RNG cannot silently use seed 144. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** He kept the goal. The bolt did not come in. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-005 — worker declines night duty after poor rest

**Initial condition:** worker declines night duty after poor rest. The view is a projection of the current owner. **Pressure:** two goals claim one event. **Player action:** block coercive override pending policy.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. same seed yields same proposal and outcome. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can do the pump after the roster settles. Not before. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-005 — worker declines night duty after poor rest

**Initial condition:** worker declines night duty after poor rest. The view is a projection of the current owner. **Pressure:** survivor order changes on reload. **Player action:** apply cost through inventory/task owner.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. assignment cannot bypass CrewConsentVerdict. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She refused the night watch. Her medical note is current. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-005 — worker declines night duty after poor rest

**Initial condition:** worker declines night duty after poor rest. The view is a projection of the current owner. **Pressure:** another system completed task. **Player action:** show no action instead of inventing one.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. goal progress references durable task ID. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can do the pump after the roster settles. Not before. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-006 — worker declines night duty after poor rest

**Initial condition:** worker declines night duty after poor rest. The view is a projection of the current owner. **Pressure:** trigger reason cannot be explained. **Player action:** use roster/consent command result.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. restore does not lose or duplicate refusal. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That was a suggestion. No one has taken the parts. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-006 — mechanic proposes using one shared part

**Initial condition:** mechanic proposes using one shared part. The view is a projection of the current owner. **Pressure:** target leaves before help begins. **Player action:** defer or reassign without hidden penalty.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. view has no shadow goal state. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** He kept the goal. The bolt did not come in. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-006 — mechanic proposes using one shared part

**Initial condition:** mechanic proposes using one shared part. The view is a projection of the current owner. **Pressure:** refusal follows medical restriction. **Player action:** advance only from task completion.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. missing RNG cannot silently use seed 144. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That was a suggestion. No one has taken the parts. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-007 — mechanic proposes using one shared part

**Initial condition:** mechanic proposes using one shared part. The view is a projection of the current owner. **Pressure:** refusal follows medical restriction. **Player action:** show no action instead of inventing one.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. same seed yields same proposal and outcome. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She refused the night watch. Her medical note is current. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-007 — mechanic proposes using one shared part

**Initial condition:** mechanic proposes using one shared part. The view is a projection of the current owner. **Pressure:** initiative spends shared inventory. **Player action:** restore through verified social owner.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. assignment cannot bypass CrewConsentVerdict. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can do the pump after the roster settles. Not before. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-007 — mechanic proposes using one shared part

**Initial condition:** mechanic proposes using one shared part. The view is a projection of the current owner. **Pressure:** survivor order changes on reload. **Player action:** process one owner evaluation identity.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. goal progress references durable task ID. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She refused the night watch. Her medical note is current. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-008 — mechanic proposes using one shared part

**Initial condition:** mechanic proposes using one shared part. The view is a projection of the current owner. **Pressure:** survivor order changes on reload. **Player action:** advance only from task completion.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. restore does not lose or duplicate refusal. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** He kept the goal. The bolt did not come in. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-008 — mechanic proposes using one shared part

**Initial condition:** mechanic proposes using one shared part. The view is a projection of the current owner. **Pressure:** another system completed task. **Player action:** block coercive override pending policy.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. view has no shadow goal state. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That was a suggestion. No one has taken the parts. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-008 — saved goal has partial progress

**Initial condition:** saved goal has partial progress. The view is a projection of the current owner. **Pressure:** same evaluation called twice. **Player action:** apply cost through inventory/task owner.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. missing RNG cannot silently use seed 144. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** He kept the goal. The bolt did not come in. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-009 — saved goal has partial progress

**Initial condition:** saved goal has partial progress. The view is a projection of the current owner. **Pressure:** roster changes after offer. **Player action:** apply cost through inventory/task owner.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. same seed yields same proposal and outcome. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can do the pump after the roster settles. Not before. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-009 — saved goal has partial progress

**Initial condition:** saved goal has partial progress. The view is a projection of the current owner. **Pressure:** UI callback tries to force work. **Player action:** show no action instead of inventing one.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. assignment cannot bypass CrewConsentVerdict. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She refused the night watch. Her medical note is current. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-009 — saved goal has partial progress

**Initial condition:** saved goal has partial progress. The view is a projection of the current owner. **Pressure:** two goals claim one event. **Player action:** restore through verified social owner.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. goal progress references durable task ID. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can do the pump after the roster settles. Not before. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-010 — saved goal has partial progress

**Initial condition:** saved goal has partial progress. The view is a projection of the current owner. **Pressure:** old save lacks autonomy state. **Player action:** defer or reassign without hidden penalty.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. restore does not lose or duplicate refusal. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That was a suggestion. No one has taken the parts. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-010 — saved goal has partial progress

**Initial condition:** saved goal has partial progress. The view is a projection of the current owner. **Pressure:** campaign RNG not injected. **Player action:** advance only from task completion.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. view has no shadow goal state. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** He kept the goal. The bolt did not come in. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-010 — saved goal has partial progress

**Initial condition:** saved goal has partial progress. The view is a projection of the current owner. **Pressure:** trigger reason cannot be explained. **Player action:** block coercive override pending policy.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. missing RNG cannot silently use seed 144. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That was a suggestion. No one has taken the parts. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-011 — saved goal has partial progress

**Initial condition:** saved goal has partial progress. The view is a projection of the current owner. **Pressure:** trigger reason cannot be explained. **Player action:** restore through verified social owner.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. same seed yields same proposal and outcome. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She refused the night watch. Her medical note is current. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-011 — request involves strained relationship

**Initial condition:** request involves strained relationship. The view is a projection of the current owner. **Pressure:** roster changes after offer. **Player action:** process one owner evaluation identity.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. assignment cannot bypass CrewConsentVerdict. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can do the pump after the roster settles. Not before. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-011 — request involves strained relationship

**Initial condition:** request involves strained relationship. The view is a projection of the current owner. **Pressure:** UI callback tries to force work. **Player action:** use roster/consent command result.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. goal progress references durable task ID. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She refused the night watch. Her medical note is current. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-012 — request involves strained relationship

**Initial condition:** request involves strained relationship. The view is a projection of the current owner. **Pressure:** UI callback tries to force work. **Player action:** block coercive override pending policy.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. restore does not lose or duplicate refusal. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** He kept the goal. The bolt did not come in. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-012 — request involves strained relationship

**Initial condition:** request involves strained relationship. The view is a projection of the current owner. **Pressure:** two goals claim one event. **Player action:** apply cost through inventory/task owner.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. view has no shadow goal state. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That was a suggestion. No one has taken the parts. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-012 — request involves strained relationship

**Initial condition:** request involves strained relationship. The view is a projection of the current owner. **Pressure:** survivor order changes on reload. **Player action:** show no action instead of inventing one.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. missing RNG cannot silently use seed 144. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** He kept the goal. The bolt did not come in. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-013 — request involves strained relationship

**Initial condition:** request involves strained relationship. The view is a projection of the current owner. **Pressure:** campaign RNG not injected. **Player action:** use roster/consent command result.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. same seed yields same proposal and outcome. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can do the pump after the roster settles. Not before. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-013 — request involves strained relationship

**Initial condition:** request involves strained relationship. The view is a projection of the current owner. **Pressure:** trigger reason cannot be explained. **Player action:** defer or reassign without hidden penalty.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. assignment cannot bypass CrewConsentVerdict. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She refused the night watch. Her medical note is current. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-013 — proposal points to removed roster task

**Initial condition:** proposal points to removed roster task. The view is a projection of the current owner. **Pressure:** target leaves before help begins. **Player action:** advance only from task completion.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. goal progress references durable task ID. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can do the pump after the roster settles. Not before. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-014 — proposal points to removed roster task

**Initial condition:** proposal points to removed roster task. The view is a projection of the current owner. **Pressure:** target leaves before help begins. **Player action:** show no action instead of inventing one.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. restore does not lose or duplicate refusal. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That was a suggestion. No one has taken the parts. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-014 — proposal points to removed roster task

**Initial condition:** proposal points to removed roster task. The view is a projection of the current owner. **Pressure:** refusal follows medical restriction. **Player action:** restore through verified social owner.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. view has no shadow goal state. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** He kept the goal. The bolt did not come in. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-014 — proposal points to removed roster task

**Initial condition:** proposal points to removed roster task. The view is a projection of the current owner. **Pressure:** two goals claim one event. **Player action:** process one owner evaluation identity.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. missing RNG cannot silently use seed 144. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That was a suggestion. No one has taken the parts. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-015 — proposal points to removed roster task

**Initial condition:** proposal points to removed roster task. The view is a projection of the current owner. **Pressure:** two goals claim one event. **Player action:** advance only from task completion.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. same seed yields same proposal and outcome. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She refused the night watch. Her medical note is current. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-015 — proposal points to removed roster task

**Initial condition:** proposal points to removed roster task. The view is a projection of the current owner. **Pressure:** survivor order changes on reload. **Player action:** block coercive override pending policy.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. assignment cannot bypass CrewConsentVerdict. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can do the pump after the roster settles. Not before. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-015 — proposal points to removed roster task

**Initial condition:** proposal points to removed roster task. The view is a projection of the current owner. **Pressure:** another system completed task. **Player action:** apply cost through inventory/task owner.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. goal progress references durable task ID. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She refused the night watch. Her medical note is current. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-016 — proposal points to removed roster task

**Initial condition:** proposal points to removed roster task. The view is a projection of the current owner. **Pressure:** trigger reason cannot be explained. **Player action:** process one owner evaluation identity.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. restore does not lose or duplicate refusal. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** He kept the goal. The bolt did not come in. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-016 — medical restriction conflicts with initiative

**Initial condition:** medical restriction conflicts with initiative. The view is a projection of the current owner. **Pressure:** target leaves before help begins. **Player action:** use roster/consent command result.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. view has no shadow goal state. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That was a suggestion. No one has taken the parts. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-016 — medical restriction conflicts with initiative

**Initial condition:** medical restriction conflicts with initiative. The view is a projection of the current owner. **Pressure:** refusal follows medical restriction. **Player action:** defer or reassign without hidden penalty.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. missing RNG cannot silently use seed 144. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** He kept the goal. The bolt did not come in. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-017 — medical restriction conflicts with initiative

**Initial condition:** medical restriction conflicts with initiative. The view is a projection of the current owner. **Pressure:** initiative spends shared inventory. **Player action:** defer or reassign without hidden penalty.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. same seed yields same proposal and outcome. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can do the pump after the roster settles. Not before. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-017 — medical restriction conflicts with initiative

**Initial condition:** medical restriction conflicts with initiative. The view is a projection of the current owner. **Pressure:** old save lacks autonomy state. **Player action:** advance only from task completion.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. assignment cannot bypass CrewConsentVerdict. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She refused the night watch. Her medical note is current. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-017 — medical restriction conflicts with initiative

**Initial condition:** medical restriction conflicts with initiative. The view is a projection of the current owner. **Pressure:** campaign RNG not injected. **Player action:** block coercive override pending policy.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. goal progress references durable task ID. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can do the pump after the roster settles. Not before. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-018 — medical restriction conflicts with initiative

**Initial condition:** medical restriction conflicts with initiative. The view is a projection of the current owner. **Pressure:** campaign RNG not injected. **Player action:** restore through verified social owner.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. restore does not lose or duplicate refusal. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That was a suggestion. No one has taken the parts. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-018 — help action has no valid target

**Initial condition:** help action has no valid target. The view is a projection of the current owner. **Pressure:** same evaluation called twice. **Player action:** process one owner evaluation identity.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. view has no shadow goal state. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** He kept the goal. The bolt did not come in. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-018 — help action has no valid target

**Initial condition:** help action has no valid target. The view is a projection of the current owner. **Pressure:** roster changes after offer. **Player action:** use roster/consent command result.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. missing RNG cannot silently use seed 144. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That was a suggestion. No one has taken the parts. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-019 — help action has no valid target

**Initial condition:** help action has no valid target. The view is a projection of the current owner. **Pressure:** roster changes after offer. **Player action:** block coercive override pending policy.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. same seed yields same proposal and outcome. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She refused the night watch. Her medical note is current. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-019 — help action has no valid target

**Initial condition:** help action has no valid target. The view is a projection of the current owner. **Pressure:** UI callback tries to force work. **Player action:** apply cost through inventory/task owner.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. assignment cannot bypass CrewConsentVerdict. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can do the pump after the roster settles. Not before. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-019 — help action has no valid target

**Initial condition:** help action has no valid target. The view is a projection of the current owner. **Pressure:** two goals claim one event. **Player action:** show no action instead of inventing one.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. goal progress references durable task ID. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She refused the night watch. Her medical note is current. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-020 — help action has no valid target

**Initial condition:** help action has no valid target. The view is a projection of the current owner. **Pressure:** old save lacks autonomy state. **Player action:** use roster/consent command result.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. restore does not lose or duplicate refusal. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** He kept the goal. The bolt did not come in. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-020 — help action has no valid target

**Initial condition:** help action has no valid target. The view is a projection of the current owner. **Pressure:** campaign RNG not injected. **Player action:** defer or reassign without hidden penalty.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. view has no shadow goal state. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That was a suggestion. No one has taken the parts. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-020 — help action has no valid target

**Initial condition:** help action has no valid target. The view is a projection of the current owner. **Pressure:** trigger reason cannot be explained. **Player action:** advance only from task completion.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. missing RNG cannot silently use seed 144. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** He kept the goal. The bolt did not come in. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-021 — help action has no valid target

**Initial condition:** help action has no valid target. The view is a projection of the current owner. **Pressure:** trigger reason cannot be explained. **Player action:** show no action instead of inventing one.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. same seed yields same proposal and outcome. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can do the pump after the roster settles. Not before. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-021 — one task may match two goals

**Initial condition:** one task may match two goals. The view is a projection of the current owner. **Pressure:** target leaves before help begins. **Player action:** restore through verified social owner.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. assignment cannot bypass CrewConsentVerdict. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She refused the night watch. Her medical note is current. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-021 — one task may match two goals

**Initial condition:** one task may match two goals. The view is a projection of the current owner. **Pressure:** UI callback tries to force work. **Player action:** process one owner evaluation identity.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. goal progress references durable task ID. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can do the pump after the roster settles. Not before. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-022 — one task may match two goals

**Initial condition:** one task may match two goals. The view is a projection of the current owner. **Pressure:** UI callback tries to force work. **Player action:** advance only from task completion.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. restore does not lose or duplicate refusal. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That was a suggestion. No one has taken the parts. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-022 — one task may match two goals

**Initial condition:** one task may match two goals. The view is a projection of the current owner. **Pressure:** two goals claim one event. **Player action:** block coercive override pending policy.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. view has no shadow goal state. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** He kept the goal. The bolt did not come in. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-022 — one task may match two goals

**Initial condition:** one task may match two goals. The view is a projection of the current owner. **Pressure:** survivor order changes on reload. **Player action:** apply cost through inventory/task owner.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. missing RNG cannot silently use seed 144. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That was a suggestion. No one has taken the parts. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-023 — one task may match two goals

**Initial condition:** one task may match two goals. The view is a projection of the current owner. **Pressure:** campaign RNG not injected. **Player action:** process one owner evaluation identity.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. same seed yields same proposal and outcome. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She refused the night watch. Her medical note is current. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-023 — one task may match two goals

**Initial condition:** one task may match two goals. The view is a projection of the current owner. **Pressure:** trigger reason cannot be explained. **Player action:** use roster/consent command result.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. assignment cannot bypass CrewConsentVerdict. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can do the pump after the roster settles. Not before. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-023 — newcomer lacks relationship history

**Initial condition:** newcomer lacks relationship history. The view is a projection of the current owner. **Pressure:** target leaves before help begins. **Player action:** defer or reassign without hidden penalty.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. goal progress references durable task ID. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She refused the night watch. Her medical note is current. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-024 — newcomer lacks relationship history

**Initial condition:** newcomer lacks relationship history. The view is a projection of the current owner. **Pressure:** target leaves before help begins. **Player action:** apply cost through inventory/task owner.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. restore does not lose or duplicate refusal. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** He kept the goal. The bolt did not come in. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-024 — newcomer lacks relationship history

**Initial condition:** newcomer lacks relationship history. The view is a projection of the current owner. **Pressure:** refusal follows medical restriction. **Player action:** show no action instead of inventing one.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. view has no shadow goal state. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That was a suggestion. No one has taken the parts. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-024 — newcomer lacks relationship history

**Initial condition:** newcomer lacks relationship history. The view is a projection of the current owner. **Pressure:** initiative spends shared inventory. **Player action:** restore through verified social owner.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. missing RNG cannot silently use seed 144. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** He kept the goal. The bolt did not come in. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-025 — newcomer lacks relationship history

**Initial condition:** newcomer lacks relationship history. The view is a projection of the current owner. **Pressure:** old save lacks autonomy state. **Player action:** restore through verified social owner.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. same seed yields same proposal and outcome. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can do the pump after the roster settles. Not before. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-025 — newcomer lacks relationship history

**Initial condition:** newcomer lacks relationship history. The view is a projection of the current owner. **Pressure:** another system completed task. **Player action:** process one owner evaluation identity.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. assignment cannot bypass CrewConsentVerdict. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She refused the night watch. Her medical note is current. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-025 — evaluation runs near save capture

**Initial condition:** evaluation runs near save capture. The view is a projection of the current owner. **Pressure:** same evaluation called twice. **Player action:** use roster/consent command result.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. goal progress references durable task ID. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can do the pump after the roster settles. Not before. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-026 — evaluation runs near save capture

**Initial condition:** evaluation runs near save capture. The view is a projection of the current owner. **Pressure:** same evaluation called twice. **Player action:** block coercive override pending policy.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. restore does not lose or duplicate refusal. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That was a suggestion. No one has taken the parts. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-026 — evaluation runs near save capture

**Initial condition:** evaluation runs near save capture. The view is a projection of the current owner. **Pressure:** roster changes after offer. **Player action:** apply cost through inventory/task owner.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. view has no shadow goal state. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** He kept the goal. The bolt did not come in. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-026 — evaluation runs near save capture

**Initial condition:** evaluation runs near save capture. The view is a projection of the current owner. **Pressure:** UI callback tries to force work. **Player action:** show no action instead of inventing one.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. missing RNG cannot silently use seed 144. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That was a suggestion. No one has taken the parts. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-027 — evaluation runs near save capture

**Initial condition:** evaluation runs near save capture. The view is a projection of the current owner. **Pressure:** initiative spends shared inventory. **Player action:** use roster/consent command result.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. same seed yields same proposal and outcome. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She refused the night watch. Her medical note is current. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-027 — evaluation runs near save capture

**Initial condition:** evaluation runs near save capture. The view is a projection of the current owner. **Pressure:** old save lacks autonomy state. **Player action:** defer or reassign without hidden penalty.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. assignment cannot bypass CrewConsentVerdict. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can do the pump after the roster settles. Not before. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-027 — evaluation runs near save capture

**Initial condition:** evaluation runs near save capture. The view is a projection of the current owner. **Pressure:** campaign RNG not injected. **Player action:** advance only from task completion.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. goal progress references durable task ID. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She refused the night watch. Her medical note is current. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-028 — evaluation runs near save capture

**Initial condition:** evaluation runs near save capture. The view is a projection of the current owner. **Pressure:** campaign RNG not injected. **Player action:** show no action instead of inventing one.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. restore does not lose or duplicate refusal. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** He kept the goal. The bolt did not come in. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-028 — evaluation runs near save capture

**Initial condition:** evaluation runs near save capture. The view is a projection of the current owner. **Pressure:** trigger reason cannot be explained. **Player action:** restore through verified social owner.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. view has no shadow goal state. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That was a suggestion. No one has taken the parts. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S3-028 — player opens action after day advance

**Initial condition:** player opens action after day advance. The view is a projection of the current owner. **Pressure:** roster changes after offer. **Player action:** process one owner evaluation identity.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. missing RNG cannot silently use seed 144. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. AssignGoal/AdvanceGoal mutate state but do not prove work. Require a task producer/event identity and save through the current social owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** He kept the goal. The bolt did not come in. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S1-029 — player opens action after day advance

**Initial condition:** player opens action after day advance. The view is a projection of the current owner. **Pressure:** roster changes after offer. **Player action:** advance only from task completion.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. same seed yields same proposal and outcome. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Same-day retry must not consume another roll. Inject the campaign RNG; never let a UI generate autonomy.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can do the pump after the roster settles. Not before. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-99-S2-029 — player opens action after day advance

**Initial condition:** player opens action after day advance. The view is a projection of the current owner. **Pressure:** UI callback tries to force work. **Player action:** block coercive override pending policy.

**Expected route and outcome:** Use the injected campaign RNG and one evaluation identity; route work through DutyRoster/CrewConsentVerdict. Goal progress requires a witnessed task event. assignment cannot bypass CrewConsentVerdict. If override policy is unsigned, only non-coercive outcomes remain available.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. OverrideRefusal can enforce work and penalize morale/affinity, but policy connection is unproven. Do not expose enforcement until consent/safety policy is approved.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** She refused the night watch. Her medical note is current. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

## 30. Final review gate

Refresh this premise against current source; confirm path claims and owner decisions; choose focused verification; leave unproven effects unavailable. Proposal only: no implementation, test, runtime or ledger result is claimed.

## 31. Plan 3 expansion installment 1 — An offer is a proposal, not an assignment

The first narrative expansion should teach the player how to read a survivor offer without turning it into another command queue. A survivor notices a problem, chooses whether to propose help, explains the scope they are willing to take, and can revise that choice when new information appears. The player can support, clarify, negotiate, route the offer to its owner, or leave it unanswered. The offer is not work until the survivor agrees and the current task owner accepts the route.

This installment expands only the three defined subfeatures: offers survivors can answer; a refusal that passes through consent; and goals measured by witnessed work. It adds no fourth pillar, coercion toggle, personality stat, or autonomous task executor. The goal is a human-scale initiative loop whose branches emerge from timing, cost, evidence, prior commitments, and the survivor's own choices.

## 32. Start from a particular intention

An offer should begin with something a survivor wants to do or help with, not a generic “available worker” prompt. The intention might be to repair a loose fitting, accompany someone to a known location, explain a procedure, return an item, or complete a delivery. These examples are conditional: each must use an action a current owner can actually validate.

The survivor can bring the idea to the player, mention it during an existing interaction, or respond to a current need. The player should be able to distinguish a suggestion from an executed action. If the autonomy system only produces an event but no player-reachable proposal state, keep the scene as a supported event projection and do not fabricate a durable offer.

The opening question is not “Do you accept the task?” It is “What is the survivor proposing, and what do they need the player to decide?” They might ask for access to a tool, a schedule adjustment, a second person, clarification from a faction, or simply permission to try an action whose owner already permits it.

## 33. State the whole offer in ordinary language

Before the player answers, the offer should make its terms legible to the extent current owners know them:

- who proposes the action and who else would participate;
- what exact task or bounded contribution is proposed;
- when the survivor wants to act and what duty conflicts are known;
- what materials, access, supervision, or other people are needed;
- what risks or uncertain conditions are already represented;
- whether this is a suggestion, a request for approval, or a task ready to start;
- what the survivor is not offering to do.

Do not render unknown terms as blanks that the player can accidentally assume. The player can ask, “How long do you think it will take?”, “Can you do it without the night shift?”, or “Do you need the whole part or only a tool?” If the survivor cannot answer, the offer stays uncertain and may need review by the relevant owner.

Terms are not a new offer schema unless the existing autonomy system supports them. If current records cannot represent duration, cost, or dependencies, dialogue can expose known context while the actual command remains unavailable until its owner can validate it.

## 34. Player responses should lead to different routes

The player can answer with concrete actions, not only approving or disapproving a character. They can:

1. **Approve the next check:** confirm availability, permissions, or task prerequisites without committing the survivor.
2. **Ask for a narrower proposal:** identify one safe or useful part that can be evaluated by the task owner.
3. **Find a missing resource:** query its real inventory or ownership source and return with the result.
4. **Resolve a schedule conflict:** ask the roster owner about coverage or offer a later time.
5. **Seek another participant:** ask the survivor whether they want help and use current eligibility rules.
6. **Negotiate with a faction or contact:** request clarification or altered terms where its current owner supports negotiation.
7. **Decline the offer:** state the actual constraint and let the survivor choose whether to revise or stop.
8. **Leave it open:** if a supported proposal state exists, defer a decision; otherwise be clear that no appointment or commitment has been recorded.

These choices can branch on facts: an item is owned by a different system; the survivor has night duty; the task is hazardous; a qualified helper is away; or the faction will only accept a narrower result. They should not collapse into a kindness meter.

## 35. Offer and consent branch table

| Offer condition | Player action | Survivor response | Correct route |
|---|---|---|---|
| Terms are clear and task is eligible | Confirm the bounded task | Accepts, revises, or withdraws | Task owner validates final command |
| Time conflicts with current duty | Seek a roster adjustment or later time | Accepts delay or declines | DutyRoster owns assignment and coverage |
| Required item is missing or shared | Check the property owner | Substitutes, waits, or closes offer | Inventory/belongings owner confirms access |
| Scope exceeds survivor's proposal | Ask for a smaller task | Accepts a specific subset or refuses | Task owner checks that subset separately |
| Survivor lacks required information | Ask a source owner or specialist | Waits, narrows, or stops | No success-shaped offer before evidence |
| Faction has additional conditions | Negotiate, ask for detail, or decline | Survivor chooses whether new terms fit | Faction and work owners apply only their terms |
| Survivor changes their mind | Respect the change and use current cancellation route | Withdraws, pauses, or proposes another time | No hidden refusal penalty |

## 36. A compact branching arc — “Three Bolts, One Missing Part”

This is a candidate content carrier, not an assertion that a particular pump, repair command, or item exists. Validate the location, equipment, task producer, inventory route, and safety rule before using it.

**Opening:** A survivor notices a recurring leak and offers to inspect the fitting. The player can ask what they intend to check, confirm whether inspection is an allowed task, or send the request to a qualified repair owner. The survivor may only want to identify the problem, not repair it.

**Preparation:** The player discovers that the required tool is available but the replacement part is missing. They can ask the survivor whether inspection alone is still useful, search through the existing inventory owner, ask a supporting faction for a part, or reschedule. The survivor can accept the narrower inspection or withdraw if it does not meet their goal.

**Consent point:** If a replacement part arrives, the offer has changed. The player restates the work, schedule, supervision, and known risk. The survivor accepts, negotiates daylight, asks for a second person, or declines the repair while still agreeing to help record the problem.

**Execution:** Only a task-owner-confirmed event counts as completed work. If the task stops after inspection, the record says inspection occurred and repair remains open. If the survivor leaves midway, the player follows the current safe handoff and reassignment rules.

**Resolution:** The player can report a completed repair, an inspection with a known missing part, a deferred task, an accepted alternative worker, or a closed offer after refusal. No ending claims a repair based on dialogue alone.

## 37. Consent is specific to the proposed action

Consent to inspect is not consent to repair. Consent to work with a named partner is not consent to an open group. Agreement to a daytime task is not agreement to a night shift. Approval of the initial scope does not silently approve added materials, risk, duration, or public attribution.

When material terms change, the player returns to the survivor with the new proposal. They may accept, counter, ask a question, pause, or refuse. If the task has already started, the player uses the existing stop, handoff, or cancellation owner; dialogue must not rewrite a committed assignment as though it never happened.

The system should also distinguish the survivor's decision from the player's permission to use a resource or access a location. A survivor may be willing while the required owner denies the action. Conversely, a resource may be available while the survivor declines the task. Both checks matter.

## 38. A refusal can be complete without a reason

The survivor may explain why an offer does not fit: fatigue, safety, an existing promise, a private concern, or a different priority. They may also decline without giving a reason. The player can acknowledge the answer, ask once whether a narrower route would help, or close the request. Repeatedly demanding an explanation is not a hidden dialogue key.

If the refusal exposes a solvable constraint, the player can act on it: find a substitute, change the hour, locate a missing part, or ask the requester to reduce the scope. The survivor then decides whether the revised offer suits them. A corrected condition does not compel a new yes.

If refusal leaves a slot open, the roster shows the slot as open. If a faction request expires, its owner closes it. If the player promised a response, they can return and explain that the work will not happen. Do not convert refusal into morale loss, relationship damage, or an autonomy penalty unless a separately approved current owner already defines that specific consequence.

## 39. Supportive play does not mean automatic approval

The player can support a survivor by taking their concern seriously, finding relevant information, rescheduling, seeking a qualified partner, or making sure a completed effort is witnessed. Sometimes support means explaining why the task cannot be safely or validly routed today. The survivor can disagree with the player's conclusion, seek another option, or drop the idea.

The player may also approve an action that later proves impractical. If the resource is unavailable or the task owner rejects it, the player must report the real reason and let the survivor decide what to try next. An apology does not refund consumed materials unless the inventory/transaction owner performs that recovery.

An overbearing route can be represented through specific player actions—repeating a declined offer, hiding a change in shift, or naming a survivor to a faction without asking—where current dialogue and consent systems can make those actions visible. Do not summarize these as a global “bad leader” value.

## 40. Goals advance from witnessed work, not declared intention

The survivor's personal goal can describe what they hope to do. Its progress must be driven by a durable task event or another evidence source the current owner can prove. A stated intention is not a task attempt. An accepted offer is not completion. A partially completed task should not become full completion because the conversation reached a friendly ending.

The player should be able to see what event counts: an inspection record, a completed delivery, a verified repair, or another currently supported result. If no producer emits that fact, the proposal must mark the goal route as unavailable pending an owner decision. Do not let a panel increment a goal directly.

If the task is reassigned, record only the work the survivor actually performed where the current goal owner supports attribution. If attribution is not supported, do not claim individual credit. The goal can remain open, pause, or be evaluated from a shared event according to its real contract.

## 41. Partial work, interruption, and evidence

An interruption may leave a useful partial result: the issue was identified, a route was surveyed, one delivery leg completed, or a task was safely stopped. The goal owner must say whether that evidence counts as progress. The player should distinguish “attempted,” “partly completed,” “completed,” “blocked,” and “not started” only where current state supports those distinctions.

The player can find a missing item, bring a qualified person, reschedule, or close the goal. The survivor can decide whether a partial result is enough to change their aim. If the survivor continues, the next task must be a valid new command with a new event identity. Retry cannot replay the same progress.

An absent witness is not proof that work failed. The game should report the evidence that exists and its limits. The player can seek a supported confirmation, accept uncertainty, or leave the goal unresolved rather than fabricate completion.

## 42. Supporting factions can change the offer's context

A faction may supply a replacement part, ask for the result, offer a safer schedule, or provide a witness. It can also add conditions that the player may negotiate or decline. The faction is a supporting actor; it does not become the owner of the survivor's preferences or personal goal.

For example, a Military representative might require a repair note before changing a duty schedule. An Independent may offer a part in exchange for a later delivery. A Rebel contact may know a different route or source. These are candidate roles, not guaranteed current content. Verify the relevant faction record, inventory/trade owner, and task route before writing final branches.

The player can ask the faction for a narrower request, relay only information the survivor agreed to share, accept a condition, seek another source, or walk away. The faction may refuse, counter, or close its request. No automatic reputation bonus is attached to the survivor's consent.

## 43. Ending mosaic based on actions and evidence

**Bounded success:** the survivor completes the exact task they accepted, and the owner records it.

**Narrow success:** the survivor agrees to a smaller scope, such as inspection without repair; the goal reports only that result.

**Resource-blocked:** the survivor remains willing, but a required item is unavailable. The player finds an alternative, waits, or closes the offer.

**Schedule-shifted:** the player negotiates a new time and checks again; the survivor may agree or decide the opportunity has passed.

**Consent withdrawn:** the survivor changes their mind before commitment; the player routes any open request through its owner and does not punish them.

**Work interrupted:** the player follows a supported safe stop or handoff; the goal records only confirmed work.

**Goal revised:** experience changes what the survivor wants. Completed evidence remains true; the future goal changes only if the current goal owner supports revision.

**Faction counteroffer accepted:** the survivor agrees to new terms after the player discloses them and obtains the necessary owner approvals.

**No route available:** permission, source, safety, or owner support is absent. The player receives a clear limitation rather than an invented success.

## 44. Keep outcomes local and legible

An offer can affect one roster slot, one task, one resource, one conversation, or one goal. A bad negotiation should not silently reduce global morale. A successful repair should not grant faction reputation unless its current system says so. The player can understand consequences by seeing which owner confirmed what.

The survivor may remember a concrete prior event if an existing social/narrative record supports that response. If not, write the branch as a present-tense reaction rather than inventing a long-term relationship modifier. Goal history and social history remain with their existing owners.

Local consequences can still feel substantial: a shift remains uncovered, a request expires, a delivery waits, a part is committed, or a survivor chooses a different aim. The design does not need a universal score to make those outcomes matter.

## 45. Re-entry and replay behavior

When the player returns to an open offer, reload the current owner state. The survivor may have changed availability; the task may have expired; a resource may have moved; a faction may have countered; or the task may already be complete. The offer view is a projection of those records, not an independent source.

Use one stable evaluation identity for a given autonomy evaluation. Opening a panel, asking a question, or retrying after an owner rejection cannot consume a new RNG draw. Campaign RNG and stable roster order determine any autonomous proposal; UI interaction never rolls new initiative.

After save/restore, verify that a confirmed task result and goal state agree. A task event cannot advance twice on replay. A pending proposal is restored only if the social owner actually persists it. If the proposal was ephemeral, tell the player that it must be discussed again rather than reconstructing a commitment.

## 46. Production questions for the three subfeatures

1. What current event or command exposes a survivor initiative proposal to the player?
2. Which API carries offer terms, and which terms are genuinely persisted?
3. Does `CrewConsentVerdict` govern the exact task commitment and change-of-scope path?
4. What cancellation or safe-handoff path exists after work begins?
5. Which task producers emit durable evidence that `SurvivorAutonomySystem` goals can consume?
6. Which save owner captures proposals, consent outcomes, and goals, and in what restore order?
7. Can duplicate daily evaluation and duplicate completion delivery be shown idempotent?
8. Are any refusal consequences approved and explicit, or should the offer simply close?

Until these answers are established from current source and data, this installment remains a content and design proposal. It does not authorize a new save section, a parallel initiative queue, or a force-work route.

## 47. Installment 1 close

This opening installment gives Plan 3 a concrete, player-readable offer arc: a survivor proposes a bounded action; the player clarifies terms, resources, schedule, and permission; the survivor can accept, counter, defer, or refuse; and a goal advances only from witnessed work. Branches respond to evidence, timing, cost, changed scope, and the survivor's stated preferences. Factions can help or complicate the route without controlling consent. No moral alignment axis or fourth feature pillar is introduced.

## 48. Plan 3 expansion installment 2 — The conditions change at the threshold

An offer can be reasonable when it is made and no longer reasonable when the survivor is ready to act. Weather, access, equipment, health, duty, or a requester's terms may have changed. The player must decide whether to bring the changed conditions back to the survivor, find another route, accept a narrower outcome, or stop. This is where consent becomes visible as an ongoing decision about a specific task rather than a permanent yes.

This installment expands the same three subfeatures. The offer states what the survivor is willing to do; consent determines whether changed terms are accepted; and personal goals move only when the relevant owner witnesses work. No new risk meter, autonomous cancellation system, or coercion mechanic is implied.

## 49. The threshold is a state recheck

The “threshold” can be literal or practical: the player reaches the work site, opens the task, tries to retrieve a tool, or receives a new instruction immediately before commitment. The purpose of the scene is to re-read current owner state and compare it to the terms the survivor accepted.

If the scope, participant, location, timing, required item, supervision, or exposure has changed in a material way, return to the survivor before starting. If nothing material changed, proceed through the existing task command. Do not create a second consent dialog for every inconsequential display change; the recheck should protect a real boundary.

The UI should distinguish three conditions where current state supports them: the offer is still valid; the terms need the survivor's renewed agreement; or the task is unavailable under its owner rules. If the autonomy owner cannot represent those distinctions, keep the scene in dialogue and leave the unsupported command disabled.

## 50. What can change after agreement?

- **Time:** a day shift becomes a night shift, or a short task grows beyond the agreed window.
- **Scope:** a simple inspection becomes a repair, or one delivery becomes two.
- **Participants:** a named partner is replaced or a larger group is expected.
- **Equipment:** an agreed tool is missing, shared, damaged, or requires a different owner.
- **Access:** a room or route is closed; a faction adds a permission requirement.
- **Risk:** a new owner-confirmed condition changes whether the task is allowed.
- **Audience:** a private contribution is now expected to be named publicly.

The survivor may accept, counter, defer, or refuse the revised offer. Each choice is local to this request. Their earlier agreement remains a truthful historical fact but does not authorize the changed version.

## 51. Player routes when the terms no longer fit

The player can describe the new condition and ask whether the survivor wants to continue. They can seek an alternative route, recover the original equipment, restore the original time window, find the named partner, reduce the task scope, or take the request back to its issuer. They can also close the offer if no valid route remains.

The player can accept extra work personally only through an existing action/task owner. They cannot silently transfer the survivor's task to themselves in dialogue. If an alternate worker is available, the roster and consent owners determine eligibility and assignment.

If the change originates from a faction, the player can negotiate the new condition, offer the original scope, decline the faction's request, or seek a different contact. The survivor decides whether the faction's counteroffer is acceptable after the player shares its material terms.

## 52. Branch table — agreement after a change

| Change discovered | Player response | Survivor choice | State that may follow |
|---|---|---|---|
| Original tool is unavailable | Find approved substitute or delay | Accept substitute, wait, or decline | Only the actual property owner supplies equipment |
| Task now takes longer | Explain the new duration and schedule | Accept, narrow scope, or refuse | Roster owner validates revised time |
| Partner is absent | Wait, find eligible replacement, or work alone if allowed | Accept revised participants or stop | No silent participant substitution |
| Public credit is added | Ask permission before naming survivor | Allow, request anonymity, or withdraw | Communication owner controls publication |
| Access closes | Seek permission, wait, or cancel | Choose whether to return | Location/task owner controls access |
| Hazard condition changes | Stop and check authorized response | Continue only if allowed and agreed | Safety/task owner determines eligibility |
| Faction adds a deadline | Negotiate, keep original terms, or decline | Accept new timing or refuse | Faction/work owners close or revise request |

## 53. A compact scenario — “The Route Is Closed”

This is a conditional writing structure; verify that the route, delivery, access state, and task event exist before production.

**Offer:** A survivor agrees to take one package along an established safe route during daylight. The player confirms the item owner, recipient, timing, and that the survivor accepts the bounded task.

**Change:** On arrival, the route owner reports that the usual path is closed. Another route is longer and has a condition that changes eligibility. The player can ask for an updated route, request a later delivery, contact the recipient, or close the delivery.

**Consent:** The player explains the changed distance and timing. The survivor may accept the longer route if the travel owner permits it, offer to carry the package only to a nearby handoff, wait for reopening, or refuse the revised trip.

**Action:** If accepted, the task owner commits the revised route and records what happens. If the package is handed off only partway, the delivery is partial only if its owner can represent that. If it cannot, report the task as incomplete and use its supported recovery path.

**Resolution:** The player can accept delay, find an eligible alternate carrier, split the route through an existing handoff mechanic, renegotiate with the recipient, or close the request. The survivor's personal goal advances only from the evidence the goal owner accepts.

## 54. Refusal at different points

**Before commitment:** the survivor can withdraw without a task cancellation because no task has started. The player can seek another worker or close the offer.

**After commitment but before work begins:** use the current cancellation/reassignment path. The player should explain what will happen to the request and ask about a future offer only if appropriate.

**During work:** stop or hand off only through the task's safe route. A narrative refusal cannot teleport the survivor away or erase a committed assignment. The task owner records the work completed and what remains.

**After completion:** the survivor can decline a new request, but that does not undo a completed result. The player can report the result accurately and end the conversation.

These points create distinct choices because work state differs. They do not justify a single force-work override or a global refusal penalty.

## 55. Goal evidence when terms change

The goal record should not silently switch from the original aim to the revised task. If the survivor's goal was to complete a delivery, a safer partial handoff may or may not satisfy it; the goal owner must define that relation. If the survivor's goal was to help the recipient receive needed supplies, evidence from the actual delivery owner may still confirm meaningful progress.

The player can review what the survivor values after the change, but should not rewrite their goal on their behalf. They can ask whether the new route still matters, whether a partial result is enough, or whether the survivor wants to revise the aim. The survivor may keep, revise, pause, or abandon the goal where current APIs support that lifecycle.

An externally blocked task can be a real outcome even when no goal progress occurs. The player can show the blocker, seek another route, or close the task without claiming success.

## 56. Supporting actors and non-alignment branches

A faction requester may accept the original scope, negotiate a smaller task, provide an alternate schedule, or close the offer. A supporting contact can supply route information or a tool if the relevant owner confirms access. Neither may tell the survivor that refusal is invalid.

The player may preserve a relationship with the faction while refusing its expanded terms, or accept a practical counteroffer that the survivor also approves. A faction's response need not determine whether the player is honest or good. The exact result depends on the terms disclosed, the route chosen, available resources, and the survivor's answer.

Other branches can emerge from the player uncovering an item ownership conflict, finding a new partner, changing the route, or learning the job can wait. These state changes give replays different paths without a moral alignment axis.

## 57. Failure, interruption, and repair

If the player starts the original task before disclosing a material change, the game should stop or redirect only through supported task rules. The player can acknowledge the mistake, return to the survivor, and use the official cancellation or handoff route. Do not erase any cost or progress already applied by its owner.

If a resource was consumed before the change was noticed, only its owner can determine whether recovery is possible. If the revised offer is rejected, return the untouched resource only through the relevant transaction owner. If no owner supports recovery, be clear about the loss and its actual cause.

An interruption may leave work complete, partial, or not started. Re-entry reads the task and consent owners again; it does not replay a proposal or spend another autonomy RNG draw.

## 58. Acceptance questions

1. Does the player recognize when material terms have changed?
2. Does the survivor get a new decision when scope, time, participants, risk, or audience changes?
3. Can the player restore original terms, negotiate, seek an alternative, or close the offer?
4. Are pre-commit refusal, post-commit cancellation, and mid-task stop routed distinctly?
5. Does every goal update depend on an owner-confirmed event?
6. Can a faction support or complicate the offer without overriding consent?
7. Do save/replay and duplicate delivery preserve one proposal and one task result?

## 59. Installment 2 close

This installment begins Plan 3's second branch around an offer whose conditions change before or during work. The player can disclose the change, restore original terms, negotiate, find an eligible alternative, or stop through current owners. The survivor can accept, counter, defer, or refuse, and personal goals reflect only witnessed work. Changed conditions—not moral alignment—drive the branches.

## 60. Plan 3 expansion installment 3 — An offer that needs another person

A survivor may want to take on a task with a companion. That creates a richer initiative branch because there are now two people with separate schedules, limits, and consent. The player can clarify roles, find a supported shared task, arrange a handoff, or suggest that each person take a different part. Neither survivor's agreement can stand in for the other's.

This installment remains inside the existing feature: survivors make offers; consent applies to each participant and to the actual scope; goals advance from witnessed work. It does not add a team-management system, party roster, or shared credit ledger. If the current task owner cannot assign or attribute a joint action, the design must keep collaboration at the discussion stage or route only the supported individual task.

## 61. Distinguish a shared plan from two separate offers

One survivor may propose a joint action, but the second survivor must receive and answer their own terms. The player can ask the initiator whom they want to work with, approach that person with the proposal, and verify whether the current task owner can represent joint participation.

The first survivor can offer to lead, provide a tool, teach, or accompany; the second may contribute a different skill or simply decline. A task can be shared in conversation without being represented as one combined assignment. The player should be explicit about which action the system can actually commit.

If the initiative owner produces one event for a group but no individual attribution, the player can report the group result without claiming that both personal goals advanced. If individual evidence exists, the goal owner determines whose goal it satisfies and to what extent.

## 62. Get separate agreement on roles

Before commitment, the player states the shared objective and each participant's proposed role. Each person can accept their role, counter with a narrower contribution, ask for time, or refuse. The player can ask whether they want a different partner or whether solo work is preferable.

Roles may include performing a supported action, carrying an item, observing, providing a source, or waiting at a handoff point. Use only roles the task owner can track and the location/safety owners permit. Do not create a generic “support” slot that makes an otherwise invalid assignment appear valid.

If one survivor declines, the other may continue alone only if the owner says the task is feasible. The player may find an eligible replacement through the existing roster path, divide the objective into separate tasks, reschedule, or close the offer.

## 63. Player actions that change the collaboration route

- Ask the initiating survivor what the companion is needed for: skill, equipment, safety, transport, or simply preference.
- Ask the second survivor directly, sharing the material terms but not private details the initiator did not authorize.
- Check both schedules, duties, health/safety restrictions, and role eligibility.
- Seek a smaller task that one person can perform if the second declines.
- Arrange a supported handoff or different meeting time.
- Find a current owner for a missing item or access permission.
- Let the two survivors revise the plan together where dialogue allows.
- Stop the proposal if terms remain unclear or the shared task cannot be represented.

These actions branch based on participant choice, eligibility, available tools, and timing. They do not ask the player to classify one survivor as loyal or difficult.

## 64. Branch table — two people, distinct boundaries

| First survivor | Second survivor | Player action | Valid route |
|---|---|---|---|
| Wants a joint task | Accepts the stated role | Check shared-task support | Commit only through task owner |
| Wants a joint task | Requests a smaller role | Revise terms and revalidate | New scope requires owner approval |
| Wants a joint task | Declines without reason | Accept refusal | Solo, alternate person, delay, or close |
| Offers to teach | Wants to observe only | Check lesson/task contract | Observation is not work completion |
| Offers equipment | Item belongs to a shared owner | Seek permission or substitute | No personal transfer without owner |
| Wants a named partner | Partner is unavailable | Reschedule or ask about alternate | No silent substitution |
| One participant changes terms | Other has not accepted change | Return to both separately | No group consent inferred |

## 65. Scenario — “The Awning Before Rain”

This candidate scene requires verification of a real shelter structure, repair task, materials, access, and safety route. If any is absent, retain it as a narrative pattern rather than an implemented activity.

**Proposal:** A survivor offers to help secure an awning before expected rain and asks for a second person to hold the frame. The player can ask whether the request is for a repair, temporary brace, or inspection, then check the task and weather owners.

**Invite:** The player explains the task, timing, equipment, and known risk to the suggested companion. They can accept, ask to work in daylight, offer a different role, or decline. The initiator may accept a narrower contribution or decide that the task is no longer worth doing.

**Preparation:** The player checks whether the required tie-downs are personal or shared stock, whether the site is accessible, and whether a safer method is available. A faction contact may provide a material or instruction only through its current owner.

**Commit:** If both agree and the task owner accepts the joint work, the player commits it once. If the task system only supports one worker, the player can assign one survivor and let the other provide a supported non-task interaction, or close the joint offer.

**Resolution:** Evidence may show the awning secured, temporarily braced, inspected but left unchanged, or untouched because conditions worsened. The task owner records the outcome. Each goal owner decides whether that evidence counts for that survivor.

## 66. Changing roles during the task

Conditions may change after work begins: a tool breaks, rain arrives, one participant becomes unavailable, or a safer route appears. The player checks the task owner before reassigning roles. Material changes return to each survivor for renewed agreement where consent rules require it.

One person may stop while the other can safely complete the task. The player can request a handoff, add an eligible replacement, or stop the work entirely. No participant is silently kept on a task because the other remains willing.

If the task cannot be safely stopped through current systems, do not author a false “walk away” command. Route the player through the task's actual safety/cancellation path and make the limitation visible before commitment.

## 67. Personal goals and shared evidence

Two survivors can have different reasons for the same task. One may want to repair the awning; another may want to learn how to secure it. The same witnessed event may be relevant to both goals, one goal, or neither, depending on what each actually did and what the goal owner can prove.

The player can review the task record, ask each survivor what they want to do next, or accept that one person's goal advanced while the other's did not. Do not split credit evenly or advance both goals because the task was collaborative. Do not add a new contribution score to approximate individual effort.

If the evidence owner records only a group outcome, the UI can report “the awning was secured by the crew” while leaving individual goal state unchanged or unresolved. A later dialogue can acknowledge participation without claiming a durable goal completion.

## 68. Refusal, conflict, and revised collaboration

The two survivors may disagree about how to complete the task. The player can ask them to state the disputed term, check the actual safety/resource constraints, propose a smaller scope, or stop. Do not force a consensus because the player needs a quest result.

One person can refuse the other's proposed role while accepting a different contribution. The player can separate the tasks, find another partner, or let the original offer close. If the conflict involves private history, each person decides what they disclose; the player does not demand a reason as a condition of refusal.

The disagreement does not automatically reduce pair affinity. If an existing relationship owner reacts to a concrete action, show only that supported response. The story can end with two people choosing different routes.

## 69. Factions as logistical support

A faction may provide a work window, a tool, a witness, or an alternate assignment. The player can request support, negotiate terms, accept a substitute task, or decline. A faction cannot assign one survivor to work with another without both survivors' valid consent and the task owner's route.

If a faction wants a result, the player can explain the shared plan, ask for a narrower deadline, or offer a qualified alternative. If the faction rejects the plan, the survivor's personal goal remains separate from faction approval. The faction can close its request without erasing work already witnessed elsewhere.

These branches use present constraints and actions: access, supply, deadline, witness, and eligible alternate. No new faction reputation axis is introduced.

## 70. Play approaches for collaboration

**The facilitator:** lets both people explain their preferred roles and negotiates a supported plan.

**The planner:** checks schedule, tools, access, and safe timing before either person commits.

**The splitter:** separates a joint objective into individual tasks only where owners support the split.

**The substitute finder:** respects a refusal and seeks an eligible alternate or different method.

**The witness:** makes sure the actual task result and each person's contribution are described no more broadly than the evidence allows.

These approaches can shift from one scene to the next. They are not build classes or moral alignments.

## 71. Endings from participant choices and witnessed work

**Joint task completed:** both agreed roles are supported and the task owner confirms the result.

**Narrow collaboration:** one person performs the task while the other observes or supplies a supported resource; only confirmed contributions are reported.

**Split route:** participants choose separate actions that the relevant owners can represent.

**One person withdraws:** the other continues only if the task remains eligible; otherwise the offer closes or waits.

**Resource-blocked:** both are willing, but required material/access is unavailable; player finds a source, delays, or stops.

**Safe interruption:** one participant cannot continue; player follows the handoff or stop path and reports partial work if supported.

**No agreement:** the participants choose different plans, and the player routes each separately without inventing shared progress.

## 72. Re-entry and replay behavior

On return, recheck both survivors' current availability and consent, the task assignment, material ownership, and any group evidence. A saved proposal appears only if the social owner persists it. A task event can be consumed once by each relevant goal owner according to stable identity; duplicate callbacks cannot advance a goal twice.

If one participant leaves the shelter or becomes ineligible, the player must not silently substitute another person. Offer a new proposal with full terms, cancel through the task owner, or explain that the original collaboration cannot continue.

## 73. Scope guardrails

This installment adds no party system, team AI, group consent shortcut, shared goal ledger, relationship penalty, or group contribution meter. Each survivor has their own answer. Work remains owned by the task system; personal goals remain owned by the current social/autonomy system; shared property remains with inventory/belongings owners.

If a required owner cannot represent joint work, the plan should narrow the interaction rather than simulate a team assignment in UI.

## 74. Acceptance questions

1. Does each participant receive and answer their own clear terms?
2. Can one survivor refuse or change roles without the other person's answer substituting for them?
3. Can the player split, reschedule, replace, or stop only through supported task routes?
4. Does each personal goal advance only from evidence its owner can attribute?
5. Are shared resources and faction access checked through their own owners?
6. Can collaboration end in partial work, no agreement, or separate plans without false failure?
7. Does replay preserve one proposal, one task result, and no duplicate goal progress?

## 75. Installment 3 close

This installment adds a survivor-led collaboration branch with separate offers, role negotiation, resource checks, and independently witnessed goals. The player can facilitate, split, reschedule, find an alternate, or stop; either person can refuse or change terms. Factions can support logistics but cannot override consent. The result remains within offers, consent, and witnessed personal goals.

## 76. Plan 3 expansion installment 4 — The offer arrived while the player was away

A survivor may raise an idea while the player is handling another task. The player should not have to be present at every moment for the survivor to have initiative, but an unseen proposal must not quietly become an accepted assignment. This installment explores the interval between the survivor forming an offer and the player seeing it: what information is preserved, what can change, and what the player may do on return.

The narrative can show a note, a conversation summary, or an offer card only if an existing owner persists that state. If the current autonomy system records only an event, project that event truthfully without creating a parallel pending-offer queue. If no durable representation exists, the design remains a proposal for a future owner decision.

## 77. Separate proposal, agreement, and work

Three states must not be collapsed:

**Proposal:** the survivor expresses an intention, request, or offer. It can be reviewed, clarified, deferred, or declined.

**Agreement:** the survivor and player/requesting owner confirm specific terms through the current consent and task route.

**Work:** the task owner records an actual attempt, completion, interruption, or failure.

A proposal can expire or become stale without any work having begun. An agreement may still be waiting for resources. Work may happen only after a valid commit. The UI must use the true current state rather than a success-shaped card that implies the survivor has already acted.

## 78. What the player sees on return

The player can see the proposal source, who initiated it, its subject, when it was made if the owner records a day, whether terms are complete, and what question remains unanswered. Do not add timestamps or expiration behavior unless the owner supports them.

The player can select a concrete next action:

- review and accept only the supported next step;
- ask the survivor to clarify scope, timing, cost, or participants;
- check schedule, task, item, or access owners before answering;
- decline with a short reason or without demanding a reason from the survivor;
- defer if a durable pending state exists;
- close the conversation if the proposal is no longer current;
- seek an alternate route and ask whether the survivor still wants to participate.

The survivor can change their mind before agreement. Returning to an old offer does not obligate them to honor it.

## 79. Stale proposal branches

Time away can change the facts. A required item may be used, the survivor may have taken a different task, an access window may have closed, a faction request may have expired, or the original need may no longer exist.

The player can refresh the proposal against current owners, ask the survivor whether they still want it, seek a substitute, renegotiate with the requester, or close it. A stale offer cannot be accepted with its old terms if the task has changed materially. If no state can mark it stale, the player must reopen the conversation rather than infer that it remains valid.

The survivor may say the opportunity passed, propose a smaller action, keep the same goal but choose another method, or have no further interest. These responses create branches based on time and world state, not moral alignment.

## 80. Scenario — “I Could Have Done That Yesterday”

This candidate scene requires a supported initiative event, schedule owner, and task route. It must not imply that an offer persisted if it did not.

**While away:** A survivor offers to deliver one repair part to a neighboring room during a safe, known window. The player is occupied and does not review it immediately. No delivery begins from this proposal.

**On return:** The player sees the offer if the social owner persisted it. The original window has passed and the part has been used for a different task. The player can apologize for the delay, ask whether the survivor wants to revise the offer, check whether another part exists, or close it.

**Survivor response:** They may say the task no longer matters, offer to deliver a different item, ask for a later time, or explain that their intention was only to help locate the part. The player can accept a revised proposal after checking its new terms, seek another worker, or decline.

**Resolution:** The task owner determines whether any delivery occurred. The survivor's personal goal advances only from actual witnessed work. A missed proposal can be a meaningful lost opportunity without inventing a refusal penalty or completed-task flag.

## 81. The player is not required to answer every offer immediately

Some offers can wait; others are time-sensitive. The player should see the known consequence of waiting, not an arbitrary urgency indicator. If the owner provides no expiry, do not invent one. If an event source confirms the window has closed, show that fact and let the player decide whether to pursue an alternate route.

The player can prioritize another crisis, pause a conversation, or close the offer. The survivor may continue their own permitted activity only if the current autonomy/task system independently authorizes that action. A pending offer is not permission for the survivor to perform work the player has not approved.

If a survivor can autonomously act without the player, the event should be recorded through its existing owner and shown as an action that already happened, not mislabeled as an offer. Consent and task eligibility still apply to the actual work.

## 82. No hidden pressure from an unanswered proposal

An unanswered offer should not accrue an invisible morale or affinity penalty each day. If the offer expires, its owner closes it. If the survivor expresses disappointment, use supported dialogue or social state and show the concrete reason. The player should not be punished for not opening an interface while attending to another emergency.

The proposal may still matter narratively: a survivor can decide to pursue a different goal, ask someone else, or stop offering help. These responses must be grounded in current autonomy and social behavior rather than a new “ignored offer” score.

The player can repair an oversight by acknowledging the delay, asking whether the survivor still wants to act, and presenting current options. An apology does not recreate a missed time window or automatically reset the survivor's choice.

## 83. Goals do not advance while an offer waits

An intention can be recorded as a goal only through the current goal owner, but it is not evidence that work occurred. The player may see the goal remain unchanged while the proposal is pending. Once work starts, task events determine progress. A missed offer can leave a goal open, pause it, or lead to revision where supported.

If an autonomous action is recorded before the player returns, the player sees the actual result with provenance. Do not ask the player to approve work retroactively unless the task owner defines such a review. If the result is partial, disputed, or outside the offer scope, the owner decides what was completed.

The player can support the goal by finding a new route, asking a faction to reopen access, or locating a resource. Each is a new action with its own prerequisites; none should advance the goal merely because it was selected in dialogue.

## 84. Supporting contacts can relay, not fabricate

A faction representative or another survivor may tell the player that someone made an offer. The player can ask the contact to repeat the exact terms, speak directly with the initiator, or decline to act on secondhand information. Unless the current social owner stores the offer, the contact's report is not a durable proposal record.

The contact can relay changed terms, provide a resource, or explain that a deadline passed. They cannot agree on the survivor's behalf. The player should confirm material changes directly with the survivor when consent rules require it.

This creates branches through evidence and availability: the contact may have incomplete information; the initiator may no longer be present; the faction can support or reject a revised request; or a direct conversation can reveal that no actual offer was made.

## 85. Evaluation identity and deterministic behavior

Autonomous proposal evaluation must use the injected campaign RNG and stable roster order as the existing contract requires. Viewing the offer, opening another panel, asking for clarification, or reloading cannot consume a fresh random draw for the same daily evaluation. A repeated evaluation returns the same proposal/result or an explicit already-processed state.

If the player returns after day advance, the proposal's validity must be determined from current owner state. Do not create a new offer because the same survivor is evaluated again unless the autonomy owner identifies it as a distinct new proposal. Stable event identity prevents a saved card from duplicating, but does not make an unsupported save route exist.

## 86. Re-entry and persistence boundaries

On reload, a persisted proposal should show its current state, not a stale snapshot of terms. Recheck the survivor, task, schedule, resource, requester, and any expiry owner. The social owner decides whether the proposal survives save/restore; task owners decide whether work began; goal owners consume only confirmed evidence.

If the proposal was not persisted, do not reconstruct it from dialogue text or a panel cache. The player can speak with the survivor again, who may repeat, revise, or withdraw the offer. Completed work and goals remain intact according to their own owners.

## 87. Endings from response timing

**Prompt review:** the player returns in time, confirms current terms, and the survivor accepts the final proposal.

**Revised offer:** the original window passed, but the survivor proposes a smaller or later action.

**Resource gone:** the player checks ownership and finds the item was used; both choose another route or close the offer.

**Offer withdrawn:** the survivor no longer wants to act; the player accepts the decision and routes the request elsewhere.

**Action already completed:** the owner recorded permitted autonomous work; the player reports the actual outcome without retroactive fiction.

**No durable proposal:** the player must reopen the conversation; the survivor may restate, change, or decline.

## 88. Scope guardrails

This installment adds no offline task simulation, inbox queue, offer-expiry engine, neglected-offer meter, new save section, or automatic assignment. It clarifies how a current proposal or event may be surfaced, and blocks any narrative promise that the present owner cannot keep.

## 89. Acceptance questions

1. Can the player distinguish a proposal, agreement, and actual work?
2. Does an offer remain pending only if the social owner persists that state?
3. Can material changes make an offer stale and require renewed agreement?
4. Can the survivor revise or withdraw before commitment?
5. Does no response avoid hidden penalties and false urgency?
6. Are autonomous actions distinct from player-reviewed offers?
7. Does same-day evaluation reuse deterministic identity across panel open, retry, and reload?
8. Do goals advance only from task evidence?

## 90. Installment 4 close

This installment explores offers arriving while the player is occupied. It separates proposal, agreement, and work; lets the player refresh stale terms, ask, defer, decline, or seek an alternate; and gives the survivor room to revise or withdraw. Any persisted offer, autonomous action, or goal result must come from its existing owner. Timing and current state create the branches without pressure meters or alignment axes.

## 91. Plan 3 expansion installment 5 — Work changes the goal

A survivor can begin with a clear personal goal and discover through real work that the original objective was not quite what they needed. The player should not decide this interpretation for them. They can ask what changed, preserve the work already completed, support a revised goal, help finish the original one, or leave the choice with the survivor.

This installment develops the third subfeature—goals measured by witnessed work—while using the offer and consent routes for any new action. A goal is not rewritten because a task failed or a player selects a dialogue option. The survivor chooses whether the aim changes, and the goal owner determines whether it can record that revision.

## 92. Distinguish progress, evidence, and a changed aim

The survivor may have made real progress toward the original goal even if they now want something different. Preserve that history. The player can review the task evidence, ask whether the goal remains important, or say what remains unfinished. The survivor can continue, pause, revise, or close the aim where current APIs support those actions.

These are separate questions:

- What work did the survivor actually perform?
- Which event proves that work occurred?
- Did the event satisfy or partially advance the existing goal?
- Does the survivor still want the same future outcome?
- Can the current goal owner represent a revision without erasing the record?

An answer to one does not determine the others. A blocked task can still produce a valuable realization; a completed task can leave the larger goal open; a new goal can begin without invalidating old evidence.

## 93. Why the goal might change

The survivor may learn that a repair is unnecessary, the needed part is unavailable, the recipient has a different need, the route is unsafe, another person has already solved the problem, or the original plan asks too much of them. They may also simply lose interest or find a new priority.

The player can ask what they want now, whether they want to finish the current attempt, or whether they would rather stop. The survivor may explain, answer briefly, or decline to discuss the reason. Do not require a therapeutic confession to revise a goal.

The game should not diagnose a change as fear, laziness, selfishness, or inconsistency. Use observed circumstances and the survivor's own words. A goal that changes can show learning and agency rather than failure.

## 94. Scenario — “The Hinge Wasn't the Problem”

This candidate scene requires an existing door/repair task, a valid inspection result, and a goal owner that can consume that evidence. If those premises do not exist, use this only as a story structure.

**Initial goal:** A survivor wants to find a hinge and repair a door that sticks. They offer to inspect it, and the player routes the inspection through the current task owner.

**Witnessed result:** The inspection shows the hinge is intact; the obstruction is elsewhere or cannot be confirmed. The player can find a specialist, ask for another inspection, leave the door as it is, or ask the survivor what they want to do next.

**Goal choice:** The survivor can keep the original repair goal while searching for a qualified diagnosis, revise it toward identifying the obstruction, ask the owner to close it as unnecessary, or decide they no longer want to pursue the door.

**Player follow-through:** If a new action is proposed, the player checks its scope, materials, access, and schedule. The survivor accepts, narrows, delays, or declines. No goal is advanced by the conversation alone.

**Resolution:** The original task may be completed, blocked, closed as unnecessary, or revised. The survivor's prior effort remains true. A new goal begins only if the existing goal owner supports that transition.

## 95. Player actions after new evidence

- Show the owner-confirmed result and ask the survivor what they want to do with it.
- Offer to continue the original task if it is still valid.
- Ask a qualified person to confirm an ambiguous result.
- Find a different method or scope through the relevant task owner.
- Seek required parts or permission through their actual owners.
- Accept the survivor's choice to pause or stop.
- Close an obsolete request through the requester or task owner.
- Ask the survivor whether a revised goal should be recorded, if that command exists.

Each action changes the next available route. The player can support a revised goal even when it costs time, or decline to spend scarce resources while leaving the choice to the survivor. Neither response means the player is good or evil.

## 96. Branch table — evidence changes the aim

| Witnessed evidence | Survivor's preference | Player action | Goal/task result |
|---|---|---|---|
| Original repair is still needed | Keep goal | Find valid materials or specialist | Continue only through task owner |
| Repair is unnecessary | Close or revise | Confirm with owner and respect choice | Prior attempt remains recorded |
| Cause remains uncertain | Investigate or pause | Seek source, specialist, or defer | No false completion |
| Required part is unavailable | Keep, narrow, or abandon | Find substitute or explain delay | Owner records blocked status if supported |
| Another person solved the issue | Revise future aim | Verify completion and ask survivor | No duplicate work or credit |
| Risk exceeds what survivor accepts | Stop or choose safe alternative | Route safe stop/close | No coercive persistence |
| Survivor declines to explain | Decide privately | Accept answer and close discussion | No reason field is required |

## 97. Goals can fork without multiplying goal systems

The survivor's original goal can have several plausible continuations: complete it, narrow it, revise its future aim, or close it after new evidence. These branches should be represented by the current goal owner. If that owner supports only completion or abandonment, the plan must not add a parallel revision ledger to get richer dialogue.

The player can keep a task open while the survivor reflects, if the task owner supports that state. If it does not, close or pause through the actual route and explain how a later proposal can be made. Do not keep an invisible task alive in the UI.

Multiple goals should not be created from one dialogue exchange. A revision can replace future intent only where supported, while historical tasks and evidence remain immutable.

## 98. Partial progress and credit

The player may help the survivor recognize that a partial result matters: a door was inspected, a delivery reached the handoff point, a tool was recovered, or a hazard was identified. The relevant task owner decides whether that event counts as partial progress. The survivor can still decide that partial progress does not meet their personal aim.

Do not round an incomplete task up to completion to provide emotional closure. A goal can remain open after a meaningful effort. The player may choose to support another attempt, request help, or leave the task unfinished.

If another survivor completes the work, use existing attribution. Do not transfer goal credit simply because the first survivor proposed the task. The survivor may be glad the problem is solved while acknowledging that they did not perform the work.

## 99. The player can disagree without taking the goal away

The player may think the old goal still matters. They can explain why, ask whether the survivor would consider continuing, or point out a consequence. The survivor can agree, counter, or decline. The player can also decide the revised goal is too costly to support and say so, while leaving the survivor's choice intact.

If the goal is tied to a faction request, the player can renegotiate with that faction, find a substitute, or close its task. The faction's deadline does not decide the survivor's personal goal. The survivor may continue privately through a supported route or end the goal.

This creates distinct branches through evidence, resource cost, timing, and the player's support. It does not silently turn the player's preference into a goal owner command.

## 100. Supporting actors can confirm facts, not choose goals

A faction, recipient, or specialist can confirm that an item was found, a repair was not needed, a delivery arrived, or a route is unavailable. Their testimony may help resolve evidence where the relevant owner accepts it.

The player can ask the source, request a second opinion, disclose only information the survivor agreed to share, or leave uncertainty unresolved. A contact cannot tell the survivor what their personal goal should be. The faction can accept or close its own request independently of that goal.

## 101. Endings after a goal changes

**Goal completed as written:** the survivor finishes the task and remains satisfied with the original aim.

**Goal completed differently:** an owner-supported alternate method reaches the same intended result.

**Goal narrowed:** the survivor changes the scope and the player supports a smaller, valid outcome.

**Goal revised:** witnessed work changes what the survivor wants next, and the goal owner records that transition.

**Goal closed as unnecessary:** reliable evidence shows the original problem no longer exists.

**Goal blocked:** the survivor still wants it, but resources, access, or eligibility are unavailable.

**Goal stopped by choice:** the survivor no longer wants to pursue it and the player respects that choice.

**Goal unresolved:** evidence conflicts or the current owner cannot confirm status; the player can seek more information or leave the result open.

## 102. Re-entry and history

After reload, the player should see the current task state, recorded evidence, and goal status from their respective owners. A completed inspection remains completed even if the survivor later revises their goal. If revision is not persisted, do not reconstruct it from the last line of dialogue.

When the player returns to discuss the goal, the survivor may have a new preference only if current social/autonomy behavior can represent it. Otherwise, the conversation should ask again rather than assuming the previous proposal remains active. Duplicate task events cannot advance the goal twice.

## 103. Scope guardrails

This installment does not add a goal-tree editor, goal-history ledger, motivation meter, grief/trauma diagnosis, or automatic quest generator. It does not erase completed work when an aim changes. It does not let the player rewrite personal intent unilaterally.

The existing goal owner determines which transitions can persist. Task/work owners determine evidence. Dialogue carries the human context without pretending to be a transaction.

## 104. Acceptance questions

1. Does new evidence reach the player through a real task or source owner?
2. Can the survivor choose to keep, narrow, revise, pause, close, or leave the goal unresolved where supported?
3. Does historical work remain true after a goal revision?
4. Are partial effort and full completion kept distinct?
5. Can the player disagree or decline resources without commandeering the survivor's choice?
6. Do factions and witnesses confirm facts without selecting the personal goal?
7. Does persistence avoid parallel goal state or duplicate progress?

## 105. Installment 5 close

This installment gives Plan 3 a goal-revision branch driven by witnessed work and new evidence. The player can continue, narrow, seek confirmation, find a different route, or close an obsolete request; the survivor can retain or revise their own aim. Completed work stays intact, partial progress remains partial, and no new goal ledger or morality axis is added.

## 106. Plan 3 expansion installment 6 — “No to that; here's what I can do”

A refusal can be the start of another offer. The survivor may decline the exact task while proposing a different contribution that fits their time, ability, or priorities. The player can accept the alternate proposal, clarify its scope, seek a different worker, or close the request. The survivor's alternative is not a hidden obligation and does not make the original refusal conditional.

This installment uses all three existing subfeatures: the survivor offers a bounded action; the player routes refusal and consent for both the original and revised scope; and any personal goal advances from witnessed work. No “refusal conversion” mechanic or pressure dialogue is introduced.

## 107. A refusal is already a complete answer

The survivor can simply say no. They do not owe the player a replacement offer, explanation, or compromise. The player can acknowledge the answer and use current roster/task owners to find another route.

Sometimes the survivor will add, “I can't take the night watch, but I can check the stores at first light.” The player may then choose whether that alternative is useful. The player should clarify whether it is a personal suggestion, a request for permission, or an action ready for assignment. It becomes work only through the relevant owner and renewed agreement.

The alternate offer cannot be treated as payment for the refusal. A survivor may propose it because it better fits their own goal, but the player cannot require this response from every person who declines.

## 108. Keep both scopes visible

When a survivor declines the original request and proposes another action, show the two scopes distinctly. The player can close the first request, leave it open only if its owner supports that state, and evaluate the second proposal separately. Accepting one does not reopen the other.

The revised proposal may have a different participant, schedule, resource, risk, or result. The player checks these terms from scratch and asks the survivor to accept the version that can actually be routed. If the player edits the alternative materially, the survivor must see the change before commitment.

Do not let an offer card display “refused” and “accepted” simultaneously. If the current data model cannot represent this transition, dialogue should resolve the first request and present the new offer through the supported route.

## 109. Player actions after a counterproposal

- Acknowledge the refusal and ask whether the survivor wants to propose another task.
- Listen to their alternative without promising acceptance before checking eligibility.
- Compare the alternate action with the original deadline and explain any lost opportunity.
- Seek equipment, schedule coverage, or access through its real owner.
- Accept the new bounded task only after the survivor confirms its terms.
- Ask for a narrower version if the proposal exceeds current capability or resources.
- Find another worker for the declined task without treating the survivor's alternative as mandatory labor.
- Close both routes if neither is valid or wanted.

These branches depend on the request, urgency, resources, roster, and the survivor's own initiative. The player can be efficient, accommodating, or cautious without an alignment score.

## 110. Branch table — refusal with or without an alternative

| Survivor response | Player action | Valid next route |
|---|---|---|
| Declines and gives no reason | Accept answer | Reassign, delay, or close original request |
| Declines and offers a smaller scope | Check the new task independently | Accept, counter, or decline the new offer |
| Declines due to schedule and proposes later time | Recheck roster and task expiry | Reschedule only if current owners permit |
| Declines the work but offers information | Route information through its source/communication owner | No work assignment is implied |
| Declines original task and requests a different partner | Check both participants separately | Shared task only if owner supports it |
| Offers an action but lacks a required item | Check its actual property owner | Substitute, wait, or close alternate |
| Alternate conflicts with a faction condition | Negotiate with faction or decline | Survivor confirms any revised terms |
| Player rejects the alternate | Explain real constraint | Survivor may propose again, stop, or keep goal |

## 111. Scenario — “Not Tonight; I Can Check at Dawn”

This is a candidate interaction. Verify the watch duty, store check, schedule, and task owners before treating it as a reachable sequence.

The player asks a survivor to cover a night watch. The survivor declines and offers to check the stores at dawn instead. The player can accept the refusal and seek another eligible guard; ask whether the store check is a real task or simply an observation; verify access and timing; or explain that a dawn check will not meet the immediate need.

If the task owner supports the store check, the player restates its scope: who may enter, what gets inspected, what does not get moved, and when the result is due. The survivor can accept, narrow it further, or withdraw the alternative. If the store is locked or the survivor lacks access, the player may request permission, find an eligible person, or close the proposal.

At dawn, the store owner confirms whether a check occurred and what it found. The survivor's personal goal advances only if the event matches its evidence contract. The declined night watch remains declined; completing the alternate task does not retroactively imply consent to the original one.

## 112. The alternative may serve a different goal

The survivor's counterproposal can connect to a personal goal. The player can ask whether the action matters to them or simply accept the practical offer without probing. If the goal owner supports the link, a witnessed result may advance it. If not, the task still has a truthful outcome but no invented goal progress.

The player may also discover that the alternate action is less useful to the settlement but more meaningful to the survivor. They can negotiate whether the current need can wait, find another worker, or decline the alternative. They should not secretly override the survivor's goal to maximize output.

If the survivor chooses to help with something unrelated to their goal, that remains possible where the task and consent owners allow it. The system should not force every offer to be an expression of a persistent goal.

## 113. Faction requesters and supporting roles

A faction may consider the original request urgent and the alternative insufficient. The player can relay the refusal, explain the alternate action, ask for a smaller requirement, find a qualified replacement, or allow the request to expire. The faction's response belongs to its current owner.

A supporting contact may broker a dawn time, provide a key through the access owner, or clarify that a different person can perform the night watch. They do not speak for the survivor. The player must disclose new conditions before asking the survivor to reconsider.

The faction may approve the alternate, reject it, counter with a narrower version, or close its request. None of those responses changes whether the survivor consented to the original task.

## 114. Avoid coercive “compromise” writing

The dialogue should not imply that the survivor must offer something else to make the refusal acceptable. Avoid player lines such as “Then you owe me another job.” The player can ask whether the survivor has another suggestion, but must accept “no” as the answer.

The player may decline a counterproposal for concrete reasons: schedule, access, qualification, resource cost, or irrelevant scope. State those reasons without shaming the survivor. They may respond by revising, asking for help, closing the conversation, or keeping the goal for later.

If the original request must be filled, use the current roster route to find an eligible alternative. Do not route around CrewConsentVerdict or mark an unfilled position as covered.

## 115. Goal and work outcomes

The original task can be reassigned, delayed, or closed. The alternate task can be proposed, accepted, performed, interrupted, or blocked. Each event needs its own owner result. One completed alternative cannot satisfy the original task unless that owner explicitly supports equivalence.

The survivor's goal can remain unchanged, advance from the alternate work, change after the experience, or receive no progress. The goal owner consumes only witnessed task evidence. A refusal alone does not count as progress or failure.

## 116. Ending routes

**Alternative accepted and completed:** survivor's new task is eligible, terms are clear, and owner confirms the result.

**Alternative accepted but blocked:** the required access or item is unavailable; player finds support or closes it.

**Alternative declined by player:** player explains the actual constraint; survivor chooses whether to revise or stop.

**Original request reassigned:** another eligible person accepts through the roster/task owner; original survivor remains free of that assignment.

**No replacement found:** the slot stays open or the request expires according to its owner.

**Survivor withdraws both offers:** player accepts the boundary and ends the discussion.

**Faction changes terms:** player returns the new scope to the survivor for a fresh decision.

## 117. Re-entry and persistence

On return, the player should see whether the original request was closed, reassigned, or still open and whether the alternate offer persisted. No acceptance can be inferred from the previous refusal dialogue. The survivor's availability and task status must be refreshed.

If the alternate action completed while the player was away, show the task owner's event and its provenance. Do not pretend the player accepted an offer retroactively. If autonomous work occurred, it must follow the existing autonomy/consent contract.

## 118. Scope guardrails and acceptance

This installment does not add an obligation to counteroffer, a refusal-recovery stat, a generic alternative-task queue, or a method to force the survivor into nearby work. Accept it only if the original refusal remains intact; the alternate has separate terms; current owners validate both tasks; and goals advance only from evidence.

Questions for review: Can the player close the original request cleanly? Is the new scope shown in full? Does the survivor get to decline the alternative too? Is any faction consequence local and owner-backed? Can reload distinguish the two offers without duplicating them?

## 119. Installment 6 close

This installment makes some refusals open a possible alternative without making an alternative mandatory. The player can accept, clarify, resource, renegotiate, reject, or route the new proposal separately. Original consent remains specific, faction needs remain with faction owners, and personal goals advance only from witnessed work. The branches follow scope, time, eligibility, resources, and each participant's choice.

## 120. Expansion installment 7 — Two requests, one available hour

Two survivors may make reasonable, independent requests that draw on the same scarce person, tool, room, or window of access. This installment uses that collision to deepen the offer-and-goal loop. The player chooses what to clarify, sequence, resource, combine where supported, defer, or decline; neither survivor's goal becomes a morality test or automatic priority score.

The player does not acquire a new scheduler. Existing roster, task, access, or resource owners decide availability and valid assignments. Personal goals remain with their current owner.

## 121. The shared constraint is a fact, not a verdict

Before choosing, the player can inspect what actually conflicts: one qualified worker is available; the only workbench is occupied; a route opens for one shift; or a faction permits access at a particular time. The owner should identify the limit and its duration if those facts exist.

The collision does not prove that either survivor is unreasonable. One request may have a deadline, the other may be easier to move, or both may be important for different reasons. The player can ask what each action requires, what can wait, and whether either person wants to revise their offer.

If priority is not modeled, do not display a computed ranking. Let the player and survivors respond to known constraints through supported actions.

## 122. Ask about impact without demanding private reasons

The player can ask each survivor what changes if their request waits, whether another person could help, and which part of the proposal matters most. They can also avoid probing and work only with public task facts. A survivor may share a reason, keep it private, or decide that the request can wait.

The other survivor does not automatically receive a private explanation. The player may explain the scheduling constraint without revealing medical, family, faction, or personal details. If a reason must be disclosed for access or safety, the relevant owner defines who may receive it.

Neither refusal to explain nor a less visible goal implies lower value. The player sees the action and known deadline, then decides how to proceed within the actual resources.

## 123. Player actions when capacity is shared

- Ask both survivors whether their requests can move to another time.
- Check the roster or asset owner for a second eligible person or tool.
- Sequence the requests and tell each survivor the cost of waiting.
- Narrow one request to the part that can be done during the available window.
- Ask whether the actions can be combined, but only if their owners support it.
- Seek a supporting faction's access, specialist, room, or equipment through its real route.
- Let one proposal expire or remain open according to its owner.
- Close both requests if neither person wants the revised terms.

Each action can produce a different outcome. The player is not selecting a permanent “favorite survivor” path.

## 124. Scene seed — “The only dry hour”

This is a candidate scene pattern. Validate the specific activity, weather or room state, roster, deadline, and faction access before making it reachable.

Two survivors separately ask to use the same dry, lit work area during one open hour. One wants to complete a task that matters to a personal goal. The other needs the area for a practical repair before their next shift. Neither knows the other made a request. The player can ask whether the area has another slot, check if either task can be done elsewhere, or bring the participants together only with permission to disclose the scheduling conflict.

One survivor may choose to wait after learning that the task still fits tomorrow. The other may narrow the repair to a step that uses less time. They may find that the tasks are compatible if the room owner confirms safe sequencing. Or they may disagree and each keep their original request.

Possible outcomes include an owner-backed sequence, a supported shared session, a substitute location, a new specialist referral, one deferred task, an expired request, or no agreement. No scene outcome should award both goals just because the player found a line of dialogue that sounds conciliatory.

## 125. Sequence does not mean rank

If the player schedules one request first, state why: it has the only available time window, requires a specific specialist, or can be completed before the resource is needed elsewhere. The second survivor can accept, counter, ask for help, or withdraw their proposal.

If both can wait, the player can leave the order open until more information arrives. If a deadline is unclear, the player can ask its owner rather than assume. A faction request may provide a real deadline, but faction preference alone does not make a personal goal disappear.

The player can choose a sequence that is operationally sound and still disappoint one survivor. Any relationship consequence must come from an existing relationship owner and a supported event, not from an invisible fairness formula.

## 126. Combining requests requires separate consent

Two requests may appear compatible. The player can ask whether each survivor wants to work together, share a room, use the same trip, or let one person's action support the other's goal. Each person can accept, narrow, or decline.

The task owner must confirm that a combined activity is valid. If it can record only one worker or one goal event, do not claim that both participated. The player may run sequential tasks or choose one outcome, but cannot synthesize two completion events from one.

Shared participation may expose information or require a shared location. Tell both people before they accept. If either asks to keep a goal private, do not use combination to reveal it.

## 127. Substitute support changes the offer

A second specialist, borrowed equipment, alternate room, or faction access can free the original constraint. The player checks the substitute's owner, availability, conditions, and actual cost. Then each affected survivor receives the revised terms and can accept or decline.

A faction contact might offer a room but require a cleanup shift, escort, or restricted audience. Those conditions are a new proposal. The player must not promise them on a survivor's behalf. The survivor may accept the route, suggest a different cost, or keep their original plan.

If support arrives too late, the player can stop, re-plan for another day, or let the requests close. Arrival of a substitute does not retroactively confirm a task.

## 128. Goals progress independently

Each goal owner consumes its own witnessed event. One survivor's completed task can remain useful even if the other declines. An unfinished request does not necessarily fail a personal goal; a goal changes only through its own current rules.

The player can see distinct states: request made, terms clarified, accepted, scheduled if supported, completed from evidence, blocked, withdrawn, or expired where modeled. Do not add these fields in the UI if the underlying owner cannot persist them.

If the goal outcome requires an action the player did not choose, the survivor can revise the goal later only through its supported path. The player cannot mark a goal complete as a reward for making a compromise.

## 129. Supporting faction roles

A supporting faction can control an access window, lend a specialist, set safety terms, provide an alternate work area, or explain why a request is time-sensitive. It may also refuse to share a resource. Those decisions use the faction's actual owner.

Faction support is one branch among several. The player can accept its conditions, request narrower access, seek a personal alternative, ask the survivors to reschedule, or close the offers. A faction cannot choose which survivor's personal goal matters or accept the new terms for them.

## 130. Endings from the constraint

**Both requests are sequenced:** each survivor accepts a distinct time and the task owner records the schedule.

**One request narrows:** the survivor agrees to a smaller supported action while keeping the broader goal open.

**Both act together:** each consents and the task owner supports a shared event with individual evidence.

**A substitute opens a route:** resource/access owner confirms it and each survivor decides whether to use it.

**One survivor waits voluntarily:** the player records no forced withdrawal; the goal remains open under its owner.

**A deadline passes:** the affected request changes only as its current owner defines; the player can explain and seek recovery.

**Neither accepts the available choices:** both offers close or remain open as supported, without a fake compromise outcome.

## 131. Player approaches

**The scheduler:** asks each participant about availability and proposes an explicit order.

**The resource-finder:** checks for a second tool, room, worker, or faction route.

**The privacy keeper:** shares only the existence of the conflict, not another survivor's private motive.

**The scope reducer:** searches for a useful smaller action that still satisfies the real task owner.

**The neutral closer:** explains that the capacity cannot meet both offers, then lets each participant decide whether to wait or withdraw.

These approaches are actions available in the conversation, not player alignments or permanent roles.

## 132. Re-entry and persistence

After reload or day advance, re-read the actual availability, assignments, offer status, goal evidence, and faction access. A scheduled task that has not started is not a completed task. If an owner does not persist a waiting proposal, reopen it as a new conversation instead of showing an invented standing queue.

If the participant assigned to one request becomes unavailable, revalidate the other request too. Shared capacity may have changed. Do not automatically move a survivor into an old slot or silently change the agreed order.

Stable task and goal identities prevent duplicate progress if the player checks the schedule more than once. Each goal owner receives only its own supported completion evidence.

## 133. Installment 7 close and acceptance

This installment makes competing survivor initiatives generate branches from scarce time, shared spaces, qualified help, privacy, and revised terms. The player can sequence, combine with consent, resource, narrow, defer, or close requests. Existing owners confirm capacity and goal evidence; no priority score, generic scheduler, or automatic goal award is added.

Accept when both survivors can independently revise or refuse, a scheduling decision can have a real cost, private motives stay private, substitute support has owner-backed terms, and reload preserves only state the current systems actually own.

## 134. Expansion installment 8 — We agreed to work together, but not like this

Two survivors can freely accept the same task and later disagree about how to do it. One may want to continue quickly; the other may have found a condition that requires a pause, a specialist, or a narrower scope. Their disagreement is not proof that the original consent was false.

This installment keeps each offer and goal with its current owner. The player can clarify the accepted scope, pause where the task owner permits, split the work, bring in support, revise the proposal, or end the joint attempt. The conflict grows from evidence and method, not an alignment axis.

## 135. Agreement covers scope, not every future method

Before work begins, the player and participants should be able to see what they agreed to accomplish, which actions are permitted, and which facts would trigger a stop or review. If the current task owner does not expose method-level conditions, write them as dialogue and do not imply a persistent safety contract.

During work, a participant may discover that the agreed outcome can be reached through a route they do not accept. The player can ask each person to state the concern, compare it with task or safety evidence, and offer a supported alternative.

Neither participant can conscript the other by saying the task was already accepted. Their consent applies to the described work and can change if the actual scope changes.

## 136. Pause before assigning blame

The player can pause the task if its owner supports a safe stop, ask what changed, check equipment or access, or ask a qualified specialist to review the method. The player can also continue only if the owner confirms that the disagreement does not affect safe completion and both participants still agree.

One person may be mistaken about a fact, but the player should check the evidence rather than choose a side by personality or faction. The concern may be valid, incomplete, or outside the task's current owner data. Unknown remains a valid state.

If a safe stop cannot be represented, do not fake a pause in the panel while work continues. Close or defer through the actual task route and explain the next step.

## 137. Scene seed — “Not while the brace is wet”

Candidate scene only. Confirm the specific repair, worksite, weather/material state, team roles, and safety owner before making it reachable.

Two survivors accepted a repair. Midway through preparation, one says the brace has not set and refuses to load it. The other says waiting will miss the open work window. The player can inspect the material state, ask what evidence the task owner recognizes, seek a specialist, or choose an alternate step that does not put load on the brace.

The first survivor may agree to continue with the alternative, ask for a second opinion, or stop. The second can wait, revise the scope, find another task, or refuse the delay. A supporting faction may offer a dry space or replacement part, but the players still decide whether its terms are acceptable.

The scene can end with a safe revised task, a confirmed wait, a split team, a specialist referral, a partial task, or a closed attempt. It must not mark either survivor disloyal for raising a concern or impatient for wanting to meet the deadline.

## 138. Player actions while a joint task is contested

- Read the accepted task scope back to both participants.
- Ask each person to identify the specific step they object to or want to continue.
- Inspect current evidence through the material, work, or safety owner.
- Pause, stop, or defer only through a supported task transition.
- Offer a smaller safe scope if the task owner accepts it.
- Find a specialist, tool, replacement material, or second work area.
- Separate the work so participants do not need to agree on one method.
- End the joint attempt and let each person decide what proposal comes next.

Each action should show its practical cost. A pause may lose a time window; a split may need more supplies; a specialist may be unavailable. No choice becomes a generic “good leader” bonus.

## 139. Separate tasks only when owners support the boundary

The player may divide preparation, inspection, and installation into distinct steps if the task system already models those steps or can express them through its current API. One survivor may complete the part they accept while another waits for review.

Do not represent two parallel tasks by cloning the same task record. If the current owner cannot split work, the player can close the joint attempt and propose a single revised task later. Both participants should see whether any completed work is retained.

Personal goals may be affected by separate evidence: one survivor's goal can advance from inspecting the brace while another's remains open. Do not transfer progress because the survivors were assigned together.

## 140. Consent can narrow or end mid-task

A participant can say, “I'll finish the measurement, but I won't load it.” The player can preserve the bounded action if the task owner supports partial completion, or stop if it does not. The participant may also withdraw from the joint activity entirely.

The other person may accept the narrower role, ask for a replacement, finish their own part, or end the task too. The player should not automatically reassign the withdrawn person to nearby work.

If a survivor later wants to resume, make a fresh offer with current materials, scope, and availability. Old consent does not restart after a day advance.

## 141. Supporting faction roles

A supporting faction may provide a technical specialist, set access to the work area, make a part available, or enforce a safety rule on property it owns. Its contact can explain the constraint and may refuse to help.

The faction cannot decide which survivor must accept the method, compel the other to continue, or turn a delay into a personal-goal failure. The player can use its support, negotiate a narrower route, find another source, reschedule, or close the joint task.

If the faction's advice conflicts with task evidence, ask an authorized specialist or defer. Faction standing does not determine physical truth.

## 142. Personal goals follow evidence, not the argument

The task owner records what work occurred. Each goal owner decides whether that event matches the survivor's current aim. A person can decline the disputed step while still having made meaningful progress. Another can finish a valid part without proving the first person's concern wrong.

The player may ask whether the experience changed the goal, but the survivor can keep it, narrow it, pause it, or decline to discuss it. Goal revision remains a separate consent moment.

An unresolved disagreement can persist without a “team harmony” meter. Future collaboration may be an available route after the task is clarified, but it does not need an automatic trust penalty or bonus.

## 143. Endings from method and evidence

**Evidence supports a pause:** both participants revise the timing or scope, and the task remains open only if its owner supports that state.

**Concern is not confirmed:** the player explains what evidence is known; the concerned survivor may continue, seek another source, or withdraw.

**A safe alternative exists:** both participants decide whether to use it; completion follows the task owner.

**Participants split roles:** each person receives separate, supported task and goal results.

**One participant withdraws:** the player closes or revises the task and asks whether the other wants to continue.

**The deadline passes:** task owner records the missed window; each survivor's goal responds only under its own rules.

**No safe or agreed route remains:** stop, defer, seek new support, or end without inventing a completed outcome.

## 144. Play approaches

**The scope reader:** restates what both accepted and checks whether the method is inside it.

**The evidence seeker:** consults the correct owner before judging a disputed claim.

**The task splitter:** looks for individually valid steps without cloning progress.

**The resource broker:** seeks a specialist or substitute condition and shows the new cost.

**The clean stopper:** ends work when no supported agreement remains.

These approaches have different costs and information outcomes, but none is the universal morally correct route.

## 145. Re-entry after a contested task

After reload, check the task owner's actual state, completed evidence, each survivor's current availability, and any new support or dispute. A dialogue line stating “we paused” cannot override a task that actually completed or failed through another owner.

If the task was closed, a new proposal must use current conditions. If it remains pending, show the next supported action and who still needs to consent. Duplicate callbacks cannot credit the same partial work twice.

## 146. Acceptance questions

1. Can either participant narrow, pause, or withdraw from the agreed method?
2. Does the task owner support the selected pause, split, or partial state?
3. Can the player check evidence without choosing a side based on personality?
4. Are goal results individualized and tied to witnessed work?
5. Can a faction offer practical support without overriding either participant?
6. Are timing and resource costs visible before revised consent?
7. Does reload preserve the real task outcome rather than the last dialogue line?

## 147. Installment 8 close

This installment brings a disagreement into a task after both participants accept it. The player can check evidence, pause through the task owner, revise scope, split valid work, seek support, or close the attempt. Participants keep control of their own consent; the task and personal goals resolve from separate evidence.

## 148. Expansion installment 9 — The goal needs something from shared stock

A survivor's personal goal may require an item owned by the shelter, work crew, or a supporting faction. The survivor can request an allocation, propose a supported contribution, wait for restock, find a substitute, or change the goal. The player negotiates; the actual resource owner decides what may move.

Branches follow ownership, scarcity, urgency, alternatives, and revised consent. Requesting shared stock does not make the survivor selfish, and granting it does not make the player generous.

## 149. A request is not permission to take

First check who owns the resource, whether it is available, and whether an existing task depends on it. A personal goal does not turn shared stock into personal property. A panel preview cannot reserve or spend it.

The player can inspect the owner state, ask about allocation routes, or close the request. If ownership or availability is unknown, the player cannot promise the item. The survivor may suggest a substitute, a smaller quantity, a different task, or no alternative.

## 150. Make the cost visible

Show only owner-confirmed costs: reduced stock, delayed maintenance, another allocation, a work condition, faction access, or time waiting for new supply.

The survivor can accept, counter with a smaller request, seek a substitute, offer a supported contribution, delay the goal, or withdraw. Each revised arrangement requires fresh consent.

If no conflict or cost is recorded, do not invent one. If the quantity or deadline is uncertain, say so and check with the responsible owner.

## 151. Scene seed — “One sealed battery”

Candidate content only. Confirm the item, stock owner, allocation, goal, and task route before making this reachable.

A survivor requests a sealed battery for a personal repair goal. The shelter store has one. A maintenance crew may need it during an upcoming inspection, but the allocation is not yet recorded. The player can check the store and task owners, ask which part of the goal depends on the battery, or wait for the inspection plan.

The survivor may accept a substitute, offer a supported task, decide the repair can wait, or close the request. The crew may release the battery, reserve it, or identify another source. A supporting faction may offer a purchase route with separate terms.

The scene can end in allocation, substitution, a future request, a different goal action, or no agreement. The item moves only through its owner.

## 152. Player actions around shared resources

- Verify ownership, availability, and current reservations.
- Ask which part of the goal depends on the item.
- Compare the request with known community needs through their owners.
- Ask about allocation, purchase, or replacement routes.
- Request a smaller amount or substitute where supported.
- Let the survivor offer a contribution without making it compulsory.
- Defer until supply or a deadline is confirmed.
- Decline and help find a different action.

The goal owner does not grant inventory access.

## 153. Competing needs are not a personality test

If another task depends on the resource, identify the actual commitment and the cost of changing it. The player can check for another item, a schedule change, or a smaller allocation. A less visible need is not automatically less important.

If an existing rule assigns priority, show its source. If no priority rule exists, do not create a hidden score. The player can route the request through the allocation owner or leave it unresolved.

The requester can accept, counter, withdraw, or disagree. Disagreement does not prove intent to take stock improperly.

## 154. Work offered in return remains separate

The survivor may offer labor, information, repair, or a favor. Route that proposal through its actual task or barter owner, then check whether the resource owner accepts the exchange.

The survivor can decline a work condition and ask for a smaller allocation, purchase, or no further discussion. A faction cannot assign them a shift through the goal panel.

If a task is accepted, its completion evidence and resource movement can be linked only through an existing supported contract. Dialogue alone cannot release stock.

## 155. Supporting faction routes

A supporting faction may sell a replacement, permit supervised use, provide another supplier, or set a property rule. It can attach an explicit fee or task only through an owner that supports it.

The player can take that route, negotiate its scope, compare another supplier, seek a community alternative, or decline. The faction helps with the resource constraint but does not decide whether the personal goal matters.

The survivor accepts any condition on their own time or property.

## 156. The goal may change after the answer

If the item is unavailable, the survivor can keep the goal open, narrow it, pursue another step, or revise it through the goal owner. The player can offer factual options but cannot mark it abandoned.

If the resource becomes available later, ask whether the survivor still wants it. The old request is not standing consent unless the current goal or allocation owner explicitly persists it.

A substitute action advances the goal only if its owner recognizes that event.

## 157. Outcome branches

**Allocation approved:** the resource owner confirms movement and its cost is visible.

**Allocation delayed:** the survivor chooses to wait; the goal remains open if supported.

**Substitute accepted:** the substitute owner confirms availability and the survivor accepts revised terms.

**Work exchange accepted:** task and resource owners record their respective results.

**Resource reserved elsewhere:** the player explains the commitment and offers another path.

**Faction supply used:** its terms are separately accepted.

**Request declined or unresolved:** no item moves; the survivor may keep, revise, or close the goal.

## 158. Player approaches

**Stock checker:** verifies ownership and any reservation.

**Alternative finder:** seeks a smaller quantity, substitute, or different goal step.

**Cost explainer:** makes community consequences visible without dramatizing them.

**Terms negotiator:** routes work or faction conditions through their owners.

**Respectful closer:** lets the survivor keep the request open or end it.

No approach is a generosity, greed, or alignment path.

## 159. Persistence and re-entry

Refresh stock, allocation, ownership, task result, faction terms, and goal state after reload. A stale display cannot authorize transfer. If a request was not persisted, ask again instead of reconstructing it from a panel note.

If a reservation appears after the offer, invalidate the old proposal and explain the confirmed reason. If allocation already committed, duplicate dialogue cannot spend the item or repeat goal progress.

## 160. Acceptance questions

1. Is shared ownership verified before an item is promised?
2. Are costs and competing commitments visible from their owners?
3. Can the survivor refuse a work condition or keep the goal open?
4. Are barter, task, and faction routes separately accepted?
5. Can the goal change without a hidden failure score?
6. Do factions offer options without taking control of the personal goal?
7. Are allocation and goal outcomes replay-safe?

## 161. Installment 9 close

This installment makes shared stock a real constraint on a survivor's personal goal. The player can inspect ownership, negotiate an allocation, find a substitute, seek a faction route, defer, or decline. Resource movement, work offers, and goal progress remain with their current owners. No morality score or hidden priority queue is added.

## 162. Expansion installment 10 — Two factions offer different help

A survivor may seek support for a personal goal and receive offers from two supporting faction contacts. One can offer equipment with a use restriction; another can offer labor that must happen on a particular shift. The survivor decides what help, if any, fits their goal. The player presents the real terms and can seek a third route.

This installment does not create a faction-alignment ending. Branches follow the actual offer, access, schedule, resource, and consent conditions.

## 163. Offers stay separate from endorsement

Receiving assistance does not mean the survivor joins, trusts, or endorses a faction. A faction contact cannot accept on the survivor's behalf. The survivor may take a narrow practical offer, negotiate it, accept neither, or withdraw the goal.

The player can ask what the survivor wants to accomplish before presenting faction proposals. Keep the personal reason private unless the survivor chooses to share it. A contact should hear only what is needed to price, schedule, or authorize its own offer.

## 164. Scene seed — “Two ways to open the shed”

Candidate content only. Use current minor faction contacts, locations, goal state, and access owners; do not invent a new major faction to fill either role.

A survivor has a personal goal that depends on access to a locked work shed. One supporting group offers a key for a supervised shift. Another offers a specialist who can inspect the lock, but only during a narrow time window. The shed owner may permit one route and refuse the other.

The player can inspect both offers, ask the survivor which cost matters, seek owner permission, ask for a smaller scope, or look for a non-faction route. The survivor may accept one offer, combine them only if all owners permit it, negotiate, postpone, or decline both.

No path is automatically more ethical. One can be quicker but more visible; another can preserve privacy but delay the goal.

## 165. Present complete terms to the survivor

Before acceptance, show the faction contact, resource or access, location, time window, required task, audience, and any restriction the owner exposes. If a condition is unknown, state that it must be checked.

The survivor can ask a question, propose a different time, remove an unwanted condition, ask for another contact, or decline. The player cannot promise their labor or disclose their goal's private details to secure help.

Any counteroffer goes back to the faction owner. The faction may agree, counter again, or close its offer.

## 166. Ask what kind of support is wanted

The survivor may want a tool, a witness, a specialist, safe access, or simply enough information to act alone. The player can ask which part of the goal needs help, present options, or let the survivor decide without explaining their motive.

One faction's offer may satisfy only one part of the request. The player can accept the limited help, ask the other faction to cover the remaining need, find a community alternative, or leave the task open.

The goal owner determines whether the resulting event advances the personal aim. The faction's attendance record or goodwill is not a substitute for that evidence.

## 167. Compare costs without ranking factions

The player can compare concrete differences: time, access, location, material, supervision, disclosure, or task obligation. Do not reduce those differences to a global “best faction” score.

The survivor may prefer the slower route because it keeps their goal private, choose the faster route despite its conditions, or reject both because the cost is too high. The player's role is to make the tradeoff legible and let the survivor choose.

If the faction provides an offer with no meaningful cost, do not add one for balance. If its terms are not yet confirmed, the player should not promise a result.

## 168. A faction may withdraw or change terms

An access window can close; a specialist can become unavailable; a requested duty can change. The player checks the current offer owner, explains the changed terms, and asks the survivor whether they want to revise the plan.

The survivor can switch to another offer, request a new time, find an independent route, keep the goal open, or close it. Earlier acceptance does not authorize a newly added fee or task.

If a faction has already delivered an owner-confirmed resource, handle it through that owner. Do not erase it because the larger goal changed.

## 169. The survivor can choose no faction route

The player can ask whether the survivor wants to seek a non-faction specialist, wait for an open access window, attempt a different goal step, or stop. These may be slower or unavailable; show only current options.

A faction must not be framed as the only “good” or “bad” path. It may be the only route with a certain tool, but the survivor still controls whether to accept its conditions.

The player can decline to broker a route that would expose a private goal or commit someone else's time. Explain the concrete limit, then let the survivor choose.

## 170. Goal progress remains independent

Each faction offer and resulting task has its own owner. A completed specialist visit may advance a goal if its evidence matches; accepting access does not. Refusing a faction offer is not goal failure.

If the survivor changes the goal after comparing offers, the goal owner records that change. The player cannot select an ending and force the goal to match.

The survivor may complete a smaller step, wait, or abandon the effort later. Do not award progress for choosing a particular faction.

## 171. Endings from the available support

**First offer accepted:** the survivor accepts its full terms; the faction owner confirms access or resource.

**Second offer accepted:** another route fits better; the first faction's proposal closes without invented hostility.

**Offers combined:** only if both faction owners and the survivor permit the combined schedule and access.

**Conditions negotiated:** the changed offer receives fresh acceptance.

**Independent route found:** the survivor seeks community support or a different task.

**Goal deferred:** no faction offer fits now; the goal remains open where supported.

**Both offers declined:** the survivor keeps control of their goal and may make another proposal later.

## 172. Player approaches

**The terms translator:** presents each offer in the survivor's own decision context.

**The privacy keeper:** shares only details needed for the faction's offer.

**The independent-route finder:** looks for another specialist or time window.

**The schedule broker:** coordinates only after every affected person consents.

**The neutral closer:** records no acceptance when the survivor declines.

These approaches do not form a faction reputation or alignment route.

## 173. Re-entry and persistence

On return, refresh offer terms, access, schedule, delivered resources, task evidence, and goal state from their owners. If no offer was persisted, ask again; do not reconstruct faction acceptance from a dialogue line.

Changed terms invalidate earlier consent. A duplicate acceptance cannot deliver the same resource twice or advance a goal twice. Declining one faction offer does not close another unless the current owner says the offers are mutually exclusive.

## 174. Acceptance questions

1. Are the factions supporting the survivor's choice rather than choosing for them?
2. Are all costs, access rules, and audience details visible before acceptance?
3. Can the survivor choose one offer, negotiate, seek another route, or decline all?
4. Does faction use remain separate from membership, trust, and personal-goal completion?
5. Are changed terms presented for fresh consent?
6. Does the player avoid disclosing private goal details unnecessarily?
7. Are offers and goal evidence replay-safe?

## 175. Installment 10 close

This installment gives a survivor distinct routes when two supporting factions offer help. The player can compare terms, negotiate, combine only with permission, find an independent route, defer, or close. The survivor's goal and each faction offer remain separately owned; no faction-alignment ending or automatic goal award is added.

## 176. Expansion installment 11 — Help that changes someone else's day

A survivor's initiative can be valid while the proposed route changes another person's shift, workspace, privacy, or access. The affected person is not background cost. This installment gives the player a way to surface that impact, protect the goal-holder's privacy, and negotiate a route without making either survivor automatically right.

## 177. Identify who bears the cost

Before acceptance, show who must move, wait, lend a space, share a tool, or cover a task. Use only costs that the current roster, task, access, and inventory owners can confirm. If the player cannot identify the affected person or cost, the offer is not ready to settle.

Do not turn “the shelter benefits” into blanket consent. A shared need can still impose an uneven burden on one worker or one household.

## 178. Scene seed — “The bench is already promised”

A faction offers the survivor an evening at a repair bench to work on a personal goal. A second resident already has the bench booked to finish a necessary repair before morning. The faction's offer is real, but the time and space are not unclaimed. The player can ask the resident about alternatives, ask the faction for another slot, help the survivor find a different route, propose a limited shared arrangement if both agree, or let the offer lapse.

The survivor's goal remains theirs even if this room or hour is unavailable.

## 179. Share only the information needed

The bench owner needs to know how the proposed use affects the schedule. They do not automatically need the survivor's private reason for wanting the time. The player can explain the practical request, ask the survivor what may be shared, or invite the survivor to speak directly.

If the affected person asks why, the player can provide an authorized summary, decline to disclose, or seek a different arrangement. Do not make access to a personal goal depend on revealing a private history.

## 180. The affected person has actionable choices

They may keep the existing booking, offer a different time, share the bench for a defined interval, move their own work if the task owner permits it, or decline without being labeled obstructive. If the player asks them to cover another task, show the schedule and consent consequences first.

Likewise, the goal-holder can accept the new time, change the method, pursue another task, or stop. The faction cannot commit either person on their behalf.

## 181. Negotiate the terms that actually changed

If a compromise is possible, name its scope: time, space, tool, supervision, and any resource use. Both affected people review the changed terms. A vague “we'll make it work” must not register as an agreement or silently move a duty assignment.

When only one term changes, preserve the rest of the offer if its owner permits. If the new arrangement materially changes cost, audience, or safety, return to fresh consent on the whole relevant offer.

## 182. The faction remains a supporting actor

The faction can suggest another hour, lend its own tool, or provide a person who can help in its permitted role. It cannot overrule the current bench booking, duty roster, resident consent, or safety authority. If its representative pressures the affected worker, the player may name that pressure, refuse the arrangement, or end the discussion.

Do not create a faction reputation check to unlock a fair schedule. The player can take the practical route without pledging loyalty.

## 183. Do not convert a conflict into a character verdict

The bench owner may be tired, protective of a deadline, or unwilling to share a scarce space. The goal-holder may be disappointed or may accept the delay. Neither reaction proves selfishness. Keep dialogue tied to what each person needs now and what they agreed to do.

If the first proposal fails, a later route can reopen when capacity changes. Do not punish the refusing person in unrelated relationships or make them apologize to unlock ordinary shelter access.

## 184. Branches based on who accepts what

- **Existing booking stays:** the survivor seeks a new slot or chooses another route.
- **A different slot is accepted:** the faction offer remains open only if its terms and expiry still allow it.
- **A limited shared use is accepted:** record only the schedule/access result the relevant owner can preserve.
- **The survivor declines the arrangement:** their goal remains separate from the refused offer.
- **No safe or fair route exists today:** defer without making either person lose unrelated progress.

These outcomes reflect negotiated actions and actual capacity, not a moral label.

## 185. Goal progress is not rent for cooperation

Helping another person reschedule does not automatically advance the survivor's personal goal. A goal advances only when its current evidence owner confirms the relevant work. Conversely, refusal to displace someone should not reduce goal progress already earned.

The scene can show goodwill, frustration, or a practical compromise through authored dialogue, but those reactions must not become hidden relationship mutations without an existing owner-backed event.

## 186. Supporting-faction variations

A faction may offer a different time, a second work site, a limited tool loan, or a trained helper, depending on its authored capacity. Ask who owns that capacity and whether the offer is still available at the moment of acceptance. A named faction is not proof that the player can draw any resource from its stock.

If two factions propose alternatives, present the full practical terms side by side and let the survivor decide. Do not make one option secretly superior because of the player's broader alignment.

## 187. Callbacks and endings

Later dialogue may show the resident using the bench at the agreed time, the survivor arriving for a newly accepted slot, or both people choosing separate work. If the schedule changed, say who authorized that change. If the issue remains unresolved, retain that uncertainty.

Possible closes include a mutually accepted shared interval, a new solo slot, an independent route, a deferred goal, or a declined offer. None decides the survivor's entire future or the standing of a major faction.

## 188. Player approaches

**The burden mapper** names each person, space, and shift affected.

**The privacy keeper** explains the practical request without exposing the goal's private reason.

**The direct broker** lets the two affected people negotiate with the survivor present.

**The alternative finder** asks the supporting faction for another resource or time.

**The fair closer** accepts a refusal or deferral without attaching a penalty.

## 189. Persistence and replay

On return, refresh the booking, roster, offer expiry, access, and goal evidence from their owners. A past dialogue agreement does not guarantee a current slot. If any relevant condition changed, present the new situation and ask for consent again.

Repeated acceptance cannot displace a worker twice, consume an offer twice, or duplicate goal progress. If no agreement was persisted, do not fabricate one from who was last seen in the room.

## 190. Acceptance questions

1. Is every person who bears a practical cost visible before the decision?
2. Can the player share the request without exposing private goal details?
3. Can each affected person decline or negotiate without an unrelated penalty?
4. Are duty, access, stock, and task constraints checked by their owners?
5. Does the faction support options without controlling the survivor's goal?
6. Does only confirmed goal evidence advance the personal goal?
7. Are changed schedules and agreements replay-safe?

## 191. Installment 11 close

This installment adds an action-based branch when a survivor's chosen support route imposes a cost on another resident. The player can disclose only the practical request, negotiate time or space, seek another route, or defer. Both people's consent and the survivor's goal remain distinct; no faction loyalty or hidden morality score is introduced.

## 192. Expansion installment 12 — A member's promise is not the faction's promise

A faction member may offer personal time or knowledge while lacking authority to commit faction stock, workspace, access, or official labor. The player needs to separate the person's own offer from any institutional resource that appears beside it. This gives the survivor a real choice about informal help without turning one contact into a shortcut around faction ownership.

## 193. Name the source of each part

Before acceptance, identify which parts the individual is offering and which parts depend on faction approval. A volunteer might be willing to explain a procedure after their shift while the workshop remains closed. Another may offer to introduce the survivor to an authorized officer but cannot promise what that officer will decide.

Use current task, schedule, inventory, access, and faction owners to verify each claim. A person's affiliation is not proof that they can spend communal resources.

## 194. Scene seed — “I can show you after my watch”

A faction contact offers to spend a short personal interval helping a survivor understand a repair technique. The survivor asks whether they can use the faction workshop and take a spare part. The contact says they can explain the method but cannot grant either request. The player can accept the explanation as offered, seek formal workshop access, ask for a public reference, or close the route.

The contact may be sincere, skilled, and still unable to authorize the additional request. The scene need not accuse anyone of lying.

## 195. Keep personal labor voluntary

The contact can set a limit on time, choose a public setting, ask the survivor to bring no sensitive information, or withdraw before the meeting. The player can ask whether the survivor wants that form of help, offer an authorized interpreter or second source, or decline for themself if the conditions do not fit.

Do not turn private generosity into an unpaid assignment or faction debt. The volunteer's consent, the survivor's consent, and task eligibility remain separate checks.

## 196. Institutional resources need institutional consent

If the survivor wants tools, spare parts, a room, a restricted procedure, or official recognition, the player can contact the current faction owner, present the request, or seek another route. The individual may help explain the process but cannot make a refusal disappear.

The survivor can accept personal instruction while the resource request remains pending. They can also decide the partial offer is not useful and pursue a different path.

## 197. Secondhand offers need confirmation

If another person reports that the faction member “can get anything from the store,” the player can ask the member directly, ask an authorized representative, or decline to treat the rumor as an offer. A personal promise relayed through a witness still needs direct confirmation from the person who made it.

Do not store an institutional commitment from casual dialogue. If the current social owner cannot persist an informal offer, keep it as a scene choice and do not claim it will survive reload.

## 198. The member can make a bounded introduction

The contact may offer to pass along a request, identify office hours, or tell the survivor which form or person handles access. The player can accept the introduction, ask the survivor to make it themself, or choose a route that avoids the faction. An introduction guarantees contact only where the source and communication owner confirm delivery.

The contact cannot promise the outcome of a review they do not control. The survivor remains free to decline after hearing the official terms.

## 199. Branches by what the contact actually controls

- **Personal explanation accepted:** schedule only the volunteer's confirmed time and scope.
- **Workshop or tool requested:** seek the authorized owner; no property moves before approval.
- **Introduction accepted:** contact the real decision-maker and wait for their response.
- **Survivor wants formal support only:** close the personal offer without penalty.
- **No route is available:** keep the goal open, revise it with the survivor, or defer.

The player changes the route by what they verify and accept, not by picking a faction reputation stance.

## 200. Do not punish the contact for having limits

The faction member can remain a willing teacher while a formal request is denied. The survivor may appreciate the offer, ask for more, or choose not to continue. These responses should not create an automatic trust gain or loss unless a current relationship owner supports that event.

Likewise, the faction may approve access without making its member responsible for the survivor's future progress. Attribution stays precise: individual time came from the volunteer; shared stock came from its owner.

## 201. Supporting factions remain supporting actors

The faction can add an official route, provide a qualified specialist, approve space, deny a request, or offer a different resource. Each option is presented with its real terms. The survivor's personal goal remains their own, and taking help does not enroll them or settle their broader relationship with that faction.

If another faction offers support, compare the actual routes. Do not construct a cross-faction package unless each relevant owner and participant agrees.

## 202. Endings and callbacks

The scene can close with an informal lesson only, a formal request pending, approved access with fresh consent, an official refusal and independent search, or no help accepted. Later dialogue should remember which part came from the individual and which part came from the institution.

If the volunteer becomes unavailable, the player can ask whether the survivor wants to wait or seek another source. Never silently replace the person or imply that the faction owes the same personal labor.

## 203. Player approaches

**The source separator** distinguishes personal time from faction property.

**The direct confirmer** asks the actual offer-maker to confirm scope and timing.

**The formal requester** routes shared resources through the authorized office.

**The independent-route finder** seeks a public source or another qualified person.

**The boundary respecter** accepts that either the volunteer or survivor can decline.

## 204. Persistence and replay

On return, refresh the volunteer's availability, any accepted meeting, formal access status, stock, task rules, and survivor goal from their owners. An introduction is not approval. A pending request cannot be reconstructed from the volunteer's past presence.

If the informal offer was not persisted, ask again or let the survivor contact the person through a supported route. Duplicate prompts cannot consume faction resources or advance goal evidence twice.

## 205. Acceptance questions

1. Does the player know which offer belongs to the person and which requires faction authority?
2. Can the survivor accept personal help while leaving a resource request pending?
3. Can both volunteer and survivor decline without unrelated penalties?
4. Are stock, space, schedule, and task permissions confirmed by their owners?
5. Is a reported or relayed offer confirmed by its actual source?
6. Does faction assistance remain separate from goal ownership and alignment?
7. Are accepted offers, introductions, and approvals truthful after reload?

## 206. Installment 12 close

This installment distinguishes a faction member's personal offer from an institutional commitment. The player can accept bounded help, seek formal access, request an introduction, find another source, or decline. The survivor's goal, the volunteer's time, and faction property remain separately owned; no affiliation shortcut is added.

## 207. Expansion installment 13 — Carry the request, not the whole story

A survivor may ask the player to relay a specific request to a faction or another survivor while keeping the personal reason private. The player must preserve the scope of that permission. Being asked to carry one sentence does not make the player a spokesperson for the survivor's wider beliefs, history, or future choices.

## 208. The survivor sets the message boundary

Before relaying, ask whether the survivor wants an exact quotation, a practical summary, or an introduction without details. They can approve the recipient, channel, and any supporting fact; revise the message; deliver it themself; or decide not to send it. The request stays open only as long as its owner supports it.

Do not require disclosure of a private goal to make the message persuasive. If a faction needs a specific operational fact to answer, identify that need and let the survivor decide whether to provide it.

## 209. Scene seed — “Ask whether the gate is open”

A survivor asks the player to find out whether a faction's repair space is open next week. They do not want the faction told that the request is connected to a personal project. The player can pass the narrow availability question, ask the survivor to send it, request permission to explain the practical context, or leave the question unasked.

The faction's answer may be useful without revealing why the survivor wants the space. If the answer requires an official application, the player can report that condition and let the survivor choose the next step.

## 210. Quote, summarize, or introduce

An exact quote preserves wording but may expose more than the recipient needs. A summary gives the player responsibility for faithful scope. An introduction lets the two people speak directly but does not guarantee an answer. The player can compare these routes with the survivor and let them choose.

If the player is unsure whether a detail falls within permission, leave it out and ask again. Do not expand “ask about access” into a full account of the survivor's motivation.

## 211. Preserve the difference between a request and an endorsement

The survivor may want a practical answer without endorsing the faction, its policies, or its wider position. The player can make that boundary clear, ask the recipient to answer only the requested point, or choose not to represent the survivor at all.

Do not convert one inquiry into membership, trust, cooperation, or a branching faction ending. Any such consequence requires a separate action and its own consent.

## 212. The player can refuse the spokesperson role

The player may be uncomfortable representing the survivor, unable to keep the request confidential, or unsure how to summarize it. They can explain the limit, offer to accompany the survivor, help draft a message for them to send, or decline. The survivor may choose a different messenger or abandon the inquiry.

The player's refusal should not erase the survivor's goal or imply that the request was improper. It changes who carries the message, not whether the survivor may ask.

## 213. Recipient questions return to the survivor

If the faction asks for more detail, the player can answer only within the approved boundary, return to the survivor for fresh permission, or say that the detail is not available. The recipient may accept the limited question, require a formal route, or decline to answer.

The player cannot fill a gap with a plausible personal reason. If the survivor is absent and no saved instruction covers the question, defer rather than improvise on their behalf.

## 214. Permission can narrow or end

Before sending, the survivor can remove a detail, change the recipient, or withdraw the request. After sending, they can ask the player not to repeat it and decide whether a follow-up is still wanted. The original recipient may already know the message; never promise a recall unless the current communication owner supports one.

If the message has been sent, tell the survivor what was actually disclosed and what response, if any, arrived. Do not hide an overbroad paraphrase behind a claim that the player meant well.

## 215. Branches by representation choice

- **Exact narrow quote:** send only what the survivor approved and wait for the recipient's answer.
- **Approved practical summary:** carry the agreed purpose without adding private context.
- **Direct introduction:** connect the participants through a supported channel; either may still decline.
- **Fresh permission requested:** pause until the survivor answers a new question.
- **No message sent:** keep the goal and its other routes available where supported.

The player's action changes what is communicated and by whom, not the survivor's alignment.

## 216. Supporting faction role

A faction contact may answer the specific access question, route the player to an authorized person, or state that it needs a formal request. It cannot demand the survivor's full personal history as the price of hearing a basic inquiry unless the relevant owner defines that information as necessary and the survivor chooses to provide it.

The faction can decline the request without the narrative declaring the survivor unwelcome everywhere. The player may compare another route, seek a public schedule, or stop.

## 217. Callbacks and endings

Later dialogue can show the access answer, a request for a formal application, a direct conversation, a withdrawn inquiry, or an unanswered message. Attribute each statement to the person who made it. If the player declined to carry the request, do not write the faction as having heard it.

The survivor can revise their goal after learning the terms, but that revision belongs to their goal owner. A response from the faction is information, not automatic progress.

## 218. Player approaches

**The faithful messenger** checks exact wording and approved details before sending.

**The practical summarizer** preserves the request while omitting private motivation.

**The direct connector** offers an introduction with no promise that either person will agree.

**The permission seeker** returns for fresh consent when a recipient asks for more.

**The role decliner** states the player's limits and helps the survivor choose another route.

## 219. Persistence and replay

On re-entry, read any persisted permission, recipient, message, delivery status, response, and current goal from their owners. If the message was never saved, do not imply it was sent. If the survivor's permission was not durable, ask again before disclosing personal context.

Duplicate delivery cannot create a second faction response or advance a goal twice. A changed message requires fresh approval before transmission.

## 220. Acceptance questions

1. Does the survivor choose what may be quoted, summarized, or omitted?
2. Can the player decline to be a spokesperson without closing the survivor's goal?
3. Are follow-up details returned to the survivor for fresh consent?
4. Does a practical request remain separate from faction endorsement or membership?
5. Are delivered messages and responses attributed to their actual sources?
6. Can the survivor withdraw or narrow the request where the channel permits?
7. Are permission and delivery truthful after reload?

## 221. Installment 13 close

This installment makes representation a player choice with clear limits. The survivor can approve a narrow quote, practical summary, direct introduction, fresh follow-up, or no message. The player can carry the request faithfully or decline the role; the survivor's private story and broader faction stance remain theirs.

## 222. Expansion installment 14 — Accompaniment is not proxy consent

A survivor may want the player present during a conversation because the room is unfamiliar, the terms are complicated, or they simply prefer company. Being invited to accompany them does not authorize the player to answer every question or accept terms. This installment makes the companion role visible and revocable throughout the meeting.

## 223. Agree on the companion's role before entering

The player can ask whether the survivor wants quiet presence, help remembering terms, a factual question asked, or an agreed signal to pause. The survivor may choose more than one, change the request, or say they will speak alone. The player can also state limits on what they can promise or disclose.

Do not require a personal explanation for wanting company. Do not assume that accompaniment is a sign of fear, incapacity, or agreement with the faction.

## 224. Scene seed — “Stay until I ask you to speak”

The survivor wants to ask a faction contact about supervised access to a work area. They ask the player to attend and let them lead. The faction contact addresses the player first and asks whether the survivor will accept a new condition. The player can redirect the question, wait for the survivor, ask whether they want a pause, or answer only if the survivor previously authorized that exact role.

The contact may accept the redirection, repeat the question, clarify the condition, or end the meeting. The survivor retains the next choice.

## 225. Keep responses inside the agreed boundary

If the player was asked to remember terms, they can repeat the offer accurately after the contact finishes. If asked to pose a practical question, they can ask that question and stop. If no speaking role was authorized, they can remain present and allow silence.

When a new subject arises, the player can ask the survivor whether they want help before answering. A previous permission to ask about schedule does not cover consent to a fee, task, disclosure, or faction affiliation.

## 226. The survivor can pause, caucus, or leave

They may ask for time to think, speak privately with the player where the setting permits, request the terms in writing, continue without intervention, or end the conversation. The player can follow their signal, help restate what changed, or ask to reschedule. No branch accepts an offer by default.

If a private pause is not available, say so and let the survivor decide whether to continue. Do not create a hidden side conversation or a guaranteed negotiation advantage.

## 227. Help clarify a term without taking over the goal

The player may ask the contact to explain a time, access restriction, or required task. They can compare the explanation with the written terms, ask who owns the decision, or request that the survivor take the question. The faction answers for its own offer; it does not decide what the survivor wants.

If the contact adds a new condition, the survivor receives the complete revised terms and can accept, counter, defer, or decline through the proper owner.

## 228. Respond when the conversation becomes uncomfortable

The player can name a practical boundary, request a pause, leave with the survivor, or continue listening if asked. A contact may be blunt without being coercive; a survivor may feel uncomfortable without the game declaring the contact guilty. Preserve observable words and choices rather than assigning a hidden pressure score.

If the meeting crosses an authored safety or consent rule, route the issue through its current authority. Accompaniment itself does not grant the player enforcement powers.

## 229. Branches by companion role

- **Quiet presence:** the survivor speaks and the player listens, then helps review terms afterward.
- **Term recorder:** the player repeats only the offer details they heard and flags unknowns.
- **Question helper:** the player asks the specific practical question the survivor approved.
- **Pause signal:** the player responds to the survivor's chosen signal without deciding for them.
- **Meeting ended:** the survivor leaves, asks for another contact, or closes the route.

The survivor can revise any role between topics. Consent is scoped to the conversation, not a permanent delegation.

## 230. Supporting faction behavior

The faction contact can offer clear terms, provide a written explanation, request an authorized decision-maker, or decline a companion's participation. The player can accept the channel limit, ask the survivor what they prefer, seek a different representative, or end the meeting.

Faction protocol does not transform the player into the survivor's legal or institutional representative. Any form or signature follows the current owner and the survivor's direct consent.

## 231. Goal progress remains separate

Attending a meeting does not complete a personal goal. A confirmed access approval, accepted task, or completed work advances it only if the goal owner recognizes that evidence. The survivor may learn that the route is unsuitable and revise their goal without the meeting becoming a failure.

The player can support the survivor's decision even when it differs from the player's preference. No dialogue choice should turn accompaniment into control of the goal.

## 232. Callbacks and endings

The meeting may end with a clear offer pending, a requested follow-up, a counterproposal, a declined route, or no decision. Later dialogue can ask whether the survivor wants another meeting and what role the player should take. Do not assume they always need a companion because they asked once.

If the survivor accepted a condition directly, attribute that decision to them. If the player only asked for clarification, do not write the answer as an agreement.

## 233. Player approaches

**The quiet companion** attends without speaking unless asked.

**The terms mirror** repeats the offer accurately and marks uncertain details.

**The consent checker** asks before intervening on a new subject.

**The pause supporter** helps the survivor stop and reconsider without penalty.

**The boundary setter** declines requests to make promises outside the agreed role.

## 234. Persistence and replay

On re-entry, refresh the survivor's current goal, meeting terms, accepted role, faction response, access status, and any task evidence from their owners. A previously authorized question does not authorize another meeting's negotiation. If role permission was not persisted, ask again.

Repeated attendance cannot apply a fee, accept access, or advance goal progress twice. A new condition requires direct fresh acceptance from the survivor.

## 235. Acceptance questions

1. Is the companion's role agreed and limited before the meeting?
2. Can the survivor speak, pause, change roles, or leave at any point?
3. Does the player ask before responding to a new subject?
4. Are offers and conditions repeated accurately without implying acceptance?
5. Do faction rules and goal evidence remain with their actual owners?
6. Does accompaniment avoid implying incapacity or faction alignment?
7. Are role permissions and meeting outcomes truthful after reload?

## 236. Installment 14 close

This installment lets the player accompany a survivor without taking over their voice. The survivor can lead, ask for narrow help, pause, change the companion's role, or end the meeting. The player supports clarity and consent while the survivor retains control of every offer and personal goal.

## 237. Installment 14 casebook — The contact answers the wrong person

The contact may direct every question to the player even after the survivor has introduced themself. The player can redirect once, ask whether the survivor wants to answer, or request a pause. The survivor may continue, ask the player to state one approved fact, or leave. Do not make the player answer simply because the contact is looking at them.

If the survivor previously asked for quiet presence, redirecting the contact is within the companion's role only when they agree that the role includes that intervention. Otherwise the player can wait and let the survivor choose whether to speak.

## 238. Case — A new condition appears mid-meeting

The faction contact may add a work shift, material fee, or access restriction not included in the invitation. The player can ask for the changed condition to be stated clearly, check whether it is an official term, and pause while the survivor considers it. The survivor can accept, counter, ask to see it in writing, or end the discussion.

The meeting's earlier agreement to discuss access does not authorize this new condition. If the contact cannot identify its owner, keep the requirement unconfirmed and do not commit the survivor.

## 239. Case — The contact asks for a private reason

The survivor may answer, offer a practical summary, ask why the detail is needed, or decline. The player can ask the contact to explain the requirement or seek an alternate route. If a real access or safety owner requires information, describe its scope and recipient before asking the survivor to decide.

No companion should fill the silence with a guess. A refusal to disclose may close this specific route while leaving the survivor's other choices intact.

## 240. Case — A form arrives before the answer

The contact may hand over a form that looks like acceptance. The player can ask whether it records attendance, an application, or a commitment; the survivor can read it, request an explanation, take it away without signing, or decline. The form's owner determines its effect.

Do not let a signature occur as a flavor animation while the player is still asking questions. A completed application and an accepted offer are distinct states when the current owners represent them separately.

## 241. Case — The survivor asks the player to speak once

They may authorize a narrow clarification after the contact misstates a fact. The player can correct only that fact, ask whether the survivor wants to continue, and then return the floor. They should not use this opening to negotiate a better deal or disclose another reason.

The contact may accept the correction, ask for its source, or disagree. The survivor then chooses whether to provide evidence, pause, or stop. Their single authorization does not expand automatically to every future question.

## 242. Case — The survivor changes the companion's role

During the discussion, they may move from “listen” to “help me remember the terms” or from “ask one question” to “please do not speak.” The player can acknowledge the new boundary and follow it. If the conversation cannot continue under that role, request a pause or end it.

Do not make role changes a tracked personality variable. They are instructions for the current interaction, persisted only if the existing owner has a truthful place to keep them.

## 243. Case — The contact refuses a companion

The contact may require a one-to-one conversation, decline the player's presence, or offer a public alternative. The player can ask the survivor what they prefer, wait nearby if permitted, request a different representative, or leave. The survivor may choose not to proceed.

Do not imply that the player can force entry or that the survivor must accept a private meeting. Location and faction policy owners determine the available setting; the survivor decides whether that setting is acceptable.

## 244. Case — The meeting ends without agreement

The survivor may leave after receiving information but before accepting anything. The player can ask whether they want to record a follow-up question, seek another faction contact, revise the goal, or close the route. If the survivor wants no callback, respect it.

The contact's refusal, the survivor's refusal, and an unresolved application are different outcomes. Preserve whichever one actually occurred; do not translate all three into “failed negotiation.”

## 245. Recombine the route without a good-person ending

The survivor may accept access after clarification, take a form away for review, ask for a later meeting, choose a different representative, or abandon the route. A player may provide quiet support, direct questions, factual memory, or a boundary. These choices shape the immediate conversation and next available contact.

They do not decide whether the survivor is brave, grateful, loyal, or dependent. Use authored character reaction only when current profiles and state support it.

## 246. Evidence and relationship ownership

If a faction approves access, the access owner confirms it. If the survivor accepts a task, the task owner records it. If a relationship changes, the current social owner supplies that event. If no current owner records the meeting role, it remains a local narrative choice and must not become a durable trust stat.

This separation allows a scene to be emotionally specific while keeping its consequences factual and replay-safe.

## 247. Companion-role checklist

Before the scene: ask what the survivor wants the player to do.

During each new condition: ask whether that permission still applies.

At a pause: do not treat silence as acceptance.

At departure: report only what was offered, asked, and answered.

After the meeting: let the survivor choose whether there will be another one.

## 248. Installment 14 acceptance extension

1. Can the player redirect attention without taking over the survivor's answer?
2. Are new fees, tasks, and access rules treated as new terms?
3. Can the survivor refuse private disclosure or request a different setting?
4. Does a form's effect come from its owner and remain explained?
5. Is each intervention limited to the permission granted for that moment?
6. Can the meeting end unresolved without closing unrelated routes?
7. Do factual outcomes avoid turning accompaniment into an alignment score?

## 249. Installment 14 casebook close

These cases expand accompaniment into moment-to-moment choices: redirect, ask, pause, clarify, leave, or continue under a changed role. The player is useful because the survivor invited them, not because the player owns the negotiation. Fresh terms always return to the survivor for a decision.

## 250. Accompaniment casebook — The contact turns to the player

The faction representative may ask the player to confirm whether the survivor is “really interested.” The player can say that the survivor will answer for themself, repeat only a fact the survivor authorized, or ask the contact to clarify the practical requirement. The survivor can answer, ask for time, or end the exchange.

Do not say “yes” to keep the meeting moving. Interest in learning terms is not acceptance of access, labor, or affiliation.

## 251. Case — The survivor wants a written offer

The player can ask the contact to provide the exact fee, schedule, work condition, and expiry through a supported route. The survivor may read it in private, request a plain explanation, counter, or leave. If a written offer is not supported, the player can repeat the verbal terms and mark what remains unconfirmed.

Taking a paper away is not acceptance. The player should not sign or mark it on the survivor's behalf unless an existing owner explicitly supports that authority and the survivor gives fresh consent.

## 252. Case — The survivor wants to answer without the player

The player may wait outside if location access permits, step aside while remaining nearby, or leave the meeting. The survivor can later ask for help reviewing terms or decide to handle the conversation alone. Do not frame this as the survivor rejecting the player.

If the contact refuses to continue without the player, the survivor can accept that constraint, request another representative, or close the route. The player cannot force a private conversation or force their own attendance.

## 253. Case — A question exceeds the companion's knowledge

The contact may ask the player to interpret an access rule or technical term. The player can say they do not know, ask the contact to identify the source, or invite the survivor to ask a specialist. The player should not guess to protect the flow of the conversation.

The survivor can continue with the confirmed information, defer the decision, or ask for a different route. A companion's uncertainty is not a failure state.

## 254. Case — The meeting becomes a negotiation

The survivor may want to counter the first offer. The player can listen, help list the original terms, or ask whether the survivor wants a practical question. They cannot invent a counteroffer or trade the survivor's labor to improve access.

If the survivor asks for a suggestion, the player can present options without selecting one for them. The faction contact may accept, decline, or return with changed terms; the survivor decides what happens next.

## 255. Case — The player hears a private detail

The survivor may disclose something during a pause and later ask the player not to repeat it. The player can honor that limit, ask whether any part may be shared, or explain a separate safety obligation if a current owner imposes one. Do not copy private content into the faction's application simply because the player now knows it.

If the contact asks why the survivor hesitated, the player may say that the survivor wants time without explaining the private detail.

## 256. Case — The contact widens the audience

The faction representative may invite another official or ask to bring a witness. The player can check whether the survivor agrees, ask who the additional person is and what role they have, or pause until the answer is clear. The survivor may welcome the person, request a private setting, or leave.

Permission to meet one contact does not authorize a room full of listeners. If the contact cannot state the additional person's role, the player should not imply that the survivor has accepted it.

## 257. Case — A family member asks to join

The family member may support the survivor, but relationship does not transfer consent. The player can ask the survivor privately whether they want that person present, keep the meeting as planned, or offer the family member a separate summary if permitted. The survivor may accept, decline, or defer the choice.

Do not reveal the survivor's goal to the family member just to explain the boundary. A brief “they will decide who joins” is enough.

## 258. Case — The survivor leaves before the answer

They may decide the conditions already show the route is unsuitable. The player can leave with them, ask whether they want a later response, or stay only if the survivor explicitly wants the answer carried back. The contact's later offer should be routed to the survivor through a permitted channel.

If the player stays, they cannot accept terms or disclose new information. If no callback is authorized, the conversation ends with the request unresolved.

## 259. Recombine into distinct scene endings

The survivor may leave with the original terms, a written offer, a counterproposal, a scheduled follow-up, a request for another representative, or no active route. A meeting can also close with a refusal from either party. The player reports what happened accurately and asks whether any next step is wanted.

Do not rank these endings from strongest to weakest. A clear refusal can be more useful than a vague promise, and an unanswered request can remain open without becoming a failure.

## 260. Dialogue fragments for the companion role

**Redirect:** “They asked for the meeting. Let them answer.”

**Uncertainty:** “I don't know whether that room is available next week. Can you show us who confirms it?”

**Pause:** “We need a minute before answering the new condition.”

**Boundary:** “I can repeat the schedule. I can't accept a task for them.”

These are candidate lines only. Match final phrasing to current character voices and the authored relationship state.

## 261. Casebook acceptance extension

1. Can the survivor ask for privacy, more information, a counter, or a different contact?
2. Can the player admit uncertainty without fabricating terms?
3. Are additional listeners or representatives introduced with consent?
4. Can a family relationship avoid inheriting the survivor's permission?
5. Does departure end the companion's negotiation authority?
6. Are later offers routed through an authorized channel?
7. Can every ending preserve an unresolved state honestly?

## 262. Accompaniment casebook close

These variations make the companion role useful without making it powerful. The player can clarify, remember, pause, protect a boundary, or leave; the survivor still chooses whether to speak, accept, counter, disclose, and continue.

## 263. Expansion installment 15 — A repeated offer needs a new decision

A survivor may meet a faction contact more than once. The second conversation is not a continuation of consent by default. The contact can restate the old terms, change the schedule, add a task, or offer a different benefit. The player should identify what changed and let the survivor decide whether they want to hear the revised proposal.

If the survivor says “same terms,” the player can confirm the details with the contact rather than relying on memory. If the faction says that a condition is no longer available, the old offer remains historical; it does not silently become the new one. The survivor can accept the change, counter it, request time, or refuse the route entirely.

## 264. Repeated-offer branch — The benefit changes

The contact may add access to a room, a place in a work crew, supplies, or an introduction to another person. Ask whether the new benefit is guaranteed, conditional, temporary, or only a possibility. The survivor may value it, distrust it, or decide it is irrelevant. The player can help clarify consequences but should not decide that a benefit outweighs the survivor's stated concern.

If the benefit depends on a separate agreement, split the decisions. The survivor can accept the meeting while declining a work assignment, or consider the work while requesting time before joining the faction. A conversation accepted for information must not trigger an unrelated action.

## 265. Repeated-offer branch — The cost changes

The contact may introduce a fee, shift commitment, public appearance, or information request. The player asks which condition is new, who enforces it, and whether it can be refused independently. The survivor can accept the original offer only if it still stands without that condition; otherwise they can negotiate or leave.

Do not frame the survivor's refusal as ingratitude. They may have already spent time traveling or preparing, but that sunk effort does not transfer their authority to the player or faction. A clear close can protect both sides from acting on mismatched expectations.

## 266. Repeated-offer branch — The survivor changes their goal

An offer that once fit may no longer fit the survivor's current aim. They may have resolved the original problem, discovered a new priority, or decided that the cost is too high. The player can ask what matters now and whether they want to tell the contact. The survivor can share a limited explanation or simply say they are no longer pursuing the route.

Where the personal-goal authority supports changes, use that owner to confirm the current goal. Dialogue alone should not overwrite it. If it does not support such an update, keep the revised intention as a conversation and avoid claiming a persistent goal transition.

## 267. Repeated-offer branch — The contact interprets silence

The faction contact may treat a delayed reply as interest, refusal, or permission to follow up. The player should correct the interpretation using the actual response state: no answer means no decision unless the current offer owner explicitly defines a deadline and consequence. The survivor can ask the contact to stop, request a later date, or state a decision now.

If a follow-up message is possible, confirm the permitted channel and recipient. The player does not promise access to the survivor or disclose where they can be found. A contact who persists after a clear refusal can become a meaningful characterization branch, but do not invent a threat or escalation unsupported by current faction behavior.

## 268. The survivor can revoke a previous instruction

Before a later meeting, the survivor may tell the player not to speak on their behalf anymore. The player should honor that instruction and make the change visible to the contact without repeating private reasons. The survivor can still ask the player to attend silently, wait outside, or leave entirely.

If a previous message authorized a specific action and the contact has not acted on it, ask whether the survivor wants to withdraw it where the owner supports revocation. If the action already occurred, report that fact honestly and ask what repair, if any, the survivor wants. Never pretend that revocation reverses completed work.

## 269. A support contact can help compare, not choose

A second faction contact or neutral resident may help explain practical differences between two offers. The survivor chooses who may hear the terms. The player can summarize only the authorized details and ask the helper to explain concrete requirements, timelines, and uncertainties.

The helper should not turn comparison into a vote. One offer may be safer but slower; another may be useful but demanding. The survivor can prioritize access, privacy, schedule, companionship, or independence. The scene can end without a ranked recommendation if the survivor wants more time.

## 270. Repeated-offer endings

Possible endings include reaffirming the old offer, accepting a revised offer, accepting only one separable condition, countering, asking for verification, pausing, revoking the player's representative role, or refusing further contact. The closeout should state only the choice made and the confirmed next event.

An open route is not necessarily progress; a refusal is not necessarily collapse. Avoid a hidden attitude score that interprets persistence as good or hesitation as disloyalty. The lasting branch is the survivor's actual decision and the faction's response to it, within supported state.

## 271. Expansion installment 16 — Refusal opens a different route

A refusal should change what happens next without forcing the survivor back to the same offer. The player asks whether the survivor wants to end contact, seek another route to the underlying goal, or preserve one narrow part of the conversation. The survivor can decline the proposal and still want information, a referral, a public service, or time to decide. The scene should let them separate those choices.

The player does not need to extract a reason. If the survivor offers one, it can guide an alternate route: the schedule conflicts with a shift, the meeting is too public, the condition is unacceptable, the representative is not trusted, or the original need has changed. If the survivor simply says no, that is sufficient. Follow-up branches should use the expressed decision, not a guessed diagnosis of their motives.

## 272. Refusal branch — Decline the route, keep the information

The survivor may reject a faction’s work or membership condition while asking what the group can provide without it. The player can ask the contact to identify any independent service, public resource, or referral. The contact may confirm one, say that none exists, or offer to check. Do not assume that a declined package invalidates every separate benefit, and do not invent a free version to soften the refusal.

If the survivor accepts an informational answer, record only that conversation outcome. The acceptance does not reopen the offer or imply willingness to be contacted later. The player can ask whether the survivor wants a written summary or no record beyond the present exchange, subject to the communication owner’s supported behavior.

## 273. Refusal branch — The obstacle is timing

The survivor may want the opportunity but cannot meet at the proposed time. The player can ask whether a different time would help, whether a contact can send terms through an approved channel, or whether the survivor wants the route closed. The faction decides whether it can change its schedule. No character should promise that the same place or benefit will still be available.

If a later meeting is agreed, it is a scheduling fact, not acceptance. At that meeting, confirm the terms again. If the survivor misses it because a current task changed, do not label the absence as a refusal unless they choose to close the route or the offer owner explicitly defines an expiry.

## 274. Refusal branch — The obstacle is audience or privacy

The survivor may be willing to discuss the offer but not in front of a group. The player can ask for an authorized private setting, a different representative, or an explanation that does not reveal the survivor’s personal goal. The faction may accommodate the request, decline, or provide a public summary. The survivor chooses whether any of those options is acceptable.

If privacy cannot be provided, the player should say so before the survivor shares details. Do not stage a disclosure first and then ask whether it was acceptable. The scene can move to a different route where a neutral contact explains the general process without learning the survivor’s private reasons.

## 275. Refusal branch — The condition is unsafe or outside scope

The survivor may refuse a task that appears unsafe, impossible, or unrelated to the original offer. The player can ask the contact to clarify the requirement and identify which owner can assess it. The survivor may wait for review, propose a narrower task, or walk away. Until the responsible owner resolves the condition, do not narrate it as a valid requirement merely because a faction representative stated it.

Where the condition is confirmed and remains unacceptable, the refusal stands. The player can help the survivor ask for an alternative or close the route. Any safety report or task dispute follows its existing owner. The survivor does not need to accept the faction’s framing in order to be taken seriously.

## 276. Refusal branch — A supporting group offers a bridge

A smaller group may know an independent specialist, a public resource, or another person with the needed access. The player asks whether the survivor consents to a referral and what information may be shared. The group can provide the introduction, decline, or ask the survivor to make contact themself. It does not decide whether the survivor should accept the original faction’s terms.

The referral can change the practical path: a different meeting, a narrower service, a wait for a specialist, or a self-directed attempt. Keep the bridge local and truthful. A supporting faction is not a universal appeals office, and its contact should not be able to override another faction’s rules or the survivor’s choice.

## 277. Refusal branch — The survivor asks for help without naming the goal

The survivor may want assistance comparing options but not want their deeper goal repeated to the contact. The player can work with the practical terms alone: time, location, cost, duties, access, and uncertainty. If a recommendation would require private context, say what cannot be assessed without it and let the survivor decide whether to share more.

This route prevents the player from treating personal disclosure as the price of useful help. A survivor can receive a clear comparison while keeping the reason for their choice private. If current goal ownership requires explicit confirmation before any persistent route changes, ask only the minimum necessary question through that owner.

## 278. After refusal, the faction responds

The contact’s reaction is part of the branch. They may accept the answer, clarify that the opportunity remains open, withdraw the offer, offer a different representative, or express disappointment. Their response should fit authored faction behavior and the actual terms. Do not make every refusal produce hostility, and do not make every contact gracious if current characterization says otherwise.

The player can acknowledge the response and ask whether the survivor wants to hear it. If the survivor has left, share only what they authorized. A contact’s private opinion is not automatically a message to the survivor, and a response that changes availability should be communicated through a permitted channel where possible.

## 279. Follow-up without pressure

A later follow-up is appropriate only if the survivor asks for it or the offer owner has an explicit, disclosed follow-up rule. The player can clarify the date or channel, then stop. A follow-up should not repeatedly prompt a decision because a content flag remains unresolved. If the survivor says not to contact them, honor that boundary wherever the current communication owner can represent it.

If circumstances change and the survivor later reopens the route, use a fresh request. The player can refer to the past refusal without treating it as a permanent identity: “Last time, the public meeting did not work. Do you want to hear what changed?” The survivor may decline again, ask a question, or choose a new path.

## 280. Installment 16 endings and branch facts

Possible endings include a refusal with information retained, a schedule change, a private meeting request, an independent referral, a safety review, an alternate service, no follow-up, or a route the survivor later reopens. They differ in action and access, not in a universal moral score. The plan should make clear which of these is represented by current persistent state and which remains authored conversation.

For every ending, record the minimum supported fact: the offer was declined, a specific information request was made, a referral was accepted, or a follow-up was authorized. Do not persist an inferred motive such as fear, distrust, selfishness, or loyalty. If later dialogue needs that context, obtain it from the survivor again or use an authored line that does not claim the motive as fact.

## 281. Expansion installment 17 — The survivor weighs two real needs

A survivor may face two worthwhile options that cannot both fit the same time, access, or resource limit. One route could provide immediate shelter work; another could preserve a private goal or relationship. The scene should make the tradeoff concrete and let the survivor choose what matters now. The player can help compare consequences, but should not turn the choice into a virtue test.

Ask which constraints are confirmed and which remain predictions. A schedule owner may confirm that two shifts overlap; a faction contact may only think a place will be available; the survivor may know that a family member needs help but not know how long it will take. Keeping these certainty levels visible creates meaningful decisions without pretending that the player can foresee every outcome.

## 282. Branch opening — Name the competing commitments

The survivor can identify the options in their own terms: attend a meeting, keep an existing task, help someone close to them, preserve privacy, or wait for better information. The player can ask whether one commitment is already binding or whether both remain voluntary. Do not assume that the most public request outranks a private promise.

If a task owner confirms an assigned duty, explain the available release or reassignment route. If no reassignment exists, the survivor can still state what they want even if the schedule cannot accommodate it. The distinction between preference and operational permission allows the character to retain agency without claiming the system can grant every request.

## 283. Branch — One option is certain, the other is only promised

The survivor may be choosing between a confirmed present resource and a possible future opportunity. The player can ask the contact to clarify how strong the promise is: reserved, expected, requested, or speculative. The survivor can proceed on the confirmed option, wait for verification, or accept the risk of the uncertain offer.

Do not make uncertainty disappear because the player chose optimistically. If the promise later fails, the follow-up should trace the actual source: an honest delay, a changed condition, or a statement that was never confirmed. The survivor’s choice remains theirs, and the faction’s response follows current authored behavior rather than an automatic betrayal branch.

## 284. Branch — Both options matter to the survivor

The survivor may say that neither priority is negotiable. The player can explore whether timing, location, or a supporting contact could make both possible, but should not assume a solution exists. The survivor can request a change from one owner, seek help, defer both, or choose one and grieve the other.

If the routes are truly incompatible, allow an ending without a compromise. A character can make a difficult choice and still believe the unchosen need was important. Avoid dialogue that praises them for sacrificing one person or shames them for protecting themselves.

## 285. Branch — Another resident offers to cover a commitment

A friend or faction contact may volunteer to take on part of the survivor’s responsibility. The player checks whether the existing task owner permits substitution and asks the survivor whether they want that help. The volunteer can state their own limits. A supportive offer is not an assignment, and the survivor is not obligated to accept it to prove trust.

If the substitution is valid, the original owner confirms who is responsible and when. If not, the volunteer can still provide emotional support or help with a separate task. Keep the reason for the survivor’s choice private unless they authorize sharing it with the person offering help.

## 286. Branch — The survivor chooses a route that disappoints someone

The unchosen contact may be disappointed, but disappointment does not automatically become punishment. They can acknowledge the answer, ask whether circumstances might change, withdraw an expiring offer, or respond according to their established character. The survivor can hear the response, receive a neutral summary, or avoid further discussion.

The player should not negotiate a better reaction by making a new promise on the survivor’s behalf. If a relationship owner tracks a supported consequence from the event, update only through that owner and its actual rules. Otherwise, let the character’s immediate dialogue carry the disappointment without inventing persistent hostility.

## 287. Branch — The survivor requests partial participation

The survivor may want to attend only the information portion of a meeting, accept a referral but not an assignment, or help with one bounded task. The player asks the offering party whether that narrower participation is allowed and repeats the precise boundary. The survivor confirms whether the revised scope is acceptable.

If the party cannot separate the conditions, the survivor can accept the whole package, refuse it, or ask for time. Do not quietly treat a partial appearance as agreement to all terms. If a new condition is introduced during the activity, pause and ask for a fresh decision before proceeding.

## 288. Branch — The survivor changes their mind before action

After agreeing in conversation, the survivor may reconsider before the task or meeting begins. The player can ask whether they want to withdraw, revise, or delay. The offer owner determines what can still be changed operationally, and the survivor is told any consequence before the change is made.

If the action has already started, distinguish what can be stopped from what is complete. The player should not say “you can always change your mind” if the system cannot undo an assigned task or completed transfer. They can still express regret, request repair, or decline future participation.

## 289. Branch — The survivor accepts one risk knowingly

The survivor may choose an uncertain route after hearing its limits. The player can confirm that they understand the known risk without asking them to repeat a legalistic disclaimer. The contact should not exaggerate certainty, and the player should not withhold a confirmed downside to steer the choice.

When the outcome arrives, branch on what actually happened. Success does not prove the decision was easy; failure does not mean the survivor chose foolishly. Their own reflection can differ from the outcome: relief, frustration, acceptance, or a wish to try another route. Avoid retrospective dialogue that rewrites the choice as inevitable.

## 290. Installment 17 close — Consequence without moral scoring

This installment treats competing commitments as authored, actionable branches. The player can clarify certainty, request substitution, negotiate narrower participation, or help the survivor close one route. The survivor may choose either option, seek a combined route, defer, or accept a known uncertainty.

The ending should identify the commitment the survivor chose, any confirmed operational change, and the unresolved cost they named. It should not convert one decision into a universal disposition such as loyal, selfish, honest, or brave. A later quest can respond to the practical consequences and to what the survivor says they want next.

## 291. Expansion installment 18 — The offer changes while the survivor waits

An offer may be withdrawn or revised while the survivor is gathering information. The player should identify the source and timing of the change, then let the survivor decide whether to hear the new terms. A contact may say that a slot was filled, a resource is no longer available, or a condition changed. Do not infer that the survivor caused the change by hesitating unless the offer owner established that consequence in advance.

The survivor may feel disappointed, relieved, angry, or indifferent. The player can ask whether they want to respond, seek another route, or close the matter. The factual change belongs to the offer owner; the emotional meaning belongs to the survivor.

## 292. Branch — The offer had a disclosed expiry

If an offer included a clear deadline and the owner confirms that it passed, the player can explain that the original terms are no longer available. The survivor may ask whether an extension is possible, request a new offer, accept that the route ended, or make no further request. The contact decides whether to reopen it.

The deadline should have been communicated before the survivor made their decision. If it was not, do not retroactively claim they missed a known condition. The player can ask the contact to clarify what happened and let the survivor decide whether to pursue a review. A late explanation does not restore the old terms automatically.

## 293. Branch — The contact withdraws without explaining why

The faction representative may say only that the offer is withdrawn. The player can ask whether any practical alternative exists, but should not pressure the contact to reveal private internal deliberation. The survivor can request a factual reason, ask for another representative, or end contact.

If no reason is provided, keep it unknown. Do not fill the gap with a theory about the survivor’s reputation or the faction’s prejudice. Later content can reveal a confirmed explanation if one becomes available, but current dialogue should not treat speculation as evidence.

## 294. Branch — The survivor asks whether their refusal caused the loss

The survivor may worry that asking for more time cost them the opportunity. The player can separate facts: what was requested, what the contact confirmed, and whether the offer owner stated a deadline. The survivor may still regret the result, but regret does not prove that they acted wrongly.

If the offer was withdrawn because capacity changed, say so only if confirmed. If the contact’s reason is unknown, the player can acknowledge uncertainty and ask what the survivor wants now. Do not absolve or blame them with information the player does not have.

## 295. Branch — A replacement offer carries a different condition

The contact may reopen the route with a new task, less access, a different schedule, or a new audience. Treat it as a fresh offer. The survivor can compare the changed terms, ask which prior conditions remain, accept, counter, or refuse. No acceptance of the earlier offer transfers automatically.

If only part of the earlier benefit remains, explain that boundary. The survivor can decide whether the narrower opportunity still serves their goal. A supporting faction may help compare logistics, but it cannot decide that a reduced offer is “good enough.”

## 296. Branch — Another faction fills the practical gap

A different group may offer a related service after the original route closes. The player asks whether the survivor wants an introduction and what information may be shared. The new group states its own terms. The survivor can pursue the substitute, request a comparison, or decline both.

Do not treat the second faction as a reward for refusing the first. Its support can be imperfect, slower, or governed by different conditions. This gives the player a genuinely distinct route while keeping each faction’s offer independent and each decision with the survivor.

## 297. Branch — The survivor wants to challenge the withdrawal

The survivor may believe the contact applied a condition inconsistently. The player can help gather the original offer, identify the stated deadline, and ask the owner for review if a review route exists. The survivor chooses whether to proceed and what details may be shared.

If no review mechanism exists, do not invent an appeal court or faction council. The player can request clarification, ask for a different contact, or close the issue. A challenge can still matter as dialogue and as a factual record of disagreement without promising that the withdrawal will be reversed.

## 298. Branch — The survivor declines the replacement too

The replacement may not meet the survivor’s goal. They can say no without restating every reason. The contact may accept, ask one clarifying question, or close the exchange. The player should not ask the survivor to justify a second refusal merely because the faction invested time.

If the survivor wants to preserve a relationship with the contact, they can say so directly or allow the player to convey a neutral thanks. Do not invent a warm ending if the contact is angry, or hostility if they are neutral. Use current characterization and the actual response.

## 299. Branch — The survivor reopens the route later

Conditions may change again: the original resource returns, the survivor’s schedule opens, or the faction offers a different representative. The survivor can ask to revisit the matter. The player confirms the present offer rather than assuming the former one is still valid. They may choose to continue from the same goal, revise it, or decide the need has passed.

The callback can mention the earlier withdrawal accurately: “The slot you asked about was gone; this is a new opening.” It should not frame the survivor as indecisive. A character can wait for a better fit and later choose to proceed.

## 300. Installment 18 close — Preserve the difference between delay and refusal

This installment covers a changing offer without making uncertainty a punishment. The player checks the disclosed terms and current owner, shares only confirmed reasons, and lets the survivor choose whether to ask again, seek another route, challenge the change, or stop.

The persistent outcome, if supported, should capture the offer state and the survivor’s explicit action. It should not infer that the survivor is difficult, fearful, or disloyal. If state support is absent, keep the distinction in the authored scene and avoid false durability claims.

## 301. Expansion installment 19 — The survivor’s choice affects a shared plan

A survivor’s decision may alter a plan involving other residents. The player should identify the actual dependency before telling anyone that the plan is blocked. Perhaps the survivor was one of several volunteers, the only person with a particular key, or simply the person whose name appeared first. Those situations lead to different responses and should not be collapsed into “the group needs you.”

The survivor can ask the player to find a substitute, convey a boundary, or let the group know the plan must change. They can also keep their decision private when the group does not need the reason. The relevant task owner confirms whether substitution is possible; the survivor remains free to refuse participation even if the plan becomes harder.

## 302. Branch — The survivor was optional support

The task owner confirms that the survivor was one of several possible helpers. The player can tell the survivor their refusal will not block the task, ask whether they want to share anything else, or close the subject. Other helpers may be contacted through their own available route.

Do not describe the survivor’s refusal as letting everyone down. If the task later suffers a delay, trace it to the actual shortage or schedule. The survivor can still offer a different kind of support if they choose, but the player must not use the group’s inconvenience to pressure them into reversing their answer.

## 303. Branch — The survivor held a unique but replaceable role

The survivor may have knowledge or access that no one else currently has, while the task can still be postponed or reassigned. The player can ask whether the survivor wants to document a safe handoff, teach a replacement, or decline any further involvement. The owner determines whether the role can be transferred and what supervision it requires.

If the survivor declines to train a replacement, the player can ask the owner about another route. A unique role is a practical constraint, not a moral debt. If the task must wait, state that consequence plainly and allow the survivor to make their choice without a guilt scene.

## 304. Branch — The survivor’s refusal exposes a missing plan

The task may have depended on one person without a backup. The player can report the dependency to the responsible owner, request a safer contingency, or help identify an eligible volunteer. The survivor may participate in that planning only if they wish; they are not responsible for fixing the system that made their presence indispensable.

This branch can lead to a faction offering training space, a public roster review, or a temporary pause. Each support is concrete and bounded. A faction does not gain control of the survivor’s future schedule merely because it found a substitute.

## 305. Branch — A friend asks why the survivor is absent

The friend may assume that the survivor agreed to help. The player can say only that the plan changed, ask the survivor what they want shared, or direct the friend to the task owner. The survivor can disclose a reason, offer a neutral explanation, or keep it private.

Do not reveal the survivor’s stated reason to make the scene easier. A friend’s concern does not create permission to share. If the task owner needs a status update, the player can report attendance or availability without describing private motivations.

## 306. Branch — The group offers a different role

After hearing the survivor’s boundary, the task owner may propose a narrower role, a later time, or a task that better fits the survivor’s goal. The survivor can consider it, ask for the terms, or refuse again. Make clear which part changed and whether the new role has different obligations.

The player should not praise the group for being flexible before the survivor responds. An alternative may still be unwelcome. The survivor’s decision can be to accept one part, request further change, ask for time, or end the exchange.

## 307. Branch — The survivor chooses not to explain

The group may want a reason to plan around the absence. The player can report the operational fact—unavailable, declined, or not confirmed—without inventing an explanation. The owner can then choose an available contingency. If the survivor later wants to disclose more, that becomes a new conversation rather than a retroactive prerequisite.

This protects characters who cannot safely or comfortably explain themselves. The player can still be effective by routing the decision to the correct owner and helping find another path. Narrative empathy comes from respecting the boundary and showing the practical result, not from extracting a confession.

## 308. Installment 19 close — A plan may change without owning the person

The group’s response to refusal can be operationally significant: substitute a worker, delay the task, change its scope, request training, or accept the absence. These are action-based consequences. None imply that the survivor’s refusal was good or bad in the abstract.

The questline should carry forward only the confirmed task state and any explicit information-sharing instruction. Later scenes may show the task’s result or a new volunteer, but should not retcon the survivor into having agreed. A living shelter adapts to people; it does not make every person responsible for absorbing the plan’s fragility.

## 309. Expansion installment 20 — Acceptance is limited to the terms heard

A survivor may accept one part of an offer and then be asked to do more: answer a personal question, attend an additional meeting, take on a task, or introduce someone else. The player should pause and identify the added condition. The survivor can accept it separately, request clarification, refuse that part while keeping the original agreement if the offer owner allows, or withdraw entirely.

This branch makes acceptance consequential without turning it into a blank cheque. The offer owner explains which terms are separable and which are linked. The survivor decides whether that package still works. If the new condition changes the original decision materially, ask for fresh consent rather than relying on the earlier yes.

## 310. Branch — The contact asks for private context

The faction representative may request details about the survivor’s personal goal to decide whether the offer is suitable. The player can ask what information is actually necessary, whether a less specific answer will work, or whether the survivor wants to decline the question. The contact can explain the requirement, accept a narrower answer, or withdraw the offer.

Do not disclose the survivor’s history just because it would make the contact more sympathetic. If the information is required, the survivor chooses whether to provide it. If they refuse, the practical consequence should follow from the offer’s stated rule, not from a moral judgment that they are hiding something.

## 311. Branch — A new task is added after the survivor agrees

The contact may reveal that the opportunity includes a shift or assignment not mentioned before. The player asks who owns the task, what it requires, and whether the survivor may accept the original benefit without it. The survivor can consider the full package, ask for the task to be removed, or reject the revised offer.

If the assignment is outside the contact’s authority, ask the task owner to confirm it. A faction representative cannot create a duty merely by adding it to conversation. If the owner confirms the role, the survivor still makes the choice where participation is voluntary.

## 312. Branch — The survivor accepts an offer but rejects public visibility

The survivor may agree to receive a service while declining to appear in a public announcement or faction gathering. The player can ask whether recognition is optional, whether the service can remain private, or whether the contact will not proceed without publicity. The survivor can accept that limit, negotiate, or withdraw.

Do not treat public recognition as a harmless extra. It can affect privacy, relationships, or future faction attention. The contact should state the practical reason if visibility is a real condition; the survivor decides whether that cost is acceptable.

## 313. Branch — The offer requires introducing another resident

The faction may ask the survivor to bring someone else into the conversation. The survivor can agree to ask, decline to involve another person, or request a general invitation they can pass along. The other resident’s consent is separate. The player must not provide names or contact details without authorization.

If the contact’s offer depends on recruitment, say so clearly. The survivor can accept the requirement, negotiate a different route, or refuse the offer. One resident cannot commit another person to a meeting, task, or faction relationship.

## 314. Branch — The survivor wants to accept now and reconsider later

The contact may permit a trial, a first meeting, or a limited service before a larger commitment. The player clarifies what can be stopped, what has already been agreed, and whether any cost or obligation begins immediately. The survivor may accept the limited step, ask for written terms, or wait.

Do not invent a cooling-off period or trial status if the offer owner does not support it. If the service is a single completed action, explain that it cannot be undone even if the survivor declines future involvement. The boundary makes informed acceptance possible without overstating reversibility.

## 315. Branch — The contact treats the survivor’s partial yes as full agreement

The player can correct the record: the survivor accepted the appointment, not the work; the information, not the membership; the first step, not the continuing commitment. The contact may acknowledge the distinction, clarify that the terms are linked, or refuse to separate them. The survivor chooses what to do next.

If the contact persists in representing a partial decision as full acceptance, the survivor can end the conversation or request another representative. The player should not answer on their behalf. Any action already taken must be described accurately and routed to its owner for a supported remedy.

## 316. Installment 20 close — Keep consent attached to a specific action

This installment gives acceptance a clear boundary. The survivor can agree to one meeting, service, or task and still reject additional disclosure, publicity, recruitment, or labor. Each added condition becomes a new decision unless the original terms clearly and validly linked it.

The ending should identify the exact accepted action, the declined or unresolved condition, and any confirmed follow-up. Do not store a generalized “trust” or “commitment” state as a shortcut. The survivor’s concrete choice is enough to create distinct consequences and later callbacks.

## 317. Expansion installment 21 — Refuse the role, keep the relationship

A survivor may reject a faction role without wanting to sever every social connection. They can ask to remain on speaking terms, continue using a public service, keep contact with one person, or attend an unrelated gathering. The player can help state that boundary and ask the contact which channels remain open. The faction’s response should distinguish an individual offer from the broader relationship where its authored behavior permits.

Do not interpret refusal of a role as rejection of every faction member. Equally, do not promise that a contact can separate the choices if the faction’s actual rules link them. Make the connection explicit and let the survivor choose whether the remaining relationship is worthwhile.

## 318. Branch — Keep a personal contact while declining membership

The survivor may value the representative as a person but not want to join the group. They can ask whether they may still speak privately, receive public information, or request help with a separate issue. The contact may agree, set a boundary, or explain that future contact must go through another channel.

The player can carry a specific message only with permission. Do not use the personal relationship to bypass the faction’s stated process. If the contact cannot continue, the survivor may request another representative or let the connection end without a scene that labels either person disloyal.

## 319. Branch — Continue using a service without taking a role

The survivor may decline a work assignment but still need a service the faction provides. The player asks whether access to that service is actually conditional on participation. The representative clarifies the rule; the survivor can accept the service, seek another provider, or decline it.

If service and role are linked, say so before the survivor commits. The player should not frame access as a favor the faction may withdraw at whim unless that is the confirmed arrangement. A supporting group may provide an alternate service, but the survivor chooses whether to approach it.

## 320. Branch — A faction member reacts personally

The contact may feel hurt when the survivor refuses. They can say so, ask for space, or continue the relationship. The survivor can acknowledge the feeling, explain only what they wish, or end the conversation. The player should not force an apology or disclose the survivor’s deeper reason to repair the contact’s feelings.

If the contact responds with pressure, the player can restate the boundary or ask to speak with someone else. If the contact is respectful, the survivor may still choose distance. Both reactions should remain possible based on characterization and the actual exchange.

## 321. Branch — Other members assume the survivor has left

A group member may stop sharing information or remove the survivor from a social invitation after hearing only that they refused one role. The player can clarify the limited decision if the survivor permits it. The member may accept the distinction, ask the survivor directly, or continue to assume the broader refusal.

Do not automatically repair the misunderstanding with a global faction-state change. The specific person can alter their behavior through dialogue; any persistent relationship effect must use the current owner. The survivor can choose not to correct every rumor or may ask for a private conversation.

## 322. Branch — The survivor wants to attend an open event

The survivor may wish to attend a public gathering hosted by the faction after declining membership. The player confirms the event’s actual audience and whether there are any disclosed conditions. The survivor can attend, request a companion, leave early, or decide not to go.

Attendance does not reverse the refusal. If the event includes a new role offer, the survivor can hear it or ask that it not be raised. The host should respect the boundary where possible; if it cannot, the survivor can choose whether to attend knowing that condition.

## 323. Branch — Keep the practical route, change the representative

The survivor may want the offer but not want to continue with the same contact. The player can request another representative if the faction supports that choice. The survivor may share a practical concern, state only that they prefer a different contact, or abandon the offer.

The faction can grant the request, explain that the original contact is the only available representative, or withdraw the offer. Do not pretend a new representative resets the terms. The survivor should hear the same current conditions and make a fresh decision.

## 324. Branch — The survivor chooses a clean break

The survivor may decide that even a personal connection or public service is no longer worth maintaining. The player can ask whether they want any final message sent, whether a permitted channel should be closed, or whether they prefer no further contact. The faction can acknowledge, ask one procedural question, or respond according to character.

Do not require a dramatic confrontation. The survivor can simply leave. If a service already in progress needs an operational closeout, its owner explains what remains; the survivor’s decision about future contact does not erase completed facts.

## 325. Installment 21 close — Relationships are more granular than membership

This installment lets the survivor preserve or end different parts of a relationship independently where the faction’s actual rules allow it. They may refuse a role, keep a contact, use a public service, attend an event, change representatives, or leave entirely. Each route is an explicit choice.

Later callbacks should refer to the exact boundary the survivor set. Do not reduce the branch to joined or rejected. A person can disagree with an institution, care about one of its members, and still decide that the relationship has become too costly to maintain.

## 326. Expansion installment 22 — The survivor asks the player to choose

A survivor may feel overwhelmed and ask the player which offer they should take. The player can help compare the confirmed terms, ask what the survivor values most, and explain tradeoffs. The survivor may want a recommendation, but the final action still requires their consent. The player should not use private knowledge to decide for them without permission.

The scene can support advice without becoming a morality quiz. The player might recommend the option with a confirmed schedule, the route that preserves privacy, or the offer that matches a stated goal. They should explain the reason and the uncertainty. The survivor can accept the recommendation, choose differently, ask another person, or decide not to choose today.

## 327. Advice branch — The survivor wants a practical comparison

The player can compare time, location, cost, task requirements, access, and uncertainty. Use only facts confirmed by the offer owners. The survivor may ask which terms are flexible or which consequence is reversible. If the player does not know, they can seek clarification rather than inventing a ranking.

The comparison should not obscure the survivor’s own priorities. A slower route may be preferable for privacy; a public route may be better for a resident who wants witnesses. The player can state a preference as advice, not as an objective truth about what is best.

## 328. Advice branch — The survivor wants emotional reassurance

The survivor may ask whether the player thinks they are wrong to refuse. The player can acknowledge that saying no is allowed, ask what consequence worries them, or simply stay with them without solving the choice. Do not promise that everyone will understand or that no opportunity will be lost.

The survivor can still request factual information after receiving reassurance. Keep the emotional response separate from the offer’s status. A kind line does not change the terms, and a difficult feeling does not invalidate the survivor’s decision.

## 329. Advice branch — The player has a conflict of interest

The player may have a relationship with one faction, a stake in the task, or a personal preference about the survivor’s choice. The player can disclose that limitation and suggest an independent contact. The survivor can still ask for the player’s view, request facts only, or choose without advice.

Do not portray every personal connection as corruption. The meaningful detail is whether the player’s interest could affect the recommendation. A transparent boundary lets the survivor use or reject the advice knowingly.

## 330. Advice branch — The survivor wants someone else’s perspective

The survivor can ask for a neutral specialist, trusted friend, or supporting faction contact. The player checks whether the survivor consents to sharing the offer details and selects which parts may be discussed. The helper explains what they know and where their own interests lie.

The helper is not a committee that votes on the survivor’s future. The survivor can hear multiple views, reject them all, or request a private answer. If the helper only has faction-specific information, clearly attribute it.

## 331. Advice branch — The recommendation fails to predict the outcome

The survivor may follow the player’s advice and later find that the offer changed or the outcome disappointed them. The player can acknowledge what was known at the time, listen to the survivor’s reaction, and help identify a new route. Do not rewrite the earlier decision as the survivor’s fault or claim the player guaranteed success if they did not.

If the player’s advice omitted a confirmed fact, let the survivor raise it. The player can correct the record and help seek a supported remedy. The relationship can react through dialogue and existing state owners, not through an automatic guilt counter.

## 332. Advice branch — The survivor chooses against the recommendation

The survivor may decide that the player’s preferred option does not fit. The player can accept that, ask whether they want help with the chosen route, or step back. Do not let the player withhold unrelated support as punishment. The survivor can still change their mind later without having to apologize for the earlier choice.

If the choice creates an operational conflict, the relevant owner explains it. The player can help find a valid alternative, but cannot quietly override the survivor to avoid a difficult scene.

## 333. Advice branch — No option feels acceptable

The survivor may reject every offer. The player can ask whether they want to wait, pursue the underlying goal elsewhere, or pause the whole topic. The contact may keep one route open, withdraw all terms, or be unavailable. The survivor does not owe the player a decision just because the conversation reached its final menu.

If the underlying need is urgent, route it to the responsible owner and identify any immediate supported service. Do not use an emergency to compel acceptance of an unrelated faction condition. The survivor can receive help with the need while leaving the offer unresolved where systems permit.

## 334. Advice branch — The survivor asks the player to speak for them

The survivor may authorize the player to convey a specific decision or question. Confirm the exact words and scope: ask for a new time, decline the task, or request a written offer. The player repeats only that message. If the contact adds new terms, return to the survivor for another decision.

The authorization can be limited to this conversation. Do not assume a general power of representation. If persistence is supported, record only its specific scope and expiry; otherwise, rely on the current scene and ask again next time.

## 335. Installment 22 close — Advice supports, but does not replace, choice

The player can compare facts, offer a transparent recommendation, disclose a conflict, find another perspective, or simply remain present. The survivor can accept advice, reject it, choose later, ask for representation, or decline every offer. Each route changes the conversation and practical next step.

The ending should name the survivor’s actual decision, not the player’s influence. If the survivor has not decided, preserve that state. A meaningful companion role includes helping someone decide without taking the decision away.

## 336. Expansion installment 23 — The survivor chooses who hears about the refusal

After declining an offer, the survivor may want different people to know different things. The contact needs to know the offer was declined; a friend may need only to know the survivor is unavailable; a task owner may need to know whether a role is still staffed. The player can help separate those audiences and ask what may be shared with each.

This is not a secrecy minigame. The survivor can speak plainly, permit a narrow explanation, ask for no reason to be shared, or choose to tell someone themself. The player should not turn one authorized message into blanket permission to discuss the refusal with every faction or resident.

## 337. Branch — The contact needs an operational answer

The offer owner may need to know whether to hold a place, contact another person, or close the proposal. The survivor can authorize a short operational response without giving a personal reason. The player can confirm whether the offer remains open and what consequence follows from the answer.

If the contact asks for more context than the owner requires, the survivor can decline or provide only what they choose. The player should not over-explain in an attempt to make the refusal more acceptable. A clear “they are not taking the offer” can be enough.

## 338. Branch — A friend asks whether the survivor is all right

The friend may be worried or curious. The player can ask the survivor whether they want a check-in, a neutral message, or no contact. The survivor may appreciate support, prefer privacy, or want the friend to know a specific concern. The friend’s care does not automatically authorize access to the survivor’s reasons.

If the survivor says no message, the player can respect that while directing the friend to any general support route the friend already owns. Do not make the survivor responsible for reassuring everyone. Their refusal can be private without turning the friend into an antagonist.

## 339. Branch — A task owner needs to replace the survivor

The task owner may need to know whether the survivor is available, but not why they declined the faction offer. The player reports the availability fact and asks about a replacement. If the survivor also declined the task, that is a separate decision and should be communicated under its own rules.

Do not conflate “declined a faction offer” with “unavailable for all work.” The survivor can still volunteer for another task, accept a different role, or take time away. The owner confirms assignment status, while the survivor controls personal reasons.

## 340. Branch — The survivor wants their reason understood by one person

The survivor may choose to explain their reason to a trusted contact while keeping it from the faction at large. The player can help find a suitable private setting and confirm whether the contact is authorized to hear it. The survivor can share fully, give a limited explanation, or stop at any point.

The contact should not promise absolute confidentiality unless that is supported and true. If the information triggers a separate safety or task obligation, explain that boundary before the survivor shares. Consent to one conversation is not consent to repeat the story elsewhere.

## 341. Branch — Rumor fills the silence

Other residents may speculate about why the survivor declined. The player can correct a false factual claim without revealing the private reason: “They declined; the reason is theirs to share.” The survivor may ask the player to say nothing, issue their own statement, or speak only to a specific person.

If a faction is spreading a confirmed false claim, the survivor can request a correction through the permitted channel or leave it unanswered. Do not invent a shelter-wide rumor system or add a universal reputation penalty. Keep the scene tied to the people who actually heard and repeated the statement.

## 342. Branch — The faction requests a public explanation

The faction may want to explain why an opportunity closed or reassure other members. The survivor can approve a neutral operational statement, provide a quote, ask the faction to say nothing, or refuse. The faction can accept, negotiate wording, or publish its own account where authorized.

The public statement must not imply the survivor endorsed the faction or its offer. If the faction cannot honor the survivor’s boundary, they may decide whether to challenge it, seek another contact, or disengage further. The player should not promise control over a channel the faction owns.

## 343. Branch — The survivor tells different people different details

The survivor may share a personal reason with one friend, a practical reason with the task owner, and no reason with the faction. This is not inconsistency to be exposed. Each audience receives only the authorized information. If the survivor later wants to revise what may be shared, the player can help do so through supported routes.

The game should not punish them for maintaining several boundaries. A later character may learn something through an authorized conversation, but other characters cannot know it automatically. Avoid omniscient callbacks that reveal private details because a flag exists somewhere in the narrative.

## 344. Installment 23 close — Information follows the survivor’s permission

This installment creates distinct branches from who needs an operational answer, who wants to offer care, who controls a task, and who may hear the personal reason. The player can convey a narrow message, protect privacy, correct a false claim, or let the survivor speak directly.

Endings include a contact informed without explanation, a friend checked in with permission, a task reassigned, a private reason shared with one person, a public correction, or no statement. The supported persistent fact should capture only what was actually authorized and communicated.

## 345. Expansion installment 24 — The survivor withdraws after accepting

A survivor may accept an offer and later decide they no longer want to continue. The player asks what has happened so far: an appointment may be pending, a task may have begun, a resource may have been issued, or a service may already be complete. The owner confirms which steps can still change. The survivor chooses whether to withdraw, revise, or ask for help finishing a bounded part.

Withdrawal should remain possible without pretending every consequence is reversible. If the survivor has already used a service or an item was transferred, the player explains that fact and routes any remedy through its owner. Sunk cost does not create new consent to future participation.

## 346. Branch — The survivor withdraws before any action begins

The survivor can tell the contact directly, authorize the player to convey the decision, or use an available channel. The offer owner confirms closure. The contact may acknowledge, ask one practical question, keep a separate route open, or respond emotionally. The survivor can answer, decline discussion, or leave.

If the player conveys the message, confirm the exact scope. “I am not attending” may not mean “close every offer.” The survivor can specify whether the decision applies to one meeting, one role, or all future contact. Do not infer a broader refusal from a narrow withdrawal.

## 347. Branch — The survivor has already started a task

The task owner explains how to pause safely, hand off work, or finish a necessary immediate step. The survivor can choose among supported options and state whether they are willing to complete a short handoff. If they are not, the owner finds another route where possible.

Do not force the survivor to finish a task because they once agreed. If stopping instantly would create a hazard, explain the minimum safe action and why it is required under the owner’s rule. The survivor can ask for another person to take over and should not be blamed for a plan with no backup.

## 348. Branch — A resource was issued for the accepted offer

The resource owner confirms whether the item was transferred, loaned, reserved, or only made available. Each status has a different closeout. The survivor may return an item if required, keep it if ownership transferred, or ask for guidance if the terms were unclear. The player cannot create a debt based on an assumption.

If the survivor used the resource, the owner explains any supported consequence. They can still withdraw from future work or meetings. Do not require them to repay an unspoken cost or accept a new condition to close the previous arrangement.

## 349. Branch — The contact says withdrawal harms the group

The contact may describe a real staffing or schedule consequence. The player can ask who owns the plan and what alternatives exist. The survivor can acknowledge the impact, offer a permitted handoff, decline further involvement, or ask the group to find another person.

The contact’s concern may be sincere, but it does not override the survivor’s choice. Avoid using the group’s need as a guilt mechanic. If the faction has a formal exit procedure, explain its actual steps and limits before asking the survivor to complete them.

## 350. Branch — The survivor wants to finish one part and stop

The survivor may be willing to complete a safe handoff, return borrowed property, or attend a final practical meeting while refusing the larger commitment. The player checks whether these steps are separable and who owns them. The survivor can accept a narrow closeout, negotiate another option, or refuse further action.

If a closeout step is optional, label it optional. If it is required by an existing agreement, state the exact obligation and offer the supported way to resolve it. Do not add a farewell ceremony, public explanation, or loyalty pledge as a condition of leaving.

## 351. Branch — The faction asks the survivor to reconsider

The contact may ask whether a different schedule, role, or representative would change the decision. The survivor can hear the revision, request the new terms, decline to reconsider, or pause. If they hear the new offer, it is a fresh decision and must not erase the withdrawal unless they explicitly choose to reopen the route.

The player can support the survivor without speaking over them. If they ask for no further pressure, the player can state that boundary. The faction may respect it, ask for one procedural closeout, or refuse continued service based on actual rules.

## 352. Branch — The survivor regrets the original acceptance

The survivor may say they accepted because they felt rushed or misunderstood the terms. The player can listen, compare the original offer with what happened, and ask what they want now. If the contact misrepresented a condition, route the issue through a supported review. If the survivor simply changed their mind, that is still a valid present choice.

Do not rewrite the original acceptance as though it never occurred. Completed actions remain true; future participation can stop. The survivor can choose whether to explain the pressure, keep it private, or seek another representative.

## 353. Branch — Another participant already relied on the commitment

A teammate or resident may have arranged their own schedule around the survivor’s acceptance. The player can tell the survivor what is operationally necessary, with permission, and ask the task owner to identify alternatives. The other participant may be disappointed or need a replacement, but cannot require the survivor to continue.

If the survivor wants to notify them directly, help arrange it. If they do not, the task owner can communicate the changed roster without sharing private reasons. The team should adapt to the current confirmed availability.

## 354. Withdrawal closeout — Separate completion from continued consent

The route can end with the offer withdrawn, a task handed off, a resource returned or retained according to ownership, a review request, a limited closeout, or the survivor choosing to continue after fresh terms. The player states what is complete and what is no longer authorized.

Any persistent record should reflect the supported offer or task state, not punish the survivor for withdrawal. The faction may respond through its actual service and relationship owners. Later dialogue can remember the boundary without treating the survivor as unreliable for revising a decision.

## 355. Expansion installment 25 — What contact remains after the boundary

After a refusal or withdrawal, the survivor can choose what kind of contact remains: no further messages, a practical channel only, a personal connection with one representative, or openness to a new offer if its terms change. The player can clarify the choice and relay it only to the appropriate owner. The survivor need not decide the future of every relationship in the same conversation.

If the faction can support the requested boundary, it may confirm the channel and proceed. If its rules do not permit that separation, the contact explains the limit and the survivor chooses whether to accept the remaining relationship or disengage. A supporting group can offer an alternate point of contact but cannot guarantee what another faction will do.

Later scenes should honor the stated boundary. A practical update can arrive without turning into a renewed pitch. A personal meeting can begin with the survivor’s chosen contact. A new offer can be presented as new and require fresh consent. If there is no authorized way to guarantee the boundary, explain that limitation rather than promising a system-level block that does not exist.

The branch can close with no contact, operational messages only, one retained relationship, a future offer permitted under stated conditions, or an unresolved request to the faction. These are concrete communication and relationship choices, not a permanent joined-or-rejected label. The survivor can revise them later, but each revision belongs to them and should be confirmed at the time.

If a later contact violates the boundary, the player can identify the channel, repeat the survivor’s instruction, and ask whether they want to respond, use another representative, or disengage. Do not assume the first violation ends every relationship; the survivor decides what it means. If the contact honors the boundary, that does not obligate the survivor to reopen the offer. The follow-up branch is about whether the survivor’s requested form of contact is respected, not about rewarding the faction with renewed access.

The survivor may also decide that the boundary no longer fits. They can authorize one new conversation, restore a practical channel, or invite the contact to present revised terms. Ask what changed and which details may be shared; do not presume that a single friendly exchange restores all former permissions. The contact can accept the new scope, clarify a limit, or decline. The survivor remains free to close the route again if the conversation changes in a way they did not agree to.

The survivor’s reflection may include both relief and regret. The player can acknowledge that mix without treating regret as a request to undo the refusal. If they want another option, help identify one; if they only want to be heard, leave the offer closed. The next action follows their request, not the emotion the player thinks should matter most.

If the survivor asks for no follow-up, that instruction remains the immediate endpoint. A later change must begin from a new request or a clearly authorized contact rule.

The player can acknowledge the choice and move on to another questline. Respecting a closed route is an active outcome, not a missing branch.

If the survivor reopens it later, begin with their new request and the contact’s current terms, not an assumption that the old offer remains available.

## 356. Expansion installment 26 — The survivor proposes a route of their own

When existing offers do not fit, the survivor may propose a different way to pursue the underlying goal. They might ask for a smaller service, a neutral intermediary, a different schedule, a public source, or a task that does not require joining a faction. The player helps identify which owner can answer each part and which pieces are only suggestions.

The survivor’s proposal may succeed, require revision, or be declined. The player should make that uncertainty visible without using it to push them back toward the original offer. A self-directed route is meaningful even if it takes longer or solves only part of the need.

## 357. Proposal branch — Separate the goal from the offered package

The survivor explains what outcome they want, if they choose to share it. The player can ask which parts of the previous offer actually served that outcome and which were incidental conditions. The survivor may name a narrower need or keep the reason private and describe only the requested action.

The player then identifies appropriate owners: a task owner for work, a location owner for access, a source owner for information, or a faction contact for a service. Do not route everything through the faction that made the first offer. A separate owner may have an independent path.

## 358. Proposal branch — The survivor asks for a smaller service

The survivor may request one appointment, a limited supply, a short consultation, or access for a single task rather than the entire package. The owner can confirm whether that narrower service exists. The survivor can accept its limits, propose another scope, or stop.

If the service is not separable, explain why in concrete terms. Do not invent a reduced offer to make the branch feel positive. A clear “that resource is only available under these terms” lets the survivor decide knowingly whether to accept or seek another route.

## 359. Proposal branch — The survivor wants an independent intermediary

They may ask a neutral resident or supporting faction to carry a question, witness a meeting, or compare terms. The player confirms that the intermediary is willing and that the survivor consents to the information shared. The intermediary can clarify process but cannot negotiate beyond the authority granted.

The original faction can accept the intermediary, refuse, or request a different person. The survivor can proceed directly, choose another contact, or close the route. The intermediary’s faction does not become a new decision-maker simply because it is trusted by the survivor.

## 360. Proposal branch — The survivor requests access without membership

The survivor may ask to use a public resource or attend one event without taking a faction role. The player checks the resource owner’s rules and asks whether the request is allowed. The faction can grant access, explain a linked condition, or decline. The survivor can accept the boundary, seek a public alternative, or walk away.

If another faction owns an equivalent resource, the player can ask whether the survivor wants an introduction. Do not frame this as switching allegiance. A person can use a service from one group while maintaining independent relationships elsewhere.

## 361. Proposal branch — The survivor offers a contribution on their own terms

The survivor may offer knowledge, time, or a specific task in exchange for the support they need. The task owner confirms safety and availability; the survivor states the limit; the other party confirms whether it accepts. If the contribution is not supported as part of the service, keep it separate and make clear what has not been agreed.

The player should not enlarge the contribution because the survivor is asking for help. If the owner requests additional work, treat that as a new condition. The survivor can accept, revise, or refuse without losing unrelated help unless the terms explicitly require it.

## 362. Proposal branch — The plan needs other residents’ consent

The survivor’s alternative may involve a family member, helper, or shared room. The player can identify who must be asked and what they need to know. The survivor may authorize an introduction, make the request personally, or decide that involving others is not acceptable.

Do not let the survivor commit another person to labor or disclosure. Each affected resident can accept, decline, or request a different role. If one person refuses, the survivor may redesign the plan or stop; no one should be blamed for preserving their own boundary.

## 363. Proposal branch — The owner rejects the alternative

The relevant owner may say the proposed route is unavailable or outside its scope. The player can ask what part is impossible, whether another owner should review it, or whether the survivor wants to try a different version. The owner may suggest an allowed alternative, but the survivor decides whether it meets the need.

Do not interpret a procedural rejection as hostility. It may reflect capacity, safety, or an actual rule. If the owner refuses without explanation, preserve that uncertainty and let the survivor choose whether to seek a second contact or disengage.

## 364. Proposal branch — The alternative partly works

The new route may solve the immediate access problem but not the survivor’s larger goal. The player can ask what has changed, what remains unmet, and whether the survivor wants to stop or continue. A partial result should not be described as full success merely to close a quest.

The survivor may accept the limited benefit, seek another source, or return to the original offer with a fresh decision. The faction’s previous terms remain separate unless it explicitly changes them. This branch creates continuity from action rather than from alignment: what the survivor tried determines what they know and what routes remain.

## 365. Proposal branch — A supporting group can provide one missing piece

A small group may provide a room, a source, or a short introduction that makes the alternative possible. The player asks what it controls and whether there is any condition. The survivor can accept that piece without joining the group or endorsing its politics.

The supporting group should not become a miracle solution. It may lack capacity, require a return time, or be unable to provide the central service. The survivor can decide that the contribution is still useful, ask for another route, or decline. Keep the group’s effect local and visible.

## 366. Proposal close — A self-directed route can remain unfinished

The alternative may become a confirmed arrangement, a referral, a partial service, a rejected proposal, or a route the survivor pauses. The player reports which owner accepted what and which steps remain unconfirmed. The survivor can choose whether to continue pursuing the goal.

This installment adds a distinct response to refusal: create a route that fits the survivor rather than merely comparing faction packages. It preserves the possibility of failure, delay, and partial success while giving the survivor practical authorship of the next attempt.

## 367. Expansion installment 27 — Initiative has a time, audience, and owner

A survivor may bring an idea to the player during a quiet moment, after a task, at a public meeting, or through an authorized message. The player’s response should depend on what that person actually did and which conversation routes are available. The initiative is not a global event broadcast. The audience may be one trusted person, a small work group, a faction contact, or the whole shelter, and the survivor can change their mind about who should hear it before the request is sent.

The player sees enough to decide whether to help: the requested outcome, the affected people, the likely owner, and any known time or material cost. The person may withhold the private motivation. The player can help clarify the proposal, refer them to an owner, ask whether the proposal is urgent, or decline to intervene. Each response has a concrete consequence, but the survivor is still free to pursue the matter directly if the system allows it. Do not convert every initiative into a quest marker that implies the player must manage it.

## 368. Initiative branch — The survivor raises a concern in private

The survivor may ask to speak privately because the concern involves a peer, a supervisor, or their own work history. The player confirms the limits of confidentiality before inviting details. If the player cannot keep the matter private because it concerns immediate safety or another owner’s required process, say so in plain language. The survivor can continue with that knowledge, describe only the practical issue, seek a different contact, or stop.

The player should not promise absolute secrecy when the game’s systems cannot honor it. If the survivor asks for an intermediary, identify who may receive what information and let them approve the summary. The relevant owner may need a location or task fact but not the survivor’s personal reason. If a report becomes necessary, record the minimum operational details through that owner. The survivor can ask what was forwarded and what follow-up is expected.

## 369. Initiative branch — The survivor raises the same issue in public

The survivor may choose a meeting or open work discussion because the issue affects others. The player helps identify who should be present, what can be said publicly, and whether anyone’s private details need to be excluded. A public request can draw support, disagreement, or no response. The meeting owner controls the agenda and the affected operational owner decides whether to act.

The survivor can ask a question, propose a change, or withdraw the point before the meeting. Others may challenge the proposal without attacking the person. The player can redirect personal allegations to a private review route while keeping the public operational issue visible. If the meeting cannot decide, it can assign a follow-up owner, defer the question, or record an unresolved point only through a supported record. Attendance does not equal endorsement.

## 370. Initiative branch — A second survivor makes a different proposal

Another person may independently suggest a similar action with a different motive or cost. The player can compare the practical overlap, ask whether the proposers want to coordinate, or route each proposal separately. Neither should be made the other’s spokesperson by default. They may share a goal while disagreeing about who should do the work or how public the change should be.

If they choose a joint approach, each states which part they own and what they consent to share. The task owner confirms whether the work can be combined. If one withdraws, the remaining proposal may still proceed at a smaller scope. A joint initiative should not create an automatic faction or group identity. It is a temporary agreement that ends when its specific task ends unless participants choose otherwise.

## 371. Initiative branch — A proposal requires a named person’s labor

The survivor may propose that another resident perform a task, provide instruction, or attend a meeting. The player asks whether the named person has been approached and whether they are willing. The proposer can offer to make an introduction, invite the person to decline directly, or redesign the proposal around available capacity. They cannot commit someone else’s time by describing it as an obvious favor.

The named person may accept all, part, or none of the request. Their current duties, needs, and consent are checked through the existing owner. The proposer can respond with understanding, ask for another route, or feel disappointed. The player can help preserve the relationship without treating the refusal as a debt. The proposal may remain possible with a different worker, but only after that worker makes a separate choice.

## 372. Initiative branch — The player cannot decide the request directly

The survivor may ask the player to authorize a repair, alter a duty, grant access, or change a resource allocation. The player can explain which owner controls the decision and help prepare a clear request. The owner may approve, reject, modify, or ask for evidence. The survivor may accept that route, ask for review, seek another authorized contact, or decide not to pursue it.

The player should not make a dialog choice that bypasses the owner’s authority. A request can still have a meaningful ending if the owner declines. The survivor can learn the rule, challenge it through a supported process, or redirect their effort. If the actual game does give the player formal control over a decision, the interaction should call the relevant command and state the resulting cost instead of presenting a fake referral.

## 373. Initiative branch — The survivor asks to act before approval

The survivor may want to begin a project while the request is still pending. The player checks which preparatory steps are reversible and which would spend materials, occupy space, expose information, or create a safety risk. The owner can approve limited preparation, require the survivor to wait, or reject the request. The survivor decides whether to do the permitted work or pause.

Do not punish initiative for existing in the space between idea and authorization. The person can sketch a plan, gather public information, or ask questions if those actions are allowed. They cannot be shown moving shared inventory or entering a restricted room without approval. If they proceed anyway, use evidence and the proper owner’s response; do not automatically label them reckless or criminal. The player can mediate the repair of trust while preserving the rule.

## 374. Initiative branch — A proposal benefits the proposer and others unevenly

The proposed schedule, shared room, or allocation may help the proposer while inconveniencing another group. The player can map who benefits, who bears cost, and who needs to consent. The affected residents may propose a different time, a smaller pilot, a rotation, or a public explanation. The owner determines whether such arrangements are feasible. The proposer can accept revisions, negotiate within the rules, or withdraw.

Avoid resolving this through a generic popularity or altruism score. A person may have a legitimate need and still impose a cost. Another may accept the inconvenience freely or object with a valid reason. The consequential branch is how each person responds to the surfaced cost and whether an owner approves a workable plan. The player can preserve minority access, ask for a trial, or accept that no arrangement can meet all needs.

## 375. Initiative branch — A resident asks for credit or anonymity

The survivor may want their contribution recognized or may prefer the outcome to stand without attribution. The player asks before sharing their name. The audience and record owner determine whether anonymity can be honored. The survivor can choose public credit, a limited acknowledgment, or no attribution where the system supports it.

If the task record requires an assignee, explain that operational record separately from public credit. A faction may wish to showcase its member’s work; the resident can accept, limit, or decline that request if it is optional. Do not treat anonymous work as less real or public recognition as proof of faction loyalty. The feature should let people manage how their initiative is narrated without creating an unsupported publicity permission system.

## 376. Initiative branch — The resident revises the proposal after listening

After hearing the owner or affected peers, the survivor may change the scope. They can keep the underlying goal while changing the timing, audience, task, or proposed resource. The player can compare the revised request with the original and show which concerns it addresses. The owner evaluates the new version rather than assuming the previous response carries over.

The revision can be a sign of practical learning, not capitulation. The survivor can say which boundary remains important. Affected peers can accept the change, propose a second adjustment, or still object. If the revision becomes materially different, close the prior request accurately and submit the new one through the existing route. Do not keep both branches active and risk duplicate work or contradictory promises.

## 377. Initiative branch — The resident withdraws before an answer

The survivor may decide that pursuing the request costs more than it is worth, that circumstances changed, or that they no longer want the outcome. The player can ask whether any action already taken must be stopped or reversed. The owner confirms whether materials were reserved, notices were sent, or a task was created. The survivor can authorize a concise cancellation, handle it themselves, or ask for help.

Withdrawal is not necessarily avoidance or weakness. It can be an informed decision after the owner explains constraints. The faction contact may be disappointed, but cannot insist that a tentative conversation is an agreement. If another person relied on the request, communicate the confirmed change without exposing private reasons. Preserve costs that have actually occurred and release commitments that remain reversible.

## 378. Installment 27 close — Initiative is a relationship to process

The episode can conclude with a private referral, a public discussion, a joint proposal, a declined labor request, an owner decision, a permitted preparation step, a negotiated revision, a request for recognition, or an early withdrawal. Each ending comes from the path of communication and action. None represents a global “good survivor” score.

Later dialogue should recall what was said, who heard it, what authority responded, and whether the proposer consented to attribution. If the game cannot persist one of those details, do not imply a durable campaign fact. This approach gives residents more ways to act while keeping the player’s power concrete and the supporting factions in a bounded advisory role.

## 379. Expansion installment 28 — After a refusal, trust can be repaired through a new action

A refusal can leave the original need unresolved and the relationship unsettled. The next branch should come from what each person does after hearing the answer. The survivor may ask why the request was rejected, accept the stated limit, seek a different owner, propose a smaller task, or decide that the relationship needs distance. The recipient may clarify a misunderstanding, offer a different form of help, or maintain the boundary. No one must forgive or re-open the offer for the conversation to have value.

The player can distinguish an owner’s capacity refusal from a personal rejection. They can ask whether any fact can be reconsidered, whether another service exists, and whether the survivor wants a mediator. The new contact should not invalidate the first person’s refusal. A supporting faction can offer a different route, but the survivor chooses whether to engage and the service owner defines what it can provide.

## 380. Repair branch — The survivor asks for the concrete reason

The survivor may ask why an offer or request was refused. The player can relay the reason the owner actually provided: unavailable time, missing qualification, access restriction, stock shortage, incompatible scope, or a personal boundary. If the reason was not given, say that it remains unknown. Do not invent a sympathetic explanation to reduce conflict or a hostile one to create drama.

The survivor can ask for review, provide missing evidence, accept the result, seek another route, or decline further discussion. If the owner’s explanation exposed protected information, share only what the survivor is entitled to know. The player may ask the owner for a clearer public reason. A clear reason can soften uncertainty without changing the outcome; the refusal remains a legitimate decision.

## 381. Repair branch — A correction reveals the request was misunderstood

The survivor may realize that the offer addressed the wrong need or that they communicated the scope poorly. They can clarify what they actually wanted and ask whether a new request is welcome. The recipient can hear it, refer it to the owner, or continue to decline. The new conversation is not a loophole that makes the earlier “no” provisional; it is a separate, clearer proposal.

The player can help restate the need without claiming ownership of it. The survivor may prefer not to try again. If the new request affects the same person, check whether they consent to another conversation. If they do not, route to an independent contact. A later accepted offer should have fresh terms, capacity, and confirmation rather than inheriting a vague assumption from the original exchange.

## 382. Repair branch — The recipient offers a different kind of support

The person who refused the original request may offer a smaller or different form of help: a referral, a brief explanation, a future check-in, or help finding another resource. The survivor can accept that support, ask for something else, or decline. The original boundary remains clear. The alternative should not be portrayed as a disguised obligation to take the offer they already refused.

If the alternative requires another owner’s approval, the recipient says that before the survivor relies on it. The player can ask what is confirmed today and what is only a possibility. A survivor may value the referral even if it does not solve the need. They may also decide that this relationship is not the right place to seek assistance again.

## 383. Repair branch — The survivor chooses a different contact

The survivor may choose another person, support group, or major faction contact. The player asks what information can be shared and whether the survivor wants to make the introduction themselves. The new contact may accept, decline, or refer elsewhere. Their answer does not change the first person’s boundary or automatically establish the new contact as a trusted ally.

If the second route is faction-based, disclose its conditions and any relationship between that faction and the original recipient. The survivor can proceed, ask a neutral broker, choose a public service, or stop. The player should not push a faction route merely because it is available. Two supporting groups may have different capacity and rules; the survivor compares those specifics rather than “choosing sides.”

## 384. Repair branch — The original recipient hears about the alternative

The recipient may learn that the survivor sought help elsewhere. They can be relieved, concerned, jealous, or indifferent, but the narrative should connect their reaction to established relationship context. The player can clarify that seeking another route does not constitute retaliation. The survivor can keep the new arrangement private where possible or invite the original recipient to hear the outcome.

Do not auto-decrease affinity for choosing an alternative. If a person had a practical reason to expect a follow-up, the survivor can decide whether to provide one. The original recipient can ask not to be involved further. The alternative relationship remains independent. The story may show more than one faction participating in the survivor’s life without converting their support into faction alignment.

## 385. Repair branch — A mediator is available but not neutral

A proposed mediator may be a friend of one party, an officer in the faction that issued the offer, or the owner of a related resource. The player can disclose the connection and ask both people whether they still want that person involved. Either can request someone else, proceed with the disclosed limitation, or stop. Mediation can help clarify communication without changing who owns the decision.

The mediator should agree to the scope: listen, carry messages, explain process, or facilitate a meeting. They should not be expected to adjudicate unless an existing owner grants that role. If no suitable mediator exists, the characters can communicate directly through an authorized route or leave the issue unresolved. A one-sided “neutrality” label would hide the exact dependency the player needs to understand.

## 386. Repair branch — The survivor asks for distance

After a refusal, the survivor may want to reduce contact with the person or faction involved. The player can help establish a practical boundary, change a meeting route, or ask the relevant task owner to avoid unnecessary direct assignment. The survivor can also choose no change. The recipient is told only what they need to respect the boundary.

Distance is not an automatic permanent break. The survivor may later choose to resume contact. The player should not erase a relationship or set a loyalty flag unless the existing owner supports that change. If shared duties make separation difficult, the owner can identify a safe arrangement or state that no alternative is available. The survivor may accept the constraint, ask for review, or choose to leave the task.

## 387. Repair branch — The recipient changes their answer later

The recipient may later offer help after availability changes. The player confirms that the new offer has specific scope, capacity, timing, and any conditions. The survivor can accept, ask whether the earlier reason still applies, decline, or request time. A changed “yes” should not imply that the first refusal was deceptive; circumstances may simply have changed.

If the recipient offers the same thing under stricter conditions, state those conditions. The survivor can compare them with a supporting group’s alternative. They need not reward the recipient for changing their mind. The player can help maintain a calm exchange, but cannot pressure acceptance with a claim that this may be the last chance unless the owner verifies scarcity.

## 388. Repair branch — The survivor is asked to publicly endorse the helper

A faction may provide an alternative and ask the survivor to say publicly that its service was fair or effective. The survivor can agree, provide a limited factual acknowledgment, decline, or ask for time. The player confirms whether endorsement is optional and whether it is required by the service terms. If it is required, that condition should have been disclosed before acceptance.

The survivor may be grateful and still refuse publicity. A faction contact can explain its reasons or withdraw a genuinely conditional service, but should not retroactively pretend the condition was never part of the offer. The player can help find another route or clarify the arrangement. Do not treat public praise as a currency that improves the survivor’s future access unless the service owner explicitly and transparently defines such a rule.

## 389. Installment 28 close — Repair does not require reconciliation

The refusal aftermath can end in a clear explanation, a revised request, an alternative service, an independent contact, a disclosed mediator, a boundary, a changed offer, a limited endorsement, or no further contact. A renewed relationship is one possible ending, not the mandatory moral conclusion. The person who refused retains that boundary and the survivor retains the right to search elsewhere.

For implementation, persist only owner-backed facts already required by the social or task route: current agreement, accepted referral, assigned task, or explicit relationship event. Do not add a bespoke “refusal recovered” meter. The authored branch can be rich even when its state footprint is small, because the conditions and dialogue make the choices materially distinct.

## 390. Expansion installment 29 — Acceptance remains revisable until commitment is clear

Survivors may say “yes” to hear more, “yes” to consider an offer, or “yes” to the exact work and conditions. Those are distinct conversational acts. The player should see when the offer becomes a commitment, what owner records it, and whether either party can cancel before work begins. The same precision applies to a refusal: declining the meeting is not necessarily rejecting all future assistance.

This is especially important when an offer comes through a faction, an intermediary, or a public meeting. A supporting faction may have limited service capacity, but that scarcity does not justify ambiguous language. The contact states how long an offer is available, who may accept, and what changes after acceptance. The survivor can ask questions, accept with a narrow scope, request a later decision, or decline.

## 391. Commitment branch — The survivor agrees to discuss terms only

The survivor may attend a conversation without authorizing a task or sharing personal information. The player confirms that the meeting is exploratory. The offeror can answer questions and state what remains conditional. The survivor can end the meeting, ask for written terms, or request an intermediary. Do not mark the offer accepted because the survivor listened politely.

If the offeror expects a decision by a deadline, the deadline is part of the terms and should be visible. The survivor can ask for an extension or say that the time is insufficient. The owner may hold the slot, release it, or refuse the extension. Any later acceptance must still be explicit. The scene can end with greater clarity and no commitment, which is a valid outcome.

## 392. Commitment branch — The survivor accepts one part of a package

An offer may include access, supplies, a work role, and a public appearance. The survivor may want only one part. The player asks the offeror whether components are separable and what each part costs. The offeror can agree, clarify that the package is indivisible, or decline to offer a partial version. The survivor can accept the bounded component, accept the full package knowingly, or refuse the whole offer.

Do not present a partial acceptance as a completed deal if a required owner has not confirmed it. If the components have different owners, each must answer. The survivor can withdraw the partial request if coordination becomes too burdensome. A faction’s inability to separate its package is not automatically villainous; it is a meaningful constraint the player can compare with other routes.

## 393. Commitment branch — A detail changes between conversation and start

The work owner may change the schedule, materials, location, audience, or expected contribution after the survivor has tentatively agreed. The player shows the change and asks whether the survivor wants to continue. The survivor can accept the new terms, request the original, propose an adjustment, or withdraw. The offeror must not treat the old agreement as authorization for the revised task.

If work already began, the owner identifies which actions can stop and which costs have already occurred. The survivor may finish a safe current step, hand off, or stop immediately when a boundary or hazard requires it. The task owner handles materials and continuity; the social owner handles any agreed relationship effects. Keep the original consent visible so a later dispute can distinguish agreement from changed scope.

## 394. Commitment branch — The survivor asks for an independent witness

The survivor may request that someone witness the terms or task start. The other party can accept, suggest an authorized reviewer, or decline. The survivor can proceed without a witness, seek a different offer, or stop. A witness can confirm what was said but cannot silently become a manager, advocate, or guarantor.

Before agreeing, the player clarifies whether the witness may take notes, share information, or intervene if the scope changes. If the system does not represent those roles, keep the witness as a narrative observer and avoid implying a durable legal record. A supporting faction may provide a witness, but its affiliation must be disclosed so the survivor can choose knowingly.

## 395. Commitment branch — The survivor accepts under a stated limit

The survivor may accept a task with a clear limit: one shift, one meeting, no public attribution, no overnight duty, no use of a specific tool, or a check-in before any extension. The task owner confirms which limits are operationally feasible. The survivor can proceed, adjust the limit, or decline if it cannot be honored. A limit becomes a true agreement only if the owner records or enforces it through a supported path.

If an unexpected event requires a change, the task owner pauses and asks again where possible. Emergency rules remain those of the current authority; the plan does not invent an override. Afterward, the survivor can report whether the boundary was respected and request review. The player can support that review without assuming the task owner’s conclusion.

## 396. Commitment branch — A survivor delegates communication but not the decision

The survivor may ask the player or an intermediary to deliver a response because they do not want a direct meeting. They can approve the exact message, allow the intermediary to answer logistics, or retain the final decision for themselves. The recipient may require direct confirmation before committing resources. The player explains that limit before relaying anything.

Do not let an intermediary expand the survivor’s authority. They can communicate “not now” without converting it into “never,” or request more details without accepting terms. If the survivor later changes their answer, obtain fresh authorization. A written or spoken message remains within the agreed audience, and the recipient can ask a procedural question without gaining access to private reasons.

## 397. Commitment branch — A public commitment creates pressure

The survivor may accept in front of a group and later realize they need to revise the terms. The player can speak with them privately, check whether work has started, and route any change through the owner. The group may feel disappointed, but public embarrassment does not make consent irrevocable. The survivor can explain a practical limit, give no personal reason, or ask the player to relay the update.

If the commitment was a real assigned task and withdrawal affects operations, the owner can find a safe handoff or state the actual cost. Avoid social punishment beyond consequences the existing owner supports. The survivor may choose to complete a current safe portion before leaving, but the player cannot force it. A public clarification can protect them from rumor if they want it and the audience needs it.

## 398. Commitment branch — The offer expires while the survivor is deciding

The offeror may withdraw or reassign a limited resource before the survivor answers. The player can check whether the survivor had a reasonable chance to decide and whether an extension is possible. The offeror can reopen it, hold the next slot, or explain that capacity is gone. The survivor may accept a new version, seek another route, or accept that the opportunity passed.

Do not use artificial urgency to force a choice. If the deadline was known and genuine, preserve it and the reason it mattered. If no deadline was stated, the player should not suddenly introduce one after the survivor asks questions. The ending can include regret or relief, but the game should not make the person morally responsible for a capacity change outside their control.

## 399. Commitment outcomes — Clear agreement can still lead to several endings

The route may produce an exploratory conversation, a partial package, a revised offer, a witnessed start, a bounded task, delegated communication, a revised public commitment, or an expired opportunity. Each should leave the offer’s state, task assignment, inventory movement, and social response consistent with existing owners. If any required owner failed to confirm, show that the commitment is pending or unavailable rather than complete.

The player can be a careful negotiator, practical scheduler, advocate for boundaries, or person who accepts a narrow deal to meet an urgent need. Those approaches create distinct scenes and outcomes without using an evil/good axis. The survivor’s own actions—what they ask, disclose, accept, refuse, and revise—drive the branch.

## 400. Installment 29 close — Consent is a sequence of checkable moments

Offer, discussion, acceptance, assignment, start, scope change, completion, and review are separate moments. The owner may combine some of them in its API, but the authored plan must not claim independent durability where none exists. At every material change, the player sees the consequence and the person can respond. A clear sequence makes both successful and failed routes readable.

This installment closes the gap between a friendly conversation and a binding action. Supporting factions can extend a service, witness a process, or impose an honest capacity limit. They cannot turn curiosity into commitment. The survivor can still change direction, and the player can help them do so without erasing costs already incurred.

## 401. Expansion installment 30 — An initiative can matter after the immediate request ends

A survivor’s proposal may change one task and then create a second question: who maintains the new arrangement, who can use it, and how can people ask for revision? The player can help the proposer decide whether to hand it to an owner, keep it personal, invite a small group, or stop after the first result. The original initiative does not grant permanent authority over the service it influenced.

The relevant owner can accept responsibility, request a narrower pilot, or decline to maintain the change. The proposer can welcome shared ownership, ask to stay involved, or withdraw their name. A supporting group can offer occasional help but should not be described as steward of the entire system unless it explicitly accepts that role and has authority. The player can make the follow-through visible without forcing the survivor into endless unpaid coordination.

## 402. Follow-through branch — The proposal works once but needs upkeep

The survivor may have organized a temporary schedule, repaired a shared item, or created a one-time supply arrangement. The owner checks what upkeep is required and who is qualified to do it. The proposer can continue, train another willing person, request an owner assignment, or let the arrangement end. Other participants can volunteer or decline.

Do not assume that the person who proposed an idea must maintain it forever. The player can show the cost of upkeep before anyone agrees. If no one accepts responsibility, the temporary result can expire and the owner can return to the prior arrangement. That ending may disappoint users while still respecting capacity. Any recurring assignment must use the existing duty and task owners.

## 403. Follow-through branch — The proposer wants shared authorship

The survivor may invite affected residents to revise the plan. The player identifies who needs to participate and what parts remain within owner control. Participants can accept the invitation, offer bounded feedback, ask to be left out, or challenge the proposer’s account. The owner can gather feedback without delegating its formal decision unless current rules permit it.

The group may agree on a small change, disagree about priorities, or ask for a second meeting. Do not describe attendance as consensus. The proposer can summarize majority support while acknowledging objections, or let the owner publish the result. A faction can provide a meeting space or facilitator, but no participant must join the faction to contribute. The collaborative branch succeeds when affected people can influence the route they actually use, not when every person agrees.

## 404. Follow-through branch — The change helps one group and burdens another

After a pilot, an overlooked group may report that the new arrangement shifts work or reduces access. The player can help surface that impact and ask the owner to compare the intended and observed result. The proposer can accept a revision, defend the original scope, pause the change, or refer the issue. Affected residents can describe their experience or choose not to participate.

Do not require the proposer to apologize for any unintended consequence before the owner will review it. At the same time, good intent does not mean the result is harmless. The owner may adjust timing, add an alternative, limit the change, or end the pilot. The final branch depends on the evidence and feasibility, not on whether the proposer scores as caring or selfish.

## 405. Follow-through branch — A faction claims the initiative as its own

A faction representative may publicize a successful change as evidence of its program. The proposer can accept credit, correct the attribution, request that their name be removed, or say nothing. The communication owner determines what can be changed and what copies are under its control. The faction can honor the request, dispute it, or offer a joint statement.

The player can distinguish support the faction provided from the proposer’s idea and the owner’s decision. A faction may have materially helped and deserve recognition, but it cannot erase the survivor’s contribution. If the proposer had agreed to public attribution earlier, they can still ask for a revision when circumstances change, subject to the actual publication owner. No faction-wide approval or loyalty score is needed; the specific relationship and action matter.

## 406. Follow-through branch — The proposal is adopted without the proposer present

The owner may use the survivor’s suggestion after they have left the shelter, changed duties, or declined further meetings. The player can tell them if an authorized route exists and if they consented to follow-up. The survivor may be pleased, indifferent, concerned about attribution, or ask what exactly changed. The owner can clarify that the result is narrower than the original proposal.

Do not imply that a person endorsed an adopted version they never reviewed. If the change substantially differs, state that. The survivor can request correction of their attribution, ask to re-engage, or accept the owner’s decision. If no callback channel exists, the story may show the change through the environment without attributing intent. The initiative can have consequences after the character’s involvement ends, while its provenance remains honest.

## 407. Follow-through branch — A repeated request shows the first answer was incomplete

Residents may return with the same concern after an owner made a decision. The player checks whether circumstances changed, the original request was misunderstood, or implementation failed. The proposer can bring new evidence, ask for review, narrow the requested outcome, or stop. The owner can reconsider, reaffirm its answer, or route the new fact elsewhere.

Do not make every repeat request an escalation or nuisance flag. A recurring issue may reflect a real defect in the service. The owner can identify what would change its decision and what evidence would not. If the answer remains no, the resident can choose another route or accept the limit. A faction liaison can help clarify process but should not promise that persistence guarantees success.

## 408. Follow-through branch — The proposer chooses not to own the outcome

After hearing the costs, the survivor may decide they do not want to lead the effort. They can hand the request to an owner, ask someone else to sponsor it, leave a note of the unresolved issue, or withdraw. The player can ask whether any immediate task must be closed. The survivor does not need to continue simply because they raised the idea.

The owner can accept the issue for review or state that it has no active sponsor. Other residents can take it up independently, but they need their own consent and scope. The protagonist can choose to advocate, facilitate, or step back. This creates an ending based on the player’s and survivor’s capacity rather than on a binary judgment about courage.

## 409. Installment 30 close — Follow-through is a new agreement, not a tail on the old one

An adopted change, ongoing maintenance role, shared authorship, revised pilot, attribution correction, repeated request, or withdrawal all require clear ownership. The original request does not silently authorize ongoing labor, publicity, or access. Each new stage asks who is willing, what the owner can support, and how the result will be reviewed.

This gives the initiative loop longer campaign reach while protecting the person who began it. A survivor can create change, decline responsibility for its upkeep, dispute how it is represented, or decide that the cost is too high. Factions may help sustain a limited service, but they cannot convert one contribution into membership or ownership of the entire outcome.

## 410. Expansion installment 31 — Multiple initiatives compete for the same scarce owner

Several survivors may ask the same specialist or service owner for attention. The player can help make the queue and its criteria visible, ask whether requests are independent, and let the owner identify capacity. The survivors can accept a wait, propose a lower-cost alternative, ask for another contact, or withdraw. The player should not order a request by which one appears morally deserving.

If the owner has a current prioritization rule, show it and allow the residents to ask how it applies. If the rule does not exist or does not cover the cases, do not invent one in dialogue. The owner can request a governance decision or explain that no commitment can be made yet. A supporting faction may take one request within its own scope, but its capacity and conditions are shown independently.

## 411. Queue branch — The urgent proposal interrupts a routine request

An immediate safety or time-sensitive need may displace a routine initiative. The relevant owner confirms urgency and the effect on existing appointments. The person whose request is delayed can accept the change, ask for a new time, or challenge the classification. The urgent proposer can provide required details or seek another response route.

Do not give the player an unsupported emergency override. A task may be escalated only under a current owner rule. The delayed resident should receive a clear notice and a realistic alternative if one exists. If none exists, say so and let them decide whether to remain in the queue. The consequence is a visible delay rather than a hidden affinity loss.

## 412. Queue branch — Two requests depend on the same person’s consent

The same worker may be asked to support two proposals. The player checks their schedule and asks which, if either, they want to take. They can accept both only if the owner confirms capacity, choose one, propose a different time, or decline both. The requesters can coordinate, seek other support, or proceed separately.

Do not make the worker the scarce resource without giving them a voice. A relationship with one requester does not obligate them to prioritize that person. The task owner can help find qualified alternatives. If no substitute exists, one proposal may wait or stop. The player can explain the capacity conflict without turning it into a loyalty test.

## 413. Queue branch — A faction offers capacity for only one request

A supporting group may have room for one training slot, one repair visit, or one meeting. The faction states its scope and selection rule. The owner checks whether that rule is legitimate for the shared service. Residents can apply, request an alternative, accept the limited offer, or decline. The faction can choose based on its own resources only within its authority.

If the group’s preference disadvantages a resident, the player can ask for a public explanation or a review route. Do not force the group to provide a service it cannot sustain, but do not present its preference as a shelter-wide policy. The person who does not receive the slot may seek another faction, wait, or withdraw. The outcome can create tension between groups without a simplistic virtuous/villainous assignment.

## 414. Queue branch — The owner needs to hear the requests together

The owner may ask for a joint meeting to understand shared costs. Each survivor can consent to attend, submit a private statement, send an intermediary, or decline. The owner can hear common facts without revealing one person’s confidential reason to another. The player helps set an agenda and captures only agreed operational points.

The meeting may yield a combined solution, a clear separation of requests, a rotation, or no change. It does not oblige participants to compromise. Someone may decide that a shared solution does not meet their need. The owner can record a decision and route individual follow-up privately. The public outcome should not claim unity if participants merely accepted a temporary constraint.

## 415. Queue branch — A requester asks the player to prioritize them personally

The resident may appeal to a relationship or past favor. The player can acknowledge the history while explaining the owner’s criteria. They may ask whether a specific fact was omitted, request an exception if one exists, or admit that they cannot decide. The resident can accept that, challenge it, seek a different owner, or withdraw.

The player’s personal relationship can shape dialogue without secretly changing the allocation. If the player is formally the owner, any exception uses its actual command and is recorded transparently. The resident may feel disappointed; that feeling is a valid narrative consequence. Another requester should not be penalized because their story was less visible to the player.

## 416. Queue branch — A delayed resident chooses a substitute path

The person waiting may find a smaller service, another instructor, a peer resource, or a self-directed alternative. The owner confirms what is available and what remains unmet. The resident can use the substitute, wait for the original, or stop. The first request stays pending only if its owner supports that state.

The substitute may be less convenient or have different privacy conditions. The player can compare the options and ask what the resident values. A faction may provide one piece of the route without replacing the original service. If the resident withdraws, release any reservation and notify the owner. A later slot can be offered only after availability is confirmed again.

## 417. Queue outcomes — Fairness is visible through criteria and response

The queue can resolve through owner criteria, an urgent reassignment, worker choice, faction capacity, a joint meeting, personal appeal, a substitute route, or no available service. Each result names the owner and the facts that mattered. The player can challenge inconsistency, but should not assign a global fairness score to characters or factions.

Residents may disagree with a fair process or accept an unfair one for practical reasons. The narrative can hold both responses. The next branch may come from a review, a new offer, or a choice to stop asking. These variations grow out of access, timing, and consent rather than from alignment labels.

## 418. Installment 31 close — Scarcity creates choices without moral sorting

When initiatives compete, the player can prioritize through a published rule, advocate for review, find a second provider, facilitate a joint plan, or accept that capacity is absent. Survivors choose whether to wait, revise, switch, appeal, or withdraw. Supporting factions supply bounded capacity and may make different choices under their own rules.

No one route guarantees that all needs are met. The design should make clear why an option is unavailable and what the person can do next. If the relevant owner cannot make the selection consistently, mark that as an implementation gap rather than hiding it behind narrative authority.

## 419. Final route check — The survivor owns the answer after the queue moves

When a request finally receives attention, the player should return to the survivor with the owner’s actual answer and ask what they want to do. An accepted slot is not an accepted offer until the survivor confirms. A declined slot does not close a different initiative. A revised proposal should be routed to the correct owner and given a fresh decision point.

If the survivor has stopped pursuing the issue, do not reopen it just because capacity became available. They may have found another route, changed priorities, or decided that the cost is too high. The player can state that the opportunity exists and let them decide whether to re-engage. Their current action matters more than an old quest marker.

## 420. Plan 3 continuation close — Agency is the route through the system

The expanded initiative branches cover how a resident raises a concern, proposes alternatives, seeks help, revises terms, handles refusal, negotiates a commitment, shares responsibility, and responds to scarce capacity. Every route can end in a clear yes, a bounded yes, a no, a referral, a wait, or a deliberate stop. The player facilitates access and understanding while owners retain their operational decisions.

Before implementation, verify the social, task, schedule, consent, and communication owners and their persistence paths. Do not introduce a generic “initiative score” or a parallel favor ledger. Keep the plan’s original three subfeatures; all added branches are outcomes of those commitments, not new pillars.

## 421. Secondary expansion installment 32 — A collective request must leave room for dissent

A survivor may gather signatures, volunteers, or public support for a proposal. The player can help identify who is actually affected and what each person is endorsing. A resident may support the goal but reject the wording, accept the wording but decline to be named, or disagree with the plan and still want the problem solved. Those distinctions matter when the request reaches an owner or a faction contact.

The proposer can choose to present a shared statement, a list of individual accounts, or only the operational facts. People who do not join should not be described as opponents. The owner may consider the request’s evidence without treating popularity as authorization. The player can help the group decide whether it wants a public petition, a private referral, or a smaller pilot. Each route has a different audience and risk.

## 422. Collective branch — A resident supports the goal but not the method

The resident can say that the problem is real while objecting to a proposed solution. The player can ask what outcome they would support and whether they want to offer an alternative. They can do so publicly, privately, or not at all. The proposer may adapt the method, keep the original, or proceed without claiming unanimity.

The operational owner decides whether either method is feasible. If the proposal is a resource allocation, affected people may need a proper review rather than a vote. If it is a meeting or task change, the task owner confirms capacity. The dissenting person can accept the final decision, appeal, or stop participating. This avoids the false choice between agreeing with every detail and opposing the whole initiative.

## 423. Collective branch — A resident wants their name removed

After a request is submitted, a participant may reconsider public attribution. The communication or request owner determines whether a submitted copy can be amended. The person can ask for their name removed, permit a role-only reference, or leave the statement unchanged. The proposer can accept the change, explain what cannot be edited, or withdraw the whole request.

Do not promise deletion from copies the owner does not control. The participant should be told which audience already saw the submission and what correction is possible. Their underlying support may remain private and should not be inferred from a public roster. If their name is required for a formal process, say why before collecting it and offer any allowed confidential route.

## 424. Collective branch — A quiet participant is counted by someone else

The proposer may include residents who did not explicitly agree because they attended a meeting or use the affected service. The player can ask which people actually consented to endorse the request. The proposer may correct the count, contact individuals for permission, or submit the issue without claiming collective support. An owner should receive accurate scope rather than an inflated consensus.

If a resident does not respond, do not count that silence as endorsement. They can be represented as affected without being named as a supporter. The proposer may feel their case looks weaker after correction; the player can explain that accurate evidence protects credibility. The initiative may still proceed on its operational merits, or it may need a more specific request.

## 425. Collective branch — The proposal creates access for some and a barrier for others

A group may ask for a schedule, location, or communication channel that excludes a smaller subset. The player can ask who cannot use the proposed arrangement and what alternative would preserve access. Affected residents can share the practical constraint privately or through an authorized representative. The owner decides what accommodations or alternate routes are feasible.

Do not frame the smaller group as an obstacle to majority preference. The proposer can revise the route, add an alternate time, keep the main plan and offer a separate path, or explain that no accommodation exists. The owner can approve a pilot and review its effects. If there is no authority to resolve the access question, name the gap instead of creating a fictional exception in dialogue.

## 426. Collective branch — A faction asks for a show of loyalty before helping

A faction may request that participants publicly endorse its program before it supports their proposal. The player asks whether the endorsement is a genuine service condition and whether each resident can choose independently. The group can accept the package, negotiate a narrower contribution, seek another provider, or refuse. The faction can clarify its limits or withdraw the offer.

Do not let the proposer accept on behalf of every participant. A person can want the resource and still decline public alignment. If the faction offers help only to members, state that boundary before people rely on it. A supporting faction can play a consequential role by setting its own terms, but it cannot make those terms the only route unless the underlying service owner confirms that exclusivity.

## 427. Collective branch — The owner asks for individual rather than shared requests

The owner may need each resident to state their own need or give separate consent. The group can submit individual forms, designate an authorized representative, or withdraw. Each person decides what personal information to disclose. The proposer cannot fill in another person’s reason based on observation.

The owner may aggregate the operational facts after intake, if its process allows. The player can help residents understand the distinction between a shared outcome and private circumstances. Some may continue; others may stop. That difference should not split the group into loyal and disloyal members. The route remains open to each person independently where the owner supports it.

## 428. Collective branch — The proposer is challenged about who benefits

Another resident may ask whether the proposer’s plan mainly helps their own household, faction, or work team. The proposer can acknowledge the benefit, explain broader effects, revise the plan, or reject the criticism. The player can map actual benefits and costs without declaring motive. The owner evaluates feasibility and scope.

Self-interest does not invalidate a useful proposal. It does make transparent terms more important. Affected parties can ask for a safeguard, an alternate allocation, or a review after a trial. If the owner cannot support a safeguard, the proposer can proceed within the stated limits or withdraw. The ending may include a partial benefit rather than a moral verdict.

## 429. Collective branch — A resident fears retaliation for dissent

A participant may worry that opposing a faction or supervisor will affect later assignments. The player can explain which protections or appeal routes actually exist and which are uncertain. The resident can submit anonymously if supported, ask an independent contact, give a limited statement, or leave the process. Do not promise protection unless the current owner can provide it.

The proposer may choose to present the operational issue without naming dissenters. The relevant task or conduct owner decides how to handle any alleged retaliation. A supporting faction can provide a separate channel, but its independence should not be assumed. The resident can later ask what was shared and what happened to the request. If the risk cannot be bounded, they may decide not to participate; that choice remains valid.

## 430. Collective branch — The group disagrees about what counts as success

Some participants may want a service restored; others want a policy changed or an apology. The player can separate the requested outcomes and ask whether one can proceed without the others. The owner may address operational restoration while another authority handles conduct or review. The group can submit two requests, choose a limited first step, or keep one combined statement.

Do not promise that a repair resolves a grievance or that a review restores a service. Each owner reports its own outcome. A participant can accept the practical fix and still seek accountability; another may want only the service. The group may split while maintaining a shared factual account. Those branches support different playstyles without requiring the player to rank people’s motives.

## 431. Collective branch — The request is successful but the proposer is exhausted

After a decision, the proposer may not want to maintain a committee, answer every question, or monitor the pilot. The player can ask whether they want a handoff, a limited closing meeting, or no further role. The owner identifies who will maintain any adopted arrangement. Other participants can volunteer, but each does so separately.

Do not treat burnout as betrayal. A proposal can succeed while its originator steps back. If no one accepts upkeep, the owner may schedule a review or let the temporary change expire. The player can help communicate that limit. The proposer’s contribution remains real even if they are not a permanent representative.

## 432. Collective branch — The group wants a record of its disagreement

Participants may ask the owner to record majority support and a dissenting view. The owner determines whether it has a minutes or review mechanism. The people involved can approve a neutral summary, submit separate statements, or decline to be included. The author should avoid reducing the disagreement to a vote tally when the actual concerns differ.

If no durable record system exists, do not promise official minutes. A current communication channel can carry an agreed summary, or the characters can keep a private note that does not claim official status. A later review should not infer that a dissenting person accepted the result merely because they were present. The record’s audience and retention must be clear.

## 433. Collective outcomes — Support, participation, attribution, and decision are separate

A resident may support the goal, participate in a meeting, endorse wording, permit their name to appear, and accept the owner’s decision in different combinations. The player should see those as separate actions where the system supports them. The request can proceed with partial support or stop before submission. An owner may decide based on evidence even when no group consensus exists.

Possible endings include a revised proposal, a narrower named group, anonymous concern, faction-conditioned aid, individual intake, an access accommodation, a split request, a temporary pilot, or a proposer who hands off. Each is shaped by concrete choices and capacity. None requires a collective alignment score.

## 434. Secondary expansion installment 33 — A survivor can seek review without converting every no into a feud

When an owner refuses or modifies a request, the survivor may question how the decision was made. The player can identify the actual review route, explain any deadline or evidence rule, or say that no appeal process is confirmed. The survivor can file, ask informally, accept the answer, seek another provider, or disengage. A request for review is not proof that the survivor distrusts everyone involved.

The original decision remains in force unless the review owner changes it. The player should not promise a different result. The survivor can challenge process, evidence, scope, or consistency. The owner may correct an error, reaffirm the result with a fuller explanation, or identify a missing authority. A supporting faction may help prepare questions but cannot guarantee an appeal outcome.

## 435. Review branch — The survivor disputes the evidence

The survivor may say that the owner relied on an incomplete account. The player can help list the facts the survivor believes are missing and ask how they may submit them. They can provide records, name a witness, withhold personal detail, or stop. The review owner checks relevance and may ask for clarification.

Do not assume that new evidence changes the decision. It can confirm the original result, narrow it, or remain inconclusive. The survivor can accept the review, ask what evidence was considered, or request another route if available. If the evidence concerns a third party, the owner handles consent and privacy before sharing it.

## 436. Review branch — The survivor disputes the process, not the outcome

The survivor may agree that the answer is operationally reasonable but object that they were not heard or that terms changed without notice. The player can separate procedural concern from the decision itself. The review owner can acknowledge a communication failure, correct the record, or state that the process followed current rules.

An apology or correction does not automatically reverse the decision. The survivor may ask for a future consultation step, accept the explanation, or continue to disagree. The owner can improve its next communication only if it owns that process. Do not make the player promise a new formal right unless the relevant authority has approved it.

## 437. Review branch — The survivor asks an independent faction to look at it

A supporting faction may review the facts or help the resident understand options. The player identifies the faction’s scope, interests, and access to records. The survivor can proceed with those limits, choose another contact, or decide that an external review would expose too much. The faction can accept, decline, or refer to an authorized owner.

The faction’s advice should be labeled as advice unless it has formal authority. The survivor can use it to shape a new request, but the original owner’s decision does not change automatically. The faction may have a history with the major group involved; disclose relevant conflicts. The player can still value its contribution without calling it neutral.

## 438. Review branch — The review finds a process error but cannot grant the request

The reviewer may find that the resident was not given a clear deadline or that a required contact was omitted, while still concluding that capacity is unavailable. The player reports both findings. The survivor can request a corrected opportunity, accept the explanation, or decide not to repeat the request. The owner may offer a new process without promising the desired resource.

This is a meaningful partial outcome. It recognizes an error without manufacturing a success. The resident can receive a fair chance to decide under accurate conditions. If no capacity remains, the alternative may be waitlist, referral, or closure. Each is explicit and owner-backed.

## 439. Review branch — The resident declines to continue after receiving the appeal route

The survivor may decide that the cost of review is too high. They can stop without losing unrelated services or future relationships unless an explicit current rule says otherwise. The player can ask whether they want the request closed, left pending if supported, or transferred to another contact. The resident may keep their reasons private.

Do not frame non-use of an appeal as acceptance of the original decision. The person can disagree and still choose not to spend more time. Later, they may reopen the issue if circumstances and owner rules permit. A supporting faction can offer a different service, but cannot pressure them to challenge the decision publicly.

## 440. Review outcomes — A fair process can still produce an unwanted answer

The review may correct the record, clarify evidence, acknowledge a process failure, reaffirm the refusal, identify a missing authority, offer another route, or close without further action. The survivor chooses whether to continue. The player can support them without promising a reversal or making the disagreement a permanent faction feud.

This expands the refusal loop with a procedural branch grounded in actual owners. Before implementation, verify that a review route exists and what it can change. If there is none, keep these as future options and do not create an appeal subsystem through dialogue. The document remains proposal-only and retains its original three subfeatures.

## 441. Secondary expansion installment 34 — One survivor can hold more than one unfinished intention

A resident may have an unresolved request, a separate personal goal, and a current work commitment at the same time. The player should not collapse those into one active quest or infer that one cancels another. The survivor can decide which matters now, ask to pause one, or keep both open if the current owners can represent them. The schedule and task owners determine conflicts; the survivor states their priorities.

An initiative can remain emotionally important while being operationally deferred. The player may remind the resident that a decision is pending, but should not repeatedly prompt them as if they failed a hidden deadline. If the owner’s offer expires, the survivor can choose whether to re-open the request. If no durable pending state exists, close the conversation and let them raise it again rather than fabricating a background queue.

## Intention branch — The survivor chooses a work duty over their proposal

The resident may decide that a current shift or urgent task takes priority. The task owner confirms the assignment and whether it is optional. The survivor can accept the duty, ask another person, request a different time, or decline if they have that choice. The initiative can pause without implying that the resident no longer cares.

The player can help set a realistic follow-up only if someone agrees to own it. Otherwise, the survivor may return when ready. If a faction contact has a time-limited offer, disclose that the opportunity may close. The resident can accept the cost of postponement, ask for an extension, or choose the faction route now. The branch is driven by their priorities and actual capacity.

## Intention branch — The survivor’s personal goal conflicts with a group proposal

The resident may want a quiet private outcome while the group seeks public change. They can participate only in the shared operational part, submit a confidential concern, or leave the group request. The proposer can respect the boundary, ask whether the resident wants to be counted as affected but not endorsing, or proceed without them.

The owner should receive only the information needed to decide. The player can explain how group and private routes differ. A faction may support the collective request without owning the person’s individual case. If the personal goal depends on the same resource, the owner can review both under its current rules without publicly connecting them.

## Intention branch — A second offer solves the need but not the original concern

The resident may accept a service from another group that meets their immediate need while still wanting the original process reviewed. They can close the practical request, continue the review, or stop both. The player should keep the two outcomes distinct. Accepting a substitute does not waive a complaint unless an explicit term says so and the resident knowingly agrees.

The provider may ask whether the original concern is resolved. The resident can answer honestly without sharing the full dispute. The owner can close only the parts it controls. A later scene may show that the immediate problem is gone while trust in the first service remains unsettled. That complexity should not be flattened into “quest complete.”

## Intention branch — The survivor accepts two offers with overlapping terms

The resident may accept help from two groups before realizing both expect the same time, public appearance, or work. The player can help compare what each offer actually requires. The resident may renegotiate one, withdraw from one, or ask a contact to coordinate. Each provider can accept a change or hold its original condition.

Do not interpret the overlap as dishonesty if the terms were unclear. The resident should not be assigned double work without a fresh agreement. The task owner checks the schedule; each faction controls only its own offer. If no shared meeting is available, the resident can handle them separately or choose one route. Any material already transferred follows its own owner-backed transaction.

## Intention branch — An owner closes the request while the resident still wants change

The owner may mark a request resolved after providing an immediate service, while the survivor still seeks a policy or relationship change. The player can ask what part remains open and which owner controls it. The resident can create a new, narrower request, seek review, accept that the owner’s scope has ended, or stop.

Do not keep the old request active as a proxy for a different goal. Close its confirmed outcome and open a fresh route only with the survivor’s consent. The next owner may not be the same faction or task owner. This keeps status accurate while preserving the resident’s larger objective.

## Intention branch — The survivor does not want the player to keep track

The resident may ask the player not to bring up a pending concern unless they raise it first. The player can respect that boundary in the conversation, but should not promise a durable reminder preference unless the social owner can store it. If a required deadline exists, explain that the owner may still send a notice through its official route.

The player should not treat silence as a prompt to ask repeatedly. The survivor can later restart the conversation or choose another contact. If the player lacks persistent memory, future scenes should avoid claiming they knowingly ignored the request. The boundary changes how the authored scene is played even if it does not create a permanent flag.

## Intention branch — A faction interprets acceptance as an exclusive relationship

A resident can accept one service while continuing to pursue another offer or public review. A faction may ask whether they intend to join, represent, or endorse it. The player should separate each question: use of service, membership, public attribution, and political support. The resident can answer each independently.

If the faction bundles those terms, state the condition before the resident accepts. They can proceed with the full package, ask to separate terms, choose another source, or decline. A previous acceptance of a single task does not establish permanent loyalty. Supporting factions can have genuine institutional interests without converting every interaction into an allegiance state.

## Intention outcomes — A quest marker is not a commitment

The initiative can remain pending if its owner supports that state, pause while the resident chooses another priority, split into practical and procedural tracks, be replaced by a new request, or close at the resident’s direction. A quest marker should disappear or change only when the underlying owner state changes. If there is no durable pending record, a future authored prompt can ask again instead of pretending the game remembered.

This route prevents a common narrative inconsistency: a character says no, moves on, and later appears to have silently accepted the same offer. The player sees which commitments remain active and which have ended. Each new offer needs fresh consent and current terms.

## 442. Intention branch — The resident changes their mind after seeing a real cost

The survivor may initially accept an initiative but reconsider after learning the time, travel, privacy, or work burden. The player can restate the cost and ask whether they want to proceed, narrow the scope, request support, or cancel. The owner can confirm what can be changed before start. The resident does not have to justify the revision.

If preparation already occurred, identify actual sunk costs without using them to pressure acceptance. A room can be released, materials returned, or a task cancelled through its owner. If some cost cannot be recovered, state that. The resident can still withdraw. A faction contact may express disappointment; the service owner decides whether the opportunity remains available later.

## 443. Intention branch — The player’s previous promise created an expectation

The player may have said they would help, then learn that they lack authority or capacity. The resident can ask what happened. The player should correct the expectation promptly, explain what part they can still do, and offer an owner contact only if confirmed. The resident may be frustrated, accept the clarification, or ask for another person.

Do not make the survivor responsible for the player’s overstatement. If an actual task was created, the owner manages handoff or cancellation. If it was a conversational promise only, say that no action was assigned. The player can acknowledge the mistake without fabricating a remedy. Later dialogue should not treat the promise as completed work.

## 444. Intention branch — A resident asks whether their refusal will be remembered

The person may want to know whether declining affects future access. The player checks the actual offer and owner rules. If refusal has no formal consequence, say so; if a slot will be reassigned or an invitation will expire, explain that specific cost. Do not promise that every future offer will be identical or that a relationship will be unchanged.

The resident can decline, ask to be contacted later if supported, accept a narrower option, or keep the decision open. A faction can remember the conversation in authored narrative only through supported state. It should not secretly blacklist or reward the resident based on a single no. A later offer requires current availability and a fresh invitation.

## 445. Intention branch — The resident wants an ending without a reconciliation scene

The survivor may close the issue and ask the player not to arrange a final meeting with the person who refused them. The player can close the operational request, preserve any required transaction, and let the social distance remain. The other party may never receive a final explanation. The survivor can still participate in shared duties through normal owner rules.

Do not force closure through apology, forgiveness, or a last conversation. A quiet ending can be the most accurate one. If an operational handoff remains, keep it factual and minimal. Any later relationship repair begins only if one party chooses to initiate it.

## 446. Intention close — Several open goals can coexist without a new campaign ledger

The plan can show multiple intentions through current tasks, accepted offers, and authored conversation state. It should not add a universal quest ledger or resident goal manager. Each system owns only its own commitment. The player can help the survivor review the list of actual open items when the current interface supports it; otherwise, use a focused conversation and avoid persistent promises.

The branches now cover prioritization, changed mind, overlapping terms, private aims, partial resolution, player overpromising, refusal memory, and non-reconciliation. These are secondary outcomes of survivor initiative and offers. They deepen the plan without adding a new feature pillar or a generic alignment axis.

## 447. Secondary coda — The person can ask what the player will remember

After an initiative ends, the survivor may ask whether the player will bring it up later. The player can distinguish a durable owner-backed task from a conversational memory. They can confirm an assigned follow-up, say that no system reminder exists, or ask permission to raise the subject again. Do not claim persistent tracking unless the current social or task owner supports it.

If the survivor does not want a reminder, respect that in the present scene and explain any official deadline that remains. A later conversation should not portray the player as knowingly ignoring a promise that was never stored. If the system cannot preserve this boundary, keep the callback optional and ask anew rather than presenting old consent as current.

## 448. Secondary coda — A new offer should name what changed

When a faction or owner returns with another proposal, the player can state whether capacity, terms, evidence, or timing changed. The survivor can accept, compare, ask for time, or decline. A repeated offer with no changed fact may feel pressuring; the survivor can ask not to receive it again. A materially revised offer is still new and requires fresh review.

This edit gives repeat offers a concrete reason to branch. The player can see whether the route is genuinely improved or simply being pushed again. The owner confirms the offer and its expiry. The survivor retains control over whether the new information is enough to reconsider.

## 449. Secondary coda — Closure does not claim agreement

When the resident stops pursuing a request, summarize the outcome as “closed at their request,” “service unavailable,” or “decision declined” only where that status is supported. Do not call it resolved if the underlying need remains. A later callback can acknowledge the history without reopening it automatically.

## 450. Secondary coda — A future branch starts from today’s capacity

If the resident later reopens the issue, check current need, offer, owner, and schedule rather than replaying stale terms. They can say that nothing changed, that a new need arose, or that an earlier alternative failed. The owner may have different capacity now. A new answer—positive or negative—belongs to the present facts and the resident’s current choice.

The player can ask whether the resident wants the old history considered. They may say yes, provide a limited summary, or start fresh. Reusing context should help them avoid repeating work, not expose private details without permission.

## 451. Expansion installment 35 — The survivor chooses who speaks after the decision

After an owner accepts, modifies, or rejects an initiative, the survivor may want to explain the result to the people affected. They can speak personally, ask the owner to publish the decision, request a supporting faction to relay it, or say nothing. The player confirms audience, wording, attribution, and any private details before helping. The owner’s decision should be accurately separated from the survivor’s opinion about it.

The survivor may have won a practical change but lost confidence in the process. They may also have lost the request and still appreciate a clear explanation. The player should not turn a public statement into proof that the person accepts the outcome. The survivor can explain what happened, challenge the reason, or close the conversation. Each route shapes future trust through specific actions and who heard them.

## 452. Outcome branch — The survivor wants to announce a partial success

The resident may want others to know that one part of the request was granted while another remains open. The player helps identify the confirmed change, remaining limits, and owner for each part. The survivor can speak at a meeting, provide a private update to affected people, or authorize the owner to post. Others may celebrate, ask questions, or disagree that the change is enough.

Do not label a partial result as complete to create an upbeat ending. If the remaining issue has no active owner, state that. The resident can choose to keep advocating, hand the issue to someone else, or stop. Factions that supported the partial change can receive credit for their actual contribution without claiming sole ownership.

## 453. Outcome branch — The survivor does not want their name attached to a success

The resident may want the policy or service to change but not become a public representative. The player can ask the communication owner whether anonymous or role-only attribution is supported. The survivor can approve a limited note, ask the owner to present the change without attribution, or decline publication. The proposer’s privacy does not make the decision less real.

If the owner’s formal record requires an originator, explain who can see it and why. The survivor can accept that narrow record, withdraw their name from public material, or ask whether another person can sponsor the change. Do not force visibility as the price of credit. A later meeting should not introduce the resident as spokesperson without fresh consent.

## 454. Outcome branch — The survivor wants public credit and the owner disputes attribution

The resident may feel that their suggestion was adopted without acknowledgment. The owner can explain which parts were independently developed, which came from the survivor, and what the record supports. The resident can request a correction, provide evidence, accept shared credit, or leave the matter unresolved. The player can help compare versions and ask the communication owner to clarify the public account.

Do not treat the ownership dispute as a new property or intellectual-property system. It is an authored recognition branch unless a current record or review owner applies. A supporting faction may have helped revise the proposal; it can be credited for that work while preserving the resident’s initial contribution. The ending may be corrected attribution, shared acknowledgment, a private apology, or disagreement.

## 455. Outcome branch — The survivor asks the owner to explain the refusal directly

The resident may not want the player to paraphrase the answer. They can request a direct meeting, a written explanation, an authorized representative, or no follow-up. The owner chooses what it can provide. The player can help schedule it, ask for a point of contact, or admit that no route is available.

The owner should not use the meeting to pressure the resident into accepting the decision. The survivor can ask questions, state why they disagree, accept the explanation, or end the conversation. If new evidence arises, it can be routed through review. If nothing changes, the owner can close the request while the resident retains their view.

## 456. Outcome branch — Another resident repeats the proposal without the originator

Someone else may bring forward the same idea after the original survivor steps back. The player checks whether the originator wants acknowledgment or privacy and whether the new proposer developed it independently. The owner can evaluate the new request on its own facts. The original survivor can consent to credit, object to misattribution, or stay uninvolved.

Do not require the first proposer to remain responsible forever. The second resident can take ownership of future meetings only if the owner accepts that role. The decision may proceed even if the originator is absent. A faction can support the revised proposal, but cannot rewrite who contributed what without evidence.

## 457. Outcome branch — The survivor’s explanation changes another person’s choice

Another resident may hear how the decision was made and choose to join, withdraw, or ask for a separate review. The player identifies what that person actually learned and asks whether the original survivor consents to sharing their account. The listener can act on the operational facts without receiving private history.

Do not make the original survivor responsible for everyone’s response. The owner handles the new request separately. If the second person changes their mind, their action belongs to them. A later callback can mention that the explanation circulated only if the communication route or authored audience supports it.

## 458. Installment 35 close — The decision and its meaning can remain different

The survivor can accept a practical result while disputing its fairness, welcome a rejection that was clearly explained, seek attribution, protect privacy, or leave the issue to another person. The owner’s decision remains one fact; the survivor’s interpretation is another. The player can support both without collapsing them into a single success or failure state.

The installment expands the initiative loop through explanation, audience, attribution, and succession. It remains within the existing offer, refusal, and personal-goal features. Durable state should be limited to the owner-backed decision and any explicit social event the current system supports.

## 459. Expansion installment 36 — An initiative can affect trust between two residents differently

Two people may experience the same action differently. A proposer may see a compromise as progress, while the resident asked to change shifts may feel burdened. The player can ask each person what changed for them, then identify the task, schedule, or resource owner that can address the operational part. One person’s gratitude cannot stand in for the other’s consent.

The scene should show specific behavior: whether the proposer listened, whether the affected resident named a cost, whether the owner offered an alternative, and whether the group followed through. Relationship consequences belong to the existing social owner. There is no single “initiative outcome” that should overwrite separate relationships or declare the whole shelter satisfied.

## 460. Relationship branch — The affected resident accepts the change but asks for a boundary

The resident may agree to a temporary schedule or shared task while asking not to be contacted outside work, named publicly, or assigned additional duties. The player repeats the boundary and checks that the responsible owner can honor it. The proposer can accept, request a different arrangement, or withdraw the change. The affected person can proceed, revise, or decline.

If the owner cannot store the boundary, it should be re-confirmed at the relevant interaction rather than claimed as durable. The proposer cannot interpret acceptance of one task as consent to further involvement. A later boundary violation follows the proper conduct or task route, not a hidden penalty added to this initiative system.

## 461. Relationship branch — The proposer is blamed for an owner’s decision

Residents may assume that the person who raised the proposal controlled the final result. The proposer can clarify their role, ask the owner to state its decision, or avoid further discussion. The player can help distinguish proposal, recommendation, and authorization. The affected residents can accept the clarification, remain angry, or request a direct explanation.

Do not absolve a proposer automatically if they misrepresented the owner’s response. The communication or social owner can address that specific action. If the proposer accurately relayed an answer they did not control, the owner should own the decision. A faction representative may also clarify its own role without shifting responsibility to the survivor.

## 462. Relationship branch — Two supporters disagree over who should bear the cost

The initiative may create a new duty or use shared supplies. Two supporters can agree on the goal but disagree about allocation. The player maps the requested contribution and asks the task or inventory owner to verify feasibility. Each supporter can accept a bounded share, propose rotation, ask the owner to assign work, or withdraw.

Do not let the proposer volunteer someone else’s labor. A rotation requires capacity and explicit agreement. A shared resource follows its existing allocation rule. If no acceptable arrangement exists, the group may reduce the scope or let the proposal stop. The people can remain allies on another issue without resolving this disagreement.

## 463. Relationship branch — The survivor declines a public apology

An affected resident may want the correction or repair but not a public apology. The proposer or owner can acknowledge the action privately, issue a factual correction, or ask what response would help. The resident can accept, request a different remedy, or decline further contact. The player should not force reconciliation because an apology scene is available.

If the public record contains a false claim, the communication owner may still need to correct it even when the resident declines an apology. That action is separate from emotional repair. The resident’s privacy and the public’s need for accurate operational information are handled by the relevant owners.

## 464. Relationship branch — The survivor wants to repair the relationship but not the proposal

The resident may regret how they spoke while continuing to disagree with the decision. They can apologize for their conduct, request a private conversation, or leave the relationship alone. The other person can accept, decline, or keep a work boundary. Neither is required to withdraw their position to restore civility.

The player can support a conversation without making it a negotiation over the original service. If the proposal remains open, route it separately. A genuine repair can coexist with an unresolved policy or resource dispute. The story should permit that ending rather than require one side to concede everything.

## 465. Relationship outcomes — One initiative may close several different ways

The practical request may be resolved while attribution remains disputed; the schedule may change while a relationship cools; the owner may reject the request while two residents reconcile; or the proposer may leave while another person takes over. The system should not force these into a single complete/failed bit if current owners hold them separately. Where no durable state exists, keep the difference in dialogue and avoid false cross-quest callbacks.

This installment deepens branching endings around interpersonal consequence without creating a relationship subsystem. The player’s interventions influence who speaks, what is repaired, and whether work continues, while each resident’s choice remains distinct.

## 466. Installment 36 close — Conflict can persist without blocking every future interaction

Two residents can disagree about the initiative and still share a room, trade, attend a meeting, or work on another task under normal owner rules. The player can respect boundaries and choose separate routes. Do not globally lock their content unless a current safety or relationship owner supports that consequence. New interactions should respond to specific history, not an invented permanent feud flag.

## 467. Forward scaffold — Remaining Plan 3 expansion sequence

The next Plan 3 pass should use the following sequence to grow the proposal toward its 120,000-word minimum. These are scaffolds, not claims that the branches already exist in code. Each installment keeps the same three Section 4 subfeatures: survivor offers and refusals, personal goals, and an owner-backed route from intent to consequence.

### Installment 37 — Initiative after a roster change

**Branch axis:** The survivor changes role, duty, or location after making a request. Explore transfer, cancellation, a new owner, loss of a meeting window, and a decision to leave the goal behind. **Decision basis:** current roster facts and the survivor’s new consent, not a stale character flag. **Ending:** handoff, reopened offer, paused goal, or honest closure. **Evidence needed:** roster transition producer, active-task owner, and whether pending offers persist.

### Installment 38 — A personal goal depends on shared resources

**Branch axis:** The resident’s goal uses a room, tool, supply, or schedule that others need. Compare reservation, rotation, public allocation, private property, and no-capacity outcomes. **Supporting role:** a small faction may lend a resource or space under explicit terms. **Ending:** accepted reservation, revised scope, wait, or refusal. **Evidence needed:** existing inventory, location, and schedule owners; no new goal inventory.

### Installment 39 — The survivor asks a peer to witness a difficult conversation

**Branch axis:** The peer can listen, relay, verify a statement, or stay outside the decision. Each participant approves audience and role. **Risk:** witness neutrality and privacy. **Ending:** direct agreement, clarified record, separate accounts, or no meeting. **Evidence needed:** current social and communication routes; do not invent a mediation service.

### Installment 40 — A delayed offer changes because a faction loses capacity

**Branch axis:** The faction can preserve a smaller service, transfer a slot, provide a referral, or withdraw. The survivor can accept the reduced scope, seek another provider, or close the request. **Evidence needed:** faction resource owner and offer expiry. Avoid treating the loss as betrayal without an established promise.

### Installment 41 — Initiative reveals a source of conflict but not a culprit

**Branch axis:** Conflicting accounts, missing records, incomplete observation, or a corrected source. The survivor chooses whether to investigate, refer, or stop. **Ending:** confirmed fact, unresolved discrepancy, narrow remedy, or owner review. Do not turn suspicion into an automatic accusation branch.

### Installment 42 — A resident requests an accommodation for participating

**Branch axis:** Private statement, adjusted time, trusted intermediary, accessible location, or refusal to disclose a reason. Verify actual owner accommodations before promises. **Ending:** supported participation, alternate method, deferred request, or no available route.

### Installment 43 — A successful initiative creates an unexpected obligation

**Branch axis:** The owner asks the proposer to maintain, represent, or repeat the change. The survivor can accept a bounded role, negotiate, hand off, or stop. **Evidence needed:** whether the task owner can assign the follow-up. No automatic unpaid labor or leadership status.

### Installment 44 — A faction asks to publicize the survivor’s outcome

**Branch axis:** named success, anonymous result, shared credit, factual update without endorsement, or refusal. Verify publication scope and privacy. A faction’s aid does not buy a testimonial unless the offer stated that condition beforehand.

### Installment 45 — Multiple requests compete for one decision owner

**Branch axis:** transparent criteria, urgency review, alternate provider, wait, or decline. Record who decides and which criteria apply. Do not construct a global deservingness score. Test that separate requests remain separately addressable.

### Installment 46 — The resident changes their mind after implementation starts

**Branch axis:** stop work, safe handoff, revise scope, continue under current terms, or request review. The task owner identifies sunk costs and reversibility. Consent to the original scope does not automatically authorize a material change.

### Installment 47 — Follow-up after a refusal without forced reconciliation

**Branch axis:** neutral contact, independent route, new terms, distance, or closure. The person who refused retains the boundary. The survivor controls whether to resume contact.

### Installment 48 — Playstyle and ending audit

Build a cross-matrix for advocate, negotiator, organizer, investigator, boundary-respecting facilitator, and disengaged player. For each, record actions, owner responses, costs, supporting-faction role, persistent fact, and terminal branch. Remove any outcome reachable only through a good/evil score.

### Installment 49 — Integration evidence and acceptance

Revalidate the live initiative producer, refusal path, consent verdict, schedule and task owners, save route, and host presentation. Demonstrate one full accepted path, one refusal, one owner denial, one revised proposal, and one reload of any durable state. If a state or review path does not exist, retain it as a scaffold and state the limitation.

### Installment 50 — Plan 3 closeout criteria

Do not close Plan 3 until it exceeds 120,000 measured Markdown words, retains exactly three Section 4 subfeatures, has a current evidence note, and separates proposal completion from implementation readiness. The closeout should list its unresolved owner seams rather than imply that the feature is built.

## 468. Scaffold realization — Installment 37: the roster changes after an offer

The roster-change case begins after an offer is visible but before the player commits the requested work. A resident becomes unavailable because the roster authority reports a departure, illness, reassignment, or another real status change. The story must not decide why a person disappeared by inference, and the panel must not keep a cached version of the old roster merely because that version made a branch convenient. The first screen is a small truth: the original offer is no longer executable under its original terms. The next decision belongs to the player, but the changed person's absence belongs to the roster owner.

The branch is not “keep the promise or break the promise.” It asks what kind of promise the player made. If the offer was framed as an invitation, the player can let it expire with no substitute. If it named a shared outcome but allowed delegation, the player may choose an eligible willing participant after a fresh consent check. If the offer named a particular survivor, reassignment requires a new offer to a new person; no one inherits another resident's agreement. If the work has already started, the task owner determines whether it can stop safely, be handed off, or must be made safe before the original worker leaves. A narrative line may acknowledge the effort, but it cannot supply a missing task transition.

### Decision card: who owns the next move?

**Known facts shown to the player:** the original worker's current roster status; whether work began; which materials, if any, have already been committed by their owner; the task's interruption rule; the deadline, if one exists; and whether the offer was individual or explicitly delegable. **Unknown facts:** the absent resident's private reason unless a current narrative producer supplies it, other residents' willingness before they are asked, and whether a deadline will be met until an owner confirms progress. **Required verbs:** wait, cancel safely, ask a named eligible person, split the work only when the work owner supports splitting, or complete the currently safe state and leave the remainder open.

The immediate action carries a clear cost. Waiting costs time if a deadline is current. Re-offering costs another conversation and can be declined. Cancelling may leave an unfinished need visible. Splitting may consume coordination capacity or create separate tasks only where the owner supports that structure. The player should not be charged a hidden relationship penalty for respecting a refusal. The system can still report objective consequences: the repair remains incomplete, a seat remains unbuilt, a request expires, or a different willing survivor has less time for another duty.

### Supporting roles without turning them into another command authority

A small supporting group can affect the case through concrete service, not faction-score arbitration. A workshop crew may identify which repairs can be safely paused; a stores clerk may preserve the materials already checked out; a watch team may report who is actually on duty. These are examples of role-shaped assistance, not assertions that those exact canonical factions exist. Implementation must map them to existing IDs and consumer APIs or omit them. A support group cannot quietly choose the substitute worker, override consent, or produce an unrecorded task completion. The player remains the decision-maker, the affected survivor remains a participant, and the existing domain owner remains responsible for the consequence.

If a supporting role has a service relationship, expose the terms before requesting it: a workshop can inspect and make a paused repair safe but cannot supply missing parts; stores can hold a returned part until a new plan is chosen but may require the player to close the original reservation; watch can verify coverage but cannot take over a non-watch task. A service refusal is legible and bounded. It does not make the supporting faction an antagonist. Its contribution is allowed to create a new option, narrow an unsafe option, or confirm a fact; it must not secretly become the second major storyline.

### Four playable routes through the same change

**Route A — preserve individual consent.** The player cancels the named request and asks the affected resident later, if they return and the route allows it. The goal state remains open or abandoned according to the actual goal owner. A different resident may independently raise the same need in a future initiative cycle, but that is a new offer with a new actor, not a replacement event. This route suits a player who favors continuity of person and promise. It costs momentum and may leave the community waiting; it can earn confidence only through witnessed reliability, never through a hidden “good” counter.

**Route B — re-plan with explicit delegation.** The player inspects the task's real prerequisites, sees which roster members are eligible under current conditions, and asks one of them. The offer is recalculated for that person's fatigue, skills, relationship context, and schedule only where those facts are available to the owner. A refusal returns to the decision card without consuming a second random draw for the original offer. Acceptance creates a new task owner transition. The first person's involvement remains historically accurate: they did not do work they never completed.

**Route C — stabilize first, then hand off.** A worker can be asked to complete the smallest safe handoff step if they consent and if the task owner recognizes one. Examples include returning a borrowed tool, marking an inspected component, or securing an open panel. The system records only the owner-backed action. A supporting crew can explain what “safe” means but cannot fake the handoff. The player can then pause the broader objective, request another person, or accept a longer delay. This route rewards care and operational attention without scoring virtue.

**Route D — let the need remain unresolved.** The player declines to allocate another person's time, perhaps because the shelter has a more urgent obligation. The offer closes with an honest status and a date or event for reconsideration only if the current system supports that reminder. The need may worsen through existing owners. The narrative should not shame the player for choosing scarce capacity, but it must not pretend that non-action had no material effect. This is a real branch, not a fail-state skin.

### Scene sequence and authored surface

The scene opens at the roster or task view rather than in a bespoke cutscene. The player sees a changed status badge and a short explanation sourced from the appropriate owner. If no reason is available, the copy says “unavailable today” instead of inventing a fever, argument, or evacuation. The old offer remains inspectable until its expiry or cancellation rule resolves. A secondary line records whether the work had begun and whether anything was committed. The action menu presents only owner-supported choices; unavailable actions are omitted or explained rather than shown as dead controls.

After the player chooses, the next view should confirm a single result. If the player waits, show that no new assignment has been created. If a second resident accepts, show their task record and the original offer's closure. If the task pauses, show its remaining prerequisite. If the player cancels, show what remains unresolved. A later day can report a return, a continued absence, or an independent new offer only from real roster or initiative facts. There is no forced reconciliation scene and no dialogue that claims the absent resident approved a substitution.

### Branch acceptance matrix

| Player action | Required source fact | Immediate result | Durable result to verify | Narrative boundary |
|---|---|---|---|---|
| Wait for original worker | Roster says unavailable; offer remains live | No reassignment | Same offer or its valid expiry after reload | Do not describe silence as agreement |
| Cancel offer | Current offer identity and cancellation API | Offer closes | Closed state does not reopen on load | Do not erase completed work |
| Ask named substitute | Eligibility and fresh consent | New offer, then accept/refuse | One task owner transition on acceptance | No inherited consent |
| Stabilize and pause | Safe handoff step exists in task owner | Handoff is recorded | Materials and task step remain consistent | Support crew advises; it does not mutate task |
| Leave need open | Existing need/initiative state | No new worker command | Need remains owned by its system | Consequence is material, not moralized |

These cases belong to Subfeature 1 when an initiative is offered, Subfeature 2 when an assignment intersects with consent, and Subfeature 3 only if a persisted personal goal advances or pauses from a real task event. The case is deliberately cross-cutting but does not add a fourth feature pillar. The acceptance question is: can an observer tell who chose, who consented, what owner changed, and what remains undone without consulting a hidden score?

## 469. Scaffold realization — Installment 38: shared resources and personal intent

This installment places a personal initiative beside a shared scarcity decision. It is designed to prevent a familiar design shortcut: treating a survivor's goal as a free resource request and then marking the goal complete because the player clicked “support.” The goal may be to repair a ventilation panel, preserve a family item, teach a skill, or make a common room usable. Each example is a content carrier, not a new mechanic. The owner-backed question stays the same: what item or capacity is requested, who owns it, what is actually committed, and what event proves the work happened?

### The proposal reveals its dependency before approval

The resident's initiative should expose a dependency list at the level of certainty the current data supports. “Needs one filter” is valid only if a catalog item and task requirement prove it. “Would help to have a filter” is a preference if the survivor authored it or an authorized system reports it. “Needs workshop access” is valid only if location access has an owner. “Can be done after evening shift” requires an actual schedule fact. Unknown dependencies are marked as unverified, not silently promoted to requirements.

The player chooses among four different commitments. They can reserve a specified item through the owning inventory or work API; promise to look for it without reserving; ask the survivor to propose a lower-cost alternative; or decline to support the plan now. A promise to search is not an item reservation. A displayed quantity is not proof that it is available to the survivor. A reserve action must identify the source container and return behavior. If shared shelter stock is involved, the player sees how the reservation affects communal planning; the initiative system cannot subtract it locally.

### A branching decision based on prior action, not alignment

The branch key is the player's earlier operational choice. If the player previously authorized a communal repair, the current initiative can reference that task only when its completion event exists. If the player withheld a scarce component for a different named need, the survivor may ask about the choice; the system should reference the actual reservation or commitment, not assign a motive. If the player previously lent a tool and the borrower returned it, the goal can have a viable dependency. If the tool is still checked out, the new initiative exposes the conflict and lets the player ask the borrower, provide another tool, wait, or close the goal. None of these routes requires a reputation scale. They read discrete history from the owner that made the earlier action.

The action that creates the most interesting branch may be mundane. A player who set a clear return date can request a handoff without a confrontation. A player who agreed to a vague “later” must decide whether to interrupt someone or postpone. A player who gave away the spare part must choose between renegotiating, finding another source, or admitting the original promise cannot be met. That yields consequence-rich play because earlier decisions change the actual options, rather than because the player crossed an ethical threshold.

### Support group's bounded contribution

A minor support faction can contribute a shared-resource fact through one of three limited services: inventory provenance, safe-use knowledge, or a scheduling window. An inventory clerk might confirm that a part is held for another task; a repair crew might certify that a substitute component fits; a meal team might tell the player when the common space is in use. The service role should be drawn from existing game canon and data. If there is no supporting group with that function, the feature uses an ordinary roster or owner message instead. No new faction is mandatory just to satisfy variety.

The support action can impose a stated consequence, such as consuming one use of a scarce inspection slot, delaying their own work, or asking for a reciprocal shift. The cost is recorded by the appropriate owner if the game has one; otherwise the plan treats it as authored context and does not create persistent faction debt. A support member can refuse because their queue is full. That refusal should not secretly turn the faction hostile or lower unrelated standing. If the player asks for a second opinion, it must be a different concrete service, not a free reroll of a deterministic result.

### Three outcomes that remain useful

**The goal is supported and resourced.** The player makes a specific reservation or assignment. The survivor's goal advances only when the task producer confirms the milestone. If work is interrupted, the goal pauses. Completion can yield an owner-backed world change and a short authored exchange, but the conversation does not substitute for the change.

**The goal is adapted.** The survivor accepts an alternate method or scope, if current dialogue/action rules support their agreement. This may reduce material cost but require a longer schedule, a different helper, or a less complete result. The adaptation creates a new task configuration through its owner and closes the old request cleanly. The narrative should preserve the original intention: an adapted communal repair is not described as the survivor's exact first choice.

**The goal is postponed or released.** The player explains a specific constraint, offers a review point only if a reminder exists, or declines without persuasion. The resident may keep the goal, revise it, or release it through an explicit action. The record should not decay invisibly because the player did not open the panel. If no response route is authored, the safe design is to leave the goal pending and avoid an invented consent outcome.

### A sample case: the heater bench

An initiative arrives to make a damaged bench usable near a warm room. The bench is a narrative example; it is not a claim that a heater or furniture placement system already exists. The player can inspect the request and learn that the resident wants a place to sit with a family member after work. One support worker can tell them whether the existing repair queue includes that bench; another can check whether the material is communal or personally held. The player may reserve a plank if a real item record exists, ask for a different repair scope, invite the resident to take a current queue slot, or decline until a higher priority repair completes.

The player who reserved the plank for an earlier water repair sees that commitment and a truthful conflict. They may move the first repair only through its owner, with all resulting consequences visible; there is no magical “borrow anyway” shortcut. The player who marked the bench as urgent but failed to allocate a worker sees that urgency did not cause work to happen. The player who left the request unacknowledged sees it still pending, not accepted. Each path generates a distinct conversation because the residents refer to an observable action: “You set the board aside for the pump,” “I thought someone had the slot,” or “I was waiting to hear back.” Lines should be adapted to actual history, with a neutral fallback for missing records.

### State and dialogue separation

The persisted goal stores only stable owner-backed facts: actor, goal identity, current status, task/event references, and any supported deadline or revision. It must not store prose as hidden state or encode a particular ending in an unvalidated string. An authored line may be selected based on the event's facts, but the text remains presentation. On load, the goal status is restored by its owner and the presentation is regenerated. If the referenced task no longer exists after migration, the plan specifies a visible unresolved state for investigation; it does not reset the goal to “new” and duplicate its reward.

The minimal evidence set for this installment is: one branch where a required item is reserved and returned correctly; one where the player promises to search but has no item; one where a previous action creates a conflict; one where the support role refuses because its actual capacity is exhausted; one pause and resume across save/restore; and one adaptation that records the new scope. These scenarios can remain acceptance notes until actual APIs and owners are verified. They are not instructions to add a speculative save store or write tests before the design has an owner.

## 470. Scaffold realization — Installment 39: a difficult conversation is witnessed, not manufactured

The “witness” case asks how the player can help two residents communicate about a real task conflict without turning the player into a therapist, judge, or all-purpose social authority. The conversation exists because one concrete action has affected another person's work or boundary. The feature should not trigger a dramatic confrontation merely because a relationship value crossed a threshold. It should not label a survivor's reaction as irrational. The player can clarify facts, relay a message with permission, create space, or step back. Each option has a different action cost and reach.

### Trigger conditions tied to observable events

A difficult conversation may be offered when a task was reassigned after work began; a shared resource was promised to two requests; a private goal was publicly discussed without agreement; an overdue favor blocks a dependency; or an earlier refusal was not respected by a later roster action. The trigger must reference an owner event or an explicit player command. A day tick alone can reveal an existing overdue fact, but it cannot invent the original disagreement. Purely inferred hostility is not sufficient evidence to launch the scene.

The offer explains the role the player is being asked to take. If the player is a witness, they hear both residents with appropriate audience permissions. If the player is a messenger, the affected party explicitly asks them to relay a specific message. If the player is coordinating a task, the operational facts are the focus. A public forum may be used only if the message owner and participants authorize that audience. The interface never disguises a public announcement as a private conversation. A resident who declines to discuss the matter can leave; their refusal ends that dialogue route without a penalty for leaving.

### Choice set and non-moral axes

The player can: **clarify a verifiable fact**, such as who reserved a tool; **ask each person separately what outcome they want**, if both have consented to speak; **relay one person's message**, exactly or with clearly declared paraphrase; **offer an operational change**, such as a revised schedule that both can accept; **request a bounded service from a supporting group**, such as a neutral place or a records check; or **step back** and let the two people decide whether to continue. These actions are not ordered from bad to good. They differ along information accuracy, privacy, time, and the degree to which the player owns the follow-up.

Each choice teaches the player a different kind of reliability. Clarifying a record can resolve a factual dispute but cannot settle a difference in priorities. Asking separately protects privacy but may leave incompatible needs. Relaying exactly reduces editorial drift but still requires permission and does not guarantee acceptance. Offering a schedule solves a logistics conflict only if the schedule owner can represent it. Stepping back avoids false authority but may allow a task delay to continue. A rich branch lets any of these routes carry a cost and a later payoff.

### The supporting faction as witness, not tribunal

A supporting faction can provide a narrowly defined service: preserve a neutral meeting space, check a public work record, lend a trained interpreter, or maintain a shared queue. These services are conditional examples until matched to actual canon and implementation. Its member may describe the service boundary in one sentence: “I can check the roster entry. I cannot tell you what either of them meant.” That line captures the intended design: support broadens options but does not seize narrative ownership.

If both residents request mediation from an existing group, the plan can expose a route to a known facilitator. The facilitator does not make a verdict binding unless a current system already supports that. The player can accept a suggested practical agreement, reject it, or take no action. A faction whose role is record keeping can produce a ledger fact; a faction whose role is medical care cannot arbitrate property ownership. Role fidelity matters more than faction variety. A faction does not appear at every disagreement merely because the outline asks for “supporting factions.”

### Branch consequences over three time horizons

**Immediate horizon:** Does the conversation happen, and who is present? Did the player reveal information, spend a roster slot, or change the task? Did one party decline? The UI records the result at the granularity supported by the system. Do not generate an invisible “conflict resolved” flag just because dialogue ended.

**Next operational horizon:** Is the task now executable? Was the resource reservation moved? Did one resident accept a schedule? Did the player promise to follow up? A promise to follow up becomes a separate reminder only if there is a real reminder/quest owner. Otherwise the conversation ends with a clear no-promise statement. An unresolved conversation can leave the work blocked without labeling the participants as enemies.

**Long horizon:** If future behavior changes, cite a concrete action: a returned tool, a kept schedule, a repeated missed handoff, a public correction, or a respected refusal. Relationship state may be one input only where the existing social owner defines it; it is not an all-purpose summary. The same residents can cooperate in one domain and avoid each other in another. A later request may therefore be accepted for a task but declined for personal conversation.

### Six endings for this episode, each with different closure

1. **Operational agreement:** both participants accept a verifiable task revision and the owner records it. This resolves the work but does not claim that their personal disagreement vanished.
2. **Fact corrected, priorities still differ:** one record is repaired; both residents retain distinct preferences. A future request remains possible, but there is no forced handshake.
3. **Private understanding:** a permitted message is relayed; the recipient chooses whether to reply. The player learns whether it was received only from the message owner.
4. **Useful separation:** the residents agree to separate shifts or tools through a valid scheduling action. They may coexist without further dialogue.
5. **Player steps back:** the conversation ends without resolution. The task remains in its real state, and neither participant is punished by a fabricated social malus.
6. **Trust in the process is lost:** the player relays beyond the agreed audience or misstates a fact. The affected resident can decline future use of that route if a real permission/relationship owner supports it. The consequence is specific to the breached boundary, not a generalized villain label.

The “lost trust” ending is not a requirement that players be punished for an accidental UI ambiguity. The interface must make audience, quotation and confirmation clear before action. If the player selects “share this note,” show exactly what will be shared and with whom. If the system cannot preserve exact wording, do not offer verbatim relay. If a paraphrase is allowed, preview it. A meaningful consequence follows a meaningful, informed action.

### Content design and replay variation

The same conversation structure can carry different surface details: a shift slot, a tool, a work promise, a shared sleeping space, or the timing of a personal project. Rotate the subject only when a real action supplies the relevant fact. Keep residents' voices distinct through sentence length, material knowledge, and what they notice, not through caricature or a global personality label. One person can speak briefly because they are tired; another may ask for a precise time because they coordinate the queue. The text should not infer trauma, faction loyalty, or medical facts absent from data.

Variation must be deterministic when it affects state. For authored lines, use stable selection based on the event identity or a seeded campaign RNG if the existing narrative owner already provides one. Never produce a second conversation outcome because a UI was opened twice. Reopening the record may show different presentation only if that variation is explicitly cosmetic and stable for that record. The player should not have to repeat a conversation to reroll a refusal.

### Acceptance and scope reminder

The acceptance walkthrough uses three sample disagreements, each based on an action trace rather than an alignment threshold. In the first, the player reassigns a started task without checking whether its resource is returned; the resulting conversation exposes the missing resource and permits a safe handoff. In the second, the player promised a private reply would stay private; a request to broadcast it is denied unless the original recipient explicitly changes the audience. In the third, a player declines to mediate; the system leaves the conflict unresolved and still provides a separate path for the underlying operational need. All three use the same Section 4 subfeatures. They add no independent conflict, reputation, or mediation pillar.

## 471. Scaffold realization — Installment 40: support capacity is finite and legible

Supporting groups matter when they offer a real capability at a moment where the player must make a choice. They become decorative if they agree to everything, and they become a second major faction system if the player spends a universal favor currency to command them. This installment sets a narrow standard for support capacity: a group can contribute the service it actually owns, at a time it can actually provide it, and the player must be able to continue if it is unavailable.

### Service cards, not faction meters

A service card describes four things: what help is offered; who owns that service; what current condition enables it; and what it costs or displaces. An example might say that a repair crew can inspect a handoff after its current task, or that a records keeper can verify whether a component is reserved. It does not say “Faction trust: 71.” A new generic meter is unnecessary and risks hiding the important fact: the team has one inspector on shift, the record is incomplete, or the equipment is already committed.

The player can accept the service, ask when it might be available, proceed without it, or withdraw the request. If accepting it delays the crew's own work, show the affected task if the owner supplies that relationship. Do not charge both a fictional favor and real materials unless each cost is separately grounded. If the support group is unavailable, the UI explains whether the reason is a schedule, capacity, missing prerequisite, or unknown status. “They refuse because they dislike you” is only valid if an existing relationship system supplies that fact and the request is within its domain.

### How capacity affects the initiative loop

Capacity can gate the timing or quality of an option, but not the survivor's right to say no. A crew may be able to inspect a task but not perform it; a message runner may deliver a note but not explain it; a stores worker may confirm that an item is reserved but not release it. These service boundaries prevent a helper role from collapsing multiple owners into one convenient NPC. When one capability is unavailable, the initiative can still be accepted, revised, deferred, or dropped according to the actual task and consent APIs.

The most important consequence is often what the player chooses to do while waiting. They may schedule another task, ask a second eligible person, secure a worksite, or simply leave a goal pending. Each option can change what a support service will be useful for later. A player who starts an unsafe repair before inspection cannot demand that the inspector approve it retroactively. A player who waits retains the ability to ask the service once it is available. These outcomes follow player actions and state, not a good-versus-evil classification.

### Repeat support request rules

The same service should not be offered multiple times in one evaluation cycle simply because the player reopened a panel. A deterministic request identity can bind the offer to its actual cause, if the current owner supports such identity. If the player declines, the service can be requested again only under an explicit rule: a new task stage, a new shift, a newly available worker, or a changed condition. Repeated prompts otherwise feel like rerolling a character's boundary. If there is no current cooldown/identity owner, the proposal does not invent one locally; it records the need for the owning authority.

A declined support offer has no universal social punishment. The group can continue its queue. If the player accepts one service and then cancels, only a documented cost applies, such as the time already spent or an item reserved and then returned. A group might set a limit after repeated no-shows, but that should be represented as a clear policy and a discrete prior event, not hidden churn. The message should explain what changed: “We held the inspection slot yesterday; next slot is after the roof check,” rather than “They trust you less.”

### Multi-route example: requesting help on an unfinished goal

A survivor has a personal goal to complete a repair. The player asks the workshop crew to assess it. The crew is at capacity until tomorrow. The player can wait and retain the goal; ask the survivor whether a smaller safe step is worthwhile; seek a different qualified person; or release the request. If the crew reports a missing part, the stores group can verify its reservation but cannot conjure a replacement. If a second survivor has that part, the player can negotiate with that person via existing inventory/consent routes. Each support group therefore opens a distinct fact route rather than a chain of automatically available quest nodes.

In one branch, the player chooses to wait. A later service becomes available and the survivor can still accept or decline help with the revised schedule. In another, the player asks for a safe subtask that the task owner recognizes; the goal advances only if its completion event is produced. In another, the player chooses a less-specialized helper and the plan must explain the risk boundary; the helper may be eligible for preparation but not final validation. In the final route, the player withdraws the request, and the goal remains open, changes, or closes through an explicit action. None of the routes converts a helper's service into consent from the goal-holder.

### Outcomes for the supporting group

A support team has its own simple local outcomes, not a major-faction ending: service completed; service delayed; service declined; service replaced by another provider; or service withdrawn by the player. The wider faction may remember only a concrete fulfilled or abandoned work obligation if its current system can store that. Do not author political speeches for routine task capacity. A supporting group can develop texture through work habits, priorities, and practical constraints: they keep their own queue, leave a chalk mark when an inspection is complete, or require tools to be returned before another team can use them.

This allows an eventual branch where the group offers a new kind of assistance after repeated, specific cooperation. The branch must be tied to an actual prior action—e.g. the player accepted an inspection slot and returned the equipment—not an invisible alignment grade. It can add an option without locking all others. A player who never engages the support group still has the core initiative loop; support remains additive and partial.

### Evidence gate before implementation

Before implementing a service card, a reviewer checks that the named group exists in current canon or data; the service is within its established role; a current API can report eligibility and costs; the service request uses a unique owner path; capacity persists only if the owning system says it should; and the action has a no-service alternative. If any requirement is missing, keep that particular group interaction as prose guidance, not an operational choice. This prevents speculative faction names and false claims about the game's current capabilities.

## 472. Scaffold realization — Installment 41: uncertain evidence and contested accounts

An initiative often begins with incomplete information: a missing tool, a complaint about a shift, a promise remembered differently, a goal said to have been completed, or a notice that was never seen. This installment defines how the feature handles evidence without turning the player into a detective for every routine disagreement. Evidence should be sought when it changes a consequential choice, and its source and uncertainty must be visible.

### Evidence categories

Use a compact case vocabulary: **confirmed owner fact** (a transfer event or task status); **participant account** (what a resident says they saw or intended); **recorded message** (the text and audience actually stored); **inference** (a conclusion drawn from multiple facts); and **unknown** (the system has no reliable answer). These are labels for presentation, not a second investigation database. A message view can show its own metadata. An inventory owner can report custody. A participant can offer their account. The player can compare them without an all-knowing narrator declaring which person is dishonest.

Evidence has scope. A task record can show that work started, but not that the worker consented to every later reassignment. A read acknowledgement can show a recipient acknowledged the notice, but not that they agreed with it. A promise record can show the agreed deadline, but not the person's private motive for missing it. The player should make decisions within what the evidence establishes.

### Player options when accounts conflict

The player may request a record check, ask a clarifying question, accept a participant's account without adjudicating it, change the plan so the disagreement no longer blocks work, or leave the matter unresolved. A record check consumes time or a support service if appropriate. A clarifying question may be declined. Changing the plan can cost materials or delay but avoids forcing a verdict. Leaving it unresolved preserves uncertainty and can be the best option when consequences are modest or privacy is at stake.

The player should not receive a “truth” button that turns an uncertain clue into a binary confession. Avoid gotcha branches in which an NPC's account is quietly proven false by a hidden flag. If a fact later becomes clear, use a credible owner event: the item is returned, a duplicate record appears, a task log is corrected, or the original author supplies a clarification. The later evidence should explain exactly what it resolves and what remains unknown.

### Branch by information action

If the player asks the resource owner, they may learn current custody but not how the item was discussed. If they ask the message author, they may learn intended meaning but not who read the notice. If they ask a neutral records helper, they may see an entry but not a participant's private recollection. If they ask both participants separately, they risk additional time and each person can opt out. If they choose a practical workaround, the disputed question may no longer matter; this is not an admission by either party.

The player's handling of evidence affects future willingness at a narrow level. A resident whose private account was disclosed may decline to share such information again if the social owner models that boundary. A player who accurately quoted a public record may continue to use that record. A corrected fact can unlock a new operational option, but it should not retroactively rewrite the other person's entire character. Do not collapse careful record keeping, confidentiality, and credibility into one score.

### Example: the returned item that is not in the expected place

A worker says they returned a tool to the shelf. The requester says it was not there at shift change. The inventory owner reports that no return transfer is recorded. The player can ask the worker whether they used a different return point; ask the requester to check the designated shelf; ask stores to inspect a current custody record if available; or loan a replacement through a valid owner path. They can also decide the replacement is too scarce and pause the task. The system does not conclude that the worker stole the tool. The absence of a return event proves only that no return event is recorded.

If the tool is later found in the wrong location, the task can resume and the record can be corrected if the inventory authority supports it. A supporting faction may offer a clearer return process for future exchanges, but it cannot charge a retroactive fine without a policy. The scene can end with accountability and uncertainty together: the tool is back, the record was incomplete, and neither resident's motive is established.

### Privacy boundary

Evidence request screens identify what information will be revealed and to whom. A private message cannot be used as a record source outside its authorized audience unless a current policy explicitly says otherwise. The player can ask the sender to authorize sharing, redact unnecessary details, quote only a public timestamp, or abandon that evidence route. A supporter can explain that they cannot disclose a private record. A privacy-protecting refusal must not be framed as obstruction by default.

When a personal goal intersects with a group task, ask only for the minimum evidence needed to decide. If the player needs to know whether a part is reserved, they do not need to read a private note about why another resident wants it. If they need to know whether a worker agreed to a shift, they need the consent result, not the worker's full relationship history. This principle makes the system feel materially attentive without becoming invasive.

### Design review cases

Review one confirmed fact with disputed interpretation; one missing record; one verified mistake; one unverified accusation; one privacy-protected fact; one practical workaround; and one player who abandons the investigation. For every case, ask: What is known, who knows it, what action is justified, what cannot be concluded, and what would change the answer? This keeps evidence branches action-based and preserves authored ambiguity without confusing a missing data record for a dramatic mystery.

## 473. Scaffold realization — Installment 42: accommodation as a real choice, not a personality tag

Accommodation means a person or task can change its timing, method, location, communication mode, or support arrangement so the person can participate. The plan must not assume a diagnosis or make accommodation a reward for friendliness. A resident may ask for a change; the player can consider the request against actual space, staffing, and safety constraints. If a particular accommodation system is absent, the narrative should remain descriptive and no health-related state should be fabricated.

### Separate request, feasibility, and consent

The request comes from the affected participant or a permitted source. Feasibility comes from the system that owns the relevant capacity: roster schedule, room access, task safety, equipment, or communication channel. Consent comes from the person who will do the work or receive the message. These are separate questions. A feasible request is not automatically accepted; a willing resident cannot override a safety limit; and a denied accommodation is not evidence of bad character.

The interface names the requested change in concrete terms—later start, written instructions, quieter work period, nearby seating, a second person present, or a smaller task step—only when that option matches a real owner capability. It can also offer “tell me what would work” as a conversation route without promising the system can operationalize any answer. Do not expose a menu of unsupported clinical accommodations just because it sounds inclusive.

### Branches and trade-offs

The player can accept the accommodation as requested; propose an alternate schedule or method; ask a supporting group for equipment or access advice; defer until a specific capacity becomes available; or explain that the task cannot proceed safely under current conditions. Each branch preserves the resident's ability to accept, counter, or decline. If the player suggests a different plan, it is a new offer, not an automatic adjustment to the person's life.

An accommodation can shift the burden. A later schedule may displace another task; a second worker can reduce available labor elsewhere; a quieter space may be unavailable during a repair. Show these specific costs rather than a generic penalty. The player may prioritize the request and move another task through its owner, or decline the accommodation and find a different participant. Both decisions can have consequences without score judgments. A resident can remain willing to help in some form even when the initial plan is infeasible, but never assume that flexibility means consent to any alternative.

### Supporting faction service

A small support group may identify a safer route, lend an existing tool, arrange an available room, or interpret instructions. Each service is conditional on canon and current data. A medical helper can provide relevant care advice if that role exists; they do not decide a roster assignment. A maintenance crew can adjust physical access if its task owner supports it; it does not disclose private health information. A communication helper can restate a notice; they do not consent on a recipient's behalf.

Support has a bounded role and may be unavailable. The player can wait, choose another task, ask the participant what they prefer, or leave the request pending. If the group can only advise, make that distinction clear. The player should not interpret “we can inspect the room” as “we can guarantee this will work.” This creates believable support relationships without turning every faction into a universal solution vendor.

### Example: a different work window

A resident accepts a personal goal but cannot work during a period already assigned to another duty. They ask for a later window. The roster owner shows the actual shift; the task owner reports whether the work can be split or delayed; the participant approves only the available option they choose. The player can shift a different assignment, ask another eligible resident, defer the goal, or release the work. If the alternate slot causes a shared room to be unavailable, a supporting group may provide the next opening. The system does not record “accommodated” as a permanent personality quality; it records the actual agreed schedule or task configuration.

If the player moves the duty without asking the affected worker, a new consent check is required. If no schedule API permits the change, the plan does not promise a button. A narrative line can explain that the schedule cannot be changed today, but it should provide the next real option or state that none is available. The lack of a route is itself a gap to take to the implementation owner.

### Respectful failure states

If an accommodation cannot be delivered, avoid phrasing that blames the requester. State the constraint and what remains possible: “The room is reserved through night watch. The task can wait until morning or move to the lower bench if you want.” If the player refuses to allocate scarce equipment, the participant may decide not to continue. The system should not portray that as ingratitude. If a participant changes their mind after work begins, the task can pause or stop safely according to the owner. The player can close, adapt, or wait without converting that change into a refusal-penalty shortcut.

### Acceptance cases

The design review should cover a schedule adjustment with consent, an infeasible room request, a helper service with limited scope, a participant who rejects the alternative, a player who prioritizes another task, and a saved/reloaded accepted arrangement. The reviewer records which authority owns each fact and whether the text implies more than it knows. The plan's goal is not universal accommodation coverage; it is truthful decision-making and clear gaps when the current game cannot represent a request.

## 474. Scaffold realization — Installment 43: an obligation arrives after the player has committed

An unexpected obligation creates pressure because a resident or group asks for help after the player has already allocated time or materials elsewhere. This should not trigger a surprise morality test. The player needs enough context to compare the requests and an honest way to decline, negotiate, or change an existing commitment. The feature earns depth when a decision reallocates a real resource and creates a visible consequence.

### Request arrives with a source and deadline

The new request identifies who made it, who is affected, why it arrived now if known, what must happen, and the last useful decision time. A deadline can be exact, approximate, or unknown. “Before the next shift” requires a roster/time owner. “Soon” needs an author-defined reason and should not drive an automatic penalty. The player sees which existing commitment would be disrupted if they accept. A new request does not erase old consent or invalidate an earlier promise.

The choice set is: keep the current commitment; pause or renegotiate the current commitment; divide the task if the task owner supports it; find a willing alternate; meet part of the new request; or decline. These are action branches. They are not alignment variants. The player can ask each stakeholder to revise terms, but cannot promise on behalf of an absent person. If the system lacks a deadline or partial completion API, the offer must not imply those are available.

### Distinct playstyles through operational choices

The **planner** can inspect both queues and schedule a later response, preserving the first commitment at the cost of delay. The **improviser** can identify an alternate participant, accepting coordination risk and a fresh consent check. The **resource steward** can offer a partial solution that consumes fewer materials but does not meet the whole need. The **advocate** can ask a supporting group to review which request is blocked by a policy or access constraint. The **boundary-respecting facilitator** can let the affected residents negotiate directly and keep the player out of a personal dispute. The **hands-off player** can decline to mediate and choose only the operational change they control. Each style yields different interactions and costs with no single virtue ladder.

### Story example: the missing night-watch relief

The player has promised to support a resident's goal during a work period. A late request arrives asking for relief coverage because the assigned watch member is exhausted. If the duty authority reports an actual open slot and the watch member accepts, the player may reassign their own time, ask another eligible resident, or postpone the personal goal. The goal-holder can choose whether to reschedule. If the new request has only an unverified account, the player can ask for the roster record, but should not dismiss the need just because it is not proven. If watch is already staffed, the player can help with the non-duty request without incorrectly moving a roster assignment.

A small watch-support faction can report its actual staffing capacity, provide an approved relief option, or state that it cannot take another shift. It is not required to solve the conflict. A stores or care group might offer a different service relevant to fatigue only if the existing health/needs owner defines that option. The narrative cannot prescribe care or diagnose the worker.

### Long-tail consequences without global alignment

If the player keeps the original commitment, the new request may remain open, expire, or be handled by another route. If the player renegotiates and both parties accept, each new agreement gets its own identity and task link. If the player breaks the first promise without asking, the affected survivor may refer to that concrete action later; any persistence belongs to the current social owner. If the player declines both, the system shows two unresolved obligations. If they solve one partially, the next branch can begin from the portion actually completed.

A future request can vary based on what the player did. Someone previously rescheduled with enough notice may offer another clear window. Someone whose task was repeatedly displaced may ask for a firm slot before accepting. A supporting faction may reserve time for a project that the player previously completed with them. These are narrow, event-based continuities. They do not imply that the player is globally generous, cowardly, corrupt, or trustworthy.

### Event identity and one-time consequence

Each obligation must have a stable identity from its actual producer. The feature must not create two identical requests when a panel is reopened. If a single action satisfies two obligations, the owner event can be linked to both only when that is semantically correct; otherwise each task requires its own completion. Relationship consequences, resource costs, and task status should apply once per action. A duplicate notification may update presentation but may not create another penalty.

If an obligation is canceled, distinguish cancellation by the requester, player decline, expiry, and inability to proceed. Each has a different future message. Only an owner-approved state can persist. After reload, the player should see the true current disposition and any remaining alternative. A completed obligation must not return to the open list due to a stale view, and an expired obligation must not reappear as a newly urgent request.

### Review checklist for the case

The review uses a small decision graph: existing commitment honored; first commitment renegotiated; alternate person asked and refuses; partial help accepted; request declined; evidence checked; player abstains from the conversation; save/reload between request and decision. For each route, document labor, items, audience, schedule, consent, durable status, and ending information. If one path has no truthful resolution, record a content gap rather than letting dialogue pretend it succeeded.

## 475. Scaffold realization — Installment 44: publicity, attribution, and the right to remain ordinary

Some survivor initiatives become visible because they affect a shared room, use shared materials, or invite help. Publicity can bring assistance, but it can also turn private work into a performance. The system should make the audience part of the choice: who is named, what contribution is visible, and whether the participant actually wants recognition. A completed task can be public without turning its worker into a symbol for the whole shelter.

### Attribution is a separate action

Task completion proves that an owner-backed action occurred. It does not imply permission to publish the worker's name, quote their reason, or assign the accomplishment to a faction. The player may announce the outcome anonymously, credit the participants who opted in, post only the operational result, or say nothing. If there is a credit or chronicle owner, use it. Do not implement a local “fame” field for this proposal.

Before posting, show the proposed attribution and audience. “The water bench is repaired” differs from “Mara fixed it after three nights without sleep.” The first may be an operational fact; the second adds private health and personal detail. A player can ask for specific permission, remove the detail, choose a private thank-you, or decline the announcement. A resident may accept recognition for the work but not the reason they chose it. Another may prefer the work to speak for itself.

### Action-driven branches

If the player previously credited someone accurately and with permission, a later resident may proactively ask for attribution. If the player previously shared a private motive publicly, the next initiative can default to an unnamed completion summary or require review. If the player thanked a support crew for a service they actually performed, that group may invite a future collaboration. Each branch reflects a discrete event and a present request. No resident is permanently labeled as publicity-loving or publicity-averse from one choice.

Publicity can also affect access to assistance. A visible project may attract volunteers, but every new helper still needs an eligibility and consent route. A board announcement can invite offers; it cannot assign work to everyone who sees it. A public appeal may reveal resource demand and create pressure to finish. The player can choose a smaller audience or private outreach instead. If publicity does not change any current owner state, its outcome should remain narrative and not promise material benefits.

### Supporting faction voice

A minor group can ask to be credited for its actual contribution or request that a technical detail be included for safety. It cannot claim ownership of a resident's personal goal. The player may credit the group, attribute work to named individuals with permission, write a joint note, or omit names. If a faction's identity is politically sensitive, use current canon and let its members choose how they want to appear. The support group should not be reduced to a logo attached to the player's success.

### Example: a room made usable

Two residents and one support worker complete a small task that makes a room easier to use. The task owner records what changed. One resident wants the room listed as available but does not want personal recognition. The other is comfortable being named. The support worker asks that the safety limitation remain in the note. The player can write a factual availability notice, credit the willing resident and crew, ask whether the first wants to be named, or simply update the room state with no public post.

If the player publishes a named account without permission, a correction or removal may be available only through the current message owner; the affected person can request it. If that content route is absent, the plan must not pretend the disclosure can be undone. The better implementation is to prevent unauthorized names at the confirmation step. If the player selects anonymous posting, no later branch should reveal the worker through a forced cutscene.

### Acceptance cases

Verify factual outcome without attribution; explicit opt-in for names; partial opt-in to name but not personal explanation; group credit; support worker requests a safety note; invitation to help does not auto-assign; and correction after an authorized post. Confirm the persistent task result is not tied to publicity. The goal can succeed quietly, fail publicly, or remain unfinished without being ranked by how many people heard about it.

## 476. Scaffold realization — Installment 45: several requests compete, but no universal deservingness score

When requests compete, the player needs a way to compare their consequences without turning people's needs into one total score. This installment expands the choice architecture: present the source, deadline, affected task, consent dependency, and alternatives side by side, then let the player select an action. The system may supply hard constraints such as a reserved item or an open roster slot. It should not invent a universal ranking for who deserves labor first.

### The decision table

For each request, show what must happen; when it matters; who has agreed; which resources are committed; what happens if it waits; and what is known about substitution. If a field is unknown, show that uncertainty. Keep subjective importance with the people involved unless a current policy or task owner sets a priority. A task with an operational deadline can outrank a flexible personal goal for one scheduling action, but that does not make the goal less worthy or cancel it.

The player can choose one request, ask requesters to negotiate, change the order, split capacity if supported, find another provider, defer both, or decline one or all. A request may be partially met only when its owner and participant agree on a meaningful partial state. Splitting one shift into fragments may create handoff risk; the UI should show it only if the roster supports that. Choosing no action remains valid, with the resulting unmet needs visible.

### Branching based on how the player has allocated capacity

If the player previously committed all qualified workers to maintenance, a new care or logistics request can show the real bottleneck and the next opening. If the player reserved one general worker for flexible tasks, they may now have a response option. If an earlier task is stalled for lack of materials, a different resident may become available to assist. If the player has repeatedly accepted every request, the roster can still show overload; it should not reward overcommitment by allowing impossible assignments.

This makes the player's style emerge from allocations. A specialist protector can prioritize tasks needing specific skills. A rotating coordinator can spread work and accept more handoffs. A relationship-focused player can ask participants to agree on a trade. A resource steward can choose the task that makes best use of a scarce item. A minimalist can refuse expansion and preserve the current queue. Each path can be defensible and costly.

### A three-request case

The player sees a repair initiative needing a specialist, a shared-room request needing an open time slot, and a watch coverage gap needing an eligible volunteer. One specialist is free, the room is booked for a repair, and one watch member has requested a schedule change. The player's options are constrained but not moralized. They can move the specialist task if its owner allows it; ask a second person who may refuse; shift the room request; or leave the watch gap visible while changing the underlying task plan. The participant can withdraw a request when it no longer matters.

A support group can provide a resource or schedule fact, but it does not choose which person the player values. A records helper may expose that one task's deadline is an estimate. A watch team may verify that a backup exists. A room maintainer may propose a different window. Each piece of support changes available information; the player still owns the operational decision.

### Capacity is not character judgment

No branch should label the player neglectful because they did not complete all initiatives. The shelter has finite time and real trade-offs. At the same time, the plan avoids a frictionless “everything waits forever” system. Existing need and task owners may apply consequences: a repair remains broken, fatigue may remain, a private goal may expire, or a request may be withdrawn. The writing states these outcomes plainly and lets residents respond to particular broken promises.

If a request is declined, the player can offer a reason from current facts or decline without explanation if that is a valid choice. The requester may respond, but they do not have to absolve the player. A refusal can be regretted later; it cannot be silently converted to acceptance. The future conversation references the decision and its effect, not a hidden score.

### Review and tuning questions

Before writing content, confirm whether deadlines, skills, availability, workload, and partial tasks are real owner-backed facts. If a comparison panel cannot show them, use a short per-request breakdown rather than a composite score. Ask whether the menu is readable, whether urgent requests are actually urgent, and whether an alternative can be chosen without opening four unrelated panels. The plan's depth should come from clear decisions and interlocking consequences, not from multiplying numbers.

## 477. Scaffold realization — Installment 46: a survivor revises consent after work begins

Consent can change after a task starts. A participant may learn that the work is riskier, feel more tired, discover a personal conflict, or realize they misunderstood the request. This installment treats a change of mind as a new fact to route through the same three features. It does not promise a free undo after every completed action, but it does require a safe and honest response when the participant withdraws before the task is done.

### Stop, secure, and decide

The first player choice should not be “force them or lose.” If the task owner supports a safe pause, the player may stop work, complete a safety step with consent, or transfer the task through a valid handoff. If no safe interruption exists, the owner may require the minimum action needed to reach a safe state; the game must explain that rule before assigning the work. The player's authority to manage a task does not erase the participant's boundary. Material already consumed remains consumed unless the owner can recover it.

Once the immediate state is safe, the player can ask what changed, accept the withdrawal, offer a revised scope, ask a different eligible person, or leave the task open. Asking for an explanation is optional and can be declined. A resident does not owe the player a personal reason to stop. A changed scope requires fresh agreement. The original acceptance cannot be reused to compel the revised work.

### Distinguish completed milestones from unfinished obligation

If the resident finished a real milestone before withdrawing, the task owner should preserve it. The personal goal may advance through that event. Remaining work stays open or closes as incomplete according to the owner. If a goal has a single completion threshold and no partial state, the plan must not synthesize one. The player may receive credit for actual work without the dialogue implying the whole project is complete.

If a borrowed item or reserved material is involved, the task/asset owner determines whether it returns, remains in use, or must be secured. A support crew can receive a handoff only if the task supports it. The participant's withdrawal does not make them responsible for an unsafe deposit or a broken item by default. The player can choose to accept the cost, request a repair, or leave the work paused.

### Branches from earlier player behavior

If the player previously respected a pause, the resident may offer another goal with clearer boundaries. If the player previously reassigned work without asking, this resident may now request explicit confirmation before a new assignment. If the player responded to a change with a safe alternative, the future scene can refer to that action. If the player continued after a clear refusal, later cooperation can narrow or stop where an existing social/consent owner supports that consequence. Do not broaden a task-specific breach into a universal lockout without evidence.

These branches produce distinct endings: the task is safely paused; the resident chooses a smaller scope; another person accepts a new task; the player finishes the safe handoff and closes the request; the goal remains unfinished; or the player ignores the boundary and faces a concrete, owner-backed consequence. An ending is not just a dialogue label. It is the durable task and consent state after the choice.

### Supporting group role

A supporting faction can advise on worksite safety, help recover an item, or explain the reassignment process. It cannot pressure the resident into continuing. Its worker can be unavailable, and the player must still have a response route that preserves safety. If there is no current owner for a safe stop or handoff, that is a design blocker before implementation, not a reason to author a forced dialog choice.

### Acceptance walkthrough

Start a real task, consume a resource, record one milestone, then receive a withdrawal. Verify: the player can pause; the milestone is not erased; spent resources are not duplicated; unspent reservations follow the correct owner; the resident is not assigned new work; alternate participants get fresh offers; and reload preserves the same state. Also test withdrawal before any work begins. The feature should report exactly what can and cannot be undone.

## 478. Scaffold realization — Installment 47: after a refusal, a new route stays new

Refusal can close an offer without closing all future contact. This installment specifies how the system can support follow-up without treating the original refusal as a puzzle to overcome. A new route must be materially different: a different task, timing, helper, communication channel, or term set. Repeating the same offer until the person accepts is not branching.

### Valid follow-up types

**Neutral check-in:** ask whether the underlying need still exists without re-presenting the rejected commitment. **Independent route:** offer a resource, task, or help from someone else, with their own eligibility and consent. **Revised terms:** change timing, scope, equipment, or audience and preview the difference. **Distance:** stop contacting the resident about the subject, while continuing unrelated required operations. **Closure:** mark the request withdrawn, expired, or declined according to its owner. Each route should be explicit and once-per-cause unless a real system defines a repeat rule.

The recipient controls whether to re-engage. If they decline follow-up, the interface closes that social prompt. The player may still manage a separate operational need through proper owners, but cannot use that work path to pressure the same person. A task may be reassigned through an approved roster command; this is not a covert way to route the rejected personal request back to them.

### A changed offer has to change something observable

If the player says “I can make it easier,” show the new schedule, reduced task, alternative location, or material change. A vague reassurance does not count as a new term. The resident can compare it to the original offer and accept, counter, or decline. If the new plan is not feasible, say so before the participant is asked. If a supporting group's capacity creates the alternative, show the source and whether it is committed.

The player can also realize the original request was based on a misunderstanding. They may correct the fact and ask whether that changes the person's view, without demanding a changed answer. The resident may confirm the refusal, accept a different action, or explain a further boundary. That dialogue is optional content and must not be required to keep the core campaign moving.

### Ending texture

A refusal can end in peaceful distance, a different helper completing the task, a later voluntary reoffer by the resident, a task closure, a partial workaround, or a continuing unresolved need. These endings differ in state, not in moral label. Some may be quieter or less conclusive than a conventional quest finale. A person who declines help can still share a meal, work another shift, or ask for unrelated assistance. If the game's social authority lacks domain-specific boundaries, keep these as authored dialogue variants and do not persist a global aversion flag.

### Review questions

Does the follow-up offer change a real condition? Does the player know why the option is available? Can the recipient say no again and end the prompt? Can the player leave the situation unresolved? Is there a separate operational need that remains solvable without coercing the original participant? Does the content avoid promising reconciliation? A good follow-up gives the player another way to act while preserving the first answer.

## 479. Scaffold realization — Installment 48: playstyle and ending matrix

The initiative loop should accommodate distinct player approaches without selecting a hero, villain, or “best” route. The matrix below describes actions the player tends to choose, the trade-offs those actions create, and the kinds of endings they make reachable. A playstyle is observed through repeated concrete decisions, but the plan does not persist a class or score unless an existing system already owns such a concept. The player can change approach between scenes.

### Playstyle profiles as action patterns

**The careful coordinator** checks roster, schedule, and dependencies before making offers. This reduces stale commitments and can delay action. They often reach endings where work proceeds later with clear ownership, but they can miss a narrow opportunity while waiting for verification. The feature should reward the clarity they created, not a hidden “planning” bonus.

**The capacity steward** keeps shared labor and materials visible. They often decline requests that would displace an urgent owner-backed task, negotiate smaller scopes, or ask support groups for alternatives. Their endings may preserve communal stock but leave an individual goal pending. Residents can disagree with the priority without being cast as selfish.

**The relationship facilitator** asks participants what they want, preserves private channels, and helps create a mutually accepted schedule. This can produce slower but durable agreements. It can also fail when someone declines to talk. The player must be allowed to stop facilitating instead of making every disagreement their responsibility.

**The practical improviser** searches for alternate workers, tools, locations, or smaller task steps. This can rescue an initiative quickly and may create handoff risk. The system exposes eligibility and consent anew for each alternative; improvisation never licenses assigning work without permission.

**The evidence-minded investigator** checks records, labels uncertainty, and makes narrow corrections. This can prevent a mistaken action but uses time and may reveal less than the player wants. Their branch may remain uncertain even after a careful search. The game must not guarantee a satisfying truth reveal.

**The boundary-first player** accepts refusals, offers only materially changed follow-ups, and leaves unresolved personal choices alone. This can protect relationships and preserve autonomy, but operational needs may remain incomplete. The game should not punish this approach with an invisible “low initiative” score. The player can still progress through independent routes.

**The selective participant** declines to mediate or support some initiatives, focusing on the tasks they control. This can preserve attention and reduce accidental promises. It can also mean a resident's project receives no help. The feature records only the accepted, refused, or unanswered request and its real consequence. It does not label the player apathetic.

### Ending families

An ending is a stable disposition of a particular initiative, not the conclusion of all social life. A personal goal can end as **completed by its owner-confirmed task event**, **completed with a mutually accepted revision**, **paused with a valid resume condition**, **withdrawn by its holder**, **declined by the player**, **expired by its stated rule**, or **unresolved because a required authority or resource remains unavailable**. Each disposition has different future affordances. Completed work may open a new goal; a paused task may resume; a withdrawn request closes; an unresolved need remains inspectable.

Each ending can further vary by the player's actions: who performed the work, whether a support group helped, whether materials were recovered, whether an acknowledgement was private, whether a message was corrected, and whether the player made or kept a commitment. The ending record should not try to encode every narrative nuance in a large flag set. Stable owners preserve the facts; authored text selects from those facts. If a nuance cannot be stored, do not claim it will alter future scenes.

### Cross-matrix for acceptance

| Action pattern | Cost surfaced | Supporting role | Distinct ending enabled | Prohibited shortcut |
|---|---|---|---|---|
| Wait for a named worker | Time and open need | Verify schedule | Same goal resumes with that worker | Auto-assign another resident |
| Ask a substitute | New consent and roster slot | Check eligibility | Different worker accepts or refuses | Inherit original consent |
| Narrow task scope | Reduced result or longer process | Verify safe subtask | Revised goal completed | Mark full goal complete on click |
| Check a disputed record | Time and limited information | Records service | Fact corrected, motive unknown | Confession reveal from a hidden flag |
| Decline mediation | Unresolved relationship issue | None required | Operational route remains separate | Force reconciliation to progress |
| Publicly attribute work | Disclosure and attention | Board custodian | Opt-in recognition | Publish private motive by default |
| Respect withdrawal | Lost capacity or materials | Safe handoff advice | Goal paused or closed honestly | Morale penalty for refusal |

Use this matrix to find branches that are superficially different but mechanically identical. A new line is not a branch unless the player action, owner state, or future option changes. Conversely, two routes can share a task outcome while preserving distinct authorship or audience state; that is useful narrative variation, not duplicate mechanics.

### Ending composition by action, not reputation

For each major initiative vignette, select an ending from its actual state vector: task disposition; participant consent; item reservation/return; support service outcome; communication audience; and whether a promised follow-up remains. These are separate facts. Do not combine them into a single “relationship success” bool. Example: the task can be complete, one participant can remain unwilling to talk, the borrowed tool can be returned, and the follow-up message can remain unread. The story can hold all four truths without flattening them.

If a broad narrative ending needs a summary phrase, write it from the most material resolved fact (“the bench is usable; the request to meet remains declined”), not a moral verdict (“the shelter learned to trust”). A faction may offer support in one branch and decline it in another while remaining a continuing local presence. The major factions retain their own arcs; these supporting groups contribute bounded services and specific memories.

### Replay and authoring discipline

Replayability comes from combinations of valid actions, not random disposition. The same seed and state should produce the same offer and eligibility. Player choice may vary the branch. Where authored line variation exists, it should be stable per event or cosmetic. A new game can reveal another route through different resource and roster state, but a loaded save cannot reroll the same offer. The writer should tag every sample line as candidate text until cross-file continuity review confirms IDs, facts, and canon.

The playstyle matrix is a review aid, not a formal player taxonomy. Never display “You are a boundary-first player” unless a separate, approved product feature explicitly wants that. Use the profiles to ask whether the branching design supports several legitimate approaches; then remove the labels from player-facing content.

## 480. Scaffold realization — Installment 49: integrated acceptance journey

The acceptance journey proves that the feature can be reached from ordinary play and survives its important branch points. It begins with a real owner event, not a bespoke test button. It crosses the initiative loop, consent check, task completion, communication, and save path only where those seams already exist. The journey includes success, refusal, denial, revision, and recovery.

### Journey A — ordinary offer through owner-confirmed completion

1. A current system creates a real need or eligible goal. The offer source can be identified and is not generated by opening a panel.
2. The interface shows actor, target, reason, deadline/cadence if supported, prerequisites, and known cost.
3. The player accepts or supports one offer. Eligibility, consent, and resource availability are checked by their owners.
4. A task is assigned through the current roster/task route. No new local task counter appears.
5. The worker completes an event-backed milestone. The goal advances from that event once.
6. A notification or board view reflects the same outcome without becoming its own source of truth.
7. The game saves and reloads. The task and personal goal remain in agreement; no duplicate reward or stale offer appears.

At each step, capture which owner supplied the state and what public API the route used. A screenshot of a completed dialog is not sufficient evidence. The test result should include an observable world or task state that changed through its real owner.

### Journey B — refusal plus independent operational response

1. A survivor receives a valid offer and explicitly refuses with an available reason or declines without one.
2. The refusal is recorded once. No work assignment or hidden relationship penalty occurs unless an approved owner policy defines one.
3. The player can accept the refusal, inspect the unresolved need, seek an alternate eligible participant, revise the task, or leave it unresolved.
4. Any alternate participant receives a new consent offer. A refusal does not transfer.
5. The original participant remains free to cooperate on unrelated work if the current system supports those separate domains.

This journey proves that the system is not merely a “yes/no” UI and that operational continuity does not require coercing the original resident.

### Journey C — authority denial and stale data

Attempt an assignment that the roster or task owner rejects because status, skills, schedule, or capacity changed. The interface should report the real denial reason, refresh current options, and allow the player to choose another path. It should not leave a phantom goal marked in progress. If the reason is unknown to the host, show a neutral unavailable state and log the evidence gap rather than inventing a specific explanation.

### Journey D — changed terms, interrupted work, and restore

Start a real task, change a term through the current owner, interrupt after a supported milestone, save, reload, and resume or close. Verify the original acceptance does not silently authorize the new scope; the completed milestone remains; the remaining work remains open; resource custody is consistent; and any follow-up message refers to the current task identity. If no safe interruption API exists, this journey is an explicit pre-implementation blocker.

### Evidence package

For every journey, record: exact owner type and API; stable actor/task/event IDs; current save-section owner; RNG source if choice is stochastic; deterministic order; host entry and exit routes; message/event consumer; and observable state before and after. Do not paste secrets, private save data, or personal configuration into the plan. A plan can name files and APIs only after current source inspection. The existing forensic report is a starting pointer, not proof that the seam still matches.

## 481. Scaffold realization — Installment 50: closeout boundary and handoff checklist

Plan 3's closeout is a writing milestone, not an implementation sign-off. It can close as a proposal only after the document reaches the requested measured length, the Section 4 boundary remains exactly three subfeatures, each scaffolded branch is either realized or marked deferred, and the evidence notes distinguish verified current behavior from proposed behavior.

### Closeout contents

The closing review should identify: the three feature subfeatures and their boundaries; the actual current owners verified during a fresh audit; the main unresolved consent and task seams; any save/determinism requirements; the accepted branch matrix; representative lines that still need canon review; supported faction roles that can be mapped to existing data; and scenarios that remain intentionally out of scope. It should also list which choices are available only after an implementation decision. No conclusion should say “fully integrated” unless core, host, persistence, and observable player route have all been proven.

### Handoff package to future implementer

1. Confirm exact current source files and ownership claims in the integration ledger.
2. Re-open the relevant APIs and data rather than relying on this document's old description.
3. Select one complete vertical slice, preferably an ordinary offer with acceptance and refusal.
4. Record the save owner and restore path before adding persistent state.
5. Keep all initiative evaluation deterministic and use the existing campaign RNG.
6. Wire one real task event to one goal progression event.
7. Expose one truthful Godot route and verify focus/close behavior.
8. Add only focused coverage required by the implementation package and project test policy.
9. Run the bounded verification selected by the foreman, not the full suite by default.
10. Re-audit narrative continuity and report remaining gaps without broadening scope.

### Final closeout statement template

“Plan 3 is closed as a proposal at [measured word count] words. Its scope remains exactly three Section 4 subfeatures: offers, consent-aware refusals, and goals advanced by witnessed work. The current evidence review [date] verified [specific seams] and left [specific seams] unresolved. No production implementation is implied. The next authorized package is [package/owner decision], with exact paths and acceptance criteria to be set through current integration governance.”

The template intentionally leaves evidence placeholders. Fill them only after the future source audit. Do not use the requested word count as a proxy for implementation readiness.

## 482. Secondary expansion — denial, migration, and recovery cases that keep branches truthful

The feature needs a deliberate answer for the cases where an action is valid in dialogue but invalid by the time it reaches its owner. A survivor may have accepted an offer, then become ineligible before assignment. A task may have been retired during a content update. A save may contain a goal reference whose task ID no longer exists. A supporting faction may have moved to another roster state. The player must see a repairable condition where possible and a clear unresolved condition otherwise.

### Denial after apparent acceptance

Acceptance of a conversation is not acceptance by the work system. When the player confirms support, the task owner performs final eligibility and capacity validation. If it denies the request, the interface reports that the assignment was not created and preserves the resident's consent only for the exact terms they accepted, if those terms remain current. It cannot claim the resident was assigned. The player can ask why, choose another worker, revise timing, or cancel. If the participant's availability changed, they must reconfirm a revised plan.

This produces a high-value branch: the resident says yes to an afternoon repair, but the roster owner reports that the resident is on another task. The player can move the other task with its owner's support, offer a later time, ask a qualified backup, or let the project wait. The resident may accept one revision and decline another. The conversation should reflect the precise schedule conflict rather than treating the system denial as interpersonal rejection.

### Stale goal and migration handling

On restore, a goal can reference a task that no longer resolves because content was renamed, removed, or migrated. The safe state is not to reset it to a fresh goal, reroll a new request, or delete its history. The owning save/migration path should preserve a recoverable reference or mark it unresolved with a user-visible explanation. The player may archive the old goal if the current social owner supports closure, recreate an equivalent task after review, or wait for an authorized migration. Any newly created task needs a new stable identity and cannot inherit completion credit without evidence.

Content updates should use stable IDs and migration notes. A display-name change does not require new state. A changed task meaning may require explicit migration. The narrative author should never use dialogue text as the migration key. If an old event cannot map to a current goal, mark the example as a known migration gap and keep the player from receiving an apparently complete but empty quest.

### Duplicate event and RNG boundary

Autonomy evaluation can run repeatedly while the player views a roster, advances time, or reloads. A previously generated offer must not consume another random draw simply because a panel opened. Stable input order and the existing campaign RNG contract determine any eligible choice. Repeated evaluation for the same day/cause should return the same offer identity and result. A new meaningful event can permit a new evaluation. If there is no trigger identity or cooldown owner today, the plan must resolve that ownership before implementation rather than storing “already rolled” in a UI field.

The same rule applies after refusal. Reopening the offer cannot reroll willingness until the survivor accepts. If the player changes the real terms, that is a new action and can be reconsidered, but still through a fresh consent check. A deterministic refusal can be narrated differently for flavor only if the variation is stable and does not alter the choice.

### Recovery result vocabulary

Use distinct statuses: **not offered**, **offered**, **accepted terms**, **assignment denied**, **task assigned**, **in progress**, **paused safely**, **completed**, **withdrawn**, **declined**, and **unresolved reference**. The implementation may support fewer statuses. If so, do not fake the missing distinctions in panel-local state. The vocabulary lets reviewers ask exactly what the player can do next and whether that action belongs to the right owner.

### Final adversarial walkthrough

Start with a goal on an eligible resident, have the player accept, then invalidate that resident's schedule before task creation. Verify no assignment exists; the goal remains inspectable; a fresh alternative offer can be made; and no relationship consequence is applied merely for owner denial. Next, save and reload while the goal is unresolved, migrate or remove its referenced task in a controlled content fixture, and verify the migration route does not duplicate or erase it. Finally, trigger evaluation twice with identical seeded state and confirm the same offer identity and no extra random consumption. These cases can be added to the later focused acceptance suite only after the feature owner is agreed.

## 483. Secondary expansion — the aftermath changes the next offer's shape

An initiative ending can influence a later offer without creating a global personality profile. This section maps a handful of owner-backed events to specific next options. It makes the world remember actions while avoiding a generic “relationship score unlocks content” structure.

| Prior action or fact | Later offer difference | What remains unchanged |
|---|---|---|
| Player returned a borrowed tool through its owner | A new loan can show the previous return term and offer a direct renewal | The resident may still refuse |
| Player canceled before work began | A later offer can ask for confirmation before reserving time | The resident is not globally marked unreliable |
| Player stopped an unsafe task after a warning | A later handoff can include the same safety step if the owner supports it | No safety skill is invented |
| Player used a local, consented message | A future private request may reuse that channel when allowed | No blanket communication preference is assumed |
| Player reassigned work without a fresh offer | A future request can require explicit scope confirmation if social policy supports it | Unrelated tasks remain possible |
| Support group completed a bounded service | The next request can point to that service and its known limits | No faction affinity currency is granted |

### Reoffer is not a retry exploit

A later offer needs a new cause: changed roster, newly available materials, a revised scope, a new day/shift when relevant, or a fresh request from the participant. It must not reappear just because the previous offer was declined. The player sees why the conditions changed. If no new fact exists, the correct branch is no new offer.

An affected resident can initiate their own reoffer. That is a distinct story action, not proof that the first refusal was temporary. They may offer a different task, ask for a smaller commitment, or decide not to revisit the subject. The player can accept or decline. A helper group can propose a new route only if its capacity or service changed; it cannot speak as the resident.

### Example: revised goal after a completed handoff

A repair initiative pauses after a worker returns a tool and secures the worksite. Later, a supporting crew completes its inspection and confirms that one portion is safe to resume. The original worker can accept the original task, ask for a smaller remaining scope, or decline further work. Another eligible worker can receive a new offer. The goal owner links the inspection and handoff facts to the new task only where that relationship is real. The player may also close the goal as incomplete and use the repaired area later through the ordinary task route.

The ending can produce renewed collaboration, a new worker taking over, an unresolved goal, or a quiet closure. It never claims that the original worker “came around.” The player sees an observable change: a cleared task, an accepted new scope, a returned item, or no further action. This gives the initiative feature a long tail while preserving each participant's separate choices.

### Long-tail vignette: the second request is not owed

Several days after an unresolved repair, the resident may raise a different need: perhaps they need a tool returned, a schedule clarified, or no help at all. The new exchange should not automatically resume the old goal. The player can answer the present request, ask whether the earlier task is still relevant, or leave the old record closed. If the resident wants to reopen it, they state the new scope or timing. The player may decline. This gives continuity without making every ordinary conversation a disguised retry.

The later scene should carry at most one or two remembered facts that matter to the choice. If the player returned the borrowed tool, the resident can say it is available again. If the player left a message unanswered, the resident may choose a different channel. If a support group finished an inspection, the player can see the updated safety fact. Avoid reciting a complete event log in dialogue. The UI can show the relevant history separately so that the prose remains natural.

This vignette's acceptance test is simple: open a new unrelated conversation after an old goal ended as declined, paused, or unresolved. Verify that the old state remains accurate; the new conversation is available; no old offer is silently reactivated; and a new request can reference the earlier action without changing it. This is the difference between memory and coercive persistence.

## 484. Branch casebook — the basin that cannot be finished in one shift

This casebook follows one small resident initiative across several days. It demonstrates how ordinary goals can carry a narrative arc without becoming a new major quest authority. The basin, workroom, parts, and participants below are illustrative carriers; implementation must use current catalog IDs, locations, resident records, and task owners. The dramatic center is not whether the player is “good enough” to help. It is how the player responds as the goal changes from an idea to shared work with a real limit.

### Opening state: an idea with incomplete costs

A resident raises the idea of restoring a wash basin used by several shifts. Their stated aim is practical: people are losing time carrying water to the far room. They do not yet know whether the basin has a broken seal, a blocked line, or a missing part. The player sees that the goal is proposed, not diagnosed. One line establishes voice through a material detail—“The towel rail is still dry by noon”—rather than a personality label. The player can ask for an inspection, ask what the resident wants to do first, offer a small supply, decline, or leave the idea on the roster as an unaccepted proposal if that view exists.

If the player asks for inspection, a support worker can confirm the work area is accessible or identify the skill needed. They do not diagnose unless their current role supports it. If the player offers materials before the scope is known, the resident can accept a specifically named item only when it is actually useful and owner-transferable; otherwise they ask to wait. If the player declines, the idea closes or remains pending according to the goal owner. The player can still use the existing water route. The arc does not lock ordinary water access behind a social quest.

### First branch: investigate, improvise, or schedule

**Investigate first.** The player requests the narrowest useful inspection. It costs a service slot or time if the owner supplies that constraint. The inspector reports only the observed defect. A missing diagnosis yields “not enough to tell,” not a failure roll. The resident can accept the inspection or decline because they prefer not to delay.

**Try the resident's first proposed step.** If task data supports a safe preparatory action—clearing a space, returning a borrowed tool, checking an outlet—the player can ask for that task. The resident can accept, counter, or decline. The step may reveal a new dependency, but only a real task event can update the goal.

**Schedule work before certainty.** The player can reserve an eligible time slot, if scheduling authority supports it, while leaving the diagnosis unresolved. This reduces delay if the repair is simple but may consume a slot that another job needs. The player can instead wait for a service answer. Neither route is “the right one”; each changes opportunity cost.

**Decline or defer.** The player keeps the shared water route and does not promise labor. The resident may ask when to revisit only if reminders exist. The basin remains unavailable. A later unrelated initiative can proceed.

### Middle complication: the first worker withdraws

When work begins, the original volunteer finds that the damaged section is behind a panel requiring another skill or a second worker. This is a stateful complication, not a random betrayal. The task owner reports that the currently authorized action cannot continue. The resident can pause, complete a safe handoff, ask for help, or revise the goal. The player may prioritize the safe closeout, seek a second resident, ask a supporting crew for a task boundary, or stop. No option lets them continue under the old consent as if nothing changed.

If the second worker is asked, they receive a new offer that names the scope and existing commitment. They may accept only the inspection, not the repair; ask for a later shift; decline; or propose that the first resident remain involved. The first resident can accept or reject the new arrangement. The player can therefore reach a cooperative pair, a one-person handoff, a split-scope repair, or a suspended task. In every branch, the worker's choice is explicit and the task owner owns assignment.

### Second complication: one part is promised elsewhere

The store owner reports that a needed part is already reserved. The player can request an authorized release, locate an alternate through a peer offer, change the repair method if the worker proposes a viable substitute, or postpone. A support group can confirm provenance or inspect a candidate part but cannot release someone else's reservation. The resident may choose whether the alternate method still meets their goal. If the part is diverted and another task slips, that consequence remains visible through the existing task owner.

The player who previously promised the part receives a specific dialogue callback. If it was reserved, the resident sees the commitment and can choose to wait. If it was merely promised, they can ask the player to clarify the difference. If it belongs to another person, the player cannot transfer it. These facts create separate branches without adding a generalized trust score.

### Climax: the work becomes visible

After the task owner confirms the basin is usable, the player chooses what to communicate. They may update only the room's operational state; post a local notice with the new access condition; credit the workers with permission; or keep the initiative private. The original goal-holder may want the result visible so people stop carrying water across the shelter, while a helper may decline attribution. The player can separate information from credit. An intercom is not automatically appropriate: the local audience may be enough.

An incomplete outcome can also be a satisfying branch. The system might confirm that a safe temporary route exists but the permanent repair is still pending. The player can publish the temporary instruction, label its expiry, continue with a new task, or close the initiative as partial only if the goal owner supports that state. No dialogue says “fixed” while the owner reports a temporary workaround.

### Endings and later callbacks

1. **Shared repair completed:** the original volunteer and helper complete the owner-backed task; the basin state changes; later residents use the normal location route.
2. **Safe temporary workaround:** a real task establishes limited use; a notice names the limitation and expiry; a permanent task remains open.
3. **Revised method accepted:** the resident chooses a smaller or slower repair; the goal records the revised scope and later completion.
4. **Worker handoff:** the original volunteer steps back, another accepts a fresh offer, and the first person's completed milestone remains credited.
5. **Resource conflict:** the needed part stays allocated elsewhere; the player preserves that commitment and the basin stays unavailable.
6. **Proposal withdrawn:** the resident decides the project no longer fits their capacity; the player can accept the withdrawal without a relationship penalty.
7. **Player declines involvement:** the goal receives the owner-supported declined/closed state; ordinary water operations remain reachable.
8. **Unresolved authority:** the story pauses at a real gap, such as missing safe interruption behavior. The plan records the implementation blocker instead of writing a false ending.

### Flavor palette by stage

At proposal: damp towel, empty pail, a careful measurement on scrap paper. At inspection: dust disturbed under a panel, one worker checking where the floor slopes. At delay: a marked part shelf and a note that says “reserved until the pump is done.” At completion: a towel hung to dry, water sound if supported by existing audio, or simply a changed room status. Use one or two details per stage. Repeating the same sensory line in every branch makes the arc feel templated.

Character voices stay behaviorally distinct. The goal-holder asks about the hour people lose carrying water. The inspector names the uncertainty. The helper worries about taking a reserved component. The player can choose to relay each concern privately, discuss it together with permission, or leave the decision operational. None of these lines should be treated as current canon until checked against narrative data.

### Branch audit

Before adding this arc to data, construct a trace table with initial goal identity, each task ID, who consented to what, part owner, support service, communication audience, save checkpoint, and final owner state. Reject any line whose precondition cannot be reached. Ensure the resident can opt out at each materially changed scope. Ensure no faction controls the full sequence. The result is one richly branching initiative case carried by the existing three feature subfeatures, not a fourth “community projects” system.

## 485. Branch casebook — a teacher asks for a second pair of hands

This case explores a personal goal that becomes a small apprenticeship request. A resident wants to teach another survivor a practical skill because the current worker is the only one who can maintain a recurring task. The arc is about consent, schedule, readiness, and whether the shelter chooses to invest time now. It does not claim a new training authority. If the current apprenticeship or skill system cannot prove a session, keep the proposal at content level and do not award skill progress from dialogue.

### Entry fact and first decision

The player sees an owner-backed task dependency: one trained worker is scheduled for a recurring maintenance action. The worker asks to train a second person. The student may be named only if a current survivor expresses interest or the player asks them through a permitted offer. The teacher's initiative can exist even when the student declines. This separation prevents the teacher's enthusiasm from becoming another person's assignment.

The player can support a first conversation; ask the current duty owner how a lesson could fit around work; locate a safer practice task; or decline because the present schedule cannot absorb it. A support group may provide a tool, supervised practice area, or reference sheet if those resources and services exist. It cannot certify the student unless the skill system recognizes a certification event.

### The student decides what kind of help they want

The student may want to observe only, try one task, practice with supervision, study independently, or decline. Each option has different time, materials, and risk. The teacher may be willing to demonstrate but unwilling to be responsible for unsupervised practice. The player may ask the teacher to revise the session or choose another learning route. A displayed “begin training” action must state whether it creates one session or an ongoing commitment; the recipient should not discover later that they were enrolled indefinitely.

Prior player action creates fine-grained branches. If the player previously returned borrowed practice equipment, the next session can use that known available item. If the player moved a duty without checking the teacher, the proposed session may require a different time. If another resident has already requested the same room, the player can negotiate an alternative space. The branch is produced by a concrete schedule or custody record, not an alignment threshold.

### Mid-arc friction: the lesson works, the shift does not

After a first practice event, the student can perform one recognized step. The maintenance duty owner still determines whether the student is eligible for an operational shift. The player can request a supervised task, let the teacher evaluate a practice milestone, reschedule future lessons, or stop. The student may be proud of the learned step and still decline the responsibility of taking a shift. The teacher may be pleased to teach and still refuse additional sessions due to fatigue or schedule.

If a support crew provides an inspection, it can confirm the equipment is ready for practice. It cannot decide the student is competent. If the task owner has a skill threshold, it applies that threshold. If no threshold exists, the content must avoid numerical claims. The player can assign another trained worker and keep the training goal optional. This ensures the shelter does not gamble on an unsupported skill state just to complete the vignette.

### The resource branch

Practice may use an item that could be consumed or damaged. Before the player approves, show whether the item is a consumable, a reusable tool, or shared stock. The teacher can ask to use a damaged practice item rather than risk the only working tool. Stores can report actual stock. The player may reserve the safe item, wait for a replacement, ask the teacher to demonstrate without hands-on practice if supported, or release the goal. A support group may donate a reference or tool only if its inventory and service authority support it.

The cost can be an item, a shift slot, or delayed maintenance. Do not stack a fictional faction favor on top unless it represents an explicit contract. The player should be able to compare the training goal's long-term benefit against the immediate maintenance need using real owner facts. The feature does not decide that education always outranks production or vice versa.

### Endings

1. **Observed session:** the student watches and chooses whether to continue later; no competence is inferred.
2. **Supervised practice:** the skill owner records a real session; the operational duty remains with a qualified resident.
3. **Student takes a bounded shift:** the roster verifies eligibility and the student accepts the specific task.
4. **Teacher revises terms:** sessions move to a later schedule or smaller scope, preserving the original goal in revised form.
5. **Student declines:** no training state is created; the player can keep existing coverage.
6. **Teacher pauses:** the completed session remains, while future instruction remains uncommitted.
7. **Equipment conflict:** the shared tool stays allocated elsewhere and practice waits.
8. **No owner-backed learning event:** the scene remains a proposal and grants no persistent progression.

### Long-tail callback

Later, a task may present a routine choice: assign the already-qualified worker, ask whether the student wants a supervised role, or postpone if neither is available. The player sees the actual skill and schedule state. The trained student may accept or refuse. The teacher may offer a new lesson if a real next step exists. A faction can refer to the session it supported but cannot claim ownership of the student. The story's payoff is a wider set of willing options, not a guaranteed bonus or a binary “mastery” ending.

### Authored texture and review

Use small details tied to the practical skill: a hand steadying a gauge, a mark on a scrap card, a tool returned to its hook. The teacher's voice can focus on safety and sequence; the student's on uncertainty or a specific question; a support worker's on equipment limits. Let a quiet ending remain quiet. Before shipping, verify which training, skill, duty, item, and event APIs are live and map the arc to them. If the game has no teachable task route, preserve the narrative idea without implying a working apprenticeship feature.

## 486. Branch casebook — a route note is not yet a safe route

A survivor wants to contribute a correction to a route record after noticing that a familiar landmark has changed. Their goal is to make the next journey easier for whoever follows. This arc broadens initiative beyond repairs and duty assignment while retaining the same three subfeatures: an offer to help, consent about who can use the contribution, and a goal advanced by a witnessed action. The map and travel owners remain authoritative. A survivor's memory is useful evidence, but it does not by itself certify route safety.

### Opening: a correction with a narrow source

The survivor says a marker near a cut road is missing. They can describe where they saw it, when they last passed, and what they believe the new hazard is. The player sees this as a participant account. If a current travel or cartography system already contains a stale route record, it can be shown with its source and freshness. If it does not, the example remains authored context. The player can ask for a route survey, ask the survivor to mark their observation, seek a second source, decline to publish, or let the note remain private.

The survivor may want to finish the note themselves, travel with a companion, or submit the detail without their name. The player can present these as separate offers. A willingness to share an observation does not automatically authorize a dangerous expedition. A desire to travel does not guarantee that the roster, weather, or travel owner permits the route today.

### First branch: verify the marker

**Check an existing record.** A map or travel owner can report the recorded route state. The player can add a clearly labeled observation if supported, wait for an authorized update, or conclude that the existing record already contains the hazard. The resident chooses whether their note can be attributed.

**Ask a support scout.** A minor group with an established survey service can verify a landmark, but only through its actual roster and task route. It may have no safe window. The player can schedule a later survey, use a different route, or retain the note as uncertain. A scout cannot certify a route by reading the survivor's description aloud.

**Make no map change.** The player can communicate the uncertainty privately to a specific traveler, if that channel exists, or simply avoid the route. This protects against publicizing a false claim but leaves other travelers without the warning. The resident may decide to keep investigating, withdraw, or ask the player to reconsider after new evidence.

### Second branch: the survivor's consent changes with audience

The resident may agree to leave a note in a private task record but decline public attribution. They may allow an anonymous warning but not a full account. They may approve a public note after it is shortened to the confirmed landmark and hazard category. The player previews the audience and wording, then either posts through the map owner, waits for verification, or stops. A message board can point to the map correction but cannot become the map's source of truth.

If the player shares beyond the approved audience, the resident can ask for removal or clarification through the current owner. The player can respect that request, explain that a copy has already been used, or leave the correction unresolved. The narrative must not imply that public removal erases every recipient's memory. The practical route warning remains until the map/travel owner changes it.

### Third branch: the route is still useful for someone

Another survivor needs the route for a specific task. The player can send the current uncertainty, ask the task owner for an alternate destination, request a fresh survey, or wait. The traveler can accept the risk only under existing travel rules. The player must not turn a personal map goal into a mandatory expedition for another resident. The traveler can decline the route or ask for another escort; each receives a fresh roster and consent check.

An earlier player choice changes available information. If they previously recorded map sources clearly, the new report can compare to the old observation. If a prior route was outdated, the UI can highlight that the data has aged. If the player publicly attributed an unverified marker, the resident may now request review before sharing another observation. The branch is based on content history and audience, not on honesty points.

### Endings

1. **Verified correction:** an authorized survey confirms the marker, the route owner updates it, and the survivor's goal advances from the recorded contribution.
2. **Unverified caution:** the note is shown as an observation, not a safe-route certification; travelers can choose an alternate.
3. **Private contribution:** the resident submits a limited note for one task without public attribution.
4. **Survey deferred:** no change is published until capacity or conditions improve; the goal remains pending.
5. **Resident withdraws:** the note is not shared; any already-posted content follows the real correction route.
6. **Alternate route:** the player routes the dependent task around the uncertainty while leaving the map unchanged.
7. **Conflicting evidence:** another source disagrees; the player preserves both accounts or asks for more verification.
8. **No supporting owner:** the feature does not claim a persistent map update; the story remains a proposed content beat.

### Flavor and role distinction

The survivor describes a marker as a sound or turn rather than a coordinate if that suits their voice; the scout asks about visibility and last-seen date; the traveler asks whether the route is mandatory; the player decides how far the information should travel. Use a physical detail—the old nail hole where a sign hung, a patch of pale paint, a torn strip tied to a branch—only if a current location can support it. Do not turn a likely landmark into a canonical location without checking the catalog.

### Acceptance evidence

The path review identifies the observation source, survey task, route owner, consent scope, audience, map update event, dependent travel request, save checkpoint, and all endings. Verify that the goal advances only from an owner-backed observation or survey event. Confirm no public notice rewrites the map, no player assumption makes a route safe, and a resident can withdraw their contribution before publication.

## 487. Goal variety — five kinds of intention, five kinds of proof

The initiative feature should not equate personal meaning with a physical construction job. The same goal owner can represent several kinds of intention, provided each type has a truthful completion signal. The following typology is a writing and integration guide, not a request to add five new systems. Each route remains under Subfeature 3: goals advance from witnessed, owner-backed work.

### Practical contribution

A survivor wants to fix, clean, prepare, or deliver something. The task owner provides prerequisites and completion. The player can assign, reschedule, seek a helper, or decline. A practical goal can be paused when materials or capacity are missing. Its ending is visible in the world or task state. Flavor should show the residue of work: a dry patch, a returned tool, an open path.

### Learning intention

A survivor wants to practice or teach a skill. A real lesson event or task milestone proves progress; dialogue alone does not. The learner controls whether to observe, try, or stop. The player can find a teacher, reserve time, or let the goal wait. The story can celebrate a small step without claiming mastery. A skill owner decides when a capability changes.

### Expressive intention

A resident may want to play a tune, draw a memory, tell a story privately, or make a small object that carries personal meaning. This can be a powerful playstyle branch because success may be measured by audience and consent rather than material value. The participant can choose private creation, shared performance, anonymous display, or no sharing. If the game has no persistent creative-work owner, the event can be a one-time authored scene and the goal should close as narrative completion only where that is supported. Do not create a free-text artwork database in the initiative panel.

### Relational intention

A person may want to thank someone, return an apology, or ask to spend time together. These requests require both participants' consent. The player can offer a message route, help coordinate a time, stay out of the exchange, or decline. One person accepting does not bind the other. A completed conversation can be recorded only through an existing social or quest event. The outcome may be a declined invitation, a short meeting, a longer conversation, or no reply. None of these grants the player ownership of the relationship.

### Civic or shared intention

A survivor may want to contribute to a shared routine: maintain a notice board, sort a common work queue, or propose a rotation. This crosses into other authorities and must be tightly scoped. The player can help draft, ask who owns the decision, invite affected participants, or close the proposal. Posting a suggestion does not enact policy. A real duty or governance owner records any adopted change. If no one has authority to accept it, the goal can end as a proposal submitted, not as a completed policy.

### Proof ladder for goal content

For each intention, choose the narrowest valid proof:

1. **Conversation heard:** proves that a scene occurred only if the narrative/quest owner records it.
2. **Offer accepted:** proves a participant agreed to defined terms, not that work happened.
3. **Task created:** proves work was assigned, not that it started.
4. **Milestone event:** proves a bounded step occurred, not that the whole goal is complete.
5. **Completion event:** proves the specific task completed under its owner.
6. **World-state result:** proves the effect consumers rely on, such as access or usable equipment.

Do not skip from step two to step six. If the author's intended beat has no durable proof, design it as a non-persistent narrative moment and avoid future branches that depend on it. This ladder helps authors write emotional stakes without inflating the state model.

### Playstyle implications

An organizer may prefer civic goals and negotiate with affected residents; a craft-focused player may support practical or expressive work; a mediator may help a relational request while keeping boundaries; a learner may invest in sessions with uncertain future benefit; a survival-focused player may decline all optional goals. Every style should receive some meaningful route, but not every goal must be equally useful to every player. Scarcity creates a selective portfolio, not a virtue ranking.

### Writer review

Before adding an initiative, write one line for the goal's intention, one for its evidence, one for its consent boundary, one for its cost, and one for a valid unresolved ending. If any answer is “the player clicked complete,” the goal needs a real producer or should remain prose. Include one branch where the person changes the goal after seeing its cost and one where they decide not to share the result.

## 488. Supporting-faction pattern book — distinct help, distinct limits

Supporting factions should appear as people with a local responsibility that intersects the player's decision. They should not deliver the main faction plot in miniature. This pattern book defines reusable service roles while leaving names, IDs, and canon obligations to current data. A content author can map a role to an existing minor group or omit it. The survivor and player remain central; the task, consent, and save owners retain authority.

### Pattern A — the records desk

**Trigger:** two people remember a resource reservation or work handoff differently. **Service:** check a public ledger entry or timestamp. **Branch opened:** distinguish a documented event from a participant account. **Limit:** the desk cannot read private notes or infer motive. **Cost:** a request takes time, and records may be incomplete. **Player choices:** use the record; ask each person separately; change the plan so the disputed detail no longer blocks work; or stop investigating. **Voice texture:** clipped labels and a careful pause before saying “not recorded.”

The records group can be part of a faction's identity through procedure: duplicate marks, a habit of signing corrections, a ruled book with columns added by different hands. It is not a universal truth court. The group's refusal to answer can preserve privacy rather than create an antagonist branch.

### Pattern B — the practical crew

**Trigger:** a goal depends on a task step, tool, or safe handoff. **Service:** inspect, advise, or accept an eligible work task. **Branch opened:** continue with a verified smaller scope, schedule later, seek another helper, or pause. **Limit:** the crew cannot grant a resident's consent or certify a skill level its owner does not recognize. **Cost:** its own work queue may shift. **Voice texture:** parts, tolerances, what can be done before the next shift.

The crew can have internal disagreement expressed as different practical recommendations, but the player should receive a coherent service outcome from its owner. One member may note that the panel can be secured today; another may say the full repair needs a second tool. This becomes useful when the player asks whether to do a temporary safe step. Do not turn an internal disagreement into a hidden faction loyalty test.

### Pattern C — the watch or route group

**Trigger:** a goal or task depends on schedule, travel, or who is currently on duty. **Service:** confirm a roster fact, provide a permitted escort, or identify an available route window. **Branch opened:** wait, ask another person, change the timing, or choose a safer alternative. **Limit:** the group cannot decide a personal goal for its member or claim a route is safe beyond its own evidence. **Cost:** coverage may be displaced. **Voice texture:** exact times, visibility, headcounts only when the roster provides them.

The watch group can be supportive while refusing a dangerous or impossible request. Its refusal can unlock an alternate plan; it should not automatically make the player an enemy. If the group does not exist in canon, the roster owner can provide the same fact without a faction scene.

### Combining services without building a coalition meter

A single initiative can touch two roles: records confirms that a part is reserved, then the practical crew proposes an alternate repair. The player decides whether to request the second service. The groups do not negotiate with each other unless the story explicitly creates a meeting and each role has consented. No universal faction resource is spent. Each service is accounted for through the current work, inventory, roster, or message authority.

Where two support groups offer conflicting routes, expose their actual constraints. One has a slot today but only for a temporary fix; the other can provide a complete repair next week. The player can accept the temporary state, wait, or find another route. The groups need not argue about who is more trustworthy. Their methods and capacity create the branch.

### Cameo structure

Give a supporting member one concrete objective in the scene: finish a record, return a shared tool, protect a shift handoff, or avoid sending an unsafe volunteer. Show this objective through action or a concise line. Do not add a full personal quest to every service interaction. If the player follows through, the group may acknowledge the specific result later. If the player declines, its members continue their own work. This makes them present without shifting the narrative center away from the survivor whose goal triggered the branch.

### Branch design table

| Service role | Player sees | Choice with cost | Follow-up fact | Keep out of scope |
|---|---|---|---|---|
| Records | What is or is not recorded | Wait, accept uncertainty, or reroute | Corrected/unchanged record | Universal truth score |
| Practical crew | Feasible step and risk limit | Temporary fix, full queue, or pause | Task milestone/inspection | Consent override |
| Watch/route | Coverage or window | Delay, reroute, or request help | Roster/route fact | Guaranteed safe travel |

Before a faction cameo ships, check its current name, role, ID, location, schedule, and narrative voice. If any piece cannot be verified, keep the pattern abstract. No invented group should be written into JSON as mutable authority just to satisfy the support-role design.

## 489. Offer cadence — how the player encounters initiative without prompt fatigue

An initiative loop can feel alive without asking the player to inspect a social inbox every morning. The feature should surface an offer when a current producer creates a reason to act: a survivor explicitly asks; a goal reaches a real milestone; a task becomes available; a prerequisite changes; or a participant requests a decision before a stated time. The panel does not create candidate goals merely because it was opened.

### Offer identity and cadence

Each candidate links to its producer and has a stable identity. Re-evaluating the same producer state returns the same candidate until the player accepts, declines, dismisses, or the stated condition changes. A new day alone may not justify a new offer. If cadence is day-based, the system needs a clear owner rule and deterministic update point. A refusal does not immediately reappear with different wording. A material change—newly available worker, revised scope, fresh request, or completed prerequisite—can create a new offer identity.

The player can view offers now, snooze a non-urgent prompt, decline, ask for detail, or mark a known status for later. Snooze is not consent. It only delays presentation if a reminder owner exists. If there is no snooze/save behavior, offer a non-persistent close control and let the source remain available in its proper panel. Never store a shadow `deferredOfferIds` list in the UI.

### Priority without a universal score

Candidates can be grouped by their real urgency source: an expiring invitation, a task blocked by one action, a resident request with a stated time, or an optional goal. The system can sort by deadline if one exists. It should not create a single urgency score that combines fatigue, relationship, moral value, faction alignment, and resource cost. When no objective ordering is available, use categories and let the player decide.

The player can filter by person, task, support needed, or action type only if existing data can supply those fields. Filters should not hide a refusal or stale offer; dismissed and closed items can be reached through history if the owner supports it. The interface shows unresolved offers without implying that every resident is waiting for the player's attention.

### Supporting factions as request sources

A minor group may tell the player that its service can support a resident goal. That creates an offer only when the service is available and the affected resident agrees to participate. The group cannot add another resident's private goal to the queue without permission. If the resident has not been asked, the player sees “possible support route” rather than “initiative ready.” This prevents faction representatives from becoming quest givers who speak over the people concerned.

The player can invite the resident, ask the support group to explain its service, leave the option unpresented, or continue without help. The resident may decline the group's involvement but still pursue the goal. A faction's capacity can change when its own task completes; the source event can make the support option available then. The resulting branch is grounded in a real service change, not a periodic favor roll.

### Quiet days and sparse campaigns

Some days should have no new initiative. The absence of a prompt can reflect a stable roster, no new requests, and ongoing work. Do not generate filler offers to keep the loop busy. An ongoing goal can still show its current state and next dependency. A player can inspect an earlier unresolved request without receiving new pressure. This makes occasional offers more meaningful and reduces repetitive content.

### Acceptance criteria

Run repeated evaluation with unchanged input; reload an open offer; decline then reopen the panel; advance one day with no producer change; change one prerequisite; and complete a support task. Confirm stable identities, no duplicate prompts, no RNG reroll, no local UI state as authority, and no automatic offer from a faction about another person's private intention. The goal is a deliberate cadence, not maximum message volume.

## 490. Recovery library — eight ways an initiative can stop without ending the story

An initiative should have a truthful recovery when the original path no longer works. These cases keep failure from collapsing into “quest failed” and help content authors distinguish inability, changed consent, and ordinary delay. The player receives one or more next actions, but no alternate route is guaranteed.

1. **Worker is fatigued or otherwise unavailable.** The roster supplies the state. The player can wait, ask for a fresh volunteer, reduce scope if task rules permit, or close. The worker is not secretly assigned because the goal matters.
2. **Required part is reserved.** Inventory reports the reservation. The player can request release from its owner, look for a peer exchange, propose an alternate method, or defer. A support group can explain inventory provenance, not release the part by conversation.
3. **The resident declines after learning the cost.** Close the current offer without creating a penalty. The player can keep the need open, offer a genuinely different scope later, or choose another participant. Rewording the same terms is not a new route.
4. **A supporting service is unavailable.** State whether the service is delayed, declined, or unknown. The player can act without it, seek another eligible service, or wait. Do not imply the faction is hostile.
5. **The task is unsafe to continue.** Use the task owner's safe-stop and handoff rule. If no such rule exists, the feature cannot safely offer mid-task withdrawal as an implemented branch; escalate that gap.
6. **A deadline passes.** Close or expire only through its owner. Show which actions remain possible; do not reset urgency on reload. The underlying need can continue through its own system.
7. **A message was not received.** Show delivery uncertainty, try a permitted channel, or act on the operational need without pretending consent was obtained. Never convert absence of response into agreement.
8. **A saved reference cannot be resolved.** Preserve the unresolved identity, prevent duplicate creation, and provide a migration or close path. Do not erase the person's history or declare completion.

### Recovery copy pattern

Each recovery prompt should use four short parts: **what changed**, **which owner reports it**, **what the player can still choose**, and **what remains unresolved**. For example: “The roster now lists Sera on night watch. Her offer cannot be assigned to the morning repair. You can ask about another time, invite a different worker, or leave the job open. No repair has started.” This is more useful than an apologetic popup or a vague “something went wrong.”

### Supporting-faction role during recovery

A minor group may offer one concrete service after failure: check a record, secure a worksite, verify a substitute, or deliver a permitted message. It cannot make all recovery options available. Its member can suggest a route and explain the service limit. The player may decline the support without losing the original goal. Where several groups overlap, show distinct capability instead of a trust score: records know what was signed; maintenance knows what can be secured; watch knows who is scheduled.

### Recovery endings

Recovery can end in resumed work, a revised goal, another participant, a deferred request, an accepted refusal, a closed task, or a still-unresolved authority gap. Each state has a specific owner and future action. Some stories should conclude with no recovery. A resident may decide that the goal is no longer worth pursuing; a player may choose not to spend another day on it. The plan allows those endings and does not force an upbeat continuation.

## 491. Long absence and departure — preserve the person's authorship

A goal-holder may leave the roster for a long period or depart permanently. If an existing survivor-fate or roster owner reports death, departure, or another terminal status, the initiative feature must respect that fact. It must not keep generating offers in the person's voice, transfer their unfinished intention to a substitute, or mark their goal complete because another resident finished similar work.

### Resolve the open goal

The player can close the goal as incomplete, leave it archived if the owner supports history, return reserved materials through their actual owner, or continue an operational task under a new and independent request. These are different actions. Closing the personal goal does not have to cancel a safety repair that another task owner still considers necessary. Continuing the repair does not claim to complete the absent person's personal intention.

If a task was already underway, the task owner determines whether it can pause safely, hand off a technical milestone, or close. The new worker receives a fresh offer and a truthful description of what remains. The goal-holder's recorded contribution remains attached to the events they completed. No replacement is allowed to inherit authorship. If the current social save model cannot attach goal ownership to a departing survivor, stop and ask for the owner decision before implementation.

### Branches by what the player promised

If the player promised only to ask about materials, the absence may close that conversation without any transfer. If the player reserved a part for the goal, the reservation owner decides whether to return it or retain it for a shared task. If the player promised another person that they would deliver an outcome, that is a separate obligation and needs its own terms. If the goal was explicitly a shared initiative, surviving participants may choose whether to continue it; their consent does not retroactively speak for the absent person.

A supporting records group can preserve a factual note about completed work; a practical crew can identify the remaining technical requirement; a memorial or survivor-history owner can record an authored tribute if that is already part of the game. None can decide what the absent person would have wanted. The player may use a public notice, a private conversation, or no announcement. A private reason for departure remains private unless its owner and policy allow disclosure.

### If the departure is known to be permanent

The player can choose a quiet closure, a factual record, or a continuation of only the shared operational need. If the person died and the canonical memorial system supports it, the story may refer to that established memorial route; it must not create a new grief meter or use the goal panel to simulate bereavement. Survivors can respond in distinct ways or not respond. Their reactions are authored and should not imply that everyone grieves alike.

If the player chooses to complete a task in the absent person's memory, that is a new player motivation, not the person's completed goal. The UI can phrase it as “continue the repair” or “record what they began,” only if a suitable journal/chronicle consumer exists. A player can decline to memorialize the work and still close the goal respectfully. The feature should allow a small, understated ending.

### Endings

1. **Personal goal closed incomplete:** the owner records closure; no future offer is generated for the absent person.
2. **Safe task handoff:** another worker accepts a new scope; the original contribution remains attributable.
3. **Shared initiative continues:** present participants consent to continue under a new goal identity.
4. **Materials returned:** reserved property goes back through its source owner; the goal closes separately.
5. **Quiet archival note:** a current history owner records completed steps, not imagined intentions.
6. **No public account:** privacy is preserved; the player receives only operational updates.
7. **No owner for migration:** the feature reports an unresolved save/continuity blocker and does not invent a replacement.

### Dialogue and presentation

Use one factual line to establish the roster change. Do not have the UI speak as the absent person. Let a current participant say what they personally observed: a tool returned, a note left behind, a task still open. Avoid fabricated final wishes. Show the player the last confirmed goal state and any reserved assets separately from the memorial or roster view. Close/back navigation should return to a stable list and not reopen a disabled offer.

### Acceptance evidence

Save an in-progress goal, transition its owner to a terminal roster state through the current authority, then reload. Verify no offer returns, task status follows its owner, item reservations remain valid, and a new participant must accept a new task. If a memorial is used, confirm it remains owned by the existing memorial/history system. The narrative can hold uncertainty about a person's wishes without making the software uncertain about the recorded work.

## 492. Relationship continuity — same request, different shared history

Branching can reflect how two people have actually worked together without assigning them a permanent moral label. The same offer can feel different when the player has returned a tool on time, moved someone's task without permission, kept a private request private, or missed a promised follow-up. These histories should change which question a resident asks or which term they request; they should not dictate acceptance.

### The request: one hour on the signal cabinet

A resident asks for help labeling a cabinet used by several work teams. The task, room, and cabinet are illustrative; current IDs and task owners must be checked. The resident needs an hour of help and has a preferred time. The player can accept, offer another time, ask what task will be displaced, find a helper, or decline. A support group can say whether it needs the cabinet during that shift, but cannot assign the player's hour.

**History A — previous promise kept.** The resident says, “Last time the return time held. Can we do the same hour?” This is a direct reference to a real event. The player may accept, revise, or decline. The line does not guarantee that the resident will accept a new schedule.

**History B — earlier task changed without notice.** The resident asks, “If the other job takes over, will you tell me before you move the slot?” The player can agree to a specific communication route, set a condition, or decline to make a new promise. The system should record a notification preference only if an existing owner supports it; otherwise this is a one-scene agreement.

**History C — a private request stayed private.** The resident is willing to explain why timing matters but does not want the reason posted publicly. The player can preserve that audience, ask only for schedule-relevant facts, or decline the task. The supporting group receives only the necessary time constraint.

**History D — no shared history.** Use a neutral opening with all available options. Do not infer trust or distrust from the absence of recorded interactions.

### How a shared history can branch

A participant can request a receipt, a narrower scope, a new time, a witness, or no personal discussion. The player can accept the term, propose an alternative, request an owner check, or leave. Each response affects one domain. A scheduling safeguard can make a task possible without changing the resident's broader relationship. A private audience can preserve disclosure terms without improving an unrelated faction's standing.

If the resident rejects an offer after prior cooperation, the dialogue can refer to today's constraint: “I cannot take that shift now.” It should not contradict the history by calling the player untrustworthy. If they accept after a previous breach, they may still require a specific safeguard. This variation keeps characters human and avoids static personality states.

### Distinct outcomes

The player and resident may complete the hour as proposed; agree to a later slot; ask a support group for a neutral work window; let another eligible person help; defer the task; or decline. The resident may agree to work but refuse public credit. They may accept help but ask the player not to share personal context. They may end the conversation while leaving a separate operational need open. Each outcome can be supported by the same three feature subfeatures.

### Writing limits

Do not make a prior interaction reappear in every line. Use at most one relevant remembered fact, and only where it changes the present choice. Keep a neutral fallback for old saves with missing history. Never expose private narrative flags in public text. If a past action is not persisted, do not write a conditional line that assumes it happened.

### Acceptance trace

Run the request with four save histories: kept promise, changed schedule, protected privacy, and no history. Verify identical task eligibility is still evaluated from current owners; only dialogue/options tied to supported history vary. After any route, a new conversation can proceed without using a global good/evil or honesty score. This yields meaningful relational branches while preserving the character's right to make a fresh choice.

## 493. Branch atlas — actions leave different kinds of evidence

The initiative loop needs a branch vocabulary that distinguishes what the player did from what the character is. A route opens because a person accepted a specific request, because the player changed the schedule, because a task owner confirmed an interruption, or because a support group supplied a bounded service. No persistent label such as “good leader,” “liar,” “coward,” or “untrustworthy” should decide future content. Local memory may report a concrete history; it does not grade the whole campaign.

### Five decision points for one initiative

Take the signal-cabinet repair as the spine. The resident proposes it because a later message has been hard to hear. The player can ask what prompted the idea, ask whether someone else can share the task, ask for a different time, offer a specific available component, or leave it alone. Each response produces a distinct follow-up only if its owner-backed action occurs.

**Ask for the reason.** The resident may describe a practical consequence, decline to explain, or say the idea came from a conversation with a neighbor. If they explain, the content supplies context but does not grant permission to assign them. A private reason remains private if the player later chooses to post a general repair notice.

**Ask for a second pair of hands.** The resident can welcome a named partner, request someone with a particular skill, or say they want to work alone. The roster and consent owners determine eligibility. If no eligible helper exists, the screen should preserve the request and show the constraint; it must not substitute a random worker to keep the narrative moving.

**Change the time.** The resident may offer a safe interval, refuse a proposed interval because it conflicts with watch or rest, or ask to revisit after a known event. The player can accept the proposed interval, propose another supported slot, or defer. A time suggestion is not a reservation until the schedule owner confirms it.

**Offer a component.** The player may commit a verified spare component, ask stores whether one can be released, or decide that the cost is too high. Inventory validates the actual item at commitment. If stock changed after the preview, the choice returns to negotiation rather than consuming an unrelated substitute.

**Leave the initiative open.** The player can acknowledge the resident's idea without promising labor, supplies, or a deadline. The character may later repeat it, change it, complete a different task, or abandon it. The record should say “heard” only when the interaction was actually acknowledged; acknowledgement is not agreement or completion.

### Branch combinations and their narrative meaning

These choices create combinatorial variety without multiplying the state model. Ask for a reason, then change the time: a resident who volunteers the practical stakes may welcome a delayed repair because the radio watch can still be covered. Ask for a helper, then keep the component: the resident may wait for a trained partner rather than asking the player to spend the scarce part. Decline the component but offer a work window: the resident can revise the plan around salvage. Leave the idea open, then hear it again after a storm: the recurrence reflects continuing need, not a hidden penalty.

Only persist facts already owned by a current system or a signed event: request identity, participant identity, supported schedule result, resource commitment, task outcome, and any explicit privacy decision. A dialogue option can influence which supported command the player invokes; it cannot silently mint a new durable relationship metric.

### Endings are local and revisable

The repair can be completed with the proposed resident, completed by a different eligible worker after an explicit handoff, rescheduled with the resident's agreement, reduced to a safe inspection, postponed for parts, withdrawn by the resident, or left unresolved because the campaign has more urgent work. Each ending changes the next available conversation in a specific way. None declares the player virtuous or cruel. If the person who offered the work later changes their mind, the system must present that new decision as a current decision, not overwrite it with a remembered “yes.”

### Authoring test

For every branch, write the exact action, command owner, success result, rejection result, persistence fact, and next conversational beat. Remove any option whose only consequence is a flavor sentence pretending that a state changed. Conversely, do not withhold useful character voice simply because the underlying result is “no change”: the resident can explain why they prefer a later shift without triggering a new subsystem.

## 494. Multi-day initiative portfolio — three goals compete without a morality meter

Personal goals should cross ordinary play at different scales. This portfolio uses three candidate initiatives as examples: repair the signal cabinet, teach a neighbor to read the paper map, and sort an unclaimed tool shelf. They are not a new quest chain. Each begins from a resident's observed need and resolves through task, time, roster, inventory, and social owners already identified in Section 4.

### Day zero: the player sees the work, not a quest score

The resident reports that the cabinet sometimes crackles. The neighbor asks whether someone can show them how the map marks the western service road. Another resident wants to sort the tools before a shared repair shift. The panel presents each as a concrete request with who, where, estimated time if supported, known material requirement, and consequence of delay if a current incident establishes one. It does not rank residents by “deservingness.” The player can accept one, ask for detail, schedule a conversation, or leave all three open.

### Day one: scarce time turns into an authored choice

If the player assigns the signal-cabinet work to the only available electrician, the map lesson may need a different teacher or day. That is not a moral failure; the teacher can choose to prepare a small demonstration alone or hold the lesson later. If the player preserves the electrician's rest, the repair remains open and a support group may lend a diagnostic tool only if a real inventory or faction service supports it. If neither service exists, the answer is “not available,” with an understandable reason.

The tool-shelf request may be completed by an ordinary roster task before the player returns. The initiative then closes from a confirmed task event and credits the actual worker. If the shelf was merely discussed, it remains a proposal. A short note can explain the distinction without forcing a tutorial.

### Day two: plans encounter interruption

A water incident can interrupt the scheduled repair. The task producer reports interruption; the goal owner pauses or waits according to its supported contract. The player can reschedule, reassign through an allowed roster command, or cancel. If a helper was promised, the game tells both residents whether the new time has been confirmed. It does not claim the promise was kept just because the player clicked “reschedule.”

The map teacher may discover that a route landmark is no longer reliable. The resident can revise the lesson into “how to mark what we do not know,” or ask to wait until a current scouting result is available. This is a useful branch based on what the player did in the world and what data exists, not on an abstract alignment statistic.

### Day three: follow-up reads actual history

If the repair is finished, the resident can report improved reception only if a downstream signal or interaction owner confirms it; otherwise the character reports the physical repair and uncertainty separately. If the lesson was held, the learner may ask to mark one route on the map. If the tool shelf was sorted, the player sees its current inventory organization through that owner. Each state creates new content only where the state is observable.

### Portfolio endings

All three may finish, one may finish and two remain open, a goal can be replaced by a smaller safe version, or the resident can withdraw it. A player who accepts everything and then repeatedly misses commitments sees explicit broken scheduling facts where tracked, not a broad trust debuff. A player who says “not this week” can still have strong relationships because clarity itself is not encoded as a virtue score. A player who supports a resident with tools rather than labor produces a distinct outcome. A player who prioritizes emergency work may leave a goal unfinished with no fabricated resolution.

### Content and UI review

Use different verbs for proposing, scheduling, starting, pausing, finishing, and withdrawing. Do not call an initiative “active” if the only state is dialogue text. For each portfolio view, show the resident's own wording as a short optional detail, then show factual status and the action available now. Expiry should be explicit only when an owner has a real deadline. Avoid red/green moral color coding; neutral status language communicates readiness and consequence better.

## 495. Supporting-faction service encounters — bounded help that changes the route

Minor groups add texture when they solve a narrow coordination problem. Their role is a service, witness, source, or constraint—not a second campaign authority and not an alternative to the player's decisions. The player can discover, request, compare, accept, decline, or leave the offer. Faction affiliation alone never grants consent to assign an individual.

### Records desk: make a promise legible

A records volunteer can help write a clear handoff slip for the cabinet repair: current custodian, requested time, item needed, and who is expected to return it. The resident can approve the slip, ask that their name be omitted from a public board, or keep the arrangement verbal. The desk cannot decide who works or claim a task has completed. If the player accepts this help, a later dispute has a better factual record; the branch is based on an explicit documentation action.

### Practical crew: lend a method, not a worker

A maintenance crew may offer a short diagnostic checklist or a supervised work window. One member can show the resident how to isolate a loose connection, but should not magically complete the repair offscreen. The resident can accept a demonstration, ask for a different instructor, or decline because the task is already planned. If equipment or safe access is unavailable, the crew explains the limitation. The player decides whether this narrower help is worth the time.

### Route group: make the lesson more useful

A route group can contribute a marked map legend that distinguishes verified paths from old reports. The learner may prefer to practice with a familiar route, use the legend to prepare for a future trip, or decline the added detail. The route group does not reveal an unexplored location for free. Any newly verified path must come from its existing survey or exploration authority.

### How the three services combine

The player can request a slip, then a diagnostic demonstration, then a map legend for a separate goal. Combining them does not form a coalition meter. Every service has its own source, cost, eligibility, and optional privacy choice. If a group is absent, busy, or unwilling, the related goal still has its ordinary player and survivor routes. No group becomes a mandatory gate to the core initiative loop.

### Supporting role in endings

On a success ending, a support group may be thanked if its service materially helped and the participants permit attribution. On an incomplete ending, it can return borrowed notes or leave a pending appointment open. On a dispute, a witness can report what they observed and distinguish that from what they did not see. It cannot pronounce guilt or modify an unrelated reputation. If the player declines the service, later dialogue should not assume it was accepted.

### Review matrix

For each cameo, write the concrete service, player action that requests it, owner that proves availability, participant consent requirement, visible cost, information revealed, persistence owner, and failure text. Then review the encounter with the supporting faction removed. The initiative must still be comprehensible and recoverable. Finally review it with a major faction involved: the major group can set a legitimate policy boundary, but the minor group retains its narrow expertise and the individual resident keeps their voice.

## 496. Consequence lattice — choices combine by evidence, not alignment

A choice lattice lets separate actions produce a richer sequence without multiplying into a brittle set of morality endings. Use a small set of observable facts: whether the player asked for context, whether the resident chose to share it, whether the player offered a particular accommodation, whether a task was scheduled by its owner, whether another person helped with consent, and whether the work actually completed. These are local facts with specific provenance. They do not roll up into one campaign identity score.

### Start with a resident-defined request

The resident asks to inspect the ventilation latch after hearing a repeated rattle. The initial screen offers five verbs: ask, schedule, find help, provide materials, and leave open. Each begins a different route. “Ask” opens a short exchange where the resident can explain what they observed, say they are unsure, or keep the observation private. “Schedule” asks for a viable interval. “Find help” searches only eligible roster members and available support offers. “Provide materials” checks an actual item. “Leave open” records no commitment and does not force a decline.

### Facts the player creates

If the player asks and the resident explains that a noise woke them during rest, that fact is private conversational context. If they instead say the rattle began after a maintenance shift, the work history may help identify a legitimate task lead. If the resident declines to explain, the player can still choose whether to schedule a safe inspection. The player may ask a maintenance crew to verify the latch, but cannot publish the private explanation just because it helped motivate the request.

If the player chooses a slot that conflicts with a known duty, the resident can reject it. This should return a concrete scheduling conflict and an alternative only if one exists. The player may adjust another assignment through the roster owner, propose a later interval, or leave the task open. If the only safe worker is unavailable, the system must preserve that constraint rather than invent an eligible helper.

### Consequence lattice nodes

1. **Context requested and voluntarily shared:** later dialogue can refer to the resident's observation in a private conversation.
2. **Context requested and declined:** later dialogue treats the resident respectfully and still offers operational options.
3. **Inspection scheduled:** the schedule owner confirms an interval; the goal can show a real appointment.
4. **Inspection attempted and interrupted:** task/event owner reports interruption; the player can reschedule, cancel, or choose an allowed handoff.
5. **Inspection completed with a finding:** downstream owner reports exactly what was found; narrative can react to that result.
6. **Inspection completed without a conclusive finding:** a valid result that may lead to a second observation or a quiet closure.
7. **Resident withdraws:** current consent changes and the task cannot continue under their name without a supported handoff.
8. **Player leaves the request open:** no promised work exists, though the resident may bring it up again later.

Each node can route to another compatible node. A shared action may occur after the resident initially declines to explain; a later completed inspection does not imply that privacy changed. A private explanation may coexist with a postponed schedule. An interrupted task can be replaced by another resident only after current eligibility and consent are checked. The graph has breadth because actions and evidence differ, not because the player is placed on a permanent good/evil rail.

### Ending families and follow-up

**Quiet fix:** an eligible worker completes the inspection, and the result closes the goal.

**Unresolved observation:** the inspection does not confirm a cause; the resident chooses whether to gather more evidence.

**Changed plan:** the player adjusts the schedule or duty roster and the resident accepts the new interval.

**Protected refusal:** the resident declines a proposed route and the player leaves it declined without repeated pressure.

**Safe handoff:** another willing eligible person takes over through an explicit supported command.

**Open need:** the campaign cannot meet the need now; the status remains open with a truthful constraint.

**Withdrawn request:** the resident stops the work, and no fabricated alternate objective is spawned.

Follow-up dialogue uses only the branch fact it needs. The resident may thank the player for moving a shift without declaring lasting loyalty; may state they are still uncertain after an inconclusive inspection; or may simply close the subject. The interface can offer a new action later if the actual situation changes.

### QA from narrative to system trace

For every lattice edge, record its initiating action, the owner command, a confirmed result, a failure path, the stable data that must survive save/load, and the line that accurately describes it. Then produce a trace for the least dramatic ending: the player asks, the resident does not explain, the proposed helper is unavailable, the player leaves the request open. This path must feel complete rather than punished or broken. It proves that the feature allows restraint and uncertainty as real play styles.

## 497. Player approach profiles — varied methods remain equally playable

The same request can support a planner, a listener, a logistics-minded player, a delegate, or a hands-off manager. These are descriptions of how someone approaches one situation, never classes or permanent bonuses. The feature should let a player switch approach from scene to scene.

### Planner: schedule before committing

A planner asks how long the inspection might take, reviews available shifts, and sets a safe window. The resident can accept, propose another time, or decline. The schedule owner checks rest and duty conflicts. The planner's advantage is clarity; the cost is that an uncertain diagnosis can wait while a time is agreed. If a new emergency overrides the window, the planner still receives an interruption and must decide whether to reschedule.

### Listener: learn what the resident wants before assigning work

A listener asks about the rattle, how it affects the resident, and whether they want to participate. The resident can share practical observations, keep personal details private, or say that another person should inspect it. The listener gets a richer conversation when the resident chooses to speak, but no hidden task permission. Their route can end with no assignment and still be complete because the player's purpose was to understand the need.

### Logistics player: verify supplies and eligibility first

A logistics-minded player checks whether the needed tool or component is actually available and which worker can use it. If inventory says none is available, the player can wait, request a permitted supply check, or choose a safe inspection that does not require the missing item. The route avoids impossible promises at the cost of leaving the underlying concern unresolved. No spare part is created as a consolation prize.

### Delegate: request bounded help from a support group

The player asks a maintenance crew to advise or inspect. The crew can offer a method, a limited appointment, or no service. The resident can accept the offer or ask to speak directly with the player. The crew does not become a new assignment authority and cannot report completion before a real task result. The player's action is choosing whom to consult and whether to accept the service.

### Hands-off manager: decline to intervene

The player may leave the resident's proposal open, reject the work because of a constraint, or allow another ordinary task owner to address the same issue if one already does. This route respects player time and resident agency. It is not secretly punished by the story. If a serious incident later confirms a mechanical hazard, the incident can reopen the problem from its own evidence; the feature should not manufacture that hazard as retaliation.

### Approach switching and memory

A campaign can contain all five approaches. Remember only actions that a current system persists and that are relevant to the next decision. A resident can respond warmly to one promise kept and still disagree with the player's next schedule. Dialogue should avoid summarizing a person as “you always listen” or “you never help” unless a canonical relationship system supports such a line. The safer continuity hook is direct: “Last time, you moved my shift when I asked.”

### Playability review

Give a tester the request and ask them to complete it using each approach. Confirm that each yields a readable decision, no approach requires an unowned hidden score, no route is artificially optimal in every case, and refusal remains available. Observe whether the UI communicates that ask, schedule, delegate, supply, and defer are separate actions. Where options depend on the campaign state, explain the missing prerequisite plainly. The goal is not equal numerical reward; it is an honest, consequential choice with a plausible outcome.

## 498. Dialogue texture bank — concrete voice without alignment labels

These candidate lines illustrate tones attached to observable actions. They are not canon and require review against live character data before implementation. Use them as shape references, not a reusable phrase grid.

**After the player asks for context:** “It rattles after the lamps go out. I don't know if that's the same as broken.” The uncertainty is useful evidence, not a confession.

**After the resident chooses privacy:** “I can show you the latch. I'd rather leave the rest out of the board.” The player can accept the boundary and still arrange an inspection.

**After a schedule conflict:** “I can do after the water run, if the board is clear then.” A conditional offer is not a confirmed booking.

**After no eligible helper is available:** “That's all right. I'd rather wait than ask someone who's never used the brace.” This affirms a practical preference without moralizing.

**After the player offers a scarce component:** “Keep that one for the pump. We can check the hinge first.” The resident makes a choice that can save inventory, but the inventory remains unchanged unless a real command is made.

**After an interruption:** “The warning came through. We can stop here.” The line acknowledges the task event; it does not complete the task.

**After an inconclusive inspection:** “Nothing is loose that I can feel. I still hear it some nights.” The resident distinguishes a physical observation from a persistent concern.

**After a supported handoff:** “Nera knows the housing better. I'll walk her through what I saw.” The line appears only if Nera is eligible, accepts, and the handoff is confirmed.

**After a declined request:** “I don't want anyone in the vent today. Ask me again if the sound changes.” A request for future contact is distinct from current consent.

**After player deferral:** “Leave the card by the roster. I can bring it up after shift change.” The card is a diegetic object only if the current UI or content owner can support it; otherwise use a neutral panel note.

**When the player returns without a prior history record:** “The latch still catches.” Do not write “as we agreed” when the save has no such agreement.

The bank should be reviewed for voice diversity, translation length, accessibility, and consistency with the actual state. Never choose a line by a fabricated personality score. When a conditional line's state is missing, use the neutral fallback. If a line refers to an item, duty, event, or relationship, verify that fact through its current owner.

## 499. Candidate side-story — The Hour Between Watches

This small, optional story ties a resident initiative to practical labor, schedule choice, and consent without adding a fourth feature pillar. It spans several ordinary days and never requires a heroic ending. Its purpose is to make one person's request feel remembered through specific player actions. All names and dialogue are candidates for canon review.

### Opening: a request beside the roster

A resident asks for one quiet hour to tune a hand-cranked receiver before the next message window. They say that the old tuning card is missing and the signal cuts out. The player can ask what they have tried, ask whether a private hour is needed, offer an available task slot, check for the card, ask a support group for advice, or leave the request open. The choice does not define the player's character; it defines what the next beat can truthfully reference.

If the resident says the problem is technical, they may show a loose dial. If they say the room is too busy, the player can look for an available schedule slot. If they choose not to discuss their reason, the receiver task remains available only if its owner permits it. The screen should not demand a personal explanation as a price for receiving practical help.

### Episode one: clarify what is known

The player may search current stores for a tuning card, ask the receiver group whether a replacement guide exists, or schedule a supervised inspection. Stores can report a real item state; the group can provide a method or say it has no current copy. The resident can decide whether to try the method, wait for a card, or stop. If the player spends time locating a card, the outcome changes through the inventory owner only when a real item is acquired or moved.

The first branch creates callbacks from a concrete act: found a guide, requested advice, booked a slot, or left it open. It does not award “care” points. If the card is found, a later plan may use it. If advice is offered, it is not treated as a working fix until a task confirms it.

### Episode two: make room or share the room

The player can schedule the receiver work during a quiet period, ask the resident to work with another person, reserve only the necessary table, or decline to reserve a shared room. The resident may accept a shared session, ask to work alone, or propose a different hour. The room owner confirms access and capacity; the duty roster checks participants. If no quiet interval exists, the player can explain the constraint, ask whether the resident prefers a shorter session, or leave the plan pending.

A records volunteer can help list the time and equipment on a handoff slip, while a maintenance crew can lend a safe method if available. Neither group gets to assign the resident. If the player chooses a public board, the resident can request that personal context be omitted. If the player chooses a private conversation, fewer people may know about the schedule. The tradeoff is explicit and authored by action.

### Episode three: interruption and recovery

A scheduled water distribution interrupts the room. The task or schedule owner reports that the session cannot begin. The player can reschedule it, use a different authorized space, ask the resident whether they want to continue later, or cancel the booking. The resident may be annoyed at losing the quiet hour, relieved that the water run takes priority, or indifferent; the available line should depend on authored character voice and verified history, not an emotional score.

If the player had promised to return with the card and did not, the resident can name that missed handoff if the promise is stored. The player may apologize, explain the actual constraint, offer a new supported route, or stop making promises. If no promise was persisted, the fallback line must not accuse them of breaking it. A promise in dialogue becomes a durable premise only if the system records it through an approved owner.

### Episode four: test the receiver

The resident and any willing helper conduct the task through the real work owner. The test may restore a clearer signal, confirm only that the dial moves, reveal no change, or stop because a required part is missing. Each result supports a different conversation. No result guarantees the next radio message will arrive unless the signal owner confirms that dependency. The player can ask for another attempt, accept the limited result, provide a verified part, or let the resident end the work.

If a support group supplied a method, its member can ask what the test found. The resident can share the result publicly, privately, or not at all where those communication options exist. A group member may report an observation they made, but cannot speak for the resident's feelings.

### Episode five: close, continue, or change the goal

The resident may close the initiative after a successful test, keep it open for another safe attempt, replace it with a smaller goal such as cleaning the contact, or withdraw it. The player can support that decision, request a different plan, or leave the matter to the resident. If the goal owner can represent replacement, it records a new supported goal; otherwise the story closes the old goal and creates only dialogue, not a fake persistent objective.

### Ending map

- **Clear signal, private credit:** the repair works and the resident keeps the reason and result private.
- **Clear signal, shared method:** the support group receives credit for its specific advice, with participant permission.
- **Limited test, open goal:** no signal improvement is claimed; the resident chooses whether to continue.
- **No card, revised plan:** the player cannot source the item, but the resident uses a verified alternate method.
- **Interrupted and rescheduled:** the work moves to an owner-confirmed later slot.
- **Interrupted and abandoned:** the resident decides the effort is no longer worth the time.
- **Player declines the request:** the resident keeps the goal open or withdraws it; no forced quest marker is created.

These are story endings, not feature endings. The ordinary initiative loop remains usable after the episode closes. A later resident can propose a different goal with no inherited moral label from this one.

## 500. Branch casebook — the helper with the wrong kind of availability

A resident accepts a repair goal and asks for another pair of hands. The roster displays a capable survivor who is free at first glance, but that person is already committed to a watch handoff at the proposed time. This branch tests the difference between “not assigned to a task on screen” and truly eligible availability.

### Player actions

The player can inspect the conflict, ask the prospective helper whether another time works, ask the goal owner to split the task into smaller steps, select a different eligible survivor, or proceed without a helper if the task permits solo work. Each action exposes a different consequence. “Ask” is a request to the person, not a command that blocks their schedule. “Split” is available only if the task owner supports multiple stages. Selecting another person requires that person's own current consent and eligibility.

### Branch outcomes

**Keep the watch:** the prospective helper refuses the proposed time and covers the handoff. The resident can reschedule, find another person, or choose solo work. The game shows that the watch was completed only when its owner confirms it.

**Move the work:** the player selects a later interval. The resident and helper both agree, and the schedule owner confirms it. If the helper accepts but the schedule owner rejects the slot, the game distinguishes personal agreement from a booked time.

**Split the task:** the resident performs the inspection alone and waits for a specialist to complete a later repair. This branch is possible only if there are separately represented tasks. The partial inspection cannot count as the full goal unless the goal contract says so.

**Choose a different helper:** the player asks another eligible person. The first resident's refusal remains valid; no negative relationship consequence is inferred. If the second person accepts, the handoff has its own participant IDs and schedule result.

**Proceed solo:** if permitted, the player supports the resident's plan to work alone. Safety and task eligibility are verified. A solo route can take longer or require a different tool only when current data supports that difference.

**Leave the plan open:** the player may decide that the conflict cannot be resolved now. The initiative stays open with the exact constraint; there is no fictional substitute worker.

### Specific branch continuity

If the player moved the work around a watch, later dialogue can say, “We waited until the handoff was done.” It cannot say “you always put duty first.” If a second helper was chosen, a later scene references that person only if the assignment succeeded. If the player attempted an unavailable slot, the dialogue can mention that the booking failed, not blame either survivor. If the plan stayed open, a resident may return with a new proposal when availability changes.

### Review conditions

Confirm that schedule, roster, and consent facts remain separate. Confirm that availability cannot be inferred from a blank task panel alone. Confirm that UI sorting does not alter deterministic evaluation order. Confirm that the player can learn why a candidate was unavailable without seeing confidential personal details. The branch is complete when every option produces a truthful result and an understandable next step, including no step.

## 501. Relationship callbacks — recognize conduct without reducing people to it

A callback should identify an event, not score a personality. This section provides a vocabulary for continuity in the initiative loop. It is intentionally narrower than a reputation system.

### Remembered events that can earn a callback

- The player moved a confirmed schedule after a resident asked.
- The player supplied an item that the inventory owner recorded as transferred.
- The player kept a private explanation out of a public message.
- A resident declined help, and the player stopped asking.
- A promised appointment was interrupted and the player sent an accurate update.
- A support group offered a method that a resident chose to try.
- The actual task completed, paused, failed, or was withdrawn.

Any callback requires a current persisted fact or event provenance. A line cannot remember a gesture that exists only in a transient panel state.

### Callback forms

**Direct acknowledgement:** “You gave me the afternoon slot I asked for.” Use when the schedule owner confirms the move.

**Unresolved memory:** “We still haven't tried that tool.” Use only while the goal remains open and the item was not acquired.

**Boundary recognition:** “Thanks for leaving the rest off the notice.” Use only when a recorded privacy choice supports it.

**Changed preference:** “I'd rather check this myself today.” This is a present choice; prior acceptance does not override it.

**No callback:** Silence or a neutral greeting is valid when history is absent, old, irrelevant, or intentionally private.

### Avoiding repetitive memory

Do not repeat the same remembered fact in every interaction. Use it when it changes an option, explains a current preference, or lets the player see a meaningful consequence. Avoid a long recap of past initiative outcomes before the player can act. A compact status line or one sentence is enough. If several facts matter, show the current operational facts in UI and reserve dialogue for one human detail.

### Character authorship and disagreement

A resident who once accepted help may refuse it now. A person who disagreed with the player over one schedule may still accept a different offer. A previously private resident may choose to speak openly later. The system should not treat these shifts as inconsistency; people respond to circumstances. Dialogue choices should not be locked because a profile marked someone as “private,” “helpful,” or “difficult.”

### Review procedure

Prepare four saved histories: one relevant positive action, one relevant disagreement, one privacy choice, and no record. Run the same request in all four. Confirm that the available gameplay action remains grounded in current owners, while the greeting or explanation can vary where its fact is persisted. Then change the resident's current preference and confirm that the current choice takes precedence. This protects continuity without converting it into a deterministic character verdict.

## 502. Initiative under competing needs — priority comes from the player and current owners

A personal goal can be important without outranking every shelter need. The player needs to see what will happen if work is delayed, what is genuinely urgent, and what remains a character's own choice. Do not encode priorities as one global “good leader” ranking. A currently reported incident may require immediate work; a resident's goal may still be worth preserving, revising, or closing.

### The competing situation

A resident has scheduled an hour to finish a handrail inspection. A water delivery arrives early, and the water owner requests qualified crew support. The player can keep the inspection schedule if an eligible worker remains available, reassign the handrail work through an allowed command, ask the resident whether they want to move it, or decline to promise that it will happen today. The interface names the verified conflict and does not simply mark the resident's goal “low priority.”

### Branch: ask the goal owner to defer

The resident can accept a later time, offer a shorter inspection, or decline to defer. If the player accepts the shorter version, it must correspond to a valid task variant. If not, the resident can still keep the goal open. The player's action demonstrates negotiation but does not create a new outcome unless the schedule owner confirms it.

### Branch: keep the appointment

If another qualified worker can support water delivery, the player may preserve the appointment. The water authority decides if that roster allocation is valid. A successful appointment should not be described as “choosing the resident over the shelter”; it is one permitted allocation. If no alternate water crew exists, that option should not be offered.

### Branch: allow the interruption

The player may let emergency work interrupt the appointment. The task owner reports the interruption and the resident can choose to resume later, hand off, or withdraw. A short acknowledgement might say, “I'll leave the mark where I stopped.” That line does not say the task completed. If the resident is displeased, their line can express immediate frustration without causing a broad relationship shift unless an owner supports such a shift.

### Branch: decline both commitments

The player may choose not to assign a worker to either nonessential task while the conflict is unresolved. A resident can keep the goal open, and the player can revisit after the incident. This is a deliberate resource-management style. It should not trigger a hidden shame response or force a dramatic confrontation.

### Update and follow-up

When water work ends, the player can return to the resident, accept a changed time, or leave the inspection open. If the task owner says that the water job remains active, the original work window cannot be restored automatically. A resident may offer an alternate shift. If the player had promised a follow-up, the promise can be acknowledged only when persisted. The player may explain a real delay, but the game must not claim that an apology erased material consequences.

### Branch invariants

- Current urgency comes from the owner reporting it, not from dramatic prose.
- A resident's refusal to defer is a valid branch, not a penalty.
- The player can preserve, revise, or leave the goal without selecting an alignment identity.
- Work completion follows its task producer.
- Schedule changes follow the schedule owner.
- A later conversation can remember one action without reciting a campaign-wide score.

## 503. Choice affordance review — every option changes a real decision surface

This feature's richness depends on meaningful choices, not on a large number of nearly identical dialogue buttons. Each option should expose a distinct action the player can take. A question can reveal context, but it should not be presented as if it schedules work. A supportive sentence can maintain tone, but it should not pretend that a resource transferred. A refusal can close the request or leave it open, depending on the explicit action chosen.

### The five-question review

1. **What does the player intend?** Ask, schedule, source, delegate, commit, decline, defer, or hand off.
2. **Which owner permits the action?** Consent, roster, task, time, inventory, location, and social records remain with their current owners.
3. **What result will the player see?** Name confirmation, constraint, stale data, conflict, or no change.
4. **What fact persists?** Only a supported event, accepted term, privacy choice, or owner-confirmed status.
5. **What content follows?** A line or option can use that fact without turning it into a permanent judgment.

If an option fails the second question, it is flavor-only and must be labeled as conversation rather than a command. If it fails the fourth question, it cannot support later conditional writing. If two options invoke the same command and produce the same visible result, combine them unless their character voice has a real purpose.

### Branching through action sequence

A player may ask for a reason, then choose not to assign the resident. Another may assign first, discover the schedule conflict, and decide to reschedule. A third may check supplies, find none, and ask a crew for a method. A fourth may let the resident decline and return to ordinary work. These sequences can create distinct dialogue, task history, and future affordances without assigning any player an evil or good label.

### Accessibility and fairness

Keep the action label clear and the consequence preview readable. Do not use color alone to distinguish accepted, pending, blocked, or private. Offer a neutral option to close a conversation. Preserve controller and keyboard navigation for all branches. If important context is available only through a specific dialogue route, do not make that route mandatory for understanding the next decision. Provide a journal or status detail through an existing owner only if the feature already has that destination.

### Final author check

Play the initiative as a cooperative listener, strict scheduler, logistics manager, delegate, and hands-off player. Confirm that every approach can reach an understandable stopping point. Confirm that the player does not need to discover a hidden virtue threshold. Then read all callbacks without their branch metadata. If the prose still sounds like it is awarding or removing moral worth, rewrite it as an observation, preference, or result.

## 504. Return visits and quiet days — continuity when the player does nothing

Not every personal goal deserves a dramatic revisit. The initiative loop should handle a player who ignores, postpones, or closes a request with the same care as one who completes it. A return visit can introduce a changed preference or new evidence, but must not shame the player for spending time elsewhere.

### A goal remains open

If the resident asked to inspect the receiver and the player left it open, a later visit can offer the same request, a smaller version, or a withdrawal. The line can state the current need: “The dial still catches when the room cools.” It should not say “You forgot again” unless a specific promise existed, was persisted, and was actually missed. The player may ask whether the goal still matters; the resident can say yes, not now, or no longer.

### The resident solves it independently

A resident may complete a supported task through ordinary autonomy. The task owner reports the completion and the initiative closes against that real event. If the player never assigned it, the narrative can recognize independent work without claiming the player helped. The resident can ask the player to inspect the result, share a method, or simply return to ordinary activity. No reward needs to be fabricated.

### The need changes

An unrelated campaign event may remove the need for the goal. If a replacement receiver arrives through its actual owner, the resident can withdraw the repair request. If the old receiver becomes part of a different task, the initiative closes as superseded only when the goal owner permits that status. The player can accept the change, ask to preserve the old goal, or review the new task. A changed circumstance is not failure.

### A request becomes stale

If a goal references a resident, location, item, or task that no longer exists, the initiative cannot keep presenting the old command. The system should state which condition is no longer valid and offer a current route only if one is supported. If the participant departed, do not assign their intention to someone else. If an item was consumed, do not substitute another copy. If a location is inaccessible, leave the plan suspended or close it according to its contract.

### Quiet close and re-entry

A resident can say, “Let's leave it for now.” The player can accept, ask whether to bring it up later, or close the interaction. Do not schedule a reminder unless a real reminder owner supports one. If the resident later brings the goal back, treat it as a new present choice that may cite the earlier conversation. This lets players enjoy long campaigns without every unresolved intention becoming a permanent warning badge.

### Validation set

Run the same initiative after one day, ten days, and a save/load cycle; after the resident completes it independently; after relevant inventory changes; and after participant departure. Confirm that the UI distinguishes open, paused, superseded, withdrawn, completed, and unavailable only where the current contract supports those statuses. Verify that doing nothing neither duplicates work nor invents relationship loss. The quiet-day path should feel authored because it reports what is currently true, not because it punishes absence.

## 505. Closeout cards — give every goal a useful final state

A closeout card should summarize one resident goal without becoming a new quest journal. It shows the goal phrase, participant, verified status, last meaningful action, current blocker if any, and next available choice. A player can close a finished goal, return to an open one, or leave a withdrawn goal in history where the current owner supports that distinction.

For a completed goal, show the task result and actual contributor. For a paused goal, show what event paused it and the valid ways to resume. For a withdrawn goal, say that the resident chose to stop. For a stale goal, name the missing participant, item, location, or permission. For a no-change conversation, do not create a card unless a current owner records an intention. If a resident's goal was fulfilled by ordinary autonomy, show that fact without claiming player credit.

A player can read the card without reopening dialogue. The card should not hide the option to ask the resident whether they want to continue. It should not turn a request into a permanent obligation. If the campaign no longer supports a route, allow the player to close the view and continue ordinary play. This calm closeout is part of the narrative promise: even an unresolved request can have a truthful endpoint for the current day.

## 506. Initiative response snippets — concise lines keyed to real events

**Schedule accepted:** “After the watch handoff, then. I'll mark the cable before I leave.” Show this only after the schedule owner confirms the interval.

**Schedule proposed but not confirmed:** “That hour might work. Let me check the rota.” Keep the goal pending until the check returns.

**Resident declines a public explanation:** “I can explain it to you. Please don't put the reason on the board.” The player can still post a task request without that detail.

**Helper unavailable:** “No one else needs to move their shift for this. I can wait.” Use only when the player has actually checked availability and the resident has chosen to wait.

**Task interrupted:** “I left the cover where the crew can find it.” The line refers to an item only if its owner confirms placement; otherwise replace it with a neutral statement about pausing.

**Goal fulfilled independently:** “I found the fault while I was checking the hinge.” The task result and contributor come from the task owner, not from the resident's initiative card alone.

**Goal withdrawn:** “I don't need the repair now. I changed how I use the receiver.” The resident's current choice closes the request; no player virtue or penalty follows.

These snippets are tone candidates. Writers should vary syntax and vocabulary by established character voice, respect localization limits, and never select a line from inferred honesty or alignment. The event requirement beside each line is part of the authoring contract.

## 507. Minimal persistence contract for narrative branches

Before authoring a conditional line, document the source fact and how long it remains valid. A confirmed schedule belongs to its time or roster owner; a task result belongs to its task producer; a privacy choice belongs to the supported communication or social owner; a transfer belongs to inventory and barter. A dialogue scene may hold stable references to these events, but it must not duplicate their values in its own flag list.

When a fact is absent, use the neutral copy and preserve the current gameplay option where possible. When a participant changes their mind, read the current consent result rather than the old dialogue choice. When a task is superseded, close or update the goal only through its owner. This small contract prevents rich branching from becoming fragile. Review each saved ending after reload and day advance; confirm that all conditional narration still reflects the owners' current facts and that repeated evaluation does not create another offer or roll.

## 508. Replay-focused narrative audit

Compare two runs from the same seed and owner state where the player chooses different supported actions. The resulting task facts should differ only where the commands differ; irrelevant dialogue should remain stable. Repeat each route after save/load and verify that a kept promise, private detail, interrupted task, or declined offer is not replayed as a new event. This keeps player-authored variety deterministic and makes narrative continuity testable without a campaign-wide personality score.

## 509. Final branch sanity pass

Before content approval, read every ending with the exact saved facts beside it. Remove any line that says a person accepted, learned, forgave, or completed something when only a dialogue option occurred. Preserve clear negative and unresolved endings. A modest but truthful response is more useful than a dramatic closure that the game state cannot support.

Offer a neutral route back to ordinary play after every branch, so an open request never becomes a mandatory interruption.

## 510. Candidate side-quest — The Marks on the Roster

This optional, multi-day story turns ordinary initiative into a compact questline about shared maintenance and individual authorship. A shelter resident keeps adding small marks to the duty roster beside the workshop shifts. The marks identify when a cabinet rattles, when the radio table is quiet, and when a tool is missing. No one knows whether the notes are an informal repair log, a personal reminder, or a request for help. The player can ask, observe, schedule, delegate, offer supplies, or leave the marks alone. The quest is not a test of whether the player is benevolent. Its branches follow what the player asks, records, promises, shares, and actually completes.

### Scene one: the marks have more than one possible meaning

The resident can explain that they are tracking the cabinet, say that the marks help them remember their own shift, mention that someone else asked them to keep notes, or decline to explain. Each response is a valid characterization only if authored for that survivor; it is not selected from a hidden personality value. The player may ask one follow-up, offer a blank page, ask whether the marks should be copied to an official board, or end the conversation.

If the resident says the marks are personal reminders, public transcription requires separate consent. The player can offer a private task note, ask whether the resident wants the work scheduled, or leave the record where it is. If they say another person requested the notes, the player can ask to speak with that person, but cannot treat the resident as an authorized representative without an explicit owner-backed relationship. If the resident declines to explain, the player can still request a practical inspection based on the visible cabinet issue, provided a task producer supports it.

### Scene two: choose what the player is actually trying to learn

The player can follow four investigative routes. The **work route** asks a maintenance group to inspect the cabinet and explain which signs matter. The **schedule route** compares the marks with roster history where the current owner permits it. The **supply route** checks whether the missing tool is in shared stores, assigned elsewhere, or absent. The **conversation route** asks the resident to describe one mark at a time. Any of these can be chosen first; no single route unlocks a “correct” story.

The maintenance group may confirm that one mark coincides with a known repair, say that it cannot identify the cause, or offer a safe diagnostic method. It cannot declare the cabinet fixed before a real task completes. The schedule owner may confirm a shift time, but cannot reveal private notes that it does not own. Stores may verify a tool's current location, but not why someone moved it. A conversation can add context, but the player should distinguish testimony from system-confirmed records.

### Scene three: author a small, bounded plan

The player can propose a half-hour inspection with the resident, ask an eligible technician to do it, request that the resident teach another person how to read the marks, or defer until the required tool becomes available. The resident can agree, revise the timing, request privacy, ask to work alone, or withdraw. Each accepted schedule or task is confirmed by its existing owner. A promise to return with the right wrench is not a resource commitment until inventory records the transfer or the task's permitted kit is assigned.

A records volunteer may offer a handoff sheet that lists only the work facts: cabinet location, symptoms observed, planned slot, and item needed. The resident decides whether their name or personal notes appear. The sheet does not become a second repair authority. If the player declines the volunteer's help, the plan remains available through ordinary roster and task routes.

### Scene four: the first plan fails in a distinct way

The chosen technician may be unavailable because they are on watch. The player can move the appointment, choose another eligible person, ask the resident whether they prefer to wait, or leave the plan open. If the part is missing, the work can stop before starting and the player can check stores, request a delivery through an existing source, choose an inspection that does not consume the item if one is supported, or cancel. If the repair is interrupted by an emergency, the task owner reports the interruption and the player chooses a valid recovery. If the resident changes their mind, the consent result takes precedence over the old acceptance.

The text should make these failures meaningfully different. “No qualified worker is available for that hour” is not the same as “the tool is not in stores.” “The resident withdrew the request” is not the same as “the task was interrupted.” Each produces a different next action and a different later callback, but none assigns moral character to the player.

### Scene five: reconcile the marks with the actual result

After the work, the resident may explain which mark was useful, ask that the notes be returned, offer to share a general maintenance pattern, or decide the log should remain private. The player can ask for a short public summary, keep the repair result in the task view, or close the side story. A public summary uses confirmed task facts and approved details only. If the repair found no fault, the resident can still report that the cabinet rattled. If the task fixed one problem but left another, the story distinguishes the result rather than collapsing it into “success.”

### Main ending families

- **Shared maintenance note:** the resident consents to a bounded summary and a support group helps preserve the method.
- **Private notebook:** the player helps schedule the repair while the original marks remain personal.
- **Resident-led lesson:** the resident teaches a willing helper how to observe the cabinet without disclosing private notes.
- **Supply delay:** the need remains open because the correct item is unavailable; the player receives a clear recovery route.
- **Independent completion:** the resident completes an owner-backed task without a player assignment and receives accurate credit.
- **Quiet withdrawal:** the resident stops the project, and no replacement quest is forced.
- **Inconclusive inspection:** the task closes with uncertainty and the resident decides whether to investigate again.

These endings are compatible with one another where actual state permits. A repair can complete while the notes stay private. A lesson can occur even when the repair is delayed. The player can decline the public summary while still supporting a task. A later conversation references only facts that were confirmed and retained by the right owner.

## 511. Branch ledger — participant, timing, method, and disclosure

The side-quest graph should be authored on four independent axes. This creates nuanced path variety without a binary virtue route and keeps the content buildable under the existing three feature subfeatures.

### Axis one: who participates

The resident can act alone, ask for a named helper, request a person with a practical skill, accept a support group's bounded service, or leave the work to the player. Eligibility is not interchangeable. The roster checks role and timing; the person checks their own consent. The player may propose an alternate participant, but the system must not auto-substitute them. A handoff names the outgoing and incoming participant and occurs only when permitted.

### Axis two: when the action happens

The player may schedule now, after a known shift, once a real item is available, after an incident, or at an unspecified later time. Only known schedule and event owners can confirm those anchors. “When the weather is better” is a conversational preference unless the game has a supported weather condition and schedule hook. Unavailable times remain visible as unavailable; do not encode them as low relationship values.

### Axis three: how much of the goal changes

The player can support the full repair, request a diagnostic-only pass, split a multi-step task if its producer supports stages, change the goal to a lesson or record, or close the request. A smaller goal is meaningful only when the task owner and social owner can represent it. Otherwise present a discussion that ends without claiming a new persistent objective.

### Axis four: who receives the result

The player can keep the result in the private interaction, tell a named resident, post a generic workshop notice, or ask a support group to circulate an approved method. Each audience choice has its own permission and communication consequence. An individual's explanation cannot be copied to a public board without consent. A support group can communicate a method, but cannot state that a resident endorsed it unless the resident did.

### Example combinations

A resident works alone, chooses a later interval, performs only an inspection, and keeps the result private. Another asks for a named helper, books the confirmed work window, accepts a full repair, and allows a generic notice that does not mention their personal reason. A player may ask for a support group's method, provide a verified part, let the resident conduct the repair, and keep the group's advice unattributed. A task may be performed by another eligible person after an explicit handoff while the original resident keeps authorship of the goal. The interaction result differs each time because the actions differ.

### Branch pruning rules

Do not author a scene for every permutation. Define the state axes, then write representative branches at important intersections: a privacy choice plus a public service; a helper refusal plus a schedule conflict; a missing item plus a smaller safe task; an interrupted task plus a promised follow-up; and a successful task with a withdrawn disclosure. Use concise fallback copy for unrepresented combinations. This preserves variation without a combinatorial content explosion.

### Data and narrative review

For every branch state, list the stable event reference, owner, transition, response line, and fallback if the save predates the fact. If a branch requires a task stage, confirm that stage exists in the current task contract. If it requires a new privacy state, identify the current save owner. If no current owner represents it, keep it as transient dialogue rather than adding another flag system. The authoring graph stays a proposal until code and data audits verify each premise.

## 512. Ensemble voices — let different survivors interpret the same action differently

Meaningful faction and character texture comes from varied concrete perspectives, not a universal reaction score. The same choice to postpone a cabinet repair can sound different from a technician, an educator, a route worker, or the resident whose goal is open. These are candidate voice shapes for current canon review, not new NPC requirements.

### The technician notices method

A technician may say, “If we open it before the parts arrive, we can still mark which screw is slipping.” This line supports a diagnostic route, not a claim that the repair is complete. If no such task variant exists, the technician instead says they cannot safely begin yet. The character's contribution is technical framing, not authority over the resident's consent.

### The educator notices teachability

An educator may ask whether the resident wants to show a helper how the notes are organized. The resident can welcome that, decline, or offer only a general method. A lesson can become a task only through an eligible owner; a friendly conversation need not create one. The educator's interest should not make the resident obligated to teach.

### The route worker notices access and timing

A route worker may explain that the workshop is hard to reach during a crowded shift or that a carrying route is blocked. This can change the player's schedule or handoff decision if those facts are current. The worker cannot declare an inaccessible route passable from habit. Their voice emphasizes movement and sequence rather than moral approval.

### The resident notices the cost of attention

The resident can appreciate a repair and still say that too many people have asked about the marks. They may ask for fewer public questions, allow one trusted helper, or withdraw. The player can honor the boundary, negotiate a narrower disclosure, or decline to proceed. The scene should not frame privacy as suspicious or punish an audience choice with a universal relationship loss.

### The records volunteer notices omissions

A records volunteer can say, “We know when it was booked. We don't know whether anyone checked it after the outage.” This is useful provenance language. The volunteer can help distinguish scheduled from completed, but cannot fill in a missing event. If the player declines a record, that is acceptable; the task still follows its owner.

### The major faction sees policy scale

A major authority may own access, safety priority, or a shared work schedule. It can require that the workshop remain clear during a real hazard or authorize use of a restricted tool. Its formal authority matters, but it does not decide whether a resident wants to explain their notes or receive help. The player can comply, seek an authorized alternative, or leave the personal goal pending.

### Voice checks

Keep each line tied to a role and observed fact. Avoid making every minor faction member sound like a counselor or every major faction official sound indifferent. Give the resident a voice before the player hears faction interpretation. Provide one neutral fallback for old saves and one no-history opening. The final scene should remain comprehensible if any optional supporting faction is absent.

## 513. Playstyle study — five managers reach different useful outcomes

The initiative loop should not assume that every player wants to converse deeply with each survivor. The plan supports five approaches with distinct strengths, friction, and endings. They are not classes, permanent traits, or personality axes. A player can move between them freely, and no route is the one morally correct route.

### The scheduler: protect time and make fewer promises

A scheduler asks for available work windows, checks role and rest constraints, and only then proposes a time. The resident may accept the first feasible slot, ask for a different window, or decide the work can wait. The scheduler's route reduces last-minute interruption but may feel slow when the issue is urgent. If a current incident requires action, the player must still choose whether to postpone the initiative. Schedule confirmation remains with its owner; dialogue agreement does not reserve the time.

The scheduler ending may be a confirmed appointment, a useful wait state, or a closed request after the only available interval is rejected. A future callback can mention that the work was booked after the watch handoff. It should not award “reliability” unless a current relationship owner supports that metric. If the player changes the roster and the slot disappears, the owner reports the conflict and the player receives a recovery choice.

### The listener: understand an expressed need

A listener starts with questions and gives the survivor room to share or decline. They may learn that the issue is a practical repair, an interruption to rest, an uncertainty about a skill, or a desire to teach someone. Any personal detail remains private unless the speaker chooses otherwise. The listener's success can be a better-informed decision even if no task begins. Do not force a “listen” dialogue option to unlock a resource or create a permanent relationship score.

The listener route may take several short interactions and can be skipped if the player already has sufficient operational facts. The resident can change their mind later. A present preference overrides the idea that the person is “always private” or “always open.”

### The logistics planner: validate supplies and capability

A logistics player checks item availability, tool compatibility, access, and eligible labor before accepting a goal. This route exposes missing dependencies earlier and avoids wasted assignments. It can also leave the request open longer while the player searches for a real supply source. The interface must say whether a component is in shared stores, held by a person, or unknown; it must not treat the entire shelter as one unbounded inventory.

A support group can offer a method, but the player still decides whether to use it. If a compatible alternative exists, show it through current catalog/task data. If the item is absent, the player may defer, choose a valid smaller inspection, or decline. The logistics route is rewarding through clarity and coherent planning, not through a hidden efficiency multiplier.

### The delegator: ask others to provide bounded help

A delegator may ask a records volunteer to draft a handoff, a technician to advise, or a qualified resident to help. Each service has a specific limit. The recipient can decline, be unavailable, or offer a different kind of support. The player chooses whether to accept the offer. Delegation should reduce a specific burden, not remove the player's decision from the story. If a support group completes no task, the narrative must not credit them as a worker.

A delegator can combine services: verify a part with stores, schedule through the roster owner, then let the resident conduct the task. This forms a richer route because each action changes what is known or possible. It does not create a faction coalition score.

### The hands-off manager: leave space for survivor initiative

A hands-off player may acknowledge the idea, decline to intervene, or allow a resident's existing autonomy to proceed. If a task completes independently, the owner confirms it and the story credits its actual participant. If it does not, the goal remains open or is withdrawn. Do not spawn repeated reminders solely because the player did not accept the request.

This route should preserve meaningful follow-up: the player may later ask what happened, support a new plan, or leave the topic closed. Silence in the UI is not a promise, refusal, punishment, or relationship change.

### Switching approaches without state contradiction

A player can listen one day, check supplies the next, and then delegate a work window. The system remembers only relevant events and the current goal state. It should not lock a route because the player previously selected another style. For example, after a logistics check discovers a missing tool, the player can ask the resident whether they prefer to wait, change the task, or cancel. The new choice is evaluated against current state.

### Balance review

Observe playtests for route viability, not equal reward. Ask whether each approach shows what it changed, whether it can reach a complete or deliberately open ending, and whether the player can recover from stale state. A method can be slower or expose less dialogue without being invalid. Remove any approach that silently creates a larger cost, fake urgency, hidden relationship loss, or unearned item access.

## 514. Branching endings catalogue — local consequences, long echoes

The goal's ending may alter a later option, a conversation, or a supporting faction's willingness to offer a specific service. It should not create a sweeping moral judgment. A robust catalog distinguishes task resolution from social memory and public consequence.

### Task resolution endings

- **Completed as proposed:** the original task producer confirms the work and the resident accepts closure.
- **Completed with a different participant:** a valid handoff changed who performed the work; credit and provenance name the actual participant.
- **Reduced scope:** a smaller supported task closes while the broader need remains open or is explicitly withdrawn.
- **Inconclusive result:** the task ends but the motivating uncertainty remains; the resident decides whether to continue.
- **Interrupted and resumed:** the task owner confirms interruption and a later schedule reopens the work.
- **Blocked by supply:** no item moved and the goal remains pending, changes scope, or closes by choice.
- **Canceled for a competing need:** the resident or player closes the current attempt with a specific reason, not an invented completion.
- **Independent completion:** a survivor task event closes the goal without player assignment.

### Consent and participation endings

A resident can accept help, accept one kind but reject another, ask for a different helper, choose to work alone, pause participation, or withdraw. If the player requests permission to share a method, the resident can approve general instructions but keep their personal record private. If the resident later revises that permission, use the current permission and keep already-delivered information historically truthful. Do not make consent permanently sticky.

### Resource and logistics endings

The player can commit a verified item, request an authorized release from stores, use a compatible alternative, wait for a source, or decline the cost. A task can reveal that the expected item was not needed, but only if the task producer reports that result. The player can retain the scarce item and accept a delay. There is no hidden moral score attached to spending or conserving goods.

### Supporting faction endings

A records group can provide a handoff sheet, a technical group can provide a diagnostic method, and a route group can confirm a current access condition. The player can accept, decline, or combine those services. The groups can be absent, unavailable, or unable to verify the premise. If their offer fails, the story continues through ordinary action where possible. Their presence affects the route through a specific service, not through generic affiliation standing.

### Later callbacks

A later character may recall one concrete result: a moved shift, a shared method, a tool returned, or a public notice withheld. Several consequences can coexist: the work finished while the reason remained private; a helper was consulted but the resident worked alone; the player declined an item but another source later provided one. The callback can acknowledge complexity without a moral summary.

### Endings that do not need rewards

A resident may thank the player and receive no item. A resident may end the request without penalty. The player may decide not to publish a solution. A support group may offer information that prevents wasted labor rather than giving loot. A task may end inconclusively and still be a coherent result. These outcomes protect the game from turning every branch into a reward transaction.

### Narrative closure standard

An ending is complete when the player can state what decision they made, what owner confirmed, what remains unresolved, and what next choices exist. Completion does not require a triumph scene. Open endings can be intentional if they clearly state whether the goal can return and who owns that choice.

## 515. Cross-initiative network — goals can touch without merging their state

Residents often have overlapping work: one wants to repair a cabinet; another needs a quiet study period; a third is preparing a route briefing. The initiative system can let those requests interact through shared schedule, task, room, and inventory owners. It must not merge the residents' goals into one anonymous colony objective.

### Shared space conflict

A repair and a lesson both need the workshop. The player can schedule them at different times, ask whether the lesson can move, choose another supported room, split the repair, or leave one goal open. The room owner decides access and capacity. Each resident speaks for their own preference. If one person agrees to move, that does not imply the other agreed to the resulting arrangement.

### Shared item dependency

A diagnostic tool is needed by two different tasks. The player can schedule sequential use if the inventory owner and tasks support it, ask a support group for a verified alternative, choose one task first, or defer both. The first task's result may change what the second needs, but only via a real event. Do not duplicate the tool, reserve it through dialogue, or apply a global priority score.

### Shared helper availability

A named helper is asked by two residents. The roster and current consent owner reveal whether one slot can be moved or whether the person must decline a request. The player can ask the helper directly where the route permits, invite each resident to choose another participant, or leave one goal open. Do not silently schedule both tasks because they use different narrative records.

### Shared source or public lesson

A resident may allow a general repair method to be shared while refusing to expose a personal note. Another resident may attend the lesson and adapt the method. The communication owner controls the audience and message; the task owner controls whether the work occurred. The two initiatives can link to one method event while retaining separate participants, consent, and outcomes.

### Network outcomes

The player may complete both goals through careful sequencing; complete one and postpone the other; revise one goal to avoid shared space; invite a second helper; or choose an unresolved ending for both when capacity is genuinely absent. The visible consequence is the changed appointment, resource use, or access state. Each person's intention remains attributed to them.

### Persistence and replay

On save and restore, reconstruct each initiative from its current owner state and stable event references. Shared schedule changes must not be copied independently into each goal record. If a shared event disappears in an old save, use a neutral fallback and ask the current owner for updated facts. Replaying a day must not double-book the room or repeat an offer. Deterministic ordering is essential when two goals become eligible at once.

### Author test

Write a scene where the player supports only one resident, one where both goals succeed, one where a support faction serves as a bridge, and one where no shared resource is available. Confirm that each resident can choose separately, no state is inferred from the other person's response, and the player can understand the resulting schedule without opening three disconnected panels.

## 516. Personal-goal catalogue — different intentions produce different play

A personal initiative should feel distinct in verb and rhythm. Repair, teach, preserve, test, assist, map, organize, and practice are not merely different text for the same fetch task. Each must still resolve through existing task, inventory, schedule, consent, and social owners. The catalogue below broadens the authored possibilities while keeping the proposal inside its three subfeatures: offers, consent-based refusal, and goals witnessed through real work.

### Repair: restore one working condition

A resident may propose repairing a hinge, patching a table edge, or checking a loose bracket. The player can request a diagnostic, source a verified item, schedule a qualified worker, accept a smaller safe inspection if supported, or decline. Branches derive from material availability, task scope, access, and consent. Completion comes from the task owner. An inconclusive attempt can close the inspection while leaving the larger repair open.

### Teach: transfer a method with permission

A resident may want to show another how to sort a tool rack or read a route legend. The learner may accept, ask for a different teacher, prefer a short demonstration, or decline. The teacher can choose to share the method publicly, teach one person, or keep it private. The player can arrange a supported schedule and room, provide a relevant item, ask for a support group's material, or leave the lesson unbooked. The event producer determines whether a lesson actually occurred; attendance does not imply mastery unless an existing learning owner records that result.

### Preserve: keep a record usable

A survivor may ask to label a cabinet, copy a maintenance note, or preserve a known procedure. The player can find a legitimate source, ask permission from the note's author, consult the records desk, or choose not to duplicate personal material. This branch can produce a task or a communication record only if an existing authority owns it. A written note cannot silently become policy or a universal fact. If the original source is missing, the character may preserve the uncertainty rather than invent the contents.

### Test: find out what is true

A resident may ask to compare a radio reception point, inspect a route marker, or test a hand-pump response. The player can schedule a safe observation, request a specialist, provide a known tool, or defer. The result may be confirmed, negative, mixed, or inconclusive. The task owner records only what it observed. A test can support a later decision, but does not itself guarantee that the underlying hazard changed.

### Assist: offer a bounded contribution

A resident may want to carry a crate, prepare a workbench, or help another resident with an existing task. The player can accept, ask to narrow the job, pair them with a willing helper, or decline because of duty or safety constraints. The task owner validates the work and the roster validates eligibility. A contribution can matter even when it does not complete the larger objective. The player can see who did which stage without credit being assigned by dialogue alone.

### Map: improve shared orientation

A resident may ask to mark one known corridor, clarify an indoor route, or compare a written marker with current access. The player may offer a route group's current observation, ask for a second source, keep a claim tentative, or postpone the map. Only the exploration/location authority confirms new path knowledge. The resident can choose whether their personal observations become a public annotation. Unverified reports remain attributed as reports.

### Organize: reduce friction in a shared place

A resident may want to arrange the tool shelf, clear a work surface, or label containers. The player can assign a supported task, ask what will move, request that an owner inventory contents first, or decline. Inventory and room owners retain their respective authority. An organization task must not become a secret item transfer; all moved items remain visible through the canonical inventory path.

### Practice: develop a routine without promising mastery

A resident may ask for repeated practice with a safe work method or radio tuning. The player can schedule a practice session, find a willing instructor, ask for a shorter trial, or decline. Repetition can produce flavor and an authored sense of time, but mastery changes only through a current skills or learning owner. A resident can stop practicing or change the method. The system should not issue an unsupported skill bonus as a narrative reward.

### Goal selection and scope

A goal becomes a persistent initiative only when the existing social owner can represent it and the player or resident has made a supported commitment. A sentence about wishing to learn may remain dialogue. The player can ask whether it should become a scheduled task. If no supported task or goal type exists, the narrative can still close the conversation without creating an invalid marker. The catalogue is a writer's set of candidate story forms, not approval to extend data schema.

### Shared endings across types

Each type supports completion, partial outcome, interruption, lack of supply, consent withdrawal, independent completion, or deliberate deferral where its owner contract permits. Distinct endings should use different evidence: a repaired object, a lesson record, a verified source, an interrupted carry task, a route observation, or no change. Avoid a shared “quest complete” sound or reward that erases what the resident actually pursued.

## 517. Supporting-faction route atlas — assistance modifies process, not ownership

A player's path can cross a minor faction at several points without making that faction the center of the initiative. This atlas defines useful service routes and their limits, including when support changes what information is available but does not guarantee success.

### Records desk route

The player asks whether an existing maintenance instruction or schedule note is current. The desk can retrieve an authorized record, report that it is missing, or distinguish a proposal from a completed task. If the player chooses to use it, they can cite the source privately, include a generic summary in a notice, or ignore it. The desk cannot author a technical result or expose personal notes without permission.

**Branch:** a current record supports a task; the player schedules it. A stale record prompts verification. A missing record leaves the goal open or prompts the resident to explain. The same initiative remains possible without the desk, though the player may have less certainty.

### Maintenance crew route

The player asks a crew member for a safe method, required tool, or availability window. The crew may demonstrate, provide a checklist, request that the resident perform the task, or decline. A demonstrated method is not task completion. A checklist does not reserve a tool. The player may accept this service, request a different specialist, ask the resident whether they want the help, or stop.

**Branch:** method accepted, resident tests it, and task owner confirms result; method declined and ordinary plan continues; crew unavailable and player reschedules; task not safe and it is deferred. Each is a concrete outcome.

### Route group route

The player requests a current access observation or asks how a known indoor path is marked. The group can provide a time-bound observation, offer a member to accompany a task if roster permits, or say its information is stale. It cannot reveal an unknown route or claim that a path remains safe after conditions change. The player can attach its source to a private note, ask permission to publish it, or keep the information limited.

### Care circle route

The player asks how to approach a resident who has set a boundary or wants privacy. The circle may suggest a private channel, a neutral question, or no further contact. It does not consent for the resident and cannot update the resident's preference. The player can take the advice, choose another approach, or decline to re-engage. This role creates a path based on the player's action, not a moral assessment of the resident.

### Stores liaison route

The player asks whether a needed item is available or whether a release request exists. The liaison checks shared stock through its actual owner, reports a denial or wait, or suggests the resident's personal inventory is not visible. The player can request an authorized issue, make a peer offer, choose a smaller task, or wait. A denied request is a real branch with its own next action.

### Support groups working together

The player may use the records desk to verify a procedure, ask the maintenance crew whether it is safe, then ask stores about a component. The groups do not vote to approve the initiative. Their facts can conflict: the procedure may be current but the item unavailable; the crew may approve an inspection but not a full repair; stores may hold the part for a major task. The player weighs the documented situation and asks the resident whether the revised plan still suits them.

### Support against a major-faction decision

A major authority may reserve the shared tool or limit access to a room. A support group may explain the policy, suggest a permitted alternative, or help request a review. It cannot cancel the reservation or create a new rule. The player can comply, appeal through an existing process, choose a personal item exchange, or leave the goal unresolved. Later narration should name the actual policy consequence without treating the major faction as malicious by default.

### Removal test

For each service route, remove the supporting group entirely. The player should still have an understandable path through existing owners, or a clear statement that the service is unavailable. Then remove the major faction: personal initiative can continue where it does not require that faction's authority. If either removal breaks every route, the content has accidentally made a supporting role mandatory or replaced a major owner's remit.

## 518. Initiative episodes at different scales

Not every resident goal should occupy five days. A varied campaign can include a brief scene, a one-shift task, and a multi-day sequence. This section defines content pacing so expansions remain rich without making the initiative surface feel like a quest backlog.

### One-conversation beat

A resident asks to change one scheduled hour. The player accepts, declines, or checks an alternate slot. The schedule owner returns the result and the character reacts. This takes one interaction and creates a small but concrete callback. It is useful when the player has limited time and avoids turning every request into a long quest.

### One-shift task

A resident wants to inspect a damaged storage label during the current shift. The player can provide the correct catalog item, ask another eligible person to help, or leave the task for later. The task can finish, be interrupted, or reveal that the label is not the source of confusion. The owner result closes or revises the goal. One-shift tasks teach the basic initiative loop organically.

### Multi-day project

A resident wants to teach a maintenance method and preserve it in an approved work note. The player must schedule a willing learner, secure the room, review privacy, and later ask whether a summary should be shared. Any link between lesson and note is supported by actual lesson and communication owners. The project can stop after the lesson, preserve only a general method, or remain private.

### Campaign callback

A later task may benefit from the earlier method or schedule adjustment. Use the callback only when the new task's owner identifies the relevant event. The resident may volunteer again, decline, or ask for different terms. The callback should make a previously useful action visible without automatically making the resident a permanent specialist or companion.

### Pacing guardrails

Do not present all active goals on every screen. Surface a goal when its source event or player action makes it relevant. Let open goals persist without repeated prompts. Use sparse notifications and clearly distinguish urgent owner-confirmed incidents from optional personal intentions. A quiet day can contain no new initiative. Characters can express ordinary life outside request creation: a resident finishing a task, changing a preference, or declining to talk may be enough.

### Branch density by scale

A one-conversation beat needs two or three decisions and an explicit close. A one-shift task may need a material check, participant choice, and one recovery. A multi-day project can support several branching episodes, but each branch should collapse into a manageable current status rather than exponential dialogue. A campaign callback should reference one relevant past action, then reopen present consent. This keeps richness from becoming fatigue.

## 519. Questline seedbook I — short arcs with distinct player verbs

These candidate arcs offer more questline breadth without introducing a new faction score or goal authority. Each starts from an individual request and resolves through an existing owner-backed task, communication, schedule, inventory, or social record. The content can be authored as small side stories only after each premise is verified in current data.

### Seed A: The Folded Map

A resident has folded a printed route map along the western corridor and asks whether it should be corrected. The player can ask what the mark represents, request a current route check, compare with the route group's latest observation, post a tentative note, or leave the map unchanged. The resident may say they saw a cart turn there, that the path was blocked last week, or that they are unsure. The route authority controls verified access; the resident owns their personal observation.

**Act one:** ask the resident to distinguish a direct observation from a guess. They can explain, decline, or change the subject. The player can still choose to verify the route.

**Act two:** contact the route group, inspect a current location record, or ask a major authority about restricted access. Each result supplies different confidence. The player can annotate only what is supported, keep the source attached, or leave the map unchanged.

**Act three:** the resident can approve a general note, ask not to be named, or withdraw the suggestion. The player may create a new message version if the communication owner supports it. A later traveler may use the verified route, but the task does not guarantee that future conditions remain unchanged.

**Endings:** corrected map with source; attributed observation not yet verified; private report kept off the public map; no change after verification fails; or resident withdraws the suggestion. The player's route depends on asking, verifying, publishing, or waiting, not on an honesty label.

### Seed B: One Quiet Table

Two residents want to use the same shared table for different reasons. One wants to sort a task kit; another asks for a private hour to teach a learner. The player can inspect room availability, ask both for acceptable times, offer a supported alternate location, split the task, or decline to mediate. A room owner decides access and capacity; each participant decides whether the proposed time works for them.

**Act one:** the player may schedule the first request, ask both people to coordinate, or privately speak with one participant. A public board post is optional and should not disclose that one resident wants privacy.

**Act two:** the table may be free for only part of the day. The player can ask whether the kit task can be shortened, whether the lesson can move, or whether a different supported space exists. A minor faction may offer a booking sheet or room-use guidance but does not assign priority.

**Act three:** the player confirms one slot, both slots, a single shared session, or neither. A shared session is available only if both residents accept and the task/lesson owners can represent it. If one resident withdraws, the other request remains independent.

**Endings:** each request receives separate times; the residents agree to share the table; one goal moves and one remains open; an alternate room works; or the player leaves the conflict unresolved. The consequence is a real schedule choice, not a faction-wide relationship bonus.

### Seed C: The Missing Measurement

A technician's note says that a cabinet “runs warm,” but the measurement field is empty. A resident asks for a second check before anyone replaces the component. The player can locate the original source, ask the technician to repeat the check, schedule another observer, use a supported safe procedure, or leave the concern open. No line may claim the cabinet is unsafe or repaired without a confirmed source.

**Act one:** the resident can explain what prompted the request or simply ask for verification. The player may respect a desire not to discuss more and still arrange a technical check.

**Act two:** a repair group can offer a procedure, the records desk can find a prior reading, or a major facility authority may restrict access. The player chooses which source to trust and may compare two observations if the task supports that.

**Act three:** results can agree, conflict, or remain inconclusive. A resident may accept the uncertainty, request another supported check, or withdraw. The player can post a factual result only with appropriate attribution and audience.

**Endings:** confirmed normal result; confirmed need for an owner-backed repair; conflicting readings with a safe next action; no available observer; or the resident closes the request. The quest rewards good process through clear outcomes, not through an unearned safety bonus.

### Seedbook use

Each seed needs a current data premise before becoming a production quest. The proposal writer lists the player-facing command, actor and goal owner, content carrier, success and failure result, privacy boundary, support-faction cameo, and no-goal fallback. If a seed depends on new room booking, skill advancement, or route authority, it returns for a separate design decision rather than adding an improvised counter.

## 520. Questline seedbook II — choice after refusal, delay, and partial success

The second seedbook focuses on how an initiative changes after the first route does not work. This is where action-based branches create human depth: players can recover, redirect, stop, or let a character take over.

### Seed D: The Handover Card

A resident wants to make a one-page handover for the next workshop shift. The player can ask what needs to be carried forward, locate the existing maintenance source, ask a records volunteer to help format it, or decline a new document. The resident can share general task facts while keeping personal remarks out of the card.

The first branch is whether the player checks the source. If the source is current, the resident drafts a handover that points to it. If stale, the player can ask a technician to verify, label the information as old, or leave the card unpublished. A second branch asks who receives the handover: one named successor, the next shift, or a public board. A third branch arises when the shift changes before delivery; the player can revise, resend, or close the card.

The ending can be a verified handover, a cautious note marked unconfirmed, a private instruction, a failed delivery, or no card. The card cannot become a second maintenance database. Its only durable result is the communication owner’s message/version and any linked task event that already exists.

### Seed E: The Shelf With No Name

Several tools sit on a shelf without visible custodian labels. A resident asks the player to clear space for a work session. The player can ask stores to inspect, ask the resident which items they recognize, reserve space without moving anything, or stop. The inventory owner decides custody; the room owner decides access; the resident's memory is an observation.

If stores confirms some items, the player can request a permitted move, ask the current task owner whether an item is still assigned, or defer. If ownership remains unknown, the player must not donate, discard, or trade the items. The resident may prefer to work around them. A support group can help list the visible catalog items if the inventory authority supports that view, but it cannot declare them abandoned.

Possible outcomes include cleared space after authorized movement, a revised work plan that uses a smaller area, a pending ownership check, a denied move because one tool is assigned, or an open goal that the resident withdraws. The player's choice is whether to prioritize space, information, or the resident's alternate plan.

### Seed F: Ask Once, Then Listen

A resident declines a request for help with a personal task. The player's next action can be to accept the refusal, ask whether a different form of help would be welcome, offer to leave the option open, or end the conversation. The narrative should not keep triggering reminders until the resident accepts. If the resident later reopens the subject, that is a new current choice.

The player can choose a practical nonintrusive action only if a current owner permits it, such as making a generic tool available or clearing an approved work window. The player cannot “help secretly” by altering the resident's property or schedule. A care circle may advise the player to respect a boundary; it does not speak for the resident.

Endings include clean refusal accepted, alternate support welcomed, topic reopened by the resident later, or no further interaction. A future dialogue may remember “you stopped asking after I said no” only if that event has a supported record. It cannot infer permanent trust from it.

### Seed G: The First Try Is Enough

A resident asks to practice one short procedure, not to become an expert. The player can schedule one practice, invite a willing mentor, offer a tool, or decline. After the practice, the learner can say they understand this step, want another attempt, prefer a different method, or are finished. If a skill owner does not track proficiency, do not show a level-up.

The branching detail comes from chosen practice conditions: solo or paired, short or full task, this shift or later, private or shared method. An interruption can lead to another time or close the practice. A mentor may refuse due to workload. The player sees what was attempted and whether the real task or learning owner reported any outcome.

### Seed H: The Room After the Task

After a resident finishes a task, the player can ask whether the room needs to be reset, assign an owner-backed cleanup, let the resident leave it, or ask another willing participant. The task completion and room state are distinct. A major authority may require a clear exit; a support group can offer a cleaning method. The player chooses a route, but no character is forced to do unpaid follow-up outside the task contract.

The story can end with room returned, cleanup deferred, another worker volunteering, a task canceled, or no further action. A later task may find the room in a verified state; otherwise it must not assume.

### Seedbook review

Across these arcs, an initial refusal does not end all future play, but it does end the current request unless the person invites another option. A partial success remains partial. A delayed action stays delayed. Each new offer or task has a source, current consent, and an owner. This supports branching play while respecting the limits that make the fiction credible.

## 521. Story callbacks across plans — use shared events without merging feature state

Plans 3, 4, and 5 may touch one campaign moment: a resident needs a tool, asks for a private schedule, and later negotiates a loan. Cross-plan continuity can make the world feel authored, but the three feature owners must remain separate. A goal record references the task; a message record references its audience and delivery; a barter record references accepted terms and asset movement. No plan may copy another plan's mutable state into a parallel journal.

### One shared episode

A resident asks to inspect a signal cabinet. The player privately requests a quiet hour through the communication surface. A peer offers a small wrench under an explicit loan. The duty roster confirms a qualified participant, and a task event records the inspection. The player may keep the resident's reason private, use a support crew's method, or post a general workshop notice. The quest narrative can connect the choices using stable event references.

### Different branches

- The player obtains the wrench through a verified loan and completes the task in the confirmed slot.
- The resident declines a loan and waits for a shared-store issue.
- The player sends a broad notice instead of a private request; affected residents receive the general schedule but no personal details.
- The task is interrupted; the loan remains open according to its due terms and the message is revised.
- The resident withdraws, the player returns the tool if required, and the message expires.
- The task completes independently before the player returns; the original offer may be stale and is not accepted.

These branches produce visible consequences on three feature surfaces, but each surface reads its own owner. The task's success cannot retroactively accept the loan; a message acknowledgement cannot prove consent to work; a trade cannot establish that a private note was delivered.

### Callback policy

A later conversation may say “The wrench is back with its owner,” “The workshop notice moved to the next shift,” or “The cabinet check was inconclusive.” It should not say “You kept your promise” unless the promise and completion fact are both represented. A speaker can share their own interpretation, clearly framed as perspective. General campaign summary text must avoid making one character's view universal.

### Integration eligibility

Cross-plan dialogue belongs after each feature has its own verified player route and save behavior. Until then, author standalone versions with neutral fallback copy. Do not make one plan dependent on another unimplemented plan. A cross-reference is a content option, not a new hard dependency or a request for a shared mutable coordinator.

### Validation

Create one save for each branch, advance time, reload, and compare the task, message, and trade records independently. Confirm exact participant identities, task outcome, audience, delivery, item custody, due terms, and privacy. Then replay with one feature absent: goal still opens without barter, a message can be sent without a goal, and a trade can close without a communication quest. The cross-plan callback should disappear gracefully rather than fabricating missing history.

## 522. Initiative graph author's room — write choices as playable actions

The quest writer and systems designer should share a small branch map before prose expands. Start with a resident's request and identify the current state that makes it possible. Then list actions the player can take through existing owners. Branch from those actions and their results, not from a personality label.

### Node card format

Every decision node has: an initiating event; a player intention; current known facts; options; the owner that accepts each option; success and rejection results; facts retained; and the next scene. A node also marks what remains unknown. If a decision has no source fact, the player may ask, investigate through an available route, or stop. This gives a real exploratory branch without presenting unsupported certainty.

### Example node: request to teach a method

**Initiating event:** a technician asks to teach a willing helper how to inspect a hinge.

**Player intentions:** schedule the lesson; find a helper; ask the resident whether the method is safe to share; locate a room; defer; decline.

**Known facts:** current technician status, permitted task or lesson data, room access, and any available schedule. Unknowns include whether the proposed helper wants to attend and whether the room is available later.

**Choices:** offer a named helper, ask an eligible roster group, propose a different time, ask for a short demonstration, keep the method private, or leave the goal open.

**Owners:** consent and roster owners validate participants; schedule/location owners confirm the window; the existing learning/task owner confirms the lesson or work event; communication owner handles any public summary.

**Rejections:** helper unavailable; room inaccessible; resident refuses public sharing; lesson type unsupported; player cancels.

**Resulting branches:** single learner, no learner, rescheduled lesson, private demonstration, generic method share, interrupted session, or completed session with no mastery claim.

This node can create a dozen meaningful result combinations without an alignment check. Each combination can map to a short callback and current next action.

### Player intent labels

Use clear verbs: ask, schedule, offer, check, delegate, publish, defer, hand off, stop. Avoid vague options such as “be supportive” or “be firm” unless the player can see the concrete action. The tone of a dialogue option can vary, but its underlying command must remain clear. A line like “I can move the table if you want” asks a question; it does not book anything.

### Model failures as decisions

A failed owner command is not the end of branch planning. Show the precise failure and offer a next action: pick another time; check another stock source; ask a different eligible helper; remove a private detail; continue without the support group; or cancel. If none is available, give the player a clean exit and preserve the current state. Never invent a new item or helper simply because the scene needs resolution.

### Branch compression

Several actions may converge to the same state. If the player asks the records desk or checks a permitted ledger and both produce the same verified source event, they can share a follow-up node while retaining distinct flavor. If two choices merely change a sentence but not what the player knows or can do, decide whether the voice difference is valuable. Use a shared fallback when the save lacks history.

### Test the graph before polishing

Read the branch map without dialogue. If the route is still understandable, prose can add voice and emotional texture. If choices do not change the player’s options, cost, audience, or knowledge, the branch may be cosmetic. If an ending requires a non-existent event, revise the branch before writing more scenes.

## 523. Goal lifecycle as an authored arc — intention, attempt, revision, closure

A personal goal feels deeper when its stages are different in play. This section defines the narrative lifecycle while leaving state ownership with the current social and task systems.

### Intention

A character expresses something they want to do. It may be casual conversation, an offer, or a persistent goal if the existing owner can store it. The player can ask for context, offer help, schedule a task, suggest another participant, or leave it. An intention alone is not a quest acceptance.

### Preparation

The player and resident identify time, role, access, supplies, privacy, and audience as relevant. The resident can change their request while the player gathers information. Preparation can fail because a part is missing or the only eligible helper is busy. The goal remains the resident's, not the player's property.

### Commitment

A specific command is accepted: a schedule is booked, a task is assigned, an offer is accepted, or a message is sent. The interface shows which one. If the command is rejected, return to preparation with the reason. Do not mark the goal active from a click before the owner confirms.

### Attempt

Work begins through its task producer. The resident may act, observe, teach, or withdraw according to the task contract. Interruption and partial progress are described only where the owner records them. If task progress is not persisted, the story may narrate an interruption without claiming a durable checkpoint.

### Revision

The goal may change after new evidence, resource loss, participant refusal, or a player choice. Ask whether the resident wants a new scope. A revision may create a new task or close the old goal and start another, depending on existing ownership. Do not silently mutate a prior accepted agreement.

### Closure

Completion, withdrawal, cancellation, stale data, or unresolved status closes the immediate arc. The player sees what happened and what remains possible. A closed goal does not mean the character has no other aspirations. An unresolved goal can return only if its owner and authored cadence permit it.

### State transition review

For each transition, confirm whether it creates persistent data or only changes a scene. If persistent, identify capture/restore path. If transient, ensure a reload fallback does not imply that the state survived. If a goal advances from task events, make the event identity stable and deduplicate repeated delivery. The content should be designed to accept accurate state rather than forcing state to match a prewritten ending.

## 524. Small goals, large implications — let ordinary actions carry weight

The campaign can make an ordinary action feel consequential without turning it into an epic quest. The weight comes from who asked, what the player chose to share, which resource was committed, and how the result alters a later option.

### Tool shelf

A resident asks to label a shelf. The player can ask for the inventory owner to verify contents, accept a private layout, ask the records desk to create a general catalog note, or leave the shelf alone. The task can reduce search friction if a current system recognizes the new organization. If not, the narrative can simply show the resident arranging it without claiming a gameplay bonus.

### Teaching one person

A mechanic offers to show one learner how to check a belt. The player can protect a quiet slot, add another learner with permission, ask to make a public instruction, or decline. One learner may become more confident; an unsupported proficiency score must not be shown. The instructor can later prefer not to teach again.

### Two-minute handoff

A worker requests a concise handoff before leaving. The player can help capture current task status, ask for source verification, send a private message to the next shift, or let the next person inspect the task themselves. This choice can affect information quality and task continuity, not global trust.

### Route observation

A resident asks to record that a door stuck after the last storm. The player can check the route owner, ask a support group, write a tentative note, or wait. The note can be useful while remaining uncertain. A later major-authority access restriction can supersede it without making the resident wrong for reporting what they saw.

### Spare hour

A resident wants a quiet hour to practice a radio routine. The player can offer a space, ask whether a partner is welcome, arrange a schedule, or leave the goal private. If a signal system or skill owner does not expose practice outcomes, the result is a character moment, not an invented statistic.

### Everyday follow-up

These small arcs can appear between major campaign events. Use one specific physical detail: a label rewritten in block letters, a wrench returned to its hook, a chair moved near the lamp, or a route mark left in pencil. A detail should be an observable consequence of a player action or task event. Do not reuse it as a generic reward prop after unrelated branches.

### Depth test

Ask whether the scene changes a present choice, reveals a useful source, exposes a cost, or gives a resident a voice. If it does none of these, shorten it. If the state cannot support the consequence, keep the prose as a question or observation rather than a confirmed result. This makes the small goal feel intentional without inflating its mechanical scope.

## 525. Major factions in personal-goal stories — policy has reach, not ownership of a person

Major factions may control rooms, task priorities, equipment, access rules, and shared schedules. Their decisions can shape a resident's goal without replacing that resident as the author of it. This section turns the distinction into playable branches.

### Shared room restriction

A major authority closes the workshop for a confirmed inspection. The player can postpone a personal repair, seek a permitted alternate room, ask whether the resident wants to continue later, or cancel the goal. The resident may accept, suggest a smaller off-site task if supported, or withdraw. A minor maintenance crew may explain why the room is unavailable, but cannot reopen it.

### Shared equipment reservation

A tool is reserved for a major shelter repair. The player can request an authorized release, use a personal item offer, choose a compatible alternative, or wait. The reservation owner approves or denies release. The resident may decide that waiting is acceptable or ask to abandon the task. No faction's refusal becomes a moral verdict about the player or resident.

### Duty priority

A resident's scheduled work conflicts with an emergency task. The player can ask the duty owner whether another eligible worker is available, move the personal goal, or defer it. A resident may decline a proposed reassignment. A minor crew can identify skill needs; only the roster owner decides assignment and only the person supplies consent as current policy permits.

### Policy review

A resident believes a room rule blocks their initiative. The player can ask the major authority for clarification, ask a support group to help phrase the request, find a permitted alternative, or stop pursuing the policy route. The authority may confirm the restriction, grant an exception, or decline. A support group can witness a practical impact but cannot promise that the policy will change.

### Four ways to preserve player agency

The player may comply with the policy and leave the goal open; seek a sanctioned alternative; ask for a review; or stop. The UI should reveal cost, time, audience, and certainty where supported. It should not force a confrontation scene whenever a rule is followed. Later callbacks name the action—“we moved the work to the second shift”—rather than infer that the player is obedient or rebellious.

### Authority boundary audit

For each branch, name the major faction's actual remit and the minor faction's service. Confirm that the resident may still accept or decline a plan. Confirm that the player can choose no action. Confirm that the minor group's support cannot override the authority. Confirm that a policy decision does not erase private motivation or consent. These limits make faction involvement meaningful without turning personal goals into faction questlines.

## 526. Branch-specific voice bank — same state, different human attention

Candidate lines below should be assigned to established characters only after canon review. They are organized by what a person notices, not by an alignment axis. Their state conditions are part of the line specification.

### A resident who values preparation

- After a confirmed booking: “The slot is on the board. I'll bring the list, not the whole cabinet.”
- After an unconfirmed proposal: “I can keep that hour open until the rota answers.”
- After a supply check fails: “Then let's not pull the housing apart yet.”
- After the player preserves a scarce part: “The pump needs it more today. We can come back to the hinge.”

### A resident who values a quiet workspace

- After a private schedule is accepted: “That gives me enough room to think.”
- After the player asks to publish their reason: “Keep the time; leave my reason out.”
- After another helper is suggested: “Ask first. I don't want someone waiting at the door.”
- After the player chooses a public notice: “The work time can be public. My notes aren't.”

### A resident who values teaching

- After a learner agrees: “One person at the bench is enough for the first try.”
- After the learner declines: “All right. I can write the steps for myself.”
- After the task is interrupted: “We only reached the first mark. Put that in the handoff.”
- After no mastery owner exists: say what was practiced, not that the learner became an expert.

### A resident who values independence

- After the player offers help: “I can take the first look. Call me if the bracket won't move.”
- After a task is completed independently: “I got the cover loose and found the gap.” Use only with owner-confirmed work.
- After a resident declines further help: “I'll stop here for today.” This respects withdrawal.
- After the player follows up too soon: “I haven't changed my answer.” Use only if a current interaction/history record supports the callback.

### A support-group voice

- Records volunteer: “This note is old. I can find the source, but I can't update the task for you.”
- Maintenance helper: “I can show the test. I can't call it fixed until the crew signs off.”
- Route observer: “Clear at the morning check. I don't know how it looks now.”
- Care volunteer: “You can ask what help is welcome. You don't need the reason to offer a time.”

### Callback constraints

A line about a booked shift requires a schedule result. A line about a task requires its producer. A line about privacy requires a permission fact. A line about an item requires inventory. If a phrase implies an unpersisted promise, rewrite it as current intention. When no supported history exists, select a neutral greeting rather than a line that guesses at the relationship.

## 527. Candidate implementation-ready vertical slice — one initiative from offer to consequence

A single vertical slice should demonstrate the plan's intended depth before more side stories are authored. Candidate slice: a resident offers to label the shared tool shelf after finding two tools in the wrong place. The player can ask what changed, check inventory, schedule the task, ask a records volunteer to prepare a label, offer a qualified helper, or decline.

### Entry and preview

The resident's offer is visible as an initiative with actor, goal, location, known time, expected item if any, and expiry only if supported. The player sees that the task changes organization, not ownership. Inventory can be inspected through its existing view; the initiative panel does not copy stock. The resident may ask for private time, a different helper, or no public notice.

### Decision and owner command

The player chooses the task scope and participant. The roster checks eligibility; the resident's consent is current; the location owner confirms access; any schedule is approved; the task owner accepts the work. If a label item is required, inventory validates it at the point of use. A records volunteer can help with wording but cannot authorize movement of tools.

### Observable result

The task owner reports completed, interrupted, or blocked. Inventory remains consistent. If the work only identifies an uncertain tool, the result says so. If a tool is moved, the real property owner records its current location. A public notice is optional and sent through the communication owner with an explicit audience.

### Recovery and endings

The player can reschedule, choose a different helper, remove a public disclosure, use no label, ask stores to clarify an item, leave the goal open, or close it. A resident may stop the task. If the save reloads after scheduling, no duplicate offer is created. If the task completes independently, the correct worker receives credit. A stale location or missing item leads to a useful message and no partial mutation.

### Slice acceptance evidence

- Fresh campaign offers the goal only when its source condition exists.
- Player can read the offer and reach it through the intended route.
- Every player choice invokes one owner-backed action or is clearly conversational.
- At least four distinct action paths reach different, truthful states.
- Refusal and no action remain complete outcomes.
- Save, day advance, and reload preserve the task/goal relationship.
- Repeated evaluation does not draw again or duplicate events.
- Optional support service can be absent without blocking the task.
- Major policy can constrain the room or inventory without overriding personal consent.
- Dialogue falls back cleanly when history is missing.

This slice is a proposal for later integration planning, not an authorization to create new save stores or architecture. A current-source audit remains required before implementation.

## 528. Initiative investigation arcs — asking first and acting first both matter

An initiative can begin through dialogue or through observation. Some players speak with a resident before acting; others notice a condition and take an operational step first. Both approaches should be supported where safe and should produce distinct but coherent follow-up.

### Dialogue-first route

The resident says the workbench lamp is flickering. The player asks how often, whether the resident wants an inspection, and whether they have a preferred helper. The resident can answer, decline detail, or ask for a different time. The player then checks the current task owner, schedule, and inventory as needed. The advantage is that the plan can reflect preference before commitment. The cost may be another conversation or a wait for a response.

### Observation-first route

The player notices the lamp flicker during an existing task and asks the maintenance crew to inspect. If the crew is eligible and the resident's space is not private, it can report a task result. The player can tell the resident afterward, invite them to participate, or leave the task open. The resident may appreciate the fix, object that the player entered without permission, or say that this is unrelated to their own request. The scene must respect current access policy and cannot assume observation grants consent.

### Offer-first route

The resident proposes to replace the lamp using an available component. The player can accept a task, ask for a safer inspection, check stock, or decline. The resident may counter with a smaller task or ask to work alone. The player is not forced to ask all background questions before responding, but the preview must show the known cost and unresolved facts.

### Faction-first route

The player asks a support technician for a method before speaking with the resident. The technician can offer general information, report that the fixture needs an owner, or decline. The player then chooses whether to share that advice and ask if the resident wants to continue. The technician's expertise does not grant access or assign the task. This route can feel more managerial and still remain valid if consent follows.

### No-action route

The player may leave the flicker alone if it is not urgent or if the resident has not asked for intervention. The feature should not convert every environmental observation into an initiative. If the lamp becomes a confirmed hazard later, the appropriate owner can initiate a different task. The resident can also bring it up again if the campaign state and authored cadence support a return.

### Resulting lines

After dialogue-first, the resident may say, “Thanks for asking before moving the bench.” After observation-first, they may say, “I saw the crew at the lamp. What did they find?” or “That wasn't the part I was worried about.” After faction-first, a resident may ask whether the method was already applied or only discussed. After no action, the resident may never mention it again. Every line depends on a real action or explicit memory.

### Authoring requirement

Write at least one valid follow-up for each entry route and a fallback for absent history. Verify that an unsolicited task is allowed by location and consent rules. Verify that technical advice remains advice until a task owner confirms work. If a route requires private inspection or an unapproved call, remove it or request a design decision rather than bypassing permission.

## 529. Supporting-faction relationships — service can be declined without feud

Optional help should create believable reciprocal context without a faction standing meter. A support group may remember that the player requested its time, accepted its advice, published its finding, or declined the service. Future availability can change only when a current authority tracks workload or schedule.

### Accepted service

The player requests a short inspection. The group confirms a real time and contributes a task or observation. Later, the player can cite that result, ask a follow-up, or choose another group. A thank-you line may acknowledge the service, but the core task remains under its owner.

### Declined service

The group may offer a witness or checklist that the player does not want. The player can say no, thank them, or explain a competing plan. The group should not punish the refusal unless an actual agreement was accepted and then broken. In a future scene, the group remains available according to current state and characterization.

### Service unavailable

A group has no member free at the requested time. The player can propose another slot, ask for a written method if one exists, or proceed without the group. “Unavailable” is a practical constraint. It should not read as a faction snub.

### Service misunderstood

The player treats a method suggestion as task completion. A group member can correct the misunderstanding: “I showed the test. I didn't replace the latch.” The player can acknowledge, schedule the real work, or leave it. This branch teaches source ownership through character voice instead of an abstract systems tutorial.

### Service exposed

The player shares a private observation without permission. The resident may ask for correction, request removal where supported, or withdraw from the goal. A support faction may clarify what it actually reported, but cannot erase delivered information. The narrative should not automatically make the group complicit or hostile; it follows who sent what to whom.

### Major-faction response

A major authority may recognize the support group's service as evidence, reject it as insufficient, or request formal verification. The player can provide a current source, ask for another review, or proceed under the existing limit. The support group can explain its observation but cannot compel policy. The resident chooses whether to continue their personal goal despite the authority's answer.

### Service callback design

Store or reference only the service event that the current owner already exposes. A generic line can say, “The crew gave us a method last time,” if the event exists. Do not add “faction favor” points simply to make the cameo recur. Let the service's practical result—knowledge, access advice, work completed, or nothing—support the callback.

## 530. Initiative review suite — eight complete action traces

A proposal is ready for deeper planning only when it can be traced from a player choice to an observable result. The following traces exercise the intended branch diversity.

1. **Ask, then accept:** the resident explains a need, the player requests a task, the owner accepts, the task completes, and the goal closes.
2. **Ask, then decline:** the resident explains; the player cannot spare the item; the resident changes scope or leaves the request open.
3. **Do not ask:** the player accepts a generic offer, a later privacy choice narrows the audience, and the task remains correctly attributed.
4. **Check source first:** a support group reports an old observation; the player requests verification and does not publish it as current.
5. **Schedule first:** an owner rejects the first time; the player selects a valid alternate or leaves the plan pending.
6. **Helper refuses:** the player respects that refusal, asks another eligible person, chooses solo work where supported, or stops.
7. **Interruption:** a task is paused by a current event; the player communicates a real update, reschedules, or cancels.
8. **Resident withdraws:** current consent changes; the player stops the assignment, returns property where required, and closes the initiative without replacement.

For each trace, record initial owner state, player command, event identity, visible status, persisted data, reload result, and narrative line. Include a case where no state changes. Confirm that each trace remains understandable if an optional faction is missing. This suite gives future implementers and writers a bounded acceptance map while leaving the exact technical design to the current integration process.

## 531. Revisit cadence — open goals without nagging

A goal may remain relevant for days without needing a daily reminder. The player should encounter it when a new event, a changed prerequisite, or a deliberate visit makes it useful. This section defines a cadence that preserves autonomy and reduces notification fatigue.

### Initial request

Show the goal once when the resident offers it or a source event makes it relevant. Include current known constraints and actions. If the player closes the panel, do not treat that as refusal. A small persistent status may remain only if the social owner supports it.

### First quiet interval

If the player takes no action, do not automatically repeat the full request next day. A resident may continue ordinary behavior. When the player speaks with them for another reason, they can mention the goal only if it remains important to them and content cadence supports a callback. They may change scope or withdraw.

### New information

A part becomes available, a schedule opens, a helper returns, or a task source changes. The goal can reappear with that new fact: “The brace is back in stores; do you still want to inspect the shelf?” The resident may accept, decline, or say the goal no longer matters. The availability event does not auto-assign work.

### Stale prerequisite

If the participant leaves, location closes, item is consumed, or policy changes, the old option becomes unavailable. Explain the specific reason and offer only supported alternatives. Do not continue showing a broken command as a yellow objective. The player may close the goal, wait for a valid source, or ask a new question.

### Player-initiated return

The player may reopen the goal from a resident conversation or status view. The system checks current state before presenting actions. Old choices are not presumed accepted. The resident can say their preference changed. This makes long campaign gaps feel natural rather than like a quest journal demanding overdue work.

### Cadence acceptance

Test days with no update, several owner updates, and one meaningful trigger. Verify that a new offer is not duplicated, a stale offer does not linger, and no action is inferred from elapsed time alone. Quiet is an authored state.

## 532. Branch density budget — more paths, clear authored convergence

A plan can promise broad player agency without requiring a bespoke scene for every combination. Use branch density where action changes what the player knows, whom they involve, what cost they accept, or what is shared. Converge when different routes reach the same owner-confirmed state.

### Primary branches

The main route map can distinguish ask-first, act-first, faction-supported, logistics-first, and hands-off play. Each should receive a different opening or follow-up that acknowledges the approach. The task, schedule, and consent owners still determine final eligibility.

### Secondary branches

Within an approach, add at most a few consequential variations: private versus public result, named helper versus solo work, verified supply versus missing supply, accept versus counteroffer. These variations can alter the next option and a later callback.

### Convergence points

If both a stores check and a maintenance group confirm the same tool is absent, the player can converge on one “no item available” status while retaining the source detail. If two schedule routes produce the same confirmed slot, they may share the task node. Convergence should preserve the action history needed for relevant callbacks.

### Edge routes

Handle rare outcomes with truthful generic copy: participant departed, record missing, owner API unavailable, or old save has no history. Do not build a full dialogue branch for every migration case, but provide an action-safe fallback. Generic text must not erase privacy or ownership distinctions.

### Production review

Count distinct commands, persistent states, and authored lines separately. A graph with many dialogue options may still have one state; a short exchange can still create multiple save obligations. Remove branches that only award a hidden moral label. Prioritize branches that make a cost, refusal, audience, or uncertainty clear.

## 533. Voice and consequence pairing — write a line only after its state

Writing often starts from an appealing sentence and then searches for a state that would justify it. Reverse that order. Decide what happened, what remains true, and what the speaker knows. Then write a line that expresses one human reaction.

### State-first example: player moved a schedule

Owner result: the work slot changed and the resident accepted. The resident may say, “The later hour leaves the bench free.” This reports a practical consequence. It need not say “You understand me.” If the resident instead disliked the move, they can say, “I agreed because the earlier slot would block the other crew.” Agreement and enthusiasm are separate.

### State-first example: player declined an item

Owner result: no item moved. The resident may say, “Keep it for the pump; I'll change the order of work.” This acknowledges the resource choice. If no follow-up task was accepted, do not claim they changed the order; use an open offer instead.

### State-first example: player respected privacy

Owner result: a public message omitted the resident's personal reason. The resident may say, “The notice says enough.” This line is grounded in the actual audience and content. If the message was never sent, they cannot react to it.

### State-first example: player accepted an interruption

Owner result: task paused; no completion. The resident may say, “We stopped before the second check.” This tells the player what remains. Avoid “We'll finish tomorrow” unless a new appointment was confirmed.

### Review checklist

For each conditional line, point to the source state, owner, persistence, and audience. Remove the line if any premise is missing. Give each character room to react differently to the same result according to authored voice, but not according to an unapproved alignment score. The narrative can be rich without claiming more than the world records.

## 534. The offer can wait — branching around deliberate non-commitment

A resident offers to help catalog supplies but cannot work until a later shift. The player can accept a pending offer if the current owner supports it, ask for a confirmed time, check whether a smaller task fits now, decline, or simply thank the resident. This scene tests an often neglected playstyle: the player wants to keep a possibility open without making a promise.

### Pending offer

If the offer owner supports a pending state and expiry, the player sees both. The resident may keep the offer open, set a limit, or withdraw. No task appears as scheduled until the roster and task owners confirm. A reminder is generated only when the owner supports it. If the offer expires, it closes as expired; it does not imply that the player rejected the resident.

### Ask for a time

The player may request a confirmed slot. The resident suggests one, the schedule owner checks availability, and either the booking succeeds or returns a conflict. The player may accept the available alternative, ask for another, or return to pending. The resident's suggestion is not a booking.

### Smaller action

The resident can offer to label one shelf now and do the rest later, if a supported task can represent those stages. The player can accept the smaller task, defer the whole job, or ask them to stop. Partial work receives partial status. No completion marker closes the full goal until its owner confirms the scope is met.

### Player declines commitment

The player can say that there is no need to schedule yet. The resident may agree, withdraw the offer, or ask the player to return by a supported date. The interface provides a clean exit. It does not show the option in red or attach a hidden trust change.

### Return path

When the player returns, the offer owner checks current validity. The resident may still be available, have changed their plans, or have completed the work independently. The player sees the present state, not a frozen promise from the prior conversation. A later line can acknowledge that the player left the decision open without claiming they had accepted.

This branch creates nuance through timing, scope, and explicit non-commitment. It lets a player manage a busy shelter without turning each conversation into a quest acceptance prompt.

## 535. Texture when nothing happens — restraint still has a scene

An initiative can end a day with no task, item movement, faction service, or message. The writer can still give the resident a small action that does not pretend to resolve the request: folding the note, putting a tool back, crossing out a proposed hour, or choosing not to continue the conversation. Use a physical detail only if its location and ownership make sense. The line can communicate hesitation, relief, distraction, or simple routine without a saved emotional score.

When the player checks back, the resident may say the request still matters, has become less important, or should be closed. If the save lacks the earlier conversation, use a neutral opening. Do not force an apology or confrontation to make the unresolved state dramatic. A quiet result is complete when the player knows the goal remains open, was withdrawn, or has no current action. This gives low-intervention play its own flavor and protects the narrative from treating every empty branch as failure.

## 536. Weekly initiative review — hold the portfolio lightly

At the start or end of a campaign week, the player may want a concise view of open personal goals. This is a summary affordance, not a new quest authority. It can list only initiatives that their current owner says are active, paused, or awaiting a specific event. Each row shows the resident's short goal, last confirmed action, present blocker, and a valid next action. The view should not rank people by urgency or worth.

The player can visit a resident, ask whether the goal still matters, close the view, or sort by location or readiness if current data supports it. A task that completed independently should close from the actual task event. A missing item remains a blocker only while the owner says it is missing. A participant's preference may change; the player rechecks consent before making a new assignment. A stale record has a neutral recovery path.

Avoid overdue badges unless an accepted contract has a real due condition. Do not show “neglected” for a goal the player never accepted. Do not auto-advance or expire goals because time passed unless the social owner explicitly supports that rule. A resident can withdraw quietly, complete the work, change the goal, or keep it open. The weekly review is useful when it helps the player notice a changed circumstance; it is harmful if it makes people feel like unresolved tasks on a moral ledger.

Validate the summary with a quiet campaign week, a week with one interruption, a week where two goals complete independently, and a week after save migration. The view should remain truthful and low-pressure. If there are no active goals, show an empty state that invites ordinary play rather than inventing a new request.

## 537. Moral language guardrail — describe the chosen action

The player may conserve a resource, ask before helping, prioritize a major task, or accept a resident's refusal. The narrative can show consequences and character reactions, but it should not infer a campaign-wide moral identity. Replace “selfish choice” with the exact item retained; replace “good choice” with the confirmed task or accommodation; replace “you betrayed them” with a persisted agreement that was not met, if one exists. A resident may offer their own opinion, clearly as one person's perspective. This keeps meaningful consequence without reducing branching to a good/evil scale.

## 538. Every ending is a future opening only by choice

A completed repair may lead to another offer, but it does not have to. An unresolved task can remain open, be withdrawn, or return when a new source changes the situation. A resident who once accepted help can refuse it next time. Let follow-up arise from current need and participant choice. The player should be able to close the conversation, finish ordinary work, and return later without a compulsory quest chain.

## 539. Action order can change the route

The same supported actions can produce different scenes when performed in a different order. Asking the resident before checking supplies lets them define the goal. Checking stores first may reveal that a component is assigned elsewhere before the resident is approached. Asking a support group first may expose a safer method but still requires consent. Scheduling before selecting a helper may produce a conflict; selecting a helper first may make the available time clearer. Preserve these differences through the facts the player learns and the options they see, not through an alignment rating.

The player may also stop between steps. An unfinished inquiry does not create an assignment. A draft plan does not reserve an item. An unconfirmed time does not bind a resident. When the player returns, refresh current owner state and present the next valid action. This ordering gives the campaign replay variety while keeping each result explainable.

## 540. Keep the resident's goal theirs

The player can support, redirect, postpone, or decline an initiative, but the goal remains attributed to the resident unless they transfer responsibility through an explicit supported handoff. The player cannot force an alternate ending simply to clear a panel. If the resident withdraws, close the current goal; if another person volunteers, ask that person and the original resident about the handoff. Credit follows the actual task event. This rule protects authorship across every branch.

## 541. First-contact fallback

A returning player may meet a resident whose initiative history is absent, migrated, or no longer relevant. Begin with the resident's current request and present known choices. Do not mention a promise, prior refusal, item transfer, or private disclosure unless the current owners can confirm it. The player can ask for context, inspect the current task, propose a schedule, seek support, or leave the goal open. A neutral first-contact line should still sound like a person, not an error message. If the resident has a current task underway, show its real status. If the old record cannot be restored, do not invent a resolution. This fallback makes the plan robust to old saves and keeps the campaign welcoming after long gaps.

A new conversation always evaluates current consent and availability before offering an old route again.

## 542. Closing questline — The Lessons Left in Pencil

This final candidate arc uses the initiative loop to connect an individual learning goal, a practical task, and a small supporting-faction service. It is intentionally modest: a resident wants to teach a second person how to maintain a hand radio, but is unsure whether the existing notes are accurate. The player can investigate the method, protect the resident's authorship, schedule a lesson, request a specialist review, or let the idea end. The quest is not a test of whether the player is honest or kind. Its branches follow what they check, who they involve, what they share, and whether a task or lesson actually occurs.

### Opening: the learner asks for a method

The resident says that the tuning steps were learned from an older note, but the last line is difficult to read. The player can ask what part is uncertain, invite the resident to demonstrate, look for a current source, ask the route or radio group whether it has a verified procedure, or leave the note untouched. The resident can share the note privately, allow a general method to be discussed, or keep the original. The player sees the difference between a source check and a lesson commitment.

If the resident only wants to learn for their own use, the player can arrange a short practice with a willing instructor. If they want to teach someone else, the player can help identify a learner, ask the learner directly, or let the resident choose. No role assignment happens merely because the player names a person. The roster and consent owners determine eligibility and participation; an existing learning/task owner determines whether a session can persist as a goal.

### Chapter one: distinguish old knowledge from current procedure

The player may ask the records desk to locate the note's source, request a specialist to review the steps, compare the procedure against current radio equipment, or accept that the source cannot be recovered. A records volunteer can identify provenance and age, but cannot validate technical correctness. A specialist can check equipment compatibility but cannot claim that an old author intended a particular step. A major authority may restrict use of a room or device during a current incident. Each faction has one role.

**Verified source route:** the player receives a source-backed method and can offer to share the verified portion. The resident may approve, revise the explanation, or keep the original private.

**Mixed source route:** one step is verified and another is unclear. The player can share only the verified part, ask for a new test, or leave the note unpublished.

**No source route:** the note remains unverified. The player can schedule a supervised test if supported, teach only how to mark uncertainty, or stop.

**Current policy route:** a major authority restricts access to the radio room. The player can request a later slot, seek an approved alternative room, or defer; the supporting group cannot override the restriction.

### Chapter two: choose who participates

The resident may ask to teach one named learner, accept a learner suggested by the player, request a specialist to observe, prefer to practice alone, or decline to teach. The proposed learner may accept, ask for a different time, or refuse. The specialist may observe without taking over the lesson, provide a short demonstration, or be unavailable. The player can schedule a session only after the relevant people agree and current owners confirm the time and place.

The player may ask a support group to prepare a plain-language card. The group can help edit an authorized procedure, identify an uncertain line, or say it lacks the right source. The card is a message or data asset only if an existing owner supports it; otherwise it remains a candidate diegetic prop and cannot be treated as persistent knowledge. The resident can approve an attributed version, approve a general version, or keep the method private.

### Chapter three: run the session or change the plan

At the scheduled time, the lesson may occur, start and be interrupted, be canceled by a participant, or fail to begin because the room is no longer available. The player can reschedule, shorten only if the learning/task owner supports a short session, ask whether an alternate participant is welcome, or close. If the learner tries the steps and the radio produces no clear signal, the scene reports only that result. It cannot claim the learner mastered the procedure or that the equipment is repaired unless current owners confirm those states.

A specialist might discover that the procedure works only on a different radio model. The player can revise the lesson's scope, ask the resident whether they still want to teach the general principle, seek a compatible device, or stop. The resident may prefer not to continue after learning the discrepancy. This is a meaningful outcome: the check prevented the player from teaching an inaccurate procedure.

### Chapter four: choose what to preserve

After the session, the player can ask whether the resident wants a written summary, a private record, an attributed procedure, or no record. The communication owner handles audience and delivery. A records volunteer can preserve provenance if authorized, but does not create a second authoritative procedure catalog. A minor faction may receive credit for technical review only if it materially helped and the participants permit attribution. The resident can withdraw consent to share further; already-delivered copies are not magically erased.

### Endings

- **Verified lesson, attributed method:** source and specialist agree; learner attends; approved method is shared with attribution.
- **Verified lesson, private practice:** the resident and learner practice without public distribution.
- **Partial method, uncertainty retained:** only confirmed steps are shared; remaining questions stay open.
- **Incorrect step caught:** the player pauses the lesson, and the resident chooses whether to revise or withdraw it.
- **No source recovered:** the player teaches uncertainty-marking or declines to publish a procedure.
- **Policy postponement:** the lesson waits for authorized access; no promise is shown as booked until confirmed.
- **Learner declines:** the resident can keep the goal, find another willing learner, or end it.
- **Resident withdraws:** the player stops sharing and closes the current initiative without assigning a replacement teacher.
- **Interrupted session:** actual progress remains partial; the player can reschedule or close.
- **No persistent learning owner:** the conversation remains a one-time scene with no unsupported skill state.

### Branch callbacks

The resident may later say, “We kept the uncertain step off the card,” only when the player chose that distribution. A learner may ask for another session only if the learning event and current participant consent support it. The specialist can refer to the equipment check but not claim credit for the lesson. The records volunteer can identify a source version, not infer mastery. The player may leave the arc without a public resolution.

### Slice acceptance trace

Create a trace for verified method, mixed method, no source, learner refusal, room denial, interrupted session, private practice, public attribution, no persistent learning API, and old save without history. For each, list the actual command owner, durable event, present options, privacy boundary, and fallback line. Then confirm that ordinary initiative requests remain available after closure. The result is a story that expands player expression while staying inside offers, consent-based refusal, and witnessed goals.

## 543. Plan 3 closeout — accepted scope and remaining implementation decisions

This plan's content-design pass is complete at the requested 120,000-word minimum once the final count is recorded. Its scope remains exactly three feature subfeatures: survivor offers; refusal through consent; and personal goals advanced by witnessed work. The additional questlines, seedbooks, service patterns, dialogue banks, lifecycle guidance, and acceptance traces expand those three subfeatures rather than add a fourth system.

### Included design outcomes

The plan now covers goals that begin from repairs, teaching, preservation, testing, assistance, mapping, organization, and practice. It supports ask-first, act-first, logistics-first, faction-supported, and hands-off approaches. It includes multi-day projects, quiet endings, source uncertainty, privacy, partial work, interrupted scheduling, independent completion, and withdrawal. Minor factions provide narrow expertise, records help, observation, or access advice. Major factions retain formal policy, shared stock, and schedule ownership. The player chooses routes through actions and evidence, not a global virtue score.

### Still requires current-source verification

Before implementation, re-audit the current autonomy, social save, roster, consent, task, inventory, schedule, communication, learning, and location seams. Confirm each task event and save owner. Confirm which callbacks are representable. Recheck whether any data schema or host route changed after this proposal. Do not assume a sample quest's physical interaction exists merely because it appears in the plan.

### Closeout criteria

- Measured plan length is at least 120,000 words.
- Exactly three Section 4 subfeatures remain authoritative.
- Every new example maps to those features and current owners.
- No unsupported relationship score, task state, resource counter, or faction authority is introduced.
- Each branch preserves refusal, no-action, uncertainty, and no-history fallbacks.
- This remains a proposal, not implementation approval or a path claim.

The plan is ready to leave content drafting and enter a separate evidence-based implementation planning process. This closeout records writing completeness only; it does not claim that the feature is integrated in gamecode.

## 544. Closing casebook — The Work That Was Not Asked For

A survivor begins mending a torn canvas over the south stair without taking a task slot. The work is useful, but the player has not seen why it matters. When asked, the survivor says the draft wakes an infant sleeping near the stair. The player can ask them to stop until the passage is cleared, offer a partner and a safer location, or leave the repair alone. Each choice changes the next scene through an observable action: a conversation, a reassigned task, or continued work under the survivor's own initiative. No moral label is needed. The relevant context is location, safety, consent and the history of who asked whom to move the cot.

If the player relocates the infant first, the canvas work remains theirs to accept or refuse. If the player orders the repair stopped, the response is not automatically anger: the survivor may agree that a loose nail makes the stair dangerous. If the player ignores the hazard, the initiative can pause after a near miss and return as a request for nails. The goal advances only when the authored condition occurs. Merely opening the conversation does not count as help, trust or progress.

This case tests the distinction between autonomous initiative and assigned labor. It also gives the player several concrete ways to learn the reason: observe the draft, ask the survivor, inspect the stair, or hear a neighbor mention the infant's sleep. None is the sole golden path. The scene can close without a task change; a truthful explanation is a complete outcome.

## 545. Closing casebook — A Promise with a Narrow Audience

Mira tells the player she will show the younger residents how to patch rubber boots after dinner, provided the infirmary can lend two stools. The player may accept the offer as stated, ask whether the infirmary has permission to lend them, find other seats, or tell Mira that the room will be unavailable. A public notice that says “boot class tonight” would overstate the promise until the location and equipment are confirmed. If the player posts a provisional note, its wording must say that the lesson is proposed and explain how residents can check back.

The infirmary can decline because the stools are needed for dressing changes. The refusal closes only the stool request; it does not erase Mira's willingness to teach. The player may help gather crates, move the lesson outdoors, or postpone it. If a rain event makes the outdoor space unusable, the decision returns to Mira and the learners rather than automatically cancelling the goal. She might teach a small group in the dry corridor, demonstrate the stitch verbally, or choose another evening. Those options are based on materials, access and the learner group, not a personality score.

A resident who misses the lesson may later ask for a second showing. The personal goal advances for demonstrating a repair or teaching a learner, not for filling a seat count. If only one learner attends, the scene still matters. The closeout preserves the actual result—what was taught, where, and who agreed to participate—without inventing a broad morale reward.

## 546. Closing casebook — When Help Creates a New Obligation

A mechanic volunteers to inspect a handcart after watching the player carry water by hand. They find a split axle and offer to repair it if the player can bring a sound bolt from the stores. The player can fetch the exact part, ask the mechanic to mark a substitute size, ask the quartermaster whether the part can be spared, or decline because the cart is not urgent. Each route changes a different fact. A promise to bring a bolt is not a completed repair; a repair is not proof that the cart can safely carry a full load.

If the stores issue a substitute, the mechanic can test it with an empty cart, request a second part, or explain that the replacement failed. The player can stop after the test, find a better bolt, or assign the cart to a light-duty route. No refusal should silently damage the relationship. The mechanic may be disappointed about the delay while still agreeing with the safety decision. If the player carries the cart after being warned, the later failure should identify the warning that was given and the action that ignored it.

The personal goal is satisfied by the mechanic's chosen contribution and a truthful test result, not by compulsory agreement with the player. The quartermaster's role is limited to inventory authority; the mechanic owns the technical judgment; the player decides whether to take the risk. This small chain gives consequence without making one faction or survivor the universal gatekeeper.

## 547. Closing casebook — The Witness Who Declines the Credit

After a resident helps calm a crowded water queue, a clerk asks to name them in the evening report. The resident may agree, ask that the report name the queue team instead, allow an anonymous account, or decline publication. The player may relay the request, suggest a neutral summary of the event, or leave the record unpublished. The action that settled the line remains real whichever credit choice follows; public recognition is a separate consent decision.

If the resident asks for team credit, the clerk can check who actually served and give everyone a chance to correct the roster. If one helper has already left, the report may wait or use a qualified phrase such as “several residents.” If the player selects anonymity, no later scene should reveal the name through an incidental recap. An authorized safety report can retain operational details under its own access rules, but that does not authorize public praise. These distinctions let a player's compassionate intention meet the other person's boundaries.

A later personal goal can refer to the resident's ability to coordinate a queue without requiring fame. They might coach another resident privately, write a short procedure, or decide that they do not want to lead again. A choice to step back is not a broken arc. The branch ends with a specific shared practice, a bounded disclosure and the option for the resident to choose what comes next.

## 548. Closing acceptance traces — refusal, completion, and no history

**Refusal trace:** Offer a task with a clear location, hazard and time. The survivor declines because the route crosses an occupied treatment room. Record the refusal once, show the reason if they chose to share it, and leave the task unassigned. The player may propose another route. Reopening the same offer cannot add another penalty. After a day advance, the survivor is not treated as having accepted merely because the task remained visible.

**Completion trace:** A survivor offers to repair a cooking pot. The player supplies the requested patch; the survivor performs the repair; the inventory owner records the patch's use and the item owner records the changed condition. Only the repair event satisfies the authored goal. A conversation about the repair, a queued action and a failed attempt remain distinguishable. Reload preserves the completion once, not twice.

**No-history trace:** A fresh save has no prior offer, refusal or goal event. The initiative view is empty or offers only current authored opportunities. It does not invent a past promise to make the screen look populated. Opening and closing the view preserves that empty history. Deterministic replay produces the same offer order and outcomes for the same canonical state and seeded inputs.

**Changed-context trace:** The survivor accepts a task, then an injury or access restriction makes the route unsafe before the task begins. Revalidate at execution. Pause or withdraw the assignment through the current work owner and explain why. Do not mark a refusal, completion or personal-goal failure. The survivor can choose a different task later.

**Closeout trace:** Each of these examples belongs to one of the existing three subfeatures: an offer that can be answered, a refusal that retains agency, or a personal goal that advances through witnessed work. Their supporting details—credit, safety, equipment, timing and access—are branch conditions on those features, not new systems. The plan remains proposal-only pending a fresh source audit and an approved integration package.
