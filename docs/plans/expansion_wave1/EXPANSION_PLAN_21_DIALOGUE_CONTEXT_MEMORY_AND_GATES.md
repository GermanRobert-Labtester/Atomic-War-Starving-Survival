# Expansion-series Plan 21 — Dialogue Context, Memory, and Gates

**Status:** Design proposal; documentation only.
**Numbering note:** This is Expansion Planning Wave 1, Plan 21.
**Purpose:** Define how a conversation selects authored variants based on relationship, knowledge, reputation, faction, location, quest history, repeated visits, player actions, skills, and character state without adding parallel memory or world-state stores.

## 1. Context is a read, not a new authority

Conversation selection should consume an immutable DialogueContextSnapshot assembled from the current domain owners when a scene opens or a node is evaluated. It is a short-lived read model with explicit provenance. It is not saved as another state section and cannot mutate its sources.

Candidate context fields:

- canonical speaker and current player/party IDs;
- current location ID and the location's knowledge/access state;
- current quest instance IDs and supported lifecycle facts;
- relationship facts exposed by the current relationship owner;
- faction affiliation and standing exposed by the faction owner;
- known information or clues exposed by the knowledge/discovery owner;
- skills exposed by the skill owner;
- memory facts exposed by the NPC memory or journal owner;
- supported character state such as alive, present, assigned, or available;
- campaign facts exposed by the campaign owner.

Only gather fields used by the selected conversation. Do not copy a full campaign snapshot into every dialogue object.

## 2. Condition language

Conditions are declarative, typed, and intentionally small. A condition can compare a supported fact with an allowed value, test whether an authored fact exists, or require a specific quest status. It cannot run arbitrary expressions, call methods by name, or read private implementation details.

Supported gate categories:

1. Relationship threshold or named relationship fact from the relationship owner.
2. Knowledge or evidence requirement from a knowledge/discovery owner.
3. Reputation band or access permission from a faction owner.
4. Faction membership/affiliation resolved through its current source.
5. Location, route, access, or discovery state from the map/location owner.
6. Quest accepted, in progress, blocked, failed, completed, expired, abandoned, or resolved status from the quest owner.
7. Repeated visit or prior conversation, only if a canonical owner can prove the event.
8. Remembered player action, keyed to a saved event or existing memory record.
9. Skill band or capability exposed by the skill owner.
10. Character availability and supported state from the roster/duty/health owners.
11. A hidden emotional presentation context, only if an existing character or relationship owner already exposes a supported state.

Unknown facts, missing actors, unloaded optional catalogs, and legacy saves must have an explicit fallback. A missing condition source does not pass a gate by default. For optional branches it selects a neutral line; for required content it produces a clear unavailable reason and a safe exit.

## 3. Repeated visits, failed quests, and remembered actions

Repeated-visit behavior must answer two separate questions: is this the first time in this conversation, and did the player make a choice that later content must remember? A panel's local open count may support a cosmetic greeting during the current session, but it cannot drive saved narrative consequences. Durable repetition requires an existing source that records it or a new owner decision.

Failed-quest dialogue queries the quest owner's actual lifecycle fact. It may offer repair, apology, a new clue, or a changed relationship scene. It may not turn failure into completion just by showing a sympathetic line. A quest's failure consequence and a dialogue's interpretation of it remain separate facts.

Player-action memory uses stable event IDs such as an accepted promise or delivered item only when an existing system records that fact. If there is no durable source, write a one-time reaction within the current quest event or defer the callback. Do not add a dialogue-specific event ledger by default.

Hidden emotional state is a writing technique first. A character may soften a sentence, avoid eye contact in a supported animation, or choose not to mention a topic when an existing context justifies it. If the game has no authoritative emotional-state field, dialogue should not invent one or save an inferred mood. The line is selected from observable facts and authored presentation.

## 4. Evaluation rules

Evaluate conditions in a documented order: validate node and speaker; resolve hard availability; evaluate required quest/location gates; evaluate knowledge, faction, relationship, skill, and memory variants; select the most specific eligible authored variant; otherwise use the explicit neutral fallback. If two equally specific variants match, use stable author order or the current seeded selection contract. Do not use dictionary iteration.

Condition evaluation is read-only and deterministic for one snapshot. Reopening a scene may produce new text only after an owner fact changed. Displaying a line never consumes knowledge, increments reputation, or records a visit by itself.

Content validation should check:

- every referenced actor, location, quest, faction, skill, fact, and text key exists;
- every gated node has a fallback or proves its gate is mandatory;
- condition combinations are not contradictory or unreachable;
- specificity does not leave ambiguous matches;
- failed/expired/abandoned variants point to the intended state;
- missing optional sources resolve safely;
- repeated-visit claims have a current owner;
- hidden emotional-state conditions have an actual authoritative producer.

## 5. Small prose example

Provisional character voice: a route clerk who is precise about times and evasive about blame.

- No prior clue: “That mark could be rain. Come back if you find the matching edge.”
- Clue known: “Same cut, lower on the wall. Someone wanted the second reader to find it.”
- Skill gate met: “The spacing is a hand's measure. Not a surveyor's mark.”
- Related quest failed: “You went. You came back without the case. Tell me what you did find.”
- Prior promise recorded: “You said the copy would stay in the shelter. I kept the shelf clear.”

These are selection examples, not approved canon or final asset strings. Each variant requires a supported condition fact. Without that fact, the line stays out of the catalog.

## 6. Minimum viable version and optional expansion

**MVP:** Build the context from one current quest fact, one location/knowledge fact, and one existing relationship or faction fact; select among authored line variants with a neutral fallback. Keep conversation memory transient unless an existing owner already persists it.

**Optional expansion:** More skill-sensitive lines, failed-quest callbacks, saved-action memory, repeated-visit variation, broader faction voices, accessibility hints, authoring tools, analytics for unseen lines, and hidden emotional presentation. Each addition requires a named source owner and tests for missing/old data.

## 7. Integration course

**Phase 0 — Owner map:** Trace each requested condition to its current API, persistence, and event producer. Record unavailable categories rather than fabricating adapters. Check active claims for quest, relationship, memory, faction, knowledge, and skill owners.

**Phase 1 — Snapshot contract:** Define the minimal immutable context for one pilot conversation and document fact freshness. Derive values from current owners at the host/Core boundary.

**Phase 2 — Condition schema:** Add only supported typed predicates to the selected dialogue catalog; provide strict validation and explicit fallback semantics.

**Phase 3 — Read-only resolver:** Evaluate conditions deterministically in Core or the existing narrative owner. No gate evaluation writes state. Send selected text and allowed responses to the existing UI.

**Phase 4 — Memory and save gate:** For every callback requiring memory, identify the current save owner and migration path. Defer anything without one. Test old save, missing actor, failed quest, and repeated visit.

**Phase 5 — Acceptance and handoff:** Run focused condition/loader/UI checks after claims are assigned. Record branch coverage, source facts, unknown-condition handling, commands, and limitations.

## 8. Acceptance

Every displayed variant has a traceable source fact; missing context chooses a safe fallback; repeated display has no side effect; current quest failures remain failures; condition evaluation is stable; durable memory is owned once; and UI explains unavailable choices without leaking hidden information. Documentation-only work runs no tests.

## Continuation pass 2 — condition truth table and fallback policy

### Condition evaluation table

| Gate | Read from | Unknown or missing source | Dialogue fallback |
|---|---|---|---|
| Relationship | Current relationship authority | Unknown is not treated as friendly or hostile. | Neutral greeting; no locked reward claim. |
| Knowledge/evidence | Current knowledge or discovery owner | Missing clue remains unknown. | Ask a question or point to a discoverable lead. |
| Reputation/access | Current faction authority | Missing faction ID is invalid content. | Use a non-faction-specific line or explain lack of contact. |
| Affiliation | Canonical character/faction binding | Absent optional actor closes only that branch. | Delegate, delay, or safe exit if the quest requires them. |
| Location | Map/location owner | Unknown site cannot be inferred from a quest string. | Show clue marker or named route blocker. |
| Quest status | Quest owner instance and lifecycle | No matching instance means no status condition. | Offer the correct start line or neutral response. |
| Repeated visit | Existing durable history, if available | No history means “unknown,” not “first visit.” | Use a noncommittal greeting. |
| Player action memory | Saved canonical event | Absent event, including old save, remains unknown. | Do not accuse or credit the player; use neutral variant. |
| Skill | Current skill owner | Missing skill definition is an authoring error. | Show baseline explanation without implying inability. |
| Character availability | Roster, duty, health, and location owners | Conflicting owner facts require a stable precedence contract. | Do not surface a conversation entry point for an absent speaker. |
| Emotional presentation | Existing supported character fact, if any | No fact means no hidden emotion gate. | Let authored wording carry subtext without state dependence. |

### Specificity and overlap

Use specificity to choose between variants without accumulating a growing priority score. A node may have one base line and a small set of mutually exclusive variants. Prefer explicit ordered predicates such as “failed quest,” “known clue,” then “base.” The order must be written in the catalog or fixed resolver contract. If two variants have equal specificity and both match, report a content validation error unless an existing seeded selector is deliberately part of the design.

Avoid negative predicates that hide multiple states in one test, such as “not trusted” matching unknown, disliked, unavailable, and old-save cases. Model unknown, absent, and known-false separately where the owning source supports that distinction. Keep the number of condition dimensions small; splitting one scene into several nodes may be easier to test than combining relationship, skill, faction, location, and history into a single compound gate.

### Old-save and repeated-visit behavior

For a new condition key added after release, define the older-save default before the first data row ships:

| New fact | Existing save behavior | Reason |
|---|---|---|
| Optional clue knowledge | Unknown; baseline clue remains available. | Preserves discoverability. |
| Prior choice callback | Unknown; use neutral copy. | Avoids assigning an unrecorded choice. |
| Optional faction membership | Absent; hide that spoke and keep the core conversation. | Keeps optional content optional. |
| Quest resolved flag | Query the current quest owner; do not duplicate a copy in dialogue. | Keeps lifecycle coherent. |
| Visit count | Unsupported unless an existing owner records it. | Prevents UI-local memory from becoming progression. |
| Character emotional flag | Unsupported unless a canonical system produces it. | Avoids a hidden parallel state. |

A repeated line can vary for prose texture within the current session, but durable reactions need durable facts. If the same line is displayed after a load, it must not advance the quest or change a relationship again. Display is not a confirmation event.

### Authoring examples

- Relationship gate: if current owner says the character will discuss repair plans, reveal the practical offer. If absent or unknown, use the shared introductory line.
- Failed quest gate: if the quest owner reports Failed, acknowledge the attempt and offer its authored alternative. If Expired, mention the missed timing rather than accusing the player of abandoning it.
- Skill gate: if the skill owner grants a supported interpretation, offer a more technical line. Keep the objective solvable without that skill response.
- Memory gate: if a canonical event proves the player returned a copied register, the clerk thanks them. If the fact is missing, ask whether they have seen the copy rather than asserting either outcome.

These examples require proof that the fact and wording are available. They are not permission to introduce four new memory systems.

## Continuation pass 2 acceptance

The gate policy is ready for implementation review when every condition points to one source, all unknown cases are defined, matching rules are deterministic, old saves use neutral fallbacks, and repetition cannot apply side effects. If a desired gate lacks a current source, defer that gate and keep the scene playable.

## Continuation pass 3 — context snapshots and condition conformance

### Snapshot timing

Build the dialogue context at a defined boundary. For scene entry, gather the facts needed to decide whether the scene is available. Before a player response that can change gameplay, refresh the subset of facts required by the effect owner and validate the response again. For a purely cosmetic response, the entry snapshot can be retained for the open scene so the player does not see a line change midway through reading.

This creates two useful views:

- **Presentation snapshot:** stable for the currently open scene; used to select text and explain current options.
- **Commit snapshot:** fresh enough to validate an accepted quest, item transfer, faction action, or map change.

The presentation snapshot is not a reservation. The commit snapshot does not require copying the full world. If an owner fact changed, the response can be rejected with a clear refresh line. A map pin calculated at expedition dispatch is validated by the expedition and map owners at dispatch, not by a stale dialogue scene opened earlier.

### Three-valued fact semantics

For narrative gates, a fact can be true, false, or unknown. Unknown includes absent optional content, unsupported old-save history, unavailable owner data, or an ID not recognized by the current loader. It is not equivalent to false for every story question:

- “The player has delivered the parcel” may be false if the quest owner proves it has not happened and unknown if no instance exists.
- “The player refused the offer” is unknown on an old save if no refusal was recorded.
- “The route is open” is false only when the map owner reports closed; missing route data is a content error.
- “The speaker is present” is false when the roster/duty owner says absent; missing actor ID is invalid authored content.

The condition language should distinguish these cases without supporting a general-purpose boolean scripting engine. Use explicit operators from a small set: equals, belongs to approved range, exists, and is unknown. Avoid nested arbitrary expressions and negation over unknown values.

### Gate grouping

Place conditions in three reviewable groups:

1. **Scene availability:** Is this scene allowed to open at all? Speaker, location, campaign phase, and required quest should be handled here.
2. **Response availability:** Which player actions are currently possible? Knowledge, skill, faction access, relationship comfort, and current objective facts may gate specific responses.
3. **Text variation:** Which authored line is shown for the same action? Tone, remembered action, or previous failure may select variants without adding/removing player control.

This grouping helps distinguish hidden content from disabled choice and line variation. A required scene with a missing optional spoke still opens. A disabled response includes a short reason when the player can reasonably act to unlock it. A text variant does not change the available response set unless that behavior is explicitly authored.

### Gate validation matrix

| Situation | Scene | Response | Text | Required behavior |
|---|---|---|---|---|
| Required speaker absent | Hide/defer entry | No response | Use delegate or unavailable copy | Do not create a replacement character. |
| Optional character absent | Keep scene open | Remove only that character's spoke | Use neutral variant | Preserve core conversation. |
| Relationship value unknown | Keep scene open | Keep baseline responses | Neutral tone | Do not infer trust or hostility. |
| Skill threshold met | Keep scene open | Expose optional explanatory question | Technical variant may be selected | Do not make the quest unsolvable without it. |
| Faction access lost after scene opens | Keep presentation stable until choice | Revalidate at commitment | Explain changed access | No standing effect is applied. |
| Quest changes in another system | Refresh status from quest owner | Reject stale command if invalid | Use current lifecycle response | Do not apply a terminal transition twice. |
| Secret destination ID known internally | Keep the clue scene available | No exact-map response until earned | Continue with clue wording | Preserve map knowledge boundaries. |
| Prior choice is absent in legacy save | Keep main path available | Do not invent a callback choice | Unknown/neutral line | Maintain continuity without accusation. |

### Character emotion without a hidden meter

Emotion can be presented through writing and animation without adding hidden quantitative state. If the roster, relationship, or mental-health authority exposes a named state relevant to dialogue, the resolver may read that state through its documented contract. If it does not, authors can still show controlled subtext from observable facts: a character pauses before answering, repeats a task instead of a name, or leaves a chair unclaimed.

The tone must not imply a mechanical relationship change unless the relationship owner reports one. A warm line does not secretly increase trust; an angry line does not secretly reduce it. When a character's emotional continuity matters across sessions, write a relationship or quest fact with an identified owner and capture/restore path. Otherwise, treat the scene tone as local presentation.

### Content coverage and performance

Create one test case per distinct source fact and one pairwise coverage pass for combinations likely to conflict, such as failed quest plus high reputation, absent character plus known clue, or old save plus optional faction. Exhaustive cross-products are unnecessary when each condition evaluator is simple and the interaction pairs are documented. Tests should report node/response ID and condition fact on failure.

At runtime, evaluate only conditions referenced by the current node, not every dialogue catalog entry. Cache immutable parsed predicates at load if the existing loader supports it. Do not cache mutable relationship or location values across owner changes. Log aggregate diagnostics rather than full conversation text or private memory details.

## Continuation pass 3 acceptance

Condition integration is ready when scene, response, and text gates are separate; unknown facts are explicit; commit-time effects revalidate; absent content preserves a playable route; hidden emotion remains authored or owner-backed; and condition coverage has bounded, useful diagnostics.

## Continuation pass 4 — predicate vocabulary, memory provenance, and test design

### Keep the condition language small and inspectable

Start with a bounded vocabulary of comparisons over owner-backed facts: fact exists; boolean equals; numeric threshold; ID membership; quest phase membership; location availability; relationship band; faction reputation band; skill threshold; and explicit all/any/not composition. Use stable identifiers for facts and comparison operands. Do not allow arbitrary script expressions, embedded code, direct file paths, or recursive user-defined functions in authored conditions.

Every predicate should declare its source owner and freshness expectation. A condition may read current health or carried-resource state only through the relevant owner; a dialogue panel must not infer a fact from visible inventory text or cached values. If the owner is absent, not loaded, or cannot resolve an old-save fact, return Unknown with a reason code. Define presentation behavior per gate: hide optional response; show disabled response with reason; offer neutral line; or block scene entry with an alternate route. The policy belongs to authored content and must not be globally guessed from the fact name.

Three-valued logic needs fixed behavior. In an all-group, any False makes the group false; otherwise an Unknown makes the group unknown. In an any-group, any True makes it true; otherwise an Unknown makes it unknown. Negation swaps True and False while preserving Unknown. This avoids treating missing data as false when that would incorrectly hide a recovery route, and avoids treating it as true when that would grant access or a reward.

### Context snapshot and commit-time revalidation

Build a short-lived read snapshot when a scene or node is presented. Include only referenced facts and their owner revision/event markers where available. Reuse that snapshot for rendering the node so two responses cannot flicker as different facts arrive mid-frame. Before an irreversible response is committed, re-read its required facts through the owner and evaluate again. If the result changed, keep the conversation open and explain that the situation changed; do not apply a stale consequence.

Snapshot lifetime ends when the scene closes, the relevant owner reports a change, or the player selects a response that requires fresh validation. Do not write the whole snapshot to the save. Persist only the canonical state in its existing owner. If a callback arrives while the scene is open, schedule a stable refresh and preserve keyboard/controller focus on the nearest still-valid response.

### Memory and emotion are different kinds of evidence

An NPC may remember that the player returned a tool, abandoned an escort, or shared a clue only if an existing relationship, quest, or memory owner stores that action. Dialogue can choose a tailored line from that fact. A line's emotional tone alone does not create memory, trust, resentment, or faction reputation.

Hidden emotional states should be handled as presentation categories with a declared source and fallback: a sourced relationship band; a current scene event; or an authored state local to this one conversation. Do not derive durable emotion by counting prior dialogue openings unless an existing memory owner explicitly records that history. If the source is unknown, select the neutral voice variant and preserve the available interaction.

Skill and reputation gates need player-readable outcomes. For example, a high fieldcraft skill may unlock a more precise question about a damaged latch, but the base route must still communicate the relevant clue. A faction-gated briefing can have a public summary, a refusal response, or a future access condition. Never place sole critical progression evidence behind an optional skill or reputation threshold.

### Test matrix for conditions

Use table-driven cases for each predicate operator with True, False, and Unknown outcomes. Add interaction pairs: hidden quest clue plus unavailable location; old save plus missing optional faction; failed quest plus high reputation; character absent plus known memory; owner changes between presentation and response; stale skill fact; and unknown condition inside all/any/not groups. Assert the chosen presentation policy, not only the boolean result.

For every gated node, identify at least one reachable test fixture for each intended variant and one safe fallback fixture. For commit-time effects, prove the second read prevents a stale operation. Keep diagnostics limited to IDs and reason codes needed by authors; do not dump private character memory, full player conversation history, or all hidden conditions into logs.

## Continuation pass 4 acceptance

The condition package is ready when its predicate vocabulary is bounded, missing facts use explicit three-valued behavior, node presentation is stable for its snapshot, irreversible responses revalidate through owners, and every optional knowledge or reputation gate preserves an understandable route.

## Continuation pass 5 — owner fact adapters, gate matrices, and freshness rules

### Read facts from their present owners

Treat the condition evaluator as a small interpreter over a supplied fact snapshot. It does not discover, calculate, or persist world truth. Each fact adapter asks an existing owner for a typed value and returns the value with a source ID, availability status, and freshness/revision marker when the owner provides one.

| Fact family | Source to verify during implementation | Example dialogue use |
|---|---|---|
| Quest phase/outcome | Current quest runtime or the specific questline owner. | Show evidence question while the task is active; aftermath after resolution. |
| Location discovery/availability | Current map and expedition owners. | Offer a clue instead of a pin when the destination is unknown. |
| Relationship/memory | Current relationship or NPC-memory owner. | Tailor a greeting after a recorded action. |
| Faction standing/access | Current faction authority. | Show a public summary and gate restricted details. |
| Skill/capability | Current player capability/skill owner. | Offer a more precise optional question. |
| Inventory/resource | Current inventory/resource owner. | Offer a donation request only if the item can be checked and transferred. |
| Time/deadline | Current world clock and owning quest deadline rule. | Change urgency copy after a real deadline transition. |
| Character presence | Current roster/actor availability authority. | Hide a live conversation and offer a journal/alternate contact path. |

These are owner families, not permission to create a universal fact registry or mirror. The implementation audit must name the concrete API and lifecycle for every predicate used by the first scene. If an owner cannot answer a needed question, mark that predicate unsupported and keep it out of released content until the architecture is approved.

### Predicate schema and authoring diagnostics

Each predicate reference should state: stable predicate ID; source owner; fact key; operator; typed operand; unknown policy; and optional author-facing explanation key. Group predicates with explicit all/any/not structure and a bounded maximum depth to make authoring review predictable. Content should not embed arbitrary expressions, method names, or source paths.

At load time, validate identifier syntax, source/operator compatibility, operand type/range, group shape, maximum depth, and that every referenced fact adapter exists in the selected content profile. At authoring time, show a human-readable rendering such as “the nurse memory contains the unresolved destination clue” alongside the stable predicate ID. Runtime should carry compact result codes, not full authoring explanations or private memory values.

Unknown is a first-class evaluation result. Distinguish at least unavailable owner, absent optional content, stale snapshot, malformed reference, and missing old-save state in diagnostics. These causes may share neutral player-facing behavior, but authors need enough information to repair content rather than accidentally treating all gaps as ordinary false.

### Presentation policies are authored per use

One predicate may need different presentation behavior depending on where it is used. An optional question can be hidden when unsupported; a meaningful but unavailable faction response may remain visible and disabled with a reason; a whole scene can use a neutral entry; and a unique recovery route should generally survive an unrelated unknown gate. Store policy at the scene/response use site rather than assigning one global policy to the predicate.

| Use | True | False | Unknown |
|---|---|---|---|
| Optional curiosity response | Show. | Hide or show a truthful disabled option by author choice. | Hide or show a neutral disabled option; never imply the player failed a check. |
| Required scene entry | Enter the contextual scene. | Use an authored alternate entry. | Use the neutral scene or a safe journal route. |
| Critical quest continuation | Offer the owner-backed next action. | Offer the quest's alternate/fail-forward action. | Preserve a clue, retry, or delayed route; do not strand the quest. |
| Irreversible commitment | Allow selection, then revalidate at commit. | Keep the response unavailable. | Do not commit; explain that the situation cannot be confirmed and preserve exit. |
| Cosmetic line variant | Use the variant. | Use the neutral/default variant. | Use the neutral/default variant. |

Avoid a generic “unknown is false” default for critical progression. It can erase the only recovery route after an optional faction or character is absent. Avoid “unknown is true” for access, rewards, or irreversible effects. The author must choose a safe presentation policy consistent with the effect scope.

### Unentered Shelf gate truth matrix

Apply one set of scene requirements to the shared fixture:

| Context | Briefing | Evidence branch | Commit/transfer response |
|---|---|---|---|
| Quest not accepted | Clerk can explain the shortage in public terms. | Hidden or framed as an optional clue. | Not offered. |
| Quest active, destination unknown | Offer tally and witness questions. | Show the nurse's uncertainty and a route clue. | Do not promise a destination or transfer. |
| Quest active, clue known, location undiscovered | Show the route clue, not an exact map pin. | Offer dispatch preparation or another evidence source. | Keep allocation uncommitted. |
| Destination discovered and expedition-eligible | Show destination-specific preparation if still valid. | Offer a check tied to current evidence. | Quest response may request a transfer only with explicit player confirmation. |
| Optional character absent | Use the clerk's neutral count and available records. | Do not infer that the absent nurse/mechanic disproves the clue. | Keep the quest route accessible through an alternate owner-backed source. |
| Quest failed or expired | Show the actual failure cause if available. | Offer the authored recovery route when eligible. | Do not present the old success commitment. |
| Quest completed with local-hold outcome | Show the confirmed local allocation line. | Keep the unresolved destination visible as a separate fact. | Do not replay the one-shot transfer. |
| Owner or save fact unknown | Use neutral explanatory copy and safe exit. | Preserve a retry/clue route where possible. | Revalidate and defer or reject without applying anything. |

This matrix makes the different dimensions visible: quest progress does not itself imply discovery; discovery does not imply expedition eligibility; character memory does not imply proof; dialogue selection does not imply transfer; and an unknown owner fact is not a failed player action.

### Snapshot consistency and invalidation

Collect only predicates referenced by the visible node and its available responses. Resolve them against a coherent read point when the current owners support it. If owners do not provide a shared version, record per-owner markers and define the invalidation events that force a refresh. Do not invent a global transaction or pretend unrelated owners supplied an atomic snapshot.

A response with only cosmetic or informational effects may use the displayed snapshot through the next stable interaction. A response that changes a quest, faction, relationship, location, resource, or ending fact must ask the responsible owner to re-evaluate its preconditions. If the owner rejects because facts changed, refresh the scene and explain the changed availability without erasing the player's earlier choices.

Close the snapshot when the scene exits, the relevant actor/location becomes unavailable, an owner publishes a relevant change, or a commit requests fresh validation. Snapshot invalidation must not clear durable state because it contains no durable state. On save/load, rebuild the snapshot from current owners instead of restoring cached truth.

### Relationship, reputation, skill, and memory authoring rules

Relationship facts, faction facts, skill facts, and remembered actions can all change a line, but they are not interchangeable:

- A relationship band describes a current interpersonal relation as defined by its owner.
- Faction standing describes access or trust with a group, not an individual NPC's private response.
- Skill can alter interpretation, available questions, or action feasibility only through the current skill authority.
- Memory records a supported prior action or observation, not an emotional conclusion invented by dialogue.
- An authored local emotional state can color one scene, but it expires with that scene unless an existing owner commits a durable consequence.

Keep the critical clue or quest continuation on a public route. Optional gates can add precision, intimacy, a shortcut, or a distinct interpretation. They must not transform a social statistic into a hidden requirement for basic progression. If a scene makes the player aware that a response is unavailable, give a concise reason without exposing exact hidden thresholds or confidential internal state.

### Reachability and live-change review

For each condition-bearing scene, author a compact gate table: intended fact combinations, visible responses, expected next node, and owner outcome. Review positive, negative, and unknown cases. Include at least one route through every critical outcome with optional character/faction/skill content absent. Validate loops and revisits so stale choices disappear after commitment but unrelated questions remain available.

During implementation, focused tests should exercise predicate operators independently, group semantics, old-save unknowns, absent optional catalogs, stale snapshots, an owner change before commit, a rejected command, and a complete fail-forward path. Tests should assert presentation and owner calls separately: a visible response does not prove the command succeeded, and a successful command does not prove the right copy was shown.

## Continuation pass 5 acceptance

The context package is ready when every production predicate names a current source owner and typed contract, Unknown retains a safe authored behavior, the Unentered Shelf matrix preserves progress under missing optional facts, and snapshot refresh plus commit-time revalidation are independently reviewable. No mutable condition cache or private dialogue history is introduced.

## Continuation pass 6 — fact adapter contract, evaluation traces, and gate safety

### Predicate definition record

Every released predicate needs a definition record that is narrow enough for static validation and clear enough for writers:

| Field | Contract |
|---|---|
| Predicate ID | Stable content-facing ID; independent of prose and localized labels. |
| Fact family | Quest, map/location, relationship/memory, faction, skill, inventory/resource, time, or actor presence. |
| Source owner | The existing authority to query; not a panel, cache, or plan name. |
| Fact key | Typed field or bounded query supported by that owner. |
| Operator | One supported comparison with explicitly typed operand. |
| Evaluation stage | Scene entry, node presentation, response presentation, or commit-time revalidation. |
| Unknown policy | Hidden, disabled with reason, neutral fallback, safe alternate, or blocked with retry. |
| Freshness rule | Snapshot revision/event invalidation or always-read-at-commit. |
| Author explanation | Localizable/editorial explanation of why the fact matters. |

The record describes a read request; it does not register or own the underlying fact. Keep the first vocabulary deliberately small: boolean equality, numeric threshold, stable-ID membership, quest-phase membership, location disposition, relationship/faction band, and skill threshold. Add an operator only when a current owner produces the required type and a real scene needs it.

### Evaluation trace for a gated response

For diagnostics and focused verification, an evaluation trace can be represented as:

1. The graph identifies the predicates needed for the currently visible node and response list.
2. Fact adapters query the named current owners.
3. Each adapter returns True, False, or Unknown plus a compact source/reason marker.
4. The bounded group evaluator applies fixed all/any/not semantics.
5. The authored presentation policy maps the result to visible, hidden, disabled, neutral, or alternate-route behavior.
6. The scene projection records the relevant owner markers for refresh.
7. A selected consequential response triggers fresh validation through the effect owner.
8. The graph refreshes from the confirmed owner result.

The trace should be inspectable in development tools and test output without revealing full memory contents or hidden campaign state. A human-readable explanation can say “This response needs the route clue” without exposing an exact skill threshold. The runtime result should identify the stable predicate and reason code so the content defect can be reproduced.

### Conditions should answer one question each

Prefer atomic predicates with one owner and one meaning. “The player knows the nurse's account and has enough trust and can reach the depot” is three conditions and should be modeled as such. This lets authors use a neutral route when one fact is missing and lets tests prove each owner adapter independently.

Avoid predicates that:

- infer discovery from a visible map label;
- infer acceptance from an offer being displayed;
- infer a relationship from dialogue choice count;
- infer a deadline from day text rather than the clock owner;
- infer inventory from a button caption;
- combine several owners into a boolean with no trace;
- query arbitrary actor or world object names;
- depend on a random draw or text variant;
- treat a missing optional expansion catalog as player failure.

If a condition requires a fact that no current owner can supply, document the gap. Do not add a dialogue-local boolean just to make the branch selectable. The correct next step is either to choose an existing supported fact, keep the line ungated, or request a narrow architecture decision.

### Gate resolution and conflict rules

When several gates affect the same response, make order explicit:

1. Verify the scene/actor is available.
2. Evaluate safety and progression-critical conditions.
3. Evaluate knowledge and optional context conditions.
4. Apply authored presentation policy for each failed/unknown group.
5. Resolve conflicts by preserving a safe exit and a valid critical continuation.
6. Revalidate all irreversible preconditions at selection/commit.

An optional knowledge gate must not hide a required refusal or recovery path. An unavailable character should not cause an unrelated faction condition to become true. A stale or missing source should not be interpreted as a negative relationship. If two condition policies conflict, favor the policy that preserves a neutral route and flag the graph for authoring correction; do not invent a global priority based on predicate name.

### Memory fact minimization and respectful copy

Dialogue needs the smallest fact that supports the line. Prefer a coarse record such as “the player returned the borrowed tool” over a free-form transcript, date-stamped conversation history, or inferred emotional score. Use only memory already held by the current memory/relationship owner. New durable memory categories require owner review, save review, privacy review appropriate to the project, and a reason why existing facts cannot represent the event.

Keep player-facing copy focused on observable action: “You brought the tool back” is more grounded than “I can finally trust you.” A stronger relational statement is acceptable only if the existing relationship owner confirms the relevant band and the character's established voice supports it. If an optional memory fact is absent, the neutral greeting should still make sense and the scene should not accuse the player.

Store local tone choices inside the authored scene only for that scene instance. On exit, the tone is gone unless an existing owner records an explicit supported consequence. Returning later can select a different tone from a canonical quest result or memory fact, not from an unsaved counter of how many times the player opened the conversation.

### Gate coverage matrix for a scene bundle

Each bundle should enumerate the following fixtures where relevant:

| Dimension | Positive case | Negative case | Unknown/change case |
|---|---|---|---|
| Quest | Active phase exposes next action. | Terminal state removes stale commitment. | Old save lacks optional phase detail. |
| Location | Discovered and eligible target shows dispatch route. | Locked target does not appear as available. | Route changes after node presentation. |
| Relationship/memory | Sourced action selects tailored acknowledgment. | Known absence uses neutral wording. | Optional memory owner unavailable. |
| Faction | Access fact exposes restricted briefing. | Low standing offers public summary/refusal. | Faction content bundle absent. |
| Skill | Threshold grants an additional interpretation. | Base clue remains understandable. | Skill fact unavailable or refreshed. |
| Inventory/resource | Current stock permits a request. | No stock keeps the response unavailable. | Stock changes before commit. |
| Time | Active deadline shows correct urgency. | No deadline uses ordinary pacing. | Deadline crosses while the scene is open. |
| Actor | Present speaker can respond. | Missing actor uses a record/alternate route. | Roster changes before response. |

Do not add irrelevant tests simply to fill a matrix cell. Mark the dimension not used and explain why. This makes the matrix a review of actual dependencies rather than a demand that every conversation depend on every system.

### Refresh, focus, and player comprehension

When a fact change alters responses, preserve the player's current node and focus where possible. If the focused option becomes unavailable, move to the nearest valid option in a deterministic order and announce the reason in text. Do not silently select a new response. If the entire scene becomes invalid because its actor or location disappears, close through a clear neutral line and preserve any already-confirmed owner outcome.

For an irreversible choice, place its consequence preview next to the response in plain language. The preview describes the requested action and known cost; it does not guarantee acceptance. After a rejection, show a short reason and the next valid action. Do not expose hidden numeric thresholds or private memory facts in the disabled label. Keyboard and controller users receive the same explanation and available routes.

### Implementation boundary and acceptance evidence

The first implementation claim should name every predicate ID in the chosen scene and the precise source owner behind it. It should not begin by building a general-purpose fact catalog. Static content validation proves that referenced predicate definitions and adapters exist; focused owner tests prove the facts; graph tests prove the intended options; host tests prove refresh/focus; Plan 22 tests prove commit revalidation.

Record these evidence artifacts at handoff: predicate list and owners; True/False/Unknown fixture results; one old-save or missing-content case; one stale-fact rejection; one keyboard/controller refresh path; and confirmation that no condition snapshot is serialized as campaign truth. Keep any unsupported predicate in the authoring notes and out of the released graph.

## Continuation pass 6 acceptance

The gate package is ready when every predicate is atomic, typed, owner-backed, and assigned a freshness policy; evaluation can be traced without leaking private memory; conflict rules preserve critical and neutral routes; and graph, adapter, UI refresh, and commit validation evidence are reported separately.

## Continuation pass 7 — knowledge ladders, memory evidence, and contextual coverage

### Knowledge ladder for optional depth

When a scene has several kinds of context, author a knowledge ladder rather than a single hidden gate:

1. **Public baseline:** every player can learn the essential situation and next valid action.
2. **Clue-supported detail:** a discovered record, location clue, or completed interaction provides more precise information.
3. **Skill interpretation:** an existing skill/capability fact helps interpret evidence or adds a safe optional question.
4. **Relationship/memory recognition:** a sourced personal fact changes the speaker's tone or reveals an additional concern.
5. **Faction access:** an owner-backed standing/access fact may expose a restricted contact or resource route.
6. **Commitment gate:** a state-changing response revalidates all needed facts before it can be executed.

Each rung adds depth, convenience, voice, or an alternate route. No optional rung should be the only source of a critical location, deadline, survival requirement, or main-quest objective. If the content cannot preserve a base route beneath its gates, reduce the gate or move the content to a clearly optional expansion.

For the Unentered Shelf, the baseline tells the player that the tally and destination claim disagree. A record clue may show the number set aside. A skill response may distinguish correction marks from signatures. A nurse memory may establish that a promise was discussed. A faction contact may offer a second archive only when current access supports it. None independently proves that the neighboring shelter received the blankets.

### Evidence states for dialogue conditions

Use a small authored evidence vocabulary when reviewing dialogue claims:

| Evidence label | Meaning | Example wording |
|---|---|---|
| Seen | The player or a verified interaction directly observed it. | “You saw the seal intact.” |
| Recorded | A canonical record states it, with source and scope. | “The ledger lists six bundles set aside.” |
| Reported | A character or faction claims it. | “The nurse remembers that help was promised.” |
| Inferred | A speaker draws a conclusion from existing facts. | “That mark may be a correction.” |
| Rumored | An unverified lead supports further inquiry. | “Someone says the old depot road still connects.” |
| Contradicted | Two valid sources disagree, and the disagreement is still unresolved. | “The tally and the witness do not establish the same thing.” |
| Unknown | No supported fact answers the question. | “We do not have a receipt.” |

These labels guide text selection; they become runtime predicates only if existing owners can represent them. A dialogue condition should usually query the source event or owner state, not a new generic evidence score. Contradiction is not necessarily failure: the quest may intentionally let the player choose a cautious resolution.

### Memory provenance matrix

| Memory candidate | Minimum supporting event | Durable only if | Safe fallback |
|---|---|---|---|
| Player returned an item | Inventory/quest owner confirmed the return. | Existing memory/relationship owner records that event. | Neutral line without accusation. |
| Player investigated a clue | Quest/discovery owner confirms interaction. | Current quest or memory owner persists it. | “The file is here if you want to look again.” |
| Player abandoned an escort | Quest owner confirms abandonment/failure. | Current owner retains history and speaker can access it. | State only what the current journal proves. |
| Player shared a faction secret | Faction or quest owner accepted disclosure. | Existing faction/relationship authority records result. | Do not imply trust or betrayal. |
| Player visited repeatedly | No durable fact unless an owner records visits. | A real map/quest visit fact is available and relevant. | Use an ordinary greeting. |
| NPC felt embarrassed/afraid | Authored local dramatic state or sourced relationship outcome. | Durable emotion is explicitly owned and supported. | Use a neutral tone variant. |

Never infer moral character from a failed action alone. A player may fail because a route was unavailable, an optional actor was missing, or an owner fact was unknown. Copy can describe the action's consequence without assigning a permanent identity to the player.

### Context matrix for the shared scene

Use this compact matrix to author and review which fact changes which part of the scene:

| Context fact | Alters entry? | Alters information? | Alters commitment? | Base fallback |
|---|---|---|---|---|
| Quest not accepted | Yes, offer framing. | No evidence history assumed. | Acceptance request may be shown. | Public account and decline. |
| Quest active | Entry acknowledges the task. | Evidence options may be available. | Only current objective commands. | Continue investigation. |
| Destination discovered | No, unless location context changes. | Exact route question may appear. | Expedition request can be offered if eligible. | Clue route when known only as rumor. |
| Expedition route blocked | No. | Explain verified blocker. | Do not submit start command. | Keep task active or use explicit equivalent. |
| Skill threshold met | No. | Adds interpretation/question. | Never bypass owner preconditions. | Plain-language clue. |
| Witness memory available | Tone or sourced detail can change. | Clarifies what the witness recalls. | Does not prove physical receipt. | Neutral account. |
| Faction access granted | Optional archive/contact branch. | Restricted material may be shown. | Faction consequence still routed to owner. | Public information route. |
| Stock changed | No. | Current count can be reported. | Transfer command must revalidate. | Say count is not yet confirmed. |
| Quest failed/expired | Aftermath/recovery entry. | Failure cause if known. | Old completion choices removed. | Explain current route or close. |
| Local outcome accepted | Return line acknowledges result. | Other unresolved fact stays distinct. | One-shot transfer is not offered again. | Query owner; neutral if unknown. |

If a fact affects more than one stage, record all stages explicitly. A reputation gate that changes both node visibility and a consequence command needs a commit-time check; it is not enough to validate only when the scene first opens.

### Hidden emotional state: constrained usage

“Hidden emotional state” means the character's manner is not displayed as a numeric UI meter. It does not mean content can secretly infer or mutate an unowned emotional variable. Use only:

- authored line-level tone tied to the current scene;
- a current owner-backed relationship band;
- a sourced memory event;
- an immediate event result already accepted by an owner.

If an author proposes “afraid,” “guarded,” “relieved,” or “angry,” identify the source that justifies it and the route back to neutral. Avoid permanent personality changes based on one conversation unless an existing owner supports the arc. Keep voice distinct through sentence shape and what a character notices even when the relationship fact is unknown.

### Gate classes and disclosure policy

Classify each gate by purpose:

- **Progress gate:** required for the current action; it needs an alternate or clear prerequisite explanation.
- **Disclosure gate:** controls optional information; use hide/disabled/neutral presentation.
- **Convenience gate:** unlocks a shortcut while preserving the normal route.
- **Tone gate:** changes wording only; should be low-risk and use neutral fallback.
- **Commit gate:** protects a durable operation; always revalidate through the owner.
- **Safety gate:** prevents an action that is currently invalid or harmful in game terms; clearly explain the actionable condition.

Authors should not use a single boolean “available” gate to mix all six purposes. Separate facts make it easier to diagnose why a response disappeared and prevent an optional tone branch from accidentally blocking the quest.

### Focused fixture set for content authors

For each high-impact graph, provide named static fixtures:

1. Fresh save with no optional memories.
2. Accepted quest and unresolved evidence.
3. Discovery known but exact location not selected.
4. Location selected and preview valid.
5. Preview stale because an owner fact changed.
6. Supporting character absent.
7. Faction bundle/access absent.
8. Skill below optional threshold.
9. Skill above optional threshold.
10. Quest primary route failed, recovery route not yet accepted.
11. Recovery route active and destination unavailable.
12. Resource changed before commitment.
13. Result Applied and scene revisited.
14. Result AlreadyApplied after repeated input or restore.
15. Old save lacks an optional fact.

Fixtures should state facts and expected visible options without embedding implementation-only prose into the save itself. A fixture that cannot be constructed from canonical owners exposes a real integration dependency and should remain a design gate.

### Player understanding and disclosure review

When showing a disabled response, disclose only information the character plausibly understands and the design wants the player to know. “Need a route clue first” is useful; exposing a hidden ID, exact relationship score, or faction threshold is not. Do not mislead the player into believing a missing optional character has rejected them. When a response is hidden, preserve another route or make the information clearly optional.

Review critical information in transcript form. Every spoken clue should have a source reference in content notes; every unavailable response should have a tested presentation policy; every gate needs a neutral fallback; and no essential instruction may exist only in audio, animation, color, or a portrait expression.

## Continuation pass 7 acceptance

The knowledge/memory package is ready when optional context forms a depth ladder over a complete public route, every memory claim has event provenance, hidden emotional tone has an explicit source and neutral return, and fixtures cover the relevant combinations without creating a universal dialogue-history store.

### Cross-owner freshness without a fictitious atomic snapshot

Quest, map, roster, faction, skill, inventory, and memory owners may update at different times. A dialogue snapshot is therefore a stable presentation point, not a transaction guarantee. Store a marker per contributing owner when available and list the events that invalidate each family. If an owner exposes no revision token, re-read the relevant fact immediately before a consequential command and treat uncertainty as Unknown.

When several owners change at once, refresh once at a stable UI boundary rather than redrawing after every callback. Preserve the current node if it remains valid, recompute response availability, and move focus only when required. Do not combine values from different time points into a new durable fact. If the scene depends on a coherent cross-owner conclusion, the relevant domain owner must supply that conclusion.

This prevents dialogue from becoming an accidental coordinator of time, location, and relationship state. The graph can explain that “the route changed while we were talking” only after the map/expedition owner reports a material change.

### Condition review for unsupported or retired facts

If a predicate's source is removed or renamed, fail content validation before release and identify every graph reference. Runtime fallback for a supported old save should use the author's declared neutral/alternate route. Never reinterpret the retired fact ID as a different fact because the new label seems similar. If migration is necessary, the original owner or approved migration path supplies the replacement value.

Keep authoring diagnostics actionable: scene/node/response ID, predicate ID, missing owner or operand, and fallback chosen. Avoid full serialized state dumps. A useful gate system makes missing evidence easy to repair without teaching writers to bypass validation.

## Continuation pass 8 — lifecycle-aware gates, response availability, and review fixtures

### Gate behavior by quest lifecycle

Conditions often change as a quest moves through its lifecycle. Author the intended presentation at each lifecycle point instead of attaching a gate once to an entire scene:

| Quest phase or projection | Offer availability | Conversation options | Consequence policy |
|---|---|---|---|
| Inactive | No offer unless a discovery trigger exists. | Public/ambient conversation only. | No quest mutation. |
| Available | Offer entry can be shown. | Ask, accept, decline, or defer as authored. | Acceptance goes through the quest owner. |
| Discovered | Clue/source explains why the task is visible. | Review evidence and choose whether to engage. | Discovery and acceptance remain distinct. |
| Accepted/active | Current objective is shown. | Only options relevant to supported next actions. | Revalidate phase at commit. |
| Blocked | Missing prerequisite has a known explanation. | Offer alternate evidence, wait condition, or safe leave. | Do not repeatedly submit an impossible command. |
| Partial progress | Accepted evidence changes available questions. | Show completed evidence and remaining gaps separately. | Repeated evidence must be idempotent. |
| Failed/expired | Confirmed cause and history are available. | Remove stale success option; offer recovery only if real. | Failure stays recorded. |
| Completed/resolved | Current outcome drives aftermath. | Callback and optional review; no repeated one-shot choice. | Reopen only through a supported owner transition. |
| Abandoned | Owner confirmed explicit abandonment. | Restart/reoffer only if owner policy allows it. | Inactivity alone cannot produce this phase. |

Do not bind a node condition to a presentation label if the implementation only stores a smaller state set. Use owner-backed objective facts and a reviewed projection. If the phase is Unknown, the safe route should be neutral, not a guessed transition.

### Availability is not the same as selectability

A response can be presented in four ways:

- hidden because it is optional and unavailable;
- visible but disabled with a truthful reason;
- visible and selectable for information only;
- visible and selectable for a consequential request that will be revalidated.

Use hidden for secrets the player has not earned a clue to notice. Use disabled when the player benefits from learning a prerequisite. Use informational responses when no durable mutation is requested. Use commit responses only when the preconditions can be checked again by the owner. Never visually imply that a disabled choice is a punishment for relationship, faction, or skill unless that is the intended readable game rule.

For controller/screen-reader parity, available and disabled states must be announced in words; color or icon alone is insufficient. A text reason should be concise, not a dump of internal predicates.

### Context-stability cases

For each scene, identify which facts should be frozen for the open conversation and which facts require a current read:

- Speaker identity and authored voice are stable during the scene.
- Current quest phase can be snapshotted for display but must be re-read for mutation.
- A location marker can be snapshotted for a question but map/expedition must revalidate dispatch.
- Current skill may be refreshed when a player changes loadout or the capability owner updates.
- Inventory is always rechecked before transfer or recipe commitment.
- Relationship/memory tone can remain stable until the scene ends, except when the owner reports a relevant accepted event.
- A deadline crossing is a material time event and should refresh only after the clock/quest owner reports it.

Do not refresh every field on every UI frame. Define invalidation events and refresh at stable interaction boundaries. For high-cost reads, gather only facts referenced by the visible node and candidate responses. A cache may be a short-lived presentation optimization only if it is invalidated correctly and never becomes canonical save state.

### Relationship/faction/skill gate examples

**Relationship:** A returning character may acknowledge that the player completed an earlier promise if the memory owner records it. Without that fact, the character gives a neutral greeting. The quest objective remains visible in both cases.

**Faction reputation:** A guarded archive response may be disabled until the faction owner reports access. The public explanation remains available. If the faction catalog is absent, no response should reveal an expansion-only ID or block the base quest.

**Skill:** A fieldcraft skill can show a second interpretation of a route sketch. The baseline still communicates that the sketch is old and needs a current map check. Skill affects the extra question, not the truth of the route.

**Failed quest:** The player can ask what happened only if the quest owner provides the failure cause. A missing cause uses neutral history copy. A recovery option is shown only when the follow-up task and its location policy are valid.

**Repeated visit:** A later greeting changes only when a canonical outcome or visit fact supports it. Reopening a scene ten times cannot create trust, fatigue, or a new response count.

These examples all use the same gate evaluator but different source facts and disclosure policies. They should not become global “relationship gates,” “skill gates,” or dialogue counters with independent saved state.

### Coverage fixtures and expected outcomes

For a critical quest conversation, define at least one explicit fixture for each materially different truth:

1. All required facts known and every relevant owner present.
2. Optional relationship/memory absent but base route playable.
3. Location known but blocked, with a clue or delay route.
4. Skill route available, plus the baseline route without skill.
5. Faction access available, plus public route without access.
6. Quest phase changed while the scene was open.
7. Deadline crossed between response presentation and selection.
8. Resource changed before a transfer response.
9. Character disappears after conversation entry.
10. A prior one-shot outcome is already applied on restore.

Each fixture states the exact visible/disabled/hidden responses, expected next node, and owner calls. Mark non-applicable dimensions rather than inventing state just to fill the matrix. Reviewers can then compare text and actual branch behavior without needing to infer the intended gate from prose.

### Gate-change copy and recovery

When an option becomes unavailable during conversation, explain the changed fact with the shortest accurate sentence: “The route is closed now,” “The stock count changed,” or “The task was already settled.” Provide a next valid route if one exists. If no action is possible, leave the conversation safe to close and make the journal/map state consistent.

Do not call a changed fact “Unknown” to players when the game actually knows the route is blocked. Unknown is for missing/unresolvable evidence, not for softening a negative result. Equally, do not say “you lack trust” when the real state is that the optional relationship owner is absent.

### Composite condition design and branch ownership

When a response depends on multiple facts, express the logic in small named parts. For example: “quest active” AND (“record inspected” OR “witness account accepted”) AND “location can be requested.” Keep each component atomic and sourced. The branch owner is the dialogue graph; the condition engine evaluates the expression; the quest, evidence, and map owners remain authoritative for each input.

If an OR group contains two alternate routes, each route must lead to an outcome that satisfies the same stated objective or to an explicitly distinct resolution. If one source only proves that a promise was made and the other proves delivery, they cannot share the same objective result. Use separate evidence IDs or separate terminal outcomes.

Avoid combining an optional relationship threshold into a mandatory progression AND group. If the player lacks relationship context, the graph should select its public or neutral path. If a faction condition is required for a particular agreement, give the player a readable refusal or alternate term.

### Hidden response disclosure ladder

For content that rewards exploration or character familiarity, disclose progressively:

- Before a clue, use ambient prose that establishes place and mood without naming hidden content.
- After a clue, offer a general question or regional hint.
- After exact discovery, reveal a named location only if the map owner confirms that knowledge.
- After an accepted task, show the actionable objective and any real prerequisites.
- At commitment, revalidate current facts and present a plain consequence preview.

Do not hide the only way to learn a required quest verb, survival rule, or main-story deadline. A secret can reward a curious player with lore, shortcut, or optional result while the core path remains complete.

### Gate data review checklist

Before graph release, review every response gate for: stable predicate ID; current fact owner; expected type and comparison; scene/node/response evaluation stage; presentation policy; unknown behavior; localization of unavailable reason; optional-content behavior; refresh event; commit-time validation; test fixture; and fallback route. Mark cosmetic line variants separately from availability gates.

Review negative outcomes just as carefully as positive ones. A “False” result should mean the owner knows that the condition is unmet. “Unknown” should mean the system cannot safely determine it. This distinction is essential for old saves, absent expansions, unavailable actors, and asynchronous owner changes.

### Predicate precision and scope

A predicate should describe one fact at one scope. “Quest active” refers to a canonical quest instance, not every task with a similar template ID. “Location known” refers to map knowledge for a stable location, not whether it appeared in this dispatch. “Faction access” refers to a current group-level fact, not an individual speaker's mood. Include the scope in the predicate definition when IDs could otherwise be ambiguous.

If an operator compares ordered bands, define the order in the owning domain contract; do not assume display labels have a stable numeric meaning. If a fact is a set, compare stable IDs rather than localized names. If a number has units or a cap, include that in authoring documentation so a threshold cannot accidentally mix days, items, and points.

## Continuation pass 8 acceptance

Lifecycle-aware gates are ready when each presentation label projects from a verified source, selectable and visible states are distinct, snapshots refresh only on meaningful owner changes, and every critical fixture asserts both player-visible availability and the owner calls required by the selected response.

## Continuation pass 9 — condition decision record, change precedence, and author diagnostics

### Condition decision record

For every nontrivial scene, keep a compact record of why its conditions exist:

| Decision | Required author answer |
|---|---|
| Gate purpose | Progress, disclosure, convenience, tone, commit safety, or content compatibility. |
| Player knowledge | What does the player reasonably know before this response appears? |
| Fact source | Which current owner proves the condition? |
| Alternative | What can the player do if false or unknown? |
| Player message | Is the reason useful and appropriate to reveal? |
| Revalidation | Does selection require a fresh owner read? |
| Test fixture | Which owner facts make each branch reachable? |
| Removal behavior | What happens if the predicate or optional owner is retired? |

The record discourages gates added only to make dialogue look reactive. A condition should change information, route, availability, or supported consequence. If it changes only punctuation or an adjective, make it a text variant and keep the predicate out of progression logic.

### Condition precedence under competing constraints

When several predicates govern one response, apply them by purpose:

1. Content/owner compatibility: is the referenced scene and command supported in this build?
2. Actor and location validity: can this interaction occur in the current context?
3. Critical quest eligibility: does the quest owner allow the next action?
4. Optional information: can the player see a deeper explanation?
5. Commit safety: do current facts still satisfy the owner command?
6. Presentation: which line and availability message best explain the result?

This is an authoring order, not a replacement for each domain owner's rules. A content compatibility failure blocks the response and should be caught before release. A missing optional relationship fact should use neutral copy. A quest phase rejection should refresh the action. A final commit check belongs to the owner. Never let a low-priority tone condition override a valid exit or recovery route.

### Unknown propagation scenarios

Test Unknown distinctly for the same authored gate:

- The owner is not loaded yet: wait for its normal lifecycle or show a neutral entry.
- The optional expansion is absent: select its declared core fallback.
- The old save lacks a newer optional fact: migrate through the owner if supported, otherwise use neutral copy.
- The stable ID is malformed: validator error; do not treat it as ordinary absence.
- The owner data changed while the node is open: invalidate and reevaluate at a stable boundary.
- The condition source timed out or failed operationally: preserve safe exit and log compact reason.

These cases may share a fallback line, but they must remain distinguishable in diagnostics and test names. Otherwise writers cannot tell whether a branch was unavailable by design or broken by an integration gap.

### Author tooling requirements

A useful condition authoring view should show: predicate ID; owner/fact key; typed operator and operand; gate purpose; True/False/Unknown presentation; response(s) that consume it; and fixture reachability. It should highlight unsupported source owners, cycles in authored condition references, impossible groups, unreachable responses, and critical routes that rely on optional facts.

The tool should not evaluate private campaign memory from arbitrary live saves merely to preview content. Use synthetic, named fixtures built from public test contracts. Diagnostic export should identify content IDs and reason codes without serializing all hidden state. If no authoring tool exists, keep the decision record and fixtures in the content review package until a separately owned tooling task is approved.

### Dialogue-condition regression contract

For every schema or owner change, compare branch truth tables and expected visible responses for the affected scenes. If a predicate's meaning changes from “known location” to “selected opportunity,” its stable ID should not be silently reused. Introduce a new ID or explicit migration, update the associated copy, and retest exact marker, clue-led, blocked-route, and no-candidate cases.

Snapshot caches must not survive the scene lifecycle. On reopen, rebuild from current owners. If a result was already committed, the response may remain visible as history or be replaced by an aftermath option, but it cannot submit the same operation again. This regression contract protects both deterministic replay and player trust.

### Player-facing explanations for gated responses

A gate needs an understandable presentation when it affects an action the player expects to take. The explanation can name a missing practical requirement, clue, unavailable person, or blocked route. It must not reveal protected information merely because the evaluator can read it, and it must not expose an internal predicate name or raw owner result code. If the reason itself is hidden by design, offer a neutral line that preserves the player's ability to leave or pursue another lead.

| Gate category | Useful explanation | Safe alternative |
|---|---|---|
| Progression prerequisite | “The board is still missing the route mark.” | Ask where it might be found or leave. |
| Actor unavailable | “Mara is away with the survey group.” | Return later or use another supported contact. |
| Knowledge disclosure | “You have not found a source that confirms that.” | Ask what is known or inspect a relevant location. |
| Faction access | “The gate crew has not logged your passage.” | Ask for terms or take an already open route. |
| Commit safety | “The stores count changed; check the amount before you confirm.” | Refresh the offer and decide again. |
| Optional memory unavailable | Neutral greeting with no claim of recognition. | Continue the ordinary conversation. |

Unavailable, blocked, and unknown states need different copy when they imply different next actions. “Unavailable” fits a closed optional topic; “blocked” means an accepted commitment remains pending; “unknown” means the system cannot truthfully evaluate the fact yet. Do not show a disabled control without a focusable explanation when keyboard/controller users need to understand why it cannot be selected.

### Disclosure and hidden-state review

Review both logical correctness and information release. A response can be logically allowed while disclosing a secret faction contact, private memory, or undiscovered location too early. Hiding every gated response can also make the interface look empty and prevent the player from learning what would help. Select among three patterns: visible locked response with an approved reason, omitted response with a contextual clue, or neutral fallback that does not acknowledge a secret branch.

Record the disclosure tier as public, learned by the player, known only to the speaker, or protected until a reveal event. This is a content-review label, not a new world-state authority. At runtime, the current knowledge, discovery, faction, or relationship owner remains authoritative. If optional memory is absent in an old save, missing data must not be treated as proof that the player never acted; use a supported migration or neutral version.

Skill-conditioned dialogue needs the same scrutiny. The player should understand why a response is available through contextual cues, recognized expertise, or a clear status marker. Do not use skill gates to hide essential instructions when the baseline route has no alternative. A skill branch may add a diagnosis, challenge a witness, identify a material, or reduce uncertainty; it should not make the base scene incoherent or imply that an unavailable response was morally inferior.

### Gate review fixture: context changes while a scene is open

Build a focused fixture with an optional information response and a command-bearing confirmation. While the scene is open, change one relevant owner fact through the established fixture path: reveal a clue, revoke temporary access, complete the task elsewhere, or alter resource readiness. Re-evaluate at the defined stable boundary. Remove the stale response, disable it with an explanation, or let the owner reject submission; never accept it from cached pre-change facts.

Then close and reopen the scene. Reconstruct the visible response set from current owners. A learned fact may remain available as history after its original clue route closes; a committed command displays its accepted result without resending it. Repeated visits can change a greeting through a supported memory fact, but cannot advance a quest or relationship solely because the scene reopened.

Name fixtures after the player-visible contract: known-and-accessible, known-but-route-blocked, unknown-with-neutral-fallback, stale-before-confirm, accepted-elsewhere, optional-owner-disabled, and old-save-without-optional-memory. Record response IDs, explanation class, safe exit, command count, and owner result. This keeps content review, UI behavior, and predicate meaning aligned without coupling tests to a private save dump.

### Gate scope and player expectation audit

For every gate, ask what the player expects to happen after selecting the response. A pure information choice may reveal text and return to the hub. A quest choice can create an offer or send a command only if the node explicitly tells the player that it is making a commitment. A relationship condition can select a tone variant but cannot imply a reward unless the relationship owner confirms one. A faction condition can reveal an available negotiation; it cannot represent standing as a dialogue-local boolean.

Review gates in the context of the entire interaction, not as isolated predicates. If the player is shown a response, then the owner rejects it every time for a known missing prerequisite, the gate should ordinarily explain that prerequisite before the player commits. If the rejection is genuinely dynamic, the scene can revalidate and show current facts. If the outcome is intentionally uncertain, explain what is uncertain and avoid presenting a guaranteed success label. This reduces false affordances while preserving legitimate surprises.

When a node has several predicates, identify which one controls visibility, which one controls disclosure, and which one is rechecked at commit. Keep optional presentation conditions separate from hard command preconditions. An unavailable companion callback should not disable the main quest exit; an unknown remembered name should not invalidate an unrelated resource transfer. The authoring fixture should prove that one absent optional fact cannot accidentally close the whole graph.

### Gate package closeout

For every promoted scene, retain a small gate inventory with predicate purpose, source owner, disclosure tier, false/unknown explanation, commit-time revalidation rule, and a fixture that makes the route reachable. Review it whenever predicate meaning changes or an owner API is replaced. Remove conditions that neither alter player understanding nor protect a valid operation. A complete package leaves expected actions discoverable, explains meaningful blockers, preserves neutral behavior for unavailable optional facts, and keeps durable outcomes with their current owners.

## Continuation pass 10 — memory provenance, character perspective, and context lifetime

### Distinguish world facts from what a speaker believes

Dialogue often needs to represent imperfect knowledge. Keep the canonical event, the source's account of it, the character's interpretation, and the player's current understanding separate. A character can remember hearing that a gate opened without the gate owner confirming that it opened. A player can discover a torn instruction without knowing who removed it. A quest can accept the physical inspection while leaving intent unresolved. These are different facts, and branch logic must not collapse them into a single “knows” boolean.

| Narrative evidence class | What it permits dialogue to say | What it does not prove on its own |
|---|---|---|
| Direct observation | “I saw the lights go out before dawn.” | Who caused the outage or whether it was intentional. |
| Attributed report | “The night crew told me the west door was open.” | That the speaker personally verified the door. |
| Physical trace | “The latch is bent inward.” | Who bent it or when. |
| Inference | “Someone may have wanted the room found.” | The motive or identity of that person. |
| Accepted player commitment | “You said you would return with an answer.” | That the answer was found or that the task succeeded. |
| Corroborated conclusion | “Both records show the same departure time.” | Any broader detail outside the corroborated claim. |
| Contradicted recollection | “I was wrong about which road they used.” | That the speaker deliberately lied. |

Store or derive these distinctions only through existing knowledge, quest, relationship, or NPC-memory owners. This table is an authoring vocabulary, not a proposal to add seven global memory kinds. If a current owner records only the event and source ID, dialogue can keep the finer interpretation inside a transient node or authored line. Do not invent a persistent confidence score when no owner can capture, restore, and expose it consistently.

### Memory provenance card

Every remembered-action callback should answer four questions: who remembers; what specific event can they recall; what source or owner proves that event; and what may the line safely conclude? Add the expected scene context and neutral fallback. A memory card might say “speaker was present for the accepted repair request; task owner proves the request, but not whether it was fulfilled.” It should not say “speaker remembers the player as trustworthy” unless the relationship owner exposes that fact and defines its meaning.

| Field in the authoring note | Purpose |
|---|---|
| Event reference | Stable fact being recalled; never localized prose as identity. |
| Source owner | Current authority that confirms the event. |
| Remembering actor | Character whose voice reflects the event. |
| Knowledge boundary | Directly witnessed, learned from another, or inferred. |
| Lifetime | Current scene, active quest, campaign fact, or owner-defined memory. |
| Conflict rule | What to say if another current fact contradicts the recollection. |
| Missing-data route | Neutral line for old save, absent optional package, or unavailable actor. |
| Consequence rule | Explicitly state if the callback is cosmetic/informational only. |

The transcript must attribute hearsay to its source where useful: “I was told the road was clear,” not “the road was clear.” If the source is unknown, preserve that uncertainty. If the player confronts a speaker with contrary evidence, author a response that can acknowledge correction without making the whole memory system rewrite campaign truth. Contradiction may change the current conversation; durable trust or faction consequences still require their existing owner route.

### Memory lifetime and supersession

Avoid arbitrary time-based forgetting as the default dialogue behavior. A person may revise an interpretation after new evidence; the old event remains historically true. If a time/event owner makes a rumor stale, its current availability may change while the fact that it was once reported remains useful history. If the game has a supported memory-decay mechanic, reference that owner and its documented save/restore semantics. Do not add a dialogue timer or quietly erase a player commitment after enough days.

| Change | Canonical event history | Dialogue presentation |
|---|---|---|
| New evidence corroborates an earlier report. | Preserve the original report and add the new accepted fact through its owner. | Speaker can say what is now confirmed and cite the changed evidence. |
| New evidence disproves an interpretation. | Keep the observation; mark/update only the derived conclusion if its owner supports that. | Speaker can correct their reading without claiming the observation never happened. |
| Quest route expires. | Quest owner records the expiry or alternate route. | Callback states the actual lost opportunity and any remaining recovery. |
| Character unavailable. | Memory remains with its owner, if durable. | Use another source or omit the callback; do not put words in the absent actor's mouth. |
| Optional memory package missing. | No new fact is fabricated. | Select the baseline line, not an accusation or a credit. |
| Player revisits repeatedly. | No new event is implied by visit count alone. | Vary pacing or greeting only from supported visit/memory facts. |

Use supersession for beliefs or interpretations only when a current owner models that concept. Otherwise, author each line as a situated response to the present fact set. Keep historical transcripts immutable if the product retains them; current dialogue can offer a correction, but should not rewrite what was previously said.

### Relationship and reputation gates are different questions

Relationship context is personal and local to an actor/companion. Faction reputation and access are group-level facts. Skill/capability is a player or actor capability. Knowledge is evidence available to the speaker or player. A conversation may combine them, but each predicate must have an owner and a separate reason for use.

| Context source | Appropriate dialogue use | Unsafe shortcut |
|---|---|---|
| Relationship owner | Select familiarity, willingness to confide, apology, or supported companion option. | Treat “not close” as hostile, or grant a durable favor from a warm line. |
| Faction owner | Show a current contact, permission, negotiation term, or refusal. | Treat a friendly character as faction authorization. |
| Skill owner | Offer precise analysis, crafting insight, or a safer explanation. | Make essential objective instructions invisible without a baseline path. |
| Knowledge/discovery owner | Reveal information the player actually learned. | Assume the speaker knows everything the player learned. |
| Quest owner | Reflect accepted, blocked, failed, or resolved task state. | Use sympathetic dialogue to change a terminal result. |

If the speaker can know something the player has not discovered, classify the disclosure: does the dialogue reveal it, hint at it, or keep it private? Do not expose the information through response labels, locked option names, or map text before the intended reveal. A gate may hide a topic, show a locked label, or substitute a neutral answer; choose one pattern consistently for the scene.

### Context lifetime in a loaded scene

Build one immutable view of source facts for an individual render or use a documented owner refresh signal. A single graph traversal should not mix values from before and after a relevant state transition. If a fact changes while a conversation is open, invalidate the affected response set at a stable boundary and revalidate all command-bearing actions at submission. Avoid polling every owner on every focus movement.

Keep different refresh moments explicit: panel opened, response set requested, node rendered, command submitted, command result received, panel reopened after restore. Optional flavor can use the opening snapshot for consistency; a consequential command must use current owner preconditions. If a gate result becomes unknown during refresh, preserve a safe exit and neutral copy rather than reusing stale success.

### Revisit scripts that show memory without inventing state

Write a set of short revisit cases for one speaker and one event. First visit with no history uses an ordinary introduction. After a player accepted a task, the speaker can acknowledge the commitment but not the outcome. After the owner confirms completion, the speaker may thank or question the player according to their approved voice. After the task fails or expires, they name the supported result and offer only a valid next step. If the owner data is absent or contradictory, the speaker asks for clarification instead of assigning blame.

These lines can make a world feel attentive with little branch growth when they differ at one meaningful state boundary. Do not add one line per arbitrary visit count. Add a revisit variant when the underlying state changes, a new fact is learned, or the relationship owner supports a meaningful shift. The authoring card lists the condition and source beside the text so revisions cannot turn a current callback into a permanent statement.

### Memory-condition release fixture

Use a bounded fixture with two sources that disagree about one event, one later corroborating observation, a player commitment that is accepted but incomplete, and an optional character memory absent from an older save. Verify that the UI distinguishes observation, report, and inference; that no unavailable optional fact is interpreted as “the player never did this”; and that later evidence updates only the supported conclusion. Include one repeated visit and one panel reopen to prove rendering does not itself write memory.

The closeout artifact lists visible response IDs and their provenance, the exact owner query/result for durable facts, the neutral fallback, and any omitted speculative field. If the fixture requires a new persistent owner or save field, stop at architecture review. The correct next step is a separately approved decision with a migration and restore contract, not a convenient dialogue-local counter.

### Composite condition semantics for authors

Use a small, typed condition vocabulary whose meaning is readable in a review table. Each predicate identifies an owner fact, operator, operand type, and result domain. Groups should use explicit all-of/any-of/none-of semantics; avoid clever negations whose behavior on missing data is unclear. The evaluator can remain a read-only query over current facts. This plan does not require a new scripting language or arbitrary expression parser.

| Condition form | Intended meaning | Unknown behavior to author |
|---|---|---|
| All-of A and B | Both independent preconditions must be true. | Identify whether either unknown yields a neutral fallback or waits for that owner. |
| Any-of A or B | Either supported route makes the response available. | A known true route is enough; otherwise distinguish false from unresolved sources. |
| None-of A and B | Neither disqualifying fact is present. | Missing facts cannot prove safety; default to an explicit conservative response. |
| Exact value match | Current owner reports a named state/value. | Invalid ID is a content error; absent value remains unknown. |
| Range check | Numeric owner fact lies within an authored allowed range. | Use owner validation and named bounds; do not compare localized/display text. |
| Event present | Stable accepted event exists. | A missing old-save event is unknown unless migration proves absence. |

Do not let `not true` silently include unknown. For an access gate, “access is not denied” is not the same as “access is granted.” For a memory callback, “no event found” may mean no event occurred or the old save did not capture it. For a critical route, unknown should either defer safely or use the baseline route, never grant the route accidentally.

### Predicate composition example

Suppose a response can show a detailed route analysis if the player has a map clue and either a relevant skill or corroborating testimony. Write the logic as: known clue AND (skill available OR witness evidence accepted). The baseline location question remains visible without the detailed-analysis option. If the clue source is unavailable, the detail stays neutral; if the skill fact is missing but testimony is accepted, the alternate proof is sufficient. If both optional sources are unknown, no detail branch appears, but the player can ask what is already known or leave.

The authoring fixture should include one case for each useful truth combination rather than enumerating every possible save-state permutation. Verify that the `any-of` group accepts either supported source, the outer `all-of` still requires the clue, and a missing source does not become proof. If the content can no longer explain why the branch appears, simplify the predicate group or split the scene into clearer dialogue nodes.

### Predicate identity and change control

Stable predicate IDs describe a meaning, not a code location or a string of current implementation details. Do not reuse one ID after changing “has heard a rumor” into “has confirmed a route,” even if both currently point to the same UI line. Record owner, meaning, source version, and consumers in the content review. Rename/migrate through the current authoring/catalog process so old content cannot silently receive a new interpretation.

When an owner schema changes, list affected predicates and classify them as still valid, remapped, unsupported, or removed. An unsupported optional predicate can use the declared neutral branch. An unsupported critical predicate blocks the dependent action and requires a baseline or migration decision. Keep diagnostic output limited to stable IDs and reason codes; never export private campaign memory or full save state as an authoring convenience.

### Gate-performance and graph-scale boundary

Evaluate only predicates referenced by the current scene and response set. Do not scan every quest, actor, faction, or remembered event when a node is opened. Reuse a snapshot for consistent presentation while it remains valid, and revalidate command preconditions at submission. If profiling later shows gate evaluation to be costly, capture scene size, predicate count, owner query time, refresh frequency, and allocation before proposing a cache.

Any index or memoization remains derived and invalidated through current owner revisions/events. The cache cannot become a private relationship, knowledge, or quest store. If the content graph grows to a scale where authors need condition tooling, first surface unreachable branches, invalid references, cycles, and truth-table fixtures; avoid making authors dependent on arbitrary executable expressions that cannot be localized, validated, or safely migrated.

## Continuation pass 11 — context evaluation contract, gate catalog, and dialogue fixture suite

### A typed condition reference

A content condition should be data that a current owner can evaluate, not an executable string embedded in dialogue. Give every predicate a stable identity and a reviewable source/meaning. The exact serialized shape must follow the current data authority; the semantic fields below are a checklist for the authoring contract.

| Predicate attribute | Required meaning | Rejection/fallback behavior |
|---|---|---|
| Predicate identity | Stable key for the condition's meaning. | Duplicate ID or changed meaning blocks content validation. |
| Source owner | Current quest, actor/relationship, faction, skill, location/knowledge, campaign, or time/event authority. | Unknown owner/reference is invalid content, not ordinary false. |
| Fact reference | Canonical fact/event/resource key read from that owner. | Missing optional fact remains unknown; missing required definition is an error. |
| Operator | Typed equality, membership, range, presence, or supported logical group. | Unsupported operator is rejected; no string reflection or script callback. |
| Operand | Typed stable ID, boolean, enum, or bounded numeric value. | Wrong type/range is rejected at content load/review. |
| Purpose | Availability, disclosure, cosmetic variant, command precondition, or result selection. | Purpose drives copy and revalidation; cosmetic predicates cannot block a required command. |
| Truth handling | Behavior for true, false, unknown, unavailable, and stale values. | Every state has an explicit safe route. |
| Player presentation | Text/disabled/hidden/fallback policy where useful. | No raw predicate IDs or leaked protected facts. |
| Evaluation boundary | When a stable owner snapshot is read and when the predicate is refreshed. | No nondeterministic reread mid-graph without an authored refresh boundary. |

Separate build-time validation from runtime evaluation. Static validation checks owner names, fact IDs, types, ranges, condition groups, and graph reachability. Runtime evaluation reads current facts and returns a typed result. Do not use runtime absence to silently waive a static required reference. A missing optional owner can use a declared neutral path; a malformed ID should fail the content gate and be reported to the author.

### Context categories and safe line behavior

The context model can support many kinds of dialogue while keeping each fact with its current authority. The table makes the requested categories concrete without claiming that every one is fully wired in the game today.

| Context category | Source question | Safe use | Unknown/unavailable behavior |
|---|---|---|---|
| Quest lifecycle | Is the task offered, active, blocked, failed, completed, expired, abandoned, or resolved under the current owner model? | Select the proper opening, objective detail, recovery, or result node. | Use a neutral scene fallback and never recast failure as completion. |
| Objective evidence | Which proof has the quest owner accepted? | Acknowledge progress and expose the next valid objective. | “We still need a source” rather than claiming the event did not happen. |
| Relationship | What personal relationship fact does this actor owner expose? | Change willingness, familiarity, or supported companion options. | Ordinary respectful greeting; no assumed trust/hostility. |
| Reputation | What group standing/access does faction authority expose? | Show eligible terms or current restriction. | Do not infer standing from one friendly character. |
| Faction identity | Is this speaker/contact tied to a canonical group in this scene? | Select faction-specific offer/copy after availability passes. | Fall back to generic contact or explain no current liaison. |
| Knowledge/evidence | What has the player learned, and from what source? | Reveal detail or permit a corroboration response. | Treat missing old-save fact as unknown unless migration proves absence. |
| Location | Is this the canonical scene site and is interaction currently possible? | Select location-local questions and inspect actions. | Offer the base conversation or a route explanation. |
| Repeat visit | Does an existing owner report a prior visit or interaction? | Change greeting or show an aftermath callback. | Do not infer a visit from reopening the panel. |
| Failed quest | Did the quest owner record an actual failed outcome? | Apology, alternate route, recovery task, or closure. | No failure copy when the quest is merely blocked or data is absent. |
| Player action memory | Did a canonical event record a promise, delivery, refusal, or discovery? | Attribute a callback to the event. | Neutral phrasing; no accusation or credit. |
| Skill/capability | Can the actor/player perform or interpret the relevant action? | Show a deeper explanation, diagnostic, or safe option. | Keep baseline instructions and core branch understandable. |
| Hidden emotional state | Does an existing character owner expose an approved state? | Tone/animation or supported availability variant. | Neutral line; never synthesize a hidden mood score. |
| Time/event window | Is a supported deadline or temporary event active? | Explain urgency or temporary access. | Wait for owner readiness; do not invent a dialogue clock. |
| Actor availability | Is the canonical speaker present and able to respond? | Offer their graph or alternate contact route. | Do not simulate a response from an absent actor. |
| Campaign consequence | Has the campaign owner accepted a relevant milestone/input? | Select a callback or late-game question. | Use baseline wording; the absence of an optional branch is not a negative ending choice. |

A content author can choose a smaller set for any one scene. More categories do not automatically make dialogue richer. A predicate is useful only when it improves the information, player affordance, voice, or valid command. Avoid two predicates that differ only in internal implementation but select the same lines and same behavior.

### Gate stages: availability, disclosure, and commit

Evaluate gates according to what they control. **Availability** decides whether an action can be selected at all. **Disclosure** decides how much information the player or speaker should reveal. **Variant** selects an optional line while leaving the action unchanged. **Commit** rechecks the owner preconditions for a mutation. **Result** selects copy after a typed command response. A single predicate may inform more than one stage, but the author must state each use.

| Stage | When evaluated | Author question | Required response when facts changed |
|---|---|---|---|
| Scene eligibility | Before opening or when listing speakers. | Is this scene currently available at this location? | Hide/disable safely or route to another contact. |
| Node availability | When current response set is built. | Which player actions make sense now? | Rebuild at a stable boundary and announce meaningful change. |
| Disclosure | Before rendering sensitive detail. | Has this speaker/player earned or revealed the information? | Use a nonrevealing fallback. |
| Cosmetic/voice variant | At render from a consistent snapshot. | Which wording fits current context? | Select deterministically from the same view; no mutation. |
| Commit validation | At command submission. | Do current owner facts still permit the requested operation? | Reject stale request and show refreshed options/reason. |
| Result routing | After owner response. | What actually happened? | Match accepted/rejected/deferred/unknown/duplicate copy to result. |

Do not use a presentation gate as a commit guarantee. A response can be visible when opened and become stale before selection; the owner still decides. Do not re-evaluate a cosmetic relationship phrase after every character is typed, because the scene could change voice mid-sentence. Keep one rendering snapshot and one explicit commit boundary.

### A worked gate group without an all-or-nothing scene

Consider a player talking to a route contact in a public shelter. The basic “ask about the route” response is always available. A detail about a previously found map fragment appears when the knowledge owner reports that clue. A question about a faction escort appears only if the faction owner confirms an active contact and the route contact is present. A skill-based inspection option appears if a relevant ability is present, but its absence cannot prevent the player from asking for directions. A returning greeting can vary through a supported memory/relationship fact.

The gate group should not make the entire scene disappear when faction access is unknown. It should not expose the escort term through a locked response label if the faction relationship is secret. If the faction owner is unavailable because an optional package is disabled, use baseline route copy; do not tell the player they were rejected by that faction. If the map fragment is known but the route has changed, the known-detail branch remains a historical question while the command confirmation refreshes current route readiness.

The authoring card identifies the exact response IDs for each case, the source facts, and the fallback line. The content graph remains navigable with no skill, no faction, no memory, and no discovery facts. A baseline player can leave, ask ordinary questions, and continue an unrelated task. Optional branches add context and choices rather than withholding the only actionable instruction.

### Combinations and truth-table reduction

Do not write one fixture for every Cartesian combination of all context facts. Group dimensions by the owner contract and test each meaningful boundary: one true path, one false path, one unknown path, one optional-owner-absent path, one conflict/stale path, and one command result. For compound groups, include cases that prove the intended `all-of`/`any-of` relation and a case where an unrelated optional condition is missing.

For example, a response that needs a known route clue and either a skilled inspection or corroborating witness has an outer prerequisite and an inner alternative. Test: clue+skill; clue+witness; clue but neither; no clue but both optional sources; and unknown witness data from an old save. Only the first two satisfy the full logic. The no-clue case does not become valid because both optional sources are present. The old-save case uses the declared unknown behavior rather than quietly counting as “never spoke to witness.”

Truth tables should describe visible actions, not just boolean evaluator output. Record the response set, disabled reasons, safe exits, selected text key, command eligibility, and owner result. That makes an impossible branch, hidden essential option, or misleading result copy visible to content and UX reviewers.

### Dialogue graph structures and gate placement

| Graph structure | Appropriate gate location | Maintainability rule |
|---|---|---|
| Linear scene | Scene/node entry or end confirmation. | Do not add branches unless information or consequences differ. |
| Hub-and-spoke | Each question availability and spoke return. | Keep hub exit available; isolate optional condition failures. |
| Short branches that reconverge | Response variant and summary knowledge. | Preserve learned fact as current context without duplicating the whole graph. |
| Relationship dialogue | Opening/variant and any explicit owner command. | Keep personal context separate from reputation/access. |
| Knowledge-gated dialogue | Detail response, with baseline explanation. | Show source or hint without leaking hidden text. |
| Faction-specific dialogue | Contact eligibility and commit confirmation. | Faction owner controls access and result, not graph visitation. |
| Location-specific dialogue | Scene availability and local action. | Canonical location ID and current route/arrival are distinct facts. |
| Repeated-visit dialogue | Current state callback. | Visit itself is not a quest/relation event. |
| Failed-quest dialogue | Owner-backed failure result. | Offer recovery only when it is a real eligible task. |
| Skill dialogue | Optional analysis or alternate method. | Core path retains sufficient instructions and safe exits. |

If a dialogue structure requires so many conditions that the graph cannot be reviewed as a transcript, split it into scenes by purpose or query a concise derived owner view. Do not duplicate the full condition matrix on every node. A shared authoring macro is acceptable only if the resulting content still has stable IDs, visible predicate references, and a clear diagnostic path.

### Invalidation, refresh, and deterministic replay

A scene reads a coherent context view at its supported boundary. If an owner publishes a fact change, refresh affected availability at a stable transition: after returning from another screen, after an accepted command, or when reopening the scene. Do not use wall-clock time or dictionary order to pick among equally eligible variants. Use stable author order or the existing deterministic stream where variety is intentional and replay-safe.

Persist no snapshot cache unless an existing owner already supports the saved fact. A cached response list is invalid after the quest finishes, location closes, actor departs, faction access changes, optional bundle is disabled, or the scene is reopened from a save. The commit command must always be revalidated. When an owner event arrives while the panel is active, either refresh the complete relevant context or show a stale-state notice and return to an explicit safe selection point.

Same seed plus same owner snapshot should produce the same generated line variant and same response availability. Changing only cosmetic text must not alter command eligibility. Changing one canonical owner fact should update only the response nodes that declare that dependency. A paired replay fixture helps detect hidden reads from global state or a random draw that occurs on each panel render.

### Gate integration dossier

For one dialogue package, submit a source-owner inventory, predicate list with meanings and types, graph response matrix, disclosure/fallback copy, static validation output, fixture truth table, UI focus behavior, stale-state trace, and restore/reopen behavior. List unsupported categories honestly, including any unavailable memories or relationship facts. Do not mark a category “wired” just because prose for that category exists.

The smallest runtime slice should verify one quest gate, one knowledge gate, one relationship or faction variant from a real owner, one neutral unknown, and one command whose precondition changes between display and submission. Later support for hidden emotions, broad memory callbacks, analytics of unseen branches, or large authoring editors is optional and needs an explicit owner/API review. No dialogue-specific condition engine or save store is proposed here.

### Relationship facts and promises are not interchangeable

A relationship value, an accepted promise, a remembered visit, and a current willingness to talk answer different questions. Use the narrowest current fact that supports the response. “We spoke before” can select a greeting; “you accepted the repair task” can acknowledge a commitment; “the task owner recorded completion” can support thanks; a relationship owner may independently expose familiarity or willingness. Avoid using a broad trust threshold as a shortcut for all four.

| Player history | Safe acknowledgement | Unsafe inference |
|---|---|---|
| Opened a conversation | Normal greeting or scene context. | “You came through for me.” |
| Asked for information | Refer to the question if its event is retained. | Assume the player accepted the task. |
| Accepted an offer | Acknowledge the promise/commitment. | Claim the promised result has happened. |
| Completed an owner-confirmed objective | Describe that accepted outcome. | Claim every related character/faction changed their opinion. |
| Declined or deferred | Respect the player's choice and preserve supported future options. | Treat hesitation as betrayal or hostility. |
| Failed/expired | Name the actual failure or deadline result if appropriate. | Infer bad intent or a permanent relationship loss. |

If the relationship owner does not distinguish the needed facts, keep the line contextual and local. Do not add a “remembered promise” save section solely for one callback. If a quest owner already records acceptance, a dialogue node can query that fact and render the line without storing it again.

### Multi-speaker disagreement and evidence order

When two characters report conflicting accounts, author who each source is and what they observed. Do not resolve truth through a character's reputation score or the order in which their conversation is opened. A respected witness can be mistaken; a nervous witness can be accurate. If the game has an existing skill/relationship context that changes how the player interprets testimony, it can offer extra questions, but the underlying source facts stay attributed.

The conversation can disclose disagreement in stages: first speaker gives an attributed account; second speaker provides a conflicting account; the player can ask about what each directly observed; a location/record interaction can corroborate one detail; then a quest/evidence owner accepts what the evidence supports. The scene may still end with uncertain motive or authorship. This avoids turning the dialogue gate system into a hidden truth oracle.

| Conflict case | Gate/content rule | Result copy |
|---|---|---|
| Both sources directly witnessed different moments | Show both moments and their time/scope; they may not contradict. | “You saw the door after the bell; I saw it before.” |
| One source repeats a rumor | Keep it attributed and optional. | “That is what I heard, not what I saw.” |
| Record confirms one detail only | Reveal the confirmed field and leave other claims open. | “The hour is recorded. The name is not.” |
| Skill identifies a physical inconsistency | Offer a more precise inspection/interpretation response. | State what was measured; do not name an unknown actor. |
| One speaker is unavailable | Use remaining evidence path or neutral wait. | Do not present absence as proof against their account. |

### Disclosure tiers across an entire scene

Sensitive information can leak through more than the dialogue line. Review node title, response label, locked-option text, character portrait, journal hint, map pin, accessibility announcement, and transcript/export. A “hidden” choice whose label names a secret faction is not hidden. A map marker can leak an exact location even if the scene only says “somewhere beyond the ridge.”

| Disclosure tier | Typical information | Design rule |
|---|---|---|
| Public | General town procedure or already posted information. | May appear in baseline lines and public map summaries. |
| Player learned | A clue the player found through an owner-backed action. | Can be referenced after discovery; keep its source clear. |
| Speaker-private | Personal experience the actor may choose to reveal. | Gate the disclosure to the actor/context; avoid surfacing it in menu labels. |
| Protected/unknown | Secret contact, hidden location, unresolved motive, or unreleased package detail. | Use a neutral branch until a valid reveal event occurs. |

Disclose in an intentional order: hint, attributed clue, confirmable fact, then any consequential interpretation. A discovery gate should not reveal content through its disabled-response explanation. If a response must remain hidden for narrative reasons, ensure the baseline node still offers a useful question and a safe exit.

### Current context conflict between owners

Two owners can publish facts that appear to disagree because they describe different scopes or update at different times. A route owner may report a road open to foot traffic while an expedition owner reports that a particular party cannot start. A quest may be active while its optional character contact is unavailable. A relationship fact may be old while an NPC-memory event is current. Dialogue should report the narrower fact and route the action to the owner that decides it.

Do not invent global precedence such as “quest always wins” or “faction always wins.” The response's declared purpose determines which owner is authoritative. For scene availability, use the host/actor location seam. For map knowledge, use discovery/map authority. For task phase, use the quest owner. For a command's final eligibility, use that command's owner. When the view cannot explain a conflict clearly, show a neutral status and allow a safe exit rather than making a stronger claim.

### Condition/copy consistency through localization

Each condition state maps to a text key or an intentional hidden/fallback rule. Avoid composing a long sentence from a fragment selected by one gate and a second fragment selected by another when the final grammar, speaker, or disclosure level can become invalid. Prefer complete authored lines for important branches. If a locale needs a different word order, the localized line should remain a complete variant under the same owner/context meaning.

Context notes identify the subject of pronouns, the source of a claim, whether the line is a question or confirmation, and what changes in the scene. A one-word response such as “Fine.” can mean acceptance, resignation, or anger. The author should attach an intent note and state whether the choice is cosmetic or command-bearing. Translators and voice performers need that distinction to avoid turning a neutral acknowledgement into a threat or binding promise.

### Gate review at catalog scale

For a large dialogue pack, cluster predicates by owner and fact meaning, then review the content rows that use each cluster. A stable “quest active” predicate may appear in many responses, but each graph still needs reachability and fallback review. A faction access predicate should not be recreated with different spellings and inconsistent unknown semantics in every scene. A shared content reference can standardize wording and validation while keeping scene-specific explanatory text local.

The batch report should include predicate count by owner, missing/unsupported references, true/false/unknown branch coverage, unreachable response count, disabled-response reason coverage, text-key completeness, stale-state handling, and command revalidation cases. Distinguish reusable predicate coverage from scene-specific transcript coverage. A predicate that passes one fixture does not prove that every scene uses it for the same purpose.

When changing a common predicate, list every consumer graph and inspect the affected transcripts. A change from “has access” to “may request access” can make confirmation options appear too early even when the predicate ID and data type remain unchanged. Treat semantic change as a content migration/revision event, not a routine refactor hidden behind the old name.
## Continuation pass 12 — skill gate fairness, alternate competencies, and no-soft-lock policy

This pass deepens gate design around a specific player-experience risk: a dialogue option can become a silent requirement even when the quest appears to offer several approaches. A skill- or knowledge-gated line should add a meaningful way to understand or act on a situation. It should not hide the only route to core progression, equate a high stat with moral correctness, or require the player to reload and rebuild their character to continue.

This is an authoring policy for facts exposed by current skill, knowledge, relationship, reputation, faction, quest, or location owners. It does not add a dialogue-owned skill system, a new memory score, a hidden emotional meter, or a second gate evaluator. If the current game does not expose the fact an option needs, the option remains a design proposal until the right owner contract is verified.

### Gate purpose and player promise

Before a gate is written, the author names its purpose. Common purposes include:
- **Expression:** the player can phrase an idea in a way that reflects a supported skill or background.
- **Interpretation:** the player notices a detail or connects two facts the player has already encountered.
- **Shortcut:** the player can avoid an extra step because they understand a relevant process.
- **Risk reduction:** the player can choose a safer approach or recognize a warning, but no guaranteed success is implied.
- **Access:** the player can request a restricted interaction when a current authority has granted the required standing or access.
- **Character depth:** the player can respond with a personal or emotional register consistent with supported context.
- **Optional mastery:** the player can discover a useful secondary detail, side route, or extra dialogue, while the main objective remains completable.

A gate must not exist only to make a response feel “special.” If the line does not change the player's understanding, action, or meaningful characterization, prefer an ungated line. If it is gated, the author records whether the lock is visible, whether its reason is disclosed, and whether a different route can communicate the same critical fact.

The player-facing promise is about a path, not a guaranteed outcome. A technical skill may let the player identify a loose casing. It does not guarantee that the owner accepts the player's proposed repair, that a resource is available, or that the machine can safely operate. A negotiation background may produce a clearer question. It does not make every faction agree. A survival trait may reveal a route risk, but the expedition owner remains responsible for route selection and result.

### Gate classes and alternatives

| Gate class | Can enrich | Must not become | Alternative route candidate |
|---|---|---|---|
| Technical/mechanical understanding | Inspect a component, distinguish a mark from an instrument result, ask a precise repair question. | A mandatory technical check for every objective at a machine location. | Ask the mechanic, locate a manual, compare a second site, or accept an unverified conclusion. |
| Social/negotiation context | Ask who must consent, restate a proposal without overclaiming, identify a condition for a meeting. | A charisma score that overrides an NPC or faction owner's decision. | Ask a neutral question, bring a supported witness/evidence, or decline to negotiate. |
| Survival/environmental knowledge | Notice path exposure, interpret shelter preparation, select a safer staging route. | An invisible hazard check that punishes an unbuilt character with unrecoverable loss. | Read a public warning, request local guidance, delay to a safer time, or choose another route. |
| Medical/care knowledge | Recognize that fatigue or injury needs an owner-supported response, ask for help respectfully. | A diagnosis or treatment result that bypasses current health/needs owners. | Seek an authorized caregiver or choose a non-clinical support route. |
| Faction/relationship access | Ask for a private conversation or a narrower offer. | A universal faction pass or a numeric affection result authored in dialogue. | Public meeting, earned introduction, objective completion, or return after a supported prerequisite. |
| Knowledge/discovery | Interpret a discovered sign, map mark, report source, or previously shared fact. | Access to the only solution when the player may never receive the clue. | A discoverable environmental route, another owner-backed source, or an explicit uncertainty ending. |

“Alternative route” does not mean every player receives identical information at the same time. It means the critical route is not silently locked behind an optional build or optional content. A player with a relevant skill can be rewarded with sharper language, earlier warning, or fewer steps, while another player receives a fair route through observation, conversation, preparation, or an honest unresolved result.

### A three-level gate quality model

**Level 1 — flavor variation.** A supported context selects a different line, gesture, or question. The scene reconverges and the player's available actions stay the same. This is the lowest-cost use of a gate and often the best way to show character voice.

**Level 2 — optional understanding or bonus route.** A gate reveals a side clue, additional motive, optional location hint, or reduced-risk approach. The player can continue without it through a longer or less certain route. The bonus is stated in the content card so that localization, map, quest, and UX review can account for it.

**Level 3 — required access or command precondition.** A gate controls a consequential action such as entering a restricted site or submitting an offer. The owner—not the presentation predicate—revalidates the requirement. The task has a declared route for players without access, and the player can see the reason for unavailability when disclosure is appropriate. This level receives cross-owner and save/restore review.

A route with an ending-level or campaign consequence is not a “Level 4 skill gate.” It is a campaign decision with a much broader review burden and an explicit owner. Character skill may change what the player understands about that choice; it does not choose the ending on the player's behalf.

### Tidemark scene: the useful gate is not the only gate

At Switchback Sluice, the player may have a supported mechanical skill fact, may have discovered Sootstep Marker, may have heard Sena's account, or may arrive with none of these. The gate design should permit the following routes:

| Context available | Dialogue contribution | What remains open |
|---|---|---|
| Mechanical knowledge supported | “The casing has shifted at the hinge. The scale may still be readable, but the wheel should not be used as a test.” | Ask Sena to inspect; submit an observation if the quest owner accepts it; delay the test. |
| Marker discovered | The player can ask why two cuts face away from the sluice, connecting the site to the older survey line. | The clue can deepen the investigation; it does not complete the gate inspection. |
| Relationship/familiarity fact supported | Sena explains why she separates a scratch from a reading before the player asks. | The conversation remains informational; no command or transfer is implied. |
| No special context | The player can ask, “Which part can we confirm from here?” Sena explains the public inspection boundary. | The player can continue by requesting inspection or accept uncertainty. |
| Relevant context unknown or unavailable | Use a neutral line: “The casing shows a mark; the reading is not confirmed.” | No route is treated as false, and the scene can close safely. |
| Expansion faction absent | Edda's coalition procedure is omitted; the local inspection route remains. | Core quest conclusion remains possible without the Assembly package. |

The technical line gives useful interpretation but does not assert a water-quality fact. The marker line connects authored discovery but does not create hidden evidence. Relationship context changes what Sena volunteers, not her mechanical state. The neutral line is a complete, localizable fallback. The graph does not present a blank player response if any source owner is unknown.

At Mothglass Nursery, a care-oriented context can add a humane question about who has been working without a break. The main story does not require this insight to persuade Mina or gain the lamp. The task's availability and any labor assignment still come from the current quest/character interaction contract. A player who declines the task remains able to finish the main inspection.

### Hidden gates and respectful disclosure

A hidden option is reasonable when discovery itself is part of the fiction: the player notices a symbol, recalls a clue, or sees a familiar tool. The hidden state should not be used to conceal an essential cost, a deadline, or an irreversible commitment. If a response is unavailable due to a public prerequisite, the UI may state the prerequisite directly. If revealing the condition would spoil a secret or expose a character's private memory, provide a safe generic explanation or let the player discover the reason through a fair route.

Do not show a locked response whose label reveals private facts such as a character's trauma, medical history, or hidden faction plan before the player has earned that context. Accessibility labels must follow the same disclosure rule as the visible line. A screen reader must not announce a hidden option that the visual UI intentionally withholds. Conversely, if a visible disabled response says “requires a tool,” the accessible state should convey that reason in text, not only by color or tooltip.

Emotion is written through current authored behavior and, where the game has it, supported character or relationship facts. Do not encode “trust” as an unreviewed integer threshold simply to gate a line. A character can be tired, guarded, or relieved in prose without the game storing a new hidden emotion value. If an existing owner exposes an appropriate state, the author must document who can know it and how it changes.

### Branch value audit

Every gated response should have a branch-value note answering:
1. What new understanding or action does this option give?
2. Does it change objective eligibility, player cost, access, or only the wording?
3. If it changes eligibility, which owner validates the fact again at action time?
4. What can the player do if they never have this fact?
5. Does the option appear early enough for its information to matter?
6. Could it be mistaken for a guaranteed result?
7. Does its absence reveal that the player is missing a character build, or does the scene supply a clear in-world alternative?
8. Does the route remain valid with optional content disabled or an older save?
9. Can the player understand the effect through text, focus, and feedback?
10. Is the result worth the extra authored, localized, and tested branch?

If the answer to the first question is “it gives a higher number,” inspect the mechanic owner to determine whether numbers are visible or meaningful in the current game. A gate that simply rewards a stat check can feel arbitrary. If there is no meaningful difference, turn the line into flavor or remove it.

### Quest access and soft-lock prevention

A core quest route should have an availability audit across plausible player histories and build profiles. The objective is not to guarantee that every optional result is reachable; it is to guarantee that core progression never depends on an unannounced optional fact, a removed package, or a location that cannot appear.

For each required objective, list:
- the fact that opens it;
- where the player can earn that fact;
- the other sources that can supply equivalent proof;
- whether the fact persists under its current owner;
- the path if a prerequisite is absent, expired, lost, or package-gated;
- whether failure leaves the quest active, blocked, partially completed, failed, or resolved;
- the exact player-facing explanation and return route.

If a gate must control a required action, the content brief explains why no alternate action is available and how the player can earn the prerequisite. “The author intended a high-skill check” is not a player-facing reason. A timed required gate must disclose timing before acceptance and route to a fail-forward conclusion if missed. An alternate route can have a different cost, uncertainty, or story result, but cannot require content the player does not own or cannot reach.

Do not let a skill gate become a soft lock through branch chaining. For example, a mechanical gate might reveal that the upper instrument is useful, but if the player needs a separate faction gate to access it, the route can fail for two unrelated build/context reasons. Model the entire prerequisite path. Either make the chain optional, provide a public route, or ensure every prerequisite is earned through the core path.

### Fairness and branch-budget matrix

A large dialogue pack can accumulate gates until the aggregate interaction becomes exclusionary even though each individual gate seems harmless. Review gate density by quest chapter and character, including how many high-value facts require a particular build or relationship route.

| Review measure | How to interpret it | Corrective action when it fails |
|---|---|---|
| Core-objective reachability by build profile | Can a low, middle, or high relevant skill profile reach an owner-supported main objective? | Add an observation, preparation, help-seeking, or uncertainty route. |
| Unique critical facts behind optional gates | Does one gate hide information required to make the main choice understandable? | Surface the minimum fact publicly or make the gated insight optional depth. |
| Gate concentration on one character | Does one NPC repeatedly demand the same build or faction standing? | Diversify the source and give the character a clear reason to offer alternate help. |
| Consequence clarity | Can players distinguish “I may say this” from “the owner will accept it”? | Rewrite labels, show costs/preconditions, and revalidate at the owner. |
| Expansion dependency | Does a core route need a late-game scene or faction? | Split optional content from the base solution. |
| Revisit fairness | Does returning after a prerequisite change reveal the updated option? | Refresh from current owners and announce material new availability without forcing repetition. |
| Cognitive load | Are many predicates stacked on one response? | Split the choice, show a concise prerequisite, or simplify the branch. |
| Localization stability | Does a gate depend on a fragment or wordplay that cannot translate? | Use a complete authored variant under the same semantic condition. |

A cap is not required as a universal numeric rule. The purpose of the matrix is to detect qualitative pressure across a chapter. Content analytics, if currently available and approved, can help measure abandoned conversations or repeated visits, but those metrics do not decide whether a narrative route is fair. A new telemetry system should not be added by this plan.

### Test and review fixtures for gate fairness

When a future implementation is authorized, use named profiles to check branch access rather than relying on the writer's preferred playthrough. Profiles can be constructed in focused fixtures or an existing test harness, depending on current policy:
- low or absent mechanical knowledge, no marker clue;
- high mechanical knowledge, no faction access;
- discovery clue earned before meeting Sena;
- optional faction present but relationship context unknown;
- core-only package with no Assembly character;
- objective partially complete, scene reopened after restore;
- visible disabled action whose owner prerequisite has changed;
- response selected, then the context changes before command validation;
- player declines all side tasks and still reaches a truthful mainline result;
- one locale where a conditional line is longer and requires a different layout.

For each fixture, record which options appear, which are hidden or disabled, why, what the player is told, the owner result after selection, and the next allowed action. A screenshot alone cannot prove that the quest owner accepted the intended fact. A predicate unit test alone cannot prove that the player understands why the choice appeared. Use evidence proportional to the route: focused logic coverage, transcript review, UI focus/readability, and owner state observation where required.

Review false unknowns and stale truths explicitly. If a relationship adapter cannot supply a value, the graph may not assume “not close.” If an optional package is unloaded, the graph may not treat its faction fact as false and select a punitive branch. If a selected option was visible under old context, command-time rejection must produce a safe refresh rather than an unrelated consequence.

### Authoring card for a consequential gate

Attach the following brief to each Level 3 gate:
- **Gate intent:** what the player understands or can request.
- **Fact source:** current owner and verified readable fact.
- **Visibility:** hidden, visible, disabled, or explained, with a disclosure reason.
- **Availability timing:** when the gate is evaluated and refreshed.
- **Commit-time check:** the owner validation that prevents a stale view from submitting an invalid command.
- **Alternative path:** reachable route without this fact, and its honest result.
- **Information boundary:** what the line does and does not prove.
- **State lifetime:** ephemeral context versus owner-persisted fact.
- **Unknown/unavailable copy:** complete text for missing adapters or optional packages.
- **Accessibility note:** focus, label, keyboard/controller, and screen-reader behavior.
- **Localization note:** intent, speaker relationship, and complete-text requirements.
- **Verification evidence:** the smallest current target that proves predicate and owner behavior.

The plan's next implementation proposal should start with a single non-exclusive gate, such as the optional mechanical observation at the sluice. The first review proves that the player without the fact can still inspect, ask, or choose uncertainty. Only after that route works should an access-critical gate be proposed. If current fact providers cannot supply reliable supported context, the safe baseline is an ungated complete line rather than invented state.

### Three gate micro-rehearsals

A reviewer can test gate fairness with complete dialogue fragments, then compare the options and outcomes under different supported contexts.

**Mechanical interpretation available:**<br>
Player: “The hinge is carrying the mark, not the gauge face.”<br>
Sena: “That is what it looks like from here. I would still check the casing before I called it a reading.”<br>
The skill fact sharpens the question, but Sena and the objective owner still determine what is verified.

**No mechanical interpretation available:**<br>
Player: “Which mark should I trust?”<br>
Sena: “Neither by itself. One came from above; this casing moved afterward. We need a second look.”<br>
The player receives enough information to continue and can request another inspection or leave the result unresolved.

**Negotiation context available:**<br>
Player: “Would you publish the method before you agree on the result?”<br>
Edda: “Yes, if the report says the reading is incomplete. The method and the conclusion are separate.”<br>
This is a more precise conversation route, not authorization to publish. The command owner and Edda's current faction authority still validate any public action.

The gated and ungated lines reconverge on the same next-step question. The distinction is useful to players who invested in the relevant context but does not make that context a moral ranking. A gate review compares information value, player action, and owner acceptance separately. The graph should not award a better quest result simply for choosing the most technical line unless the quest contract explicitly supports a different proof outcome.

An evaluator fixture should capture the displayed option set, reason for any unavailable option, result after a selection, and next scene. The transcript review should also verify that no character claims certainty the corresponding condition did not establish. These examples may be translated as complete exchanges rather than sentence fragments; the intent notes travel with the line card.
## Continuation pass 13 — signal knowledge provenance, rumor gates, and correction dialogue

The Farline story makes a useful stress case for dialogue context: a single sound can be heard by one character, reported by another, written down by a courier, and later repeated by someone who never visited the location. The dialogue system must distinguish these relationships without turning every rumor into a persistent memory object. This pass defines content semantics and gate review for those cases using facts and event records that current owners already expose.

### A bounded evidence vocabulary for conversation

Use the following words consistently when a line describes a signal or route claim:

| Evidence phrase | Player-facing meaning | Condition source needed | What the phrase does not establish |
|---|---|---|---|
| **Heard** | The speaker or player encountered a sound/report. | A supported cue/report event or attributed authored observation. | Source identity, emission time, route status. |
| **Received** | A named person or site accepted a message or packet. | Current delivery/quest/interaction owner result. | That the recipient read it or acted on it. |
| **Inspected** | A person examined a named physical marker/site. | Current quest/location proof owner. | That the signal emitted from it, or the route remains safe. |
| **Reported** | A source made a claim, with attribution. | Authored/source reference or current accepted report fact. | Truth of the claim. |
| **Corroborated** | Two sufficiently distinct sources support one narrow statement. | Current evidence/quest owner contract defining distinct sources. | Universal certainty or faction adoption. |
| **Confirmed** | The responsible current owner accepted enough evidence for a defined claim. | A canonical owner result, not dialogue inference. | Claims outside that exact scope. |
| **Retracted** | An authorized source withdrew its earlier claim or message. | Existing message/quest/faction authority. | That the opposite claim is true. |
| **Unanswered** | A reply was expected but no supported response was received. | Current communications/quest result, if any. | Refusal, hostility, or operational failure. |
| **Unknown** | Current content/owners cannot determine the fact. | Explicit unknown/unavailable result. | False, unsafe, or not yet discovered. |

This vocabulary is authoring semantics, not a new persisted evidence-state enum. If the current systems cannot supply one distinction, use the narrower truthful phrase. For example, if the game knows only that a quest item was found, do not claim that a message was received.

### Speaker evidence cards

Every major statement in the Farline scenes has a source and scope. A speaker can be mistaken, limited, or uncertain; that is part of characterization. The graph should not quietly convert a character belief into global truth.

**Yara's knowledge profile**
- Personally knows that she carried one wrapped packet to Half-Span.
- Personally saw a route marker change after she stopped forwarding the tone.
- Does not know who played the earlier signal or who heard it.
- Believes her silence caused confusion but cannot know every listener's interpretation.
- May mention the player's help only if an existing quest/relationship owner or stable quest result permits the callback.
- Should never be used as a source for a verified route status she has not inspected.

**Tovan's knowledge profile**
- Knows the local marker and current route only to the extent current location/travel owners support.
- Can explain the Lantern Wardens' practice and why they want local confirmation.
- Does not know whether every distant shelter received the old message.
- May be confident about local repair but must attribute a wider claim to its source.
- A line should not expose the exact state of another location merely because he is a route steward.

**Iri Venn's knowledge profile**
- Knows the Compact's authored reporting terms and accepted procedures.
- Can say a report was received only when current route/story facts confirm receipt.
- Does not know that a recipient agrees or that a local path is passable without a supported reply.
- Can acknowledge that the network's former message is outdated without admitting a sabotage fact the story does not establish.
- Should not use technical certainty as a substitute for personal empathy.

**Pell's knowledge profile**
- Knows how the relay plate is maintained if their character sheet and scene place them there.
- Understands the difference between “heard,” “inspected,” and “confirmed” as a developing lesson.
- Does not automatically infer hidden route facts from a skill statistic.
- Optional explanation can appear through a supported knowledge or visit context; the ungated main scene conveys enough to proceed.

These cards can be attached to content review without introducing a global speaker-memory service. If character knowledge is persistent in current source, cite that owner. If knowledge is authored per scene, keep it within the scene and do not imply recall elsewhere.

### Rumor and correction chain

The storyline's information route is a chain of attributed content, not a world-state broadcast graph invented by dialogue:

1. An old authored instruction defines what a tone once meant.
2. A current signal/audio event or report causes a character to say that the tone was heard. If the game has no such event owner, the content uses an attributed report rather than asserting playback.
3. Yara provides her personal account of carrying or withholding a packet.
4. A local inspection or report owner may accept a narrow observation about a plate or marker.
5. The player can submit a correction request only through an owner that accepts it.
6. The Farline Compact may respond through its faction/current message owner if the request is eligible.
7. Later characters can repeat only the accepted result and its scope.

Each edge is a content reference, an event, an owner read, or a command. A rumor does not magically propagate to every NPC. A new line is eligible when the appropriate source is current. If no persistent delivery state exists, do not show an NPC reacting to a message that supposedly reached them.

### Gate matrix for source and scope

| Available context | Safe dialogue variant | Unsafe variant to reject | Gate visibility |
|---|---|---|---|
| Player personally heard a supported signal event | “I heard the two notes near Half-Span.” | “The relay sent a warning.” | A knowledge-gated response can reveal the player's observation if the event is supported. |
| Player has only an attributed report | “Yara said the old tone was heard last night.” | “The tone played last night.” | The source should remain in text; do not hide the uncertainty. |
| Relay plate inspected, date missing | “The plate has no readable date.” | “The relay has been sending this for years.” | Inspection can be an optional finding; the missing date is still an explicit unknown. |
| Route owner says current route unavailable | “The approach is currently closed.” | “The old signal caused the collapse.” | Route fact can be public; causal claim needs separate evidence. |
| No current source for signal origin | “We do not know which point sent it.” | “It came from Siltglass.” | Use a neutral branch, not a false condition or guess. |
| Compact owner records receipt but not adoption | “The Compact received the correction.” | “The Compact agreed.” | Receipt and adoption are distinct outcomes. |
| Local owner records an inspection | “The Wardens inspected this marker.” | “The whole route is confirmed.” | Keep geographic scope precise. |
| Old-save context lacks optional scene fact | “The status cannot be checked here.” | “The group never answered.” | Treat missing integration data as unknown, not negative. |

A condition should specify subject and scope. “Signal heard” is incomplete: who heard it, where, and under which source? “Route known” is incomplete: known to which actor, at what time, from which owner, and for what action? Authors can use concise predicate labels once their semantics are registered under current project conventions.

### Conflict and correction language

When two accounts differ, present their sources side by side without forcing a winner:
- “Yara carried the packet to Half-Span.”
- “The Relay plate has no date.”
- “Tovan says the lower approach changed.”
- “The current map owner marks the approach unavailable.”
- “No reply has been confirmed from the Compact.”

The final dialogue can acknowledge a conflict: “Those facts do not answer the same question.” It should not write “someone lied” unless a supported result establishes deliberate deception. If a prior line is corrected, the correction should state which source changed and why. It should not rewrite the player's memory or remove an already earned clue.

A player may challenge a character's claim. The response should expose the current source boundary:
- “Did you see the tone sent?” — “No. I saw the packet after the route changed.”
- “Did the Compact agree?” — “I know they received the report. I don't know what they decided.”
- “Is the crossing safe today?” — “Ask the route owner. I have not walked it today.”
This makes knowledge gates legible and character voice specific without adding numerical trust.

### Correction and supersession policy

New evidence may supersede a prior report, but not erase the fact that the prior report existed. The narrative can distinguish:
- **correction:** the source updates a claim;
- **qualification:** the source narrows its earlier scope;
- **retraction:** an authorized source withdraws a claim;
- **contradiction:** another source reports something incompatible;
- **unresolved disagreement:** the content has not established which claim is valid.

The quest owner determines whether a corrected fact changes an objective. The campaign owner, if any, determines how ending interpretation consumes it. A dialogue node may route from current accepted facts to new copy but cannot overwrite the event history. If the current data/save contracts do not preserve an earlier report, a later scene should say “the current report says” rather than promise a complete archive.

### Context refresh when new information arrives

A scene may remain open while new facts appear. The graph should refresh only through the current dialogue/context owner and the owners that provide the relevant facts. Preserve cursor/focus when possible; if an option disappears because its precondition changed, explain the update and return focus to a safe remaining option. Do not commit the old selection under a new context.

Scenarios to specify:
- the player inspects the relay while a dialogue panel is open;
- another current event changes route availability;
- the Compact response arrives after the player leaves the conversation;
- a quest becomes terminal after an accepted correction;
- an optional package unloads or is disabled under supported configuration;
- a saved game restores before and after the owner accepted the report;
- an audio cue is missing while text-report content remains;
- the speaker leaves the location before a command is submitted.

A read snapshot can help keep one scene internally consistent, but it is not a fictional atomic snapshot of all game systems. Commit-time checks remain with the relevant command owner.

### Coverage fixtures and contradiction audit

A focused dialogue content review should include profiles that differ in source provenance, not only skill level:
- heard signal directly, no relay inspection;
- inspected relay, no signal event;
- heard from Yara, no source identity;
- clue from Old Echo, no faction contact;
- accepted local correction, no Compact response;
- Compact response received, no adoption fact;
- conflicting route report and current route owner result;
- stale save with no optional package;
- failed or abandoned quest with a later corrected report;
- two speakers with different knowledge limits;
- missing optional audio cue;
- localized branch whose line must retain source attribution.

For each profile, capture the eligible statements, hidden/disabled options, explanation, and post-selection result. The review rejects any combination that turns a rumor into fact or treats operational unavailability as a character refusal. A “false” context and an “unknown” context need separate coverage whenever they select different player-facing copy.

### Narrative integration and authoring handoff

Plan 20 owns the exact lines, speaker voices, and graph topology. This plan owns the definition and fairness of gates. Plan 19 owns authored/generated record boundaries and package closure. Plan 22 owns the result route after a command. A writer can propose a new knowledge gate, but the integration packet lists which verified fact supplies it and who can refresh it.

A new line about the Compact changing its labels must reference an accepted owner result. A line about a sound heard at a location must use a supported event or attributed report. A line about the current route must read the map/travel owner. This separation allows the story to remain vivid while preventing convenient prose from becoming a parallel world-state database.

The implementation slice should begin with one optional source-aware branch at Half-Span. It proves that the player can distinguish direct hearing from an attributed report and still complete the main task without the branch. If no current source adapter exists, author the neutral version and record the unresolved integration need.


## Continuation pass 13 — context matrix, source credibility, and gate fairness audit

### Why this matrix exists

Dialogue gates can make a world feel responsive, but only when they expose information the player could plausibly know and do not conceal essential progress behind an opaque condition. The Farline Circuit needs several forms of context: the player may have inspected a record, heard a rumor, earned a character’s confidence, learned a skill-relevant fact, or visited a location after it changed. This module turns those ideas into an authoring and review matrix. It does not propose a new global memory database. Facts should continue to belong to the existing quest, character, faction, location, and save owners that already persist them.

The main failure mode is an invisible gate that authors understand but players cannot infer. A line appears only after an unspecified “trust” threshold; the player has no clue why a character is withholding it. Another failure mode is a gate that creates a soft lock: a required clue is available only through a companion who has left. A third is a gate that has no narrative meaning, such as requiring a high skill merely to hear a phrase that could be stated openly. Every gate needs an authored purpose and an unblocked fallback for critical information.

### Context categories and evidence confidence

Use a small vocabulary in design notes to separate context sources:

- **Observed:** the player personally interacted with a clue or event.
- **Reported:** a character told the player something; the source identity and confidence are known.
- **Inferred:** the player combined existing evidence through a supported action or authored choice.
- **Recorded:** the player has an accessible note, map entry, or journal fact.
- **Believed:** a character’s interpretation, which may be contradicted.
- **Corrected:** earlier information has been explicitly revised.
- **Unknown:** no valid source is available or the player chose not to establish one.

This vocabulary is for authoring clarity. It only becomes implementation data when the current content schema and state owner can express it. Do not add an evidence confidence enum by assumption. Existing quest objective flags, clues, or narrative facts may already cover the relevant distinction. Where the current architecture lacks such a concept, phrase the design as a requirement and seek a bounded decision.

A useful context fact includes who or what established it, when it became available in the story, whether the player personally saw it, and whether later content may supersede it. An unnamed global “knows_relay_secret” flag is not enough if it conflates two sources with different reliability. Conversely, persisting every line heard by the player creates unnecessary state. Store only facts needed for a later decision or meaningful variation.

### Gate design card

Every gated line or response gets a short card:

1. **Gate type:** quest, knowledge, relationship, faction, location, skill, time, prior action, or combination.
2. **Player-facing clue:** what tells the player this gate may exist.
3. **Material benefit:** what new information or action it provides.
4. **Criticality:** optional flavor, useful context, alternate route, or required progression.
5. **Fallback:** how the player can obtain essential information without this gate.
6. **Visibility:** whether the locked choice is shown, hinted, or omitted under current UI norms.
7. **Persistence:** which current owner stores the prerequisite.
8. **Expiry:** whether the opportunity can disappear and what happens then.
9. **Truthfulness:** whether the line distinguishes evidence from belief.
10. **Revisit behavior:** whether the line stays available, changes, or closes.

The gate card discourages “because the graph can” complexity. A skill gate that only gives a more poetic description may be a cosmetic variant. A skill gate that identifies a pressure mark can supply useful evidence but still should not decide the investigation automatically. A relationship gate can permit a more personal question but should not hold a required clue hostage unless an alternate source exists.

### Farline gate matrix

| Scene or content | Candidate gate | Added value | Required fallback |
|---|---|---|---|
| Siltglass relay clue | Location discovered or quest active | Makes an interaction available at the correct place | An authored report route if the site cannot appear |
| Timing comparison | First pulse observation recorded | Lets the player compare interval rather than repeat the initial observation | Journal can state that no valid comparison exists |
| Yara’s deeper question | Prior respectful recordkeeping or established relationship fact | Personal explanation of why she preserves exact wording | Basic practical dialogue remains available |
| Pell’s diagnostic detail | Relevant crafting or repair skill, if current system supports it | One physical observation about regulator wear | General explanation retains story-critical meaning |
| Iri’s shorthand correction | Previous route-mark clue or direct inspection of the copy | Clarifies that a mark denotes passage, not source | She states the distinction directly if needed for progress |
| Public notice options | Evidence quality and player knowledge | Offers only wording the player can support | Always offer an honest “incomplete” treatment |
| Hidden yard interaction | Environmental clue or prior visit | Optional route into the side chain | Main quest does not require it |
| Faction courier follow-up | Existing faction availability state | Shows how the chosen notice affects delivery practice | A board note conveys essential closure if the courier is absent |

The matrix should be checked against the actual game systems before use. For example, “repair skill” is a candidate only if a relevant skill and dialogue condition already exist. If there is no such source, use a non-gated diagnostic line or an explicitly approved implementation change. Never invent skill labels or numerical thresholds for content convenience.

### Visibility and fair signaling

A visible locked response can communicate that the player lacks a known capability or relationship, but it should not reveal confidential content. If the current UI supports locked choices, its hint should be truthful and concise: “You do not recognize the notation” is clearer than “You are not ready.” If locked choices are not part of the current interface, do not add them to the design as though already supported. A character’s reply can instead signal the missed opportunity in natural dialogue.

A gate is fair when a curious player can connect it to prior experience. If an earlier repair scene reveals that Pell cares about failed regulator parts, a later technical question can use the player’s related skill. If nothing established that association, the gate feels arbitrary. If a faction choice affects access, the game should expose the relevant relationship through ordinary events before the player reaches the locked exchange. Do not make the player reload to discover a hidden threshold.

For essential information, use at least one route that does not depend on a single character’s trust or availability. The route can be longer, riskier, or less detailed, but it must be viable. The player should not need to recruit Yara to learn the objective’s only required clue. A character arc can reward trust with context and personal stakes while the core task remains playable.

### Knowledge provenance and corrections

When a character shares a claim, the dialogue should distinguish direct experience from inherited report. “I heard it at the relay” differs from “the courier said it came from the relay.” If the speaker cannot recall the source, preserve that limit. These distinctions are particularly important for the Farline Compact because messages move through several hands.

A later correction should not retroactively make the player foolish for acting on information that was previously presented as reliable. The correction scene should identify what changed: a new source, a clearer artifact, or evidence that disproves the earlier interpretation. If the player made a reasonable choice under uncertainty, the reaction acknowledges that. Reputational effects can arise from the player knowingly presenting an unverified claim as confirmed, but only if the game clearly communicated its confidence and the relevant faction owner can represent that consequence.

Corrections also need a precedence rule. A source is not erased just because a newer report disagrees. The journal can preserve both in a concise form: earlier account, later evidence, unresolved point. If the current journal cannot show a chronology, avoid promising an auditable evidence ledger; instead, use a short updated summary that indicates a correction occurred. The player-facing result matters more than exhaustive internal provenance.

### Relationship context without manipulation

Relationship-based dialogue should describe familiarity, reliability, or a personal boundary, not a single hidden approval score. If the game has a current relationship model, the plan must inspect its semantic scope and avoid adding a shadow measure. If the model only records broad affinity, a line that assumes deep trust may be inappropriate. Use an observed prior action when it conveys the connection more clearly.

A fair relationship gate has a visible cause and proportional result. If the player previously kept Iri’s copied note intact, she may offer to explain the shorthand she uses privately. If the player sold or discarded the note, she may decline, but the basic route fact remains accessible. The line should not claim betrayal unless the player was told the item was entrusted to them. Characters can remember practical care without becoming a reward vending machine.

Repeated visits require special care. A “they remember you returned” line may be cosmetic and should not imply a deep relationship state. If repeated visits can alter a relationship, identify what meaningful action happened during the visit. A visit counter alone is not necessarily a good cause. Avoid making NPCs seem to remember every map transition; use clear authored milestones and relevant events.

### Faction and location gates

Faction access is a meaningful consequence when it changes actual options: a courier accepts a note, a notice reaches a route, a safe location opens, or a task becomes available. A line that says “the Compact trusts you” without an owner or observable result is not integration. If standing is not currently modeled for a proposed group, this plan should not add it through dialogue. The story can express a local individual’s confidence without implying a faction-wide state.

Location-specific dialogue should use the location’s current condition, not just its static identity. At the relay, a weather-damaged board may change what the player can inspect. At the shelter, an active kitchen shift can alter who is free to talk. The condition should come from current location or world-state ownership, and the dialogue must degrade gracefully if the richer state is unavailable. Do not put a duplicate weather counter in the dialogue graph.

A location gate can also prevent continuity errors. If the player has not visited the site, a character should not discuss what they personally saw there as common knowledge. If the information came by report, identify the reporting path. A character may be wrong, but the design should know whether they are lying, mistaken, or simply using incomplete information.

### Gate ordering and branch convergence

When several conditions apply, the dialogue should prefer the most specific eligible line and then fall back to a stable general line. The ordering should be deterministic and explicit in the current graph representation. For example: completed quest callback before active quest progression; newly observed evidence before generic topic; location-specific response before ordinary hub response; then ambient line. The exact runtime order must follow the current consumer, not a new priority algorithm written into prose.

Short branches should reconverge after the meaningful distinction has been acknowledged. If the player asks about timing, the response can vary with knowledge, then return to the same choice about posting a warning. Avoid cloning the remainder of a scene for every context combination. Exponential graph growth makes review and localization fragile. A typical scene should isolate one or two meaningful branches and reconverge; major consequences can start a separately authored follow-up node.

Branch order must also handle overlapping eligibility. If the player has both a high relevant skill and strong relationship, authors need to decide which line appears, whether one response combines both, or whether one context takes precedence. The user should not receive duplicate response buttons that differ only in tone. Content validation can detect overlapping predicates if the current tool supports it; otherwise a manual condition matrix is required.

### Negative test scenarios for gates

A gate review should deliberately try cases where context is absent or contradictory:

- The player heard the rumor but has not seen its source.
- The player discovered the location before taking the quest.
- A required character left after the player met the first prerequisite.
- The quest’s evidence was gathered through an equivalent site.
- The player explicitly selected “I do not know.”
- The player learned a fact from an unreliable report.
- A faction standing condition is below the proposed threshold.
- A skill is present but no relevant interaction was performed.
- The player revisits after an authored correction.
- A save is restored with the quest active and one line already consumed.
- Two gates are simultaneously true and compete for one response slot.
- A temporary location has expired while the player is on another expedition.

For each case, record the expected available lines, objective state, and player-facing explanation. A test that only checks that a line exists does not establish fair availability. Verification should eventually prove that the relevant owner supplies the prerequisite and that a save/reload keeps the same choice set. This plan does not create tests before the implementation seam is approved.

### Review rubric

Reviewers score each gate as necessary, useful, or decorative. Necessary gates protect continuity or a real gameplay dependency. Useful gates add an alternate route or a meaningfully different perspective. Decorative gates alter wording only and should be labeled as cosmetic. Any gate that changes a consequence without clear player signaling needs revision. Any required clue with no alternate access is a soft-lock defect. Any condition whose data owner cannot be named remains a design dependency.

The dialog graph should be reviewed on both a full-context path and a low-context path. The full-context path tests whether extra knowledge feels earned rather than omniscient. The low-context path tests whether the player can still act. Where possible, review with two authors: one who wrote the content and one who follows the graph cold. If the second reviewer cannot explain why an option appeared, the gate needs better signaling or a simpler condition.

### Core and expansion boundary

The minimum viable dialogue system requires stable authored nodes, speaker and location identity, supported conditions, player responses, defined effects, and reliable next-node routing. A compact conditional branch and a hub can prove this. Long-term memory, hidden emotional states, broad reputation gates, skill-specific dialogue, and time-aware world changes are optional expansion layers, only if present owners can support them or an approved architecture decision adds them. This boundary keeps the Farline story expressive without turning every conversation into a general-purpose simulation.


### Worked gate audit: the player arrives with conflicting accounts

Imagine the player comes to Half-Span Shelter having heard a traveler say the relay tone was a distress call, while a maintenance card suggests that a regulator could repeat a pulse mechanically. The player has not personally inspected the relay and has no repair skill. This is a useful low-context case because several pieces of information compete, but none is confirmed.

Yara’s opening should ask where the claim came from, not assume the player believes it. If the player identifies the traveler, the dialogue records that the claim is reported and preserves the source. If the player paraphrases without attribution, Yara asks whether the player heard it directly. This is a knowledge-provenance prompt, not a trust test. The player can answer “I do not know” without losing access to the investigation.

Pell can explain that an old regulator may repeat a tone, but he cannot diagnose the specific relay without seeing its condition. A repair skill may reveal one extra observation about the card if the current game already supports such a condition. It cannot transform possibility into proof. The core option remains: compare the relay’s physical marks or seek another source.

If the player later visits Siltglass and finds a mechanical fault, the dialogue can distinguish that observation from the traveler’s account. The traveler may still have heard the tone accurately; only its interpretation changes. A later character must not say “the traveler lied” unless evidence actually supports deliberate deception. The correction should say that the earlier report was incomplete.

If the player never gets access to the relay, the quest’s alternate source can establish that the mechanism was plausible but not definitive. The conclusion remains less certain. The knowledge gate enriches the investigation without deciding it. This audit catches a common narrative shortcut: treating contradictory statements as proof that one speaker is dishonest.

The implementation review records the exact state owner for each fact and the line selection for each profile. It verifies that the two accounts do not collapse into one “knows signal” flag and that a save reload preserves what was observed versus reported. If the current data model cannot express the distinction, the content is simplified or the owner requests a specific architecture decision.

### Memory minimization and fact expiry

Dialogue memory should persist only what changes future choices or makes a later acknowledgement trustworthy. The player does not need a durable fact for every greeting, every line read, or every time a hub was opened. An authored memory candidate should be tested with one question: if this fact were removed after the current scene, would a later available choice become incorrect or misleading? If not, it is likely transient presentation.

Some knowledge should expire or be superseded. A temporary route closure may cease to be current after repair, while the fact that the player once learned of the closure can remain in a journal history if that history is already supported. A rumor does not become true when a later message repeats it. A corrected location condition should gate current instructions, while old dialogue should not reappear as current truth.

Expiry must be tied to a domain event rather than arbitrary cleanup. If a world owner reports that a crossing is open, a dialogue condition can prefer current access information. If there is no such state, keep the claim explicitly historical: “the last report said it was closed.” Never erase a player’s meaningful action to simplify dialogue selection. The existing save owner decides what is restored; this plan only identifies which narrative distinctions merit persistence.

### Revisit policy

A revisited topic can change because of new evidence, a corrected report, a completed delivery, or a relationship event. The condition should name that cause. “Second visit” is appropriate only when repetition itself matters, such as a character recognizing the player has returned to ask again. Even then, it changes the conversation’s tone rather than inventing new knowledge.

Keep a general hub route available after a gated topic closes. The player should be able to leave the topic, ask about another matter, or return when the stated prerequisite is met. A gate that traps the conversation in an empty response state is a routing defect.

### Knowledge summary language

Journal summaries should distinguish what the player saw, what someone reported, and what remains an interpretation. Prefer “The traveler said the tone called for help” over “The tone was a distress call” when the player has no direct evidence. Prefer “Pell thinks the regulator could repeat the interval” over “Pell identified the source” when he has not inspected the relay.

Keep summaries concise and update them when a correction changes the current best explanation. Do not erase the earlier account if the player may have acted on it; mention that a later clue revised the interpretation. Where current journal support cannot preserve both stages, write a truthful current summary and avoid claiming a complete evidence history.

These summaries are part of the gate’s fairness. They help players understand why a new dialogue option is available and which assumptions remain unresolved.

## Continuation pass 14 — Empty Shift dialogue context matrix and evidence-aware gate policy

### Context objective

The Empty Shift investigation puts a specific pressure on dialogue architecture: speakers can remember a shift, read a roster, infer who performed a task, or repeat someone else’s account. Those are not interchangeable. The dialogue system needs to select lines from facts the player has encountered and the speaker could plausibly know. It must also let the player move forward when they lack an optional skill, relationship, or location visit.

This is an authoring contract around existing dialogue and quest owners. It does not add a new global memory database or a numerical credibility subsystem. The first playable version can use existing quest stages, journal facts, relationship state, location access, and skills where verified. Anything unsupported remains a content dependency.

### Fact classes for this storyline

**Direct observation:** the player personally inspected the two roster copies, a mark, a latch, a tool, or the service panel. The fact must correspond to an actual inspect interaction or other current game event.

**First-person testimony:** Mara, Oren, Nessa, or Sella reports what they personally did or saw. The line’s speaker is part of the provenance. A first-person account can still be mistaken or incomplete.

**Second-hand testimony:** a character repeats what another person told them. The dialogue should identify that route when it matters. “Oren said he traded the shift” differs from “the shift was traded.”

**Record-derived claim:** a roster, meal list, note, or service log contains a statement. The record may be stale. Reading it establishes that the player saw the entry, not that the entry is true.

**Inference:** the player selects or states a conclusion from observations. An inference is a narrative outcome, not necessarily a proven fact.

**Correction:** new information changes the status of an earlier claim. The record of what the player once heard should not silently be overwritten if that history affects a later reaction.

**Unresolved:** evidence is insufficient, a source is unavailable, or the player chooses not to infer. This is a valid state for a completed investigation.

These terms help authors reason. They should map to existing fields and facts if possible. They do not require a new confidence score for every sentence.

### Gate matrix

| Scene | Gate | What it can reveal | Critical path fallback |
|---|---|---|---|
| Two Sheets | Player inspected both roster copies | The exact visible difference or match | Mara reports that they were intended to match, with lower certainty |
| Annex Exterior | Player visited or has a credible report of the annex | Physical condition and access | Service log, panel interaction, or witness route |
| Mara on the Mark | Prior question about the correction mark, or relevant record knowledge | Covered-versus-worked distinction | Margin note in the Dry Shelf Room |
| Oren at the Annex | Oren available and has prior work history | A first-person account of the older task | Another worker describes the handoff without personal detail |
| Nessa on Coverage | Work-board context or a direct question about coverage | That expected assignment may differ from attendance | General explanation remains available after inspection |
| Sella on Portions | Player has raised the meal count or visited Cook Passage | How the count was used | Main quest does not require the meal allocation |
| Technical Observation | Existing repair/inspection skill and actual interaction | A limited material observation | Nontechnical evidence route reaches the same objective |
| Personal Confession | Relevant relationship history or an authored invitation | Why a temporary workaround persisted | The investigation resolves without the personal explanation |
| Return Callback | A resolved report and a later story milestone | What changed on the next shift | Journal or board text provides essential closure |
| Hidden Name Note | Optional clue chain and site availability | A possible reason the roster remained unchanged | Not required for the main chain or ending |

Every row needs a real state source. “Player is thoughtful” is not a gate. “Player inspected both sheets” is concrete if the current objective owner tracks the interaction. “High trust” is too vague unless the existing relationship owner has a defined threshold and the dialogue explains what the player did to establish that relationship.

### Fairness rules for character availability

A character can be absent, unavailable for work, or unwilling to speak. These conditions should produce different dialogue behavior only when the story needs them. Absence may lead to a written note. Work pressure may shorten a scene or defer it until the character is free. Refusal may close that speaker’s personal route while leaving a different evidence path. Do not use the same generic “not available” result for all three states.

If a character is required to authorize a board change, the quest has a single-point-of-failure problem. The player needs a second authorized route, a way to wait, or a closure that does not require the change. A character’s personal revelation can remain gated; an essential definition of the task should not.

The map and journal should distinguish player knowledge from character availability. The player may know where Oren works even when he is not there. The player may know that Mara requested a review even if she leaves before the return. Do not remove a known objective location just because its conversation source has moved.

### Skill gate policy

A skill gate earns its place when it lets the player notice a meaningful detail through a supported mechanic, reduces risk, or unlocks an alternate route. It should not be used to keep basic quest comprehension inaccessible. For this story:

- A repair-oriented skill may identify that the panel scratch is newer than the latch.
- A recordkeeping or observation skill may notice that copies share the same unusual alignment.
- A social capability may let the player ask Oren to explain the old shift without making it sound like an accusation.
- A survival skill may suggest postponing an exposed route during dangerous weather.

Each is conditional on current skill support. If no such mechanic exists, the same information can be available through an ordinary inspect action, with the skilled option offering additional context or efficiency. Skill wording must not claim “you know Oren lied” when the evidence supports only a discrepancy.

The gate review compares success, non-skill, and unavailable-location paths. If only the skill path can complete the main objective, the plan must either revise the quest or request an explicit design approval for a new capability. The absence of that ability cannot be compensated with an undocumented threshold.

### Relationship gate policy

Relationship conditions are best used for personal context and trust-bound actions, not for proof that a public record is wrong. Mara might discuss her own choice to leave a placeholder only after the player has handled the roster carefully. Oren might volunteer for a limited inspection if the player asked before assigning work. Neither condition should make their basic factual account disappear.

When a player has behaved harshly, a character can set a boundary. The fallback can be a written record or another witness. The story should not convert a boundary into universal hostility unless the existing relationship system and player-facing consequences support that outcome. Do not gate a line solely to reward the player for choosing a prior dialogue answer the game did not signal as important.

A relationship variant should change how a character shares personal information, not what happened in the world. If Oren’s account is conditioned on affinity, define whether the player knows that he declined to speak or whether the line is simply absent. Avoid contradictory world facts across relationship values unless the character is deliberately unreliable and the content clearly signals that possibility.

### Location and timing gates

Location-specific dialogue depends on both where the player is and which location state is current. Mara can speak at the Board; the same topic at the Annex may be a brief practical exchange. If the player has already discovered the Dry Shelf before the quest starts, the dialogue should acknowledge that and avoid sending them back to rediscover it. If a location has changed, use the current owner’s state to select a present-tense line.

Timing gates should reflect meaningful story windows. “Before the next shift begins” can matter if there is an existing day or schedule system that advances. If that system is absent, do not simulate a clock in the dialogue graph. A quest can instead make the player decide before leaving the shelter or before ending the current visit, using existing interaction boundaries. A temporary worker’s departure needs an explicit in-world cause and a fallback route.

Repeated visits are not a substitute for timing. A second conversation may acknowledge that the player came back, but it cannot imply that a shift passed unless a world milestone recorded one. This protects the chronology and save restore behavior.

### Gate overlap and deterministic selection

The graph may have several simultaneously valid conditions. For example, the player can inspect both sheets, have a high repair skill, and have a strong relationship with Mara. The content must say whether one combined response appears, one context takes priority, or the first question determines which detail is offered. The same state should not create duplicate responses with indistinguishable text.

Use the existing dialogue consumer’s ordering behavior. Document the priority in a scene map and verify it in runtime when implementation is authorized. Do not prescribe a second selection algorithm in content. If the current consumer cannot deterministically resolve overlapping nodes, that is an architectural gap to raise with evidence.

Branches should reconverge once the context-specific fact has been acknowledged. The skilled player learns about the scrape; the general player can learn the latch was replaced from Oren. Both can then ask what the roster means. This limits node growth and avoids making each skill and relationship combination a full duplicate scene.

### Knowledge memory minimization

Persist only the facts needed for later behavior. The core chain may need to remember that the player inspected both copies, learned the mark’s definition, chose an outcome, and whether a key location was discovered. It need not remember every line read or every time the player opened the board. The current quest or discovery owner should hold each fact in its existing responsibility.

A transient dialogue topic can refresh from current quest state. A durable result must survive save and restore through the current owner. If a later line depends on whether Mara told a personal story, that fact should exist only if the story changes a future choice or callback. Otherwise keep it within the conversation and do not grow save state.

If the player learns the same definition from the margin note before speaking with Mara, later dialogue can recognize that shared knowledge and move to a more useful question. The note may still provide its own evidence. Do not mark the clue undiscovered merely because the NPC route was skipped.

### Worked cases

**Player accepts and visits only the board.** The player learns that the roster copies match, but does not know if the shift was worked. The Annex objective remains available. Dialogue offers a practical next action.

**Player finds the Annex first.** The player sees a fresh scrape but not who made it. When Mara later offers the quest, the journal acknowledges the prior inspection and keeps the clue.

**Mara unavailable.** The player can find the margin note or ask Nessa. The personal explanation of Mara’s workaround is missed, but the quest is completable.

**Oren refuses.** The player can use the repair trace and another account. His boundary is respected; the journal states that one account was not obtained, not that he is guilty.

**Sella’s allocation is absent.** The resource side quest is omitted or uses an authored journal note. Main quest progression is unaffected.

**No skill.** The player can describe the scrape as newer only after inspection text tells them; the skilled response gives a more precise comparison. Both routes preserve the evidence distinction.

**Conflicting sources.** The player can keep both accounts and resolve the task as uncertain. A later line acknowledges the unresolved difference rather than arbitrarily choosing one.

### Review and acceptance

The author and a cold reviewer should walk all cases from the gate matrix. For every option, they answer why it is visible, what fact it conveys, whether that fact is essential, where it came from, and what happens if the source is absent. Any essential line that appears only because of an invisible relationship or skill threshold is revised. Any node that can be reached with contradictory world state is a content defect.

A future implementation can add focused verification for the supported combinations, but this plan does not create tests or assert current coverage. The acceptance package must identify the exact current owners for quest facts, dialogue routing, relationship state, location state, skills, and save restore. If one of these is unsupported, simplify the content or request a new architecture decision.


### Dialogue condition evaluation as a maintainable boundary

A scalable dialogue design separates authored content from the state used to decide whether that content can appear. The author writes a node condition using concepts the game already owns: active quest stage, discovered location, known clue, speaker availability, relationship value, current faction standing, skill capability, or a world event. The runtime consumer obtains a read-only view of those facts, selects an eligible node using its existing deterministic rules, and presents the response. When a player chooses an option, the established command path routes its effects to the current owner.

This is a conceptual boundary, not a proposal to create a new universal condition engine. The first audit should determine whether a dialogue resolver and condition grammar already exist. If so, use and extend that contract. If dialogue conditions are currently embedded in host UI code, the architecture owner may choose to improve that seam separately. These plans should not create another expression language simply because content authors need a matrix.

The context view should be treated as a snapshot for one interaction. It prevents a scene from showing one option based on stale context and then applying an effect against unrelated state. The snapshot is not itself persistent. The source owners remain authoritative. A player action is validated again at the command boundary before effects are accepted.

### Condition categories and authoring semantics

Conditions can be grouped conceptually into:

- **Campaign:** current chapter, day/phase, or ending eligibility.
- **Quest:** active, resolved, blocked, failed-forward, or specific evidence gathered.
- **Knowledge:** player has observed or heard an identified fact.
- **Character:** speaker or target exists, is present, is willing, or has a relevant state.
- **Relationship:** an existing relationship condition supports a more personal route.
- **Faction:** an existing access or reputation owner permits the exchange.
- **Skill:** a current skill capability exposes a useful observation.
- **Location:** current place, discovered place, access state, or interaction state.
- **World:** an existing simulation fact such as weather, resource, or condition.
- **Prior action:** player used, refused, delivered, repaired, or preserved something.
- **Presentation:** one-time scene visibility or current node routing, usually transient.

These categories must not blur into one mutable “dialogue memory” object. Campaign state remains campaign state. A quest clue remains with its current quest or journal owner. Character availability is owned by the character/lifecycle system. Presentation state may be ephemeral if revisiting should replay. A content author references the owner’s fact; the graph does not duplicate it.

A compound condition is reviewed as a logic statement. “Player inspected the roster and either heard the margin note or asked Mara, and Oren is available” should be broken into named facts and tested for each route. Avoid opaque strings or Boolean fields whose meaning is not documented.

### Condition precedence and branch composition

A node can be eligible under several contexts. Use a documented preference order that follows current dialogue routing. A typical design priority might be: urgent closure; newly available quest progression; response to a recent irreversible choice; correction of an outdated fact; location-specific topic; character follow-up; ambient conversation. This is a proposal for author review, not a runtime order until checked against the actual resolver.

If two conditions provide compatible information, combine them into one authored node or short variant. If they conflict, decide which fact is relevant to the player’s current question. For example, a high repair skill can reveal a fresh scratch while a strong relationship allows Oren to describe his former duties. The player should not receive a response that simultaneously claims the scratch proves Oren’s presence and that Oren refuses to discuss the room.

Branch composition stays small by reconverging after the meaningful difference. The extra skill observation can be a single node that returns to the same “what does the mark mean?” hub. The personal line can add one sentence before returning to the common task. If a prior choice changes the actual quest result, its route remains separate only until the supported outcome is established; later ordinary dialogue may share nodes again.

### State freshness and stale context

A dialogue option can become invalid between display and selection if a character leaves, a location closes, or another action resolves the quest. The host should validate the command against current owner state. If it is no longer valid, the player receives a graceful response and refreshed options. This protects against duplicate or stale UI interactions.

Do not keep a long-lived cached dialogue context that copies quest, faction, and relationship values. Recompute from current owners when opening or refreshing the scene, using the project’s existing pattern. A stale cache can show a line that belongs to a previous outcome. An over-eager refresh can also interrupt a scene, so only update at meaningful boundaries, such as returning to a hub or completing a command.

Save/restore should reconstruct options from restored authoritative facts. If the player saved while a scene is open, the current interaction contract must define whether it reopens at the same node or returns to the hub. Do not persist a copy of all conditions. Persist only the player’s choice or fact through the normal owner where required.

### Detailed gate matrix: six context axes

The following axes make author review systematic. They do not mandate new state fields.

**Evidence axis.** None, reported, directly observed, independently corroborated, corrected, or unresolved. A direct observation must be tied to an inspect or event. Independent corroboration requires a distinct source, not a second conversation with the same person.

**Speaker axis.** Present, absent, off shift, willing, unwilling, or unknown. Avoid treating unknown as absent. Use alternate text only when the story has a plausible source.

**Location axis.** Unknown, approximate, discovered, accessible, blocked, or changed. A line must not assume the player visited an undiscovered location. A changed location uses a current owner.

**Relationship axis.** Unspecified, familiar, trusted for personal context, or currently strained, but only when the existing system supports these meanings. If it offers only a scalar, authors must map these bands after inspecting that API.

**Skill axis.** Capability absent, capability present, action performed, or result known. Having a skill is not the same as having inspected the clue. Use skill gates for what the player can do or notice, not for arbitrary exposition.

**Campaign axis.** Before request, active, pending closure, resolved, or later callback eligible. Exact labels map to current quest/campaign state.

For each scene, produce a compact cross-product review that checks meaningful combinations rather than every theoretical combination. The key is to cover boundary cases: no evidence, one source, contradictory sources, missing speaker, inaccessible location, no skill, prior discovery, and completed outcome. Any combination that would mislead the player or leave no response must have an explicit resolution.

### Player-visible gate signaling

A hidden option need not be shown if its absence carries no essential consequence. A visible unavailable option can be useful when the UI convention already supports it and the player can understand why. The game should not reveal a future secret just because an option is greyed out. It can say “You have no repair insight to add” or let the character naturally answer the broader question.

When an option appears because of a skill or relationship, the player should understand its benefit. The option can mention the relevant action: “Compare the scratch to the new latch” or “Ask Oren about the old handoff.” Avoid options like “Use your experience” that reveal no intent. A choice’s label must not promise a result beyond its actual effect.

If the player has no eligible special response, a general option remains. For the roster conversation, “Ask what the mark means” is always available. A skill adds an observation; relationship adds personal context; prior clue adds a precise comparison. None blocks basic comprehension.

### Memory source and scope matrix

| Remembered fact | Keep it where | Dialogue use | Expiry/revision |
|---|---|---|---|
| Board was inspected | Existing quest/discovery owner | Skip redundant inspection prompt | Permanent discovery or current owner’s rule |
| Definition came from Mara | Quest/journal if later source matters | Character callback | Superseded only by correction |
| Oren declined to answer | Dialogue/relationship only if it affects future route | Respect boundary on revisit | Do not expire without new interaction |
| Player chose annotation | Quest result or existing world owner | Show outcome callback | Durable while referenced |
| Meal recommendation | Existing food owner only if applied | Report actual result | Updated by later count if supported |
| Player revisited a hub | Usually transient | Brief recognition line | No durable state unless meaningful |
| Old roster was wet | Location presentation if player saw evidence | Optional description | Cosmetic unless it affects clue availability |
| Hidden impression found | Existing quest/discovery owner | Unlock optional scene | Remains discovered |

Do not persist all eight rows automatically. The table helps decide which facts matter. A persistent fact needs a current owner and a reason to survive.

### Dialogue state machine review without adding a state machine

For each authored scene, reviewers can sketch its reachable positions: entry, topic hub, optional question, branch response, player decision, result acknowledgement, closure. This is a graph audit, not a new code abstraction. Every visible response leads somewhere valid. Every one-time effect is behind a validated selection. A closed branch returns to the hub or exits cleanly. A saved and restored conversation cannot award the same result twice.

Common defects include a node that can only be entered after its own effect; a branch whose exit references a missing node; a response that updates the quest but leaves the map in the prior state; a one-time response that remains selectable; and a personal line shown after the speaker’s absence. The review spreadsheet or graph tool, if existing, can surface these. No new editor is needed before content demand is proven.

### Scenario table: eligibility and player understanding

| Player context | Available information | Fair response | Avoid |
|---|---|---|---|
| Has not inspected board | Request and basic task | Explain where the board is | Treat the names as observed |
| Inspected board only | Copies match; mark unclear | Ask where to look next | Assert a shift occurred |
| Heard Oren’s report | First-person account if sourced | Ask for independent evidence | Label the account confirmed |
| Found margin note | Covered may differ from worked | Compare note with current copy | Reveal the note before inspection |
| Has repair skill | Material detail after interaction | Offer a technical comparison | Skill grants culprit identity |
| Mara unavailable | No personal account | Use note or another witness | Hide the objective |
| Oren refuses | One source unavailable | Continue via physical clue | Treat refusal as guilt |
| Selected unresolved closure | Known mismatch, unknown attendance | Acknowledge limits | Reopen as active without cause |
| Learned correction later | Earlier claim revised | Explain what changed | Rewrite history as if player never heard it |

### Quality and implementation readiness

The implementation-ready scene package identifies the exact current condition sources, effect route, save owner, and dialogue refresh behavior. It includes a manual matrix or existing validator evidence for key condition combinations. It does not add a generalized memory catalog, relationship layer, or condition syntax without an architecture decision.

A playable vertical slice needs only five context facts: board inspected, one independent clue observed, one speaker available, report outcome selected, and callback milestone reached. If the runtime cannot express all five, reduce the content until it can or seek a bounded owner decision. This gives the narrative team a realistic foundation and prevents optional context from outrunning implementation capacity.


### Dialogue memory and world-state interaction boundaries

A dialogue graph can react to world state without owning that state. The graph asks whether the Annex is accessible; the current location owner answers. It asks whether the roster clue was inspected; the quest/discovery owner answers. It asks whether a worker is available; the current character or work owner answers. It asks whether the player chose annotation; the quest result or board owner answers. This read path must not copy values into a second mutable store.

A player response travels in the opposite direction. The graph describes a supported command or effect, but the owning system validates and applies it. An option can be hidden if the command is unavailable. A stale option must be rejected gracefully at the command boundary. The dialogue system presents the owner’s actual result, including failure or pending status when supported.

This boundary is especially important for relationship dialogue. A graph condition can read a current relationship fact; it should not directly increase affinity because a line was selected. If conversation itself changes a relationship under existing design, use the established interaction outcome. Avoid hidden multipliers based on response length or number of visits.

### Context snapshot and refresh policy

When a conversation opens, the current consumer gathers the relevant facts into a temporary evaluation context. It selects eligible topics and responses. The context is discarded or refreshed when the interaction closes or an owning action completes. It is not saved wholesale.

Refreshing too frequently can cause the conversation to jump topics while a player reads. Refreshing too rarely can show stale options. The current UI lifecycle should define when the panel asks for updated content: on open, after a quest command, after a character availability change, and after returning from a map transition. If another owner changes while the conversation remains open, the player should not lose focus unexpectedly. The selected command is revalidated on submission.

A fallback response is required if conditions change after rendering. For example, the player selects “Ask Oren about the old shift,” but Oren leaves before the command is processed. The system can show a short response that he has gone and preserve the note route. It must not attempt to call a destroyed node or apply a stale relationship effect.

### Gate transparency and accessible control flow

The user interface should preserve predictable focus through conditional dialogue changes. If a response disappears after a state refresh, focus moves to a sensible remaining choice and a visible message explains the change. The close/back action remains available. A keyboard or controller user can reach all valid responses. Color, portrait expression, or audio tone cannot be the only indicator that a choice is restricted.

If locked choices are shown, the label and reason use plain language. “Unavailable: Oren is not here” is functional. “You lack trust” is not appropriate if the game offers no visible explanation of how trust was established. If the UI does not currently support disabled choices, do not design around them; use natural dialogue or omit the option.

A knowledge gate should have an accessible source. The player can inspect a note through the text interaction, not only parse a tiny mark. A skill gate should state its benefit in the response. A faction gate should indicate access conditions through existing feedback. A character gate should not expose sensitive personal content as a locked tooltip.

### Gate composition examples

**The board was inspected, but the Annex was not.** Show the exact board observations and a route hint to the Annex. Do not show the technical panel observation.

**The player has a repair capability but has not inspected the panel.** Offer an invitation to inspect; do not pre-reveal the result because the skill exists.

**The player inspected the panel but has no repair capability.** Show the fresh scratch as a basic fact and leave its cause uncertain. The skill path can compare wear but not identify a worker.

**The player heard Oren’s story before meeting Mara.** Mara can ask whether the account is direct and whether the player has another source. She does not repeat a basic objective as if the player knows nothing.

**The player found the margin note before the scene.** The node can acknowledge the note and ask what it means to the current roster. It must not show the note again as a discovery reward.

**The player has a strong relationship with Oren but never visited the Annex.** Oren can share his personal boundary or work history, but cannot describe a recent panel mark he did not see.

**The player has low standing with an existing group.** A member may decline to alter an official record, but the player can still close their investigation through a local or personal route if main progression requires it.

**The player selected unresolved closure.** Normal hub topics remain available. The investigation does not reappear as an active decision unless new evidence causes an authored reopen.

### Fairness review for hidden emotional cues

The broader dialogue design may eventually use hidden emotional states, but the Empty Shift does not need a latent emotion simulator. Characters can express fatigue, concern, irritation, or relief in authored lines. A hidden state is justified only when the current game already models it and a later choice materially depends on it. Otherwise writers should communicate the emotion through observable behavior and choice outcomes.

Do not score a player’s “empathy” from a polite dialogue response. Do not infer guilt from a refusal. Do not make a character’s willingness to share crucial evidence depend on an unobservable emotional variable. The player can be tactful or blunt, and characters can respond, but the system should keep the cause legible.

### Knowledge correction policy

When new evidence corrects a previous line, the graph should offer a short bridge:
- identify the earlier claim;
- state the new source;
- explain what part changed;
- preserve what remains true;
- offer the next decision.

Example: “The note shows that ‘covered’ meant someone accepted the task. It does not show who arrived.” This corrects the interpretation while preserving the roster’s actual text. Avoid a character saying “We were wrong about everything” if only one term changed.

If the earlier line was presented as direct fact and now proves false, the narrative should own that error. The source can explain why they believed it or acknowledge that they passed along an assumption. Do not silently replace the text on revisits and pretend the player never saw it.

### Dialogue graph review methods

A reviewer should trace a graph from several starting profiles:
1. no quest, no discoveries, all characters present;
2. quest active, board inspected, no Annex visit;
3. Annex discovered first, quest accepted later;
4. one witness absent;
5. alternate note found;
6. high skill and high relationship overlap;
7. low relationship and a refusal;
8. corrected information after save/load;
9. unresolved closure with no late callback;
10. prior choice plus a new side quest.

For each profile, record eligible topics, visible response labels, expected source of each fact, effect route, and closure. A condition table should note when multiple predicates overlap. If the reviewer cannot explain why a line appears, simplify the gate or improve the player signal.

### Dialogue context must not leak secrets

Hidden content should not leak through disabled choice text, journal summaries, map labels, response counts, or a character’s generic fallback. If a hidden note has not been discovered, no line should mention an erased name. If a location is only approximate, the UI should not label it with its canonical internal name. If a branch is secret, ordinary dialogue should not reveal that an unseen branch exists.

At the same time, secrecy must not hide a required route. Main content gets reliable entry and fallback. Optional secrets can remain difficult to find, but they are never silently included in progression checks.

### Accessibility and authoring constraints

Dialogue variants should not rely solely on voice acting, color, or facial expression to communicate evidence confidence. Subtitles and text interactions should convey the necessary nuance. If a speaker is visually obscured, their identity should remain available through current dialogue presentation. Focus order should be stable when a gate changes the set of options.

Content authors should separate spoken text, response label, optional hint, and effect summary. A response label is an action statement, not a paragraph. Long explanations belong in dialogue or journal. Choice summaries need localization and screen-reader review. The current UI can limit simultaneous choices; respect that limit by splitting a hub or reconverging branches.

### Minimal viable context layer and optional layers

The MVP supports authored nodes, linear flow, short branches, one hub, simple prerequisite facts, player responses, recognized effects, and a closure path. The Empty Shift MVP uses no more than a handful of facts. Optional layers include relationship-specific lines, skill observations, faction access, visit memory, time windows, corrected-source history, and hidden emotional nuance. Add one layer only when it solves a demonstrated content need and has an owner.

Do not add context dimensions merely because the plan lists them. A maintainable system is defined by a small supported grammar, clear ownership, deterministic resolution, testable fallbacks, and author-friendly documentation.


### Worked context profiles and expected outcomes

The following profiles give authors and implementers a shared review surface. They are not a replacement for the current test suite or a claim that every state currently exists.

**Profile A — clean start.** The player has not seen the board, met Mara, or discovered the Annex. Available content: basic request, general survival topics, map lead after acceptance. Hidden content remains concealed. Expected result: player learns the task without being told what the roster proves.

**Profile B — discovery before request.** The player inspected the board while exploring but did not open a quest. Available content: Mara recognizes the prior inspection only if a current discovery fact proves it; otherwise she offers a fresh inspect action. Expected result: no duplicate reward, no false memory.

**Profile C — single source.** The player has a roster observation but no independent clue. Available content: compare, ask, or close unresolved. Expected result: no option to identify a worker as verified.

**Profile D — second-hand account only.** The player heard that Oren worked the shift from another person. Available content: ask who told them, seek direct confirmation, or mark the report as indirect. Expected result: the journal names a report source if supported and does not convert it to player observation.

**Profile E — direct observation and skill.** The player inspected the panel and has a relevant capability. Available content: technical comparison; general path remains. Expected result: the skill provides detail but does not decide identity.

**Profile F — direct observation without skill.** The player inspected the panel but has no technical capability. Available content: basic description and a witness route. Expected result: the player can continue with less precise evidence.

**Profile G — witness unavailable.** Oren has left. Available content: margin note or second witness. Expected result: personal dialogue is unavailable, but main path remains reachable.

**Profile H — strong relationship, no evidence.** The player knows Oren well but has not inspected any record. Available content: Oren’s personal boundary and a request to inspect. Expected result: relationship does not create factual knowledge.

**Profile I — evidence conflicts.** Roster, note, and testimony do not align. Available content: correction question and unresolved result. Expected result: graph preserves the conflict rather than selecting a preferred source automatically.

**Profile J — branch completed.** The player annotated the roster and saved. Available content: callback based on the actual persisted result. Expected result: no option to apply the same annotation twice; unrelated topics remain accessible.

**Profile K — newer evidence.** A later source contradicts the earlier report. Available content: explicit correction. Expected result: the player sees what changed and can revise where supported.

**Profile L — optional secret absent.** The player never found the folded name. Available content: main quest closure. Expected result: no late scene presumes that the player knows the secret.

### Transition table for dialogue topics

| Current context | Topic behavior | Revisit behavior |
|---|---|---|
| No quest and no discovery | Offer request only through authored source | Keep ambient topics available |
| Quest accepted, no clue | Explain next action | Do not replay opening after every map return |
| First clue observed | Ask for second source | Acknowledge the clue briefly |
| Evidence route blocked | Offer equivalent or show delay | Refresh only when cause changes |
| Character absent | Use safe alternate or omit personal topic | Restore topic when character returns |
| Report choice available | Present clear action and effect scope | Preserve choice availability until selected |
| Outcome applied | Show closure and next useful hook | Do not reapply effect |
| Outcome unresolved | Allow ordinary conversation | Reopen only on a new source |
| New source contradicts report | Explain correction | Keep previous action legible |

A topic can be hidden when it has no useful response. But the whole dialogue panel must not become a dead end. Close/back behavior remains stable and an ordinary topic can still be reached.

### Gate test design principles

Future tests, when authorized, should distinguish content eligibility from effect application. A condition test verifies that a line appears for the correct context. An interaction test verifies that the selected response reaches its current owner. A save round-trip verifies that durable results are restored. A replay test verifies that a repeated interaction does not duplicate effects. These are separate risks and should not be collapsed into one broad happy-path test.

Use focused cases:
- one positive and one negative case for each critical gate;
- overlap case for skill and relationship conditions;
- absent-character fallback;
- location unavailable fallback;
- stale response revalidated at selection time;
- unresolved terminal outcome;
- correction after an earlier claim;
- save/reload between outcome and callback.

Test coverage belongs to the existing owner’s test policy. Do not create speculative test names or assume public APIs before an implementation package exists.

### Authoring rules for response conditions

Conditions should be named for facts, not author interpretation. Use “player inspected the roster” rather than “player is attentive.” Use “Oren is present” rather than “Oren is ready to reveal the truth.” Use “annotation result applied” rather than “player chose responsibly.” These names keep dialogue from encoding moral judgments.

A condition should have one clear source and expected lifetime. If a fact is derived from several owners, document the derivation and prefer reading current state directly through the existing consumer. Avoid hidden Boolean combinations in text labels. If authors cannot explain a gate without using internal abbreviations, the condition model may be too complex.

### Content authoring interface needs

A maintainable authoring surface should help writers see node IDs, source speaker, location, condition, text, responses, effects, and next nodes. It should catch missing references and unreachable nodes where the existing tooling allows. This plan does not propose a new editor until current content tooling is reviewed. A spreadsheet can support review but should not become canonical.

Writers need a readable condition summary and a preview for at least three profiles: no special context, expected context, and conflicting/absent context. If the current tools cannot provide previews, maintain a manual gate matrix with the package. Do not ask authors to infer behavior from opaque code or unrelated UI callbacks.

### Context-layer scope gate

A proposed new context feature must answer: what content cannot currently be authored; which existing owner was checked; what player benefit it enables; what save state it adds; how deterministic resolution works; how authors validate it; and how it can be removed. If the answer is “more dynamic dialogue,” the feature is too vague. The Empty Shift does not require generalized sentiment inference, learned NPC beliefs, procedural conversation, or fully simulated memory. Those ideas remain outside the current scope unless a future proposal proves a concrete need.


### Stale choice and concurrent update case

A response can become stale if a character leaves or a quest closes while the conversation panel remains open. Before applying the response, the existing owner should recheck its prerequisites. If the action is no longer valid, the player receives a brief explanation and the panel refreshes without losing focus unexpectedly. No effect is applied twice.

For example, the player selects “Ask Oren to confirm the handoff,” but Oren departs before the scene processes the choice. The game should not record testimony. It can say he has gone and offer the margin note route. If the quest was already resolved by another action, the conversation should acknowledge the result instead of reopening the evidence stage. This race is a host/lifecycle integration concern and must use the current command contract.

### Hidden and knowledge-gated content

A hidden branch should be discoverable through an authored cue, not a secret Boolean with no player-facing signal. The indentation beneath the old roster can be found by inspecting the reverse side or by comparing paper layers. If the player never performs that action, ordinary dialogue must not allude to the erased name.

Knowledge-gated choices should describe an action the player can take with what they know. They must not assume the player accepts the game’s inferred interpretation. “Ask whether the mark means covered” is safer than “Confront Mara about the false roster.” The latter presupposes both a conclusion and an accusation.

The branch remains optional. Its absence cannot suppress main quest completion, block location selection, or alter the ending baseline without explicit approval.

### Hard-gate audit

A hard gate blocks a required action or information. Each hard gate needs an explicit owner, player-visible reason, recovery route, and save behavior. If its condition can never change in the current playthrough, the quest must not wait on it. Optional gates can enrich or personalize a response without blocking the objective.

Review every critical objective with all optional gates disabled. The main investigation still reaches a terminal result. If the path fails, move the information to a guaranteed source or downgrade the gate to an optional detail.

### Context closeout

Every gated response should preserve three things: the player can understand why it appears, the speaker’s knowledge has a source, and the required story remains reachable without optional context. That is the acceptance standard for future dialogue expansion, regardless of how many additional condition types the architecture eventually supports.

## Pass 15 — Dialogue context snapshot, truthful gates, and remembered actions

**Status: PROPOSAL, premise-gated.** The master world bible identifies delayed callbacks, cohort-tied memory, and under-connected narrative systems as promising lanes. This plan makes context checks legible while reusing current state owners. It does not create a second relationship store, dialogue-history ledger, faction reputation meter, skill tree, quest status, or memory engine.

### 15.1 Correction and collision boundary

Any Empty Shift conversation that remembers a work assignment, roster conflict, or shift outcome must first be compared with DutyRosterSystem and DutyRosterQuestRuntime plus the current duty-roster catalogs. The prior storyline is a DRAFT collision candidate. Do not preserve its own “who remembers the shift” state. For all dialogue, use existing authoritative sources for quest state, actor survival, faction standing, skill, map knowledge, relationship, inventory, and encounter outcomes. A gate is an evaluation of current owner facts, not a new memory.

### 15.2 Context snapshot

At a conversation boundary, gather a read-only snapshot of the minimum facts required to select valid lines. The snapshot should be immutable for that one response selection so a UI refresh does not see half-updated state. It may contain stable IDs and simple values such as current day, location ID, active quest/objective status, a previously recorded choice fact, faction standing band, relationship band, skill threshold, actor availability, location knowledge, item possession, and visit-count facts only when a current owner exposes them. Keep it small; do not copy every system into every dialogue DTO.

The snapshot is not authoritative and is never saved. It is rebuilt on scene entry and after an owner event that changes a relevant field. If a needed fact has no owner or read API, write an integration dependency. Do not silently make a local cache authoritative because it is convenient for the dialogue panel.

### 15.3 Gate types and evaluation order

| Gate | Truth source | Player-facing behavior |
|---|---|---|
| Quest gate | Quest owner/read model. | Offer a line only when the current stage makes it useful. |
| Knowledge gate | Existing journal, clue, signal, or narrative fact owner. | Explain what evidence the player has found, not the internal flag name. |
| Relationship gate | Existing character/relationship owner. | Change tone or disclosure at a defined band; do not invent a new score. |
| Reputation gate | Existing faction stance/standing authority. | Reflect access and trust as that owner reports them. |
| Location gate | Map/discovery and current route authority. | Do not expose a destination as reachable if it is only rumored or blocked. |
| Skill gate | Current skill progression owner. | Add a bonus observation or alternate wording; protect required clues from skill exclusion. |
| Repeated visit | Existing visit/encounter history if exposed. | Use a specific change marker; do not repeat introductory copy forever. |
| Failed quest | Canonical lifecycle and failure consequence owner. | Acknowledge the actual failure and present valid next steps. |
| Prior action | Canonical choice/consequence record. | Remember what the player did, not an inferred motive. |
| Emotional state | Existing trait, morale, guilt, trauma, or phantom-memory owner where relevant. | Use sparingly and never label diagnosis from a single gameplay value. |

Evaluate hard eligibility first, then spoiler/knowledge safety, then access, then optional tone variants. If two gates conflict, the more restrictive requirement wins for the line that contains sensitive information. A failed optional gate should leave another conversational action, an explicit reason, or a graceful exit; it must not leave an empty dialogue panel.

### 15.4 Memory fact versus memory interpretation

A character memory should preserve an observable event: “the player returned with the package,” “the player declined the request,” or “the report omitted the witness.” An interpretation such as betrayal, kindness, cowardice, or loyalty belongs to the speaking character's perspective and may be wrong. Dialogue can show that interpretation, but must not convert it into an unowned global fact.

Prefer an existing source record that already captures the action. Quest status, encounter resolution, consequence ledger, Chronicle milestone, relationship event, or NpcMemorySystem may be relevant; inspect each current API before relying on it. PhantomMemoryEngine is a distinct item-triggered per-survivor system with its own trigger rules and persisted records. It is not a generic dialogue-memory service. Do not put dialogue visit history into it merely because it already contains “memory” in its name.

Use an event fact reference, not copied consequence state. If the event source is later migrated, maintain the reference mapping in the source owner. Presentation can say “you said the route was clear” only if an existing event records that exact line/choice, not only a broad flag like “helped faction.”

### 15.5 Knowledge and information-flow rules

For each character at each conversation node, list: knows; suspects; does not know; cannot know yet; source; and first valid exposure point. A character learns a player action only through a plausible channel—being present, receiving a report, hearing a broadcast, reading a posted result, or being told by a known messenger. Radio reception and location access do not imply perfect comprehension. Delayed information arrives only after the relevant system/event path allows delivery.

Skill checks can reveal an additional clue from observable evidence. They cannot produce a fact that has no in-world source. Faction-gated dialogue may reveal institutional vocabulary or private procedure; it must not become a hidden mandatory gate to story comprehension. Repeated-visit dialogue may reflect wear, impatience, relief, or a new duty, but each version needs a world-state predicate rather than arbitrary “visit number” progression unless a current owner provides that count.

### 15.6 Gate fairness and recovery

When a response is hidden, the player should be able to understand the broad reason and how to act on it, unless explanation itself would reveal a mystery. Avoid false affordances: a disabled response must not look selectable; an apparently selectable response should not fail after commitment because a stale panel snapshot changed invisibly. Confirm the snapshot at command execution and return a clear refreshed result if a requirement changed.

For a required clue, supply at least one non-skill route and one recovery path after optional failure. For faction and relationship gates, show the minimum action that can improve access without promising that a particular answer is correct. If a companion is unavailable, use another allowed speaker, a recorded message, or a different quest path; do not silently replace the character with a new one.

### 15.7 Example gate table for The Long Thaw

These examples remain DRAFT and contain no new IDs.

| Moment | Gate | Allowed response | If gate fails |
|---|---|---|---|
| Registrar asks about the report | Knowledge owner says player saw only one of two records. | Ask where the second copy was made. | Explain that comparison is not yet possible; leave the investigation objective active. |
| Workshop discussion | Inventory/crafting owner confirms substitute part. | Commit it, reserve it, or ask how to source another. | Do not show the consume option; offer a map or trade lead only if one is valid. |
| Route briefing | Expedition owner confirms route reachability. | Choose exposed route or known route. | Mark the blocked route and display the reason from map/weather/faction owner. |
| Witness debrief | Choice record says player preserved uncertainty. | Ask the witness to restate the observed facts. | If the witness is absent, use an authored report only if it contains the same verified testimony. |
| Faction contact | Standing owner reports contact is hostile or closed. | Request a neutral intermediary or leave. | Do not present a free negotiation that the faction owner will later reject. |
| Later callback | A confirmed due event exposes callback content. | Acknowledge, dispute the earlier account, or end scene. | If no delivery event fired, do not expose the callback early. |
| Companion discussion | Current relationship/availability owner supports the scene. | Listen or ask a follow-up. | Do not synthesize a relationship level in UI. |

### 15.8 Dialogue node authoring contract

A candidate node may use the user's suggested shape—ID, speaker, location, conditions, text, player responses, effects, quest updates, relationship changes, world-state changes, and next node—only where the current data schema and effect dispatcher support those fields. The list is not permission to introduce a new node schema. If a schema is missing a needed field, first prove that current encounter, narrative consequence, or quest data cannot express the use case.

For review, every response should state its text key, visible condition, hidden-condition explanation, consequence class, owning system, reconvergence node, failure response, and whether the option is reversible. Condition evaluation must be pure; effects apply only after an explicit player command through the current dispatcher/owner route.

### 15.9 Acceptance matrix

Review with paired cases: same action, different quest status; same relationship, different knowledge; same knowledge, different faction standing; same skills, missing optional item; same clue reached through two routes; first and repeated visit; quest active and then failed; callback due and not yet due; save/reload immediately before response; actor absent after choice; and an owner fact changing between panel display and command. Each case should prove truthful options, no duplicate state, stable saved outcome, readable recovery, and no path to a locked progression dead end.

The plan advances when all gates name concrete read APIs and owners, test data represents valid and invalid states, hidden text has an information-flow review, and performance work is based on measurement rather than speculative memoization. This pass proposes no production gate or memory service and runs no tests.

### 15.10 Gate precedence and snapshot invalidation

A gate evaluator should have a documented order so that one condition cannot leak a line before another condition blocks it. The proposed order is:

1. Verify the dialogue definition and response references are valid.
2. Verify the speaker exists, is available, and can plausibly be at this scene.
3. Verify location and route state.
4. Verify quest stage and terminal status.
5. Verify the player has the knowledge required to understand the line.
6. Verify faction or relationship access for optional disclosure.
7. Evaluate skill-conditioned observations.
8. Select the appropriate repeated-visit or emotional tone variant.
9. Construct the visible response list and a broad reason for unavailable options.
10. On response submission, ask the owner to revalidate any condition that may have changed since the snapshot.

This ordering is a design proposal, not a claim about current code. Hard safety/access conditions outrank optional tone. A skill observation must not reveal a future fact. An optional relationship response must not override a quest-completion requirement. If a character leaves the scene between display and submission, return a clear “conversation changed” result and refresh without applying partial effects.

A conversation snapshot should have a clear invalidation trigger: scene entry; quest event; actor availability change; faction or relationship event relevant to the current node; location discovery/access event; or day/event transition when the node uses a time gate. Avoid polling the whole game state every frame. A snapshot can be cheaply recomputed at a command boundary.

### 15.11 Gate examples with alternatives

**Knowledge gate:** If the player has the first report only, ask where the second was filed. If both exist, ask whether to compare them. If the second is lost, offer a physical retest or close with uncertainty. All routes preserve the core objective.

**Faction gate:** If the player's standing grants access, an official can provide a restricted maintenance record. If access is denied, a public copy, paid intermediary, or refusal branch may remain, but only if those routes exist in current catalogs. Never pretend that a hostile faction will negotiate because one dialogue option was labeled “appeal.”

**Relationship gate:** A trusted companion can explain why a refusal mattered to them; a neutral companion can state observable behavior. The trusted line adds personal context but does not contain a required clue unavailable elsewhere.

**Skill gate:** A trained survivor notices that the gauge face is mounted upside down. Another survivor can still find the maintenance label or compare two readings. Skill changes interpretation detail, not access to the only valid objective.

**Repeated visit:** First entry establishes the request. After a real quest state change, the registrar quotes the updated objective. If nothing changed, return to the same concise hub with a clear exit rather than inventing impatience.

**Failed quest:** If the deadline elapsed, a character can state which window has closed. They cannot claim a community was harmed unless the failure consequence owner recorded that result. The recovery choice is available only if the corresponding route remains valid.

**Phantom memory:** If a scavenged object triggered a genuine PhantomMemoryEngine event, a character may react if the host/narrative path exposes that event to them. A different dialogue service must not replay the phantom text or duplicate morale/guilt effects.

**Callback not due:** Keep the future branch invisible or display a neutral current topic. Do not hint that someone has returned before a valid event has delivered the callback.

### 15.12 Explainable option reasons

Player-facing explanations should say what action is missing, not expose implementation syntax:

- “You have not compared the two copies.” (knowledge/evidence)
- “The gate is closed to your party today.” (route/access)
- “The report is not due for review yet.” (time)
- “No one can take that shift now.” (actor/schedule, only if supported)
- “You do not have enough material to commit this repair.” (inventory/crafting)
- “The witness did not hear that exchange.” (information-flow boundary)
- “The required contact is unavailable; the request remains open.” (actor availability)

Avoid “Requires flag X,” “stat check failed,” or “relationship < 25.” An explanation is not a promise to reveal every secret branch. For a hidden response, communicate the next general action without naming the content.

### 15.13 Memory quality and retention

Remember consequential actions, not every conversation click. Candidate memories include accepted/refused requests, factual corrections, a publicly filed report, promise kept/broken, objective completed/failed, or a witnessed rescue. Casual flavor lines should not add persistent history. Before adding a memory, ask whether a later line or game system consumes it; unused history increases save size and continuity obligations.

When the owner records a memory, prefer a stable event reference and compact outcome classification over a transcript copy. Store exact text only if the player explicitly authored it or the canonical journal already owns the quotation. On old saves, absent memory should mean “not recorded,” not “the player never did this,” unless the migration can prove that meaning. If one event has several interpretations, keep the fact singular and let speaker-specific dialogue interpret it.

### 15.14 Dialogue fairness review

For each important choice, review the option list at low and high skill, neutral and high/low relationship, friendly and hostile faction standing, with and without optional knowledge, active/failed/expired quests, actor present/absent, first/repeated visit, and valid/missing route. Check:
- At least one valid way to exit or continue.
- No essential fact is gated behind an invisible roll.
- Locked responses do not look available.
- Costs are stated before commitment when known.
- The player is not blamed for an owner-level failure.
- A refusal does not accidentally masquerade as acceptance.
- A missing actor does not fabricate a new speaker.
- Reopening the panel shows the current owner state.
- Translation expansion does not clip or hide the reason.
- Controller and keyboard focus order is stable.

### 15.15 Context-read performance

A dialogue panel should request only the context needed by its current scene. Prefer one context build on open plus event-driven refresh to repeated scans over every candidate node on every frame. Resolve IDs through already-built catalog indexes. Do not memoize across a quest or faction change without an invalidation contract. Measure large-catalog scene-open latency, allocations, hidden-condition evaluation cost, and repeated panel open/close behavior. Optimization is a separate evidence-backed task if a measured threshold is missed.

### 15.16 Acceptance evidence

A context-gate integration is reviewable when each gate points to a current API or a named missing seam; each memory is owned and consumed; conditions are deterministic; visible options remain fair under alternate states; snapshot invalidation is correct; no new mutable store was added; and save/load preserves existing owner facts. The focused test plan belongs to the later implementation package and should cover the matrix above without aggregating independent lifecycle cases.


## Pass 16 — Explainable information gates for bible-seeded story threads

The master world bible proposes cross-generational folklore, a seasonal numbers-station arc, and hydrophone mystery. This pass defines how dialogue context can expose those stories fairly while preserving the current owners. The source audit confirms an existing cipher chain state model, hydrophone and folklore discovery references, and cohort/lineage systems. It does not prove a complete record-to-maturation dialogue callback, so that branch remains optional pending a direct wiring trace.

### Separate evidence from interpretation

For all three story families, the context builder should distinguish at least these conceptual states: not encountered; encountered but not verified; verified observation; interpretation available; resolved or explicitly unresolved. Map these to existing quest, discovery, journal, or world-state records only after identifying the canonical owner. Do not add duplicate flags merely to make dialogue conditions convenient.

For a cipher scene, the existing engine’s heard, key-acquired, decoded, and target-revealed progression provides the premise for distinct conversation options. The interface may say “You have the signal sheet” only if the item or state is actually present. Never expose decoded dialogue from a speculative text search or from a target reveal that the save does not own.

For a hydrophone scene, “record collected” does not mean “source identified.” Give the player separate choices to report the observation, ask for a comparison, or make a theory. A skill may expose an additional technical description, but it cannot be the only route to quest progress or basic accessibility context.

For folklore, the record itself is authored testimony. A resident’s interpretation can depend on what the player has heard or inspected. A later-life callback may depend on a verified cohort transition event, not merely elapsed text or a guessed birthday. If no stable owner exposes that event, use a general campaign milestone condition or leave the callback out.

### Gate precedence and explanation

Resolve gate context in this order: content and quest availability; location and visit state; information the player actually knows; relationship or faction access; optional skill flavor; presentation variant. A low-priority tone variant must not override an unavailable quest or present a false memory as fact. If several gates conflict, show a safe, truthful line and preserve the next actionable step.

A blocked option should have an authored reason when the fiction permits it: “I need the decoded line first,” “Bring the second recording back,” or “That room is still sealed.” Avoid empty or disabled choices without context. The reason must not reveal an undiscovered secret. When a requirement is unavailable because a destination failed to spawn, the dialogue should state a clue or a delay through the quest’s canonical owner.

### Relationship, faction, and knowledge fairness

Relationship gates may change confidence or intimacy, not access to mandatory evidence unless the quest has a second route. Reputation gates should name the relevant access condition and offer an alternative where practical. Faction-specific dialogue may carry different interpretation, but the player journal must keep a shared factual record so that narrative perspective does not rewrite an observation.

Skill gates are bonus lenses. Provide a baseline answer that permits progress, and let a skill reveal precision, uncertainty, or a new follow-up question. For audio clues, captions or transcripts should convey the same clue as the sound. For number patterns, a non-color-dependent, screen-reader-compatible presentation should preserve the puzzle’s inputs. A player who misses a limited broadcast needs a replay or an authored transcript route.

### Memory scope and repeated visits

Use current persisted quest/discovery/relationship owners for durable memory. Repeated-visit dialogue should derive from those owners or an existing visit tracker. Do not add a per-NPC memory store if an existing relationship or narrative session owner is responsible. A memory statement must be grounded in an observable prior action: “You brought the sheet” is different from “you solved the case.” Only say the latter when the canonical chain reports resolution.

Failures need memory too, but the line should describe the actual alternate route. After an inconclusive acoustic comparison, an NPC can invite a future sample. After a cipher misreading, the radio keeper can preserve the clue. After an unresolved rhyme, the archivist can keep both versions. These callbacks should survive reload without repeating one-time reward dialogue.

### Context decision table proposal

For each condition, author a row with: data source; truth asserted; visible player knowledge; branch unlocked; branch suppressed; reason text; alternate path; persistence owner; and test evidence. A condition with unknown data source is not ready to ship. Conditions should be pure reads during dialogue selection; effects happen only after the player commits to a response and pass through Plan 22’s approved effect route.

### Acceptance cases

Review first encounter, repeat visit, restored save, failed quest, alternate interpretation, missing clue item, hidden location, reputation-gated option, low-skill option, inaccessible audio presentation, and an unavailable target. The player must never see a factual claim unsupported by state, lose the main route because of a flavor gate, or be told that an NPC remembers an action absent from the saved campaign. This contract is a proposal for integration review and adds no context cache or independent memory authority.

## Pass 17 — Knowledge channels and grief-aware evidence gates

This pass grounds dialogue gates in the master world bible's information-flow canon and applies its loop-closure matrix to burial archives, memorials, micro-locations, and oral lore. The world rule is strict: characters cannot know an event before experiencing it or learning it through an established channel; dead survivors appear only in memories, recordings, memorial rites, and epilogues. The gate model must enforce those rules through existing state owners rather than a parallel per-character memory ledger.

### Evidence provenance for the burial case

The burial record is authored source text. It may distinguish reported death day, burial day, witness, ceremony, and personal effects. The current campaign memorial entry is a live state record. A resident's recollection is testimony. A player's conclusion is an interpretation. These are separate facts with separate sources.

For the cenotaph record, dialogue may confirm that the record says the ceremony took place on a later day. It may not upgrade a second-hand report into confirmed death time unless a witness or source explicitly supports that. A character who attended the ceremony may speak to ceremony details; they may not claim first-hand knowledge of a death they did not witness. A dead person cannot answer new questions outside an authored recording, remembered quotation, rite, or epilogue.

Each dialogue condition should be expressible as a pure read from a canonical quest, discovery, memorial, journal, relationship, or location owner. If the implementation cannot identify the source, the gate is not ready. Do not persist the full conversation transcript as a substitute for domain state.

### Gate categories and precedence

Resolve context in this order:

1. Is the content record loaded and valid for this campaign?
2. Is the conversation or encounter available at this location and stage?
3. Which source records has the player actually discovered?
4. Which facts reached this speaker, through which modeled channel, and on what day?
5. Is the speaker willing or permitted to discuss the subject based on current relationship or faction state?
6. Is there an optional skill lens or presentation variant?
7. Has the consequence already been applied, and what acknowledgement is appropriate?

A later gate cannot manufacture evidence blocked by an earlier gate. A skill check may identify a date convention or material mark, but must not be the only way to ask a basic question. Relationship or faction access may change candor, not rewrite the source record. A player may choose to leave the case unresolved.

### Knowledge-channel contract

Potential channels include direct witness, journal or ledger access, a known radio report, courier delivery, public rumor, and a memorial rite. Use only those modeled and reachable in the current campaign. A rumor may unlock a question, not a confirmed factual conclusion. An archival record can unlock a document-based response even if no character has heard it. If a faction reacts to a correction, its reaction is delayed until a known information channel delivers it; no faction receives global omniscience.

For oral-lore scenes, first-heard state is sourced to an existing producer context and persisted through the oral-lore owner. A character can react only when the relevant song was actually discovered or performed in a known context. A producer listed in DefaultProducerMap is not sufficient proof that a performance callback occurred; trace the live hook first. Do not open a memorial conversation solely because a song record exists in JSON.

For micro-location dialogue, the encounter condition must reflect current parent location, encounter eligibility, and depletion. Knowing the parent site does not mean the player has found its local clue. A depleted encounter may produce a recognition line, but it must not present the hidden clue as still available.

### Explainable blocks and alternatives

Every unavailable question should map to an honest reason or to an intentionally quiet presentation. “I cannot tell you what I did not see” is a character boundary; “the entry is in the archive you have not opened” is an actionable gate; “we do not have a second copy” is an evidence limitation. A gate caused by a missing destination should expose the planned clue or delay rather than blame the player.

Accessibility must carry equivalent information: descriptions for audio-only material, captions for songs or recordings where sound matters, readable labels rather than color-only source categories, and screen-reader-friendly distinction between quoted record and player inference. Long-form records should not be the only way to learn a required clue; provide a concise summary that preserves uncertainty.

### Memory and return visits

Use current first-heard, memorial, quest, relationship, and journal authorities for durable callbacks. A return line should be derived from a saved fact: record reviewed, correction appended, rite performed, encounter depleted, or a source delivered to the speaker. Do not store a separate “NPC remembers burial investigation” boolean if the quest or narrative owner already exposes the same milestone. Do not equate a heard song with a performed song, a discovered grave with a completed investigation, or an opened record with a verified fact.

### Fairness and acceptance matrix

Review a first visit before discovery; a player who found the record but has not shown it to the witness; a rumor without the primary source; a memorial after Mourn; a deceased speaker referenced only through an authored memorial artifact; a discovered parent location with a hidden micro-location; an already-depleted encounter; a song found through travel but not performed in the shelter; an unavailable character; and a restored save. In every case, dialogue must assert only what the stored source supports, keep the main route actionable, and explain a block without revealing the secret. These cases are review requirements for a future implementation, not a request to add a memory engine.

### Pass 17B — Condition truth table for records, encounters, and memory

The dialogue authoring and implementation teams need a shared way to talk about truth without inventing a condition language. The following conceptual table maps player-facing claims to the state required to make them.

| Player-facing claim | Minimum supporting state | Safe response when absent |
|---|---|---|
| “The archive lists a later ceremony date.” | Burial source loaded and entry discovered | Ask where the record can be found |
| “The widow learned about the death later.” | The relevant record or a witness statement is actually known | Call it a report, not a confirmed timeline |
| “The cemetery has an unread mark.” | Parent location visited and local encounter resolved far enough to expose it | Explain that no physical inspection has occurred |
| “The grave was intentionally left without a marker.” | Source record discovered, plus any required testimony if the line claims intent as confirmed | Attribute the claim to the source |
| “I remember you corrected the entry.” | Canonical quest/narrative owner records a completed correction | Say the record was reviewed, or use a neutral greeting |
| “We heard that song together.” | A supported performance event, not merely a catalog record or first-heard ID | Refer only to the player's known first-heard entry |
| “The faction knows what happened.” | A verified information-flow delivery to that faction | Say the information has not reached them yet |

The table is illustrative, not a new runtime schema. Implementers should connect each row to existing API reads and preserve the source when projecting the line.

### Stale-context handling

Dialogue may be opened with a context snapshot and selected later. Between those moments, the player may leave the location, lose an item, complete the quest through another route, or load a different campaign state. Revalidate the consequential condition at response commit. If it no longer holds, keep the option from applying and refresh the dialogue with a concise explanation. Do not apply a command against a stale conversation object.

Read-only gating should not mutate discovery, visit count, or relationship. Any “first time” semantics need a deliberate committed event from the existing narrative owner. This prevents opening and closing a conversation from repeatedly advancing story state.

### Uncertainty vocabulary

Use consistent language for what is known:
- “The record says” introduces a source.
- “The witness remembers” introduces attributed testimony.
- “We confirmed” requires an independent corroborating source.
- “It may have been” introduces an authored hypothesis.
- “We cannot tell” is a valid terminal or interim state.

The UI need not display confidence percentages if the underlying memorial case has no quantitative confidence model. Use source labels and ordinary prose rather than adding a confidence meter. Contradictory evidence should remain visible as contradiction, not be averaged into a fake precision.

### Character availability and grief-sensitive timing

A resident may be unavailable because of a known schedule, health state, relationship boundary, or death. The conversation should then provide a document, another living witness, or a delayed return when possible. If a deceased character's testimony is essential, it must already exist as a recording, journal, memorial quotation, or epilogue text. Do not generate new dialogue in their voice because a quest requires a speaker.

Avoid a gate that requires the bereaved player character to perform a public disclosure. Offer private, anonymous, or no-publication outcomes when the story permits them. These options may differ only in local acknowledgement. A larger relationship or faction change requires a verified owner and a clearly communicated consequence.

### Acceptance walkthrough additions

Review response selection after a campaign save is restored; after a second player source arrives; after the target encounter is depleted; after the witness becomes unavailable; after the player changes from an unresolved to a corrected branch; and after Plan 145 is resolved at campaign seal. The dialogue must read current truth at the time it is shown. It must not reopen a sealed ending or mutate a completed campaign through a late dialogue callback.


## Pass 18 — Explainable context gates for calibration scenes

### Gate model

The Lane B calibration expansion can make dialogue respond to the player's knowledge and to a device's state, but every gate must answer three questions: which current owner supplies the fact, when is it evaluated, and what does the player see when it is false? A hidden gate may change optional tone. It must not conceal the only way to continue a required objective.

The current calibration source supports device registration, assigned survivor, battery level, sensor condition, quality, readings since calibration, overdue state, station reservation and due day, calibration count, last calibration day, and error band. Those are candidate read-only predicates, not permission to copy them into a dialogue-local save object. Skill levels, relationship levels, faction reputation, discoveries, and visit counts must each be verified against their existing owner before use.

### Predicate matrix

- Device registered: enter device-specific lines; otherwise use the generic explanation and do not show a command for an unknown device.
- Overdue: show the calibration warning and reading-count context. If false, use a calm operational line instead of implying danger.
- Battery below the existing reading minimum: show the battery blocker, and offer replacement only if the current command is available.
- Sensor below its existing minimum: show the service blocker with the same availability rule.
- Station occupied through a future day: show the due day and a wait/leave option. Do not promise completion today.
- Station occupied and due day reached: show the completion action only if the owning system reports calibration complete.
- Completion event observed: show completion prose and update the quest once.
- Quality improved: phrase the result comparatively. Never gate “safe” dialogue on a quality score.
- Error band wide: encourage cautious interpretation, not automatic route rejection.
- Prior clue known: optional line can acknowledge the clue if an existing discovery or journal owner supplies it.

Each predicate should have a false-state fallback and a stale-state response. Dialogue opens on a snapshot for display, then the command must re-check all mutable preconditions at commit. This avoids a stale conversation starting a procedure after a battery was consumed or station occupancy changed.

### Skill and knowledge gates

The world-bible wording mentions deterministic skill encounters. The reviewed calibration path contains no skill-check consumer, so the MVP should not make a character skill a prerequisite or substitute for the procedure. If later integration confirms a compatible skill resolver, use it for optional interpretation: a knowledgeable character may explain why a broad uncertainty interval affects a route decision, while an unskilled character receives the same actionable instruction in plainer words. Both branches reconverge before the player commits to calibration.

Skill must not change true dose, calibration device quality, or the success of the existing start/complete transition unless a future signed design explicitly assigns that authority. The skill result should be deterministic under the existing seeded encounter contract and must be visible in the content data. Avoid hidden random failure that consumes a day or destroys a device.

Knowledge gates follow the same principle. A prior report, discovered note, or repeated visit may unlock a richer comparison, but the core action and essential explanation remain available without collecting it. Reputation and relationship gates may alter greeting, trust, or optional disclosure only. If any gate changes access or a quest route, document it as a quest or relationship consequence and route it through its existing owner.

### Memory and repeated visits

Use existing first-heard, discovery, journal, or dialogue-history storage if one is already authoritative for the exact event. Do not add permanent booleans such as has_seen_calibration_warning to the dialogue node or quest instance when another system owns that fact. If no suitable owner exists, make the MVP repeat the concise status explanation; do not invent persistence for polish.

A repeated visit can be useful without new saved state: render the current device state and a short reminder. If historical comparison is later required, specify the exact provenance, lifetime, and save owner. Do not infer that a character remembers a previous reading merely because the player opened a panel.

### Grief, safety, and uncertainty presentation

A line about a suspect reading can implicate an injured survivor or a place connected to a death. Keep evidence language careful: “the record is uncertain” is not “the survivor lied.” The player may see a comparison that separates the remembered value, current device condition, and confidence interval. Avoid presenting a numeric band as a probability unless the source system defines one.

Essential state is communicated in text and numeric labels, not color, icon, or hidden emotional tone alone. Optional emotional variants may reflect known relationship or memorial facts only when those systems are the actual owners. Never use a grief-sensitive branch to add a concealed penalty or block medical care.

### Failure and acceptance cases

- A device disappears between dialogue entry and action: refresh, report the missing device, preserve the quest, and offer the generic route.
- The due day has not elapsed: reject completion and show the remaining wait.
- The reservation is cancelled: return to an actionable state and explain the interruption.
- A save is restored mid-procedure: dialogue reconstructs the status from restored calibration state, not from a duplicate flag.
- A skill or relationship predicate is unavailable: hide only the optional line and keep the core explanation.
- A context predicate is stale: reevaluate before applying any command; no command should be inferred from selected dialogue text.

The review packet lists predicate owner, data type, freshness, false fallback, action revalidation, and whether the line changes availability or only wording. Acceptance requires no dialogue-owned mutable device copy, no mandatory skill gate, no essential clue hidden behind collection, and no new save field before an owner gap is demonstrated. Current implementation or test paths must be claimed and verified separately.


## Pass 19 — Provenance-aware gates for intercepted documents

### Gate principle

A wiretap branch may depend on whether a particular document was found, whether the player read it, whether another source corroborates it, or whether the player chose to submit it. These are separate facts. The current BunkerWiretapEntry contains static source metadata and prose, but no saved read state or validated cast/location references. The source catalog is not currently called from production code. The dialogue plan must not treat catalog availability as player knowledge.

### Predicate register

| Candidate predicate | Authoritative source required | Safe false result |
|---|---|---|
| Transcript exists in loaded data | SignalIntelligenceCatalog plus content validation | Hide the quest entry; report a content gap to diagnostics |
| Transcript discovered | Existing discovery or Codex owner, once a production consumer is confirmed | Show a generic lead only if another current source grants it |
| Transcript played/read | Existing document presentation event; currently no wiretap-specific runtime read event was found | Keep evidence and quest progress unchanged |
| Source copy secured | Current inventory or archive owner, if the content truly gives the player a physical copy | Describe it as a catalog record, not a carried item |
| Clarity metadata visible | Authored audio_clarity_score field | Show “quality metadata unavailable”; do not infer deception |
| Corroborating document found | Exact validated document/evidence link | Offer an unresolved outcome |
| Speaker identity mapped | Current cast catalog mapping | Display the authored identity string with “unverified attribution” |
| Faction mapped | Canonical faction ID and current standing owner | Treat target_faction as source text only |
| Submission accepted | Existing Verdict evidence chain after a successful enrollment result | Keep the finding as a private quest note |
| Location reached | Current expedition destination/discovery owner | Leave the travel branch unavailable or delayed |

Do not add a default score threshold that turns clarity into credibility. If the UI later uses clarity bands, the thresholds must be calibrated against the corpus and labeled as signal quality. They cannot gate whether an allegation is true.

### Gate evaluation and command-time revalidation

At dialogue entry, build a read-only context snapshot from the current owners. Use it to decide which optional lines and actions to show. Before a response applies a command, revalidate its mutable predicates: quest status, document discovery, selected location, faction access, and evidence enrollment eligibility. If a predicate changed, keep the conversation open and return a precise refreshed explanation.

A node that offers “submit” must check the canonical evidence route at commit. A line saying “the copy is sealed” must not create an inventory item unless the inventory owner confirms a transfer. An option that references a witness must recheck that character availability at action time. If a catalog fails to load, do not fall through to a built-in transcript body that diverges from JSON authority.

### Knowledge and skill access

The essential question and the basic provenance explanation are available to every player. A research or listening skill may expose optional technical context only if the existing skills authority and dialogue runtime provide a truthful API for it. Because the reviewed BunkerWiretapEntry itself carries no difficulty or skill field and no consumer has been found, this is not part of the minimum feature.

A skilled survivor might explain that clear audio does not prove the speaker's identity. The unskilled path delivers the same insight through the records clerk. Both paths reconverge before the player chooses to submit, seal, seek corroboration, or defer. Skills can change explanation detail; they do not make evidence disappear, determine guilt, or alter the transcript.

### Memory and repeated visits

There are at least four possible memories: the transcript was discovered, it was played, its metadata was reviewed, and it was submitted. They require separate IDs and clear ownership if the game persists them. Do not compress them into one dialogue-seen flag. Before adding any memory field, identify an existing discovery/Codex/dialogue-state owner and use its established save path. If no owner fits, the first release may repeat a brief source label on every visit.

Repeated visits should reflect current quest state: not yet reviewed, evidence route unavailable, corroboration pending, submitted as lead, sealed, or resolved. The dialogue should not claim that a character remembers a prior conversation unless a current character-memory system supplies that fact. Reopening a page does not count as a new listening event.

### Faction and privacy gates

The current transcript's target faction is a string, not proof of faction access or reputation. A branch can be faction-specific only after mapping it to a canonical faction and confirming a current reputation/access query. If access fails, show a plain reason or a non-faction route; do not make the player infer a missing UI action.

Privacy is not a cosmetic gate if it changes who can be accused or how a document is presented. The MVP can offer “submit as attributed lead” versus “keep private” without creating public disclosure mechanics. If later disclosure has relationship, faction, or world effects, document each effect with its owning system and provide a warning before commit.

### Testing checklist for future implementation

Create focused fixtures for: missing catalog entry, loaded but undiscovered transcript, read event repeated, unknown speaker mapping, unknown faction mapping, unavailable corroboration location, delayed expedition, evidence already enrolled, restored quest with pending submission, and a changed predicate between dialogue opening and response. Check that every false predicate has a safe line and no critical route depends on hidden metadata. No tests were run in this documentation-only pass.


### Pass 19B — Combined-state truth table

Gate combinations must resolve to one clear actionable result rather than a pile of hidden conditions. These cases are proposal-level fixtures for a later implementation review.

| Discovered | Read | Corroboration | Already admitted | Dialogue/action result |
|---|---|---|---|---|
| No | No | Any | No | Do not show the transcript-specific quest. Do not infer discovery from static catalog presence. |
| Yes | No | No | No | Offer playback or exit. Do not count evidence. |
| Yes | Yes | No | No | Offer investigation, private preservation, submission-as-lead, or deferral according to authored rules. |
| Yes | Yes | Yes | No | Show both source references and allow a submission request; do not declare truth automatically. |
| Yes | Yes | Any | Yes | Show the enrolled status and suppress duplicate submission. |
| Yes | Yes | Contradictory source | Yes | Preserve the original entry and offer a separate correction or dispute route. |
| Yes | State unknown after migration | Any | No | Show a generic previously discovered record view; require the player to reread before new actions. |

“Any” means the state is irrelevant to that row, not permission to ignore safety or access rules. If discovery state is not persisted by a current owner, the system must not claim that it can reconstruct the state after save/load. In that case, use an explicit reread action.

### Context precedence

Resolve gates in this order:
1. content validity and canonical ID existence;
2. current player availability and location/access;
3. persisted discovery/read facts;
4. quest lifecycle and branch availability;
5. corroboration and contradiction references;
6. optional skill, relationship, or faction presentation variants;
7. cosmetic voice and repeated-visit text.

A later cosmetic condition must never override an earlier validity failure. A low skill level must never hide a mandatory action. A relationship variant cannot pretend that a document was read. Faction-specific lines cannot substitute for the source's recorded target-faction string.

### Consent and privacy review

The transcript contains named or identifiable speech. Before the player submits it for public consideration, the content needs an authored preview that explains what will be shared. The exact consent or privacy policy must be defined by the game's fictional institutions, not borrowed from a real legal process. Do not imply that anonymization is complete if speaker identities remain inferable from the transcript's content.

If the player chooses to seal the source, distinguish:
- sealing the physical original or copy, which needs an inventory/archive owner;
- hiding transcript text from a public hearing, which needs a presentation/consequence owner;
- keeping the quest private, which needs a quest owner;
- preventing evidence admission, which means no EvidenceLedger effect.

These choices may converge narratively while keeping state effects separate. If an implementation cannot represent a particular promise, do not show that option.

### Review checklist

For each dialogue node, list the predicate, owner, freshness, privacy consequence, command-time validation, fallback text, and whether the action is reversible. The reviewer should be able to answer: can a player enter this scene after restore, can the document be absent after load, can another path admit it first, and what happens if the faction or location changes while the scene is open? Every state race resolves with a refreshed status and no unintended evidence enrollment.


## Pass 20A — Context rules for Verdict theater scenes (DRAFT)

### Gate premise

Part 43, seed 19 of the world bible offers radio theater as a procedural narrative lane. The live VerdictRadioSystem publishes a broadcast event when an authored corpus row reaches its day/phase gate and saves fired IDs. That does not establish which player heard it, whether a radio UI surfaced it, or what the player remembers. RadioProgramProductionSystem owns production jobs and follow-up hooks, not player reception. This plan therefore treats “broadcast fired,” “broadcast surfaced,” and “player reviewed” as different facts and requires a source owner for each before any dialogue condition uses them.

The Quiet Hours conversations need a compact, explainable context model. The initial scene is available from a verified clue route; optional details can react to episode exposure, quest stage, consent, prior actions, skills, relationship, and location. Conditions must be read-only. Opening a dialogue panel cannot accept a quest, mark a message heard, consume an item, or change reputation. The player commits through a response that invokes the appropriate existing command or quest transition.

### Context categories

1. **Authored availability:** day, Reckoning phase, episode prerequisite, location role, and route availability. These should match the authoritative scheduler and map owners rather than copy their values into a dialogue-only clock.
2. **Quest context:** current lifecycle state and completed objective IDs from the quest owner. A dialogue response can offer the next step only when its transition is valid; a missing objective is a content error, not a reason to fabricate a default.
3. **Exposure and knowledge:** whether an episode was actually presented and whether the player opened or listened to it, if those concepts exist as persisted facts. If only the scheduler-fired ID exists, dialogue may say the transmission was sent, not that the player heard it.
4. **Consent and disclosure:** the witness’s authored preference and the player’s confirmed disclosure choice. Consent is not inferred from relationship score or faction standing. Once public identification is committed, the scene must acknowledge that it cannot be undone merely by revisiting dialogue.
5. **Relationship and reputation:** read current values from their present owners. A threshold may reveal an optional personal line or a second explanation; it cannot silently change an accepted quest prerequisite or deny the main investigation unless the design says so explicitly.
6. **Skill knowledge:** a skill may expose a clue about how the script was edited or let the player notice a discrepancy in a document. It must not convert a disputed account into fact without corroboration.
7. **Repeated visit and location:** choose a stable line for the current state. Prefer a derived state key (episode resolved, consent chosen) over an arbitrary visit counter. A visit counter is justified only if an existing memory owner already supplies and saves it.

### Gate policy

Critical information has a minimum route that requires no particular faction alignment, rare item, high relationship, or skill. Optional context can be gated by those facts. Every gate includes a visible alternative, a later retry, or an explicit statement that the branch is optional. Avoid nested opaque checks such as high skill plus reputation plus prior visit plus secret location; those compound a small story beat into an untestable content lottery.

When multiple conditions apply, order them deterministically: terminal quest outcome; explicit consent/disclosure state; active objective; verified exposure; location eligibility; optional knowledge/skill; fallback greeting. This order prevents a generic greeting from masking a valid follow-up. If a condition references content that is unavailable in the current build or save, choose the safe authored fallback and log a content-validation warning in tooling; do not cause a null node or a softlock.

Memory should be proportional to consequence. Cosmetic tone can be derived from the latest quest state. Local scene consequences may use a persisted choice only if the quest owner already persists that choice. Relationship and faction consequences must read and write their canonical owners through Plan 22. World and ending consequences require a separately reviewed authored state transition. This plan does not add a DialogueMemoryStore or an NPC relationship cache.

## Pass 20B — Gate table and save/replay requirements

| Context fact | Source to verify | Permitted dialogue change | Forbidden shortcut |
|---|---|---|---|
| Entry became scheduler-eligible | Verdict radio scheduler | A station operator can mention a scheduled case if surfaced | Assume the player heard it |
| Player reviewed the episode | Current radio/archive reception consumer, if any | Use “you heard/read the case” wording and unlock its optional callback | Store a second heard bit in the quest panel |
| Investigation accepted | Current quest owner | Offer current objective and relevant questions | Accept quest on panel open |
| Witness consent chosen | Authored quest transition/current persistence owner | Change anonymity wording and available disclosure response | Infer consent from affinity or standing |
| Player noticed script discrepancy | Existing skill/inspection contract, if verified | Reveal one extra question or optional note | Auto-validate the testimony |
| Public correction delivered | Existing radio/reception and quest outcome owner | Use follow-up line and truthful journal summary | Assume prepared program was delivered |
| Quest failed, delayed, or abandoned | Quest owner | Explain recovery, alternate route, or closure | Reset state when the dialogue graph is reloaded |

Save/replay cases must include a save before the first scene, after quest acceptance, after each consent choice, after an episode fires but before the player surfaces it, and after the final correction is prepared but before any delivery event. Restoring should reproduce the same available responses and should not redraw a generated witness/site variant. If a radio event and quest transition arrive in one update, their order must be defined by the existing event/host contract. The UI should not race an uncommitted dialogue choice against a day tick.

A compatibility fallback for old saves should be truthful. Missing episode exposure data means “not known to have been reviewed,” not “reviewed.” Missing new quest fields means the quest is not active unless the current quest migration says otherwise. If an old save references a retired episode ID, use a reviewed legacy text fallback or preserve a resolved recap; do not redirect it to unrelated new dialogue.

Acceptance requires a gate truth table covering all available inputs, plus a no-lock proof for the mandatory investigation. It should demonstrate that the player can reach the main witness interaction on a normal path; a low skill, neutral relationship, absent faction membership, missed broadcast, or unavailable optional site only removes optional wording or delays a visit. Every skill-gated line has a non-skill summary route for the core fact. The authored graph must lint for unreachable nodes, conditions that can never be true, overlapping conditions with no priority, and response effects that have no owner. These are content-design gates, not a reason to invent a new general-purpose dialogue engine.



## Pass 20C — Context snapshot examples and gate failure behavior

### Derived context snapshot

The dialogue layer should consume a narrow, read-only snapshot assembled from current owners at node entry. Conceptual fields for this scene are: current quest status; current objective; whether a player-facing episode surface confirms review; the committed disclosure choice if one exists; whether the player has visited the required canonical site; current relationship band if that owner exposes one; relevant skill band if the skill owner exposes one; and whether a follow-up delivery was confirmed. Do not copy all world state into a persistent dialogue context. If no current adapter can answer a required field, remove the gate or mark the story integration blocked until the existing owner exposes it.

A snapshot may be stale after a day tick, return from an expedition, or a quest transition. Recompute it when the player opens or refreshes a node, and revalidate before committing a response. If a condition changes between display and selection, return a clear blocked result and present the still-valid options. This protects against dialogue race conditions without turning the panel into a state authority.

### Wording under uncertainty

When the scheduler fired but reception is unproven, use a line such as: “The station sent a case about an unassigned hour.” Do not write: “You heard the case.” When the player has reviewed an enacted scene, the performer can ask what they thought of it, while a journal summary labels it as staged. If a witness recollection conflicts with the roster, use “The account and the roster do not match yet.” Only after a corroborating source is actually obtained may the line say the record confirms one specific detail. If no review state exists in the current game, all nodes should use neutral language and the quest should open through a different verified clue.

### Gate failures and recovery

- **Episode was missed:** The witness or a printed program can provide the core premise, if those authored routes exist. Optional performance commentary stays unavailable.
- **Player lacks a skill:** A basic follow-up question reveals the critical fact; the skill line adds how the script was edited. Never turn skill into permission to believe the witness.
- **Relationship is low or neutral:** The watch lead provides a short guarded account and requests privacy. A later conversation can add depth after trust improves; basic quest completion remains possible.
- **Faction status is absent:** Use the ordinary shelter route. Faction-specific color may alter the greeting, but no faction membership is required for the main case.
- **A required location is unavailable:** Follow Plan 18’s selected substitute, clue, or delayed state. Dialogue must not claim that the player visited a location they never saw.
- **The player already chose public disclosure:** Remove any response promising anonymity. Present an apology or corrective action if authored; do not silently restore the original consent state.
- **The player revisits after resolution:** Use one outcome-specific line. Do not redraw the quest or repeat a response that would reapply the effect.

### Memory without a new store

Use existing canonical facts wherever possible. A resolved quest already tells the dialogue whether it is resolved; the radio owner can say whether an entry fired; a verified reception owner can say whether it was reviewed; the relationship system can supply current affinity; and a location authority can supply discovery. The dialogue graph composes those values at read time. If a detail must survive independently—for example, a selected witness anonymity option—store it as part of the owning quest instance, not in a new parallel dialogue memory section.

A content author should distinguish “memory” from “history.” A character remembering a player’s action is a presentation of a persisted event owned elsewhere. If the event is absent, write a neutral line rather than fabricate recollection. A later authored update may summarize it, but the source fact must remain attributable. This is especially important for repeated visits: familiarity tone can be derived from a resolved stage; a numerical visit counter is not needed unless an approved mechanic depends on it.

Acceptance checks should pair each condition with an owner, test fixture, true/false wording, and fallback. Require at least one combination test where multiple optional gates are true simultaneously, one test where the same gates change after restore, and one test for missing optional data. Verify that every main quest node remains reachable when all optional relationship, skill, faction, and exposure gates are false.


## Pass 20D — Optional audience scene gate matrix

The Second Margin is available only when the player has a genuine conversation entry route and a valid parent outcome. It should not require the player to have heard a broadcast if they reached the parent by an alternate clue, but it may offer a different opening line. The dialogue graph needs three separate predicates: parent quest resolved; parent outcome value is known; and optional follow-up route is currently available. A single “radio quest done” boolean would collapse important differences.

| Parent context | Follow-up entry | Valid topic | Safe opening |
|---|---|---|---|
| No episode reviewed, no parent clue | Hidden | None | No prompt |
| Parent discovered, not accepted | Available as optional question only if clue surface supports it | What the staged case means | “I heard there is a new performance being discussed.” |
| Parent accepted/in progress | Suppressed or presented as a neutral callback | The investigation, not audience response | “How is the inquiry going?” |
| Resolved privately | Available if existing hub/appointment route is valid | Wording without public delivery claims | “The revised line stayed private.” |
| Public correction prepared, not delivered | Available only if a verified character scene can discuss preparation | Intent, not audience reaction | “The performer has a revision ready.” |
| Public correction delivered | Available | Audience interpretation of actual delivered text | “I read the correction. I still hear the first line differently.” |
| Resolved by silence | Optional private closure only when authored | Respecting the request | “You left the pages alone.” |
| Parent state missing/invalid | Hidden with authoring diagnostic | None | Neutral greeting |

A relationship gate may reveal why the listener is willing to speak at length, but it cannot reveal their identity if they are explicitly anonymous in the content contract. Skill gates may expose that the revised line changes attribution from a universal to a local claim; the basic reading of the correction remains available to all players. Faction status can add a greeting only if a faction-specific role is already established; it must not determine whether the listener is credible.

Repeat visits derive from resolved outcome and follow-up completion. Do not increment a visit counter just to rotate a new interpretation line. If the player starts but abandons the optional conversation, the entry remains available or follows the current dialogue owner’s normal return rule. Reopening the parent quest is prohibited unless an authored new event explicitly changes the parent state. Completing the optional scene cannot rewrite the consent decision.

Missing-data behavior matters. If a public-delivery fact cannot be queried, do not present audience-reaction dialogue. If the quest record has an outcome but the dialogue catalog is missing an outcome-specific node, use a neutral resolved greeting and fail the content gate. If the listener character is unavailable because the current save has no matching role instance, keep the side quest optional and close it only by an authored in-world route; never create a character from a dialogue fallback.

The gate test table should cross all three parent outcomes with three reception states—unseen, surfaced, reviewed—and with optional listener present/absent. That yields a small but useful matrix. Add old-save, missing-field, repeated-visit, and altered-context tests. Verify that the content graph does not offer “tell the station” after a public delivery already occurred, does not display a public listener reaction after a private correction, and does not punish the player for refusing the optional follow-up.


## Pass 20E — Full dialogue availability truth table

Use this table as a content-review aid. “Visible” means a response can be presented; it does not mean its effect may commit without a fresh command validation. “Optional” means it adds characterization or context but never blocks the parent investigation.

| Parent state | Exposure | Consent | Location | Optional response | Required behavior |
|---|---|---|---|---|---|
| Inactive | None | Unknown | Any | Hidden | No character claims the player knows the case |
| Available | Clue known | Unknown | Valid | Ask what the case is | Offer discovery, do not accept automatically |
| Discovered | Not reviewed | Unknown | Valid | Ask about the clue source | Use neutral “sent/staged” language |
| Accepted | Reviewed | Unknown | Valid | Ask what was omitted | Show current objective and consent boundary |
| In Progress | Reviewed | Unknown | Invalid | Visit prompt | Explain unavailable/delayed route; no false arrival |
| In Progress | Reviewed | Private | Valid | Ask about the account | Keep witness identity private |
| In Progress | Reviewed | Public | Valid | Prepare correction | Revalidate consent and audience before commit |
| Completed | Surface unconfirmed | Any | Any | Delivery recap hidden | Do not equate preparation with delivery |
| Resolved private | Private delivery/closure | Private | Valid | Second Margin | Discuss wording without audience-reaction claim |
| Resolved public | Delivery confirmed | Public | Valid | Second Margin | Listener may react to the actual delivered version |
| Resolved silent | None | Declined | Valid | Optional closure | Respect silence; no public response is invented |
| Reopened | New event confirmed | Current value | Valid | New follow-up | Name the new information and keep old history |

### Condition ordering details

Check terminal parent state before optional branch conditions so a stale objective cannot mask a resolved callback. Then check whether the player has an actual entry clue, whether consent was selected, whether an irreversible action has already committed, whether the relevant location/person is present, and finally optional skill or relationship context. Fallback greeting runs only after all specific eligible lines have been considered. Conditions should be explicit and data-testable; avoid arbitrary string expressions or hidden evaluation order.

Overlapping conditions need a declared priority or disjoint predicate. For example, “public outcome” and “resolved outcome” overlap intentionally, with the public-specific line taking priority. “Broadcast fired” and “player reviewed” are not equivalent and should never compete as if they were. The graph linter should flag any pair that can both match but has no priority. It should also identify dead conditions, response nodes with no incoming edge, nodes whose only incoming edge is impossible, and gates that rely on an unstated default.

### Memory and no-lock audit

For every main fact, mark one of: always available; optional skill reveal; optional relationship detail; optional faction greeting; or expansion-only callback. Critical facts belong in the always-available column. A hidden emotional state should be conveyed by authored wording and scene context, not inferred from opaque meters. If a player asks the same question twice, the graph should respond with a stable recap or a brief acknowledgement instead of re-awarding the same fact.

Test a neutral player with no faction, low relationship, no special skill, missed broadcast, and a previously unavailable location. They must still be able to discover the main story through its approved clue, understand the witness’s request, choose among valid resolution paths, and receive an accurate ending. Then test a player with all optional gates true and ensure the additional lines do not skip a required consent step or bypass a location objective. This pair catches both accessibility lockouts and overpowered knowledge routes.

For saved memory, capture the minimum needed durable choice in its owner. Do not save a redundant string transcript of every opened node. A node’s visibility can be derived from quest state, exposure state, and parent outcome. Only if design requires one-time text suppression should the existing dialogue owner consider a seen-node fact; prove the requirement before adding it. Content should remain stable when opening/closing menus and when the language changes.


## Pass 21A — Life-stage, origin-memory, and belief gate contract

### Resolve the age vocabulary before authoring

The source set currently exposes different life-stage concepts. CohortSystem marks a child matured and uses that state for work/duty eligibility, with a default elapsed-day maturation rule. ChildDevelopmentSystem has developmental stages through young adult using its age-days schedule. SurvivorEducationSystem categorizes childhood, adolescence, young adult, and adult graduate by age values. SurvivorAgingProgressionEngine classifies survivor life stage using years and an explicit days-per-year basis. These are existing owners with different meanings and units. A dialogue gate cannot assume that “matured,” “young adult,” “graduated,” and “adult” are synonyms.

Before implementation, the integration plan must identify which source owns this story’s age-appropriate label and how a cohort child maps to the survivor roster. If no single reliable crosswalk exists, keep the scene available through a neutral “cohort member” role and avoid calling the character an adult in quest copy. Do not add a new age calculator to dialogue. Avoid using the 365-day work/duty maturation flag alone as a social or legal adulthood condition.

### Read-only context inputs

The conversation can use: a verified quest stage; the referenced folklore ID; a valid catalog entry; a verified discovery/review fact if one exists; current life-stage wording from its owner; optional location visit state; a current belief profile for flavor only; and a prior quest response if the quest owner persists it. The graph does not store another copy. Morality memory is not a typed folklore-origin link. Absence of such a link yields a neutral line, not an invented childhood memory.

A currently held belief profile can add an optional question such as “Does that account fit the way you see tradition?” It cannot gate the core conversation or determine the answer. The player may ask the teacher and cohort member the same question regardless of profile. A doctrinal/movement-specific node is permitted only when the belief owner provides a stable profile and the authored line is written for that movement; no runtime guessing from a name, creed text, or codex tag.

### Gate matrix

| Fact | Core scene? | Optional line? | Fallback |
|---|---|---|---|
| Folklore catalog row exists | Required for tale-specific scene | — | Use a general childhood-story quest or do not offer it |
| Player has opened the codex entry | No, if alternate clue route is authored | Yes: “You saw the page” callback | Neutral “I remember the story” line only if speaker identity is verified |
| Cohort origin folklore ID exists | No in MVP | Yes: specific memory callback | Do not infer; use generic voice |
| Adult life-stage is confirmed | Required before adult label/story age claim | No | Use neutral cohort role or delay the character arc |
| Belief/movement profile exists | No | Optional tailored question | Generic question |
| Player visited origin_sector map location | No | Optional setting callback | Keep sector as textual provenance |
| One adult is absent | No | Alternative character line | Partial account and later revisit |

### Memory behavior

There are at least four distinct memories: the community’s authored folklore, the individual’s exposure to a story, the person’s current interpretation, and the player’s quest choice. The codex owns only the first. A current journal discovery path may own the player’s discovery. A cohort-origin field would be needed to prove individual exposure; no such field was found in the reviewed DTO. Current belief profile ownership covers the present doctrine, not its childhood cause. The quest owner can remember the player’s response without becoming a general character-memory store.

If a save lacks origin memory, default to unknown. If it contains a nonempty morality-memory string, do not parse it as a catalog ID unless an approved migration proves its format and values. If the active belief profile is unknown or absent, show the neutral branch. Restoring a save must preserve the same authored response outcome while allowing current profile flavor to refresh. This separates persistent consequence from present-day state.
### Pass 21B — Gate freshness and context precedence (DRAFT)

Dialogue conditions should ask for the narrow fact needed by a line. A line about the marked page can depend on the player having inspected that page. A line about a field site can depend on the location visit fact. Neither should depend on a broad “knows folklore” condition that may be true merely because the codex catalog loaded.

#### Context freshness rules

1. Evaluate durable quest and world facts from their current owners at scene entry and again before applying a consequential response.
2. Treat transient presentation context, such as current room or speaker availability, as fresh only for the current interaction. Do not persist it as a story fact.
3. If an actor becomes unavailable between showing a choice and resolving it, return to a safe scene state and preserve the player's prior durable evidence. Do not apply a response effect to an absent actor.
4. Do not infer that a player heard a line because the dialogue node was eligible. Record witnessed dialogue only if the conversation actually advances under the current dialogue owner.
5. Repeated-visit variation should use an explicit visit or conversation count if the current owner provides one. Do not add a local integer to a panel.

#### Precedence when several conditions apply

For this arc, use a stable resolution order: hard availability and safety constraints; completed quest facts; active quest stage; location-specific evidence; character-specific knowledge; relationship or faction gates where already supported; optional tone variation. A later, broader condition must not overwrite a more specific truth. If the player has visited the site but not inspected its clue, use the “visited, clue unseen” response rather than the “field evidence known” response.

#### Life-stage gate wording

The source audit found that CohortSystem maturation is an eligibility transition, while development and education systems expose their own stage concepts and thresholds. Before writing an age-gated line, the implementation owner must name the authoritative stage event. Until then, scenes should use story facts such as “the character has joined the adult work roster” only where that exact fact exists. Avoid the words adult, child, or adolescent as hidden code predicates based on a dialogue-local day calculation.

If stage changes during a long-running quest, determine whether the scene should reflect the stage at exposure time or at conversation time. The preferred narrative rule is to preserve the witnessed childhood account as historical testimony while allowing current dialogue tone to respond to the present relationship. Do not rewrite earlier journal entries when a cohort member matures.

#### Gate review table

| Gate | Required fact | Safe fallback |
|---|---|---|
| Marked page inspected | quest-owned evidence fact | show the initial discovery line |
| First account heard | witnessed scene or durable quest fact | ask the character to introduce their memory |
| Second account heard | separate witnessed fact | offer “I have only heard one account” response |
| Site visited | location owner visit fact | use shelter recollection, with no site claim |
| Reply written | authored completion fact | keep the writing invitation available |
| Relationship-sensitive confidence | current relationship API, if present | use neutral, non-intimate wording |
| Faction-specific audience | current faction access fact | do not expose restricted scene |

Gate failures should degrade into a valid line or a visible unavailable choice, not a blank dialogue node. If the current dialogue engine cannot express this fallback, flag that as a scoped integration dependency rather than hiding it in prose conventions.
### Pass 22A — Return truth, evidence freshness, and debrief gates (DRAFT)

The aftermath arc needs a gate that can distinguish a person who came home from one whose expedition ended in the terminal Failed phase. The quest cannot infer this distinction from an NPC being present in a roster or from a text message saying “expedition failed.” Use the canonical result and actual return facts exposed by current owners.

#### Required context packet

At debrief entry, resolve: active quest ID and stage; linked expedition ID; canonical terminal or completed phase; whether the character/party actually returned; destination ID; whether that location was entered; objective evidence facts; whether the player has already witnessed this debrief; and current availability of each speaker. If any required field is not available through an existing owner, the matching dialogue branch is not eligible. Do not fill the gap with assumptions from prose.

#### Gate table

| Dialogue or objective | Minimum evidence | Do not infer from |
|---|---|---|
| Returning-party debrief | canonical inbound completion / actual returned party fact | terminal Failed event, quest accepted state, or destination selection |
| “Reached the ridge” | location visit or encounter fact | map marker, planned route, or destination ID alone |
| “Route unverified” | return occurred and proof predicate remains false | empty loot alone |
| “Route disproved” | explicit authored evidence that contradicts the route claim | no encounter, weather delay, or missing data |
| Secondhand contact lead | a valid authored contact or current rumor record | an unconsumed rumor catalog entry |
| Follow-up available | selector returned an eligible authored route and party preconditions pass | narrative branch selected or player optimism |
| Repeat debrief | witnessed prior conversation plus current owner repeat policy | panel opened or node became eligible |

#### Condition precedence

Safety and actor availability first; actual expedition return second; quest objective evidence third; prior conversation and selected outcome fourth; optional relationship/faction tone last. A terminal expedition failure must route to its existing failure/missing-person/memorial pathway, if authored, not into this survivor debrief. If completed return exists but the objective did succeed, show success debrief. If return exists and evidence is partial, show the narrower partial or inconclusive wording.

#### Memory lifetime

The stable game fact should be the evidence or choice, not a copy of the whole dialogue transcript. The dialogue owner can retain a witnessed-scene identifier or reuse quest facts if available. The journal may render a concise summary from these facts. Do not persist transient speaker availability, current room, or UI focus. When a speaker later leaves, preserve the player’s already witnessed report; do not revise the remembered testimony.

If multiple expeditions can be linked to the same objective, their result facts must remain distinguishable by stable expedition ID. A second attempt cannot overwrite first-attempt evidence. If the existing quest model supports only a single value, implementation requires a documented model review rather than an untracked local list.

#### Dialogue safety and UX

Show why a route is unavailable and whether waiting, a clue, or closing as inconclusive are available. Disabled responses need an accessible explanation; no meaning may depend only on color. The debrief should not interrupt medical care or other higher-priority shelter flows without a designed scheduling rule. Provide a way to close and return later without losing the current state. Avoid presenting “reputation consequences” in a choice tooltip when no standing owner or consequence contract has been selected.
### Pass 22B — Context permutations and privacy-preserving recall (DRAFT)

Test the scene conceptually against combinations that often create false narration. If two or more conditions conflict, use the narrower factual wording and preserve the underlying event.

#### Context permutations

| Situation | Correct line family | Prohibited inference |
|---|---|---|
| Party returned, no objective evidence, site not visited | travel failed to reach the site | “The site was empty” |
| Site visited, clue inspected, result still short of proof | observation retained; route inconclusive | “Nothing was learned” |
| Site visited, no clue inspected | visited but unexamined | “The clue was found” |
| Objective proof exists but the quest journal is stale | refresh from canonical objective owner | asking the player to repeat a completed action |
| Terminal Failed expedition | existing failure/missing-party pathway | survivor debrief |
| Rumor record exists but no one has intercepted/heard it | no secondhand dialogue yet | “Everyone knows” |
| Rumor was heard but source confidence is low | attributed, uncertain report | verified map state |
| Speaker unavailable | pending conversation or neutral archive route | fabricated quotation |

#### Recall without surveillance

The game should remember durable player choices that matter to the quest, but the dialogue need not simulate a complete transcript history. Store or reuse a compact fact such as “player selected inconclusive record” only if the quest owner already has a place for it. Avoid tracking how long the player hovered, whether a line was merely highlighted, or whether the player opened a panel. These events are not meaningful story consent.

When a character recalls a prior action, identify it in ordinary language: “You left the end of the route open.” Do not claim that a character remembers an unselected response. If the player changed the conclusion through a later valid step, acknowledge both the earlier uncertainty and new evidence rather than rewriting history.

#### Staleness and invalidation

Before rendering an offer to launch another expedition, recheck location eligibility, active party requirements, and quest stage. If a destination becomes unavailable after the player opens the panel, the command preview should fail truthfully and provide a return-to-debrief path. Avoid stale dialogue actions that point at retired IDs. When an optional rumor expires under its owner’s policy, remove only the rumor-based response; retain direct expedition observations and the quest conclusion.

#### Privacy and accessibility

Keep private testimony out of public faction dialogue unless the player explicitly authorizes a sourced report and the information-flow owner confirms dispatch. A private journal choice does not equal public disclosure. State that distinction before confirmation. The dialogue UI should expose the same choice information to keyboard, controller, screen reader, and pointer users. If an action is irreversible under an approved rumor system, include a clear confirmation that names the audience and source attribution.
### Pass 23A — Rumor knowledge states and listener gates (DRAFT)

Rumor dialogue must distinguish possession of a text entry from knowledge of its content, and knowledge of a report from belief in its claim. Current rumor records include an intercepted flag and reached hub IDs. These do not establish that a named character personally heard the rumor, understood it, repeated it, or believes it. The conversation gate must use only facts supported by an existing consumer.

#### Proposed knowledge ladder

| Knowledge state | Meaning | Eligible dialogue | Persistence question |
|---|---|---|---|
| Catalog-defined | authored content exists | none by itself | static data only |
| Generated at origin | rumor record exists | origin-side scene if surfaced | RumorSystem currently saves record |
| Reached hub | hub ID appears in reach list | location briefing if player accesses that hub | existing saved record |
| Intercepted | canonical interception fact is true | player may discuss intercepted wording | existing IsIntercepted field |
| Personally heard | a character or player witnessed a line | memory-sensitive conversation | no per-person rumor hearing field verified |
| Investigated | quest-owned checks were completed | bounded result dialogue | requires quest evidence facts |
| Believed or acted on | character/player adopts a claim | only if a current social owner supports it | do not infer from hearing |

If the system does not track personally-heard or believed state, do not create new character memory just for this story. Use direct current dialogue with an attributed speaker and avoid later recall claims.

#### Conditional line contract

An ambient-only rumor line is available only if the hub has an approved ambient-only policy, the rumor is tagged with the approved source-free provenance, and the player reaches the existing presentation point. A personal recall line additionally needs a real witnessed-conversation fact. A source-check result line needs a quest fact that identifies what was checked and the interval. If any predicate is unavailable, fall back to the initial attributed report.

Truthfulness and hub credibility may shape a neutral descriptor such as “low-confidence report” only if the UI contract defines that wording. They must not be substituted for belief, consent, or event confirmation. Because current briefing logic marks values at or above 0.80 as verified, any new ambient class needs an explicit override or separate presentation path before a high-confidence ambient item can enter the briefing.

#### Staleness and repeat visits

RumorSystem can remove stale entries after time/decay rules. If the rumor has expired, a previously witnessed conversation remains a remembered conversation, but current availability and propagation claims should not be shown as live. The story may render “you heard a report” from a durable quest fact; it must not promise the rumor remains in the network. If per-person hearing is absent, do not give companions personalized repeat-visit dialogue about it.

Repeated visits should not manufacture a growing chorus. A second identical prompt is available only if there is a new account, an actual propagation event, or a later authored consequence. Otherwise the hub falls back to ordinary location dialogue. No wall-clock timer or random day threshold should be introduced for the story.

#### Consent and source disclosure

Before the player confirms forwarding a report, state who will receive it and that its source is unknown. If the current host lacks recipient-level delivery receipts, do not claim a named hub received it. If the player says “do not pass this on,” store that as a durable quest fact only if the quest contract supports it; do not infer a systemwide information embargo. Consent to share testimony does not authorize naming a witness unless the authored choice says so.
### Pass 23B — Gate truth table, expired content, and knowledge repair (DRAFT)

The narrative should have a safe line for each partial state instead of treating every missing condition as a hard error. The presentation may degrade gracefully, while the quest remains truthful about which knowledge is absent.

#### Gate truth table

| Ambient policy | Rumor record | Heard by player | Search facts | Available line |
|---|---|---|---|---|
| absent | any | any | any | no ambient quest entry |
| approved | absent | no | none | no rumor claim; ordinary hub scene |
| approved | present | no | none | “A report is available” only where actual delivery supports it |
| approved | present | yes | none | attributed account and investigation invitation |
| approved | expired | previously heard | saved account fact exists | historical testimony, no live circulation claim |
| approved | present | yes | partial checks | partial, scoped result |
| approved | present | yes | full checked interval | bounded report conclusion |
| unknown/invalid | present | any | any | hide ambient branch; authoring/runtime diagnostic |

#### Knowledge repair path

If a player reaches the conversation through a stale map marker but the rumor is gone, refresh the owning projection before showing rumor-specific dialogue. If the authored quest already stores an account the player witnessed, use the historical line; otherwise return to a generic hub greeting. Do not recreate the expired record to satisfy dialogue. If a save references a missing hub ID, preserve the player’s existing quest facts while suppressing only the invalid hub branch.

#### Skills and reputation

Skills can change which questions the player asks only when the current dialogue API has a supported, explainable skill predicate. A skill may reveal that a log is incomplete; it cannot generate a new source. Reputation or faction alignment may affect who will speak only if a current owner can answer that gate. A low standing score must not make all witnesses lie, and a high skill must not turn an absence of evidence into proof. The player should understand why a knowledge-gated response is available through established feedback.

#### Hidden emotional state

Characters may sound unsettled, tired, or defensive in prose; do not infer a hidden clinical state. A character choosing not to discuss a rumor can be a consent boundary, not an illness symptom. Any persistent emotional arc belongs to the established relationship or psychological owner and needs its own approved premise. These dialogue branches must not alter treatment, stress, or diagnosis.
### Pass 23C — Context edge cases and honest feedback matrix (DRAFT)

The player should receive distinct feedback for content absence, blocked access, expired information, and completed investigation. These should not all collapse to a silent unavailable choice.

| Condition | UI/dialogue feedback | State behavior |
|---|---|---|
| no hub has ambient policy | no quest card; ordinary hub presentation | no quest instance |
| authored row fails validation | omit row and surface integrity diagnostic outside player flow | no malformed record saved |
| rumor reached hub but player has not visited | no personal-hearing line | record remains at hub under rumor owner |
| player visited but rumor already expired | say the lead is historical only if witnessed state exists | no regeneration |
| source check unavailable | explain which check cannot be performed | quest may remain pending or resolve by choice |
| player closes dialogue | return safely to hub | no effect applied |
| player declines forwarding | confirm private choice | no propagation command |
| recipient delivery fails | say it was not sent | preserve local testimony only |
| new evidence arrives after closure | re-open only by explicit quest rule | preserve earlier bounded conclusion |

#### Feedback truth policy

Use “unavailable” when a requirement is unmet, “not found” when a search was performed with no result, “unknown” when no search can establish an answer, and “not sent” when a delivery command fails. Avoid ambiguous “nothing happened.” The player must be able to tell whether the system lacks content, the character lacks knowledge, or the investigation returned an inconclusive result.

#### Accessibility and localization review prompts

Can the player distinguish source-unknown from low-confidence with text alone? Can the player revisit the evidence label without replaying the whole dialogue? Does a confirmation name the audience and the witness attribution? Can all choices be reached and closed with a controller? Are “two notes,” “third bell,” and “no recording” localizable without relying on English wordplay? If any answer is no, revise presentation before adding more content.

## Pass 24A — Context gates for earned, reported, and restored entries (DRAFT)

### State distinctions

This pass applies the world-bible Part 46 field-guide trigger question to dialogue memory and gate truth. Keep these states distinct: catalogued means a definition exists; selected means a location/encounter was eligible this expedition; surfaced means the player received the encounter; resolved means a choice was accepted by Core; unlocked means the existing field-guide state contains the ID; displayed means Codex UI rendered it; and saved means the existing persistence owner captured it. None implies all the others. In particular, a restored unlock is not a fresh observation, a loaded catalog row is not player knowledge, and a choice result that contains an ID is not proof the host applied it.

The six Plan28 ecology entries already have a direct observation mapping through `WildlifeSeasonalCalendar.FieldGuideEntryFor` and `UnlockFieldGuideObservation`. Preserve that path. The reviewed travel bridge appears to preserve other consequences while omitting the guide ID, so dialogue must not offer a “you learned this from the encounter” gate until the host route proves the unlock happened. This is a premise gate for future work, not a request to add dialogue memory to `FieldGuideState`.

### Predicate ordering and truthful fallback

Evaluate gates in a stable, explainable sequence: (1) relevant entry ID exists; (2) current owner says it is unlocked; (3) source/quest context exists if the line names a source; (4) optional relationship/reputation/skill condition is checked through its own owner; (5) the selected line is rendered. If any optional context is stale or missing, fall back to the neutral line, not a fabricated memory. If the entry is unlocked but provenance is not saved, say “you have the entry,” not “you saw it yesterday.”

### Gate matrix

| Context | Allowed line | Denied/fallback | Prohibited inference |
| --- | --- | --- | --- |
| Catalog row only | Generic Codex availability text | Empty-state copy | Player has seen it |
| Valid unlocked ID | “Your guide includes…” | Generic line if entry missing | Encounter caused it unless recorded/verified |
| Direct ecology event | Observation-specific acknowledgement | Neutral entry line | Player personally saw it if event is only aggregate migration |
| Travel choice result | Source-aware line after owner confirms unlock | “The report is not in your guide yet” or neutral | Successful Core result automatically means persistent unlock |
| Save restore | Returning-player acknowledgement | Neutral line if UI lacks restore context | New journal event/reward |
| Invalid ID | Diagnostic/non-player-facing fallback | No source-specific line | Substitute by display name or nearest species |

### Memory and repeat-visit rules

Repeated-visit dialogue may react to stable state only. Prefer the existing quest/journal/field-guide owner as the predicate source. Do not add a free-text “heard this” field or a second dialogue-memory save section. If the player has a previous report but no supported provenance record, the scene can repeat safely without claiming it remembers the exact report. Hidden emotional states should be expressed through authored tone or an existing character-state owner, not a new boolean attached to this field-guide feature.

## Pass 24B — Stale-context, denial, and failure response contracts (DRAFT)

A quest may be failed, abandoned, reopened, or postponed while its guide entry remains unlocked. Dialogue must query quest lifecycle and field-guide unlock independently. A failed quest should not erase knowledge; an unlocked entry should not mark a quest complete unless its objective explicitly requires that ID. If a player chooses to keep a location private, future lines may honor that only if the choice was routed to an existing location-access or privacy owner. Otherwise it is local scene tone and must not be presented as a world rule.

Reputation gates affect who will discuss a report, not whether the Codex hides a legitimately earned entry. Skill gates may reveal a more cautious interpretation, but the ungated route must remain understandable. Faction-specific dialogue may dispute the meaning of a mark; it cannot rewrite a canonical entry or silently change faction standing. The player needs a direct response that distinguishes dispute from system truth.

Denial copy should state the missing condition when it is safe and useful: “No entry is attached to that report yet.” Do not expose internal IDs, trigger-token names, save details, or validator diagnostics in player-facing text. If the host cannot establish the condition, choose neutral dialogue and log the discrepancy for development diagnostics. All context checks must be null-safe and robust to old saves with no optional provenance.

Future focused acceptance should exercise: unlocked by observation; unlocked via resolved travel choice; invalid/missing entry ID; restored unlock; quest failed but entry retained; entry unlocked with no source record; faction/relationship gate denied; and repeated visit. Verify each branch's visible line and absence of unintended effects through the existing owners. This is planning only; no tests were run or proposed as speculative new coverage files.

## Pass 25A — Explainable gates for forecasts, warnings, and route choices (DRAFT)

### Context contract

Part 46’s mid-route storm subject requires careful distinction between what the player knew at dispatch, what changed later, and what the active sortie actually experienced. Current source evidence includes dispatch-time weather inputs and a weather-gate force operation; it does not yet establish one persistent in-route warning/response record. Dialogue conditions must therefore use only verified owner state. A weather forecast row is not the same as the player having seen it; a surfaced warning is not proof the survivor received it; an active-front multiplier is not proof the expedition encountered that front; a forced gate is an explicit action with a known cost, not an implicit choice.

### Gate order

For a “you were warned” line, check in this order: (1) the forecast/window is valid for the relevant day; (2) the player had access to the forecast surface before dispatch; (3) dispatch estimate recorded the relevant weather inputs or gate status; (4) the sortie was in an eligible phase when an actual transition occurred; (5) the warning was surfaced before the relevant outcome; (6) the player had a real response command; and (7) the outcome owner recorded the response. If any stage cannot be verified, use neutral wording: “The forecast expected a change,” not “You ignored the warning.”

### Gate table

| State | Permitted dialogue | Fallback | Forbidden claim |
| --- | --- | --- | --- |
| Forecast exists; player never opened it | “The day’s forecast included…” only in reference copy | “Conditions changed during the sortie” if proven | “You were warned” |
| Forecast viewed before dispatch | Explain the forecast that was available | Neutral estimate language | Forecast accuracy guarantees |
| Gate blocked at dispatch | Name the route condition and offered force cost | Existing block feedback | Mid-route storm hit the team |
| Gate forced | Acknowledge the player’s explicit choice and cost | Neutral route line | The team chose a reroute |
| Storm window active during sortie, bridge unverified | Use debrief only if the sortie result confirms exposure | Generic debrief | A mid-route warning or choice occurred |
| Active-route warning and response recorded | Explain timing and selected command | Neutral if one timestamp is missing | Retrospective blame beyond recorded facts |
| Save restored after trigger | Continue from persisted owner state | “The report remains open” | Replaying warning or choice |

### Relationship, faction, and skill context

A survivor may resent being sent during a forecast window only if a valid relationship event owner receives that choice or the existing quest state explicitly describes the issue. Faction access may change who provides a route report, but it cannot rewrite the weather state. A weather-aware skill line can explain a local sign only if an authoritative skill check exists. Never invent a confidence percentage in prose or use high trust as proof that a forecast was correct.

## Pass 25B — Timing freshness, retries, and privacy (DRAFT)

Use campaign day and the actual expedition tick/order for freshness. Avoid “yesterday” when the outcome payload only stores a day and multiple ticks occur within it. If no hour or event sequence exists, dialogue can say “before departure” only when the host records that order; otherwise use “the report you saw.” A stale forecast should retain historical truth—what was predicted at the time—while the current map/dispatch control displays current passability.

When a quest is blocked by a missing in-route hook, present an honest dormant state in the quest log only if the existing lifecycle can represent it. Do not repeatedly prompt the player to make an unavailable choice. A later update may reopen the branch if a real owner event arrives. Failed, abandoned, reopened, or completed quest status stays separate from route state: a closed quest does not imply that the weather danger ended, and a still-unlocked destination does not imply a passable gate.

If survivors provide sensitive testimony about another team member, gate who sees it using existing journal/privacy rules. Do not reveal medical details on a faction radio channel merely because a storm scene needs stakes. Refusing to answer can be a local dialogue consequence; any persistent trust/privacy change requires an explicit owner and consent review.

A future verification matrix should vary: forecast opened/not opened, forced gate/no force, outbound/looting/inbound, warning before/after result, save/restore in the interval, failed quest/active quest, relationship low/high, source known/unknown. Each case must show the exact line and the predicate evidence behind it. No dialogue may claim an intervention existed if the game offered none. This pass proposes no code or tests.

## Pass 25C — Contradictory reports and explainable memory tiers (DRAFT)

A storm quest can produce contradictory evidence without creating a contradiction in save state. Store only facts supported by their owner: forecast viewed, dispatch choice, route/phase, returned testimony, and committed objective result. Interpretation is a presentation layer that can say “the two times do not match.” It should not write `storm_was_misreported=true` unless an authorized investigation result actually establishes that conclusion. A player hypothesis remains a hypothesis.

### Memory tiers for character dialogue

- **Observed:** a current event owner says the character directly saw a mark or physical condition.
- **Reported:** the character received a message or debrief, with source type known.
- **Inferred:** the player or character connects two records; display this as an interpretation and keep it revisable.
- **Unconfirmed:** report is incomplete, stale, or source-less.
- **Restored:** state is loaded from save; it must not sound like a fresh event.

These tiers are a content vocabulary, not a new global memory enum. If existing state cannot distinguish them, use neutral copy and do not fake persistence. Inference should be possible only when the relevant evidence can be revisited and does not depend on a transient UI panel.

### Consequence gates

A relationship consequence can require both a player choice and a later witnessed response. Faction access can require a recorded delivery, not merely a conversation about it. A quest-completion line can require the result owner to confirm the objective, not just that the destination was selected. An ending callback may refer to an unresolved storm report only if the endgame owner consumes that specific state. This keeps late-game narrative from treating an unverified suspicion as canon.

### False-positive checks

Test authoring should reject lines that use “you knew,” “you abandoned,” or “you sent them to die” without predicates for forecast access, actual intervention, and resulting state. A player who saw a forecast but had no action choice did not “ignore a warning.” A player who used a force gate accepted its listed cost, not every later weather outcome. A survivor’s incomplete testimony is not proof of deception. A neutral fallback is always preferable to an emotionally coercive but unsupported accusation.

## Pass 26B — Context snapshot, memory lifetime, and stale-fact rules (DRAFT)

Contextual dialogue should receive a read-only snapshot assembled at the time the interaction opens. The snapshot contains only the facts required by the candidate scene: location identity, current quest states, relevant relationship/faction readings, known evidence IDs, prior response IDs that are explicitly remembered, and current campaign/expedition phase. Each fact carries an owner and freshness boundary. The dialogue system selects lines from this snapshot; it does not become the owner of those values, update them opportunistically, or retain a second copy after the conversation closes.

### Memory has an explicit lifetime

- **Scene-local:** a selection controls the current exchange and is discarded on exit.
- **Quest-local:** a response is remembered by the quest authority because a later objective or acknowledgement depends on it.
- **Character relationship:** a durable relationship change belongs to the relationship owner and should be expressed through its supported values or events.
- **Faction standing/access:** reads from the faction owner and changes only through its command/event contract.
- **Campaign/world:** a persistent fact can change future location, resources, or events only through its canonical owner and registered save path.

Do not persist a player response merely because a later writer might want to mention it. First demonstrate a later scene that reads it and explain how old saves treat the absent value. Do not use dialogue visit counts as a disguised trust meter. A repeated-visit line can be selected from an existing quest transition or a narrowly scoped scene-completed fact if such a fact has an approved owner.

### Gate priority and fallback

Gate evaluation is ordered for explanation, not hidden advantage. First confirm that the node’s location and scene are available. Then check hard quest prerequisites, campaign phase, faction/relationship conditions, and knowledge/skill alternatives. If several responses are legal, show them with clear labels. If a gate closes every mandatory continuation, the conversation must surface the missing requirement or route the player to another reachable action. Hidden lines are appropriate for optional flavor, not for content required to resolve an active quest.

A missing or stale input evaluates to unknown, never to success. Unknown relationship data cannot unlock a high-trust line; missing evidence cannot be treated as evidence absent; a stale weather sample cannot describe present conditions. The copy should distinguish “not recorded,” “not known,” and “false” wherever the distinction affects the player’s decision. The scene can still provide a neutral response and remain usable.

### Time and event boundaries

Dialogue opened during an expedition uses the expedition snapshot and cannot read a later campaign result. A debrief uses a committed return/result fact, not an in-flight estimate. If the world changes while a panel is open, the active scene remains internally consistent; refresh or reopen only at a defined interaction boundary. Triggering a quest from dialogue must not advance campaign time unless the accepted command already does so. Repeated UI redraws cannot reevaluate a random line or repeat an effect.

### Testable context fixtures

For each context-dependent scene, authors supply a small truth table: base context; each gate individually true; each gate false; missing owner data; stale context; both mutually exclusive facts present due to migration; repeated visit; and save/restore if the dialogue choice is durable. Expected outputs identify node/response eligibility, displayed explanatory text, and allowed effect. These are design fixtures to map to the actual data/test architecture, not a request to create speculative test files. They prevent a prose revision from silently changing eligibility and prevent an API rename from leaving a line unreachable.

### Performance and authoring limits

Context should be assembled once per interaction from bounded owner queries. Avoid scanning every quest, faction, or journal record per frame. Candidate nodes are indexed by location or scene entry where supported; expensive checks occur at interaction start, not on every panel refresh. Content authors should not write conditions as opaque compound expressions. Named, validated predicates are easier to localize, audit, and cache. If the current dialogue consumer cannot handle this query model, the minimum viable approach is a small set of pre-resolved scene variants routed by the existing host, with the same owner boundaries and no new persistent cache.

## Pass 27 — Evidence and knowledge gates for an industrial craft mystery (DRAFT)

“The Last Dry Strike” distinguishes access to a conversation from authority to interpret an assay. A player can inspect the surviving note without a special skill. Expertise can add a useful question—such as whether the recorded interval is long enough to support the word stored—but cannot convert absence of data into a positive result. This keeps skill gates additive rather than punitive and avoids creating a new quality or chemistry skill.

### Gate table

| Gate | What it permits | What it does not prove | Fallback |
| --- | --- | --- | --- |
| First record discovered | Start the investigation | That a second test exists | Ask the archivist where duplicate sheets were stored |
| Second record available | Compare the two test conditions | Batch identity beyond the page’s own label | Preserve both reports and mark the link uncertain |
| Relevant craft knowledge | Ask a more precise follow-up | That the player can reproduce a recipe | Give the same three report choices without the technical line |
| Delivery mark actually read | Ask about the transfer shelf | Who personally carried the box | Accept incomplete chain of custody |
| Character previously met | Use a familiar greeting | Relationship change or trust increase | Neutral greeting |
| Expedition result returned | Acknowledge what was actually brought back | That the item was used or survived storage | Use the committed result only |

The specific skill ID, if any, must be checked against the current skills catalog. Do not use a profession label or trade-specialty tier as an implied knowledge predicate if the owner does not expose that fact to dialogue. A missing skill definition leaves the technical line as optional authored exposition, not a hidden requirement.

### Knowledge states and provenance

Track only knowledge the player actually encounters through a current discovery or quest authority. Catalog loaded, location visited, record opened, record understood, and player selected an interpretation are separate concepts. If the present owners cannot distinguish them, the dialogue must not pretend to offer a persistent knowledge-gated branch. It can instead use explicit scene sequencing: a response reveals a technical explanation now, with no later memory claim.

The first note is firsthand about an immediate test. The second is firsthand about a later storage test if its author performed that test. Iven’s recollection is observational but cannot identify the carton. The player’s conclusion is an authored interpretation, not a discovered historical fact. The archive can remember which report label the player selected only if a quest owner persists that response and a later callback consumes it.

### Freshness and return behavior

Once a conversation begins, its context is a consistent snapshot. If an optional expedition is completed while a scene is open, the current scene does not retroactively gain the result. On the next interaction, dialogue may offer a debrief response if the expedition owner has committed a return fact. Reopening a node after the report decision shows a short acknowledgement rather than replaying the same choices as if no decision occurred. A reload with no saved quest state returns to a neutral state and cannot claim that the choice is remembered.

### Missing and contradictory data

Missing record ID, missing author, or unresolved date must evaluate as unknown. It cannot unlock the most confident conclusion. If migration reveals both an old complete status and a newer unresolved report, the quest owner needs a deterministic precedence rule before dialogue consumes either value. The scene should say that the record is incomplete, not that the player failed a hidden check. Conflicting testimony remains visible as conflict; do not select the more dramatic witness by random weight.

### Truth table for minimum acceptance

- Neither record discovered: the quest scene is unavailable; neutral archive dialogue remains usable.
- First only: inspection and clue request are enabled; publication is not required.
- Both records: all three report labels are enabled.
- First plus craft knowledge, no second record: technical context is available, certainty remains bounded.
- Second record available but first missing: show the second record as a standalone discovery; do not infer the missing first result.
- Player refuses to publish: quest resolves or remains open according to packet choice, with a clear journal state.
- Character absent or dead: remove personal acknowledgement; do not replace it with a fabricated speaker.

Performance remains bounded: resolve scene context on open from narrow owner queries. No all-quest scan, repeated per-frame gate evaluation, new save cache, or general-purpose dialogue expression language is justified for this slice.

### Pass 27B — Gate composition and stale-context walkthrough (DRAFT)

Context should be composed from a small set of owned facts at interaction-open time, then held stable until the player exits the scene. The story has no need for a generalized condition language. A scene resolver can select a base entry line, expose records actually discovered, and select a callback from the quest owner’s committed report outcome. If any current consumer cannot expose those facts, the gated branch remains proposed and the scene uses neutral copy.

**Case 1: the player has a relevant craft capability but no second source.** The capability may unlock one question about what a test interval means. It does not enable comparison or a conclusion. The journal says the second source is not found. No skill failure screen appears.

**Case 2: the second source is read before the first.** The record can be inspected independently. The UI must not call it “the retest” until the first record’s batch relation is established. After the first record is found, the quest can present both with the relationship marked as claimed or verified according to source data.

**Case 3: source relation is uncertain.** The scene distinguishes “same batch written on both slips” from “same carton delivered.” Only the first can be asserted from matching text. The second requires a chain-of-custody link. The dialogue gate may expose a question about the gap, but cannot grant a stronger conclusion because the player has a high relationship level.

**Case 4: the worker is unavailable.** A dead, absent, or unspawned speaker cannot provide a memory. If an authored copy survives, the player can read it and get only the claims present on the page. Another survivor cannot be assigned the missing person’s voice as fallback.

**Case 5: player reopens after a decision.** The quest response ID selects the corresponding short acknowledgement. A return to the hub does not reset the decision menu. If the state is not persistent, the system must not pretend the archive remembers it across reload.

**Case 6: quest record references an obsolete response.** Migration or validation maps it to an explicit neutral archive state, not to the most rewarding branch. There should be no preference for “published” over “preserved” merely because the label is newer.

### Knowledge, relationship, and faction gates stay separate

A discovered note can unlock a knowledge line without changing how Mara feels about the player. A relationship fact can change a greeting without proving that the second record exists. A faction standing value cannot rewrite the content of the note or automatically unlock the conclusion most favorable to that faction. If an external group asks for a copy, its access policy is a separate scene and consequence contract.

The story may include hidden emotional states as performance direction—Mara taps the eraser twice before answering—but the condition for that line must be authored in a way the dialogue system can actually evaluate. If no persisted emotional owner exists, use one scene-local beat, not a cross-campaign memory flag. Failed quests can receive a concise acknowledgement only when failure was recorded by the quest owner. Abandoned and failed remain distinct.

### Freshness and observability

The interaction panel can show a source badge: “read from file,” “heard from Iven,” or “player interpretation.” This is a presentation proposal and must reuse the current journal/source convention. It should not reveal the entire condition formula. After an accepted report choice, show a visible “Report saved” or “Draft left open” result only if the command confirms it. For a narrative-only draft, say “You leave the page on the desk”; do not display a persistence icon or completion receipt.

Context query cost is bounded by using one scene context snapshot and a small number of direct owner reads. No timed loop should reevaluate conditions while the player is idle. A return callback uses an event or interaction boundary already supplied by the host; if none exists, the plan remains a content design and does not prescribe a polling system.

## Pass 28 — Truthful gates for care-handoff dialogue

A care scene needs precise knowledge gates because the simulation stores less than characters can say. This pass defines a conservative condition model for “The Cup on the Rail.” It prevents unknown data from becoming blame, avoids stale responses, and keeps authored emotion separate from canonical facts.

### Gate classes and authoritative inputs

**Pair gate:** current GetCaregiverForPatient(patientId) and GetPatientForCaregiver(caregiverId) agree. If no pair exists, the active-assignment scene is unavailable. A general request-for-help conversation may still exist, but it must not use active-pair copy.

**Eligibility gate:** life, caregiver fitness, and patient need are queried through existing callbacks at preview/execution. Dialogue may show an action as provisional; it cannot cache eligibility as truth. Revalidate on click. If the command owner returns unavailable, report its result without translating generic failure into a character’s refusal.

**Roster gate:** show a specific job only when the current duty-roster query returns it. Since care start vacates the existing roster role, a post-assignment snapshot may show no role. Absence of a role is not proof of which job was vacated. Do not infer historical work from a blank slot unless an independent supported source records it.

**Bond/dialogue gate:** the current bond query is patient-keyed. Core fires an unlock event when a tick crosses the threshold; a content consumer must prove how that event becomes conversation availability. Do not recalculate the threshold in a second dialogue subsystem, and do not equate bond with consent, universal affection, or willingness to accept a replacement.

**Fatigue gate:** CaregivingSystem applies a fatigue delta during a valid tick, but the reviewed public host getters do not expose accumulated care hours or a caregiving fatigue ledger. If the needs owner exposes current fatigue, use that owner and a reviewed threshold; otherwise omit numeric gates. A spoken request for relief can be authored without a number.

### Unknown-safe truth table

| Pair query | Patient alive/needs care | Candidate eligible | Roster fact | Dialogue behavior |
|---|---|---|---|---|
| Active pair | True | Unknown | Unknown | Show perspectives; offer inspection, not blind assignment |
| Active pair | Changed since display | Any | Any | Refresh and withdraw outdated responses |
| No active pair | Any | Any | Any | Use general shelter scene or mark this scene unavailable |
| Active pair | True | False | Known role | Explain action unavailability using exact owner feedback; role is optional context |
| Active pair | False | Any | Any | Do not imply neglect; close as changed circumstances or re-query quest |
| Pair differs after reassignment | True | Any | Any | Bind to current IDs; never retain stale speaker references |
| Bond event absent | Any | Any | Any | Do not claim dialogue was unlocked by bond |
| Bond event observed | Any | Any | Any | Permit only mapped callback; no invented relationship effect |

### Freshness and return behavior

Create a read-only scene snapshot: survivor IDs, current pair, optional roster role, and any condition needed to offer an action. The snapshot supports display, not authority. On each consequential response, ask the owning host to preview/execute with its current state version. If versions differ, discard the stale result and present refreshed text such as “The board has changed since we spoke.” Do not save a second version counter in dialogue memory.

After save/restore, rebuild context from owners. The quest may remember lifecycle only through its existing quest save owner. Do not restore old dialogue snapshots as facts. Reopened scenes re-read the pair. If a caregiver dies and Core removes the pair during a tick, no active assignment remains to display. If the patient no longer satisfies NeedsCare, replacement assignment may be unavailable; say the current care action is unavailable, not that the original story was false.

### Relationship and emotional-state limits

Hidden emotional subtext can be expressed through wording, pauses, and who begins a conversation. Labels such as resentment, gratitude, fear, exhaustion, or consent are authored interpretation unless an existing validated owner provides them. Do not turn repeat visits into persistent hidden-emotion variables. Cosmetic variants may rotate deterministically only if a current seeded selector exists; every variant preserves the same gate and consequence.

Relationship changes are reported only when the canonical owner changes them. Care ticks already invoke affinity adjustment and maintain a care bond; dialogue must not issue a duplicate increment. Handoff is not a default penalty or reward. A line may adapt to current owner state but should not silently write relationship memory.

### Reusable gate manifest

For every dialogue node, record gate ID, owner queried, query timing, stale-state response, null/unknown behavior, privacy rule, and whether the gate changes text or command availability. Reuse the manifest for treatment discussions, duty transitions, companion callbacks, and faction mediation. A failed query produces an unavailable option or neutral wording, never an accusation.

**Minimum viable:** active-pair gate, safe general fallback, command revalidation, and result-backed response text. **Optional:** role-aware context and post-threshold callback after event consumption is proven. **Deferred:** memory across multiple care scenes, procedural emotional tone, and any missed-care history; each requires an explicit owner and migration/design decision. No dialogue implementation or new gate registry is authorized here.
## Pass 29 — Location truth, encounter memory, and unknown-safe context

Location-specific dialogue and quests need a truth table that distinguishes “record authored for this destination,” “record eligible here,” “record selected,” and “record resolved.” These are different facts. The static catalog proves the first two conditions only in part; runtime selection and saved encounter history own the latter two. The player must never be told that they visited or understood a micro-location because its definition exists.

### Context snapshot

For a proposed follow-up, a read-only context snapshot may contain:
- canonical destination ID supplied by the current expedition host;
- selected encounter ID and source catalog;
- selected choice ID and resolution result, when the current owner exposes them;
- day/expedition identity only if a canonical owner provides stable values;
- current quest stage from the quest owner, if one exists;
- whether the exact-site requirement matched and whether the content is depleted.

Do not persist this snapshot in the dialogue system. Rebuild it from the location, encounter, and quest owners when the conversation opens. A missing field is UNKNOWN, never false-by-default evidence of player choice. Revalidate before a consequential response.

### Gate matrix

| Evidence | Gate may unlock | Must not imply |
|---|---|---|
| Encounter ID micro_hospital_chapel_ledger selected at abandoned_hospital | Hospital-ledger observation callback | Who wrote the final entry or why it stopped |
| Same encounter ID at another location | No exact-site callback; likely impossible under current exact filter | A hospital visit |
| micro_depot_undertow_raft_line selected at location_flooded_subway_depot | Route-observation callback | Who owns the crates or where they travel |
| micro_gamma_levy_board selected at loc_garrison_checkpoint_gamma | Levy-board interpretation | A universal faction policy or future enforcement |
| Choice ID read_the_names / note_the_route / memorize_the_board resolved | Choice-specific authored callback if resolution consumer is verified | Knowledge stat, quest completion, or permanent map reveal |
| Definition is present in catalog but not selected | None | Player discovery or visit |
| Definition has empty requiredLocationId | General eligibility only | Guarantee, local fit, or player familiarity |
| Encounter has a depleting choice | Revisit behavior follows owner state | A permanent location closure unless the saved owner says so |
| Unknown choice or unresolved record | Neutral base prose | Inferred intent or consequence |

The chosen ID strings above are current data evidence; callbacks remain proposals until consumer and save paths are proven.

### Freshness, repeated visits, and memory

An exact destination gate should compare the canonical IDs from both sides, not title strings. At the point a conversation is displayed, recheck the resolved encounter record rather than relying on map discovery. If an alias enters through the host, the host’s authoritative resolver must produce a canonical ID before dialogue unlocks. If IDs differ or resolver output is absent, withhold the callback and offer a neutral line.

Repeated visits need distinct semantics. A not-yet-selected record may remain eligible on another run. A record resolved with a non-depleting choice may follow current NarrativeEncounterSystem behavior; a depleting choice uses the current saved depleted set. Do not introduce “seen site” or “memory freshness” flags in the dialogue layer. If the encounter owner has no public query for resolution history, treat callbacks as unavailable pending an API/consumer audit instead of mirroring the history.

Knowledge gates should be specific to evidence acquisition, not inference. Reading names can unlock a line that the player read the names; it cannot unlock a claim about a person’s identity. Marking the route can unlock a note that a route was mapped; it cannot establish safe passage. Memorizing the board can unlock recall of its marks; it cannot certify payment, collection, or legal status. Use authored textual uncertainty to make that limit clear.

### Accessibility and fallback behavior

If the location ID, choice result, quest state, or dialogue route is unavailable, display the original encounter text and let the player continue. A missing callback is not a failed quest. Explain why a pinned objective is waiting only when the quest owner can present that state; avoid raw IDs in player-facing copy. Subtitle and journal versions should include the same uncertainty cues. Do not use color alone to distinguish exact-site evidence from general information.

**Minimum viable:** static exact-ID validation plus no callback until the resolution consumer is proven. **Optional:** a read-only post-encounter scene with ID/choice gates and reconvergent prose. **Deferred:** cross-visit familiarity, inferred faction knowledge, rumor propagation, and generated place memory; each requires a canonical information owner and an explicit provenance contract. No new dialogue memory registry is proposed.
## Pass 30 — Gate regional prose on canonical map knowledge

The region chart is authored context; the travel map is canonical route knowledge. Dialogue must not conflate them. Current POI labels are explicitly local to map_regions.json. The travel map and locations catalog carry no region field. The live map panel projects WastelandMap state. Content may discuss the chart before a route exists, but only the canonical map owner can establish known, surveyed, visited, or reachable.

### Knowledge state matrix for “Two Maps on the Table”

| Chart entry | Canonical node | Route status | Player-safe wording |
|---|---|---|---|
| Region exists, no node resolved | None | None | “The shelter chart names a region; route not verified.” |
| POI label only | Label is catalog-local | None | “The chart marks a point; the map cannot route there yet.” |
| Candidate node matched by approved crosswalk | ID resolved | Locked/unreachable | “The place is identified, but the route is unavailable.” |
| Candidate node resolved | ID resolved | Reachable | Offer ordinary map selection; do not force a quest destination |
| Node surveyed | Canonical survey says surveyed | Recheck route | Say “surveyed,” not physically visited |
| Node visited | Canonical knowledge says visited | Existing route | “Visited” may be shown, subject to source freshness |
| Chart disagrees with map | No authoritative resolution | Any | Preserve both sources and identify them |
| Missing/corrupt map context | Unknown | Unknown | Hide consequential option; retain neutral atlas text |

A similar display name is not a canonical match. Prefix grammar is not evidence.

### Conversation graph and commands

The scene can open from an existing archive or map panel. The archivist explains the difference between regional description and route graph without requiring discovery. The expedition lead may name a route only when the map provides a node ID and current route state. The player’s “verify a mark” response invokes the existing map survey/route action; it does not create a node. If no approved mapping exists, disable the command and state that the mark cannot yet be matched.

Main.RecordCartographySurvey requires a valid living survivor and canonical map node, delegates to WastelandMap, and awards existing scavenging XP. Use the returned canonical result, not a cached region survey. If the action fails, say no survey was recorded. Do not convert an ineligible survivor or missing node into a refusal scene.

### Freshness and memory

A dialogue instance may hold an ephemeral chart label and candidate ID for display. Before travel or quest advancement, re-read current node, fog, and route eligibility. A map/world change invalidates the action view. On save/restore, rebuild the display from canonical map and authored chart. Quest stage persists only through the existing quest owner; dialogue must not copy map discovery.

Player information types remain distinct:
- Read the chart: authored text opened.
- Heard a rumor: only if a canonical rumor event/source exists.
- Surveyed a map node: canonical survey status.
- Visited a destination: canonical map says visited.
- Verified regional membership: only after crosswalk approval and an action that actually verifies membership.

A response cannot set these facts itself. Do not infer hazard knowledge merely because a player opened an atlas page unless the page was available to them.

### Emotion and relationship gates

Regional identity can color voice but does not imply faction attitude, local loyalty, fear, or trust. The archivist may be cautious because records are incomplete; that is authored tone. The expedition lead can disagree without changing relationship values. Any relationship consequence needs an explicit current-owner command and is outside this scene.

**Minimum viable:** neutral archive scene and truthful availability copy. **Optional:** a canonical map survey branch after crosswalk validation. **Deferred:** regional reputation, generated dialogue from map density, dynamic site control, and memory of unvisited locations. Each needs one stable owner, explicit unknown behavior, privacy review, and save contract. No dialogue-memory registry is proposed.
### Pass 30B — Regional knowledge state transitions and edge cases

| Before action | Player action | Canonical result | Allowed update | Forbidden inference |
|---|---|---|---|---|
| Chart read; no node link | “Verify” | No command can resolve label | Keep quest at research step | Mark destination discovered |
| Link exists; node unknown | Request route | Map owner reports unknown/locked | Show clue or ordinary map path | Force quest marker onto unknown node |
| Node known; path unavailable | Survey request | Survey fails or route rejects | Explain unavailable; retain partial state if supported | Award survey XP or count objective |
| Node reachable; living survivor selected | Survey | Main survey succeeds | Consume returned map result once | Add duplicate CartographySystem discovery |
| Node surveyed but not visited | Discuss arrival | Map says surveyed | Say “surveyed” | Say player visited location |
| Node visited | Discuss return | Map says visited | Use current map callback | Infer a second POI was also visited |
| Chart and map disagree | Ask archivist | No accepted resolution | Keep both sources labeled | Overwrite either source from dialogue |
| Node-link schema removed | Reopen quest | Relation absent | Return to archive-only path | Read stale relation from quest save |

A survey result records canonical map knowledge; it does not certify that every regional POI label corresponds to a distinct node. If one node is later assigned to a region, the interface should still distinguish region membership from discovering a second point.

**Predicate ordering:** first check that the archive surface and quest are available; then resolve region ID as authored context; then ask for an approved node relation; then query canonical node/fog/route; finally preview a player command. Stop at the first unavailable layer and preserve neutral text. Do not query later systems with an empty string and interpret a default answer as an affirmative result.

**Staleness:** the route may lock between graph display and action. Revalidate immediately before map action. A stale result returns the player to the region card with current availability copy, not to a quest failure. If a region relation changes in a future catalog revision, rebuild from content on load; do not trust a serialized old relation.

**Privacy:** chart contents may identify households, military infrastructure, or hazards. A public shelter board should not reveal a player’s exact map discoveries unless the existing map-sharing feature permits it. Dialogue text may discuss a region generally while withholding node details. No faction reputation or interpersonal trust gate should be derived from whether a character believes a chart.

This truth table can later support map quests, radio-reported routes, faction maps, and expedition debriefs. It preserves distinct states for authored report, rumor, survey, visit, and verified regional assignment.

## Pass 31 — Trade Context as Ephemeral Evidence, Not a New Memory Store

### Context packet

For caravan-related dialogue, context should be a read-only packet assembled at the point the scene opens. Candidate fields are: route ID and confirmed arrival status; current stop and day if exposed; trader/faction identity; observed inventory IDs and quantities; relevant authored route demands/surpluses; computed regional modifier for a requested good; current quest state; canonical player knowledge flags; existing trust/stance inputs; and the source/provenance of each fact. This packet is an interface proposal, not permission to create a second save store. Do not persist a second “caravan memory” just to remember a cargo list that the current caravan owner already owns.

Gate categories:
- Visit gate: require a confirmed current presence, not a planned or rumored arrival.
- Cargo gate: require the good to be in the current visible inventory, not merely eligible under regionalSupply.
- Price gate: query the current atlas/transaction projection; absent price entry means neutral modifier, not “unknown price.”
- Route gate: distinguish the route catalog's authored planned demands/surpluses from observed cargo.
- Knowledge gate: require a clue or journal fact the player actually acquired.
- Trust gate: use existing trade stance/trust band selection; avoid duplicating numerical thresholds in dialogue JSON.
- Faction gate: resolve current access/standing through its canonical owner, if that exact query exists.
- Quest gate: check the supported state and prerequisite outcome, including expired/missed-window alternatives.

If a requested fact is unavailable, gates fail closed into an explicit unknown or neutral branch. Never convert null into false evidence (“there is no route demand”) or true permission (“the caravan must be here”). The line should remain coherent in both states. A factual fallback can say “The route sheet is incomplete”; a neutral price fallback can show the computed standard amount. UI should communicate why an option is missing only where the player understands the reason without exposing hidden narrative variables.

Memory levels for this topic should be owned by existing systems:
- Session-local: selected line rotation may use the existing seeded RNG; it must not become a new campaign fact.
- Current visit: presence and visible stock are read from the active caravan/trade surface.
- Durable player knowledge: quest/journal/map owners record clues or discovered places when their contracts support it.
- Relationship memory: canonical trust/relationship owner only.
- Long-term route history: route owner or Chronicle only if an event fact is already emitted and consumed. No reconstruction by scanning UI dialogue.

Repeated visits need disciplined callbacks. On the first visit, the trader can explain the manifest. On a later visit, dialogue may reference a previously completed agreement only if the quest/chronicle state supplies a stable fact. If no record exists, use a noncommittal greeting that does not imply memory. A changed cargo list can alter what is currently said without implying that the character remembers the player's last offer.

State leakage safeguards:
1. Build context fresh after save restoration and before presenting choices.
2. Revalidate each gated option at selection/commit time.
3. If context changed between render and commit, return an unavailable result and refresh the view; do not apply stale effects.
4. Keep scene-local node visitation transient unless the existing dialogue owner owns it.
5. Make save/load tests assert source-authority restoration rather than duplicate cached state.
6. Ensure deterministic tell selection is stable for same inputs and RNG sequence; do not consume campaign RNG for cosmetic dialogue if it perturbs unrelated simulation streams.

Test cases for future implementation: absent route fact; planned but not arrived; arrived with no regional specialty lot; specialty lot present but not in current inventory; missing regional price row; item-level price override winning over category; known rumor versus verified clue; active quest resolved while trade panel remains open; save/load before conversation; same seeded stance selecting the same tell. These are acceptance scenarios, not tests added by this documentation edit.


### Pass 31B — Context validity and UI copy

A conversation context packet should carry a freshness boundary. The trade UI can remain open while campaign day, caravan position, or inventory changes; every response therefore needs a validity rule. At presentation, record the canonical IDs and visible facts used to build the option list. At commit, query the owner again. If the current state differs, return a stale-context response and rebuild the dialogue without applying the response's durable effects.

Suggested user-facing fallbacks:
- “That cart has already moved on.” Use only when the route/caravan owner confirms departure.
- “The list was copied before the load was counted.” Safe as a character report; it does not assert which goods are present.
- “We have no current price difference recorded here.” This may be misleading if neutral base price is valid; prefer displaying the valid quote with a short “standard rate” label only after UI semantics confirm it.
- “You have not heard that part of the story yet.” Use only when knowledge gating is intentional and the option is visibly locked.
- “The goods changed while you were deciding. Check the table again.” Appropriate for stale inventory if the user has a refresh action.

Hidden emotional states should affect phrasing only, not unlock a transaction or change the amount. Reputation gates should expose only the minimum truthful reason (“they will not negotiate with us today”) and must not leak secret faction thresholds. Skill gates may provide extra interpretation of a manifest, but the baseline route and cargo facts must remain understandable without a build-specific skill. A skill adds context; it must not be the sole way to discover a hard-required progression clue.

The context owner must identify which values are snapshots and which are live queries. Snapshot fields are appropriate for a rendered line that has already been spoken; live fields govern a transaction. Save tests should establish that conversation node progression, if durable, restores from its authority, while visible stock and price are re-derived from the restored trade state. Do not serialize a copy of stock into the dialogue node save.

## Pass 32A — Evidence Freshness, Human Memory, and Diagnostic Gates

### Context contract

A maintenance conversation combines facts with different lifetimes. The implementation design should label each context field as current query, dated authored source, player knowledge, or speaker report. Suggested read-only packet:
- machine_id and authored display identity (if resolved);
- condition_key plus current value/band only when the owner exposes it;
- time/day of the reading and the query's freshness window;
- current diagnostic tell IDs and whether an existing journal knowledge key says they have been noted;
- maintenance record IDs, authored record dates, author/speaker, and discovery status;
- current repair/service availability as returned by its owner;
- quest instance and legal transitions;
- relationship/access facts from their canonical owner only if required by a branch;
- source attribution and uncertainty marker for each reported statement.

The packet is not a save object. It is reconstructed when the scene opens and revalidated before any state-changing response. If the diagnostic API returns no reading, the gate produces “condition not available” rather than substituting 100, zero, or a remembered value. If an authored record has no machine identity, it remains searchable historical text, not a machine-specific fact.

### Gate taxonomy for this story

**Observation gate:** the current tell was actually shown or an existing journal knowledge fact proves that the player noted it. A threshold crossing alone does not prove exposure. **Historical-record gate:** the player has discovered the exact source record; catalog presence alone is not player knowledge. **Witness gate:** the speaker is available and their reported statement has not been mistaken for objective telemetry. **Skill interpretation gate:** a relevant skill may explain a reading, but the baseline clue remains legible without it. **Relationship gate:** only an existing durable relationship value can change tone; it cannot rewrite the underlying machine evidence. **Access gate:** a restricted record is hidden only if an existing access owner enforces it. **Action gate:** an inspection or repair response is enabled only when its command owner confirms a safe, eligible action and costs.

Gate errors need four-valued handling: true, false, unknown, stale. Unknown means no source; false means a source was checked and condition did not hold; stale means a previously true snapshot is too old for the action; true means current evidence supports the branch. If current dialogue schema is boolean-only, use explicit fallback nodes and resolve gates at scene-building time. Never infer “safe to open” from a quiet sound or “failed machine” from a low-confidence tell.

### Memory and time

Transient conversation memory: which node the player has seen in this open scene. Persist only through an existing dialogue/quest owner if the player can leave and resume. Durable knowledge: JournalSystem's actual knowledge keys and quest facts; a glitch-noted key means “noted,” not “repaired.” Character memory: a verified relationship/chronicle fact if the owner exposes one. Machine memory: current owner state and its own save path. Authored record memory: fixed source document and date. These five must not be collapsed into one boolean such as maintenance_complete.

Timeline design distributes pressure:
- Same shift: a tell is observed and noted; no diagnosis necessarily follows.
- Next shift: another witness reads the existing record, allowing an attributed callback.
- Several days later: service can be attempted only through a real owner command, with an explicit availability check.
- Seasonal or campaign-scale: a repeated maintenance motif can pay off in a chronicle only when canonical events were emitted and retained.
Any in-world date shown in dialogue comes from the source record or canonical campaign day. Never derive a work date from wall clock or file modification time.

### Context invalidation and privacy

If the machine condition changes while the conversation is open, an action based on the old value must be disabled and the scene refreshed. If the only change is display wording, the already-heard line need not be retroactively rewritten. If a character leaves, do not keep their conversation option available. If a record is discovered through another interface, re-evaluate the knowledge gate rather than duplicating the record. If a player chooses not to disclose an observation publicly, make no privacy promise unless the journal/access owner can enforce it.

Log the gate outcome for debugging only through existing diagnostics, not player-facing hidden-state dumps. Tests later should cover no reading, fresh reading, stale reading, tell displayed but not noted, record catalog-loaded but undiscovered, witness absent, unknown identity, and relationship change after scene open. These are design acceptance cases; no tests are added by this pass.


### Pass 32B — Gate truth table, replay cases, and safe fallbacks

| Context predicate | True path | False path | Unknown/stale path |
|---|---|---|---|
| Current machine reading exists | Show value, units, and source time if available | Do not display a diagnostic result | “Current reading unavailable”; disable service action |
| Tell was presented or noted | Allow “investigate this tell” | Keep the machine usable; no quest claim | Rebuild context and ask the existing owner |
| Static record discovered | Quote or summarize with author/date | Keep exact text hidden | Show only catalog-neutral description |
| Witness available | Offer attributed conversation | Route to record or return later | Do not synthesize a replacement witness |
| Service action currently permitted | Show cost and downtime before confirmation | Explain owner-returned reason | Refresh the schedule and re-evaluate |
| Prior action committed | Show its exact result | Keep action available if still valid | Query receipt; never replay on assumption |
| Same prior quest conclusion exists | Offer callback using recorded fact | Present first-visit wording | Use neutral greeting until source resolves |

Replays should include a fixed campaign seed and identical source state, but dialogue selection should not consume unrelated simulation randomness merely to vary a phrase. If the existing tell provider consumes its supplied RNG, call it only through its established seam and record whether this is a cosmetic choice or a durable quest result. Same context plus same seed must not change eligibility. If output wording rotates by design, keep that rotation separate from selection, repair, or quest transition.

Skill checks must not gate the only clue needed to preserve progression. A player with a diagnostic skill may see “the interval changed between two dated notes”; another player still sees both dates and can ask for a check. Faction or reputation gates can alter whether a character will share a private work note, but if the note is mandatory, an alternate discoverable copy must exist. A hidden emotional state may make Vale curt or hesitant; it cannot decide whether the machine is safe to service.

Privacy branch contract: “Keep my report off the board” must be phrased as a request, not a guarantee, until an existing access or journal authority can store and enforce the visibility choice. Otherwise offer “Tell Vale you prefer a private conversation” as cosmetic scene content. A quest choice should never promise permanent redaction when a codex or save file has already stored the fact.

Staleness thresholds are owner-specific. A machine reading taken earlier in the current frame/day may be acceptable only if that owner defines it; do not invent universal minutes or days in dialogue data. Authored dated logs never become stale in their historical meaning, though their interpretation can be superseded by later records. Character statements retain their speaker and time; a later statement does not erase the earlier one.

Future focused verification should prove the truth table through public owner contracts: every gate's true, false, unknown, and stale path; scene close/reopen; save/load; another panel recording the tell; machine condition changing while dialogue is open; and command rejection due to stock or schedule. Until these APIs exist, acceptance criteria remain architectural test designs and not test files.


### Pass 32C — Cross-surface memory and staleness contract

The dashboard tell, journal, maintenance catalog, and quest card have different roles but should project the same underlying source facts. A tell is an immediate diagnostic projection; a journal entry is durable player knowledge; the catalog is authored evidence; the quest card is an action summary. Opening one surface must not mark the others read unless the existing discovery owner explicitly defines that behavior.

Distinguish events: tell eligible, tell presented, tell noted, record discovered, record read, action requested, action accepted, action completed. Persist only distinctions the current owners actually support. A one-shot tell that has no presented/noted distinction cannot safely unlock a later dialogue branch by assuming the player saw it. A repair request is not a completed repair.

Cross-surface case: the tell appears; player opens its journal entry; the display refreshes and no longer shows it; on returning to the quest scene, the existing noted fact remains usable. Conversely, if a log was read first and a tell later appears, say “this may relate” unless both exact source IDs are available for a precise callback. Re-rendering must never unlock a second reward.

A context packet should be a read-only snapshot of stable IDs and observed facts. Revalidate at commit. Locale changes affect display labels, not identifier comparison. If a player requests privacy but no access policy enforces it, describe the request as conversational only and do not label the record confidential. Do not promise erasure when the fact is already saved.

Verification should compare dashboard, journal, catalog, quest view, and restored save. Each should identify source and date when evidence conflicts. A mismatch is a content/system finding, not a reason for whichever panel loads last to overwrite the rest.


### Pass 32D — Restore and interaction edge cases

After loading a save, rehydrate machine owners, journal knowledge, and quest state before constructing the conversation packet. Never reuse an in-memory packet from the prior run or slot. If restoration reports a missing optional log catalog, preserve durable quest facts but remove choices that depend on that source; show neutral fallback copy. If the machine owner restores a different current state than the pre-save view, rebuild the scene and invalidate any pending action.

A panel closed during a pending inspection must not commit it unless the command owner already accepted it. Reopening should query the receipt or service state, not submit again. A second UI surface that records a clue should become visible through its canonical knowledge owner, while retaining original source attribution.

## Pass 33A — Difficulty, Ration, Cohort, and Source Context Gates

### Context packet for the 30-day view

Build a read-only scenario packet from current canonical owners:
- campaign snapshot day and campaign/save identity;
- active difficulty preset ID and effective thirst multiplier;
- survivor IDs, current need values, and relevant life-stage state;
- ration policy and whether the projection holds it constant or reads a day-by-day history;
- inventory counts for clean_water and irradiated_water;
- WaterTreatmentSystem tank quantities, clearly separated until reconciliation is proven;
- WaterSourceSystem source state, discovered/active status, stored liters, contamination, last test day/result, and nominal catalog flow;
- active visitors and their authored daily water rates, if the current visitor owner uses them in the scenario;
- action/job draws, with exact resource, amount, unit, and schedule;
- known unknowns and stale fields.

The packet is not a save state and cannot change inventory, survivor needs, treatment jobs, source activity, or policy. It is rebuilt whenever the player opens the budget view and revalidated when an action is selected.

### Gate matrix

**Difficulty gate:** only the preset ID stored for the campaign and the provider's current scalar count as effective. A settings preview or uncommitted preset selection does not alter the campaign scenario. **Ration gate:** a future policy switch can be projected as a scenario, but actual policy changes use the existing ration command and owner. **Cohort gate:** late arrivals/deaths invalidate per-person totals and require a new snapshot. **Unit gate:** do not sum inventory item quantities, liters, and thirst-meter points. **Source gate:** nominal flow is visible as catalog context; operational output requires current source state. **Quality gate:** a test result is valid for its source and test day, not every connected source unless the water owner establishes propagation and validity. **Consumer gate:** list only discrete draws for which current amount and schedule are known. **Freshness gate:** after any inventory, policy, cohort, source, test, or job change, mark the prior projection stale.

Four-valued results:
- true: required snapshot and all modeled inputs are current;
- false: a checked condition does not hold, such as a source being inactive;
- unknown: owner does not expose a required amount or the units cannot be reconciled;
- stale: the input was valid but changed after the scenario was calculated.
Do not coerce unknown to zero. A “surplus” label is forbidden while an essential consumer remains unknown.

### Memory and dialogue

A saved scenario receipt may record that a player ran a projection, plus its snapshot identity and declared assumptions, only if an existing journal/quest owner supports this. It should not copy every mutable stock value into a new dialogue save store. Re-opening the budget after a new day should create a fresh projection; a past report remains an attributed historical estimate. If the player changes difficulty, the result is recalculated for the new committed preset, not retroactively rewritten into the old one.

Character dialogue must query knowledge, not infer it from catalog presence. A source card can be authored and loaded but undiscovered; a water test may exist in the save but not be read; a character may know the spring while the map does not. Keep these gates distinct. Skill may explain contamination units or test confidence but must not be the sole path to the mandatory meaning of a water warning.


### Pass 33B — Freshness, revalidation, and save-resume contract

A 30-day projection can become stale without a single dramatic event. A survivor joins, leaves, or dies; a child changes life stage; a ration policy changes; an item is consumed; a water treatment job completes; a source test is taken; a source becomes inactive; or a visitor's state changes. Define a source fingerprint from stable IDs and owner-provided revisions if available. If no revision exists, compare a fresh read at action time and recalculate. Do not trust a cached projection across day advance or save restore.

The displayed context should reveal the scenario assumptions: “same roster, same ration policy for thirty days” or “current policy held constant.” A projection under Dirge is not the campaign's current state if the save remains Standard. Previewing another preset is presentation only; selecting a new preset uses the authorized new-campaign/settings route and its persisted campaign rules. Never alter live difficulty when a player merely compares rows.

Save/resume behavior: persist actual campaign owners through their existing save sections. A scenario history may persist only if an existing quest/journal owner has a suitable durable field; otherwise the completed scenario can be regenerated. If a saved report is shown later, mark it historical with its snapshot day. On load, compare it against current roster, policy, stock, source, and jobs before offering “refresh.” Do not overwrite the prior record; write a new scenario result or show a fresh unpersisted preview.

Gate cases: empty roster, no difficulty owner, malformed/missing preset, unknown ration policy, inventory below requested policy amount, treatment tank present but no bridge to inventory, source flow but inactive source, old test result, schedule not exposing an interval, unknown consumer, source count changed, and a fresh cohort snapshot. Each case yields an explicit state. Missing preset falls back only as the current difficulty authority defines; no quest-side default should overrule that.

Medical treatment, hydroponics, foundry operation, visitors, and industrial consumers may share the item name while using different units or scheduling. A context packet must not infer that they share a stock store or are active simultaneously. Branch availability comes from the owner that will execute the command, and all dynamic quantity text must be recomputed before confirmation.


### Pass 33C — Accessibility and projection integrity

A budget view can be mathematically correct and still mislead if it collapses a state scale into a resource bar. Present inventory units and liters with visible unit labels, and thirst as an individual condition meter. Screen-reader output should announce resource name, unit, source owner, snapshot date, and whether the number is actual, authored, or projected. Never use color alone to signal scarcity, contamination, unknown, or stale data.

Do not average away severe individual thirst in a cohort summary. The panel can show cohort range, median, and named severe outliers only if the existing roster provider exposes them. Averages are optional context, not a substitute for per-survivor inspection. If privacy or screen-space constraints prevent names, expose count and severity bands without hiding that individual status varies.

Knowledge gates should not reveal exact water source IDs before discovery. A locked response can say “You need a recorded test before the medic will interpret this result,” only if a valid test is genuinely required. Do not use a hard skill gate to hide the fact that contamination has been measured; skill can explain measurement confidence, not erase the source data.

Comparison layout has four rows for the four presets and a separate policy section. If the player previews another preset, label it PREVIEW and leave the committed campaign preset visible. Keyboard/controller navigation should move between row groups predictably; opening a source detail should return focus to the selected row. Unknown consumer count and stale snapshot warnings must be textual, persistent until refresh, and not communicated only by an icon.

Context integrity checks include rounding. Do not round a small but nonzero treatment output to zero while separately calling it absent; show a bounded precision appropriate to the owner. Do not sum floats from liters with integer item counts. A displayed “remaining” value should be derived from the same snapshot as its inputs and identify omitted demand.


### Pass 33D — Context truth table for campaign changes

| Campaign change while view is open | Required behavior |
|---|---|
| Difficulty preview changes | Keep committed campaign scalar; label the preview only |
| Difficulty is actually changed through an allowed route | Rebuild thirst comparison; preserve prior report as historical |
| Survivor joins or leaves | Invalidate roster snapshot and per-person projection |
| Ration policy changes | Recompute policy debit; do not edit prior receipt |
| Clean inventory changes | Refresh current stock and state whether the projection is stale |
| Source test is recorded | Use the new dated result; retain previous test as history |
| Treatment job advances | Re-read tank/job results; do not infer output from elapsed time |
| Save slot changes | Discard transient packet and reconstruct all owners |
| Unknown consumer appears | Remove surplus conclusion and mark omitted demand |

A previewed scenario is never an implicit player choice. Any “apply” button must identify the owner command, current policy, resource effect, and confirmation step. Return focus to the affected row after refresh so the player can compare old assumptions with new values. A campaign day boundary invalidates any owner snapshot whose day-sensitive values may have ticked.


### Pass 33E — Hidden information and fair explanation

A hidden source can support discovery play, but it cannot be a silent dependency for the basic budget comparison. A locked source dialogue should leave the player a usable policy and inventory view. If a report is hidden by faction access, explain the access limit without revealing the report's contents. If its source identity is unknown, the player can still record that an unattributed report exists.

## Pass 34A — Context Gates for Honest Testimony

### Gate matrix

This packet specifies dialogue availability around “The Return Column” without creating an additional persistent memory system. Every gate must read an existing fact through its owner or a read-only projection. Quest acceptance comes from the quest owner; expedition outcome from the expedition result; identity and availability from the roster; map visibility from cartography; standing from the standing owner; and knowledge provenance from the Codex or narrative knowledge path if the required fact is exposed there. A dialogue panel may cache a view for rendering but never become an authority.

| Gate | Required evidence | On false | Player-facing fallback |
| --- | --- | --- | --- |
| Returned party | Resolved expedition lists one or more living returnees | Scene stays unavailable | Ordinary expedition debrief |
| Objective failed | Result identifies the objective as unmet | No failure-forward report scene | Standard outcome summary |
| Rell is present | Roster/scene availability says Rell can speak | Hide voiced response | Use authored statement only if one exists; otherwise allow delay |
| Empty case observed | Result or inventory evidence records the case | Hide item-specific response | Use the neutral report option |
| Relay clue known | Provenance says player learned the lamp went dark | Hide follow-up inference question | Offer inspection or return visit only when valid |
| Report already published | Quest/world record says publication occurred | Show publication choice | Keep the choice available |
| Faction account known | A channel has delivered the account | Hide rebuttal or challenge | Do not preview unlearned faction rhetoric |

These rows are design contracts, not assertions that each exact field already exists. During integration, mark each field VERIFIED, PROJECTABLE, or BLOCKED with source references. A PROJECTABLE fact may be exposed through an existing host adapter if that is within its current contract. A BLOCKED fact pauses this branch; do not replace missing evidence with a guessed boolean in dialogue state.

### Memory tiers and repetition

The scene needs only narrow memory: whether the player chose correction, publication, or delay; whether testimony has been heard; and which clue source was actually learned. Use the quest’s persisted choice and current knowledge owner where possible. Do not store a transcript, player free text, or an inferred emotional profile. Revisit variation should derive from those remembered actions and the current world facts. If a save predates a field, default to an uncommitted state and provide a neutral line. A migration may not reinterpret missing data as “player accused the team.”

Emotional tone is a presentation layer: Vesta may sound relieved after a corrected notice, tense after a public accusation, or tired if the issue has been deferred repeatedly. These variants do not create secret relationship scores. If the relation owner reports a relevant established relationship state, the scene can select a voice variant within authored bounds. It cannot recalculate relationship from conversation frequency. An unavailable companion must not appear in a remembered line unless the content is a stored message or journal record with a real source.

### Repeat-visit and failure behavior

On the first visit, show the initial question. After the report is corrected, show a brief acknowledgment and one new question about the follow-up clue. If the player left without deciding, resume at the same hub with the current choices. If the player chose delay and the required witness remains unavailable, explain what is missing and offer to wait, abandon the optional follow-up, or use another valid route. If the player’s selected quest route later fails because its deadline elapsed, acknowledge that fact and expose any authored continuation; never reopen a completed consequence as if it had not fired.

### Accessibility and freshness

Gate explanations must be available through visible text, not color alone. Disabled options should identify the missing knowledge in ordinary language without exposing information the player has not learned. Keep focus order stable when options appear or disappear; restore focus to the hub if a node is gated after returning from a child branch. Freshness tests should cover immediate repeat visit, next-day return, save/load between clue and conversation, changed roster availability, and a stale map clue. A stale clue may lead to a blocked explanation, not an invisible node. All deadline comparisons use an anchored day and authored window.

### State-by-state dialogue availability

The quest status controls scene entry, while evidence and context control individual nodes. Inactive and unavailable quests expose no entry. Available/discovered quests may show a board lead; acceptance is an explicit action. In progress shows only the unresolved choice. Blocked state presents its dependency and any supported fallback. Partially completed state identifies what is recorded and what remains. Failed state gets a short acknowledgment and the authored continuation, if any. Completed state shows closure once. Resolved state may supply a later callback but must not replay the active choice. Expired and abandoned states explain how the player reached closure. Reopened state requires a new supported fact; old dialogue should not reappear solely because the game loaded a save.

| Quest state | Entry copy | Available actions | Prohibited implication |
| --- | --- | --- | --- |
| Available / discovered | “A report is waiting at the desk.” | Inspect or accept | Claim that a returnee consented |
| Accepted / in progress | “The clerk needs a verified account.” | Hear, compare, correct, defer | Promise that every witness is reachable |
| Blocked | “The signal record is not available here.” | Wait, use shelter witness, abandon optional lead | Mark the quest failed without authored rule |
| Partially completed | “One account is recorded; the cause remains open.” | Review source, continue, close neutral report | Turn uncertainty into accusation |
| Failed / expired | “The filing window closed before the second witness arrived.” | Read closure, pursue permitted sequel | Hide the deadline or imply a factual verdict |
| Completed / resolved | “The notice now matches the evidence collected.” | Review journal, leave | Offer duplicate irreversible actions |

These lines are placeholders for UX review and localization. If a state label does not exist in the live quest system, map the design to the nearest supported state and document the difference. Do not add a parallel dialogue lifecycle just to match this table. The plan’s required contract is clear feedback for players and no silent loss of an active quest.

### Skills, reputation, and knowledge

A skill gate may expose a question, a source comparison, or an alternate way to interpret timing. It should not unlock the only completion route. An unsuccessful check can reveal that the player lacks enough evidence and return to the main hub. A reputation gate may alter whether a faction representative attends or whether a public board will display a note; the gate must be based on the current faction owner and cannot be inferred from a dialogue choice. A knowledge gate requires a provenance record for the exact clue. An unknown rumor stays unknown even if the player guesses it in dialogue.

Faction-specific, location-specific, repeated-visit, failed-quest and player-action-reactive dialogue are all useful here, but they compose as conditions rather than as a combinatorial prose explosion. Prioritize the conditions that change the player’s understanding or available action. The first slice should have a neutral fallback whenever multiple gates overlap, and gate evaluation should be stable for the same saved facts. Store only the minimum choice needed to resume and render the scene; do not save every line read.

### Test matrix

Content review should enumerate all gate pairs that could conflict: player knows rumor but witness is absent; witness is present but no clue is known; report has been published and the quest deadline has passed; faction representative is unavailable after an expedition; player has high reputation but no evidence; old save has no explicit report choice. Verify no contradictory node appears, one fallback is always reachable, and return navigation restores focus. Confirm hidden emotional variants do not reveal the outcome choice before the player sees the report. Use source-owner read APIs rather than polling panels for state.

### Consent, hidden state, and player trust

The scene must separate private testimony from public attribution. A returnee can agree to speak to the player while refusing to have their name printed. Treat those as two separate consent decisions only if the project has a current consent owner capable of recording them; otherwise keep the first slice to a single explicit authorization with no inferred permission to publish. The player must see the audience and scope of each choice. “Record this for the file” is too vague if the record will appear on a public board or affect a faction.

Hidden emotional states may shape authored tone only through an established relationship or character-state owner. Do not invent “fear of blame” as an invisible dialogue variable. If no relevant state exists, provide a stable voice line. Relationship changes must be visible through the normal relation projection and must not secretly modify quest eligibility in the same response unless the consequence contract states that effect. A refusal should preserve access to neutral completion and should not be framed as dishonesty.

Gates should reveal their source without spoiling hidden content. A disabled response might say “You do not have a statement from the relay crew” rather than disclose the crew’s later whereabouts. When the player has not learned a rumor, dialogue cannot quote it. When an old save contains a completed expedition but no migrated provenance, treat its knowledge as unknown or use a safe broad line. Never backfill knowledge from the current catalog because that would make old runs omniscient.

A content designer should review gate count, not just scene count. For each gate, answer: source owner, source field, save/migration behavior, visible blocked message, and safe false default. If two conditions are redundant, remove one. If the matrix exceeds the authoring team’s capacity, split the scene into independent conversations rather than adding a hidden priority override. The acceptance measure is a truthful, comprehensible choice set, not maximum personalization.

### Release rubric

The gate review should include a player-facing walkthrough with an ordinary save, a save made between clue and conversation, and a save predating the optional quest. At every disabled choice, the player should understand the next available action without learning a secret prematurely. Hidden state is justified only when an existing owner can explain it and the presentation meaningfully changes. If the player must guess which invisible condition failed, the gate is incomplete. If the gate is merely decorative, remove it. The first slice needs no emotional inference, no transcript archive and no new player profile. It needs clear memory of the actual report choice and a safe default for every older save.

## Pass 35A — Doctrine and Evidence Context Gates

### Gate authority matrix

This dialogue family depends on facts from several separate owners. The dialogue layer consumes a read-only context snapshot; it does not derive faction policy, journal delivery, territorial control, or player reputation itself. Before implementation, each proposed fact below needs an exact source field/API and a reviewed save/load path. If a fact is not exposed, mark it blocked instead of synthesizing a conversation-local substitute.

| Context fact | Gate source | False or unknown behavior | Safe text |
| --- | --- | --- | --- |
| Current doctrine is the subject | WarlordDoctrineSystem read projection | Hide doctrine-specific scene | Generic road notice, if authored |
| Radio key was delivered | Radio reception/knowledge owner | Do not quote broadcast | “A notice is being discussed” only when independently known |
| Journal key was received | Journal/discovery owner | Keep source unavailable | No journal-specific response |
| Enforcement action resolved | Action/event owner | Do not offer confirmed-action route | Record announcement or unresolved claim |
| Location is player-accessible | Map and expedition owners | No location scene | Continue at shelter hub |
| Witness is present and willing | Character/roster/consent owner if supported | Hide witness-specific response | Anonymous record or leave scene |
| Player saw the receipt | Existing inventory/document/knowledge owner | Do not count it as inspected proof | Allow testimony to remain second-hand |
| Relevant standing is known | Standing projection | No reputation-gated option | Use neutral access route |

The table is an integration checklist, not proof that the project exposes all of these facts through dialogue today. The world bible says current owners are authoritative; the exact context source should be verified through a small call-site census and ownership record. Do not expose hidden doctrine state if the player has not learned its identity.

### Memory scope and state changes

Memory needs are deliberately small: quest accepted or not; source record inspected or not; witness permission for public attribution if a consent owner supports it; and the player’s selected report route. Prefer the existing quest, journal, inventory-document, and consent state owners. The dialogue graph should not persist a transcript, a new “trust” counter, or a secret conclusion. A recurring visit derives its line from a real stored outcome and current owner projections. A save predating the choice must default to undecided; it must not default to “player sided with the faction.”

The active doctrine may change between the first conversation and a return visit. The scene should preserve the historical doctrine that was actually under investigation if that fact is already stored by the quest/event owner; it must not silently reinterpret the old report using the current doctrine. If there is no historical snapshot, show a neutral “the situation has changed” line and block the old doctrinal comparison. No wall-clock or latest catalog read may rewrite past campaign context.

### Emotional, skill, and faction conditions

Character personality is authored in voice blocks and can influence wording. Hidden emotional states are not inferred from silence, refusal, or conversation count. Skill checks may reveal an available detail but cannot establish an unseen enforcement fact. A failed check must reconverge to the unresolved branch. Reputation can change who will speak or where a notice is accepted, but any such gate requires an existing standing consumer and an explanation in the UI; no threshold should be invented for this plan. Faction-specific responses must appear only after the player has learned the faction context and must not reveal a hidden goal through an unearned line.

Review combinations for stale doctrine, radio heard but not action, action occurred but no player witness, map location known but closed, anonymous source, duplicate visit, and old save lacking a selected response. Every state needs a neutral fall-through and an accessible exit. Hidden-state variation should add meaning, not turn one scene into dozens of hard-to-author permutations.

### Decision and memory model

The dialogue should evaluate facts from a snapshot at scene entry. At minimum, it needs the doctrine ID under investigation, the player-visible source IDs, the active quest state, known location and scene availability, any relevant event identity, and a fresh campaign day if the quest is timed. It should not hold an open reference to a mutable AI object or query UI controls to infer state. If the active doctrine changes while the scene is open, resolve the current response against the entry snapshot or close and refresh the graph before presenting a new option. Never combine half-old and half-new facts.

Remembering the player’s action requires one explicit route marker owned by the quest system: source inspected, report selected, and optional publication requested. Avoid storing every dialogue node visited. If the player repeats a conversation after loading, the same supported source should not be awarded twice. If a later event contradicts the earlier conclusion, reopen only through an authored quest transition that cites the new event. A previously published unresolved record remains unresolved in history; it should not be silently rewritten to a confirmed result.

A source may have three distinct knowledge states: not present in world; present but not delivered to player; and learned by player. If the current knowledge system only represents first-heard identity, use that exact limit and do not add a secret “available in catalog” state that the UI treats as player knowledge. Dialogue can say the player has no record yet, but should not reveal that a hidden broadcast exists. A journal marker emitted by WarlordDoctrineSystem may be a campaign trigger; verify how it becomes a readable player-facing journal entry before using it as knowledge.

### Gate truth table

Test combinations should include: doctrine known, no action event; doctrine unknown, action event exists; broadcast delivered but no encounter; encounter occurred but source record not inspected; source inspected but party lacks permission to publish; location known but closed by faction state; doctrine changed since the initial report; quest completed and later contradictory event arrives; and old save with marker but no per-player delivery record. For each case, specify visible text, enabled responses, quest effect and safe fallback. Every branch must retain a way to leave and return.

Knowledge and reputation gates must be explainable. A skill check can reveal an inconsistency but does not create evidence. High standing can provide access to a room only when access uses the current faction/territory authority. Low standing cannot silently erase a public route unless that policy is in an explicit owner. A refusal to publish should still permit private quest completion. Emotional tone variants must never alter gate truth, consequences or the user’s ability to exit.

### Data freshness and failure handling

Persist only player choices and whatever source IDs the current quest contract requires. For older saves with no choice record, show the undecided hub. For a missing doctrine key, render a neutral unavailable-source response rather than a fallback invented from doctrine description. For a removed speaker, use a documented authored record or a generic close; do not generate a substitute survivor. On stale graph detection, refresh all context together and return focus to a stable hub node. The player sees a short explanation; developer diagnostics log which owner projection was stale.

### Context freshness, repetition, and availability windows

A doctrine question can become stale when the campaign changes its active doctrine, territorial controller, event stage, location state, or player knowledge. Snapshot only what the selected dialogue graph needs and define when it refreshes. Opening the conversation takes a consistent read. Returning from a subscene rechecks whether the source has been invalidated. A changed doctrine should close or neutralize the obsolete branch rather than show current doctrine text as the historical order. A changed controller may alter who occupies the site but not erase the memory of an earlier event.

First visit, repeat visit, next-day return, and post-transition return are different contexts. First visit introduces the case. Repeat before a decision resumes it. Repeat after completion acknowledges the actual result. Post-transition return may explain that the faction changed its policy, but only if that later transition is observed and delivered. Never produce a generic “they have changed” line from the internal AI state alone. The player may not know the transition has occurred.

Timed content is optional. If used, anchor the deadline to the accepted quest day or a named faction event day, not to the number of times a panel opens. Display a clear date/window and a warning before closure. An expired report can remain unresolved or move to a later hearing if authored; it cannot fail by hidden calendar arithmetic. An open-ended investigation is preferable when the game has no appropriate persistent deadline owner.

### Dialogue state availability grid

| Quest/owner context | Graph response | Memory requirement | Safe fallback |
| --- | --- | --- | --- |
| Doctrine key present, no player delivery | No doctrine quotation | None | Generic shelter rumor only if learned elsewhere |
| Broadcast delivered, no action event | “An order was announced” | Radio history | Announcement-only resolution |
| Action event present, no witness route | Do not claim firsthand knowledge | Event identity | Source unavailable / unresolved |
| Witness statement heard, document absent | Attribute claim to speaker | Knowledge provenance if available | Do not mark document inspected |
| Document inspected, public consent absent | Private evidence response only | Consent owner or local choice | Keep source anonymous |
| Current doctrine differs from quest snapshot | Historical report remains scoped | Stored case source identity | Neutral “situation changed” line |
| Location was consumed by another event | Hide repeat encounter | Encounter/event state | Journal or shelter route |
| Old save lacks delivery memory | Do not infer broadcast receipt | Migration default unknown | Unavailable-source response |

The grid is a content contract. The implementation must map it to the smallest set of current owner APIs. If a current system does not distinguish delivered from authored, the choice is to defer the knowledge-gated text or obtain an approved owner contract—not to add a dialogue-only receipt ledger.

### Accessibility and player confidence

Blocked responses need words, not only color or an icon. A player should know whether an option requires a heard broadcast, a witnessed action, a visited site, a signed source, or simply a later day. This wording should avoid spoilers. Focus order remains stable when a response appears after a source is found. Tooltips must be accessible by controller and keyboard. Date, doctrine and source names should be read as separate structured labels when the accessibility layer supports it.

Onboarding can teach one distinction: “A rule was announced” and “a rule was enforced” are separate claims. Do not add a tutorial modal that interrupts the first conversation. Teach through a short choice preview and a journal glossary entry only if the current Codex route supports it. The quest should remain playable if the player skips the glossary.

### Conversation-specific memory without an omniscient journal

A major case can accumulate many sources, but the dialogue should ask the current knowledge owner for each one rather than keep a shadow transcript. A case view may project a source checklist: known broadcast, inspected copy, witness account, verified action, and public result. The checklist is a UI projection assembled from existing source facts and quest progress. If one of those facts has no persistence owner, mark it unavailable; do not store it in a panel’s private list. On restore, the same sources and choices must appear.

Memory should distinguish “heard the clerk say it,” “saw the document,” and “recorded a conclusion.” A line might be paraphrased in a later summary only if the source ID remains available. If player response text is not authored, there is no free-text memory to replay. The game does not need to remember every accusation the player spoke; it must remember only the closed choices that changed route or consequence. This reduces save size, migration burden and privacy risk.

A repeated conversation may have one of five states: not started; started/no evidence; one source recorded; report selected; resolved. Choose this minimal ladder only if the existing quest owner can represent it. Otherwise map to its supported statuses, retaining evidence detail in existing item/knowledge state. Avoid a parallel dialogue status enumeration that disagrees with the main quest lifecycle Plan 17.

### Six context groupings for author review

- **Campaign context:** day, active doctrine, territorial state, relevant event phase. Review whether these are a live snapshot or a historical case anchor.
- **Player knowledge:** delivered radio, discovered location, inspected document, testimony heard. Hidden data is not player knowledge.
- **Character context:** availability, consent to speak, consent to be identified, relationship. Do not infer consent from relationship.
- **Quest context:** inactive, discovered, active, blocked, partially complete, failed-forward, complete or reopened. Display only the states the current owner exposes.
- **Location context:** discovered, dispatchable, occupied, consumed encounter, accessible interview. A visible map location does not guarantee the unique encounter remains.
- **Interface context:** selected response, focus, current graph version, panel lifecycle. UI state must not become gameplay memory.

Each group can change independently. Define refresh points and precedence so stale data does not produce a contradictory choice. If campaign context changes, update the historical comparison separately from the live present. If character consent changes, hide the naming response but preserve the anonymous completion path. If the graph version is retired, route to a compatible neutral terminal rather than reopening an old choice.

### UX acceptance and cognitive load

The conversation should surface no more than the evidence the player can act on in the moment. A case board can place sources in date order with source type labels, but should not require the player to memorize 24 doctrine names. New terms should be introduced in plain language and supported by a glossary only if the game already has the current codex route. Responses should describe likely scope (“file a private report,” “publish the verified action”), while a confirmation step is reserved for materially consequential actions. Back/close should always return to the expected panel and preserve focus. Disabled options disclose the missing prerequisite without revealing hidden content.

### Relationship, reputation, and emotional response boundaries

The player can build trust with an individual by keeping a source anonymous, but no plan should predict that trust value unless a relationship owner confirms a supported effect. There are four distinct questions: Did the speaker consent to tell the player? Did the speaker consent to have their name recorded? Did the player choose to publish the account? Did a faction or community actually receive it? These are not interchangeable. The first slice can avoid persistent consent state by making the player choose whether to record the statement at all, then displaying the exact audience before publication. If private and public audiences must persist, use the current consent and journal owners or defer the feature.

Faction reputation can gate who agrees to an interview, but should not gate access to all facts. A low-reputation player may hear public radio or inspect a public board if the current world supports those channels. A high-reputation player may be offered a private register only if a character authorizes it. Relationship-based, faction-based and knowledge-based dialogue may combine, but precedence must be explicit: missing consent should hide the public attribution option even when relationship is high; absent evidence should hide a confirmed claim even when faction trust is high.

Emotional states can be presented through visible acting cues or authored line variants tied to available survivor state. Do not use a secret “guilt” or “loyalty” score to select an accusatory response. The player should not be punished for expressing uncertainty or refusing to publish a claim. Failure reactions should acknowledge that the selected evidence path did not resolve the question, not that the player is cowardly or disloyal. Characters can disagree openly; the quest state remains factual and owner-driven.

### Availability, consent, and no-softlock rules

For each optional gate, record whether it is necessary for story flavor, a new clue, an owner command, or completion. Only the first three can justify a disabled response; none should block the neutral close unless the entire quest is designed around that exact owner action. If a character is absent, do not synthesize them through generated dialogue. If they died after providing a valid record, preserve the record and remove only future conversation. If consent is revoked before publication, hide the naming route; if the report was already published, the game needs an explicit correction/removal owner before promising revocation. If such an owner does not exist, do not add a revoke choice with no effect.

The same rules apply when the doctrine changes. A conversation can retain the old case and ask whether the player wants to compare a new announcement, but the player must know it is a new source. If no valid new source exists, show the previous case summary and close. Do not automatically reopen every prior quest when the Warlord changes doctrine. Trigger new cases only from existing delivered event signals and bounded active-case rules.

### Player testing scenarios for gates

Walk through as a new player who has not met the Warlords; a player who has heard the radio but not visited a checkpoint; a player who inspected the receipt but refused to identify Ivo; a high-standing player whose preferred witness is absent; a low-standing player with public evidence; a save loaded after doctrine transition; and an old save that lacks delivery details. Ask each player to explain which facts are known, which are disputed, and what action remains possible. Confusion is a content defect even when predicates technically pass. This qualitative review complements, not replaces, static gate matrix checks and focused implementation tests.

### Predicate composition and deterministic evaluation

A dialogue node can combine predicates for doctrine, event, delivery, location, witness and quest state, but conditions should be evaluated from a stable read snapshot. The authoring contract should define conjunction, disjunction, negation and unknown behavior. Missing source is not the same as false evidence; unknown must not enable a branch that requires proof. If a node accepts either a witness or a record, that alternative must be explicit and the result should retain which source actually satisfied the requirement. Otherwise later dialogue may incorrectly claim both sources were seen.

Evaluate pure predicates before choosing a response. Do not let response ordering, dictionary iteration or visit count influence which fact becomes active. If the graph supports deterministic seeded variation, use it for voice polish only after eligibility is resolved. A randomized line must not reveal an event the quest gate did not permit. Persist a result only where reloading could alter an irreversible selection. Cosmetic line selection can be regenerated if it stays semantically equivalent, but the deterministic content contract should still keep paired replays aligned when possible.

Cross-plan state ownership is explicit: Plan17 defines quest lifecycle; Plan18 selects valid player destinations; Plan19 defines authoring and provenance boundaries; Plan20 structures the graph and prose; Plan21 resolves context and memory gates; Plan22 routes effects. No plan may bypass another owner. Plan20 cannot mark a quest completed by closing a dialogue. Plan21 cannot invent an eligible location. Plan22 cannot turn a gate pass into a faction standing mutation. Integration should preserve these boundaries in APIs and save ownership.

### Hidden quest and clue discovery

A hidden enforcement investigation could begin when a player notices the same notice copied in two locations or hears an existing repeatable phrase. That trigger must be an authored discovery record consumed by the current discovery system. It should not require the player to inspect every map node. The hidden quest title can remain undisclosed until the clue is actually learned. Once discovered, it should appear in the journal with enough information to continue. If the clue is unavailable on a particular run, the hidden quest is optional content and cannot block main progression.

Discovery metadata should name its trigger type, exact record/location/encounter, repeat policy, first-heard behavior and fallback. Repeated exposure to the same source should not create duplicate quest instances. A second copy can reopen or deepen an existing case only if its stable identity differs and the quest owner supports that. An “ambient rumor” should use the existing RumorSystem if the story asks to propagate it; do not add a parallel rumor type or treat dialogue text as automatically propagated.

### Gate QA and accessibility matrix

In addition to the previous state cases, review player knowledge with case-sensitive source IDs, translation keys, a stale catalog alias, missing speaker display name, unknown location after a map reset, no active doctrine, repeated same-day radio reception, a clue received after quest abandonment, and quest reopen following an actual new action. Each should resolve to a neutral line, a clear blocked explanation, or a new valid case. No missing key should crash or display an internal ID.

Focus must move predictably as response lists change. Screen readers should announce disabled reason text and distinguish speaker from player option. High-contrast mode should differentiate known and hidden leads without relying only on a faction color. An option whose precondition fails between opening and selecting must revalidate before effect; if stale, return to the hub and explain what changed without consuming the player’s turn or supplies.

### Full dialogue state contract and failure recovery

A node evaluation result should be explainable as a set of true/false/unknown predicates and one selected authored node. Unknown is an explicit status because old saves and inaccessible source records are expected. If the current graph contract supports only boolean predicates, the authoring adapter must define which safe default means unknown; it must not conflate “not yet learned” with “false claim.” Multiple eligible branches require a stable authored priority or an explicit choice hub. They must not depend on unordered dictionary iteration. A selected branch that changes the world is revalidated immediately before its command.

The graph can represent linear scenes, hub-and-spoke conversations, short reconvergent branches, knowledge-gated questions, relationship/faction/location gates, repeated-visit text, failure reactions, player-action memory and quest updates. The first playable slice should use a hub with a small number of short branches. Relationship, faction, skill, location and emotional-state conditions are optional layers, not a checklist that every dialogue must implement. Add a gate only where it unlocks a genuinely different response or protects factual integrity.

Recovery matrix:

- Context snapshot stale before player selects: refresh graph, return to hub, preserve focus.
- Context changes during command: reject stale command, show current owner reason, do not spend resources twice.
- Source key exists but delivery missing: hide quotation and keep generic route.
- Character absent: hide live line; use a stored note only if one exists.
- Player declined publication: preserve private/unresolved state and permit exit.
- Doctrine changed: retain historical case anchor; offer a separately sourced update.
- Quest package retired: map active case to safe neutral closure or keep the legacy graph readable.
- Old save lacks choice: initialize as undecided/unknown, never infer a side.
- Faction access lost after acceptance: offer a valid alternate source or explain the block.
- Map destination no longer valid: remove its action marker and offer the quest’s explicit fallback.

A repeated visit must acknowledge both the quest state and the actual event chronology. Replaying a scene cannot award evidence twice. A failed check should identify what remains unknown and where to continue. It should not make the character speak as if they had been interrogated successfully. The player’s skill may change what they notice, not the source’s historical knowledge.

### Quality measures and playtest questions

Measure gate comprehensibility through direct questions: “What do you know now?” “What is still uncertain?” “Who authored this record?” “What can you do next?” “What happens if you leave?” A player need not remember internal doctrine names to answer. The UI should expose enough context to make choices informed, without turning every line into a legal disclaimer. Check the smallest screen scaling and longest localization string. Confirm keyboard and controller close/back routes and focus restoration. These reviews assess the conversation surface; they do not authorize a new relationship or faction-state owner.

## Plan 21 Closeout and Integration Course — Context, Memory, and Gates

### Complete scope receipt

Plan 21 defines how dialogue reads current campaign facts without becoming an authority. It gates on current doctrine, resolved action, source delivery, player knowledge, quest state, location availability, character presence/consent and any supported standing/relationship context. It separates absent, authored-but-undelivered, delivered, and learned sources wherever current owners expose those states. It remembers only player choices and source IDs required to resume a case; it does not persist transcripts, hidden loyalty, inferred guilt, or a new trust score.

Context is evaluated from a stable snapshot. Unknown is distinct from false. An old save defaults safely to undecided or unknown. A doctrine transition does not rewrite a historical case. A missing witness does not establish guilt. A skill check may reveal an existing clue but cannot turn a rumor into fact. Gates explain the next action without exposing secrets. Disabled choices preserve accessible exit and completion routes.

### Integration course

1. Enumerate predicates used by the selected Plan20 graph and identify the live read-only source for each.
2. Record true/false/unknown and stale-state behavior; remove redundant or decorative gates.
3. Ensure quest lifecycle and source memory use Plan17 and existing journal/knowledge/save owners.
4. Bind location gates to Plan18’s valid candidate projection, not static labels or UI controls.
5. Preserve historical source identity across doctrine changes, location-state changes and package updates.
6. Define refresh points and stale-command behavior before connecting effects under Plan22.
7. Test first visit, repeat visit, next-day visit, missing source, absent speaker, consent refusal, faction access change, old save, and reopened case.
8. Review visible blocked reasons, focus restoration, controller/keyboard parity, high contrast and localization.

### Acceptance and cut line

Every dialogue predicate has an owner, source field/API, safe false/unknown default, save/migration behavior and player-facing fallback. No panel owns hidden gameplay memory. A choice that becomes stale between display and selection is revalidated before command; it cannot consume time or supplies twice. If current owners cannot distinguish delivery from catalog presence, omit the gated line or request a narrow owner contract. The first slice needs only the actual report choice and source it uses; relationship/reputation/skill/emotional layers remain optional.

Closeout proof includes a truth table, old-save walkthrough, stale-context route, repeated-visit behavior, navigation lifecycle, and a short playtest in which players can explain what they know and what remains uncertain. The plan is integrated only when the context snapshot and save owner are recorded and the implementation tests pass under focused policy. Until then it remains a design contract. Rollback removes the contextual line variants but preserves the quest’s stable choices and source history.
