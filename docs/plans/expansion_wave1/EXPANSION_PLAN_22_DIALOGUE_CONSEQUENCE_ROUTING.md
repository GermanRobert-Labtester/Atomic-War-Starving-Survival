# Expansion-series Plan 22 — Dialogue Consequence Routing

**Status:** Integration design proposal; documentation only.
**Numbering note:** This is Expansion Planning Wave 1, Plan 22.
**Purpose:** Define the safe path from player response to local scene change, quest/relationship/faction/world consequences, and ending callbacks, routing every mutation through its existing owner.

## 1. Consequence model

A dialogue response is authored content plus a command request. The dialogue resolver validates the selected response and returns a typed result. The host sends each allowed request to the system that owns that fact. The owner validates and applies the change, then publishes the resulting fact or event. Quest and dialogue read models refresh from those authorities.

The dialogue layer can own node traversal and presentation selection if the current architecture assigns it those duties. It cannot own inventory quantities, map discovery, health, relationship values, faction standing, campaign completion, or quest lifecycle.

## 2. Scope of effects

| Scope | Meaning | Valid owner route |
|---|---|---|
| Cosmetic | Changes wording, pose, or immediate tone only. | Dialogue presentation; no durable gameplay mutation. |
| Local | Changes the current scene or encounter outcome. | Existing encounter/location owner, if one exists. |
| Quest | Starts, advances, blocks, fails, completes, resolves, abandons, or reopens a quest through a supported command. | Current quest runtime or quest-specific owner. |
| Relationship | Changes a supported relationship fact. | Current relationship owner. |
| Faction | Changes standing, access, or a faction quest fact. | Current faction/reputation/access owner. |
| World | Changes a canonical location, resource, hazard, shelter, or event fact. | The domain owner of that state. |
| Ending | Adds a supported campaign observation used by existing ending resolution. | Existing campaign/ending input owner. The dialogue UI never selects an ending directly. |

Every response should declare its intended scope and one or more validated effect IDs. Cosmetic is the default. Higher scopes require a traceable owner and a real player-facing consequence. Avoid stacking several unrelated effects onto a single short line.

## 3. Typed response result

An implementation may need a result concept with these fields, adapted to existing types rather than copied as a new duplicate system:

~~~text
DialogueResolution:
    conversation_id
    node_id
    response_id
    source_event_id
    validated_effect_requests[]
    quest_commands[]
    presentation_result
    resolution_status
~~~

Each effect request identifies its target owner, command kind, canonical target ID, bounded parameters, and stable source event/request ID. Data must not include executable callbacks, arbitrary type names, reflection paths, or unbounded key/value mutation. The receiving owner rejects unknown command kinds, invalid targets, and out-of-range values.

Repeated submission of the same response must not grant an item or standing twice. Prefer the current owner's idempotence mechanism. If none exists, determine whether the current encounter or quest owner already marks the choice resolved. A new global effect ledger is not the default solution. If multiple owner effects can partially apply and the current event contract provides no recovery, reduce the slice to one durable effect or stop for an architecture decision; do not promise transactionality the code does not have.

## 4. Quest control and outcome boundaries

Dialogue may begin, advance, or complete a quest only by issuing a validated command to the quest owner. The quest owner verifies allowed transition, objective status, deadline, and instance identity. Dialogue text may report a blocked reason or a new route; it cannot set arbitrary lifecycle values.

Quest failure can lead to a different authored scene or a new recovery objective. The original failure remains a failure unless the quest owner has an explicit reopened/sequel operation. A choice that resolves an investigation may record the selected conclusion and route knowledge/reputation effects to their respective owners. A late-game choice may contribute an ending observation but does not itself determine the ending.

## 5. Ordering and user feedback

Recommended sequence:

1. Confirm conversation and response are still available in the current snapshot.
2. Resolve the response once using a stable event/request identifier.
3. Ask the canonical quest owner to validate any quest transition.
4. Submit each other effect request to its named owner.
5. Collect accepted, rejected, and deferred results.
6. Display immediate feedback accurately and refresh dialogue, quest, journal, and map read models from the owners.

This sequence is a design candidate, not a claim about current host event order. Premise review must check current seams and lifecycle. If a required effect is rejected, the UI must not display the promised successful outcome. If a cosmetic choice succeeds but an optional side effect is unavailable, present the actual result and record the limitation.

Use feedback proportionate to the consequence: line tone changes may need no banner; quest acceptance shows the new quest; location unlock changes the map; a reputation change uses the current faction feedback surface; ending observations may be reflected at campaign closure. Preserve keyboard/controller focus and allow safe conversation exit.

## 6. Cost, reuse, and core/expansion placement

Cosmetic variants are low cost and easy to reuse. Local encounter choices are medium cost due to branch and fallback testing. Quest/relationship/faction effects have medium or high cost because they cross state owners and persistence. World effects have high cost when they touch map availability or resources. Ending effects have the highest continuity cost and belong only after the ending owner and its input facts are proven.

The core game should demonstrate one quest-start choice and one visible local or relationship consequence through existing owners. A small campaign choice may affect an existing faction or quest fact. Broad faction diplomacy, multi-owner negotiation, and late-game ending variations fit expansions after a stable owner map and regression path exist.

## 7. Failure handling and rollback

- Invalid response ID: keep conversation open or return to a safe node with visible feedback.
- Quest instance missing: use an authored fallback line; do not create a replacement instance implicitly.
- Owner rejects a request: show the actual accepted effects and a clear reason if the player can act.
- Location or actor missing: resolve to a supported clue, delegate, delay, or neutral exit.
- Old save lacks a choice fact: select the explicit default/unknown branch.
- Duplicate command: owner returns prior outcome or rejects the duplicate without reapplying the reward.
- Partial multi-owner result: report each applied result accurately; recovery is owner-specific and must be documented before adding such a branch.

Rollback removes or disables the new response reference, not a saved fact already used by current saves. If an effect schema or save field has shipped, preserve its decoder and add an explicit migration plan before retirement.

## 8. Integration course

**Phase 0 — Trace one response:** Select one current conversation trigger and trace input, Core resolution, host effect application, saves, quest and UI refresh. Check active claims and the live batch.

**Phase 1 — Owner table:** For every requested effect, list the target owner, command/event API, validation, persistence, deterministic behavior, read-model refresh, and failure response. Drop effects without a live route.

**Phase 2 — Minimal authored branch:** Use one response with one durable consequence and one reconvergent continuation. Store effect identifiers in canonical authored data and run schema/reference validation.

**Phase 3 — Host wiring:** Bind the response to an existing host seam. Keep UI free of state decisions. Verify exactly-once behavior and truthful feedback.

**Phase 4 — Persistence and regression:** If state changes durably, add owner-specific capture/restore and migration coverage. Run focused tests for accepted/rejected/duplicate response, quest state, relationship or faction effect, and branch refresh; add targeted Godot verification only for a changed host route.

**Phase 5 — Expand scope:** Add further effect classes only after the first branch is accepted. Run narrative continuity review, accessibility checks, and focused performance checks for large catalogs.

**Phase 6 — Handoff:** Submit evidence, exact files, effect ownership, focused commands and results, save compatibility, partial-failure limits, and unclaimed shared paths. The integrator updates the ledgers.

## 9. Acceptance

Every response effect has one canonical owner; the dialogue layer stores no duplicate mutable domain fact; quest changes obey quest lifecycle; effects are idempotent or guarded by an existing resolved state; displayed outcomes reflect owner results; legacy/missing context has a safe route; ending selection remains with the existing ending authority; and focused verification proves the chosen slice. This plan is not an implementation authorization or test report.

## Continuation pass 2 — effect contract and owner receipts

### Effect routing table

| Effect request | Validation before dispatch | Canonical result owner | Feedback after owner result |
|---|---|---|---|
| Cosmetic text/tone | Response and target node are valid. | Dialogue presentation. | Continue scene or show selected variant. |
| Local encounter result | Encounter is active and choice remains unresolved. | Existing encounter/location authority. | Show its outcome and any changed encounter state. |
| Quest start/advance/complete | Instance or definition exists and transition is allowed from current state. | Current quest runtime or quest-specific owner. | Refresh quest log and show accepted/progress/resolved status. |
| Relationship change | Actor exists and requested change is within owner limits. | Current relationship authority. | Refresh relationship-facing read model only if visible in UI. |
| Faction standing/access | Faction and amount/permission are valid. | Faction standing/access authority. | Show changed access or current reputation feedback. |
| World/location/resource | Target domain object exists and can accept the command. | Map, shelter, inventory, hazard, or other owning system. | Refresh the affected panel or map marker. |
| Ending observation | Campaign is in a phase where this observation is accepted. | Existing campaign completion/ending input owner. | Reflect the fact in the later ending presentation. |

The router may sequence commands, but it does not reinterpret their meaning. It should not know how a faction value is stored, how a map node is discovered, or how an ending is selected. Each owner returns accepted, rejected, or already-applied with a reason safe for the host.

### Exactly-once response behavior

Use a stable source event ID built from the current conversation/quest instance and response identity where the current architecture permits it. If an existing encounter choice already tracks resolved response IDs, reuse it. On retry after a save or signal duplication:

1. Validate the same response against current authoritative state.
2. Ask the owning system whether the source event has already been applied.
3. Return the prior result or reject as already resolved.
4. Do not issue duplicate item, standing, quest, or ending requests.

If no owner has an idempotence key, first reduce the MVP to an effect guarded by the owner's existing resolved state. If no such guard exists, stop and propose the smallest new owner-specific contract. A global dialogue receipt ledger would duplicate state and is not an automatic fallback.

### Partial result example

Suppose a conversation both accepts a delivery quest and offers a faction-standing reward. The quest owner may accept while the faction owner rejects because the faction binding no longer exists. The host must show the quest acceptance and omit the standing claim; it cannot display the authored “they trust you now” line. If the faction reward is essential to the quest's narrative promise, the content should instead gate acceptance on the live faction binding or use a single owner-supported transaction/command. Do not pretend two independent systems committed atomically.

Likewise, if a choice records a local line but the quest command is invalid, keep the conversation truthful: “I can ask again once the ledger is found.” Do not create a quest instance in the panel or preserve an untracked success flag.

### Effect scope policy

Default new dialogue content to cosmetic scope. Add local consequences when an existing encounter already owns them. Quest effects may be core content when they expose a validated quest command. Relationship and faction changes require explicit owner limits and player-readable feedback. World effects require a demonstrable gameplay or map consumer. Ending observations require the highest continuity review and must remain optional unless the campaign already guarantees their producer.

An authored response should not stack an ending flag, faction reward, relationship change, item grant, map unlock, and quest completion merely to feel important. Choose one primary consequence and, at most, a small set of dependent effects supported by a single coherent owner flow. Higher volume expands authored response variety, not the number of coupled systems per choice.

### Rollback and content withdrawal

When a dialogue row is found invalid before release, remove its route from the catalog and retain a valid neighboring response. After release, hide or redirect the response without deleting durable owner facts. If a response has already created a quest instance, preserve its definition decoder until that instance resolves or a migration maps it to a safe state. If a later ending callback reads the consequence, keep the old fact available to the ending owner even when the original dialogue scene is retired.

## Continuation pass 2 acceptance

The effect contract is ready for a future integration package when every scope maps to a current owner, duplicate response handling is defined, rejected and partial results produce truthful copy, rollback preserves already-saved facts, and no dialogue-specific save ledger is required. Each effect class needs owner-specific focused coverage before it ships.

## Continuation pass 3 — effect requests, result states, and cross-system examples

### Request envelope

An effect request should carry only enough data for its owner to decide:

~~~text
EffectRequest
    source_conversation_id
    source_node_id
    source_response_id
    source_instance_id
    target_owner_kind
    command_kind
    target_id
    bounded_parameters
    source_event_id
~~~

The actual project types may already offer a result or command envelope. Reuse them. The source IDs support diagnostics and duplicate protection; the target and command IDs are validated against the receiving owner; bounded parameters prevent authored content from smuggling arbitrary state changes. Never accept raw C# type names, method names, serialized closures, or a dictionary of unrestricted world keys from content.

The request's source event ID must be stable for retries and unique for a meaningful response invocation. If existing quest or encounter instances already provide a stable transaction/event key, use that. If not, keep the effect behind the current owner's choice-resolved state or defer adding durable side effects until an owner-approved idempotence contract exists.

### Result states and user-visible behavior

An owner response can be modeled as Applied, AlreadyApplied, Rejected, or Deferred, provided the existing command seam can communicate those distinctions:

- **Applied:** show the authored confirmation and refresh current read models.
- **AlreadyApplied:** do not replay the reward; show the current state or continue if the response is already resolved.
- **Rejected:** show no promised effect; preserve the conversation or offer a safe exit.
- **Deferred:** show what is waiting and how the player can continue. Defer only when the receiving owner supports a real pending state.

A generic “success” toast is incorrect when a multi-owner response partially fails. The host should collect owner results, refresh authoritative read models, and choose copy from what actually changed. If the current UI cannot explain partial outcomes, constrain the authored response to a single durable effect in the MVP.

### Cross-system example: accepting a field investigation

The route clerk offers an investigation. On the confirm response:

1. The conversation verifies the speaker and the currently available quest definition.
2. The quest owner creates or accepts the instance and returns its actual status.
3. The quest owner exposes a required location binding.
4. The expedition selector reads that binding at the next dispatch and pins the exact site or clue marker at every expedition start while the quest remains active.
5. The map owner controls whether the player knows the site and whether the route is traversable.
6. A discovery event advances the objective through the quest's event producer.
7. The journal shows the new fact from the current journal/quest read model.

If the exact site cannot be selected, Plan 18's equivalent, delay, clue, or fail-forward path applies. Dialogue cannot set a map marker directly. If the quest command rejects, the conversation does not claim acceptance.

### Cross-system example: local promise with a later callback

A survivor asks the player to keep a tool for a repair crew until morning. The immediate response can be cosmetic, or it can start a quest. To support a later callback, the quest owner records the accepted commitment or a canonical memory owner receives the supported event. At the later scene, Plan 21 reads that fact and selects an authored line. If the saved fact is absent, the line asks neutrally whether the tool was kept; it does not assert a promise that the game cannot prove.

The tool itself remains in inventory ownership. A delivery or transfer uses the inventory command and reports whether it was applied. The dialogue graph never holds a shadow copy of the item.

### Cross-system example: ending observation

A final conversation may ask whether a public archive should remain open to every shelter or be placed under a caretaker group. The response can contribute an authored campaign fact only through the existing completion/ending input owner. Earlier faction and quest states may change which choices are available, but the dialogue panel must not derive or select the campaign ending itself. Old saves with no archive choice use an explicit neutral ending variant.

This is high continuity cost and should be postponed until the ending owner, save, and snapshot contracts are audited. A well-written ending choice cannot compensate for an untracked campaign fact.

### Idempotency and consequence sizing

One response should have one primary durable consequence. Secondary effects are acceptable when they are necessary to that result and can be applied through a coherent existing owner route. Example: accepting a quest and pinning its destination are one logical effect only if the quest owner exposes the location binding and map/expedition owners derive the marker. Do not separately save a dialogue marker, a quest destination cache, and an expedition requirement.

Repeatedly opening a conversation is safe. Repeating a response after it applied must not duplicate rewards. Loading a save before or after owner application must preserve the actual owner state. Replaying the same seeded campaign should produce the same eligible response set and generated binding, while player choice remains an explicit input.

### Effect authoring cost and review

Every non-cosmetic response adds production tasks: validate the command, define refusal and stale-state copy, confirm owner feedback, test save/load if durable, test duplicate invocation, verify refresh of affected panels, and review interaction accessibility. Cross-system effects add a seam review for each owner. Ending effects add narrative continuity review across old and new saves.

The writer can continue drafting a scene while a seam is under audit, but any response without a confirmed command is marked as prose-only and cannot be presented as operational. During implementation, promote one effect scope at a time and update the content validator to reject unrecognized commands.

## Continuation pass 3 acceptance

The request/result contract is ready when command types are allowlisted, outcomes are truthful, duplicate invocation is safe, one primary consequence owns each response, cross-system markers derive from the quest and map owners, and ending observations route through the current ending input authority.

## Continuation pass 4 — effect execution policy, retries, and fail-forward outcomes

### Separate dialogue choice from consequence execution

A selected response can produce a presentation result, request one or more approved owner commands, and then render a result line after the owner reports what actually happened. The graph should not optimistically set quest, faction, relationship, inventory, location, or ending facts before the owner accepts the request. Treat a response as pending while its command is in flight if the current host is asynchronous; if it is synchronous, still retain the same accepted/rejected/deferred contract.

Proposed command envelope fields: stable command ID; dialogue scene/node/response IDs; campaign or session correlation ID supplied by the current owner; command kind; canonical target ID; validated arguments; preconditions; and idempotency key. Do not send rendered prose, private context dumps, or arbitrary serialized UI state as command authority. A command handler validates its own current state and returns a typed result with the owner fact or reason ID needed by presentation.

### Order effects by dependency, not by copy order

For a response that starts a quest and requests a location, the quest owner first confirms the quest transition. The location-selection owner then receives the canonical requirement through its existing seam at the next eligible dispatch. The dialogue response can say the quest is accepted only after the quest owner confirms; the map can show a marker only after expedition selection and visibility rules succeed. If map availability is deferred, the quest remains accepted with a journal message explaining the next opportunity.

If multiple commands are required, declare the dependency order and define compensation or safe partial completion. Do not pretend that unrelated owners share a transaction. For example, a relationship acknowledgement can succeed while a later optional map hint is deferred. The response result should identify each outcome, and a retry should submit only the still-pending command with the same idempotency key. A failure in cosmetic refresh must never roll back a completed quest.

### Explicit retry, stale-state, and recovery behavior

Use stable result states: Applied; AlreadyApplied; Rejected with a reason; Deferred with a retry condition; and Failed when the owner reports an unexpected operational error. The dialogue layer may safely re-render after Applied or AlreadyApplied. Rejected and Deferred require authored copy or a clear generic fallback that does not misrepresent the outcome. Unexpected failures are logged by command ID and owner reason; player-facing copy should preserve the conversation and offer a safe exit.

An idempotency key identifies the response commitment within the canonical campaign/session scope. Re-opening a scene creates no new key for an already-applied one-shot choice. Repeatable choices need an explicit owner-approved repeat policy, cooldown/eligibility source, and bounded reward rule. Do not rely on UI button disabling as duplicate protection.

If save/load happens while a request is deferred, the current owner decides whether the request is durable. Persist only through that owner's existing save contract. On restore, query the owner for the actual result before presenting a success line. A stale response whose preconditions no longer hold returns Rejected or Deferred; it must not partially invent an outcome.

### Fail-forward case: the missed signal

In a provisional quest, a player agrees to warn a remote crew before a storm. The primary route dispatches a signal from a relay. If the relay location is unavailable, the dialogue request may establish the accepted task but cannot claim the warning was sent. The expedition plan can offer a permitted substitute or delay the route. If the player misses the deadline, the quest owner records the failed warning and opens a recovery task: locate the crew's last known shelter and deliver a face-to-face message. The failed primary objective remains visible in the history; the recovery route has its own actionable objective, location requirement, and reward rule.

The consequence chain is: accept request; confirm quest active; select an available expedition route; report signal sent only after its owner confirms; otherwise keep the warning pending; on expiry, record failure cause; offer the recovery route; on successful delivery, resolve the relationship or faction response through its owner. This path is a concrete example of “failed but continues through a different route” without rewriting failure into success or letting dialogue control the clock.

### Consequence-scope review

Before content approval, label each effect cosmetic, local, quest, relationship, faction, world, or ending. For every non-cosmetic effect, record the command owner, preconditions, result states, persistence rule, duplicate behavior, UI refresh, accessibility feedback, save/load case, and rollback or compensation. Ending consequences additionally need cross-quest continuity review and a decision about what old saves observe.

Keep the first implementation slice to local scene feedback and one quest-owner command. Add relationship/faction/world/ending commands only after each seam has a current API and an agreed ordering. A response with unresolved command ownership remains prose-only and cannot imply an applied consequence.

## Continuation pass 4 acceptance

Effect execution is ready when owner commands validate their own preconditions, retries are idempotent, partial outcomes are shown truthfully, save/restore asks the owner for the actual result, and a failed objective can open a distinct recovery route without being rewritten as success.

## Continuation pass 5 — command inventory, response lifecycle, and outcome evidence

### Response commitment lifecycle

Represent a consequential response as a small request lifecycle, not a direct mutation from a dialogue callback:

1. **Presented:** Plan 20 supplies an authored response and Plan 21 evaluates whether it can be selected.
2. **Selected:** the UI records the response ID for this scene interaction and preserves a clear back/exit route.
3. **Revalidated:** each required fact is read again through its owning authority.
4. **Submitted:** the response creates an allowlisted command with stable target IDs and an idempotency key.
5. **Accepted or declined:** the owner returns Applied, AlreadyApplied, Rejected, Deferred, or Failed with a typed reason/result reference.
6. **Rendered:** the scene chooses result copy from the returned outcome, refreshes relevant facts, and routes to the authored continuation.
7. **Closed or revisited:** the owner remains responsible for durable state; the dialogue graph is recomputed from that state.

Purely cosmetic choices may skip command submission, but should still have a stable response ID for transcript and replay review. If a runtime operation can be retried after scene refresh, crash, or save/load, the idempotency key must identify the same logical commitment. If the current command owner cannot provide duplicate-safe behavior, keep the choice non-consequential or hold the feature for an explicit architecture decision.

### Effect ownership inventory

| Requested consequence | Responsible authority to confirm | Dialogue responsibility |
|---|---|---|
| Start, advance, fail, expire, resolve, or abandon quest | Existing quest runtime/questline owner selected for that quest. | Request transition; display accepted state and owner-supplied reason. |
| Reveal a location or clue | Current map/discovery owner, with expedition selection contract where dispatch is involved. | Offer clue copy only after confirmation; do not force a map pin. |
| Select a destination for an expedition | Current expedition/location-selection path. | Communicate eligibility or delay; never substitute a local target silently. |
| Change relationship or remember an action | Existing relationship or NPC memory authority. | Supply the authored event/reference; display only the accepted outcome. |
| Change faction access/reputation | Current faction owner. | Request a typed change; do not calculate rank or access in the graph. |
| Transfer item/resource | Existing inventory/resource owner. | Confirm what is requested and render transfer only after accepted result. |
| Advance time or deadline | Current clock and quest deadline owner. | Do not advance time from ordinary dialogue unless the game already defines a time-cost command. |
| Change location state/world event | Current map/location/world-state owner. | Request one named operation with preconditions and fallback copy. |
| Resolve major ending | Current ending/campaign resolution authority. | Request a terminal choice after continuity and prerequisite validation. |

The inventory is intentionally owner-focused. Content fields can describe intent, but no general “dialogue effect manager” should become a second authority. If one effect has two current owners, define command ordering and partial-result behavior before implementation.

### Partial outcomes and truthful copy

One response may request multiple independent consequences only when the player understands that they are separate. Order commands by dependency and record each result separately. A quest can become active while a map clue is deferred; a relationship can acknowledge the conversation while an optional resource transfer is rejected. Do not roll back an accepted quest merely because an optional marker could not be shown.

The result copy must distinguish:

- what the player asked for;
- what the owner accepted;
- what remains pending or unavailable;
- the next valid route.

Avoid single success copy for mixed results. Prefer compact factual copy such as “The request is on the board. The route is still unconfirmed.” If the operation failed unexpectedly, preserve the conversation and give an exit or retry path; do not display a technical exception to the player or silently claim success.

### Unentered Shelf outcome map

The fixture's decision is whether to assign a counted blanket stock to the current shelter room while keeping an unverified destination unresolved. A possible command chain is:

1. Dialogue presents the count and the player's proposed allocation.
2. Quest owner confirms the current task phase and permits the local-hold choice.
3. Resource/inventory owner validates stock and performs the transfer or reservation according to its existing model.
4. Quest owner records the confirmed objective/outcome only after the resource result it depends on is available.
5. Map/expedition owners continue to represent the uncertain destination separately.
6. Relationship or NPC-memory owner records the clerk's supported acknowledgement only if the current system supports this event.

Define result branches explicitly:

| Owner result | Quest/history treatment | Scene response |
|---|---|---|
| All required commands Applied | Record the local-hold resolution with the destination question still open if that is the authored outcome. | Clerk acknowledges the allocation and names the remaining uncertainty. |
| AlreadyApplied | Do not repeat transfer or reward; query the owner's actual state. | Show the same confirmed result line without duplicate credit. |
| Resource rejected | Keep the objective unresolved or route to a truthful alternative, according to the quest owner. | Explain that the counted stock could not be moved; preserve retry/exit. |
| Quest phase rejected | Submit no dependent transfer. Refresh active choices. | Explain that the record changed and offer the current valid route. |
| Destination clue deferred | Preserve accepted local choice if independent; keep map clue pending. | State that local stock is accounted for while the route remains unknown. |
| Optional memory unavailable | Do not block the practical result. | Use a neutral acknowledgement rather than claiming the clerk remembers. |
| Unexpected failure | Do not infer any effect; let owners answer on retry/restore. | Keep scene safe and offer a non-committing exit. |

The sequence may change after the concrete APIs are audited. In particular, do not assume inventory can reserve stock or that quest completion can consume a resource event; confirm the available command contracts first. If the current owner cannot express the intended distinction, revise the content rather than creating parallel mutable counters.

### Fail-forward outcome ledger

Every quest-affecting response should have a small authored outcome ledger:

| Field | Question answered |
|---|---|
| Primary objective | What was the player trying to accomplish? |
| Failure evidence | Which owner reports the failure and its cause? |
| Preserved facts | What remains true after failure? |
| Recovery offer | Which alternative action becomes available, and through which owner? |
| Recovery requirements | Which location, character, item, or clue is required? |
| Success evidence | Which owner confirms recovery completion? |
| Reward/closure | What is granted or resolved, with what duplicate policy? |
| Copy variants | What is said for success, failure, delay, rejection, and unknown? |

For the missed-signal example, the failed warning stays failed in history; the recovery task is a separate route to the crew's last known shelter. If that shelter cannot appear, Plan 18 chooses a permitted equivalent, delay, or clue. The conversation reports whether the task was accepted, never whether the warning reached its intended audience until the signal/message owner confirms it. The later relationship/faction response depends on the recovered contact and its canonical owner.

This ledger also applies to escort, protection, timed, resource, investigation, and faction quests. Failure can change available content without erasing the original event. A recovery route needs a concrete objective and proof condition; “try again later” is not enough unless a real retry window and owner-backed eligibility exist.

### Replay, telemetry, and diagnostics

Log stable scene, node, response, command, owner, and result IDs plus compact reason codes needed to diagnose routing. Keep logs deterministic and privacy-conscious: do not record rendered dialogue text, hidden emotional state, full inventory, or private memory payloads. Replay tooling should be able to show the ordered command/result sequence from owner facts and stable IDs without depending on localized prose.

Content review should track response reachability, command acceptance/rejection/defer rates in controlled tests, duplicate prevention, and fallback frequency. Runtime telemetry, if introduced, requires the project's current privacy and telemetry authority; this plan does not authorize a new telemetry pipeline. For local diagnostics, aggregate counts by stable IDs and test fixture only.

On restore, query the command owner for a pending or previously applied result. If the owner has no durable pending command concept, do not invent one in dialogue save data; re-evaluate the scene and show the actual current state. For migration, preserve stable response IDs where the logical decision remains equivalent. If meaning changes, use a new ID and an explicit old-save resolution instead of interpreting an old selection as a different action.

### Integration order and bounded slice

The first implementation slice should contain one scene, one quest transition, one map/clue handoff, and no more than one additional state-changing owner. It should prove the full sequence from presented response through current owner result to save/restore observation before adding faction, relationship, resource, or ending effects.

Suggested promotion order:

1. Audit current command APIs and save owners for the selected quest/location.
2. Claim only the exact implementation paths under the live ownership process.
3. Add or adapt the smallest typed request/result seam; reject unsupported command kinds.
4. Wire one dialogue response through the existing host/event path.
5. Add focused tests for Applied, AlreadyApplied, Rejected, Deferred, duplicate retry, stale fact, and restore.
6. Validate the authored result copy, focus behavior, transcript, and fallback scene.
7. Record the integration evidence and remaining unsupported effect families.

The proposal does not authorize creating shared runtime architecture on its own. If source inspection reveals that the only available seam would require a new cross-system transaction, save section, or registry, stop and submit the narrow design decision for the current foreman.

## Continuation pass 5 acceptance

The routing package is ready for integration when each consequential response has a named owner, typed request/result behavior, duplicate and stale-state policy, and truthful copy for every result; the Unentered Shelf chain does not double-transfer or conflate destination discovery with allocation; and the first vertical slice can be verified end to end under the existing save and ownership contracts.

## Continuation pass 6 — command contract, dependency trace, and recovery closure

### Command definition sheet

Before a dialogue response requests a durable change, complete one command definition sheet:

| Field | Required meaning |
|---|---|
| Command kind | Narrow allowlisted operation understood by one current owner. |
| Target | Stable quest, location, character, faction, item, or campaign ID. |
| Initiating response | Stable scene/node/response IDs for traceability. |
| Preconditions | Current facts the owner must revalidate before mutation. |
| Idempotency scope | Campaign/session or owner scope in which a retry maps to the same operation. |
| Arguments | Typed, validated values; no arbitrary UI object or rendered text. |
| Result states | Applied, AlreadyApplied, Rejected, Deferred, or Failed with typed reason. |
| Durable owner | Existing save-section authority if the operation changes campaign state. |
| Dependent commands | Ordered follow-up operations and how partial success is presented. |
| Result copy | Localized lines keyed by actual result, plus neutral fallback. |
| Retry rule | Whether retry is safe, when eligibility can return, and how duplicates are prevented. |

This sheet is authoring/integration evidence. It does not justify a central dialogue command registry. The implementation should dispatch through existing owner/event seams and validate command kinds at the narrow boundary that already owns them.

### Idempotency and one-shot response semantics

The command owner, not the UI, is the final duplicate guard. The UI can disable a button while a command is pending, but duplicate protection must survive double input, focus activation, redraw, quick reopen, and save/restore. A stable idempotency key should derive from a durable request identity already available to the owner, such as campaign scope plus quest instance and authored response ID. Do not base it on localized text, wall-clock time, panel instance ID, or a fresh random value.

The owner should return AlreadyApplied when it can prove the same logical operation has already been accepted. The dialogue layer then reads the actual current outcome and selects the same factual success copy without replaying the transfer/reward. If the owner cannot distinguish a repeated request from a new request, the content must not offer that operation as safely repeatable.

Repeatable conversation options need an explicit policy from their owner: repeatable observation with no mutation, repeatable command with bounded consequences, or one-shot command with a completed marker. A scene loop is never itself a cooldown. A response disappearing from a menu is not a persistent completion fact.

### Ordered effects without a distributed transaction

When a response affects more than one owner, describe the dependency graph and all partial outcomes. Do not imply that independent game systems commit atomically. A safe chain may be:

1. Quest owner validates the active phase and records an accepted decision.
2. Resource owner applies a physical transfer if stock is still available.
3. Quest owner records the dependent objective only after the resource result is confirmed.
4. Map/expedition owner independently reveals or schedules a location opportunity when its rules permit.
5. Relationship/memory owner records a supported acknowledgement if that effect is optional and its API accepts the event.

If step 2 rejects, step 3 must not record the transfer objective. Step 4 can still be deferred or remain pending if it is not a dependency. Step 5 cannot claim that the clerk remembers a completed transfer. The scene reports each committed fact accurately and offers a valid retry or alternate route.

Do not create a generalized saga service, cross-system transaction, outbox, or compensation ledger for this plan. Use only mechanisms already present in current owners. If reliable partial completion cannot be represented by existing APIs, narrow the first slice to one durable command or stop for an explicit architecture decision.

### Failure classes and player-facing treatment

| Failure class | Owner/runtime interpretation | Player-facing behavior |
|---|---|---|
| Stale precondition | World changed since the option appeared. | Refresh choices and explain the practical change. |
| Ordinary rejection | Valid request, but current owner rule disallows it. | State what was not done and offer an alternate or exit. |
| Deferred availability | The action may become valid after a named event/prerequisite. | Keep the quest alive and explain when/where to try again. |
| Missing optional target | Expansion/character/location content is absent. | Use neutral or alternate content; preserve core progression. |
| Duplicate request | Same logical commitment was already processed. | Query owner state and show the confirmed result once. |
| Operational failure | Owner could not determine or complete the operation. | Preserve the scene and offer safe exit/retry; avoid false success. |
| Invalid authored command | Content references unsupported command or target. | Block at validation/release; runtime falls back safely if encountered. |

Avoid generic red error banners for ordinary story consequences. A refused transfer can be a meaningful story event; an unexpected internal failure is an operational problem and should not be disguised as character intent.

### Unentered Shelf command trace

The fixture offers three dispositions but only one changes physical stock. Their result contracts differ:

- **Reserve locally:** quest owner checks that the active objective permits a local hold; resource owner validates stock and commits the reservation/transfer supported by its current model; quest owner records the accepted result. If stock is unavailable, no completion is reported.
- **Wait for verification:** quest owner records the decision to keep investigating if that distinction exists in its current API. This can be a no-op on inventory. The player receives a clear next evidence route or a truthful delay.
- **Request a route:** Plan 18 receives the canonical location requirement and returns an exact target, permitted equivalent, clue, or deferral. The dialogue does not mark the destination discovered until the map/discovery owner confirms that event.

The response preview says what the player is asking to do: “Set these bundles aside here while we keep the other claim open.” It does not promise that stock can be moved. After Applied, the clerk says, “The room's count is updated. The other shelter's receipt is still missing.” After Rejected, the clerk says, “The count does not match what we can move. Nothing has been reassigned.” After Deferred, the clerk says, “I can keep the question open until the route is checked.” If the response was AlreadyApplied after a reload, the scene queries the owners and uses the Applied line only if the saved canonical facts still support it.

This trace separates a quest decision, a resource operation, and a location lead. If the current resource owner does not support reservations, rewrite the local-hold choice as a non-mutating “keep investigating” option rather than creating a parallel stock ledger.

### Fail-forward completion contract

A failed objective and its recovery route need separate IDs and separate evidence. The outcome ledger for a timed warning should therefore include:

1. The primary intent: warn the crew before the storm.
2. The deadline owner and exact expiry fact.
3. The recorded result: warning not delivered on time.
4. The preserved information: crew's last known shelter, if already discovered.
5. The recovery offer: deliver a face-to-face message, only if the quest owner can start that follow-up.
6. The recovery location requirement: exact/equivalent/clue/delay semantics from Plan 18.
7. The delivery proof: interaction/event confirmed by its current owner.
8. The social response: relationship/faction result from its own owner, if any.
9. The final journal copy: both failure history and recovery result remain visible.

The fail-forward action does not rewrite the original timeout as success. It can produce a different resolution with its own reward, relationship outcome, or no reward. If the follow-up cannot be started because no supported quest path exists, display the failure truthfully and leave the player with a meaningful non-mutating next step; do not fabricate a new quest instance.

### Integration trace and observability

For the first committed response, record a bounded trace:

| Trace point | Expected evidence |
|---|---|
| Presentation | Scene/node/response IDs and result of Plan 21 conditions. |
| Selection | Stable response ID, input method, and request intent. |
| Revalidation | Owner facts checked and whether the snapshot became stale. |
| Submission | Allowlisted command kind, stable target ID, idempotency identity. |
| Owner result | Typed state and compact reason/reference. |
| Follow-up | Ordered dependent command results, if any. |
| UI update | Correct result line, focus, transcript, and journal/map refresh. |
| Persistence | Existing owner capture/restore path and post-load query. |

This is test/development evidence, not authorization for a new analytics or telemetry system. Logs should not store the entire conversation, hidden condition values, private memory text, or inventory dump. A stable trace identifier helps diagnose the chain while canonical state remains in its current owners.

### Promotion checkpoints

Promote consequence types one at a time. First demonstrate an informational result that reads an owner fact. Next route one quest transition and restore it. Then add a map clue/selection handoff. Only after those seams are stable should a package add resource transfer or relationship/faction effects. Major world or ending consequences require a dedicated audit of all paths that can reach them, including old saves and absent expansions.

At each checkpoint, freeze the authored text against the actual typed outcomes. If the owner changes its result contract, update the graph's fallback and the focused fixtures in the same integration package. A compile or catalog load alone cannot prove that the dialogue reports the accepted result; review the full interaction from player choice through owner state and later revisit.

## Continuation pass 6 acceptance

The command package is ready when each state-changing response has an owner-validated command sheet, retries are safe without UI-only protection, dependent effects have explicit partial-result rules, failure and recovery preserve truthful history, and a complete trace proves presentation, owner result, UI update, and restore behavior.

## Continuation pass 7 — consequence-scope matrix, command families, and integration traces

### Consequence scope is a design contract

Classify the result before selecting an owner command. The same line can sound consequential while changing no state, or look small while changing a major campaign route:

| Scope | What changes | Source of truth | Minimum result handling | Production placement |
|---|---|---|---|---|
| Cosmetic | Wording, tone, or a nonpersistent scene detail. | Authored graph and current read facts. | Re-render safely; no durable command. | Core dialogue framework. |
| Local | Current scene interaction or a local noncampaign presentation detail. | Existing scene/location owner if stateful. | Clear scene feedback and safe exit. | Core only when a current local owner exists. |
| Quest | Offer, acceptance, objective, failure, completion, or resolution. | Current quest runtime/questline owner. | Typed result, journal refresh, duplicate protection. | Core architecture; content by route. |
| Relationship | A supported interpersonal event or band change. | Current relationship/memory owner. | Accepted fact drives later callback; neutral if absent. | Optional character content unless base progression requires it. |
| Faction | Reputation, access, pact, or faction response. | Current faction authority. | Typed outcome and access refresh; no graph-side arithmetic. | Core if needed for campaign; otherwise expansion. |
| World | Canonical location, resource, hazard, encounter, or world event. | Existing owner for that concern. | Owner validation, partial result, map/UI refresh, save proof. | Requires a specific integration claim. |
| Ending | Major resolution or campaign endpoint. | Current ending/campaign authority. | Cross-quest validation, migration, terminal result, and history. | Dedicated milestone/integration package. |

For each scope, separate “response selected,” “request accepted,” and “outcome observed.” A cosmetic response can be applied immediately. A quest or world consequence cannot. Relationship and faction scopes may require a result line only after the owner accepts the event. Ending consequences need a review of every reachable prior choice, not just the scene that presents the final button.

### Allowlisted command families

Start with explicit command families that match current owners:

