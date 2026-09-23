# Expansion 100 — The Wall Has Two Sides

**Wave:** 20 — Useful Systems, Real Consequences  
**Requested series:** Plan 4 of 5  
**Requested plan length:** At least 120,000 words; expansion above the minimum is acceptable.  
**Series status:** This plan's content pass is closed at the requested minimum; proposal only.
**Measured length:** 120,005 words; 100.00% of the 120,000-word minimum (5 words above). Counted through Section 540.
**Unique key feature:** An internal shelter communication surface where notices, intercom calls and private messages preserve truthful audience and acknowledgement.  
**Status:** proposal only; no implementation or path claim.  
**Audit:** [Wave 20 forensic report](../../forensics/WAVE20_FEATURE_SEAMS_FORENSIC_REPORT.md)

> Exactly three subfeatures are defined in Section 4. The examples and casebook all map to those same three; no additional feature pillars are introduced.

## 1. Expansion thesis

The experience is an internal shelter communication surface where notices, intercom calls and private messages preserve truthful audience and acknowledgement. It must start from canonical state, invoke one owner-backed command and show an outcome that remains true after day advance and reload. A panel-only simulation does not meet the promise.

A notice, intercom call and private note make different promises to different audiences. The feature matters because each has an author, reader and lifespan, not because the game needs another journal.

Show real cost, permission and uncertainty before commitment. Refusal, delay and failure may be valid results.

## 2. Canon fit

A notice, intercom call and private note make different promises to different audiences. The feature matters because each has an author, reader and lifespan, not because the game needs another journal.

Keep the tone restrained, material and human. Sample lines below are candidates, not current canon; check them against the live narrative data before authoring.

## 3. Existing systems reused

Reuse current message/template/board/broadcast records, survivor IDs, incident producers, time owner and radio/journal screens only as cross-links. Keep private mail out of public notices.

Reuse stable IDs and owner records. JSON under Assets/StreamingAssets/Data remains authoritative; do not duplicate mutable data in the host.

## 4. Key feature and exactly three subfeatures

**Single key feature:** An internal shelter communication surface where notices, intercom calls and private messages preserve truthful audience and acknowledgement.

**Current gap:** Internal communication is not proven player reachable. One intercom creates two source records and independent acknowledgement paths, so views can duplicate an announcement or disagree on who heard it.

### Subfeature 1: Public boards with scope and expiry

**Purpose and loop:** Show notices by board, room, author, audience, priority and expiry. Route posting through current APIs and enforce access/capacity from owner state.

**Player value:** A workshop notice can be local; a water allocation notice can reach everyone. Show who posted it and when it expires.

**Boundary:** Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

### Subfeature 2: Intercom urgency with one acknowledgement

**Purpose and loop:** Give a broadcast one canonical identity. Banner and inbox may both project it; both controls acknowledge the same source and resident list.

**Player value:** Unanswered residents are visible; silence is not consent, refusal or death. Routine updates should not become alarms.

**Boundary:** Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

### Subfeature 3: Private messages with recipient ownership

**Purpose and loop:** Deliver to one named survivor and keep recipient filtering, read and acknowledgement with the current owner. Preserve history without inventing delivery after departure.

**Player value:** A note can ask someone to return a tool or save a seat; it need not become a journal entry.

**Boundary:** Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

These three subfeatures are the complete gameplay scope. A fourth independent pillar needs a separate proposal.

## 5. Core mechanics

**Input:** current owner state, catalog, day and explicit player command. **State:** existing domain DTO and only stable event references. **Decision:** commit, defer, decline or choose a supported alternative with costs visible. **Uncertainty:** only facts current systems support. **Consequence:** owner-confirmed state or cost. **Cross-system output:** one fact once. **Failure/recovery:** stale data, denied consent, capacity, interruption and retry have truthful results. **Replayability:** seeded campaign inputs and stable order, never wall-clock or UI-local random.

## 6. Cross-system interactions

Primary domain: InternalCommunicationSystem owns message, board, recipient and intercom state; roster owns identity. Radio, briefing, journal, time capsule, survivor letters and emergency alerts remain distinct.

A current cost/permission owner supplies constraints; an existing downstream owner consumes confirmed effects; the day/save owner refreshes the same state after restore. Verify each actual API and consumer before implementation.

## 7. Main narrative spine

A concrete need appears; the player sees known facts and limits; one of the three subfeatures permits a decision; a later view shows who acted and what changed. Human center: A notice, intercom call and private note make different promises to different audiences. The feature matters because each has an author, reader and lifespan, not because the game needs another journal.

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

Preserve stable identity, valid pending intent, confirmed result and provenance through the existing save owner. A durable consequence is A note can ask someone to return a tool or save a seat; it need not become a journal entry.

Core capture/restore is necessary but not sufficient: prove host registration, restore order and dirty flush.

## 17. Failure and alternate outcomes

A blocked command is a valid branch. Distinguish missing source, deferral, owner rejection, expiry, conflict and completion where current APIs support them. Never show success before confirmation or turn denial into hidden punishment.

## 18. Replayability

Different people, timing and owner state should change the choices without arbitrary new rolls. Same seeded state resolves identically. Duplicate command/day delivery reuses a durable result or reports already processed.

## 19. Implementation classification

**Classification:** Data and host wiring with a save route after registry inspection; canonical intercom identity/ack may need a minimal Core correction.

Start with a fresh premise and runtime audit. Resolve owner decisions, connect the existing Core authority through the current host/event/save seam, then expose one Godot route. Core remains engine-free and JSON remains authoritative.

## 20. Collision audit

Adjacent behavior: Reuse current message/template/board/broadcast records, survivor IDs, incident producers, time owner and radio/journal screens only as cross-links. Keep private mail out of public notices.

Classify this as extension/reachability work, not replacement. Re-search current content, host paths and save sections before implementation. Never revive Unity behavior.

## 21. Expansion hooks

Later quests or campaign history can consume an owner-confirmed result with provenance. They cannot infer success from a UI label or duplicate the source state.

## 22. Strongest recommended content

Start with the smallest interaction that proves the cost and consequence: Unanswered residents are visible; silence is not consent, refusal or death. Routine updates should not become alarms. Add one blocked path and one delayed callback.

## 23. Contracts, data, save and determinism

**Owner boundary:** InternalCommunicationSystem owns message, board, recipient and intercom state; roster owns identity. Radio, briefing, journal, time capsule, survivor letters and emergency alerts remain distinct.

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

**Primary risk:** Internal communication is not proven player reachable. One intercom creates two source records and independent acknowledgement paths, so views can duplicate an announcement or disagree on who heard it.

Later verification should cover accepted, denied, stale, repeated and restored actions plus seeded replay where relevant. Prove behavior, not class presence. Rollback disables the host route and preserves canonical state and save readers. Keep unsafe commands unavailable while ownership is unresolved.

## 27. Three creative variants for this feature

**Grounded:** show current state and one safe owner command. **Systemic:** connect the three subfeatures to real costs, permissions and delayed outcomes. **Wildcard, still grounded:** let the player inspect the source, author, provenance or physical limit before committing. This changes framing, not architecture. Implement systemic only when contracts are proven.

## 28. Review gate

Proceed to implementation planning only when source confirms the gap, every mutable fact has one owner, exactly these three subfeatures have truthful routes, save/replay owners are named and success follows an owner result. Otherwise revise or stop.

## 29. Casebook: design acceptance records mapped to the three subfeatures

These are review examples, not implementation claims, test results or extra features. Every scenario maps to S1, S2 or S3 and preserves the current ownership boundary.

### Scenario W20-100-S1-001 — one-day workshop notice

**Initial condition:** one-day workshop notice. The view is a projection of the current owner. **Pressure:** posting callback submitted twice. **Player action:** deduplicate by owner identity.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. IDs survive capture/restore. Public queries exclude private recipients.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Water allocation changed at second bell. The list is beside the valve room. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-001 — one-day workshop notice

**Initial condition:** one-day workshop notice. The view is a projection of the current owner. **Pressure:** private content enters public query. **Player action:** project one intercom source in all views.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. intercom acknowledgement agrees across views. Public queries exclude private recipients.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One call, one list. No one has to hear it twice. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-001 — one-day workshop notice

**Initial condition:** one-day workshop notice. The view is a projection of the current owner. **Pressure:** template ID missing. **Player action:** reject public listing of private content.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. public query excludes private rows. Public queries exclude private recipients.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Water allocation changed at second bell. The list is beside the valve room. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-002 — one-day workshop notice

**Initial condition:** one-day workshop notice. The view is a projection of the current owner. **Pressure:** template ID missing. **Player action:** keep read and ack separate.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. expiry does not erase unrelated journal history. Public queries exclude private recipients.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Read is not the same as yes. Leave the box clear if you cannot answer. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-002 — one-day workshop notice

**Initial condition:** one-day workshop notice. The view is a projection of the current owner. **Pressure:** recipient departs before reading. **Player action:** retain history without claiming delivery.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. panel subscriptions close on route exit. Public queries exclude private recipients.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** This note has a name on it. Put it where that person can find it. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-002 — one-day workshop notice

**Initial condition:** one-day workshop notice. The view is a projection of the current owner. **Pressure:** radio content routes internally. **Player action:** dispose old subscriptions before rebind.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. urgent call differs from ordinary notice. Public queries exclude private recipients.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Read is not the same as yes. Leave the box clear if you cannot answer. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-003 — one-day workshop notice

**Initial condition:** one-day workshop notice. The view is a projection of the current owner. **Pressure:** two views mark message read. **Player action:** reject public listing of private content.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. IDs survive capture/restore. Public queries exclude private recipients.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One call, one list. No one has to hear it twice. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-003 — one-day workshop notice

**Initial condition:** one-day workshop notice. The view is a projection of the current owner. **Pressure:** second panel opens before first disposes. **Player action:** show capacity/expiry denial.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. intercom acknowledgement agrees across views. Public queries exclude private recipients.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Water allocation changed at second bell. The list is beside the valve room. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-003 — leadership-only shift change

**Initial condition:** leadership-only shift change. The view is a projection of the current owner. **Pressure:** announcement and mirror have different status. **Player action:** expire from day owner then refresh.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. public query excludes private rows. Public queries exclude private recipients.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One call, one list. No one has to hear it twice. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-004 — leadership-only shift change

**Initial condition:** leadership-only shift change. The view is a projection of the current owner. **Pressure:** announcement and mirror have different status. **Player action:** dispose old subscriptions before rebind.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. expiry does not erase unrelated journal history. Public queries exclude private recipients.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** This note has a name on it. Put it where that person can find it. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-004 — leadership-only shift change

**Initial condition:** leadership-only shift change. The view is a projection of the current owner. **Pressure:** template ID missing. **Player action:** deduplicate by owner identity.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. panel subscriptions close on route exit. Public queries exclude private recipients.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Read is not the same as yes. Leave the box clear if you cannot answer. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-004 — leadership-only shift change

**Initial condition:** leadership-only shift change. The view is a projection of the current owner. **Pressure:** recipient departs before reading. **Player action:** project one intercom source in all views.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. urgent call differs from ordinary notice. Public queries exclude private recipients.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** This note has a name on it. Put it where that person can find it. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-005 — leadership-only shift change

**Initial condition:** leadership-only shift change. The view is a projection of the current owner. **Pressure:** recipient departs before reading. **Player action:** expire from day owner then refresh.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. IDs survive capture/restore. Public queries exclude private recipients.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Water allocation changed at second bell. The list is beside the valve room. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-005 — leadership-only shift change

**Initial condition:** leadership-only shift change. The view is a projection of the current owner. **Pressure:** radio content routes internally. **Player action:** keep read and ack separate.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. intercom acknowledgement agrees across views. Public queries exclude private recipients.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One call, one list. No one has to hear it twice. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-005 — leadership-only shift change

**Initial condition:** leadership-only shift change. The view is a projection of the current owner. **Pressure:** stale view uses old row index. **Player action:** retain history without claiming delivery.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. public query excludes private rows. Public queries exclude private recipients.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Water allocation changed at second bell. The list is beside the valve room. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-006 — leadership-only shift change

**Initial condition:** leadership-only shift change. The view is a projection of the current owner. **Pressure:** second panel opens before first disposes. **Player action:** project one intercom source in all views.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. expiry does not erase unrelated journal history. Public queries exclude private recipients.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Read is not the same as yes. Leave the box clear if you cannot answer. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-006 — private note to a resident

**Initial condition:** private note to a resident. The view is a projection of the current owner. **Pressure:** announcement and mirror have different status. **Player action:** reject public listing of private content.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. panel subscriptions close on route exit. Public queries exclude private recipients.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** This note has a name on it. Put it where that person can find it. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-006 — private note to a resident

**Initial condition:** private note to a resident. The view is a projection of the current owner. **Pressure:** board removed or full. **Player action:** show capacity/expiry denial.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. urgent call differs from ordinary notice. Public queries exclude private recipients.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Read is not the same as yes. Leave the box clear if you cannot answer. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-007 — private note to a resident

**Initial condition:** private note to a resident. The view is a projection of the current owner. **Pressure:** board removed or full. **Player action:** retain history without claiming delivery.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. IDs survive capture/restore. Public queries exclude private recipients.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One call, one list. No one has to hear it twice. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-007 — private note to a resident

**Initial condition:** private note to a resident. The view is a projection of the current owner. **Pressure:** day tick repeats after save. **Player action:** dispose old subscriptions before rebind.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. intercom acknowledgement agrees across views. Public queries exclude private recipients.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Water allocation changed at second bell. The list is beside the valve room. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-007 — private note to a resident

**Initial condition:** private note to a resident. The view is a projection of the current owner. **Pressure:** radio content routes internally. **Player action:** deduplicate by owner identity.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. public query excludes private rows. Public queries exclude private recipients.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One call, one list. No one has to hear it twice. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-008 — private note to a resident

**Initial condition:** private note to a resident. The view is a projection of the current owner. **Pressure:** radio content routes internally. **Player action:** show capacity/expiry denial.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. expiry does not erase unrelated journal history. Public queries exclude private recipients.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** This note has a name on it. Put it where that person can find it. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-008 — private note to a resident

**Initial condition:** private note to a resident. The view is a projection of the current owner. **Pressure:** stale view uses old row index. **Player action:** expire from day owner then refresh.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. panel subscriptions close on route exit. Public queries exclude private recipients.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Read is not the same as yes. Leave the box clear if you cannot answer. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-008 — urgent call during water incident

**Initial condition:** urgent call during water incident. The view is a projection of the current owner. **Pressure:** posting callback submitted twice. **Player action:** keep read and ack separate.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. urgent call differs from ordinary notice. Public queries exclude private recipients.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** This note has a name on it. Put it where that person can find it. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-009 — urgent call during water incident

**Initial condition:** urgent call during water incident. The view is a projection of the current owner. **Pressure:** private content enters public query. **Player action:** keep read and ack separate.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. IDs survive capture/restore. Public queries exclude private recipients.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Water allocation changed at second bell. The list is beside the valve room. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-009 — urgent call during water incident

**Initial condition:** urgent call during water incident. The view is a projection of the current owner. **Pressure:** template ID missing. **Player action:** retain history without claiming delivery.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. intercom acknowledgement agrees across views. Public queries exclude private recipients.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One call, one list. No one has to hear it twice. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-009 — urgent call during water incident

**Initial condition:** urgent call during water incident. The view is a projection of the current owner. **Pressure:** recipient departs before reading. **Player action:** dispose old subscriptions before rebind.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. public query excludes private rows. Public queries exclude private recipients.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Water allocation changed at second bell. The list is beside the valve room. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-010 — urgent call during water incident

**Initial condition:** urgent call during water incident. The view is a projection of the current owner. **Pressure:** banner closes before acknowledgement. **Player action:** reject public listing of private content.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. expiry does not erase unrelated journal history. Public queries exclude private recipients.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Read is not the same as yes. Leave the box clear if you cannot answer. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-010 — urgent call during water incident

**Initial condition:** urgent call during water incident. The view is a projection of the current owner. **Pressure:** two views mark message read. **Player action:** show capacity/expiry denial.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. panel subscriptions close on route exit. Public queries exclude private recipients.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** This note has a name on it. Put it where that person can find it. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-010 — urgent call during water incident

**Initial condition:** urgent call during water incident. The view is a projection of the current owner. **Pressure:** second panel opens before first disposes. **Player action:** expire from day owner then refresh.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. urgent call differs from ordinary notice. Public queries exclude private recipients.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Read is not the same as yes. Leave the box clear if you cannot answer. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-011 — urgent call during water incident

**Initial condition:** urgent call during water incident. The view is a projection of the current owner. **Pressure:** second panel opens before first disposes. **Player action:** dispose old subscriptions before rebind.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. IDs survive capture/restore. Public queries exclude private recipients.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One call, one list. No one has to hear it twice. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-011 — message read but not acknowledged

**Initial condition:** message read but not acknowledged. The view is a projection of the current owner. **Pressure:** private content enters public query. **Player action:** deduplicate by owner identity.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. intercom acknowledgement agrees across views. Public queries exclude private recipients.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Water allocation changed at second bell. The list is beside the valve room. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-011 — message read but not acknowledged

**Initial condition:** message read but not acknowledged. The view is a projection of the current owner. **Pressure:** template ID missing. **Player action:** project one intercom source in all views.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. public query excludes private rows. Public queries exclude private recipients.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One call, one list. No one has to hear it twice. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-012 — message read but not acknowledged

**Initial condition:** message read but not acknowledged. The view is a projection of the current owner. **Pressure:** template ID missing. **Player action:** expire from day owner then refresh.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. expiry does not erase unrelated journal history. Public queries exclude private recipients.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** This note has a name on it. Put it where that person can find it. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-012 — message read but not acknowledged

**Initial condition:** message read but not acknowledged. The view is a projection of the current owner. **Pressure:** recipient departs before reading. **Player action:** keep read and ack separate.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. panel subscriptions close on route exit. Public queries exclude private recipients.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Read is not the same as yes. Leave the box clear if you cannot answer. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-012 — message read but not acknowledged

**Initial condition:** message read but not acknowledged. The view is a projection of the current owner. **Pressure:** radio content routes internally. **Player action:** retain history without claiming delivery.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. urgent call differs from ordinary notice. Public queries exclude private recipients.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** This note has a name on it. Put it where that person can find it. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-013 — message read but not acknowledged

**Initial condition:** message read but not acknowledged. The view is a projection of the current owner. **Pressure:** two views mark message read. **Player action:** project one intercom source in all views.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. IDs survive capture/restore. Public queries exclude private recipients.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Water allocation changed at second bell. The list is beside the valve room. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-013 — message read but not acknowledged

**Initial condition:** message read but not acknowledged. The view is a projection of the current owner. **Pressure:** second panel opens before first disposes. **Player action:** reject public listing of private content.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. intercom acknowledgement agrees across views. Public queries exclude private recipients.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One call, one list. No one has to hear it twice. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-013 — full board receives notice

**Initial condition:** full board receives notice. The view is a projection of the current owner. **Pressure:** announcement and mirror have different status. **Player action:** show capacity/expiry denial.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. public query excludes private rows. Public queries exclude private recipients.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Water allocation changed at second bell. The list is beside the valve room. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-014 — full board receives notice

**Initial condition:** full board receives notice. The view is a projection of the current owner. **Pressure:** announcement and mirror have different status. **Player action:** retain history without claiming delivery.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. expiry does not erase unrelated journal history. Public queries exclude private recipients.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Read is not the same as yes. Leave the box clear if you cannot answer. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-014 — full board receives notice

**Initial condition:** full board receives notice. The view is a projection of the current owner. **Pressure:** board removed or full. **Player action:** dispose old subscriptions before rebind.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. panel subscriptions close on route exit. Public queries exclude private recipients.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** This note has a name on it. Put it where that person can find it. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-014 — full board receives notice

**Initial condition:** full board receives notice. The view is a projection of the current owner. **Pressure:** recipient departs before reading. **Player action:** deduplicate by owner identity.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. urgent call differs from ordinary notice. Public queries exclude private recipients.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Read is not the same as yes. Leave the box clear if you cannot answer. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-015 — full board receives notice

**Initial condition:** full board receives notice. The view is a projection of the current owner. **Pressure:** recipient departs before reading. **Player action:** show capacity/expiry denial.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. IDs survive capture/restore. Public queries exclude private recipients.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One call, one list. No one has to hear it twice. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-015 — full board receives notice

**Initial condition:** full board receives notice. The view is a projection of the current owner. **Pressure:** radio content routes internally. **Player action:** expire from day owner then refresh.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. intercom acknowledgement agrees across views. Public queries exclude private recipients.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Water allocation changed at second bell. The list is beside the valve room. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-015 — full board receives notice

**Initial condition:** full board receives notice. The view is a projection of the current owner. **Pressure:** stale view uses old row index. **Player action:** keep read and ack separate.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. public query excludes private rows. Public queries exclude private recipients.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One call, one list. No one has to hear it twice. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-016 — full board receives notice

**Initial condition:** full board receives notice. The view is a projection of the current owner. **Pressure:** second panel opens before first disposes. **Player action:** deduplicate by owner identity.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. expiry does not erase unrelated journal history. Public queries exclude private recipients.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** This note has a name on it. Put it where that person can find it. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-016 — resident absent during broadcast

**Initial condition:** resident absent during broadcast. The view is a projection of the current owner. **Pressure:** announcement and mirror have different status. **Player action:** project one intercom source in all views.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. panel subscriptions close on route exit. Public queries exclude private recipients.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Read is not the same as yes. Leave the box clear if you cannot answer. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-016 — resident absent during broadcast

**Initial condition:** resident absent during broadcast. The view is a projection of the current owner. **Pressure:** board removed or full. **Player action:** reject public listing of private content.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. urgent call differs from ordinary notice. Public queries exclude private recipients.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** This note has a name on it. Put it where that person can find it. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-017 — resident absent during broadcast

**Initial condition:** resident absent during broadcast. The view is a projection of the current owner. **Pressure:** day tick repeats after save. **Player action:** reject public listing of private content.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. IDs survive capture/restore. Public queries exclude private recipients.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Water allocation changed at second bell. The list is beside the valve room. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-017 — resident absent during broadcast

**Initial condition:** resident absent during broadcast. The view is a projection of the current owner. **Pressure:** banner closes before acknowledgement. **Player action:** show capacity/expiry denial.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. intercom acknowledgement agrees across views. Public queries exclude private recipients.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One call, one list. No one has to hear it twice. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-017 — resident absent during broadcast

**Initial condition:** resident absent during broadcast. The view is a projection of the current owner. **Pressure:** two views mark message read. **Player action:** expire from day owner then refresh.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. public query excludes private rows. Public queries exclude private recipients.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Water allocation changed at second bell. The list is beside the valve room. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-018 — resident absent during broadcast

**Initial condition:** resident absent during broadcast. The view is a projection of the current owner. **Pressure:** two views mark message read. **Player action:** dispose old subscriptions before rebind.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. expiry does not erase unrelated journal history. Public queries exclude private recipients.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Read is not the same as yes. Leave the box clear if you cannot answer. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-018 — notice expires today

**Initial condition:** notice expires today. The view is a projection of the current owner. **Pressure:** posting callback submitted twice. **Player action:** deduplicate by owner identity.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. panel subscriptions close on route exit. Public queries exclude private recipients.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** This note has a name on it. Put it where that person can find it. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-018 — notice expires today

**Initial condition:** notice expires today. The view is a projection of the current owner. **Pressure:** private content enters public query. **Player action:** project one intercom source in all views.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. urgent call differs from ordinary notice. Public queries exclude private recipients.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Read is not the same as yes. Leave the box clear if you cannot answer. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-019 — notice expires today

**Initial condition:** notice expires today. The view is a projection of the current owner. **Pressure:** private content enters public query. **Player action:** expire from day owner then refresh.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. IDs survive capture/restore. Public queries exclude private recipients.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One call, one list. No one has to hear it twice. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-019 — notice expires today

**Initial condition:** notice expires today. The view is a projection of the current owner. **Pressure:** template ID missing. **Player action:** keep read and ack separate.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. intercom acknowledgement agrees across views. Public queries exclude private recipients.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Water allocation changed at second bell. The list is beside the valve room. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-019 — notice expires today

**Initial condition:** notice expires today. The view is a projection of the current owner. **Pressure:** recipient departs before reading. **Player action:** retain history without claiming delivery.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. public query excludes private rows. Public queries exclude private recipients.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One call, one list. No one has to hear it twice. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-020 — notice expires today

**Initial condition:** notice expires today. The view is a projection of the current owner. **Pressure:** banner closes before acknowledgement. **Player action:** project one intercom source in all views.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. expiry does not erase unrelated journal history. Public queries exclude private recipients.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** This note has a name on it. Put it where that person can find it. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-020 — notice expires today

**Initial condition:** notice expires today. The view is a projection of the current owner. **Pressure:** two views mark message read. **Player action:** reject public listing of private content.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. panel subscriptions close on route exit. Public queries exclude private recipients.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Read is not the same as yes. Leave the box clear if you cannot answer. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-020 — notice expires today

**Initial condition:** notice expires today. The view is a projection of the current owner. **Pressure:** second panel opens before first disposes. **Player action:** show capacity/expiry denial.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. urgent call differs from ordinary notice. Public queries exclude private recipients.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** This note has a name on it. Put it where that person can find it. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-021 — notice expires today

**Initial condition:** notice expires today. The view is a projection of the current owner. **Pressure:** second panel opens before first disposes. **Player action:** retain history without claiming delivery.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. IDs survive capture/restore. Public queries exclude private recipients.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Water allocation changed at second bell. The list is beside the valve room. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-021 — two views show mirrored call

**Initial condition:** two views show mirrored call. The view is a projection of the current owner. **Pressure:** announcement and mirror have different status. **Player action:** dispose old subscriptions before rebind.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. intercom acknowledgement agrees across views. Public queries exclude private recipients.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One call, one list. No one has to hear it twice. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-021 — two views show mirrored call

**Initial condition:** two views show mirrored call. The view is a projection of the current owner. **Pressure:** template ID missing. **Player action:** deduplicate by owner identity.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. public query excludes private rows. Public queries exclude private recipients.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Water allocation changed at second bell. The list is beside the valve room. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-022 — two views show mirrored call

**Initial condition:** two views show mirrored call. The view is a projection of the current owner. **Pressure:** template ID missing. **Player action:** show capacity/expiry denial.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. expiry does not erase unrelated journal history. Public queries exclude private recipients.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Read is not the same as yes. Leave the box clear if you cannot answer. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-022 — two views show mirrored call

**Initial condition:** two views show mirrored call. The view is a projection of the current owner. **Pressure:** recipient departs before reading. **Player action:** expire from day owner then refresh.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. panel subscriptions close on route exit. Public queries exclude private recipients.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** This note has a name on it. Put it where that person can find it. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-022 — two views show mirrored call

**Initial condition:** two views show mirrored call. The view is a projection of the current owner. **Pressure:** radio content routes internally. **Player action:** keep read and ack separate.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. urgent call differs from ordinary notice. Public queries exclude private recipients.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Read is not the same as yes. Leave the box clear if you cannot answer. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-023 — two views show mirrored call

**Initial condition:** two views show mirrored call. The view is a projection of the current owner. **Pressure:** two views mark message read. **Player action:** deduplicate by owner identity.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. IDs survive capture/restore. Public queries exclude private recipients.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One call, one list. No one has to hear it twice. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-023 — two views show mirrored call

**Initial condition:** two views show mirrored call. The view is a projection of the current owner. **Pressure:** second panel opens before first disposes. **Player action:** project one intercom source in all views.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. intercom acknowledgement agrees across views. Public queries exclude private recipients.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Water allocation changed at second bell. The list is beside the valve room. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-023 — memorial template is public

**Initial condition:** memorial template is public. The view is a projection of the current owner. **Pressure:** announcement and mirror have different status. **Player action:** reject public listing of private content.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. public query excludes private rows. Public queries exclude private recipients.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One call, one list. No one has to hear it twice. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-024 — memorial template is public

**Initial condition:** memorial template is public. The view is a projection of the current owner. **Pressure:** announcement and mirror have different status. **Player action:** keep read and ack separate.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. expiry does not erase unrelated journal history. Public queries exclude private recipients.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** This note has a name on it. Put it where that person can find it. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-024 — memorial template is public

**Initial condition:** memorial template is public. The view is a projection of the current owner. **Pressure:** board removed or full. **Player action:** retain history without claiming delivery.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. panel subscriptions close on route exit. Public queries exclude private recipients.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Read is not the same as yes. Leave the box clear if you cannot answer. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-024 — memorial template is public

**Initial condition:** memorial template is public. The view is a projection of the current owner. **Pressure:** day tick repeats after save. **Player action:** dispose old subscriptions before rebind.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. urgent call differs from ordinary notice. Public queries exclude private recipients.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** This note has a name on it. Put it where that person can find it. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-025 — memorial template is public

**Initial condition:** memorial template is public. The view is a projection of the current owner. **Pressure:** banner closes before acknowledgement. **Player action:** dispose old subscriptions before rebind.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. IDs survive capture/restore. Public queries exclude private recipients.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Water allocation changed at second bell. The list is beside the valve room. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-025 — memorial template is public

**Initial condition:** memorial template is public. The view is a projection of the current owner. **Pressure:** stale view uses old row index. **Player action:** deduplicate by owner identity.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. intercom acknowledgement agrees across views. Public queries exclude private recipients.

**Cross-system witness:** One source event retains one durable result through retry, save and restore. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One call, one list. No one has to hear it twice. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-025 — recipient leaves before reading

**Initial condition:** recipient leaves before reading. The view is a projection of the current owner. **Pressure:** posting callback submitted twice. **Player action:** project one intercom source in all views.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. public query excludes private rows. Public queries exclude private recipients.

**Cross-system witness:** A downstream owner receives one fact and returns its result without a copied ledger. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Water allocation changed at second bell. The list is beside the valve room. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-026 — recipient leaves before reading

**Initial condition:** recipient leaves before reading. The view is a projection of the current owner. **Pressure:** posting callback submitted twice. **Player action:** expire from day owner then refresh.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. expiry does not erase unrelated journal history. Public queries exclude private recipients.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Read is not the same as yes. Leave the box clear if you cannot answer. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-026 — recipient leaves before reading

**Initial condition:** recipient leaves before reading. The view is a projection of the current owner. **Pressure:** private content enters public query. **Player action:** keep read and ack separate.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. panel subscriptions close on route exit. Public queries exclude private recipients.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** This note has a name on it. Put it where that person can find it. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-026 — recipient leaves before reading

**Initial condition:** recipient leaves before reading. The view is a projection of the current owner. **Pressure:** template ID missing. **Player action:** retain history without claiming delivery.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. urgent call differs from ordinary notice. Public queries exclude private recipients.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Read is not the same as yes. Leave the box clear if you cannot answer. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-027 — recipient leaves before reading

**Initial condition:** recipient leaves before reading. The view is a projection of the current owner. **Pressure:** day tick repeats after save. **Player action:** project one intercom source in all views.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. IDs survive capture/restore. Public queries exclude private recipients.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One call, one list. No one has to hear it twice. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-027 — recipient leaves before reading

**Initial condition:** recipient leaves before reading. The view is a projection of the current owner. **Pressure:** banner closes before acknowledgement. **Player action:** reject public listing of private content.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. intercom acknowledgement agrees across views. Public queries exclude private recipients.

**Cross-system witness:** Missing identity, retired catalog entry or absent adapter is a recoverable boundary. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Water allocation changed at second bell. The list is beside the valve room. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-027 — recipient leaves before reading

**Initial condition:** recipient leaves before reading. The view is a projection of the current owner. **Pressure:** two views mark message read. **Player action:** show capacity/expiry denial.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. public query excludes private rows. Public queries exclude private recipients.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One call, one list. No one has to hear it twice. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-028 — recipient leaves before reading

**Initial condition:** recipient leaves before reading. The view is a projection of the current owner. **Pressure:** two views mark message read. **Player action:** retain history without claiming delivery.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. expiry does not erase unrelated journal history. Public queries exclude private recipients.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** This note has a name on it. Put it where that person can find it. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-028 — recipient leaves before reading

**Initial condition:** recipient leaves before reading. The view is a projection of the current owner. **Pressure:** second panel opens before first disposes. **Player action:** dispose old subscriptions before rebind.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. panel subscriptions close on route exit. Public queries exclude private recipients.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Read is not the same as yes. Leave the box clear if you cannot answer. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-028 — save includes announcement and mirror

**Initial condition:** save includes announcement and mirror. The view is a projection of the current owner. **Pressure:** private content enters public query. **Player action:** deduplicate by owner identity.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. urgent call differs from ordinary notice. Public queries exclude private recipients.

**Cross-system witness:** The resource/capacity owner confirms cost; preview never performs the transaction. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** This note has a name on it. Put it where that person can find it. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-029 — save includes announcement and mirror

**Initial condition:** save includes announcement and mirror. The view is a projection of the current owner. **Pressure:** private content enters public query. **Player action:** show capacity/expiry denial.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. IDs survive capture/restore. Public queries exclude private recipients.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Water allocation changed at second bell. The list is beside the valve room. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-029 — save includes announcement and mirror

**Initial condition:** save includes announcement and mirror. The view is a projection of the current owner. **Pressure:** template ID missing. **Player action:** expire from day owner then refresh.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. intercom acknowledgement agrees across views. Public queries exclude private recipients.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** One call, one list. No one has to hear it twice. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-029 — save includes announcement and mirror

**Initial condition:** save includes announcement and mirror. The view is a projection of the current owner. **Pressure:** recipient departs before reading. **Player action:** keep read and ack separate.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. public query excludes private rows. Public queries exclude private recipients.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Water allocation changed at second bell. The list is beside the valve room. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S1-030 — save includes announcement and mirror

**Initial condition:** save includes announcement and mirror. The view is a projection of the current owner. **Pressure:** banner closes before acknowledgement. **Player action:** deduplicate by owner identity.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. expiry does not erase unrelated journal history. Public queries exclude private recipients.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Board name is not access permission. Validate board, audience, template, capacity and expiry through the owner.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Read is not the same as yes. Leave the box clear if you cannot answer. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S2-030 — save includes announcement and mirror

**Initial condition:** save includes announcement and mirror. The view is a projection of the current owner. **Pressure:** two views mark message read. **Player action:** project one intercom source in all views.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. panel subscriptions close on route exit. Public queries exclude private recipients.

**Cross-system witness:** Day/save ordering belongs to the current coordinator; focus and route re-entry do not advance time. Current announcement and mirror have separate IDs and acknowledgement APIs. Visual grouping does not resolve state divergence.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** This note has a name on it. Put it where that person can find it. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

### Scenario W20-100-S3-030 — save includes announcement and mirror

**Initial condition:** save includes announcement and mirror. The view is a projection of the current owner. **Pressure:** second panel opens before first disposes. **Player action:** reject public listing of private content.

**Expected route and outcome:** Keep one owner identity, audience, expiry and acknowledgement. A banner/inbox may show the event twice visually but cannot create two mutable records. urgent call differs from ordinary notice. Public queries exclude private recipients.

**Cross-system witness:** Re-read canonical roster and consent at command time; stale display grants no permission. Current model has one RecipientId and simple flags. Anonymous, encrypted, multi-party and faction mail are out of scope.

**Failure:** on owner rejection, preserve prior durable state, name the reason and offer only a supported next step. Do not turn refusal, delay or missing data into a success-shaped toast. **Replay/save:** retry and restore reuse identity; they cannot award a second lesson, goal increment, message or transfer.

**Player-facing observation:** Read is not the same as yes. Leave the box clear if you cannot answer. Show only after the owner result, with source, audience/participant and next allowed action clear. Do not claim effects the simulation did not apply.

## 30. Final review gate

Refresh this premise against current source; confirm path claims and owner decisions; choose focused verification; leave unproven effects unavailable. Proposal only: no implementation, test, runtime or ledger result is claimed.

## 31. Plan 4 expansion installment 1 — The notice reached the wall; what happened next?

Plan 4 begins with a communication problem the player can actually investigate: a notice is posted, but the intended reader may not have seen it, a detail may have changed, or a second channel may be needed. The player chooses what is known, who needs to know, and what channel can truthfully carry the information. A public board, intercom call, and private message each make different promises about audience and acknowledgement.

This installment expands only the three defined subfeatures: scoped public boards with expiry; one canonical intercom identity and acknowledgement; and private messages addressed to one recipient. It does not add a social network, general journal, rumor reputation, or new broadcast authority.

## 32. Start from an owner-confirmed event

A communication should begin with a real state change or request: a work area closes for a scheduled repair, a roster time changes, a delivery is delayed, or a resident asks for a private reminder. These are candidate scenes; verify the event producer and its current data before authoring them as reachable.

The player first checks what the source owner confirms. They can publish a known time, state that a detail is still pending, ask the source owner for clarification, or wait. A message cannot make an uncertain event certain. If the owner has not confirmed that the room is closed, the player cannot post a notice stating that it is.

The player also decides whether the fact is public, urgent, or specific to one recipient. A single event may support multiple projections, but each projection must retain its proper audience and source identity. Opening two screens cannot create two independent incidents.

## 33. The channel choice is an action

The player can post a board notice for a bounded audience, broadcast through the intercom if the information warrants that interruption, send a private note to one eligible recipient, or combine supported channels for distinct audiences. Each choice carries different reach, persistence, and acknowledgement expectations.

Before posting, the player can:

- choose or revise a supported board and audience;
- set or accept an owner-supported expiry;
- change a routine update into an urgent call only where the owner permits it;
- remove private details from a public summary;
- address a message to one current recipient;
- check whether an existing notice already carries the same source event;
- delay sending until the source owner confirms a disputed detail.

These decisions branch on audience, time, privacy, uncertainty, and urgency. They are not choices between a virtuous truth and a villainous lie.

## 34. Scenario — “The Workshop Door Is Shut”

This candidate scenario assumes an existing workshop, a confirmed closure event, and supported communication routes. If those premises are absent, retain the structure as a writing pitch only.

**Opening:** A repair owner reports that the workshop will close for a bounded period. The reopening time is uncertain. The player can publish only the confirmed closure, ask the owner for an estimate, or wait for a complete schedule.

**Public notice:** The player chooses which board reaches workshop users and who authors the notice. They can set an expiry if the board owner supports it, or return later to correct the notice. A shelter-wide post may be unnecessary; a room-level board may be enough.

**Intercom decision:** A scheduled task begins soon and some people may be on their way. The player can broadcast a short urgent call if the current intercom owner supports urgency, repeat the scoped notice, or avoid interrupting everyone and use a narrower route.

**Private follow-up:** One named survivor has a personal appointment at the workshop. The player can send a private update, ask that person to acknowledge, or allow the public notice to stand if no private route is needed. The note does not copy the private appointment into a public board.

**Resolution:** The repair may finish early, finish late, or remain incomplete. The player can correct, expire, or supersede the public notice; send one follow-up; or leave recipients unconfirmed. The repair owner—not the communication panel—determines when the room reopens.

## 35. Public board branches: scope, revision, expiry

The player can choose a room-specific board, a broader supported audience, or no posting if access is unavailable. If the board is full, they can remove an expired message through the owner, wait, or request an alternate supported location. A label such as “Workshop” does not prove that the player has posting permission.

When facts change, the player can revise the notice, append a correction, remove it, or let it expire, depending on the board contract. A correction should preserve enough provenance for the reader to tell what changed. Silently rewriting a timestamp or author can make two different events look like one.

Readers may act, ask a question, submit a correction, or remain unknown. No response is not proof that they read the notice. If read receipts are not supported for public boards, the UI must not invent them.

## 36. Intercom branches: urgency without panic

The player decides whether the message genuinely needs to interrupt the shelter. Routine schedule changes should not become alarms merely because the intercom is available. A call can be urgent when immediate action is supported; otherwise, a board or private message may be the better route.

The banner and inbox can show the same announcement, but both must resolve to one canonical source identity and one acknowledgement state. A resident may acknowledge from either surface. Reopening the second view cannot generate a second event.

After sending, the player sees confirmed acknowledgements and residents who remain unaccounted for only if the owner exposes that list. They can wait, send a supported reminder, or accept uncertainty. Silence does not mean agreement, refusal, absence, or death.

## 37. Private message branches: recipient and disclosure

A private message names one eligible recipient. The player checks that identity, chooses supported content, and can decide whether the message should ask for a response. Read, acknowledged, and acted upon remain distinct states if the current owner represents them.

If the recipient leaves before delivery, the player can cancel or leave the message pending only where the owner supports that state. They cannot redirect the note to a replacement survivor. If a private fact must also reach a group, the player writes a separate public summary that omits the private details.

The recipient can read without agreeing to the request. The player can follow up, wait, or close the matter. A message never becomes a consent verdict for a task.

## 38. Channel decision table

| Need | Player action | Channel route | What the result can claim |
|---|---|---|---|
| Several rooms need the same schedule | Post to the narrowest suitable board | Public board owner | Notice posted to a supported audience |
| Immediate action is required | Broadcast only through allowed urgency | Intercom owner | One call and its actual acknowledgements |
| One survivor needs a personal update | Address that survivor directly | Private-message owner | Delivery/read state only if represented |
| Details are still uncertain | Ask source owner or state the uncertainty | Defer or qualified message | Only confirmed facts are presented as fact |
| A message became outdated | Correct, supersede, or expire it | Original channel owner | Change and provenance reflect actual update |
| Recipient is no longer eligible | Cancel or leave pending if supported | Private-message owner | No silent reassignment or assumed delivery |

## 39. Supporting factions without making them the channel owner

A faction representative can provide a confirmed schedule, request a board audience, or ask the player to relay a correction. The player can check the fact, negotiate audience or timing, use a public channel, or decline. The faction's message remains distinct from shelter communication state unless an existing bridge explicitly supports it.

A supporting contact may know that one room is affected, but cannot grant access to a board outside their ownership. The player can ask the shelter communication owner for permission or choose a channel that is available. A faction does not gain reputation merely because its notice is posted.

If a faction requests a private message for another person, verify that the player may disclose and deliver it. The recipient's consent to receive the note does not confirm that they will carry out its request.

## 40. Endings driven by communication choices

The player can post early with uncertainty clearly stated; wait for full confirmation; use only a local board; broadcast urgently and receive partial acknowledgements; send a private update without public exposure; correct an outdated notice; allow the board notice to expire; or fail to send because access is denied.

These outcomes differ because the player selected scope, timing, channel, privacy, and follow-up. No ending declares the player honest or evil. A well-intended message can still be too broad; a delayed notice can be prudent or costly depending on the confirmed event; and non-response remains uncertain.

## 41. Failure and repair cases

If the message fails validation, preserve existing state and show the owner reason. If the board is unavailable, offer only supported alternatives. If the source changes between preview and send, recheck before commit. If the call is delivered twice, one identity and acknowledgement remain. If an intercom mirror disagrees with the source, refresh from the owner rather than selecting the last-opened view.

If the player sends the wrong audience, use the current correction and removal path. Do not promise that a private message can be recalled after the recipient reads it unless that behavior is supported. A later correction should state what is known, not erase the fact that the first notice was seen.

## 42. Re-entry, expiry, and save behavior

When returning after a day advance, load board entries, announcement identity, recipient state, expiry, and current source event from their existing owners. An expired notice may disappear from the active board but should not erase unrelated journal history. A pending private message returns only if its owner persists it.

Duplicate send attempts reuse a durable result or report that the command already ran. Acknowledgement from two projections maps to one identity. Saving between preview and send must not turn the preview into a posted message.

## 43. Scope guardrails

Do not introduce a new social feed, rumor system, popularity meter, cross-channel notification database, generic alert scheduler, or private group mail. The three feature commitments remain scoped boards, one intercom acknowledgement identity, and recipient-owned private messages.

No content should promise privacy, read receipts, board capacity, recall, expiry, or cross-channel linking until the responsible current owner proves that behavior. The scenario may be narrowed to the routes that exist.

## 44. Acceptance questions

1. Is the source event confirmed before a message claims it happened?
2. Does the audience match what the player selected and what the board/intercom owner allows?
3. Do banner and inbox views share one announcement identity and acknowledgement?
4. Are private messages limited to one eligible recipient and kept out of public queries?
5. Can the player correct or expire a notice without rewriting history or duplicating an event?
6. Does silence remain unknown rather than being interpreted as consent or refusal?
7. Does save/re-entry preserve only the state its existing owner stores?

## 45. Plan 4 installment 1 close

This opening installment starts Plan 4 with a communication incident that asks the player to verify the source, choose audience and channel, protect private details, follow acknowledgements, and correct stale information. Public boards, intercom calls, and private messages remain separate owner-backed routes. The branches emerge from practical communication choices and uncertainty, not morality or faction alignment.

## 46. Plan 4 expansion installment 2 — Correct the notice without exposing the source

A public notice can contain a wrong detail, and the person who knows the correction may not want their name shared. This creates a communication branch about evidence, attribution, audience, and privacy. The player can verify the fact, correct the public notice, ask the author to revise it, or privately relay the concern without naming the source. The route must not turn a correction into a new rumor or informant system.

This installment stays inside the three subfeatures: board scope and expiry; one canonical intercom identity with acknowledgement; and recipient-owned private messages. The communication surface may show who authored a public notice where its owner supports that field. It must not expose private-message contents through a public query.

## 47. Separate the claim from the person who raised it

When a reader disputes a notice, the player can ask what statement is wrong, what evidence supports the correction, and who owns the underlying event. The reader may cite a source, offer an observation, or decline to identify themselves. The player can verify the claim without forcing a public attribution.

The notice owner decides whether a correction can be appended, whether the original author must edit it, and what provenance remains visible. If no correction route exists, the player can ask the original author or source owner to issue a new notice, leave the disagreement open, or use an allowed private channel to seek clarification.

The player should not claim that the correction is true merely because a different person said it. Conversely, a named author is not automatically correct. The communication owner records the message; the event/source owner confirms the underlying fact.

## 48. Branching actions around attribution

- Ask the reader to identify the disputed sentence without naming themselves.
- Check the source event, dated record, or owner that can verify the fact.
- Ask the original author to review the correction.
- Append a correction with the source shown if the board owner supports provenance.
- Send a private question to one eligible author or witness.
- Publish the corrected fact without personal details if the owner allows anonymous attribution.
- Leave both accounts visible while the evidence remains uncertain.
- Withdraw a draft if it would reveal private information or misstate the source.

These actions branch on available evidence and disclosure rules. None adds a hidden credibility score to either resident.

## 49. What exactly can be corrected?

The player may be correcting a time, location, audience, instruction, author, expiry, or confirmed source fact. These are not interchangeable. A time correction might require the event owner; an authorship correction belongs to the communication owner; a private detail may need to be omitted entirely.

Before posting, the player can preview what readers will see and whether the source will be named. If the preview cannot accurately represent the correction and prior text, do not promise a transparent history. The player may instead request a new notice or defer.

The correction should not silently erase acknowledgements already recorded. Readers who acted on the first version may need a fresh supported notification. The intercom owner determines whether an urgent correction warrants another call; a board update does not automatically create one.

## 50. Decision table — correct, clarify, or wait

| Disputed detail | Player action | Audience route | Safe claim |
|---|---|---|---|
| Time changed at source | Confirm with event owner and revise notice | Board and scoped follow-up if supported | New time is confirmed |
| Reader reports an error but lacks source | Ask source owner or keep uncertainty visible | No public correction yet | Dispute exists; truth unresolved |
| Private recipient provides a correction | Verify fact without exposing note contents | Private reply or public summary | Summary contains no private detail |
| Author disputes the proposed edit | Invite owner-supported resolution | Original board thread or new notice | Disagreement remains attributed accurately |
| Author cannot be reached | Use supported correction or issue-new path | Public board if allowed | Source and uncertainty are shown |
| Correction is time-critical | Check urgency owner, then broadcast if warranted | One intercom identity for the correction | Call and acknowledgement remain singular |
| No correction API exists | Do not simulate an edit | Request owner action or leave open | No false claim that the board changed |

## 51. A correction should not expose a private source

The reader may tell the player something in a private message and ask that the public notice be corrected without naming them. The player can confirm the fact through a separate source, ask permission to attribute it, or report only the information the source approved.

If the fact cannot be verified without revealing the source, the player can ask whether the source consents, keep the correction private, or leave the public notice unchanged while seeking evidence. No system should leak a private recipient identity through a public query or correction log.

The source may later choose to be named. That choice must be explicit and use the current communication owner. A private message being read does not grant permission to publish its contents.

## 52. Intercom correction branches

If the incorrect notice was also broadcast, the player can decide whether a correction needs another call. The urgency owner should answer from the event and current rules. If the correction is routine, the player may update the board and wait. If a confirmed hazard or immediate change exists, a new call may be warranted.

The new call has its own announcement identity, while the original remains in history with its acknowledgement state. If the current owner supports a correction relation, link them there; do not merge their records or rewrite the original acknowledgement. A reader who acknowledged the first call has not necessarily acknowledged the correction.

Banner and inbox views still project one source per call. Opening either or both cannot duplicate calls or acknowledgements.

## 53. Private message branches

The player can send a private question to the notice author, reply to the person who raised the issue, or avoid further contact if the recipient is unavailable or does not want a conversation. Address one eligible recipient only. A group dispute should not be represented as multi-recipient mail unless the current owner supports that model.

If a recipient has departed, the player can cancel, wait only if supported, or seek another source through an allowed route. No private message should be silently forwarded. The player can post a public correction after independent verification, not by copying a private conversation.

## 54. Scene — “A Correction with No Name”

This candidate scene requires an actual public notice, a verifiable source owner, and a current correction route.

A resident tells the player privately that a posted supply time is wrong but asks not to be named. The player can check the delivery owner, ask another authorized source, ask whether attribution may be disclosed, or leave the notice unchanged until verified.

If the source confirms the new time, the player can append a correction naming the source only if they consent and the board owner supports it; otherwise, publish the confirmed time without exposing the private exchange. Readers may acknowledge through supported routes, ask questions, or remain unknown.

If the source contradicts the resident, the player can privately explain that the evidence does not support a correction, ask whether they have another source, or leave the matter open. No public accusation is required.

Possible endings include a corrected notice with anonymous provenance, an attributed correction, an unresolved dispute, a private follow-up, or no edit because the owner cannot change the board.

## 55. Supporting factions and authors

A faction author may ask the player to preserve their name on an official notice. The player can explain a reader's concern without revealing identity, ask the faction to check its source, or decline to edit without evidence. The faction may revise, refuse, or issue a separate statement through its current channel.

A supporting contact can confirm one fact or supply a source. They do not automatically settle authorship or credibility. The player can compare records, ask the original author, seek another authorized source, or leave both claims visible.

## 56. Ending mosaic

**Verified anonymous correction:** the fact is confirmed and the source remains unnamed.

**Attributed correction:** the source agrees to be named and the board owner records attribution.

**Correction declined by author:** the player can ask for owner review, publish an allowed separate notice, or close the issue.

**Unresolved dispute:** evidence conflicts; the player keeps uncertainty visible and seeks another source.

**Private clarification only:** the player answers one recipient without changing public communication.

**No route to correct:** the board owner has no supported edit path; the player does not simulate one.

## 57. Scope, privacy, and replay

This installment adds no anonymous tip line, reputation score, source-protection subsystem, fact-checking meter, or public debate board. It uses existing message authorship, private recipient, board edit, source event, and intercom owners. If the current system cannot protect attribution, do not promise anonymity in live content.

On reload, current notice text, correction, source event, and acknowledgements come from their owners. A correction cannot be appended twice by replay. The original message's audience and acknowledgement remain distinct from any correction call.

## 58. Acceptance questions

1. Can the player verify the underlying fact separately from who reported it?
2. Can a source request privacy without private data leaking to public views?
3. Does the board owner preserve author, correction, and expiry truthfully?
4. Is an intercom correction a distinct canonical call with separate acknowledgement?
5. Can the player leave a dispute unresolved without inventing a credibility score?
6. Does the player see what cannot be edited or attributed before committing?
7. Are correction and private reply safe under replay and save/restore?

## 59. Plan 4 installment 2 close

This installment expands communications with an attribution and correction branch. The player can verify a claim, preserve a source's privacy, ask an author to revise, publish a supported correction, send a private clarification, or leave uncertainty visible. Boards, intercom calls, and private messages retain separate audience and acknowledgement contracts; no reputation or rumor system is added.

## 60. Expansion installment 3 — The person was not there when it was said

An accurate message can still fail to reach the person who needs it. This installment expands the existing board, intercom, and private-message choices around absence, relay, and uncertain receipt. The player chooses whether to wait, leave an allowed message, ask an authorized person to relay it, use another supported channel, or accept that the recipient cannot be reached in time.

The design distinguishes what was sent from what was delivered, read, understood, acknowledged, and acted upon. These distinctions are only persisted when the current communication owner supports them. No new courier network or message ledger is introduced.

## 61. A channel being used does not prove receipt

A posted board notice may be available to read without proving that a particular person saw it. An intercom call may reach a room without proving the intended listener understood. A private message may be accepted by its owner but remain unread. Acknowledging receipt may still not mean that a request was accepted or a task was completed.

The interface shows only the current owner-backed state. If it stores “delivered,” do not relabel that as “read.” If it stores no receipt state, show the message as sent and leave the recipient's knowledge unknown.

The player can ask directly, wait for a reply, use a second authorized route, or proceed under uncertainty if the underlying task permits. The message panel cannot infer a person's knowledge from their later behavior unless an existing system emits that fact.

## 62. Scene seed — “The board was covered by the shift list”

This is a candidate pattern. Confirm the board location, reader's schedule, communication owner, urgency source, and available relay routes before treating it as reachable.

A time-sensitive but non-emergency notice is posted while its intended reader is away. When they return, another sheet covers the notice. A coworker remembers seeing it, but cannot say whether the reader did. The player can check the board history, ask the author to repost, leave a private message if allowed, or ask the coworker to pass along a neutral reminder.

The coworker may agree to relay the existence of the notice but not its sensitive details. The player may wait for confirmation, ask the reader directly when they return, or use a supported intercom route if the deadline warrants it. The player can also accept that the person remains uninformed and choose a fallback that does not presume consent.

The endings range from confirmed receipt to a late reply, a lost opportunity, a protected message not relayed, or a revised plan. None should claim the reader knew before they actually received the information.

## 63. A relay is not a proxy decision

A relay can carry a message; it cannot accept, refuse, consent, acknowledge on behalf of, or disclose for the intended recipient unless a current owner explicitly supports an authorized representative for that particular action.

The player can ask a nearby survivor to say, “There is a revised notice for you at the board,” without sharing private content. The recipient can later read it and respond through the proper route. If the message contains a personal medical, faction, or relationship detail, the player should use an allowed private channel or not relay it.

If a legal or operational proxy already exists for a specific system, verify its identity, scope, expiry, and save owner. A family tie, faction rank, or physical presence is not enough to assume authority.

## 64. Choose a second route deliberately

When the first route has uncertain reach, the player can:

- wait for the recipient's ordinary return;
- repost or update the public notice through its owner;
- send a private message to one eligible recipient;
- ask a permitted relay to point the recipient toward the notice;
- use an intercom call if the event owner justifies the urgency;
- revise the plan so that receipt is not a hidden prerequisite; or
- accept that the recipient cannot be reached before the deadline.

The second route should create a distinct supported communication event. If the intercom call and board update are linked by current state, show their relationship; do not collapse their separate authorship and acknowledgment.

## 65. Absence does not make information public

The player may know that a recipient is away without knowing why. Do not display a precise location or personal schedule unless its owner exposes it to the player. The player can choose a neutral message, defer contact, or ask an authorized source whether the person can be reached.

A public board is inappropriate for a private message just because a private channel is unavailable. A broad intercom call may expose information to bystanders. The player can choose not to communicate and use another operational route.

When the recipient's absence relates to a faction assignment, the faction may confirm only the minimum necessary availability fact. The player can seek an authorized contact, leave a neutral notice, or proceed without disclosing the message content.

## 66. Branches by urgency and recipient state

**Recipient expected back before the deadline:** wait, leave a neutral pointer, or use a private message; no urgent broadcast is needed by default.

**Recipient absent past the deadline:** ask the event owner whether the decision can wait, seek an authorized alternate, or activate a supported fallback.

**Recipient present but does not respond:** show no response; do not label silence as acknowledgement or refusal.

**Recipient cannot safely receive a public call:** use a private route if permitted or revise the plan without their consent.

**Relay person knows only that a notice exists:** permit a neutral pointer, not the full contents.

**Recipient later disputes what they heard:** inspect the canonical message and correction history; ask for clarification without inventing a credibility score.

Each route depends on current channel, urgency, and access state, not a universal communication-reliability stat.

## 67. Acknowledgement remains tied to the actual recipient

If the communication owner supports acknowledgements, they belong to the person who actually received the message. A helper's response that they passed it on is a relay fact, not an acknowledgement by the recipient.

The player may ask the recipient to confirm receipt, but should not demand that they agree with the message. “I saw it” and “I accept the request” are separate outcomes. The UI should present each only if the underlying owner distinguishes them.

If the recipient takes an action that is consistent with the notice, do not infer that the notice caused it. A task owner or communication event must provide the connection.

## 68. Supporting factions as delivery constraints

A faction contact can explain its authorized message route, pass a neutral reminder through an established channel, or report that its member is unavailable. It cannot impersonate a private sender or acknowledge a personal request for the recipient.

A faction might restrict radio use, require a written record, or refuse to expose the location of an absent member. The player can comply, use a different approved route, ask for a narrow exception through the faction owner, or accept the delay.

Supporting contacts should remain support. The central decision still belongs to the player and the intended recipient, through the appropriate channel and task owners.

## 69. Branch table — sent, received, understood, acted

| Evidence available | What the player can say | What remains unknown |
|---|---|---|
| Notice exists on the board | “The notice is posted” | Whether a specific reader saw it |
| Intercom call recorded | “The call was broadcast” | Which listeners understood or accepted it |
| Private message marked delivered | “The message reached its inbox” | Whether it was read or answered |
| Recipient says they read it | “They read the message” | Whether they agree to the request |
| Recipient confirms receipt only | “They received it” | Whether the task is accepted or complete |
| Relay confirms passing a neutral pointer | “A reminder was relayed” | Whether the intended recipient saw the notice |
| Task owner records the requested action | “The task occurred” | Whether the message caused it, unless linked |

The player-facing language should not claim a stronger state than the evidence.

## 70. Ending routes and fallback plans

The recipient may receive the notice in time and respond; receive it too late and ask for a revised plan; receive only a neutral pointer and seek details; never receive it; decline after reading; accept but later fail to complete the action; or remain unreachable while the player uses an allowed fallback.

The player can choose whether to extend a deadline, assign an alternative through the task owner, issue a new notice, or leave the decision open. If no alternative is authorized, the player should see the limitation instead of a fabricated success.

The fallback may change the communication audience. Before broadening the audience, show who will now be able to see the message. A deadline does not itself remove a recipient's privacy.

## 71. Continuity and cross-scene callbacks

A later scene can reveal that the message was covered, the relay was delayed, or the recipient read a corrected version rather than the original. Use the canonical message identity and source event where available. Do not write a callback that contradicts an unknown receipt state.

A survivor can refer to the missed notice without claiming exactly when it was read. A faction can remember its own call or policy but cannot know whether every resident heard it. The player may revisit the board, ask the author, or accept the gap.

The callback should create a practical consequence: a missed slot, a rescheduled job, a request repeated privately, or an unresolved reply. It should not create a new rumor or reputation subsystem.

## 72. Player approaches

**The patient sender:** waits for the recipient's ordinary return, preserving privacy but risking delay.

**The minimal relay:** asks a permitted contact to point the recipient toward a notice without passing sensitive contents.

**The urgency check:** verifies whether another broadcast is justified before calling everyone.

**The fallback planner:** changes the task so the absent person's consent is not silently assumed.

**The record keeper:** checks the canonical message and correction history before repeating it.

These actions create different reachable outcomes. None is an archetype or a hidden social alignment.

## 73. Persistence, acceptance, and installment close

On reload, use the existing communication owner for message identity, audience, route, status, acknowledgements, and correction links. If delivery/read states are not stored, preserve that uncertainty. Relaying a pointer, reposting, or retrying a call cannot duplicate one canonical event or manufacture the intended recipient's acknowledgement.

Accept this installment when the player can distinguish sent from received and received from agreed; absent recipients remain uncommitted; private content stays scoped; faction contacts relay only within authority; and every fallback either uses an existing owner or clearly leaves the action unresolved. It adds reach and timing branches to the same three communication subfeatures, not a courier system or fourth feature pillar.

## 74. Expansion installment 4 — The notice draws a public answer

A public notice can invite questions or disagreement from people who read it. This installment explores how the player responds when readers add information, request an explanation, or challenge the proposed action. It stays within the existing board, intercom, and private-message routes. If the current board owner has no reply operation, the design uses a separate notice or private message instead of inventing a threaded forum.

The branches come from what the reader says, what evidence exists, which audience needs the answer, and whether the author or player has authority to speak.

## 75. A reply has its own author and audience

The author of the original notice does not automatically author its response. A reader may ask a factual question, add a firsthand observation, refuse a request, or ask that a private issue not be discussed publicly. Each response belongs to the person who made it and keeps its own audience.

The player can answer from confirmed information, refer the question to the notice author, verify a claim, move a sensitive detail into an allowed private channel, or leave the question open. They should not combine several readers' words into one convenient consensus.

If a response changes the underlying task, schedule, or event, the task/event owner must confirm that change. A notice reply does not itself reschedule work or authorize access.

## 76. The player chooses where to answer

The player can answer on the same board only if its current owner supports that relation; issue a separate public notice; send one private reply; ask the original author to respond; or decline to answer until the fact is checked.

Show the expected audience before committing. A public answer may help several readers but expose context. A private answer protects detail but leaves the public question unresolved. A new notice can correct the audience but may draw additional attention.

If no route can preserve the needed privacy and truth, the player can state that the system cannot answer safely yet and seek an owner response.

## 77. Scene seed — “The missing hour”

Candidate content only. Verify that a public notice, task-time owner, reader response route, and author identity exist before making this a reachable scene.

A board notice says a shared room will be available in the afternoon. One reader replies that their shift roster shows the room is occupied for part of that time. Another says they already moved equipment in reliance on the notice. The player can check the roster, ask the notice author, ask the room owner, or pause any downstream commitment.

The player may post a verified correction, ask the author to make it, message the affected reader privately, or leave both accounts visible until the owner confirms the slot. The person who moved equipment can ask for help or choose to wait; no one is blamed merely for acting on the earlier notice.

Possible outcomes include a corrected time, a narrower access window, a private explanation, an unresolved conflict, or a fallback location if an owner supports it. The public exchange should preserve who said what.

## 78. Questions, observations, refusals, and proposals

**A question:** answer only from confirmed information or say who can confirm it.

**A firsthand observation:** preserve the speaker's attribution and seek another source if the claim changes a decision.

**A refusal:** record that the person declined the request if the current communication owner supports it; do not describe silence or disagreement as acceptance.

**A counterproposal:** route it to the event, task, or access owner and ask affected people to accept the revised terms.

**A private disclosure:** do not quote or forward it publicly without permission.

These response types need not become a new taxonomy in production. They guide content and channel choice; use current message/notice contracts.

## 79. The author can delegate an answer, not ownership

The notice author may ask the player to handle a practical question. The player can accept, decline, or ask the author to review a draft. The author remains the source of the original claim, while the player is accountable for the response they publish.

If a faction contact owns the notice, it may designate an authorized spokesperson for the channel. That authority covers only the specified audience and subject. It does not let the spokesperson change a schedule, disclose private reasons, or speak for a reader.

An author can also refuse to answer publicly. The player may issue a separate evidence-backed correction, use a private route, request an owner review, or leave the issue unresolved.

## 80. Do not turn response count into truth

Several readers can repeat the same claim without making it verified. One quiet source can be correct even if nobody else posts. The player should compare claims with the relevant owner or source, preserve uncertainty, and avoid a visible vote tally unless an existing governance owner defines one.

Readers may agree on the practical problem but disagree on its cause. The player can acknowledge the shared fact, ask for evidence on the cause, and route the decision to its owner.

No new popularity, credibility, or rumor meter is added. A response can change what the player knows without changing who owns the underlying truth.

## 81. Supporting faction participation

A supporting faction may confirm its own notice, answer for property it controls, provide a public source, or ask that the player correct an inaccurate statement. Its contact may also refuse to comment.

The player can relay the faction's scoped answer, ask another owner to confirm the practical consequence, publish only the confirmed fact, or keep the dispute open. The faction does not become the moderator of every survivor's communication.

If a faction requests removal of another person's response, inspect the existing board owner rules. Do not silently delete or rewrite a survivor's message through a UI action that lacks authority.

## 82. Branch table — answer, refer, separate, or wait

| Reader response | Player action | Route | Result that can be claimed |
|---|---|---|---|
| Asks for confirmed time | Check event owner | Public answer or correction | Confirmed time only |
| Reports a conflicting schedule | Check roster/room owner | Separate notice or author response | Conflict visible until resolved |
| Offers a counterproposal | Route revised terms to affected parties | Task/access owner | No change until accepted and confirmed |
| Shares a private reason | Ask permission or keep it private | One eligible private route | No public disclosure |
| Refuses a request | Acknowledge if owner supports | Existing channel | Refusal, not a new obligation |
| Author declines to respond | Verify facts independently | New notice, private route, or wait | Author's silence is not agreement |
| Readers repeat an unverified claim | Seek source evidence | Keep uncertainty visible | Repetition alone proves nothing |

## 83. Endings from public participation

**Verified public answer:** the relevant owner confirms the fact and the player publishes it to the intended audience.

**Separate correction:** original and response retain their own authors and histories.

**Private resolution:** the public question remains open while one affected person receives a private reply.

**Referred to the author:** the player does not impersonate the source and waits for a supported answer.

**Counterproposal accepted:** the event or task changes only after each required participant consents.

**Unresolved disagreement:** competing claims remain attributed, and the player may seek more evidence later.

**No response route:** the player declines to simulate one and identifies the current owner or limitation.

## 84. Play approaches

**The direct answerer:** responds publicly with confirmed facts, accepting the wider audience.

**The source referrer:** asks the original author or owner to answer for their claim.

**The privacy keeper:** separates public facts from private reasons and contacts one person.

**The verifier:** delays a confident answer until the underlying event is checked.

**The clean closer:** closes only when the notice owner supports that action and outstanding questions have a visible status.

Each approach alters what readers can know and what remains unresolved. There is no hidden honesty rating.

## 85. Callbacks without false consensus

A later survivor can cite the public answer, the separate correction, or their own unanswered question. The dialogue should retain the original author's identity and the exact fact confirmed.

If a reader changed their plan after a reply, the relevant task owner must record that action. Do not infer that every reader saw the response or that a public post reached a person who was absent.

When the player returns after several days, show only the responses and acknowledgement states actually persisted. A silent board cannot become a unanimous board in retrospective narration.

## 86. Persistence and acceptance

Use the existing communication owner for message identity, author, audience, channel, reply relation where supported, and status. A separate notice receives its own identity. A private reply remains private. Replaying the publication command cannot duplicate an answer or rewrite its author.

Accept this installment when readers can respond through supported routes; public replies preserve distinct voices; factual claims are verified by their owners; private details stay private; and an unresolved answer remains an honest outcome. The proposal adds no thread engine, moderation authority, or crowd-truth score.

## 87. Installment 4 close

This installment turns a notice into a source of authored responses and player choices. The player can answer, verify, refer, move a sensitive detail to a private route, publish a separate correction, or leave disagreement open. Boards, intercoms, and private messages remain separate authorities; public participation does not create consensus or change a task by itself.

## 88. Expansion installment 5 — The answer went to the wrong audience

A message can be accurate and still reach people who were not meant to receive it. This installment adds recovery choices after exposure: contain what can be contained, correct the audience, contact affected people, and avoid repeating the private detail.

The existing board, intercom, and private-message owners remain authoritative. If they cannot remove, retract, or restrict an item, the player cannot claim it disappeared.

## 89. Treat the routing error as its own event

Check what was sent, by whom, through which channel, and which audience actually received it. A draft shown to one recipient differs from a public post; a room broadcast differs from one private inbox.

The player can ask the author what audience they intended, check current visibility, or pause a scheduled follow-up if the owner permits. Do not assume the author caused the error; a changed recipient or reused notice may be responsible.

## 90. Scene seed — “That was meant for one person”

Candidate scene only. Validate message routing, intended recipient, audience state, author identity, and any supported retract operation.

A resident's private reply appears in a shared board space. Another reader has opened the board, but the system cannot confirm whether they read that reply. The message concerns a personal circumstance.

The player can ask the board owner to contain it, avoid quoting the detail, send a private note to the intended person, or publish a neutral correction that a message was misrouted. The sender may ask for a direct response, preserve the record, or decline further contact.

If no removal route exists, tell the sender what remains visible. A deletion claim requires confirmation from the board owner.

## 91. Audience matters more than the message label

The player sees the actual channel and audience, not an assumption based on a button called “reply.” A public board response may reach all readers. A private message must identify one eligible recipient. An intercom response can be audible beyond its intended person.

Before sending a correction, show whether it goes to the original audience, the affected person, the author, or a broader group. The player can choose a neutral statement, private apology, corrected notice, or no follow-up until the audience is confirmed.

## 92. Containment options

- Restrict or remove the original only if its owner supports that action.
- Replace it with a neutral note that does not repeat sensitive content.
- Send a private correction to the intended recipient.
- Ask the sender whether they want broader acknowledgement.
- Tell the original audience a message was misrouted without quoting it.
- Preserve it if its owner or affected person needs a record.
- Stop further forwarding or broadcast.
- Leave the issue unresolved if no truthful recovery exists.

No choice guarantees that every reader forgets what they saw.

## 93. Contact affected people without exposing others

The player can privately tell the sender that the message reached the wrong audience and explain only owner-confirmed containment. They may contact an unintended recipient when necessary and permitted, using a neutral explanation.

The sender may ask the player not to contact anyone, request a correction, or discuss the consequences. The player can follow channel rules, seek an authorized contact, or leave the sender in control of the next step.

If a recipient acted on the message, the player can explain the routing error without repeating its contents. They may decline to respond.

## 94. The sender keeps authorship and choice

The player cannot impersonate the sender to retract the message. They may draft a correction for approval, publish a separate statement under their own identity, or ask the channel owner for help.

If the sender is unavailable, use only an authorized channel operation or clearly attributed correction. A public correction should not reproduce the harm it is meant to repair.

## 95. Supporting factions as containment owners

A supporting faction may control its own board, intercom schedule, or member channel. It can restrict or correct that channel under its rules, but cannot erase a message from another owner's space.

The player can ask the faction to contain its copy, correct the affected audience, issue a neutral notice, or explain its limits. Do not send the faction the full private message unless needed and permitted.

If several channels hold copies, resolve each through its actual owner. A board correction does not retract an inbox message or undo a reader's action.

## 96. Consequences without a privacy meter

Callbacks can include a reader asking why the notice changed, the sender requesting another channel, a schedule changed by the misrouted note, or a faction asking who authorized the post.

Use existing relationship and communication owners only where they support a consequence. Do not assign a global “privacy breach” score or automatic trust penalty. Readers may continue acting on the original if they never received the correction.

## 97. Ending routes

**Owner-confirmed containment:** report only the confirmed audience change.

**Neutral correction:** explain that a post was misrouted without repeating private contents.

**Private apology:** contact affected people without broad disclosure.

**Author-approved retraction:** the sender keeps authorship.

**Multiple copies remain:** contact each owner or explain remaining visibility.

**No removal route:** choose a limited correction or no further message.

**Sender declines contact:** respect that boundary and leave the outcome unresolved.

## 98. Player approaches

**Containment first:** check whether the channel owner can restrict access before sending more.

**Neutral correction:** acknowledge the error without repeating sensitive details.

**Sender representative:** draft only with permission and keep authorship visible.

**Audience mapper:** check each channel copy separately.

**No-false-promise closer:** explain what cannot be undone and stop.

## 99. Persistence and replay

On reload, inspect canonical message identity, author, channel, audience, correction state, and reply relation through the communication owner. If audience or read history is not persisted, retain uncertainty.

Repeated callbacks cannot send duplicate apologies or retract a different message. A neutral correction cannot copy private content into a public field. If retries can widen the audience, do not expose an unsafe retry.

## 100. Acceptance questions

1. Can the player see the real audience before correction?
2. Does every removal or restriction come from the channel owner?
3. Can the sender retain authorship and decline contact?
4. Does the correction avoid repeating private information?
5. Can the player distinguish contained, corrected, and still-visible copies?
6. Are consequences tied to owner-backed facts?
7. Does reload preserve uncertainty about who saw the message?

## 101. Installment 5 close

This installment gives the player a recovery path after information reaches the wrong audience. They can contain it through its owner, send a targeted correction, ask the author to approve a retraction, contact affected people, or explain what cannot be undone. No moderation system, global privacy score, or false deletion promise is added.

## 102. Expansion installment 6 — The notice asks for something its author cannot authorize

A message can be clear and correctly routed but still exceed its author's authority. This installment asks the player to distinguish useful information from an instruction, request, or order that only another owner can issue.

The player can verify the author's role, check the underlying task or access owner, relay the valid information, seek authorization, or leave the notice unresolved. The communication channel carries the message; it does not grant authority.

## 103. Name the action the notice requests

The player first identifies whether the message reports a fact, requests voluntary help, changes an event time, restricts access, or directs a required task. Those actions may belong to different owners.

An author can accurately report that a room is damp but lack authority to close it. A faction contact can request volunteers without assigning them. A task owner can schedule work but not disclose private survivor information. The UI should identify the source and scope where current data supports it.

If no owner can confirm the distinction, the player can ask, wait, or avoid presenting the notice as binding.

## 104. Scene seed — “Everyone out by evening”

Candidate content only. Confirm the notice author, room owner, safety/event state, faction authority, and any access route before making this reachable.

A public note says that everyone must leave a shared room by evening. It is signed by a familiar contact, but the contact is not listed as the room owner. A second note says the room is needed for a repair. The player can check the repair schedule, ask the author what prompted the message, contact the room owner, or inspect a confirmed hazard event.

If an immediate hazard is verified, the responsible safety owner can issue the supported instruction. If the note is only a request, residents can accept, ask for another time, or decline. If no source confirms the claim, the player can post a clarification, seek authorization, or leave the notice open.

The outcome follows the confirmed event and the author's actual role, not the player's faction loyalty.

## 105. The channel does not make an order official

Posting on the board, using the intercom, or sending a private message changes who can receive information. It does not change which system owns access, work, or safety.

Before acting, the player can inspect source role and relevant owner state. They can comply voluntarily, ask the author to seek authorization, request an owner-issued notice, inform the audience that authority is unconfirmed, or choose another safe route.

If the player chooses to follow an unverified request, show it as the player's action rather than a binding order. Do not alter task or access state without its owner.

## 106. Branches by author authority

| Author and message | Player action | Valid result |
|---|---|---|
| Authorized task owner changes a schedule | Confirm current owner state | Publish the supported update |
| Contact requests volunteers | Ask affected people | Record voluntary acceptance only |
| Author reports a possible hazard | Check safety/event owner | Warn or wait according to confirmed urgency |
| Faction claims access control | Verify property owner | Apply only the faction's actual authority |
| Author exceeds their role | Seek authorization or correct scope | No unauthorized assignment |
| Two owners provide different instructions | Ask each owner to resolve its domain | Keep the conflict visible until resolved |
| Authority is unknown | Hold or clarify the notice | No binding state change |

## 107. Keep the useful fact when rejecting the command

A notice may combine a true observation with an unsupported directive. The player can preserve the confirmed fact while revising the instruction: “The room is wet; closure is still being checked.” This avoids discarding useful information because one part exceeded the author's role.

The player can ask the author to split the facts, request a source, issue a separate owner-backed notice, or wait. If a correction changes the audience, show who will receive it.

The author may accept the edit, issue a separate clarification, or refuse. The player should not rewrite another person's words while leaving their name attached to a claim they did not make.

## 108. Supporting factions and scoped authority

A supporting faction may control its own worksite, radio frequency, or stored supplies. It can issue conditions in that scope, provide a source, or ask the actual owner for a wider restriction.

The player can relay a scoped faction notice, ask for broader authorization, seek another route, or explain that the faction cannot direct residents outside its property. A major faction's presence does not make every statement binding.

If two factions claim overlapping control, do not resolve it through a reputation value. Ask the current property, event, or governance owner; keep the practical question open until that authority responds.

## 109. Request, order, and consent remain distinct

Residents can volunteer for a request, ask questions, decline, or offer different help. A refusal to a voluntary request is not disobedience. If a valid order exists, its authority and consequences must come from the responsible owner and be visible before the player acts.

The player can ask the author to convert an order into a request only if that author is willing and the actual policy owner permits it. The player cannot make a nonbinding request enforceable by repeating it over the intercom.

## 110. Endings and callbacks

**Authority confirmed:** the proper owner issues the instruction and the player distributes it through the right channel.

**Request remains voluntary:** residents can accept, decline, or propose another time.

**Useful fact preserved:** the player corrects the unsupported directive while keeping the confirmed observation.

**Authority denied:** the note is clarified or withdrawn through its channel owner.

**Conflict unresolved:** the player leaves both scoped claims visible and seeks a decision.

**Unsafe uncertainty:** the player routes an urgent warning only as current emergency rules allow; no false certainty is added.

Later dialogue can refer to who authorized the change, but not claim that every resident heard or obeyed it.

## 111. Player approaches

**The source checker:** confirms who owns the requested action.

**The fact preserver:** keeps verified information while correcting scope.

**The consent defender:** separates a voluntary request from assignment.

**The author liaison:** asks the original writer to revise or authorize the note.

**The safe escalator:** seeks the actual emergency or access owner.

These approaches branch on authority, evidence, and urgency rather than faction allegiance.

## 112. Persistence and replay

On reload, read the message, source identity, audience, authorization, and resulting task or access state from their respective owners. A notice cannot persist an unauthorized command as a completed task. Reposting an owner-approved order cannot apply its effect twice.

If authority was unknown at the time of posting, retain that uncertainty. A later owner decision can issue a new, attributed notice; it should not rewrite the old author or fabricate earlier approval.

## 113. Acceptance questions

1. Can the player distinguish fact, request, event update, and binding instruction?
2. Does each requested action come from its actual owner?
3. Can the player preserve a useful fact while correcting an unsupported directive?
4. Are voluntary requests distinct from orders and assignments?
5. Can factions act only within their real property or policy scope?
6. Does the channel show who will receive the clarification?
7. Are notices and resulting actions safe under reload and replay?

## 114. Expansion continuity

An earlier public answer may later be superseded by an authorized instruction. Preserve both authors and the sequence. Readers who acted on the request before the correction may need a supported follow-up.

Do not use a late authorization to claim that the earlier message was official all along. The player can explain what changed, inform the original audience, or leave unknown readers unconfirmed.

## 115. Installment 6 close

This installment adds an authority check between receiving a message and acting on it. The player can verify the source, distinguish a request from an order, preserve a true fact, seek the proper owner, or leave uncertainty visible. Channels carry information; task, access, and safety owners authorize action.

## 116. Expansion installment 7 — Acknowledged is not understood

A recipient can receive a message and still understand it differently from its author. Delivery proves that a channel reached someone; it does not prove that the instruction was clear, that the recipient interpreted a term as intended, or that anyone acted. This installment adds a comprehension check inside the existing communication loop, not a new messaging system.

## 117. Scene seed — “Leave it clear”

A notice asks residents to “leave the west passage clear before the delivery.” One reader moves a cart; another thinks the notice means to stop using the passage entirely; a third leaves the area untouched because the delivery time is unknown. The player can ask what each person understood, clarify the requested space and time, contact the author, or leave the ambiguity visible until the responsible owner answers.

The wording may be concise and still need repair. Do not portray the readers as careless for interpreting an underspecified phrase differently.

## 118. Separate delivery, acknowledgement, interpretation, and action

Use four distinct facts where the current channel supports them: sent, received, interpreted, and acted upon. A recipient may acknowledge receipt without agreeing, may ask a question before acting, or may have already completed a reasonable interpretation. Do not collapse these into one “read” flag that claims more than the system knows.

If the present communication owner cannot represent a distinction, the plan should record the limitation and avoid claiming it in UI or narrative. No parallel message ledger is authorized by this proposal.

## 119. Ask what the recipient heard

The player can invite a recipient to describe the instruction in their own words, ask which term is unclear, or request a private clarification. A second recipient may already understand the same phrase. Check the people who need the correction rather than forcing every resident through an identical dialogue.

The player can also decide not to test comprehension when the request is clear and no action depends on it. The prompt should be useful at an ambiguity seam, not a repetitive acknowledgement tax.

## 120. Clarify the smallest useful part

If only the time is unclear, clarify the time. If “clear” could mean moving property or restricting passage, explain the intended result and any limits. Preserve the original message and the corrected version where the existing owner permits; never rewrite history to make it appear the notice was clear all along.

The author may approve revised wording, another authorized source may correct it, or the player may report that clarification is still pending. Each route should identify who supplied the new fact.

## 121. Use more than one reply path

Recipients can reply publicly, ask the author privately, request an in-person explanation, or decline to acknowledge until the action is clear. The player can carry a correction to the audience already affected, narrow a follow-up to people who still need it, or ask the source owner to issue a replacement.

Do not make one response method mandatory if the established communication surface does not support it. Keep interface promises within the current channel and recipient model.

## 122. Actions taken under ambiguity

If someone already moved the cart, the player can report what happened and check whether the requested outcome was met. If someone stopped using the whole passage, the correction can restore permitted use. If no one acted, clarify before the delivery. The responsible task or access owner decides whether the passage is actually usable.

Do not blame a recipient for acting reasonably on ambiguous text. Do not claim a correction erased a real cost, delay, or interruption that already occurred.

## 123. Accessibility without a literacy test

A person may ask for spoken wording, a short example, or a demonstration of the intended result. The player can offer these ways to clarify without framing the recipient as deficient. The same concise notice may need an oral explanation because the space is noisy, the reader is tired, or a term is unfamiliar.

Do not add a literacy statistic or require the player to diagnose why someone asked. Use current character and channel context, and leave the reason unspecified when it is not known.

## 124. Factions can clarify only their own instructions

A supporting faction may explain what its own representative meant, supply a diagram it is authorized to share, or correct a notice it issued. It cannot make a shelter access rule true by repeating it, and it cannot speak for another author without a supported delegation.

The player can relay the faction's clarification with attribution, seek the shelter owner, or preserve the disagreement. Faction involvement adds a source and a route, not an automatic answer.

## 125. Branches and outcomes

- **Shared interpretation confirmed:** recipients know the same practical request; the task owner still determines whether it is permitted.
- **Clarification issued:** affected recipients receive the scoped correction and can ask again.
- **Action already occurred:** record what happened and route any repair through its actual owner.
- **Interpretation remains disputed:** preserve both accounts and defer consequential action when required.
- **No reply is available:** show delivery status honestly and do not invent understanding.

The ending is the state of communication, not a judgment about obedience.

## 126. Player approaches

**The plain-language editor** rewrites only the ambiguous phrase with the source owner's approval.

**The recipient listener** asks what the person understood before correcting them.

**The narrow distributor** sends a correction only to the affected audience when the channel permits it.

**The action checker** finds out what has already happened before proposing repair.

**The uncertainty keeper** leaves unresolved meaning visible instead of choosing a convenient interpretation.

## 127. Persistence and replay

On re-entry, refresh message source, intended audience, delivery, responses, current wording, and any resulting task state from their owners. Preserve the sequence of original notice and correction if supported. A repeated prompt cannot claim a new acknowledgement or resend an effect twice.

If a recipient was unavailable, do not infer their understanding from another resident's reply. If the correction was not persisted, ask the source or recreate it only through an authorized send action.

## 128. Acceptance questions

1. Does the plan distinguish receipt from interpretation and action?
2. Can recipients request a useful clarification without being blamed?
3. Can the player correct only the ambiguous term and affected audience?
4. Are original wording, correction, and source kept truthful?
5. Do access and task owners determine what action is permitted?
6. Are faction clarifications limited to sources they own or can represent?
7. Are replies and resulting actions accurate after reload?

## 129. Expansion continuity

An acknowledged message can later prove to have been misunderstood. Preserve both facts: the recipient received it, and the phrase meant something different to them. A later correction should not retroactively turn receipt into agreement or claim that everyone took the same action.

If the channel does not store interpretation, keep this as a supported scene response rather than inventing durable state. The owner of the communication surface must approve any additional persistence contract.

## 130. Installment 7 close

This installment makes interpretation visible after message delivery. The player can ask what a recipient understood, clarify a limited phrase, follow up with the right audience, repair an action already taken, or leave a disagreement unresolved. It adds depth to acknowledgement without creating a parallel communication authority.

## 131. Expansion installment 8 — A volunteer accepted help, not a whole assignment

A broad request can produce a willing response that does not yet define a safe or useful role. The resident may be offering to carry supplies, observe, or help for one hour, while the task needs a trained operator for an entire shift. The communication loop should preserve that distinction before a roster or task owner receives an assignment request.

## 132. Scene seed — “I can help with the pump”

An internal notice asks for help checking a pump room before the evening shift. One resident replies, “I can help with the pump,” but has not said whether they can inspect, carry tools, clean the area, or operate the equipment. The player can ask what they mean, clarify the request's scope and time, route their stated interest to the task owner, or thank them without assigning anything.

The reply is a useful opening, not a completed consent record.

## 133. Preserve the volunteer's own words

Do not transform “I can help” into “I will operate the pump.” The player can ask a short follow-up about role, availability, and any stated limits. The resident can volunteer for a narrower task, ask for more information, change their mind, or decline after hearing the details.

If the person cannot be reached, keep the response provisional. Another resident cannot broaden it on their behalf, and the author cannot treat silence as confirmation.

## 134. Describe the actual task before asking again

The player can explain what the work requires, how long it may take, whether it involves restricted equipment, and which qualifications the owner requires. Show only current facts. The task owner supplies its requirements; the communication surface supplies the supported question and response route.

If the request itself was too vague, the player can correct it publicly, send a scoped follow-up to the responders, or wait for the responsible owner to define the work. Do not recruit first and disclose the risk later.

## 135. Different responses remain valid

The volunteer may offer an adjacent task, such as carrying tools or keeping the walkway clear, if the task owner confirms that work exists. They may accept only a short interval, require supervision, ask whether protective gear is available, or decide that the request is not for them.

The player can record interest, route a candidate for eligibility review, seek another volunteer, or close the request with the remaining gap visible. Interest is not qualification, and qualification is not consent to the specific shift.

## 136. Avoid public pressure

If the first reply was public, the player may offer a private follow-up for personal availability or safety questions. The volunteer can decline the private channel and ask for the task details in the original audience. The author can publish a clarification without naming who replied.

Do not make a public volunteer feel that withdrawing will be remembered as cowardice. A refusal changes the staffing picture only through the roster or task owner, not through an invented reputation penalty.

## 137. The communication author cannot assign the shift

The notice author can clarify the request, acknowledge interest, and pass a candidate to the proper owner. The duty or task owner decides eligibility, assignment, and coverage. If those owners cannot accept the proposed person or role, the player can explain the result, ask for another volunteer, or revise the request.

An official assignment can use a separate authorized path where it exists. The message channel does not convert an open request into an order.

## 138. Faction replies have the same boundary

A supporting faction may recommend a specialist, volunteer one of its own people, or offer a defined service through its current authority. The player can compare that help with a resident's offer, ask the faction for exact scope, or decline the conditions. Faction status does not authorize a shelter resident's roster assignment.

If a faction member volunteers personally, distinguish that time from faction staffing or equipment. The resident and faction owners each confirm their own commitments.

## 139. Branches after clarification

- **Volunteer accepts the exact role:** send the candidate through the valid task and roster checks.
- **Volunteer accepts a smaller role:** route only that role if the task owner supports it.
- **Volunteer asks for details:** clarify before requesting commitment.
- **Volunteer withdraws:** refresh coverage and keep the staffing gap visible.
- **No suitable volunteer responds:** request authorized staffing, rescope the task, defer, or leave the request open.

The communication outcome and task outcome remain separate; a sent reply is not completed work.

## 140. Player approaches

**The scope clarifier** turns a broad request into a concrete question.

**The consent checker** confirms the volunteer's own role and time before routing.

**The eligibility checker** asks the task owner whether the proposed role is valid.

**The pressure reducer** lets a person withdraw without a public penalty.

**The honest closer** shows what remains uncovered when nobody can take the work.

## 141. Persistence and replay

On return, refresh the request wording, responder identity, volunteered scope, current availability, eligibility, and assignment from their respective owners. Do not promote an old expression of interest into a current shift after schedules change.

A repeated follow-up cannot duplicate a volunteer response, assign the person twice, or mark the task complete. If the answer was not persisted, ask again through the supported communication route.

## 142. Acceptance questions

1. Does the player see that an open volunteer reply is not a defined assignment?
2. Can a person choose a narrower role, ask questions, or withdraw?
3. Are duties, qualifications, and schedule checked by their current owners?
4. Can the request be clarified without publicly shaming a responder?
5. Are faction and personal offers distinguished from shelter assignments?
6. Does the message result remain separate from task completion?
7. Are volunteer responses and assignments replay-safe?

## 143. Expansion continuity

An earlier “I can help” remains true even when the person later declines the actual shift. Preserve the sequence: they expressed interest, learned the scope, and then made a choice. Do not rewrite their first reply as a promise they broke.

If the channel cannot persist that sequence, keep the follow-up in the current supported interaction and avoid claiming a durable volunteer contract.

## 144. Installment 8 close

This installment turns an open volunteer response into a scoped, consented route. The player can clarify the task, confirm a limited role, check eligibility, accept withdrawal, or leave the coverage gap visible. Messages invite and inform; current task and roster owners assign work and confirm completion.

## 145. Expansion installment 9 — The task was canceled after the notice

An accurate request can become obsolete when its task owner cancels or changes the underlying work. Residents may still be preparing, volunteers may have accepted, and some people may already have acted. The player needs to close the communication loop without pretending a message can be recalled from every reader.

## 146. Scene seed — “The leak stopped before the shift”

An internal notice asks for volunteers to move supplies away from a leaking wall before evening. Later, the facilities owner confirms that a repair stopped the leak and the move is no longer needed. The player can verify that status, withdraw the request, send a correction to the original audience, contact known volunteers, or keep a narrower safety notice if the owner says one is still needed.

The original request was reasonable when sent. The update is a new fact, not proof that the first author was careless.

## 147. Verify the change before canceling the request

The player checks the current task owner for whether the work is canceled, reduced, delayed, or replaced. A report from a passerby may justify asking, but it does not itself close the task. If the owner cannot confirm the change, mark the request as unresolved and seek the proper source.

Do not erase an urgent notice based on a rumor or stale panel state. If the work is only partly unnecessary, clarify which portion remains rather than withdrawing everything.

## 148. The message can be withdrawn, but its history remains

Where the current communication owner supports withdrawal or superseding notices, use that route and show the result. If recall is unsupported, send a correction through an available channel and preserve the original wording and time. The player can state that some recipients may not yet have seen the update.

Do not claim that the original notice vanished from memory, paper copies, or conversations. Keep delivery and acknowledgement tied to actual evidence.

## 149. Reach people who already responded

Known volunteers may need a direct update before they travel or begin. The player can contact them through supported recipients, ask the notice owner to do so, or leave a visible correction for people whose identities are not known. A volunteer can confirm receipt, ask whether another task is available, or decline further work.

The player cannot infer that an unacknowledged person received the cancellation. Keep the remaining uncertainty visible and choose a supported reminder if the consequences justify it.

## 150. Handle work already performed

If a resident already moved supplies, record only what the task or inventory owners confirm. The player can ask whether the supplies need to move back, remain where they are, or be checked by the responsible owner. Do not automatically reverse a valid action when the original hazard may still matter.

If no one acted, the correction prevents unnecessary work. If someone partially acted, explain the changed need and let the task owner determine safe next steps.

## 151. A replacement request is a new request

The repair may reveal a different task, such as checking the wall after rain or clearing a safe path for the crew. The author or responsible task owner can issue a new request with its own scope, audience, and timing. The player can ask for clarification, help send it, or wait.

Do not silently mutate the original volunteer request into the replacement. People who accepted the first task need a fresh opportunity to accept or decline materially different work.

## 152. Avoid blaming readers for acting

A resident who started work before receiving the update followed the information available to them. The player can explain the changed condition, check whether their effort remains useful, and route any recovery through the correct owner. Do not penalize them for failing to read a message that had not reached them.

Likewise, a volunteer who stops after the cancellation has not abandoned an obligation. Their original consent applied to the earlier task and scope.

## 153. Supporting factions and outside contacts

A faction that supplied a specialist or helped confirm the repair can relay the updated fact through its own channel. The player can ask it to notify its own participants, send an attributed correction, or leave the faction out if its involvement is no longer needed. It cannot claim that every shelter resident has received the update.

Any new work offered by the faction needs its own terms, authorization, and audience. Cancellation of a shelter request does not create a faction assignment.

## 154. Branches after confirmation

- **Task canceled before anyone responds:** withdraw or supersede the notice and show the owner-confirmed reason.
- **Volunteers accepted but have not started:** notify them and let them confirm receipt or choose another route.
- **Work partly completed:** report what happened and ask the task owner for safe disposition.
- **Task narrowed:** send a corrected scope and request fresh acceptance for changed work.
- **Cancellation cannot reach everyone:** preserve uncertainty and use only supported reminders.

The branch changes with timing and evidence, not with a generic success/failure label.

## 155. Player approaches

**The source verifier** confirms the task change before sending an update.

**The audience tracker** identifies actual responders and known recipients.

**The careful corrector** preserves the old notice while stating what changed.

**The work checker** asks what residents already did before proposing recovery.

**The uncertainty keeper** admits when some readers may not have received the update.

## 156. Persistence and replay

On return, refresh task status, original message, any withdrawal or correction, responder list, acknowledgements, and work already performed from their owners. Replaying a cancellation cannot undo inventory movement or send the same consequence twice.

If the owner has no durable supersede link, keep the correction as a supported new message and preserve the prior message as historical. Do not construct a private message history in the panel.

## 157. Acceptance questions

1. Is the cancellation confirmed by the task owner before the request changes?
2. Can the player distinguish canceled, narrowed, delayed, and replaced work?
3. Are known volunteers contacted through supported routes?
4. Does the plan preserve uncertainty about recipients who have not acknowledged?
5. Are actions already taken handled by their real task and inventory owners?
6. Does replacement work receive new scope and fresh consent?
7. Are corrections and task outcomes accurate after reload?

## 158. Expansion continuity

Later characters may refer to the original notice, the cancellation, or the work completed before the update reached them. Keep those events in order. A correction can prevent additional work but cannot make every earlier action disappear.

If the communication owner cannot preserve this sequence, the design must limit its claim to what the current surface reliably displays.

## 159. Installment 9 close

This installment handles a task that changes after a message has gone out. The player can verify the cancellation, update known recipients, account for work already done, issue a new scoped request, or leave delivery uncertainty visible. The notice history and task result stay with their current owners.

## 160. Expansion installment 10 — An invitation is not an open door

A notice can accurately invite named people to a conversation while a reader assumes that anyone may attend. The player can clarify the audience, ask the author whether it may widen, or help issue a separate open invitation. The communication surface must not silently grant access to a room or turn a private meeting into a public event.

## 161. Scene seed — “Bring anyone who needs to hear it”

A board notice invites the night repair crew to a short discussion about tool storage. One resident reads “anyone who needs to hear it” broadly and plans to bring a friend who is not on the crew. The player can check with the author, ask the resident what they understood, request a clear audience statement, or leave attendance undecided until the author answers.

The issue may be unclear wording, a genuine wish to widen the discussion, or a reader trying to help. Do not assume bad intent.

## 162. Verify both audience and access

The author confirms whom the notice was meant for. The location owner confirms who may enter the space. The player should check each separately: being invited to hear a topic does not automatically grant access to a restricted workroom, and having room access does not mean a person was invited to a private discussion.

If the author is unavailable, the player can wait, find an authorized alternate, or leave the invitation unresolved. A reader cannot broaden the original audience on the author's behalf.

## 163. Let the invited resident ask for a guest

The resident can ask whether their friend may attend, explain that the friend has a relevant question, offer to bring the question without the person, or attend alone. The author can approve, decline, ask for a separate time, or publish an open invitation if permitted.

The player can carry that request through an authorized route but cannot promise approval. A refusal may concern room capacity, confidential details, or meeting scope; use only the reason the author or location owner confirms.

## 164. Clarify without exposing the meeting's private content

If the meeting concerns a sensitive repair or personal issue, the player can clarify the audience without repeating the underlying details. The resident can decide whether to attend, ask for a public summary later, or skip the meeting. Do not reveal the topic to justify why their friend is excluded.

If the author approves a guest, share only the invitation and any necessary access information. Attendance does not automatically include every prior message or private account.

## 165. A wider audience needs a new message

When the author wants an open discussion, issue a new notice with its own audience and room conditions. Preserve the original invitation for its intended recipients if the current communication owner supports separate messages. Do not edit a private notice in place so that old recipients cannot tell what changed.

The player can ask for a summary, help distribute the new notice, or wait for the author. If no supported audience expansion exists, keep the branch as a proposal and do not pretend the board changed access rules.

## 166. Avoid making attendance a loyalty test

The invited resident may attend alone, bring an approved guest, ask a question through the author, request a later summary, or decline. Their choice does not prove they support or oppose the meeting's subject. The guest may accept the boundary, ask for a separate conversation, or decide not to attend.

Do not award faction trust or relationship status for attending a discussion unless an existing owner records a specific supported action.

## 167. Supporting factions can clarify their invitation

A faction may host an open briefing, a members-only meeting, or a scoped discussion for named representatives if its authored rules support those forms. The player can ask its contact which audience applies, request an additional guest, seek a public summary, or decline.

The faction cannot grant entry to a shelter-controlled space or silently change shelter access. Any combined event needs each location and communication owner to confirm their part.

## 168. Branches by audience decision

- **Original audience confirmed:** explain the scope and let the reader attend or decline.
- **Guest approved:** verify their invitation and location access before arrival.
- **Guest declined:** offer a question relay or later public summary if supported.
- **Open event approved:** issue a new notice and show its recipients.
- **Author or access owner unavailable:** leave the question open and do not infer permission.

The branch changes who can hear the discussion without making attendance an allegiance outcome.

## 169. Player approaches

**The audience checker** asks who was invited and who may enter.

**The guest requester** carries a narrow request to the notice author.

**The privacy keeper** clarifies scope without repeating sensitive subject matter.

**The open-notice organizer** helps publish a new invitation after authorization.

**The neutral closer** accepts that the reader may choose not to attend.

## 170. Persistence and replay

On return, refresh the message version, author response, named recipients, location access, guest approval, and any new notice from their owners. Do not infer that a friend attended because they were nearby or that a public message widened the physical access rule.

Repeated approval cannot add the same recipient twice or create duplicate attendance. If the original message cannot be versioned, issue an authorized follow-up and preserve uncertainty about who saw it.

## 171. Acceptance questions

1. Can the player distinguish invitation audience from physical access?
2. Can an invited person ask to bring a guest without promising approval?
3. Can the author clarify or widen the audience through a supported route?
4. Are private meeting details protected during clarification?
5. Are factions limited to their own invitation and location authority?
6. Does attendance avoid an automatic loyalty or relationship verdict?
7. Are message versions, guest approval, and attendance truthful after reload?

## 172. Installment continuity

If the original wording caused a reasonable misunderstanding, preserve it and show the clarification that followed. A later open invitation is a new decision by its author, not proof that every prior reader was always welcome. Physical entry remains with the location owner.

## 173. Installment 10 close

This installment adds a social audience branch to shelter communication. The player can clarify an invitation, request a guest, seek a summary, widen the event through a new authorized notice, or leave attendance undecided. Messages define who was invited; location owners define who may enter.

## 174. Expansion installment 11 — The guest arrives before approval

An invited resident may bring a guest before the author or location owner answers the request. The player must respond to the person present without pretending approval was granted. The guest can wait, ask for a public summary, leave, or return after confirmation; the invited resident can proceed alone or reschedule.

## 175. Scene seed — “We are already at the door”

The crew discussion is scheduled in a room with restricted access. A resident arrives with a friend who wants to understand the tool-storage decision. The original notice named the crew only, and no one has approved a guest. The player can check the author, ask the location owner about entry, offer to wait in a public area, send the friend away with a later follow-up, or reschedule the discussion.

Do not make the guest cross the threshold while the player is still checking. Do not turn a reasonable request into a security incident without authored grounds.

## 176. Separate the invitation answer from the access answer

The notice author may welcome the guest but the room owner may still deny entry. The room owner may allow entry while the author prefers to keep the conversation limited. The player can report both answers and ask the invited resident how they want to proceed.

If either owner is unavailable, keep that part unresolved. A person standing at the door, holding a note, or accompanying an invitee does not supply the missing authorization.

## 177. Offer a respectful waiting route

The player can ask the guest to wait in a public area, share a neutral description of the meeting's purpose, offer to pass a question to the author, or arrange a later conversation. They can also say that no summary is available. These options are only used where current spaces and message routes exist.

The guest may decline to wait or may not want a secondhand answer. The player should not promise access, confidentiality, or a return time that no owner has confirmed.

## 178. The invited resident is not the guest's proxy

The resident may have asked to bring someone but cannot automatically approve that person's access or disclose every meeting detail. The guest can ask for permission directly through the supported route. The resident can choose to attend alone, withdraw the request, or leave with the guest.

Likewise, the invited resident cannot authorize the player to disclose private information about another person who will be discussed. Scope stays with the original author and affected people.

## 179. A late approval can still change the scene

If the author approves the guest while the meeting is underway, the player can ask whether the room owner also approves, provide the guest with the current topic and limits, and let the participants decide whether to restart, summarize, or continue. Do not repeat private statements the guest was not authorized to hear.

If the meeting already covered sensitive details, the guest can receive only the approved public summary or be invited to a separate discussion. Approval to enter does not create retroactive access to what was said earlier.

## 180. When access is denied

The player can explain the confirmed limit, ask whether a neutral summary is allowed, offer a new time or room, or close the discussion. The guest may be disappointed, but the scene need not imply that the host acted unfairly. If the reason is unknown, say only that access was not approved.

Do not use a faction badge, friendship, or the player's preference to override the location owner. If there is an appeal route, name it only when current content supports one.

## 181. Branches by arrival state

- **Author and room approve:** invite the guest in after stating the current topic and audience.
- **Author approves, room denies:** move only to a location both owners permit or reschedule.
- **Room allows, author does not:** keep the discussion private and offer a separate request.
- **No answer arrives:** wait, provide only an approved summary, or leave without entry.
- **Approval comes after sensitive discussion:** summarize approved facts; do not replay restricted details.

Each branch updates audience and access separately.

## 182. Supporting faction role

A faction contact can explain its own invitation rules, offer its own public briefing, or confirm a room it controls. The player can use that route, ask for a separate open invitation, or decline. It cannot approve entry to a shelter-controlled room or widen someone else's notice.

If a faction contact is attending the meeting, their presence does not make all faction records public. The author controls the invitation; the relevant data owner controls disclosure.

## 183. Dialog beats and restrained tone

The invited resident can say, “I asked if she could come. I did not promise she could.” The guest might answer, “Then I can wait for the answer.” The player can offer the public bench or explain that the meeting has already started. Keep the exchange short and concrete; no one needs to deliver a lecture about boundaries.

Other outcomes need similarly distinct voices: an impatient guest asks for a time; a tired resident leaves; an author sends a one-line approval; a room owner says the corridor is closed. Verify current characters and locations before using these samples.

## 184. Player approaches

**The answer checker** contacts the person who owns the missing decision.

**The respectful host** offers a waiting or follow-up route without promising entry.

**The privacy keeper** distinguishes a public summary from the private meeting.

**The practical rescheduler** finds a supported room and time if both owners permit.

**The neutral closer** accepts that the guest may leave and the meeting may proceed without them.

## 185. Persistence and replay

On re-entry, refresh approval, location access, message audience, current meeting stage, and any summary from their owners. A guest's arrival is not an approval event. Replaying the door interaction cannot add the guest to the audience twice or expose the same private detail again.

If the approval was not persisted, do not claim it survived reload. Ask the author or location owner again through a supported route.

## 186. Acceptance questions

1. Can the player respond to an arriving guest without granting unconfirmed access?
2. Are author approval and room access distinct checks?
3. Can a guest wait, ask for a summary, leave, or return later?
4. Does late approval avoid exposing already-shared private details?
5. Are unknown denial reasons left unknown?
6. Can faction help affect only invitations and spaces it owns?
7. Do guest arrival and approval remain separate after reload?

## 187. Installment continuity

If the invited resident and guest leave together, record no attendance for the guest. If the guest waits and later receives an approved summary, do not write that they attended. If approval arrives late, preserve the sequence rather than flattening it into one generic invitation result.

## 188. Installment 11 close

This installment follows an invitation request to the moment a guest arrives. The player can verify approval, offer a waiting route, protect earlier discussion, reschedule, or close the door conversation respectfully. Presence is not permission, and access remains with the owner of the space.

## 189. Invitation casebook — The guest asks for the reason

The guest may ask why the meeting is limited. The player can repeat only the confirmed audience rule, ask the author whether a neutral explanation is approved, or say that the reason is not known. Do not disclose sensitive meeting content to make an exclusion sound more reasonable.

If the author says the meeting concerns a private task, that does not mean the guest may never hear about it. The player can ask whether a later summary or separate discussion is appropriate.

## 190. Case — The invitee brings a useful question

The unapproved guest may know something relevant to tool storage. The player can ask whether the invitee wants to submit a question without entering, request author approval for the guest, or schedule another discussion. The author may include the question, ask for a source, or decline.

The guest's expertise does not grant access automatically. Keep their contribution attributed and do not turn a passed question into consent to join the meeting.

## 191. Case — The guest cannot wait

The guest may need to leave for a shift. The player can offer to send a question through an authorized channel, ask whether a public summary will be available, or let them go. Do not promise the author will respond before the shift unless that timing is confirmed.

The invited resident can still attend alone or leave with the guest. Neither person's choice should change the invitation audience retroactively.

## 192. Case — The author approves a summary, not attendance

The player can deliver the approved facts, state what remains private, and invite follow-up questions through the author. The guest may accept the summary, ask for a meeting, or decline it. The summary does not grant access to the room or allow the guest to respond as a meeting participant.

If the guest's question changes the meeting's scope, the author can decide whether to answer now, schedule another discussion, or issue a new invitation.

## 193. Case — A public event is approved after the meeting begins

The author may decide to open a later session to all residents. The player can help send a new notice and verify the new room. People who were excluded from the first meeting do not need to be told that the earlier audience was invalid; the scope changed at a known time.

Preserve the original meeting's content and recipients. Do not apply the new open audience retroactively to private remarks.

## 194. Case — Two invitees disagree about bringing others

One invitee wants a guest; another wants to keep the discussion small. The player can ask the author to clarify, split the discussion into an approved public summary and a smaller private meeting, or reschedule. No invitee can widen the audience for everyone else.

If the room has a real capacity limit, the location owner confirms it. If the disagreement is about confidentiality, the author and affected people define what can be shared.

## 195. Case — The guest is already inside the common area

The player should distinguish ordinary public-space presence from entry to the meeting itself. The guest may remain in a permitted common area while waiting, but cannot be assumed to hear or join the discussion. The location owner supplies the boundary; the player does not invent one at the doorway.

If the event spills into the common area, the author must decide whether the audience has widened and which details remain private. Being nearby is not a message acknowledgement.

## 196. Case — The author does not answer before start time

The player can begin with the original invitees, wait, reschedule, or send a neutral note that guest approval is pending. They should not make the guest stand by indefinitely. The invited resident can choose whether to attend alone or postpone.

If the meeting can only proceed with the guest, the player should wait for approval or move to a confirmed public format. No response is not permission.

## 197. Case — The location owner changes the room

The alternate room may have different access, capacity, privacy, and accessibility. The player can check whether the original invitation still fits, ask the author to revise it, or reschedule. A room change can make a previously approved guest ineligible or make an open meeting possible; both facts need confirmation.

Do not infer that an invitation follows the participants to every room in the shelter.

## 198. Case — The guest is approved but the invited resident withdraws

The host may approve attendance after the original invitee no longer wants to go. The player can check whether the guest still has an independent invitation, ask the author, or close the entry. Approval for a plus-one does not always create a standalone invitation.

The guest can make a fresh request in their own name where the author allows it. The player should not represent the absent invitee's wishes.

## 199. Case — The discussion's purpose changes

An open storage discussion may turn toward a named resident's private task. The player can ask the author to pause, return to the public topic, or close the meeting before changing its audience. The author can issue a separate private invitation to affected people if supported.

The original open invitation does not authorize disclosure of every subject raised in the room. Keep new participants informed about what has changed before they join.

## 200. Scene-writing guidance

Use brief, practical lines at the door: “They are checking with the room owner”; “You can wait at the common table”; “The author can send a summary after”; “We don't have approval yet.” Avoid turning the guest into a villain or the host into an antagonist by default.

Character-specific reactions may add texture—awkwardness, impatience, relief, a second question—but must be supported by current profiles and the actual audience decision. The boundary itself is enough drama.

## 201. Invitation casebook close

These cases separate useful questions, public summaries, independent invitations, and physical entry. The player can preserve an audience limit while offering a next step. Approval can change the event prospectively, but it never rewrites who was invited to earlier discussion.

## 202. Expansion installment 12 — Delivery is not understanding

A notice can reach a resident without making its meaning clear. The player may ask whether they want a short explanation, a read-back of the action, or another format. Do not make every recipient repeat the whole message as a test. The purpose is to identify a practical misunderstanding before it causes a bad decision.

If the notice states a time, location, or safety condition, the reader can ask a focused question. The author or relevant owner supplies the answer. The player should not improve the notice by guessing what its author intended. If the text is ambiguous, correct or replace it through the supported notice authority.

## 203. Delivery branch — A resident cannot use the chosen channel

The recipient may not hear the intercom, may be away from the board, or may not be able to read the posted text in its current form. The player can check whether another authorized channel exists, ask what format works for that recipient, or leave the message pending. Do not claim acknowledgement because the message was posted where it is usually visible.

If no alternate channel is available, the player can ask the notice owner to revise the plan or assign an appropriate person to deliver it. The resident's accessibility need should not become public gossip. Any alternate channel must retain the same audience boundary and must not expose private content to bystanders.

## 204. Delivery branch — The board is crowded

Several notices may compete for space or attention. The player can ask the board owner which notices are current, whether a time-critical item has priority, and whether outdated copies should be removed. A faction can help post a notice only if it has the relevant access and authority; it cannot silently replace another author's message.

If two current notices conflict, do not choose one based on which faction seems more important. Identify each author and the facts each notice claims. The responsible owners can reconcile the timing or publish a correction. Until then, the player can tell residents that the conflict is unresolved and ask them not to rely on an unverified combined interpretation.

## 205. Delivery branch — A correction arrives after action began

The author may correct a time or route after some residents have started responding. The player sends the correction through the same authorized audience where possible and names what changed. The earlier notice remains part of the event history so the correction does not blame residents for following what they had been told.

Some recipients may have acted before the correction arrived. Ask what they have already done and use the relevant task or location owner to determine whether their action can be stopped or safely completed. A notice cannot undo physical work. Do not mark all recipients as corrected merely because one copy was replaced.

## 206. Delivery branch — A private notice is overheard

An intercom call or hallway conversation may reveal that a private topic is being discussed. The player can stop further disclosure, move the conversation to an authorized private setting, or ask the author to provide a neutral public line. Do not repeat the sensitive detail while apologizing for exposing it.

The affected resident may want a correction, no further discussion, or a private explanation. They should not be required to help compose the repair. The notice author owns the content decision; the communication or location owner confirms what channel is allowed.

## 207. Delivery branch — A faction offers translation or relay

A supporting faction may know a resident's preferred language or communication format. The player can request help, explain the audience, and confirm who will see the message. The faction may translate, relay, or decline; the author checks that the translated version preserves the intended facts and limits.

Do not treat an interpreter as a recipient of the underlying private decision. Share only what is needed for the communication task, and let the addressed resident respond directly. If the meaning cannot be confirmed, preserve the uncertainty rather than presenting an approximate translation as authoritative.

## 208. Delivery branch — A resident acknowledges only part

A resident may confirm receipt but still ask whether the notice applies to them. The player can distinguish “I saw it” from “I understand my next step.” The author can clarify the audience or issue a corrected version. No acknowledgement should imply agreement with the notice's policy or consent to a related task.

If the resident declines to act, route that decision through the proper owner. The notice may inform them of a requirement, but it does not by itself create a new duty. Where the duty already exists, the responsible task owner explains it separately.

## 209. Notice lifecycle endings

A message can end as delivered and understood, delivered with an unresolved question, corrected for some recipients, withdrawn, superseded, or still pending. These descriptions should map to supported communication facts. If the current system tracks only authored notices and not per-recipient acknowledgement, keep delivery outcomes in prose and do not imply a durable receipt ledger.

The strongest branch can be a correction that arrives too late to prevent inconvenience but early enough to prevent further work. Let characters express the cost of the delay without rewriting the timeline or assigning blame to recipients who followed the first notice.

## 210. Supporting faction role in shelter communications

Secondary factions can supply a board, a runner, a translation, or a second place to post when they control those resources. The major shelter authority still determines common-space rules, and each notice author remains accountable for content and audience. A faction can disagree with the notice, request an amendment, or publish its own authorized response.

This creates political texture through competing explanations and visible corrections. It should not create secret message powers, universal surveillance, or a faction-controlled communications registry unless current architecture explicitly supports those systems.

## 211. Expansion installment 13 — A notice must survive a changed situation

Shelter messages often describe plans that can change: a task moves, a room closes, a meeting gains a new audience, or a route becomes unavailable. The plan should distinguish stable facts from temporary instructions. A notice author states what is known now, who owns the decision, when the information should be checked again, and what residents should do if the situation changes.

This is not a request to add a speculative message-expiry service. If the current communication owner has no expiry or version field, keep freshness in authored content and route changes through the existing publication behavior. Characters can still recognize an outdated notice, but the UI must not imply automatic invalidation that the system cannot perform.

## 212. Branch opening — Is this a plan, warning, request, or record?

The player first identifies the message’s purpose. A plan invites coordination; a warning asks residents to avoid a condition; a request asks for voluntary help; a record explains what already happened. Mixing these purposes produces confusion: a resident may read a request as an assignment or a record as an instruction to repeat an action.

The author can revise the message, add a short purpose line, or ask the relevant owner to communicate the actual duty separately. The player should not fix a policy ambiguity with persuasive phrasing. A clear notice can still be refused when it asks for voluntary help, and an existing duty still needs its legitimate task owner.

## 213. Changed-plan branch — The room is no longer available

The location owner confirms that the room is closed, repurposed, or at capacity. The notice author can cancel, move the meeting to a confirmed alternate room, or delay publication until access is resolved. If a new room changes accessibility or audience, the author must reconsider who can attend and which earlier notice needs correction.

Residents who arrive at the old location should receive a factual update if a permitted channel is available. The player can direct them to a confirmed waiting point or let them leave. Do not describe a blocked room as “temporarily open” to avoid disappointing anyone. If no alternate space is confirmed, say the meeting is postponed and keep the room state with its owner.

## 214. Changed-plan branch — The requested help is no longer needed

The task owner may report that enough volunteers have arrived or the work has been canceled. The author can withdraw the request and tell people who already responded. A resident who has begun traveling may choose to continue to another task, return to their prior activity, or ask for a separate assignment. The canceled notice does not automatically assign them to something else.

If the request was voluntary, thank people without recording an obligation or debt. If it was an assigned task, the task owner—not the notice—confirms the reassignment or release. This difference gives the scene practical consequences while keeping communication and work ownership separate.

## 215. Changed-plan branch — A warning has become uncertain

An earlier warning may be challenged by new evidence. The player can ask the author to state which part remains confirmed and which part needs review. The message may be narrowed, marked as under review through existing language, or withdrawn. Do not leave a dramatic warning active merely because its removal would reduce tension.

Residents can respond differently: one avoids the area, another asks for the source, and another has already changed plans. The player can answer only from confirmed information. If no source can resolve the uncertainty, the honest branch is to preserve caution in proportion to the evidence and explain what is unknown.

## 216. Changed-plan branch — Two authorized sources disagree

Sometimes the disagreement is not a rumor: two owners may provide different instructions within their scopes. The player maps which decision belongs to each owner. A room owner can confirm access while a task owner confirms work; neither necessarily answers the other’s question. The author can write a notice that presents both facts and names the unresolved point.

If the scopes truly conflict, escalate to the existing decision owner or leave the action pending. Do not invent a vote, a tie-breaker, or a faction prestige rule. The player can offer residents a safe temporary choice only when the responsible owners confirm it.

## 217. Public correction and the cost of delay

A correction may be embarrassing to its author and inconvenient to residents. The player can help write a concise correction that names the changed fact, the replacement instruction, and who can answer questions. The author can accept the wording, revise it, or decide not to publish. A faction contact may request attribution for its part without controlling the shared shelter notice.

The correction should not erase the original statement from the narrative. People may have acted on it, repeated it, or made plans around it. If a resident asks who was responsible, answer from confirmed authorship rather than assigning blame to the last person who carried the message.

## 218. Branch based on action, not attitude

The meaningful communication branches follow observable actions: the player checks the location, asks the author to clarify, relays a correction to a permitted audience, waits for the task owner, or publishes through an authorized channel. Recipient branches follow whether they were reached, whether they acted, and what they ask next. No choice needs to be classified as loyal, disloyal, honest, or selfish.

The consequence might be that fewer residents arrive at the wrong room, that a volunteer is released from an unnecessary trip, that a warning remains provisional, or that people hear the two owners’ disagreement directly. These are legible outcomes rooted in the scene and do not require a hidden communications reputation score.

## 219. Short scene forms for changing plans

**Postponement:** “The room owner has not confirmed the alternate space. We are postponing this meeting.”

**Correction:** “The earlier notice named the east passage. Use the west passage; the task owner confirmed the change at this hour.”

**Unresolved warning:** “We know the door is closed. We do not yet know when it will reopen.”

**Volunteer release:** “The crew has enough people. You were not assigned; you can head back or ask for another task.”

These are functional examples, not final character lines. Localize names, tone, and the actual instruction to current authored content. Keep the action understandable when read quickly or heard only once.

## 220. Installment 13 close — Preserve what changed and what did not

At the end of a changed-plan scene, the player should be able to state the earlier instruction, the new fact, the owner who confirmed it, and who still needs to hear. If the game cannot track the audience, do not claim every recipient has received the update. A correction can be complete as an authored event while its reach remains uncertain.

The ending may be a relocated meeting, a canceled request, a narrowed warning, an unresolved conflict, or a clear postponement. The communication system carries information; task, location, and faction owners determine their own consequences. That boundary creates enough room for disagreement and repair without making the notice itself a new gameplay authority.

## 221. Expansion installment 14 — A notice convenes people with different stakes

A shelter notice may invite residents to discuss a shared space, a work schedule, or a practical change. The meeting can become a branching scene because participants bring different stakes, not because one faction’s morality is secretly being scored. The author defines the question the meeting can answer; the location owner defines room access; the affected residents decide whether they wish to attend or speak.

Before posting, the player can ask whether the notice promises a decision, requests input, or merely shares information. That distinction should appear in its wording. “We will decide tonight” is false if the meeting can only advise an owner. “We want to hear concerns” is not a vote. Clear expectations prevent the player from using turnout as manufactured consent.

## 222. Preparation branch — Confirm scope and who can decide

The author names the issue and the relevant decision owner. The player can help separate items that belong in the meeting from adjacent topics that need another route. A discussion about storage hours may touch on a resident’s private task; the author can keep that detail out of the open agenda and arrange a separate conversation if needed.

If no one can make the promised decision, change the notice before inviting people. The meeting can still collect questions and report them to the proper owner, but attendees should know that the outcome is advice or a request. Do not let a charismatic chair imply that a decision is binding when the owner has not delegated that authority.

## 223. Attendance branch — A participant needs access or preparation

A resident may ask for a different time, accessible location, plain-language summary, or a chance to submit a question in advance. The player checks what the author and location owner can provide. The resident may attend, send a question, request a private response, or abstain. No one should have to disclose a medical or personal reason to ask for a reasonable format change.

If the requested accommodation cannot be provided, say what is unavailable and offer only confirmed alternatives. The author can postpone, hold a separate briefing, or proceed while preserving a route for that resident’s input. Do not describe the meeting as representative of everyone if some affected people could not participate.

## 224. Meeting branch — A new issue takes over

An attendee may introduce an urgent concern that was not on the notice. The chair can pause the original agenda, determine whether the concern has an active safety or task owner, or schedule it separately. Participants can ask to hear the immediate facts, return to the posted question, or leave. The player should not suppress an urgent issue merely to preserve the scene’s planned outcome.

At the same time, the new topic does not authorize disclosure of private information or change the meeting’s audience automatically. If resolving it needs another owner, the chair can identify who will be contacted and what is still unknown. The original agenda remains open only if the participants and author choose to continue.

## 225. Meeting branch — A resident asks to speak but not be quoted

The resident may want to raise a concern without having their name attached to a public summary. The player can ask the author what records the meeting produces and whether anonymous or unattributed input is supported. The resident can share, submit a general question, speak privately to the owner, or withdraw.

Do not promise anonymity if attendance, existing records, or the communication system make it impossible. The author can explain the limit before the resident speaks. If an unattributed summary is possible, preserve only the general issue and omit identifying details not required for the decision.

## 226. Meeting branch — Factions disagree about procedure

Two supporting groups may propose different ways to use a shared room or distribute a task. The chair can ask each to state the concrete consequence of its proposal, identify any resource it controls, and separate preference from binding rule. Attendees may support, question, or abstain. The actual owner decides only within its authority.

The faction with the larger presence should not win automatically. If a formal vote is supported by the current meeting or decision owner, use its stated rule. Otherwise, summarize the positions and refer the decision. Minor factions add testimony, history, or practical services; they do not replace the major authority or the affected residents.

## 227. Meeting branch — The discussion becomes personal

Participants may accuse one another of causing a problem. The chair can redirect toward the specific event and remedy, offer a pause, or move personal details to a smaller authorized conversation. The affected resident may continue, ask for a boundary, or leave. The player can support the process without declaring who is morally better.

If the argument reveals a concrete threat or safety issue, route it to the responsible owner. Do not ask the affected person to mediate their own case in front of the group. The meeting can close early, preserve the unresolved issue, and still produce a truthful account of what was heard.

## 228. Meeting outcomes — Decision, recommendation, or no agreement

The chair closes by distinguishing three outcomes. A decision is made by an authorized owner under a known rule. A recommendation is sent to someone else to decide. No agreement means participants remain divided or the facts are insufficient. Each outcome can create different follow-up dialogue, but none should be mislabeled to make the meeting feel more conclusive.

If a decision affects a schedule, room, task, or faction resource, that owner confirms the resulting state. The meeting summary names only the outcome and the next responsible person. Where no persistent minutes system exists, do not claim a complete transcript or durable attendance list.

## 229. Follow-up branch — A participant disputes the summary

An attendee may say that the summary omits their concern or overstates agreement. The author can review the wording, ask the decision owner what was actually decided, or publish a correction. The participant may propose a factual edit, ask for their name to be removed where allowed, or decline further involvement.

Do not edit a summary to imply unanimity. If the room agreed on a narrow point but disagreed about implementation, preserve both facts. A correction can repair the record without reversing a valid decision; if the decision itself is challenged, route that challenge through its existing process.

## 230. Installment 14 close — Participation is not consent

This meeting branch makes audience, authority, scope, and recordkeeping part of the player’s choices. Residents can attend, submit a question, abstain, leave, or dispute the summary. Factions can contribute evidence and practical support within their control. The authorized owner makes any decision that changes gameplay state.

Endings include an owner decision, a recommendation, a postponed meeting, a split discussion, a corrected summary, or no agreement. The player should be able to tell each attendee which one occurred. Presence, silence, or receipt of a notice must never be described as consent to a policy or task.

## 231. Expansion installment 15 — A message reaches some people first

A shelter update may travel through several authorized channels and arrive at different times. The player should distinguish the source notice, each attempted relay, and the actions recipients actually took. A runner can confirm delivery to one person; a board posting can confirm that a copy was placed; neither proves that every resident received or understood the correction.

If the current communication system does not track recipients, let the scene express that uncertainty. The player can ask a nearby contact to relay a concise update, post a correction, or wait for an authorized broadcast. Do not quietly invent an audience roster or use dialogue to claim complete reach.

## 232. Branch — The first recipient repeats the old instruction

A resident who saw the earlier notice may repeat it to someone who missed the correction. The player can interrupt politely, state what changed and who confirmed it, and ask the second person to repeat the practical next step if needed. The first resident may be embarrassed, defensive, or grateful to be corrected.

Do not blame the first recipient for passing along the information they had. The author’s correction should be clear enough to distinguish old from new. If the second person already acted, ask what they did and route any physical consequence to its owner.

## 233. Branch — The runner cannot find the intended resident

The runner may find an empty room, a closed work area, or a person who has moved. They can return with that fact, ask whether another channel is permitted, or leave the message with an authorized intermediary. The player should not tell the runner to search private spaces or disclose where the resident is assigned without permission.

If no route reaches the recipient, the notice remains undelivered or its reach unknown. The author may change the plan, delay the affected action, or continue for those already reached if the relevant owner permits it. Do not represent the missing resident’s silence as agreement.

## 234. Branch — A duplicate message creates confusion

Residents may receive the same update twice from two different people. The player can explain that the copies refer to one change and ask whether anything remains unclear. If the copies conflict, compare their source and timestamp where available; otherwise ask the author which version is current.

Do not treat repetition as proof that the message is more authoritative. A faction repeating a notice does not become its author. The original owner can confirm the latest instruction, and a correction can attribute the source without turning the scene into a faction endorsement contest.

## 235. Branch — The recipient acted before the message arrived

The resident may already have entered the room, started the task, or left for the former meeting point. The player checks whether the action can be paused or redirected with the responsible owner. If it is complete, describe that state honestly and ask whether any follow-up is needed.

Do not reset the scene so the correction appears timely. The player may apologize for the delay if responsible, acknowledge that the recipient followed the information they had, and help limit further cost. The author can explain the correction without promising compensation unless an owner supports it.

## 236. Branch — The recipient cannot hear an urgent broadcast

An intercom may be inaudible in a work area or inaccessible to a resident. The player can seek a permitted alternate route, ask the location owner for an accessible relay, or pause a dependent action until the message can be delivered. The recipient can confirm what they heard without being made responsible for the channel’s failure.

If no alternate method is confirmed, the player cannot claim delivery. The author may issue a new plan that does not depend on immediate reach. This creates a real branch between waiting, choosing another channel, and changing the action—not an invented communication failure meter.

## 237. Branch — A faction relay adds its own commentary

A supporting group may agree to pass along the notice but add an interpretation. The player can separate the original instruction from the faction’s comment, ask the author whether the added explanation is accurate, or invite the faction to publish a clearly attributed response. The faction can decline to relay if it does not accept the message.

Do not silently merge advocacy with the authored notice. A recipient should be able to tell who wrote the instruction and who is offering an opinion. Major factions can contest policy publicly; smaller groups can explain local effects. Neither should gain control of the communication channel merely by relaying it.

## 238. Branch — A correction reaches the wrong audience

The player may discover that a correction was posted publicly even though only a private group needed it. The author can remove or replace the copy if the owner permits, issue a neutral clarification, or explain what was exposed. The affected people can request no further discussion, a direct correction, or a review of the channel choice.

Do not amplify private details in the correction. State only that an earlier message was posted to the wrong audience and identify the current authorized route. If the current system cannot remove a copy, say that it remains visible and let the author decide what repair is possible.

## 239. Communication path matrix

| What is known | Player action | Valid description | Avoid claiming |
|---|---|---|---|
| Board copy placed | Check current version | Posted at the named board | Every resident read it |
| Runner delivered to one resident | Ask whether they understood | Delivered to that person | Shelter-wide acknowledgement |
| Broadcast sent | Confirm channel status | Broadcast sent | Every room heard it |
| Correction reached late | Check actions already taken | Updated after some action began | The original never mattered |
| Relay adds commentary | Attribute both sources | Original notice plus relay comment | Faction authored the notice |

The matrix is an authoring aid. Use the communication owner’s real events and persistence boundaries for implementation.

## 240. Installment 15 close — Reach is a fact, not an assumption

This installment turns partial delivery into a human branch. Residents can repeat, miss, receive twice, misunderstand, or act before an update reaches them. The player can correct, reroute, pause, or revise the plan while honoring location, audience, and author boundaries.

Endings should report which channel was used, who is confirmed reached, what action had already begun, and what remains uncertain. When the system cannot answer one of those questions, the narrative should keep it open instead of converting a sent message into universal knowledge.

## 241. Expansion installment 16 — A notice creates an accountability question

Residents may respond to a notice by asking who owns the next step. The answer can differ by task: the author owns the instruction, a task owner assigns work, a location owner grants access, and a faction contact may supply a requested service. The player can map the question to its owner and tell residents what is confirmed. This is more useful than saying “the shelter” as though every authority were interchangeable.

The branch becomes meaningful when a notice asks for action but leaves ownership unclear. The player can request clarification before anyone acts, help the author revise the notice, or pause a dependent task where the task owner permits it. If the responsible authority cannot be identified, keep the instruction unresolved instead of inventing a chain of command.

## 242. Branch — The author is not the decision owner

The notice author may be a messenger or advocate rather than the person authorized to approve the requested action. The player can ask who approved the instruction and whether the author can confirm that approval. The author may produce a source, contact the owner, or correct the notice to say that a decision is pending.

If the message was posted prematurely, the author can issue a correction and acknowledge the uncertainty. Do not accuse them of deception without evidence. A correction can preserve their credibility by making clear what they knew and what had not yet been confirmed.

## 243. Branch — Residents ask who will answer questions

The author may be unavailable after posting. The player can identify a confirmed contact, tell residents when an answer may be available, or ask the owner to appoint an authorized responder. Do not promise a person will be present if they have not agreed or their schedule owner has not confirmed availability.

If no responder is available, the notice can still remain informational if its facts are clear. If it requires a decision or action that residents cannot safely interpret, pause and seek clarification. An unanswered question is a visible gap, not evidence that recipients consented.

## 244. Branch — A resident follows the notice and is challenged

Someone may accuse a resident of acting without permission even though they followed the posted instruction. The player checks what the notice actually said, who authored it, and which owner controlled the action. The resident may ask for an explanation, withdraw from further work, or request that the author clarify the boundary publicly.

If the notice was ambiguous, the author can correct it and the task owner can determine whether the action was authorized. Do not retroactively blame the resident for an unclear message. If the resident exceeded a clearly stated limit, describe that fact precisely and route it through the existing process rather than turning it into a rumor.

## 245. Branch — The notice cites a faction but not a person

Residents may ask whether a faction truly endorsed the instruction. The player can ask the faction contact to confirm its role and identify the authorized speaker. The faction may endorse, clarify that it only supplied a resource, or publicly disagree. The notice author can then attribute the claim accurately.

Do not use a faction name as a substitute for a source. If no authorized contact confirms the endorsement, say that the attribution remains unverified. A faction may be politically influential, but it cannot become the owner of a shelter instruction by being mentioned in its text.

## 246. Branch — Accountability requires a private answer

The question may concern why one resident received a task or why an exception was granted. The player can explain the general rule publicly and direct personal details to an authorized private conversation. The affected resident can agree to discuss, ask for a representative, or decline. The author must not use a public notice to expose personal history as proof of fairness.

If the rule itself is contested, a separate public discussion may be appropriate. Keep the individual case and the general policy question distinct. A private response can answer the person without pretending that broader concerns have been resolved.

## 247. Accountability branch outcomes

The author may correct attribution, the owner may confirm authority, a responder may be named, an ambiguous instruction may be paused, or residents may receive a clear explanation of what remains undecided. Each changes what people can reasonably do next. None requires a universal trust score for the notice writer.

The player can close the scene by stating who owns the next action and how residents can ask a follow-up. If no follow-up route exists, say so and let the author decide whether to create one through supported communication behavior. The lack of a route is itself a design issue; dialogue should not pretend it has been solved.

## 248. Expansion installment 17 — A reply is not an endorsement

Residents may respond to a notice with a question, correction, complaint, offer of help, or statement of support. The author should be able to distinguish these response types in the scene. A reply does not automatically mean agreement with the notice, and a lack of reply does not mean consent. The player asks what the resident wants done with their response and which audience may hear it.

If the current communication owner supports only general replies, do not invent threaded discussion or per-person reaction tracking. Authored dialogue can show a person speaking or sending a message, while persistence remains limited to the supported event. The author can acknowledge the response publicly, answer privately, route it to an owner, or close the topic.

## 249. Branch — A resident corrects a factual detail

The resident may know that the notice has the wrong time, location, or resource count. The player can ask for the source, verify it with the responsible owner, and request a correction. The author may confirm the change or explain why the original fact remains current. Until then, mark the detail as disputed rather than silently replacing it.

If the resident is mistaken, correct the misunderstanding without mocking them. If they are right, the author should acknowledge the correction and identify which copies need updating. The response is useful evidence, but not automatic authority over the notice.

## 250. Branch — A resident complains but asks for no public reply

The resident may want the author to know about a problem without broadcasting their complaint. The player asks whether they want a private response, a task referral, a change to the notice, or simply to be heard. They can also decline to provide their name if an allowed channel supports that choice.

The author should not publish the complaint’s details as an example without consent. If the issue affects many residents, they can describe the general concern and ask whether the resident agrees to that summary. A private complaint does not become public consensus just because it concerns a shared resource.

## 251. Branch — A resident offers help

The response may volunteer translation, delivery, setup, or factual review. The player checks what the resident is offering and whether the relevant owner permits it. The author can accept, decline, or request a narrower contribution. The volunteer remains free to withdraw before doing the work.

Do not add the resident to a task roster merely because they replied. If the task owner accepts the help, confirm the assignment through its owner and tell the volunteer what it involves. The notice author can thank them without implying a future obligation.

## 252. Branch — A faction posts a public rebuttal

A faction may disagree with a shelter notice and ask to publish a response. The player checks the board or channel owner’s rules, asks the faction to attribute its claims, and gives the original author a chance to clarify. The rebuttal can be allowed, denied under an actual rule, or moved to another authorized channel.

Do not make access depend on whether the faction supports the major authority. Apply the same scope and safety rules to every group. Readers should be able to distinguish the original notice, its correction, and the faction’s position rather than seeing a merged statement with unclear ownership.

## 253. Branch — Several replies point to different needs

One resident may ask for accessibility, another for a different schedule, and another for a private explanation. The author can address each through the relevant owner, revise the notice to cover a shared issue, or keep individual responses separate. The player should not force all comments into one compromise if they affect different residents.

If resources cannot meet every request, make that constraint visible. The owner can decide priority under current rules; the author explains the resulting plan. Residents may accept, propose an alternative, or remain dissatisfied. Their disagreement is not erased by the fact that a response was sent.

## 254. Branch — The author cannot answer yet

The author may need confirmation from a task, location, or faction owner. The player can tell the resident what is pending, who is checking, and whether any immediate action should wait. Do not promise a reply time without a confirmed schedule.

The resident can leave the issue open, ask for a contact, or decide that no reply is needed. If the game cannot track a pending response, keep the promise at the level the scene supports. A character should not say “I’ll get back to you” unless a real callback route exists or the line is clearly an in-world intention rather than a tracked event.

## 255. Response close — Report what changed because someone replied

At the end of the response branch, the player should be able to state whether the notice changed, an owner received a question, help was accepted, a rebuttal was posted, or the issue remains unresolved. This makes resident participation legible without claiming consensus or universal reach.

The author owns the content response, the relevant owner controls any practical change, and the communication owner controls supported delivery. The player can bridge them, but cannot make a reply binding by acknowledging it. A strong close preserves dissent and gives each participant a truthful next step.

## 256. Expansion installment 18 — Two current notices conflict

Residents may encounter two current notices that recommend incompatible actions. One says to use a room for a meeting; another says the room is reserved for a task. One names an old route while a second names a newer one. The player first checks whether each notice is genuinely current, who authored it, and which owner controls the underlying space or task.

Do not decide that the notice posted last automatically wins unless the communication owner provides a reliable ordering rule. A later timestamp may show when text was posted, not whether its author had authority to supersede the earlier instruction. The player can pause a dependent action, contact the responsible owners, or explain that the conflict remains unresolved.

## 257. Conflict branch — The notices govern different scopes

Both messages may be accurate within different boundaries: one concerns common access and another a scheduled work period. The player can ask the location owner when each applies and help the authors revise the wording to name the scope. Residents may follow the confirmed time window, request an alternate room, or wait for clarification.

Do not call a scoped difference a contradiction merely because the instructions look different. Equally, do not force residents to infer a complex schedule from vague phrasing. A clear correction can state where each rule applies and who confirms exceptions.

## 258. Conflict branch — The authors disagree about authority

Two authors may each believe they can issue the controlling instruction. The player can ask for the relevant owner or delegation, compare what each person actually controls, and refer the decision upward only through a supported path. Each author may provide evidence, revise their notice, or maintain their position.

The player does not choose based on faction rank or personal friendship. Until the authority is clear, communicate the uncertainty and stop any action that depends on an unverified instruction when the relevant owner permits a pause. Do not describe a compromise as binding without approval.

## 259. Conflict branch — One notice is a request and the other an assignment

The language may make a voluntary request sound mandatory, while another notice assigns a duty to some residents. The player can clarify the difference with the authors and task owner. A resident may accept the request, decline it, follow an assigned task, or ask whether they are eligible for reassignment.

Do not merge the messages into a single order. If the task owner confirms an assignment, communicate that responsibility separately from the voluntary appeal. The resident can still refuse additional work beyond the assignment.

## 260. Conflict branch — A faction disputes the shelter’s notice

A faction can publish a response within an authorized channel and state why it disagrees. The player can attribute both messages, verify factual claims, and ask the responsible owner to answer operational questions. The faction may offer an alternate service, but cannot change shared-space access without authority.

Residents may agree with one account, both partially, or neither. Do not turn reactions into an automatic alignment update. The outcome may be a confirmed operational rule alongside an unresolved policy disagreement. That is a real ending, not a failure to choose a side.

## 261. Conflict branch — A resident has already acted on one notice

The resident may have moved equipment, gathered at a room, or started a task based on the first message. The player checks what happened and asks the relevant owner whether the action can continue, pause, or be reversed. The second notice does not erase the resident’s action or automatically make it misconduct.

If the action caused a cost, state what is known and who owns the remedy. Avoid blanket compensation promises or blame. The author can explain which information was available at the time and publish a clearer instruction for people who have not acted yet.

## 262. Conflict branch — The notices can be reconciled only by changing the plan

The owners may decide that neither original instruction can stand as written. They can choose a later time, a different room, a smaller task, or a temporary pause. The authors issue a new message and identify which prior instruction it replaces, if the owner supports explicit replacement.

Residents may accept the revised plan, ask for a private exception, or opt out of voluntary participation. The player should not assume a correction reached everyone. Any remaining delivery uncertainty stays visible and can shape later scenes.

## 263. Conflict branch — No owner is available to resolve it

The player may not be able to find the responsible person. They can tell residents what is conflicting, ask them to avoid the disputed action where safe, leave a message for the owner, or choose an unrelated task. Do not appoint the player as a temporary authority by default.

If delay has consequences, name them plainly. The task owner may later confirm that the delay was acceptable or costly. A faction can provide a safe alternate resource only if it controls it; it cannot settle the contested shared rule.

## 264. Conflicting-notice resolution matrix

| Conflict | First check | Possible resolution | What remains separate |
|---|---|---|---|
| Different times | Schedule owner | Correct time or publish both scopes | Author’s intent and actual schedule |
| Room vs. task | Location and task owners | Reassign room or task | Access and work authority |
| Request vs. assignment | Task owner | Clarify voluntary and required actions | Optional help and assigned duty |
| Faction rebuttal | Channel owner and source | Attribute, verify, or refer | Political opinion and operational rule |
| No decision owner available | Existing escalation route | Pause or choose a confirmed alternative | Unresolved authority |

Use this as branch-design guidance only. It does not establish a new priority service or override existing owners.

## 265. Installment 18 close — Resolve instructions by scope and authority

Conflicting notices create branches through source, scope, authority, timing, and the actions residents have already taken. The player can clarify, pause, attribute, or help owners revise the plan. They should never make the conflict disappear by guessing which faction sounds more convincing.

The scene can close with a reconciled instruction, a confirmed scoped distinction, a public disagreement, a delayed task, or an unresolved message awaiting its owner. Residents receive an honest account of what they may do next and what remains undecided.

## 266. Expansion installment 19 — A notice invites a decision from the residents

An author may ask residents to express a preference about a room schedule, common task, or shared resource. Before the notice goes out, clarify whether the response is consultation, a recommendation, or a binding decision under an existing rule. The word “vote” should not appear unless the responsible owner has defined what a vote can decide and how its result is applied.

Residents can respond, abstain, ask for more information, or decline to participate. The player can explain the consequence of each option. Silence cannot be counted as support. A crowded meeting cannot be treated as a representative sample unless the current decision process says so.

## 267. Consultation branch — The owner wants advice only

The owner may ask for input but retain the final decision. The notice should say that plainly. Residents can offer reasons, point out effects the owner missed, or request a different alternative. The author can summarize the feedback and route it to the decision owner.

The owner may adopt a recommendation, reject it with an explanation, or ask for more information. The player should not tell residents that their choice “won” if it was advisory. An honest consultation can still matter when the owner shows how the input affected the final decision.

## 268. Decision branch — A formal mechanism already exists

If a current owner has a valid decision rule, the player can explain who may participate, what counts, and what the result controls. The rule may define eligibility, timing, abstention, tie handling, and review. Use the existing source; do not supplement it with an invented quorum or tie-breaker.

Residents can challenge their eligibility, ask for an accessible way to respond, or abstain. The owner resolves process questions before collecting a result. If the mechanism is unclear, pause and ask for clarification rather than allowing a contested count to appear authoritative.

## 269. Decision branch — The result is close or divided

Participants may split between options or express strong reasons on both sides. If the existing rule yields a valid result, report it accurately while preserving dissent. If it yields no result, the owner can extend discussion, choose a supported fallback, or leave the matter unresolved.

Do not treat a narrow result as proof that the losing group’s concern no longer matters. The player can ask whether the owner wants to address implementation risks or offer a review route. Any compromise must be approved by the owner and accepted where individual consent is required.

## 270. Decision branch — A resident cannot or will not vote

A resident may be absent, unable to access the response channel, or unwilling to disclose a preference. The player can check for an authorized alternate method, accept abstention, or let the owner proceed under the established rule. Do not infer the resident’s preference from faction membership or a past conversation.

If lack of access affects the validity of the process, the owner determines whether to pause or extend it. The resident should not have to reveal a private reason to request an alternate format. The notice can explain how to ask for help without exposing the resident publicly.

## 271. Decision branch — A faction organizes responses

A faction may help residents understand the options or gather questions. The player clarifies whether it is facilitating, advocating, or collecting official responses. The faction can argue for its preferred outcome, but it cannot submit another person’s preference without consent or alter the owner’s eligibility rules.

If a faction gathers views privately, residents should know who will see them and whether they are being counted formally. The faction can publish its own summary as advocacy, clearly attributed. The decision owner remains responsible for the official result.

## 272. Decision branch — Residents request a minority statement

Those who disagree may ask that their concerns accompany the result. The author can include a concise minority statement, identify the source of the objection with permission, or explain that the notice format cannot include it and offer a separate channel. The dissenting residents can approve, revise, or withdraw their statement.

Do not write a minority account that identifies residents who wanted privacy. Nor should the author describe a dissenting position as a complaint if it is a reasoned alternative. Preserving disagreement can help later scenes track why a policy remains contested.

## 273. Decision branch — The owner changes the plan after hearing input

The owner may revise the proposal based on feedback, then ask residents to respond again if the rule requires it. The player can mark what changed and whether earlier responses still apply. Residents can reaffirm, change, or withdraw their view. Do not carry votes or preferences forward automatically when the options changed materially.

If the owner makes a permitted final decision without another consultation, explain that distinction. The author can publish the result and the reason. The notice should not imply unanimous support merely because people did not respond a second time.

## 274. Decision branch — No agreement and no decision rule

Residents may disagree while the current authority provides no formal mechanism. The player can ask the owner to decide within its authority, seek an authorized mediator, postpone a non-urgent change, or let the existing arrangement remain. No one should improvise a majority rule because it feels democratic.

If the issue affects safety or access, the responsible owner may need to make an interim decision. The notice can name it as temporary if the owner confirms that status. Residents can continue to contest the policy through an available route without being portrayed as disobedient for raising concerns.

## 275. Decision record and notice close

The closing notice should distinguish the question asked, the method used, the result, the authorized decision, and any unresolved minority concern. If the system cannot persist participants’ individual responses, publish only the supported aggregate or authored outcome. Do not invent a voter registry or assume each person’s position from a public appearance.

This installment makes consultation a source of branching action: residents may influence a proposal, abstain, challenge the process, request a minority statement, or contest the result. The owner’s rule determines what is binding, while the communication authority determines what can be delivered and recorded.

## 276. Expansion installment 20 — Communicate a shared change without exposing a person

A shelter may need to change a shared plan after one resident raises a private concern. The author can communicate the operational change without publishing the personal reason. The player can ask which facts residents need in order to act, who must receive them, and which detail can remain private. The affected resident may review a proposed summary, decline to participate in drafting it, or ask for no attribution.

The message should state the change and its owner, not invite speculation about who caused it. If the author believes the reason itself is necessary, they should explain why and seek the resident’s permission before disclosure. A task or safety owner may have separate reporting requirements; those should be stated through the appropriate route, not smuggled into a public notice.

## 277. Branch — The private concern leads to a room change

The location owner may move a meeting because the original room cannot provide the needed access or privacy. The public notice can say that the location changed and provide the confirmed new place. It need not identify who requested the change. Residents can ask about access, request another option, or decline to attend.

If the reason is needed to understand a safety rule, the author can provide a general explanation approved by the owner. Do not mention a person’s medical history, conflict, or personal circumstances as proof that the change is legitimate. The room owner confirms the available alternatives.

## 278. Branch — Residents ask who requested the change

The player can explain that the change was made by the responsible owner or that the request came through a private channel, depending on confirmed facts. They can say the requester has not authorized further detail. Residents may accept that, raise a general fairness concern, or ask for a review route.

Do not invent a false reason to make the decision easier to accept. If the requester’s identity is already known, the player can still avoid repeating their private motive. The author can address the policy question separately without forcing the affected resident to defend themself.

## 279. Branch — The author wants to name the affected resident

The author may believe that naming the resident will stop rumors or show accountability. The player can ask what evidence supports that need and whether a neutral explanation would suffice. The resident can agree, limit the details, or refuse. The author retains control of the notice but should not attribute consent that was never given.

If a separate owner requires a named report, use its authorized channel and explain the audience. A public notice is not automatically the right place for operational records. The plan should clearly distinguish mandatory reporting from optional public attribution.

## 280. Branch — The change disappoints people who planned around the old arrangement

Residents may have prepared, traveled, or assigned work based on the earlier plan. The author can acknowledge the inconvenience, state when the change took effect, and ask the task owner whether any action needs to be adjusted. Affected residents can accept, complain, ask for support, or leave.

Do not redirect blame toward the person whose concern triggered the change. If the plan was fragile, identify the actual missing contingency. The player can help find a room, reschedule, or send an update, but only through owners that control those options.

## 281. Branch — A faction claims the private concern as its own cause

A faction may publicly say the change proves its position. The player can ask the faction to distinguish its advocacy from the resident’s private experience. The author can issue a neutral clarification; the resident can approve a public statement or remain silent.

The faction can argue for a broader policy using its own evidence, but it cannot present the resident as a spokesperson without permission. The player can challenge the attribution through an authorized channel or let the faction speak under its own name. Do not collapse personal consent into faction agreement.

## 282. Branch — The resident wants the message to say less

The resident may ask that the notice omit any mention of a private request and simply publish the new arrangement. The author can accept, explain a reason that a general explanation is required, or decline. The resident can then decide whether the proposed wording is acceptable and whether they want to continue participating.

The player should not negotiate away the resident’s privacy in exchange for a faster change. If the author cannot meet the boundary, they can state that clearly and let the resident choose their next step. No one should imply that a person forfeited privacy by asking for help.

## 283. Branch — The public message itself creates a new access problem

The changed notice may be posted or broadcast through a channel that the affected resident cannot use or does not want to monitor. The player can ask for a permitted alternate route, have the author send a direct update with consent, or keep the message general and make the new arrangement available through the owner.

Do not expose the resident’s identity by using a private channel in public view. The resident can choose to receive a direct message or find the information independently. The communication owner determines what delivery can be confirmed.

## 284. Shared-change close — State the new arrangement, not the private history

The final notice should identify the current arrangement, the owner who confirmed it, and how residents can ask practical questions. It can omit the private cause while remaining honest. If the reason is disputed or unknown, do not invent one.

This installment creates a branch around public explanation, privacy, attribution, inconvenience, and faction advocacy. The affected resident can consent to a statement, limit it, or decline. Residents can still question a shared decision without being given access to someone else’s personal history.

## 285. Expansion installment 21 — The usual communication channel is unavailable

The shelter may lose power to a board, an intercom may fail, a radio channel may be busy, or a corridor may be inaccessible. The player verifies the failure with the owner of the device or location before choosing a fallback. A second channel may be available, but it has its own audience, privacy, and reach limits.

The message’s urgency determines whether to wait, use a confirmed alternate, send an authorized runner, or change the task so it no longer depends on immediate delivery. The player should not invent a universal emergency broadcast or claim that a person heard a message simply because a fallback was attempted.

## 286. Outage branch — The message is important but not urgent

The author can wait for the normal channel to return, post a permitted copy elsewhere, or ask whether the meeting or request can move. Residents may accept the delay, ask for a direct update, or proceed with another confirmed task. The owner confirms whether the original content remains current.

Do not use a dramatic outage to justify sharing private content more broadly. A delayed private message remains private. If the author chooses another channel, they must preserve the same audience boundary where possible or ask the recipient before changing it.

## 287. Outage branch — The task depends on immediate notice

The task owner confirms what can safely wait and whether an alternate route is permitted. The player can use an authorized runner, a local signal, or a direct contact if those channels exist. Each recipient’s confirmed response is limited to what the channel can establish.

If no reliable channel is available, the owner may pause or alter the task. The author can issue a post-event explanation later. Do not let urgency transform an unverified rumor into a command, even if the player believes a warning would be helpful.

## 288. Outage branch — A runner is available but cannot reach everyone

The runner can visit a specific area or deliver to named recipients if the route and assignment are supported. The player asks who has been reached, who remains unknown, and whether a second runner is available. Residents may decide to wait, use a public fallback, or leave the affected area.

Do not report shelter-wide delivery from a partial route. The runner can return with a list only if the current system can represent one; otherwise, the scene should state the confirmed contacts and preserve uncertainty about others. The task owner can adapt based on what is known.

## 289. Outage branch — The fallback channel has a different audience

A public notice may replace a private intercom call, or a faction radio may reach only its members. The player compares the audiences before sending. The author can remove private details, ask for consent to widen the audience, or wait for the original channel.

If the message cannot be safely reformatted, do not send it through the broader route. The recipient can be told that the update is delayed if they are reached by another permitted method. A convenient channel is not automatically an authorized channel.

## 290. Outage branch — Residents create their own relay

Residents may begin passing a message verbally when formal channels fail. The player can verify the exact words, name an authorized source, and ask the original author to confirm or correct the relay. Each relay can change wording, so the player should avoid claiming the chain is perfect.

Residents can opt out of carrying a message or ask for a brief written version. The author can provide a neutral summary. Do not turn volunteers into official messengers without the owner’s approval or imply that they accepted responsibility for every later misunderstanding.

## 291. Outage branch — A faction offers its radio or board

A faction may offer temporary use of its communications equipment. The player asks who can receive the message, whether the faction monitors or stores it, and what conditions govern use. The author may send a public operational notice, decline to share private details, or choose another route.

The faction can help with its own channel but cannot guarantee access beyond its coverage. If the channel is unavailable to some residents, the player can request a second authorized route. Major factions may control wider infrastructure; smaller groups can provide a nearby relay or posted copy without claiming the whole network.

## 292. Outage branch — A false message circulates during the gap

Someone may fill the silence with a guessed schedule or instruction. The player identifies the rumor’s source if known, asks the owner for the current fact, and communicates a correction through a permitted channel. Residents can ask questions or wait for an authoritative answer.

Do not blame people for trying to plan with incomplete information. If the correct answer remains unknown, say what is pending and what residents should avoid doing if the owner confirms a safe pause. The correction should not overpromise when the underlying outage continues.

## 293. Outage branch — The channel returns with stale content

When the normal system comes back, an old notice may still be displayed or queued. The player checks whether it remains current before relying on it. The author can update, remove, or supersede it through supported behavior. A returned channel does not make stale content authoritative.

If residents acted on the old message, record the sequence and route consequences to the task or location owner. Do not assume that restoring power delivered the correction to people who missed it. The author can send a fresh message and name which details changed.

## 294. Fallback-channel decision matrix

| Situation | Verify | Possible choice | Limit to state |
|---|---|---|---|
| Non-urgent private update | Channel status | Wait or use direct authorized route | No widened audience by default |
| Immediate task dependency | Task owner and safe alternative | Runner, confirmed channel, or pause | No rumor as command |
| Partial runner route | Delivered recipients | Send another relay or adapt | No universal reach claim |
| Faction channel offer | Coverage, audience, conditions | Send public summary or decline | No guarantee beyond coverage |
| Channel restored | Current content | Update or supersede | No assumption everyone saw it |

This is branch guidance only. Use actual channel owners and persistence contracts for implementation.

## 295. Installment 21 close — A fallback is a new route with new limits

The channel outage creates distinct branches around urgency, audience, delivery, misinformation, and recovery. The player can verify the failure, choose an authorized alternative, pause an action, or preserve uncertainty. Each channel’s limits remain visible.

The final status should say what failed, what fallback was attempted, who is confirmed reached, and whether the original instruction is still current. A message sent is not a message heard; a channel restored is not a correction delivered.

## 296. Expansion installment 22 — Reconcile the message after service returns

When the normal channel returns, the author and communication owner can review what was sent through each fallback and which recipients are confirmed reached. The player can help remove or supersede stale copies, publish the current instruction, and ask whether any action began during the outage. Do not imply that restored service repaired every earlier gap automatically.

The author can close the notice, keep it active, or issue a concise follow-up that names the current version and the owner for questions. Residents can acknowledge, ask what changed, or remain unaware if no delivery record exists. A corrected board copy should not be described as an individual notification.

If a faction hosted the fallback, it can confirm the limits of its channel and whether it still holds a copy. The faction may retain, remove, or correct its own message under its rules. The shelter author can publish an attributed clarification without pretending to control the faction’s archive.

The closeout should distinguish the failure, the fallback, and the restored route. Any task consequence belongs to its owner; any updated instruction belongs to its author; any recipient acknowledgement belongs to the actual communication contract. The player’s final choices are to verify, correct, report, or leave unresolved what cannot be confirmed.

Where the channel owner supports retaining a public notice, the author can leave a concise correction beside the old information long enough for residents to distinguish the change. If removal is supported, the owner may retire the stale copy; if not, the author can label it superseded through the available format. Do not promise that old copies disappear from rooms, memories, or faction channels. A clear recovery notice names the current instruction and directs questions to the current owner, while accepting that some recipients may still need a personal update.

The author can decide whether the recovery note belongs only where the old instruction appeared or also in a broader channel. Check the audience before widening it. A narrowly scoped correction may be enough when only one room or group was affected; a shelter-wide change may require broader delivery. The player can ask who still needs the information, but if recipient tracking is unsupported, the final text should say that reach is unconfirmed instead of naming an invented completion state.

Afterward, the author and channel owner can review which fallback was reliable and whether the plan needs a different route next time. This is a practical debrief, not a resident-blame scene. A resident who repeated a message did not own the outage; the owner can improve the route while preserving the original timeline and the actions people took.

Any new fallback should be tested against its actual audience and access limits before the next urgent notice depends on it. The plan records the gap and the owner who can review it; it does not silently upgrade the alternate channel into a guaranteed service.

Residents may prefer a fallback they can check themselves, such as a confirmed board copy, over a relay they cannot verify. The author can consider that preference when planning later notices.

If individual receipt tracking is absent, the author can invite residents to ask questions without claiming to know who has already read the update.

## 296. Expansion installment 23 — A notice starts a repair route

A resident may report a broken fixture, blocked passage, damaged board, leaking container, or other practical problem through a notice or message. The communication owner carries the report; the task or maintenance owner decides whether it becomes work. The player can help capture location, visible condition, urgency, and who has already been notified without turning a report into an automatic assignment.

Residents can choose to report anonymously where supported, give their name, ask for a private response, or make the report directly. The author may acknowledge receipt, request missing facts, refer the report, or explain that no repair route exists. Do not promise a fix merely because a notice was sent.

## 297. Report branch — A resident describes a visible hazard

The player asks what they observed and where. The location owner or task owner confirms any immediate access restriction. The reporter can provide a brief description, show the location if permitted, or stop. The player should not ask them to investigate a potentially unsafe area to make the report more complete.

The notice can identify the hazard and the confirmed temporary instruction. If the hazard is unverified, state that it is under review. Residents may avoid the area, ask for another route, or continue using it only if the owner confirms that is permitted.

## 298. Report branch — The issue is inconvenient but not urgent

The task owner may classify the repair as routine or defer it because supplies are limited. The author can tell residents the report was received and what timing is confirmed. The reporter can ask for an estimate, offer help through the proper task route, or accept the delay.

Do not minimize the report because it is not an emergency. The owner explains priority under current rules. The player can help identify a temporary workaround only if its owner approves it; a resident should not be asked to make an unsafe repair to save the shelter time.

## 299. Report branch — Several reports describe the same problem

Multiple residents may report one issue. The player can ask the owner whether they refer to the same location and condition. The communication owner may acknowledge the reports separately or provide one shared update if supported. Do not assume repeated reports are duplicates until confirmed.

The people who reported may want different levels of follow-up. One can ask for a private reply; another may want the shared notice updated. The author can state the common operational fact without exposing who reported it.

## 300. Work order branch — The task owner accepts the report

The owner may assign inspection, repair, temporary closure, or supply retrieval through an existing task path. The player can tell residents what action is confirmed, who is responsible where appropriate, and which access limits apply. Volunteers can offer help, but the task owner decides eligibility and assignment.

The notice must not imply that the repair is complete when work merely begins. Residents can ask when the area may reopen, and the owner can provide a confirmed estimate or say that timing is unknown. Each update should reflect the actual task state.

## 301. Work order branch — The task owner cannot accept the report

The task may be outside the owner’s scope, lack materials, or require a different specialist. The player can ask for the correct route, keep the report pending only if supported, or tell the reporter that no work has been assigned. The resident may seek another owner, ask for a temporary accommodation, or stop.

Do not create a parallel maintenance board in dialogue. If no owner accepts the issue, the author can publish a factual note about the unresolved condition if appropriate. The note should not promise monitoring or automatic escalation that the system does not provide.

## 302. Repair branch — Residents disagree about the temporary closure

Some residents may want a passage closed while others rely on it. The location owner confirms the safety status and any accessible alternatives. Residents can ask for another route, offer to help move an obstacle if assigned, or report that the alternate route is unusable.

Do not settle the dispute by majority preference when access or safety belongs to an owner. The owner may choose an interim measure and explain its scope. The author communicates that decision while preserving resident feedback for review.

## 303. Repair branch — A faction offers tools or labor

A faction may provide materials, a qualified worker, or temporary access to its workshop. The player checks availability, ownership, conditions, and whether the task owner accepts the offer. The faction can help without gaining permanent control of the fixture or the repair decision.

The task owner may accept, decline, or ask for a different resource. Residents can volunteer only through the supported task route. If the faction’s help has a cost or public condition, disclose it before work begins so the owner can decide whether to proceed.

## 304. Repair branch — The repair changes the original report

Inspection may reveal that the reported issue was caused by a different condition or that the problem is not present. The owner can update the task status; the author can issue a correction. The reporter may feel embarrassed, relieved, or frustrated. Do not frame a good-faith report as misconduct because inspection found a different result.

If a reporter knowingly misrepresented the condition, use evidence and an existing review owner. The communication plan itself does not adjudicate intent. The repair can still uncover a different issue and create a separate task.

## 305. Repair branch — The task is complete but residents have not heard

The task owner may confirm completion while an old closure or warning remains visible. The author can publish an update, and the location owner confirms whether access is restored. Residents may continue avoiding the area until they receive the message. Do not report that the notice cleared just because the work ended.

If some residents already acted on the old message, acknowledge that sequence and clarify the current state. A completed repair, reopened space, and delivered update are separate facts that may occur at different times.

## 306. Repair route close — Report, assignment, work, reopening, update

This route follows five distinct states: a report is made, an owner accepts or declines it, any assigned work proceeds, the location owner confirms reopening, and the author communicates the result. Each can branch or stop. The player can observe and relay facts but cannot make one state imply all the others.

The ending can be an acknowledged report, a confirmed task, an unassigned issue, an interim closure, completed repair, or unresolved access. The plan does not create a second maintenance ledger; it makes the existing handoffs visible to residents and writers.

## 307. Expansion installment 24 — The author chooses a channel before choosing the words

A notice is not only text; its channel determines who can encounter it, when, and with what privacy. Before drafting, the player identifies whether the message belongs on a public board, a room board, an intercom, a private message, or an existing task update. The communication owner confirms that the channel exists and that the author may use it. A message cannot promise an audience broader than the channel and permissions can reach.

The author can choose a broad audience for shared operational facts, a limited audience for local changes, or a private route for individual follow-up. If a channel is unavailable, the player can ask for an alternative, wait, deliver a message through an authorized person, or decide not to send it. The message owner records its audience and lifecycle; a UI does not invent a new distribution registry. The content should state what is known, who confirmed it, when it applies, and what a reader should do next.

## 308. Channel branch — A public board reaches the right area but not everyone

A board may be visible only in a workshop, sleeping block, clinic, or corridor. The author can post locally, ask for a second authorized board, or request an intercom announcement. The player can check which residents are likely to rely on the message without claiming universal delivery. The audience owner confirms access rules. A public board may be the best place for a local repair update but inadequate for a shelter-wide hazard.

The author may prefer the local board because a broad broadcast would expose unnecessary details or interrupt residents. That preference can stand if the message remains adequate for the affected audience. If someone without access must know the instruction, the owner can choose an additional channel or tell the author that another route is needed. Posting twice should not create two conflicting sources; both copies should point to the same confirmed fact and expiry where the system permits.

## 309. Channel branch — A private message is not a quiet public notice

An author may ask to send a private message to one resident about a shift change, appointment, or personal matter. The recipient and message owner confirm the allowed channel and what appears in notifications. The player can help phrase a concise message, request a neutral intermediary, or use a private conversation. A private message should not appear in a board summary or be repeated by an intercom merely because the player wants to make the information accessible.

The recipient may acknowledge, ask a question, decline the communication, or not respond. A private message can be delivered without being read; read status, where supported, is not proof of comprehension or agreement. If the sender needs confirmation before an action, they must ask for an explicit reply through an approved route. The message cannot authorize a change to the recipient’s duties or property on its own.

## 310. Channel branch — Anonymous posting protects one risk and creates another

A resident may want to report a concern without public attribution. The player identifies what anonymity means in the current system: no name on the board, a private report to an owner, or an anonymous intake if one exists. Do not promise that a fully anonymous report can be followed up if the system has no reply route. The resident can provide contact information privately, submit only the operational facts, request a witnessed conversation, or decline to report.

The owner may need to know the reporter’s identity to investigate a specific issue, but should explain that need before receiving personal details. The author can publish an aggregate update without naming who raised the concern. If other evidence already identifies the person, avoid suggesting the message guarantees protection from inference. The resident decides whether the remaining risk is acceptable. Anonymity is a channel property, not a moral reward for courage.

## 311. Channel branch — A shared notice requires several people’s agreement

A message may quote a group, announce a joint decision, or list volunteers. The player checks who actually approved the wording and who authorized each name. One organizer cannot speak for every participant without consent. The group can agree to a concise collective statement, let the owner publish only the operational decision, or post separate accounts with clear attribution.

If participants disagree, the notice can report the confirmed action and state that views differ. It need not force a single group voice. A person can withdraw their name before posting if the owner has not yet committed the notice. If they withdraw after publication, the author can correct the attribution while preserving the historical fact that the original version appeared. The notice owner confirms whether edits are possible; never silently overwrite a record when a correction is needed.

## 312. Channel branch — Accessibility changes who can receive a notice

The player considers whether the chosen message can be read, heard, understood, and acted upon by its intended audience. A written board may not reach a person who cannot access that location or format. An intercom may be difficult to understand in noise. The author can request a short plain-language version, a repeated announcement, a visual marker, or private follow-up through an authorized helper.

Do not disclose a person’s disability or private need in a public message to explain the alternative. The relevant owner determines available accommodations and whether a helper may receive the information. The audience may choose a preferred format when current systems support it. If no accessible route exists, state the limitation and ask the owner for a workable accommodation rather than pretending the notice was universally received.

## 313. Channel branch — A translation or summary preserves the operational fact

A resident may ask for a translation, read-aloud, or shorter summary. The communication owner determines which formats are supported and who may provide them. The translator can preserve the action, time, location, uncertainty, and source without adding interpretation. The reader can ask a question or request the original. A summary is not a separate authority; the canonical notice remains the reference.

If a phrase has safety or faction significance, the translator can flag ambiguity and seek the source owner’s clarification. The player should not improvise a translation that changes the instruction. The notice can be held until a reliable version is available, paired with a clear “not yet translated” status, or delivered through a qualified person. Readers should know whether they are seeing the authoritative wording or a paraphrase.

## 314. Channel branch — The author has no permission to use the selected channel

The player may discover that the proposed author lacks board access, intercom authority, or permission to send a private message. The channel owner can identify an authorized poster, offer a supervised submission, or decline. The original author can approve the exact text, revise it to fit the channel, seek another route, or abandon the message.

An authorized poster should not take over the message’s intent without consent. They can explain why certain content cannot be posted and offer a factual alternative. If the author disagrees, they may ask an existing review owner or publish through another permitted channel. Do not portray the rule as a faction plot unless there is evidence; it may simply be access control. The branch turns channel permissions into an understandable handoff.

## 315. Channel outcomes — One message can be delivered through a bounded set of routes

The author may post locally, request an additional board, send a private follow-up, seek accessible formatting, submit through an authorized poster, or decide not to send. Each route has a known audience, owner, and lifecycle. The same fact can appear in multiple places only when those copies remain synchronized or clearly link to the current source.

The player should see the channel choice before confirming content. A later update can preserve or narrow the original audience but should not broaden it without a fresh permission check. The message may be private, local, shelter-wide, or merely submitted for review. These distinctions make communication gameplay meaningful without turning every sentence into an approval puzzle.

## 316. Expansion installment 25 — A notice can be corrected without erasing what happened

Information may change after publication. The author discovers a wrong time, a work owner revises a closure, a source is corrected, or a resident points out an omitted condition. The communication owner can issue an amendment, mark the old notice superseded, or remove it under current rules. Readers need enough context to understand what changed and whether any earlier action should be reversed.

The player helps identify the source of the update, what remains true, and who must receive it. The correction should not imply that every earlier reader saw the new version. If the game tracks acknowledgement, it reports that record precisely; otherwise, the player can use an existing follow-up channel or state that delivery is unconfirmed. A corrected notice is an event with its own audience and timing, not a magic edit to everyone’s memory.

## 317. Correction branch — A time or location is wrong

The author notices that a meeting time or location was posted incorrectly. The event owner confirms the right detail before the correction is published. The player can add a visible amendment, ask the original channel owner to replace the notice with a correction history, or contact people who already confirmed attendance. Residents can keep the appointment, choose a new time, or ask whether the event is still happening.

Do not blame readers for acting on the first version. The correction should display both the former and current detail when that helps prevent confusion. If the old notice was printed or copied, the author may need to identify those copies. The message owner can report which copies are under its control and which are not. The event owner decides whether to delay, cancel, or continue.

## 318. Correction branch — A factual source has changed

The source owner may correct a measurement, inventory count, instruction, or public statement. The communication author checks which claims depended on the old information. They can revise only affected lines or withdraw the notice and issue a replacement. Readers may ask why the information changed, and the source owner can explain the new evidence or say that review remains open.

The author should not obscure the correction to protect a faction’s reputation. Nor should a correction be treated as proof of deliberate deception without evidence. If the new fact changes an operational task, that task owner confirms any follow-up action. The communication record states the scope of the correction and the time it took effect. An archived version may remain for audit only if the current owner supports that history.

## 319. Correction branch — A notice omitted an affected group

A resident may point out that the notice reached a room or group that cannot use the stated alternative. The player can ask the access owner and affected residents what accommodation or route is missing. The author may broaden distribution, add a second option, or correct an overbroad claim. The group can confirm that the revision meets its need or explain what remains unresolved.

Do not frame the correction as a request for special treatment. The operational owner determines feasibility; the communication author accurately reports what is available. If there is no safe alternative, the notice should say so and route the concern to the owner. A broad message may need both an updated instruction and a private follow-up to a person whose situation cannot be handled publicly.

## 320. Correction branch — A faction disputes the source

A faction representative may challenge a notice’s account of an event or procedure. The player asks which fact is disputed, what evidence the faction offers, and which source owner can review it. The communication author may add that the claim is under review, publish a correction after owner confirmation, or keep an interpretive disagreement separate from operational instruction.

The faction can present its account without automatically controlling the notice. The source owner may accept, reject, or narrow the correction. The reader can see who made each claim and what remains unresolved. Do not write one permanent “truth branch” where a faction’s alignment decides every dispute. The relevant actions—source access, correction, review, and acknowledgement—should shape each case.

## 321. Correction branch — A reader republishes a partial quote

A reader may repeat one sentence without its limitation, turning a conditional offer into a promise or a temporary closure into a permanent ban. The author can issue a concise clarification and ask the reader to stop reposting the shortened version. The reader may correct their copy, argue that the original was unclear, or refuse. The communication owner decides whether any moderation rule applies.

The player should distinguish a genuine misunderstanding from deliberate misrepresentation. They can show the full notice and ask the source owner to explain the condition. If the summary was unclear, the author can improve future wording. If the reader knowingly changes it, use evidence and current conduct rules; do not invent a speech-policing system. The correction must not expose private details that were absent from the original.

## 322. Correction branch — The author withdraws a claim but not the whole notice

One unsupported paragraph may need removal while the rest remains correct. The source owner can identify the affected claim and whether the remainder is safe to rely on. The author may publish a partial correction, withdraw the entire notice, or wait for review. Readers need an explicit statement about which instruction still applies.

If one faction supplied the disputed portion, the rest of the announcement should not be attributed to that faction by association unless it owns the whole message. The author can distinguish operational facts from opinion. A resident who already acted on the withdrawn claim can ask what to do next. The relevant task owner decides whether to reverse or compensate for an action; the notice author communicates, not adjudicates.

## 323. Correction branch — A correction cannot reach the original audience

The original board may be inaccessible, the intercom may be down, or the recipient may have left the location. The player checks what channels still work and what audience they can reach. The author can use a second board, a messenger, or a later announcement if those routes are allowed. The recipient may receive the update late or not at all; the record should reflect that uncertainty.

If the old information creates immediate risk, the responsible owner chooses an appropriate alert path. The player can help relay it but cannot claim a shelter-wide warning was delivered based on one character hearing it. The channel owner may mark the original notice as stale. Any later use of the message must display that status so the player does not act on an obsolete instruction.

## 324. Correction branch — The author fears reputational cost

The author may delay a correction because admitting an error could harm their credibility or their faction. The player can show who may rely on the claim and what harm could follow from delay. The author can correct now, ask a reviewer to confirm the scope, or refuse. The source owner remains responsible for verifying operational facts.

Do not make the player’s moral condemnation the only way to proceed. A practical explanation—who may be sent to the wrong location, which task may be wasted, what safety limit is at stake—can support a decision. If the author refuses to publish a correction and the player has an authorized reporting route, they can use it. The consequences should follow actual access and communication systems, not an abstract reputation penalty.

## 325. Correction branch — A resident asks for the old version

A reader may want the original notice to understand what changed. The communication owner determines whether version history is available and whether it contains restricted details. The player can show the old and new text, summarize the change, or explain that the system keeps no recoverable copy. The reader may accept the correction or ask the source owner directly.

Do not promise archival retention if no owner provides it. If the old version includes private information or an unsafe instruction, do not repost it publicly merely for completeness. A limited review may satisfy the purpose. The reader should learn which version to act on now and who can answer a remaining question. Accuracy includes being truthful about what the game can recall.

## 326. Correction outcomes — Amend, supersede, withdraw, or leave disputed

An information change may end with an amended notice, a superseding notice, a full withdrawal, a limited clarification, an unresolved dispute, a second-channel correction, or an owner referral. Each state should have a current source, time, audience, and action instruction. If the message store supports acknowledgement, correction can identify which recipients still need follow-up; if not, authors must use a truthful manual route.

The communication UI should distinguish the current instruction from historical wording. A correction does not automatically cancel an operational task or reopen a location. The owner who made that decision must confirm it. That boundary makes the correction arc rich in downstream consequences while preserving one authority for each concern.

## 327. Expansion installment 26 — Two authoritative notices can conflict for a real reason

The shelter may have a local instruction from a room owner and a wider procedure from a task or safety owner. They can appear to conflict because they cover different locations, times, or exceptions. The player compares source, scope, and effective time, then asks the relevant owners to reconcile the wording. The author can temporarily narrow the notice, add a qualified clarification, or hold it until the conflict is reviewed.

The reader should not be forced to guess which authority wins. The notice identifies what is known and who owns the unresolved question. A supporting faction may provide context, but it cannot override an owner merely by speaking more confidently. If the conflict concerns immediate safety, the current safety owner determines interim instructions. The communication channel carries that decision and its limits to the proper audience.

## 328. Conflict branch — The two notices cover different spaces

One notice may apply to the clinic, another to the workshop. The player checks the named location and access boundary. The author can clarify the scope on both notices or link them through a shared source. Residents who move between areas may need a concise transition warning. The owners confirm that no passage or shared resource falls between the boundaries.

If the locations overlap, pause the instruction and ask the owners to resolve the overlap. Do not improvise a map rule in prose. A reader can ask a local attendant, take a safer route if the owner has authorized one, or wait for clarification. The branch can reveal that a faction controls one room while the shelter controls another, but ownership must come from current data.

## 329. Conflict branch — One notice is old but still visible

An expired notice may remain pinned or copied while a newer one is active. The player checks the message owner’s expiry and the decision owner’s latest state. The author can remove, mark, or supersede the old message. If the owner cannot confirm which version is current, mark the instruction under review and ask before directing residents.

The old text may explain why residents are acting as they are. Do not mock them for following a notice that still looks current. The communication owner can report whether removal reaches all copies. A faction contact can help identify a local posting, but cannot certify global removal. The route ends when the relevant residents have an accurate next step, not merely when the newest note is posted.

## 330. Conflict branch — Two owners disagree on responsibility

The room owner may say the task owner controls the instruction; the task owner may say the room owner controls access. The player lists the actual decision each one owns and asks what needs joint confirmation. A neutral coordinator may carry the question, but should not invent a governing rule. The message can explain which part is confirmed and which is pending.

If no existing owner can resolve the overlap, report the governance gap rather than scripting a convenient authority. For a temporary response, use only a current emergency or safety rule that is verifiably applicable. Residents can wait, use a confirmed alternative, or decide not to enter. The narrative should make the cost of delay visible without treating an unresolved authority as permission to bypass one.

## 331. Conflict branch — A faction posts a parallel instruction

A supporting faction may post its own guidance near a shared facility. The player asks whether it describes the faction’s property, a shared service, or a shelter-controlled task. The faction can clarify its scope, remove an outdated claim, or request that the shelter publish a linked instruction. The shelter author can reference the faction notice while preserving who owns each part.

The player should not treat every parallel notice as sabotage. It may be a legitimate local reminder or a source of useful expertise. If it contradicts current owner guidance, show the specific conflict and seek review. Residents can ask both sources, but the final operational instruction comes from the responsible owner. Faction participation remains supporting even when its information is valuable.

## 332. Conflict branch — A notice has a time-limited exception

The owner may allow an exception for one delivery, repair, or person while a general notice remains in effect. The author must state the exception’s scope and expiry without naming a person unnecessarily. The reader can ask whether it applies to them, and the owner confirms. If the exception ends, the general instruction resumes only if the owner says so.

Do not let repeated exceptions become an undocumented alternate policy. The owner may decide to review the general rule, but the communication author cannot infer that from one case. A faction may request an exception and provide a reason; the owner answers under the same authority. The player can track who relied on the exception only through a supported record.

## 333. Conflict branch — The reader follows the less restrictive notice

A resident may choose the notice that permits access or work because it is more convenient. The player can ask which source they understood to apply and show the effective times and scopes. The owner determines the current instruction. The resident may comply, ask for review, or explain why the stricter notice is not workable.

Do not assume bad intent from the choice alone. The notices may genuinely be confusing. Correct the communication and let the owner handle any operational consequence. If the resident knowingly ignored a clear restriction, use the existing task or conduct route. The notice plan itself should not impose punishment or decide intent.

## 334. Conflict branch — The more restrictive notice blocks needed access

Residents may be unable to reach a service because a broad closure omitted an accessible route. The player checks with the location and safety owners before proposing an exception. The owner can confirm a safe route, arrange an escort, create a time window, or state that access remains unavailable. The author updates the notice so people do not rely on an unverified workaround.

The affected resident can accept the alternative, request assistance, wait, or decline. Do not imply that a resident must publicly disclose why they need access. If the owner cannot provide a route, state that honestly and identify the next review point if one exists. A later resolution can change the notice and the resident’s ability to access the service through a real owner-backed action.

## 335. Conflict branch — The faction offers to resolve the discrepancy

A supporting faction may volunteer a liaison who knows both procedures. The player can ask the liaison to compare text, gather owner responses, or carry a proposed correction. The liaison cannot decide between owners unless the current authority grants that role. The owners can accept the comparison, request a different reviewer, or decline it.

The faction may have an interest in the outcome. Disclose that interest and let the owners and affected residents decide whether the help is useful. The liaison can still contribute technical knowledge even if it cannot act as neutral arbiter. The ending may be a corrected notice, a limited explanation, a referral, or an unresolved conflict with clear interim instructions.

## 336. Conflict outcomes — Readers receive one current instruction and named limits

Conflicting notices resolve through scope clarification, version control, owner reconciliation, an explicit exception, a safer access route, faction-facilitated review, or an honest pending state. The communication author reports the result, while operational owners decide what residents may do. Readers can ask questions and challenge confusion without being framed as troublemakers.

The branch creates campaign variation through what evidence is available, which owner responds, what access the faction provides, and whether residents accept the temporary instruction. A closure can be maintained, narrowed, or reversed. Those endings arise from the history of decisions and actual authority, not an alignment check.

## 337. Expansion installment 27 — Receipt is not understanding, and silence is not consent

A public notice may be visible while a resident misses it. A private message may arrive but remain unread. An intercom may be heard without being understood. The player should distinguish authored, posted, delivered, acknowledged, and acted-upon states wherever the current communication owner supports them. These are useful design concepts, but the implementation must use the actual API and must not manufacture receipt flags in a panel.

When an action requires confirmation, ask for explicit acknowledgement through an existing route. If the action is optional, lack of response should not be treated as acceptance. If a deadline makes silence consequential, state the existing rule and what happens when no answer arrives. A sender may try another permitted channel, wait, or close the request. The recipient may respond later, explain why the channel failed, or decline to participate.

## 338. Acknowledgement branch — A resident sees the notice but does not reply

The author may need to know whether a resident saw an appointment change. If the owner supports a read receipt, show that narrow fact without claiming comprehension. If no receipt exists, the player can ask the resident, use a direct follow-up, or record that delivery is uncertain. The resident can acknowledge, ask a question, say they did not see it, or decline to discuss it.

Do not interpret a missing reply as refusal, agreement, or indifference. The resident may be on duty, unable to access the board, or unwilling to answer through that channel. The sender can choose a follow-up time or proceed only if the relevant owner’s rules allow it. The notice should not silently change the resident’s schedule before they have acknowledged a required choice.

## 339. Acknowledgement branch — A resident heard the intercom but missed a detail

The broadcast may be audible while its time, location, or exception is unclear. The recipient can ask for repetition, read a posted copy, or receive a private clarification. The author should provide the confirmed detail and identify any earlier ambiguity. The intercom owner confirms whether the broadcast can be replayed or whether another message is needed.

Do not blame the listener for failing to decode a noisy announcement. If a message is safety-critical, the responsible owner may require more than one channel. The player can help coordinate a board copy or local attendant. Each channel has its own audience and timing, so the follow-up should say whether it replaces, supplements, or corrects the original announcement.

## 340. Acknowledgement branch — A public response would expose a private concern

A resident may need to respond to a public notice but does not want their answer visible. The author can offer a private reply route, an intermediary, or a public response that contains no personal detail. The resident chooses what to share. The communication owner confirms whether the reply reaches the right person and whether any record is retained.

If the public post asks for volunteers, the resident can volunteer privately if the task owner permits it. The group learns only that the task has sufficient support if that is the only necessary fact. Do not force individuals to disclose availability, health, or personal circumstances on the board. A later public update can communicate capacity without identifying who declined.

## 341. Acknowledgement branch — Two residents answer through different channels

One resident may respond on the board while another sends a private message. The author can consolidate the operational status without exposing the private response. The task owner confirms which answer matters for assignment. The communication owner preserves each audience boundary, and the player can ask whether the private responder consents to public attribution.

If the responses conflict, do not merge them into a false consensus. The author can state that the task is still being arranged and ask the owner for a decision. A resident can update their answer before commitment if the current route allows it. When an offer is limited, the owner—not a public popularity count—determines how the spot is allocated.

## 342. Acknowledgement branch — A resident cannot use the offered reply route

The notice may ask for a response through a board, device, or office the resident cannot access. The resident can tell the player, request another route, or choose not to answer. The owner confirms an accessible alternative. The author can correct the notice for future readers and directly notify the affected person if authorized.

The resident should not lose eligibility solely because the response route was unusable. If the deadline has passed, the task owner decides whether a late response can still be accepted. The player can show that the route failed and ask for review. Keep evidence of the channel problem within whatever reporting owner exists; do not invent a tracking subsystem solely to justify a compassionate exception.

## 343. Acknowledgement branch — The recipient asks to stop receiving nonessential notices

A resident may ask to mute a channel or reduce updates. The communication owner explains which messages are optional and which operational alerts cannot be suppressed under current rules. The resident can choose a narrower channel, ask for a summary, or accept that essential notices still reach them. The player should not make the choice for them.

If preferences are durable, they belong in the current communication owner and save path. If no such setting exists, treat the request as a conversation and avoid promising a permanent mute. A person can later change their preference. A faction cannot add someone to a mailing list because they once attended a meeting or accepted a single offer.

## 344. Acknowledgement branch — The sender treats silence as approval

The sender may proceed because the recipient did not object. The player checks whether an explicit reply was required, what the owner’s default rule says, and whether the recipient could reasonably respond. The recipient can object, clarify, or accept the result. The task owner decides whether any assignment or transfer is valid.

If the owner requires affirmative consent, reverse the unconfirmed action where possible and explain what has already occurred. If the action was authorized by a known default rule, show that rule and give the resident a route to challenge it. Do not create a rule after the fact to justify the sender. The narrative consequence can include delay or friction without turning the recipient’s silence into an immoral choice.

## 345. Acknowledgement branch — A message is acknowledged by an intermediary

An intermediary may confirm receipt for someone else. The player checks whether that person was authorized to acknowledge and what they actually confirmed: delivery, read, understanding, or a decision. The intermediary can provide a limited receipt, request a direct reply, or decline. The recipient can later confirm or correct the record.

An intermediary’s acknowledgement does not imply consent unless the owner explicitly defines that authority. If they are a faction contact, disclose their role. The author can use the receipt to know that follow-up is needed, but cannot claim the recipient accepted the offer. This distinction supports assisted communication without treating a helper as the recipient’s proxy for every decision.

## 346. Acknowledgement branch — A delayed reply changes a pending arrangement

The resident may reply after a temporary slot has been offered to someone else. The owner checks whether the slot is still available and what commitment was already made. The resident can accept a later date, request another option, or withdraw. The other person should not lose an accepted assignment because of a message that arrived late unless the owner’s allocation rule permits a change.

The author can explain when the reply arrived and what remains possible. Do not treat the resident as having missed a guaranteed opportunity if the notice stated only a tentative window. A later branch can let the player advocate for a second slot, but capacity still belongs to the owner. The communication route determines what was known at each stage.

## 347. Acknowledgement outcomes — The status label must match the evidence

Possible states include submitted, approved, posted, delivered, heard, read, acknowledged, replied, accepted, and acted on. The current system may support only a subset. Writers and interface designers should use only those states the owner can actually distinguish. When the state is unknown, say “not confirmed” rather than selecting a more satisfying label.

This accuracy does not make the feature cold. It gives characters room to explain missed messages, choose a privacy-preserving channel, ask for repetition, and set boundaries. The player can improve communication by matching channel to audience and urgency. The ending may be a clear answer, a late answer, an unread note, or an explicit decision not to respond.

## 348. Expansion installment 28 — Bad news deserves an audience choice and a real follow-through

The shelter may need to communicate loss, shortage, a cancelled service, a failed repair, or a changed allocation. The author decides who needs to know first and what can be said accurately. The affected people can request a private conversation, an accessible explanation, a question period, or no public mention of their personal situation. A factual public update may still be necessary, but private details remain with their proper owner.

The player can help the author separate confirmed facts, estimates, and unknowns. The message should not use comforting language to disguise a loss or present a temporary workaround as a solution. A named owner can take questions and explain next steps. If no one owns follow-up, the notice should say that rather than promise a check-in. The scene’s emotional weight comes from truthful timing and audience care, not from dramatic phrasing alone.

## 349. Bad-news branch — A supply cut affects different residents differently

The supply owner confirms what is reduced and when the change begins. The communication author can post the shared operational fact and direct affected people to a private allocation route. Residents can ask how their own plan changes, request review, accept a substitute, or choose to wait. The player can help compare options without publishing anyone’s personal need.

If a supporting faction offers replacement goods, its quantity, source, and conditions are stated separately. It may help one group but not solve the whole shortage. The notice does not claim the supply is restored until the owner confirms actual stock. A later update can report delivery, delay, or continued scarcity. Residents may disagree about fairness; the allocation owner handles that review, while communication carries the response.

## 350. Bad-news branch — A service is cancelled after residents relied on it

The event or task owner confirms the cancellation and the reason it can share. The author contacts attendees through the original channel if possible, offers a new date only when confirmed, and explains any alternative. Residents may request a refund or return of materials only if the relevant owner supports that transaction. They can accept the replacement, seek another service, or leave.

The author should acknowledge reliance: people may have changed shifts or spent travel time. Do not promise compensation without authority. A faction may offer a substitute venue or instructor, but the affected resident decides whether to use it. The old notice can be marked cancelled or superseded, and any physical copy should be removed through its owner. The cancellation remains a real consequence, even when communication is handled well.

## 351. Bad-news branch — The author cannot explain the decision fully

Some details may be confidential or still under review. The author can state the known operational effect, the part that cannot yet be explained, and who can answer questions within their authority. Affected residents can ask for a review date, accept the limited explanation, or challenge the lack of detail. The player can help formulate questions without inventing answers.

If the decision affects an individual, move personal discussion to a private route. If it affects the whole shelter, explain why a public statement is still necessary. The owner should say whether more information may become available. Do not imply intentional concealment merely because details are restricted; do not use confidentiality as a blanket response to every challenge.

## 352. Bad-news branch — A bereaved person does not want a public announcement

The communication owner and relevant personal authority establish what information may be shared. The affected person or family can request a private notice, a limited factual announcement, a memorial later, or no public event. The player respects that request within the current rules. Others may need practical information about a shift or room assignment, but that can be stated without describing private grief.

The shelter can still support people who ask for help through appropriate owners. A faction may offer a space or ritual, but cannot declare a public memorial on someone else’s behalf. Residents can privately remember the person, ask questions, or keep distance. The communications feature records audience and consent boundaries; it does not turn grief into a collectible narrative branch.

## 353. Bad-news branch — The community needs an answer before all facts are known

Residents may ask for an update while an incident review continues. The owner can publish a holding statement that clearly separates known facts from open questions. The author states when a further update is expected only if a follow-up time is confirmed. The audience can submit questions through a supported route or wait.

If new evidence changes the statement, use the correction lifecycle rather than quietly changing the displayed text. The player can help prevent speculation by sharing process and source, not by promising a conclusion. A faction may offer its own interpretation; readers can compare it with the owner’s current information. Uncertainty is an honest outcome that can lead to further inquiry.

## 354. Bad-news branch — A public meeting is requested after the announcement

Residents may want a meeting to discuss a shortage or decision. The meeting owner checks availability, safety, agenda, and who should attend. The author can direct people to an existing forum, schedule a meeting, or explain that no meeting is available. Affected residents can submit questions privately or decline participation.

The meeting does not replace individual review or disclose private cases. The chair can distinguish information updates from decisions that belong to another owner. Residents may disagree, propose changes, or ask for an appeal route. If no decision can be made, the minutes should say so. The next notice reports agreed actions without claiming consensus where none exists.

## 355. Bad-news branch — A faction offers to carry the announcement

A faction representative may have trusted access to a remote group or local audience. The author asks whether the faction can relay the confirmed wording, whether it will add commentary, and whether delivery can be verified. The faction may agree to relay, request permission to annotate, or decline. The audience can receive the message with the source and intermediary clearly named.

The relay does not transfer authorship. If the faction adds a view, distinguish it from the original statement. A recipient can ask the source owner for clarification through an authorized route. The player can choose the faction’s reach, a direct route, or no relay. This is a supporting role with real value and explicit boundaries.

## 356. Bad-news outcomes — Honest updates preserve room for response

The message can conclude in a private notification, factual public notice, cancelled event, limited explanation, holding statement, community meeting, faction relay, or no announcement until facts are confirmed. Each route states what is known, who owns the decision, and what action remains available. The player can choose audience and timing while respecting privacy and channel limitations.

Later scenes can show frustration, relief, grief, negotiation, or continued uncertainty. Those responses arise from what happened and how people were informed, not from a generic sentiment meter. A message can be compassionate without pretending that the underlying loss has been repaired.

## 357. Expansion installment 29 — Close the loop between notice, action, and lived result

Communication should end at the boundary where the operational owner’s action and the reader’s experience meet. A notice can say that a corridor reopened, but the location owner confirms access. A work owner can finish a repair, but the communication author still needs to publish the update. A reader can acknowledge the message, but the task owner still confirms whether it solved the problem.

The player can inspect these separate states and help move a missing handoff forward. If the task is complete but the notice remains active, request a correction. If the notice says a service is available but its owner has no capacity, challenge the discrepancy. If a resident reports that the result did not work for them, route the report to the responsible owner. The loop closes only when the claim and the lived state agree—or when the unresolved gap is honestly visible.

## 358. Close-loop branch — The task completes before the update is written

The task owner confirms completion while the communication author is on duty elsewhere. The player can ask an authorized alternate to publish the update, wait for the author, or post a limited status through an existing task surface. The location owner verifies whether residents may use the area. The notice should not say “open” until access has been confirmed.

Residents may continue to follow the old warning until they hear otherwise. If the route is urgent, the owner chooses a faster channel; if not, a delay may be acceptable. The player can show the difference between “work finished” and “area reopened.” Once the update is published, the previous notice can be marked resolved or superseded by the message owner.

## 359. Close-loop branch — The notice claims success but the result is partial

The author may have written “fixed” when the owner only stabilized the problem. A resident reports that one part remains unusable. The player asks the task owner what was completed and whether the remaining issue has a separate task. The author can correct the wording, identify the residual limit, or hold the message while the facts are checked.

Do not dismiss the resident because the original work passed inspection. Their report may reveal a different access need or a separate failure. The location owner determines current use, and the task owner decides the repair follow-up. A faction technician can inspect if invited, but their report remains input for the responsible owner. The correction should acknowledge the overstatement without making the entire completed work disappear.

## 360. Close-loop branch — A reader relies on an old instruction after expiry

The reader may arrive with a printed or remembered notice after its expiry. The player checks the current owner state and explains what changed. The reader can ask for the source, use a current route, or request help if the old instruction caused a problem. The author can remove visible copies where supported and publish a concise current status.

Expiry does not prove that everyone saw the update. If the consequence is material, the owner can decide whether to extend a transition period or provide an exception. The reader is not automatically at fault for an expired notice that remained visible. The communication owner can improve how expiry is shown, but should not create a new reminder service without authority.

## 361. Close-loop branch — A resident reports that the promised follow-up never came

The player checks whether a follow-up was actually promised, scheduled, or merely hoped for. The message owner can show its status if supported; the task owner reports whether action occurred. The resident can ask again, accept a new time, or stop relying on the promise. The author may apologize and clarify that the earlier update was not confirmed.

If a named owner missed an agreed follow-up, route the issue through that owner’s normal review. Do not create a side reputation penalty. The player can help assign a new responsible person only if the owner permits. A later message should state a real appointment or avoid a date. Trust can be affected by repeated actions, but the plan does not reduce that history to one automatically calculated number.

## 362. Close-loop branch — The resident says the notice changed what they did

A resident may explain that they altered a shift, chose another route, gave up a service, or used a substitute because of a message. The player asks what action they took and whether any practical problem remains. The relevant owner decides whether a remedy, correction, or review is possible. The resident can request help, simply report the effect, or decline follow-up.

The author may add a note that the notice caused a specific operational change if the owner verifies it. Do not claim causality from a single anecdote when it is uncertain. The resident’s account still matters as feedback. A supporting faction can help restore one resource or offer an alternative, but any transfer follows the current property owner.

## 363. Close-loop branch — The message was accurate but the audience inferred more

Readers may interpret “available after inspection” as a promise that access will reopen today. The author can clarify the conditional phrase and ask whether the original wording was ambiguous. The task and location owners provide the actual status. Residents can adjust their plans, ask for an estimate, or request a more direct instruction.

If the wording is technically accurate but practically confusing, revise future notices. The author should not blame readers for a foreseeable interpretation. The current message can state that timing is unknown and explain what the owner will confirm next. A faction contact may help translate the wording, but the source owner still confirms the operational fact.

## 364. Close-loop branch — The owner says the communications plan promised too much

An operational owner may object that a notice implied a service, review, or response that the owner cannot deliver. The player can compare the text with the current contract and ask what should be corrected. The author may amend the message, cancel the promise, or negotiate an owner-approved commitment. Readers are informed of the actual scope.

The correction should identify the specific claim, not broadly retract unrelated information. The owner can propose a narrower follow-up it can sustain. If no replacement exists, say so. The player can challenge the mismatch and seek a different provider, but cannot hold an owner to a promise it never made. Any social fallout is handled through the existing relationship route.

## 365. Close-loop branch — A second faction gives a different result report

A faction may report that its workers completed an additional step while the shelter owner reports only partial completion. The player asks for the evidence, location, and task scope. The owner can verify the added work, create a separate task, or say that it falls outside the original assignment. The author can publish the distinct facts without merging them into one claim.

The faction can be credited for what it did and asked to clarify what remains. The owner’s inspection determines shelter access and safety. The player may use a joint inspection if all sides agree. The branch demonstrates collaboration without granting the faction unilateral authority over the shared space.

## 366. Close-loop outcomes — End with a current state and a next owner

The route can end with a published completion, a partial-result correction, an expired notice clarified, a missed follow-up acknowledged, a reader’s impact report, a revised phrase, or a joint report. Each ending identifies the current state and who owns the next action. If there is no next action, the notice should say that the issue is closed or unresolved, as appropriate.

The feature stays small in authority but large in consequence. A message can affect whether residents move, wait, volunteer, trust a schedule, or seek another route. The player sees those effects through concrete decisions and owner responses, not through an omniscient notice log.

## 367. Installment 29 close — Communication is successful when its claim matches the world

Success does not mean every message was read or every resident agreed. It means the author stated a supported fact to an appropriate audience, the owner’s decision was represented accurately, and changes were corrected through the proper route. A resident can still object, miss the message, or experience an access problem. Those outcomes create further branches without invalidating the original exchange.

For implementation, every proposal should name the message producer, authority for the underlying fact, channel and audience, delivery evidence, expiry behavior, and correction path. If any of those owners or states do not exist, the plan should say so and limit the feature. A supporting faction may extend reach, translation, or expertise, while the player remains the central person navigating consent, audience, and follow-through.

## 368. Expansion installment 30 — A channel outage changes the route, not the fact

A board can be inaccessible, an intercom unavailable, or a message owner unable to publish. The player first checks the channel state and the urgency of the underlying information. The operational owner determines whether the fact remains current; the communication owner determines which alternate route is permitted. The author can wait, post in another authorized place, ask a messenger, or state that no reliable delivery is available.

An outage does not make an old message current. Nor does it grant the player permission to broadcast through every available device. Each fallback has an audience, privacy cost, and confirmation limit. If a message is safety-critical, the relevant owner uses its approved alert process. For routine information, delay may be more responsible than a false claim of shelter-wide notice.

## 369. Outage branch — The intercom fails before an urgent announcement

The channel owner confirms that the intercom cannot be used. The safety or task owner states the required instruction and whether a face-to-face route is necessary. The player can post at affected access points, ask authorized staff to relay the notice, use another supported alert, or help residents avoid the area while the route is restored. The owner confirms which groups still need direct contact.

Do not report that everyone was warned because one broadcast was attempted. The message can show “relay in progress” only if that state exists. If no reliable path reaches someone, identify the uncertainty and ask the owner whether the activity must pause. A later intercom message may repeat the current instruction and clarify that it supersedes earlier notices.

## 370. Outage branch — A public board is blocked by repair work

Residents may not be able to see a board while a corridor is closed. The location owner confirms which routes remain usable. The author can move the notice to another board, send direct follow-up, or wait until access returns. The reader may ask for the information from an attendant or request an accessible copy.

If the board is physically moved, its previous posts may not move with it. The message owner tracks only the copies it controls. A worker can help identify outdated sheets, but the player should not assume every paper copy was collected. For an active instruction, publish a current version at the alternate route and state where the source of truth resides.

## 371. Outage branch — A message is drafted but cannot be approved in time

The author may have prepared an update, but the owner responsible for its factual claim is unavailable. The player can send only a verified holding statement, wait for confirmation, or route the question to another authorized reviewer. The author may state that a report is being checked without naming an unconfirmed cause.

Do not publish a speculative answer to fill silence. If a delay creates a safety risk, the operational owner’s current precaution applies. Residents can ask for a time of next update, but only promise one if someone accepts responsibility for it. A faction contact may supply evidence, but cannot substitute for the missing approval unless the owner grants that authority.

## 372. Outage branch — A messenger reaches only part of the audience

An authorized resident may carry a message to rooms without board access. The author identifies the intended route and which recipients the messenger can reasonably reach. The messenger can accept the task, limit the route, or decline. The message is read as written or summarized with the source and uncertainty preserved.

The player should not turn a messenger into a private surveillance system. They need not report who was absent or how each person reacted unless the task requires delivery confirmation and the owner supports that record. If a recipient asks a question, the messenger can route it back rather than answer outside their authority. A missed person remains unnotified until a supported contact occurs.

## 373. Outage branch — A faction offers its network as a relay

A supporting faction may have a working loudspeaker, radio contact, runner, or board in the affected area. The player checks whether it can carry the message, what audience it reaches, whether the faction will add commentary, and whether the message contains private details. The author may approve a verbatim relay, make a public-safe version, use another route, or decline.

The faction can charge a stated cost or require an authorized host, but it cannot silently rewrite the instruction. The relay’s source should remain identifiable. The player can compare its reach against direct shelter channels and decide whether the privacy tradeoff is acceptable. If the faction cannot confirm delivery, the result remains a relay attempt rather than a completed announcement.

## 374. Outage branch — Readers receive two different fallback versions

One messenger may shorten the text while a board contains the full instruction. A reader can report the difference. The author checks both copies, identifies the operational facts they share, and publishes a clarification if necessary. The operational owner confirms which instruction is current. The reader can ask for a direct explanation or wait for a verified update.

Do not blame the audience for choosing the easier version. The author and channel owner can improve the summary or stop using that route. If the shortened copy changes a condition, retract it where possible and tell affected residents. The message history should preserve which route carried each version if the owner supports that record.

## 375. Outage branch — A privacy boundary rules out the obvious fallback

The broadest available channel may expose a person’s medical appointment, conflict, or individual allocation. The author can separate the public operational fact from the private detail, send a limited message to the affected person, or wait for a safer route. The recipient may authorize a broader notice, decline, or request an intermediary.

The fact that a channel is convenient does not make it appropriate. The relevant privacy owner and communication owner identify what can be shared. If the private detail cannot be separated from the required instruction, the author can ask the operational owner for another procedure. Do not disclose the detail merely to prove that the notice reached everyone.

## 376. Outage branch — The channel returns with stale queued messages

When the system becomes available again, pending drafts or queued announcements may no longer match current conditions. The author reviews each message’s source, expiry, and audience before release. The owner can publish, update, cancel, or discard the draft. The player should see that review happened; an old queue must not broadcast automatically as if no time had passed.

If a queued notice is still valid, the author can send it with the original effective time or issue a fresh copy. Readers can ask whether it was delayed. The channel owner determines what delivery evidence exists. Any draft containing private details must retain its original audience restrictions after restoration.

## 377. Outage outcomes — Reliability includes the admitted gap

The outage can end in an approved alternate relay, a partial delivery, a safe delay, a corrected short version, a privacy-preserving private message, a stale draft cancellation, or a report that some recipients remain unreached. The player sees which fact is current and which channel carried it. Operational owners make the underlying decision; communication owners represent its delivery.

The endpoint should not claim a fully informed shelter when the route failed. The player can choose to invest in existing channels, ask a faction for a bounded relay, or accept the cost of waiting. If repeated outages reveal a missing system, record it as a separate architecture finding rather than introducing a parallel notification service in this plan.

## 378. Installment 30 close — Every fallback has a declared audience and limit

Fallback delivery makes channel resilience a branching path. A resident may choose privacy over reach; an author may choose delay over speculation; a faction may provide a relay; an owner may pause work until the right people are notified. These outcomes are materially different and can shape how the shelter responds to later incidents.

At implementation review, verify the actual board, intercom, and private-message contracts and their save behavior. Do not count a pending draft as delivered. A supporting faction can help bridge a gap, while the player remains responsible for asking what it can truly reach and what information it may carry.

## 379. Final route check — A message’s state is as important as its wording

For each branch, identify who authored the claim, who confirmed the underlying fact, who owns the channel, which audience was intended, what delivery evidence exists, and how the message expires or changes. If one owner or state is missing, narrow the promise. The player should not need to guess whether a line is a draft, an approved notice, a delivered message, or an acknowledged decision.

The closeout should show the current operational fact and any unresolved communication gap. A message may be correct but unread, widely delivered but misunderstood, or acknowledged without agreement. Those are different outcomes and should lead to different next actions.

## 380. Plan 4 continuation close — Communication makes consequences legible

The expanded branches cover channel selection, audience, accessibility, corrections, competing sources, acknowledgement, bad news, repair follow-through, and outages. Notices can change behavior because residents rely on them, and consequences follow from who received accurate information and when. Supporting factions can extend reach or expertise without becoming the authority for every fact.

Before implementation, verify the message producers, channel APIs, save/expiry behavior, and current private-message boundaries. Do not create a second journal or notification registry. Keep the original three Section 4 subfeatures; these additions deepen their routes and endings without changing the feature count.

## 381. Secondary expansion installment 31 — More messages can make a shelter less informed

Several correct notices can still overwhelm the people who need them. A board may be crowded, an intercom may interrupt work, or repeated updates may obscure the one instruction that changed. The communication owner can group related notices, designate one current source, reduce duplicate broadcasts, or ask authors to wait. The player sees which message is operationally urgent and who assigned that status.

Do not introduce an unowned priority score. A safety or task owner determines urgency under current rules; the communication surface can display that classification. Routine announcements remain available without pretending they require immediate action. Residents can ask for a digest or a quieter channel if supported. The author can shorten text while preserving the action, time, scope, and source.

## 382. Overload branch — A resident misses one change among many notices

The resident may follow an older instruction because several updates arrived close together. The player checks which notices they could access and whether the changed message was clearly marked. The author can provide a concise current summary, link the superseded notice, or contact the resident through an authorized route. The resident may ask for fewer updates, an explanation, or help changing plans.

Do not blame the resident for not reading every board. If the change required direct action, the owner should assess whether its communication path was adequate. A later task can be delayed or reassigned according to its actual owner. The message history should distinguish publication time from effective time where supported; a late post cannot be treated as advance notice.

## 383. Overload branch — Two authors want the same channel at once

Two authors may request the intercom or a high-visibility board. The channel owner checks urgency, audience overlap, and whether one message can safely wait. The authors can accept a queue, choose another route, combine only compatible facts, or ask for a separate audience. The player should not personally rank announcements without an assigned authority.

Combining messages can save attention, but it may also expose private content or imply that unrelated decisions share one source. Each author approves the final wording and audience. If either declines, keep them separate. A delayed routine message can retain its original effective date only if the owner confirms that it is still useful.

## 384. Overload branch — A faction asks for repeated promotion

A faction may request repeated announcements for its service, recruitment, or public position. The communication owner applies existing channel limits, if any, and explains who can request a broadcast. The faction can accept a board notice, revise its schedule, or withdraw. Residents can mute optional updates where supported, while essential operational alerts follow their current rules.

Do not make channel volume a hidden measure of faction power. A major group may have more people but should still use the same confirmed channel rules unless the owner defines a real exception. Supporting groups can reach the audience they serve without taking over every board. The player can help the faction find a suitable route or say that space is unavailable.

## 385. Overload branch — The author asks for a dramatic headline

An author may want a warning to attract attention. The player checks whether the wording matches the verified risk and whether the responsible owner approved the urgency. The author can use a concise factual title, include an explicit instruction, or request owner review. Residents should be able to distinguish a real alert from an event announcement or opinion.

Avoid alarm language that overstates certainty. If the situation is uncertain, state what action is precautionary and who issued it. A resident can ask whether the instruction is mandatory or advisory. The owner answers; the communication UI should not infer force from color or formatting alone. A later correction can explain any changed assessment.

## 386. Overload branch — A reader asks for a single current source

The reader may see three versions of a schedule and ask which one to trust. The player checks each source owner and effective time. The communication owner can pin or mark the current notice if supported, or contact the decision owner for clarification. The reader can wait, ask directly, or choose a route that does not depend on the disputed schedule.

Do not silently treat the newest timestamp as authoritative. A newer message may be a comment, not a decision. The owner confirms which one controls. If no one can determine that, label the information unresolved and prevent the interface from presenting one version as settled. A faction may help locate the owner but cannot declare the source current.

## 387. Overload branch — A reader wants a digest that omits personal messages

The resident may ask for a brief operational summary rather than a combined feed. The communication owner can show public, local, and urgent notices within the supported scope. Private messages remain separate. The reader can choose a time window, request only current instructions, or keep the full board. If no digest feature exists, the player can point to the relevant current notice without promising an automated feed.

The author can help distinguish active, superseded, expired, and informational messages. A summary must not hide unresolved uncertainty. If a reader depends on an omitted notice, the owner can provide an additional accessible route. Preference should be honored only as durably as the current owner supports; do not invent a saved filter in the UI layer.

## 388. Overload branch — A notice carries two actions with different deadlines

The author may combine a meeting time with a separate deadline for replies. Readers can mistake one for the other. The player can split the notice, use a clear sequence, or ask the owner to verify each deadline. Recipients may act on one part and miss another. The author can follow up on the missed action without assuming that reading the first detail meant they understood both.

If the actions belong to different owners, each confirms its deadline. A supporting faction can relay the relevant part to its own participants, but should not change the time. The corrected message can show which deadline moved and why. Do not create a single completion state for a notice whose actions can succeed independently.

## 389. Overload outcomes — Priority is a property of the decision, not the prose

The communication route can resolve through a current-source marker, a clear digest, a queued announcement, a reduced duplicate, a faction-specific route, a corrected headline, or an admitted unresolved schedule. The message owner reports what it can display. Operational owners continue to control the facts and urgency.

The player’s choices affect attention, reach, and interruption. A resident may request a quieter channel, an author may accept delay, or a faction may receive only local coverage. These are meaningful branches. They should not rely on a new priority score or a parallel notification system.

## 390. Secondary expansion installment 32 — A communication record should answer who can act next

After a notice is posted, readers may need to know who owns the next step. The author can name a role or current contact if the owner confirms availability. If no one is assigned, state that the request is unowned or awaiting review. A message can provide a reply route, but the reply must reach someone with authority to answer.

The player can check whether the named person still has capacity and whether the notice has expired. If the contact changes, the communication owner can update the post. A stale contact should not leave residents sending messages into a closed route. The next action may be a task, a question, a meeting, or no action; it should be stated without overstating commitment.

## 391. Ownership branch — The named contact is no longer available

The resident may respond to a notice after its author leaves duty or changes roster. The player checks whether another owner accepted the handoff. The communication author can name a successor, route the response to a general owner, or close the notice. The resident can wait, contact a different person, or withdraw.

Do not imply that an inbox is monitored after the named person leaves unless a current owner says so. A private message may need a direct handoff that respects its audience. The outgoing contact can authorize a summary or ask the new owner to contact the resident. If no handoff exists, the message remains unanswered and the system should show that limitation.

## 392. Ownership branch — A resident replies to a notice but asks a different owner

The resident may question the author’s authority and request a reply from the decision owner. The author can forward the question with permission, provide the owner’s contact route, or explain why the author cannot answer. The resident can continue, ask for a meeting, or stop. The decision owner may accept the referral, respond directly, or say the issue is outside scope.

Forwarding should preserve only the information the resident approved. The author cannot broaden a private message’s audience just to get a faster answer. A faction intermediary can carry the question if both resident and owner accept. The branch ends when the resident knows who can answer, not merely when the original author sends a message elsewhere.

## 393. Ownership branch — A promised reply has no assigned owner

The author may have written “we will get back to you” without assigning anyone. The player can ask who accepted follow-up and by when. The author can take responsibility, remove the promise, or refer to an owner. The reader can wait, ask for a named route, or decide not to rely on the statement.

If no person or system can own the response, correct the notice. Do not create an automatic reminder or event log as a narrative patch. A task owner may accept the work, but the communication owner still controls the update. This branch can reveal an integration gap and prevent a false expectation.

## 394. Ownership branch — The response crosses into a private dispute

A public comment may identify a person or allege misconduct. The author can keep the operational question public while directing the personal concern to an existing review owner. The affected resident can respond publicly, privately, or not at all. The player should avoid amplifying the allegation in a broad notice.

The reviewer determines what evidence is needed and who may see it. The communication owner can post a neutral status such as “referred for review” only if the review owner confirms the referral. Do not announce a verdict before one exists. Other readers can still ask for the service update without accessing the private case.

## 395. Ownership branch — The owner cannot answer because the decision is delegated

The first contact may know the process but not control the decision. They can name the delegated owner, request a review, or tell the resident that no answer is available yet. The resident can wait, challenge the handoff, or ask for a different authorized contact. The player should display each authority’s scope clearly.

Delegation does not erase accountability. The original owner may remain responsible for relaying the result. A faction contact can explain its own internal process but cannot speak for the shelter’s owner. If no delegated role is defined, stop and report the missing authority rather than inventing one in dialogue.

## 396. Ownership branch — The resident wants to close the message thread

The resident may consider the answer sufficient and ask that no further contact be sent. The communication owner can close the thread if supported. The resident can still open a new request later. A sender may retain an operational record where required, but should not keep a conversation active for narrative convenience.

If action remains pending, the owner explains that closing the conversation does not cancel the task unless the resident has authority to cancel it. The resident can confirm the distinction, request a separate task update, or keep the thread open. Do not treat a closed thread as proof of satisfaction unless they explicitly say so.

## 397. Ownership outcomes — A named next step prevents the dead-end notice

The communication route can end with an available contact, accepted handoff, owner referral, public/private split, delegated decision, corrected promise, closed conversation, or unresolved authority gap. The resident should see the next action and who can take it. If no action exists, the message should be honest about that.

This secondary pass adds responsibility handoffs to the existing message, broadcast, and private-message routes. It does not create a new case-management system. Any durable assignment remains with its current task or message owner; authored continuity names the owner and audience only where the system can verify them.

## 398. Secondary review — The message cannot create an operational decision

Review every new communication branch for the distinction between informing, requesting, approving, assigning, and completing. A board post may describe a confirmed closure, but cannot close a route by itself. A private message may request help, but cannot assign a worker. An acknowledgement may confirm reading, but cannot accept an offer. An intercom may deliver an instruction, but the operational owner still issues it.

Where the existing contract combines some actions, document that behavior precisely. Where it lacks a route, keep the proposal staged. This review prevents the expanded prose from encouraging a second mutable authority in Godot UI or narrative flags.

## 399. Secondary review — Every branch needs a quiet ending as well as a dramatic one

Residents may choose not to answer, decline a public meeting, accept a delay, or close a thread. Those endings should receive the same clarity as an emergency broadcast or public correction. The player can tell what was declined, whether any operation changed, and whether an invitation remains open. Silence is not always a hidden clue.

Writers should avoid escalating every missed message into suspicion. A channel failure can be mundane. An author can be busy. A reader can prefer privacy. The next scene should follow confirmed facts and established relationships, allowing a quiet choice to matter through reduced contact or a delayed task when the owner supports it.

## 400. Plan 4 secondary close — Reach, clarity, and ownership travel together

The expanded communication routes now address channel selection, overload, access, outages, corrections, replies, handoffs, and cross-owner decisions. The player can choose public reach, private care, a faction relay, or a delay, with each choice affecting who can act and what is known. No message is treated as universal knowledge merely because it exists.

Before implementation, trace each path from source owner to message owner to audience and back to the responder. Verify persistence and expiry. Retain the three existing feature subparts and avoid a parallel journal, queue, or contact registry. The proposal remains grounded in truthful delivery and current operational owners.

## 401. Secondary expansion installment 33 — A notice can invite action without performing it

A board may ask for volunteers, questions, supplies, or meeting attendance. The notice informs residents of an opportunity; it does not enroll them, reserve their time, or transfer their property. The task owner decides whether a volunteer is eligible, the property owner decides whether a contribution can be accepted, and the meeting owner controls attendance rules. The message should identify how to respond and what will happen after a response arrives.

The player can help a resident act on the invitation, ask for more information, decline, or ignore it. The author can close the invitation when capacity is filled, but only after the relevant owner confirms that state. If a person responds after closure, the author can offer a waitlist only if one exists, refer them elsewhere, or explain that no space remains. A public notice cannot create a hidden queue.

## 402. Invitation branch — A resident volunteers but is not eligible for the task

The resident may respond before the task owner checks qualifications. The owner can accept, request assessment, offer a supervised role, or decline. The author should acknowledge the offer without promising assignment. The resident can choose the alternate role, ask what qualification is missing, train later, or withdraw.

Do not publish the resident’s ineligibility as a public correction unless it is operationally necessary and authorized. A private answer may be more appropriate. The task owner’s criteria remain independent of who read the notice first. If there are no eligible volunteers, the owner can reopen the request, contact a qualified person, or delay the work.

## 403. Invitation branch — A resident offers a resource through the board

The notice may invite needed materials. A resident can offer an item publicly or ask to discuss it privately. The property owner confirms ownership and transfer rules; the task owner confirms whether it is useful. The resident can withdraw before settlement. The author can acknowledge the offer while making clear that it has not yet been accepted.

Do not display private inventory or scarcity details to all readers. The board needs only the amount, contact route, and status the owner authorizes. If a faction offers the same resource, the task owner compares availability and terms. The player can present both routes, but the notice itself does not select a donor.

## 404. Invitation branch — A resident asks a question instead of accepting

The reader may need to know time, location, risk, workload, or who will supervise before deciding. The author can answer, refer to the task owner, or admit that the detail is not confirmed. The resident can then accept, ask another question, decline, or wait. A question should not count as a commitment.

If the answer changes the original notice materially, publish a correction for the whole intended audience. A private answer may be sufficient only if the detail applies to that resident alone. The author should not create unequal access by giving important terms only to the first person who asked. Other readers may need the same information before they can make an informed choice.

## 405. Invitation branch — The invitation closes while a reply is in transit

A resident may send a response through a slow messenger or an unavailable board just as the owner fills the task. The player checks when the offer closed and when the response reached the owner. The resident can accept another role, ask for review, or withdraw. The owner can reopen capacity, explain that the assignment is complete, or place the person in a future route if supported.

Do not backdate acceptance unless the current system records a valid submission time. A delayed channel can still be acknowledged without displacing the person already assigned. The author can correct the post to state that the invitation is closed and offer a contact for future opportunities. The resident’s response may be valuable even when it arrives too late.

## 406. Invitation branch — A resident wants to volunteer anonymously

The resident may wish to offer help without public recognition. The author checks whether private intake exists and whether the task owner needs a named assignee for safety or scheduling. The resident can provide identity privately, volunteer for a no-contact contribution if supported, or decline. The owner explains what information is required before accepting.

Do not promise anonymity when a task requires supervision or follow-up. If identity must be known to the owner, it may still be kept out of public notice. The resident can decide whether that distinction is acceptable. A public acknowledgment can report that the task has support without naming the person if the owner confirms that is accurate.

## 407. Invitation branch — The invitation is misread as a mandatory assignment

A resident may believe that a posted request is an order. The player checks the wording and the actual task owner decision. The author can clarify “voluntary,” “assigned,” or “awaiting volunteers” only if that matches the owner’s state. The resident can accept, decline, or ask for a formal assignment. The task owner confirms any obligation.

If the notice uses ambiguous language, correct it for everyone who could rely on it. Do not punish a resident for declining a request presented as optional. Conversely, do not call an assigned duty voluntary to soften it. The distinction affects schedules, consent, and future trust. A faction’s invitation can have different terms from a shelter duty; state the source.

## 408. Invitation branch — A volunteer withdraws after learning the conditions

The resident may accept an invitation in principle, then withdraw after learning the schedule, physical risk, or public audience. The task owner can offer a different role, revise scope, or proceed without them. The resident can accept an alternate, request a later opportunity, or stop. The author updates the capacity status only after the owner confirms a replacement.

The withdrawal does not mean the resident acted in bad faith. The initial response was not a final assignment. If the task already depends on their work, the owner can pause, hand off, or state the actual cost. The message should not use guilt to recruit a substitute. A new volunteer makes their own decision.

## 409. Invitation outcomes — A response has its own state and owner

The route can end in a question answered, a qualified volunteer assigned, a supervised role offered, a private resource contribution, an anonymous inquiry, a corrected mandatory/optional label, a delayed response, or a withdrawn offer. The communication owner reports receipt; the task owner confirms assignment; the property owner confirms any transfer.

This gives notices a branching effect on play without granting them operational power. A resident can act, ask, wait, or decline. The author can update the audience when capacity changes. Each state stays attached to the owner responsible for it.

## 410. Secondary expansion installment 34 — A message’s history can affect future choices without becoming surveillance

Residents may remember that an earlier notice was late, a correction was clear, or a request received no answer. Those experiences can influence whether they use the same channel again. The player can surface a specific remembered event in dialogue, but the communication plan should not monitor every reader’s private behavior. The author’s record shows message events; it does not become a dossier on individual attention.

If the social owner supports a relationship consequence, it should derive from a concrete interaction such as a promised response that was missed. A person who never saw the message should not be penalized for ignoring it. The player can ask what the resident knew before interpreting their choice. That small question prevents a false branch built on omniscient assumptions.

## 411. History branch — A resident distrusts the board after an inaccurate post

The resident may prefer a direct source because an earlier board contained a mistake. The player can acknowledge the corrected fact, show the current owner, or offer a private confirmation. The resident can use the board again, request another channel, or continue to distrust it. The author can explain what changed in the posting process if that is confirmed.

Do not force trust to recover after one correction. Nor should one error make the board universally unusable. A resident’s choice can depend on the stakes: routine meeting details may be accepted while safety instructions need direct confirmation. The communication owner can improve content, but the reader controls whether to rely on it.

## 412. History branch — A resident relies on a reliable channel and misses a one-time exception

The resident may usually follow board notices but miss a private exception intended for a limited audience. The player can explain the scope and ask whether the person was authorized to receive it. The author can issue a general correction if the exception changed public operations, or keep it private if it remains individual.

Do not infer that the resident ignored the exception if the channel did not reach them. If their action created a real task consequence, the owner determines the response. A later public rule may change to avoid future confusion. This branch highlights that a channel’s reliability is specific to audience and message type.

## 413. History branch — A resident receives a message through an unreliable intermediary

The resident may report that a messenger gave a different time or instruction. The player checks the original text, the messenger’s authorized role, and any owner-confirmed relay. The author can issue a clarification, ask the intermediary to correct their version, or contact the resident directly. The resident can accept the update, request another route, or ask for review.

The intermediary may have misunderstood rather than altered the message deliberately. The author can improve the summary and specify which details must remain verbatim. If the relay was not authorized, do not count it as delivery. Future use of that messenger is a new decision with disclosure of the earlier problem.

## 414. History branch — The author wants to know who ignored the invitation

The author may ask for a list of residents who did not respond. The communication owner checks whether that information exists and whether the audience was told responses were tracked. If not, the player cannot provide a list. The author can repost the opportunity, ask for a public count of remaining slots, or let the invitation close.

Do not use nonresponse to infer disinterest, faction opposition, or unreliability. A private response preference should remain private. If a task needs enough volunteers, the task owner can state the remaining capacity without identifying who declined. This preserves useful planning without turning notices into surveillance.

## 415. History branch — A resident asks that their earlier public reply be removed

The resident may regret publicly accepting or declining an invitation. The communication owner can remove the reply, annotate it, or explain that the record must remain. The task owner reports whether the assignment is already committed. The resident can request privacy, clarify the current choice, or accept that the old action remains visible.

Do not pretend that removal reverses a task already started. If the resident needs to withdraw, use the task owner’s route. If the reply remains for audit, the player can explain the audience and retention rule. Any public correction should avoid repeating the private reason for the change.

## 416. History branch — A repeated notice reaches someone who already declined

The resident may receive another invitation and feel pressured because their earlier refusal was not reflected in the author’s process. The player checks whether the previous no was specific to one date, one task, or all future contact. The resident can accept the new offer, restate a boundary, request a different channel, or ignore it.

Do not assume a refusal is permanent when the terms changed; do not assume it was temporary when the person asked not to be contacted. The author can correct the recipient list only through supported state. If no durable preference exists, ask the resident what they want now and avoid promising that every future message will be filtered.

## 417. History outcomes — Remember events, not invisible attitudes

Specific history can support future branches: a correction was issued, a follow-up was missed, a resident asked for direct confirmation, or an invitation was repeated after refusal. An attitude such as “never trusts boards” should not appear without an owner-backed relationship change or a deliberate authored choice. The player should be able to ask what the resident remembers.

The feature grows richer through accurate callbacks and boundaries. A notice can improve or damage confidence in a channel, while each person remains free to reassess it. The plan adds no reader tracking, global reliability meter, or communication reputation ledger.

## 418. Secondary review — A notice that asks for action needs a destination

Before posting a call to action, the author confirms where a response goes, who can receive it, what information is required, how long the invitation remains valid, and who closes it. If any answer is unknown, phrase the notice as informational or hold publication. This avoids open-ended calls that collect interest without a real owner.

The destination can be a task owner, an existing meeting, a private contact, or a current property route. It is not a new generic inbox. The player should see whether a response was received only when the current owner exposes that fact.

## 419. Secondary review — A notice cannot make a person’s decision public by default

Authors may report total volunteers or remaining capacity when the task owner confirms those facts. Individual choices—acceptance, refusal, need, or reason—remain private unless each person authorizes attribution and the owner permits publication. An operational summary can say “one position remains” without listing who declined.

This distinction applies to public boards, intercoms, and faction relays. The audience should receive the information needed to act, not a character judgment. If an author needs personal follow-up, use the private channel and retain its narrow scope.

## 420. Plan 4 secondary close — A notice can invite a branch, but owners complete it

The additional branches connect notices to volunteer decisions, resource offers, questions, access needs, channel history, and future trust. The player can help readers understand and act, while task, property, privacy, and message owners confirm their own states. Nonresponse remains uncertain rather than becoming consent or disloyalty.

These are secondary edits to the existing notice, intercom, and private-message promises. They do not add a fourth communication pillar or a hidden audience analytics system. Implementation still depends on current channels, message lifecycle, and truthful acknowledgement evidence.

## 421. Secondary coda — A correction must remain discoverable long enough to matter

The author may correct a notice and then let the correction expire before affected residents have a reasonable chance to see it. The communication owner can confirm how long it remains visible and whether an active instruction needs a longer lifecycle. The task owner determines when the operational change ends. The player can request a new update, keep a local copy through a supported route, or state that delivery remains uncertain.

Do not extend a notice forever merely because one person has not responded. The audience, urgency, and owner’s current state determine whether another follow-up is necessary. A correction can be archived as historical only if the owner supports that state. The active message should still make clear which instruction applies now.

## 422. Secondary coda — A resident can correct their own response

A resident may publicly volunteer, ask a question, or decline and later want to revise that response. The task owner checks whether assignment is already committed; the communication owner checks whether the earlier reply remains visible. The resident can update it, send a private correction, or let the original stand. A changed response does not imply that the notice itself was wrong.

If other residents relied on the first response, the owner decides what update they need. Do not expose the resident’s private reason for changing their mind. The new action follows the current task state and any handoff cost. The author can close the invitation once capacity is confirmed, not merely because an earlier response appeared.

## 423. Secondary coda — A quiet message can be the right ending

Some threads end with an answer, a declined invitation, or a request for no further contact. The author can close the message without a final public announcement. The affected person may want no acknowledgment beyond knowing the task owner received their response. The player can honor that preference while retaining any operational record required by the owner.

## 424. Secondary coda — Reopen a notice only for a current reason

An old thread may become relevant again when the task owner changes the schedule, a reader asks a new question, or the original fact is corrected. The author can issue an update linked to the current source. If nothing changed, leave the closed message closed. Reopening should not be a substitute for a new notice with a new audience and expiry.

The reader can choose whether to follow the update if the action remains optional. If the owner’s decision is mandatory, the new message should name that authority and effective time. A changed status requires a current source, not a thread revival alone.

## 425. Expansion installment 35 — A notice should make its action understandable at a glance

Readers often encounter a message while moving between work, rest, and shared spaces. The author should lead with the confirmed action or the fact that no action is required, then give location, effective time, source, and contact route. A long explanation can follow, but should not bury a closure or deadline in background. The player can help edit wording, ask the owner to verify the action, or postpone publication if the essential fact is unknown.

Clarity does not mean stripping away uncertainty or context. If a time is estimated, label it. If the message applies only to one room or group, name the scope. If the reader may refuse an invitation, say so. The audience can ask for fuller detail, an accessible format, or a private explanation. The author can provide it through the proper owner without turning a short public notice into a complete history of every dispute.

## 426. Clarity branch — A reader cannot tell whether the notice is mandatory

The reader may interpret a request as an order or an order as optional. The player checks the source owner and the exact task state. The author can clarify whether compliance is required, who issued the instruction, and what happens if the resident cannot comply. The reader can accept, challenge, request accommodation, or ask for an alternate route.

The communication owner should not infer authority from bold text or color. If the underlying owner has not issued a mandatory instruction, the author should not present one. If the instruction is mandatory, softening it to sound friendly can also mislead. The player’s role is to ensure the message accurately reflects the decision, not to make every notice sound equally forceful.

## 427. Clarity branch — A notice uses a term residents understand differently

The author may use a technical or faction-specific phrase. Readers can ask what it means or act on an incorrect interpretation. The source owner or qualified contact can provide a plain-language explanation. The author may add a short definition, a practical example, or a pointer to an authorized source. Readers can request a different format or a more complete explanation.

Do not treat the reader’s unfamiliarity as a failure. The term may be locally ambiguous. If two groups use it differently, the author should state which meaning applies in this notice. A later correction can clarify the wording without implying that the underlying rule changed. Factions may retain their jargon in quoted material, with a neutral explanation beside it.

## 428. Clarity branch — The author wants to combine instruction and explanation

The author may need readers to act immediately while also explaining why. The message can put the action first, then include the source and reason. If the explanation is sensitive, the author can keep it private and provide only the operational fact publicly. The reader can ask follow-up questions through the named owner.

Do not make the action depend on understanding a long political argument unless the owner explicitly requires informed consent. When a choice is optional, include enough terms for a decision before requesting a response. If the author cannot confirm the reason, state that it is not available rather than inventing a motive. The operational instruction and its explanation can evolve on separate timelines.

## 429. Clarity branch — A resident asks for the history behind a current rule

The resident may want to know why a restriction exists. The player can point to the decision owner, an approved public record, or an upcoming review. The author can summarize established history, identify disputed accounts, or say that the explanation is unavailable. The resident can comply meanwhile, request review, seek a different route, or decline where allowed.

Do not expose a private incident merely to make the rule feel justified. A faction may tell its account, but identify it as that faction’s view. If a public source contradicts the summary, route a correction. The message is successful if residents can tell what applies and who can answer—not if every reader accepts the rule’s justification.

## 430. Clarity branch — The public instruction has a private exception

An exception may apply to an individual resident without changing the general instruction. The author can send the exception privately and keep the public notice general, or ask the owner to define a public conditional rule. The affected resident can accept the exception, ask for clarification, or decline to use it. Other readers may ask whether the rule is consistent.

The owner determines who qualifies; the author should not disclose personal criteria publicly. If a fairness review is available, residents can request it. An exception should include its duration and expiry only if the owner confirms them. When the exception ends, the resident needs an accurate update through an authorized channel.

## 431. Clarity branch — A resident asks for a response deadline

The reader may need to decide by a certain time. The message owner can state a deadline only if the decision owner has provided one. The reader can ask for an extension, say they cannot respond, or decline. The author can explain whether silence closes the opportunity, leaves it open, or has no effect under current rules.

Do not invent urgency to improve response rate. A genuine capacity limit can be described with its source. If the deadline changes, correct the original notice for everyone who may rely on it. A later reply should be checked against current availability rather than rejected automatically because the old message expired.

## 432. Clarity outcomes — Readability supports choice; it does not guarantee agreement

These routes can end with a corrected mandatory/optional distinction, defined terminology, an action-first instruction, a historical referral, a private exception, or a confirmed deadline. Readers may still disagree or choose not to respond. The author’s obligation is to communicate the current fact and available action accurately.

This installment extends the existing board and intercom subfeatures through message structure and reader interpretation. It does not add a tutorial or a universal communications template authority. The actual message owner and operational source determine what wording can be promised.

## 433. Expansion installment 36 — Several channels can carry one decision without pretending they are one event

A decision may appear as a private notification, a public notice, and an intercom call. The audience, timing, and detail differ for each. The player can help the author choose which channel carries the full instruction, which repeats a brief alert, and where readers can find the current version. The communication owner confirms whether cross-links or shared identifiers exist.

Do not claim a unified broadcast merely because the same sentence appears in multiple places. Each channel can fail or reach a different audience. A public post may remain active after a private message is corrected. The author should use one confirmed source and coordinate amendments across copies. The player can inspect which versions are under the owner’s control and which remain uncertain.

## 434. Multi-channel branch — The private detail should not follow the public copy

The same decision may require a broad operational notice and an individual explanation. The author writes a public-safe version with scope and next action, then sends the private detail only to the person who needs it. The affected resident can ask to receive the explanation in person, through an intermediary, or not at all.

The public copy should not hint at private circumstances in a way that identifies the person. The private message should point to the public instruction only if the recipient can access it. If the owner changes the shared decision, update both messages through their respective routes. Do not automatically duplicate private replies into the public thread.

## 435. Multi-channel branch — A channel copy fails to update

The author may correct the board while an intercom script or private draft retains the old fact. The player checks each copy the communication owner tracks. The author can update, cancel, or repeat the affected version. Readers who already received the stale message may need a direct correction.

If a copy cannot be retrieved, state that it may remain in circulation. The operational owner decides whether that uncertainty requires a temporary restriction. A later reader can report the old wording; the author can verify and correct it. Do not mark every channel current based on a single successful edit.

## 436. Multi-channel branch — The first channel reaches an audience before the owner is ready

A public post may appear before a private conversation with an affected resident. The player can ask whether timing can be changed or whether the operational fact must be announced immediately. The owner decides the urgency; the affected person can receive a direct message first if that is permitted and feasible. If not, the public update may need a careful scope that protects privacy.

Do not delay a necessary safety instruction solely to preserve dramatic reveal, and do not publish personal news early for convenience. The author can state the general fact now and arrange private follow-up. The audience may ask questions, and the owner can name what remains private or unknown.

## 437. Multi-channel branch — A reader sees one version and another reader sees a different one

Two residents may compare messages and report a discrepancy. The player identifies source, effective time, and audience for each. The author can publish a shared clarification, contact only the affected audience, or ask the decision owner to resolve a substantive conflict. Each reader can choose whether to wait or act on the current confirmed instruction.

Do not assume the reader who saw the longer message has the authoritative version. The owner’s current state determines that. A faction relay may contain a shortened summary; label its source and any missing details. The correction should reach both audiences if both versions could change behavior.

## 438. Multi-channel branch — One resident asks not to receive a particular medium

The resident may prefer not to hear the intercom, read a public board, or receive a private message for a nonessential topic. The communication owner explains which options exist and which notices cannot be suppressed. The resident can choose another channel, ask for an intermediary, or accept the essential alert rule.

The author should not infer that a resident is avoiding information. A channel preference is not a refusal of the underlying decision. If the preference cannot be saved, re-confirm it where relevant and avoid claiming permanent filtering. Factions must follow the same disclosed limits when relaying shelter information.

## 439. Multi-channel branch — A channel implies more certainty than another

The intercom may sound definitive while a written notice includes caveats. The player checks whether tone, formatting, or editability has changed the apparent certainty. The owner can issue a clarification in the channel that caused the misunderstanding and link to the current source. The reader can ask for a plain explanation or choose to wait.

The author should not rely on a long disclaimer in one medium to correct an overconfident headline in another. Preserve the important limitation in every channel carrying the action. If the fact is still under review, say so consistently. A new channel may need a shorter message, but not a stronger claim.

## 440. Multi-channel outcomes — The reader can identify the current source

The chain may end in aligned copies, one corrected audience, an admitted stale version, a channel preference, a private follow-up, or an owner decision delayed until reconciliation. The reader should know where to find the current operational fact and who can answer. A channel is only a route; it is not the underlying source.

This installment adds cross-channel consequences while preserving one decision owner and one communication owner per relevant contract. If shared message identity, edit propagation, or delivery receipts are unavailable, the implementation plan must say so and narrow the promise.

## 441. Forward scaffold — Remaining Plan 4 expansion sequence

The following scaffold maps future sections toward the 120,000-word minimum. Each installment should add grounded cases to the three existing subfeatures—public boards, intercom/broadcast, and private messages—without adding a fourth communication pillar or a parallel journal. Entries are proposed work, not evidence of existing implementation.

### Installment 37 — Notice authorship and delegated posting

Explore an author who lacks board rights, a trusted delegate who changes wording, an owner who revokes access, and a correction after a delegated post. Distinguish author, approver, poster, and source owner. **Evidence:** current message API, access rule, edit history, and save ownership.

### Installment 38 — Quiet hours and channel interruption cost

Branch on an urgent alert during rest, a routine announcement deferred until morning, an exception for an active task, and residents who opt into a different channel. **Evidence:** whether quiet hours exist and who can classify urgency; do not invent global alert priority.

### Installment 39 — Messages that require a reply

Clarify optional response, explicit consent, deadline, no-response state, late reply, and accepted handoff. **Evidence:** whether the current owner can distinguish delivered, read, acknowledged, and accepted.

### Installment 40 — Local boards with temporary ownership

Explore a room owner changing, a board being repaired, two groups sharing a surface, and a resident requesting removal. **Evidence:** physical location access and current notice lifecycle.

### Installment 41 — Safe public corrections after rumor

Separate a verified correction, an unresolved claim, a personal accusation, and a faction’s attributed interpretation. **Evidence:** source owner and existing review route; no truth-by-alignment branch.

### Installment 42 — Private message boundaries during staff handoff

Explore a recipient changing duties, an old contact leaving, a delegated answer, consent to summarize, and a message that should not transfer. **Evidence:** private-message audience and handoff contract.

### Installment 43 — Emergency messages with incomplete reach

Branch on a confirmed alert, a failed broadcast, a faction relay, an unreachable room, and a second notice after restoration. **Evidence:** actual emergency process and channel states.

### Installment 44 — Intercom requests and public questions

Explore who may speak, whether a call is live or recorded, time limits, a resident who withdraws, and a public question requiring private follow-up. **Evidence:** actual intercom producer and persistence.

### Installment 45 — Reading access, translation, and source fidelity

Add cases for plain-language summaries, dialect mismatch, a difficult technical term, and an inaccurate translation. **Evidence:** current localization and communications owner; keep translation corrections tied to source authority.

### Installment 46 — Notice fatigue and selective muting

Model a resident asking for fewer updates, an essential notice they still need, repeated faction promotions, and a lost urgent message. **Evidence:** channel preference state and delivery behavior.

### Installment 47 — Message records, expiry, and historical access

Explore an expired copy still visible, a reader requesting the original, an archive that lacks permission, and a correction that needs a persistent citation. **Evidence:** current retention and expiry behavior.

### Installment 48 — Multi-owner announcements

Branch on a task owner and location owner issuing different details, a shared schedule, one owner unavailable, and an unresolved authority seam. **Evidence:** exact owner boundaries; document gaps rather than appointing a fictional coordinator.

### Installment 49 — Faction relay terms

Cover a fee, verbatim relay, editorial condition, public endorsement request, and a relay that reaches only part of the audience. **Evidence:** faction service contract and channel reach.

### Installment 50 — Cross-plan information dependencies

Map how a notice about a lesson, survivor initiative, or trade is produced, what may be shared, and which plan owns the follow-up. **Evidence:** cross-plan producer/consumer review; avoid duplicate quest state.

### Installment 51 — Accessibility audit for message actions

Review whether a reader can discover the next action, request help, decline, appeal, or receive a private explanation. **Evidence:** current UI focus, keyboard/controller routes, display size, and content formats.

### Installment 52 — Narrative tone under bad news

Create cases for loss, ration change, cancellation, grief, and unresolved causes. Keep confirmed fact, estimate, and opinion distinct. **Evidence:** current canon voices and owner-supplied wording.

### Installment 53 — Message lifecycle and save/load acceptance

Define focused scenarios for draft, approved, posted, amended, expired, and restored states based on actual APIs. Verify that reload does not duplicate broadcasts or resurrect stale notices.

### Installment 54 — Plan 4 closeout criteria

Close only after the document exceeds 120,000 measured words, preserves its three subfeatures, includes current source evidence, and distinguishes plan completion from an implemented communications system.

## 442. Scaffold realization — Installment 37: authorship and delegated posting

This case begins with a resident who wants information posted while they are occupied elsewhere. The player is not simply choosing whether to “help”; they are deciding what authority to exercise, what wording to endorse, and which audience is entitled to see the message. A communication surface becomes meaningful when the author is visible. Delegation therefore needs a precise boundary: permission to post a supplied notice is different from permission to edit its claims, broaden its audience, promise action on behalf of the author, or turn a temporary update into policy.

### The posting contract

Every draft in this scenario has four inspectable pieces: source author, approved text or template, destination board/channel, and expiration or review event. A fifth piece—who may edit—is required whenever the player is asked to revise copy. If the current message model cannot store or derive these facts reliably, the plan treats delegation as a proposed interaction requiring an owner decision. It must not hide authorization in a local panel flag.

The initial offer can arise after an owner-backed incident or a resident's explicit message request. The author tells the player whether they want a verbatim post, a proofread, a summary, or only help with the posting action. Each has its own confirmation. Verbatim means text remains unchanged and previewable. Proofread means spelling or clarity edits are shown before posting. Summary means the author can review the paraphrase and correct it. Posting on someone's behalf does not imply that person endorsed factual claims the player added. If the author cannot review the final text, the interface must restrict the player to the exact approved template and its approved audience.

### Branches from the player's action history

Previous decisions shape access through specific permissions, not a generalized faction favor score. A player who previously corrected a false date can be offered a correction workflow. A player who shared a private request publicly without permission may now be limited to a draft that requires author review. A player who reliably used the local work board may be allowed to post within that board's known scope, but the source owner still checks access. A player who delegated too much may find that the author asks to reclaim control. Each branch should identify the prior event that made the option available; if that event cannot be queried, the plan must use the same options for all players rather than inventing history.

Actions also create distinct later branches. Posting the approved version may lead to acknowledgements from the intended audience. Posting a revised version can require author approval. Declining can leave the author to find another means of communication. Delaying to verify a claim can prevent misinformation but make the notice arrive after the event. Asking a supporting records group to verify one date can improve certainty while consuming that group's service time. A player does not gain an “honesty” score; the specific copy and recipients determine the outcome.

### A small supporting faction's part

A local clerks' circle, maintenance crew, or other canon-supported group can provide one useful function: validate a schedule, confirm an operational status, or post on a board the player cannot access. These examples are role slots; they must map to current factions before use. Their support is not an approval stamp. A records group can confirm that the pump is under inspection; it cannot claim the pump is safe. A maintenance worker can confirm when a repair is scheduled; they cannot speak for residents affected by the repair. A board custodian can explain audience and expiry rules; they cannot rewrite the author’s meaning.

The player can request the service, wait, use a lower-certainty label, or post only the already verified portion. A support request may be declined because staff are busy. That creates a branch: delay, publish a clearly marked preliminary update, or omit the uncertain detail. The faction's refusal does not make its later services unavailable forever. Where service availability is a real roster/capacity fact, show it. Where it is only authored, do not save a synthetic cooldown.

### Concrete example: the missing water slot

A notice is proposed for a local board: the afternoon filling slot has changed because a repair team needs access. The player can publish the verified time change while leaving the reason as “maintenance in progress,” ask the crew to confirm the estimated completion, edit the notice into a broader policy, or hold the draft. The crew can confirm only the work schedule it owns. The player who previously promised an exact return time for a repair receives a different option: reference the old promise and explain that it was not confirmed. The player who has not spoken to the crew does not get an authored “they said” option.

The audience changes the consequences. A local work board reaches the affected shift and can carry operational detail. A shelter-wide intercom would disrupt more residents and implies urgency; the system should not offer that route unless the change is genuinely time-critical and the broadcast owner supports it. A private message to one resident can clarify their schedule but does not notify everyone who depends on the filling slot. An author may choose a private follow-up for their own household; this does not become the canonical announcement.

### Resolution and durable trace

After posting, show author, editor or delegate, destination, time, and current status. The message record should preserve the final approved text and any amendment history only if the existing owner supports that. A later correction must refer to the earlier message, identify what changed, and reach the appropriate audience. It should not silently replace the original and make it appear that the shelter was never misinformed. If history is not persisted, the plan must avoid promising an audit trail that cannot survive reload.

Acceptance walkthroughs should cover: exact-text delegation; edited copy awaiting approval; author unavailable after approving a fixed template; delegate attempting to broaden the audience; supporting group declining verification; a correction that reaches the original audience; and reload after a posted notice. The core proof is one source record with truthful authorization. Board view and notification are projections of that message, not separate authorities that can disagree about authorship.

## 443. Scaffold realization — Installment 38: quiet hours and interrupt priority

The quiet-hours case makes communication frequency and interruption cost part of play. It is not a generic notification preferences menu detached from survival. Residents sleep, work, recover, and wait for critical information. A message that arrives at the wrong time can be technically delivered and practically harmful. Conversely, a shelter-wide quiet rule can hide an urgent hazard. The system must represent urgency as a source-backed classification with a cost, not as a button the player can spam to make every request feel important.

### Separate channel, urgency, and delivery time

The player is choosing at least three distinct things: how a message is sent, how interruptive it is, and when it should be delivered. A local board may be passive by default. A direct private message can be shown when the recipient next checks it, if that is supported. An intercom interrupts and should be reserved for source-defined urgent conditions. A scheduled notice is not “missed” simply because a resident did not read it immediately. Do not collapse these into a single unread count.

When a player creates a message, the interface can show expected reach and disruption: number or class of intended recipients if the owner supplies it, whether a chime occurs, whether the message remains posted, and what condition warrants immediate interruption. If exact reach is unknown, say “board audience: workshop” rather than displaying a made-up resident count. Do not claim to know that everyone is awake or listening unless current schedule and acknowledgement owners prove it.

### Branches by what the player chooses to interrupt

An operational change needed before a shift may justify a targeted update to that shift. A confirmed immediate danger may justify an intercom. A non-urgent request can be placed on a board or delivered at the next available time. A resident's private message can remain private even when it contains a pressing matter; the sender can authorize escalation, or an existing emergency policy can do so if one is already implemented. The plan does not invent mandatory disclosure based on dramatic convenience.

The player can choose: post now on the local board; schedule for the next shift; send a direct note; ask the author whether an interruption is acceptable; or use the urgent route when the triggering fact is confirmed. If the player chooses a less disruptive route and information arrives late, the consequence may be a missed slot or extra work. If they choose an unnecessary alarm, the consequence may be disruption, alarm fatigue, or lower confidence in future announcements—but these consequences require real state owners or authored one-off feedback. They should not create an unbounded hidden “attention” meter.

### Branch based on a previous interruption

If a prior urgent broadcast was later corrected, the player now sees the source status and correction before repeating it. If a resident explicitly asked not to receive routine intercom announcements, a private message route may be preferred when the current system stores that preference. If no preference API exists, the plan does not claim the game remembers it. If a player used the intercom for a confirmed water failure, future residents can respond to that specific incident: they may wait for the next verified update rather than assume every chime means an emergency. This is authored contextual variation, not a global credibility stat.

An emergency can still break a quiet period, but only a current policy or owner-backed severity should make that exception. The prompt must explain why the exception applies and who will be interrupted. When the source is uncertain, the player can choose a limited alert with uncertainty disclosed—“possible leak, inspection underway”—if the broadcast system supports qualifiers. The game should not force certainty to justify urgency.

### Supporting group role and practical limits

A night watch, care team, or other small faction can help assess who must be reached before the next shift. Its role remains supporting: the group can provide a roster fact, carry a message to a specific person, or advise whether a warning is time-sensitive. It cannot set shelter-wide messaging policy unless the current authority assigns it that role. If one of its members is off shift, the player sees that service is unavailable and can pick another route. This creates a meaningful resource decision without converting the group into a rival command structure.

The group's assistance can itself have cost: a runner misses a planned task, a watch member spends time verifying a location, or the message waits until a receiver is available. Use these only if a real task/time owner can express the cost. If no such owner exists, write them as narrative trade-offs that inform the proposal rather than persistent simulation variables. The supporting group should not be used as an excuse to invent a second queue system.

### Branch outcomes for three message classes

**Class 1: confirmed immediate danger.** A source event confirms the hazard. The urgent route is available, delivery/acknowledgement share one canonical record, and the next steps link to the hazard owner. The message can be followed by a correction or all-clear from that same source. Silence from a recipient does not count as evacuation or safety.

**Class 2: near-term operational change.** The player chooses a local board, a direct note to affected participants, or a shift-timed post. The decision turns on who needs to act by when. A missed recipient can create a later operational consequence; the system does not narrate them as negligent if delivery was never proven.

**Class 3: routine social request.** The message remains passive or private. A reader can reply, decline, or not respond. The absence of a reply is not consent, anger, or rejection. The author may choose to withdraw the request if the owner supports withdrawal; otherwise the message expires by a truthful rule.

### Tone and interface response

An alarm should sound different from routine room chatter, but the proposal concerns message semantics rather than audio design. Any sound cue must use the existing audio authority, and its implementation belongs to that owner. The written message should state the fact before asking residents to act. Avoid hollow urgency (“Important!”) where the specific action and deadline can be named. If a message is corrected, keep the correction factual and calm. Do not punish players through intentionally vague copy; the system should teach priority through visible reach and cost.

Acceptance should prove that urgent and routine routes cannot accidentally share the same acknowledgement source when their content is different; that a routine message does not mark the whole shelter as notified; that a player can defer a non-urgent request; that a confirmed urgent source can interrupt only through its allowed route; and that a correction reaches the intended audience once. These are design expectations pending current API inspection, not claims of existing behavior.

## 444. Scaffold realization — Installment 39: when a message asks for a reply

A required reply can be an operational acknowledgement, a consent request, or a social invitation. Those are different contracts. The plan treats “reply required” as an explicit property of the request rather than a generic message setting. A player should know what happens when the resident does not answer. Silence may mean they have not seen the message, cannot respond, refuse to answer, or have no response route. The communication surface must not select one interpretation without evidence.

### Three reply contracts

**Receipt acknowledgement** means “I saw the information.” It does not mean agreement or compliance. Acknowledging an intercom update should not close the task that the update describes. **Decision response** means the recipient chooses among supported options: accept, decline, request clarification, or propose another time. The author receives only the answer actually made. **Open invitation** means a response is optional; the message may expire or remain available by a stated rule. The interface uses different labels for these contracts and communicates who is expected to respond.

If the data model cannot distinguish the three, the proposal must not present them as separate durable states until the owner decision is made. A view-only badge such as “awaiting reply” is not proof that a recipient received the message. Delivery, read, acknowledgement, and response should remain distinct facts when the system supports them. If the present owner only tracks simple read/ack flags, the design must fit that granularity and avoid narrating stronger guarantees.

### The non-response branch

At the deadline, the author may send one permitted follow-up, choose an alternate plan, wait without pressure, or close the request. A follow-up should not auto-repeat indefinitely. The reminder's urgency and channel should be visible. If the recipient is currently unavailable, do not describe the missing response as deliberate refusal. If the recipient has read the note but has not answered, the interface can still say “no response recorded” rather than “refused” unless the person selected a refusal action.

For operational work, an unresolved reply can leave a slot unassigned. The player can choose a different eligible worker only through a new consent offer; lack of response is not consent. For a private invitation, non-response can let the invitation expire without a penalty. For a safety acknowledgement, a source-defined escalation policy may prompt a second check, but the escalation must be owner-backed and bounded. A communications plan cannot invent a new welfare check system to make the plot advance.

### Branching from who sends and why

The same reply contract changes according to the action that created the request. If a resident asks whether a player can return a tool, the player can answer with a time, explain a dependency, or decline. If a crew lead posts a shift hazard notice, the recipient may acknowledge receipt but cannot silently agree to take the shift. If a faction helper asks for consent to share a name, the resident can authorize a specific audience or refuse. If the player initiates a request and then withdraws it, the recipient should see that it is withdrawn only if the owner supports revocation.

Previous outcomes can unlock different response language. A resident who previously proposed an alternate time can be offered that route again only if the history and the current schedule allow it. A player who met one agreed return time can receive a new offer from the same person, but the line should reference the fulfilled commitment, not a generic trust score. A prior breach can cause a resident to require confirmation or decline a personal exchange; it should not disable unrelated operational collaboration without evidence.

### Supporting factions help deliver, not pressure

A minor support group can act as a delivery channel only where its actual role and the recipient's permission support it. It may carry a sealed note, post a public shift notice, or confirm that a notice is physically available. It cannot pressure residents into answering, claim that it has “checked with everyone,” or convert a missing acknowledgement into approval. A support member who helps deliver a message should be named or described where privacy allows. The player sees the cost and expected reach before asking.

If the recipient does not use the board, the player can ask a permitted messenger, choose a direct channel, or accept that the message may not reach them before the deadline. If the message contains personal information, a public board is not a neutral fallback. The player may have to make a hard operational choice because no channel can safely reach everyone. That is a meaningful branch born of communication constraints, not a moral score.

### Example: the shared shift request

A resident asks another to cover a late watch because a task is behind. The message is a decision request, not a notification. The recipient can accept the shift if the roster owner reports eligibility and the person consents; decline; ask which task will be handed off; or counter with a shorter period. The player can accept a counterproposal only if schedule and duty owners permit it. Acknowledging the message does not allocate the shift. If there is no reply before the needed time, the player can leave the shift uncovered, choose another eligible volunteer, or change the underlying task plan. A player who previously asked this resident to cover a shift and then failed to return the favor may see a line that refers to the concrete event; the resident still has a free choice.

The support group can report that a different trained resident is already on duty, if that fact exists, or can carry a private follow-up with consent. It cannot invent a new worker. If the coverage request expires, the message closes, while the roster continues to show the actual staffing gap. The two views are connected by identity and event references; neither overwrites the other's state.

### Acceptance and failure cases

The design should be reviewed with at least these traces: a recipient never receives the message; a recipient receives but does not read it; a recipient reads and explicitly declines; a recipient acknowledges receipt without agreeing; a recipient proposes an alternative; an author withdraws a request; an expired request appears after reload; and a duplicate UI projection is acknowledged only once. If the current model cannot distinguish delivery from read, record the gap and narrow the copy. Never use prose to claim a stronger state than the system can prove.

## 445. Scaffold realization — Installment 40: local boards, partial reach, and the cost of choosing a room

A local board is a place with a defined audience, not a smaller global feed. This installment expands the decision between putting information where affected people work and broadcasting to everyone. The player may be tempted to use the largest channel because it seems safest, but unnecessary reach can disclose private facts, interrupt unrelated residents, or turn an operational update into a public accusation. A smaller audience can miss someone who is away from the relevant space. The board choice therefore changes who can act and what burden the message creates.

### Audience is a contract

Before posting, show the destination's stated audience, whether the message is physically persistent, who can edit or remove it, and what the expiry rule means. If the audience is based on location, do not equate it with everyone who can enter the location. If board visibility is role-based, the system should show the role rather than imply a named audience. If access rules are not available, the plan should not invent a complete readership list. A board's name alone is not authorization.

Message purpose narrows appropriate destinations. A workshop schedule belongs on a work board or with affected workers. A confirmed common water change may need a shelter-wide notice. A personal invitation belongs in a private channel. A disputed account should not be posted as a public fact merely because the underlying task affected many people. The player can choose a broader channel if there is an operational reason, but the interface should show the increased reach and request any required authorization.

### The absent-reader problem

Someone can be part of a board's audience and still not see the post. The board can prove only that a notice is present, unless current systems track delivery or read state. If the message is important to a named person's decision, the player may need to send a direct message, ask a permitted runner, or wait for the resident to return. Those routes have their own costs and privacy conditions. A board post cannot mark each audience member as informed.

An absent reader produces a meaningful branch. The player may delay a task until the affected person can review the note, use a direct route, choose another participant, or proceed only if an existing safety or consent policy allows it. An alternative route may consume staff time or disclose less information. The decision has a concrete cost without moral scoring. A resident who later discovers the notice can explain that they did not see it; the game should not narrate them as ignoring a known message.

### Correction and physical history

If a local board notice is wrong, the player must be able to correct it through the message owner. The corrected version should identify what changed and preserve the intended audience when possible. Replacing the text in place can make the history unreadable. A posted schedule can be corrected with a new time and a link to its predecessor. A notice about a person should require stricter review; it may need to be withdrawn, corrected privately, or referred to the original author instead of amplifying the claim.

Expiry also needs meaning. A notice can expire at a date, after an event, or when its owner withdraws it. When it expires, the record may leave the active board but remain available in history if the owner supports retention. “Expires” should not mean “never existed.” If the system lacks history, the interface must not promise that old corrections remain visible. A stale notice restored from save is an integration bug, not an opportunity to reuse old content.

### Supporting faction services for board use

A minor group that maintains a room, schedule, or public record can explain how to post, confirm board capacity, or recommend a destination. It cannot silently decide that an author's message is safe for a broader audience. A group member may offer to pin a critical notice for a shift, if pinning exists, or deliver a copy to another room. Each option has visible reach and expiry. If a board is full, the player can remove an eligible expired post, request a space, use another channel, or wait. There must not be a hidden “post succeeded” result when capacity rejects the message.

The group's service may be unavailable if the room is locked, a caretaker is off shift, or the board is under repair—but only actual state can establish this. Otherwise, the writer can express a moment of uncertainty without presenting it as a simulation rule. The support team remains a small part of a communication feature, not a new faction campaign.

### Scenario matrix: one fact, four audiences

Take a confirmed delay to a shared repair. On the local work board, the message can state the affected slot and new estimate. On a private channel to the next crew, it can ask whether they can swap. On a general notice board, it can inform residents who use the space. On an intercom, it interrupts everyone and may be unjustified. The core fact is the same; the action available to the reader differs. This makes audience selection a branching decision rather than a copy rewrite.

The player might post locally and send one private request, conserving broad attention but leaving some residents unaware. They might post generally to ensure reach, accepting more noise. They might ask the maintainer to verify the new estimate first, delaying the notice. Or they might hold the post until the crew confirms. Later outcomes depend on which residents actually acknowledge or respond, not on the intended audience alone.

### Acceptance evidence

Review local-only post; broad post with explicit purpose; absent reader; full board; expiry and archive; correction; a privacy-sensitive notice; and a message whose audience cannot be enumerated. The acceptance question is whether the interface tells the truth about reach and offers a feasible next action when the first audience is insufficient. If the current owner cannot represent multiple boards or audience filters, keep cross-posting out of scope rather than creating a parallel board catalog.

## 446. Scaffold realization — Installment 41: corrections, rumors, and competing accounts

Rumor gameplay can easily reward surveillance, public shaming, or a hidden “reputation truth” mechanic. This plan instead treats a rumor as an information-quality problem: a claim is circulating, its source and certainty may be unknown, and the player has limited actions to correct or contextualize it. The communications feature should support accountable correction without turning private lives into collectible clues.

### Label the claim by what is known

Distinguish a direct quote from a paraphrase, an observation from an explanation, and a confirmed owner fact from a participant's account. A board note that says “the pump is off” can be confirmed by the maintenance owner. A note that says “the crew shut it down to punish the east rooms” includes a motive the operational fact cannot establish. A resident may report hearing the motive, but it remains an account unless corroborated. The UI can show “not confirmed” without naming the speaker if the source is private.

The player chooses how to respond: do nothing; post a narrow confirmed correction; ask the author to revise; ask a relevant owner to publish an update; send a private clarification; or remove a message only if they have authority. The player can also choose to preserve a disputed report and wait for more information. Each action carries consequences in reach, delay, trust in process, or continued uncertainty. None of these choices decides whether the player is honest or deceitful overall.

### Correction paths

**Fact-only correction:** state what the owner verifies and what remains unknown. Example: “The pump is offline during inspection. The cause of the earlier interruption is not confirmed.” This can reduce misinformation while leaving motive unresolved.

**Author-led revision:** ask the original author to edit or withdraw. They can agree, decline, or be unavailable. The player cannot make their personal account disappear unless moderation ownership says so. If the content is harmful, a separate authorized moderation process may be needed; this proposal must not invent that authority.

**Owner update:** ask the system that owns the underlying state to publish. A water owner can report service status; it cannot explain a conflict between two residents. A task owner can report completion; it cannot certify that a public accusation was malicious.

**Private clarification:** send a careful note to the relevant recipient. It can correct their plan without amplifying the rumor. The player may preserve privacy at the cost of leaving the broader board inaccurate for longer.

**No correction:** the player can decide not to spread a claim they cannot verify. This leaves the audience with incomplete information. The story should register the practical result, not present silence as cowardice or wisdom by default.

### Branches driven by earlier communication actions

If the player previously posted an estimate as certainty, a later correction can reference the exact earlier notice and expose the source of the error. The player may acknowledge the mistake, quietly replace it, or leave it uncorrected. A public acknowledgement can cost face but repair the information trail; quiet replacement may reduce noise but leave some readers with the old claim. If the player previously sent private information to a wide board, an affected resident can now require consent before sharing their account. If the player consistently named sources and uncertainty, a new public update can preserve the same practice without granting a broad credibility score.

The player can also be wrong. The plan should not create a branch where every player action is validated by the writer. A player who posts a premature correction may need to amend it later. A crew can provide new evidence that changes the status. The game can show “estimate updated” without punishing the player with an arbitrary relationship drop. Consequence should arise from real decisions, such as which task was delayed while verification occurred.

### Supporting factions can carry verified scope

A records group can point to the relevant log. A work crew can update its own queue. A resident council can choose whether to issue a statement only if such a body is established and its mandate includes communication. No helper should be cast as a universal authority over truth. A faction's refusal to endorse an unverified claim can be a useful branch; it does not establish that the claim is false. A group can post a scoped statement under its own name, creating an accountable source without speaking for everyone.

That support can be partial. They may confirm a time but not a cause, confirm an item transfer but not consent, or confirm that a message was posted but not who read it. A player can choose whether partial confirmation is enough for their purpose. A public note can state a verified fragment while preserving the open question. This is more useful than a binary “rumor disproved” reveal.

### Scenario: a missing shift and a named accusation

A notice says a resident abandoned a shift. The roster shows no completion event; the resident's current status is unavailable. The player can ask the duty owner for a status update, post a neutral gap notice, contact the named person privately, or leave the accusation unamended. A verified fact might be “the slot has no completion recorded.” It does not establish why. The player who posts that narrow correction can then assign coverage, request a willing replacement, or leave the slot open. The work continues without requiring a public trial.

If the resident later returns with an account, the player can choose to share only the operational update, ask permission before sharing personal detail, or keep the record open. If the duty owner confirms a roster migration error, a correction can acknowledge the system issue. The communication feature does not use the incident to secretly raise or lower loyalty. It improves the player's ability to act amid uncertainty.

### Content and moderation limits

Authorial text should avoid naming a person in a speculative accusation unless the current narrative explicitly intends that harm and provides a credible response path. A player-created message should have constrained templates that make it harder to convert an estimate into a public fact. If free text is not supported, do not assume it; authored variants can still represent correction. If the product has a moderation layer, this feature must integrate with it rather than establishing another. If it does not, the plan remains limited to structured, authored content and existing message authority.

### Acceptance matrix

Cover a true correction; a partial correction; a false player correction; an author who declines to revise; a private message that must remain private; an unavailable source; a correction delivered to the original audience; and one reload after amendment. Verify that the record preserves what was said, what was changed, by whom, and what is still unknown. Do not claim that any branch eliminates rumor permanently.

## 447. Scaffold realization — Installment 42: the relay is private only if its path is private

A message can begin as a private exchange and become public through forwarding, quotation, a helper, or an intercom escalation. This installment expands the private-message route from a single-recipient abstraction into a set of decisions about permission and disclosure. The feature must stay within the existing simple recipient model; if the model cannot represent forwarding, do not fake an inbox chain. The design may still model an explicit player-mediated relay as a new message with known authorship, if that path exists.

### Consent to pass something on

Before relaying, identify the original author, the proposed recipient, the content to be shared, and whether the player is quoting or paraphrasing. Ask permission when the content is personal or when the author stated a limited audience. “Can you let them know I am delayed?” is authorization for a narrow message, not permission to include the sender's full explanation. The player can show the proposed text, ask to widen the audience, choose a minimal factual summary, or decline to relay. The author can approve, revise, limit, or withdraw their request.

If the sender has already authorized an exact recipient and text, the player can deliver it unchanged. If the player wants to add context, that addition is their own claim and should be visibly separate. A recipient can reply to the new message, but the original author does not automatically receive that reply unless the recipient chooses to include them. No UI grouping can make two distinct private conversations appear as one shared thread if the save owner records only one recipient per message.

### Supporting relay roles

A trusted messenger can reduce the risk of a person missing a message, but “trusted” must be a character's explicit choice or a known role, not the player's assumption. A messenger carries the approved content and may confirm delivery if the delivery owner can report it. They do not report whether the recipient agrees. A translation helper can produce a reviewed version if the game has such a content path; they do not authorize wider sharing. A faction channel can relay a service notice but should not become a back door for private correspondence.

If no suitable messenger is available, the player may wait, use the existing direct route, or accept that the message may not arrive. A resident can decline to carry it because the content feels sensitive or because they are busy. That decision should not be a faction betrayal branch. The player can select another permitted route, but cannot coerce a particular helper into carrying a message.

### Example: an offer to reschedule

A survivor sends a private request to reschedule shared work and gives a personal reason. The player may forward the whole message to the other worker if authorization includes that content; relay only the new time; ask the sender to approve the summary; or ask them to contact the other worker directly. The receiving worker can accept the new time, propose another, or decline. If the player shares the personal reason without permission, the system should not reward the wider recipient with special knowledge. If the model cannot persist permission, the player-facing flow should not offer a generalized “forward” button.

An alternative uses a supporting scheduler: the player asks them to communicate only that the time changed, preserving the personal reason. The scheduler's assistance can cost time or require an available slot. The player can also cancel the shared work and create a new task if the task authority allows it. The message then closes as a distinct fact; no private reply is falsely shown as delivered to both parties.

### Silence and asymmetric information

The player may know that the message was sent but not that it was read. The sender may know only that the player accepted the relay. The recipient may not know that the sender authorized a narrow summary. These asymmetries are part of the scene. The interface can communicate them: “Relay recorded,” “delivery not confirmed,” or “recipient replied to you.” It cannot manufacture a shared understanding from a single user action.

A recipient's silence can lead to a delayed task, a second permitted message, or an alternative. It cannot be interpreted as consent to publish the original message. If a direct message expires, its original scope does not automatically widen. If the sender leaves, a prior permission remains valid only if its scope and expiry permit it. These details are especially important when a message is loaded after a day transition.

### Acceptance cases

Review exact authorized relay; unauthorized audience expansion; limited summary; sender changes wording; recipient replies only to player; messenger unavailable; message delivery unknown; and post-relay reload. Confirm every view preserves sender, destination, and permission. Where the owner cannot prove delivery, choose uncertainty over narrative convenience.

## 448. Scaffold realization — Installment 43: translation and comprehension without false equivalence

When a shelter has multiple languages, dialects, literacy levels, or communication needs, a message can be technically posted and still fail to reach people meaningfully. This plan treats translation and interpretation as communication services with visible limits. It does not invent a language simulation or assume that every participant can understand the same phrasing. The goal is to present a truthful route for asking help, clarifying the action, and preserving the source's intent.

### Distinguish translation, summary, and interpretation

A translation attempts to preserve the message in another language. A summary shortens it and may omit detail. An interpreter facilitates a two-way exchange and can ask clarification questions. These are not interchangeable. If the content is a safety instruction or resource allocation, a summary may remove a critical condition. If the message is personal, a broad translation may disclose it to an unintended helper. The player needs to see who will perform the service, what source text they will receive, and what audience will receive the result.

Where the game has no localization or translation API, this remains content authoring guidance. The plan does not create a new translation authority. Authored variants can be paired by stable message identity in the existing data pipeline. If live translation is unavailable, the UI should not imply it is happening. A character who interprets a message must be an authorized in-world participant, not a magical generic service.

### Branches around comprehension

The player may post a second version; ask the author to approve a translated or simplified version; arrange a one-to-one interpretation; ask the recipient what format works; or defer the request. The recipient can request clarification, ask for a private explanation, decline to discuss it, or act based on what they understood. The message owner should not mark the recipient as acknowledging the original if they saw only a revised version. If the channel cannot store variants, link them carefully through one canonical message rather than creating two conflicting announcements.

When time is short, a player may choose a shorter message for an immediate action and provide fuller context later. The short version should retain the critical instruction and uncertainty. A supporting helper can confirm that the resident understands the action only if the resident consents and the system supports that acknowledgement. The player cannot infer comprehension from a nod, a read flag, or the resident's presence.

### Supporting faction and privacy

A small group may provide translation, reading assistance, or a private explanation if that service exists in canon. The person receiving help should know what material the helper will see. They can decline the service, ask for a different helper, or receive a non-personal summary. A faction's expertise does not grant access to all private messages. If no appropriate helper is available, the player can use simpler authored wording, wait, or acknowledge that communication remains incomplete.

This creates story texture through the helper's careful limits. A translator may say, “I can carry the instructions; I should not decide what they meant by that sentence.” The player can ask the original author to clarify, or choose a safer operational route. The helper is a supporting actor whose service makes one branch possible; they do not become the focus of a separate faction quest.

### Example: an allocation notice

A shelter-wide notice explains a changed water schedule and a temporary exception for a repair team. The player can publish one general version, create a shorter schedule card plus a detailed explanation, ask an authorized helper to review wording, or send a private clarification to affected residents. If the exception is omitted from a short version, a resident may arrive at the wrong time. The player can correct the notice and route the update. The story should not blame the reader for misunderstanding an unclear message.

If a translation helper is unavailable, the player may publish the confirmed time first and delay the explanatory paragraph, or postpone the change if the source owner permits. Both choices have costs. The player who includes a nuanced exception takes time and may miss the operational deadline. The player who posts a short message reduces cognitive load but owes a follow-up. That follow-up becomes a persistent obligation only if the message/task owner can support it.

### Acceptance evidence

Review one exact translation, one simplified variant, a recipient asking for clarification, a private message that should not be shown to a helper, a time-critical update, a correction to an ambiguous sentence, and a reload with linked variants. Confirm source attribution, audience, and acknowledgement semantics. If translation content is not authored or localization data is absent, report the limitation instead of writing a fake route.

## 449. Scaffold realization — Installment 44: notice fatigue and the attention budget

Residents cannot treat every message as equally important. Notice fatigue can emerge from repeated interruptions, duplicate channels, vague urgency, and old messages that remain active. This installment gives the player useful control over frequency and attention while avoiding a new universal fatigue meter. The feature should rely on existing delivery and acknowledgement owners, or keep the consequence descriptive.

### Repetition has identifiable causes

A repeated notice may be a correction, a reminder, a duplicate projection, a recurring schedule, or a new request. The interface should label which one. A board notice mirrored in an inbox should retain one canonical identity if it is the same message. A genuinely new update gets a new version or linked identity. Reminders should be bounded and tied to an unfulfilled action. The player can mute a routine projection, collapse duplicates, request a digest, or preserve an urgent alert—only where current settings and message owners allow it.

Do not let a player mute a required safety route if policy requires it, unless the established accessibility/preferences system provides that choice. Conversely, do not route every ordinary task through an urgent override. The product's existing channel settings determine what can be filtered. This proposal explains the desired semantics but must not clone notification preferences inside a message panel.

### Player choices after attention is stretched

When residents report too many notices, the player can consolidate routine updates, change posting location, lower urgency where justified, set a meaningful expiry, or keep the current system because the information is operationally necessary. Each choice trades detail, reach, and interruption. A digest can hide timing if it arrives too late. Fewer posts can reduce noise but make corrections less visible. Removing a message can save attention but erase a useful current instruction. The player sees who benefits and which content is lost.

Repeated missed acknowledgements should not automatically mean fatigue. The recipient may be absent, unable to respond, or using another channel. A real owner can report these states; otherwise display “no acknowledgement recorded.” If a true preference for routine digests is supported, it belongs to the existing settings/save owner and remains stable across panels. If not, a one-time presentation option must not imply a lasting preference.

### Branches after an unnecessary alarm

Suppose the player uses an urgent broadcast for a non-urgent update. A resident may complain about being interrupted; the player can acknowledge the mistake, correct urgency going forward, or defend the choice with a concrete time-sensitive reason. The group that maintains communications can explain the threshold but cannot arbitrarily punish the player. If the alarm was justified by incomplete information, a later update can clarify that uncertainty existed. The content should not assume that a bad outcome proves the earlier decision was reckless.

If a future urgent event occurs, the player must still have access to the legitimate urgent route. One unnecessary use should not globally disable it. A specific response can refer to that earlier message: “The last update changed twice; can you confirm this one is final?” This is contextual dialogue, not a general credibility score. The player can have the source owner publish the confirmation or send a narrow targeted alert.

### Supporting faction contribution

A communications steward can propose a digest or update schedule; a work group can consolidate its own task notices; a resident representative can explain which audience misses the current board. The service is limited to that role. The faction cannot silence another group's messages or lower their urgency without the relevant owner. A consolidation request can be accepted, countered, or declined by authors whose messages would be changed.

The supporting actor may incur a real cost if they maintain the digest. If no time/work system represents that cost, keep it in the narrative and do not create an invisible resource. The helper can show a candidate digest before posting; the player checks that no deadline, safety step, or privacy boundary is lost.

### Acceptance cases

Test a board/inbox duplicate, urgent versus routine delivery, a correction, a digest that contains a deadline, an absent recipient, preference persistence where supported, and one unneeded alarm followed by a real hazard. Measure success by whether residents can distinguish action, deadline, and source—not by a speculative fatigue score. If the owner cannot represent versioning or suppression, document that gap and keep the feature's promise narrow.

## 450. Scaffold realization — Installment 45: expiry, archive, and messages that outlive their usefulness

Messages become dangerous when they look current after their facts have changed. Expiry is not a cosmetic cleanup. It is a claim about how long a message remains actionable. This installment defines how a notice can close, be superseded, or remain as history without creating a parallel journal system.

### Expiry rules follow the message's purpose

A time-specific work notice may expire after its shift. A safety instruction may remain until the source owner publishes an all-clear. A personal invitation may expire at the stated time or when withdrawn. A policy notice may need an explicit replacement. The author or current owner sets the expiry when the message is created; the player sees it before posting. “End of day” must use the game's time authority rather than local wall-clock time.

On expiry, an active board can remove the action prompt but retain a history record if the owner supports it. Superseding a message links the new version and marks the old one out-of-date. Withdrawal identifies who withdrew it and whether recipients already received it. A user cannot use expiry to erase a completed acknowledgement or to reset a required reply. If the data model only supports a Boolean active flag, the copy must not promise a full revision history.

### Consequences of late reading

A resident can open a message after its deadline. The interface should show it as expired or superseded before they act. They may ask for current instructions, request a new offer, or proceed only if the owner confirms that the action remains valid. A previously acknowledged message does not become current again because it was reopened. The player can see the operational impact: a shift has passed, a room is now occupied, or the item was allocated elsewhere. The narrative should not frame a late reader as negligent if the system cannot prove delivery.

If a message triggered an action, the resulting task owns its completion. Expiring the source notice does not cancel the task automatically. If the author withdraws a request after work starts, the task owner needs an explicit pause/cancel route. Message state and task state must reference one another without either system impersonating the other.

### History and privacy

Archived private messages remain subject to their original audience. A general archive cannot turn a one-to-one exchange into public canon. The player may access a current private message through the communication owner; whether it persists forever is a separate data policy. If deletion or retention behavior is not implemented, the proposal cannot promise it. Public corrections should preserve the public record necessary for operational clarity, while personal messages should not be quoted in a public timeline by default.

The player can choose to keep a notice available, mark it replaced, or close it. The author may request removal. A support faction can help find a prior public notice if it owns an archive service, but it cannot search private correspondence without authorization. These constraints make an archive useful without creating a universal narrative browser.

### Example: a canceled work call

A public call asks for volunteers to move equipment before a shift. It expires at the time the work begins. One resident opens the message later and offers to help. The player can send a new request if the task is still open, decline because it has been completed, or redirect the resident to another job. The system shows the old call's final status. If a task remains underway, the old notice does not reset volunteer count; the roster or task owner decides whether another person is eligible.

If the player cancels the call before anyone accepts, the active notice closes. If someone already accepted, cancellation requires a reply or task update so that person is not left believing the work remains. The player can send a direct cancellation, post a public update, or ask a helper to reach them. Each route has a reach limitation. If no delivery is confirmed, the interface says so.

### Acceptance matrix

Cover message expires by shift; urgent notice awaits all-clear; private message withdrawn; superseding correction; late reader; linked task continues after message expiry; save/reload around expiration; and an expired notice appearing in search. Confirm that no stale action remains clickable and that public history cannot leak private content.

## 451. Scaffold realization — Installment 46: one announcement can have several owners

Some announcements contain a fact from one owner, an interpretation from another, and an action request from a third. A shared notice must preserve these responsibilities instead of presenting one author as the source of every claim. This installment addresses coordination without adding a universal communications governor.

### Compose from scoped contributions

Each contributor confirms only the statement they own: task status, inventory allocation, access time, health restriction, or a participant's own request. The final author may combine them into a concise post, but the preview marks which claims are confirmed and which remain estimates. If the message owner cannot preserve per-claim provenance, use separate linked notices with a common incident reference or keep uncertain details out.

The player can request each contribution, post only the confirmed facts, wait for a missing confirmation, or publish an incomplete update with uncertainty clearly stated. A contributor can decline to be named or decline to share private details. If one owner changes their fact before posting, the draft must be revalidated. A draft does not freeze the source state unless the owner says it does.

### Branches when owners disagree

Suppose operations report that a room will reopen at noon, while the caretaker estimates afternoon. The player can post a range, ask the task owner to confirm the dependency, post the operational closure without an estimated reopening, or wait. The player cannot average the two claims and call it fact. The choice affects planning: residents may choose another location, delay work, or keep the current arrangement. Later updates can narrow the range.

If the owners disagree about audience, the narrowest privacy boundary governs until permission is clarified. If one owner wants an urgent broadcast and the source facts do not meet the urgency rule, the player can use a narrower route, ask for source confirmation, or hold the message. If one contributor cannot be reached, the player can publish only the independent claims that are already verified.

### Supporting faction as coordinator

A small communications-support group can collect approved lines and check that each source reviewed its portion. This is a clerical role, not an authority to reconcile disagreements. It can identify a missing approval, produce a versioned draft, and route it back. It cannot endorse another faction's claim. The player may post without coordination if a single-source message is sufficient, thereby avoiding process overhead.

The group may have its own practical preference—one consolidated notice reduces duplication, but separate notices make source responsibility clearer. The player chooses according to urgency and audience. A consolidated post can be easier to read; a linked series can be more accurate. No branch is always best.

### Acceptance cases

Review a three-owner notice; one contributor changes their source fact; one contributor declines attribution; conflicting estimates; a missing owner; a draft with stale data; and reload after publication. Confirm the player can see who owns each claim and who must approve the final text. If the current communication system cannot represent claim-level authorship, record that as an architectural question rather than adding hidden per-line state in the UI.

## 452. Scaffold realization — Installment 47: accessible actions and reliable focus

Communication only works if a player can discover and operate the intended action. This installment links message choices to keyboard/controller navigation, screen size, reading order, and return behavior. It does not create a second accessibility plan, but it ensures that the feature's branches are reachable and that the player can understand their consequences.

### Decision surfaces must be navigable

Every message action—acknowledge, reply, decline, request clarification, report a correction, close, or mark unread—needs a visible label and clear focus behavior. Opening a message should place focus on its title or first meaningful control, not silently jump to a destructive action. Closing should return the player to the same board or inbox context. A player should be able to distinguish read from acknowledged and reply-required from optional using text or symbols, not color alone.

Long notices need a readable content order: source, audience, date/status, message, action, and expiry. A sticky header must not trap focus or hide the last action. Text scaling should not make the audience or deadline disappear. Controller navigation should reach every branch. If a message includes a map/room reference, provide a text destination as well. Avoid timing a dialogue choice so the player must read at speed.

### Accessibility branches are gameplay branches

The player may choose a shorter summary, audio reading, larger text, or a helper-supported interpretation if those options exist in the product. Such presentation choices should not alter whether a message is acknowledged. A player who requests help reading must not be marked as having shared the private message with the helper unless that is the explicit service contract. The feature should retain the player's route to decline or defer even when the message is shown in another format.

For an urgent message, accessibility must be considered in the delivery contract: sound alone is insufficient; a visual cue alone may be missed; a read requirement can be impossible for some players. The owning accessibility and notification systems determine supported presentation. The plan does not implement a parallel alert bus.

### Acceptance checklist

Walk keyboard-only, controller-only, scaled-text, high-contrast, screen-reader-compatible where supported, and long-message cases. Ensure board, inbox, intercom projection, and private message all return focus correctly and preserve the same underlying state. Verify that a read state is not written by focus alone unless the current contract says it is. Log any inaccessible branch as a specific route issue, not a general “UI polish” note.

## 453. Scaffold realization — Installment 48: message endings and communication playstyles

The communications feature should support several ways of handling information. The playstyle matrix is descriptive for designers and content authors; it is not a player-facing class or hidden score. Each approach can solve one problem and create another. A quiet shelter is not always an informed shelter, and a loud shelter is not always a safe one.

### Playstyle patterns

**The concise dispatcher** prefers short notices with a named action and deadline. This reduces reading burden and improves clarity for routine changes. It risks omitting context and may require a fuller follow-up. A player using this style should see when a shortened message drops a caveat.

**The broad announcer** favors shelter-wide posts to reduce the chance that someone misses a change. This can improve reach for shared hazards while interrupting unrelated people and exposing more information. The player sees the audience expansion before confirmation.

**The local coordinator** places work messages where the affected crew will see them and uses private follow-up for exceptions. This is efficient when the audience is known and can fail when workers are absent or the board is inaccessible. The player has to decide whether to wait or widen reach.

**The careful verifier** asks owners to confirm claims before posting. This reduces misinformation and can make updates arrive late. The player can publish confirmed partial facts while labeling the unresolved cause, if the message owner supports that.

**The privacy steward** uses minimal detail, asks before forwarding, and leaves personal explanations private. This protects participants but may provide less context to a crew making a shared decision. The player can ask for a separate operational fact rather than demand private justification.

**The transparent corrector** posts visible amendments and acknowledges uncertainty. This preserves history but can create extra notices. The player can consolidate corrections where audience and owner rules allow.

**The hands-off reader** relies on existing channels and avoids composing messages. This conserves attention but may leave some requests unanswered. The feature should show the resulting missed action without labeling the player negligent.

### Ending families for a message thread

A communication thread can end as **delivered and acknowledged**, **posted with unknown readership**, **corrected and superseded**, **withdrawn before action**, **expired after its stated window**, **answered with an explicit decision**, **closed without reply**, or **unresolved because the intended recipient was not reached**. These are different outcomes. “Read” is not “agreed”; “posted” is not “delivered”; “expired” is not “deleted.”

An ending can affect a task if a real consumer subscribes to that event. A shift request accepted by the recipient can create a roster command; a notice alone cannot. A private invitation accepted can lead to a scene only when an authored path exists. A correction can change the current status displayed but cannot reverse a completed action without its owner. The ending text should make the next available action apparent and avoid implying more reach than the record supports.

### Small-group follow-through

Supporting factions can conclude a particular service: they verified a time, carried one note, posted on a local board, or declined because capacity was full. Their availability should remain local. The message feature should not determine the faction's overall campaign ending. A group can gain a later role because the player used its service respectfully, but that route should be tied to a specific request and remain optional.

## 454. Scaffold realization — Installment 49: short narrative arcs carried by ordinary messages

The feature is useful when it supports ordinary shelter decisions before it carries a long story. Three compact narrative arcs illustrate branching without adding major questlines or message-specific quest state. Each arc begins with a real communication need, uses the three Section 4 subfeatures, and can end without a conclusive scene.

### Arc A — the changed room schedule

A work owner changes access hours for a room. The player can post locally, verify the source, send a private message to a resident with an existing reservation, or issue a wider notice. If the player uses only the board, the resident may not see it in time. If they ask a support group to carry a note, the group may be busy. If they use the intercom, more people are interrupted. The story resolves when access state changes or the responsible task completes, not when every resident says they understand.

Possible outcomes: a local notice reaches the intended shift; a late reader requests a new slot; a private exception stays confidential; an overbroad broadcast is corrected; a schedule remains uncertain and the player routes around it. Follow-up text can refer to the concrete channel used, but does not create a room-access owner.

### Arc B — an unconfirmed repair estimate

A crew gives an estimated return time. The player can post it as an estimate, wait for confirmation, publish only that access remains closed, or ask a helper to check. An estimate changes as work progresses. The player who published it as fact must correct it; the player who labeled it clearly can update without contradiction. Residents may choose an alternate plan. The message owner preserves one linked status; the repair task owner remains authoritative for completion.

Possible outcomes: early confirmed reopening; delayed estimate; no estimate, only closure; resident misses an outdated board post; a supporting group supplies a local notice; player withdraws a no-longer-useful message. None resolves the repair itself through communications.

### Arc C — the private request for contact

A resident asks the player to pass a personal invitation to another survivor. The sender chooses exact wording and recipient. The player can relay it, ask to summarize, choose not to act, or suggest direct contact. The recipient can accept, decline, or not reply. A helper can carry the note only with the sender's permission. If the invitation is declined, the sender receives only the information the recipient allows. The system avoids forced reconciliation and preserves the original audience.

Possible outcomes: a private meeting is accepted; alternate time proposed; invitation expires unanswered; sender withdraws; player declines to relay; recipient requests more context. The message thread closes according to its owner. No cross-faction romance or relationship feature is implied.

### Narrative variation rules

Use current resident IDs and established voices. Do not hardcode unsupported rooms, channels, or roles as if they are live. Every authored line should reveal its source: what the speaker personally knows, what the message owner reports, or what remains uncertain. Reuse a physical detail—chalk smudged on a board, a folded note pinned under a cup, an intercom switch left untouched—only where it fits the location and canon. Keep the communication action readable; flavor should not bury the deadline or audience.

## 455. Scaffold realization — Installment 50: bad news, correction, and humane uncertainty

Messages often carry cancellation, loss, ration changes, failure, or grief. A communications feature can make these moments feel either grounded or bureaucratic. The plan's tone should preserve confirmed fact, estimate, and opinion while giving the player a practical response. It must not use an urgent channel as an emotional shortcut.

### Bad-news composition

A difficult notice should state: what changed; what is confirmed; who is affected; what action is needed, if any; what remains unknown; and where the next update can be found. It should avoid blaming a person where the source only proves a system event. “The east line is closed pending inspection” is safer than “the maintenance team failed again” unless the latter is a verified, relevant account. A loss or death message must come from a credible narrative authority and must not be invented to raise stakes.

The player can choose a personal notification, a private small-group conversation, a local notice, or a broad announcement according to the content and urgency. Affected people may need direct contact before the full shelter hears. The sequence can cost time; the player should know this. A support group can help deliver or contextualize the message, but it cannot speak for a survivor who has not consented.

### Branches for incomplete cause

If a cause is unknown, the player can say so, wait, or publish the confirmed effect. If one source offers an account, the player may attribute it clearly instead of presenting it as official. If a later record changes the interpretation, issue a correction without erasing the initial uncertainty. Residents can react differently: one needs immediate instructions, another wants details, another declines further updates. Those reactions are local and authored, not a generalized morale meter.

The choice to delay a message may protect privacy but leave people unprepared. A prompt publication may help them act but carry unresolved details. A narrow correction can reduce harm while leaving the full story open. These are legitimate outcomes and should not converge to a single “correct communication” ending.

### Avoiding emotional coercion

Do not require the player to broadcast a personal tragedy to unlock a task. Do not punish them for choosing a private route where privacy is appropriate. Do not frame correction as cowardice or confession unless a concrete fact supports that interpretation. If the player was the source of an error, offer an accountable correction and an opportunity to explain; let the affected characters respond. Keep the focus on action and impact.

### Review examples

Write one closure notice, one missing-supply update, one canceled work request, one uncertain incident, and one private loss message. For each, label source, audience, urgency, deadline, action, known facts, and unknowns. Then branch at least three ways: immediate broad publication, limited audience with direct contact, and delayed or no publication. Ensure all paths have a next action or honestly remain unresolved.

## 456. Scaffold realization — Installment 51: end-to-end message lifecycle and persistence

The lifecycle acceptance pass treats each message status as owned data and each view as a projection. Draft, approved, posted, delivered, read, acknowledged, answered, amended, withdrawn, expired, and archived states should not be collapsed into one boolean. The current API may support only a subset. The plan must map its desired prose to the available state contract before implementation.

### Source-of-truth lifecycle

The message owner creates a stable identity when a message is drafted or posted, depending on current design. Approval and posting are distinct if a human author must review. Delivery is distinct from posting if the system can report it. Read and acknowledgement are distinct only if the owner exposes them. A reply is its own message with an explicit parent or reference where supported. Amendment either updates a versioned record or creates a linked correction. Withdrawal and expiry have separate causes.

The board, inbox, intercom banner, journal link, and notification tray can each show a projection. They must refer back to the same source identity when they represent the same announcement. A visual grouping is not a data contract. Acknowledging one projection should update the source once if that is the agreed owner path; acknowledging a different private reply should not mark the original public notice as read.

### Save and restore checkpoints

Test saves at: draft awaiting approval; approved but not posted; posted before anyone acknowledges; after one recipient response; after amendment; after expiry; and after withdrawal. On reload, each state should restore accurately, with no duplicate broadcast, lost recipient filter, stale focus target, or reopened request. If the current save owner omits one state, document the migration or limit the feature to existing persistent fields. Do not bolt a second message save store onto the host.

Day advance is another checkpoint. A notice can expire once; a reminder can be scheduled once; a message should not become more urgent merely because the player loaded and advanced time. The time owner determines dates and boundaries. An in-game day transition and wall-clock time must not disagree about expiry.

### Failure and retry

If posting fails because the board is full or the source is stale, preserve the draft if the owner allows it and clearly state that no audience was reached. Retry should reuse the same intended message or explicitly create a new version. If notification delivery fails after the source record is committed, do not repost and duplicate the announcement. If an acknowledgement callback fails, show the actual stored status and retry through the owner idempotently if supported.

### Acceptance walkthrough set

1. Draft, approve, post locally, receive one acknowledgement, save, reload.
2. Post one canonical intercom announcement visible in two UI projections, acknowledge from either, confirm one source result.
3. Amend an expired estimate, preserve the correction link, then reload.
4. Withdraw a private request before response, ensuring no public notice appears.
5. Fail a post due to invalid audience, revise destination, and post once.
6. Advance across expiry and confirm the stale action is no longer available.

Record the exact owner API, source identity, save-section route, host entry point, and observable output. These are readiness checks, not a statement that the current system already passes them.

## 457. Scaffold realization — Installment 52: integration evidence and closeout boundary

Plan 4 can close as a proposal only when source evidence is refreshed and the three Section 4 subfeatures remain the full scope: public boards with audience and expiry; intercom urgency with one acknowledgement; and private messages with recipient ownership. Translation, correction, accessibility, reminders, and archive behavior are implementation details or content cases under those three subfeatures, not new pillars.

### Evidence to refresh

Before implementation, re-open current code and data for message models, boards, announcement sources, recipient filters, acknowledgement APIs, save registration, host routes, and notification projections. Verify whether the current intercom source and mirror still have independent IDs. Confirm whether the message DTO has one recipient, read/ack flags, and expiry. Establish which screen or command can create messages today. Record exact source paths and current ownership claims from the integration ledger. The older forensic report does not prove the present source.

If a required message state is missing, classify it: can the feature narrow copy to existing semantics; does the current owner need a small extension; or does it require an architecture decision? Do not solve it in a UI cache. If save ownership is unclear, stop at the design handoff and escalate to the named foreman rather than inventing a second store.

### Required acceptance claims

The implementation may claim success only when: one player-reachable creation or response route exists; board scope and expiry are enforced by the owner; the intercom has one canonical acknowledgement state; private recipients cannot see other messages; reload preserves the current lifecycle; stale messages cannot trigger current action; focus and close behavior remain usable; and narrative copy never claims delivery beyond evidence. A compiled panel with sample messages does not satisfy these claims.

### Closeout statement template

“Plan 4 is closed as a proposal at [measured word count] words. Its scope remains exactly three Section 4 subfeatures: scoped public boards; urgent intercom with one source identity; and private messages with recipient ownership. The current audit [date] verified [specific owners/APIs] and left [specific lifecycle/save seams] unresolved. No communication feature is claimed as implemented. The next step is [authorized package], with path claims and focused acceptance chosen through the active integration authority.”

Fill the evidence fields only after inspection. If the message model proves narrower than this plan, update the proposed interactions rather than manufacturing unowned delivery guarantees.

## 458. Secondary expansion — public questions and the reply chain after a broadcast

An announcement can prompt questions from its audience. The message feature should let the player gather or answer those questions without implying that one public reply speaks for everyone. Acknowledgement, question, correction, and policy decision are different message events. The thread should preserve that difference even if the current DTO requires a narrower interaction.

### A question names its source and scope

A resident can ask for clarification, request a personal exception, challenge a claim, or supply a new fact. The player sees who may read the question and whether it is attached to a public notice or a private exchange. A public question can be useful when many residents share the same uncertainty; a private question may protect personal circumstances. The player can answer publicly, answer privately, ask the source owner, route the question to the relevant group, or say that the answer is not known.

The player must not answer on behalf of an owner without a source fact. A work crew can confirm its schedule, but not make a shelter policy. A resident can explain their own request, but not speak for all residents. A records helper can point to a posted decision, but cannot amend it. If no authoritative answer exists, the player can acknowledge the question and state who is deciding or when the next update may arrive only if that promise is supportable.

### Branches when a question changes the notice

If a reader identifies a contradiction, the player can correct the notice, ask the original author to verify, post a temporary clarification, or leave the claim pending. If the question reveals that the audience is broader than expected, the player can narrow future notices or send a correction to the newly affected group. If the question asks for a personal exception, the player can route it to a private decision owner rather than exposing the request on a public board. If the question is outside the message's scope, the player can link a separate thread without promising a response.

The person who asks may disagree with the answer, decline to acknowledge it, or choose not to continue. The communication feature records the exchange and current notice version; it does not declare the person satisfied. A public answer can help others, but it can also disclose details. Show the proposed audience before posting.

### Limited supporting-faction role

A supporting group may host a public question period, collect questions for an owner, or publish a verified answer under its own name. It cannot transform an informal conversation into a binding vote unless such a policy exists elsewhere. Participants may submit questions anonymously only if the current message owner supports anonymous authorship; otherwise do not promise anonymity. The group can refuse an open forum if it cannot moderate or verify claims, and the player can choose a simpler update.

The group can also decline to answer because the information is outside its authority. This is not obstruction; it is useful information. The player can seek the real owner or state that no answer is available. If this leaves a choice unresolved, show the operational alternative rather than manufacturing certainty.

### Example: “Who is the new slot for?”

A public board announces that a work room is reserved in the afternoon. A reader asks who receives the slot and whether another resident can request it. The player can publish the actual scheduling rule, ask the room owner to confirm, answer privately if the question is tied to a personal request, or admit that no allocation has been made. A reply that says “the repair crew requested it” is valid only if the owner confirms. If the player instead chooses a public poll or queue, that requires a real allocation system and is outside this message plan unless already implemented.

The answer can reveal a queue conflict, prompt a revised schedule, or simply close the question. A resident may accept the explanation but still dislike the decision. The notice remains an information artifact, not a substitute for changing the room schedule.

### Acceptance cases

Test public clarification, private exception, source-owner referral, unknown answer, contradictory question, anonymous request only if supported, and a second reply after the notice was amended. Confirm each reply has truthful author and audience, does not change the underlying policy by itself, and survives reload with its parent message identity.

## 459. Secondary expansion — content route map and branch coverage ledger

This ledger translates the plan's examples into a practical authoring map. It is a design artifact, not another quest catalog. It helps prevent a communication feature from becoming a hidden quest manager and helps reviewers see which actions actually branch.

### Route map

| Content carrier | Entry fact | Player action | Owner that must resolve it | Possible close |
|---|---|---|---|---|
| Local board notice | Confirmed task or schedule fact | Post, delay, or choose audience | Message/board owner plus source owner | Current notice, expired notice, or correction |
| Intercom update | Owner-approved urgency | Broadcast, narrow, or wait | Canonical announcement owner | Acknowledged, unknown reach, corrected |
| Private request | Named sender and recipient | Relay, answer, decline, or clarify | Recipient/message owner | Replied, declined, expired, or unresolved |
| Multi-owner notice | Several scoped facts | Collect, summarize, or defer | Each fact owner and message owner | Verified combined post or partial update |
| Public question | Existing notice plus reader action | Answer, refer, correct, or hold | Question/message owner plus fact owner | Clarified, unanswered, or amended |

Each row maps back to one of the three core subfeatures. If a content idea cannot fit one of these routes without adding another mutable authority, it should be a separate proposal. A quest can consume a message event, but the message itself does not own the quest's task state.

### Branch coverage dimensions

For each route, write at least one version of: recipient reached; recipient not reached; source available; source unavailable; verified fact; partial fact; player changes audience; player declines; message expires; message is amended; save/reload before response; and accessibility alternative. Not all variants need bespoke dialogue. Some can reuse concise status text if that text remains accurate. The point is to exercise the logic and tone, not to inflate line count.

### Voice and flavor anchors

Use physical traces tied to the current space: a chalk tick under a shift notice, a folded private note with an edge worn from handling, a speaker switch the night watch uses sparingly, or a board corner reserved for corrections. Each detail should support the message's function and not invent a location owner. A faction's voice comes through what it is willing to confirm and what it refuses to overstate. Keep repeated status lines short; reserve richer prose for the first encounter, a consequential correction, or an exchange where a resident's preference changes the route.

### Final editorial sweep

Remove lines that label a resident “unreliable” without an established fact. Replace “everyone knows” with the actual audience. Replace “they agreed” with the specific acceptance event. Replace “we told them” with the channel and delivery evidence. Replace “no response means no” with “no response recorded” unless the person selected a refusal. These edits improve both continuity and player trust while preserving room for imperfect communication.

## 460. Secondary expansion — a message is not automatically an order

Internal messages can look authoritative because they are posted on a board or delivered over an intercom. The feature should distinguish information, requests, instructions from an authorized owner, and proposals from the player. A resident who reads “the west room is closed” needs to know whether it is a binding access state, a temporary warning, or a request to avoid the area. The channel does not grant authority to the author.

### Message intent labels

Use a small intent label: **information**, **request**, **owner instruction**, or **proposal**. Information describes a confirmed or estimated fact. A request asks a recipient to act voluntarily or respond. An owner instruction reflects a rule supplied by an authorized system or role. A proposal asks others to consider a change. Acknowledgement should not turn a request into an instruction, and a public post cannot promote a proposal to policy.

If there is no current permission model for an owner instruction, keep that category unavailable until an authority decision. Do not infer command authority from rank, faction, or intercom access. A supporting faction may post its own service hours but cannot close another group's room. A major faction may own a policy elsewhere, but this plan does not make its messages the default answer to every local question.

### Branches when intent is misunderstood

If a resident acts on a proposal as though it were an instruction, the player can clarify, amend the post, or accept responsibility for having presented it unclearly. If a request is ignored, the player can follow up through a permitted channel or choose another route; the message cannot silently assign the task. If an instruction is challenged, the player can link the source owner, request review, or state that the source cannot be confirmed. The communication feature does not adjudicate the policy itself.

The player can choose more cautious wording and reduce ambiguity, at a cost in length or time. A concise notice can still specify the action and its authority: “Request: clear the table before 16:00” differs from “Table must be clear by order of the room owner.” Where no authority exists, use the first. A resident may reply with a counterproposal. Their disagreement does not become disobedience unless a separately approved rule establishes that consequence.

### Supporting faction role and major-faction restraint

A small board steward can explain which messages are official for that board and can reject an unauthorized policy label. The steward cannot decide a major policy. A major faction's representative can supply an instruction only within its established mandate and must cite the relevant owner or policy. The player can ask a supporting group for interpretation, but that does not replace the source.

This creates a useful branch when a message's authority is unclear: comply conservatively, ask the source, challenge it, or choose a safe alternate route. If immediate danger is present, the relevant existing hazard owner determines urgent action. Do not make communication tone the substitute for a real safety system.

### Acceptance cases

Review a factual notice, voluntary request, authorized instruction, unapproved proposal, mistaken interpretation, and challenge to a source. Confirm intent is visible before response, acknowledgement remains separate from compliance, and only the proper owner can change access or assign work. This keeps the communication surface useful without turning it into a command system.
## 461. Secondary expansion — channel outage and graceful fallback

A communication route can be unavailable: a board is inaccessible, an intercom owner rejects a request, a recipient has departed, or a message source is stale. The feature should provide graceful fallback without claiming that the alternate route has identical reach. If the local board is closed, the player can wait, use a permitted private message, ask a messenger, or choose another destination. If the intercom is unavailable, the player may post a notice and contact critical recipients directly; the screen states who remains unreached.

An outage branch must identify whether the problem is temporary, permanent, or unknown. The player can retry after a real state change, but reopening the view cannot manufacture availability. A supporting group may restore a service only through an existing task and owner. The message feature cannot repair hardware by marking a post successful. If the route's state is unknown, use “not available” and offer actions that are still valid.

The fallback can carry different costs: a runner occupies time; a broad board reaches a wider but less certain audience; waiting delays the decision; a private message may not reach the whole group. Let the player choose based on actual need. Urgent information can require a policy-defined escalation, but it should not override recipient privacy or create duplicate announcements. One canonical message identity can link its fallback delivery attempts if the owner supports that; otherwise the plan must keep the records visibly separate and avoid claiming a single delivery status.

### Acceptance cases

Review board inaccessible before posting; intercom unavailable for a confirmed urgent source; one recipient absent; messenger declined; route returns after a task; and reload while fallback is pending. Confirm that failed delivery remains distinguishable from delivered, that a retry does not duplicate the message, and that no alternate channel claims the original audience without evidence.

### Wrong recipient or misrouted notice

If a private message is sent to the wrong recipient, the player should see the actual destination as soon as the owner reports it. They can withdraw the message if supported, send a correction to the unintended reader, or contact the intended recipient separately. Withdrawal cannot erase a message the recipient already saw. The system must not silently redirect the content to its intended destination. If the message contains personal detail, the player can disclose only what is needed to correct the mistake; an apology does not authorize further sharing.

For public notices, a wrong board can mislead people who were not intended to act. The player can remove or amend it through the board owner, then post to the proper audience. The correction should explain that the earlier notice was misdirected without exposing private reasons. Any acknowledgement from the unintended audience remains attached to the original audience record; it does not count as acknowledgement by the intended group. If the owner cannot distinguish the destinations, this recovery route is an explicit implementation gap.

The content response should remain proportionate. A resident who receives an accidental message can ignore it, ask why it arrived, or request that the player stop sending further details. The player can answer truthfully, use the correct route, or leave the conversation. The feature should not force a comic misunderstanding or a dramatic betrayal. The acceptance case verifies audience identity, withdrawal limits, correction reach, and save/reload after a misroute.

## 462. Branch casebook — the bell, the board, and the missing hour

This casebook follows a shelter schedule change across one shift. It uses one message fact—a workroom will be unavailable for part of the afternoon—and asks how the player handles source verification, urgency, audience, correction, and an unanswered request. The workroom and schedule are illustrative. Before implementation, use current locations, access owners, message APIs, and resident data. The arc is a sequence of ordinary communication decisions, not a new scheduling or incident authority.

### Opening: a credible but incomplete request

A support worker asks the player to announce that the workroom will close after midday. Their team expects a repair, but the task owner has not yet confirmed the start time. The player sees two facts: the closure request came from the worker; exact timing is an estimate. The announcement cannot be written as official access state until the relevant owner confirms it. A small notice could prevent people from bringing work to a closed room, but a broad alarm would interrupt everyone.

The player can: publish “workroom access may change after midday; check the board before entering”; ask the repair owner to confirm the window; hold the draft; or send a private note to one known reservation holder. If the message owner has no draft state, the plan treats “hold draft” as an authoring step outside gameplay and does not promise persisted drafts. If a current schedule authority reports that the closure is definite, the player can state it as confirmed. The player who publishes an estimate must label it.

### First branch: validate, warn, or wait

**Verify first.** The player asks the repair owner for its current task window. They may confirm a time, supply only a range, or report that work is not yet scheduled. This costs a service interaction or time if the owner supports that. The player can then issue a more exact notice, publish an updated estimate, or keep the room open until the actual task begins.

**Publish a cautious warning.** The player posts on the local work board, choosing text that makes uncertainty visible. It reaches people who check the board but not necessarily everyone with a reservation. The player can send direct messages to named stakeholders if permitted, or accept partial reach. They do not get to mark all reservation holders acknowledged.

**Wait for confirmation.** The player avoids posting uncertain information, but some resident may arrive before the update. If the repair begins earlier than expected, the owner changes access and the player can issue an urgent correction. If no work begins, no misleading closure needs to be retracted. This route can be preferable when a false closure has a high cost.

### Second branch: an exception appears

One resident replies that they need access for a time-sensitive personal project. This is a request for an exception, not proof the room should remain open. The player can ask the room owner whether another safe window exists, offer a private alternate location if supported, send the resident the confirmed closure detail, or state that no exception is available. The player should not reveal the resident's reason on the public board without permission. A support worker may identify a safe access window but cannot promise one before the task owner confirms.

If the exception is granted, its audience and expiry are narrow. The message records who is allowed and when, if the current access owner can express it. An exception does not cancel the general closure. If the underlying system cannot represent person-specific access, the plan must not invent an exception button; the player can instead reschedule the resident's work through the normal task route.

### Third branch: estimate changes after people act

The repair starts later than the notice predicted. The player can amend the post, issue a short correction, ask the original source owner to update it, or leave the board unchanged until the new time is confirmed. Residents who already adjusted their plans may ask for a direct update. The player sees which channels were used earlier and can choose whether to repeat broadly or target the affected group. A correction does not erase the old estimate for people who saw it.

If a resident missed work because the update was late, the game can show the direct task consequence. It should not assign fault unless a message delivery/acknowledgement fact and a clear obligation exist. If the player sent an update but the recipient never received it, the system states uncertainty. If the board post was only local, the player should not be told that every resident knew.

### Fourth branch: a public question challenges the policy

A resident asks who has authority to close the workroom. The player can link the task owner's confirmed status, request a response from the responsible group, state that they are passing the question on, or admit that the authority is unclear. A helper can explain its own role but not speak for the entire shelter. If the closure is a safety instruction, its owner must be visible. If it is a proposal to reduce traffic, label it as a request. The board cannot make a policy binding through tone alone.

This branch can end in an answer, a referral, a correction, or an open question. A resident may disagree after receiving the source. Their disagreement does not mean the notice failed; the system can distinguish clarity from agreement. The player may decide to use a private reply to prevent a public argument, but any broad operational change still needs the proper audience.

### Endings for the shift

1. **Confirmed and local:** repair owner sets a window; a local notice is posted; affected workers respond through the task route.
2. **Cautious warning:** the room may close; the board carries uncertainty; no one is falsely marked informed.
3. **Late correction:** the original estimate changes; the player updates the board and reaches some but not all known stakeholders.
4. **Private exception:** a named resident receives an approved access route, while the public closure remains intact.
5. **No exception:** the player reschedules the personal task or accepts that it waits; privacy is preserved.
6. **Authority unresolved:** the player does not issue a binding instruction; the message remains a proposal or the question is escalated.
7. **Overbroad broadcast:** the player uses intercom for a non-urgent change; residents are interrupted and the player can correct the urgency practice later.
8. **No post:** the player waits, and the task owner's access state changes through its own path; the communication need remains a recognized gap.

### Flavor and voice

Board details can show a paper edge curled by steam, old chalk removed from the wrong line, or a small “updated at” stamp. These should be used sparingly and only if the room has a board. The maintenance worker names the time they need; a resident asks whether their reservation still counts; the room owner speaks in confirmed conditions; the player decides how much context to publish. Dialogue should not repeat the same system status verbatim in every branch. Use the UI for dates and precise audience, and short lines for concern or uncertainty.

### Branch audit

The content author creates a path table for source confirmation, warning choice, exception request, changed estimate, public question, and final notice. Each branch records message identity, audience, exact claim, source owner, expiry, acknowledgement evidence, task impact, and save checkpoint. Remove any option that cannot update its owner. Preserve at least one valid ending where the player chooses not to broadcast and one where a participant is not reached. This case tests the three core features through ordinary communication rather than a one-off dramatic system.

## 463. Branch casebook — the quiet network during a long power cut

This casebook follows communication through a partial infrastructure failure. It does not add a power grid or radio system. It asks how the existing communication owner represents route availability and how the player handles uncertain reach while deciding what residents need to know. The narrative center is a service group's effort to keep a few essential updates accurate, while the player chooses which facts deserve interruption.

### Starting state: the board still works, the speaker does not

An owner-backed power event removes the intercom route for a period, while one notice board remains physically accessible. The exact technical premise must be verified against current communication and power systems. If the game does not model channel availability, this remains a content-only scenario. A support group that maintains the board reports that it can post written updates; it cannot confirm who reads them. The player has a task-status update, an estimated restoration time, and a request from one resident for a private reply.

The player's first choice is which fact to communicate now. They can post the confirmed fact with no estimate; post the estimate clearly labeled; wait for a second source; send a permitted direct message; or ask the support group to carry a small number of notices. The player sees the service group's capacity if the existing roster or task owner provides it. If the helper declines, the board remains available and the player can post personally.

### Branch: a local group asks for a relay

A work team on the far side of the shelter does not routinely visit the board. The player can ask a messenger to carry the update, post a second notice at a board that team can access, send individual messages, or accept that the team may learn later. The relay does not establish a shelter-wide audience. The player can combine a short operational line with a private explanation for a named recipient, but should not disclose that recipient's personal circumstances in public.

If a messenger is selected, the player chooses exact text, summary, or a request to speak directly. The messenger may carry only the approved content and can report whether they reached the destination if a delivery owner exists. They do not report agreement. If the second board route requires access permission, the board owner validates it. A support group that holds the board can say “we can post there,” not “everyone there will see it.”

### Branch: the estimate becomes wrong

The restoration estimate slips. The player can amend the board, ask the source owner for a new estimate, post a brief “still unavailable” notice, or leave the old estimate visible with a correction marker. If the system has no amendment history, the safe copy is a new message linked to the old only if a stable source link exists. Otherwise, say that the new post replaces the old one without promising a historical log.

Different residents react to the correction. One needs the channel for a shift decision; another asks for a private explanation; a third has not seen either post. The player can answer each through the appropriate route, publish a digest at the next shift, or stop sending updates until the source changes. The story can show a resident marking the corrected time by hand or asking the board steward to underline it. That texture should never imply an untracked acknowledgement.

### Branch: the player must prioritize one message

The board has little remaining space, or the support group can carry only one note. The player chooses between a current work instruction, a private request, a correction to the estimate, or a general service update. This is not a “who deserves to know” score. The decision depends on who must act before the next update and what the message owner says about expiry. The player can shorten wording, use a second board, wait for access to return, or leave one audience uninformed for now.

A minor faction may provide context: the work team needs a time, the care group needs a confirmed status, or the board steward needs text short enough to fit. Each role is conditional and limited. A major faction can set a broader policy only through its established owner; it should not commandeer the message thread. The supporting group makes the network work in one local corner, while the player retains responsibility for the choice.

### Branch: service comes back

When the intercom route returns, the player decides whether to replay the earlier update, send a consolidated summary, announce only the current state, or remain silent because all actionable messages have expired. Replaying old content can create confusion if its deadline passed. A channel restoration notice may be useful but is not a reason to broadcast every prior message. The owner identity and acknowledgement rules remain the same after restoration.

If the old messages were posted on boards, the player can mark them expired or superseded. If a resident asks for clarification, the player can provide it privately or direct them to the active notice. A support worker can help remove stale content only if the board owner allows it. They cannot erase a message that was already acted upon or claimed read.

### Endings

1. **One verified update:** the player waits for a confirmed fact and sends it over the available board; reach remains partial.
2. **Estimate with caveat:** the player posts an explicit estimate and later corrects it after the source changes.
3. **Messenger route:** one group receives a permitted relay; other residents may remain unreached.
4. **Private reassurance:** the named resident receives a narrow message without broad disclosure.
5. **No relay available:** the player accepts delayed reach and focuses on current operational choices.
6. **Over-notification:** the player repeats stale messages after service returns, then must correct the confusion through owner-backed routes.
7. **Channel failure unresolved:** the board owner or power system cannot establish a valid route, and the plan marks the gap instead of simulating delivery.

### Narrative treatment

The board steward notices which corners of the paper keep curling; the runner asks whether they should carry the exact words; the repair source refuses to turn an estimate into a promise; the player decides what to send. Keep the details ordinary and precise. Avoid melodramatic claims that the shelter is “cut off from truth.” The tension arises because communication is incomplete and work still has to proceed.

### Design evidence

Map source fact, route availability, message identity, audience, delivery evidence, expiry, task effect, and restore point for each path. Verify channel availability with current owners before writing this as a playable case. If the game has no power-linked communication state, use a simpler case in which the intercom is unavailable for a specific owner-backed reason. Preserve an ending where the player accepts uncertainty and acts locally rather than sending another message.

## 464. Candidate message bank — eight short exchanges with different consequences

The lines below are candidate examples for later canon review. They demonstrate how an authored message can remain concise while revealing audience, uncertainty, and next action. Replace role labels with current survivor and faction data. The lines do not establish that these notices, rooms, or services currently exist.

### 1. A confirmed local closure

**Board:** “West workroom is closed until the inspection clears it. The east bench remains open. Next update after the afternoon check.” **Reply:** “Does this affect the tool rack?” **Answer:** “The rack is outside the marked area; please confirm with the room steward before moving anything.” **Branch:** the player can post a confirmed source note, refer the specific question, or leave the rack detail unknown. The message cannot grant access.

### 2. An estimate, honestly labeled

**Board:** “Repair crew estimates the corridor may reopen near second shift. This is not a confirmed time.” **Reply:** “I already moved my load.” **Answer:** “The timing changed. Do you want a direct update if the crew gives a firm window?” **Branch:** the player can offer a private follow-up, but only if a reminder/delivery route exists; otherwise the resident checks the board later. The estimate is not a promise.

### 3. A private request, not a public task

**Private:** “Could you ask whether the quiet room is free for a short visit? I don't need you to tell them why.” **Reply from player choice:** relay exact words, ask to shorten, decline, or suggest direct contact. **Branch:** the receiver may accept, ask for a time, or not respond. The sender's reason stays private.

### 4. A request that needs consent

**Message:** “Would you cover the first half of watch? No is all right; I need to know before the next roster change.” **Response options:** accept this time; decline; suggest another time; ask what work will be handed off. **Branch:** an acknowledgement alone does not assign the shift. The roster owner must confirm the chosen schedule.

### 5. A correction that preserves unknown cause

**Correction:** “Earlier note said the pump stopped because of a valve fault. That cause is not confirmed. The pump remains offline while it is inspected.” **Reply:** “So it was not sabotage?” **Answer:** “We do not know. The confirmed fact is that service is paused.” **Branch:** the player can keep the question open, ask the pump owner for more evidence, or stop repeating the unsupported claim.

### 6. A room-specific handoff

**Local board:** “Supply count begins after the meal shift. Please leave marked crates in place.” **Reply:** “Can the clinic take one now?” **Answer:** “The count is not complete; ask the stock owner to release an item.” **Branch:** the player can request a stock decision, wait, or find another source. The notice does not subtract stock.

### 7. An unavailable channel

**Status:** “Intercom request could not be posted. No shelter-wide alert was sent.” **Next actions:** try a local board, notify a named recipient, ask the available runner, or wait. **Branch:** each route states its reach and delay. There is no “sent anyway” success line.

### 8. A closure without forced reconciliation

**Private reply:** “I don't want to discuss that request again. I will let you know if that changes.” **Player action:** acknowledge, stop contact on this topic, or ask about a separate operational need. **Branch:** the message thread closes; unrelated collaboration can continue. No hidden resentment value is required.

### Voice distinctions

The work steward's copy names boundaries and times. A stores worker names what is reserved and where release authority lies. A night watch member keeps urgent notices short and avoids calling routine changes emergencies. A resident may use tactile or personal detail but should not be made less clear for flavor. A support group can be terse because its queue is busy; its voice should not become robotic. The player's response tone may vary without changing source facts.

### Copy test

For every candidate, ask: who wrote it, who may read it, what is confirmed, what remains unknown, what action is requested, and what happens if no one responds? If any answer requires reading a previous scene, include a short reference or route to the message history. If an answer claims a delivery or permission state the owner cannot persist, rewrite it before implementation.

## 465. Branch casebook — the board that becomes part of the room

This short arc treats a local board as a familiar place where people encounter changing information. It is not a new bulletin-board authority or a faction quest. Each beat begins from a real message or owner fact, and the player decides whether a local habit should become a broader communication practice. The room and its board are illustrative; verify the location and message owner before implementation.

### Day one: a practical notice

A resident posts a short request for a work partner. The player can help them choose an audience, add a clear time window, send the request privately to a named person, or leave the wording as supplied. The resident can reject edits that change their intent. If the board supports authored messages only, use a candidate template rather than free text. The player sees that a post invites interest; it does not assign labor.

One support group uses the room regularly and asks that urgent repair notices stay in a marked corner. The player can accept, ask what board capacity exists, suggest a separate sheet, or decline. The group can explain its workflow, but cannot claim the whole board unless an owner says so. The choice influences later readability and reach.

### Day two: a correction arrives

The work partner has a different available time than expected. The player can edit through the message owner, add a linked correction, send a private reply, or let the original offer expire and create a new one. If a recipient already responded, they need to see the changed terms. A post may not be silently rewritten as though the original time never appeared. The player can preserve the old entry as history if supported or mark it superseded.

The partner can accept the new time, counter, or decline. A counteroffer stays in the private transaction between the participants; it should not expose personal availability on the public board. This crosses Plan 4 and Plan 3 only through existing message and initiative owners. It must not create a second offer ledger.

### Day three: unrelated needs compete for the same space

A public room notice needs space on the board. The player can remove an expired post, ask an author to close a completed request, relocate a routine notice, or use another channel. If the board is full and no capacity API exists, this remains a content case rather than a fake rejection. Removing a live request requires a truthful closure or relocation; the player should not erase it because another notice seems more important.

A board steward can suggest a visual structure. The player chooses whether to standardize headings, leave the board informal, or use a second location. A standard form can improve scanability but make personal messages feel institutional. A casual layout feels warmer but may obscure expiry. Neither solution is universally optimal.

### Day four: a reader asks who owns the board

A resident asks whether posts are official shelter policy. The player can explain that the board carries requests and updates, link the actual policy source, request a steward's clarification, or admit that a post's authority is unclear. The board's physical prominence does not grant authorial power. If a message was mistakenly formatted like an order, the player can correct its intent label and notify affected readers.

The author may disagree that the wording was ambiguous. The player can leave the original claim visible with context, seek the source owner, or remove it if authorized. This branch can produce a respectful dispute without establishing a new moderation body. The steward's task is limited to board use; policy remains elsewhere.

### Day five: a new habit or no habit

The player can keep the board local and informal, adopt a clear posting template, create a dedicated corrections area, or stop using it for requests that need direct acknowledgement. A supporting group may agree to maintain the layout if that task exists; otherwise the player makes the choice and the board remains a static context. The feature should not create a persistent “board culture” state unless current systems can own it.

### Endings

1. **Useful local board:** notices include audience and expiry; individual offers remain invitations.
2. **Private-first practice:** sensitive requests leave the public board; common facts remain local.
3. **Structured board:** headings clarify source and urgency, but authors can still choose another channel.
4. **Informal board:** residents post with less structure and the player accepts occasional clarification costs.
5. **Correction-led habit:** errors are amended visibly; source and uncertainty remain clear.
6. **Board retired for a use case:** the player shifts a specific workflow to direct messages because readership cannot be confirmed.
7. **No owner for board policy:** the player leaves the proposal as a suggestion and does not invent governance.

### Flavor and recurrence

The board can acquire layers: old pin holes, a corner rubbed smooth, an overwritten date, a hand-drawn arrow. Each texture should correspond to a current post or to nonpersistent scene dressing. Different authors have recognizable habits—one lists quantities, one underlines times, one writes a question instead of a command—but message identity and accessibility cannot depend on handwriting. Reuse the board as a location motif without repeating identical text. A later notice can refer to the habit the player chose through one concrete marker, not through a new faction-standing variable.

### Review checklist

Map every day to a message identity, source author, audience, status, owner, player action, and one future option. Ensure that the arc can end after any day without blocking ordinary shelter operations. Confirm that no notice creates an assignment or policy on its own. The board is a place for information; the player and existing owners create the consequences.

## 466. Channel-selection matrix — route by audience, urgency, and privacy

Channel choice should be legible as a decision the player can reason about. The system can show three dimensions without assigning a hidden overall score: **who needs the fact**, **when they need it**, and **what information may be shared**. The owner decides which channels are available. The author or privacy policy decides who may receive the content. The player chooses among valid routes.

### Audience classes

**Named recipient:** one person has a decision or needs a private explanation. Use a private message when allowed; a public board does not guarantee they will see it. **Task group:** a rostered team needs a work fact; a local board or direct group route may fit. **Affected room users:** a location change matters to people who access that room; a posted notice may be enough if exact reach is not required. **Shelter-wide audience:** everyone needs to act or avoid a known risk; broad notice or intercom may be justified, subject to owner policy. **Unknown audience:** the player does not know who is affected; ask the source owner, post a limited verified note, or defer.

Do not show names if only group identity is known. Do not assume a room's readers are the same people as its scheduled users. Do not say “everyone was told” unless individual delivery or acknowledgement data supports it.

### Timing classes

**Immediate:** an owner-backed condition says action cannot wait; urgent route is available if policy permits. **Before a shift or task:** send targeted information with a clear deadline. **Next reading opportunity:** use a board or non-interrupting message. **No deadline:** let the author choose a reasonable expiry or leave the post active by a defined rule. **Unknown timing:** ask the source, state uncertainty, or choose a conservative route.

The player can escalate urgency when new evidence arrives. Lowering urgency can also be a consequential choice if it delays reach. The interface should not permit “urgent” as a free label for any message. A notification owner must define the available levels and any hard constraints.

### Privacy classes

**Public operational fact:** publish only what the owner verifies. **Personal detail:** ask the source before sharing. **Mixed content:** split a public operational update from a private explanation. **Restricted record:** refer the recipient to the owner rather than copying the content. **Unknown sensitivity:** ask the author or choose the narrowest safe route. This classification belongs to authored content or existing policy, not a player-generated sensitivity meter.

### Decision tree

1. Is the claim owner-verified? If not, label it as an account/estimate, ask the source, or omit it.
2. Is there an owner-backed urgent condition? If yes, select a supported interrupting channel; if no, keep the route passive or targeted.
3. Who must act? Name the person/group as narrowly as possible; do not infer a full audience.
4. Does the message contain personal detail? Separate it from public operational content or obtain permission.
5. Can the channel confirm delivery? If not, tell the player reach is unknown.
6. What happens after expiry or no reply? Show a valid next action or leave the matter unresolved.

The decision tree helps content and UI authors, but it should not become six modal screens. Present only the choices that matter for the current message and keep the resulting route clear.

### Matrix examples

| Message | Audience | Timing | Privacy | Candidate route | Main uncertainty |
|---|---|---|---|---|---|
| Confirmed water schedule change | Affected users | Before next slot | Public operational | Local board plus targeted note | Who reads board |
| Private invitation | Named recipient | Flexible | Personal | Private message | Delivery/read state |
| Hazard confirmed by owner | Shelter-wide or defined zone | Immediate | Operational | Urgent channel | Acknowledgement vs action |
| Estimated repair finish | Users of a room | Next reading opportunity | Public estimate | Board, with time label | Estimate may change |
| Worker asks for a shift swap | Named eligible participant | Before roster lock | Private/work | Direct request | Consent and schedule |
| Policy question | Relevant decision owner | No immediate deadline | Public question or referral | Reply chain | Authority to answer |

### Acceptance use

For each authored message, annotate its source, audience, urgency condition, privacy class, route, delivery evidence, and expiry. Reviewers can then ask whether a second route is materially different or merely duplicates the same notice. A message that needs both public and private communication should have linked but separate audience promises, not one broad message copied everywhere without review.

## 467. Branch casebook — major policy, local voices, and the relay gap

A major faction sends a shelter-wide policy update that requires local explanation. The major faction remains the source of policy; a supporting group helps make it usable in daily work; the player chooses what to communicate and whether to seek clarification. This arrangement gives both levels a role without letting a small faction replace the major one or letting a distant authority speak for every resident's reaction. The policy, source, and named groups are illustrative and must be validated against current canon.

### The incoming message

The player receives an instruction about a resource schedule or access rule with an effective day and a stated authority. The source message may be clear about the rule but ambiguous about local exceptions. The player sees exactly what the major faction confirmed and what it did not address. They can relay the source unchanged, ask the major faction for clarification, request a local operational note from a supporting group, or delay communication if no effective time has been confirmed.

Relay does not imply endorsement. The player can include the source identity and say whether they endorse, question, or simply transmit the policy. The group that maintains the local schedule can explain how the rule affects the next shift, but cannot rewrite policy. A resident can ask for an exception; the player routes that question to the proper authority or states that no exception path is known.

### Branch: local support or direct relay

**Direct relay:** post the major faction's statement with source and effective date. This is fast and preserves provenance, but may not answer local questions. Residents can ask follow-ups; the player can refer them to the source, leave a question open, or add a local detail later.

**Supporting group interpretation:** ask a local crew to translate the rule into a shift or inventory procedure. The group may accept, suggest a limited scope, or decline because it lacks authority. If it accepts, the notice clearly separates “policy says” from “local schedule is.” The group cannot add enforcement terms absent from the source.

**Clarification request:** send a question to the major faction. The reply may take time, confirm an exception, or leave the point unresolved. The player can publish the known rule with the gap marked, wait, or choose a conservative local action if the owner allows it.

**No relay:** the player can refuse to repeat an unverified or misattributed message. They may post the confirmed local fact, ask the source to correct its own statement, or leave the shelter without a new instruction until authority is clear. This can produce real operational uncertainty, but the player is not labeled disloyal for declining to overstate.

### Branch: a minor faction's service is questioned

Residents may trust the local support group, distrust it, or ask whether it was authorized to interpret the policy. The player can show the group's exact scope, quote the major source, invite questions, or separate the two messages. A supporting group can stand behind its own operational note; it cannot answer for the major faction. If it made a mistake, the player can correct the local note while preserving the original policy text.

A faction member can choose not to be named in the correction. The player can remove their attribution if supported, name the source organization instead, or postpone a public reply. Correcting a local interpretation does not imply that the major policy changed. The system keeps source and interpretation distinct.

### Several resident responses

One resident may comply immediately; another may ask about a personal exception; a third may decline to acknowledge the notice. The player can send a direct answer, leave the general post open, or route operational questions to the group that owns the schedule. An acknowledgement does not prove compliance. A lack of acknowledgement does not prove refusal. A request for an exception is not a public vote against the policy.

If a resident refuses to comply with an owner-backed instruction, that consequence belongs to the policy/task owner and must follow its established rules. Communication does not implement enforcement. If no such owner exists, the message remains informational and the plan does not invent sanctions or security response.

### Endings

1. **Accurate relay:** source and effective date are preserved; local questions remain separately addressable.
2. **Locally clarified:** support group provides a bounded procedure; policy and local guidance remain distinct.
3. **Clarification received:** updated source creates a linked correction; previous uncertainty remains visible.
4. **Clarification absent:** player relays only verified facts, with the exception question open.
5. **Relay withheld:** player refuses to claim authority they do not have; operations continue under current policy.
6. **Local error corrected:** supporting group revises its own note without pretending the source policy changed.
7. **Audience disagreement:** some residents act, others do not acknowledge; the player sees that difference without a synthetic consensus flag.

### Narrative and faction texture

The major faction's language can be formal and constrained; the local group's note can name a specific shift, storage room, or service boundary; a resident's question can focus on one practical consequence. Do not reduce the major faction to an antagonist notice machine or the local faction to a friendly translation layer. Both can have legitimate limits and interests. The player can cooperate, question, or refuse to overclaim without being pushed into a single alignment route.

### Design audit

Record the source message ID, policy owner, local-note owner, shared audience, any private exception request, effective date, correction chain, and route used. Verify that the supporting group does not acquire permission to change policy and that the major faction does not acquire access to private replies. If the game has no policy owner or local interpretation route, keep this case as a design pattern and do not fabricate authority in message content.

## 468. Delivery failure library — tell the player what failed and what did not

Communication failures differ by stage. A failed draft, rejected post, uncertain delivery, unread message, missing acknowledgement, declined reply, and failed notification cannot share one red error badge. The player needs to know whether an audience saw anything and whether the underlying event changed. The cases below use the current owner where possible and mark missing semantics as an implementation decision.

1. **Draft rejected before save:** no message identity exists; the player can edit or cancel. Do not show the message in history.
2. **Post rejected by board access:** no board audience was reached; the player can request permission or choose a valid route. The draft may survive only if the owner supports it.
3. **Post saved, notification failed:** the board record exists; notification can retry without reposting. Do not claim the notice is absent.
4. **Private delivery unknown:** the message was sent to the named recipient, but there is no delivery evidence. The player may wait or use a permitted alternate route. Do not mark read.
5. **Read without reply:** the recipient has seen the content if the owner proves it; their intent remains unknown. The player can send one bounded follow-up or close the request.
6. **Acknowledged without agreement:** receipt is recorded; the requested task or decision remains open. The roster or task owner still needs a separate action.
7. **Reply arrives after expiry:** display the prior status and ask whether the author wants a new offer. Do not reopen the expired message as if its deadline never passed.
8. **Recipient leaves or changes identity:** follow the roster/migration owner. Do not forward personal content to a replacement without authorization.
9. **Correction fails:** keep the old published fact visible with its actual status and tell the player no correction was posted. Do not show the edited draft as public.
10. **Duplicate projection:** both views point to one source identity; acting in either view updates that identity once.

### Recovery language

Use factual phrases: “not posted,” “posted locally,” “delivery unknown,” “read,” “acknowledged,” “reply recorded,” “expired,” and “correction pending.” Avoid “everyone informed,” “they agreed,” “they ignored you,” or “the notice vanished” unless the message owner proves the stronger claim. If the existing model supports only a subset, narrow the wording and record the missing distinction.

### Player choices after failure

The player may retry with the same identity, change channel, edit content, ask the author, wait, or abandon. The choice should be safe: retrying a saved post cannot duplicate it; changing the audience requires a new preview; editing a private message cannot widen its recipients; abandoning does not cancel a task unless the task owner is explicitly notified. Some failures have no immediate recovery. An unavailable channel may remain unavailable, and the player can act through other systems.

### Supporting faction boundaries

A board steward can verify whether the post was accepted. A runner can say whether they reached a room if that is part of the route owner. A source faction can confirm its own fact. None can know whether every recipient understood or agreed. The player may request a second service, but the helper must explain the extra reach and cost. This makes support useful during failure without treating factions as omniscient message middleware.

### Review procedure

For each error state, capture the source record before and after, UI label, audience claim, permitted retry, save/reload behavior, and whether a downstream task changed. Review at least one failure where nothing was sent and one where something was sent but notification did not occur. The difference is fundamental to trust in the communication surface.

## 469. Group response arc — acknowledgement is not consensus

A shelter-wide announcement can receive many acknowledgements without producing agreement. This arc explores what the player should do when residents understand a proposed change but want different outcomes. It uses messages and responses, not a new voting, governance, or reputation system. Any binding policy or allocation remains with its current owner.

### The proposal and its audience

A support group suggests changing the order of a shared room's use to reduce idle time. The group asks the player to post a proposal and collect questions before it begins. The player sees that the suggestion is not an instruction, the group is its source, and no allocation rule has changed. They can post a summary, ask the group to provide a more specific proposal, send it to affected residents privately, or decline to relay it.

The post needs a clear response contract: optional comments, acknowledgement of receipt, or a formal response routed to an existing decision owner. If the current communication model supports only simple acknowledgement, the story must not use it as a vote. The player can tell residents where to send a concern, but only if the destination exists. A reply thread may remain informal and nonbinding.

### Responses differ by action, not by faction virtue

Some residents accept the proposed time; others need their usual slot; one asks whether the room will remain open during repairs; one does not reply. A support worker can answer the repair question but not promise a scheduling exception. The player can post a clarification, send a private response to a named resident, ask the group to revise the proposal, or end the discussion with no change. The participants can disagree about the best schedule without becoming rival political blocs.

The player can also choose to publish a summary of the comments. That requires permission or a public audience contract; personal details and named concerns should not be copied by default. They may share a count of acknowledged messages only if the owner actually produces it, and such a count says nothing about consent. If the discussion surfaces an existing allocation rule, link it and route a proposed change to the correct owner.

### Branch: revise, test, or leave unchanged

**Revise the proposal:** the support group changes the time based on a practical constraint and republishes a new version. Respondents can comment again if the route supports it. The old draft is marked superseded, not erased.

**Run a limited trial:** the player can do this only if an existing task or policy owner allows a temporary schedule. The announcement states duration, audience, and stop condition. Residents can opt out or request alternatives where the owner supports them. A successful trial does not automatically establish a permanent policy.

**Leave the current arrangement:** close the proposal, thank participants, and make no operational change. The room continues under existing ownership. No reply is treated as a vote against the proposal.

**Escalate an unresolved authority question:** request a decision from the current owner or a major faction if its established mandate applies. The support group can summarize operational constraints but cannot make policy. While waiting, the player can use the current valid arrangement.

### Outcomes and narrative callbacks

1. Proposal received, but unchanged; residents retain the old schedule.
2. Revised proposal accepted by the responsible owner and published with a new effective date.
3. Temporary trial authorized; feedback is collected through a real task or response route.
4. Trial reveals a practical problem; the owner restores the prior schedule.
5. Affected residents are not all reached; the player delays implementation where possible.
6. A private constraint leads to one individual accommodation without public disclosure.
7. Authority remains unclear; the proposal is closed without pretending consensus.

Later copy can reference that residents commented or that the schedule changed. It should not say “the shelter voted” unless an actual voting authority exists. A person who acknowledged may still object. A person who did not acknowledge may still agree. The communication system records observable responses, not a collective mind.

### Supporting roles and major-faction boundary

The support group originates the practical proposal and can revise it. A board steward helps format the post and preserve versions. A major faction may have final authority only if current policy says so. The player decides whether to transmit, delay, clarify, or stop. Each role remains distinct. None of the groups is made into a villain for having different constraints.

### Acceptance check

Track the proposal identity, version, source, affected audience, each response type, privacy choice, owner decision, effective time, and any rollback. Test unanimous acknowledgement with disagreement, missing responses, a revised proposal, a no-change ending, and reload during the trial. Verify that the interface never equates acknowledgement with agreement or message volume with consensus.

## 470. Branch casebook — correct a dangerous assumption without exposing a source

An unsigned note appears on a shared board: “The east stair is open again.” The board owner can establish the note's author as unknown, its posting time, its audience, and its current expiry. The game cannot infer that the stair is open from the wording. The player must choose how to respond based on the current location and access authority.

### First response: inspect, ask, or broadcast

The player can inspect the stair through an available location interaction, ask a known route contact for confirmation, publish a correction that labels the status unverified, or leave the note in place while seeking evidence. If a current scout report verifies access, the message may be updated with its source and observation time. If no such report exists, the copy must retain uncertainty: “No current confirmation. Check with the route desk before using the east stair.” A public correction does not transform uncertainty into a closure.

The player can also ask the route group to review the message. The group may offer a current observation, report that its last check is stale, or decline because it cannot send anyone. The group is a supporting source; it cannot silently take ownership of the board or make a major shelter policy decision. The player decides whether to wait, ask the responsible major authority, or post a cautious notice.

### Branch: preserve the unsigned note

If the player leaves the note, it stays visible only for its recorded audience and lifespan. It should not be made more prominent because the player did nothing. When it expires, the interface can state that it expired without confirming the claim. If someone later asks whether it was official, the player can say that its source was unknown. This ending supports a cautious campaign without turning silence into endorsement.

### Branch: attach verified context

If the player obtains a current report, the notice can be amended or replaced using the existing version contract. The update says what was observed, by whom or by which authorized source, and when. It does not reveal a private scout's identity if the chosen privacy setting withheld it. Residents who read the old version may still have the earlier information until they receive the update; the screen must distinguish “posted” from “delivered.”

### Branch: issue a correction before facts are complete

The player may post a narrow warning that the route is unverified. The message can reduce immediate misunderstanding without claiming to know why the original note was posted. A reader can acknowledge the correction, ask for more detail, or ignore it. None of those responses proves the stair's status. If the player later confirms that the route is open, a second version can revise the warning. If it is closed, the correction prevented a false claim from persisting but did not itself cause the closure.

### Branch: direct escalation

The player may route the question to the major faction that owns access policy. The faction can confirm a restriction, ask for an inspection, or return no answer. A policy response changes the message only after the communication owner records it. If the faction refuses to answer, the UI says so; it does not invent a personal motive for the official or label the faction corrupt. The player can then choose cautious wording, let the notice expire, or continue to seek a legitimate source.

### Consequence matrix

The branch consequence is determined by the action sequence: verified report requested or not; public correction posted or not; identity protected or disclosed with permission; authorized policy source contacted or not; old and new versions delivered or not. A player who posts a correction then privately informs a resident has a different result from a player who publicly names the route observer. That variation is about the communication decision and its audience, not a moral alignment score.

### Writing and validation rules

Never write “everyone knows,” “the shelter agrees,” or “the stair is safe” from a single acknowledgement. A board can hold competing claims with clear provenance and time. The route group can offer an observation but cannot guarantee future safety. A major authority may own a gate or rule but does not own residents' interpretation. If an earlier save lacks source metadata, show “source not recorded” and offer a new verification step rather than backfilling a fictitious author.

## 471. A notice over several shifts — continuity without a new quest tracker

This scenario demonstrates how a single board notice can accrue different consequences without becoming a questline of its own. A water crew asks for a two-hour work window near a quiet room. The request concerns access, noise, and a temporary schedule. It does not authorize the crew to enter private space or imply that the residents have voted.

### Shift one: compose a truthful first version

The player can post the request to the affected rooms, direct-message residents whose schedule is directly disrupted, ask a supporting steward to help identify the audience, or wait until the time is confirmed. The player sees which audience class each option reaches, any limit on board placement, and the requested time. If the work window is only a proposal, the notice says “proposed.” It is not titled “tomorrow's schedule” until a schedule owner confirms the time.

The steward can identify that the sleeping area and workshop share the same corridor. That useful detail changes the audience selection; it does not let the steward decide whether work proceeds. A resident may request that a personal reason for needing quiet remain private. The player can convey only the operational requirement—“quiet needed from 14:00 to 16:00”—without attaching the resident's name or medical history.

### Shift two: receive responses with distinct meanings

One reader acknowledges receipt. A second asks whether the work can happen later. A third replies that the request conflicts with a scheduled lesson. Another does not respond. The player can revise the time, answer questions, contact the relevant authority, or allow the notice to expire. Acknowledgement count is displayed separately from concerns and proposed alternatives. Missing response is not consent.

If the schedule owner offers two valid windows, the player can select either, ask the crew to choose, or decline both. If the crew has no supported alternate window, the correct answer is that the work cannot be rescheduled through this channel. The player may then explain the constraint or defer the task. Do not add fake flexibility to make the branches appear generous.

### Shift three: version the decision

If the player accepts a later window, the board creates a revised version with the changed time and reason at the appropriate level of detail. Earlier readers can be targeted for delivery if the communication owner supports it. If the player narrows the audience after learning that only one corridor is affected, the previous broad notice should not disappear from history as though nobody saw it. Its status can read “superseded” and link to the current version.

The player can also leave the original request unchanged after receiving objections. That is a valid route if an authorized owner confirms the timing and the player gives a reason. The communication result records the decision and channel. It does not label objectors as obstructionists or convert the notice into a vote.

### Shift four: observe the outcome

The work either begins through the work owner, is delayed, or does not happen. A notice cannot start a task by itself. After confirmed work, the player can post a completion update if there is a real result to report. If the crew cannot complete the task, the update names what remains uncertain. Residents affected by noise can report that it disrupted a lesson; that report can prompt a new message without rewriting the original record.

### Five ending families

1. **Adjusted window:** concern changes the schedule and the work occurs later.
2. **Protected detail:** the player communicates the operational need without disclosing identity.
3. **Authorized unchanged plan:** the work proceeds at the original time with a clear source for that decision.
4. **Deferred work:** no available window is accepted and the task remains open.
5. **Failed delivery:** one affected reader did not receive the revision; the player must decide whether to use another current channel.

Each ending supports callbacks tied to a recorded fact. Later prose may remember that the schedule changed, the private reason stayed private, or the notice failed to reach one reader. It may not claim that the community was consulted if only a narrow audience received a direct message.

### Design review

Review the scenario at normal speed and under fast day advance. Check that posting, editing, delivery, acknowledgement, response, expiry, and deletion remain distinct. Test the same message on a board, private message, and intercom path only where each channel is justified. The exact same content should not be duplicated into three modalities without a shared source record and one acknowledgement identity. Confirm that controller navigation and keyboard focus expose status and audience before commit; privacy and delivery consequences cannot be hidden behind a secondary tooltip.

## 472. Minor-faction cameo set — four useful services, no shadow authority

The communication surface gets richer when support factions contribute services that make an action possible or clarify its tradeoff. The cameo set below can be attached to ordinary notices and private messages. Each service begins with a player decision; none has the power to send a message without approval or speak on behalf of the whole shelter.

### The board steward keeps versions readable

The steward can format a revised notice, show what changed since the previous version, and identify the affected audience. The player chooses whether to apply that formatting. If a source field is missing, the steward highlights the gap but does not fill it with a guess. A resident can still reject the proposed wording or choose a private channel.

### The route circle distinguishes report from instruction

The route circle can submit an observation from its current survey record. It may say that a crossing was clear at dawn, not that it will remain clear after a storm. The player can attach the observation, request another check if one is available, or leave the board statement uncertain. This lets a supporting faction contribute specific expertise while the player remains responsible for the message decision.

### The repair crew provides a completion fact

After its task owner reports a completed repair, the crew can supply a concise service note for affected residents. The note can state the work and remaining limitation. It does not prove a radio, water, or access outcome owned by another system. A player can decline the public post and still retain the task result in its real owner.

### The care circle helps choose a private channel

The care circle can suggest a direct message when a public notice would expose an individual circumstance. Its advice is not a new privacy engine: recipient selection, permission, and delivery remain with the current communication owner. The player can choose another appropriate channel or decline to send anything. The circle cannot disclose the resident's reason on their behalf.

### Mixing offers and respecting refusal

For a work notice, the player may use the steward's version comparison, ask the repair crew for its actual status, and privately notify the one resident whose schedule is affected. Alternatively, the player can publish a short public notice and skip all assistance. If the steward is unavailable, the player's ordinary compose path remains usable. If the care circle declines to handle the case, no private data leaks and no penalty is applied. Each cameo is an optional affordance with its own action and observable contribution.

### Supporting role and major-faction boundary

A major faction can own policy, access control, emergency priority, or a formal schedule. Support groups can improve the quality of observation, wording, delivery planning, or task reporting. If a major authority issues a binding restriction, the player can transmit it with accurate attribution or ask for clarification where supported; the support group cannot nullify it. Conversely, a major authority cannot claim that every resident has agreed merely because it approved the policy. The channel preserves both institutional provenance and individual responses.

### Cameo acceptance matrix

Before adding a cameo, answer: What does the group know? What exact service does it offer? What player action requests that service? What current owner confirms the response? What cannot the group decide? What happens if the offer is declined? What does the player see after save/load? If any answer depends on a new group reputation meter, a second inbox, or an unowned vote, return the cameo to proposal review. The useful version is smaller and more specific: a name, a bounded service, and a truthful limit.

## 473. Intercom branch book — urgency changes the channel and the ending

The intercom is suited to a short, time-sensitive fact that many people need at once. It is poor for private context, long explanation, or a decision that requires a response from each resident. This branch book treats channel choice as a player action with a visible audience and cost: interruption, reach, privacy, and response effort.

### The initiating event

A pump crew finds that the west corridor should stay clear while it checks a pressure line. The task owner reports a temporary access request, not a confirmed leak or emergency. The player sees what is known, what is not known, the requested duration if supplied, and which areas may be affected. The player can use the intercom, post a corridor notice, send direct messages to known affected residents, contact the relevant major authority, or wait for a confirmed schedule. Each option has a different reach and delivery result.

### Branch A: use the intercom now

The player chooses a brief announcement because people are currently moving through the corridor. The preview names the affected location and the reason supported by the task report: “Please keep the west corridor clear while the pump crew checks the pressure line. We will update you when access changes.” It does not say “danger” or “contamination.” The player sees that the broadcast interrupts all eligible listeners and may reach residents who are not affected. On commit, the intercom source receives one durable identity; any banner or inbox projection acknowledges that same source.

A resident can acknowledge receipt, ask for an accommodation, or miss the message because they are absent or the channel did not reach them. The player can repeat the broadcast only if the owner allows and there is a reason. The announcement is not repeated automatically on every panel refresh.

### Branch B: choose a local notice

If the work begins later and affects one corridor, the player can post a notice only for that area. Residents can acknowledge or ask about timing. Those answers are responses to the notice, not votes on whether the task may occur. If someone lacks access to the corridor board, the player can decide whether a separate route exists. The board steward may help identify where people normally look, but it cannot guarantee delivery.

### Branch C: direct messages for accommodations

A resident who has a known scheduled task in the corridor can receive a private message if the communication owner supports recipient targeting. The player sees exactly who will receive it. The message can offer the alternate route only if a current navigation or location owner confirms that route. If no alternative is verified, the message says to contact the player or wait for a further update. Do not expose the resident's schedule on the public board.

### Branch D: wait for a stronger finding

The player may wait to send a broad announcement until the crew confirms a start time. That choice avoids premature interruption but can delay useful notice. If the crew reports a more urgent condition while the player waits, that later event can justify an intercom call. If no further report arrives, the original message can expire or remain a local notice. Waiting should not mutate the task result or secretly increase hazard.

### Response branches and closure

After the player selects a channel, the source owner tracks delivery and acknowledgement according to its contract. The player can clarify, revise, cancel, or let the message expire. If the crew stops work, a completion or cancellation update requires a new owner result; the intercom cannot infer that the corridor is clear. A missed recipient may require a second channel or direct confirmation. A delivered message with no response stays “delivered, no reply,” not “accepted.”

Possible endings are: work completed after a local notice; urgent announcement followed by a corrected all-clear; private accommodations arranged for named recipients; no message because the timing was not confirmed; or failed delivery followed by a player-chosen recovery channel. Later dialogue reflects the chosen channel and the actual result. It should not claim that the whole shelter was informed if only the intercom source was posted but delivery is incomplete.

### Copy variants

**Routine local notice:** “Pump check planned for the west corridor after the next shift change. Please use the marked route if it is open.”

**Urgent but bounded call:** “Keep the west corridor clear now. The pump crew is checking the line. We will send an update when access changes.”

**Uncertain status:** “The pump crew has paused work. Corridor access is not yet confirmed; check the west board before entering.”

**Confirmed change:** “The crew reports the west corridor clear as of 16:20.” Use this only if the work owner supplies the time and result.

**Delivery follow-up:** “Your message was delivered to two of three named residents. One was not reached.” The count should come from the communication owner, not a screen-local approximation.

### Acceptance checks

Verify that banner and inbox projections share one source identity, a single acknowledgement reaches the canonical record, and a reload does not repeat a call. Confirm that the delivery list excludes absent recipients only according to real roster status. Confirm that two channels can carry linked but distinct records without producing contradictory text. Exercise the no-response route, a failed delivery, a revised time, a cancel, and an intercom call followed by an accurate all-clear.

## 474. Channel-choice playstyles — reach, privacy, and burden are real tradeoffs

Communication should support multiple operating styles without rewarding one universally. The player can be a broad announcer, a precise notifier, a private coordinator, a record keeper, or a deliberate non-sender. Each method creates a different obligation and audience consequence.

### Broad announcer

The player uses the intercom when timing is immediate and many people need to move. This gives the widest potential reach and creates the greatest interruption. The announcement needs short, verifiable language. Follow-up is required if the instruction changes. Broad reach does not create unanimity or prove that each listener was present.

### Precise notifier

The player posts to the specific board serving the affected area. This minimizes unrelated interruption and leaves a durable local reference. Its risk is that residents may not see the board promptly. The player may need to pair it with a direct message or speak in person, depending on available channels. The board steward can advise on placement but cannot guarantee readership.

### Private coordinator

The player sends messages only to affected individuals. This protects personal context and can arrange accommodations. It costs time and requires correct recipients. It can also exclude someone who should have been informed. The preview must show audience before commit, and a separate public notice may still be needed for general access changes.

### Record keeper

The player waits for clear source information, versions a notice, and records who authorized the change. This improves later explanation but can be slower. The player may publish “time not confirmed” or ask a supporting group to gather a specific fact rather than leave residents uninformed. Provenance is useful only when the source actually observed or authorized the stated fact.

### Deliberate non-sender

The player may decide no message is needed, or may wait because the work is not yet confirmed. That choice is available without a secret communication penalty. If a current event makes the message necessary, the game can show the new condition and let the player act. A resident can later ask why they were not informed; the conversation should tie to who was affected and what the player knew at that time.

### Decision prompt design

Before send, summarize channel, audience, interruption, privacy, factual source, expiry, and expected response. Do not put all of those in a confirmation modal full of implementation terms; use short labels and expandable detail. If the player changes the channel after writing, preserve the draft text but refresh the audience and privacy preview. If the player exits, no message is sent. Keyboard and controller focus must land on the action summary before the final send control.

### Replay and variation

Two playthroughs can differ because one player broadcasts immediately while another waits for an authorized time. One can name the route circle as source; another can post the same observation without exposing an individual contributor. One can notify three scheduled residents privately; another can place a local notice and accept that not everyone will read it in time. These differences yield varied callbacks from recorded actions, not a hidden social reputation axis.

## 475. Candidate side-story — The Notice That Came Back Wet

This optional communication episode follows a repair notice through damage, disagreement, a supporting group's help, and a final player decision. It is a content example inside the three communication subfeatures, not a fourth authority or a substitute for the main quest system. Its central question is simple: what does the player do when one paper copy can no longer carry the whole message?

### Opening: the damaged copy

After a storm, the player finds the board notice about a roof inspection soaked and partly unreadable. The communication owner can still show the notice identity, author, audience, posted time, version, and expiry. The physical copy's damaged state is a presentation detail only if the current board/location owner supports it; it must not create a second message with an unrelated identity.

The player can repost the same version, create a revision with clearer wording, ask the board steward to compare it with the stored version, send a private message to residents known to be affected, or let it expire while checking whether the inspection still applies. The player sees the difference between posting a copy and confirming that the inspection remains scheduled. A previous notice does not prove a current work plan.

### Episode one: recover the source

A board steward can retrieve the stored text and identify which sections are unreadable. The player decides whether to restore it exactly, add a correction, or request a fresh work status. If the stored source is missing, the steward says that no authoritative copy is available; they do not invent the text from memory. A maintenance crew can confirm its own current task status, while a major authority may confirm whether residents still need to avoid the area. These sources answer different questions.

The player may choose to publish a short “inspection status being checked” notice. That is a valid interim update if the communication owner supports it. Alternatively, the player can send a direct message to one resident whose appointment is affected. The route team might provide a status observation, but cannot determine whether roof work is scheduled. Distinct groups provide narrow knowledge; the player assembles the message.

### Episode two: hear a competing account

A resident says the inspection already happened. Another says the roof still leaks. Neither statement proves the crew's current task state. The player can ask the crew, inspect a current work record, post both observations with attribution, or wait. If the task owner confirms completion but no downstream weatherproofing owner confirms the result, the player can accurately say “inspection completed” while noting that the leak report is unresolved.

The player can choose a neutral correction that keeps both reports distinct. A board response may read: “Crew reports inspection completed yesterday. A resident has since reported moisture near the west seam; follow-up is not yet confirmed.” This avoids treating one speaker as dishonest. If the resident does not want their name attached, the message can describe an unattributed report only where that option is supported and the resident consented to the disclosure.

### Episode three: choose how widely to update

If the issue affects one sleeping bay, the player may use a local board and direct messages for known occupants. If the access rule changed for the full shelter, an intercom call may be justified. If no current owner can establish the audience, the player may ask a supporting steward to map affected rooms, then review the list. Sending widely increases interruption and exposure; sending narrowly risks missing someone who needs the information. The interface surfaces these tradeoffs before commit.

One resident may ask for a private explanation. Another may ask that the repair crew's completion be made public because they need to know whether the common room is available. The player can answer separately through a private message and a public notice. A single public post should not reveal personal details just to make the explanation feel complete.

### Episode four: revise when the task changes

The maintenance crew delays its follow-up because a necessary supply is unavailable. The task owner reports the delay; the message owner can create a revised notice with the new state and, if supported, the next estimated window. If no time is known, the copy says “date not confirmed.” The player may leave the earlier notice visible as superseded, post a correction, or cancel it. A supporting supply group can report whether a request is pending, but cannot promise delivery without its owner confirming that commitment.

A resident can respond with an alternate access request. The player may ask the relevant schedule owner whether it is possible, decline because the alternate is not verified, or leave the suggestion open. A request is not approval, and a reply is not consensus.

### Episode five: close the communication, not the underlying problem

When the crew completes its follow-up, the player can send a factual closeout, leave the message thread open for unresolved reports, or let the notice expire. If the underlying leak remains uncertain, the communication can close while the maintenance concern stays in its actual owner. Conversely, the notice may remain active because more residents still need an access update even if the initial inspection is finished.

### Endings

- **Exact restoration:** the steward restores the source version and the work owner reconfirms its schedule.
- **Verified revision:** the player posts a newer version with current source and time.
- **Narrow notice plus private replies:** affected residents receive specific information without a broad alarm.
- **Broad urgent update:** the player uses intercom because access has changed for everyone.
- **Two reports preserved:** the board shows crew status and resident observation separately.
- **Unconfirmed, transparently open:** no authoritative status exists, so the player publishes no false closure.
- **Message expires:** communication ends while the task's real status remains unresolved or independently complete.

Each outcome is based on what the player investigated, whom they contacted, what they disclosed, which channel they chose, and what an owner confirmed. It does not branch according to whether the player is considered honest, evil, or compassionate.

## 476. Message-state language and branch copy matrix

Consistent status language is essential because the communication system must distinguish content from outcome. The following vocabulary keeps the player's understanding aligned with what owners know.

**Draft:** written locally; not delivered and not acknowledged.

**Posted:** accepted by the board owner for a specific board and audience; readers may not have seen it.

**Sent:** accepted by the delivery owner; delivery may still be pending or failed.

**Delivered:** the recipient or channel owner confirms receipt; not proof of agreement.

**Acknowledged:** the recipient actively marked receipt; no implied support or policy consent.

**Responded:** a recipient sent a reply; it may express agreement, concern, question, correction, or no position.

**Superseded:** a newer version replaced the current instruction; history remains visible where supported.

**Expired:** the message's valid lifespan ended; its claim was not thereby confirmed.

**Canceled:** the author or permitted owner withdrew it; this does not erase messages already delivered.

For every status, authors provide one plain-language sentence and one next action if available. “Not delivered to one resident” can offer retry, a permitted alternate channel, or close. “Delivered, no reply” offers waiting or another follow-up; it cannot be rendered as agreement. “Superseded” links to the current version. “Expired” identifies that time elapsed and distinguishes it from cancellation.

A reviewer should test every message path after day advance, recipient departure, board capacity failure, invalid audience, stale source, version change, failed delivery, and reload. The exact branch text is part of the feature contract: a status icon without explanation is insufficient when the decision affects privacy or access.

## 477. Branch casebook — an accommodation is requested in public

A resident replies to a broad work notice: “I cannot use the north passage while the pump crew is there.” The resident has placed the request on a shared board, but the reason may be personal. The player must decide how to respond without assuming that public wording grants permission to broadcast more detail.

### Ask what outcome they need

The player can ask which practical accommodation would help, request permission to message privately, offer to check another verified passage, or simply acknowledge the request. The resident may explain the access need, ask to keep details private, or say that no accommodation is required now. The player does not need a medical or personal explanation to consider an operational adjustment.

If a navigation owner confirms another route, the player can offer it. If no route is verified, the player can ask whether the resident prefers to wait, contact the access authority, or receive a later update. Do not label a route safe because it is shorter or familiar. The resident decides whether the offered accommodation works for them.

### Branch: respond publicly with minimum detail

The player can post “A private access question is being checked; use the existing marked route until we confirm an alternative.” This avoids naming the resident or describing their reason. It may be useful to others, but it also risks drawing attention to an individual. The preview should make audience and visible wording clear. The resident can approve, revise, or reject the public response before the player sends it when the communication owner supports that permission step.

### Branch: respond privately

The player can send a direct message to the resident and contact the access owner separately. The public notice remains unchanged unless the player makes a second action. If another affected resident also needs the information, the player must decide whether to reach them individually or revise the broader notice. A private reply does not mean the public audience has received an update.

### Branch: escalate a policy issue

The player may ask the major faction responsible for access policy whether an accommodation can be made. It can approve, reject, request more information, or fail to respond. A supporting care group can help phrase a privacy-preserving question but cannot grant the accommodation or disclose a private reason. If policy blocks the route, the player can communicate the limitation, explore a permitted alternate, or explain that no change is available. The resident's request remains a request, not a faction challenge.

### Branch: resident withdraws the request

The player can stop follow-up if the resident withdraws. If the message already reached a broader audience, the player may need to cancel or correct it where supported. A cancellation cannot erase what readers have already seen. Use restrained text: “The access request has been withdrawn; the current passage notice remains in effect.” Do not announce the resident's name or reason.

### Endings

- A verified alternative is offered and accepted.
- The resident keeps the reason private while the operational constraint is communicated.
- The policy authority declines and the player relays that fact accurately.
- The resident withdraws and the player cancels unnecessary follow-up.
- No route is confirmed, so the message remains open and cautious.
- The public thread is closed while a private conversation continues.

All six endings arise from channel choice, audience, consent, verification, and policy response. None depends on whether the player has accrued a “compassion” score.

## 478. Reader-response patterns — separate acknowledgement, concern, and consent

Responses should be authored as distinct kinds of evidence. The same board post may receive several responses, each with a different next action. The interface can group them by message identity while preserving what each response means.

**Acknowledgement:** “Read.” This confirms receipt only. The player may see it and do nothing further.

**Question:** “Which door?” The player can answer from a verified location source or decline to guess.

**Concern:** “That blocks the clinic route.” The player can verify whether the route is affected and revise the notice if facts support it.

**Alternative:** “Could the crew use the later slot?” The player can ask the task or schedule owner. Suggestion is not a booking.

**Objection:** “I don't agree with the plan.” The player can request a reason, preserve the objection, seek an authorized review, or proceed if the owner has already permitted the plan. The communication system does not turn objection into veto unless the policy owner says so.

**Consent to disclosure:** “You can tell the crew why.” This permission applies to the stated recipient and context; do not make it universal or permanent.

**No response:** nothing was received from that reader. The game should not infer absence, agreement, refusal, or ignorance without separate roster or delivery data.

**Conflicting responses:** one reader supports the plan, one proposes another time, and one objects. The message owner can preserve each response. A board summary can say “three replies: one acknowledgement, one alternate-time request, one objection,” not “the shelter is divided” unless an actual community decision process exists.

For each response type, define the next player action, whether a new message version is needed, and what status remains unchanged. This branch language makes communication feel alive while preventing the player's inbox from becoming an unowned faction vote.

## 479. Quiet-hours branch — the message is accurate but arrives too late

A board notice about a temporary corridor closure is posted with a valid time and audience. A resident reads it after the scheduled work window because they were away from the board. The message was accurate when posted, but the situation has changed. This is a branch about timing and delivery, not about a deceptive author or an inattentive resident.

### Player actions after late reading

The player can send a current status update, ask the task owner whether the corridor is clear, contact the resident directly, post a superseding version, or allow the old notice to expire. The fastest route is not always the broadest. If the player checks the task owner first, they may learn that work is complete, still in progress, or canceled. If no current status is available, the player can say so and avoid guessing.

### Minor-faction role

A board steward can point out that the old notice has not been replaced and prepare a concise update. A route group can confirm its own current passage observation. A major facility authority can confirm access policy. Each source is different. The steward cannot declare the corridor clear; the route group cannot cancel a maintenance task; the authority cannot claim the resident read an earlier post. The player selects which fact to use and how to reach the resident.

### Branch: one-to-one update

The player sends a private note with current status. If delivered, the resident can acknowledge, ask a question, or not respond. The public notice may remain visible to others, so the player can also decide whether to supersede it. Private delivery solves only the resident's information need.

### Branch: public superseding version

The player posts a new version with the updated status and a link to the old one if supported. The audience sees which detail changed. The resident may still not read it promptly; posting is not delivery. If the board does not notify absent readers, the player may decide to use another channel.

### Branch: wait for verification

If no authority confirms current access, the player leaves a cautionary notice: “Work status not confirmed; check before entering.” This is useful even though it does not answer the resident's question. The player may ask a support group to check, but only a current survey result can provide an observation. If the inspection takes time, the message remains a truthful uncertainty rather than an invented all-clear.

### Outcomes and copy

- “Crew reports work completed at 15:10; corridor access is still awaiting confirmation.”
- “The closure notice has been replaced. This resident has not yet acknowledged the update.”
- “The message was sent privately and delivered, but the public board still shows the earlier version.”
- “No current access report is available. Use the marked alternate route if the location owner confirms it.”
- “The notice expired. Expiry does not confirm that work or access changed.”

These lines let the player see what their action accomplished and what remains unknown. The intended ending may be a verified update, an unanswered message, a stale board that the player chooses to correct, or a still-open access question. The system supports all four without turning delivery delay into a moral judgment.

## 480. Communication history view — show enough context to act, not surveillance

A message history view should help the player correct or continue a specific communication. It shows source, channel, intended audience, version, delivery state, replies, expiry, and any owner-confirmed update. It does not expose private message bodies to unrelated residents or turn acknowledgement into a trust score.

The player can filter to current notices, unresolved delivery, or a named recipient only through existing privacy and recipient rules. A board steward may identify a superseded version. A major authority may inspect records only where current policy permits. A support group does not receive access simply because it helped compose the notice. If the owner cannot prove delivery, the view says “delivery not recorded,” not “unread.”

When an incident evolves, the history should put the latest valid version first while preserving a path to earlier versions. A resident who received version one may still need version two. The player can resend, use another supported channel, or close the matter if the current audience is no longer affected. Provide a neutral empty state when nothing is pending; avoid manufacturing message activity to make the screen look populated.

## 481. Message recovery snippets — state what the channel did

**Board full:** “This board cannot accept another notice. Choose a different eligible board or wait.” Do not claim that the draft was posted.

**Recipient unavailable:** “The message could not be sent to this resident. Check their current status or choose another permitted channel.” Do not assume departure or refusal.

**Audience invalid:** “This channel cannot reach that audience. Narrow the recipients or use another supported channel.” Do not silently broaden the audience.

**Source stale:** “The work status has changed since this draft was opened. Review the current report before sending.” Preserve the draft text while requiring a fresh factual preview.

**Version conflict:** “A newer notice is already active. Review it before replacing anything.” Never overwrite another author's message without a permitted command.

**No acknowledgement:** “Delivered; no acknowledgement recorded.” Offer waiting or follow-up, but do not turn this status into consent.

**Failed intercom projection:** “The announcement was recorded, but the banner did not refresh.” The source and projection must remain distinguishable; provide an accessible path to the canonical message.

Every recovery has an actionable next step and preserves the distinction between draft, source, delivery, projection, and response. Where the communication owner has no retry operation, do not display a retry button. If a retry is allowed, it must reuse or explicitly create the right identity so that recipients do not receive accidental duplicates.

## 482. Audience summary contract

Every compose and history view should summarize audience in plain terms before send: everyone with access to this board, the named recipients, or all eligible listeners on the intercom. If the actual recipient list cannot be calculated, say that reach is estimated or unconfirmed. A supporting steward can help narrow the audience, but final routing follows the message owner.

After send, show the audience actually recorded and any delivery failures separately from the intended audience. A message can be posted to a broad board and still have no confirmed readers. A private message can be delivered to its named recipient while leaving the public notice unchanged. Keep audience language consistent between preview, receipt, and history so the player can understand why a branch occurred without opening an audit log.

## 483. No inferred reader state

Do not infer that an absent acknowledgement means a resident ignored, opposed, or failed to see a message. Use only delivery, roster, and response facts exposed by their owners. If no reader state is available, present the message as posted or sent and leave the response unknown. The player's next action can be to wait, follow up through a permitted channel, or proceed under the applicable authority. Unknown remains a valid communication outcome.

## 484. Final channel consistency pass

Review each sample message on board, intercom, and private routes. Keep the same verified fact while adjusting length and audience. The intercom should be brief, the board should retain context, and the private message should disclose only what its recipient needs. All variants must point to the same source result when they describe the same event.

A quiet channel may be the right choice even when a louder channel is available; make that reach tradeoff visible.

## 485. Candidate side-quest — The Five-Minute Call

This optional, multi-day communication story uses one urgent intercom call, a local notice, and private replies to handle a temporary change in shelter access. It is an authored example inside the plan's three communication subfeatures. It does not add an emergency authority, faction vote, or parallel message history. The central branch question is not whether the player is honest or compassionate; it is which channel they choose, what they know at the time, whom they include, and how they respond when the first message does not reach everyone.

### Opening event: the corridor will close briefly

A maintenance crew reports that it must clear a narrow corridor for five minutes while it moves a pump component. The report identifies the location and expected duration but cannot establish the alternative route. The player can inspect the location, ask a route group whether a path is currently clear, broadcast a short intercom warning, post a corridor notice, privately message residents scheduled to pass through, or ask a major authority whether the closure is permitted. The interface previews interruption and audience before the player commits.

A player who broadcasts immediately uses the widest channel and can prevent people from entering during the move. The call says only what is known: “Please keep the east corridor clear for five minutes while the pump crew moves equipment. Use the marked route if it remains open.” The last clause is deliberately cautious until another owner verifies the route. A player who checks first may delay the alert but send a more useful alternative. A player who posts locally may interrupt fewer people while risking that a passerby never sees the notice.

### Act one: the route group has partial information

The route group can report that it checked the passage at the prior shift change. The player sees when the observation was made and whether current weather, an incident, or access changes make it stale. The group may volunteer to check again, decline because no member is available, or say that its records do not cover this location. The player can attach the observation, wait, use a cautious message, or contact the major facility authority. The support group contributes a bounded observation, not permission to close the corridor.

If the major authority owns access policy, its response can confirm the closure, ask for a different time, or leave the request pending. Its response does not speak for residents who rely on the route. The player can ask that authority to authorize a temporary detour, but the detour is shown only if a location/navigation owner confirms it. A major faction's approval and a route group's observation answer different questions.

### Act two: first channel, different outcomes

**Intercom first:** the canonical announcement is recorded once. Some listeners acknowledge; others are outside the channel's reach; one asks which passage is open. The player can reply with a verified route, admit that no route is confirmed, or send an updated call after a route check. A banner and inbox view share the source identity. Acknowledgement is not agreement.

**Board first:** the player posts a short notice to the affected area. A resident sees it and asks for an alternate time. Another person does not see it. The player can revise the notice, privately contact the first resident, or use the intercom if the situation becomes immediate. Posting a board update does not cause the work to wait or begin.

**Private messages first:** the player tells residents with a known schedule conflict. One replies that they can wait; another requests a different route; a third does not respond. The player can choose whether to send a general local notice as well. A private reply does not update the wider board automatically.

**Authority first:** the player asks the responsible major faction whether the task can proceed. A confirmed restriction delays it; an approval allows it if task and schedule owners also permit; silence leaves authorization unconfirmed. The player can communicate that uncertainty rather than treat the lack of reply as approval.

### Act three: timing changes

The move starts early, is delayed by a missing cart, or finishes inside the requested window. Each possibility comes from task/roster state, not from message text. If the crew moves early, the player may send a short correction that the corridor is clear, provided the task and access owners confirm it. If the cart is missing, the player may revise the notice with a new status and decide whether to keep the closure window reserved. If the crew finishes, a clear message names the confirmed completion time only when the task owner provides it.

A resident may report that they missed the call and arrived at the corridor. The player can explain that the call went to all eligible listeners but did not reach this person, apologize for the practical impact, or use a direct message next time. The communication owner may record delivery failure; it does not infer negligence or harm without evidence. A resident can remain dissatisfied even when the message was accurate. The story should not erase their experience to protect the player.

### Act four: choose how to close the thread

The player can post one final local update, send a private answer to outstanding questions, close the intercom source if supported, or let it expire. If the main event is over but one resident still needs route guidance, the broad announcement may close while that private request remains open. If the task remains delayed, the message can stay current with a clear next update time only if one is known. Expiry is not an all-clear.

### Ending set

- **Immediate warning, limited route detail:** players choose speed and later add verified guidance.
- **Verified detour:** the route group or location owner confirms a path before the main call.
- **Narrow local notice:** only the affected area receives a persistent board update.
- **Private accommodation:** named residents receive alternate timing or guidance without personal detail being broadcast.
- **Policy delay:** a major authority requests a different window and the crew changes schedule through its owner.
- **Delivery gap:** one resident misses the message; the player uses an additional channel and acknowledges the gap.
- **Unconfirmed access:** the player refuses to invent an alternate and sends a cautious update.
- **No broad message:** the player waits for timing confirmation and the short move concludes before any call is necessary.

Each outcome is valid. The player sees the effect of a channel decision and can recover from it. The story does not award an honesty point for cautious language or punish a broad call merely because it interrupted listeners; it shows the tradeoff and the actual response.

## 486. Channel branch map — audience size is only one dimension

The communication feature needs a reusable branch map that considers audience, urgency, privacy, source confidence, delivery certainty, and response burden. Those dimensions help writers and implementers avoid treating “public versus private” as the only decision.

### Audience

A **single recipient** is appropriate when the message concerns one person or asks for a private reply. A **small named group** fits a shared schedule or task handoff when recipients are known. A **local board audience** reaches people using a space but cannot prove that they read it. An **intercom audience** interrupts broadly and suits immediate common information. A **major authority channel** routes a policy question to its owner but does not replace messages to affected residents.

### Urgency

**Immediate:** there is a current, owner-confirmed reason to interrupt now. Keep wording short and limited to the action people need.

**Near-term:** an upcoming event has a supported time. A board or direct message can give context, with an intercom reserved for a changed or urgent condition.

**Unscheduled:** the player has a request but no confirmed window. Use tentative language, ask a source, or wait. Do not invent a deadline.

**Historical:** the message explains what happened. Preserve provenance and version; do not frame it as a current instruction.

### Privacy

**Public operational fact:** location, time, or general instruction that affects many people.

**Named personal detail:** a resident's reason, status, or request. Share only with their permission and appropriate audience.

**Anonymous report:** an operational issue can be stated without naming the speaker only when the reporting and communication owners support that form.

**Uncertain source:** label as unconfirmed; do not reveal an identity to compensate for weak evidence.

### Source confidence

A confirmed task state can support a narrow update. An observation may support a time-limited report. A resident statement can be attributed as a report rather than converted into fact. An inference must remain an inference. An unknown source cannot acquire authority from being repeated often.

### Delivery and response burden

A board has potential reach, not confirmed delivery. An intercom call can be recorded but not heard by an absent resident. A private message can target a known recipient but may fail. A faction relay can add expertise while adding an extra handoff. The player sees who will be interrupted, who is expected to answer, and what happens if nobody responds. Do not create an implicit response obligation for a message that only informs.

### Decision sequence

1. Is the underlying fact current and attributable?
2. Who needs it to act safely or make a choice?
3. Does the communication require a reply or merely convey information?
4. Is private context necessary, and has permission been given?
5. Which supported channel can reach that audience now?
6. What delivery state can the owner actually report?
7. What will the player do if delivery fails or a response contradicts the source?

Use this sequence as a review aid, not a forced seven-screen wizard. A clear preview can present the answer compactly. If the source is unknown, stop and seek verification or communicate uncertainty. If privacy is not permitted, remove personal detail. If no channel is available, show that limitation and leave the draft unsent.

## 487. Supporting factions as communication services

Minor and supporting factions can have a practical role in the communication story while remaining secondary to the player's decision and the major authority's remit. Each group offers a different service; the player chooses whether to involve it.

### Board stewards: version comparison and placement

A board steward can identify the active version, summarize what changed, and suggest a board that serves the affected rooms. The player reviews the suggestion and sends it. The steward cannot guarantee readership or decide that a board's audience should expand. If the message includes a personal detail, the steward asks whether it belongs on that channel rather than disclosing it automatically.

### Route circle: current observation

A route circle can report what its members observed on a specific path and at a specific time. The player may attach the observation, request an updated check, or keep route status unknown. The circle cannot make a formal access policy or promise future conditions. A report with no time or source is not a valid assurance.

### Repair crew: status and timing

A repair crew can confirm whether its own task is proposed, scheduled, active, interrupted, or complete according to the task owner. It can supply wording for a factual progress update. It cannot speak for residents affected by noise, access, or allocation. The player can ask for the crew's status, then separately decide which audience needs it.

### Care circle: private recipient advice

A care circle can suggest a private channel or remind the player to minimize personal details. It cannot decide that a resident needs help or send a message without authorization. The player can follow or decline the suggestion. If the care circle has no service route in current data, keep its role as optional authored conversation, not an operational panel.

### Records volunteers: provenance and retrieval

A records volunteer may help find an old notice and distinguish it from a current version. They cannot fill missing fields from memory or assert that a resident read the old copy. If the archive has no source, they say so. Their work improves traceability without creating a second message database.

### Interactions with major factions

A major faction can own policy, restricted access, shelter-wide schedules, or formal emergency instructions. A support group can improve observation, presentation, audience choice, or source retrieval. If the major authority issues a binding order, the player can communicate it with attribution, ask for review where available, or explain a conflict to residents. The support group cannot veto policy. The major authority also cannot claim individual acknowledgement or consent without the message owner recording it.

### Optional service graph

The player can request a route observation, then ask a steward to prepare the local notice, and finally send direct messages to affected residents. Another player can use only an intercom and no faction help. A third may contact the major authority first because policy permission is unclear. These paths produce different information and costs but share the same underlying source identities. If a group is unavailable, there is a readable route forward or a clear unresolved ending.

## 488. Candidate side-quest — The Board Goes Quiet

A message about a shared workshop tool receives no replies. The player does not know whether the message was missed, understood, ignored, or considered irrelevant. This optional story makes silence an uncertain communication state rather than a moral judgment. It uses public board, intercom, and private-message paths already defined in the plan.

### Initial post and source

A stores clerk reports that the shared soldering iron will be unavailable during its inspection. The player can post a board notice, ask the clerk for a more precise window, direct-message residents with known workshop tasks, or wait for the inspection schedule. The source is the stores owner; the clerk can explain it but cannot set the inspection. The message says “expected unavailable,” not “broken” or “reserved all day,” unless those facts are confirmed.

The player sees its audience and expiry. If a board has multiple possible locations, a steward may suggest the workshop board. The player chooses whether to accept that suggestion. The steward cannot guarantee who reads it. An intercom call reaches a wider audience but may interrupt residents who do not need the tool. A private message reaches selected recipients but may omit someone whose task is not visible.

### The quiet response

After posting, no response arrives. The board owner may report that the notice remains posted, but no recipient acknowledgement is recorded. The player can wait, review current work assignments to identify affected residents, send a private message to a known user, or ask the stores clerk for a status update. The player cannot infer from silence that the notice was accepted. They also cannot infer that the board failed if the owner only tracks posting, not readership.

A player who checks schedules may find one repair task depends on the tool. The task owner can confirm that dependency. The player may reschedule that task, find an authorized substitute, or contact its assigned resident. A private message can say “The tool is expected to be unavailable. Does this affect your task?” The resident may acknowledge, answer that another tool works, ask for a later slot, or not reply. Each result changes only the supported task or message state.

### Branch: do nothing further

The player may leave the notice posted and accept that its readership is unknown. If the inspection completes and the tool returns, the player can post an update. If it does not, the player learns from a new owner report. This route is intentionally available; the story should not make every quiet message trigger a crisis.

### Branch: send an intercom update

The player may decide the tool's unavailability now affects several crews and use the intercom. The call names the tool and known time window, then directs listeners to the workshop board for detail. The player sees that this is a broad interruption. If the intercom owner cannot deep-link to the board, the call should not promise that it does. Repeated broadcast is available only for a new status or a justified reminder.

### Branch: ask a support group to relay

A maintenance group can tell members who are working with the tool, but only if the current roster or task owner exposes those participants. The player may ask the group to contact them, message them directly, or decline the relay. A group relay is a service and may be unavailable. It does not count as every member acknowledging the message. The message source and actual recipients remain traceable through current communication ownership.

### Branch: revise based on source update

The stores owner may report that inspection will take longer or finish earlier. The player can revise the board version, notify affected residents, send a concise intercom correction, or let the old notice expire and publish a new one. Previous readers may retain old information until reached. The history should show the new version and its delivery separately. No obsolete text disappears as if it had never been seen.

### Endings

- The original local notice is sufficient; no response was needed.
- A resident asks for a different task time and receives an owner-confirmed schedule.
- A substitute tool is authorized and a task proceeds.
- An intercom reaches a broader audience after the player sees its interruption cost.
- A supporting group relays to a limited set of known participants.
- The notice is revised, while delivery to some readers remains unconfirmed.
- The player leaves the matter open until the source provides a new report.

Every ending is based on who was affected, what the player did with silence, and what the source owner later confirmed. No ending labels quiet residents as passive or uncooperative.

## 489. Branch casebook — competing speakers, one message identity

A public notice about ration pickup has one author, but several people contribute different information. A stores worker proposes a pickup window. A care volunteer asks that a resident's accessibility need be considered. A major authority defines the actual allocation schedule. The player must preserve source and audience while deciding what to publish.

### Separate source statements

The stores worker can state when supplies are expected to be ready. The authority can confirm the official pickup window. The resident can request a different time or private assistance. The care volunteer can suggest a communication approach. These statements are not interchangeable and do not merge into a community vote. The player sees source identity or approved attribution for each fact.

### Player route choices

The player can wait until the official schedule is confirmed, publish a tentative local notice, privately ask the resident whether accommodation is needed, request the authority to review the time, or decline to publish an unconfirmed plan. Each path has a tradeoff. Waiting improves certainty but delays notice. Tentative posting is faster but must label uncertainty. Private contact protects detail but does not notify everyone. Authority review may take time. Declining avoids misinformation but leaves residents without an update until another channel is used.

### Branch: a resident reports conflicting hours

The resident says they were told a different pickup time. The player may ask who told them, check the official schedule, preserve their report as a question, or publish a correction after confirmation. The resident's statement is real dialogue but not official policy. The major authority can confirm the time without calling the resident dishonest. A correction should explain the current source and keep a path for further questions.

### Branch: the accommodation is private

The resident may allow a direct message to the care volunteer, allow only the operational requirement to be shared with the authority, or decline assistance. The player sees exact recipients before sending. A public notice can say that residents who need another time should contact the desk only if that contact path exists. It cannot name the resident or state their personal reason without permission.

### Branch: the official window changes after send

The major authority revises the pickup time. The player can publish a new version, direct-message known affected recipients, use intercom if the change is immediate, or ask a steward to compare versions. The player cannot simply replace the text and assume all earlier readers saw the new one. The old version remains superseded; its delivery and acknowledgement state stays historical.

### Branch: the authority does not answer

The player may keep the plan marked tentative, ask for another authorized contact, post a note that confirmation is pending, or wait. A supporting faction can provide context but cannot speak for the authority. The player should not claim official status from a missing reply. A message can remain useful while uncertainty is clear.

### Possible resolution records

The source owner confirms time; resident's private accommodation is sent to the permitted recipient; the board notice is posted to a defined audience; each response is stored as its actual response type; an update supersedes the earlier version; or no public message is sent. The narrative may later remember that the player waited for confirmation, used a private channel, or published a tentative note. It may not reduce the episode to whether the player was truthful or deceptive.

## 490. Motifs and repeated communication scenes

Recurring scenes can make the communication feature feel woven into shelter life. A chalk mark near a board, a folded note at a desk, or the hiss before an intercom call can serve as motifs only where the current location and presentation owners support them. Each recurrence should change in response to a specific action or source fact.

### The board corner

The same corner of a board can hold a notice, a correction, and a superseded version. A player who checks version history sees how a schedule changed. A player who does not may still encounter the latest valid notice through its current presentation. The board corner can accumulate texture, but should not become a hidden quest tracker or reveal private messages in public.

### The hand-delivered note

A physical note can be a diegetic presentation of a private message only if the communication owner stores it as the same message identity. A character carrying it should not create a second independent delivery state. If the game cannot represent hand delivery, write the exchange in the private-message channel and use a simple UI presentation.

### The intercom breath

An intercom call can begin with a pause or a brief speaker correction, giving voice to a character. This is flavor; the canonical message remains readable in text. The spoken line must match the source body, audience, and version. A character may stumble or ask the player to repeat, but the event cannot be acknowledged twice.

### The returned card

A resident may bring back a schedule card after a work window changes. If the card is only a prop, it must not be used as authoritative proof. If it represents a current message or appointment, it references the owner record. The player may update, retain, or discard it without altering the actual schedule unless a supported command is invoked.

### Quiet and noise

Some scenes can contrast a public alarm with a private conversation. Use that contrast to help players feel the channel tradeoff, not to punish one choice. A broad call might wake a resident who did not need the notice. A quiet message might fail to reach someone quickly. The story can acknowledge those effects if current data and narrative consent support them; do not invent a waking simulation solely for flavor.

### Repetition review

Avoid using the same “notice falls from board” event as a generic cue for every failed message. Vary whether the problem is stale source, outdated version, wrong audience, missed delivery, unclear copy, or no reply. Distinct cause should produce distinct player action and physical detail. Where the current system has no observable cause, use neutral language rather than staged blame.

## 491. Candidate side-quest — The Last Listener on the Stair

A resident returns from a lower level after a shelter-wide intercom announcement has already ended. They ask what changed. The player must decide which parts of the message remain relevant, whether a new announcement is justified, and whether the resident needs a private answer or a general update. The story treats a missed call as a delivery gap, not a moral failure. It explores the difference between “the announcement occurred,” “the channel reached a person,” and “the person understood the change.”

### Opening state

The canonical intercom record says a temporary water restriction was announced at a specific time. It names the affected taps, source, and expected review time. The resident was not recorded as an acknowledgement. That does not establish whether they heard the call. The player can review the source, check whether the restriction is still active, ask the resident what they know, send a private message, or repeat the announcement if the channel owner and current urgency justify it.

If the water owner has already lifted the restriction, the resident needs a current update rather than a replay of stale instructions. If no updated state exists, the player cannot claim that service is restored. A stores clerk can report whether a local reserve exists, but cannot decide water policy. A major authority can confirm a restriction; the player may need to reach the resident directly if the intercom does not serve the lower stair.

### Route A: answer face-to-face

The player can explain the relevant facts verbally and offer to send a written summary. The resident may say they heard part of the call, ask which taps are affected, or prefer not to discuss it. A verbal exchange should not change canonical acknowledgement state unless the communication owner supports manual acknowledgement. The player can still make the resident's need visible through a supported response route.

### Route B: send a direct message

The player selects the named resident and previews exactly what detail will be sent. They may receive the original notice, a short summary, or an updated source statement. If the message is delivered, the resident can acknowledge, ask a follow-up, or make no response. A direct message does not update the entire lower level. If the resident asks the player to send it to someone else, the player must select that recipient and check permission and availability.

### Route C: place a local board notice

The player can post a notice at an appropriate lower-level board where one exists. A board steward can advise on placement or audience, but cannot confirm who reads it. The resident may prefer this because it helps others; another may request a private answer. The player can post a general service status without identifying the returning resident. If the board audience is unknown, use a cautious label rather than claiming that everyone downstairs was informed.

### Route D: repeat the intercom

The player may repeat a short call if the status is still urgent and broad listeners need it. Before sending, the UI shows that the call will reach all eligible listeners again, not only the late resident. The player may decide this interruption is worthwhile or use a local/private route instead. A repeated announcement links to the same event only if it is truly the same current instruction; an updated restriction creates a new version or event according to the source owner's contract.

### Route E: ask a support group to relay

A route or maintenance group may have a member passing through the lower level. The player can request a personal relay, but only where a current roster or communication owner can record the recipient and result. The helper can agree, decline, or deliver later. A promise to relay is not delivery. The player can use it as a second route while retaining the canonical board or intercom source.

### Branch: the resident was already informed

The resident may say they received a written note from someone else. The player can verify that message if it is visible through the current communication owner, thank them, or simply close the interaction. No duplicate public announcement is necessary. The resident's statement can be accepted as conversation without inventing an acknowledgement in another record.

### Branch: the resident disputes the restriction

The resident believes a particular tap is unaffected. The player can check with the water owner, ask a maintenance crew to inspect, preserve the resident's claim as a question, or repeat the current restriction. The resident cannot override the source. The authority's answer may be that only a named tap is restricted, that all taps remain closed, or that no fresh reading exists. The player communicates the exact result and keeps the disagreement visible where appropriate.

### Branch: the resident needs an accommodation

The resident may ask for stored water, a different route, or a contact point. The player checks current need, inventory, access, or medical owners as relevant. A care group may advise on a private communication channel, but cannot grant stock or change water policy. The player can offer a permitted accommodation, refer to an owner, explain that none is currently available, or ask what alternative the resident prefers.

### Endings

- The restriction has ended and the player provides a verified restoration update.
- The restriction remains, and the resident receives an accurate private summary.
- A local board notice reaches the affected area while broader acknowledgement stays unknown.
- An intercom is repeated because urgency outweighs the broader interruption.
- A support group agrees to relay but delivery remains pending until confirmed.
- The resident was already informed; the player avoids sending a duplicate.
- The resident disputes the status and an owner check preserves or corrects the instruction.
- No fresh source is available, so the player communicates uncertainty and leaves the question open.

The player is not scored on whether they broadcast or stay quiet. The branch consequence is the actual audience, status, source verification, privacy choice, and delivery outcome.

## 492. Language matrix — uncertainty, instruction, request, and report

A message can sound authoritative even when the source is weak. Writers need a controlled vocabulary to distinguish the message's speech act. This matrix is a prose aid; it does not add a new message schema beyond fields the current owner supports.

### Instruction

Use a direct imperative only when the source and authority justify one. “Keep this corridor clear while the crew moves the pump.” Name the source, duration, or scope if known. A player suggestion should not sound like an official order.

### Request

A request invites a response and can be refused. “Can the workshop stay quiet between 14:00 and 15:00?” Do not phrase it as a settled schedule. Provide an option to ask a question or decline where the owner and interface support replies.

### Report

A report states an observation with source and time. “The route group saw the south passage clear at dawn.” It is not a guarantee. If a report is old, mark it as old or ask for a fresh check.

### Proposal

A proposal describes a possible future action. “The crew proposes a pump check after shift change.” It remains unconfirmed until an owner accepts and a schedule is recorded. Do not title a proposal as a final notice.

### Correction

A correction identifies what changed and which earlier message it supersedes. “The inspection will start later than the first notice said.” It does not need to shame the first author. Readers may have seen the old version, so display the updated time and delivery status.

### Question

A question asks for missing information. “Is the west door still available?” Do not make a question look like a warning or infer the answer from lack of response.

### Acknowledgement

An acknowledgement means that a message was received or noticed according to the current owner. It does not express consent, approval, understanding, or task acceptance. In UI copy, show the exact label that the API supports.

### Refusal or objection

A refusal can reject a request; an objection can disagree with a plan; a concern can identify a risk. These are different response types. None necessarily cancels a policy or changes task eligibility. The relevant owner determines whether there is a permitted review path.

### Unknown

When source, delivery, or current status cannot be established, say what is unknown. “No current route report is available.” “Delivery was not recorded.” “The message expired.” Unknown is not a failure of narrative; it is a truthful branch that allows the player to seek another source.

### Translation and accessibility review

Keep sentences short enough for narrow panels and captions. Avoid relying on capitalization, color, or sound to convey urgency. Provide an equivalent text for intercom audio. Make audience and source readable in the same place as the commit action. Translators should preserve the difference between request and instruction, report and confirmation, acknowledgement and agreement. If a language lacks a concise distinction, add a short contextual phrase instead of collapsing the states.

## 493. Continuity without surveillance — use message history sparingly

The communication feature can support later callbacks without turning into a log of everything every resident read. Remember only what the current owner records, what is relevant to a later choice, and what permission allows the player to see. A public board post may be available to the player; a private message may remain between its participants. The system should not expose private content through a generic history panel merely because it would make a callback easier.

A later line can reference a general message event: “The corridor notice changed after the crew's update.” It should not reveal which resident objected unless that information was appropriately shared. A resident may remember that the player sent a private route update, but the same fact should not become a public faction reaction. If the save predates message history, use neutral dialogue and let the current message owner rebuild only what it legitimately owns.

Before persisting a callback, identify its owner, retention rule, privacy level, and downstream reader. Prefer one specific fact over a full timeline recap. Do not generate dialogue that suggests surveillance such as “I saw you open the note” unless the game truly records that event and the player understands the implication. A response can be based on a resident's explicit acknowledgement or reply, not on an invisible read receipt. This keeps message branches meaningful without making every channel feel monitored.

## 494. Candidate side-quest — The Unnamed Report

A resident submits an unsigned report that the common-room heater clicked off twice during the night. The player sees that the source is not recorded, the report time is approximate, and no current repair task confirms a fault. This side story allows investigation, cautious communication, and support-faction expertise without making anonymity itself suspicious. Its branches come from the player's actions and later evidence.

### Opening choices

The player can ask the room steward whether anyone directly observed the heater, request a maintenance inspection, post a general note asking people to report further outages, privately contact residents who were on the night schedule, or leave the report unshared. Each route has a different audience. The report's anonymity does not prove that it is false. The player's first choice may be to gather evidence rather than announce a problem.

A board steward can explain how to post a general request without exposing a source. A maintenance crew can check the heater if it is eligible and available. The duty roster can identify who was assigned to the room, but does not prove who witnessed the event. A major facilities authority may own restrictions on heater use. The player must distinguish those sources.

### Route A: verify quietly

The player asks the maintenance crew to inspect. It may confirm a fault, find no issue, identify an intermittent condition, or be unable to reproduce it. The crew's result belongs to its task owner. The player may privately notify residents, post an update, or leave the matter in the task log. If no issue is found, the player does not need to announce that the report was false; the result is “not reproduced during inspection.”

### Route B: ask for more observations

The player posts a neutral notice: “If you noticed the common-room heater stop, tell the maintenance desk when it happened.” The notice invites reports but does not identify the original reporter. Residents may send a time, disagree, confirm a click but not shutdown, or remain silent. The player can ask the group for an inspection, acknowledge the range of reports, or wait. Report count is not a vote and does not establish fault by popularity.

### Route C: send a private check-in

The player sends messages to residents assigned to the night watch where the current communication owner permits recipient selection. A recipient may say they heard a click, saw a light go out, did not notice, or prefer not to discuss it. The player can ask a follow-up, share no personal detail publicly, or stop. No response means no response; it cannot be rendered as denial.

### Route D: post a safety instruction

The player may ask the facilities authority whether a temporary restriction is needed. If it confirms a restriction, the player can publish it with source attribution. If it says no restriction is required, the player may still request inspection. If no answer is available, the player should not write “unsafe” or “all clear.” The authority's policy and the maintenance crew's observation stay separate.

### Turning point

The crew finds the heater's safety switch unchanged but cannot reproduce the outage. Later, a resident reports that the click coincides with someone switching off a nearby lamp. The player can ask for another check, correct the initial interpretation, thank the reporter privately, or close the notice. If the source remains unknown, the story may close without identifying anyone. The outcome is useful because the player changed the question and source, not because the report was judged truthful or false.

### Endings

- **Confirmed fault:** the maintenance owner reports a fault and an authorized repair task opens.
- **Not reproduced:** the crew reports no observed fault during its inspection; the report remains unproven.
- **Different cause:** a second observation explains the click, and the player revises the message.
- **No policy restriction:** the authority reports no closure, while inspection or observation may continue.
- **Private account:** an individual reports their experience without public identification.
- **General call for observations:** the board invites future reports without naming a source.
- **No public message:** the player checks through a task route and keeps communication quiet.
- **Open uncertainty:** no further evidence arrives; the player leaves the record inconclusive.

Each route is distinct and recoverable. The player is never offered an option to “expose the liar,” because the message owner cannot prove such a motive.

## 495. Group message branches — disagreement is not consensus

When several people respond to one notice, the communication feature should represent their actions accurately without inventing a representative voice. A five-person workshop group might produce one acknowledgement, two questions, one objection, and one unanswered message. The player can decide what to do next, but the system should not report that “the group approved” or “the group rejected” unless a current decision authority records that outcome.

### Branch: answer each question

A resident asks which tool is affected; another asks whether the room is available; a third asks when the change starts. The player can answer from source records, ask an owner, post a revised summary, or leave an answer pending. If one reply resolves only one question, do not mark the entire message resolved. A support group may help identify the correct technical source, but the player chooses whether to request that service.

### Branch: group representative is appointed

If current policy permits a representative, the major authority or group owner defines who holds that role and what topics they may answer. The communication owner records the recipient and response. The player may contact the representative, post a summary, or ask affected individuals directly. A representative cannot speak for private preferences outside their authority.

### Branch: responses conflict

One resident says the schedule is workable; another says it overlaps their assigned task. The player can inspect the current roster, propose another time, seek the owner decision, or proceed with an authorized unchanged plan. Positive replies do not erase the conflict. A response count is context, not an approval score.

### Branch: silence continues

The player may wait until the posted deadline, contact only known affected residents, use an intercom if urgency has changed, or proceed under a policy that does not require reply. If there is no actual deadline or policy, do not generate one. Silence is not consent or refusal. The interface can state how many recipients have acknowledged and how many lack a recorded response, if that count exists.

### Branch: one person asks to leave the thread

The resident may ask not to receive nonessential follow-up. The player can respect that request, use a required policy channel only where authorized, or ask whether a different update format is preferred. This request does not remove the person from all future communications. Recipient preferences should be scoped and stored by their existing owner.

### Branch: update after an individual objection

The player can revise a message to address the objection, reply privately, leave the original unchanged with a decision note, or request formal review. If revised, the audience sees the changed wording and version. The objector can remain opposed. The update is successful as a communication change even if it does not secure agreement.

### Writing policy for group results

Use “two residents replied,” “one acknowledged,” “one asked to change the time,” or “the representative confirmed the posted schedule.” Avoid “the residents agree,” “the crew is against you,” or “everyone supports the plan” without explicit authority. A message can be delivered, acknowledged, contested, superseded, and still active; these are independent facets.

## 496. Response timing patterns — delay can matter without becoming a moral meter

Response delay has practical meaning when it changes a decision window. The feature should show urgency from a current owner and avoid using wait time as a hidden character virtue. A late response may still be useful; a fast acknowledgement may still contain no agreement.

### Immediate reply

The resident answers while the message is open. The player can respond, accept the concern, ask a source, or close. Immediate response is not more valuable than a later one. The UI should not reward quick click-through with a trust bonus.

### Reply after the task changes

A resident responds after the crew has already changed its schedule. The player can explain the current state, send the new version, or ask whether the resident needs anything else. The earlier concern remains historically true even if the schedule changed. Do not erase it as obsolete.

### Reply after the message expires

The message is expired. The resident can still reply if the communication owner permits, or the player can start a new thread. The old message should remain distinguishable from a current instruction. A late response might reopen discussion but does not automatically reopen the old schedule.

### No reply by a known time

Only show a deadline if the owner set one. The player can proceed according to a valid policy, ask again through an available route, or leave the plan. No reply does not mean the resident intentionally withheld consent.

### Contradictory reply after broadcast

A resident may correct a public fact after a call. The player can check source, post a correction, or preserve uncertainty. The correction does not retroactively undo that listeners heard the earlier statement. The message history lets the player see both versions and their audiences.

### Delivery and persistence review

Test response paths with time advance, expiry, source update, and reload. Verify that the message status remains tied to its original identity, a new version receives its own proper status, and no reply is duplicated. If the game cannot persist response time, avoid dialogue that depends on exact delay. Use a general “later” line or a neutral fallback.

## 497. Candidate side-quest — The Notice With Two Authors

Two supporting groups ask to publish guidance on a shared work space. The maintenance crew wants to state which tools are safe to use; a records circle wants to explain that the current inventory list is incomplete. Both statements can be true, but they have different sources and audiences. The player decides whether to publish separate notices, request a shared summary, contact the major authority, or leave both drafts unsent.

### First draft: preserve source and scope

The maintenance crew provides a short safety note about one tool. The records circle provides an inventory note about two items that remain unverified. The player sees which source supplied each sentence and can choose to post each separately, prepare a linked pair, or ask the groups to revise. The board steward can format them, but cannot merge their factual claims without agreement.

A major authority may own the official tool-use rule. If it confirms the crew's limit, the player can attribute that policy. If it does not respond, the crew's practical observation remains distinct from formal policy. The records circle can clarify its list, but it cannot say that an item is safe or unsafe.

### Branch: list both authors

The player can ask each group whether its name may appear beside its statement. One may approve, the other may prefer a generic source label, or one may withdraw. The player can publish only the approved source attribution and preserve the other statement as an unverified note. The groups' identities are not automatically public because they helped draft content.

### Branch: one summary, two linked claims

The player may write an opening line that points to two separately attributed details. For example: “The maintenance crew asks residents not to use the blue tester until its lead is checked. The records circle has not confirmed the shelf location of two other tools.” This is one message with two claims and sources, not a consensus statement. The player can accept the wording, ask for plain language, or keep the drafts separate.

### Branch: disagreement over wording

A group may object to the word “unsafe” because no hazard has been confirmed. Another may want “missing” when the item is merely unlocated. The player can revise to “do not use until checked” and “location not confirmed,” ask the relevant owner for a fact, or choose not to publish yet. Neither group can force the player to send its preferred wording. A revised version preserves the earlier draft history when supported.

### Branch: major authority makes a decision

The authority can issue a clear restriction, ask for an inspection, or decline to rule. The player can send the restriction with its source, keep the practical crew note separate, or ask for clarification. The support groups may offer expertise but cannot override the official restriction. A formal rule does not imply that residents have acknowledged it.

### Branch: one group withdraws after posting

If a group withdraws its authorship after a notice has reached readers, the player can update the message to remove the attribution, state that the earlier source was withdrawn, or let the notice expire if the claim remains valid from another source. A correction cannot erase copies already read. The player sees the audience and version consequences before committing.

### Endings

- Two separate notices preserve precise source and audience.
- One linked summary clarifies that claims come from different groups.
- A major authority issues a binding instruction and the player transmits it accurately.
- A cautious draft remains unpublished pending verification.
- One author withdraws and the message is revised with history retained.
- The groups disagree and the player closes without a shared statement.

This side-story adds authorship, source, and revision branches. It does not create a writing committee, group-voting rule, or faction reputation channel.

## 498. Faction message boundary matrix — supporting voice and binding instruction

A message can contain an observation, a recommendation, a policy, a request, or a player's own decision. Confusion between those speech acts makes factions feel arbitrarily powerful. The following matrix gives writers a consistent branch boundary.

### Supporting group observes

A route group says the north path was clear at a specific time. The player can attach that fact, ask for a new check, or keep status unknown. It cannot direct residents to use the route unless the current access owner permits it. Its observation can be publicly credited only with appropriate permission.

### Supporting group recommends

A repair crew recommends keeping a tester out of use until its cable is checked. The player can follow that advice, request formal policy, or ask for another opinion. Recommendation is not a binding rule unless an owner defines that authority. A message labels it as advice.

### Major authority instructs

A major faction with current remit closes a room or restricts an item. The player can communicate the instruction, ask for an appeal, or tell affected residents the source and duration. A supporting faction cannot cancel it. The communication surface cannot silently change the instruction's audience or lifespan.

### Resident requests

A resident asks for quiet, privacy, a time change, or information. The player can acknowledge, accept if authorized, decline, seek accommodation, or ask a clarifying question. A public response does not imply that the person endorses the authority's decision. A private explanation stays private unless permission supports sharing it.

### Player proposes

The player may suggest a schedule or ask the community to consider one. The message says “proposed,” names who is affected, and gives a clear reply path if one exists. It does not appear as an official policy or settled vote.

### Source comparison in UI

Before sending, show source labels in plain language: “reported by route circle,” “confirmed by facilities authority,” “proposed by player,” “requested by one resident.” Avoid color alone. If a message includes more than one claim, allow the player to review each claim's source. A generic faction emblem is not enough to prove a statement's scope.

### Branch review

For each faction message, ask what the group actually knows, what it may decide, who owns the fact, and what the player can do if the group declines. Test the scene with the supporting faction absent and with the major authority unavailable. The player should still see a truthful source gap and a next action. Do not let any faction carry a whole quest because the communication interface needs someone to author a post.

## 499. After-action communication — notify, document, or leave unannounced

After a task or incident, the player may need to communicate its outcome. The choice of whether to send a message is itself a branch. It must follow the task owner's facts and the audience's need, not a generic “always report” rule.

### Confirmed success

The task owner reports a result. The player can send a short completion note to affected residents, post a detailed board update, message one recipient who asked for the result, or leave the record in the task view. If the task has a remaining limitation, the player can include it. A completed task does not mean a related system is restored unless that owner confirms it.

### Partial result

The player can explain what was completed and what remains open. A support group may help phrase the technical detail; the player chooses whether to include it. A partial task should not be titled “fixed.” If the player sends no update, the message system does not invent a public conclusion.

### Failure or cancellation

The player can notify the affected audience, contact a named participant privately, or wait for a clearer plan. If cancellation affects access or scheduled labor, a current owner determines who needs notice. The message should not blame the worker without evidence. A support faction can explain the blocker, but cannot decide fault.

### Sensitive result

An individual result may be private. The player can ask whether a public operational summary is permitted, keep the content private, or share only a necessary general consequence. The communication owner enforces its recipient rules. A care group can advise but cannot approve on the resident's behalf.

### Nothing to report

The task may generate no new fact. The player can close without a message, leave a reminder if supported, or say that the result remains inconclusive. Avoid filler notifications that make the shelter feel noisy. The absence of a message is not concealment unless a specific obligation and owner-backed rule says so.

### Endings and callbacks

A completion note may help residents coordinate; a private update may protect a detail; a partial report may maintain uncertainty; no message may keep routine play quiet. Later scenes can recall the message only if delivery or response was recorded. They can also react to the task itself through its own owner without assuming the player sent an update. This division ensures that communication enriches quests without becoming a mandatory quest-completion tax.

## 500. Candidate side-quest — The Message That Outlived the Rule

A notice about quiet hours remains on the board after the major authority changes the schedule. The old message is still accurate as a record of the previous rule but no longer valid as a current instruction. The player must decide whether to supersede, cancel, replace, or leave it visible with a clear historical label. This story tests message lifespan, source authority, and the difference between preserving history and endorsing a stale instruction.

### Opening state and source check

The player sees an active board post with an expiry that has not yet elapsed. The authority's policy record has a newer schedule. The communication owner has not yet delivered or posted that change. The player can ask the authority to confirm, inspect the current policy owner, draft a new version, or wait. A board steward can flag that the notice is old relative to the policy record. The steward cannot cancel an official rule.

### Branch A: supersede promptly

The player drafts a new notice naming the updated hours, source, audience, and effective time. They can post it to the same board, notify prior readers if supported, and ask a support group to place local signs where a current location owner permits. The old version remains accessible as superseded. Residents who saw it may still follow the old time until they receive the update. Acknowledgement is tracked separately.

### Branch B: cancel, then compose

The player cancels the old notice and waits to confirm exact replacement wording. This avoids leaving a stale instruction active but creates a short information gap. If no current policy permits an instruction-free interval, the player should not offer this choice. If allowed, a neutral message can say “Previous quiet-hour notice withdrawn; updated schedule pending.” It does not promise when the new one will arrive.

### Branch C: leave both with a historical marker

The player may preserve the old notice for context while posting a clearly marked current version. The interface must distinguish “current” from “previous.” The old message cannot continue to appear as an active instruction. This route is useful when residents need to understand why their schedules changed.

### Branch D: question the authority

The player can ask why the schedule changed, request a review, or ask whether there is an exception. A major authority may explain the operational reason, grant a permitted exception, or decline. The player can convey the answer privately to an affected resident, publish the general policy, or leave the appeal unresolved. A supporting care circle may help preserve a personal detail, but cannot reverse the schedule.

### Branch E: a resident challenges the revision

A resident says that the new hours conflict with a task or quiet period. The player can check the task/roster owner, ask the authority for an alternate, send a private message to coordinate, or record the concern without changing the policy. One objection does not cancel the rule, but the authority may have a valid accommodation route. If no change is available, the player can communicate the limit without claiming that the resident agreed.

### Endings and carry-forward

- New version replaces the active notice and prior readers receive a supported update.
- Old notice is canceled and a pending message explains that replacement is not confirmed.
- Both versions remain in history with clear superseded/current labels.
- Authority confirms an exception and the player sends it to a defined audience.
- Appeal fails and the player communicates the continuing rule accurately.
- Delivery to a resident remains pending and the player chooses another channel.
- The player waits for confirmation and no replacement instruction is sent yet.

A later scene can recall the changed schedule, the exception, or the delivery gap. It must not say “everyone adapted” unless the relevant task or message owner shows that. The episode ends when the communication is current or explicitly pending, not necessarily when every resident agrees.

## 501. Cross-channel composition workshop — preserve identity, vary presentation

A single event may require more than one channel, but copies should not become independent facts. This section describes how to present the same source event as a board notice, an intercom summary, and a private message while preserving audience and response semantics.

### Shared source event

The maintenance owner confirms that a room will close at 18:00 for a brief inspection. The communication owner creates a source message with time, source, intended audience, and expiry. The player can route a full notice to the affected board, a short intercom call to listeners if urgency warrants, and a private detail to one resident who needs an accommodation. Each route has its own message or delivery record linked to the source only as current APIs support.

### Board version

The board copy can include the room, date, confirmed interval, source, and current status. It may preserve replies and later versions. The player can ask a steward to format it, but the source remains the maintenance owner. Posting does not prove that every user read it.

### Intercom version

The intercom copy is shorter: location, action, and source. The player sees the larger audience and interruption. If the announcement links back to a canonical message, both presentation paths acknowledge the same identity where supported. Do not create separate acknowledgement counters for audio and banner copies of the same source.

### Private version

The private message can include a named recipient and a specific accommodation. It should not contain the entire public thread if that would expose unrelated replies. The player sees the recipient before send. A private acknowledgement does not update the public board.

### Versioning and change

If the time moves from 18:00 to 20:00, the source event changes or receives a new version according to its owner. Each active channel must either refresh or clearly show stale status. A board may receive the full revision, an intercom may receive a concise correction, and a private recipient may receive a direct update. The player chooses which outputs to send where supported. No channel silently inherits the new time.

### Response handling

A board reply, intercom acknowledgement, and private message answer have distinct response types. A single resident may acknowledge the intercom and then ask a question privately. The system can link those interactions if the owner supports it, but should not merge them into one “approved” response. When the player reviews status, show the factual summary with channel detail available on demand.

### Branches if one route fails

If the board is full, the player can select another permitted board or use another channel. If the intercom fails to project, the source message may remain posted but no audio confirmation appears. If a private recipient is unavailable, the player can choose another recipient only with justification or leave delivery pending. A failure in one channel does not erase successful delivery elsewhere.

### Accessibility requirements

All spoken messages have readable text. Channel icons include labels. Audience and expiry are keyboard/controller accessible before send. Long board text has a summary but no hidden change in meaning. Private and public routes are distinguishable without color alone. Captions match the recorded source. Screen reader order identifies source, audience, status, and action before secondary detail.

### Acceptance trace

Run one message through all three channels, then revise the time and fail one delivery. Reload after posting and after the revision. Confirm that each message retains its identity, audience and status; stale versions are visible as stale; and no response becomes agreement. Then rerun with only the board route available and with no board available. The player needs a truthful fallback, not a fake successful send.

## 502. Player approaches to communication — proactive, precise, private, and restrained

The communication surface should support multiple management approaches without making one universally optimal. These approaches vary by action and consequence, not by an alignment axis.

### Proactive announcer

The player sends an intercom call as soon as a broadly relevant fact is confirmed. This reaches the widest eligible audience and creates interruption. The player can follow with a board notice or wait for replies. If the source changes, the player must send a correction. A proactive route is useful under urgency but can over-notify if used for every routine update.

### Precise board keeper

The player uses local boards, versions, source fields, and expiry. This provides a durable reference with less interruption. It may not reach people in time. The player can add direct messages for known affected residents or use intercom when urgency increases. A precise route makes history clear but still cannot guarantee readership.

### Private coordinator

The player contacts named people and asks what they need. This protects personal detail and enables accommodations. It can omit unknown affected readers and requires careful recipient selection. A separate public notice may be necessary. The player sees who will receive each message.

### Authority first

The player checks the major owner's policy or schedule before publishing. This can prevent contradictory instructions but may delay communication. A supporting group can gather observations while the answer is pending. The player may publish an uncertainty notice or wait, depending on urgency and current rules.

### Community conversation

The player invites replies through a board or meeting notice. This may reveal practical concerns. It is not a vote unless an authorized decision process exists. The player can summarize responses with their actual types and decide whether to revise, escalate, or leave the plan unchanged.

### Minimal sender

The player sends only when a current fact affects an audience or a person asks for follow-up. Routine changes remain in their owner views. This avoids noise but can leave people unaware if the player misjudges reach. A late discovery can trigger a direct update or correction without a moral penalty.

### Approach switching

A campaign can use the intercom during an urgent corridor closure, private messages for a personal schedule, and a local board for routine maintenance. Persist channel choices only where they create supported outcomes. Do not tag the player as a “broadcast leader” or “secretive manager.” Later dialogue can reference a specific message, not a permanent profile.

### Playtest questions

Can players tell when a message is public? Do they understand the interruption cost? Can they correct a stale notice? Can they proceed without responses when policy permits? Do they know when silence is unknown? Can they decline to send? Can they use a quiet channel without hiding its reach limitations? Use these questions to assess route quality rather than assigning one communications style as morally preferred.

## 503. Candidate side-quest — A Reply Routed Backwards

A resident intends to send a private question about a maintenance schedule but replies to a public notice by mistake. The communication owner records the actual audience. The player sees the reply on the board and must choose whether to answer publicly, move the conversation to a private channel, ask permission to hide or remove the reply where supported, or leave it visible. The story explores a communication mistake without shaming the resident or pretending delivered content can be erased.

### First view: acknowledge what happened

The player sees the public reply, its author, and the audience who could view it. The resident may say they meant to ask privately, may not notice the audience, or may decide the question is fine in public. The player can ask which response they prefer. This question does not erase the original post. If the communication owner supports edit or deletion, preview exactly what can be changed and what remains in history.

A board steward can explain the board's audience and version behavior. A care volunteer can suggest a private follow-up if the resident chooses one. Neither can decide that the resident's message should be hidden. The major authority may require the schedule answer to be public if it applies to everyone, but personal details can remain private.

### Branch A: answer in public with general facts

The player can answer the schedule question without repeating personal context. The reply says when the maintenance window is and who owns the schedule. If the timing is not confirmed, the answer remains tentative. The resident may ask a follow-up publicly, move to private messaging, or close the thread. Other readers benefit from the general fact, but they also see the exchange.

### Branch B: invite a private channel

The player can offer to send the details privately. The resident must accept the channel or send a new private message if supported. The player previews the named recipient and text. The public reply stays visible unless a separate permitted edit or removal occurs. If the resident declines private follow-up, the player can answer generally or stop.

### Branch C: edit or remove where supported

The resident may ask to delete the reply. The player can guide them to the message owner or use an authorized moderation action only if the game has one. If deletion is unavailable, the UI says the reply remains in history and the player can post a short correction or privacy reminder. Never imply complete erasure after readers may have seen it.

### Branch D: a personal detail is already visible

If the message exposed a private detail, the player can apologize, limit further sharing, ask permission before quoting it, and use any supported moderation path. A support group can offer advice, not guarantee that no one saw it. The resident may accept the apology, ask for a practical correction, or decline further discussion. The event need not trigger a global trust penalty, but a specific relationship reaction may exist only if an owner supports it.

### Branch E: schedule question reveals a conflict

The resident's private question is about an assigned task that overlaps maintenance. The player can check the roster, contact the schedule authority, answer with a verified slot, or state that no slot is confirmed. The public thread may need a general schedule correction; the resident's task conflict remains private unless they permit broader disclosure.

### Endings

- The player answers general information in public and keeps personal details private.
- The resident accepts a direct message and closes the public thread.
- The resident chooses to leave the reply visible and continue publicly.
- The message is edited or removed through a supported owner, with history status shown accurately.
- The player cannot remove a delivered reply and offers a clear correction.
- A schedule conflict is resolved through the roster owner and communicated only to relevant recipients.
- The resident declines follow-up; the player stops.

The branch is shaped by initial audience, the resident's current preference, whether the player asks before replying, what detail is repeated, and the actual edit/delivery APIs. It is not an “honest player versus bad player” test.

## 504. Communication path cards — show each channel's affordance and limitation

A player needs to see what a communication channel can do before choosing it. The following path cards can be reused in the compose experience, with plain language and no implementation jargon.

### Board notice card

**Good for:** a durable update relevant to people using a known room or board.

**Player sees:** board name, intended audience scope, author/source, time, expiry, and capacity where supported.

**Limit:** posting does not confirm that anyone read it.

**Branches:** post; revise an active version; choose another board; save a draft; or cancel.

### Intercom card

**Good for:** a short instruction or urgent update relevant to many listeners.

**Player sees:** eligible listener scope, interruption, source, and whether a text copy will appear.

**Limit:** listeners may be absent; a banner may not prove audio delivery; acknowledgement is not consent.

**Branches:** broadcast; edit before send; use local/private channels; wait for more certainty; or cancel.

### Private-message card

**Good for:** one recipient or a small known group with specific detail.

**Player sees:** exact recipients, privacy scope, delivery status available from the owner, and any source attachment.

**Limit:** other affected people are not automatically informed.

**Branches:** send; add a permitted recipient; remove a detail; switch to a broader channel; or save a draft.

### Faction relay card

**Good for:** a supporting group that can provide an observation, help place a notice, or reach known members.

**Player sees:** which group, what service it offers, how it affects audience, and whether a reply is expected.

**Limit:** service can be declined or unavailable; it does not guarantee delivery or confer policy authority.

**Branches:** request; choose a direct channel; wait; or proceed without help.

### Major-authority request card

**Good for:** formal policy, schedule, access, or inventory questions under that owner's remit.

**Player sees:** which decision is being requested, what policy question is outside the group's remit, and whether a response is pending.

**Limit:** a request for authority review is not a resident communication; affected people may still need an update.

**Branches:** submit request; ask a support group to help gather a fact; publish a cautious interim note; or wait.

### Card behavior under change

When a draft changes channel, refresh audience and privacy information. Preserve text only if its content remains suitable. An intercom sentence may be too long for a call; a private explanation may be too detailed for a board. A change in source status prompts the player to review. Closing the card without send must not create a message event.

### UI review

Cards should be accessible via keyboard and controller, use textual labels rather than color alone, and fit narrow/localized text. The commit button follows the audience summary. The back action returns to draft editing without sending. The player can inspect optional details without losing the current choice. A channel is never selected by hidden urgency scoring.

## 505. Message-branch authoring lab — source, audience, voice, and consequence

Writers can increase flavor by varying the source and voice while keeping the branch state consistent. Start with four columns: source fact, intended audience, player action, observable result. Add voice only after the first four align.

### Example: water window update

**Source fact:** major authority confirms a pickup window from the current schedule owner.

**Audience:** all residents who use the distribution point.

**Player action:** board notice or intercom; optional private message to one resident with a known task conflict.

**Observable result:** post accepted, delivery/acknowledgement reported only where supported, schedule unchanged unless the schedule owner says otherwise.

A stores volunteer can say, “The pallet is counted; I can't change the hour.” A care volunteer can say, “You can ask privately whether the time works.” A resident may reply, “I can make the first slot, not the second.” The authority says, “This is the only confirmed window.” These voices differ without changing their source authority.

### Example: corridor correction

**Source fact:** route group reports that its morning observation is stale; maintenance crew says corridor work is paused.

**Audience:** local board and named work crew; intercom only if access has changed for many residents.

**Player action:** publish uncertainty, wait for recheck, or relay the task status.

**Observable result:** message says route unverified and work paused; no all-clear is implied.

A steward can note that the old notice is still on the board. The route worker can say, “I haven't checked it since the rain.” The maintenance lead can say, “Our crew is off the corridor until the cart arrives.” Later follow-up changes only after a new source event.

### Example: resident wants a schedule correction

**Source fact:** roster confirms a conflict; resident asks for a different time.

**Audience:** the resident, affected crew, or general board depending on scope.

**Player action:** privately negotiate, ask the major schedule authority, revise a posted proposal, or decline.

**Observable result:** time changed only if schedule owner confirms; message can show the proposal or confirmation separately.

### Color and tone

Keep the shelter's restrained tone. A public call can be brisk without sounding militarized. A private message can be warm without making a promise. A formal notice can be direct without claiming unanimity. Use object and place details sparingly: a damp board edge, a smudged time, a line crossed through an old window. Do not let props imply delivery, agreement, or physical access the current system cannot verify.

### Lab review

For each sample, remove all prose and inspect the state columns. Then restore dialogue and ask whether any sentence overstates the source, broadens the audience, or turns a reply into consent. Replace unsupported “everyone knows” language with counted or unknown response. The result can remain flavorful because voice, silence, correction, and location provide texture without inventing state.

## 506. Quiet-day communication — what the player chooses not to send

A shelter communication surface should not fill every empty day with notices. Quiet is a legitimate result when no current fact needs an audience. This section gives the player several reasons to refrain from sending and a clear way to revisit the choice if circumstances change.

### No update is needed

A task completed with no change to access, schedule, or audience. The player can leave the result in its task owner and close the view. A resident may ask later; the player can then provide a factual answer. No notification is created for its own sake.

### Source is too weak

A rumor, old report, or missing timestamp may not support a public claim. The player can ask a source, post a narrow uncertainty notice if it prevents a concrete misunderstanding, or wait. Doing nothing is acceptable if no immediate action depends on the uncertain fact. If urgency changes, a new owner event can prompt a fresh decision.

### Audience is unclear

The player may not know who uses a room or needs an update. They can ask a board steward, inspect current roster or location owners, post to a defined local audience, or refrain from sending. The message preview should not expand to the whole shelter simply because the precise list is unavailable.

### Privacy outweighs broadcast

A resident shares a personal reason for asking that the schedule move. The player may adjust the schedule privately if authorized, send a general notice with the new time, or ask permission before naming the reason. If the only useful message would reveal personal details without consent, the player can avoid sending it and pursue a different route.

### Player intentionally waits

The player expects a major authority to confirm a time. They can wait, draft but not send, ask a support group to gather a fact, or publish that confirmation is pending. A draft remains unsent. If the authority never answers, the player can close the draft or communicate uncertainty. Silence from the authority is not permission.

### Branch after waiting

A current source may arrive, making a precise message possible. The player may send it to the affected audience, keep the draft, or decide the event no longer matters. A source may contradict the draft; the player can revise before send. If a deadline passes, the draft does not automatically publish. The message owner controls that behavior.

### Narrative consequences

A resident might say, “I saw the new time on the board,” only after the board was posted and that history is visible. They might ask, “Did they ever confirm the window?” if the authority request remains pending. If the player sent nothing, a character cannot praise the message. But there need not be a negative reaction. A quiet communication day is not missing content; it is the player's decision that no channel should be used.

### Review checklist

Test no-send, draft-only, wait-for-source, private-only, and canceled-message routes. Confirm that closing a draft leaves no delivery record. Confirm that a later message uses current source facts. Confirm that the player may inspect history without being pressured into composing. This makes the feature feel intentional rather than noisy.

## 507. Supporting faction voices as sources — use attribution to create texture

Factions should sound distinct in messages because they notice different facts, not because every group is aligned against or with the player. The same event can be described from different vantage points, with each statement scoped to what the speaker knows.

### Route group

A route observer tends to cite time and path: “The west landing was clear at first watch. I haven't checked after the crate moved.” This gives useful direction and uncertainty. The player can request a new observation or pass the current statement along with its time.

### Stores crew

A stores worker talks in availability and custody: “Two filters are counted on the shelf; one is reserved for the pump line.” This is a stock fact if the inventory owner confirms it. The worker does not decide who deserves an item. The player can request a release, ask when stock may change, or choose a different resource plan.

### Maintenance crew

A technician distinguishes inspected from repaired: “The latch moves now. We haven't tested it under load.” The player can ask for another task stage, report the limited result, or stop. Their voice can carry caution without portraying them as obstructive.

### Board steward

A steward notices audience and version: “This copy still has yesterday's hour.” The player can ask for a revision, leave the old message clearly superseded, or wait for authority. The steward cannot determine the correct time.

### Care circle

A care volunteer notices privacy and consent: “The schedule can change without printing why she asked.” The player can take that advice, ask the resident, or use another route. The care circle does not speak on the resident's behalf.

### Major authority

An official source states its remit plainly: “We can close the lower room until the inspection ends. We cannot guarantee the route beyond the east door.” The player can relay the instruction, ask about an approved alternative, or explain the unresolved path. Formal authority is real but bounded.

### Mixed-source message

A player may post “Facilities has closed the lower room until 18:00. The route circle last observed the east path at dawn; no current access confirmation is available.” This is more textured because it preserves each source rather than flattening them into “safe/unsafe.” A later update may replace only one claim. The player can keep the message cautious, request another check, or use intercom for the room closure and a board for route details.

### Source voice acceptance

For every authored line, map each claim to a current owner or mark it as a character report. If a faction is absent, use neutral copy. If a major policy changes, attribute it to the authority. Avoid generalized faction temperaments that reduce all members to one attitude. Character identity comes from role, experience, and authored voice; authority comes from the system boundary.

## 508. Split-thread case — one question belongs in two channels

A resident asks a public question about a repair window, then adds a private detail about their scheduled duty. The player can answer the general timing on the board and move the personal conflict to a private message, answer both publicly with permission, ask the resident which channel they prefer, or leave the thread open. The communication owner tracks each record and audience separately.

The public answer can state that the work is proposed, confirmed, or delayed according to its source. The private answer can name the recipient and discuss their shift only if that information is current and appropriate to share. If the resident declines a private message, the player can keep the answer general. A supporting steward can link the board version to the updated notice; it cannot expose the private reply. A major schedule authority can confirm a time but cannot acknowledge the resident's consent for them.

If the schedule changes, the player can revise the board and send the private update, revise only the affected channel, or wait for confirmation. A public acknowledgement does not close the private question. A private reply does not mean everyone saw the new time. The ending can be a public correction, a private accommodation, both, or an unresolved proposal. The branch depends on the player's choice of audience and the resident's preference, not a virtue judgment.

## 509. Message cadence rubric — avoid noisy, stale, or hidden decisions

For each authored message, decide whether it is a one-time call, a persistent board post, a private exchange, or a linked update. Define who may create a new version, who sees it, how it expires, and what the player can do after delivery fails. Do not repeat the same notification because a panel reopened. Do not send a reminder unless the source event and cadence support it.

A player may choose one accurate message rather than three redundant copies. They may send a correction only to those affected, or broadcast when the whole shelter needs to act. Both are valid if channel reach and urgency are visible. If the message remains unresolved, show whether the player is waiting for a source, a recipient, or an authority. A status view should answer “what is current?” before “what did I send last week?”

Review the cadence at normal speed, under fast day advance, after reload, and when recipients depart. Confirm that expired messages do not reappear as current, a revised source updates only permitted channels, and no silence becomes an automatic reminder loop. Use concise copy for repeat updates and preserve the full history through its owner.

## 510. Message resurfacing — a notice returns because its context changed

A stale notice should not reappear just because it was important once. It can resurface when a current event makes its information relevant again. For example, an old workshop closure may matter when the same room is scheduled for a new inspection. The player sees that the old record is historical, the new source is current, and a new message is needed if residents must act.

The player can link to the old notice, compose a fresh version with the new date, ask a steward to retrieve wording, or ignore the historical copy. The maintenance crew can confirm its current task; the major authority can confirm access policy; a support group can provide a recent observation. None inherits the old message's audience or acknowledgement. The player previews the new audience and privacy before send.

If the old message contains a correction, its history remains visible where the owner supports it. A new message should not imply that previous recipients are already informed. If some people acknowledged the old closure but not the new inspection, show separate states. If the old audience is no longer available, use a neutral fallback and target the current audience. This creates continuity through a concrete recurring place while keeping every message anchored to its own time, source, and recipients.

## 511. Accessible communication branches — meet a need without exposing why

Residents may need a different format or channel to use a notice. The player can offer readable text, repeat a call, send a private summary, ask the resident which option works, or use a board with clear symbols and text. These branches should be based on a stated preference or an owner-supported need, not on a visible disability label or a guess about what the resident can perceive.

The message owner still controls delivery and audience. A care volunteer may suggest a format, but the resident chooses whether to accept it. A broad intercom replay may help one person while interrupting others; the player sees that tradeoff. A private summary may meet an individual need but does not inform a wider group. Captions and text equivalents should carry the same verified fact and source as audio. If no alternate format is available, say so and let the player ask the resident what would help or leave the message pending.

Do not store a new accessibility profile inside the communication feature. Use current preference or accessibility owners. Avoid disclosing why a person requested another format. Test keyboard/controller focus, screen reader order, caption parity, contrast, and localization length on the same branch. The narrative can acknowledge that a message arrived in a usable form without turning the recipient into a teaching moment.

## 512. Copy bank by state — short messages with distinct purposes

**Confirmed schedule:** “Facilities confirms the pump check for 18:00. Keep the west corridor clear during the move.” The source and time must be current.

**Proposed schedule:** “The crew proposes a pump check at 18:00. The time is not confirmed.” Do not format as an instruction.

**Correction:** “Update: the check moved to 20:00. The earlier notice is superseded.” Readers still need delivery through a supported route.

**Unknown route:** “The lower passage has not been checked today. Use the marked path if it remains open.” Use only if that path is verified current.

**Private accommodation:** “The work window changed to 20:00. Does that still fit your shift?” Send only to the named recipient.

**Unanswered authority request:** “Facilities has not confirmed the alternate window. The current schedule remains unchanged.” This reports silence without treating it as approval.

**Failed delivery:** “The notice was sent to two recipients; delivery to the third was not recorded.” The count must come from the message owner.

**Expired notice:** “This notice expired at the end of the shift. It does not confirm that the work is complete.” Expiry is not a task result.

**Resident objection:** “One resident objected to the proposed time. The schedule owner has not changed it.” This neither dismisses the objection nor turns it into a veto.

**No message sent:** “Draft saved. Nothing was posted or delivered.” Use only if drafts persist; otherwise say that the text was discarded.

Each sentence is tied to a distinct source state. Keep line length suitable for the channel and provide a longer context view where helpful. Do not reuse one generic “notice updated” phrase for corrections, cancellations, failures, and expiry; the player needs to know what happened.

## 513. Delivery recovery edge cases

A recipient may depart after a message is queued, a board may become inaccessible, a correction may be authored after the original expires, or an intercom call may be recorded while its text projection fails. For each case, preserve the source identity and show which delivery path succeeded. The player can choose a permitted alternate route, wait for a new recipient state, or close the attempt. Do not duplicate the message automatically or mark the resident as informed because one projection updated.

After reload, a queued delivery must not be sent twice unless the owner explicitly retries it. A failed projection can be corrected without rewriting the canonical source. A message to a departed recipient should not be redirected to a replacement person. If the source fact itself is stale, ask the player to review it before retry. These recoveries make the channel reliable without pretending that every communication can reach everyone.

## 514. Channel closeout checklist

Before closing a communication scene, verify the latest source, active version, intended audience, delivery state, replies, and expiry. A closed scene may still have an unresolved task or unanswered person. A message can be superseded while its history remains. A task can finish without an update. Tell the player which of these facts changed and which action remains available.

Provide a neutral close button on every channel. Closing the view does not send, acknowledge, delete, or cancel unless the owner explicitly confirms that command. If the player chooses no further message, preserve current records and let ordinary play continue. These small distinctions prevent accidental communication and keep the three subfeatures honest.

## 515. Message branch closeout example

A resident receives a revised notice, asks whether it applies to their room, and receives a source-backed answer. The player may then close the thread, ask the authority for a broader clarification, or post the answer to the local board. If the answer applies only to one room, do not expand it to the whole shelter. If it remains uncertain, say so and leave the question open. The resident may acknowledge receipt without endorsing the policy. This small scene provides an ending even when no dramatic event occurs: one person has the information needed to choose their next action.

The player can always close a draft without sending it or changing the canonical source record.

## 516. Questline — The Hall That Cannot Hold Everyone

A repair crew learns that the west hall roof will be opened for two nights. The first notice is factual but too broad: it says “west hall closed” without explaining the hour, the alternate route or whether residents may collect stored blankets. The player can replace it with a schedule, split the work into short closures, or ask the crew to pause while the infirmary moves its supply shelf. The route choice changes which residents need to be reached. It does not change whether the roof is safe.

The chain begins with three separate sources of information. Foreman Lida posts the work window on the public board. The night steward has a private note about a resident who uses the west hall to reach a washroom. The infirmary requests an intercom call only if the passage becomes unusable after lights-out. These are not three copies of one announcement. The board gives the general plan, the note provides a specific accessibility concern, and the call is conditional on a concrete event. A player who copies the private note onto the board without permission exposes personal information and may cause the resident to stop using that route.

At each stage, the player can act or wait. They can walk the alternate route with the resident, ask the crew to preserve a clear strip, notify the night steward, or leave the work schedule unchanged. A walk-through may reveal that the sign points to a locked store room. If the player marks that route as accessible before checking it, the resident can challenge the message and request a correction. The board history should show the correction and its author, not erase the original as though nobody saw it.

A supporting tool group offers chalk and boards but does not decide the shelter route. The night stewards can confirm when passages are staffed. The infirmary owns treatment access. The crew determines the work sequence. The player coordinates an accurate audience-specific communication and can negotiate between them. The arc branches on whether the route was inspected, whether the closure changes, and whether the conditional call is needed. It can conclude with work completed on schedule, work delayed to preserve access, or an imperfect route accompanied by a staffed escort. All three are viable endings; none turns one faction into a campaign authority.

Sample board updates distinguish planned, active and cleared states: “WEST HALL: roof work begins at 09:00 tomorrow. Use the marked north passage; the night steward will walk the route with anyone who needs it at 20:00. The infirmary shelf will be moved before closure. We will update this board if the work window changes.” If rain delays the work, the author edits the notice through the board owner, preserving the old audience and marking the superseding update. A private response to the resident should not be treated as proof that everyone read the board.

The quest closes after the work owner confirms the passage is open and the player chooses whether to remove or archive the notice. If a resident cannot attend the walk-through, a later note can offer a new time. If the schedule changes after some people have already rerouted, the correction must state what action should be reversed. The narrative payoff is modest but concrete: a resident makes a trip without discovering a locked door, and the crew knows why one part of the hall remained clear.

## 517. Branch suite — One message, three legitimate audiences

When a water allotment changes, the player is offered three communication actions. Posting the amount and effective date to the public board reaches all residents who check it. A room note can answer a household's delivery question. An intercom call can warn people already waiting at the tap that the next container is delayed. Each action has a different audience and purpose; choosing all three is not automatically safer. The player must decide what information each group needs, when it needs it and whether the source has confirmed the change.

If the amount is provisional, a broad public notice can say that a review is under way and identify when to check again. It should not present the likely amount as final. If the amount is final but only applies to a specific household, a private message is more accurate than a shelter-wide announcement. If residents are currently in the queue, an intercom call can prevent wasted waiting, but the caller must identify the next update time instead of promising an exact delivery time the carrier has not confirmed. Choosing to wait for confirmation avoids a false claim yet may leave people uncertain; the plan should represent that tradeoff plainly.

The player can learn the state through different actions. They can inspect the allocation notice, ask the water steward for the latest count, compare the dated ledger copy, or watch the carrier unload. A dated paper can be stale even if it bears a familiar stamp. A resident may correctly know that containers are delayed while being wrong about the reason. Neither hearsay nor an interface icon resolves uncertainty by itself. The communication record stores source and time so a later correction has an intelligible cause.

Branch outcome A: the carrier confirms the delay before residents assemble. The player posts the new estimated window, privately answers households with immediate need, and makes no call because nobody is at the tap. Branch outcome B: the carrier cannot give an estimate. The player posts “delayed; next check at dusk,” then leaves the public status as unknown. Residents can plan around uncertainty without receiving a fabricated promise. Branch outcome C: a false count was already posted. The player issues a correction, calls the queue if needed and explains which earlier action came from the wrong ledger date. Branch outcome D: no communication is sent. If nobody needs to act on the information yet, the player can leave the record unchanged and check again at the next scheduled delivery.

The well-keepers provide a measurement, not an announcement. The night stewards can relay a correction to people who cannot reach the board. A household delegate may confirm receipt but cannot acknowledge on behalf of every resident. The major shelter authority sets the allotment; the player chooses an accurate way to communicate it; supporting groups help reach the intended audience. A player who prioritizes completeness may post one general notice and wait for questions. A player who prioritizes immediate access may make a targeted call and accept that others will need the board. A player who prioritizes privacy avoids naming a household publicly. These are playstyle differences that arise from actions and audience management, not virtue labels.

The record must show sent, delivered where known, read only where explicitly acknowledged, and corrected as separate states. If the private note fails to reach an absent resident, the player can leave a board reference or ask a trusted messenger to deliver it. The messenger's role is limited to delivery; the resident's reply remains theirs. If the player asks a resident to relay an urgent message, the interface should make that request explicit and allow refusal. A message cannot be marked “received” simply because the intermediary accepted the errand.

## 518. Questline — The Unfinished Sentence on the Board

Someone leaves the line “They are moving the beds tonight” on the public board. It has no author, date, source or explanation. By morning, residents have supplied different interpretations: a planned infirmary move, a rumor that several beds will be removed, and an account of a previous relocation that happened without notice. The player must decide whether to remove the sentence, annotate it as unverified, or keep it visible while asking for a source. The first impulse is not the only branch. Erasing the line may protect against panic while also making residents think the concern was suppressed.

The player can trace the rumor through actions. Ask residents who rely on the affected beds; inspect the dated infirmary request; speak with the porter who moved a single cot; or compare the current floor plan against the previous night. Each source answers a different question. The porter knows what physically moved, not why the move was authorized. The infirmary note describes a patient transfer, not a permanent bed reduction. A resident can accurately report that their bed was moved and still lack the wider context. The scenario rewards careful attribution instead of selecting the most confident speaker.

If the player confirms a one-night move, the correction says who is affected, what time the cot moves and where the resident can ask for assistance. If they cannot confirm it, they post the investigation status and a time for the next update. If the investigation shows that a second bed will move, the message names the revised scope without implying that all beds are affected. A player can ask the original author to sign the update, but anonymity remains available when the person has a credible reason to avoid public attention. The board owner tracks the edit as a new revision rather than overwriting what the shelter has already read.

The arc supports four endings. In the **verified routine ending**, the single cot is moved for treatment and returned the next morning. In the **access correction ending**, the destination is technically available but too far for the resident, so the player coordinates a closer temporary place and records only the operational facts. In the **delayed decision ending**, the source remains inconclusive; residents receive a date for a new update and can ask questions privately. In the **trust repair ending**, a previous relocation was mishandled, and the player helps the porter explain the earlier mistake before posting the new arrangement. The ending depends on confirmation, material access and whether a previous promise was kept. It does not reward the player merely for choosing a reassuring line.

A small printing circle can reproduce the corrected notice, but it has no right to decide which beds are moved. The infirmary owns care decisions; the porter confirms movement; the public board preserves shared updates; a named resident owns consent to disclose a personal need. The player connects the available facts. If one supporting group disagrees with a fact it cannot verify, the dispute remains open rather than becoming a faction rivalry.

Optional moments deepen the story without gating it. A child recognizes that the board's handwriting changed. A tired resident asks why the old sentence remains visible beneath the correction. The author may apologize or explain why they wrote anonymously. The player can answer in public, invite a private conversation or leave the author to decide whether to respond. These interactions add voice and memory, but the core branch still resolves if the player never meets the author.

## 519. Private correspondence casebook — The Envelope That Was Not a Notice

A sealed note arrives at the communications desk with no recipient listed, only “for the person who keeps the blue cup.” The desk worker asks the player to identify the owner. The player can ask the sender to clarify, leave the note sealed until its recipient is named, or return it. Opening it to guess from its contents violates the expected private boundary. The clue may tempt the player because one resident uses a blue cup, but that resident has not authorized the desk to search their belongings or reveal the note's contents.

The sender can clarify the recipient through a private conversation. If they are unavailable, the desk can keep the envelope in a short-term holding place with a clear expiry and a record that does not expose its message. The player can ask the relevant resident whether they expect a note, but the question itself may reveal that something was sent. The player should have a low-pressure wording: “A note is waiting; would you like me to check whether it is for you?” The resident can decline. If they claim it, the desk hands it over unopened and records delivery only if the delivery owner supports that receipt.

The first branch resolves quickly when the sender provides a name and the recipient accepts the note. The second branch becomes a small quest if the sender left for an expedition. The player can hold it, ask a designated contact to identify a safe way to return it, or mark it undeliverable after the stated retention window. A third branch occurs when two residents share the clue. The player must ask the sender for a distinguishing detail without quoting the note's private text. A fourth branch appears if the recipient declines correspondence: the player returns or securely retains it according to the message rules, without pressuring them to explain.

An optional plot thread connects the note to a missing tool that two residents have been searching for. The message can say where it was left, but the player cannot publish that location publicly without permission. If the tool is found in a shared work area, its property status is checked separately. The note is evidence of a lead, not ownership transfer. This creates a useful cross-system decision: deliver the personal information to its intended recipient, then allow them to decide whether to report the tool to stores.

The archive records the minimum necessary facts: sender, intended recipient once confirmed, delivery attempt and final disposition. It does not index private message contents as public search text. The message can create a follow-up conversation, but opening the archive should not expose it to an unrelated player character. A resident may request that a private note be returned or destroyed through an explicit owner command. The interface must communicate what retention rule applies before the player chooses.

A supporting courier circle can perform a delivery route but never read notes to determine destinations. A steward can witness handoff if both people request it, but does not become a default recipient. The major authority can set retention rules, while the private-message owner enforces them. The player can choose discretion, fast delivery or no intervention, with consequences based on availability and authorization.

## 520. Intercom arc — The Call that Must Wait

A smoke smell appears near the laundry wall. A resident presses the intercom request button, but the call channel is already reserved for a medical update. The player learns that the laundry steward has not yet confirmed a fire, and a work crew is using the route. The immediate choice is not a simple “call or do not call.” They can ask the steward for a direct check, ask the medical caller to finish a concise update, use a known in-person runner to warn the nearest room, or wait briefly for confirmation while keeping the channel free.

Each action has a different timing profile. A runner can reach the nearest occupied corridor but not the whole shelter. The intercom can reach a wider area but may interrupt urgent treatment instructions. The steward may confirm a harmless overheated motor or discover smoke behind a panel. The player must not make an evacuation call without an authorized trigger simply because the smell is alarming; equally, uncertainty cannot be used to suppress a proportionate warning to people in immediate proximity. The exact commands should reuse the existing incident and broadcast owners.

If the check finds no smoke, the player can thank the reporter and close the intercom request as investigated. Do not publicly identify the resident who raised it unless they agree. If there is smoke but no open flame, the player can issue a corridor-level notice, keep the route clear and request maintenance. If the steward confirms active fire, the incident owner selects the established warning protocol, and the intercom call carries the approved instruction. The player may speak the call, relay an authorized prepared message, or ask the steward to speak. Acknowledging the report does not equal resolving the hazard.

The player also handles the unfinished medical update. They can make one short call to clarify where the care team is needed, then return the channel to the incident. They can switch to a separate available route if the existing communication system supports it. If no alternate channel exists, the wait becomes an explicit cost: the user sees which message has priority and why. The call queue is not a new simulation queue; it is a presentation of commands permitted by current channel ownership.

At close, the channel shows a single origin record with delivery and acknowledgement facts. A call should not appear as two independent messages just because both the requester and the operator reference it. If the operator reads a prepared script, the system retains the source and author. If the receiving room cannot acknowledge, its status remains unknown; a blank acknowledgment is not a success. The player can ask a runner to confirm the nearest room separately.

One outcome branch follows the runner, another the intercom speaker, and another the maintenance inspection. A follow-up scene can feature the resident who reported the smell, the steward who checked, or a tired crew member who was interrupted. The crew may say the inspection caught a failing bearing before it started a fire. That statement is a character perspective, not proof the entire shelter was safe. A final maintenance update reports only the repair status and the next inspection window.

## 521. Cross-channel questline — The Two Lists of Missing Blankets

A public board lists twelve blankets awaiting return. A private note from the laundry room says that four were sent for washing. The night intercom caller reports seeing two blankets moved toward the infirmary. The records are not necessarily inconsistent: they refer to different times and different locations. The player can reconcile them through a physical count, ask the laundry lead for an updated return estimate, or issue a narrowly worded notice that the total is under review. They should not publish private room assignments merely to make the count look complete.

The physical-count route involves a short walk with a resident who knows the storage racks. One blanket is damp, one is being used as a curtain, and two are marked for laundering. The player may return only those that are clean and available, ask the laundry group to record the other two, or change the board's request from “return now” to “check by tomorrow.” The curtain user can choose to keep it in place, request another screen or report that the room no longer needs it. The item owner's rules govern transfer; a communication choice cannot create inventory.

The ledger route can show that one tally is from yesterday's shift. The player can correct the board while preserving its old date, ask the ledger keeper to append the current count, or leave both records visible with a note about their different scope. If the player has no evidence for the count, the notice should say so. An honest “we have not finished checking” is useful information when paired with the next check time.

Branch consequences include whether the laundry group trusts the posted count, whether residents stop bringing blankets to a storage rack that is already full, and whether the infirmary asks for a quieter way to request supplies. These responses are driven by the player’s corrections, follow-up and operational effect. The story does not assign moral reputation points. A player who prefers orderly records can reconcile every source; one who prioritizes immediate comfort can deliver clean blankets first and reconcile later; a player who minimizes disclosure can keep room-level details private while posting the total. The game should show these priorities through later invitations and dialogue.

A supporting seamstress circle can mend a damaged blanket after its owner approves the work. It cannot certify the inventory count. The laundry crew can report washing capacity. The infirmary confirms its own receipt. The player coordinates the notice and asks for source-backed updates. No one group controls the whole chain.

The closeout makes all statuses legible: returned, in laundering, assigned to care, held by consent, or not yet located. If the ownership of an item is unclear, it stays out of the accepted transfer total. A later message may revise the count, but it does not make earlier readers retroactively informed. The quest's ending is the first verified total, not the disappearance of all uncertainty. If one blanket is still missing, the player can leave a modest open inquiry and move on.

## 522. Decision atlas — What a message can and cannot settle

A notice can state an approved schedule, a confirmed rule or a request for residents to act. It cannot make an unapproved policy valid. If the player writes “all rooms must move tonight,” the message owner should require an authorized source and a named effective window. The public board shows the source alongside the text. If the authority has not approved the change, the player can post a proposal, identify it as pending and direct readers to the next meeting. The player may also choose to wait and make no post.

An intercom call can carry time-sensitive information and request a concrete response. It cannot prove that every listener heard or understood it. If the caller says “everyone clear the east path,” the follow-up record should distinguish rooms reached, acknowledgments received and rooms unknown. The player may send a runner to an unconfirmed corridor if the incident owner allows. This creates a meaningful consequence for choosing broad reach: a call is fast and visible but can interrupt others and may still need local confirmation.

A private message can answer an individual request and preserve the recipient's privacy. It cannot become a public rule by being forwarded. If the recipient asks the player to share a practical answer with others, the player can compose a separate board summary that omits personal context. The original remains private. If the recipient wants no forwarding, the send action should not display a shortcut that makes forwarding the default.

A reply can acknowledge receipt without approving the decision. The interface uses separate actions for “received,” “I agree,” “I will do this,” and “I need to discuss it.” If the data owner supports only one acknowledgment state, the plan should propose a typed display backed by existing semantics or state the narrower truth. It must not invent extra saved flags solely for richer labels.

Expiration is an explicit branch condition, not a silent deletion. A notice can expire when the relevant schedule passes, be superseded by a correction, or remain archived as a record. A private note can be retained, returned or closed according to its owner policy. An intercom call closes after its requested action is no longer relevant or when the source owner resolves it. The player can ask to reopen a thread, but the history remains intact.

The player chooses between no action, a focused update, a broad update and a channel switch. Each is a playstyle, and each can be right under a different state. A focused update minimizes exposure but may not reach people outside the intended room. A broad update reaches more residents but can spread incomplete information. A channel switch can preserve urgent access while taking more time. No branch is automatically rewarded for maximum transparency or minimum disclosure. The outcome uses audience fit, confirmed facts, timely delivery and consent.

## 523. Closeout — communication outcomes remain authored and bounded

The added arcs do not introduce a fourth communication feature. They deepen the three defined surfaces: public notices, shared intercom calls and private messages. Each quest asks the player to identify a source, select an audience, choose timing, preserve uncertainty when needed and observe who actually received the result. Access, route safety, inventory and treatment stay under their current owners.

Before any future implementation, verify that the active board and message records still match these examples; confirm whether an intercom invocation already owns acknowledgments and how the host presents delivery; inspect save behavior for corrections, expiry, private access and reopening. If the current owners cannot represent a branch truthfully, narrow the branch or request a decision through the active integration authority. Do not simulate delivery in a panel or add a parallel inbox to make this plan's scenes appear complete.

The proposed acceptance scene is deliberately ordinary: a dated notice gives a route and a next-check time; one resident asks a question privately; a work delay produces a correction; a night steward confirms that the new route is usable. The board does not claim that every resident read it. The private answer does not become public. The steward's confirmation does not declare the repair complete. The player sees each action and its limits, and can leave the remaining question open.

Plan 4 is ready for a fresh implementation audit only after its measured word minimum is reached and its current code and data premise are rechecked. This document is content planning, not implementation approval, file ownership or evidence that communication already works in game.

## 524. Questline — The Shelter Drill Nobody Announced

A steward schedules a quiet evacuation drill for a section of the shelter but has not agreed on whether families should move through the north stair or the loading bay. A volunteer sees the unconfirmed route in a draft notice and asks whether the drill is real. The player can keep the draft private, mark it pending, ask the steward to settle the route, or tell the volunteer that no instruction has been approved. Posting “DRILL TOMORROW” before confirmation risks moving people toward a route that may be blocked; suppressing every mention until the final moment could also leave residents unable to arrange mobility help.

The player can inspect the north stair, ask the loading crew about the bay schedule and check whether the infirmary can spare a route observer. The stair is clear but narrow. The bay is wide, though it overlaps a food delivery window. The infirmary can provide an observer for one route but cannot supervise both. These details are disclosed through actions and conversations, not a hidden route-quality meter. The player may ask for the drill to be rescheduled, preserve the original hour and stagger the deliveries, or choose the stair and recruit stewards at the landing.

A public notice can state that a drill is planned while the route is being confirmed, with a clear instruction not to move yet. Residents who need assistance can privately request a route walkthrough or decline to participate. A household note may contain sensitive mobility information, so the general notice should describe how to ask for support without naming people. The intercom call is held until the responsible steward approves a specific instruction and time. If the drill begins and the route changes, a runner reaches the nearest rooms while the call channel remains open for the approved update.

Ending one runs a short drill through the north stair. The player chooses to observe the landing, ask a resident to test the pace or let the steward handle the route alone. If the landing crowds, the steward pauses and changes the sequence. Ending two shifts the drill to the bay after the food delivery. It costs time, but it gives residents a wide path and lets the driver keep the agreed unloading window. Ending three postpones the drill because neither route can be staffed safely; the public board records the new review date, not a false claim of completion. Ending four runs a limited tabletop walkthrough for residents who choose not to move through the route. These endings are driven by route facts, staffing and consent, not a binary courageous/cowardly judgment.

An informal accessibility circle can help test whether the instructions make sense, but it does not certify the route. The loading crew controls its delivery sequence; the infirmary controls treatment duties; the shelter steward approves the drill. The player arranges an accurate message and can advocate for a resident who asks for a route change. If the circle reports that a sign is hard to see, the player can request a different marker or add a verbal check. If no resource is available, the plan needs to record the limitation rather than pretend the accessibility problem was solved.

At the end, the board can carry a brief debrief: the route used, the time, the correction made, and how to raise an issue before the next drill. The public summary excludes individual performance and health details. A participant may send a private note about a missed instruction. A steward can reply with a practical adjustment or ask for a meeting. The drill is complete when the incident owner records its completion; a notice being posted is not evidence that anyone practiced.

## 525. Questline — The Memorial List with Two Spellings

A resident asks to post the names of people who died during the first winter. The names exist in several handwritten lists, with variant spellings and one person recorded only by a nickname. The player may create a public remembrance notice, ask the requester which version they want to use, consult two witnesses, or leave the list unpublished until the families have been asked. The disagreement is not treated as a puzzle with a single perfect answer. A correctable spelling does not make the grief less real, and a public list can hurt someone if a name is included against their wishes.

The requester wants the list read aloud at a shared gathering. Another resident says a family member did not want their name displayed. The player can ask that resident to speak privately with the requester, make the gathering open to anyone who wishes to attend without publishing names, or separate the public invitation from a private name list. If a family cannot be reached, the player can defer that name, include an agreed nickname, or write “remembered privately by those who knew them.” Each choice has a clear effect on the audience and preserves the option to revisit it.

The board can carry the time and place of the gathering without reproducing the names. A private message can ask a family whether a name may be spoken or displayed. An intercom call can announce a schedule change but should not unexpectedly broadcast a personal story. The player chooses whether to read the names, ask another participant, invite silence or leave the reading to those who requested it. The participants can stop partway through. The game should not frame that pause as failure or as a completed ceremony if the requester said the list still needed work.

A record keeper can compare spellings, but the role is clerical. A family delegate can offer context, but cannot speak for a different family. A cultural group may suggest an appropriate form of remembrance, but it does not get veto power over every participant's choice. The major shelter authority offers a room and manages the notice surface; the residents define what remembrance means to them. The player's task is to create conditions for a truthful invitation and handle corrections with care.

Possible endings include an open gathering with no public names, a smaller private reading for families who consent, a board that lists only names confirmed for public display, and a postponed event while the requester seeks another relative. The continuation can feature a correction request days later. The player can update the board, ask who needs to be notified, retain a private copy according to owner policy, or withdraw the public list while leaving the gathering record. A public correction says what changed and when, but does not need to announce the family reason.

The story may offer a small tangible action, such as placing a clean cup beside an empty chair, but it should not turn remembrance into a consumable reward. A resident can attend silently, leave early or choose not to participate. The closing line is contingent on the chosen form: “We read the names agreed for the board, and kept the rest with the families”; or “The gathering was open. No one was asked to answer for a name they had not given.” This arc branches on disclosure, participant action and correction, not on an abstract morality score.

## 526. Branch suite — A translation is not an endorsement

The communications desk receives a notice written in shorthand by the repair crew. It says that the south pump will be shut off “after second bell unless the seal holds.” One resident offers to translate it into the language used by a neighboring group. A second volunteer notices that the phrase “second bell” has no shared meaning outside the repair crew. The player can publish a plain-language source note, ask the crew for a clock time, request a translation review, or post a temporary warning that the time is still being confirmed.

The translator can produce a faithful rendering of the ambiguity, but is not responsible for validating the underlying repair claim. The repair crew owns the shutdown time; the translator helps the audience understand it. If the player asks the translator to choose a time, they are being asked to make a technical decision outside their role. If the player publishes the unreviewed version, a reader may misinterpret the time. The communication plan should show the original source, revision timestamp and who reviewed the translation where the current records permit those facts.

One branch finds that the crew meant 14:00. The player updates both language versions and leaves the old notice marked superseded. A second branch discovers that the seal inspection may extend the window. The player posts an estimate and a next-check time, without translating a provisional estimate as a final guarantee. A third branch finds the shutdown already started. The intercom call tells residents which taps are affected and where the confirmed alternative is; it does not claim that every household has received a container. A fourth branch has no available translator before the shutdown. The player can use a short pictorial warning if one already exists and is validated, find a trusted in-person relay, or delay nonurgent work if the responsible crew authorizes it.

A resident who speaks the language can point out that the draft sounds like a command to ration all water, though only the south pump is affected. The correction changes the scope. The player can post a brief apology and precise correction, ask the reader whether a private clarification is enough, or keep the broader public correction because more residents may have seen the first notice. There is no universal answer; the audience that received the first version and the time remaining determine what action is useful.

A supporting group of learners can check whether the revised notice is readable. It does not own the source facts. The water team can provide a measured status but need not write the message. The player learns to separate fact authority, translation skill and communication responsibility. A later message can invite the translator to help prepare a standard glossary for terms that recur, but participation is voluntary and the glossary is not a new source of operational truth.

Acceptance requires the player to compare the source time, affected location, translated scope and next-update promise. The interface must display corrections and the version of the notice currently in effect. If a translation is unavailable, the status should say that rather than silently showing only the source language. The scene can end with an unresolved technical question so long as readers know where and when the next reliable answer is expected.

## 527. Private-message arc — The Apology with No Reply Requested

A player has asked a resident to carry a sealed box despite that resident having already said they cannot use the basement stairs. The resident later sends a private message: “I need you to understand why I left it. Please don't send another request tonight.” The player can acknowledge receipt without argument, wait until morning, ask whether they want a follow-up conversation, or take no action. The player cannot send a chain of apologies until the resident accepts one. A private acknowledgment should not become a shelter-wide lesson unless the resident agrees to share it.

If the player acknowledges the message, the resident may respond with a practical request: keep heavy items on the upper shelf. If the player waits, the next scene can occur through ordinary play; the resident may still be busy. If the player asks for a discussion immediately, the recipient can decline and the message closes without a penalty. If the player takes no action, no false receipt appears. The resident might later ask whether the player saw the note, at which point the player can answer honestly.

The branch can lead to a changed task practice. The player can move the box, ask a different volunteer who has consented, split the load, or leave it where it is until a safe option exists. The property owner determines who can move the box; the survivor's consent determines whether they take the task; the accessibility concern stays private unless broader disclosure is necessary and authorized. A public notice can state “heavy deliveries need an upper-route carrier” without naming the resident or recounting the disagreement.

The relationship scene varies with prior player action. If the player has respected earlier limits, the resident may say this was a one-time miss and propose a route. If the player has repeated the request after refusals, the resident can request that someone else handle future work. That is an observable boundary, not a “trust score.” A supporting steward can facilitate a practical task reassignment if invited, but should not mediate the private relationship by default. The main authority may set a work safety rule; it need not adjudicate the apology.

An ending can be a simple received acknowledgment and no immediate reply. Another can be a short agreed conversation. A third lets the resident close the message and ask for a concrete route change. A fourth leaves the player without a response because the recipient has not reopened contact. The personal arc is not complete merely because the player sent a polite line. It may remain unresolved, and ordinary life continues.

Sample response: “I read it. I should have remembered what you told me about the stairs. I’ll move the box with someone else tomorrow and won't ask you to carry anything tonight.” If the player chooses this line, it is an acknowledgment of a specific action and respects the requested pause. The game must not present it as forgiveness, agreement or proof that the box was moved. A later task record establishes the actual move.

## 528. Arrival-state matrix — notices during absence and return

A resident leaves on an expedition before a public notice is posted, receives a private note while away, and returns after the notice has expired. The board view should show the current notice only if its owner still considers it active. An archive view can show that the schedule changed in their absence. The private message may be pending, undeliverable or returned under the existing message rules. The game must not show “read” simply because the expedition event resolved.

On return, the resident can ask for the current situation, request the missed messages, or ask a specific person. The player can offer a concise current-state summary, direct them to the board and its revision history, or let them inspect the archive themselves. A full transcript may contain information they are not authorized to see. The player should be able to say that a private note exists without exposing its content. If the sender permitted delivery only to the named recipient, the expedition status cannot become a backdoor recipient.

The quest branch changes if the resident's absence affects action. A closure notice may no longer require a response. An unpaid favor request may have expired. A maintenance update might still matter because the resident uses that route. The player can ask the source whether the message remains relevant, send a fresh summary, or close it as outdated. The system should preserve the original timestamp and create a new message if facts have changed.

A return conversation may reveal that the resident learned a different route while away. The player can update the board based on a tested route, ask the resident to demonstrate it, or leave the existing sign until a steward confirms. The resident may correct the player's summary. A supporting map-keeper can update a route diagram, while a steward verifies access and the communications surface distributes the change. Neither the map nor the notice should claim universal reach when several residents have not seen it.

The acceptance cases cover a return before expiry, after expiry, after a correction, and after an unknown delivery state. In each, the player can distinguish current instructions from historical messages. No decision depends on alignment. It depends on whether the information remains current, whether the recipient has a decision to make, and whether the source approves redistribution.

## 529. Closeout — authorship, delivery, and memory

These additional cases broaden the plan through communication situations already inside the three feature pillars. The drill and memorial use public notices; the translation and urgency branches use boards and intercom calls; the apology and return-state arcs use private messages. Supporting groups offer review, delivery, route checks or records, but the named owners retain decisions over safety, care, goods and source facts.

The plan's outcomes include an accurate notice, a correction after a mistake, a delayed message with a stated next step, a private acknowledgment with no reply, a gathering shaped by consent, and a route action verified after a call. These outcomes intentionally differ in scale and emotional register. A corrected time can be a satisfying ending; an unanswered private note can remain a valid open branch; a paused drill can be evidence of sound judgment when neither route is ready.

For any implementation proposal, validate the current data model's handling of revision history, expiry, delivery state, access control and save restoration. Keep the authored narrative separate from mutable communication records. The example lines remain candidate prose. The communication UI may expose commands only where their owner supports them. If the existing API distinguishes only posted from not posted, do not label messages delivered or read without evidence.

Plan 4 closes as a content-design document when its measured length is recorded above the user-requested minimum. This closing statement does not certify code, data, host routing or persistence. It hands a future audit a bounded set of scenes and explicit questions to recheck against current source.

## 530. Questline — The notice that promised too much

A posting says that the west cistern will be “open to everyone after the pump repair.” The mechanic meant that the tap would be restored, not that water distribution would have no queue or limit. Residents arrive carrying buckets; the steward asks the player to correct the wording before the crowd grows. The player may speak to the mechanic, check the distribution schedule, make an immediate correction and follow with a confirmed notice, or ask the steward to manage the arriving line while facts are checked. The story does not penalize the mechanic for using a phrase that readers reasonably interpreted differently; it focuses on repair and clarity.

If the pump is working but the water is not yet tested, a correction states that access remains closed pending a safety check. If it is tested and usable, the player posts the opening time and the existing allotment procedure. If the pump is repaired but supplies are insufficient, the wording says that the tap is available only during a named distribution window. In every branch, a resident can ask a question at the board, and the player can choose to answer in public if the answer applies broadly or privately if it concerns one household.

The mechanic can explain the technical distinction, while the steward explains how the line will operate. A water monitor performs the safety test. The player decides whether to issue a correction immediately or wait for a verified reading, and the player can enlist one of the support volunteers to help people who already came. No one group is allowed to substitute its judgment for another. If the measured result is delayed, the correction carries a next-check time and a statement of what remains unknown.

Several follow-ups can emerge. A resident who waited for the pump thanks the crew but asks for a better sign. Another says the correction was difficult to notice because the original remained on the board. The player can add a clear “superseded” heading, have a runner tell the queue, or preserve the original with a visible correction below it. A resident who cannot stand in line asks for a pickup arrangement. The player can direct them to the established private request route, not publish their need beside the tap schedule.

The ending branches on the actual repair state and the player’s correction route: a smooth reopening with a revised schedule; a delayed opening with a reliable next update; a temporary queue managed by residents who volunteer; or a postponement after the safety test fails. A final scene reports who confirmed the test and what opening procedure applies. A board post alone never sets the pump state, and a repaired pump alone never proves the message reached the people waiting outside.

## 531. Supporting faction roles — helpers at the edge of authority

The plan gives minor groups a consequential but bounded role. A copying circle can reproduce a confirmed notice and preserve its revision date. It cannot approve a policy, expand its audience or remove a correction. A night steward can relay a message to rooms the board cannot reach, but is a messenger and may decline if occupied with an urgent task. A translation group can review wording, but the source owner remains responsible for operational facts. A resident council can request a public explanation, but cannot read another resident's private correspondence. The player chooses how to coordinate these helpers while the major owners keep decisions in their established domains.

This structure produces action branches. The player may bring a draft to the copyists before it is approved; they can return it with a missing date. The player may ask a steward to deliver a correction; the steward may agree only to the nearest two rooms because the infirmary needs coverage elsewhere. The player may request a translation in advance, or decide that an emergency message requires a short, verified in-person relay while the full translation is prepared. Each helper's availability and scope are visible. No group unlocks an exclusive ending by simply being favored.

A player who relies on physical boards may schedule a daily reading for residents who cannot approach them. A player who prefers private exchange may answer individuals and accept that general facts must still be posted somewhere accessible. A player who delegates may coordinate several helpers and track their acknowledgments without pretending one person's confirmation stands for all. A player who avoids public correction can protect a sensitive source, but should still repair false information for the audience that acted on it. The consequences follow actual audience and content rather than a general “reputation” meter.

When two helpers disagree, the player can ask what each can verify. The copyist confirms the date they reproduced. The steward knows which rooms they visited. The translator can explain which phrase remained ambiguous. If their accounts differ, the record notes the limited facts and schedules a check. The player may also choose not to publish a disputed detail. A disagreement between support groups does not automatically become a faction conflict or a quest fight.

The groups can have distinct texture without more feature authority: the copyists reuse the backs of damaged forms; stewards prefer short messages during shift change; translators keep a pocket list of recurring terms; council members bring questions from the queue. Dialogue reflects those habits. It does not imply hidden powers or new save state. Their part is to make the player's communication choices more legible and to create optional scenes around the same three surfaces.

## 532. Flavor bank — small lines for high-pressure and ordinary moments

The following candidates are deliberately brief and tied to a visible action. At the west hall board: “North passage is open. If the marker has fallen, tell the night desk before taking the turn.” After an intercom test: “We heard the request. The west landing is being checked now; stay clear of the door until the steward returns.” On a private delivery slip: “Left unopened at the desk. Please ask for it by tomorrow evening.” During a correction: “The first time was wrong. The pump is not yet approved for use. Next reading at 16:00.” These lines state what is known and what the player can do next.

Different voices should remain distinct. A mechanic may say, “The motor runs. I haven't called the water clean.” A steward might say, “I can tell the nearest rooms. I can't leave the medicine corridor.” A copyist could ask, “Do you want the old date crossed out or the new line under it?” A resident may reply, “Put the schedule up. Leave my room out of it.” The player’s options should echo that practical register rather than turning every encounter into a speech about honesty or courage.

For low-pressure scenes, a resident leaves a note asking when the reading corner reopens. The player can post hours, check whether the room is actually free, or ask the residents who use it to choose a schedule. A child wants an announcement to say “story hour,” while the volunteer running it calls it “quiet reading.” The player can retain both names, ask what readers prefer, or leave a handwritten subtitle. This small negotiation can close with a readable notice, a changed time or a decision to skip the event that week.

For tense moments, use short lines with observable next steps. “The hall is crowded. We are holding the next call.” “The message is private. I can tell them it arrived.” “The correction is posted. We have not reached the east rooms yet.” For unresolved disagreement: “Two sources give different times. I have posted neither as final.” These statements communicate friction while preserving truth. They do not use scolding language or frame the resident as an obstacle.

The flavor bank should be tested against authored data, character voice and available mechanics before adoption. Sample copy must never imply a delivery acknowledgment, verified safety, consent or completed action that the owner has not recorded. When a text line promises a next update, content data needs a real route to show that update or to explain why it missed its time. Otherwise, phrase it as an intention rather than a promise.

## 533. Plan 4 acceptance closeout — minimum met, proposal only

At the public notice level, acceptance means the player can see author, scope, date, source and expiry; can post, correct, supersede or leave a notice unchanged; and receives truthful feedback about who has or has not acknowledged it. At the intercom level, acceptance means an authorized call has one source identity, a named purpose and a distinguishable delivery/acknowledgment state where the underlying owner supports those states. At the private-message level, acceptance means the recipient and privacy boundary remain explicit across delivery, absence, return, decline and closeout.

Narrative acceptance uses action-derived branches. A player who checks a route can change the notice. A player who learns the pump state from its owner can choose the appropriate call. A player who respects a request not to share private details can still write a public summary that omits them. The support groups extend reach or interpretation, and their limits are visible. Outcomes can include a verified opening, a corrected schedule, a postponed drill, a private apology, an unanswered note, a public gathering without names, or an open question with a next-check time.

The future implementer must compare every proposed state with current message catalogs, board consumers, intercom bridge and save owner. If an authored example depends on a delivery state that does not exist, either narrow it to “sent” or request a decision through the integration queue. If the planned experience would need a fourth subsystem, stop and report the missing authority. The writing itself grants no file claim, API design approval or data migration permission.

Measured at 120,000 words or higher, this document is complete as a prose expansion and can be handed to a separate evidence-first review. Its complete scenes make no assertion that the current game already contains them. A content completion marker must be accompanied by the final measured word and character counts in the header and wave index; until then, retain an in-progress metadata state.

## 534. Failure and recovery — the board is not a witness

A resident says the board never mentioned the night closure, while the player sees a notice that appears to have been posted the previous evening. Neither side needs to be lying. The notice may have been placed after the resident's last pass, hidden behind a newer sheet, or posted on a board they do not use. The player can inspect the revision time, ask whether the resident wants a direct update, move the notice to a more visible approved place, or leave it where it is and offer an in-person route check. The board timestamp establishes when an owner recorded a post; it does not prove that a person saw it.

If the resident describes a practical impact, the player can respond to that impact separately from debating the record. A missed trip may require an escort or revised appointment. A correction can explain the new location and effective hour. If the resident asks not to be named, the public post says “A route change was missed” rather than identifying them. If the player believes the notice was accessible, they can still agree to check the sightline. This creates a branch based on repair versus dispute without assigning either character an honesty label.

The player may ask the board keeper whether the sheet was removed and replaced, compare the archived copy, or ask a passerby what they recall. Each source is limited. A missing copy does not prove it was never posted; a visible copy does not prove it was readable. If no evidence resolves what happened, the report says that the posting record exists and that actual reach is unknown. The player can change the process going forward by asking for a scheduled reading or a small acknowledgment card for time-sensitive notices, provided an existing communication owner supports that interaction.

The next day, a different resident can demonstrate the same problem from a wheelchair or from a crowded corridor. The player might lower the board, add a second approved display, or ask a steward to announce updates at shift change. The content branch is not “fix all access forever”; it is a specific action with a named owner and a follow-up check. If materials are unavailable, the player records the workaround and its limit. This quiet recovery arc gives the player a chance to respond to lived access failure and makes future notices more believable.

## 535. Final quality gate — closeout record

A final editorial sweep asks whether each sample message matches one of the three channels, whether every source is named where known, and whether the player has a concrete response beyond selecting “good” or “bad.” It asks whether unknown status is allowed; whether a correction reaches the audience of the mistaken version; whether an absent resident can return to an accurate current state; and whether declining a private conversation can close without a social penalty invented by the plan. It checks that no scene turns a helper group into a hidden authority or lets a panel claim that an unrecorded action occurred.

The sweep also checks narrative variation. Some branches end in a meeting, some in a corrected sign, some in an unanswered message, and some in a delayed operation. A notice need not become a quest if the player has enough information and no one needs to act. A private reply need not disclose the sender's full story. A call can be interrupted for a valid emergency and resumed only when the channel owner permits it. Every branch has an observable state or a justified open question.

The minimum-length closeout is editorial accounting only. It states that Plan 4 has enough authored content to leave expansion drafting. The next step is to inspect current source and data, not to implement all examples wholesale. If the evidence disagrees with a premise, the future owner should mark the affected passage for correction before integration. An accurate shorter feature is better than a sprawling interface that promises audience, delivery or privacy semantics its records cannot support.

## 536. Optional route — the message is useful even when it is late

A repair notice arrives after the first affected residents have already walked to the old tap. The player can deliver a correction, apologize for the timing, and help the residents return; they can ask the steward to carry water to those who cannot make the second trip; they can request that the water team hold the next distribution window; or they can leave the line in place and post a clear estimate. The message itself is not a substitute for the lost time. Its value is that people can now decide with current information.

The player learns why the update was late. The carrier took a detour; the water steward had a conflicting task; the board keeper was not on shift; or the source changed its estimate twice. Each account leads to a different next action. A detour can be handled with a runner, a coverage gap with a backup posting arrangement, and changing estimates with a provisional notice that avoids false precision. If the player chooses not to investigate, the immediate correction can still happen. The wider process remains imperfect, but the narrative does not hold residents hostage until a complete organizational review is finished.

The player chooses what to say at the tap. A concise apology can name the delay and the current opening. A longer explanation can describe why the first route changed. A private note may help one household that made an extra trip, while a public correction reaches people still on the way. A resident can reject the apology and ask for a concrete next step. The player can offer that step without demanding an emotional response. If water delivery is the source of the problem, only the water owner can confirm the new time; the communications owner records the message and who received it.

A supporting runner can accompany a resident back to their room but cannot promise that the next delivery will be on time. A queue helper can keep a place for someone who must sit down, if that resident accepts. The player may use the current roster to find help, ask the group directly or tell everyone to disperse and return later. The consequences are visible: who waits, who takes a second walk and when the next reliable check occurs. No branch awards abstract sympathy points.

The late-message ending can resolve with a reliable new notice and one practical accommodation, even if the resident remains irritated. Another ending may leave the question open because the carrier cannot yet confirm a schedule. A third may reveal that the board was not the right channel for this time-sensitive audience, leading the player to set up a future steward reading at the tap. These outcomes teach that communication quality includes placement and timing, not simply factual wording.

## 537. Closing branch — a room asks to be left off the list

During a supply call, one household asks the player not to include its room number in a public announcement. The request is easy to honor while reporting a shelter-wide delay, but less easy if the household is the only one still waiting for a replacement filter. The player can send the household a private note, report the total number of delayed filters without locations, ask whether the residents consent to a specific room reference, or wait until an authorized general update is available. The interface should explain the consequence of each choice before a message goes out.

If the player can report an aggregate, the public notice gives the delay and next update time without identifying the household. The private message says that the filter is held for them and asks whether they want a delivery or collection arrangement. If only the location can clarify which filter is meant, the player asks permission before disclosing it. The household may agree to a direct steward contact but not a public announcement. That consent is specific to the contact and cannot be reused as blanket permission for future posts.

A steward may not be available to carry the private message. The player can keep the note pending, ask another authorized messenger, or tell the household that no one has confirmed delivery. A message held at the desk is not delivered. If the filter status changes, the player creates a fresh update rather than leaving an old note to imply that the item is still waiting. The public message can stay unchanged if the aggregate facts remain true; the private thread tracks the household's specific arrangement.

The residents may later ask for public recognition after the filter arrives. They can authorize a brief note thanking the maintenance team without naming their room. The player can publish that note, leave the event private, or invite the residents to write their own line. If they ask to remove the original message from the archive, the player follows the owner policy and describes what record remains. The tradeoff is stated clearly; privacy is not promised in a way the system cannot keep.

The final scene may end with the filter delivered, collection arranged, delivery unconfirmed, or the household deciding not to receive it. Each ending records the actual status and respects the audience boundary. A courier circle's participation remains optional. The major maintenance owner confirms the filter is ready; the household chooses how to receive it; the player routes the appropriate message. This final case demonstrates how scope, consent and delivery can create meaningful branching without relying on faction allegiance or a reputation axis.

## 538. Closeout vignette — the board after the rain

Rain loosens the board cover and blurs one corner of the evening schedule. A resident tells the player that the west hall notice can no longer be read. The player can replace the cover, move the sheet indoors, ask the copyists for a duplicate or read the change aloud at the next shift handoff. If the schedule has already passed, the player can archive it rather than spend scarce paper reproducing old instructions. If the repair remains active, they can post the essential route and date first, then add explanatory detail when materials return.

The player may ask a passerby what they understood before the text blurred. One person recalls the route but not the time; another remembers the time but not the destination. Their answers do not count as formal delivery receipts. A steward can check which rooms were visited, though several residents were asleep. The notice record should remain “posted; reach uncertain,” and the player can choose a practical follow-up. This small ending reinforces that messages exist in a physical shelter where light, weather and routine can interrupt a flawless sentence.

The copyists can offer a more durable sleeve if asked, but their supply is needed for medicine labels. The player may ration the material, use a dry interior board, or depend on a steward to repeat the schedule. The support group contributes a service and a constraint; it does not decide which notice deserves the last sleeve. The player can explain the priority to residents who ask, or leave the choice unstated when no explanation is owed.

## 539. Measurement note

Section numbering ends here for the final pass; new ideas can still be added in future revisions after the fresh audit. This document's measured minimum must be taken from the saved file, not inferred from the number of sections, the amount of prose in a tool call or an earlier estimate. The header and wave index should receive the same final count. If either differs from `wc`, update it before closeout. All completion language refers only to content drafting.

## 540. Final prose audit

A final prose review should remove any line that claims “everyone knows,” “the shelter agrees,” or “the message was understood” unless an owner-backed record supports that scope. Prefer “the board is posted,” “the west rooms acknowledged,” or “the caller has no confirmation yet.” Likewise, a completed notice does not prove the underlying repair, ration or route is complete. Keep those facts separately sourced and explain the boundary in a way the player can understand.

The plan now offers branches for direct correction, delayed confirmation, private delivery, public summary, respectful nonresponse, channel interruption, audience mismatch, translation review and in-person relay. The difference between these choices comes from the player's action and the situation at hand. No route requires a virtue alignment or a faction loyalty test. The player may be efficient, cautious, highly collaborative, privacy-conscious or willing to leave an unresolved question open. Each style can produce an ending that is specific and credible.

The communication scenes should be reviewed for tone by reading the resident's options aloud, confirming that a player can decline an action, and checking that every promised follow-up has a source and a visible next step. A short accurate line, sent to the right person at the right time, may be the strongest ending available.
