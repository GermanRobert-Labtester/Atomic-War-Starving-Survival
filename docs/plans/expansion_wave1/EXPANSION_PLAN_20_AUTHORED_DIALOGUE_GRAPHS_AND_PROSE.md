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