- QuestOffer, QuestAccept, QuestAdvance, QuestFail, QuestResolve, QuestAbandon, or other exact transitions the current quest API supports.
- RevealClue or request discovery only if the existing map/discovery owner has a corresponding command.
- RequestExpeditionOpportunity as a requirement handoff, distinct from starting the expedition.
- StartExpedition only through the existing preview/start command and its current validation.
- TransferResource only when an existing inventory/resource command confirms amount and target.
- RecordRelationshipEvent only when the existing memory/relationship owner supports the event.
- RequestFactionOutcome only through the current faction owner.
- ResolveCampaign only through the approved ending authority.

These names describe semantic command groups for plan review; they are not commitments to add these enum values. During integration, map each supported action to its real API and reject unsupported kinds. Content cannot submit arbitrary side effects such as “set any field to this value.”

### Consequence sequencing patterns

**One-owner operation:** revalidate, submit once, render the accepted result, refresh. This should be the default for early integrations.

**Dependent two-owner operation:** owner A confirms a prerequisite, then owner B performs the dependent effect. If A rejects, B is never called. If B rejects, the quest owner does not report the dependent objective complete. State the partial outcome and offer the route the owners support.

**Independent optional effects:** a required quest transition can succeed while an optional memory callback or map clue is deferred. Report both outcomes separately; failure of the optional effect does not roll back the required result.

**Terminal campaign operation:** validate prerequisites, present explicit confirmation, submit through campaign authority, then render the terminal state from the returned result. Do not allow scene exit/reopen to replay a terminal operation.

Do not generalize these into a transaction engine. Use current owner order and persistence. If the code has no safe way to represent the intermediate result, reduce the content to one owner or ask for a signed architecture decision.

### Result copy design

For each command, author copy for all results that can occur:

- **Applied:** say exactly what is now true.
- **AlreadyApplied:** use current owner state and avoid a second reward/transfer.
- **Rejected:** say what did not happen and why in player terms.
- **Deferred:** say what event/prerequisite can make it available later.
- **Failed:** preserve the interaction and offer a safe exit or retry if owner-confirmed.
- **Unknown after restore:** query current state; if still unresolved, use neutral copy that makes no success claim.

Prefer a small number of truthful reusable result patterns to six near-identical success paragraphs. However, do not reuse a generic line if it obscures different outcomes. “The route is ready” and “The route request is logged” are not interchangeable.

### End-to-end trace across Plans 17–22

The Unentered Shelf route is a compact integration exercise:

1. **Plan 19:** content bundle defines a permanent shelter context, authored evidence interactions, and a provisional dialogue graph. It identifies which text is generated flavor and which facts are owner-sourced.
2. **Plan 17:** the quest owner accepts the investigation, tracks evidence, and decides whether local hold, confirmed delivery, or unresolved closure is a valid outcome.
3. **Plan 18:** if the player requests destination evidence, the opportunity layer offers the exact site, a permitted evidence equivalent, a clue, or a delay.
4. **Plan 20:** the graph presents only valid questions and choices; stable IDs carry no mutable campaign authority.
5. **Plan 21:** conditions distinguish report, discovery, skill interpretation, location availability, and current stock; unknown facts take neutral routes.
6. **Plan 22:** the selected response is revalidated and sent to the quest/resource/map owners; the graph waits for typed results.
7. **Later revisit:** the graph recomputes from confirmed quest/resource/map/memory facts and shows the accurate outcome.

Failure at any step has a visible boundary. If a referenced content row is missing, Plan 19's validation catches it. If the quest is not active, Plan 17 rejects the transition. If no legal location exists, Plan 18 defers or offers the authored fallback. If the response condition is unknown, Plan 21 chooses its safe presentation. If a command is rejected, Plan 22 displays no false success. If a saved state lacks an optional fact, the neutral revisit remains valid.

### Endings and cross-quest consequence review

An ending choice is a separate production class because it combines several facts and may permanently settle unresolved content. Its dossier must list:

- every quest/faction/relationship/world fact the ending reads;
- the owner supplying each fact and how stale values are refreshed;
- all responses and their consequence previews;
- whether the player can defer or return before committing;
- accepted, rejected, duplicate, and old-save outcomes;
- changes to locations, faction access, surviving characters, resources, and follow-up scenes;
- expansion-disabled behavior and stable ending ID;
- final journal/chronicle entry and localized transcript;
- migration behavior when a prior quest definition has been retired.

Do not calculate a campaign “ending score” inside dialogue unless the current campaign authority already owns that model. If prerequisites span several owners, define the read/commit sequence and how a changed fact is handled. A major choice should not disappear silently when one optional expansion is absent.

### Testable command matrix

Each implemented command family should have a focused behavior matrix:

| Case | Expected state mutation | Expected graph behavior |
|---|---|---|
| Valid first request | Owner applies once. | Show result copy and refresh. |
| Duplicate request | No duplicate state/reward. | Query state and show accepted result once. |
| Stale precondition | No mutation. | Keep scene open; announce refresh. |
| Rejected business rule | No mutation. | Show authored reason and alternate route. |
| Deferred prerequisite | No premature mutation. | Keep task possible and identify next condition. |
| Missing optional owner | No inferred effect. | Neutral route; preserve critical flow. |
| Save before request | No pending command required. | Re-evaluate ordinary scene on restore. |
| Save after applied request | Existing owner stores result. | Query owner and suppress repeat action. |
| Save after deferred result | Persist only if current owner does so. | Re-query; do not invent a dialogue pending record. |
| Partial multi-owner completion | Only accepted owner mutations remain. | Render each result distinctly and continue safely. |

Tests should assert both owner state and player-observable feedback. A passing command unit test cannot prove correct dialogue, map, transcript, or focus behavior.

### Production sizing by consequence scope

Cosmetic and information-only scenes can be authored and reviewed in small batches. Quest commands need the current quest owner and save path. Relationship/faction outcomes require independent source and callback review. World changes need the owning system, persistence, UI refresh, and deterministic replay. Endings require full cross-quest continuity and old-save handling. Estimate the schedule from the highest consequence scope in the graph, not the median line.

Keep the first content wave to one low-scope informational scene and one quest-owner transition. Expand to map, resource, relationship, faction, and ending effects only when the previous owner seam has been proven. This sequence gives narrative authors a usable graph and result-copy pattern without leaving half-connected effects in content.

## Continuation pass 7 acceptance

The scope and command package is ready when every response effect is classified and bound to a verified owner, multi-owner steps have explicit dependency and partial-result handling, the shared six-plan trace can be walked from authored definition to later revisit, and any ending work has a dedicated campaign-level contract.

### Recovery after an accepted partial effect

An accepted first step remains true even when a dependent second step fails. If the quest owner accepts a decision and a later resource transfer is rejected, record the quest decision only if its semantics are independent of the transfer. If the decision promised a completed transfer, keep the objective incomplete and expose the accepted intent as pending only through the current quest owner. Do not erase a confirmed owner event to make the graph look atomic.

If a compensating action is needed, it must be a new owner-approved operation with its own preconditions and result. For example, if a location clue has already been revealed but the expedition becomes unavailable, the compensation is not to “undiscover” the location; it may be to defer travel and explain the current route restriction. A story does not require every partial result to roll back.

If there is no safe compensation or continuation, keep the partial state truthful, stop subsequent dependent commands, and route the remaining work to the integrator for an architecture decision. Never use dialogue copy to conceal a partial commit.

### Response confirmation and player trust

For an irreversible choice, state the practical action before execution, then acknowledge the owner's result. Confirmation copy is not a legal waiver; it is a comprehension tool. It should identify the target and consequence in the terms the player knows: destination, resource, faction, or ending. After an owner rejection, do not blame the player for accepting a previously visible option; explain the changed condition and restore a valid path.

When an accepted result is delayed, distinguish waiting for an owner result from waiting for an in-world prerequisite. The former is an operational pending state and should be rare/handled by the host; the latter is a quest state owned by the game. Do not present an indefinite spinner or unresolved request as a story deadline.

## Continuation pass 8 — command lifecycle, owner-specific outcomes, and recovery copy

### Commands grouped by effect scope

Use the consequence matrix from pass 7 to select the narrowest command that matches the player's intent:

| Scope | Command shape | Validation questions | Reversal policy |
|---|---|---|---|
| Cosmetic | Choose an authored line/variant. | Is the fact still current for the scene? | Recompute on next presentation. |
| Local | Request one local interaction change. | Does a current location/scene owner support it? | Owner-defined; no dialogue rollback. |
| Quest | Request one quest transition/objective result. | Is the instance active and is this transition legal? | New transition or supported recovery, never rewrite history. |
| Relationship | Record a supported event. | Does the memory/relationship owner accept this event ID? | Later owner event, not subtraction from an arbitrary score. |
| Faction | Request standing/access outcome. | Does the faction owner define this effect and current precondition? | Explicit owner command, if available. |
| World | Change route, resource, hazard, or location state. | Which canonical owner validates the state and save path? | Compensating action only through that owner. |
| Ending | Resolve campaign outcome. | Are all prerequisites current and has the player confirmed? | Usually terminal; any correction requires campaign migration policy. |

A response may emit no command at all. Treat “the player heard a rumor,” “the player considered an option,” and “the player accepted a task” as distinct facts. Listening can reveal authored information; accepting a task calls the quest owner; acting on the task calls whichever owner controls the action.

### Owner response schema and safe host behavior

The host adapter should receive a typed response containing accepted status, stable result/reason ID, canonical affected target, and any owner-supported revision marker. Avoid returning an arbitrary object bag with fields the dialogue layer interprets opportunistically. The host maps the result ID to authored copy, refreshes affected views, and routes to a known node or exit.

On a malformed response, missing owner, unknown reason ID, or thrown operational error, do not infer success. Keep the graph in a safe state, release pending input, restore focus, and offer a neutral error/retry/close path according to the current UI contract. Development diagnostics record stable IDs and failure kind; player-facing copy does not expose internal exceptions.

If the owner reports Applied but the UI refresh fails, durable state remains applied. On next open, query the owner and render the actual state. A presentation fault cannot undo a canonical mutation. Conversely, rendering success before an owner response is a false claim even if the operation is likely to succeed.

### Detailed Unentered Shelf outcome script

The evidence-review scene ends with a player request. The host and owners process one of three routes:

**Request to keep investigating:** Quest owner confirms the active quest can remain open. No inventory command is sent. If the quest owner reports the task already resolved, the graph refreshes to the outcome view. Copy after Applied: “All right. Keep the bundles where they are until we can say more.”

**Request a dispatch lead:** Quest owner confirms the location objective is still relevant; location-selection seam receives its exact/equivalent/clue/delay policy; map owner confirms the resulting discovery presentation. Copy after a clue result: “There is a lead, not a marked destination. We will need to follow it.” Copy after Deferred: “Nothing on the board can take us there today. The question remains open.” If the quest ended during the call, discard the stale location request and show the current quest result.

**Request local reservation:** Quest owner validates the outcome; resource owner applies a supported transfer/reservation; quest owner records the dependent objective after confirmation. Copy after full success: “These are accounted for here. The other shelter's receipt is still missing.” After resource Rejected: “The store count does not support that move. Nothing has been reassigned.” If no reserve command exists, this response must be removed or rewritten as the non-mutating investigation route before release.

Each path produces an independent transcript result and journal/map refresh. None treats the command request itself as proof that a character remembers or a faction approves.

### Recovery and compensation rules by scope

Cosmetic variation can be recomputed. A local presentation change can use its owner's inverse only if that inverse is supported. A quest failure can create a follow-up, but does not delete the original failure. A relationship event can be answered by a later relationship event, not by subtracting trust. A faction action can be renegotiated only under the faction owner's rules. A world resource transfer may need another explicit transfer to compensate; it cannot be silently reversed because later prose changed. An ending correction is not ordinary compensation and requires a campaign migration decision.

For each multi-owner action, specify which earlier effects remain if a later effect fails. If a resource moved but the quest owner cannot record its objective, preserve the resource fact, expose a truthful recovery path to reconcile through the current quest owner, and stop duplicate transfer attempts. Do not rely on journal copy as the only record of a discrepancy.

### Idempotency key and replay cases

Test duplicate protection for:

- rapid double confirm from keyboard;
- repeated controller submit before UI redraw;
- panel close/reopen while request resolves;
- owner result arriving after scene exit;
- quick save/load after Applied;
- restore after Deferred;
- retry after Rejected because a legitimate prerequisite changed;
- old save with response ID but no optional expansion content;
- repeated conversation where the option remains visible by design.

The idempotency identity must distinguish separate quest instances when the same authored response appears in two campaigns or rotations. It must also collapse retries of the same commitment. Let the owning authority define the correct key scope; do not guess from scene ID alone.

### Player comprehension for high-impact choices

Before quest, faction, world, or ending commitment, show a short consequence preview that names the known affected target and uncertainty. Example: “Reserve these counted bundles here; the neighboring shelter's receipt will remain unconfirmed.” This preview states intent and known tradeoff, not success. After execution, copy is chosen by owner result.

For a major ending, list irreversible changes and let the player review the current journal/map before confirming where the game supports that behavior. Do not put hidden consequences in the preview simply to surprise the player, and do not promise details whose owner state may have changed since the prior scene.

If the option is unavailable, the reason should match the source: “Need the route clue,” “The expedition preview is no longer valid,” or “The task has already been resolved.” This keeps the narrative voice while respecting actual system state.

### Concurrent input and in-flight request policy

While a consequential request is pending, suppress only duplicate activation of that same response; keep safe close/back behavior available. A second unrelated response must not start against a stale snapshot while the first command is unresolved. If a result is returned after the player exits, route it to the owning quest/journal/map view and do not reopen dialogue automatically.

If the UI receives two identical results, owner idempotency and stable command identity collapse them to one logical effect. If it receives conflicting results for the same request, treat that as an operational fault, query the authoritative owner, and render the current state after reconciliation. Do not choose the last callback or whichever line arrived first.

The host should make command dispatch single-flight per logical response unless the current command owner explicitly supports safe concurrency. This is a presentation/input guard, not the durable duplicate guarantee. Save/restore and owner-level idempotency remain necessary.

### Event facts versus command requests

Keep requests and accepted facts distinct:

- A command request asks an owner to do something.
- An accepted result states what the owner did.
- A domain event reports a fact that happened.
- A dialogue node presents authored text about a fact.

Dialogue must not emit a domain event just because the player selected a response. For instance, selecting “I will take the route” does not mean an expedition started. The current expedition owner reports start success; only then may a quest objective listen for dispatch. A scene can request the operation and show the returned result, but it cannot manufacture the event that satisfies itself.

Where an existing host/event bridge already publishes accepted facts, use that bridge and its owner. Do not subscribe both the panel and quest runtime to a UI callback that can be invoked twice. One accepted operation should have one event path and one observable consequence.

### Re-entrancy and callback ordering

An owner callback may refresh a scene, update a quest journal, alter map opportunities, and dispose the dialogue panel. Define which host component owns the response completion and UI lifecycle. The panel should ignore callbacks after disposal while durable owner state remains intact. A reopened scene queries current state instead of relying on a callback retained by the old panel.

If a quest update triggers a location-selection refresh, let the host process the quest result first, then rebuild Plan 18's derived opportunity set at the next planning entry or stable UI boundary. Do not recursively start a new expedition from the quest callback. If the current architecture cannot order those events predictably, keep the first integration slice synchronous through its existing seam or request a bounded lifecycle decision.

### Journal, map, and transcript reconciliation

After a successful command, refresh only the read models affected by its accepted result: quest transitions update the quest journal; confirmed discovery updates map knowledge; a resource transfer refreshes the relevant stock view; a faction command refreshes access/standing where the current UI exposes it. Do not broadcast every response to every panel or write copied dialogue-outcome state into a new journal.

The transcript says what the character and player said. The journal says what the quest owner accepted. The map says what the discovery/location owner knows. These records can differ without contradiction: the player may hear a rumor that is not yet a map discovery, or accept an investigation without confirming the rumored event. A reconciliation review checks that each surface reports its own authority and does not overstate the command result.

## Continuation pass 8 acceptance

The lifecycle package is ready when command result data is typed and owner-sourced, host failure never invents success, each shared side-quest disposition has a separate proven route, duplicate/retry cases cover scene and save boundaries, and compensation follows the owner of the affected consequence.

## Continuation pass 9 — request/result event cycle, read-model refresh, and promotion map

### Request/result event cycle

Route a consequential conversation through a clear sequence:

1. **Authored graph** exposes a response after its conditions pass.
2. **UI input** selects one stable response ID and disables only duplicate activation for that pending request.
3. **Context layer** supplies the minimum validated facts needed for the command request.
4. **Host adapter** maps the authored command reference to the current domain API.
5. **Domain owner** checks current preconditions, applies or rejects the operation, and returns a typed result.
6. **Existing event/refresh path** notifies dependent read models of an accepted fact.
7. **Dialogue graph** selects a result node from the typed result and fresh context.
8. **Later revisit/restore** queries canonical owners and renders the current state again.

The request is not the event; the event is not the dialogue line; the dialogue line is not the save. This separation makes it possible to prove which authority accepted a change and to show the same truth after a panel is disposed or the game reloads.

### Read-model refresh ownership

After owner acceptance, refresh only surfaces that display affected facts. A quest state refresh can update journal availability and a dialogue condition. A discovery result can update map fog and expedition opportunities. A resource transfer can update stock/readiness. A faction result can update access options. A relationship event can change an NPC callback when that owner publishes the new fact.

The dialogue panel should not send a broad “refresh all game state” command for every line. It should route through the current host/event seam, and each presentation surface should query its existing owner. If the event bridge lacks a relevant notification, a bounded adapter change may be needed; do not create a second manually synchronized cache.

### Result-node inventory

Each command-backed response should have a result destination for:

- confirmed application;
- duplicate/already-applied response;
- expected rejection;
- in-world deferral;
- unavailable optional owner;
- unexpected operational failure;
- restored current state after scene closure.

Several result types may reconverge when they truthfully communicate the same situation, but never merge “rejected” and “deferred” if the player needs a different next action. A neutral result node can offer a safe exit when the owner cannot answer; it cannot claim the action failed in-world.

### Promotion map by consequence scope

| Promotion wave | Scope introduced | Entry criteria | Exit evidence |
|---|---|---|---|
| A | Cosmetic and informational. | Stable graph/data loader; no mutation. | Transcript, localization, and branch reachability. |
| B | One quest-owner operation. | Current quest command and restore path verified. | Applied, rejected, duplicate, and post-load behavior. |
| C | Map/discovery and expedition handoff. | Map and expedition contracts independently verified. | Clue visibility, opportunity resolution, preview/start result. |
| D | Resource or relationship result. | Existing owner supports command and duplicate handling. | Partial result, journal/UI refresh, revisit truth. |
| E | Faction/world changes. | Current owner plus access/effect semantics and expansion fallback. | Owner result, save compatibility, cross-scene callback. |
| F | Ending/campaign resolution. | Campaign authority and all relevant quest facts audited. | Terminal result, history, old-save behavior, disabled bundles. |

Do not promote a content package to a later wave because its prose is already written. The scope requires the source API, ownership claim, save/event seam, and focused acceptance evidence for that stage.

### Operational fault containment

If the domain owner throws, returns an unknown result code, or times out, stop dependent commands. Preserve any earlier owner-confirmed facts. The host returns control to a safe scene node, logs stable command/scene/owner IDs, and presents an authored generic recovery line. On the next open, query the owner rather than replaying the request unless its idempotency contract says the retry is safe.

Never convert operational failure into a character's refusal, a faction's betrayal, or an expired quest. That would punish the player for a technical fault and corrupt narrative history. Keep operational diagnostics separate from authored failure states.

### Acceptance trace for the shared vertical slice

The integration handoff should include a trace for one accepted and one rejected outcome:

- response and condition IDs;
- pre-commit fact snapshot and owner markers;
- command kind, canonical target, and idempotency scope;
- owner result/reason;
- dependent commands sent or suppressed;
- quest/map/resource read-model refreshes;
- result node and transcript output;
- save/restore query and duplicate retry result.

The accepted trace proves the requested action became canonical state. The rejected trace proves no false success was displayed and a recovery/exit remained available. Together they are stronger evidence than a happy-path screenshot or a passing isolated command test.

### Sequencing a response that touches multiple owners

Most dialogue actions should submit one domain command. When one player choice legitimately affects several owners, describe a short ordered choreography rather than inventing a universal transaction authority. Identify the primary command whose acceptance defines whether the choice occurred. Then list dependent commands, prerequisites, and the result shown if a later owner rejects. For example, a choice to deliver supplies can request a transfer from the inventory/resource owner, then notify the quest owner using the accepted transfer receipt. The quest objective cannot complete before the transfer is confirmed.

| Operation shape | Order | If a later step fails | Recovery action |
|---|---|---|---|
| One owner, one fact | Submit once to the owner. | Display its typed rejection. | Refresh the option or leave. |
| Physical transfer plus quest proof | Resource owner accepts transfer; quest owner accepts its receipt. | Transfer remains canonical; quest is pending reconciliation. | Explain partial state and offer a supported follow-up. |
| Quest plus faction result | Submit the choice to its owner; send dependent faction command only if permitted. | Do not infer standing/access from dialogue. | Refresh faction options and show a truthful pending result. |
| Discovery plus map disclosure | Discovery owner records the interaction; map reads that fact. | No reveal occurs until discovery is accepted. | Offer another clue route or neutral summary. |
| Multi-target broadcast | Submit once to canonical owner; event consumers refresh their views. | Do not fan out duplicate writes from the host. | Present owner result and affected read-only surfaces. |

This ordering does not authorize a new coordinator or cross-system save section. Use existing commands and events. If the operation has no owner-defined sequence, idempotency key, or partial-failure semantics, stop promotion and identify the smallest missing contract. Do not automatically compensate with refund, deletion, or reputation reversal unless each affected owner supports that reverse operation and restore behavior.

### Concurrency, duplicate input, and cancellation windows

Treat one visible confirmation as a request with a stable identity scoped by the existing command contract. Rapid double input, controller repeat, delayed callbacks after panel disposal, and retries after load must not duplicate rewards or mutations. Disable or mark the response pending while the request resolves, but retain a safe close/back path where the host lifecycle allows it. Pending presentation is not proof of success.

Cancellation has a boundary. Before submission, leaving the scene cancels the unsubmitted choice. After an owner accepts an irreversible operation, closing the panel closes presentation; it does not roll back the result. If the owner returns a pending result, the player can leave and later query current state. Reopening must not submit again. If a command is stale, refresh relevant facts and invite a new deliberate confirmation.

For compound operations, record each accepted result as it arrives and stop dependent work at the first rejection. Do not collapse partial success into a generic failure. The read model shows which facts were accepted, which remain unresolved, and which next actions are legal. If an operational error occurs after an owner accepted, preserve that fact and let the normal owner/event path recover; never ask the player to repeat a physical transfer to repair a notification gap.

### Command-to-result acceptance matrix

Map every command-bearing response through the complete route:

| Stage | Required evidence |
|---|---|
| Availability | Current owner facts and gate result at scene open. |
| Confirmation | Player-visible target, cost, and uncertainty before submission. |
| Request | Stable response/command identity and canonical target. |
| Acceptance | Typed owner result, including duplicate or rejection reason. |
| Dependent work | Ordered follow-up commands, or proof that none are needed. |
| Read-model update | Existing event/host route refreshes affected journal, map, actor, faction, or resource views. |
| Transcript | Exact result line for accepted, rejected, deferred, duplicate, and operational fallback. |
| Restore | Reopen queries current owners and cannot replay a committed mutation. |

No row may say “dialogue sets flag” unless the current canonical owner is explicitly that flag authority and route, persistence, and consumer contracts are verified. A UI-local boolean can remember that an animation played; it cannot substitute for quest completion, discovery, delivery, reputation, or world state.

The strongest closeout fixture confirms an action, closes the panel before its presentation refresh, restores the scene, and queries all affected owners. The panel must show the canonical result even if the original callback was lost. Repeat the request with the command's supported duplicate identity and confirm there is no second mutation. Pair it with a stale/rejected request to prove the interface explains that no action occurred and still offers a safe exit.

### Partial-result policy for narrative actions

Some choices can produce a truthful partial result: a smaller amount transferred than requested, one destination confirmed while another remains unknown, a route clue found without safe passage, or an agreement accepted by one faction representative but not ratified by its authority. The response model must name the scope of acceptance and preserve unresolved questions. Do not create a synthetic “half complete” campaign state in dialogue; let existing owners retain their individual facts and let quest progress project the combination.

Before writing a partial-result line, identify which operations have already been accepted, which were rejected or deferred, and which were never attempted. A dependent operation that was suppressed is not a rejection. A missing optional owner is not an in-world refusal. An operational failure is not a character's choice. These distinctions determine the next available response and whether the player needs to wait, retry, negotiate, or pursue a different route.

If partial completion creates a follow-up task, that task must have an owner-supported eligibility trigger and a stable link to the original result. Its offer cannot erase the original transcript or repeat already accepted costs. If no follow-up is supported, close with a clear unresolved note and leave the world facts intact. This keeps narrative ambiguity deliberate while making the system outcome auditable.

### Consequence-route closeout

The route inventory is complete when every command-bearing response names its canonical owner, request identity, dependent effects, duplicate behavior, result node, and restore behavior. Mark informational responses so reviewers can confirm that reading dialogue has no hidden mutation. For compound choices, list the accepted sequence and each partial-result branch. A missing command owner or undefined partial-failure rule blocks integration of that action while leaving unrelated prose available for revision. Closeout evidence should show accepted and rejected routes after panel teardown and restore, with current owner state matching the dialogue shown to the player.

## Continuation pass 10 — consequence timing, campaign callbacks, and effect budgets

### Consequence scope and consequence timing are separate axes

The existing scope labels describe which part of the game a result affects. A second review axis describes when the player can observe it. Keep these axes separate: a faction consequence can appear as a current access change or as a later negotiation; a relationship consequence can alter the next line or wait until a supported callback; a quest result can complete now while the world presentation updates at the next normal refresh.

| Timing class | Example observation point | Required source | Player expectation |
|---|---|---|---|
| Immediate | Result node directly after accepted command. | Typed owner result. | The line reports exactly what was accepted or rejected. |
| Same scene | Another node becomes available after the result. | Refreshed read model from the affected owner. | New option appears only if current facts qualify it. |
| Next visit | An NPC, map, or journal callback reflects the accepted fact. | Durable existing owner fact or supported event. | The player can revisit to see it; no hidden extra command occurs. |
| Next expedition | Location pool or route information changes for a later dispatch. | Current map/quest/faction/event owner. | The result may change opportunity, not guarantee arrival. |
| Owner event/window | A later clock, campaign, or faction event reads the result. | Existing event/campaign owner. | Timing is explained as pending; dialogue creates no timer. |
| Campaign resolution | Ending/campaign authority reads accumulated facts. | Existing ending input owner. | Earlier choices contribute; no single response selects the ending directly. |

The timing class does not authorize a new queue or scheduler. If a result must wait, name the current owner event that will make it visible or make the effect immediate and narrower. “Something may happen later” is too vague to validate. If no owner can produce the callback, keep the consequence local or hold the branch for an architecture decision.

### Callback ledger by player-visible promise

For each consequential response, record the promised effect and the earliest supported moment when the player can verify it. This is an editorial/integration ledger, not a mutable gameplay ledger. Its purpose is to catch promises with no delivery surface and consequences that appear before the player can understand their cause.

| Promise class | Example line before confirmation | Proof after acceptance | Invalid result wording |
|---|---|---|---|
| Local help | “I can keep the room clear while you check.” | Current scene changes through its owner, if supported. | “The shelter is safer now” without a world fact. |
| Quest commitment | “Bring back a verified route and I'll reopen the request.” | Quest owner records accepted instance/objective contract. | “The search is complete” at offer acceptance. |
| Relationship | “I'll remember that you came back.” | Relationship/memory owner exposes the supported event. | “You have earned my trust” from a purely cosmetic line. |
| Faction term | “The watch can escort one group through.” | Faction/access owner accepts that limit. | “Everyone can travel freely” from one representative's dialogue. |
| Resource transfer | “These supplies are yours if the stores count holds.” | Inventory/resource owner confirms exact accepted amount. | “Delivered” before transfer acceptance. |
| Campaign contribution | “This record will be read at the council.” | Campaign/ending owner accepts the observation input. | An immediate ending verdict before resolution. |

If the owner rejects a requested effect, use result copy that explains what did happen and leaves a valid next action. If the effect is deferred, say what will trigger reevaluation when known. If a dependency is unavailable, do not frame it as an in-world refusal. The callback ledger should map the response ID to all later text and UI surfaces that may mention its consequence, so content can be revised without an untraceable branch.

### Consequence budget and branch-depth control

Give every player choice one primary durable consequence. Add a secondary durable effect only when it follows from the same decision and each owner route is independently justified. A single response that changes quest state, inventory, faction access, a companion relationship, map availability, and an ending observation creates a large verification surface and makes player feedback hard to parse.

Estimate the branch cost with more than the initial node count:

| Cost dimension | What to count |
|---|---|
| Owner breadth | Distinct authorities receiving a command or publishing a new fact. |
| Persistence | Existing save sections that capture/restore affected state. |
| Follow-up scenes | Later lines and conditions that must acknowledge the result. |
| Map/expedition impact | Required markers, candidate pools, route states, and fallbacks changed. |
| Failure surface | Rejection, deferral, duplicate, partial, stale, and operational outcomes. |
| Localization surface | New text keys, short-choice ambiguity, and variant expansion. |
| Campaign reach | Old-save, optional-package, and ending-resolution compatibility. |

If one response exceeds the reviewed budget, separate the decisions into understandable commitments or reduce the number of lasting effects. A player should know whether they are accepting the quest, transferring the item, endorsing the faction term, or making all of these choices. Avoid hiding durable effects in a “continue” or “say nothing” response.

### Late-game and major-faction consequence boundaries

Large faction arcs need a written map of which authority owns contact, standing, access, resource exchange, hostilities, and eventual campaign resolution. A dialogue graph can expose negotiation and present terms; it does not calculate the faction's total state from branch visits. If a late-game coalition or rival group is proposed, define what the current faction/campaign owners can represent before writing many endings around it. A missing alliance contract is an architecture gap, not a reason to set several faction values from one node.

An ending-facing choice should usually record an owner-approved observation or commitment. The campaign resolver later reads that fact with other quest, faction, character, and world outcomes. Preserve the distinction between “the player promised,” “the faction accepted,” “the route was used,” and “the campaign resolution incorporated the result.” Each may be true at a different time. The final dialogue should explain what is settled and what remains outside the speaker's authority.

If an expansion is disabled, the baseline campaign must still be able to reach its supported resolution. Optional faction results can enrich the ending only when their absence maps to a neutral/unknown input that the current campaign owner understands. Do not make the player look hostile merely because an optional faction definition or save fact is missing. Any new ending consequence needs a continuity review across all supported quest outcomes, not only its initiating conversation.

### Cancellation, reversal, and revisability

Classify an effect as reversible, compensatable, or irreversible only from the owning system's contract. A reversible map filter is not the same as a delivered item; an abandoned conversation can be reopened, while an accepted faction treaty may need a separate owner-defined termination. Do not offer an undo button for a result whose owner has no reversal operation.

Before command submission, the player can usually back out without consequence. After acceptance, later content may allow a new decision that changes the world again, but it does not erase the fact that the original choice happened. Journal/history copy should use time-appropriate language: a past agreement can remain part of history even after its current access condition ends. When a player can revisit or renegotiate, show the current state and the new cost before confirmation.

For a failed or expired action, separate retry from reversal. A retry attempts an unaccepted operation under fresh conditions. A recovery quest offers another route after a recorded failure. A reversal is a new command that compensates for an accepted result. These are not interchangeable. Route each through the current owner and make the line reflect which operation the player is choosing.

### Temporal callback acceptance trace

Prove one immediate consequence and one delayed callback using the same owner fact. Capture the accepted command and typed result; close the scene; advance only the existing event/dispatch boundary that is supposed to expose the callback; then reopen the affected view and query the owner. The callback should appear once, refer to the correct scope, and avoid resubmitting the initial operation. Repeat after restore and with the optional callback package disabled.

The negative trace changes one precondition so the owner rejects the original request. No delayed success line should appear later. The neutral or recovery transcript remains available; if the callback surface is unavailable, the accepted owner fact is still correct and can be shown through another current read model. This pair demonstrates both player-visible follow-through and the rule that a future line cannot claim an event that never occurred.

### Consequence evidence and player history

The player should be able to understand important accepted choices after the original conversation has closed. Use the current journal, quest history, faction access surface, or relationship presentation as its owner allows. Dialogue transcripts can preserve what was said, but a transcript alone is not proof that an effect succeeded. Keep the spoken promise, submitted request, accepted owner result, and later observed consequence distinguishable in both review documentation and player-facing copy.

| Record/view | What it can establish | What it cannot establish |
|---|---|---|
| Conversation transcript | Which line and response the player saw/selected, if the current system retains it. | That a domain owner accepted its command. |
| Quest journal | Current accepted quest/objective result from quest authority. | Inventory moved unless the resource owner confirms it. |
| Resource display | Current stock or possession from its owner. | Which dialogue choice caused it without an event/history source. |
| Faction presentation | Current access/standing from faction authority. | That every individual faction member agrees. |
| Map marker | Current visible/discovered opportunity from map/expedition owners. | Arrival, objective completion, or a future safe route. |
| Campaign closure | Resolved result from campaign/ending authority. | A single dialogue line's full meaning outside its input contract. |

When the game has no historical consequence view, do not create a new ledger merely to make a narrative callback possible. Present the current authoritative state where the player already expects it and use dialogue to explain it. A missing causal history may limit how specific the callback can be; choose a truthful generic line rather than backfilling a source that was never saved.

### Out-of-order updates and result reconciliation

Owner events can arrive after the panel closes or after another read model has refreshed. Each event consumer should be idempotent and refresh from canonical state, not assume that it observed every dialogue callback. If two independent accepted events contribute to one quest, the quest owner decides whether both count and in what order. The conversation should render the currently accepted combination after querying that owner.

| Event order | Safe behavior |
|---|---|
| Quest result arrives before dialogue callback | Reopen/query shows the accepted result; callback does not replay the command. |
| Resource transfer succeeds but quest notification is delayed | Preserve transfer; quest remains pending until its owner reconciles accepted evidence. |
| Duplicate event arrives | Owner's current duplicate contract prevents a second effect; view refresh is harmless. |
| One owner accepts and another rejects | Preserve accepted fact, suppress dependent effects, and show partial result. |
| Result is unknown after an operational fault | Keep safe exit and query again through the owner; do not write a narrative failure. |
| Optional callback package is absent | Show baseline current-state text; canonical outcome remains unchanged. |

If a required synchronization contract is absent, reduce the authored action to one owner or defer it. Do not solve event ordering by coupling the dialogue panel directly to multiple private fields or by writing a manual repair flag. Integration should identify whether the missing piece is the command result, event notification, read-model refresh, or restore query.

### Re-entry contract for consequential scenes

When the player returns to a scene after acting, the graph should determine whether it is a new conversation, a result review, a follow-up offer, or no longer available. Each route has a distinct eligibility source. An accepted quest may keep the scene open to report progress. A terminal result can replace the acceptance node with an aftermath conversation. A follow-up appears only when an owner says it is eligible. The original choice remains part of history and cannot be submitted again by reopening the panel.

Do not use visit count as a substitute for those states. A player who reopens the panel because they misread the text has not made a new commitment. A player who leaves during a pending request has not canceled an accepted operation. A player who revisits after an owner rejection may try again if the current preconditions allow it. The graph's “return” route should query current facts and offer a reliable close/back action in every case.

For command-bearing responses, specify the behavior when an actor moves, a location becomes unavailable, an expansion is disabled, or the underlying quest becomes terminal between visits. An old accepted result should remain readable even if new offers are withdrawn. If no appropriate return scene exists, a current journal/read model can carry the result; do not keep an unusable dialogue node alive just to preserve story history.

### Late-game effect audit by escalation

Large consequences should move through explicit escalation gates: local scene response; current quest/faction/relationship owner accepts its fact; the player can later observe the change; campaign/ending owner consumes supported facts; and only then does the game present a resolution. Each step is independently reviewable and can be optional where the current campaign permits it. A branch does not become ending-critical merely because its dialogue is dramatic.

For a proposed major faction outcome, list who can offer terms, who can accept them, which current owner records access/standing, what world event makes that choice visible, and which campaign input consumes it. Include refusal, no-contact, unavailable-package, and old-save cases. Avoid simultaneously inventing a new faction state machine, a dialogue effect registry, and an ending ledger. If current source owners cannot express the required consequence, the plan records a decision gap and leaves the prose provisional.

### Consequence route severity bands

Use severity bands to choose review depth rather than to give narrative actions a numeric danger score:

| Band | Example | Minimum review surface |
|---|---|---|
| Presentation | Tone or animation only. | Transcript and UI focus review. |
| Local reversible | Temporary scene arrangement or opened conversation topic. | Owner route and scene reopen behavior. |
| Quest durable | Accepted objective/result or new task. | Quest proof, restore, duplicate, failure, and journal. |
| Cross-owner | Resource plus quest, or quest plus access. | Ordered result trace, partial failure, event refresh, save owners. |
| Campaign-facing | Major faction/world fact contributing to resolution. | Full dependency/callback graph, optional package, old-save and ending review. |

If the effect belongs in a higher band, its evidence cannot be reduced because the initial implementation uses a short line. High-stakes consequences cost more because more owners and future content must agree on what happened. The review closes only when the observable result is truthful at the scope and time the player expects.

## Continuation pass 11 — consequence request contracts, result taxonomy, and owner acceptance

This continuation turns the consequence route into an authorable and reviewable contract. It focuses on the information that must travel from a player response to the current owner and back to the scene. The dialogue graph describes what the player can attempt and what they are told; it does not become the place where resources, quest progress, relationships, faction standing, world availability, or endings are stored. Every field below is a planning requirement. Its eventual representation must fit the existing data schema and the existing Core command/event/save seams. If a proposed effect cannot be expressed by a current owner, the plan names the decision gap and holds that effect instead of inventing a shadow authority.

### Route request: minimum semantic envelope

A command-bearing response needs enough meaning to be validated and traced after its originating panel has closed. The exact serialized field names are implementation choices, but authors and engineers should agree on the semantic envelope before expanding content volume. A request can be represented conceptually as follows:

```text
DialogueActionRequest
  request_identity
  source_scene
  source_node
  selected_response
  actor_context
  target_context
  requested_operation
  consequence_scope
  bounded_parameters
  preconditions
  dependency_policy
  expected_owner
  expected_result_kinds
  presentation_key
```

