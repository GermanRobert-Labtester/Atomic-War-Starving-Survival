# Expansion-series Plan 20 — Authored Dialogue Graphs and Prose

**Status:** Dialogue/content architecture proposal; documentation only.
**Numbering note:** This is Expansion Planning Wave 1, Plan 20.
**Purpose:** Provide a readable, validated representation for authored conversations and in-game prose that can start or advance quests through existing owners.

## 1. Writing goal

Dialogue should sound like people trying to solve a concrete problem under pressure. Distinct voice comes from attention, vocabulary, pace, omission, and what a person will or will not promise. New prose supports the quest and location architecture in Expansion-series Plans 17–19. This plan defines the conversation graph and authoring discipline; Plans 21 and 22 define contextual conditions and consequence routing.

The current source includes encounter choice systems and NPC memory dialogue templates. The inspected paths do not establish one general-purpose DialogueNode graph used by every conversation. Reuse the active encounter or dialogue owner if it already covers the target route. A new graph schema needs premise evidence, an explicit owner, and a live content consumer before implementation.

## 2. Maintainable node model

The requested node shape is a useful author-facing outline:

~~~text
DialogueNode:
    id
    speaker
    location
    conditions
    text
    player_responses
    effects
    quest_updates
    relationship_changes
    world_state_changes
    next_node
~~~

For implementation, make each field a typed reference or validated data value:

- id: stable dialogue node identifier;
- speaker: canonical actor ID or an explicitly supported narrator;
- location: canonical node/context ID, optional only where the host supports off-site conversation;
- conditions: bounded declarative conditions evaluated against an immutable context snapshot;
- text: localization key and optional authored tone variant;
- player_responses: ordered response records with stable IDs, text keys, optional condition, and target node;
- effects: typed effect request identifiers, not embedded code;
- quest_updates: commands to the canonical quest owner, with validation that the target quest/objective exists;
- relationship_changes: requests to the current relationship owner;
- world_state_changes: named consequences routed to their domain owner;
- next_node: explicit continuation or terminal marker, with no ambiguous default.

A response record may point to a node or a terminal result. If both the response and node define the same effect, validation should reject the double definition or specify one source of truth. Dialogue content cannot bypass quest validation by writing a status string directly.

## 3. Supported conversation shapes

- **Linear scene:** one entry and a fixed sequence. Use for briefings, acknowledgments, and authored exposition.
- **Hub-and-spoke:** a player can ask several questions in any order; each spoke returns to the same hub. Mark one-time spokes only when a real persistent owner records them.
- **Short branch that reconverges:** a choice changes tone, information, or a bounded local consequence, then both paths meet again. This is the default for side quests.
- **Relationship-based dialogue:** selects a line from a current relationship fact; it does not increment relationship merely because the line played.
- **Knowledge-gated dialogue:** reveals details only when a named knowledge owner reports the fact.
- **Reputation-gated dialogue:** checks current faction standing or access through its owner.
- **Faction-specific dialogue:** uses speaker, affiliation, and an explicit fallback if the faction actor or content is absent.
- **Location-specific dialogue:** binds a node to a canonical location and a reachable host trigger.
- **Repeated-visit dialogue:** may choose an alternate authored line when an existing memory/visit owner provides a stable fact. Cosmetic-only repetition can remain transient.
- **Failed-quest dialogue:** reacts to an actual Failed, Expired, or Abandoned quest fact and points to an authored recovery or closure.
- **Player-action memory:** references a saved event such as an accepted promise, refused trade, or delivered item. The source fact must be traced.
- **Skill-sensitive dialogue:** offers useful vocabulary or interpretation when a canonical skill query allows it; skill must not grant facts that the skill owner does not support.
- **Emotional subtext:** uses authored performance and wording tied to a supported character context. Do not invent a hidden emotional meter in the dialogue layer.
- **Quest-control dialogue:** starts, advances, blocks, completes, or resolves a quest by issuing a validated command to the current quest authority.

The graph validator should reject missing node targets, duplicate response IDs, invalid actors/locations, unreachable required nodes, unhandled condition branches, invalid quest references, and cycles without an intentional loop policy. Every conversation has an entry and a clear exit. Back and cancel behavior belongs to the existing host UI.

## 4. Authored scene sample

Provisional sample only; speaker and location names must be checked against current catalogs before use.

**Scene seed — “The Other Cup”**
Location: a shelter meal room after a ration count.
Speaker: a quartermaster who notices what is not entered on the board.

Opening: “You marked the second cup as returned. Nobody signed for it.”
Responses:

1. “It was for the night watch.” Opens a short line about keeping the watch fed; the branch returns to the hub.
2. “I thought it was waste.” Opens a short line about how the count is made; the branch returns to the hub.
3. “I don't know.” Leaves the question unresolved and exits without inventing a fact.

The first two responses can alter only the wording of the immediate scene in the MVP. If later content remembers the answer, it must create a supported quest, relationship, or memory consequence through Plan 22's owner routing. The character's personality is conveyed by attention to the count and practical concern for a night shift, not by a new statistic.

## 5. Authored prose families

The same content contract can support quests, field notes, location descriptions, notices, journal entries, radio lines, and short dialogue. Each prose record should name its speaker or author when known, setting, delivery trigger, text key, conditions, and any linked canonical IDs. Keep excerpt length suited to the UI surface; avoid making every discovery a long document.

For each new location, write a concise arrival impression, one discoverable detail, and a return-state variation only when the game can observe that state. For each character, record voice guidance: what they notice first, what they avoid promising, their practical vocabulary, their humor boundary, and how they speak under stress. These notes guide authored lines but do not become runtime behavior.

## 6. Minimum viable version and optional layers

**MVP:** One short conversation with stable node and response IDs, a linear opening, one reconvergent branch, one terminal exit, text keys, current-owner condition reads, and no cross-system effect unless the current host route can dispatch it safely.

**Optional expansion:** Hub-and-spoke scenes, repeated visits, skill and reputation gates, faction voices, more speaker and location variants, quest-control effects, voice/sound presentation, localization, graph preview and authoring validation. Ship each layer only with evidence for its state source and consumer.

## 7. Risks and acceptance

Risks include prose branches that cannot be reached, dangling quest updates, repeated lines caused by transient UI state, localization overflow, choices whose effects are unclear, and sprawling trees that are expensive to maintain. Prefer short reconvergent branches, stable IDs, condition coverage, explicit fallbacks, concise response labels, and authoring lint.

Acceptance for an implementation package: every node and response resolves; the graph has known entry and exit paths; all condition branches are reachable or intentionally hidden; quest updates route through the quest owner; dialogue effects are applied once; localization keys exist; controller and keyboard flows can exit; and the scene works when an optional character or location is missing.

## 8. Integration course

**Phase 0 — Schema and owner audit:** Inspect current encounter, NPC memory, radio, journal, quest, and dialogue consumers. Find the active host trigger and confirm whether an existing graph contract already owns the content. Read live claims.

**Phase 1 — Draft content before runtime:** Write one scene in a node table, then check voice, branch purpose, required facts, fallbacks, text length, and duplicate canon. Do not create a loader for a single sample until a real consumer is identified.

**Phase 2 — Define typed data:** Agree on stable identifiers, conditions, response transitions, terminal representation, text keys, and schema version using the current canonical data format.

**Phase 3 — Connect to owners:** Register and resolve through the existing Core authority and host route. Keep presentation in Godot and Core logic engine-free. Quest and other state changes pass through owner commands.

**Phase 4 — Validate and verify:** Run the focused catalog and graph validation and affected conversation/quest tests after a path claim. Cover every edge, each fallback, duplicate invocation, localization fallback, and close/back lifecycle.

**Phase 5 — Handoff:** Record final schema, content IDs, host entry point, owner routes, focused commands, results, limitations, and shared seams left untouched.

## Continuation pass 2 — complete reconvergent scene

The following scene expands The Other Cup into a small graph. It is a writing and schema example only. Quest and response IDs are provisional, and each must be checked against the live catalogs before implementation.

### Entry node

~~~text
DialogueNode
    id: meal_room.cup_open
    speaker: provisional_quartermaster
    location: existing_meal_room_context
    conditions: speaker_present AND conversation_available
    text: "You marked the second cup as returned. Nobody signed for it."
    player_responses:
        - response_id: ask_about_watch
          text: "Was it for the night watch?"
          next_node: meal_room.cup_watch
        - response_id: ask_about_count
          text: "What does the count actually tell us?"
          next_node: meal_room.cup_count
        - response_id: leave_uncertain
          text: "I don't know."
          next_node: meal_room.cup_close
    effects: none
    quest_updates: none
    relationship_changes: none
    world_state_changes: none
    next_node: none
~~~

The entry has no hidden reward. Its three responses are different player intents, and each destination exists in the graph.

### Short branches

The watch spoke answers the practical question: “Two people were out past the gate. One came back carrying the cup. The other came back carrying a sleeve full of bolts.” It then returns to the shared close node.

The count spoke answers the record question: “It tells me what left the shelf. It doesn't tell me who needed it, unless somebody writes that down.” It also returns to the shared close node.

The uncertain response goes directly to the close node. It remains a valid choice and does not set a false fact.

### Reconverged close and quest handoff

At the close node the quartermaster says: “There's a crate behind the dry-goods shelves with no entry beside it. If you want to settle the count, bring me a second witness.” The player may accept the provisional Unentered Shelf quest through a validated quest-owner command, ask to hear the request later, or leave.

Acceptance is a quest consequence and belongs to Plan 22's owner-routing contract. If the quest definition is missing, the response falls back to “I'll find the entry before I ask you to carry anything.” The scene exits normally and does not create a phantom quest.

The response graph is compact: one opening, two optional information spokes, one reconvergent close, and one validated quest command. The hub does not need a saved spoke-visit counter unless a later scene reads it through an existing owner.

### Voice and readability notes

The quartermaster notices discrepancies before motives. Avoid writing them as a suspicious bureaucrat; their method protects tired people from being accused on incomplete records. Keep labels concrete (“returned,” “signed,” “shelf”) and let emotional meaning come from what remains uncounted. Response labels state player intent without announcing hidden effects.

At 1920x1080 and smaller UI scales, keep line chunks short enough that the response list remains visible. If the dialogue panel cannot fit four responses and readable text, reduce the number of responses or use a scrollable panel with retained focus. A line should never be the only clue to an objective; the quest log receives a concise summary through the quest owner.

### Graph authoring review

For every authored conversation, create a node/edge table and verify:

- one valid entry and at least one intentional terminal exit;
- all next-node IDs resolve and all required nodes are reachable;
- every optional gate has a neutral or unavailable fallback;
- all response commands are legal for their target owner;
- response effects have a visible outcome label where useful;
- there are no unbounded cycles or implicit “continue” edges;
- every character line follows its voice notes and continuity constraints;
- localization keys and fallback language exist;
- close/back/cancel works without dropping an accepted command.

The same review applies to radio, journal, and environmental prose only where those surfaces use the same graph. Do not force a linear note or one-line bark through a conversation framework that adds no value.

## Continuation pass 2 acceptance

This scene is structurally ready for content review when every response reaches the intended line, both spokes reconverge, no branch invents a world fact, and the quest offer uses a supported command with a missing-definition fallback. It is not a canonical shipped scene.

## Continuation pass 3 — scene packages, voice, and content-scale rules

### Scene package template

Each authored conversation should be delivered as a scene package that a writer and implementer can review together:

~~~text
ScenePackage
    scene_id
    purpose
    entry_trigger
    supported_locations[]
    supported_speakers[]
    required_context_facts[]
    entry_node_id
    nodes[]
    terminal_nodes[]
    consequence_scope
    missing_content_fallback
    text_keys[]
    voice_notes
    continuity_references[]
    accessibility_summary
~~~

The data loader may not use this exact wrapper if a current catalog already groups conversations differently. The required information remains: why the scene occurs, how the runtime reaches it, which facts gate it, where it can appear, how it ends, what it can change, what happens when dependencies are absent, and how its text is reviewed.

Scene purpose is written for the player experience, not for code: “The player decides whether to tell the route clerk that the bridge is unsafe before the next convoy leaves.” Entry trigger names an existing event or deliberate interaction. An unavailable speaker has a safe response that does not impersonate them. A scene with no consequence is still valid if it reveals useful information; it does not need a fake reward.

### Conversation structure choices

Use the smallest shape that makes the exchange clear:

**Linear scene** — useful for a first briefing or short acknowledgment. Keep one entry, a short sequence, and a clean exit. A skip or fast-forward may be provided by the host if other conversations support it.

**Hub-and-spoke** — useful when the player can ask about route, supplies, and a character's concern in any order. Each spoke returns to a stable hub; the hub does not require a persistent viewed flag unless a later event reads it.

**Reconvergent choice** — useful for the majority of side quests. The player may state suspicion, sympathy, or uncertainty. The speaker answers that intent, then returns to the same actionable request. Persist only the response that later content must remember.

**Relationship scene** — useful when an existing relationship fact changes which topic is safe to raise. If no relationship fact exists, the writer can still convey warmth or distance in the line without inventing a stat.

**Quest-control scene** — useful when a conversation accepts, advances, or closes a task. Put the confirmation response at a clear commitment point. Before it, the player can ask questions or decline; after it, the owner returns the actual quest status.

**Investigation scene** — useful when the player brings evidence. The speaker should distinguish observation from conclusion and allow “I don't know” as an honest response. A wrong theory can have a consequence, but the evidence objective remains governed by the quest owner.

Do not combine every structure in one graph. A large hub with many conditionally gated spokes is harder to translate, test, and navigate than two short scenes with a clear purpose.

### Character voice sheet

For each major speaker, authors record:

- **Role:** what work they do and whom their work serves.
- **Attention:** the first detail they notice in a room or report.
- **Vocabulary:** words they use for tools, people, risks, and uncertainty.
- **Boundary:** what they will not promise, reveal, or joke about.
- **Care signal:** what practical action shows affection, fear, loyalty, or grief.
- **Pressure change:** how sentence length or directness changes under stress.
- **Conflict style:** what they do when they disagree.
- **Recovery:** how they speak after a failed quest or a harmful choice.
- **Variation range:** how far tone can change before the voice becomes a different person.

Example voice notes for the provisional quartermaster: notices missing signatures and damaged containers; uses quantities but admits what a count cannot prove; avoids calling a person dishonest without evidence; shows concern by setting aside a dry place for a returning party; under pressure asks one question at a time. This offers more useful guidance than “stern but caring.”

### Location prose contract

Location text should orient and reward attention in three layers:

1. **Arrival line:** what the player can see immediately and what makes the site recognizable.
2. **Discoverable detail:** one optional object, sound, smell, or contradiction that offers a clue or mood.
3. **Return variation:** a line that changes only when an existing owner fact proves a meaningful change.

An arrival line is not a gameplay guarantee. If it says “the western gate is open,” the gate owner and map state must agree. Avoid directions that require a route not shown on the map. A discoverable detail needs a real interaction or encounter consumer; decorative text may remain cosmetic. A return variation must have a durable source or remain transient.

Sample field note for a location not yet named in canon: “The wind turns the hanging tin sign inward. When it turns back, the arrow points at a wall.” This is atmospheric only unless an authored interaction uses the sign as a clue.

### High-volume prose production

High-volume dialogue is produced in small voice-specific batches. Each batch should vary intent, sentence rhythm, and information, not just swap a noun:

- arrival acknowledgments: short and informational;
- repeated-visit variants: only where the source fact supports revisiting;
- calm-state lines: practical, with ordinary conversation;
- pressure lines: shorter and more urgent without shouting every time;
- refusal lines: preserve player agency and explain immediate cost;
- recovery lines: acknowledge the event without erasing failure;
- late-arrival lines: refer to the actual expiry or changed location fact.

Each batch needs a source-fact matrix, unique text keys, duplication scan, voice check, UI line-length check, and continuity review. A batch that repeats the same emotional claim in five word substitutions should be cut to one stronger line and one purposeful variant. Content throughput is measured by accepted reachable lines, not raw row count.

### Localization and interaction requirements

Keep node IDs and response IDs stable across translation changes. Do not encode meaning in punctuation, line breaks, or color alone. Store short response labels separately from longer accessibility descriptions if the existing UI supports both. A response may include a meaningful cost; state it before commitment in plain language.

For keyboard and controller users, focus enters on the first available response, remains visible after a branch returns to a hub, and moves predictably when a response becomes unavailable. Cancel exits or backs out consistently with the current conversation surface. Screen readers should announce speaker, line, response options, disabled reasons, and any quest status result. An effect must not be hidden behind a decorative portrait change.

## Continuation pass 3 acceptance

The scene package is ready to scale when each conversation has a purposeful structure, distinct voice guidance, valid entry and exit, short and translatable prose, source-backed variants, accessible response handling, and no effect embedded in presentation copy without an owner contract.

## Continuation pass 4 — scene production kit, branch accounting, and sample exchange

### Scene kit for writers and integrators

Each scene package should include a one-sentence dramatic purpose; entry speaker and location; participant availability; context facts it reads; node and response IDs; branch/reconvergence map; variants with their source facts; quest updates requested; consequence requests delegated to the consequence plan; exit route; accessibility notes; and localization limits. The authoring kit should render a readable scene outline and a machine-checkable graph from the same source. Do not maintain a spreadsheet copy that can drift from the authoritative dialogue catalog.

A compact scene can use the following shape:

1. Entry line establishes the immediate subject and speaker attitude.
2. Hub offers two or three questions with distinct information value.
3. Each answer branch grants knowledge, surfaces a concern, or exits; cosmetic branches reconverge quickly.
4. Any meaningful commitment gets a confirmation line that states the practical consequence.
5. The return-to-hub line changes only when a source fact supports that change.
6. Exit text leaves the player with a clear next action or no action.

For a short branch, each response needs a clear label, a consequence preview if applicable, and an accessible unavailable reason. A branch that cannot affect information, tone, relationship, quest, faction access, or world state should probably be a line variant or a shorter hub option rather than a large graph fork.

### Revisit branch for the shared Unentered Shelf fixture

Keep this as a second beat of the existing clerk/nurse/mechanic side-quest fixture, rather than introducing another key-custody story. The clerk opens with: “You came back. Is there another count, or only another account?” The question responds to the quest's current evidence state. The player can answer “I found a route to the depot record,” “The nurse remembers the promise, but no second witness,” or “I have no new proof.”

If the evidence route is eligible, the first response sends a validated quest command to Plan 22 and exposes its canonical destination through Plan 18. If the witness memory is sourced but incomplete, the second advances an evidence-gathering objective while preserving uncertainty. The final answer leaves the quest open or presents the explicit local-hold resolution, depending on the accepted quest state. None of the responses changes inventory until the player later chooses a physical transfer and the inventory owner accepts it.

On a confirmed local hold, the clerk says: “Then I will mark the blankets for this room and keep the destination question open.” That line is selected from the accepted outcome fact. If the owner refuses or the old save lacks the outcome fact, the neutral line remains: “The count is still open. I haven't moved anything.” The second visit changes wording, objective availability, and the player's understanding without inventing a second NPC or an unowned location flag.

### Branch budgets and reuse

Use an authored branch budget per scene: number of unique nodes, unique responses, meaningful outcomes, and reconvergence points. The initial vertical slice should favor short branches that reconverge, with a small number of major splits reserved for faction, relationship, or ending consequences. The budget is a production control; it is not a runtime limit unless the current dialogue host proves one is necessary.

Reusable patterns include a greeting plus hub, clue examination, request negotiation, character check-in, faction briefing, location arrival, aftermath, and refusal/fail-forward scene. Reuse the structure and validation rules, not identical prose. Speaker-specific vocabulary and purpose must remain distinct. A template may provide node roles and condition slots; each scene still owns its text and intent.

### Graph and prose quality gates

For every response, check that the target node exists, the response can become available under a reachable fact set, branch edges are valid, exits are deliberate, and any quest/effect request has a known recipient. A cycle must be intentional and exit-safe. A hub must not present a response that remains selectable after its outcome has made it misleading. Repeated visits should not duplicate irreversible effects.

For prose, use short spoken lines and a separate optional detail where needed. Keep speaker identity clear in the transcript, avoid essential information encoded only through performance cues, and ensure response labels still make sense when translated without adjacent prose. Maintain voice sheets with concrete diction, sentence rhythm, what the speaker notices, what they avoid saying, stress behavior, and a neutral fallback. Text variants should differ because the speaker or situation has changed, not to inflate line counts.

## Continuation pass 4 acceptance

The scene kit is ready when writers can create a complete scene from one graph package, its sample patterns remain reusable without cloning voice, branches have a deliberate budget and reconvergence, and every line that implies an action is backed by a reachable response and an owner-routed effect.

## Continuation pass 5 — runtime contract, authoring profile, and scene portfolio

### Keep the graph authored and the scene result current

The graph definition is stable authored content. Its nodes, edges, response labels, condition references, localization keys, and owner-command references are reviewed and versioned with the content bundle. At runtime, a short-lived scene projection combines that definition with the current speaker availability, location context, quest phase, memory facts, faction access, and owner-approved outcomes. The projection is disposable: close the scene and it can be recomputed from the graph and canonical state.

This boundary supports repeatable writing without copying campaign state into each node. A response can point to a quest transition, a relationship request, or a location clue using a stable typed reference; it does not carry a mutable campaign record inside the graph. The active quest, relationship, map, or faction owner remains the source of truth. Plans 19, 21, and 22 define the linked content, context, and effect boundaries.

Current code evidence shows dialogue-shaped interactions in questline choices, encounters, faction war snippets, and NPC memory tone selection, but no shared general-purpose DialogueNode runner was identified during this audit. That is a reason to keep the proposed schema small and adapter-friendly, not to assume every existing interaction must be migrated. Before building a common runner, compare its minimum requirements against each current caller and show a real duplicated contract that the shared runner removes.

### Minimal scene definition profile

Use one validated scene bundle with a stable scene ID and a bounded set of node kinds. A node may provide:

- stable node ID and speaker reference;
- optional location reference and entry/exit role;
- text key and optional authored variants;
- scene/node conditions referencing Plan 21 predicates;
- player responses with stable response IDs and text keys;
- optional consequence-preview key;
- owner-command references from Plan 22;
- next-node or explicit scene-exit target;
- authoring notes kept outside runtime authority.

Responses are data edges, not executable scripts. A response contains its condition, visible/disabled presentation policy, next-node edge or exit, and zero or more reviewed command references. Keep command argument schemas in the owner contract, rather than allowing arbitrary key/value payloads that turn content into unreviewable code.

The first runtime profile should support a linear scene, a hub with two or three spokes, short branches that reconverge, and a clean exit. Add relationship, faction, skill, location, repeat-visit, and quest-phase variation through Plan 21's condition references. Do not introduce a general scripting language, recursive variables, or arbitrary callbacks merely to avoid writing a few explicit nodes.

### Scene pattern contracts

| Pattern | Required graph shape | Main risk | Minimum fallback |
|---|---|---|---|
| Linear scene | Entry, ordered nodes, one explicit exit. | Missing speaker/location interrupts the sequence. | Neutral arrival or a journal note, then exit. |
| Hub-and-spoke | Hub, bounded questions, deterministic return target. | Hub repeats stale choices after an effect. | Rebuild hub from current owner facts. |
| Reconverging branch | Distinct information or tone branches, shared continuation. | Branch labels imply consequences that did not occur. | Separate acknowledged outcome from requested effect. |
| Relationship scene | Relationship gate or memory fact plus neutral alternative. | Missing memory is mistaken for rejection. | Neutral greeting and a public-information route. |
| Knowledge scene | Explicit evidence source and player-facing clue. | Critical fact exists only behind one check. | Base line communicates a useful clue. |
| Faction briefing | Faction access fact and readable refusal/summary. | Reputation changes during open scene. | Revalidate before commitment and refresh the hub. |
| Location arrival | Stable location reference and discovery/availability rules. | The graph reveals an undiscovered destination too early. | Offer a clue or wait state through the map owner. |
| Aftermath/revisit | Quest or world result selects the current line. | Dialogue invents memory from visit count. | Neutral line when the result is unavailable. |

Templates should standardize edge labels, field order, reviewer prompts, and test fixtures. They should not stamp identical greetings and emotional cadence across unrelated characters. A template should make the writer's choices visible, not make those choices on the writer's behalf.

### Unentered Shelf scene portfolio

Use the existing provisional Unentered Shelf fixture as one small portfolio with distinct conversational purposes. It is not a new permanent location proposal. Bind its references to real canonical content only after source and data verification.

**Initial briefing, clerk:** “I can count what came in. I cannot count the people the route never named.” The player may ask about the tally, the promised destination, or what would make the clerk comfortable assigning the blankets. These choices reveal different knowledge but do not silently transfer supplies or accept a quest.

**Nurse's account:** “The promise was made in a crowded room. I remember the hands raised. I do not remember a door.” A knowledge gate may let a player with the relevant skill ask about the nurse's careful distinction between a spoken promise and verified receipt. The base option still communicates that the destination is not confirmed. The skill variant improves interpretation; it does not become the only path.

**Mechanic's route check:** “That depot road is passable on paper. The culvert is the part that keeps changing its mind.” This line may surface a clue to a verified location or expedition requirement. If no current map target can satisfy the quest, the response opens a clue-led/delayed route as specified by Plan 18. It never claims that the road is already selected for the next expedition.

**Return after a player decision:** If a canonical owner confirms local allocation, the clerk says, “Then I will mark these for this room. The other names stay on the list.” If the route remains pending, the neutral response is, “The count is still open. I have not moved anything.” If the destination is confirmed but a transfer is rejected, the scene says what was confirmed and keeps the physical allocation unresolved. Plan 22 supplies these result branches; this plan only authors the graph and copy.

The sample portfolio covers briefing, evidence review, location clue, and aftermath while reusing one set of characters and one central question. It demonstrates variety of scene purpose without creating three new quests or duplicating an existing plot.

### Branch sizing and content review

For each scene, set a target budget before prose expansion: total nodes, response count, meaningful branch count, maximum depth, number of owner commands, and expected return visits. These values guide review and production estimation; they are not global runtime limits. A small conversation may need more nodes when accessibility requires explicit confirmation, while a branching finale may justify a larger graph after continuity review.

Mark each branch as cosmetic, informational, local, quest, relationship, faction, world, or ending in editorial metadata. Require an explicit reason for a branch that creates unique prose but no meaningful information, voice, or state distinction. Conversely, do not collapse two branches that reflect genuinely different speaker knowledge just to reduce node count.

Review each edge from both directions: can the player reach it under a valid context, and does every response have a valid continuation or exit? Include absent-speaker, absent-location, unknown-condition, repeated-visit, stale-fact, and owner-rejection cases. Writers should see a plain diagnostic naming scene/node/response IDs and a reason code; runtime diagnostics should avoid dumping full hidden context or player history.

### Localization, transcript, and input requirements

Give nodes and responses stable localization keys independent of the text itself. Do not concatenate translated fragments to create a sentence. Preserve speaker and response IDs in the transcript so screen readers and history views can identify who spoke and which choice was selected. Essential information must be available in text and cannot depend on animation, portrait expression, audio, color, or an unlocalized gesture.

Every scene has a deterministic keyboard/controller path: enter, move among available responses, inspect any consequence preview, confirm, back out, and close. When owner facts update while a scene is open, refresh the available responses without stealing focus; if the selected response becomes invalid, move focus to the nearest valid response and announce why. Avoid time-limited dialogue input unless it is an explicit, tested quest mechanic owned by the current system.

### Core and expansion placement

The graph schema, validators, fallback conventions, and first reusable patterns are core architecture if adopted. Optional scene bundles, new characters, and faction-specific branches may be expansion content. Expansion content cannot be required to complete the base game's critical path, satisfy an existing quest's only exit, or resolve a reference that the base bundle claims to guarantee. Build and inspect both enabled and disabled catalog states.

## Continuation pass 5 acceptance

The dialogue graph package is ready for a bounded implementation claim when the minimal profile has stable IDs and explicit exits, its node patterns preserve existing owner boundaries, the shared fixture has reachable neutral and contextual routes, every consequence has a Plan 22 reference, and transcript/localization/controller requirements are testable without imposing a broad migration of existing dialogue-shaped systems.

## Continuation pass 6 — authoring worksheets, graph rehearsal, and extended scene sample

### Four artifacts before a conversation is called production-ready

For a consequential conversation, keep four small artifacts together:

1. **Scene intent card:** why this conversation exists, what the player knows on entry, what can change, what the scene must not claim, and how it ends.
2. **Graph inventory:** stable scene/node/response IDs, edges, conditions, exits, and command references.
3. **Voice sheet:** speaker-specific word choice, rhythm, attention, concealment, stress behavior, and neutral fallback.
4. **Review fixtures:** reachable context snapshots for each intended branch, missing optional content, unknown facts, repeated visit, and owner rejection.

These artifacts are an authoring discipline, not four runtime stores. Keep only content required by the existing data authority in the shipped bundle. External editorial notes can explain why a line is written a certain way; they must not become a hidden source of runtime truth.

The scene intent card should make its dramatic question answerable in one sentence. For the Unentered Shelf evidence review, the question is: “What can the shelter responsibly do while the record of a promised delivery remains incomplete?” The scene must not tell the player who owns the blankets, whether a neighbor received them, or whether a route is safe unless current owner facts prove those claims. It may make the practical stakes understandable through the speakers.

### Node and response inventory for a reviewable scene

Review the graph in a compact node table before filling every line:

| Node role | Entry condition | Player response | Exit/next edge | Owner request |
|---|---|---|---|---|
| Scene entry | Clerk available; current task state known or neutral fallback possible. | Review the record; ask about witness; leave. | Three bounded branches. | None. |
| Record review | Record interaction or known clue exists. | Ask what the mark proves; ask what remains unknown. | Reconverge at evidence summary. | Quest evidence request only if an interaction is completed. |
| Witness account | Nurse or approved alternate source available. | Ask about the remembered promise; ask about the destination. | Reconverge with separate facts. | Memory/evidence query, not a write. |
| Route clue | Mechanic available or route record exists. | Ask where the road leads; ask what blocks certainty. | Return to evidence summary. | Map clue request only after owner validation. |
| Evidence summary | At least one route completed or neutral route used. | Hold stock; request verified delivery path; decide later. | Plan 22 result node or exit. | One reviewed command at a time. |
| Result | Owner has returned a typed result. | Continue, retry if valid, or close. | Rebuild hub from fresh context or exit. | No direct mutation. |

The table exposes missing entry, exit, and fact ownership before writers spend time on dozens of alternate lines. Add node variants only when a real fact changes speaker knowledge or player understanding. Avoid cloned nodes whose only difference is an adjective unless they serve a meaningful voice or localization purpose.

### Extended evidence-review scene

This is provisional sample prose for the same fixture and should not be bound to canonical IDs without a fresh content collision and source audit.

**Clerk, opening after the player returns with one piece of evidence:** “I have a note with a number on it and a shelf with fewer bundles than the note expects. Those are two facts. I need a third before I call them a delivery.”

Player responses:

- “Read the note aloud, exactly as written.”
- “Tell me what the nurse remembers.”
- “Show me the route the mechanic checked.”
- “I’m not ready to decide.”

**Record branch:** The clerk traces the worn line with one fingernail. “This says six were set aside. It does not say who signed for them.” If the player has the verified record-reading fact, a second line may add: “The mark after the number is a correction, not a name.” If that fact is unknown, the neutral line remains: “The ink runs through the last mark. I will not guess at it.” Both branches return to the evidence summary; neither awards evidence merely for viewing the conversation.

**Witness branch:** The nurse folds the paper once, then stops before making a crease. “I remember the request. I remember people agreeing that someone should carry a share. I remember the room was noisy.” The player can ask whether she saw the bundles leave. “No. I saw the request. I did not see the handoff.” The scene records only the sourced witness statement if the quest owner accepts the interaction; it does not convert memory into proof of receipt.

**Route branch:** The mechanic answers from a grease-marked sketch. “The lower road is drawn as open. That tells me what someone expected on the day this was made.” Asked whether it is safe now: “I can check a route when the map says there is a route to check. I cannot promise what the water did after.” Plan 18 decides whether an expedition can be offered; the character does not override the map.

**Decision branch:** The clerk offers three clear responses: reserve the counted bundles for this shelter; wait for a verified destination; or request a real route/evidence lead. Before a consequential response, the conversation explains the practical commitment in plain language. The selected answer becomes a request, not a success line.

**After an accepted local reservation:** “I can account for these here. I still cannot write a delivery receipt for the other shelter.” This line appears only after the current resource and quest owners confirm their parts. If stock transfer is rejected, the alternative is: “Then the count stays open. Nothing moved while we were talking.” If the result is unknown, use: “I need the stores ledger before I can call this settled.” The scene preserves a safe exit in all cases.

This sample uses one scene with several information routes and a meaningful commitment. It keeps the nurse's recollection, the record's claim, the mechanic's route knowledge, and the physical stock result separate. It also demonstrates that prose can create emotional weight without introducing a hidden trust meter.

### Voice sheets as constraints on revision

For each speaker, write observable voice rules:

- **Clerk:** counts, qualifiers, and practical distinctions; distrusts unsupported totals; stress makes sentences shorter, not crueler.
- **Nurse:** distinguishes what she saw, heard, and remembers; uses concrete care language; avoids diagnosing motives.
- **Mechanic:** speaks in tolerances, wear, and conditions; resists absolute safety claims; humor is dry and brief.

These are provisional writing guides for the fixture, not canon biographies. A future author must verify whether the roster already defines these characters and adjust the sheets to the existing voices. Do not turn “stress behavior” into a hidden emotional state that changes access unless an owner-backed fact supports that variation.

### Graph rehearsal and branch matrix

Before implementation, rehearse the scene with a table of contexts:

| Fixture | Expected visible choices | Expected scene result |
|---|---|---|
| No evidence yet | Ask about record, ask a witness, leave. | No commitment; quest remains available. |
| Record inspected, witness absent | Review record, request route clue if available, wait. | Unknown destination stays unknown; an alternate route remains. |
| Clue discovered, location not visible | Review clue, request expedition opportunity, leave. | Plan 18 returns clue-led or deferred status. |
| Destination visible and eligible | Request verification expedition or choose local hold. | Start path revalidates target; local hold separately validates stock. |
| Quest expired | Review actual failure and any supported recovery. | No old success response remains selectable. |
| Resource command rejected | Review evidence, retry only if owner says eligible, leave. | No transfer success wording or duplicate request. |
| Owner fact unavailable | Ask public questions, use neutral fallback, leave. | Scene remains complete and does not strand a critical quest. |

Validate the graph as a directed structure: every entry reaches an exit, every mandatory node has an incoming path, each response's condition is satisfiable, repeated hubs refresh from owner facts, and one-shot command nodes cannot be triggered twice. Diagnostic output should identify stable IDs and failing edges, not quote private dialogue.

### Scope tiers for authored conversation

Classify each proposed scene before production:

- **Tier 0, presentation:** line variation with no state effect.
- **Tier 1, information:** exposes a supported fact or clue but does not mutate durable state.
- **Tier 2, quest request:** asks a quest owner to offer, accept, advance, or resolve a task.
- **Tier 3, cross-owner commitment:** sequences a quest request with inventory, faction, location, or relationship effects.
- **Tier 4, campaign resolution:** changes a major world/ending outcome and requires cross-quest continuity review.

The first integration slice should be Tier 1 or a tightly bounded Tier 2. Tier 3 needs explicit partial-result behavior. Tier 4 must not be smuggled into an ordinary dialogue effect list. Production estimate and focused verification grow with scope tier; prose length is not a reliable proxy.

## Continuation pass 6 acceptance

The scene package is ready when a scene intent card, graph inventory, voice sheet, and branch fixtures tell one consistent story; the sample distinguishes observation from inference and request from confirmed result; every graph path exits safely; and proposed scene scope matches the current owner seams and available verification.

## Continuation pass 7 — scene catalog, expedition briefing, and prose production standards

### Dialogue use-case catalog

Each conversation should be classified by what the player is trying to learn or decide. These shapes may share a graph pattern while preserving speaker purpose:

| Scene use case | Entry | Main player action | Typical branch behavior | Required exit |
|---|---|---|---|---|
| Linear briefing | Quest/character owner reports an active situation. | Hear a short explanation. | One or two optional clarifying lines. | Accept, defer, or close. |
| Hub conversation | Speaker and context are available. | Choose a topic. | Spokes return to hub after information is consumed. | Explicit leave response. |
| Evidence review | One or more clues are owner-backed. | Compare what each clue proves. | Reconverge before a decision. | Keep investigating or choose a supported disposition. |
| Relationship check-in | Existing memory/relationship fact exists. | Ask about the relationship or prior event. | Voice variant; consequences only through owner. | Warm, neutral, or bounded exit. |
| Faction negotiation | Faction context and access are known. | Review terms or respond. | Distinct accepted/refused/deferred outcome. | Result comes from faction/quest owners. |
| Expedition preparation | A selected opportunity set exists. | Confirm target, ask about a clue, or leave. | Location/route facts may change; revalidate. | Start only through existing expedition command. |
| Location arrival | Canonical site is reached. | Inspect, speak, or leave. | Local knowledge branches; no fake travel. | Return to current exploration/expedition flow. |
| Consequence aftermath | Owner has confirmed a result. | Review what changed. | Branch on accepted result; optional callback. | Close without replaying one-shot effect. |
| Environmental discovery | A clue interaction is available. | Inspect or ignore. | Knowledge branch from the discovery owner. | Journal/map update only after confirmation. |
| Failure/recovery | Failure or expiry is confirmed. | Choose a recovery action or stop. | Recovery is a separate route with its own objective. | Preserve original failure in history. |

Use these as an authoring catalog, not as ten new runtime scene classes. The graph may remain a set of stable nodes and edges while the authoring UI supplies templates for common shapes. A scene type should be promoted into code only if source evidence shows repeated structural behavior that cannot be expressed cleanly in existing data.

### Expedition-preparation scene: short branch, useful information

This sample occurs before a legal expedition start and assumes the selected destination was already exposed by the current map/expedition path. It does not make the selection itself.

**Clerk:** “The board has three routes on it. One is clear. One is old. One is a line someone drew around a place they have not seen.”

Player responses:

- “Show me what the map confirms.”
- “Ask the mechanic about the old route.”
- “Why is the marked place still only a rumor?”
- “Leave the board for now.”

**Confirmed-route branch:** The clerk points at the route status shown by the map owner. “This one is known and open today. That is what the board can promise.” If a route changes before dispatch, Plan 22's operation result and the existing preview/start path refresh the scene; the dialogue cannot lock in a stale “open” claim.

**Old-route branch:** The mechanic turns a folded chart to the blank side. “The line was useful when it was drawn. It is evidence that somebody went that way, not evidence that you can do it now.” A skill-gated response may ask what visible wear means, but the base line still distinguishes historical route evidence from current route readiness.

**Rumor branch:** The clerk answers, “A rumor can tell you where to ask. It cannot put a safe road on the map.” The player can request a clue-led opportunity if the quest/location owners allow it. If no clue source exists, the option returns to the hub with a truthful “not enough evidence” result.

**Exit:** “I will leave the routes as they are until you are ready.” Leaving the board does not advance time or accept an expedition. Only the existing expedition start command can commit the selected target.

This scene supports linear briefing, hub-and-spoke questions, location-specific information, a knowledge-gated skill variant, repeat-visit copy when route facts change, and a consequence-safe exit. It demonstrates a small amount of prose serving several systems without granting the dialogue graph authority over them.

### Authoring line cards

For every line that conveys an operational fact, complete a line card:

- **Speaker intent:** What does the speaker want the player to understand or do?
- **Evidence basis:** Which current owner fact permits this line?
- **Epistemic wording:** Is this confirmed, reported, inferred, rumored, or unknown?
- **Player action:** Does the line invite a command or simply offer information?
- **Fallback:** What truthful line appears when the fact is absent or unknown?
- **Revision sensitivity:** Would changing this line alter a reasonable player expectation?
- **Localization note:** Does the line depend on wordplay, line order, or untranslated fragment concatenation?

Do not use exposition to backfill a missing data route. If a line says “the faction opened the checkpoint,” the faction/location owner must already report that access. If no such state exists, revise the text to an attributed rumor or remove the operational claim.

### Branching dialogue patterns and their cost

Useful graph patterns include:

- **Linear scene:** low branch cost, useful for a short factual update.
- **Hub-and-spoke:** moderate authoring cost; requires refresh after owner state changes.
- **Short branch/reconverge:** moderate prose cost; preserves one continuing objective.
- **Knowledge fork:** adds an optional interpretation while preserving base information.
- **Relationship variation:** requires sourced memory/relationship facts and neutral copy.
- **Reputation fork:** requires separate public and restricted routes.
- **Faction-specific scene:** optional speaker/context availability plus expansion fallback.
- **Location-specific response:** map/scene availability, discovery, and travel truth.
- **Repeated-visit scene:** canonical result/memory fact; no visit-count cache.
- **Failed-quest response:** confirmed failure cause and a different recovery route if one exists.
- **Skill-specific response:** skill owner exposes a question or action; critical clue remains accessible.
- **Emotionally guarded line:** authored tone or sourced relationship fact; no hidden arbitrary meter.
- **Quest-starting dialogue:** request to quest owner; acceptance copy waits for result.
- **Choice consequence scene:** result observed later through the actual outcome owner.
- **Ending scene:** high-cost continuity package and explicit campaign resolution contract.

No branch is free. Estimate its unique text, entry conditions, player-facing reason, failure case, return path, owner command, localization burden, and save/replay fixture. Reuse the structural graph with new voice and facts rather than copying a whole dialogue tree and replacing only names.

### Personality, lore, and location information without state invention

Character personality is expressed through attention, values, and word choice: the clerk notices counts and signed receipts; the nurse distinguishes witnessed care from rumor; the mechanic distrusts maps that hide their date. These are voice rules for this provisional fixture, not new canonical roster records. Bind to existing characters only after checking current canon and memory data.

Location lore has a similar boundary. A location description can establish sensory atmosphere and historical context, while its interactive state is read from the canonical map/site owner. “The stairwell smells of wet plaster” is authored atmosphere. “The stairwell is flooded and impassable” is a live route fact if it affects expedition eligibility. “A faded arrow points toward the old market” is a clue only if the discovery interaction can surface it.

Shelter lore can explain routines, work, and shared memory without creating a hidden shelter-culture simulation. If the project later wants staffing, morale, public works, or resource outcomes from those details, each requires a current system owner and integration proof. A character can have a distinct personality before the game tracks a new statistic.

### Prose production and QA budget

Estimate a scene by authored unique strings, node count, response labels, condition-dependent variants, result variants, speaker count, localization keys, and evidence links. Count written lines once even when they appear in several test fixtures. Keep sample prose separate from shipping content until IDs and voice/canon references are verified.

Use a prose review sequence: dramatic purpose; source truth; voice; graph reachability; plain-language player choice; localization; transcript/accessibility; continuity and collision. A line may pass literary review and fail integration because its destination is not selectable. A graph may pass schema validation and fail writing review because every option sounds identical.

## Continuation pass 7 acceptance

The conversation catalog is ready when every scene uses the smallest matching graph shape, operational lines have evidence and a fallback, the expedition-preparation sample never claims to select or start a route itself, and prose/branch/accessibility costs are visible before a large dialogue wave is commissioned.

### Interruption, close, and resume behavior

Every graph needs an explicit response to interruption: player backs out; scene owner becomes unavailable; location context changes; a quest result arrives; or the application restores after a save boundary. Backing out is not equivalent to refusal, abandonment, or failed quest unless the current command owner receives and confirms that action. A scene can be resumed by recomputing its entry node from current facts, not by restoring a stale node projection.

If the player closes while a command is pending, the command owner decides whether it has been submitted and what result is durable. The graph must not create a second request on reopen. If a result arrives while the scene is open, it can refresh the response list and offer a result node. If the scene is closed, the quest/journal/map owner surfaces the actual result through its ordinary read model.

Back, close, confirm, and cancel labels should be consistent across keyboard/controller input. The player should always be able to leave a nonterminal conversation. A terminal campaign choice uses an explicit confirmation and follows the current campaign resolution authority.

### Text length and information pacing

Break spoken dialogue at one thought per node unless a deliberate monologue is essential. Put optional detail behind an explicit “ask more” response and keep it skippable. Do not make the player traverse repeated hub lines to remember a clue; preserve important facts in the existing journal/map read models where supported. Transcript entries should reflect the final spoken result, while response history identifies what the player chose.

## Continuation pass 8 — three-scene side-quest arc, character voice, and dialogue data contract

### Three-scene arc: The Unentered Shelf

This is a provisional side-quest prose package. The working label names the fixture, not a final canon quest title. The arc begins with a discrepancy, develops through evidence and a dispatch decision, and closes with a result the player can observe on a later visit. Its central tension is modest and practical: everyone wants to help, but nobody should invent a receipt.

#### Scene one: the count does not match

At the shelter store room, the clerk has set two open ledgers beside a stack of folded blankets. The player can inspect the marks, ask what changed, or leave the room.

**Clerk:** “This line has six marked aside. The shelf has four I can count. There is no signature underneath.”

If the player asks whether the rest were delivered, the clerk replies: “I can tell you what the page says. I cannot tell you where the other two went.” If the player asks whether anyone made a promise, the clerk offers the nurse's account as a lead: “She was present when people talked about sending supplies. She did not sign the ledger.”

The inspection option becomes evidence only after the existing interaction/quest owner confirms it. Merely reading the dialogue does not advance the objective. If the task is not accepted, the player may leave without creating an active quest. If the quest offer is unavailable, the same room description can still function as atmosphere without presenting a false task command.

#### Scene two: decide what can be proved

After one or more sources have been checked, the graph summarizes facts by source:

**Clerk:** “The page says six. The shelf says four. The nurse remembers a promise. The road sketch says somebody expected a route. None of those marks says the blankets arrived.”

The player can review the ledger, hear the nurse's recollection, ask the mechanic to explain the old route, request a current expedition opportunity, or keep the question open. A skill response may distinguish a correction mark from a name; it cannot turn a reported memory into a signed receipt. A faction contact may add a verified archive only if the current faction owner exposes that route. If no location can be selected, the dialogue offers a delay and identifies which clue is missing.

#### Scene three: record a disposition

Three authored responses are available only if their current preconditions pass:

- **Keep investigating:** “Leave the bundles where they are. I want a receipt before we write a destination.” The quest owner keeps a valid evidence route active.
- **Reserve locally:** “Set aside what we can confirm here. Leave the neighbor's claim open.” The quest/resource owners must confirm any stock action before the clerk acknowledges it.
- **Close the question as unresolved:** “We do not have enough to say what happened. Record that clearly.” This outcome is available only if the quest owner supports an unresolved resolution and its history remains visible.

For confirmed local reservation, the clerk says: “I can account for these in this room. I will not write that the other shelter received a share.” For a rejected resource action: “The count did not match what we can move. Nothing has been reassigned.” For continued investigation: “Then I will keep the line open and leave the shelf alone.” For a missing owner result: “The stores book has not confirmed it yet.” These are separate result nodes, selected from actual command outcomes.

#### Return scene: consequence as ordinary conversation

On a later visit, the clerk's first response is chosen from the canonical quest/resource outcome. The player can ask what was settled or revisit the remaining uncertainty. A confirmed delivery can be acknowledged only when a transfer/receipt owner confirms both the destination and result. A local hold can acknowledge the shelter stock without resolving the neighboring claim. An unresolved closure can state that no transfer was recorded. A save missing optional outcome detail receives a neutral line and an owner query.

This side quest adds a repeat visit and three legible dispositions, but no new character, faction, item, location identity, relationship score, or secret morality branch. The distinct outcomes come from supported owner results rather than a hidden dialogue variable.

### Character voice and personality card

The scene uses three provisional voice profiles, subject to current roster/canon verification:

**The clerk** is careful with numbers and accountability. They pause before promising anything, use short conditional phrases, and become more direct under pressure. Their flaw is over-reliance on records; the arc tests whether they can act under uncertainty without falsifying the record.

**The nurse** separates witness from conclusion. She speaks in concrete sensory memories, corrects herself when she overstates what she saw, and avoids assigning motive. Her uncertainty is not a puzzle penalty; it is honest testimony.

**The mechanic** uses conditionals about roads, wear, and current inspection. They are wry when someone treats an old map as a guarantee. Their arc, if expanded, is learning when a practical warning has become an excuse to avoid helping.

Any durable personality development must be authored through existing character/relationship systems after an audit. These voice cards do not imply persistent fear, trust, guilt, or profession state in the runtime.

### Dialogue node data contract by responsibility

For integration review, separate graph fields from owner references:

- **Graph identity:** scene ID, revision, entry node, stable node IDs, stable response IDs, and explicit exits.
- **Text:** localization key, speaker ID, optional authored variant key, transcript role, and safe fallback.
- **Conditions:** stable predicate references plus presentation policy from Plan 21.
- **Edges:** next node or scene exit; no executable code in edge fields.
- **Effects:** typed command references from Plan 22; no direct state assignment.
- **Editorial notes:** purpose, voice intent, evidence basis, word count, collision note, and production estimate kept outside gameplay authority.

A node can contain text and responses, but text cannot imply a command the graph does not expose. A response can point to a command, but the command result controls the next result node. A speaker/location may be unavailable; the graph requires a fallback or the content validator must prove the scene is optional.

### Scene review sequence

1. Read the scene without conditions to judge purpose and voice.
2. Walk every graph edge and verify entry/exit.
3. Read each branch with its condition explanation and unavailable policy.
4. Compare spoken claims against the source-owner facts.
5. Simulate accepted, rejected, deferred, duplicate, and unknown results.
6. Check transcript, keyboard/controller back behavior, localization, and focus.
7. Search the completed prose and mechanics for story collision.
8. Estimate implementation and QA cost from branch/effect count, not word volume.

A scene is not ready because its prose is polished; it is ready when text, graph, facts, and owner results all describe the same player-visible situation.

## Continuation pass 8 acceptance

The side-quest arc is ready for canon and integration review when each scene has one dramatic job, the four outcomes remain distinct in prose and owner state, all three provisional voices have neutral fallbacks, and the return conversation can be reconstructed from canonical facts after save/load.

### Faction negotiation graph template

Faction negotiation is a reusable structure, not a new faction proposal. Bind it only to a currently authored group and its existing standing/access owner:

1. **Public entry:** the representative states the issue and what the group can offer.
2. **Terms hub:** the player can ask about cost, risk, evidence, and who is affected.
3. **Context branches:** knowledge, prior relationship, or faction access changes how much detail is available.
4. **Response node:** accept, refuse, ask for time, or propose an authored alternate term.
5. **Owner result:** faction/quest owners confirm accepted, rejected, or deferred results.
6. **Aftermath node:** the graph reports only what those owners changed.

**Representative:** “We can open the store room for your search. We cannot promise what the record will say.”

**Ask about cost:** “The watch will be short while the clerk checks the shelves. If you need a guard at the gate, ask again before you leave.”

**Ask for another term:** “I can request one shift from the watch. I cannot sign away supplies we have not counted.”

**Refuse:** “Then the door stays closed for now. The public notice is still yours to read.”

The sample deliberately avoids a faction name, standing threshold, resource amount, or guaranteed access result. Those values belong to the current faction and resource authorities. A future author can specialize its voice and political context after searching the canon registry and confirming the faction's actual rules.

### Location prose cards by knowledge level

Write separate text keys for arrival, investigation, and aftermath where each changes the player's understanding:

- **Arrival:** sensory detail and orientation that do not assert route safety or quest completion.
- **Investigation:** what can be seen/interacted with now, tied to actual scene objects and current discovery.
- **Clue:** a specific observation with provenance, plus uncertainty when interpretation is not confirmed.
- **Return:** a concise comparison between the earlier state and current owner-backed result.
- **Unavailable:** a known route/location condition, surfaced through the map owner.

An arrival paragraph may establish dust, drainage marks, damaged signage, or a shelter routine; it must not claim the cause or owner of the damage without evidence. A clue can direct the player toward a quest-only site, but Plan 18 determines whether the map shows an exact point, regional hint, or delayed route. Keep the short text version available for transcripts and localization.

### Character and faction branch balance

In hub scenes, avoid giving the highest-reputation speaker all useful information while other characters offer only filler. A gated branch should add perspective, convenience, or tone, while the base branch retains the essential task and its next action. If a faction's refusal is the intended outcome, make it a substantive result with a public-information route, not an invisible menu disappearance.

Make response labels describe the player's intent rather than the expected hidden effect. “Ask who signed the receipt” is clearer than “Challenge the clerk.” The owner result and character voice can make the exchange tense; the choice label should still let the player predict the conversational move.

### Prose variant constraints

Keep variants semantically equivalent when they exist only for tone or atmosphere. If one line reveals a location and another does not, that difference is knowledge-bearing and needs a Plan 21 condition and continuity review. If one response sounds like a promise and another like a possibility, the consequence may differ even if both point to the same node; writers must align language with the command/result contract.

## Continuation pass 8 acceptance supplement

The negotiation and location-prose templates are ready when they can bind to existing faction/map owners without invented mechanics, each knowledge level has a localization-safe text key, and gated branches add optional depth while leaving the core player action understandable.

### Transcript and localization acceptance

Every spoken node and player response needs a stable text key with enough context for translators to understand speaker, situation, and branch intent. Keep response labels independently understandable when shown in a history or accessibility list. Avoid references such as “the former” when the prior speaker line can be skipped or reordered by localization. Convey essential differences through wording, not punctuation, portrait tint, or unvoiced gestures.

The transcript records the line actually shown and the player response selected. It should not reveal hidden alternatives the player did not see. A return scene may show a newly valid result line, but it does not rewrite the earlier conversation. When text is missing, use the approved neutral fallback and emit a content diagnostic; never display raw IDs as prose.

## Continuation pass 9 — dialogue authoring blueprint, character beats, and scene variants

### Writer-facing node blueprint

Before writing full branches, complete one row per node:

| Node role | Speaker aim | Player information | Required conditions | Responses | Exit/effect |
|---|---|---|---|---|---|
| Opening | Establish why the conversation is happening now. | Current situation and speaker's immediate concern. | Actor/location available, or neutral fallback. | Ask, act, defer, leave. | None until a response requests it. |
| Detail | Answer one question with sourced information. | Evidence and uncertainty, attributed to source. | Evidence fact if this is a special detail. | Ask one follow-up or return. | Informational unless interaction owner records evidence. |
| Interpretation | Let player compare or question a claim. | Distinguish observation from conclusion. | Optional skill/knowledge fact. | Accept interpretation, ask more, leave. | Usually no durable command. |
| Commitment | State the practical choice in plain terms. | Known cost, target, unresolved facts. | Quest/location/resource preconditions. | Confirm, revise, cancel. | Owner request through Plan 22. |
| Result | Acknowledge what actually happened. | Accepted/rejected/deferred outcome. | Typed owner result. | Continue, retry if valid, close. | No repeated command. |
| Callback | Show a later consequence or unresolved thread. | Canonical result and optional memory. | Current owner-backed outcome. | Review, ask new question, leave. | New command only if separately authored. |

This blueprint prevents every graph from expanding into a conversation tree before its purpose and exit are clear. A writer can add nodes where they improve player understanding, but each node should earn its place through new information, voice, consequence, or accessibility.

### Character beats across the side-quest arc

Give each provisional speaker a small, observable progression:

- **Clerk, opening:** insists on separating count from destination. “I can add the bundles. I cannot add a name that is not here.”
- **Clerk, after evidence:** admits that recordkeeping does not resolve every obligation. “The book tells me what we wrote down. It does not tell me what we meant to do.”
- **Clerk, after accepted local hold:** accepts responsibility for the local stock while leaving the other claim open. “I will sign for this room. I will leave the other line blank.”
- **Nurse, opening:** recalls a request without overstating delivery. “I remember people asking. I did not see the bundles go.”
- **Nurse, after player asks for certainty:** corrects herself. “No. I was going to say I remembered the destination. I remember the direction people were looking, not the door.”
- **Mechanic, old route:** distinguishes a map's age from its current condition. “The road was drawn cleanly. Roads do not read their own maps.”
- **Mechanic, after a current route check:** reports only the checked segment. “This stretch is open as far as the culvert. What lies past it still needs a look.”

These beats change how the player understands each speaker, while the canonical quest facts remain independent. A later relationship arc can deepen a speaker only if current memory/relationship owners support the new callbacks. Otherwise, the speaker's growth is expressed in authored scene context, not a hidden saved personality score.

### Branch matrix for a returning conversation

| Current owner result | Opening line | Available follow-up | Prohibited claim |
|---|---|---|---|
| Still investigating | “You came back with another question?” | Review remaining evidence or request a clue. | That any delivery happened. |
| Local stock confirmed | “I signed for the bundles in this room.” | Ask about unresolved destination. | That the neighboring shelter was served. |
| Delivery confirmed | “The receipt is in the book now.” | Review who confirmed it and close. | That the receipt exists if its owner did not confirm. |
| Task unresolved/closed | “We left the record open as incomplete.” | Read the closure note. | That uncertainty has been resolved. |
| Task failed/expired | “The route window passed.” | Ask about a supported recovery task. | That a late recovery changes the original deadline result. |
| Result unknown | “I need the current stores record before I answer.” | Retry a read or leave. | Any success or failure outcome. |

The line is selected by quest/resource evidence, not by how many times the player visited or which response was previously displayed. Optional memory can add a greeting, but it cannot change this outcome matrix.

### Dialogue pattern selection rubric

Choose a **linear scene** when the player mainly needs a concise status update. Choose **hub-and-spoke** when questions serve different information goals. Choose **reconvergent branches** when knowledge or voice differs but the main task continues. Choose a **long split** only when outcomes alter distinct quest/faction/world states and the production team can support all callbacks. Choose a **hidden node** only when the clue is optional and the base route remains understandable.

When a branch rejoins, make the return line reflect the knowledge gained without repeating the full introduction. When it does not rejoin, show the later consequences and all exit routes in the dependency graph. Avoid creating a branch that offers the same action under three emotional labels; write one response with a tone variant if the player consequence is identical.

### Dialogue implementation acceptance card

For each scene, deliver: stable IDs; node/response table; owner facts and effects; text keys; localized response labels; character voice notes; availability/fallback matrix; one complete transcript per major outcome; keyboard/controller path; missing speaker/location behavior; save/restore query; collision search; and focused test fixtures. If a dialogue editor or validator is proposed, its output must preserve stable IDs and make unreachable branches visible without silently rewriting the graph.

### Dialogue scene production cards and scope budgets

Treat each conversation as a production unit with a defined purpose, branch surface, and reviewable stopping point. Before prose drafting, the writer and system designer agree on entry context, central question, fact sources, available exits, and command-bearing responses. Classify every branch as cosmetic, informational, local, quest, relationship, faction, or world consequence. Ending-scale consequences need a separate content and integration review; they should not arrive as an unmarked response in a small character scene.

| Scene card | Core player need | Recommended graph shape | Main production risk |
|---|---|---|---|
| Main quest briefing | Understand the next task and commitment. | Short hub with questions, then confirmation. | Cost hidden behind optional dialogue; stale offer accepted. |
| Character conversation | Learn a person's view and decide whether to help. | Reconvergent branches with supported relationship variants. | Greeting or mood line mutates relationship authority. |
| Faction negotiation | Compare terms, access, uncertainty, and refusal. | Hub-and-spoke information, then explicit confirmation. | Dialogue promises access the faction owner did not accept. |
| Discovery scene | Understand what the environment revealed. | Linear observation followed by optional interpretation. | Inference presented as confirmed fact. |
| Investigation interview | Compare testimony and ask targeted questions. | Evidence-gated spokes returning to a neutral summary. | Repeated questioning duplicates evidence or canonizes testimony. |
| Return/callback scene | See what changed after an accepted result. | Result-specific opening and safe close. | Replaying the operation instead of reading its outcome. |

Track practical counters: node and response count, distinct owner predicates, command kinds, unique text keys, non-reconvergent exits, and supported transcript variants. These are planning signals, not universal limits. When one scene has many predicates or command families, split it by player purpose or defer unsupported routes. When a large hub repeats information, collapse it into a clear summary and preserve questions that reveal new facts or voice.

The production card includes a speaker voice note written as observable habits, not hidden personality stats: what the character notices first, what they refuse to claim without evidence, how they react when corrected, and what sentence shape they use under stress. Another writer should be able to produce a compatible line without inventing a private mood system. Relationship owners may influence allowed variants only when their current contract supplies a stable fact.

### Prose review: fact, inference, and commitment

Review significant lines for three layers. The fact layer states what the speaker knows or what an owner confirmed. The inference layer states what the speaker believes it may mean. The commitment layer states what the player is asked to do and what the result could affect. Keep these layers legible when a speaker is mistaken, evasive, grieving, or persuasive; distinct voice does not require misleading the player about whether an action occurred.

For an investigation, establish the source (“I counted four sealed tins”), attribute uncertainty (“I did not see who carried them”), then offer a next step (“the registry may show who signed out the cart”). A response can challenge, ask, or leave. Only owner-confirmed inspection creates evidence. A skill variant may improve precision or reveal an additional question, but it should not convert hearsay into certainty unless the content explains why that skill supports the conclusion.

For a commitment, present action, target, known cost, and uncertainty before confirmation. If the player can negotiate, each term is a distinct authored response sent to its proper owner. A tone choice should not imply a different settlement when the consequence is identical. After acceptance, the result node names what the owner accepted and what remains open; it does not embellish a partial outcome into success.

### Reuse without voice flattening

Reusable graph patterns standardize navigation and validation while character-specific language and knowledge remain local. A shared ask-detail/return-to-topic pattern may be reused, but text, predicates, and evidence references stay attached to the actual speaker and scene. Avoid one generic quest-giver graph where characters present identical choices and differ only by display name.

When characters discuss the same event, reference a shared fact with separate interpretations instead of duplicating the event's truth. A medic may focus on who needed supplies, a clerk on what was recorded, and a mechanic on whether the route could carry the load. Each can be incomplete in a distinct way. A later owner-confirmed result can update their callbacks, but dialogue must query current state rather than store a copied version in every node.

Before localization, provide context for ambiguous short strings, speaker, emotional intent, grammatical assumptions, and whether a response is a question, confirmation, refusal, or tone variant. Keep placeholders typed and safe. Transcript review shows the exact line sequence for every major route, including fallbacks and final exits. A graph that is reachable in the editor but produces an abrupt or contradictory transcript is not ready for promotion.

### Branch review and prose completion criteria

For each non-reconvergent branch, state why it remains separate: it changes a lasting quest result, relationship, faction access, location availability, or ending resolution. If its only difference is phrasing, reconverge and carry the approved tone/text variant. If the branch remains split, inventory every later callback that must recognize the result and identify its owner. The team should know the future writing cost before multiplying early choices.

Assess side-quest prose for more than word count. It should establish a character want, an obstacle, evidence the player can gather, a choice with understandable limits, a result that changes the scene or leaves a deliberate uncertainty, and a callback that confirms the world remembers the accepted result. The player must be able to decline and understand the consequence of deferring. A quest can be emotionally unresolved while its system result is accurately recorded; avoid making those two meanings depend on one ambiguous “complete” line.

The packet should include a first-visit transcript, at least one changed-state revisit, and the generic fallback for missing optional content. Writers note which details are permanent canon, which are provisional examples, and which are selected from generated variants. This lets later expansions add lines without contradicting authored facts or presenting generated flavor as a source of world truth.

### Transcript closeout criteria

Close a dialogue packet only after reviewers can read complete transcripts for the baseline route, a meaningful alternate route, an unavailable action, and a return visit. Each transcript names selecting conditions and owner facts behind result lines. Mark provisional prose clearly, preserve stable node identities when meaning is unchanged, and list localization context for short responses. Final review checks voice distinction, branch necessity, readable commitment language, safe exits, and callbacks that reflect accepted results. Unreachable choices and contradictory lines return to the author before any runtime integration claim.

## Continuation pass 10 — location prose kit, witness scene, and environmental narrative layers

### Location text carries a different job at each stage

Write location prose in small, separately selectable units so dispatch, arrival, inspection, return, and closure can stay truthful without duplicating paragraphs. These are authored presentation roles, not new map or quest state fields unless the current content schema already supports them.

| Text role | Player need | Draft example, intentionally unnamed | Content constraint |
|---|---|---|---|
| Dispatch summary | Decide whether the known destination matters. | “A public records room sits beyond the covered crossing. The route note is old; the final approach has not been checked.” | State known relevance and uncertainty, not an unverified hazard. |
| Arrival | Orient the player on site. | “The entrance is still marked with the old civic seal. Wind has taken the paper notices, but their tacks remain in the wood.” | Give one strong image plus a usable spatial clue. |
| Inspection detail | Reward attention with an observation. | “One line was cut free with a blade. The fibers are clean where rain has softened the rest.” | Distinguish physical observation from interpretation. |
| Ambient text | Add tone without blocking play. | “A loose metal strip taps the frame whenever the gusts turn.” | Do not imply a quest action or danger unless an owner reports it. |
| Return after evidence | Recognize a changed canonical fact. | “The marked place is empty now; the trace you recorded remains in the journal.” | Read the accepted result; never infer it from revisit count. |
| Closed/unavailable | Explain a route or content limitation. | “The outer hall is closed today. The note does not say when it will reopen.” | Name the reason only when known; offer a safe exit or alternative. |

Keep the base description useful without the quest. A location can carry optional lore, but the prose must not hide the only instruction in a generated variant. If the same site serves three quest families, compose a stable arrival anchor with small quest-specific overlays selected by current owner facts. Do not copy the entire site description into each quest row, where revisions will drift and localization workload will multiply.

### Provisional witness scene: what was seen, what was said

The following is an unassigned prose sample for testing node shape and voice. It reserves no character, location, faction, canonical event, or ID. The speaker is an unnamed records keeper; the scene demonstrates a conversation where the player has found a deliberately altered notice but has not yet learned who changed it.

**Opening:** “If you're asking whether I saw the paper come down, I saw the wall in the morning. I did not see the hand.”

**Ask what is certain:** “The two pinholes are there. The board was used. That is all I can put my name beside.”

**Ask for a guess:** “People have guesses. A guess can be useful if you carry it as a guess.”

**Challenge the keeper:** “You want me to accuse somebody because the empty space looks deliberate. It does. That still isn't a name.”

**Offer to stop:** “Then leave the space empty for now. An honest blank is better than a name we cannot take back.”

This exchange supports several character directions without requiring a hidden mood score. The keeper is careful, uneasy about false accusation, and willing to resist pressure. A more defensive variant can shorten answers; a more trusting variant can volunteer where a second witness might be found. Those variants require a real owner fact such as an accepted prior promise or current relationship state. The same core evidence statement remains invariant across them.

### Branch and evidence card for the sample

Model the conversation as a short hub with one evidence interaction and two informational spokes:

| Response | Scene behavior | Durable result permitted | Next node |
|---|---|---|---|
| Ask what is certain | Keeper lists direct observations and their limits. | None unless an inspection command separately records the board evidence. | Return to hub. |
| Ask for a guess | Keeper attributes rumor to an unknown source. | No confirmed evidence; optional rumor only if its owner supports that fact. | Return to hub. |
| Challenge | Keeper pushes back without ending the conversation. | A local tone response only, unless an owner-backed consequence is approved. | Rejoin with the same practical question. |
| Leave the space empty | Player stops the inquiry for now. | A quest decline/defer request only after explicit confirmation. | Safe exit or supported deferred state. |
| Inspect the board | Scene hands off an inspection request. | Discovery/evidence owner result, then quest update if accepted. | Result node based on typed owner outcome. |

If inspection is unavailable, the action is visibly blocked with a reason such as “the board is beyond the closed hall,” if that fact is current. The player can still ask questions or leave. If the clue is unknown, do not show the exact location of the second witness; an authored general hint may become visible after a real rumor/evidence event. If the player declines, the keeper's final line does not claim the player has accused or forgiven anyone.

This sample can scale into a short investigation about record integrity, mistaken attribution, or community trust. The writer chooses only one primary question for the actual quest. If its result is “we cannot prove who removed the notice,” that can still be a complete investigative resolution with an honest unknown. Do not fabricate a culprit merely to force a conventional reveal. If a later story supplies new evidence, create a supported follow-up rather than rewriting the earlier transcript.

### Character differentiation through attention and syntax

Build personality through repeatable choices in what a speaker notices, what they qualify, and how they end a thought. One character cites measurements, another recalls who was present, and another asks what decision must be made today. These habits are authored voice guidance; they are not world facts or a new character-stat system. They should remain understandable even when the player lacks relationship or skill context.

For each recurring speaker, give the writer four notes: their default subject of attention, a phrase or sentence rhythm to avoid overusing, the limit they will not cross when evidence is weak, and one change they can show after an accepted player action. Avoid reducing a survivor to a dialect spelling or a single verbal tic. Distinct voices come from priorities and reasoning as much as vocabulary.

When two speakers interpret the same evidence differently, preserve the shared observed fact in the quest/evidence owner and author their claims as attributed interpretations. The more emotional speaker may believe the missing notice was meant to protect someone; the more procedural speaker may suspect a routine removal. Neither interpretation becomes canonical because of stronger prose or a skill check. Later evidence can confirm one, reject both, or leave the question unresolved.

### Environmental story without forced interaction

Environmental writing should reward observation while letting players decide how much to investigate. A scene can offer three levels: ambient image, inspectable clue, and a deeper interpretation unlocked by knowledge or skill. The ambient image is optional atmosphere. The inspectable clue needs an actual interaction/evidence producer. The interpretation is a dialogue or journal explanation that should state the evidence it relies on and its uncertainty.

Do not make all environmental writing a collectible. Some details exist to establish safety, shelter, loss, routine, or the passage of time without an interaction prompt. Some clues support a quest but do not complete it. Some objects may change a character's later line while remaining irrelevant to campaign progression. Mark those roles in the authoring card so content validators do not demand a quest consumer for every atmospheric sentence.

### Prose completion and localization bundle

For each location/scene packet, deliver the base arrival paragraph, short map summary, inspection text, optional variants, closed/unknown copy, and post-result callback when applicable. Include speaker attribution, target context, uncertainty, interaction labels, and placeholders. Translators need to know whether “it” refers to an object, a person, or a route; tiny response labels often carry more ambiguity than long paragraphs.

Review each localized graph for line expansion, response order, readable focus, and important information that was only present in optional branch text. Provide a complete transcript for the principal paths and ensure all paths end in a supported node or safe exit. A fallback should sound authored and diegetic, not like a technical error, while diagnostics keep the actual missing content ID available for maintainers.

### Prose card: the room, the trace, and the speaker

An investigation scene can use a three-part writing card to keep location, evidence, and voice coherent. The room card says what the player notices first and how it can be navigated. The trace card records one concrete observation and the interpretations it does not support. The speaker card identifies the character's immediate concern and what they will not claim. Together they give writers enough material for a scene without requiring a cinematic branch for every line.

| Card | Draftable content | Example boundary |
|---|---|---|
| Room | A distinct layout, sensory anchor, and one visible interaction point. | “The painted sign points both ways; only one hinge has fresh oil.” The sign does not prove who changed it. |
| Trace | Observable detail, source, and unresolved question. | “The paper is torn at the staple holes.” The tear alone does not prove a hurried departure. |
| Speaker | Immediate need, verbal priority, and uncertainty limit. | A keeper may care about preserving attribution; they cannot confirm a person they did not see. |
| Choice | Ask, inspect, challenge, commit, defer, or leave. | Asking for a guess does not turn it into recorded evidence. |
| Result | Typed acceptance, refusal, or unavailable response. | “I can confirm what the marks show. I cannot give you a name.” |

Do not make every location clue a riddle. A clear physical description can be memorable without withholding the verb the player needs. If a clue is intentionally ambiguous, show the evidence plainly and let the uncertainty come from interpretation. A map marker may indicate where to inspect without pre-answering what the inspection will reveal.

### Response composition for common dialogue shapes

Author each response around the interaction type so players can tell what it will do:

| Response shape | Example player-facing text | Expected scene behavior |
|---|---|---|
| Linear continuation | “Show me what you found.” | Advance through the current authored sequence. |
| Hub question | “What was written on the board?” | Open an informational spoke, then return to the hub. |
| Reconvergent interpretation | “That sounds deliberate” / “It may be wear.” | Show a voice/knowledge variant, then return without false state. |
| Knowledge-gated detail | “Compare the marks with the old survey.” | Available only when the player has the required knowledge; otherwise baseline scene remains complete. |
| Relationship variant | “You said you would tell me if this changed.” | Select tone only from a supported relationship/memory fact. |
| Command confirmation | “I will ask the records office to reopen the notice.” | State target and commitment, then send one owner-routed request. |
| Recovery exit | “Leave it unresolved for now.” | Preserve the open/failed/deferred state accurately and close safely. |

Use direct verbs in choice labels. Avoid “continue,” “maybe,” and “stay silent” when they hide materially different outcomes. A hub can provide optional questions without making the player ask every one before acceptance. A confirmed action should not be nested under a response that appears informational. After an owner result, remove or transform the original command response so repeated selection cannot submit it again.

### Writing callbacks as observed change

The most useful revisit line points to a world or relationship fact that actually changed. It can mention a returned object, a new route report, a repaired sign, a character's revised account, or an open question that remains unsettled. Avoid generic praise for “helping everyone” when the player only performed a local action. Avoid an exact time or population claim unless a current owner supplies it.

| Callback basis | Good writing direction | Keep out of the line unless verified |
|---|---|---|
| Quest owner says complete | Name the objective or practical outcome. | Broader community-wide success. |
| Evidence owner says partial | Acknowledge what is confirmed and what remains open. | Certainty about an unobserved event. |
| Relationship owner changes | Reflect a supported shift in familiarity or willingness. | A hidden score or permanent friendship claim. |
| Faction owner updates access | State the current access/term. | Assumed agreement by every member. |
| No owner fact changed | Use ordinary greeting or ambience. | A fabricated memory because this is a later visit. |

Callbacks are an efficient way to make the world feel responsive, but every line still has a maintenance cost. Prefer a few clear, high-value callbacks over a sentence for each small numerical change. If a callback is optional, its absence should not make the main story confusing. If it communicates a key result, include a baseline route through the journal or another current read model.

## Continuation pass 11 — provisional witness quest, location lore, and playable dialogue packet

### Story packet status and dramatic question

This section expands the unnamed altered-notice investigation introduced above into a complete writing fixture. It is still provisional: it reserves no canon name, actor ID, faction ID, location ID, quest ID, or objective schema. Its function is to test whether a small story can distinguish observed fact, rumor, interpretation, player commitment, and accepted result across several conversations.

The central question is deliberately modest: a public notice was removed from a board; the player can learn what the paper showed, why its empty space matters to one resident, and whether a name should be attached to the removal. There is no required culprit reveal. The meaningful choice is whether the player publishes only verifiable facts, requests a private follow-up, or leaves the record open. This story can close as “unresolved but accurately documented.” That is a valid ending for an investigation quest.

The dramatic tension comes from two reasonable needs. The keeper wants future readers to know which statements are observations and which are guesses. The returning resident wants to know whether a person was named on the removed paper. A runner remembers carrying notices but never saw this one on the wall. No speaker is the omniscient quest narrator. The quest owner stores whatever evidence the player actually proves; dialogue gives those facts a human setting.

### Location and shelter lore cards

The location treatment uses existing location/map authority and can be represented by an already-supported public-room site. If a new site is proposed later, it must be canon-checked and added through the canonical location pipeline. Do not create several IDs just to host a scene that can work as an interaction at one existing place.

| Location role | Lore and physical details | Availability and discovery | Story use |
|---|---|---|---|
| Public notice wall | Layers of pins, old seal marks, protected timber beneath the paper edge; the wall has been repainted around older marks. | Ordinary access if an existing public room is available; a clue may reveal the board as a place to inspect. | Establishes that people use the wall for practical coordination, not just lore collectibles. |
| Records desk | Two worn ledgers, a repaired drawer, a blank index card kept for corrections. | Visible only after the relevant contact or location knowledge is available. | Provides a record of what was copied or received, not proof of who removed a notice. |
| Covered passage | A route from the board toward a communal work area; one side carries chalk arrows whose direction changes with repairs. | Optional observation or route clue, not automatically quest-required. | Lets an environmental detail suggest use and maintenance without resolving the accusation. |
| Return view of the board | Same canonical room and anchor; current pin/notice state changes only after an owner-confirmed record result. | Revisit when normal location access permits it. | Makes the player's chosen documentation visible without claiming the removed paper was recovered. |

The shelter lore emphasizes routine: people mark shift swaps, water collection, and repairs where everyone can see them because private messages do not reach every resident. The board is not treated as a magical community database. Some residents cannot read every notice; some prefer a spoken version; corrections are ordinary. A missing sheet can matter because it carried a practical instruction, even when its authorship remains uncertain.

### Character cards and voice boundaries

Use role labels until canon and character ownership are verified. Each role has a distinct attention pattern; neither an attitude label nor a line choice creates a new persistent personality meter.

| Speaker role | Personality in play | What the speaker knows | What they will not claim |
|---|---|---|---|
| Records keeper | Careful, dryly humane, dislikes certainty borrowed from a crowded room. | How corrections are recorded and what marks remain on the board. | Who removed the sheet without seeing the act or an accepted source. |
| Returning resident | Direct, tired of being told to wait, asks what changed since last visit. | Why the removed notice was important to them personally. | The identity or motive of the person who posted it. |
| Former runner | Quick with directions, more precise about routes than paperwork. | Which workrooms and passages they used to carry notices. | That carrying other papers proves they carried this one. |
| Optional coordinator | Patient and procedural, keeps public discussion from becoming an accusation. | Which public review or correction process currently exists. | That their presence confers faction authority or resolves the quest automatically. |

The keeper speaks in distinctions (“written,” “seen,” “copied,” “confirmed”). The resident uses concrete stakes (“I was waiting for the time, not a name”). The runner gives spatial references and corrects their own estimates. The coordinator uses a short summary and asks the player what may be stated publicly. Those habits carry personality without exaggerated dialect or repetitive verbal tics.

### Three-scene arc and quest-purpose boundaries

**Scene A — Environmental discovery.** The player encounters a board with pinholes and a clean tear. Initial text should not say that the paper was “stolen” or “censored.” It offers an inspection verb and an ordinary exit. Inspection yields only the physical observation if the current discovery/evidence owner accepts the interaction. The optional first conversation with the keeper explains how corrections are usually recorded.

**Scene B — Compare accounts.** The keeper distinguishes the board's physical trace from the ledger's entries. The resident explains why they needed the notice but does not name its author. The runner remembers a route used that day but qualifies that memory. The player may compare these sources, ask the keeper to mark uncertainty, request a private follow-up, or stop. Relationship/skill/faction variants can add context only when their current owners provide facts; they do not replace the source comparison.

**Scene C — Choose what to record.** The player can record a verified observation without attribution, pursue an optional source, or leave the question open. A command-bearing response previews whether the result is public, private, or deferred and then sends one owner-routed request. On acceptance, the board/quest read model can show the current result. On rejection or unavailable owner, the player keeps the safe “leave open” option. The keeper acknowledges the accepted scope; no dialogue line reveals an unverified culprit.

The core quest type is investigation. It can begin through environmental discovery and include a character conversation. It is not a timed quest unless an existing time owner defines a real deadline and the story explains it. It is not faction-gated by default. The optional coordinator can be an expansion layer; the base route still records an observation and resolves as an acknowledged unknown if that character is absent.

### Branch map and evidence contracts

| Branch | Required source fact | What the player learns | Objective outcome |
|---|---|---|---|
| Inspect physical trace | Discovery owner records interaction with the board. | The sheet was attached and removed; precise condition of the paper. | Inspection objective advances only if the evidence matches its definition. |
| Read the ledger | Records/interaction owner exposes a current entry. | What was copied or corrected in the record. | Supports record-comparison objective; does not prove who removed the sheet. |
| Ask the resident | Actor available and conversation condition passes. | Personal significance and timing need. | Adds attributed testimony if the current quest/evidence owner accepts it. |
| Ask the runner | Actor or alternative account available. | A route and approximate time, explicitly uncertain. | Adds an attributed lead, not confirmation of authorship. |
| Ask for public correction | Current coordinator or record owner is available. | How an uncertainty note can be preserved. | Sends a separately confirmed record request; no accusation. |
| Leave inquiry unresolved | None beyond active quest eligibility. | The limits of available evidence. | Completes/resolves only if the quest owner supports an unresolved closure. |

The player can complete the core arc with the physical trace plus keeper explanation. The resident and runner add perspective but are optional. If either actor is absent, the journal explains the missing source only if the quest requires it; otherwise the task remains coherent. Do not make the player visit every optional speaker before the graph permits closure.

### Sample location prose and transcript beats

**Arrival:** “The board is under the roofline where people stop to shake rain from their sleeves. The newer paint ends around a darker rectangle.”

**Inspect prompt:** “Look at the paper edge.”

**Inspection result:** “Two pinholes remain in the wood. The tear is clean at the top and feathered along the damp lower edge.”

**Keeper, first visit:** “You can write down what the wall shows. If you write down who did it, you need a source for the name.”

**Resident, after the player asks why it matters:** “I needed the hour. Nobody promised me an answer. I only wanted to know when to come back.”

**Runner, when asked whether they carried the paper:** “I carried a stack past the covered passage. I couldn't see the headings under the cord. That's not the same as carrying this one.”

**Player asks for a guess:** “I can guess. So can anyone who heard the room go quiet. The board won't know the difference.”

**Player chooses a verified note:** “Put down that the page was there and is gone. Leave the name empty.”

**Player leaves it unresolved:** “Then we keep the space honest. If someone brings better proof, there's still room to add it.”

The arrival/inspection/result lines are visible only at their intended locations and evidence stages. The keeper's line may be selected on the first relevant conversation. A return greeting after the public record was accepted can say: “The new note is pinned below the empty space. It says what we checked, and nothing more.” If no result was accepted, use: “The blank is still blank. We have not learned who took the page.” The latter line is not a failure consequence; it reports the actual unresolved state.

### Knowledge, skill, relationship, and faction variants

| Context | Additional response | Information/result boundary |
|---|---|---|
| Player has a relevant research skill | “Compare the pin spacing with the older board marks.” | May identify whether marks are consistent; cannot infer the remover's identity. |
| Player has read a public ledger | “Show the keeper the entry you found.” | Opens a comparison node; evidence still comes from the accepted record interaction. |
| Player has a supported relationship fact | Keeper can offer a warmer or more guarded introduction. | Tone only unless a separate, accepted trust command exists. |
| Player has faction contact | Ask whether the coordinator recognizes a public correction route. | Faction option may clarify process; it cannot certify the removed sheet or grant standing by dialogue. |
| Player previously failed a related task | Keeper acknowledges the unresolved result without shaming. | Quest owner supplies failure/recovery context; the scene does not reopen the old instance silently. |
| Context owner is absent/unknown | Baseline question and safe exit remain. | No accusation, skill penalty, or false memory. |

Specific gated responses should include a player-readable reason if their absence would otherwise confuse the scene. Do not show secret response labels or reveal a private faction contact in a public transcript. Every optional branch must reconverge at the decision or carry a named later callback if its owner-supported consequence is truly different.

### Failure, recovery, rewards, and replay

| Situation | System result | Story response | Reward policy |
|---|---|---|---|
| Player never finds the board | Quest remains undiscovered/available according to the current owner. | A supported area hint may direct attention to public notices. | No reward. |
| Board cannot be inspected | Keep the route blocked or use a supported alternative interaction. | Keeper can describe the ordinary correction process but not claim inspection occurred. | No evidence reward. |
| Keeper unavailable | Continue through a public ledger or close as unresolved only if supported. | Do not simulate a conversation with an absent character. | No relationship effect. |
| Sources disagree | Keep observations and testimony attributed. | Player can document the conflict or seek another source. | Information/closure only if accepted by the quest owner. |
| Player publishes an unsupported accusation | If no current owner route supports this choice, do not author it as a durable result. | Allow challenge/revision dialogue without a false canonical name. | No fabricated faction/reputation penalty. |
| Player chooses a verified anonymous note | Owner accepts only the verified public record scope. | Keeper acknowledges that the name remains absent. | Small information/relationship consequence only if supported. |
| Player leaves | Preserve active/deferred state using existing quest semantics. | Keeper provides a clean exit. | No hidden acceptance or abandonment. |
| Quest definition is retired after acceptance | Keep the saved reference resolvable or migrate. | Provide a truthful legacy closure. | No repeat reward. |

The value of the reward is information and player-authored restraint, not a guaranteed item. A later callback may confirm that the corrected note helped residents coordinate, but only if the world/quest owner reports a supported result. A surviving open question can seed a follow-up investigation later without rewriting this quest's terminal outcome.

### Reuse, expansion placement, and production cost

The scene pattern can be reused for other evidence investigations: a physical trace, a source with bounded knowledge, a second attributed account, an uncertainty-preserving choice, and a supported return line. Reuse graph topology and validation fixtures, not character names, motives, or prose. Another investigation must change the practical question, evidence types, and likely consequences to be a distinct story.

The minimum core version uses one existing public location, one keeper, one interaction, a truthful unknown path, and a journal/dialogue close. A medium expansion adds the resident/runner viewpoints and a revisitable public record. A larger expansion can add a faction coordinator or campaign callback only if that owner and dependency are current. Production cost is primarily transcript/localization and evidence-review time; it rises sharply if a unique quest result changes later access, character relationships, or ending input.

Reviewers should receive a node/response graph, complete transcripts for anonymous-record/optional-source/unresolved routes, localization context, evidence provenance table, fallback for absent speakers, and a collision search for the central story function. The arc remains a proposal until live source/data/owner audits confirm it can use current APIs and locations.

### Transcript variants for the witness scene

The same scene should demonstrate how a small number of supported facts can alter a conversation while leaving its evidence contract intact. The following variants are provisional writing examples for the records keeper and do not imply new saved relationship or mood fields.

| Scene condition | Keeper response | Narrative function |
|---|---|---|
| Player has not inspected the board | “We can talk about the notice. If you want to say what the wall shows, you'll need to look first.” | Directs the player to the real evidence source. |
| Inspection accepted, no corroboration | “You found the pinholes and the torn edge. That gives us a page that was here, not a name.” | Acknowledges scope without overclaiming. |
| Player found the ledger entry | “The book records a correction. It doesn't say who lifted the old sheet.” | Separates the recorded action from physical removal. |
| Player asks for a culprit | “You can write a suspicion in your own notes. I won't put it on the public board as fact.” | Holds the character's evidence boundary. |
| Quest owner reports a failed approach | “The person you hoped to ask is gone. We still have the paper and the ledger.” | Reacts to failure while keeping the alternate route visible. |
| Player returns after accepted anonymous note | “The note is pinned beneath the gap. It says what we checked.” | Confirms the accepted public scope. |
| Player returns with no new fact | “Nothing on the board changed. We can leave it that way.” | Avoids inventing a new visit result. |
| Optional memory fact is unknown | “Good to see you. What did you want to check?” | Neutral fallback; no false familiarity or accusation. |

These variants should be authored as a compact state-to-line table, not a separate full scene copy for each combination. If the keeper's relationship state changes the opening, that line can vary while the evidence detail remains the same. If the quest fails, its owner supplies the failure fact, but the keeper's interpretation can remain compassionate or reserved as written. A skill line can reveal how staple spacing was measured, yet it cannot prove authorship without a supported evidence source.

### Provisional second-scene transcript: compare the sources

**Resident:** “I came back at the hour the old sheet gave me. The wall had a white square and nothing else.”

**Player — ask what the sheet promised:** “It told me when to return. It did not tell me who would be here.”

**Player — ask whether anyone was named:** “Not on the copy I saw. I was waiting for an instruction, not a person's name.”

**Keeper:** “That is what the resident remembers. The record has a correction mark. The wall has a gap. We have not connected those three things.”

**Runner, if available:** “I carried a bundle down the covered passage. The cord was tight enough that I couldn't read the top pages.”

**Player — suggest the runner removed it:** “You have a route and a bundle, not a sighting.”

**Runner:** “Right. I know where I walked. I don't know what was in the bundle. I should have said that first.”

**Player — ask for a public summary:** “Write the three things we know, and mark the rest as open.”

**Keeper:** “I can ask the records owner to pin that summary. I cannot make an open question look finished.”

This exchange permits the player to challenge both characters. A mistaken inference can be corrected within the scene without a permanent relationship penalty. If a player has the relevant research knowledge, an optional response can point out a physical mismatch in staple spacing, but the scene still lacks an identified hand. If no optional character is present, the keeper and player can compare the wall and ledger alone. The primary route remains intelligible on first visit.

### Provisional third-scene transcript: resolve the public record

This scene is available only after the quest owner has the required evidence and the player chooses to make a public record request. It has an explicit confirmation boundary:

**Keeper:** “I can pin a note that the sheet was present, that the resident returned for its hour, and that the record shows a correction. The author remains unknown.”

**Player response — confirm:** “Write those facts. Leave the name and motive open.”

**Player response — ask for a private copy:** “Keep the observation with the inquiry record. Don't put it on the public wall.”

**Player response — defer:** “Let me check one more source before it goes up.”

**Pre-submit detail:** “The public note will share the checked observations. The private copy will stay with the record request. Neither identifies who removed the sheet.”

**Accepted public result:** “The keeper pins the note beneath the gap. The blank space is still visible.”

**Rejected/stale result:** “The records desk is closed right now. Nothing has been posted. You can come back or leave the inquiry open.”

**Deferred result:** “The keeper sets the card aside without signing it. The record remains unfinished.”

The public/private/defer choices are meaningful only if an existing owner can represent those scopes. If the current system supports only one record outcome, narrow the prose to that outcome instead of implying privacy controls the game does not implement. An accepted private copy must not be described as public knowledge. A rejected request does not mean the keeper refused on principle; the line attributes the problem to the current owner result.

### Branching dialogue with bounded reconvergence

The graph can use a hub for source questions, short knowledge or skill spokes, one command confirmation, and a result node. Source-specific branches rejoin at a neutral summary that marks which accounts were heard. A significant lasting outcome such as the public record choice may exit to its own callback, but the evidence-gathering spokes do not each require a separate ending.

| Branch tier | Example variation | Rejoin policy |
|---|---|---|
| Cosmetic | Keeper is warmer after supported prior contact. | Same question and evidence. |
| Local knowledge | Player recalls a ledger correction. | Rejoin with an added source in the summary. |
| Investigation conclusion | Player chooses public note, private record, or leave open. | Stay distinct until the owner returns its result. |
| Relationship consequence | Keeper responds to a prior accepted promise. | Rejoin only if the durable relationship result is identical. |
| Campaign consequence | A later authorized process uses the verified summary. | Split only when campaign owner accepts a different fact. |

Before keeping a branch split, list the later transcript and map/journal consumers it creates. A branch whose only difference is “I trust your answer” versus “I believe your answer” should usually reconverge unless the relationship owner distinguishes those acts. This prevents a small investigation from producing a large, untestable tree.

### Location information as an authored sequence

The location packet should communicate what changes as the player learns more. Before discovery, a general regional lead may say “a public board used to stand near the covered walk” if that clue is already known. On arrival, the player sees the old seal and empty rectangle. Inspecting reveals the pinholes and cut paper. A later visit after an accepted note shows the new card and the unchanged gap. Each stage reads the discovery/quest owner and uses a distinct text key; it does not invent a second location-state system.

Optional room lore can describe how the shelter uses the board: pinned requests have a return date, corrections are marked in pencil, and repeated notices are copied into the records desk. Keep those details consistent across arrival, inspection, and dialogue. If the location is unavailable, the map/expedition owner supplies that state and the scene uses its closed fallback. Writers should not reference inaccessible details in the baseline quest instructions.

### Narrative handoff and interaction requirements

The authoring package for this witness arc includes the story question, scene purpose, speaker cards, location lore, transcript variants, branching graph, evidence/source table, accepted command scope, unresolved ending, and localization notes. It identifies which lines are provisional examples and which future canon review must approve. Plan 21 owns predicate truth and disclosure tiers; Plan 22 owns command/result effects; Plan 17 owns quest progress and closure; Plan 18 owns location opportunity and dispatch. Plan 20 supplies dialogue and prose references without taking over those responsibilities.
## Continuation pass 12 — The sluice at low light: dialogue and location narrative packet

This pass turns the provisional Tidemark portfolio into a writer-facing narrative packet. The packet contributes playable scenes, side-quest dialogue, location descriptions, faction and character voice rules, and branching structures. It remains draft-only: names, places, history, and text are not canon, do not receive production IDs here, and must be reconciled with current narrative data before use. The content is designed to attach to the quest, location, context, and consequence contracts in Plans 17–19, 21, and 22 without repeating their systems specifications.

### Dramatic premise and narrative boundaries

At the Switchback Sluice, the argument is not whether water matters. Everyone agrees that it does. The disagreement is whether a distant reading is sufficient to authorize a release when local machinery is worn and the receiving shelter has not confirmed its intake condition. Sena, a local mechanic, wants no one to operate the wheel on a guess. Edda Marr, a High Meridian Assembly delegate, wants a record that can be compared with other sites before the next seasonal route is planned. Neither character is written as secretly malicious. Their blind spots come from the work they are trying to protect: Sena overweights immediate physical evidence; Edda overweights procedure that works across a large network.

The emotional question is: can the player help these people make a responsible decision without claiming certainty they do not have? There is no hidden “correct faction” answer. Each route should let the player describe what was observed, leave an issue unresolved, or decline to make a public recommendation. The narrative must not imply that a character's consent is equivalent to a faction's consent, or that an accepted quest result automatically means a physical water release occurred.

### Location information sequence

Use information in layers so a player can orient before being asked to choose:

**Expedition preview.** “A raised track bends above a narrow channel. A wheel tower marks the crossing, but the route report does not say whether the lower platform is clear.” This establishes a visible landmark and uncertainty, not a hidden danger score.

**Arrival description.** “The sluice wheel is taller than the doorway beside it. Its handle has been wrapped where hands tend to slip. A strip of cloth hangs from the upper rail, faded to the color of old paper.” The player can understand the mechanism and its human use before dialogue begins.

**Inspection description.** “The gauge face carries two sets of chalk marks. One line ends at a clean notch. The other has been rubbed into the metal by repeated corrections.” The description suggests disagreement without claiming which mark is right.

**Shelter/safe-space description at Mothglass Nursery.** “Warm glass fogs above rows of shallow trays. Every pot has a name, even the empty ones. A repaired vent ticks at a steady interval, slower than the people working beneath it.” This gives the shelter a culture of care and maintenance while making no unsupported promise about shelter bonuses, warmth mechanics, or rest.

**Late-game High Meridian Perch description.** “The old signal frame faces three valleys. Someone has added a second sighting bar, then left it unpainted so no one will mistake the new measure for the old one.” The site introduces the Assembly's standardization goal and local caution without declaring faction control.

Each layer can be omitted when unavailable, but the key uncertainty cannot be communicated only through color, sound, or a single inspection line. Screen-reader and transcript text should preserve the distinction between reported reading, observed mark, and verified outcome.

### Character voice and personality cards

| Character | Public role and value | Voice pattern | Useful contradiction | Avoid |
|---|---|---|---|---|
| **Sena** | Local mechanic among the Lowwater Stewards; protects the crew from acting on a mechanism she has not checked. | Short statements, concrete nouns, gives instructions before explanations, repairs others' imprecise wording without scolding. | She knows the gate intimately but distrusts records from sites she has not visited. | Making her a magical engineer, universal expert, or obstacle who refuses all help. |
| **Edda Marr** | High Meridian Assembly route delegate; wants common methods so shelters can compare reports. | Longer, composed sentences; names the source and limits of a claim; often asks what would make a result repeatable. | She understands coalition logistics but can miss what a single damaged instrument means in the field. | Making her a bureaucratic villain or claiming that formal records decide local access. |
| **Rusk** | Runner attached to the nursery's supply route; knows where people actually wait and which paths close after heavy ashfall. | Fast phrasing, changes subject when worried, remembers people through small favors rather than titles. | He can navigate well but has only partial information about the sluice mechanism. | Giving him exact technical answers he could not plausibly know. |
| **Mina** | Nursery caretaker; maintains labels and distributes work so exhausted residents can take a turn away from the vent. | Patient, precise, speaks in invitations, avoids asking for more labor than she can account for. | She can protect her small group so carefully that she delays useful help to the wider route. | Treating kindness as a resource meter or making her a quest dispenser in every visit. |

The roles and names are examples. If current game cast or faction canon already contains a suitable character, adapt the scene to that owner-approved voice rather than adding four new persistent NPCs. The table supports writers with constraints and observable habits; it does not define relationship values, hidden emotion fields, or encounter availability.

### Scene one: the quiet wheel

**Entry text:**<br>
The wheel does not move when the wind touches it. The cloth on the rail does.

A woman under the tower roof tightens the strap on her glove. She does not look up until your boots stop at the painted line on the boards.

“Stand there, please. The platform takes weight badly at the edge.”

She taps the gauge casing with a wrench handle, once, then waits for the faint ring to settle.

“Someone brought you a reading?”

**Available responses:**
- “I heard two readings disagree.”
- “Can I look at the gauge?”
- “I brought a repair kit.” *(shown only if a supported inventory/quest condition permits the offer)*
- “I’m not here to decide what you do.”
- “Leave the conversation.”

**If the player asks about the disagreement:**<br>
“Then you heard the useful part. They disagree.” She sets the wrench on a dry board. “One mark came from above. One came from here. Neither one turns the wheel by itself.”

**If the player asks to inspect:**<br>
“Look from the line. If the casing is open, wait for me.” She points to a nick beside the lower mark. “That is a scratch. It is not a reading. I keep having to say that.”

**If the player offers help:**<br>
She looks at the kit before she looks at you. “A kit is useful when we know what is missing. What did you bring?” The owner receives the proposed item only if its contract accepts the item and quantity. If it does not, the response remains informational: “That may be useful elsewhere. I will not take it on a guess.”

**If the player declines to decide:**<br>
“Good. We need an inspection, not a vote from someone who has just arrived.” She turns back to the casing. “If you want to help, ask what we can prove.”

This entry scene keeps the dramatic ask understandable while preserving a safe exit. No response automatically opens the sluice or raises faction standing. The offered kit is a command only if the inventory and quest owners support it; the neutral branch remains valid in core-only content.

### Scene two: the three readings

When Edda is present, the graph uses a short hub-and-spoke conversation. The player can ask about the Assembly's method, share the local observation if it has been accepted, ask what can be delayed, or leave. All short branches reconverge on a planning question. The scene never forces the player to exhaust every topic.

**Edda on method:**<br>
“We compare the instrument, the time, and the person who took the reading. If one is missing, we mark the entry incomplete. I know that sounds slow. It is faster than asking every shelter to trust a number no one can retrace.”

**Sena, if the player invites her to answer:**<br>
“The upper team used a clean instrument. I believe that. This casing has shifted since then. I can show you where.”

**Edda, after the supported observation is shared:**<br>
“Then my copy should say exactly that: the upper instrument was sound when read; this casing has since moved. I can ask for a revised route. I cannot sign a release from those two facts.”

**Rusk, if the player has met him at Mothglass Nursery:**<br>
“They asked me to carry the note before the path changed. I carried it. I did not carry the path, though.” He glances toward the low track. “If the meeting moves, tell Mina before she sends anyone with the trays.”

The line with Rusk is relationship/visit-gated only if a current owner fact supports that he met the player. Otherwise, Edda summarizes the practical routing issue. The conversation should not store a visit counter merely to create a callback.

### Scene three: the public recommendation

The player is asked to select a recommendation, not to operate the mechanism. This is the central choice scene. The exact command and consequence depend on current owners. The prose can be authored provisionally while the game-state result remains blocked until the owner route is verified.

**Recommendation A — request another local inspection.**<br>
Sena lays the wrench down with its handle facing the edge of the board.

“That is a fair request. I want another pair of hands here before we move anything. Edda, can your route wait?”

Edda closes her booklet. “The route can wait. I will mark the estimate incomplete, not rejected.”

The journal copy should say that the report remains unresolved and that a new inspection is requested. It must not say the water is safe or unsafe.

**Recommendation B — publish only what is known.**<br>
Edda writes one line and reads it aloud before the player confirms:

“Upper gauge was read cleanly. Local casing shows movement. No release recommendation made.”

Sena listens once. “Keep the last sentence.”

This is the most conservative public result. A current quest owner can accept the report as an outcome even if no water or faction state changes. The line should not claim that the entire route has agreed.

**Recommendation C — support the local crew's pause.**<br>
Sena's shoulders lower a fraction, then she looks at the other workers.

“Thank you. I still need the crew to agree. I will ask them before I call it a decision.”

The player has expressed support; no collective decision is implied. If a relationship or faction owner records an effect, the result is shown through that owner's supported read model.

**Recommendation D — leave the recommendation open.**<br>
Edda closes the booklet without marking a line.

“We can leave the place blank for now. Blank is not the same as safe. It means we have not agreed to say more.”

This is a fail-forward route. It should preserve the main question, give a clear reason to return, and avoid portraying caution as failure. If the main quest requires closure, the quest owner can accept “unverified” as a distinct supported resolution; dialogue cannot manufacture that state by itself.

### Optional side-quest: one night before the turn

At Mothglass Nursery, Rusk asks the player to carry a covered lamp to the staging rail before the evening inspection. The task is about safe access and shared labor, not about awarding a hidden combat or skill bonus.

**Mina:** “The lamp is marked on both sides. Keep the cloth over it until the path bends. People see a bright window and start walking toward it.”

**Rusk:** “I can carry it. I can also find the path. I should not do both at once.”

Possible player responses are: “I’ll carry the lamp,” “Can someone else take the route?” and “I can’t help tonight.” Acceptance, assignment, and any item transfer must be represented through current quest/inventory owners. If the player declines, Mina offers a later shift only if the task owner supports another availability; no emotional penalty is inferred.

**On successful handoff:**<br>
Rusk checks the cover. “Good. Nobody has to guess where the turn is.”

**On a missed window:**<br>
Mina folds the lamp cloth and puts it away. “The inspection moved to daylight. The lamp did its job by staying dark.” The task may expire or partially complete according to the authored contract, but the main quest proceeds through its safe route.

This brief side quest shows character personality, shelter lore, and a small playable action. It does not add a new lighting system, companion command, or hazard calculation.

### Environmental text set

Use short inscriptions and found notes sparingly. Their prose should complement the visit rather than hide required instructions in decorative language.

**Painted line on the platform:**<br>
STAND BACK UNTIL CALLED.<br>
Below it, in smaller writing: WE KNOW WHERE THE EDGE IS. YOU MAY NOT.

**Nursery notice:**<br>
Two people on the vent route at a time.<br>
If you are tired, say so before you reach the ladder.<br>
The empty tray is still labeled because someone may return.

**Assembly travel slip:**<br>
Site: Switchback crossing<br>
Instrument: upper gauge, condition checked<br>
Local comparison: pending<br>
Public recommendation: none<br>
The final entry is left blank on purpose.

**Sena's tool tag:**<br>
Handle binds when wet. Do not pull harder. Tell me first.

**Old marker beside Sootstep:**<br>
Two cuts for the lower path. One for the road above.<br>
The third cut is not a number.

The last inscription offers mystery but not a hidden exact coordinate or proof result. If discovery content is not selected, it should remain a report or transcript fragment through an authorized route rather than appearing in every expedition.

### Branch chart and prose QA

The central scene's response types should be labeled as information, request, commitment, or exit in editorial review. A choice is not a branch simply because it changes a sentence; the team records whether it changes the current scene, a quest result, relationship, faction, world, or campaign outcome using the existing owner definitions.

| Choice family | Immediate scene change | Durable outcome candidate | Required author note |
|---|---|---|---|
| Ask about method | New factual dialogue; reconverges. | None unless a current knowledge/quest owner accepts a fact. | Speaker source and certainty. |
| Inspect the mark | Reveals a bounded observation. | Evidence only after an existing quest owner accepts it. | What the player can see and what it does not prove. |
| Offer kit | Cost/transfer may occur. | Inventory result; possible character task progress. | Owner, quantity, rejection and duplicate response. |
| Recommend second inspection | Meeting plan changes. | Quest follow-up or relationship/faction fact if supported. | Must not be represented as a public release. |
| Publish known facts | Current report can be accepted. | Quest resolution as “unverified” if supported. | Keep the exact uncertainty in the player-facing result. |
| Decline or leave | Scene closes. | Normally none. | Keep a return route and avoid shaming the player. |

Every graph path needs an accessible exit, a transcript route, and a return behavior if its owner result is pending. The protagonist can leave without asking every informational question. A relationship-gated or knowledge-gated variant should reconverge without withholding the main task. The map marker and journal use the same certainty language as the scene.

Prose review asks whether each sentence reports an observed fact, attributes a belief to its speaker, makes an inference, or asks for a commitment. It flags all dialogue that promises a resource change, opens a location, grants access, or settles a faction dispute without an owner-backed contract. Translators receive role, tone, gesture, and result context; each important branch is a complete line rather than a fragile collection of fragments.

### Production package and expansion placement

The base candidate is the arrival scene plus an informational hub and one accepted quest result. The character/side-quest scene, optional clue, alternate meeting location, and High Meridian coalition callbacks can be produced in phases. The late-game faction is a major narrative addition and should remain an expansion candidate until faction availability, access, and campaign inputs are verified. A faction's presence in prose is not proof that the faction system can represent its intended choices.

Production requires writing, continuity, data references, localization, voiced/non-voiced presentation decision, accessibility, map/journal copy, owner command wiring for accepted actions, result feedback, and focused route review. Reuse a common arrival pattern, but preserve distinct character attention: Sena notices wear and hand placement; Edda notices sources and missing fields; Rusk notices who is waiting; Mina notices fatigue and maintenance. That repeatable attention pattern gives the cast recognizable personalities without requiring an emotion simulation.

The packet is ready for an implementation proposal when all speaker, location, quest, and faction references are verified; the content bundle has a declared core/expansion profile; every command-bearing choice names its owner and result copy; and the central recommendation reaches a truthful conclusion on both the normal and fail-forward route. If those gates are not met, keep the prose as provisional planning material and do not claim it is playable content.

### Additional arrival and return prose cards

These fragments extend the Tidemark packet with short location-specific text for distinct points in a visit. They are alternatives for a writer to evaluate, not locked canon lines. Each can be shortened, localized, or replaced after checking the approved world voice.

**Rillstep Intake, first approach:**<br>
The channel wall hides the water from the path. A thin white line clings to the stone where the level used to reach. Someone has placed a shallow cup beneath the drip, then covered it with a board.

**Rillstep Intake, after inspection:**<br>
The cup is dry. The stain is not. You can see where the mineral edge continues behind the wall, but the path does not tell you when it formed.

**Mothglass Nursery, late return:**<br>
The panes have cleared. The rows look smaller in clean light, and the empty tray still has its label. Mina sets a tool down before she asks how the inspection went.

**Lintel Camp, after a report is accepted:**<br>
A fresh slip hangs beneath the old route notice. It does not cover the earlier account. The two pages remain side by side, each marked with the day it was written.

**Switchback Sluice, unresolved return:**<br>
The cloth has been folded over the rail now. No one has tied it into a signal. The wheel stands where you left it, with the lower mark still waiting for another reading.

These lines observe surfaces and people rather than announcing invisible state. The map or journal carries the actual accepted result. If no owner fact changed, use ordinary ambience rather than a false callback. If a site is unavailable or the package is disabled, do not show a fragment that implies the player visited it. For localization, retain the core distinction between observation and inference, especially in the Rillstep descriptions.
## Continuation pass 13 — The Signal That Outlived the Road: multi-scene story packet

This pass adds a second provisional narrative line built around unsafe information rather than a missing record. The draft arc is **The Signal That Outlived the Road**, part of the Farline Circuit. It is distinct from the Tidemark sluice story: Tidemark concerns uncertain physical readings and local water procedure; Farline concerns messages whose old meaning persists after a route changes. They may inhabit one broad world only if continuity review confirms the geography and factions fit.

The two coalition concepts have distinct fictional remits. The High Meridian Assembly is a water-stewardship network from the Tidemark proposal. The Farline Compact is a route-message and courier network. Neither automatically commands the other, and membership overlap is not assumed. If later canon chooses to combine them, that is a deliberate narrative/authority decision, not something this content pass silently implies.

The Farline story uses the Lantern Wardens as local route stewards. The Wardens are willing to close a dangerous approach, but their knowledge is local and may not reach distant shelters. The Compact keeps older message standards alive so that reports can travel between settlements; its weakness is that a signal can outlive the conditions it described. Neither group intends to mislead people. The player helps them decide how to retract or qualify an old signal without erasing the history of why it once mattered.

### Story arc and player-facing contract

The arc begins when an old route tone is heard or reported near Half-Span Shelter. The tone once meant “the raised crossing was inspected.” The crossing's current status is unknown. A courier, Yara, stopped forwarding the tone after seeing that the old approach no longer matched the ground. The local Wardens want to mark the message as unverified. The Compact wants to identify which relay emitted it and issue a correction through its network. The player can help inspect, compare, and recommend; they cannot force universal adoption by choosing a dialogue response.

The player-facing contract:
- hearing an old signal is not proof that the represented route is open;
- a local observation does not certify every connected site;
- a faction request is not an accepted faction result;
- the player can refuse to carry a message or speak for a group;
- the main quest can conclude with a narrow, honest finding;
- an optional clue can deepen the explanation without blocking progression;
- a late-game Compact outcome is only presented if current content and owner contracts support it.

### Characters, personalities, and voice distinction

**Yara, route courier.** Yara stopped forwarding the old tone because she could not verify the road, then became afraid that the decision looked like negligence. She is alert to delivery details: seal, wrapping, route marks, how long a packet has been exposed. Her speech is quick at first and becomes careful when she describes a decision. She does not ask the player to absolve her. Her arc is learning to state what she knows without taking responsibility for facts outside her route.

**Tovan, Lantern Warden.** Tovan maintains the short route between Half-Span and the relay approaches. He uses plain, practical sentences and marks uncertainty with his hands: a palm facing down means “wait here,” a finger tracing the path means “known to me.” He is proud of local knowledge, sometimes too proud of it. His arc is accepting that an outside report can be useful if it is clearly attributed.

**Iri Venn, Farline Compact dispatcher.** Iri helps shelters share message formats and repair notes. She names the source of a report before giving its conclusion. Her habit is to ask for a route, date, and receiving point, which can sound cold when people are frightened. Her weakness is believing that a carefully formatted message will reach the right person in time. She is not a remote villain and cannot sign for a local crew.

**Pell, signal apprentice.** Pell has learned how to keep a relay plate legible but has never been allowed to declare a route safe. They test language aloud and ask whether a word means “heard,” “seen,” or “confirmed.” Their curiosity creates optional teaching dialogue. They are not a child companion or a free skill tutor; their role and age are subject to canon review.

**Lantern Warden crew.** The group shares tools and short-route reports. They value the right to close a crossing without a distant committee overruling the person standing at its edge. Their blind spot is that reports may not travel far enough to protect someone outside their reach.

**Farline Compact members.** The network values interoperable terms, reliable couriers, and corrections that reach previous recipients. Its blind spot is that a clear protocol can hide unequal access to a route. Membership does not guarantee that every shelter accepts each recommendation.

### Location and shelter lore cards

**Half-Span Shelter.** Built beside a route that no longer crosses as it once did, the shelter's earliest wall contains brackets for a board that was moved indoors after repeated dust storms. People still place paper in the empty brackets before deciding where to post it. The shelter teaches a difference between public information and information that needs a person to explain it. It can host Yara, Tovan, an expedition briefing, or a return conversation only when current availability facts permit.

**Siltglass Relay.** The relay is older than the present route marker. Its plate holds a tone label and a line of faded instructions. The player can inspect the label without activating the signal. Service access requires the current interaction owner. A receiver or sound cue appears only through an existing supported audio route. The relay's lore concerns maintenance continuity: a repaired plate can carry old meaning as faithfully as new meaning.

**Crowstep Span.** The remaining abutment shows where the old approach ended. No description claims the current route is passable until the route owner confirms it. Wind moves through the open gap with a low resonant note; this is environmental sound, not automatically the obsolete tone itself.

**Old Echo Cut.** A side path carries a second set of notches where a relay worker once marked that no confirmation came back. The marks reveal that message silence was once an expected status. Their meaning is optional context, not a hidden binary proof that the Compact failed.

**Farline Overlook.** The late-game meeting place has two sighting bars, one used for the old network and one left unpainted for future comparison. The view ties local terrain to a wider route network without showing a map that claims every route is open. It appears only when a supported late-game offer and package profile permit it.

The shelter lore in this packet is not a shelter service definition. Food, rest, protection, crafting, occupancy, and resource availability continue to belong to current shelter and world systems.

### Scene one: the tone at Half-Span

**Arrival prose:**<br>
A tone reaches the shelter from somewhere beyond the ridge. It has two falling notes and a pause long enough to invite an answer.

No one answers.

A runner at the door lifts a hand. The people inside keep working until the sound has ended.

**Yara:** “That was the old route signal.”

**Player responses:**
- “What did it used to mean?”
- “Did you send it?”
- “Can we find where it came from?”
- “I only came to report that I heard it.”
- “Leave.”

**If asked what it meant:**<br>
“Someone had walked the upper crossing and checked the boards. That was the message. Not that it would stay safe. Not that it was safe everywhere. One route, one check, one time.”

**If asked whether she sent it:**<br>
“I carried the plate copy. I stopped carrying it after the path changed.” She rubs the seam of her coat. “I should have told them sooner. I couldn't say the route was open, so I said nothing. Those aren't the same thing, but people heard them the same way.”

**If asked to locate the source:**<br>
Tovan sets a narrow strip of paper on the table. “Start with the marker. If the signal came through a relay, the route report should name it. If it came from someone repeating an old tone, we need to know that too.”

The scene's main purpose is to establish uncertainty and invite an investigation. It does not automatically accept the quest or add a location marker. The player can leave and return if the current quest/content owners support that path.

### Scene two: at Siltglass Relay

**Inspection prose:**<br>
The plate is cool beneath the grit. Two screw heads have newer scratches than the engraved label. The instruction line still reads: INSPECTED ROUTE — RETURN ONLY IF CHANGED.

A section of the lower plate has been replaced. The new metal has no date.

**Pell:** “If the date is missing, do we call the whole thing old?”

**Tovan:** “We call the date missing.”

**Player responses:**
- “Can the tone be dated from this plate?”
- “Which route did someone inspect?”
- “What would a correction need to say?”
- “I won't activate the relay.”
- “Return to the shelter.”

If the player asks whether the tone can be dated, Pell says, “Not from the sound by itself. A note is not a clock.” Tovan adds, “The plate can tell us when it was marked if the date remains. This one doesn't.” This line does not prove when the message was emitted. It gives a truthful limit.

If the player asks which route was inspected, Tovan points to the upper bend but avoids claiming that it remains accessible: “This is the named approach. We need today's route state before anyone walks it.” The map follows the current route owner.

If the player asks about correction wording, Pell offers: “Heard here; route not confirmed.” Iri's later proposal can refine the phrase, but it should not overwrite the local crew's account.

### Scene three: Yara's last delivery

Yara's conversation is a hub-and-spoke scene. The player can ask about timing, the packet she carried, why she stopped, or whether she wants the player to help. Optional discovery gates can surface more precise questions, but the ungated route must still reach her practical account.

**Yara on timing:**<br>
“I saw the marker the morning after I stopped. The old tone came through the night before. I don't know whether the tone was sent then or whether someone found the old plate and played it again.”

**Yara on the packet:**<br>
“The wrapping was clean. The route marks were not. I kept the packet dry and carried it back. That part I can prove. I can't prove who heard the first message.”

**Yara on why she stopped:**<br>
“I didn't want to carry a warning that said ‘safe’ when I couldn't look at the road. I should have carried a correction. I didn't have one.”

**If the player says silence can also mislead:**<br>
Yara waits before answering. “Yes. It can. I thought I was refusing to make the wrong claim. I didn't think about what the empty space would say.”

This is not a confession of sabotage. A correct ending may acknowledge that the courier delayed the correction and that the delay had consequences, without naming a specific injured or lost group unless current content establishes it.

### Scene four: the public message meeting

The player can attend only if the faction/contact and location owners confirm eligibility. The meeting may end with a proposed correction, a local-only report, a request for a later trial, or no endorsement.

**Iri:** “I can put three labels on the message: heard, inspected, confirmed. We can send the first label now. I won't use the third unless someone owns the inspection.”

**Tovan:** “Then the label is useful.”

**Yara:** “And if nobody answers?”

**Iri:** “We send ‘no confirmation received.’ It isn't a blank. It tells the next person not to fill in the answer.”

Player responses may ask who receives the correction, whether local crews can override it, what happens after no reply, or whether the player endorses the format. The important branch is consent: a personal recommendation cannot substitute for faction adoption. The owner result determines whether the Compact accepted the statement.

### Scene five: return and consequence

After a supported result, the player can revisit Half-Span. If a correction was accepted locally but not adopted by the Compact, Tovan might say, “Our notice changed. Their schedule hasn't.” If the Compact accepted only the vocabulary and not the timing, Iri says, “They kept the labels and declined the route interval.” If no durable fact changed, the scene uses ordinary greeting instead of a fabricated callback.

Every end state preserves the core distinction:
- **heard:** a signal or report was encountered;
- **inspected:** a specific location/object was inspected;
- **confirmed:** an appropriate owner accepted sufficient evidence for the claim;
- **corrected:** an authorized content/world/faction owner accepted the correction;
- **adopted:** a faction authority accepted a policy, if the game supports that state;
- **unresolved:** no claim beyond known evidence is made.

These words should be localized as distinct semantic units. The dialogue should not treat them as synonyms for dramatic rhythm.

### Side-quest anthology and optional scene chains

These side-quest seeds widen the Farline arc without putting every piece of lore on the critical path. Each can be promoted independently after its data references and current owner seams are verified.

#### A reply should have a source — faction side quest

The Compact has received a phrase that appears to answer the old tone, but the sender is unknown. Iri asks the player to help determine whether it is a legitimate reply or an old copy being repeated. The story is about attribution and restraint, not a mystery villain.

**Entry:** an accepted main quest observation makes Iri's public offer eligible. The faction/quest owner controls the offer; the player is not automatically enlisted.

**Scene sequence:** At Lantern Yard, Iri shows the exact phrase only if the current content/disclosure contract permits it. At Siltglass Relay, the player can compare the phrase with an authored plate. At Half-Span, Yara can say whether she carried a packet that used the wording. The branches offer different source context and reconverge on “source identified,” “source uncertain,” or “no source found,” based on accepted evidence.

**Failure:** if one location cannot be selected, the player can wait, accept an incomplete source report, or decline the review. The quest must not assert forgery based only on missing provenance.

**Reward and reuse:** the owner may record an evidence result or new offer. No faction standing is presumed. A “compare report with plate” pattern can be reused in other authored contexts; this particular phrase and source chain are unique. Medium/high production cost because it touches several speakers and a hidden disclosure boundary. Expansion placement.

#### Carry the Unlit Cell — character/escort scenes

**Before departure:**<br>
Yara holds the wrapped cell by its cloth loop. “It stays dark until we know who is receiving it.”<br>
Pell asks, “Could we test it here?”<br>
Tovan: “If we test it here, we have a test here. We do not have a route check.”

The player can accept the escort, ask about the destination, suggest that the crew wait, or refuse. Any actor assignment or item custody is routed through current owners. The dialogue offers no hidden test that turns the object into a guaranteed navigation tool.

**At a safe handoff:**<br>
Yara checks the seal before handing it to the recipient. “This tells you which case it came from. It does not tell you what the road looks like.”<br>
The recipient can accept only if the current item/quest owner confirms transfer.

**If the outing is interrupted:**<br>
Tovan: “We got everyone back. The cell is still covered.”<br>
Yara: “Then it is still ours to place when the route is ready.”<br>
These lines are shown only if current owners confirm a safe return/custody result. An unknown item state uses neutral recovery copy and a status query.

**Failure and placement:** the escort can end as completed handoff, secure return, deferred delivery, or owner-defined failure. No character is killed by narrative default, and the item is not consumed unless its owner says so. The named character chain is high cost and likely expansion content; its transport pattern can be reused only if future tasks share the same item custody semantics.

#### Three Knocks, No Answer — environmental discovery episode

The discovery begins when the player notices that the second set of marks has been arranged for a reply that never came. It can be discovered from a clue or from the Old Echo Cut visit. The player learns that silence was once an explicit message, but the content does not claim that every listener understood it.

At the location, a brief optional inspection exposes three interpretations:
- the marks are a signal grammar;
- they are a maintenance reminder;
- they are simply a sequence someone used to count visits.

A supported knowledge gate can allow the player to ask a sharper question. The ungated route states that the marks are ambiguous. A current quest/evidence owner determines whether the observation counts as accepted clue proof. The player may leave without submitting an interpretation.

**Prose fragment:**<br>
The first notch is clean. The second is crossed by a later cut. The third has been worn smooth enough that a thumb can find it in the dark.

**Dialogue if Yara is present:**<br>
“I thought the third mark meant no answer. Maybe it meant no safe answer. I don't know who cut it.”

**Outcomes:** the clue can reveal an optional location hint, enable a return conversation, or remain unresolved. It cannot be the only way to complete the main quest. Production cost is medium: one secret location, one clue event, one short dialogue node, and localization. Expansion or optional base content.

#### A route too quiet — survival and timed opportunity

At a supported meeting, the Wardens offer a daylight route check before the next dust movement. The player can help carry tools, ask for another date, or let the opportunity pass. The time window uses the existing world clock if applicable. There is no independent timer in the dialogue scene.

**Before accepting:** the player is told when the check is expected and what expires: this specific group inspection. The main quest and the existing route map remain valid independently.

**On arrival:**<br>
Tovan listens at the marker. “No answer from the ridge.”<br>
Player: “Does that mean the route is empty?”<br>
Tovan: “No. It means we didn't hear a return.”

The line demonstrates the difference between silence and verified absence. If the current environment/expedition owner provides a safe/unsafe result, the scene can report it. Otherwise it remains an observation.

**After expiration:** the meeting becomes a report-only conversation or closes as expired. It cannot secretly fail the campaign. The player can still use another core route.

#### The Notice That Stayed Up — location-based revisit

At Lantern Yard, a stale notice remains beside a new one so the player can see that correction does not erase history. The player may ask who owns the notice, read it, or request removal. Only a current location/faction owner can change a persistent notice state. If there is no such mechanic, the paper is static environmental prose and the player simply receives an explanatory line.

**Mina:** “If we take every old notice down, someone will think it never happened.”<br>
**Iri:** “If we leave every old notice up, someone will follow the wrong one.”<br>
**Player:** “Can the old one stay, with the correction beside it?”<br>
A permitted response can propose a treatment; the current owner determines acceptance. The scene never implies that one conversation has updated every copy elsewhere.

### Character arc beats and reveal order

The cast's progression is expressed through changed phrasing and supported events, not new emotion meters:
- **Yara:** begins by defending why she stopped; later separates what she observed from what she feared; ends able to say “I don't know” without leaving the correction unsent.
- **Tovan:** begins with a local-only warning; later reads a Compact report without dismissing its value; does not surrender local decision authority unless an owner-supported faction outcome says so.
- **Iri:** begins with a standard form; later adds an “unanswered” label because of what the player brought back; never claims that a published label means the route is open.
- **Pell:** begins by asking whether the sound proves an event; later identifies the difference between a sound cue and its source; their insight can be a knowledge-gated line but not the only solution.

Order the revelations so the player has a clear reason to investigate:
1. The old tone had a specific, limited meaning.
2. The local route changed after that meaning was assigned.
3. The courier stopped forwarding the tone and did not supply a correction.
4. The relay plate cannot date the event by itself.
5. Silence and absence of response have several possible meanings.
6. A public correction can narrow the claim but not certify the route.
7. The factions can agree on vocabulary while disagreeing on who controls local access.

Do not reveal all seven points in one long exposition node. Spread them across arrival, inspection, optional discovery, side quest, and late-game negotiation. Each scene should provide a useful next action or a satisfying exit. The journal can summarize the current strongest facts without becoming a transcript dump.

### Dialogue reuse and scene-production cards

A reusable scene pattern has a clear mechanical shape: **arrival cue → source question → optional clarification → owner-backed response or no-op → re-entry summary**. The wording remains specific to the speaker and location. An author cannot paste Iri's careful source attribution into Yara's fear without revising voice and intent.

For each scene, the writer submits:
- dramatic question and intended player decision;
- available entry routes and who can make them eligible;
- scene start text and its information certainty;
- every response label and whether it commands, branches, or reconverges;
- complete copy for every relevant result;
- character source and limit of knowledge;
- interruption, close, and revisit behavior;
- location availability and package fallback;
- transcript length and localization context;
- branch-pruning evidence for unreachable or redundant nodes.

Branch cost depends on outcome difference, not just node count. Four responses that reconverge with distinct voice are inexpensive. One response that changes faction access, map visibility, quest eligibility, and future ending text is expensive even if its node graph has only two branches. Estimation records narrative, data, localization, owner integration, UI feedback, accessibility, and verification effort separately.

### Production routing and expansion placement

The core story candidate is the short main investigation at Siltglass Relay with one source-aware conversation. Optional scenes are added in this order:
1. Yara's character arc and a neutral return route.
2. Three Knocks discovery.
3. Lantern Warden/Compact review conversation.
4. Side quest or survival window.
5. Farline Overlook late-game decision and callback.

The Farline faction and public correction arc are strong expansion candidates because they add a major coalition, multiple locations, cross-faction choices, and persistent callback expectations. The core story can still close with a narrow inspection result. If the content budget cannot support localization and branch QA for faction effects, preserve the short local story and hold the broad campaign interpretation.

All scenes remain proposals until canonical cast/faction review, content ID assignment, location compatibility, and owner API inspection. Existing game systems decide whether an audio cue, message, crafting task, expedition actor, or shelter interaction can be presented. Do not fake those operations with narrative scripting.


## Continuation pass 13 — scene treatment, playable prose, and dialogue re-entry

### Scene design intent

The Farline dialogue packet should show a community trying to decide what a sound means while several people remember different procedures. A scene advances through an actionable exchange. It gives the player a chance to inspect, question, offer, refuse, or choose how to communicate. Dialogue must not become a transcript of the design document. The player needs enough information to make a decision, but the scene should preserve the uncertainty that gives the decision weight.

All material below remains proposed content. Names and scene wording are provisional until a canon and collision review confirms that no current character, group, or location already owns the same function. The snippets are compact samples to establish voice and interaction shape; production writing should adapt them to the actual dialogue format, line-length constraints, localization, and current UI. Choices and effects named here are conceptual, and must map to supported current owners.

### Scene 6: A measure of silence

**Setting:** half-span shelter, after the player returns with a timing note and a damaged service card.

The shelter has two clocks. The first is a wall clock with no glass. The second is a kitchen timer used to make ration lines move. Yara sets the kitchen timer beside the card and waits for the player to finish. Pell is repairing a stove hinge. He stops only when the timer rings.

Yara: “How long between pulses?”

The player may give the measured interval, show the card, say that the count is uncertain, or decline to report it.

If the player gives the interval:

Yara: “That is longer than the board says.”
Pell: “Board does not have a battery.”
Yara: “The board has a hand. Someone wrote that interval.”
Pell: “Someone wrote what they wanted the next person to do.”

The player can ask whether the discrepancy proves alteration. Yara answers that a difference is evidence of a difference, not proof of intent. This line supports the plan’s evidence theme without making her a lecturer. The player can ask Pell if the tone could be machinery. He says a loose regulator can hold a rhythm “for a while,” then begins listing which parts usually fail first. The exchange shifts into practical diagnostic speech.

If the player shows the card but not the timing:

Yara: “The cut is straight.”
Pell: “Straight cuts still tear.”
Yara: “Not at that edge.”
Pell: “Could be a tool. Could be somebody keeping the paper from splitting.”

The dialogue offers a physical inference and an alternative. It should not label either one the correct answer. If the player has a relevant skill, it can reveal a small observation such as the direction of pressure marks; the skill does not unlock a line that declares the culprit.

If the player reports uncertainty:

Pell: “Good. That saves me having to argue with a number I never saw.”
Yara: “It does not tell us what to post.”
The player is invited to choose what can safely be communicated at this confidence level.

**Playable consequence:** the group’s notice can carry a measured interval and source, a warning about a repeating sound without a duration, or an explicit statement that the report remains incomplete. These options are not moral labels. Each can be useful under different circumstances. The choice should only change the current notice and approved downstream response facts; it should not imply that either Yara or Pell permanently agrees with the player.

### Scene 7: The third copy

**Setting:** lantern yard, in the narrow room where route cards are dried above the stove.

The player discovers a third copy of a message. Its paper is newer than the two previous copies, but the writing is weaker and the date is missing. Iri Venn recognizes the shorthand. She says the mark belongs to a courier who copied the boards “when the boards were too wet to read.” She remembers the practice, not the individual writer.

Iri: “The mark says it crossed through here.”
Player response: “Then this is the source?”
Iri: “No. It is the route.”
Player response: “Someone had to carry the first copy.”
Iri: “Someone does. A route mark is not a name.”

The branch turns on the player’s wording. If they call the copy the source, Iri corrects the category. If they ask what the mark proves, she says it proves that someone expected the message to continue. A knowledge-gated response can note that a different mark usually records receipt, but the current copy has neither a receipt mark nor a refusal. The absence is relevant but not conclusive.

A relationship-based line can be available if the player has previously backed Iri’s recordkeeping. She does not disclose a secret. Instead, she explains why she refuses to guess aloud: “If I put a name in the blank, somebody will copy the name too.” If the player has dismissed her earlier, she states the same boundary more formally and offers to let the player inspect the drying rack alone.

The player can ask permission to preserve the copy, leave it in the yard’s archive, or take a sketch. Each action has a distinct practical consequence. Removing the paper might protect it from moisture but deprive the route of a working reference. Leaving it preserves access but risks further damage. A sketch supports investigation but cannot retain every material detail. The scene can complete a character objective through a choice of evidence handling instead of a fetch-and-return loop.

### Scene 8: The public wording

**Setting:** a notice rail at the edge of the yard, after the player has enough information to choose a message.

Pell provides three pieces of chalk. Yara holds the paper flat with two fingers. The player selects a line or writes a supported short label through an existing choice set. The system should not invite unrestricted text if the current game does not provide a safe, persistent player-authored text feature.

Possible notice treatments:

- “A repeating mechanical tone was heard at Siltglass. Source not confirmed.”
- “Do not travel toward the old relay on the basis of the tone alone.”
- “The relay interval differs from the archived board. Further comparison needed.”
- “No public notice. Keep the record available for inspection.”

The first protects the distinction between observation and source. The second emphasizes immediate travel safety. The third is useful for investigation but may not serve someone with no access to the archive. The fourth protects against spreading a weak claim while leaving travelers without a warning. Each notice should have a short and long presentation where required. The short version carries the practical instruction; the long journal record carries provenance.

Yara: “Write what we can stand behind.”
Pell: “That is easy. Write it small.”
Yara: “People still read small.”
Pell: “People read a warning faster when they are cold.”

The exchange illustrates an ordinary disagreement about legibility and urgency. It should not turn into a dramatic speech. The player’s choice is the dramatic action.

### Re-entry after choices

Dialogue graphs often fail at the return visit. If the player revisits the notice rail, the scene should acknowledge the current notice, not replay the full authoring scene. A short hub response can point to the posted wording and offer one useful action: review its source, replace it if the player has new evidence, or leave it in place. A replacement should retain the prior notice in authored world history only if the current world-state owner can represent that fact. If not, use a simple current-state line and avoid promising a visible revision history.

A character who has already spoken on the topic can still be spoken to about another subject. Dialogue selection must not let a completed Farline scene monopolize the hub. Use a priority order that favors urgent quest progression, then newly eligible character content, then ordinary ambient topics. Within the Farline topic, prefer a concise response that reflects the last known action. Avoid replaying a long branch on every shelter visit.

If the player has no new evidence, the topic should acknowledge that without inventing a countdown. If a time-sensitive route notice becomes stale, offer a review prompt only when the underlying world fact has changed or the authored expiration condition is reached. Repeated visits should not generate escalating dialogue pressure. The story’s tension comes from the consequences of communication, not from a character nagging the player.

### Reusable scene forms

The story can use a limited set of scene forms while retaining authored specificity.

**Evidence handoff.** A character receives an object or report, distinguishes observation from conclusion, then gives the player one next action. Use for the relay card and route sketch.

**Two-person disagreement.** One character advocates prompt action, another careful verification. The player can ask each a practical question and select a temporary policy. Reuse only when the underlying stakes differ; do not repeat the same argument with a new prop.

**Knowledge correction.** An NPC identifies the limit of a mark, tool, or phrase. They correct a category without revealing the final answer. Use for route marks and receipt notation.

**Notice composition.** The player selects audience, certainty, and urgency through authored options. The consequence is the notice state, not a hidden persuasion score.

**Return acknowledgement.** A short line reports what changed since the last visit. It should be conditioned on a real event or current state, not a visit counter unless repeated visits themselves have a design purpose.

**Closure conversation.** A character recognizes the player’s chosen handling of uncertain evidence. The character may agree, remain cautious, or raise a practical concern. It closes a personal beat without claiming that the whole community adopted the choice.

### Side chain: A lamp for the crossing

This small chain gives the Farline theme a physical counterpart. At Crowstep Span, the player finds a marker lamp with an intact casing and an improvised wick. A posted note says the lamp was lit when someone had crossed. It does not identify who crossed or whether the route was safe.

The quest begins through environmental inspection or an invitation from a character already repairing the span. The player can salvage the lamp, repair it in place, or leave it unlit and record the hazard. Each option should fit the existing crafting and location-state mechanics if they can support it. The story question is whether a signal of passage should be treated as evidence of safe passage.

A repair route needs available parts and a proof of installation. A salvage route yields a useful item only if the inventory authority supports it and the item has an actual gameplay use. A record-only route is still a valid resolution: the player adds a warning that the marker’s meaning is uncertain. No option should award equivalent material rewards if their costs differ substantially, but every path should produce a meaningful closure.

On a later visit, a small callback can reveal that someone moved the lamp to the sheltered side of the span. It may indicate use or merely protection from weather. A companion with relevant route experience can note the distinction. The map can retain the crossing’s risk state only through an existing location owner. If persistent location updates are unsupported, keep the callback as dialogue and do not invent a new location save field.

### Quest consequences translated into prose

When an authored effect updates a quest, relationship, faction, or location, the next line should report the consequence in the correct voice. A quest result can be plain: “The source remains unverified.” A relationship reaction belongs to the affected character: “You kept the copy intact. I can work with that.” A faction response may be indirect, such as a courier asking for the source note before accepting the next report. A world response can be visible in an altered notice or changed route access. The dialogue packet must not claim a consequence that has no state consumer.

The strongest return lines are specific but not encyclopedic. Do not have a character summarize every previous choice. Let them react to the one consequence relevant to the current conversation. If the player chose silence, Pell might say, “The rail stayed clear.” He should not assert that the whole settlement is safe. If the player posted an unverified warning, Yara might report that two travelers asked where the tone came from. That is a concrete response, not an automatic moral judgment.

### Voice and line economy

Yara speaks in short questions and distinguishes records from interpretations. Her caution comes from handling copied information, not from broad cynicism. Pell thinks in terms of time, cold, and whether a person will act before reading the entire note. He can be impatient without being cruel. Iri speaks about what a mark can prove and what she will not attach to it. Tovan, where present, speaks from movement and delivery constraints: which crossing is passable, which person will carry a message, and how much can fit in a satchel. They should not all use the same vocabulary of “truth,” “memory,” and “trust.”

Line reviews should check whether each speaker could plausibly say the line while occupied. A person holding a wet card may ask the player to keep it flat. A repairer may answer while maintaining the tool in motion. A courier may interrupt because departure time matters. These small physical contexts differentiate voices more effectively than additional lore paragraphs.

### Graph and prose acceptance checks

For each scene, verify that every response has an identified effect or is explicitly cosmetic; that the next node is reachable; that a knowledge-gated line has a real supported source; that the same fact is not introduced by two supposedly independent witnesses; that a refusal has a coherent continuation; and that return visits do not replay a completed scene. Review both branch text and the player’s journal summary. The journal should describe what the player actually did, not the scene the author expected.

A focused prose review should also confirm: no line turns uncertainty into supernatural implication; no faction is a single opinion; the player can state “I do not know”; no authored text claims a location changed if it did not; and no branch uses a hidden emotional state to punish a player for a reasonable choice. The goal is a memorable conversation because the player has to decide what to write, not because the characters deliver a thesis about communication.

### Scope control

These scene packets are an expansion layer. The core game needs a maintainable dialogue graph that can represent linear scenes, short reconverging branches, hub topics, conditions, and supported effects. The Farline content should prove those shapes with a small number of nodes. Relationship-gated variants, faction-specific lines, repeated-visit memory, skills, and hidden emotional states are optional layers only where a current owner or approved architecture supports them. Do not require every dialogue feature for every scene.


### Scene 9: What the notice did not say

**Setting:** a route shelter on the player’s return after the notice has circulated.

Tovan has a folded copy of the warning. One corner is held down with a clean washer. He says he passed the message to two travelers who were already preparing to leave. They did not turn back; they waited until morning. He cannot say whether the warning changed their minds or only gave them a reason to delay.

Tovan: “They asked if we knew what made the sound.”
Player response: “What did you tell them?”
Tovan: “That the note did not say.”
Player response: “Did they believe you?”
Tovan: “They asked the next question.”

If the player posted a sourced mechanical account, Tovan may add that one traveler wanted the maintenance card copied. If the player posted an unverified safety warning, he says they asked which route remained open. If no notice was sent, he reports only that the travelers waited near the shelter and asked about the crossing. This last line must not pretend a public message existed.

The player can ask whether the warning helped. Tovan answers: “It gave them another thing to weigh.” This is not a verdict. The player may ask whether they reached their destination. If the campaign state has no supported travel outcome, Tovan says he did not see them again. Do not manufacture a world consequence to provide emotional closure.

The scene ends with a concrete task: Tovan asks whether the current copy should remain with the shelter’s route cards. The player can approve, retrieve, or mark it for review if those actions match the available item and location owners. The choice gives the original decision a human-scale callback while preserving the limits of what the game knows.

This scene is optional expansion prose. Its purpose is to acknowledge a consequence and demonstrate uncertainty in a grounded way. It must not become a mandatory new quest stage if the story can close without it. If the recipient character is unavailable, a short shelter notice can report that the copy was placed with the route cards, provided the relevant owner can represent that state.

### Dialogue transcript review sheet

Before scene text is entered into the canonical dialogue records, assemble a transcript sheet that lists speaker, location, scene purpose, player responses, condition, effect, next node, and fallback. Keep each spoken line adjacent to its triggering context so reviewers do not need to reconstruct the graph from prose. For any important line, note whether it is required for comprehension, optional flavor, or a correction. This helps localization and ensures that a skipped optional branch does not leave the next node unintelligible.

Read the scene aloud once for practical cadence. Lines should sound like people working in a shelter or route yard, with room for interruption and silence. Replace repeated thematic phrasing with physical actions: a card held flat, a timer reset, chalk dust on a sleeve. Then read only the player responses in sequence. If every response is a version of “tell me more,” add an actual choice or remove redundant options.

The reviewer should also inspect the scene without stage directions. The dialogue must still make sense when the game cannot render the proposed animation or prop close-up. Conversely, stage directions should not contain the only evidence. If the player needs to know that a mark is missing, provide an inspectable detail or a spoken acknowledgement in a supported interaction.

A transcript sheet is a production aid, not another authority. Once dialogue is integrated, the canonical record controls the text. If the sheet is retained, identify it as generated or review-only and provide a way to detect drift. The release handoff points to the actual data path and the consumer that presents it.

### Presentation fallback

Every essential exchange needs a fallback if a character animation, portrait, prop view, or optional location overlay is unavailable. The line and response should still communicate the actionable fact through the current dialogue surface. A missing presentation asset must not remove a clue or choice. If a scene requires the player to inspect a mark visually, ensure an established examine interaction exposes its relevant text and that the game can identify which fact was observed.

The fallback is not a second scene. It is a reduced presentation of the same authored event, with identical supported effects and a clear route to the next node.

### Player response audit

Review each response for both intent and implication. “Post the warning” should cause the player to expect communication. “Keep the record here” should imply that the source remains accessible. “I cannot confirm that” should not be read by later dialogue as denial. If a response combines two intentions, split it unless both effects are inseparable and clearly described.

Do not write a response that only exists to let the player express the author’s favored interpretation. Give the player practical language: ask for another source, warn without naming a cause, preserve the paper, or wait for a safer route. Refusal and uncertainty are valid responses. They should not be framed as cowardice or ignorance by the narrator.

After choosing, show the immediate change in the scene or journal. The acknowledgement can be quiet, but it should prove the game accepted the input. If a choice changes only tone, label it cosmetic in review so the team does not promise downstream memory.

## Continuation pass 14 — The Empty Shift authored scenes, competing accounts, and shelter voice

### Provisional cast and voice boundaries

The cast is intentionally small. Mara Venn is a shift clerk who keeps records dry and treats categories as practical tools. Oren Vale is a former valve worker who dislikes having his name assigned to work he no longer performs, but he may still cover a task when asked. Nessa Pike manages current shelter duties and remembers people through what they did, not through old job titles. A fourth person, Sella, runs the evening meal queue and can explain how the roster affects portion planning without acting as a universal narrator.

These are placeholders pending canon review. Existing survivors may fill the roles. If an existing character is reused, their established history, profession, personality, and relationship state take precedence over these notes. Do not duplicate a survivor to preserve a new story beat. The voices differ through attention: Mara asks what the record says; Oren asks who expects him to do the task; Nessa asks who is currently covering it; Sella asks whether the meal count can be trusted.

### Scene 1: Two sheets

**Setting:** the Sleeve Board, during a shelter visit.

Mara has removed one sheet from its sleeve and set it beside a newer copy. A small brass clip holds the corners flat. The player can inspect either sheet, ask why there are two copies, ask what “covered” means, or leave.

Mara: “This one stayed here. That one went on the route board.”
Player: “They look the same.”
Mara: “That is what I am asking you to check.”
Player: “You do not know?”
Mara: “I know they were meant to match. I do not know when the mark changed.”

The line preserves Mara’s limits. She is not pretending to forget information the player needs. If the player inspects both copies, an interaction can report that the names and task line match, while the lower margin of the route copy is too worn to read. If the player has a relevant reading or recordkeeping skill under an existing system, a line can reveal the correction mark’s shape, but the player still needs another source to interpret it.

If the player immediately says someone falsified the roster:

Mara: “That is a reason to check it. It is not what the paper says.”
The player may apologize, ask who benefits from the entry, or refuse to speculate. This exchange should not apply a hidden relationship penalty. It teaches that a concern can be pursued without accusation.

If the player asks to take the original, Mara says it is the only dry copy and offers a sketch or supervised inspection. If inventory ownership supports a removable copy, the player may take a duplicate. If not, keep the interaction at the board. Do not create a unique inventory item only to support a single line of dialogue.

### Scene 2: A locked room

**Setting:** the exterior of the Heat-Service Annex.

A new hasp sits beside an older latch. The dust around the lower hinge is undisturbed, but a fresh scrape crosses the paint near the service panel. Oren stands nearby with a tool wrapped in a shirt sleeve.

Oren: “You are here about the names.”
Player: “Did you work this shift?”
Oren: “I worked this room. That is not the same answer.”
Player: “When?”
Oren: “Before the roster stopped being useful.”

The player may ask what changed, whether the room is safe to open, or if Oren can show where he worked. Oren can explain that the valve was once checked from inside, but a panel repair may have moved the check outside. He does not know whether the roster was updated after that change. If the player has a relevant repair capability, a technical response notices that the scrape is newer than the hasp. It does not identify the person who made it.

If the player attempts to force the door, use the current interaction and danger model. The scene should not invent a bespoke lockpicking minigame. A forced-entry route might damage the latch, trigger a location consequence, or simply be unavailable; whichever applies must use existing supported mechanics. The player should still have a clue path through the panel or a conversation.

### Scene 3: Covered

**Setting:** near the Repair Bench.

Nessa returns a pair of gloves to the shelf. The cuff of one is patched with a different thread.

Player: “The board says the shift was covered.”
Nessa: “It used to.”
Player: “By whom?”
Nessa: “That is why I said it used to.”

If the player asks what “covered” means, Nessa explains that a person could trade into the shift without changing the posted name until the next board revision. Some nights the crew worked; some nights another person checked the service from outside. The roster was a plan, not an attendance ledger. This is useful context, but it does not prove what happened on the disputed nights.

If the player says the shift was empty, Nessa asks, “Did you look at the panel?” That question pushes the player back toward evidence rather than shaming them. A skill-gated response can identify wear patterns only if the existing game supports that capability. The general route remains open.

### Scene 4: The meal count

**Setting:** the Cook Room Passage during a quiet period.

Sella has crossed out a number and written it again. The meal list uses the same shift label as the work roster, but it does not list individual names. The player can ask whether an unworked shift received extra food, whether the count was adjusted, or if Sella can remember who took the portions.

Sella: “A number on a board is a number on a board.”
Player: “That is not an answer.”
Sella: “It is the part I can prove.”
Player: “What can you remember?”
Sella: “Who waited. Not who the food was meant for.”

This scene avoids turning a small allocation discrepancy into a theft accusation. Sella can remember that portions were held for an absent worker or that the count was split among people on duty, but she cannot necessarily identify who made the decision. The player can ask whether to preserve a reserve, remove the duplicate count, or wait for a direct roster correction. If current food mechanics can reflect the choice, route it to that owner. Otherwise it remains a dialogue and quest outcome.

### Scene 5: The margin note

**Setting:** the Dry Shelf Room or a supported document interaction within an existing shelter.

The note has a folded corner pressed flat by a spoon. Its ink runs through one line but leaves “covered after bell” legible. A second hand has written “not the same as present.” The player can compare the handwriting with the roster, preserve the note, or leave it where it was found.

The note’s author is not identified. It may have been written by Mara, Nessa, or an unknown clerk. The dialogue must not assign it to a character just because the writing resembles a current hand. If a handwriting clue is available, it says “consistent with” or “not consistent with” only when the game has a supported inspection interaction. The player can conclude that the roster tracked expected coverage rather than actual attendance, but still not know who covered which night.

### Scene 6: The report

The group meets at the board. The player lays out what is known: copies match; the correction mark may mean a covered shift; the Annex shows recent tool activity; the exact worker is not confirmed. The dialogue options state the evidence level and decision:

- Correct the names to show coverage remains unknown.
- Preserve the roster and add a note distinguishing planned work from confirmed attendance.
- Request another account before changing the board.
- Close the inquiry as unresolved and leave the current sheet in place.

Mara: “If we change the name, say why.”
Nessa: “If we keep the name, say what it means.”
Oren: “If you write my name there, ask me first.”
Sella: “And tell me whether I am counting people or places.”

These lines show each character’s practical concern. The player’s choice can update the board only through an existing location or quest outcome. If a current world owner supports work scheduling, it may also affect who is assigned. That deeper effect is optional and requires an approved integration route.

### Scene 7: Return after the next shift

If the player returns after an authored milestone, the board displays the settled wording. Mara can say the new copy is legible; Oren can correct an outdated task label; Nessa can report who accepted the next assignment; Sella can confirm whether the meal count used the new category. Each line requires a real owner or an authored scene outcome. The game must not present all four reactions if only one of those systems changed.

If the player delayed the decision, the scene can remain open but should not replay all prior dialogue. A compact hub response asks whether the player has another source. If the main story has already ended elsewhere, the board can show the last known status and close as unresolved rather than trap the player in an unfinished conversation.

### Optional character beats

Mara’s personal beat is not a confession. She once kept a name on a roster after the person stopped working a task because a meal allocation depended on the line. She thought the board would be corrected the next morning. It stayed for weeks. She can acknowledge the practical reason without claiming it was harmless. The player can respond by asking who received the food, telling her the record matters, or saying the current question is the next shift. Her arc is about being willing to separate a useful temporary workaround from a trustworthy record.

Oren’s beat concerns work he can still do but no longer wants assigned by default. He is not defined by an injury or refusal. If asked directly, he can volunteer for one safe inspection after a handoff is agreed. If the player assumes he is unavailable, he corrects them. If the player pressures him to resume the old role, the scene can set a boundary. Any relationship effect must be supported by the current relationship owner and based on the actual choice.

Nessa’s beat concerns replacing undocumented coverage. She knows that informal work keeps the shelter functioning, yet she has seen uncredited effort disappear from records. She can disagree with both Mara’s caution and the player’s urge to fix the board quickly. Her position does not become a faction ideology. It is a local work concern.

Sella’s beat is a small practice: she keeps a spare spoon beside the tally so a short meal count can be checked by touch when the light fails. If the player asks, she explains that it helps with counting, not with knowing who was absent. The prop is environmental detail, not a secret clue.

### Dialogue graph scope

The first slice needs a linear scene for task acceptance, a hub with short reconverging branches, knowledge checks for the margin mark, one supported relationship variant, location-specific interactions, and a closure callback. It does not need hidden emotion simulation or a universal dialogue-memory system. Each node should declare its actual supported conditions and effects. The graph should remain readable when optional scenes are omitted.

The dialogue packet should maintain separate text for immediate speech, journal summary, map hint, and notice. Reusing one paragraph in all four surfaces will create poor readability and may disclose uncertain information at the wrong moment. The journal can retain evidence nuance; map labels need a few words; characters use their own vocabulary.

### Prose limits and implementation notes

The physical descriptions should carry atmosphere without forcing a unique asset for every clue. A patched cuff, rubbed latch, dry paper sleeve, and spoon used as a paperweight can be represented through existing prop sets or plain interaction text. An essential conclusion must not depend on an expensive close-up. Environmental art should support the scene, while the current interaction and dialogue owners deliver its gameplay meaning.

All names and prose remain provisional. Canon review may replace the cast, location terminology, or story cause. The underlying design—distinguishing planned work from observed work and preserving uncertainty during correction—should survive that revision. If the story collides with existing shelter-management content, fold it into the current characters and locations rather than shipping a duplicate board system.


### Additional scene packet: The Quiet Count

**Purpose:** let the player see that resource plans can be reasonable and still fail to describe actual attendance. The scene is a short optional chain that can close without changing food state.

**First exchange — the tally.** Sella has written the expected shift count on the meal slate. A line is erased and written again. The player can ask whether the number changed, whether it came from the roster, or whether the food is already portioned.

Sella: “The count changed.”
Player: “Because the shift changed?”
Sella: “Because the count did.”
Player: “Did anyone check?”
Sella: “That is what I am asking you.”

Sella does not say that food is missing. The player can ask her what happens if the number is wrong. She explains that the kitchen holds a small reserve when it can, but that the reserve is not guaranteed. If the current resource system has no reserve concept, this line should be revised to a purely local planning concern.

**Second exchange — the source.** If the player has inspected the roster, they can show the copied shift line. Sella recognizes the label but cannot confirm attendance. If the player has only heard about it, she says “I did not read the sheet.” This distinction lets the dialogue represent the player’s context without making the cook an omniscient source.

Sella: “This tells you who we expected.”
Player: “Not who came?”
Sella: “The bowl tells me who came to the line.”
Player: “And if somebody took a portion elsewhere?”
Sella: “Then the bowl does not tell me that either.”

**Choice — what to count.** The player can recommend using the planned count, hold the current reserve, or mark attendance unknown until a worker responds. The scene should show the practical tradeoff. Holding food may reduce immediate distribution if the current resource owner supports that; using the plan may serve expected workers; an unknown mark can prompt another check. None is universally correct. The player is not asked to accuse anyone of taking food.

**Callback.** A later visit can report that the next count was signed by the person who prepared it. If no food-state change occurred, do not display a resource delta. Sella can say, “This one has a name beside it.” That is enough to show a procedural improvement without inventing a new ledger.

### Additional scene packet: The Tool That Came Back Clean

**Purpose:** distinguish possession, use, and testimony through one small investigation.

**Opening.** Nessa finds a valve key on the bench with a clean grip and a fresh scratch at its base. The tool return sheet says it came back after the night shift. Oren says he has not used it since the repair. The player can inspect it, ask who logged it, or let the detail go.

Nessa: “It was signed out.”
Player: “By Oren?”
Nessa: “That is what the line says.”
Player: “Did he take it?”
Nessa: “I watched him sign the line. I did not watch him leave.”

This scene gives Nessa precise knowledge and a clear limit. It must not use the paper record to declare who carried the tool. If the player has a supported repair skill, the scratch can be described as a mark from a panel edge, not as proof of a specific repair. Without that skill, the interaction can report a fresh scratch and leave its source unknown.

**Branch.** The player can return the key to the tool board, keep it for a route inspection if current inventory supports the action, or ask Oren to demonstrate what he remembers. If the player keeps it, the tool is not a quest trophy; it is returned or consumed through existing inventory policy. If a unique item system would be required, remove the inventory branch and resolve the scene through inspection alone.

**Resolution.** The evidence supports that the key was signed out and later returned, but not which person used it. The journal says exactly that. A later scene can show the tool’s next checkout recorded clearly. The quest’s design purpose is the player’s distinction between a record and an observed action.

### Additional scene packet: The Walk Before Dark

**Departure.** A temporary worker named Rill is due to cross the West Intake Walk with a repair bundle. Their concern is not fear of the route in general; the evening wind has picked up, and they do not want the tools to freeze before arrival.

Rill: “I can carry them.”
Player: “The walk is exposed.”
Rill: “I know where it turns.”
Player response can offer escort, ask about a covered route, recommend waiting, or ask who is expecting the bundle.

If escorted, the player chooses whether to carry the bundle or let Rill keep it. This is a small agency point: Rill has knowledge and preference. A hazard check uses only supported movement or expedition mechanics. Do not invent a bespoke stamina challenge. If the player cannot escort, a companion or shelter worker may make the handoff if the current roster/character systems support that route.

At the destination, the receiver checks the fastener count and signs the note. If the task was delayed, the receiver may have used a substitute repair or postponed the work. The player receives a truthful result: the bundle arrived, was handed to another worker, or remains at the shelter. The scene does not claim that the Annex was repaired unless a real owner applied that change.

### Additional scene packet: The Names We Keep

This hidden route is built on restraint. The player finds an indentation on an old sheet where a name was erased. The mark is too faint to identify with confidence. A character can say they remember someone taking food to a neighbor during a period when the neighbor could not work. They do not identify the erased name. The player can leave the clue, ask where the person went, or record that the roster once had another entry.

The content should not attach the secret to an existing person until continuity review establishes permission. Avoid a reveal that turns an old kindness into a scheme or suggests that workers manipulated allocations for selfish gain. The material fact can remain small: the schedule was used to hold a place for someone who needed help. The exact arrangement remains uncertain.

A hidden emotional response should not be represented as an opaque approval flag. If the player asks gently, the speaker may share the limited account. If the player presses for a name, the speaker can refuse. The main quest continues. The player is not rewarded with the erased identity for choosing the correct tone.

### Scene text and branch review table

| Scene | Required context | Optional context | Choice/effect boundary |
|---|---|---|---|
| Two Sheets | Board accessible | Reading skill; prior discovery | Record copies compared |
| Locked Room | Annex or fallback source | Repair skill; Oren present | Observe condition, not assign guilt |
| Covered | Work-board topic available | Relationship with Nessa | Learn terminology, not attendance |
| Meal Count | Kitchen scene available | Resource-system state if verified | Recommendation, not automatic ration edit |
| Margin Note | Optional shelf interaction | Prior clue | Interpret limited source |
| Report | Main evidence sufficient or unresolved route | Additional witness | Select report outcome |
| Next Shift | Later milestone and outcome | Character still present | Acknowledge only actual change |

### Barks and ambient lines

Ambient lines help a shelter feel occupied, but they must not become a hidden quest ledger. Use a small bank that reflects current activity: a worker asking where the gloves went, someone asking whether the board has been moved, or a cook checking who is on the late meal count. These lines can be selected from authored content if current dialogue and event systems support them. Their conditions should be broad and inexpensive. They do not change quest state unless the player opens a deliberate interaction.

A bark should not reveal an essential clue that the player can miss because they walked past. If it contains a useful fact, repeat it in a reliable dialogue or journal path. Avoid random barks that contradict a later scene because they were authored before the branch outcome. The current state owner should select any outcome-specific line.

### Dialogue quality gates

The content review asks whether each scene: gives the player an actionable question; lets the speaker state their knowledge limits; avoids treating absence as guilt; offers a refusal or uncertainty path; does not require optional art; closes safely if a speaker is absent; and uses a supported effect owner. The prose review asks whether different people attend to different details, whether everyone sounds too articulate, whether a line repeats the same theme, and whether the physical action can carry emotion without explanation.

A line that says “the record is not reality” is too broad and on the nose. A person asking whether the count came from the roster or the kitchen line teaches the same distinction through a practical question. This is the tonal standard for the rest of the package.


### Extended dialogue packet: the board correction meeting

The meeting scene occurs after the player has gathered at least one source and may have additional evidence. It can be staged at the Sleeve Board or as a conversation in an existing shelter hub. It should not require a new meeting-room location. The scene is short enough to fit between survival tasks but gives each participant a distinct practical concern.

**Opening.** Mara places the old copy to the left and the current copy to the right. Nessa stands near the board rather than the table. Oren stays by the exit until the player asks him to sit. Sella is present only if the player pursued the meal count. The staging should remain readable if the game does not support separate character positions: dialogue labels can carry speaker identity.

Mara: “Start with what you saw.”
The player can list the matching names, the ambiguous mark, the panel condition, or say that they have no independent account.
Mara: “And what did you not see?”
This follow-up lets the player state a limit, not only a conclusion.

If the player has a corroborating record, Mara asks whether its source is separate from the first copy. If not, the player can say they do not know. The graph should not infer independence from two different paper objects if one is a copy of the other.

**Competing practical views.** Nessa asks whether the next assignment needs an immediate name. Oren asks whether the player is proposing to put his name back on a job. Sella asks whether the meal count should continue to use the planned shift size. Each question exposes a different downstream consequence. The player chooses which concern to address first. The scene can present up to three hubs and reconverge before the final report.

Nessa: “If the line stays, somebody still has to go.”
Oren: “If you write me in, somebody will decide I agreed.”
Sella: “If you remove the count, somebody waits for food that is already cold.”

The lines are not arguments over morality. They are costs. No character claims that their view is the only correct one.

**Report choice.** The player chooses correction, annotation, further inquiry, or unresolved closure. Before confirming, show a one-sentence consequence preview in ordinary language:
- “This changes the names only if your evidence identifies current coverage.”
- “This leaves the names in place and marks the entry as a plan.”
- “This keeps the current sheet while you seek another account.”
- “This records the mismatch and closes the inquiry without naming a worker.”

If the existing UI cannot provide previews, place the wording in the response itself. The choice label should not hide the action.

**After choice.** Mara either applies the supported edit or says she can prepare the next copy. Nessa confirms the next task is still uncovered if that is known. Oren confirms or refuses a proposed role. Sella acknowledges whether the count was changed. The scene uses only the subset of lines corresponding to actual effects. If no work, food, or board owner exists, characters can acknowledge the narrative recommendation without claiming a system update.

### Extended dialogue packet: the handoff at the next shift

This callback should be small and causally linked. The player arrives as a new worker is checking the board with one finger on the current line. They look at the player, then at Mara.

Worker: “Does ‘covered’ mean someone is coming?”
Mara: “It means someone accepted it.”
Worker: “So someone is coming.”
Mara: “It means someone accepted it.”
The worker waits.
Mara: “I will check who.”

If the player chose to annotate, the scene shows that the difference matters: the next person asks a better question rather than assuming attendance. If the player chose to correct names, the scene uses the updated roster only if verified and applied. If the player left the record unresolved, Mara can say the line remains marked as unconfirmed. The callback should not claim the work is complete.

The player can ask whether anyone signed in. If the current game has no sign-in system, the response remains an authored report from the character. If no one can verify, the scene closes with the uncertainty intact. An unresolved callback is not a narrative failure; it confirms that better wording did not magically create evidence.

### Character voice micro-guides

**Mara:** Uses categories and paper-handling verbs. She asks what a mark means, who copied a sheet, and when it changed. She avoids saying “truth” in broad terms. When stressed, she repeats a practical instruction: “Keep this copy flat.”

**Oren:** Uses task language and boundaries. He asks whether someone expects him to do the work, which tool is needed, and who will carry the handoff. He may leave sentences unfinished when the answer is obvious to him. He is neither a bitter recluse nor a secret hero.

**Nessa:** Uses current staffing and movement. She asks who is in the room now, who can cover the task, and whether the route is safe. She can disagree with a record while valuing the people who made it.

**Sella:** Uses counts, bowls, and preparation. She knows how the kitchen uses an estimate but not who worked elsewhere. She resists being turned into an investigator when her role is to make meals arrive.

**Temporary worker:** Uses immediate practical details—cold tools, distance, time, a safe path—and has agency over their own work. They are not an exposition source for the whole story.

These are voice constraints, not fixed line quotas. Existing characters must retain their own canon. If a reused survivor already has an established verbal style, this guide adapts around it.

### Prose reuse and localization notes

Every scene can be adapted into dialogue, journal, and map surfaces, but the text should not be copied mechanically. The spoken lines are short and situational. A journal entry can say, “Mara explained that the mark may mean coverage, but no available source confirms who worked.” A map label can be “Annex panel.” A notice can say, “Attendance not confirmed.” Each format preserves the fact boundary at the length it needs.

Repeated vocabulary is a risk: “covered,” “worked,” “planned,” and “confirmed” must be stable across speakers and UI. Provide translators with glosses and context. Do not use a pun or repeated metaphor as the only distinction between states. A player using subtitles or screen reader should receive all essential information. If a paper image contains shorthand, an inspect text equivalent is required.

### Optional barks after resolution

After resolution, a few ambient lines can reflect the current board practice:
- “I checked the copy before I took the shift.”
- “The old sheet is in the dry sleeve.”
- “Ask who covered it. The name stayed from last week.”
- “No sign-off yet. Leave the line open.”

These should be selected only when the current state warrants them. They provide atmosphere and practical world texture. They cannot introduce a new required clue or contradict the specific result. If current ambient dialogue cannot read quest outcomes, omit the barks rather than adding a parallel state cache.

### Scene coverage and production estimate

The full package contains one main opening, three evidence scenes, one report meeting, one callback, three optional quest scenes, and a small number of short ambient lines. A minimal slice can ship with the opening, board inspection, one witness exchange, the report meeting, and one result acknowledgement. The rest can be staged in later content drops. This staged approach preserves narrative completeness while keeping the first implementation tractable.

Production review counts unique text nodes and contextual variants, not only prose words. The total cost includes recording or subtitle synchronization if applicable, localization, speaker presentation, location backgrounds, map/journal copy, and QA across branch conditions. A side scene can be cut without breaking the main graph if its fallback and optional status were designed from the start.


### Prose variants for report outcomes

**Verified correction.** Mara reads the final line once. She checks the name against a second copy, then moves the older sheet beneath the board rather than tearing it up. “This one is current,” she says. “The old one still explains why the count drifted.” Oren asks whether the task now belongs to him. The player must confirm only if the assignment was actually made through the work owner.

**Annotation.** Mara writes “planned coverage” in the margin and leaves the names. “It will make the next copy longer,” she says. Nessa answers, “It will make the next person ask.” The scene shows a changed label only if the board owner persists it. The two characters agree on the wording without agreeing that attendance is known.

**Request another account.** The player asks to wait. Mara places a small paper weight on the sleeve so it will not curl. “Then this stays open.” If the quest owner cannot persist a pending result, the line should instead say that the player can return with another account, and the current stage remains the established active state.

**Unresolved closure.** The player states that the evidence is not enough. Oren leaves his name off the new copy. Sella takes the old count back to the kitchen. Nobody says the decision solved the staffing question. The journal closes with what was learned and what remains unknown.

These short outcome treatments can reuse scene staging but must not collapse into the same text with only the option label changed. Each reflects a real difference in what the player authorized. They are not promises of resource changes.

### Side conversation: the worker who changed the task

A new worker can ask why the roster lists valve checks when the maintenance panel is inspected from outside. The player may explain what Oren said, show the margin note, or admit they do not know. This scene provides a natural way to repeat information without a lore dump.

Worker: “Do I go inside?”
Player response: “The panel may be checked outside.”
Worker: “May be?”
Player: “That is what the note supports.”
Worker: “Then I will ask before I open anything.”

If the player previously found no evidence, the response options do not appear. A general response asks the worker to wait for confirmation. The scene models uncertainty as actionable: the worker can pause a risky assumption. It does not give the player command over the worker unless current assignment mechanics support it.

### Side dialogue: the cook’s closing ritual

At the end of a meal, Sella places the tally spoon across the slate so the line cannot be altered before the next cook sees it. The player can ask why. Sella says that the spoon does not make the count correct; it shows that someone intends to check it. This is a quiet callback to the story’s distinction between plan and confirmation.

If the player pursued the count quest, the line varies by outcome. If they did not, Sella only asks them to move the spoon before taking the slate. The ambient detail cannot act as a quest clue in one branch and unimportant dressing in another without explicit interaction proof.

### Branching dialogue versus prose-only variation

Not every variation needs a graph branch. Weather can change the paper’s appearance in the location description. A speaker’s current workload can shorten a greeting. A major report choice changes who acts and therefore needs a state-bearing branch. Relationship tone may require a supported condition but can reconverge. The author marks each difference as presentation-only, local, quest, relationship, faction, world, or ending scope.

Writers should avoid adding a new node for every adjective or greeting. Use prose variants when the meaning is stable and no state changes. Use a branch when the player’s response or world condition changes what can happen. Keep that distinction visible in the content packet so QA can focus on meaningful combinations.

### Final dialogue readthrough

A final readthrough follows three voices: a player with no optional context, a player who has found all evidence, and a player who made a report that later proves incomplete. All should understand the immediate action. The first should not be locked out; the second should feel rewarded by precision; the third should receive a chance to correct without erasing the past.

Reviewers should underline each sentence that states a fact and identify its source. If no source exists, revise it as opinion, uncertainty, or remove it. This sentence-level test is especially useful for a story built from copied records.


### Silence, interruption, and readable staging

Silence can be shown as a short pause or a character continuing the task while the player decides. It should not be encoded as a missing response node. If the current dialogue UI supports only text, use an authored beat: Mara smooths the paper; Oren returns the tool to the bench; Sella turns the slate around. Each beat reinforces what the character attends to.

Interruptions should carry a practical cause. A meal line forms, the worker needs to leave, or a tool slips from the bench. Do not use a random interruption to cut off a required explanation. If the scene is resumed, the player should retain available choices or receive a concise recap from a character who was present.

Readable staging means names and line focus remain clear for keyboard, controller, subtitle, and screen reader users. Essential evidence is spoken or exposed through the inspect interaction; animation and ambient sound add texture but do not carry the only clue.

### Branch re-entry after an interrupted conversation

If the player leaves the correction meeting before selecting an outcome, reopening it should summarize only the facts required to continue. It should not replay every witness account or reset a one-time choice. The graph can return to a compact hub with “review what you know,” “ask about the record,” and “choose how to report.” If new evidence arrived while the scene was closed, place it in the evidence summary and let the player decide whether to revisit earlier interpretations.

If the quest was resolved through another route, the meeting becomes a closure conversation or disappears. Do not present stale report options. If an earlier choice was applied but the dialogue was interrupted before acknowledgement, the next entry shows the result, not the choice again. This protects both narrative continuity and effect idempotency.

### Cuttable prose and required facts

Mark each scene paragraph as essential, optional context, or atmosphere. Essential facts need a supported interaction and fallback. Optional context can be removed if the speaker or location is absent. Atmosphere can vary or be cut without changing the quest. This label helps production trim scenes while preserving comprehension and branch fairness.

Do not bury the only explanation of “covered” in a long optional monologue. The player can learn the term from a short response or the margin note. Character history can deepen the choice, but it cannot be the only way to understand its immediate effect.

### Prose closeout

The strongest lines in this thread name a practical limit: the sheet says who was expected; the bowl says who came to the line; the panel shows a recent mark but not who made it. Keep that specificity through revision. Let characters disagree about action while staying honest about evidence.

## Pass 15 — Mid-winter dialogue scenes with authored evidence boundaries

**Status: DRAFT/PROPOSAL.** This pass uses the master bible's Days 90–180 pacing lane to supply a concrete dialogue authoring slice. The existing encounter, quest, narrative consequence, and character catalogs are the first places to inspect. No general-purpose dialogue graph engine, new character, new faction, or catalog ID is authorized here.

### 15.1 Collision reconciliation

The prior Empty Shift dialogue packet may repeat existing duty-roster characters, scenes, and quest beats. ASHFALL has active DutyRosterSystem and DutyRosterQuestRuntime owners and authored duty-roster catalogs. Keep prior dialogue only after checking cast IDs, stage IDs, choice effects, and roster facts against those live records. If it belongs to that storyline, integrate it as a scene or alternate line under that owner; do not register an independent “shift story.” The line samples below are generic DRAFT demonstrations without canon character names.

### 15.2 Scene purpose and cast discipline

Working sequence title: The Long Thaw. The sequence is not a complete production quest. It demonstrates how dialogue can carry evidence, pressure, and character intent across quest, expedition, and resolution:

- The **intake registrar** wants a record that will survive scrutiny and is afraid of making a promise on incomplete measurements.
- The **maintenance worker** cares about a repair that can be done today; they are not a shorthand “engineer voice.”
- The **expedition lead** wants a route that returns the crew with the same number of people who left.
- The **community representative** wants an answer before the next cold period and does not accept “we will see” as a plan.
- The **radio operator** treats signal quality as evidence and dislikes being asked to certify a source they could not identify.
- The **returning witness** can describe one observation and distinguish it from rumor.

These are role placeholders. Bind them to existing characters only after checking their IDs, survival state, schedule, faction knowledge, voice signature, and current arc. Do not add a cast merely to give every dialogue node a face.

### 15.3 Reusable scene sequence

| Scene | Location/context | Dramatic action | Choice form | Reuse |
|---|---|---|---|---|
| Intake, first report | Existing shelter entry or briefing context, if the current route supports it. | Two reports conflict about the timing of a service failure. | Ask for a source, accept the operational summary, or defer judgment. | Revisit line reacts to whether evidence was logged. |
| Workshop, practical read | Existing work area already represented by the game. | A damaged part is shown beside a substitute. | Spend the substitute, preserve it, or ask for an external source. | The line changes after a real crafting outcome, not a guessed result. |
| Map, route commitment | Existing expedition planning screen/location. | Crew sees the required target and possible alternate clue. | Take the short exposed route, the longer known route, or postpone. | Dialogue reflects actual route selection and travel result. |
| Return, witness statement | Existing debrief or journal channel. | One observation can narrow the cause but cannot prove intent. | Record exact words, add an interpretation with attribution, or leave uncertainty open. | The selected report style changes later exposition. |
| Community response | Existing faction/settlement contact context. | The representative asks what can be promised before the deadline. | Offer confirmed help, limited help, or an honest refusal. | Faction and quest outcomes come from their owners. |
| Quiet follow-up | An optional later scene on return. | A character explains what the player’s wording changed for them. | Listen, correct the record, or move to the next task. | Relationship consequence only if current relationship owner supports it. |
| Callback | A later valid event window. | The same early choice returns in a changed form. | Acknowledge, dispute, repair, or decline the conversation. | Uses a confirmed deferred-event seam; otherwise stays an optional authored follow-up. |
| Chronicle close | Endgame readout only if the current Chronicle can consume the fact. | The record is described as precise, incomplete, or disputed. | No new ending choice is required by this scene. | Chronicle line is conditional on a verified outcome key. |

### 15.4 Sample lines for voice and branch testing

All lines are DRAFT and should be checked against existing prose before use.

- Registrar, first report: “I have two times for the same outage. I can enter both. I cannot make them agree.”
- Maintenance worker: “The spare will hold. The report says how long. It does not say what we stop making to use it.”
- Expedition lead: “Mark the return route before we argue about the forward one.”
- Community representative: “I can tell them you came. I need a verb for what happens next.”
- Radio operator: “The voice says the crossing is open. The carrier says nothing about the bridge.”
- Returning witness: “I saw the lamp go out. I did not see who touched the switch.”
- Registrar, if the player attributes blame without evidence: “I can file that as your conclusion. I cannot file it as a measurement.”
- Registrar, if uncertainty is preserved: “This copy leaves the cause blank. The date and the missing load are still here.”
- Community representative after a refusal: “You said no while I could still change the route. That is a hard answer. It is an answer.”
- Maintenance worker after a costly substitute: “It fits the housing. Keep the old part. Someone will ask why the store shelf is empty.”
- Radio operator after a failed source check: “The signal held long enough to waste our fuel. That is not the same as a lie.”
- Returning witness after an alternate route: “We did not see the mark. We saw the water line where it used to be.”

The samples deliberately avoid stating that a person caused the failure, that a faction lied, or that a repair succeeded. The runtime owner must establish those facts before copy may report them.

### 15.5 Choice structure

Prefer a short branch that reconverges on the next concrete action. A choice is authored only if it has a different player meaning, accessible wording, and truthful outcome. Cosmetic tone, local scene action, quest progress, relationship change, faction standing, world access, and ending consequence must be distinguishable. Several response options may lead to the same objective while changing the player’s chosen phrasing; that is acceptable if the UI makes the difference clear and the line does not promise a hidden mechanical reward.

Node copy should separate: current fact; speaker interpretation; information the player lacks; immediate request; response labels; and resulting owner command. A witness should never speak knowledge from a future event. A faction representative should not know a private shelter decision unless a current information channel exposed it. Skill-gated text may improve observation, but should not cause an unskilled player to lose a required clue.

### 15.6 Branch and reconvergence map

- **Evidence branch:** source requested, summary accepted, or investigation deferred.
- **Operational branch:** substitute committed, reserved, or sought through an external route.
- **Travel branch:** risky route, safer route, or delay.
- **Interpretation branch:** exact observation, attributed inference, or unresolved record.
- **Social branch:** confirmed promise, limited promise, or refusal.
- **Callback branch:** acknowledges, disputes, repairs, or declines.

The evidence, travel, and social choices must have distinct canonical facts only where the current owners can store/apply them. Reconverge at the next available objective without erasing those consequences. If two branches are purely verbal, do not create a new flag solely to preserve flavor unless the authored callback genuinely depends on it.

### 15.7 Reuse and production cost

The same scene structure can support a radio report, a shelter debrief, or a faction meeting by changing the verified context, speaker, and evidence source. Reuse the structure, not identical dialogue. Repeated visits need an authored change caused by a real owner fact: prior decision, objective status, location state, failed quest, relationship band, or new evidence. Do not randomize an NPC's memory.

Classify each scene before production: data-only when existing fields express it; data plus minor wiring only when a current adapter supports the needed interaction; Core extension when a missing command/effect is proven; cross-system only after naming the owners and save implications. A new dialogue graph abstraction is FOUNDATIONAL and requires an explicit architecture decision; prose examples do not justify it.

### 15.8 Prose quality and accessible interaction

Maintain the house voice described in the world bible: concrete records, restrained emotion, bureaucratic pressure, and distinct speaker needs. Give each choice a short verb-led label; use full text for nuance. Tooltips or dialogue details should explain locked responses by broad reason (“you have no confirmed route evidence”) without exposing spoilers. Never make the only readable distinction a color change. Keyboard and controller navigation must reach every visible response and return focus after the scene closes. Screen-reader names should retain the speaker, the response, and any irreversible commitment warning.

### 15.9 Acceptance gate

A scene packet is ready for a data trial only after: the scene is attached to a verified existing source catalog; all cast and location bindings resolve; each line is legal for the speaker's knowledge and voice; each choice has a defined consequence class; mechanical consequences route to existing owners; branches cannot strand mandatory progression; callback delivery is confirmed or clearly marked absent; localization keys and accessible labels are planned; and every DRAFT line has been compared against current narrative content for duplication. This pass adds no production prose record and runs no tests.

### 15.10 Side-conversation packet: “The register stays open”

**Context:** Optional after the player has seen the conflicting reports, before they choose whether to present an interpretation. The scene uses a verified current room and two existing cast members only after the premise audit binds them. Until then, “registrar” and “worker” remain role labels.  
**Purpose:** Let the player choose between preserving an imperfect record and polishing it into a confident story.

> Registrar: “The clean copy has one answer. The carbon has two.”
>
> Worker: “The carbon also has a thumbprint over the lower time.”
>
> Registrar: “That is a mark. It is not a clock.”
>
> Responses:
> - “Keep both readings.” — Cosmetic/local wording unless a current record owner supports a durable evidence choice.
> - “Use the clean copy.” — Local report selection; it must not falsify source data.
> - “Hold the report until we can compare the gauge.” — Quest delay only if the quest owner supports a waiting objective.
>
> Worker, if the player preserves both readings: “Then nobody gets to call the missing hour a mistake yet.”
>
> Registrar, if the clean copy is selected: “I can issue it. I will keep the carbon with the maintenance file.”
>
> Worker, if the report is held: “Good. I can show you which screw was already warm.”

The scene branches for a few lines, then reconverges on the player's next action. It does not conclude which reading is correct.

### 15.11 Side-conversation packet: “A verb for the notice”

**Context:** A community representative is waiting for an answer about a repair commitment. The player can offer confirmed support, limited support, or refusal. The exact resources and faction consequences come from canonical owners.  
**Purpose:** Make refusal a valid outcome and keep the requester's agency visible.

> Representative: “The notice has a space for what you will do.”
>
> Player responses:
> - “We will send the crew.” — Show only if the expedition owner confirms the party and route can be committed.
> - “We can send tools, not people.” — Show only if the item owner and destination can receive them.
> - “We cannot promise help.” — Always legible if the request can be refused.
>
> Representative, on a confirmed promise: “I will tell them the crew is coming when you put the names on the manifest.”
>
> Representative, on limited aid: “Tools do not keep a hand warm. They still give us something to try.”
>
> Representative, on refusal: “I would rather write no than send people out to wait for a maybe.”

The lines do not award standing directly. The response is sent to the quest/faction/expedition owner; the later scene reflects the actual accepted result.

### 15.12 Location card and arrival copy

**Map card — required destination:** “A field report asks for a second reading. The route is open; the cause is not known.” Use only when the active quest and map owners verify both claims.

**Map card — optional clue:** “The old mark is missing from the north side of the conduit. No one has checked the retaining wall.” Use only when the scene has established the conduit and the visible direction.

**Map card — secret clue not yet solved:** “The carrier narrows when the party faces the drainage line. It may be a reflection from the cut.” Do not show the destination identity before the discovery gate passes.

**Arrival prose, DRAFT:** “The room is colder than the tunnel by enough to make the brass gauge honest. A strip of paper has been pinned beneath its glass. The numbers do not match the copy in the shelter.”

**Return prose, DRAFT:** “The mark did not survive the rain. Its absence did. On the wall below it, somebody measured the old bracket twice and wrote down two different heights.”

All destination names and facts remain placeholders until location history, weather, route, and encounter catalogs are cross-checked. Arrival prose describes only what the player can observe.

### 15.13 Reusable dialogue pattern library

1. **Linear scene:** one necessary instruction and one exit. Use for briefings and clear safety information.
2. **Hub-and-spoke:** a character offers evidence, motive, and practical next step as separate topics. Each spoke returns to the hub; topics already exhausted are visibly closed.
3. **Short branch and reconverge:** one answer changes tone or local report, then the next actionable request is shared.
4. **Relationship response:** wording responds to a canonical relationship band, not a hidden private score.
5. **Knowledge response:** one line cites what the player has actually learned, with a second line that explains uncertainty.
6. **Faction response:** access language comes from the current faction owner. A hostile response can still provide an exit or neutral alternative.
7. **Repeated visit:** after the introduction, the NPC references a real new event or asks a different question; no randomized “you again” loop.
8. **Failed quest response:** acknowledges the actual outcome and offers a valid follow-up or closure, not blame unsupported by the branch.
9. **Skill observation:** adds an optional technical detail while preserving the same required objective.
10. **Emotional-state line:** uses a current state only when that state is observable to the speaker and appropriate to disclose.
11. **Quest-starting conversation:** the acceptance command appears as a clear response and updates the objective only after the quest owner accepts it.
12. **Environmental discovery:** a note, mark, damaged object, or sound reveals a clue through an existing discovery path; prose states who could have placed it and when.

Do not force all twelve structures into the first release. Choose the smallest set that gives this content a distinct rhythm.

### 15.14 Audio and non-spoken scene alternatives

A character who is absent can leave a field report, a radio fragment, a signed note, or no substitute at all. A recorded message has an authored date and audience; it cannot react to a decision that occurred after recording. Radio phrasing respects signal uncertainty and location. A note does not function as omniscient narration. If voice acting is later commissioned, each speaker gets line count, reactivity tags, localization and recording cost, silence/bark policy, and subtitle fallback. The game must retain complete meaning with text alone.

### 15.15 Content-density budget

For an optional side conversation, a useful initial budget is: one opening, two or three response choices, one or two response lines per choice, one reconvergence, and one revisited-state variant. Add another branch only when it expresses a real player decision or a different actor's knowledge. Track unique prose words, repeated facts, gated lines, recorded lines, and localization units. A large branch tree that reconverges without changing knowledge, relationship, or world state is a rewrite candidate.

### 15.16 Authoring handoff checklist

Each scene packet contains a one-sentence player purpose; existing speaker/location bindings; entry conditions; exact knowledge ledger; authored node/response list in the current supported format; visible and hidden condition explanations; consequence class and owner; branch diagram; return/repeat text; failure and unavailable-speaker behavior; short/long text limits; accessibility order; localization notes; house-voice review; and content-utilization evidence. Production begins only after the current schema is named and the cross-catalog collision check passes.


## Pass 16 — Three connected scene packets with restrained prose samples

This pass builds dialogue-ready material from the master world bible’s seasonal broadcast, hydrophone mystery, and shelter folklore seeds. The packets are DRAFT samples. They do not add runtime node IDs, assume an unverified character roster, or replace existing authored cipher, hydrophone, or folklore records. A writer and integrator should reconcile each line against the live corpora before inclusion.

### Scene packet: The Winter Count

**Scene purpose.** A shelter radio keeper connects a recurring count to a signal the player has already heard. The player should be able to ask about the pattern, ask who kept the record, or leave and return. The clue is not explained before the player has the relevant evidence.

**Voice notes.** The keeper speaks in practical measurements and corrections. They distrust grand theories but feel responsible for a list that has outlived its author. Avoid a mystical oracle tone. Their personal stake is that a count may represent people omitted during an evacuation, not a treasure.

**First visit, before the decode:** “Three winters, same pause after the seventh number. I wrote it down because the generator stutters there too. Maybe those are both just old machines talking.”

**After the player brings a verified decode:** “That line was a name column. I thought we were counting crates. I am going to read it slowly, so nobody gets turned into a total again.”

**After a failed interpretation:** “We made the answer fit what we wanted. Keep the first recording. It has not changed just because we did.”

Player choices should include asking for the source sheet, sharing the decoded phrase, withholding the answer, and ending the conversation. A withheld answer changes local trust or later dialogue only if that consequence is supported by a current relationship owner. The final scene can reconverge on the quest’s canonical completion route while preserving distinct acknowledgement.

### Scene packet: The Shelf That Answers

**Scene purpose.** A listening specialist reviews an ambiguous hydrophone record. The sequence separates the sound itself from the player’s conclusion and gives uncertainty a human voice.

**Voice notes.** The specialist marks what can be measured, pauses before assigning motive, and is willing to say “unknown.” A second resident may offer a competing but plausible interpretation after the player returns with another record. No speaker should treat every hostile-sounding sound as an enemy.

**At initial review:** “The pulse comes back at the same interval. That tells me it is regular. It does not tell me who made it.”

**After a second observation:** “The shelf carries sound farther when the ice shifts. Our two recordings may not be two callers. They may be one noise and a very good echo.”

**On inconclusive resolution:** “Write ‘unresolved’ where everyone can see it. A blank space invites a better answer than a confident lie.”

The dialogue may reference only records the player actually collected. A captioned transcript accompanies the audio. The branch that concludes “natural,” “mechanical,” or “unknown” must be authored to the evidence and reviewed against the chosen source record. If a branch changes a route or faction opinion, Plan 22 must name the owner and confirmation path.

### Scene packet: Rhyme After the Door

**Scene purpose.** An adult resident remembers a rhyme that children used to repeat near a shelter door. The player investigates what it preserved and what it distorted. The story values the rhyme as culture even if no physical secret is found.

**Voice notes.** The resident is affectionate but not sentimental about the shelter. Their memory can be vivid and still inaccurate. If a cohort callback is eventually approved, the character should sound older through changed priorities and learned detail, not a caricature of childhood.

**First account:** “We sang it when the lights went out. The last line changed every week. Nobody agreed on what the door was hiding.”

**With a physical maintenance clue:** “That mark is real. The monster was ours. The jammed latch was real too.”

**When evidence does not settle the origin:** “Leave both versions in the book. The rhyme kept us together. It never promised to be a report.”

A later cohort-age callback is optional and depends on verified maturation and presentation wiring. Until that is proven, offer a repeat-visit or campaign-milestone scene instead. Avoid assigning age, parentage, or faction history without checking current canon.

### Graph and copy rules

Each packet can use a short branch that reconverges, with optional hub questions and repeat-visit variants. Conditions read canonical quest, discovery, relationship, and location context. Effects route through an approved command or event owner; dialogue text does not directly mutate gameplay. Local acknowledgement can vary more freely than quest completion. Every speaker has a stable source attribution and a defined knowledge boundary.

Location descriptions should be observation-first and never pre-spoil a hidden quest destination. A radio room copy seed: “A pencilled tally climbs the wall beside the receiver. The newest mark is careful; the older ones have been rubbed thin by sleeves.” A coast listening-site copy seed: “The wind flattens the water into dull metal. In the headphones, a small repeated knock refuses to sound far away.” A shelter-door copy seed: “The latch has been mended twice. The children’s rhyme remembers the first break better than the repair.”

### Review packet

Before these samples enter a data catalog: compare phrase overlap with current corpora; confirm speaker IDs and localization conventions; bind each variant to a real condition; check that no sample reveals a locked answer; create caption-equivalent text for all audio-dependent clues; and obtain narrative continuity review. The short samples here illustrate tone and branch intent only; they are not final game strings.

## Pass 17 — Spoken records, memorial scenes, and micro-location copy

This pass rotates the prose register toward oral memory and institutional testimony, following the master world bible's Lane A register rotation. Existing source evidence shows 26 oral-lore pieces across two catalogs, a performance system with named producer contexts, a persisted first-heard ID ledger, a journal acknowledgement on first hearing, and an expedition discovery hook. MemorialSystem also raises a once-per-deceased OnMourned event. The implementation boundary is important: a producer can be registered in the oral-lore map without proof that its context is invoked in live play. The reviewed host hook confirms expedition location discovery; memorial and room producer call sites still require a wiring trace. These are DRAFT scene materials and do not add a new song catalog.

### Reusable conversation: the second copy

**Scene function.** The player brings an archival burial entry to a memorial conversation. The speaker separates what the ledger contains from what the living remember. The conversation can end with a correction, an appended note, or an intentional blank.

**Voice direction.** The clerk uses administrative nouns and short clarifications; the grieving resident uses concrete objects and avoids legal language; the undertaker can explain procedure but should not speak for the deceased. Each voice must be matched to a currently authored character and knowledge state before these lines are placed in data.

Opening line, archive clerk: “The book says when the service happened. It does not say when anyone knew.”

Resident response, if the player has shown the record: “I remember the ring on the stone. I do not remember the date they wrote beside it.”

Undertaker response, if the player asks for a correction: “I can add who told me. I cannot turn a second-hand account into a first-hand one.”

Resolution acknowledgement: “We left the blank where the witness left it. The next reader will know which part is missing.”

These lines model epistemic restraint. The player is not asked to decide whether the bereaved person is honest. Avoid a “solve the mystery” voice that rewards suspicion for its own sake.

### Reusable oral-lore beat: song as a shared action

Do not repeat or lightly rewrite an existing lyric. Use the current catalog title and performance_context through the oral-lore consumer. The scene should show who heard or performed a piece and why that context matters, then let the catalog provide the song record. A memorial variant may acknowledge a piece already heard in the room or during an expedition. It must not claim that a song was performed at a specific funeral unless the live memorial/producer event confirms it.

DRAFT setup: “The tune is already halfway through when you reach the room. No one introduces it. The person keeping time taps the table once when the old line returns.”

DRAFT follow-up after a first-heard event: “You ask where it came from. The singer says, ‘From whoever needed it before me.’”

A second visit can change the response from discovery to recognition: “They do not start the song again. They make room on the bench.”

This is a local social scene, not a morale bonus. Any gameplay effect needs an explicit owner and a separate consequence review.

### Micro-location description and encounter framing

Use cartographic prose to make a local detail legible without making it a new destination. At a cemetery parent location: “The path narrows where the stones lean close. Some names are deep enough to hold rain; one line is only a shallow scratch.” At an archive approach: “A strip of waxed cloth hangs over the cabinet latch. Dust marks where the drawer stopped, not who stopped it.” The description should not say that an active quest item is present until the encounter becomes eligible.

A location encounter can surface an optional physical clue, a depleted one-time interaction, or a repeatable observation. The player should know whether the site changed and whether anything remains to do. A repeat visit can use a brief alternate line rather than replaying the entire first-discovery prose.

### Branch structure and copy volume

The first playable conversation should use a small hub-and-spoke shape: inspect the date, ask how the record was made, ask about the physical memorial, or leave. Evidence-gated branches reconverge on the resolution choice. The physical-inspection branch is available only when the parent destination and micro-location encounter are valid. The song branch is available only when the corresponding first-heard or performance state exists. An unresolved branch has a complete, dignified closing line and a return condition.

For each scene, the writing packet should include: first-visit text, one evidence response, one mistaken-but-recoverable interpretation, one unresolved ending, one return line, speaker knowledge notes, audio caption/transcript, and localization length guidance. A compact packet should fit existing node structures if verified; it should not force creation of a generic dialogue graph engine.

### Character and location personality notes

The undertaker's personality can be defined through habits already supported by the burial records: precise about ceremony and plot, willing to preserve uncomfortable facts, careful about what he did not witness. A grieving resident should retain agency and may decline to discuss an entry. The archive clerk can be procedural without becoming a villain. These are role notes, not finalized character definitions. Reconcile aliases, identities, age, relationships, and voice blocks against the character catalog before reuse.

### Editorial gates

Narrative review should check that the scene respects the source record, does not claim an unverified performance, distinguishes recollection from fact, avoids repeating existing song lyrics, and gives the player a non-accusatory route. Content review should compare sample phrases against existing burial, memorial, oral-lore, and journal prose. Integration review must show the specific node, producer hook, or encounter consumer. Until then these lines are DRAFT samples, not authoritative in-game text.

### Pass 17B — Additional branch cards and voice separation

The first dialogue slice should be expandable through reusable scene packets with distinct jobs. The following drafts add material without introducing new named characters or quoting existing song lyrics. All speaker identities and node conditions require a live character-catalog check.

**Archive clerk, correction offered.**
Player: “The service date is clear. The report date is not.”
Clerk: “Then the index should say that. It is shorter than the truth, but closer.”
Player: “Will the old page change?”
Clerk: “No. The page stays as it was. The note tells the next reader what we learned.”

This branch models a correction that preserves provenance. It should only become available after a second source is verified. The clerk's procedural habit makes the archive feel like an institution with an ethical standard, not a neutral prop.

**Undertaker, physical inspection delayed.**
Player: “The road is closed. I could not reach the plot.”
Undertaker: “You brought the page back. That counts as work.”
Player: “Does the date still need checking?”
Undertaker: “It does. When the road opens, we can look. Until then, write down what the page can prove.”

This line keeps the quest in a blocked-but-actionable state and avoids pretending the player inspected the site. The travel condition must come from current expedition/world state.

**Resident, the source is private.**
Player: “I can add your account beside the record.”
Resident: “You can ask me again. You cannot put my words where everyone reads them.”
Player: “I will leave the page unchanged.”
Resident: “Thank you. Listening was enough for today.”

This choice protects a witness who declines publication. A relationship effect is not implied; the acknowledgement may remain local scene state unless the current owner supports more.

**Oral-lore return beat.**
The room settles after the final note. Someone at the far end says, “That is the verse my mother kept.” Another voice answers, “She kept the tune. You gave it the verse.” They leave the disagreement in the air and pass a cup down the bench.

Use this as optional prose around a song already discovered through its existing producer. The scene describes a shared performance but does not assign its origin as fact. If the game has only first-heard state and no performance state, do not add the line “you sang it together” to a persistent journal entry.

**Micro-location encounter closure.**
“You found the shallow mark beneath the moss. It is too worn to read. The ledger can tell you where the stone belongs; it cannot tell you whose hand cut it.”

The line makes a local discovery valuable without inventing an answer. It can be reused for several encounters only if it remains accurate to each site. Otherwise author a specific variant for each validated parent location.

### Voice and localization controls

Maintain a voice sheet with sentence length, preferred nouns, evasions, repeated patterns, and forbidden knowledge for every recurring speaker. The archive clerk should use filing and attribution language; the undertaker should focus on material details and procedure; the resident should ground emotion in an object or remembered action. Avoid giving all three the same solemn, polished cadence.

Keep each response concise enough for the existing dialogue panel and controller navigation. A branch should have a readable label, visible focus state, and a non-color cue for unavailable requirements. The DRAFT copy must be translated only after string freeze and stable node conditions. Any audio performance must have caption-equivalent text and a transcript route so sound is not a required channel.

**Scene reuse tests for authors.** Replace the burial record with another archive source and check whether the scene still makes sense. If so, the structure can be reused, but source-specific names, objects, chronology, and knowledge boundaries need new prose. Replace the song with another existing oral-lore ID and verify that the scene does not falsely assign a lyric or origin. Reuse the graph topology; do not reuse a distinctive line when it no longer fits the speaker or evidence.


## Pass 18 — Calibration dialogue scenes: voices, branches, and field copy

### Scene design

This pass converts the master world bible's calibration seed into dialogue cards that can be reviewed and later mapped to a verified quest or encounter schema. All samples below are DRAFT. They intentionally describe measurement confidence, not a change to the irradiated world. They require no new named character; roles such as instrument keeper, scout, and record clerk can be assigned to existing cast members only after a cast audit.

A calibration scene should have a quiet, practical tone. People who rely on a Geiger counter learn to respect what it cannot tell them. The instrument is not a magical truth machine: a good calibration narrows uncertainty, while damaged hardware and a broad error band still call for judgment.

### Scene A — overdue discovery

The instrument keeper taps the casing with a fingernail, then stops before the second tap.

“Forty readings since the last check. That is the point where I stop calling this number clean.”

Player responses:
- “Does that mean the place is more dangerous?” → knowledge response; clarify that the meter's confidence changed, not the location's actual dose.
- “Can we still use it?” → explain the current confidence and reading-eligibility state returned by the device owner.
- “Put it on the bench.” → begin the existing one-day calibration if its actual prerequisites pass.
- “Leave it for now.” → close the scene without altering state.

Reconvergence: the keeper points to the nominal reading and its uncertainty band. The player leaves with the same available quest actions regardless of cosmetic phrasing.

### Scene B — blocked by condition

Battery route:
“The display is alive, but a weak cell can leave us with no reading at all. I can swap it if we have the right part.”
Show the action only if the existing battery replacement command is available. A response grants no battery by itself.

Sensor route:
“The cell is full. The sensor has taken too much wear to trust the next sweep.”
Show the service route only when an existing service action is reachable. Otherwise state the blocker and leave the request open.

Station occupied route:
“The bench is already holding another instrument. We can wait for that procedure to clear or take the other unit.”
Do not invent a second station capacity; read actual availability from the current owner.

### Scene C — one-day return

At start:
“Bench is reserved until tomorrow. We will keep the old report beside the new one, so nobody mistakes a cleaner instrument for a cleaner world.”

At premature return:
“Not yet. The station's day has not turned. We can use the time to compare the reports or carry on without this instrument.”

At completion:
“The needle still has a margin. It is a narrower one now. Mark both the value and the uncertainty.”

Cancellation:
“Power failed before the check settled. The old reading record is still here. We have to decide whether to restart the procedure.”
Cancellation is a system result; the line does not imply equipment damage unless the current owner reports it.

### Graph structures

The minimum graph is a short hub-and-spoke scene: overdue discovery → one of three questions → reconverge on current device status. Starting calibration exits to a quest update only after the real start command succeeds. Returning early reopens the hub with a blocked explanation. Completion reopens it with the updated owner-provided quality and uncertainty. The player can leave at every point.

A second layer may add a short branch-and-reconverge debate between the keeper and a scout who wants to leave immediately. The branch changes tone and grants no permanent relationship consequence in the MVP. A relationship-aware variant can be authored only if the existing relationship owner supplies a current predicate and Plan 22 supplies a valid effect route.

### Writing, UX, and access requirements

- Put the nominal reading, uncertainty band, and overdue state in text as well as color.
- Never communicate a critical blocker through animation or color alone.
- Keep speaker turns short enough that players can scan them between shelter tasks.
- Make the consequence of selecting Start explicit: one in-game day is reserved.
- A Start choice that fails because conditions changed since opening the dialogue must return a specific refreshed explanation.
- Do not use a false “perfect reading” line when quality is merely improved.
- Preserve controller and keyboard navigation, stable focus on refresh, close/back behavior, and readable long-line wrapping.
- Do not require the player to read all journal material to understand the essential state change.

### Location and prose reuse

The dialogue packet can be reused in a shelter-side panel, a character conversation, or a quest journal only by using presentation-specific line lengths and context. A field report discovered during an expedition should use a separate authored scene that identifies its source and date; it must not reuse the completion line unless the actual procedure completed. Optional repeats may shorten to a status reminder after the first full explanation, with first-heard state owned by the existing memory or narrative system.

### Implementation handoff and acceptance

The writer's delivery unit is a node table with stable ids, role, location context, entry conditions, visible text, responses, reconvergence target, quest update, and consequence class. It also includes fallback copy for missing device state and missing speaker state. The implementer maps it to the existing dialogue/quest authority after a catalog-level collision check.

Acceptance requires proof that all responses remain reachable under their stated conditions, the graph can exit cleanly, the scene works with no optional skill knowledge, the text agrees with live calibration values, and no dialogue option directly modifies the device or dose. These cards remain content scaffolding until the current loader and runtime consumer are named.


## Pass 19 — Listening room scenes for an attributed wiretap case

### Dramatic frame

Part 43's wiretap evidence seed offers a story about how a community should use a private, incomplete record. The following material is DRAFT and uses the existing Office ammunition transcript only as a cited artifact. It adds no transcript variant and does not claim that the named speakers exist as runtime character objects. Roles such as receiver operator, records clerk, and hearing delegate are placeholders for cast verification.

The central tension is not “is the recording true?” The player must decide how much a clear signal can establish, how to preserve the source, and whether the community should hear an allegation before it has a corroborating record. Every branch preserves the distinction between what the tape says and what has been independently established.

### Discovery card

A narrow strip of paper has been folded around a spool label. The ink has bled through the fold, but the channel code is still legible. The receiver operator does not hand over the tape until the player asks what has already been copied.

Operator: “The channel is in the index. The route that got it here is not.”

Options:
- “Play the transcript and keep the source attached.” Opens the hearing-room hub after the current player-read owner records discovery.
- “Show me what the index can prove.” Opens a knowledge scene about source metadata only.
- “Seal the identities while we investigate.” Opens a confidentiality branch if the current record system supports redaction.
- “Leave it unopened.” Closes the scene, preserving the lead without enrolling it or creating a quest unless that action is the authored trigger.

### Hub-and-spoke dialogue

At the shelter records desk, the clerk places a blank register beside the copied transcript.

Clerk: “I can enter the accusation. I can also enter that we do not know who moved the box afterward.”

Player responses:
- “Enter it as a lead, not a finding.” Begins the attributed-summary branch.
- “Find the matching issue record first.” Starts an optional corroboration objective only if an existing catalog location or record can be cited.
- “Read it into the hearing now.” Routes through the explicit submission choice and current Verdict evidence seam; if that seam is not available, explain that the record can be preserved but not admitted.
- “Keep the copy sealed.” Records a local confidentiality choice only through an existing state owner.
- “I need another day.” Leaves the quest in an active, non-expiring state.

All branches return to the hub with the current quest status and a concise source label. The player may exit without consequence.

### Cautious hearing branch

Delegate: “A clear voice is still one voice.”
Clerk: “The tape gives us an order of words. It does not give us the inventory count.”
Operator: “If we play it publicly, the Office will know we have the copy.”
Player: “Then record the source, the clarity, and the missing corroboration together.”

This branch can produce an attributed lead only if the existing evidence contract accepts the source. Otherwise it ends with a journal note that the player has chosen a cautious interpretation; it must not increment Verdict's evidence count through a UI-only flag.

### Optional side quest: The Blank Receipt

Purpose: give players a short, location-based corroboration option where the current authored world already contains a reachable requisition record relevant to the accusation. No such record is asserted by this plan. The content author must identify one existing record and valid parent site before the quest is promoted.

Structure:
1. Ask the clerk what would count as corroboration.
2. Select an available archive or storeroom site only from validated location data.
3. Find a matching receipt, an explicit mismatch, or no useful record.
4. Return to compare source quality and record provenance.
5. Choose corroborated, contradicted, or unresolved wording.

Failure-forward outcomes: the site is unavailable, the receipt was lost, or the record predates the intercepted exchange. Each ends in “unresolved,” not quest failure, and leaves the main investigation open. Rewards are a clearer journal summary and a new dialogue response if the relevant owner supports it.

### Character voice controls

- Receiver operator: careful about chain of custody; speaks in channel, spool, and copy terms; avoids declaring motive.
- Records clerk: humane bureaucrat; asks what the community can responsibly enter; avoids courtroom flourish.
- Hearing delegate: understands consequences of public accusation; asks who bears the cost of uncertainty.
- Player: gets short, plain responses with a meaningful choice. Do not force the player to adopt one character's moral vocabulary.

These are voice functions, not new cast definitions. Existing character personalities and faction allegiances must be checked before assignment. If no matching character exists, use a role card that does not persist a fabricated identity.

### Scene topology and reuse

The base conversation is a hub with five short spokes that reconverge. The optional receipt quest is a side branch with three endings and a return to the same hub. The operator's source explanation can be reused in a Codex reading view and journal detail panel, but each surface needs a length-specific version. A generic signal-provenance card can later support other intercepted documents while retaining document-specific voice and consequence data.

Acceptance requires readable source attribution, no dialogue claim that the accusation is proven, a clear exit from every branch, no duplicate transcript prose, and no direct effect applied by text selection. The live dialogue schema and read-state owner must be named before these cards move from DRAFT to data.


### Pass 19B — Additional scene cards and location copy

#### Scene: the source label

The operator turns the spool box until the handwritten channel label faces the light.

Operator: “That is what the receiver called it. The sender did not sign the box.”
Clerk: “Then the label travels with the copy. It does not become a name.”
Player responses:
- “Keep the channel label and mark the speaker unverified.” → source-attribution branch.
- “Remove every name before review.” → privacy branch, if an existing redaction owner supports it.
- “Ask whether another record can confirm the speaker.” → optional corroboration branch.
- “I do not want to copy this.” → close without recording or admitting evidence.

The operator's voice is practical rather than conspiratorial. The character is worried about a copied label being mistaken for a verified identity. The clerk is more concerned with what the community will do after reading the allegation. This contrast gives the player an ethical choice without turning either character into a villain.

#### Scene: the copy is challenged

Delegate: “If you read it aloud, people will hear the accusation before they hear the margin.”
Clerk: “Then the margin goes first.”
Player: “And if they only remember the accusation?”
Clerk: “We will have written a poor record. We can still write a careful one.”

Response A: “Submit the claim with its limits attached.” Requests the current Verdict evidence path and reports its actual result.
Response B: “Hold it until a second source arrives.” Keeps the investigation active with no hidden deadline.
Response C: “Close the case as unresolved.” Resolves the quest only if the existing lifecycle accepts an unresolved outcome.
Response D: “I want the words entered, but the voices sealed.” Uses a privacy effect only where its owner exists; otherwise present this as a local scene choice.

#### Place description: shelter records desk

DRAFT short description: A strip of felt keeps the spools from rolling against the register. The clerk has left the next line blank, not because there is nothing to enter, but because no one has agreed what the first line means.

DRAFT long description: The desk is built from two different cabinets joined by a length of brass angle. One drawer holds paper that can be replaced. The other holds copies that cannot be recalled once read aloud. A receiver hums behind a screen, low enough that the room can hear a pause. The register is open to a page with one ruled line left empty. The player may inspect the source label, ask for a transcript, or leave the record sealed. No option changes the actual document until the owning route confirms the action.

This is a room dressing description, not a new world-map location ID. If no existing shelter desk provides a home for the interaction, use the verified records interface and do not create an otherwise empty map node.

### Branching dialogue production card

- Node: wiretap_source_review_open
- Speaker role: receiver operator or verified cast match
- Entry: transcript is available in a production consumer; player has not submitted it
- Lines: use the source label; explain unknown identities; offer review, seal, submit, or exit
- Effects: none on node entry
- Response effects: player-read fact only after actual playback; submission request only after confirmation; quest step only after owner acknowledgment
- Reconvergence: wiretap_case_hub
- Fallback: source unavailable, no consumer, or current player state cannot be reconstructed

### Reuse and quality bar

The record-desk description can be adapted for other documents, but the empty register image and exact dialogue belong to this case. A reusable template should supply source label, what is known, what is missing, and the player's next choices; it should not repeat the clerk's lines verbatim across unrelated factions.

The dialogue review should include three readings: one as an isolated scene, one without any optional skill knowledge, and one with a previously read related record. At each reading, the player should be able to identify who authored the statement, what remains unverified, and whether a response will submit evidence or simply close the conversation.


## Pass 20A — The Quiet Hours: authored radio-theater dialogue sample (DRAFT)

### Dramatic design

The master world bible’s Part 43, seed 19 proposes the Machine’s tribunal broadcasts as radio theater with procedural case content. Plan 94 already expanded the Machine-Register corpus to 30 broadcasts, so this pass develops a distinct staged-hearing story rather than writing replacement technical notices. The Quiet Hours is a provisional four-part arc about overlooked night work, public correction, and the limits of formal records. No character, place, faction, or episode title is canon until the content and name-collision audit is complete.

Keep the radio text and the playable dialogue graph separate but linked by stable authored reference. Broadcast prose is short, scheduled, and legible when heard without context. The quest dialogue supplies choices, relationship texture, and the player’s opportunity to act. A broadcast can hint; it cannot silently select a dialogue response or record the player’s consent.

### Voice roles

- The relay performer is exact with words, uneasy with improvisation, and protective of the program’s credibility. Their humor is dry and used to defuse pressure, not to belittle the witness.
- The night-watch lead remembers tasks by their consequences rather than by official titles. They will correct an error privately first, but object when privacy is used to erase the crew again.
- The younger listener is observant and impatient with euphemism. They want the public to name the work, but can be surprised by the cost of naming an individual.

Keep these as role/personality cards until names and established relationships are checked. Give each a distinct information boundary: the performer knows the script and production choices; the watch lead knows lived practice; the listener knows what the audience heard. None is a universal lore dispenser.

### Scene graph: first conversation

Scene entry is available only from a real clue or verified radio surface. Use a short branch that reconverges before the player accepts a quest:

- Performer: “The hearing was written for a clean room. Our shelter has never had one.”
- Player: “Was the case meant to describe your watch?”
- Performer: “No name was printed. That is not the same as no one being described.”
- Player: “Then let me ask the people who were there.”
- Performer: “Ask first. Decide what to repeat later.”

The alternative response may ask whether the broadcast can be withdrawn. The performer answers that delivery cannot be recalled from listeners who already received it; they can prepare a correction or decline to repeat it. Both branches reconverge at a clear offer: investigate the omission, leave the matter alone, or ask to return after speaking with the watch lead. Declining does not mark the quest failed. It leaves the lead available under its authored return rule.

### Scene graph: witness conversation

Start with one hub question and three optional spokes: what the watch actually did; why the roster omitted it; and whether the witness wants public identification. Use a knowledge gate only for the roster detail, a relationship-sensitive gate for the personal account, and no reputation gate for basic access to the main story. The player can ask questions in any order. Each spoke returns to the same hub with a compact memory flag owned by the quest/dialogue contract only if the current save owner supports it; otherwise derive the recap from objective facts.

Sample exchange:

- Watch lead: “We kept the west latch from freezing shut. There is no line for a door that does not fail.”
- Player: “The hearing called the hour empty.”
- Watch lead: “The hour was quiet. That was the work.”
- Player: “May I tell the station?”
- Watch lead: “Tell them what happened. Ask me before you tell them who did it.”

If the player has already chosen public attribution, the final question changes to acknowledge the lost anonymity and offer a correction. If the player previously declined, the lead asks whether the player has returned with new information. Avoid a scolding line for a valid prior choice.

### Production standard

Each authored node has stable ID, speaker, scene/location role, conditions, text, response IDs, effects, quest updates, relationship changes, world-state changes, and next-node references, subject to the current dialogue schema and loader. Text stays in authored data. Effects are typed references reviewed under Plan 22. Conditions are reviewed under Plan 21. A node with no valid response path must have an explicit terminal marker and an accessible close/back route.

Use captions for every spoken line, avoid color-only status, keep response meaning visible before commit, and preserve keyboard/controller focus when the branch reconverges. The scene should remain understandable with audio muted and at a slower reading pace. Production cost is modest for one hub and two reconvergent spokes; it rises with unique performance, voice acting, facial animation, and localization. Those presentation layers remain optional expansion work.

## Pass 20B — Four-episode outline, repeated-visit lines, and prose guardrails

**Episode I: The Quiet Hour.** A tribunal play presents a clerk reading an empty hour from a schedule. The player hears the scene through an available surface or learns of it from the performer. End on a question, not a claim that the tribunal has judged a real shelter. A one-sentence log explains that the player has heard a dramatization.

**Episode II: The Door That Did Not Freeze.** The watch lead describes routine work that prevented a visible emergency. The setting description should carry the evidence: a latch with fresh oil, chalk marks on a shift board, and a thermos left on the safe side of a draft. Do not add a new permanent location; place the scene in an existing eligible shelter or relay space selected by the location authority. The player can ask for a private correction, public correction, or no intervention.

**Episode III: Who Gets Named.** The performer explains that a public correction will reach more people but may turn a private crew into an emblem. The listener argues that unnamed labor remains easy to repeat and easy to forget. The conflict is not solved by a skill check. Skills can reveal production detail, omitted stage direction, or a clue about how the script was assembled; they do not make one character’s consent irrelevant.

**Episode IV: The Revised Reading.** If the player chose a correction, present a short authored follow-up that distinguishes verified fact from theatrical framing. If the player chose privacy, the program corrects its general wording without identifying the crew. If the player chose silence, a private scene still resolves the witness’s request, but no public claim is made. Each ending uses the same factual basis and different disclosure scope. The recap can be read from a journal surface only if that surface is a verified consumer.

Repeated-visit lines should reflect one durable fact at a time: before accepting, the performer notes that the script is still on the desk; after consent, the watch lead asks whether the player has decided what to share; after public correction, the listener refers to the revised reading; after privacy, the performer describes the wording change without naming the crew; after silence, the witness says the choice was honored. Do not rotate random variants on each visit or suggest a consequence that did not occur.

Prose guardrails: the Machine’s voice may be formal, but the theater should not turn bureaucracy into cartoon villainy. Use procedure as dramatic pressure: a form can make a person legible, but it can also leave the wrong box blank. Keep descriptions concrete and restrained. Avoid exposition that explains the entire tribunal or its endgame; the local episode should function on its own while adding one small, compatible question to the larger story. Do not borrow phrasing from existing broadcast rows. A collision review must compare topic, image, repeated phrase, day trigger, and emotional turn against all current corpus entries and already-integrated radio content.

A prose acceptance pass checks that each line can be attributed to its speaker, the scene does not overstate what evidence proves, the player sees the disclosure audience, and every ending is represented in the state graph. A story editor signs off on voice; the data owner signs off on references; accessibility review checks reading order and text length; continuity review checks episode order and existing canon. No author should infer that a dramatic sample is already wired.



## Pass 20C — Script-ready scene pack: radio, common room, and follow-up

The following lines are original DRAFT copy for voice and graph testing. They are not copied from the existing Verdict corpus, and they should not be inserted until the collision and continuity review passes. Keep node IDs stable even if line text changes. Use content-role references until canonical character and location IDs are verified.

### Broadcast fragment: The Quiet Hours

> CLERK: “The schedule records one hour without assigned work.”
>
> EXAMINER: “Was the shelter empty?”
>
> CLERK: “No. It was quiet.”
>
> EXAMINER: “Then why is the hour blank?”
>
> CLERK: “The form has no box for a door that did not freeze.”

The final line is the episode hook, not a claim about a real game location. Keep this to a short staged exchange, followed by an unobtrusive label that the scene is an enacted case. Do not load the broadcast with explanatory lore or use it as a substitute for the player’s conversation with the watch lead.

### Common-room hub

Entry line depends on whether the player has encountered the staged case. If the case is only scheduled but not surfaced, use a general greeting. If it has actually been reviewed, the performer can ask: “Did the blank hour sound familiar?” The player may answer, “I want to know who wrote it,” “I want to ask the watch,” or “I do not want to turn a play into a charge.” These responses converge on the same objective offer but set different immediate tone. Only an explicit accept action starts the investigation.

Description: A small lamp has been turned to face the program desk. Its shade is dented near the rim, and pencil marks on the table show where pages were rearranged. The performer keeps the last script sheet under a mug so the draft will not slide when the door opens. No map-specific feature is implied by this description; place it only in a verified existing interior.

### Follow-up scene fragments

**Private correction:** “We changed the wording. We did not name your crew. The hour is no longer called empty.” The watch lead replies: “That is enough for tonight. Ask again before you call it enough for everyone.”

**Public correction:** “The earlier reading was a staged case. The work described here was real, and the people named it for themselves.” This line is available only after explicit consent and a confirmed delivery surface. If names are not consented to, use the anonymous version. The response is not a universal reputation bonus.

**Silence:** The performer asks whether the player still wants the pages kept. The watch lead says, “Keep them. A page can stay private without pretending it never existed.” This resolves a deliberate choice without presenting silence as failure or moral weakness.

### Reusability and cost

The hub’s information spokes can be reused by later episode callbacks if each line references a real state and has a neutral fallback. Do not reuse the exact moral case with another cast just to increase quest count. A new case should introduce a different procedural tension, source type, and consequence surface. A radio theater wrapper can recur as a content pattern: short authored scene, clear enacted-case label, one concrete contradiction, optional player inquiry, and a reviewed closure. That format is reusable; its people and local stakes should remain specific.

Minimum production package: one broadcast fragment, one hub description, up to six response labels, three response lines, one alternate line per final state, a brief recap, and short/long accessibility text. Expansion production adds actor-specific barks, localization variants, optional skill observations, and follow-up lines for revisits. Voice recording should wait until text IDs, consent language, and consequence routes are locked. The content author owns prose; the integration owner verifies node loading and reachability; the UI reviewer checks that a long line does not hide the response that commits an irreversible choice.

### Graph lint checklist

The graph must have one reachable entry for each supported exposure state; no invisible response may commit a public action; every branch has an explicit target or terminal marker; required variables have defaults; conditional responses are mutually ordered or explicitly non-overlapping; reconvergent nodes do not repeat already-seen exposition; and every high-impact response has a plain-language preview. A read-aloud pass should distinguish the performer, watch lead, and listener without relying on speaker labels. A transcript pass should preserve stage directions and tone for deaf and hard-of-hearing players. The script can be atmospheric, but the player’s next action remains obvious.


## Pass 20D — Follow-up dialogue: The Second Margin (DRAFT)

### Three-voice scene

The optional conversation begins with the younger listener reading the correction slowly, then turning the page face down. “I understand the new sentence. I am not sure it changes what the first one sounded like.” This is an interpretation, not a puzzle answer. The player can ask what sounded accusatory, ask whether the listener wants a different explanation, or say the correction was not written for every listener. These short spokes reconverge at the performer, who can explain the production choice without claiming that the audience misunderstood.

Sample hub:

- Listener: “I heard the clerk say the hour was blank. It sounded like someone was being blamed for not filling it.”
- Performer: “The clerk was blaming the form.”
- Listener: “Then the form should have been named sooner.”
- Player: “Would an explanation help, or would it only make the page longer?”
- Listener: “Ask me after you decide who the explanation is for.”

The watch lead joins only if the player’s parent outcome and consent make that meeting valid. They do not arrive as a universal arbiter. If the player chose public correction with consent, the lead can say: “I agreed to the work being named. I did not agree to speak for every person who keeps watch.” If the player chose private correction: “You changed the words without putting our names under them. That was the agreement.” If the player chose silence: “I asked you not to carry it. You listened.”

### Branch outcome text

**Narrow correction:** The performer prepares one additional sentence naming the limit of the dramatization. The listener may accept the clarification while keeping their interpretation. The graph reconverges to a local resolved state without implying unanimity.

**Broader explanation:** The performer offers to explain the case format and why the script left the form unnamed. This can be a later authored program note only if the delivery consumer exists. Until delivery is confirmed, dialogue says “prepared,” not “aired.”

**No further statement:** The player closes the subject. The listener’s view remains theirs; the parent outcome remains intact. The scene ends with a concrete activity—folding the page, returning the script, or leaving it on the table—rather than a moral score.

### Location and atmosphere copy

For a reused conversation hub: “The table has been cleared except for two versions of the same page. One is marked for performance. The other has a line pencilled into the margin, small enough to miss unless you lean close.” This can appear only if both authored page states actually exist in the current scene. Otherwise use the neutral hub description and do not conjure an object from prose.

The second margin is a motif, not a collectible item. It can reappear in a later conversation as a remembered phrase if the canonical quest state supports it; it should not be added to inventory, archive, or map discovery. That keeps a literary image from accidentally becoming a new item system.

### Accessibility and reusability

Keep the listener’s uncertain response explicit in captions and avoid relying on a facial expression to communicate discomfort. Response labels should say whether the player is asking, clarifying, or closing the subject. The branch must work with audio muted, and the short scene summary must preserve that no consensus was forced. Production cost is one optional hub scene, three reconvergent questions, three outcome lines, and a small callback matrix. Reuse its structure for future audience-response stories, but not the same exact disagreement; future episodes need a different listener concern and a specific reason that concern matters.


## Pass 20E — Character voice, shelter texture, and micro-quest prose bank

### Character progression by behavior

The relay performer begins by defending the script’s precision, not the Machine’s authority. After the player asks for a correction, the performer learns to name the limits of a dramatization before a listener has to complain. The watch lead begins with guarded, practical language and ends with the same caution but a clearer boundary about public attribution. The listener begins impatiently and ends able to distinguish a correction from agreement. None undergoes a sudden conversion. Their progress is a change in what they are willing to say and under which conditions.

Avoid making one character the author’s moral mouthpiece. Give each an unanswered question: the performer wonders whether a careful correction can still be used as propaganda; the watch lead wonders who will be responsible if their crew becomes a symbol; the listener wonders how to ask for recognition without claiming to speak for others. Their motivations can overlap without resolving into unanimity.

### Shelter-lore description bank

Use these small details only in a verified suitable scene:

- On the program board, yesterday’s schedule has been folded under today’s rather than erased. Someone wanted to keep the old handwriting visible.
- A common-room lamp throws light across the table but not the doorway. People can read without making everyone entering feel observed.
- The watch rota is marked in pencil. The names are kept short so a replacement can write one in quickly when the wind makes the latch stick.
- A strip of cloth covers the microphone stand when no program is being recorded. It keeps dust out and signals that the room is not currently on air.
- At the end of the hearing, someone has left one chair turned toward the wall. It is not a symbol unless a character tells the player why it matters.

These descriptions are atmospheric, not new interactable props. If a line says a document can be picked up, moved, or inspected, that action requires a real interaction and save owner. Otherwise write it as static environmental prose.

### Micro-quest: The Page Left Face Down

A player finds two versions of the same short scene on a table in a valid location. The question is not which one is true; it is which was staged and which was revised. The player can compare a visible revision mark, ask the performer, or leave the pages untouched. If they ask, the performer explains that the first draft made a person’s labor sound like an administrative failure. If they leave, no state changes and the optional lead can remain undiscovered. Resolution grants a clear program-history recap, not a collectible or evidence item. Failure recovery is an alternate verbal clue or a later return, if supported by the location owner.

**Sample exchange:**

- Player: “Which page was read?”
- Performer: “The one with the clean margin.”
- Player: “And this one?”
- Performer: “The one that says who had to live with the clean version.”
- Player: “That is not a date.”
- Performer: “No. It is the reason the date stayed.”

This exchange is intentionally suggestive. Editorial review must decide whether the last two lines are clear enough for the scene; do not rely on mystery to conceal a missing objective.

### Short quest descriptions

Available: “A staged hearing has made an ordinary hour sound empty. Someone at the shelter says the wording does not match what happened.”

Accepted: “Ask what the case leaves out. Before repeating anyone’s account, ask what may be shared.”

Blocked: “The person you need is away. The inquiry can wait until their route returns.”

Resolved, private: “The correction stayed private. The record now distinguishes a quiet hour from an unused one.”

Resolved, public: “A correction was delivered with the witness’s consent. The broadcast names the limits of its staged case.”

Resolved, silent: “You chose not to carry the account further. The witness’s request was respected.”

These descriptions are provisional and must only be displayed when the corresponding owner state is true. A journal recap should not claim that every listener accepted the correction.


## Pass 21A — The Yellow Lamp Has a Shadow: authored scene and character arc (DRAFT)

### Voices and disagreement

The central cohort character is a young adult by the current age/life-stage authority, if that authority confirms it; until then use “matured cohort member” or a neutral named role. They grew up hearing that the sun was a yellow lamp in a room too large to measure. Their curiosity is practical: if the story was wrong, which other useful lessons might be only metaphors? Their former teacher is warm and exact, willing to admit that a rhyme simplified reality but unwilling to call the children foolish for believing it. A second cohort adult, also from the cohort, sees the verse as a promise that helped them imagine a world beyond concrete. Their politics and spiritual beliefs are not preset by these roles.

The scene begins in a verified archive/codex conversation surface, not automatically at the sector named in folklore metadata. Setting copy: “The page is thumb-soft at the fold. The old line has been read aloud often enough that the crease crosses the word lamp. In the margin, a newer hand has drawn a shadow with no lamp beneath it.” Use only if a real page and annotation are part of the scene. Otherwise keep the description to the existing codex entry and character dialogue.

### Opening hub

- Cohort member: “They taught us the sun was a lamp. I thought the important part was that it was yellow.”
- Teacher: “The important part was that there could be light you did not make yourself.”
- Second adult: “That is what you meant. It is not all that we heard.”
- Player: “What should the next class hear?”

The player can ask how the verse was used in childhood, what changed after the adult saw open sky, or whether the teacher meant the line literally. Each branch offers a distinct piece of context and reconverges before the resolution choice. Do not use an “insight” skill check to declare one memory more truthful. A skill can reveal an optional historical detail about the written entry’s date or origin only if the catalog supports it.

### Three resolution responses

**Keep the verse as written.** The teacher explains that a story can be comforting without being a lesson in astronomy. The adult character says the next generation deserves an honest explanation alongside it. The authored outcome preserves the text but adds a conversation recap; no profile is changed.

**Add a context note.** The player asks for an annotation that says the verse is a child’s metaphor and not a literal description. The adults agree on wording after a short reconvergent exchange. Only expose this as an in-game codex feature if the Journal/Codex owner supports authored notes; otherwise record the choice in quest recap and dialogue only.

**Write a reply verse.** The cohort member contributes a new line about the first shadow they saw. This is not a player-generated text system. The content team authors a fixed line and includes it as a quest outcome reference after provenance review. The teacher can choose to read it at a future education session only if the existing education/content route can present it.

Sample response line: “A lamp can be carried. A shadow tells you the light is already here.” Treat this as provisional copy, not an objective fact or real-world saying. The other characters need not agree that it is the best line.

### Branching and return lines

Short hub-and-spoke nodes reconverge; only the final selection changes the outcome. Before resolution, return visits repeat the open question with a gentle state-aware variation. After the context note, the teacher says the original remains and the new note travels beside it. After preservation, the cohort member asks whether metaphor and fact can sit on one page. After the reply verse, the second adult asks whether children will be allowed to disagree with the new line too. Every ending honors the original story and the player’s chosen scope.

The dialogue graph must not use the same node for the original codex prose and the new conversation; the existing entry remains canonical, and the scene references it. Captions and transcript include speaker roles and stage directions. No scene requires audio playback, and no response hides whether it changes the written record or merely the local conversation.
### Pass 21B — Scene variants and restrained authored sample (DRAFT)

The following sample is a design excerpt to demonstrate branching shape, not a final canon lock. It should be reviewed alongside the current character roster and shelter vocabulary before conversion into data. The scene keeps its branch short and convergent; consequences remain attached to a single quest fact or optional local annotation.

#### Scene: under the yellow lamp

**Place:** shared shelter room, after the evening meal. **Participants:** a teacher who preserved the old verse and two adults who remember different versions. **Entry:** the player has discovered the marked page. **Mood:** quiet disagreement, no accusation. **Exit:** all branches return to the next authored question.

**Teacher:** “I left the line as it was. A child asked whether the lamp could cast a shadow, and I did not want the page to answer before they had.”

**Player response A — Ask about the mark:** “Was the question yours first?”

**Teacher:** “No. I wrote it down because I was afraid I would tidy it away by morning.”

**Player response B — Ask what the verse protected:** “What did the old version help people endure?”

**Adult one:** “It gave us a light we could carry in our heads when the corridor lamps failed.”

**Adult two:** “And it taught us to call every dark corner harmless. Some corners were not.”

Both answers can be true. The scene should not arbitrate which memory is objectively correct.

**Player response C — Leave the question open:** “Then let the page keep the question.”

**Teacher:** “That is a kind of answer. It is not the same as forgetting.”

All responses reconverge. The player may choose one of three follow-up intents: preserve both accounts; add a note that the accounts differ; ask whether someone wants to write a reply. These are local narrative resolutions. They do not change faction reputation or force a doctrine.

#### Optional location scene: the shuttered beacon

If the expedition selector offers the authored origin-site equivalent, the player finds a lamp housing with a repaired shade and a strip of cloth tied to its handle. Environmental evidence is deliberately incomplete. A dated maintenance mark can place the object in a period, but not prove that a specific person told the verse there. A short interaction can reveal that the lamp was repaired more often than the official log suggests. This supports the theme of preservation through repeated practical acts.

If the optional location does not appear, a character can describe the repair from memory. The player receives no “visited site” fact, map discovery, or field evidence. The fallback is a different scene with a different evidentiary status.

#### Three optional dialogue modes

**Classroom retelling:** the teacher asks the player to read the verse aloud, then pauses at a disputed line. The player may ask for a second voice or stop. Suitable for a short hub-and-spoke scene.

**Private recollection:** one adult asks to speak away from the group. The player can listen, request permission to write, or defer. This is relationship-sensitive only if an existing relationship contract supplies the gate.

**Field comparison:** after an optional site visit, the player can compare a physical mark to the remembered line. The scene must say “consistent with” or “does not settle,” not “proves,” unless the content authority has concrete evidence.

#### Writing and accessibility constraints

Keep the vocabulary concrete and the disagreement legible. Avoid making the player select a “correct” culture-wide interpretation. Provide captions and speaker labels through the normal dialogue UI; preserve complete keyboard and controller navigation; do not convey a branch solely through color. If the content framework supports localization keys, author stable keys rather than embedding UI-specific markup in prose. Every response should preview its actual tone and avoid misleading consequence promises.
### Pass 22A — The Map With a Blank Edge: debrief scenes (DRAFT)

This arc is a new authored scene family based on Part 46's expedition-aftermath question. Keep the people specific and the claims modest: a surviving team can be tired and disappointed without being blamed for a route that was not safe. The following dialogue is original proposal prose, not locked canon.

#### Scene 1 — The return board

**Place:** shelter debrief board. **Entry:** the expedition has returned through its normal inbound/completed path, and its linked quest objective remains unverified. **Speakers:** returning surveyor, route clerk, player. **Mood:** practical fatigue; no public tribunal.

**Route clerk:** “The crate is empty.”

**Surveyor:** “The crate is the part we brought home empty. That is not the same as bringing home nothing.”

**Player response — Ask what they saw:** “Start with the last thing you can place on the map.”

**Surveyor:** “The white cut in the ridge. After that, the marks on our sheet stopped agreeing with the ground.”

**Player response — Ask whether the route is safe:** “Would you send another team tomorrow?”

**Surveyor:** “No. I would send them with a better question. Tomorrow is a date. It is not a reason.”

**Player response — Let them rest:** “We can write this down after you eat.”

**Route clerk:** “Then I will leave the line blank until you are ready to call it something.”

Branches reconverge on a factual choice: record the last confirmed landmark, mark the route conclusion inconclusive, or leave the record pending. The third option must preserve the witnessed return and objective state without falsely recording a conclusion.

#### Scene 2 — The pencil line

Later, the surveyor can bring a page where a route mark stops at the edge of a water stain. The page is not magical and the stain does not encode a secret. A second character recognizes the pencil used to mark the shelter's old flood boards. That detail can open a personal conversation about how people learn to trust a mark. It must not establish that the same flood reached the surveyed location.

**Surveyor:** “I can redraw the line. I cannot redraw the ground to match it.”

**Player:** “Then we keep the break visible.”

**Surveyor:** “A break is useful. It tells the next hand where not to pretend.”

#### Scene 3 — A route chosen with care

The player may request a second expedition if the selector returns a valid candidate and the resource/party checks allow it. Dialogue previews that the new route answers a narrower question. If no valid destination is available, the same scene offers to file the attempt as inconclusive and close the local quest. No dialogue implies that the player can force a safer route by choosing optimistic wording.

#### Optional secondhand account

A caravan contact may report that another group uses a different ridge. Unless the information-flow owner confirms an actual rumor record or the quest has a valid authored contact source, present this only as an authored lead: “Someone mentioned another approach,” not as a verified route. The player can ask for the source, decline to act, or request that the contact bring a name next time. Do not write a generic, randomly named caravan survivor to fill the scene.

#### Branch structure and production rules

Use a short hub-and-spoke scene followed by a reconvergent report choice. The player can be curious, cautious, or kind; none is coded as the morally correct answer. Avoid a relationship delta unless an existing relationship contract is expressly selected. Speaker labels and captions remain visible. Each response should say what it changes: “Record as inconclusive” is precise; “Trust them” is too broad. Localized text should preserve uncertainty and avoid idioms that make an uncertain report sound like a joke.
### Pass 22B — Location texture and character voice pairings (DRAFT)

Use three recurring voices to carry the arc: **Mira**, a route clerk who distrusts unlabelled measurements but does not treat uncertainty as incompetence; **Oren**, a surveyor who remembers terrain by sound and resists having a bad weather day turned into a personal failure; **Tavi**, a junior map copyist who is eager to make the record useful and must learn that a clean line can be less honest than a visible gap. These are provisional roles. Before implementation, resolve each against the current character roster and reject or rename any collision.

#### Short location descriptions

**Survey desk, shelter:** “A steel ruler is chained to the board. Old route slips overlap at the corners; the newest one stops before the paper does.”

**Wind-cut shelf, field lead:** “The ridge narrows to a strip of stone. Every loose mark has moved since the last rain. A cord remains tied to the lower post, its knot turned toward the lee.”

**Map archive, return visit:** “The blank edge has not been filled. Someone has placed a cup on the page to keep it from curling.”

These descriptions create texture without claiming an actual expedition visited a location. The field text is eligible only in an authored location encounter that the player truly enters. The archive text may be used as a shelter scene only after the quest's return fact.

#### Character voice distinction

Mira speaks in labels and checks: “Which mark is the last one you can stand behind?” Oren uses physical orientation and sound: “Past the cut, the wind came from under us.” Tavi asks for a usable instruction: “Can I copy the line if I leave the end open?” None speaks in abstract verdict language. The player is not required to mediate their disagreement; they can set the record down and return later.

#### Small branch bank

- **Care:** “Eat first. We can write after.” Oren accepts; no morale or medical effect is implied.
- **Precision:** “Tell me the last confirmed point.” The result records only that fact if the quest owner supports it.
- **Caution:** “Leave the route open, not approved.” The player chooses an inconclusive record label.
- **Disagreement:** “Your two marks do not match.” The characters may explain the difference without one being declared dishonest.
- **Refusal:** “I cannot authorize another attempt today.” The follow-up remains optional and may be revisited if its requirements remain true.

#### Prose quality gate

Every location line must pass an evidence audit: who could know this, when could they know it, and what gameplay fact does its phrasing imply? Replace “the route is blocked” with “the team could not confirm a route” when the team lacks proof of a blockage. Replace “the player found” with “the catalog contains” unless an actual discovery occurred. Avoid repeated atmospheric descriptions when the same location is reopened; use a changed line only if an authored state change supports it.
### Pass 23A — The Third Bell: scene and voice packet (DRAFT)

This is original proposal prose for an ambient-only rumor story. It is not a claim that any current hub is noise-dominant and must not be inserted into a live rumor catalog before the policy gate in Plan 19 is satisfied.

#### Cast and voice goals

**Sella, night listener:** records what reached the speaker, not what she thinks caused it. Her sentences are short and attentive. **Niko, relay mechanic:** recognizes machinery, but is careful not to claim that a familiar pattern has only one cause. **Aven, runner:** has repeated the rumor at two hubs and worries that people now treat it as a warning. The three characters have different interests; none exists to ridicule the others.

#### Opening: the third tone

The room is warm around the receiver and cold at the door. A pencil rests across the log at the hour mark.

**Sella:** “It came after the pair. Not every night.”

**Player:** “Did you record it?”

**Sella:** “I recorded that I heard it. The machine did not keep the sound.”

**Niko:** “A loose contact can leave an interval like that. So can a relay far down the line. I can name two ways to make the note. I cannot name which one was there.”

**Player response A — Preserve the distinction:** “Write down what you heard, and leave the source open.”

**Sella:** “That I can sign.”

**Player response B — Request a check:** “Can we compare it with the relay clock and the room log?”

**Niko:** “We can check both. If they agree, that still will not tell us what the sound meant.”

**Player response C — Stop circulation:** “Let's not pass it on until we know more.”

**Aven:** “I can tell the next person it is a story, not a warning. I cannot make them forget the warning they already heard.”

#### Middle: attribution, not consensus

The player may listen to one more account. The line “I heard it too” is presented as personal testimony, never as a system-level confirmation that the sound occurred in the world. A later interview can reveal that a second person remembers the same interval but not the same number of tones. The scene should allow both people to be sincere without resolving the discrepancy by authorial decree.

**Aven:** “They ask whether the east gate will open.”

**Sella:** “I did not say there was a gate.”

**Niko:** “Then we write that down too. The question is traveling farther than the sound.”

The player can mark the audience as “notified of uncertainty” only if the current rumor or dialogue owner has a real recipient/action contract. Otherwise this is a private conversation and no propagation effect occurs.

#### Endings

**Attributed note:** “Sella reports an intermittent third tone at the listening room. No source has been established.” This is a journal summary of testimony if the journal owner can render it; it does not assert a sound event in the world.

**Bounded negative finding:** “The clock and room log do not corroborate the report during the checked interval.” The conclusion is scoped to those checks. It does not mark the witness unreliable or delete future possibilities.

**Unresolved lead:** The player leaves without a conclusion. The story can reopen only through an explicit new account or evidence event, not a random daily prompt.

**Do not circulate:** The player chooses not to repeat the claim. The characters acknowledge the choice; no faction standing or NPC trust is silently reduced.

#### Diegetic description and presentation

The log sheet has two columns: “heard” and “source.” The first contains a pencil mark; the second is blank. Use the blank as a visual metaphor, not an error state. Provide a text label (“source unknown”) so color or layout is not the only cue. Audio, if later produced, must not accidentally encode three literal bell strikes as proof; any sound playback is illustrative and should be labeled as reconstruction or ambience.
### Pass 23B — Dialogue graph and prose variants (DRAFT)

The scene graph should be intentionally small. Ambient rumors are not a reason to write a sprawling conspiracy tree. Their value is in contrasting testimony and careful language, then allowing the player to leave the source unresolved.

#### Maintainable node sketch

| Node | Entry condition | Response family | Effect ceiling |
|---|---|---|---|
| `third_bell_report` | approved ambient record surfaced | ask source / ask interval / leave | dialogue only |
| `third_bell_check` | player requests available checks | compare log / compare clock / stop | quest fact if owner supports it |
| `third_bell_accounts` | one or more separate accounts available | hear second account / preserve one / stop | witnessed-account facts only |
| `third_bell_resolution` | investigation fact set complete or player chooses to stop | bounded finding / unresolved / do not circulate | quest result, no world effect |
| `third_bell_forward` | explicit share action and real delivery route exist | send attributed note / keep private | rumor-system command only |

Nodes reference stable IDs and canonical predicates. A `third_bell_forward` node is omitted entirely when the host cannot confirm a receiver. Never show a clickable choice that only changes prose while promising delivery.

#### Prose variants by register

**Log register:** “Report received after the second interval. Listener count: one confirmed, one remembered. Source: not established.” **Shelter conversation register:** “I heard it twice. The second time might have been the pump.” **Field note register:** “No corresponding change at the relay clock during the checked span.” **Private response register:** “You can keep the question without giving it a name.”

These variants communicate different evidence scopes, not random tone. The content system should choose based on scene type and known facts. It must not randomly switch a personal memory into an official log entry.

#### Side-scene: after the shift

When the player returns later, Sella is wiping dust from the receiver dial. The pencil remains across the hour line.

**Player:** “Did it happen again?”

**Sella:** “I heard the pump. I wrote ‘pump.’ I did not write ‘bell.’”

**Player:** “Are you sure?”

**Sella:** “I am sure what I wrote. The sound can keep its own answer.”

This short branch shows skill in reporting without turning the character into an infallible sensor. If the new visit lacks a fresh sound event, it is a fixed character line, not proof that the rumor recurred.

#### Localization and accessibility

Avoid sound-only distinctions between “two” and “three.” Text captions must describe any illustrative sound. The labels for “source unknown” and “not corroborated” must be localizable as separate states. Reading order should be speaker, utterance, evidence label, response. Do not use waveform color alone to indicate confidence. Provide the actual affected audience before the player forwards a note.
### Pass 23C — Additional dialogue fragments and sensory restraint (DRAFT)

Use these as alternate lines only after the scene's evidence gates are approved. They should not all be shown in one conversation.

**At the receiver:** “The needle moved. The log did not.” — mechanic, when there is a real instrument discrepancy.

**At the shelter table:** “I can repeat what I heard. I cannot lend it a sender.” — listener, when asked to name an origin.

**After a bounded check:** “For this hour, the clock kept its own time.” — operator, when a real clock comparison was performed.

**When a second account differs:** “You remember two. I remember a space between them.” — witness, preserving memory difference without accusing either person.

**When the player declines to forward:** “Then it stays with the people who heard it.” — clerk, if no public delivery occurs.

**At the close:** “We left the source column empty. That is still a record.” — listener, only if the authored interface genuinely preserves the entry.

#### Sensory and tonal restraint

Do not represent a source-free rumor through a supernatural sting, ominous camera treatment, or emergency interface color that tells the player a threat is real. If a sound asset is later authored, it is atmospheric and must not imply a canon event or location. Keep the acoustic description human-scale: a hum, a metallic interval, a relay tick, a memory of a tone. The scene may be eerie because people respond to uncertainty, not because the game secretly confirms an unseen force.

#### Repeated-visit copy

If nothing new happened, repeat no rumor prompt. Use ordinary room texture: “Sella has closed the logbook. The receiver keeps its low mechanical hiss.” If a new account was actually added, name the account as new and identify its source. Do not imply the sound itself recurred merely because the player revisited the hub.

## Pass 24A — Field observation dialogue and original Codex-facing prose (DRAFT)

### Scene intent

Use the world-bible Part 46 question as a human-scale content test: when a field guide entry appears, who actually saw the evidence, and what does the survivor believe it proves? The scene is optional, short, and attached to existing shelter/codex interaction. It does not add a new location or replace the existing Codex panel. The player can choose to share a verified observation, keep a sensitive location private, or mark a second-hand report as uncertain. Each response reconverges on a readable entry while preserving the distinction between an authored description and a player-specific event.

### Sample hub-and-spoke scene (DRAFT copy)

**Speaker: Mara, trail medic. Condition:** valid field-guide entry has just been unlocked through a verified source. **Location:** existing shelter codex interaction.

Mara: “You marked the ash-white tracks. Did you see the animal, or only the print?”

- **I saw it cross the cut.** “Then write what it did. Leave the rest blank.” Effect: local wording records direct observation, if an existing journal field can represent it.
- **I found the tracks.** “A track is a direction, not a face.” Effect: present the entry as sign-based evidence; do not claim a sighting.
- **Someone told me.** “Keep the name. Mark the source.” Effect: use reported wording, not direct observation.
- **Keep this place private.** “Fair. Some marks are safer without an address.” Effect: cosmetic/local privacy choice unless an existing location-discovery owner supports a real access consequence.

Mara: “The guide is a tool. It is not a promise.” End node: return to Codex. If the entry was already unlocked, use a brief repeated-visit line acknowledging the old mark without issuing another reward. If the source is second-hand, Mara should not falsely celebrate a verified unlock.

### Location and description fragments

**Codex empty state:** “No entry yet. A blank page means the record has not been earned; it does not mean the land is empty.”

**Field station description:** “A map weight pins the paper against the draft. Beside it, someone has drawn a track twice: once as it was found, once as they hoped it would lead.”

**Journal feedback, direct observation:** “You added a field note from what you saw. The guide records the mark; the animal keeps its own counsel.”

**Journal feedback, travel report:** “A traveler’s account was added with its source still attached. The report is useful. It is not a sighting.”

These are content candidates, not committed strings; review against established character voices and localization conventions. Keep the lines concise enough for UI and do not encode effects in prose. If current UI cannot show source distinctions, do not imply it can: first establish the data and presentation contract in the integration plan.

### Reusability and production cost

The reusable unit is a scene template with explicit condition, speaker, source kind, node IDs, response IDs, effects, quest updates, relationship changes, and world-state changes. Most instances should use a two-to-four response hub-and-spoke that reconverges. Reserve a deeper branch for a genuinely different quest or faction consequence. Cost is chiefly source audit, narrative pass, UI fit/localization, and one integration review per new condition; do not multiply scenes for every catalog entry when one source-aware scene can serve a family.

## Pass 24B — Dialogue variants, accessibility, and content guardrails (DRAFT)

Repeated visits should acknowledge state without nagging. First visit teaches how to read provenance; later visits can show one short contextual line keyed to an actual state such as first unlock, prior report, or an already-known entry. Do not use random dialogue variation for facts about what the player did. If deterministic variation is desired, use the existing seeded content choice only after its owner is verified.

Knowledge-gated lines should explain their basis in natural language (“the tracks stop at the fence”) and remain skippable. Reputation gates should not prevent the player from reading an entry already earned. Faction-specific versions may change trust and terminology but must not mutate the underlying species fact. Failed-quest dialogue should acknowledge a real failed or postponed state and give a truthful next step; it must not imply a new location has spawned. Skill-reactive dialogue can change interpretation only when that skill is already an authoritative capability and the text does not grant a hidden mechanical bonus.

Accessibility requirements for a future UI implementation: keyboard/controller navigation reaches all response choices; focus remains visible after returning from the Codex; text does not rely on color alone to distinguish observed/reported/uncertain; the source is readable at supported scaling; and a repeated visit does not trap the player in an unskippable scene. The plan adds no new panel or modal. Use the existing surface and lifecycle.

Review checklist: voice distinction; evidence accuracy; no accidental claim of map discovery; no undisclosed resource/faction effect; no duplicate quest acceptance; no repeated unlock reward; string-key ownership; branch reconvergence; subtitle/localization length; and a graceful response for empty/missing source metadata. This remains DRAFT until the catalog audit establishes which scenes are reachable.

## Pass 25A — Mid-route storm story beats and grounded voice samples (DRAFT)

### Scene package: “Last Clear Time”

This content develops the World Bible’s storm-window question as an optional expedition story, not a new weather simulator. It depends on a verified active-sortie warning/result seam. Until then, the scenes are debrief-only and may be triggered solely by an actual expedition outcome that current owners expose.

**Opening report, radio room (DRAFT):**

Operator: “I have their last clear time. I don’t have a position after it.”
Quartermaster: “Do we know if the filter was changed?”
Operator: “We know what they signed out. That isn’t the same thing.”

Response options: **Read the dispatch estimate aloud** (reconstructs the forecast; no accusation), **Ask for the raw time mark** (opens the evidence line only if a current journal/record owner supports it), **Send another call** (only if a real radio command exists; otherwise unavailable), **Stop the replay** (cosmetic/consent choice). The scene reconverges on a practical question: which team member needs treatment, repair, or a second interview? Do not allow dialogue to apply medical treatment, change route risk, or resolve a missing sortie.

**Debrief, returned team:**

Surveyor: “The board had a white edge when we went out.”
Medic: “You said the mark was dry.”
Surveyor: “It was. Then it wasn’t.”

The disagreement is about observation time, not a scripted liar. A skill/knowledge-gated line may distinguish precipitation from accumulated dust only if the game already has an authoritative skill consumer. Otherwise present the disagreement without a faux expert check.

### Three response paths

1. **Protect the witness:** treat exposure or fatigue through existing medical/needs owners; let the objective remain incomplete. Delayed callback: the same character refuses a second departure until equipment inspection is logged, only if such an inspection command exists.
2. **Preserve the route record:** compare the player-visible forecast and actual report. If an existing journal can keep both, retain disagreement; otherwise keep the difference in scene text without promising persistent evidence.
3. **Prioritize faction delivery:** send only recovered material through existing expedition cargo/trade/faction pathways. Standing changes require an authorized faction effect; the dialogue itself cannot grant it.

### Environmental copy and location detail

**Route marker:** “The painted arrow is still visible under a skin of gray. Its lower half points into the ditch where the signpost has settled.”

**Temporary shelter, if an existing location permits the scene:** “A strip of filter cloth is tucked under the door. Someone used it to stop the draft and left the serial number facing out.”

**Debrief journal:** “The last clear call and the return report do not describe the same weather. The team had no position record after the marker.”

These lines report evidence without declaring who made the wrong call. Do not label an encounter as a “storm shelter” if no catalog location backs it. Keep station, route marker, and destination terminology consistent with existing IDs and UI labels.

## Pass 25B — Branch matrix, replayability, and production scope (DRAFT)

| Branch | Player intent | Mechanical prerequisite | Immediate result | Delayed callback |
| --- | --- | --- | --- | --- |
| Return early | Preserve people/time | Existing return command or a terminal result that already returns | Objective may remain incomplete | A later expedition revisits the route if available |
| Force the weather gate | Accept known cost to keep schedule | Current `WeatherGateBlock` with authored force cost | Existing stamina/radiation owner applies cost | Debrief records the choice only if journal contract supports it |
| Wait/hold | Avoid worsening exposure | Requires a verified in-progress wait/camp command; otherwise omit | No invented pause state | A future route check uses only real elapsed time |
| Continue without new signal | Maintain objective | Current route tick continues; warning must not pretend to be actionable | Existing encounter/exposure resolution applies | Team may return with partial proof |
| Abort evidence collection | Protect source/privacy | Existing quest response supports abandonment/postponement | Quest marks according to its owner | Future dialogue acknowledges the chosen limit |

Reusability should be template-first but voice-specific. Each instance references an actual warning source, an actual expedition result, one responsible location label, and an observable next action. Do not write one dramatic storm scene for every weather type. Black blizzard can obscure route markers; ash fallout can affect filters and dose; flood/thaw changes access; thermal inversion affects shelter and radon. These distinct outcomes need separate factual grounding and may be deferred until owners are verified.

Minimum viable content is one forecast scene, one debrief scene, and three short outcomes that require no new location. Expansion scale adds survivor arcs, optional faction testimony, and a return visit to an existing map location. Production cost includes route/source audit, dialogue implementation, localization, UI truncation checks, and branch regression. A unique CG or full cinematic is not justified by the current evidence.

The scene must be skippable, keyboard/controller navigable, and accessible through the existing journal/debrief surfaces. Source disagreement should not rely on color, tiny map symbols, or audio-only cues. Avoid a forced modal at a lethal moment. If no actionable in-route command exists, the story belongs in a post-result scene and must not fake a player decision.

## Pass 25C — Additional quest scenes, voices, and diegetic copy (DRAFT)

### Scene: before dispatch

Quartermaster: “The estimate is for this weather.”
Surveyor: “How long is ‘this’?”
Quartermaster: “Long enough to get the gate. Not long enough to promise the return.”

Responses: **Take the short route** (only when an eligible route exists); **Wait for the next forecast** (only if current day/forecast system supports waiting); **Send no one** (must remain a valid refusal). The dialogue describes the estimate’s limits but never calculates a new risk. If the player declines, no hidden quest penalty should be applied.

### Scene: returned gear inspection

Mechanic: “This buckle was opened under load.”
Surveyor: “It held.”
Mechanic: “That is not what I asked.”

Responses: **Record the damage**; **Repair it before the next trip** (only through an existing repair command); **Keep the team off the route** (only if a real assignment/dispatch gate supports it). The mechanic’s concern is functional: the next trip estimate can only use current gear state if its owner already feeds the condition into estimate math. If not, write the line as advice rather than a mechanical guarantee.

### Scene: faction courier after a lost window

Courier: “The crate was due before the road changed.”
Player: **Show the dispatch note**, **Offer what returned**, or **Refuse to certify the route**. Faction consequences require an existing faction command and explicit cost/reward. “Show” does not automatically prove weather causation; the note proves the schedule the player saw. “Offer what returned” consumes only items actually in inventory. “Refuse” can close the conversation without punitive hidden standing.

### Environmental fragments

- A shelter log has three columns—departed, expected, returned. The last column is blank for one row, then completed in a different hand.
- A vehicle tarp is patched with a route map whose ink dissolved along the fold; the destination remains legible while the return mark does not.
- A radio headset has a strip of tape bearing two times, one crossed out and one written over it. There is no explanation until a second source arrives.

These details are optional authored props/text, not proof of an event or a new procedural asset system. Place them only at existing accessible locations with a real event predicate. Ensure codex/journal copy does not duplicate every environmental clue verbatim.

### Register and variation

The surveyor speaks in distances and visible marks; the quartermaster speaks in quantities and condition; the operator speaks in times and gaps; the faction courier speaks in delivery windows and signatures. None narrates the theme. Repeated-visit lines should shorten after the player has read the record. Translation must preserve uncertainty terms (“expected,” “last known,” “reported”) and not upgrade them into certainty. Each scene returns to a useful player action or a clear end; no branch ends in a decorative dead node.

## Pass 26B — Dialogue packet schema, graph linting, and scene kit (DRAFT)

A dialogue packet separates what a speaker says from what the game is allowed to do. The node owns stable identity, speaker, optional location, eligibility conditions, localized text key or authored text field, player responses, outgoing edges, and presentation hints. Responses name a visible action and point to a consequence declaration; they do not embed imperative code or mutate world state by themselves. Conditions read canonical facts supplied by quest, relationship, faction, expedition, skill, or location owners. If a fact has no owner or API, it remains an authoring question rather than an invented flag.

### Node authoring contract

Each node has a unique stable ID and one clear scene purpose: introduce a fact, invite a decision, acknowledge a prior action, or close a conversation. A response states the player’s intent in language that matches its effect. “I’ll take responsibility” must not secretly mean “spend supplies and lose faction standing.” Text carries speaker voice and uncertainty. It should avoid narrating internal variables or promising a system outcome that the current game cannot produce. A response with no gameplay effect can still be valuable, but should be presented as tone or information rather than a false mechanical choice.

The schema should support `conditions`, `text`, `player_responses`, `effects`, `quest_updates`, `relationship_changes`, `world_state_changes`, and `next_node` as declarative fields. This list is conceptual until compared with the actual dialogue data authority. Reuse the current parser and catalog integrity validator where available. Do not introduce executable expressions, arbitrary scripting in JSON, or a second dialogue router merely to satisfy the document shape.

### Graph lint rules

- Every response edge resolves to a node or explicit terminal.
- Every gated node declares why it can be unavailable and has a reachable alternative where the scene is mandatory.
- A graph has a start condition and at least one legitimate terminal.
- Loops are allowed for hub-and-spoke conversations but require a stable exit and must not repeat one-time effects.
- A reconvergent branch preserves the selected response in a fact only when later content needs to remember it.
- Choice labels are distinct after localization and do not imply mutually exclusive outcomes when both lead to the same scene.
- One-time effects are idempotent or transition guarded; returning to the hub cannot award the same resource twice.
- Text lengths, missing substitutions, speaker fallbacks, and keyboard/controller focus order are checked in the final rendered surface.

### Reusable scene kit

**Linear scene:** one entry and one path, for a short warning, tutorial reminder, or fixed disclosure. **Hub-and-spoke:** one topic menu, several self-contained topics, and a clear leave option; topic visits do not silently commit. **Short branch and reconverge:** a meaningful local tone or evidence choice followed by a common next beat. **Knowledge-gated:** a player skill or discovered clue opens a precise additional question. **Relationship/faction gated:** a truthful alternative response becomes available from the relevant owner’s state; do not infer access from dialogue history. **Repeated-visit scene:** first visit introduces the issue, later visits acknowledge the actual prior choice or give a concise no-change response. **Quest-start scene:** the quest update is visible and commits only when the player accepts or performs the declared trigger.

### Example: return-room exchange

Entry condition: an expedition result has been committed by the existing owner and the debrief scene is available. The mechanic says, “The clasp is bent where it meets the load ring. I can tell you what I found; I can’t tell you when it happened.” Responses: **Ask what was inspected** (knowledge request; no state change), **Record the damage** (only if an existing report/journal command supports it), **Leave the report open** (navigates to a terminal that preserves uncertainty). The character may have a different line if the player previously chose not to certify the route. That line acknowledges the choice without upgrading it into a relationship score.

### Prose review and production

Review voice separately from state behavior: a speaker sheet gives vocabulary, sentence rhythm, what they know, what they avoid, and a short sample. Scene writers receive the facts available at that scene, not an omniscient campaign summary. The editor checks that variable insertions do not create false grammatical or factual claims. The implementation review maps each accepted action to a supported command or labels it a non-operative narrative response. QA checks one route per node kind, a gated branch, a return visit, and a localized long-label case. The core slice uses existing dialogue surfaces; portraits, voiced performance, cinematic blocking, and large companion arcs are expansion scope until production capacity and data wiring are verified.

## Pass 27 — Dialogue and diegetic packet for “The Last Dry Strike” (DRAFT)

This content packet turns the assay-culture seed into a short, playable scene set. The mystery is procedural rather than criminal: two workers called a batch by different standards. Each voice shows what that person handled and what they cannot know. No speaker explains the full theme, invents manufacturing expertise, or claims the game’s item-quality mechanics changed.

### Primary scene: the paired test slips

**Mara Venn, record keeper:** “This line says it lit. It does not say how long the box sat beside the intake.”

**Player responses:**
- **Read the second slip.** Opens the next node if its record is available.
- **Ask why ‘passed’ was written.** Mara explains that a single successful strike was the shop’s old acceptance test.
- **Ask whether the writer lied.** Mara corrects the premise: “No. They answered the question on the card.”
- **Leave the report open.** Exit without choosing an interpretation.

The node presents an explicit source citation or record title in the journal surface. It does not mark a hidden truth-discovered state merely because the scene opened. If the second slip is absent, the second response is removed and replaced by a clue route or a clear statement that the storage interval is unknown.

### Secondary scene: the clinic transfer shelf

**Iven, store worker:** “It came in with the tins. I remember the wet edge of the carton, not which box was inside it.”

**Player responses:** Ask what was wet; ask who signed the delivery; stop asking. The first answer can establish a leak, not batch identity. The second is knowledge-gated only if a real delivery mark is present. The third ends cleanly. None changes clinical inventory or certifies that a patient received a failed product.

### Report scene: the public card

The player chooses one of three concise labels: **Passed under the first test; storage result incomplete**, **Records disagree; keep both**, or **No certification from this evidence**. The line below the choices previews the scope: it applies to the written report, not to market stock, inventory, recipe outcome, or faction standing. The character can object, but cannot veto the player’s archive choice unless a current owner explicitly provides that command.

### Voice and physical setting

Mara uses condition words and dates. Iven remembers shelf position, water marks, and delivery handling. A young worker speaks in box counts but admits they copied the word “passed” before checking the back page. A later investigator uses careful administrative language and is willing to file a blank. Do not give each survivor a polished metaphor. Let the scene show a gum line on the paper, a carton softened at one corner, and pencil pressure darker on the word “passed.”

The proposed local anchor is a workbench or archive surface already present in the canon. A separate location such as “Dry Store Annex” is a proposed content role, not an authorized location ID. An interior description may include a slate of daily jobs, waxed paper stacked beside absorbent cloth, and a small jar holding rejected match heads, but the jar remains set dressing unless an existing item authority makes it interactive.

### Optional character thread: “One Box for the Walk”

A departing surveyor requests a sealed carton because the route has no safe way to relight a lamp. The player may ask the quartermaster to check current supply, decline, or propose a return trip with a different plan. If no canonical match supply exists, the dialogue must make this a request to record an unresolved need; it cannot consume a fictional item. If an existing owner exposes the item and a command, the prompt states the quantity and commits through inventory. The later line reflects the real result: supplied, unavailable, or refused.

### Editorial and implementation gates

All response labels survive localization at the minimum UI width. The report scene remains keyboard/controller navigable and closes without trapping the player. Text distinguishes immediate ignition from storage testing. Dialogue history does not reroll each time the panel is refreshed. Each response either maps to an existing command, navigates, or is clearly an information request. The first release slice is two speakers and four nodes; optional performances, new location art, and a wide character arc remain expansion cost. None of the scene copy should be inserted into production data until IDs, consumer routes, localization fields, and continuity references are verified.

### Pass 27B — Full hub, branch, and callback sample (DRAFT)

**Hub entry: workshop archive.** The player can choose: **Read the first test**, **Ask who kept the box**, **Compare the storage sheet**, or **Leave**. Topics are not ordered as a puzzle lock; the player can leave after one. The hub shows a small source label above each excerpt. If a record has not been discovered, the unavailable topic is either omitted or given a truthful “No second sheet is filed here” explanation.

**Node: read the first test.**

Mara: “One sample from each end of the carton. Both caught. That is the whole sentence the card asked for.”

Player: **So it passed?**

Mara: “For the card’s question. The card did not ask about a wet drawer.”

Player: **Who wrote the card?**

Mara: “Eda wrote the form. I filled this row. Those are not the same job.”

Player: **Leave it as written.**

Mara: “It can stay. The back is blank because the back was for corrections.”

This scene reveals facts but commits no report choice.

**Node: compare the storage sheet.** If available, Mara points out that the second writer waited longer and counted failures. If unavailable, the hub does not claim the player has seen it. The player can answer: **The tests asked different questions**, **The later sheet is more useful**, or **Neither one settles the delivery**. These response IDs only become persistent if the report decision later reads them.

**Short branch and reconvergence.** If the player asks whether Mara is protecting the earlier worker, she answers, “I am protecting the words on the card. You can decide what to do with them.” If the player asks whether she is avoiding blame, she says, “I am avoiding a name I cannot prove.” Both paths reconverge at the public report menu. They change local tone, not faction standing or relationship score.

**Return callback, one expedition later.** If the player published the correction, Mara says: “Someone copied the interval onto the new sheet. They kept the word incomplete.” If the player preserved both records, she says: “The two cards are still together. They are easier to argue with that way.” If the player declined certification, she says: “The blank stayed blank. Somebody has to leave it blank.” If quest persistence cannot support those exact branches, use one generic acknowledgement rather than inventing memory.

**Diegetic descriptions.**
- A carton corner is soft enough to bend without tearing. Someone pressed a ruler against the water tide mark and wrote its height in the margin.
- The first card has two pencil ticks. The second has a number written over a smudge, with no initials beside the correction.
- The shelf has a dry side and a wall side. Dust lies evenly on the dry side; the wall side shows a pale outline where a box was moved.

These details imply handling without proving which carton carried which test result. Do not add an interactive inspect choice unless the current location UI can route it.

**Character sheet: Mara Venn.** Surface: patient archive clerk, quick with forms. Function: keeps source documents sorted. Contradiction: she dislikes official classifications but uses them precisely. Material history: sharpened pencil kept in a prescription tube. Under pressure: stops speaking and writes the exact question on a blank line. Hook: respects the player who retains an unknown; resents being asked to invent intent. Mechanical hook: none assumed; use only existing quest/dialogue facts.

**Character sheet: Iven Saar.** Surface: clinic store worker. Function: receives deliveries and tracks shelf condition. Contradiction: remembers the leak by the smell of damp cardboard but forgets dates unless they are on a tag. Under pressure: checks physical seals before answering. Material history: a folded inventory strip in a coat cuff. Hook: may disagree with Mara about what counts as a useful record. Mechanical hook: no medical diagnosis or supply quantity is inferred from memory.

The full scene kit supports linear scene, hub-and-spoke, short branch/reconvergence, knowledge gate, and repeated visit without an extra dialogue authority. It is suitable for a data-driven implementation only after the actual narrative schema and consumer are confirmed.

## Pass 28 — Three voices at the bedside: authored handoff scene

This dialogue packet turns Part 46’s caregiving prompt into a restrained, implementable conversation. It is DRAFT copy. Names must be mapped to real survivors only after entity-level voice and canon review. It can use an existing shelter dialogue surface; it does not require a new bedside location, quest engine, or conversation database.

### Scene card: The Cup on the Rail

**Speakers:** a caregiver currently assigned through CaregivingSystem; the assigned patient; an optional roster keeper whose role is read from DutyRosterSystem. **Setting:** shelter/ward context already supported by the host. **Player goal:** understand the request before changing the assignment. **Graph shape:** hub-and-spoke opening, two optional spokes, reconvergent decision, three short endings. No choice is labeled good or bad.

**Caregiver entry:**

> “I can do the work. That is not the same thing as being able to be the only person who does it.”

Responses:
- “What would relief look like?” The caregiver asks for a second eligible survivor or an end to the assignment, while clarifying that no replacement is promised.
- “Did the patient ask you to leave?” The caregiver answers honestly that the request is theirs; this avoids casting patient frustration as consent to end care.
- “I need to check the roster first.” Open the existing roster interaction if available; no quest effect yet.

**Patient entry:**

> “I want help when I ask for it. I do not want my name to mean that someone else disappears from the rest of the shelter.”

Responses:
- “Would you accept another caregiver?” — “Maybe. Let me meet them before the board calls it settled.” This is authored preference, not a persisted consent flag. Assignment remains governed by current validation and the existing player command.
- “Would you rather be alone?” — “No. I would rather be asked.” Do not translate this into an engine rule that automatically rejects assignment.
- “I will speak with the person tending you.” Return to caregiver spoke.

**Roster keeper entry (optional):**

> “A name leaves one column and the blank does not stay blank for long. That is how the board is meant to work. The trouble is when we stop seeing the person whose name moved.”

Use a specific previous role only if the current roster query or supported history confirms it. Otherwise keep this generic. The speaker does not issue a roster assignment for the player.

**Reconvergent choice:**
- “Look for an eligible replacement.” Hand off to the existing caregiving panel/command. Return with result-backed text only after the command result is known.
- “End the assignment.” Use existing unassign behavior and say plainly that the patient may have no caregiver afterward. Keep bond-history semantics accurate: Core retains bond strength when assignment ends.
- “Leave the assignment as it is for now.” Close with acknowledgement, not false success. The quest remains available only if its owner supports that state.

### Result-backed endings

**Assignment succeeds:**

> “The board changes. No one calls it a cure. Iven watches the new name arrive, then looks back at Mara. ‘Ask me again after we have met,’ he says. Mara nods. The cup is still cold, but it is no longer the only thing either of them can see.”

Names and pronouns come from mapped entities. This promises a meeting; if no scene can fulfill that promise, remove the sentence.

**Assignment rejected or stale:**

> “The board does not change. The request is still heard; the assignment is not.”

Follow with a localized status reason supplied by the host when safe. Do not claim the selected survivor refused unless the failure code truly means that. Generic validation failure is not character rejection.

**Unassignment succeeds:**

> “Mara steps back from the rail. Iven keeps the blanket within reach. Neither calls the space between them a clean ending.”

Do not imply improved fatigue or a new caregiver. The existing event records assignment end; relationship bond remains in the care owner.

**Defer:**

> “You do not move the name. You do not tell them the problem is solved. The request remains where both can see it.”

Only use “remains” if the quest system truly retains availability. Otherwise say the conversation ends without a recorded resolution.

### Voice and prose controls

Caregiver speech is direct and specific, never saintly or resentful by default. Patient speech retains agency without turning dialogue into a medical prognosis. The roster keeper uses workboard metaphors and observes opportunity cost, not management jargon. Keep lines short for subtitles and allow reconvergence after one spoke, so hearing every optional line is not required to reach a valid command. Avoid repetitive “you are tired” declarations; fatigue may not be surfaced to dialogue and is not the same as emotional exhaustion.

### Dialogue node shape

A maintainable node references node ID, speaker ID, shelter location tag, eligibility conditions, authored text key, response IDs, owner command intent, result mapping, quest update contract, local callback tag, and next node. Text does not embed direct save mutations. A local tag such as caregiver_heard_request may exist only in the conversation instance if the current graph owner supports ephemeral state. Do not create durable memory solely to make the prose flow.

**Branching potential:** add one short companion callback after the existing dialogue-threshold event only if an established listener consumes it; add a later visit where the caregiver reports an unrelated personal activity only if a real state fact supports it. **Reusability:** the opening/reconvergence structure suits other labor handbacks; voices and conflicts remain specific. **Production cost:** medium for three speakers and command-result returns; high if an event bridge or persistent dialogue memory must be invented. One scene is core-scale; a multi-character care anthology is expansion content after a roster/voice audit.
## Pass 29 — Three local records, three voices of place

The micro-location coverage prompt suggests a writing goal beyond increasing the count: make the exact-site records feel as if they belong where their location binding sends the player. The current three bound definitions provide a compact DRAFT prose portfolio. They are authored encounter records, not new dialogue nodes; any spoken follow-up requires a current consumer that can open dialogue after encounter resolution.

### Hospital chapel ledger: “The Hand That Kept Writing”

**Current authored anchor:** the visitors’ ledger stops after names and bed numbers, while one date repeats; the chapel candle box contains stubs and matches. The binding is abandoned_hospital. Preserve the restraint: no narrator declares why the writer stopped or who died. New follow-up prose can be delivered through a journal/quest surface only if that surface is already reachable.

**Optional observation line:** “The dates are neatest where the ink begins to thin. The last line is not crossed out. It simply has no name beside it.”

**Short branch, after reading:**
Archivist: “Did the list tell you who stayed?”
Player: “It tells me someone kept a place for visitors.”
Archivist: “Then write that. Leave the rest to the blank.”

**Choice tone:** reading is an act of attention, not proof of a cure or a memorial reward. Taking the lighter is an item action in the existing encounter. Do not restate it as “taking matches”: the live choice grants cigarette_lighter, a content detail worth preserving exactly. Do not add a third choice or alter morale/guilt values in a prose-only plan.

### Flooded subway depot: “The Quiet Line”

**Current authored anchor:** sealed crates move on a greased pulley along the flooded concourse; the line bears the Undertow’s tar-symbol. A player may map pulley anchors or cut a crate. Do not infer the current owner, route destination, legal status, or faction doctrine from this one mark.

**Optional note after mapping:** “The anchors are spaced for a person who knows the water’s pull. The map records the line; it does not tell you who will come to collect what moves along it.”

**Optional follow-up exchange:**
Surveyor: “You drew the pulley, not the cargo.”
Player: “The cargo changes hands.”
Surveyor: “A route lasts longer than one crate.”

**Choice tone:** mapping leaves the line alone; cutting a crate is theft within the authored scene. Do not reward map notes with a new cartography stat unless the existing cartography authority exposes one. Do not turn the faction-mark into a reputation effect without a supported consequence.

### Checkpoint Gamma: “Square for Paid”

**Current authored anchor:** the levy board shows household names, quotas, and square/circle marks for paid/owed; fresh chalk suggests someone still visits. Choices are to memorize the marks or take chalk and stamp. The second choice text explicitly frames forgery as a possibility. Do not convert this implication into a guaranteed disguise system or faction penalty without an owner.

**Optional observation line:** “Someone rubbed out a circle and wrote it again darker. The board remembers only the second hand.”

**Follow-up question for a records keeper:** “Does square mean paid?”
Answer: “On this board. That is what the chalk says. Whether the collector believes it tomorrow is another matter.”

The answer distinguishes authored board semantics from future enforcement. Keep it local, not a universal rule for every levy or faction.

### Writing and graph constraints

All prose variants must preserve the current authored choice IDs, rewards, depletion behavior, and local stakes unless a later data change is separately reviewed. For graph-based follow-up, use short branches that reconverge after one optional question. Provide an accessible no-dialogue fallback that leaves the original encounter complete. Never block expedition return or journal access on reading every line. Speaker IDs and location tags must be mapped to existing entities and host routes; the samples above use role labels until that audit is complete.

**Reusability:** the three records form a micro-site writing rubric: physical detail, a careful claim, a second interpretation, and a consequence that the player can actually enact. Apply it to future sites without copying ledger/line/board imagery. Rotate registers: archival restraint, route-work pragmatism, and bureaucratic unease. **Production cost:** low for additive log copy; medium for three conditional follow-ups; high if new dialogue routing or persistent knowledge is required. Core content should keep current records stable. An expansion may add the optional lines after a duplicate/continuity audit.
## Pass 30 — Eight regions, eight ways to be uncertain

These DRAFT atlas passages use the eight authored region records without pretending their POI labels are canonical destinations. They may appear in an existing shelter archive/journal surface only if that surface already supports regional entries. Each frames hazards and landmarks as chart claims, not route availability or live forecasts.

### Atlas prose seeds

**Ash Valley Basin — wasteland.** The page draws a shallow basin in strokes that could be contour lines or old plow marks. It names a quarry and a radar station, then circles both in the same tired pencil. The note beside them says ash storms and radiation pockets. It does not say which comes first.

**Dead Coast Estuary — water.** A tide gate is drawn as a square with a hinge on the wrong side. The port ruins have no depth marks. Whoever copied the chart left space for the water to move, then wrote “corrosive fog” where a distance should have been.

**Ironspire Metropolis — urban.** The metro station and general hospital share one heavy underline. Structural collapse is written in block letters. “Sniper nest” appears smaller, in another hand, as if the second writer did not want the first to see.

**Missile Silo Grounds — industrial.** A silo and command bunker sit apart on the sheet. EMP residue and toxic slurry are listed below them with no source note. The paper is folded so the two hazards meet at the crease.

**Iron Ridge Escarpment — wasteland.** The mine shaft symbol is cut into the ridge line. The lookout is drawn on the other side, where the page thins. A note about rockslide and gale winds has been recopied until the letters look like rails.

**Verdant Impact Basin — rural.** A farming commune is marked beside a spring, but the blue ink has spread into the basin border. The map names mutated flora and spore drift as observations; it does not promise either can be seen from a route.

**Submerged Industrial Run — industrial.** The pump station and oil depot are inked below a waterline. Methane pocket and flash flooding are written as separate warnings. Someone added “listen before opening” in the margin, without saying what answered.

**Frozen Highland Gap — rural.** The crossing mark sits where the route narrows to a pencil scratch. The repeater is drawn beyond it, reachable only in the copyist’s imagination. Black ice and hypothermia gale share one warning box.

The prose is grounded in authored fields; none adds a route, hazard mechanic, faction, settlement, or destination. A specialist should review copy so catalog hazard labels are not shown as active forecasts.

### Hub scene: “Two Maps on the Table”

Archivist: “This one tells me what the land is called. That one tells me where a road has been walked.”
Expedition lead: “A name is not a route.”

Player responses:
- “Keep both sheets separate.” The archivist labels them “region chart” and “travel map.”
- “Find one mark we can verify.” The lead asks for a canonical route-node ID; the action remains unavailable until an approved crosswalk resolves it.
- “Leave the margin open.” They preserve uncertainty and return to the existing map panel.

Reconvergent line: “No one throws either sheet away. The table is wide enough for a known road and an honest blank.”

This is not a faction argument and does not alter standing. If the host has no regional chart surface, keep it as an atlas-page proposal rather than fabricating a map screen.

### Availability copy

- “Region recorded in the shelter chart; expedition route not yet verified.”
- “Travel node known; regional assignment not yet verified.”
- “Survey recorded through the canonical map.”
- “This note describes a chart label, not a guaranteed expedition destination.”

Only the third line can reflect canonical WastelandMap knowledge. The first two describe authored-source status and are not player-earned discoveries.

### Future regional scene template

After a node-to-region relation is approved, a location scene can contain an entry description, one arrival variation, one environmental detail, one authored clue, one choice with owner-routed effects, and one return callback. Keep registers distinct: Ash Valley plainness; coast erosion; urban compression; industrial procedure; ridge caution; rural ecological ambiguity; submerged quiet; highland cold. Give each region a different story question—who maintains the route, whose measurement is trusted, which warning should be published, what should remain unmarked, how scarcity changes passage, how a shelter remembers the survey, what a community owes, and when a dangerous shortcut should stay secret. Avoid eight identical “visit two points” quest scripts.

Localization and accessibility: use stable text keys, avoid critical information solely in colored map icons, keep readable contrast and subtitle length, and provide a written route-state equivalent. These are acceptance requirements for later work, not current UI claims.
### Pass 30B — Cartographer voice signatures and regional branching

The atlas should have a human voice without hiding data uncertainty. Three provisional shelter roles can carry the conversation, subject to an entity and dialogue-host audit.

**Archivist:** patient, exact, unwilling to complete a line from wishful thinking. “The chart gives the basin a name. It does not give me the road.” When pressed to guess: “I can leave a blank. I cannot send someone into it.”

**Expedition lead:** practical and accountable for returning people, not dismissive of scholarship. “A route is a promise made with mileage, weather, and whoever has to walk it.” If the chart lacks a node: “I’ll take the note. I won’t take the column there on a note alone.”

**Junior surveyor:** eager, embarrassed by uncertainty, learns to label confidence. “I thought the two marks meant two stops.” After the explanation: “Then I’ll write what I saw and what the page said in separate lines.”

These roles are not new canonical survivors or faction leaders. Bind them to existing people only after checking name, personality, availability, age, relationship, and voice. Otherwise use role labels in a non-dialogue archive page.

**Short reconvergent exchange:**

Archivist: “Which sheet should go on the wall?”
Player: “Both. One shows names; one shows roads.”
Expedition lead: “And the blank between them?”
Player: “Leave it until someone comes back with a line we can verify.”
Junior surveyor: “I can mark the margin as unknown.”

The final line is a proposed text choice, not a command that writes a new cartography state. If the journal owner supports source annotations, the note can be recorded there; otherwise it stays dialogue.

**Eight region-specific callbacks:** avoid repeating full descriptions on every visit. Ash Valley callback notices grit under a folded page; Dead Coast callback asks whether the tide mark has a date; Ironspire callback distinguishes a hospital label from a route; Missile Silo callback asks who wrote the hazard line; Iron Ridge callback folds the map along a contour; Verdant Basin callback separates a spring report from an ecological warning; Submerged Run callback leaves a silence after “listen”; Frozen Gap callback checks whether the repeater was heard or only drawn. Each callback is cosmetic unless a canonical owner provides new evidence.

**Branching design:** open archive hub, select one region card, ask one source question, then reconverge at a three-way “verified / reported / unknown” close. The graph remains playable with only one branch selected. A later return scene unlocks only on canonical map state, not prior dialogue. Do not persist every text choice as player memory. Keep regional hazard words accessible through captions and journal text, not color-only overlays.

## Pass 31 — The Trader's Manifest: Dialogue Corpus and Quest Scenes (DRAFT)

### Scene design

The prose layer should make differences in trade legible through people, not present a spreadsheet as a story. Existing TradeTellEngine already selects an original, data-defined line by trader stance and trust band; trade_tell_lines.json has terse posture pools, and the trade screen renders a selected stance tell. That owner is suited to short negotiation posture, not a complete branching quest dialogue graph. Keep the current tell plate intact and use the existing authored dialogue/quest graph contract for longer scenes if one is proven. Do not add per-caravan dialogue state to the panel.

A small initial scene set can carry the theme:
- Arrival: a guard unhooks a tarp, pauses over an empty lash point, then asks the shelter's quartermaster to count what is present rather than what was promised. This is an opening image, not proof of a specific cargo loss.
- Manifest: the trader places a damp route sheet under a tin cup. The player can ask what was expected, what arrived, or who else has seen the list. Responses distinguish witnessed inventory from memory and rumor.
- Specialty exchange: the trader explains a locally made good in terms of labor and repairability. The player may disclose a matching need, negotiate, or decline. Avoid claiming a “settlement specialty” until such a specialty is represented by a canonical field or explicit authored text with an owner.
- Departure: if the player acted, the trader leaves a practical instruction or warning tied to the actual chosen branch. If the window was missed, the remaining line acknowledges the absence without inventing a promised return date.

Sample draft lines:
- Trader, neutral: “Count what made it here. The paper kept traveling after the crates stopped.”
- Quartermaster, guarded: “A name on a route sheet isn't a sack in the storeroom.”
- Guard, warm: “We still carry the tools that can be mended on the road. That is not the same as carrying enough.”
- Trader after a failed window: “You came back to the same table. I came back with a different load.”
These are new prose candidates, not canonical shipped lines. Their claims remain deliberately non-specific; route- and cargo-dependent dialogue needs a live data binding before it can be made exact.

Dialogue structure: hub-and-spoke at the manifest, then short branches that reconverge at the offer; relationship tone can vary by existing trust/stance bands; knowledge gates reveal only clues already witnessed; faction-conditioned choices require an existing standing/access query. Repeated visits may change one greeting or reveal a callback only if the dialogue system has canonical visit memory. Quest effects belong to the quest owner; market transactions belong to the trade owner; relationship and faction consequences belong to their current authorities. Text nodes should not mutate inventory directly.

Voice palette:
- Trader: economical clauses, prices and routes as daily burdens, rarely claims certainty about distant stops.
- Quartermaster: concrete nouns, exact quantities when visible, distrusts promises without a manifest.
- Escort: sensory observation, wind, wheel noise, watch rotation; not an omniscient narrator.
- Local producer: explains time and technique, pride tempered by scarcity, no heroic “chosen settlement” framing.
- Player response options: readable intent labels (“Ask what was lost,” “Offer the stock we can spare,” “Keep the route private”) so the chosen consequence is legible.

Corpus tiers:
1. Reusable stance tells: existing posture pool.
2. Scene entrance/exit copy: brief authored scene text with no state mutation.
3. Quest nodes: conditions, responses, and consequence IDs governed by the chosen dialogue/quest schema.
4. Observation text: manifest/cargo descriptions generated only from a confirmed visible inventory projection.
5. Diegetic journal entry: written only after a canonical quest transition, with observed facts and attribution (“the trader says…”) clearly distinguished.

Use a line matrix before authoring volume: speaker × scene × trust band × knowledge state × result. Write short but distinct cells; omit unsupported combinations and fall back to restrained neutral copy. Avoid mechanically substituting faction name, region name, or good ID into every line: grammatical and canon review should treat rendered variants as content, not template tokens. Do not copy a line from TradeTellEngine into a new quest corpus merely to increase volume.

Production target: 1 opening scene, 1 manifest hub, 3 optional questions, 2 reconverging offer branches, 3 outcomes, 1 missed-window closeout, and no more than 20 reusable one-sentence posture/exit variants. Localization keys, speaker attribution, accessibility reading order, and dialogue graph validation are part of cost. Success is measured by branch clarity and factual safety, not count of words or variants.


### Pass 31B — Extended sample with attribution-safe branches

Trader: “You came to ask what the road kept.”
Player response intents:
- “Show me what the manifest says.” Opens the authored route-demand and export-surplus projection only if the current screen can show that source.
- “Show me what arrived.” Reads current inventory; absent goods are not listed.
- “What do you need from us?” Uses current demand/offer contract if available; otherwise opens a noncommittal conversation, not a fabricated shopping list.
- “We have nothing to spare.” Ends the trade branch without penalty or forced disclosure.

If the player asks about a missing lot, the trader can say, “It was on the paper when we left.” That is a reported claim. A follow-up may ask who handled the cargo, but should not branch into theft, sabotage, or faction blame until a narrative encounter or event supports that cause. If the cargo appears, a conditional line can acknowledge the visible item without claiming the origin beyond the catalog.

On the local specialty branch, the producer demonstrates a repair method with an already authored tool. The camera/text focuses on the hands, the worn jaw of the wrench, and the relief of a part that fits. This beat can be reused as a short scene even when the player declines to trade. A craft unlock is separate content and must be omitted unless the recipe owner records it.

After a guarded agreement: “No promise of a full load,” the trader says. “Only that I’ll ask before it leaves.” This is safe only if the durable agreement is no more than an in-scene intention; do not represent it as a future shipment obligation. The journal can record, “The trader agreed to ask about local stock before departure,” only after the selected branch was applied and the quest owner exposes that completion fact.

Authoring and voice QA: each response label should predict player intent; each line must retain attribution when spoken information is uncertain; faction names use canonical IDs only at data boundaries and display names in prose; optional prose should not repeat the full inventory table; no outcome promises an unimplemented discount, destination, faction shift, or new recipe. Track which lines are original samples, which are conditional variants, and which are existing corpus text. This is important for both provenance and localization.

## Pass 32A — Maintenance Voices, Record Fragments, and Scene Writing (DRAFT)

### Creative direction

Part 47's loop-closure requirement suggests a narrative sequence that begins with an audible physical detail, passes through conflicting records and human interpretation, and ends with a maintained or deliberately unresolved handoff. Tone remains grounded: no machine becomes a mystical oracle, no technician is a disposable exposition channel, and no log proves sabotage without a supporting event. Machine “personality” comes from behavior and the people who work beside it; diagnostics stay mechanically legible.

Character palette:
- Irena Vale, night mechanic: listens for rhythm and records intervals rather than diagnosing causes. Patient, economical speech; bristles when an unverified report is called carelessness. Draft role only until a real survivor/character ID is assigned.
- Tomas Rill, day electrician: prefers measured readings and signed work orders; will admit a reading was taken after the sound stopped. He is methodical, not cold.
- Nadiya Sorn, shift lead: tracks risk and staffing pressure, worries that a machine outage will be blamed on the person who reported it. She asks for a decision and a deadline, not a culprit.
- The player-character: response labels should permit skepticism, care, urgency, and privacy, each with a visible conversational consequence. No hidden personality score is inferred from one option.

Opening scene draft: “The third knock came after the belt stopped. Vale held one palm above the housing, feeling for the warmth that used to arrive a breath earlier. She did not touch it. On the board behind her, somebody had written RUNNING CLEAR in chalk and circled the word twice.”

This opening conveys a sensory report but does not assert a failure or change a state. The scene can be presented only if the diagnostic/quest trigger exists; otherwise the sentence belongs in a static story sample and must not appear as a live incident.

### Record fragments and competing interpretations

Draft authored document A, technician note: “Night shift, two short knocks on start-up. Third knock after load settles. No visible belt slip. Waited for the housing to cool before checking the guard. Did not open the cover; the line was still carrying load.” Its purpose is to model safe observation and uncertainty, not to create a repair recipe.

Draft authored document B, earlier log excerpt: “Housing warm at end of shift. Crew called it expected friction. No temperature written down.” This record lacks a numeric reading by design. It is an incomplete historical account, not proof of neglect.

Draft dialogue:
- Vale: “I wrote down when it happened. I couldn't tell you why.”
- Rill: “The gauge was steady when I checked. That tells you about the minute I checked.”
- Sorn: “If we stop it, write why. If we keep it running, write that too.”
- Player (support): “Keep both notes. No name on the conclusion until we check.”
- Player (urgent): “I want the inspection window now. Tell me what it will take offline.”
- Player (skeptical): “The record and the reading disagree. Show me the dates.”
- Player (privacy): “Keep the report in the service file, not on the public board.” This option only has a lasting privacy effect if an existing document/access authority supports it.

Outcome prose must remain result-backed:
- Corroborated concern: “The log was not a diagnosis. It was enough to earn a second look.”
- No present anomaly: “The sound did not return during the check. Vale left the note where the next shift could find it.”
- Deferred: “The inspection waited for a safe window. The reason was written beside the date.”
These endings do not imply that the machine was repaired, that the sound has permanently stopped, or that staff trust changed mechanically.

### Location and object description fields

The machine-room description should orient the player: safe approach point, visible isolation control (if it exists), sound source, diagnostic display, and whether interaction is observational or mutating. Do not write that the player can open a guard, shut off a line, or replace a belt unless a current action exists. The maintenance archive description can identify a card index, page date, missing signature, and source catalog, but exact days should come from the authored entry. A room's flavor description should remain separate from dynamic machine status so it never shows “damaged” when the owner says normal.

Branch writing uses short branches that reconverge. Hub: ask about timing, ask about prior work, choose whether to request inspection. Each side branch reveals one source with attribution, then returns to the decision. If the player refuses, the scene respects that choice and leaves the task in its actual supported active/blocked state. No loop of redundant dialogue nodes to simulate character depth.

### Corpus expansion tiers

Tier 1: reuse current diagnostic tells and machine identity descriptions; author nothing new.
Tier 2: add one reusable handoff scene with three tone variants, all keyed to supported trust/relationship inputs.
Tier 3: write a pair of maintenance notes for an exact machine family after verifying an unmatched corpus gap.
Tier 4: add a quest graph with record provenance, fail-forward outcomes, and a visible result receipt.
Tier 5: add recurring maintenance-story callbacks only after quest/Chronicle owners can supply a verified prior fact.

Authoring review checks factual scope, tradeoffs, character voice, safe work procedures, temporal continuity, state-conditioned variants, localization readiness, and whether each line is diegetic report or omniscient narrator text. No copied sentences from current logs or source texts should be reused; sample prose above is original proposal copy. High-volume writing should follow a coverage matrix of actual machine families, not generate near-duplicate “rattle / hiss / hum” descriptions for the sake of count.


### Pass 32B — Extended branch sample and implementable copy packet

**Hub: Vale beside the service board**
- Vale: “I put the time beside the sound. That's all I know.”
- Player: “Show me the note.” Effect: opens the exact discovered record; no mutation.
- Player: “What did the machine owner report?” Effect: requests a fresh, supported reading; if unavailable, the option returns a neutral unknown message.
- Player: “Who last worked on it?” Effect: opens a witness branch only if a source record names that person or a current character availability query confirms it.
- Player: “Leave this until the service window.” Effect: records a deferral only if the current quest owner accepts that transition.

**Record panel description:** “The card is folded across the date. A pencil line marks the shift, then stops before the signature box. The handwriting changes on the second line.” It implies incompleteness without claiming falsification.

**Fresh reading panel:** “The display shows the reading taken now. Vale's note belongs to another shift. The two marks can sit together without one erasing the other.” This copy is valid only if the display has a timestamp or the host can truthfully call the value current.

**Inspection request:** “Rill checks the schedule before he answers. ‘I can look when the belt is cold. I won't open it under load.’” This is a character line; the player action still requires an actual supported schedule and safe command.

**Outcome: no issue reproduced:** “The third knock did not return during the check. Vale signed the note as heard, not solved.” The result is an observation, not a permanent system improvement.
**Outcome: follow-up accepted:** “Rill added a second line to the card. The space for a cause stayed blank.” This is safe if the actual owner records the action or the passage is clearly authored scene closure.
**Outcome: deferred:** “The shift wrote down why it waited. The machine kept its own answer.” This avoids implying harm or repair.

Potential side quest opener: a young apprentice asks why the older technician writes “heard” instead of “broken.” The player can explain evidence discipline, ask the mechanic to answer, or move on. It is educational character content and should not gate a required repair. Another optional thread follows a missing carbon copy from a maintenance ledger. It can be a quiet document search, not a theft plot; absence of a copy must not automatically imply sabotage.

For high-volume prose, populate distinct registers rather than multiplying synonyms: (a) technician's measured note; (b) shift lead's concise task board; (c) apprentice's questions; (d) player journal's source-attributed summary; (e) maintenance-room environmental description. Limit each micro-record to one salient physical observation, one constraint, and one uncertainty. Require an editor to tag each sentence's state source and intended time. A reusable prose template can provide ordering but should not create variable interpolation that forms unreviewed sentences at runtime.

Production cost matrix: opening scene low; two log fragments low; character relationship branches medium; dynamic fresh-reading line medium due snapshot API; repair result UI high because actual command receipts and accessibility states are required; multi-machine voice/canon review high. Core content should teach that a tell is evidence to inspect, while optional expansion can provide 3–5 related shift stories and a later chronicle callback. Localization, text-to-speech order, controller focus, and screen-reader attribution are included in UI writing cost.


### Pass 32C — Reusable scene packet and writing cadence

A maintainable scene packet contains a one-line setup, optional object/location description, two to four speaker lines, two to four player intent responses, result-backed outcomes, and an optional journal summary. Reuse four scene functions: report an observation, compare records, negotiate a service window, or hand off an unresolved task. Do not multiply adjective variants where a source or consumer is missing.

Original copy seeds:
- Workbench: “Two sockets are missing from the row. A wrench has been left in their place, its handle wrapped in cloth.”
- Shift board: “Inspect after the line stops. Keep the west feed active. Write down why if either instruction changes.”
- Apprentice: “If it stopped, why did you write heard instead of failed?”
- Mechanic: “Because I heard it. Someone else has to decide what it means.”
- Record footer: “Second reading requested. No replacement part entered on this sheet.”
These are DRAFT and must appear only where the object or action exists. They do not assert an inventory shortage or a completed repair.

Voice callbacks can deepen character without a new personality system: Vale records timing; Rill supplies the method and date; Sorn asks for the operational cost; the apprentice asks concrete evidence questions. A callback requires a supported relationship or quest fact; otherwise use a neutral greeting. Machine-room copy should describe safe approach, visible panel, sound, and interaction type. Never invent a button, isolation control, repair instruction, or status that the owner does not expose.

For larger prose batches, choose distinct source registers—technician note, task board, apprentice question, player journal summary, environmental description. Each piece needs one observation, one constraint, and one uncertainty. Tag every sentence with source class and time. Core writing teaches “tell is evidence, not diagnosis”; optional expansion can add several shift stories and a later Chronicle callback when its owner accepts a durable fact.


### Pass 32D — Prose lint and accessibility requirements

Before a line enters a data catalog, tag speaker, source, tense, certainty, location, gate, consequence, and localization context. A line that says “the machine is failing” requires a live authoritative result; “Vale thinks the sound is changing” requires an attributed report; “the old log records a warm housing” requires a dated authored source. Reject lines that blur these categories. Keep response intent labels shorter than body copy and make costs visible before confirmation.

Read order must introduce speaker and source before a long record excerpt. Avoid color-only diagnostic descriptions; pair status color with text and a meaningful icon/label. Controller focus should land on the first valid response and remain usable after a stale-context refresh. Accessibility review is part of dialogue production, not a polish-only expansion phase.

## Pass 33A — Water Ledger Story, Character Voices, and Location Copy (DRAFT)

### Story packet — “The Line Under the Number”

The shelter board has thirty tick marks, one for each day on the planning sheet. Beside them sit two containers: a ration inventory tally in units and a source estimate written in liters/day. The disagreement is not that one character is lying. They are answering different questions. A quartermaster counts what the ration owner removes; a hydrogeologist counts what a source could provide before treatment and loss; a medic watches thirst in each survivor; a greenhouse worker knows how much one action can draw. The player's role is to ask what each number means before committing the reserve.

Quartermaster Leena Marku is exact about inventory and avoids translating the count into “days of water” without knowing future ration policy. She says: “Three is the daily request on this board. It is not a promise that three are left.”
Hydrogeologist Emil Sato speaks in ranges and qualifiers: “The well can move that volume when the source and pump agree. Potable output is a second question.”
Medic Arvo Neri observes people rather than tanks: “Thirst rises in each person. The preset changes how quickly. I cannot read that as liters.”
Greenhouse keeper Sima Radu focuses on competing use: “The plants do not drink by policy. They ask when the cycle runs.”
These are proposed characters pending valid character IDs and owner access; do not bind names to existing survivors by similarity alone.

### Dialogue hub and branch copy

Hub: “Which number should we put first?”
- “Count the ration requests.” Show the three constant-policy scenarios and label them inventory units across thirty days.
- “Show me thirst pressure.” Show difficulty multipliers and, only if a read-only projection exists, survivor need trajectories.
- “Count the source.” Show nominal source flow with a visible “authored capacity, not treated output” label.
- “List the other draws.” Show only actions and jobs confirmed in the current scenario; mark unknown consumers.
Each branch returns to the hub and stores no state until the player selects a supported action.

Original copy:
- Board description: “The chalk line reaches day thirty, then stops. Under it, the quartermaster has written UNITS. Someone else added LITERS in smaller letters, without crossing the first word out.”
- Inventory row help: “Stock is counted in the item units used by the ration owner.”
- Needs row help: “This meter describes a survivor's thirst state. It is not a volume.”
- Source card: “Nominal flow from the source record. Current contamination, equipment, and treatment may reduce what reaches storage.”
- Unknown draw: “No scheduled amount is included for this activity.”
- Closing line: “They left the words units and liters on the board. This time nobody erased either.”

No line says water “runs out on day X” until a full scenario computation supports that forecast. Avoid language that turns a model estimate into a prophecy. A player who chooses irradiated supplement should see the separate irradiation risk/medical consequence contract, not a clean-water savings line alone.

### Side stories and reusable high-volume fields

1. “The Cup With Two Measures”: a small character quest about a marked cup used for rationing versus testing. It adds a source-attributed discovery and a reconciliation choice.
2. “Rain on the Roof Sheet”: discovery of a rain-catchment source record and discussion of contamination. It does not declare the collector operational.
3. “Three Nights on Half”: a survival quest reads an existing ration-policy history if available; absent history, it remains a scenario, not a memory claim.
4. “The Seed Tray's Share”: a greenhouse worker requests water through a current action. If the command is unavailable, the scene is only a planning conversation.
5. “A Test Before the Thaw”: investigation of a water-test result with source, day, tester, measured contamination, and confidence, if an existing test owner supplies those values.
6. “The Dry Margin”: optional journal prose on the social choices around reserves, not a new morale score.

Reusable scene fields: speaker role and valid identity; scene context; source IDs; observation date; unit label; certainty; known/unknown status; player response intent; consequence owner; result wording; fallback text; localization/accessibility note. Vary the underlying question, not just the name of the container. Short fragments should focus on the tangible work of measuring, carrying, cleaning, waiting, and choosing who receives a reserve. Keep engineering details readable and avoid real-world procedural instructions that could be unsafe.


### Pass 33B — Branching conversation: ration board and water test

**Scene: the planning table.** The quartermaster has one column labeled STOCK and another labeled REQUEST. The medic places a survivor roster beside it. The water technician brings a source card with flow and contamination fields. The player can inspect each item in any order; no branch is mandatory before the comparison.

Quartermaster: “I can tell you what the ration owner asks us to set aside. I can't promise the shelf has it.”
Medic: “The thirst reading belongs to a person. It rises on its own clock.”
Technician: “This card gives the source's declared flow. The sample tells us something else.”
Player options reconverge:
- “Keep units, need, and liters separate.”
- “Show the current roster and policy.”
- “What has actually been tested?”
- “End the review without a forecast.”

If the player asks for a total and an owner field is unknown, the NPC should refuse the false precision: “Not until we know what the greenhouse cycle actually drew.” If all modeled inputs are known and the read-only projection returns a valid forecast, replace that line with a source-attributed summary of the returned range. Do not author a static day-of-exhaustion message.

A test branch names source ID, sample day, tester, contamination result, and accuracy only if each value exists in WaterSourceSystem's persisted test result. A stale result is still a historical test; it is not “the water today.” Dialogue can say “Last tested on day X” rather than declaring safe or unsafe now. Test interpretations should be written for uncertainty and not turn a raw contamination number into a medical diagnosis without the actual quality protocol.

Optional character story: Leena inherited a ration board from a previous quartermaster whose unit labels were rubbed away by wet sleeves. She can choose to rewrite the labels or keep the old marks as history. The choice can alter only a supported document or a local scene; it should not change stock or ration policy. A second arc follows a water tester who signed a report while the pump was offline. The player can preserve the report, request a repeat test, or note the limitation. The story is about conditions of measurement, not misconduct.

Location prose: “The source shed smells of wet stone and hot wire. The flow gauge taps once when the pump catches. A paper test strip is pinned under glass, its date facing outward.” Use only if the actual location, pump, gauge, and record exist. In a general map description, omit dynamic flow language. Player-facing copy should differentiate “potential source,” “active,” “sampled,” “test current,” and “quality unknown.”

Endings:
- Reconciled ledger: both unit sets remain and the assumption is stated.
- Partial ledger: known policy debit is documented, other demand unknown.
- Disputed source: sample result is attributed and another test is suggested.
- Declined: the player closes the review without changing campaign state.
The journal summary always records its snapshot day and whether it is a scenario or actual history.


### Pass 33C — Story fragments, shelter lore, and branching closeout

**The chalk board's history:** “Day 4: clean stock counted. Day 11: policy changed. Day 18: names added in the margin. Day 30: no total entered.” This is a sample authored object, not a claim about campaign events. If used as a static prop, label its chronology as authored fiction. If used as a live scenario, fill dates only from canonical campaign records.

**Shelter lore:** older residents teach that a cup with a blue thread belongs to the tester, not the person drinking. A child has tied the thread to a cracked handle; an apprentice thinks the mark means “safe.” The story asks whether to teach the difference between a sampling vessel and a ration cup. Keep folklore human and practical; do not let a superstition override a water-quality result or confer a mechanical blessing.

**Quest-only dialogue branches:**
- If player knows only the source card: technician explains nominal flow versus tested quality.
- If player has a current test: medic asks for the source/day and reads only supported findings.
- If player has an old test: quartermaster marks it as historical and asks whether to retest.
- If player has no source discovery: characters speak about the shelter inventory and do not name an undiscovered site.
- If budget projection is incomplete: close with the known policy debit and list the unknown draw in plain words.
These branches reconverge on the choice to act, defer, or preserve an uncertain report.

**Location description — rain catchment walk:** “The gutters lean toward a tank that has not yet been opened. A strip of old paint marks the route of water down the wall. The inspection card hangs under the eave, face turned away from the rain.” Use only if this location and object exist in the shipped world. It does not claim the catchment is active.

**Faction story seed — Shared Measure:** a neighboring group proposes exchanging a test kit for an agreed sample record. The player can share method, share only the result, refuse, or ask for a reciprocal test. This is a trade/diplomacy quest seed, not a guaranteed treaty or faction-standing change. It belongs in an expansion unless the current core campaign already has a supported inter-settlement sample exchange.

Writing production should vary not only voices but stakes: domestic allocation, clinical observation, technical yield, ecological use, and diplomatic trust. A reusable branch is safe when its conditions correspond to actual known states. Text for “clean” and “unsafe” quality requires the exact WaterQualityProfile or source-test interpretation contract; otherwise write “result recorded” and let a panel show the source-backed measurement.


### Pass 33D — Radio, journal, and environmental prose variants

Radio fragment, attributed to the shelter clerk: “The board says ninety requests under the same policy. It says nothing about the pump.” This may be used only for the constant Standard-policy arithmetic and should be tagged as an authored explanation, not an in-world broadcast unless the radio owner supports scheduling and delivery.

Journal closeout for an incomplete model: “The thirty-day sheet compares the ration request with the roster's thirst settings. It does not reconcile source flow with treated storage. No reserve was changed.” This is a concise factual summary and should be generated only from confirmed scenario inputs.

Environmental notice: “Keep samples off the ration shelf. Put the source ID and day on the jar before it leaves the room.” Use as fictional workflow only if sample handling is actually represented; otherwise write as a static text clue, not a command prompt.

Two alternate character closes:
- Quartermaster: “We can work with a number that admits what it leaves out.”
- Medic: “A thirst reading tells me who needs help. It doesn't tell me what the pump made.”
These lines teach the distinction without making any character omniscient. Each prose variant should have a source-state tag, exact display location, and neutral fallback if the relevant report is missing. Keep radio, codex, quest dialogue, and UI tutorial copy in their actual owning corpora; do not clone lines across catalogs merely to increase the content count.


### Pass 33E — Quiet character callbacks

After a later review, Leena may say, “We kept the old sheet behind the new one.” The line should appear only when the earlier receipt exists. Sato can ask whether anyone retested the source; Neri can ask which survivor's thirst changed; Sima can point to a crop cycle only if its owner reports one. These callbacks keep each voice attached to its evidence and avoid a generic narrator summarizing unknown system state.

## Pass 34A — Sample Dialogue: The Return Column

### Scene contract

This is an authored hub-and-spoke scene for the shelter notice desk after a party returns without completing its objective. It is available only when current campaign evidence confirms a failed objective and the returning survivors are available to speak. It must not show in the generic expedition-failure callback when no one returned. The scene routes back to the quest record in Plan 17 and uses the location/fallback decision in Plan 18. The scene itself does not mutate expedition state or write reputation directly.

### First conversation

Clerk Vesta: “The sheet says returned. That is what it measures.”

Rell, field recorder: “It measures boots on this side of the gate. The relay stayed dark.”

Vesta: “Then give me the sentence you can prove.”

Player responses:

1. “Write that the party returned and the objective failed. Leave the reason open.” This is a local scene effect and advances the quest only if the existing quest authority accepts a neutral report. It records no inferred sabotage.
2. “Rell carried the empty key case. Let them testify.” This is knowledge-gated: it is available only when that item/result evidence is in the current run. It opens a short reconverging branch in which Rell explains the observed chain of events and the player chooses whether to name the missing relay key.
3. “The team ran.” This is a public-account choice, not a truth claim. It may change the notice wording. Any standing or relationship change requires a current consequence route and authored consequence ID; without one, the branch is cosmetic and must be labeled that way in implementation notes.
4. “Hold the notice until another witness is found.” This begins the follow-up quest route. If there is no valid destination, the dialogue offers a shelter interview or an explicit delay instead of inventing map content.

### Reconverging testimony branch

Rell: “I can tell you what my hands carried. The box was empty when we crossed back.”

Player: “Did you see the relay fail?”

Rell: “I saw the lamp go out. I did not see who touched the switch.”

This branch models the difference between observation and accusation. A skill check, if the current dialogue framework exposes one, can unlock a follow-up question about signal timing; it cannot grant truth that the witness did not observe. A failed skill check still returns to the choice hub with a plain explanation and an accessible route to inspect the map note. Do not hard-fail the quest because the player did not invest in a skill.

### Craft and voice notes

Vesta’s speech is clipped, administrative, and protective of records because inaccurate notices have previously put people at risk. Rell is concrete, reluctant to overstate, and uncomfortable when the player asks them to name a culprit. Neither character gives a lore lecture. Repeated visits should acknowledge whether the notice was corrected, postponed, or published; do not replay the opening scene verbatim after state changes. If a survivor is unavailable due to a real roster state, use a written statement only when the game has an authored record for that person. Never synthesize a letter from an absent character.

### Production breakdown

The first playable slice requires one hub scene, four responses, one optional two-node testimony branch, two reconverging nodes, and three state-dependent closing lines. The build review must count every node reachable in each quest state, check response focus order and keyboard/controller navigation, and verify that visible wording names the consequence scope. Voice, localization, and UI copy must fit the compact conversation panel without clipping. These sample lines are original draft prose, not shipped canon. They should be reviewed alongside the recurring-speaker voice block and the project’s restraint/tone rules.

### Extended branch samples and character distinctions

When Vesta is asked why she prepared the notice before hearing the party, she answers: “Because people ask before the ink dries. I wanted a sentence ready that could not blame the dead.” This line can appear only if the scene has already established that no party member was reported dead. It reveals her concern without asserting that anyone died. If asked whether the relay failed, she says: “I file what arrives. I do not repair a silence by naming its cause.” That is a voice signature: procedural care, not cold indifference.

Rell’s optional line after the player chooses a neutral report: “Write the lamp. Leave the hand that touched it blank.” This line should appear once, after the player has heard Rell’s testimony and chosen not to accuse. On a revisit, do not repeat it; use a short acknowledgment such as “The corrected copy is on the board.” Repetition state must come from a persisted choice or a quest state, not a conversation-local boolean that resets on reload.

A third voice, Mara, a shelter runner who learned of the failed objective from another household, provides the public-account perspective. She says: “The market says the team came home with the key.” If the player has not discovered this rumor, this line is not eligible. If they have, Mara must be identified as repeating hearsay rather than witnessing the expedition. Responses can ask who told her, correct the specific claim, or decline to discuss the crew. The “who told her” response yields a clue only if the rumor source system or authored encounter supplies a source identity. Otherwise it remains a conversation choice with no hidden source invented.

For a faction branch, the authorized speaker might frame the failure as a breach of route discipline. Their exact identity and faction membership require source verification. Keep this branch short: one greeting, one contested phrasing, two player responses, and a reconvergent exit. The player can accept a factual route deviation if the expedition result verifies it, or reject the speaker’s added motive. Neither response rewrites the outcome record. A faction-specific line may affect future access only through a supported standing gate; it may not serve as an undocumented soft lock.

### Reusable dialogue components

Reuse a small, typed set of nodes: EntryNode presents context; EvidenceNode names the source and confidence; ChoiceHub offers a closed set; ClarificationNode explains a gated concept; ConsequencePreviewNode describes the likely scope in player-facing language; TerminalNode returns to the owning quest. These are authoring patterns, not new runtime classes unless the current dialogue framework demonstrably lacks equivalent node types. Linear scenes work for debriefs; hub-and-spoke for the report choice; short reconverging branches for testimony; relationship and reputation gates only when read-only contracts exist; repeated-visit lines derive from recorded actions; failure reactions mention the specific failed route; skill gates add information without blocking completion.

Each response should carry an author-only intent note, its mechanical effect reference or “cosmetic only” marker, and its return node. Avoid choice labels that imply a stronger effect than the owner can deliver. “Correct the record” can update a report; “clear the crew’s name” implies a broader social outcome that needs evidence and multiple systems. Provide concise labels for the UI and full response text for narration separately when the current schema supports it.

### Side-story scene cards and location texture

At loc_municipal_archive, if the destination is verified to support an existing archive interaction, the player may find two versions of a report notice with different dates. The text should not name the author until a document field proves it. Sample environmental copy: “The carbon sheet kept the pressure of the first signature. The second had been written after the lamps went out.” This is atmosphere and a clue to inspect the timestamp, not proof of deliberate falsification. If the archive site has no suitable current interaction, retain the line as a future authoring card and do not attach a new destination object by assumption.

At loc_radio_relay_mast, a possible short scene asks what the player listens for: carrier tone, recorded call sign, or mechanical relay click. Each response should route to an observation appropriate to current audio/evidence systems. The scene can instead be a text encounter if the actual audio apparatus is not wired. Sample line: “The mast turns in the wind. No voice comes back. The instrument still marks the interval.” The wording avoids claiming whether the silence is technical, human, or environmental.

At the shelter, Vesta has a second side scene after publication. “I moved the first sheet behind the corrected one. I did not burn it.” The line communicates that correction does not erase history. If the player had chosen accusation, Vesta’s wording can be firmer: “The board remembers what we printed, and who asked us to print it.” Neither line guarantees faction or relationship change. They expose consequence and invite a later quest.

Each location card needs a purpose, availability condition, intended evidence, map visibility, authored description, encounter cost, fallback, and reason for reusability. A location is not made quest-only merely by attaching a quest marker. If a site is intended to appear only for this quest, it requires a real content record and selection contract. Prefer known locations and optional discovery where possible. Avoid loading the campaign with map markers that disappear without explanation.

### Release rubric

A dialogue sample is ready for implementation only when all player options have labels, all labels reflect their actual effects, and every gate has a plain-language explanation or a safe hidden fallback. Review scenes in context of the panel’s available space and focus model; line count is not a substitute for readable turn-taking. Use full sentences for localized variants, preserve silence as a valid response, and keep the characters distinct through what they refuse to claim as much as through vocabulary. The narrative director should sign off on Vesta’s procedural care, Rell’s eyewitness restraint, and Mara’s explicitly second-hand rumor before the story is treated as canon. The first slice can ship with fewer lines if every state is legible and no line asserts unseen facts.

## Pass 35A — Sample Scene: The Order on the Gate Has No Witness

### Entry scene at the Toll House

This scene is conditional on an existing player-accessible route to loc_toll_house and on a verified notice or witness that refers to the currently active doctrine. The location name is an authored candidate in the Warlord territory catalog; dispatchability and encounter reachability still need source review. If the player has only heard a general broadcast, this is not the correct entry scene. Use the ordinary quest hub until a real lead exists.

Nera, a road clerk: “They posted a new number before dawn. The driver says it was different when he crossed.”

Ivo, a wagon mender: “I saw the chalk. I did not see the hand that wrote it.”

Nera: “The order has a seal. The second copy has a signature. That is not the same thing.”

Player options:

1. “Show me both copies and tell me where each came from.” Opens the evidence node. It is available only if both sources are actually present in the scene or in a delivered record.
2. “Who collected the charge?” Asks for an observed action. If neither witness saw collection, the response says so plainly and does not guess.
3. “The doctrine changed. That settles it.” This is a player assertion, not a fact. The scene asks whether the player means “the rule was announced” or “the rule was enforced.” If the system cannot store that distinction, keep it as a local dialogue branch.
4. “I will not put either name on the public notice.” Preserves anonymity where the current consent/content contract allows it and offers an unresolved close.

### Evidence node and reconvergence

Nera: “The first paper came over the radio desk. No mark from the gate.”

Ivo: “Mine came from a driver who paid. I saw the receipt when he asked me to mend the strap.”

Player: “Did you see the collector?”

Ivo: “No. I saw the receipt. It says where he went after.”

The scene can only call the receipt firsthand evidence if the player actually obtains or inspects the record. Otherwise, Ivo’s statement is hearsay about a physical document and needs its own provenance. A successful skill option might identify an inconsistent date or seal only if the game already exposes an applicable skill check and document inspection consumer. A failed check does not block the unresolved route; the player can record that the documents differ and leave the cause open.

After the evidence node, all branches reconverge on the notice choice: record an announcement, record a witnessed act, or mark the claim unresolved. The choices should preview scope: “journal this source,” “publish the witnessed event” (only if a supported public-account command exists), or “close without a verdict.” Do not label a choice “expose the Warlords” unless it truly affects public knowledge and faction consequence owners.

### Voice, tone, and recurring visits

Nera protects chain-of-custody and resents pressure to convert forms into proof. Ivo is practical and precise; he will say “I saw the receipt,” never “I know who took the charge” without direct evidence. A Warlord clerk’s voice should express a coherent institutional rationale rather than generic menace. The player can disagree with the policy without the speaker becoming a caricature. The story should not borrow real-world faction names or recognizable historical propaganda.

On return after a neutral close, Nera says: “The notice stayed open. The blanks are honest.” After an authored confirmation, she says: “The board names the action and the source. I made the source line larger.” These are state variants only if corresponding campaign facts exist. Do not replay the entire scene on each visit. All variants require a concise UI label, localization key, voice context, and response route. The first playable script is a short hub-and-spoke conversation; optional radio and checkpoint scenes are expansion content after their actual consumers are verified.

### Three scene branches and a compact story arc

Scene one, “The Board at Dawn,” begins in the shelter only after the player receives a current notice. A clerk reads the date and stated rule without interpreting it. A driver reports a payment; a mechanic remembers a broken strap and a receipt, but did not see the transaction. The player chooses to trace the paper, ask what the driver witnessed, or leave the matter unresolved. The scene teaches the distinction between an order and its enforcement through character disagreement, not an exposition panel.

Scene two, “The Gate Keeps Its Own Copy,” is an optional location encounter at an audited Warlord checkpoint. A guard has one posted copy, a clerk has a register, and the player may compare them. None should reveal a hidden secret simply because the scene is at a gate. The guard’s personal line is: “I can tell you what I was told to do. I can’t tell you what every wagon paid.” The clerk’s line is: “The register has a column for collected. It has no column for refused.” These original lines support a design where a formal procedure omits lived experience. If the current event does not present both sources, the scene must be reduced accordingly.

Scene three, “After the Price,” occurs at the shelter after the player has selected an evidence outcome. A late-game faction representative asks whether the player’s record will be used to challenge a transition in doctrine. The player can share the verified event, share an unresolved account, or decline. This is an expansion scene, not a new voting or faction-governance feature. It can show a political consequence only if a live owner consumes one; otherwise its purpose is character and information continuity.

### Branching dialogue sample

Guard: “The order says what we are meant to collect.”

Player: “Did your post collect it?”

Guard: “Not from every cart that crossed.”

Player: “Which ones?”

Guard: “The ones written in the book. You can read the book if the clerk says yes.”

This is a knowledge-gated invitation. It does not prove that the book is accessible or that consent exists. A second branch can offer a public ledger review if that is a real interaction. If not, replace the line with “I can tell you what I saw.” Do not falsely present an NPC’s permission as the player’s access to a file.

Clerk: “The second copy has another figure.”

Player: “Which one is true?”

Clerk: “Those papers answer different questions. One names the order. One records a collection. The same hand could have written both.”

Player: “Then I’ll mark the difference and leave the cause open.”

Clerk: “That is a record I can keep.”

### Character arcs and late-game use

Nera’s arc concerns accountability without exposing vulnerable sources. Ivo’s arc is about whether a practical craft worker will accept being treated as a witness after making a repair. A faction clerk can be a late-game recurring character whose loyalty lies with procedure rather than doctrine leadership. Give each a distinctive vocabulary, fear, desire and boundary. Their dialogue changes after a player action only when the action is persisted and the character can plausibly learn it. Late-game scenes should reveal institutions through changed paperwork, access and speech, not introduce an omniscient narrator.

An endgame callback may quote a report’s verified summary or unresolved status, if the existing chronicle/ending input accepts that fact. Do not append the whole conversation to the ending. Keep the callback to a single sourced memory line or omit it. Core scope is one shelter scene and one proven action case; the checkpoint and late-game branches remain expansion options with separate production estimates.

### Side quest scenes and environmental prose

Side scene “The Blank Column” is an optional talk with the checkpoint register keeper. She shows a ruled page where the official form has columns for date, wagon, and amount, but no place for refusal. Sample environmental note: “The paper is soft at the corners. The empty column is darker where a thumb held the page flat.” This is an atmospheric clue, not proof that a payment was concealed. The player can ask whether refusals were recorded, compare the form with a delivered notice, or leave the entry unmarked. The keeper’s answer must match the actual data package; if no form record exists, this remains a prose candidate rather than a generated pickup.

Side scene “The Strap Mender’s Receipt” begins at a shelter workbench. Ivo says: “I took the wagon in after the crossing. The paper was folded around the broken buckle.” The player may inspect the receipt if a real item or document owner supports it, ask Ivo to read the date aloud, or record only that he remembers seeing a paper. A direct read can add a source record; testimony alone should remain a statement by Ivo. The choice to keep his name off the notice needs an explicit consent boundary.

Side scene “Frequency Without a Name” uses a radio operator. “The voice gave a rule and a road. It gave no collector’s name.” This scene is only available after the relevant broadcast has actually reached the player’s radio history. If a catalog key exists but no reception is recorded, use no line. The player can replay the broadcast, compare its date to the register, or leave. Replaying is not a second discovery and should not award duplicate journal state.

Environmental location descriptions should explain what the player can perceive now: a board partly covered by rain, a register tied to a post, a waiting line painted over an older line. They should not state that an order was enforced when the location state does not prove it. Variant fields can cover accessible, contested, evacuated, or retired location states only when the location owner selects those states. A location-only description cannot carry quest completion authority.

### Branch quality and UX delivery

A dialogue branch should be short enough for the conversation panel, but not so compressed that it hides the evidentiary distinction. Response labels can be concise (“Inspect the register,” “Ask what was witnessed,” “Leave the cause open”) while narration carries nuance. If a choice has an irreversible public effect, confirm its audience and show that effect in the option text. A neutral leave/exit response remains available in every scene. On narrow scaling or localization expansion, the graph should split long exposition into speaker turns rather than truncate it.

Content QA should sample every one of the six evidence outcomes with one line each and verify that the narrator never crosses the boundary. A read-aloud or screen-reader pass should identify speaker changes and choice purpose. Do not require voice audio to make the scene understandable. The first production slice can be text-only and remain complete.

### Branch bank: twelve playable scene beats

These scene beats are a high-volume authoring plan, not twelve guaranteed new quests. Each is independent and may be activated only after its evidence and consumer are verified.

1. **Notice arrives:** a radio operator identifies the sender and date, then lets the player choose whether to replay or file it.
2. **Old board uncovered:** a worker removes a newer paper and finds an older notice beneath it; the player may compare dates, not infer why it was hidden.
3. **Refusal remembered:** a driver reports refusing a charge; the report remains testimony until a second source confirms it.
4. **Receipt found:** a receipt is presented by its owner; inspection is a separate player action from hearing about it.
5. **Book unavailable:** a clerk says the register is sealed; the player can wait, ask for an authorized excerpt if supported, or close unresolved.
6. **Two signatures:** one signature is a posting authority, another a collector acknowledgement; the UI explains they do not prove the same event.
7. **Empty watch:** an abandoned checkpoint contains an unstaffed desk; it proves absence at the visit time only.
8. **Boundary argument:** two factions contest whether the road lies inside a toll zone; use the territorial owner for current boundaries and do not add a parallel map.
9. **Clerk’s correction:** an official acknowledges a copying error; a correction can alter the record only through the owning content or quest command.
10. **Witness withdraws:** a source retracts consent to be named; preserve privacy and let the player continue with an anonymous account if supported.
11. **Rule changes:** a later doctrine signal is delivered; the scene compares its language with the old notice but does not rewrite the original case.
12. **Case returned:** a later character asks what was settled; show confirmed facts and unresolved items separately.

### Short prose bank

Checkpoint notice: “The board has one nail left. The old holes are darker than the new paper.”

Register margin: “Amount copied twice. Collector’s mark absent. The clerk drew a line through neither.”

Driver’s statement: “I gave them nothing. That is what I remember. I cannot tell you what the wagon behind me gave.”

Radio log: “Instruction received at first bell. Receipt of instruction is not receipt of payment.”

Shelter board after unresolved closure: “Accounts differ. No collection verified.”

These lines are drafts, source-bounded and localization-ready candidates. Each needs an authoring context and cannot be selected merely because a doctrine name matches. The prose bank should include calm, pressure, refusal and late-arrival variants only when they express a different known context. Do not inflate to dozens of near-synonyms.

### Graph structure and reuse

The implementation-ready graph uses one entry, one source-selection hub, evidence-specific child nodes, a report-choice hub and three terminal outcomes. Optional character scenes can be separate small graphs that return to the quest hub. A linear scene suits broadcast playback; a hub-and-spoke conversation suits record comparison; short branches reconverge after clarification; relationship, knowledge, reputation and faction gates remain read-only predicates; repeated visits select a concise outcome line; failures mention the lost evidence; and player memory drives one truthful callback. Quest updates, relationship changes and world-state effects are explicit and routed, never hidden in text callbacks.

A scene graph should be authored as stable nodes with speaker, location/channel, conditions, text key, response IDs, effects, quest updates, relationship requests, world-state requests and next-node IDs only to the extent the current schema supports them. Adding a field to this conceptual model does not approve a new dialogue engine. If the current encounter schema has closed consequence strings, keep using them; do not embed free-form system effects in response text.

### Character dossier and faction voice sheet

**Nera, records clerk.** Wants the shelter to keep a record that can survive a change of controller. She values exact dates and distinct copies. She is not secretly a rebel or an informant; her conflict is whether administrative precision can protect people when institutions use the same paper to punish them. She avoids naming motives without testimony. Her voice uses short statements, corrections, and careful questions. Sample: “The seal belongs to the office. The mark belongs to the person who touched the page.”

**Ivo, wagon mender.** Wants work to stay practical and private. He is willing to describe what passed across his bench but resents being turned into a symbol for a political argument. He distinguishes a broken strap from the reason it broke. Sample: “I fixed the buckle. I did not bless the cargo.” He can become a recurring character if the player respects source and attribution boundaries; relationship effects still require the existing owner.

**Marek, checkpoint clerk.** Works under the Warlord doctrine and believes predictable procedures keep armed men from improvising. He can criticize missing signatures without becoming a defector. He speaks in operational terms: shift, register, copy, exception. Sample: “When the book is complete, no one needs to remember who shouted.” That line carries confidence in process and an underlying fear of arbitrary force.

**Sera, route runner.** Knows which notices arrive and when, but not what occurs at every post. Her special function is delivery provenance, not universal intelligence. Sample: “I carried it sealed. The first hand opened it at the toll house.” Only use that line if the campaign records the handoff; otherwise say “I carried it sealed as far as the toll house.”

### Shelter lore and location descriptions

At the shelter notice wall, several nail holes remain after the papers are removed. Residents use the gaps to infer that rules change before the official board changes. That belief is a piece of shelter folklore, not evidence of a hidden policy. A resident might say, “The board has a shadow for every paper it takes down.” Use the idiom sparingly and attach it to this speaker. The journal can note that residents disagree about whether the shadows mean anything.

At the weighbridge, if canonical location content supports it, the painted scale marks show a replacement plank that sits slightly higher than the old one. This observation might indicate repair, damage, or a deliberate change; the player can inspect it through existing location interaction. Sample description: “The lower line is under fresh pitch. An older groove crosses the timber below it.” Do not claim that the toll amount changed because of the plank. At the grain silo, a posted notice may be weighed down by a rusted bolt. The bolt’s source is unknown unless a clue says otherwise.

Shelter lore should expand as the player learns, but not become a global morale effect by default. A repeated saying may enter the journal if the existing journal accepts it; otherwise it remains dialogue. Character personalities should drive how the saying is interpreted: Nera treats it as rumor, Ivo as a joke, Marek as a complaint, Sera as a warning about bad schedules. Distinct voices prevent the institution itself from speaking with one mouth.

### Branch writing budget

For the first polished sample, budget roughly 18 dialogue nodes, 4 optional line variants, 6 response labels, 3 terminal summaries, and 2 environmental text lines. Every added state variant should have a gate row. Voiceover, if desired, is a later asset/cost decision. The first pass can remain text-based. A conversation should be readable in a single UI panel or page sequence with no paragraph wall. Put source/date in structured journal detail, not hidden inside an overlong line.

### Doctrine-specific voice variations

The writers’ room can draft voice contrasts for testing without committing to new factions or mechanics. These are candidate lines attached to distinct speakers, and they become available only if their doctrine source and character identity resolve.

A toll collector who supports regular tribute: “Same mark, same amount. A traveler can hate a known price and still plan around it.” This conveys the speaker’s rationale, not whether the current player paid.

A checkpoint clerk under a consolidation principle: “The post moved closer to the road. The register did not move closer to the truth.” This suggests disagreement inside procedure without claiming misconduct.

A driver who has experienced a withdrawal order: “They left before the last wagon. That is what I saw from the ditch.” The scene must gate it on an actual withdrawal event and an available witness; otherwise it is prohibited text.

A courier discussing indirect action: “The notice crossed three hands before it reached the board. I can name the hands. I cannot name who wrote the sentence.” Use only if the delivery chain is authored and tracked.

A laborer describing a labor-focused doctrine: “The roster says a shift. The wrist says how long it lasted.” This is a character’s metaphor, not proof of a particular work obligation or injury; the relevant labor/health owner must supply any mechanical state.

A trader in a resource-stranglehold scene: “Every crate still had a price. The road between them became the price.” This line can evoke scarcity while avoiding invented commodity quantities or regional market modifiers.

### Reusable journal, location, and radio descriptions

Journal entry template, announcement only: “On [day], [source] announced [rule]. No source in this case confirms that it was applied at [location].” The final text should use a complete localized variant rather than raw token concatenation if the grammar changes by language.

Journal entry template, observed enforcement: “[Witness/source] reports [action] at [location] on [day]. The record establishes the action described, not that the same rule was applied elsewhere.”

Journal entry template, conflicting records: “The notice and register disagree about [bounded field]. The available sources do not resolve which version governed [specific crossing].”

Map description template: “A known checkpoint associated with the current report.” Do not use “active enforcement site” unless a live world-state fact supports it.

Radio lead template: “A road instruction for [named route] is being repeated. The sender gives a rule and a date; no collection is described.” This content must not be delivered merely because the catalog contains a radio key.

### Character arc progression beats

Nera begins with a careful private note and gradually learns to file a clear uncertainty rather than waiting for a perfect record. Ivo begins by refusing to be publicly named and may later choose to read his receipt himself, if the consent boundary is supported. Marek begins convinced that a complete form prevents arbitrary force and later must confront a specific omission only if the game’s sources establish it. Sera’s arc concerns the burden of carrying official notices without endorsing them. None of these arcs requires a new relationship score; the choices are written as distinct quest results and later callbacks can read those results.

Side quests should include short debrief reactions when the player returns, but not force a chain. A player who leaves a witness alone has a complete path. A player who asks for a document may learn no new fact and still complete the side activity as “source unavailable.” This avoids making every narrative interaction a fetch quest.

The late-game faction hearing, if pursued, can put two records on the table and ask what the official chronicle may say. The chair does not ask the player to adjudicate every doctrine. It asks whether the summary accurately reports the available evidence. That narrower conflict supports political drama while remaining grounded in the game’s information systems.

### Main story arc: The Board, the Book, and the Road

**Opening: The Board.** A delivered radio notice announces a change in road procedure. At the shelter board, residents disagree about what it means. One thinks the rule begins immediately; another remembers a different rate from an older paper. The player can replay the broadcast, ask for the source, or defer. The story’s first dramatic question is not “Who is evil?” but “Which part of the claim is known?” The writing leaves room for fear and material pressure without turning uncertainty into a puzzle with a secretly predetermined villain.

**Middle: The Book.** A witness provides a receipt or a register entry, if a verified encounter makes one available. The player learns that a posted rule and a specific collection are related but not identical records. Nera wants the source preserved; Ivo wants his name withheld; Marek wants the procedure judged by its actual form; Sera worries that a notice arriving late can change a route even if no one collects anything. Their personalities produce a conflict about privacy, timing and responsibility. No character is an exposition dispenser.

**Closing: The Road.** The player selects a private note, a public account, an unresolved case, or no publication. The choice should have a clear, immediate receipt and a truthful journal summary. If an existing public consequence owner accepts the report, the world may respond through that system. If not, the close remains a local record. Later, a new doctrine announcement may invite comparison; it never rewrites the old report. The late-game callback asks what the player chose to preserve, not whether they “won” against the Warlords.

### Optional side-quest episodes

**The Driver Who Waited:** a protection/escort episode in which a witness refuses a direct route. The player can choose a safer, longer existing route, help the witness remain anonymous, or decide not to escort. This quest is only viable if current escort/expedition owners can model the action; otherwise turn it into a shelter interview, not a pseudo-escort with no mechanics.

**The Clerk’s Copy:** a resource/crafting episode about salvaging archival paper or ink. The player can recover materials only through existing scavenging/crafting systems. If the game has no usable record-restoration recipe, keep this as narrative texture and do not invent an item or recipe solely to support the line.

**The Quiet Post:** a discovery episode at an unstaffed or evacuated location, if current location state permits it. The player finds absence rather than an enforcement scene. The text must state the observation time and cannot prove that the site was always empty.

**The Second Hearing:** a faction episode in late game where rival speakers contest the same record. It creates a closed dialogue choice and a journal outcome, not a new tribunal manager. Its production cost is high due to character, faction, state and chronology dependencies.

Each episode lists required location, availability, failure state, reward, branching potential, reusability, production cost and core/expansion status. The main story can ship without any optional episode. This separation avoids making every doctrine investigation a chain of expeditions.

### Narrative polish and repetition control

Use varied sentence length and concrete objects: pin, carbon copy, buckle, register, axle, nail shadow, wet ink. Avoid explaining political doctrine in a monologue. Give the player one new fact or one consequential choice per scene. Each recurring speaker has a distinct avoid-list: Nera does not use threat language; Ivo does not speculate about faction command; Marek does not claim a receipt proves what he did not see; Sera does not claim to know every checkpoint. Keep a voice sheet near the authored graph, and audit all later expansions against it.

## Plan 20 Closeout and Integration Course — Scenes and Story

### Complete scope receipt

Plan 20 supplies the dramatic and human texture: the board, the book and the road. The case is carried by Nera, Ivo, Marek and Sera, each with a distinct motive and limit on what they can claim. The player hears a doctrine announcement, examines evidence when it is actually available, and decides whether to record a confirmed action, an announcement, a contradiction or uncertainty. Environmental prose uses objects—paper, register, buckle, board and road—to make institutional pressure tangible without turning atmosphere into proof.

The graph supports a hub-and-spoke conversation, evidence nodes, optional character scenes, public/private response choices and later state variants. Short branches reconverge; repeated visits acknowledge actual saved outcomes; skill checks add interpretation but never create facts. The sample lines, character dossiers, shelter lore and late-game hearing are DRAFT prose. They require canon and voice review and must map to existing dialogue/encounter schemas before data authoring.

### Integration course

1. Choose a verified source delivery route and define which scene can open from it.
2. Map nodes, speaker IDs, conditions, response IDs, effect references and next-node IDs to the existing content schema.
3. Keep response effects out of generic UI callbacks; commands route through quest or owning system adapters.
4. Draft complete localization units, not grammar-sensitive fragments; define missing-token fallback.
5. Review every statement against the truth ledger and character knowledge limits.
6. Build a graph reachability report for every quest state, disabled gate and terminal outcome.
7. Review panel size, line length, speaker transitions, focus, close/back controls, and keyboard/controller navigation.
8. Retain neutral exit and unresolved completion when optional evidence is unavailable.

### Acceptance and cut line

A complete scene has legible choice labels, a visible consequence scope, no hidden fact in a gated line, and a way to leave. Screen-reader behavior identifies speakers and options. Localization expansion must not clip text or change evidence semantics. The first implementation uses one shelter scene and perhaps one proven optional evidence encounter. Radio playback, escort, crafting episode, a new location scene, multiple doctrine voices and late-game hearing are separate expansion slices with their own owner checks and estimates.

The story can be removed without damaging the Warlord simulation. Rollback hides the quest entry and uses existing journal/radio/debrief surfaces. Previously saved source IDs remain readable through the archive fallback. The handoff includes scene graph, line inventory, voice sheets, all prose marked as observed/inferred/public claim, localization keys, disabled-path copy, graph lint result, accessibility review, expected panel behavior and integration dependencies. Do not report these samples as shipped canon before production review.
