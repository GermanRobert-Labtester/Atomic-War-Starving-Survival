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