`request_identity` associates one submitted player action with its response. It must be stable for a retry of the same logical action where the owner contract requires idempotency; it must not make two intentionally distinct choices collapse into one. A request identity is a correlation aid, not a second ledger of already-applied outcomes. The owner remains the place that can answer whether the action is accepted, already applied, rejected, or still unresolved.

`source_scene`, `source_node`, and `selected_response` support authoring validation, diagnostics, and the return route. They should be stable content references when the current catalog permits them. They do not need to be copied into every persistent subsystem. If a result record owned by an existing system carries a source reference, it should be included only through that system's established contract. The dialogue layer must not create a parallel history table merely to preserve attribution.

`actor_context` and `target_context` identify the domain entities involved, such as the speaking character, the quest, or the location. A context value should resolve through its canonical owner. It should not contain a copied resource balance, copied relationship score, serialized location availability snapshot, or another value that may become stale between the panel rendering and submission. The response can display a current read model, then the owner validates current facts on the request.

`requested_operation` names the supported action in owner language: accept a quest, submit evidence, offer an item, request access, record a supported relationship interaction, or invoke another operation that already exists. It should never be a free-form script expression. `consequence_scope` tells reviewers which blast-radius class applies; it does not grant authorization to mutate every system in that class. The owner route and its current validation rules are the authority.

`bounded_parameters` contains only the parameters the operation contract accepts. A resource transfer might name a defined item and quantity range; a proof submission might name a known evidence reference; a relationship response might select a defined interaction kind. Arbitrary dialogue text, an unbounded numeric delta, an authored callback name that can call private fields, or a general-purpose effect list is not a safe parameter. Content validation should reject malformed and out-of-range values before they reach runtime.

`preconditions` describe the facts under which the option is presented and, where appropriate, the facts the owner must recheck at submission. Presentation conditions improve relevance. Owner validation protects state when facts changed after display. These are related but not interchangeable. For instance, a response may appear because the character is at the location and the player has enough medicine in the last observed view; the inventory owner still checks current quantity when the action is submitted.

`dependency_policy` records which effects rely on another accepted result and what the route does when that dependency is unavailable. A compound donation-and-quest response should identify whether quest progress is contingent on accepted transfer evidence. It should not treat the two operations as an unordered list. If the prerequisite is not accepted, dependent operations must be suppressed, returned as deferred when retry can be safe, or routed to a defined alternate result. The author should never have to infer this from the order of entries in a JSON array.

`expected_owner` names the current command owner or an integration seam that can identify it. `expected_result_kinds` helps validate that the authored response has copy for every possible non-operational result. `presentation_key` selects wording or a result node; it is not the canonical fact. A stale response key may be logged and fall back to safe generic wording, but must not alter gameplay outcome.

The semantic envelope deliberately omits a dialogue-owned mutable “applied” flag. After a reload or duplicate event, the owner resolves the result. The dialogue scene may query that result for presentation, but it must not make its own answer authoritative. If the existing owner cannot distinguish accepted from unknown, that is a seam gap to document and schedule; adding a local Boolean only hides it.

### Request lifecycle and submission boundaries

Treat option display, player selection, request submission, owner decision, observable result, and scene re-entry as distinct moments. This prevents authoring copy from promising an effect merely because a button was visible. The lifecycle also creates a stable review trace without requiring a new persistent lifecycle manager.

| Moment | Authoring responsibility | Runtime responsibility | Expected player-facing behavior |
|---|---|---|---|
| Candidate construction | Name the operation and scene conditions. | Read current owner views and build eligible options. | Only relevant actions appear; inaccessible choices may be explained if that explanation is safe and helpful. |
| Option display | State cost, risk, commitment, and intended scope clearly. | Render the validated text and command affordance. | The player can distinguish information, proposal, and commitment. |
| Selection | Supply a stable response reference and bounded parameters. | Verify the response still belongs to the active scene. | The selected action is visibly acknowledged, and the UI prevents accidental duplicate taps where appropriate. |
| Submission | Supply owner, dependency, and expected-result metadata. | Dispatch through the established command seam. | Loading/pending feedback does not claim that the world has changed. |
| Owner resolution | Provide result copy for the supported outcomes. | Validate current state and return a typed result or an explicitly unknown operational outcome. | The scene reflects the result rather than the intent. |
| Consequence observation | Identify the journal, character, map, or world read model that exposes the accepted fact. | Refresh through the normal event/read-model path. | The player can see changes at the promised scope and appropriate time. |
| Re-entry | Author an aftermath, retry, or close route. | Query canonical current facts and result history where supported. | Reopening does not repeat an accepted operation or erase a rejection. |

The owner may resolve synchronously or asynchronously. Content should not assume a synchronous response unless the command contract guarantees one. While pending, the scene needs a usable close/back route and should avoid both a false success line and an in-world failure line. If the player navigates away, the request remains under its owner’s lifecycle. The panel does not cancel the operation by disposal. On return, the graph asks for current result state, then offers an appropriate follow-up.

There are three times at which a route can be checked. At authoring or catalog-load time, validate structural facts such as referenced response, owner operation, parameter type, declared result coverage, and localization keys. At option-construction time, evaluate current presentation conditions using current read models. At command time, the owner checks gameplay preconditions and authoritative state. Do not push ownership-specific validation into the content loader, and do not ask the owner to resolve prose, speaker, or translation concerns.

The route should also define a boundary for a response selected from an obsolete view. The player may have left the scene, accepted another option, changed the active actor, or loaded a new campaign context. The host should verify that the selection still belongs to the current interaction. An obsolete UI event should be discarded or reported as stale without submitting a materially different action. If the player deliberately reopens the conversation, the graph reconstructs its current options from owners; it does not replay a stale selection payload.

### Typed result taxonomy and copy obligations

A response should map owner results into a small typed set that is expressive enough for content and narrow enough to validate. Existing public result types take precedence. This table defines planning vocabulary only; it does not mandate that every subsystem adopt one global enum.

| Semantic result | Meaning | Gameplay handling | Dialogue handling |
|---|---|---|---|
| Accepted | The owner accepted and recorded the requested fact. | Continue dependent effects only under their declared dependency rule. | Show success language and, if needed, transition to the accepted aftermath node. |
| Accepted with partial outcome | A supported portion succeeded, with a defined remaining portion unavailable. | Preserve the owner's accepted fact and suppress unsafe dependent work. | Explain precisely what happened and expose the supported follow-up. |
| Already applied | The same logical request is already represented by canonical owner state. | Return the existing state without reapplying the effect. | Show current aftermath, not a second reward or duplicated commitment. |
| Rejected by gameplay rule | Current owner state fails an in-world requirement. | Preserve state; return a reason category the owner is allowed to expose. | Use a grounded refusal, blocked route, or changed-opportunity response. |
| Stale selection | The selected option no longer matches active context. | Do not reinterpret it as a new command. | Refresh choices or ask the player to reopen the conversation. |
| Deferred | The owner or a declared prerequisite says a safe retry can occur later. | Keep canonical state unchanged unless the owner expressly records a pending fact. | Explain the wait or prerequisite; give a discoverable return path. |
| Unsupported feature/package | This optional action is not available in the current configuration. | Keep baseline campaign state valid; do not create partial shadow state. | Hide the option when that is the content contract, or show an honest unavailable route when the player has already encountered it. |
| Invalid authored request | Content violates a schema, reference, or bounds rule. | Reject before gameplay mutation and report through diagnostics. | Use a safe fallback for runtime resilience; treat the content defect as a release-blocking validation failure. |
| Operationally unknown | The owner cannot establish whether the request committed, often after interruption. | Query or reconcile through the existing owner; never guess and never submit a fresh materially different action as a retry. | Keep wording neutral, preserve safe exit, and offer a current-state check. |
| Expired opportunity | The canonical owner says the opportunity is no longer available. | Do not fabricate an accepted or failed quest state. | Explain the changed route and offer any authored alternate or closure. |

The taxonomy separates a gameplay refusal from a service or integration failure. “The quartermaster will not take the offer” is an in-world fact only when the relevant owner returned that refusal. “The current request result could not be checked” is operational uncertainty and must not be voiced as the character rejecting the player. Content should not turn timeouts, malformed data, missing optional packages, or owner exceptions into dramatic failure beats.

Every consequential response requires copy coverage for accepted, already-applied, rejected, and operationally-unknown results, even when some branches are generic. A route with a genuine partial outcome or deferred condition must cover those too. A response that cannot reasonably explain every low-probability result may use approved shared text, but the selected text has to remain truthful for that result. Ensure the shared string conveys whether the operation happened, whether the player must act, and whether a follow-up is available. Avoid vague “Something went wrong” text where the player needs to know whether to check inventory or wait.

Result reasons should be classified into player-actionable and non-actionable groups. Actionable reasons may safely communicate a missing item, absent evidence, insufficient standing, unmet public prerequisite, unavailable character, or closed location, provided the owner exposes that fact and revealing it does not undermine discovery. Non-actionable reasons include internal identifiers, serialization problems, network/process faults, content paths, stack traces, and hidden conditions whose premature disclosure would harm the quest. Authors should write the player-facing copy against reason categories, not engine exception text.

### Consequence scope to owner-route matrix

This matrix makes the boundary between scope and ownership explicit. The owner column is intentionally phrased as the existing domain owner rather than a proposed new dialogue service. During integration, replace each conceptual owner name with a verified current API. If more than one owner participates, name the source fact, ordering rule, and failure behavior for each.

| Scope | Example player action | Canonical owner route | Evidence shown to player | Failure boundary |
|---|---|---|---|---|
| Cosmetic | Speak gently, ask a question, or show hesitation. | Dialogue presentation/graph state only if the graph already supports ephemeral scene selection. | Tone, gesture, or a short follow-up line. | No durable gameplay fact; a reload may reconstruct the neutral scene unless the current dialogue contract persists that choice. |
| Local scene | Open a door, place an agreed marker, or reveal a clue in the present encounter. | Existing location/interaction owner for the local change. | Door, map clue, object, or current scene response. | If the local owner rejects, do not show the object as changed; retain a readable fallback route. |
| Quest | Accept, update, prove, partially complete, or resolve a task. | Existing quest/progress owner. | Quest journal, objective, failure route, or completion feedback from its current read model. | No dialogue-local objective or duplicate completion flag. |
| Relationship | Apologize, make a defined promise, or deliver a supported personal action. | Existing relationship/character owner when it owns that concept. | Character response or relationship read model, within the current visibility model. | Do not infer a numeric delta from prose or create one relationship tracker per conversation. |
| Faction | Negotiate access, testify, pledge aid, or deliver faction evidence. | Existing faction standing, access, or quest owner, as source code establishes. | Access, service, dialogue availability, or a journal fact the owning systems support. | Cross-owner dependent effects are gated on accepted evidence; prose alone cannot confer rank or access. |
| World/location | Signal a route opening, alter a hazard, place a resource cache, or make an area unavailable. | Existing world/location/event owner. | Current map and expedition read models, plus scene descriptions. | No second map registry or dialogue-owned availability field; required quest reachability needs a declared fallback. |
| Campaign/ending | Ratify a settlement, expose a late-game truth, or supply a campaign resolution input. | Existing campaign/ending authority, if present and verified. | Explicit journal or later scene evidence matching the accepted fact. | A dramatic line is not an ending outcome; absent authority means provisional content and an unresolved decision. |

A response can have multiple scopes only if its contract says which scope is primary and which results depend on it. For example, presenting a signed record might first transfer an inventory item, then submit quest evidence, then make a character's follow-up line eligible. If the transfer succeeds but evidence submission does not, the content needs an honest partial state and a supported recovery route. It cannot imply that the quest owner accepted proof merely because the inventory operation worked.

Each cross-owner operation should specify an ordering graph rather than a prose phrase such as “also update the faction.” The graph contains only supported commands and event/read-model dependencies. It must be acyclic, finite, and auditable. A result event that causes the dialogue graph to offer a line is a read dependency; it is not a second command. If an owner publishes an accepted fact but a downstream refresh is delayed, the player may see a pending presentation state while the canonical fact remains accepted. The UI must not tell the player to repeat the action to repair a refresh delay.

### Worked vertical slice: the sealed transit ledger

This example demonstrates one evidence handoff with distinct command, result, and presentation steps. The record itself is fictional sample content, and its names are placeholders for authoring discussion. It does not add a canon location, character, faction, quest ID, or gameplay owner. The goal is to show how to write a maintainable slice that can be replaced by a verified real content package.

**Player-facing situation.** The player has found a sealed transit ledger during an expedition. A returning clerk recognizes its route marks but cannot tell whether the pages are complete. The available dialogue includes: ask what the marks mean; offer to submit the ledger for comparison; keep the ledger; or ask for the clerk's account of the last dispatch. The first and last responses are informational. Keeping the ledger is an informational/exit action. Submission is the only command-bearing response.

**Authored operation contract.** The submission response points at a quest or evidence owner that already accepts proof for the target objective. Its bounded parameter is the ledger evidence reference. The visible precondition requires the ledger to be available and the clerk to be present. The command owner rechecks evidence ownership and objective eligibility. The route does not set completion directly. If no current quest owner accepts this evidence type, the submission line remains disabled draft content and the plan raises a decision gap instead of adding a dialogue-only quest.

**Accepted result.** The owner records the evidence. The dialogue presents a restrained confirmation: “She turns the seal until its edge catches the light. ‘That is the copy I was afraid had gone missing.’” The current journal or objective read model reports what changed. A later exchange becomes available only when the owner publishes an eligible fact. The clerk's line and the quest state are independent surfaces that should be checked against the same accepted owner result.

**Already applied result.** If the owner says that this evidence was already submitted, the player sees the current state: “The comparison is already entered in the dispatch book. She leaves the ledger open to the mark you brought her.” No reward is repeated, and no new submission is sent. This wording is not a generic success fallback if the owner cannot verify prior application.

**Rejected result.** If the evidence does not match the objective, the owner returns a safe reason. The clerk can say, “The route is right, but this page ends before the seal.” If the player can find a missing leaf, the journal or dialogue supplies a supported clue. If the mismatch should remain hidden, the player receives a less revealing but still truthful response, and a more explicit clue becomes eligible after the associated discovery fact.

**Partial result.** If the owner accepts one proof fact but cannot accept a second dependent one, the accepted fact stays recorded. The clerk might say, “This confirms the south crossing. It does not tell us who signed the return.” The accepted portion appears in current objective state; the player is directed to the missing proof only if the objective supports that path. Do not withdraw the first proof because the second operation failed.

**Operational uncertainty.** If the result cannot be queried after submission, do not give either the success confirmation or the character's rejection. The safe panel says that the comparison cannot be confirmed and that the ledger should not be submitted again until its current status is checked. Re-entry queries the canonical owner. If that owner cannot reconcile the request, integration is blocked for this command-bearing route.

**No package or feature route.** If this narrative package is not installed in a supported configuration, the baseline content does not refer to a missing clerk or a ghost objective. If the player can acquire the ledger in the base game, it must retain a valid ordinary use, sale, storage, or descriptive route through existing owners. Optional expansion content cannot silently consume the only solution to a core quest.

This slice includes the minimum evidence for a route review: authored option; owner command; bounded input; current-state checks; result set; dependency order; success/partial/rejection/unknown copy; observable read model; idempotent revisit; panel-close behavior; and optional-package behavior. It can be implemented through an existing evidence submission path without a dialogue consequence engine. If any of those current seams are absent, the integration checklist should identify the precise gap and its smallest owner rather than generalizing the entire dialogue architecture.

### Bounded parameters and content validation rules

A typed request boundary makes content safer to extend and easier to validate. It also limits the ways that two similar-looking responses can produce inconsistent outcomes. The following validation categories should be present in a future validator or existing catalog integrity checks, using whatever error model the project already owns:

- The action reference resolves to a supported command or known informational response. Unknown operations are invalid content, not a no-op that appears successful.
- Each command identifies one canonical primary owner. A second owner is listed as an explicit dependent route, with an ordering and partial-result policy.
- Each parameter matches the operation's declared type, range, identifier grammar, and maximum collection size. Empty, oversized, duplicate, or unknown values have a deterministic rejection at validation or command time.
- Each condition references a supported fact provider and uses a supported comparison. Authors cannot read arbitrary object members, call game APIs, inspect files, or evaluate executable expressions from content.
- Each command-bearing response names accepted and rejected presentation routes, plus any possible partial, deferred, duplicate, stale, or unknown route that its owner contract can produce.
- Every presentation key exists for every supported locale or has a validated fallback. Missing translation cannot change the selected operation.
- Every referenced quest, actor, item, location, faction, or campaign fact resolves in the applicable content package. Optional-package dependencies are declared and cannot turn a missing reference into a valid empty result.
- A response is either informational or command-bearing. The validator flags ambiguous entries where prose says “I hand over” but no transfer command is defined, or where a supposedly informational line has a non-empty mutation payload.
- A response cannot apply the same effect twice through two aliases or both a direct command and a generic callback. Owner-level duplicate behavior remains an additional safeguard, not a substitute for clear content.
- A command with an ending-level scope declares its verified campaign consumer. If no consumer exists, a provisional narrative label is permitted for planning but not production acceptance.

These checks should report a stable content path and actionable error, not dump all game state. For example, a validator can say that a specific response uses a parameter outside the accepted range or lacks an unknown-result presentation key. It should not print player inventories, credentials, or full serialized saves to explain the defect. Diagnostics belong to the content/validation workflow and do not become player-facing dialogue.

### Safe composition for responses with multiple effects

Compound player actions are tempting because a single line can appear to advance a quest, change a relationship, grant access, and open a location. The production plan should split that dramatic sentence into explicit accepted facts and let current owners determine what each fact enables. This does not require separate player clicks for every operation, but it does require an authored dependency contract that explains the transactional expectation.

Before approving a compound response, answer these questions in order:

1. What is the player's intended commitment? If two independent commitments are hidden in one response, split the choice or make the combined offer clear.
2. What is the first canonical fact? Identify its owner and accepted result. Costs, item removal, and proof acceptance have different failure consequences and should not be represented as one vague success.
3. Which downstream facts depend on it? State whether each is mandatory, optional, or merely a presentation refresh. An optional follow-up should not roll back the first fact.
4. Which owner can reject each step, and what partial state is valid? If partial success would leave the player with an unrecoverable cost or contradictory quest state, redesign the route or use an existing atomic owner command.
5. How does a retry identify the same logical action? The owner’s idempotency contract must prevent accidental duplication. The UI's disabled button is useful feedback but not durable protection.
6. What should the player see after a partial or unknown result? The copy must not overstate acceptance or ask the player to repeat an operation whose result is uncertain.
7. What can be inspected later? The journal, inventory, relationship read model, faction access, or map should report its own current fact. If the only evidence is that a past dialogue line played, the consequence may not be observable enough for the promise made.

When several owners need simultaneous atomicity and none currently provides it, record the architecture decision and keep the content route out of production. Do not mimic a transaction by issuing sequential calls in a dialogue panel and attempting compensating writes on failure. Compensation can be domain-specific, can itself fail, and can mislead save/restore logic. A proper owner seam may accept one command and publish a result after its own supported transaction, but that solution must be grounded in source evidence and approved through current integration governance.

For optional consequences, use a degraded but truthful result. An optional portrait animation may fail while a quest fact remains accepted; the scene should continue with standard presentation. An optional expansion conversation may be unavailable while the base quest result remains valid. Conversely, a required cost or required access change cannot be treated as optional just because its presentation callback is unavailable. The authored dependency graph distinguishes gameplay acceptance from presentation polish.

### Localization, accessibility, and player comprehension

Result copy is part of the command contract because misunderstanding a request can cause the player to make a false commitment. Each consequential option should tell the player whether it asks a question, offers an item, spends a resource, starts a timed obligation, submits evidence, promises an outcome, or closes another opportunity. The player should not need to infer a hidden mechanical cost from a character's ambiguous line.

The UI should label pending and result states in concise language and maintain focus when the response arrives. A result should not rely only on color, sound, animation, or portrait motion. The actionable choice should support keyboard/controller activation and a safe cancel/back action before submission. After selection, feedback should make clear whether input was accepted, still pending, or rejected as stale. These are presentation requirements for the current UI owner; they do not justify a separate dialogue-specific input system.

Localization review should inspect semantic equivalence, not just placeholder substitution. Translators need to know whether a line is an offer, a promise, a conditional result, or a failure message, and whether it is used for accepted versus already-applied outcomes. A short context note can explain the speaker's knowledge and emotional register. Do not combine an operational uncertainty string with an in-world character line if translation could make it sound like a refusal. Do not rely on English punctuation or word order to encode state transitions.

Long prose can remain in the journal or an inspectable transcript while immediate result feedback stays brief. This permits players to confirm consequences without blocking the scene. Accessible text should preserve the key information: what was accepted, what remains, and what the player can do next. The log should identify the current source in player terms; internal request identities and schema errors stay out of the ordinary interface.

### Save, restore, and replay acceptance for a routed consequence

Persistence evidence must follow the current owner of each fact. A dialogue request does not itself require a new save section. For each accepted state, identify the owner’s existing capture/restore responsibility, then verify that the dialogue read path reconstructs a truthful scene from that restored state. If a new owner field or save section appears necessary, this proposal is insufficient authority to add it; the plan should point to the owner-level design decision needed first.

The following scenarios form a focused acceptance trace for one consequential option. They are future integration checks, not a request to add speculative test fixtures in this documentation pass:

| Scenario | Setup and action | Required state after reload/re-entry |
|---|---|---|
| First accepted submission | Submit valid evidence once; capture and restore through the current save route. | Evidence remains accepted once, objective state matches, and the accepted dialogue route is available. |
| Duplicate selection | Dispatch the same logical request twice under the owner contract. | There is one canonical effect; dialogue reports current state without a second cost or reward. |
| Panel closes while pending | Submit, close the panel, then query after owner resolution. | Closing did not cancel or duplicate the owner command; return state is reconstructed from the owner. |
| Owner rejection | Submit a validly formed request that fails a current gameplay precondition. | No unsupported mutation is present; safe reason copy and retry/alternate route match current conditions. |
| Dependent owner rejection | First supported fact succeeds, a declared dependent fact cannot proceed. | First fact remains, dependent effect is absent, and partial-state copy explains the current route. |
| Unknown operational result | Simulate an interruption at the existing owner boundary, then use its supported reconciliation path. | Dialogue does not invent acceptance or rejection and does not issue a new materially different action. |
| Old content or package absent | Load a campaign where the optional action is unavailable. | Baseline state and core progression remain valid; no missing node is needed to continue. |
| Locale fallback | Load a supported locale with a missing optional presentation variant. | Safe fallback text appears; canonical result and selected response remain unchanged. |
| Stale scene selection | Trigger a response after the active interaction or target has changed. | No action is redirected to a different target; refreshed options use current context. |
| Visit after quest terminality | Complete, fail, expire, or resolve the relevant task before returning. | The graph presents the matching aftermath or alternate route and cannot resubmit terminal proof. |

Determinism review applies when an owner makes seeded choices or emits ordered effects. Dialogue routing itself should not introduce nondeterministic gameplay by depending on hash iteration order, wall-clock time, or unseeded randomness. A cosmetic variant may use an existing deterministic selection contract if one exists; it must not change eligibility or the canonical outcome. If narrative variety is desired and no supported deterministic selector exists, author a stable sequence or defer the random variation rather than inventing a second RNG.

### Review worksheet for authors and integrators

A reviewer should be able to follow the selected response from authored text through canonical state and back without guessing. The following worksheet is intentionally short enough to attach to an individual route but detailed enough to reveal hidden coupling:

| Review prompt | Acceptable evidence |
|---|---|
| What does the player believe they are choosing? | Option wording states commitment, cost, and key condition in clear player language. |
| Is this information or a command? | One unambiguous response classification; informational text has no mutation payload. |
| Which system owns the requested fact? | Current public command/result path and its source location are cited during implementation review. |
| Which values are parameters and who bounds them? | Explicit identifiers and ranges validated by the existing contract. |
| What can change between display and submission? | Each relevant current precondition is rechecked by its owner. |
| What results can that owner return? | Result coverage matches accepted, duplicate, rejected, partial, deferred, stale, and uncertain outcomes that apply. |
| Are effects ordered or dependent? | A finite dependency graph and defined behavior for every rejected prerequisite. |
| Where does the player observe the result? | Existing journal, map, inventory, character, faction, or campaign read model is named. |
| What happens when the player returns? | The current graph queries the owner and routes to accepted aftermath, retry, alternate, or closure. |
| What does restore prove? | A focused current save path reconstructs the same canonical fact and truthful presentation. |
| What is the package-off route? | Core play remains coherent and required progression has a valid path. |
| Which review is proportional to the scope? | Presentation, owner, cross-owner, or campaign review matches the effect's severity band. |

The integrator records unresolved answers as explicit blockers, each assigned to the actual architecture or content owner. “Needs better dialogue” is not precise enough if the real problem is that the faction owner exposes no supported access command. “Needs a new save flag” is not precise enough if the existing quest owner already persists accepted proof but the dialogue graph fails to query it. Accurate ownership lets the smallest seam be fixed without enlarging the dialogue plan into a competing game architecture.

### Production slicing and throughput boundaries

Do not author large sets of consequential options before the route contract has one passing vertical slice. Start with an informational conversation that proves graph traversal and a single command-bearing response with a single canonical owner. Add one bounded compound action only after its owner dependency is proven. Then extend to a cross-owner route and a late-game/campaign route when their real owners and integrations exist. This sequence controls production risk while still allowing writers to draft future prose as provisional material.

For each slice, count more than transcript rows. Production cost includes the authored nodes and variants; content references; localization; character voice review; map or journal observation; command wiring; result presentation; save/restore evidence; accessibility checks; balance; optional-package review; and branch regression. A ten-word command choice can have higher integration cost than a page of descriptive prose if it spends resources and changes faction access. Conversely, a long environmental transcript may be low risk if it is informational, stable, localized, and has no gameplay side effect.

Estimate a route using the current project's real workflow rather than an invented universal person-day. Track the following in a content ticket: authoring effort, engineering owner seam, data validation, narrative continuity review, UX/localization review, focused verification, and follow-up ownership. Record actual estimates after the first integrated slices. Use that evidence to decide whether a package belongs in core or an expansion. A feature should not be labeled “cheap” solely because its data file is small.

When content volume grows, group review by owner route and consequence scope. This allows one reviewer to verify that all evidence submissions use the same owner semantics, another to examine the character voice, and a third to confirm map/journal aftermath. Preserve an independent review for rare campaign-critical responses; merging all cases into a single table-driven test can hide the exact ordering and persistence contract that matters. Existing project test policy governs any future test selection.

### Decision log and change control

Before implementation, the plan owner should append a compact decision record to the relevant integration package. The record should state: verified owner and API; exact command/result shape; data schema locations; response to each current result kind; save owner; optional-package behavior; accepted partial states; visible read model; focused verification target; and paths claimed under the live worktree authority. This expansion plan itself does not claim those paths and does not supersede the current integration queue.

If code inspection reveals that a named owner or result does not exist, revise the proposal around the current system or stop at that boundary. Do not infer an API from a class name in an older plan, an index entry, or a test that was quarantined. If the new route overlaps a live owner claim or requires an architecture decision, present the seam and evidence to the foreman/user under current governance. If the proposal is merely too large, reduce it to one traceable slice while retaining the future catalog model in this document.

A content change that alters the meaning of an accepted player commitment requires both narrative and contract review. Copy can be revised freely when the consequence remains the same and the current localization process permits it. Changing cost, eligibility, retry, persistence, or owner ordering is a gameplay contract change, even if only one line changes. Update affected acceptance traces and indexes after any future implementation rather than treating the prose file as proof that runtime behavior exists.

### Pass 11 closeout criteria

This pass's design contribution is complete when the six plans' shared boundaries can be reviewed together: quests describe proof and result ownership; locations distinguish authored anchors from expedition appearances; content packaging resolves references without duplicate authority; the witness-story packet remains provisional and follows owner results; dialogue gates read current facts; and this plan maps dialogue choices to typed owner outcomes and observable state. The current pass supplies a request envelope, result vocabulary, scope-to-owner matrix, worked evidence slice, validation checklist, compound-action review, player comprehension rules, restore trace, and author/integrator worksheet.

It does not establish a final serialized schema, new runtime service, new save store, new faction or ending authority, or production canon. Those decisions require current source and queue evidence. The concrete next implementation step is to choose one existing quest/evidence owner, verify its actual public command and result contract, author a single response against it, claim only the necessary files under the active ownership record, then evaluate the focused acceptance trace. Further prose and content packages can be added once that slice demonstrates the route end to end.
## Continuation pass 12 — Tidemark settlement decision and late-game consequence rehearsal

This pass supplies a second, distinct case study for consequence routing: a late-game decision about whether a regional coalition should publish a shared measurement procedure for water infrastructure. It builds on the Switchback Sluice scenes from Plan 20, but the decision is not “who gets the water,” and no dialogue response directly opens a gate, transfers a resource, or changes shelter supply. The question is whether the player will endorse, qualify, delay, or decline a proposed public standard when the evidence is incomplete and local people have not agreed to the same terms.

The High Meridian Assembly is a provisional late-game major faction: a network that connects distant shelters through route schedules, shared maintenance methods, and public reports. The Lowwater Stewards remain a local operator group with their own obligations and knowledge. The Assembly can invite a regional discussion; it cannot be assumed to command local infrastructure. The characters from the prior prose packet keep their perspective and limits: Sena is responsible for a mechanism she can touch, Edda for a standard that must be understood elsewhere, and the player decides what they are willing to publicly support.

All names and outcome labels remain draft. This case does not add a new faction state machine, diplomacy ledger, world-consequence registry, campaign-ending store, or save authority. Every candidate effect below must be expressed through a verified existing owner or held as provisional narrative content.

### Decision brief and player commitments

The scene appears only after the player has a supported accepted inspection/result and an eligible late-game offer. The player is told that a regional meeting will publish a statement. The statement may affect how other characters interpret the player's position, but only actual owner-backed outputs can change access, reputation, relationship, map availability, quest progression, or campaign resolution.

The player must understand four possible commitments:
- **Support a shared procedure:** endorse a common reporting method while explicitly saying that it does not certify the current sluice reading.
- **Support a local pause:** recommend that the Stewards finish the inspection before the Assembly asks for a comparison.
- **Request a limited trial:** permit a nonbinding comparison at one eligible site and review the result before making a regional statement.
- **Decline to endorse:** allow the Assembly to publish its own report while recording that the player has not agreed on behalf of local crews.

These responses are not four cosmetic tones of one predetermined outcome. Each can produce a distinct local conversation and quest result. Standing, access, long-term faction state, physical infrastructure, and ending effects remain conditional on what current owners can represent. A player should never be told that they spoke for the Stewards unless an authorized faction/quest owner says their role permits that representation.

Before the choice, the dialogue summarizes what is known: a clean upper instrument was read; the local casing moved; no water-quality result has been verified; and a shared procedure could make future reports easier to compare. The scene also states what remains unknown: whether the procedure will be adopted elsewhere, whether the next inspection will occur before the meeting deadline, and whether any local group will accept the Assembly's terms. This disclosure prevents a later faction callback from feeling like a consequence the player could not anticipate.

### Consequence sequence and owner boundaries

Treat the meeting as a small sequence of decisions with separate owner results. The authored graph may present a single clear choice, but it must not call several owners and imply an atomic all-or-nothing settlement. The sequence below is the review contract; exact commands are chosen only after current API inspection.

1. The dialogue submits the player's stated recommendation to the current quest/story owner, if it has a supported accepted-result command.
2. The quest/story owner returns accepted, duplicate, rejected, deferred, or unknown. The scene does not continue as though the recommendation has been recorded until that result is known.
3. If the accepted route requests a faction response, the current faction owner decides whether the player had standing to make the request and what access/standing result is supported.
4. Any change to a public notice, site availability, or map hint is requested through the current map/location/world owner. The response is not inferred from the faction line.
5. A campaign or ending consumer may later read only the accepted facts its current contract understands. If no such owner exists, the ending-level implication stays an authoring note rather than a saved outcome.
6. Dialogue, journal, map, and faction views refresh from their existing read models. A delayed presentation callback cannot undo a canonical accepted result.

If step 1 accepts a recommendation but step 3 rejects faction standing, the accepted quest/story record can remain while the faction effect is absent. The player-facing copy must distinguish “your recommendation was recorded” from “the Assembly adopted it.” If step 4 cannot publish a map notice, the group may still have discussed the proposal; the map remains unchanged and the line must not say that a route is open. Unknown results follow Plan 22's existing reconciliation contract and do not become narrative refusals.

The smallest viable route is one owner: the quest/story owner accepts a local recommendation and the journal shows it. The faction/region/campaign layers are optional expansion layers added only when their current owners are verified. If the current quest owner cannot store or expose a supported recommendation, even the smallest command-bearing route is blocked; a dialogue panel must not persist an alternate version.

### Branch result matrix

| Player commitment | Immediate accepted fact candidate | Scene/journal acknowledgement | Dependent effect candidate | Safe fallback |
|---|---|---|---|---|
| Support shared procedure | Player endorses the method, with a clear caveat about this site's unresolved reading. | The journal says the player supported comparison, not that water was certified. | Faction owner may record a supported request for future standards; campaign owner may consume it only if it recognizes the fact. | Record only the quest/story conclusion or keep the statement local if faction support is unavailable. |
| Support local pause | Player asks for a new local inspection before regional adoption. | Sena acknowledges the request; Edda records that the proposal is delayed. | Quest owner may create a follow-up objective if supported; temporary inspection location can be requested from the selector. | A report-only or later appointment route remains; the core quest need not wait for the Assembly. |
| Request limited trial | Player supports one comparison at an eligible site, with no claim of regional adoption. | The journal names the trial as pending or accepted, depending on owner result. | Location owner may make a compatible visit eligible; evidence owner may accept its result later. | If no trial site is available, provide a clue/delay response or replace the request with a nonbinding report. |
| Decline to endorse | Player does not authorize their own name or role to be used as support. | The scene says the Assembly may continue its own process and that the player did not endorse it. | Faction owner may retain ordinary access; it must not invent punishment. | No mutation beyond a local/quest record if the current owner supports it; otherwise the scene simply closes. |
| Leave before choosing | No recommendation is accepted. | The player may return while the offer remains eligible. | Opportunity deadline may close only under an existing owner/time contract and prior disclosure. | The main quest retains its valid local conclusion; the player can later receive a closure line. |

An accepted recommendation is not the same as implementation. A faction member can hear a proposal without agreeing. The player can support a trial without choosing its outcome. A trial can be scheduled without a location visit appearing in the next expedition. The trial can happen without the result supporting either side's expectation. Each of those distinctions matters to the quest log and to later dialogue.

### Late-game campaign callbacks without a new ending ledger

The proposal can support late-game writing while avoiding a new store for faction/campaign state. Callback candidates are conditional prose examples, not promises:

- If a current faction owner confirms that the Assembly adopted a shared procedure, Edda can later say that another crew used the same labels to challenge an outdated report. The line names a practical change but does not claim every settlement agreed.
- If the local pause was accepted and a verified local inspection occurred, Sena can mention what the repair crew measured. Use only the owner-backed inspection result; do not invent a safe water reading.
- If the player requested a trial but no candidate site appeared, the journal can say “trial not scheduled” and Edda can offer a later route if the owner confirms one.
- If the player declined endorsement, a later scene can acknowledge the boundary without portraying the player as hostile: “You left your name off the statement. We published the measurements and marked the decision open.”
- If the Assembly package is disabled, the base game concludes through the local result. It does not mention a missing coalition as a failed dependency.
- If an old save has an accepted local quest result but no late-game package, the load path preserves the local result and leaves optional campaign copy unavailable. No dialogue loader invents a neutral faction flag.

An ending may consume a supported campaign fact if the existing campaign authority accepts it. That consumer must document whether it distinguishes endorsement, adoption, trial, or mere discussion. If the owner supports only one coarse fact, write an ending line that matches that limited meaning; do not pretend the game stores four distinct outcomes. If no relevant campaign consumer exists, the late-game outcome is a quest/faction callback and not an ending variable.

### Conflict cases and interpretation policy

Cross-owner disagreement should be represented as a known conflict, not settled by whichever panel refreshed last.

| Current owner information | Dialogue situation | Correct response |
|---|---|---|
| Quest owner says recommendation accepted; faction owner says player lacks authority to bind the group. | Player endorsed a method but cannot speak for the Stewards. | Preserve the accepted personal recommendation, explain it was not group adoption, and do not apply faction access or standing. |
| Faction owner says meeting is open; location owner reports the meeting site unreachable. | The Assembly still offers a discussion, but no visit can happen now. | Keep the opportunity visible as delayed or remote only if a supported route exists; do not spawn a false location marker. |
| Location owner says a trial site was selected; quest owner says trial precondition is not satisfied. | Candidate exists, but the quest cannot accept it as proof. | Treat it as optional exploration or remove it from the trial route; never mark the objective complete from presence alone. |
| Campaign owner has no field for the recommendation. | Dialogue requests an ending callback. | Keep the branch local or defer the campaign prose; do not add a shadow ending field. |
| Optional package is absent but old read model contains an unknown faction reference. | Returning player loads a partial profile. | Preserve core facts; hide or neutralize optional text according to the package contract and record diagnostics outside player copy. |
| One owner returns unknown after a previous request may have committed. | Player returns to the meeting scene. | Query the canonical owner through its existing reconciliation path; show no success or refusal until the fact is known. |

The graph must not resolve contradictions by escalating to a more dramatic answer. A player-friendly interface can say that the local recommendation is recorded while the regional response remains pending. It can offer the journal or a safe exit. Do not write “the Assembly refuses you” when only the meeting location was unavailable, or “Sena agreed” when only a quest record accepted the player's text.

### Branch continuity through expedition and location systems

The limited trial branch connects to Plan 18 only as a request to make an eligible site available. The dialogue does not add a map location, choose a visit, or override active required destinations. The location selector applies its normal priority and fallback policy. If the trial competes with a mandatory quest site, the mandatory site retains its guarantee; the trial waits or uses a compatible alternative only when its proof meaning remains correct.

Before the player accepts a trial, the option should describe that it may require another expedition and that the site is not guaranteed to appear immediately unless the dispatch owner offers such a guarantee. If the selected site is only a reported opportunity, the map communicates that uncertainty. If the visit expires or cannot be selected, the quest owner receives no fabricated trial evidence. A location callback can still report that no trial was scheduled only if that result is backed by the current quest/story owner.

The map may show a public meeting location and a private site clue under different disclosure rules. A hidden location should not leak through a faction marker, quest title, accessible label, or selection order. When the package is off, its site IDs are not referenced by core quests. When enabled, the trial receives its own completion and failure route instead of piggybacking on the main inspection result.

### Player interface and result feedback

