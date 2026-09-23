# Expansion 101 — A Trade Held in Both Hands

**Wave:** 20 — Useful Systems, Real Consequences  
**Requested series:** Plan 5 of 5  
**Requested plan length:** At least 120,000 words; expansion above the minimum is acceptable.  
**Series status:** This plan's content pass is closed at the requested minimum; proposal only.
**Measured length:** 120,002 words; 100.00% of the 120,000-word minimum (2 words above). Counted through Section 518.
**Unique key feature:** A reachable peer barter loop where promises, property movement, favors and disputes remain truthful under one transaction owner.  
**Status:** proposal only; no implementation or path claim.  
**Audit:** [Wave 20 forensic report](../../forensics/WAVE20_FEATURE_SEAMS_FORENSIC_REPORT.md)

> Exactly three subfeatures are defined in Section 4. The examples and casebook all map to those same three; no additional feature pillars are introduced.

## 1. Expansion thesis

The experience is a reachable peer barter loop where promises, property movement, favors and disputes remain truthful under one transaction owner. It must start from canonical state, invoke one owner-backed command and show an outcome that remains true after day advance and reload. A panel-only simulation does not meet the promise.

A neighbor exchange can matter more than a market price: one has dry gloves, another a borrowed tool. The feature adds negotiation without assuming all survivors own tradable stock.

Show real cost, permission and uncertainty before commitment. Refusal, delay and failure may be valid results.

## 2. Canon fit

A neighbor exchange can matter more than a market price: one has dry gloves, another a borrowed tool. The feature adds negotiation without assuming all survivors own tradable stock.

Keep the tone restrained, material and human. Sample lines below are candidates, not current canon; check them against the live narrative data before authoring.

## 3. Existing systems reused

Reuse offers, rules, pair reputation, favors and the real property owner only for assets it controls. Shared stock remains under its existing owner. Do not repurpose ShelterBarterPanel for peer property.

Reuse stable IDs and owner records. JSON under Assets/StreamingAssets/Data remains authoritative; do not duplicate mutable data in the host.

## 4. Key feature and exactly three subfeatures

**Single key feature:** A reachable peer barter loop where promises, property movement, favors and disputes remain truthful under one transaction owner.

**Current gap:** Peer barter has no proven host route and item settlement is not guaranteed. With delegates absent, a trade can complete without goods; a partial callback can move one side only. Atomic transfer is a prerequisite, not a solved feature.

### Subfeature 1: Offers state the whole exchange

**Purpose and loop:** Show parties, items, favor terms, property source and expiry. Validate in barter owner and recheck both sides at acceptance. Relationship is not consent.

**Player value:** An offer may be item, favor or mixed; obligations and expiry are visible. Rejection has no hidden penalty.

**Boundary:** CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

### Subfeature 2: Settlement is both sides or neither

**Purpose and loop:** Prepare all changes through the asset owner; commit all or none; only then mark trade, record completion and apply trust once.

**Player value:** A trade should fail because an item moved, not because a callback transferred one then failed on the other.

**Boundary:** Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

### Subfeature 3: Favors, disputes and closure

**Purpose and loop:** Give obligations typed due conditions and fulfill from real task/work events. Disputes identify the trade and reason; trust changes once from verified outcome.

**Player value:** A favor can be late but fulfilled. The player sees what fact changed trust.

**Boundary:** TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

These three subfeatures are the complete gameplay scope. A fourth independent pillar needs a separate proposal.

## 5. Core mechanics

**Input:** current owner state, catalog, day and explicit player command. **State:** existing domain DTO and only stable event references. **Decision:** commit, defer, decline or choose a supported alternative with costs visible. **Uncertainty:** only facts current systems support. **Consequence:** owner-confirmed state or cost. **Cross-system output:** one fact once. **Failure/recovery:** stale data, denied consent, capacity, interruption and retry have truthful results. **Replayability:** seeded campaign inputs and stable order, never wall-clock or UI-local random.

## 6. Cross-system interactions

Primary domain: SurvivorBarterSystem owns offers/favors/disputes/reputation. PersonalBelongingsSystem owns selected keepsakes and gifting. InventorySystem, ShelterBarterSystem, MarketSystem and Holdfast own shared/caravan stock. Roster owns identity.

A current cost/permission owner supplies constraints; an existing downstream owner consumes confirmed effects; the day/save owner refreshes the same state after restore. Verify each actual API and consumer before implementation.

## 7. Main narrative spine

A concrete need appears; the player sees known facts and limits; one of the three subfeatures permits a decision; a later view shows who acted and what changed. Human center: A neighbor exchange can matter more than a market price: one has dry gloves, another a borrowed tool. The feature adds negotiation without assuming all survivors own tradable stock.

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

Preserve stable identity, valid pending intent, confirmed result and provenance through the existing save owner. A durable consequence is A favor can be late but fulfilled. The player sees what fact changed trust.

Core capture/restore is necessary but not sufficient: prove host registration, restore order and dirty flush.

## 17. Failure and alternate outcomes

A blocked command is a valid branch. Distinguish missing source, deferral, owner rejection, expiry, conflict and completion where current APIs support them. Never show success before confirmation or turn denial into hidden punishment.

## 18. Replayability

Different people, timing and owner state should change the choices without arbitrary new rolls. Same seeded state resolves identically. Duplicate command/day delivery reuses a durable result or reports already processed.

## 19. Implementation classification

**Classification:** Cross-system host integration with hard transaction gate. If no owner can guarantee both transfer legs, keep item acceptance unavailable and return the architecture decision.

Start with a fresh premise and runtime audit. Resolve owner decisions, connect the existing Core authority through the current host/event/save seam, then expose one Godot route. Core remains engine-free and JSON remains authoritative.

## 20. Collision audit

Adjacent behavior: Reuse offers, rules, pair reputation, favors and the real property owner only for assets it controls. Shared stock remains under its existing owner. Do not repurpose ShelterBarterPanel for peer property.

Classify this as extension/reachability work, not replacement. Re-search current content, host paths and save sections before implementation. Never revive Unity behavior.

## 21. Expansion hooks

Later quests or campaign history can consume an owner-confirmed result with provenance. They cannot infer success from a UI label or duplicate the source state.

## 22. Strongest recommended content

Start with the smallest interaction that proves the cost and consequence: A trade should fail because an item moved, not because a callback transferred one then failed on the other. Add one blocked path and one delayed callback.

## 23. Contracts, data, save and determinism

**Owner boundary:** SurvivorBarterSystem owns offers/favors/disputes/reputation. PersonalBelongingsSystem owns selected keepsakes and gifting. InventorySystem, ShelterBarterSystem, MarketSystem and Holdfast own shared/caravan stock. Roster owns identity.

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

**Primary risk:** Peer barter has no proven host route and item settlement is not guaranteed. With delegates absent, a trade can complete without goods; a partial callback can move one side only. Atomic transfer is a prerequisite, not a solved feature.

Later verification should cover accepted, denied, stale, repeated and restored actions plus seeded replay where relevant. Prove behavior, not class presence. Rollback disables the host route and preserves canonical state and save readers. Keep unsafe commands unavailable while ownership is unresolved.

## 27. Three creative variants for this feature

**Grounded:** show current state and one safe owner command. **Systemic:** connect the three subfeatures to real costs, permissions and delayed outcomes. **Wildcard, still grounded:** let the player inspect the source, author, provenance or physical limit before committing. This changes framing, not architecture. Implement systemic only when contracts are proven.

## 28. Review gate

Proceed to implementation planning only when source confirms the gap, every mutable fact has one owner, exactly these three subfeatures have truthful routes, save/replay owners are named and success follows an owner result. Otherwise revise or stop.

## 29. Casebook: design acceptance records mapped to the three subfeatures

These are review examples, not implementation claims, test results or extra features. Every scenario maps to S1, S2 or S3 and preserves the current ownership boundary.

### Scenario W20-101-S1-001 — two residents offer owned belongings

**Initial condition:** two residents offer owned belongings. The view is a projection of the current owner. **Pressure:** ownership hook absent at offer creation. **Player action:** block settlement until transaction owner exists.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. each asset has one owner before/after. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That is my coat. The tin is theirs. Check both names first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-001 — two residents offer owned belongings

**Initial condition:** two residents offer owned belongings. The view is a projection of the current owner. **Pressure:** first leg succeeds and second fails. **Player action:** prepare and commit both legs atomically.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. failure on either leg leaves both unchanged. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The stock room is not either of ours. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-001 — two residents offer owned belongings

**Initial condition:** two residents offer owned belongings. The view is a projection of the current owner. **Pressure:** requested item no longer owned. **Player action:** keep offer pending with conflict.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. status cannot accept without confirmed transfer. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That is my coat. The tin is theirs. Check both names first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-002 — two residents offer owned belongings

**Initial condition:** two residents offer owned belongings. The view is a projection of the current owner. **Pressure:** requested item no longer owned. **Player action:** route shelter stock to shared owner.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. no shared stock changes in peer trade. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can mend the hinge by Friday. I cannot promise the wire today. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-002 — two residents offer owned belongings

**Initial condition:** two residents offer owned belongings. The view is a projection of the current owner. **Pressure:** shared ration appears in private offer. **Player action:** fulfill favor from task event.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. favor/trust/dispute survive restore. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One offer, two signatures. If one changes, stop. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-002 — two residents offer owned belongings

**Initial condition:** two residents offer owned belongings. The view is a projection of the current owner. **Pressure:** overdue favor tick repeats. **Player action:** record dispute without inventing transfer.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. duplicate accept cannot mint another trust gain. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can mend the hinge by Friday. I cannot promise the wire today. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-003 — two residents offer owned belongings

**Initial condition:** two residents offer owned belongings. The view is a projection of the current owner. **Pressure:** dispute lacks completed trade ID. **Player action:** keep offer pending with conflict.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. each asset has one owner before/after. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The stock room is not either of ours. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-003 — two residents offer owned belongings

**Initial condition:** two residents offer owned belongings. The view is a projection of the current owner. **Pressure:** stale panel dispatches communal stock. **Player action:** return existing result for duplicate accept.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. failure on either leg leaves both unchanged. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That is my coat. The tin is theirs. Check both names first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-003 — requested object is shared stock

**Initial condition:** requested object is shared stock. The view is a projection of the current owner. **Pressure:** transfer fails after first item. **Player action:** separate favor-only agreement.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. status cannot accept without confirmed transfer. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The stock room is not either of ours. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-004 — requested object is shared stock

**Initial condition:** requested object is shared stock. The view is a projection of the current owner. **Pressure:** transfer fails after first item. **Player action:** record dispute without inventing transfer.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. no shared stock changes in peer trade. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One offer, two signatures. If one changes, stop. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-004 — requested object is shared stock

**Initial condition:** requested object is shared stock. The view is a projection of the current owner. **Pressure:** requested item no longer owned. **Player action:** block settlement until transaction owner exists.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. favor/trust/dispute survive restore. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can mend the hinge by Friday. I cannot promise the wire today. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-004 — requested object is shared stock

**Initial condition:** requested object is shared stock. The view is a projection of the current owner. **Pressure:** shared ration appears in private offer. **Player action:** prepare and commit both legs atomically.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. duplicate accept cannot mint another trust gain. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One offer, two signatures. If one changes, stop. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-005 — requested object is shared stock

**Initial condition:** requested object is shared stock. The view is a projection of the current owner. **Pressure:** shared ration appears in private offer. **Player action:** separate favor-only agreement.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. each asset has one owner before/after. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That is my coat. The tin is theirs. Check both names first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-005 — requested object is shared stock

**Initial condition:** requested object is shared stock. The view is a projection of the current owner. **Pressure:** overdue favor tick repeats. **Player action:** route shelter stock to shared owner.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. failure on either leg leaves both unchanged. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The stock room is not either of ours. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-005 — requested object is shared stock

**Initial condition:** requested object is shared stock. The view is a projection of the current owner. **Pressure:** reload after transfer before status save. **Player action:** fulfill favor from task event.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. status cannot accept without confirmed transfer. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That is my coat. The tin is theirs. Check both names first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-006 — requested object is shared stock

**Initial condition:** requested object is shared stock. The view is a projection of the current owner. **Pressure:** stale panel dispatches communal stock. **Player action:** prepare and commit both legs atomically.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. no shared stock changes in peer trade. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can mend the hinge by Friday. I cannot promise the wire today. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-006 — favor names a task and due day

**Initial condition:** favor names a task and due day. The view is a projection of the current owner. **Pressure:** transfer fails after first item. **Player action:** keep offer pending with conflict.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. favor/trust/dispute survive restore. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One offer, two signatures. If one changes, stop. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-006 — favor names a task and due day

**Initial condition:** favor names a task and due day. The view is a projection of the current owner. **Pressure:** accept command arrives twice. **Player action:** return existing result for duplicate accept.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. duplicate accept cannot mint another trust gain. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can mend the hinge by Friday. I cannot promise the wire today. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-007 — favor names a task and due day

**Initial condition:** favor names a task and due day. The view is a projection of the current owner. **Pressure:** accept command arrives twice. **Player action:** fulfill favor from task event.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. each asset has one owner before/after. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The stock room is not either of ours. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-007 — favor names a task and due day

**Initial condition:** favor names a task and due day. The view is a projection of the current owner. **Pressure:** belonging ID confused with generic item. **Player action:** record dispute without inventing transfer.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. failure on either leg leaves both unchanged. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That is my coat. The tin is theirs. Check both names first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-007 — favor names a task and due day

**Initial condition:** favor names a task and due day. The view is a projection of the current owner. **Pressure:** overdue favor tick repeats. **Player action:** block settlement until transaction owner exists.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. status cannot accept without confirmed transfer. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The stock room is not either of ours. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-008 — favor names a task and due day

**Initial condition:** favor names a task and due day. The view is a projection of the current owner. **Pressure:** overdue favor tick repeats. **Player action:** return existing result for duplicate accept.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. no shared stock changes in peer trade. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One offer, two signatures. If one changes, stop. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-008 — favor names a task and due day

**Initial condition:** favor names a task and due day. The view is a projection of the current owner. **Pressure:** reload after transfer before status save. **Player action:** separate favor-only agreement.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. favor/trust/dispute survive restore. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can mend the hinge by Friday. I cannot promise the wire today. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-008 — both parties own items when offered

**Initial condition:** both parties own items when offered. The view is a projection of the current owner. **Pressure:** ownership hook absent at offer creation. **Player action:** route shelter stock to shared owner.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. duplicate accept cannot mint another trust gain. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One offer, two signatures. If one changes, stop. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-009 — both parties own items when offered

**Initial condition:** both parties own items when offered. The view is a projection of the current owner. **Pressure:** first leg succeeds and second fails. **Player action:** route shelter stock to shared owner.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. each asset has one owner before/after. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That is my coat. The tin is theirs. Check both names first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-009 — both parties own items when offered

**Initial condition:** both parties own items when offered. The view is a projection of the current owner. **Pressure:** requested item no longer owned. **Player action:** fulfill favor from task event.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. failure on either leg leaves both unchanged. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The stock room is not either of ours. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-009 — both parties own items when offered

**Initial condition:** both parties own items when offered. The view is a projection of the current owner. **Pressure:** shared ration appears in private offer. **Player action:** record dispute without inventing transfer.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. status cannot accept without confirmed transfer. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That is my coat. The tin is theirs. Check both names first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-010 — both parties own items when offered

**Initial condition:** both parties own items when offered. The view is a projection of the current owner. **Pressure:** recipient cannot receive another belonging. **Player action:** keep offer pending with conflict.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. no shared stock changes in peer trade. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can mend the hinge by Friday. I cannot promise the wire today. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-010 — both parties own items when offered

**Initial condition:** both parties own items when offered. The view is a projection of the current owner. **Pressure:** dispute lacks completed trade ID. **Player action:** return existing result for duplicate accept.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. favor/trust/dispute survive restore. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One offer, two signatures. If one changes, stop. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-010 — both parties own items when offered

**Initial condition:** both parties own items when offered. The view is a projection of the current owner. **Pressure:** stale panel dispatches communal stock. **Player action:** separate favor-only agreement.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. duplicate accept cannot mint another trust gain. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can mend the hinge by Friday. I cannot promise the wire today. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-011 — both parties own items when offered

**Initial condition:** both parties own items when offered. The view is a projection of the current owner. **Pressure:** stale panel dispatches communal stock. **Player action:** record dispute without inventing transfer.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. each asset has one owner before/after. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The stock room is not either of ours. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-011 — item moves before acceptance

**Initial condition:** item moves before acceptance. The view is a projection of the current owner. **Pressure:** first leg succeeds and second fails. **Player action:** block settlement until transaction owner exists.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. failure on either leg leaves both unchanged. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That is my coat. The tin is theirs. Check both names first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-011 — item moves before acceptance

**Initial condition:** item moves before acceptance. The view is a projection of the current owner. **Pressure:** requested item no longer owned. **Player action:** prepare and commit both legs atomically.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. status cannot accept without confirmed transfer. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The stock room is not either of ours. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-012 — item moves before acceptance

**Initial condition:** item moves before acceptance. The view is a projection of the current owner. **Pressure:** requested item no longer owned. **Player action:** separate favor-only agreement.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. no shared stock changes in peer trade. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One offer, two signatures. If one changes, stop. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-012 — item moves before acceptance

**Initial condition:** item moves before acceptance. The view is a projection of the current owner. **Pressure:** shared ration appears in private offer. **Player action:** route shelter stock to shared owner.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. favor/trust/dispute survive restore. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can mend the hinge by Friday. I cannot promise the wire today. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-012 — item moves before acceptance

**Initial condition:** item moves before acceptance. The view is a projection of the current owner. **Pressure:** overdue favor tick repeats. **Player action:** fulfill favor from task event.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. duplicate accept cannot mint another trust gain. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One offer, two signatures. If one changes, stop. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-013 — item moves before acceptance

**Initial condition:** item moves before acceptance. The view is a projection of the current owner. **Pressure:** dispute lacks completed trade ID. **Player action:** prepare and commit both legs atomically.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. each asset has one owner before/after. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That is my coat. The tin is theirs. Check both names first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-013 — item moves before acceptance

**Initial condition:** item moves before acceptance. The view is a projection of the current owner. **Pressure:** stale panel dispatches communal stock. **Player action:** keep offer pending with conflict.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. failure on either leg leaves both unchanged. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The stock room is not either of ours. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-013 — saved offer reopens before expiry

**Initial condition:** saved offer reopens before expiry. The view is a projection of the current owner. **Pressure:** transfer fails after first item. **Player action:** return existing result for duplicate accept.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. status cannot accept without confirmed transfer. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That is my coat. The tin is theirs. Check both names first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-014 — saved offer reopens before expiry

**Initial condition:** saved offer reopens before expiry. The view is a projection of the current owner. **Pressure:** transfer fails after first item. **Player action:** fulfill favor from task event.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. no shared stock changes in peer trade. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can mend the hinge by Friday. I cannot promise the wire today. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-014 — saved offer reopens before expiry

**Initial condition:** saved offer reopens before expiry. The view is a projection of the current owner. **Pressure:** accept command arrives twice. **Player action:** record dispute without inventing transfer.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. favor/trust/dispute survive restore. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One offer, two signatures. If one changes, stop. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-014 — saved offer reopens before expiry

**Initial condition:** saved offer reopens before expiry. The view is a projection of the current owner. **Pressure:** shared ration appears in private offer. **Player action:** block settlement until transaction owner exists.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. duplicate accept cannot mint another trust gain. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can mend the hinge by Friday. I cannot promise the wire today. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-015 — saved offer reopens before expiry

**Initial condition:** saved offer reopens before expiry. The view is a projection of the current owner. **Pressure:** shared ration appears in private offer. **Player action:** return existing result for duplicate accept.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. each asset has one owner before/after. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The stock room is not either of ours. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-015 — saved offer reopens before expiry

**Initial condition:** saved offer reopens before expiry. The view is a projection of the current owner. **Pressure:** overdue favor tick repeats. **Player action:** separate favor-only agreement.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. failure on either leg leaves both unchanged. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That is my coat. The tin is theirs. Check both names first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-015 — saved offer reopens before expiry

**Initial condition:** saved offer reopens before expiry. The view is a projection of the current owner. **Pressure:** reload after transfer before status save. **Player action:** route shelter stock to shared owner.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. status cannot accept without confirmed transfer. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The stock room is not either of ours. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-016 — saved offer reopens before expiry

**Initial condition:** saved offer reopens before expiry. The view is a projection of the current owner. **Pressure:** stale panel dispatches communal stock. **Player action:** block settlement until transaction owner exists.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. no shared stock changes in peer trade. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One offer, two signatures. If one changes, stop. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-016 — recipient reaches belonging capacity

**Initial condition:** recipient reaches belonging capacity. The view is a projection of the current owner. **Pressure:** transfer fails after first item. **Player action:** prepare and commit both legs atomically.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. favor/trust/dispute survive restore. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can mend the hinge by Friday. I cannot promise the wire today. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-016 — recipient reaches belonging capacity

**Initial condition:** recipient reaches belonging capacity. The view is a projection of the current owner. **Pressure:** accept command arrives twice. **Player action:** keep offer pending with conflict.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. duplicate accept cannot mint another trust gain. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One offer, two signatures. If one changes, stop. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-017 — recipient reaches belonging capacity

**Initial condition:** recipient reaches belonging capacity. The view is a projection of the current owner. **Pressure:** belonging ID confused with generic item. **Player action:** keep offer pending with conflict.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. each asset has one owner before/after. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That is my coat. The tin is theirs. Check both names first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-017 — recipient reaches belonging capacity

**Initial condition:** recipient reaches belonging capacity. The view is a projection of the current owner. **Pressure:** recipient cannot receive another belonging. **Player action:** return existing result for duplicate accept.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. failure on either leg leaves both unchanged. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The stock room is not either of ours. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-017 — recipient reaches belonging capacity

**Initial condition:** recipient reaches belonging capacity. The view is a projection of the current owner. **Pressure:** dispute lacks completed trade ID. **Player action:** separate favor-only agreement.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. status cannot accept without confirmed transfer. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That is my coat. The tin is theirs. Check both names first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-018 — recipient reaches belonging capacity

**Initial condition:** recipient reaches belonging capacity. The view is a projection of the current owner. **Pressure:** dispute lacks completed trade ID. **Player action:** record dispute without inventing transfer.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. no shared stock changes in peer trade. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can mend the hinge by Friday. I cannot promise the wire today. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-018 — exchange has two transfer directions

**Initial condition:** exchange has two transfer directions. The view is a projection of the current owner. **Pressure:** ownership hook absent at offer creation. **Player action:** block settlement until transaction owner exists.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. favor/trust/dispute survive restore. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One offer, two signatures. If one changes, stop. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-018 — exchange has two transfer directions

**Initial condition:** exchange has two transfer directions. The view is a projection of the current owner. **Pressure:** first leg succeeds and second fails. **Player action:** prepare and commit both legs atomically.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. duplicate accept cannot mint another trust gain. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can mend the hinge by Friday. I cannot promise the wire today. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-019 — exchange has two transfer directions

**Initial condition:** exchange has two transfer directions. The view is a projection of the current owner. **Pressure:** first leg succeeds and second fails. **Player action:** separate favor-only agreement.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. each asset has one owner before/after. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The stock room is not either of ours. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-019 — exchange has two transfer directions

**Initial condition:** exchange has two transfer directions. The view is a projection of the current owner. **Pressure:** requested item no longer owned. **Player action:** route shelter stock to shared owner.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. failure on either leg leaves both unchanged. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That is my coat. The tin is theirs. Check both names first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-019 — exchange has two transfer directions

**Initial condition:** exchange has two transfer directions. The view is a projection of the current owner. **Pressure:** shared ration appears in private offer. **Player action:** fulfill favor from task event.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. status cannot accept without confirmed transfer. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The stock room is not either of ours. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-020 — exchange has two transfer directions

**Initial condition:** exchange has two transfer directions. The view is a projection of the current owner. **Pressure:** recipient cannot receive another belonging. **Player action:** prepare and commit both legs atomically.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. no shared stock changes in peer trade. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One offer, two signatures. If one changes, stop. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-020 — exchange has two transfer directions

**Initial condition:** exchange has two transfer directions. The view is a projection of the current owner. **Pressure:** dispute lacks completed trade ID. **Player action:** keep offer pending with conflict.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. favor/trust/dispute survive restore. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can mend the hinge by Friday. I cannot promise the wire today. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-020 — exchange has two transfer directions

**Initial condition:** exchange has two transfer directions. The view is a projection of the current owner. **Pressure:** stale panel dispatches communal stock. **Player action:** return existing result for duplicate accept.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. duplicate accept cannot mint another trust gain. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One offer, two signatures. If one changes, stop. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-021 — exchange has two transfer directions

**Initial condition:** exchange has two transfer directions. The view is a projection of the current owner. **Pressure:** stale panel dispatches communal stock. **Player action:** fulfill favor from task event.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. each asset has one owner before/after. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That is my coat. The tin is theirs. Check both names first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-021 — participant withdraws consent

**Initial condition:** participant withdraws consent. The view is a projection of the current owner. **Pressure:** transfer fails after first item. **Player action:** record dispute without inventing transfer.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. failure on either leg leaves both unchanged. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The stock room is not either of ours. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-021 — participant withdraws consent

**Initial condition:** participant withdraws consent. The view is a projection of the current owner. **Pressure:** requested item no longer owned. **Player action:** block settlement until transaction owner exists.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. status cannot accept without confirmed transfer. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That is my coat. The tin is theirs. Check both names first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-022 — participant withdraws consent

**Initial condition:** participant withdraws consent. The view is a projection of the current owner. **Pressure:** requested item no longer owned. **Player action:** return existing result for duplicate accept.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. no shared stock changes in peer trade. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can mend the hinge by Friday. I cannot promise the wire today. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-022 — participant withdraws consent

**Initial condition:** participant withdraws consent. The view is a projection of the current owner. **Pressure:** shared ration appears in private offer. **Player action:** separate favor-only agreement.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. favor/trust/dispute survive restore. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One offer, two signatures. If one changes, stop. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-022 — participant withdraws consent

**Initial condition:** participant withdraws consent. The view is a projection of the current owner. **Pressure:** overdue favor tick repeats. **Player action:** route shelter stock to shared owner.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. duplicate accept cannot mint another trust gain. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can mend the hinge by Friday. I cannot promise the wire today. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-023 — participant withdraws consent

**Initial condition:** participant withdraws consent. The view is a projection of the current owner. **Pressure:** dispute lacks completed trade ID. **Player action:** block settlement until transaction owner exists.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. each asset has one owner before/after. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The stock room is not either of ours. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-023 — participant withdraws consent

**Initial condition:** participant withdraws consent. The view is a projection of the current owner. **Pressure:** stale panel dispatches communal stock. **Player action:** prepare and commit both legs atomically.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. failure on either leg leaves both unchanged. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That is my coat. The tin is theirs. Check both names first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-023 — completed trade disputed with reason

**Initial condition:** completed trade disputed with reason. The view is a projection of the current owner. **Pressure:** transfer fails after first item. **Player action:** keep offer pending with conflict.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. status cannot accept without confirmed transfer. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The stock room is not either of ours. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-024 — completed trade disputed with reason

**Initial condition:** completed trade disputed with reason. The view is a projection of the current owner. **Pressure:** transfer fails after first item. **Player action:** route shelter stock to shared owner.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. no shared stock changes in peer trade. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One offer, two signatures. If one changes, stop. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-024 — completed trade disputed with reason

**Initial condition:** completed trade disputed with reason. The view is a projection of the current owner. **Pressure:** accept command arrives twice. **Player action:** fulfill favor from task event.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. favor/trust/dispute survive restore. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can mend the hinge by Friday. I cannot promise the wire today. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-024 — completed trade disputed with reason

**Initial condition:** completed trade disputed with reason. The view is a projection of the current owner. **Pressure:** belonging ID confused with generic item. **Player action:** record dispute without inventing transfer.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. duplicate accept cannot mint another trust gain. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One offer, two signatures. If one changes, stop. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-025 — completed trade disputed with reason

**Initial condition:** completed trade disputed with reason. The view is a projection of the current owner. **Pressure:** recipient cannot receive another belonging. **Player action:** record dispute without inventing transfer.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. each asset has one owner before/after. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That is my coat. The tin is theirs. Check both names first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-025 — completed trade disputed with reason

**Initial condition:** completed trade disputed with reason. The view is a projection of the current owner. **Pressure:** reload after transfer before status save. **Player action:** block settlement until transaction owner exists.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. failure on either leg leaves both unchanged. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The stock room is not either of ours. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-025 — trust reaches upper bound

**Initial condition:** trust reaches upper bound. The view is a projection of the current owner. **Pressure:** ownership hook absent at offer creation. **Player action:** prepare and commit both legs atomically.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. status cannot accept without confirmed transfer. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That is my coat. The tin is theirs. Check both names first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-026 — trust reaches upper bound

**Initial condition:** trust reaches upper bound. The view is a projection of the current owner. **Pressure:** ownership hook absent at offer creation. **Player action:** separate favor-only agreement.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. no shared stock changes in peer trade. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can mend the hinge by Friday. I cannot promise the wire today. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-026 — trust reaches upper bound

**Initial condition:** trust reaches upper bound. The view is a projection of the current owner. **Pressure:** first leg succeeds and second fails. **Player action:** route shelter stock to shared owner.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. favor/trust/dispute survive restore. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One offer, two signatures. If one changes, stop. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-026 — trust reaches upper bound

**Initial condition:** trust reaches upper bound. The view is a projection of the current owner. **Pressure:** requested item no longer owned. **Player action:** fulfill favor from task event.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. duplicate accept cannot mint another trust gain. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can mend the hinge by Friday. I cannot promise the wire today. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-027 — trust reaches upper bound

**Initial condition:** trust reaches upper bound. The view is a projection of the current owner. **Pressure:** belonging ID confused with generic item. **Player action:** prepare and commit both legs atomically.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. each asset has one owner before/after. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The stock room is not either of ours. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-027 — trust reaches upper bound

**Initial condition:** trust reaches upper bound. The view is a projection of the current owner. **Pressure:** recipient cannot receive another belonging. **Player action:** keep offer pending with conflict.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. failure on either leg leaves both unchanged. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That is my coat. The tin is theirs. Check both names first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-027 — trust reaches upper bound

**Initial condition:** trust reaches upper bound. The view is a projection of the current owner. **Pressure:** dispute lacks completed trade ID. **Player action:** return existing result for duplicate accept.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. status cannot accept without confirmed transfer. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The stock room is not either of ours. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-028 — trust reaches upper bound

**Initial condition:** trust reaches upper bound. The view is a projection of the current owner. **Pressure:** dispute lacks completed trade ID. **Player action:** fulfill favor from task event.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. no shared stock changes in peer trade. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One offer, two signatures. If one changes, stop. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-028 — trust reaches upper bound

**Initial condition:** trust reaches upper bound. The view is a projection of the current owner. **Pressure:** stale panel dispatches communal stock. **Player action:** record dispute without inventing transfer.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. favor/trust/dispute survive restore. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can mend the hinge by Friday. I cannot promise the wire today. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-028 — peer offer appears beside caravan market

**Initial condition:** peer offer appears beside caravan market. The view is a projection of the current owner. **Pressure:** first leg succeeds and second fails. **Player action:** block settlement until transaction owner exists.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. duplicate accept cannot mint another trust gain. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One offer, two signatures. If one changes, stop. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-029 — peer offer appears beside caravan market

**Initial condition:** peer offer appears beside caravan market. The view is a projection of the current owner. **Pressure:** first leg succeeds and second fails. **Player action:** return existing result for duplicate accept.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. each asset has one owner before/after. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That is my coat. The tin is theirs. Check both names first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-029 — peer offer appears beside caravan market

**Initial condition:** peer offer appears beside caravan market. The view is a projection of the current owner. **Pressure:** requested item no longer owned. **Player action:** separate favor-only agreement.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. failure on either leg leaves both unchanged. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** The stock room is not either of ours. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-029 — peer offer appears beside caravan market

**Initial condition:** peer offer appears beside caravan market. The view is a projection of the current owner. **Pressure:** shared ration appears in private offer. **Player action:** route shelter stock to shared owner.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. status cannot accept without confirmed transfer. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** That is my coat. The tin is theirs. Check both names first. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S1-030 — peer offer appears beside caravan market

**Initial condition:** peer offer appears beside caravan market. The view is a projection of the current owner. **Pressure:** recipient cannot receive another belonging. **Player action:** block settlement until transaction owner exists.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. no shared stock changes in peer trade. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. CreateOffer conditionally checks only offered items, does not reserve them and may accept stale requests.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can mend the hinge by Friday. I cannot promise the wire today. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S2-030 — peer offer appears beside caravan market

**Initial condition:** peer offer appears beside caravan market. The view is a projection of the current owner. **Pressure:** dispute lacks completed trade ID. **Player action:** prepare and commit both legs atomically.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. favor/trust/dispute survive restore. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Optional sequential delegates do not prove rollback/retry safety. Do not chain gifts; stop if an atomic owner API is absent.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One offer, two signatures. If one changes, stop. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-101-S3-030 — peer offer appears beside caravan market

**Initial condition:** peer offer appears beside caravan market. The view is a projection of the current owner. **Pressure:** stale panel dispatches communal stock. **Player action:** keep offer pending with conflict.

**Expected route and outcome:** Do not commit until the property owner confirms every transfer. Optional sequential callbacks do not prove rollback. duplicate accept cannot mint another trust gain. If one leg fails, both owners remain unchanged and trust/favor effects are not applied.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. TickDay may lower trust each overdue day and RaiseDispute lacks a reason parameter. Review before exposing.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** I can mend the hinge by Friday. I cannot promise the wire today. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

## 30. Final review gate

Refresh this premise against current source; confirm path claims and owner decisions; choose focused verification; leave unproven effects unavailable. Proposal only: no implementation, test, runtime or ledger result is claimed.

## 31. Plan 5 expansion installment 1 — A fair trade begins with terms, not a price

Peer barter should let two survivors decide what an exchange means to them. The same object can be urgent to one person and replaceable to another. A favor may matter more than an item, or neither side may want to trade at all. The player can help make terms clear, but should not calculate a universal fairness score or pressure either person to accept.

This installment starts Plan 5 inside its three defined subfeatures: offers state the whole exchange; settlement moves both sides or neither; and favors, disputes, and closure use confirmed owner state. Item ownership remains with the real property authority. A proposed exchange is not a transfer, and relationship context is not consent.

## 32. A proposal is not yet a deal

The player may help one survivor offer an item, request an item, suggest a favor, or form a mixed exchange only if the current barter owner supports that form. The offer must identify both parties and describe what each promises. If a requested item is held by shared inventory, the player routes through its owner or removes it from the peer proposal.

The recipient can accept, refuse, counter, ask for detail, or defer. A counteroffer creates a new set of terms that both participants must see. The original offer remains unaccepted unless its owner explicitly records a transition.

The player may clarify quantity, condition, timing, due event, expiry, and whether an item is being given or lent only where current APIs can represent those terms. Anything the owner cannot store remains conversation, not a durable contract.

## 33. Value is personal, settlement is factual

The design should let participants value goods differently without labeling one party irrational. A worn glove can matter because it fits; a spare tool can be useful because someone else knows how to repair it; a favor can be meaningful because it frees time on a specific day. The player can listen, ask what would make the exchange useful, suggest an alternative, or step aside.

There is no universal “fairness” number. Do not secretly improve trust because an exchange looks equal by market price, or penalize a survivor for accepting a lopsided trade they willingly negotiated. The system's factual questions are narrower: did each owner permit the transfer, did both sides commit, did the required capacity exist, and were any recorded obligations fulfilled?

The player can also choose not to negotiate. A refusal or no offer is a complete result when either party prefers to keep their property.

## 34. Branching the offer before commitment

- **Accept as written:** both parties confirm the same terms and the owner revalidates availability.
- **Counter with another item:** check its real owner, quantity, and capacity before showing the revised offer.
- **Counter with a favor:** define the action and due condition only where the favor model supports them.
- **Reduce the exchange:** request less, offer a smaller quantity, or narrow the task.
- **Ask for time:** leave pending only if the offer owner persists that state; otherwise explain that the conversation must be reopened.
- **Decline:** close the proposal without an automatic trust or relationship penalty.
- **Withdraw after a stale-state change:** if an item moved, show the conflict and let participants revise or stop.

The branches depend on property, timing, capacity, intended use, and each participant's terms. They do not ask whether the player is generous or greedy.

## 35. Property boundaries the player can inspect

Before any offer is accepted, the player should be able to identify whether an item belongs to one survivor, a shared store, a caravan, a faction, or another owner. The offer UI must not make a personal item appear tradable when it is reserved or centrally owned. A panel preview cannot move or reserve stock.

The player can ask the owner to confirm access, find a different item, request an allowed shared allocation through its own route, or cancel. If ownership cannot be resolved, the offer cannot commit. Do not infer ownership from who is standing near the object.

If the same asset appears in a keepsake/belongings system and the trade owner, use the current PersonalBelongingsSystem route for a gift where appropriate. Do not duplicate the asset into barter state or let a second system claim authority.

## 36. Atomic settlement as a story promise

When both participants accept, settlement is one commitment: both transfer legs succeed or neither does. Only after the asset owner confirms the atomic result may the barter owner close the offer, record completion, fulfill a favor, or apply a supported trust change.

The player-facing branches should be honest about every failure point. If one item moved before the other leg failed, the transaction contract is broken and must be fixed before the feature is exposed. If the owner supports rollback, the player sees that nothing changed. If no atomic route exists, item acceptance remains unavailable while the parties can still discuss or cancel.

Retrying after a timeout or reload must return the original result, not transfer the same goods again. A stable trade identity and owner idempotency are required. A duplicated callback must never create two trust gains or fulfill a favor twice.

## 37. Scenario — “The Gloves and the Socket Set”

This is a content proposal. Confirm the items, property owners, and any repair task before treating the exchange as reachable.

One survivor offers a pair of dry gloves; another asks for a socket set. The player checks who owns each item. If both are personal and the participants agree, the exchange may proceed through the atomic settlement owner. If the socket set belongs to a shared work crew, the player can ask that owner for permission, find a different item, or close the offer.

The receiver may counter with a promise to help repair a door tomorrow. The player asks what “help” means: bring a named tool, complete one supported task, or simply be present. The survivor can define a narrower offer or say they did not mean a formal obligation. If the favor model cannot represent its due condition, keep it as dialogue and do not show a durable favor record.

The item exchange can succeed while the favor is declined, because these are distinct terms. The participants can accept the glove/socket exchange alone, revise both sides, defer, or walk away. Their later relationship is not decided by a price comparison.

## 38. Favors require a witnessed condition

A favor should name an action and the event that would count as fulfillment. “Help when you can” may be meaningful dialogue but cannot become a durable task obligation unless the current owner has a precise completion condition. “Bring this tool to the workshop before the shift starts” is clearer only if delivery, ownership, and timing are supported.

The player can help the parties define the scope, renegotiate due time, accept a supported partial result, or decide not to record a favor. The debtor can ask for a change; the other participant can agree or refuse. Neither has to reveal a private reason.

Late fulfillment, partial work, a blocked route, missing materials, or impossibility should produce distinct outcomes only where current state supports them. Day ticks should not automatically reduce trust repeatedly for the same overdue condition unless that behavior is explicitly part of the current owner contract and is communicated to players.

## 39. Dispute and repair branches

If a participant says the item was not what they agreed to, the player can inspect the trade record, verify item identity/condition through its owner, ask for a repair, renegotiate, or close the dispute unresolved. A dispute must identify the trade and reason if the owner supports those fields.

The player should not decide a dispute only by choosing which survivor they like more. Existing evidence may show that the offer changed, an item was moved before settlement, a due task was not complete, or the claimed condition is unknown. The participants may accept a compromise, request a supported witness, or end the discussion without a resolution.

Any property return, replacement, or repair uses its real asset owner. A dispute panel cannot issue a refund by changing a label. Trust changes, if any, occur once from the verified outcome through the current social/barter owner.

## 40. Player approaches without a barter skill class

**The verifier:** checks ownership, condition, availability, and due terms before anyone commits. This reduces stale offers but may slow the exchange.

**The translator:** asks each person what matters about the item or favor and helps them restate terms. The player can discover a substitute neither had considered.

**The boundary keeper:** stops shared/faction property from being treated as personal stock and routes permission to its owner.

**The minimalist:** helps each side define a smaller exchange and avoids adding a favor neither needs.

**The witness:** checks whether a completion event exists and records only the outcome the owner confirms.

Any approach can be used in one conversation. These are action patterns, not stats, perks, or a morality path.

## 41. Supporting faction roles

A faction can clarify whether an item belongs to its stock, provide a trade term, set access conditions, or answer a dispute about a location it owns. A supporting contact can introduce the parties or witness a task when that role exists. The faction cannot approve a survivor's personal property exchange on their behalf.

The player may negotiate a faction-owned condition, seek a personal alternative, ask a different contact, or leave the peer exchange alone. A faction may decline or impose a cost through its own market/trade owner. Peer trust and faction reputation stay separate.

Do not turn this into a general shop, shared marketplace, or new faction inventory. The peer loop remains between actual participants, with outside actors adding constraints or routes only where current systems support them.

## 42. Branch table — what changes the outcome?

| Evidence or pressure | Player action | Possible participant decision | Owner-backed outcome |
|---|---|---|---|
| Both items are personal and available | Confirm whole exchange | Accept, counter, or refuse | Atomic transfer or no transfer |
| One item is shared stock | Seek permission or substitute | Wait, revise, or cancel | Shared owner authorizes or denies |
| One item moved after offer | Recheck and show conflict | Replace, renew if supported, or withdraw | Stale offer cannot settle |
| Favor is vague | Clarify action/due condition | Make it precise or keep it informal | Persist only if favor owner supports it |
| Due event did not occur | Show evidence and ask next step | Extend, revise, dispute, or close | No false fulfillment |
| Item condition disputed | Check owner record and provenance | Repair, accept, compromise, or remain split | Dispute resolved only by supported state |
| Faction changes access terms | Negotiate or find another route | Accept or walk away | Faction owner controls access, peers control consent |

## 43. Endings that preserve both hands

The exchange can settle exactly as agreed; settle after a counteroffer; close without transfer; remain pending if the owner supports a pending state; fail because an item is unavailable; fulfill a favor; end in an evidence-backed compromise; or remain disputed. A participant can also decide that keeping the item is more useful.

The player can be transparent but still make a poor estimate, or cautious but miss an opportunity. A faction may support one route and reject another. Outcomes emerge from the terms, owners, resources, and evidence. None needs a global alignment ending.

## 44. Persistence and replay requirements

On re-entry, reload current offers, owner states, inventory/belongings, expiry, favor progress, and dispute status. If an offer is no longer valid, show the reason and do not settle stale terms. A completed transaction remains complete after reload. Duplicate acceptance, daily tick, or evidence delivery cannot transfer again or repeat a trust/favor mutation.

The save owner must persist the transaction and any supported obligation with stable IDs and provenance. If pending offers are not persisted, present them as conversations to reopen, not durable commitments. No new save store is proposed here.

## 45. Production gates and acceptance

Before implementation, verify owner APIs for offer creation, revalidation, both-sided atomic transfer, rollback, capacity, expiry, favor evidence, dispute reason, trust-once mutation, and save/restore. Confirm the current property boundary for every item source. If atomic settlement cannot be guaranteed, keep item settlement blocked and return for an owner decision.

Accept this installment only if participants can understand the terms, consent remains separate from relationship, property moves through one authority, favors close from evidence, and failure leaves both sides unchanged. No implementation, test, or runtime result is claimed.

## 46. Plan 5 installment 1 close

This opening installment starts Plan 5 from the meaning of an exchange to the factual conditions for settling it. It gives the player actions around ownership, terms, substitution, favors, evidence, disputes, and faction constraints. Participants can accept, counter, defer, or walk away; a trade commits both sides or neither. The branches arise from different needs, property states, timing, and evidence rather than an alignment score.

---

## Expansion installment 2 — Was it a gift, a loan, a trade, or a favor?

This installment deepens the existing offer and closure loop by making the participants' expectations legible before property moves. These words describe different promises. The design must not imply that every word already maps to a durable transaction type in the current owner. Where an owner cannot represent the distinction, keep it in the conversation, leave the goods where they are, or use a transaction type the owner demonstrably supports.

The central branch is not “generous versus selfish.” It is whether each person understood the same terms, whether the item can move, and whether the current owners can truthfully keep the promise afterward.

## 47. The promise behind the object

A gift has no required return. A loan has a return expectation. A trade has reciprocal terms, even when one side includes work instead of an item. A favor asks for an action whose scope and completion condition should be understood by both people. These are useful distinctions for dialogue and player comprehension; they are not authorization to add four new ledgers or flags.

Before asking for acceptance, show the participants and the player what has actually been agreed. If the current transaction owner stores only a single supported offer shape, do not label its records as a gift, loan, or favor unless that owner exposes a matching contract and lifecycle. A spoken intention can remain a spoken intention. It should not silently become durable debt after a save, a day tick, or a change of scene.

The player should be able to ask a plain question: “What do you expect after this?” The answer may be “nothing,” “bring it back when you are done,” “the other pair of gloves,” or “help me carry the water drum on Tuesday.” Each answer exposes a different possible route, not a hidden morality check.

## 48. Ask both people separately

The offer should allow the player to check each participant's understanding without making one answer visible to the other by default. This is especially useful when a quiet recipient is accepting under pressure or the giver assumes a return that was never stated. The player can ask the giver, ask the recipient, compare their answers, or let them speak directly.

Possible replies describe concrete expectations:

- “Keep it. I have another.”
- “Use it for tonight. I want it back before the morning shift.”
- “I'll take the repair work you offered, if the parts are there.”
- “I thought you wanted the tool, not a promise.”
- “I did not agree to owe her anything.”

These lines are examples for later authored content, not current canon dialogue. Participants can correct themselves, qualify an answer, decline to explain, or refuse to continue. The player can restate the terms neutrally, ask each person to confirm, propose a smaller arrangement, or close the conversation. A participant should never be marked as dishonest solely because the two recollections differ.

## 49. Mismatched expectations pause the transfer

If one person means “keep it” and the other means “return it,” the player sees the mismatch before settlement. The mismatch does not accuse either participant. It creates a choice point: make the item a no-return gift if its owner supports gifting and both agree; define a supported favor or trade; keep the return request informal and leave the item in its current ownership; return it now; or end the offer.

The player cannot resolve the disagreement by choosing a more sympathetic speaker. The parties retain consent. If either person withdraws, no transfer occurs. If both revise the terms, the owner must receive the revised offer and revalidate its item and permissions. An old acceptance cannot be reused after the substance of the promise changes.

If no current owner can record an explicit loan, the game must not promise that it will remind the player, recover the object, or track its location. The player may still broker an informal request, but the interface must say that no durable return commitment has been recorded. Better still, if the request is likely to be mistaken for a system-backed loan, ask for a supported gift, trade, or favor structure, or decline to move the asset.

## 50. Scenario seed — The coat kept for the morning shift

This scene is a proposed content seed; confirm the item catalog, owner, character roster, location, and work schedule before treating it as reachable. The coat itself is only an illustrative object. It is not a claim that a particular coat already exists in the data.

At a cold work area, one survivor offers a coat to another. The recipient is due to work outside before dawn. A third person says the coat belongs to a shared crew. The giver calls it a gift; the recipient believes they should return it after the shift; the crew representative has not authorized its removal. All three statements can be true from their speakers' positions.

The player can inspect the current ownership, ask the giver what they meant, ask the recipient what they understood, and ask the crew representative what permission they control. The representative can confirm only the crew's property boundary, not speak for the giver or recipient. The player may find a different approved coat, request shared-stock allocation through the shared owner, ask the giver to offer a personal item, or close the exchange.

If the item is not actually available, the scene can still conclude through clarification without moving anything. No missing asset is conjured to make the content work.

## 51. The gift branch — no hidden balance due

Where the current belongings owner supports gifting for the specific personal item, the giver may explicitly choose to transfer it without a return condition. The recipient may accept or refuse. The player can confirm that neither side expects a later payment, or step back and allow them to agree privately. The transfer uses the gifting/property owner; SurvivorBarterSystem must not create a second copy or a parallel gift record.

A recipient may still feel grateful, awkward, or unwilling to accept. Those reactions can shape later dialogue or a supported relationship event, but the system must not automatically create a favor debt. If the recipient later offers help, treat that as a separate offer requiring its own terms and consent. A kindness does not authorize a future command.

Potential outcomes include: the gift is accepted and the ownership owner confirms it; the item stays with its giver because either participant declines; the parties switch to a different supported arrangement; or the giver retracts the offer before settlement because the item is reserved or needed elsewhere. A retracted gift should not be described as a broken trade.

## 52. The loan branch — return expectation without false tracking

A loan is useful only if the game can preserve the return expectation. Before exposing a durable loan choice, verify that an existing owner can record who lent what, to whom, under what condition, where a return is accepted, how the obligation changes when the item is consumed or damaged, and what happens after reload. This plan does not authorize a new loan ledger.

If those facts are unsupported, the player can still ask whether the parties would consider another arrangement. They may decline the exchange, leave the item with its owner, turn the request into a supported one-way gift, or define a supported task-based favor that does not pretend to return the same asset. If a supported favor can represent only “bring the item to the workbench by morning,” it cannot represent the loan itself unless the item-return event is a real completion event for that owner.

When a loan-capable owner already exists and is verified, useful branches include returning the item early, returning it at the agreed checkpoint, asking to extend the period, reporting that it was damaged, or being unable to return it because the asset has been consumed or lost. The response should follow the actual state and agreed terms. No repeated overdue penalty should accumulate each day unless the existing contract says it does and the player was told.

## 53. The trade branch — exchanged value stays explicit

A trade gives each side reciprocal terms. Those terms need not have equal catalog value. One participant might need the item more; another might value the scheduled work or access more. Avoid a universal fairness score that reduces those needs to a hidden market verdict.

The player can help specify what each side offers, ask for a substitute, remove an unwanted favor, defer until stock is verified, or close the offer. Both participants accept the same complete terms. Any item legs still follow the atomic settlement rule established in subfeature 2: both settle or neither does. A spoken revision after one party accepts invalidates that acceptance until the revised terms are confirmed.

If one side contributes work, do not settle the other side's item immediately unless the existing contract explicitly supports the timing, risk, and failure path. Otherwise, the arrangement is not an immediate barter settlement. Keep it as a supported favor/offer with its own real completion evidence, or do not present the combined arrangement as secure.

## 54. The favor branch — an action, not a price tag

A favor describes an action one person agrees to perform, not a generic currency token. The action needs an owner-backed completion event and a comprehensible scope. “Check the west hinge before the rain” might be supportable if the relevant work event exists; “be there for me” can remain meaningful dialogue without pretending to be a task record.

The player can ask the requester to define the action, ask the prospective helper to state what they can actually do, narrow the request, set a supported time boundary, or leave it informal. The helper may offer a different action, refuse, or ask for tools. The requester may accept a smaller contribution or decline it. Neither side has to disclose a private reason.

If the favor owner already tracks the obligation, a later result can be complete, partial, blocked, renegotiated, or unfulfilled according to its actual fields and events. If those states are not represented, do not display them as persisted statuses. A dialogue close can still acknowledge “you said you would look at it,” but the interface must not invent task progress or settle an item exchange from that line.

## 55. One item can have different meanings without changing its owner

An object can be personally owned, crew-owned, shared stock, or faction property before anyone starts negotiating. Its story and usefulness do not alter that boundary. A player can discover that an item is kept for a particular shift or repair, but narrative importance does not grant permission to trade it.

The item owner confirms whether it can move and through which existing command. A gift uses the supported personal-gifting route. A peer trade uses the verified transaction owner and atomic property route. A shared allocation or market purchase uses its existing stock owner. A loan is available only if a real owner can track and restore that obligation. If two systems both claim the same item, stop and resolve ownership before exposing the action.

The interface can show a small provenance summary: current owner, availability, reserved use if that owner exposes it, and which participant must approve movement. This is a read of existing owner state. It must not become an inventory cache, a new claim registry, or a visual promise that a preview itself reserves the object.

## 56. Supporting factions — witness, access, or constraint

A supporting faction can make the exchange more legible without becoming a third party to every promise. Its contact might confirm that a tool is checked out to a work crew, provide a sanctioned return point, explain a market's opening hours, or witness completion of a job when the current contract allows that role. The contact's knowledge is limited to the faction's actual responsibility.

The faction cannot declare a personal gift on the owner's behalf, force a recipient to accept a loan, or convert an informal favor into an obligation. A faction representative can refuse access to faction stock, state a worksite constraint, or offer a faction route with its own price. The player can take that route, find a personal alternative, seek another permitted witness, wait, or leave the peer conversation unresolved.

Faction support can open a path while leaving the core relationship between the two participants. For example, an authorized return point may make an existing loan contract practical; it does not create the contract. A work crew may provide a replacement tool; it does not prove that the original was returned. Keep faction reputation and peer trust under their separate owners.

## 57. Endings follow the promise actually made

The scene should have several valid closes:

1. **Gift accepted:** the supported property owner confirms a one-way transfer; no favor appears automatically.
2. **Gift declined:** the item stays with its owner and both participants can continue working together.
3. **Terms clarified:** the parties agree to a supported trade and both transfer legs settle, or neither does.
4. **Return request remains informal:** the item does not move if the interface cannot truthfully track the expected return.
5. **Loan deferred:** participants wait until a real loan-capable route or suitable item is available.
6. **Favor narrowed:** an existing completion event can verify the action and both sides accept its scope.
7. **Shared property denied:** the crew or faction owner rejects removal, so the player chooses a substitute or ends the offer.
8. **No agreement:** one or both participants walk away without an invented penalty.
9. **Terms later change:** the current owner revalidates the whole arrangement; old approval is not reused.

These are outcome patterns, not nine new persistent ending flags. Persist only distinctions that the existing state owner can carry and later consume. A line of dialogue can explain a local close without becoming a campaign-wide ending.

## 58. The player's approaches are actions, not an archetype

The player can approach the same scene through different useful actions:

**The plain-language check:** Ask each person what they expect after the exchange. It can expose a mismatch early, but a private answer may remain unknown.

**The property check:** Inspect the actual owner and availability before anyone promises the object. It avoids unauthorized transfers, but it does not decide what the item means to its owner.

**The smaller promise:** Remove a vague future obligation and keep only what both participants can verify now. This can reduce risk while disappointing someone who wanted longer support.

**The direct conversation:** Let the participants negotiate without mediation. The player may learn less, and their own relationship to the exchange stays limited.

**The boundary:** Decline to broker a promise the game cannot represent. The player can explain the limit, ask for a supported alternative, or leave the item where it is.

No approach should be represented as a hidden skill check or an alignment score. They are available player actions whose outcomes depend on participant consent, actual owner state, timing, and supported contracts.

## 59. Branch table — identical item, different agreement

| What each person expects | What the player can do | Condition to move the item | Supported close |
|---|---|---|---|
| Giver expects nothing; recipient accepts | Confirm the one-way gift | Personal property and gifting route allow it | Gift owner confirms once |
| Giver expects nothing; recipient declines | Accept the refusal or ask about an alternative | None; no transfer | Item remains with giver |
| Giver expects return; recipient did not hear that term | Restate terms and ask both to decide | No movement before renewed consent | Supported loan, revised trade, informal discussion, or close |
| Both want reciprocal items | Show both complete legs and revalidate | Both owners permit atomic settlement | Both legs settle or neither |
| One side offers future work | Define the action and completion event | Owner can track it without false timing | Separate supported favor or no combined commitment |
| Item belongs to a crew | Request permission or choose another item | Crew owner authorizes movement | Shared route or no transfer |
| Participants disagree after an item moved | Read the original terms and verified movement record | No improvised panel refund | Existing dispute owner resolves, or conversation remains open |
| Loan return is expected but unsupported | Explain the limit and offer a no-transfer choice | Do not move it under a false durable promise | Wait, choose another supported form, or end |

## 60. Installment 2 persistence and acceptance

The branch must remain understandable after leaving the conversation and returning. On reload, any persisted transaction, gift, favor, or dispute should come from its actual current owner with stable identity and confirmed provenance. If the owner does not persist informal dialogue, it must not reappear as an active loan or debt. Repeated acceptance, arrival at a return point, a day tick, or a witness callback must not transfer an item twice or repeat a relationship mutation.

Before implementation, verify which current owner can represent each promise form, which property route moves each candidate asset, which system confirms favor completion, and what save section restores the resulting state. Any missing capability is a scope boundary, not a reason to invent a panel cache. This installment is accepted when the player can distinguish what was said from what was recorded, participants can revise or refuse without fabricated penalties, and every completed movement follows one authoritative property route.

### Installment 2 close

This installment adds expectation-checking branches to the same three subfeatures: offers show the whole promise; settlement uses the correct property and transaction owner; favors and disputes close only from evidence. Gifts, loans, trades, and favors are clearer to players, while unsupported durable forms remain unavailable. The item, timing, owner, consent, and completion evidence determine the outcome. No moral alignment path, universal fairness score, parallel ledger, or fourth feature pillar is introduced.

## 61. Expansion installment 3 — The other participant cannot accept

An offer belongs to the people who actually made it. A participant may become unavailable between discussion and settlement because of a shift, injury, travel, a closed route, or a changed decision. This installment explores that interruption through the three existing subfeatures: offers keep the parties and terms explicit; both sides settle together; favors and disputes close only from owner-confirmed evidence.

The central question is whether the player can preserve a fair opportunity without treating an absent person's earlier interest as present consent.

## 62. An offer is attached to named participants

Show who proposed each item or task, who is expected to receive it, and who has authority to approve movement. A second survivor cannot step into an offer merely because they share a room, faction, family, shift, or inventory.

If a participant asks to include someone else, the player can create a new supported offer, revise the existing one if the owner supports that transition, or close the original and start over. The new participant reviews the full terms and gives their own consent. The departing participant's acceptance cannot be copied to a replacement.

The visible offer identity should remain stable across edits. If the current owner cannot distinguish the original parties from a revised proposal, do not allow a silent participant change.

## 63. Absence and withdrawal are different states

A participant may be temporarily unavailable and still want the offer later. Another may withdraw. A third may be reachable but unable to confirm. The interface should distinguish these states only if the current owner does; otherwise, use cautious language such as “not confirmed” and leave the state unresolved.

The player can wait if the offer's terms and expiry support waiting, ask whether the absent participant has a permitted contact route, renegotiate with both people later, or close the offer without penalty. They cannot tell the other party that silence means consent.

If the person is injured or under pressure, do not make their personal circumstances public merely to explain the delay. The player can say the offer is not currently confirmable.

## 64. Scene seed — “She meant to bring it after shift”

This is a candidate pattern. Confirm the participants, item owners, shift schedule, travel state, and available re-entry path before making it live.

Two survivors discussed an exchange before one went to work. The second survivor returns with the item ready, but the first has not arrived. A nearby contact says the first is on another assignment; they cannot speak for the absent person. The waiting participant asks whether they can complete the trade with someone else.

The player can show the original terms and ask whether the waiting person wants to keep the offer open, withdraw, or propose a new exchange. They can check the first participant's actual availability through an authorized route, find a substitute item, or close the conversation. If the first participant later returns, they can confirm the original terms, revise them, or say that circumstances changed.

No goods move while one side's current consent is unknown. The waiting person is not punished for changing their mind after a delay.

## 65. Waiting has a cost and a boundary

An offer may remain open until a supported expiry, but the player should see what waiting affects: item availability, a scheduled repair, storage capacity, or the opportunity to trade elsewhere. Only show costs the current owners expose.

The player can keep the offer pending, ask the present participant to release their item, agree on a new time if both parties support it, or close the offer. A panel must not reserve property merely because the player opens a negotiation. If an owner does reserve stock, show that reservation and its duration from the owner state.

When waiting makes a deadline impossible, the player can propose a new arrangement to the available participant. It is a new consent decision, not an automatic consequence of the missed time.

## 66. Replacing a participant creates a new agreement

If the original party withdraws and another survivor offers to help, the player can check the newcomer independently: ownership, availability, terms, and task eligibility. The other original participant also gets a chance to accept or reject the changed proposal.

A replacement may require a different item, favor, timing, access, or witness. The player can negotiate those changes, seek a faction route, or end the exchange. The system must not label the substitute as equivalent unless the affected participants and current owners confirm it.

If the original owner's offer record cannot be edited safely, close it with the reason the owner supports and create a separate offer. Preserve the old record as a closed proposal where the current owner provides history; never reuse its stable identity for unrelated parties.

## 67. Relatives, coworkers, and faction officers do not inherit consent

A relative might deliver an item or message. A coworker may know the participant's schedule. A faction officer may confirm an assignment. None of these facts makes them an authorized proxy for a private transaction.

The player can ask a contact to pass along a neutral request to confirm the offer. The contact can decline. The player must not disclose private terms to a third party unless the participant consented and the current communication route permits it.

Where an existing system supports an explicit representative, verify its scope and identity. A representative can act only within that defined scope. No general proxy rule is introduced in this plan.

## 68. The absent party cannot complete a favor by implication

If one side's contribution is a task, the player needs a task-owner event showing who performed it and whether it meets the agreement. A similar task performed by a substitute does not automatically fulfill the absent participant's promise.

The participants may revise the favor so another person can perform it, accept a partial contribution if supported, or cancel the obligation. The original helper can later explain, dispute, or confirm the change through the current favor owner.

If the action happened while the player was away, use its recorded provenance. Do not infer agreement from a completed work event if the trade owner did not link that event to the offer.

## 69. No one-sided settlement

The waiting participant may ask to hand over their item now and receive the other side later. The player can explain the settlement boundary, seek a supported escrow/hold route only if one already exists, renegotiate into two separately valid transactions, or decline.

Without a real owner that protects both sides and recovery behavior, the player must not expose a one-sided transfer as a pending trade. A conversation can remain open, but its goods remain with their owners until a complete, accepted transaction can settle atomically.

If the owner reports a transaction already committed before the participant became unavailable, display that confirmed result and its provenance. Do not reverse it through dialogue. Any remedy uses the existing dispute or property-return route.

## 70. Supporting factions as contact and constraint

A supporting faction can confirm a member's duty window, provide a permitted meeting location, arrange an authorized witness, or decline to release faction-owned property. Its role concerns availability, access, or property it actually owns.

The faction cannot accept the peer offer, change the absent person's terms, or demand that the other participant keep waiting. The player can ask for another contact, request a new schedule, use a personal alternative, or close the offer.

If the faction imposes a fee or condition to make the meeting possible, show it before either party accepts. Each participant decides whether to use that route.

## 71. Ending mosaic

**Original participant returns and reconfirms:** the original offer is revalidated and settles only if both sides still agree.

**Offer expires without transfer:** each item remains with its owner; the player may reopen only through a new supported offer.

**Present participant withdraws:** the player closes the proposal and does not treat their earlier interest as binding.

**A substitute is proposed:** all affected people review the changed terms and accept or decline separately.

**A contact relays a request:** the recipient's agreement remains unknown until they respond.

**A task completes without transaction consent:** the owner records that task, but the trade stays unsettled unless the parties confirm its connection.

**The transaction had already committed:** show the owner-backed result; route any disagreement through the existing dispute owner.

## 72. Player approaches

**The patient broker:** holds the offer open within its real expiry and lets both parties decide later.

**The boundary keeper:** prevents a proxy or relative from signing for an absent participant.

**The practical substitute finder:** checks whether a different person or item could satisfy the actual need, then presents a new offer.

**The clean closer:** ends a stale negotiation and returns attention to other plans without moving property.

**The evidence reader:** checks whether a task or transfer already occurred before making any claim.

These approaches can be mixed. They are actions shaped by identity, timing, ownership, and participants' choices, not moral alignment.

## 73. Save, retry, and re-entry

On return, refresh each participant's presence, consent, property state, offer expiry, task evidence, and any dispute from its current owner. The fact that a participant was unavailable yesterday cannot close or accept today's offer.

Stable offer identity prevents repeated acceptance after reconnect, reload, or duplicate callbacks. If participants changed, the revised offer must have an owner-supported version or a new identity. Property moves exactly once. A favor event can close exactly once. A contact's relay cannot become a second participant's acceptance.

If a pending offer is not saved, reopen it as a conversation and ask again. Do not reconstruct it from a UI cache or infer consent from an old line.

## 74. Installment 3 close and acceptance

This installment expands the peer barter loop for absence, withdrawal, replacement, and delayed confirmation. The player can wait, close, find a substitute, seek an authorized contact, or propose revised terms. Offers remain participant-bound; item movement remains both-sides-or-neither; favors require linked evidence.

Accept when no proxy inherits consent by association, a changed party receives the complete terms, one-sided transfer is unavailable without a real owner contract, and re-entry cannot duplicate a settlement. This adds no generic escrow, proxy, or participant-substitution system.

## 75. Expansion installment 4 — A witness sees the handoff, not the whole bargain

Some participants want another person present when goods change hands. One may want a neutral witness after an earlier dispute; another may not want private terms exposed. A witness can confirm only the event their position and the current owner allow them to observe.

This installment expands the existing offer, settlement, and dispute loop. It does not create a new trust score, public registry, arbitration office, or witness ledger. If the transaction owner cannot attach a witness to a canonical event, the player must not present testimony as persisted proof.

## 76. Ask what the witness is being asked to see

“Watch the exchange” can mean several things: confirm that both people arrived, observe the item transfer, verify item identity, see the stated condition, hear the terms, or later testify about a dispute. These requests have different privacy and evidence consequences.

Before inviting anyone, the player can ask each participant what needs verification and what they want kept private. The witness candidate can agree to a limited role, decline, or state that they cannot verify the requested fact.

The witness should not be asked to judge whether the deal is fair or decide who is telling the truth. Their account is one piece of evidence. Ownership and transfer still come from the relevant property and transaction owners.

## 77. Consent to an observer is separate

Either participant can decline a witness. The player can propose another mutually acceptable observer, request an owner-generated receipt if one exists, move to a public but permitted location, schedule the exchange later, or end the offer.

One participant's request does not authorize disclosure of the other's item, personal reason, or favor terms. The player can show the candidate only the information necessary for the role both parties approved.

If a faction's property owner requires an authorized witness for a transfer, state that condition before acceptance. The participant may comply, seek personal property instead, request an exemption, or cancel. The faction's role concerns its own asset or access rule, not private barter generally.

## 78. Scene seed — “Stand by the door”

Candidate content only. Validate the participants, property owners, location, witness eligibility, and current transaction API before treating this scene as reachable.

Two survivors agree to exchange a tool and a supply item. One asks a nearby worker to watch the handoff because a similar item was disputed last week. The other agrees to a witness for the transfer but does not want the worker told why the tool is needed.

The player can ask both survivors to define the witness's scope, check whether the worker is eligible and willing, request that the witness confirm only the visible transfer, or find another route. The worker may decline, have a direct interest in the exchange, or be unable to verify the item's condition.

If the existing transaction owner can attach the witness fact, the offer can settle atomically and preserve that narrow observation. If not, the player can arrange a conversation-only witness without promising durable evidence, use a supported owner receipt, or cancel the trade.

## 79. A witness can be neutral without being omniscient

A survivor outside the parties may see both items cross hands. They may not know whether one was promised as a gift, a loan, or part of a favor unless both participants chose to disclose that term.

A faction contact may verify its own stock leaving an authorized store. That does not prove the peer recipient accepted the item's condition or that a separate task was completed. A market clerk may confirm a recorded transaction only within that market's owner state.

The player can choose a witness according to the needed fact: identity of participants, permitted item movement, location access, or completion evidence. If no one can verify the disputed detail, say so and use another supported remedy.

## 80. Keep the terms between the participants

The witness may learn the transaction's full terms only when both participants agree and the owner route requires or supports that disclosure. Otherwise, show the witness only the transfer they are asked to observe.

One party may ask for proof of receipt without disclosing the reason for the trade. The player can suggest an owner-generated confirmation, a limited witness, or a public handoff with private terms. If the current owner cannot separate those facts, do not promise that the exchange can be confidential.

The participants may decide that privacy matters more than witness protection. They can use an owner-backed receipt, wait for a different transaction route, choose a different item, or leave without trading.

## 81. The witness does not settle a disagreement

After an exchange, a participant may dispute whether the item was complete, whether both legs occurred, or whether a favor was fulfilled. Read the canonical transaction and item-owner state first. Ask the witness only about the fact they actually observed.

If the witness saw the transfer but not the condition, their statement cannot settle a condition dispute. If they heard a term but the current owner did not store it, the player should not convert memory into an authoritative contract after reload.

The dispute owner may accept the witness fact, seek another source, offer a supported remedy, or leave the dispute unresolved. The player cannot reverse inventory by declaring the witness credible.

## 82. Supporting factions and limited roles

A supporting faction can provide an authorized exchange space, a contact who can verify its stock, or a rule that requires observation for property it owns. It can also refuse to host a private peer transaction.

The player may use that route, ask for a narrower witness role, find a personal alternative, request a different time, or leave. Faction reputation remains separate from peer trust; using a faction witness does not make the faction a party to the deal.

If a contact has a conflict of interest, the player can disclose it to both participants, choose another witness, or proceed without one. A hidden conflict should not be resolved by an invisible neutrality score.

## 83. Branch table — observation scope

| Requested proof | What a witness may confirm | What still needs its owner |
|---|---|---|
| Both people attended | Presence, if supported and observed | Consent and complete terms |
| Item crossed hands | Visible transfer event | Ownership, atomic settlement, final inventory |
| Item had a named condition | Only if the witness inspected it and condition is represented | Catalog/item owner state |
| Favor was completed | A witnessed task only if task owner links the event | Favor closure and transaction record |
| Participant accepted the deal | Their explicit response, if captured | Offer owner's acceptance state |
| Faction stock was released | Authorized release from faction owner | Recipient acceptance and peer terms |
| Dispute occurred later | The fact the witness personally observed | Dispute resolution and remedy |

## 84. Endings from the witness request

**Both parties accept a limited witness:** transfer proceeds only through the atomic transaction owner.

**One party declines disclosure:** use a narrower observation, an owner receipt, or no trade.

**Witness is unavailable or conflicted:** seek another permitted observer or end the offer.

**Faction requires an authorized observer:** accept the stated condition, request another route, or choose different property.

**Witness confirms transfer only:** later item-condition or favor disputes remain open to their evidence owners.

**No witness model exists:** the player can keep the witness as dialogue only, avoid settlement, or use an owner-backed alternative.

**All participants agree to proceed without one:** settle as usual, retaining exactly the normal transaction evidence.

## 85. Player approaches

**The scope setter:** defines the witness's role before anyone arrives.

**The privacy broker:** asks what can remain undisclosed and checks if the owner supports that separation.

**The neutral finder:** seeks a witness with no direct stake, while accepting that neutrality is not omniscience.

**The owner-receipt user:** avoids a person witness when the transaction owner can produce reliable proof.

**The clean canceler:** ends the trade when participants cannot agree on the observation boundary.

These actions branch on evidence, privacy, access, and consent rather than the player's honesty or generosity.

## 86. Re-entry and persistence

On reload, a persisted witness fact must reference the transaction, observation scope, witness identity, and source owner if those fields exist. If they do not, the player's dialogue history cannot stand in for a formal record.

Repeated arrival of the witness, duplicate callbacks, or retry after a timeout cannot settle twice. The witness cannot receive the goods or terms as a side effect of observing them. If the transaction settled, later disagreement follows the current dispute owner.

## 87. Acceptance questions

1. Do both participants know the witness's exact scope before accepting?
2. Can a participant choose a narrower role, another observer, or no trade?
3. Does the witness claim only facts they could observe?
4. Do property, transaction, favor, and dispute outcomes remain with their existing owners?
5. Are private terms hidden from observers only where the current system can enforce that boundary?
6. Can the player resolve the exchange without a public fairness judgment?
7. Are witness identity and evidence safe from duplicate settlement or replay?

## 88. Installment 4 close

This installment makes the witness an optional, bounded part of the handoff. Participants can set the observation scope, seek a permitted contact or owner receipt, proceed without a witness, or cancel. Witnesses confirm only what they observe; property, terms, settlement, and disputes remain with their existing authorities. No arbitration or witness ledger is added.

## 89. Expansion installment 5 — Someone else depends on the item

An item may be personally owned and still matter to another person's current task. This installment asks what the player does when a valid offer collides with an owner-backed dependency before settlement. That dependency can shape timing or substitution, but it does not automatically give a bystander ownership or veto.

The offer remains between its named participants. The resource or task owner determines whether the item is reserved; the transaction owner still settles both sides or neither.

## 90. Ownership and dependence are different facts

Check who owns the item, whether it is available, and whether a current task or reservation depends on it. A coworker's preference is not a reservation. A goal's importance does not change ownership.

The owner may report an unreserved item, a confirmed task need, or an uncertain schedule. Each result leads to a different conversation. Do not add a dependency registry: consult the current inventory, work, or reservation owner. If none can report the dependency, keep it as dialogue rather than a hidden lock.

## 91. Ask affected people without transferring control

The player can explain that a task may be affected, ask whether the traders will wait, seek another item, or check whether the task can use a substitute. The item owner decides whether to trade; the recipient decides whether to accept revised terms.

The person relying on the item may describe the task or request a new time, but does not automatically cancel another person's trade. If the dependency changes the offer, both participants see the new terms and accept again.

## 92. Scene seed — “The wrench was on the list”

Candidate content only. Confirm the item, ownership, task reservation, deadline, substitute options, and survivors before making this reachable.

Two survivors agree in principle to exchange a wrench for another item. Before settlement, a worker says the wrench is needed for a repair later in the shift. The player checks the task owner. It may confirm a reservation, report none, or show that the schedule is tentative.

If the reservation is real, the player can ask both participants to wait, find a substitute, request a task reschedule, or end the offer. If no reservation exists, the owner and recipient decide whether to proceed after hearing the worker's concern.

## 93. Reassignment is a separate proposal

The player can ask whether another worker or tool can cover the dependency. Verify that person's eligibility, consent, schedule, and equipment. A substitute assignment is not silently accepted because it frees the trade item.

The original worker may accept, refuse, or request a different scope. The trading participants can wait, revise, or withdraw. Each decision is separate.

If no alternate exists, show the consequence confirmed by the task owner. Do not invent a penalty to make the trade feel costly.

## 94. A task dependency is not a hidden reservation

Distinguish a formal reservation from a likely future need. A confirmed reservation may block settlement. A tentative request can prompt negotiation but is not an invisible property lock.

If the inventory owner has no reservation state, say that use elsewhere is unconfirmed and let the parties decide whether to wait. The owner may proceed when the item is truly available; a bystander's preference alone does not veto personal property.

## 95. Revalidate at settlement

Before final acceptance, refresh property, task reservation, capacity, and participant consent. If a reservation appears after one person accepts, invalidate the offer and explain the change. The parties can wait, revise, substitute, or cancel.

If a reservation clears, confirm again when timing or terms changed. Atomic settlement still commits both transfer legs or neither; reserving or releasing stock cannot move one side of the trade.

## 96. Supporting faction routes

A supporting faction may lend a replacement, offer a sanctioned checkout, provide a different work area, or move its own task window. It can refuse or attach a fee through the relevant owner.

The player can present that route to the traders and task owner, negotiate, or decline. The faction cannot force the item owner to preserve the trade or the recipient to accept a substitute.

## 97. Favors cannot substitute for the item's role

One participant may offer to help with the affected task in return for moving the item now. Check whether the favor owner can represent the exact work, due event, timing, and failure state. The task owner must separately approve the worker or method.

If those contracts do not exist, do not settle against an unsupported promise. Completing work later does not retroactively prove that the exchange was authorized.

## 98. Ending routes

**Reservation confirmed:** defer the trade until release or cancel; goods remain with their owners.

**Substitute found:** owners confirm it and participants reaccept complete terms.

**Task rescheduled:** its worker and owner approve; traders then decide again.

**Dependency tentative:** explain uncertainty and let the parties choose.

**No alternate route:** show the real cost; either party may still cancel.

**Item available:** settle atomically even if a bystander would prefer to keep it.

**Dependency appears after settlement:** resolve the task consequence through its owner without falsifying the trade.

## 99. Player approaches

**Reservation checker:** asks the task and inventory owners whether the need is formal.

**Substitute finder:** searches for a valid item, worker, or time.

**Terms renegotiator:** restates the changed cost and asks for fresh consent.

**Property boundary keeper:** prevents a third party's preference from becoming ownership.

**Consequence reader:** explains what the task owner says will happen if the item moves.

## 100. Persistence and replay

On reload, read transaction, property, task, reservation, and replacement states from their owners. If the offer was not saved, reopen the discussion; do not restore a reservation from a panel cache.

Duplicate retries cannot move either item twice. If a reservation changed while the player was away, revalidate the complete offer before showing acceptance.

## 101. Acceptance questions

1. Can the player distinguish ownership from another person's dependency?
2. Does a formal reservation come from its actual owner?
3. Can a tentative need inform negotiation without becoming a hidden veto?
4. Are replacement work, item substitutions, and faction offers separately accepted?
5. Does settlement remain both-sides-or-neither?
6. Can a later dependency affect the task without falsifying the trade?
7. Does reload restore only owner-backed state?

## 102. Installment 5 close

This installment makes a third person's dependency visible during a peer exchange while preserving the named owners' authority. The player can verify a reservation, negotiate timing, find a substitute, ask for a task change, proceed when the item is available, or cancel. No dependency registry, hidden veto, or one-sided settlement is added.

## 103. Expansion installment 6 — The trade settled; now someone wants it back

A participant may regret a completed trade after their circumstances change. Regret is not automatically proof of fraud, and a completed settlement cannot be silently rolled back through dialogue. The player can ask whether both people want a new exchange, check whether the item still exists and who owns it, route a genuine dispute to its owner, or let the completed trade stand.

This installment extends the existing offers, atomic settlement, and dispute/closure subfeatures. It adds no universal return window or post-trade debt.

## 104. Separate regret, changed need, and a disputed term

The player can ask what changed without requiring a confession. A participant may need the item for a new task, discover it was different from the agreed description, feel pressured at the time, or simply wish they had kept it.

These statements lead to different routes. A changed need may support a new offer. A mismatch with recorded terms may be a dispute. Regret alone does not rewrite the other participant's ownership or force a refund.

Check the canonical transaction, item state, and current owners before promising a remedy. Do not decide by which participant sounds more upset.

## 105. Scene seed — “I thought I could spare it”

Candidate content only. Confirm participants, item catalog, ownership, transaction record, current condition, and any replacement route before making this reachable.

Some time after a trade, one participant asks for their item back. The other has used the received item in a repair and says they cannot return it unchanged. The requester explains that a new need arose after the trade, but does not want the whole shelter told why.

The player can ask each participant privately what they want now, inspect the original agreement, check whether the item can be returned, propose a new exchange, find a substitute, or close the discussion. The recipient can agree, counter, refuse, or ask for time.

Possible endings include a voluntary new trade, a partial substitute where supported, an unresolved request, a formal dispute if terms were violated, or the original trade remaining final.

## 106. The original record stays intact

If the trade owner says the exchange completed, preserve that result. A later agreement to return or replace goods is a new transaction with its own consent and identity.

The player can read the original terms to both participants, ask whether a listed condition was breached, or show what the item owner confirms. The record may answer some questions and leave others open.

Do not edit the original offer into the new arrangement. Reusing its identity could erase the history needed to understand both transactions.

## 107. Actions after a return request

- Check original terms and settlement evidence.
- Ask the current owner who holds the item and whether it remains usable.
- Clarify whether the request alleges a broken term or a changed need.
- Offer a new trade, gift, or supported replacement route.
- Let each participant counter, accept, defer, or refuse.
- Seek a faction source for a substitute only with explicit terms.
- Close the discussion without changing ownership if no agreement exists.

The player is a facilitator, not an automatic refund authority.

## 108. A voluntary return is a new offer

If the current holder is willing to return the item, the parties can define what moves back and what happens to the other leg. The transaction owner must support that complete exchange and settle it atomically.

The holder may offer a different item or work instead. The requester can accept or decline. A changed object, condition, or favor is a new term and requires fresh consent.

If the owner supports only a simple item transfer, do not represent a mixed replacement deal as complete. Keep it conversational, choose a supported trade, or stop.

## 109. The item may no longer be returnable

The item could be consumed, incorporated into a repair, damaged, lost, or already transferred onward. The player checks the current asset owner and describes only what it confirms.

The participants can negotiate an alternative, ask a supporting faction about replacement stock, accept that no return is possible, or leave the issue unresolved. A replacement must be a real available item and must use its actual owner.

The game must not spawn a duplicate or take an unrelated item as an automatic refund.

## 110. Regret does not erase consent, but pressure can matter

A participant may say they agreed because they felt they had no choice. The player can ask what terms were presented, whether a refusal was respected, and whether they want the dispute reviewed. The other participant can respond without being presumed guilty.

If the original transaction owner records coercion, unauthorized transfer, or a dispute reason, use that route. If it does not, do not label the trade invalid through a dialogue-only verdict. The player may still offer a new voluntary exchange.

Both participants can decline further discussion. Peer trust changes, if any, come once from the supported outcome owner.

## 111. Supporting faction role

A supporting faction can provide a replacement item, neutral meeting place, or authorized dispute contact for property it controls. It cannot force one survivor to return personal property or decide a private trade's fairness.

The player can take the faction route, request narrower help, seek another supplier, or close the request. A faction condition such as a work shift or access fee is a new proposal and must be shown before acceptance.

If the faction is already a party to the original trade, route the issue through that transaction owner rather than involving a second authority.

## 112. Endings after settlement

**Voluntary return:** parties make a new supported exchange; the first trade remains in history.

**Replacement agreed:** item and transaction owners confirm the substitute and both parties accept.

**Terms were breached:** a dispute owner verifies the issue and applies only its supported remedy.

**Request declined:** the original trade stands; the player does not impose a penalty.

**Item unavailable:** the parties can counter, seek a real substitute, or end the discussion.

**Issue remains disputed:** the player records no fabricated verdict and offers a supported review if available.

**No one wants another transaction:** the player closes the conversation and preserves the existing state.

## 113. Player approaches

**The record reader:** checks what both accepted and what actually moved.

**The needs clarifier:** distinguishes a new need from a claim that the trade was wrong.

**The voluntary broker:** proposes a new exchange without coercion.

**The property checker:** verifies whether a return or replacement is possible.

**The clean closer:** lets a completed trade stand when no new agreement forms.

These approaches branch on present state, evidence, and consent, not generosity or blame.

## 114. Re-entry and replay

After reload, preserve the completed trade, any owner-backed dispute, and any new return offer as separate records. A request to reverse cannot reactivate the original transaction or repeat its trust change.

If a new trade was not persisted, ask again. If a participant changed their mind before that new offer settled, no item moves. If the original owner reports a confirmed remedy, show its provenance and do not apply it twice.

## 115. Acceptance questions

1. Can the player distinguish regret, changed need, and disputed terms?
2. Does the original completed transaction remain intact?
3. Is any return or substitute a new, fully consented offer?
4. Do actual property owners confirm whether an item remains returnable?
5. Can a participant request review without an unsupported dialogue verdict?
6. Are faction remedies limited to their own stock and authority?
7. Are transactions and remedies replay-safe?

## 116. Installment 6 close

This installment handles a participant who wants a completed exchange reversed. The player can verify the original record, distinguish regret from a breached term, offer a new trade, seek a supported replacement, request review, or let the original stand. No automatic rollback, duplicate goods, or post-trade obligation is added.

## 117. Expansion installment 7 — A favor needs a finish line

Some exchanges include work rather than a movable item: sharpening a tool, carrying a load, repairing a latch, or showing someone how to use a device. A favor can be meaningful without becoming an unlimited obligation. This installment makes the promised work, its safe boundary, and the participant's right to stop visible within the existing barter and favor scope.

## 118. Define what was actually offered

Before acceptance, describe the task in terms both people can understand: the intended result, approximate scope, place, timing, materials, and any safety limits. “Fix the door” may mean making it close, replacing a hinge, or making it secure enough to leave unattended. The player can ask for a narrower commitment, propose a different exchange, or decline.

Use the task owner for the real operation and the barter owner for the agreement. Neither dialogue nor an inventory item can substitute for a task permission.

## 119. Scene seed — “The hinge, not the whole door”

One resident offers to repair a loose hinge in exchange for two measures of lamp oil. The recipient agrees because the door needs to close before night. On inspection, the frame is warped and a full repair needs a part neither person has. The player can ask whether the resident will secure the hinge temporarily, renegotiate for a later full repair, return to the original terms before settlement, or stop without pretending the door is fixed.

The exchange concerns a bounded offer, not ownership of the worker's time until the problem disappears.

## 120. Distinguish attempt, partial result, and completion

Work can begin without being finished. A partial repair may make the door usable for a limited purpose, or may leave it unchanged. The task owner determines what occurred; participants decide whether that result fulfills the agreed terms. Do not mark an exchange complete because the worker spent time or because the recipient wanted the outcome.

If the agreement did not specify a partial result, ask both parties before treating it as acceptable. A partial result is not automatically failure, but it is not automatically full settlement either.

## 121. Let terms change only with consent

When the missing part changes time, scope, risk, or material cost, show the revised offer. The worker may ask for more oil, less work, or a later date; the recipient may accept, counter, or stop. Either person can decline without the scene labeling them unreliable.

If the exchange already settled for a completed deliverable, any later adjustment is a new offer governed by the existing transaction rules. Do not reopen an earlier completed exchange by silently rewriting the promised work.

## 122. Safety is not a bargaining chip

No participant should be offered a dangerous shortcut as a cheaper way to satisfy the bargain. If the task owner says the operation needs a qualified worker, unavailable part, or safe conditions, the player can defer, seek a permitted specialist, or choose another exchange. A participant may stop when conditions change.

The other party can be disappointed, but cannot use the barter agreement to override safety or compel labor. A faction can support an alternate route only within its own authority and capacity.

## 123. Accepting a narrower result

The recipient may accept an explicitly limited result, such as a door that closes but is not weatherproof. The player should show the limitation before settlement and preserve it in any supported transaction description. The recipient can also refuse the partial result and ask for the original scope later.

Do not invent a hidden quality tier. If the current task owner reports only completion or interruption, present those facts and let participants negotiate from what is actually known.

## 124. Outcomes follow participant actions

- **Bounded work completed:** settle only the agreed exchange, once, after the task owner confirms completion and both parties accept the terms.
- **Partial result offered:** the recipient accepts the stated limit, proposes revised terms, or declines.
- **Work interrupted safely:** preserve the interruption and return to negotiation; do not charge an unagreed penalty.
- **Needed part is unavailable:** defer, find a supported source, narrow the work, or end the offer.
- **Either participant stops:** leave unsettled goods and work with their actual owners until a new agreement exists.

These branches change the practical outcome without reducing the relationship to kindness or dishonesty.

## 125. Witnesses do not settle the bargain for others

A witness may confirm what they saw: work began, a part was missing, or the door now closes. They cannot decide whether the result satisfies a private agreement unless the participants gave them that role and the current owner supports it. The player can ask for a witness, rely on direct inspection, or keep the terms between the participants.

Do not expose a favor's details on a public board solely to make the branch legible. Record only what the existing transaction and task owners are authorized to retain.

## 126. Supporting faction role

A faction may lend its own part, offer an authorized specialist, or confirm whether its workshop can take the job. It cannot compel either participant to change the deal or certify work outside its expertise. The player can compare the faction route with a narrower peer agreement, wait, or decline the additional conditions.

If a faction supplies a missing part, its cost and ownership remain visible. Do not let a supporting offer erase the original participants' consent or silently alter who owes what.

## 127. Endings for a bounded favor

The scene can close with completed work and accepted settlement; a limited result accepted with its boundary understood; a safe interruption and later appointment; an abandoned offer before goods move; or a new, separately accepted exchange. It can also end with both participants choosing to stop.

No ending implies that a person now owes future labor. If the recipient still needs the larger repair, they can request it later as a new task and agreement.

## 128. Player approaches

**The scope clarifier** asks what result the participants mean before they commit.

**The task verifier** checks the real operation and permitted conditions.

**The partial-result negotiator** states what was accomplished and lets both parties decide.

**The safety-first broker** pauses when the task becomes unsafe or impossible.

**The clean closer** lets either participant end without inventing a debt.

## 129. Persistence and replay

On re-entry, refresh the offer terms, task evidence, material ownership, and settlement from their existing owners. A saved conversation is not proof that the work finished. If a completion event was not persisted, do not settle the exchange from a repeated animation or dialogue line.

Replaying the closeout must not move goods, record labor twice, or turn a partial result into full completion. If terms changed, show the new terms and collect fresh acceptance.

## 130. Acceptance questions

1. Is the promised work concrete enough for both participants to understand?
2. Can they accept a limited result, renegotiate, defer, or stop?
3. Does the task owner report the work outcome and safety conditions?
4. Does barter settle only after confirmed work and accepted terms?
5. Are witnesses limited to what they observed and were asked to do?
6. Can faction assistance support the agreement without controlling it?
7. Are interruptions, new terms, and settlement replay-safe?

## 131. Installment 7 close

This installment gives labor-based favors a clear scope and finish line. The player can define the requested result, respond to partial work or changed conditions, negotiate new terms, defer, or stop safely. Task completion and barter settlement remain with their existing owners; no unlimited labor debt or hidden quality meter is added.

## 132. Expansion installment 8 — A promise due later is not goods in hand

Participants may want to exchange something that is not available until a later task, shift, or delivery. A future promise can create a meaningful route, but it is not present inventory and should not settle as though the item already moved. The player needs to show what is certain, what depends on future events, and whether the current transaction owner can preserve that promise at all.

## 133. Scene seed — “After the next mending shift”

A survivor offers to trade two repaired filters for a tool that another person can hand over now. The repairs depend on parts arriving after a scheduled mending shift. The player can ask whether the recipient is willing to wait, check whether parts and labor are actually available, suggest a present item instead, or end the negotiation without moving the tool.

The scene turns on timing and uncertainty, not on whether either person is trustworthy.

## 134. Separate a present offer from a future condition

The player can review the promised quantity, expected date or event, dependencies, and what happens if the work cannot be completed. The person making the promise can narrow the amount, offer a different date, withdraw, or propose something they already own. The recipient can accept the uncertainty, counter, ask for a current item, or decline.

Do not imply that an expected delivery is guaranteed because it appears in dialogue. Confirm stock, task capacity, schedule, and any reservation with their current owners.

## 135. No unsupported escrow or debt

Do not remove the present item and place it in a panel cache while waiting for future goods. Do not add a new deposit ledger or infer that either participant owes compensation if the condition fails. If an existing transaction owner has no durable deferred-offer contract, keep this as a nonbinding proposal and settle nothing.

The player can still use the scene to compare terms, ask for confirmation, choose a trade using current property, or close. A limitation in persistence is stated honestly rather than covered by narrative shorthand.

## 136. Make the waiting cost visible

Waiting can mean the recipient goes without the offered tool, misses another task, or needs to check back later. Show only effects supported by existing task, inventory, and schedule owners. The recipient can decide that the delay is acceptable, ask for a smaller current exchange, seek another supplier, or let the offer expire.

The promised worker can say what they know about the parts without being forced to guarantee a delivery they do not control.

## 137. Conditions can change before the due point

The needed parts may arrive, be redirected, or fail to arrive; the worker may become unavailable; or the recipient may no longer need the item. At the due point, refresh those facts and ask whether the original participants still want the trade. Do not assume either side renews consent because time passed.

If a condition changed, the parties may revise the date, substitute an available item, create a new supported offer, or end the proposal. Preserve the original terms as the terms they actually made.

## 138. Partial delivery needs a new answer

If only one filter is ready, the recipient can accept a smaller trade, wait for the second, propose a different item, or decline. The worker can offer the partial quantity without representing it as full completion. Any settlement follows the transaction owner's supported rules and requires both participants to accept the changed terms.

Never split a two-sided settlement into a one-sided transfer unless an existing owner explicitly supports that structure and both people consent to it.

## 139. Faction support cannot guarantee another person's promise

A supporting faction may confirm whether it has its own replacement stock, sell a present substitute, or host a later meeting. It cannot guarantee the peer's work or redirect communal goods to cover the shortfall. The player can accept faction terms, ask for a narrower solution, find another supplier, or decline.

If a faction is the actual future supplier, its authorized owner must confirm the delivery conditions. The survivor's separate promise cannot be used to manufacture an institutional obligation.

## 140. Branches at the due point

- **All conditions met:** reconfirm terms and settle through the existing transaction owner.
- **Some goods are ready:** propose an explicitly partial or revised trade; settle only if supported and accepted.
- **Dependency failed:** defer, substitute, renegotiate, or close without inventing a penalty.
- **Recipient no longer needs the item:** withdraw before settlement or propose a new trade.
- **No durable deferred contract exists:** no property moves; the participants can reopen a current offer later.

The ending depends on the condition and each person's decision, not on a secret honesty rating.

## 141. Avoid turning uncertainty into leverage

An expiring offer can pressure the recipient to accept a future promise without checking its dependencies. The player can ask for a longer window, request a present alternative, or let it close. An expiry belongs to the existing offer owner; it should not be invented as punishment for asking questions.

The worker can also refuse a guarantee and offer only what they know. That boundary is not proof that the promise is false.

## 142. Player approaches

**The dependency checker** verifies parts, capacity, timing, and ownership.

**The present-value negotiator** asks whether an available item can replace the future promise.

**The uncertainty accepter** allows a deferred proposal only when its terms and persistence are supported.

**The pressure reducer** extends or declines an expiring offer where the owner permits.

**The clean closer** ends the proposal before either side transfers property.

## 143. Persistence and replay

On reload, refresh the proposal, participants, dependencies, expected timing, current inventory, and any supported reservation from their owners. If the deferred terms were not persisted, do not reconstruct them from memory or a journal sentence. Ask the participants to make a fresh offer.

At settlement, replaying the event cannot duplicate the present item or future delivery. If no future-contract owner exists, the correct persisted result is no settlement.

## 144. Acceptance questions

1. Does the player see which parts are available now and which depend on future events?
2. Can either participant decline waiting, narrow, substitute, or withdraw?
3. Are dependencies, availability, and timing confirmed by their owners?
4. Does the plan avoid unsupported escrow, inventory holds, or debt?
5. Is partial delivery presented as revised terms rather than automatic completion?
6. Can faction support address only resources or capacity it controls?
7. Are deferred offers and eventual settlements replay-safe?

## 145. Installment continuity

A future promise may remain unresolved after the conversation ends. Keep the distinction between a proposal, a persisted commitment supported by the current owner, and a completed trade. If the game supports only the first, show no durable obligation and let the participants make a new offer when the item exists.

## 146. Installment 8 close

This installment adds a careful route for trades that depend on future work or delivery. The player can verify dependencies, accept a supported delay, negotiate partial or current goods, seek a faction's own substitute, or close without moving property. A promise is not settlement, and no unsupported escrow or debt is added.

## 147. Expansion installment 9 — Count the offer before moving it

Participants can agree about the kind of item and still picture different amounts: one wrapped bundle, a handful, a measure, or every piece in a stack. The settlement scene should make quantity and condition inspectable before either side accepts. This is about making the offered property clear, not pricing it through a universal fairness score.

## 148. Scene seed — “The two strips in the bundle”

One participant offers “the cloth bundle” for a tin of preserved food. The recipient expects several usable strips; the owner means the two pieces visible in the wrapping, one of which has a torn edge. The player can open and count the offered bundle with permission, ask the owner to specify the lot, propose a smaller trade, or leave it wrapped and decline.

Neither person needs to be lying. The phrase simply carried different assumptions.

## 149. Show the actual item and unit

Use the current inventory owner to display what can be transferred: item identity, count, and any supported condition or ownership facts. If the interface only knows a catalog item and integer stack count, do not imply it also knows weight, quality, freshness, or hidden contents.

The owner can correct the offer before acceptance. The recipient can ask for a clearer count, inspect where the current system permits, propose another item, or decline without forfeiting a different agreement.

## 150. Let inspection be a meaningful action

The player may ask to see the goods, accept the stated description, bring a permitted witness, or stop the trade. The owner can allow inspection, refuse access to private property, or withdraw the offer. A refusal to expose an item does not prove it is defective, but it may leave the recipient unwilling to accept.

If inspection itself changes ownership, damages packaging, or consumes a resource in the current game, show that cost before the action. Do not invent a free preview when the item owner cannot support one.

## 151. Resolve mismatched assumptions before settlement

When the count or condition differs from what the recipient expected, the player can ask which detail mattered, revise the offer, accept the visible lot, or close. Revised terms go to both participants again. The original offer should not silently update after one person accepts.

If both participants still want the trade after seeing the same item, the transaction owner settles the confirmed quantity according to its current contract. If they do not agree, neither side transfers property.

## 152. Avoid turning ordinary variation into a hidden grade

Two pieces may look different without the game having a condition system that can classify them. Narration can describe an authored visible difference, but the transaction should not subtract invisible value or invent a quality number. Ask the participants whether the stated lot meets their terms.

If the difference has safety consequences, route it to the responsible safety or task owner. A barter conversation cannot certify equipment for a hazardous task.

## 153. Factions can witness only what they can inspect

A supporting faction may provide a scale, public counter, or trained appraiser if such a route and capability exist. The player can request the service, accept its terms, compare it with direct inspection, or decline. A faction's involvement does not make an uncertain item authoritative.

Do not introduce an appraisal economy or faction-wide quality register. If no current owner supports measurement, keep the exchange to visible count and participant consent.

## 154. Branches from the inspection choice

- **Both accept the displayed lot:** settle the agreed quantity once.
- **Owner revises the count:** present the complete new offer to both sides.
- **Recipient rejects the condition or amount:** keep property with its owner and close or renegotiate.
- **Inspection is declined:** the recipient can accept the known description or leave.
- **No supported measure exists:** state the limit and do not manufacture precision.

The player chooses how much evidence to seek and whether the remaining uncertainty is acceptable.

## 155. Player approaches

**The count checker** confirms only the quantity the inventory owner actually exposes.

**The condition clarifier** asks which visible detail changes the recipient's decision.

**The private-property respecter** accepts that inspection may be declined.

**The neutral witness seeker** requests independent confirmation only when a real source exists.

**The clean closer** ends the negotiation before an unclear lot changes hands.

## 156. Persistence and replay

At acceptance and settlement, revalidate both parties, offered item identities, counts, ownership, and any supported condition facts. If a stack changed while the offer was open, show the current amount and request new acceptance. A panel preview cannot reserve the old count.

On reload, read settled quantities from the transaction and inventory owners. Replaying inspection cannot reveal or consume goods twice; replaying settlement cannot move a second copy.

## 157. Acceptance questions

1. Can both parties see the actual quantity before accepting?
2. Does the game avoid claiming condition data that the owner does not expose?
3. Can either participant inspect, revise, refuse inspection, or close?
4. Are new counts and conditions reaccepted by both sides?
5. Are inspection and settlement handled by current item and transaction owners?
6. Are faction witnesses limited to a real capability and their stated terms?
7. Does replay preserve the exact settled quantity?

## 158. Installment continuity

If the participants agreed to a description rather than inspection, preserve that fact. A later disagreement can begin from what each accepted, but the scene must not claim that the recipient inspected the goods when they did not. Any remedy follows the existing dispute and new-offer routes.

## 159. Installment 9 close

This installment makes quantity and visible condition part of the player's settlement choices. The player can inspect, clarify, revise, accept known uncertainty, or stop before property moves. Inventory and transaction owners determine what exists and what settled; no hidden quality scale or one-sided transfer is added.

## 160. Expansion installment 10 — Agree where the handoff happens

Two participants can accept the same items and still lack an agreed place or time to exchange them. A handoff location affects access, safety, privacy, and whether both people can carry the goods. The player can help settle those practical terms while keeping property and settlement with their current owners.

## 161. Scene seed — “Not in the clinic corridor”

Two survivors agree on a small exchange but choose different meeting places. One suggests the clinic corridor because it is central; the other does not want private trading near patients. The player can ask each participant what locations are acceptable, check access and schedule, propose an authorized neutral point, reschedule, or end the offer.

No one has to reveal a medical reason to decline the corridor. A practical boundary is sufficient.

## 162. A proposed place is not automatically available

Before confirming, check whether the location exists, is accessible at the proposed time, and permits the participants' intended use. The player can offer the existing market area, a common room, a private authorized room, or no meeting if none fits. A location name does not prove the person can enter or that the trade is permitted there.

The location owner supplies access and availability. The transaction owner supplies any supported meeting or settlement terms. Do not create a second location-access system inside barter.

## 163. Preserve participant choice and privacy

Each participant can accept the proposed place, request a different one, ask for a public handoff, choose a permitted private location, or withdraw. The player can state what each site exposes: who may observe, whether access is controlled, and whether the path is available. Do not promise secrecy in a common area.

A public location may make the handoff easier to witness but also reveal that a trade is happening. A private location may protect conversation while requiring access permission. Neither is automatically safer or fairer.

## 164. Timing and carrying capacity matter

The participants may be available at different times or unable to carry the full lot in one trip. The player can reschedule, ask whether a smaller trade is acceptable, check for an authorized carrier route, or leave the offer unsettled. Do not infer a physical delivery or reserve items from a planned meeting.

If a task or duty changes availability, return to both parties for revised terms. The original agreement to trade the items does not force them to meet at any hour the player selects.

## 165. A location change requires fresh agreement

If the chosen space becomes unavailable or one participant asks to move, present the new place and any changed privacy, access, or timing terms. Either participant can accept, counter, wait, or close. A location change is not a purely cosmetic edit when it changes who can observe or enter.

If the current transaction owner cannot persist meeting location, keep it as a scene plan rather than claiming a durable contract. Settlement still occurs only through the supported item transaction.

## 166. Do not invent a courier obligation

If one person cannot attend, the other can wait, reschedule, withdraw, or use a representative only where an existing owner supports that exact authority. The player cannot assign a relative, faction contact, or nearby survivor to carry property just because they are present.

Without supported delegated delivery, no one-sided item transfer occurs. The clean alternatives are direct handoff later or closing the proposal.

## 167. Supporting faction role

A faction may offer its own neutral counter, supervised meeting time, or authorized storage space. The player can accept those terms, ask whether both participants may use the site, choose another location, or decline. A faction site does not become neutral merely because it is centrally located.

The faction cannot take custody of the goods unless an existing property and transaction owner supports that role. Avoid escrow, storage, or courier mechanics created solely for this scene.

## 168. Branches at the handoff decision

- **Both accept an available location and time:** proceed to the existing settlement check.
- **One requests a different place:** revalidate access and collect fresh agreement.
- **No acceptable site is available:** wait, choose direct handoff later, or close.
- **A participant cannot attend:** reschedule or withdraw; do not infer proxy authority.
- **One site changes privacy or access materially:** show the difference before reacceptance.

The route follows practical constraints and each participant's choice, not a personality judgment.

## 169. Player approaches

**The access checker** verifies location and participant eligibility.

**The privacy explainer** describes who may observe without promising secrecy.

**The schedule matcher** looks for a time both participants can accept.

**The direct-handoff advocate** avoids unsupported carriers or storage.

**The clean closer** ends the offer when no acceptable place exists.

## 170. Persistence and replay

On re-entry, refresh the offer, both parties' consent, location access, time window, item ownership, and settlement state from their owners. A saved meeting plan does not prove that the participants arrived. If the location or schedule changed, ask again before the handoff.

Replaying arrival cannot move items twice. If location terms are not durable, recreate the meeting only through a supported offer and never infer custody from a prior scene.

## 171. Acceptance questions

1. Can both participants choose an acceptable handoff place and time?
2. Are access and location conditions confirmed by their actual owner?
3. Does the player explain privacy limits without promising secrecy?
4. Can a participant reschedule or withdraw without an unrelated penalty?
5. Are carriers, storage, and proxies prohibited unless a current owner supports them?
6. Does a location change trigger fresh acceptance when terms change?
7. Is settlement still atomic and replay-safe after arrival?

## 172. Installment continuity

Keep the agreement on goods separate from a plan for where to meet. If no durable meeting contract exists, the dialogue can coordinate a one-time supported route without claiming the barter system stores a standing handoff location. A missed meeting leaves property with its actual owner.

## 173. Installment 10 close

This installment makes the physical handoff part of the player's negotiation choices. Participants can accept a permitted place and time, request a change, reschedule, or close without transfer. Location access, item ownership, and atomic settlement remain with their current owners; no courier or escrow layer is added.

## 174. Expansion installment 11 — The accepted lot is too heavy to carry

Participants may agree on the goods and place, then discover that the current carrier cannot move the full lot safely. The player can check actual carrying capacity, ask whether both people want to revise quantity, use an authorized transport route, or reschedule. Physical inconvenience must not quietly turn into a one-sided transfer.

## 175. Scene seed — “One trip or two”

The exchange involves several containers of clean water filters and a small tool. One participant can carry the tool but not the full filter lot through the available route. The player can confirm the actual load and path, ask both parties whether a smaller lot is acceptable, check for an available cart through its owner, or leave the items where they are until another time.

Do not invent a weight limit or assume that a nearby survivor, vehicle, or faction depot is available.

## 176. Split the offer only if the owner supports it

Participants may agree to trade a smaller number now and discuss the rest later. If the transaction owner supports separate settlements, create distinct offers with clear quantities and fresh acceptance for each. If it only supports an atomic two-sided trade, do not simulate a partial transfer through two UI callbacks.

The players may instead defer the whole trade, find a supported carrier, choose another location, or close. A technically unsupported split is not rescued by narrating that both sides “understood.”

## 177. A carrier is not a new owner

An authorized transport route can move goods without changing who owns them or who is party to the trade, but only if the current item and transaction owners support that distinction. The carrier needs a defined role, route, and return condition where those are modeled. The player can ask for the service, review the terms, or choose direct handoff later.

If no carrier owner exists, nearby presence is not enough. Do not create temporary inventory, courier duties, or an informal escrow record in the dialogue layer.

## 178. The route may impose its own costs

A cart, safe passage, or faction transport may require time, a tool, an access request, or a fee. The player can show these conditions, seek a shorter load, arrange a different place, or decline. Each affected participant decides whether the altered cost is acceptable.

Do not add a hidden fatigue or carry penalty unless the current needs and transport owners expose it. A scene can describe that the load is awkward without inventing a new encumbrance system.

## 179. Revalidate goods if the handoff is delayed

If participants wait until later, refresh item ownership, count, condition facts, task reservations, and both people's consent. The original offer does not reserve goods unless the current owner explicitly does so. The holder can withdraw or revise before settlement; the other person can accept a new offer or leave.

If the trade depends on future delivery, use the deferred-offer boundary from the prior installment. Do not claim that a delayed handoff has already secured the goods.

## 180. Branches when capacity is insufficient

- **Smaller lot accepted by both:** settle only the supported smaller offer and leave the rest unpromised.
- **Authorized carrier available:** show its costs and move only under the current contract.
- **Another location is easier:** recheck access, privacy, schedule, and fresh consent.
- **No safe route exists today:** wait or close with all property in place.
- **Participants disagree about splitting:** retain the original offer as unsettled; neither side transfers goods.

Every outcome keeps ownership clear at each step.

## 181. Avoid treating logistics as a character flaw

A participant who asks for a smaller load is not trying to cheat. A person who refuses to use a faction carrier may have practical or privacy reasons they do not want to discuss. The player can present the route and its cost without diagnosing either choice.

Likewise, a participant who insists on a single handoff may be protecting the atomic agreement, not being difficult. Keep the trade terms distinct from the participants' personalities.

## 182. Supporting faction transport

A supporting faction may lend a cart or offer a supervised move if its current stock and task owners allow it. The player can accept, negotiate the route or time, ask whether the equipment can be used without faction membership, or decline. A faction vehicle does not automatically provide a storage location or custody contract.

If the faction's offer changes the transaction parties, audience, or property custody, present those new terms separately and seek fresh agreement. The original peer trade cannot silently become a faction-mediated exchange.

## 183. The handoff may be interrupted

The carrier may become unavailable, the route may close, or one participant may need to leave. The player can stop before settlement, return goods to their actual owners, reschedule, or create a new supported offer. Do not leave an item in an unowned “in transit” state because a cinematic started.

If an interruption happens after the transaction owner commits, use its recovery result. Never attempt a second transfer because one participant did not see the first confirmation.

## 184. Player approaches

**The capacity checker** verifies the load and route through their owners.

**The quantity negotiator** offers a smaller trade only with both participants' consent.

**The carrier verifier** checks that the transport role is real and authorized.

**The privacy protector** avoids a carrier or route that exposes the trade unnecessarily.

**The atomic closer** waits until both sides can settle together or ends the offer.

## 185. Persistence and replay

On re-entry, refresh item counts, participant acceptance, carrier availability, route access, any supported custody state, and transaction status. A planned load is not a transported load. A carrier arriving is not proof of delivery.

The transaction owner must settle the same complete trade once. Replaying transport cannot move another copy or split an atomic transaction. If the owner cannot persist custody, use direct settlement only or leave the feature proposal-only.

## 186. Acceptance questions

1. Does the player verify the actual capacity and route before handoff?
2. Can the participants revise quantity or timing without one-sided movement?
3. Are partial settlements used only where the transaction owner supports them?
4. Does any carrier have a real, bounded, owner-backed role?
5. Are transport costs and privacy implications visible before acceptance?
6. Is custody truthful during interruption and after reload?
7. Does the final transfer remain atomic and replay-safe?

## 187. Installment continuity

The goods may be agreed upon before the physical route is ready. Keep those facts separate: accepted terms, transport plan, arrival, and settlement. If the route fails before settlement, property stays with the original owners. A later attempt needs fresh confirmation when timing or conditions changed.

## 188. Installment 11 close

This installment adds capacity and transport choices to the trade handoff. Participants can reduce the lot, find an authorized carrier, change place or time, wait, or close without transfer. No nearby person becomes a courier by default, and every settlement stays with the current transaction owner.

## 189. Transport casebook — The cart is promised elsewhere

The player sees a cart near the meeting point and wants to use it. Check its owner and current reservation before presenting it as available. The owner may confirm a different task, report that it is free, or be unable to verify its status. The participants can choose a smaller load, wait, or end the exchange.

A nearby object is not automatically communal property. A coworker's casual assurance is not a reservation release.

## 190. Case — One participant can carry only one container

The trade includes several sealed containers. The player can ask whether the owner will propose a smaller lot, whether the recipient wants to accept separate future offers, or whether an authorized carrier can move the whole load. If the transaction owner supports only an atomic exchange, no container changes hands until both legs settle together.

The players can also defer and keep the original offer unaccepted. Do not turn the first container into an informal deposit.

## 191. Case — The corridor closes during the move

The location owner or task owner may close the route. The player can stop, return any unsettled goods to their owners through the supported path, choose an authorized alternate route, or reschedule. If settlement already committed, use its reported state; do not roll it back because the transport scene was interrupted.

If the system cannot represent goods in transit, do not narrate a durable transit state. Leave them with their original owners until a supported handoff can occur.

## 192. Case — Weather changes the route

An exterior handoff may become impractical or unsafe. The weather and location owners confirm the current condition. Participants can move indoors if access is allowed, wait for a safer window, reduce the load, or close the offer.

Do not create a new hazard rating for the trade. If the game has no owner-backed weather constraint for this route, keep the change as character preference and avoid claiming a mechanical travel penalty.

## 193. Case — The recipient arrives late

The owner may have waited past the agreed time and needs to leave. The player can verify whether the offer is still valid, ask whether a new meeting is wanted, or close it. The other participant can accept a new time, revise terms, or withdraw.

Do not claim that lateness broke a contract unless the offer owner represents that deadline and consequence. A missed meeting is not settlement and does not transfer goods.

## 194. Case — A participant requests a private carrier

The person may prefer not to announce the trade publicly. The player can check whether a supported private handoff exists, explain its limits, ask both participants to agree, or choose a public location. Do not promise secrecy or invent an anonymous courier system.

If the private route would hide a safety or ownership condition, the player can decline and offer direct handoff, rescheduling, or no trade.

## 195. Case — The faction offers a supervised carry

A faction may say it can move goods under supervision. The player can ask who retains custody, what route is used, whether the faction takes responsibility, and what fees apply. Both traders must accept those terms, and the relevant transaction and property owners must support them.

If custody cannot be represented, the faction may still provide a meeting space or cart, but it cannot hold property in an invented escrow state.

## 196. Case — The load cannot enter the agreed room

The room may have a real capacity or access constraint. Check the location owner. Participants can choose a different authorized space, a smaller supported offer, or another day. The player cannot leave property in an unauthorized corridor or claim that the trade has settled because the participants reached the building.

If the room is only uncomfortable rather than blocked, explain the practical condition and let participants decide whether to continue.

## 197. Case — The meeting point exposes the trade

Another resident may ask what is being carried or why the participants are meeting. The player can give a neutral answer, say they are not discussing private terms, or move to another permitted location if both participants agree. Do not lie about ownership to protect privacy.

The participants may choose to continue in public, wait, or cancel. Their choice does not create a secrecy or trust score.

## 198. Case — The carrier withdraws

An authorized carrier may become unavailable before collection. The player can tell both participants, offer direct handoff, seek a different supported carrier, reduce the lot, or reschedule. The carrier's withdrawal does not make the trade itself invalid unless the route was a required term.

If the carrier's service was a condition both accepted, present the revised offer and obtain fresh consent. No property moves on the basis of the earlier plan.

## 199. Case — The participants disagree about splitting

One person may want to split the lot into two trips, while the other prefers one atomic handoff. The player can inspect whether the current transaction owner permits two distinct trades, clarify that a split would be a new offer, or leave the goods untouched. Neither person can unilaterally define the settlement structure.

The participants may keep the full offer for later, form two explicitly separate trades, find a carrier, or close. Each settled trade must be complete under its own accepted terms.

## 200. Transport decision matrix

| Obstacle | Player check | Available route | Required boundary |
|---|---|---|---|
| Cart reserved | Verify its owner | Smaller load or later time | No assumed communal use |
| Route closed | Check location/task state | Authorized alternate or defer | No invented transit custody |
| Carrier unavailable | Confirm service status | Direct handoff or new offer | Fresh terms if required |
| Privacy concern | Explain audience and limits | Different site or close | No secrecy promise |
| Split requested | Check transaction contract | Separate offers if supported | No one-sided partial transfer |

Use the table only as a branch-design aid. It does not establish new transport or inventory APIs.

## 201. Transport casebook close

These variants let the player solve carrying, timing, route, and privacy problems without creating a courier system. The useful actions are to verify, reschedule, reduce the offer through supported transactions, or stop before transfer. Goods remain with their owners until an atomic settlement succeeds.

## 202. Expansion installment 12 — Define what makes the offer workable

Before a counteroffer, ask each participant which term matters most: amount, timing, quality, condition, privacy, or how the goods will be used. These are conversation prompts, not hidden weights. The player can help make the constraints explicit, but the participants decide whether the trade still works for them.

A participant may care about an object's history more than its market usefulness. Another may want a smaller quantity today because they need to preserve supplies for a later task. Do not force these motives into a universal currency value. A trade is viable when both parties accept its terms under the current transaction owner, not when a hidden evaluator declares the exchange fair.

## 203. Negotiation branch — The requested item is unavailable

The requested item may be reserved, consumed, damaged, or absent. Check the current inventory owner before proposing substitutes. The participant can offer another item, change the quantity, ask for a later exchange, or withdraw. The other party may accept a substitute only after seeing its actual condition and agreeing to the revised terms.

If no equivalent exists, say so. Do not promise that a future shipment will arrive unless the relevant supply authority confirms it. A refusal can lead to another conversation later, but the game should not mark an offer as pending indefinitely when no owner can preserve it.

## 204. Negotiation branch — One person needs the goods urgently

Urgency may lead one participant to ask for a lower price, faster delivery, or a gift. The player can ask what deadline is real and who confirms it. The other participant may choose to help, keep the original terms, offer only part of the goods if supported, or end the exchange.

Do not make urgency a moral coercion shortcut. The person with the goods can decline without becoming cruel; the person in need can ask without becoming manipulative. Let the consequences follow from the actual availability of goods and the choices made, not a morality label.

## 205. Negotiation branch — Labor is proposed as payment

One participant may offer to work in exchange for goods. Verify whether the task has an owner, whether the work is safe and available, and whether the transaction system supports a service as a term. The recipient may accept the proposal, change the task, request a different exchange, or refuse.

If service-for-goods is not represented by the current transaction authority, keep it as a separate task agreement and settle it only through its proper owner. Do not silently grant the goods when the participant promises labor, and do not create a debt record in dialogue. Explain the separation so both people know what has and has not changed hands.

## 206. Negotiation branch — A participant offers a gift instead

The giver may decide that no return item is needed. The recipient can accept, decline, or ask whether the offer carries expectations. The player should not convert gratitude into a debt or a future favor. If the gift route is supported, use it; otherwise explain that the current system handles only a trade and leave the property unchanged.

The scene can remain warm without implying that every gift has a hidden price. If the giver states a condition, record it as a separate explicit agreement only where an owner supports it. The recipient is free to ask for clarification before accepting.

## 207. Negotiation branch — The participants disagree about quality

The item may be functional but worn, incomplete, or uncertain. The holder can describe what they know and allow inspection where permitted. The recipient can accept the stated condition, request a different item, ask for verification, or decline. Neither person should claim a precise grade unless the inventory authority provides one.

If the disagreement concerns a defect discovered after settlement, route it through an existing dispute or return path. If none exists, do not improvise a reversal. The player can help both participants discuss a voluntary repair or future offer, while clearly stating that the original transaction has already completed.

## 208. Negotiation branch — Counteroffer accepted with one term unresolved

Participants may agree on the item and quantity but still disagree about the meeting time. Keep the offer uncommitted until all required terms are resolved. The player can restate the agreed facts, isolate the open question, and ask whether either person wants to wait, choose another time, or withdraw.

If the transaction owner supports partial or staged commitment, make each stage explicit and explain what becomes binding at that point. Otherwise, conversation agreement is not a partial settlement. No participant should discover after the exchange that the player treated an unresolved term as accepted.

## 209. A refusal can preserve the relationship

The recipient may say the goods are not useful, or the holder may decide they cannot spare them. The player can help close politely, ask whether a different offer is welcome, or leave future contact to the participants. A refusal does not automatically create a relationship penalty or a feud.

If either person is disappointed, allow that feeling in dialogue without making it a persistent faction alignment shift. A later exchange can begin from the current relationship and actual history rather than an unseen score for generosity, bargaining, or compliance.

## 210. Negotiation endings and support roles

The scene can end with an accepted offer ready for settlement, a revised proposal awaiting confirmation, an explicit gift where supported, a separate service agreement, a request for verification, a respectful refusal, or no agreement. The player reports the outcome precisely and lets the transaction owner confirm any transfer.

A minor faction can help find a meeting space, verify an item's use, or provide an independent witness if participants request it and the relevant owners permit it. It does not set prices for everyone, guarantee a future supply, or decide whether either participant has behaved fairly. Keep faction influence local to the concrete help it actually provides.

## 211. Expansion installment 13 — The trade continues after the handoff

Settlement is a boundary, not the end of every human consequence. After an exchange, participants may discover that an item meets the stated need, that it needs repair, or that it was less useful than expected. Let the next scene begin from confirmed condition and the participants’ own words. Do not make the trade owner silently judge whether the exchange was fair or create an unapproved reputation score.

The player can ask whether either person wants to discuss what happened. They may share a practical observation, request help with a separate task, express disappointment, or decline to revisit the exchange. The transaction history establishes what moved; the people establish how they feel about it.

## 212. After-settlement branch — The item works as expected

The recipient may report that the item solved the immediate problem. The player can acknowledge the outcome, ask whether they want to tell the original holder, or leave the matter closed. The original holder may appreciate the news, ask no follow-up, or be unavailable. Do not convert a satisfied exchange into a generalized trust or goodwill point.

If the item was used in an owner-backed task, that task owner confirms the task result. The trade scene can reference the item’s role but must not declare a task complete on its own. A useful item may support one specific action without proving that every future item of the same kind will be suitable.

## 213. After-settlement branch — Condition differs from expectation

The recipient may discover a missing part, damage, or a limitation that was not apparent before settlement. First establish what was disclosed and what the inventory owner actually recorded about condition. The player can invite both participants to compare accounts, ask for a specialist inspection, or route the issue through an existing dispute or return mechanism.

If no supported remedy exists, do not promise a rollback, refund, or replacement. The participants can make a new voluntary offer, request an unrelated repair task, or end the discussion. Distinguish an honest mistake from a deliberate misrepresentation only when the evidence supports that distinction; suspicion alone should not become a persistent accusation.

## 214. After-settlement branch — The intended use changes

The recipient may decide to use the item for a different purpose than the one discussed. If the original trade had no use restriction, the player should not invent one afterward. If the parties explicitly agreed to a supported condition, check its current owner and terms before describing a breach. A spoken preference is not automatically a durable contract.

The original holder can feel surprised or concerned, and the recipient can explain or keep their plans private. The player may help them clarify future expectations, but cannot demand disclosure merely because a trade occurred. This branch allows the exchange to affect their relationship through actual dialogue without turning property transfer into ownership over the recipient’s choices.

## 215. After-settlement branch — One party wants to make a second offer

The holder may offer another item after seeing how the first exchange went. The recipient can hear the terms, ask for time, refuse, or propose a different arrangement. This is a new transaction: check current ownership, availability, condition, and participant consent again. Prior acceptance does not authorize the next exchange.

The offer may respond to a revealed need, but the player should not expose that need without permission. A supporting contact can introduce the participants if both agree; they should not broker terms or hold property unless a current owner supports those actions.

## 216. After-settlement branch — A witness remembers the exchange differently

A third person may have observed the handoff and recall a different quantity or condition. The player can ask each participant separately what they remember, compare that account with the transaction owner’s record, and explain any remaining uncertainty. The witness provides testimony, not a verdict.

If the authoritative transaction result is available, use it for what moved. It may not settle every question about what was promised or said beforehand. If the result is unavailable or incomplete, do not reconstruct a precise settlement from confident dialogue. The scene can end with a practical next step and an unresolved disagreement.

## 217. After-settlement branch — A faction asks about the goods

A faction may ask whether the recipient will use the item in a public project or whether the holder can supply more. The player can ask what information the participant is willing to share, explain that one trade does not promise a supply stream, and let the participant answer. The faction may offer transport or inspection if it has the relevant resource and both traders agree.

Faction involvement can create consequences: the recipient might gain access to a repair specialist, the holder may be asked to provide a separate quote, or the group may publicly disagree with the price. These are concrete interactions, not proof that the faction controls future trades. A refusal to involve the faction leaves the completed exchange intact.

## 218. Repair, replacement, and new agreement are distinct

When a problem appears, keep three possible responses separate. Repair changes an item through its repair owner. Replacement requires a new item and a supported transfer. A new agreement creates fresh terms. The player can present only paths that exist and explain what each participant must accept.

Do not make an apology automatically transfer property, and do not make a repair promise count as a completed repair. If the parties agree to discuss the problem later, state whether a reminder or follow-up is actually supported. Otherwise the player can give them a route to reconnect without claiming a tracked appointment.

## 219. No universal ledger of gratitude or grievance

The follow-up should not turn every completed trade into a favor debt, a trust adjustment, or an alignment shift. If an existing relationship owner records a supported change, use its actual semantics and evidence. If not, write the immediate response in dialogue and let future authored scenes reference the event without pretending to calculate the participants’ entire relationship from one exchange.

Different playstyles remain viable: a player may facilitate a direct exchange, ask a specialist to inspect condition, keep factions out, invite a permitted witness, or decline to arbitrate. The consequences emerge from the route chosen and from what the owners confirm, not from an unseen score for generosity or aggressive bargaining.

## 220. Installment 13 endings — Close the exchange honestly

The post-handoff scene can close with satisfaction, an inspection request, an unresolved condition dispute, a separate repair, a fresh offer, a faction referral, or no further conversation. State whether the original settlement remains complete. If it does, later help should be described as a new agreement unless a current dispute owner says otherwise.

This gives the trade a longer narrative tail without weakening transaction integrity. The recipient can value the item, regret the choice, or change their plans; the holder can feel proud, uncertain, or disappointed. Both retain control over their future property and participation. The record says what was exchanged, while the characters decide what that exchange means to them.

## 221. Expansion installment 14 — Several people need the same scarce item

A scarce item can attract more than one offer. The player should first determine whether it is actually available for trade or already reserved by an authoritative task, owner, or shared-use rule. If it is reserved, explain the boundary and do not stage a bidding contest over property that cannot be offered. If it is available, the holder can decide whom to approach, whether to hear multiple proposals, or whether to keep the item.

This creates social tension through scarcity without introducing a universal market simulator. Each potential recipient can explain their need, but the player does not rank their worth. The holder’s ownership and any current shelter allocation rule remain distinct: a private item can be offered by its holder, while shared stock follows its designated owner.

## 222. Scarcity branch — A participant believes the item is communal

One resident may assume that a tool, container, or supply belongs to everyone because it is stored in a common area. The player checks the inventory and location owners before anyone proposes a trade. The item may be communal, privately owned, assigned to a task, or simply unidentified. Each status leads to a different route.

If ownership is uncertain, pause the exchange and ask the responsible owner to identify it. No one should be able to claim property by being the first to offer a substitute. If the item is communal, the trade plan ends or changes into an authorized allocation request. If privately held, its owner can decide whether to offer it, subject to any current restrictions.

## 223. Scarcity branch — Two offers arrive before the holder decides

The player can tell both participants that no agreement exists yet and ask the holder whether they want to consider both offers. The holder may choose one, ask for a comparable fact, request more time, or decline both. The player should not disclose one person’s private terms to another without permission.

If the holder asks for comparison, summarize only the dimensions both parties authorized: quantity, timing, condition, and any explicit service. Do not announce a winner based on hidden value. Once one offer is accepted, tell the other participant only what they are permitted to know; the item’s availability may change, but private reasons can remain private.

## 224. Scarcity branch — The need is urgent but not owner-confirmed

A participant may say the item is essential today. The player can ask what task depends on it and route the claim to the task owner. The need may be confirmed, partially confirmed, or still personal preference. The participant can continue negotiating while the owner checks, but the trade should not be described as an emergency allocation unless the authority says so.

If the task owner confirms urgency, the item may be unavailable for trade. If not, the holder can still choose to help as a personal decision. Either way, do not turn the requester into a villain for asking or the holder into a villain for refusing. The practical branch is whether the item can move and under which owner.

## 225. Scarcity branch — A third person proposes sharing

A resident may suggest lending or sharing the item instead of transferring ownership. Check whether the current inventory and task owners support that arrangement and whether participants understand its duration, custody, and return condition. The holder can accept, reject, or propose a trade. The recipient can ask for a different form or decline.

Do not treat a loan as a barter transaction with an invisible debt. If no loan state exists, keep the proposal as conversation until an owner can represent it. The characters may choose a simple supervised use if that is supported, or they may abandon the idea without any item changing hands.

## 226. Scarcity branch — A faction offers to prioritize its own member

A faction contact may argue that its member’s need should come first. The player can ask which resource or rule the faction controls and whether the item is within that scope. The holder may agree to the request, reject it, or ask for the faction to contribute a separate replacement. A faction cannot claim a privately owned item by citing its affiliation.

If the item belongs to shared stock, the allocation owner applies its actual rules. A major faction may have influence over that owner, but the scene should show the concrete mechanism rather than silently applying a reputation bonus. A smaller faction can offer testimony or logistics without deciding priority for everyone.

## 227. Scarcity branch — The holder wants to avoid choosing publicly

The holder may ask the player to keep competing offers private. The player can arrange separate conversations where authorized, ask the holder what may be disclosed, or explain that availability must be reported to a shared owner. Privacy cannot conceal a transfer that affects a task or shared stock.

The holder can also decide not to hear offers. The player should not pressure them by describing each requester’s hardship in detail. A neutral statement that the item is not available is enough unless the holder asks for more context and the requesters consent to share it.

## 228. Scarcity branch — The selected offer fails before settlement

The chosen recipient may withdraw, become unavailable, or fail to meet an agreed condition before the owner settles the exchange. The player checks the transaction state. If no transfer occurred, the holder may reopen the item to other offers, keep it, or wait. The next participant receives a fresh offer, not an automatic inheritance of the first terms.

If a settlement did occur, the item is no longer the holder’s to offer. Do not roll it back because a second requester arrives with a stronger need. Any return or dispute follows the current transaction authority and requires the relevant participant’s agreement where supported.

## 229. Scarcity outcomes without a fairness score

Possible outcomes include a direct trade, an allocation referral, an authorized shared-use arrangement, a decision to keep the item, a private refusal, or an unresolved ownership check. The scene can acknowledge who received the item and what practical need it addressed. It should not rate the holder as charitable or greedy or the recipient as deserving or manipulative.

If the item’s distribution has broader consequences, let them appear through observable state: a task can proceed, another task waits, or a faction must find a substitute. Do not create a single scarcity reputation number that flattens those consequences. Factions can respond within their existing relationships and resources.

## 230. Installment 14 close — Scarcity makes ownership more important

The player’s route is to verify ownership, hear permitted offers, protect private terms, and allow the holder and owner to decide within their actual authority. Scarcity can make the decision painful, but it cannot turn uncertain property into public stock or give factions automatic priority.

Endings should make clear whether any item moved, whether an allocation owner intervened, and which request remains unanswered. If a later supply arrives, that is a new event; it does not retroactively make the earlier choice costless. This keeps the branch grounded in actions and consequences while leaving room for varied playstyles and faction responses.

## 231. Expansion installment 15 — Both participants confirm the same handoff

Near settlement, the holder and recipient may discover that they understood different terms. One expected a single container; the other prepared two. One thought the exchange would happen now; the other understood that the offer was only being considered. The player should restate the item, quantity, condition, timing, and any supported extra term, then ask each participant separately whether that matches their understanding.

The transaction owner remains the authority for settlement. Dialogue agreement is a chance to catch mismatches, not a substitute for the owner’s validation. If either person hesitates, stop before transfer and let them revise, defer, or withdraw. No one should be carried through the exchange because the other participant already said yes.

## 232. Branch — The quantity differs

The holder may have prepared fewer units than promised or the recipient may have expected a different amount. The player can check the actual inventory and original offer, then present the accurate quantity. The recipient can accept the smaller amount if a new offer can represent it, wait for the full amount, or decline. The holder can confirm, correct, or withdraw their offer.

If the current transaction owner requires all-or-nothing settlement, do not hand over the portion that is present. If it supports a separate smaller transaction, create fresh terms and get both confirmations. The player should not resolve the discrepancy by quietly changing the quantity field or by telling one participant the other will probably agree.

## 233. Branch — Condition is not mutually understood

One person may describe an item as worn but usable while the other expected it to be ready for immediate use. The holder can show the condition where safe and permitted. The recipient can request inspection, ask a qualified person, accept the limitation, revise the offer, or stop.

If the item is sealed or cannot be inspected, state that limit before settlement. Do not invent a warranty. If a condition description comes from the inventory owner, preserve its exact scope; a generic condition label may not support a claim about future performance.

## 234. Branch — One participant thought the exchange was conditional

The recipient may believe the item is a loan, or the holder may believe the recipient will return something later. Ask each person to describe the agreement they believe they made. If the transaction owner supports only transfer, explain that a loan or future service requires a separate supported route. They can revise the offer, postpone, or end the exchange.

Do not choose whichever interpretation makes the scene easier. If the parties cannot agree, no transfer should occur. The player can help them clarify future terms, but cannot create a debt or return obligation in the current trade by narration alone.

## 235. Branch — A final condition is introduced at the handoff

One participant may add a new requirement at the last moment: keep the item private, deliver it later, or promise to use it for a particular task. The other person can accept, ask to revise the terms, or decline. The player checks whether the new condition is representable and whether it changes ownership or use rights.

If the new condition is outside the transaction contract, do not settle it as a binding trade term. The participants may form a separate agreement through its owner or proceed under the original terms only if both still accept them. Late conditions should create a meaningful pause, not a forced penalty.

## 236. Branch — The participant who said yes becomes unavailable

The recipient may be called away before settlement, or the holder may need to leave. Check whether the owner supports a durable pending offer. If it does, the player can explain how long it remains valid and what can change. If it does not, the exchange remains uncommitted and must be offered again later.

Do not let a proxy accept for an absent participant unless the current system grants that authority and the participant authorized it. A faction witness can confirm what was prepared, but cannot consent on either trader’s behalf. The goods stay with their original owner until a valid settlement occurs.

## 237. Branch — The participant disputes the recorded result

After the transaction owner reports settlement, one participant may say the result is not what they expected. First distinguish a display misunderstanding from an actual ownership change. The player can inspect the authoritative transaction result, explain it in plain language, and ask what the participant believes is missing.

If the record and account conflict, do not edit the transaction to satisfy the louder person. Route the dispute to the existing owner or review path. If no path exists, be honest about the limitation and offer a new voluntary conversation. The other participant should not be accused until the evidence supports it.

## 238. Persistence branch — The game resumes between agreement and handoff

If the player saves or changes scenes after participants agree but before settlement, restore only the state the current transaction owner actually persists. A pending proposal can be shown again only if it was captured; otherwise ask both participants to confirm the terms anew. Never reconstruct a commitment from nearby dialogue alone.

After load, verify that property still belongs to the original holder before offering the exchange. If the transaction owner reports completion, do not show the item as available again. This branch protects the narrative promise from duplication and keeps the save path aligned with the transaction authority.

## 239. Two-sided confirmation checklist

Before the owner commits, the scene should make it possible to verify:

1. The offered property is currently available and owned by the named holder.
2. The recipient accepts the current quantity and stated condition.
3. Any timing or location term is confirmed by its own owner.
4. Both parties understand whether this is a transfer, loan, or separate service.
5. Any revised term has received fresh agreement from both participants.
6. A refusal or interruption leaves property unchanged unless settlement already occurred.

This checklist describes the narrative contract. It does not add new transaction fields or persistence semantics.

## 240. Installment 15 close — One confirmation cannot substitute for the other

This installment focuses on the last reversible moment before settlement. A mismatch can lead to a revised offer, a new time, an inspection, a clean withdrawal, or a valid transfer. The owner confirms what actually moved; each participant confirms the terms they accept.

If a later dispute occurs, the player can separate the recorded transfer from the participants’ expectations and route each question to the proper owner. The branch remains consequential without rewarding pressure or punishing caution. A paused exchange is often the clearest result when the two people are not agreeing to the same thing.

## 241. Expansion installment 16 — A trade changes another person’s plan

A peer exchange can affect someone who is not a trader: a shared task may need the item, a family member may expect it to remain available, or another resident may have been waiting for the holder’s decision. The player checks whether that dependency is formal and owner-backed or simply a personal expectation. The distinction changes what must happen before transfer.

The traders retain authority over their private property, subject to current rules. Shared stock follows its designated owner. A third party’s disappointment does not automatically veto a valid private trade, but a confirmed reservation or task requirement may make the item unavailable. Explain the source before asking the holder to decide.

## 242. Branch — A task owner has a confirmed reservation

The inventory or task owner confirms that the item is reserved. The player can tell the holder the item cannot be offered through this trade right now, ask whether a substitute exists, or request that the owner review the reservation. The recipient can wait, revise the offer, or withdraw.

Do not imply that the holder is selfish for honoring the reservation or that the recipient has no right to ask. The task owner controls availability, while the participants control whether they want a different trade. If the reservation changes, the owner reports that change before the item is offered again.

## 243. Branch — The dependency is only an expectation

A friend may have assumed the holder would keep an item for them without a confirmed agreement. The player can ask the holder what was actually promised and whether they want to talk with the friend before settlement. The holder may honor the expectation voluntarily, explain that no promise was made, or revise their decision.

The friend can express disappointment, but cannot seize ownership by invoking an implied promise. If the holder wants to repair the misunderstanding, they can make a separate offer or give an explanation. Do not write a hidden debt or relationship penalty unless an existing owner represents that state.

## 244. Branch — The recipient will use the item for a shared task

The recipient may intend to use the item to help a group. The player can ask whether the task owner knows and whether the item would become shared property or remain privately owned. Both traders should understand the intended use, but the recipient does not need to disclose more than the offer requires.

If the task owner requires the item to be dedicated to the task, the owner explains that condition before settlement. The recipient can accept the restriction, propose another arrangement, or decline. Do not treat a good intention as automatic consent to future access by the whole group.

## 245. Branch — A faction offers to replace the stock later

A faction contact may promise to provide a substitute if the holder releases the item now. The player asks whether the substitute is confirmed, when it will arrive, who owns it, and what conditions apply. The holder can proceed based on a concrete supported offer, wait until replacement arrives, or reject the risk.

If the faction can only express intent, label it as a possibility. Do not transfer the current item on the assumption that future stock will appear. The recipient may still choose another offer, and the holder may keep the original item. A faction’s influence is visible through the resource it can actually deliver.

## 246. Branch — The third party asks the player to block the exchange

The third party may appeal to the player’s authority or friendship. The player can check whether they have any authority to intervene, explain the confirmed rule, or invite the parties to speak directly if everyone agrees. They should not use a panel or generic callback to veto a valid trade because the request feels emotionally compelling.

If the third party presents new information about a reservation or safety issue, pause and verify it with the relevant owner. If the claim is only a preference, the holder can consider it and decide. The player’s role is to surface the distinction, not decide whose need matters more.

## 247. Wider consequences without overriding the traders

After settlement, the task owner may report a delay, a substitute may arrive, or the third party may adjust their plan. Those outcomes can create callbacks and new quests. They should follow from the actual item state and owner decisions, not from a generalized fairness judgment attached to the trade.

If a future task was affected, the player can help source a replacement, ask a faction for a bounded service, or explain the delay. The traders may choose to help, but they are not automatically responsible for every downstream consequence. The scene can preserve both individual agency and the fact that resources are shared across a shelter.

## 248. Expansion installment 17 — A chain of trades must remain separate

One participant may want to trade an item they expect to receive in another exchange. This creates a chain: A offers to B, then B hopes to offer the same item to C. The player checks whether the first transfer has actually settled before treating B as the owner. A promise from A does not give B authority to promise the item onward.

If the first exchange is complete and the inventory owner confirms the transfer, B may make a new offer to C under fresh terms. If it remains pending, B can tell C the item is not yet available, propose a different item, or wait. Do not create an in-transit ownership state or promise that both trades will complete together unless the transaction owner supports atomic multi-party settlement.

## 249. Branch — The first trade settles, the second does not

B may receive the item from A and then discover that C declines the offer. The first trade remains complete. B owns the item if the owner reports that transfer, and can keep it, offer it later, or ask A whether a new trade is possible. The player should not reverse the first exchange just because a second requester arrives with a stronger need.

If B accepted the first item only because of the expected second trade, they can express regret and ask A for a voluntary solution. A may agree, refuse, or be unavailable. Any new exchange requires fresh terms. The story can acknowledge the risk of chaining without turning either participant into a villain.

## 250. Branch — The second recipient needs proof of ownership

C may ask whether B actually owns the offered item. The player can check the inventory or transaction owner and report only the confirmed result. If ownership cannot be verified, pause the second offer. C can wait, request a different item, or withdraw.

A witness can describe seeing the first handoff, but the authoritative owner determines current property state. If the record is missing, do not let confidence or faction affiliation substitute for it. The item stays where the owner says it is until the discrepancy is resolved.

## 251. Branch — A participant wants all trades to depend on one another

A, B, and C may ask to make all three transfers contingent on every person accepting. The player checks whether the current transaction owner supports an atomic group exchange. If not, explain that the game can only settle separate trades and that each could leave someone with an unwanted result if later offers fail.

The participants can choose sequential independent trades, renegotiate a two-party exchange, or stop. Do not pretend that an informal promise links settlements. If atomic group settlement is supported by a future owner, its terms, failure behavior, and save path must be explicit before this branch is authored as operational.

## 252. Branch — The chain depends on a task reservation

The item B expects may be reserved for a shelter task after the first trade. Check the reservation owner before presenting it to C. If the reservation applies, B can keep the item for that purpose, ask the owner whether it can be released, or tell C the item is unavailable. C can wait or revise the offer.

The first trade may itself be invalid if the reservation already restricted A’s ability to transfer the item. Verify before settlement, not after the chain is underway. If the reservation appears only later, the owner explains the change and the transaction authority determines what actions remain possible.

## 253. Branch — A faction proposes to coordinate the chain

A faction may offer a place, witnesses, or transport for multiple participants. The player asks whether the faction is only providing logistics or is taking responsibility for settlement. Each trader must understand the distinction. A meeting space does not make the faction an escrow owner, and a witness does not guarantee future transfers.

The faction can decline to host a complicated exchange or set conditions for its own resource. Participants can accept those limits, use separate direct meetings, or stop. Keep faction help local to the service it actually controls.

## 254. Branch — The chain produces a delay for the last recipient

C may have planned around receiving the item but has no confirmed agreement. The player can tell C what is known without disclosing A’s or B’s private terms. C can find another source, ask for a new date, or abandon the plan. The task owner determines whether C’s dependent work can wait or needs a substitute.

Do not make B responsible for C’s entire plan unless B explicitly accepted that responsibility through a supported agreement. The chain’s uncertainty is visible and may have a cost, but each person retains control over their own offer and task.

## 255. Installment 17 close — Track each transfer on its own facts

Chained trades create rich branching from timing, ownership, reservation, and expectation. The player can verify the first transfer, help form a fresh second offer, or stop when the item is not available. The transaction owner reports every completed movement; dialogue cannot collapse several promises into one magical exchange.

Possible endings include the chain completing one trade at a time, the first trade settling while the second fails, a reservation blocking a later offer, all parties choosing separate terms, or the group abandoning the chain. If a future system supports atomic multi-party exchange, it requires its own explicit contract rather than being implied by this narrative plan.

## 256. Expansion installment 18 — Participants use different meanings for the same terms

Two participants may agree verbally while understanding different quantities, conditions, or timing. A “bundle” may mean several items to one person and one package to another. “Later” may mean after the next task or at some unspecified future time. The player should ask each person to describe the concrete exchange in their own words before settlement.

This is not a test of literacy or intelligence. It is a chance to catch ambiguity. Participants may ask for an object to be shown, a quantity counted, a condition described, or an unfamiliar word explained. They can also stop if the terms remain unclear. No one should be pushed into a transaction they cannot restate.

## 257. Clarity branch — Quantity uses an informal measure

One holder may offer “a small sack” while the recipient expects a fixed number of units. The player checks the inventory owner for the representable quantity and asks both people whether that amount matches their understanding. They can accept the counted quantity, revise the offer, or decline.

Do not translate an informal phrase into a precise amount without evidence. If the system only supports whole item counts, explain that limit. The participants can agree on a supported quantity or leave the exchange uncommitted.

## 258. Clarity branch — Quality words are subjective

Words such as “good,” “usable,” or “almost new” may mean different things. The holder can state observable condition, while the recipient can ask to inspect or ask a qualified person. The inventory owner supplies any authoritative quality field. The participants may agree on a description, request a different item, or stop.

Avoid turning dialogue into a formal grade when no grade exists. A simple concrete note—missing handle, worn edge, sealed container—may be more truthful. If inspection is unsafe or disallowed, say what cannot be checked before settlement.

## 259. Clarity branch — One party assumes a future obligation

The recipient may think the holder will help carry the item later; the holder may believe the recipient arranged transport already. The player asks who is responsible, whether a task owner has confirmed it, and whether that service is part of the trade. The participant can accept the responsibility, seek another carrier, revise the location, or cancel.

If transport is not represented as a transaction term, keep it separate. Neither participant should discover that an informal promise became an obligation after the goods moved. A faction can offer a bounded carry service only if its owner confirms availability and both traders agree.

## 260. Clarity branch — A language or interpretation gap appears

A participant may ask for a term to be explained or translated. The player can seek an authorized interpreter, ask the other participant to restate the offer plainly, or postpone until both understand. Share only what is needed for the exchange and preserve each person’s privacy.

If no reliable interpreter is available, do not present a guess as exact. The participants may use demonstrable quantities or visible condition, but must still agree on the meaning. Translation does not transfer bargaining authority to the helper.

## 261. Clarity branch — The participant cannot inspect the item

The item may be sealed, distant, or currently in use. The holder can describe what is known and state what remains unverified. The recipient may accept that uncertainty, wait for access, request a different item, or decline. If a current owner controls inspection, ask that owner before arranging it.

Acceptance of an unknown condition must be explicit and based on what was disclosed. The player cannot infer it from silence or urgency. If the participants choose to wait, no item transfers; a later inspection may change the offer and requires fresh confirmation.

## 262. Clarity branch — A witness notices an omitted term

A witness may point out that the participants never settled who would return a container or whether an accessory was included. The player can ask both traders whether the term matters and let them add, remove, or leave it unspecified. The witness cannot insert a condition on their behalf.

If the missing term is required by the transaction owner, settlement pauses until it is specified. If it is optional, the traders may proceed without it. Do not make a witness’s concern into a universal trade rule.

## 263. Clarity branch — The offer changes while it is being explained

One participant may revise a quantity or condition after seeing the other’s reaction. The player marks that as a new proposal, not as clarification of the old one. The other participant can accept, counter, ask for time, or withdraw. The transaction owner receives only the terms they both finally confirm.

The scene should preserve the sequence so neither person is blamed for rejecting a condition that was not present earlier. If one party declines, the original offer may remain only if its owner says it still stands. Otherwise the player asks for a fresh offer later.

## 264. Plain-language agreement check

Before settlement, the player can summarize without adding terms: “You are offering one sealed container today. The condition inside is unknown. No transport is included. Do you both still want this exchange?” Each person answers separately. If either corrects the summary, revise it and check again.

This line is a design example, not a required dialog script. Use actual item, quantity, condition, and terms. The transaction owner validates the result; the plain-language restatement reduces preventable misunderstanding.

## 265. Installment 18 close — Shared understanding precedes transfer

This installment creates branches from ambiguous wording, informal measures, unseen condition, transport assumptions, interpretation needs, and late revisions. The player can make terms concrete, ask for a qualified helper, or stop before transfer. Clear agreement is observable in the participants’ confirmation, not inferred from their presence.

Endings include a clarified trade, an explicitly accepted uncertainty, a revised proposal, a request for inspection, an interpreter referral, or a respectful cancellation. Each preserves ownership until the transaction owner confirms settlement and gives the participants a fair chance to understand what they chose.

## 266. Expansion installment 19 — The exchange moves into a public market space

A shelter may host a public exchange area where residents can display goods or ask for offers. The player first checks who owns the space, what may be displayed, whether transactions occur there, and whether the area has any safety or access limits. A public location does not make private property communal, and it does not authorize anyone to inspect goods without the holder’s permission.

Public exchange can create new branches: participants may prefer visibility, ask for privacy, dispute a posted description, or receive unsolicited offers. The player can manage these interactions through the current trade and location owners. Do not add a global price board, auction, or market registry unless the game already supports one.

## 267. Market branch — The holder posts a description

The holder may describe an item’s quantity, condition, and acceptable exchange. The player can check the description against inventory and ask whether the holder wants any details withheld. Interested residents can ask questions or decline. The holder remains free to revise or remove the offer where the current system allows.

Do not turn a description into a guarantee beyond what the holder knows. If condition is uncertain, say so. A correction can be posted if the owner supports it; people who relied on the old description deserve an accurate update before settlement.

## 268. Market branch — A resident makes a public counteroffer

The counteroffer may reveal terms to bystanders. The holder can answer publicly, move to a permitted private conversation, ask the other person to submit a specific offer, or decline. The recipient can keep the counteroffer open only if the trade owner supports pending offers; otherwise, the terms remain conversational until confirmed.

The player should not announce a participant’s private limits to improve bargaining. If the holder wants a minimum quantity or a certain condition, they can choose to state it. The other person can decide whether to continue without pressure from a crowd.

## 269. Market branch — An observer claims the listed price is unfair

A bystander may challenge the exchange based on scarcity or need. The player can ask whether the item belongs to shared stock, whether a rule governs its allocation, and whether the traders want to hear the concern. If it is private property and no rule applies, the bystander’s opinion does not invalidate the trade.

If an allocation rule does apply, the relevant owner explains it before settlement. Do not let public criticism become a hidden penalty. Participants may reconsider voluntarily, explain their terms, or end the exchange. A dispute can continue as dialogue without seizing property.

## 270. Market branch — A trader wants to keep their identity private

A resident may want to inquire without announcing their need. The player checks whether a private offer route exists and how the other participant will receive it. If the system cannot conceal identity, explain the limit before sharing details. The resident can make a public inquiry, ask a trusted intermediary to introduce them if supported, or leave.

An intermediary may carry a message but cannot alter terms or accept for the resident without authority. The holder can decline anonymous approaches. Privacy is a negotiated condition, not a promise the player can make unilaterally.

## 271. Market branch — Several offers arrive at once

The holder may receive several proposals in a short period. The player can help list the confirmed differences, ask which participants consent to comparison, and let the holder choose whether to consider them. No offer becomes accepted merely because it is highest or most urgent.

If the holder chooses one, the others can receive a permitted decline or remain unanswered if no response channel is available. The player should not reveal private offer details to justify the decision. The holder may also withdraw the item from the market and keep it.

## 272. Market branch — The display is mistaken for a reservation

A resident may see an item displayed and assume it is reserved for them or available immediately. The player can clarify whether the holder is inviting offers, has accepted one, or has removed the item. The transaction owner confirms any actual reservation or settlement.

If the display creates repeated confusion, the holder or space owner can ask for clearer signage or a different presentation. Do not mark an item sold until the owner reports transfer. A visible price or offer is not an inventory change.

## 273. Market branch — A faction wants to sponsor the exchange area

A faction may provide tables, lighting, guards, or a public notice. The player checks exactly what it controls and whether its sponsorship imposes conditions. Traders can use the space, seek another location, or decline. Sponsorship does not give the faction ownership of listed goods or authority over private terms.

If the faction requires a fee or a safety check, state it before participants bring goods. Traders can accept, negotiate where allowed, or leave. A major faction can influence access through real resources; a smaller group can provide practical support without claiming regulatory power.

## 274. Market branch — A transaction becomes a crowd dispute

Several observers may argue over whether a trade should proceed. The space owner can enforce actual conduct and access rules; the traders decide whether to continue. The player can move the conversation if allowed, ask a neutral contact to help, or stop the exchange if there is a confirmed safety concern.

Do not turn crowd approval into consent. If the traders want to stop, no one should demand that they finish publicly. If they wish to proceed and all owners permit it, observers cannot veto a private exchange solely by disapproval.

## 275. Installment 19 close — Public visibility changes negotiation, not ownership

The public exchange space can make offers visible, invite comparison, expose privacy concerns, and create disputes about shared rules. The location owner governs the space; the holders govern private offers; the transaction owner confirms transfer. Each faction’s role is limited to resources and rules it actually controls.

Possible endings include a public offer accepted through the owner, a private conversation, a corrected description, an item withdrawn, a location dispute, or no trade. Avoid automatic price discovery and reputation effects. Public attention gives characters something to react to, but it does not decide what belongs to whom.

## 276. Expansion installment 20 — Information can be part of an offer

A participant may offer information instead of a physical item: where a route is usable, how a device behaved, who can provide a service, or what a faction announced. The player checks whether the information is the participant’s to share, whether it is confirmed or hearsay, and whether the transaction owner can represent an intangible term. Do not treat every fact as transferable property.

Both people should understand what kind of information is being offered and what remains uncertain. One party may agree to share a public source, decline to reveal a private contact, or offer only their own observation. If the current barter owner supports only item transfers, keep the information exchange as conversation and do not imply that a trade settled.

## 277. Information branch — The offered information is public

The participant may offer a published instruction, location note, or open notice. The player can verify the current version and ask whether the recipient wants a copy or a direct explanation. The information may be useful, but its public availability does not guarantee that it is accurate for every situation.

If the recipient wants to exchange goods for the information, the owner must support that form of consideration. Otherwise the participants can share it without calling it a settled barter, make a separate supported item trade, or stop. No one should pay for a fact that the holder cannot actually provide as a service.

## 278. Information branch — The offered information is a personal observation

The holder can describe what they saw, when, and under which conditions. The recipient can ask follow-up questions, seek corroboration, accept the account as testimony, or decline to rely on it. The player should not rewrite an observation as verified fact.

The holder may ask that their name remain private. The recipient’s agreement to hear the account does not authorize wider distribution. If the information affects a task or safety decision, route it to the relevant owner and explain who may need to know.

## 279. Information branch — The offer would reveal another person’s confidence

The participant may propose to trade a secret they learned from someone else. The player asks whether that person authorized disclosure. If not, the information should not be offered as ordinary property. The holder can instead share their own conclusion without revealing protected details, seek permission, or withdraw the proposal.

The recipient may still want the information. They can accept a non-identifying summary, ask the original person directly through a permitted route, or decline the exchange. A goods transfer cannot purchase consent on behalf of a third party.

## 280. Information branch — The recipient wants a guarantee of truth

The recipient may ask the holder to guarantee that the information is correct. The holder can explain their source and confidence, seek a witness, or state that they cannot guarantee it. The recipient can accept that uncertainty, ask for verification, offer different terms, or stop.

Do not make the player certify information they did not verify. If the information comes from an authoritative owner, cite that source and its scope. If it is rumor, label it as such. The parties can exchange an account without making it a fact the game treats as confirmed.

## 281. Information branch — The content is an access credential or private route

A participant may offer directions or a code that could expose a restricted place. The player checks the access owner and safety rules before facilitating. The holder may have knowledge but not authority to grant entry. The recipient can request an authorized introduction, accept a public route, or decline.

Do not let barter bypass access control. If the information itself is sensitive, the holder can choose not to disclose it; if the owner permits an introduction, the relevant person confirms access. The trade owner should not create permission simply because the information was exchanged.

## 282. Information branch — The holder wants the recipient to act on the information

The offer may include a request to visit a location, contact a person, or perform a task. The player separates the information from the requested action. The recipient can accept the information while declining the action, ask what the action entails, or refuse both.

If the task is part of the trade, verify that the transaction and task owners support such a combined agreement. Otherwise form a separate task offer with its own consent. A recipient should not discover after giving up goods that they also accepted an unspoken obligation.

## 283. Information branch — The account proves wrong later

The recipient may discover that the information was outdated or mistaken. The player checks what was stated, what source was used, and whether the holder represented uncertainty honestly. The recipient can ask for clarification, request a supported dispute path, or decide not to make another exchange.

Do not automatically reverse the physical trade. If a return or dispute owner exists, follow its rules. If not, the participants can make a new voluntary agreement or close the matter. Distinguish a good-faith error from deliberate deception only when evidence supports it.

## 284. Information-trade boundaries and endings

Possible endings include a public source shared without barter, a bounded personal account exchanged where supported, a third-party confidence withheld, a verified referral, a revised physical trade, an access request routed to its owner, or a canceled offer. The player states what was shared and what remained private.

The trade owner confirms only supported consideration and settlement. The access owner controls permission; the source owner controls authoritative claims; the original speaker controls any personal disclosure. This gives information-based scenes multiple branches without treating knowledge as a universal commodity.

## 285. Expansion installment 21 — A completed trade is disputed

After the transaction owner confirms settlement, one participant may challenge what happened. The player first separates the recorded transfer from the earlier conversation: which items moved, what was confirmed, and which expectation remains disputed. The transaction record governs current ownership; it may not answer every question about what each person believed.

The participant can request a review, speak directly with the other trader, ask a neutral witness, propose a new exchange, or close the matter. The player should not promise a reversal or declare fraud before the evidence is checked. A dispute is a new branch after settlement, not proof that the transaction never occurred.

## 286. Dispute branch — The record is clear, the expectation differed

The transaction owner confirms the exact item and quantity transferred, while one person expected an additional term that was not part of the accepted offer. The player can explain the record, let both participants describe their expectations, and ask whether they want to negotiate a new agreement.

The disappointed person can accept the distinction, remain unhappy, or ask for a voluntary remedy. The other participant can offer help, decline, or clarify their own memory. Do not edit the original transaction to include a condition neither person confirmed.

## 287. Dispute branch — The record is incomplete or unavailable

The player may be unable to verify the transfer details. They can ask the transaction owner to inspect supported history, hear each participant separately, or pause any new exchange until the facts are clearer. A witness can add testimony but does not replace an authoritative record.

If the system cannot establish what moved, state the limitation. Do not choose the account that seems more sympathetic. The participants can agree on a voluntary resolution, seek an existing review owner, or end the discussion unresolved.

## 288. Dispute branch — A witness is also a faction advocate

A witness may have a faction interest in the outcome. The player can disclose that role, ask what they directly observed, and seek another source if available. The witness can still provide useful testimony, but their conclusion is not automatically independent.

The participants can accept the account, request a second witness, or proceed without relying on it. A faction can facilitate a conversation if both traders agree, but should not adjudicate a private trade unless it owns a supported dispute process.

## 289. Dispute branch — The participants agree on a voluntary remedy

The traders may agree to return an item, offer a replacement, repair damage, or make another exchange. Check the relevant property and task owners, then create fresh terms through supported routes. Each person can accept the remedy, revise it, or stop before action.

The remedy does not erase the original settlement unless the transaction owner explicitly reverses it. If the participants exchange new property, record it as a new transfer. Do not use an apology as a proxy for a completed return.

## 290. Dispute branch — One participant refuses a remedy

The other person may want a refund or replacement that the holder will not offer. The player can explain any existing dispute path, help gather facts, or acknowledge that no remedy is supported. The disappointed participant can continue through that route, seek advice, or close the matter.

The player cannot compel a private participant to make a new trade. If a current owner can investigate fraud or safety concerns, route the report there. If no mechanism exists, do not invent a tribunal or automatic reputation penalty as a substitute.

## 291. Dispute branch — The disagreement affects a shared task

The item may be needed for a task that cannot wait for the dispute to conclude. The task owner determines whether a substitute exists, whether work can pause, or whether the item is still usable. The traders can choose whether to cooperate with a temporary plan, but neither is forced to settle their dispute first unless the task owner’s actual rules require it.

The player can keep the two threads separate: resolve the urgent task through its owner and leave the trade disagreement open. Do not seize the item or assign it to the task because it would be convenient. Ownership remains with whoever the transaction owner reports.

## 292. Dispute branch — The participant wants a public accusation

One trader may want to warn others that the exchange went badly. The player can ask them to distinguish confirmed facts from personal interpretation and whether they want to share the matter publicly. The other trader can respond through an authorized channel or decline.

Do not suppress a truthful account solely to protect a faction or the market’s appearance. Do not present an unverified accusation as fact. A public response can describe the dispute as unresolved and identify any review route without exposing unrelated private details.

## 293. Dispute branch — The participants want no further contact

Both traders may prefer to stop speaking. The player can ask whether an owner still needs to complete a required closeout, then respect the boundary. If a new message must be sent, use a permitted channel and share only the operational information.

No reconciliation scene is required. The trade can remain settled and the relationship strained or simply distant through current supported state. The player can move on without forcing a conversation to restore a neat ending.

## 294. Dispute outcome matrix

| Evidence state | Supported next step | Trade status | Avoid claiming |
|---|---|---|---|
| Clear transaction record | Explain or negotiate fresh terms | Settled as recorded | Unaccepted promise was binding |
| Missing record | Review, testimony, or unresolved close | Unknown until owner confirms | Sympathetic account is proof |
| Voluntary remedy agreed | New supported transfer or repair | Original remains recorded | Apology completed the remedy |
| Task depends on item | Ask task owner for substitute or pause | Ownership unchanged | Task need grants seizure authority |
| No review path | State limitation | Settlement remains as reported | New tribunal or penalty exists |

The matrix is a writing aid. It does not create a dispute ledger or reverse-transaction API.

## 295. Installment 21 close — A fair process can end without agreement

Post-settlement disputes create branches through records, testimony, voluntary repair, public response, task dependencies, and the choice to stop contact. The player can help clarify what is known and what each owner can do. They cannot rewrite completed state to make every participant satisfied.

Possible endings include a confirmed misunderstanding, a new remedy, an owner-led review, an unresolved disagreement, a separated task plan, or no further contact. Preserve the actual settlement and the current ownership result. Let the participants decide what the exchange means for their relationship.

## 296. Expansion installment 22 — A new exchange can follow a dispute

After a dispute closes, either participant may later propose another trade. The player should not assume that the earlier disagreement makes the new offer invalid or that a friendly apology restores automatic trust. Check current ownership, item condition, availability, terms, and each participant’s willingness again. The parties can trade, request a witness, choose a different location, or decline.

If the earlier dispute remains unresolved, say so before presenting the new offer. A participant may want to settle the old issue first, while the other prefers to keep the new exchange separate. The transaction owner determines whether any prior transfer or remedy is still pending. No new goods should move as an unspoken settlement of the previous disagreement.

A supporting faction can provide a neutral location, an inspection service, or a witness if it controls that resource and both participants agree. Its involvement does not decide who was right. The participants may accept the support, negotiate directly, or avoid trading with one another again.

The scene can close with a fresh independent trade, a new offer held for review, a decision to use a different counterparty, or no exchange. Any later relationship change must follow current relationship ownership and the actual choices made. A second transaction is a new decision, not a verdict on the first one.

If either participant requests a new offer as a way to repair the relationship, let the player make the purpose explicit: this is a voluntary new exchange, not a required apology or automatic restitution. The other person can accept, ask for a different remedy, or decline without reopening the completed transaction. If the parties do not want to trade again, they can still reach a practical close through an owner-backed repair, a neutral explanation, or no further contact. The narrative can preserve consequence without forcing reconciliation.

If a mediator or witness is invited to that follow-up, ask each trader whether they accept that person and what the person may hear. The mediator can help compare the recorded facts, explain a process, or keep the conversation orderly; they do not decide the value of the original exchange unless a current dispute owner grants that role. Either participant can stop the conversation. A neutral process is an option that may help, not a condition the traders must satisfy before they can move on.

If the follow-up happens in a public exchange area, the traders can move to an authorized private setting or keep the discussion public with no personal details. The location owner confirms access, and each person chooses what to disclose. A resolved transaction does not grant bystanders a right to hear the dispute.

When the exchange closes, state whether the original transfer remains final and whether a separate remedy was agreed. If neither changed, leave the transaction record untouched and let the participants decide whether future contact is welcome.

Before any new offer appears, confirm that the proposed item still belongs to its holder and is not reserved. Prior involvement in the dispute grants no new authority over either person’s property.

If disagreement remains, mark the conversation unresolved and avoid implying that future trades are prohibited unless one participant sets that boundary.

A voluntary remedy needs fresh agreement from both participants and confirmation from the transaction owner before property moves again.

## 296. Expansion installment 23 — A local trade network grows one agreement at a time

Several residents may start connecting people who need goods, tools, references, or services. The player can help with introductions and confirm who is willing to hear an offer. Each exchange remains a separate decision with its own owner and consent. A network of contacts is not a pooled inventory, shared currency, or automatic claim on anyone’s property.

The useful branch is who knows whom and what support each contact can actually provide. One person may know a repairer; another may have a spare part; a faction may provide a safe meeting area. The player can route an introduction, but should not promise availability or disclose personal need without permission.

## 297. Network branch — A participant requests an introduction

The requester can ask to meet a named contact, send a general inquiry, or receive a referral without sharing their reason. The player confirms that the contact is open to introductions and what details may be passed along. The contact may accept, decline, or ask the requester to use an existing public route.

An introduction does not guarantee a trade. The two participants negotiate their own terms, and each can stop. The player should not tell one person that the other has agreed to a price or service before confirmation.

## 298. Network branch — A contact cannot provide the requested item

The contact may know who usually has the item but cannot confirm current stock. The player can ask whether a referral is welcome, help check an authorized source, or tell the requester the lead is unverified. The requester can pursue it, wait, or look elsewhere.

Do not convert a name into a promise. A contact can decline to be approached or ask that their availability remain private. If a faction maintains an authorized public supply notice, use it; otherwise, preserve the uncertainty.

## 299. Network branch — The requester has nothing to offer in return

The person may ask for a gift, loan, or service rather than a direct trade. The holder can decide whether to offer help, ask for different terms, or decline. The player checks whether the transaction owner represents that kind of arrangement. If not, keep it separate from barter and avoid inventing a debt.

The requester can also ask for an introduction to another service. The player can help find one without pressuring the holder to give away property. A network can make support easier to find while leaving each giver free to set boundaries.

## 300. Network branch — A faction wants to coordinate multiple offers

A faction may offer to host a weekly exchange, maintain a list of available goods, or provide a contact point. The player asks what it will actually track, who can access the information, and whether offers expire. The faction can help organize introductions only if the current system and its own rules support that role.

Do not claim the list is complete or live unless someone maintains it. Residents can opt out of public listing, request a private contact, or trade independently. The faction’s coordination may create convenience, but it should not gain ownership of listed property or decide which trades are fair.

## 301. Network branch — A listed offer has already changed

A resident may discover that an item shown as available has been reserved, consumed, damaged, or withdrawn. The holder or inventory owner confirms the current state. The player can update or remove the listing through a supported route, notify people who relied on it where possible, or explain that the current status is uncertain.

The requester can accept a substitute, wait, or end the inquiry. Do not blame them for acting on stale information. If the network cannot refresh availability automatically, descriptions should include a clear date or confirmation step only where the authoring system can support it.

## 302. Network branch — A referral is used to pressure the recipient

The original contact may tell a referred person that they owe the player or faction for the introduction. The player can clarify that the referral carried no obligation unless an explicit supported agreement said otherwise. The recipient can continue, ask for different terms, or leave.

If a faction charges a disclosed fee for brokerage, the terms must be stated before the referral. A friendly introduction is not an implied contract. The contact who made it can express disappointment, but cannot use it to claim the recipient’s future labor.

## 303. Network branch — Several people coordinate a shared purchase

Residents may want to combine contributions to obtain a larger item. The player checks whether the transaction and inventory owners support pooled contributions and shared ownership. If they do not, the group can arrange separate individual trades, ask one person to purchase and later transfer shares through supported terms, or abandon the plan.

Do not create a shared pool from dialogue. Each person should know who owns the purchased item, who may use it, and what happens if someone withdraws. If the owner cannot represent those terms, the group should choose a simpler arrangement or wait for an authorized system.

## 304. Network branch — A dispute affects the wider contact web

One bad exchange may lead a participant to warn a mutual contact. The player can help distinguish a factual account from an opinion and ask whether the person wants it shared. The mutual contact may listen, request evidence, or remain out of the dispute.

Do not make one dispute automatically blacklist either participant from the whole network. If a current owner maintains eligibility rules, apply those rules and allow review. Otherwise, let individual contacts decide whom they want to approach without turning the scene into a universal reputation system.

## 305. Network branch — The contact web helps during shortage

When a supply runs short, the player can use known contacts to ask about substitutes, repairs, or timing. Each lead must be confirmed with the owner before presenting it as available. The requester can accept a different item, wait, revise the need, or stop searching.

A supporting faction can contribute one resource or introduction; a major faction may control broader distribution. Show the specific role and limitation. The network is useful because it shortens the search and brings people together, not because it guarantees stock.

## 306. Network close — Connections are not a common treasury

This installment creates branching routes through referrals, changing availability, gifts, pooled-purchase proposals, faction coordination, and disputed information. The player can help people find one another while preserving property ownership and consent. Each completed transfer still goes through its current transaction owner.

Endings include a confirmed introduction, an unverified lead, a separate gift, a supported purchase, a withdrawn listing, a request to keep details private, or a contact network that declines to coordinate. The story can show stronger local ties without inventing shared inventory or automatic mutual obligations.

## 307. Expansion installment 24 — Negotiation begins with what each person knows

A peer offer can fail because the parties are using different information, not because one is dishonest. The holder may not know an item is needed for an upcoming task; the requester may not know that it is damaged, borrowed, or reserved. Before an offer becomes a commitment, the player helps both parties describe the item, its current owner, condition, quantity, and any known limitation. The inventory or property owner confirms the facts it controls. Personal value and willingness remain the participants’ own judgments.

The requester can ask a question, make a conditional offer, seek a substitute, or leave. The holder can disclose a known defect, say that they do not know, or decline inspection. The player should not force either party to reveal private reasons or complete a deal. The conversation may end with better information but no transfer. That is a useful outcome because both people can choose based on facts rather than a hidden approval roll.

## 308. Offer branch — The item is available but already has a planned use

The holder may possess an item that is not currently assigned but intends to use it for a later repair, trip, or personal project. The item owner confirms whether it is reserved and what rules apply. The holder can offer it now, offer it after the planned task, lend it temporarily, or keep it. The requester can accept a delay, find another item, or stop.

Do not call the item “surplus” just because it sits unused today. The player can ask the owner to compare the proposed transfer with the future task, but cannot decide that the holder’s plan is unimportant. If the future use is not recorded by the owner, phrase it as a stated intention rather than durable reservation state. A later owner-backed task may change the availability, and both participants should be asked again before settlement.

## 309. Offer branch — The parties value the exchange differently

One participant may consider the item essential while the other sees it as replaceable. The player can ask what each party is willing to exchange and what outcome they need. The barter owner validates supported quantities and terms; it need not assign a universal price to every object. Participants can counteroffer, choose a smaller exchange, add a favor if supported, request a gift, or decline.

Do not treat a disagreement over value as evidence of greed. A person may have practical context the other lacks. The player can make the discrepancy visible and ask whether either side wants to share more information. If they still disagree, the trade ends without penalty. A supporting faction may provide a public price reference or appraisal only if the source is current and the parties understand its limits.

## 310. Offer branch — A substitute solves only part of the need

The requested item may be unavailable, but the holder can offer a substitute. The requester describes the task the item must support, and the relevant task owner confirms whether the substitute is suitable. The requester can accept it for limited use, ask for another alternative, wait for the original item, or walk away.

The substitute does not become equivalent in every system because the participants agreed to trade. If it covers only one use, state that. If there is no task owner that can verify compatibility, the parties can make a personal choice while the UI avoids a universal safety guarantee. A partial exchange can still be a satisfying ending when it meets a smaller need with the requester’s informed consent.

## 311. Offer branch — The holder offers less than the requested quantity

The holder may have only a small amount available or may want to keep the remainder. The item owner confirms the transferable quantity. The requester can accept the smaller amount, change what they offer in return, ask for a later transfer, or decline. The holder can state a minimum amount or withdraw the item entirely.

If both participants agree on a partial quantity, settlement must move precisely that quantity and leave the remainder with its actual owner. The transaction owner is responsible for atomicity and rollback if one side fails. The narrative should not portray the exchange as complete if only one party’s property moved. If current transfer APIs cannot guarantee the agreed partial exchange, keep it at proposal stage and mark atomic settlement as a blocker.

## 312. Offer branch — A favor is not a debt unless terms say so

One participant may offer help without asking for a direct item in return. The recipient can accept it as a gift, ask whether the giver expects something later, offer a specific return favor, or decline. The player helps clarify whether there is a reciprocal agreement. Do not create a hidden debt or make future help contingent on a vague phrase like “you know what you owe me.”

If the barter owner supports explicit favor terms, identify the task, scope, expiry, and cancellation rule. The recipient can accept, change the scope, or refuse. If no supported favor record exists, keep the exchange as a voluntary act and do not expose a fictional obligation as a future quest mechanic. A character can remember kindness in dialogue without a save-backed debt ledger.

## 313. Offer branch — Borrowing is different from transfer

The requester may need temporary use of a tool rather than ownership. The property owner confirms whether lending is supported, what return condition exists, and whether the item can be tracked. The holder can lend it, offer a supervised use, propose a trade instead, or keep it. The requester can agree to a return time, ask for a different method, or stop.

Do not represent a loan as a completed transfer unless the owner has a loan contract and restore path. If the current system only transfers inventory, the plan must not invent a borrowed state in a panel. The parties can arrange supervised use where the owner permits it or decline until a real loan route exists. A return reminder should not be promised unless the owning system can issue one.

## 314. Offer branch — The item belongs to a household or shared room

A resident may want to trade an item that is stored in a shared space or used by a household. The item owner determines who may transfer it. The player identifies affected co-owners or authorized users and asks whether their consent is required. The holder can seek permission, offer personal property instead, or withdraw the proposal.

Do not assume that the person nearest the item owns it. If one person has authority to transfer shared property, show the rule and any notification that follows. If ownership is unresolved, pause the trade and ask the current owner rather than choosing the most convenient character. A faction steward can clarify its group inventory but cannot grant residents’ private property.

## 315. Offer branch — A gift and a trade carry different expectations

The requester may accept a gift but feel uncomfortable with a direct exchange. The holder can say whether the item is free to give, whether a future favor is requested, or whether a trade is required. The requester can accept the stated arrangement, suggest another term, or decline. The player should not turn generosity into moral superiority or pressure a holder to give because the requester is in need.

If an owner-backed gift transfer exists, route it through that owner and show the amount. If not, keep the intention in dialogue. A later character may express gratitude without creating a debt. The two participants can also choose a nominal exchange they both prefer. The important result is that the terms are understood, not that the transaction follows a single ideal form.

## 316. Offer branch — A shortage changes the fairness of an offer

The requester may need the item urgently while the holder knows supplies are scarce. The player can inspect the relevant stock owner’s current state, clarify whether this is private property or shared allocation, and ask whether any public distribution rule applies. The holder can donate, trade, delay, or keep the item. The requester can use an approved allocation route, seek a substitute, or accept the risk of waiting.

Do not make desperation erase consent. The player can help the requester reach a service owner without compelling a neighbor to sell. If a faction offers a substitute, state the price, limit, and conditions before acceptance. The trade may be refused even under pressure. A separate emergency authority, if one exists, remains responsible for essential distribution; peer barter is not a covert way to override it.

## 317. Offer branch — Labor appears as part of the exchange

The participants may propose an item for a repair, delivery, lesson, or other service. The relevant task owner confirms the work scope, safety, qualifications, and schedule. Each person separately agrees to the task. A trade offer cannot bypass a duty assignment or turn a resident into unpaid labor without clear terms.

The service may be a one-time favor, a formal task, or a supported barter obligation. The player asks which one the parties intend. If the task owner cannot record the obligation, do not treat a conversational promise as guaranteed completion. The holder can still give the item with no work required, choose a supported task instead, or decline. The recipient may ask for a smaller task or a different exchange.

## 318. Offer branch — A faction broker offers to facilitate

A supporting faction may host a meeting, verify a listed item, provide a scale or workbench, or carry an offer to a remote resident. The player asks whether there is a fee, what information the faction retains, and whether it takes custody of property. The participants can use the service, meet directly, select another intermediary, or abandon the exchange.

The broker’s role is limited to the service it confirms. It does not set the private item’s price, guarantee quality, own the transferred goods, or force either side to close. If the faction charges for facilitation, disclose the amount and payer before the meeting. The broker may also decline a transaction outside its rules. Supporting factions gain a real campaign role through access and trust while major factions and the participants retain their separate authorities.

## 319. Offer branch — The broker wants a record of who is trading

The faction may request names, item descriptions, or a copy of the agreement. The player asks what information is necessary for its service and who can access it. Each participant can consent to the bounded record, request a less identifying route, or refuse. If the broker requires identification, state that condition before the offer is accepted.

Do not imply that a private trade becomes public because an intermediary was involved. If a transaction owner requires a durable record, explain its purpose and audience. If no record is supported, do not promise that the faction will settle later disputes from memory. The broker can provide a witness or host only when the participants agree to that role.

## 320. Offer branch — The listing contact shares more than the owner allowed

A contact may reveal an item’s location, owner, shortage, or personal need beyond the permission they were given. The player can ask what was authorized, who received the information, and whether the owner wants a correction or withdrawal. The contact can apologize, dispute the scope, or explain a misunderstanding. The affected owner can continue the trade, change the audience, or stop.

Do not automatically blacklist the contact from all exchanges. The owner may choose an individual boundary or use an existing conduct review if one applies. The requester should not be punished for relying on an authorized introduction. If the item remains available, remove unnecessary personal detail from the listing. A later trade can proceed with explicit terms, or the owner can decline further contact.

## 321. Offer branch — A defect appears during inspection

The requester may discover wear, damage, missing parts, or an uncertain condition. The holder can disclose what they know, permit an authorized inspection, suggest a lower quantity or different terms, or withdraw. The requester can accept the item as-is, ask for a repair, seek a second opinion, or decline. The relevant safety owner determines whether the item can be used for a particular task.

An inspection is not a guarantee against hidden defects. The transaction record should contain only facts the current owner supports, and the parties should understand any remaining uncertainty. If a safety-critical item cannot be inspected, the trade may stop. A faction technician can advise, but the item owner and task owner retain their respective decisions.

## 322. Offer branch — The item’s origin is uncertain

The holder may not know whether an item was borrowed, recovered, purchased, or assigned. The player asks the current property owner to confirm control before any transfer. The holder can seek a provenance check, offer a different item, or end the conversation. The requester can wait or accept only if the owner confirms authority.

Do not use uncertain provenance as automatic evidence of theft. It is a reason to pause settlement, not a character verdict. If the item belongs to a faction or task store, the relevant owner decides whether it can be released. A supporting group may help identify it, but its conclusion remains within the authority the owner grants. The trade can resume later if ownership becomes clear.

## 323. Offer branch — Delivery is part of the agreed exchange

The parties may agree to meet at a particular time and location. The location owner confirms access, while the task or barter owner confirms whether delivery can be represented. Each participant can request an accessible meeting point, use a permitted intermediary, or choose an in-person handoff. The item remains with its owner until the transfer succeeds.

If one party is late, the other can wait within the stated window, reschedule, leave, or use a supported deposit route. Do not record a transfer when an offer was only placed at a meeting point. The parties may have different understandings of custody. The player should confirm who holds the item at each stage and what happens if the meeting is cancelled.

## 324. Offer branch — Storage is limited while the deal is pending

A large or fragile item may need temporary storage. The storage owner confirms capacity, conditions, and custody. The holder can keep it, place it in approved storage, or withdraw the offer. The requester can accept a later handoff, find another location, or cancel. No one should leave property in an unowned public room because a quest dialogue says it is reserved.

If a faction offers storage, state whether it is a fee-based service, a loan of space, or part of a barter arrangement. The faction can refuse items outside its rules. The owner decides what happens if the agreement expires. If the current inventory system cannot represent temporary custody, postpone or cancel rather than duplicate the item in a local cache.

## 325. Settlement branch — Both sides must move or neither does

The exchange is ready only when each party has approved the final terms and the owner confirms both sides are available. Settlement must be atomic or otherwise recoverable: if one transfer fails, the other must not remain stranded. The player sees a preview of quantities, recipients, and any supported favor. Each participant can make a final confirmation or cancel.

This behavior is a prerequisite rather than a narrative fiction. If the current transaction APIs can move only one side at a time without rollback, do not label peer barter complete. Keep the route as an offer or referral and report the technical blocker. A story scene cannot guarantee restoration after reload. Once settlement is supported, capture the canonical result and allow the inventory owner to remain the sole property authority.

## 326. Settlement branch — One participant withdraws before confirmation

The participant can change their mind before the transaction commits. The player cancels through the barter owner and confirms that no property moved. The other participant may ask why, accept the withdrawal, propose different terms, or end the exchange. The withdrawing person does not need to provide a personal reason.

If a reservation was made, the owner releases it or states why it cannot yet be released. A public listing can be updated through its owner. Do not charge a cancellation penalty unless the terms and owner explicitly support it. The other party may lose time, which is a real but bounded cost. The player can help arrange another meeting without implying that consent has become irrevocable.

## 327. Settlement branch — A partial callback arrives after one side moved

The transaction owner may return an error after an item moved on one side. The system must either roll back that transfer or place both parties in a clearly recoverable state controlled by the same owner. The player should not have to manually invent compensation through dialogue. The owner reports what moved, what remains pending, and who currently holds each item.

Until the repair path is proven, this branch blocks implementation of a completed barter loop. A test should cover the failure after the first transfer and a reload after recovery, but this plan does not ask for speculative code or broad tests. The existing transaction contract and focused verification must establish the behavior before authored content promises atomic exchange.

## 328. Dispute branch — The item differs from what the parties discussed

After delivery, the requester may report that the item’s condition or quantity differs from the accepted offer. The player asks what was promised, what the owner recorded, and what arrived. The holder can acknowledge, contest, or explain an honest mix-up. The transaction owner determines whether the transfer can be reversed, amended, or left final.

Do not let dialogue directly seize the item from the recipient. A remedy requires the property owner and both parties’ consent or a current rule that authorizes it. The requester can keep the item, return it, negotiate a replacement, or seek review. The holder can offer a correction or decline. A supporting faction that witnessed the meeting can provide facts without deciding the dispute.

## 329. Dispute branch — A promised favor is not performed

The recipient may say that a specific favor was part of the trade and the other party did not perform it. The player checks the recorded terms, due time, and task owner’s state. The other party may have completed it, been prevented by a changed duty, misunderstood the scope, or withdrawn. The affected participant can ask for a new date, renegotiate, release the obligation, or request review.

If the favor was only spoken informally and no supported record exists, say that the game cannot prove its exact terms. The characters may still discuss what each remembers, but do not create a hidden contract retroactively. The barter plan needs a narrow owner-backed favor mechanism before it promises enforceable work. An unresolved disagreement can end the branch without automatically making either party a liar.

## 330. Dispute branch — A participant requests a neutral review

The parties may disagree about whether the transfer matched the offer. The player can identify a current review owner, an agreed witness, or the transaction records that exist. Both participants can consent to a review, choose separate contacts, or stop. The reviewer’s authority and scope must be disclosed before the review begins.

A faction broker who hosted the trade may know what happened but may also have an interest in keeping its service trusted. The participants can still choose that review with the conflict disclosed, ask for another, or rely on the transaction record. If there is no review owner, do not invent an arbitration court. The possible endings are a negotiated remedy, a factual clarification, a referral, or an unresolved dispute.

## 331. Dispute branch — The participants want separate accounts

Each person may tell the player a different version and ask that it not be shared. The player explains that a review may require specific evidence and that private accounts cannot automatically become public facts. Each party can authorize a summary, provide a limited detail, ask for a separate meeting, or decline.

The player can compare claims without pretending to know the truth. The transaction owner’s record, witness account, and item state may confirm some facts while leaving others uncertain. Do not broadcast a personal accusation on the barter board. If a conduct owner exists and evidence meets its rules, the player can refer the matter. Otherwise, parties decide whether to continue trading with each other.

## 332. Dispute branch — A mistake repeats across several exchanges

Repeated mismatches may show a process problem: ambiguous listings, poor item descriptions, confusing quantities, or a broker who fails to confirm availability. The player can ask participants for examples and the transaction owner whether a shared format can help. Individuals can change how they offer goods, ask for confirmation, use a witness, or avoid the service.

Do not turn a small sample into a universal rating. If a faction broker controls a service, it may review its process and publish a correction. Participants can still choose that broker or use a private route. Repeated bad experiences can shape authored trust dialogue only through supported facts and current social owners, not through a new global reliability meter.

## 333. Dispute branch — The holder refuses a remedy

The holder may decline to replace or return an item after a mismatch claim. The requester can accept the final state, seek review, offer a different settlement, or stop trading with that person. The holder can explain their position or end the discussion. The property owner and any review authority determine what remedies are actually available.

The player cannot force a transfer through a social response option. If the owner has a rule for reversing an invalid transaction, invoke it; otherwise, let the parties decide what they will do. The requester’s decision may affect this relationship, but should not remove all future access to the whole peer network. A supporting faction can witness or offer a separate future trade without declaring a verdict.

## 334. Dispute branch — A participant asks to remove their listing after a dispute

Either participant may want a public listing removed or corrected. The message or offer owner determines whether removal is supported and whether a record of the original offer remains. The person can request privacy, update availability, or leave the listing visible with revised terms. The other party can ask to keep a record for review, but cannot demand public access to private details.

If the trade itself remains unresolved, removing a listing does not erase the transaction history required by the owner. The player should explain which information is public and which is retained for operational reasons. Do not promise deletion beyond what the current owner provides. The dispute can continue through a private review or end as a personal boundary.

## 335. Dispute branch — A participant apologizes without offering a remedy

The holder may apologize for confusion but be unable to replace the item. The requester can accept the apology, ask for a specific remedy, continue the exchange, or end contact. The holder may express regret without being forced into a new obligation. The player helps distinguish repair of the relationship from settlement of property.

If the requester accepts the apology but keeps the item, the transaction state remains what the owner records. If they return it, both parties confirm and the property owner executes the movement. If no action is possible, the game can close the conversation with unresolved material harm. An apology is meaningful but does not automatically reverse cost.

## 336. Dispute branch — The dispute reaches a major faction

A participant may ask a major faction to intervene because it controls a broader service or rule. The player checks whether the faction has jurisdiction over the private exchange. It can offer a mediation route, explain that it has no authority, or decline involvement. The participant can accept the referral, ask a supporting group, or keep the matter between the parties.

Do not let a major faction absorb the peer transaction owner. If it controls only a shared marketplace or allocation, its authority applies to that resource and not automatically to private property. Its decision can affect access to its own service, with scope made clear. The player can navigate between institutions while each remains bounded.

## 337. Dispute outcomes — Remedies must follow the recorded agreement

Possible outcomes include correction, replacement, return, partial settlement, a future favor, a review, a private boundary, a withdrawn listing, a continued trade relationship, or no remedy. Each depends on what was agreed, what the owners record, and what participants now consent to. A truthful unresolved ending is better than a fabricated justice score.

When outcomes affect future offers, carry only supported facts such as a completed transaction, open review, or explicit agreement. Do not make one dispute automatically lock every faction or contact route. Residents can choose to trade again, avoid a person, use an intermediary, or leave peer barter entirely.

## 338. Installment 24 close — A dispute is not proof of a universal character type

The branch can show generosity, caution, negotiation, withdrawal, and accountability in the same character. One fair trade does not make a person permanently trustworthy; one error does not make them a villain. The player learns from specific exchanges, source confirmations, follow-through, and willingness to communicate. The current social owner may represent relationship changes, while the transaction owner retains property facts.

This supports endings based on action: the parties settle, separate, seek review, disclose uncertainty, or decide not to trade. A faction can offer a bounded mediation service or decline to intervene. None of these choices need a good/evil meter. The story should let the cost of an exchange remain visible even when no perfect remedy exists.

## 339. Expansion installment 25 — The player can choose how much to participate

The player may broker an introduction, compare terms, inspect an owner-confirmed item, coordinate a meeting, witness a transfer, or leave the participants to negotiate directly. These are distinct playstyles with different information and responsibility. Before a player takes a role, the participants can say what help they want. The player should not become an uninvited negotiator who pressures the holder or requester into a preferred outcome.

The player’s intervention can reduce confusion, but it can also expose private information or make a participant feel cornered. Each person can ask the player to stop, switch to an intermediary, or continue alone. If the player is responsible for an official resource decision, that formal role is separate from informal brokerage. The player should not use shelter authority to bias a private trade unless a current owner’s rule applies.

## 340. Player-role branch — The player helps compare two offers

The requester may ask the player to compare a neighbor’s offer with a faction service. The player can list quantities, timing, condition, fees, return terms, privacy, and uncertainty. The requester chooses what matters most. The faction and neighbor confirm their own offer; the player cannot claim either one remains available until its owner verifies it.

The comparison should not declare one route objectively better through a hidden score. One may be faster but disclose more information; another may be cheaper but less certain; a third may preserve independence but require travel. The requester can accept one, ask for a revision, wait, or take none. The player’s skill comes from exposing tradeoffs rather than choosing the “correct” faction.

## 341. Player-role branch — The player is asked to recommend a price

The participants may ask the player what a fair exchange would be. The player can provide a verified reference if the relevant owner publishes one, describe comparable offers, or admit that no reliable basis exists. The parties can use that information, ignore it, or request a neutral appraisal. A recommendation does not bind either side.

If the player personally controls the public distribution rule, they must disclose that separate interest. A private peer trade should not be priced by shelter authority without a governing rule. A supporting faction may provide a market board or recent transaction data, but the data source and limits should be clear. The participants set their terms and can walk away.

## 342. Player-role branch — The player is asked to carry property

One party may ask the player to hold an item until the other arrives. The property owner and custody rules determine whether that is allowed. The player can accept through an existing transfer route, ask for a neutral store, or decline. Both participants must know who will hold the item and how it returns if the trade fails.

Do not make the player a temporary inventory authority through quest dialogue. If the current system cannot track custody, the player should facilitate a direct handoff or postpone. If custody is supported, record the source, recipient, and release condition through the current owner. A faction can provide storage only under its own explicit rules. The player can choose to take on the practical risk when that choice is real and supported.

## 343. Player-role branch — The player leaves the deal to the participants

The participants may prefer not to have the player involved. The player can provide the confirmed contact route and step away. They can still check the task owner if safety or property ownership requires it, but should not keep asking for updates. The trade may complete, fail, or remain private without the player knowing every detail.

This ending should not be treated as a missed quest. It reflects respect for agency and can open a later callback only if a participant chooses to share the outcome. The game need not expose the full contract to the player if they were not a party. A supporting faction may act as the chosen broker instead, with its role visible to the participants.

## 344. Player-role branch — The player’s own need creates a conflict

The player may want the same item they are helping another survivor obtain. The transaction owner can show availability, but the player’s role as broker creates an apparent conflict. The player can disclose their interest and step aside, proceed only after the other parties agree, or withdraw from the trade. A neutral contact may take over the introduction.

Do not let the player secretly prioritize themselves while appearing neutral. If the game supports player inventory transfer, settlement uses that owner like any other transaction. The parties can decline a deal involving the player. This branch gives the player a consequential choice about fairness without moralizing the resource need.

## 345. Player-role branch — The player is asked to speak for a faction

A faction contact may ask the player to present its offer as a trusted intermediary. The player can accept, ask the contact to deliver it directly, disclose their role, or refuse. If the player accepts, they may transmit only the approved terms. They cannot promise a concession or guarantee acceptance unless authorized.

The recipient can ask for direct confirmation from the faction. The player can provide the contact route or say that it is unavailable. If the faction later changes terms, the player relays the update and does not pretend the original offer remains valid. The player’s relationship with the faction can affect access to information, but the recipient still decides.

## 346. Player-role outcomes — A useful broker knows when to stop

The player’s involvement may end with a comparison, a confirmed introduction, a witnessed meeting, a direct handoff, a referral, a disclosed conflict, or a deliberate step back. The exact role should appear in the result. Do not award a generic “broker” status for every successful connection. The participants’ transaction remains their own if the player only introduced them.

These branches let players support trade through diplomacy, logistical planning, price research, mediation, or non-intervention. Each has a cost and limit. The plan’s central test remains whether the transaction owner can settle property truthfully and whether both participants knowingly agreed.

## 347. Installment 25 close — Participation should not erase participant ownership

The player can make trade easier without becoming the owner of every decision. Each participant maintains control of their offer and property. A broker or faction can help only within a defined role. The transaction authority records settlement; the social authority owns relationship consequences; communication owns audience and listings. No one route should turn an offer into an automatic transfer.

The resulting endings remain varied: direct deal, revised terms, a safer intermediary, a neutral comparison, an accepted favor, a failed settlement, or no trade. Their difference comes from what each person wants and what they are willing to exchange, not from an alignment score.

## 348. Expansion installment 26 — A completed trade changes the next decision

Once an exchange settles, each participant may use the item, pass it onward through a new trade, store it, repair it, or decide that it did not meet the need. The property owner records the actual current holder. A completed transfer does not guarantee a useful result. The requester may discover that the item solves only part of the task; the holder may need a substitute for their own plan. The player can help both people identify the next owner without reopening the original deal automatically.

The barter owner should close the transaction at settlement and preserve only facts needed for its supported history. Follow-up is a new branch with fresh consent. A person can be satisfied, disappointed, or silent. Do not infer approval from the fact that no complaint was filed. A later task can mention the item only if the current inventory or task owner confirms its use.

## 349. After-trade branch — The requester uses the item successfully

The requester may return to say that the item helped with a task. They can thank the holder publicly, privately, or not at all. The holder can accept thanks, ask that their name remain private, or say that the exchange is complete. The player can connect the result to the task owner if that system benefits from the information.

Do not make one successful outcome prove that the offer was fair in every respect. The transaction remains as agreed; any favor or future relationship is separate. The requester may choose to recommend the holder, but the holder can decline public listings. A later resident may ask about availability, which must be confirmed again rather than inferred from a past trade.

## 350. After-trade branch — The item fails during intended use

The requester may report that the item broke or did not perform as expected. The player asks what use was agreed, what the owner recorded about condition, and what happened. The task owner determines whether a hazard or replacement is needed. The holder can share known history, offer help, dispute the claim, or ask to stop contact.

The barter owner handles only remedies its contract supports. If there was no warranty or condition promise, do not invent one after the fact. The parties can agree to a voluntary replacement or no further action. A safety incident follows its existing route. The failure can still affect how each person chooses future exchanges without adding an unsupported global quality score.

## 351. After-trade branch — The holder misses the item later

The holder may realize that they gave away an item they now need. They can ask whether the requester is willing to trade it back, offer a new exchange, or accept that the transfer is final. The requester may be using it, may be willing to return it, or may decline. The property owner confirms current custody before a second transaction.

The player should not reverse the deal automatically because the holder regrets it. The regret can matter to dialogue and future caution, but consent remains bilateral. The holder can seek a substitute from a supporting faction or another contact. The requester can offer a different item or no help. This callback lets trade produce emotional consequence without changing the original settlement record.

## 352. After-trade branch — The item becomes scarce after transfer

A supply shortage can make a previously ordinary item more valuable. The owner reports current availability. The former holder may ask to repurchase it; the current owner may keep it, lend it if supported, trade it, or gift it. The player can compare current offers but cannot impose a new price on a completed private transaction.

If a major faction now controls distribution, it can set rules for its own stock, not automatically reclaim property already transferred. A separate emergency rule may exist, but it must be verified. The participants can decide whether to help each other during scarcity. Their decision can change future access and tone without being reduced to a permanent generosity trait.

## 353. After-trade branch — A participant wants to refer another buyer

The requester may introduce a third person to the holder. The holder chooses whether to be contacted and which information may be shared. The requester can pass a public listing, make a named introduction, or keep the contact private. The third person negotiates separately and does not inherit the first exchange’s terms.

The holder may ask not to receive repeated inquiries. The communication owner can remove or narrow the listing if supported. A supporting faction may host the next exchange, with its own conditions. The player should not expose a private shortage or imply that the holder still has stock. Each referral is a new consent point.

## 354. After-trade branch — A participant asks for a reference

Another resident may ask whether the first trade went well. The previous participant can provide a factual account, an opinion, a limited private answer, or no reference. The player confirms whether the person wants their identity shared. A broker can report only transactions it actually observed.

Do not convert a positive reference into a guarantee. The recipient can decide how much weight to give it and ask for current condition and availability. A negative account can be challenged or contextualized; it should not automatically blacklist anyone. If a conduct or transaction review exists, use that owner for verified findings. Personal impressions remain attributed as impressions.

## 355. After-trade branch — Participants ask for a repeat arrangement

Two people may want to trade regularly or reserve future goods. The current owner checks whether recurring offers or reservations are supported. Each person can agree to a single next exchange, propose a time-limited arrangement, or decline. The player states what is confirmed and what still depends on future availability.

Do not reserve property that the holder has not yet received. A recurring route must revalidate consent, stock, and task needs at each supported commitment. If the system handles only single transactions, keep the relationship in dialogue and create a fresh offer each time. The participant can end the pattern without an exit penalty unless explicit terms say otherwise.

## 356. After-trade branch — A participant wants no further contact

After settlement or dispute, one person may ask not to receive future offers from the other. The communication or social owner determines whether a contact restriction can be recorded. The person can choose a limited boundary, ask for an intermediary for operational matters, or decline to formalize it. The other party receives only what is needed to respect the request.

Do not block the person from every peer trade route by default. Shared duties may require separate communication through an owner. The boundary can be revisited if the participant asks, but not because the player wants reconciliation. If no durable preference state exists, do not promise persistent filtering. The scene can still honor a present request through the current conversation.

## 357. After-trade outcomes — Later consequences stay attached to current facts

The post-trade route can show successful use, a defect, regret, changed scarcity, an authorized referral, a reference, a repeat offer, or a contact boundary. The player should see whether property still belongs to the person who holds it and whether a new offer was actually made. Any operational task follows its own owner.

These outcomes can create distinct endings and callbacks without treating barter as a reputation economy. A resident can have one successful and one poor exchange. A faction can help with later access without owning the private transaction. The system should preserve the transaction’s actual terms and avoid carrying forward opinions as universal truth.

## 358. Installment 26 close — Settlement is an ending and a beginning only by consent

The initial trade ends when its owner confirms the transfer. What follows is optional: gratitude, complaint, a second offer, a referral, an apology, or silence. Each follow-up requires the current participant and relevant owner. The story can show lasting consequence without leaving every exchange permanently open.

For implementation, avoid an indefinite “trade relationship” state unless the social owner already supports it. Use a new offer, task, or communication for each new commitment. That keeps item custody truthful and lets participants set fresh terms after circumstances change.

## 359. Final route check — Verify both property movement and follow-up

Before calling a transaction complete, confirm the participants accepted the same terms, the transaction owner moved both sides or recovered cleanly, and the property owner reports the expected holders. After settlement, close the original offer. A later complaint, favor, referral, or repeat trade is a separate action with fresh consent and current availability.

If atomic movement, rollback, or save recovery is not supported, keep the plan at proposal level and do not claim that a narrative choice completed a trade. The player should see which item moved, who owns it now, and which obligations remain open only if the owner can represent those facts.

## 360. Plan 5 continuation close — Trust is built from specific exchanges

The expansion now follows trade from discovery through negotiation, inspection, settlement, dispute, brokerage, and later use. Participants can be generous, cautious, mistaken, or unavailable in different situations. Factions can provide limited access, storage, appraisal, or mediation when supported, while the player remains the central negotiator and each property owner retains authority.

Before implementation, verify one transaction owner, bilateral atomic settlement, rollback, and save restoration. Do not add a pooled treasury, universal price score, or global trust meter. Preserve the original three Section 4 subfeatures; the additional endings elaborate offers, settlement, and disputes without creating parallel systems.

## 361. Secondary expansion installment 27 — A trade route must be usable by both participants

The exchange may be formally bilateral while practically difficult for one party to complete. A meeting place may be too far away, the item may be hard to carry, the offer text may be difficult to read, or the participant may need a quieter way to negotiate. The player can ask what would make the exchange workable, then check the relevant location, inventory, and communication owners. Each participant can request a change, accept an alternative, or leave.

Accessibility and convenience affect whether consent is meaningful. The player should not assume that a person can meet at a public counter, carry a heavy object, or read a dense contract. The holder may agree to a different handoff or decline because the alternative is too costly. The requester can accept that limitation or seek another contact. No one is required to explain a private condition in front of the other party.

## 362. Access branch — The meeting point is difficult to reach

One participant may not be able to reach the proposed location. The location owner can identify an accessible alternate, authorize a delivery, offer an intermediary, or state that no alternate exists. The parties can agree to a new site, choose a supported handoff, or cancel. The player should not mark the participant as unreliable for declining an inaccessible meeting.

Changing the location can affect safety, privacy, and custody. The owner confirms access and who may observe. If a faction hosts the alternate site, its rules and any fee are disclosed. A public space may be convenient but unsuitable for confidential terms; a private room may require permission. The transaction owner confirms the handoff path before property moves.

## 363. Access branch — The item cannot be carried by the requester

The requester may need help transporting an item. The holder can offer delivery, a smaller quantity, temporary storage, or no assistance. The requester can provide an authorized helper, schedule a different transfer, or decline. The task or inventory owner confirms whether transport consumes a resource or requires a qualified worker.

Do not silently debit fuel, labor, or cart capacity through a dialogue choice. If delivery has a real cost, show it before acceptance. The helper is a separate participant whose time and consent matter. If no transport route exists, the exchange may remain an offer and expire. A faction can provide a vehicle or runner only under its confirmed terms.

## 364. Access branch — The offer terms need a different format

A participant may ask for the offer read aloud, summarized, translated, or given privately. The communication owner and both participants confirm which format is available. The summary preserves quantity, condition, return terms, time, and uncertainty. The receiver can ask questions, request the full wording, or decline.

Do not hide important conditions inside a condensed version. If a term cannot be translated accurately, the parties can wait for help or choose another exchange. The translator does not become a witness or guarantor unless everyone agrees. A later dispute should use the version both participants understood, where the owner can establish it.

## 365. Access branch — A participant needs more time to decide

The requester may want to think before accepting. The holder can keep the offer open for a stated period, withdraw it, or offer a different deadline. The transaction owner records an expiry only if supported. The requester can accept, ask for an extension, or let the offer close. The player should not frame deliberation as ingratitude.

If another person is waiting for the item, the holder can explain the competing demand without disclosing unnecessary private information. The requester can decide with that context. A deadline should be a real capacity condition, not manufactured urgency. Once the offer expires, the holder must confirm availability again before making a new one.

## 366. Access branch — One party wants a support person present

The participant may ask a trusted person to sit in, help read the offer, or witness the handoff. The other party can accept, request a different arrangement, or decline. The support person agrees to a limited role and does not negotiate beyond the participant’s authorization. The player can clarify whether the person may see item details or only the terms.

If the support person belongs to a faction, disclose that connection. The participant can choose someone independent or proceed alone. A witness can report what they saw but does not guarantee quality or fairness. If either party is uncomfortable, the exchange can move to a public service owner or end without blame.

## 367. Access outcomes — A workable exchange can still be declined

The route can end with a different location, delivery, translated terms, extended deadline, support person, a failed accommodation, or cancellation. An accommodation makes a decision possible; it does not obligate either party to trade. The player should not treat completion as the only positive outcome.

If an access preference is not stored in the current communication or trade owner, ask again later. Do not add a global trade-profile system. Keep any durable transaction terms within the transaction owner, and personal preferences within their existing authority.

## 368. Secondary expansion installment 28 — Unequal need can sharpen negotiation without erasing choice

One participant may need the item urgently while the other can wait. The difference creates leverage and risk, but the story should show specific behavior instead of assigning a permanent exploitative or generous identity. The player can clarify how urgent the need is, whether another route exists, and which terms the holder is actually offering. The requester can accept, negotiate, seek a public allocation, ask a mediator, or walk away.

The holder may respond with a fair exchange, a high demand, a gift, a refusal, or a request to wait. Current law or emergency allocation rules, if applicable, come from their own owners. Barter itself does not decide what is fair. The player can challenge coercive pressure through a supported conduct route, but should not override property movement through moral dialogue alone.

## 369. Unequal-need branch — The requester cannot offer an equivalent good

The requester may have no tradable item that matches the holder’s request. They can offer a smaller contribution, ask for a gift, offer a supported service, seek another provider, or stop. The holder decides whether any alternative is acceptable. The player can help identify a service owner and make the labor scope explicit.

Need should not automatically generate an obligation to work. If the holder proposes labor, the requester can ask what task, duration, risk, and supervision it involves. The task owner confirms eligibility and safety. The requester can refuse without losing access to unrelated shelter services. A faction can provide aid under its rules, but those terms are disclosed separately.

## 370. Unequal-need branch — The holder asks for a return favor much later

The holder may ask for help at an unspecified future time. The requester can ask for a concrete task and deadline, accept only a narrow favor, offer another term, or decline. If the barter owner cannot store open-ended obligations, do not claim the favor is enforceable. The participants can record a clear task through an existing route or make the transfer a gift.

An undefined favor can create dramatic tension, but the interface should identify it as uncertain rather than a guaranteed debt. If the requester later declines a vague demand, the holder can express disappointment without repossessing the item automatically. Any actual property remedy must follow a current owner and contract. The player can mediate the misunderstanding or let the parties settle their relationship.

## 371. Unequal-need branch — The holder raises the price after learning urgency

The holder may change terms after the requester reveals how urgent the need is. The requester can accept the new price, challenge the change, ask for an intermediary, or leave. The holder can retain the revised offer or withdraw. The player should show the original terms and the change where the transaction owner supports that history.

If the new demand involves prohibited work, coercion, or shared resources, route it to the proper owner. Otherwise, allow the parties to decide whether they accept the trade. A supporting faction may offer an alternative, but should not guarantee a better price unless its owner confirms. The requester’s choice can be desperate and still be voluntary; the story can portray pressure without pretending there is no agency.

## 372. Unequal-need branch — The requester discovers a public service after negotiating

The requester may learn that a public allocation, repair, or loan route exists. They can compare that service with the peer offer and decide whether to proceed. The holder may release the item, keep the offer open, or ask for a decision by a stated time. The public owner confirms eligibility and availability.

Do not penalize either participant because an alternative exists. The requester can value speed, privacy, or direct trust more than a public service, provided terms are clear. They can also cancel the peer offer without owing compensation unless an explicit contract says otherwise. The player can help them make an informed comparison rather than steering them to a factional answer.

## 373. Unequal-need branch — A mediator questions whether the terms are understood

The participant may ask for a neutral person to read back the terms. The mediator repeats quantities, condition, delivery, expiry, and any favor without recommending a choice. Each party can correct the summary, accept it, or stop. The transaction owner confirms the final contract.

The mediator may notice an ambiguity but cannot rewrite it unilaterally. The parties can clarify, renegotiate, or let the offer expire. If there is no supported witness role, treat the conversation as a one-time clarification and do not promise a durable record. A faction may supply a mediator, with affiliation and interests disclosed.

## 374. Unequal-need branch — The requester accepts but asks to revisit after one use

The parties may agree to a trial use or staged transfer. The owner confirms whether staged settlement is supported. The requester can take a small quantity first, test it under a safe owner-approved condition, then accept or decline the remainder. The holder agrees to keep the remaining amount available only if the owner supports that reservation.

Do not represent the first transfer as a complete deal if terms remain open. The interface must show what has moved and what is pending. If staged barter is unsupported, use a single smaller trade or wait for a verified trial mechanism. The participants can still choose to negotiate informally, but the plan cannot promise system-level escrow without an owner.

## 375. Unequal-need branch — The item is essential to a shared service

The holder may offer an item that is personally owned but currently needed by a shared service. The relevant task owner reports the impact; the holder decides whether to proceed, delay, or withdraw. If the item is actually assigned to shared stock, the property owner determines whether it can be transferred. A requester may choose another route after learning the consequence.

Do not use the phrase “shelter needs it” as an automatic override. The authority and rule must be visible. A supporting faction may lend a replacement, allowing the holder to trade, but only if the loan is confirmed. The final outcome can preserve the shared task, allow the trade, or leave the requester without the item. The choice is grounded in actual ownership and capacity.

## 376. Unequal-need outcomes — The plan surfaces leverage and keeps each boundary

This route may end in a gift, a clearly priced trade, a service agreement, a refusal, an alternative public route, a mediated clarification, a trial quantity, or a protected shared resource. The player can advocate, negotiate, seek another source, or step back. No option is automatically righteous because it produces the most resources.

Before implementation, verify which current owners represent public stock, private property, task commitments, and transaction settlement. If a policy is absent, mark the governance issue. Do not implement a hidden fairness calculator or dynamic price score as a shortcut.

## 377. Secondary review — The same participant can change roles from one trade to the next

A resident may be a giver in one exchange, a requester in another, an intermediary in a third, and a person who declines to participate later. The authored branches should not infer a stable class of “buyers” or “sellers.” Current capacity and consent are checked for each offer. Any relationship history is local context and must come from the existing social owner.

This avoids a feedback loop where past generosity makes a resident permanently available for requests or past refusal locks them out of future trade. A faction representative can vary their response based on current stock, duty, and rules. The player can ask again when the circumstances change but should respect a present no.

## 378. Secondary review — No exchange should be silently netted against a previous one

If the participants made two separate trades, each transaction should have its own terms and settlement. The player may notice that one person owes an item from a prior exchange, but that obligation exists only if an owner-backed record says so. Do not automatically combine quantities or cancel one side against another because the narrative remembers both.

Separate transactions make reversals, save recovery, and disputes understandable. The barter owner can present linked references if it supports them. Otherwise, the player can resolve each separately. A participant can offer a new trade to settle an old disagreement, but both sides confirm it as a new commitment.

## 379. Secondary review — Settlement language matches system evidence

Writers should use “offer made,” “terms accepted,” “transfer started,” and “trade completed” only when the corresponding owner state exists. If the UI cannot distinguish a committed offer from an informal conversation, the dialogue should not claim one. If rollback fails or restore is unproven, the feature remains blocked at proposal stage.

Use focused verification for the transaction path once implementation is approved: both sides move, one side failure rolls back, save/load preserves final custody, and a duplicate confirmation does not repeat the transfer. These are acceptance risks because the story’s promise depends on them, not because the plan should add speculative tests.

## 380. Plan 5 secondary close — A fair offer is legible before it is accepted

The secondary pass adds accessible negotiation, time to decide, support people, uneven need, labor boundaries, staged offers, and current shared-service constraints. The participant can accept a narrow bargain, use another service, ask for a mediator, or walk away. The holder retains control of property within the current owner’s rules.

The plan stays within its original three subfeatures. It does not add a price score, pooled resources, persistent barter profile, or informal court. The player’s deeper role comes from exposing terms and finding valid routes; each participant decides whether the exchange is worth its concrete cost.

## 381. Secondary expansion installment 29 — Several offers for one item create a choice, not an auction by default

Two residents may approach the same holder before an offer is settled. The holder can hear both, choose one, propose a sequence, or withdraw the item. The player can help explain that the first conversation was exploratory if no commitment was made. The property owner confirms current availability, while the transaction owner determines whether any offer is reserved.

Do not automatically convert the situation into a highest-price auction. The holder may care about timing, use, privacy, or a specific relationship. They can state their preference or keep it private. Each requester can make a revised offer, use another route, or leave. The outcome should reflect the terms the holder actually accepts, not a hidden fairness ranking.

## 382. Multiple-offer branch — A tentative promise conflicts with a later offer

The holder may have said “I can probably spare it” to one person before a second request arrives. The player asks whether this was a confirmed offer, a reservation, or an informal possibility. The holder can honor the first offer, clarify that no commitment existed, or ask both parties whether they will consider a shared solution.

The first requester can accept the clarification, ask for review, or withdraw. The second requester should not be told the item is available until the holder confirms. If the transaction owner cannot represent tentative reservations, keep that distinction in dialogue and require fresh confirmation. Do not punish either requester for relying on ambiguous wording.

## 383. Multiple-offer branch — The holder wants to divide the available quantity

The holder may have enough for both requesters if the item can be split. The inventory owner confirms quantity and transfer precision. Each requester states whether a partial amount is useful. The holder can divide, choose one recipient, or keep the item. The player cannot assume that equal portions are fair or operationally useful.

If one participant needs a minimum amount, they can explain it, use a separate service, or decline. The other may accept a smaller quantity or request a different item. Any split settles through the same atomic transaction owner. If partial settlement is unsupported, do not conduct two sequential transfers and call them one fair division.

## 384. Multiple-offer branch — One requester asks the holder to keep the decision private

The requester may prefer not to know who else asked or how the holder compares offers. The holder can make a private decision, share only the result, or ask for a transparent process. The player should not disclose another resident’s need without consent. Each requester can be told whether an offer is accepted, declined, or still pending.

Privacy may reduce the chance to negotiate, but the requester can choose that tradeoff. A faction broker can carry offers separately if it agrees and has the relevant route. It must not promise confidentiality it cannot provide. If the holder chooses one offer, the other person receives a clear response without private comparison details.

## 385. Multiple-offer branch — A faction’s public stock and a private offer overlap

A faction may list an item while a resident privately offers a similar one. The player checks which owner holds each item, whether the public listing is current, and what conditions differ. The requester can compare price, timing, transport, privacy, and source. The faction can update its stock or decline to match a private offer.

Do not let a faction claim a resident’s item because it is the same type of good. Nor should a private holder be described as competing unfairly with a public service. The two offers remain separate. The requester can accept one, ask for a revision, wait, or take neither. The property owner confirms the actual source of the item.

## 386. Sequence branch — Participants want to complete two related trades

Two residents may each offer an item the other needs. They can negotiate a single reciprocal exchange or two independent transactions. The transaction owner determines whether multi-party settlement is supported. The player can display both sets of terms and ask whether each person wants all-or-nothing closure, staged confirmation, or separate deals.

If one trade fails, the other should not remain completed by accident when the agreement was conditional. If they are independent, settle and record each separately. The parties must confirm that structure before movement. Do not create a barter bundle in dialogue without an owner that can atomically settle it.

## 387. Sequence branch — A participant needs a restock before delivering

The holder may offer goods they expect to receive later. The player checks whether the item is currently owned or merely anticipated. The requester can wait, accept only after stock arrives, choose a substitute, or decline. The holder may ask for a future reservation, but the transaction owner must support that state before it is promised.

A faction can provide a restock estimate only from its current source. The player should distinguish expected, shipped, received, and transferable. If the delivery is delayed, the holder re-confirms the offer. The requester does not owe a response to a good that never arrived. This branch is especially important where trade meets the shelter’s canonical merchant-restock owner.

## 388. Sequence branch — A task consumes the item before the trade settles

An operational task may use or reserve the item while the offer is pending. The property owner reports the change. The holder can withdraw, offer a remaining quantity, or ask the task owner whether a substitute exists. The requester can wait, accept the partial offer, or stop. The author updates any public listing only after availability is confirmed.

Do not settle against stale inventory. The task’s claim has precedence only if the current owner’s rules say so; the player should not invent a general hierarchy. If an offer was already accepted and the property moved, use the transaction’s agreed terms and owner remedy. A new task cannot silently reclaim the requester’s property.

## 389. Sequence branch — The requester’s need disappears before delivery

The requester may solve the problem through repair, a public allocation, or another neighbor. They can cancel the pending offer, keep it if still useful, or ask to redirect it to someone else. The holder can agree, keep the item, or make a new offer to the third party. The owner confirms whether anything was reserved or moved.

The player should not assume that an accepted offer remains desired indefinitely. If settlement already completed, a return is a new transaction unless the original terms included a supported condition. If it is still pending, release reservations through the owner. The participants can close amicably or disagree about time spent; no hidden cancellation penalty applies.

## 390. Sequence branch — A third party offers to pay the obligation

Someone else may want to contribute an item or service on behalf of the requester. The requester, holder, third party, and any task owner clarify who receives what and who owes any agreed favor. Each participant consents separately. The barter owner determines whether third-party payment is supported.

Do not route property through the player as a workaround. If the owner supports a direct multi-party transaction, show the full movement and terms. If it does not, the third party can make a separate gift or offer, and the requester can negotiate again. The holder may decline a payment from someone they do not know.

## 391. Sequence branch — A family member objects to the exchange

A family member may rely on or share the item. The property owner determines whether the holder can transfer it alone. The player can ask whether consent is required and what the object is used for. The holder can seek approval, offer personal property, or stop. The family member can agree, decline, or request a substitute.

Do not treat family status as automatic co-ownership or automatic veto; use the current property authority. If the objection is relational rather than legal, the holder can still choose but may face a real conversation. The requester should not be placed between family members without permission. Any later settlement follows the item’s recorded owner.

## 392. Sequence branch — A participant faces a changed duty or emergency

The participant may need to leave before the handoff. The task owner confirms the new duty, and the participant chooses whether to reschedule, use a permitted intermediary, or cancel. The other party can wait, propose another time, or withdraw. If an emergency rule governs the property, the responsible owner explains its scope.

Do not automatically blame a participant for prioritizing an assigned duty. At the same time, the other party’s time matters. A later offer needs fresh confirmation. The player can choose to wait, facilitate a handoff, or find a substitute route; each creates a different cost. The trade remains pending only if the transaction owner supports that state.

## 393. Sequence branch — The transaction is interrupted by a save or reload boundary

If the game can save during an offer or settlement, the current owner must define what happens to pending terms and partially completed work. On restore, the player should see whether the offer remains open, expired, accepted, cancelled, or settled. No duplicate transfer should occur from repeating a confirmation after load.

If pending transactions are not persisted, close or cancel them before save where the current contract requires it, and do not promise continuity. The plan must not create a panel cache to make the interaction look durable. Focused save verification is part of the implementation acceptance because item ownership is central to the feature.

## 394. Sequence branch — A participant asks to revise the record after settlement

The participant may say the summary omitted a condition or misspelled an item description. The transaction owner determines whether a non-economic annotation can be corrected without changing property state. Both parties can approve a factual amendment, dispute it, or leave the original record. The player should not edit a settlement to fabricate a different agreement.

If the correction affects quantity, custody, or favor terms, use an actual remedy or new transaction. If it affects only public wording, the communication owner can update the listing while preserving the transaction truth. Each audience receives only the level of detail the owner allows.

## 395. Sequence outcomes — Availability, terms, and custody are revalidated at each transition

The route may end with a private selection among offers, a split quantity, two atomic exchanges, a delayed restock, a cancelled need, third-party contribution, changed duty, a restored pending offer, or a factual record correction. Each transition rechecks what the owner says is current. A saved offer is not a saved promise of item availability.

This secondary addition links peer barter to the restock, task, and save owners without duplicating them. The player can coordinate several people and still keep every transfer separate and truthful. If the current transaction owner cannot represent a proposed sequence, use one simpler transaction or defer the feature.

## 396. Installment 29 close — A changing situation requires renewed consent

The holder’s stock, requester’s need, participant’s availability, and faction offer can all change before settlement. The player must surface those changes and ask whether terms still stand. A single confirmation should not authorize every later variation. Participants can revise, pause, or cancel while the current owner releases any supported reservations.

The branch ends when the parties settle current terms or end the offer—not when the player has exhausted every possible alternative. A quiet cancellation can be correct. The property owner reports custody, the transaction owner reports settlement state, and the social owner handles any relationship consequence.

## 397. Secondary review — Separate a negotiation aid from a price authority

The plan can show comparables, public stock, item condition, travel, urgency, and personal preferences. None of those automatically produces a canonical fair price. If a current market owner publishes prices, label its scope and freshness. Otherwise, the parties negotiate without a numeric recommendation from the game.

This keeps a player’s strategy open: accept a known reference, rely on a personal relationship, ask for a second offer, or walk away. A faction can advocate its own terms, but its quote does not become a universal value. Any later price mechanic needs its own design and authority decision.

## 398. Secondary review — Do not promise future availability from a past exchange

A previous trade can show that two people communicated successfully once. It does not prove current stock, willingness, qualification, or consent. The player may ask a past contact again, but the holder confirms a new offer. A public referral requires permission and a current availability check.

This avoids stale-contact quests that repeatedly send players to empty inventories. If the holder is unavailable, the contact can say so without a negative relationship outcome. A supporting faction can provide another route, with its own current terms.

## 399. Plan 5 secondary close — Every new exchange starts with current facts

The plan now includes accessibility, unequal need, competing offers, staged decisions, stock changes, third-party contributions, save boundaries, and post-settlement record review. These branches deepen negotiation while preserving property ownership and bilateral consent. The player has room to broker, compare, disclose, wait, or withdraw.

Implementation remains dependent on one atomic transaction owner, accurate item custody, supported cancellation and save behavior, and current integration with public stock. Keep the three original feature pillars and the plan’s proposal-only status. No local inventory cache or universal trust/pricing score is introduced.

## 400. Secondary coda — A participant can return after declining without being treated as inconsistent

The requester may decline an offer and later ask whether it remains available. The holder checks current stock and willingness; the requester confirms that the need and terms still apply. The player can restate the original offer only as historical context, then allow a fresh negotiation. A changed mind is not a breach when no agreement was made.

If the item has moved or the holder no longer wants to trade, say so plainly. The requester can accept another source, make a new offer, wait, or stop. The holder does not have to reopen the first offer, and the requester does not owe an explanation for returning. This branch makes refusal and reconsideration compatible with a living trade network.

## 401. Secondary coda — A completed trade should not auto-enroll either person in future contact

After settlement, each participant can decide whether to receive another offer or remain listed as a contact. The communication owner controls any durable preference. If no opt-in list exists, do not promise automatic notices. A participant can still be introduced again with fresh permission, or ask that the player stop carrying offers between them.

One successful exchange is evidence that one transaction completed. It is not blanket consent to future negotiation, publicity, or recurring service. The next contact begins with current availability and a new choice. That boundary completes the plan’s secondary edit on transaction aftermath.

## 402. Secondary coda — A referral is not a guarantee of fairness

The referrer can say why a contact may help and identify what they personally observed. The requester decides whether to rely on that account. The holder or broker confirms current terms. If the referral came from a faction, its relationship and any incentive should be visible where relevant.

The player can use referrals to widen options without turning them into endorsements. A later mismatch follows the dispute path and current evidence; it does not retroactively prove that the referrer acted maliciously. This leaves the network useful while keeping each participant responsible for their own offer and acceptance.

## 403. Secondary coda — Close the offer when the owner confirms its end

After a trade completes, expires, or is cancelled, remove the active offer through its current owner if that route exists. A stale display can invite a second requester to rely on unavailable goods. If the owner cannot retract every copy, mark what remains uncertain and provide the current contact route. Closing a listing does not erase the transaction record or decide any later dispute.

If a participant asks to reopen the listing, verify current stock and terms first. The prior transaction gives context, not a standing offer. Each new requester receives a fresh confirmation.

## 404. Expansion installment 30 — Peer trade meets the shelter’s operational forecast

A proposed transfer may change whether a task team can meet its next assignment. The item owner confirms current custody, and the task owner reports any actual reservation or dependency. The player can show the requester that the item is in demand, seek a substitute, ask the holder to wait, or proceed if the owner confirms it is available. A forecast is information; it is not a hidden veto unless the owner’s rules say so.

The holder can decide that their private plan matters more than the projected need. A requester can accept delay or choose another source. A faction may provide a replacement and preserve both routes, but its stock and terms must be confirmed. The branch keeps private barter connected to operations without inventing a shadow allocation system.

## Operational branch — A later task reserves the offered item

The holder may make an offer before the task owner has reserved the item, then receive a confirmed task assignment. The player checks whether the offer was exploratory or accepted and whether transfer began. The holder can honor a committed trade if allowed, renegotiate with the requester, or ask the transaction owner whether cancellation is valid. The task owner cannot silently reclaim property that already transferred.

If the offer was only tentative, the holder can withdraw with a clear explanation. The requester can accept a delay, seek a substitute, or stop. The owner’s rule governs shared stock; personal property follows its actual owner. The game should not resolve the conflict by assuming that shelter operations always outrank private commitments.

## Operational branch — The request itself creates a new task dependency

The requester may plan to use the item for a repair or delivery. The task owner can confirm that the item is suitable and whether the task depends on it. The requester can disclose only the necessary purpose, ask for a general substitute, or keep their personal reason private. The holder can decide whether that context changes the offer.

The trade owner records the transfer; the task owner records task progress. If the item is not used as planned, no task outcome should be inferred from the transaction alone. If it is used, the task owner verifies the result. This separation supports callbacks without making barter a task authority.

## Operational branch — The item is returned from a task damaged

A resident may want to trade an item that was recently used or repaired. The current property owner confirms its condition and who may transfer it. The task owner can report known wear without exposing private operational details. The requester may accept it as-is, ask for a repair, request a different item, or decline.

The holder can disclose what they know, but should not be required to guarantee hidden defects. If the item is unsafe, the safety or maintenance owner determines whether transfer for use is allowed. A faction technician can inspect only under an accepted service. Any repair cost is agreed before the next exchange; it is not silently added after the original trade.

## Operational branch — A peer offer conflicts with a public ration or allocation rule

The parties may offer an item that is also covered by a public distribution policy. The player identifies whether it is private property, a shared allocation, or an item the policy restricts. The relevant authority decides what transfers are allowed. The participants can proceed within the permitted scope, seek an exception if one exists, or stop.

Do not imply that every private trade is exempt from a public rule, or that every public rule claims private property. Show the actual owner and policy. If the boundary is not defined in current evidence, mark the issue for governance review and do not settle the transfer in the proposal. A major faction may explain its policy but cannot silently resolve a shelter ownership gap.

## Operational branch — A recipient wants the item earmarked for someone else

The requester may intend to give the item to a third person. They can disclose the end recipient, keep the purpose private, or ask that the item be delivered directly. The holder decides whether that matters. The property owner checks whether the transfer path supports a third-party recipient. The third person has no obligation to accept the item.

If the item is meant to fulfill another person’s request, confirm that need is still current. Do not let the requester make an offer in another resident’s name without permission. The transaction may go directly to the recipient, proceed between the original two parties, or stop. Any favor or social meaning remains separate from the property transfer.

## Operational branch — A faction’s public stock estimate differs from the peer offer

The faction may list a public quantity that appears to compete with a resident’s private offer. The player checks timestamps and actual stock. The faction can correct its listing, state that its amount is reserved, or confirm current availability. The private holder may keep, revise, or withdraw their offer.

Do not accuse either source of deception from a mismatch alone. One may be stale, reserved, or scoped to another location. The relevant owner resolves its own record. The requester can wait for confirmation, take the peer offer, choose the faction route, or seek a third source. This branch makes information reliability matter without creating a global market authority.

## Operational branch — Transfer would break a shared item set

Several items may function as a set for a work group. The item owner can confirm whether pieces may be transferred separately and whether a task depends on the full set. The holder can offer all, part, or none; the requester can ask for a set or accept a substitute. The task owner confirms any effect on work.

Do not model set integrity through a UI note alone if the simulation depends on it. Use current owner data or keep the dependency as a narrative warning. If the set cannot be split, explain why and offer a route to another source. A faction may lend the missing piece, but lending requires its own supported custody contract.

## Operational branch — The parties disagree about who should bear a failed task cost

If the requester uses the item and a task fails, they may blame the holder. The player checks what the holder represented, what the task owner required, and what the item owner can confirm. The task owner handles task consequences; barter handles only its recorded terms. The participants can negotiate a new remedy, seek review, or end contact.

Do not automatically charge the holder for a failed task unless the accepted terms and owner support that outcome. Conversely, a holder who knowingly misrepresented a required condition may face an existing review route. The requester can report facts without claiming a verdict. A supporting faction witness can add evidence within its role.

## Operational outcomes — A trade can change an option without determining the task

The operational branch can end with a task-safe transfer, a deferred offer, a substitute source, an allowed private transfer, an unresolved property boundary, a third-party delivery, a corrected stock notice, or a review. The trade changes what the participants possess; the task owner determines what work follows.

The player sees how private choices intersect with shelter needs while retaining the right to negotiate. If the current inventory or task owners cannot express a proposed dependency, keep the callback authored and avoid a mechanical claim. No parallel stock forecast or property ledger is introduced.

## 416. Expansion installment 31 — Information exchanged between participants has limits too

People may offer access, maps, instructions, location knowledge, or introductions alongside physical goods. The player should ask whether the information is public, personally sourced, restricted by a faction, or about another resident. A transfer of knowledge can create privacy or safety consequences even when no item moves. The participants can share a public source, provide a bounded summary, seek authorization, or decline.

Do not treat personal information as an ordinary barter good. A person cannot sell someone else’s private history or consent on their behalf. The source owner determines whether a restricted document can be copied. A faction can grant access to its own information under disclosed terms. The recipient may use what they learn but should know its source and uncertainty.

## Information branch — A participant offers a map or route description

The map owner confirms whether it is current and what areas it includes. A participant may share a public route, a personal observation, or a restricted facility plan. The recipient can ask about age and hazards, accept the information, verify it with a location owner, or decline. The player does not make an old route safe merely by showing it in a trade scene.

If the route affects travel or safety, the current navigation or location owner remains authoritative. The map can be a useful lead rather than a guarantee. A faction may provide its route under a condition; disclose who can see the copy and when it may expire. If the map contains private locations, seek the relevant owner’s permission before copying.

## Information branch — A participant trades a lesson or demonstration

The parties may agree to teach a skill in exchange for an item or service. The education and task owners confirm qualifications, time, and safety. The learner can accept a one-time lesson, ask whether a future favor is involved, or decline. The teacher can limit the subject and refuse to certify a result they are not authorized to assess.

The barter transaction does not award proficiency automatically. The education owner records a supported lesson, and any qualification decision belongs to its consumer. If the participants only exchange advice, describe it as informal. A faction-hosted course can have a separate fee and audience condition. The learner can accept the teaching without joining the faction if its terms allow.

## Information branch — The information is about a third person

One participant may offer to share another resident’s schedule, need, location, or history. The player checks whether the information is authorized and necessary. The third person can consent to a bounded disclosure, refuse, or use a direct route. The trade may continue with a general description, a public source, or no information.

Do not let a favor or item payment convert privacy into transferable property. If immediate safety depends on sharing, use the relevant owner’s existing rules. Otherwise, protect the third person’s audience boundary. A faction representative can provide general service availability without identifying who asked for help.

## Information branch — A lead turns out to be wrong

The requester may discover that a contact or route did not exist. The original participant can correct the information, explain uncertainty, offer another lead, or deny responsibility. The player can compare what was stated with what the source owner confirms. The parties may negotiate a new exchange or close the matter.

If the lead was explicitly uncertain, do not portray its failure as fraud. If it was presented as verified and was knowingly false, use the current transaction or conduct review. The recipient can ask for a remedy, but no automatic item return occurs without owner support. The story should distinguish a bad outcome from a dishonest source.

## Information outcomes — Provenance and consent travel with the knowledge

The information route can end in a confirmed map, a tentative lead, an authorized lesson, a private detail withheld, a restricted source referral, or a corrected claim. The recipient learns who supplied it, what they know, and what remains uncertain. The original source keeps its authority where one exists.

These branches extend the offer and dispute features without creating an information marketplace. They provide more ways to negotiate and more reasons to verify before acting. The player can accept, question, protect privacy, or stop.

## 417. Plan 5 scaffold — Remaining expansion sequence

The following sequence is a writing and evidence scaffold for continuing Plan 5 toward its 120,000-word minimum. It extends the existing offer, settlement, and dispute pillars. Each entry should become full branching prose before closeout; this outline is not counted as implementation evidence.

### Installment 32 — Trade after a roster transition

Track what happens to open offers when a participant leaves, changes duty, or moves. Branch on verified handoff, cancellation, property return, and no contact. **Evidence:** roster owner, pending transaction persistence, and custody rules.

### Installment 33 — Borrow, loan, and return contracts

Develop temporary use, damage, late return, extensions, and retrieval. **Hard gate:** do not promise loans until one owner stores custody and restore state; no panel-only due dates.

### Installment 34 — Trade with a household or shared group

Explore who may offer common property, how dissenting co-users respond, and what happens when the group has no recognized representative. **Evidence:** actual ownership and consent APIs.

### Installment 35 — Favor terms and work obligations

Build bounded labor exchanges, cancellation, hazard changes, completion evidence, and disputes. **Evidence:** task owner, schedule, qualification, and any existing favor contract.

### Installment 36 — Market references and uncertainty

Compare public price references, local scarcity, condition, travel, and personal value without introducing a global price score. **Evidence:** current data source and freshness.

### Installment 37 — Trade under rationing or emergency rules

Separate private property, public stock, essential allocation, and emergency authority. Branch on allowed trade, exception request, alternative supply, and blocked transfer. **Evidence:** signed policy or current owner API; otherwise report a governance gap.

### Installment 38 — Inspection, repair, and item provenance

Expand condition checks, uncertain ownership, repair estimates, and chain of custody. **Evidence:** inventory metadata and maintenance owner.

### Installment 39 — Multi-party agreements

Consider pooled contributions, group purchases, staged transfers, and one participant withdrawing. **Hard gate:** atomic settlement and rollback across all participants; do not simulate a pool in dialogue.

### Installment 40 — Trade listings and audience controls

Explore public listings, private referrals, anonymous offers, expired copies, and removal requests. **Evidence:** message and barter listing lifecycle; no duplicate registry.

### Installment 41 — Dispute evidence and remedies

Develop item mismatch, missing delivery, unclear favor, witness conflict, and unresolved review. **Evidence:** transaction log and actual reviewer authority.

### Installment 42 — Cross-faction brokerage

Compare a major faction market, a supporting group’s local exchange, and a direct peer offer. Each source states fees, stock, reach, and limits. No faction becomes a universal market.

### Installment 43 — Personal boundaries after a bad trade

Branch on no further contact, mediated contact, a new offer, public correction, and continued shared work. **Evidence:** social owner and privacy route.

### Installment 44 — The requester’s changing need

Explore urgency, substitute found, task cancelled, and item repurposed before settlement. Recheck availability and consent at every state change.

### Installment 45 — Accessibility and travel costs

Develop handoff location, carrying help, language, timing, and private support person. **Evidence:** location access and transfer route.

### Installment 46 — Trade failure and save restoration

Specify focused scenarios for partial transfer, duplicate submit, reload during pending offer, rollback, and final custody. No authored success branch until the transaction owner proves recovery.

### Installment 47 — Post-trade callbacks

Show later item use, regret, repair, referrals, and scarcity without inferring blanket trust. Use only owner-backed custody or authored dialogue with clear audience.

### Installment 48 — Playstyle and ending matrix

Compare direct negotiator, cautious verifier, faction broker, privacy advocate, generous participant, and player who steps back. Each ending lists the concrete action, cost, owner result, and future option.

### Installment 49 — Content continuity review

Cross-check item IDs, faction names, location access, task callbacks, and existing quests. Remove any branch that contradicts current data authority.

### Installment 50 — Plan 5 closeout criteria

Close the planning document only after it exceeds 120,000 measured words, preserves the three subfeatures, has evidence notes for settlement and save behavior, and clearly states what remains unimplemented.

## 418. Scaffold realization — Installment 32: a roster transition during negotiation

A peer exchange is a conversation between people who may become unavailable while terms are still being discussed. A survivor can change shifts, leave the shelter, become ill, or simply refuse to continue. The design has to distinguish an offer that has not been accepted from a transaction already being settled. It must also distinguish someone's presence from ownership: a roster transition changes who can participate now; it does not automatically transfer their belongings to the player or another survivor.

### Negotiation status is explicit

Before any handoff begins, an offer may be proposed, revised, withdrawn, accepted, expired, or refused. These states are not interchangeable. A proposal does not reserve items unless its owner deliberately implements reservation. A revision supersedes only the terms the participants have accepted as current. A refusal closes the current offer, while a counteroffer creates a new set of terms. An expiry follows the stated deadline and does not punish either party. A player who reloads after a pending offer sees the same offer identity and exact current state, rather than a newly rolled opportunity.

When a party becomes unavailable, the player sees the status and only the reason supplied by the roster or narrative owner. They may wait, withdraw, ask the party whether they want a representative, or close the negotiation. Representation is not implied by kinship, faction membership, co-residence, or an old friendship. If a party has explicitly named a delegate through a real system, the delegate may continue within the stated scope. Otherwise the trade pauses or expires according to its terms.

### Branches that preserve each person's ownership

If a party leaves while still holding their item, the item remains with that party's owner record or container. The trade cannot complete by treating the departure as a free inventory transfer. If they had offered a favor rather than property, the favor does not become a task assigned to whoever is still present. If both sides explicitly agreed to a delegated exchange, the delegate can execute only the agreed action and only after the transaction owner verifies both participants' standing consent. A new item, quantity, due date, or recipient requires renewed confirmation.

An unavailable party can produce several valid endings. The player can cancel and return any *reserved* items through the owner. They can hold the offer open when a supported deadline and save path exist. They can renegotiate with the remaining participant only for property that participant controls. They can ask a named representative to confirm the existing terms, if an actual delegation record exists. They can walk away and leave the relationship unchanged except for specific communication facts. The system never chooses the most convenient party to keep the quest alive.

### A concrete scenario: the half-finished repair exchange

One resident offers a spare fitting in exchange for a repaired tool. The tool's repair is still a promise: no task completion event has occurred. The fitting has not been moved. Before acceptance, the offeror becomes unavailable. The player can ask the other resident who requested the tool whether they still want to negotiate, but they cannot accept the absent person's terms on their behalf. They can withdraw the proposal, wait for the offer to expire, or make a new offer with a different owner-controlled item. If the tool repair later completes, that completion does not retroactively accept the fitting exchange.

If the original offeror had set up an explicit delegate, the interface shows the delegate's authority: “deliver the fitting at the agreed quantity” rather than “make any deal.” The settlement owner validates the exact item and ownership. If the delegate proposes a different exchange, it returns to negotiation with the original party when available. No dialogue option can bypass this contract.

### Supporting faction as safe custodian

A small broker, stores team, or other canon-supported helper might hold an item temporarily only if a current inventory/custody owner supports that transfer. The helper's role is a service: label and protect a package, keep a receipt, or confirm a delivery window. They do not become the item's owner and do not guarantee completion of a trade. If a custody service is not present in the game, this route remains a design option rather than a reason to create a parallel escrow ledger.

Where a service exists, its terms are concrete: who placed the item, which transaction it is associated with, what condition permits release, and how it is returned if the transaction closes. The player can accept the service fee or delay, choose a direct handoff, or cancel. A custodian's refusal due to capacity is a valid branch. The system must not show “held safely” while the item remains in an unrelated container without a persistent reference.

### Acceptance trace

Review at least: party departs before acceptance; party departs after a supported reservation; a delegated action remains inside its permission; a delegate proposes changed terms; a favor offerer becomes unavailable; and a trade is canceled after a reload. Each trace must demonstrate that ownership remains intact, no party's consent is inherited by another, reservations return once, and no item is duplicated or lost. This is a planning acceptance set; implementation must be verified against the actual transaction and save owners.

## 419. Scaffold realization — Installment 33: loans, borrowing, and honest return terms

Not every peer exchange should transfer ownership. A loan can express a different social contract: the borrower can use an item for a defined task or period, while the lender expects it back. The user interface should say whether an item is sold, gifted, borrowed, or held temporarily. A “trade” that silently changes permanent ownership when one participant expected a loan is a content failure even if inventory counts balance.

### A loan is not a cheap barter variant

A loan request identifies the item, current owner, intended use, borrower, return condition, due event or date, and any wear risk known to the current system. If the game has no item-condition owner, the plan must not promise that every loan returns in the same state. It can still author a no-wear loan or treat wear as an unresolved prerequisite. Terms need an owner decision about custody during the loan: does the item move to the borrower's inventory, remain checked out from a shared stock owner, or exist as a reference? The proposal cannot mix those models.

The lender may accept, decline, or ask for a narrower term. The borrower may accept, counter with a different return time, or decline. Both sides must confirm a material change. A player can provide context, suggest an alternative item, offer a direct transfer instead, or step away. Relationship is not a substitute for ownership validation or consent. A refusal does not trigger a hidden affinity penalty.

### Return conditions create useful branches

The item can be returned when the task ends, at a named day, at a location, or when another item becomes available. A deadline should be an owner-backed event where possible; otherwise use an explicitly approximate date and let the player see the uncertainty. “Return it when finished” is ambiguous if task completion can fail, be interrupted, or be reassigned. A better contract defines what counts as finished and what the borrower should do if the work stops.

If the borrower completes the task, they can return the item, renew the loan with permission, or offer to trade for it. The system should not auto-convert an overdue loan into a sale. If the task is interrupted, they can return the item early, request an extension, or keep using it only when the lender previously authorized that case. If the borrower leaves, the loan pauses or follows an explicitly defined retrieval path; property does not teleport to the lender. If the lender withdraws consent, the borrower is given a safe return route and the task may pause.

### Branches born from earlier actions

A player who returned a previous loan on time can be offered a direct renewal conversation; the specific successful return supports that history. A player who returned an item late may be asked for a firm deadline or a task milestone before a new loan. This does not globally mark them untrustworthy. A player who disclosed that a task failed before the due date may retain a cooperative route because the event is truthful and useful. A player who falsely marked the item returned should face a record correction if the owner can prove custody; the narrative can address that actual discrepancy.

The borrower's own history also matters. If they kept a tool through a repair outage because the lender agreed to that condition, the next loan can refer to the shared contract. If they exceeded scope and used a personal keepsake for another task, the person may refuse future loans of that item specifically. Avoid a broad exclusion flag that disables all peer trade unless the existing reputation owner actually models it and the design has evidence for that breadth.

### Example: the survey lamp

A resident asks to borrow a lamp for a route inspection. The owner can lend it until the inspection returns, ask for it to be returned before evening duty, decline, or offer a different route that does not require the lamp. The player sees who owns it, whether another task has already reserved it, and what return event is expected. A small map-making or maintenance group could identify a safe inspection window, but it cannot authorize the owner's property. If a second resident needs the lamp first, the player can change the schedule through the task owner, negotiate a shorter loan, or refuse both requests and preserve the current commitment.

On completion, the task producer emits the actual event. The borrower may return the lamp at the named location, request an extension because the return route is unavailable, or keep it until an explicitly authorized milestone. The later quest text can mention whether the lamp was returned, but the item state is authoritative. If the task fails, there is no success line saying “the lamp came back” unless the return action actually happened.

### Dispute paths and humane handling

When return does not happen as agreed, first distinguish a missed deadline from a confirmed refusal. The system can present: “due, no return recorded,” “borrower asked for more time,” “borrower unavailable,” or “item not found,” according to owner facts. The player can send one reminder, ask for an explanation, accept a new deadline, request an authorized helper retrieve the item, or close the request unresolved. Retrieval cannot be offered unless a current owner and consent rule permit it. A player may decide that the item is no longer worth pursuing; that closes the social request but does not falsify inventory custody.

Do not require a confrontation quest to get the item back. Do not automatically reduce trust every day it is overdue. If a trust change is approved and owned by the existing reputation system, attach it to one verified, clearly defined outcome and apply it once. A late-but-honest return, a broken promise, a missing item, and an explicit refusal are distinct situations. They can lead to different dialogue and future options without moral alignment scoring.

## 420. Scaffold realization — Installment 34: household and shared-group exchanges

Some items are held or used collectively. A household may share blankets; a work group may own tools; a survivor may have personal custody of one object while contributing its use to communal tasks. The feature must not assume that every item has one simple individual owner, nor use “shared” as a license for the player to take it. A household exchange needs a clear ownership or custody authority before it can be offered.

### Make the represented party legible

The offer must name the party capable of consenting. If one survivor speaks for a group, show whether they are the authorized representative and how that authority is known. If the existing data has no representation model, a group offer cannot be accepted as if one resident's dialogue binds everyone. The plan should instead route the player to an existing group decision or narrow the transaction to property the speaker personally controls.

For shared stock, the inventory or communal allocation owner remains responsible for availability and transfer. SurvivorBarterSystem can hold the social terms, but it must not shadow the count. If several members depend on an item, a trade may require a cost display: the number of current tasks that will lose access, the next scheduled use, or a simple “currently assigned to workshop queue,” whichever facts the owner actually provides. Do not fabricate a complete dependency tree from item descriptions.

### Three group structures and their distinct branches

**Household-use group:** Several residents rely on an object but one named person owns it. The owner can offer it, and the group can raise a concern only through a defined communication route. The player may negotiate timing, find a substitute, or decline. Consent to lending by the owner does not mean every affected person is happy; the system can present the known effect without requiring unanimous agreement unless policy says so.

**Work-crew stock:** An item is part of a task or shared tool pool. It cannot be traded peer-to-peer while allocated unless the asset owner supports a release. The player can request release, wait, or find another source. A crew representative can explain priority but cannot waive the stock owner’s availability rule.

**Unclear or disputed custody:** Two people describe the same item as theirs, or the record has no clear owner. The right branch is a pause and fact-finding route. The player can seek a receipt, ask the current custodian, or cancel. The design must not reward the player for choosing the more sympathetic claimant. A supporting group can provide evidence only if it has the relevant record.

### Negotiating a group benefit

A group may offer an item in return for a contribution: a repair shift, training time, transport help, or access to a shared space. That is not a free item with decorative dialogue. The obligation needs a clear owner, recipient, due condition, and completion event. If no task or roster owner can represent it, the proposal should phrase it as non-persistent narrative context or avoid it. A contribution should not become a hidden labor tax on one unconsenting member.

The player can accept the full obligation, negotiate a smaller contribution, offer an alternative item, or refuse. A group can counter only when the offer model supports revision. The player sees who will perform the contribution; the offer cannot say “the crew will handle it” when no authorized crew action exists. If a group decision requires discussion, provide the supported response route and time cost. Do not create multiple synthetic votes just to make the interaction feel systemic.

### Example: the shared handcart

A work group controls a handcart used on alternating shifts. A resident asks to borrow it for personal transport in exchange for repairing a damaged wheel. The player must learn whether the cart is shared equipment, who owns its schedule, and whether the repair is already assigned. If it is currently allocated, the player may request a later slot, ask for another carrier, or decline. If the repair is a genuine task, the resident can offer to take it; the roster and task owners then determine eligibility and consent. If the player promises the repair on their behalf, the promise is invalid unless they own that action and have permission to assign it.

The outcome can be a timed loan, a crew-approved repair exchange, a declined request, or an unresolved request after a schedule conflict. Each ending changes the next playable options. A timed loan creates a return obligation. A repair exchange creates a real task and a peer offer. A decline leaves the cart with its current owner. An unresolved request may remain on the board only if a supported expiry and identity path exist. No branch creates a general “cart faction” or additional system.

### Content continuity rules

Use group names, resident IDs, and item identities from current catalogs. Sample items such as a lamp, handcart, fitting, blanket, tool, and family keepsake are illustrative and must be checked for canonical availability before authored implementation. A generic case may use current item IDs and can be rewritten when data evidence changes. Group dialogue must reflect its actual authority. A crew cannot grant personal property; an individual cannot dissolve a communal reservation; a broker cannot guarantee repair capacity; a household member cannot consent for an absent owner without an explicit delegation.

This installment's review set covers: single-person ownership under shared use; true shared inventory; disputed custody; unauthorized representation; group benefit with an actual task completion event; and a group refusal. It asks whether every acceptance can be traced to one responsible authority and whether each participant has a truthful route to decline or renegotiate.

## 421. Scaffold realization — Installment 35: favors as obligations with a named scope

A favor is not an item, and it is not a currency. It is a bounded promise between participants. If the player offers “help later,” the recipient cannot know what was agreed, and the system cannot tell when it has been fulfilled. This installment turns favors into legible terms while preserving their human flexibility: a person may ask for a concrete contribution, revise the request, or decide that they no longer need it.

### Minimum contract

Every persistent favor needs: who owes it; who may receive it; the agreed scope; the trigger or due condition; any material cost; whether the offer is accepted; and what event proves completion. If one of these cannot be represented, do not persist the favor as an obligation. Keep it as an ordinary line of dialogue with no promise of later system behavior. Avoid vague obligations such as “do something for me,” “be there when it matters,” or “make it right” unless an existing quest/task owner already defines a meaningful resolution.

A scoped favor can still feel personal. “Take the late tool return so I can finish the clinic queue,” “show me the repaired latch when the crew clears it,” or “walk the route with me next time” carries voice and context. The scope says what was offered; the writing says why it matters. The system should not translate emotional warmth into an invisible numeric balance.

### Negotiation branches

The recipient can accept, decline, ask for a different scope, or ask what the player means. The player can offer a smaller favor, replace it with an item, suggest a task route, or withdraw before acceptance. Once accepted, materially changed terms require confirmation from both parties. If a due date shifts because the underlying task is delayed, the person who receives the favor may be asked to approve a new date; it does not silently move.

The player can also ask whether the person wants a favor at all. A resident may prefer direct compensation, a loan, or no exchange. This option prevents every relationship from being framed as reciprocal debt. If the person declines a favor, their refusal ends that route; the player can continue with another transaction only by starting a new offer.

### Fulfillment and partial completion

Fulfilling a favor requires a real completion source. A rostered shift is proved by the duty owner, a delivered item by inventory transfer, and a completed repair by task owner. A spoken promise to do work is not completion. Partial progress can be shown only if the task producer has valid milestones and both sides understand what partial fulfillment means. If the player finishes the underlying task but not the promised help, the favor remains due. If the requested beneficiary no longer needs it, the recipient can close it through an explicit action.

An overdue obligation creates options rather than an automatic punishment: send a reminder, explain a blocker, request revised terms, perform it now, or close the favor unresolved. The recipient may accept a new deadline, narrow the request, or decline further exchange. If the existing reputation owner applies a consequence, it should be based on one verified event and run once. Repeating a daily tick must not deduct trust every day by default. A delayed fulfillment and an intentional refusal are not the same outcome.

### Supporting group contribution

A support faction can fulfill part of a favor when its real service matches the scope—for example, a workshop may make a tool available, a courier may carry an item, or a roster owner may confirm shift completion. That contribution does not make the player personally responsible for an action they did not perform, unless the original terms allowed delegated completion and the recipient accepted it. The helper's time or materials remain with their own owner.

Delegation needs explicit rules: who chose the helper, whether the recipient approved delegation, what the helper actually completed, and whether the original owing party retains responsibility. The safest default is that a favor promised by a person is completed by that person. Delegated work can be a negotiation branch, but it is not a free substitute. A helper can decline without harming their relationship to the favor recipient.

### Example: repair help in exchange for route knowledge

One survivor offers to repair a hand lamp if another walks them through a known route. The repair task has prerequisites; route knowledge may be a dialogue flag or merely authored content. The player must not promise the second participant's time. They can help the repair happen, ask whether the route guide wants to participate, offer a different exchange, or decline to broker it. If the route discussion is only narrative and has no completion authority, the exchange cannot be persisted as a favor that later affects trust. It can still be a one-scene agreement with a clear authored outcome.

If the route guide agrees, the requested route walk becomes a named task or a scheduled scene only if supported. If they decline, the player may ask another guide or cancel. If the lamp is repaired but the route walk never occurs, the interface should show the two facts separately; it cannot mark a balanced transaction complete. The parties can renegotiate, but the player should not choose for them.

### Review matrix

Test a completed favor; an overdue but honestly disclosed favor; a changed scope; partial work; a recipient canceling the need; a delegated helper refusing; a save/reload with the favor pending; and a legacy or missing task reference. Confirm there is one owner, one status, and one consequence per verified action. If those conditions are unavailable, retain the exchange as content and do not claim durable favor simulation.

## 422. Scaffold realization — Installment 36: prices are references, not authority

Peer barter can coexist with merchant markets and shared shelter trade only if price information is presented as context. An item value may be a market estimate, a replacement cost, a recent offer, or a participant's personal valuation. These figures do not establish ownership or compel consent. The player should understand how a reference was produced and what it cannot tell them.

### Label the kind of value

A price display should identify its source and age where the system has that data. A market listing is a seller's request. A current merchant quote is a quote for that transaction. A catalog value is a balancing reference. A barter history is a prior exchange, not a binding price. A personal keepsake may have no reliable exchange value. The UI should never display one of these as an objective “fair value” without an approved economy authority.

If an item has a current condition or quality owner, an estimate may account for it. If no condition owner exists, do not invent discounts or wear. If supply is unstable, show an estimate range or state that no recent quote exists. If the current API returns no price, an authored default is not a substitute. The player can still trade by agreement without a price label.

### Player choices and asymmetry

The player may accept the requested terms, make a counteroffer, request an estimate, compare a shared market listing, or decline. The other party can accept, counter, ask for a different item, or end negotiation. A counteroffer does not consume an item; reservation and settlement are separate. The player may hold more information than the counterparty or vice versa. The design can surface known differences without making one party omniscient.

A low estimate does not make the seller dishonest; a high ask does not establish greed. The player can ask why an item matters to the owner, but the owner may choose not to explain. They can offer an alternative with less personal cost, such as a timed loan instead of permanent transfer. This makes negotiation branch on the asset's role and the player's action, not a binary ethical score.

### Branch outcomes tied to prior exchanges

If the player previously completed a peer trade involving the same item class, the interface can show that transaction as a reference only if history is retained. A later participant may set a different value. If the player previously accepted a market quote without reading its conditions, the next screen can give clearer fees or scarcity information; it should not imply the player is foolish. If the player asked for an estimate and then chose a non-monetary exchange, the branch may preserve a more personal conversation. The player's preference may be displayed only if a current system stores it; otherwise the same set of choices remains available.

### Supporting group as source, not arbitrator

A small trader group can publish a current asking range or recent supply note. It cannot declare what a private item is “worth” to its owner. A stores team can report availability but not dictate peer terms. A repair group can estimate service time only from an approved task model. If support group information is stale, show the timestamp or omit it. Its presence helps players reason; it does not remove uncertainty.

The group may have incentives of its own, which can add texture if canon supports them: it may prefer standardized goods, avoid storing delicate items, or reserve stock for common needs. These limitations should be explicit and useful. The player can accept their reference, ask another source, or negotiate directly. No group gets a universal price-setting role simply because the barter plan needs an authority.

### Example: two batteries and one promise

A survivor offers a partially charged battery for a promise of help. If charge level is a real item condition, show it; otherwise call it a battery without asserting charge. The market reference indicates that batteries are scarce, but the survivor says they need this one for a portable radio. The player can propose a loan until a scheduled use, a different item, a concrete favor, or no deal. The survivor can refuse the battery loan because the radio use matters more than a price. A support trader may confirm that replacement stock is scarce; they cannot determine the survivor's needs.

If the player accepts a permanent exchange, the item owner verifies the transfer and only then does the transaction owner record completion. If the player counteroffers and the survivor declines, the current offer closes. If they agree to a favor, it follows the favor rules in Installment 35. If a loan is chosen, it follows the return rules in Installment 33. These connections create branching depth by routing to actual terms, not by multiplying counters.

### Review checks

Inspect a current quote, stale estimate, no-price item, personal keepsake, unequal valuations, market comparison, and a counteroffer after the original item becomes unavailable. The review asks whether the price source is explicit, whether a player can proceed without it, and whether transaction settlement revalidates current custody and availability.

## 423. Scaffold realization — Installment 37: rationing and emergency priorities

Scarcity can make a barter decision urgent, but emergency framing must be earned by a real owner fact. A trade involving medicine, water, batteries, or repair supplies may affect people beyond the two participants. The player should see the relevant shared allocation without being asked to adjudicate a person's worth. The plan does not add a new moral triage system or let the barter panel override existing rationing rules.

### Distinguish peer property from shared essentials

Before negotiating, identify whether the offered object is private property, an item held for another person, shared stock, or an allocated essential. A survivor can offer only what they own or are authorized to transfer. The player's role in shared stock remains with the current inventory/market owner. A peer trade cannot pull from communal reserves simply because the interface shows a matching item ID. If the item is already reserved for a shelter task, a release must follow that reservation's owner.

If the item is private but its exchange has communal consequences, show the owner-backed fact: a common task is waiting, a substitute exists, or the current reserve is allocated. The player can ask for a delay, offer another item, seek a shared-stock route, accept the risk of a task delay if authorized, or decline. The participant may be willing to trade even when the player chooses not to accept. A displayed public need does not erase individual consent.

### Emergency decision branches

An urgent request may support a faster exchange but cannot bypass revalidation. The player may agree to a temporary loan, ask a supporting group to coordinate a shared reserve, request a minimal quantity, exchange after the emergency passes, or decline. If the actual transaction owner cannot settle a loan atomically, the route remains conceptual. If existing rationing policy gives priority to a medical need, that policy must be applied by its own authority and visible to both parties.

The player may also choose to prioritize the peer request over a shared task. This should be an explicit action with a confirmed effect: a reservation is released or delayed, the dependent task updates, and the participant understands the terms. The game must not secretly strip a shared item to complete the trade. If no release action exists, the player can negotiate with the owner or choose another source; they cannot bypass it in the narrative.

### Avoiding a deservingness contest

The player's decision can account for urgency, consent, ownership, timing, and substitutes. It should not ask the player to rank people by faction, health label, or perceived moral worth unless a separate approved feature defines such a policy. If there are incompatible essential requests, each owner supplies valid operational constraints. The player can choose a transparent priority rule, seek a shared allocation decision, split a resource if the asset supports it, or leave a request unresolved. The system records the action taken, not a hidden virtue score.

The strongest branch may be to pause the trade and ask both parties to clarify whether the item can be loaned or divided. A participant may reject division because the object needs to remain intact. Another may prefer a short loan. A supporting group can explain what the shared stock owner permits. The player can preserve agency by presenting these choices honestly, even when every path costs someone time.

### Supporting group role during scarcity

A ration board team or stores crew can show current allocation and expected restock if that data exists. It can identify substitutes and process a release that the owner permits. It cannot promise an arrival date from an unverified caravan, nor give the player a secret discount on shared goods. Its supporting role helps expose constraints and procedural options. If the group declines a release because an item is allocated, the player can accept the delay, renegotiate with the peer, or choose another path.

When urgency is unknown, the player can ask the requestor or consult the relevant owner. The scene can represent uncertainty without withholding necessary facts artificially. If someone declines to share private medical detail, the player still receives whatever operational urgency policy permits. They are not entitled to a diagnosis to make a trade decision.

### Scenario and endings: the last filter

A resident offers the last privately held filter cartridge for a repair component. The shared reserve is allocated to a water task. The player can propose a loan that returns after repair, ask stores whether the reserved item can be released, find a non-filter repair approach, accept the private exchange and let the water task wait if policy permits, or walk away. The filter owner can decline any deal. The repair may be critical, but only the relevant owner can establish that. The player sees concrete effects and names the person who controls each choice.

Possible endings: a temporary loan and verified return; a permanent exchange with confirmed ownership transfer; an alternate repair method; a delayed water task after authorized reallocation; a declined exchange; or an unresolved request. Each ending has a different next option and no “hero” or “villain” label. A faction may help inspect the filter or explain allocation, but remains a supporting actor.

### Acceptance conditions

Verify that shared reserve cannot be spent through the peer path; a private owner can decline; an authorized release updates its source; an emergency qualifier is owner-backed; no diagnosis is disclosed unnecessarily; partial quantity is handled only if the item API supports it; and reload preserves the chosen allocation. The plan should not claim that scarcity is solved, only that the exchange tells the truth about who controls the item.

## 424. Scaffold realization — Installment 38: inspection, provenance, and contested condition

Participants may disagree about what an item is, where it came from, whether it works, or whether it is theirs to trade. Provenance can create compelling branches, but it can also turn every exchange into an evidence minigame. This installment limits inspection to situations where the fact changes a decision, and identifies the responsible source for each claim.

### Four claims that must stay distinct

**Identity:** which catalog item is present. **Custody:** who currently holds it or which container contains it. **Condition:** what state the item is in, if an owner models condition. **Provenance:** how it came to this holder, if an event history exists. A survivor's story about an item is a participant account; it is not automatically a transfer record. An inventory count proves quantity only according to that inventory owner. The player can make a choice with incomplete provenance when the remaining risk is acceptable.

Inspection options should name their reach. A visual check can identify a label but not ownership. A work crew can test function within its skill, but not prove who acquired it. A receipt can show a prior transfer but not that the current holder still wants to sell. A private history may not be available to the player. The interface must not collapse these into “verified.”

### Branching options

The player can accept the item as described; ask the owner to demonstrate a function; request a record check; negotiate a return clause; choose a lower-risk substitute; or decline pending more information. The other party can agree to inspection, set a privacy boundary, revise the price, or withdraw. If inspection could damage the item, that risk must be stated and owner-supported. An inspection refusal is not proof of fraud.

If the item is damaged or not as described, the trade may be renegotiated, canceled, or completed with explicit revised terms. Both parties confirm changed terms, and settlement uses the actual item state. A player who knew the uncertainty and accepted it should not receive a later surprise that the UI hid. A player who discovers a real discrepancy can pursue a remedy under Installment 41, but not by simply editing the original offer.

### Provenance can enable, but not force, a story branch

A known prior owner may connect an object to an earlier quest or personal goal. The player can ask about its history, preserve the item, trade it, or decline. The item's narrative meaning belongs to its current owner and established canon, not an assumed sentimental valuation. If the object is a personal keepsake controlled by PersonalBelongingsSystem, that authority governs custody and transfer. Peer barter may reference it only through a legitimate owner API.

The item's origin can unlock a dialogue option only if the history is truly known to the speaker. A survivor should not recognize an item from an unseen flag. If the item moved through multiple hands, the current owner may know only part of that chain. An incomplete history can remain incomplete. The player can still trade without solving its entire past.

### Supporting faction and specialized inspection

A repair crew, archivist, or broker may inspect a narrow property: function, catalog identity, or prior public custody entry. The group should state its confidence and limits. A repair team might report “the hinge still moves under load” but not “this will last a year.” An archivist might find a label in an old record but not authenticate the current item. A broker may provide a market comparator, not a moral verdict about the seller.

The service may cost time, consume a test material, or require an appointment where the system supports it. The player can skip it or use a different route. If the group is unavailable, the trade can remain unresolved rather than being auto-approved. Specialized support contributes to texture through expertise and limits.

### Example: the repaired radio shell

A resident offers a radio shell they say was repaired after a water leak. The player can ask the current radio or item owner whether the game models functional condition; ask for an inspection; make a lower-stakes exchange that does not depend on operation; accept with uncertainty stated; or decline. If a known repair task exists, its event can show that work occurred. It does not prove the radio works under every circumstance. A helper can inspect the unit if this is within current systems, and can only report their actual result.

If the player accepts the uncertainty, the settlement records the exact item and terms. If the inspection reveals a condition mismatch, a counteroffer or cancellation is possible. If the helper finds no issue, the player can accept; the result does not guarantee future performance. A radio quest may later use the item only if the consuming system supports it. This avoids promising a gameplay consumer that does not exist.

### Acceptance table

| Claim | Best evidence owner | What it establishes | What it does not establish |
|---|---|---|---|
| Item identity | Catalog/inventory | The item type and count | Current function or title |
| Current custody | Inventory/transfer | Where it is held now | Why it is held there |
| Work performed | Task producer | A task event occurred | Universal quality guarantee |
| Prior transfer | Trade/history owner | A recorded exchange | Current consent to sell |
| Participant intention | Current dialogue/consent | What they presently offer | A hidden motive |

Use this table as a narrative review aid, not a new evidence service. Confirm that each inspection option is backed by current systems before it appears in a build.

## 425. Scaffold realization — Installment 39: settlement is one contract across two hands

Settlement is the point where a negotiated promise becomes a durable transfer. It is also the sharpest technical seam in the plan. If each participant's property moves through independent callbacks, the system can produce a partial trade: one side has paid and the other side has not. A polished dialogue scene cannot repair that contract. This installment defines the desired transaction boundary and explicitly blocks implementation until one owner can provide it.

### Prepare, validate, commit, report

The transaction needs a single identity and a clear sequence. **Prepare** verifies that the accepted terms match the current proposal, both parties are still eligible, each item is still controlled by the stated owner, and any favor or loan terms have their own valid owner route. **Validate** checks that required sources and destinations can accept the movement and that no competing action has invalidated the offer. **Commit** transfers both sides as one atomic operation or transfers neither. **Report** emits one completed transaction fact for history, quests, notifications, and relationship consumers.

The exact internal mechanism belongs to the transaction and asset owners. This plan does not dictate a database transaction, generic ledger, or parallel escrow system. It requires observable all-or-nothing behavior. If the existing owner cannot guarantee it, the feature remains a proposal; do not chain gifts or compensate with a fragile sequence of inverse calls.

### Failure branches at every stage

Before prepare, either party may decline or withdraw. During prepare, an item may become unavailable, a party may depart, or an offer may expire. During validation, an inventory capacity limit may reject the destination. During commit, a process or save boundary may interrupt. After commit, a notification may fail. Each failure must identify what actually happened. A notification failure cannot roll back a completed transfer; a transfer failure cannot report a completed trade.

The player-facing result distinguishes: **not started**, **still pending**, **completed**, **canceled**, **expired**, and **failed without transfer**. “Partial” is not a valid successful status unless the participants explicitly agreed to partial settlement and the owner represents it as a separate contract. If an item moved before a failure, recovery must restore or finalize through the transaction owner; the UI should not offer the player an ad hoc retry that can duplicate value.

### Idempotent retries and duplicate input

Accepting the same offer twice—through double click, reopening a panel, or replaying an event—must not transfer items twice. A stable transaction identity supports idempotent processing if the current owner implements it. The UI can disable the commit control while a result is pending, but that is not the correctness guarantee. On retry, the owner returns the known transaction outcome instead of applying it again.

If the game crashes after commit but before the UI refreshes, reload must recover the completed transaction and show the right outcome. If it crashes before commit, the items remain with their prior owners. If it crashes during a storage operation, the transaction owner resolves the state before allowing another exchange. These requirements are acceptance constraints, not claims about the current implementation.

### Cross-plan callbacks

Only after a successful commit should the barter system emit the completed event. The task/quest owner may then update a journal or objective; the message system may notify participants; relationship logic may apply one approved consequence; a market view may refresh; and a personal belongings owner may update a selected keepsake if that exact transfer is supported. Each consumer handles its own projection. A callback failing should be recoverable without replaying settlement.

The event should carry stable IDs and the confirmed terms, not duplicate mutable item state. A consumer must not independently subtract the item count. If a downstream task needs to react, it reads the transaction fact or receives an immutable event. The existing event/save seam determines how this is achieved. The plan explicitly rejects building a second transaction log just for narrative effects.

### Acceptance matrix

| Failure point | Expected durable property state | Expected transaction state | Allowed player action |
|---|---|---|---|
| Offer declined before prepare | Both original owners retain property | Closed as declined | Start a new offer |
| Offered item moved before prepare | No transfer by this trade | Rejected or stale | Revise terms |
| Capacity rejects destination | Neither side loses property | Failed without settlement | Choose another destination or cancel |
| Duplicate accept request | One transfer at most | Same result returned | View outcome |
| Reload after commit | Both sides reflect final terms | Completed once | Continue from completed state |
| Notification callback fails | Both sides reflect final terms | Completed once | Retry presentation safely |

The rows must be mapped to actual APIs and save owners before implementation. If a row cannot be made true, report the blocker to the integration authority. “Usually both items transfer” is not sufficient for a property system.

## 426. Scaffold realization — Installment 40: an offer has an audience and a private edge

An offer may be private between two residents, visible to a small group, or published as an open request. Its audience changes who can respond and what information can be inferred. A public listing can help find a counterparty but can reveal scarcity, possession, or personal intent. The feature needs privacy choices without turning peer barter into a full market-board system.

### Offer visibility contract

Before creation, show who can see the offer, which terms are public, whether the item owner is named, how long the listing lasts, and who may reply. A private offer exposes its terms only to the intended recipient and the player if that is the current system contract. A small group offer can invite eligible participants without exposing unrelated personal information. An open listing may be out of scope if the existing barter owner has no public audience model; do not implement it as a communication-board post with hidden trade behavior.

Visibility is distinct from consent. A public offer does not authorize anyone to take the property. A private message does not reserve the item. A player who sees an offer can counter only through a valid response route. If the owner is absent, no one can accept for them unless an explicit representative exists.

### Branches by exposure choice

The player can share minimal terms publicly, invite a named group, send a private offer, or avoid publication and negotiate in person if a real dialogue route exists. Minimal public terms may increase replies but require later identity checks. A named offer can get a quicker response but reveals who owns the item. A group invitation can bring useful options and social pressure. A private offer protects details but may remain unanswered. Each route changes reach, delay, and privacy rather than merely changing a dialogue line.

If the player is seeking a rare item, open visibility may reveal the shelter's shortage. If the player offers a personal keepsake, they may prefer a private negotiation. If the transaction concerns an urgent shared task, a group can coordinate availability but cannot convert shared stock into a peer offer. The player may choose no listing and accept that they might not find a partner.

### Response moderation and competing offers

If several people respond, the owner remains free to choose among valid proposals or decline them all. The player may compare terms, confirm identity, ask questions, or close the listing. The system should not automatically rank offers by value if one includes a loan, a favor, or a personal cost. An alternate offer does not cancel the first until the owner selects an action. The original item must be revalidated at acceptance.

If the owner accepts one response, other pending responders receive a truthful closure if the message system supports it. They do not need to be told private details about the chosen counterparty. If an offer expires, responses close. If the item becomes unavailable, the listing is stale and cannot be accepted. A group broker can help compare logistics but cannot pick the person on the owner's behalf.

### Supporting faction role

A small brokerage group may provide a location, announce a public exchange window, or help parties meet. It does not set market rules or hold everyone's property unless an existing owner supports custody. Its rules can be local and practical: no exchange after a shift change, no open flame near stored goods, or a requirement to identify whether an item is loaned. Use canon-supported roles and current access data.

The group may request that offers use a standard form. The player can accept the form, negotiate privately, or leave. Standardization helps clarity but can make personal exchanges feel procedural. This tension is useful texture, provided the group cannot force disclosure beyond its legitimate safety role.

### Acceptance cases

Test private offer, named group offer, public listing if supported, multiple responses, item invalidated before acceptance, expiry, rejection, and audience after reload. Confirm an unauthorized reader cannot see a private offer and a public listing cannot move items without a new owner-confirmed settlement. If multi-party visibility is not in current APIs, the proposal must state that the feature starts with private two-party offers only.

## 427. Scaffold realization — Installment 41: disputes have causes and proportionate remedies

A dispute should explain what transaction fact is contested and what remedy is available. The current surface includes RaiseDispute without a reason parameter; that is a real design limitation to verify before building a player-facing dispute flow. The feature must not launch a generic “they cheated” branch from one button. Disputes can concern missing property, condition, timing, privacy, or an obligation that one participant believes was met.

### Reason and evidence

The dispute begins with the affected participant identifying a transaction and selecting a reason class or giving an authored account. The system stores only what the current owner can represent. It should distinguish verified mismatch from allegation. A record may prove that an item transfer occurred but not that it worked. A task event may prove that labor happened but not that the favor's recipient considered the terms complete. The player can review facts, ask both parties, request a limited inspection, propose a remedy, or close the dispute without resolution.

The other party may respond, decline to discuss, accept a remedy, propose another, or dispute the record. A response is not a confession. The player can choose whether to mediate, but neither party is forced to accept the player's judgment unless an approved rule grants that authority. If the player is also a party to the trade, show that conflict clearly and offer a neutral route if one exists.

### Remedy menu

Possible remedies are: return the item; complete the agreed favor; revise the due date; repair or replace an item if an owner supports it; reverse the trade atomically if both sides and the transaction owner permit it; offer a new exchange; or close with an unresolved record. A remedy must be executable and proportionate. An apology can repair a relationship but does not replace property. A returned item may not undo wear. A replacement is not valid if it is not equivalent under the participants' terms.

The player should not be able to confiscate unrelated property, impose labor, or deduct a universal trust currency as a punishment. If there is no executable remedy, the honest result is unresolved. The participants may choose not to trade again, but that future boundary should be narrow to the interaction and owned by existing systems.

### Branching based on prior repair behavior

If the player previously acknowledged a transfer error, the dispute may include a straightforward correction route. If the player previously ignored a return request, the affected participant can ask for a neutral witness or a stricter item check. If the player has honored loan terms, the other party may be willing to negotiate a return window. These are event-based conditions. The player can still surprise expectations by acting differently; no branch should be locked solely by a static reputation label.

The same fact can lead to different outcomes depending on what the player does now. A missing item can be found, replaced, or left unresolved. A late favor can be fulfilled, renegotiated, or declined. An item condition dispute can receive inspection, a price adjustment, or cancellation if atomic rollback is possible. The sequence matters, and the record should preserve it.

### Supporting group as neutral process helper

A small records or repair group can confirm a transaction log, inspect function, or host a meeting. It cannot decide personal intent or compel agreement. The helper's output is scoped; both parties can see what was checked, subject to privacy limits. If the player rejects the finding, they can continue negotiating or close without remedy. If the group lacks capacity, the dispute remains open or expires under a stated rule.

### Acceptance matrix

Cover missing item, stale offer, disputed condition, late favor, privacy breach, owner-verified record error, player as interested party, no agreed remedy, and reload during an open dispute. Verify the reason, linked transaction, evidence, response, remedy, and final status. If current APIs cannot store the reason, do not show a reasoned resolution in narrative; first resolve the ownership seam.

## 428. Scaffold realization — Installment 42: cross-faction brokerage without a grand coalition plot

Several small groups may help people find one another, verify terms, or move an item safely. Their value is operational and local. The player can bring groups together for a particular exchange, but the system should not escalate every barter into a political alliance. Supporting factions remain supporting: each contributes one limited capability, has its own schedule and interest, and can decline.

### Broker roles are non-overlapping

A **matchmaker** knows who is looking for a type of item; a **custodian** holds property if an owner-backed custody API exists; a **verifier** checks a specific item or record; a **messenger** carries approved terms; a **transport helper** moves an item through a real route. These are design roles, not assertions about current canon. One group may perform several only if canon and current systems support them. Don't add five parallel registries because the taxonomy is convenient.

The player can request a match, ask for verification, coordinate a handoff, use a private direct route, or trade without support. Each service has a cost and information boundary. A matchmaker can suggest a participant but cannot expose private inventories without permission. A verifier can report a result but not set the price. A messenger can deliver terms but not accept them. A transport helper can move the item but not change its owner.

### Multi-party branches

Suppose the player asks one group to find a battery and another to verify it. The battery owner may not wish to be identified publicly. The player can request an anonymous match, ask the owner to contact them directly, or stop. If the verifier's schedule delays the exchange, the player can accept uncertainty or wait. If the messenger costs a roster slot, the player can perform a direct meeting instead. A third group's standard form can clarify return terms, but it may not fit a personal loan. The player chooses whether process is worth the cost.

Groups can disagree about service boundaries: one may be willing to transport but not store; another may inspect only at a workshop. The player can choose the route that fits both sides, ask the participants to meet directly, or abandon the exchange. No faction relationship score automatically overrides the service contract.

### Consequences for future play

The player who chooses direct exchange gains speed but must manage the handoff. The player who uses a broker gets a wider pool but reveals some demand. The player who requires inspection gets more certainty but may lose the opportunity. The player who asks a faction to hold goods depends on the custody owner and any fee. These styles can lead to different callbacks and available help later without becoming global build classes.

A group may later offer a more efficient route if the player completed a specific exchange responsibly. It may also decline if the player repeatedly left scheduled deliveries unattended, if that history is owner-backed. This does not make the faction hostile or erase other service options. It changes one contract. A player who wants to repair the relationship can complete a new bounded task, but cannot buy forgiveness with a generic token.

### Example: three groups, one exchange

One resident wants to trade a spare hand tool for help carrying supplies. A work crew can confirm the tool is not allocated; a resident courier can arrange a meeting; a stores worker can suggest a shared substitute. The player may use all three, one, or none. The work crew's confirmation does not prove the resident wants to trade. The courier's willingness does not authorize the carrying work. Stores cannot redirect communal stock without its owner. The player can choose a direct private offer and bypass the group network entirely.

If the trade is accepted, settlement remains one atomic transaction. Supporting actors may receive separate service outcomes, but they do not each create another copy of the offer. If one service fails after the trade commits, the transfer stays completed and only that service is unresolved. If a courier carries an item before settlement, custody must be represented by the real inventory owner and the transaction must define what happens if acceptance fails.

### Acceptance checks

Test a single-service route, multiple services, service refusal, privacy-preserving match, delayed verification, direct exchange, and a support action failing before or after commit. Confirm faction roles do not overlap property ownership or transaction authority. If the design needs a multi-faction coordinator not present in the game, narrow the feature instead of adding a new major system.

## 429. Scaffold realization — Installment 43: personal boundaries around objects and favors

Some objects are private because of who owns them, what they mean, or how they are used. Some favors ask for time, access, labor, or personal contact. The exchange system must make boundaries negotiable without making them purchasable. This installment clarifies how a participant can offer a smaller scope, refuse a category of request, or change their mind.

### No means no; terms can still change

An owner can reject an offer without explaining. They may state a boundary—“not that item,” “not in public,” “not while I am on watch,” or “I can lend it but not give it away.” The player may accept the boundary, propose another item or time, ask whether a less personal exchange would work, or end the conversation. Repeating the same offer is not a new branch. Offering a different item can be a new trade, but the owner remains free to decline.

A boundary can be attached to one item, one audience, one task, or one participant. Do not infer that a person who declines to share a keepsake will decline every transaction. If the social owner does not persist narrow boundaries, use authored one-scene dialogue and avoid claiming durable memory. A player who respects the boundary can still offer unrelated help without treating it as leverage.

### Personal belonging authority

Items marked as personal belongings or keepsakes may have an owner that differs from ordinary inventory. The barter feature must consult that owner before offering transfer. The player can ask the resident whether they want to lend, gift, or trade it, but the UI cannot take the item from the belongings list. Some items should be non-transferable under current design. If the owner has a custody flag or consent route, reuse it; do not add an item-level morality rule in barter.

Where provenance or memory matters, the participant can disclose only what they choose. A player can learn that an object is important without learning the full story. The writing can make the boundary meaningful through a pause, a careful wording, or a practical alternative. It should not exploit personal history to make the player feel entitled to a concession.

### Favor boundaries

A favor involving labor must name expected time and scope. A request for personal company must be voluntary and should not be disguised as a work task. A favor that exposes a private location or message needs separate permission. The recipient may say yes to the practical portion and no to the personal portion. The player can separate them into distinct offers or withdraw. The system should avoid a single accept button that bundles multiple boundaries.

If the player previously offered a trade in exchange for information, the other person can limit or withdraw that information before sharing. If they already shared it, the player cannot promise to erase their memory; they can agree not to repeat it and use the communication boundary where supported. This is a narrative and privacy principle, not a new “secret” collection system.

### Branch scenario: a family coat

A resident offers a coat for a needed tool, then pauses when the player asks whether it is a permanent trade. They may offer a temporary loan, ask for another item, withdraw, or continue with explicit terms. The player can accept a loan, counter with a repair favor, ask for a different source, or walk away. A peer who previously returned a loan may be able to accept the coat under a timed agreement; someone who did not return a different tool may be asked for a stricter handoff. The participant still decides. The item must use current PersonalBelongingsSystem rules if it is classified as a keepsake.

Possible endings: the coat remains with its owner; a loan is recorded with return condition; a different exchange occurs; the player finds a shared-stock route; or the request remains unmet. The narrative can acknowledge the need without making the owner cruel for declining. A supporting group may help locate another coat or tool, but cannot appropriate the personal item.

### Acceptance review

Test item-specific refusal, alternate terms, loan versus gift, private information boundary, personal belonging restriction, partial favor acceptance, changed mind before settlement, and a later unrelated trade. Confirm one refusal does not globally disable all commerce and one accepted offer does not authorize future access. Close each scene with the terms or boundary the participants actually chose.

## 430. Scaffold realization — Installment 44: access, carrying, language, and the practical reach of a trade

A trade is not complete for a player merely because two dialogue participants agreed. They must be able to reach the meeting point, handle the item, understand the terms, and use an accessible route to accept or decline. This installment makes those practical conditions visible without inventing a universal travel or accessibility simulation.

### Meeting location is part of the offer

If the handoff has a location, identify who selected it, who can access it, whether the item can be carried there, and when the location is available. A room label is not proof of access. A survivor may not be able to attend a meeting at a worksite during their shift. A cart may not fit through a narrow route. A storage area may be closed. These facts must come from current location, schedule, inventory, or task owners.

The player can accept the proposed location, ask for a different place, use a permitted messenger, delay the exchange, or cancel. If a meeting is private, moving it to a public market space changes the audience. If a handoff is at a restricted location, access authorization is necessary. The offer should not assume that the player can physically teleport an item or character to satisfy a dialogue choice.

### Carrying and handling support

Some transfers may require help carrying, inspection, packaging, or safe storage. A supporting work group can provide such help if its current role and capacity support the service. The player sees what the helper will do and what remains the participants' responsibility. Help carrying an item is not permission to inspect its private contents. A custodian does not become its owner. A carrier can decline if the route is unsafe or the service is unavailable.

If carrying is outside the game's current mechanics, keep it as a meeting description, not a persistent action. Do not create an item weight stat or transport queue just for peer barter. The correct scope is the smallest truthful condition needed to make the trade readable.

### Language and comprehension

The terms should be presented in a format both participants can understand. If an in-world interpreter or translation helper exists, they translate only the approved terms. If the game provides no such system, author clear text and do not simulate language barriers through arbitrary refusal chances. A participant can ask for the offer to be repeated, simplified, or shown as a written receipt if those routes exist. Understanding terms is not the same as accepting them.

The player should be able to review the whole exchange: item identities, quantities, ownership, loan/gift/trade distinction, deadlines, favors, and dispute route. Use text labels and not color alone. Keyboard/controller navigation must reach accept, counter, decline, inspect, and cancel. No timed reaction should make the player accept a transaction before reading.

### Branches

A direct handoff can be fastest and most private, but requires both parties available. A messenger can bridge schedules but adds a service and privacy contract. A group meeting can create witnesses but expands audience. A later handoff reduces current pressure but risks expiry. The player may also abandon the deal and pursue shared stock or a different task. Each choice changes operational cost and reach.

### Acceptance cases

Review inaccessible meeting point, unavailable participant, item requiring supported handling, private handoff, helper with limited role, translated terms if supported, and controller-only interaction. Ensure a player can decline after inspecting full terms and that an unavailable route does not silently force the other party to travel.

## 431. Scaffold realization — Installment 45: interrupted settlement, rollback, and restoration

Because trade settlement touches both sides of an exchange, interruption and save/load behavior are central gameplay requirements. This installment describes expected observable outcomes around interruption. It does not prescribe implementation internals or create a second save authority.

### Checkpoints around commit

Before commit, both parties still own their property. During the owner transaction, the UI can show a pending state but should block conflicting actions on those items only through the existing owner. After commit, both sides reflect the final terms. If a save boundary occurs before commit, the offer remains pending or returns to a safe pre-commit state. If it occurs after commit, reload must show completion. There is no valid state where one item permanently moved and the other did not unless the participants explicitly agreed to partial settlement.

The source owner determines whether an in-progress transaction is serialized or atomically completed before save. The UI cannot implement recovery by storing a shadow copy of item counts. If the save owner cannot guarantee either pre-commit or post-commit consistency, the plan requires an architecture decision before the feature is exposed.

### Recovery branches

If an offer is stale, the player can revise, renew with both parties, or cancel. If an item moved to a different owner through another legitimate task, the old trade is rejected and terms can be renegotiated. If destination capacity changed, the player can select a valid container or decline; a valid alternate destination must not change who owns the item. If notification failed after commit, presentation can retry without repeating transfer. If one side's item is missing after restore, freeze the transaction route and invoke owner recovery; do not offer an ordinary retry.

### Save evidence set

Capture state before offer; after offer creation; after counteroffer; after both accept; immediately before commit; immediately after commit; and after notification. Reload each snapshot through the current campaign save path. Compare both inventories/ownership facts, offer state, favor state if present, dispute state if present, and downstream quest/message consumers. Use a stable checksum or owner-specific assertions only if the current project provides them. The plan calls for focused verification once implemented, not a new broad test suite.

### Determinism and repeated requests

An offer generated from a seeded event should retain its identity across reload. UI open/close does not create new offers or re-run a settlement. Counteroffer selection is a player choice, not random value negotiation. If a support group availability check is seeded, reuse campaign RNG and stable roster ordering. A retry cannot reroll an inspection or chance outcome to fish for a better exchange. If there is no random outcome, do not introduce one to add drama.

### Acceptance outcomes

Verify atomicity with two items; no transfer after a stale rejection; one transfer after repeated accept; durable completed state after save; no duplicate favor completion; no resurrected offer after reload; and a recoverable communication failure. All outcomes are measured at the property owner and transaction owner, not inferred from dialogue.

## 432. Scaffold realization — Installment 46: callbacks after a completed exchange

A completed trade can change what happens in later tasks and dialogue. The callback must be based on the transaction's confirmed fact and should not run before settlement. It must also be scoped to the feature's three subfeatures: offer terms, atomic property settlement, and obligations/disputes. A narrative callback is a consumer, not another trade system.

### Post-commit event contract

Once a transaction owner marks the exchange complete, it may emit one immutable completion event with stable participants, transaction identity, item/quantity references, term type, and completion time in game terms. The event should not duplicate mutable inventory authority. Consumers can update a quest, goal, message, or relationship record through their own owners. They must be idempotent so a replayed event does not double rewards or double trust changes.

If a callback fails, the property transfer stays completed. The consumer can retry independently, with a visible pending note if needed. If the save restores a committed trade but not its callback, the source owner needs a recovery route that safely replays the fact once. Do not let a player manually click “complete the story” to repair a missing integration.

### Narrative callback classes

**Use callback:** an item acquired by the player enables a real task or owner action. If no consumer exists, do not promise new utility. **Return callback:** a borrowed item comes back, closing or extending its obligation. **Social callback:** a participant refers to a concrete fulfilled term or specific breach. **Referral callback:** a participant tells the player about another possible exchange, which becomes a fresh offer and fresh consent route. **Dispute callback:** an unresolved issue affects whether the same parties want to deal again, only if the existing owner can represent that boundary.

Each callback should have a no-callback case. A completed trade may not interest the wider faction. An item may be stored and never used. A participant may say nothing after receiving it. Do not force every ordinary exchange into a questline. The system gains credibility when consequences scale to what the participants agreed.

### Branches from item use

If the player trades for a component and uses it in a task, the task owner supplies the result. The other party may later ask how the item worked, but the player can answer truthfully or decline. If the player kept the item unused, the trade remains complete. If the item is broken by an unrelated event, do not rewrite the original exchange as fraudulent. If the player pledged a favor and then uses the item, the favor persists until its own event occurs.

An item can also reveal a missed dependency. The player may decide to reserve it for a different use, ask the original owner to trade back only if both consent, or keep it. The seller does not retain veto after an ordinary completed sale unless terms specify a return option. A loan is different and follows its agreed return condition.

### Supporting group callbacks

A broker or verifier may ask for confirmation that its service was useful. The player can report a result only if known. The group can update its service availability or offer a future route through an existing system; no generic faction affinity is needed. A courier may request the item be picked up; a repair crew may ask that a loaned tool return before the next shift. These are practical follow-ups, not major faction quest branches.

### Acceptance review

Verify transaction event exactly once; consumer retry after load; favor distinct from trade; referral creates a new offer; item utility not promised without a consumer; unresolved dispute does not rewrite settlement; and post-trade message references the correct parties. Record any current event or save seam that cannot support these callbacks.

## 433. Scaffold realization — Installment 47: playstyles and multiple exchange endings

Peer trade should not funnel every player toward the largest inventory gain. Different approaches create distinct endings and future options. These are play patterns for design review, not persistent character classes or alignment scores.

### Playstyle patterns

**The direct negotiator** prefers clear private terms and quick agreement. They save service time but may miss information a broker could provide. Their trades should still show full terms and settlement risk.

**The cautious verifier** checks custody, condition, and price sources before acceptance. They reduce uncertainty and may lose a scarce opportunity while waiting. Not every item needs an inspection, and the game should not punish them for wanting evidence.

**The relationship broker** asks participants what they value, uses favors or loans, and seeks terms both can accept. This can build specific recurring exchanges but adds follow-up obligations. They can also be declined without a hidden social penalty.

**The privacy steward** avoids public listings, shares minimal details, and limits helper access. They protect personal boundaries but may have fewer potential matches. A private offer still has a valid route and is not mechanically inferior by default.

**The generous participant** gives items or time without exact equivalence. That should be a valid choice, but generosity does not waive item ownership or turn others into debtors. The player can make a gift, loan, or favor explicitly.

**The scarcity manager** prioritizes shared stock and emergency needs, often declining private bargains that conflict with a common task. Their route preserves communal commitments and can leave individual needs unresolved. The plan does not reward them with a “good citizen” score.

**The selective trader** avoids exchanges that carry too much uncertainty or social cost. They may miss opportunities but preserve capacity. The feature must allow them to walk away without a punitive event.

### Ending families

An exchange ends as: **completed trade**, **gift**, **active loan**, **favor owed**, **favor fulfilled**, **renegotiated terms**, **declined**, **expired**, **canceled before settlement**, **failed without transfer**, **disputed**, or **unresolved**. Only owner-supported states should appear. Each has a different next step. A completed sale does not imply friendship; a fulfilled favor does not imply indefinite access; a dispute does not prove fraud; an expired offer can be renewed only by a new consent action.

### Ending matrix

| Player approach | Immediate gain | Material/social cost | Possible callback | No-score boundary |
|---|---|---|---|---|
| Direct accept | Fast property exchange | Little verification | Use or return item | No automatic trust bonus |
| Inspect first | Better-supported terms | Time/service cost | Condition-specific remedy | No “paranoid” penalty |
| Offer loan | Temporary access | Return obligation | Extension or return | No permanent transfer |
| Offer favor | Work/social reciprocity | Scope and due date | Verified fulfillment | No favor currency |
| Use broker | More matches | Privacy and capacity | New participant offer | No faction alliance ending |
| Decline | Preserve current stock | Need remains open | Alternative source | No greed/cowardice label |

The ending should be calculated from transaction status and owner facts. A player can be generous in one transaction and cautious in another. Do not aggregate outcomes into a persistent playstyle flag unless an explicitly approved system owns one.

## 434. Scaffold realization — Installment 48: content continuity and canon validation

Before authored examples become data, validate every item, faction, resident, location, task, and event reference. The current project has a single JSON data authority and stable IDs; narrative examples in this document remain candidates. A plausible name or item is not proof it exists in the canonical catalog.

### Continuity review dimensions

1. **Items:** stable snake_case ID, schema version, owner/type, transfer restrictions, quantity/condition support, and current consumers.
2. **Residents:** existing ID, roster status, profession/skill if relevant, relationships only from current owner facts, and no silent replacement after departure.
3. **Factions:** canonical name and role, current availability, service scope, and no duplicated major-faction authority.
4. **Locations:** stable location identity, access/capacity, route, and actual use by current systems.
5. **Tasks and quests:** producer, objective state, completion event, save owner, and one-time reward behavior.
6. **Trade terms:** item/favor/loan semantics, settlement owner, dispute reason, and expiration rule.
7. **Dialogue:** source knowledge, audience, time, conditional facts, tone, and fallback text when history is absent.

### Canon-safe fallback patterns

If an example item is absent, replace it with a current equivalent only after checking ownership semantics. If no small faction owns the described service, remove the role or use an existing survivor. If a location does not exist, make the meeting location generic and non-persistent. If an event producer is missing, retain the scenario as a design illustration and mark it non-implementable. Never add placeholder IDs to authoritative game data just to make the plan's prose look complete.

### Branch reachability

For each branch, identify a player action and a current state condition that makes it reachable. If the only condition is a speculative personality score, rewrite it. If it depends on a prior transaction, verify that history is saved. If a branch can only appear after a faction service that is not available in ordinary play, it must not be the sole completion route. Each ending should have at least one reachable path from the core loop, and optional faction routes should remain optional.

### Review deliverable

The continuity pass should produce a compact ledger of used canonical IDs and unverified examples, linked to source data and owner contracts. That ledger belongs to the future implementation/content package if one exists; this document should not create a competing catalog. A reviewer can then reject or rewrite unsupported content without revisiting the whole design rationale.

## 435. Scaffold realization — Installment 49: implementation readiness and blockers

Plan 5 is not ready for implementation solely because it has many branches. Its hardest requirement is atomic ownership transfer. The readiness gate asks whether the current code can perform the promise at all and whether the result is player reachable, saveable, and observable.

### Required premise audit

Re-open SurvivorBarterSystem, PersonalBelongingsSystem, InventorySystem, ShelterBarterSystem, MarketSystem, Holdfast, and the actual host/persistence routes. Verify current offer creation, acceptance, item reservation, transfer delegation, rollback, retry, trust updates, disputes, save registration, and UI reachability. Confirm the reported conditional item check, absent reservation, sequential delegates, daily overdue trust change, and missing reason parameter still exist before treating them as findings. Source state may have changed.

Map each operation to one owner. Decide who owns the transaction identity and atomic commit. Confirm how both asset sources are prepared and how a failed destination capacity check is rolled back. Confirm how a completed event reaches quest, message, personal belongings, and reputation consumers. Identify who captures/restores pending offers, loans, favors, and disputes. If the owners cannot agree on one transaction seam, stop and request an architecture decision; do not implement compensating local state.

### Minimum vertical slice

The first implementation slice should be deliberately small: two eligible residents; one existing transferable item each; a private offer; a counteroffer; an explicit decline path; atomic acceptance; current inventory reflection; save/reload after completion; and one observable outcome. Do not include public listings, multi-faction brokerage, condition inspection, personal keepsakes, or generalized favors until that slice proves the owner contract. This order reduces the chance of writing content around a settlement primitive that cannot meet its promise.

### Verification claims

The team may claim the slice is integrated only if: one UI route creates and resolves the offer; both sides transfer or neither; duplicate requests are idempotent; a stale offer is rejected; save/load preserves the outcome; one downstream owner reacts once; and refusal does not mutate property. Verification should be focused on the files and system seam assigned by the foreman. Full-suite testing is not implied by this document.

## 436. Scaffold realization — Installment 50: closeout checklist for Plan 5

Plan 5 closes as a proposal only after its measured length meets the requested threshold, its exact three-subfeature boundary remains intact, examples are marked as canon candidates where necessary, and the current transaction evidence has been re-audited. Length alone cannot prove a safe trade implementation.

### Closeout package

The final plan review should contain: the three Section 4 subfeatures; the atomic settlement requirement; owner map for current property and trade state; consent and privacy boundaries; supported loan/favor/dispute semantics; save and replay expectations; current host route evidence; canonical content inventory; and unresolved architecture decisions. It should state whether any branch is deliberately deferred because the current owner cannot express it.

### Handoff checklist

1. Claim exact files in the current worktree ownership ledger.
2. Revalidate the forensic premises against current source, data, and tests.
3. Resolve the single transaction-owner decision before adding content.
4. Implement one private, two-party vertical slice through existing authorities.
5. Capture and restore every persistent offer/transaction state through its owner.
6. Use deterministic event identity and idempotent settlement.
7. Keep personal belongings and shared stock behind their existing owners.
8. Add focused verification for transfer, refusal, stale state, duplicate request, and reload.
9. Add broader content only after the complete transfer can be observed in play.
10. Report unimplemented brokerage, public listings, condition, and dispute paths accurately.

### Final closeout statement template

“Plan 5 is closed as a proposal at [measured word count] words. It retains exactly three Section 4 subfeatures: explicit two-party offers, all-or-nothing settlement, and scoped favors/disputes. The current source audit [date] verified [transaction owners and API facts] and left [atomicity/save/host gaps] unresolved. No production barter feature is implied. The next package is [authorized package], contingent on [owner decision] and using the current shared ownership ledger.”

Complete the placeholders only after the source audit. If atomic settlement remains unavailable, the plan can be complete as a document while the feature remains blocked from implementation.

## 437. Secondary expansion — player-originated offers and counteroffer branches

The player should be able to initiate an exchange from an actual item or need, not only react to a scripted offer. A player-originated offer begins by choosing an item the player is permitted to transfer, borrow, or offer as a gift. The interface then identifies possible recipients only from an existing roster or trade route. It must not enumerate every survivor's private inventory to manufacture a convenient match.

### The offer is an invitation, not a command

The player previews exact terms, recipient, audience, expiry, and the current source of each item. They can send, revise, save as a draft if the owner supports drafts, or cancel. Sending does not reserve either side's property unless an explicit owner action does so. The recipient may accept, decline, counter, ask a question, or not respond. A player cannot make a trade happen by selecting a dialogue option that claims the other person's agreement.

The player may also propose a loan, gift, or favor instead of a sale. The UI names the distinction before transmission. If the player chooses a loan, it shows the return condition. If they choose a favor, it shows the concrete scope and due condition. If they choose a gift, it states that no return is expected. These options should not be hidden beneath a single “offer item” button.

### Counteroffers change one term at a time where possible

A recipient may counter with a different item, quantity, time, delivery route, or obligation. The interface shows the changed term and retains the previous proposal for comparison. Both sides reconfirm the final version. If multiple terms changed, summarize them together. The player can accept, counter again, ask for clarification, decline, or withdraw. The original offer does not remain active in parallel unless the owner explicitly supports multiple proposals.

The player can make the counter less favorable to themselves, but the system should not label this generosity. A person may reject a seemingly generous offer because the item is unavailable, the favor is unwanted, or the timing is wrong. Their response is theirs. If no reply arrives, the player may send a bounded follow-up, wait, or close the offer. Repeated follow-ups must not create pressure loops.

### Initiating from an unmet need

When the player lacks a component, they can ask a known resident, check a public listing if one exists, ask a support broker for a match, seek a shared-stock route, or defer the task. The system can suggest candidates based on public offers and eligibility, but not private possessions. A candidate can be unavailable or decline. The player may use another route without treating refusal as betrayal.

When the player has a surplus, they can offer it privately, post a bounded listing, give it away, or keep it. A public listing can create more replies and more attention; a private offer can preserve discretion but may receive no response. The player should see quantity and transfer restrictions before selecting recipients. Items reserved for a task must be released through their owner before they can be offered.

### Response writing and failure states

Use distinct responses: “not interested,” “need it for current work,” “can consider another time,” “that item is not mine to offer,” and “I did not see this before it expired,” where state supports them. Avoid a single stock refusal that makes all characters sound alike. A participant can provide no reason. The player can continue with a different person or stop. If the offer becomes invalid before response, the recipient should not accept a stale contract.

### Acceptance walkthrough

Create player offer, counter one term, revise, get a decline, create a second offer to a different eligible participant, accept it, and reload after settlement. Confirm offers are not sent twice, counterterms are exact, refusal does not alter property, and the second recipient consents independently. If public listings do not exist, keep the candidate search limited to current APIs.

## 438. Secondary expansion — minor-faction throughlines that remain supporting roles

The request calls for supporting factions with a real part to play. Their contribution should be felt across several scenes but remain subordinate to the player and the two people making an exchange. A group can have a recognizable service culture, help in a few specific branches, and remember a concrete collaboration without taking ownership of the trade feature.

### A throughline made from repeated services

At first, the player can ask a stores or workshop group for a narrow fact. Later, the group may offer a more efficient process if the player returns equipment, gives advance notice, or respects the group's capacity. A third interaction can reveal a limitation: the group cannot handle private keepsakes, urgent delivery, or a disputed ownership claim. These callbacks make the faction feel present while preserving scope.

The player's choices create different group relationships in action, not in score. A player who uses the service and follows its terms may receive a direct route next time. A player who changes a request after the group has committed staff may be asked to reschedule. A player who does not want help can trade directly. The group remains available for other legitimate work. No one action determines a faction ending.

### Major-faction boundary

Major factions may set broad shelter conditions or control a high-level resource, but ordinary peer exchanges should not repeatedly route through them. If a major faction's policy constrains item transfer, show the rule through its actual owner and let minor groups help interpret or execute a permitted process. Do not use a major faction representative to approve every trade. Do not turn a small support crew into a political proxy for a major faction.

### Three compact service arcs

**The clean handoff:** a helper teaches the player a receipt-and-return routine; later, the player can complete a loan with less uncertainty. The reward is clarity, not a permanent discount.

**The overloaded counter:** a group is too busy to broker an exchange. The player can wait, use a direct offer, or choose shared stock. If they return later, the service may be available. The refusal is about capacity, not hostility.

**The boundary correction:** a helper initially assumes an item belongs to a group, then checks and learns it is personal. The player can stop the listing, ask its owner, or continue with another item. The group updates its process; it does not seize or adjudicate ownership.

Each arc uses only a service fact, one or more player decisions, and a bounded callback. It can be dropped without harming the main trade loop. The content author should replace these role names with canonical factions only after checking data and current narrative authority.

## 440. Secondary expansion — failure after one participant believes the handoff happened

A handoff can be socially complete in the participants' minds while the owning system has not recorded a transfer. The player might see an item on a table, receive a verbal confirmation, or watch a helper carry a package. Those observations are not a substitute for custody state. The interface should acknowledge what the player saw while making clear whether the transaction actually committed.

If the owner reports that no transfer occurred, the player can retry through the same transaction identity, ask the holder to confirm current custody, cancel and return to negotiation, or leave the exchange unresolved. If one item moved but the transaction failed, freeze ordinary use of the partial state and let the transaction owner recover it. Do not make the player manually give a second copy or steal the item back through an unrelated gift route.

### Participant-facing explanation

The explanation should avoid accusing either person: “The exchange was not recorded; both items remain with their listed owners,” if that is what the owner proves; or “The handoff is incomplete; check custody before continuing,” if it cannot yet establish both sides. A helper may say they carried the box but cannot confirm the final receipt. The player can choose to wait, contact both parties, or abandon the transaction. No one is labeled a liar merely because state and recollection differ.

If the two participants remember different outcomes, the player can compare the transaction record, inspect the current custody source, and ask for a corrected receipt. A support group may help locate the item, but cannot settle ownership by fiat. The dispute remains open until a valid remedy or closure. If the current APIs cannot represent this state, the player-facing scene should not be implemented until the transaction owner can resolve it.

### Acceptance walkthrough

Interrupt immediately after an in-world handoff animation but before the owner confirms commit. Reload and inspect both item owners, transaction state, and messages. Ensure the player is told which actions are safe; neither side can duplicate the item; and a retry returns the same result. Then simulate a legitimate owner transfer occurring during the pending exchange and verify that the original offer becomes stale instead of reclaiming the item. The test is about custody and recovery, not whether the narrative scene looked complete.

## 439. Secondary expansion — unequal exchanges, generosity, and informed consent

Not every accepted exchange has equal market value, and the system should not assume that equality is the only fair outcome. One participant may need an item urgently; another may prefer a small practical favor; a third may give something without expecting a return. The game can support these choices as long as the participant understands the terms and owns the decision.

### Equality, equivalence, and preference

An equal quantity is not necessarily equivalent. One tool can replace another for a particular task but not for a personal use. A short loan can matter more than a permanent transfer. A participant may accept less market value because they want the exchange completed today, or reject a high-value offer because it includes an unwanted favor. The player can see available references but cannot calculate another person's private preference.

If the game exposes a value estimate, present it as information, not as a fairness verdict. If the player offers an unequal exchange, the recipient can accept, counter, decline, or ask why. The system does not label them exploitable or greedy. A player can ask whether they want a different term, offer a gift, make a loan, or walk away. Clear consent is more important than mathematical equality.

### Gifts and asymmetric generosity

A gift explicitly carries no return obligation. The recipient can accept or decline; accepting does not create an indefinite claim on their future help. The player can give away an item without acquiring social leverage. A resident may respond warmly, neutrally, or with discomfort depending on authored context, but the item transfer remains the factual outcome. If a gift is actually a loan or favor, name it before the recipient agrees.

Generosity can be costly. The player may give away a spare component needed later, and a support group may confirm that replacement stock is uncertain. The decision can still be made. The later consequence is that the player lacks the component or must find another source, not a hidden virtue bonus. A resident who cannot accept a gift may propose an exchange instead; the player can honor that preference.

### Pressure and refusal safeguards

The offer screen should avoid defaults that favor acceptance, countdown pressure without an owner-backed expiry, or confirmation buttons that hide obligations. A participant can say no without an explanation. A player can withdraw before settlement. If one party has more power over the other, the feature should not presume that a quick yes is free consent; where the project has an existing consent authority, use it. If it does not, keep the scene narrow and avoid presenting coerced terms as a healthy trade.

The participant can change their mind before commit. After a completed transfer, a return is a new agreed transaction unless policy or a defect remedy says otherwise. This distinction prevents the player from treating a gift as revocable whenever it becomes inconvenient while also protecting pre-commit consent.

### Example: the replacement filter

The player offers two common parts for a filter that has no reliable market estimate. The recipient accepts because the parts solve an immediate task, then asks that the deal remain private. The player can confirm the terms and private audience, ask whether a simpler one-part exchange would work, or decline. A support trader may state that the filter is rare, but this does not override the owner's choice. If the player takes the filter and later gives it away, the first trade remains completed; the new gift is a separate action. If the player promises the filter to a task before settlement, that promise is not fulfilled until the owner commits the transfer.

Possible results include an unequal accepted exchange, revised terms, a private gift, refusal, or a shared-stock alternative. The scene is complete only when the transaction owner confirms the outcome. The narrative can remember a clear preference, but only a persisted, owner-backed boundary may affect future offers.

### Review questions

Can a player understand every obligation before acceptance? Can a participant freely decline? Is a gift actually a gift? Does an estimate become a verdict? Can unequal terms be accepted without being framed as deceit? Is the audience explicit? Are post-commit reversals handled by an owner? These questions let the feature support different playstyles while keeping negotiation humane and mechanically truthful.

## 441. Branch casebook — the handcart on three schedules

This compact arc follows one shared-use handcart through two peer requests and one support service. It is written to create several distinct paths from player actions without adding a new commerce campaign. The cart is illustrative: replace it with an existing transferable item or remove the case if no suitable item and task owner exist. The core challenge is scheduling and custody, not a morality judgment about who deserves help.

### Starting conditions

A resident offers to repair a cart wheel in exchange for a timed loan of the cart on the next work period. A second resident has already asked to use it to move supplies. The current inventory owner reports where the cart is held; the task or roster owner reports which use is scheduled. The player can inspect the terms, ask both people to clarify their time window, propose another item or task, use a support crew to check the repair, or decline. No one owns the cart merely because they need it.

The offer is not accepted yet. The repair is a promise, not completed work. The cart is not reserved unless the owner supports reservation. The player sees the difference between an intended loan and a confirmed transfer. If the item is communal equipment, the exchange must route through its actual owner and may not be available for private barter at all. The story begins only if a valid exchange route exists.

### First branch: establish custody and purpose

The player can ask the cart holder to verify its availability. The holder can confirm it is free, explain a current reservation, or be unavailable. A support repair crew can inspect the wheel, but it does not decide who may use the cart. If the player proceeds without inspection, the cart may be offered with a clear condition uncertainty only if the owner can represent that uncertainty. If the player waits, the work period may pass and both requesters need new terms.

The repairer may want access to the cart before the loan, a loan after the repair, or a separate tool exchange. These are materially different offers. The player can ask them to specify which action they agree to perform and when. If they refuse to define terms, the player can leave the offer open or decline it. A support crew may explain the safe repair steps; it cannot promise the resident's labor.

### Second branch: negotiate the competing use

The second resident's request can be handled separately. The player can give them priority and ask the repairer for a later loan; ask the repairer to take the cart to a different time; see if the cart's owner allows a short split use; locate another carrier; or decline both requests and preserve the existing task order. A split schedule is valid only if task timing and item owner support it. No one gets a free slot because the player picked a dialogue line.

If the player tells the repairer that the cart will be free without checking the second reservation, they create an inaccurate offer. Before acceptance, the owner rejects the stale terms. The repairer can revise or withdraw. The player can own the mistake through a corrected message, but a relationship penalty is not automatic. If the player checks first, they may discover that a smaller, later window works for both people.

### Third branch: the repair changes the deal

Inspection shows the wheel is usable for light loads but not the heavier route originally planned. The repairer can still take the job, ask for a different tool, perform only a simple adjustment, or decline. The player may accept the revised scope; find someone qualified for a full repair; postpone the loan; or proceed with the cart under a disclosed use limit if the relevant owner supports that. The second resident may choose to take the cart for a light load or request another route.

The revised condition matters to settlement. A complete wheel repair can be a favor or task completion but does not itself transfer the cart. If the trade is a timed loan, the cart remains the lender's property and needs a return event. If the terms change to permanent transfer, both participants confirm the new offer and settlement uses the atomic transaction route. The interface should not silently switch terms because a task callback fired.

### Supporting-faction contribution

A small workshop group can test the wheel or lend a repair tool. A stores group can confirm whether another cart exists in shared stock. A transport crew can offer a separate route for the supply run. These are conditional roles, not new named factions. Each helps one part of the decision and has a visible limit. The workshop cannot prioritize the borrower; stores cannot take a privately held cart; transport cannot accept the peer trade. The player may use none of them and still negotiate directly.

Support has opportunity cost only when a current task/roster owner can represent it. A workshop check may delay another repair. A stores lookup may take a day if that is a real service constraint. If cost cannot be persisted, the prose should state the uncertainty without inventing a service queue. The group can decline because it lacks capacity; the player can wait, use another route, or cancel.

### Resolution paths

**Timed loan plus completed repair:** the cart is transferred into temporary custody through the owner; the repair task completes; the cart returns by an agreed event; the original owner retains title. The supply run may happen before or after the return depending on schedule.

**Repair first, loan later:** the player preserves the second resident's immediate use, then negotiates a new loan window. The repairer accepts or declines the revised time. The initial counteroffer does not remain active.

**Alternative transport:** the player uses a support group's route and leaves the cart with its owner. The repair may be postponed or declined. A referral becomes a separate task or message, not an automatic trade.

**Permanent exchange:** the player and owner accept a revised property transfer; atomic settlement moves both sides or neither. Later use belongs to the new owner. A repair promise remains a distinct obligation only if the final terms include it.

**No deal:** one or both participants decline. The cart stays with its owner; the underlying task can use another route or remain unresolved.

**Dispute:** a transfer or return is contested. The player sees the exact reason and available remedies; the system does not automatically decide fraud.

### Late return and the humane dispute route

If the loaned cart is not returned at the expected time, first show the recorded status: due, no return recorded. The player can send a reminder, ask for a new window, check current custody, request an allowed helper, or close the favor unresolved. If the borrower says they returned it but custody is elsewhere, the two accounts remain distinct until an owner fact resolves them. A support group can check the storage point or record, not infer intent.

The borrower can explain a delay, decline to discuss it, or ask for more time. The player may accept, counter, request return, or choose another operational route. A late return does not cause repeated daily trust deductions unless a current approved owner policy says so. A verified broken wheel can lead to repair, a negotiated replacement, or a dispute; it does not erase the original trade record.

### Endings and callbacks

1. The cart returns on time and both residents use the agreed sequence.
2. The repair succeeds but the second resident's supply task uses another route.
3. A short loan works; the full repair remains a new task.
4. The player chooses a support transport service and the peer offer expires.
5. The cart remains in shared custody; no private transfer occurs.
6. A late return is renegotiated without escalating to a dispute.
7. An item-location disagreement remains unresolved but the player can continue other tasks.
8. Atomic settlement fails safely and neither side loses property.

Later dialogue should reference only the observed outcome. “The wheel held through the short route” is a specific report if the task owner or participant confirms it. “We can always count on the workshop” is too broad from one service. A person may offer another loan because the last one returned; they may also decline for an unrelated reason. Supporting factions appear in only the branches where the player requested their service.

### Acceptance map

The content review lists every offer identity, item owner, reservation, loan duration, task completion event, support service, save checkpoint, return status, dispute state, and ending. It traces two paths through commit and one path through failure/retry. If the cart cannot be loaned through an owner-backed custody seam, replace the example with a supported item rather than scripting a fake temporary transfer. This preserves the design's human stakes without claiming unsupported mechanics.

## 442. Branch casebook — the valve that might still belong to stores

This casebook follows an exchange whose first problem is ownership, not price. A resident has a small replacement part that could help complete a repair. They offer it to the player in return for a task favor. The inventory record does not clearly establish whether it is personal or communal. The player can proceed only after the appropriate owner resolves custody. The part is illustrative and must be replaced by a canonical transferable item if the project supports one.

### Opening offer and what is not yet known

The resident says they brought the part back from a work area and no longer need it. The inventory owner reports that one matching unit is absent from communal stock, but no transfer event links it to this resident. The resident offers a trade because they want help carrying a personal load later. The offer names no due time. The player sees three separate unknowns: current custody, authority to transfer, and exact favor scope.

The player can ask the resident to clarify where it came from; ask stores to check its ledger; pause the offer while the provenance is checked; propose a different item; or decline. The resident can decline to answer personal questions while still allowing a narrow inventory check. A support stores group can look up a public stock record but cannot assert the resident stole the part. The player must not accept the item solely because the need is urgent.

### First branch: the record resolves or remains ambiguous

If stores confirms a signed-out loan and the current resident is its borrower, the player can ask them to return it, request an authorized transfer if policy allows, or close the barter offer. If the record shows a legitimate personal purchase, the owner can make a new offer with clear terms. If no reliable record exists, the player can accept only if the current property owner permits the transaction with unresolved provenance; otherwise the trade pauses. The narrative keeps the uncertainty visible.

The resident may say they thought the part had been released. That account is not proof, but it can open a respectful process: ask the storekeeper, return the part without blame, or let the owner request a formal transfer. A player can avoid public accusation and still protect shared stock. The resident may agree, counter with labor instead, or withdraw. No route forces confession.

### Second branch: define the favor

If the item is transferable, the resident asks for help carrying a load “when there is time.” The player can ask for a concrete scope: one trip, a named location, a time window, or help with a task. The resident can specify that they need only a hand for the first section, not a full escort. The player can agree to the bounded favor, offer another item, give the part as a gift, or decline. If carrying is not an owner-backed task, the favor remains narrative and cannot be saved as a persistent obligation.

The player may also choose a loan rather than transfer. The resident can accept if they need the item only temporarily. The offer specifies return condition and the later event that ends the loan. A support helper can arrange a meeting or verify the part, but does not fulfill the player's favor. Terms that change after the item is inspected require both sides to reconfirm.

### Third branch: the communal task takes priority

Once the part's status is clarified, a common repair task may still need it. The player sees the reservation and can preserve it, request release, use a substitute, or let the peer exchange proceed only if the owner permits. The supporting stores group can explain the queue. A small repair team can say whether a substitute works if that falls within its role. Neither decides that the resident's need is unimportant.

The player can choose to release the part for the common task and make a different offer; leave it with the resident and accept a delay to the shared repair if permitted; find another item; or stop both plans. A major faction may have a broad resource policy, but only its established authority determines that restriction. The minor group helps explain or execute a permitted route and does not become the political center of the episode.

### Distinct endings

1. **Returned to shared stock:** the resident returns the part, the inventory owner records it once, and the player can pursue the communal repair.
2. **Authorized peer trade:** current ownership is verified, exact item/favor terms are agreed, and settlement completes atomically.
3. **Temporary loan:** the part changes custody with a return condition; no permanent sale is implied.
4. **Alternative favor:** the resident withdraws the item offer and accepts a smaller, clearly scoped task.
5. **Substitute found:** the shared task uses another permitted item, and the peer offer continues independently.
6. **Uncertain custody:** the offer is paused until evidence or owner decision; the player does not accuse or transfer.
7. **No deal:** either participant declines; property remains unchanged.
8. **Dispute after transfer:** a record mismatch opens a reasoned remedy route; settlement is not silently reversed.

### The support group's limited memory

If the player returned the part and recorded the disposition, stores can later prepare a receipt or confirm stock. If the player used an authorized trade, the group can refer to that transaction when checking another item. If the player bypassed the stock owner, future assistance may require a verified record, but only if current service logic supports that. The group does not become hostile, and the player does not gain a generic “stores favor.” Its recurring presence is a process and voice: measured labels, concern for tools that vanish between shifts, and a willingness to explain the record.

### Voice and sensory detail

The resident might turn the part over to show a stamped edge; the storekeeper checks a smudged line in the ledger; the repair worker asks whether the part is for the common task or the private offer. Use these details to distinguish evidence from memory. Do not imply that handwriting, wear, or a stamp proves more than the current authority can establish. A small receipt can become a later callback only if the game persists it.

### Implementation map

For each ending, map item ID, custody source, transfer/return API, offer identity, favor owner, support service, task consumer, save point, and dialogue condition. If the current inventory system cannot represent ownership distinctions or transfer origin, do not author the provenance dispute as a working branch. Keep the content premise, report the seam, and use a simpler owner-verified offer for the first vertical slice.

## 443. Candidate exchange lines — counteroffers that reveal terms and voice

These short lines are provisional examples. They are not canon dialogue and must be checked against current residents and setting facts. Each one adds a practical distinction to the offer, rather than labeling the speaker as virtuous, greedy, suspicious, or difficult.

### 1. From sale to loan

**Offeror:** “I can spare it for two shifts. I don't want to lose it for good.” **Player actions:** accept the loan terms; ask whether return after one task works; offer a different item; decline. **Consequence:** a loan record exists only if the transaction owner can represent it. The player's agreement cannot turn a temporary handoff into ownership.

### 2. The item is not theirs to offer

**Offeror:** “It sits in my locker, but it belongs to the work team.” **Player actions:** ask the team owner; offer a personal item instead; pause; walk away. **Consequence:** physical custody does not prove transfer authority. The resident is not blamed for clarifying the boundary.

### 3. A favor is too open-ended

**Offeror:** “Help me later.” **Player:** “What would count as help?” **Offeror:** “One hour sorting the wire after the inspection. If the inspection runs late, ask me again.” **Consequence:** if the player accepts, the obligation has a scope and a recheck condition; it does not automatically trigger from an unrelated task.

### 4. A counteroffer protects scarce stock

**Offeror:** “Two batteries for the filter.” **Player action:** offer one battery and a later repair; the owner replies, “One battery now. We can talk about the repair after I know the filter fits.” **Consequence:** settlement splits only if both resulting contracts are separately supported. Otherwise, keep the exchange as a single all-or-nothing agreement.

### 5. A personal boundary

**Offeror:** “Not the coat. I can lend the spare blanket.” **Player actions:** accept alternate item; ask whether another deal might work; decline. **Consequence:** refusal is item-specific; it does not mark the resident unavailable for all trade.

### 6. A price reference is not a command

**Support broker:** “Last week, a similar part went for two tins. That tells you what changed hands, not what this one is worth to its owner.” **Player actions:** use it as context; ask for a current quote; negotiate without a price; leave. **Consequence:** the reference informs but cannot overwrite consent.

### 7. Late return, no accusation

**Borrower:** “The cart is still at the lower ramp. The wheel caught; I stopped there.” **Player actions:** ask for a safe recovery route; extend the loan; request the item back; open a dispute if terms were breached. **Consequence:** the item remains in the actual custody state. A delay is not automatically a refusal.

### 8. The deal no longer fits

**Offeror:** “I don't need the tool now. Keep the offer closed.” **Player actions:** acknowledge closure; ask whether a different item is needed; stop. **Consequence:** no obligation survives unless another accepted term remains. The offer does not recur without a new cause.

### 9. Disagreement after completion

**Participant A:** “I thought the repaired handle was part of the exchange.” **Participant B:** “We agreed on the spare handle only.” **Player actions:** check the recorded terms; ask each participant; offer a new exchange; leave it unresolved. **Consequence:** the transaction record confirms what was stored, but cannot determine unrecorded intent. Any additional transfer is new and requires consent.

### 10. A faction helper declines to broker

**Helper:** “We can check the ledger. We can't choose a price for either of you.” **Player actions:** accept the ledger check; negotiate directly; use another service; withdraw. **Consequence:** the helper's boundary narrows the service but does not block all routes.

### Voice and accessibility pass

Keep each response understandable when displayed alone in a trade card. A short line can be read by screen readers and remain legible at scale. Put the essential term—loan, quantity, due condition, audience—into structured labels as well as dialogue. The prose adds warmth and uncertainty; it does not hide a deadline in a flourish. Avoid shaming refusal or framing a counteroffer as a combat challenge.

### Branch authoring test

For every line, create at least one response that accepts, one that revises, and one that declines. Then check that each response maps to an owner-backed state. If the system cannot represent a line's promise, remove the line or keep it as transient dialogue with no saved consequence. Rich voice comes from the exact detail a person notices, not from unsupported mechanics.

## 444. Bundled-offer design — separate item, favor, timing, and audience

A survivor may propose a bundle: one item now, a temporary loan later, and a promise of help after a task. Bundles can create rich negotiation, but they also hide risk if the player accepts them as one vague favor. Every term must be visible and owned. The feature should allow a bundled exchange only if the transaction owner can settle or coordinate its components safely.

### Four term layers

**Property term:** item identity, quantity, current owner, destination, and transfer type. **Obligation term:** who performs what, for whom, by when or after which event. **Timing term:** when the handoff and work occur, including expiry. **Audience term:** who may know the arrangement and any personal details. A participant can accept one layer and decline another only if the offer model supports partial acceptance; otherwise, the offer is all or nothing and that is stated clearly.

The player can accept the whole bundle, ask to split it into separate offers, counter one term, propose a smaller package, or decline. Splitting helps clarity but can change leverage: the item may move before the favor happens, or the participant may decide they only want one part. Both sides reconfirm each resulting contract. No item moves during negotiation unless a supported reservation state does so.

### Example: a tool, a delivery, and a future favor

A resident offers a repair tool in exchange for the player delivering a container to a named room and helping sort materials after the next inspection. The player should see that the delivery is an immediate task, the sorting is future work contingent on an inspection, and the tool transfer may be permanent or temporary. The player can accept a loan for the tool and the immediate delivery while leaving the future favor for later; ask to remove the future favor; offer another item; or decline. The resident can counter with a narrower sorting task or decide the bundle is no longer useful.

If a delivery task cannot be assigned or the future inspection has no owner-backed event, the system cannot save those terms as a durable obligation. The player may still agree to an authored one-scene exchange, but the UI must not promise a future reminder. If the tool moves through a peer trade, that settlement is atomic. The task event and favor are separate outcomes and cannot be merged into “deal complete” until each has a truthful disposition.

### Failure and renegotiation

The tool may become unavailable while the task is underway. The inspection may be canceled. The recipient may no longer need the delivery. The player can renegotiate, close the affected term, or cancel the remaining bundle. Completed terms remain completed unless a supported reversal exists. An incomplete future favor does not automatically reverse an already-settled item transfer. Participants may agree to a return or new exchange, but that is a new contract.

If one participant accepts a revised bundle and the other does not, retain the last mutually agreed terms or close, according to the offer owner. Never store a half-updated local view as the official deal. A stale party status can invalidate the whole bundle before settlement; the player sees which terms can still be renegotiated. If the bundle is too complex for current owner APIs, ask the parties to split it into a supported two-party offer and one bounded task or remove the scenario.

### Supporting group contribution

A minor faction can help with one layer: verify the tool's shared allocation, perform an authorized delivery, or confirm the inspection event. Its contribution does not validate the entire package. If a group performs the delivery, the player is not credited for doing it unless the terms allowed delegation. If the group reports that the inspection moved, the favor due condition follows the event owner. If it holds the tool, custody must be recorded by the asset owner.

### Bundle endings

1. All terms are accepted and each owner confirms its part.
2. The item transfers, but the future favor is explicitly removed before acceptance.
3. The item is loaned, and the immediate task completes; return remains pending.
4. The parties split the bundle into a trade and a separate task.
5. One term becomes impossible; both renegotiate the remainder.
6. The whole offer is declined before transfer.
7. A save/reload shows the last mutually accepted version with no duplicate component.
8. A support service fails after the trade commits; only that service is unresolved.

### Review gate

For any authored bundle, list its contracts, owners, acceptance identities, event prerequisites, save states, cancellation rules, and customer-facing confirmation. If two terms cannot be represented distinctly, do not pretend one compound status makes them durable. A simple, clear exchange can carry more dramatic depth than a complicated bundle whose promises do not survive a day transition.

## 445. Information in an exchange — consent to share is not property transfer

Survivors may exchange directions, instructions, memories, or an introduction. These can create rich peer negotiation, but information is not an item that the barter system owns. A participant may agree to explain a route, share a technique, or provide a copy of a public record; the game should not treat that as transferring the underlying knowledge permanently. The person may choose what to disclose and can decline further questions.

### Define what is being offered

An information exchange names the topic, level of detail, intended audience, and any follow-up. “Tell me where the shelter is” is too broad. “Show me the marked turn on the map you are willing to share” defines a bounded request. “Teach me how to read the pressure gauge” names a session, not permanent skill transfer. “Introduce me to the trader” requests a contact, not a guaranteed deal. The recipient can accept, narrow, counter, or refuse.

If information is represented as an item or quest flag, confirm that the current data owner actually uses that model. Do not create a secret ledger of private facts in SurvivorBarterSystem. A knowledge exchange may instead be an authored scene, a task event, or a referral message. The system records only what current owners can support and only the result the recipient agreed to share.

### Branches and limits

The player can offer an item for a lesson; offer a favor for a map annotation; ask for a source check; request an introduction; or decline to trade for information. The other person can share a public fact, share a personal account, offer a demonstration, provide a partial answer, or say they are not comfortable discussing it. The player cannot choose “reveal everything.” A partial answer can still be useful and may lead to an alternate route.

Some information cannot be verified by the speaker. A survivor may teach the route they remember while a cartography owner marks it unverified. A mechanic may describe their own repair method but cannot guarantee another tool will behave identically. A support records group may supply a documented fact, but the trade must not charge a person for public information the group owns. These distinctions shape the branch and prevent the player from buying certainty.

### Example: instruction for a spare part

A resident can show the player how they identify a compatible fitting. They ask for a spare part in return. The player can accept a one-time demonstration, ask for a written note if an authored content route exists, offer a smaller item, or decline. The resident can choose to explain the safe step but withhold their personal shortcut. A repair crew can verify the instruction against current task rules, but the crew does not decide whether the resident should share their knowledge.

If a task later uses the instruction, its owner still validates materials and worker eligibility. The player cannot use a remembered dialogue line to bypass a catalog gate. If the information is wrong, the player can ask for clarification, seek another source, or abandon the method. The exchange may be completed even though the method was not useful; a remedy requires explicit terms, not an automatic accusation.

### Supporting-faction interactions

A minor group can offer public training, a posted map, or a recorded procedure as its own service. A resident may offer personal experience that differs from the group's guide. The player can compare sources, ask each to clarify, or act conservatively. Faction membership does not make one account universally correct. A group can decline to share restricted knowledge, while a participant can decide whether a personal account remains private.

This also creates a supporting role in trade: a records desk may distinguish public reference from private note; a workshop may host a demonstration; a scout may annotate a route observation. Each service is bounded and does not own the barter contract. If the group asks for materials or labor, those costs use actual inventory/task owners.

### Endings

1. **Demonstration completed:** the participant teaches one step; capability changes only through the skill owner.
2. **Partial information shared:** the recipient learns a bounded fact; uncertainty remains visible.
3. **Referral made:** a new person may choose whether to respond; no trade is guaranteed.
4. **Public source used:** the player consults a shared record rather than extracting private knowledge.
5. **Information declined:** the player finds another route or leaves the need open.
6. **Conflicting accounts:** the player verifies, chooses a cautious path, or waits.
7. **Terms misunderstood:** the participants clarify or cancel before any item moves.

### Review conditions

Ask whether the “information” is personal, public, teachable, verifiable, and persistable. Make sure the participant can limit it, the player can decline, the exchange does not imply permanent ownership, and any downstream use goes through its actual system. If an information exchange is merely narrative, keep it as a conversation rather than inventing an item or save section.

## 446. Scarcity clock — trade now, wait for a route, or change the need

A peer offer often arrives just before a merchant shipment, a task deadline, or a weather window. The player can trade now at a personal cost, wait for a possible supply route, use an existing market, or revise the underlying task. These paths connect barter to the wider economy without making peer trade a replacement currency system. The relevant market, caravan, and travel owners determine what is actually known.

### Availability versus expectation

The player sees confirmed stock, current offer, estimated arrival, and rumor as different evidence. A peer's private item is available only if its owner currently offers it. A caravan's expected cargo is not stock until the trade owner records arrival. A market estimate does not bind a peer seller. A supporting faction can explain its forecast or list known requests, but cannot guarantee supply from another group.

If a route is delayed, the player can keep the peer offer open, renegotiate its expiry, choose an alternate item, or accept that the need waits. Any extension requires the offer owner and participant to agree. If the peer withdraws, the player cannot hold the item in a local cache. Waiting can save resources but lose the present opportunity. Trading now can solve an immediate task while creating a future shortage. These consequences come from owner state, not a scarcity score.

### Four branching routes

**Accept the present peer offer.** The player commits to the stated terms and settlement runs atomically. The task can proceed if its own prerequisites pass. Later arrival of market stock does not undo the deal.

**Wait for the caravan.** The player asks the seller to hold the offer, if reservation/expiry semantics exist, and waits on the actual route owner. If the caravan arrives, a new source becomes available. If it is delayed or rerouted, the player may accept a revised peer offer or defer again.

**Use the existing market.** The player compares the merchant route, fees, stock, and travel time. That system owns its quote and transfer. A peer barter offer remains separate and must be closed or left open by its own terms.

**Change the need.** The task owner may allow a substitute, a reduced scope, or another schedule. The player can pursue that route and withdraw the trade request. A trade offer cannot mutate a task requirement itself.

### Minor-faction role

A local broker or stores group may tell the player which route currently reports an item, help compare delivery windows, or host a handoff. It may have an incentive to keep exchanges within its network; the player can accept that service, trade directly, use the existing market, or decline. Its advice is labeled by source and freshness. It does not set a universal price, control private inventory, or override a major faction's published allocation policy.

A major faction's policy can constrain access to a resource. The supporting group may explain a permitted process or suggest an alternate supplier, but it cannot waive policy. The player can comply, challenge the rule through an established route, or operate within private ownership only where the current law/policy owner permits that. The barter panel is not a loophole for bypassing governance.

### Example: the late shipment

The player needs a spare seal for an active repair. A resident offers one for a borrowed tool. A current trade-route notice estimates that a merchant caravan may bring seals in several days, but its arrival is uncertain. A stores worker can verify that no shared unit is available. The player can accept the resident's terms; ask them to hold the offer through the expected arrival; seek another market; use an approved repair substitute; or let the repair wait. The resident may want the tool now and decline to hold the offer. The player may still agree to a loan instead of permanent transfer.

If the caravan is delayed, the player can reopen discussion only through a new or extended agreement that the resident accepts. If it arrives, market purchase is a separate transaction. The player who accepted the peer deal earlier may use the item for another task or keep it. The later shipment does not make the first exchange a mistake or invalidate it. The story acknowledges opportunity cost without scolding the player.

### Acceptance evidence

Verify source freshness, offer expiry, caravan status, market quote ownership, task substitute rules, shared stock, and save behavior while waiting. Test route delay, offer withdrawal, shipment arrival, and competing purchase. Confirm no item is counted in both peer and merchant stock. If a support group cannot see live route data, it must say so rather than predicting an arrival.

## 447. Cross-plan narrative slice — one request, one message, one settlement

This vertical narrative slice ties Plans 3, 4, and 5 together without merging their authorities. A survivor initiative creates the need; the communication surface carries an approved request; peer barter settles a specific exchange; the existing task owner consumes the item; and the goal owner records only witnessed work. Supporting factions can assist in bounded ways. Major factions may set a policy constraint, but do not adjudicate each private exchange.

### Opening: a personal goal needs a scarce part

A resident wants to complete a repair that could improve a shared routine. The goal owner records the initiative and the resident's chosen scope. The task owner exposes the required component and current eligibility. Inventory reports that shared stock is reserved elsewhere. The player can ask whether a substitute works, invite the resident to seek a peer exchange, defer the goal, or decline. The goal-holder chooses whether to continue and whether to make a request visible.

The resident selects one of three communication scopes: ask a known participant privately; post an anonymous request on a local board if supported; or ask a minor broker to locate an interested party. The player can help draft terms, but the resident approves audience and content. The message owner records the request; it does not reserve the part or bind anyone to sell.

### Route A: private offer with an explicit loan

A known resident responds that they can lend the component until their next scheduled use. The player sees the return condition and can ask the goal-holder whether the loan is useful. The goal-holder may accept, request a permanent transfer, choose an alternate method, or stop. The owner of the component independently accepts or declines the final terms. A task can be scheduled only after both sides agree and the transaction/custody owner can represent the loan.

If the loan is accepted, the player can choose whether to tell the wider group that the part is temporarily unavailable. A local notice may prevent accidental reservation, but it should not reveal the lender's identity unless they authorize it. The task begins after custody is confirmed. The part returns through its owner after the task, and the personal goal advances from a real completion event. If the task fails, the loan still follows its stated return conditions; no success callback is fabricated.

### Route B: public request and limited broker service

The resident chooses to post a narrow offer: what kind of component is needed, what they can exchange, and when they need a response. The board may reach some residents but not all. A supporting broker can match an offer with a participant who has publicly listed compatible terms. The broker cannot inspect private inventories or choose the winning counteroffer. The player may use its service, wait for direct replies, use the existing market, or close the request.

Several replies can arrive. One offers a temporary loan, one asks for a different item, and another is a public market quote. The player compares terms but does not rank personal valuations as objectively fair. The goal-holder can accept one, counter, or decline all. Unchosen respondents receive a closure only through the communication owner. Once one deal is accepted, the barter owner revalidates current custody and commits atomically. The board post then closes or updates, but it does not trigger settlement.

### Route C: no exchange, revised task

The player may decide that the component is too scarce to trade away, or the goal-holder may decide the repair is no longer worth the cost. The player can ask the task owner about a substitute, wait for shared stock, or close the initiative. A message can be withdrawn before response; if people have already seen it, the player may need to post a cancellation. No recipient is blamed for answering the request. The goal-holder can keep a smaller personal intention even if the repair does not proceed.

### Major-faction constraint, minor support

If a major faction's valid policy restricts transfer of this component, the policy owner states the rule and the player follows or challenges it through an existing route. A minor stores group can explain current reservation, a local broker can find a permitted alternate, and a repair team can assess substitution. None can waive the major rule. The player may choose to delay, renegotiate, or abandon the transaction. This supports faction depth through distinct responsibilities rather than an alignment check.

### Branch endings

1. **Loaned part, goal completed, part returned:** all three owners record their distinct outcomes.
2. **Permanent peer transfer:** atomic settlement completes; the task then uses the transferred item.
3. **Market purchase:** the peer request closes; the existing market owner settles its own transaction.
4. **Alternate task method:** task owner accepts a substitute; no peer transfer occurs.
5. **Offer expires unanswered:** the message ends accurately; the goal remains or closes by its owner rule.
6. **Resident withdraws:** the goal-holder changes their mind before settlement; no property moves.
7. **Policy blocks transfer:** support factions explain options but cannot bypass policy.
8. **Settlement fails:** neither side loses property; the communication thread says the exchange did not complete.
9. **Task fails after settlement:** the trade remains complete, while the item and goal follow their own owners.
10. **Player declines involvement:** the resident may pursue an available route or leave the goal open; no forced offer appears.

### Event and save boundaries

The initiative event must reference the resident and goal. The message event must reference the approved request and audience. The trade event must reference accepted terms and atomically settled property. The task event must reference its actual completion or failure. The goal owner consumes only the task event. These records can link by stable IDs but must not copy each other's mutable state.

Save after request creation, after response, after acceptance, immediately after settlement, and after task completion. On restore, no new request or duplicate transaction is created. If the task is canceled after settlement, the deal remains complete unless a separate return agreement is accepted. If the message expired, it cannot silently restart the barter offer. This vertical slice should be the first cross-plan integration candidate because it proves the feature boundaries clearly.

### Acceptance evidence

The package must show the player-reachable routes for all three systems, the current source owner for each fact, permitted/failure outcomes, event identities, save sections, and one trace per ending family. A successful dialogue flow alone is insufficient. The final visible result must show the resident's actual task, the component's actual custodian, the message's actual audience/status, and the initiative's actual disposition.

## 448. Settlement casebook — the offer changes while the player is away

A resident offers a pair of insulated gloves for a hand tool. The player inspects the offer and sees the listed item, current custodian, proposed recipient, and expiry. Before the player accepts, a day advances and the resident uses the gloves during an outdoor shift. The offer is now stale. This scenario is the central proof that an offer is not a reservation and acceptance must revalidate every side with the actual property owner.

### Preview and commitment are different moments

At preview, show that the resident currently holds the gloves and wants the hand tool. If item condition is owned and exposed by the inventory authority, show the supported condition description. If condition is unavailable, do not invent a durability percentage. The preview does not lock either item, change ownership, raise trust, or promise a future favor. The resident may withdraw before acceptance, and the player may leave the screen without consequence.

When the player accepts, the barter authority asks the asset owner to validate both sides against present state. The resident still has to hold the gloves; the player still has to hold the hand tool; both transfers must be authorized; receiving capacity must be valid if the inventory owner enforces it. A mismatch returns a reason with no partial movement. The offer remains available for negotiation only if its expiry and owner contract permit that. Otherwise it closes as stale and the player may ask for a new offer.

### Branch: the offered item was consumed

If the resident used the gloves and no longer owns them, acceptance fails before settlement. The game must not take a different pair from communal storage just because it has the same catalog ID. The resident can apologize, explain that the gloves were needed, offer a different item if one is actually available, or leave the matter unresolved. The player can accept the explanation, ask for another offer, or decline. The earlier offer is retained as history if the current barter owner supports history; it is not represented as a completed deal.

### Branch: the requested item changed hands

If the player gave away the hand tool to a repair crew, the resident's offer cannot still settle. The player can ask for a substitute, wait until a tool returns, propose a task in place of the item, or cancel. A substitute is a new negotiated term and must be visible before the resident accepts it. Do not automatically consume an item with a similar display name or substitute shared stock.

### Branch: receiving capacity is unavailable

If the player cannot receive the gloves because a current capacity rule rejects them, offer only owner-supported routes: ask whether the resident can hold them until a later time, decline the offer, or accept a different supported item. If no hold contract exists, do not create a magical escrow. The response can acknowledge the practical issue—“I can’t carry them today”—and preserve the resident's choice to withdraw.

### Branch: partial settlement is attempted

The implementation should make the partial route impossible. If a test or future adapter can demonstrate that one side moved before another failed, the feature is not ready for player exposure. The UI must wait for a single transaction result before showing completed. Do not compensate by placing a copy in a cache or issuing a second transaction; that would introduce another mutable authority and an uncertain recovery path.

### Closing beats

A successful exchange ends with both actual owners updated and one trade result. A stale offer ends with the exact failed precondition and a chance to renegotiate. A declined offer ends without a hidden trust loss. A player who loses capacity after viewing the offer receives a truthful message. On reload, the same stale status remains; the game does not reopen it as a new opportunity or replay the transfer.

### Acceptance evidence

Capture before and after owner snapshots for both participants, the offer identity, accepted term version, and the transaction result. Inject a change to each precondition between preview and acceptance: offered item removed, requested item moved, capacity filled, offer expired, participant departed, and one side no longer eligible. Each rejection must leave both inventories unchanged and produce a distinct response where the owner exposes the reason. Successful same-state acceptance must move each side exactly once across reload-safe event processing.

## 449. Item history and provenance — distinguish a gift, a loan, and a return

Players should understand what kind of exchange they accepted. The feature can use plain language and existing transaction terms rather than inventing a collectible provenance system. Provenance here means the terms and owner-confirmed movement needed to explain the active deal; it does not mean that every ordinary object gains a new lore history.

### Four contract shapes

**Transfer:** the item changes custodian once and no return is promised. The recipient may later gift or trade it only if the property owner permits that action.

**Loan:** the item changes custodian under an explicit return condition. The borrower sees who expects it back and the currently supported due event or time. If no owner can represent the due condition, keep the proposal out of scope instead of writing a pretend reminder.

**Temporary use:** the owner retains custody while another resident uses the item. This is valid only if current inventory/task owners can model use without a contradictory second owner. A dialogue promise alone does not prove temporary use occurred.

**Service or favor:** no object moves. The deal records a specific requested service only if the barter authority and task producer support it. The obligation closes from a real task or other owner-confirmed event, not from the player's statement that it is done.

### Narrative distinctions

A transferred item can receive a simple line such as “Mara took the wrench.” A loan says “Mara borrowed the wrench and agreed to return it after the pump check,” but only if both the borrower and due condition are stored. Temporary use says “The wrench stays with stores; Mara has permission to use it on the pump,” only if that permission is actually represented. A favor says “Mara agreed to help carry the cabinet,” not “Mara owes you,” unless the accepted terms use that language and the system can support the resulting obligation.

These verbs alter expectations. The writing review should reject copy that calls an outright trade a loan, treats a loan as a gift after one day, or says a player has a claim over a resident merely because they once exchanged goods. A resident can renegotiate a loan's due condition; the player can accept, decline, or ask to return the item. That is a new decision, not silent mutation of the original record.

### Item condition and identity

If a catalog entry represents stacks rather than unique instances, the proposal must not pretend to identify a specific glove or wrench without an instance owner. If two identical stacks merge, the transaction can still settle by valid quantity as the inventory API defines, while prose stays generic. If condition is recorded per instance, expose the actual supported condition and ensure the transaction preserves it. If condition is not available, avoid lines about a “new,” “worn,” or “repaired” item unless another authority supplies that fact.

### Dispute boundaries

A dispute can concern a missed return, an incorrect quantity, a claimed favor, or an unexpected custody change. It names the agreement and observed discrepancy. The barter owner can register the dispute; it cannot decide what a witness saw or alter the inventory to make one account true. The player may review the record, ask both parties for a response, propose a revised term, or leave the matter open. Any resolution that changes custody must call the real property owner.

### Save and continuity rule

The active trade record needs only stable references to the parties, terms, affected assets, event identities, and current disposition supported by the existing save owner. Do not copy full item stacks or create a second inventory snapshot. Restore should rebuild the view from the owner records and transaction references. If one referenced object is missing after a legacy migration, report an unresolved historical term and do not fabricate a replacement asset.

## 450. Negotiation branch map — the player's method matters

A meaningful offer supports several approaches whose consequences follow from concrete actions. The sample case is a resident who wants a replacement hand tool after lending their own to another crew. The player can offer a direct item exchange, propose a returnable loan, offer work through a supported task, ask a supporting group to verify the item source, or decline. These are not five virtue tests; they are different contracts, costs, and exposures.

### Route A: direct exchange

The player offers a verified spare item and asks for the resident's tool. Both sides see the precise item or quantity, the current custodian, and any supported condition. If the resident accepts, the two-side settlement contract applies. If they counter with another item, the player reviews the changed offer. If the tool is no longer available, the deal fails and returns to negotiation. Direct exchange is best when ownership is clear and neither party needs a return date.

### Route B: loan with a real return trigger

The player asks to borrow the tool and offers an item or favor as assurance. The resident may accept if the owner supports a loan and a return trigger can be named, reject because the tool is needed tomorrow, or propose another trigger. The UI explains whether the trigger is a task completion event, an agreed schedule, or another supported condition. “When I remember” is not a reliable due rule. The item remains the resident's property if the loan contract says so.

### Route C: task in place of property

The player can offer to help finish a repair or carry equipment instead of moving an item. The task owner determines whether the work exists, who can perform it, and when it is complete. The resident can accept that route, request a narrower contribution, or refuse because they need the tool immediately. The barter record links to the task event but does not duplicate task progress. If the task is interrupted, the obligation stays open or expires only as its agreed terms specify.

### Route D: source check through a supporting group

A stores volunteer can verify that a tool is surplus, borrowed, or assigned to a current job, where stores records support that distinction. A maintenance group can explain which tool variant the task actually requires. The player may use this information to reformulate the offer or decide no exchange is appropriate. The group is a witness or service provider; it cannot authorize a transfer outside the property owner or force the resident to accept.

### Route E: decline and preserve the relationship

The player may decline because the item is scarce, because the conditions are unclear, or simply because they do not want the deal. The resident can accept the refusal, ask whether a smaller offer is possible, or end the conversation. The exchange is closed without a hidden moral penalty. If the player wants to revisit it, a new offer must be created using current inventory and current consent.

### Branch graph and endings

These routes may recombine only after a new explicit offer. A stores check can precede a direct exchange; a failed loan proposal can lead to a task offer; a refused item transfer can end with no transaction. Once the transaction owner reports settlement, later task failure does not retroactively undo the property transfer. A separate return agreement or compensation offer can address the new situation if both parties agree. This separation makes consequences legible and prevents a quest ending from rewriting history.

### Review questions

Can a player reach each route with the current interface? Does each route state who owns the item before and after? Is a favor defined in language its producer can verify? Can either participant decline without being penalized by alignment? Are counteroffers new terms rather than hidden substitution? Can every failure be shown without partial settlement? Does a supporting faction add knowledge or service without deciding for a major owner? Does the dialogue remain coherent if no supporting group is present? Answer these before implementation begins.

## 451. Shared exchange timeline — negotiation, performance, and aftermath

An exchange is more than a confirmation click. Its narrative timeline begins with a need, shows the exact terms, distinguishes consent from settlement, and lets later events update only the part they own. This case uses a portable lantern offered in return for help carrying a water crate. The example is valid only if inventory can transfer the lantern atomically and an existing task producer can confirm the carrying work.

### Moment one: the need is stated

The resident says the lantern is needed for an evening repair. The player may ask whether a loan would work, offer a permanent transfer, offer to help carry the crate instead, ask whether the resident has another route, or decline. The resident can answer with their own preference; their faction does not dictate it. If they say the lantern is family property and will not be traded, that closes this offer. It does not make all barter unavailable.

### Moment two: specific terms are assembled

For the carrying route, specify the task, expected participant, timing if known, and whether any item changes hands. If the player also proposes a small supply, make it a separate term with its own owner and acceptance. The resident may accept the task only, accept both terms, counter with a different task, or reject the bundle. The interface should make clear that accepting a discussion is not accepting a final offer.

A bundle can make a deal feel human, but every term needs a definite owner. If one item transfer cannot be committed atomically with the second item, do not include both in one settlement. If one side is a favor and its task contract cannot be represented, keep the favor as a conversation possibility until a separate approved capability exists. The story must not outrun the system.

### Moment three: acceptance is explicit

Both sides review a concise summary: “Lantern, transferred to you; carry one water crate with Jori after the next shift change.” Each participant can accept or revise. If the player accepts, the final command revalidates the item, capacity, task eligibility, schedule, and offer expiry. A changed fact returns the exchange to the appropriate negotiation step. It must not silently accept the item while dropping the work condition.

### Moment four: property and work resolve separately

If the lantern transfers at acceptance, its actual owner changes once. The carry task begins only when the task owner confirms it is available and scheduled. If the task is blocked, the lantern trade remains complete if the explicit contract says immediate transfer; the player can negotiate a separate return or remedy. If the accepted agreement makes the transfer conditional on completed work, the contract needs a supported atomic/conditional settlement operation. Without that operation the conditional version remains unshippable.

When the task completes, the task owner emits its real outcome. The barter owner closes the favor from that event only if the agreement links to it and the event satisfies the agreed condition. If the crate is moved by a different worker, the resident can decide whether that meets the accepted terms. The system should not assume equivalence. If the task is canceled, the barter owner can mark the obligation unresolved or canceled according to the accepted terms, without undoing unrelated work.

### Moment five: memory is narrow and useful

The resident can thank the player for carrying the crate, ask about the lantern, or say that the arrangement worked but the repair remains pending. The conversation should not award a broad trust point for a single deal unless the existing reputation contract specifically defines that result. If there is no tracked relationship change, the line can still convey relief or disappointment without altering saved state. The player can choose whether to negotiate again.

### Endings and re-entry

1. **Task-only agreement:** the lantern stays with its owner; the player completes the accepted carry task.
2. **Immediate transfer plus task:** ownership changes at acceptance and the task remains separately visible.
3. **Item-only transfer:** the player receives the lantern with no hidden work obligation.
4. **Counteroffer accepted:** new terms are visible and accepted as a new version.
5. **Stale offer:** a precondition changed; nothing transfers and no task starts.
6. **Task blocked:** property follows the explicit settlement terms; work reports its own status.
7. **No deal:** both sides decline without penalty and can revisit only through a new offer.

The timeline preserves the difference between promise and action. It also creates callbacks tied to the chosen contract, making this system legible even when the player does not complete every task.

## 452. Supporting-faction exchange services — information and witness, not command

Supporting factions can make a peer exchange more nuanced when they provide a service the player can choose to use. They do not set private item ownership, adjudicate disputes automatically, or replace the major stock owner. The following roles add routes and texture while preserving the player's position as negotiator.

### Stores clerk: verify whether an item is actually surplus

A clerk may confirm that an item is in shared stores, personally held, or assigned to a known task if the inventory and assignment owners expose those facts. The player can ask for a check before making an offer. The clerk can say “I can verify the shelf, not what someone keeps in their pack.” This creates an informed branch without letting the faction inspect or seize private property. The player still makes the offer and the actual property owner still settles it.

### Repair crew: recommend an appropriate substitute

A maintenance group may advise that a wrench of one catalog category is insufficient for a particular task, or suggest a safer tool if the current recipe/task data supports it. The player can request a substitute offer, accept the original limitation, or abandon the exchange. Advice is not a grant of stock. If the substitute is unavailable, no phantom item appears.

### Ledger volunteer: witness the terms

A ledger volunteer can help the participants write a clear record of an accepted loan or task obligation if the barter owner supports such a record. Both parties review the terms, and either can ask for a private agreement instead. A witness records what was accepted; they do not guarantee performance or later decide the dispute. If one party did not agree to the note, the ledger cannot publish it as a contract.

### Route team: clarify delivery cost

A route group can report that a physical exchange point is accessible from a verified path or that its last survey is out of date. The player can schedule a handoff, ask for another check, meet at a supported in-shelter location, or cancel. The route team cannot guarantee travel safety. If the game has no supported delivery or location command, present the group’s information as flavor and do not pretend that a delivery is scheduled.

### Care circle: protect a personal boundary

A care circle can remind the player that an item is personally held and that a public offer may reveal who needs it. The player can switch to a private channel if the communication owner supports it. The circle cannot give consent on the resident's behalf, and its involvement should not reveal the resident's reason for keeping the item.

### Multi-service route

A player might ask stores whether a tool is surplus, consult the repair crew about its suitability, then negotiate a private loan witnessed by the ledger volunteer. Another player might go directly to the resident and make a simple item-for-item trade. Both should be playable. Services are optional, and each one requires a concrete player action. If a support group declines, the trade remains negotiable through ordinary participants when current ownership is clear.

### Faction and major-authority boundary

Major factions continue to own their formal inventories, caravan stock, market rules, or shelter policy. A support group can explain a route, witness agreement, or clarify a record. It cannot discount a major faction's goods, transfer stock without the owner, or veto a personal agreement between eligible residents. A major faction should not automatically override a resident's consent to trade personal property unless current policy and property ownership explicitly grant that authority.

### Branch review

For each supporting service, name its information source, exact offer, player-request action, known cost, refusal path, output event if any, and response after reload. Remove any branch that depends on universal faction approval. Confirm the major faction still matters where it owns the relevant stock or rule, while a supporting group contributes a narrow capability and the two residents remain the people who accept or decline the deal.

## 453. Candidate side-story — The Lamp at the Packing Table

This optional barter story follows two residents and three different possible needs. A packing-table lamp is no longer bright enough for late work. A neighbor has a spare lamp but needs a replacement latch for a cabinet. A small repair crew has a latch but expects the current one to be returned after inspection. The player can broker a direct exchange, a loan, a task, or no agreement. The story uses only offer, atomic settlement, and favor/dispute closure; it creates no fourth feature pillar.

### Chapter one: establish ownership and need

The player hears about the dim lamp while checking the work area. The first resident may offer it outright, offer a loan, say it is shared property, or decide to keep it. Those statements must be checked against the current property owner. If the lamp belongs to shared stores, the peer barter route cannot treat the resident as its owner. The player can ask stores for a release, ask the resident about a personal lamp, or leave the request open.

The cabinet neighbor explains the latch need. They may trade, lend, request help installing it, or decline any exchange because the cabinet must remain usable. Their task cannot be inferred from dialogue. The player can compare terms, ask a supporting repair group which latch fits, or stop. The repair group can advise on compatibility but cannot give away another group's property.

### Chapter two: a support group discovers a constraint

A stores clerk checks the requested latch and finds it is assigned to an inspection. This fact changes the offer path. The player can wait for the inspection, ask the repair crew whether an alternative exists, offer a task in exchange for temporary use if owners permit it, or abandon that route. The crew may say that the current latch must be returned before it can be released. The clerk has provided information, not vetoed a personal item exchange.

A ledger volunteer offers to write down a loan term if both residents agree. One resident wants the loan back at the end of the next shift; the other prefers an event condition, “after the cabinet test.” The player can choose either supported term, ask the residents to settle on one, or leave without a deal. If the barter owner cannot represent a due event, the event-based term is not accepted in the feature. The writing remains candid about that limitation.

### Chapter three: the player chooses a contract

**Direct trade:** the lamp changes hands for the latch, if both items are currently personal property and the owner can settle both atomically. No installation task is implied.

**Loan and return:** the lamp is borrowed until a supported due condition, while the latch remains in the repair crew's custody until released. This route is valid only if both owners can model the two distinct custody arrangements. Otherwise it stays out of scope.

**Task exchange:** the player offers to help with the cabinet test in place of the latch transfer. The task owner validates the work; the residents define whether that task satisfies their agreement. If either person does not accept the task as equivalent, the player must renegotiate.

**Temporary wait:** the player asks the residents to revisit once the inspection finishes. The current offer may expire or remain open according to its actual terms. A reminder appears only if the current system stores one.

**No agreement:** the player declines or either resident withdraws. No goods move and no hidden trust loss appears. The dim lamp remains a practical inconvenience rather than a forced quest failure.

### Chapter four: interruptions test the terms

A shift change delays the cabinet test. If the exchange was direct, the property transfer remains complete and the task is separate. If the deal was a loan, its due condition determines whether the loan remains open. If the terms were conditional and cannot be represented, that route should never have been accepted. The player can ask for a revised due event; both residents must agree to the changed version.

If the latch fails inspection, the repair crew retains its own item and reports the result. The player can offer a new route, locate another compatible part through a current inventory owner, or close the story unresolved. No duplicate latch should appear as compensation. If the lamp is returned late, the barter owner records a dispute based on the original agreement and actual custody facts. The ledger volunteer can witness the agreement, not decide who is responsible.

### Chapter five: close the deal in human terms

The story can end with the packing table lit, the cabinet repaired, only one of those outcomes, a pending loan, a renegotiated task, or no exchange. The player may ask whether the arrangement worked, but each resident speaks only for their own experience. A working lamp does not prove the cabinet test succeeded. A completed task does not prove that the item was returned. A closed barter record does not require every relationship to be repaired.

### Branch callbacks

After a direct trade: “The lamp's yours now. I hope it lasts through the night shift.” Use only after confirmed settlement.

After a loan: “I can bring it back after the inspection, if that still works.” Show the due condition and allow renegotiation; do not claim a return happened.

After a task exchange: “You carried the cabinet while I checked the latch.” Use only if both task and participant records confirm it.

After no deal: “I'll work earlier, while there's still light.” This line lets a resident adapt without converting refusal into defeat.

After an unresolved delay: “The crew hasn't released the latch. I'll wait before promising it.” This records the real constraint and leaves a possible future route.

The story is successful when the player can see how their choice affected custody, work, timing, and later dialogue, including when no goods changed hands.

## 454. Dispute response ladder — escalating process without punitive alignment

A dispute should let the player respond to a concrete mismatch while keeping its remedy proportional. It is not a morality quiz and does not need a tribunal system. Use the existing barter dispute owner and actual property/task owners. A resident says they returned a borrowed lamp; the other party says it never came back. The current records show that a return event was not recorded. That fact alone cannot prove what happened in the room.

### Step one: read the agreement

The player can review the accepted terms, stated due condition, item identity or quantity, relevant task events, and current custodian from their owners. If a record is missing, show that it is missing. Do not silently choose the account with the higher faction rank or friendship. The player can ask both participants for a statement, ask a consenting witness what they personally saw, or leave the dispute open.

### Step two: separate facts from claims

A participant may say “I left it on the shelf.” The system can record that statement only if the dispute API supports it; otherwise it is dialogue, not a new evidentiary state. A witness can say “I saw a lamp on the shelf before shift change,” but not infer that the correct lamp was returned. A stores clerk can inspect shared inventory, but cannot determine a personal handoff that its owner did not record.

The player may ask for a practical check: inspect the work area, compare an item instance where supported, or ask whether someone has already moved it into another task. If the current game lacks such interactions, do not write success lines based on them. The dispute can remain unresolved, which is better than an invented investigation mechanic.

### Step three: offer a remedy

Where both participants agree, the player can propose returning the item, extending the loan, replacing it with an accepted item, converting the agreement into a task, or closing it without remedy. Every proposed asset change uses atomic settlement. A remedy must be a new accepted term, not an automatic transfer ordered by the UI. If neither party agrees, the player can preserve the dispute as open or close the player-facing conversation while leaving its status unresolved if the owner permits.

### Step four: prevent repetition without creating a reputation machine

The specific dispute may be referenced when the same item or agreement recurs. Do not globally label one person dishonest or one player unreliable. A future offer can present clearer terms, require a witness if both participants want that, or avoid a loan altogether. Those choices arise from a remembered agreement and explicit preference, not a hidden penalty tier.

### Possible closures

- The item is located and returned through the property owner.
- Both parties agree to extend the loan with a revised due condition.
- Both parties accept a replacement trade and atomic transfer succeeds.
- A task event fulfills a revised favor that both accepted.
- The agreement is closed with no remedy by mutual choice.
- The dispute remains open because the evidence or authority is insufficient.
- One participant withdraws from further negotiation; no forced settlement occurs.

### Content and fairness review

For each closure, the writer identifies which record supports it and avoids attributing motive that is not observable. The player can make a generous offer, a strict offer, or no offer. The system shows who accepted and what actually changed. Neither “forgive” nor “demand return” becomes a universal good/evil branch. A missing item remains a practical loss; the narrative can acknowledge the consequence without implying a moral verdict.

## 455. Negotiation approaches — five playable methods for the same need

The player should be able to make an exchange feel like their own interaction style. These are approaches, not character classes. Any player may move between them, and no method is a universal best answer. Use a water filter cartridge as the requested item and a resident's spare hand cart as the possible exchange.

### Ask-first negotiator

The player asks what the resident needs before proposing a term. They may learn that the cart is needed for a scheduled delivery, that the resident would loan it if it came back before dusk, or that they do not want to exchange it. This route surfaces preference and timing. It does not guarantee a deal or unlock a better price. The player can accept the limit, make a different offer, or stop.

### Clear-terms negotiator

The player states exact item, quantity, custody change, return condition if any, and expiry. The resident can accept, counter, or reject. This approach reduces ambiguity but may feel formal. A ledger volunteer can help only if the player requests it and both parties consent. A clear summary is not itself proof that the terms were accepted.

### Needs-based negotiator

The player describes the filter need and asks whether another solution is acceptable. The resident may offer a smaller cartridge, a loan, or advice about a current compatible item. Any substitution must pass catalog and inventory validation. If no compatible item exists, the resident may decline. Dialogue can show that they are trying to help without pretending that a resource changed.

### Task-based negotiator

The player offers a defined task in exchange. A task owner validates eligibility and confirms whether the task can be scheduled. The resident can say that work would help, but cannot accept a task contract the barter authority cannot store. If the task is too vague—“help later”—the UI asks for a supported definition or returns to item terms. This is a meaningful route only when the event can close it honestly.

### No-deal negotiator

The player decides the filter is too costly or the hand cart cannot be spared. The player declines without insulting the resident; the resident can accept, ask for a smaller offer, or end the conversation. No deal is a playable ending. The player can later approach a market owner, search stores, or change the underlying plan through current systems.

### Negotiation sequence examples

Ask-first may lead to a loan proposal and then to clear-term review. A failed item substitute may lead to a task route if a real task is available. A resident can counter with a smaller quantity; the player can accept the counter, return to needs-based discussion, or decline. A support group can verify item compatibility at any point, but it cannot authorize the resident's consent. Every new counteroffer changes terms and requires renewed acceptance from both sides.

### Tone and consequence

After the player asks first, a resident might say, “The cart has one good wheel left. I can lend it until the water run.” After clear terms, they might say, “That reads right. Let me check the day.” After a task proposal, they may answer, “I need the cart sooner than I can wait for that.” After no deal, they can say, “Fair enough. I asked because I was short, not because you owe me.” These candidate lines express voice without awarding moral points.

The consequence comes from the contract: borrowed cart due at an agreed time; filter cartridge transferred; task accepted; offer expired; or no agreement. A friendly tone does not transfer ownership. A tense tone does not invalidate a properly accepted transaction.

## 456. After the exchange — consequences beyond the transaction receipt

When the trade closes, the player should see the immediate property result and the next useful decision. The exchange can also shape later scenes through concrete context: a tool is now available for a task, a loan return is due, a recipient asks for a separate favor, or no deal leaves the player seeking another route. Avoid generic “relationship improved” summaries unless the current relationship owner emits that fact.

### If both sides received what they asked for

The transaction receipt identifies the accepted terms and actual item movement. The resident may be relieved, practical, or already focused on the next task. If the traded item solves a real need, its downstream owner can expose that effect. If it does not, the narrative should not claim success merely because the bargain succeeded.

### If the terms completed but the need remains

The resident may say that the cartridge fits but will not last through the full week. The inventory exchange is still complete; the need remains with its own owner. The player can make another offer, search an authorized stock source, ration current use if a real system supports it, or leave the need open. No automatic free replacement is awarded to make the story feel resolved.

### If a loan is returned

A return occurs only when the real custody owner records the item returning. The barter owner closes the agreement from that event. The player can see “returned” and the due result if the system supports one. If the item is late, the record can say that the due condition passed before return; it need not apply a daily trust penalty. Both parties can renegotiate or close the matter.

### If a loan is not returned

A missed due condition can open a dispute or prompt a conversation, according to the existing owner contract. The resident may ask the player to help find the item, propose a later date, or state they are done lending. The player can investigate through existing inventory records, offer a remedy, or decline. The game should not create an item copy or seize a similar item from stores.

### If the transaction succeeds but a task fails

If the deal transferred an item immediately, later task failure does not reverse it. If the deal explicitly conditioned the item on task completion, that contract must have a supported conditional settlement path before implementation; otherwise it remains a proposal-only route. The player can negotiate a fresh remedy. Clear temporal boundaries are essential to make both task and trade owners trustworthy.

### If a participant leaves

If someone departs while an offer is pending, the barter owner must return a valid status or close according to its current contract. A pending private offer cannot be accepted by an assumed replacement. If a loan is active, the existing owner must define the valid handoff or unresolved status; do not invent a custodian. The player can receive a factual message and decide whether to ask a major authority for a permitted recovery route.

### If the player abandons the conversation

Closing the panel does not mean acceptance. An offer may continue until its actual expiry, or the owner may close it if that is current behavior. The player can return and view its status. If it expired, any new bargain requires a new offer based on current inventory. This prevents surprise settlement and supports a deliberate, low-pressure negotiation style.

### Aftermath authoring check

For every ending, write separate sentences for transaction status, item custody, task status, relationship fact if any, and next available player action. Do not compress unrelated outcomes into “deal complete.” Review the copy with all involved owners unavailable in the UI; the interface must still avoid claiming results it cannot read. The player should understand both what changed and what did not, without reading a technical event log.

## 457. Scarcity choices — a scarce offer is not a coercion shortcut

An item can be urgently useful and still belong to someone else. The barter loop should let the player weigh that need without turning scarcity into forced consent. A resident has the only known replacement filter. The shelter's current stores show no matching item. The player can negotiate, seek another supported source, change the plan, wait, or accept that the need is unresolved.

### State the actual shortage

Inventory can establish that no matching filter is currently available in the relevant shared stock. It cannot establish that no resident owns one unless personal inventories are included in that authority and the player has legitimate access to the information. The interface should say “none in stores” rather than “the last filter in the shelter” unless the latter is verifiably true.

### Ask without pressure

The player may ask whether the resident is willing to trade, lend, or share the filter. The resident may refuse because they need it, offer a partial quantity where stack rules permit, ask for a specific return condition, or suggest another route. The player's visible need does not obligate the resident. A repeated ask can remain possible only if the resident permits another offer; the game should not use urgent music or dialogue repetition to make refusal feel invalid.

### Compare permitted alternatives

The player can ask a major stock owner about a requisition, contact a support supply group for a delivery estimate if one exists, offer an alternate item, or revise the underlying task. The major faction may have an authorization rule. The supporting group may know about a route or compatible substitute. The resident's personal offer remains a separate negotiation. A player can pursue several routes without a coalition score or a faction-wide approval roll.

### Wait or alter use

If the need can be delayed, the player may wait for a known restock or delivery event. If the task has a safe lower-supply variant, the player can select it through its owner. Do not create rationing, partial efficiency, or degradation behavior unless an existing system owns it. The narrative can show inconvenience: the task stays open, another task takes priority, or the resident keeps the filter.

### Resolve without a forced winner

The player may receive a filter by accepted transfer, borrow one under a supported due condition, receive an authorized shared-stock issue, use a verified substitute, defer the task, or leave the need unresolved. Each result changes a different owner state. If the deal fails because the resident declines, there is no transfer and no hidden punishment. If the player secures one elsewhere, the resident's refusal remains respected.

### Review questions

Does scarcity text describe only known stock? Does the resident have a meaningful refusal? Is every alternate source owned by a real faction or inventory owner? Are delivery time and compatibility facts verified? Can the player wait or revise the task? Does the game avoid calling a personal item “community property” without authority? Can a failed bargain close without a moral label? If any answer is no, the branch needs redesign before content polish.

## 458. Boundary with markets and shared stores — peer choice stays peer choice

Peer barter must remain distinct from merchant restock, caravan prices, shared stores, and communal allocation. A resident's personal offer is not a market listing. A market owner may set formal prices and stock. A stores owner may issue shared property. The barter owner may coordinate voluntary exchange between eligible residents. The player should always know which route they are using.

If a resident says that a tool came from shared stores, verify its current custodian before accepting a peer trade. If a merchant offers the same catalog item, the player can compare price and availability through the market panel; the barter plan does not copy that inventory. If shared stores release an item, that action follows its owner and can then become a resident's property only through an explicit transfer. Do not let a peer agreement bypass allocation rules.

These boundaries create useful choices: pay the merchant if stock and price are available; request a permitted stores issue; negotiate a personal loan; offer a task; or wait. Each route has a distinct authority and consequence. A support faction may explain the difference, but does not merge the ledgers. The transaction receipt identifies a peer deal and references only the actual property movement it caused.

## 459. Value is personal — avoid fake equal-price negotiation

A peer exchange does not need a universal conversion rate for every item and favor. The parties may value timing, custody, privacy, or convenience differently. The UI should present concrete terms and let each participant accept or decline rather than generate a hidden “fairness” score. A player can offer more than the resident requested; the resident can accept the generosity, counter, or refuse because they do not want extra goods.

If a catalog provides market values for an item, those values may help explain formal trade or scarcity, but they do not prove what a resident should accept in a personal exchange. A task favor has no automatic item price unless a current system owns that rule. The player can compare alternatives—market purchase, shared stores issue, personal loan, task exchange—while each uses its own authority. Do not show “bad deal” warnings based on an unapproved valuation model.

After settlement, report exactly what each side gave and received. If one participant later says the exchange felt unfair, that is a new conversation or dispute only if the current owner supports it. It does not retroactively void the atomic transfer. The player may negotiate a remedy, decline, or let the relationship remain awkward. This preserves both the transaction boundary and the human truth that people can make a valid agreement they later regret.

## 460. Transaction result card

After settlement or rejection, show the accepted term version, each item's actual movement, any remaining favor condition, and the next valid action. If nothing moved, say that clearly. If a task remains open, keep it separate from the completed transfer. If the terms were declined, leave no ambiguous “pending success” label. A support witness may be listed only if both participants accepted that role.

The player should be able to reopen the result after save/load and see the same custody facts from the property owner. A disputed or missing historical reference is shown as unresolved; it is never repaired by copying an item into the transaction record. This card closes the immediate interaction while preserving legitimate routes for return, renegotiation, or no further action.

## 461. Close a no-deal branch cleanly

When either participant declines, close the offer according to its current expiry and consent rules. Show that no property moved and no task began. The player may ask about another supported route, leave the need open, or return to ordinary play. Do not create an automatic counteroffer, repeat the same prompt, or apply a hidden trust penalty. The resident remains available for unrelated conversations and future agreements they choose to make.

## 462. Final settlement consistency pass

Read the offer, confirmation, receipt, and later callback as one chain. Item identity, quantity, custodian, and obligation must agree at each stage. If the agreement changed, show a new accepted version. If it did not settle, avoid success language. Keep task completion and property movement independently traceable.

The player can always stop negotiating before acceptance and keep the current inventory unchanged.

## 463. Candidate side-quest — The Blue Ledger

This optional peer barter story explores the difference between a remembered promise and an accepted agreement. A resident has written several informal exchange notes in blue pencil: a borrowed file, a returned lantern, and an offer to repair a table leg. Another survivor says one note is unfinished. The player may inspect the terms, ask participants, use a witness service, propose a remedy, or leave the matter open. The quest never turns a missing note into proof of guilt and never creates a universal barter reputation score.

### Opening: one note, three possible meanings

The note reads: “File back after hinge work.” It does not specify who borrowed the file, which hinge task, or whether the work is complete. The current barter owner may hold a linked offer or may have no record at all. The player can inspect the available record, ask the note's author what they meant, ask the claimed borrower, or decline to investigate. A ledger volunteer can help compare the note with an accepted agreement only if both parties consent to that review.

If there is a matching accepted agreement, the player sees its parties, terms, due event, and status as owned by the barter system. If there is no matching transaction, the paper remains an informal note. It cannot be promoted retroactively into a binding loan. The player can offer to help the residents form a new agreement, but only on current terms and with current consent.

### Chapter one: establish the file's current custody

The player can ask the inventory owner whether the file is in shared stores, held by one participant, assigned to a task, or not located. The answer may narrow the search but does not establish how it arrived there. If it is personal property, a resident can decide whether to return, loan, transfer, or retain it. If it is assigned to a current job, a major stores authority or task owner may control release. If the location is unknown, the player can ask a permitted source to check or leave the dispute unresolved.

The player can separately ask whether the hinge work happened. A task producer confirms completion or interruption. The hinge can be repaired by another file or method, so completion does not prove the blue pencil file was used or returned. An item owner and a task owner answer different questions.

### Chapter two: let each participant define the disagreement

The note's author may say they expected the file that evening, that “after hinge work” meant the next day, or that they never agreed to lend it. The claimed borrower may say they returned it to the shelf, still have it, or understood the note as a gift. The player can record each statement only through an owner that supports dispute evidence; otherwise dialogue remains dialogue and must not become a fabricated case file.

The player can ask a witness what they directly observed. A witness may remember seeing a file on the table, but cannot identify it as the same item unless the item authority supports instance identity. The ledger volunteer can explain that the note is ambiguous; they cannot decide whose memory is true. The player may choose to stop at this point rather than force a verdict.

### Chapter three: propose new terms

The participants can create a clear return agreement, convert the current arrangement into an outright transfer, exchange a different tool, offer a defined task, or close the matter without remedy. Each option is a fresh accepted term. The player can ask the resident what would help, but cannot impose a trade as compensation. If a task is proposed, its owner confirms eligibility and completion. If an item is exchanged, the barter system and inventory owner validate and settle atomically.

A supporting stores clerk may confirm that a replacement file exists in shared inventory. The clerk cannot make it a personal replacement without an authorized issue. A repair crew can suggest a different tool if the task data supports it. A major authority can decide whether shared stock may be released, while the two residents decide whether they accept the proposed remedy.

### Chapter four: time and performance

A new agreement may use a specific return time or a task event as the due condition. The parties choose what they can actually represent. If the current system can track a completed hinge task but cannot bind it to return, do not offer that conditional contract. Use a date/time or other supported due field, or keep the story at discussion level. The player sees when the agreement is active and what counts as fulfillment.

The work may complete and the item may return; the work may complete while the item stays out; the item may return before the work; the task may be canceled; or the item may remain missing. These are independent facts. The barter owner handles the agreement, the task owner handles work, and inventory handles custody. No one result overwrites the others.

### Chapter five: close the book without rewriting history

If the file returns, the agreement can close with a return fact. If the participants choose an outright transfer, it closes as a new transfer. If they cannot agree, the dispute can remain unresolved or close without remedy where the owner allows. The player may invite the ledger volunteer to record a new, clearer deal, but old ambiguity remains part of history. No after-the-fact note can prove that a past exchange was accepted.

### Ending families

- **Verified return:** inventory confirms the file returned and barter closes the linked agreement.
- **New loan:** both participants accept clear terms and a supported due condition.
- **Outright transfer:** the file changes custody once and no return is expected.
- **Task remedy:** a specific task is accepted and fulfills its own linked condition.
- **Stores release:** a major stock owner issues an alternative and the parties negotiate from current state.
- **No replacement available:** the residents keep the dispute open or close without remedy.
- **Unresolved testimony:** statements differ and no owner can verify the missing step.
- **No investigation:** the player declines involvement; existing owners and parties retain their current state.

These outcomes change according to whether the player checks custody, separates task facts from item facts, invites a witness, proposes a supported remedy, and asks both participants to accept. There is no good/evil score and no automatic penalty for refusing to arbitrate.

## 464. Offer construction workshop — every term has a visible owner

Offer composition is a player-facing negotiation process. The screen should help a player state a deal without implying that any term is guaranteed. Use a distinct preview stage and acceptance stage. Each term is optional, and either party can decline or revise.

### Party and capacity

Identify both participants and their current eligibility. The roster and relationship owners may constrain whether the interaction can occur; the UI should not infer eligibility from faction membership. If a participant is absent, departed, or unable to receive the offer, show that fact and keep the offer unsent. The player may choose another permitted time or close the draft.

### Property term

For each item, specify catalog identity, quantity or instance as current inventory permits, source custodian, intended recipient, and whether custody is permanent or temporary. If item condition is supported, show the current fact. If the item is shared stock, route to its owner. If it is borrowed from another party, the offerer may not have permission to trade it. A preview does not reserve stock.

### Obligation term

A favor needs an action the task or barter owner can recognize: carry a named crate, complete a specified repair stage, return an item after a supported due event, or another supported obligation. Vague phrases such as “help later” can be discussed but cannot be settled as a durable task unless the system can represent them. If a favor has a date or deadline, the due rule must be defined by current APIs and displayed before acceptance.

### Timing and expiry

Separate when the offer is available from when a favor is due. The offer may expire after a day while the accepted loan remains active for longer. If offer expiry is not supported by the current barter owner, do not imply one. If a date advances during preview, acceptance revalidates the offer. A stale offer returns to negotiation without taking either side's property.

### Audience and privacy

Most peer offers can remain between the participants. A witness, ledger, or public notice is a separate explicit choice, requiring the relevant communication route and participant permission. The existence of an offer should not be broadcast to a faction or the shelter by default. If the player requests a witness, show who will know and what record is created.

### Acceptance sequence

1. The player assembles a draft from visible current facts.
2. The resident reviews terms and may accept, reject, or counter.
3. The player reviews any changed version.
4. The resident confirms the final version if the owner requires a second confirmation.
5. The barter owner revalidates and invokes atomic settlement or records the accepted obligation.
6. The result card shows what moved and what remains due.

An offer is not acceptance. Acceptance is not settlement. Settlement is not task completion. Each step has a distinct status, owner, and player-facing verb.

### Counteroffer behavior

A counteroffer creates a new version with changed terms, not a silent edit. If quantity changes, revalidate that stack. If a favor changes, show the new task or due condition. If a witness is added, repeat the audience preview. If the original player is offline or has left the screen, the offer remains pending or expires according to current contract; it must not auto-accept. Both participants can leave without penalty.

### Design examples

A resident offers two bolts for a hand cart repair: the player can ask whether one bolt is enough, decline the item and offer a task, or accept the stated quantity. A borrower requests another day for a lantern: the player can extend the due time if supported, request its return, or accept a new exchange. A resident wants help carrying a crate: the player can assign a valid task, suggest a smaller supported carry, or explain that no worker is available. Every example uses the same offer/favor/settlement boundaries.

## 465. Dialogue and receipt bank — make the contract human and precise

The barter loop needs distinct lines for what each participant wants, what they accept, and what actually happened. Candidate lines below require canon review and should be assigned to appropriate character voices rather than generated from one relationship score.

**Opening need:** “I can lend the cart through the morning run. I need it back before the floor gets wet.” This states preference and timing, not a completed agreement.

**Clarification:** “Do you mean the small wrench, or the one in the blue case?” The player must select a catalog identity the owner can distinguish.

**Ownership boundary:** “That wrench is on the stores list. Ask the clerk before offering it.” This prompts an authorized stock route.

**Loan preference:** “Use the lantern tonight. Bring it back after the next shift, if the records can mark that.” If that due condition is unsupported, the UI offers an alternative or leaves the exchange unaccepted.

**Counteroffer:** “I can give you the bolts, but I need two, not one.” Recheck quantity and confirm the revised version.

**Refusal:** “No, I need the cart for my own work.” No hidden disapproval is attached.

**Stale inventory:** “The hand tool moved to the repair crew. That offer is out of date.” The player may create a new draft but cannot settle the original against a substitute.

**Partial terms:** “The item moved. The carry task has not happened yet.” Use where an immediate property transfer and separate task are explicitly the accepted contract.

**Completed favor:** “The crate is at the dry shelf now. That's the work I agreed to.” Show only after the task owner confirms the move.

**Unresolved return:** “I still don't have the lamp back. I don't know where it went.” This is a participant report; the inventory owner remains authoritative for custody.

**No deal:** “Let's leave it there. I can ask again if I find another cart.” The player can end the interaction without closing unrelated relationships.

**Receipt:** “Trade completed: two bolts moved to you; one hand cart moved to Rada. No task was included.” The exact assets and quantities come from the transaction result.

The receipt should avoid lyrical language that obscures custody. Character dialogue may carry warmth, irritation, humor, or hesitation, but the state summary remains direct. Accessibility and translation review should confirm that item names, quantities, due events, and result status remain readable in every supported display mode.

## 466. Candidate side-quest — The Hand at the Sluice

This optional barter story follows a request for a sluice handle during a week of repair work. One resident has a personally owned lever that fits the service panel; a maintenance group has a shared handle that is assigned to a different job; a major water authority controls the actual flow schedule. The player can negotiate a personal loan, request a shared-stock release, offer a task, wait for the other job, seek a supported substitute, or decline. The quest creates no new water authority and never lets a peer trade decide system-wide allocation.

### Opening: a practical shortage, not a faction crisis

The resident tells the player that their lever fits the old panel. The task owner says a panel adjustment is needed, but the exact item requirement must be verified against current task data. The player may ask to see the item, ask the maintenance crew whether it fits, request a stock check, or start a peer offer. The resident can say the lever is personal, shared, borrowed, or not for trade; inventory and property owners verify what they can. The resident's statement remains meaningful but does not override ownership records.

A supporting repair group can identify the tool shape or advise whether an alternative is safe. If the group does not know, it says so. The major water authority can explain when the sluice can be worked on, but does not own the resident's lever. The player first sees the separate constraints: item availability, task compatibility, scheduled access, and participant consent.

### Route A: personal loan

The player asks to borrow the resident's lever until the panel check. The resident may accept a specific supported due condition, ask for return before a later task, propose an outright trade, or refuse. If the barter owner can link return to a task event, the due condition can reference that event. If not, the player selects an available time or keeps the offer unaccepted. A loan does not transfer permanent ownership; it follows the exact contract and inventory semantics.

After acceptance, the settlement owner revalidates item custody and any required capacity. If the resident has since moved the lever to another task, no transfer occurs. The player can renegotiate or stop. If the lever moves successfully, the task owner later decides whether the panel work can begin. The barter owner does not schedule the repair, and task completion does not automatically return the lever unless the accepted terms bind the two through a supported event.

### Route B: stores release

The player asks whether the shared handle can be reassigned from the other job. The stores owner or major authority decides whether release is permitted. A maintenance volunteer can explain which job currently uses it, but cannot release stock. The player may request a new window, ask the resident to wait, find a different tool through the catalog, or abandon this route. If release is denied, the UI names the authority and reason that its owner actually returns; it does not depict the minor group as betraying the player.

### Route C: trade for a different tool

The resident may want an available item, a clearly defined task, or nothing in return. The player can offer a compatible tool, a verified item, or an owner-backed task. Any counteroffer becomes a new version. The resident can refuse a proposed price or task without losing the right to keep their lever. The barter system is not a marketplace that computes universal value; both participants decide whether the terms work.

### Route D: supported substitute

A repair group may propose a different tool if current task or catalog data supports it. The player can inspect the option, confirm availability, ask the resident if they still want to loan the original, or use the substitute. The resident can withdraw their offer once a substitute solves the need. The story shows that a conversation led to a better path, not that the resident's item was secretly taken.

### Route E: wait for access

The major water authority may provide a later work window. The player can accept the schedule, ask the resident to keep the offer open if supported, negotiate a new expiry, or decide the goal no longer matters. A schedule change does not reserve the item. If the offer expires before the window, the player must create a new one with current facts.

### Conflict event and response

The authority advances the maintenance window because of a system event. The player may send a private update to the resident, post a general notice, or discuss the new time face-to-face. The communication route informs participants; it does not alter the water schedule. The resident may confirm the loan still works, ask to change its due condition, or withdraw. Any revision requires renewed agreement. The player may alternatively choose an authorized common handle and close the peer offer.

### Ending families

- A personal loan enables a task and returns through a verified due event.
- An accepted direct trade transfers a lever and tool atomically.
- A shared handle is reassigned by its real owner and the personal offer is declined.
- A compatible substitute removes the need for a personal item.
- The maintenance window moves and all terms are renegotiated.
- A stock request is denied and the player waits or changes the task.
- The resident refuses and the player leaves their property untouched.
- The panel task remains open because no safe item or time is available.

Each ending follows the player's specific actions: check compatibility, request authority, negotiate, accept a counteroffer, wait, or close. The final state keeps the item custodian, water schedule, communication record, task status, and resident choice distinct.

## 467. Value and leverage — scarcity changes options but not consent

When one item appears to be the only path to an important task, the player may feel pressure to accept a bad deal. The narrative should acknowledge scarcity without converting it into forced exchange, faction coercion, or an automatic moral score. This section adds branch depth around asymmetric need, perceived value, and negotiated alternatives.

### Unequal need is not an error

The player may desperately need a tool while the resident values it as a keepsake, work necessity, or backup. The game need not balance these interests through a numerical price. The resident can refuse, request more, offer a loan, suggest a different task, or ask the player to wait. The player may accept the cost, negotiate, seek another owner, revise the task, or stop.

If a formal market value exists, show it only as the market's reference and do not imply it decides personal barter. A resident may value an old wrench because it fits their current bench. That preference needs no global utility score. Dialogue can communicate why they will not trade without forcing the player into a persuasion mini-game.

### Pressure routes the player may choose

**Urgent but respectful:** the player explains the immediate task and asks whether a short loan is possible. The resident can agree, decline, or set a different condition.

**Offer more than requested:** the player adds a second item or task. The resident can accept, counter, or decline the excess. The game must not assume more value equals consent.

**Appeal to a major authority:** the player asks whether the item is required for shared operations. The authority can confirm a policy, deny the request, or say it has no claim over personal property. A policy decision can constrain only what it owns.

**Ask a supporting group:** a crew may identify a substitute, reduce the need, or report that the item is essential. It cannot compel the resident to lend it.

**Use leverage:** if the game permits a formal assignment or confiscation under a specific approved policy, that lies outside this peer barter proposal and requires its own authority and plan. Do not disguise enforcement as an “offer.”

### Resident response routes

The resident may accept a loan because it returns before their work, trade for something they need more, request that a witness record the agreement, accept an alternate tool instead, state that no item offer is acceptable, or ask to revisit when pressure is lower. These routes give them agency independent of whether they seem friendly. The resident's no remains legible and is not a puzzle for the player to defeat.

### Player consequence without moral label

If the player accepts a high-cost exchange, inventory shows exactly what left and arrived. If they decline, the task may remain incomplete. If they ask an authority, the answer may be a policy boundary. If they wait, a source may become available later or the need may change. The narrative can recognize frustration, relief, or uncertainty from the actual parties, but should not issue global labels such as “exploitative,” “generous,” or “selfish” unless the characters say them as personal interpretation.

### Branch combination examples

The player asks for a loan, gets a refusal, checks for a compatible substitute, and completes the task with that substitute. Another player offers a direct trade, hears a counteroffer, and declines; no item moves. A third asks the major authority, receives a confirmed stock rule, then negotiates with a different resident who owns a spare. A fourth leaves the task unresolved to conserve a scarce ration. Each campaign differs because its actions and available sources differ.

### Fairness check

Ensure the resident can decline before and after the player previews a cost. Ensure repeated prompts are not used to wear them down. Ensure the offer does not convert shared property to personal stock. Ensure the player can walk away without automatic penalties. Ensure a scarcity warning describes current availability rather than invented exclusivity. Ensure any enforcement route is clearly separate from voluntary barter.

## 468. Cross-faction exchange sequence — support groups coordinate, major owners decide

A peer exchange may require information from more than one small group and still leave the actual choice with the participants. This sequence demonstrates a layered route without creating a council or collective ledger.

### The player asks the route group

The route group reports that a handoff point can be reached by a verified corridor at the current time. It does not promise the path will remain clear later. The player can meet inside the shelter, ask for a later check, or choose a different place that the location owner permits.

### The player asks the records desk

A records volunteer checks whether the tool is under an accepted loan or marked for a task. If records are missing, the volunteer says the custody is unknown. The player can ask the current holder, choose another item, or close the offer. A records check is not a legal ruling.

### The player asks the repair crew

A technician says that a similar tool cannot perform the planned task safely, or confirms that another catalog item works. This determines compatibility only if the task/recipe owner backs it. The player may use the compatible substitute or return to negotiations. The crew cannot supply stock unless its inventory owner authorizes release.

### The player asks a major authority

A major faction may confirm that shared reserve stock is reserved for an urgent repair. It may deny release, authorize a specific transfer, or say personal property is outside its reach. The player can accept the policy, seek an authorized appeal, wait, or negotiate another peer offer. A minor group may help explain the consequences but cannot overturn the major owner's decision.

### Participants choose terms

The buyer and holder then agree or refuse. They can transfer items, create a supported loan, tie a specific favor to a task event, propose a new amount, or end talks. Support groups do not vote on fairness. The transaction owner validates the accepted agreement; inventory applies property movement atomically.

### Branches and closures

The route group may be unavailable, the records may show an outstanding loan, the repair crew may find a substitute, the major authority may deny common stock, and the residents may still agree on a personal exchange. Conversely, every support faction may help and the holder can still refuse. A well-supported decision does not guarantee a favorable result.

### Story presentation

Show each source as a short optional step in an “Ask” menu or conversation, not an unavoidable sequence of errands. If the player already knows a fact, allow them to proceed without asking again. Summaries should separate “route group says the corridor was clear at dawn,” “records desk found no current loan,” and “major stores authority reserved its handle.” The player can compare those facts without a moralized faction alignment panel.

### Acceptance review

Verify that each group has a bounded service and no hidden authority. Check that the player can accept an offer without consulting every group, decline after consulting them all, and renegotiate after facts change. Test each route with one group unavailable. Confirm that the eventual transaction contains only terms accepted by participants and that no faction's report silently transfers a property item.

## 469. Candidate side-quest — The Borrowed Weight

A small hand scale is missing from the workbench. One resident says they lent it to a neighbor to measure a crate; another says the scale was left in shared stores. A storekeeper remembers receiving a similar scale but cannot identify its instance. The player can check current custody, ask both participants, compare a written agreement, offer a replacement, or decline to arbitrate. The story concerns practical property and memory without constructing a criminal-investigation system.

### Start with what the owners can prove

The player reviews any active peer offer, inventory custodian, task assignment, and accepted terms. If a particular instance ID exists, the owner may identify it. If inventory only tracks a stack of similar scales, the game must not imply that it can distinguish the exact instrument. The storekeeper can confirm current shared stock, but not how the scale got there. A resident can describe a handoff, but that remains their account unless a supported transaction record exists.

### Ask each party separately

The lender may say they expected a return after the crate was weighed, wanted the scale back by the next shift, or believed it was a gift. The borrower may agree, disagree, or remember a different condition. The player can ask what would resolve the situation, ask whether a witness saw the handoff, or stop asking. Neither resident has to present a full personal explanation to retain a say in the offer.

A ledger volunteer can compare an accepted agreement with the statements. If no accepted agreement exists, the volunteer says that the written note is incomplete. A witness can share a direct observation if permitted; they should not infer which identical scale was moved. The player sees confidence and limits in plain language, not a numerical guilt meter.

### Remedy route: return, replace, or close

If the scale is currently in one resident's custody and the original agreement supports return, the player can ask them to return it, propose a new due time, or accept a different arrangement. The resident may agree, counter, or decline. An actual return routes through inventory and then closes the barter obligation. If the item is in stores, a major stores authority decides whether it can be issued; the peer barter owner cannot take it. If the item cannot be located, the player may offer a replacement, negotiate compensation if supported, or leave the dispute open.

A replacement is a new offer, not a silent repair of the old history. It requires a currently held item and atomic settlement. If the residents accept a task instead of a replacement, the task must be defined and tracked by its owner. A friendly apology cannot create a transfer. If both parties agree to close without remedy, the dispute can end while the scale remains unlocated.

### Time branch

The scale may be found later during ordinary work. If inventory reports its current custodian, the player can reopen the conversation or let the owner close the record according to its rules. The discovery does not prove that either resident lied. A late return can be accepted, disputed, or declined. If no current source connects the discovered scale to the earlier agreement, the UI must not automatically attach it.

### Ending families

- The accepted loan is confirmed and the scale is returned.
- The borrower counters with a later due date and the lender accepts.
- Shared stores holds the scale; the major owner issues or denies its release.
- A replacement is offered and atomically transferred under new terms.
- A task is accepted as remedy and closes only from its real completion event.
- The residents agree to close without resolving custody.
- The scale remains unlocated and the dispute remains open.
- The player declines to arbitrate; participants retain their own current choices.

The story has no “honest” and “dishonest” endings. It branches on the available records, participant statements, player investigation, proposed remedy, and consent.

## 470. Micro-story atlas — barter hooks that begin from different needs

Peer exchanges should arise from varied situations. This atlas offers short content seeds that can be expanded into a full branch scene without adding another authority. Each hook uses current inventory, task, consent, and barter ownership.

### The dry pair

A resident has two pairs of dry gloves and offers one for help mending a sleeve. The player can transfer a verified item, accept a task, counter with a different repair, or decline. A care group can advise on private disclosure if the sleeve damage is personal, but cannot make the trade.

### The annotated wrench

A tool bears a mark showing it belongs to a particular work crew. The player can check whether the mark represents assignment or ownership, ask the crew, or find a substitute. A records volunteer can read a current tool tag if the inventory authority supports it. The player cannot barter an assigned tool merely because they found it on a table.

### The shared kettle

Two residents want to use one kettle at different times. They can negotiate a schedule rather than exchange property. The player can help agree to a time, ask a major owner about shared-use policy, or leave them to work it out. If the current barter system only represents property and favors, this remains a conversation and schedule path, not a barter transaction.

### The map copy

A route group offers a copy of a current map note in exchange for a verified task. The player can ask whether the map is public, request a narrower excerpt, offer a different task, or decline. The map authority determines whether its information is current; the barter owner records any supported obligation. The route group cannot sell unverified travel safety.

### The spare buckle

A caravan trader controls a formal stock of buckles; a resident owns one personal spare. The player can use the market route for the trader's inventory or a peer offer for the resident's item. A supporting repair group can confirm compatibility. The three sources remain separate: market stock, personal property, and technical advice.

### The returned book

A resident asks for a book back from a borrower, but both remember the agreed time differently. The player can review a supported loan record, ask for a new due condition, or decline to mediate. A records volunteer can witness a revised agreement if both consent. If no loan was recorded, the narrative avoids presenting the book as stolen.

### The watch stand

A resident offers to cover a short duty in exchange for use of a tool. If the duty roster supports swap and the barter owner supports a task obligation, the player can construct a clear contract. If those owners do not integrate, the player can negotiate the tool separately and make the duty change through its own route. Do not collapse a roster assignment into item barter.

### The weather tarp

A household asks to borrow a tarp before a storm. The player can check inventory, request a shared-stock issue, offer a personal trade, or wait for a permitted supply delivery. The player cannot promise delivery from a faction that has not confirmed stock. A failed offer leaves the shelter's current resources unchanged.

### The quiet corner

A resident offers to lend a piece of equipment if the player secures a quiet work interval. The barter term depends on a schedule owner, so both the offer and time must be confirmed. If the room is unavailable, the player can counter with another supported slot or close the offer. A private preference should not be posted publicly without consent.

### The missing washer

A tiny component disappears from a repair task. The player can ask stores, inspect a supported task inventory, negotiate for a personal spare, or switch to a compatible design if current task data permits. This hook reinforces the difference between missing, reserved, borrowed, and unavailable.

### Atlas authoring template

For each hook, name the initial need, property owner, task or schedule owner, exact player actions, counteroffer, refusal route, support faction service, major authority boundary, settlement result, and later callback. Require a no-deal ending and one uncertainty ending. A hook is not ready if it assumes an item appears, a resident accepts, or a faction acts without a confirmed route.

## 471. Negotiation styles — player identity comes from choices, not labels

The player can approach exchange as a clarifier, a planner, a reciprocal trader, a cautious holder, or a broker. The game should not name or lock the player into any of these. They are narrative lenses for testing whether different approaches produce useful routes.

### Clarifier

The player asks what is being offered, who owns it, and what each side expects. This can expose ambiguity before acceptance. It may make the conversation slower or more formal. The resident can respond with a clear term, ask the player to trust their word, or withdraw. Clarification does not imply distrust.

### Planner

The player checks timing, task eligibility, and item availability before talking terms. This avoids impossible commitments but can lead to a delay while one or more owners respond. The player may choose to proceed with known uncertainty, wait for full verification, or abandon the exchange. No path gets a universal bonus for planning.

### Reciprocal trader

The player offers something tangible in return and is willing to counter. The resident can accept, ask for less, ask for more, or reject the basis of trade. The player can revise terms or stop. The resident's counteroffer reflects their own need rather than an alignment judgment.

### Cautious holder

The player may refuse to part with scarce property, ask for a loan rather than a transfer, or wait until the item is no longer assigned. This protects current resources but can leave a task open. A resident can offer an alternate item or decline. Conservation is a viable management style, not evidence of selfishness.

### Broker

The player asks a support group to clarify compatibility, source, or agreement terms. This can reduce ambiguity but brings a third party into the exchange. The player can choose whether that extra audience is acceptable. The group does not set fairness or acceptance. It can decline its service and let the participants negotiate directly.

### Style changes by context

A player may carefully verify a tool loan, then make a direct exchange for gloves, then decide not to mediate a book dispute. Each decision is contextual. Save only the underlying accepted terms, fulfilled tasks, and supported relationship facts. Do not persist a player “bargaining archetype” that constrains future dialogue.

### Playtest observation

Give testers one scenario with an ambiguous loan, one with an immediate task, one with scarce stock, and one with a minor-faction service. Ask them to explain what moved, what remains due, and who can say yes. If they repeatedly confuse a counteroffer with acceptance or a market price with personal consent, revise the interaction and copy. The system succeeds when a player can choose differently while keeping the consequences legible.

## 472. Questline — The Long Handle and the Short Week

This candidate story follows an item exchange across several days of changing demand. A resident lends a long-handled tool for one task, then a second resident asks to use it for a different job before its agreed return. The player must respect the first agreement, ask for a revised term, find another permitted tool, or decline to intervene. The story tests time, multiple participants, and the limits of a promise without a global reputation counter.

### Day one: define a loan that can be honored

A resident offers the tool until a named maintenance task is complete. The player checks whether that task has a supported completion event and whether barter can store it as a due condition. If yes, both participants review the term and accept. If no, the player can choose a specific supported time, propose an outright trade, or leave the offer unaccepted. The tool's owner and receiving capacity are revalidated at settlement.

The task is scheduled but not yet complete. The player sees “loan active, return due after task completion.” The tool moves according to the inventory contract. A receipt shows that the loan is not a permanent transfer. A support ledger volunteer can witness the terms if both parties request it; they do not hold or control the item.

### Day two: a second request arrives

Another resident needs the tool for an immediate job. The player can tell them it is currently loaned, ask the first resident whether an earlier return works, ask the second resident to wait, check for a compatible substitute, or leave the second request open. The second resident does not inherit rights over the tool. The first resident can accept an early return, reject the change, or propose a different time.

If the player requests a return, the inventory owner confirms custody. The first resident may still need it; acceptance of an early return is a new term. If the second task can use a substitute, the maintenance group can advise and stores can verify availability. The player may provide that substitute from an authorized source or ask the second resident to choose another method.

### Day three: the first task is delayed

The task owner reports that the original maintenance work is interrupted due to a missing component. The due event has not occurred. The player can ask the first resident to extend the loan, return the item under an alternate condition, renegotiate the second request, or leave the task blocked. An extension creates a new accepted term if the owner supports versioning. The old condition remains visible in history.

A major faction may reserve shared substitute stock for a separate repair. The player can request an authorized release, accept denial, find a personal offer, or defer. The supporting group can describe compatibility but cannot bypass the reservation. The resident who lent the tool may withdraw their offer if the current contract allows and the item can be returned safely.

### Day four: multiple endings diverge

**Original due condition met:** the task completes, the loan becomes due, and the item returns through inventory. The second resident can then make a new offer; the first loan is closed.

**Extension accepted:** the first resident agrees to keep the loan open until a new supported condition. The second task uses a different tool or waits. The player sees two separate statuses.

**Early return accepted:** the first resident accepts a new return time. The item returns and may be offered to the second resident under a new agreement. No automatic second loan begins.

**Original lender refuses revision:** the first agreement remains unchanged. The player seeks another tool or tells the second resident none is available. The tool is not transferred twice.

**Offer withdrawn:** the lender requests return under the original or permitted recall terms. The player executes an owner-backed return or reports that the item is still in use. The task may pause; the game does not silently replace the tool.

**Task abandoned:** the task owner closes or cancels it; the loan due condition responds according to its contract. If the task event is the due trigger, cancellation handling must be explicit before the feature is implemented.

**No supported due event:** the conditional loan route is omitted. The player negotiates a time-based loan or makes no exchange.

### The second resident's perspective

The second resident may thank the player for checking, ask whether a substitute is actually available, say that the task can wait, or decide to do a supported smaller job. They should not be told that the first resident is selfish or that the player promised the item unless those facts are true. The player can explain the constraint without revealing private terms beyond what the lender allowed.

### Long-term callback

If the player returned the tool on time, the lender may accept a later offer more readily because the actual agreement closed successfully, but no numeric trust bonus is implied. If the player asked for an extension and it was accepted, future dialogue can refer to that arrangement specifically. If the second resident chose another tool, that resident can propose it again in a different task only if the current inventory and task data support it.

### Design proof

Test same-day acceptance, delayed task, changed due condition, early return, recall, participant departure, no substitute, and reload after every transition. Capture both the barter contract and inventory custodian state. Verify that the active loan is not duplicated, a returned item is not still marked as held, and no downstream goal interprets “task scheduled” as “task complete.”

## 473. Settlement variants — direct transfer, loan, task, and access service

Not all exchanges move an item. A peer bargain can include a direct transfer, a temporary loan, a task obligation, or a service such as access to a work interval if current owners represent those terms. The player-facing language should make these variants distinct.

### Direct transfer

The owner changes once, both parties' agreed items move atomically, and the transaction closes. The resident has no automatic claim to a later return. A later return would be a separate offer. The receipt says “transferred,” not “borrowed.”

### Loan

Ownership remains associated with the lender according to the property owner; the borrower receives permitted custody or use. The agreement states a supported return condition. The player can renegotiate or return the item. Overdue status is one fact, not automatic moral failure. The owner must define behavior if either participant leaves or the due event is canceled.

### Task obligation

A task is described precisely enough for an existing task producer to validate. The barter contract may wait for the event and close when it arrives. The barter owner does not copy task progress. If the work is interrupted, canceled, or completed by someone else, the agreement follows its stated rule; it is never inferred from a narrative line.

### Access or time service

A resident may offer a work window, room use, or shared access. The schedule/location owner decides whether the slot can be booked. If the barter system cannot represent access as a term, keep the exchange in the scheduling feature and do not call it a barter settlement. A task or item can still be negotiated separately.

### Information or method

A resident may share a method, instruction, or observation. This can be a conversation or a communication record. Unless a current owner tracks a licensed/transferable information asset, do not treat knowledge as inventory. A method can be shared without creating a property transfer, and privacy permission remains specific to its audience.

### Mixed terms

An offer may combine an item and a task only if atomic property movement and task-linking semantics are sound. If the item transfers immediately while work remains pending, the UI must show two results. If the item is conditional on work, the transaction owner must support conditional settlement and rollback. Otherwise, break it into two voluntary agreements or decline the bundle.

### Branch result copy

- “The lantern transferred. No return is expected.”
- “The tool is on loan until the pump test completes.”
- “The task was accepted; the item has not moved.”
- “The work slot was booked through the schedule owner. It is not a barter item.”
- “The resident shared the method privately. No inventory record changed.”
- “The bundle could not settle because the item moved. No task was started.”

These distinctions make the feature understandable to players and reduce narrative contradictions.

## 474. Acceptance and dispute scene ladder

A compact scene ladder helps writers show a negotiation's progression without adding a full arbitration system. The player can advance, revise, pause, or stop at each step.

### Step one: interest

A resident names a need or answers an offer. Interest is not acceptance. The player can ask a question, propose a term, or leave. The resident may show curiosity without promising anything.

### Step two: proposal

Both sides can see exact items, quantities, task, due condition, time, and audience. If a term is unknown, it remains pending. The player can revise the draft. A support group may supply information upon request.

### Step three: response

The resident accepts, declines, or counters. A counter creates a new version. The player reviews it before proceeding. If a proposal expires, it closes according to the current barter owner.

### Step four: final validation

At acceptance, the owner checks current custody, participants, capacity, due condition, and the relevant task/schedule contract. Any failure returns a reason and leaves all property unchanged. The player may renegotiate or close.

### Step five: settlement or obligation start

An atomic transfer produces a transaction result. A task obligation becomes active only after the task owner accepts it. A loan becomes active with its due condition. The UI tells the player which status applies.

### Step six: performance

A task event, return event, or participant action updates the relevant owner. The barter contract reads that result once and closes or remains open. Repeated event delivery does not duplicate fulfillment or trust changes.

### Step seven: dispute or closure

If a party reports a problem, the player can review terms, ask for a permitted fact, propose a remedy, or stop. The barter owner records dispute state. Property changes still require the inventory owner. A witness states only what they observed.

### Step eight: future relationship

The resident may make another offer, avoid a certain contract type, or decline new trade. Any future dialogue is based on specific recorded history and current preference. One dispute does not create a universal label.

### Ladder bypasses

A player can stop at any step. Closing the panel before acceptance does not settle. A resident can reject the first proposal. A no-deal result is complete. If the owner does not support a later stage, do not simulate it with narrative text. The ladder describes player-facing states; it does not authorize a new save ledger.

## 475. Relationship continuity around an exchange — memory belongs to the participants

A barter interaction can affect how participants approach the next negotiation, but it should not turn every exchange into a global reputation score. People may remember a specific accepted term, a returned item, a missed due time, a refused counteroffer, or the fact that the player stopped asking. The current barter and social owners determine which facts persist and who can see them.

### Distinguish outcome from interpretation

The transaction owner can establish that the item moved. A participant may interpret the deal as fair, useful, rushed, or awkward. If the current social owner tracks such an interpretation, the player may see a narrow relationship consequence. If it does not, the dialogue can express perspective without a persistent metric. The game should not convert every friendly line into a trust increase or every tense line into a penalty.

### Relationship states that can coexist

A resident may thank the player for a timely return while refusing to loan the same item again. They may keep a professional relationship after a dispute, accept another kind of trade, or avoid the player for a while if canon and current social data support it. They may regret a deal but still honor its terms. A single resolved dispute need not restore warmth; a successful transfer need not imply friendship.

### Branch examples

**On-time return:** the lender accepts another loan but asks for a shorter due time. The old agreement closed; the new offer has new terms.

**Late return with accepted extension:** the lender agrees to revise the due condition. A future offer may reference that extension if the owner persists it, but does not accuse the borrower of unreliability.

**Late return with no agreement:** the lender may decline another loan or request a witness. The player can accept the boundary, propose a different exchange, or stop.

**Declined offer:** a resident may be willing to trade another item later. The refusal was local to that offer, not a global rejection of the player.

**Successful task favor:** the resident may prefer task exchanges in future because the previous task completed, if that preference is explicitly stored. Otherwise they can simply say the help was useful in a conversation.

**No-deal after dispute:** the residents can choose not to settle. They may continue sharing the space while keeping the disagreement unresolved.

### Callback limits

Use one relevant remembered fact in a later scene. Do not list all prior deals. Avoid “you always pay back” or “you never give me a fair offer” unless an established relationship model supports a broad claim. Prefer concrete phrasing: “The last lantern came back before the shift ended.” It can be true without becoming an identity label.

### Information access

A participant's private offer history should not automatically be visible to unrelated residents or factions. A supporting ledger volunteer sees a record only if both parties agreed to their witness role. A major market owner does not receive peer barter history unless a current system explicitly authorizes it. A player-facing history panel shows only the scope allowed by existing ownership and privacy rules.

### Review procedure

Run the same new offer with histories of a clean return, accepted extension, unresolved dispute, declined offer, and no record. Verify that current item availability and consent are evaluated identically from current owners while conversation options can vary on supported history. Remove any line that presumes an outcome absent from the save. Confirm that no branch requires a global good/bad merchant reputation.

## 476. Favor verification — define performance before accepting the promise

A favor should be specific enough to recognize later. It should not become a vague debt that the game carries indefinitely. The player and resident need to agree on what action counts, which owner confirms it, and how interruption or substitution behaves.

### Concrete task favor

“Carry the crate to the marked shelf during tomorrow's shift” is concrete only if the task owner has that task, the location is valid, the time can be scheduled, and the resident is eligible. The player may accept, counter with another shift, ask someone else, or decline. If the task completes, the producer reports the actual contributor and result. The barter owner closes only the linked obligation.

### Completion event favor

“Return the tool after the inspection” can be represented if the inspection task has a stable completion event and the barter owner can use it as a due trigger. If inspection is canceled, the agreement needs a defined fallback: a time, a renegotiation, or a return request. If the current API has no cancellation behavior, the conditional favor should not ship.

### Advice or information favor

“Show me how you tune the radio” may be a conversation rather than a durable obligation. If a lesson system records a session, the barter record can reference its completion. If not, do not leave a permanent debt flag. The participant can invite the lesson later, decline, or share a method without asking for compensation.

### Substitute performance

If another participant does the task, the original beneficiary decides whether that satisfies the agreement only when terms allow a substitute. The system should not assume any equivalent work counts. A changed contributor may require a new accepted version. Task attribution remains accurate.

### Interruption and partial work

An interrupted task may not fulfill the favor. If the accepted terms define a partial stage, the task owner records it and the beneficiary can choose to accept it, request completion, or renegotiate. A line saying “I started” cannot close an agreement requiring completion. The player may offer a remedy, but a new task or property movement requires a new authorized command.

### Expiry and withdrawal

A favor may be due by date or event if supported. If no expiry is accepted, do not invent one. Either participant may withdraw before acceptance; active agreements follow their current contract. A withdrawal after acceptance may require negotiation or a dispute route. Keep the old version visible and make the revised terms explicit.

### Verification table for authors

For every favor, fill in: task or event ID; responsible owner; participant eligibility; due condition; allowed substitute; partial completion rule; cancellation rule; withdrawal rule; save reference; success line; failure line; and renegotiation option. If any field cannot be answered, keep the scene conversational or design a separate proposal. This prevents a charming promise from becoming an unsupported persistent obligation.

## 477. Branch templates for peer offers

Writers can create reusable branch structures while changing the actual human problem and local details. Templates should be prompts for design, not repeated text formulas.

### Ambiguous ownership

A resident finds an item in a shared space and asks to trade it. Player routes: inspect current inventory owner, ask the apparent holder, ask a supporting records group, propose a different item, or stop. Outcomes: verified personal property, shared stock, assignment to a task, ownership unknown. Only personal or released property can enter the peer offer.

### Changing need

A resident accepts an offer, then the underlying need changes before settlement. Player routes: confirm whether they still want the deal, revise terms, allow withdrawal, or proceed only if the original offer remains valid. Outcomes: new offer version, expired request, declined trade, or successful settlement on revalidated terms. The player must not assume that urgency preserves consent.

### Counteroffer with audience cost

A participant wants a witness or a public record. Player routes: agree to invite a named witness, choose a private agreement, ask for another permitted method, or leave. Outcomes depend on both participants' consent and the communication owner. A public record can create exposure; preview the audience.

### Useful substitute

A support group suggests another item. Player routes: check compatibility, verify availability, offer it, or decline. The original participant may accept, counter, or refuse. Outcomes include substitute settled, unsupported item rejected, source unavailable, or no deal. The group provides advice, not a free item.

### Post-settlement task failure

The trade completes, then an unrelated task fails. Player routes: leave the trade closed, propose a new remedy, ask whether a separate return agreement is wanted, or accept the resident's refusal. Outcomes never retroactively reverse atomic settlement. The narrative explains both states separately.

### Partial delivery

A route group cannot deliver the whole requested quantity, or inventory cannot accept the offered stack. Player routes: reduce quantity through a new offer, wait, find a second source, or stop. No partial settlement unless the transaction owner supports and both parties accept that smaller term. A failed second side must not leave the first side moved.

### Template acceptance

Each template includes a clear no-deal ending, an owner-backed success, an uncertainty result, and a specific later callback. Remove the template if it implies character traits from a single choice, requires a faction score, or cannot explain property custody after reload.

## 478. Quest seedbook — barter stories beyond a missing tool

Peer barter can carry a wider range of human situations than a simple item swap. The seeds below stay within offers, atomic settlement, and supported obligations. Each includes a no-deal route and avoids making every disagreement a faction conflict.

### Seed A: The Recipe Card

A resident offers a copied meal note in return for a small cooking task. The player can ask whether the card is personal or intended to be shared, offer a task, trade a permitted item, request the source be kept private, or decline. If the card is just dialogue or authored information, do not make it an inventory item. A communication or learning owner must support any persistent sharing event. The resident may share a method without accepting a task, or may prefer to keep it.

**Branches:** private exchange, generic shared note, task accepted and later completed, offer revised to an item, or no deal. A food or nutrition effect exists only if current systems consume that information.

### Seed B: The Bent Handle

A resident wants the player to repair a personal hand cart handle. The player can offer materials, an existing repair task, a loan of a replacement cart, or no help. The resident can accept a task but retain ownership, trade the cart, or withdraw. The task owner defines repair scope; inventory changes only on an accepted property transfer. A maintenance group may advise on safe repair, but cannot make the personal cart communal property.

**Branches:** repair completes; repair attempt is inconclusive; component is unavailable; player offers a temporary replacement; resident withdraws. The result should not make the resident owe an unrelated favor.

### Seed C: The Route Legend

A route group has a paper legend and a resident asks for one annotated copy. The player can ask whether current exploration data supports it, arrange a task in exchange, offer a permitted supply, or keep the information as a conversation. The route authority determines whether its marks are verified. The barter owner records a task or item agreement only if one is actually accepted. The legend cannot reveal unknown paths through barter.

**Branches:** verified legend shared; report marked uncertain; task exchange accepted; group declines to distribute; player stops. The route group is a supporting knowledge source, not a market for safety.

### Seed D: The Night Watch Thermos

A resident offers to lend a thermos for one shift while asking for a spare lid. The player can check personal versus shared custody, accept a time-bounded loan, offer a compatible lid, ask stores, or decline. A support care circle can suggest a private exchange because the request relates to rest, but does not speak for either participant.

**Branches:** item transfer settles; loan returns at a supported time; lid is not compatible; shared stores denies release; the resident withdraws. No health or morale bonus is granted unless another current owner confirms it.

### Seed E: The Folding Stool

A resident lends a stool in exchange for help moving a workbench. The player can accept a task route, negotiate a different time, check whether the room permits movement, or decline. A room owner controls access; the task owner controls work; barter records only the accepted property or favor terms. If bench movement is not a supported task, the exchange can remain a verbal agreement with no persistent debt.

**Branches:** task completes; room unavailable; alternate method; stool returned late; no agreement. A return condition must be explicit.

### Seed F: The Pencil Tin

Two residents share a pencil tin and disagree about whether one can be borrowed. The player can ask its current owner, check whether it is communal stock, propose a small replacement, or leave them to resolve it. The records volunteer can help label shared versus personal supplies only if inventory ownership supports that distinction. This is a small exchange with a low-cost no-deal ending; do not escalate it into a criminal case.

### Seed G: The Manual in Two Hands

One resident has a manual; another has practical knowledge but no copy. The player can arrange a supervised lesson, ask for a permitted copy, offer an item for a loan, or leave the request open. If learning and communication owners support a session, a task or message can represent it. The manual remains with its owner unless transferred. Knowledge sharing does not imply ownership transfer.

### Seed H: The Unused Strap

A resident says a strap is unused, but it may be assigned to a current repair. The player can ask stores or the task owner, negotiate with the personal holder, select a substitute, or stop. The player may find that “unused today” is not “surplus.” The major owner decides reassignment for shared assets; peer consent applies to personal property.

### Seed use and content scale

Each seed can be a short side interaction or expand into a two-day story if the player accepts a supported task. Avoid turning the entire seedbook into a fetch list. Surface one request when its actor, item, or task makes it relevant. Keep the exact feature promise visible: negotiate voluntary terms, settle property without partial transfer, and close a supported favor from real events.

## 479. Candidate side-quest — The Shared Tarp

A storm is forecast in current weather data. A resident has a personal tarp that fits one window; a major stores authority controls communal covers; a route group can report which outside passage was checked. The player may negotiate a personal loan, request a shared issue, propose a task, ask for a check, or decide no cover is available. The barter choice changes who may use a personal item; it does not set the weather, safety policy, or shelter allocation.

### Chapter one: verify the need and sources

The player can ask the resident why the tarp matters, inspect the window condition through its owner, check shared stores, or consult a maintenance crew about the fit. The resident can share a practical reason, keep it private, or withdraw the offer. The weather owner supplies the forecast. A route group's observation does not establish building protection. The player sees which facts are confirmed and which are reports.

### Chapter two: separate personal and shared property

A stores check may find a communal cover reserved for another location. The major authority can release it, deny the request, or offer a waiting time. A minor group can explain the reservation but cannot override it. The personal tarp remains the resident's choice. The player can ask for a temporary loan, offer a compatible item, offer a supported repair task, or leave it alone.

### Chapter three: negotiate exact use

The resident may loan the tarp until a supported event, transfer it permanently, accept help fitting it, or refuse. If temporary use is not represented by current inventory semantics, the offer must remain proposal-only until approved. The player can propose an accepted time, ask for a shorter period, or decline. The receiving location must be valid and accessible. A conversation alone does not reserve the tarp.

### Chapter four: conditions change

Weather may intensify, pass without effect, or shift to another location according to the weather owner. The player can ask whether the agreement still works, return the tarp early, request an extension, or use a shared issue. The resident can renegotiate or withdraw as permitted. A shift in forecast does not automatically change the barter contract. If the tarp is already transferred, a separate return agreement is needed.

### Chapter five: aftermath

A successful use does not prove the shelter is safe. The location/structure owner reports the actual result. The resident may ask for the tarp back, keep the trade closed, or offer another arrangement. If the cover fails, the task owner reports the failure and the player can seek a different permitted route. The item remains where its owner says it is.

### Endings

- Personal loan is accepted, used, and returned through a supported transaction.
- Permanent exchange settles atomically and the resident receives accepted terms.
- Major stores releases a shared cover; personal offer is declined.
- Stores refuses release; player negotiates, waits, or closes the request.
- Forecast changes; the parties revise or cancel their offer.
- The resident declines and the player leaves their property untouched.
- No cover is available, so the player sees an unresolved need rather than a fabricated solution.

### Faction roles

The major authority governs communal stock and any binding safety policy. The route group supplies a time-bound access observation. The maintenance crew advises on fit and installation. A records volunteer can witness terms if both parties want a record. The player decides whom to ask; the resident decides whether to loan or trade. Support is useful without becoming command.

## 480. Post-trade narrative — let the exchanged thing enter the world

An item transfer should produce a visible and meaningful consequence where current owners can show one. The transferred object can become available to a task, disappear from the seller's inventory, return as a loan, or be used by another participant. Avoid treating an item as an abstract reward detached from the people who exchanged it.

### The item in use

If the task consumes or equips the item, inventory/task owners report that effect. The recipient may refer to its use. If the item stays in storage, do not narrate that someone carried it. A candidate detail such as “the wrench now hangs beside the cabinet” requires a location or inventory owner that can establish placement.

### The seller's changed options

A resident who traded a tool may later need it. They can ask for a new offer, propose a return, borrow a compatible alternative, or decide not to revisit the deal. The system does not automatically reverse an accepted transfer. If they regret it, the player can negotiate a new term or decline.

### The buyer's changed options

The player may now start a task that was previously blocked, but only when task eligibility and item availability are still true. The item may have been consumed, moved, or reassigned. A transaction receipt is not an eternal guarantee that the item remains available.

### A favor changes another task

A completed task obligation can create a useful route for another current task. The task owner provides completion; the barter owner closes its link; the new task owner checks its own prerequisites. Do not make one completed favor satisfy multiple unrelated goals unless their owners share the same stable event intentionally.

### Dispute as a future scene

A post-trade dispute can arise if the item was not what the offer described, the loan returned late, or a promised task failed. The player reviews the accepted version and source owner. A character can be frustrated without the game declaring fraud. A new remedy is negotiated and settled separately.

### Quiet callback

Sometimes the best callback is only an object in use: a repaired hinge, a lamp at the packing table, or an empty hook where the borrowed tool was stored. It can add flavor if current state supports it. Do not force dialogue to praise the player after every exchange. Ordinary continuity makes the world feel richer than a repeated reward line.

## 481. Candidate side-quest — The Handcart's Three Stops

A handcart is used by three residents on different shifts. One has current custody, another has a scheduled task, and a third wants to move a crate. The player can facilitate an agreed handoff, propose a loan, find a second cart through an authorized source, reschedule the task, or refuse to coordinate. The story tests multi-party timing without assuming the barter system is a group scheduler.

### First stop: identify the current custodian

The player checks inventory and task assignments. If the cart is shared property, the stores or task owner controls release. If it is personal property, the current holder may offer it voluntarily. If custody is unknown, the player asks a source or leaves the request open. A handcart standing in a corridor does not identify its owner.

The current holder may say they need it for their own shift, can lend it after a task, or are willing to transfer it. The player can ask for a clear time, propose a task in return, request a witness, or stop. If the holder declines, the player cannot send a different resident to retrieve it.

### Second stop: two tasks overlap

The roster shows that the cart's current task ends near the time the second resident wants it. The player can ask the first resident whether an early release works, ask the second whether a later slot is acceptable, check for an authorized shared cart, or abandon the overlap route. A schedule owner confirms any timing change. A barter owner records only the accepted loan or favor. Both records may link to the same task event without copying schedule state.

### Third stop: the crate is heavier than expected

The task owner may report that the crate requires two eligible workers or a different cart. The player can ask a support crew for handling advice, select another worker, change the task scope, or stop. The current holder can still withdraw a pending offer before settlement. If a trade already transferred the cart, the player cannot revert it because the later task changed; a new return or exchange is needed.

### Branch: make a three-way plan

The player may propose that the first resident release the cart, the second resident complete a crate move, and a third resident return it to a shared place. This is valid only if the current barter and task owners support the participants and terms. Each person must accept the relevant obligation. If a three-party obligation is unsupported, decompose it into valid two-party agreements or keep the plan as conversation. Never present a group handshake that cannot be persisted.

### Branch: use a substitute source

The stores owner may release a communal cart; a repair group may identify a safe carry alternative; or a market owner may have a formal offer. The player can compare those routes. A support group does not create the item. A major authority may reserve the shared cart for priority work, and that reservation remains binding unless its owner changes it.

### Branch: no deal

The player can decide that the handoff is too complicated, tell the requester that no cart is available, choose another task, or wait for the schedule to clear. No resident owes labor for having considered an offer. The current cart stays with its actual owner.

### Ending matrix

- A personal loan is accepted and returned by the agreed time.
- A direct transfer settles and the cart's new owner decides its use.
- A shared cart is issued by the correct authority.
- The task is rescheduled and the original offer expires.
- A supported two-person task replaces the larger move.
- A three-party plan is declined or decomposed into valid agreements.
- A late return opens a dispute with facts linked to the accepted loan.
- No handcart is available and the player closes the request without property movement.

### Carry-forward

A later resident may refer to the cart's actual availability or the completed task. Do not let one successful handoff create a universal “trusted cart keeper” label. If another request arrives, recheck custody, eligibility, and consent. The cart is a shared object in the world, not a permanent plot token.

## 482. Multi-party offer review — consent and authority do not scale automatically

When more than two people participate, a bilateral trade can accidentally become a group obligation. The interface must identify each person's role and what they accepted. A witness, helper, recipient, and owner are different roles.

### Participant roles

**Offerer:** proposes terms and may withdraw before acceptance under current rules.

**Counterparty:** reviews the terms and can accept, reject, or counter.

**Asset custodian:** controls whether the property can be transferred; may be one of the participants or a separate major owner.

**Task contributor:** performs work under its task contract; joining the conversation is not consent to work.

**Witness:** sees or records terms only with the agreed audience and permissions; does not guarantee performance.

**Beneficiary:** receives a result, which does not necessarily make them a party to the transaction.

The UI should show each role, not a single group icon that implies collective acceptance. If a third party controls shared stock, their authorization does not mean the recipient accepted the exchange. If a witness agrees, that does not mean the task contributor accepted a shift.

### Acceptance rules

A two-party trade can settle only after both participants accept the same term version and all owners revalidate. A multi-party favor needs explicit acceptance from every person whose work or property is included. If one person counters, the terms version changes and prior acceptances may need to be renewed. If one person leaves before acceptance, the owner returns a valid stale/withdrawn state. No silent proxy acceptance is inferred from faction membership.

### Partial agreement

If two of three participants accept, the player may save or continue only if the owner supports a partial offer state. Otherwise, no transaction starts. The UI says which terms are pending and which parties have not answered. It does not show “mostly accepted.” The player can simplify to a two-party exchange, ask for a new group proposal, or stop.

### Withdrawal and rollback

Before settlement, a participant may withdraw where current policy allows; the offer is revalidated. After atomic settlement, an absent participant cannot unilaterally erase another party's property movement unless the accepted contract and owner provide such a remedy. A changed deal is a new agreement. If the current system lacks rollback, do not promise it in narrative.

### Adjudication limits

A major authority can approve access to communal stock or enforce policy. It cannot decide that all residents consented to a personal trade. A supporting group can witness terms but cannot authorize them. A player can stop the arrangement, but cannot sign for an absent participant without a supported authority. This prevents faction size or narrative urgency from silently replacing individual agreement.

### Testing the group shape

Test unanimous acceptance, one counteroffer, one refusal, one unavailable participant, one shared-stock denial, and one witness withdrawal. After each, verify that no property moved prematurely, no task began without assignment, and the accepted version is unambiguous. After reload, each participant's role and transaction state should remain correct.

## 483. Item story continuity — object detail follows actual custody

Physical objects can carry narrative texture, but the game must know who holds them before making them visible in a later scene. A lamp transferred in barter may appear on a resident's bench only if an inventory or location owner can support that placement. A loaned tool may be described as borrowed while its accepted agreement remains active. A shared stores item remains under its shared owner even if someone used it for a task.

When the object moves, the receipt and later dialogue should agree. If it is consumed by a task, show that result from the task/inventory owners. If it is returned, the custodian changes through the valid return route. If it is missing, do not place a duplicate on another shelf to preserve a story beat. A visual prop can suggest use but cannot contradict canonical inventory. If object instance identity is unavailable, write about the catalog item or quantity rather than “the scratched wrench with the blue mark.” This keeps small details rich and credible.

## 484. Non-item favors — trade a service only when it can be witnessed

A peer exchange may include a service, but the word “favor” can hide several different obligations. Before accepting, distinguish scheduled labor, advice, temporary access, a lesson, information sharing, or a promise to return an item. Each has a different owner and proof.

Scheduled labor requires an eligible task and a confirmed roster/time route. Advice may remain conversation and should not create a debt. Temporary access requires a location or schedule owner. A lesson needs a learning/task event if it is to close a persistent obligation. Information sharing needs a communication route and permission. A return promise needs a supported due condition and inventory movement.

The player can offer the service, counter with a simpler exchange, ask the resident to define what would count, or decline. The resident can reject a vague term. If the two parties cannot define a verifiable action, the conversation may still be warm, but the barter owner should not store an open-ended obligation. A later task can create a new offer when details are known. The system should not invent “debt” to make dialogue feel consequential.

### Example: help with a radio lesson

The player asks whether a one-hour lesson would be a fair exchange for a lamp. The resident can accept if the learning route supports it, ask for a shorter demonstration, offer a different task, or decline. The learner may stop partway through. If the accepted favor requires one session, the session owner closes it when the session occurs, even if mastery is not claimed. If the accepted favor requires competence, do not offer it unless a skill owner measures that result.

### Example: access to a workbench

A resident may offer a workbench slot. The player checks room scheduling and access. If the workbench is communal, the resident may not own the right to allocate it. A major authority or room owner decides availability. The players can exchange an item for another permitted time slot only if the schedule owner supports that agreement. Otherwise, the offer remains discussion and no trade is accepted.

### Example: information as a gift

A route worker may share a remembered observation. The player can ask whether it may be copied or passed to a group. If the observation is not in an authoritative route record, it remains a personal report. No item moves and no permanent favor is created unless both participants define a supported obligation. The player may thank the person without owing them a service.

### Review

For each favor, state the promise, consent, owner, due condition, success event, interruption event, cancellation behavior, and no-deal route. If the event cannot be proved, keep the obligation informal. This preserves the human difference between generosity, advice, access, and work.

## 485. Exchange after a change of plan — close the loop without erasing history

A barter agreement can become inconvenient after the participants learn something new. A task may be canceled, the recipient may leave, a part may arrive early, or the player may discover a compatible substitute. The system should give the participants a clear renegotiation path while preserving the original agreement.

### Task canceled before settlement

If the offer was pending and task cancellation removes a required term, revalidate before acceptance. The resident may create a new offer, keep the item, or leave. No transfer occurs on the stale version. The player can ask for an alternate task or decline.

### Task canceled after an immediate transfer

If the item already transferred and the task was separate, the trade remains complete. The player may propose returning the item, offering a substitute task, or keeping the original agreement. The resident can accept or refuse. A cancellation does not automatically reclaim property.

### Substitute arrives before acceptance

The player can tell the resident that the item is no longer needed, continue with the original offer, ask to revise, or close. The resident may still want the trade for another reason, but the player decides whether to proceed. The substitute's arrival does not invalidate consent by itself.

### Participant departs

A pending offer may become unavailable and must not transfer to a replacement resident. An active loan follows the existing owner contract for departure and due status. If the system has no safe handoff, show an unresolved state and request the appropriate major authority only where its remit permits. Do not fabricate a next of kin or custodian.

### New information changes value

The player learns that an item is more useful than expected or a market price changes. Existing personal terms remain accepted unless the agreement permits revision. The player can ask to renegotiate; the other participant may refuse. A new market price does not automatically alter a peer barter. The transaction owner stores the accepted version, not a live price estimate.

### Dispute after a changed plan

One participant may claim that the other should still perform a task after cancellation. The player can review the accepted terms, task state, and cancellation rule. The barter owner can open a dispute; the task owner confirms what happened. The player may propose a new remedy or let the dispute remain open. Dialogue does not retroactively rewrite the contract.

### Closeout language

- “The offer was withdrawn before settlement. Neither item moved.”
- “The trade completed before the task was canceled. The item remains with you.”
- “The work did not occur. Your accepted return condition is still pending.”
- “A new offer is required because the original item is no longer available.”
- “The participant has departed. Current custody is not confirmed.”

Use each line only if the relevant owners can prove it. If status is unknown, say what is unknown and do not imply fault. The player can choose a remedy, stop negotiating, or continue ordinary play. The exchange remains human because changed plans have consequences, not because the game reverses accepted facts at will.

### Review trace

Create saves before offer, after acceptance, after immediate transfer, after task start, and after cancellation. Exercise participant departure, source substitution, changed price, and task interruption. Verify that the accepted terms remain stable, all item movement is atomic, and repeated save restore creates neither a duplicate return nor a duplicate obligation. The narrative should provide a practical next step at each state.

## 486. Negotiation accessibility and pressure controls

Barter scenes should support players who want to read terms carefully, compare a counteroffer, or leave without rushing. Present the complete item list, quantity, custody change, favor, due condition, audience, and expiry in a reviewable summary. The player can reopen details before acceptance. Time pressure should come from an actual offer expiry or campaign event, not a countdown animation designed to force a quick choice.

The player can pause, close the panel, ask a question, or decline. If the offer remains valid, it can be revisited with current facts; if it expires, the owner reports that state. Do not settle when the player navigates away or presses a default key. Keyboard and controller focus should not jump directly to acceptance when the offer opens. A clear confirmation repeats who gives what and when.

Use readable language for loan, transfer, and favor. Avoid relying on color to distinguish property leaving or entering. Show the actual quantity and recipient. Where a player asks for more time, explain whether the offer can remain pending. Supporting factions can help clarify a term, but cannot decide on the player's behalf. These choices make negotiation deliberate and fair while preserving the resident's right to counter or refuse.

## 487. Regret, satisfaction, and changed preference after agreement

A participant may feel differently after a deal is accepted. The narrative can acknowledge that response while preserving the transaction facts. A resident may be satisfied with the trade but regret losing an item; they may accept the favor but dislike the timing; they may decide not to make similar offers again. The player can ask whether they want to renegotiate, propose a return, or accept the current outcome. The resident can decline further discussion.

If property already moved atomically, regret does not reverse custody. A new return or exchange requires both parties and the inventory owner. If work remains due, the task obligation stays open according to its terms. If the deal included a supported cancellation condition, use that contract. Do not add a post-hoc cancellation just to make the scene kinder.

A satisfied participant may still keep boundaries: “The bolts fit, but I don't have another pair.” A disappointed participant may say, “I thought you meant tomorrow, not after the inspection.” The player can review the accepted version and propose a clarification. If the written terms were clear, the other person can still dislike them; the system need not label either party dishonest. If terms were ambiguous, a dispute may remain open until a new agreement is reached.

Future offers can vary from this specific history. The resident may request written terms, prefer a direct transfer over a loan, decline a witness, or stop trading for a while. Persist only those preferences if the current social/barter owner supports them. One emotional reaction must not become a broad reputation tier. This preserves the distinction between an enforceable transaction and a person's evolving feelings about it.

## 488. Historical offer recovery

An older save may contain an offer with missing party, item, due-condition, or settlement references. The player should see only what can be recovered from current owners. Mark incomplete history as unresolved, let participants create a new offer from present facts, and avoid assigning a lost object to any person by guess. A missing witness or stale market value cannot be backfilled as a contract term.

The current transaction must remain unaffected by malformed history. No property moves until both parties and the asset owner validate a new accepted agreement. If the old offer was already settled, use the existing transaction result if available; otherwise present a neutral historical record and stop short of claiming success or failure. This gives old saves a safe narrative fallback without inventing migration state.

## 489. Transaction continuity after save and load

Persist the accepted terms through the existing barter save owner and preserve actual property through the inventory owner. On restore, rebuild the offer view from stable references; do not serialize a second copy of each item or task. If settlement occurred, reloading must not move the goods again. If the offer was still pending, revalidate it against current custody and expiry before showing acceptance.

A favor remains open only while its due condition and participant identities are valid. If the task completed during another route, consume its event once. If an event reference is missing, show an unresolved state and offer a new negotiation rather than crediting completion. A returned item updates custody once. A historical dispute does not create a new transfer.

Save after draft, counteroffer, acceptance, settlement, partial task progress where supported, return, cancellation, and dispute. Restore each state and compare transaction status with inventory, task, and roster owners. Deterministic replay must produce the same state from the same campaign input. This is the evidence needed before the narrative claims that an exchange can safely span multiple days.

## 490. Final barter consistency audit

Review every branch for who owns each item, what the participants accepted, when settlement occurred, and which task or return remains due. A counteroffer must show its new terms; a refusal must leave property unchanged; a partial settlement must be impossible unless explicitly supported. Check that a major faction's stock rule cannot be bypassed by a peer offer and that a supporting group cannot decide consent. Then trace one successful deal, one stale offer, one interrupted favor, and one unresolved dispute across save and reload. Narrative callbacks must describe those exact results, not a moral label.

## 491. Counteroffer readability pass

A counteroffer changes the agreement, so the player must be able to compare it with the prior version. Highlight the changed term in text: quantity, item, due condition, task, participant, time, or audience. Keep unchanged terms visible as well, especially when the counter modifies one part of a bundle. The resident can ask a clarifying question before accepting. The player may accept, make another counteroffer, or stop.

Do not rely on color, animation, or memory of the previous panel. A screen reader should announce the new and old values in a predictable order. On controller, focus should land on the summary and then the available responses. The confirmation screen repeats the complete accepted version so that neither side's contribution disappears in a compact notification.

If a term cannot be represented by the barter owner, explain that the player must remove it or choose a different arrangement. Do not quietly omit a due condition or task from the settled deal. After settlement, the receipt uses the same terms the player reviewed. This preserves informed consent and makes negotiation accessible without adding a second contract system.

## 492. Questline — The Drill with Three Owners

A carpenter offers a hand drill in exchange for a tin of screws and a promise to return the drill by the end of the week. A second resident says the drill belongs to the shared repair shelf, while the carpenter insists it was a gift from a relative. A shelf label lists the tool but has no owner name. The player can ask the carpenter where it came from, inspect the shelf record, ask the storekeeper to check any sign-out, or decline to negotiate until the ownership question is clearer. The tool does not move during these conversations.

A physical inspection finds a repaired handle marked with the carpenter's initials. That is evidence of maintenance, not conclusive proof of ownership. The shelf record shows the drill was deposited after a roof repair, but not whether it was donated or lent. The player can ask the previous repair lead, who remembers that the carpenter said “keep it with the tools,” but cannot recall whether that meant a permanent donation. The ambiguity remains plausible. The player may help locate another drill, propose borrowing the shared one through the normal sign-out process, or ask the carpenter whether they want to withdraw the offer until the record is clarified.

If the carpenter confirms the drill was a loan, the player can help return it to its owner or ask permission to use it for a short job. If the storekeeper finds a clear donation record, the drill belongs to the shared shelf and should not be traded as private property. If no record resolves the history, both residents can agree to leave it in place while the carpenter offers a different tool. One ending has the carpenter trade a personal wrench for the screws. Another uses a common tool under a logged loan and settles only the screws. A third ends with no trade, but the residents help repair the shelf so future tools are easier to identify.

The screws are checked separately. A resident may own the tin, or it may be stores stock. The carpenter may own the drill but not have authority to promise a return date if they will be away. Each item and term is validated at acceptance and again at settlement. If either item is no longer available, the system must not transfer one side and leave the other side with nothing. The player can amend the offer, arrange a new time or close it unaccepted.

A supporting tool circle can remember who repaired the handle; a storekeeper can verify shared inventory; the previous repair lead can offer testimony. None alone decides a contested claim. The player can choose a patient documentation style, pursue an immediate substitute, or stop the exchange and keep the work moving another way. The dispute grows from contradictory evidence and concrete action, not a good/evil or faction-loyalty check.

If the drill remains contested, the ending need not be a courtroom. The residents can leave a note for the next meeting, choose not to use the tool meanwhile, or identify a replacement. The unresolved matter is bounded to this item. A later record can reopen it when new evidence appears. No relationship penalty is added merely because the player refused to move contested property.

## 493. Exchange arc — A coat offered before the cold front

Toma offers a patched coat to Ren in exchange for three clean bandages and a morning shift carrying crates. Ren accepts the coat but says they cannot promise the shift until their knee is checked. The player can help Toma and Ren revise the offer, ask the clinic when Ren can safely lift, or decline to record a deal that treats an uncertain task as guaranteed. Toma is willing to hold the coat until morning, but the offer expires before the cold front moves through. The pressure is real; it does not authorize the player to decide for either resident.

The item terms need examination. The coat belongs to Toma, but the bandages are from a clinic cabinet and require clinic approval before transfer. Ren can offer a different item they own, volunteer a shorter seated task, or agree to revisit after the knee check. The player can ask whether Toma values immediate supplies or the promised labor more; this conversation changes the exchange terms only if Toma chooses to propose them. Ren may ask for the coat as a temporary loan. The player can record an explicit return condition if the owner supports such loans, or explain that the current barter owner handles transfer and cannot promise loan tracking.

If the clinic approves one bandage for personal use, the trade can settle that item and replace the remaining terms with a clearly described chore after the assessment. If the clinic declines, the player can offer a sewing kit, a food ration or no replacement. The player should not draw from shared stock because a deal was already discussed. If Ren's knee check clears a short crate shift, both parties can accept the revised deal. If lifting is unsafe, Ren can refuse the shift and Toma can choose among an alternative, a loan or cancellation.

The cold front changes urgency but not ownership. A neighborhood sewing group can reinforce the coat's torn lining before delivery; it does not own the coat or decide who receives it. A clinic aide can give a safe activity recommendation within their role; they do not set barter value. The player may prioritize warmth, fairness of terms, medical caution or avoiding delay. Those preferences show up as concrete alternatives. None requires a reputation alignment.

Ending A settles the revised deal after the knee assessment and transfers only approved items. Ending B gives Ren the coat as a temporary loan with the owner's explicit conditions, then closes the trade offer. Ending C has Toma keep the coat and give Ren a smaller patching job instead. Ending D cancels the exchange; the player asks the sewing group whether a spare coat exists through its regular inventory route. If no spare exists, Ren receives an honest shortage update rather than a promise. Later dialogue can reflect how the parties handled the change without assigning them a permanent score.

## 494. Favor branch — the lift that was returned differently

A resident asks Oren to help carry a water drum upstairs and says, “I owe you one.” Oren accepts the lift but does not name a return favor. The player can let the phrase remain a social expression, ask whether both residents want a recorded favor, or clarify that no future task is guaranteed. If the current favor owner records obligations, the player must show its supported terms and expiration. If it only records a simple open favor, the interface must not suggest that Oren can demand any service he chooses.

The next day Oren asks the first resident to sit with his brother during a repair appointment. The original helper is already scheduled at the kitchen. They can accept, counter with a shorter visit, ask someone else, or decline. Oren may be disappointed, but his request is not automatically a valid claim against their time. The player can help find another volunteer or explain that the earlier lift was not a transferable debt. The original resident can also remind Oren that they intended the phrase as thanks, not a promise. The scene resolves through clarification and choice.

One branch has both residents agree to a specific half-hour visit and settle the favor when it happens. Another has the helper offer a different task, such as bringing a blanket, which Oren accepts. A third closes the favor with no reciprocal act because both agree that the lift itself was complete. A fourth leaves the request open while Oren asks his brother what kind of company he wants. These branches matter because the player supported a specific conversation and task, not because they selected a “generous” response.

If a favor is recorded, its terms name the requesting party, the accepting party, the bounded request and when it expires. It does not reserve inventory or consume a work slot until accepted through the relevant owner. If a resident becomes ill before completing the favor, the parties can pause or cancel it. If the requester leaves, the favor does not become an enforceable burden on another survivor. The player can close it, request an extension or leave a truthful open state.

A meal circle might arrange a shared sitting hour, but it is not a debt collector. A work steward can explain task availability, but cannot force the helper to accept. The player can mediate only with both participants' permission. One playstyle is to make every obligation explicit; another trusts informal exchange until a misunderstanding arises; another prefers to close favors quickly so no one feels indebted. Each changes the conversation and the kind of future request available, without creating a hidden social score.

## 495. Settlement branch — both hands let go at once

Two residents accept a trade of a filter cartridge for a repair kit. The player sees each item in the summary and presses confirm. Before settlement, the filter's owner uses it to replace a failed unit elsewhere. A naive interface might transfer the repair kit anyway and leave the recipient without the promised cartridge. The barter owner must revalidate both assets immediately before commit and either transfer both sides atomically or transfer neither. If current APIs cannot guarantee this, the plan treats settlement as blocked and proposes a source audit instead of simulating success in the panel.

The player then chooses how to respond. They can explain the changed availability and ask whether the parties want a revised exchange, wait for another cartridge, substitute an approved item, or cancel. The repair kit remains with its owner until a new accepted deal settles. One resident may propose that the original recipient keep the kit as a temporary loan, but that is only supported if the current transaction owner represents loans truthfully. Otherwise, the player can arrange an ordinary return through existing inventory ownership or decline the unsupported term.

A retry branch shows why idempotency matters. The player confirms once; the screen pauses; they press again. Only one transaction may commit. A stable transaction key or equivalent owner contract should make repeat submission return the first result rather than create a second transfer. If the API does not provide that guarantee, keep confirmation disabled while pending and recover from the owner's recorded result after reload. Never infer success because the animation played.

The failure view names which side changed without exposing unrelated inventory. If the cartridge is gone, the player sees “filter cartridge no longer available,” not a fabricated receipt. If the repair kit is no longer available, the same rule applies. If the owner reports an unknown result after a crash, the interface offers a status check and blocks a second settlement until the owner resolves it. The parties may remain in conversation while the system checks; no item moves in the UI preview.

A fair ending can be “no exchange today.” One party may find another cartridge later, both may revise quantities, or they may withdraw. The player can ask the settlement clerk to review the failed transaction, but that role reports the owner state rather than independently moving goods. The quest's dramatic moment is a brief pause before confirmation and a shared decision to wait. It demonstrates reliability through what the player refuses to pretend.

## 496. Dispute arc — the pan returned with a crack

Lea lends a cooking pan to Sadi for a communal meal. When it comes back, Lea finds a crack near the handle. Sadi says it was there before; a third resident remembers seeing a dent but not a crack. The player can inspect the pan, ask both people what they recall, check an earlier condition note if one exists, or help them agree on a next action without determining blame. The pan cannot be repaired through barter until its owner approves the work and any proposed material is available.

If there is a dated condition record showing the crack, the player can share that narrow fact and ask whether the two residents want to close the dispute. If the record shows only a dent, it cannot answer when the crack formed. If no record exists, the game leaves the cause unresolved. Lea may request repair, replacement or return without compensation. Sadi may offer to mend the handle, contribute a patching plate or decline responsibility while still helping find a usable pan. Each proposal can be accepted, countered or rejected.

A cookware group can demonstrate a repair. It does not decide who pays. A stores worker can confirm whether there is a spare pan. They do not adjudicate personal blame. The player may look for a practical agreement, preserve the relationship by pausing the conversation, or allow the residents to disagree and use a different pan. If they both consent, a neutral witness can observe the handoff and record only the terms. Nobody is compelled to accept mediation.

The exchange record distinguishes a favor, a repair contribution and an item transfer. A promise to bring a patch is not the same as the patch being delivered. A repaired handle is not proof that the pan is safe for all uses. If the residents agree to a repair, the owner of the cookware confirms the resulting condition. If the pan remains unsafe, it stays out of active use and the barter screen does not offer it as a tradable item.

Ending A closes with a repair both residents accept. Ending B finds a spare pan and returns the cracked one to its owner. Ending C has Lea keep the pan for non-cooking use and Sadi offer a modest contribution. Ending D leaves the cause unresolved and the relationship strained, but the parties agree not to trade further until they choose to reopen the matter. A later cooperative meal can happen without pretending the dispute never occurred.

## 497. Playstyle map — negotiation methods from action

The player can develop a recognizable approach to exchange through repeated, observable behaviors. They may inspect ownership before discussing price, ask both parties to define each term, seek a quick substitute when a deal is blocked, favor verbal agreements until someone wants a record, or routinely pause when one party is uncertain. These are not selectable classes or hidden affinity values. They are patterns the dialogue can remember when the same resident invites the player to help with a later negotiation, provided the current narrative state already tracks relevant facts.

A careful negotiator asks: who owns each item, what exactly moves, when, and what happens if it is unavailable? This can prevent misunderstandings, but some residents may find the extra questions tiring. An expedient negotiator moves toward a workable substitute quickly, but risks overlooking a term that matters to one party. A community-minded negotiator brings in a witness or support group, but must ask whether the participants consent to that audience. A minimalist negotiator steps back and lets the residents talk directly, which can be respectful or simply leave someone unsupported. The story responds to the actual results: a clarified term, a delayed offer, a private conversation or an unresolved need.

There is no all-purpose best style. When property is contested, verification matters. When the owners already know each other and the items are simple, a short exchange may be enough. When a favor could create pressure, explicit boundaries help. When both parties decline outside assistance, stepping away can be the right action. The player learns these distinctions through outcomes and dialogue rather than tutorial labels.

A future implementation should not store a personality category for the player merely to trigger alternate lines. If a later response depends on a prior action, use an existing event or narrative fact with a clear meaning, such as “player helped revise this agreement” or “deal ended without transfer,” where a current owner supports it. If that event does not exist, let the resident's current line respond to the current conversation and avoid inventing campaign memory. No new reputation ledger is proposed.

## 498. Questline — The kettle agreed to stay warm

An older resident offers to boil water in their personal kettle during a power window if another resident brings dry fuel. The player must discover whether the kettle is a private item, whether the fuel belongs to stores, and whether either person expects a favor in return. The offer is not an authorization to draw from common fuel. The player can ask the pair to meet at the store counter, find a fuel source they own, agree that the kettle owner will use their own stock, or leave the exchange pending until the power schedule is confirmed.

When the power window shifts, one resident has already gathered the fuel. They may keep it, return it through the owner, or offer a different exchange. The kettle owner can continue with a small pot over the stove, but only if the cook station owner confirms that the burner is available. The player can split the water task between the two residents or cancel the barter and ask the shelter steward to use its existing hot-water service. Each branch tracks what was actually provided and by whom.

A fuel circle can tell the player when the shared stock is low, but cannot authorize a withdrawal. A kitchen steward can reserve a burner, but does not own the private kettle. The two residents decide whether the exchange terms are acceptable. The player can be resource-conscious, prioritize speed or protect the household's privacy by handling the conversation away from the counter. These actions affect whether the pair share the workload, trade a smaller amount or decide the arrangement is too complicated.

A successful branch has the kettle owner provide two pots, the other resident bring a personal bundle of dry fuel, and both agree that the exchange ends there. Another branch changes the offer to a favor: the second resident tends the fire while the first gathers water. A third ends without a deal because the kettle owner cannot safely leave their room. They can still ask the player to help locate an existing hot-water route. A fourth branch reveals that no dry fuel is available, so the pair reschedule rather than using shared stock.

The story closes with someone carrying a cup to a neighbor who missed the power window. That act is not automatically recorded as repayment. If the neighbor offers thanks, the kettle owner can accept it without a new obligation. The scene's warmth comes from a material exchange with clear limits and a small human outcome, not from a magical increase in community spirit.

## 499. Counteroffer library — six terms that can change

When an offer changes, the game highlights the exact terms: who receives the item, what quantity moves, whether an item is condition-limited, whether a favor is requested, when settlement occurs and what happens if a participant is unavailable. A counteroffer can alter one term or several. The participant who receives it can ask a question, accept, counter again or stop. The final confirmation repeats the whole offer, so a changed due time cannot be mistaken for the old one.

A **quantity counter** reduces three bandages to one. The player rechecks that both quantities are available and approved. A **condition counter** offers a repaired lantern rather than a working one, with its known limitation stated. A **timing counter** moves handoff from tonight to the next morning, which can make the goods unavailable or the need irrelevant. A **location counter** changes a handoff from the public room to a private doorway; the player checks access and consent. A **favor counter** replaces a spare item with one bounded task. A **withdrawal** cancels without blame and returns everyone to their prior ownership state.

The player can explain why a counter is needed, ask the proposing resident to make the change themselves, or leave the negotiation to the participants. If the player edits both sides without telling them, the feature loses consent. A resident's silence is not approval. If one participant is absent, the offer can remain pending only until its stated expiry. The player cannot accept for them unless an existing owner-backed delegation permits that precise action.

If an offer contains a term the transaction owner cannot represent, the UI identifies it before acceptance. “Return if still usable” may be unsupported even when a fixed item transfer is supported. The player can remove the term, switch to a documented loan authority if one exists, or stop. The interface must not drop a condition silently. A narrative line can still say that the residents intend to return the item, but the transaction summary should not claim the system enforces that intention.

## 500. Questline — The seed tin and the allotment line

A gardener offers a tin of saved seeds to a cook in exchange for a week of meal portions. The seed tin is private, but the meal portion request touches shared food. The player can ask the cook whether the offer is within the kitchen's allocation, ask the gardener how many seeds are needed for the next planting, propose a smaller exchange, or decline to broker a deal that would bypass ration authority. The seeds may be valuable later; the meal portions are needed now. That tension comes from timing and ownership, not a price meter.

The gardener says the seeds can spare a second planting, but a field steward reports that the plot will not be ready for another week. The player may verify the schedule, negotiate fewer meal portions, offer labor instead, or keep the seeds until planting is possible. The cook may not have authority to promise meals for future days. A current meal can be provided through normal distribution, while a future meal promise requires an owner that supports that reservation. If it does not, the player cannot sign it on behalf of the kitchen.

A seed-keeping circle can test germination with the gardener's permission. It can report the number that sprout, but cannot guarantee the crop. A meal coordinator can state today's available portions, but does not decide the seed tin's ownership. The field steward can confirm plot readiness. Each supporting group answers a limited question. The player may prioritize immediate food, preserve the seed stock, or seek a non-food alternative such as repairing the watering frame.

If the tin's ownership is clear and the meal authority approves a same-day exchange, the players can settle a small number of seeds for an available meal. If future meal terms are unsupported, they can record a one-time exchange or stop. If germination is poor, the gardener can withdraw the remaining seeds. If the cook's portions are reassigned before settlement, the exchange revalidates and either both sides move or neither does. An accepted conversation never consumes the seeds by itself.

The possible endings include a modest immediate deal; a revised trade for frame repair; a shared demonstration in which the seeds remain with the gardener; or no trade, followed by a normal meal distribution and a later planting check. The gardener may tell the player that preserving seed diversity matters more than the portions. The cook may say the kitchen needed one less promise to track. Both can be true, even when their priorities differ.

## 501. Dispute follow-up — who paid for the patch

After a jacket is mended, the thread owner shows a receipt for the patch but says the cloth belonged to another resident. The player can inspect the patch supply entry, ask the seamstress what she received, or invite the owner and resident to agree on how the contribution should be recorded. The mending can be complete while the material contribution remains unclear. The barter record should not claim that the jacket owner paid for every item if the source is unknown.

If the patch was donated by a sewing group, the group can confirm that no repayment was expected. If a resident supplied it privately, they may ask to be reimbursed, request a returned scrap, or say the jacket was important enough to help without repayment. If the seamstress used shared stores, only the inventory owner can confirm that withdrawal. The player cannot settle a disputed cost by transferring another item from someone else.

A practical agreement might be that the jacket owner returns an unused scrap, the resident receives a replacement from stores after approval, or no transfer occurs and both accept the material as a gift. The parties can also leave the question open. If the group needs the patched jacket for work, the owner may loan it for one shift with explicit consent, independent of who supplied the patch. The repair condition, ownership, use and reimbursement remain separate facts.

The seamstress can be a supporting witness to work performed, not a judge of value. A storekeeper can verify stock, not personal intent. The player may help draft a neutral receipt listing the confirmed contributions, or keep the record private if both parties prefer. If they disagree, the game leaves their positions visible without manufacturing an arbitration victory. Later dialogue can return to the jacket's practical use rather than replaying blame indefinitely.

## 502. Atomic transaction matrix — assets, callbacks and interrupted play

The settlement acceptance matrix covers ordinary success, unavailable item, quantity mismatch, access denial, stale consent, duplicate confirmation, cancellation, owner timeout and save/restore during a pending result. For success, both sides transfer once and the receipt matches the confirmed terms. For any precondition failure, neither side transfers. For a timeout with unknown outcome, the UI checks transaction status before it permits a retry. For cancellation before settlement, both parties retain their items. For a saved pending offer, reload restores the accepted or pending state from the transaction owner rather than replaying a UI callback.

The offer preview is a projection of current owner state. It must not hold a second copy of item quantities that can diverge from inventory. When a trade is accepted, settlement should use stable participant and item identities, validate ownership, revalidate availability, and commit all sides under the owner's supported atomic contract. If one item has split stacks or condition, the selected portion and resulting remainder must be explicit. If the owner cannot represent that identity, the feature should limit which items can be traded.

A player can close the barter panel while an offer is pending. Closing changes presentation, not transaction state. Reopening retrieves the same offer. If a participant withdraws before acceptance, the offer closes without transfer. If they withdraw during a non-cancellable settlement, the panel explains that the transaction is being resolved and reports the owner's result. Do not offer a cancel button that implies an action the owner cannot honor.

A receipt is created only after the owner confirms completion. It lists both participants, transferred assets, accepted favor terms where supported, time, and any supported dispute status. A transaction failure can have a receipt-like record of the attempt only if the current owner distinguishes attempts from completed transfers. Never display a “trade complete” toast before commit. An observer character can comment after success, but that comment is not evidence of the transfer.

The recovery path includes an explicit no-history case: a new campaign has no old offers. A mid-save case loads one pending counteroffer exactly once. A settlement replay case returns the original result. A stale item case reports which condition changed. A restore mismatch is surfaced as an owner error and blocks a second attempt pending authoritative recovery. These are not narrative branches but foundational truths underneath every authored arc in this plan.

## 503. Supporting groups — useful witnesses, limited authority

A repair circle remembers which tools were used for a job. A sewing group knows who supplied a scrap. A meal coordinator can report what portions are authorized today. A storekeeper confirms shared stock. A courier pair may witness a delivery if the parties ask. A neighbor may remember an earlier agreement. These support roles enrich the exchange with local knowledge and optional scenes while the existing item, inventory, work and save owners remain authoritative.

The player can choose whether to ask for a witness. A public handoff may benefit from one, while a private exchange should not recruit a room full of observers just to make it feel legitimate. A participant can decline to share a reason. If a witness cannot remember a detail, they say so. If two witnesses disagree, the game does not turn them into a faction war. The player can narrow the agreement, pause the transfer, or let the parties decide whether to proceed despite uncertainty.

Each group has one practical limitation. The repair circle may lack a needed part. The sewing group can mend cloth but not certify that a jacket is weatherproof. The kitchen can offer only current approved portions. Stores cannot trace a gift without a record. Couriers can confirm a handoff but not what was inside a sealed bundle. The player learns these boundaries through conversation, not a locked-reputation threshold.

An arc can invite a support group into a follow-up: the tool circle labels shelves after the drill dispute; the sewing group keeps a scrap box with donor consent; the meal coordinator posts current exchangeable portions; the courier pair offers a witnessed route for people who prefer it. Each is an optional improvement to the local scene. None introduces a competing inventory, ledger or favor system.

## 504. Flavor bank — spoken terms with character

Trade dialogue should sound like people naming what they have, what they can spare and what they cannot promise. Toma: “The coat is mine. The bandages aren't.” Ren: “I can sit with your brother. I can't carry crates until the clinic checks my knee.” Oren: “You said you owed me. I want to know what you meant before I ask.” Lea: “I don't need you to say you broke it. I need a pan I can cook with.” Sadi: “I can patch the handle. I can't tell you when it cracked.” These lines create decisions and preserve each speaker's limits.

For a quick success, the receipt exchange can prompt: “Both sides are ready. Confirm the coat for two cloth rolls?” If a quantity changes: “Toma changed two rolls to one. Ren has not accepted the new offer.” If stock changes: “The bandage is no longer available from the clinic. Nothing moved.” If the player withdraws: “No exchange recorded. Both residents still hold their items.” The wording is plain so a player can understand whether the world changed.

A resident who dislikes negotiation may say, “Tell me the terms once. I don't want to haggle all afternoon.” Another may prefer to consider: “Leave the offer open until morning, but don't set the coat aside yet.” A player can respect either preference. The game should not equate concise negotiation with selfishness or careful questioning with virtue. Distinct voices come from pace, material context and what each person chooses to explain.

Use ambient lines to make settlements feel social without claiming transaction facts. A child might ask why the drill is back on the shelf. The carpenter says, “Because we didn't prove it was mine.” A cook may tell the gardener, “Keep the seeds. I found today's portions.” A witness may say, “I saw the pan come back. I didn't see what happened to it.” The best flavor often clarifies what a character knows and what they do not.

All sample lines are proposals. Before adoption, check existing character voices and data IDs. A new line should not imply that a loan, favor enforcement, condition grade or dispute appeal exists unless the relevant owner supports it. Where an owner records less detail, the narrative needs to speak at that narrower level.

## 505. Three-party request — keep settlement pairwise

A cook wants a mesh strainer from the repair circle and offers a bowl of preserved fruit. The repair circle says the strainer belongs to the shared bench, not to any one member. A third resident offers a personal sieve if the cook lends them the fruit bowl for a later gathering. This is a three-party conversation, but it should not become a three-way atomic barter if the current owner supports only two participants. The player can separate it into pairwise agreements, find a privately owned strainer, ask the repair circle to loan the shared tool through its normal route, or stop the exchange.

If the repair circle authorizes a loan, the cook and circle use the supported shared-tool process. The fruit bowl remains out of the transaction. If the third resident offers their own sieve, they can negotiate directly with the cook, while the player helps clarify the terms. The cook may accept the sieve but decline to lend the bowl. If the current system cannot express a time-bounded loan, the player can ask the resident to keep the sieve until the cook is done or return to the shared bench for another option. The feature must not bundle unsupported transfers merely because the dialogue makes them sound convenient.

The player may coordinate separate pairwise exchanges in sequence. After the first settles, the second is revalidated because the cook's available goods may have changed. If the first agreement fails, the second remains unaccepted. No item is promised twice. If one participant wants a multi-party arrangement, the player can explain that each owner must approve its own transfer and that the plan cannot guarantee a simultaneous basket without a supported contract. They can decide whether to proceed in stages, use no trade or ask an integrator for an architecture decision.

The repair circle can confirm the strainer's maintenance state. The fruit preserver can explain storage time. Neither can commit the bowl unless its owner agrees. A supporting clerk can help write two clear receipts if that is within current owner behavior, but cannot merge them into a new ledger. The player who favors speed may choose the private sieve; one who favors shared access may request the standard loan; one who dislikes interconnected promises may end the chain after the first agreement. Each playstyle has an intelligible result.

The ending may be a small successful trade, a shared tool loan and no barter, a single pairwise exchange after the other one falls through, or a decision to serve the fruit without the strainer. The meal can still happen. The complication teaches a useful limit without making the player solve a network of debts.

## 506. Questline — A child wants to trade a carved bird

A young resident offers a small carved bird in exchange for a roll of colored thread. The thread is in a shared craft box, and the child has permission to use it for a lesson but not to barter it. An adult may be nearby, but their presence does not automatically grant authority over either person's belongings. The player can ask who owns the bird, ask the craft steward about the thread, help the child choose a personal item they may offer, or decline to broker a trade that uses shared stock.

The child says they carved the bird from a permitted scrap. The intended recipient likes it but asks for a second bird. The player can explain that making another one takes time, suggest a smaller exchange, or let the child set a boundary. The child may say the first bird is the only one they want to trade. That refusal closes the second-item request; it does not make the first agreement invalid. If the recipient counters with a ribbon they own, the child can accept, ask for another color, or withdraw.

If the thread can be issued for lesson use, the player might help both children make a shared craft instead of settling a property exchange. That action is not barter unless each owner agrees to transfer an item under the supported contract. A craft mentor can explain which scraps are available, but does not decide what the child should give. A guardian can help clarify consent if the children request assistance, but should not automatically speak over them. The game can present age-appropriate plain terms while preserving agency.

A successful ending has the children accept one small exchange and later sit together to carve another bird. A second ending keeps the bird with its maker; the recipient decides to draw a picture instead. A third has the player return the thread to the shared box and explain why it cannot be traded. A fourth leaves the offer open until the next lesson, when the recipient brings a ribbon. The transaction receipt uses no adult's name as a substitute owner. If the relevant system does not model underage participation, the content needs an explicit design review before implementation.

This case tests whether a barter feature assumes all survivors can negotiate under the same conditions. The scene should not gamify pressure, encourage an adult to extract a child's property, or infer ownership from who made an item alone. The player's actions, the child's stated permission and the thread's actual source determine the branch.

## 507. Disagreement without a judge — the two cups

Two residents each claim a dented metal cup. Both have used it at the common table. One says it came from their kit; the other says it was taken from an unlabeled shelf. The player can check for a maker's mark, ask the storage keeper whether there is a matching kit record, or help them use different cups while the question remains unresolved. They can also ask whether either person is willing to trade a cup they clearly own, separating the immediate need from the contested object.

The maker's mark is worn. The kit record lists a cup but not the dent. The shelf count is one cup short, but the location of the missing cup is unknown. Each fact narrows the possibilities without settling the claim. One resident suggests flipping a coin, but the other declines. The player can respect that refusal, propose a neutral storage hold if an owner supports it, or keep the cup in place and make it unavailable for trade until they agree. No arbitration result is generated by a minigame.

A resident who wants the cup for a night shift can borrow a different one through the existing kitchen route. Another may prefer to pause and ask an absent roommate. The player can carry the question forward, leave a note with consent, or close the conversation until new evidence appears. If the roommates later confirm the cup was shared property, they can donate it to a common shelf. If one presents an old kit list, they may agree to mark their initials on the cup. If no evidence comes, both can choose to stop contesting it without declaring a winner.

A ledger volunteer can explain how the kit records were made; they cannot certify a worn cup's provenance. A kitchen steward can issue a substitute; they cannot force either resident to accept it. The player may prioritize immediate use, documentation or emotional de-escalation. The resulting branch is a grounded action and later dialogue, not a score change.

The closeout makes explicit whether a trade occurred. If no ownership was established, the contested cup does not move. If a substitute is issued, that follows its actual owner. If the residents agree on a shared-use arrangement that the current owner cannot persist, it remains an informal proposal and cannot be presented as a saved schedule. A small uncertainty is allowed to stay unresolved.

## 508. The market day that almost became a faction vote

Before a community meal, several residents propose a table for private barter. A major faction representative wants the table to use the faction's seal, arguing that the seal would discourage theft. A seamstress says she will bring repaired gloves only if the event remains open to people outside that faction. The player can ask the representative to support neutral rules, hold separate bilateral exchanges without a public market, ask each participant what makes them comfortable, or cancel the table and keep the existing trade route.

The player cannot turn the support circle into a rival market authority. The table is a temporary meeting place, not a new pricing or property system. Each item transfer still uses the current owner, and each pair reaches its own terms. A posted card can explain that only personal goods may be offered and that shared stock needs its owner's approval. The representative may lend two benches, but the benches remain faction property and need to be returned. The seamstress can bring gloves without endorsing the faction. A resident can participate privately or not at all.

If neutral rules are agreed, a modest market happens. Some offers settle; others end in counteroffers or no exchange. If the player chooses bilateral meetings, the event becomes slower but more private. If the representative insists on the seal, the player can refuse that condition and either seek another venue or cancel. The faction may withdraw the benches, creating a real practical cost. A cooking group can offer floor mats from its own stock if authorized; it does not run the market or force participation.

The event's ending can vary: a neutral barter table with three completed exchanges; two private trades after the public venue falls through; a canceled meeting with a later direct arrangement; or a faction-branded table that only proceeds if every participant who uses it knowingly agrees. The player does not need to defeat or flatter anyone. What matters is whether participants understand the terms and whether item owners authorize transfers.

A participant can report that the representative watched every exchange and made them uncomfortable. The player can move to a quieter location, ask the representative to leave, or allow the participant to withdraw. The table closes if no one wishes to continue. If a dispute arises over a glove's repair quality, it goes back to the two participants and the item's actual condition, not the faction seal. The faction remains supporting context, not a substitute gameplay authority.

## 509. Slow offer — goods are not reserved by a conversation

Nara offers a tin cup in exchange for a wool scarf but asks for a day to decide. The player can leave the offer pending without reserving either object, ask Nara to set an expiry, or tell the other resident that the offer may not remain available. If the barter owner supports explicit reservation, the screen names the reserved item and release time; otherwise, the offer must state that both parties remain free to use their property before acceptance. A pending conversation is not an invisible lock.

Before expiry, the scarf is given to someone who is shivering. The player can tell Nara that the offer changed, ask whether she wants to propose another item, or let the offer expire. The cup remains hers. The other resident may be disappointed, but the player has not broken a settled trade. If the scarf's owner wants to offer a blanket instead, the item is rechecked for ownership and availability. Nara can accept, counter or decline.

If the offer expires while the player is away, return from an expedition shows it closed with its last terms; it does not silently reopen. If a participant wants to renew it, the owner creates a new offer with current items and a new expiry. A player can show the prior conversation as context, but the earlier acceptance cannot carry forward. If a pending offer saves and reloads, its expiry and status should come from the authoritative owner and remain deterministic.

A resident may offer to hold an item informally for a short period. The player can help both parties say what “hold” means, but cannot represent an enforced reservation unless the current owner supports it. The interface can record a note or message only if that is already an available feature and cannot imply that the item is protected from other use. A clear refusal to promise availability is preferable to a false guarantee.

The ending may be acceptance before expiry, an amended offer, no trade after the scarf is used, or renewal as a fresh negotiation. These branches create tension through time and genuine resource use, not through hidden penalties for walking away.

## 510. Return and restitution — the borrowed lantern

A resident returns a lantern after using it on a route. The owner notices a cracked glass pane. The borrowed item authority may be separate from barter, so the player first follows its real return and condition procedure. Only after the lantern is returned can the owner propose a barter or repair arrangement. A promise to replace the pane does not restore the lantern's condition. The player can ask whether the owner wants repair, replacement, a contribution to materials or simply the lantern back.

The borrower says a branch fell during a storm. The route witness confirms that the group sheltered under a tree but did not see the lantern hit the ground. The owner may accept the explanation while still asking for repair. The player can help source a pane, ask the repair circle whether the glass can be patched, check for an existing spare or stop the conversation. If replacement material comes from shared inventory, its owner authorizes the draw. If the owner prefers a different lantern, the borrower can offer a personal item only if they choose.

An ending has the repair circle fit a pane and record the lantern's new condition. Another has a spare lantern transferred through the proper store route while the cracked one stays with its owner. A third accepts a partial material contribution but no promise that the repair will be completed. A fourth leaves the lantern cracked and out of service because no one agrees on the proposed costs. The relationship may remain awkward, but the property state is clear.

The owner may offer to trade the repaired lantern for a tool after all, but this is a new deal and must be reviewed from current state. The prior dispute cannot auto-accept it. The player can ask for a witness at the handoff, or allow the residents to settle directly. A route volunteer can describe the fall, but does not determine liability. The major authority can set safety rules for lantern use; it does not dictate repayment between residents unless an existing policy says so.

This story closes a loop between loan, return, condition and barter without merging them. If the current owners cannot hand off state between those steps, the implementation audit must record that seam before the feature proceeds. A convincing narrative chain cannot paper over a missing contract.

## 511. Follow-up ending atlas — what stays after a deal

After a trade settles, the relationship can change through explicit conversation, but the transaction itself must not silently create a universal reputation effect. A resident may offer future trades because a previous receipt was clear. Another may prefer direct negotiation after feeling over-managed. Someone whose offer expired can choose to make a fresh one. The player's follow-up actions—returning an item, honoring a changed term, checking on a repair, or accepting refusal—provide the basis for each authored line.

**Practical closure:** Both items transferred and a task was completed. The residents move on; the receipt is enough. **Repair closure:** A disputed item is usable again, though the reason for damage remains unknown. **Boundary closure:** A resident declines future favors, and the player respects that request. **Deferred closure:** A claim awaits evidence, with the item held or left in place according to the owner. **Non-trade closure:** Both residents find another way to meet the need. **Renewed exchange:** A later offer is created from current state, not resurrected from a stale promise.

The player's playstyle changes scene framing. A careful player may get a short acknowledgment that the terms were clear. A fast player may hear that a useful substitute saved the day. A player who delegates may see a witness remember a handoff. A player who steps back may let two residents settle privately. These lines should reference observable events without declaring that one style is always superior.

The story should not repeatedly reopen a settled trade for dramatic noise. An accepted agreement has a clear closure condition. A completed transfer is not undone by a later argument unless a supported return or dispute process occurs. If a participant says they regret the deal, the player can help negotiate a new agreement; the original record remains truthful. If no one asks to revisit it, ordinary play continues.

## 512. Closeout checklist — peer barter scope and evidence

Before implementation, verify the current transaction owner can represent both sides of a trade, item ownership and quantity, condition where available, atomic settlement, repeat confirmation, cancellation, expiry and recovery after save/load. Verify whether favor terms exist and how they close. Identify which items are eligible and how shared stock is excluded or routed through its owner. Confirm whether transaction receipts are persisted and whether a failed attempt leaves a durable record. These are evidence questions, not approvals to add new stores or ledgers.

For narrative, confirm each exchange has a start, a decision set, meaningful no-trade branches, a truthful transfer result and a follow-up that does not overstate saved relationship state. Confirm side groups help with bounded tasks and do not control the major factions or the player's campaign. Confirm different playstyles emerge through action: verify, delegate, improvise, preserve privacy, wait, revise, or walk away. Confirm refusal does not automatically mean hostility or moral failure.

The authored examples in this plan are proposals. Current character names, item IDs, quest callbacks and faction details need a fresh audit against live data. If an example requires an unsupported three-party atomic exchange, loan reservation or favor obligation, keep that example as an open design question or simplify it to the current owner contract. Never let a dialogue option imply that an item moved when it did not.

Plan 5 is ready to close as a content document once its final measured length is at least 120,000 words and its header and wave index are reconciled. This closeout is not a feature implementation claim, a new path claim or a new integration decision. A future foreman can use the scenarios to define one narrow integration package after checking the live authority.

## 513. Questline — The spool that ran short

A tailor offers to repair a torn sleeve for a tin of lamp oil. The oil owner agrees, but the spool runs short before the second stitch. The tailor can finish with a different thread, return the sleeve unfinished, ask for a second spool or offer to repair only the cuff. The player may inspect the material, help find a compatible spool through the current inventory owner, revise the offer or let the two residents choose. The first spool does not disappear into a vague “repair cost”; its actual use is recorded only by the owner that tracks it.

The sleeve belongs to the oil owner, but the coat is used by their sibling on a night watch. The player can ask whether a temporary repair is acceptable, find a spare coat, or ask the sibling what they need before settlement. If the sibling asks to keep the coat for tonight, the owner can agree or decline. Their request does not transfer ownership. If the tailor suggests keeping the oil until the repair is complete, the player checks whether the barter owner supports conditional transfer. If not, no oil moves until the accepted exchange can settle atomically.

A compatible spool is found in a sewing kit. The kit's steward allows a small amount for communal repairs but not for private barter. The player can ask the owner to amend the offer so the tailor does the work as a contribution, or find a spool that the tailor owns. The tailor may choose to finish the cuff only, and the coat owner can accept that reduced outcome. Each revised term is visible before commitment. A player may also stop the trade and return the coat exactly as it is.

The seamstress circle can test a stitch on scrap cloth and explain which thread is durable. It cannot promise how the coat will perform in rain. A night watch resident can report whether the sleeve must be covered, but does not decide what work the tailor owes. The player can prioritize a timely minimum repair, a full repair later, or preserving the oil. The outcomes vary according to what the parties value, not a hidden fairness score.

Ending A completes a cuff repair and transfers the agreed oil once. Ending B leaves the coat with its owner and schedules a second attempt after a compatible spool is available. Ending C trades for a different service, such as mending the tailor's own work apron in return. Ending D closes the trade because the night watch needs the coat immediately and no safe repair can finish in time. A later conversation can thank the tailor for the attempt without claiming completion. The unresolved stitch remains visible in the item state.

## 514. Questline — A recipe shared, not sold

A cook offers a handwritten soup recipe in exchange for two clean jars. Another resident says the recipe came from a family notebook and should not be copied publicly. The cook says they learned it from an old communal meal and are willing to teach it to one person. The player can ask what the cook intends to transfer, clarify whether this is a lesson rather than property exchange, ask the other resident about their concern, or stop the barter until both agree on how the recipe may be used.

The jars belong to the recipient, but one is already promised for medicine storage. They can offer a single jar, a different container, or no trade. The cook can accept a smaller exchange, teach the recipe without taking jars, or withdraw. If the recipient wants to write the recipe down, the cook can set a boundary that it remain private. The communication and barter owners must not mark a public recipe as transferred if the actual agreement is a one-time lesson.

A meal group may ask to use the soup for a communal supper. The cook can say yes, offer a simplified version, or say the recipe stays private. A public meal does not create permission to reproduce every ingredient or family detail. The player can invite the cook to lead the kitchen session, ask another resident to teach a different recipe, or serve a familiar dish. The meal remains possible even if this barter ends.

One ending has a private lesson in exchange for one jar, with the cook retaining the recipe. Another has the cook share an adapted version freely and decline the jars. A third has both residents agree on a one-time public cooking demonstration without distributing written copies. A fourth closes the negotiation when the intended use cannot be represented by the current barter owner. The outcome is about consent to share labor and information, not just possession of items.

A pantry steward can confirm that the jar is not reserved for medicine; the family member can explain the recipe's context if invited; the meal group can arrange a kitchen time. None dictates what the cook may share. The player can choose a quiet negotiation, a public teaching event or no event. Later flavor can show a resident cooking from memory without claiming that the recipe was copied into a global catalog.

## 515. Loss condition — both residents change their minds

A trade is accepted but has not settled. One resident learns that the item is needed for an urgent repair; the other decides that the proposed favor would interfere with their night shift. Both withdraw before confirmation. The player can close the offer, ask whether they want to propose different terms, or leave the meeting. No item changes hands. The interface should say that the offer was withdrawn by its participants and that neither side transferred property.

The player may feel pressure to salvage the deal because the repair is time-sensitive. They can ask the repair owner for another suitable item, ask whether the job can be delayed safely, or request a volunteer who has not made another commitment. They cannot tell either resident that withdrawal would be selfish. If an emergency authority can requisition property under an established rule, that is a different owner and a different command; ordinary barter cannot smuggle compulsion into a confirmation prompt.

If the repair can use an alternative, the first resident keeps their item and the player offers the alternative to the repair owner. If no alternative exists, the work waits or follows its existing escalation route. The second resident can offer a smaller favor on another day or decide not to. If both residents later reopen the exchange, a new offer is created from the then-current inventory, work schedule and consent. Its terms may differ from the old one.

A supporting repair circle can help identify substitutes. A work steward can verify the night shift. The two residents own their participation. The player can move quickly to contingencies, document the withdrawal neutrally, or give the participants space. These playstyles lead to different scene pacing without framing anyone as unreliable or disloyal.

The closeout checks that the accepted-but-unsettled state is not confused with completed settlement. The accepted offer can close as withdrawn, expire, or return to negotiation. The user should see no transfer, no favor obligation and no receipt claiming a completed exchange. If the system's current contract cannot express withdrawal after acceptance, this branch marks an implementation question and not a license to invent a new cancellation path.

## 516. Ending vignette — the empty place on the shelf

After several trades, the player sees an empty place where the shared drill used to rest. The tool was checked out through its ordinary owner, not sold. A resident has left a note saying it will return after the repair. The player may check the due time, ask the borrower whether the schedule changed, or let the matter stand because no decision is needed. If the borrower reports that the drill broke, the owner process handles its condition. The player does not create a barter dispute simply because the shelf is empty.

Later, the carpenter offers a personal wrench to a neighbor who needs a quick adjustment. The two residents negotiate directly. The player can watch, offer to clarify a term, or walk past. If they settle, the wrench leaves one inventory and enters the other once. If they decide not to, the wrench stays where it was. The empty shelf and the new exchange are separate events, even though both involve tools.

The visual endpoint is ordinary: the common shelf has a return note, a marked space for the missing drill and a spare hand file. Someone has written “ask before borrowing” beneath the label. This line does not turn the shelf into a new authority; it is a reminder proposed by its users. The player can help make the label clearer, ask the storekeeper to update the inventory entry, or leave it alone. One small negotiated habit can make the shelter feel lived in without requiring a sweeping faction resolution.

A resident who benefited from an earlier trade may mention it with restraint: “The coat held through the cold night. I gave it back after the seam was checked.” Another may say, “We never found a trade that worked for the cups.” The world remembers completed action where it has evidence and lets unresolved matters remain unresolved. Plan 5's strongest ending is not that every item finds a buyer; it is that players can trust the game to show who owned what, what was agreed and whether it actually happened.

## 517. Final authored-scope audit

Every scene in this expansion should pass a plain-language audit: can the player name both participants; identify who owns each item; state what each side expects; see whether either term is a future favor, a loan or an immediate transfer; and understand what happens if one side changes before settlement? If a player cannot answer those questions, the scene should pause for clarification instead of resolving through an inferred value or relationship score. The opportunity to walk away remains visible.

Every supporting role should answer a bounded question or perform a bounded service. A witness remembers a handoff; a steward confirms shared stock; a repairer reports condition; a meal coordinator reports today's portions. None decides a participant's consent or takes over an existing inventory owner. Every ending should preserve the difference between offer, acceptance, settlement, return, repair and dispute. The receipt and narrative must agree.

The expanded casebook now contains trades that complete, change, expire, fail safely, become a favor, return as a fresh offer or remain unresolved. It includes practical, private, social and time-sensitive playstyles. Its content minimum is reached only after the saved file is measured and the header and index are synchronized with that result. That number marks editorial completion; code and data authority remain with the existing system owners.

## 518. Small ending — a trade declined kindly

A resident offers a polished button in exchange for the player's last spool of thread. The player can accept if the thread is theirs, counter with a smaller length, ask why the button matters, or decline because the spool is already promised to a repair. The resident may say the button came from a coat they lost, or choose not to explain. The player can simply say the thread is unavailable. No one needs to turn a small refusal into a dispute.

If the resident asks again later, the player can answer with current facts: the spool was used, the repair is still pending, or a new one has been found. If they want to make a fresh offer, both sides review the terms again. A later trade can be accepted, revised or declined. The button stays with its owner until settlement; the thread stays with its owner unless the repair consumes it through the correct authority. The brief scene ends with a nod, not a reputation award.

This example is deliberately tiny beside the larger questlines. It reminds the plan that every barter interaction need not become a campaign branch. A clear offer, a respectful no and an unchanged inventory state are already a complete authored outcome.

A final usability pass should read the offer aloud, review it with keyboard and controller focus, and confirm that unavailable terms are explained before acceptance. The player should be able to back out without losing either resident's item or creating a social consequence the game cannot prove. After a successful exchange, the receipt should use the same quantities and condition the participants saw on the confirmation screen.

No exchange should be announced as complete until the transaction owner confirms both sides. If settlement is unavailable, report the exact blocked term, preserve ownership and let the residents decide whether to retry, revise or leave the offer closed.

After a failed attempt, the screen returns to the pending offer with a clear status, not an empty panel. The player can reopen it only after the authoritative owner reports that no transaction remains active.

This closes Plan 5's authored content pass at the requested minimum.
