# Expansion-series Plan 17 — Quest Content and Lifecycle Architecture

**Status:** Design proposal; documentation only. No implementation ownership is claimed.
**Relationship to global plan numbering:** This is Expansion Planning Wave 1, Plan 17. The repository also has a separate global Plan 17 for environmental storytelling. Keep the namespaced filename and do not replace or renumber that plan.
**Purpose:** Define a small, maintainable contract for authored quests and generated quest instances, with enough content structure to add distinctive storylines and side quests without creating a second quest engine.

## 1. Design outcome

Use the existing quest runtime as the player-facing lifecycle and read model. Keep permanent definitions in the current canonical data authority. Keep generated instances as bounded, seeded snapshots made from validated authored templates. Let the quest owner own objective progress and status; let faction, relationship, inventory, location, campaign, and journal owners apply their own consequences.

The architecture is a flow through existing owners:

1. Authored definitions and templates are loaded and validated.
2. Eligibility reads a snapshot of current campaign, character, faction, and location facts.
3. A deterministic selector chooses a definition or template and binds permitted parameters.
4. The current quest runtime registers one instance with a stable definition ID and a distinct instance ID.
5. Existing game events advance objectives or expose a choice.
6. A typed result requests consequences from the systems that own those consequences.
7. The quest log and map display read models of the same authoritative state.

The selector must not write relationship values, faction reputation, map discovery, inventory, or ending facts. UI panels must not own eligibility, progression, or rewards. This plan adds no quest registry, parallel journal, world-state store, or generic narrative scripting language.

## 2. Evidence and boundaries

Current source includes QuestRuntimeCoordinator with a player-facing QuestLog read model and lifecycle states Offered, Active, Completed, Failed, Expired, and Abandoned. QuestType currently classifies Crisis, Expedition, Exploration, Faction, Shelter, and Story. DynamicQuestGenerator has authored templates and generated instances, while its source documentation identifies ProceduralNarrativeSystem as the canonical template authority for production candidate generation. PersonalQuestSystem and NarrativeQuestlineSystem serve more specific survivor-story scopes.

This evidence shows that quest concepts and runtime paths already exist. It does not prove every data catalog is consumed, every quest state is represented, or every effect has an active host route. Plans 59, 171, and 200, global Plan 17, and the current quest-runtime integration work are mandatory collision checks before promotion. Reuse the narrowest current owner and update the live integration ledger only through its authorized integrator.

## 3. Permanent definitions and generated instances

Permanent authored content is reviewed, versioned, and stable across runs. It contains narrative intent, allowed objectives, location and actor requirements, gating rules, text keys, reward request identifiers, failure routes, and authoring metadata. Definitions do not store mutable per-run progress.

Generated content is an instance bound from an authored template. Its saved facts are limited to what is required to continue the player's accepted instance: stable instance ID, definition/template ID, generation seed or stable binding, bound actor/location/resource IDs, objective progress, lifecycle status, accepted/created day, explicit deadline if any, and already-applied outcome identifiers if supported by the current owner. Generated prose must use authored fragments and checked substitutions. A generated quest may choose among approved values; it may not invent a location ID, reward, faction, character, or consequence.

The generation seed does not authorize wall-clock seeding, unstable hash enumeration, or a second RNG. Use the existing seeded RNG contract and stable sort order. If the current save owner cannot capture a proposed field, keep it derived or defer it pending an architecture decision.

## 4. Requested quest lifecycle

The requested vocabulary has more stages than the current shared QuestLifecycleState. Treat it as a design target requiring a compatibility audit, not as an instruction to add a competing enum. Keep definition visibility and instance lifecycle conceptually distinct:

| Requested state | Meaning and intended transition |
|---|---|
| Inactive | Authored definition is not eligible or its campaign gate is closed. |
| Available | Eligible and offered through an existing board, character, faction, or discovery route. |
| Discovered | A clue or environmental interaction revealed the opportunity; acceptance is still optional. |
| Accepted | The player committed to the instance; objective and destination become visible at the appropriate knowledge level. |
| In Progress | At least one objective or travel step is active. |
| Blocked | A named prerequisite is temporarily unavailable; keep the quest visible and give a recovery action. |
| Partially Completed | One or more objectives are complete while required objectives remain. |
| Failed | The primary approach failed. Where authored, transition to an alternate route instead of deleting the story. |
| Completed | Required objectives are complete and the quest's primary outcome is recorded. |
| Resolved | A branch, settlement, or epilogue has closed the narrative question. It may follow completion or a recoverable failure route. |
| Expired | A supported deadline passed. Show the changed world result and any continuation. |
| Abandoned | The player explicitly leaves the commitment; use current abandon semantics and preserve relevant history. |
| Reopened | A deliberately authored sequel or supported current API reactivates a resolved thread with a new instance or stage. Do not silently reset old state. |

The MVP uses current lifecycle values wherever they accurately express the behavior. Add any new persisted status only after verifying all consumers, save migration, terminal-state reporting, and compatibility with existing quest-specific systems. Blocked and Partially Completed may be derived read-model labels if the objective model already carries sufficient facts. Reopened should normally create a sequel instance, preserving the prior completed/resolved record.

Every transition needs an actor, triggering event, allowed source status, resulting status, visible feedback, and recovery path. Invalid transitions are rejected without partial rewards. Failed, Expired, and Abandoned are distinct because the player action and world consequence differ.

## 5. Quest type production cards

All cards use these review fields: purpose, typical structure, required locations, possible failure states, rewards, branching potential, reusability, production cost, and core-game or expansion fit. A location may be a known map node, a clue-led destination, a shelter context, or no physical destination. Reuse refers to structure and authored templates, not repeating unique story text.

### Main quest

- **Purpose and structure:** Carry the central survival mystery or campaign obligation through a chain of milestones, choices, and a clear resolution.
- **Locations:** A small authored set of reachable mandatory nodes; expose each objective and route through the map owner.
- **Failure and rewards:** Prefer costs, delay, or a changed route over a hard fail. Reward new access, information, or a campaign fact owned elsewhere.
- **Branching and reuse:** Branch at meaningful decisions and reconverge where the story permits. Reuse objective types and validation, never the unique central revelations.
- **Cost and placement:** High writing, continuity, and QA cost. Core game if needed for campaign completion; optional deeper chapters belong in expansions.

### Character quest

- **Purpose and structure:** Reveal a survivor through a concrete task, a pressure point, and a choice or changed relationship.
- **Locations:** Character's shelter duty, a familiar shared room, or one reachable expedition site; offer a delegate/delay route if absent.
- **Failure and rewards:** Missed timing may change trust or opportunity only through current owners; no silent arc deletion. Rewards may be knowledge, a trait request, access, or authored closure.
- **Branching and reuse:** Two or three voice/outcome variants can share the same objective rail. Reuse stage patterns, not personality.
- **Cost and placement:** Medium to high continuity cost. One representative arc fits the core; a roster of arcs is expansion content.

### Faction quest

- **Purpose and structure:** Make an agreement, dispute, or resource obligation playable at local and regional scale.
- **Locations:** A faction contact plus a reachable delivery, meeting, or investigation site.
- **Failure and rewards:** Refusal, broken supply, or unavailable contact routes to negotiation, public disclosure, or a smaller agreement. Rewards route through the faction and item/location owners.
- **Branching and reuse:** Reusable negotiation framework with faction-specific goals and distinct voices.
- **Cost and placement:** Medium cost for one core faction; high for a wide cast. Major faction arcs may be expansion content.

### Discovery quest

- **Purpose and structure:** Turn a found object, unusual marker, or field report into a question and a return choice.
- **Locations:** Starts at an environmental clue and may point to a second known or hinted location.
- **Failure and rewards:** A depleted or inaccessible clue grants a substitute lead or explicitly closes the opportunity. Reward knowledge, map discovery, or a journal record through its owner.
- **Branching and reuse:** High reuse of clue-to-investigation structure; keep evidence and interpretation specific.
- **Cost and placement:** Low to medium. Core content should teach the discovery loop; additional discoveries fit expansions.

### Investigation quest

- **Purpose and structure:** Gather at least two independent clues, compare explanations, and record a supported conclusion.
- **Locations:** Evidence nodes must be reachable by different routes where possible; make a clue available if a location pool is exhausted.
- **Failure and rewards:** Incomplete evidence produces uncertainty, not a fabricated answer. Wrong interpretation can be corrected or recorded with its consequence.
- **Branching and reuse:** Reuse evidence accounting and conclusion surfaces, not mystery solutions.
- **Cost and placement:** Medium continuity cost. One core investigation is useful; regional cases can expand the catalog later.

### Escort or protection quest

- **Purpose and structure:** Move a person or valuable item through a defined danger window with player-set preparation and a return condition.
- **Locations:** Start, route, and destination must be stated before acceptance; use existing expedition/travel routes.
- **Failure and rewards:** Injury, separation, retreat, or partial delivery must be represented honestly by current health/quest owners. Protect the story with a rescue or contact-again route.
- **Branching and reuse:** Reuse encounter and route scaffolding; cast and stakes stay authored.
- **Cost and placement:** Medium gameplay and balance cost. A short core example can teach preparation; longer escorts fit an expansion.

### Survival quest

- **Purpose and structure:** Respond to weather, health, shelter integrity, or scarcity using a sequence of diagnosis, action, and reassessment.
- **Locations:** Usually shelter or one expedition site; display required supplies before dispatch where possible.
- **Failure and rewards:** Scarcity can force triage or delay; never mint resources. Rewards should be a real resolved condition, knowledge, or trust through owners.
- **Branching and reuse:** High reuse of objective rails but require varied human stakes and multiple viable tactics.
- **Cost and placement:** Low to medium. Core game because it reinforces survival systems.

### Resource and crafting quest

- **Purpose and structure:** Source named materials, decide what to consume, and deliver or craft through canonical inventory and recipe systems.
- **Locations:** A reachable source or shelter workbench; list alternatives when item provenance permits.
- **Failure and rewards:** Shortage, contamination, or recipe lock can block the task visibly and open a substitute recipe or delayed route. Never create a quest-local inventory.
- **Branching and reuse:** Very reusable objective patterns; author distinct motivations and fair quantities.
- **Cost and placement:** Low content cost but requires economy/balance review. Core tutorial examples; specialized chains fit expansions.

### Location-based quest

- **Purpose and structure:** Make a place matter through an objective that changes with discovery, access, or prior use.
- **Locations:** The site is the mechanic and must have a stable canonical ID, route, visibility rule, and fallback clue.
- **Failure and rewards:** Closure, hazard, or exhausted content must update availability visibly. Reward map knowledge or access through current owners.
- **Branching and reuse:** Reusable site state and event triggers; unique location lore remains authored.
- **Cost and placement:** Medium due to map art, data, travel, and QA. Core locations support critical progression; optional sites may expand the world.

### Timed quest

- **Purpose and structure:** Communicate a real deadline and show what changes when it passes.
- **Locations:** Any required site must remain reachable before the deadline, and the map must display route time from the current expedition authority.
- **Failure and rewards:** Expiry is explicit and may switch to a repair, apology, or aftermath route. Never use a hidden timer.
- **Branching and reuse:** Reuse deadline checks only where a canonical campaign clock exists.
- **Cost and placement:** Medium balance and save/load cost. Use sparingly in core; seasonal/rotating examples suit expansions only after time ownership is proven.

### Repeatable or rotating quest

- **Purpose and structure:** Offer a bounded recurring task with changing authored parameters, cooldown, and clear rewards.
- **Locations:** Draw from a validated set of reachable sites and avoid consecutive identical dispatches where current state can support it.
- **Failure and rewards:** Expiry or abandonment applies its documented result. Rewards are capped and issued by existing owners.
- **Branching and reuse:** High template reuse; use deterministic seeded selection and stable IDs.
- **Cost and placement:** Low to medium. A small core rotation supports replay; larger catalogs are expansion material.

### Hidden quest

- **Purpose and structure:** Reward attention to a specific authored signal, document, or place without depending on arbitrary pixel hunting.
- **Locations:** Starts at a legible clue; undiscovered location remains hidden while its clue has a reachable route.
- **Failure and rewards:** Missing content resolves to a clue or unavailable state. Rewards are information or access with explicit feedback.
- **Branching and reuse:** Reusable clue mechanics; keep secrets distinct and avoid checklist repetition.
- **Cost and placement:** Medium discovery QA. Optional in core, well suited to expansion.

### Environmental-discovery quest

- **Purpose and structure:** Begin from a diegetic sign, recovered note, altered site, or encounter outcome.
- **Locations:** The initiating object must be part of an active location or encounter consumer, with a stable discovery event.
- **Failure and rewards:** If the source cannot spawn, provide another discoverable lead or do not offer the quest. No invisible acceptance.
- **Branching and reuse:** Reuse event routing and text slots; vary the evidence and interpretation.
- **Cost and placement:** Medium authoring and content-validation cost. A few core examples teach the loop; broad sets expand later.

### Choice-reactive quest

- **Purpose and structure:** Change later dialogue or objectives based on a prior, explicitly recorded player decision.
- **Locations:** Later locations remain optional unless part of critical progression; provide a no-prior-choice variant.
- **Failure and rewards:** Missing or old-save facts resolve to an authored neutral variant. Consequence ownership must be named.
- **Branching and reuse:** Prefer short branches that reconverge. Reuse condition evaluation, not consequence semantics.
- **Cost and placement:** Medium continuity cost; use in core for high-impact decisions and in expansions for callbacks.

### Fail-forward quest

- **Purpose and structure:** Preserve a story after failure by exposing a costly but coherent next route.
- **Locations:** Original and alternate destinations need independent reachability or an explicit clue bridge.
- **Failure and rewards:** Failure changes the objective, cost, or speaker; it never marks success. Reward may be smaller, delayed, or replaced by knowledge.
- **Branching and reuse:** High value for reuse across equipment, route, and social failure cases.
- **Cost and placement:** Medium design/QA cost. Core-critical stories should fail forward; optional challenge quests may have honest terminal failure.

### Playable side-quest seed: The Unentered Shelf

This provisional quest begins when a shelter clerk finds an unlisted crate behind the dry-goods shelves. Its label has been cut away; the crate contains ordinary blankets, each patched with a different maker's thread. A mechanic says the crate came from a sealed depot. A night nurse says some of the blankets were promised to a neighboring shelter. Neither claim is enough to establish ownership.

- **Purpose:** Let the player decide what a shared resource record should mean when the object is safe but its promise is unclear.
- **Typical structure:** Inspect the crate at the shelter, ask two people what they remember, then choose to keep it for local triage, carry part of it to the neighbor, or record the uncertainty and wait for proof.
- **Required locations:** The shelter store is mandatory. A depot or neighbor visit is optional and can be substituted by an authored clue if that location is unavailable.
- **Possible failures:** The expedition can return late or with no additional evidence. The quest then remains unresolved with an explicit “no proof yet” journal line; the crate is not silently awarded or consumed.
- **Rewards:** A recorded decision, possible access or standing request through the current owner, and a visible change to the shelter's stock only if the inventory owner accepts the transfer.
- **Branching potential:** Three short responses reconverge at a later count. A later quest can quote the recorded choice if an existing campaign or quest owner persists it.
- **Reusability:** The evidence-and-stewardship structure can support other shared goods; the blanket story, voices, and promise remain unique.
- **Production cost and placement:** Medium because it needs two credible witnesses, item references, and fallback text. A shorter version fits the core; a regional delivery sequel belongs in an expansion.

Sample offer text: “The crate is dry. The label isn't. I can count the blankets, but I can't count a promise from an empty line.”

Provisional character notes: the clerk speaks in counts and careful questions, never calls uncertainty theft; the nurse speaks in practical exceptions and remembers who was cold; the mechanic distinguishes a sealed door from a locked one. These are writing anchors, not runtime traits.

## 6. Minimum viable version and optional layers

### Minimum viable version

1. Audit the current definition loaders and choose one existing canonical catalog.
2. Validate stable IDs, allowed quest type, objective references, location references, and reward command identifiers.
3. Add one authored quest that registers with the existing runtime, appears in the existing quest read model, and can be completed or abandoned.
4. Add one generated template only if the current canonical generation authority and seeded RNG path can bind it without a second lifecycle store.
5. Make one accepted quest target visible in the current map/expedition flow with a fallback if its required location is unavailable.

### Optional expansion layers

- More quest types after each has a real objective-event producer and owner-routed consequence.
- Additional authored branches, character and faction arcs, and campaign callbacks after continuity and save evidence.
- Rotating templates after cooldown, persistence, deterministic selection, and de-duplication behavior are proven.
- Quest graph visualization and authoring lint after current content authority and CI ownership are agreed.
- Localization, voice, and cinematic layers are independent presentation expansions and must not enter Core.

## 7. Dependencies, risks, and acceptance

Dependencies are current quest lifecycle/API, existing canonical data loaders, objective-event producers, seeded RNG, quest save ownership, location/map reachability, and effect-owner host seams. Plan 18 consumes quest location requirements; Plan 19 defines authored/generated separation; Plans 20–22 define dialogue conditions and consequence routing.

Acceptance for any future package: one unique instance is registered; all objectives have live producers; the quest appears through a truthful read model; every target is reachable or has a visible failover; each reward routes to its existing owner; save and deterministic replay cover all persistent generated facts; catalog integrity passes; and focused tests cover completion, failure, expiry, and a fallback. The current task changes documentation only and runs no tests.

## 8. Integration course

**Course 0 — Intake:** Verify the premise against current source, data consumers, live batch, exact claims, and known debt. List competing quest owners and choose one.

**Course 1 — Contract:** Freeze the smallest authored definition and instance fields. Mark which lifecycle values are current, derived, or require schema/migration changes. Agree on invalid-data behavior and unknown-ID handling.

**Course 2 — Content:** Add the authored row to the selected canonical data source. Validate references, gates, route reachability, refusal, and a no-content fallback before connecting the host.

**Course 3 — Runtime:** Register and advance through the established quest runtime and existing event producers. Return typed effects; do not mutate other authorities from the quest module.

**Course 4 — Persistence and UI:** Use existing save ownership. Show accepted status, progress, blocker, deadline if real, destination knowledge, and outcome in current surfaces. Preserve accessibility and lifecycle.

**Course 5 — Verification:** Run only the claimed package's focused tests and validators. Prove a fresh run, load continuation, repeated resolution protection, deterministic generated binding, and all failure routes. Use a targeted Godot check only if host integration changed.

**Course 6 — Handoff:** Record evidence, changed files, owner contract, command results, untested boundaries, rollback behavior, and next dependency. The integrator updates the live ledger; this plan does not self-assign paths.

## Continuation pass 2 — runtime and authoring contract

### Definition contract

The authored definition should be small enough to review as one quest. A candidate contract is:

~~~text
QuestDefinition
    quest_id
    revision
    quest_type
    title_key
    offer_text_key
    availability_conditions[]
    accepted_location_policy
    objectives[]
    failure_routes[]
    reward_requests[]
    completion_text_key
    authoring_tags[]
~~~

Each objective has a stable objective ID, event kind, canonical target IDs, required amount, prerequisite objective IDs, progress visibility, and an optional alternate route. “Talk to the engineer” must name the actor or use an explicit role selector supported by the roster owner. “Find any medical supply” must bind a validated set of item IDs. Text fields remain localization keys. Authoring tags support audits and filtering only; they must not secretly become gameplay rules.

The generated instance snapshots only bindings that could change after the template is selected: instance ID, definition revision, generated seed/binding, actor/location/item bindings, objective progress, lifecycle, accepted day, explicit deadline, and resolved outcome. Store the definition revision or equivalent content identity so a later catalog edit cannot reinterpret a half-completed save without a migration decision. Avoid copying long descriptions, reward logic, or entire source templates into every save.

### Objective event contract

An objective advances only from a named, observable domain event or a validated player command. For each objective, author and integration review must be able to answer:

1. Which system emits the completion fact?
2. What stable identifiers are included in that fact?
3. Does the quest owner reject events before acceptance or after terminal resolution?
4. Can one event advance more than one objective, and is that intended?
5. Can a load or replay deliver the same event again?
6. What player-visible feedback follows partial and final progress?

An objective with no live producer is content-only and must not be advertised as playable. A delivery objective reads a receipt from the inventory/economy owner. A discovery objective reads a discovery fact from the map owner. A conversation objective reads a supported conversation result. An escort objective reads actual party/location/health outcomes from their owners. The UI may request an action but cannot synthesize its completion fact.

### State invariants and transition checks

The requested state vocabulary needs invariants that survive save/load:

- Inactive and Available describe definition eligibility until an instance is offered.
- Discovered identifies a real clue event; it does not imply acceptance.
- Accepted and In Progress have an instance identity and an owner-supported progress state.
- Blocked always includes a reason code and at least one next action or an explicit wait condition.
- Partially Completed is a read-model description of unfinished objectives unless the current lifecycle API proves a persisted state is required.
- Failed, Expired, Abandoned, Completed, and Resolved are not interchangeable terminal labels.
- Reopened points to a new stage or linked instance and retains the earlier resolution record.

Before a transition is applied, verify the expected current state and instance ID. A duplicate completion command returns the previous result or a harmless rejection. A stale event cannot reopen a terminal quest. If the transition fails, no reward request is applied.

### Content validation additions

The current catalog validator remains the validation entry point if it owns the relevant data. Quest-specific rules should report the exact definition, objective, reference, and reason. Check objective graph acyclicity unless a bounded repeat loop is explicit; ensure each non-optional objective has a producer; ensure every mandatory location has a route or failover; ensure a deadline is backed by a canonical time source; and verify that reward targets and parameters are accepted by their owners.

For a branch, inspect both mechanical and narrative coverage. Every offered response should have an authored consequence label, a valid target, and a neutral/unknown fallback where prior state may be absent. A branch that only changes one adjective remains cosmetic and should not acquire a durable world flag.

### Side-quest slice extension

The Unentered Shelf can be the first vertical slice only if the inventory owner can distinguish the crate from ordinary stock and the quest owner can bind an optional delivery. If those facts are not available, reduce it to a record-and-dialogue quest: inspect the existing crate as a clue, hear two witnesses, choose a written interpretation, and store the outcome through an already-supported quest fact. The neighboring shelter is optional. Its absence produces an explicit “no route confirmed” outcome, not a false delivery.

Three short response variants should keep each speaker distinct:

- Clerk: “I can enter what arrived. I cannot enter who it was promised to.”
- Nurse: “The smallest blanket went to the child who woke cold. That is what I remember.”
- Mechanic: “The seal was cut before the door opened. I checked the hinge. I did not check the label.”

These lines establish different standards of evidence. None proves who owns the crate. That question remains the player's to resolve.

## Continuation pass 2 acceptance

This pass is ready for future premise audit when the implementation packet can map each field, event, and effect to one current owner; when lifecycle transitions preserve the current public API or include migration evidence; and when the story slice has reachable required objectives plus explicit missing-content behavior. No runtime or data changes are made by this pass.

## Continuation pass 3 — authoring package and questline production rules

### Content card required for every quest

Before prose is expanded, the author creates one review card. The card is the human-readable contract for the authored definition and helps prevent a quest from becoming a pile of unrelated flags.

| Field | Authoring question | Integration evidence |
|---|---|---|
| Purpose | What player decision, knowledge, or survival behavior does this quest make meaningful? | A short statement that can be tested in play. |
| Typical structure | What starts it, what are its beats, and where does it end? | Entry event, objective graph, and terminal route. |
| Required locations | Which sites are mandatory, hinted, or optional? | Stable catalog IDs, route, map visibility, and fallback. |
| Failure states | What can go wrong, and what remains playable afterward? | Supported lifecycle, reason, event producer, and alternate objective if any. |
| Rewards | What changes for the player, character, faction, or place? | One canonical owner per effect and a visible result. |
| Branching potential | Which choices alter state, and which only alter tone? | Explicit branch edges and owner-routed effect requests. |
| Reusability | Which pattern or template can be reused without flattening the story? | Reusable objective form separated from unique text and canon. |
| Production cost | What art, audio, UI, location, localization, and QA does it require? | Named content dependencies and a bounded release slice. |
| Core or expansion | Is it required to understand or finish the base campaign? | Critical-path and missing-content analysis. |
| Owner and consumer | Who owns the state and which running surface displays it? | Current source/API, loader, host seam, save owner, and acceptance case. |

Cards are reviewed before high-volume prose. If purpose or required consumer is unclear, the item stays a story seed. If a failure route has no owner, simplify the design rather than creating another state store.

### Objective pattern library

Patterns reduce mechanical authoring cost while keeping the player's reason for acting specific:

- **Observe:** one stable discovery event completes an objective. It needs a real visible or environmental trigger and cannot count a map hover.
- **Acquire and deliver:** a canonical item receipt advances the objective; the inventory owner decides whether the item remains, is consumed, or is transferred.
- **Compare evidence:** each clue has a distinct ID and source; a conclusion becomes available only when required evidence is present. Missing evidence has a lead, not a guessed answer.
- **Reach and return:** arrival proves location contact; the quest may also require a return event. The map and expedition systems provide travel facts.
- **Protect through a window:** party safety, destination, and time period must be explicit. Use current health, roster, and travel owners.
- **Choose and reconcile:** a player command records one branch; the next beat can react to it using the saved quest outcome. No branch may be recorded by merely opening a panel.
- **Wait for a condition:** the blocker names a canonical owner fact or day threshold. The quest log says what will unblock it, and the event source is verified.

Every pattern has a default failure response: preserve completed objective facts, mark the actual failure source, and route to an alternate authored beat only if that route has its own completion criteria. Reusable patterns describe event handling; they do not authorize generic, consequence-free quest text.

### Branch budget and continuity control

A small quest should usually present one meaningful branch and reconverge at its next shared action. A main story may split into different routes when the required destinations, character availability, and saved outcomes can be maintained. Record branch fan-out, reconvergence point, terminal states, and callback count in the content card.

For each branch, answer whether it changes:

1. Cosmetic delivery only.
2. The current scene or encounter.
3. An objective or quest status.
4. A relationship fact.
5. Faction access or standing.
6. A location, resource, enemy, or other world fact.
7. The campaign ending input.

Multiple levels may be composed only when their owner and observable result are documented. If a branch affects later content, list every authored reader and the neutral behavior for missing/old-save data. This keeps Plan 17's quest graph compatible with dialogue gates in Plan 21 and effect routing in Plan 22.

### Release slice by content family

| Family | First shippable slice | Follow-on work that stays optional |
|---|---|---|
| Main | One four-beat chapter with a clear entry, one decision, a fail-forward route, and a closing journal summary. | Additional endings, multiple required factions, or a second region. |
| Character | One three-stage arc with absent-character fallback and one saved choice fact. | Roster-wide bespoke arcs and voice performance. |
| Faction | One request, one negotiation, one owner-routed standing/access result. | Multi-faction coalition simulation and rotating mandates. |
| Investigation | Two distinct clues plus one supported conclusion and an unresolved fallback. | Large evidence webs and multiple culprit simulations. |
| Repeatable | One template with a bounded eligible pool, deterministic binding, cooldown supported by current state, and capped reward. | Seasonal catalog rotations and cross-run metaprogression. |
| Hidden | One legible environmental clue, a clue-led map marker, and an optional resolution. | Multi-layer secret chains and rare-only progression. |

This table is a production budget guide. It does not set runtime concurrency limits. Before choosing any numeric cap, inspect existing limits and measure the player-facing log and host cost.

### Completion receipt for a quest package

An implementation handoff should include the content card, current APIs used, data IDs and consumers, source event for each objective, state transitions exercised, required map locations, save owner and migration note, deterministic selection evidence if generated, UI route and accessibility check, focused test command/results, and known content gaps. The integrator can then mark the package accepted, stale, or blocked without inferring intent from prose.

## Continuation pass 3 acceptance

Quest production is ready to scale only when each added quest has a completed content card, bounded branch budget, real objective producer, canonical location route, owner-routed outcome, and truthful terminal/failure behavior. Content volume follows validated patterns; it does not replace those gates.

## Continuation pass 4 — lifecycle translation, fail-forward packets, and release gates

### Keep the expanded vocabulary compatible with runtime lifecycle

The requested planning vocabulary is more expressive than the currently verified quest runtime lifecycle. The live coordinator has been observed using Offered, Active, Completed, Failed, Expired, and Abandoned. Do not persist every planning label as a new runtime state merely because it appears in a design document. Define the semantics first, then map them to the owner already responsible for the quest:

| Planning label | Proposed interpretation | Persistence decision |
|---|---|---|
| Inactive | Definition exists but its offer conditions are false. | Derived from definition and campaign facts unless current owner already records suppression. |
| Available | Offer conditions are true and the quest can be presented. | Prefer derived eligibility; avoid storing a second availability flag. |
| Discovered | Player found a clue or trigger but has not accepted the formal task. | Persist only when discovery itself is a durable campaign fact with a save owner. |
| Accepted / In Progress | Player committed to the task and at least one objective is actionable. | Map to the current accepted/active authority; do not add a parallel instance store. |
| Blocked | An objective cannot currently advance because a named prerequisite is missing. | Derive from current facts and expose a reason; never strand an active quest without recovery. |
| Partially Completed | Some objectives are fulfilled while required objectives remain. | Derive from objective facts where possible; aggregate in the journal for display. |
| Failed / Expired / Abandoned | Different causes for a route ending. | Retain distinct reason codes even if the runtime owner has a shared terminal bucket. |
| Completed / Resolved | Completion describes the objective result; resolution describes that follow-up handling is settled. | Keep the distinction in content semantics unless an existing owner needs a durable resolution fact. |
| Reopened | A later owner event makes a previously settled thread actionable again. | Require explicit reopen authority and idempotent transition rules; never infer it from revisiting a location. |

Every transition must state its source fact, owner, player-visible wording, and recovery path. An unavailable objective is not a new lifecycle state unless the runtime can explain how it becomes available again. Terminal labels should not be rewritten in place: record the cause and present the relevant alternate continuation.

### Author a fail-forward packet as a complete route

Use the already introduced provisional “The Unentered Shelf” as the fail-forward fixture. The clerk asks for a second witness to settle the crate's intended destination. If a depot or neighbor route cannot be selected for this expedition, the quest stays actionable with a visible evidence-pending objective; the map-selection owner may supply an authored clue route or defer that optional leg. If the player returns without evidence, the count remains unresolved and the crate is neither transferred nor treated as stolen. If the player chooses local use, neighbor delivery, or a written hold, the choice is recorded only after the quest owner accepts it and any physical transfer is accepted by the inventory owner.

This packet has four authored resolution records: evidence supports a local reservation; evidence supports delivery and the inventory owner confirms the transfer; evidence remains inconclusive and the player records a temporary hold; and the player declines or abandons the task. The records share the same character wants and objective identity, while owner-backed evidence, reward eligibility, and follow-up dialogue differ. Acceptance requires an observable route for every branch, a journal entry that describes the actual state, and no reward or inventory result that claims an unperformed transfer.

### Expand the content family without inventing a bespoke mechanic per quest

The first production slice should include one example each for a main quest, personal character task, faction request, discovery, investigation, escort/protection, survival, resource/crafting, location-based, timed, repeatable, and hidden task. Add two cross-cutting cases: an environmental clue that starts as Discovered, and a choice-sensitive quest whose failed objective opens a distinct route. These are content coverage fixtures, not a demand for twelve new runtime subsystems.

For each fixture, writers provide: entry trigger; canonical location or safe substitute; prerequisite facts; objective evidence producer; deadline owner if timed; repeat eligibility if rotating; cancellation and failure language; reward owner; dialogue follow-up; save/replay expectation; localization notes; and the expansion/core classification. Reuse an objective pattern only when its event source and failure meaning are genuinely the same. “Find,” “inspect,” and “deliver” must not collapse into one generic completion event if the owners report different facts.

### Validation and production gates

The content validator should report each quest ID with missing requirements, invalid transitions, unresolved location references, unowned effects, duplicate objective keys, reward commands without consumers, and branches with no reachable ending. A graph walk should distinguish unreachable nodes from intentionally hidden nodes and should identify what fact reveals a hidden route. A producer map should show the existing subsystem event or approved new owner required for each objective; presence in a JSON catalog alone is not a pass.

Release in slices: first one quest with no branch; next a short reconvergent branch; then one fail-forward case; then a timed or rotating case only after a current deadline/cooldown owner is confirmed. Save migration is required only for data the canonical quest owner persists. Replaying a deterministic campaign must preserve selection and generated bindings while respecting the player's explicit choices.

The implementation handoff includes the accepted state mapping, content cards, reachable graph report, per-objective producer, location and fallback receipt, reward/effect owner, focused state-transition and save checks, and an end-to-end journal/UI observation. If any owner is missing, retain the plan proposal and cut that mechanic from the first release slice.

## Continuation pass 4 acceptance

This lifecycle tranche is ready for a premise audit when planning labels map cleanly to current runtime semantics, blocked and failed quests have visible recovery, authored variants cannot award false outcomes, and validation reports graph reachability and owner evidence per quest. No new quest store, deadline clock, reward ledger, or save section is implied.

## Continuation pass 5 — owner map, quest portfolio, and the shared vertical-slice contract

### Source-verified owner map

The current implementation already separates quest responsibilities. The shared runtime coordinator records instances and exposes its read model; the procedural narrative authority owns authored template eligibility and seeded binding; the specialized Year of Ash questline owns its stage-and-choice records; survivor narrative questlines and personal quests retain their dedicated scopes. The content package must identify which owner consumes its definition. It must not route every authored arc through the shared coordinator merely to make the quest log uniform.

| Concern | Verified current seam | Design instruction |
|---|---|---|
| Shared quest instance lifecycle | QuestRuntimeCoordinator; current states Offered, Active, Completed, Failed, Expired, Abandoned. | Keep expanded labels as derived meanings unless a verified runtime consumer requires a durable new state. |
| Structured generated candidates | ProceduralNarrativeSystem and QuestTemplateCatalog; NarrativeWorldSnapshot carries day, world tags, eligible actors, known locations, obtainable items, protected IDs, and active quests. | Extend validated template slots and existing seeded selection. Generated bindings stay within canonical actor, item, and location IDs. |
| Shared quest instance persistence | ProceduralNarrativeRunState carries procedural narrative state alongside QuestRuntimeState. | Inspect the current registered save path before changing fields; do not create a second quest section. |
| Long stage-and-choice arcs | YearOfAsh QuestlineSystem and its QuestlineDefinition, Stage, and Choice records. | Keep its condition tags and host evaluation contract intact until audited. A graph proposal must name unsupported condition behavior. |
| Survivor-specific arcs | NarrativeQuestlineSystem and PersonalQuestSystem. | Use these for their supported personal scope; do not collapse them into generic dialogue nodes. |
| Map destination truth | WastelandMapSystem and ExpeditionSystem. | Quest content supplies a canonical requirement; those owners determine map status, discovery, route, and dispatch validity. |

The source also includes more than one loader and several content families. The presence of a class or JSON file does not establish that a new content family is live. A promotion packet names the loader, the host constructor that binds it, the runtime method that consumes it, and the persisted owner, if any. QuestTemplateCatalogLoader.Validate and actual host use need an explicit check in the package; a catalog row is not production-ready merely because it deserializes.

### Portfolio design: many quest types, one tested runtime path

Build the content portfolio as a matrix of authored intent against existing producers. A survival task may observe a canonical day or needs transition; a location investigation consumes a real discovery/visit fact; a personal arc uses an existing survivor event; a faction request routes its outcome through the faction owner; a resource request checks and consumes through inventory; a timed task uses the campaign day owner. If a cell lacks a producer, classify the row as a proposed owner extension and leave it outside the minimum viable release.

The first portfolio should contain a small number of complete exemplars rather than one thin row for every quest type. A complete simple quest proves definition load, offer, accept, objective update, journal/read-model refresh, completion or explicit refusal, and restore. A reconvergent quest proves two ways to gather evidence. The Unentered Shelf fixture proves inventory transfer and a no-evidence resolution. A timed quest proves expiry only after its clock owner and route semantics are confirmed. Repeatable generation remains a later layer because cooldown, eligibility, stable instance IDs, and objective-module reachability all need proof together.

### Unentered Shelf vertical slice across owners

The story question is stewardship under incomplete evidence: a clerk finds unlisted blankets; the nurse believes a neighboring shelter was promised a share; the mechanic can identify the sealed depot but cannot testify to the promise. None of these accounts alone establishes title. The task is intentionally modest and should create a practical state change, not a generic morality score.

1. **Discovery:** The crate or its existing authored clue is encountered. The discovery owner emits the fact; the quest becomes Discovered only if the current quest path can represent that distinction, otherwise the journal projects a pending offer.
2. **Offer:** The clerk asks for an evidence route. Acceptance creates one stable quest instance; deferring creates no phantom accepted instance.
3. **Evidence:** The player can ask either witness, inspect a selected canonical location, or return without further evidence. Location requirements are passed to Plan 18; clues and record fragments are authored under Plan 19.
4. **Decision:** The player may choose local reservation, confirmed delivery, or unresolved hold. Dialogue gates read evidence from Plan 21, while choices send typed requests through Plan 22.
5. **Resolution:** The quest owner settles its objective. Inventory performs any real blanket transfer. A journal line reports the result. Relationship or faction standing changes only if an approved owner-backed consequence has been authored; this story does not require one.
6. **Callback:** A later revisit can show the count or delivery state only from the relevant owner. Reopening the cabinet, starting a second instance, or loading an old save cannot duplicate the transfer.

This single vertical slice exercises quest, expedition, location, authored/generated boundaries, dialogue, memory gates, and consequence routing. It adds no faction, NPC profile, item definition, independent location state, relationship meter, or reward currency. A later expansion may add a separate neighbor perspective only after the content atlas confirms that such a delivery arc is not already represented.

### Quest-type content backlog with distinct production purposes

Assign each candidate to one of three content roles. A core teachable task demonstrates an existing mechanic and remains short. A character arc changes how a known survivor approaches a practical problem, using the personal quest owner and sourced memory. An expansion investigation reveals information whose ambiguity is deliberate and whose destinations are optional. These roles prevent quest-type labels from generating the same fetch-and-return story under different headings.

For every proposed quest, record its dramatic question, player verbs, consequences, source facts, counterfactual route, failure meaning, revisit behavior, campaign stage, and collision keywords. Collision keywords include objects, institution, location function, moral dilemma, and ending callback, not only the title. Search these against implemented content, active integration plans, completed expansion waves, and partial-plan placeholders before promoting prose. If only the structural pattern is reusable, retain the pattern and rewrite its content after continuity review.

## Continuation pass 5 acceptance

This tranche is ready for a premise audit when each content family is assigned to a verified current owner, the Unentered Shelf fixture has a complete player-visible path through all six plans, repeated actions cannot duplicate physical outcomes, and the collision search covers story function as well as exact names.

## Continuation pass 6 — production dossier, lifecycle projection, and route proof

### Quest dossier: minimum authored fields for a complete playable task

Treat a quest dossier as a design-time completeness checklist. It is not a proposal for a second quest runtime schema. During implementation, map each field to the current quest definition and its existing consumers; leave unsupported fields in documentation until an owner and loader are verified.

| Dossier field | Authoring question | Required evidence before integration |
|---|---|---|
| Stable identity | What durable authored quest is this, and is this ID already used? | Current catalog and duplicate-ID search. |
| Story purpose | What does the player understand or change by engaging? | One-sentence dramatic question tied to a gameplay verb. |
| Quest family | Is it main, character, faction, discovery, investigation, escort, survival, crafting, location, timed, repeatable, hidden, or choice-dependent? | Existing category vocabulary or an approved content extension. |
| Core/expansion placement | Is this required for the base route or optional expansion content? | Expansion-disabled reachability review. |
| Discovery trigger | What authored clue, encounter, conversation, or owner event makes it available? | Existing source that can produce the trigger. |
| Offer and acceptance | Who presents the task, and how does the player defer or refuse? | Current quest and dialogue command seam. |
| Objective verbs | What can the player actually do: inspect, carry, protect, negotiate, repair, survive, report? | Existing or separately approved interaction consumer. |
| Evidence model | What observable owner fact proves each objective? | Canonical event/fact owner and persistence path. |
| Required locations | Which exact locations are essential, substitutable, clue-led, optional, or return-only? | Stable location definitions and Plan 18 fallback contract. |
| Character requirements | Must an actor be present, or can a record/alternate witness preserve the route? | Current roster and authored fallback. |
| Condition gates | What knowledge, skill, relationship, reputation, or phase changes presentation? | Plan 21 predicate and unknown policy. |
| Consequence requests | Which response requests a state change? | Plan 22 typed owner command and result copy. |
| Failure semantics | What action or deadline constitutes failure, and which owner records it? | Clock/quest owner and explicit failure evidence. |
| Recovery route | Can the story continue in another way? What new evidence or action proves recovery? | Distinct objective and successor route, never rewritten success. |
| Resolution | What terminal outcomes exist, including refusal or unresolved hold? | Current quest terminal status or approved projection. |
| Reward | Which existing owner grants the reward and how does it prevent duplicates? | Valid catalog item/resource and accepted command. |
| Repeat policy | Can the task recur? How do cooldown, identity, and outcome carry over? | Existing deterministic eligibility and save rules. |
| Revisit policy | What line/objective changes on later visits? | Persisted owner fact; no visit-count inference. |
| Map and journal copy | What do map and journal report at each phase? | Current read-model consumers and localization keys. |
| Generated variation | Which wording or objective parameters may vary? | Approved template, bounded fields, seeded inputs. |
| Save/migration | What stable reference is saved, and what if this definition changes? | Current owner capture/restore and replacement path. |
| Accessibility | Can every required action be understood without audio, color, or transient animation? | Text, focus, input, and feedback review. |
| Production estimate | What authored rows, scenes, assets, voice, and QA cases are required? | Bounded deliverable list; no speculative runtime scope. |
| Collision record | Which existing quest or plan is closest in function, setting, and choice? | Corpus search plus explicit distinct-purpose note. |

A dossier is incomplete if quest acceptance has no objective consumer, an objective has no evidence owner, a location has no availability/fallback rule, or a reward has no idempotent grant path. A complete design does not require every possible feature field. Optional gates and generated variants must earn their production cost.

### Player-facing lifecycle is a projection over current quest authority

The requested presentation vocabulary includes Inactive, Available, Discovered, Accepted, In Progress, Blocked, Partially Completed, Failed, Completed, Resolved, Expired, Abandoned, and Reopened. Current implementation evidence includes a more compact runtime set in QuestRuntimeCoordinator. Do not add permanent enum values just to match design labels. Define how each label is projected from the actual owner state and objective evidence:

| Presentation label | Design meaning | Safe source or projection rule |
|---|---|---|
| Inactive | No current offer is available. | No eligible offer from the current quest owner. |
| Available | The player could accept the task. | Owner reports an offer and its entry conditions pass. |
| Discovered | A clue has brought the task to the player's attention. | Use a real discovery fact; if the owner cannot persist this separately, present it as journal/offer copy without claiming a durable phase. |
| Accepted | The player has agreed to the task. | Confirmed transition from the owning quest authority. |
| In Progress | At least one objective remains actionable. | Active quest plus current objective evidence. |
| Blocked | Required progress cannot presently be made. | Active quest and a known missing prerequisite; include retry or alternative route. |
| Partially Completed | Some objective evidence has been accepted. | Objective projection from owner-backed progress, not a parallel counter. |
| Failed | The primary condition failed. | Explicit failed/expired outcome from the owner, including cause where available. |
| Completed | The authored primary completion condition succeeded. | Current owner terminal result. |
| Resolved | An alternate, negotiated, or deliberately unresolved route has a settled outcome. | Only if the current model distinguishes it; otherwise present an authored completion detail, not a new global state. |
| Expired | A real deadline elapsed. | Current clock/quest owner has recorded expiry. |
| Abandoned | The player intentionally left an accepted task. | Owner-confirmed abandon command; do not infer from inactivity. |
| Reopened | A prior terminal task has a supported new continuation. | Explicit owner transition or a separate follow-up quest; never reopen silently on scene revisit. |

This mapping lets UI copy remain expressive while state authority stays compact. It also prevents Blocked or Partially Completed from becoming new save values when they can be derived from existing active-state objectives. Any state that cannot be reconstructed after restore requires a current owner proposal before it can be presented as durable.

### Fully worked Unentered Shelf quest card

**Pitch:** A shelter clerk discovers that a count of blankets does not match the old distribution note. A nurse remembers that a neighboring shelter was promised help, while a mechanic can identify a possible route but cannot verify delivery. The player investigates the discrepancy and chooses a truthful disposition.

**Purpose and family:** Small investigation/character task; core-game placement only if the existing onboarding and location catalog can support it. Otherwise an optional expansion vignette. Its purpose is to model how players can act responsibly with incomplete records, not to create a new trade ledger or morality meter.

**Offer:** The clerk exposes the discrepancy after the existing shelter discovery trigger. The offer explains that the records disagree. Accepting requests an investigation; declining leaves the supplies and quest owner untouched. Discovering the note may reveal an offer, but the task becomes accepted only after the quest owner confirms the player's choice.

**Objectives and evidence:** First, inspect the record through its canonical interaction. Second, ask the nurse or consult an approved alternate record for the promise claim. Third, optionally investigate the route through Plan 18. Fourth, return with a supported disposition: confirmed delivery, local allocation, or unresolved hold. Every verb maps to a concrete event or interaction result. Talking to a character is not automatically evidence that the character's claim is true.

**Branch policy:** The critical resolution is available even if one optional witness is absent. A skill gate can clarify what the record's handwriting does and does not establish. Faction reputation may add a contact or alternate record only if a current faction owner supplies that opportunity. No optional gate is the only way to understand the shortage.

**Failure and continuation:** If an expedition deadline or route becomes unavailable, the quest owner can keep the proof objective pending, substitute an equivalent authored source where explicitly allowed, or move to an unresolved-hold resolution. If a deadline truly expires, record the failed delivery attempt as failed and offer a separate recovery route only if a valid owner-backed task exists. Do not claim the blankets arrived because the player selected a dialogue response.

**Outcomes:** Confirmed delivery requires an actual inventory/resource operation and accepted destination evidence. Local allocation requires a confirmed stock operation and keeps the neighbor claim unresolved. Unresolved hold records no transfer and closes the investigation only if the quest definition explicitly supports that resolution. Reward is recognition or a currently supported item/resource, never a newly invented reward counter. A no-reward outcome remains valid.

**Revisits:** After delivery, the clerk reports the owner-confirmed transfer. After local allocation, the clerk reports the room assignment and separately states that the destination claim remains open. After unresolved hold, the clerk reports no movement. If the relevant result is missing in an older save, use neutral copy and query the canonical owner; never reconstruct from the fact that the scene was visited.

This card is deliberately small but end-to-end. It exercises offers, objectives, clue discovery, location selection, optional knowledge, owner-routed effects, a fail-forward option, a journal projection, and a later callback. It does not require a new faction, dialogue history system, location registry, or save section.

### Route-proof table for a quest before content approval

For every accepted quest, trace at least one playable sequence through each row:

| Proof point | Review question | Expected artifact |
|---|---|---|
| Entry | How can a fresh save discover or receive the offer? | Trigger reference and visible cue. |
| Acceptance | What command makes the task active? | Owner and accepted/rejected copy. |
| First objective | Which interaction/event advances it? | Evidence ID and consumer. |
| Optional absence | What happens if a supporting NPC, faction, or expansion bundle is missing? | Alternate source or still-playable baseline. |
| Location unavailability | Can Plan 18 provide an equivalent, clue, delay, or explicit route change? | Named fallback semantics. |
| Stale state | What if the facts change after a conversation opens? | Revalidation and refresh behavior. |
| Failure | Which owner records the condition and cause? | Failed/expired evidence and retained history. |
| Recovery | Is recovery a real objective with its own proof? | Separate follow-up route or explicit no-recovery close. |
| Reward | Can replay, retry, or restore grant it twice? | Owner command and duplicate result. |
| Restore | Does the task reconstruct after save/load? | Current capture/restore evidence. |
| Ending/callback | Does later prose reflect only confirmed facts? | Source-linked text variants and neutral fallback. |

Promotion should stop at the first missing proof point. Mark the proposal as a gap with an owner question; do not fill the gap by inventing local state in the quest definition or dialogue callback.

## Continuation pass 6 acceptance

The quest package is ready for a vertical-slice claim when a fully populated dossier can be mapped to current owner APIs, presentation labels do not require speculative durable states, the Unentered Shelf card remains completable across missing optional evidence and unavailable routes, and every accepted/failed/recovered/reward outcome has an observable proof source.

## Continuation pass 7 — quest family coverage, production sizing, and route recipes

### Portfolio coverage without a checklist-shaped game

The game should support different quest families because each offers a different player verb and consequence pattern, not because the catalog needs one of every label. Use this matrix to decide what belongs in a release and what needs a distinct owner path:

| Quest family | Typical structure | Required locations | Possible failure state | Rewards and branching | Reuse, cost, placement |
|---|---|---|---|---|---|
| Main story | Reveal a pressure, make a choice, observe a consequence, then revisit the changed situation. | At least one critical route plus a base-game fallback. | Route missed or evidence unavailable; preserve a valid continuation. | Unlocks authored progress; meaningful branch count kept low and reviewed across acts. | Low reuse; high production cost; core campaign. |
| Character | A survivor's practical problem exposes a personal belief, then a later callback tests it. | Character's current shelter context and at most one optional external site. | Character absent or trust fact unavailable; alternate record/contact route. | Relationship result only through its current owner; usually reconverges. | Reusable scene structure; medium/high cost; core if tutorial/arc critical, otherwise expansion. |
| Faction | A group negotiates access, duties, or terms with visible tradeoffs. | Faction contact point, plus any required project site. | Negotiation fails, access deferred, or faction leader absent. | Standing/access from faction owner; branches need explicit future costs. | Medium template reuse; high continuity cost; optional expansion unless core factions are required. |
| Discovery | An environmental clue reveals a fact and an opportunity. | Initial clue site; destination may remain hidden. | Clue destroyed, unreadable, or contradictory. | Knowledge, map lead, or quest offer; can branch by what the clue proves. | High reuse as pattern, medium content cost; use in core exploration and expansions. |
| Investigation | Gather independent evidence, compare accounts, choose an interpretation or unresolved hold. | Multiple evidence sources; no single optional witness can be mandatory. | One source unavailable or evidence disproves the initial theory. | Information/reputation/outcome; branching can change judgment without changing facts. | Reusable evidence template; medium/high cost; core for a complete detective loop, optional for extra mysteries. |
| Escort/protection | Prepare, escort or defend, then account for the protected party's outcome. | Departure, route segment, destination or safe fallback. | Party lost, delayed, split, or protected by another route. | Owner-backed materials or access; failure route preserves identity and history. | Reusable objective modules; high encounter QA; core only if protection teaches a main mechanic. |
| Survival | Meet a concrete environmental/needs challenge and return or stabilize a situation. | Hazard site or shelter interaction that uses existing survival owners. | Need/hazard threshold breached or route becomes unsafe. | Survival supplies only through current resource authority; choices change method, not invented score. | High systemic reuse; medium balancing cost; core when it teaches required survival verbs. |
| Resource/crafting | Identify a shortage, source inputs, perform an existing recipe/action, report the result. | Source and work site defined by current item/crafting catalogs. | Inputs missing, recipe locked, or station unavailable. | Existing item/resource result; alternatives use supported recipes. | High mechanics reuse, data QA cost; core when recipe is baseline, expansion when optional. |
| Location-based | Arrive at a named site, notice a local state, complete an interaction or choose to leave. | One stable canonical location with discovery and route rules. | Location unavailable, locked, or changed before arrival. | Site-specific information or result; avoid duplicating the location's permanent story state. | High scene-template reuse, moderate location authoring; core or expansion by route necessity. |
| Timed | Owner-backed deadline, visible urgency, clear result on time or expiry. | Required dispatch target and a valid no-target/delay policy. | Expiry recorded by the clock/quest owner; follow-up stays distinct. | On-time result and late recovery have distinct outcomes and rewards. | Low story reuse, high QA; core only when deadline play is an established mechanic. |
| Repeatable/rotating | Owner supplies an eligible task from a bounded, deterministic set; resolve and wait for eligibility. | Candidate sites from current map/expedition definitions. | Candidate exhausted or temporary content expired; no phantom offer. | Bounded, duplicate-safe outcome; no unbounded currency loop. | High system reuse, high balance cost; optional live/expansion layer unless base game already relies on it. |
| Hidden | Environmental or knowledge condition reveals an optional objective. | Clue site and optional hidden destination. | Missed clue does not block base progression. | Discovery, lore, or supported item; no critical unique power hidden behind an unmarked gate. | Low template cost, high continuity review; usually optional/expansion. |
| Environmental start | Object, sound, map annotation, or damage starts or advances a task. | The authored context in which that cue can be perceived. | Cue destroyed or unreadable; alternate journal/interaction route. | Evidence or offer from current quest owner after the event. | Reusable pattern; requires accessibility/visual/audio QA; core when it explains a required mechanic. |
| Choice-dependent | Prior owner-backed outcome changes later offer, line, or objective. | Any location required by the earlier choice plus a recovery route. | Prior outcome incompatible with the assumed route; alternate branch must remain complete. | Consequences may be local, quest, relationship, faction, or world; label each. | Low prose reuse, highest continuity cost; use sparingly in core and expansions. |
| Fail-forward | Primary action can fail, with a different action and proof route still possible. | Primary and recovery destinations, each with explicit selection policy. | Preserve original failure while continuing through a separate route. | Recovery result has its own reward/closure; never rewrite failure as success. | Reusable routing pattern; medium/high QA; appropriate in both core and expansion when authored. |

“Core” means the player can understand and complete the base campaign's promised route without optional bundles. “Expansion” means an optional bundle can add another instance of the pattern without owning the only base-game evidence, system, or exit. Production cost is not simply prose volume: objective consumers, condition branches, unique location assets, voice work, save migration, and route QA dominate many estimates.

### Quest family acceptance cards

Before approving a family for a milestone, choose one exemplar and complete a family card:

- **Main story card:** name the act transition, canonical source facts, minimum critical route, at least one understandable player choice, and the old-save/replay state that proves the transition. Estimate all downstream callbacks before adding branch endings.
- **Character card:** identify one supported memory fact, one interaction that tests the character's stated approach, one neutral route when the character is absent, and the owner that records any relationship change. Do not let a single optional relationship threshold hide critical progress.
- **Faction card:** state the terms offered, the current faction access/reputation authority, the consequence of acceptance/refusal, and a route for players who have not joined or do not qualify. Separate player comprehension from faction arithmetic.
- **Discovery/investigation card:** identify what each source can actually prove, what remains uncertain, and whether a clue reveals a precise location or only a region. A contradictory source should change interpretation, not silently overwrite the other source.
- **Escort/survival card:** define departure, progress checkpoints, interruptions, arrival, and rescue/fail-forward outcomes. Use the existing needs, health, hazard, route, and roster owners rather than task-local survival counters.
- **Resource/crafting card:** name input IDs and recipe/action consumer, record why each input is needed, and ensure missing materials create an alternate or deferred route when the quest promises one.
- **Timed/repeatable card:** name the deadline/cooldown owner, state the exact eligibility fact, define deterministic candidate selection, and prove no reward duplication after restore or repeated acceptance.
- **Hidden/environmental card:** state how the clue is detected through accessible modalities, what it reveals, and why missing it cannot make the main route impossible.

The card ends with a decision record: accept for the current release, hold pending a source seam, move to expansion, or reject as duplicate. “Write more later” is not an acceptance decision.

### Compact side-quest route recipes

The following recipes describe route shapes, not new canonical quests. Use them to populate a portfolio only after collision review finds a clear story purpose.

**Evidence-and-interpretation route:** a clue begins the task; two evidence channels can be visited in either order; each has separate provenance; one optional skill question clarifies a detail; a final response chooses an interpretation or explicitly leaves it open. The canonical facts survive every branch. This recipe suits investigation and environmental discovery.

**Protection-and-recovery route:** the player accepts a protection task; the expedition contains a small number of owner-backed checkpoints; a delay or hazard changes arrival condition; failure remains recorded; a different recovery task helps the party after the original deadline. This recipe suits escort and survival content. Avoid multiplying escort targets until the current roster and combat/route systems can represent them.

**Shortage-and-work route:** an NPC reports a shortage; the player checks an existing resource/recipe owner; one path uses a supported craft action and another seeks an existing alternate item; inability to source either leaves the task deferred or produces a truthful substitute result. This recipe supports crafting and community work without inventing a new economy.

**Choice-and-callback route:** the player makes one local decision; the quest owner records it; a later scene reacts to the canonical result; an absent speaker has a record/alternate contact; a later branch may acknowledge the result but does not retroactively change it. This recipe creates continuity without requiring a global choice-history registry.

For each recipe, test the blocked, failed, deferred, abandoned, and completed paths. If a route has no purposeful failure condition, do not add an arbitrary timer or random lock simply to make the quest appear systemic.

### Rewards and player-facing closure

A reward can be material, access, information, or closure, but it must be observable and owner-backed. Before authors request a material reward, confirm a current catalog item/resource and duplicate grant behavior. Before promising access, confirm the faction/location authority can grant it. Before promising a relationship result, confirm the relationship owner accepts the event. Information can be a reward without changing mutable state if it has a supported journal or map presentation.

Completion copy should identify what was accomplished; resolution copy should identify how an alternate outcome settled the question; failure copy should name the lost opportunity without blaming the player for a system gap. A no-reward ending may still provide closure. Do not use vague “reputation increased” copy unless the faction owner supplies that exact result and the UI already supports showing it.

### Release gates by content volume

Small quest tranche: one definition, one complete route, one failure or refusal behavior, one focused owner path, and a save/restore observation if stateful.

Medium tranche: multiple quest definitions with shared objective modules, repeated-character continuity, one optional condition family, dependency graph, and expansion-disabled checks.

Large tranche: a campaign arc with cross-quest dependencies, multiple endings, generated candidates, and late callbacks. It needs a dedicated continuity map and staged integration wave; it must not land as one giant content-only batch that cannot be attributed to current owners.

## Continuation pass 7 acceptance

The quest portfolio is ready for production sizing when each proposed family demonstrates a distinct player verb and evidence source, core and expansion placement are justified by dependency rather than word count, failure/recovery and reward policies are explicit, and the selected exemplar can pass the route-proof table from pass 6.

### Minimum field budget by quest scope

Keep required fields proportional to the quest's effect scope. A discovery vignette can begin with stable ID, trigger, evidence, text, location policy, and a no-mutation exit. A resource/crafting task additionally needs item/recipe references, current inventory feasibility, missing-input behavior, and a duplicate-safe result. A timed escort adds its clock owner, deadline evidence, checkpoints, participant availability, route fallback, and distinct recovery outcome. An ending quest adds every source fact, campaign authority, old-save treatment, expansion-disabled behavior, and a reviewable outcome map.

Do not require a faction matrix for a personal task that never touches faction state. Do not add a reward field that says “none” when the quest is informational. Explicitly mark dimensions as not applicable in the design card when their absence could otherwise be mistaken for an omission. This lets reviewers distinguish a lean design from an incomplete one and keeps validation proportional to actual dependencies.

Every quest still needs a player-facing entry, a comprehensible next action, a truthful terminal/continuation result, and a way to leave safely. Those are quality requirements, not reasons to add new save fields.

## Continuation pass 8 — objective evidence, dependency graph, and quest review gates

### Objective kinds and proof contracts

Quest objectives should describe player-understandable verbs while depending on explicit proof. Use a small family of objective patterns and route each through the current system that can observe it:

| Objective pattern | Player-facing verb | Proof event | Common false positive |
|---|---|---|---|
| Inspect | Examine a record, site, machine, or clue. | The current interaction/discovery owner confirms the inspection. | Being near the location or opening a dialogue. |
| Gather | Obtain one or more canonical item IDs. | Inventory/resource owner confirms current possession or acquisition event. | A quest-local counter that ignores transfer/drop. |
| Deliver | Transfer an identified amount to a canonical recipient/container. | Resource owner accepts the transfer and quest owner records the dependency. | Carrying the item near the destination. |
| Report | Return to a speaker or terminal with accepted evidence. | Dialogue/quest owner accepts the report command. | Reopening a conversation without a new command. |
| Protect | Keep a participant or site within a supported safe condition during a route. | Existing health/roster/expedition authority supplies the result. | A narrative line claiming survival without an encounter result. |
| Craft/repair | Perform a supported recipe, repair, or interaction. | Crafting/item/equipment owner emits the result. | Owning ingredients or selecting a response. |
| Discover | Find or learn a stable location/clue. | Map/discovery authority changes its knowledge state. | A hidden location being selected internally. |
| Wait/survive | Reach a time or environmental condition. | Clock and relevant needs/hazard owner report the condition. | A dialogue scene merely advancing a deadline. |
| Negotiate | Choose a supported term with another actor/faction. | Quest/faction owner accepts the selected outcome. | Showing a choice or reputation-flavored line. |
| Resolve | Settle an authored question through a valid terminal path. | Quest owner confirms one defined result. | Treating missing evidence as success. |

An objective record should bind intent, evidence kind, evidence owner, expected target ID, optionality, repetition policy, and completion copy. Multi-part objectives should state whether evidence is ordered, independent, or mutually exclusive. If an existing owner exposes only an aggregate status, content should use that supported granularity rather than manufacture sub-objective saves.

### Quest dependency graph

Represent authored dependencies as a directed graph for review:

- **Prerequisite edge:** one confirmed quest outcome enables another offer.
- **Evidence edge:** a clue or interaction is consumed by an active objective.
- **Alternative edge:** either of two proof sources can satisfy the same authored requirement.
- **Follow-up edge:** a failed/complete task offers a distinct next task.
- **Callback edge:** a later scene reads a prior owner-backed fact but does not require it.
- **Exclusion edge:** two mutually exclusive outcomes prevent incompatible content.

Each edge names the producer, consumer, stable IDs, and absence behavior. Prerequisite cycles are rejected unless the current quest owner explicitly supports a deliberate repeatable loop. Callback edges should usually be optional; otherwise a side quest becomes an undocumented critical dependency. An alternative edge is valid only when both sources prove the same statement.

For the Unentered Shelf, the record inspection and witness account can be independent evidence nodes; route inspection is an optional lead; the transfer outcome depends on both a valid quest phase and the resource owner; later dialogue is a callback. There is no dependency from “heard the nurse” to “neighbor received supplies.” This graph makes the story's epistemic boundaries testable.

### Quest chain shapes and pacing

Use one of four chain shapes as a planning lens:

1. **Single beat:** one offer, one action, one result. Suitable for a tutorial or local discovery.
2. **Evidence loop:** offer, gather/inspect, compare, resolve. Suitable for investigations and character tasks.
3. **Expedition loop:** preparation, dispatch, encounter/action, return/report. Suitable for location and survival content.
4. **Campaign chain:** prerequisite, multiple chapter tasks, branch, consequence, later callback. Suitable for main/faction arcs and high-cost expansions.

The chosen shape drives production cost and QA. A single-beat task still needs refusal and restore behavior if stateful. An evidence loop needs at least one route when an optional source is missing. An expedition loop must account for stale preview/start state. A campaign chain needs a dependency graph and stable terminal outcome mapping. Do not stretch a short task to several expeditions just to increase word count or apparent scale.

### Failure taxonomy

Separate failure causes because recovery behavior depends on who owns them:

- **Player-decision failure:** the player declines or chooses a mutually exclusive outcome. Record only through the quest owner and show a clear consequence preview.
- **Deadline failure:** the clock/quest owner records expiry. Keep the original goal in history and offer recovery only if authored.
- **Evidence failure:** a source is contradicted, destroyed, or insufficient. Preserve the evidence result and expose another proof path or unresolved close.
- **Availability failure:** actor, location, route, or expansion content is absent. Do not charge the player with failure; delay or substitute under authored rules.
- **Resource failure:** an input is missing or transfer is rejected. Keep the objective open or offer an existing alternative recipe.
- **Operational failure:** an owner cannot process a valid command. Preserve the scene and retry/exit safely; never turn it into an in-world failure.
- **Intentional abandonment:** only an explicit owner-confirmed player action counts; inactivity is not abandonment.

The journal can use concise language while the quest record retains the exact owner evidence. This reduces ambiguous “failed” states and prevents a route defect from being reported as a player's mistake.

### Acceptance gates for a quest family

Before a category joins a release, the reviewer should answer:

1. Does it use a player verb supported by a current interaction or an approved implementation claim?
2. Does it have a source of truth for progress, failure, and rewards?
3. Does every mandatory location resolve through Plan 18?
4. Do condition branches preserve a baseline route through Plan 21?
5. Do response effects use owner commands from Plan 22?
6. Does Plan 19 identify authored versus generated facts and stable IDs?
7. Does Plan 20 have a reachable graph, localization keys, and transcript-safe prose?
8. Can a fresh save enter the route and a restored save observe its accepted result?
9. Is there a clear outcome if optional content is absent?
10. Has the narrative collision check identified a distinct purpose?

Mark each answer proven, not applicable with explanation, or unresolved. Any unresolved critical route, effect owner, or save path blocks implementation. A proposal may remain useful while blocked, but its integration status must say exactly which seam is missing.

## Continuation pass 8 acceptance

The objective package is ready when player verbs map to observable evidence, dependency edges distinguish prerequisites from callbacks and alternatives, failure cause is attributed to the proper owner, and each chosen quest-chain shape has matching save, fallback, dialogue, map, and result-copy coverage.

### Rotating and repeatable quest eligibility

Repeatable tasks should enter through an existing candidate-generation owner, not a new daily-board authority. An authored template defines valid objective modules, allowed locations, requirements, failure behavior, reward bounds, and whether the same player can hold more than one instance. The current generator determines whether a candidate can be offered from canonical world facts and the game's seeded random stream.

The candidate must have a stable instance identity for its accepted lifetime. Its template ID, generation inputs, and owner-managed outcome must be sufficient to restore it. Do not recreate a completed instance under the same ID, or use a fresh random draw after load to replace its active objective. If the current quest owner cannot safely persist the candidate, keep generation to offer-time and create a normal canonical quest instance only after acceptance.

Eligibility should be deterministic and explainable: active instance cap; prior completion/cooldown if the owner tracks it; location/actor availability; campaign phase; resource feasibility; and optional-content presence. If no template qualifies, show no offer or an authored empty state. Do not generate a fallback that violates objective or reward bounds merely to keep the board populated.

Treat “rotating” as content availability changing at a defined owner event, not as a wall-clock reroll. The game clock or event owner decides when a window starts and ends. A quest accepted before the window closes follows its declared completion/expiry rule; it is not silently deleted by the next rotation.

### Hidden-quest entry and anti-stranding rules

A hidden quest needs a discoverable signal that can be perceived through supported modalities, a stable clue/discovery fact, and an optional-task classification. If the player misses the clue, the base campaign remains complete. If the clue's presentation asset is missing, text or an alternate interaction should preserve essential discovery where the content is intended to be accessible. Skill or faction gates may add interpretation or shortcut, but the secret cannot be the sole path to a required system tutorial or ending prerequisite.

Review hidden-task copy at two knowledge levels. Before discovery, ambient text must not reveal a precise objective or promise a reward. After discovery, the journal can state what the player actually learned and what remains unverified. If an old save contains a quest reference but not the newer discovery fact, use the save owner's migration/fallback rule rather than guessing that the player found the clue.

### Journal language by lifecycle projection

Journal text should distinguish an offer from an accepted task, an objective from an outcome, and a failure from a delay. For an offer, say what prompted it and what the player can choose. For an active task, name the next supported verb and any known prerequisite. For a blocked task, identify the real owner-backed condition and whether waiting, a clue, or an alternate route can change it. For partial progress, list confirmed evidence separately from unresolved claims. For failure, preserve cause and any recovery link. For resolution, state the accepted disposition and any remaining uncertainty.

Do not manufacture a new journal status store to achieve this language. Derive the read model from the quest owner's state and objective facts. If the current journal only supports a smaller set of labels, use concise explanatory copy inside that model; do not make the UI's wording appear more certain than the owner's underlying state.

## Continuation pass 9 — candidate-to-instance flow, objective contracts, and quest portfolio gates

### Candidate, offer, instance, and history are different records

Keep four moments distinct in quest content and implementation:

1. **Template eligibility:** a current owner/generator says an authored definition may be presented in this campaign. This is a temporary candidate, not an active quest.
2. **Offer presentation:** the player has enough knowledge to understand the offer. Showing the offer does not create an accepted instance.
3. **Acceptance:** a confirmed request creates or activates one canonical quest instance through the current quest authority.
4. **Outcome history:** the owner records completion, failure, expiry, abandonment, or supported resolution. Revisiting a conversation cannot create a new instance or repeat its reward.

When the player declines, determine whether the offer can be repeated, is permanently refused, or remains available later; that policy belongs to the quest definition and current quest owner. When the player defers, do not create a half-active task unless the owner has a distinct supported state for it. When the generator presents several candidates, accept only one stable definition/instance reference at a time through the current API.

Existing source evidence identifies QuestRuntimeCoordinator with a compact persistent state set and separate template/candidate generation concepts. Promotion should map these moments to those exact APIs. Do not add a competing global state enum for every editorial label. If an owner cannot represent the intended behavior, simplify the offer semantics or submit the smallest architecture decision.

### Instance and objective identity

An accepted quest instance needs a stable owner-defined identity; each objective needs a stable definition reference and a current evidence source. If an authored task can be accepted repeatedly, distinguish separate instances without deriving identity from localized title, list position, system time, or random GUIDs. Use the project's current deterministic ID/session contract and confirm restore behavior before release.

When an objective is repeated, decide whether the evidence is one-shot, cumulative, latest-result, or a set of unique targets. For example, inspecting the same record twice should usually remain one inspection; delivering three items should use the inventory/resource owner's current amount semantics; visiting three sites should use stable site IDs and not count a revisited site twice unless the quest explicitly asks for repeated trips. Do not add local integer counters that become inconsistent after restore or item transfer.

Objective text can be authored in stages: instruction, progress, blocked reason, completion acknowledgement, and failure/recovery explanation. The current quest/journal authority should choose which text key is relevant from canonical progress. A dialogue line may add context but cannot replace the objective record.

### Deterministic generated offer contract

When a template candidate comes from an existing narrative/quest generator, record the eligible template set and the owner facts that filtered it. A repeated call during the same offer window should not produce a different visible candidate merely because the panel was redrawn. If a new dispatch or world event is intended to change candidates, use the established seeded stream at that owner transition.

Template constraints should include:

- allowed quest family and objective module;
- compatible location/exploration types;
- required actor/faction presence or approved absence route;
- minimum and maximum objective count;
- supported failure/recovery semantics;
- bounded reward request and duplicate policy;
- campaign phase and active-instance cap;
- whether the player may decline and see another candidate;
- whether an accepted task survives template retirement.

Generation selects within these constraints; it does not author arbitrary new consequences. If every candidate fails its preconditions, report that no offer is available. Never return an invalid fallback that quietly violates objective type, location requirement, or reward bounds.

### Quest portfolio progression and pacing

Plan a release portfolio across campaign pacing, not only category counts:

| Campaign interval | Content role | Example system proof | Branch budget |
|---|---|---|---|
| Early shelter | Teach an existing interaction and readable journal state. | Offer → accept → inspect → report. | One short branch, mostly reconvergent. |
| Early exploration | Show map knowledge versus travel readiness. | Discover clue → candidate opportunity → preview/start. | Optional discovery path with baseline. |
| Mid-campaign | Make resource/character decisions matter. | Existing inventory/recipe or relationship owner accepts result. | One meaningful local outcome plus callback. |
| Mid/late faction arc | Present terms, access, refusal, and separate follow-up. | Faction owner controls standing/access; quest owner tracks task. | Distinct terms but no arithmetic in dialogue. |
| Late campaign | Resolve cross-quest commitments and prepare an ending. | Campaign owner reads canonical quest/world facts. | Carefully audited, bounded irreversible choices. |
| Post-resolution | Let the player revisit settled locations and people. | Owner result drives world/map/journal callbacks. | No replay of one-shot operations. |

This distribution teaches systems before asking players to combine them. A late-game major faction or ending can be proposed only after the current canon and expansion plans are audited; this plan's category table does not authorize a new faction or campaign layer.

### Quest acceptance and refusal copy contract

An offer scene needs three distinguishable responses where the quest owner supports them: accept now, ask for information, and decline/defer. Acceptance explains the task and known commitment before sending its command. Information questions do not mutate quest state unless an owner-backed clue interaction is actually completed. Decline copy avoids implying that supplies moved, reputation changed, or future content was permanently removed unless the owner confirms that policy.

The journal and dialogue should agree on offer state after closing and reopening. A duplicate accept request returns the existing instance or an owner-defined AlreadyApplied result. If an offer expires while open, the owner rejects the stale acceptance and the scene refreshes to a current option or neutral exit. Acceptance is complete only when a fresh save can restore the owner-confirmed instance.

### Portfolio review and production stop rules

For a wave of quests, report unique templates, accepted instance modes, objective evidence types, required map IDs, new scene graphs, owner command families, failure/recovery paths, localization strings, and save fixtures. Halt the wave if several quests depend on one unverified shared owner seam, if mandatory locations exceed the authored budget, or if all quest families repeat the same dramatic question and player verb.

This review can reject or defer content without deleting useful prose. Keep unsupported scenes in the planning bundle with their unverified fields marked. A content batch becomes integration-ready only when each accepted instance can be traced from candidate through terminal result under current quest/save authority.

### Validation fixture: transition replay and owner receipts

The transition fixture should be a sequence of owner observations, not a hand-authored list of expected labels. Begin from a clean campaign snapshot, query eligible definitions, create one offer, accept it, advance one objective, save, restore, and query again. The journal projection after restore must match the accepted quest facts and objective evidence. Reopening the dialogue must not create a second instance or repeat a completed request. This sequence covers offer eligibility, stable identity, objective evidence, save ownership, and read-model refresh without adding another quest-state store.

Add targeted branches to the fixture rather than cloning a full campaign for every variation:

| Variant | Fact changed | Expected owner result | Player-visible proof |
|---|---|---|---|
| Evidence unavailable | Required witness or location fact is absent. | Instance remains actionable or has an explicit blocked reason. | Journal names what is missing and a supported retry or alternative. |
| Evidence arrives late | A valid clue arrives after a temporary route closes. | Task follows the authored recovery route; the expired route stays expired. | New route is distinct from the missed opportunity. |
| Partial transfer | Resource owner accepts less than requested. | Progress reflects only the accepted amount or a typed rejection. | Dialogue and journal show the remainder without claiming full delivery. |
| Competing evidence | Two valid events arrive in one update interval. | Each stable evidence identity is processed once in defined order. | Both accepted facts appear, or a deterministic reason explains rejection. |
| Stale acceptance | Eligibility changes while its panel stays open. | Owner rejects or refreshes the stale command. | Current option appears and no hidden commitment is recorded. |
| Duplicate completion | Same response or event is submitted again. | Prior result returns or duplicate is harmlessly rejected. | No extra reward, relationship change, or world mutation appears. |
| Follow-up after resolution | A supported sequel becomes eligible. | New stage or linked instance appears; original remains terminal. | Journal retains the resolution and names the new commitment. |

Keep a receipt for each owner operation: source instance, evidence ID, command, target, request identity, acceptance or rejection, and resulting canonical state. The receipt is a review artifact unless a current production owner already exposes a durable history contract. Do not turn it into an unofficial save record or a second audit ledger. It lets reviewers distinguish an objective bug from a presentation refresh bug: if the owner accepted evidence but the journal is stale, the repair belongs to the existing projection/event seam; if the owner rejected it, dialogue cannot invent success.

The fixture should also demonstrate that a blocked objective is recoverable in a way the player can understand. A blocker needs an owner fact, a reason code, an expected refresh trigger, and either a next action or an honest wait condition. “Come back later” is only useful when the content tells the player what may change or where they can continue meanwhile. If no change can clear the blocker, the route is not a temporary block; it needs a failure, deferral, or alternate resolution authored before release.

### Content proof package for a release candidate

Before a quest enters an integration wave, provide a compact package linked by stable content IDs: the definition and references, objective-to-evidence mapping, state/branch diagram, normal transcript, recovery transcript, localization inventory, and the owner receipt fixture. Include the source revision or catalog version used for review so that a later prose-only correction can be distinguished from a semantic change. Do not paste runtime state dumps or player save data into this package.

The reviewer should answer five questions without reading every node: what does the player believe at entry; what fact makes each objective true; who accepts the result; what remains possible after failure or uncertainty; and how does the player see the final result after restore? If an answer depends on an unverified API, keep the quest at proposal status and identify the missing contract. One line or location already present in content does not make the entire quest integrated.

For larger quest sets, group fixtures by shared objective contract and include at least one boundary case per group. Several inspection objectives can share a fixture only when they use the same evidence producer, duplicate policy, and rejection semantics. Delivery, discovery, escort, timed survival, and relationship tasks remain distinct when their proof and failure routes differ. This reduces review repetition without hiding a risky behavior behind a broad happy path. The package should name both what was exercised and what was explicitly deferred, especially when a quest's branch graph contains unsupported faction, ending, or optional-package results.

### Quest tranche closeout criteria

Close a content tranche when every accepted quest has an owner-backed entry path, stable instance reference, objective evidence definitions, explicit outcome or recovery path, and restore/revisit description. Mark each unsupported branch as deferred at the response or objective level; do not describe the whole quest as playable when only its opening scene works. The review note records unresolved owner contracts, required location references, duplicate story-function checks, and the next smallest integration slice. If one blocker affects several quests, raise one shared dependency for the foreman instead of scattering guessed workarounds across definitions.

## Continuation pass 10 — questline topology, episode pacing, and multi-thread delivery

### Separate prerequisite edges from story references

A campaign quest graph contains several relationships that look like “links” but have different gameplay meaning. A prerequisite edge controls whether an offer may appear. An objective edge says which evidence advances an accepted instance. A callback edge selects later dialogue after an outcome. A thematic reference connects stories that share a setting or character but does not gate either task. These edge kinds must remain distinguishable in authored review and validation output. A quest should not become unavailable merely because an optional callback target was removed.

Use an explicit edge contract in the content packet:

| Edge kind | May block offer/progress? | Required evidence | Removal behavior |
|---|---|---|---|
| Critical prerequisite | Yes, only when the campaign explicitly requires it. | A canonical fact accepted by its owner. | Provide a core route or stop the dependent quest from release. |
| Objective evidence | Yes, for the stated objective only. | A typed event or accepted owner result. | Keep the objective blocked with an authored alternate or delay. |
| Optional invitation | No critical progression block. | Current offer eligibility. | Omit the offer or show a neutral fallback. |
| Callback reference | No; it changes a later response or journal detail. | Prior accepted result or memory fact. | Use the baseline line when the optional source is absent. |
| Thematic relationship | No runtime gate by itself. | Editorial approval and collision review. | Remove the cross-reference without changing playability. |

This separation supports a main storyline with character, faction, investigation, and discovery threads without requiring the player to finish every side story. It also makes critical-path review possible: follow only blocking edges from campaign start to resolution, then prove that each node on that path can be offered, reached, and completed using core content and current owners.

### Thread map across campaign phases

Plan a multi-thread release as a set of short arcs with different player verbs and different reasons to revisit the world. The table below describes production roles rather than new canon or content IDs:

| Thread | Early appearance | Mid-campaign development | Late callback or resolution | Optionality rule |
|---|---|---|---|---|
| Main campaign | A concrete shelter or expedition problem. | Evidence changes which solution is credible. | One authored campaign resolution reads accepted outcomes. | Required steps have a known accessible route. |
| Character | A person's practical request or hesitation. | Their interpretation shifts after player-backed events. | A callback recognizes the player's accepted or refused commitment. | No character ending blocks core survival progression. |
| Faction | Terms, access, or an unresolved claim. | The player can negotiate, delay, or decline with clear limits. | Current faction owner supplies standing/access facts to campaign closure. | A non-faction route exists for essential information. |
| Discovery | An environmental clue with observable detail. | Separate clue from the player's interpretation. | A later scene explains what the clue did and did not establish. | Secret evidence is not the only critical objective proof. |
| Investigation | Conflicting sources and an evidence question. | Player can compare sources or pursue a corroborating site. | Owner-confirmed conclusion selects a result or a known-uncertain closure. | Missing one optional witness does not dead-end the case. |
| Rotating world task | A short, repeatable contribution. | Current world/resource facts change eligibility. | Journal history records the accepted result without replaying it. | Rotations cannot crowd out required campaign tasks. |

Each thread needs one introduction, one distinct development beat, and one closure or honest open-state callback before it contributes to the campaign slate. A theme alone is not a second story arc: if two tasks share the same character, destination, evidence, choice, and result, combine their production or make their gameplay distinction explicit.

### Episode pacing and branch convergence

Shape a long quest chain as episodes that can each return control to ordinary play. A useful episode contains an arrival or contact, a player-readable question, one or more evidence paths, an explicit decision or confirmation, and a close that names what is still unresolved. The player should not need to keep a dialogue graph open while waiting for an unrelated world event. A blocked task can remain active in the journal while the player pursues another task; its resumption rule comes from its owner facts.

| Episode position | Player question | Evidence or action | Convergence decision |
|---|---|---|---|
| Hook | Why should I care now? | Witness, alert, discovered object, or accepted request. | Create an offer only after its owner says it is eligible. |
| Inquiry | What can be verified? | Inspect, travel, ask, craft, protect, or compare records. | Rejoin after different ways produce equivalent evidence. |
| Interpretation | What does the evidence mean? | Contrast source quality and uncertainty. | Keep separate only if the owner records a different conclusion. |
| Commitment | What am I choosing to do? | Confirm a target, cost, or relationship/faction term. | Branch for a real owner-backed consequence. |
| Return | What changed? | Query the owner result and any current world projection. | Retain the settled result; offer a new task only through new eligibility. |

Branch convergence is a production decision, not a promise that every path has identical flavor. Reconvergent routes can preserve a knowledge marker or an approved response variant while sharing the next objective. A non-reconvergent route must list its unique downstream scenes, journal copy, reward treatment, map/location consequence, and ending input. Before adding a new branch, estimate the number of future callbacks that must acknowledge it; if the team cannot support those callbacks, choose a narrower local result or defer the route.

### Shared dependencies and multi-quest handoffs

Two active quests may ask the player to visit one site or speak with one character. They may share the physical expedition opportunity only if each objective still accepts its own evidence contract. The location arrival event can be common; inspection, conversation, delivery, and interpretation can remain separate actions. Do not make the second quest complete because the first quest's dialogue happened in the same room.

When one quest produces evidence another can use, name the stable evidence kind, source owner, visibility level, and whether re-use is allowed. A later quest may reference an already learned public fact if the current knowledge owner can prove it. It must not copy the old objective's progress counter. A private character memory may qualify a conversation variant without satisfying a public investigation objective. This protects both narrative continuity and fair quest expectations.

If a shared dependency becomes unavailable, evaluate each consumer separately. One quest can delay while another takes its substitute. A faction branch can close while a personal task remains available. A common location can be visible but unreachable, with distinct objective fallbacks. The content review lists all consumers of a shared node and verifies that disabling one optional bundle does not collapse the critical quest graph.

### Portfolio capacity and production investment

Choose a release portfolio by complete playable arcs rather than by raw quest count. For each proposed quest, estimate authored locations, speakers, unique scene graphs, new objective proof kinds, command owners, generated variants, localization load, outcome callbacks, and focused fixture families. Count reuse only when the same contract genuinely applies. A common objective module can lower implementation cost while a new character voice and multi-ending callback still carry writing and continuity cost.

Use three practical investment bands. A **small task** reuses a current offer route, one objective contract, one primary location, and one result. A **medium arc** adds a reconvergent evidence path, a character/faction response, and one later callback. A **large storyline** crosses several existing authorities, changes multiple later scenes, or contributes to campaign resolution. Large storylines require a graph review, critical-path analysis, package-disabled fallback, old-save scenario, and explicit continuation budget before they enter implementation planning.

If a slate contains too many unique mechanics, reduce the number of simultaneous story threads instead of spreading a thin partial implementation across all of them. If the writing volume exceeds review capacity, freeze new branches and finish the baseline/recovery transcripts. The useful measure is the number of complete and verifiable player routes, not how many quest titles have been drafted.

### Critical-path worksheet for an expansion slate

For each campaign milestone, write one row describing the minimal route, then annotate optional additions separately. The worksheet should show the start condition, required owner fact, evidence producer, accessible location path, fallback, and terminal/continuation result. A milestone that depends on a rare site, hidden character, optional dialogue package, or unreleased faction has failed the core-route check unless the current campaign explicitly accepts that dependency.

| Milestone | Critical route | Optional enrichment | Required review |
|---|---|---|---|
| Introduce the campaign question | One available quest/contact or authored discovery. | Additional rumor source or companion reaction. | Ensure optional source absence leaves a coherent hook. |
| Establish evidence | At least one legal evidence-producing route. | Alternate witness, skill analysis, or hidden context. | Prove the objective consumes owner-confirmed evidence. |
| Ask for commitment | One clear player choice with cost and uncertainty. | Tone variant or optional negotiation question. | Revalidate exact command preconditions at confirmation. |
| Record the result | One accepted, failed, expired, or explicitly unresolved result. | Map/character callback and optional reward. | Preserve result after save/restore and scene revisit. |
| Continue or close | Authored next task or supported terminal resolution. | Follow-up thread if eligible. | Prevent automatic re-open or duplicated reward. |

This worksheet catches a campaign graph that looks connected but has no complete player route. It also helps writers distinguish optional detail from required proof and gives integration a small acceptance target before the full slate is authored.

## Continuation pass 11 — quest definition contract, objective composition, and portfolio production kit

### Semantic groups for an authored quest definition

The exact JSON schema belongs to the live data authority. This proposal separates the questions a definition must answer so future schema work can reuse current catalogs and avoid one unstructured blob of prose, logic, and duplicated state. A definition should be reviewed in semantic groups; not every group becomes a new runtime field.

| Definition group | Authoring questions | Runtime consumer or owner to verify |
|---|---|---|
| Identity and revision | What stable content ID and compatibility revision identify this quest? Is it permanent content, a template, or a linked sequel? | Current quest/catalog loader and save reference owner. |
| Presentation | What title, summary, giver, opening text, journal key, and closure text appear in each locale? | Existing quest log/dialogue/localization path. |
| Offer eligibility | Which campaign phase, actor, faction, discovery, or prerequisite fact can make the offer available? | Current quest generator and each referenced fact owner. |
| Acceptance contract | What commitment does acceptance create, and what can the player still decline or ask before committing? | Existing accepted-quest command/state path. |
| Objective graph | Which typed proof is required, optional, alternative, or ordered? | Existing objective representation and evidence producer. |
| Location requirements | Is the task tied to a specific canonical site, a class of evidence, or a clue-only lead? | Map and expedition owners; Plan 18 review. |
| Outcome semantics | What is completed, resolved, failed, expired, abandoned, or continued? | Current compact lifecycle API and campaign/quest owner. |
| Reward request | What owner-confirmed item, access, relationship, or information result may be requested? | Existing inventory/faction/relationship/campaign owner. |
| Recovery and compatibility | How can this route continue after blocked progress, retired content, or an old-save reference? | Current owner migration/restore contract. |
| Editorial provenance | Who authored it, which bundle owns it, what is provisional, and which callbacks consume the result? | Content review package; not a campaign save field. |

The group list is useful even if a current definition combines several fields. Before proposing a schema change, find the existing representation and show exactly which question it cannot express. A missing design detail is not a reason to add a mutable flag; it may be a content note or a derived read-model label. Conversely, if a durable fact cannot be reconstructed after load, name its canonical owner and migration requirement before authors depend on it.

### Objective composition rules

Complex quests need clear logic for how evidence combines. Specify the relationship between objectives in plain authoring language and confirm that the current quest authority can represent it. Do not infer ordering from list position unless the existing data contract defines ordering. Do not interpret all objectives as mandatory when an authored alternative is intended.

| Composition | Meaning | Example use | Review rule |
|---|---|---|---|
| Ordered sequence | Objective B becomes actionable after accepted proof for A. | Find a route clue, then travel to the revealed region. | Explain why B was previously unavailable; preserve A after restore. |
| All required | Each listed proof is needed for the primary outcome. | Confirm two independent resource stores. | One blocked proof keeps the whole result open but does not erase completed proofs. |
| Any-of alternatives | Any one approved evidence source can satisfy the requirement. | Corroborate a claim with either a record or witness. | Each branch yields equivalent objective meaning; unknown is not success. |
| Parallel work | Several tasks can progress in any order. | Prepare food and repair a shelter boundary. | Each owner event is idempotent; aggregate result recomputes from canonical evidence. |
| Optional objective | Adds a bonus or detail but is not required for the core resolution. | Find a second record that changes a callback. | Player-facing labels distinguish optional completion from required progress. |
| Fail-forward substitution | A failed approach activates a different proof route. | A missed interview is replaced by a physical record. | Original failure remains in history; the substitute has its own owner proof. |

For any-of groups, document why the evidence sources are interchangeable at the quest level. A witness may establish presence but not delivery; a signed record may establish a recorded transfer but not who physically carried it. If the narrative conclusion differs, the paths are meaningful branches, not simple alternatives, and the owner must preserve that distinction. For parallel work, never use a local sum of boolean callbacks when the individual owners can report accepted objective facts.

Avoid cyclic prerequisite/objective graphs. A quest cannot require its own unresolved result, directly or through another task, unless a bounded loop is an intentional repeatable activity. A repeatable loop is usually better represented as fresh accepted instances from a stable template than by resetting a terminal instance. Validation should identify strongly connected prerequisite chains, unreachable objectives, optional goals that accidentally gate completion, and branches with no terminal or supported continuation.

### Evidence and objective contract extension

For every objective, record the event producer, accepted evidence identity, duplicate policy, target scope, accumulation mode, source quality, and player-facing acknowledgement. Use event IDs that are stable under save/load and independent of localized prose. Define whether repeated evidence is unique-target, cumulative, latest-value, or one-time. If the current owner cannot provide a stable event or target, simplify the objective to a supported observable interaction rather than counting dialogue visits.

Progress must be recoverable from its owner facts. An objective such as “visit three distinct shelters” should use the canonical shelter identities and accepted visit evidence. “Deliver five units” should read accepted resource-transfer results, not an inventory snapshot after the player may have used some items elsewhere. “Convince the contact” must identify the relationship/faction/quest result that demonstrates acceptance; displaying a persuasive response is not proof.

If one event could satisfy multiple objectives, define whether evidence is reusable and whether each objective requires a different target. A single inspection can legitimately serve two tasks when both are about the same public fact and each owner accepts the source. It must not satisfy “inspect the east room” and “inspect another room” merely because both listen to a generic inspection callback. A shared source fact may provide context, while separate objective requirements maintain the task's distinct scope.

### Outcome packet and reward boundaries

An outcome packet describes the task's result before any reward is implemented. Separate: objective outcome; explicit player choice; reward request; owner acceptance; later callback; and campaign-resolution contribution. A primary task may complete with no item reward. An optional discovery may provide information but no inventory change. A negotiation may grant access only if the faction authority accepts its command. The quest definition asks for effects; it does not write directly to those owners.

| Outcome case | Quest-facing result | Reward/effect handling | Journal language |
|---|---|---|---|
| Full primary route | Completed when all required proofs and result are accepted. | Submit each allowed reward request once to its owner. | State verified outcome and any remaining uncertainty. |
| Optional evidence missing | Primary route may still complete if authored that way. | Omit bonus; do not invent a penalty. | Show optional evidence as missed/unknown only if useful. |
| Partial proof accepted | Keep active/blocked or resolve through an authored partial route. | Grant only effects whose preconditions succeeded. | Separate confirmed facts from outstanding work. |
| Alternate fail-forward route | Record primary failure and alternate progress. | Use the alternate route's reward/effect contract. | Explain why the path changed without calling failure success. |
| Explicit unresolved resolution | Owner records a supported terminal/settled outcome. | No reward is implied unless separately accepted. | Tell the player the investigation is settled as unknown, not solved. |
| Abandoned task | Owner accepts abandonment or existing semantics apply. | Apply only the documented cancellation behavior. | Preserve history without claiming deadline failure. |

Avoid a generic reward field that mixes quantities, faction access, relationship, and ending observations as arbitrary strings. Each reward has a typed owner and a scope-specific result message. If the same owner handles quest completion and reward, the command/result contract still needs duplicate behavior. A repeated completion dialog should not grant rewards twice.

### Expanded production cards by quest family

Use this matrix to check that content families differ by proof and player activity, not just their category label:

| Family | Primary player verb | Evidence owner boundary | Main failure/recovery question | Cost band tendency |
|---|---|---|---|---|
| Main quest | Investigate/decide/advance. | Campaign and quest owners accept milestone proof. | Does the core route remain accessible? | Large when multiple outcomes feed resolution. |
| Character quest | Ask/help/return. | Quest and relationship owners distinguish promise from result. | Can the player decline without losing core instruction? | Medium with recurring callbacks. |
| Faction quest | Negotiate/serve/withdraw. | Faction authority controls standing/access; quest tracks task. | What if contact is absent or terms are refused? | Medium/high across package and campaign edges. |
| Discovery quest | Notice/inspect/reveal. | Map/discovery owner records knowledge; quest accepts evidence. | Can a clue be missed yet the task remain possible? | Low/medium, high if it opens critical content. |
| Investigation | Compare/corroborate/conclude. | Evidence source and quest outcome owner remain distinct. | Can an honest unknown resolve the task? | Medium/high due to branching transcripts. |
| Escort/protection | Prepare/travel/defend. | Expedition/encounter owner proves arrival/survival. | What alternate exists after interruption or loss? | High when simulation state is involved. |
| Survival | Endure/prepare/recover. | Needs/health/time owners provide thresholds and events. | Is failure fair, legible, and recoverable? | High; do not invent pressure counters. |
| Crafting/resource | Gather/produce/transfer. | Inventory/recipe/resource owners report accepted operations. | What if the player spends or transfers items elsewhere? | Medium; high if economy balance changes. |
| Timed | Prioritize/act before owner deadline. | Existing clock/deadline authority records expiry. | Can the player see time pressure and the after-state? | High because clock/save behavior matters. |
| Rotating | Choose/complete/repeat. | Generator eligibility plus quest instance owner. | What prevents duplicate and runaway offer volume? | Medium/high at scale. |
| Hidden/environmental | Discover/interpret. | Discovery evidence is recorded before offer/acceptance. | Is there a clue-led fallback or is it truly optional? | High if hidden route affects critical progression. |

Every card also states whether it belongs in the core game or an expansion. Core content teaches the minimum path and all required verbs. Expansion content may deepen the same owner contracts, add optional places or story branches, and contribute supported campaign facts. Expansion-only authorship cannot become a silent dependency for the core objective unless the game explicitly supports that package as required.

### Repeatable and rotating content production safeguards

Repeatable content has two identities: the immutable template and each accepted instance. A new instance uses a deterministic stable reference from the current generator/quest contract; it does not mutate a prior completed instance. Eligibility reads current world facts, campaign phase, active-instance limits, recent content if an existing owner records that history, and reward limits. If any required fact is unavailable, the template is omitted or the offer system reports no eligible task.

Rotating content should be finite and explainable. A rotation can choose from authored templates, but each candidate still declares location needs, objective proof, failure behavior, reward request, core/expansion status, and duplicate policy. Avoid combining high-volume rotations with unique named-character arcs or irreversible faction choices unless the generator has an explicit authored instance model. Generated repetition is appropriate for small contributions; major character and campaign story beats remain permanently authored.

Offer volume should be bounded by current owner policy, not a dialogue-local cap. Test an empty candidate set, one eligible candidate, several equivalent candidates, all candidates with unavailable locations, an already active instance, a repeated completion, and a save restored after template retirement. The correct result can be “no offer today.” Never backfill with an invalid quest just to keep a board visually full.

### Quest acceptance dossier for the next integration wave

For one representative quest family, the handoff should include its current owner map, conceptual definition card, objective-composition diagram, evidence receipts, player-facing status copy, outcome/reward table, location availability request, dialogue responses, and compatibility cases. Report which existing APIs/data rows were directly verified, which are inferred from earlier source review, and which still need a fresh premise audit. This is a review checklist, not an authorization to implement a new schema or claim code ownership.

The first integration slice should keep the feature narrow: one accepted quest instance, a small objective graph, one evidence producer, a truthful blocked/recovery case, one terminal/continuation outcome, one post-restore query, and a visible journal/dialogue update. Add generated offers, multiple quest families, broad faction outcomes, or high-volume content only after that vertical slice proves the existing owner and save seams end to end.

### Quest journal and active-task workload

The journal is a projection of quest authority and objective evidence. A row should help the player answer: what did I agree to; what is actionable now; what is waiting; what did I finish; and what should I do next? The journal can group Active, Blocked, Partially Completed, terminal, and discovered/offered tasks for presentation, but those labels must map to current owner facts. Do not persist a parallel set of UI statuses to remember which tab the player last used.

| Journal row element | Source | UX rule |
|---|---|---|
| Title and short premise | Authored definition/localization. | Preserve a stable text reference; truncate visually without losing full detail. |
| Current objective | Quest owner plus canonical objective text. | Distinguish actionable work from an optional/complete objective. |
| Blocked reason | Current missing prerequisite or owner result. | Include next action or honest wait condition. |
| Location hint | Quest request and map/discovery owner. | Never expose a secret coordinate before the player earns it. |
| Last result | Accepted/failed/expired/abandoned outcome. | Preserve cause and history; do not collapse unlike terminal states. |
| Follow-up | New eligible offer or sequel reference. | Display only when owner confirms the next task is available. |

Tracking a task is a presentation preference. If the existing UI lets the player pin one or more tasks, the pin can influence emphasis but cannot alter selection priority, quest eligibility, or world state. If a location is hidden, tracking shows the earned clue or region rather than revealing the target. When a task becomes terminal, the pin clears or transitions through the existing UI behavior and the player receives readable feedback.

Active-task volume should be tested with several simultaneous but distinct responsibilities: one main task, a character request, an optional exploration lead, a blocked task awaiting an event, and one completed task with a follow-up. The journal must not lose the main objective under optional content, repeat a reward prompt, or make blocked tasks appear failed. Do not solve an overloaded journal by silently canceling older accepted quests. A visible filter/grouping change is presentation; an actual active-instance cap belongs to the current quest owner and must be explained before acceptance.

### Journal acceptance states

Review the following transitions as player-visible before/after records: no offer to available offer; discovered clue to optional task; accepted instance to first actionable objective; active objective to blocked reason; partial evidence to a remaining objective; owner-confirmed completion to reward/result acknowledgement; failure to alternate route; and terminal result to sequel. Every state must have a clear way to leave the journal and return to the current game loop. Keyboard/controller focus returns to the correct row after refresh, while a removed row causes a spoken/visible notice rather than a silent jump to another task.

The journal should preserve enough copy to explain past decisions without becoming a second transcript database. Use concise owner-backed result summaries for normal play. If the current game has no persistent narrative history store, do not promise a browsable full dialogue archive as part of this plan. Link to the existing quest history path or keep the full transcript ephemeral.
## Continuation pass 12 — integrated Tidemark quest portfolio and fail-forward lifecycle

This pass adds a new, provisional quest portfolio to exercise the lifecycle contracts established earlier. The portfolio is called **Tidemark Compact** as an editorial label only. It introduces no canon IDs, runtime quest manager, faction standing store, water ledger, or campaign flag. Its central question is social and practical: during a dangerous water-cycle change, how can several shelters coordinate access without claiming that one group owns the truth? The playable content should make uncertainty, labor, and consent visible while leaving water quantities, inventory, faction standing, and expedition availability to their current owners.

The portfolio is deliberately broader than one main quest. It contains a mainline, a character thread, a faction negotiation, a discovery thread, a survival request, and a resource/crafting task. Each thread can be played as its own authored package, but their graph includes clear prerequisites, optional branches, and a critical-path audit. The names below are draft labels for planning prose; the eventual data identifiers and canon review remain future work.

### Portfolio intent and player contract

The player hears that the Switchback Sluice has been opened and closed on conflicting reports. One shelter says the upstream gauge predicts a safe release; another says the downstream intake has already turned bitter. The Lowwater Stewards maintain the local mechanism and distrust remote schedules. The High Meridian Assembly is a late-game coalition that offers coordination and record standards, but has no assumed authority to operate local infrastructure. These are provisional faction concepts: the Stewards are patient, practical, and protective of local consent; the Assembly's delegates value comparability across settlements and can mistake procedure for trust.

The portfolio does not ask the player to choose which group is morally correct. It asks them to help establish what is known, decide what can be communicated, and support a safe next step. The game should offer routes for verification, repair, delay, or refusal. No choice claims that every shelter receives equal water, that a faction is permanently trusted, or that a late-game coalition has taken control unless a verified owner records the corresponding fact.

**Player-facing promise:** accepted tasks remain understandable; a missed window changes the route rather than erasing the player's effort; optional proof deepens the negotiation but is not the only route to core progression; and every result that affects play appears in an existing journal, inventory, map, character, quest, or faction read model.

### Quest portfolio cards

| Draft quest label and family | Purpose and typical entry | Required locations | Failure state and continuation | Reward shape and placement |
|---|---|---|---|---|
| **The Gate That Must Not Guess** — main quest, investigation plus location-based | Learn why two public readings disagree and decide whether to verify, repair, or delay the next test. It becomes available from a current core route or a supported discovery clue. | Switchback Sluice is mandatory; one of Rillstep Intake or the marked gauge lookout can provide corroboration. | A missed safe test window records the missed opportunity through the quest owner, then routes to a later inspection or a public “unverified” conclusion. It must not strand the critical path. | Core. Reward is recognized objective progress and an intelligible map/journal update, not a free water grant. |
| **Sena's Handful of Teeth** — character quest, resource/crafting | Help Sena recover or make a safe mechanism part after she explains why the gate cannot be tuned from a guessed reading. The character's precise name and line remain draft. | The sluice workshop or an owner-approved substitute bench; a material source can be optional. | A failed craft attempt may leave the task active with a repair alternative; loss of a required component routes to an existing salvage or trade path if one is available. | Core candidate if the character is in the base narrative; otherwise expansion. Reward is a character callback, practical access clue, or owner-confirmed repair evidence. |
| **A Measure Both Crews Can Read** — faction, timed negotiation | Carry a proposed measurement method to the Stewards and Assembly delegate before the next public planning meeting. The deadline affects the meeting, not the existence of the factions. | A local shelter meeting point and a supported Assembly contact location; either can be substituted by a clue if unavailable. | Expiration changes the meeting to a report-only follow-up. It does not invent betrayal, remove faction access, or close the main task. | Expansion candidate due to cross-faction and campaign callback cost. Any standing/access result requires its current owner. |
| **The Second Marker** — discovery and hidden lead | Find an older marker that explains where the gauge reading was taken. It begins through a physical clue or earned map knowledge, not a mandatory dialogue click. | A discoverable route edge or survey point selected by the existing expedition location owner. | If the site does not spawn, a witnessed mark, report, or equivalent clue can make the discovery available later. The clue gives context without automatically completing the proof. | Optional. Reward is knowledge, new safe wording, and a possible evidence alternative; no direct numerical skill increase is assumed. |
| **One Night Before the Turn** — survival or protection | Keep the night inspection safe while the mechanism is unstable and the local crew decides who can participate. | A supported shelter/safe staging location plus the sluice arrival scene. | If the inspection cannot run, participants withdraw and the objective moves to daylight or a remote report. No companion or shelter resident is silently consumed as a failure cost. | Core or expansion according to cast capacity. Reward is a verified inspection fact and a relationship conversation if the current owner supports it. |
| **A Dry Crate of Useful Things** — resource/crafting, repeatable only if owner permits | Prepare a small tool and seal kit requested for the inspection. The task teaches that readiness reduces risk, not that every expedition can be made safe. | Existing crafting/workbench and a current source for the accepted materials. | If the kit cannot be made, a trade, borrow, or postpone route remains. The player is not charged before the owner confirms the task's requirements. | Optional. Rewards can be a returned reusable tool or better briefing detail only when inventory and equipment owners support them. |

Each card must carry the plan's full authoring fields: purpose, expected duration, acquisition source, required and optional locations, prerequisite facts, accepted proof, visible status changes, branch consequences, failure and recovery routes, rewards, reusability, production cost, and core/expansion classification. The table is a portfolio overview, not a substitute for complete definitions. A task is not ready for runtime merely because a title and hook exist.

### Mainline graph and critical-path invariants

The mainline route is a small graph with one required arrival, two possible forms of corroboration, and multiple honest conclusions:

1. **Offer or discovery:** the player receives a report that readings disagree. The source and confidence of the report are identified in content; the player is not told that the water itself is unsafe before current evidence supports that claim.
2. **Reach the site:** the quest requests Switchback Sluice. Plan 18 determines whether the authored anchor, an equivalent compatible site, or a clue route satisfies expedition selection. The quest owner remains responsible for objective phase.
3. **Inspect the mechanism:** the player may ask Sena, inspect the gauge housing, or first collect The Second Marker. These approaches provide different evidence and dialogue but do not duplicate or bypass current quest proof.
4. **Choose a supported conclusion:** corroborated readings permit a verified report; incomplete readings permit an explicitly unverified report; an unsafe test can be delayed; a repair route can be requested if the existing crafting/inventory owners accept its inputs.
5. **Close or continue:** a current quest result closes the relevant objective. The faction meeting, character callback, and optional resource task can continue only if their own eligibility facts remain valid.

No unique location is required for the player to reach a core conclusion unless the location-selection owner can guarantee it. If a required site is not valid for the expedition, choose one of three authored policies before release: a compatible substitute with equivalent proof semantics, a delayed quest with a visible reason and earned clue, or a safe report-only conclusion. The runtime must never silently leave an accepted quest without a possible route.

Critical-path audit asks whether the player can complete the mainline by:
- finding the marker before the sluice;
- arriving without the marker and asking the local crew;
- arriving when the safe test window has passed;
- failing a craft attempt;
- choosing not to support either faction's proposal;
- having the optional Assembly package disabled;
- accepting, then abandoning, a supporting character task;
- restoring after one evidence source is accepted and another remains incomplete.

A route passes when the player can still learn why the result is uncertain and reach an owner-supported resolution. The main task may conclude with less information; it may not claim the same verified conclusion as the full evidence path.

### Lifecycle mapping and state interpretation

The status vocabulary from the shared quest design remains a projection of canonical quest facts. Map each label to a meaningful player action and an owner-backed event. Do not add one copy of these statuses to the portfolio itself.

| Status family | Portfolio interpretation | Required player-facing detail |
|---|---|---|
| Inactive / Available | The related chapter or offer is not yet eligible / the owner permits an offer. | State the prerequisite or how the lead can be found if that information is safe. |
| Discovered / Accepted | The clue is recognized / the player has explicitly taken responsibility. | Distinguish “heard about it” from “agreed to do it.” |
| In Progress / Blocked | Proof is being gathered / an owner-confirmed prerequisite is missing. | Name the actionable objective or safe wait condition. |
| Partially Completed | Some readings or materials are accepted; at least one declared objective remains. | Preserve accepted evidence and identify what remains. |
| Failed / Expired / Abandoned | The original route failed, the opportunity window passed, or the player withdrew. | Use distinct copy; show a fail-forward or closure route where designed. |
| Reopened / Resolved | An owner permits renewed work / the accepted question has a supported conclusion. | Explain the reason for reopening or the actual conclusion; never replay a closed reward. |

Expiration belongs to a specific opportunity, such as the meeting date or safe inspection window, rather than the entire arc. If timed content is used, record what changes at the deadline and what remains available. The player should be able to tell whether a missed deadline loses only a bonus conversation, a temporary meeting, an optional reward, or the chance for a verified reading. A deadline must not be hidden behind an atmospheric line when it changes an accepted obligation.

Abandonment is a player choice and should not be authored as character betrayal by default. Reopening requires an owner fact or a new explicit offer. Reopened content should keep evidence already recorded when the quest owner supports it, then present only the remaining work. Failed or expired nodes cannot be marked “resolved” by a prose callback that lacks a corresponding owner result.

### Objective and proof contracts for the portfolio

Every objective is phrased as an observable action with a supported proof source. “Understand the sluice” is a dramatic intention but not a sufficient runtime objective. An author can express it as “inspect the named gauge housing,” “receive an accepted report from Sena,” “compare two discovered readings,” or “submit a safe unverified report.” These still require current owner APIs and catalog references to be checked during implementation.

Proof records should answer four separate questions: what action occurred, what evidence supports it, who currently owns that evidence, and which objective may consume it. One action can produce evidence relevant to several threads, but duplicate consumers must be intentional and individually eligible. The Second Marker can inform the main investigation and unlock a character topic without directly completing A Measure Both Crews Can Read. The same clue can be discussed in both scenes while the quest owner still decides its progress meaning.

Before marking an objective complete, content review should check:
- the source is reachable under core-only and enabled-package configurations;
- the evidence reference is stable and does not depend on translated text or row order;
- the proof can be accepted once without granting repeated progress or reward;
- the result remains truthful after save/restore;
- the failure or unknown case does not become false success;
- the player has a visible route from partial proof to the next allowed action;
- the evidence is not simultaneously interpreted as a verified water-quality result and a simple mechanical observation.

If current owners cannot express the distinction between “the gauge was inspected” and “the water is safe,” the proposal must not collapse those states. The quest can still close an inspection objective while leaving the risk claim unverified. The narrative should use the limited fact honestly.

### Cross-thread schedule and interruption policy

The portfolio can create urgency without placing six simultaneous deadlines on the player. Only the faction meeting and safe inspection window are candidates for time-bounded opportunities. The character repair, discovery clue, and optional kit remain available through the arc unless existing world rules say otherwise. Content review should define a schedule card that names:
- what starts the timer;
- which current owner owns elapsed time;
- whether the deadline advances while in a dialogue panel or menu;
- the first player-visible warning;
- whether pausing, save/restore, or fast-forward changes eligibility;
- which partial work survives expiration;
- the next safe route after expiration.

Do not invent a quest-local clock when a current world clock or calendar already owns elapsed time. The quest can ask that owner whether the deadline is open. The selection system decides how an expedition can present a site. The quest should not advance a second timer based on expedition starts unless the game has that explicit semantics.

Interruption examples should appear in the production brief. If the player leaves before the test, the crew secures the mechanism and records no inspection result. If they leave after inspection but before reporting, the evidence remains available to submit through a supported return conversation. If a faction delegate leaves before the meeting, a later appointment can be offered if the content package defines it. If the Assembly package is disabled, its missing meeting does not expire the core inspection. These are content decisions that require current owner paths before production.

### Consequence boundaries and reward policy

A quest reward is a design promise, not a direct effect. Content lists the intended acknowledgement and the domain owner that can fulfill it. The expected reward can be:
- an owner-recorded quest result that unlocks the next eligible node;
- a character callback when a supported relationship or character fact changes;
- an optional expedition clue appearing through current map/discovery behavior;
- a specific item or material transfer only through inventory/crafting ownership;
- an access or standing result only through an existing faction owner;
- a presentation-only acknowledgement that persists nowhere if no durable gameplay claim is intended.

Do not use a resource shortage as a narrative reward to be fabricated in dialogue. If the player contributes a crafted kit, the inventory owner controls whether it is consumed or returned. If a report unlocks a location, the map/discovery owner exposes it. If the Assembly recognizes the player, the faction authority records only the standing/access it currently supports. A character saying “we owe you” is not a mechanical debt unless an existing owner represents it.

Prefer a small reliable outcome over a multi-owner reward bundle that cannot fail safely. If a compound reward is approved, list it as a dependency graph: accepted quest proof; then inventory result if requested; then faction or character read-model refresh; then dialogue callback. Specify the valid partial results. The player should not lose a confirmed task result because a secondary presentation animation or optional expansion line fails.

### Portfolio regression scenarios

The following cases are to be used when an implementation package is later authorized. They are design coverage, not an instruction to add tests during this documentation pass.

| Scenario | Expected quest behavior |
|---|---|
| Main task offered with optional threads unavailable | The core objective remains clear and completable. |
| Clue discovered before quest offer | The owner may surface a valid offer or preserve the clue until eligible; no hidden completion occurs. |
| Required location selected normally | The location entry and quest objective agree on identity and purpose. |
| Required location lacks a valid candidate | Substitute, delay with clue, or report-only conclusion follows the authored fallback. |
| Two proof sources arrive in different orders | The same supported conclusion is reached; no duplicate reward or conflicting status. |
| Player declines the faction proposal | The mainline retains a non-faction route to an honest conclusion. |
| Meeting expires | The meeting opportunity changes, while unrelated quest work remains legible. |
| Resource/crafting task cannot be fulfilled | No unapproved cost is charged; a borrow, trade, or postpone route is offered if designed. |
| Partial inspection followed by save/restore | Canonical accepted evidence remains, and the next objective accurately reflects it. |
| Optional expansion is disabled | Base quest references resolve or use approved neutral presentation; no missing expansion node is required. |
| Quest is reopened after a changed world fact | Only supported remaining work returns; completed proof and rewards do not replay. |
| Panel is closed during a pending owner request | Re-entry queries the owner and never submits a duplicate command automatically. |

### Production-size and placement guidance

The core slice should include the main task, one location, one optional proof source, and one player-readable fail-forward ending. It should not require a new faction system, new campaign authority, newly persistent NPC emotion, or bespoke timed-quest engine. Add the character quest when a verified character/relationship route exists. Add the faction negotiation when the current faction owner can provide access and result facts. Add repeatable tasks only if their current owner supports reset semantics that are clear to players.

The full portfolio is likely an expansion because it needs several locations, a late-game coalition, multiple quest families, additional localization, and cross-owner callback review. A reduced mainline variant can fit core if it uses the base cast and available locations. The content team should estimate authoring, integration, map, dialogue, localization, accessibility, balance, and restore coverage separately. A large transcript is not the only cost driver; every durable outcome adds owner and verification work.

The next implementation review must select a single end-to-end slice, likely The Gate That Must Not Guess with one current quest owner and one existing location. Source inspection verifies the actual objective/evidence API and save route; live governance claims exact files; one focused acceptance target is selected under current test policy. If the required interaction cannot be expressed with current public seams, the integrator records the precise decision gap and keeps the rest of the portfolio provisional.

### Post-resolution closure and open-thread reconciliation

When The Gate That Must Not Guess reaches a supported conclusion, the portfolio must explain what happens to each accepted or optional thread. A main quest closing does not imply that the character task, discovery lead, or faction meeting is complete. Conversely, an open optional thread should not keep the main objective displayed as active after its owner resolved it.

| Thread at mainline resolution | Closure rule | Journal/callback result |
|---|---|---|
| Accepted character repair task remains active | Keep its actual owner status and remaining work. Do not auto-abandon it because the main investigation concluded. | Show it under active tasks with a short explanation that the repair is still optional. |
| Discovered but unaccepted clue | Retain discovery only under its current owner; withdraw the offer if the content contract says it is no longer actionable. | Keep a clue note, not an active commitment. |
| Faction meeting is still available | Preserve it only while the current owner says the offer remains valid. | State whether it can still affect a future procedure or is now report-only. |
| Timed inspection window expired | Close that opportunity while retaining its distinct expired result. | Explain the later daylight or unverified route without reusing completion language. |
| Repeatable preparation task was completed | Offer another cycle only if an existing quest owner explicitly supports repeat semantics and its reset behavior is legible. | Avoid presenting a second cycle as a sequel or new story consequence. |
| Main quest is failed or abandoned | Do not fabricate reconciliation. A supported reoffer can reopen it; otherwise provide a truthful closure if available. | Show why it changed and whether player action can resume it. |

Quest closure should be evidence-backed and proportionate. If an optional thread is no longer useful, a character may say that the meeting already passed; the task owner still determines whether it is expired, resolved, or unavailable. A new conversation line cannot silently erase the accepted task. If a sequel becomes available, its offer must be a fresh eligible transition, not a repeated reward prompt.

The portfolio review should include one full player history in which the main quest finishes early, the player returns to finish Sena's task, and the player later finds the marker. It should also include the reverse order: clue first, side task second, main conclusion last. The visible journal and map should tell a coherent history without requiring every optional branch. Future implementation should prove those cases through current owners and focused verification; this documentation does not assign integration paths.
## Continuation pass 13 — the Farline circuit quest portfolio and rotating operations

This pass opens a second provisional storyline, separate from Tidemark. The editorial label is **The Farline Circuit**. Its central question is how communities communicate route safety after an old signal continues to describe a road that no longer exists. It provides a new major late-game faction candidate, a set of character/faction/discovery/survival/resource quests, and a carefully bounded rotating-operation proposal. All names and beats remain provisional. No faction state, radio authority, quest scheduler, time source, or generated-campaign system is created by this plan.

The late-game faction candidate, **the Farline Compact**, coordinates distant relief routes and argues for a shared signal vocabulary. Local **Lantern Wardens** maintain the short paths between shelters and resist signals whose meaning is unclear on the ground. The Compact is not automatically right because it is larger; the Wardens are not automatically right because they are local. Their positions should emerge from the work and evidence available to each. An existing radio/audio feature can present authored reports if a current data consumer exists, but this content packet does not add a second radio or cue system.

### Dramatic and playable objective

At Siltglass Relay, an old route tone is still being repeated. The tone once indicated that a covered crossing was inspected; the crossing has since collapsed or shifted. The surviving signal infrastructure cannot be treated as a live authority merely because its sound is familiar. A courier named Yara stopped carrying the warning after receiving two conflicting route slips. The Compact wants a reproducible procedure for identifying obsolete signals. The Wardens want a local confirmation step before any public schedule changes.

The player can help determine what is known, identify which route report is stale, and decide whether to support a shared process. The player does not set a new global communications standard by selecting one dialogue line. A quest owner records accepted investigation results; any supported faction owner resolves access or standing; any signal/audio system remains the source for playback; and any campaign owner consumes only facts it currently understands.

### Interlocking quest cards

| Draft title and family | Entry and required play | Location requirement | Failure and continuation | Reward and production placement |
|---|---|---|---|---|
| **The Signal That Outlived the Road** — main investigation, location-based | A route report or environmental cue leads the player to ask why a known tone points toward a blocked crossing. Compare the relay plate with one independently sourced route slip. | Siltglass Relay is the authored anchor; an eligible approach or substitute can satisfy travel, but only the relay plate proves the signal's age. | If the relay is unavailable, the quest becomes visibly deferred with a report/clue route. If one source is missing, conclude “obsolete, source incomplete” only if the quest owner supports that resolution. | Core candidate if the location/cast fit the base campaign; otherwise expansion. Reward: accepted proof and a clear journal/map explanation. |
| **Yara's Last Delivery** — character quest, investigation and personal arc | Yara needs help reconstructing whether she abandoned a delivery or correctly refused to carry an unsafe notice. She is not hiding a villainous act. | Half-Span Shelter for the conversation; either the old path or its public report can provide optional corroboration. | Player can help, decline, or accept partial evidence. A missed meeting changes the available witness route, not Yara's personality or the main quest state. | Expansion candidate due to cast and callback costs. Reward: an owner-supported character result or new dialogue eligibility, never an invented friendship score. |
| **A Signal You Can Trust** — faction quest | The Compact requests a field example and the Wardens request a local safety review. The player can present evidence, ask for a trial, or refuse to speak for either group. | Siltglass Relay plus a Warden notice point or supported Compact contact. | Either group can refuse the recommendation. The main investigation remains completable; refusal changes the faction route or meeting offer only when a current owner records it. | Late-game expansion. Reward: access, recognized report, or callback only through current faction/quest consumers. |
| **Three Knocks, No Answer** — discovery/hidden quest | An environmental clue reveals that a second signal was once used when the main tone could not be trusted. | A discoverable mark near the route edge; may be a temporary visit referencing an authored definition. | If absent from the current expedition, the clue remains a clue and can point toward a later opportunity. No exact target is revealed early. | Optional discovery content. Reward: alternate evidence or a safer reading, not automatic main-quest completion. |
| **Carry the Unlit Cell** — escort/protection | Move a protected signal component with a crew member who is unwilling to power it near the route. The meaningful action is care and safe transport. | Staging shelter and a valid arrival point; never require an unspawnable secret location. | If the expedition must end early, the owner-approved route can return the component, leave it secure, or offer a later transfer. The character is not killed as a punishment for a missed dispatch. | Expansion due to expedition and companion/cast review. Reward: supported item custody or follow-up only through current owners. |
| **Storm Window** — survival/timed opportunity | Inspect an exposed marker during a supported safe window. Timing creates pressure but not a mandatory chapter gate. | A valid exposed route or compatible sheltered observation point. | Window closes into a daylight or report route. The quest owner distinguishes expired opportunity from failed investigation. | Optional. The deadline uses the existing time authority if one supports it. |
| **Hush Kit** — resource/crafting | Prepare a component cover or insulating case using accepted recipe/material contracts. It reduces exposure to a route risk only if an existing owner supports the effect. | Current workbench/crafting location and real material sources. | If crafting is unavailable, the player can ask a Warden for safe handling or postpone. No hidden materials are consumed. | Optional system tutorial/expansion task. Reward is a supported crafted item or description, not a free bonus added in dialogue. |

Quest titles are writer labels only. Before implementation each row needs the full quest definition contract from Plan 17 pass 11: objective semantics, proof owner, terminal/failure path, rewards, locations, availability, volume class, and core/expansion placement.

### Questline topology and valid conclusions

The main quest's critical path is deliberately short: receive a clue/report, reach a valid route to the relay, inspect the plate or acquire an owner-approved equivalent source, then report what the evidence supports. Optional quests add characters and interpretation but cannot own the only mainline proof.

The graph accepts several investigation sequences:
- The player visits Siltglass Relay first, then learns that the road has changed.
- Yara explains the delivery refusal before the player inspects the plate.
- The player discovers Three Knocks, No Answer and returns to the relay with a new question.
- The player reaches the relay after a storm window and accepts a less certain report.
- The player refuses all faction requests but still concludes the main quest with the accepted observation.
- The player never finds the discovery clue and receives no punitive or contradictory ending.

A conclusion needs an epistemic label consistent with accepted proof. “Signal plate is obsolete” can be supported by the plate's date/physical condition. “The road is no longer safe” needs current world/location evidence. “The Compact adopted a standard” requires a faction result. “All Wardens agree” requires broader evidence the plan does not assume. The quest should allow the narrowest truthful conclusion and reserve larger claims for owner-backed later results.

### Lifecycle and dependency edges

A dependency graph may connect the main investigation to the faction quest, but the edge should request evidence or eligibility, not set the destination quest's status by copying a Boolean. Example: a quest owner accepts the plate inspection; the faction offer becomes available if its current prerequisite is satisfied; the faction owner later decides whether a particular report changes standing/access. Failure or expiration in the faction quest does not rewrite accepted plate evidence.

**Required path:** report or environmental clue → available main quest → accepted offer → valid relay approach → accepted observation → truthful conclusion → terminal or resolved owner result.

**Optional threads:** Yara can add a personal account; Three Knocks can reveal a second source; the Hush Kit can support safe handling; the storm opportunity can improve route timing. None is a hidden prerequisite to the required path.

**Late-game edge:** accepted local evidence may be offered to the Farline Compact. This is a new faction quest, not a new objective phase in the main investigation unless the current quest system explicitly models that relationship. If the faction package is absent, no main quest reference points to an unavailable Compact scene.

Define the outcome of each edge:
- **mandatory:** the next main objective cannot start without accepted prior evidence;
- **optional enhancement:** an extra response or side objective is eligible;
- **soft dependency:** the later content can use a neutral or incomplete route;
- **terminal closure:** a current owner confirms no further work is possible;
- **editorial-only:** a callback idea has no runtime eligibility contract yet.

A condition in prose is not an edge. The production graph names source owner, consumer, and failure behavior.

### Repeating route work without a second quest scheduler

A future rotating-operation board may reuse authored templates such as “check the covered marker,” “deliver a route correction,” or “retrieve an insulated component.” These are proposals for repeatable tasks only if the current quest owner already supports repeat/reset semantics. Do not add a parallel board ledger, content rotation service, per-quest cooldown store, or wall-clock timer.

Each repeat candidate should identify:
- a stable authored template and compatible location roles;
- which current owner offers/accepts a new instance;
- the authoritative time/event source, if rotation depends on time;
- a seed or ordering contract for deterministic candidate choice, if candidates are randomized;
- whether an active instance blocks another offer of the same template;
- how a previous result affects the next offer;
- reward source and quantity bounds;
- explicit abandon, expire, and reopen semantics;
- package-off and older-save behavior;
- the player's visible explanation for why the task returned.

A rotation is an offer-selection decision, not a reset of a completed quest. If the current owner cannot distinguish completed instances or define a safe reset, the repeatable item remains a one-time side quest. If an existing world clock cannot provide stable expiry, use “available until accepted or explicitly withdrawn by an owner event” rather than inventing time.

### Rotation fairness and portfolio limits

Repeated work should not replace authored campaign content, appear endlessly after every expedition, or make one missing location block the entire notice board. Candidate selection should use the current deterministic RNG contract when variety is needed. Avoid modulo/iteration behavior tied to hash order. The selector should evaluate only currently valid offers, preserve active accepted commitments, and produce a bounded visible set.

Design review should assess:
- maximum simultaneously accepted route-operation instances;
- number of consecutive runs before a template repeats;
- whether a required active main quest is shown above rotating optional jobs;
- whether one location or reward source dominates the pool;
- how player level/skills change difficulty without making a different outcome authority;
- whether failure creates a cooldown or an alternate job only when the owner supports it;
- how a player who ignores the board can continue the campaign;
- how content rotation changes under expansion profiles;
- performance as template and location candidate counts increase.

No fixed numeric threshold is imposed here. The first integration slice should use a very small hand-authored pool and record its observed readability and performance. Add more templates only if the current panel and owner contracts can show why a task is available and preserve stable behavior through save/restore.

### Coverage of quest types and player entry routes

The Farline arc can test the quest families enumerated across the plan without forcing every family into the critical path:
- **Main:** The Signal That Outlived the Road.
- **Character:** Yara's Last Delivery.
- **Faction:** A Signal You Can Trust.
- **Discovery/environmental:** Three Knocks, No Answer.
- **Investigation:** compare plate and route report.
- **Escort/protection:** Carry the Unlit Cell.
- **Survival/timed:** Storm Window.
- **Resource/crafting:** Hush Kit.
- **Location-based:** an objective only at Siltglass Relay with fallback travel if selection fails.
- **Repeatable/rotating:** a bulletin template, deferred until reset ownership is verified.
- **Hidden:** discovery of the second signal clue, hidden only until earned.
- **Choice-reactive:** accept a local statement, request a trial, or decline faction endorsement.
- **Fail-forward:** report-only, alternate witness, or later inspection when the preferred route is gone.

Quest type is an authoring and balance label, not an effect or state category. A single task can be both investigation and location-based, but it still has one canonical lifecycle owner and a clear objective model. Classifying tasks helps production estimate cast, location, UI, and test cost. It must not create separate quest state machines for each family.

### Production card: the disabled signal route

The main quest's failure continuation should be authored as an explicit packet. When the relay is not reachable:
- The journal says the site cannot be reached under the current route state and names any earned clue.
- The player can return to the dispatcher or wait for a supported future opportunity.
- If a report is available, the quest can conclude at a lower confidence level only if that result is defined.
- The map does not show a substitute as though it were the same physical relay.
- The faction quest remains unaccepted or blocked until its actual prerequisites are met.
- A prior accepted clue remains accepted after restore if its owner persists it.
- No reward is issued twice when the player tries again.

When Yara's conversation is unavailable, use a current character/quest availability fact. Do not create a ghost Yara node that opens at a nonexistent location. If she has left the region, a letter/report route can replace her only if the content package provides that artifact and the quest owner accepts its evidence.

### Repeatable-quest status and UI contract

A rotating task should be visibly different from a one-time personal story. Its card identifies whether it is a campaign commitment, optional request, limited opportunity, or repeatable job. The UI can group by kind, but grouping must derive from authored definition/current owner facts, not a parallel status cache.

The notice-board row should show:
- task title and issuing source;
- whether it is new, returning, time-limited, or continuing from a prior run;
- the reason it is offered now, in player language;
- location certainty and whether it is mandatory, optional, or a clue;
- any known cost or accepted material requirement;
- what happens if ignored, declined, abandoned, or expired;
- the current objective and last supported result;
- a concise reward description sourced from the relevant owner.

Returning tasks should avoid false familiarity. “The relay needs another visit” is valid if the owner says it does. “You know how this ends” is not justified by a resettable work order unless the character actually has memory of prior completions under current narrative facts.

### Rewards and balance safeguards

The Farline scenario should reward information, safer choice, narrative recognition, and access only as supported by current systems. No quest automatically grants a signal skill, permanent travel speed, route accuracy buff, unique resource stockpile, or coalition membership. If a reward is a practical item, inventory/crafting owners define its ID, quantity, stack/weight behavior, and persistence. If it is a journal result, quest ownership defines the proof. If it is a location hint, the current map/discovery owner controls visibility.

Optional content should not become an efficiency tax. A player who declines Carry the Unlit Cell should not face a worse core ending unless that consequence was explicitly designed, disclosed, and accepted by a current owner. A crafted Hush Kit may permit a different approach; it should not be the sole proof path. A high-skill line may reveal that the signal is obsolete sooner; it cannot make low-skill players accept a false signal.

Balance review should compare time and resource costs across alternate solutions:
- inspection at the relay versus a report-only conclusion;
- later local review versus the storm-window route;
- carrying or crafting versus borrowing from a supported owner;
- local conclusion versus faction submission;
- optional discovery versus ordinary witness dialogue.

The content team records which route is shorter, safer, more resource-intensive, or less certain. It should not assign fictional costs until the source owner and balance data are verified.

### Branch and restore acceptance plan

| Player history | Required result |
|---|---|
| Main quest accepted; relay missing from one expedition | Quest remains possible through the authored delay or evidence fallback. |
| Player finds the second clue before Yara | The later conversation recognizes the supported discovery without duplicating proof. |
| Yara's personal quest fails or is abandoned | Main investigation still reaches an honest conclusion. |
| Player chooses a report-only result | Journal and follow-up copy preserve uncertainty; no adoption claim is made. |
| Faction package disabled | No mandatory reference to the Farline Compact remains. |
| Repeating job completed once | A second offer appears only under verified repeat/reset semantics and does not replay prior rewards. |
| Repeating job expires while accepted | Existing owner contract decides whether it remains active, fails, or routes forward; the board does not overwrite it. |
| Save occurs after a clue but before submission | Restore preserves only the clue state its canonical owner actually stores. |
| Same deterministic seed/profile is replayed | Selection and eligibility remain consistent under the game's established RNG contract. |
| Player declines all optional branches | Main quest remains understandable and reaches its supported terminal state. |

The eventual implementation should use focused tests permitted by the live test policy and may need a bounded deterministic selection check if dispatch choice changes. This pass does not ask to create speculative tests or to run any suite. It provides the content and verification questions for the later package owner.

### Integration boundary and next slice

The first proposed slice is the main quest's location request and accepted inspection result. Verify the current quest and location/dispatch APIs, current route ownership, the save owner, and the player-facing journal/map path. Only after that route is integrated should Yara's character thread or the Farline Compact branch be promoted. Repeatable operations come last because reset and persistence semantics have the highest risk of becoming a parallel scheduler.

If the audit finds an existing radio, audio, dispatch-board, or rotating-event owner, the plan extends that owner only if its contract supports this content. Otherwise the appropriate foreman decision is recorded. The proposed major faction and its late-game content remain authored narrative candidates; they are not proof that the current game has faction diplomacy or radio-driven quest activation.

### Expanded quest cards and objective-proof recipes

The overview cards above are not enough for production. This subsection expands the first set into individual authoring packets with explicit entry, objective, proof, outcome, failure, continuation, reuse, cost, and placement. Labels remain editorial. The recipes intentionally avoid inventing new IDs or state fields.

#### The Signal That Outlived the Road — main investigation packet

**Purpose and entry.** The player receives a report that a route tone directs people toward a crossing whose current status is unknown. Entry can come from an existing main-story offer, a discovered public notice, or a supported environmental trigger. The trigger must be one already represented by the current discovery/quest owners. Receiving the report is not acceptance.

**Required locations.** Siltglass Relay is required to inspect the plate itself. Half-Span Shelter can host a supported report. A substitute location can only satisfy travel or partial context, never the relay-specific inspection.

**Objective recipes.**
1. Find a route to the relay or learn why its approach is unavailable.
2. Inspect the signal plate through the appropriate interaction.
3. Collect one corroborating source, or select a supported lower-confidence report route.
4. Submit a conclusion that names the evidence boundary.
5. Receive the canonical quest result and review it in the journal.

**Evidence.** A plate inspection proves something about the authored plate. A route report proves that a source communicated a claim. A physically inspected crossing proves only the current condition represented by the route owner. Do not count any one of these as proof of the others.

**Possible failures.** The relay is unavailable; the interaction fails; the optional witness cannot be reached; the storm window expires; the player declines to report. Each case retains already accepted evidence. The quest can remain blocked, partially complete, or conclude with uncertainty according to the current owner contract.

**Rewards and reuse.** Completion gives a clear journal result and can unlock an optional faction offer if its owner supports it. Reuse is low because the plate identity and story question are unique. The location-arrival grammar may be reused; the clue text may not be copied to another site.

**Placement and cost.** Core candidate only if the base game supports the required location and quest owner route. Otherwise expansion. Medium integration cost for one location, evidence source, and journal result; high cost if it also requests faction and campaign effects.

#### Yara's Last Delivery — character quest packet

**Purpose and entry.** Yara asks the player to help establish why she did not carry an old route slip. She is concerned that refusing delivery will be interpreted as abandoning a community. The task asks the player to help her make a truthful account, not prove her innocence in a trial.

**Objectives.** Ask what was requested; inspect or receive the route slip if an existing item/document interaction allows it; identify the relevant conflicting route fact; return with a bounded account. The player may stop after hearing her account and decline to gather extra proof.

**Required location.** Half-Span Shelter is the normal conversation anchor. If Yara is not currently there, use an existing character availability or quest fallback. Do not spawn a duplicate Yara merely to satisfy the authored node.

**Branching and failure.** A discovery clue can make the player ask a sharper question but cannot force Yara to trust them. If a report is missing, the player can accept a partial account. If the character is unavailable, the task is deferred only when the owner supports such a state; otherwise it stays undiscovered or receives a neutral closure. A failed optional proof route does not make Yara's personal concern disappear.

**Reward, reuse, and cost.** The reward is a supported follow-up conversation or evidence result. No numeric relationship adjustment is presumed. Voice cost is high because the arc depends on restraint, silence, and whether Yara feels pressured. Expansion placement is likely unless the existing cast can absorb it.

#### A Signal You Can Trust — faction quest packet

**Purpose and entry.** The Farline Compact proposes that the player help compare a report format with the Lantern Wardens' local inspection method. The offer is available only after the main quest records an accepted observation or another supported prerequisite.

**Objective alternatives.**
- Present the plate inspection and ask whether the Compact will record its source.
- Ask the Wardens to explain what a field check can and cannot verify.
- Request a limited trial without endorsing a permanent standard.
- Decline to speak for either group and close the offer.

**Location requirement.** A real eligible faction contact plus a supported meeting location. If the meeting location is absent, the offer may wait or use a remote current communication owner. This proposal does not invent one.

**Failure and rewards.** A refusal by either group should be a faction result only if the current faction owner supplies it. An unavailable meeting is not a refusal. Rewards are access or recognition only where current owners support them; otherwise this remains a narrative/journal closure. Because two groups, at least one location, and a cross-faction callback are required, it is a late expansion candidate.

#### Three Knocks, No Answer — hidden discovery packet

**Purpose and trigger.** A carved sequence implies that a second signal existed when the main relay was silent. The clue is discovered by observation, an earned knowledge context, or a sourced report. It does not start as a task in the journal until the current quest/discovery owner recognizes the lead.

**Discovery route.** A player can encounter the clue near Old Echo Cut, hear it from Yara after meeting the relevant condition, or find a notice at Lantern Yard. These are alternate lead sources, not three copied clue states. Each source attributes its own information and does not reveal an exact destination unless that source is allowed to.

**Failure/recovery.** If Old Echo Cut is not selected, retain the lead as a hint and offer a future expedition possibility. If the player sees the mark but cannot interpret it, provide an optional follow-up through a current owner or leave it as environmental lore. The discovery cannot be required for main quest closure.

**Reward and placement.** The reward is a second report source and a line of optional dialogue. It may support a better-supported conclusion but not a guaranteed faction adoption. Production cost is moderate because secrecy, localization, map disclosure, and location availability must agree.

#### Carry the Unlit Cell — escort/protection packet

The escort's central verb is “carry with,” not “protect at all costs.” Before accepting, the player is told the item is unpowered and that its route is conditional. The quest asks the player to accompany an existing actor only if the current expedition system supports actor assignment and persistence. If not, use an abstract supported delivery/interaction route; do not add a companion controller solely to satisfy the prose.

Valid outcomes include successful handoff, secured return, delayed transfer, and player refusal. Loss or destruction is not a default failure state. If a current owner supports an item custody transition, it reports accepted/rejected/unknown results. Dialogue does not remove the cell on selection. When the expedition aborts, the item's current owner decides its custody; the quest owner reads that result.

The quest can be replayed as a different authored job only if the owner supports repeat semantics. A personal named escort should otherwise be one-time. Production cost is high if it requires an actor to move across an expedition, medium if it is represented as a single source/target handoff.

#### Storm Window and Hush Kit — linked optional packet

These two tasks can share a preparation relationship without coupling their state. Storm Window offers a time-bounded observation. Hush Kit offers a crafted handling item or preparation plan. The kit may reduce one supported risk or unlock a safer observation approach, but its absence does not make the main quest impossible.

If the player completes the kit first, it should remain useful for another supported activity when the inspection window expires. If the player misses the window, the kit is not consumed unless its owner confirms consumption. If crafting is unavailable, the player can use a public route or wait for a new opportunity. If the timed activity begins, its duration and end condition come from the existing time authority; no menu-local countdown can decide expiry.

### Objective graph edge audit

Every objective edge in the Farline portfolio is labeled with a semantic type:

| Edge type | Meaning | Example | Review question |
|---|---|---|---|
| Required evidence | A later objective depends on accepted proof. | Inspect plate before submitting plate-specific conclusion. | Can the proof be reached and accepted once? |
| Offer eligibility | A new quest becomes available after a fact. | Faction request after main observation accepted. | Which owner publishes eligibility? |
| Optional context | A clue changes available exposition, not objective progress. | Yara recognizes the double-knock clue. | Does the main route remain clear without it? |
| Location requirement | An objective names a place or compatible role. | Visit Siltglass Relay. | What happens if the site cannot be selected? |
| Timed opportunity | One optional interaction is available within a window. | Storm Window. | What does expiration close, and what remains? |
| Owner consequence | A command asks a domain system to record a fact. | Submit recommendation. | What exact owner result drives the next node? |
| Presentation callback | A line or marker reflects a current fact. | Yara comments on an accepted report. | Is the callback still true after restore and revisit? |

A dependency edge must be one of these declared meanings or another explicit owner-backed type. It cannot mean “some other quest probably happened.” Any cycles are reviewed for a valid exit: two quests should not wait on each other's acceptance. If a reciprocal relationship is intended, break the cycle with one reachable initial offer or an explicitly optional step.

### Objective count and active-log communication

A large portfolio can overwhelm a player even if every quest is valid. Keep each accepted quest focused on one next action with optional context collapsed into details. The main investigation should show one current objective and at most a concise next-step hint. The faction request should remain separate from the player's core inspection responsibility. The rotating board should not push personal arcs out of view.

If the current journal supports grouping, classify by authored family and owner facts. If it does not, the interface change is a separate UI proposal; do not create a hidden quest-priority number in content. A tracked task is a view preference only. The main quest does not become more eligible because it is pinned. The player can hide optional tasks without canceling them.

For simultaneous tasks, show when two opportunities share a location but require different actions. Visiting Lantern Yard to read a notice is not the same as accepting the faction meeting. The journal can list both as separate actions so that one interaction cannot silently advance multiple quests unless each owner independently accepts the same evidence.

### Content design and verification budget

A future release should split work by risk:
- low: descriptive report, no command, no hidden prerequisite;
- medium: single-owner objective or discovery transition, save/read-model review;
- high: required location plus proof fallback, timed opportunity, or item transfer;
- critical: faction/campaign result, ending input, cross-owner sequence, or state migration.

The first release does not need all seven quests. The recommended order is main observation slice; Yara conversation; discovery clue; faction request; optional escort/crafting; repeatable board only after reset semantics are verified. This order gives story coverage while placing high-risk mechanics behind proven owner paths.

The focused acceptance plan should include main path, alternate clue order, rejected proof, missing required location, core-only package, expired opportunity, interrupted escort, duplicate item transfer, and save/restore. The existing test-policy owner determines what to run and which fixtures are permissible. This document makes no test additions or claims that the proposed story already functions.


## Continuation pass 13 — quest lifecycle rehearsal, recovery routes, and authoring acceptance

### Purpose of this module

This module turns the Farline Circuit portfolio into a reviewable lifecycle rehearsal. It makes the proposed quests legible to content authors, implementers, and reviewers before anyone creates catalog records. It does not claim an existing quest schema, a new quest runtime, or an additional save authority. Each record remains a design packet that must be mapped to the currently owned quest data format and current host route after a premise audit. The architectural objective is modest: every authored objective must have a trustworthy proof, every interruption must have a defined player-facing response, and every branch must preserve a route to meaningful resolution.

A content packet should answer four different questions that are often collapsed into one. What is the fiction asking the player to do? What observable state proves it happened? What other authored content becomes eligible afterward? What does the player see if the expected state cannot be reached? A prose answer alone is insufficient. A flag name without a player-facing explanation is also insufficient. A reviewable packet needs both a human-readable intention and a machine-mappable condition, while leaving actual identifiers and ownership to the content validation pass.

The circuit story offers a useful stress case because its premise concerns unreliable information. The player hears a relay tone, visits places where its source may have moved, and decides what is safe to repeat. The quest design must not require an invisible “correct interpretation.” It should reward evidence collection and deliberate communication, while allowing the player to preserve uncertainty. A player who declines to endorse a claim must still receive a coherent continuation: they can label the report unverified, ask for a second source, or leave the record sealed. No path should silently require that a character accept a rumor as fact.

### Lifecycle rehearsal: A Reply Should Have a Source

The following rehearsal is an authored example, not a proposed new universal quest framework. It tests how a compact chain behaves through acceptance, active work, blockage, partial completion, failure-forward continuation, resolution, and delayed callback.

**Opening condition.** The player has encountered the old relay pulse at a location in the active expedition, or has received a report whose provenance is explicitly uncertain. Yara’s personal conversation can expose the job, but the player may also receive it through the Lantern Wardens’ board. These two entry points must converge on the same quest identity if the existing quest architecture supports shared starts. If it does not, one entry point is selected as the canonical start and the other provides a pointer to it. The design must avoid two parallel jobs that compete for the same evidence.

**Acceptance.** The description states the practical task: record where the pulse was heard, identify what kind of equipment could have produced it, and return with enough evidence for the group to decide whether to repeat it. Acceptance does not mean that Yara believes a rescue call exists. The journal should make the distinction plain: “Find the source of the repeated tone; do not treat the message as confirmed.” The player can decline without a penalty that affects unrelated faction standing. If the job expires due to a campaign deadline, the expiration should close this opportunity and expose a later reconstruction path rather than erase the evidence from the world.

**First objective.** Record one direct clue at Siltglass Relay or the location that currently fulfills its equivalent role. A clue may be a damaged service ledger, a timed interval between pulses, a manually cut wire, or a repair mark. The objective should accept evidence classes rather than a specific prop instance. That gives level dressing room to vary without changing the story contract. A clue is only valid when a gameplay interaction or established discovery event records it. Merely rendering the prop must not complete the objective if the player did not inspect it.

**Second objective.** Compare the first clue with one independent source. The preferred source is a visit to Half-Span Shelter or an equivalent route shelter where a different person remembers the tone. If that site cannot be selected for the expedition, a recorded note or a known character conversation can serve as the fallback only when its provenance is distinct. The same person repeating the same story in a different room is not independent corroboration. The quest packet should identify source provenance for each candidate clue and state which pairs count as independent.

**Blockage.** The player can be unable to travel because an injury, weather, expedition roster, or route condition makes the shelter visit unreasonable. A blocked quest is a temporary state with a visible reason and a way to reassess it. The journal might say that the route is closed by a washout, and that a survey mark or a report from a returning scout could substitute. A blocked marker must not be represented as a dead-end failure. Nor should the system repeatedly announce the same obstruction after every expedition. Refresh the explanation only when its underlying condition changes or the player opens the related quest entry.

**Partial completion.** The player returns with one credible source but no independent comparison. Credit the evidence already acquired. Do not force a rescan or make the player revisit a location solely because an expedition boundary occurred. The next step should explain the remaining uncertainty and show the useful source category, not reveal the exact hidden object if discovery is intended. If a related quest has already supplied the second source, a shared proof may satisfy this step only when both quest packets explicitly permit evidence sharing and the evidence object records its source.

**Failure-forward route.** The player may dismantle or destroy the relay before recording its maintenance state, or a danger event may make the location inaccessible. This is a loss of one evidence path, not a loss of the whole storyline. The player can interview a person who handled its battery, compare a wear pattern at another site, or state that the source cannot be established. The consequences differ: direct attribution is stronger than a reconstruction, and reconstruction is stronger than an unsupported conclusion. The quest should not punish the player with a false objective that remains impossible forever. If all acceptable proof options are gone, the system changes the objective to document the missing evidence and resolve the report with an explicit uncertainty label.

**Interpretation.** With evidence available, Yara and Pell disagree over how to communicate it. Yara favors preserving the original wording with a warning. Pell wants an immediate route notice because people may be walking toward the sound. Neither response is a pure good/evil test. Preserving exact wording may reduce alteration but can also spread a frightening fragment. A practical warning may prevent a dangerous journey but may be misunderstood as confirmation. The choice is framed as audience, confidence, and timing. The player may request a short delay for one more check, but that consumes a meaningful opportunity rather than pausing a fictional clock indefinitely.

**Resolution.** The player chooses one of a small number of message treatments: confirmed mechanical source, plausible but incomplete reconstruction, unverified alert, or sealed record. These are design outcomes, not mandatory new world-state labels. They should map to existing narrative effects if those can represent them. An outcome changes who trusts the player’s reports and which notice is available; it does not create four separate versions of every later conversation. Most later content reads a compact evidence-quality value or one mutually exclusive outcome, then varies one or two lines.

**Delayed callback.** Several days or an authored later milestone later, a courier arrives with an account that may reinforce or complicate the player’s conclusion. The callback must have an expiry and replay policy. If the relevant character has left, a note or board notice conveys the material information. If the campaign ends before the callback is eligible, no dangling quest remains active. The closure record preserves the selected treatment and lets a future authored scene reference it.

This rehearsal gives the team concrete transitions without suggesting a bespoke state machine for every quest. Quest lifecycle remains the current owner’s responsibility. A content author supplies start conditions, objective proof, failure transitions, and closure facts in the existing accepted shape.

### Objective proof recipes

Every objective should define a proof recipe. The recipe describes events and conditions in plain language so an integrator can map them to current data fields. It should not prescribe a host callback or invent a domain API. The following recipes illustrate the needed precision.

- **Inspect a record:** required interaction is the established inspect action on a clue tagged for this story; minimum result is “record opened”; optional result is a quality grade based on legibility. Opening a container that happens to hold the paper is not the same proof.
- **Compare two accounts:** require two sources with different provenance groups, each logged as understood by the player. If one source is revised later, preserve the initial discovery and record the correction separately.
- **Deliver a warning:** require that an authorized receiver has encountered the warning through a recognized conversation, notice, or message handoff. Putting an item into inventory is not delivery.
- **Protect a traveler:** success is the end of the travel operation with the protected character or load still available; avoid assuming the character is unharmed if the game’s current injury model distinguishes injury from loss.
- **Restore a route marker:** success is the established world interaction changing the route’s usability or recorded condition. A crafting recipe by itself is not proof if the marker was never installed.
- **Choose a message treatment:** require a player choice persisted through the current quest completion path. The text option selected should match the outcome used in later dialogue.
- **Resolve without evidence:** require an explicit “close as unresolved” or equivalent authorable choice. The player should not obtain the same confidence reward as direct corroboration, though the story can value honest uncertainty.

A recipe is good when two different implementations could be authored against it without changing its meaning. It is too loose when any vaguely related action counts. It is too brittle when only a single prop identifier can satisfy a concept that should survive location substitution.

### Resume and recovery rules

Quest work commonly crosses expedition boundaries. A player may leave a location after an incomplete interaction, return after weather changes, or load a save in the middle of a dialogue choice. The plan should require each multi-step quest to identify its recovery checkpoint. The checkpoint is not a new save section; it is a statement of which current owner retains the accepted quest and completed objective facts.

On resume, show the next useful action, preserve already proven objectives, and explain changed eligibility only when it matters. If the player has already discovered a substitute clue, do not demand the unavailable original. If a character's new state invalidates a conversation, surface a new route or an explicit closure. If a one-time dialogue node was already consumed, reopening the quest log should not replay the scene as if it never happened. Save restoration should reconstruct the same active objective and branch outcome from existing persisted state, not from a newly initialized narrative cache.

The resume checklist for every major chain is:

1. Record all irreversible player choices in a durable owner.
2. Recompute only derived eligibility after load; do not fabricate discovery history.
3. Preserve evidence provenance and prevent duplicate proof from the same source.
4. Rebuild map markers from quest facts and the active expedition roster.
5. Reconcile expired or unavailable locations into a visible alternate route.
6. Ensure dialogue offers do not restart a completed quest.
7. Make journal language consistent with the restored branch.
8. Allow abandoned content to return only through an authored reopen condition.

### Content graph review as a human task

A graph validator can report missing references, unreachable nodes, impossible terminal states, and contradictory IDs. It cannot decide whether a branch is emotionally credible or whether two characters have independent knowledge. The authoring review therefore needs a compact walk through each branch. For each outcome, answer: what fact did the player learn, what did the player communicate, who heard it, what remains uncertain, what is the next viable action, and which later scene can acknowledge the choice?

Reviewers should trace at least four paths: the intended evidence-rich path; a path where the player refuses the job; a path where a location is absent; and a path where the player loses access to a clue after making a risky choice. If all paths collapse to identical state and nearly identical lines, the branches may be cosmetic and should be labeled as such. If two paths promise distinct faction or relationship outcomes but no current owner can persist those facts, either scope the promise down or request an explicit architecture decision before implementation.

The Farline Circuit also serves as a check against quest proliferation. Its central question is evidence handling, and all candidate jobs should test a different pressure: preserving testimony, physically repairing a marker, keeping a traveler safe, or deciding what to announce. A quest that only repeats “find the next relay” is removed or folded into a distinct route event. The circuit can still feel expansive through varied contexts, but each activity must justify its own player decision and production cost.

### Content package acceptance card

Before any record is authored, complete a one-page review card:

- **Player verb:** the action the player will actually take.
- **Fictional purpose:** why the character asks for it.
- **Entry paths:** every eligible start and how duplicate starts converge.
- **Objective proof:** the observable event or condition that earns credit.
- **Evidence provenance:** source identity and whether evidence may be shared.
- **Availability contract:** minimum locations, characters, and campaign conditions.
- **Fallback:** what happens when each required dependency disappears.
- **Failure result:** whether the quest fails, changes route, or resolves with uncertainty.
- **Effects:** cosmetic, local, quest, relationship, faction, world, or ending scope.
- **Persistence owner:** current authority to verify before implementation.
- **Map behavior:** when a place is visible, discovered, quest-only, or omitted.
- **Replay policy:** one-time, repeatable, rotating, or reopened.
- **Return scene:** the delayed acknowledgement that proves the world remembers.
- **Review cost:** expected dialogue, location, art, QA, localization, and test work.
- **Removal condition:** what evidence would justify cutting the content.

The card is deliberately brief. It exposes missing design decisions before prose volume grows. The full plan may contain hundreds of examples, but each future addition should still be legible through this common review surface.

### Pass-level acceptance and bounded implementation path

The pass is ready for conversion into implementation tasks only after these conditions are met: every proposed quest is assigned an existing or explicitly approved category; required locations have a fallback chain; story-critical proof has independent provenance where needed; no ending depends on an unavailable optional job; outcomes have a named owner for each persisted effect; and all recurring jobs state how they avoid repeating completed rewards. The Farline material remains provisional until the expansion context, live catalogs, and current runtime consumers have been audited for collisions and actual field support.

The recommended first playable slice is “A Reply Should Have a Source” with one start, two independent evidence opportunities, one alternate route, one ending choice, and one delayed callback. Ship no rotating board generator as a prerequisite. Use authored static task options and existing expedition selection behavior where they can support the slice. Only after this slice proves useful should the team consider larger variation. This keeps risk close to the story premise: the player learns to evaluate a message because the game demonstrates why provenance matters, rather than because a new journal subsystem was added.

If current APIs cannot represent provenance, branch restoration, or an unavailable-site transition, record the exact gap and stop at the approved planning boundary. The absence is useful evidence for a future architecture decision; it is not permission to create a parallel quest ledger. Likewise, if existing data already supports the behavior, the implementation should reuse that path and avoid renaming concepts just to match this plan’s vocabulary.


### Quest board operations and cadence review

The Lantern Yard board should present authored work as a small set of practical commitments, not an endlessly refreshing quest dispenser. A board entry states who needs help, the expected risk, whether the player is expected to return with proof, and whether another person can act while the player is away. This allows the player to compare the job against shelter needs and expedition readiness. If the current interface has no board model, the first slice can use a character conversation; do not create a second acceptance surface merely to match the fiction.

Rotating work should be finite in a given campaign window. Each entry needs an availability cause, an expiry cause, a completion definition, and a repeat policy. A route-check job might return after an authored interval only if the world still has a reason to need another check. It cannot generate the same reward indefinitely by cycling the same completion fact. If the game has no current campaign scheduler, author several static variants and expose them through existing event timing rather than inventing a generic daily contract manager.

Acceptance should disclose the job’s basic cost. If a courier expects the player to carry a fragile record, say so before departure. If the task might consume a scarce repair material, make the requirement visible. Uncertainty about the result is an intentional story feature; uncertainty about what the player is being asked to risk is avoidable frustration.

Cadence review asks how often the player encounters a Farline task, how many active objectives it can create, and whether it competes with urgent survival decisions. Space major conversations across meaningful returns rather than triggering several immediately after one location visit. Repeatable tasks should use short debriefs and fresh practical circumstances, not re-stage the whole investigation. If a rotating task lacks a new decision, remove it.

Reward review follows the same principle. Information access, route safety, a repaired marker, a character’s confidence, and modest materials are different reward types. Each must connect to an existing player-facing system. Do not assign generic currency to every quest by default. A reward can be a safer approach or an option to decline a dangerous route, but the journal should record what changed.

Finally, validate the player’s exit paths: refuse the board, accept and later abandon the job, resolve it through an equivalent clue, and return after its window has passed. The board should show the resulting state and never make a completed job appear available again unless an explicit repeat rule says why.

### Lifecycle table and player-facing vocabulary

A quest packet should map its conceptual lifecycle to the states the current runtime actually supports. The design vocabulary in this series includes inactive, available, discovered, accepted, in progress, blocked, partially completed, failed, completed, resolved, expired, abandoned, and reopened. These labels are not instructions to implement fourteen enum values. Several may be presentation descriptions over fewer existing states. Before authoring conditions, the integrator must identify the current representation and define how each needed distinction is expressed.

For the Farline chain, “discovered” means the player has encountered an eligible lead but has not accepted the work; it must not display as accepted. “Blocked” means work remains possible after a named condition changes. “Partially completed” means one or more proof facts remain valid while the overall objective is open. “Failed” means a chosen route can no longer satisfy its original objective. “Resolved” describes closure that may preserve an uncertain outcome. “Reopened” requires an explicit later event and must never happen merely because the player revisits a location.

The journal should use stable, plain language even if several conceptual states map to one runtime value. For example, a blocked investigation says what is unavailable and which alternative can help. An expired rotating task says that the opportunity has passed and whether any later route remains. An abandoned personal conversation does not remain in the active list indefinitely. These descriptions should be reviewed with status transitions so text cannot claim that the player has accepted a job they only discovered.

A transition table accompanies each content graph: current conceptual state, triggering action or condition, next state, objective facts preserved, player-facing copy, map change, and whether the transition persists. Invalid transitions include a failed quest returning to in-progress without an authored reopen, an expired job awarding its normal completion reward, and a resolved story continuing to display an urgent marker. This table is a review artifact; the current owner and save model remain authoritative.

### Concurrency limits

Set a deliberate cap on simultaneous Farline commitments through the existing quest presentation policy. The cap is a pacing guideline unless the current runtime already enforces one. If several entries are active, prioritize the main evidence chain and let optional work remain discovered or available. Do not auto-abandon accepted content to make room. Explain when a new request can be accepted later, and preserve progress on any existing task.

Shared proofs require explicit compatibility. One visit to a relay may satisfy two objectives if both ask the player to inspect the same physical condition; it cannot automatically satisfy a personal promise to deliver the result to two different people. When two quests share a proof, record the completion once through its owner and let each objective consume the fact according to its contract. The player-facing journal should avoid listing the same action as two unrelated errands.

If the existing owner has no concurrency control, the first slice should simply author few simultaneous offers. Do not build a new queue to manage a content volume problem before proving that the game’s current quest flow needs it.

### Authoring review dry run

Before inviting a player to accept a Farline task, read its card from the player’s perspective with no knowledge of this design packet. Can they tell what action is expected, what risk is being taken, what kind of proof matters, and whether the job is urgent? If the answer depends on hidden architecture terms such as provenance class or branch state, rewrite the visible copy in ordinary language while keeping the underlying contract precise.

Then simulate a refusal. The character should acknowledge the decision without falsely marking the quest failed or imposing a relationship penalty that was never communicated. Simulate acceptance followed by a long absence. The journal should remain accurate, any deadline should have a reason, and the task should not repeatedly trigger the same opening conversation. Finally simulate success through the least direct valid route: an equivalent location, a second witness, or an uncertainty-preserving resolution. The player should receive a complete ending for the chosen route, even if it is not the most evidence-rich result.

This dry run catches mismatches between quest classification and actual play. A discovery quest should reward observation; an escort should make protection the central action; an investigation should let evidence change interpretation. If the dominant activity does not match the label, reclassify the content or redesign its objective before it enters the canonical catalog.

## Continuation pass 14 — The Empty Shift questline, roster evidence, and shelter-work continuity

### Expansion premise

The Empty Shift is a second provisional story thread, independent of the Farline Circuit. It begins when a shelter roster continues to list a night crew for a heat-service annex that no longer appears to be staffed. The names belong to people who still receive a work allocation, but residents disagree about whether the work is being done, whether the schedule is copied from an older sheet, or whether the crew is maintaining a different part of the shelter. No supernatural explanation is implied. The mystery concerns a record that may preserve useful work, misdirect scarce labor, or shield people from a dangerous shift.

This content should connect to the game’s actual shelter, work, resource, health, and relationship owners only after their current responsibilities are verified. The quest is playable as an investigation even if no new work-assignment mechanic is approved. Its first slice can be data and dialogue around existing interactions: inspect a roster, compare it with a location condition, speak with two people, and choose whether to correct, annotate, or preserve the record. If the player-facing game has a current way to assign workers, a later integration may let the outcome alter that system through its existing owner. If it does not, the story must not invent a shadow roster manager.

### Core question and player promise

The core question is not “who is lying?” It is “what should a community do when a useful work record no longer matches what can be observed?” The player promises to investigate a discrepancy, not to expose a villain. The roster may contain a harmless copying error, a deliberately maintained placeholder, a concealed reassignment, or a blend of these. Evidence should let the player distinguish what is known from what remains uncertain.

At acceptance, communicate the practical stakes. A shelter coordinator says that the annex roster has three names on every night but the room is locked and the service log has not been signed. The current work board uses the roster to plan relief and food portions. If the record is wrong, someone may be counted twice or sent toward unsafe work. If it is merely incomplete, removing the shift may leave an actual task unsupported. The player can accept, decline, or ask who will be affected by a correction. Declining does not mark the player as disloyal. It leaves the question open until a later start condition or expiry is explicitly authored.

The story’s player-facing promise is modest: find a reliable account of the shift, report evidence rather than accusation, and leave the shelter with a usable next step. It does not promise a perfect census, a universal scheduling system, or a final answer to every person’s motives.

### Main questline: The Empty Shift

**Stage 1 — The sheet that never changes.** The player is shown two roster copies from different periods. Their shift names and task line match exactly, including the same faded correction mark. The objective is to compare the copies and record whether they are identical, altered, or incomplete. If a copy cannot be inspected, a character can summarize it but the evidence grade is lower. The player should be able to accept the investigation without a rare skill.

**Stage 2 — The room that is locked.** Visit the heat-service annex or an authored equivalent. The door may be secured, the room may be repurposed, or the work may be performed from a nearby service niche. The quest cannot assume that a room described as locked contains proof of abandonment. A location interaction should reveal a practical observation: dust pattern around a hinge, fresh tool marks, a replaced cable, or a handwritten note behind the outside panel. At least two observations must remain compatible with more than one explanation.

**Stage 3 — The person who knows the key.** Speak with Mara Venn, a provisional shelter shift clerk. She keeps the roster in a cloth sleeve and remembers which names were copied by which hand. She will not identify a culprit from handwriting alone. She can explain that the correction mark was once used for “covered shift,” a status distinct from “worked shift.” Her useful information is gated by the player’s willingness to ask about the mark rather than accuse someone. A non-gated route can find the same definition in a maintenance margin note.

**Stage 4 — The account from the night crew.** Meet one or more people whose names appear on the sheet. Candidate characters include Oren Vale, who has a stiff shoulder and used to repair valves, and Nessa Pike, who now handles a different shelter duty. Their stories need not agree. Oren may say he worked the first month and then traded shifts; Nessa may remember carrying parts to the annex but not entering it. Their statements are evidence with source identities, not global truth. If a character is absent, an authored log or an alternate witness supplies basic information without equal emotional detail.

**Stage 5 — The missing sign-off.** Compare a material trace with the record. A service mark may show that work occurred, but not who did it. A parts allocation may show that supplies left the store, but not that they were used at the annex. The player can acquire a direct signed note, observe a repair, or find an alternate route record. This stage teaches evidence categories through play. It must not let a high skill silently solve the whole case.

**Stage 6 — The correction meeting.** Present the evidence to the current work-board owner or relevant shelter character. The player chooses how to update the record: correct the named crew and task; preserve the roster but annotate that coverage was reassigned; keep the entry while seeking another source; or close the inquiry as unresolved because the evidence does not support a safe edit. The choice is about how to keep the work actionable. Any resulting resource or worker-state change requires a verified existing owner. Without one, the immediate result can be a revised authored board and dialogue outcome, with no claimed simulation change.

**Stage 7 — The next shift.** A delayed callback checks the practical result. A shift can be visibly covered, a task can be carried by a different named group, or the board can retain an uncertainty note. The callback should not claim that shelter heat improved unless the real heat authority changed. It can instead report who accepted the work and whether the record was clear enough to plan around. If no callback trigger is supported, place the closure in the next relevant conversation rather than creating a background scheduler.

### Evidence outcomes

The quest distinguishes four report outcomes:

1. **Verified reassignment:** independent evidence shows the listed people no longer perform the task and identifies the current assignment or the absence of one.
2. **Work observed, worker unknown:** a physical trace confirms activity, but it cannot be assigned to a named person.
3. **Record mismatch established:** the copies or signatures conflict, but the current operation remains uncertain.
4. **No safe correction:** the player lacks sufficient information and preserves the discrepancy with an explicit note.

These outcome bands can map to existing quest results or closure flags only if the current schema supports them. They do not require a new evidence engine. The journal should use language that corresponds to the chosen band. “The annex is unstaffed” is not an acceptable summary if the player only found a locked door. “The roster was not independently confirmed” is safer.

### Supporting quest portfolio

**Mara’s Margin Note — character quest.** Mara asks the player to find a box of old shift annotations before the paper is moved to a drier shelf. The task is to preserve the work history, not to validate every claim. The player can catalog, return, or leave the notes in their current order. A relationship outcome may reflect whether the player retained the original distinction between “covered” and “worked,” but the basic quest must not depend on a hidden affinity value.

**The Tool That Came Back Clean — investigation.** A valve tool is logged out for the night shift and returned with no damage. One person says it was used at the annex; another says the same tool never left the repair bench. The player can inspect wear, compare inventory records if current systems support them, or ask for a direct demonstration. If the game has no tool history owner, the quest uses an authored paper record and does not imply that inventory knows the tool’s past.

**Relief Before Dark — escort/protection.** A temporary worker wants to leave before weather worsens, but their replacement is late. The player can guide them to the safe shelter, help hand off the task, wait for the replacement, or advise that the task be suspended. Success is a safe, explicit handoff, not necessarily completion of the repair. Failure can leave the task unresolved and expose a later route to resume it.

**The Duplicated Portion — resource and crafting.** The kitchen has two meal allocations tied to a shift that may be a duplicate. The player may check the roster, ask the cook about actual attendance, or keep a reserve until the crew is identified. If an existing food or ration system can represent the allocation, use it. If not, make the consequence a narrative choice about the board and do not add a parallel ration counter.

**A Quiet Place to Work — location discovery.** Environmental evidence suggests some work was performed from a service niche rather than inside the locked annex. The player must locate the niche from scratches, pipe vibration, or a route sketch. This is a discovery quest with a defined search area and an alternate report route. It must not depend on hidden map knowledge that was never signaled.

**Covered Is Not Absent — dialogue investigation.** A resident insists that the night crew “did not show up,” while a clerk remembers a covered shift. The player can ask what “covered” means, who provided coverage, or whether the record was copied. The resolution teaches how procedure changed under pressure without revealing a single mastermind.

**The Board After the Move — repeatable/rotating task.** After the first resolution, the player can periodically help transfer the current work board to a dry location. The repeatable version uses fresh authored conditions and a finite practical reward; it does not reopen the original mystery or generate infinite food, trust, or currency.

**The Names We Keep — hidden environmental quest.** A player who revisits the old board may discover a personal mark beneath the current roster. It reveals that a former worker kept their shift listed after leaving so the replacement could collect the ration intended for a dependent. This is one possible story route, not a universal reveal. It should be present only if canon review confirms it does not contradict existing character history. The discovery is optional and does not convert the whole roster into a lie.

### Failure and recovery

If the annex never appears, use an equivalent evidence source: an exterior service panel, a maintenance log, or a witness who saw the job performed. If the player destroys a paper copy, the quest may continue with the second copy but record lower confidence. If Mara leaves, the margin note becomes the route for the definition. If a listed worker is unavailable, the player can document the missing account and still reach an honest result. If the player publicly accuses a worker without evidence, the quest can continue through a repair conversation or a retraction, but any lasting social consequence requires a real owner and explicit player-facing setup.

A failed evidence route should not invalidate all prior observations. The player may lose one source while preserving another. If no reliable path remains, the quest can resolve as “mismatch observed, assignment unknown.” That is a real conclusion and must not be treated as a hidden failure. A player who abandons the investigation can return only through an explicit reopen trigger, such as a new roster discrepancy, not merely by entering the shelter.

### Integration and acceptance

The first playable slice comprises one roster interaction, one location clue, Mara’s explanation, two outcome choices, and one return line. It adds no scheduling simulation. A second slice may connect the result to a verified work assignment, shelter task, food allocation, or heat authority if the relevant owner exists and the integration package approves it. Acceptance requires that objective proof is observable, the player can distinguish testimony from confirmed fact, unavailable characters and locations have fallbacks, all stateful effects use current owners, and save restoration preserves the chosen report outcome.


### Pass 14 quest anthology: work, testimony, and the cost of a clean record

The main investigation is only one route through the Empty Shift. The supporting quests give the player several kinds of action and let the shelter’s record problem connect to ordinary survival decisions. Each card has a distinct verb, source of uncertainty, failure route, and closure. They should not all be required. The portfolio is reviewed against current systems before any record or reward is authored.

#### The Glove With the Blue Stitch — character and discovery

A patched glove is found on the Repair Bench. The roster says Oren was assigned that night, but the glove is too small for his hand. The player can return it to the communal bin, ask who repaired the cuff, or leave a note for whoever owns it. The clue does not prove who worked the shift. It opens a character scene in which Nessa explains that the gloves are passed between workers when the storage locker freezes.

The quest stages are: notice the glove; inspect the blue stitch; ask one worker; decide whether to tag it; and optionally return it to the person who claims it. A discovery skill may reveal that two threads are different, but a plain interaction still notes the patch. If the player gives it to the wrong person, the quest remains recoverable through an apology or a return to the communal bin. The reward is not a stat boost; it is a better source for a later work handoff, if the current relationship owner supports such a reaction. Otherwise the line is local and the quest closes as a small story.

#### The Quiet Count — survival and resource choice

Sella asks the player to compare the expected night count with the people present before food is portioned. This is not an invitation to police who eats. The player can ask for a reserve, use the current list, or leave the count to the cook. The task explores the cost of treating a projected roster as attendance data. The player should see the practical uncertainty before choosing.

If the existing food owner exposes portion decisions, route the choice through it and use the current balancing rules. If there is no such owner, keep the task entirely conversational. Do not add a second ration ledger. Failure occurs if the player leaves before the count is settled or if the roster source is invalid; the alternative is a documented estimate, not a punishment that silently removes food from the shelter. Any resource outcome must show its exact result and survive save/load through the existing owner.

#### The Dry Copy — timed but non-punitive

Rain is expected during the next expedition cycle, and Mara asks that a paper copy be moved before the shelf leaks. The time pressure concerns document condition, not an invisible countdown. The player can move the copy now, ask someone else to protect it, or decide that preserving the original matters more than convenience. If no campaign calendar exists, the task expires at a supported expedition boundary rather than a hard-coded day count.

If the player does not act, the copy becomes harder to read or an alternate copy is used, but the main quest remains possible. The task can resolve as “source copy damaged; duplicate retained.” The weather effect itself is only included if current location and environmental systems can express it. Otherwise, use a pre-authored event and state the limitation.

#### The Walk Before Dark — escort/protection

A temporary worker needs to cross the West Intake Walk before the shelter’s shift changes. The player can escort them, find a covered passage, arrange for another person to accompany them, or recommend that the trip wait. The protected person’s goal is to reach the board with a tool and sign their own name, not to be escorted passively as cargo.

The sequence includes a departure conversation, a route hazard or practical obstacle, one decision about pace or shelter, and a handoff at the destination. If the player cannot travel, a support route remains through a second witness or note. If the escort fails, the task continues with an account of what was delayed. It does not presume injury or death. The quest should use existing movement, expedition, injury, and character availability owners; no custom escort simulation is implied by this card.

#### The Line Reversed — investigation branch

A correction mark appears on the second copy with its hook facing the opposite direction. The player can ask whether the mark was copied incorrectly, compare another roster page, or inspect the tool used to make the mark. Each route supplies different evidence. The mark may indicate revision order, not a worker identity. The player can publish that limited conclusion, keep investigating, or close the question.

The quest has a deliberate contradiction: a character remembers the mark meaning “covered,” while a ledger note says it was used for “coverage requested.” Both can be true if the term changed over time. The player can establish that the notation is ambiguous without deciding who wrote it. This is a mystery conclusion, not a failed investigation.

#### The Bench Ticket — crafting and repair

A repair ticket asks for a replacement fastener that can be used on the outside panel. The player can craft or salvage the part if those actions exist. The task is primarily a repair activity; it does not require the player to infer who worked the night shift. Completing it enables a safer inspection only if the current location/maintenance owner supports that effect. If not, it is a resource quest that provides a small, explicit material exchange or closes as an authored repair report.

Production cost is high if the part requires a bespoke recipe, new item, and new location state. Keep it as an optional task until a current resource owner proves the content can be expressed using existing materials. Do not add a new repair tree just to make the story feel systemic.

#### The Name Under the Fold — hidden environmental discovery

The player finds a pencil impression on the back of an old roster. The visible paper shows only a half-erased name. The impression leads to an optional conversation about a person who kept taking an extra shift after leaving the room because someone at home depended on the allocation. The player can record the clue, ask a careful question, or leave the page alone.

This branch requires sensitivity review and canon validation. It cannot reveal a secret that retroactively changes an established survivor’s biography without an approved narrative decision. If the existing cast already has a comparable history, the branch should attach to that character or be removed. The main quest does not require it, and no faction standing depends on finding it.

#### A Board Is Not a Witness — repeatable practice

After the investigation, the player may occasionally help review new roster entries with a character. Each appearance uses a finite authored variant: new task, new source, and one practical ambiguity. The player chooses whether to seek confirmation, annotate a planned assignment, or leave the line unchanged. The task only repeats when the world has a new reason to need review. It should not trigger every expedition or reward the same proof repeatedly.

If no rotating-content owner exists, ship one or two static board reviews. If the current quest system has no repeatable completion policy, do not simulate one with duplicated quests. Reuse a single authored conversation only if the current runtime can reset or reopen it safely and deterministically.

### Portfolio balance map

| Quest | Dominant player verb | Criticality | State owner candidate | Distinct failure |
|---|---|---|---|---|
| The Empty Shift | Compare and report | Main | Quest lifecycle | Resolve with uncertainty |
| The Glove With the Blue Stitch | Trace and return | Optional | Item/quest | Leave in communal bin |
| The Quiet Count | Choose and explain | Optional | Food owner if verified | Use documented estimate |
| The Dry Copy | Preserve | Timed optional | Location/environment if supported | Use duplicate source |
| The Walk Before Dark | Protect and hand off | Optional | Expedition/character | Delay travel safely |
| The Line Reversed | Interpret evidence | Optional branch | Quest facts | Mark notation ambiguous |
| The Bench Ticket | Repair or salvage | Optional | Crafting/location | Close as deferred repair |
| The Name Under the Fold | Discover and ask | Hidden | Journal/quest | Leave the page untouched |
| A Board Is Not a Witness | Review and annotate | Repeatable concept | Existing quest/event | Skip without campaign loss |

This table is a portfolio review tool, not a proposed enum or data schema. If a candidate owner is missing, its system connection is deferred. Production review should remove cards whose dominant verb duplicates a core quest without adding a new choice.

### Player-state profiles

**A player who accepts the main quest early** should see the next useful objective and retain the opportunity to do optional work later.

**A player who discovers the board before acceptance** should not need to inspect it twice. The quest start recognizes the previously observed fact only if the existing discovery owner persists it.

**A player with severe survival pressure** should be able to defer optional character scenes and still close the main investigation.

**A player who refuses the task** should not receive repeated pressure. The board remains available if a later explicit event reopens it.

**A player who uses only indirect evidence** can reach a lower-confidence result and receive a complete closure.

**A player who loses one copy** keeps progress from the second source and receives a fallback, rather than an impossible fetch step.

**A player who chooses to preserve the uncertain roster** may later see a new report if the current world state supplies one. The original outcome is not automatically overwritten.

### Quest lifecycle and UI contract

The quest log distinguishes: available request; accepted investigation; active evidence-gathering; blocked by a specific unavailable source; partially complete with known evidence; resolved with verified, partial, or uncertain result; and abandoned/expired where applicable. These labels map to current lifecycle values after inspection. The journal summary contains one short next action, one sentence on evidence already gathered, and the current limit. Map pins appear only for known, selected, actionable places. A blocked quest should not show a pin to a site that cannot be visited unless it is explicitly an approximate lead.

On acceptance, show any material risk: possible exposure, the fact that a source may be unavailable, or the need to carry a fragile copy. On completion, show the selected outcome and any actual reward through the current UI. Do not use celebratory language for an unresolved record. A quiet “You recorded the mismatch; attendance remains unconfirmed” is more truthful.

### Integration readiness gates

Before content entry: verify canon fit and existing character opportunities; locate the current quest schema; map supported lifecycle, objective, branch, and result fields; identify exact owner for every proposed state effect; inspect the save restore route; confirm location catalog and map consumer; review available skills, inventory, food, work, and maintenance authorities; and identify the content integrity command.

Before runtime implementation: claim exact paths; complete a bounded host/core/data package; add only the focused tests required by the task and policy; verify save and determinism behavior for stateful facts; and trace from player entry to result after reload. These plans remain conceptual until that work is authorized and owned.


### Quest state transitions and authored transition contract

The Empty Shift uses many familiar lifecycle names, but a large vocabulary can become an implementation liability if every word turns into a new runtime state. The plan therefore distinguishes the player’s conceptual status from the current quest owner’s actual state. Before data entry, map the following transition table to the existing API and save representation.

| Player-facing situation | Trigger | Information retained | Next useful action |
|---|---|---|---|
| Request can be accepted | Board or character reveals job | Entry source if later response uses it | Accept, decline, or ask scope |
| Lead discovered | Player sees roster or clue before accepting | Discovery fact if existing owner persists it | Start later without re-inspection |
| Accepted | Player chooses to take the task | Contract and basic scope | Inspect copies or visit Annex |
| In progress | A supported evidence interaction succeeds | Each distinct source and objective proof | Seek another source or report |
| Partially resolved | Some evidence exists but result not chosen | All proven observations | Compare, ask, or stop |
| Blocked | A required route is unavailable | Prior proof plus reason for block | Use alternate evidence or wait |
| Failure-forward | One source was lost or one interpretation disproved | Remaining proof and the loss | Reframe the question |
| Resolved verified | Evidence supports the chosen correction | Result and evidence basis | Receive acknowledgment |
| Resolved uncertain | Evidence does not support a safe correction | Explicit uncertainty | Close with no false assignment |
| Abandoned | Player explicitly leaves the work | Any irreversible action already applied | Reopen only on an authored event |
| Expired optional | A temporary task window ends | History of the offer only if needed | Optional later report or closure |
| Reopened | New source creates a genuinely new task | Prior result plus new evidence | Reassess, do not replay original start |

The integration owner may map several rows to one current state and use objective/result facts to distinguish them. That is acceptable if the player-facing journal remains accurate and restore behavior is deterministic. The table is a content requirement, not a demand for a new enum.

**Transition invariants.** Evidence already gathered survives blockage. Failure-forward routes preserve unaffected evidence. Resolved quests do not continue showing urgent objective markers. Abandonment does not reverse an already applied board edit. Expiration affects access to a time-bounded optional action, not the truth of what happened earlier. Reopening has a new cause and a visible reason. No random expedition roll may move a quest from active to failed.

### Branch contract for the correction meeting

Each report option is represented as a compact branch contract.

**Correct the roster.** Preconditions: enough independent evidence to identify the current assignment and the current board owner can apply the change. Confirmation copy: “Replace the planned names with the verified coverage.” Result: updated record; only a verified downstream assignment effect may follow. Fallback: annotate that the names are unconfirmed.

**Annotate the roster.** Preconditions: at least one source shows that planned work and attendance are distinct, or the player explicitly chooses caution. Confirmation copy: “Keep the current names and label them as planned coverage.” Result: uncertainty is visible. Fallback: if the board owner cannot persist annotations, resolve as a narrative report and do not imply a public edit.

**Request a second account.** Preconditions: a plausible source exists or can appear later. Confirmation copy: “Leave the entry unchanged while you seek another account.” Result: quest remains active/blocked if supported. Fallback: unresolved closure if the source becomes permanently unavailable.

**Close unresolved.** Preconditions: player may close the investigation at any point after seeing the mismatch, or after losing access to all routes. Confirmation copy: “Record that the copies match but attendance is still unknown.” Result: quest resolves with explicit limits. Fallback: none; new evidence can reopen only by authored cause.

No branch should be a hidden punishment option. If the player cannot know that correction is permanent, the UI needs a confirmation according to existing UX conventions. If a branch can be revised later, describe the revision limits.

### Objective lifecycle and journal composition

A quest objective should be written as a player action plus proof. “Learn the truth about the night shift” is too broad. “Compare the roster copies” is actionable. A journal entry can then show: action, proven detail, remaining question. Example: “Both copies list the same names. The correction mark is still unexplained; the Annex panel or a second record may help.” When the player later learns the mark’s possible meaning, update the summary without erasing the fact that attendance remains unknown.

The log should not show all internal branch keys. Use a short objective title and one next action. Supporting evidence can be grouped beneath it if the current journal supports that presentation. If not, keep the summary concise and use dialogue to remind the player of a relevant clue. Do not create a custom evidence board UI just for this chain.

A quest with several optional paths should distinguish required progress from bonus context. If the hidden name impression is found, the journal may add an optional note. It does not replace the main next action or imply that a personal confession is needed. Completion is determined by the authored contract, not by the number of nodes visited.

### Reusability and objective templates

Reuse should happen through stable content patterns, not a new generic quest framework. The first few Empty Shift tasks can demonstrate:

- Compare two authored records.
- Inspect an environmental condition.
- Ask a named witness with an absent-speaker fallback.
- Deliver a report to a supported receiver.
- Protect a traveler through an existing expedition route.
- Repair or recover a supported item.
- Resolve an investigation with uncertainty.
- Revisit after a meaningful milestone.

For each pattern, document what remains invariant and what changes per quest. A record comparison always needs a source pair and a supported conclusion; the actual note and stakes vary. A witness quest always labels direct versus reported knowledge; the witness and setting vary. A route escort always needs a safe failure-forward outcome; destination and material risk vary.

Do not instantiate these patterns as hard-coded templates until repeated use proves that the current data format cannot express them cleanly. Template abstraction can make content less legible if every quest’s real differences are hidden behind generic properties. Start with clear authored records, identify repeated fields from actual examples, then propose a schema-level improvement through the normal owner process.

### Playtest observations to capture

When this slice is eventually playtested, record whether players know what proof is needed; whether they mistake the roster entry for attendance; whether they infer fraud too quickly; whether the blocked state gives a useful alternative; whether they understand that the unresolved result is complete; whether the optional resource/crafting tasks compete with survival; whether the map includes too many shelter locations; whether the journal repeats dialogue; and whether the final choice feels informed.

Do not interpret all player disagreement as confusion. If some players keep the roster unchanged while others annotate it, that may indicate a real value tradeoff. If most players believe the game secretly has a correct answer, the evidence framing or reward structure needs revision. If the player tests only one clue and concludes beyond what it supports, dialogue should better communicate the confidence boundary.

### Change-scope boundaries

The first integration package should not include new scheduling simulation, a new resource ledger, a general-purpose rumor engine, a procedural witness system, or a new campaign event manager. It may extend an existing quest or journal owner to persist a small result only after exact API evidence and a path claim. If a broader owner is required, split that work into a separate plan with a clear player benefit and migration/test obligations.


### Quest architecture contract: authored definition, runtime instance, and outcome

A maintainable quest design separates three concepts even when the current implementation stores them together. The authored definition describes stable content: purpose, start conditions, objective graph, location and character dependencies, possible outcomes, rewards, and fallback. The runtime instance describes one player’s current progress through that definition. The outcome records the result that later content may consume. This conceptual separation prevents generated expedition details or player progress from being mistaken for permanent authored content.

The existing quest authority determines the actual types and serialization. The content author should not put runtime progress into a catalog definition. A definition does not become a save record. A generated site does not own quest completion. A dialogue node does not determine whether the objective is complete. The current quest owner holds the instance and result; other systems expose their own state through established contracts.

For the Empty Shift, the authored definition can specify that inspection of a roster yields a clue, while the runtime instance records that this player observed it. The outcome can represent verified correction or unresolved attendance. If the actual architecture expresses these differently, preserve the semantics while using current types. Avoid creating a generic “quest service” that duplicates an existing manager or save section.

### Quest graph constraints

Every graph has a start, one or more reachable objective routes, and at least one terminal outcome. The main investigation can begin at the board or from Mara if the existing quest owner supports multiple starts; otherwise the first slice chooses one start. Optional discoveries can add context but cannot strand the player on a required branch.

The graph must specify:
- whether an objective is sequential or can be satisfied in any order;
- whether evidence is consumed, retained, or shared;
- when a branch becomes irreversible;
- whether a blocked objective permits another action;
- how failure-forward closure differs from abandonment;
- whether a new source can reopen a resolved question;
- how journal text is selected for each state;
- which later scenes consume the outcome.

If the player can inspect the Annex before accepting, a start condition may read the existing discovery fact. If that fact is not persisted, the quest should not imply prior knowledge. If the player can gather evidence out of order, objective completion should be recognized on acceptance only when the current owner supports event history. Otherwise make the clue re-discoverable through a later visit.

### Branching potential and critical-path isolation

Branching should change the player’s information, action, or later access, not multiply dialogue volume for its own sake. The report choices produce four bands, but the next ordinary shelter conversation can reconverge. A main quest branch remains distinct only while its outcome affects a later task or character response.

No optional branch should be a hidden prerequisite for campaign completion. The hidden name impression can affect an optional character beat. The meal-count recommendation can affect an existing food owner if supported. Neither is required to visit the Annex or resolve the roster. If the late-game record-keeping policy consumes one branch, it must also accept an absent or unresolved result with a coherent default that does not erase the player’s past.

A content map can mark edges as hard progression, optional context, or delayed callback. Reviewers should trace the hard path with every optional node removed. The main quest must still start, gather sufficient evidence or state its limits, resolve, and close. This cut-down graph is the minimum viable path.

### Availability and expiry by quest type

**Main quest:** remains startable through at least one stable route; does not expire because a random site was not selected.

**Character quest:** can be unavailable while the character is absent, but its personal scene needs a fallback or a clear optional closure. A character’s absence does not retroactively invalidate an account already heard.

**Faction quest:** this story does not introduce a new faction. If an existing faction adopts the board practice, access is controlled by that faction’s current authority and should remain optional unless progression explicitly requires it.

**Discovery quest:** can be missed if hidden, but its absence cannot block the main quest. A clue should have an environmental route and a clear discovery action.

**Investigation quest:** may complete at lower confidence if evidence paths are lost. Its unresolved result is authored, not an automatic failure.

**Escort/protection quest:** has an explicit travel window only when the current time or expedition system can express it. Failure moves to a report or delayed handoff, not an unannounced character loss.

**Survival quest:** should connect to an existing survival pressure and offer a way to decline or defer. Avoid adding a bespoke survival meter.

**Resource/crafting quest:** uses current item recipes and inventory ownership or remains a narrative request. No parallel material count is allowed.

**Location-based quest:** has exact map visibility rules and location fallback. It does not rely on a place that has no selected route.

**Timed quest:** defines start, expiry event, player warning, and aftermath. If no time authority supports it, use an interaction boundary rather than a fictional clock.

**Repeatable/rotating quest:** has a finite cadence, new circumstance, and no duplicate infinite reward. It is deferred if the current runtime cannot safely reset it.

**Hidden quest:** is fully optional, discoverable through a plausible clue, and excluded from critical path.

**Environmental start:** begins from a deliberate interaction or clue, not passive rendering alone.

**Choice-reactive quest:** reads a supported prior outcome, not an invented flag.

**Failure-forward quest:** loses one route but continues with a truthful alternative or uncertainty closure.

### Reward and production-cost policy

Rewards should match the task. An evidence-rich investigation may unlock a better-informed option. A repair task may use existing craft costs and outputs. A character quest may yield a new conversation or safe work offer if supported. An escort may reduce uncertainty about a route. Do not add a new numeric reputation reward for every action.

For each quest, estimate number of unique dialogue nodes, conditions, locations, art assets, items, translations, objective proof cases, save profiles, and cross-system owners. Distinguish one-time creation cost from ongoing maintenance cost. A repeatable task with ten variations may require more QA than a single main quest. If its replay value does not justify the cost, reduce it to two authored variants.

### Architecture readiness decision

The plan is implementation-ready only when the current quest definition schema can express required objective and outcome semantics; a runtime owner tracks progress; a save owner restores state if needed; a location owner provides reachability; dialogue uses the same facts; and current integrity tooling validates references. If any required seam is missing, the first work package can still implement a narrative-only subset, but the missing architecture must be separately approved before systemic claims are made.


### Implementation rehearsal: acceptance by slice

**Slice 0 — premise check.** Before an implementation claim, inspect current quest catalogs and runtime consumers for roster, work-board, and shift content. Record whether the story is an extension, a functional duplicate, or new space. Check existing survivor histories before assigning any candidate role. Verify current status labels and save behavior. If the premise fails, adapt or retire this thread.

**Slice 1 — narrative-only investigation.** Add one reachable start, one board inspection, one independent account or authored fallback, and an unresolved terminal result. Use the current quest and dialogue owners. No assignment, food, or maintenance change is included. Acceptance: no duplicate quest start; evidence is not overclaimed; all outcomes close; save restore does not restart the scene.

**Slice 2 — location routing.** Add one Annex interaction or existing location equivalent, one fallback location/report, and accurate expedition-map wording. Acceptance: active main progress remains possible if the preferred place is absent; discovery is not repeated; optional sites do not consume mandatory capacity.

**Slice 3 — characters and optional tasks.** Add one character quest and one short survival or escort activity using existing systems. Acceptance: missing character and failed route have alternatives; optional activity can be declined; rewards cannot be duplicated.

**Slice 4 — systemic effects.** Only after source evidence, connect the selected report to an existing assignment, resource, or maintenance owner. Acceptance: one command route owns the change; persistence and deterministic behavior are tested; UI reflects actual state.

Every slice can be shipped, paused, or cut independently. A later slice does not retroactively justify unverified effects in an earlier one.

### Quest package sign-off questions

The content lead confirms the player action and voice. The quest owner confirms state and objective mapping. The location owner confirms reachability and map behavior. The save owner confirms any persistent result. The UI owner confirms journal and response presentation. The named integrator confirms exact path claims and acceptance. A role without an available owner becomes a documented dependency.

The package should answer: Can a player reject the task? Can they make progress without the optional clue? Can they lose one route without losing all evidence? Can they finish without accusing anyone? Can a save restore midway? Can a later conversation report the outcome accurately? If any answer is no, define the missing behavior or simplify the story.


### Future handoff fixture for the main quest

The integrator can use one handoff fixture to check the contract without implementing every optional card. Start with no quest facts. The player accepts from the board, inspects both copies, receives an alternate account because Mara is unavailable, inspects the Annex panel, and chooses unresolved because attendance is still unproven. The journal closes with the exact observations and limit. Save and reload; the quest remains resolved. Reopen the board; it does not offer the same main investigation again.

A second profile finds the margin note before acceptance and selects annotation after asking Mara to explain its limit. The quest recognizes prior discovery, applies the outcome once, and exposes the correct callback. A third profile loses the panel route and uses a service-log fallback. These profiles exercise the high-risk contracts while leaving optional food, crafting, hidden, and repeatable content deferred.

Record expected entry, proof, state, map, and journal after each step. This fixture is a future implementation aid; the owning test and content policy determine whether it becomes automated or remains a manual verification route.

### Quest content review: production-cost checkpoints

Estimate production cost before expanding the anthology. Count unique nodes, conditional variants, authored evidence objects, required locations, NPC presence states, map and journal strings, rewards, and closure profiles. Mark which costs are one-time, such as a new scene backdrop, and which persist across every future edit, such as a branch matrix or migration contract.

A side quest is a candidate for removal when it repeats the main question without a new verb, has no distinct failure route, or needs a new system only to grant a small reward. Keep it when it gives the player a different kind of agency: protect a worker, preserve a document, repair a panel, or choose not to infer. Production scope should follow playable difference, not page count.

Before approval, the narrative lead and technical owner agree on a minimum slice and a cut line. If the schedule tightens, optional scenes can be removed in a documented order while the main quest’s evidence and closure remain intact.

### Reopen policy and new evidence

Reopening is not a retry loop. It is justified only when new evidence changes what the player can responsibly conclude or when the world creates a genuinely new task. The old result remains part of history. The reopened objective identifies the new source and asks a narrow follow-up, such as whether a corrected roster uses the same meaning for “covered.” It does not send the player back through the original investigation.

If the current quest owner cannot represent a reopened result, create a separate authored follow-up that references the earlier outcome through an existing fact. Do not reset a completed quest or clear its reward flag. If there is no supported reference, keep the callback as dialogue and avoid claiming that the earlier quest reopened.

### Quest handoff note

The design handoff should include one primary route, one fallback, the unresolved result, and the exact owner questions still open. It should not ask the implementer to infer whether a clue is mandatory or whether a side quest can be skipped. A short, explicit contract saves more time than a larger set of prose examples.

## Pass 15 — Winter-horizon quest portfolio and lifecycle reconciliation

**Status: PROPOSAL, premise-gated.** This pass uses the master world bible's high-confidence Days 90–180 pacing gap as a bounded content target. It does not authorize a new quest engine, new quest catalog, or new status enum. Current source already contains QuestRuntimeCoordinator, QuestInstanceState, QuestLifecycleState, NarrativeQuestlineSystem, DynamicQuestGenerator, domain quest owners, and several authored quest catalogs. The integration course begins by identifying which of those currently owns each candidate beat.

### 15.1 Correction to the previous pass

The provisional Empty Shift storyline must be treated as a collision-review packet. The current game has DutyRosterSystem and DutyRosterQuestRuntime, plus duty-roster location, quest, mark, and season catalogs. Any “stale roster,” shift reconciliation, roster fire, work-post dispute, or shift-ledger witness concept may overlap those live systems. Retain only the parts that survive an entity-by-entity comparison; merge an accepted story beat into the canonical duty-roster owner and existing catalog, or retire it from the proposal. No independent roster quest, location, actor, state, save section, or ledger is implied. Existing text remains DRAFT pending that comparison.

### 15.2 Evidence boundary and question to answer

The live QuestRuntimeCoordinator owns a player-facing aggregate read model and the common lifecycle transitions Offered, Active, Completed, Failed, Expired, and Abandoned. It also stores definition and instance identifiers, actor/location bindings, objective runtime state, reward bindings, failure consequences, generation seed, parent/child links, and deadlines. That is a useful common seam, but it is not evidence that every specialized quest domain should migrate into it. Domain-specific owners stay authoritative for their specialized state. The current DynamicQuestGenerator source also documents a canonical path in which ProceduralNarrativeSystem supplies candidates and QuestRuntimeCoordinator receives an accepted instance; older generator templates are not approval to create a parallel pipeline.

Before selecting a quest record, the premise audit must answer, with current code and data citations: which engine emits the initiating event; which catalog defines the authored content; which owner decides objective truth; which owner grants rewards; which world facts a branch reads; whether the location is reachable; and whether the content utilization path proves the record can appear in play. A record present in JSON without an eligible event and consumer is not a playable quest.

### 15.3 Days 90–180 narrative portfolio

Working title only: The Long Thaw. Do not reserve an ID or imply canon until the named entities and catalog placement are checked. The portfolio responds to the bible's “mid-winter slump” observation without inventing a generic scarcity meter. It uses existing day, weather, crop, power, medical, faction, expedition, and Chronicle facts only after their current owners are verified.

| Quest shape | Player-facing problem | Typical objective pattern | Failure-forward route | Design purpose |
|---|---|---|---|---|
| Investigation | Several maintenance reports disagree about why a district's heat margin is narrowing. | Compare a physical trace, a log, and a witness; return with a defensible explanation. | The cause stays uncertain; the player can still choose a conservative operating procedure through the relevant owner. | Let evidence quality matter without making perfect diagnosis mandatory. |
| Protection | An exposed community requests a short, specific escort window for a repair crew. | Commit a party, choose a route, and protect the crew's return. | A late arrival changes the request into recovery, evacuation, or a written warning; it does not erase the next arc. | Convert strategic pressure into an expedition decision. |
| Resource and crafting | A damaged part has a substitute recipe, but the substitute consumes material needed elsewhere. | Obtain a documented item set or ask a faction for a loan. | A failed craft consumes only what the current crafting owner says it consumes; quest recovery may use barter, salvage, or a reduced-output repair. | Make costs legible and owner-resolved. |
| Character | A survivor wants the group to stop describing a past failure as proof of incompetence. | Offer testimony, private conversation, or a practical re-assignment. | Silence is recorded as unresolved, not as consent; later scenes can revisit through a valid trigger. | Give relationship consequences without a new relationship axis. |
| Faction | Two established groups disagree over who may service an exposed shared route. | Present evidence, carry a limited offer, or refuse arbitration. | Each position gains a stable consequence; neither required path depends on a single faction being liked. | Tie faction standing to access or future dialogue through its live owner. |
| Discovery | A familiar route reveals a changed physical mark after an ash event. | Inspect the mark and connect it to an existing map or Chronicle fact. | Missing the first opportunity leaves an alternate clue source if the content is meant to be required. | Reward expedition observation while preserving progression. |
| Timed | A repair window narrows over several in-game days. | Decide what can be completed before the deadline. | Expiry transitions through the coordinator's existing lifecycle and opens a salvage, apology, or changed-service branch. | Make deadlines consequential but recoverable. |
| Repeatable | A settlement requests small seasonal checks when weather and access conditions permit. | Choose one suitable task from an authored set. | Cooldown or eligibility is enforced by its current owner, not by another quest ledger. | Reuse locations and procedures without repeating the same text. |

The portfolio is a set of candidate shapes, not eight approved quest IDs. The data audit must search all current quests, dynamic templates, area arcs, encounter catalogs, faction branches, side-quest families, and hidden/discovery content. Consolidate overlapping beats before drafting production prose.

### 15.4 Lifecycle projection instead of lifecycle duplication

The earlier user-facing state list—Inactive, Available, Discovered, Accepted, In Progress, Blocked, Partially Completed, Failed, Completed, Resolved, Expired, Abandoned, Reopened—must be treated as a vocabulary of player-visible conditions, not a demand to add thirteen more enum values. The current runtime enum has six states. A plan may propose a read-only display mapping only where the current model gives enough evidence:

- An authored definition that has not met its reveal conditions can remain unlisted; do not persist a second Inactive status.
- An eligible, surfaced offer can display as Available or Offered according to the current UI vocabulary, with the canonical runtime value preserved.
- A discovered clue may be represented by the owning narrative/map evidence only if that owner already persists it. Do not duplicate it as a quest state.
- Active with one or more incomplete objectives is In Progress. Partially Completed is a derived display only when objective progress supports that distinction.
- Blocked should identify a real prerequisite or a waiting condition visible to the player. It must not conceal impossible content. If no current model can express it, record a specific integration dependency rather than invent a new saved status.
- Failed and Expired remain distinct where the runtime records them. Recovery content is a new eligible route governed by its owner, not resurrection by mutating history.
- Completed and Resolved may be separate presentation labels only if a domain outcome needs post-objective closure. The resolution fact must have one canonical owner.
- Abandoned remains a player action. Reopened is not assumed to exist; propose it only if the owner has an explicit, deterministic transition and a reason that preserves the original terminal record.

Acceptance requires proving that UI state is derived from the single canonical runtime/domain authorities and cannot drift after load, reset, or late event delivery.

### 15.5 Failure-forward contract

For each required objective, the record must declare: the observable completion fact; the owner that emits it; whether it is reversible; the point after which the player has knowingly committed; the deadline rule; the failure transition; the alternate route; and the consequence owner. “Quest failed” may describe the outcome but cannot be the only behavior. A missed escort can produce a survivor's departure, delayed medical access, a hostile rumor, or a different discovery route only when corresponding existing systems can truthfully apply those facts.

Never punish the player for a location that did not spawn, a hidden prerequisite that was never signaled, an actor unavailable after a prior branch, or an item that the authoritative catalog cannot produce. If a branch cannot be completed, use an explicit delay, substitute objective, clue-forwarding route, or documented non-critical expiry. Do not mark an impossible objective as silently failed.

### 15.6 Delayed choice callbacks

The world bible calls out early DoorEncounterSystem decisions that lack multi-month delayed callbacks. Current DoorEncounterSystem resolves a choice, records resolved encounter IDs, updates its own cumulative reaction facts, emits OnEncounterResolved, and captures/restores its state. Its inspected entry and state contracts do not themselves provide a general due-date scheduler. The bible names IFlagLedger as a possible seam; current repository also contains Flags/IFlagLedger.cs and CampaignConsequenceLedger.cs. Their current contracts must be read before any proposal assumes that they can schedule and deliver a future encounter.

Use a delayed callback only as a content chain through established owners: the first choice records its existing canonical fact; a later day/event gate becomes eligible; the callback is authored in the owner’s existing encounter or quest catalog; a delivery receipt makes duplicate application harmless if the scheduler supports it; a missing window falls back to a letter, radio report, or faction intermediary if those consumers exist. Never append a second date queue to DoorEncounterSystem or write “returning in 100 days” as though the data currently guarantees it. If the required due-date seam is absent, make the callback a separately reviewed Core extension with migration, deterministic time semantics, save/restore, and exactly-once acceptance criteria—or leave the narrative as a non-timed follow-up.

### 15.7 Record and review template

Every quest candidate in this arc should be submitted with the following record:

- Status labels: CANON for verified current facts; PROPOSAL for the new design; DRAFT for prose; INFERENCE for a likely gap not yet proven.
- Current content matches: catalog path, existing IDs, current stage/choice behavior, integration consumers.
- Player purpose: what the player learns, risks, gives up, or changes.
- Quest type and acquisition channel: main, character, faction, discovery, investigation, escort, survival, crafting/resource, location-based, timed, repeatable, hidden, environmental, choice-dependent, or failure-forward.
- Required locations and valid substitutes; map visibility timing; expedition selection priority.
- Actor knowledge at each node; faction state; time window; evidence prerequisites.
- Objectives and owning event for each objective.
- Failure states, fallback route, terminal meaning, and any re-open proposal.
- Rewards with their canonical reward owner and item/faction/location references.
- Reuse plan, repeat cap/cooldown owner, and replay behavior.
- Production cost: DATA-ONLY, DATA + MINOR WIRING, CORE EXTENSION, CROSS-SYSTEM, or FOUNDATIONAL, with evidence for the classification.
- Core or expansion classification and reason.
- Structural, continuity, gameplay, prose, save, determinism, accessibility, utilization, and UI review criteria.

### 15.8 Proposed minimum playable slice and exit gate

The minimum slice is one short investigation that can be entered during the middle horizon, one optional expedition discovery that enriches but does not block it, and one later callback whose delivery does not depend on the player revisiting a specific NPC. It must reuse a verified location, existing quest owner, existing clue channel, and canonical failure path. It should demonstrate success, refusal, expiry, missed-location fallback, and save/reload in the corresponding owner. This is a proposal, not a new batch claim.

The plan is ready for a premise audit when each candidate maps to an existing producer and consumer; each required location has a verified ID and a reachable fallback; no duplicate roster, quest, relationship, memory, or world state remains; effects have a named owner; and unresolved architecture decisions are listed rather than improvised. No test run is claimed in this documentation-only pass.

### 15.11 Quest blueprint cards for the winter horizon

The following cards are deliberately written as prose-ready design packets rather than production IDs. Each must be reconciled with existing content before adoption. “Existing site” means a specific current location selected by the premise audit; it is not an implied new location.

**Card A — Three Readings Before Dawn.** Type: investigation, with an optional expedition leg. Purpose: give a player in the Days 90–180 horizon a problem that is solvable through careful records rather than combat. Opening: a service summary has a single confident conclusion, while two independent readings disagree about when a cold room began losing heat. The registrar can preserve both times; the maintenance worker can explain what each gauge measures; the expedition lead can retrieve an old field copy if a current location supports that trip. Objective variants: compare both records; ask the worker to repeat the test; or accept the safe operating procedure without resolving the cause. Failure states: the field copy is inaccessible, the deadline expires, or the player declines further testing. Failure-forward result: the shelter adopts the conservative procedure and the cause remains disputed; the next request can still occur. Rewards: access to the verified procedure or a journal entry, not a free resource bundle. Branching: the player chooses whether the archive presents observation, attribution, or uncertainty. Reuse: later quests may cite the procedure but cannot relabel it as proof of sabotage. Cost: likely data-only if existing encounter/quest fields support the record and outcomes; otherwise a small owner extension. Core/expansion: core-candidate because it teaches how to read systemic evidence, but content review may reserve it for an expansion if it lengthens the early campaign.

**Card B — The Part That Fits.** Type: resource/crafting and character. Purpose: force a visible trade between an immediate repair and a later contingency. The work area exposes a substitute component that fits but has a shorter supported service life. The player can commit the item, reserve it, or authorize a trip for another part. No choice should be represented as a guaranteed quality roll until the crafting owner confirms the outcome. Failure: the trip is delayed or the substitute is spent elsewhere. Alternate route: accept a slower service schedule, ask an existing faction for a documented loan, or continue without the repair and accept the consequence reported by the owning system. Reward: stable access, knowledge, or relationship response; do not grant a second item through dialogue if crafting already consumed it. Reuse: same choice pattern can be used in unrelated equipment content only with distinct stakes and material. Cost: DATA-ONLY when existing crafting and quest contracts express the requirement; CORE EXTENSION only if the result cannot be observed through current events.

**Card C — The Return Window.** Type: escort/protection with a deadline. Purpose: put a human face on route selection without using a binary “save everyone or fail” structure. An existing community or faction requests protection for a brief work party traveling through a known corridor. The objective can be fulfilled by escorting the party, securing the crossing first, or arranging a verified alternate return. Failure: the party misses the window, the route closes, or the player declines. Failure does not automatically mean death; use only outcomes that the current expedition/combat/character owners can record. Alternative close: the request becomes a recovery operation or a message explaining the route change. Rewards: trust/access through the faction owner; no uncatalogued gear. Reuse: escort structure can recur with different route pressure, but exact dialogue should not loop. Cost: cross-system if combat, expedition, quest, and faction results all interact; scope down if the event path cannot guarantee exactly-once effects.

**Card D — The Unsent Copy.** Type: character, discovery, and dialogue. Purpose: show how a private record changes meaning when the player distinguishes a person's words from an institution's conclusion. A document found at a verified existing location contains a copy with one sentence omitted from the public record. The player may deliver it, preserve it, or present it with a caveat. Failure: the player never discovers the record or chooses not to carry it. Alternate route: no mandatory progression is locked; a later public account may reference the missing sentence only if the current information path makes it public. Reward: character acknowledgement, a clue link, or one Chronicle input only if supported. Reuse: the document format can recur, but not the same reveal. Cost: content-only if existing codex, quest, and journal consumers suffice.

These cards model consequence breadth and cost. They are not a promise that all four fit the existing timeline or that their specific prose is canon.

### 15.12 Objective contracts and event naming

Use objective names for author review, not as new API. The production objective definition must resolve to a fact emitted by an owner, such as “encounter choice resolved,” “location discovered,” “item delivered,” or “character dialogue completed,” only where that event actually exists. An objective that says “understand why it happened” needs an observable evidence criterion; player intent cannot be guessed from a line selection.

For each objective, list its completion event, duplicate-event behavior, source owner, timing, and persistence. If two systems can emit similar events, include a stable source instance reference so that completing one unrelated expedition cannot advance another quest. If the current event contract has no instance ID or exactly-once guard, that is a design seam to audit, not a reason to poll arbitrary state every frame.

### 15.13 Quest portfolio balance

Across a campaign horizon, alternate pressure modes: one objective centered on information, one on movement, one on resource commitment, one on social repair, and a quieter optional discovery. Do not put a timed deadline on every request. Maintain a visible mix of mandatory, optional, hidden, repeatable, and expired content, with optional tasks providing texture rather than indispensable items. Verify the actual content distribution from the current catalog before asserting a gap.

A time-gated record must reveal the applicable window and explain the consequence of waiting. A repeatable record must state what repeats (need, procedure, reward, or location) and what changes (weather, actor, available evidence, or branch). A hidden record needs a legitimate discovery source and must not silently vanish if that source is permanently missed. A quest with dynamic bindings must preserve stable player-visible meaning across valid candidates.

### 15.14 Integration closeout receipt

The future implementation handoff for this plan must contain: one selected owner and catalog per quest; a dependency list in integration order; a frozen accepted content slice; current file claims; migration note if any serialized contract changes; exact focused verification command(s); expected before/after observations; data-integrity and utilization evidence; continuity report; deferred decisions; and rollback boundary. A green schema check alone cannot prove that the quest appeared in a playable expedition. A passing quest test alone cannot prove that its location or dialogue is reachable. No receipt should be filled in until implementation actually occurs.


## Pass 16 — World-bible bridge portfolio: signal, coast, and shelter memory

This pass turns three expansion seeds in the master world bible into candidate quest packages. It deliberately connects them to existing narrative content authorities and runtime owners. Every new quest ID, location ID, faction tie, reward quantity, and gate below remains a proposal until a focused premise audit confirms the live catalog, quest runner, destination route, and ownership ledger. The intention is to make existing authored records playable as coherent journeys, not to make a second cipher, discovery, map, or cohort system.

### Evidence boundary and reuse rule

The source snapshot reviewed for this pass shows a persistent CipherQuestChainEngine with three authored chain definitions and save capture/restore. SignalIntelligenceCatalog reads the numbers-station cipher corpus. AbyssalAnomaliesCatalog reads hydrophone acoustic logs, and NarrativeDiscoveryCatalog plus the discovery manifest already expose selected records as discoverable content. DailySurvivalCatalog loads the base children’s folklore catalog and batch two. The manifest also contains folklore and hydrophone discovery rows. CohortSystem and GenerationalLineageExtension are existing owners for maturation and family history; the exact bridge from folklore records to mature-cohort dialogue is not established by the reviewed evidence. Therefore the folklore-to-adult thread is a gated content proposal, not a claim that maturation callbacks already exist.

The old Empty Shift candidate remains under collision review because DutyRosterSystem, DutyRosterQuestRuntime, and duty-roster data already exist. Do not use these new packages to create a competing shift assignment or shelter duty authority.

### Candidate package A: The Winter Count

**Purpose.** Expand the existing cipher-hunt chain into an investigation that links a repeated broadcast, a verified decoding step, a mapped destination, and a human explanation for why the count was preserved. The quest must call the current cipher chain owner and respect its existing flags and restore behavior.

**Typical structure.** A radio operator notices that a station’s count changes on a seasonal cadence. The player hears a broadcast, finds or earns access to the relevant key material, decodes the message, and follows the already-authored target-location reveal. At the location, a noncombat investigation resolves whether the count marks stored medicine, burial places, or a correction to a past evacuation list. The final answer can vary in tone and local consequence while the established chain resolves once.

**Required locations.** The starting shelter/radio context and the exact target location in the selected existing chain. Any proposed archive room, repeater, or relay site needs an entity-by-entity check against the current map catalog before it is named as a required node.

**Possible failure states.** The player may miss a broadcast window, destroy or sell a clue item, reach the target before decoding, or decode incorrectly. The chain should remain recoverable through the existing state model: repeat the broadcast, recover an alternate clue, or obtain an explicitly authored correction from an NPC. A failed interpretation may reduce the optional reward or alter who trusts the player, but it must not silently make the target impossible.

**Rewards and costs.** Prefer a modest information reward, journal evidence, a relationship change, or a location unlock already supported by current owners. Avoid minting a novel currency or permanent stat. The content cost is moderate: one hub scene, one clue beat, one destination scene, a failure-forward clarification, and a compact resolution callback. The production cost rises sharply if an additional location or bespoke radio presentation is required.

**Reusability.** The same scene skeleton can serve other existing cipher chains if the target, speaker, and interpretation are data-bound. Do not clone a new quest runner per chain.

### Candidate package B: The Shelf That Answers

**Purpose.** Let hydrophone evidence support a location-based investigation while preserving a meaningful gap between an acoustic observation and a confirmed explanation. Existing records include natural, mechanical, biological, and hostile-sounding signals; the player should not receive a guaranteed correct interpretation merely for collecting a log.

**Typical structure.** A shelter technician asks the player to compare two listening records. A first expedition can surface a clue or discovery record; a later trip, instrument calibration, witness conversation, or map inspection can improve confidence. The resolution may identify a safe route, a hazard, or an unresolved sound that remains part of the setting. Build candidate branches around records that actually exist and are currently reachable after a per-record consumer audit.

**Required locations.** A current listening or shelter context; the expedition site only if an existing destination or evidence-backed route can host the beat. The log’s presence in the catalog does not itself prove there is a dispatchable map location.

**Possible failure states.** Poor weather, missed equipment, or a mistaken conclusion can produce an inconclusive report and a later retry. A dangerous interpretation can close one optional route or reduce a reward, but must leave another way to finish the investigation. If no appropriate destination currently exists, use a clue-leading-to-existing-location design or defer the quest instead of inventing a map node during implementation.

**Rewards and costs.** A resolved record, route knowledge, a small relevant supply, or faction trust are candidate outcomes. Do not promise weapon upgrades or permanent underwater mechanics as a quest reward unless the corresponding system is verified. Content cost is medium-to-high because distinct acoustic sources need distinct descriptions and at least two truthful confidence states.

**Reusability.** Keep an authored observation independent from the interpretation branch. Reuse scene structure and validation, not the same explanation text or arbitrary outcome across all recordings.

### Candidate package C: The Rhyme After the Door

**Purpose.** Give shelter folklore a playable social and investigative path. A childhood rhyme is found in the existing folklore corpus; adults disagree about whether it records a real maintenance practice, a frightening story, or an imperfect memory. The resolution should make the shelter feel inhabited without treating a child’s story as a literal quest marker by default.

**Typical structure.** A record is discovered through the existing narrative/discovery path. The player may compare versions with a present-day resident, an archive entry, and a physical clue. If a cohort has matured during the campaign, a later conversation may revisit what the rhyme meant to that person. The latter is permitted only after a source audit proves an owned maturation-event route; otherwise make the callback available by ordinary campaign progression or omit it.

**Required locations.** A shelter conversation and one already-authored room, archive, or expedition destination connected by evidence. Room names and dialogue availability remain unassigned pending live content checks.

**Possible failure states.** Contradictory records can remain unresolved; a resident may stop sharing; or the player may choose to preserve the frightening version for comfort. None should erase the underlying folklore record or block unrelated cohort content.

**Rewards and costs.** Relationship, shelter-memory, or journal outcomes fit the existing tone. Production cost is moderate when it uses existing spaces and high if it requires a new room, custom character schedule, or age-transition presentation.

### Cross-package lifecycle contract

For each candidate, record entry conditions, the current canonical state owner, the next legal transition, one recoverable block, one failure-forward route, one terminal resolution, and the visible journal/map evidence. “Blocked” must explain what the player can do next. “Failed” is not a dead end when the fiction supports a changed route. Reopening requires a deliberate authored trigger and idempotent reward rules; it is not an automatic side effect of loading a save.

### Acceptance and authoring packet

Before a package advances beyond DRAFT, attach: a record-to-consumer table; a unique entity check for every location, NPC, item, flag, and quest key; a branch table with successful, inconclusive, and failure-forward outcomes; a reward-owner mapping; save/restore expectations for every persistent transition; a short voice sample; and a review of repeated-visit behavior. Acceptance requires that an active quest can still be completed or explicitly delayed under missing-destination conditions, that the player receives an intelligible next action, and that replay does not grant the terminal reward twice. No implementation or catalog addition is authorized by this plan text.

## Pass 17 — The ledger that remembers: an archival investigation quest family

This pass advances the master world bible's undertaker-registry seed into a candidate investigation package. The source audit found narrative/undertaker_burial_records.json with stable-looking burial records and a cemetery location referenced by those records. The location ID location_ash_dune_cemetery resolves in the current locations catalog. MemorialSystem already owns campaign memorial entries, has idempotent memorialization, a once-per-deceased mourning action, and capture/restore. However, an exact search of Core and host source did not find a loader or runtime consumer for undertaker_burial_records.json. Treat the corpus as authored but not yet proven playable. The gap is a content-utilization and reconciliation question, not authorization to add a second memorial ledger.

### Quest concept: The Date Left Blank

A resident asks why an old ledger gives a burial date long after the reported death. The player must determine which entries are observation, hearsay, administrative shorthand, or a deliberate memorial for someone whose body was never recovered. The investigation does not assume fraud. It may conclude that a record is internally consistent once its source is identified, that a clerk made a correctable transcription error, or that the evidence cannot resolve what happened.

A useful initial case is the existing cenotaph-style record for the widow's husband: the record distinguishes the date the death was said to occur, when the widow learned about it, and when the ceremony was held. Its blank body, empty plot, and ring placed at the grave are meaningful authored facts. The quest should not collapse those distinctions into a single “true death date” field or turn grief into a scavenger-hunt gimmick. A second record about an unmarked plot could provide a contrasting case only after duplicate content, speaker, and continuity checks.

### Candidate quest packet

**Quest family and lifecycle.** Investigation chain with a discovery start, an evidence-gathering middle, an explicit interpretation choice, and a resolution that may preserve uncertainty. “Partially completed” means the player has established a discrepancy but lacks a source. “Blocked” names an actionable next step. “Failed” routes to a limited but honest resolution, such as preserving the original text and recording an unresolved finding. “Reopened” requires a deliberate new record or campaign event and cannot replay one-time rewards.

**Entry conditions.** A known discovery path must expose the burial corpus or an equivalent already-wired journal record. The player must have a reason to consult the undertaker or archive. The plan must not assume a UI panel can query a content file that has no loader. If an existing questline or narrative-discovery owner can surface the record, prefer that route; otherwise the plan remains gated on identifying the smallest existing loader/consumer seam.

**Required locations.** The shelter archive or memorial conversation may be sufficient for a compact version. The cemetery is a candidate expedition location because it resolves in locations.json, but dispatchability and map visibility still require checking the WastelandMap graph and destination catalog. A resolved location record does not prove it is an available expedition destination. If the cemetery is shelter-only or absent from the dispatch surface, the quest can use an authored map clue to an available site or remain a shelter investigation.

**Evidence sequence.** First, compare the ledger's event dates and ceremony type. Second, obtain an independent account from a person who could know the event, with a clear channel and day on which the information reached them. Third, inspect the physical memorial only if the destination route is valid. Fourth, ask the player to classify the result as corrected, corroborated, contested, or unresolved. The journal should preserve both the archival statement and the later interpretation, rather than overwrite the source.

**Branching potential.** The player may correct a transcription, append a source note, preserve two conflicting accounts, or decline to make a finding. A correction is permitted only when a verified source supports it. A public accusation, resource penalty, or faction consequence is outside the first slice. A private acknowledgement from the undertaker or grieving resident is enough to make the choice legible.

**Failure and recovery.** If a character is unavailable, use a surviving document, witness, or later conversation. If an expedition target cannot spawn, delay the physical-inspection step and provide a truthful journal reminder. If the registry corpus remains unreachable in the live build, do not start the quest from its raw JSON presence. If evidence is lost, allow the player to return to its source or complete as unresolved. No path may erase a canonical memorial entry.

**Rewards.** Prefer a journal finding, an optional relationship acknowledgement, or one existing knowledge unlock. Do not grant a new memorial currency, funeral resource, or standing dimension. If the ending layer is to remember this action, Plan 22 must name the current consumer and accepted input; a private quest flag is insufficient.

### Lifecycle and content cost

The smallest playable slice needs one validated record source, one shelter scene, two evidence beats, three resolution choices, one repeat-visit acknowledgement, and one failure-forward path. The larger expansion adds additional record families, a second location, a contested public account, and a rumor or radio response. Estimate cost by source records that receive a complete discovery-to-memory path, not by raw JSON entry count. Each added record needs an authoring review and a consumer binding.

### Reuse and acceptance packet

The same investigation skeleton can support a missing name, an uncertain cause, a disputed plot, or an unclaimed personal effect, but every case needs its own evidence and voice. Reuse transition rules, not resolution text. Before promotion from proposal, provide: exact file and loader trace; current memorial entry identity mapping; location/destination proof; all speaker knowledge boundaries; branch-to-owner table; one old-save scenario; and an explicit unresolved outcome. Acceptance requires that the player can distinguish what the ledger says from what the quest concludes, that reloading cannot repeat a reward, and that a missing loader or location produces a visible deferment instead of a broken quest.

### Pass 17B — Case portfolio and production order

The registry investigation should be authored as a portfolio of independent cases, not a single massive quest whose branches all depend on one witness being present. The existing records suggest three distinct playable questions. They remain archival-content proposals until a loader and consumer are established.

**Case 1: A name without a body.** The cenotaph entry for the widow's husband distinguishes an empty grave from a burial of remains. The player checks the source of the reported death date, learns when the widow received the report, and decides whether the archive needs a provenance note. Its central choice is what to record about uncertainty. It can complete in the shelter without an expedition if the cemetery route is unavailable.

**Case 2: The unmarked plot.** A record describes a burial whose marker was intentionally withheld and whose undertaker objected. The player can inspect the reason for the compromise, listen to the living disagreement, and choose whether to preserve the administrative note or a personal account beside it. It should not turn into an investigation that punishes the person who requested the blank marker. A valid route can resolve with no physical marker ever added.

**Case 3: The ledger's first rule.** The first recorded burial establishes a plot numbering convention, but old records may use different naming or time conventions. The player can help the archive distinguish an ID, a name, and an origin designation. This case supports a practical reward such as a journal index entry, not a new crafting recipe.

**Production order.** Begin with the smallest single record and prove loader, presentation, journal observation, and save/revisit behavior. Add the second case only after the first record has a verified consumer and branch ownership. Add the cemetery expedition last because it introduces destination dispatch and local-encounter requirements. Do not author all cases into one data batch before confirming which catalog can host them. The smallest phase can be merged into an existing narrative questline if its schema supports the required evidence and outcome fields.

**Production-cost bands.** Low cost: a shelter-only archival conversation and a journal result. Medium: a testimony branch, repeat-visit acknowledgement, or two records with shared context. High: new destination scene, recorded testimony playback, voice/audio work, faction-level disclosure, or epilogue input. The plan must state which band each case occupies and why. No cost estimate should count a static record as playable until its source is loaded, reachable, and connected to a state owner.

**Boundary with campaign death.** These records contain historical details that may not match the current run. A case about a static record may proceed even if that named person is not in the current roster. A case about a campaign death must read the live memorial owner. Never spawn or kill a survivor to force the record and campaign into agreement. If the narrative needs the two timelines to meet, write the relationship explicitly and let the player inspect the evidence for the connection.

**Completion review.** Review each case for its main route, a respectful unresolved route, a missing-speaker route, a missing-location route, and repeated-load behavior. A case passes content design when the player can state what was established, what remains uncertain, and what the next action is. It does not need a dramatic accusation or a faction war outcome to justify its place in the game.


## Pass 18 — The Needle's Margin: calibration as a failure-forward side case

### Intent and evidence boundary

The master world bible's Part 43 lists deeper Geiger calibration as a Lane B expansion seed. This is a proposal for one optional quest packet, not evidence that a quest, skill challenge, or new calibration runtime already exists. Source review found a persistent DosimeterCalibrationSystem with registered devices, battery and sensor wear, a forty-reading overdue threshold, a one-day station reservation, completion quality, confidence and error-band calculations, cancellation, and capture/restore. The system explicitly separates observed measurement confidence from true dose and the dose ledger. The reviewed path exposes no skill-check call. The content must therefore use the existing deterministic timed procedure in its minimum viable release; a skill test is a later optional wrapper only if a current owner supports it.

### Draft quest packet: The Needle's Margin

- Type: optional investigation and maintenance side quest. It teaches the player why reliable observations matter without suggesting that calibration cleans a location or removes radiation.
- Trigger: a registered instrument becomes overdue after its established reading threshold, or the player asks the assigned instrument keeper for help. The overdue event is already emitted by the calibration system. Wiring that event into the quest owner is a premise check; do not add a second counter or silently synthesize a quest instance from panel refresh.
- Required location: the existing shelter-side calibration interface and the registered device. No map location is required. A new site, station, or character is not justified by this packet.
- Story premise: an instrument keeper has noticed that two reports disagree. The disagreement is not proof that either report is false; the worn instrument's uncertainty band has widened. The player can investigate the record, choose whether to delay a route until the device is trustworthy, and return after the one-day procedure.
- Player promise: the quest explains what changed, shows how uncertainty affects a decision, and gives a clear route forward when equipment is too damaged to calibrate.

### Lifecycle and outcomes

1. Inactive: no tracked request exists.
2. Available: an eligible registered device is overdue, and the existing quest owner can receive that fact. The panel itself must not own quest availability.
3. Discovered: the player receives a plain-language explanation that readings have become less precise. Keep the measured value and uncertainty band separately visible.
4. Accepted: the player chooses to inspect the report or proceed directly to maintenance.
5. In Progress: the device is eligible and the player starts calibration. The quest records a reference to the device and the procedure's existing state; it does not copy battery, wear, quality, or dose into quest-local fields.
6. Blocked: the station is occupied, battery is below the current minimum, or sensor condition is below its service threshold. Explain the actual blocker and show an available repair route.
7. Partially Completed: the player has secured a replacement battery or serviced the sensor, or has reconciled conflicting reports. These are milestones only when the relevant current system returns success; dialogue alone cannot grant them.
8. Failed: reserve for an authored narrative failure, such as an expedition departing before a requested verification. Equipment becoming overdue is not quest failure.
9. Completed: the procedure has completed through the owning calibration system and the player has reported back.
10. Resolved: the player has chosen a consequence-bearing interpretation, such as using the corrected confidence interval to revise a route decision.
11. Expired: only a genuinely time-limited offer may expire. The calibration procedure itself remains available.
12. Abandoned: the player declines or leaves the case. The device's actual state is unchanged.
13. Reopened: a later overdue transition may open a fresh case only if the quest owner supports repeatable instances. Otherwise it reactivates the same optional request with a new visible reason and no duplicate reward.

### Branches and failure-forward routes

- Inspect the paper trail first: a short report comparison teaches nominal value versus error band, then returns to the maintenance hub.
- Calibrate first: starts the real one-day station reservation, then returns to the report comparison after the owning clock reaches the due day.
- Battery blocked: present battery replacement as an existing system action if it is available in the campaign. Otherwise offer a non-mutating clue and leave the quest Blocked.
- Sensor blocked: direct the player to existing sensor service when available; never mark service complete from a dialogue choice.
- Cancelled procedure: return to Available or In Progress with a clear interruption note. Cancellation should not erase completed reading history or invent a damage penalty.
- Missed route deadline: if the campaign has already departed, use the lower-certainty report as a changed route clue, not as a silent soft lock. Keep the player able to finish the case later.
- False conclusion: a character may insist that the high reading proves a place is unsafe. The game should let the player challenge this with the displayed confidence band; it should not reveal that the location's true dose changed.

### Rewards and content ceiling

The core reward is informational: a legible before-and-after confidence explanation, acknowledgement from the instrument keeper, and a Chronicle entry through its current authority if the existing quest-to-Chronicle seam supports it. Optional material rewards must come from an existing inventory/economy owner and are not a reason to add a second ledger. No ending, faction access, or survivor health is altered by this small case. Any relationship or faction consequence belongs to a separate authored variant and Plan 22's verified effect routing.

### Production and acceptance packet

The writer supplies one short discovery scene, one hub scene, one blocked-device response for each real prerequisite, one start response, one elapsed-time return scene, one cancellation response, and one completion scene. Each line is tagged with speaker role, location context, device facts required, quest transition, and effect class. Keep the prose concise enough to pair with a numeric uncertainty display.

Before implementation, verify that the quest loader can reference a calibration device and receive the owning system's completion event; verify the live campaign day source; and verify where equipment-service commands are offered. Acceptance requires: no quest-local copy of calibration state, no hidden true-dose adjustment, an explicit return path from every blocked state, no mandatory skill gate, and a repeatable save/restore case that preserves an in-progress station reservation. Until those premises are confirmed and exact paths are claimed, this remains a documentation proposal.


## Pass 19 — The Unplayed Side: investigation quests that distinguish intercept from proof

### Why this case belongs here

The world bible's Part 43, seed 17 proposes using wiretap transcripts as standing-record evidence. Current source review finds a narrative catalog that can deserialize wiretap entries, but no game-side caller for that catalog; the two corpus files are marked CODEX_ONLY in the current content registry and utilization baseline. The Verdict evidence chain currently listens to player reads from MachineLogSystem, while Verdict items can be enrolled through the existing host method when their authored effect is positive. The separate Standing Record engine owns layouts, location memory, and room encounters; it is not a general evidence ledger. The quest below is therefore a playable-content and integration proposal, not a claim that transcripts already affect Verdict or standing.

### Draft quest packet: The Unplayed Side

- Type: optional investigation, with a faction-facing variant only after faction IDs and access rules are verified.
- Source document: existing wiretap_office_cartridge_allocation_quarrel. Its authored claim concerns a disputed ammunition shortfall. The quest does not restate the transcript as fact, and the transcript's clarity score is not treated as a truth score.
- Start condition: the player reaches an existing consumer that can present the transcript and chooses to open it. Catalog load alone must not create an active quest, discover a location, or enroll evidence.
- Main question: what may the community responsibly record when a recording captures an accusation but does not capture the whole chain of events?
- Required locations: none for the minimum viable case. The initial scene can occur in an existing shelter records or radio interface, but the actual screen and route must be confirmed before data is authored. Any field corroboration site is an optional quest dependency until a valid location ID is proven.
- Objective: distinguish “heard,” “transcribed,” “corroborated,” and “admitted to the Verdict record.” These are different player actions and different state facts.

### Lifecycle and state transitions

Inactive → Available only when the existing discovery or codex owner reports that this exact transcript can be presented. If no such event is available, the implementation must add a consumer seam or leave the quest unpromoted. Do not use a frame refresh or a catalog-loaded flag as a player discovery.

Available → Discovered when the player sees a clearly attributed lead. The visible card gives source ID, channel label, timestamp as written, and the author-provided clarity value. It states that an intercept can be incomplete or biased.

Discovered → Accepted when the player elects to investigate. The player may instead leave it as a known lead; refusal does not reduce faction standing.

Accepted → In Progress when the player chooses an investigation approach: preserve an unchanged copy, seek corroboration, or prepare a cautious record entry. A UI option records intent only through the quest owner. It does not write to EvidenceLedger directly.

In Progress → Blocked when the source consumer, matching location, evidence definition, or requested witness is unavailable. The status must name the blocked dependency. The player can pursue another branch or return later.

In Progress → Partially Completed after one verified activity, such as comparing the transcript's stated time with an available record. Since the current entry has no structured custody chain or corroborating-document reference, this milestone requires new authored support before it can be represented as factual corroboration.

Partially Completed → Completed when the player creates an accurately attributed case summary. Completion means the investigation packet is ready, not that an allegation is proved.

Completed → Resolved only after the player chooses whether to submit, seal, or defer the summary under the existing consequence owner. Resolved records an outcome; it does not force a verdict ending.

Failed means a separately authored deadline or a destroyed optional opportunity has passed. Losing access to one witness should route to an alternate record-based approach where possible. Expired is reserved for an explicitly timed opportunity. Abandoned preserves discovered material and completed work. Reopened is permitted only through the existing quest lifecycle contract and must not double-award an already resolved case.

### Branch outcomes

1. Submit an attributed lead: the player states that the recording is relevant but incomplete. If the current Verdict evidence contract accepts a new source type, the submission may enroll one authorized evidence ID through that contract. The result is “recorded as a lead,” never “accusation confirmed.”
2. Seek corroboration: the quest checks for an authored, source-linked corroborating item or site. If no valid site is available, the quest falls back to an existing-record comparison or remains Blocked with a reason. A new map location cannot be invented by interpreting the intercept channel as geography.
3. Seal speaker identities: preserve the substantive record while hiding or withholding personal identity. This is an authored confidentiality choice, not an automatic relationship penalty.
4. Defer: keep the file in the journal and return later. No evidence is enrolled, no faction standing changes, and no deadline is invented.
5. Withdraw an earlier interpretation: if the player discovers that a cited document was misattributed, allow a correction record. The monotonic EvidenceLedger cannot silently erase prior enrolled evidence; any correction must use an explicit existing contradiction or supersession contract.

### Failure-forward content and rewards

If the transcript cannot be loaded at runtime, the quest does not appear. If the catalog entry exists but has no player-read route, it is a content-utilization gap, not a quest failure. If a field team cannot reach an optional source, the player may submit a qualified summary or defer the case. If the player chooses an unsupported conclusion, the authored dialogue can record dissent and keep the claim attributed, without fabricating a physical fact.

Rewards are informational and relational only when verified owners support them: a journal finding, a new question in an existing Verdict conversation, or a character's acknowledgement that the player preserved uncertainty. No scrip, gear, faction standing, legal outcome, ending input, or item is awarded by prose. Any material or systemic reward must be separately authored and routed through its current owner.

### Production packet and scope

The minimum package is one discovery card, one investigation hub, four response branches, two reconvergence scenes, one blocked-state response, one correction scene, and one resolved summary. The main quest should be expansion content because it depends on a currently unreachable corpus consumer and touches Verdict-ending evidence counts. A small non-consequential Codex reading view could be core only if an existing host route and utilization gate already own it.

Acceptance requires proving the exact transcript loads in production, a player read event exists, quest state is saved by the current quest owner, every branch is recoverable, and no consequence is applied on catalog load. Before implementation, audit the current EvidenceLedger catalog and ending thresholds, claim exact integration paths, and confirm how a correction to an enrolled claim is represented. Until then, this packet remains DRAFT.


### Pass 19B — Reusable investigation packet and case variants

The Unplayed Side should be authored as a reusable case template with case-specific evidence inputs. This keeps the runtime lifecycle stable while letting later expansion content explore different stakes. These variants are proposal candidates drawn from existing transcript rows; none is a verified quest or corroborated case.

| Case seed | Question the player investigates | Primary branch | Failure-forward resolution |
|---|---|---|---|
| Office cartridge allocation quarrel | Does the intercepted dispute establish a missing stock count, a disputed ledger, or only an argument? | Compare against a separately verified requisition record if one exists. | Record the allegation as unresolved when no matching record is available. |
| Courier Guild route-collapse briefing | Does the route report still describe a current obstruction, or only an old intercepted warning? | Check a current route authority before dispatch. | Preserve the historical warning and let the expedition owner provide current reachability. |
| Rebuilders crop-failure conversation | Can a private failure report help prevent ration harm without falsely converting an intercepted estimate into current crop stock? | Seek a current agriculture or inventory report under that system's owner. | Save the transcript as a lead and let the existing food authority determine present stock. |

Each case packet contains: source ID; source type; author-provided metadata; verified player-read trigger; optional corroboration reference; admissibility class; permitted conclusion language; blocked-state copy; and a correction route. A case can omit corroboration, but then its completion describes a careful unresolved summary. Avoid making every document quest a demand to travel.

A shared lifecycle contract can support these variants: discovery opens the case; read acknowledgment records that the player saw the artifact; investigation records each validated comparison; submission asks the existing Verdict authority to consider it; admission is acknowledged only by that authority; and resolution records the player-facing summary. The quest must be able to end at “preserved, not submitted” without calling that failure.

Quest instances should store only stable references and owner-returned milestone IDs. The transcript text, clarity score, faction label, and character names remain in their authoritative catalogs. A save stores quest progress through its current owner and evidence IDs through EvidenceLedger. On restore, the UI reconstructs the view from these sources; it must not recreate enrollment by replaying dialogue.

### Content and implementation sizing

The first release should include one case, three response routes, one optional corroboration step, and one correction scene. Adding a second transcript variant costs additional authored dialogue, source mapping, continuity review, and endgame-count review. Adding all three variants in one release also requires a reachable viewer and explicit legal/admissibility copy. The scalable plan is to prove one end-to-end case first, then add each other transcript as data-driven content using the same tested lifecycle.

Reuse is high for state transitions and presentation rules, medium for dialogue nodes, and low for factual prose because each record has a distinct author and claim. The production owner must reject a template that copies an allegation from one transcript into another or applies one source's trust rating to an unrelated faction. The case matrix is accepted when every row has a verified catalog ID, one truthful completion state, one safe no-corroboration route, and a declared answer to whether it affects canonical Verdict evidence.


## Pass 20A — Verdict Radio Theater as a failure-forward quest arc (DRAFT)

### Purpose and evidence boundary

This continuation uses Part 43, seed 19 of ashfall-master-world-bible-and-expansion-authority.md: the Machine tribunal’s radio theater and procedural case stories. It is a new planning lane beside the existing burial, calibration, and wiretap investigations. The existing VerdictRadioSystem is an authored, one-shot corpus scheduler: it checks day and Reckoning phase, publishes radio.verdict.broadcast, and persists fired entry IDs. Plan 94 has already expanded verdict_radio.json to 30 Machine-Register broadcasts. This proposal does not claim that a published event means a survivor heard it, that an episode starts a quest, or that any new story row is canon. Those are separate reachability premises to prove before implementation.

The first candidate arc is The Quiet Hours, a four-part DRAFT about how a formally precise public hearing can erase unrecorded care. A tribunal’s staged case treats quiet hours as unused hours. A shelter’s night watch lead recognizes the omission; a relay performer wants to correct the script without making the next broadcast a political weapon; a young listener wants the correction to be heard because it honors people who were never entered in a roster. Character names, shelter identity, chronology, and affiliations remain placeholders until checked against the bible and current catalogs. The conflict is resolved through testimony, correction, or deliberate refusal to broadcast; it does not alter the Machine’s overall verdict or require a new faction.

### Quest packet and lifecycle

Classify the arc as an investigation with dialogue, environmental discovery, and optional community service beats. The broadcast is a lead, not a mandatory trigger. Discovery can also come from a printed program, an NPC describing the performance, or a previously accepted quest clue if those sources have reachable consumers. Acceptance is explicit. The first episode asks the player to find out whether the tribunal’s scenario is fictional shorthand or a distorted account of real shelter practice. The second gathers a witness account and checks it against an available authored record. The third offers a short response choice: correct the program, preserve the witness’s anonymity, or decline to intervene. The fourth resolves the local dispute through a follow-up scene whose wording reflects the chosen response.

Map the existing lifecycle vocabulary precisely. Inactive means conditions have not opened the packet. Available means at least one valid entry route exists. Discovered records that the player has encountered a clue; it is not acceptance. Accepted creates the player’s commitment. In Progress tracks the current authored objective. Blocked is reserved for a temporary, named condition with a route to retry. Partially Completed records a completed optional branch while the main resolution remains open. Failed means the chosen route cannot be completed as intended; the alternative remains playable. Completed means the objective sequence was satisfied. Resolved records the authored ending and outcome. Expired applies only to a genuinely dated opportunity. Abandoned records a deliberate exit. Reopened is permitted only when a later authored event presents new information, not because a loader silently reset a terminal state.

A broadcast must not advance the quest twice after save/restore. Use the existing event identity plus a quest-side objective identity at the existing quest owner; do not add a second radio-fired ledger. If the game exposes only scheduler-fired state and no user-facing reception state, the first release must keep the broadcast as optional atmosphere and open the quest through a verified clue or dialogue route. The quest must remain understandable without audio and without owning a specific radio program slot.

### Failure-forward outcomes

- The witness cannot be reached: accept an alternate route through an authored note, a second witness, or a later appointment. If none is supported by existing content, delay the quest and show why.
- The record contradicts the witness: expose the contradiction as the investigation result. Do not label either account false automatically; let the player ask a follow-up question, preserve uncertainty, or close with a partial finding.
- The player reveals the witness’s identity: route the local consequence only after an explicit confirmation. Keep a private correction available, mark anonymity lost, and remove the public-anonymity ending without deleting quest progress.
- The player ignores the fourth episode window: mark the opportunity expired only if the authored schedule really closes. Otherwise leave it Available or Blocked with a visible next condition.
- A critical location is absent: Plan 18’s substitute/clue/delay contract applies. Never let a hidden location requirement strand the main path.

### Rewards, production cost, and release gate

Rewards should be primarily informational and relational: a corrected public account, a witness relationship response, a journal entry, and a changed follow-up line. Any item, morale, reputation, or standing-reckoning effect needs an identified current owner and explicit approval. No reward is granted merely for hearing a transmission. The core-game candidate is a three-scene, one-ending-neutral investigation with one short reconvergent branch; the four-part radio theater is an expansion layer because it needs additional authored episodes and callback checks. Production cost is moderate: one quest packet, four broadcast or program references, three characters with distinct voices, one reused shelter interaction site, a small line matrix, and continuity review across the episode order.

Acceptance requires: (1) one verified way to discover and start the quest without assuming scheduler event equals reception; (2) every nonterminal state has an observable next step; (3) save/load preserves the same state and does not replay a resolved stage; (4) the unavailable-witness and missing-location paths remain completable or visibly delayed; (5) all proposed rewards route through owners proven at implementation time; and (6) the storyline does not duplicate an existing Verdict broadcast, evidence chain, or already-authored quest after a catalog-level collision audit.

## Pass 20B — Episode-to-quest beat sheet and implementation handoff

| Beat | Player-facing event | Quest state effect | Alternate route | Closure evidence |
|---|---|---|---|---|
| I. The hearing calls a quiet hour empty | Player encounters a surfaced transmission, printed program, or a witness’s description | Available → Discovered only after a real player-facing clue | Another clue source can supply the premise | Player can state what is disputed without accepting the quest |
| II. The roster has no box for it | Player meets the watch lead or reviews an available authored record | Accepted → In Progress; records a lead, not a verdict | If the primary contact is unavailable, use a second authored witness or return date | Journal wording distinguishes recollection from confirmed record |
| III. The correction has a cost | Player chooses private clarification, public correction, or silence | Optional branch may become Partially Completed; main route remains open | Lost anonymity removes one ending but leaves correction possible | Confirmation prompt names audience and permanence |
| IV. A later program carries the answer | Player hears or reads the follow-up through a verified surface | Completed → Resolved after the response is observed | If follow-up delivery is missed, an authored recap at the existing hub closes it | Outcome reflects the player’s selected disclosure level |

Keep the state graph small and explicit. The final state is not inferred from a journal paragraph or a broadcast ID. The quest owner records objective completion; the radio owner records its own fired entries; presentation reports both without writing either state. If the player chooses silence, the scene still resolves: the witness’s private account is preserved if consent permits, and the public script remains unchanged. This is a valid resolution rather than a hidden failure.

The packet should declare entry routes, prerequisites, expiry policy, location anchors, required evidence, response consequences, and fallback route per objective. Every requirement must point to a current data ID after a premise audit. Until that audit, use semantic placeholders such as existing relay-capable site rather than inventing a location ID. Mark any potentially stateful field as proposed and keep it out of production saves until the owning quest contract and migration path are approved.

The first implementation slice should be the smallest demonstrable vertical path: one clue opens a quest; the player asks two questions; a single response choice is confirmed; a follow-up closes the objective; a reload between choice and callback preserves the selected route. Add the remaining episodes only after this path proves there is a truthful reception surface. Content lint must reject duplicate objective IDs, unresolved required references, a terminal state with an unhandled outgoing transition, a failure state with no recovery text, and a reward without a recipient owner.

The story’s emotional center is recognition rather than adjudication. The player is not asked to prove that one person deserves more care than another. They decide whether an omitted form of work should be spoken publicly, corrected quietly, or left in the hands of those who did it. That distinction keeps the quest compatible with uncertain records and gives the three principal voices independent motives: the watch lead protects the crew from becoming a symbol; the relay performer wants the program to remain credible; the listener wants their work named. Their disagreement should persist after the objective resolves, so a later visit can acknowledge the player’s choice without pretending that one small broadcast repaired every social cost.

Handoff contract: a narrative/data author may prepare a DRAFT packet; the foreman must identify the current quest data consumer, event-to-player reception route, save owner, and exact file claims before promoting it. The radio team validates broadcast IDs and schedule constraints. The quest integrator validates lifecycle persistence and failure recovery. Dialogue and consequence owners review response gates and effects. A compile-green catalog is insufficient if there is no reachable player route.



## Pass 20C — Quest content authoring sheet and lifecycle test cases

### Candidate packet record

Use this as the authoring worksheet for The Quiet Hours, not as an approved production schema. The first field is a stable quest-template ID selected after checking the current catalog and naming policy. Record the content owner; short and long descriptions; quest class; core/expansion label; discovery routes; explicit acceptance action; prerequisites; objective sequence; evidence source type; location roles; dialogue references; expiration policy; completion conditions; failure-forward routes; reward references; save owner; accessibility recap; localization notes; and canon-review status. Every required reference needs one current consumer. A field without a consumer is either editorial metadata or a blocker; it must not be presented as a playable feature.

The objective sequence should be minimal: establish that a staged case has been encountered; ask what the case leaves out; obtain or decline consent to repeat a witness account; choose a private correction, public correction, or silence; observe a valid follow-up; resolve. Optional details include how the roster was compiled, why the performer accepted the script, and what the younger listener wants from the audience. These deepen interpretation but do not decide whether the investigation is completable.

### Explicit lifecycle transitions

| From | To | Required event | Player feedback |
|---|---|---|---|
| Inactive | Available | At least one reviewed entry condition becomes true | A discoverable clue exists; no auto-accept |
| Available | Discovered | Player observes a clue through a real surface | Identify the source as a staged case, not a verified fact |
| Discovered | Accepted | Player confirms the investigation | State the first objective and any irreversible disclosure risk |
| Accepted | In Progress | Quest owner begins the first objective | Show where to continue, or name the missing requirement |
| In Progress | Blocked | A temporary route, character, or location condition prevents action | Name the condition and retry/fallback |
| In Progress | Partially Completed | Optional evidence/branch is complete | Preserve the main objective and clarify what remains |
| In Progress | Failed | Intended method became impossible | Offer another route or explain a recoverable delay |
| In Progress | Completed | Required objectives are satisfied | Present the pending local outcome |
| Completed | Resolved | The authored response is observed or deliberately closed | Summarize what changed and what did not |
| Available/Discovered/Accepted | Abandoned | Player uses an explicit abandon action | Confirm whether progress is retained and whether re-entry exists |
| Expired | Reopened | A named later event creates a new opportunity | Explain why the old window reopened; never clear history |

The test harness should exercise each legal edge and reject each illegal edge. For example, Discovered cannot become Resolved merely because the scheduler fired; Blocked cannot be terminal unless the packet explicitly marks a closed failure; Reopened must point to a new authored opportunity; and a duplicate event cannot create two active instances if the quest design allows only one. Do not test invented service APIs: first identify the current public quest contract.

### Production cost and branch budget

Estimate content effort by delivered authored surfaces, not by abstract word count. Core slice: one staged-case clue, one hub conversation, two short spokes, one confirmed response, one truthful recap, and one fallback for an unavailable witness. Expansion slice: four episode callbacks, three personality cards, a repeated-visit set, a private/public/silence ending matrix, optional skill observations, and translated or voiced variants. The high-volume dialogue bank should be generated only after branch IDs and character voice are stable; otherwise late structure changes multiply rewrite work.

Avoid branch explosion through convergence. A single early choice should produce one durable outcome flag or derivable quest state, then return to a common scene. The three final outcomes can have distinct closing text while sharing the same resolved quest contract. An optional archive clue may mark Partial Completion but cannot fork the main story into a parallel quest. This keeps save compatibility and authoring QA tractable.


## Pass 20D — Optional listener-response storyline: The Second Margin (DRAFT)

### Story purpose

The Quiet Hours asks whether overlooked work should be named. The optional follow-up, The Second Margin, asks whether a public correction can be understood by people who heard the original case differently. A listener says the staged exchange sounded like an accusation against every person who ever relied on a schedule. The performer fears that retracting the whole episode would erase the witness’s point; the watch lead does not want the correction to become a speech written about them. This is a second story question, not a second verdict system or evidence ledger.

Keep the listener anonymous in the authored core scene. Their identity can remain unknown; the player is not required to investigate it. The story starts from a verified follow-up surface—an in-person remark, program response, or other existing interaction the content audit confirms. Do not invent a mail system or treat a received message as a radio-reception fact. If no follow-up consumer exists, make the scene an optional conversation at a currently valid hub or defer it.

### Quest shape

Class: discovery/investigation with a social dialogue resolution. Availability: after the player has encountered the episode or the correction, plus one reachable prompt. Acceptance: explicit offer to compare what the scene said with what listeners understood. Objectives: hear the listener’s interpretation; compare it to the authored broadcast; ask the performer what was intentionally left ambiguous; choose a narrow correction, a broader explanation, or no further statement; resolve with a short response. Required locations: none beyond one valid existing conversation anchor. A second location is an optional content layer, not a completion dependency.

Failure states: the listener’s appointment is missed; a scene version is unavailable; the correction window closes; or the player abandons. Recovery routes should be a later neutral conversation, an authored recap, or an explicit resolved-as-declined result. Do not make the listener wait indefinitely while the UI presents an impossible objective. Do not label a different interpretation as a false report. The player can resolve the quest without persuading anyone.

Rewards: a clearer authored program note, one outcome-specific line from each role, and a journal recap separating the original staged case from the audience’s interpretations. There is no mandatory resource reward, reputation gain, or ending change. Reusability: high as a pattern for future broadcast stories where audiences infer different stakes, but each episode must supply its own misunderstanding and character motives. Production cost is low for a single reconvergent conversation and medium if three new surface-specific responses are voiced or localized. It belongs in an expansion because the base game can close The Quiet Hours without it.

### Data and acceptance gates

The optional packet records an explicit parent quest reference, allowed start states, objective IDs, dialogue node references, response choice IDs, resolution states, fallback route, and provenance tags for every disputed statement. It must never reopen the parent quest automatically. If the parent resolved through silence, the follow-up can ask about a later explanation but cannot pretend a public correction was made. If the parent resolved through public correction, the listener can react to the actual delivered text only when delivery is proven. If the parent resolved privately, the scene should not claim the public heard anything new.

Acceptance requires three distinct starting histories, one for each parent resolution; an unavailable listener fallback; a save/restore after choosing a response; and a repeated visit after resolution. All histories must arrive at an accurate local ending. An omitted or invalid optional packet must not stop the parent arc. The content linter checks parent references, terminal states, localization, and a hard separation between staged script text and listener interpretation.


## Pass 20E — Case portfolio, quest-type fit, and outcome coverage

### Why this portfolio exists

A radio-theater arc can accidentally become a chain of nearly identical “listen, ask, report” quests. The content portfolio below keeps the playable verbs distinct while sharing one readable lifecycle. Each row is a DRAFT content card; names and local details require canon and catalog collision review.

| Packet | Primary type | Required play | Failure-forward route | Core or expansion |
|---|---|---|---|---|
| The Quiet Hours | Investigation / discovery | Compare a staged case with a witness account | Alternate record or visibly delayed witness visit | Expansion arc; its first clue is a small core-compatible slice |
| The Second Margin | Character / dialogue | Hear an audience interpretation and decide whether to clarify | Close without consensus or return later | Optional expansion |
| The Page Left Face Down | Environmental discovery | Find a program page in a valid location and infer whether it is a draft or delivered copy | Use a neutral recap if the location is absent | Optional discovery quest |
| No Names on the Sheet | Protection / consent | Preserve a witness’s anonymity while discussing the case | If public identity was already revealed, offer a corrective response | Optional character branch |
| A Reading for the Next Shift | Resource/crafting only if a current production recipe owner supports it | Prepare a copy or schedule a reading through a proven command | Resolve in person if preparation or delivery is unavailable | Expansion only; do not invent crafting or slot costs |

The core game should not include an escort quest, timed quest, or repeatable quest merely to satisfy a checklist. The story does not naturally need those types. A timed window is appropriate only if an authored performance schedule exists and the player is told the closing day; an escort is appropriate only if the character physically travels under an existing expedition/companion owner; repeatability is inappropriate for a once-only disclosure choice. Faction quests belong only if a current faction actor has a meaningful role and a verified reputation/access contract. This restraint avoids attaching unsupported mechanics to a narrative subject.

### Shared record and distinct objectives

These packets may share an authored episode reference and character cards but cannot share one mutable “radio investigation” state if their outcomes differ. Each quest instance must be independently startable or explicitly dependent on its parent. The Page Left Face Down can reveal a clue but must not count as a player-heard broadcast. No Names on the Sheet can share the parent consent decision if that is already stored in the parent quest, but it cannot change the original decision by replaying a conversation. A Reading for the Next Shift may create a preparation intention only if the radio owner accepts such a job; otherwise it is a prose-only face-to-face closure.

The story graph should define three common outcomes—resolved with privacy, resolved with public correction, and resolved without further action—and a status-aware recap for each optional packet. A branch can finish as Partially Completed when the player found the page but did not identify its version; it can resolve after the player chooses not to pursue the uncertainty. The design must avoid rewarding the player for exposing a witness or penalizing them for refusing to speak publicly.

### Route and reliability matrix

For each packet, the author lists one primary entry, one alternate clue or retry, one terminal condition, and one observable recap. If the primary location cannot spawn, a clue can satisfy discovery but not a visit-specific objective. If an NPC is absent, reschedule or resolve as unavailable only if the packet permits it. If a timed opportunity truly expires, the outcome uses Expired and offers the next authored route. If content was never surfaced, the quest cannot advance from a day tick alone.

QA should test packet isolation: omitting any optional quest does not disable the parent; omitting the parent’s optional radio reception path does not lock the core story; failing one side quest does not reset another; duplicate clues do not create duplicate instances; abandonment remains distinguishable from failure; and Reopened requires an authored event. Production acceptance includes a narrative map, lifecycle table, data-reference audit, recovery text, and current owner evidence. The packet remains proposal until the existing quest loader, runtime, and save owner are named.


## Pass 21A — Bunker folklore into adult voice: quest arc (DRAFT)

### Premise verified against current content

Part 43, seed 3 of ashfall-master-world-bible-and-expansion-authority.md proposes that bunker children’s folklore follows cohort members into adulthood and informs beliefs and political positions. Current authored folklore already includes entries with stable IDs, tradition type, origin sector, theme, relative timestamp, tags, and prose. DailySurvivalCatalog loads the base folklore catalog and batch 2, and JournalCodex renders entries as children’s folklore rows. This is a strong content foundation and must be referenced rather than re-authored. It does not show that a particular cohort child heard a particular story, and CohortChild currently has no explicit folklore-origin field. Its moralityMemory string is documented as the story told, not a typed folklore reference; do not silently repurpose it.

The proposed arc is The Yellow Lamp Has a Shadow, centered on an adult cohort member revisiting a childhood verse that described the surface sun as a lamp. They have since encountered the surface and now disagree with the lesson the verse seems to teach. Their former teacher says the rhyme gave children a way to picture what they had never seen. A second cohort adult argues that keeping the verse unannotated now makes an old metaphor sound like an official fact. The player is asked to decide how the shelter should tell the story to the next group of children—not which adult has the correct inner belief.

### Quest packet

Class: environmental discovery plus character investigation, with optional follow-up dialogue. The quest may open from a codex story entry, a character conversation, or a location clue only if the current surface proves that route. Discovery does not imply acceptance. Acceptance explicitly asks the player to help compare the remembered verse, its authored folklore record, and the adult’s current account. The main objectives are: identify which authored story is being discussed; ask each adult how they understand it now; decide whether to preserve the verse unchanged, add a contextual note, or write a new response verse; and observe the accepted local resolution. The choice does not rewrite the original folklore entry or change a political movement automatically.

Candidate entries include the existing sun/lamp folklore and, for an optional callback, the tree-growth hope legend. Both are known data themes, but their use in this particular quest remains DRAFT pending a line-level duplication and canon review. Do not quote the existing prose in the new dialogue. The quest can refer to the tale by stable catalog ID internally and use a short natural-language title in UI.

Failure-forward routes: if the relevant story is not exposed through the codex in the player’s current build, a character can introduce the premise without pretending the player read it; if one adult is absent, the remaining account supports a partial resolution and a later return; if the expedition clue is unavailable, keep the quest In Progress or offer the authored dialogue route; if the player declines to author or approve a new text, preserve the tradition and resolve without penalty. Expired is valid only if a real lesson/session window closes. Reopened requires a later authored event and must not erase the earlier choice.

Rewards are authored and local: an outcome-specific codex annotation if the current codex owner supports annotations, a character callback, or a new authored verse in an approved content catalog. Do not award faction standing, skill XP, morale, or political influence merely because the player selected an interpretation. Core candidate: one existing tale, two adult viewpoints, three reconvergent responses, one journal recap. Expansion layer: a cohort-specific callback, a second tale, teaching scene, and movement/faction response if current owners can support it. Production cost is moderate because the original lore exists; the expensive work lies in life-stage validation, cross-reference reachability, and ensuring that interpretation does not become a deterministic trait assignment.

### Lifecycle acceptance

Keep quest status distinct from life stage. The current CohortSystem’s matured flag gates work/duty eligibility; do not treat it as proof of legal or narrative adulthood. Before promotion, identify the current canonical life-stage source and decide whether the story uses adult, young-adult, or simply “matured cohort member” wording. The quest remains available to ordinary survivors as a general story if no cohort-linked protagonist can be proven, but it must not claim childhood experience that the character’s data does not record.

Acceptance requires: a true, reachable entry route; explicit player confirmation; no automatic completion from reading a codex row; all three resolutions valid; no forced belief assignment; optional absence of a linked origin memory falls back to a general conversation; and saved quest status remains owned by the current quest authority. A short vertical slice should work without adding a new cohort field. Per-person folklore memory is a separate, decision-gated expansion phase.
### Pass 21B — Optional quest cards and lifecycle routing (DRAFT)

This section expands the authored arc into small, independently schedulable quest cards. The cards reuse the existing folklore catalog and cohort references. They do not create a second story journal, a second relationship ledger, or an alternate child/adult classifier. Their purpose is to make the central premise playable through different kinds of investigation while retaining a single resolution authority.

#### Card A: The Teacher's Margin

**Type:** discovery into investigation; core-game candidate if the journal and current quest authority can represent a catalog-linked lead. **Start:** the player sees an annotated verse during a normal shelter visit, or receives a contextual conversation from a teacher-role character. **Required location:** a currently available common room, classroom, or equivalent authored shelter scene. The scene must not demand a new room type. **Purpose:** establish that one remembered version of the lamp story differs from the public codex entry, without implying that either version is fraudulent.

**Steps:** inspect the margin; ask the teacher whether the mark is theirs; compare the phrase with the codex entry; choose whether to leave the difference unlabelled, record it as a local variant, or ask the cohort to compose a reply. **Failure states:** the teacher becomes unavailable, the relevant cohort member leaves the shelter, or the player ends the expedition before returning. Each failure pauses the conversation step and records a recoverable lead. It does not erase the event or silently close the quest. **Rewards:** a codex annotation, a small relationship-neutral journal entry, or a new dialogue prompt. No scarce resource reward is required. **Reusability:** the card structure can support other traditions, but each tradition needs bespoke evidence and voice; do not randomize culturally specific prose from generic fragments.

#### Card B: First Shadow, Second Account

**Type:** character quest with an optional investigation trip. **Start:** after the player has learned that two accounts exist. **Purpose:** let one adult explain what the story helped them do as a child, while another explains what it obscured. The second account is not a villain reveal. **Required locations:** a safe conversation location; the source sector is optional unless current map and expedition authorities can guarantee access. **Failure states:** a speaker is absent, a route is unavailable, or the player chooses not to ask. The quest can resolve as “accounts retained separately” with no missing speaker fabricated. **Rewards:** a relationship response and a small world-chronicle observation if the current owner supports it. **Expansion cost:** medium, because voice continuity and branching responses must be reviewed against canon.

#### Card C: A Place for the Old Verse

**Type:** location-based discovery. **Start:** a clue points toward the tradition's remembered origin sector. **Purpose:** show a physical context that helps explain why a particular image endured. The site must not claim that folklore is a literal historical transcript. **Required location:** none for the core quest; an optional site can be scheduled through Plan 18. **Failure states:** no eligible expedition appears or the expedition ends early. Use a delayed clue or a shelter-based resolution. **Rewards:** a contextual codex note and an optional scene variation. **Reusability:** suitable for other origin-tagged traditions only where their authored location evidence is specific.

#### Card D: The Reply Verse

**Type:** short authored follow-up, available only after the player has heard at least two accounts or has explicitly declined to choose between them. **Purpose:** allow a character to answer the old verse in their own voice. The player may listen, help transcribe, or leave the page blank. The reply is an authored personal response, not an official correction. **Failure states:** the writer leaves or the player does not return. The scene remains a missed opportunity; no content is automatically generated as if the player had witnessed it. **Rewards:** a journal entry if witnessed, otherwise a future ambient reference only when supported by a real world-state fact.

#### Lifecycle map

| State | Entry evidence | Allowed next steps | Recovery rule |
|---|---|---|---|
| Available | parent quest or authored prerequisite is true | discover, accept, or remain available | retain availability until an explicit expiry rule applies |
| Discovered | the player encountered a valid clue | accept, investigate, or postpone | preserve the clue and map knowledge |
| Accepted | player chose to pursue the lead | in progress, blocked, abandoned | reopen only through an explicit authored trigger |
| In progress | a required step has begun | complete step, pause, fail forward | retain completed step facts |
| Blocked | a required actor or destination is unavailable | clue fallback, delayed return, alternate resolution | never mark complete merely to clear the queue |
| Partially completed | at least one durable step fact exists | continue, resolve with limits, abandon | surface what remains possible |
| Failed | a route or timed opportunity ended | alternate route, archive, or close | failure does not erase known information |
| Resolved | authored outcome was acknowledged | completed or archived | do not reopen without a named replay rule |

The present game may not expose every listed label as a persisted enum. Treat the table as a design vocabulary until the quest owner confirms the actual state contract. If it supports fewer states, map carefully and document information lost by the mapping. Do not add state values to a parallel system.

#### Acceptance and production gate

Before this becomes a content implementation task, verify that a quest can reference a folklore entry by stable ID, preserve step facts through save and restore, and expose blocked or alternate-route status to the journal. If those capabilities do not exist, this plan remains a content package proposal and must identify the smallest owner extension for a future approved integration. Acceptance requires: every card has a start condition, visible objective, non-silent blocked fallback, completion predicate, and authored effect list; none makes a belief, faction, psychological, or ending change by implication; and no unavailable expedition location is treated as evidence the player visited it.
### Pass 22A — The map with a blank edge: unsuccessful return quest (DRAFT)

**World-bible subject:** Part 46, Expedition and the surface: what does a failed-but-survived expedition leave behind in world state, rumors, and standing? **Working title:** The Map With a Blank Edge. **Status:** DRAFT proposal; this is a quest-layer scenario, not a new expedition phase or a change to terminal expedition semantics.

#### Premise and source boundary

An expedition is sent to confirm a route and return with evidence of a usable crossing. The team comes home alive but without the required proof: the crossing is impassable, the landmark could not be reached, or the survey record was damaged before it could be verified. The story is about how a shelter handles an incomplete report. It is not a story about the team being lost, dead, captured, or secretly successful.

The observed source contract makes the distinction important. ExpeditionSystem Retreat moves a looting expedition to Inbound; a completed expedition later raises OnExpeditionCompleted. Its Fail method changes phase to Failed, sets outcomeText, raises OnExpeditionFailed, removes the active expedition, and raises state change. ExpeditionHostSession currently presents completion and failure as separate LastEvent summaries. A player who returned by retreat must therefore reach this quest through the completed-return path and objective evidence, not through OnExpeditionFailed. Never infer that a failed expedition survived, or that every completed expedition fulfilled its quest objective.

#### Quest card

**Type:** location-based investigation with a failure-forward continuation. **Core/expansion:** core-compatible if the existing quest owner can represent objective evidence and alternate resolution; any new expedition-to-quest event bridge is an integration dependency and requires an owner decision. **Start:** an accepted survey quest's relevant expedition completes, but the quest's required proof predicate remains false. **Required locations:** shelter debrief space; the surveyed destination is optional after the return. **Objective:** determine what can be truthfully reported and what would make a later attempt worthwhile.

**Beat 1 — The empty hands report.** The returning party reports that it could not establish a safe route. The player can ask for the last confirmed landmark, mark the answer as uncertain, or end the debrief and leave the quest open. No report option claims the crossing was inspected if the team never reached it.

**Beat 2 — Two traces.** A witness recalls a practical detail: a rope tied below the flood mark, a trail scoured by wind, or a line on a damaged survey sheet. Each detail is authored to match the selected location and expedition facts. The trace can become a lead only if it is present in the actual result or authored scene; it is never rolled into existence as proof.

**Beat 3 — Choose a next route.** The player may archive the attempt as inconclusive, commission a lower-risk follow-up when the existing travel owner permits it, or ask a location-aware contact whether another approach exists. The player can also decline further risk. The debrief route is a legitimate resolution, not a punishment for choosing not to dispatch again.

**Beat 4 — Close the ledger.** The quest resolves as verified, disproved, or inconclusive. The label describes evidence quality; it does not automatically mutate the world map, rumor network, faction standing, or route availability. A later owner may consume a verified route fact if that contract exists.

#### Alternate outcomes and recovery

1. **Verified:** the team returned with the configured proof. This is not the blank-edge path, but it closes the same quest family and prevents a false inconclusive state.
2. **Disproved:** the expedition brought back evidence that the proposed route is not currently usable. The journal records the evidence and date; the destination is not deleted.
3. **Inconclusive:** the team returned but could not verify the route. Keep the uncertainty explicit, preserve any actually witnessed observations, and allow a clue-driven follow-up.
4. **Returned without reaching destination:** mark travel return accurately. Do not create a site visit, local discovery, or on-site dialogue.
5. **Terminal expedition failure:** use the existing failure route. Do not show the survivor debrief to a character who did not return. A rescue, missing-person, or memorial follow-up would be a separate authored quest with its own evidence and owner.

#### Lifecycle and player contract

The quest should use the current quest-state authority and its actual persisted states. Proposed design labels are available, accepted, in progress, blocked, partially complete, resolved, and archived; do not add these enum values locally. The journal should state why progress is blocked, what evidence is already retained, and whether another attempt is optional or necessary. “Not verified” is a valid conclusion. Avoid an invisible mandatory rerun.

#### Rewards and production cost

Primary reward: a truthful journal and map briefing update if current projection owners can show one. Optional rewards include a small relationship-neutral recognition line and reduced uncertainty in an already-owned briefing. No loot, skill point, standing increase, or route unlock is required. Cost is medium: several result-conditioned lines, a location-specific evidence table, a fallback debrief, and integration review across quest and expedition owners. Reuse the lifecycle shape, not the factual traces or dialogue, for later surveys.

#### Acceptance criteria

The scenario distinguishes retreat-return from terminal failure; never claims a destination visit without the visit fact; preserves known evidence when the player declines the follow-up; does not force a second expedition; and has an explicit owner for every visible map, journal, rumor, or standing output. Until the existing quest contract and host bridge are inspected for these exact facts, this remains an implementation proposal rather than a claim about available fields.
### Pass 22B — Quest variants, dependency slots, and content production (DRAFT)

The base survey story can support three authored variants without duplicating its lifecycle. Keep its evidence grammar stable and rotate the human pressure: **weather window**, where the safe return matters more than the objective; **equipment disagreement**, where the party's readings conflict; **community demand**, where a shelter wants a route answer before the next ration shipment. These are scenario skins with specific evidence and dialogue, not random quest generators.

#### Variant matrix

| Variant | Evidence question | Failure-forward route | Additional production cost |
|---|---|---|---|
| Weather window | Was the route observed under conditions that make the reading usable? | record the weather limitation and wait for a suitable window | forecast-aware copy and matching condition display |
| Equipment disagreement | Which instrument or observation is independently corroborated? | keep both measurements and request a second method | instrument-specific lore and conflict-neutral dialogue |
| Community demand | What can be said before a caravan or work crew relies on the report? | deliver a cautious interim bulletin and preserve the incomplete objective | audience-specific consequence review |

For each variant, authored content must state whether the expedition can complete the objective, what evidence is required, and which fallback is available if its special condition cannot be represented by current runtime facts. If weather or instrument provenance is absent from the relevant event payload, retain a generic inconclusive result rather than adding an invented condition.

#### Dependency slots and scheduling

The quest may expose one debrief and one follow-up slot. A second expedition is never auto-started. The player must choose it through the existing expedition command path, and the normal party/resource/route checks remain authoritative. If the quest queue is full or another conversation blocks the actor, the completed return is still saved; the debrief becomes available on the next valid interaction. Scheduling should avoid daily repeated alerts for an unchanged blocked condition.

#### Content acceptance card

Each variant package includes: stable quest and step IDs; destination reference; result predicate; minimum evidence; one success route; one inconclusive route; one unavailable-location fallback; debrief speaker IDs; response effect declaration; journal summary; and line-localization keys. Reviewers must be able to answer “what happened?” from saved facts without reading the original prose. Reject any line that describes a result absent from that fact set.

#### Reusability rule

Reuse the outer structure for route surveys, water-source checks, radio mast inspections, and salvage reconnaissance only where a returning report can truthfully resolve the parent objective. Do not reuse the blank-edge dialogue for unrelated combat retreats, terminal loss, rescue operations, diplomacy, or animal encounters. Those have different witnesses, evidence standards, and responsible owners.
### Pass 23A — The Third Bell: ambient rumor investigation quest (DRAFT)

**World-bible source:** Part 46, Information and knowledge: “What rumors circulate with no underlying event (kernel-less noise — allowed for noise-dominant stations only)?” **Working title:** The Third Bell. **Premise status:** DRAFT. Current sources do not identify any hub as noise-dominant, so this cannot be assigned to an existing location or rumor stream without a content-authority decision.

#### Story premise

A listener at an as-yet-unapproved signal room reports a third bell in a sequence that usually has two. The sound arrives on some nights and not others. People use it to tell stories: a gate is opening, a convoy is overdue, someone is tapping from below. The quest does not make those claims true. The player investigates the rumor as a social and listening problem, then decides whether to record it as ambient chatter, keep the source uncertain, or decline to spread it.

The player can learn that machinery, weather, fatigue, and expectation can produce overlapping impressions. The quest is not a forensic proof that every listener was mistaken. Its strongest supported conclusion is bounded: no corroborating event or source was found in the places and time window actually checked. The story preserves room for later evidence without retroactively converting the rumor into a hidden canonical event.

#### Quest packet

**Type:** discovery, investigation, character dialogue. **Core/expansion:** expansion candidate because a noise-dominant hub classification and source-free rumor policy are not present in the reviewed data model. **Start:** a rumor record or authored one-shot cue is made available by an explicitly configured ambient-only hub. **Required locations:** the configured listening hub and one shelter debrief scene; any second location is optional. **Required evidence:** a heard rumor, an attempted source check, and a player resolution. **Reward:** a codex note or journal summary that explains the rumor’s status; no loot, standing, unlock, or route change is required.

**Beat 1 — The statement.** A listener says, “Three bells, then nothing.” The first objective is to ask when and where they heard it, without declaring the source. If the clue is only an authored ambient vignette and no rumor record exists, the journal must not imply the rumor entered the propagated network.

**Beat 2 — The listening interval.** The player reviews the interval with a host or operator. The interface states the actual search duration and the checks performed. A missing recording is “no recording found,” not proof of silence. If the player leaves early, the conclusion remains pending.

**Beat 3 — Compare accounts.** A second listener describes two notes; a third declines to identify a sound at all. The player can preserve each as attributed testimony, write a cautious common summary, or stop collecting accounts. Do not collapse accounts into a numerical vote that manufactures truth.

**Beat 4 — File or leave.** Outcomes are “ambient report, source unknown,” “no corroboration in this search,” “lead for later,” and “not recorded.” Only the first two complete the investigation, and neither changes campaign facts about a gate, convoy, faction, or location. The last two preserve the player's right not to amplify a claim.

#### Lifecycle and failure-forward routes

If the hub lacks ambient-only configuration, the quest is unavailable and the game continues normally. If the hub exists but the rumor expires before the player accepts, retain an archive clue only if the source owner preserves the text; otherwise expire the offer transparently. If the player cannot reach the hub, offer a shelter conversation that states the account is secondhand. If no recording system is available, the investigation can still resolve as “source not established” but must not promise audio analysis. If the player abandons the quest, already witnessed dialogue remains witnessed; no public report is created.

#### Quest fit and exclusion

This is a discovery/investigation/character quest. It is not a timed crisis, escort, survival supply task, crafting requirement, faction mission, or repeatable daily rumor grind. Do not add arbitrary failure for not investigating. Do not gate the main campaign or a critical destination on proving a negative. The content is reusable only as a structure for attributed, low-stakes uncertainty in a designated ambient-only hub; the sound motif, witnesses, and conclusion are authored per location.

#### Production and acceptance

Medium production cost: hub-specific sound description, three distinct listener voices, a bounded search procedure, localization review, and a journal summary. Acceptance requires that every line distinguishes a direct hearing, a remembered account, and a source-checked result; an absent event is never fabricated; and the quest cannot begin unless the hub configuration is explicit. Implementation remains gated on the rumor owner’s schema, save, UI, and consumer contracts.
### Pass 23B — Quest states, optionality, and staged production (DRAFT)

The Third Bell should be a small case that teaches the player how this world handles uncertainty. Its success condition is not “solve the mystery.” It is that the player can identify which statements are heard, which checks were performed, and which conclusions remain unavailable. This makes it suitable for a single case, not a daily quest loop.

#### Quest status projection

Use the existing quest state machine and add no state enum unless an approved premise audit proves the owner cannot represent the flow. Player-facing labels can be expressed as: **Available** when an ambient-enabled rumor is surfaced; **Discovered** after the player hears its attributed description; **Accepted** after they choose to investigate; **In Progress** after at least one check is requested; **Blocked** when the hub or evidence path is unavailable; **Partially Complete** after one check but before the player selects an outcome; **Resolved** after a bounded conclusion; **Abandoned** when the player explicitly leaves it. Expired rumors do not necessarily expire the quest: historical testimony may still be investigated if a preserved clue exists.

The game should never convert an idle state into a timed failure. It should not reopen after resolution unless a new authored report arrives through the canonical rumor event. If no delivery/arrival event is available, the story remains closed after its first resolution.

#### Side-quest and faction boundaries

The default case is a local information quest. A faction-specific variation is a later expansion: it requires a real faction source, an explicit audience, and a standing/access owner. The ambient report alone cannot add or remove standing. An NPC may express disagreement in authored prose, but the dialogue must not imply a mechanical faction penalty. A character quest can reuse the listening scene only when a current character record supplies the speaker and the player's relationship gate; do not invent a named companion to fill a high-volume content slot.

#### Production phases and cost

**Phase A: content-only prototype.** Write the three listener voices, bounded search prompts, and four outcomes. The prototype may be displayed as a one-shot authored scene for review and does not load into the live rumor catalog. Cost: low. **Phase B: owner-contract review.** Determine hub policy, rumor provenance shape, briefing behavior, persistence/migration, and UI presentation. Cost: medium and architecture dependent. **Phase C: playable pilot.** Wire one approved ambient-only hub, one rumor entry, and the quest/debrief path. Cost: medium-high because content and owner integration must agree. **Phase D: content family.** Add further hub-specific entries only after utilization, accessibility, and continuity review. Cost: high if each site has distinct voices and acoustic context.

#### Done when

The case can resolve without declaring the rumor true or false beyond evidence; a player can decline to investigate or forward it without penalty; no event, threat, opportunity, discovery, or faction change is fabricated; unavailable provenance blocks the feature cleanly; and every resolved line can be reconstructed from canonical facts after load. Until these are met, the quest remains DRAFT.
### Pass 23C — Ambient case bank and variation rules (DRAFT)

These case seeds are a high-volume authoring backlog for a future approved hub. None is a live rumor row, and none asserts an underlying event. For each case, the author supplies the source-free policy, witness, bounded check, uncertainty language, and closeout.

| Seed | Opening observation | Bounded check | Safe resolution |
|---|---|---|---|
| The Third Bell | listener recalls a third tone after two | compare a specific listening interval to the room log | source unknown in checked interval |
| The Late Knock | a pattern sounds after the night pump stops | inspect only the available maintenance mark | no matching note located |
| The Blue Window | a receiver indicator seems brighter once | ask whether anyone else saw the same instrument | single account retained, no signal claim |
| The Borrowed Call | a call sign is repeated without a sender | check whether a sender ID was logged | no sender ID recorded |
| The Southward Hum | low-frequency sound seems to travel with wind | record wind direction only if current weather history supports it | sound account remains unlocated |
| The Empty Minute | one listener remembers a pause in the broadcast | inspect the actual saved transmission if available | playback absent, memory attributed |

#### Variation rules

Change one dimension at a time: witness count, source-check availability, rumor age, player consent, or hub audience. Preserve the same core epistemic outcome unless authored evidence differs. Do not vary a source-free account into an actual faction threat. Do not generate a new faction call sign or location from a random table. A variant may end with a line of prose and no reward; reward equality is not required for every narrative choice.

#### Reuse and repetition limits

Each seed is suitable for one authored vignette and at most one follow-up response. Do not turn all six into daily quests. A future seasonal return can reuse the hub only when a new authored stimulus or real source event exists. The ambient topic can remain unresolved across the campaign without producing an endlessly growing quest backlog.

## Pass 24A — Field-guide discovery as an optional quest thread (DRAFT)

### Source premise and boundary

The world-bible Part 46 prompt asks which field-guide entries never trigger. Current source landmarks narrow that question to a content-to-consumer audit, not a proposal for a second bestiary: `FieldGuideCatalog` owns the catalog and unlocked state; `Main.EcologicalInfestations.cs` owns observation unlock and journal feedback; `WildlifeSeasonalCalendar.FieldGuideEntryFor` maps six observed species to the existing “reading the land” entries; `FieldGuideSaveStore` owns persistence. `TravelEncounterChoice` and its resolver carry `UnlocksFieldGuideId`, while the reviewed `ExpeditionEncounterBridge` patrol branch maps the travel result into a narrative result containing morale and guilt only. That branch therefore warrants a premise recheck for the omitted unlock. The direct combat-resolution host method also resolves through an overload and discards the returned field-guide ID. This is evidence for a bounded integration candidate, not proof that every choice or all 32 entries are unreachable.

Do not create a quest registry, discovery ledger, field-guide save section, or a new universal event bus. Plan20's 32-entry catalog, Plan20A's save ownership, and Plan28's six ecology observation mappings remain authoritative. Before implementation, inventory each authored `unlock_trigger`, all choice-level `unlocks_field_guide_id` references, and every resolution entry point; distinguish an intentionally unavailable entry from an unresolved integration seam.

### Quest packet: “A Mark Worth Keeping”

**Purpose:** teach the player that a field-guide entry records an observation with a source, not a guarantee that every rumor is true. This is a small optional thread that can begin when a travel encounter explicitly grants a valid entry or when the player completes a supported ecological observation. It must not gate campaign survival, location access, or the main story.

**Typical structure:** environmental cue at an expedition start; optional inspection; a short hub conversation with a knowledgeable survivor; return to the Codex; one follow-up choice about sharing, keeping private, or marking the observation uncertain. Each route reconverges on the guide entry being readable, with distinct journal wording and optional relationship/faction consequences only if an existing owner supports them.

**Required locations:** an already eligible expedition location that contains the triggering authored encounter, plus the existing shelter/codex surface for follow-up. Do not add a map node solely for this quest. A clue or a later expedition may substitute when the original site is absent.

**Possible failure states:** encounter choice unavailable, entry ID invalid, player leaves before examining the clue, or no eligible encounter in the current expedition. Leaving should postpone rather than silently fail. Invalid content must report an authoring diagnostic and never unlock a different entry by fallback. The thread can resolve as “not enough evidence” and remain available later.

**Rewards:** a useful Codex entry, a journal record with source and confidence, and optional modest relationship feedback. No resource grant, permanent faction change, or hidden skill bonus is required. Rewards must not imply that a descriptive entry changed the underlying species simulation.

**Branching/reuse:** reuse the packet pattern for existing entries only after source-event validation. Each authored instance supplies an entry ID, trigger event, location predicate, speaker voice, and one factual observation. Keep common state transitions reusable while writing distinct copy per entry. The six ecology observations retain their current observation route; this proposal does not restage them as quests.

**Production cost/core-vs-expansion:** core game if the existing travel choice and catalog data are enough to demonstrate the bridge; expansion only if the content requires new locations, characters, or a larger investigation chain. Estimate from validated IDs, dialogue nodes, localization, UI feedback, and focused tests, not from raw prose count.

### Lifecycle and acceptance contract

Use the existing quest lifecycle states only where a quest object already owns them: inactive → discovered → accepted → in progress → completed/resolved, with blocked/postponed where current quest semantics permit. Do not introduce a parallel “guide quest status” in `FieldGuideState`. Every state transition should be caused by a concrete observation, validated player action, or supported owner event. Completing the thread requires both a valid entry ID and an observable guide update. Replaying the same resolution must be idempotent, and declining the optional conversation must preserve the unlocked entry.

Acceptance evidence for a future implementation package: (1) a catalog audit table maps all authored trigger strings to actual producer events; (2) a travel choice with a valid unlock ID reaches the existing field-guide owner exactly once through each supported host path; (3) unknown IDs and empty IDs do not grant anything; (4) save/reload preserves the unlocked entry through the existing field-guide save owner; (5) journal and Codex feedback agree; and (6) no campaign-critical quest becomes impossible when an optional encounter is absent. The package remains DRAFT until queue authority, exact path claims, and focused verification are established.

## Pass 24B — Quest acceptance cases and failure-forward review (DRAFT)

Use three authoring cases before expanding this into a multi-entry quest suite. **Case A, direct observation:** the player sees a currently supported species event; the six existing ecology mappings are exercised through their present owner, without a duplicate scripted reward. **Case B, travel encounter:** a choice grants a field-guide entry through `TravelEncounterSystem`; acceptance requires the bridge to preserve the returned ID and the host to call the existing unlock owner. **Case C, unsupported trigger:** the data contains a recognized entry but its `unlock_trigger` has no verified event producer; the quest must not manufacture an unlock. It either remains discoverable later or resolves with an “unverified report” note, subject to narrative review.

A branch can alter only the claim’s provenance and tone: “I saw it,” “someone reported it,” or “the mark is uncertain.” Do not make player response text silently rewrite ecology facts. A failed or skipped examination can continue through a second source if an authored fallback exists; if none exists, it should remain dormant and explain nothing until valid evidence becomes available. Never use a generic “any encounter” fallback because that would make the field guide inaccurate.

The map may show a destination only when the expedition selector has already selected the underlying location. Quest discovery cannot fabricate a travel location or force a deterministic spawn. If the authored encounter is not selected, preserve quest possibility through a later valid selection, a clue at an existing destination, or a postponed objective. Distinguish “quest is active” from “the triggering content is available this expedition.”

Production checklist for content authors: stable entry ID; canonical trigger token; producer/event owner; expected location tags; optional quest IDs; exact journal key; one factual sentence; uncertainty label; source-type label; repeat-resolution behavior; and proof that the matching Codex row exists. A missing speaker or location string must not be inferred from free text. This case set intentionally avoids adding new species, new ecology cadence, or a second Codex index. It is an integration and content-truth tranche, not an approval to implement.

## Pass 25A — Storm-window sortie arc: “The Measure Between Gusts” (DRAFT)

### Thesis and canon fit

The World Bible Part 46 prompt asks what happens when an expedition is caught by a storm window mid-route. ASHFALL already has a dispatch-time weather projection, a day-ranged Year of Ash storm catalog, weather gates with force costs, and seeded active-sortie ticks. This story extension should make the forecast legible as a decision with imperfect information. It must not imply that current code already reevaluates every active sortie against a new front. First verify that lifecycle seam; until then this is a content-and-architecture proposal, not a route interrupt that can be wired by dialogue alone.

**Central experience:** the player sends a small team with a weather estimate. The forecast changes after they have committed. Their next report is partial: a broken chalk mark, an unreturned tool, or a time-stamped radio call that ends before the answer. The player must decide whether to preserve the original objective, authorize a delay/hold if supported, or accept a costly forced crossing at a known weather gate. The story remains material: wet canvas, a clogged filter, a map folded to hide a tear. There is no miraculous rescue and no mandatory combat.

### Main quest packet

**Working title:** The Measure Between Gusts. **Type:** survival + investigation, with optional character/faction follow-ups. **Start:** a forecast indicates a storm window overlaps the projected sortie duration, or the player has already received a verified active-front warning. Do not launch a quest merely because a world event catalog row was loaded. **Stages:** (1) compare dispatch estimate to forecast interval; (2) pack or refuse a physical weather countermeasure already supported by inventory; (3) commit to the expedition; (4) receive a mid-route condition only if a real host event exists; (5) decide whether to hold, return, proceed, or use an authored gate-force option; (6) reconcile the result on return with the team’s actual state and objective evidence.

**Required locations:** an existing dispatch surface; the already-authored target and route/gate represented by current data; optional shelter debrief. A new storm shelter node is not required. If the selected route has no weather gate or the current source cannot represent a hold, dialogue should only describe uncertainty and the next supported action.

**Failure states:** forecast absent or stale; the storm begins after dispatch with no in-route decision hook; no route gate exists; radio contact is unavailable; the team returns without objective proof; forced passage exhausts stamina or applies the existing acute dose; a vehicle has already broken down; or the sortie reaches a terminal state before the player can answer. These are distinct outcomes. A missing host hook is a development blocker, not an in-world failure. A quest can postpone its response beat until debrief rather than silently mark the player negligent.

**Rewards:** retain the recovered item/evidence under its owning expedition result; unlock a truthful weather note only if an existing record owner can store it; optionally improve a survivor relationship through an already-authorized relationship event. Do not grant a generic storm-resistance buff. A prepared team might preserve time, condition, or information only where an extant mechanic supports that cost/reward.

### Branches and reuse

The meaningful branch concerns *what the team protects*: people, cargo, schedule, or evidence. It should not collapse into a morality score. A team that returns early may save a survivor but lose the target opportunity; proceeding may secure the evidence but wear gear or create a delayed return. A forced gate remains a visible, consentful dispatch/route decision with its data-defined cost; it is not a free escape. Reuse this structure for black blizzard, ash fallout, and ice fog only after each event type’s active-route producer is verified. Do not reuse a single consequence table if event severity or eligible route differs.

**Core vs expansion:** the forecast/dispatch decision is core quality work if it closes a live readability gap. Additional characters, a new route, and the three-stage faction aftermath belong to an expansion. The arc remains DRAFT until current host behavior proves when a storm transition can reach an active expedition.

## Pass 25B — Failure-forward beats, arcs, and acceptance contract (DRAFT)

A return without proof is not equivalent to a failed expedition. The survivor can return with a credible description, a partial tool, an exposure problem, or evidence that the route itself changed. The main quest should have a post-return continuation for each result and never assume that an interrupted radio line means death. If a sortie ends in a terminal failure state, preserve the existing owner’s meaning and let the quest resolve through a documented loss or uncertainty path. No dialogue branch may resurrect or reclassify the sortie.

**Character hooks:** a route surveyor marks time by the condensation line inside a mask; a radio operator refuses to say “clear” until a second station answers; a quartermaster keeps returning damaged filters in a separate tin because the serial labels matter. Each is a writing role until an existing survivor/NPC can own the state. Do not create three new persistent character subsystems. A delayed callback can occur when a later expedition finds the same shelter door repaired from the inside, but it must be authored against a real location and availability contract.

**Quest-only location option:** none in the minimum viable version. The team can encounter an existing route feature, weather gate, or selected destination. A quest-only shelter should be considered only if a validated location catalog already supports an alternate encounter within a parent location. Its map identity, visibility, returnability, and fog rules must be explicit. It must never appear retroactively just to satisfy the quest.

**Failure-forward map:** if the player cannot take an in-route action, the objective becomes “reconstruct the route from what returned,” not “choose the missing action.” That branch can use an existing journal/debrief surface and later expedition access. If evidence is lost, the story continues as an incomplete survey; no silent re-roll, automatic clue spawn, or unearned faction trust. If the target location is blocked for the rest of the current window, postpone it with an explicit date/condition only if the calendar owner can support that forecast.

Future integration acceptance should answer: Which exact event changes while the sortie is active? Who owns its time basis? How does the active expedition learn about it? Can the player intervene before the next deterministic tick? Which active fields are sampled at dispatch versus recalculated? What does save/restore do between warning and choice? How is a duplicate warning suppressed? Which result owner records the final route? Content is not accepted until every branch resolves to an observable state and a retry or no-retry rule.

## Pass 25C — Side-story portfolio and timeline callbacks (DRAFT)

The main storm arc should support three smaller stories that can stand alone and later converge in a debrief. These are content packets, not three new systems.

**Side quest: “A Filter for the Return Call.”** A maintenance survivor notices that the expedition team signed out a filter model that is present in inventory but listed as worn in an existing gear-condition owner. The player can replace it if the inventory transaction is possible, dispatch without the replacement, or postpone the sortie. The quest succeeds by documenting the actual equipment state; it does not guarantee safety. Failure states include the item being unavailable, the team already dispatched, or the UI report being stale. A later callback can show a clean filter returned unused or a damaged one returned without assigning blame. Production cost: one equipment-state predicate, one short choice scene, no new map destination. Core candidate because it reinforces a real dispatch decision.

**Side quest: “The Last Board.”** A route marker was repainted after a storm, and two sources disagree about when. The player can compare the dispatch note, a survivor’s account, and an existing location record. It can complete as verified, plausible, or unresolved. The clue is not a universal storm forecast and cannot unlock hidden geography without the map owner’s normal discovery route. It is suitable for an expansion if a returning location visit is needed.

**Character quest: “No One Calls It Clear.”** An operator has a practical rule: do not mark a line clear until a second station answers. The player may preserve the rule, suspend it to send one urgent message, or ask for a different operator. The consequence belongs to current radio/relationship owners only if each has a genuine supported command. Otherwise this remains dialogue and a journal callback, with no invented reliability stat.

### Long-run callback calendar

- **Before first storm window:** introduce the operator’s handwritten “last clear” time on an ordinary day. It is texture, not a quest marker.
- **First overlap with a sortie estimate:** let the player inspect the forecast and decide whether to dispatch, if the existing interface supports the decision.
- **After a changed condition:** use a report only when an actual owner event confirms it; otherwise defer all outcome-specific content until return.
- **One or more expeditions later:** revisit the route marker or hear a different survivor use the same time notation. The callback must depend on a stored quest/event fact, not a guessed day count.
- **Late game:** the player may compare several historical forecasts against outcomes to decide whether the network was mistaken, delayed, or withholding data. Each explanation remains evidence-based and incomplete; no single character becomes an omniscient narrator.

### Reuse, expansion, and exclusion tests

The grounded version uses one existing location, one existing weather gate, and two scenes. The systemic version connects equipment condition, forecast visibility, expedition phase, and debrief state. The wildcard remains physical and plausible: multiple groups share a chalk notation, but each means a different time reference; a faded correction exposes the mismatch. Exclude any branch that depends on magical prediction, guaranteed rescue, a global trust number, instant radio contact, or automatic casualty selection. Core scope ends when the player can understand current estimate/gate feedback; persistent character arcs and multiple storm-season callbacks are expansion content.

## Pass 25D — Integration readiness and scope close (DRAFT)

### Production plan and stop conditions

The narrative package can be reviewed independently from implementation. Phase 1 is a source census of weather-window, active-front, gate, expedition tick, save, journal, and forecast consumers. Phase 2 chooses the minimum truthful player-facing seam: clearer dispatch forecast, post-return debrief, or an actual dynamic event bridge. Phase 3 writes one playable slice with one storm type and one supported route; only after that slice is reachable should the portfolio expand to more seasons, factions, or locations. Phase 4 records unimplemented branches as explicitly deferred instead of writing them as if live.

Stop if the proposed response requires a parallel expedition state, a second weather clock, a new faction-stats owner, unapproved save fields, or unsourced route/location IDs. Stop if one path changes an active expedition while another route can bypass the consequence. Stop if tests would require broad simulations before the minimum rule is agreed. The current plan itself does not claim that any storm transition reaches an active sortie.

The release-quality story is modest by design: the player can distinguish a forecast from a result, understand a gate’s cost before choosing, see what actually returned, and continue after incomplete evidence. Stronger content—multi-character distrust arc, cross-faction blame, storm-window convoy, new quest-only route—belongs to a later approved expansion after the live seam is proven. This keeps the feature playable without making the world sound more certain than its systems are.

## Pass 26A — Completeness matrix: five-chapter “Measure Between Gusts” spine (DRAFT)

Apply the World Bible Part 47 loop-closure and time-spread matrices to the Part 25 story. The objective is to leave a trace of the decision at every stage without writing five disconnected errands. Each chapter ends with a state that later content can truthfully consume.

| Chapter / story function | Player action | State fact required | Failure-forward exit | Later return |
| --- | --- | --- | --- | --- |
| 1. Disturbance | Compare route conditions with the dispatch estimate | Forecast availability and current gate status from their owners | No forecast: delay the chain or use neutral dispatch copy | Remember what was available at the time |
| 2. Discovery | Inspect a route record or hear a report | A concrete source record; no inferred witness | Source absent: keep the lead unresolved | Revisit after a verified observation |
| 3. Interpretation | Decide whether two times refer to the same observation | Player response plus available evidence IDs | Choose “unresolved” without quest penalty | A later record can challenge either reading |
| 4. Complication | Decide what should be protected on the next eligible outing | Valid inventory/dispatch command, if any | If the command is unavailable, postpone the action | Character response follows actual choice |
| 5. Aftermath | Close, reopen, or leave the report incomplete | Expedition result and explicit quest resolution | Incomplete evidence is a terminal narrative answer, not a soft-lock | Chronicle callback at a later supported milestone |

This spine supports a main quest, a character quest, and an optional faction-facing report without sharing one ambiguous completion flag. Each instance needs its own stable quest ID and transition contract. Shared scenes may read the same forecast or return facts, but they cannot complete a sibling quest by proximity. A branch that closes the investigation should state what the player accepts as unknown; it should not unlock a perfect answer behind repeat visits.

### Loop closure by state, not checklist

The disturbance closes when the player can tell whether a forecast existed and whether it was visible. Discovery closes when a source is actually read/heard. Interpretation closes when the player chooses a claim or leaves it open. Complication closes only when a real command commits or the objective is postponed. Aftermath closes when the quest owner records resolved, completed, failed-forward, or abandoned state. If any close condition relies on a future integration seam, mark that chapter DRAFT and keep it outside a release-ready quest packet.

### Severity and reward ladder

Use four consequence levels: cosmetic wording; local expedition/report state; quest/character relationship; campaign-wide faction/endgame. The first playable slice should stay within the first two levels. Relationship and faction changes require evidence and an approved owner. Ending consequences are expansion scope and must not make an unresolved report secretly determine a major ending. Rewards scale similarly: information and a trustworthy report first; access or resources only when a real transaction owner and authored scarcity justify them. No blanket “storm skill” reward.

### Reuse and production cost

The generic reusable asset is a five-chapter lifecycle template and outcome checklist; authored scenes, speaker voices, evidence IDs, and location availability remain specific. A single storm type plus a single route is the core slice. Adding alternate storm types multiplies review across access, effect, UI language, and save state, not merely prose. Each additional type needs its own factual effect owner and failure-forward route before it is counted as content-complete.

## Pass 26B — Quest packet acceptance contract and authored examples (DRAFT)

A quest packet is a bounded unit that can be reviewed by narrative, design, engineering, and QA without requiring any one reviewer to infer hidden behavior from prose. Its header identifies the quest ID, quest type, intended campaign phase, core/expansion classification, prerequisites, owner systems, required catalog references, estimated content volume, and unresolved premise checks. The packet then provides its lifecycle graph, objective text, location availability rules, success and failure-forward outcomes, rewards, state effects, replay/re-entry rules, and acceptance examples. Every referenced ID is provisional until checked against the current catalog. A prose name is not a runtime identifier.

The first storm-investigation slice should demonstrate how the contract works at modest scope. The main quest asks the player to reconcile an estimate with the report from a returned expedition. The character quest asks why the surveyor stopped signing route cards. The optional faction quest asks whether the player will certify a delivery window. These three arcs may reuse the same record and debrief location, but they do not share completion state or imply that one faction owns the underlying weather truth. Their packets each specify which facts they may read and which effect owners, if any, can accept an action.

### Required packet fields

- **Entry:** exact event, prerequisite, or environmental observation that permits discovery; repeat-entry behavior; and behavior when the entry surface is unavailable.
- **Availability:** the expedition, campaign, character, faction, and location constraints. Use positive conditions with an explanation for hidden or delayed availability.
- **Objective:** a player-understandable verb and an observable completion signal. “Learn the truth” is not testable; “compare the two recorded times and choose a report status” is.
- **Required assets:** existing location IDs, dialogue node IDs, evidence IDs, and reward IDs. New IDs must be explicitly labeled proposed.
- **State transitions:** each legal transition, its actor, its triggering action, and whether it can be repeated, resumed, or reversed.
- **Failure-forward:** what the player can still do after a source, companion, route, or deadline is lost. Record whether the quest stays open, resolves with uncertainty, or is abandoned.
- **Reward and cost:** exact owner and transaction, bounds, and whether the response is information, access, material, relationship, faction, or ending level.
- **Acceptance examples:** at least one ordinary completion, one unavailable dependency, one repeated interaction, one abandonment or failure-forward outcome, and one save/restore boundary if state persists.

### Outcome taxonomy

`Completed` means the declared objective was fulfilled. `Resolved` means the story has an authored terminal interpretation, including an explicit unresolved conclusion. `Failed` records that a required objective can no longer be achieved; it must still provide a clear forward route where the story promises one. `Expired` is appropriate only when a real time window exists and the player has been told about it. `Abandoned` is player-initiated and must not be silently substituted for failure. `Blocked` is a temporary state with a named dependency and a recovery path. `Reopened` requires a new fact or action that justifies new work; it is not a default response to a repeated visit.

### Content acceptance sample

For “Measure Between Gusts,” the report scene accepts three answers: confirm the schedule was missed, mark the cause unknown, or refuse to certify. All three close the delivery-certification branch. Only the first uses language asserting a missed schedule; none claims that weather caused it. The investigation branch remains open if a second evidence source is a real prerequisite. If no second source can be selected for an expedition, the quest selector must either provide a clue route or delay the relevant objective with a visible explanation. It may not leave an accepted quest pointing at an impossible location.

### Review and production cost

Narrative review checks voice, ambiguity, and whether the scene respects what each speaker could know. Design review checks affordance clarity, reversibility, and reward fit. Technical review checks owner APIs, stable IDs, persistence need, and save migration scope. QA review checks transition coverage and an observable UI route. A packet is not ready merely because its dialogue reads well. The first slice stays core only if it works with existing weather, expedition, journal, and dialogue surfaces; any new save-bearing quest ledger, active-sortie mutation, or faction transaction needs its own approved integration premise. Multiple location variants, voiced scenes, and major ending consequences remain expansion candidates with explicit cost estimates.

## Pass 27 — “The Last Dry Strike”: match-manufacture assay quest architecture (DRAFT)

This pass adapts the World Bible Part 43 assay-culture seed to an under-covered production subject: match manufacture. Current data contains a scavenged matchbook and prose that references a match, while the narrative directory has several craft-assay families and no filename-level match-manufacture family in the reviewed inventory. This does not prove the game lacks every match recipe or consumer. No new consumable, recipe, workshop station, quality statistic, or trade specialty is proposed here. The content story can ship as records and choices only if the existing discovery and quest owners can expose them; any material effect needs a separate consumer audit.

### Quest thesis and story spine

“The Last Dry Strike” starts when the player finds a rejected notebook tucked inside a reused match carton. The first page records a batch as good because every test stick ignited. A later page records the same batch as failed because half the box went dead after a damp night. The disagreement is not fraud: one worker tested immediately after drying, another after ordinary storage. The player investigates how an assay becomes a promise to someone who needs a reliable flame.

The main quest has six stages: discover the paired records; locate the second test sheet; learn why the two workers used different storage intervals; compare one household’s complaint with the batch note; decide whether to publish a corrected description, preserve the original entry with an uncertainty note, or decline to certify; and receive a delayed acknowledgement from a character who uses the correction. No stage claims that match quality changes gameplay. The essential outcome is whether future readers inherit an honest record.

### State contract

The quest requires one stable instance ID, entry predicate, explicit accepted/discovered distinction, evidence IDs, response ID, outcome, and callback eligibility. Reading a catalog entry is not proof that a player saw it; use the existing discovery or chronicle fact if a supported consumer exists. Otherwise the packet remains unintegrated and does not create a parallel seen-assay save list. Completed means the player made a report decision. Resolved can represent a deliberate refusal to certify. Blocked is reserved for a named missing record with a clue/fallback route. Failed is reserved for an irrecoverably unavailable required artifact, not for declining to publish. The quest must not auto-complete when a record is loaded.

### Branch portfolio

1. **Publish the corrected interval.** The player preserves the earlier worker’s name and changes the public interpretation to “passed when dry; storage test incomplete.” A later character line recognizes accuracy, not bravery.
2. **Preserve both records.** The player refuses to flatten conflicting evidence. The archive keeps the original and adds a neutral note that the tests used different conditions.
3. **Withhold certification.** The player declines to turn a narrow test into a general safety promise. The quest resolves with an explicit unknown and no hidden punishment.
4. **Lose the second sheet.** If a route or expedition cannot surface it, the story can conclude from the conflicting surviving documents; the missing sheet is a declared limitation, never a permanently blocking objective.

### Companion and side content

The notebook’s author, Mara Venn, speaks in test intervals and dislikes the word reliable without a duration. Iven, who kept the carton in the clinic store, remembers that the order arrived after a leak but cannot identify the batch. A kitchen worker remembers the drawer being moved twice, not who moved it. Their testimonies differ in scope, not moral worth. Optional character quest “One Box for the Walk” asks whether the player will leave a small sealed carton with an outbound team. It must not consume a match item unless the canonical inventory transaction supports that item and the interaction displays the exact cost. Otherwise it is a conversation and report choice only.

### Failure-forward and acceptance cases

The quest is playable if only one record survives: the player can preserve uncertainty. If the author is absent, their signed note remains evidence but cannot unlock a relationship response. If the location is not available, the selector supplies an alternative clue or delays the visit. If a save is restored after the report decision, the callback must not award or mutate anything twice. A repeat visit can deliver concise acknowledgement; it cannot reroll the narrator or rewrite the conclusion. Acceptance includes all three report outcomes, absent source, repeat visit, refusal, and a saved/resumed instance wherever the current quest owner persists state.

The core candidate is a small environmental investigation with an authored evidence pair, one conversation, and a report decision. Expansion scale adds four to six assay records, an optional household side story, a workshop inspection scene, and a late-game archive callback. It does not add production simulation. Integration classification is DATA ONLY if the current narrative discovery and quest consumers reach the records; DATA + WIRING if an existing event route can expose them; CORE EXTENSION only if a needed quest transition has no current owner seam. Any change to item use or recipe outputs is outside this plan’s present authorization.

### Pass 27B — Side-quest portfolio and release boundaries (DRAFT)

The side-quest portfolio rotates types around one small evidence problem so every branch does not become “find another paper.” Names and locations below are proposed story labels until matched to current data IDs.

| Quest seed | Type and typical length | Required player action | Failure-forward route | Reward class | Scope / cost |
| --- | --- | --- | --- | --- | --- |
| “The Back of the Card” | Discovery, 1 scene | Inspect the reverse of the original test card | If illegible, record a missing condition and continue | Information | Core candidate; low |
| “A Box Left Open” | Investigation, 2 scenes | Reconcile a storage note with a witness memory | Close with chain-of-custody unresolved | Quest knowledge | Core if both sources exist; low |
| “The Walker’s Lamp” | Protection, 1 choice plus callback | Decide whether to delay a departing worker until an existing supply check is made | Let the person leave without a promise; the need remains a note | Character acknowledgement only unless inventory owner supplies a real handoff | Optional; medium |
| “Dry Paper, Wet Hands” | Resource/crafting, 1 planning scene | Ask the existing workshop owner whether any current recipe can produce a suitable package | If there is no such recipe, convert to information and do not imply production | Existing recipe output only, if verified | Expansion; medium and premise-gated |
| “Who Copied the Heading?” | Character, 2 scenes | Ask the junior clerk about copying “passed” without reading the second page | Accept their explanation without forced confession | Relationship only through an existing command; otherwise prose | Optional; medium |
| “For the Route Board” | Delivery/report, 1 return | Carry the corrected statement through an already supported journal or report action | If no report command exists, resolve as an unposted draft | Archive acknowledgement | Expansion; low to medium |
| “The Storekeeper’s Count” | Faction/institution, 2 scenes | Ask the clinic and kitchen to state how they define a usable carton | Keep the definitions separate if they disagree | Local trust only when a real owner supports it | Expansion; medium |
| “Keep the Old Word” | Hidden, 1 short scene | Notice that “passed” was underlined twice on the original page | Missed clue changes no required outcome | One optional line | Expansion; low |

No seed is repeatable by default. A recurring shipment audit is appropriate only if an existing system emits repeated batches and offers an observable result. The portfolio’s “repeatable” candidate is therefore explicitly deferred rather than written as a daily quest loop. A timed quest is also excluded from the core: the story has elapsed days in its record, but no player-facing deadline. If an authored edition later adds a genuine deadline, it must identify the clock owner, tell the player the limit, and preserve a non-timed completion path for accessibility and campaign recovery.

### Reuse and production-cost controls

Reusable pieces are the evidence-pair contract, report choice, missing-source fallback, and callback format. The workers, batch facts, voices, and item references are authored per story. Reuse does not mean reskinning one exact dialogue exchange across every technical trade. A second assay arc must use a materially different decision, source conflict, and consequence. The first release slice needs two source records, a discoverable path, three report labels, a journal acknowledgement, and one return line. A small team can author and validate it without new art if an existing archive surface can present records. New voiced cast, workshop animation, physical match-production interaction, and multiple seasons are not in the minimum scope.

The packet is complete only when the content reviewer can answer: which claim is supported by each record; which missing fact remains unknown; what the player can refuse; what each terminal state means; whether any reward has an actual owner; and how an unavailable clue still reaches an honest conclusion. If any answer requires a new item quantity, location route, or reputation rule, the proposal returns to premise review instead of adding a convenient flag.

## Pass 28 — Relief Without a Failure Counter: caregiving as labor and relationship

### Source premise and content boundary

The World Bible Part 46 asks what caregiving failure looks like and which relationships it strains. Current CaregivingSystem supports an authored story about pressure and handoff, but its actual facts are narrower than “failure” suggests. Core stores caregiver-to-patient assignments and patient bond strength. During a positive-hour tick it applies a recovery bonus, caregiver fatigue, affinity, bond growth, and a one-time dialogue-threshold event. Assignments can end when a caregiver or patient is no longer alive. Save capture persists assignments and bond; it does not persist hours, missed visits, neglect, consent, a caregiver’s expressed limits, or a failed-care outcome. Assignment validation checks survivor identity, life, caregiver fitness, and patient need. Reassignment automatically releases a prior pairing. The Godot host connects care starts to the Duty Roster: the caregiver’s existing role is vacated, and restore order reapplies this vacancy. This is a DRAFT authored quest extension using those boundaries; it does not assert that Core records a lapse or can diagnose a patient.

### Quest packet: “The Cup on the Rail”

**Purpose:** make the cost of sustained care legible as a labor and relationship choice, then let the player take a truthful action through the existing assignment interface. The dramatic problem is not a hidden medical emergency. A caregiver asks for relief after routine work has narrowed their life to the bedside; the patient objects to becoming a task passed between people. A third survivor has written down which shelter jobs have gone uncovered while the caregiver has been unavailable. None of those facts proves harm. The quest asks whether the shelter can acknowledge care as work while respecting both survivors as people.

**Typical structure:** a short hub-and-spoke conversation in the shelter, followed by a reconvergent resolution. It is a character quest with a small relationship/civic layer, not a new medical simulation. The player may hear the patient first, caregiver first, or roster keeper first; order changes optional lines, but the central choice remains available. Do not force an expedition: the playable pressure is allocation of one already assigned survivor.

**Required locations:** caregiving panel or equivalent shelter interaction; one authored bedside scene only if the narrative system can target a ward/shelter location; one duty-roster interaction for optional context. If there is no valid location-aware dialogue route, deliver the scene through the existing shelter hub. Do not mint a map destination or new ward instance.

**Availability:** offer only when a live caregiving pair is queryable and both IDs resolve to living survivors. The caregiver’s previous duty may be discussed only when the current roster actually identifies a prior role; never infer a specific missed shift solely from the care assignment. A no-pair version can appear as a general conversation lead, but it must not promise an assignment-changing resolution until a valid pair is selected. If the assignment or survivor state changes between presentation and response, re-read and gracefully close the branch.

**Opening prose seed:**

> The cup has gone cold on the rail. No one remembers setting it there. Mara sits with both hands around it anyway, as if warmth might arrive by agreement. Across the room, Iven watches the door instead of the blanket. “I asked for a person,” he says. “They keep sending a shift.” Mara does not look up. “I am a person. I just want one hour in which nobody needs me to prove it.”

Names are placeholders until an entity audit maps the scene to an approved roster. The scene contains no diagnosis, unlogged duration, or claim that care was withheld.

### Resolution graph and lifecycle

1. **Discovered:** enter through a spoken request, a care-start event surfaced by an existing narrative consumer, or opening a shelter interaction. Do not imply an event-to-quest bridge exists until the consumer is verified.
2. **Accepted:** the player agrees to hear all three perspectives. Acceptance records only the quest’s own state if the existing quest owner has that contract. It does not mutate care, affinity, fatigue, or roster state.
3. **In progress:** inspect assignment, survivor availability, and any assigned duty from canonical owners. Present unknown as unknown. Optional conversations do not create a silent failure when skipped.
4. **Resolution A — relieve:** if a valid replacement exists, expose the ordinary assignment command. Existing single-caregiver and single-patient reassignment behavior applies. Report success only after the host command succeeds and a post-query confirms the pair.
5. **Resolution B — unassign:** the player may end the current pairing through the existing action. This is a real local consequence: assignment ends and the existing event fires. Bond is retained by Core, so do not narrate the relationship as erased. The patient may have no caregiver afterward; state that plainly.
6. **Resolution C — defer:** acknowledge the request but take no assignment action. This is a narrative closure only if the current quest owner supports “resolved without intervention”; do not award operational success or claim conditions improved. If unsupported, retain the quest as available/in-progress.
7. **Changed circumstances:** death ends an assignment through existing Core behavior. Narrative may close as changed circumstances without fabricated causal blame. Patient recovery may make assignment validation fail; current need is authoritative, and rejection is not evidence of neglect.

**Failure states:** invalid IDs; dead or incapacitated candidate; patient no longer needs care; assignment changed before response; stale command version; no compatible replacement; or player exits. These differ from patient harm. Recover by refreshing state, offering a non-operational dialogue close, retaining an eligible unresolved quest, or pointing to the existing panel. Never create an unbounded retry.

**Rewards:** authored recognition: private lines, a roster-board note acknowledging care as labor, and journal summary if a current journal route exists. Mechanical reward is limited to the effects already caused by a successful assignment/unassignment; no bonus affinity, fatigue relief, recovery, item, faction standing, or roster mark for choosing a “kind” response. A reward proposal must identify a valid owner and balancing review.

### Reusable quest-family schema

Use this packet as a template for care-related stories: trigger evidence; pair eligibility; current canonical snapshot; speaker order; player understanding goal; available owner commands; command revalidation; narrative-only closure; state-changed fallback; consequence scope; return behavior; and optional authored callback. Reuse it for distinct situations—a novice asks to learn bedside work, a patient rejects a familiar caregiver’s manner, a caregiver requests a different duty, or a survivor returns after an assignment ended—but do not multiply near-identical errands. Every variant needs a different human question and a verifiable owner fact.

**Production estimate:** medium for one quest and three short scenes if current quest/dialogue consumers support conditional conversation and command results; high if a new event bridge or persistent resolution is required. Keep event-to-quest routing out of the first slice unless evidence identifies a supported subscriber. One concise shelter character quest fits the core game; a wider companion anthology is an expansion candidate.

**Acceptance evidence:** entity mapping for each speaker; traces of valid reassignment, rejected assignment, and unassignment through current APIs; confirmation that roster vacancy is applied/restored once; proof bond is retained on unassignment; and a quest-state contract with no new caregiving field. These are gates, not work performed by this document.
## Pass 29 — A Place Is Not a Destination: micro-location evidence and quest coverage

### Verified inventory and question framing

The World Bible Part 46 asks which micro-locations lack narrative encounters. A focused static census provides a useful baseline, but not a complete playability answer: locations.json contains 179 location records; micro_locations.json contains 28 encounter definitions. Three definitions have a nonempty requiredLocationId that exactly matches a location ID: micro_hospital_chapel_ledger → abandoned_hospital; micro_depot_undertow_raft_line → location_flooded_subway_depot; and micro_gamma_levy_board → loc_garrison_checkpoint_gamma. The other 25 have no required location. EncounterDefinition.GetEffectiveWeight applies an exact ordinal location check only when requiredLocationId is nonempty. The common NarrativeEncounterSystem loads micro_locations.json into the same encounter catalog and selects by weight, depletion, weather gate, stance, danger, and location. Therefore “25 unbound records” means globally eligible under those checks, not “25 missing encounters,” “25 universally reachable,” or “25 map destinations.” A separate route/consumer audit is needed to prove where and how often these records are offered.

### DRAFT coverage quest: “The Index Without a Map”

This short discovery/investigation quest turns the difference between a place, an encounter, and a map destination into an in-world question. The shelter archive has three surviving references: a hospital chapel visitor book, a flooded-depot raft line, and a checkpoint levy board. The references are already authored micro-location encounters. The player is asked to compare what the records actually say, not to repair a supposedly broken map. Each site contributes one observation; none alone proves who maintained a route, who imposed a levy, or who visited the hospital.

**Quest purpose:** teach that a location can host an authored encounter while the encounter may be globally eligible, exact-location-gated, or unreachable because its reference is invalid. In fiction, the archive keeper is trying to decide which reports belong under a place heading and which belong under “seen somewhere on the road.” The player’s final annotation can be “verified at named site,” “portable account,” or “not enough evidence.” These are authored journal categories only unless an existing quest/chronicle owner supports them. Do not create a second map ledger or a free-text evidence database.

**Typical structure:** environmental discovery at any of the three existing encounter sites; an optional shelter-hub conversation; a three-clue investigation with partial completion; reconvergence at the archive. The player can accept the investigation before visiting all sites and can resolve it with one or two sources as an explicitly incomplete account. No forced expedition itinerary: location access remains governed by existing expedition/map owners and normal destination availability.

**Required locations:** one existing shelter archive interaction if supported; otherwise an existing journal or hub return. The three encounter-bound sites are optional evidence sources, not guaranteed spawns for this DRAFT quest. The current micro encounter record is not itself proof that its site is reachable in a given campaign. A future implementation must either reserve a real required location through the current expedition-selection contract or provide an authored clue/equivalent evidence route.

**Quest states:** discovered from a real encounter resolution or archive conversation only after a live subscriber is verified; accepted; in progress with a count of evidence observations held by the quest owner; blocked when no remaining eligible location can appear; partially completed after one or two records; resolved with a scope-limited conclusion; reopened only after an additional canonical discovery. “Blocked” must have a visible recovery route, such as a later expedition or an alternate existing record. If the quest owner cannot represent these states or evidence observations, ship the authored content as standalone encounter/journal prose instead of shadow-saving it.

**Branching conclusion:** (A) “Keep the exact names”: preserve the three-site evidence chain and state that the other 25 general records are not site census results. (B) “Use the reports as a travel index”: useful to route future searches but lower confidence about origins. (C) “Leave the page unfinished”: a valid failure-forward resolution; no player is penalized for unavailable locations. None grants faction standing, inventory, or map unlock automatically. Any reward requires a canonical owner and a fresh design decision.

**Sample closing prose:**

> “Three names can make a route look certain. The archive keeper draws a line under the last one, then leaves the space below it blank. ‘If we fill every blank with a guess,’ she says, ‘someone will walk there expecting a door.’”

The line makes no factual claim about an absent door or faction and can be removed if no matching archive character exists.

### Reusable quest portfolio and cost

The same packet supports three smaller follow-ups: a character quest about whose name is missing from the hospital book; a faction-neutral discovery at the depot asking whether a route mark is still current; and a civic investigation of how paid/owed marks change on the checkpoint board. These are distinct questions, not three fetch errands. They reuse real encounter IDs and destination IDs, but require current entity, quest-link, and effect-consumer mapping. Do not copy the encounter text into quest records; reference it through stable content IDs when the current schema allows.

**Core versus expansion:** the coverage rules and one optional investigation belong in the core documentation architecture; the three-site quest chain is a modest expansion, since it depends on cross-site availability and authored callback wiring. A data-only release can ship location-bound micro encounters independently; a multi-step campaign quest needs a reliable resolution-to-quest event path.

**Production cost:** low for a static coverage report and corrected references; medium for one quest using existing encounter resolution and journal owners; high if a new subscription, location reservation, or persistent evidence ledger is proposed.

**Done when:** an automated or repeatable census classifies every record as exact-site, global, invalid-reference, or intentionally dormant; checks IDs against the location catalog and reachable destination set; verifies content utilization separately; and documents every uncovered destination only after the consumer path is inspected. No new quest, map, or save authority is implied by this pass.
## Pass 30 — The Map Has Two Edges: regional knowledge and playable destinations

### Current evidence and limit

Part 46 asks which map regions have low destination density. The data exposes three different structures: map_regions.json has eight region records, each listing two points_of_interest labels; wasteland_map_v1.json has 22 canonical travel nodes and 68 routes; locations.json has 179 location records. Neither the 22 travel nodes nor the 179 location rows carries a region field. CatalogIntegrityValidator explicitly treats map_regions.json/points_of_interest as identifiers local to that catalog and consumed as CartographySystem KnownPoiIds labels, not cross-catalog references. None of the 16 region POI labels matches a location ID or canonical travel node in the reviewed pair. So density by playable destination is not computable from a direct join. Prefix loc_ is not proof of a destination ID.

The live UI path also matters. Main.GetCartographyProjection and MapPanel project canonical WastelandMap nodes and persisted knowledge. Main.RecordCartographySurvey mutates the canonical WastelandMap owner and awards existing scavenging skill XP. Core also contains mutable region-survey methods and a loader for map_regions.json, but that API’s existence is not proof the Godot host uses it. Plans must use the currently wired map path or pause for an owner decision; they cannot awaken a parallel region ledger to answer the density question.

### DRAFT quest architecture: “The Map Has Two Edges”

**Player outcome:** an archive keeper asks the player to reconcile a region chart with the route map. One sheet names terrain and hazards; the other shows travel nodes and links. The sheets agree that travel is dangerous, but not where every mark belongs. The player records what is verified, leaves uncertain marks in a margin, and chooses which real route to survey next. The quest teaches that a region with no mapped destination is not necessarily empty.

**Quest type:** discovery/investigation with a small cartography action and failure-forward conclusion. It can fit the core game after a valid quest-to-map route exists; a larger regional anthology belongs in an expansion. It is not a new selector or cartographer career.

**Trigger and availability:** an archive conversation can offer the comparison without a region-to-destination link. A region-specific continuation requires a canonical map node and a current survey or route action through WastelandMap. Until that relationship is authored and validated, the eight region labels are source text, not selectable destination IDs.

**Objectives:** compare the two sheets; identify one or more verified map nodes through current map knowledge; optionally survey a route node through the existing map host if an eligible living survivor and access are available; return with the canonical result; conclude with verified, uncertain, or unvisited annotations. Quest progress must not count reading a label as discovering a location. If the existing quest owner cannot record partial evidence, ship this as a linear archive conversation or journal note rather than adding another quest-state store.

**Resolution routes:**
- A canonical destination is present and surveyed: report only what the returned map result confirms.
- A chart label has no verified destination crosswalk: preserve the uncertainty; do not reward a guessed location.
- A route is inaccessible: retain a blocked/delayed objective only if the quest owner supports that state; otherwise resolve the chart comparison as incomplete without claiming a survey.
- The player leaves the chart: allow postponement without erasing map knowledge.
- The chart and map disagree: WastelandMap wins for actual travel; keep the chart as a separate authored source until reconciled by an approved authority.

**Rewards:** authored knowledge and a truthful journal conclusion. The survey action already awards its existing scavenging XP; completion must not award it again. No region-discovery bonus, faction standing, route unlock, map marker, or item is implied.

### Regional story portfolio

The eight region records can seed distinct place-focused stories after a canonical crosswalk is approved: Ash Valley Basin pairs ash storm/radiation-pocket observations with quarry/radar labels; Dead Coast Estuary uses corrosive fog/submerged rebar around port/tide-gate labels; Ironspire Metropolis frames collapse/sniper risk around metro/hospital labels; Missile Silo Grounds uses EMP residue/toxic slurry around silo/bunker labels; Iron Ridge Escarpment uses rockslide/gale around mine/lookout labels; Verdant Impact Basin contrasts mutated flora/spore drift with commune/spring labels; Submerged Industrial Run centers methane/flash flooding around pump/oil-depot labels; Frozen Highland Gap uses black ice/hypothermia gale around crossing/repeater labels. These are writing prompts, not verified destinations, applied hazards, or faction claims. Each quest must identify which facts are flavor, which node IDs are canonical, and which hazards have a live gameplay consumer.

Vary the player question across content: who maintains a route; whose measurements are trusted; which warning should be published; what must remain unmarked; how scarcity changes passage; how a shelter remembers an old survey; what a community owes; when a dangerous shortcut should stay secret. Avoid eight copies of “visit two points.”

**Dependencies and cost:** architecture decision on region-to-node ownership; exact map/destination census; verified quest subscriber and map action; save review only if quest evidence persists; deterministic route state; entity/content mapping. Static research is low cost, one quest medium, a multi-region chain high. This is a DRAFT scaffold, not approval to modify MapRegion, map nodes, location catalogs, or quest state.
### Pass 30B — Region quest cards and failure-forward portfolio

The quest portfolio below uses only the eight region names, terrain, hazards, and POI labels already present in map_regions.json. These are archive/rumor concepts until each label is tied to a canonical node. The card’s required location is therefore “existing shelter archive” for the first slice; regional travel remains optional and disabled until the crosswalk exists.

| Region seed | Quest form and question | Possible progress evidence | Failure-forward resolution |
|---|---|---|---|
| Ash Valley Basin | Investigation: did the quarry and radar notes come from one survey? | Two distinct authored records, not a guessed route | Preserve them as separate claims |
| Dead Coast Estuary | Survival/discovery: which tide warning is current? | A canonical survey plus dated testimony | Mark chart stale; return without crossing |
| Ironspire Metropolis | Character/escort: who should receive a hospital warning? | Verified route and consent to share | Keep warning private or delayed |
| Missile Silo Grounds | Timed/critical: can the warning arrive before access closes? | Owner-verified day window and route eligibility | Defer without inventing a deadline |
| Iron Ridge Escarpment | Expedition: is the lookout a real reachable node? | Crosswalk match and reachable route | Convert to map-room research |
| Verdant Impact Basin | Resource/survival: which spring account can be trusted? | Existing water source fact and source provenance | Carry two conflicting accounts |
| Submerged Industrial Run | Investigation: who wrote “listen before opening”? | Authored document or encounter result | Leave author unknown |
| Frozen Highland Gap | Faction-neutral discovery: does the repeater still answer? | Existing radio signal or verified discovery route | Record no confirmed reply |

No card asserts that the named hazard is simulated, a faction owns a site, or an NPC is already present. Any timer, water test, escort, reward, or radio result requires a current owner. In particular, the “timed” silo concept is unavailable until an actual time-window owner and recovery path are identified. This preserves the quest taxonomy without making content impossible through invented gates.

**Quest reuse rules:** all cards share a small lifecycle skeleton—offer, evidence gathered, route unavailable, partial conclusion, final interpretation, optional revisit—but each must differ in player question and information risk. Location availability is not a reward. A region chart label cannot satisfy a destination objective. If the quest owner lacks partial completion, the first shipping slice is one linear archival quest with a final uncertainty choice. Mechanical rewards remain out until an owner-backed balance case exists.

**Production sequence:** source/ID census; architecture decision; one canonical node-region mapping slice; one Ash Valley or Dead Coast quest prototype; resolve one current-map survey; then expand the quest family only if the prototype’s event and save path are stable. The four remaining region cards are content backlog, not a promise to create eight quests at once.

## Pass 31 — The Empty Place on the Scale: Caravan Quest Portfolio (DRAFT)

### Scope and evidence boundary

This pass develops an authored quest portfolio around an arriving caravan, its visible cargo, the settlement's need, and what a trader is willing to say. It is a content and integration proposal, not a claim that the proposed quest chain, region-to-settlement specialty mapping, or character already exists. The World Bible's caravan-specialty prompt supports this topic; current code supplies useful but distinct building blocks: route records describe origin/destination, travel days, risk, faction, imports and exports; RegionalSupplyRouter derives specialty lots from goods provenance; RegionalPriceAtlas applies authored modifiers with a neutral default; TradeTellEngine chooses posture text by stance and trust. No new trade ledger, route schedule, price authority, quest state store, or cargo generator is proposed.

### Quest packet: “The Empty Place on the Scale”

Premise: a scheduled trader reaches a settlement after a harsh leg. The player sees an unusually small or unfamiliar specialty assortment and hears the quartermaster explain that the route's expected surplus did not arrive intact. The quest is not “make the caravan carry a hand-picked item.” Its authored trigger is a confirmed visit / available trading scene; its premise facts are read from current route, cargo, and authored context. Until a host proves that a route-arrival fact is exposed to quests, the trigger remains a DRAFT integration dependency.

Quest classes and outcomes:
- A discovery beat asks the player to compare the manifest, stall, and a local worksite. It can resolve through inspection and adds no resource.
- A faction or character branch asks whether to disclose a local production capability, keep it private, or bargain for a future shipment. These are authored decisions whose results must route through existing quest/faction/market owners only where a real effect is supported.
- A resource branch lets the player offer a currently owned, eligible good through the existing barter flow. A quest must never create inventory, reserve hidden stock, or debit an offer twice.
- An investigation branch follows a discrepancy between the route's authored demand/surplus description and the observed caravan cargo. It can conclude as ordinary loss, a trader's deliberate reprioritization, or inconclusive evidence; these are possible story interpretations, not facts the simulation may assert without authored evidence.
- A failure-continuation branch preserves play if the player leaves before the caravan departs, refuses the offer, lacks the requested goods, or cannot access the stop. Reframe the outcome as a missed window, a smaller exchange, or a later clue, rather than silently marking unrelated objectives complete.

Required locations should be constrained to currently resolvable places: the active shelter/trading scene, the existing caravan route stop if it is materialized, and at most one discovered worksite or clue destination selected by an owned expedition/location seam. Do not make a new named POI mandatory until its ID, discoverability, fallback, map visibility, and consumer are confirmed. A missing worksite becomes a clue at the available trade scene; the quest remains possible through conversation and manifest comparison.

Quest lifecycle proposal: Available when its proven arrival/visit precondition is true; Discovered when a manifest or tell exposes the discrepancy; Accepted after the player explicitly agrees to investigate; In Progress while any required evidence is unresolved; Blocked only when a named prerequisite is temporarily unavailable; Partially Completed after a clue is recorded but the negotiation remains; Failed only when the authored opportunity expires or an explicit branch closes; Reopened only by a supported later arrival or new authored evidence; Completed when the chosen branch resolves; Resolved when its consequences have been applied and the log can stop showing pending work. “Abandoned” means the player stops tracking it, not that a trade is reversed. If current quest schema cannot express all these statuses, map to supported states and document the limitation; do not extend save state by implication.

Acceptance checklist for a future implementation package:
1. Show a journal card only after its actual trigger is witnessed.
2. Every branch has one explicit completion and one recoverable failure path.
3. No branch depends on an invented settlement-specialty field.
4. Cargo and barter quantities are re-read at action time.
5. Closing or missing a visit has legible feedback and a future clue or honest expiry.
6. Replay and save semantics use current quest authority and deterministic existing inputs.

Production slice: one quest with three short stages, two reconverging negotiation branches, one optional clue, and three endings (cooperation, guarded agreement, or missed opportunity). Begin with one route and one actual authored destination; prove host facts first. Add further routes only when distinct cargo provenance creates a genuinely different player decision. Cost remains medium because branch QA and state recovery exceed the writing volume; expansion is gated by route and quest ownership evidence.


### Pass 31B — Quest variants, rewards, and production card

| Quest seed | Entry condition | Core play | Failure that continues | Reward shape | Reuse |
|---|---|---|---|---|---|
| Manifest at the Table | Current caravan scene is confirmed and quest not previously resolved | Ask about route expectation, compare current stock, choose a negotiation posture | Visit closes; journal records missed window only if the window is authored | Information or ordinary successful barter; no free goods | Medium: same graph can consume different observed lots |
| The Missing Lash Point | Player notices an empty tie-down or a marked manifest gap | Inspect an available clue source or ask the escort; decide whether to share the finding | Clue site unavailable; preserve the lead and offer a later visit | Clue and relationship-facing scene only, unless a canonical owner supports more | Low to medium; requires authored evidence variants |
| A Repairable Specialty | Eligible local or caravan good is actually visible | Offer a supported barter, request a repair instruction, or keep the capability private | Needed item absent; discuss technique or decline | Trade receipt, recipe only if a recipe authority owns it, or narrative acknowledgement | High as a scene template, but recipes are content-specific |
| The Next Load | Prior agreement fact exists and later visit is confirmed | Verify what changed, fulfill or release the earlier agreement | Trader does not return or cargo differs; close the promise as unresolved without phantom delivery | Completion record or apology/alternative branch | Low; requires durable agreement owner and future-arrival evidence |

Reward discipline matters: quest rewards should prefer information, access to an already implemented service, a current trade opportunity, or a small authored relationship beat. A quest should not award “regional price discount,” faction standing, a new map region, a recipe, or guaranteed future cargo unless those outcomes are already owned, persisted, and validated. Ordinary goods obtained via barter are transaction results, not quest rewards; do not count them twice in journal totals.

Story texture can be carried through optional inspection rather than adding a second quest. A player can inspect the route sheet, count the lash points, or listen to the escort. Each clue needs an evidence label (seen, read, reported), a source ID, and a resulting dialogue option; if the current content contract has no source attribution field, keep the clue authored in a single graph node and avoid pretending it is a general-purpose evidence framework.

The thin slice should be measured by distinct states and effects, not prose volume. Suggested authoring budget: 8–12 nodes, 4 player intent responses at the hub, 3 reconverging decisions, 2 nonfatal failure exits, 3 conclusions, and no more than 5 durable quest flags. A content reviewer can then trace every flag to an observable journal change. Add a second route only after the first route proves arrivals can be observed and replayed. If the trade scene has no route-arrival event, keep the design as an opportunistic shelter merchant scene and remove scheduled-travel claims.

## Pass 32A — “The Third Turn” Maintenance Investigation Portfolio (DRAFT)

### Core premise and playable promise

Part 47 asks for closed loops, not just more logs. “The Third Turn” is a shelter-scale maintenance investigation about a recurring vibration heard during the night shift. A machine tell, an old handwritten record, and a technician's account disagree about whether the sound is worsening. The player must distinguish a warning from proof, decide which inspection can be spared, and leave a truthful record for the next shift. The fiction does not assert that an actual component is failing. Only the current system owner can establish the machine's condition; the authored logs provide historical testimony and context.

This premise builds on verified structures without claiming the integration already exists. Main.ShelterInfrastructure builds MachineConditionReadings from several current host owners and evaluates machine quirk/glitch content. A repeat-once glitch can be recorded through JournalSystem's existing glitch-noted knowledge. BunkerMaintenanceCatalog loads the authored maintenance-glitch file, while bunker maintenance logs and equipment failure logs are separate narrative files. These records do not by themselves prove live wear, and the proposed quest must not mutate a machine's health by reading prose.

### Portfolio cards

**Primary investigation — The Third Turn.** Entry: a supported live diagnostic tell is presented to the player or an existing journal/codex fact establishes that a specific glitch was noted. Core loop: compare the tell with a dated maintenance entry; request a second account; choose to inspect, defer with a named reason, or stop treating the sound as evidence. Required locations: shelter machine/maintenance presentation and one authored archive, journal, or work area only if that location is truly addressable. Failure-forward: no inspection capacity, unavailable witness, contradictory records, or machine condition returning to normal resolves into an uncertainty finding rather than a false diagnosis. Reward: a documented conclusion, an existing service action if directly supported, or a character response; no free parts, condition points, or skill grants. Type: investigation + shelter survival. Reuse: high, if multiple actual condition owner keys map to authored incident packets through validated IDs.

**Character quest — The Night Mechanic's Margin.** Entry: a rostered technician or speaker is available through an existing character/dialogue owner. The technician says that they write down sounds because no shift receives the same machine twice. Player can protect the note from blame, ask for a second signature, or file it under the machine rather than the person. Outcomes change the scene's trust and attribution; no relationship meter or duty roster is modified without a currently supported command. Failure-forward: the character is absent; their note remains but conversation waits. Reward: a new dialogue node, an attributed codex discovery, or a resolved quest receipt. Reuse: medium; voice variation must be authored by identity, not randomized titles.

**Faction/operations quest — The Service Window.** Entry: a visible service schedule, shortage, or maintenance task exists in the current economy/workshop owner. The player weighs taking a machine offline now against waiting for available work and parts. Choices may be “inspect only,” “defer and post a reason,” or “use the existing repair command.” The quest itself does not invent a queue or consume resources. Failure-forward: unavailable part or work slot leaves a logged deferral and a later retry. Reward: truthful service receipt or preserved capacity; no promised uptime bonus absent a supported measurement.

### State, exit, and evidence discipline

Proposal states: Available after a supported tell/source is observed; Discovered after the player inspects a record or hears a report; Accepted when the player agrees to reconcile it; In Progress while evidence or a decision remains; Blocked when a specific owner reports a temporary constraint; Partially Completed when one source has been recorded but interpretation remains; Completed when the chosen investigation outcome is recorded; Resolved after any supported downstream consequence is confirmed. Failed is reserved for an explicitly time-limited inspection or witness opportunity that closes. Abandoned means the player stops tracking the task, not that a repair was attempted. Reopened requires a later, independently observed signal with a supported link to the same incident. If current QuestSystem exposes fewer states, map this proposal onto actual states and keep richer wording in the design only.

Evidence classes must not collapse: live reading, authored historical log, character report, player's interpretation, repair receipt. A historical record can be old, incomplete, or refer to a different machine. The quest should let the player say “same sound, uncertain cause,” rather than force a binary solved/unsolved answer. Data with no exact machine ID or condition key remains context, never a diagnostic result.

### Production slice and success criteria

First slice: one machine family; one noncritical tell; two historical records; one character; three player intent responses; one explicit deferral; two endings (documented uncertainty or corroborated issue). Keep it outside critical progression so absence of a diagnostic, log, or character never strands a campaign. The player sees what was observed, what was reported, what action the owner accepted, and what remains unknown. Integration readiness requires exact current APIs for tell discovery, journal facts, quest transitions, character availability, and optional maintenance commands. Production cost: medium for a single scene, high for multiple machine families because condition keys, log references, and narrative voice need per-row review.


### Pass 32B — Side-quest bank, outcomes, and production sequencing

The primary arc should be accompanied by small independent stories that add variety without requiring a generalized maintenance simulator.

**“A Label Turned Inward” — discovery quest.** A player finds a service tag whose date is legible but whose machine reference is turned to the wall. The player can read it, ask who filed it, or leave it alone. Its outcome adds only a discovered authored record if the existing narrative discovery path supports this exact catalog. Failure states: the tag is unreadable, the content was already seen, or no matching source record exists. Reward: provenance and one character reaction. Reuse is medium; the sensory template can be reused but each record identity is authored. Production cost low. Core if it explains a safety-relevant live interaction, optional expansion if purely archival.

**“Last Safe Window” — survival/service quest.** A current machine owner presents a real service option whose downtime overlaps another authored need. The player chooses whether to use it now, wait for a named availability condition, or record refusal. The quest does not create a timer: it observes whatever schedule already owns the service. Failure states include resource shortage and changed schedule; both keep a later route where the owner permits one. Reward: a returned result or informed tradeoff, never a guaranteed efficiency buff. Reuse high only if several owners expose consistent service results. Production cost high because balance and save behavior are cross-system.

**“Two Hands on the Wrench” — relationship/character quest.** Two people disagree over whether to document a repair before completing it. The player can mediate, defer judgment, or select an existing work result. Relationship consequence is optional and gated on a proven owner. If no relationship action exists, branches change only the characters' spoken tone and journal framing. Failure state: a witness is absent; the disagreement remains unresolved instead of casting a random survivor. Reward: human closure and a future callback only when Chronicle can remember the exact action. Reuse medium.

**“The Part That Fits” — resource/crafting quest.** A part from an authored inventory/recipe source appears to match a machine request. Player may compare specifications, request an ordinary transaction, or refuse a risky fit. This story is blocked until the real repair API declares accepted items and the inventory authority confirms possession. It never generates a spare or introduces a crafting recipe by narrative fiat. Reward: completed transaction receipt or a declined offer. Production cost high; put in an expansion unless it closes an already-critical onboarding gap.

Recommended sequence: first ship the non-mutating discovery and record comparison; next test one existing repair result in a sandboxed implementation package; only then author consequences tied to uptime or campaign supply. Keep each quest independently completable. A broken optional record must not prevent an ordinary machine action. Core content should teach players to distinguish observation and action. Expansion content may add deeper crew histories, repeated service callbacks, rare component provenance, and campaign chronicle branches after their owners are verified. Completion analytics should count resolved evidence questions and confirmed commands separately; “quest complete” alone hides whether players understood the machinery.


### Pass 32C — Completion receipts and quest graph hardening

Require each quest implementation card to state trigger owner/event, source IDs, player-facing surface, legal transitions supported by the current quest owner, optional service command, save owner, failure-forward route, and focused acceptance evidence. Reject any card where “machine got worse” is inferred from dialogue or a generic maintenance file.

The completion receipt should answer four questions: what the player did, what the source owner confirmed, what remains uncertain, and whether another action is available. Example: “You recorded Vale's report. No inspection was run. The diagnostic tell has not returned. The service window remains available.” This is more informative than an opaque “investigated” state and does not require a new telemetry system if projected from existing quest outcome and machine reading.

Include a negative-result completion. The player can establish that two records cannot confidently be joined, preserve them separately, and close the optional investigation. If later evidence appears, reopen only through an owner-backed trigger and identify the changed fact. Do not re-open a solved quest whenever the same tell renders again.

Stage graph: optional trigger; discovery of one dated record or witness report; reconciliation of identity/date/source; player intent to inspect, defer, or document uncertainty; owner-confirmed result; closeout. A missing source can route to “unknown; preserve report” and then a supported decision. A core tutorial variant must distinguish “observe” from “act” and keep an action reversible until the command confirms it. The wider investigation remains optional so missing tell, log, or character never strands campaign progress.

Expand to additional characters only when each adds a distinct source class or human tradeoff. A second mechanic repeating Vale's note adds no loop closure. Measure success by correct distinction between observation and action, legible failure recovery, and the number of confirmed commands, not only by quest completion rate.


### Pass 32D — Quest quality signals and implementation backlog

Do not evaluate this arc by completion percentage alone. Useful design measures are: players who can identify which statement is a current reading versus a report; optional leads abandoned without blocking core shelter play; stale service choices rejected without loss; number of decisions that end in documented uncertainty; and confirmed repairs separated from narrative conclusions. These are future evaluation questions, not new runtime telemetry requirements. If no analytics authority exists, use focused review scenarios instead of adding tracking.

Ordered backlog: (1) verify the actual quest and JournalSystem transition APIs; (2) make one read-only record comparison packet; (3) validate character availability; (4) add a failure-forward closure; (5) only then bind one owner-confirmed inspection result; (6) add recurring callbacks after a canonical event exists. Each item can be declined independently if current evidence is stale.