The choice screen should present commitment language in one sentence, with a short details view for uncertainty and follow-up. Put the commitment verb first: “Support a shared reporting method,” “Ask for another local inspection,” “Request a limited trial,” or “Decline to endorse.” Avoid “continue” and “agree” when they obscure whether the player is authorizing public use of their name.

Before dispatching a command, show any known cost, time window, location requirement, or nonbinding nature. If the action is command-bearing, the confirmation action uses the existing command path and offers a clear cancel route. Once submitted, disable only the duplicate input while pending; the scene can be safely closed. On accepted result, display what changed and what did not. On rejected result, provide the current owner-approved reason and next route. On unknown result, state that the outcome cannot yet be confirmed and make the journal/revisit path clear.

If an action has no gameplay effect, it should not be presented like a binding world decision. A roleplay line can still matter emotionally, but copy and UI must not imply a faction or campaign state. Conversely, consequential outcomes should not be hidden behind a color pulse or an unexplained variation in music. Provide readable text, transcript access, keyboard/controller focus restoration, and screen-reader labels that match the actual result.

The journal entry should distinguish:
- **Recommendation made:** player's statement recorded, no adoption claim.
- **Meeting outcome accepted:** current faction/story owner confirms a response.
- **Trial requested:** eligible follow-up requested, not yet scheduled.
- **Trial scheduled:** current location/quest owners expose a valid visit.
- **Trial completed:** accepted evidence is recorded by its owner.
- **Regional procedure adopted:** only if a current faction/campaign owner supports this precise outcome.
- **Unresolved:** no supported result or valid player choice exists yet.

These are labels for presentation when current owner facts permit them; they are not proposed new saved states. If the owner cannot distinguish a status, collapse the copy to the level it actually supports.

### Acceptance trace across packages and saves

Future integration should prove each authored branch against the current owners and their restore path. At minimum, review:

| Trace | Starting condition | Action or interruption | Required result |
|---|---|---|---|
| Shared method accepted locally | Valid quest/story offer and supported context. | Submit the recommendation. | Owner records exactly the available fact; dialogue says personal support, not universal adoption. |
| Faction adopts or rejects | Accepted recommendation; faction owner returns a typed decision. | Reopen meeting. | Callback and access match faction owner state; no inferred group consensus. |
| Limited trial waits | Trial accepted but no valid location candidate. | Start an expedition. | Mandatory quest locations remain possible; trial status is delayed with a clear reason. |
| Trial candidate appears | Trial is eligible and selector returns a compatible site. | Visit and submit evidence. | Location presence alone does not complete the quest; accepted proof does. |
| Player declines | Offer available. | Decline and close dialogue. | No unapproved penalty, cost, or hidden ending effect occurs. |
| Save after one owner accepts | Quest recommendation accepted; faction result remains pending. | Save, restore, then query. | Accepted fact remains; no duplicate submission; pending faction result remains honest. |
| Result unknown | A request may have committed. | Close panel and return. | Current owner reconciliation resolves it; dialogue does not send a fresh command. |
| Core-only load | Optional coalition disabled. | Reach the local conclusion and reopen the quest. | Core route is complete and readable, with no missing faction reference. |
| Late-game package removed | Optional prior save facts exist. | Load under current supported package behavior. | Existing canonical facts restore under current policy; absent content does not create a new outcome. |
| Locale changes | A response result is pending or already accepted. | Switch locale and revisit. | Current result remains; copy and accessibility labels preserve commitment meaning. |
| Conflicting map/quest result | Quest requests trial, map owner has no reachable site. | Open journal and map. | Both show the same delay/uncertainty boundary. |
| Ending consumer present | Existing campaign owner declares supported inputs. | Reach resolution. | Ending text consumes only those inputs and does not infer missing detail. |

Deterministic behavior applies if location selection or any owner route uses seeded variation. The authored choice outcome itself must not depend on wall clock, hash ordering, or unseeded randomness. A cosmetic line variant can use an existing deterministic selector only if its result never changes eligibility or consequence. An optional random location appearance must not decide whether a required campaign outcome is possible.

### Production sequencing and risk budget

The first integration slice should stop at the local recommendation stored by one verified quest/story owner. It needs one dialogue response, one typed result, one current journal read, and save/restore evidence through that owner. This proves whether the game can support a durable player-authored conclusion without creating a new state authority.

The second slice can add a faction result only after the faction owner and its read model are verified. The third can add the optional trial through current expedition and location owners. A campaign callback or ending interpretation is last, because it has the broadest dependency and content review surface. These are ordering recommendations for future implementation, not live path claims or authorization.

Estimate each layer separately:
- writing and translation of the four choice families and result variants;
- quest/story command and read-model mapping;
- faction availability, access, and result semantics;
- compatible location selection and required-location interactions;
- journal/map changes and accessibility;
- save/restore, duplicate/retry, partial failure, and unknown-result behavior;
- campaign/ending dependency review;
- old-save and package-off behavior;
- focused verification permitted by current test policy.

If the desired ending consequence cannot be represented by the current campaign authority, defer that layer. Do not compensate by writing multiple small faction flags into dialogue, quest content, or the generated visit. Keep a documented open decision with the exact consumer and evidence needed.

### Pass 12 closeout criteria

This decision packet is complete as a design proposal when the player can tell what they are endorsing, the local and regional outcomes remain distinct, each possible fact has one verified owner candidate, the limited-trial branch cannot bypass expedition guarantees, missing optional content preserves the core route, and save/re-entry behavior is explicit. It supplies a late-game faction scenario, branch outcomes, cross-owner conflict cases, interface language, an acceptance trace, and production sequencing while leaving the final schema and runtime architecture to the current project authority.

The next step is not to implement every branch at once. Verify the current quest/story owner and create one accepted local recommendation slice under the live integration queue. If the current owner cannot represent the result, record the seam and keep campaign-facing content provisional. The plan does not claim completion of the game feature, faction, expedition system, dialogue runtime, or ending pipeline.

### Short aftermath exchanges by owner result

These additional exchanges give the late-game decision a restrained human aftermath while keeping each line subordinate to current results.

**Quest/story owner accepts the player's limited recommendation; faction response pending.**<br>
Edda: “I can record that you asked for a comparison. I cannot call that an agreement.”<br>
Sena: “Then write both sentences.”<br>
Journal: “Comparison requested. No regional method adopted.”

**Faction owner confirms adoption after review.**<br>
Edda: “They accepted the headings and rejected the schedule. That is still more agreement than we had yesterday.”<br>
The line is eligible only if the faction owner exposes that exact partial result. If the owner supports only “adopted” or “not adopted,” the transcript must use the supported level of detail.

**The requested site never becomes a valid expedition candidate.**<br>
Sena: “No trial today. The gate does not owe us an answer because we asked for one.”<br>
Journal: “Trial not scheduled. Local inspection remains available through the accepted route.”<br>
These lines require a current quest/story result that confirms the delay and cannot be used for an operationally unknown request.

**Player declined endorsement.**<br>
Player: “I won't put the crews' names under a method they haven't accepted.”<br>
Edda: “Then I will publish the measurements as mine to carry. Your name stays off the page.”<br>
That response does not imply the Assembly accepts the player's argument, grants access, or records hostility. Only the relevant owner can support those outcomes.

**Re-entry with no new fact.**<br>
Sena: “The wheel is still here.”<br>
This neutral line keeps the scene human without manufacturing memory, progress, or disappointment. If a current owner later supplies a new result, the graph may replace the neutral route with a truthful callback.
## Continuation pass 13 — Farline correction routing, effect sequence, and player trust

The Farline storyline gives consequence routing a different stress case from Tidemark. Tidemark asks whether a regional group accepts a procedure; Farline asks how a stale route message can be qualified or corrected without claiming that a sound, a courier, a map, and a faction all mean the same thing. The proposal needs to express a chain of narrowly scoped outcomes. It does not authorize a new broadcast engine, world-state flag registry, or direct edits from dialogue into route/map data.

### Correction request stages

A player who reaches the late-game choice can support one of several actions: preserve the old message with a visible qualification, submit a local correction, ask the Compact for a network correction, request a limited trial, or decline to endorse a message. Each action is distinct from its delivery and acceptance.

**Stage A — record the player's scope.** The quest/story owner, if it supports this command, records that the player made a recommendation. The result means only “recommendation recorded.” It does not say that the Compact or Wardens accepted it.

**Stage B — ask the responsible group.** A verified faction/quest owner determines whether the player can request a local or network response. The owner may accept a review request, reject it, or defer it. A missing relationship or standing adapter is unknown, not automatically hostile.

**Stage C — route the correction.** If an existing message/radio/dispatch owner supports correction, it accepts a bounded source, target, and message meaning. It may return received, delayed, rejected, duplicate, or unknown according to its public contract. This plan does not invent a delivery confirmation.

**Stage D — update the visible source.** An existing location, map, journal, or notice owner publishes its own supported result. A journal can say that the player requested correction even if the network never received it. A map should show a changed route only after the travel/map owner says the route status changed.

**Stage E — later interpretation.** A current faction/campaign consumer may use the accepted facts to select an ending callback. It must not infer adoption from the player's initial recommendation or from a message being received.

Each stage's effect is bounded. A later stage can be optional only when the previous accepted result remains truthful and usable. If the package requires all-or-nothing publication but no current owner offers that transaction, reduce the action to one supported owner or defer it.

### Effect-by-effect routing matrix

| Intended player action | Primary owner candidate | Dependent owner, if any | What acceptance means | Partial/unknown behavior |
|---|---|---|---|---|
| Tell Yara the signal was heard | Quest/dialogue interaction owner, if a persistent result is required. | None for a purely informational line. | The conversation occurred; no signal origin is asserted. | If nothing durable is supported, keep the response local and do not store a new memory flag. |
| Submit relay plate inspection | Current quest/evidence owner. | Map/journal read models refresh from accepted fact. | The specific plate was inspected or its condition recorded. | Rejected proof leaves objective unchanged; unknown requires owner reconciliation. |
| Ask the Wardens to qualify a notice | Current faction/quest owner. | Notice/location owner if it supports the public wording. | The group accepted a local review request or note. | If note publication fails, the request may remain accepted locally while display is pending, if that state is supported. |
| Request Compact correction | Current faction/communication owner verified in source. | Current world/map/route owner for any route-state change. | The Compact received or adopted a defined correction, depending on exact owner result. | Receipt cannot be voiced as adoption; delayed/unknown results do not close the route. |
| Request limited trial | Quest/story owner. | Existing location/expedition selector and evidence owner. | A trial is eligible or requested, not necessarily scheduled or completed. | No candidate site means delayed request; no proof is accepted from selection alone. |
| Withdraw personal endorsement | Quest/story owner only if the choice itself is a durable action. | Faction/campaign owner only if it accepts a supported consequence. | The player no longer personally endorses the proposed wording. | Does not retract a group notice or message without that owner's accepted command. |
| Publish ending callback | Current campaign/ending authority, only if it exists. | None from dialogue directly. | A supported campaign input influences resolution. | If the authority cannot represent the outcome, omit the campaign claim. |

Names in the owner column are concepts to verify, not API assertions. The project may route a report through a quest owner and never have a separate communications owner. If so, the content should use that current architecture and not introduce a new owner because “message” sounds like its own subsystem.

### Outcome matrix for the principal choices

**Preserve and qualify.** The player asks to retain the old tone record while marking its route confirmation as outdated or absent. The immediate local outcome can be a supported notice entry. A communication owner may propagate it only if current systems support that. Fallback: the local journal carries the qualified result and the player is told the network outcome is unknown.

**Submit a local correction.** The player asks the Wardens to publish “heard here; route not confirmed.” The immediate result can be a local recommendation accepted by the quest/faction owner. The route itself remains unchanged. If the location owner cannot publish a notice, the scene states that the recommendation was recorded but the marker has not changed.

**Request a network correction.** The player asks the Farline Compact to send a scoped correction to the report's known recipients. The option is available only if the owner can identify eligible recipients or supports a broader bounded target. The player sees that submission does not guarantee receipt. If the Compact has no supported recipient model, this remains an editorial proposal and cannot ship as a fake global effect.

**Request a trial.** The player asks for a new inspection at a compatible route/site. The expedition selector considers it after mandatory destinations. A successful selection creates an opportunity to inspect, not an accepted result. If no compatible site can appear, the owner can defer the request, replace it with a clue, or allow a report-only closure.

**Decline endorsement.** The player refuses to let their name be attributed to the correction. This should not block someone else's report unless a current owner says the player's role is actually required. The player receives a clear copy that they did not endorse; the game does not infer hostility or loss of faction standing.

**Withdraw after acceptance.** If the player later asks to withdraw a recommendation, verify whether the request is reversible and whether it has already been distributed. A local recommendation may be amendable. A message already received by other characters may require a correction rather than erasure. If no owner supports recall, dialogue cannot claim that the information disappeared from every recipient.

### Exact player promise and effect budget

A branch contract should use a phrase whose scope matches the result:
- **“I heard it”** records a personal observation only.
- **“We inspected the plate”** is valid only for an accepted inspection with the relevant actor/source.
- **“The Wardens posted a local qualification”** requires the appropriate local result and visible notice.
- **“The Compact received the correction”** requires a receipt result.
- **“The Compact adopted the correction”** requires an adoption result distinct from receipt.
- **“The route is open”** requires current route-owner status.
- **“Everyone was warned”** requires a supported coverage/recipient result; absent that, never use it.
- **“The old tone was false”** is too broad unless the accepted owner fact defines that exact conclusion.

The effect budget grows with reach:
- A local dialogue acknowledgement touches one scene and has low cost.
- A quest result touches journal/lifecycle and requires persistence and duplicate review.
- A local notice may change the map/read model and requires refresh and accessibility review.
- A network correction may affect several sites or characters and requires recipient/partial delivery semantics.
- A faction adoption may affect access, standing, and later content and requires the faction owner.
- An ending callback has campaign-wide implications and requires an explicit ending consumer.

If the present game supports only a local quest result, author the smallest true wording at that level. Do not widen scope because later scenes would be more dramatic.

### Branch dependency graph and failure cut points

The main correction route can be represented as:

report encounter → accepted observation → player recommendation → owner acceptance → optional faction/communication response → optional map/notice update → later callback.

At each edge, reviewers identify its type:
- a content reference links one authored record to another;
- a read evaluates current owner facts;
- a command requests a durable/domain action;
- an event announces an accepted fact;
- a projection refresh changes presentation only.

No edge is implied by adjacency in a dialogue file. If a message is accepted but a downstream map refresh fails, the message remains accepted if its owner says so; the map stays truthful or temporarily stale under current refresh policy. If the map changes but a later callback is unavailable, the player still sees the current route state. A cosmetic cue failure does not roll back canonical data.

A failure cut point is a place where the route can safely stop without leaving a false claim:
- before recommendation: no durable decision;
- recommendation accepted but group request rejected: personal report stands, group result absent;
- group request accepted but route unavailable: request is pending/delayed, map unchanged;
- message receipt unknown: do not resend a materially different request until reconciled;
- map update delayed: journal says route status pending only if the owner supplies that state;
- optional callback absent: current canonical result remains available elsewhere.

If no safe cut point exists, the proposal needs a single current owner operation with its own transaction/atomicity semantics or a smaller player choice. Dialogue-level multi-call compensation is not acceptable.

### Stale and repeated action scenarios

When the same player returns, the graph reconstructs its options from current results. A previously accepted correction command is not selectable as new work unless an owner supports amendment. A duplicate tap or repeated command returns the owner's existing fact. An expired trial cannot be restarted by selecting an old response; a fresh offer must be created by the current quest owner. An operationally unknown submission keeps the response neutral until the owner reconciles.

A message may have become outdated since the player last saw it. The scene should tell the player that it is now superseded only when a source confirms that update. If the signal event happened before a save and the story owner retains the result, the dialogue can use that fact after restore. If it is merely a past ephemeral sound with no persistent owner fact, do not promise a remembered playback after loading.

A stale selection payload also needs protection. Between rendering and command submission, the player may change location, switch active quest context, or receive a new route status. The owner revalidates. The host can refresh the panel and ask the player to choose again; it cannot redirect the action to another route target.

### Player-facing result copy

| Result | Compact response | Required implication |
|---|---|---|
| Local inspection accepted | “The plate is inspected. Its date is unreadable.” | Inspection happened; emission time remains unknown. |
| Local note accepted, publication pending | “The Wardens recorded the qualification. It is not posted yet.” | Do not claim that others received it. |
| Compact receipt confirmed | “The Compact received the correction request.” | Receipt only; adoption remains open. |
| Adoption confirmed | “The Compact accepted the new wording for its notices.” | Scope only to Compact notices; no route status implied. |
| Trial delayed | “No compatible site is available this expedition. The request remains open.” | Valid only if the owner supports an open request. |
| Trial unavailable/expired | “The inspection window closed. A report-only conclusion remains.” | This is an in-world timing result, not an operational failure. |
| Owner rejected request | “The current route owner cannot publish that change.” | State a safe actionable reason if allowed; no blame assigned to character. |
| Result unknown | “The request cannot be confirmed yet. Check its status before sending another.” | No success, refusal, cost, or narrative failure claim. |
| Duplicate request | “That correction is already recorded.” | No repeated effects/rewards. |
| Optional feature absent | Hide the option or show a truthful unavailable route per profile contract. | Core quest remains valid. |

The copy remains concise in the immediate scene. A transcript or journal can carry fuller context. Accessibility labels use the same result meaning and do not announce hidden recipient or adoption facts before their disclosure gate.

### Integration sequencing and closeout gate

Implementation order should reduce the number of owners in the first slice:
1. Verify one existing quest/evidence owner can record the signal plate observation.
2. Prove the accepted result appears accurately in the journal and survives its current save path.
3. Add a local recommendation through the same or another verified current owner.
4. Add a faction response only when current faction API/result semantics are established.
5. Add message propagation only if the current game exposes a communications/dispatch owner that supports the required scope.
6. Add location/map changes only through the current location/travel/map owners.
7. Add any campaign callback last, after the consumer and its old-save behavior are verified.

Each phase needs exact file ownership from the live ledger, focused verification under the current test policy, and an observable route. No phase is authorized by this plan alone. A missing owner ends the dependency chain at that boundary and triggers the current architecture decision process.

The proposal closes when every player-visible statement has an owner-backed meaning or is explicitly a character belief; every consequential command distinguishes request, receipt, adoption, and route status; optional features preserve a core route; no retry can duplicate a durable effect; and accepted facts remain truthful after restore and scene re-entry. The Farline story then provides a high-value use case for the existing command/result architecture without requiring a second messaging or consequence authority.


## Continuation pass 13 — consequence routing rehearsal, branch closure, and long-term callbacks

### Consequence routing contract

A player choice matters only if the game can apply its result, preserve what must persist, and later show an outcome consistent with that result. The Farline message decision is an end-to-end rehearsal for that contract. It begins as a choice of wording and audience, passes through the current quest or dialogue owner, reaches the appropriate existing faction, location, or campaign owner, survives a save when needed, and returns to the player through a visible notice or conversation.

This plan does not authorize a generic effect bus, new world-state store, or additional campaign flag registry. Each effect must be routed through the existing owner for the concern. If an effect has no owner, reduce the promise to a local scene consequence or request an explicit architecture decision. One scene should not write directly into several mutable stores. The owning quest or host route should emit the supported fact or command, and current adapters should perform the established persistence and presentation work.

### Effect envelope

For every meaningful player response, write a conceptual effect envelope containing:

- the originating dialogue choice or quest action;
- the exact fact the player established;
- the current owner responsible for the fact;
- the scope of the outcome;
- whether it is immediate or delayed;
- whether it persists through save/load;
- the visible acknowledgement;
- any deduplication or replay rule;
- the fallback when the receiver is unavailable;
- the point where the effect can be considered resolved.

This envelope makes effect scope explicit. “Warn the route” is a player intention. The concrete result might be that the current notice changes, a courier receives the information, or a later expedition gets a safer route hint. These are different effects and should not be collapsed into an unowned boolean.

### Consequence scopes in practice

**Cosmetic scope.** The wording changes but no state changes. If the player chooses “I cannot confirm the source,” the immediate line can acknowledge that caution. Do not label it as a lasting consequence.

**Local scene scope.** The notice rail displays the chosen wording for the current scene. If local scene state is not persisted, its duration must end with the scene and must not be referenced later as history.

**Quest scope.** The investigation resolves as confirmed mechanical source, plausible reconstruction, unverified report, or unresolved record. The existing quest owner records the terminal outcome if its contract supports that distinction.

**Relationship scope.** Yara or Pell reacts to the player’s method. Use the existing relationship authority only if it captures this kind of change. If not, write an authored response without implying a numerical or permanent relationship shift.

**Faction scope.** A courier network adopts, delays, or refuses the notice. This is a faction consequence only when a current faction owner represents the change. A single courier’s local decision may be described as character or local-scene scope instead.

**World scope.** A route, site, resource availability, or later event changes. This requires the current world/location/campaign owner to expose a supported transition. Dialogue text is not proof of a world change.

**Ending scope.** A major resolution references the player’s communication policy, but only if the campaign’s existing ending logic can consume the fact. Otherwise, the choice remains a quest consequence with a later character callback. Do not promote a local branch into an ending dependency merely to make it sound important.

### Routing table for Farline choices

| Player action | Immediate result | Candidate owner | Persistent proof | Callback |
|---|---|---|---|---|
| Post a sourced mechanical explanation | Notice includes source and uncertainty boundary | Existing quest/notice route | Resolved objective outcome, if supported | Courier asks for the maintenance reference |
| Post a safety warning without attribution | Travelers receive a caution with no asserted cause | Existing notice or faction message route | Notice treatment or equivalent fact | A traveler reports choosing a longer road |
| Post a comparison request | The route network seeks another record | Existing quest progression | Follow-up task becomes eligible | New card arrives with contradictory interval |
| Keep the report private | No public message is sent | Quest outcome and local scene | Private/withheld outcome if supported | Yara checks whether the record remains available |
| Destroy or lose the copy | One proof route closes | Current objective/failure route | Failure-forward state or explicit closure | Another source can reconstruct part of the evidence |
| Decline to decide | Quest remains blocked or closes as unresolved | Existing quest owner | Clear terminal or blocked state | No false callback assumes a public notice |

The named owners are candidates, not verified APIs. An implementation plan must replace each candidate with the exact current owner and its route. If no supported notice authority exists, the effect can remain within the quest and be reflected by dialogue. A new public-message system is not justified by this table alone.

### Order of operations

A consistent sequence prevents partial side effects:

1. Validate that the response was available in the current dialogue context.
2. Apply the choice through the established quest/dialogue interaction path.
3. Update the primary owning state once.
4. Emit or derive any dependent presentation facts through existing adapters.
5. Persist through the current save owner if the fact must survive.
6. Recompute eligibility for follow-up content from the authoritative state.
7. Refresh the visible journal, map, or notice after the authoritative result exists.
8. Record enough diagnostic context to explain a failed route without exposing secrets.
9. Confirm that repeated interaction does not apply the same consequence twice.

The exact technical order depends on the current architecture. This conceptual order exists to identify boundaries. If the current host applies dialogue effects before the quest accepts the interaction, write a narrowly scoped integration proposal; do not rearrange shared seams from this content plan. If a host callback can fail after the quest state changes, the existing pattern must define whether the event retries, is safely idempotent, or surfaces a recoverable error.

### Idempotency and duplicate delivery

A consequence should not duplicate when a player repeats an interaction, reloads after a save point, revisits a hub, or receives the same notice through two eligible routes. This is especially important for rewards and faction communications. The current objective completion should be the authoritative source for whether a reward was granted. A dialogue line that says “the note has been sent” cannot be used as a second independent send trigger.

For each effect, classify replay behavior:

- **Once-only:** the story event occurs once; further interactions show its settled state.
- **Replace-current:** a notice can be edited, and the current version replaces the previous one if the owner supports that history.
- **Append-once:** a report is added once using an existing collection owner and stable identity.
- **Repeatable without state growth:** ambient dialogue can recur without accumulating duplicate consequences.
- **Retryable delivery:** if delivery failed, the effect can be retried without granting duplicate rewards or changing the chosen wording.
- **Expired:** the effect is no longer actionable, and the player receives an explicit closure.

Avoid solving idempotency with a new set of shadow flags. Use the current domain identity and completion facts. If the existing state cannot tell whether the effect was applied, that is a specific architecture gap for the owner to assess.

### Failure cuts and recovery behavior

A routing rehearsal should cut the path at each boundary and define a safe response.

**Choice accepted, receiver unavailable.** The quest records the chosen treatment; the notice or courier delivery can wait if its owner supports deferred delivery. The player should see “prepared” or another truthful status. Do not report successful delivery.

**Receiver disappears before callback.** Use an authored alternate carrier, a board notice, or a closure line that reports the missed opportunity. The alternate route must preserve the same essential information. If no route was authored, close the optional callback instead of inventing a voice for the absent character.

**Save occurs after choice but before presentation refresh.** On restore, recompute the visible result from the authoritative choice outcome. Do not replay the choice or apply its effect again.

**Location is unavailable during the effect.** Queue only through an existing event/persistence mechanism. If no such mechanism exists, keep the consequence local or delay quest completion until an available route exists. A dialogue graph should not be an event queue.

**Quest is abandoned.** Clarify whether the choice remains effective. If the player posted the warning and then abandoned the follow-up, the warning should not vanish. If the choice existed only as a proposal and had not been delivered, abandonment may cancel it.

**The player chooses no action.** Keep the conversation available or permit an explicit unresolved closure. Do not auto-select the default branch when leaving the scene unless current interaction semantics clearly require and signal that behavior.

**Conflicting effects are requested.** If another quest has posted an incompatible notice, define whether the player edits, appends, or withdraws it. A last-writer-wins rule may be simple but should not accidentally erase another character’s authored message. If the current notice owner cannot represent conflict, delay the second choice or keep both as separate scene outcomes without claiming a single shared rail.

### Delayed callback schedule

Callbacks should be tied to meaningful story milestones rather than arbitrary real time. Candidate triggers include the next completed expedition, a later visit to Half-Span Shelter, a faction meeting, or a specific campaign chapter. Use whichever conditions the current quest/world owners support. Avoid a hard-coded number of days if the player may remain in the shelter or enter a long expedition loop; that can make the callback appear before its causal event.

A callback record states: earliest eligibility, latest useful eligibility, delivery surface, character-availability fallback, whether it can be missed, and whether it is required for closure. Main-story proof should not depend on an optional callback. A delayed line should be suppressed if its fact has been superseded. For example, if a later expedition established the signal’s source conclusively, an earlier “we still do not know” callback should become a correction or be skipped.

The callback should add one new perspective, not restate the entire outcome. The courier may say, “We sent the warning without the source line.” That establishes the practical choice. A traveler might mention that they waited for daylight instead of crossing at dusk. This is an observed result, not proof that everyone followed the notice. Avoid claiming global behavior from one report.

### Reconciliation of branch outcomes

Four outcome bands make the story’s resolution readable without requiring a separate content universe for each:

- **Established source:** direct evidence supports a mechanical source, but does not prove who altered a copy.
- **Supported reconstruction:** multiple sources support a likely route or process, with a named unresolved point.
- **Unverified warning:** the player chose precautionary communication without stating an origin.
- **Unresolved record:** the player preserved the material or chose not to communicate while acknowledging missing evidence.

These are editorial bands. Actual implementation may encode them through existing quest stages or result IDs. A branch should not derive one band from another by line text alone. The later game can summarize these outcomes through one or two lines and preserve the distinction in any authored content that genuinely depends on it.

When two outcomes lead to the same future event, they can reconverge with a line that acknowledges the different reasoning. When they cause materially different access or trust, they must remain distinguishable in current state. Do not merge outcomes simply to reduce implementation work if the player was promised a meaningful result. Conversely, avoid carrying a distinction into every later scene if it no longer affects the player’s experience.

### Player trust and consequence clarity

The player should understand what kind of consequence they are choosing. Before posting, the UI or dialogue should communicate the immediate audience and confidence: public warning, comparison request, sourced report, or private record. It need not disclose every downstream reaction. The choice remains uncertain, but its scope is legible.

Trust can be damaged by three specific failures: the game promises a route change that does not occur; an NPC attributes a position to the player that they did not select; or a save/load changes which response was chosen. It can also be damaged if the player is punished for an uncertainty they were never shown. Review the wording immediately before and after the choice for this reason. “Warn the travelers that the road may be unsafe” is clearer than “send the message.”

A character may disagree with the player without treating their choice as a moral failure. If Yara preferred an exact record and the player chooses a short warning, she can say that the source detail was lost in the summary. Pell can say the warning reached people sooner. Both observations can be true. The story should not reward the author’s preferred answer through every voice.

### Multi-owner integration map

Before implementation, identify a single route for each seam:

- dialogue selection and response delivery;
- quest objective/result ownership;
- location discovery and map visibility;
- faction access or reputation, if the current system supports it;
- notice or world presentation, if such an owner exists;
- save capture and restore for outcome facts;
- localization and UI rendering;
- deterministic event ordering where selection is seeded.

The named integrator confirms each seam against current evidence. Builders claim exact paths through the active ownership process. The content package does not edit shared owners or create an alternate route. If no current owner is suitable, the proposal is returned for a decision with: player benefit, required fact, least invasive candidate seam, save implications, deterministic implications, alternatives, and a clear no-go condition.

### Acceptance rehearsal

A future integration should demonstrate the same result from: first-time play; a player who already visited a required site; a fallback site; an interrupted conversation; a save immediately after choosing; a reload before map refresh; an unavailable courier; an expired temporary location; and a repeated interaction. The verification is focused on current affected regions and owners once the integration package is approved. This plan does not run tests or claim coverage.

The content is ready for implementation planning when each choice has an observable immediate result, a verified owner or explicit decision dependency, an idempotency rule, a persistence statement, and a delayed-response policy. A consequence with no player-visible acknowledgement should be questioned. A callback with no persisted cause should be removed. A large ending consequence without a current consumer should remain an expansion hook, not be smuggled into a dialogue node.


### Late-game resolution hook: archive, relay, or open record

A late-game scene may return to the Farline decision when a larger route network is being organized. This is an expansion hook, not a requirement for the initial playable slice and not an assumed ending flag. The player can recommend that the community preserve the original record, reinstall a corrected relay notice, or leave both versions available with a clear correction attached. These approaches express different institutional habits: preserve provenance, prioritize a usable current instruction, or keep a public record of disagreement.

The hook is earned only if the earlier quest outcome is available through a current owner. The conversation can acknowledge that the player once chose a message treatment, but it should not overstate the choice as the sole cause of the community’s later policy. Other characters, new evidence, and material shortages may also matter. If the earlier outcome is unavailable in a fresh save or legacy profile, the late-game scene should present the current facts and allow a present-day choice without fabricating a past decision.

The three recommendations should lead to distinct, bounded outcomes if the architecture supports them. An archive route preserves the source copy and adds a correction. A relay route places the concise current guidance where travelers can use it, while a record remains available somewhere suitable. A disagreement route keeps both accounts visible and explains which part is known. The practical result should be observable through an existing location, faction, notice, or campaign owner. If no such owner exists, the scene stays a character discussion and the plan marks a future system dependency.

Failure-forward closure remains valid. If the original record was destroyed, the archive route can preserve a reconstruction with its uncertainty stated. If the relay is beyond repair, the current instruction can be posted at the shelter instead. If faction leadership is absent, the player can document the recommendation without claiming adoption. The goal is to let the story reach an honest resolution under degraded conditions.

This hook is valuable only if it extends the question from “what should this message say?” to “how should a community preserve corrections?” If it merely repeats the initial notice choice at a larger scale, cut it or change its decision. Late-game content should expose a new constraint, such as limited archival space, route safety, or who is allowed to edit public records. No new major faction is required: existing groups and local stewards can disagree through their practical responsibilities.

### Consequence consistency audit across surfaces

The same outcome may appear in a dialogue line, journal entry, map label, inventory item, and later quest offer. These surfaces should agree about the scope and status of the action. A journal can say that the player drafted a warning while the courier has not yet carried it. The map can show a route as uncertain while the character says it is safe to inspect. An inventory item can remain a paper copy even after a notice is posted. Review these cases together rather than validating each surface in isolation.

For every consequence, make a surface matrix with rows for conversation, journal, map, quest availability, location interaction, and save restoration. Mark each surface as required, optional, or not applicable. A required surface gets exact expected wording or state. An optional surface must not contradict the result. “Not applicable” is preferable to inventing a marker merely because another consequence has one.

The audit should include transitions in both directions where allowed: a notice can be revised, a location can be repaired and later damaged if current systems support that, or an active lead can become unavailable. If a transition is one-way, make that explicit and ensure the player understands the commitment before acting. When a result is irreversible, the consequence owner—not a dialogue string—must enforce it.

Finally, compare the consequence outcome after a save and reload. The visible journal, map, and next dialogue offer should be reconstructed from the same authority. If one surface depends on an ephemeral scene variable, it may disagree after restore. Such a defect should be fixed at the existing owner seam before adding more branch content.

### Stop conditions

Do not expand consequence scope until the existing owner is identified and the immediate effect is observable. Stop a proposed branch if its result depends on a new persistence authority, a second campaign flag store, or an unowned notification queue. Keep the scene local, rewrite the promise, or raise the exact architecture question through the established process.

A branch is ready to expand when its state, save path, visible acknowledgement, and repeat behavior are all known. This stop condition protects the story from making promises the game cannot yet keep and gives future integration work a bounded starting point.

### Review the consequence from the receiving side

A consequence should be reviewed not only where the player makes it, but where another character or system receives it. If a courier carries a warning, what do they need to know to act? If a notice is posted, who can read it and when? If a faction refuses it, does the player learn whether the refusal was about the message, the source, or capacity? These receiving conditions make an effect concrete.

The receiver should not be a passive trigger. A character can accept the message but shorten it, delay it, or ask for a source. Such changes require explicit authored options and supported effects. If the scene cannot represent a receiver’s response, keep the outcome local and describe it honestly.

This pass completes the routing review: the player’s intent is recorded, the current owner applies it, the receiver can act within their role, and a later surface reports only what the game can verify.

## Continuation pass 14 — Empty Shift report outcomes, work handoffs, and owner-specific consequences

### Design goal

The Empty Shift story ends when the player chooses what the shelter should do with an unreliable record. It is not enough to display different dialogue. The selected report must have a defined effect boundary, an authoritative owner, a persistence rule if needed, and a later acknowledgement that does not claim more than the game can verify.

This plan does not establish a new workforce simulator, roster authority, food ledger, or heat meter. If current systems expose work assignment, resource allocation, or shelter condition, the integration package should route the outcome through those owners. If those systems are absent or cannot safely represent the proposal, the first release remains a narrative record correction. A fictional change in staffing cannot be represented as a gameplay change merely because the dialogue says it occurred.

### Four report treatments

**Correct the names.** The player recommends replacing a stale assignment with a verified worker list. This option is available only if direct evidence identifies who currently covers the task. The immediate board can be updated through its owner. If the current work system has no assignment consumer, the result is a corrected authored notice without a claim that shifts or shelter conditions changed.

**Annotate the distinction.** The player leaves the names in place but adds that the roster denotes planned coverage rather than confirmed attendance. This preserves existing operations while making uncertainty visible. It is the lowest-risk outcome when the task continues but the named workers cannot be confirmed. A later dialogue can show how another person reads the note, but should not claim the whole shelter adopted a new policy.

**Request another account.** The player delays a correction until the next relevant person can confirm the handoff. This creates a blocked or in-progress outcome only if the current quest owner supports that lifecycle. The journal identifies the missing evidence and a fallback. The task cannot wait forever without an authored expiry or a safe unresolved close.

**Close unresolved.** The player records that the copies match but attendance remains unknown. The investigation completes as an uncertainty-preserving result. This is not a failed quest. Its reward can be informational closure, a visible correction label, or a character response. It cannot grant a “verified assignment” reward.

Each treatment is mutually exclusive for the current record version. A later authorized edit can create a new version if the existing board owner supports revision. The dialogue graph must not write into a record that the quest owner already controls.

### Owner map

| Outcome fact | Candidate authority to verify | Safe narrative result if unsupported |
|---|---|---|
| Investigation stage and report treatment | Current quest result/lifecycle owner | Close with a concise authored summary only if quest state is not promised |
| Roster display or annotation | Existing location interaction or board content owner | Show the choice in dialogue/journal, avoid claiming a persistent board edit |
| Worker assignment | Existing survivor work/assignment owner, if one exists | State that coverage remains unknown |
| Meal quantity or reserve | Current food/inventory/resource owner, if applicable | Keep the resource choice as a discussion; do not change a parallel count |
| Annex access or repair | Current location/maintenance owner | Leave the door or system unchanged and report access limits |
| Character reaction | Existing relationship owner where persistent; otherwise current conversation | Present a local response without a lasting trust claim |
| Map marker and route availability | Existing expedition/location owner | Keep the known site visible with its known access state |
| Later callback | Existing quest/campaign trigger owner | Use the next relevant authored conversation, or omit the callback |
| Save/restore | Current save-section owner for each persisted fact | Do not persist unsupported derived state in dialogue nodes |

The candidate list is not confirmation that each authority exists. A source audit must replace each candidate with an exact current owner and supported command/event. The plan’s first playable slice can avoid all unsupported owners by limiting effects to quest closure and authored conversation, if that route is available.

### Handoff protocol

The player’s choice should be treated as a recommendation or authorized edit according to the fiction. If Mara controls the board, the player may propose an annotation and Mara may accept it. If the player has explicit authority to update the record, the choice can perform the edit directly. This distinction affects whether the result is immediate or pending. Do not present a player choice as enacted if a character still needs to approve it.

A safe sequence is:

1. Offer the player a clear report treatment with its immediate scope.
2. Record the chosen treatment through the existing quest/dialogue interaction.
3. Ask the current board owner to validate whether the action can be applied.
4. Apply the supported edit once.
5. If the effect is deferred, expose a pending status and retain the chosen treatment.
6. Refresh the board/journal/map through their normal presentation routes.
7. Save through existing ownership.
8. On restore, rebuild the same board/result state without replaying the choice.
9. Trigger a callback only after the receiver has actually acted or an explicit expiry occurs.

If the board owner rejects the edit due to new evidence, the game needs a clear re-open or alternate result. The player’s earlier decision remains part of history. A correction is not proof that the player’s earlier choice was foolish; it reflects new information.

### Causal effects and non-effects

The package should separate intended consequence from collateral consequence. A corrected roster might make it easier to coordinate work. It does not automatically increase heat, reduce illness, improve morale, or grant food. Such broader effects require system support and balancing. A physical repair may improve a service if its owner measures that condition, but an authored line cannot substitute for a simulated repair.

Likewise, a character may appreciate that the player asked before assigning their name. That local reaction can be written without claiming a permanent relationship gain. A faction-wide trust effect is inappropriate unless a faction owner represents it and the player was told the report would be shared. A map marker may appear if the player discovered the Annex; the report outcome should not reveal the location to every survivor by implication.

List each proposed downstream consequence as confirmed, candidate, or deferred. The first release should have a small number of confirmed outcomes. Deferred ideas remain expansion hooks and should not appear in the player-facing choice descriptions.

### Failure cuts

**Board was moved.** The player can still report the outcome through the current character or location route. The old map marker may lead to an empty wall only if the scene acknowledges the board’s relocation. Do not keep both board instances alive as independent authorities.

**Named worker leaves.** The record can be annotated as unconfirmed. If the task requires a current worker, present another authored route or close the task without assigning an absent survivor. No automatic replacement should be invented by dialogue.

**Player has no independent source.** Offer the unresolved outcome or a request for another account. Do not select “correct the names” as a default.

**Player chooses correction but no work owner exists.** Apply only the supported narrative/quest result. Tell the player that the record was marked for follow-up rather than claiming work reassignment.

**Save/load interrupts the scene.** Restore the current dialogue or result using the established interaction contract. The player can continue the conversation or see the completed result; the edit must not run twice.

**Character who accepts edit disappears.** The pending edit either remains queued through an existing owner or closes with a notice that no confirmation was received. If neither is supported, do not offer a deferred edit.

**Conflicting quests edit the same board.** Merge through the current board owner with explicit version or overwrite behavior. If no owner exists to resolve conflicts, serialize the edits by requiring the first task to close or remain local.

**Player abandons the quest.** Preserve an edit already applied. Cancel an unapplied recommendation only if the current quest owner can represent that transition and the player was not told delivery already occurred.

### Delayed acknowledgement

The next relevant scene can show the outcome at a practical scale. If annotated, a new sheet carries a small phrase: “planned coverage; attendance not checked.” If corrected, a worker’s name appears only when validated by evidence. If awaiting another account, the board still shows the prior entry with a question mark or other current UI convention. If unresolved, the entry remains but the journal closes the investigation with the stated limit.

Characters should report only what they know. Mara can say she has copied the annotation. Nessa can say who agreed to cover the next shift. Sella can say the meal count is now based on the people expected in the room. None should claim that the whole shelter’s operational performance changed unless the current system measured it.

A callback can be skipped if its character is unavailable. The required closure should remain visible in the journal or current board. No ending depends on a particular callback line. If the outcome later matters to a larger campaign, use the same persisted fact or quest result; do not create a duplicate campaign flag solely for an ending sentence.

### Consequence validation plan

A future implementation package should verify: each response is eligible only after the right context; effect is applied once; the board text matches the outcome; a worker is not assigned without evidence or owner support; resource values change only through their current owner; a pending outcome cannot masquerade as applied; save/restore reproduces the selected result; repeated interaction does not duplicate rewards; and delayed dialogue accurately reports only observed results.

The minimal slice should test one immediate annotation and one unresolved outcome. A separate authorized task can later connect assignments, food, or maintenance if the live code supports them. Focused verification should follow TEST_POLICY.md and the owner’s acceptance gate. This document creates no test file and claims no runtime coverage.


### Consequence contract for the Empty Shift

The player’s choice about a roster should create a concrete result without pretending the conversation itself runs shelter operations. A well-scoped outcome says what changed in the record, what remains uncertain, who can act next, and which owner is responsible. It separates record correction from work assignment, resource allocation, relationship response, location repair, and campaign reputation.

The minimal consequence is a quest result plus a truthful acknowledgement. Larger cross-system effects are optional layers and require current evidence that a suitable owner exists. A future plan can extend them, but the current narrative should remain playable if no such extension is approved.

### Effect envelope by outcome

**Correct names after verification.** Primary fact: current assignment is supported by independent evidence. Candidate owner: current work assignment or board content owner. Immediate feedback: corrected entry and journal result. Persistent scope: only the record and any actual assignment the owner accepts. Callback: next shift can acknowledge the new line. If no assignment owner exists, report that the names were corrected on the board but do not state that labor moved.

**Annotate planned coverage.** Primary fact: the roster is a plan, not attendance proof. Candidate owner: current board/location or quest result owner. Immediate feedback: annotation is visible or the journal describes the choice. Persistent scope: the interpretive note. Callback: a later reader asks who will confirm attendance. No automatic resource outcome.

**Request another account.** Primary fact: player deferred correction pending a source. Candidate owner: quest lifecycle. Immediate feedback: the missing source is named and the current record remains. Persistent scope: quest remains active or blocked according to current state model. Callback: new account is offered when an authored milestone occurs. If no such milestone is possible, the player can close as unresolved.

**Close unresolved.** Primary fact: mismatch was observed; attendance was not established. Candidate owner: quest result. Immediate feedback: quest closes and the journal states the limit. Persistent scope: outcome if future dialogue reads it. Callback: a later line does not claim the problem was solved.

**Decline the investigation.** Primary fact: player has not authorized further work. Candidate owner: current quest offering route. Immediate feedback: request remains available or closes as declined according to current policy. Persistent scope: only if future offers need to avoid repetition. No reputation penalty by default.

### How state changes should cross owners

The player’s dialogue choice originates in the current dialogue route. It should not directly mutate a location, character, food count, or work assignment. The dialogue route asks the appropriate quest or domain owner to apply a supported action. That owner validates whether the action is legal and returns its actual result. Host adapters then refresh UI or location presentation from the resulting authoritative state.

This separation matters because a choice can be valid as an idea but invalid operationally. The player may request that Oren’s name be removed, but current evidence may not identify a replacement. The board owner can accept an annotation while rejecting a reassignment. A food owner might accept a count review without changing any inventory amount. The story should describe the action that succeeded, not the one the player hoped would happen.

The owner response should be explicit enough for the host to distinguish applied, pending, rejected, and unavailable outcomes if current contracts support that distinction. If not, simplify the interaction into options whose effects are immediately supported. Do not build an effect queue just to represent pending paperwork.

### Ordering and transactional behavior

A multi-owner branch can fail halfway if the quest is marked complete before a required board change succeeds. The integration design should choose a primary owner and make other effects conditional follow-ups. For example, the quest result can record “annotation chosen”; the board owner then applies the annotation if it can. If the board update fails, the player sees “ready to post” or the quest stays open, according to the current owner’s existing transaction pattern.

Avoid two authorities writing the same result. A dialogue node should not both set the board’s state and emit a command that causes the location host to set it again. The current event/command seam decides which part is authoritative. Idempotency prevents a repeated click or restore from applying the same change twice.

If one consequence must update two independent domains, the owner’s contract should define failure recovery. For example, a verified reassignment might update the work schedule and then refresh meal planning. The game must know whether both are atomic, whether the second is derived, or whether a failed refresh can be retried. This plan does not prescribe infrastructure. It requires the implementation owner to demonstrate the existing pattern before the branch is approved.

### Consequence matrix by surface

| Surface | Correct names | Annotate | Request account | Unresolved |
|---|---|---|---|---|
| Dialogue | States what was applied | Explains limited claim | Names missing source | Confirms deliberate closure |
| Quest log | Complete with verified result | Complete with distinction | Active/blocked if supported | Complete with uncertainty |
| Map | No pin if no next action | Optional board marker if known | Source route marker | No active quest marker |
| Board | New names only if supported | Visible note if supported | Existing copy remains | Existing copy with unresolved status |
| Food/work UI | Changes only through verified owner | No change unless separately chosen | No change yet | No change |
| Character callback | Confirms assigned action | Reports who understood note | Supplies new account | Acknowledges limits |
| Save restore | Outcome reconstructed | Annotation preserved if owner persists | Active route restored | Terminal result remains |

If a surface is unsupported, its row becomes not applicable. Do not invent a map marker or work-panel update because the table includes a column.

### Delayed effect and callback contract

A delayed callback must have a trigger that is already expressible: a later quest stage, a return visit, a completed expedition, a scheduled world event, or another current milestone. The callback should not depend on the user spending exactly two days in a particular place unless a reliable current time system supports that. An arbitrary delay can become immediate for a player who waits or never occur for a player who travels differently.

Before firing, check that the underlying outcome is still current. If the player revises the roster or new evidence supersedes an earlier report, the callback should use the latest accepted state or explicitly mention the correction. A callback that says “the board stayed as you wrote it” is invalid if another authorized actor changed it. When the owner cannot persist revisions, keep the callback local and avoid referring to history beyond the current quest outcome.

If the intended speaker is absent, an alternate medium can convey basic closure: a board annotation, a short note, or another character’s firsthand observation. The alternate source must not claim access to private dialogue. Optional personal reaction can be lost; operational closure should remain available when the main quest depends on it.

### Recovery and consequence reversibility

Some outcomes can be revised; others should be final. An annotation can be updated after another account arrives if the current board owner supports revisions. A meal already served cannot be unserved. A character’s earlier statement cannot be erased. A public accusation may require an explicit retraction rather than a silent flag flip. The plan should label each effect as reversible, compensatable, or final.

**Reversible:** a board annotation can be replaced with a newer note. Preserve provenance only if the current owner supports it.

**Compensatable:** a route was delayed; the player can arrange a later handoff, but time has passed.

**Final:** the player disposed of the only copy or openly accused someone. Offer a repair route without pretending the prior action never happened.

A player should know when a choice is final. The game can use a confirmation step if it matches current UX conventions and consequence magnitude. Do not add confirmation dialogs for ordinary dialogue flavor.

### Possible failure states

**Effect rejected.** The relevant owner rejects the requested change. Report the reason and offer a narrower alternative, such as annotation instead of reassignment.

**Receiver absent.** Keep a pending action only if current architecture can own it. Otherwise offer a note or a later conversation.

**Evidence contradicted.** New information disproves an earlier interpretation. Preserve the original choice’s context and allow correction.

**Duplicate event.** The same dialogue action is submitted twice. The owner returns the existing result and no duplicate reward or assignment occurs.

**Save restore mismatch.** The journal and dialogue disagree after reload. Resolve through the authoritative owner and focused verification; do not add a second copy of the state.

**Location removed from the build.** The record references an unshipped location. The content validator blocks release or the quest maps to a tested equivalent.

**Optional system deferred.** Work or food simulation cannot be affected. The narrative outcome remains local and clearly describes the board/report only.

### Player expectation and ethics of failure-forward outcomes

A failure-forward route should not feel like the game is laundering a bad outcome. If the player accuses Oren and later evidence contradicts them, a correction scene can let the player retract the claim, offer an apology, or acknowledge uncertainty. The response is about repairing a relationship, not awarding a clean-slate score. If the player corrects a roster prematurely, the board may need an amended note. This consequence should be understandable from prior dialogue.

If the player preserves a questionable assignment and a later task goes uncovered, do not imply that a single player choice caused all subsequent hardship. Show the causal facts: what the player knew, what the record said, and what changed later. The world can respond to the decision without moralizing. Conversely, a cautious choice should not guarantee that no one is harmed. The game preserves uncertainty both in evidence and in outcome.

### Late-game bridge

At a later chapter, the player may be asked to help establish a shared shift-record practice across several shelters. This is not a new faction and does not require a universal roster platform. It can be a policy choice among record labels: planned, covered, worked, and confirmed. The player might choose to preserve multiple stages or keep the sheet brief. Each option affects how later characters interpret work history only if existing campaign and dialogue owners can read the chosen result.

This late-game bridge should be considered only after the local story works. It creates a new design question—how to keep records useful during crisis—rather than simply raising the same roster issue to a larger map. It may unlock a character conversation or a location board variant. A broad mechanical effect on survivor assignment is a later architecture proposal, not implied content.

### Integration sequence

1. Verify current dialogue command/effect route.
2. Verify current quest lifecycle and result representation.
3. Identify actual board/location owner, if any.
4. Confirm whether work, food, or maintenance systems expose a supported command.
5. Choose a narrative-only first slice if domain owners cannot safely receive effects.
6. Claim exact files under live worktree ownership.
7. Implement one immediate result and one unresolved result.
8. Prove save/reload and duplicate-interaction behavior in the focused target.
9. Add a delayed callback only after the outcome is observable.
10. Expand cross-system consequences under separate acceptance once evidence supports them.

### Acceptance criteria

The story consequence is ready when: the player can predict the immediate scope; each effect has one current owner; no panel or dialogue callback becomes a parallel authority; unsupported work/food changes are omitted; repeated selection is safe; save restoration is truthful; callbacks use verified outcomes; and every non-success route still closes or remains explicitly active. The plan remains documentation-only until the required owners and integration authorization are confirmed.


### System interaction scenarios for Empty Shift outcomes

The story touches work, food, maintenance, relationships, maps, and quest results. That breadth creates useful design opportunities but also integration risk. Each scenario below states the player value and a safe boundary.

#### Scenario A — Record only

The player chooses to annotate the roster. The quest result records that the player distinguishes planned coverage from attendance. The board interaction shows the annotation if its current owner supports a persistent edit. No work assignment changes. No food value changes. The player receives a short acknowledgement from Mara or the journal.

This is the preferred MVP because it can prove branch and persistence without depending on a new simulation system. If even the board is not stateful, the choice can complete as a narrative result and be acknowledged in later dialogue only if the quest owner persists it.

#### Scenario B — Existing work owner accepts reassignment

A later source identifies the current worker, and the current work system exposes an approved assignment command. The player may recommend a replacement. The owner validates that the survivor is available and eligible. If accepted, the work schedule changes through the owner; if rejected, the board remains annotated and the journal explains that no assignment was made. The quest cannot bypass skills, injuries, or existing work rules.

This scenario is a future integration slice, not part of the narrative-only baseline. It requires save restore, deterministic event order, and focused verification.

#### Scenario C — Existing food owner recalculates expected count

If an existing food system reads a current roster, the proposal may update its inputs through that owner. If it does not, the story cannot claim that meal quantities change. A cook can still discuss whether the count is useful. Do not build a parallel allocation counter for one quest.

The user-facing outcome must distinguish expected count from actual inventory. If the player holds a reserve, that choice should be explicit, bounded, and balanced by the current resource owner.

#### Scenario D — Maintenance owner changes the Annex condition

If the player repairs the service panel and an existing maintenance or location system models its condition, the repair can alter the current state. The work assignment is still separate: a repaired panel does not prove who completed prior maintenance. The scene can show both outcomes without conflating cause and evidence.

#### Scenario E — Relationship response only

Oren may appreciate being asked before his name is assigned. If the existing relationship owner supports that interaction, record it through its normal command. Otherwise, write a local dialogue acknowledgement and do not claim permanent trust. Relationship change must be proportionate and not a reward for selecting the author’s preferred report.

#### Scenario F — Campaign policy callback

A later leadership discussion may ask whether work boards should distinguish planned, covered, worked, and confirmed. The player’s earlier choice can be referenced if the outcome was persisted by an existing owner. The late-game decision should not unlock only for players who found a hidden clue. An unresolved or missing earlier result receives a neutral choice based on the current information.

### Consequence dependencies and owner graph

The outcome graph can be represented at design time as:

Player response → current dialogue command route → primary quest/result owner → optional board/location owner → derived UI refresh → save capture/restore → later dialogue condition.

Optional cross-system routes branch from the primary outcome only when their owner exists:
- board outcome → work assignment owner;
- board outcome → meal planning/resource owner;
- physical repair → location/maintenance owner;
- relationship reaction → current relationship owner;
- campaign policy → current campaign/ending owner.

Each route needs a dependency row. If the primary quest result succeeds but an optional owner rejects a secondary effect, the player sees the actual result and the quest does not lie. If a mandatory effect cannot apply, the primary choice should remain pending or offer a different option. The owner graph is editorial until exact APIs are verified.

### Reward architecture

Rewards should not be copied across all branch outcomes. The quest can grant a completion acknowledgment or unlock information without material payout. If the player repairs something, a resource cost and benefit use current craft rules. If the player escorts a worker, the return may be access to a conversation or safe route, not an arbitrary item. If evidence is lost, the player may still close the quest but receive a less certain result.

A reward matrix should state:
- reward type;
- existing authority that grants it;
- exact trigger;
- whether the player sees the result immediately;
- whether duplicate completion can grant it again;
- whether failure-forward completion qualifies;
- balancing reason;
- localization/UI label.

If no current reward owner applies, leave the option narrative. Avoid granting faction reputation for a choice that no faction reacts to in-world. Avoid bonus materials as compensation for choosing a cautious branch if that creates an obviously optimal answer.

### Faction and community effects

The Empty Shift does not introduce a new faction. If a current faction has a legitimate role in the shelter’s work records, it may react through existing standing or access. The design first asks whether a local group’s policy is better represented as a character or location practice. A minor board disagreement is not enough reason to create a new territorial group.

A faction consequence needs an observable result: authorization changes, a representative agrees to review the record, a route-specific task becomes available, or a public notice is accepted. A line that says “they respect your judgment” is not sufficient. The effect must be within the existing faction owner’s meaning and persistence.

Community outcomes can remain local. A board annotation may be adopted by a handful of workers without implying settlement-wide reform. A later scene can show one person checking the copy. Scope language matters: “Mara changed the sheet” is verifiable; “the community now records all shifts correctly” is an unsupported broad claim.

### Ending consequence boundaries

A late-game ending can reference the player’s history with work records only if campaign ending logic has a supported place for that fact. Even then, the reference should be proportional. The player’s annotation might give a character a reason to ask for another source; it should not determine an entire settlement’s governance outcome by itself.

Ending consequences are separated from local and quest consequences in review. For every candidate ending reference, identify:
- which earlier fact is consumed;
- how it is saved;
- what if the player never accepted the quest;
- what if they resolved it as uncertain;
- what if the character/source was absent;
- whether other choices can produce equivalent evidence;
- which ending route presents the result;
- how the line stays truthful if the work system was never changed.

If these questions cannot be answered, keep the ending hook in the plan only. Do not add ending flags in the dialogue graph.

### Delay, expiry, and pending results

A pending report is different from a blocked quest. Pending means the player submitted an action and the owner has not confirmed it. Blocked means the player cannot make progress until a condition changes. The current lifecycle may not distinguish those states; if it does not, use plain journal wording and a bounded next action without adding new save state.

Every pending result needs:
- a real receiver;
- a supported delivery path;
- an expected resolution event;
- timeout/expiry behavior;
- cancellation or retry policy;
- duplicate suppression;
- player-facing status.

If no event system can deliver the result, the player should receive an immediate narrative closure instead of a fake pending state. A message sitting forever in a nonexistent queue is worse than a scoped result.

### Consequence reversibility and compensation

An annotation can be updated after a new source. A worker assignment may be changed through the current work system, with any costs that owner applies. A meal already distributed is final. A public claim may require a retraction. The player should know the difference before selecting a branch.

When correcting a mistake, preserve causal history only at the level the current system can support. The journal might say a previous entry was revised. Do not build a full version-control system for one board. If the board has no history model, the quest can surface a correction conversation and show the current text without claiming archival provenance.

Compensation is not always a resource payout. It can be a chance to apologize, reassign a safe task, clarify a notice, or accept that evidence was lost. The narrative should not erase consequences to make every branch converge on a perfect outcome.

### Outcome/event ordering and determinism

If a branch triggers a host event, the event should be based on a stable result from the primary owner. The order of side effects must not depend on collection iteration or dialogue node loading order. Any seeded variation in a later callback uses the existing RNG owner and must not change the underlying report. Repeated same-seed runs with the same player choices should produce the same state when deterministic behavior is required.

The future implementation review will inspect event subscribers, save ordering, and restore callbacks. A scene may display after a choice, but the display does not own the state. Save checksums or existing replay tools, if relevant to the affected owner, should verify that the result survives the normal route. This plan does not call those tools or claim deterministic coverage.

### UI and player feedback

After a consequential choice, the UI should communicate:
- accepted choice;
- applied/pending/rejected state, where relevant;
- immediate state change;
- next action if one remains;
- effect not applied if a system dependency is unavailable.

Do not show multiple redundant popups for a single choice. The dialogue acknowledgement and journal update can work together if each provides distinct information. Map markers refresh only when a location or route actually changes. Focus and close behavior remain consistent with the current dialogue panel.

Feedback should be accessible and readable. Use text to describe status, not only color. A pending effect must not appear as a completed checkmark. A failed owner command should not leave the response button in a success state. If a new status surface is needed, that UI work is a dependency and should be scoped under the existing presentation owner.

### Acceptance and release decision

The minimal route is accepted when a player can inspect the record, choose an honest outcome, see a truthful acknowledgement, save, reload, and continue without duplicate effects. Cross-system behavior is accepted only when its owner-specific command is verified. The release packet documents any feature intentionally omitted: no worker reassignment, no meal count change, no new faction reputation, no universal record history, or no ending dependency unless implemented through an approved current owner.


### End-to-end route examples

#### Route 1 — The player annotates

The player inspected both roster copies, heard that “covered” may mean a substitute accepted the task, and found no source that confirms who worked. In the report meeting, the player chooses annotation. The existing quest owner records that result. If a board owner supports the edit, the next copy says “planned coverage; attendance not confirmed.” If it does not, the journal records the selected wording and the scene makes clear that the public sheet has not yet changed.

On the next eligible visit, Mara reports whether she copied the annotation. If the player saved after the first choice, restore reconstructs the same outcome and does not offer a second reward. The map does not gain a new marker unless a location owner changes its availability. The story remains complete even if no work or food system was touched.

#### Route 2 — The player closes unresolved

The player has one roster and a witness who cannot remember the shift. They choose unresolved closure. The quest result is terminal for the current evidence set. The journal says the copies match but attendance is unknown. The ordinary shelter dialogue remains open. The optional hidden clue may later produce new evidence, but the main quest only reopens if an authored condition explicitly says so. A late callback can mention that the old record was kept; it cannot assert that the player chose an annotation.

#### Route 3 — The player recommends reassignment

The player obtains a current worker’s direct confirmation and asks to replace the old names. If the current assignment owner supports that command, it validates the proposed worker against existing availability and constraints. The UI reports whether the assignment changed. If rejected, the player can annotate instead. The dialogue does not mark the worker assigned simply because the choice was selected.

#### Route 4 — The player repairs the panel

The player uses a supported repair interaction and changes the Annex’s current condition. That repair can enable future inspection. It does not prove the previous shift was staffed. The quest evidence and maintenance state are separate. A later report can state both: the panel is serviceable now; prior attendance remains uncertain.

#### Route 5 — The player follows the escort branch

The player escorts a temporary worker and reaches a handoff. The receiver signs or verbally confirms receipt through a current event route. That supports delivery, not prior roster attendance. The journal should not merge the proof. If the player cannot travel, a different carrier or a delayed result is offered only when authored and supported.

### Consequence relationship matrix

| Consequence kind | Primary question | Reversible? | Evidence source | Owner check |
|---|---|---:|---|---|
| Cosmetic text | Did wording or tone change? | Yes | Choice response | Dialogue/presentation route |
| Local notice | Is the current scene display updated? | Usually | Player action | Current scene/location owner |
| Quest result | Did the objective resolve and how? | Rarely | Objective proof | Quest lifecycle/save owner |
| Relationship response | Did the character’s persistent relation change? | According to system | Actual interaction | Relationship owner |
| Faction access | Does the group permit a new action? | According to system | Shared/accepted action | Faction owner |
| World/location state | Did physical access or condition change? | Depends | Repair/travel event | Location/world owner |
| Resource result | Did a count or item amount change? | Often not fully | Existing resource command | Inventory/resource owner |
| Campaign ending input | Should a later ending branch consume it? | Usually final | Persisted prior result | Campaign owner |

The author must choose the smallest consequence scope that satisfies the player promise. A local board annotation is not a faction consequence. A character’s statement is not a relationship update. A completed escort is not proof of repaired infrastructure.

### Ordering, save, and restore rehearsal

A save can happen after the player chooses a report but before a callback. On load, the game reconstructs the quest result first through its save owner, then derives which callback is eligible. It should not replay the whole meeting. If the current save process restores host UI before Core state, the existing lifecycle decides when the panel refreshes; this plan does not rearrange that sequence.

A save can also occur after a worker assignment succeeds but before the board visually refreshes. The restored board should derive from the assignment or board owner, not from a dialogue-local flag. If the two owners have independent state, the integration package must define source precedence. The player must not see one name on the board and another in the assignment panel.

A failed save must follow current recovery policy. Do not display success optimistically if the persistent owner has not confirmed the action. If the domain change is committed but a presentation refresh fails, the UI can recover by re-reading state on the next open.

### Event identity and duplicate suppression

Any consequence crossing an event seam needs an identity or completion fact sufficient for the current owner to suppress duplicates. The design does not prescribe a new event ID format. It asks the implementation owner to demonstrate that repeated clicks, a repeated callback, or save restoration cannot apply the same result twice.

One-time rewards are granted by the established quest completion path. A dialogue response cannot grant the same reward again on every revisit. An optional repeatable board task needs a current repeat policy. If no owner can distinguish a new cycle, keep the content one-time.

### Resource and balance boundary

The Quiet Count may discuss food, but its branch should not alter quantities unless the existing food/resource authority accepts an explicit supported command. If it does, balancing review considers the cost of holding or distributing a reserve, who benefits, and whether the branch creates an exploitable reward loop. The story must not grant food simply for asking a question.

The Bench Ticket may use a current crafting item. Craft cost and return must follow the existing inventory/recipe owners. If the action unlocks a repair, the location owner sets the result. The quest’s text describes the practical outcome without asserting exact resource changes that the system did not apply.

### Relationship and reputation boundary

Character trust can differ from faction reputation. Oren’s willingness to confirm a task is an individual action. A faction adopting a work-board policy requires explicit shared authorization and a current faction effect. Do not conflate them into a single “community trust” reward.

If the existing relationship system cannot represent the fine distinction, use dialogue-only reactions. The character can say that they appreciate being asked, but later content should not gate a major event on a relationship change that was never saved. If a faction’s access changes, show which service, meeting, or route became available.

### Endgame and long-range callback matrix

**No quest accepted:** a later leader can explain the current board policy without referencing the player’s earlier choice.

**Quest resolved verified:** the ending scene may cite the verified practice, not claim every future roster is correct.

**Quest annotated:** the leader may ask whether the distinction should be used more widely; the player can choose again.

**Quest unresolved:** the leader receives an uncertain record and can decide whether to investigate independently.

**Hidden name clue found:** an optional personal line can acknowledge the account if canon and save owner support it.

**No resource effects integrated:** the ending remains narrative, with no claim that staffing or food efficiency improved.

The campaign should not penalize players for skipping this optional thread. If its outcome is absent, a neutral baseline is used. A major ending must not silently assume that the player accepted a side quest.

### Human review of consequence fairness

A review group should read the choice text with no context, then read the result without seeing the choice. They ask:
- Does the choice communicate what the player is authorizing?
- Does the result acknowledge the same action?
- Does a character claim more than they know?
- Can an ordinary player infer the remaining uncertainty?
- Is the reward proportional?
- Can the branch be revised, and was that explained?
- Can an absent NPC or unavailable location still produce closure?
- Does a later scene remember the outcome correctly?
- Is the lack of a systemic effect clearly reflected in the prose?

If the result surprises players in a way that comes from hidden implementation rather than intended uncertainty, revise the choice or the acknowledgement. Consequences can be uncertain, but the game’s contract should be clear.

### Failure-forward final states

The investigation can end with an unresolved report, a damaged source, a rejected assignment, an absent witness, or a correction that arrives after the player acted. These are not identical. The journal should preserve which situation occurred. The game can offer a new action where plausible: request a second account, post a correction, keep a private copy, or leave the board as is. No forced apology, compensation, or resource penalty is required in every case.

The system’s purpose is to make state truthful and consequences legible. If a failure route has no new action, it can still close as an honest result. The player should not be trapped in a quest that repeats a failed interaction forever.

### Implementation review receipt

When this proposal is promoted, the integrator should record:
- actual owners and exact path claims;
- effect routes and save boundaries;
- deterministic assumptions;
- any unsupported effects removed;
- focused verification and runtime path;
- player-visible outcome for each branch;
- retry/idempotency evidence;
- rollback or content retirement plan;
- the remaining expansion hooks.

Only that receipt can establish that the implementation matches the design. This plan remains a design contract until then.


### Consequence audit with a cold reader

A reviewer who has not read the design should see only the player-facing choice and result. Ask them to state what they expected, what changed, what did not change, and what remains uncertain. Compare that account with the intended owner effects. If the reviewer assumes that choosing “correct the names” reassigned a worker but the game only changed journal wording, the response overpromised. If they expect an unresolved report to remain active but it closes, the lifecycle copy is unclear.

The audit should include one branch with no systemic effect. Its wording must be honest and satisfying as narrative content. A game can deliver a meaningful choice without changing a numeric resource; it cannot claim a numeric or organizational change that never happened. Record the cold-read result with the content handoff.

### Rollback and branch retirement

Before content is shipped, remove a branch by deleting its unreferenced authored records through the canonical data owner and regenerating indexes as required. After release, first inspect saves and current consumers. Active players need a closure or redirection if the branch disappears. Persisted report outcomes cannot be dropped while later content reads them.

Rollback should preserve the last truthful player-facing state. If a board annotation feature is disabled, the journal can still report the player’s prior choice, but the game must not show an annotation that no longer exists. Document which effects remain, which are suppressed, and how a saved active quest resolves. Do not reset player state to simplify content retirement.

### Failure messages and recovery clarity

When a consequence cannot apply, tell the player what actually happened and what remains possible. “The schedule did not change; no replacement was confirmed” is clearer than a generic failure toast. “The note is ready, but Mara has not posted it” is truthful only if a pending state exists. Otherwise say that the player can raise the question again during the next visit.

The recovery option should use the same owner path as the original effect. A retry cannot bypass validation or create a second result. If there is no safe retry route, close with an honest unresolved state and preserve any earlier applied consequence.

### Consequence closeout

A consequential dialogue choice is complete when the owning system accepts it, the player sees the actual result, and future content reads the same authoritative fact. If any of those parts is missing, keep the proposal local or mark the seam as an explicit dependency.

## Pass 15 — Consequence routing, delayed callbacks, and Chronicle closure

**Status: PROPOSAL, premise-gated.** This pass defines how authored dialogue could cause results without owning duplicate game state. It draws on the world bible's delayed moral-choice callback and endgame Chronicle lanes. It does not claim that a generic “dialogue graph” or deferred-event scheduler is already integrated.

### 15.1 Prior-pass collision correction

The Empty Shift storyline must be compared with the live duty-roster system and its catalogs before any consequence is promoted. If a response changes assignment, shift safety, coverage, or roster history, route it to DutyRosterQuestRuntime/DutyRosterSystem only after inspecting their supported commands. Do not add roster state to a dialogue consequence ledger. The story, route names, characters, and local outcomes in earlier passes remain DRAFT collision candidates.

### 15.2 Consequence layers

Use the consequence vocabulary from the master plan as an impact label, not as six independent storage systems:

| Layer | Meaning | Canonical application |
|---|---|---|
| Cosmetic | Changes wording or tone only. | Dialogue presentation/read model. |
| Local | Changes the current scene or encounter result. | Existing encounter result owner. |
| Quest | Advances, fails, or resolves an objective. | Quest owner, commonly through its accepted event/command. |
| Relationship | Changes a character relationship or remembers a social act. | Current relationship/character event owner. |
| Faction | Changes access, standing, or hostility. | Faction stance/standing authority. |
| World | Changes a location, resource, enemy, weather consequence, or future event. | The domain owner for that fact. |
| Ending | Supplies a verified fact to a major resolution/Chronicle path. | Existing verdict, ending evaluator, or Chronicle input owner. |

A response may touch more than one layer, but each mutation must be independently named and observable. “The world changes” is not a valid effect definition.

### 15.3 Command envelope and exactly-once behavior

Treat a dialogue response as a request, not a direct write into arbitrary systems. Before a production extension, compare its fields with the current EncounterChoiceEffectDispatcher, NarrativeConsequence graph/router, IFlagLedger, CampaignConsequenceLedger, quest event path, and host adapter. Reuse the narrowest current seam that can express the effect.

For each committed choice, the review packet names: stable authored response ID; current encounter/quest instance; expected precondition; owner command; effect idempotency key if supported; result returned; journal/Chronicle fact if supported; and what happens if the command is rejected or replayed. A duplicate click, UI refresh, save boundary, event retry, or deterministic replay must not grant a reward twice or apply a faction shift twice.

Do not have the panel first mutate a local “choice made” flag and then ask Core to apply the effect. Do not fire two owners from one vague string. If one action should cause independent effects, the owner/host event path should make their ordering explicit and provide a recoverable diagnostic when one rejects.

### 15.4 Delayed callback design

The master world bible identifies a strong content opening: early DoorEncounterSystem decisions can receive multi-month follow-up through a choice fact in IFlagLedger and a return around a later horizon. Source inspection confirms DoorEncounterSystem currently resolves and persists encounter IDs and reaction totals, but does not define a generic future-delivery queue in the inspected class. The repo has IFlagLedger and CampaignConsequenceLedger, but their presence alone does not prove that either supports due-day scheduling, save/restore, exactly-once dispatch, or event delivery.

The safe proposal is a three-part slice:

1. An early authored choice writes only its existing canonical choice/outcome fact through the current effect path.
2. A later eligible encounter or quest is surfaced by a verified day/event scheduler. It reads that fact and an explicit eligibility window; it does not infer the choice from prose.
3. Delivery records an owner-approved receipt so callback effects cannot repeat after load or event replay.

If a current scheduler can perform all three, author content and add utilization evidence. If not, describe one bounded extension to an existing owner, with a new save field only after the owner, migration, defaulting, deterministic tick order, duplicate-delivery prevention, and focused test target are approved. Do not build a standalone CallbackManager, delayed quest queue, or second flag ledger from this proposal.

Fallback policy: if the callback window is missed but the story is non-critical, expire it visibly and let the Chronicle omit it. If the callback is necessary to continue a quest, provide a verified alternate channel or delay the objective. If an old save has the choice fact but no delivery receipt, migration must define whether the callback remains eligible; do not reroll its consequences.

### 15.5 Named outcome example

DRAFT example with no assigned catalog ID: At an early shelter arrival, the player accepts a family’s testimony but refuses to call it proof against a faction. Much later, a member returns with a maintenance receipt that supports one part of the testimony and contradicts another. The first branch should preserve two separate facts: testimony was heard; attribution remained unproven. The later content should not convert “heard testimony” into “accused faction.” Its dialogue may say “you left my name off the charge” only if the speaker is the original witness or has a valid report path.

Outcomes:
- Accepted evidence and maintained uncertainty: callback reveals the new receipt; player can amend, keep, or append the record.
- Accepted the accusation without evidence: callback challenges the attribution; the player can correct the record or stand by it; the existing faction/relationship owners decide any effects.
- Refused the initial testimony: no callback may imply the player heard it privately. An alternate public notice or later witness statement is required if the storyline must continue.
- Missed or expired callback: no invented penalty; at most the Chronicle records that the evidence was not revisited, if the Chronicle supports that distinction.

### 15.6 Chronicle and ending boundary

EpilogueChronicleBuilder and EpilogueChronicleCatalog are current source owners for presentation of Chronicle material. Their existence does not mean every new quest fact belongs in the ending matrix. An ending consequence needs a stable canonical input key already accepted by the verdict/ending path, a tested mapping for supported permutations, and prose that does not claim unavailable evidence.

Prefer one Chronicle note keyed to a meaningful outcome class, rather than a different ending for every dialogue wording. Cosmetic tone should not multiply ending combinations. If an outcome is not collected by the existing Chronicle input, keep it as a local/quest/relationship consequence and mark the endgame link as future work requiring a premise audit.

For each proposed new Chronicle line, specify: the source event; when it becomes known; whether absence means “no event,” “unknown,” or “not applicable”; which ending permutations can observe it; existing fallback text; localization impact; and continuity proof. Missing data must not default to the most flattering or punitive conclusion.

### 15.7 Rejection, partial application, and fallback

An effect owner may reject a command because an actor is absent, the location became inaccessible, an item was consumed, or a branch was already terminal. Define each rejection as one of: retry when a named precondition becomes true; show a different available response; close with a failure-forward quest route; or report an integration error during development. Production content must not show “choice applied” until the owner returns success.

If effects span multiple owners, avoid pretending they are atomic unless the existing transaction seam proves it. Prefer a first owner command that emits a fact consumed by others; define repair/replay behavior; persist the event result through the existing save arrangement. Do not create a transaction coordinator only for dialogue. The integration plan must map event ordering in Setup/Save/Flush if host orchestration changes.

### 15.8 Player-facing feedback

Every meaningful response yields visible feedback: changed objective, updated access, revised map marker, altered relationship dialogue, faction standing notice, or a clear journal line. The response copy should state immediate cost before commitment when the cost is known. Delayed effects need a fair hint (“they may remember this”) without revealing future plot detail. Irreversible actions require a final accessible confirmation only when current interaction conventions call for one; do not use the panel as the gameplay authority.

On rejection, preserve the selected response focus and explain the updated condition. On successful effects, ensure reopening the scene does not show an uncommitted option. Journal language distinguishes direct observation, attributed testimony, and player interpretation.

### 15.9 Verification course and scope

The premise audit should inspect current dispatcher behavior, flag semantics, ledger persistence, scheduler/day-tick ordering, DoorEncounter save state, quest event routing, and Chronicle inputs before assigning source paths. The focused verification matrix should cover: first application; repeated command; duplicate event; save before deadline; save on deadline; restore after deadline; old save without receipt; missing character; missed window; player refusal; quest expiry; ending with and without callback; and event replay under the same seed. Verify with the smallest owner tests under TEST_POLICY plus the relevant content-utilization/host selftest only after implementation ownership exists.

This pass leaves all implementation and data authority untouched. It records a route to an auditable feature, names the known seam gap, and does not claim end-to-end integration.

### 15.10 Owner routing matrix

Before drafting an effect, fill this matrix with actual source paths and public methods. The owner examples below are search targets, not a completed wiring map.

| Intended result | First owner to inspect | Required proof before authoring |
|---|---|---|
| Objective advances or fails | QuestRuntimeCoordinator plus specialized quest owner | Exact event/command; duplicate delivery behavior; terminal transition. |
| Item is granted or consumed | Inventory and crafting/economy owner | Item ID exists; amount and failure timing are explicit. |
| Location becomes known or accessible | WastelandMap/Discovery/LocationEvolution owner | Canonical map identity; topology; visibility; save owner. |
| Faction access or standing changes | FactionStanceEngine/current faction owner | Correct faction namespace; threshold/access behavior; journal feedback. |
| Character relationship changes | Current character/relationship owner | Supported relationship fields; affected actor and persistence. |
| Choice fact is recorded | IFlagLedger/CampaignConsequenceLedger or current event owner | Key semantics, deduplication, scope, capture/restore, migration. |
| Delayed callback is delivered | Day/event scheduler and encounter/quest owner | Due semantics, trigger ordering, exactly-once receipt, expired fallback. |
| Chronicle text changes | EpilogueChronicle input/catalog path | Source fact, permutation coverage, missing-data behavior, localization. |
| UI reports result | Existing Host event/read model/panel | Presentation is updated from owner result; no hidden gameplay mutation. |

No row may remain at “some manager” at implementation start. If an owner is absent or inaccessible, narrow the content scope until an approved decision supplies one.

### 15.11 Atomicity and ordering cases

A single response can appear to combine effects that must be ordered. Example: consume a medicine item, advance an escort objective, and raise faction standing. If the item owner rejects consumption, the objective and standing should not still apply. If the item is consumed and a later notification fails, the player must not lose the item and receive no visible outcome. Define the transaction boundary in the existing host/event/save architecture. If the current path cannot coordinate the effects safely, split the player decision into smaller committed steps or ask for an explicit architecture review.

Review these cases:
- Response submitted twice from a slow panel.
- Response accepted, then save occurs before secondary notification.
- Secondary owner absent during load.
- Actor or faction changes between opening and response.
- Item is removed by another system before commit.
- Quest expires on the same day as response.
- Callback fires on the same tick as a new encounter.
- Host retries an event after an exception.
- A duplicate narrative fact already exists from an older content path.
- Chronicle construction happens before a late callback is delivered.

The desired behavior is either one committed result or a clear rejection with no partial side effects. Do not claim atomic behavior without an existing transaction contract.

### 15.12 Delayed callback content variants

For a callback that appears after a long gap, use more than one delivery form only if each remains truthful:

- **Return visit:** the original speaker is available and the location can be reached.
- **Radio report:** the event could plausibly be transmitted, the signal path is valid, and the message is not treated as perfect proof.
- **Written notice:** someone recorded the outcome and the player can physically receive it.
- **Third-party account:** the intermediary has a credible source and states the source.
- **Chronicle-only mention:** the fact is stable, but no live callback scene can be scheduled.

One form should be primary. Alternate forms are recovery channels for unavailable actors or locations, not extra rewards. If there is no verified deferred scheduler, do not encode a fixed day in content and claim delivery. Keep the later response as an ordinary eligible quest/encounter trigger until a scheduling owner is approved.

### 15.13 Consequence summaries for the player

After the owner accepts a response, summarize the result by fact and source:

- “The witness statement was filed as unconfirmed.”
- “The repair request is delayed until a second part is found.”
- “The crew will use the known route.”
- “The faction contact refused the proposed terms.”
- “The callback window has passed; the investigation can continue through the archive.”
- “The Chronicle records that the player preserved conflicting testimony.”

These lines must reflect actual result payloads. If no owner event says the request was delayed, do not claim it. If a faction owner reports no standing change, do not display gratitude as a mechanical effect. For complex events, show immediate result first and journal detail separately.

### 15.14 Consequence precedence and reversibility

Mark each effect as reversible, compensable, or irreversible. A cosmetic line is reversible by presentation refresh. A local scene outcome may be revisited if the encounter owner permits it. A quest terminal state should not be reopened unless the quest owner explicitly supports that transition. A relationship or faction effect may be adjustable by later choices, but history should remain observable. An ending consequence is irreversible once its ending inputs are frozen.

If one choice changes several layers, order player warnings from immediate cost to delayed branch effect. Offer a confirmation step only for established high-impact interaction patterns and ensure cancel returns without mutation. Do not make every dialogue feel like a legal waiver.

### 15.15 Callback and ending interaction

A delayed callback may arrive before the endgame, during the ending transition, or after its facts are frozen. The content contract must state the cutoff. If the callback occurs before Chronicle construction, it may contribute a fact through the current supported input path. If it occurs after the ending is frozen, it cannot retroactively rewrite the prior ending; it may appear as post-ending journal material only if the product supports that. If a save is loaded from before the transition, deterministic replay must yield the same ordered facts.

The epilogue should not reward the player for selecting the “best sounding” dialogue response. It summarizes what happened and what the record can prove. Missing callback data has an explicit neutral/unknown interpretation. A callback that was never eligible is not evidence of indifference.

### 15.16 Consequence test inventory for future implementation

Keep independent tests for lifecycle, persistence, determinism, and routing. Suggested cases for the bounded implementation slice:
1. A valid response applies once and emits one observable result.
2. Repeating its command does not duplicate reward or standing.
3. A response rejected by its first owner applies no dependent consequence.
4. Save/restore after the first fact but before callback preserves eligibility.
5. Save/restore after callback prevents a second effect.
6. A callback outside its allowed time window takes the documented fallback.
7. Old data with no callback receipt follows the approved migration rule.
8. An ending receives a verified fact once and ignores absent/unknown facts correctly.
9. Removing the speaking actor does not invent a substitute outcome.
10. Replaying the same seed and event order yields stable consequence order.
11. UI presentation reflects owner results and maintains focus on rejection.
12. An inaccessible location yields a valid failure-forward route or clear deferral.

These are proposed acceptance cases, not test files created or test runs performed. Select the smallest focused target after the source and ownership audit.

### 15.17 Multi-plan integration seam

The plans hand off in a narrow chain: Plan 19 defines which records are authored or generated; Plan 17 defines which quest owner accepts and tracks the instance; Plan 18 selects a valid location set for an expedition; Plan 20 authors the scene and response text in a supported content format; Plan 21 evaluates conditions from read-only owner facts; Plan 22 routes the accepted command and presents its result. Each boundary has a failure return: invalid record, failed registration, unavailable location, unavailable speaker, stale gate, or rejected consequence. A failure in one plan cannot be silently repaired by writing another plan's state.

For the first integrated slice, use one active quest, one mandatory location, one optional clue, one scene, one gate, one accepted outcome, and one fallback. Keep callback scheduling and ending projection as separate acceptance milestones unless the current scheduler and Chronicle path can prove them already. This sequencing limits the blast radius while retaining an expansion path for later content.


## Pass 16 — Close the evidence-to-memory loop without parallel state

This pass applies the master world bible’s signal, coast, and shelter-memory expansion seeds to consequence routing. It defines observable state changes for future content packages and identifies current boundaries that need an implementation audit. The intended flow remains source record → player observation → interpretation → owner-routed consequence → journal or dialogue acknowledgement.

### Consequence map for the three story families

**Seasonal numbers-station arc.** Hearing, key acquisition, decode, and target reveal already belong to the current cipher-chain owner, which captures and restores its state. Dialogue can ask that owner for current facts and emit a proposed player response. The existing chain should remain the authority for whether the target is revealed and whether the chain resolves. A new quest wrapper may request journal copy or a local relationship effect only through a verified owner; it must not repeat the cipher state machine or grant the map reveal independently.

**Hydrophone-coast mystery.** The hydrophone corpus, anomaly catalog/projection, and discovery manifest are established source landmarks. A selected record can be acknowledged as collected or discovered only through its current discovery/journal consumer. A character’s interpretation is not a new canonical acoustic fact. If an authored branch reveals a hazard, route, or faction stance, identify the respective existing owner first; when none exists, keep the choice local or defer the consequential branch.

**Shelter folklore and cohort callback.** Folklore records have current catalog/discovery references, while CohortSystem and GenerationalLineageExtension own cohort and family-history behavior. The exact callback route joining a specific record to a maturation event remains unproven. Until confirmed, consequence routing may record that a conversation occurred only through the current narrative owner and may present a later campaign-milestone scene. It must not synthesize an age transition or family relationship.

### Effect severity and routing discipline

Classify each proposed response as cosmetic wording, local scene consequence, quest progress, relationship effect, faction access/reputation, world change, or ending-resolution input. The category determines which owner must accept the command. A response option is not evidence that the command succeeded: only the owner’s resulting state can authorize the follow-up acknowledgement.

Keep effects narrow. “Preserve both versions in the journal” can be a journal or discovery operation if an existing command supports it. “Reveal the cipher target” is a map/cipher-owned transition, not a direct dialogue callback. “Warn the coast camp” is a faction or world-state action only if such an owner and contract are verified. “Change the ending” is not available to these expansions by default; the active Plan 145 ending-resolution work is a dependency boundary, and any new ending input must be reviewed there rather than added as a private flag.

### Idempotency, retries, and failure-forward outcomes

Each persistent effect needs a stable source identity and a duplicate-delivery rule. Reopening a dialogue, restoring a save, revisiting a location, or replaying a delayed callback must not grant the same unique reward twice. If an effect is already applied, the player sees an acknowledgement that matches the saved state and the handler returns safely.

An inconclusive result should be a legitimate state, not an accidental half-write. Store only what the current owner needs to distinguish it from unresolved and complete. A failure-forward branch must name its new objective, destination or clue, allowed reward, and terminal condition. It cannot mark the original evidence false unless an authored record explicitly supports that conclusion.

If the target owner is unavailable at runtime or after content migration, preserve the player’s chosen response where the current narrative owner allows it, report the unresolved effect, and expose a recoverable route. Never silently consume the dialogue choice and pretend the world changed.

### Ordering and persistence

Before dialogue opens, compose a read-only context from canonical state. On response selection, validate the context again against current state, then issue one owner-directed command. Apply the owner’s event and save path, refresh the journal/map/dialogue projection, and only then display a durable success acknowledgement. Presentation refresh failure must not roll back a committed domain event, and a failed domain command must not display success.

The final design must confirm event ordering with existing hosts and save owners. Do not add a cross-system transaction coordinator for this trio unless current architecture requires and approves one. Use existing domain events and host adapters; keep any mapping table as documentation until its owners are verified.

### Integration prerequisites and review artifacts

Before implementation, the integrator should attach an effect matrix for each branch: command, owner, idempotency key, event, save/restore route, presentation consumer, retry behavior, and recovery message. Add a conflict table for repeated callbacks and overlapping active quests. Prove that the same selection does not resolve twice across a load/restore cycle. For every world or faction consequence, link the canonical API and active ownership claim.

Plan 145 is an explicit dependency for any ending-level result. Plans 17–21 supply lifecycle, location eligibility, authorship boundaries, graph intent, and read-only context; this plan routes their committed effects but cannot authorize new architecture. Content remains DRAFT until all owner and save seams are confirmed. No production source, JSON, save schema, or test files are changed by this plan continuation.

## Pass 17 — Memorial-to-chronicle consequences after Plan 145 completion

Correction to the prior pass's dependency wording: the live integration ledger now records Plans 141 and 145 as fully integrated and complete on 2026-09-23. Plan 145's UnifiedEndingResolver, epilogue_personalization.json, save section, host lifecycle, campaign-seal hook, and EpiloguePanel consumer are present. New work in this plan should treat that endgame path as a verified existing downstream owner whose public input contract must be inspected, not as an active unfinished package.

### What the current endgame path can observe

Current host code constructs UnifiedEndingContext from campaign state, including total days, living survivors, total deaths, treaty status, faction branch, ending identity, moral band, fates, research-derived shelter upgrades and expedition discoveries, and faction standings. The death count is derived from survivor-fate and memorial counts. Main.Endgame resolves the personalized ending after campaign sealing and places the resulting chronicle prose in the endgame report. This verifies that memorial/fate totals can inform the existing epilogue path. It does not prove that a particular burial-record correction, mourning rite, or oral-lore performance is an accepted endgame input.

Consequently, this expansion may propose a richer chronicle acknowledgement only if the current UnifiedEndingContext, catalog, or a ratified adapter already carries the relevant canonical fact. Do not add a private “burial truth” flag, arbitrary epilogue weight, or second ending resolver. If no current field expresses the player choice, keep the effect local to the quest journal or memorial conversation and file a separate architecture proposal for any needed endgame input.

### Consequence routing for the registry investigation

The archival source remains read-only. A quest response can request an append, correction, or unresolved finding through an existing quest/narrative consequence owner. It cannot rewrite the source JSON or create a second memorial entry. If the chosen action concerns an actual deceased campaign survivor, MemorialSystem is the canonical live record owner; use its existing identity and idempotency rules. A historical archive entry with no current memorial identity remains an archive fact and must not create a campaign death.

A memorial action can be acknowledged after the domain owner succeeds. MemorialSystem exposes OnMemorialized and OnMourned; Mourn rejects missing or already-mourned entries and records a day. The host wires the event to persistence and presentation. A follow-up scene can listen to an existing event only after exact lifecycle and save behavior are verified. It must not replay the rite or grief effect because a dialogue window reopened.

### Consequence levels and ownership

- Cosmetic: wording changes from “unknown” to “second-hand report” in the current scene; no persistent write.
- Local: a speaker acknowledges that the archive was reviewed; use the current narrative or journal owner.
- Quest: the case becomes corroborated, contested, corrected, or unresolved; use the canonical quest/narrative state authority.
- Relationship: a witness chooses to share more or ends the discussion; use the existing relationship owner and preserve alternate routes.
- World: an actual map reveal or destination availability change; route through the map/expedition owner after validating a real site.
- Cultural: first hearing of an existing oral-lore record; use OralLorePerformanceSystem and its saved stable ID. The current owner is explicitly cultural discovery only, so do not attach morale, healing, faction, or route effects.
- Saga: personalized ending prose or a legacy result; use the completed Plan 145 contract. No new input is assumed.

### Ordering, exactly-once behavior, and recovery

Build dialogue context from canonical state. When the player selects an option, validate its source and eligibility again, issue the owner command once, persist through that owner's existing save route, refresh journal/map/epilogue projections, and display the success acknowledgement only after the owner reports success. If a callback is delivered twice, the domain operation must remain idempotent or the adapter must recognize the stable source identity. MemorialSystem already prevents duplicate memorialization for a survivor and Mourn is once per memorial; quest and oral-lore handlers require their own verified duplicate behavior.

A content migration that removes an archive record should preserve an active quest through an explicit alias, equivalent evidence, or an explained delay. If the consequence owner is unavailable, keep the source and player choice legible in the quest's current state where possible; do not display a false success. Failure-forward outcomes should remain specific: unresolved report preserved, witness conversation unavailable, or physical inspection postponed.

### Expanded integration card

For each dialogue choice, document source record, speaker's knowledge channel, command owner, accepted result, save section, event/callback, duplicate rule, UI acknowledgement, failure route, and whether Plan 145 currently consumes the resulting state. Endgame review must compare the proposed effect to the real UnifiedEndingContext and current epilogue catalog. If it cannot point to an existing field or verified downstream projection, its scope ends at the current quest or journal.

### Handoff and scope boundary

This pass is a content and integration proposal only. It corrects stale dependency language in the preceding Plans 22 continuation. It adds no new save section, ending input, memorial field, oral-lore effect, quest status, or dialogue authority. Implementation must follow the current integration queue and ownership ledger; the documentation does not preempt the active Plans 147/148 batch or claim any code path.

### Pass 17B — Outcome cards for a truthful memorial investigation

These cards make the consequence route concrete while respecting the completed Plan 145 surface. They are implementation design examples only; each card must be mapped to a verified command before production.

**Preserve the source.** Player chooses to retain the original burial entry and add an unresolved note. The quest/narrative owner records the resolution once. The journal projection may acknowledge that the account remains unresolved. The source file is not edited at runtime. The endgame resolver is affected only if its current inputs already represent this choice; otherwise there is no saga-level effect.

**Correct a supported field.** Player chooses a correction after obtaining corroborating testimony. The quest owner records the correction and its source. The archival record remains immutable; a second authored or state-backed note carries the correction. If the system has no append-note mechanism, keep the result within the quest state and journal, and request an architecture decision before implementing an archive mutation.

**Perform a mourning rite.** The player opens an actual memorial entry and uses the current MemorialSystem.Mourn operation. On success, its existing event route can update the presentation and save state. The dialogue must handle the already-mourned and unknown-memorial blocked outcomes explicitly. A historical burial record alone is not sufficient to invoke this operation.

**Discover a physical clue.** The player reaches a verified parent destination and resolves its eligible local encounter. The encounter owner records the resolution and depletion if authored. A journal or discovery projection can mention the clue after success. No dialogue path should set map discovery directly when the map owner has not done so.

**Hear an existing song.** A validated producer route causes OralLorePerformanceSystem to mark an existing stable ID heard and the current journal listener adds the first-heard acknowledgement. This remains a cultural-memory outcome. No morale, healing, relationship, faction, or epilogue change is attached by this plan because the live host contract explicitly excludes those effects.

**Close the campaign.** Plan 145 already resolves the unified ending at campaign seal. It can observe the currently constructed campaign context, including death totals and survivor fates. If the plan seeks to personalize the chronicle around a corrected burial record or oral performance, the required fact must be exposed by a current accepted input and validated catalog reference. Until then, the ending remains unchanged and the local quest outcome stays truthful.

### Ordering and failure matrix

The effect owner must return a success or a specific rejection before the UI acknowledges a durable change. A rejected Mourn command leaves the memorial unchanged. A missing micro-location candidate leaves the quest delayed or routes to an alternate clue. An unresolved archive interpretation writes an unresolved state, not a correction. A failed save must be reported through the existing host persistence path; it must not turn a presentation refresh into proof that the domain command committed.

Retries must reuse the same stable source identity. Reloading before a committed command may allow the player to choose again; reloading after commit must show the same result without granting a second reward. If a handler is invoked twice, the canonical owner or adapter must make the operation idempotent. Never rely on a UI button being clicked only once.

### Consequence ownership worksheet

For every proposed player response, complete these columns before implementation: evidence source; active campaign state; read owner; command owner; event/result; save section; journal/codex projection; map/destination projection; duplicate behavior; fallback text; Plan 145 input, if any. Unknown owner fields block promotion. A branch that is intentionally cosmetic should say so and have no hidden persistent side effect.

### No new authority boundary

This outcome design does not add a burial database, archive-editing subsystem, oral-lore effect dispatcher, faction rumor channel, ending resolver, or independent dialogue consequence queue. It identifies interactions among current owners and marks the missing burial loader and any new chronicle input as explicit review work. The first implementation slice, if approved later, should remain the source-loading/utilization bridge and one journal-visible outcome; broader consequences should be staged only after that loop is proven.


## Pass 18 — Consequence routing and clock truth for calibration outcomes

### Consequence envelope

The master world bible's calibration minigame subject should initially be treated as a small side-story whose durable result is more trustworthy measurement, not a large world-state branch. This plan separates six effect classes so writers cannot turn a descriptive line into an unowned state mutation:

1. Cosmetic: change tone or wording only.
2. Local: select a scene response or reveal the current station status.
3. Quest: advance The Needle's Margin only after a successful result from the quest's existing owner.
4. Relationship: optional later effect using the relationship owner and its accepted event route.
5. Faction: out of scope for the first packet; calibration cannot grant standing by itself.
6. World or ending: out of scope for the first packet. Do not add private ending flags or modify the verified ending-context contract.

The calibration system's current completion callback is the authority for its procedure result. A UI button press is only a request. The quest must not complete because the player selected a line, closed the panel, read a record, or found an expedition clue.

### Proposed event sequence

1. Panel or dialogue requests StartCalibration for a registered device.
2. The calibration owner re-checks device condition and station occupancy.
3. If start succeeds, the owner emits its existing started/state signals. The host reports the reservation and due day truthfully.
4. The campaign advances through its current clock owner. The panel must read that authoritative day when refreshing.
5. At or after the due day, the player explicitly requests completion. The owner validates the reservation, updates quality and uncertainty, resets the calibration reading count and emits completion/state signals.
6. The current host/event seam forwards one completion fact to the quest owner. It should carry device identity and the stable procedure instance or equivalent idempotency key only if the existing event pattern supports it.
7. The quest updates once, then dialogue and journal presentation read the new quest and device state.

The panel code reviewed in this pass calls completion with currentDay + 1 when the player presses the button while the station is occupied. That makes the button act as a synthetic day advance in the reviewed path. Treat this as an integration finding that must be rechecked against the full runtime binding before implementation. A campaign panel must not advance time by changing an argument. It should use the real simulation day and leave early completion blocked. Do not write a corrective code patch as part of this plan-only pass.

### State and persistence boundaries

DosimeterCalibrationSystem already exposes CaptureState and RestoreState for per-device condition and in-progress reservation data. Extend or register nothing until the actual save owner is verified. If the live campaign does not currently persist this system, the implementation phase must follow the established save-section owner and include an in-progress round trip; a parallel calibration save store is prohibited.

Quest outcome, first-heard dialogue, discoveries, Chronicle entries, relationship changes, faction standing, true dose, and cumulative booked dose belong to their existing owners. The calibration quest may reference an outcome or device id, but should not duplicate those values. A restored procedure must not emit a second completion consequence simply because the UI refreshes or host setup replays state.

The constant TestSourceExposureMsv is defined in the calibration system, but source search found no operational use. No consequence should consume survivor health, radiation dose, inventory, or ledger value from that constant until its semantics and owner are established. Keep the minimum viable procedure free of implied test-source exposure costs.

### Failure matrix

- Start rejected for unknown device: no consequence; explain the missing device and preserve quest state.
- Start rejected for low battery or sensor condition: set/retain Blocked through the quest owner only if the quest was accepted; show the actual maintenance route.
- Start rejected for occupied station: show the known due day and return later.
- Completion requested early: no completion event, no reward, no quest progress.
- Cancellation: emit failure/cancel information once, keep historical readings, and offer restart.
- Duplicate completion notification: ignore the duplicate at the integration seam; do not double-pay or double-advance the quest.
- Save/load mid-procedure: retain reservation and due day; no premature event during restore.
- Quest abandoned: the calibration owner remains truthful; quest abandonment does not cancel an independent procedure unless the player separately invokes the owning cancel command.
- Expedition clue unavailable: keep the side quest viable without map selection or remote content.

### Handoff and verification contract

A future implementation claim must identify the calibration state owner, live-day source, panel host session, quest lifecycle owner, current save section, and event bridge. It must claim exact files before touching them and use focused verification on the directly affected routes. Suggested checks include start preconditions, due-day boundary, cancel behavior, capture/restore, exactly-once quest advancement, seeded replay if a skill layer is later added, and UI refresh with truthful status. These are design gates, not tests run in this documentation pass.

Completion is accepted only when the observable panel, quest journal, and dialogue agree on the same device state and due day; the quest advances from a real calibration completion; the true-dose ledger remains unchanged by calibration; save/restore preserves an in-progress reservation; and every effect has one current owner. This pass makes no claim that any of those integrations have been implemented.


## Pass 19 — One evidence route, explicit admission, and correction semantics

### Current consequence owners

The existing Verdict system already owns a save-backed EvidenceLedger and a Reckoning count. VerdictEvidenceChain subscribes to MachineLogSystem.OnEntryRead, enrolls its evidence tag once, and increments Reckoning only when the ledger accepts that ID. VerdictHostSession also has an item path that enrolls authored Verdict items when enrolled_evidence is positive. The current wiretap catalog is not connected to either path. The Standing Record engine separately owns location layout, location memory, and site encounters; its name does not make it the owner for admissible documents.

The consequence design should extend the existing Verdict producer/consumer seam if and only if documentary evidence is approved. It must not introduce a second evidence ledger, a shadow standing count, a direct panel mutation, or a new ending counter.

### Status ladder

Represent these meanings distinctly in content and UI, using existing owners where they exist:

- Available: the authored record exists in data.
- Discovered: the player learned that a record exists.
- Read: the player opened or listened to it.
- Investigated: the quest compared it with another source or documented its limits.
- Submitted: the player asked for it to be considered.
- Admitted: the existing evidence owner accepted its stable ID.
- Sealed: the player chose not to make the record public, subject to an existing privacy contract.
- Corrected: a later source changes the case summary or adds a contradiction.

Only Admitted changes the canonical Verdict count. If the current game does not model Submitted separately, introduce it only through the current quest/consequence owner after path audit. A display badge must not serve as mutable gameplay authority.

### Event and effect routing

Proposed sequence:
1. The narrative loader supplies an immutable transcript record.
2. The existing content presentation owner reports a real player read/discovery action.
3. The quest owner opens the investigation and records branch progress.
4. A player who chooses submission receives a consequence preview naming the source and uncertainty.
5. The owning Verdict evidence bridge validates a registered stable ID and accepts it through EvidenceLedger.
6. Only a successful first enrollment increments the existing Reckoning evidence count.
7. The quest owner marks the submission milestone; the journal and dialogue read the resulting state.

Use stable source and evidence IDs, not the transcript prose or a generated hash. The bridge should have an idempotency rule and report whether it enrolled, rejected, or had already enrolled the record. On restore, reconciliation must not double-increment the count. If the current producer contract cannot accept this event without widening ownership, pause for an architecture decision rather than expose Reckoning to a UI panel.

### The ending-count gate

Because enrolled evidence count is an input to Verdict endings, making wiretap records admissible changes more than a codex page. Before any transcript can be enrolled, classify whether it is:
- informational clue that never affects the evidence count;
- eligible evidence that requires corroboration;
- eligible evidence whose value is explicitly one existing evidence unit;
- inadmissible or sealed content.

The content owner and balance owner must review this classification. Do not add multiple evidence rows for a single underlying incident simply because it appears as a transcript, quest item, dialogue callback, and Chronicle entry. A canonical incident-to-evidence mapping should deduplicate those representations. If a single record produces multiple independent claims, each requires separate authored identity and acceptance criteria.

### Consequence classes and examples

Cosmetic: choose whether the clerk says “intercept” or “record.”
Local: keep the transcript private at the shelter desk.
Quest: mark the evidence-comparison step complete after a valid record is read.
Relationship: a survivor appreciates that the player withheld an unverified name; use only an existing relationship owner.
Faction: the Office reacts to a public accusation only when its current faction event route supports it.
World: no supplies, routes, or site ownership change in the minimum packet.
Ending: the admission may contribute to the existing evidence count only after the endgame contract and threshold impact are reviewed.

The correction branch is a local case correction, not deletion from the monotonic ledger. If admitted evidence is later challenged, retain the original source and add a separate contradiction or correction through a supported evidence model. The final UI can state that the record remains in the archive but is disputed. No silent withdrawal is allowed.

### Failure and save cases

- Transcript loaded but no consumer: no quest or effect.
- Read event fires twice: no duplicate evidence enrollment.
- Submission uses unknown ID: reject with a clear status and no Reckoning update.
- Player chooses private preservation: record the quest choice only if a current owner supports it; otherwise treat it as local dialogue.
- Save restored after submission: re-read the ledger and quest state; no second enrollment or payout.
- Later contradiction arrives: preserve the admitted source and add a distinct correction.
- Quest abandoned: do not retract already admitted evidence or alter an independent Verdict state.
- Content is unloaded or invalid: retain any previously saved canonical evidence ID and render a safe unavailable-source label.

### Integration acceptance

Before implementation, verify catalog reachability, current player-read state, item/evidence registration, save ownership, Verdict ending thresholds, and the current link between EvidenceLedger enrollment and Reckoning. Claim the exact owned files through current governance and verify one user action end to end. Acceptance requires no parallel state, no automatic enrollment on load, exactly-once count change, no loss of evidence during restore, legible attribution, and an explicit non-admission route. This proposal does not change the Verdict ending contract and does not claim that wiretap evidence is currently integrated.


### Pass 19B — Exactly-once result and correction protocol

The wiretap feature is safe to integrate only when its full event path has an observable, idempotent result. The proposed contract reuses existing ownership:

| Event | Owner | Required result |
|---|---|---|
| Static transcript parsed | Narrative catalog loader | Immutable record available to a real consumer |
| Player opens or listens | Verified discovery/presentation owner | One player-read fact; no Verdict count change |
| Quest accepts investigation | Current quest owner | One quest instance or supported existing quest progression |
| Player requests submission | Quest/dialogue command seam | Confirmation preview; no mutation until accepted |
| Evidence ID enrolled | Verdict EvidenceLedger through its chain | One unique ID; repeated request returns already-enrolled |
| Reckoning count changes | Existing evidence chain | Increment only after the first accepted enrollment |
| Correction or contradiction added | Existing evidence/effect owner, after premise audit | Preserve the original and add a distinct correction reference |
| Campaign save captured | Current Verdict/quest/discovery save owners | Restore the same admission and branch status |

If one source action must both create quest progress and admit evidence, order the effects so that the canonical enrollment result is obtained first. Then the quest acknowledges success. If the quest saves a pending-submission state before the ledger call and the call fails, it remains pending and can retry without double-counting. If enrollment succeeds but quest advancement fails, restore/reconcile should observe the existing ledger ID and complete the missing quest acknowledgment without incrementing Reckoning again.

A successful evidence result should contain only what the current contract needs: stable evidence ID, whether newly enrolled or previously enrolled, and the canonical consequence outcome. Do not copy transcript text, speaker identities, clarity score, or faction standing into an effect message. UI feedback may summarize the source using the document catalog.

### Contradiction and withdrawal

The EvidenceLedger is one-way and idempotent; therefore “withdraw” cannot mean erase. The player can withdraw a public accusation or add a correction only if a current case or faction system represents that state. The original evidence remains enrolled in the canonical record. If no correction owner exists, the MVP should allow the player to mark the case unresolved before admission and should not offer a false post-admission reversal.

An endgame view may present a disputed record with its correction alongside it. The evaluator still receives only the accepted canonical evidence count and current supported state. Do not add a hidden negative evidence count, subtract from enrolled count, or write a private ending flag.

### Failure routing and feedback copy

- Read accepted, evidence source unsupported: “The transcript is preserved as a lead. The register cannot admit this source yet.” The quest remains investigable.
- Admission rejected for unknown ID: name the missing source link in diagnostics and tell the player the record was not counted.
- Duplicate admission: show “already recorded”; no repeated sound cue that suggests a new reward.
- New contradiction found: “The record remains. The claim now has a documented dispute.” Only show this after the correction owner confirms it.
- Save restored with admission complete but quest pending: reconcile the quest milestone once, with no duplicate Reckoning event.
- Save restored with quest complete but ledger missing: do not infer admission. Show a pending or inconsistent state and route through the owner’s recovery contract.
- Source corpus unavailable after a game update: retain the canonical evidence ID and show a generic unavailable-source entry; never remove it from the save.

### Acceptance and rollback

A later implementation should first run a focused rehearsal with one transcript and one existing evidence entry. Confirm that all unauthorized routes (catalog parse, Codex display, repeated read, journal refresh) leave the count unchanged. Confirm that the single explicit admission increments once, persists, restores, and changes the Verdict readout only through the current chain. Then verify one rejected source and one duplicate request.

Rollback may remove a new presentation or admission affordance, but it must preserve already saved EvidenceLedger IDs and quest state. If the catalog reference is removed after release, provide an archived-source fallback that names the missing document without making the save invalid. No rollback can rewrite history by deleting an admitted fact.

This protocol is a design proposal. Do not alter ending thresholds, EvidenceLedger semantics, or the Verdict save schema until current evidence, package ownership, and a narrow acceptance plan authorize that work.


## Pass 20A — Consequence routing for authored radio-theater choices (DRAFT)

### Boundary and player promise

The new world-bible subject is a Machine tribunal radio-theater series, not permission to make every broadcast mutate the world. VerdictRadioSystem schedules authored corpus entries and publishes radio.verdict.broadcast. Plan 94 has already expanded that corpus to 30 broadcasts. RadioProgramProductionSystem owns program-preparation jobs and follow-up hooks; it explicitly leaves scheduling, reception, and propaganda pressure outside its authority. The theater’s story choices therefore need a confirmed player action and a reviewed effect route. A scheduled or merely surfaced broadcast produces no relationship, faction, morale, evidence, quest, or ending consequence by itself.

The Quiet Hours arc offers three responses after the witness conversation: prepare a private correction, prepare a public correction with explicit consent, or decline intervention. The response click is not delivery. It records an intended quest outcome only through the current quest authority. A later program or scene may deliver that outcome if the existing radio owner reports successful delivery. If no current delivery surface can express the correction, the story should resolve in a face-to-face scene rather than pretend the radio program was aired.

### Consequence tiers and owners

- Cosmetic wording: dialogue changes a line’s tone or recap. It can be derived from the quest outcome and need not create a new persistent flag.
- Local scene: the performer changes a script page or the witness acknowledges privacy. Route through the owning quest/dialogue transition; do not mutate location data from UI.
- Quest: objective completion, delay, partial completion, failure-forward transition, or resolution. The existing quest owner validates allowed transitions and saves the canonical result.
- Relationship: a character’s response changes only if the current relationship owner exposes a command for that consequence. Never write an affinity number from the response node.
- Faction: no faction reputation change is required by the core arc. If later approved, use the existing faction authority and only after actual audience delivery and content review. A public correction is not equivalent to generic propaganda success.
- World: do not create, remove, or restock locations based on this arc. Any future world change requires an explicit owner and replay-safe event contract.
- Ending: this local story does not alter the campaign ending or Machine verdict. Keep major-resolution consequences outside the slice.

### Command/effect order

On response selection, revalidate the node’s conditions against current quest and consent state. Present a confirmation if the response publicly identifies a witness or otherwise has an irreversible audience. On confirm, issue one typed command to the owning quest host/session. That owner validates the transition and returns success or a specific blocker. Only after success should the UI show the updated line and refresh the journal/map projections from their owners. A cancellation leaves state unchanged. A blocker retains the current node and explains the reason; it must not optimistically display a choice as completed.

A repeated click, double input, panel re-open, event retry, or save/load between command and presentation must not apply the same consequence twice. Prefer the existing objective or event identity as the idempotency key. Do not introduce a second effect ledger solely for the theater. If the current host contract cannot make a command idempotent, that is an integration blocker to resolve under the existing owner before authoring multiple consequence nodes.

### Failure and rollback

If an authored response references a missing objective or effect recipient, disable the response only when it is optional and display a valid alternative; a mandatory node with no valid response is a content-build failure. If public delivery fails because the station is unavailable, the quest remains pending with an explicit retry or face-to-face fallback. It must not award delivery, audience response, or reputation. If the player abandons after committing a private correction but before it is delivered, preserve the intent and offer a deliberate cancel or resume route; do not erase a witness’s disclosure choice through reset.

## Pass 20B — Consequence table and integration acceptance

| Player action | Immediate owner | Observable result | Persistence requirement | No-effect condition |
|---|---|---|---|---|
| Ask about the staged case | Dialogue presentation | Optional information line | None unless current dialogue owner already tracks exposure | The line is merely viewed; no quest auto-accept |
| Accept the investigation | Current quest owner | Journal/objective appears as In Progress | Existing quest save/restore | Invalid or unavailable entry route |
| Ask to identify the witness | Quest/consent contract | Player sees clear disclosure options | Persisted where current quest state is saved | Back/cancel returns without mutation |
| Choose private correction | Current quest owner | Follow-up is pending with private scope | Stable selected outcome | No delivery or relationship gain yet |
| Choose public correction | Quest owner, then verified radio delivery surface | Confirmation, then pending public delivery | Idempotent intent plus delivery outcome under existing owners | Consent absent; explicit confirmation canceled; delivery unavailable |
| Decline | Current quest owner or authored dialogue exit | Quest stays available or resolves as declined according to packet | Only if the current quest model distinguishes this outcome | Dialogue window closed without selecting decline |
| Follow-up actually delivered | Current radio/reception owner plus quest completion route | New authored line; objective completes/resolves | Persisted delivery/result through existing owners | Scheduler fired but no player-facing delivery occurred |

The implementation package should prove the player-facing chain from content to observable result: authored response ID resolves; condition evaluation succeeds; command reaches one current owner; that owner reports accepted or blocked; save capture contains the intended canonical state; restore yields the same response availability; and a visible surface reflects the new state. A valid broadcast event alone is only one step in that chain.

Build a focused consequence matrix for private correction, public correction, silence, lost anonymity, missing relay, interrupted interaction, duplicate response, old save, and absent optional episode. For each case, record the expected quest status, delivery status, character line, map marker, journal statement, and whether any relation/faction/world scalar changes. The default for an unapproved effect is no change. This is safer and easier to audit than a generic “apply effects” callback capable of mutating arbitrary game state.

Performance should stay bounded: condition checks occur when entering or refreshing a dialogue node, never every frame; references are resolved at catalog load or through current indexed owners; response effects are small typed commands. Avoid scanning the full radio corpus during every dialogue refresh or copying all world state into a dialogue context object. The UI should request a narrow context snapshot through an existing adapter if one exists; if not, list that as a required architecture seam rather than fabricating a broad cache.

The final acceptance decision belongs to the named integrator after current-source verification. Required receipts: exact owner paths, data/schema changes if any, migration behavior, direct consumer path, focused tests selected under TEST_POLICY, and a handoff using AI_AGENT_WORKFLOW. This document remains a design proposal; it does not claim implementation, save support, or path ownership.



## Pass 20C — Integration sequence and consequence verification worksheet

### Dependency sequence

1. **Confirm source surfaces.** Verify the current Verdict catalog loader, its event subscribers, the radio panel/log surface, the quest host/session, the conversation loader, the location selector, and the save owners. The completion of Plans 94 and 173 is baseline evidence, not permission to assume every downstream UI path still exists.
2. **Agree on meaning.** The narrative author defines exactly what counts as an episode being scheduled, surfaced, reviewed, corrected, and delivered. The radio and quest owners approve the terms they can truthfully report.
3. **Lock the graph.** Dialogue nodes reference stable quest and effect IDs. Plan 21 owns condition semantics; this plan does not invent a second condition evaluator.
4. **Route a vertical slice.** One response reaches the current quest owner, receives an accepted/blocked result, persists through its current path, and appears in the next scene. Add public delivery only after the existing radio interface can represent it honestly.
5. **Expand outcomes.** Add private correction and silence, then public correction with consent. Do not implement arbitrary effects or faction consequences as generic strings.
6. **Close handoff.** Record changed paths, migration, focused tests, limitations, and intentionally untouched shared owners through AI_AGENT_WORKFLOW.

### Observable behavior table

| Checkpoint | Expected truth | Player feedback | Persisted authority |
|---|---|---|---|
| Broadcast scheduled but not surfaced | Episode is eligible or fired in the radio owner only | No “you listened” statement | Verdict radio state |
| Episode surfaced but not reviewed | A readable/listenable item is available | Offer review; no quest side effect on opening panel | Existing presentation/reception owner, if present |
| Quest accepted | Player explicitly commits | New objective appears once | Existing quest owner |
| Public option selected but canceled | No consequence applied | Return to dialogue with prior state | No mutation |
| Public option confirmed, station unavailable | Intended outcome is pending or visibly blocked | Explain retry or offer authored face-to-face route | Quest owner stores intent only if supported |
| Correction delivered | Delivery is confirmed by current radio/reception owner | Follow-up text appears and quest can resolve | Existing delivery owner plus quest owner |
| Save restored after effect | Same selected outcome and no duplicate application | Follow-up line remains stable | Existing capture/restore owners |

### Rollback and observability

If dialogue content ships before the optional radio delivery path, preserve a face-to-face ending and keep the new broadcast reference dormant. If a referenced effect ID is removed, catalog validation should fail in development and the player-facing graph should use its approved fallback in a compatible content revision. Do not catch and discard unknown effects silently. If an old save contains a selected choice but no new delivery field, apply the documented migration default and show a truthful unresolved/closed recap; never infer public delivery from the selected choice alone.

Operational logs should report content IDs and failure codes, not full personal testimony. Debug detail can identify an unresolved node reference, invalid transition, missing recipient, or duplicate command. Player-facing copy should say what is missing in plain language. Logs should not turn private narrative text into telemetry or invent a new analytics pipeline.

### Focused acceptance list

- A passive scheduler poll causes no quest, relationship, faction, or evidence change.
- Opening and closing a dialogue panel causes no state mutation.
- Each response is validated against current state at commit time.
- Canceled confirmation produces no effect.
- A duplicate command is ignored or safely rejected by the existing owner.
- A blocked delivery does not count as delivered and has a visible recovery route.
- Save/restore between choice and follow-up preserves exactly one selected outcome.
- The journal, map, and dialogue recap agree with the canonical quest and location state.
- No ending or Reckoning outcome changes from this local story without a separately approved design.

These checks should reuse current focused test owners and stay within TEST_POLICY. This plan does not call for a full-suite run. If the feature has no current test that can observe the true command route, first verify the gap and ask the named foreman to assign a bounded integration package; do not create a parallel harness or revise the save architecture inside a narrative patch.


## Pass 20D — Optional follow-up consequence map

The Second Margin must not turn disagreement into a hidden success meter. Its valid outcomes are local and descriptive: the performer prepares an explanatory note; the listener understands the intended scope but keeps a different impression; the player declines to continue; or the optional conversation remains available for later. None automatically changes faction standing, morale, trust, Verdict evidence, or the campaign ending.

| Choice | Route | Persisted fact | Resulting presentation |
|---|---|---|---|
| Ask what sounded accusatory | Dialogue read | None unless exposure is already owned | Listener explains their interpretation |
| Ask performer to explain production context | Dialogue read | None | Performer explains intent without proving audience response |
| Prepare an added note | Quest command to an approved owner, if one exists | Prepared-not-delivered outcome | UI says the note is ready; no audience response |
| Confirm a valid delivery | Existing radio/reception owner, if it can perform this action | Delivery result from that owner | Follow-up reaction becomes available only after success |
| Close the optional subject | Optional quest/dialogue transition | Declined or resolved status if supported | Neutral closure; parent ending unchanged |

If no current radio program command can deliver a textual correction, the minimum implementation should end at “prepared” and stage the next scene in person. It must not repurpose RadioProgramProductionSystem as a script editor or assume a template ID can carry arbitrary episode prose. If a future package routes through program production, it must verify that the authored template, slot, delivery callback, and follow-up hook can represent this content without changing the existing scheduler contract.

Failure rules: an absent listener hides the optional scene; an absent performer leaves a truthful prepared note or delays the conversation; an invalid parent outcome blocks the optional node; a failed delivery leaves the reaction unavailable; duplicate delivery results resolve idempotently; and abandoning the side quest does not reset the parent. Any unresolved mandatory reference is a content integration failure, not an invitation for a broad fallback effect.

The scene’s implementation should use a typed response outcome owned by the quest/dialogue seam and a narrow optional delivery command only after the radio owner confirms its contract. Do not pass arbitrary effect strings such as “increase credibility” to a generic UI callback. If an event is emitted, it reports a fact like correction prepared/delivered; current observers decide their own effects. The radio theater remains an authored content feature, not a proxy for propaganda.

Acceptance includes a negative assertion that resolving the optional scene leaves all unrelated ledgers unchanged. Specifically, a reviewed test should verify no change to evidence count, relationship value, faction standing, morale, ending flag, map-discovery bit, or resource inventory unless a distinct, approved owner contract says otherwise. Also verify the positive local behavior: the performer’s next line reflects the chosen explanation, the listener’s interpretation persists or is intentionally not persisted, and the parent quest’s terminal outcome remains unchanged. This explicit no-effect surface limits scope and makes integration safer.


## Pass 20E — Consequence ordering, feedback states, and defect taxonomy

### Command outcomes

Every player response that mutates state should return one of four observable outcomes through its current owner: Accepted, Blocked with a retry condition, Rejected as invalid/stale, or NoChange for a deliberate close/read action. The dialogue presentation must map each outcome to truthful feedback. Accepted refreshes from canonical state. Blocked keeps the player at a useful node and states what must change. Rejected reports that the scene changed and refreshes valid options. NoChange closes or reconverges without implying a transaction. A generic success toast is insufficient when the player is deciding who may be named publicly.

Avoid cross-owner partial application. Public correction should not update the quest to Resolved before a delivery owner confirms the correction reached its audience. If delivery and quest resolution are separate systems, use the existing event/host sequence and an idempotent result contract; do not implement a two-phase transaction manager for this one story. If no such contract exists, keep the ending local to the quest owner and use a face-to-face scene. This is an explicit scope choice, not a missing UI trick.

### Event ordering and duplicate handling

A radio scheduler event can be delivered before or after a player interaction in the same campaign day. The feature must define which facts are valid at response commit time. The player’s response should use the authoritative current snapshot; stale UI presentation triggers a refresh. A duplicate scheduler event remains suppressed by its existing fired-ID state. A duplicate player response is rejected or idempotently accepted by the quest owner. A duplicate delivery callback cannot create a second audience reaction or award a second outcome.

If an event subscriber is absent, the developer diagnostic should identify the missing route. Do not add an implicit subscriber in a panel. If the radio event is observed by more than one legitimate listener, each listener owns only its own state: radio presentation can surface text, quest logic can offer a lead if the user-facing trigger is confirmed, and analytics or chronicle surfaces must not claim delivery from schedule eligibility.

### Consequence defect taxonomy

- **Ghost outcome:** UI says a correction aired while only a preparation job reached Ready. Fix: use the delivery owner’s confirmed result.
- **Double outcome:** repeated click or callback applies relationship/reputation twice. Fix: existing command identity/idempotence.
- **Silent lock:** an unavailable location leaves the response disabled with no explanation. Fix: visible delayed/fallback route.
- **False memory:** dialogue says “you heard” because the system fired. Fix: use exposure owner or neutral wording.
- **Evidence inflation:** staged text is enrolled as evidence on catalog load. Fix: preserve the VerdictEvidenceChain’s current eligibility/enrollment contract.
- **Cross-story bleed:** optional audience response changes the parent ending. Fix: keep outcome scopes independent.
- **Rollback loss:** correcting or removing a content row erases a resolved save fact. Fix: stable IDs and reviewed migration/legacy recap.
- **Panel authority:** UI writes quest, map, or reputation fields directly. Fix: route through current host/command owner.

### Final review checklist

A reviewer should trace one outcome from authored response ID to condition, command, owning state mutation, save capture/restore, subsequent event, dialogue recap, map/journal projection, and focused test. Then trace a no-op and a failure route. Check that all side effects are explicit, no content-load event mutates gameplay state, passive polling is harmless, and the UI can report when the radio delivery path does not exist. Performance stays bounded by node-level context refresh and indexed catalog references; no per-frame world scan or new cache is justified.

The release package should separate the core outcome from future layers: Core may ship the static staged case and one private resolution; Expansion may add public correction delivery, audience response, alternate site callbacks, voiced performances, and additional authored interpretations. Every added layer needs its own owner and acceptance evidence. The DRAFT content set is not complete merely because the lines are written; it is complete when every promised response has a truthful, reachable, persistent route or is explicitly labeled noninteractive prose.


## Pass 21A — Folklore-to-belief consequence limits and owner map

### Narrative choice is not automatic political conversion

The player’s keep, annotate, or reply-verse decision changes how this local story is presented. It does not assign a belief profile, convert an adult into a movement, create a new faction, change roommate compatibility, or trigger a psychological breakdown arc. Current code has separate authorities for belief movement definitions, ideological friction, zealotry, and psychological arcs. The content pass must preserve those boundaries and use them only after a specific effect contract is reviewed.

The belief movement catalog defines authored creeds, comfort and blind-spot themes, practices, conflict profiles, and tags. IdeologicalFrictionSystem reads existing belief profile IDs and affects roommate compatibility/affinity. ZealotrySystem references belief movement definitions. These existing mechanics show why a “political position” cannot be an untyped dialogue string: changing a profile can alter shelter relationships and rest outcomes. The MVP uses no such effect. Characters can explain their political view in authored dialogue while their current game profile remains unchanged.

### Effect recipient matrix

| Candidate effect | Current authority to verify | MVP decision | Required condition before expansion |
|---|---|---|---|
| Quest resolved with selected story treatment | Existing quest owner | Use after premise audit | Valid transition and save/restore path |
| Codex annotation displayed | Existing journal/codex owner | Optional; omit if no authored-annotation contract | One canonical text source and old-save behavior |
| New fixed verse available | Existing narrative/education consumer | DRAFT content only | Reachable surface and localization |
| Adult dialogue callback | Dialogue/quest owner | Local outcome | Revisit resolves from canonical quest state |
| Belief profile assignment | Belief/ideology owner | No automatic assignment | Explicit reviewed command, consent/player-world rationale, deterministic save path |
| Faction membership or standing | Existing faction authority | No change | Approved eligibility and consequence contract |
| Psychological arc | Psychology/treatment owners | No change | Independent clinical narrative premise and owner review |
| Ending/legacy score | Endgame owner | No change | Signed endgame consequence scope and verified input contract |

### Command sequence and negative assertions

When a player confirms an outcome, the dialogue layer sends one typed result to the current quest owner. The owner validates that the quest is active, the selected response is allowed, and any required consent is true. On acceptance, the UI refreshes its projection. The codex or education surface receives only an approved content reference. A failed write leaves the current node and outcome unchanged. Repeated confirmation is idempotent under the existing quest command identity.

A focused integration verification for any later implementation should assert that the dialogue choice does not mutate belief_profile_id, IdeologicalFriction affinities, Zealotry followers, psychological-arc state, cohort dose bands, maternity/health, or faction records. It should assert the intended quest outcome and line change. If an approved expansion later adds an ideological consequence, test that consequence independently and ensure it is not inferred from folklore exposure alone.

This arc is a useful story about how political language inherits childhood images, but its point is not that every person exposed to the same rhyme shares one position. Dialogue should show disagreement among characters with similar histories and continuity among people whose interpretations change. The player gets to choose the shelter’s official teaching treatment, while each adult retains a perspective. That design makes new content possible without building a new political simulation or flattening existing belief systems into collectible traits.
### Pass 21B — Effect ledger, failure route, and non-effects (DRAFT)

Every branch in this arc should declare the effect category it intends to produce. Wording alone is not a gameplay effect. The content author should select the smallest effect category that makes the player's action observable and understandable.

#### Proposed effect ledger

| Player action | Category | Owning fact | Player-visible feedback |
|---|---|---|---|
| Inspect the margin | quest | clue-inspected step fact | journal records what was actually read |
| Hear an adult's account | quest | witnessed-account fact | dialogue is available for later reference |
| Preserve both accounts | local consequence | annotation choice, if supported | codex or journal shows both versions remain distinct |
| Add a difference note | local consequence | authored annotation ID | visible note says accounts differ without naming a winner |
| Invite a reply verse | quest | invitation accepted or declined | next scene availability is clear |
| Visit optional lamp site | world/location fact | normal location visit fact | map/journal reflects actual visit |
| Decline field trip | cosmetic or local only | none unless a durable choice is needed | conversation closes without penalty |

No row implicitly changes relationship, faction, beliefs, ideology compatibility, stress, clinical status, cohort membership, shelter resources, or ending. If later authored content needs one of those outcomes, it must name its existing system owner and add a separate, reviewed effect contract. One dialogue choice should not fan out into unrelated consequences merely because several systems exist.

#### Failure and alternate resolution

**Unavailable teacher:** keep the marked page as a lead; a journal note may say the conversation is pending. A later shelter visit can resume it. If the character is permanently gone, offer an authored archive route only when a real archive interaction exists.

**Only one adult account heard:** allow the player to preserve the single account with an explicit “one account heard” label. Do not fabricate consensus or fill the absent voice with generated dialogue.

**Optional location absent:** resolve the core quest through the shelter scene. A site-specific branch remains unavailable and is not counted as failed or visited.

**Expedition ends before clue inspection:** retain the discovered location if the player legitimately discovered it, but leave the clue uninspected. If the location was only selected and never entered, do not create discovery state.

**Player abandons the quest:** keep already witnessed codex and dialogue facts. Do not roll them back. Resume only through the current quest owner's explicit reopen behavior.

#### Consequence-category checklist

Cosmetic wording can vary by tone or character voice. A local consequence changes the current scene or its annotation. A quest consequence advances or blocks an objective. Relationship consequences require the relationship owner. Faction consequences require the faction reputation/access owner. World consequences require a named location, resource, enemy, or event owner. Ending consequences require the campaign-ending authority and a stated prerequisite. The folklore arc's minimum viable version uses cosmetic and quest effects, with at most one local annotation. It has no ending consequence.

#### Review questions for each authored response

Does the response promise a specific effect? Is that effect owned by the named system? Can the player observe whether it happened? Does a failed availability condition have a truthful alternate line? Is the same consequence applied exactly once after restore or repeated interaction? Can the branch reconverge without erasing a durable choice? Does the response leave room for two characters to remember the same tradition differently? Any unowned effect is removed from the content proposal until an architecture decision assigns it.
### Pass 22A — Consequence routing for the unsuccessful-return arc (DRAFT)

The arc proposes a richer aftermath, but the first playable slice should route only facts that are already observable and owned. The master world bible's question about world state, rumors, and standing is a subject prompt; it is not authorization to mutate three systems on every unsuccessful expedition.

#### Baseline routing table

| Trigger | Proposed output | Authority | Required evidence | Default when unavailable |
|---|---|---|---|---|
| Completed return with unresolved objective | debrief becomes available | quest/dialogue owner after premise audit | canonical return plus linked active objective | keep existing expedition completion feedback only |
| Player records last confirmed landmark | quest-local evidence fact | quest owner | player confirms a specific source fact | preserve the unresolved objective |
| Player marks the route inconclusive | quest resolution class | quest owner | return fact; no proof predicate satisfied | leave pending if state cannot be saved |
| Player visits an optional clue site | location discovery/visit | expedition/location owner | actual entry or discovery event | do not show site as visited |
| Player authorizes a public account | rumor creation, if supported | RumorSystem adapter / information-flow owner | sourced report and approved rumor contract | keep the report private in the journal |
| Faction receives a report | standing/access response, if supported | faction standing owner | explicit faction source, valid action, approved rule | no standing mutation |
| Journal projects the outcome | read-only summary | current journal/codex projection | durable quest facts | no duplicate journal store |

#### Minimum viable consequence set

The minimum release slice has one quest result class (inconclusive), one witnessed debrief, an optional follow-up lead, and a journal summary generated from the quest's durable facts if the existing projection supports it. It does not create rumors or modify faction standing. A later expansion may add a player-confirmed rumor report and a faction-specific response as separate feature slices, each with its own owner, save path, feedback, and rollback behavior.

#### Exactly-once and replay requirements

The same completed expedition may trigger at most one debrief-availability transition for its linked quest. Reopening a panel cannot duplicate a journal entry or create repeated rumor records. If the player loads after choosing “record inconclusive,” the same conclusion remains and the response is not applied again. Idempotency keys should derive from stable quest/expedition/effect identifiers through an approved owner; do not use wall-clock timestamps, random IDs, or hash iteration order in Core. If no effect-dispatch contract supports this, keep the choice local and non-mutating.

#### Failure, compensation, and rollback

If the expedition completes but the quest owner is absent, do not discard the completed expedition; preserve current completion behavior and leave the quest unresolved for later reconciliation. If a rumor adapter rejects an authored report, keep the player's private journal choice and state that it was not sent. If standing or world-state mutation fails, do not claim it succeeded in dialogue. Roll back only the attempted effect through its owner; never roll back the expedition result or previously witnessed facts.

#### Observable acceptance and integration order

1. Reconfirm retreat/completion/failure semantics and the exact return fact.
2. Confirm the current quest owner can correlate its active objective to the expedition ID without a new parallel registry.
3. Define the result predicate for success, disproved, and inconclusive outcomes.
4. Wire the smallest debrief event through the existing host seam, with unsubscribe/lifecycle behavior.
5. Add journal projection only through its current read-model owner.
6. Consider rumor publication only after source attribution and uncertainty classification are supported.
7. Consider faction response only after an explicit standing rule and feedback path are approved.
8. Verify save/restore, duplicate delivery, UI close/reopen, and deterministic replay under the package-specific test policy.

The current source facts establish neighboring owners and host events, but do not prove steps 2–7 already exist. This section proposes a dependency order; it does not claim an integration gap is approved for implementation.
### Pass 22B — Bridge lifecycle, performance, and rollback gates (DRAFT)

Any event bridge from expedition return to quest debrief should have explicit subscription ownership. ExpeditionHostSession already subscribes to ExpeditionSystem completion/failure events for current presentation feedback. A future bridge must be attached through an owned host/session lifecycle, unsubscribe or be reconstructed safely on teardown, and avoid adding a second listener after save restore. It must not put narrative decisions in a panel callback.

#### Integration seam checklist

1. Identify the canonical quest runtime and the existing way it receives expedition/objective facts.
2. Confirm whether the expedition completion payload contains a stable expedition ID, destination, actual visit facts, and objective-relevant evidence.
3. If a payload lacks a needed fact, ask the source owner to expose a fact through its established event; do not read private mutable internals from a new adapter.
4. Let the quest owner evaluate the objective predicate and create a debrief-available fact.
5. Let the dialogue UI query this fact and emit a player command through its existing route.
6. Let journal, rumor, map, and faction adapters consume their own approved events independently.
7. On teardown/restore, ensure the event is neither lost nor delivered twice.

#### Resource and performance limits

The return bridge should process once per completed event, not poll every frame. It should avoid scanning all quest definitions and all rumors on each journal refresh. Resolve only active quest bindings through existing indexes. Keep diagnostic breadcrumbs compact and bounded; do not embed full prose in expedition state. Unknown or inactive quest references should no-op with a diagnostic or the project’s established integrity behavior, not throw during a normal return.

#### Rollback and partial integration

The bridge can be rolled back while preserving expedition completion. If the quest integration fails, the expedition remains completed under its existing owner. If journal rendering fails, the quest result remains queryable through the quest owner. If rumor propagation fails, no public report is claimed. If a faction adapter fails, no standing delta is claimed. The UI must never optimistically announce cross-system success before the owner confirms it.

#### Handoff and bounded verification

Implementation readiness requires a package owner, exact path claims, a premise note, focused verification target, save implications, and an acceptance handoff. Focused cases should distinguish retreat then inbound completion from terminal Failed; objective fulfilled from unresolved; destination selected from destination visited; one event from duplicate delivery; and restore-before-debrief from debrief-before-save. Do not run broad suites by default. The documentation addition itself needs only a whitespace check and measured index update.
### Pass 23A — Ambient information effects and consequence firewall (DRAFT)

The ambient-rumor idea must stay informational. Its purpose is to let a believable community discuss uncertain sounds without turning every utterance into a hidden quest, threat, or faction event. Because RumorSystem currently uses numeric truthfulness both for decay and report classification, ambient-only entries need a reviewed representation before they can safely share its generation and briefing paths.

#### Consequence class map

**Cosmetic:** alternate wording that does not alter facts. **Local:** a private note or current-scene annotation, if the dialogue/quest owner supports it. **Quest:** a bounded investigation result or player-selected “do not circulate” outcome. **Relationship:** none in the minimum slice. **Faction:** none. **World:** none. **Ending:** none. This hierarchy is intentional: the dramatic weight comes from testimony and uncertainty, not a hidden reputation multiplier.

#### Proposed rumor creation contract

Creation is a command, not a side effect of loading catalog data. It should require an explicitly ambient-enabled hub and an authored rumor definition whose provenance class says source-free ambient. It must reject a source-free rumor at a normal hub. The rumor should not point to an invented Event subject ID. Its confidence/verification presentation must be separate from truthfulness decay, or it must be excluded from the current report’s verified threshold. Exact implementation may require a schema/save decision; do not encode this as a negative truthfulness value or an undocumented sentinel.

#### Propagation boundaries

If ambient entries propagate, they should be permitted only among hubs whose approved policies allow that class. Propagation cannot transform ambient chatter into evidence-backed fact, increase its confidence, or mark a destination discovered. If an existing network cannot enforce class-aware propagation, the MVP should keep ambience local to its origin hub and present the quest through authored dialogue. A player-authored decision to forward a note should be a separate, explicit command with truthful delivery feedback.

#### Briefing projection requirements

The briefing should label the entry as ambient or source-unknown and show attribution. It must not say verified, threat, opportunity, actionable lead, or confirmed event merely because of the current numeric threshold or subject-type mapping. If the current BriefingItem shape cannot carry this distinction, a new projection contract must be approved before content is wired. Do not add a second briefing panel or rumor list.

#### Persistence and exactly-once rules

If a new provenance field is added to WastelandRumor, capture and restore must round-trip it and legacy rumor records need an explicit default. Stable rumor IDs continue to use the rumor owner’s sequence contract. Quest conclusion effects must be idempotent: repeated intercept, repeated briefing reads, save/restore, or scene reopen cannot create duplicate rumors or duplicate public reports. A generated ambient rumor should be authored or seeded deterministically through the established campaign RNG if runtime selection is introduced; never use wall clock or unordered collection traversal.

#### Failure handling

If hub policy is unknown, reject ambient creation with a visible authoring diagnostic and omit the feature in release content. If an ambient rumor reaches an unsupported receiver, do not relabel it as an event rumor. If the player forwards a report and delivery fails, preserve the source testimony and show “not sent.” If the rumor expires, preserve the quest’s historical conclusion but stop presenting it as currently circulating. If no source is found, preserve “not found in checked sources,” not “there is no event anywhere.”
### Pass 23B — Consumer matrix, no-op behavior, and acceptance tests (DRAFT)

Ambient-only information should reach only consumers that can preserve its epistemic class. This consumer matrix is a proposal for the RumorSystem owner and should be checked against the live host wiring before any implementation ticket is opened.

| Consumer | Allowed ambient input | Required output | Prohibited output |
|---|---|---|---|
| Rumor origin hub | explicitly authored ambient record | attributed local entry | inferred event or faction action |
| Rumor propagation | same class only if approved policy permits | attribution retained at recipient | ambient promoted to evidence-backed |
| Intelligence briefing | typed source-unknown class | visible uncertainty and origin | verified, threat, or opportunity by numeric shortcut |
| Quest runtime | player-heard/accepted fact | optional bounded investigation | quest from catalog load alone |
| Expedition selector | no direct rumor input; only quest-owned valid site request | eligible existing location candidate | generated map node from headline |
| Codex/journal | witnessed or resolved quest facts | historical summary | duplicate mutable rumor store |
| Faction standing | no default ambient input | none | automatic reputation delta |
| World evolution | no default ambient input | none | world mutation from unsourced chatter |

#### No-op and error behavior

When the hub policy is unset or invalid, ambient creation returns an explicit rejected result or follows the existing content-integrity failure contract. It must not silently fall through to ordinary rumor generation. When the consumer cannot represent provenance, suppress that consumer projection and keep the source record intact only if its owner considers the save valid. A UI failure to render an ambient badge cannot be worked around by marking the rumor verified.

#### Acceptance scenarios for a future owned package

1. Ambient record at a non-designated hub is rejected.
2. A designated hub can surface the ambient item but cannot turn it into a threat/opportunity automatically.
3. A high numeric confidence does not produce “verified” for source-free material.
4. Ordinary event-backed rumors retain their existing behavior.
5. Expiry removes live circulation while witnessed quest history remains.
6. Propagation, if approved, retains provenance and does not manufacture an event source.
7. Dialogue opening and closing do not create duplicate rumor records.
8. Save/restore preserves provenance and legacy saves preserve existing meanings.
9. Missing hub or subject reference follows the documented integrity/fallback policy.
10. Equal state and seed yield equal ambient selection where seeded selection is used.

These are candidate acceptance cases, not a direction to add tests in this planning pass. Any implementation still requires exact path ownership, current-ledger queue placement, focused tests, migration review, and handoff. No architecture decision is made by the plan itself.
### Pass 23C — Consequence audit worksheet (DRAFT)

Before implementing any ambient rumor response, fill a row for the exact action. The table prevents ambient narrative from leaking into unrelated systems simply because a bridge happens to exist.

| Action | Source fact | Command owner | Event/result | Save owner | User feedback | Rollback |
|---|---|---|---|---|---|---|
| surface authored ambient entry | approved hub + valid rumor definition | RumorSystem host | generated/surfaced record | rumor save section | attributed “source unknown” | remove feature row; preserve legacy rumors |
| read the report | reachable hub + report exists | briefing projection | read only | no new save unless current owner tracks it | display origin and epistemic class | no state change |
| begin investigation | player accepts and quest supports it | quest runtime | accepted/started fact | quest save owner | objective visible | abandon through quest owner |
| inspect real source log | actual log asset/record exists | log owner | observation fact | owning log/quest section | report checked scope | preserve source, reverse only mistaken conclusion |
| forward account | player confirms recipient | rumor host | propagation command result | rumor save owner | delivered/not sent | owner-specific withdrawal if available |
| alter faction standing | no default source fact | none in MVP | none | none | no message | not applicable |

#### Fault-injection questions for review

What if the hub policy loads but the rumor row does not? What if the row loads but the quest definition is missing? What if the quest resolves but the briefing projection refreshes late? What if a delivery command succeeds but UI feedback is interrupted? What if the save is captured between the local report and explicit forwarding? The implementation must preserve truthful partial progress and never replay a completed outward effect on restore.

#### Observability and metrics

If maintainers need to know whether ambient rows are used, record bounded content-utilization facts through the existing instrumentation authority. Do not record player belief, emotional reaction, or conversation text as analytics. A runtime metric can count surfaced/read/accepted outcomes by stable authored ID if that data policy already exists. Metrics do not grant new rumor persistence or make a narrative claim true.

## Pass 24A — Preserve travel-choice guide rewards across consequence owners (DRAFT)

### Verified seam

Part 46 asks which field-guide entries never trigger. The inspected Core resolver returns `TravelEncounterResolutionResult.UnlocksFieldGuideId`; the legacy overload also returns the identifier separately. `ExpeditionEncounterBridge.ResolveChoice` recognizes patrol encounters and creates `NarrativeEncounterResolutionResult` from the travel result, copying morale and guilt but not the field-guide ID. The normal host path then applies narrative encounter consequences. Separately, `ExpeditionHostSession.ResolveTravelChoiceWithCombat` calls a discard-output overload (`out _, out _, out _`) before applying combat. These two reviewed routes therefore warrant a loss-of-effect audit. The evidence does not establish that no other adapter consumes the field-guide ID, so the first integration task is a full call-site census.

### Owner-routed sequence

1. Core validates and resolves the authored encounter choice under its existing transaction and returns the stable guide ID as part of the resolution result.
2. The adapter that owns field-guide integration validates that the returned ID exists in the already-loaded `FieldGuideCatalog`.
3. That adapter calls the existing unlock operation once. It does not write a new ledger, save section, or UI-owned counter.
4. Existing journal/Codex feedback reports whether the unlock was newly applied. Repeated unlocks remain no-ops and must not replay reward text.
5. Existing `FieldGuideSaveStore` capture/restore persists the state under its current owner. The resolver must not directly persist or mutate a panel.
6. Encounter and combat consequences continue through their current owners exactly once; guide failure must not roll back a fully valid unrelated choice unless the owner contract explicitly requires atomicity.

This sequence is a design target only. Before choosing the adapter, verify whether `ExpeditionEncounterBridge`, `ExpeditionHostSession`, a campaign host, or a separate result consumer is the correct integration owner. Preserve a single route for both standard and combat-added resolution; avoid creating one special field-guide grant path per caller.

### Effect and failure table

| Condition | Guide effect | Other encounter effect | Required observability |
| --- | --- | --- | --- |
| Valid ID, first unlock | Unlock existing entry | Apply once | Codex/journal confirms new entry |
| Valid ID, already unlocked | Idempotent no-op | Apply once | No duplicate reward |
| Empty ID | None | Apply ordinary valid choice | No false guide message |
| Unknown ID | Reject guide grant | Preserve valid non-guide outcome per approved contract | Diagnostic identifies encounter/choice/ID |
| Bridge maps result | Preserve ID to owner | Preserve morale/guilt | Integration outcome is visible |
| Combat wrapper resolves same choice | Share same result routing | Combat runs once | No duplicate choice resolution |
| Save restored | No new grant event | No replayed encounter | Entry remains readable |

### Ordering and atomicity

Choice costs and requirements are validated by Core before resolution. The guide effect occurs only after a successful resolution result, never on offer, selection preview, encounter surfacing, or map spawn. If the guide ID is invalid, the resolver's ordinary transaction result remains governed by existing Core semantics; host code must not pretend a failed guide grant reversed consumed inventory. Whether the field-guide grant belongs inside Core transaction atomicity is an architecture decision only if current evidence shows partial commit risk. The minimal likely route is a typed effect carried in the existing resolution payload and applied at the existing host consequence seam.

## Pass 24B — Exactly-once and integration acceptance plan (DRAFT)

Audit every caller of both `TravelEncounterSystem.ResolveChoice` overloads and every adapter that handles `TravelEncounterResolutionResult`. For each caller, record whether it is a production route, CLI/sample path, self-test, or unused helper; whether the ID is preserved; and which existing field-guide owner is available. The direct combat route deserves special scrutiny because it currently discards the output values. Do not infer that a demo CLI proves Godot runtime integration.

A minimal integration package should change only the chosen shared seam and, if necessary, the smallest host adapter. It should not touch ecology observation code, add a new save section, move Codex catalog ownership, revise all 32 entries, or expand the world bible. The owner must deduplicate by stable entry ID and route feedback through existing journal/UI conventions. Ensure both `ResolveTravelChoiceWithCombat` and the standard surfaced-encounter route cannot apply the same choice twice. If Core allows legitimate direct re-resolution, the host acknowledgement boundary must still prevent duplicate player-facing effects according to the existing queue contract.

Acceptance criteria: every supported route carries the returned ID; a valid ID unlocks via one canonical owner; already-unlocked state is idempotent; invalid IDs are contained and diagnosable; save/load retains the result without replaying it; ordinary morale/guilt/cost/combat behavior remains unchanged; and source-specific dialogue never claims a grant before the owner confirms it. The call-site audit must then inventory `unlock_trigger` strings and identify which are wired, intentionally future, or unresolved. That broader content census is a follow-up package, not a prerequisite to the narrow bridge repair.

**Governance:** DRAFT, documentation-only. Before execution, confirm queue availability in `INTEGRATION_PLANS.md`, claim exact paths in `WORKTREE_OWNERSHIP.md`, recheck all premises against current source, choose focused targets under `TEST_POLICY.md`, and report save/determinism implications in the handoff. No implementation or test is authorized by this plan tranche.

## Pass 25A — Mid-route weather transition as a typed effect contract (DRAFT)

### Existing owners and unresolved seam

The World Bible Part 46 prompt asks how an expedition behaves when a storm window overlaps a trip already underway. Current Core `ExpeditionSystem.TickHours` processes stamina drain, encounter roll, vehicle breakdown, and phase advance; the state also retains dispatch-time weather multipliers. Host dispatch pulls weather inputs from the current weather effects table. Separately, `YearOfAshStormCatalog` queries day-range windows, `WeatherGateBlock` describes a dispatch gate and force costs, and `WeatherCascadeHostSession.RouteExpedition` changes active-front encounter multipliers. The reviewed evidence does not prove a single event routes a newly active storm window into an existing expedition state. This is the central premise gate.

### Candidate consequence envelope

If current architecture permits a dynamic route effect, represent it as an explicit owner-produced fact with bounded data: `event_id`, `expedition_id`, `storm_window_id` or active-front identity, campaign day/tick, applicable route/phase, effect kind, magnitude, and whether the player was offered an action before commit. This is a design checklist, not a new DTO approval. The expedition owner validates eligibility and applies any travel/encounter consequence. Host presentation renders a truthful warning/debrief. Journal records only accepted facts. Weather authority remains owner of storm windows and current weather; UI cannot apply the effect.

No new effect category should be added until reviewing the current expedition and weather event vocabularies. A delay could alter remaining route ticks; a hazard could alter encounter risk; exposure could reach radiation/gear wear; a blocked route might require a supported return/detour choice. These require different consumer owners. Avoid a generic “storm severity” that simultaneously changes speed, encounter risk, radiation, vehicle condition, and quest state without explicit balance design.

### Exactly-once ordering

1. Weather owner reports a genuine transition with stable identity.
2. Host/session determines whether the affected expedition is active and its phase is eligible.
3. Expedition owner accepts the event once and captures the application marker through its existing state owner.
4. If a player command is available, the UI presents it before irreversible effect application; command validation rechecks state version and costs.
5. Existing medical/radiation/vehicle owners receive typed consequences as applicable, never duplicated in the expedition panel.
6. Quest/narrative adapters consume the committed result, not the forecast or warning attempt.
7. Save capture preserves pending/committed distinction; restore neither drops a pending choice nor replays a committed effect.

If no event can reach an active expedition, the correct scope may be dispatch forecast clarity plus after-return debrief, not a new dynamic effect system.

## Pass 25B — Duplicate, rollback, and failure contracts (DRAFT)

**Duplicate event:** same front/window identity on consecutive day ticks must not apply repeated one-time route delay. Repeating continuous exposure is valid only if the existing exposure clock owns it; do not add a second per-storm dose counter.

**Stale player command:** if the sortie phase changed since its warning panel opened, reject with current truthful state and do not consume stamina, equipment, or cargo. Existing state-version/command-result contracts should determine this behavior.

**Missing expedition:** log or discard the weather-to-expedition projection according to the source owner; never create a ghost sortie. **Unknown route:** apply only a general effect if authored data explicitly permits a global route effect; otherwise block and diagnose. **Already completed/failed sortie:** do not mutate past results. **Save between warning and choice:** preserve exactly the state needed to resume or cancel the offered command. **Clock disagreement:** campaign day and expedition hour ordering must be resolved before any deadline or warning claims to occur before impact.

An implementation plan must name the single integration seam, affected state owner, current save capture/restore route, deterministic ID generation, journal or UI observer, and smallest focused test target. The contract must preserve current dispatch forced-gate acute-dose path and ordinary travel tick order. No parallel weather ledger, expedition event registry, route manager, or player-facing dialog state is proposed.

Acceptance review should include a negative assertion that a storm-window catalog row alone cannot alter an expedition, plus positive examples for each supported route-phase effect. It should compare the dispatch estimate to the saved active-sortie inputs, verify one application through restore, and confirm that debrief prose derives from the committed result. Future tests require package approval and the current test policy; this documentation tranche adds no tests or production changes.

## Pass 25C — Cross-system result matrix and observability checklist (DRAFT)

| Outcome fact | Canonical owner to verify | Narrative consumer | Persistence question | Duplicate-effect risk |
| --- | --- | --- | --- | --- |
| Forecast interval shown | Weather/forecast host | Dispatch conversation | Is exposure/view state saved? | Claiming player saw an unseen panel |
| Dispatch weather estimate | Expedition host/Core | Quest gate and debrief | Are sampled multipliers retained? | Recomputing from different weather later |
| Gate force cost | Weather gate + dispatch result | Journal/debrief | Is forced choice captured with sortie? | Applying stamina or acute dose twice |
| Active front changes | Weather cascade | Expedition adapter, if any | Is front identity stable on restore? | Same front raising risk repeatedly |
| Expedition phase result | Expedition Core | Quest and dialogue | Existing expedition save path | Rewriting terminal states |
| Returned testimony | Narrative/journal owner | Investigation branch | Is source/timing persisted? | Treating display text as evidence |
| Equipment damage | Gear/vehicle condition owner | Repair/dispatch decision | Existing item durability save | Duplicating wear in quest flags |
| Faction delivery | Inventory/trade/faction owners | Courier branch | Existing transaction/reputation path | Awarding standing on dialogue alone |

Each outcome should be observable in at least one player-facing surface, but not every fact requires a new modal. Dispatch details belong in the existing estimate/route UI; forced gate cost belongs in pre-commit feedback; active expedition changes need an in-context warning only if a timely command exists; post-return consequences can appear in the existing report/journal surface. A log entry must identify the actual committed outcome and use stable, deduplicated identity.

### Integration cost tiers

- **Tier 0, prose-only debrief:** source event already exists and supplies a truthful result. Content/localization/UI fit only.
- **Tier 1, presentation bridge:** current owner emits a verified result but host does not surface it. Small adapter work, no new state.
- **Tier 2, saved choice:** player can intervene and resume after save; requires current state owner and capture/restore extension, deterministic command identity, and focused save coverage.
- **Tier 3, dynamic route simulation:** storm transition changes active expedition timing/phase/effects. Requires explicit architecture decision, ordering contract, balance review, UI, persistence, and cross-system verification. Do not conceal Tier 3 cost inside a “quest content” ticket.

The preferred first implementation, if approved, should select the lowest tier that produces a truthful player outcome. If no route transition bridge exists, ship forecast readability and post-return source-aware writing first, then separately decide whether dynamic interruption belongs in core or an expansion. Do not conflate a narrative gap with evidence that a new simulation system is necessary.

## Pass 26B — Consequence declarations, idempotency, and cross-plan routing (DRAFT)

Dialogue choices should declare consequences at the level the player can reasonably anticipate. A choice may affect wording only; close a local scene; advance or resolve its quest; update a character relationship; alter faction access; change a world/location fact; or contribute to an ending. These levels are not interchangeable. A line that sounds like a promise to aid a faction cannot be routed as cosmetic flavor if the player expects access or resources to change. Conversely, a small tone choice should not unexpectedly alter a major ending.

### Consequence envelope

Each response declaration includes response ID, consequence class, command or event owner, input parameters, preconditions, success feedback, rejection feedback, repeat policy, and whether the effect is immediate or deferred until an existing commit point. A consequence can request an owner action, but dialogue itself does not directly mutate the owner. If the owner rejects the request—for example, because the item is not available—the response must remain coherent and explain the failure. Do not show a success line before the authoritative result is known.

The implementation order is: validate scene and response eligibility; submit the declared command once; receive an accepted/rejected outcome; update the owning quest or relationship through its supported contract; display truthful feedback; refresh dependent UI at the next safe boundary. An event may notify a journal or character response after commit, but subscribers must not replay the original cost. Save restoration must restore the owner’s committed fact and not rerun dialogue consequences.

### Idempotency and transactional outcomes

One-time costs and rewards need a stable action identity or owner-provided deduplication rule. Reopening a conversation, clicking twice, reloading after a committed save, or reconstructing the scene must not charge or grant twice. If one response requires multiple owner actions, define partial failure behavior: either the operations form a supported transaction, or the player receives an explicit partial result and a safe recovery route. Do not emulate atomicity by holding duplicate state inside a dialogue manager.

A failure response is not a successful consequence. Rejected transactions preserve current state, identify a recovery condition, and keep the conversation navigable. Deferred effects name the event that will apply them, such as expedition return or the next daily owner tick; they may not depend on an undocumented callback order. If a change must survive restart, the existing owner’s save capture/restore path is part of the acceptance evidence.

### Cross-plan routing example

A player chooses “I’ll deliver what returned” in the courier scene. The dialogue submits the existing inventory/contract command using the exact eligible returned items. If accepted, the contract owner records delivery, the quest owner advances its own objective, and the next scene acknowledges the confirmed quantity. The faction owner may update standing only if an explicit supported rule maps this delivery to standing. If the command is rejected for insufficient quantity, no quest or faction effect fires; the player sees the missing amount and may choose another response. A storm report alone never grants faction standing.

### Consequence review grid

| Class | Minimum evidence | Expected player feedback | Persistence owner |
| --- | --- | --- | --- |
| Cosmetic | same scene, no state mutation | wording or tone changes | none beyond scene lifetime |
| Local | supported scene/session state | visible local outcome | current scene/host owner |
| Quest | valid quest transition | objective/status updates | quest authority |
| Relationship | supported character command/event | character response or displayed state | relationship authority |
| Faction | supported faction transaction | access/standing feedback | faction authority |
| World | supported location/resource/event owner | observable world/map/result change | world owner |
| Ending | approved ending-resolution contract | clear preview where possible and ending evidence | campaign/endings authority |

### Scope limits

Minimum viable dialogue consequence routing supports cosmetic/local responses and one validated quest update through the current owner. Relationship, faction, campaign, and ending effects are optional layers requiring integration premises, save review, and distinct player feedback. The storm investigation’s initial branch should stop at information, local report status, and a quest transition. This produces meaningful choice while avoiding a large, unsupported web of hidden faction and ending mutations.

## Pass 27 — Owner-routed outcomes for “The Last Dry Strike” (DRAFT)

The player’s report choice is meaningful because it changes how the archive describes its own evidence, not because every conversation needs a reward meter. Consequences remain proportional to the action and explicit about their owner. The baseline implementation has one quest-level outcome and an optional chronicle/journal acknowledgement if the current owners expose the needed route. It does not add a match reliability stat, recipe bonus, inventory stack, market price modifier, trade specialty, or faction reputation change.

### Response-to-owner map

| Player intent | Consequence class | Request/owner | Observable result | Repeat rule |
| --- | --- | --- | --- | --- |
| Publish corrected interval | Quest | Existing quest/report transition | Journal says immediate test passed; storage unknown | One transition per quest instance |
| Preserve both records | Quest/local archive | Quest owner; archive display only if a current surface supports it | Both source notes remain cited | Idempotent on reload |
| Decline certification | Quest | Resolve as uncertainty through quest owner | No pass/fail claim is made | Terminal response; no hidden penalty |
| Ask for available supplies | Resource inquiry | Inventory/quartermaster owner, read only until confirmed | Actual stock or unavailable state | Query has no cost |
| Commit a supply handoff | Resource/quest | Existing inventory transaction first, then quest update | Exact accepted quantity shown | Transaction key or owner deduplication |
| Change caravan price or standing | Faction/economy | Not part of the core slice | No such output | Requires separately approved mechanics |
| Change map access or route risk | World | Not part of the core slice | No such output | Requires map owner and new decision |

The owner sequence is validate response eligibility; submit one existing command if needed; wait for accepted/rejected result; record the quest transition; refresh journal/dialogue at the next safe boundary. A rejected inventory transaction does not advance delivery or alter standing. A report can still close as “not certified” if the player has no item, because certification is based on evidence rather than possession.

### Consequence severity and scale

- **Level 0, wording:** a character uses “test slip” instead of “quality card.” No stored fact.
- **Level 1, local quest state:** the archive report selection changes. Core slice.
- **Level 2, system state:** an existing discovery or chronicle owner displays the corrected note. Only if the current API supports the action.
- **Level 3, cross-system cascade:** a future authored delivery objective reads the report when it chooses supplies. Expansion and requires a source census.
- **Level 4, world state:** new markets, routes, or settlement prices change. Not recommended for this content seed; requires market/faction owner review.
- **Level 5, saga:** a late-game ending treats the archive choice as evidence of institutional honesty. This is expansion-only and must use the existing ending input contract; it cannot be implied from one small report without authored weight and review.

### Transaction and save boundaries

Dialogue reconstruction, panel refresh, repeated clicks, and save/load cannot repeat a cost or award. A supply request that is only a query is side-effect free. If an existing command consumes an item, the UI must show the actual item ID translated to a player-facing name and exact quantity before submission. The narrative result is emitted only after acceptance. If quest state persists, its existing save owner must capture and restore the selected report outcome; this plan adds no save store. A restored outcome cannot rerun the callback transaction.

### Failure and recovery behavior

If the report owner is unavailable, choices can remain prose only if the scene clearly describes the decision as an in-world conversation with no archived state change. Prefer delaying the scene over fake functionality. If a character is absent, their relationship consequence is dropped, while the evidence quest remains resolvable. If the location cannot spawn, use the Plan 18 clue fallback. If the player chooses an incomplete record set, the terminal state remains uncertain and can still be a successful completion of the stated objective. If the player abandons the investigation, record abandonment distinctly from a failed assay.

### Production recommendation

Core scope: authored evidence, three report responses, one local quest transition, and a concise callback. Expansion scope: multiple workshop reports, cross-season recurrence, a character arc for Mara, and a faction-facing supply contract. Exclude new economic effects until market demand and ownership are verified. The strongest recommended content is the unresolved report: a player can protect future readers without receiving a bonus, and the late callback can show the cost of precision when another group would prefer certainty.

### Pass 27B — Action contract, negative cases, and implementation staircase (DRAFT)

**Choice 1: publish a narrower statement.** Preconditions: both reports are discovered or the player explicitly chooses to publish a single-source limitation. Request: existing quest/report owner stores the selected outcome. Success: the journal presents the chosen wording and citations. Rejection: if the report route is not available, keep the conversation open and offer preserve/refuse. Repeat: identical response is a no-op.

**Choice 2: keep both papers side by side.** Preconditions: at least one record is available. Request: quest owner resolves to “evidence preserved, interpretation open,” if supported. Success: a later archive visit can show the pair. Rejection: if no persistent archive surface exists, show a local acknowledgment and do not promise a durable file. Repeat: no duplicate entry.

**Choice 3: do not certify.** Preconditions: none beyond reaching the choice. Request: quest owner resolves with uncertainty or records refusal. Success: a clear terminal status that does not count as a failed assay. Rejection: the scene remains navigable and explains that the decision could not be recorded. Repeat: no cost.

**Choice 4: ask for a carton.** Preconditions: an existing item definition and inventory query. Request: read-only stock check. Success: present the actual number or “not available.” Rejection: display the source’s unavailable state. No quest transition occurs until the player explicitly confirms a supported transfer command.

**Choice 5: confirm transfer.** Preconditions: visible quantity, current inventory, and a command that accepts the transfer. Request: owner transaction, then quest update. Success: display actual accepted quantity and new quest objective. Rejection: no payment, inventory mutation, or relationship effect; let the player retry or decline. If the existing game has no such command, this choice remains excluded.

### Integration staircase

1. **Content review:** verify the record family does not duplicate existing assay content and the prose’s claims are internally consistent.
2. **Consumer audit:** find the actual catalog loader, discovery route, quest instance owner, journal surface, and save owner. Catalog presence is insufficient.
3. **Minimum integration:** connect two records and one local report decision through a single current owner; no new resource system.
4. **Outcome wiring:** prove accepted and rejected commands, callback timing, and no duplicate effects on repeated input.
5. **Optional expansion:** only after the slice is live, add a second small craft corpus or a supply-delivery side quest that uses current item APIs.
6. **Cross-system expansion:** market, faction, ending, or route consequences need a new premise review and a source-backed causal rule.

Stop the integration if the only way to remember the response is a new dialogue-only ledger, if the public wording would be generated by an unowned flag, or if a later report needs to mutate historical source text. Keep one responsible owner for each durable fact. The dialogue layer requests commands and displays outcomes; it is not a second quest system, journal, inventory, or social simulation.

### QA handoff cases

The reviewer should be able to demonstrate: first test read with no second source; both records read in either order; a mismatch between same batch and same carton; player refusal; unavailable speaker; unavailable location; invalid response ID; inventory query without mutation; transfer rejection; transfer accepted exactly once; panel reopen; campaign save and reload; and localized long labels. Each case records expected quest state, visible text, and owner mutation. Any unverified case remains a named blocker rather than a guessed pass.

The core slice’s measurable outcome is modest but complete: the player can read two different tests, identify their limits, choose a report treatment, see whether that choice was recorded, and encounter no false material promise. Expansion work can make the archive echo through future craft content, but only by adding another authored source and an actual consumer, not by making the first report silently control the economy.

## Pass 28 — Consequence routing for a care handoff

This pass defines where “The Cup on the Rail” may cause change. Relevant authorities are CaregivingSystem, which owns assignments/bond and emits start/end/bond/dialogue events; DutyRosterSystem, whose Godot integration vacates the caregiver’s role when care starts; and existing needs, health, medical, disease, radiation, and relationship owners. Quest and dialogue are orchestrators, not authorities over those values.

### Effect classes

| Player action | Consequence class | Owning path | Required result handling |
|---|---|---|---|
| Ask a character a question | Cosmetic/local | Dialogue graph | Advance node only; no simulation mutation |
| Inspect current pair | Read-only | Caregiving host query | Refresh and display pair |
| Assign replacement | Local system | CaregivingHostSession command | On success reconcile pair; on failure show safe reason and preserve state |
| End assignment | Local system | CaregivingHostSession unassign action | Confirm current pair, report ended pair, preserve bond semantics |
| Hear roster explanation | Cosmetic/local | DutyRoster read query | No role mutation from prose |
| Reopen scene | Quest-local | Existing quest owner if supported | Recompute facts; never replay old commands |
| Mark quest resolved | Quest consequence | Existing quest lifecycle owner | Advance only after defined terminal response |
| Change affinity, fatigue, health, faction standing, or medical status | Out of scope | Existing respective owners | No such effect absent a separately approved command |

### Transaction sequence

1. **Present:** build read-only context from current pair and optional roster query. Bind actors to current canonical survivor IDs. If the pair is missing, hide pair-specific responses.
2. **Choose:** player selects semantic intent (“seek replacement,” “end assignment,” or “defer”), never a direct set-value operation.
3. **Preview:** if current host exposes preview for assignment, call it with current version and show availability. For unassignment, confirm the target still has the expected caregiver because the current API is void and does not return a structured result. Do not claim success until a post-query confirms change.
4. **Execute:** call existing host action once. Do not retry automatically after stale or failed result. Host events and roster vacancy remain owner behavior.
5. **Reconcile:** query pair again. For assignment, verify selected caregiver is now assigned. For unassignment, verify patient has no caregiver. If post-query disagrees, show unresolved status and use existing bounded diagnostics; do not synthesize success.
6. **Advance quest:** update lifecycle only after reconciliation and only through existing quest owner. A deferred conversation is not completed merely because dialogue reached its last node.
7. **Save:** rely on caregiving and duty-roster save owners. Quest owner persists only lifecycle it already supports. Do not create a transaction journal, outbox, or new save section as shortcut.

### Exactly-once behavior and event ordering

A successful care start causes the care system’s start event, then the Godot subscriber empties the prior duty role. Restore order contains a vacancy rule for either load order. Quest logic must not clear the role again. A care end event does not itself restore a prior role in the reviewed code; dialogue must not promise automatic return to work. If role restoration is desired, it needs a separate owner/behavior decision and collision review.

A bond-threshold event does not prove that a dialogue consumer displayed a scene and is not a quest reward. Confirm the event listener and delivery semantics before adding a callback. Event replay after load must not duplicate quest completion or authored rewards. Keep any later callback idempotent through the existing quest event contract, not a new care-event ledger.

### Failure matrix

- **Assignment rejected:** no pair mutation; do not show the successful relief ending.
- **Stale preview:** refresh and require another player action; no blind retry.
- **Patient no longer needs care:** close the operational branch neutrally; do not imply harm.
- **Candidate cannot provide care:** do not say they refused. Offer another valid candidate or defer.
- **Unassignment target changed:** re-query and ask the player to choose against current state; never unassign a different caregiver silently.
- **Quest owner lacks deferred state:** leave quest available/in progress or end conversation only; no parallel quest store.
- **Survivor dies:** canonical system ends assignment; narrative may acknowledge changed circumstances without assigning unsupported blame.
- **Roster query unavailable:** omit work-specific claims and use only care-owner actions.
- **Host/save unavailable:** state that no confirmed change occurred and retain current quest state.

### Observability and UI

Distinguish “assignment active,” “assignment ended,” “replacement assigned,” and “conversation deferred.” Avoid a success toast on button press alone. Keep medical status and privacy in their current UI owners; a public roster note reveals no patient condition. Keyboard/controller back closes dialogue without selecting “leave as-is.” Focus returns to the originating shelter panel. Use readable, non-color-only feedback for unavailable candidates. These are UX integration checks, not new authority.

**Minimum viable route:** authored hub scene; read-only pair context; existing assignment/unassignment actions; result-backed text; lifecycle advance only if current owner supports it. **Optional:** idempotent bond callback, later return line, roster-aware prose. **Deferred:** fatigue reward, automatic caregiver rotation, restored labor assignment, persisted consent, faction standing, and care-failure history. Each crosses an owner boundary and requires architecture review.

**Production cost:** medium for one result-aware scene; high where structured unassignment results, new event bridge, persistent deferred state, or automatic duty restoration is needed. The core game can include a handoff as authored story plus existing action. A multi-person care arc is expansion content after queue review, ownership, data mapping, focused verification plan, and exact consequence contract. No code, data, save state, or tests changed in this planning pass.
## Pass 29 — Micro-encounter consequences through canonical owners

This pass binds the coverage audit to consequence discipline. The existing micro encounter definitions carry authored choice consequences, and the common NarrativeEncounterSystem resolves choices into its current result payload and saved history. The host routes selected effects into owners. One concrete existing seam is micro_dead_livestock: its micro_contamination_exposure flag is handled by MicroLocationHazardRegistry, which maps it to disease_zoonotic_flu through the canonical DiseaseSystem and guards replay. This is evidence for one specific hazard route, not a general “micro-location consequence bus” that new effects can assume.

### Effect routing table

| Authored outcome | Consequence owner | Safe content claim | Audit before extension |
|---|---|---|---|
| Morale/guilt deltas | Current narrative/encounter outcome path | Encounter choice changes the authored dimensions | Verify how UI/report exposes each delta |
| Item grant | Inventory owner through host consequence applier | A listed item is granted if the command succeeds | Confirm item ID, quantity cap, and failure behavior |
| Depletion | NarrativeEncounterSystem state | A depleting choice prevents that encounter from recurring under its owner rules | Confirm save/restore and duplicate-resolution guards |
| World flag | Existing flag ledger | The flag is recorded if host wiring accepts it | Identify every consumer and exactly-once behavior |
| micro_contamination_exposure | DiseaseSystem via MicroLocationHazardRegistry | Dead-livestock exposure is routed to the canonical disease owner | Do not clone exposure state or infection |
| Quest progress | Existing quest owner, if a subscriber exists | A known event can advance an authored objective | Verify subscriber, event identity, replay behavior, and save owner |
| Location discovery | Existing map/discovery owner | Only a real map command changes discovery | Encounter selection alone is not a new map reveal |
| Faction standing | Existing faction owner | Only a supported consequence changes standing | A faction mark, board, or symbol does not itself alter standing |

Do not make the narrative dialogue callback the owner of consequences. It may request a command, observe an authoritative result, and display localized feedback. A follow-up scene that merely interprets evidence is cosmetic/local; a response that grants a resource or changes a flag is a separate owner-mediated effect.

### Exactly-once sequence

1. Candidate selection receives the canonical expedition location ID and applies the existing exact requirement check.
2. The selected encounter ID is presented to the player; catalog existence alone does not create a resolution.
3. The player chooses an authored choice ID.
4. NarrativeEncounterSystem validates the encounter and choice, records the resolution, and returns the current consequence payload.
5. The host applies each supported effect through its owning subsystem. Unsupported or unavailable routes must fail visibly or be omitted during authoring validation; do not claim success from a prose transition.
6. For an accepted world flag, the host commits the flag before calling the hazard registry. The registry checks whether the flag was already set and invokes the disease authority once.
7. Quest/dialogue consumers observe the result only after successful resolution and must be idempotent across save/reload/event replay.
8. UI reports the outcome from returned owner results, not from choice text.

This ordering matters when an encounter is depleting: a retry cannot double-grant an item or reinfect a survivor. A future quest callback must not reapply any of the original choice effects.

### Coverage failures and recovery

- **Invalid requiredLocationId:** the bound encounter has zero effective weight at all catalog locations. Correct the authored reference to a valid canonical location only after confirming intended site; otherwise disable the entry through the established catalog policy and report it as invalid.
- **Known but inaccessible location:** do not claim successful coverage merely because the ID exists. Use an equivalent reachable site only if it supports the same authored evidence, or add a clue/delayed objective through the quest owner.
- **Wrong location ID passed by host:** exact-bound content is excluded. Diagnose the resolver/caller seam; do not weaken exact matching globally.
- **No eligible candidates:** let the current encounter system return no selection and preserve normal expedition flow. Do not force a global micro encounter as filler without design evidence.
- **Selected encounter but failed choice resolution:** retain the current state and show a truthful failure. Do not trigger flags, items, disease, or quest progress.
- **Host effect owner absent:** preserve the canonical source state where safe, surface unavailable behavior, and log the integration gap through existing diagnostics. Never create a parallel fallback inventory or disease counter.
- **Questionable consequence implied by prose:** revise copy or acquire explicit canon; do not implement an effect because the text sounds consequential.

### Acceptance, cost, and release scope

A future content batch is ready only after: all micro encounter IDs are unique across the common catalog; every nonempty requiredLocationId maps to exactly one intended location; every location identifier passed at runtime is canonical; content utilization sees all entries consumed through a real runtime route; bounded seeded replays show stable candidate/choice outcomes; each effect maps to one owner; and return/fallback behavior remains possible when no micro encounter is eligible. Static JSON validation can be low cost. End-to-end quest callback and location selection are medium/high, depending on whether a current event subscriber and location reservation already exist.

The core game should keep the canonical loader, exact filter, deterministic encounter choice, and owner-routed effects stable. An expansion can add location-specific encounter packs only after the coverage census identifies genuinely quiet or thematically mismatched destinations. No additional registry, map pool, hazard service, quest event bus, save section, production code, data, or tests are authorized by this planning pass.
## Pass 30 — Regional knowledge effects through the canonical map owner

Main.RecordCartographySurvey validates a node and survivor ID, checks that the survivor is alive and WastelandMap is available, calls WastelandMap.DiscoverSurvey, records five scavenging XP through shared skill progression using the existing campaign RNG fork, marks world state dirty, and refreshes the map panel. Cartography display derives from canonical map nodes and saved knowledge. Core also has region registration/survey methods and can parse map_regions.json, but no live host instantiation was found in the reviewed usage search. The validator calls the POI labels catalog-local. Content must therefore use the wired map action and avoid a competing regional state path.

### Consequence classes

| Action | Class | Current owner | Boundary |
|---|---|---|---|
| Read an atlas entry | Cosmetic/knowledge display | Existing archive/journal surface if supported | No discovery mutation |
| Compare chart and map | Local dialogue | Dialogue/quest owner | No route assignment |
| Survey a canonical node | Map consequence | WastelandMap via Main survey route | Use returned success; no duplicate discovery |
| Grant survey XP | Skill consequence | Existing shared skill progression | Never duplicate from quest reward |
| Reveal region membership | Architecture decision | No verified crosswalk owner | No current effect allowed |
| Select quest destination | Map/expedition owner | Existing graph and selector | Requires canonical reachable node |
| Record quest completion | Quest consequence | Existing lifecycle owner | Persist only supported quest state |
| Change relation/standing | Out of scope | Existing relationship/faction owner | No inference from terrain or hazard copy |

### Ordered action contract

1. Resolve a canonical travel-node ID; reject region-local labels as node IDs.
2. Verify current map access and living surveyor.
3. Call the existing Main survey route once; it delegates mutation to WastelandMap and shared skill progression.
4. Confirm canonical map knowledge changed before advancing a quest objective. Button press is not survey completion.
5. Advance through the existing quest owner using observed node/effect evidence. If no subscriber or API exists, retain the content as an atlas conversation.
6. Refresh map UI from its canonical projection. Do not make an unresolved POI into a marker.
7. Save through current map and quest owners only where their existing contracts require it. Static membership does not warrant a new save section.

Do not call CartographySystem.SurveyRegion and Main.RecordCartographySurvey for one player action. The former mutates region state separately and risks duplicate discovery/skill semantics. A foreman must decide whether that API is historical or intended for another host before implementation; this plan does not choose.

### Failure and recovery

- **Label has no approved mapping:** no survey command; preserve atlas prose.
- **Unknown node ID:** fail before mutation; do not substitute a similar name.
- **Node locked/unreachable:** keep the quest partial/delayed only if supported; never auto-unlock.
- **Survey returns false:** no XP or completion claim.
- **Survivor unavailable:** no fictional scout or action-history record; offer another supported eligible survivor or return to shelter.
- **Duplicate click/event:** rely on current map knowledge/idempotency; do not add a second ledger.
- **Quest consumer absent:** resolve as reading only, not a gameplay survey quest.
- **Stale save/restore:** rebuild from canonical map/quest owners; no regional snapshot overrides node knowledge.

### UI, performance, rollout

Separate atlas prose from live travel actions. A survey button shows node, surveyor, and result. Region-record counts cannot masquerade as destination counts. Provide focus return, keyboard/controller support, non-color-only route state, and readable “label not linked” feedback. Compute a future region-node report once at catalog load or offline, not every panel refresh.

Implementation staircase: (0) content-only atlas copy; (1) crosswalk decision and source census; (2) membership in one ratified owner; (3) reference validation and route reachability report; (4) quest consumes successful canonical survey; (5) replay and save/restore verification; (6) optional regional encounter weighting only after measured need. If the owner declines the crosswalk, stop after stage 0 and keep regions descriptive.

Cost is low for copy, medium for a crosswalk/validator/UI, and high for guaranteed regional destinations or encounter arbitration. No production changes, data edits, state mutations, or tests occurred in this planning pass.
### Pass 30B — Route/result acceptance matrix and bounded integration

| Case | Preconditions | Expected owner mutation | Quest/dialogue result | Save/replay expectation |
|---|---|---|---|---|
| Read atlas page only | Archive content available | None | Text viewed; no survey claim | No map or quest state added unless existing journal supports it |
| Select unresolved POI label | No approved node relation | None | Explain that the mark is not a route | Reopen returns to same truthful status |
| Survey valid node | Living surveyor; canonical map permits action | WastelandMap knowledge; existing scavenging XP | Advance only after confirmed result | Existing map/skill owners capture state |
| Survey rejected | Node/survivor invalid or action unavailable | None | Keep objective incomplete or offer archive-only close | No XP, flag, or duplicate quest progress |
| Repeat survey callback | Same canonical node already surveyed | Follow WastelandMap semantics | Do not duplicate reward or claim new POI | Replay remains deterministic |
| Crosswalk revised | Content change with old map save | Static relation changes only | Recompute route eligibility; preserve node knowledge | No serialized region membership to migrate |
| Region quest selects node | Approved relation and reachable path | Existing expedition owner dispatches | Objective opens at canonical node | Same seed/state yields same choice |
| Optional location unavailable | Candidate filtered or no slot | None | Drop optional candidate | Mandatory objective remains possible |
| Relation revoked | No current mapping | None | Use archive-only fallback or retire quest branch | No stale quest snapshot overrides map |

**Integration ownership:** data authoring owns region descriptions and an approved static relation if the architecture decision selects one. WastelandMap owns travel nodes, route graph, fog, and surveys. Expedition selection owns dispatch candidate choice. NarrativeEncounterSystem owns weighted encounter eligibility/resolution. Quest authority owns objective lifecycle. Dialogue only presents state and issues supported commands. The host coordinates these seams and persists through their existing save owners. Any proposal that gives the dialogue graph a region count cache, persistent node copy, route list, or survey ledger is rejected as a parallel authority.

**Instrumentation:** future selection traces should count how many nodes were eligible and why candidates were excluded, but report only bounded aggregates by canonical region/node IDs. Do not log personal character text or add per-frame telemetry. Use deterministic seeded replay for selection; static crosswalk validation needs no random seed. Run focused tests only after the responsible integrator claims paths and chooses a test target under TEST_POLICY.

**Rollout:** ship region chart copy independently; then approve the crosswalk; validate and render node-region tags; add one quest using a successful canonical survey; observe selection/fallback in a bounded playtest; only then add regional weighting or rarity. Roll back by removing the quest dependency and static relation while leaving canonical map discovery untouched. This sequence keeps prose useful even if the crosswalk decision is delayed.

## Pass 31 — Route the Bargain Once, Then Let the World Speak

### Effect ownership and branch routing

A negotiation scene may create several kinds of consequence, but one choice must not be fanned out indiscriminately. Route each effect to the single current authority that owns it, then expose a result fact for journal/dialogue projection where supported.

- Cosmetic consequence: choose one authored reaction/tell for this screen. No durable write.
- Scene-local consequence: advance a node or close the current scene in the existing dialogue owner.
- Quest consequence: apply one validated quest transition through the quest owner. A consequence ID is not itself proof the transition happened.
- Inventory/economy consequence: submit the accepted offer once through the canonical barter/trade command. Display the resulting receipt or failure returned by that owner.
- Relationship consequence: request a supported relationship change only when its owner/API is confirmed; no local trust cache.
- Faction consequence: use the canonical standing/access owner when a signed, authored branch actually intends it; a trader's line alone does not change faction standing.
- World/map consequence: reveal or mark only through the existing map/world owner after a real clue or event. Do not add a new location because the dialogue says “north road.”
- Ending/Chronicle consequence: emit or record only the canonical resolution fact already supported by the current completion seam.

Atomic bargain flow: (1) player selects a response; (2) host re-reads current visit, quest state, cargo and offer eligibility; (3) canonical trade operation validates balances and stock; (4) its result returns success/failure and actual quantities; (5) only on success does the quest transition apply; (6) only after confirmed transition does the journal/Chronicle projection update; (7) dialogue closes or reconverges. If the quest transition fails after a committed trade, define an idempotent recovery receipt or design the content so the trade owner can report the outcome before any separate quest mutation. Do not claim atomicity until the real API supports it. A second click must not double-debit or duplicate rewards.

Failure routing:
- Trader absent: no bargain command; preserve quest state and provide a supported delay/missed-window update.
- Required item absent: do not synthesize it; allow alternate information branch or explicit refusal.
- Stock changed: show current unavailability, refresh view, keep player inventory unchanged.
- Price changed/recomputed: show the current quote before commit; no stale cached quote.
- Quest already resolved: suppress stale response effects and return to current scene state.
- Map clue cannot be recorded: retain the dialogue outcome only if no map reward was promised; otherwise surface a retryable integration failure to the owner rather than silently consuming the clue.
- Partial delivery: only describe partial completion if barter/trade returns actual accepted quantity and the quest schema can represent it. Otherwise use all-or-nothing offers.

Observable completion should match the consequence. “They owe us a shipment” is permitted only if a durable contract owner records a shipment obligation. Without that owner, use “They agreed to discuss it on their next visit” only if an authored, supported future visit exists; otherwise use a present-tense agreement that does not promise a callback. A repaired route, reduced hostility, new map pin, or resource grant must never be inferred from warm prose.

UI/UX requirements: response label states intent; disabled responses give concise reason; commit displays actual barter result; failure retains the scene and refreshes available actions; close/back remains functional; keyboard/controller focus returns to a sensible node after refresh; journal feedback is accessible without relying on color. The trade tell remains descriptive presentation and does not impersonate the decision result.

Acceptance package for implementation: exact Core/host/JSON owners, command/result seam, failure semantics, save/event implications, and focused verification commands must be named before edits. Minimum slice tests cover double-submit, stale stock, insufficient offered inventory, absent caravan, quest already completed, failed effect routing, and save/load restoration. Keep the exact required sources of truth separate: route system schedules the caravan; cargo router selects lots; barter/trade owns exchange; quest system owns objectives; tell engine renders posture; map/Chronicle own their facts. A fully integrated feature exists only when source command, observable result, durable state where needed, and narrative text agree.


### Pass 31B — Transaction outcome matrix

| Attempt | Authoritative result | Quest update | Dialogue/journal |
|---|---|---|---|
| Offer is valid and accepted | Trade owner returns committed quantities/receipt | Advance only the matching offer step once | Confirm exact completed exchange |
| Offer has insufficient goods | Trade owner rejects without mutation | Keep step active; show alternate information response | Explain what is short using current inventory |
| Caravan departed | Route/caravan owner reports absence | Delay or resolve a designed missed-window branch | Do not show a stale trade option |
| Requested lot is not present | Current inventory projection excludes it | Preserve alternate negotiation path | Never imply the good was sold unless sale history proves it |
| Quest was completed in another UI path | Quest owner reports terminal/resolved | Suppress stale quest reward | Refresh card and scene |
| Clue destination is unavailable | Selector returns explicit unresolved reason | Keep lead active or move to authored clue fallback | State delay; do not award discovery |
| Player closes scene | No command is committed | No state transition | Preserve current focus/back behavior |

For asynchronous or multi-step integration, attach a stable command/idempotency key from the existing command system if one exists. Do not mint an ad-hoc GUID for determinism or replay. If there is no idempotency seam, prevent double-submit in the UI and still make the domain command validate current balances and stock; UI disabling alone is not correctness.

A future result projection should expose success, rejection reason, actual quantity, actual price, and any quest transition ID. Presentation may map reason codes to restrained copy, but cannot reinterpret a rejection as success. The log should distinguish player choice from world outcome: “You offered 3 filter cartridges” is not “the convoy received them” until the transaction confirms transfer.

Acceptance evidence must include one observable path from authored response through command owner to updated inventory/quest state, and one rejected path proving no state was partially applied. The plan remains a proposal until exact public APIs, save ownership, and active claim boundaries are checked for the implementation package.

## Pass 32A — From Tell to Record to Accepted Action

### Consequence spine

The intended loop is: machine owner supplies a current fact → existing tell projection presents a human-readable observation → the player decides whether to preserve or investigate it → JournalSystem or the existing quest owner records the supported knowledge/action → if the player requests a repair, the specific machine/service owner validates and performs it → a result is projected in dialogue/journal. This spine is a design target, not evidence that the complete loop is already wired. In source review, the tell path and journal note path exist; a general tell-to-work-order or tell-to-maintenance-log bridge was not established. That gap must be treated as an explicit implementation dependency.

Effect taxonomy:
- Flavor: the character recognizes a sound; no persistent change.
- Knowledge: an authored record is discovered or an existing glitch-noted fact is applied through its current journal owner.
- Quest: a stage moves only after source facts or a command result support it.
- Machine: only the machine owner changes condition through a valid action. Prose and catalog lookup cannot repair it.
- Inventory: only a confirmed service command consumes parts. A quest response cannot call inventory mutation independently.
- Relationship: only the canonical relationship command changes durable trust, if available.
- Map/world: no map location appears because a log refers to a place. Existing map discovery must validate the clue.
- Chronicle: summarize a canonical event only if a persistent event fact already exists.

### Proposed command path

1. Build a fresh context packet from current machine reading, noted tell, authored records discovered, and quest state.
2. Present response intent with the effect disclosed: preserve the note, ask for inspection, defer, or request a supported service.
3. At selection, revalidate source freshness, player permissions, service availability, costs, and quest state.
4. Route exactly one action through the owning API.
5. Inspect the returned result. If the command rejects, retain the scene, display its reason, and apply no completion/reward.
6. Apply the quest transition only after action/knowledge result is confirmed.
7. Refresh machine tell from current reading; do not claim a durable fix just because the action returned “accepted” if completion is scheduled.
8. Journal an outcome with distinct verbs: observed, reported, inspected, repaired, deferred. These verbs communicate which owner produced evidence.

### Failure and recovery matrix

- Tell is no longer eligible: stale response disabled; refresh from the machine owner.
- Historical log not found: keep the story at “reported,” do not synthesize the document.
- Repeated tell already noted: do not duplicate codex discovery; allow a new investigation only when supported.
- Character unavailable: offer a readable record or leave the branch closed; no random NPC substitute.
- Service command unavailable: allow the player to document or defer; do not consume parts.
- Service consumes resource but later validation fails: architecture blocker. The implementation must order operations or provide a receipt/reversal contract before shipping.
- Repair is scheduled but not complete: show pending state only if the service owner exposes it; do not display “repaired.”
- Save/load during the arc: restore quest and journal through their current authorities; reread machine owner state and rebuild the view.
- Player abandons quest: do not cancel a repair already committed by its owner; report current machine state separately.
- Two UI surfaces submit the same request: domain-level validation or current command/idempotency contract must prevent duplicate resource loss.

### Observability and release acceptance

A later integration must demonstrate one end-to-end story in which the same machine identity appears in a tell, a verified record, a player action, and a truthful conclusion. Evidence should include the source item ID, condition key, host query, journal/quest command, machine command (if any), save owner, and exact focused verification. A panel showing a tell and a catalog listing 20 glitches is not enough to prove a closed loop. A successful user path must expose current reading, origin of historical evidence, action cost/result, and state after save/reload; a rejected path must prove no partial consequence.

Architectural stop condition: if no existing machine command can perform the proposed repair, split the story so its completion is “finding documented” or “decision deferred.” Do not create a generic MaintenanceAuthority, shared wear ledger, or cross-machine repair façade in the content plan. New architecture choices belong to an approved owner decision outside this documentation wave.


### Pass 32B — Severity levels, UI contracts, and consequence examples

Apply Part 47's severity ladder intentionally:
- Level 0 flavor: hear a rhythm, read a log, discuss a missing signature. No persistent gameplay change.
- Level 1 local state: record a quest decision or witness interaction through the supported quest/journal owner.
- Level 2 system state: a real maintenance command changes an actual machine condition, consumes supported parts, or creates owner-defined downtime.
- Level 3 cascade: changed condition affects power, water, ventilation, staffing, or another dependent system only through already implemented daily owners and events.
- Level 4 world state: a surface repair site or route changes only through its map/route owner after a canonical result.
- Level 5 saga state: a chronicle or ending references the incident only if a durable fact is enrolled through its accepted contract.

One story should not jump from Level 0 tell to Level 3 cascade through prose. Every crossing requires a named command/result owner and an observable acceptance test. A visible machine response may naturally influence player decisions, but the authored plan must not imply unimplemented cascades.

UI command sequence: display the observation with timestamp/source; show the record and speaker attribution; present response intent; reveal resource/time costs before commit; require confirmation for irreversible service actions; return either a committed receipt or concise failure reason; refresh the reading and available responses; append a journal line only after its owner confirms. On failure, focus stays on a sensible actionable response, close/back remains functional, and the player can still access the source record. Do not disable the whole panel because one service command is blocked.

Consequence examples:
- “Preserve the report” records only that the player chose to keep the note, if the journal supports that exact fact. It does not certify the diagnosis.
- “Ask for inspection” creates no state until a service owner accepts a valid inspection job; the UI distinguishes requested, scheduled, and completed.
- “Use a spare” routes through a validated repair operation; if no such operation exists, this option is not shown.
- “Tell the crew it is safe” is never a consequence in the MVP because neither dialogue nor quest owns a safety certification.
- “Do not name the witness” is a narrative line until an access policy owner can enforce identity masking.

Branch result needs stable identifiers for idempotent journal/quest progression. Prefer an existing command receipt ID; if none exists, do not invent a random identity inside UI code. For content design, use an authored response ID plus accepted quest transition to reason about exactly-once behavior, then have implementation owners decide the command contract.

Do not make a failure path punitive by silently degrading another machine or consuming shift labor. If a service owner reports an outage or consumed item, that actual result may create a complication through its own event path; the quest can narrate it after it happens. The implementation must test all side effects together and then perform a fresh provenance audit so narrative text and current state agree.


### Pass 32C — Integration slices and closed-loop proof

Phase zero inventories the seven machine identities, condition keys actually populated in host readings, tell/glitch eligibility, one-shot journal note handling, room/category projections, exact discovery consumers for maintenance corpora, and machine-specific service commands. Deliver raw-ID crosswalk and unmatched-join list without changing data.

Phase one is a read-only scene: show one existing diagnostic source and one discovered authored record. No machine command runs. Prove missing/stale evidence falls back safely. Phase two binds one real inspection or repair command; show cost/downtime, revalidate immediately before commit, consume one result, and advance the quest only after success. Test accepted, insufficient supplies, stale condition, unavailable schedule, double-submit, and restored-state paths. Phase three refreshes the current reading and distinguishes requested, scheduled, and complete. Phase four adds other machine families or Chronicle callbacks only after the coverage census and durable event contract exist.

Acceptance table columns: publisher, payload, freshness, consumer, durability, visible output, failure fallback. Example loop: machine owner provides current fuel; host builds a reading; tell logic projects it; journal records supported knowledge; quest checks that fact; service owner validates action; UI shows its receipt; save restores durable owners; view recomputes from restored state. Missing edges remain explicit gaps.

Handoff states achieved severity: flavor, knowledge, quest, machine mutation, or saga consequence. Name evidence for every achieved level and list higher levels intentionally deferred. A catalog that loads and a panel that shows a tell do not prove closed-loop integration. If no current machine command can perform the proposed repair, close the story as “finding documented” or “decision deferred”; do not introduce a generic maintenance authority, shared wear ledger, or cross-machine repair facade.


### Pass 32D — Observable routing matrix closeout

For each outcome, record expected before/after facts by owner: journal knowledge, quest stage, machine condition, inventory, character relationship, and visible UI. Unchanged owners should be explicitly marked unchanged. A scene can be narratively complete while the machine remains unchanged; the journal must say so. A failed command should preserve balances and machine condition unless its owner returns a documented partial result.

Integration evidence should include both the accepted path and the rejected/stale path, save/reload after a durable result, and a screenshot or headless-visible output from the actual UI seam if applicable. A passing content validator proves references and field shape, not player reachability or consequence routing. Keep that distinction in closeout claims.
