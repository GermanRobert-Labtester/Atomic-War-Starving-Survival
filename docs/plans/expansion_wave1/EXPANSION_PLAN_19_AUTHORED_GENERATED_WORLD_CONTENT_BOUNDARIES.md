# Expansion-series Plan 19 — Authored and Generated World Content Boundaries

**Status:** Architecture and content proposal; documentation only.
**Numbering note:** Expansion-series Plan 19 is namespaced within this folder and does not replace any global Plan 19.
**Purpose:** Separate permanent authored content from per-run generated content across quests, expeditions, locations, dialogue, characters, and world facts. Define an MVP that uses existing owners and can grow without a general-purpose simulation framework.

## 1. Architecture rule

Keep the distinction plain:

- **Definition:** permanent authored content with a stable ID, validated references, text keys, eligibility rules, and no mutable run progress.
- **Instance:** one runtime occurrence created from a definition, with only the bindings and progress needed to continue it.
- **World fact:** current state owned by an existing system, such as a discovered map node, quest outcome, relationship, faction standing, shelter condition, or campaign result.
- **Presentation:** a read model or text selected from those facts; it does not become another owner.

The world context used for eligibility is a read-only snapshot gathered from current owners. It is not a new global world-state database. When a new kind of durable fact is necessary, assign it to the domain that owns the consequence or stop for an architecture decision.

## 2. Content flow

1. Canonical authored JSON defines quest, location, character, dialogue, encounter, and template identities.
2. Current loaders validate schema, stable IDs, typed references, ranges, and optional-field behavior.
3. A selector reads an immutable snapshot from relevant owners and filters definitions.
4. The existing deterministic RNG or stable ordering selects from eligible content.
5. A runtime owner creates an instance with bound IDs and the minimum required state.
6. Events and player commands advance that instance through its owner.
7. The UI and journal display read models.
8. Save sections capture durable owner state, not entire catalog copies or presentation caches.

Authored content may define what can happen. Generated content may choose among those authored possibilities. Generated content cannot manufacture plot canon, a new faction, a location ID, an item, a character, a new rule, or an unvalidated consequence.

## 3. Separation by domain

### Quest definitions and generated quests

An authored quest has a permanent ID, type, objectives, requirements, text keys, location/actor constraints, rewards, failure path, and versioning metadata. A generated quest instance stores its stable instance ID, source definition, seed or binding, selected actor/location/resource IDs, progress, status, and owner-supported outcome facts. One definition can create many instances; an instance does not rewrite its source definition.

### Locations and expedition appearances

A permanent location has a canonical map or micro-location ID, authored name and description, route/anchor, visibility and discovery rules, tags, hazards, content availability, and stable references. A temporary expedition appearance is a selected instance of that location, not a second location identity. Temporary weather, encounter, depleted state, or quest presence is held by the existing owning system. A generated appearance must refer to a valid authored location and cannot create a disconnected map node.

### Characters and dialogue

A character definition supplies identity, voice guidance, role, authored availability, and valid relationship/memory references. The current survivor roster or character owner supplies run-specific identity and state. Dialogue text is permanent authored content; line selection is generated from allowed conditions. A line never creates a hidden character or durable emotional value by itself.

### World facts and story consequences

Quests, relationships, factions, map discovery, inventory, shelter, and campaign completion each retain their current owner. Dialogue and generated quest selection read the smallest facts they need. World consequences return as typed requests to those owners. Do not serialize a broad “world snapshot” as a duplicate save.

## 4. Location availability by quest type

| Quest type | Availability rule | Map treatment |
|---|---|---|
| Main | Required stable authored location or explicit alternate route; critical progression cannot rely on a rare roll. | Pin known target or show a mandatory clue. |
| Character | Location may follow the character's existing duty/availability; provide absence fallback. | Show only what the character or quest has revealed. |
| Faction | Require live faction/quest gate and an accessible contact or delivery site. | Show contact or known destination after acceptance. |
| Discovery | Start from an authored clue or a valid discovery consumer. | Hide site until clue reveals it; show clue marker first. |
| Investigation | Require enough accessible evidence sources to reach at least one supported conclusion. | Reveal evidence nodes as knowledge is earned. |
| Escort/protection | Route, start, and destination must all be valid before acceptance. | Preview route and travel estimate. |
| Survival | Use a real shelter or expedition context that the relevant system can observe. | Display the task context and required supplies. |
| Resource/crafting | Require an existing source, item, recipe, or workbench consumer. | No invented deposit; show known source or alternatives. |
| Location-based | Require stable ID, consumer, route, visibility, and one-time/repeat policy. | Visible according to discovery and quest knowledge. |
| Timed | Require a real canonical deadline source and enough route time. | Display remaining time and route estimate. |
| Repeatable/rotating | Choose from a validated pool with cooldown/deduplication supported by an existing owner. | Show current offer and expiry if real. |
| Hidden/environmental | Require a legible clue that can actually appear in a live scene or encounter. | Show nothing until discovered; then show the clue or lead. |
| Choice-reactive | Require an existing saved choice fact or neutral fallback for old/missing saves. | Reveal future destination only when its gate is met. |
| Fail-forward | Require a distinct alternate objective and route, not merely a different completion label. | Repoint the map when the quest owner changes the requirement. |

## 5. New content seeds for later authoring

The following are provisional story scaffolds, not approved IDs or canon. Search current catalogs before naming any final IDs.

### Side quest seed: “A Place for the Copy”

A shelter record keeper asks the player to return a damaged field register from a location already on the map. The original can be preserved, copied for a neighboring shelter, or left in place to avoid a dangerous trip. The required location appears on the expedition map after acceptance. If it becomes unavailable, an existing route clue identifies a second copy or the quest enters a visible delay state. The reward is a journal fact or access permission only if a current owner supports it.

### Discovery seed: “Three Marks on the Culvert”

Three scratched marks use the same direction code but have different dates. The first clue starts the quest; the second offers a contradiction; the third allows a cautious conclusion. Each site is an existing reachable node. If one clue is not eligible to spawn, the data must provide a different authored clue path rather than quietly reducing the evidence count.

### Character voice kit: the three Unentered Shelf witnesses

Reuse the already introduced clerk, nurse, and mechanic as provisional roles rather than adding another key-custody character. The clerk asks which count can be repeated; the nurse separates a promise remembered from a promise witnessed; the mechanic identifies what a sealed depot can physically prove. They disagree on evidence quality while sharing a practical interest in preventing a false accusation. Their lines vary with verified quest facts, not with a new personality meter. Final names, survivor IDs, histories, and voice details require a continuity search against the live survivor and dialogue catalogs.

These seeds favor small scenes with consequences the player can see. They avoid requiring a new global scheduler, procedural prose generator, or hidden relationship score.

### Location description and voice kit

**Location seed — the west counting room:** The room has one window and four old shelves. The labels survived; the ledgers did not. A line of pencil marks on the sill stops halfway through a winter, where somebody used the paper for kindling.

**Location seed — the cable court:** The cable still crosses the yard, but its warning flags have faded to the color of dust. From the gate, the far post looks close. The broken paving makes the approach slow enough that a player should see a travel estimate before dispatch.

**Return-state variant:** “The new sheet is nailed beside the empty hooks. Someone has copied the names in larger writing.” Use this line only if the journal, quest, or location owner can prove that the copy was made. Without a source fact, use the neutral arrival description.

**Character voice seed — Toma, provisional route clerk:** Toma records what can be repeated and marks everything else as a report. He is patient with honest gaps, impatient with certainty unsupported by a route. Under pressure his sentences get shorter, but he does not become cruel. Any use of Toma's name, role, or history requires a search of current survivor and narrative catalogs.

## 6. Minimum viable version

The MVP adds one stable authored location or reuses an existing one, one authored quest, one optional generated binding if an established generator consumes it, and one dialogue exchange. The quest can complete or take a visible fallback route. The map shows a truthful pin or clue. Any state that survives save/load belongs to a current owner and is round-tripped through that owner's registered save section.

The MVP explicitly defers unrestricted procedural narrative, dynamically invented locations, unrestricted faction generation, general-purpose rule expressions, a universal world-state registry, free-form dialogue scripting, and campaign-wide fact snapshots.

## 7. Optional expansion layers and dependencies

- **Content variety:** more authored templates, variants, locations, and character arcs. Depends on catalog integrity and a live consumer.
- **Seasonal or rotating availability:** depends on a canonical day/season source, deterministic selection, and clear expiry.
- **Richer generated binding:** depends on validated candidate pools, replay stability, and persisted instance snapshots.
- **Cross-quest callbacks:** depends on stable, saved outcome facts with identified readers.
- **Narrative tooling:** depends on agreement about the canonical authoring schema and CI validators.
- **Localized prose:** depends on text-key coverage, fallback language, and UI layout verification.

## 8. Integration course

**Phase 0 — Authority census:** Review current loaders, data files, quest, map, expedition, character/memory, dialogue/encounter, save, RNG, and host owners. Verify actual consumers. Compare global Plans 17, 32, 49, 59, 92, 133, 171, and 200 and any current claims.

**Phase 1 — Boundary contract:** Write a short definition/instance/fact/presentation table for the selected feature. Mark each fact as authored, derived, mutable-owned, or generated binding. Reject any item with two owners.

**Phase 2 — Authored slice:** Add a minimal catalog row and one consumer. Run the current integrity validator and any data-specific validation before adding volume. Confirm references and all fallback strings.

**Phase 3 — Runtime route:** Connect the definition to the current owner, the owner to canonical events and commands, and the returned state to existing UI. Avoid broad composition-root rewrites.

**Phase 4 — Durability and replay:** Specify save section, default migration behavior, stable ordering, seeded selection, and idempotence. Add only the persistence or determinism tests warranted by the actual state change.

**Phase 5 — Content expansion:** Add more authored rows only after measured reachability, loading cost, text quality, and replay behavior meet acceptance. Use bounded pools and lazy presentation where current architecture supports it.

**Phase 6 — Review and handoff:** Document files, consumers, state owner, commands, outcomes, and limitations. The integrator updates shared ledgers after acceptance.

## 9. Acceptance

Definitions remain immutable at runtime; instances bind only valid authored identifiers; generated text stays within authored fragments; one owner writes each mutable fact; map availability never silently blocks an accepted main quest; save data has one registered owner; and deterministic selection is stable under the existing replay contract. This proposal itself changes no game code or authoritative data.

## Continuation pass 2 — content provenance and change control

### Provenance table

Every runtime object shown to the player should be traceable to authored content or to a valid generated binding. Keep provenance in the definition/instance type that already owns the object:

| Content visible in play | Permanent authored source | Allowed generated binding | State that stays elsewhere |
|---|---|---|---|
| Main or side quest | Quest definition and objective graph | approved actor, item, or location selection | progress and lifecycle in quest owner |
| Expedition destination | Location definition and route metadata | selected candidate IDs and mission context | discovery and travel in map/expedition owners |
| Character conversation | Dialogue nodes, speaker voice notes, text keys | eligible variant selection | relationship, skill, memory, and quest facts in their owners |
| Faction request | Faction identity and authored obligations | choice of an eligible authored request | standing, access, and resources in faction/economy owners |
| Environmental clue | Clue definition and placement rule | appearance at a valid authored site | discovery and journal record in current owners |
| Temporary encounter | Encounter definition and trigger policy | deterministic selection from eligible encounters | encounter resolution and resulting effects in encounter/domain owners |

Generation is selection, binding, and controlled variation. It is not content authorship at runtime. When no authored candidate survives filtering, return no candidate and a truthful empty state. Do not produce placeholder prose that implies an event occurred.

### Content revision and save compatibility

An authored catalog may change between software versions while a player has an active instance. Before promoting a definition revision, classify each changed field:

- Display-only copy can change without altering a saved objective.
- A new optional response can be added if old state selects a neutral route.
- An objective target change needs a migration or compatibility mapping.
- A removed location needs a supported equivalent or an explicit conversion to a delayed/fail-forward route.
- A changed reward must not be applied retroactively or granted twice.
- A changed quest type or terminal meaning requires a review of read models, statistics, and save migration.

Prefer stable identity plus a small authored revision value when it lets the current save owner detect relevant changes. Do not add a second content snapshot to saves unless the current persistence architecture requires that capability and the owner approves it. Old saves with missing or unknown provenance resolve through authored fallback behavior.

### Event ownership and freshness

The host can gather dialogue or quest context from several owners at different points in the campaign tick. Define when each snapshot is built: for a conversation, immediately before entry or response resolution; for expedition selection, at dispatch; for a quest objective, when the canonical event arrives. Avoid relying on a stale panel snapshot after another system has changed a requirement.

Where a choice crosses owners, validate the response against fresh state at commit time. If the speaker is no longer present, a faction gate has changed, or the location closed, reject or redirect safely. A previously displayed line is not authority to apply a now-invalid reward.

### Authorship and review checklist

For each new content record, the writer and integrator should be able to answer:

1. Is the ID new, or does it duplicate a current catalog entry?
2. Is the row permanent authored content or a generated instance?
3. Which loader reads it, and which active runtime consumer uses it?
4. Which stable owner facts gate its availability?
5. What happens when an optional actor, faction, location, or old-save field is absent?
6. Which system owns every result?
7. Which exact text keys, map IDs, item IDs, and quest IDs are cross-referenced?
8. Can the player see why this content is available, hidden, blocked, or complete?
9. What remains stable across a repeated seeded run?
10. How can the row be withdrawn without corrupting an existing save?

### Narrative growth without a global scheduler

An authored side story can feel responsive without a new scheduler. Let an existing canonical event expose a condition, let a quest or encounter owner select the next authored stage, and let location availability follow that stage. For example, the damaged register in A Place for the Copy can lead to a shelter visit after the player finds a second source. If that source is unavailable, a distinct authored clue can preserve the question. No daily story simulation is needed.

An expansion may add new locations, characters, and faction scenes as data when the current systems can consume them. New mechanics require a separate owner proposal and live integration package. This distinction keeps creative output scalable: authors can add material within proven contracts while engineers add new contracts only when a playable gap requires them.

## Continuation pass 2 acceptance

The provenance boundary is reviewable when every authored row has a loader and consumer, every generated binding is constrained to canonical IDs, saved instances survive content revision, and all missing-content and stale-snapshot paths have explicit behavior. No catalog or save implementation is claimed here.

## Continuation pass 3 — catalog contracts and world-state projections

### Definition and instance boundary by field

The following table makes common ownership decisions explicit. It is a review aid; actual types and serializers come from current source.

| Field | Authored definition | Runtime instance | Canonical mutable owner |
|---|---|---|---|
| Quest name, title, description, voice notes | Yes, through text keys and authoring notes | No copy required unless current save contract needs it | Content catalog/localization |
| Quest type, objective graph, allowed failure routes | Yes | Current objective and status only | Quest runtime |
| Location name, description, authored tags, base route reference | Yes | Selected instance ID or quest binding | Map/location authority |
| Location discovered, completed, locked, occupied, depleted | Default or rule only | No second value | Current map, encounter, or domain owner |
| Character identity, role, voice, allowed scenes | Yes | Present/absent binding when required | Roster and character systems |
| Character relationship, skill, health, assignment, memory | No mutable defaults masquerading as state | Read when needed | Relationship, skill, health, duty, memory owners |
| Generated candidate set and selected IDs | Candidate pools are authored | Yes, if needed to continue an active expedition/quest | Expedition/quest owner |
| Generated line selection | All possible text variants are authored | Usually transient; persist only if a later effect depends on the choice | Dialogue or current quest owner |
| Faction identity and authored asks | Yes | Bound actor or active request ID | Faction and quest owners |
| Faction standing/access | Initial display only where valid | No duplicate score | Faction authority |
| World event definition | Yes | Active occurrence only when the event owner supports it | Campaign/event owner |
| Ending interpretation | Authored presentation mapping | Outcome facts from completed play | Existing ending/campaign owner |

If one field could appear in two columns, document whether the second occurrence is a read model, immutable snapshot, or actual owner state. Do not serialize a read model as if it were independent truth.

### Availability, visibility, and commitment

Content has three gates that should remain distinct:

- **Availability** asks whether a definition can be offered under current campaign and actor rules.
- **Visibility** asks what the player has learned and what the map or conversation may display.
- **Commitment** asks whether the player accepted a quest, entered an expedition, or chose an effect.

An available secret quest may be invisible until a clue event. A visible location may not yet be reachable. A quest can be accepted while its exact destination remains unknown if the authored clue path is playable. These states should be derived from canonical facts and represented in the existing read models rather than stored as four competing booleans on every catalog row.

Use explicit authored eligibility conditions and owner-backed current facts. Do not let a generated selector promote an unavailable permanent definition, and do not let an undiscovered site become visible because a generated instance contains its ID. Acceptance can expose a quest marker when the narrative grants that knowledge; the map still controls whether it is an exact pin or a clue.

### World-state projection contract

When a quest or conversation needs multiple facts, compose a minimal projection for that decision:

~~~text
QuestEligibilityView
    current_day_or_supported_campaign_phase
    player_known_facts[]
    active_quest_ids[]
    eligible_actor_ids[]
    reachable_location_ids[]
    faction_access_facts[]
~~~

This is a query input, not a new saved type. The projection should be built at the decision boundary and discarded after the decision unless the current owner deliberately stores a deterministic binding. Use stable ID sorting and explicit “unknown” values. Do not copy inventory contents, full relationship histories, every campaign flag, or all map state when the selector needs only a small subset.

If source facts can change between the projection and effect application, revalidate the critical command against the live owner before commit. For instance, a destination can close after a panel is opened; the old display cannot authorize dispatch to a now-unreachable node.

### Content bundle and release discipline

Ship a vertical content bundle with:

1. Canonical authored definition rows.
2. One loader/validator path and a test fixture using current schema.
3. A confirmed runtime consumer.
4. The first host entry point and read model.
5. Text keys and accessibility-friendly summaries.
6. A save/migration note when mutable state is introduced.
7. A deterministic replay or stable-order note for generated selection.
8. A missing actor/location/catalog fallback.
9. A content-continuity review.
10. A handoff receipt with focused verification results.

Keep these items in the feature package instead of scattering “temporary” copies among feature docs. When an authored definition is withdrawn, use stable redirects or an explicit migration for active instances; do not leave dangling references or silently reassign a player's choice.

### Story and location content expansion

An expansion pack can add a new authored district using the existing location contract: arrival description, one optional local detail, a quest availability rule, discovery/visibility behavior, supported route, and return-state variation sourced from an actual owner fact. It can add a character voice sheet with role, concrete wants, boundary, vocabulary, stress change, and relationship-safe fallback. It can add generated variants only after authored candidate IDs and all consumers are validated.

Example location prose for the provisional cable court: “At the far post, three clamps are bright where the rest of the metal has gone dull. The cable is still carrying a load. Nothing in the yard says who last checked it.” A follow-up line after a real repair could read: “The clamps hold. The post still leans, but now the warning flag stays up in the wind.” The second line is selected only from an owner-backed repair result; merely accepting a quest cannot display it.

This structure supports additional lore without creating a location simulation. Existing world owners continue to decide whether a cable is damaged, a route is open, or a site has been visited.

## Continuation pass 3 acceptance

The boundary package is ready to promote when field ownership is unambiguous, each content bundle has a loader and consumer, eligibility/visibility/commitment are separated, read projections are short-lived and revalidated, and withdrawing a definition cannot strand a saved instance.

## Continuation pass 4 — provenance records, save boundaries, and content release workflow

### Content identity and provenance record

Every permanent definition needs a stable content ID, content kind, authoring source, schema version, owning feature, and references to canonical location, character, quest, faction, and dialogue definitions. Generated instances carry the template ID and the generation inputs needed to reproduce their authored variation, but they do not acquire permanent identity merely because the player encountered them. A hybrid record points to one authored anchor and identifies which fields are permitted to vary.

Proposed provenance metadata is a validation aid, not a second gameplay registry: definition ID; source bundle; source revision; authored/generated/hybrid classification; allowed variant fields; required owner facts; runtime consumers; localization keys; and deprecation/replacement note. Keep build-only provenance out of saves unless the runtime truly needs it to restore an instance. When it does, persist the smallest stable reference and generation seed/state required by the canonical owner.

An authored location can be revised for clarity without changing its ID if its gameplay meaning remains compatible. A generated instance must restore against the same stable template or a declared migration. Removing a template referenced by a save requires a replacement mapping or a safe orphan presentation handled by its owner. Never derive durable identity from row order, localized names, dictionary iteration, current display text, or a freshly rolled seed after load.

### Save and replay decision matrix

| Content situation | What must survive save/load | Replay rule |
|---|---|---|
| Permanent authored site | Canonical site ID and owner-backed persistent changes only. | Same definition; no random identity. |
| Generated variation on an authored site | The canonical site plus variation inputs if they alter gameplay-relevant facts. | Same seed and input facts reproduce the same eligible variant. |
| Temporary encounter at a site | Encounter state only if its owner treats it as campaign-persistent. | Re-select from the explicit deterministic stream when a new dispatch occurs. |
| Dialogue variation | No text copy in world state; save only the underlying fact if a canonical owner owns it. | Re-evaluate from current context facts. |
| Quest-bound location substitution | Quest route fact and current canonical destination as required by the quest owner. | Do not rebind during restore unless the saved route is invalid and a migration rule applies. |

This matrix prevents generated content from becoming an accidental second save authority. Before adding persistence, identify the existing owner and show its capture/restore path. If there is no such owner, keep the content presentation-only or bring a bounded architecture proposal to the integrator.

### Bundle validation and release flow

Treat a content release as a dependency bundle rather than an isolated file drop. Resolve referenced IDs against the current catalogs; validate schema and allowed variant fields; prove each entry has a runtime consumer; check quest-to-location reachability; check dialogue conditions against owner facts; run narrative continuity and duplicate-text review; and generate the normal data-integrity report. Any new loader or runtime consumer is a separate implementation seam and must be claimed and integrated under the live ledger.

The authoring flow has five review states: draft; reference-complete; consumer-verified; continuity-reviewed; and release-ready. Draft prose may be developed while an API is under audit, but operational text stays explicitly unbound until a current action/fact producer is proven. A partially integrated feature may only be extended at the verified boundary: mark unavailable fields, avoid a second owner, and write fallback copy that does not claim the pending feature works.

Deprecation should preserve old-save readability. Mark an old definition unavailable for new generation, keep its ID resolvable for saved references, and route it to a compatible replacement or a truthful retired-content response. Deletion is a repository maintenance action with current ownership evidence, not a narrative convenience.

### Example bundle with a controlled generated layer

Consider a provisional “service-yard survey” content bundle. The authored anchor supplies the yard's identity, arrival text, route constraints, and a fixed inspection object. A generated layer may select one of three weather descriptions from a stable seeded choice. It cannot change whether the yard is open, invent a repair result, create a new character relationship, or claim a quest objective completed. Those gameplay facts remain with their existing owners.

If the player later repairs a mast, the return description reads the canonical repair result; it is not stored as a mutable paragraph. If the authored weather variant is removed in a later release, an older save still resolves to the yard anchor and can use the neutral description. This is the standard pattern for adding atmosphere without forking location simulation or save state.

### Expansion versus core placement

Core-game content belongs in the base bundle when it is required for onboarding, critical progression, systemic examples, or a complete minimum viable route. Expansion content may add optional arcs, regions, factions, locations, and variants, but it must not make a core quest unknowable, break saved canonical IDs, or add a required consumer that only the expansion supplies. Shared schema improvements belong to the owning runtime package; optional authored rows belong to the expansion bundle. This distinction is validated with expansion-disabled and expansion-enabled catalog builds.

## Continuation pass 4 acceptance

Provenance work is ready when stable IDs, permitted variation, consumers, and persistence boundaries are explicit; bundle validation catches unresolved references and hidden operational claims; and content withdrawal has a save-compatible replacement path. Generated flavor may enrich a site, but only the canonical owner can change the site's durable gameplay state.

## Continuation pass 5 — content classes, change control, and collision review

### Content lifecycle and evidence states

Give each bundle an editorial lifecycle that makes its implementation status visible without pretending the game has a new runtime state machine. The authoring/review labels are Draft, Canon-Checked, Schema-Checked, Consumer-Verified, Integration-Ready, Released, Deprecated, and Archived. They describe evidence about a bundle. They do not replace quest status, map visibility, faction state, or save state.

Draft content may use provisional IDs and may contain unbound outcome language. Canon-Checked content has been compared with the current setting, location roster, character roster, and existing narrative corpus. Schema-Checked content satisfies the current data shape or has an explicitly approved schema proposal. Consumer-Verified content has a named current reader in source; a filename or plan reference is not a consumer. Integration-Ready content additionally has current ownership, a route into play, persistence analysis where needed, and a focused acceptance list. Released content has passed the project's active review and integrity pipeline. Deprecated content remains readable where a saved reference requires it. Archived content is a documentation record and is not eligible for runtime loading.

The lifecycle prevents a polished proposal from being mistaken for an integrated feature. Each review transition should link the evidence that justified it: source path and API for a consumer; canonical content ID for a reference; validator result for a data claim; and current ownership record for implementation work. If evidence becomes stale, lower the review state and re-audit the seam rather than silently carrying an old approval forward.

### Field ownership by content layer

| Layer | Permitted responsibility | Prohibited responsibility |
|---|---|---|
| Authored definition | Stable identity, authored intent, baseline text, explicit prerequisites, links to existing owners. | Mutable campaign truth, current inventory, inferred discoveries, or an unsourced success claim. |
| Generated candidate | A deterministic choice among approved templates and explicitly variable fields. | New durable identities, arbitrary quest completion, hidden resource grants, or permanent location unlocks. |
| Runtime projection | Current display-ready combination of definition and owner facts. | Becoming an alternate catalog, save record, simulation, or long-lived cache of mutable facts. |
| Player save | State captured by the existing canonical owner and stable references needed to restore it. | Copies of prose, duplicated map/quest/faction state, or transient presentation snapshots. |
| Editorial metadata | Provenance, review state, references, and change notes for maintainers. | Runtime gameplay authority unless a current consumer explicitly requires a field. |

The same field name may appear at different layers only when its meaning and owner are explicit. For example, a location definition can say that a door is usually sealed; the map or location owner reports whether this campaign's door is open. A dialogue line can describe a remembered promise; the relationship or quest owner supplies evidence that the promise was made. A generator may choose which approved description is shown; it cannot decide that the promise was kept.

### Change-control matrix

Review changes according to what they alter, not how small the text diff looks.

| Change | Required review | Save concern |
|---|---|---|
| Wording-only revision preserving meaning | Voice, localization, accessibility, and continuity review. | None if text is not copied into save state. |
| Condition or eligibility change | Owner-fact audit, reachability analysis, unknown handling, and expansion-disabled check. | Existing saves may now expose or hide a branch; define the fallback. |
| New response effect | Command-owner evidence, ordering, duplicate behavior, stale-state behavior, and save/restore outcome. | Persist through the command owner's existing section. |
| New location or character reference | Stable ID, current catalog resolution, map/roster consumer, and missing-content path. | Preserve references or supply a migration/retired-content response. |
| Authored-to-generated conversion | Permitted variation list, deterministic inputs, stable template identity, and replay review. | Persist only seed/input data required by the established owner. |
| Removal or replacement | Search saved references and active content, replacement mapping, old-save behavior, and release note. | Do not strand the saved record or reinterpret its outcome. |
| New schema field | Schema owner and loader/consumer claim, migration defaults, validator, and focused tests in the implementation package. | New data cannot imply new save ownership. |

For narrative-only changes, keep a concise editorial change note: prior meaning, new meaning, affected lines, IDs preserved, known saves affected, and reviewer. This avoids large prose histories in runtime JSON. If gameplay meaning changes, require a named owner and an integration plan before marking the bundle ready.

### Dependency contract across Plans 17–22

These six proposals form a bounded architecture chain:

1. Plan 19 defines what is permanent, generated, hybrid, or merely projected, and records source provenance.
2. Plan 17 describes a quest as authored content plus commands and state held by current quest owners.
3. Plan 18 selects a legal expedition opportunity set using the active quest/location requirements and current map authority.
4. Plan 20 packages dialogue content as bounded graphs with stable node and response references.
5. Plan 21 resolves scene and response visibility from an explicit snapshot of facts read through their owners.
6. Plan 22 routes selected consequential responses to those owners and renders only their accepted results.

The chain is directional, not a proposal for six new shared registries. A downstream package consumes stable references and typed facts from upstream owners. Plan 20 may author a quest command reference, but Plan 17 and the current quest owner decide whether it is valid. Plan 21 may evaluate a location gate, but Plan 18 and the current map/expedition owners decide whether the location is available. Plan 22 may request a relationship or faction effect only when a current owner accepts that command. If the source API is missing, the content remains unbound and uses a truthful fallback.

At each handoff, define four things: producer, consumer, stable identifier, and failure behavior. A table row without all four is a design note, not an integration contract. No plan should import another plan's projected state as if it were canonical.

### Collision control and motif review

Before approving a new pitch, search stable IDs, titles, distinctive phrases, character roles, location purposes, and the core choice/mechanic across current data and both plan/content corpora. A title change does not resolve a duplicate mechanic. Record the nearest match and state whether the new proposal is a replacement, extension, or distinct use case.

The collision review for this wave found two examples that should not be developed as fresh story concepts: “The Borrowed Gauge” overlaps the established Expansion 98 lesson-between-shifts material, and “The Spare Key” repeats a motif already used in Prose Wave 89 and moral-choice gossip content. They are withdrawn from these six plans as proposed story seeds. The Unentered Shelf remains the shared architecture fixture because it already serves Plans 17, 18, 20, 21, and 22 as a single linked example; its use here is to demonstrate data boundaries, not to claim new canon.

Do not multiply the shelf fixture into several renamed versions. Keep the clerk, nurse, mechanic, uncertain destination, and blanket allocation as one reviewable example. Later production content should either bind it to verified existing IDs or replace the whole fixture after a corpus search. The fixture's provisional status is useful precisely because it exposes unresolved references instead of disguising them as established canon.

### Release checklist for content bundles

For each release candidate, the author/editor records:

- bundle ID, revision, content class, and stable IDs;
- existing owners for quest phase, discovery, location availability, relationship, faction reputation, skills, inventory, time, and outcome;
- every generated field and the deterministic input that selects it;
- every external reference and the behavior when it is absent;
- map-visibility and expedition-eligibility expectations, reviewed with Plan 18;
- dialogue conditions and their fact owners, reviewed with Plan 21;
- each response effect and its command owner, reviewed with Plan 22;
- prose variants, localization keys, speaker voice rationale, and text-only fallbacks;
- expansion-disabled behavior, save compatibility, and replacement/deprecation route;
- duplicate/canon search terms and the nearest similar existing material;
- focused implementation claims and verification evidence, when promoted.

Reject a bundle that relies on an implied consumer, ambiguous ownership, or “the generator will figure it out.” A manually authored content package can still be incomplete if no current path reads it. A technically valid JSON row can still be unreachable. Conversely, a useful story proposal may remain in Draft while an implementation seam is audited; it must simply avoid claiming current functionality.

## Continuation pass 5 acceptance

The content package is ready for a new integration claim when its review state is evidence-backed, field ownership is clear at definition/candidate/projection/save layers, cross-plan handoffs name a producer and consumer, changed meaning triggers the right save and reachability review, and collision review distinguishes reuse from duplication. The two withdrawn story seeds remain closed unless a future pitch demonstrates a materially different purpose and passes a fresh canon search.

## Continuation pass 6 — content bundle contract, reproducibility, and retirement policy

### Bundle manifest as a review artifact

Group related authored definitions into an editorial bundle that can be reviewed and validated as one playable promise. A bundle may include a quest definition, location references, dialogue graph, conditional text variants, and asset/localization references. It is a packaging and review boundary; it must not become another runtime catalog or registry when the current JSON data authority already loads the individual content types.

The proposed manifest records:

- stable bundle ID and revision;
- content class for each row: authored, generated template, hybrid, or projection-only;
- stable definition IDs and expected current catalog;
- dependencies on other stable IDs and whether each is required or optional;
- owner fact families read by conditions;
- command kinds and owner IDs requested by responses;
- location eligibility, discovery, and map visibility assumptions;
- deterministic variation fields and the approved seed/input source;
- localization keys and asset references;
- base-game versus expansion classification;
- schema revision and migration/deprecation notes;
- review status, evidence links, and unresolved owner questions.

Do not put speculative script names or unverified API contracts in a manifest that will be read at runtime. Editorial notes may document a proposed consumer, but runtime data must not treat a note as an implementation binding. If current loaders cannot read a manifest, keep it as a planning artifact until a separate, bounded implementation claim creates a real consumer.

### Definition contract versus instance observation

Separate the authored definition, generated choice, observed fact, and player-specific result:

| Record | Example in the shared fixture | Owner |
|---|---|---|
| Definition | The scene contains a record-check conversation and three possible resolution approaches. | Authored content bundle. |
| Generated choice | A seeded weather/ambient text variant is selected from an allowed list. | Existing deterministic generation/session owner. |
| Observed fact | The nurse reported a promise but did not identify the destination. | Quest/evidence or memory owner after a real interaction. |
| Player result | The player accepted an unresolved hold, and the quest owner confirmed the outcome. | Existing quest/consequence owners. |

The line “I remember a promise” is not an observation unless an interaction or record has produced that evidence. A generated sentence about a blocked route is not a world fact. A graph edge labelled “deliver” is not a completed transfer. A bundle can link all these pieces, but only the canonical owners can transform an authored opportunity into a campaign result.

### Reproducibility envelope for generated content

When a content layer varies, define its envelope before writing multiple variants:

1. **Anchor:** the stable authored location, quest, speaker, or encounter that gives the variation meaning.
2. **Allowed fields:** a short list such as arrival text, noncritical detail, weather description, or an optional clue phrasing.
3. **Forbidden fields:** identity, objective truth, reward, relationship, unlock, deadline, inventory, or completion unless a current owner explicitly owns that variation.
4. **Inputs:** stable seed stream and a bounded set of canonical context facts.
5. **Eligibility:** requirements under which each variant may appear.
6. **Fallback:** neutral authored text if a variant is absent or invalid.
7. **Replay behavior:** whether the same input must reproduce the same choice or whether a new dispatch intentionally uses a later deterministic draw.
8. **Persistence:** the smallest stable seed/index/state required by the existing owner, if any.

Do not include localized display names in seed derivation or stable identity. Normalize candidate ordering before draw. If a generated choice is purely cosmetic, prefer recomputation when reopening the scene; if it carries evidence or gameplay implications, it is no longer merely cosmetic and needs explicit owner state.

### Validation layers and failure isolation

Validation should catch distinct classes of failure so authors know what to repair:

| Layer | Checks | Example diagnostic |
|---|---|---|
| Syntax/schema | Required fields, types, enum values, ID shape, supported schema version. | Bundle/row/field and expected type. |
| Reference | Quest, location, character, faction, item, node, response, localization, and asset IDs resolve. | Missing stable ID and required/optional classification. |
| Owner compatibility | Predicate and command families have a verified current consumer. | Unsupported fact family or command type. |
| Reachability | Entry, nodes, quest route, required location, and resolution can be reached under some valid fixture. | Response unreachable under all declared fixtures. |
| Progression safety | Critical base route works with optional expansion/actor/faction content absent. | Sole required route depends on optional bundle. |
| Determinism | Variant inputs are stable; no hidden source of nondeterminism. | Unordered candidate list or unsupported random source. |
| Save compatibility | Referenced IDs remain resolvable, and retirement has a migration/fallback. | Saved ID has no replacement or safe orphan behavior. |
| Prose quality | Canon voice, factual truth, accessibility, localization, duplicate/motif review. | Unowned operational claim or repeated story function. |

An error at one layer should not be “fixed” by weakening an unrelated layer. For example, if a character reference is optional, do not allow a missing required location through validation. If an old-save ID is unresolved, do not silently choose the first matching display name.

### Release variants and expansion disablement

Build a release matrix for base content and each optional expansion bundle. For every quest or dialogue route, record whether the row is required for onboarding, critical progression, optional depth, or cross-expansion callback. When an expansion is disabled, all required base references must still resolve. A base quest may offer a different line or clue if an expansion character is missing, but it cannot keep a command that targets a definition absent from the build.

When expansions depend on one another, declare that dependency in the content graph and reject invalid combinations at validation time. Do not let a Plan 20 scene require a character introduced only in an unrelated optional bundle unless the owning quest explicitly has a fallback. Do not duplicate stable location or faction identities across bundles to make the content appear self-contained.

### Retirement, supersession, and old saves

Retirement is a controlled content state, not deletion by prose edit:

1. Stop creating new instances/offerings from the retired definition.
2. Keep the stable ID resolvable for any saved reference that may exist.
3. Determine whether active instances can finish, migrate, or close through a neutral recovery path.
4. Map the definition to a successor only when the successor has equivalent gameplay semantics.
5. Preserve the original outcome in history if meaning changes.
6. Include a focused save fixture for each supported old state.
7. Remove the compatibility stub only when current save/version policy proves no supported record can refer to it.

Supersession differs from renaming. A prose title can change while an ID remains stable if meaning, commands, and outcomes are unchanged. If the quest's promise, failure conditions, reward, or destination semantics change, use an explicit revision/migration decision. Content authors should not edit a stable ID merely to clear a duplicate-title warning.

### Collision-review procedure

For a new bundle, search at least five dimensions: exact title and IDs; distinctive wording; setting and location function; player verb/loop; and moral or systemic choice. Then compare against integrated data, all active plans, recently completed plans, partial placeholders, and content that may be mid-authoring. Record the nearest matches in the bundle note and explain whether the work is a correction, continuation, variant, or genuinely separate experience.

If a fresh story pitch collides, preserve reusable architecture while withdrawing or reframing the content. The Unentered Shelf stays the single cross-plan architecture fixture for this wave. Its reuse must not be mistaken for six independently integrated content packages, nor should a future author produce a second mislabeled-supplies plot merely to populate each plan. The retired Borrowed Gauge and Spare Key concepts remain excluded from this bundle absent new evidence of a distinct narrative purpose.

### Production sizing without volume inflation

Estimate content production by deliverables rather than word count alone: number of unique scenes, nodes, response edges, gated variants, languages/localization keys, speaker voices, locations, quest objectives, command types, asset references, and save fixtures. A 300-word scene with six condition paths can cost more to implement and QA than a 900-word linear conversation. Keep a budget for authoring, schema validation, continuity, accessibility, and implementation review.

Use a smallest-complete slice to price the system before populating a large library. The slice should contain one required and one optional reference, one generated cosmetic variation, one context-gated response, one owner-routed consequence, and one missing-content fallback. Estimate the next content wave from actual integration cost instead of multiplying a word target across every plan.

## Continuation pass 6 acceptance

The bundle contract is ready when its manifest remains an editorial/review boundary unless a live reader is proven, generated variation has a strict reproducibility envelope, validation reports reference/owner/reachability/save/prose defects independently, base routes survive expansion disablement, and retirement preserves supported old references without inventing replacement semantics.

## Continuation pass 7 — field authority map, release gates, and content change rehearsal

### Field-level authority map

The authored/generated boundary becomes practical when writers and implementers can classify each field rather than labeling an entire file “dynamic.” Apply this map before adding or changing data:

| Field meaning | Authored definition | Generated selection | Runtime projection | Persisted state |
|---|---|---|---|---|
| Stable location identity | Yes: permanent canonical ID. | No new identity; may select one approved ID. | Reads current location data. | Existing map/expedition owner only. |
| Display title and baseline description | Yes, localization-backed. | May select a bounded approved variant. | Chooses current text from canonical facts. | Do not store rendered copy. |
| Route requirement | Authored eligibility/rule reference. | May choose among legal pre-authored routes. | Reads current route/lock facts. | Route owner if it owns durable route changes. |
| Quest objective | Authored objective intent and evidence kind. | May choose an approved objective module only if current owner supports it. | Shows owner-backed current progress. | Quest owner. |
| Item/resource requirement | Authored stable item/resource IDs and amount. | May select from a bounded compatible set if designed. | Reads current inventory/resource state. | Inventory/resource owner. |
| Relationship/reputation gate | Authored predicate reference. | Does not invent standing. | Evaluates a fresh owner fact. | Relationship/faction owner. |
| Dialogue branch text | Authored node/response keys. | May select an approved line variant. | Renders from scene snapshot and result. | No text copy; only underlying outcome by its owner. |
| Weather/ambient detail | Authored variant set. | Deterministic selection permitted within the set. | Displays a short-lived selected description. | Persist only if an existing owner needs it for replay. |
| Discovered/visited/completed | Never set by text alone. | Cannot be inferred from selection. | Reads map/quest/expedition owners. | Current state owner. |
| Reward grant | Authored reward request/reference. | No generated quantity unless a current economy owner defines the rule. | Displays accepted result. | Rewarding inventory/resource/quest owner. |
| Character memory | Authored eligible reactions. | May select a line based on a supported memory fact. | Reads relationship/memory source. | Existing memory/relationship owner. |

Fields that cannot be classified stay out of implementation. This is especially important for broad JSON records that mix permanent definition facts, encounter choices, and campaign outcomes. Split authority at the owning data contract only when current loader evidence supports it; do not split one consumer into parallel catalogs merely for authoring convenience.

### Review-state transition gates

Each content packet advances through evidence gates:

**Draft → Canon checked:** the author has searched stable IDs, titles, motifs, similar mechanics, character roles, and location function; any overlap has a written decision.

**Canon checked → Schema checked:** every field maps to an existing content schema or is marked as a proposed schema change that requires an owner decision. Examples include whether “blocked” is a presentation label or a persisted state.

**Schema checked → Consumer verified:** each required row is read by a current loader and reachable through an identified runtime route. A validator that accepts a row without a gameplay consumer does not satisfy this gate.

**Consumer verified → Route verified:** the content has entry, objective, fallback, outcome, revisit, and disabled-expansion paths with owner-backed evidence.

**Route verified → Integration ready:** exact implementation files are claimed, data/save/test impacts are explicit, and acceptance can be focused.

**Integration ready → Released:** the responsible integrator has recorded implementation and verification evidence. Planning authors cannot promote a package to Released by updating its own status.

When any gate fails, return to the nearest relevant earlier state. A stale source reference sends the packet back to Consumer verified. A story collision sends it back to Canon checked. A field with no persistence owner remains Draft or Schema checked. This makes status informative instead of ceremonial.

### Data evolution rehearsal

Before changing a stable record, perform a dry rehearsal in prose:

1. Find all current references to the stable ID in catalogs, authored graphs, plans, saves/fixtures, and integration claims.
2. State whether the revision is wording-only, compatible behavior, or meaning-changing.
3. Identify active quests and old saves that may reference the previous definition.
4. Choose preserve ID, replacement mapping, supported deprecation, or migration request.
5. Confirm optional bundles do not supply a required replacement for the base route.
6. Define a neutral fallback for missing localization or optional variant data.
7. Specify which validator and focused save fixture would prove the change after implementation.

For wording-only changes, IDs can remain stable if the line does not alter player knowledge or promise. If wording changes what the player can reasonably infer, treat it as a narrative meaning change and review affected choices. “You may find a record” and “the record is at the depot” create different map promises even when only a short sentence changed.

### Generated-content regression cases

Any authored/generated bundle that makes a claim about the world should include cases for:

- same seed and same owner snapshot yield the same variant;
- reordering catalog files does not alter candidate eligibility or result;
- adding an optional variant does not change objective truth or reward;
- removing one variant selects a neutral valid fallback;
- generated text cannot create a location marker or discovery event;
- restored saves query canonical state rather than replaying stale rendered text;
- expansion-disabled builds resolve every required base reference;
- deprecation preserves supported stable IDs and old outcomes;
- localized names do not influence identity, seed, or command target;
- an optional generated candidate cannot displace a mandatory active quest site.

The content owner should review the authored variant envelope and a systems owner should review any gameplay-related field. If no gameplay field changes, the proposal can remain content-only after current loader and data-integrity checks prove reachability.

### Editorial truth and evidence language

Mark copy in an authoring review as **confirmed**, **reported**, **inferred**, **rumored**, or **unknown**. This is an editorial truth label, not a runtime knowledge system. It helps authors avoid turning testimony into objective fact or a map clue into a discovered site:

- Confirmed copy is supported by an owner or completed interaction.
- Reported copy attributes a statement to its speaker/source.
- Inferred copy makes the speaker's reasoning visible without claiming certainty.
- Rumored copy remains a lead and must not promise exact access.
- Unknown copy names what is not established and preserves a valid next action.

If runtime requires players to distinguish these states, bind each label to existing quest/map evidence; do not serialize the editorial label as another campaign truth. A character can say “I think the road still reaches the old depot” while the map remains unconfirmed. The copy is truthful because it reports an inference, not because the location is guaranteed.

### Bundle handoff record

At integration handoff, include a concise record containing bundle revision; validated IDs; current readers; exact owner commands and predicates; unsupported fields; expansion dependency matrix; old-save behavior; collision search terms; focused verification evidence; and remaining prose/localization work. That record goes to the existing project handoff authority. It is not a competing catalog status file.

The review is complete only when another person can answer: Which definitions are permanent? Which fields may vary? Which owner confirms each player-visible result? What breaks when this bundle is absent? What happens to an old reference? If the answer depends on reading the author's intent, the handoff remains incomplete.

## Continuation pass 7 acceptance

The field and release package is ready when every mutable-looking field has a canonical owner, review status advances only on evidence, meaning-changing revisions preserve old saves or declare a migration, generated variants remain within an approved envelope, and prose truth labels are checked against player-facing claims.

### Asset and localization references

An authored bundle may reference portraits, ambient audio, map art, icons, or scene illustrations, but it does not become the asset registry. Use the current Godot-native asset registry and import policy; never point new content at retired Unity-era structures. A missing optional portrait must not hide a speaker's text or make a scene unreachable. A missing required map/interaction asset is a release defect if the mechanic depends on it.

Store stable localization keys separately from translated text where the current content pipeline expects that shape. Do not use localized labels as IDs, asset paths, sort order, or random seeds. For generated variants, each language needs a semantically equivalent approved option or a valid neutral fallback; a selector should not pick a variant only the source language supports.

Asset and text review should share the bundle's stable references but report separate diagnostics. This makes it possible to fix a missing import without changing quest eligibility, or to fix a broken localization key without changing the location's canonical identity.

### Content change impact note

For each released revision, record which quest/scene/location IDs changed and whether the change affects information, eligibility, consequence, accessibility, or only wording. Identify the nearest integration owner and the smallest focused review needed. A prose-only note does not authorize a gameplay migration; a gameplay effect cannot be shipped under a “copy refresh” label.

## Continuation pass 8 — content package profiles, authorship controls, and scale policy

### Permanent, generated, hybrid, and temporary profiles

Classify content according to identity and lifespan:

**Permanent authored profile:** a stable location, character, quest, faction, scene, or item definition. It has a stable ID, reviewed references, and a declared consumer. Campaign outcomes associated with it live in their current owners.

**Generated selection profile:** a deterministic choice among authored definitions or variants. It has an approved candidate set, stable ordering, a seeded source, explicit constraints, and no power to create a permanent ID. Its result may be transient or persisted only through a verified existing owner.

**Hybrid profile:** a permanent authored anchor with selected, bounded variation. The author names every field that may vary and every gameplay field that cannot. A weather line can vary while the location's identity, route rule, quest objective, and reward remain fixed.

**Temporary profile:** a definition available for a declared interval/event or campaign condition. The owner of that interval controls eligibility and expiry. Once expired, a known instance can present an unavailable/closed state; it does not silently reopen because a dialogue graph still references it.

**Projection-only profile:** a read-time combination of stable text and owner facts. It is not saved. It may be cached for one screen/frame where needed but is discarded or invalidated according to the current host lifecycle.

Do not label content “generated” simply because it is selected at runtime. Authored random tables still have permanent IDs and constraints; generated state still needs deterministic ownership.

### Data package profiles by gameplay responsibility

Organize review around responsibility rather than proposed directory names:

- Location package: stable place identity, authored description, map/expedition references, eligibility assumptions, clue and retirement behavior.
- Quest package: stable quest definition, objective evidence, required/optional location links, outcomes, condition references, reward requests.
- Dialogue package: scene/node/response IDs, text/localization keys, owner facts, command references, explicit graph exits.
- Character package: stable actor identity and voice/lore references, while relationship and memory state remain elsewhere.
- Faction package: stable group identity and authored access/consequence intent, while current standing/access remains with its owner.
- Generated-variant package: anchor, allowed fields, candidate IDs, deterministic inputs, exclusions, neutral fallback.

These are review packages and may map onto existing JSON catalogs in more than one way. Do not create one mega-file or new registry until the actual loader and data-authority path are verified. Do not duplicate a location's name, state, or discoverability in each quest that uses it.

### Authoring controls for variation

For every selectable variant, list:

1. the fixed anchor and purpose;
2. allowed variation fields with types and bounds;
3. candidate count and deterministic ordering;
4. facts that constrain eligibility;
5. fields that must remain identical across all variants;
6. fallback if one or all variants are unavailable;
7. whether selection is stable per campaign, per expedition, per scene, or per render;
8. evidence that the current generation owner supplies the correct seed.

“Per render” should almost always be presentation-only; otherwise opening a panel can change the world. “Per expedition” requires stable behavior throughout the planning/start transaction. “Per campaign” requires a save owner and restore evidence. If the requested cadence cannot be supplied by the current deterministic owner, hold the variant rather than deriving a new random source in content code.

### Canon and continuity review at scale

Large content batches need an index of stable IDs, titles, character roles, locations, themes, player verbs, outcomes, and unique phrases. Use the index as a search aid, not as canon authority. Canon remains in current source data and approved narratives. Before a batch is promoted, search across completed content, active plans, partial placeholders, and any material explicitly flagged as being written. If a broad motif is common—lost supplies, a damaged route, a missing record—compare the dramatic question and resolution mechanic, not only nouns.

For each intentional reuse, mark one of:

- **Shared fixture:** used to demonstrate contracts across plans; not a promise of multiple released quests.
- **Mechanic template:** same structure, fresh character problem and prose.
- **Direct continuation:** same canon event/characters and a declared dependency.
- **Variant:** same location or mechanic with a different supported context.
- **Collision:** too similar to release; rewrite or withdraw.

This categorization prevents the same Unentered Shelf fixture from being counted as six independent narrative additions and prevents a copied template from being treated as a new storyline.

### Content analytics without runtime telemetry assumptions

For planning, track authoring metrics in the editorial review: unique reachable nodes, choice outcomes, condition families, unresolved references, fallback branches, string count, asset needs, estimated localization work, and QA fixtures. Do not claim the runtime will collect player behavior unless a current telemetry owner exists. If later balancing needs outcome rates, make a separate privacy/product decision and use its approved infrastructure.

Metrics should diagnose production complexity rather than reward volume. A large word count is not evidence of high quest coverage. A useful content batch adds distinct player verbs, supported outcomes, readable fallback routes, and canon-compatible situations with low unresolved-reference count.

### Expansion compatibility checklist

For each optional package combination, verify:

- every required stable ID resolves;
- optional characters have a baseline speaker/record route where required;
- base-game quests do not depend on expansion-only conditions;
- expansion-only rewards are not referenced by base-game command targets;
- hidden locations remain undisclosed when the package is absent;
- old saves with a now-disabled bundle have a declared neutral or retired-content result;
- localization and asset fallbacks do not create invisible required actions;
- generated candidate sets remain deterministic when optional rows are removed.

If two expansion packages conflict, declare the dependency and reject the unsupported combination during validation. Do not allow runtime selection to resolve a missing dependency by picking a similarly named definition.

## Continuation pass 8 acceptance

The package profile is ready when permanence/lifespan classification is explicit, each content responsibility maps to an existing catalog/owner rather than a speculative registry, variant cadence and seed source are bounded, intentional reuse is distinguished from duplicate narrative, and optional-package removal has a complete fallback story.

### Content-load boundary and malformed data behavior

Treat content validation and runtime fallback as separate responsibilities. The data-integrity pipeline should catch malformed authored definitions, duplicate IDs, invalid references, unsupported schema versions, disallowed generated fields, missing required localization, and commands/predicates without current consumers before release. Runtime should still fail safely if an older save or optional bundle supplies an absent reference.

When optional content is malformed, isolate the optional bundle where the current loader supports that policy; do not permit a partially loaded quest to reference half of its required graph. When core content is malformed, report a clear integrity failure rather than silently substituting unrelated gameplay data. A neutral line fallback is appropriate for missing presentation copy; it is not a substitute for a missing quest objective or reward owner.

If data is loaded more than once, use the existing loader/caching lifecycle. Do not make each dialogue panel parse its own catalogs or retain a separate mutable dictionary of active content. Any cache remains disposable, derived from the JSON authority, and invalidated through the owning lifecycle.

### Authoring migration and schema-version discipline

Every proposed schema change should identify its consumer, default behavior for existing rows, migration source, invalid-value response, and removal plan for any temporary compatibility field. Adding a field without a reader is not progress. Renaming an ID-bearing field can break saved references even when the prose remains intact.

Keep field naming, ID casing, localization references, and schema-version conventions aligned with the current data authority. If a legacy field is supported for old content, document precedence when both old and new forms appear. Reject conflicting values during validation instead of choosing one by file order. Generated content should always reference the schema version of its authored anchor; it should not synthesize a new schema shape.

### Content bundle rollback after release

If a release bundle has to be withdrawn, first identify accepted live quest instances and saved references. Remove the row from new eligibility while preserving old-ID resolution. A broken cosmetic variant can fall back to neutral text; an unavailable required location may need a replacement clue or explicit delayed route; a removed command needs a safe route through its owner, not a dead graph edge. Keep a short release note naming the affected IDs and outcomes so future continuity review can distinguish intentional retirement from accidental disappearance.

### Reference lifecycle from draft through restore

Track a required reference through the same lifecycle as its content: authored ID introduced; schema validated; consumer found; route made reachable; accepted instance created by its owner; save captures the stable ID where needed; restore resolves that ID; later deprecation retains a fallback. A broken link at any stage has a different repair. Draft-time missing IDs are authoring gaps; release-time missing IDs are integrity defects; restore-time missing IDs are migration defects.

Review optional references separately. A missing optional speaker can choose a neutral scene, while a missing required quest target must block release or invoke a declared replacement. Treating both as “skip missing data” can silently remove critical content.

## Continuation pass 9 — provenance walk-through, field inheritance, and release rehearsal

### Provenance walk-through for a quest clue

Use a single clue to demonstrate the difference between authoring, generation, discovery, and consequence:

1. The authored location definition establishes a fixed room, its canonical ID, and the fact that a notice board exists there.
2. The authored quest definition says that inspecting the board can produce a route clue with a named evidence kind.
3. A generated cosmetic variant may describe rain, dust, or a damaged corner, selected from an approved deterministic list.
4. The interaction/discovery owner confirms that the player inspected the notice and records what the source actually states.
5. The quest owner accepts that evidence only if it matches the objective.
6. The map owner reveals exact or approximate location knowledge only if the inspected source supports it.
7. The dialogue graph presents follow-up questions from that state; it does not create the clue itself.
8. If the player requests an expedition, Plan 18 resolves a legal opportunity and the current expedition owner validates dispatch.

At no point does the weather variant, scene title, or generated description change what the notice proves. If the notice row is removed from a future release, the content bundle must preserve old references or provide a replacement clue with equivalent evidence. If no equivalent exists, the quest is delayed or migrated rather than silently completed.

### Field inheritance across content layers

When several definitions share a concept, use references instead of copied authority:

| Shared concept | Authored source | Reference consumers | Values that should not be copied |
|---|---|---|---|
| Location identity | Canonical location/expedition definition. | Quest requirement, dialogue scene, map marker, encounter. | Name, route state, discovery state, visit state. |
| Character identity | Current actor/character roster. | Quest giver, dialogue speaker, memory event, faction contact. | Relationship, presence, memory, current mood. |
| Quest intent | Canonical quest definition/owner. | Journal, dialogue, map opportunity, reward request. | Active state, objective progress, terminal result. |
| Item/resource meaning | Existing item/resource catalog and owner. | Objective requirement, transfer command, reward display. | Possessed quantity, stock, reservation result. |
| Faction identity | Current faction catalog/owner. | Briefing, location access, quest branch. | Standing, alliance/access, active-war status. |
| Text identity | Localization authority for the content row. | Graph node, transcript, journal, map. | Current rendered text in save state. |

If the current loader requires a repeated display field, document it as a presentation mirror and define which source supplies the authoritative value. Never use the mirror to decide gameplay behavior.

### Release rehearsal: change one definition

Before a new bundle becomes integration-ready, rehearse a small revision:

**Revision A:** Add a second arrival-text variant. Confirm that only presentation changes; stable location, route, evidence, and save IDs remain fixed.

**Revision B:** Correct a clue's meaning so it says “a request was made” rather than “delivery occurred.” Treat this as a gameplay/narrative meaning change; review quest evidence, dialogue branches, journal copy, and any completion path.

**Revision C:** Retire a temporary site. Stop new selection after its owner-defined interval, preserve the old ID for saved references, and provide a factual closed/unknown route response.

The rehearsal demonstrates that editorial edits, compatible data changes, and semantic changes have different review paths. A small diff can change progression; a large prose edit may not.

### Bundle dependency graph

Track required and optional references as a directed graph. Required edges must resolve in every supported build. Optional edges can be absent only when a declared fallback exists. Use cycle detection for quest prerequisites and dialogue follow-ups; intentional loops must have a bounded exit and owner-backed eligibility. When content A points to B and B points back to A, that does not automatically make either content playable.

For a bundle spanning Plans 17–22, the required-edge chain is: quest reference to evidence/location; location to current expedition definition; scene to existing speaker; condition to current fact owner; response to command owner; result key to actual accepted/rejected state. Break the chain in each test fixture to verify the correct layer reports the problem. Avoid one generic “content invalid” error that hides whether the issue is a missing ID, absent API, unreachable graph, or unsupported effect.

### Release audit record at useful scale

For a multi-quest expansion packet, summarize coverage by stable ID and dependency rather than copying every line into the audit. Report count of permanent definitions, generated variants, hybrid anchors, temporary rows, required/optional cross-references, optional bundle dependencies, old-save references, unresolved consumer contracts, and duplicate story-function warnings. Link exact validation reports from the owning generator when one exists.

The release auditor should be able to select one example from each content class and trace it end to end. A representative sample does not replace row validation, but it can reveal systemic mistakes such as every generated variant claiming the same route effect or all optional speakers being required by core quests.

### Revision rehearsal: distinguish copy edits from contract changes

Identify the semantic class of every revision before it enters a package. A spelling correction is presentation-only. A change in what a clue proves, who may speak, whether a location is critical, or which response sends a command changes the contract even if one JSON field changes. Record stable IDs, consumers to revalidate, and whether old saves can resolve the previous reference.

| Revision | Classification | Required review | Old-reference behavior |
|---|---|---|---|
| Correct punctuation or translation | Presentation-only. | Text length, tone, localization, transcript. | Keep identity when meaning is unchanged. |
| Add an approved cosmetic variant | Compatible content addition. | Deterministic selection, fallback, no effect leakage. | Existing saves can resolve the base row. |
| Replace speaker or location | Dependency change. | Catalog, availability, fallback. | Preserve a legacy row or supported alias/migration. |
| Change rumor into confirmed evidence | Semantic change. | Quest, map, dialogue gates, journal, fixtures. | Version the meaning; do not reuse its evidence ID. |
| Make optional content a prerequisite | Progression change. | Graph, package-off behavior, critical path, old saves. | Requires approval and an accessible fallback. |
| Retire a generation template | Generator compatibility change. | Candidate eligibility, accepted instances, replay/save. | Existing accepted instances remain answerable. |
| Split or merge graph nodes | Identity change. | Links, transcript history, reachability, duplicate commands. | Preserve a supported historical mapping. |

For semantic edits, include a before/after statement of player-observable truth. “Text updated” is insufficient when “I saw the box leave” changes to “I heard that it left.” That edit may change evidence class and quest reachability while sounding similar. The package needs only a concise revision note and links to affected IDs; avoid copying all prose or schema into a second authority document.

### Compatibility behavior by save and package combination

Review supported combinations: a new campaign with the full package; a new campaign with the optional package disabled; an existing campaign whose accepted quest points to a current row; and an existing campaign whose accepted quest points to a retired or revised row. Add combinations only when a release supports them. This catches cases where an editor preview succeeds but an old instance cannot reconstruct its dialogue, location, or evidence path.

For each combination, name the owning response. A missing optional package can use a declared core fallback or make its optional opportunity unavailable. An unresolved required reference blocks release validation. An accepted instance remains answerable even when its template is no longer offered. A retired location may provide a closed-site response, canonical substitute, or delayed step, but cannot create a fabricated visit fact. If no migration is supported, retain the legacy row until the save owner establishes another route.

Generated content needs the distinction between a template's lifetime and an accepted instance's lifetime. New offers use the current eligible template set. An accepted instance retains its stable reference and owner-supported progress even when that template is retired from new offers. Do not regenerate the accepted instance from today's seed or overwrite its branch with the newest template text. For a missing field in an old save, use a documented default only when meaning is preserved; otherwise defer through the current owner and present neutral recovery copy.

### Content release stop conditions

Hold a bundle when a required edge points outside the build, one stable ID has competing meanings, a cosmetic generator changes gameplay truth, a retired target has no restore behavior, or a critical quest depends on a secret or rare selection. Hold it when an accepted old instance can send a command no current owner recognizes. These are integrity failures, not reasons to add a generic catch-all node.

The stop report lists content IDs, edge type, first consumer that cannot resolve them, affected package/save condition, and smallest safe remediation: fix the reference, declare an optional dependency with fallback, retain a legacy row, or delay promotion. This report belongs to the existing content review and governance path. This plan authorizes no schema migration or package release.

### Separation audit for generated content

For every generated field, label it authored input, deterministic selection, current owner fact, or rendered presentation. Authored input constrains what may be selected; deterministic selection chooses among approved variants; owner facts record what happened; presentation describes those facts. If an output cannot be assigned to exactly one role, the content is likely mixing authority. For example, a generated weather phrase can decorate a location; it cannot decide that a path is blocked unless an existing environment/route owner reports that state.

Use a simple mutation audit: hold all canonical owner facts and seed constant, then vary only the cosmetic stream. Quest status, map knowledge, available dialogue commands, faction access, inventory, and expedition readiness must remain identical. Next, hold presentation constant and change one owner fact; only the explicitly dependent consumers should update. This paired review makes accidental coupling visible without requiring a broad simulation or a new cache.

The same audit applies to authored prose. A note can state that the writer saw a cart near the east door; it cannot mark the door discovered unless the interaction owner records discovery. A faction bulletin can describe an offer; it cannot grant faction standing. A shelter ledger can present a count; it cannot change stock. A quest definition can request a proof; it cannot claim that proof occurred. These boundaries let writers add density and voice without making content files competing gameplay stores.

### Authorship boundary closeout

Close package review with an ownership map of immutable authored inputs, generated presentation choices, canonical owner facts, and derived read models. For every high-risk field, identify its producer and consumers and define behavior when the field is missing, retired, or unavailable in an optional package. A field that cannot be traced remains a proposal, not an implicit implementation requirement. This lets content scale through validated rows and approved variants while existing catalogs, owners, save sections, and event paths remain the single source of gameplay truth.

## Continuation pass 10 — content production pipeline, review evidence, and batch governance

### A release pipeline with explicit evidence at each handoff

Move a content bundle through named review stages that say what has been proven. These labels describe editorial/integration evidence; they are not runtime states and should not be added to a saved campaign model.

| Stage | Work performed | Evidence required to advance |
|---|---|---|
| Scope request | State player value, target systems, core/expansion placement, and non-goals. | Owner map and dependency list; no reserved IDs without approval. |
| Canon review | Compare characters, events, places, chronology, and story function with current narrative authority. | Continuity notes, collision search, provisional/canon labels. |
| Structured draft | Fill the current supported data shape and stable-reference rules. | Required fields present; IDs and localization keys follow authority conventions. |
| Schema/integrity review | Validate syntax, schema, ranges, references, and generated-field restrictions. | Current catalog/integrity report with actionable row-level errors. |
| Consumer review | Trace each field to a real owner, UI projection, or non-runtime editorial note. | Evidence of a consumer or explicit disposition as unused proposal. |
| Playability review | Walk the offer, selection, dialogue, command, result, and return paths. | Reachable route table and at least one failure/unknown case per behavior class. |
| Bundle review | Enable/disable optional pieces and resolve cross-package edges. | Required dependencies close; optional edges have safe fallbacks. |
| Release candidate | Freeze stable IDs and compare changes since the reviewed revision. | Focused validation, localization review, save compatibility result, and handoff. |
| Released/deprecated | Retain compatibility definitions or approved migrations as references age. | Release note, old-save behavior, and new-offer eligibility policy. |

The stages can be light for a small text-only addition, but each required proof still has an owner. A two-line callback can skip a new gameplay command review if it truly has no effects; it cannot skip canon or localization review. A large region bundle should not jump from a spreadsheet concept to “ready” because the prose is complete. A review state belongs in the package checklist or governance record, not in a second mutable game catalog.

### Validator findings should point to the smallest repair

Keep validation feedback categorized so authors and integrators know whether they can fix a row locally or need an owner decision:

| Finding class | Example | Release effect | Primary next step |
|---|---|---|---|
| Structural error | Malformed JSON, duplicate ID, invalid required field. | Blocks loading/release. | Correct the row and rerun the owning validator. |
| Referential error | Missing character, location, quest, text, or effect ID. | Blocks a required edge; optional edge needs a declared fallback. | Repair the reference or revise dependency classification. |
| Semantic contract error | Variant field can change an owner fact or a required objective has no evidence producer. | Blocks the affected content function. | Redesign the content contract or obtain the required owner decision. |
| Reachability error | Node is unreachable, loop has no exit, or critical task depends on a hidden-only route. | Blocks playability. | Repair graph/availability and verify the baseline path. |
| Compatibility error | Old accepted instance resolves to a retired or changed reference without a path. | Blocks release for supported saves. | Retain a legacy row or provide an owner-approved migration. |
| Quality warning | Sparse optional pool, repeated story function, unusually high branch count. | Review decision; not automatically a loader failure. | Improve content variety or record an accepted limitation. |
| Editorial note | Voice, uncertainty, chronology, or translation context needs attention. | Blocks narrative sign-off where meaning is unclear. | Revise copy or document intentional ambiguity. |

Reports should name file/row/content ID, reference path, consumer, and a reason code. A single “bad content” count is not enough for a 200-row wave. Conversely, do not report every optional line omission as an error. Treat absent required fields and absent optional content differently in both tooling and release review.

### Review high-volume content without sampling away risk

Split large batches by shared contract, not by arbitrary row count. A group of location descriptions can receive an editorial sample review when every row already passes structural validation and no row changes gameplay. A group of objective definitions must receive row-level evidence validation, because a single invalid target may strand an accepted quest. A dialogue group with one response per faction needs consumer-by-consumer condition review, even if its graph structure is shared.

Use three complementary views. First, compare the whole batch for IDs, references, counts, distribution, and duplicate/near-duplicate text. Second, select a representative route from each semantic class and trace it through a live owner contract. Third, inspect all high-risk rows directly: critical-path references, save-bearing IDs, command-bearing responses, timed deadlines, hidden destinations, late-game outcomes, and optional bundle boundaries. Sampling is suitable for cosmetic variation; it is not a substitute for checking every persistent reference or every claim that changes player state.

When the volume exceeds reviewer capacity, split promotion into coherent bundles with complete dependencies. Do not accept half of a required graph while its companion location or result package is unfinished. Make optionality real: the base bundle can still complete its supported campaign route, and disabled optional rows do not leave dead response IDs. Keep manifests and generated reports reproducible from the owning tools rather than manually editing generated outputs.

### Change control while multiple authors contribute

Give each content batch one coordinating owner for stable-ID allocation and integration notes. Writers can work on disjoint rows, but shared schemas, indexes, quest/faction outcomes, and location references need one reconciled proposal. Before merging a batch, compare its base revision with the current authority because another change may have added a speaker, consumed an ID, retired an optional dependency, or altered a supported owner fact.

If two authors define the same idea under different IDs, treat it as a duplicate-content review, not a harmless parallel addition. If they assign different meanings to the same ID, block promotion. If one branch changes while its dependent callback is untouched, list the callback as stale and either revise it or use a neutral line. The smallest safe conflict resolution usually preserves one stable definition and redirects or removes the other while retaining compatibility for any released reference.

The handoff names changed content IDs, exact consumers, validation commands/results, old-save cases, core-versus-expansion classification, unresolved decisions, and files still owned by another package. This keeps the content packet reviewable and allows a future integrator to distinguish “written,” “validated,” “reachable,” and “released” without inferring status from word count.

### Localization and generated-copy boundary

Generated prose should select from authored, localized variants or compose only through an already-supported localization mechanism. Do not translate arbitrary generated English at runtime, concatenate untyped fragments, or build sentences whose grammar depends on unstable IDs. Every variant has a stable text key or an established parameterized localization contract, a context note, and a neutral fallback. If a locale lacks a rare optional line, the fallback preserves gameplay information without showing a raw key.

| Generated presentation choice | Allowed data | Must remain fixed |
|---|---|---|
| Weather/condition flavor | Approved variants selected through the deterministic content stream. | Route legality, hazard facts, quest proof. |
| Speaker tone | Authored line variants selected from supported context facts. | Speaker identity and command result. |
| Arrival detail | Stable authored details with localization context. | Canonical location ID and map visibility. |
| Rumor phrasing | Attributed, approved descriptions of the same uncertain report. | Whether a report is verified or objective-completing. |
| Journal summary | Current owner-backed progress labels and localized copy. | Quest state and accepted evidence. |

When using placeholders, validate parameter type, count, grammar, and missing-value behavior. A location name parameter must be the localized name from its canonical source, not an ad hoc string stored in the quest. An amount parameter must come from the resource owner and use the locale's number format. A missing speaker name should select a safe complete sentence, not leave an empty pronoun or expose an internal identifier.

### Reproducible content build record

For a release review, record the content revision, schema/catalog revision, localization revision, generator version, deterministic seed/input contract, and validator report identity. The record is not campaign save state; it makes the approved content output reproducible. If a generator's output changes after a tool update while authored inputs remain fixed, review whether the change is cosmetic or affects eligibility, IDs, or gameplay references before accepting it.

Build outputs should be generated through the owning tool and checked with its normal verification mode. Do not hand-edit a generated manifest to make a release appear complete. If a source file changes after validation, invalidate only the dependent evidence: a punctuation edit may require text/layout review; a location-ID change invalidates map/quest references and old-save compatibility; an effect-kind change invalidates owner and command tests. This keeps revalidation focused while ensuring the final package matches what was actually reviewed.

### Package inventory at a useful scale

Summarize each package by semantic counts: stable authored definitions, generated presentation variants, canonical cross-references, required and optional edges, command-bearing dialogue responses, persistent IDs, localized text keys, and old-save references. Compare those counts with the prior release and explain large shifts. A jump in row count can be harmless if it is localized flavor; one new persistent ID can be high risk even in a tiny package.

The inventory should also list content deliberately excluded from the release and why: unresolved owner API, incomplete voice/continuity, missing fallback, low-value duplicate, or localization gap. Keep excluded material in its authoring workspace without allowing it into runtime catalogs. The release handoff names the exact package boundary and supports an expansion-disabled build review where the base game remains self-contained.

## Continuation pass 11 — permanent anchors, generated instances, and content authority matrix

### Five data roles, five different lifetimes

Content should be classified by what it means and how long it lives. “JSON” or “generated” is not enough to tell whether a value is a stable definition, a selection result, or a persistent world fact. Before adding a field, place it in one of the roles below and identify its owner.

| Role | Definition | Example | Lifetime and authority |
|---|---|---|---|
| Permanent authored definition | Stable designed identity and allowed behavior. | Quest objective kind, location anchor, dialogue node, faction term text. | Ships in the authoritative authored catalog; immutable for an accepted save reference except through compatibility process. |
| Generated presentation choice | Deterministic selection among allowed authored variants. | Weather wording, incidental description, approved portrait/voice tone variant. | Recomputed per documented boundary or restored from the current owner if that choice is durable. |
| Generated content instance | A constrained instance created from an authored template. | A rotating task with selected allowed target and objective module. | Identity/progress belongs to the current quest/instance owner; template stays separate. |
| Canonical world fact | Accepted gameplay event or current system state. | Discovery, accepted transfer, route access, relationship result. | Produced and persisted by exactly one existing domain owner. |
| Derived presentation/read model | Short-lived query result assembled for a screen. | Journal row, map marker label, current dialogue choice list. | Rebuilt from owners and authored data; not a second campaign save authority. |

A field can be authored permanently yet contain an enumerated set of generated variants. That does not make its parent definition generated. A derived map summary can render a canonical location without owning its discovery. An accepted generated quest instance can persist under the quest owner without copying its template into campaign state. Use these distinctions in code review, catalog review, and save design.

### Authority matrix by content family

| Content family | Permanent authored anchor | Allowed generated dimension | Canonical owner fact that remains external | Saved reference that may be needed |
|---|---|---|---|---|
| Quest | Stable quest/template ID, objective definitions, valid outcomes and text keys. | Eligible template selection and approved target/variant selection. | Offer/accept/progress/result and evidence. | Quest instance ID, stable definition/template reference, owner state. |
| Location | Stable site ID, region, authored description, supported interactions and pool metadata. | Safe descriptive variants or allowed optional selection. | Discovered, visited, open/blocked, current route/hazard. | Location ID only where current save owner needs the reference. |
| Dialogue | Stable node/response identity, speaker, semantic content and allowed graph links. | Selection among authored line variants from current facts or seed. | Relationship, knowledge, quest, faction, and world changes. | Current accepted owner facts; transcript history only if its current owner stores it. |
| Encounter | Stable encounter/interaction definition and supported results. | Approved composition/variant selection. | Combat, needs, injury, inventory, survival outcome. | Encounter or event identity only through existing owner contracts. |
| Faction/campaign | Stable authored faction and milestone references. | Optional offer selection when current generator supports it. | Standing, access, conflict, campaign input and resolution. | Existing faction/campaign save references and schema compatibility. |
| Localization | Stable text key, locale entry, context and formatting contract. | Only approved authored localized variants. | No gameplay authority. | Usually no rendered-string persistence; use current key/reference rules. |

The matrix is a review prompt. Existing owners and data shapes remain authoritative even when the plan uses more descriptive semantic roles. If a current field is duplicated across records for loader convenience, document the source-of-truth and conflict behavior. Prefer references; if the source changes and the mirror does not, the loader should detect conflict instead of selecting whichever row happens to load last.

### Permanent anchor and generated instance walkthrough

Consider a permanent authored template for a short field request. The template defines its stable identity, permitted objective modules, legal target classes, campaign eligibility, failure/recovery policy, reward request, and localized text keys. At a new offer boundary, an existing deterministic generator filters the eligible template set from current owner facts and selects a valid candidate. Candidate selection does not create an accepted task, reveal a hidden location, reserve inventory, or change faction access.

If the player accepts, the quest owner creates or records one stable instance using its current identity/save contract. It stores only what that owner needs to resume: template reference/version or migration key, accepted objective configuration, deterministic variant/seed data if the generator requires it, accepted evidence/progress, and current outcome. It should not copy the complete description, every possible template variant, current location registry, dialogue graph, relationship state, or read model into a custom save blob. The authored source remains available to reconstruct the instance, and compatibility rules cover any retired reference.

During play, a generated description can vary within the template's allowed fields. Its objective kind, evidence producer, and reward bounds remain fixed by the authored constraints. If current location/actor conditions invalidate all allowed targets, the generator reports no valid offer or invokes an authored fallback. It cannot manufacture an arbitrary location ID to fill an empty board. Once accepted, a later template update does not silently rewrite the player's objective or reroll its target.

On return from play or after load, the owner rehydrates canonical progress and the UI asks for a fresh presentation. If a locale key or cosmetic variant is missing, use the declared safe fallback. If a required objective reference is missing, flag a compatibility/content failure and preserve an honest blocked state. Never mark the quest complete merely because a current build lacks the content row needed to display it.

### Selection, generation, and acceptance are different transactions

For review purposes, use three boundary events even if current APIs combine them: candidate discovery, player acceptance, and objective/result acceptance. The candidate boundary may read existing seed and owner snapshots. Acceptance belongs to the current quest command and must be explicit. Objective evidence belongs to its producer and quest consumer. A UI refresh can show a candidate repeatedly without making the operation more likely or more committed.

| Boundary | May read | May change | Must not change |
|---|---|---|---|
| Candidate build | Immutable definitions, eligibility facts, deterministic stream. | Disposable candidate view. | Quest state, inventory, discovery, faction or save state. |
| Offer presentation | Candidate view and localized copy. | Panel focus/display. | Candidate eligibility or world state. |
| Acceptance | Current quest owner facts and explicit player response. | Owner-approved active instance. | Other owners' facts unless a separate valid command follows. |
| Evidence receipt | Producer event and stable target/source ID. | Producer and quest owner facts through their contracts. | Unrelated objective counters or inferred outcomes. |
| Read-model render | Current owner results and canonical definitions. | Transient display. | New content/world state. |

If existing behavior cannot be separated cleanly, trace its current command/event/save path before changing the boundary. A narrow host adapter may coordinate established commands, but it cannot become the new authority for quest candidates or state. If the code currently generates and accepts in one operation, do not make generation visible until its side effects and duplicate behavior are documented.

### Content dependency closures

A package manifest should state which definitions are required to load, required for one campaign route, optional for enrichment, or retained only for old-save compatibility. These meanings differ. A required-to-load schema field can be absent from an optional content bundle; a required campaign node cannot be absent from a save that has accepted a task depending on it.

For each bundle, compute a dependency closure from entry points: starting quests, locations, speakers, condition facts, command kinds, rewards, and callback text. Required edges must resolve in the target build. Optional edges need a playable baseline fallback. Compatibility-only rows resolve old references but are excluded from new offers. Unreachable rows may be retained for future authorship but should not be reported as current playable content.

Do not let bundle dependencies form an accidental cycle in which core content requires an expansion faction while that faction package depends on a core campaign result that only the expansion can produce. The dependency review should flag cycles, cross-bundle critical prerequisites, shared IDs with different revisions, and optional packages that inject a required runtime consumer. If two optional packages refer to one another, both need a joint availability contract or a neutral absence route.

### Load and resolution behavior by severity

| Failure | Required response | Content authority boundary |
|---|---|---|
| Malformed required catalog | Fail the owning integrity gate with row/file detail. | Do not silently use an empty catalog and claim content is available. |
| Missing required stable ID | Block the affected release/content path. | No guessed string-to-ID or row-order substitution. |
| Missing optional variant | Select its authored fallback. | Do not affect game-state eligibility. |
| Missing old-save definition | Use supported legacy mapping or neutral owner-managed recovery. | Do not erase progress or mark a task resolved. |
| Unknown generated candidate | Reject it and report the source template/reason. | Generator cannot elevate invalid content into a location/quest authority. |
| Unsupported effect kind | Disable/block that command-bearing response. | Do not execute arbitrary callbacks/reflection from data. |
| Missing locale text | Use approved fallback locale/key behavior. | Never expose raw content IDs as player-facing prose. |

If an optional package fails integrity, isolate its content only if the current loader supports that boundary. If loading is currently all-or-nothing, do not improvise per-row suppression. Report the loader limitation and require a separately owned architecture decision if fault isolation is needed. Error containment is itself a runtime contract, not a content author convenience.

### Persistence and restore boundary inventory

Before a content field is marked durable, write the capture/restore source in the field's owner map. Durable references commonly include accepted quest/instance IDs, stable objective configuration, accepted world discoveries, persistent faction/relationship outcomes, and campaign facts. The plan does not prescribe a new list of save sections. Each value follows its current owning store and migration policy.

Transient values usually include current panel selection, sorted map list, candidate rejection explanation, uncommitted response focus, localized line text, and an unaccepted random candidate preview. On restore, recalculate them from the current catalog/seed contract and owner facts. Avoid saving transient view state because a content revision could make it stale while providing little player value.

For any generated value that must survive load, answer whether the current owner stores the selected value, enough deterministic inputs to rebuild it, or a migration reference. Re-running generation from a new seed can change an accepted target and strand the player. Persisting the entire generated world can bloat saves. The current system contract decides the minimum durable data; this plan requires that it be identified before content relies on it.

### Content-volume performance and memory policy

Optimize catalog scale in stages. Load and validate immutable definitions through the current authority. Keep dialogue graphs and descriptions out of selector hot paths unless they are needed for a visible entry. Resolve stable IDs through existing catalog indexes. Evaluate dynamic owner facts at documented refresh points, and avoid re-running the full candidate filter when the player merely changes map focus.

Measure startup/load time, catalog parse time, definition count, reference resolution, candidate filtering, dialogue graph size, allocations, localization lookup, and long-session memory before adding caches or splitting packages. A generated pool with 5,000 rows can be acceptable if it is indexed and queried rarely; a 50-row graph can be expensive if parsed on every conversation open. Record workload and profile before choosing a structure.

If caching immutable metadata, keep invalidation tied to content revision. If caching dynamic eligibility, invalidate on the relevant owner revision/event or rebuild it at each stable dispatch boundary. Never retain stale map knowledge or faction access because a cache outlived the owner facts. Do not store duplicate authoring copies in both editor cache and runtime catalog and then let both write changes.

### Integration ladder for the authored/generated boundary

Advance in this order: inspect current canonical catalogs and loader; document one permanent definition and its current consumer; introduce one bounded generated presentation/offer dimension through an existing deterministic seam; accept one instance through the current quest owner; save/restore its stable reference; validate optional-bundle fallback; then assess catalog size and performance. Each phase ends with a reviewable evidence artifact and exact owner/path claim under the current integration ledger.

Do not begin by generating a large region, broad quest slate, or branching dialogue corpus. First prove the definition-to-consumer chain and old-save behavior. A large amount of well-written content amplifies a schema/ownership mistake rather than proving the boundary. The first release can be intentionally small while the architecture remains able to scale by adding more valid rows later.

### Bundle layering and conflict policy

An expansion bundle should add stable definitions through the current catalog path. It must not silently shadow a core definition by reusing its ID and relying on load order. If an approved override mechanism exists, its precedence, compatibility rules, and revision source must be explicit. If no such mechanism exists, use a distinct definition/reference or stop and request an architecture decision; file ordering is not an override contract.

| Cross-bundle case | Expected policy |
|---|---|
| Unique optional ID | Resolve independently and mark its edges required or optional. |
| Duplicate ID with identical content | Treat as accidental duplication; keep one canonical definition. |
| Duplicate ID with different semantics | Block validation; do not select a winner silently. |
| Intentional compatible text correction | Update the canonical source revision and preserve old references as needed. |
| Optional package references missing expansion | Disable/defer the optional edge or declare a joint package requirement. |
| Expansion references core content | Resolve through the core stable ID and current consumer contract. |
| Core critical route references expansion-only content | Reject unless that expansion is explicitly required and has an accessible package path. |
| Retired row still used by accepted save | Retain compatibility resolution or migrate through the current owner. |

Dependency closure must be acyclic for required package edges. Optional cross-links should also have explicit absence behavior, even when they do not block package load. For example, two character packs that add reciprocal callbacks can each use the baseline line when the other pack is absent. A package that cannot function without another should state that dependency and be reviewed as one combined release unit.

### Safe extension and patch model

When content needs to extend an existing location, quest, or dialogue scene, decide whether the change adds an independent row, appends an optional variant, or changes existing meaning. Additive rows are easiest to review when they have separate stable IDs and a clear eligibility reference. New variants should use an existing supported variant mechanism and declare which fields may vary. Semantic edits require the revision/migration review already described in this plan.

Do not create partial overlays that copy the original definition and replace only some fields unless the current loader owns merge semantics. Ambiguous partial patches can make different platforms or save versions resolve different values. If merging is necessary, specify required base revision, field-level precedence, missing-field behavior, validator output, and rollback contract. The compiled result should be reproducible from source manifests rather than assembled from hidden editor state.

For a dialogue extension, link to existing stable nodes only when their current meaning remains valid. For quest additions, reference existing evidence/objective IDs rather than rewriting their owner facts. For locations, add pool membership through the location authority rather than duplicating route/discovery fields. For generated content, extend the allowed variant set without making the generator a source of new gameplay rules.

### High-volume content review by risk and reuse

Assign a review depth based on semantic risk, not file size. Cosmetic text variants can be checked for localization, repeated phrasing, deterministic choice, and fallback. Permanent location rows need ID, discovery, route, interaction, and retirement checks. Quest/objective rows need evidence reachability, failure/recovery, reward ownership, and old-save handling. Dialogue command rows need condition provenance, typed results, duplicate behavior, transcript and callback review. Campaign-facing definitions need full critical-path/optional-package analysis.

| Risk tier | Typical content | Validation approach |
|---|---|---|
| Low | Ambient lines, optional cosmetic description variants. | Full structural checks plus representative editorial review; verify no gameplay fields changed. |
| Moderate | Optional discoverable site, noncritical conversation, repeatable offer. | Row-level references and eligibility plus a representative route/seed fixture. |
| High | Accepted quest objective, required location, resource transfer, faction access. | Row-level producer/consumer review and focused state/restore/rejection cases. |
| Critical | Core campaign gate, ending input, save-bearing ID, cross-owner mutation. | Full dependency closure, old-save and package-off route, owner trace, content/canon review. |

Reuse should reduce repeated validation only when the underlying contract is the same. Two dialogue nodes can share a navigation pattern while retaining different predicates and effects; they still need independent owner review. Two locations can share an arrival layout but have different route status. Conversely, many rows that truly share a single schema/consumer can share one representative test while every reference is still validated. This is how a large catalog stays reviewable without replacing row-level correctness with a few broad screenshots.

### Authoring identity and localization freeze

Stable IDs should be assigned before downstream quests, maps, or dialogue rely on a row. A temporary draft key can exist in editorial work, but the release bundle must either reserve the approved stable ID or provide a controlled mapping. Never derive IDs from titles, paragraph text, speaker names, or row positions. Renaming an English title is not automatically an identity change; changing what the target means usually is.

At localization freeze, record the text keys in use, fallback locale policy, variant count, placeholder contract, and any lines requiring speaker/scene context. If a semantic edit occurs after translation, invalidate the translations whose meaning changed. A punctuation tweak need not restart every content review, but a changed choice outcome or uncertainty statement always requires translators and continuity review. The final runtime content should not mix a translated base line with a stale English-only generated variant.

### Package-off review procedure

Run the design through two explicit modes: core-only and core-plus-expansion. In core-only mode, every core quest has its baseline offer, required objective, location/evidence route, and closure. Expansion content can disappear without leaving unresolved graph edges or raw content IDs. In enabled mode, added locations and storylines enrich the route and present their own optional dependencies.

Compare the visible offer and map set, not only whether the game boots. Confirm that a missing optional speaker uses a neutral baseline line; an absent optional location is not chosen by a quest; an extra faction callback does not become the sole access route; and campaign resolution receives a valid neutral value for absent optional inputs if the current resolver requires one. If the existing campaign owner cannot represent package-off input, document the limitation before adding expansion conclusions that rely on it.
## Continuation pass 12 — Tidemark package closure, reproducible rehearsal, and release evidence

This pass uses the provisional Tidemark portfolio to make the authored/generated boundary concrete at package scale. It defines how a multi-plan content proposal can be reviewed as one coherent bundle while each gameplay fact remains with its current owner. The purpose is not to prescribe new JSON schemas or add a general-purpose package manager. The existing data authority, integrity validator, loaders, feature configuration, and save owners remain controlling. Where their current contracts cannot represent a proposed content relationship, the package stays blocked at that seam.

### Bundle inventory and source roles

A content bundle for this scenario contains authored definitions, presentation assets/text, references to current domain facts, and runtime-generated visit data. These categories must remain explicit in authoring and release records.

| Bundle component | Authored or generated? | Canonical responsibility | Does it persist as a new world fact? |
|---|---|---|---|
| Quest portfolio cards and objective descriptions | Authored permanent content. | Quest content source; consumed through the current quest owner. | The definitions are content. Accepted instances remain owned by the current quest system. |
| Location identities and semantic compatibility tags | Authored permanent content. | Location catalog/map owner contract. | The definition is permanent; discovery and availability are current owner facts. |
| Dialogue graph nodes, speaker voice, and prose | Authored permanent content. | Dialogue content; conditions read current facts and commands route to owners. | Text is not a world fact. Only explicitly accepted owner results persist. |
| Faction descriptions and character voice sheets | Authored content, with all gameplay references verified. | Narrative package; standing/access remains with current faction/character authority. | No faction membership or relationship value is created by a description. |
| Expedition selection candidate list | Derived from authored definitions plus current eligible facts. | Existing dispatch/selection owner. | The eligible list is a view, not durable content state. |
| Selected temporary expedition visit | Generated/selected for one dispatch under the established deterministic contract. | Existing expedition lifecycle owner. | Only current visit data supported by that owner persists; it cannot mint a permanent location ID. |
| Dialogue context for a particular conversation | Evaluated from current owner reads. | Dialogue host/context seam. | Snapshot or ephemeral context per current contract; not a replacement save store. |
| Quest completion, evidence, inventory, map discovery, faction access | Gameplay state owned by the current systems. | Their verified domain owners and save routes. | Yes, but only via the owner's current persistence contract. |
| Preview screenshots, trace reports, and branch fixtures | Generated review artifacts. | Build/review tooling and existing CI or test workflow. | No player save mutation; generated output follows repository artifact policy. |

Every release note should state which role each artifact serves. A generated visit may refer to an authored anchor and chosen variant, but it must not rewrite that anchor's content. A dialogue node may name a current quest result, but it must not persist that result. A package may declare that an optional location requires an expansion profile, but it cannot silently substitute a new profile default into an existing save.

### Dependency closure for the multi-plan slice

The cross-plan bundle has a small dependency graph. A valid build should be explainable from authored roots to runtime consumers:

- The provisional main quest definition references the required sluice location role and accepted objective/proof semantics.
- The location portfolio names the Switchback Sluice anchor and compatible corroboration sites.
- The dialogue scenes reference speaker and response content, plus condition facts that the verified owners can supply.
- The consequence route references the current quest/evidence command and uses the owner result to choose accepted, partial, or alternate dialogue.
- The late-game faction scene is included only in its declared optional profile; it consumes faction or campaign facts only if those owners exist.
- The expedition selection request references active quest requirements and candidates, then produces a visit using the current dispatch owner.
- Map and journal views read current discovery, quest, and visit facts through their existing presentation routes.

The build validator should distinguish a missing required edge from a deliberately optional edge. A missing required mainline location is a hard content failure. An absent expansion faction scene is valid under core-only mode. A missing optional descriptive variant can use an approved fallback if the content bundle says so. A required consequence owner that is absent cannot be replaced by generic prose because that would preserve appearance while dropping the gameplay contract.

Reference closure should be checked in both directions. Forward closure asks whether each authored reference resolves to a definition or supported owner operation. Reverse reachability asks whether each required definition is reachable from a supported offer, discovery, dispatch, or earlier content node. This detects orphaned dialogue, unspawnable required sites, stale faction scenes, and optional package content that is accidentally pulled into the base path. The validator reports the origin and target keys in human-readable form without dumping save contents.

### Profile rehearsal matrix

Use explicit profile names in editorial and build records. The names here are conceptual and must map to existing configuration contracts where available.

| Profile | Included content | Excluded content | Required proof of validity |
|---|---|---|---|
| Core-only | Base quest slice, Switchback Sluice anchor if approved, neutral local report route, ordinary dialogue fallback. | High Meridian coalition site and expansion-only meeting/voice variants. | Main task reaches an honest conclusion; all required references resolve; no optional ID is demanded. |
| Core plus local portfolio | Main quest, character task, optional discovery, selected local destinations and their prose. | Late coalition outcome if its package is still under review. | Quest objective and location role agree; failures and partial states remain readable. |
| Full Tidemark package | Local portfolio plus faction meeting, late-game signal location, and approved callback candidates. | Anything outside the declared bundle. | Cross-owner outcomes have evidence, package references close, and campaign-facing terms remain provisional until their owners are verified. |
| Older save with optional content removed | Saved owner state created while more content was enabled, loaded under a smaller profile. | Removed optional definitions and text variants. | Core save restores; missing optional presentation is safely hidden or neutralized; no state is rewritten by a content loader. |

A profile is a content configuration, not a new game mode or save-state enum. The loader must follow current contracts for enabled/disabled content. If the game cannot safely remove content once a save contains references to it, record that constraint and keep the expansion feature from claiming profile-removal support. Do not solve it by injecting a one-off migration flag into dialogue or by preserving a shadow list of disabled content.

### Reproducible content rehearsal

Before release, a reviewer should be able to reproduce the same authored package checks and deterministic selection sample from recorded inputs. The rehearsal report records the project revision, content package/profile, validation command or workflow, selection seed where applicable, locale, and current test target. It does not claim that a particular seed alone proves all location variants are valid.

A content review run proceeds in stages:

1. **Static reference closure.** Validate IDs, types, enum values, text keys, package prerequisites, required/optional references, command support declarations, location compatibility, and response-result coverage.
2. **Reachability.** Walk from core start/offer roots through quest, location, and dialogue dependencies; report orphaned required content and loops without exits. Separate intentional secret content from unreachable content.
3. **Profile comparison.** Load core-only and full package profiles. Compare required route availability, selectable candidates, unresolved references, and player-facing fallback text.
4. **Deterministic selection rehearsal.** Run a small curated seed set that exercises mandatory inclusion, exclusion collision, hidden clue, no-valid-candidate fallback, repeated-site avoidance, and package-off behavior. The exact count follows current test policy and project performance.
5. **Transcript and map review.** Capture one normal, one partial/failure, and one package-off route. Confirm that map, journal, and dialogue use matching certainty and status language.
6. **Restore and revisit review.** Use the current owner save path for accepted evidence and task progress. Reopen scenes and inspect current owner state rather than expecting a transient generated instance to be permanent.
7. **Final editorial sign-off.** Record provisional canon status, translation readiness, known limits, and the owner who accepts each remaining seam.

This is an evidence workflow, not a proposal for a new CI pipeline. Reuse the existing validation and test commands after the actual target is confirmed. If one stage has no current owner or tooling seam, the package report marks it as unresolved rather than emulating it with a script in the game runtime.

### Diagnostic categories and sample report

Content diagnostics should state severity, source reference, expected authority, and a precise repair direction. The report must separate editorial warnings from package blockers.

| Severity | Example for this scenario | Suggested action |
|---|---|---|
| Error: missing required reference | Main objective requires Switchback Sluice, but no enabled profile resolves that role. | Add an approved compatible target or hold the quest from the profile. |
| Error: unsupported command | A dialogue response promises to transfer materials, but no verified inventory command is named. | Remove the promise or acquire an approved owner seam before production. |
| Error: unsafe ambiguity | A location is both “secret” and exactly marked before its discovery prerequisite. | Correct visibility conditions or authored disclosure. |
| Error: unreachable mandatory response | The only submit-evidence node requires the evidence that it is supposed to request. | Rework the graph so the player can reach a valid offer. |
| Error: invalid package direction | Core quest content references a late expansion-only faction node as a required dependency. | Move the edge behind a supported optional route or remove the dependency. |
| Warning: missing optional voice variant | One locale lacks a nonessential callback line with an approved fallback. | Supply translation or confirm fallback copy and context. |
| Warning: duplicate semantic candidate | Two optional sites satisfy the same clue function and use the same prose role. | Clarify their distinct purpose or remove a redundant candidate. |
| Warning: high-cost branch | A low-frequency late-game result requires several owner callbacks and unique localization. | Estimate and approve the full cost before content lock. |
| Note: intentional unresolved knowledge | The player receives a reported reading that remains unverified. | Preserve uncertainty; no repair needed if the journal explains it. |

A report should avoid declaring every warning a release blocker. The package owner signs off only the warnings permitted by the declared profile and acceptance criteria. Required-path and owner-command errors cannot be waived by narrative review. A copy-quality warning can be deferred if the fallback remains truthful and readable.

### Content revision rehearsal: four change types

Different edits have different impact. The revision record should identify the type and rerun only the checks that the changed contract affects, while still running the minimal project-required gates.

**Text-only revision.** A line becomes shorter for readability without changing speaker intent, condition, commitment, or result. Recheck transcript length, localization context, and visual fit. Do not churn stable content identity if the project schema defines text keys separately.

**Semantic text revision.** A line changes from “we can inspect it” to “we can open it,” or a deadline becomes firmer. Recheck all dependent quest/dialogue paths, localized variants, and player commitment. This is not a cosmetic typo.

**Reference revision.** A required target shifts from the gate to a corroboration site. Recheck objective proof equivalence, location compatibility, selection fallback, map visibility, and every profile's critical path. A title change alone does not permit a reference migration.

**Owner-contract revision.** A result is newly accepted as proof, an item cost changes, or an optional faction outcome becomes campaign-relevant. Recheck owner API evidence, dependency ordering, save/restore, duplicate handling, journal/map refresh, and focused verification policy. This revision cannot be approved only by content QA.

The revision log should state which consumers changed and why. It should not be a generic narrative of the entire package. If the change touches multiple owners, list the authoritative source for each expected result and preserve the ordering.

### High-volume content governance for the portfolio

A large release may contain many quest cards, locations, dialogue variants, reports, and translations. Volume should not collapse into a single “package passed” checkbox. Create review batches by authority and contract: quest definitions; location compatibility; dialogue condition/command references; generated selection configurations; localized presentation. Then run one cross-bundle route review to verify that those components tell the same player-facing story.

Each batch includes:
- row count and the stable references reviewed;
- schema and consumer versions;
- required versus optional dependency counts;
- validation errors and warnings by severity;
- representative normal and fallback route traces;
- content changed since the prior review;
- review role and unresolved decision owner;
- estimated runtime and memory impact where selection/catalog scale changes;
- package-off, older-save, and deterministic replay status if affected.

Use aggregate coverage only for genuinely homogeneous properties such as required text keys, compatible tag spellings, and numeric bounds. Independent save, deterministic selection, state transition, and cross-owner outcomes remain separate review cases where the current project test policy requires them. A high-volume catalog can share a reusable validation rule while still reporting the specific row and consumer that violated it.

### Package activation and fail-safe behavior

Content should be activated only after the declared profile's static references and consumer contracts are accepted. If an optional component fails validation, follow existing package behavior: either reject that optional package cleanly or use its declared safe fallback. Do not partially register half a graph and then leave dangling references in active content. If a core required component fails, the profile should not present itself as valid.

At runtime, unexpected content lookup failure must preserve safe gameplay flow. An unknown optional dialogue node can route to a neutral exit if the existing graph contract allows one, but the validator should still identify it as a defect. A missing required quest objective cannot be replaced by “continue” text. A location missing from a single eligible selection should invoke the authored quest fallback, not delete the objective. Operational failure must be diagnosed as operational and not narrated as an in-world refusal.

If the loader has an established transaction/activation boundary, content registration follows it. Do not add a new activation manager solely to support this scenario. When the current system exposes no safe way to reject a bad bundle, the plan identifies the missing decision and keeps the bundle out of production until an owner-approved route is chosen.

### Handoff packet for implementation

Before a builder starts, the foreman/integrator needs a concise content contract packet with:
- verified source/data roots and current schema/version facts;
- the live package/profile mechanism and exact enabled-content behavior;
- the current quest, location, dialogue, faction, map, save, and localization owners relevant to the slice;
- a one-page dependency graph distinguishing authored definitions, generated visits, and owner facts;
- required/optional reference list and deterministic selection assumptions;
- fallback decisions for missing location, missing optional scene, and unsupported operation;
- profile rehearsal evidence and a focused verification plan;
- exact claimed files under WORKTREE_OWNERSHIP, with shared integration seams assigned once.

This plan is documentation and grants no ownership. The first implementation should prove one small core profile and its optional-content boundary, then grow the catalog after a passing end-to-end route. If the source audit finds a competing current package authority or an unresolved foreman decision, record the evidence and stop rather than creating a second content registry.

### Cross-plan handoff map for the first Tidemark slice

The content bundle is reviewable only when each document supplies a different part of the contract. This handoff map prevents an implementation package from copying the same quest, location, or consequence definition into several plan-owned drafts and accidentally treating those copies as authorities.

| Design artifact | Plan contribution | Implementation evidence needed later |
|---|---|---|
| Mainline quest card and objective/proof graph | Plan 17 authors the quest's intended lifecycle and acceptable evidence meaning. | Current quest owner accepts the objective/proof through a reachable command and restore path. |
| Location identity and expedition visit candidates | Plan 18 defines permanent anchor roles, candidate compatibility, and selection cases. | Current location/dispatch owner selects only valid candidates and reports a truthful map state. |
| Package/profile dependency closure | Plan 19 names authored roots, optional edges, and profile-off behavior. | Existing data loader/validator proves references resolve without a duplicate registry. |
| Transcript, voice, and location prose | Plan 20 writes complete scenes and localization context. | Current dialogue consumer can reach the nodes and preserve focus/exit behavior. |
| Context and skill-gate policy | Plan 21 specifies which current read facts may reveal optional wording or routes. | Verified adapters return supported facts and safe unknown behavior. |
| Consequence request/result and campaign bounds | Plan 22 names the owner sequence and observable result for each commitment. | Actual owners accept/reject/reconcile results and capture their state through current saves. |

The integrator should draw one path from accepted quest offer to an expedition result, then from returned evidence to a dialogue recommendation, then to the visible journal/map outcome. At each arrow, record whether the edge is a content reference, an owner read, a command, an event, or a presentation refresh. If a plan's reference expects a command that no current owner exposes, the chain stops there and the architecture decision is written down. It does not become an empty event callback.

This map also guides revision impact. Changing an adjective in an arrival description may require only prose/localization review. Changing a location's compatibility role affects Plan 17 proof reachability, Plan 18 selection, Plan 19 reference closure, and possibly Plan 22's trial branch. Changing a dialogue gate affects Plan 21 and the transcript/reachability in Plan 20. Changing what a faction recommendation means affects Plan 22 plus the campaign owner and every callback that states adoption. The change record lists affected consumers rather than copying a complete bundle summary into every document.

The first release receipt should name the profile tested, content revision, validation results, route trace, current owner APIs verified, focused verification target, known limitations, and exact live file claims. No document in this series is itself that receipt.
## Continuation pass 13 — Farline record contracts, identity rules, and owner traceability

This pass defines a content-facing model for The Farline Circuit while preserving the project's single-source rules. Its records describe permanent authored content and the references needed to ask current owners for results. Generated expedition visits and current gameplay facts stay outside authored definitions. The structures below are conceptual review contracts, not approved JSON schemas. Every eventual field, ID convention, loader, and consumer must be checked against the current data authority.

### Conceptual record families

| Record family | Stable authored meaning | References it may carry | Values it must not own |
|---|---|---|---|
| Quest definition | Purpose, family labels, offer text, objective sequence, evidence semantics, failure/closure copy. | Supported location roles, known proof types, dialogue scene references, command-owner operation references after verification. | Live lifecycle, accepted proof state, inventory amount, relationship score, faction standing, current world availability. |
| Location definition | Permanent place identity, semantic role, authored descriptions, permitted encounter use, compatibility information. | Quest and dialogue content references; current travel/map owner IDs only when verified. | Whether an expedition selected it, current resource contents, discovery state, route reachability, temporary visit state. |
| Dialogue scene/node | Speaker, authored text, response labels, conditions, result presentation, graph transitions. | Supported condition facts and current command owners; localization keys; quest/location references. | Canonical objective completion, side-effect execution, hidden persistent memory, campaign outcome. |
| Faction profile | Draft identity, narrative goals, voice constraints, authored availability beats. | Verified faction owner reference if one exists. | Membership, access, reputation, hierarchy authority, diplomacy outcome. |
| Character sheet | Voice, role, public facts, limits of knowledge, draft appearances. | Verified character/relationship/quest facts and scene references. | Numeric disposition, permanent emotion, live location, conversation visit count. |
| Discovery clue | Authored clue meaning, source description, possible reveal routes, disclosure policy. | Current map/discovery owner facts and quest proof reference. | Exact discovered status or expedition selection state. |
| Generated visit context | Selected anchor, deterministic variant inputs, current expedition context. | Authored location identity plus existing owner facts. | New permanent identity, hidden quest progress, noncanonical random outcome. |
| Release profile | Intended content inclusion and required/optional dependency closure. | Existing package/configuration mechanism. | Player campaign mode or a new save flag. |

The content author can propose a stable reference to a current owner, but the runtime value comes from that owner. If the quest asks whether a signal plate has been inspected, the quest definition identifies the accepted proof meaning; the canonical progress store determines whether proof exists. A generated visit can point to the plate's authored location definition, but it does not make the plate discovered.

### Example logical content references

For editorial review, this storyline can use draft handles such as “Farline main investigation,” “Siltglass Relay,” “Yara character scene,” “three-knock clue,” and “Compact meeting.” These handles are readable labels, not final IDs. Production identity should be assigned by the project’s current snake_case/data-ID rules, and any canonical ID must be stable when titles or localized text change.

A future record can express relationships in plain terms:
- main quest requires one relay inspection objective;
- Siltglass Relay exposes the authored signal plate;
- a current discovery fact reveals Old Echo Cut;
- Yara's optional scene can offer a second account;
- a faction offer becomes eligible only from supported quest/faction facts;
- an expedition visit may be selected from compatible location definitions;
- a consequence request routes through a current quest/faction owner.

It must not express “if player heard file 7, set faction score to 2 and mark road safe.” That sentence binds presentation, progress, standing, and travel into a script-like data effect with no clear owner. Split the proposal into readable facts and one supported command at a time.

### Provenance and content authority ledger

Each high-impact content row should have a review ledger that identifies its source and consumer. The ledger is documentation for design/build review, not a second runtime registry.

| Question | Example answer for Siltglass Relay |
|---|---|
| Who authored the location's identity and description? | Content author under the location catalog process. |
| Who says the site is selectable now? | Existing location/expedition availability owner, verified before implementation. |
| Who says the relay plate was inspected? | Existing quest/proof owner after accepting supported evidence. |
| Who knows the old road signal? | The speaker's supported knowledge source, or an explicitly attributed report in authored text. |
| Who decides whether the player can submit a Compact recommendation? | Current quest/story command owner, with faction validation where required. |
| Who controls whether a map marker is visible? | Current map/discovery owner and its disclosure contract. |
| Who persists a durable result? | The current owner of that result under its existing save capture/restore route. |
| Who can approve an ending callback? | Current campaign/ending owner if such a consumer exists; otherwise no durable ending claim. |

If two plans name different owners for a fact, resolve that conflict against source and live authority rather than choosing the more convenient one. The content package remains blocked on a reference when the owner is unknown. A prose label such as “reported” can still be drafted, but it cannot justify a command or permanent state change.

### Field-by-field conceptual content envelope

The goal is to provide enough information to author, validate, and review a record without embedding gameplay state. Conceptual fields can be grouped as follows:

**Identity:** stable content reference, human title key, package/profile requirement, authoring revision, and retirement/replacement reference where applicable.

**Presentation:** description keys by context, speaker attribution, localization notes, accessibility description, and optional asset/audio cue references that resolve through existing asset/audio owners.

**Compatibility:** semantic family, permitted location role, supported quest family, content size/branch limits, and any required feature/profile condition.

**Conditions:** a list of supported read facts and declared unknown/fallback policy. Facts identify the owner and meaning; they do not embed arbitrary expressions or script.

**Action reference:** operation owner, bounded command parameter contract, dependency order, result kinds, and result text references. Informational responses explicitly declare that they have no gameplay action.

**Outcome presentation:** accepted, rejected, partial, deferred, duplicate, stale, and operationally unknown copy coverage as applicable. Durable facts remain outside the content record.

**Review metadata:** risk band, narrative/canon status, required/optional, core/expansion candidate, known integrations, validation note, and unresolved decision link.

This is not a mandate to create one mega-schema containing every kind of record. The existing catalogs may use separate shapes. The benefit is a shared semantic checklist that prevents authors from silently adding owner state to a convenience field.

### Authored permanent anchors and generated instances

**Siltglass Relay** is a permanent authored location definition. Its name, service hatch, signal plate, route-history prose, and allowed encounter roles are stable content. A visit might vary by eligible route report, weather description if supported, or quest objective. The generated context does not change the location's identity.

**Old Echo Cut** is an authored hidden-place definition whose visibility is controlled by current discovery facts. A selected expedition may instantiate a visit referencing it, but it does not mint new clue identity. If it is not selected, the player keeps whatever hint the discovery owner supports and the quest does not claim inspection.

**Half-Span Shelter** is an authored permanent anchor. Who is present, what task is available, whether the location can act as safe staging, and whether an expedition may return there are current owner facts. The description cannot turn it into an inn, medbay, or crafting station unless those systems support the service.

**Farline Overlook** is an optional expansion anchor. The core-only profile contains no required reference to its IDs or dialogue. If the expansion is disabled, the base investigation can close locally. Optional references are validated against their profile but cannot invalidate core load when absent.

The separation enables reuse without copying state. Multiple quest definitions can reference the stable Relay anchor. Each accepted quest objective still belongs to its current quest owner. Multiple generated visits can select the same anchor if lifecycle rules permit them, but they must obey existing state and discovery contracts.

### Deterministic authoring and selection requirements

Authored data itself should have stable identity and order where order is meaningful. Content validation should not depend on unordered file enumeration. If a generation/selection owner chooses among valid authored location candidates, it uses the established seeded RNG contract and stable candidate ordering. The data layer does not introduce a second seed or invent a per-record random choice.

Prose variation should be deterministic when it affects repeated visible output that players may rely on for decisions. Purely cosmetic line variation can be optional; it may not alter the represented risk, quest eligibility, or accepted outcome. A generated arrival must not randomly claim that the path is safe in one seed and blocked in another unless the corresponding world owner supplies that variation.

A release rehearsal records the profile, content revision, and seed/context used for representative selection cases. It should also state which values are authored defaults, which are selected variant data, and which are current owner results. This trace makes a failing replay diagnosable without serializing all player state into diagnostics.

### Identity changes and content retirement

A title or prose correction usually preserves the stable reference. A changed quest meaning, proof type, location role, command owner, or faction availability rule is a semantic change. Review the impact on accepted saves and active campaigns before replacing a referenced record.

Retiring a location or scene requires a dependency search. If an active quest may refer to it in a supported save, the project needs its existing migration or compatibility behavior. Do not simply delete the row and rely on dialogue to offer an unregistered substitute. If the current content/save system has no safe retirement contract, keep the content in place, deprecate its use for new offers through an existing mechanism, or obtain an architecture decision before removal.

Text changes that alter uncertainty, consent, cost, deadline, or irreversible commitment need semantic review even if the underlying reference is stable. A translator should receive the old and new intent, not only a string diff. Voice or audio assets, if any, are referenced through the current asset/catalog route and retire under that owner's lifecycle.

### Reference validator rules for the Farline package

A future validator can check the following without becoming a gameplay authority:
- every required quest location reference resolves in the active profile;
- each location role is compatible with the proof requested by its consumer;
- secret/hidden references do not appear through unearned map or dialogue routes;
- every scene speaker has a defined content entry or approved fallback;
- condition references point to supported owners and include unknown behavior;
- each command-bearing response names a current operation/result contract;
- result copy exists for all owner outcomes that may occur;
- package dependencies are one-way or otherwise explicitly supported;
- a core root never requires an expansion-only target;
- generated location candidate inputs resolve to stable authored definitions;
- two authored rows do not claim the same stable identity;
- retired or deprecated references have a documented replacement/compatibility policy;
- no content row serializes arbitrary script, private save data, or a local gameplay counter.

The validator reports the exact row and relation in error messages. It can say “Quest reference requires Siltglass Relay under core profile, but the profile excludes it.” It should not dump user files or raw save payloads. A warning can point out a line without a speaker-context note; a blocker is reserved for missing required references, unsupported operations, or unsafe profile closure.

### Core and optional profile contract

The main investigation can be a core candidate if its first slice can use an existing core-compatible location and owner APIs. The Farline Compact, Farline Overlook, multi-faction meeting, and late-game policy result are expansion candidates. Three Knocks, No Answer may be either, depending on whether it adds a secret location to the base world or just optional prose.

Profile acceptance compares actual player routes:
- core profile: receive report, reach supported inspection, record a narrow conclusion;
- expanded profile: access character and faction branches without changing core objective ownership;
- optional package absent: no missing Compact node or Overlook location blocks the journal/map;
- older save with stale optional references: follow the project’s current compatibility behavior and preserve existing canonical owner state.

A profile does not change the player's campaign state. It only determines what authored content can be loaded and selected under the existing package mechanism. If the game cannot support disabling content after campaign creation, the design states that deployment constraint rather than pretending that profile-off migration is already implemented.

### Data-to-runtime handoff receipt

Before implementation, the integrator attaches a receipt naming:
- actual catalog files and current schema versions discovered from source;
- current loader, validator, and runtime consumers;
- stable IDs approved for the first slice;
- quest, location, dialogue, faction, map, save, and optional audio owners verified;
- exact references between authored records and generated visits;
- command operations and allowed result kinds;
- active content package/profile behavior;
- deterministic inputs and ordering policy;
- old-save/retirement constraints;
- focused verification and output evidence;
- exact file ownership and acceptance path in the live ledger.

The receipt is the point where a conceptual design becomes an implementation proposal. This plan is not the receipt and does not claim paths. If an existing owner or schema conflicts with this model, the model must be revised to fit the verified authority.

### Package walkthrough: from approved brief to player-visible result

A release rehearsal should follow one concrete Farline route through the authored and runtime boundaries. The goal is to expose missing handoffs before a large content batch is written.

**Editorial brief.** The brief states that the player finds an obsolete route signal, inspects a stable signal plate, and can request a local/coalition review. It lists the claims the story may make and the claims it may not make. The latter include “the route is safe,” “the faction adopted the standard,” and “the warning reached every shelter” unless current owners can establish those facts.

**Content design.** The author proposes a main quest definition, a Siltglass Relay location definition, a disclosure-controlled Old Echo Cut clue, one Yara conversation, and a late-game Compact scene. Required and optional edges are marked separately. Each authored item receives a stable identifier through the current data authority; draft labels remain non-runtime.

**Static verification.** The current catalog validator checks syntax, IDs, localized text keys, references, compatibility, supported condition facts, command/result coverage, and profile closure. A missing required relay reference blocks the content profile. An absent optional coalition scene is allowed in the core profile. A command with no current owner operation blocks that response from production.

**Runtime route.** The expedition owner selects the stable Relay anchor using the active quest request and deterministic selection policy. The quest owner records inspection evidence. The dialogue context adapter reads that accepted fact. A later response submits the recommendation to the verified owner. The journal and map refresh through current read paths. The release trace identifies the owner at each transition.

**Review and release.** The package is reviewed in core-only and expansion-enabled profiles, checked for save compatibility and deterministic replay where relevant, and released only after the declared acceptance evidence is captured. If a semantic change happens after content lock, the package reopens only affected reviews plus required baseline gates.

This walkthrough is intentionally linear. It does not require an event bus dedicated to narrative content. Existing events, read models, and data consumers remain the implementation paths.

### Type-specific field review cards

A shared semantic checklist should not force every data record into the same serialized shape. Use a card for each record family and verify which pieces already exist in the current schemas.

**Quest definition card**
- Stable reference and display/localization keys.
- Authored quest family and intended production tier.
- Entry source and distinction between discovery and acceptance.
- Objective definitions, order, optionality, and accepted evidence meaning.
- Required location request and compatibility/fallback policy.
- Offer/accept command owner, proof/result owner, and known response types.
- Failure, expiration, abandonment, reopen, partial, and resolved copy.
- Reward intent with actual reward owner and duplicate behavior.
- Package dependencies and core-only path.
- Review trace for journal and save/restore.

**Location definition card**
- Stable authored identity and map/discovery reference.
- Permanent anchor versus temporary visit role.
- Compatible content functions and unsupported uses.
- Arrival/inspection/return text keys and accessible description.
- Risk certainty source and clue-disclosure rule.
- Optional asset/audio links through existing catalogs.
- Package/profile membership and absence fallback.
- Quest proof relation, if any, without copying quest state.
- Reuse allowance and unique lore boundary.
- Map/route/performance review.

**Dialogue scene card**
- Stable scene/node/response references in the current data model.
- Speaker and location references plus source of each factual claim.
- Conditions from current owners and behavior for false/unknown/unavailable facts.
- Response classification: information, local interaction, or command.
- Command owner, bounded inputs, result kinds, dependency sequence.
- Copy for accepted, rejected, partial, pending/deferred, stale, duplicate, and unknown where applicable.
- Graph exit, revisit, pending-request, and terminal quest behavior.
- Localization, accessibility, transcript, focus, and scene-size notes.
- Profile-off and old-save fallback.

**Faction/character card**
- Provisional or canon-approved identity and scope.
- Narrative value and limits of knowledge/authority.
- Scene availability source and public versus private facts.
- Existing gameplay owner reference, if verified.
- What behavior is authored only and what behavior is a command/result.
- Callback eligibility and package dependency.
- No local relationship or faction score.

A card prompts authors and integrators to discuss responsibility; it does not demand that every catalog duplicate all data. If a field belongs in a separate quest owner or localization catalog, the content reference points there according to current project convention.

### Reference edge review and deletion scenarios

Package closure is stronger when reviewers test both normal addition and removal. For the Farline package, run at least these design rehearsals:
- Remove the optional Compact scene; confirm the main investigation still closes locally.
- Remove Old Echo Cut; confirm no core quest objective silently depends on its clue.
- Remove Yara's character scene; confirm a main report does not point to an absent speaker.
- Remove the optional Hush Kit crafting route; confirm relay inspection remains possible.
- Remove Siltglass Relay from the profile; confirm the core profile is marked invalid unless an approved equivalent or delay is authored.
- Retire a temporary service niche; confirm no permanent authored location key depends on the generated visit.
- Change a speaker title; confirm references use stable IDs rather than title text.
- Change the meaning of an objective result; identify active save and campaign impact before release.

The reviewer distinguishes “optional edge removed” from “broken required edge.” The first is a valid profile. The second is a release error. Do not create a universal substitute resolver that silently repairs all missing references; the content designer must decide whether an alternate is semantically equivalent.

### Data validation severity and repair ownership

A diagnostic is actionable when it says which record failed, why the relation matters, and who can repair it. Severity bands are assigned by gameplay effect, not by how dramatic the prose sounds.

| Example issue | Severity | Repair owner |
|---|---|---|
| Nonessential arrival variant has no translated key, approved fallback exists. | Warning. | Content/localization pipeline. |
| Optional clue has an unreachable but nonrequired line. | Warning or content quality issue, depending on disclosure contract. | Dialogue author with graph reviewer. |
| Main quest requires a location excluded by core profile. | Error/blocker. | Quest/location/package integrator. |
| Response says “I gave you the cell,” but inventory owner never accepted transfer. | Error/blocker. | Dialogue author plus inventory command owner. |
| Repeatable task has no supported reset contract. | Design blocker for repeatability; one-time version may remain valid. | Quest owner/foreman decision. |
| Generated visit refers to an unknown authored anchor. | Error/blocker. | Location data/catalog owner. |
| Optional faction fact is unknown while package is off. | Valid only with declared neutral fallback; otherwise profile error. | Package/dialogue owner. |
| Prose overstates a report as verified route safety. | Content blocker even if the graph resolves. | Narrative continuity reviewer. |
| An editor changes an identity key based on English title. | Identity warning or error under current schema. | Data authority owner. |
| Save migration behavior for a removed row is unknown. | Architecture blocker before removal. | Save/content authority owner. |

An error should not be converted to warning because a route “probably” will not happen. A warning may be accepted only with the named fallback and responsible reviewer. Disabling an invalid optional package is safer than loading half its graph and letting dangling content leak into a campaign.

### Localization and asset/reference contracts

The data boundary includes prose keys and asset references, but neither creates a parallel game asset owner. A location may request an image/portrait/cue by stable catalog reference. The current asset/audio registry determines whether it exists, is supported, and is loaded. If the requested visual or cue is absent, presentation can use its approved fallback. A missing required gameplay interaction is not repaired by an image or sound.

For every localized string, store stable key, source language, speaker/context note, branch intent, maximum expected layout demand, and whether the string can appear in the transcript. Conditional lines should be complete semantic variants where grammar or disclosure can change. A late-game faction label should not be embedded in multiple English paragraphs when a shared localized key is supported. Semantic edits to costs, deadlines, uncertainty, or consent reopen translation review.

Asset fallback must be neutral and not create a false state. If the signal playback cue is absent, a text report can still communicate that a signal was heard only if that event is actually supported. A silent audio fallback must not imply that the relay emitted no sound. If the map icon is absent, use the existing generic icon policy and preserve a textual location label.

### Content profile test matrix at volume

The minimum profile comparison should include more than a successful boot. For each profile, record:
- enabled authored definitions and package count;
- required and optional reference closure;
- unresolved or disabled location candidates;
- reachable quest offers and terminal paths;
- dialogue nodes that depend on absent speakers/owners;
- localization fallback count and severity;
- candidate count and selection time under existing instrumentation;
- route count in the active map/journal views;
- old-save references according to current migration policy;
- deterministic selection/quest outcomes where required;
- panel refresh/disposal state if content loads asynchronously.

At higher volume, track maximum quest instances, location candidates, dialogue graph nodes, conditional responses, and text variants from real content catalogs. The design should set test thresholds only after measuring existing runtime budgets and policy. Do not embed a planned maximum in data and assume the UI can support it. If a high volume exceeds the current owner, split content into profile/load groups only through an approved existing mechanism.

### Branch preview and trace artifact

A release review needs a compact artifact linking content to its runtime consumer. One acceptable form is a trace table with one row per important player action:

| Player action | Authored reference | Current owner | Expected result | Visible surface | Profile behavior |
|---|---|---|---|---|---|
| Select relay expedition | Quest target plus location reference | Existing dispatch/location owner | Relay selected or declared delay | Expedition preview/map | Core route valid |
| Inspect signal plate | Interaction/quest proof reference | Existing quest/evidence owner | Accepted proof, rejection, or unknown | Objective/journal and scene | Required only for enabled core slice |
| Reveal Old Echo clue | Discovery definition and condition | Current map/discovery owner | Hint or exact marker under disclosure rule | Map and optional journal | Removed cleanly if optional package off |
| Request Compact review | Dialogue action reference | Verified quest/faction owner | Accepted recommendation or defined refusal | Scene/journal/faction view | Omitted under core-only profile |
| Return after restore | Current owner result references | Existing save/read owners | State reconstructed without duplicate | Journal/map/dialogue | Older saves use current compatibility behavior |

Artifacts can be generated by the existing validation or test tooling if supported. Do not add generated reports manually to a source directory or claim they are official without the repository's output policy. Diagnostics should be small enough for a reviewer to inspect, carry a content revision/profile ID, and omit private runtime data.

### Change-control scenarios

Review semantic changes before they reach a release branch:
- **A report is no longer accepted as proof:** identify active quest states and a replacement path before removing the reference.
- **The Compact becomes core content:** every core profile must gain its full required dependency closure and the base campaign still cannot assume a faction outcome.
- **Siltglass is retired:** define stable identity compatibility and active quest handling with the current save/location owner.
- **Yara's dialogue becomes a faction offer:** update condition/action classification and consequence review; do not leave the old graph semantics intact.
- **A new location appears only in generated selection:** ensure it is still tied to an authored definition and cannot mint quest proof by itself.
- **A translated line changes “unverified” to “unsafe”:** treat as a material semantic change and re-review all locales and owner facts.
- **An audio cue changes:** use the current asset/audio ownership process and confirm that narrative event meaning did not change.

The release record lists changed references and consumers. It does not require a full new audit for a punctuation-only edit, but it must include any required current validation run.

### Package closeout and future integration order

The first implementation candidate should contain one quest definition, one permanent location, one dialogue scene, one accepted proof route, and one journal read. This is the smallest set that proves authorship, reference closure, runtime consumption, and persistent outcome. Add generated visits only after stable location identity and deterministic selection are confirmed. Add optional clue content after discovery and disclosure behavior works. Add Farline faction scenes last because they depend on the broadest set of owners.

The integrator closes the content package only when the actual schema and loader are known, all selected references resolve, content ownership is unambiguous, the core and optional profiles behave as declared, and focused verification evidence is attached. Until then, the detailed field cards are a design model. They do not grant write ownership or authorize a new data format.


## Continuation pass 13 — Farline content package specification, review ledger, and release rehearsal

### Package objective

This module defines a disciplined path from a provisional narrative brief to authored records that can be reviewed and eventually integrated. Its concern is content integrity: stable identity, traceable provenance, explicit dependencies, bounded variation, and a truthful route into the game. It does not establish a new registry or validate a fictional schema as though it were already implemented. The current JSON authority and integrity pipeline remain authoritative. Before any content file is added, the responsible owner must verify the actual schema, consumer, identifier policy, migration rules, and release process.

The Farline Circuit’s central content problem is how to support player-facing uncertainty without making authored content itself ambiguous to validators. A player can hear an unverified report. The record that defines the report must still declare its speaker, source, location context, intended confidence, prerequisites, and outcome effects. Fictional uncertainty should be represented as deliberate authored fields or existing supported concepts, not left to missing data or vague prose.

### Design envelope and the no-duplicate rule

A content proposal may be represented in a design ledger before it becomes an implementation record. The ledger is useful for discussion, but it must not become a second mutable runtime authority. It should be either a planning artifact or a generated audit view from canonical files. No runtime code should read both the canonical catalog and a manually maintained ledger to decide what exists.

The package should follow one primary record per concept. A site is one location record with multiple eligibility reasons. A character is one character record with dialogue availability conditions. A quest is one authored quest record whose variants branch by facts the current system can persist. Do not create separate copies named “high trust version,” “low trust version,” and “secret version” unless the current schema explicitly requires separate records and the validation demonstrates why. Duplicated records drift in prose, references, and availability conditions.

Any derived index should be generated by the repository’s established tooling if available. If there is no generator, keep the index clearly editorial and exclude it from runtime. Changes to a canonical ID require updating references through the existing data migration process. Human-readable labels can change more freely, but player-facing text should not be used as an identifier.

### Content identity and provenance register

Every proposed record should have a review trail with the following conceptual information: stable candidate ID; content category; first authoring source; canon anchors checked; related plan sections; status as proposal, approved, implemented, or retired; dependencies; fallback; target release; current validation result; and last review date. This is a planning ledger, not a save structure.

Provenance is story content when it belongs to the fiction and content governance when it belongs to authorship. These must not be conflated. For example, a note can claim it was copied from the western relay. That is an in-world source claim. The authoring register separately tracks which proposal introduced the note and which canonical record it was based on. The player should never see internal review status. The validator should never infer source truth from a prose sentence alone.

When a record is adapted from an existing location or character, the review register points to that record and states whether the new content is an extension, contextual variation, or collision risk. A new name is not evidence of distinct design space. The content review asks whether the candidate repeats an existing mechanic or faction role. If it does, fold the new content into the existing owner unless a clear player benefit justifies a separate record.

### Field-level authoring guide

Fields should be divided into four practical groups when mapping to the actual schema:

**Identity and references.** ID, display name, category, speaker or location reference, quest references, and other stable joins. These must be valid, unique, and consistent with the repository’s current naming convention.

**Eligibility.** Conditions that determine whether a record can appear: campaign stage, prerequisite facts, location state, relationship, faction standing, expedition context, or discovery. Use supported condition forms only. Avoid free-form condition strings that no consumer evaluates.

**Presentation.** Text, title, journal summary, map label, speaker intent, localization key or current localization mechanism, and optional asset reference. Text should not encode a gameplay effect that the runtime cannot parse. A line such as “the route is open” cannot substitute for the supported route-state change.

**Effects and links.** Quest progress, relationship or reputation outcomes, location change, delayed callback, next node, reward, and result state. The owner for each effect must be identified before implementation. If the field is unsupported, mark it as a design dependency rather than adding it speculatively.

For every proposed field, the authoring packet should state its type, requiredness, allowed values, reference target, default behavior, and what happens when it is missing. The schema validator should reject malformed values and invalid references. The game should still degrade safely when content is absent: a missing optional line may fall back to a generic authored response; a missing critical progression record should be a catalog error in development and a visible safe closure in a released save, according to existing policy.

### Permanent authored content and bounded variation

Permanent authored records define the canonical identity and essential meaning of the story. The player’s conclusions, major outcomes, and location history should not be synthesized from an unconstrained text generator. Variation can come from authored alternatives and stable inputs: weather category, location condition, character knowledge, previous choices, expedition composition, or a seeded selection among equivalent lines.

Generated or runtime-composed content, where currently supported, must remain a bounded presentation layer around authored facts. The system may select one of several authored observations or combine a base description with a weather suffix. It must not invent a clue, faction decision, survivor biography, or quest requirement that is absent from the content authority. If there is no current generation owner, this paragraph remains a future design constraint rather than permission to add one.

A good variation envelope contains: a canonical base record; a small authored variant set; explicit conditions; stable selection rules; a fallback text; and a statement of which facts remain invariant. For Siltglass Relay, the invariant may be that its older board contains a gap cut through a maintenance schedule. Weather can change whether the paper is wet or frozen. The clue that the cut occurred before the last inspection remains the same. This protects narrative meaning while enabling atmosphere to respond to the world.

### Dependency ledger and removal proof

Each content record should declare both hard and soft dependencies.

- A hard dependency is required for the record to function, such as a valid speaker or the current quest objective consumer.
- A soft dependency enriches the experience, such as a companion line or optional map callout.
- A branch dependency is a fact created by a prior choice and consumed later.
- A presentation dependency is a current UI or localization surface needed to show the result.
- A continuity dependency is a canon fact that the record must preserve.
- A removal dependency is a downstream reference that must be retired or redirected if the record is cut.

For each dependency, state the safe behavior if unavailable. A missing soft companion line can be omitted. A missing required speaker can shift the scene to a notice only if the notice is authored and its source remains credible. A missing branch fact cannot be replaced by a default that quietly chooses the player’s outcome.

Removal review is as important as addition review. When a location candidate is retired, determine which quest objectives, map hints, environmental clues, and dialogue references depend on it. Redirect each reference to a supported equivalent or explicitly close the branch. The authored history should not imply that the player visited a place removed from the campaign. Keep redirects narrow and documented. A “compatibility alias” that lasts forever without consumer evidence becomes an additional authority.

### Release rehearsal: from proposal to player result

The package review proceeds in bounded steps.

**Brief freeze.** Record the intended player experience and the questions it should raise. For the Farline chain: how should an exhausted community handle a signal whose wording may have outlived its origin? What evidence is enough to justify a warning? Which risks are caused by silence and which by confident repetition? This statement is not a mechanic and cannot be used as a test assertion; it guides later prose review.

**Canon and collision pass.** Search current canonical content for the role of each proposed person, group, location, clue, and mechanic. Compare functional purpose, not only string names. The Lantern Wardens remain local route stewards; the Farline Compact remains a courier and message network. If existing factions already perform those jobs, redesign the proposals as local practices or factions’ internal roles. Do not add a new major faction merely because the plot needs two opinions.

**Schema mapping.** Identify exact current records and supported fields. Unknown concepts are logged as gaps. The implementer and author agree which facts can be expressed without extending Core. If a proposed consequence needs a new persistent kind of state, the proposal pauses for the proper architecture owner.

**Content authoring.** Create the smallest playable records in the current data authority. Use the supported identifier format and version requirements. Keep prose in existing fields, not a parallel supplemental file, unless that is the established pipeline.

**Integrity validation.** Run the current catalog validator or its focused official command after implementation is approved. Validate IDs, references, ranges, conditions, and consumer reachability. A record that parses but is never offered is not complete. A condition that references a nonexistent fact must be reported as an error.

**Runtime proof.** Trace from game entry to the player’s action, objective proof, effect application, save, reload, and later callback. Presence in JSON is not proof of reachability. The host route must actually present the content, and the current state owner must observe the outcome.

**Language and accessibility review.** Check map labels, journal summaries, dialogue, notices, and warnings for comprehensible uncertainty. Avoid color-only confidence indicators. Confirm keyboard/controller flow and focus if a new UI surface is required. Reuse existing UI whenever possible.

**Release handoff.** Record changed data files, IDs, effects, exact validation command and result, runtime evidence, known limitations, and migration needs. The handoff should name the owner of each seam. Do not claim a faction consequence was implemented because a line changes if the underlying trust owner does not change.

### Validation severity and repair routing

A useful validator outcome distinguishes severity rather than flattening all issues into warnings.

- **Invalid identity:** duplicate or malformed ID. Block content integration.
- **Broken hard reference:** missing required speaker, location, objective, or consumer. Block.
- **Unreachable required record:** valid but cannot be selected by any supported route. Block.
- **Contradictory terminal behavior:** quest can be completed and remain permanently active or both succeed and fail. Block.
- **Unsupported effect:** authoring promises a consequence without an owning consumer. Block or revise the design.
- **Missing optional presentation:** use the documented fallback and report for repair.
- **Orphaned optional variant:** remove or reconnect if no approved design use remains.
- **Unclear prose:** editorial fix; do not silently change state behavior.
- **Unverified canon claim:** return for evidence review.
- **Expected absence:** an optional site may not appear in a given seeded expedition, provided fallback and selection guarantees are met.

Repair routing should go to the owner of the contract. Data errors go to the content author and validator owner. Quest semantics go to the quest owner. Dialogue gates go to the dialogue consumer owner. Map visibility belongs to the existing location or expedition presentation owner. Cross-owner conflicts require the named integrator. Do not duplicate the behavior in a convenient panel to make a broken record appear functional.

### Localization, assets, and content scale

Large prose volume creates additional obligations. Every player-facing string should use the current localization path if one exists, or be clearly marked for that migration if the release policy requires it. Avoid embedding state identifiers in visible text. Separate a short map label from a long journal description so the UI does not need to clip a paragraph. Keep audio scripts, subtitle text, and accessibility captions synchronized when they represent the same message.

Assets should use the existing Godot-native asset registry and naming policy. A clue must remain understandable if its optional art is absent or unavailable at a small scale. The asset reference supplements the authored fact; it does not own it. If a note texture shows wording that differs from the localization string, define the current authority for that text before production. Do not put unique essential evidence only in an image that cannot be localized or exposed through accessibility support.

Estimate content by production surface rather than raw word count: number of quest records, unique dialogue nodes, speakers, branch variants, location interactions, map labels, notices, assets, VO/subtitle requirements, and test paths. A short line with six context variants can cost more to validate than one longer stable journal entry. The Farline plan should favor reusable scene functions and authored alternates where they preserve voice and evidence.

### Profile and migration matrix

Before shipping records, review at least these profiles: new game with no Farline knowledge; mid-campaign player with one discovered relay clue; a save where Yara is unavailable; a save where the preferred site was visited before quest acceptance; a save with an active route blocked; a save where the player resolved the signal with uncertainty; a save where the temporary caravan expired; and a save created before the new records were installed, if the release process supports such updates.

For each profile, document expected journal state, map state, dialogue availability, and objective result. If the current save contract cannot distinguish necessary outcomes, do not invent a new store in the content package. The right next action is a small architecture decision based on the current restore path. Existing fields may already encode the distinction, so inspect before proposing new state.

A migration rule is needed only if existing saves can contain relevant state that the new content interprets differently. The migration must be owned by the established save mechanism and remain deterministic. New content should generally be additive and tolerant of absent optional facts. Never assign a default branch that rewrites a previous choice.

### Package close criteria

The content package is ready for a scoped integration plan when every record has a stable candidate identity, verified schema mapping, clear source provenance, complete references, reachable entry path, objective proof, fallback behavior, effect owner, map/dialogue presentation, removal path, and profile expectation. The package can be rejected as oversized if its production cost exceeds the player benefit or repeats existing systems. It can also be reduced without losing its central question.

A minimal first release can include one investigation, one shelter conversation, one location substitution, and one resolved message treatment. The remainder of the Farline story can then be added in a later, separately reviewed slice. This sequencing creates a factual base for later expansion rather than requiring a large set of speculative IDs. Do not publish the design ledger as if it were a live catalog.


### Content trace and release evidence

The release handoff should make it possible to follow one authored clue from its planning origin to its final visible interaction. Choose a representative item such as the Siltglass service card and record its proposal reference, canonical data record, visual asset reference if any, location interaction, quest proof, journal summary, save owner for resulting state, and one later callback. At every step, identify the authority that actually supplies the fact. This trace exposes duplicate sources early: if both the card image and a separately maintained dialogue note independently define the same interval, edits can make them contradict.

A content release packet should include a changed-file list and a human-readable record list. It states which IDs are new, which existing IDs were extended, which optional references can be absent, and whether any migration is required. It includes the exact focused validation command and result only after implementation is actually performed. During planning, list the intended validator and owner without claiming a pass. The distinction between expected evidence and completed evidence prevents plans from turning into false implementation reports.

For prose and art review, note the content’s accessibility path. A player must be able to obtain essential written evidence without relying solely on small text embedded in an illustration. If an image carries unique information, the same fact needs a supported text interaction or accessible description. Localization review checks meaning, not word-for-word length: “unverified” must not become “false” in translation. Map labels and journal summaries can have different lengths but should preserve the same confidence.

The packet also records negative evidence. If no migration is required, state why existing saves cannot hold a partially completed form of this new chain. If no runtime change is needed, point to the currently verified consumer that already supports the behavior. If a dependency is deferred, state what the player sees until it exists. These are valuable constraints, not missing paperwork.

When content is removed or revised after review, update its trace and the affected references in the canonical files. Keep a short retirement note in the appropriate planning record if future authors could reintroduce the same collision. Do not retain dead IDs in runtime catalogs solely to preserve the narrative history of an unshipped proposal.

### Variation review and canonical fact protection

Every authored variation set needs a small invariant checklist. For the relay card, variants can change paper condition, readability, ink color, and surrounding clutter. They cannot change who wrote the interval, whether the date is present, or what the cut edge proves unless a separate authored variant explicitly changes those facts. A weather description may say the card is stiff with ice or softened by damp. It cannot silently remove a clue required by an active quest.

Reviewers should compare all variants side by side and mark each difference as presentation-only or fact-bearing. Presentation-only variants may be selected by conditions that do not alter quest logic. Fact-bearing variants need separate eligibility, proof, and fallback review. This distinction avoids accidental branch logic in descriptive text, where a phrase appears in one variant and is treated as evidence despite no corresponding state event.

The same review applies to generated combinations. A base location description and a weather suffix can combine only if every combination reads naturally and preserves the clue. Avoid stacking several fragments into incoherent prose or repeating the same sensory detail. If a generated line depends on a faction, character, or quest fact, verify that the combinations cannot produce a statement the speaker could not know.

When a variant is retired, compare its fact-bearing details with existing saves and active quests. If a player could have seen it, future text must not deny that history. If no durable visit or discovery record exists, do not create a new persistence layer merely to remember cosmetic variation; instead ensure the removed detail was not a meaningful clue. This gives content authors a practical test for whether a variation belongs in authored history or only in transient presentation.

### Ownership notes for review artifacts

Every planning table should identify whether it is canonical, generated, or editorial. Canonical content is the live JSON authority. Generated reports are reproducible outputs owned by their generator. Editorial tables explain design intent and must not be consumed by gameplay. If a review artifact begins to carry required facts, move those facts into the canonical data or withdraw the artifact.

This classification belongs in the document header or file map, not only in a handoff conversation. It prevents later maintainers from updating a spreadsheet while the game reads JSON, or from treating an index count as evidence that a record is reachable. When the canonical source changes, regenerate dependent outputs through their owner and review the resulting diff.

### Authoring freeze and change requests

Once implementation begins, freeze the identity and meaning of records for the active slice. Editorial improvements can continue when they do not alter conditions or effects. A request to change an objective, add a new faction consequence, or make a temporary location permanent reopens the relevant dependency review. Record the reason, affected references, new validation needs, and whether save compatibility changes.

A change request should name the player problem it solves. “Add a field for flexibility” is not enough. If an existing field can express the requirement, update the content within that contract. If an extension is necessary, identify the owner and state the smallest added behavior. The integrator can then decide whether the slice absorbs the change or defers it.

This discipline limits surprise late in production and helps writers avoid polishing lines for behavior that is not approved. It also preserves a clean record of which promises belong to the released game and which remain expansion concepts.

## Continuation pass 14 — Empty Shift authored record family, controlled variation, and review trace

### Record family objective

The Empty Shift is useful for this plan because its story begins with data that appears authoritative but may be stale. The content package must make that ambiguity intentional. A roster can be wrong in-world while its authored record remains valid. A player-facing maintenance note can be incomplete while the content graph still identifies its author, source, and interaction. This distinction protects narrative flexibility without weakening validation.

All records in this module are candidate content only. The existing JSON data authority, supported schemas, catalog validators, and runtime consumers control implementation. A record sketch below does not claim that the game currently supports its proposed fields. The first integration task is to map these concepts to the current data contract; unsupported needs remain design dependencies until the appropriate owner approves a change.

### Permanent authored content

Permanent records establish the stable story facts that all players can encounter:

- the shelter’s work board has a roster entry for the night shift;
- the Annex has a physical description and a set of inspectable surfaces;
- the words “covered” and “worked” have distinct procedural meanings in this local context;
- at least two characters can provide separate accounts;
- the player can choose an honest report outcome;
- the campaign does not require a single accusation to reach closure.

Permanent content includes stable identities for characters, locations, quests, evidence types, and dialogue scenes. These identities belong in the canonical catalogs once approved. The exact names may change during canon review, but runtime content must not depend on display prose as a join key.

**Roster object record.** The roster is a world prop or interactable content item only if the current location system supports that distinction. Its permanent properties are source role, physical condition range, interaction affordance, required text, and any supported evidence output. Its variable state may indicate which copy the player inspected or whether the board was moved, but only through a current location or quest owner.

**Work-note record.** A note has an authored speaker/source, creation context, intended audience, information it directly supports, and information it does not support. This lets the player hear a credible but limited account. A note saying “covered” does not automatically identify the replacement. The narrative contract can be expressed in prose annotations even when the runtime schema cannot encode all evidence metadata.

**Character records.** Mara, Oren, and Nessa are placeholders for distinct voices, not commitments to create three new survivors. The canon pass must search existing characters who could fill these roles. If suitable characters exist, add scenes to them rather than duplicate identities. Only if no current character serves the intended function should new identities be proposed. Each character’s dialog packet states what they personally did, what they were told, what they believe, and what they refuse to infer.

**Location records.** The Sleeve Board, Annex, Shelf Room, Service Niche, Bench, Intake Walk, Cook Passage, and Cot Bay are candidates. Canon review determines whether these become new locations, substages of existing hubs, or removed ideas. Stable physical facts remain authored. Weather, access, and clutter variations can respond to existing state without changing the identity of the site.

**Quest records.** Each quest has an entry path, player-facing ask, objective evidence, closure outcomes, dependencies, map needs, failure route, and effects. An optional branch is not required to produce a terminal outcome for the main chain.

**Dialogue records.** Every node must have a current speaker and location reference, eligibility condition, visible text, response set, effect list, and route continuation, mapped to the current actual schema. A missing optional node should not invalidate the whole package if the canonical consumer permits a fallback.

### Generated or conditional presentation

If the existing game supports authored variation, use it to respond to stable conditions without synthesizing facts. The board can appear dry, warped, or patched according to a weather or location condition that already exists. The same permanent roster entry remains recognizable. A room can be accessible or blocked if a current state owner provides that fact. A character line can vary based on what the player observed or which quest stage is active. These are deterministic selections from authored alternatives.

A variation record should state:

1. invariant fact that must remain true;
2. condition selecting the variant;
3. authored alternative text or interaction;
4. whether it is purely presentational or provides evidence;
5. effect and owner, if state changes;
6. fallback when the condition is unknown;
7. whether reloading should preserve the selected form or recompute it.

If a variant is purely presentational, recomputing it after load may be acceptable if it does not contradict what the player saw. If a visual condition exposes a clue, the player’s discovery must be recorded by the current quest or location owner. A clue cannot disappear after load while the quest still expects it to be known.

**Example: board condition.** A dry-board variant describes curled paper corners. A damp-board variant describes ink bleeding at the fold. Both present the same roster names and correction mark. Only an explicit inspect action records the mark as observed. Neither variant changes the work assignment by itself.

**Example: unavailable witness.** If Mara is absent, the player can read a previously authored note that explains the covered/worked distinction. This alternate route grants procedural context but not Mara’s personal relationship scene. It must not be generated from a generic fallback line that invents the content.

**Example: repeated visit.** When the board has been corrected, a return variant can show a fresh sheet or a marginal note. Its text is authored and selected by the actual board/quest state. If current owners cannot persist the correction, the return variant should remain local to the scene and avoid suggesting a durable world update.

### Conceptual record envelope

A planning record can be documented with a compact envelope while implementation fields are still being verified:

- stable proposal key, for planning references only;
- proposed canonical category;
- candidate ID and display name;
- source/canon anchor;
- permanent content summary;
- eligibility and exclusion rules;
- player-facing text surfaces;
- references to existing records;
- proposed effect and candidate owner;
- fallback behavior;
- removal or replacement plan;
- integrity checks required;
- profile and save expectations;
- production owner and review status.

The envelope is intentionally not JSON. It is not loadable game content. It makes missing decisions visible without masquerading as a schema. The actual record must be written only after the current schema is inspected and the exact content paths are claimed through the live workflow.

### Identity and content reuse

Reuse should happen at three levels. First, reuse a current character when the voice and history fit. Second, reuse a current location and add an interaction rather than a new map site when the physical space already exists. Third, reuse supported objective and dialogue shapes rather than creating a specialized runtime behavior for each evidence item.

Reusable does not mean generic. A roster interaction can share a schema with other papers while having unique content and outcome semantics. A character can support multiple quest conversations without becoming a dialogue vending interface. An Annex can be visited for a repair task and an investigation if the timeline and location state align. Reuse only where the player experiences a coherent place and person.

If the content shares an evidence item between quests, define its consumers. One inspection may advance the main investigation and an optional character quest. That shared fact should be established once and consumed by both eligible objectives without duplicate rewards. If the player can give the only paper to a character, later quests need a copy, report, or alternate route only if authored; never assume an item remains accessible after transfer.

### Authored uncertainty versus malformed content

This story depends on conflicting evidence, so validators and reviewers must distinguish intentional uncertainty from authoring defects.

Intentional uncertainty includes a source who only remembers a task; an unsigned log; a worn tool whose owner is unknown; two witnesses who saw different parts of the shift; and a location clue compatible with more than one explanation.

Defects include a missing speaker reference, a quest requiring a location with no fallback, a dialogue node reachable only if an untracked field is set, two outcomes that both claim the same exclusive board state, or text that calls a character absent when the condition selects their present scene. The content can contain uncertainty, but its rules must still be precise.

An authoring review should mark each statement as confirmed fact, witnessed observation, reported account, interpretation, or unknown. These labels can remain editorial if the engine does not support them. The player-facing lines should express the same distinctions. The validator checks objective references and graph reachability; a human continuity review checks that witnesses do not know facts they never encountered.

### Dependency and production ledger

Each proposed record links to a dependency row with the following questions answered: does it require a new location; does it require a new character; does it use an existing work, food, inventory, injury, or shelter state; does it alter any state; does it need a save migration; does it change UI; does it require an asset; can it be localized with the current pipeline; what is the fallback if its dependency is deferred?

A dependency is not approval. “Would use the shelter work system” remains unverified until a source audit finds the actual owner and proves its scope. If no work assignment owner exists, the content can still demonstrate the mismatch through authored records, but it cannot claim to reschedule survivors. This boundary protects the package from smuggling architecture through story prose.

Production cost should be counted by unique scenes, line variants, map points, interaction setups, assets, localization strings, and branch combinations. A short evidence note may require more validation than a long static journal entry because its provenance is consumed by several quests. Mark high-cost records early. Optional content with a high cost and little systemic value can be deferred without compromising the main story.

### Release trace and future audit

For one representative path, trace: player sees the board; inspects a mark; receives the corresponding quest fact; travels to the Annex or its fallback; records a physical clue; returns to Mara or reads the alternate note; chooses a report; sees the resulting board or quest closure; saves; reloads; and receives a later acknowledgement. At each step, identify the canonical file, consumer, owner, and observable result. A JSON record that cannot be reached through the host route is incomplete. A dialogue node that displays but does not apply its effect is also incomplete.

After integration, the package’s release receipt records exact changed content files, IDs, owning seams, validator command/result, focused verification, save/profile limitations, and whether the outcome is visible in a real play path. It does not label the proposal “implemented” based on presence in the index. If the content is removed, its dependent references are retired or redirected and the planning ledger marks the candidate as dropped. A new author should be able to tell that a role was considered and why it did not ship.


### Empty Shift content production contract

A feature with conflicting records needs a more precise content packet than a list of prose strings. The packet should explain which in-world document can be wrong, what the game knows independently, what the player can observe, and what state the player is allowed to change. This section proposes an editorial record family that can be mapped to current JSON. It is not a schema declaration.

#### Record family worksheet

**Story brief.** One paragraph describing the intended player experience, the main question, and the story’s practical stakes. It states that the roster discrepancy is grounded and that the game does not presuppose fraud.

**Canon map.** A list of current characters, locations, factions, resources, and quest systems searched for functional overlap. Each candidate is marked as reuse, extension, collision, or genuinely new design space. This step must use current repository evidence before names are finalized.

**Player entry.** Exact start conditions and entry surfaces. A board discovery, a character offer, and a quest prerequisite must converge without duplicate acceptance. If the current runtime cannot converge starts, select one canonical entry point for the first slice.

**Evidence map.** Every clue has source, interaction, claim supported, claim not supported, objective consumer, and fallback. “The panel is scratched” does not become “Oren worked the shift.” A roster line supports what was written, not attendance.

**Branch map.** Each choice states the immediate effect and any later consumer. A line that changes only tone is marked cosmetic. A promised board, resource, relationship, or assignment change names a verified owner or is deferred.

**Location map.** Each interaction is associated with an existing location or a candidate new one. The packet states whether it uses a map slot, a hub interaction, or environmental dressing. It lists alternative places only when they preserve evidence meaning.

**Dialogue map.** Each node declares speaker, location, context, text, responses, condition, effects, next node, and absent-speaker fallback. All fields are translated to the actual schema after inspection.

**Release map.** Changed file paths, stable IDs, validator results, focused runtime trace, save profile, localization review, accessibility pass, and handoff owner.

This worksheet is a human review surface, not runtime data. Its value is completeness and traceability. Once canonical records exist, the worksheet should be generated where tooling allows or explicitly marked as a stale-risk editorial artifact.

#### Sample conceptual records

The following examples use proposal keys only. They must not be copied into live data until the current ID convention and schema are checked.

**Proposal: shift-board interaction.** Category: location interaction. Permanent meaning: two roster copies show the same planned crew and a mark whose meaning is not established. Conditions: board is accessible; first inspection has not been recorded. Player output: the board was inspected and its visible content can be summarized. Does not establish: attendance or authorship. Fallback: character describes the same visible text at lower confidence. Effect owner: existing quest/discovery owner if supported. Removal path: fold into an existing shelter work-board interaction.

**Proposal: covered-mark clue.** Category: evidence clue. Permanent meaning: a mark may denote coverage rather than presence. Provenance: maintenance margin, not attributed to a speaker. Supports: that the notation has been used for coverage in at least one context. Does not support: who covered a specific night. Eligibility: found through a shelf-room interaction or an alternate character route. Fallback: defer interpretation and resolve the investigation as uncertain.

**Proposal: report-treatment choice.** Category: quest resolution. Permanent options: correct names with evidence, annotate distinction, request another account, or close unresolved. Invariant: no option may claim attendance that was not established. Effects: exact route mapped to current quest and board owners. Retry: not repeatable after closure unless a new source triggers an approved reopen. Save expectation: outcome survives if any later dialogue consumes it.

**Proposal: next-shift callback.** Category: dialogue follow-up. Conditions: report outcome exists and the callback milestone occurs. Fallback: journal/board closure if speaker absent. Invariant: callback describes only what current owners confirm. If no persisted outcome exists, omit the callback.

The examples reveal important omissions: no new relation score, no generated survivor biography, no general roster service, and no assumed food ledger. If the story later needs one of those, it becomes a separate decision with costs and ownership rather than an implicit field added to the packet.

### Content source precedence

When several files carry related content, define which is canonical for each dimension:

- canonical JSON owns gameplay identity and supported conditions;
- current Core owner owns state transitions and domain meaning;
- host adapter owns route/presentation conversion;
- localization authority owns translated strings where available;
- asset registry owns imported art references;
- planning documents own design intent before integration;
- generated indexes own derived navigation only.

No record may silently override another authority. For example, a map description cannot decide that the Annex is open if the location state owner says closed. A dialogue node cannot update meal stock directly. An editorial spreadsheet cannot add a character to the campaign.

Source precedence must include conflict behavior. If a canonical location is removed but a quest still references it, the catalog validator should catch the broken reference. If a localized string is missing, the established fallback is used. If an optional image is absent, the interaction remains understandable. If a saved quest outcome references a retired result, the migration policy must be explicit.

### Authored variants and constrained composition

An authored variant set can support procedural freshness while preserving story facts. For each set, define a base entry and variant dimension: moisture, wear, light, crowd state, or season, only if that input already exists. Avoid combining two fragments that repeat an observation or contradict each other. A roster description can vary the paper condition while the task line stays fixed. A hub greeting can differ because Mara is working or off shift, if those states are supported.

Generated composition must remain deterministic when its result affects discovery or objective proof. If a procedural selection changes only decorative wording, current deterministic conventions still apply, but no save state may be required. If it changes which clue is visible, the selection becomes gameplay-significant and must be reproducible across save/restore or persist its outcome through the proper owner. Do not rely on the same string seed to imply that an objective was completed.

For each combination, author at least one reviewed full sentence. Avoid free-form concatenation of nouns and adjectives that can produce awkward language or incorrect facts. If the set becomes too large to review manually, reduce variant dimensions rather than add a general text-generation system.

### Reference and validation checks

The current integrity pipeline should be used or extended only through an approved package. The design’s desired validation rules include:

- unique, convention-compliant IDs;
- all hard references resolve;
- optional references have a safe fallback;
- every quest has at least one reachable entry and terminal route;
- required location dependencies have equivalents or a delay outcome;
- all dialogue effects are recognized by current consumers;
- conditions reference known data fields or approved runtime facts;
- permanent identity is not duplicated across variants;
- generated alternatives do not alter invariant evidence;
- every required player-facing string has a localization route;
- retired proposal keys do not accidentally enter canonical catalogs;
- authored IDs remain stable through harmless display-name edits.

A content row can pass syntax validation and still be unreachable. The future package should include an event trace from game start through the host route. Presence in JSON is only one stage of integration.

### Save profiles and migration questions

The Empty Shift should be additive if possible. Profiles to consider include a player who has never seen the board; one who inspected it before accepting; one with an active quest but no evidence; one who learned “covered” from a note; one who chose an unresolved result; one whose named worker left; one who played through a saved version before new content arrived; and one who completed a related shelter task in existing content.

The save review asks: what existing state can safely determine eligibility; which new result must persist; whether older saves need a default; and how a missing optional result is handled. A default must never choose “corrected” or “verified” on the player’s behalf. If no state distinction is needed after closure, avoid persisting dialogue history. If migration is required, use the current save owner and its deterministic rules.

### Content lifecycle and retirement

Proposal to implementation transitions are explicit: concept, canon-checked, schema-mapped, approved for implementation, authored in canonical data, validated, reachable, release-ready, or retired. These are governance labels and do not become runtime quest states. A proposal can be retired because it duplicates current content, demands unsupported state, adds excessive production cost, or fails canon review.

When retiring a candidate location, search all quest, map, dialogue, and localization references. When replacing a character, check each line for the old character’s personal knowledge and relationships. A replacement is not a string substitution when the source relationship changes. Keep authored history truthful: a player must not be described as speaking with someone they never met.

### Release rehearsal

The content author prepares a small test profile and traces the following path: the player discovers the board; accepts or declines; inspects the Annex or fallback; hears a first-person account; compares a record; selects one outcome; receives acknowledgement; saves and reloads; and returns for a callback. Each step is linked to a real data record and runtime consumer after integration. The validator output is attached, as is the focused verification report.

The release note states limitations in plain terms. For example: “The board correction is narrative-only; it does not reassign shelter workers because no current owner exposes that behavior.” That is a truthful scope statement, not an invitation to make the UI imply otherwise. If a later integration adds work assignment, the release trace is updated with the exact owner and observable result.


### Content package traceability and safe editing

A large content expansion is maintainable only if each addition can be found, reviewed, validated, and removed. The Empty Shift package should define a trace from concept to player outcome without making the planning documents a second catalog. The trace can be represented in a small editorial table during design and replaced by generated metadata or repository-native validation if those tools already exist.

For each story item, identify:
- proposal source and pass;
- canon review state;
- candidate canonical ID, if approved;
- actual target data path after ownership is claimed;
- schema version and migration expectation;
- direct and transitive references;
- owning runtime consumer;
- player-facing route;
- save/restore obligation;
- fallback/removal instruction;
- content and technical reviewer;
- final integration status.

An item with no runtime consumer remains a narrative proposal. A dialogue node with no route remains a script draft. A location with no map or scene interaction remains prose. The index can describe these items, but must not label them integrated. Status is updated only through an accountable handoff.

### Safe editing contract

Before editing a canonical content file, the builder checks the active integration queue, exact path ownership, current schema, and existing local changes. Shared data catalogs are not edited concurrently without the named integrator. For a documentation-only proposal, append or edit the owned plan and index only. Do not modify generated content maps or manually patch generated code.

Changes should remain localized. Add one coherent content family, validate IDs and references, and inspect the resulting diff. Avoid mass-formatting unrelated JSON. Preserve existing fields and comments unless the schema owner approves a rewrite. When a reusable record is extended, document why the new conditions are compatible with existing consumers.

A bulk authoring script may help only if the repository already has a supported generator or the user explicitly authorizes new tooling. Do not generate many nearly identical records from a template before proving that the content design is stable. Automation can replicate a flawed premise at scale.

### Required and optional reference policy

The record packet distinguishes:
- **required reference:** without it, the record cannot load or present;
- **progression reference:** without it, a critical route fails;
- **optional narrative reference:** omission reduces flavor but not completion;
- **derived reference:** obtained through an existing owner or generator;
- **legacy reference:** retained only to interpret an existing save or migration.

A missing required reference blocks release. A missing progression reference needs a safe, authored fallback. An optional narrative reference can fall back to a short generic response, but only if the current schema has a supported fallback and the meaning remains truthful. A legacy reference needs a bounded retirement date or migration proof so it does not become permanent dead weight.

The player should not see developer-facing labels such as “optional reference.” They should see a natural account: Mara is away, but her note remains; the Annex is closed, but its outside panel can still be checked. The fallback is written as content, not as a generic error string.

### Data shape review without speculative schema

A design packet should state what information the engine needs, but it cannot dictate field names until the live schema is inspected. For a clue, the conceptual information is identity, location, interaction condition, text, evidence meaning, objective consumer, and fallback. The current catalog might express this through a quest objective, encounter, inspect record, or another existing structure. The plan must accept the current authority’s shape if it is semantically adequate.

If a schema extension is proposed, show:
- a real current record that cannot express the feature;
- the smallest additional concept required;
- how existing records default safely;
- migration requirements;
- every consumer affected;
- validation additions;
- a path to remove the extension if content is cut.

Do not add a generic “metadata” field that can hold arbitrary values. A flexible blob weakens validation and shifts interpretation into runtime code. Typed data should be added only for a proven repeated requirement.

### Authoring anti-collision review

Collision is assessed by role and interaction, not by name. The Empty Shift may overlap with existing shelter work, survivor memory, ration, or maintenance content. Search current plans and data for:
- records already used for shift planning;
- an existing shelter board or roster;
- a survivor with a similar work-history arc;
- a quest about food allocation or named portions;
- a location containing the proposed Annex function;
- mechanics for assigning or changing work;
- a journal or note system already handling evidence provenance;
- late-game governance content about record standards.

If an existing plan has the same intent, this package becomes an extension or is retired. If a current system can express the task, use its owner. Avoid adding a second “roster quest” that competes with established shelter management.

### Localized content and prose budget

A dialogue-heavy content family has costs beyond total word count. Count unique source strings, condition-specific variants, short UI labels, journal summaries, map hints, recorded notes, accessibility descriptions, and repeated lines. Each variant increases translation and QA work. Reuse exact strings only where meaning and speaker intent truly match; otherwise share a translation key only if the current localization system supports context.

Maintain a short map label and a longer journal description. The map can say “Service Annex”; the journal can say “The exterior panel is accessible, but the room is locked.” A spoken line can be more personal. Do not trim the journal until it loses its evidence boundary. Avoid essential text inside a texture or illustration without an accessible companion surface.

Localization review checks that “planned coverage” does not become “completed shift,” that “unconfirmed” is not translated as “false,” and that “covered” retains the intended procedural meaning. Provide translators with context and a distinction glossary, not only strings. The current localization owner sets the final workflow.

### Catalog integrity review plan

The data-quality review should cover:
1. IDs follow current naming authority.
2. IDs are unique across the right scope.
3. References point to approved canonical records.
4. All required fields and ranges match the live schema.
5. Objective and dialogue graphs are reachable.
6. Conditions are supported by current consumers.
7. Terminal outcomes cannot conflict.
8. Optional content has a defined omission behavior.
9. Required route references have an equivalent or delay result.
10. Dialogue effects map to one owner each.
11. Localization keys resolve.
12. Asset paths use the current registry.
13. No generated record is also manually authoritative.
14. No inactive proposal key enters the runtime catalog.
15. Save profiles have no impossible new default.

The current validator’s output should identify individual records and references. If it cannot validate a planned property, that property remains a human review item. Do not claim a broader validation pass than the tool actually performed.

### Golden content fixtures and discoverability

A future content slice can define small representative campaign fixtures: clean new game; previously discovered board; missing Mara; an unresolved result; and a branch with new evidence. These fixtures help review availability and text. They are not necessarily save files or new tests. The current test policy and data fixture conventions decide how verification is implemented.

Discoverability review asks whether a player can find the request without already knowing the design. The task may begin at the board or through Mara. The clue that introduces the Annex should appear in a reliable way. Optional hidden content remains optional. A main objective should never rely solely on an ambient bark, a tiny visual mark, or an obscure skill gate.

### Versioning, migration, and old saves

If the content is additive, existing saves should naturally lack its quest facts and remain unchanged. New content availability must use defaults that do not mark the player as having investigated the roster. If a new story consumes an existing location-discovery fact, confirm that the old save’s representation is compatible.

If the schema version changes, use the canonical migration process. Migration must not infer a player choice from unrelated facts such as a prior visit to the shelter. If the new objective uses an existing field with a changed meaning, do not reuse it without a migration design. Existing save data should remain readable even if optional content is removed later.

A save profile with the investigation active at release should resume to the same objective, preserve completed evidence, and show the same outcome choices. A save created after closure should not reopen the quest unless the new release contains an explicit reopen event. A saved unresolved result remains unresolved until new evidence is introduced.

### Content release handoff

The handoff receipt includes purpose, canonical records added or extended, exact paths, owner seams, validator commands and outcomes, focused runtime evidence, known unavailable effects, save compatibility, localization status, asset status, rollback/removal behavior, and remaining proposal-only content. The receipt identifies who confirmed each seam. It distinguishes “content present” from “player can reach it” and “effect visible” from “effect persisted.”

No production code or game data changes occur as part of this planning pass. When promoted, each task must follow INTEGRATION_PLANS.md, WORKTREE_OWNERSHIP.md, TEST_POLICY.md, and AI_AGENT_WORKFLOW.md. Any architecture decision beyond an existing owner requires the designated authority rather than an improvised local system.


### Production review: status, risk, and cut strategy

The Empty Shift package has a wide range of possible content, but the production release should be staged. A status grid prevents optional material from being mistaken for required integration.

**Slice A — narrative foundation.** Board interaction, one clue, one main quest entry, Mara or an existing character, and unresolved closure. Dependencies: current quest and dialogue owners. Risk: low if existing schemas represent the interaction.

**Slice B — location evidence.** Annex exterior, a service-panel clue, one alternate source, and map fallback. Dependencies: location catalog, expedition candidate owner, map UI. Risk: moderate because availability and discovery must agree.

**Slice C — character and side content.** Oren/Nessa/Sella scenes, Quiet Count, glove task, optional hidden note. Dependencies: character presence, relationship/food/item state only when used. Risk: content volume and continuity.

**Slice D — cross-system effects.** Board edit affecting work assignment, food allocation, shelter service, or campaign policy. Dependencies: exact domain owners, save path, balance, migration, deterministic behavior. Risk: high until source evidence proves the contract.

Do not bundle Slice D with the prose and location work merely because it is thematically connected. The story can ship at Slice A or B with truthful limitations. A later mechanics package has a clearer acceptance boundary and can be declined independently.

### Risk register

| Risk | Evidence to seek | Mitigation |
|---|---|---|
| Duplicate current roster or work-board feature | Canon registry, current data, UI/source consumer | Reuse or retire proposal |
| Existing character collision | Character catalog and narrative continuity | Adapt voice/history rather than clone |
| Ambiguous clue reads as proof | Cold read and objective mapping | Rewrite journal/dialogue boundaries |
| Critical route depends on one NPC | Availability profile | Add note/location fallback |
| Too many shelter pins | Map capacity and playtest | Fold interactions into existing hub |
| Resource consequence has no authority | Core owner and save registration | Keep result narrative-only |
| Quest outcome not saved | Quest save contract | Scope down or plan owner change |
| Text visible only in asset | Accessibility/localization review | Add supported text interaction |
| Repeated task becomes grind | Cadence and reward review | Ship static variants or remove |
| Existing saves misread a new fact | Save profiles and migration evidence | Additive defaults or explicit migration |
| Content package exceeds capacity | Production estimate | Cut optional slices |
| Generated variation changes evidence | Variant invariant review | Reduce to presentation-only variation |

Risk labels are provisional until current source and content are inspected. “Low” is not a substitute for evidence.

### Branch and reference graph audit

The package should contain a graph with nodes for quest starts, evidence interactions, dialogue scenes, location outcomes, and terminal result bands. Each edge lists the condition and target. An automated or manual review should catch:
- unreachable nodes;
- terminal states with unresolved required references;
- choices that have no response path;
- response path that starts the same quest twice;
- location reference with no fallback;
- alternative routes that yield contradictory confidence;
- callback that fires before its cause;
- branch that references a character who can never be present;
- effect that has no consumer;
- optional clue that accidentally becomes a required prerequisite.

Graph reachability alone is not enough. The author should inspect evidence provenance: two clues must be independent if they are counted as corroboration. If both came from the same person’s copied note, they are related sources. Content validators can check references; a human reviewer determines source independence and narrative meaning.

### Data migration and versioning scenarios

**Additive records only:** older saves ignore new quests, unless a discovery fact already makes them eligible. No migration is expected; prove that from current save reconstruction.

**Extension of an existing quest:** old saves may have the quest active or complete. Define how the new dialogue and outcomes behave for each state. Do not reset the quest.

**New location interaction on an existing site:** old saves may already have visited the site. Decide whether the new interaction is discoverable on return or requires a new clue. Do not mark it seen by default.

**New branch consuming old reputation/relationship state:** verify semantics and thresholds; do not infer a specific relationship outcome from a broad score without approval.

**Changed meaning of a field:** requires explicit migration and current save owner. This should be avoided for an expansion content slice.

**Retired content after partial play:** preserve saved outcomes or provide a compatibility path. Removing content from a catalog cannot cause a saved active quest to disappear silently.

A migration table should list old representation, new representation, deterministic transformation, fallback, and evidence that older saves can contain the old form. If no evidence exists, do not invent a migration task.

### Localization and copy review packet

The prose includes terms that can become ambiguous in translation: planned, covered, worked, confirmed, attendance, and record. The localization packet defines each term with a short in-world example. “Covered” means someone took responsibility for a shift; it does not necessarily mean the person physically performed the task. “Worked” denotes performed work, but not necessarily a full shift. “Confirmed” refers to evidence, not character confidence.

Map labels use short concepts; journal prose can preserve the distinction; dialogue can be less formal and character-specific. Translators should receive speaker and scene context for every line. Repeated English phrases may not translate to the same structure, so avoid assuming line reuse is free.

Do not put text inside textures as the only representation. If the roster is rendered as an image, the inspect interaction or another current accessibility surface needs a text equivalent. If line length forces abbreviation, retain the key confidence distinction in the short form.

### Content analytics without new telemetry

Before asking for telemetry, use qualitative review and deterministic fixtures. A small content test can list whether a candidate is reachable under each profile. If the game already logs selection, use the established privacy-safe diagnostics. Do not add player tracking to answer questions that can be resolved by code inspection or focused manual play.

Playtest questions include: what did players infer from the roster; which evidence did they treat as independent; did they understand “covered”; did they know they could close unresolved; did they confuse a locked Annex with a dead quest; did the interface expose the report’s consequence; and did optional scenes feel relevant rather than mandatory? Record observed behavior and quotes according to project policy, but avoid treating one participant’s conclusion as a balance metric.

### Removal and rollback card

If the package is cut, remove or leave only:
- unshipped proposal prose in this plan;
- canonical content records that no release has used;
- optional assets with no remaining reference;
- IDs and links from the plan index or generated catalogs through their owner.

If the package has shipped, remove only through a versioned content/save plan. Preserve any outcomes that existing content or saves consume. Redirect active objectives to an authored closeout. Do not delete a save fact because the original location was removed from the next build.

The rollback receipt records what was disabled, how active saves behave, which dependencies remain, and which canonical files changed. No destructive cleanup is authorized by this plan.

### Production readiness checklist

The package is ready for an implementation work item only after its canon review, schema mapping, exact file claims, objective proofs, fallback, location selection, condition sources, effect owners, save path, localization/accessibility review, content integrity validation plan, focused test plan, runtime route, and rollback behavior are all known. Anything beyond the existing architecture is submitted as a decision request with player value, dependencies, cost, and alternatives.


### Content sample audit: one clue through every authority

Use the correction mark as a content audit sample. The planning record says what the mark might mean. The canonical data row, once approved, defines the interaction and stable identity. The dialogue scene describes what Mara knows. The quest owner records that the player inspected it. The journal displays what the observation supports. The map shows the board only if the player knows its location. The save owner restores any outcome that a later scene needs. Each layer has a separate responsibility.

A sample audit should reject any of these shortcuts:
- dialogue directly sets a board state instead of invoking its owner;
- journal text claims the player saw a mark before interaction proof exists;
- asset text is treated as a canonical clue without an inspect interaction;
- location candidate tags are copied into multiple registries;
- a generated variant changes the mark’s meaning;
- a save migration guesses that every shelter visit included an inspection;
- a validator passes the ID but no host route can reach it.

The audit also checks whether a different clue source can serve as fallback. The margin note can explain the mark’s possible use, but it is not the same source as the roster. If the player finds the note first, the next scene should consume that knowledge and avoid presenting it as a new discovery.

### Change ownership and review routing

When reviewers request a change, route it to the field or owner that controls the issue:
- identity collisions go to the canon/content owner;
- schema limits go to the data authority owner;
- condition semantics go to the runtime consumer owner;
- quest proof goes to quest lifecycle owner;
- map selection goes to expedition/location owner;
- save behavior goes to save owner;
- presentation and focus go to UI owner;
- text or voice goes to narrative/localization owner.

A planning document can explain the issue but cannot assign an implementation path. If two owners disagree, pause for the named integrator or architecture decision. Do not “fix” the content by duplicating state in the least resistant layer.

### Release trace minimum

A release handoff should identify the changed files, approved content IDs, branch/outcome list, current consumers, save impact, localization and accessibility status, validator invocation, focused verification invocation, and known deferred features. It should include one reproducible path from start to callback. It should also state which proposed content was removed before release and why.

This receipt allows the next team to continue without rereading every planning paragraph. It records implemented facts only. If the release contains only the narrative-only slice, the receipt says that no survivor schedule or meal values change. Future system plans can then build from a truthful baseline.


### Regression watchlist

Before merging a future content package, compare the new records against active catalogs and continuity anchors, then check for broken references, changed IDs, stale localization keys, inaccessible variants, and unused content. Review the generated integrity report if the repository provides one. If the new content changes a shared schema or consumer, the appropriate owner includes the focused compatibility check.

After release, a follow-up audit can ask whether players reached the main investigation, whether unresolved outcomes were understandable, whether the optional content was worth its production cost, and whether any record variant contradicted the physical world. These questions guide content revision; they do not justify collecting new analytics without an approved privacy-safe route.

A regression found in prose can be fixed in the canonical content. A regression in a state owner needs an implementation work item and exact path claim. Do not disguise a runtime defect by changing text to imply that the effect never mattered.

### Documentation and data drift check

After implementation, compare the plan’s promised behavior with the canonical records and runtime route. If the shipped slice changes an outcome, update the plan’s status and integration receipt so the next author does not design against an obsolete proposal. Keep proposal text clearly separated from verified implementation facts.

An index should report measured file counts and current pass delta, but counts do not prove quality or reachability. The release receipt links to the authoritative data and focused evidence. If a proposed field was not implemented, state that the package was scoped down; do not leave the index implying that the feature exists.

### Evidence quality in the content ledger

For each clue, record whether it is a direct observation, an authored document, a first-person account, a second-hand report, or an inference. Note whether another clue shares the same source. This editorial evidence map prevents writers from accidentally treating a copied note as independent corroboration.

The map is not a runtime confidence model. Its purpose is to let the quest owner and narrative reviewer understand what each branch can claim. If confidence later becomes a gameplay value, it requires a real repeated consumer, a current owner, and a separate architecture decision.

### Record authority reminder

A proposal key is not a shipped ID, a planning ledger is not gameplay data, and an authored variation is not a generated fact. Keep those boundaries visible in every handoff. Promote records only after the actual schema, consumer, validation route, and path owner are verified.

## Pass 15 — Provenance contract for authored and generated story content

**Status: PROPOSAL, premise-gated.** This pass turns the bible's JSON authority and data-first expansion rules into a content boundary contract. It recognizes that authored locations, encounter definitions, quest templates, generated quest instances, runtime state, and save payloads already exist in several owners. The goal is a traceable boundary between them, not a new registry or generation framework.

### 15.1 Previous content collision correction

The Empty Shift story and its location circuit are not accepted as a separate duty-roster, quest, character, or location family. DutyRosterSystem, DutyRosterQuestRuntime, duty-roster catalogs, QuestRuntimeCoordinator, the existing quest families, the world map catalog, and micro_locations.json are current evidence that must be searched before any record is promoted. Earlier prose can survive as a scene draft only if its characters, work behavior, geography, and consequences fit current canon and do not repeat an existing record. Keep all unresolved material marked PROPOSAL or DRAFT.

### 15.2 Four data classes

| Class | Identity | Authority | Lifetime |
|---|---|---|---|
| Permanent authored definition | Stable catalog ID, schema version, reviewed text and references. | Existing JSON catalog plus its current loader/validator/consumer. | Across releases; edits require normal compatibility and continuity review. |
| Authored variation rule | Stable rule/template ID plus bounded options or gates. | Existing catalog family or an explicitly approved additive extension. | Versioned with its parent content. |
| Generated runtime instance | Instance ID, source definition/template, seed and bound existing entities. | Existing generator during creation; existing QuestRuntimeCoordinator or specialized runtime owner after handoff. | Session or save lifetime according to that owner. |
| Presentation projection | Localized/rendered text and visible state derived from current facts. | Existing host/read-model/UI path. | Rebuilt from the authoritative instance; not gameplay state. |

A generated instance may select an authored definition, bind an existing item/location/character/faction, and vary copy within an authored grammar. It must not synthesize canon by creating unreviewed permanent entities or persist the same mutable outcome in two owners. If a new fact needs long-term persistence, the plan names the existing save-section owner or marks the work blocked for a signed architecture decision.

### 15.3 Content envelope

Each authored record should be auditable without inventing a universal mega-schema. Use the schema already owned by its catalog. In the plan and review packet, record these common metadata fields whether or not they are physically stored in JSON:

- Stable source path and existing record ID, or “candidate with no ID assigned.”
- Evidence status and last premise-audit date.
- Content class: authored definition, authored variation, generated instance, runtime projection, or save state.
- Owner catalog, loader, consumer, and validator.
- Preconditions expressed with current supported schema features.
- Existing entity references and their canonical IDs.
- Localization/text-key source and prose approval status.
- Whether the record is mandatory, optional, secret, temporary, repeatable, or one-shot.
- Generation seed/version behavior if generation is used.
- Save implications, including whether the state is reconstructed or persisted.
- Retirement/replacement plan and branch compatibility notes.

This is a review envelope, not an instruction to add duplicated metadata fields to six catalogs. Choose one existing mechanism for each concern after checking the code, schema, generated-document rules, and data authority.

### 15.4 Deterministic generation boundary

The current DynamicQuestGenerator source includes a path that asks ProceduralNarrativeSystem to generate a candidate using a NarrativeWorldSnapshot and ISeededRng, then registers that accepted candidate with QuestRuntimeCoordinator. Treat this as a concrete precedent. Do not infer that every generated encounter, destination, or dialogue choice already follows the same path.

For each generator, freeze the minimum inputs that can change the result: stable world snapshot, eligible template IDs, existing entity candidates, seed stream, algorithm/content version, and any authored weights. Candidate enumeration is sorted by stable ID before seeded selection. Generation must not read host frame timing, wall clock, process-randomized hash order, UI focus, or a network response. A replay of the same seed, input snapshot, and content version should return the same choice and bindings.

A generated instance records enough origin information to be explainable: which authored template was used, what existing location and actor IDs were bound, the generation day, and the seed/version already supported by its owner. Do not add another parallel “generation provenance database.” If the existing instance cannot retain a required provenance fact and that fact is needed to reproduce or debug a save, open a bounded owner extension with capture/restore and migration criteria.

### 15.5 Authored truth and generated surface text

Generated wording may vary sentence order, document genre, or a few authored phrase choices. It may not alter who owns a location, when an event occurred, what a faction believes, whether a character is alive, or the result of a prior decision. Those are authored canon or current world facts. A generated statement about an authored event must be checked against the event's timeline and perspective.

Recommended variation layers:

- **Layer A: Stable authored fact.** Exact historical event, required objective, item identity, faction relation, or known outcome.
- **Layer B: Authored scene lens.** One of a reviewed set of voices or documentary forms that describes the same fact.
- **Layer C: Bounded runtime binding.** Existing actor, location, resource, day, or threat band selected from valid candidates.
- **Layer D: Optional surface variation.** Seeded short phrase, sensory detail, or question order that cannot change gameplay meaning.
- **Layer E: Consequence.** A command routed to its canonical owner, never an arbitrary text-generated effect.

Layer D must be removed when it creates ambiguity, localization problems, repeated phrasing, or a false promise about optional gameplay. Mechanical requirements and rewards must never be parsed from generated prose.

### 15.6 Permanent and temporary geography

A permanent authored location has stable identity, map/catalog membership, author-reviewed history, and canonical route references. Its state can change—blocked, damaged, contested, evacuated—through its existing evolution/world owner without mutating its historical definition. A temporary event destination can be a presentation state or an encounter attached to an existing location if that is what current systems support; it should not become a permanent map node merely because a random event referenced it.

Micro-locations already have a dedicated micro_locations.json loader. New ideas for a shack, culvert, cache, station room, or storm shelter should first be tested as: existing node plus new encounter; existing micro-location record plus additional route evidence; or a genuinely missing permanent node. Only the last case requests a location catalog addition, and that request must include graph edges, valid destination selection, region, entry routes, hazards, map visibility, discovery, reuse, and fallback.

### 15.7 Generated-data save policy

Keep immutable definitions out of save files. Persist only runtime facts that cannot be reconstructed without changing the player's result: accepted quest instance, selected authored variant when the player has seen/committed to it, objective progress, terminal outcome, and any owner-specific anti-reroll receipt. A candidate that has not been shown or accepted may be regenerated if the existing owner guarantees the same deterministic result; otherwise the owner must define why the choice is frozen.

On restore, validate references before applying a generated instance. Missing authored definitions should lead to a compatibility fallback or an explicit load diagnostic, never a silent retarget to unrelated text. Unknown fields remain governed by the current codec policy. Any schema addition requires a versioned migration from supported saves and an exactly-once review for events that might be replayed around save boundaries.

### 15.8 Content gates and release evidence

Structural gates prove unique IDs, schema version, valid enum/range values, reference resolution, map-node existence, and duplicate detection. Utilization gates prove an authored definition is reachable by a live producer and consumer. Continuity review proves characters know only available information, timelines agree, factions possess what they trade, and no ending gains a fact from an unseen branch. Playability review proves the record produces a legible decision and has a fallback when optional content is absent.

Required release evidence for a content tranche:

| Evidence | Pass condition |
|---|---|
| Catalog ownership | One identified source file and one current loader. |
| Reference integrity | Every item, location, quest, actor, faction and knowledge key resolves. |
| Reachability | At least one verified producer can surface each required record. |
| Determinism | A fixed snapshot and seed reproduce generated bindings. |
| Save review | The owner either reconstructs the state or has explicit migration and round-trip coverage. |
| Narrative review | Voice, timeline, knowledge, setting constraints and outcome text agree. |
| Accessibility review | Map and dialogue state is conveyed by readable text and focus behavior. |
| Performance review | Candidate pools and content lookups are bounded and measured before optimization. |
| Retirement | Replacement/deprecation preserves old-save and old-reference behavior. |

### 15.9 Example of the boundary in play

A proposal calls for a cold-weather service request with one required site and two possible clues. The authored part is the request's stable purpose, witness position, map hint family, valid objective alternatives, terminal outcomes, and reviewed prose. The generated part may bind one eligible site from a reviewed existing set and one eligible witness already in the cast. The runtime instance stores the chosen IDs and seed only through the quest owner. A dialogue line cannot invent a new faction or claim a substitute site contains the original evidence. If neither site is eligible, the request waits with a visible reason or uses a pre-authored alternate; the generator must not create a third unreviewed site.

### 15.10 Promotion checklist

Before DRAFT becomes candidate data, identify its source catalog and schema; before candidate data becomes shippable, verify the real loader, consumer, and integrity rule; before state becomes persistent, identify the save-section owner and migration; before a generated variant becomes replayable, identify its stable RNG stream; before a location becomes map-visible, prove a valid graph route and selection path; before a branch becomes canon, audit every reachable ending and old save. This plan records the intended evidence. It does not claim that any of those gates passed in this pass.

### 15.11 Record examples

**Authored encounter attached to a current site.** The stable content is a reviewed question, a set of responses, their supported effects, and a narrative source. The encounter references an existing location only after ID and consumer verification. If one line varies by weather, the weather fact comes from WeatherSystem or its current host projection; the record does not create another weather cache.

**Generated quest candidate.** A live narrative template selects an existing location and resource from a sorted eligible set using the injected seeded RNG. The generated instance records the template and bindings under its current owner. The prose can say “the relay above the drainage cut” only when the selected location's authored description supports it. If the record is rejected by QuestRuntimeCoordinator, the template authority must not display it as accepted.

**Dialogue projection.** The authored response key stays stable while the visible wording can depend on an existing relationship band or knowledge fact. The presentation layer resolves text; it does not store a second “NPC remembers this” entry. Any future callback reads a canonical prior event through its owner.

**Temporary event.** A weather or faction event changes eligibility at a specific time. The authored location remains permanent; only the event window changes. When the window ends, the selector removes the temporary role and leaves normal map discovery and history unchanged.

These examples are intended to clarify ownership; they are not proof that each named system currently exposes the required field or event.

### 15.12 State transition between authoring and play

Reviewers should distinguish an editorial state from a game state. Editorial states may include idea, canon review, DRAFT, premise audited, data candidate, approved, integrated, deprecated, or retired. Game state remains in the owner's runtime enum. Do not serialize “approved” into a save and do not let a catalog's editorial status control live eligibility unless an explicit shipped field is part of the schema.

A content change follows this chain: concept; canon collision search; owner and schema identification; record drafting; structural validation; reference validation; utilization proof; narrative/branch review; deterministic/runtime review; focused owner verification; release note and index update; then integration closeout. Rejected ideas remain out of authoritative JSON. Superseded content is removed through an explicit ID/migration policy, not by editing a save's generated instance invisibly.

### 15.13 Generated content rejection reasons

The generation pipeline needs inspectable rejection categories, even if the current API names differ: no template eligible; all candidates invalid; required location missing; faction access closed; actor unavailable; objective incompatible with expedition; unsupported effect; duplicate instance; failed schema validation; output exceeds size budget; or owner registration refused. Each rejected draft must either produce a user-safe reason or remain an internal diagnostic. A rejection cannot consume the same RNG stream unpredictably if it is retried; the generation owner should define whether rejected candidates consume a draw and prove the choice remains deterministic.

Never “fix” invalid generated content by swapping in an arbitrary first catalog row. That can silently change faction, geography, resource economics, or quest meaning. Fallbacks must be authored equivalence groups or a clear no-content outcome.

### 15.14 Stability and content versioning

Stable ID is the identity of a permanent authored record; title text is not identity. Changing prose does not create a new ID unless references or player-facing history require distinct treatment. Changing objectives, reward semantics, location, or effect meaning may require a new definition or explicit migration. The content version is tied to the authoritative schema or a dedicated current version contract; do not insert an unofficial version integer into every record.

Generated instance reproducibility has two valid policies: freeze the accepted instance at the moment it is shown/accepted, or recompute it from inputs that are guaranteed stable and persisted by the owner. Do not mix policies in one quest family. If a balancing update changes an authored template, accepted instances should preserve the committed objective and reward meaning or migrate under a documented rule. A new version should not retroactively grant a better reward or strand an old target.

### 15.15 Localization and text assembly boundary

Permanent prose should be stored in the current data authority and use the localization arrangement adopted for that content family. Do not concatenate arbitrary generated fragments where grammar, gender, number, context, or translation order becomes unpredictable. For localized variants, author whole sentence alternatives or structured slots with agreement metadata only if the current localization contract supports them.

Identifiers, journal facts, and hidden condition names should not leak into localized text. Text assembly may bind visible names from canonical catalogs, but missing names should fall back to a neutral authored phrase and emit a diagnostic. Never display raw null/empty keys as story prose.

### 15.16 Data growth and performance budget

A content plan should estimate record count, fields per record, text volume, average candidate count, maximum active instance count, and repeated lookup path. Large prose catalogs should load through the existing content loader or a proven lazy path; do not add per-frame full-catalog scans. Cache immutable parsed indexes only where the current owner does so and has invalidation rules. Measure startup time, peak allocation, selector latency, save size, and UI refresh cost before optimizing.

Set a bounded acceptance target for each content family. The appropriate number is derived from measured current catalogs and design capacity, not “hundreds more” by default. Every added record needs utilization value, unique player purpose, reviewer capacity, and localization cost. A small collection of distinct, reusable scenes can outperform a large catalog of near-duplicates.

### 15.17 Definition of complete

This plan is complete for a particular content tranche only when the selected records have a canonical owner; authored and generated facts are separated; no runtime authority is copied; the scenario has a reachable producer; all references validate; player-visible text matches the selected bindings; the RNG and save behavior are explicit; old content and saves have a migration policy; and the actual consumer has a focused verification. A plan is not “integrated” because its pages have been drafted or because JSON parses.


## Pass 16 — Provenance contract for signal, acoustic, and shelter-memory stories

This pass applies the master world bible’s expansion subjects to the authored/generated boundary. It distinguishes stable fiction from replay variation and ties each recommendation to a currently visible content owner. The contract is deliberately conservative: records already present should be audited and connected before new catalogs or procedural templates are proposed.

### Current source landmarks

The source snapshot reviewed for this pass has three authored cipher chains in CipherQuestChainEngine, with state capture and restore; the numbers-station corpus is loaded through SignalIntelligenceCatalog. Hydrophone acoustic logs are read by AbyssalAnomaliesCatalog and projected through AbyssalAnomaliesProjection. NarrativeDiscoveryCatalog and the discovery manifest contain hydrophone and folklore references. DailySurvivalCatalog loads the base folklore catalog and batch two. Do not infer that a similarly named expansion file is active merely because it exists in the data tree: trace its loader, validator, and consumer before integrating its records. The existing host/session narrative owners and journal projection are the route to verify for presentation.

### Content authority layers

For each story packet, preserve a chain of provenance:

- **Authored observation:** the exact broadcast, acoustic record, rhyme, note, or testimony authored by the content owner.
- **Player observation:** the fact the current run actually exposed, with its source and discovery context.
- **Interpretation:** one or more authored explanations that may be incomplete, contested, or wrong.
- **World consequence:** a permitted command routed to the current owner, such as revealing a known target or recording a resolved discovery.
- **Generated variation:** only presentation-scale choices such as which already-authored optional line is shown, which eligible optional destination fills a slot, or which existing clue is surfaced.
- **Persistent truth:** the minimum durable state needed to reconstruct what the player learned and what consequences occurred.

These labels are a proposed editorial and runtime contract, not a claim that all current schemas expose these as formal fields. A future implementation must map them to existing structures or obtain architecture approval before adding fields.

### What may vary

A seeded run may choose which eligible optional clue or side-location appears, provided all mandatory story facts and routes remain available. A repeat visit may select an authored response variant based on recorded visit or quest state. A character may express uncertainty differently after the player has gained verified evidence. A radio scene may change from static to intelligible when the established cipher chain reports its appropriate state.

Generated variation must not invent the identity of a speaker, a new historical fact, a new faction allegiance, a new kinship relation, a new unique item, a new permanent location, or a world-state consequence. Those are authored facts and require a reviewable record. Randomness must not make a required target disappear, change a decoded answer, or contradict a prior journal entry.

### Record envelope and reviewable manifest

When an integration package needs a manifest, first determine whether the current owner already holds the needed information. A proposed review table—not a new runtime schema—should list canonical record ID, source catalog, owning loader, all known consumers, authored truth, generated slots permitted, destination binding status, persistence owner, localization needs, and migration risk. Empty or unknown values are explicit blockers, never implicit permission to synthesize data.

The review should catch three common duplication errors: copying a hydrophone record into a quest catalog with divergent text; shadowing a folklore record in a dialogue file that later drifts; and adding a second target-location record when the cipher engine already controls the reveal. Dialogue may refer to a content ID if the live dialogue system supports that binding; otherwise the integration plan must identify a one-way presentation adapter rather than copying mutable authority.

### Seeded composition and save behavior

The selection seed is a stable input for optional composition, not a content-authoring tool. Given the same campaign state and seed, the same optional clue/site selection should be reproducible. Persistent state records player-visible outcomes through current save owners. Do not serialize a generated prose string as authoritative truth when its source record and chosen variant are sufficient. If a specific generated choice must be recalled verbatim, record the stable variant key through the existing owner, after confirming that owner supports it.

Content version changes need graceful behavior. A removed optional record can be omitted. A removed mandatory record must be mapped through an authored compatibility alias, converted into a recoverable clue route, or delayed with explanation. Never deserialize missing content as a silently successful quest. Do not reuse an old identifier for a different acoustic event, speaker, target, or consequence.

### Quality and accessibility envelope

Every generated presentation must remain within a reviewed voice and reading-level band. Surface which information is observation, uncertain interpretation, or confirmed resolution through text, journal state, and non-color cues. If a clue depends on hearing a sound, provide an equivalent caption or transcript in the same gameplay window. If a cipher relies on a text pattern, provide an accessible alternate representation that preserves the same reasoning and does not reveal the answer prematurely.

### Release gates

Before any new record is promoted, show a loader-to-consumer trace, duplicate-ID scan, schema and reference validation, seed replay for any generated selection, save migration review, and an author review of the authored truth. The plan remains DRAFT if a data file has no verified loader, if the record lacks a reachable presentation route, if destination identity is unknown, or if a proposed consequence has no current owner. No new authority, field, or content asset is created by this pass itself.

## Pass 17 — Separate the burial archive, campaign memorial, and generated recollection

This pass adds a provenance model for archival stories and destination-bound micro-locations. It follows the master world bible's instruction to distinguish static canon from dynamic state and its loop-closure rule: source, state, observation, consequence, and memory must remain traceable.

### Source audit and authority classes

The repository contains narrative/undertaker_burial_records.json with an items collection, burial identifiers, deceased identifiers, reported death and burial days, ceremony type, plot, witness/attendee names, epitaph, personal effects, and notes. The file references location_ash_dune_cemetery, which exists in locations.json. A direct source search did not identify a typed Core/host loader for this exact filename. That is an unverified consumption path, not proof of a live quest source.

MemorialSystem is a separate, verified campaign owner. It stores actual campaign memorial entries, supports idempotent memorialization, exposes a read-only entries collection, supports one-time mourning through Mourn, and captures/restores its state. The static burial records and the runtime memorial entries must not be silently merged. The archival record may describe a pre-authored event; the memorial state describes what happened in this save. A matching deceased_id does not prove the two records refer to the same campaign person or the same run.

The micro-location corpus is another authored authority. Its entries are encounter definitions loaded into NarrativeEncounterSystem, with current-location eligibility and depletion semantics. The parent location remains owned by the existing locations/map catalogs. Do not copy encounter text into a destination catalog to make a marker appear.

OralLoreCatalog is loaded from two existing oral-lore files by OralLoreHostSession. OralLorePerformanceSystem tracks first-heard IDs and producer contexts; OralLoreSaveStore persists stable IDs rather than copying lyric text. Main.Plans155 binds the first-heard journal strip and expedition discovery hook. The host comment states that the system is cultural discovery only, with no morale, healing, route, or faction effects. This is an established owner and a boundary, not an invitation to duplicate songs in quest prose or attach unsupported gameplay rewards.

### Proposed source-to-state table

For every future content record, the authoring packet should classify it as one of:

- authored archive statement: stable prose and fixed provenance, not automatically true in the player's current campaign;
- live campaign event: a current survivor death, memorial entry, or Mourn result owned by the memorial/fate path;
- location-bound encounter: authored text whose eligibility is tied to a validated parent destination;
- discovered cultural record: a stable oral-lore ID whose first-heard state is saved by the oral-lore owner;
- generated recollection: an optional presentation variant assembled from existing facts, never a new historical fact;
- unresolved interpretation: a player or character theory that must remain distinct from the source record.

This classification can live in the documentation/index until the actual consumer is verified. Do not add a provenance field to every content schema by default.

### Identity and time conflict policy

Fixed archival days, names, ages, causes, and plots must not overwrite dynamic campaign state. If an authored case uses a deceased ID that can also exist in a live roster, define which content mode is intended: fixed historical canon, run-specific memorial, or cross-campaign archive. Only a current owner can confirm identity equivalence. If there is no safe mapping, the text should refer to the historical record without stating that the currently controlled survivor died on that day.

When campaign day and authored document day disagree, preserve both with labels such as “ledger date,” “witness estimate,” and “ceremony day.” Do not normalize uncertain testimony during serialization. Generated prose may choose an authored phrasing variant, but its underlying claim must remain the same and its variant selection must be deterministic when persistent.

### Generated and authored boundary

A procedural recollection can vary its opening, transition sentence, or speaker acknowledgement using known facts such as location, confirmed evidence, and whether the player has already visited. It cannot invent attendance, kinship, cause of death, a new burial plot, a song's origin, or a witness statement. No generated text may impersonate a deceased person unless the scene is explicitly framed as a recording, memorial text, remembered quotation, or epilogue—and the relevant source must exist.

An authored micro-location definition supplies encounter identity, text, choices, and any current consequence fields. Generated destination composition may select among eligible encounters, but cannot change their required parent or reveal a hidden location without the normal owner. Oral-lore first-heard state stores IDs; rendering text continues to come from the current catalog. This protects content updates and save compactness.

### Migration and deletion behavior

If a burial record is removed or renamed, an optional archive scene can disappear only when no active quest/save refers to it. A required quest should migrate through an explicit alias, a replacement record with the same semantic purpose, or an explained delay. Never reinterpret an old burial ID as a different person or event. If a micro-location is retired, map its active quest reference through the location/encounter owner; do not leave an unresolved string that looks like a completed clue. If an oral-lore title or context changes, the stable lore ID preserves first-heard history, while the catalog provides updated display text.

### Promotion gate

Before an authored package becomes active, verify the exact loader, integrity validation, reachable player presentation, persistence owner, and all cross-references. For each record, report whether its source is authoritative for fiction, its runtime state owner, and its first observable channel. If any column is unknown, leave the package as proposal. The plan's purpose is to make content lineage inspectable, not to add a general-purpose provenance database.

### Pass 17B — Editorial provenance packet and corpus intake

A future content batch should arrive with a compact, auditable editorial packet. This avoids treating JSON presence as implementation or allowing a generator to absorb facts whose source cannot be found.

**Record identity card.** For each burial entry, list the stable burial ID, deceased identifier, source file, source authoring status, event-day fields, witness/attendee claims, associated location, and whether the record describes a body, a cenotaph, or an uncertain event. Add a separate column for the live campaign memorial identity, which may be unknown. Never derive one identity from matching names alone.

**Consumer trace card.** Name the exact loader, parsed DTO, runtime owner, presentation path, state write, save section, and validator that handle the record. For the present archive, the content file and cemetery reference are proven, while a direct loader/consumer remains unverified. The trace card stays incomplete until a source file and call path close that gap. Do not mark a catalog integrated because it appears in a boot-validator manifest or a data inventory.

**Text lineage card.** When a quest quotes the registry, retain a reference to the source record and label the quote as source text, paraphrase, testimony, or interpretation. A later correction becomes a separate authored note with author and evidence, not an in-place rewrite of the old archive entry. This is especially important when the original entry documents institutional harm or a contested decision.

**Location lineage card.** A micro-location encounter has its own encounter identity and a required parent location. A map marker is a different record and only belongs in the current map/location authority if it is a dispatchable destination. Keep both links explicit: content record to encounter definition; encounter definition to parent location. Do not create a second alias table in a quest file.

**Oral-lore lineage card.** The existing stable lore ID identifies a song or spoken record; the producer identifies where or how it was first heard; the saved state records discovery. A quest can point to that ID if a supported consumer exists. It cannot copy lyrics into dialogue and then diverge. The current journal strip derives display text from the catalog, so adding another persisted prose copy is unnecessary.

**Generated-language card.** For procedural line selection, list the stable authored variants, legal conditions, seed source, and which facts are invariant. If a phrase is selected after a player choice and must survive save/restore verbatim, determine whether an existing narrative owner persists that choice. If not, do not promise a unique recalled phrase; use an acknowledgement derived from durable state instead.

**Repository intake and retirement.** Before adding a new corpus, search the live data tree for equivalent records and identify whether the canonical loader already accepts the format. Before retiring an entry, find active quest references, discovery manifest references, journal links, and saved stable IDs. Preserve unknown historical IDs safely. If the target consumer has no compatibility behavior, postpone retirement until a migration plan is approved.

**Documentation closeout.** The final authoring packet should say “authored and validated,” “authored but not loaded,” “loaded but not reachable,” or “reachable and stateful.” These are separate maturity stages. It should include record-level unresolved questions and a direct evidence link. This language gives the content team a useful inventory without implying that a new system or migration already exists.


## Pass 18 — Instrument procedure provenance and generated-scene limits

### Boundary statement

The master world bible calls for Geiger-calibration depth as a creative expansion subject. The current source confirms a single state owner for dosimeter calibration and a panel that delegates to it. This plan distinguishes three layers: authored explanation, deterministic system result, and run-specific presentation. Only the first and third are content concerns. The calibration state and measurement confidence remain owned by DosimeterCalibrationSystem, and actual radiation remains owned elsewhere.

### Permanent authored material

Store authored procedure steps, line variants, quest prerequisites, and outcome copy in the existing authoritative data location once the owning content loader and schema have been verified. A proposed calibration packet may describe:

- Why the instrument is overdue after the current reading threshold.
- What low battery and poor sensor condition mean for taking further readings.
- What the one-day station reservation means to the schedule.
- How to compare nominal readings with the resulting uncertainty band.
- What cancellation means and how to resume later.
- Which reports are historical testimony and which are current system state.

Every authored entry needs a stable snake_case identifier, speaker or narrator role, context conditions, content provenance, and an explicit list of effects. The id must be checked against existing catalogs before it is accepted. Avoid a parallel JSON catalog that is invisible to the current loader. If the existing quest or encounter schema cannot represent the packet, record a small schema proposal with migration and validator needs; do not smuggle an unvalidated structure into a narrative file.

The dialogue may state that a calibration improved confidence only after a successful completion result. It may not state that the reading is exact, that the environment is safer, or that true dose changed. Narrative records can disagree about past measurements; the current device values remain the technical authority.

### Deterministic result layer

The reviewed Core flow already contains a deterministic procedure: StartCalibration checks registration, battery, sensor condition, and station occupancy; it reserves the station through a due day. CompleteCalibration requires the reservation and elapsed due day, resets the readings-since-calibration count and overdue flag, increments the count, improves quality with diminishing returns, recalculates the error band, and emits completion/state events. The visible quality update is defined by the existing owner as an improvement of 0.15 times the remaining distance to one. Error band is derived from quality, sensor condition, and overdue status. A plan implementation should call this owner rather than recalculate those values in a quest or panel.

The TestSourceExposureMsv constant is present in the reviewed source, but the searched references show no call site beyond its declaration. Treat it as an unused premise, not a consumable cost or booked dose. Before any design uses it, determine whether test exposure is meant to describe a fictional calibration source, a survivor exposure, or a retired value. No content effect may charge it to RadiationSystem or DoseLedgerSystem based only on the constant name.

### Generated and variable presentation

Generated content is limited to selecting among authored variants and to encounter placement already supported by the current content runtime. Any randomized line selection must use the established seeded stream and stable ordering; it must not generate new calibration facts, skill outcomes, location ids, device tags, dose values, quest requirements, faction effects, or reward quantities. The minimum viable version needs no generated calibration result at all.

Variable detail may reflect real input fields: device tag, assigned survivor when that identity is available, readings count, overdue status, battery band, sensor band, quality, error band, and whether the one-day procedure is reserved or complete. UI copy should round values for readability without replacing the underlying numeric authority. If the system lacks a field, use generic copy instead of inventing one at runtime.

### Provenance for mixed scenes

A scene with an authored report and live device state must label both sources in design notes:
- Report claim: authored speaker, date or chronology, and reliability status.
- Current reading: derived from the registered device.
- Current uncertainty: returned by the calibration owner.
- Quest state: returned by the quest owner.
- Player choice: recorded by the current consequence owner only when the player commits it.

This separation supports contradiction as drama without confusing the player about state. For example, a survivor may remember that a corridor was safe last season while the current instrument reports a broad band. The dialogue can preserve both memories and invite a cautious decision; it cannot resolve the disagreement through prose alone.

### Data quality and production gates

Before content authoring is promoted:
1. Verify the authoritative schema, loader, and catalog validator for the intended quest or encounter family.
2. Confirm the target speaker and location ids exist and are reachable.
3. Verify the calibration device fields can be supplied to dialogue without a copied mutable snapshot.
4. Confirm all authored conditions have a fallback line.
5. Confirm generated variants are selected deterministically and cannot alter consequence class.
6. Check ids and references with the current content-integrity pipeline.
7. Keep the complete prose packet marked DRAFT until its consumers are proven.

The minimum viable content package is one authored procedure explanation, one overdue warning, one blocked response per actual prerequisite, one elapsed-time completion response, and one cancellation response. Expanded layers may add memory of prior visits, role-specific vocabulary, faction testimony, and expedition reports only after their owners and hooks are verified. This boundary keeps authored richness high while preventing narrative text from becoming another source of gameplay truth.


## Pass 19 — Wiretap evidence provenance: document, reading, claim, and admission

### Source-to-player chain

This expansion follows the world bible's wiretap-as-standing-evidence seed, but current evidence exposes a gap. SignalIntelligenceCatalog can parse bunker_wiretap_transcripts.json and return a BunkerWiretapEntry. Repository call-site search found no production consumer of that catalog; only its own implementation and catalog tests reference the type. The corpus files are classified CODEX_ONLY in the current content utilization baseline and registry. The transcript entry has no structured evidence tag, player-read flag, custody record, linked location ID, or validated character references. The existing VerdictEvidenceChain subscribes to MachineLogSystem entry reads, and VerdictHostSession has a separate path for eligible Verdict items. Neither path generically consumes SignalIntelligenceCatalog entries.

Accordingly, the content model should name four different things, each with one authority:

- Authored document: the immutable transcript record and its source metadata, owned by the narrative data catalog.
- Player discovery/read: the fact that the player found or listened to a specific entry, owned by the current discovery, Codex, journal, or document host after that owner is identified.
- Investigation claim: the player's quest progress and interpretation, owned by the current quest lifecycle.
- Admitted evidence: a source accepted by the existing Verdict EvidenceLedger and Reckoning chain under a new, explicitly reviewed producer seam.

Do not add a mutable read boolean to the static transcript JSON. Do not create a second wiretap state store, an evidence ledger inside Standing Record, or a parallel “standing documents” catalog.

### Authored data and link strategy

The wiretap corpus already owns transcript text and descriptive fields. The minimum change proposal is a validated reference from an existing authoritative evidence definition to a transcript ID, plus a source type and presentation policy if the current Verdict data schema can express them. This should not duplicate the transcript body. If the current schema cannot express a source-document reference, document a narrow schema migration and update its owning validator before adding records. Do not add an unvalidated ad hoc field and rely on serializer permissiveness.

The link is one-way: a Verdict evidence entry may cite a document ID. The transcript remains a historical source; its prose is never rewritten when the player chooses to admit, seal, or challenge it. A cross-reference must validate against the loaded wiretap catalog and the current evidence catalog. Unknown IDs should produce a row-level validation failure and a safe fallback, not a silent generic enrollment.

Metadata in the existing record is descriptive, not automatically dispositive:
- audio_clarity_score describes the authored recording quality; it does not prove the recorded allegation;
- target_faction is an authored string and requires mapping before it can gate current faction access or standings;
- speaker_identities is text, not a cast-member relationship;
- timestamp_relative is a source claim, not the current sim day;
- tags aid retrieval but do not grant quest progression or outcomes;
- prose is the authored transcript and is not generated per save.

### Permanent versus generated content

Permanent authored material includes the transcript, its attribution note, a content warning when required, admissibility requirements, optional corroborating records, and every dialogue branch's consequence label. Generated content may select among written variants, display a current quest status, or place an eligible child encounter at a parent location already selected by the expedition owner. Generated content may not invent a speaker, alter a quote, sharpen or lower clarity, add a corroborating source, infer faction guilt, create a location, or decide that a record is admissible.

A seeded line variant must use the existing deterministic content-selection contract and stable IDs. The MVP needs no generated transcript variant. The original wiretap is the invariant record; contextual dialogue may change around it.

### Evidence identity and collision controls

Every admitted record needs one stable evidence ID. Re-reading or re-opening the transcript must not produce a second enrollment. The current EvidenceLedger already owns unique enrollment and captures enrolled IDs; the existing evidence chain reconciles read machine logs idempotently after restore. A new adapter should reuse these rules rather than invent a new document counter. Before adding any transcript-backed evidence, check whether its ID is already represented by a Verdict item, machine-log evidence tag, quest result, or another authored record.

Evidence count has major endgame consequences: Unified Verdict logic consumes enrolled evidence count. Therefore, a wiretap must not enter the canonical ledger merely because its catalog loaded, its page was rendered, or its prose appeared in the Codex. The content must specify whether an explicit player submission is required and what one entry contributes. Any change to endgame thresholds or eligibility is a separate balance and authority decision, not implied by this content bridge.

### Archive and Standing Record boundaries

Prewar archive evidence provides a pattern to inspect, not proof that all narrative documents share a generic evidence API. The current StandingRecordEngine coordinates location layouts, location memory, and site encounters; its catalog and host do not establish documentary admission. Keep courtroom/Verdict evidence and location-memory presentation as separate authorities. The same wiretap may be mentioned by both only through stable reference IDs and only if each system's current route is proven.

### Validation checklist

Before implementation, capture a source trace for: catalog load, production consumer, player read/discovery event, quest trigger, evidence definition, EvidenceLedger enrollment, Reckoning count update, save capture/restore, Codex presentation, and content-utilization classification. Acceptance requires a visible and reachable transcript, an exact reference to one catalog record, clear source attribution, an idempotent player action, no content-text duplication, no generated evidence facts, and a proven effect on endgame count. Until all links are named, label the integration DRAFT / consumer gap rather than “implemented.”


### Pass 19B — Data contract, corpus migrations, and provenance audit

The present wiretap schema is useful for authored archival display: stable ID, channel label, target faction string, numeric clarity score, speaker-identities string, relative timestamp, tags, and prose. It is not an evidence-admission schema. Keep the existing document record intact and add only a verified reference through the owning evidence data contract.

If a source-reference field is approved, it must be optional for existing evidence rows, stable across saves, and checked against the narrative catalog. The migration must preserve current IDs and default old rows to “no linked transcript.” Content integrity should report each bad reference with both file and row ID. A missing optional source reference cannot erase an already admitted evidence ID from a save. Schema changes require the current data-authority workflow and should not be hand-inserted into a runtime-only panel.

For a transcript-to-evidence mapping, document these provenance elements in authored data or in the established evidence definition:
- canonical evidence ID;
- source transcript ID;
- source kind, such as wiretap transcript;
- authored admission rule;
- whether the player must explicitly submit it;
- optional corroboration IDs;
- contradiction/correction IDs where authored;
- consequence class and whether admission affects the existing Reckoning count.

Do not record a single “reliability” score unless a current owner defines its semantics. The transcript's clarity score measures signal clarity in the source corpus; source identity, chronology, custody, completeness, and factual corroboration remain separate. Any new display should use labels that preserve these distinctions.

A generated expedition encounter may show that a clue was found at a selected parent, but the encounter instance should reference the immutable authored document ID. Its seed controls only variant choice and placement under current deterministic rules. The save need only persist discovery/quest/evidence state through existing owners; it does not copy the full transcript or a generated paraphrase.

### Provenance review examples

Example: a transcript is loaded, but the player never opens it. The authoring source exists; there is no player knowledge and no evidence enrollment.

Example: the player opens the transcript and chooses to investigate. The record is read; the quest starts. It is still not admitted evidence.

Example: the player submits it as an attributed lead, but no supporting source exists. The quest can resolve as unresolved; the Verdict ledger changes only if the existing evidence owner explicitly accepts that evidence class.

Example: a later record contradicts the first transcript. Keep both authored sources and link a correction relationship. Do not rewrite the first transcript or delete its evidence enrollment.

Example: generated dialogue chooses a more guarded greeting because of an existing faction relationship. It may not change the transcript's words, identity metadata, or evidence status.

### Review gates and change control

The data review checklist is: schema version remains valid; IDs are unique; transcript and evidence references resolve; all speaker/location/faction relationships are either canonical references or explicitly plain text; all condition fields are consumed by a current loader; missing optional references degrade safely; and content-utilization status changes only when the real consumer is integrated. Update the registry and utilization evidence after implementation rather than manually claiming reachability here.

The design passes review only if an implementer can trace each player-visible statement back to either authored text or a current state owner. No generated prose may supply a new fact that the record itself never stated. This contract makes future transcript batches re-usable without creating a mutable document shadow or an undocumented evidence API.


## Pass 20A — Authored radio theater, scheduled delivery, and deterministic quest selection

### Current ownership map

This continuation takes Part 43, seed 19 from the master world bible: The Machine’s tribunal broadcasts as a procedural-case radio series. The current system boundaries are unusually important here. VerdictRadioSystem loads the authored verdict_radio.json corpus, gates firing on day and Reckoning phase, publishes radio.verdict.broadcast, and persists fired IDs. Ownership notes say Plan 94 already brought the corpus to 30 Machine-Register broadcasts. RadioProgramProductionSystem owns player program preparation jobs and follow-up hooks; its own comments leave scheduling, reception, and propaganda pressure outside that system. ProceduralNarrativeSystem is a seeded quest-template selector, not a broadcast-script generator. These contracts prohibit using “procedural” as a reason to synthesize unreviewed dialogue or create a second delivery scheduler.

The content model therefore has three different things. Authored episode text is canonical, reviewed prose with a stable content ID and revision. Scheduled delivery is the existing radio owner’s decision that an entry becomes eligible and fires once. Generated quest variation is a deterministic selection among authored objective, clue, or location variants, if the current quest owner can support that selection. A generated choice may select a witness role, an eligible site, or an alternate clue; it may not paraphrase a testimony, invent an NPC motive, or change a line’s meaning at runtime.

### Draft data contract

First inspect VerdictCatalogLoader’s exact schema, validation, and consumer path. Do not add guessed fields to verdict_radio.json. If the loader has a compatible extension seam, episode metadata can be associated with existing entry IDs through a single authored reference. If not, the integration choice is to extend the canonical loader and its validation under a named claim, or defer episode linkage; do not add a second catalog that quietly competes for the same broadcasts. Candidate fields below are a design checklist, not approved JSON:

- stable entry ID, series ID, episode index, and script revision;
- authored message or dialogue reference, content-language key, and source classification;
- prerequisite phase/day and optional referenced quest template;
- expected delivery surface and explicit player-reception prerequisite;
- optional location-role references, which must resolve to current canonical locations;
- attribution label: dramatization, verified record, disputed account, or correction;
- migration/default behavior for older rows that lack theater metadata.

Store only facts whose owner is clear. The radio system owns whether a broadcast entry fired. The player-facing radio/archive owner, if proven, owns whether a player received or reviewed it. The quest system owns its accepted state and objective completion. The map owner owns discovery. The journal/chronicle may present a derived record, but cannot become a duplicate quest or evidence ledger. A theater transcript is not automatically VerdictEvidenceChain input; that chain’s eligibility and enrollment must be checked independently, and loading prose is never evidence enrollment.

### Generated variation boundaries

The seeded narrative selector may be useful for choosing among pre-authored variants when a quest instance is created. The candidate pool must be stable-sorted by authored ID, filtered by current conditions, and selected only through the existing seeded RNG contract. The chosen variant is then persisted by its proper quest owner so a reload does not redraw a different witness or alter the scene the player was promised. If ProceduralNarrativeSystem does not consume these templates or cannot persist the selected variant through its current owner, treat that as a blocked integration premise.

Allowed selection: among two reviewed witness introductions, select one eligible authored version; choose one clue site from candidates that satisfy route and availability; choose a response recap that corresponds to a verified state. Disallowed: generating tribunal testimony from current inventory; changing faction attribution through a random line; selecting a site that is absent from the expedition; emitting novel prose from a prompt or model; drawing a supposedly random variant per frame or per menu open; allowing enumeration order to determine the winner.

### Provenance and lifecycle

Every public case assertion needs an origin marker in content authoring: authored fictional scene, player-observed event, NPC recollection, or independently verified record. The UI may translate those categories into natural prose rather than expose technical tags, but the authored record should preserve them. When two sources disagree, dialogue should say that the accounts diverge. It must not upgrade a dramatized broadcast into a verified historical fact simply because the scheduler fired it.

A broadcast revision after release cannot rewrite what a save has already resolved. If scripts are content-only and their IDs remain stable, update the prose with reviewed changelog notes. If a field changes state semantics, use a schema migration owned by the responsible system and preserve the old resolved outcome. Removing an episode entry must not make its quest reference dangling; deprecate it, provide an explicit replacement reference, or keep a compatible legacy definition.

## Pass 20B — Content-build gates and maintenance policy

A theater content build should produce an audit table, not a second runtime registry. For each authored episode it reports: unique ID; current catalog consumer; schedule condition; player-visible delivery path; script source/revision; quest reference; dialogue nodes; referenced location roles; terminal routes; accessibility text; and unresolved canon or voice review. The same entry should not be copied into the archive, journal, and quest catalog as independently editable message text. Derived surfaces reference one authority and apply presentation-specific formatting.

The linter should distinguish hard failures from editorial warnings. Hard failures: duplicate or blank IDs, invalid phase/day range, missing consumer for a required reference, quest reference to absent content, unsupported schema version, no player-facing discovery route for a mandatory quest, variant pool with no eligible fallback, and a terminal quest path with no resolved outcome. Warnings: length variance, very similar openings, repeated moral framing, missing alternate text, a series episode whose prior-part callback has no recap, or lore flagged for canon review. A warning cannot be promoted to a runtime blocker unless the policy owner defines that rule.

The smallest implementation-compatible slice is a static authored episode that uses the current VerdictRadioSystem and is presented through a proven existing radio/log surface. It can carry an optional quest reference only after the receiver path is confirmed. Do not implement fresh scheduling, an episode sequencer, a script generator, a public-record ledger, or a new persistence section in this slice. If an existing surface can display only a terse message, keep the theater scene as a separate authored dialogue route and link it through a verified clue. A complete story is not justification for storing the same content in two places.

Source drift review is mandatory immediately before promotion. At minimum inspect the current catalog schema and loader, radio event subscriber, radio UI/log consumer, quest catalog consumer, save owner for quest progress, map selection consumer, seeded RNG interface, and VerdictEvidenceChain enrollment gates. Record the exact source paths and tests in the implementation package. Current integration-ledger completion of Plan 94 and Plan 173 establishes the baseline contract; it does not authorize editing those owners or assume their behavior will remain unchanged.

Cost model: low for adding four reviewed, static episodes to the existing authoring corpus if the current surface already supports them; medium for the reconvergent quest and dialogue references; high for voice performance, localization, and a second reception surface; very high for a new procedural script system, which is deliberately outside scope. The expansion layer may add optional variants and faction-specific interpretation after the base episode sequence works. The core-game slice remains a single static case and one interaction route, so the player can understand the feature without procedural variation.

The release note must distinguish “authored and scheduled” from “player encountered,” “quest available” from “quest accepted,” and “case dramatization” from “evidence.” It should list what a new save does, what an old save does, whether previously fired broadcast IDs remain fired, how revised text affects resolved cases, and what happens if an optional quest template is absent. This vocabulary prevents content revision from accidentally changing state truth.



## Pass 20C — Content provenance matrix and migration checklist

### Boundary matrix

| Content or fact | Permanent authored authority | Runtime/generated fact | Safe presentation |
|---|---|---|---|
| Tribunal episode text | Reviewed episode/broadcast entry | None; selection may decide whether an authored entry is eligible | Render the exact authored version through an existing consumer |
| Delivery eligibility | Authored schedule condition | Existing scheduler evaluates day/phase and fired state | Say scheduled or transmitted only when that fact is true |
| Player exposure | Authored surface/reference | Current receiver may record receipt if it owns that behavior | Say heard/read only when the receiver proves it |
| Quest objective | Authored quest template and allowed transitions | Quest instance state and selected reviewed variant | Show objective state from quest owner |
| Witness identity | Authored character content | No generated identity in the first slice | Use placeholder roles until canonical IDs are verified |
| Location | Canonical location data | Expedition selector chooses among eligible instances | Render the selected location, never a guessed alias |
| Testimony status | Source classification authored per line/evidence | Player observations and quest evidence facts | Distinguish staged case, recollection, and corroboration |
| Outcome | Authored response options | Player’s committed choice and verified delivery outcome | Recap only the effect owned and applied |

No generated fact may be serialized as if it were an authored canon fact. Conversely, a static authored broadcast must not be described as dynamically generated simply because the player hears it on a different day. The content pipeline should preserve both origin and current state; presentation can remain natural and unobtrusive.

### Revision and migration examples

If an editor corrects punctuation or a non-semantic wording issue while retaining the same entry identity, existing fired state can remain attached to the ID. If the episode’s source classification changes from dramatization to verified record, this is semantic and requires a content review plus a resolution for saves that already saw the old version. Options include preserving the prior text revision for existing saves, displaying a correction note, or keeping the quest outcome explicitly uncertain. Do not silently reinterpret a player’s earlier decision.

If an episode is removed before it ever fired, the scheduler’s corpus should no longer offer it, and any quest reference must be made optional or remapped through reviewed content. If it has already fired in a save, retain a legacy ID definition or a safe recap so the save does not contain an orphaned event. If a generated location variant disappears from the catalog, the quest must use its authored fallback or remain visibly delayed. A migration must not invent a replacement clue that was never reviewed.

### Content validation rules

A catalog gate can verify unique IDs and version support, but it should also verify the cross-reference graph. Check that every episode-to-quest reference resolves; every quest-to-dialogue node resolves; every location role has at least one eligible canonical location or explicit fallback; every alternative path reaches a legal terminal or recoverable state; every line’s provenance classification is known; and every optional variant has a deterministic fallback. Check consumer reachability separately from schema validity. A valid row with no subscriber or panel is not playable.

Stable sort before seeded selection. Persist the chosen authored variant through the proper owner. Do not derive the choice from current hash order, machine locale, frame count, or wall clock. Localization must not change IDs or eligibility. Strings can vary by language while effect references and node IDs remain stable. If the data pipeline uses translation keys, keep them in the existing localization authority and make missing-language fallback explicit.

### Handoff evidence bundle

The implementation ticket should attach a compact evidence bundle: current schema excerpt; loader path; event subscriber path; player-visible surface; existing save section and capture/restore path; seeded selector interface; location eligibility owner; quest and dialogue consumers; exact catalog IDs; proposed change list; focused test commands; and rollback plan. These references are required because the master world bible is a design authority, not proof that a current runtime path exists.

The core content can ship without generated variants. Expansion options include selected authored variants for repeated playthroughs, alternate actor roles, and extra location callbacks. Each option requires a small allowed pool, a stable seed contract, and a content provenance tag. Do not broaden the feature into open-ended script synthesis or generalized event sourcing. The maintainable structure is a bounded graph of reviewed content plus deterministic selection of authored components.


## Pass 20D — Provenance for competing audience interpretations

The Second Margin adds a content type that must stay distinct from evidence: audience interpretation. The original theater script is an authored dramatization. The witness account is an authored character’s recollection of a lived event. A listener’s response is an authored fictional opinion about what the audience understood. The player’s action is a saved quest choice. None of those statements becomes a verified world fact merely because they appear in the same journal entry.

Represent the distinction in the authoring source, even if the player-facing prose stays natural. A content record should identify the source class, speaker or role, parent episode ID, relevant parent outcome, scope of knowledge, allowed presentation surfaces, and whether the text is factual, interpretive, disputed, or a correction. These are review fields until the canonical schema is confirmed. Do not create an “audience truth” database or store raw listener interpretations as evidence items.

If the player asks the listener why they took the scene as an accusation, the answer can explain their experience without overriding the authored case. If the player shows them a verified correction, the listener can acknowledge the wording while keeping a different opinion about its tone. This creates narrative change without requiring a boolean “convinced” state. If dialogue needs to remember that the player shared the correction, the parent quest/delivery owner supplies that fact; the optional follow-up does not own another copy.

The follow-up’s generated variation, if approved later, is limited to selecting an authored interpretation compatible with parent outcome and available character. No procedural generation may change the script’s text, invent an extra witness, add a faction’s endorsement, or select a conclusion unsupported by the parent state. A save must retain the selected variant if variant identity affects later lines. When no variant is eligible, use the neutral authored response and keep the optional branch open or resolve it gracefully.

A provenance review compares the original and corrective wording line by line. Public correction can quote only the approved excerpt and must not imply player-observed facts that the player did not observe. Private correction may mention the witness’s preference but cannot disclose identity without consent. Silence contains no public recap. A recap that compresses several sources should retain the distinction: “the program staged a case; the watch lead described an unrecorded task; the listener heard the performance as a broader accusation.” This sentence summarizes three sources without converting any into universal truth.

Migration policy: if a resolved parent quest has no optional follow-up state, treat the follow-up as not started. If the parent outcome is missing or invalid, hide the optional prompt and report a content-state validation warning in developer tooling. Do not infer an outcome from a broadcast ID. If an interpretation line is revised after release, retain its stable reference where possible; if its meaning changes, author a correction or a new revision and ensure existing players see no false history. Optional content may be disabled on old saves without changing the parent’s terminal result.


## Pass 20E — Authorship, runtime selection, and content versioning dossier

### Three layers of authority

**World facts** come from current simulation and canonical system state: a site is discovered, a character is present, a quest is resolved, a transmission fired, or a program was delivered. **Authored fiction** defines dialogue, performance scripts, role histories, descriptions, and allowed outcomes. **Presentation** selects a valid line and reports the facts. These layers can meet in a scene, but no layer should silently impersonate another. The fact that a character performs a scene does not mean the scene literally happened in the player’s world.

A small runtime selection may choose among authored lines by stable conditions: whether the parent outcome was public, private, or silent; whether a scene can occur at the current location; and whether an optional knowledge gate is satisfied. The selected text and IDs remain authored. No runtime system writes new story prose, arbitrates historical truth, or invents a new faction response. If a later content tool uses generated drafts for author assistance, those drafts are reviewed and committed as static content before they enter the game data authority.

### Content graph ownership

The broadcast catalog owns broadcast identity and schedule metadata. The dialogue catalog owns nodes and line references. The quest catalog owns objective definitions and transitions. The canonical location data owns map-place identity. The current receiver, if any, owns exposure. These catalogs may refer to the same content ID, but only one owns the full text. A central episode identifier may be a stable foreign key; it must not duplicate scripts into every catalog. At load time, the integrity pipeline verifies each reference and the consumer report verifies the edge is reachable in a shipped path.

If the project currently lacks a separate dialogue catalog, the plan should use the actual data path that hosts authored dialogue rather than create a speculative one. If the radio broadcast entry can contain only one short message, keep the longer scene in the existing conversation/content owner and expose it through a verified route. If no route exists, the episode can remain a DRAFT content packet; the missing consumer is a legitimate gap to assign, not grounds to create an ad hoc panel-only authority.

### Version and localization rules

Keep stable semantic IDs independent from display text and locale. A punctuation correction does not change outcome identity. A new translation does not change schedule or quest condition. A new branch or changed source meaning increments the appropriate content revision and receives a save compatibility plan. If translated text is missing, fall back through the existing localization policy and preserve response IDs. Never parse prose to infer a quest effect.

Voice recording requires the approved text revision and speaker role. Recording replacement should not alter a node’s consequences. Captions and transcript share the same authored line source where possible. If audio contains meaningful information absent from subtitles, the content is incomplete. If the UI truncates the text, shorten through editorial review rather than silently hide the ending.

### Content utilization and failure triage

A row is integrated only when a real runtime path consumes it. The utilization audit should classify each new content item as directly loaded, referenced by an already-loaded owner, editor-only, or unused. An editor-only world bible note is not playable data. An unused DRAFT row should remain outside production catalogs. If a radio entry is loaded but has no event subscriber, distinguish loader success from surface reachability. If a quest references an optional dialogue branch that is not present in the current build, the graph’s safe fallback must be authored and tested.

Triage errors by layer: schema/ID failure; broken cross-reference; owner unavailable; event not routed; player surface unreachable; save state missing; presentation inaccurate; or narrative contradiction. The fixer should update the authority responsible for that failure. Do not repair a missing location by adding a UI marker, a missing quest transition by appending a journal line, or a missing audience-delivery result by toggling a dialogue flag.

The same rules apply when the series grows. New episodes may share a template for revision, locale, and accessibility metadata, but must have unique authored scenes and a narrative reason to exist. A content-size target does not justify duplicate callbacks or repeated descriptions. The master bible subject points to a lane; every new record still needs evidence of its consumer, canon, and purpose.


## Pass 21A — Folklore provenance, cohort memory, and belief boundaries

### What exists, what is absent

The current children’s folklore records are authored prose with structured metadata: id, tradition_type, origin_sector, folk_theme, timestamp_relative, tags, and prose. The base and batch-2 records are loaded by DailySurvivalCatalog; JournalCodex renders their text and descriptive metadata. BeliefMovementDefinition is an authored doctrine template with creed, comfort themes, blind spots, practices, conflict profiles, and tags. IdeologicalFrictionSystem reads a survivor’s existing belief profile and calculates roommate compatibility; ZealotrySystem references belief movement definitions. PsychologicalArcSystem models authored breakdown/recovery arcs through stress and treatment, not cultural inheritance. These are separate authorities.

CohortChild has parent IDs, birth day, dose bands, baseline-correction state, free-text moralityMemory, maturation state/day, and loss state. The reviewed DTO and capture/restore path contain no explicit folklore ID or origin-sector field. The maturity method is one-way and the work-eligibility comment defines its current meaning; it is not a complete adulthood or ideology transition. The data catalog’s presence in the JournalCodex proves a readable content route, not that an individual survivor heard or internalized a particular tale. No content association should be inferred from a matching tag or birthplace phrase.

### Authored versus generated contract

Authored truth includes the original folklore entry, its source sector/time/theme, each character’s specific recollection, and the available response text. Runtime state includes a verified cohort member’s current life-stage, quest outcome, relationship, active belief profile, and location availability. A generated variation may choose among reviewed dialogue variants only when its conditions are true and the selected variant is persisted by the correct owner. It may not generate a belief, infer a childhood source, or turn a story’s theme into a political position.

If an individual folklore-origin link is approved, the first design choice is whether it belongs on CohortChild or in an already existing cultural-memory owner. Prefer the CohortSystem only if this fact is truly a cohort-member fact and the current save contract can migrate it; do not create a FolkloreOriginRegistry. Candidate field origin_folklore_ids should be a list only if multiple formative stories are necessary. A singular reference is smaller but should be selected only by authored birth/childhood content and not by unstable hash order. An empty legacy value means unknown, not “never heard folklore.” Do not reuse moralityMemory as a foreign key: it currently carries a different, string-based story concept and would lose meaning or compatibility.

### Belief and psychology safety

The quest may show a character questioning or reinterpreting a childhood story. That is narrative dialogue, not an automatic faction conversion. Any later belief assignment must call an approved existing belief/ideology owner, respect its current preconditions, and represent an intentional player or world action. The story’s three outcomes are interpretive text choices; none directly applies a profile ID. Political positions should emerge from actual authored dialogue and a reviewed social consequence, not from a single formative rhyme as deterministic personality.

Do not route discomfort or disagreement into PsychologicalArcSystem breakdown triggers. That would medicalize a cultural disagreement and blur the system’s defined stress/therapy owner. If an existing ideological-friction event is used in a later optional layer, its evidence, conversion chance, save behavior, and current day owner must be verified first. A catalog entry alone cannot apply friction.

### Core and expansion data paths

Core can ship with a quest that references the existing folklore ID at the authored quest-template level, without a per-survivor origin field. Expansion can add a typed member-to-folklore link only after architecture approval, current save-owner review, and a migration plan. Keep source story text in its existing canonical catalog; the quest stores a reference and player outcome, not a duplicate prose copy. An adult callback can reference the entry but should not rewrite it. If an annotation is intended to be persistent or player-authored, its owner and moderation/version policy need separate review; MVP uses fixed authored annotations only.

Before promotion, verify the exact consumer and utilization of each folklore source file. The reviewed loader handles bunker_children_folklore.json and bunker_children_folklore_batch_2.json. Do not assume similarly named expansion JSON is in use until a current consumer is found. Data lint should validate unique folklore IDs, quest references, known life-stage predicates, authored line provenance, and safe fallback when an optional origin link is absent. Reachability lint must separately prove the quest is reachable from an actual interface.
### Pass 21B — Provenance implementation choices and review gates (DRAFT)

The story needs to distinguish a catalog entry that exists from a person who heard it, remembers it, or associates it with a particular place. Those are separate facts. The first implementation should avoid a new universal folklore-memory subsystem.

#### Candidate implementation levels

**Level 0: authored reference only.** Quest and dialogue content refer to stable folklore IDs. The current codex remains a browsable catalog. Character dialogue states their relationship to the tradition in authored text, with no claim that the system tracks individual exposure. This is the smallest safe implementation and supports the arc without changing cohort saves.

**Level 1: quest-local exposure facts.** If the current quest owner already supports durable step facts, store whether the player saw the clue and which authored account they heard. These facts belong to that quest instance and do not claim to represent the entire cohort's memory. This level is appropriate for branching dialogue when existing quest persistence is sufficient.

**Level 2: typed per-person origin link.** Only after a signed architecture decision, consider a stable reference between a cohort member and one or more folklore entries, plus an exposure source and provenance. This would require schema evolution, capture/restore, migration defaults, deterministic serialization, validation against loaded folklore IDs, and a clear owner. Do not add it merely to support one quest. Determine whether the correct home is CohortSystem or a separately authorized narrative-memory owner before implementation.

#### Data contract if Level 2 is approved

The minimum concept is not “belief.” It is a provenance record: person ID, folklore entry ID, how the person encountered it, and whether the record is authored baseline or witnessed during play. Any recall strength, interpretation, or ideological position would be a separate domain fact and must not be inferred from exposure. A stable ID is preferable to copied prose. Missing legacy fields migrate to “no tracked provenance,” not an invented exposure. Unknown catalog references produce an integrity diagnostic and a safe content fallback.

Avoid a generalized framework with arbitrary tag bags, event scripting, or procedurally generated childhood history. The case is narrow: a few authored accounts, a few known people, and player-observable consequences. Expansion to other folklore traditions should be justified by real content volume and repeated use.

#### Boundary matrix

| Concern | Existing authority to consult | This plan may request | It must not create |
|---|---|---|---|
| Folklore prose and IDs | DailySurvivalCatalog and narrative JSON loader | stable references and content validation | a duplicate folklore catalog |
| Individual maturation/work eligibility | CohortSystem | explicit provenance only after approval | a replacement cohort registry |
| Developmental stage | ChildDevelopmentSystem and education owner | one reviewed stage contract | dialogue-local age math |
| Belief doctrine | belief-movement data and spiritual loader | a voluntary authored response | automatic belief assignment |
| Ideological compatibility | IdeologicalFrictionSystem | no effect unless separately authored and approved | a story-side affinity ledger |
| Psychological stress/recovery | PsychologicalArcSystem | ordinary dialogue tone | clinical consequences for disagreement |
| Expedition occurrence | location selector | optional clue/site request | a quest-owned map generator |
| Persistence | current save-section owners | migration plan for an approved field | a second save store |

#### Data integrity and authorship gates

Each authored link must use an existing stable ID, identify the consumer, and specify behavior for missing optional content. Validators should distinguish unknown required references from missing optional context. Content must not duplicate the codex prose into a second JSON file just to give a quest its own copy. Where a scene needs a short excerpt, use the established narrative content pattern or a canonical reference, subject to current schema support.

Reviewers should ask: can a player understand why this person tells this version; can the story work when the optional expedition site is absent; can older saves load without inventing a remembered story; is each claim about a character supported by authored evidence; and can the mechanic be removed without corrupting unrelated cohort, belief, or psychology state? A “no” requires a smaller design or a new recorded decision.
### Pass 22A — Expedition aftermath evidence and generated rumor boundary (DRAFT)

The proposed survey story crosses several data layers. The plan must keep authored fact, expedition result, player observation, rumor, and faction interpretation separate. An empty cargo list is not evidence that nothing happened; a completed expedition is not proof that the objective succeeded; an event in a playtest artifact is not automatically campaign truth.

#### Provenance ladder

**Authored content:** destination descriptions, expected hazards, survey objective definitions, debrief lines, and any known faction interests. These records are static and stable. **Runtime result:** the canonical expedition outcome, phase, location ID, and available result facts. **Player-observed evidence:** a clue actually inspected, an encounter actually resolved, or an authored report actually heard. **Quest conclusion:** verified, disproved, or inconclusive, reached by a quest owner from explicit evidence. **Public account:** a rumor only if an existing information-flow owner accepts and stores it. **Faction reaction:** a standing or access consequence only when the faction owner has a rule that consumes the specific fact.

Never collapse these levels into a single “world remembers” flag. A generated text variation cannot manufacture a factual observation. A chosen debrief line cannot retroactively make the party visit a site. A rumor’s truthfulness rating does not turn unverified report into verified campaign state.

#### Existing-owner map and gap statement

Current source exposes ExpeditionSystem completion and failure events, a retreat-to-inbound path, and a terminal Fail method. ExpeditionHostSession owns the expedition aggregate, including expedition state, vehicle state, known locations, and DiscoveryConsequenceSystem state. The host currently updates a display LastEvent for completed and failed outcomes. RumorSystem has its own rumor network, information hubs, propagation/interception events, and state. StandingRecordEngine tracks location-oriented record mutations. Faction standing has separate owners. These facts establish available neighboring authorities, not an existing bridge among them.

The reviewed host does not prove that a completed-but-unsuccessful quest return automatically creates a rumor, changes standing, updates a location record, or advances a generic quest. The plans therefore propose an explicit adapter contract and leave its implementation gated. Do not add a parallel aftermath ledger, rumor queue, standing score, or discovery journal.

#### Minimal permanent/generated split

Permanent authored records should define: survey objective ID; valid destination IDs; what evidence satisfies each conclusion; authored debrief speaker/lines; and any valid fallback location/evidence pair. Generated runtime values may include the actual completed expedition ID, the party/character IDs already exposed by its owner, day from the campaign calendar, and result facts supplied by existing systems. Do not procedurally generate a survivor identity, a new faction, an unexplored location ID, or a rumor headline that asserts unobserved facts.

If the rumor owner accepts a player-authorized report, the report should reference its source quest/result and preserve uncertainty in its wording. If that owner does not support source references or suitable verification classes, stop at local journal and dialogue output. Do not duplicate RumorSystem records inside quest state. Similar rule for location memory: pass a verified location mutation through its current owner, and do not write directly to the map catalog.

#### Stable evidence packet proposal

An adapter proposal may carry a deterministic correlation key and typed references such as quest ID, expedition ID, source location ID, observed-evidence IDs, conclusion class, and originating campaign day. This is not a proposed new persisted model yet. First determine whether existing quest facts and provenance records can already express the needed links. Any added state requires a named save owner, schema/migration decision, stable ordering, capture/restore path, and tests under the project’s test policy. Missing legacy references must resolve to “not tracked” or “unknown,” never to a fabricated negative claim.

#### Review questions

Does each player-facing sentence identify whether its source is a direct observation, a team report, a secondhand rumor, or a conclusion? Is the claimed destination visit backed by a visit event? Can the narrative function if rumor propagation is unavailable? Can old saves load with no aftermath record? Is a standing change causally tied to a named owner and a deliberate player choice? Can the bridge be disabled without changing ExpeditionSystem's terminal lifecycle? Any unresolved answer keeps the story proposal local and non-mutating.
### Pass 22B — Authored survey records and runtime fact contract (DRAFT)

An authored survey objective should identify the question it can answer, the location references it may use, and its evidence rules. It should not embed a route map, a current weather result, or a claim about who currently controls the region. Those belong to runtime authorities or authored world-state inputs.

#### Candidate content record, subject to the live schema

| Field concept | Meaning | Static or runtime | Validation |
|---|---|---|---|
| objective ID | stable authored survey objective | static | unique stable snake_case identifier |
| destination IDs | existing legal location references | static | each reference resolves through current location authority |
| evidence IDs | allowed authored observation types | static | known evidence vocabulary; no free-form effects |
| success rule | predicate for a verified result | static | explainable from current event fields |
| contradiction rule | evidence that disproves a claim | static | must not equate absence with contradiction |
| fallback clue | authored clue for unavailable destination | static | clue references a real follow-up or remains narrative-only |
| expedition result ID | the concrete run associated with the quest step | runtime | stable ID from existing owner, if exposed |
| visit/observation facts | what the party actually encountered | runtime | emitted only by the owner of that interaction |
| conclusion class | verified/disproved/inconclusive | runtime | produced by the quest owner from declared rules |

This is a design table, not a request to add a new JSON schema. The existing quest catalog or event contract may already represent some or all of these concepts. Reuse supported fields; record any mismatch as a bounded premise finding.

#### Authored variation versus generated text

Safe generated variation may choose among authored phrasings that express the same known fact and uncertainty level. It may not select a more dramatic cause, invent a witness, infer intent, or change the conclusion class. For accessibility and localization, all alternatives must preserve the same player promise and use separate stable keys. A generated map note must reference an existing place and must not imply that an unselected location was visited.

#### Data lifecycle and migration

If the story remains stateless beyond the existing quest, no migration is needed. If a new durable cross-reference is proposed, first search existing quest-step, expedition ID, and campaign provenance fields. If a gap remains, document the owning save section, version bump, deterministic ordering, duplicate policy, checksum implications, and legacy fallback. Do not introduce a field that can be reconstructed from a canonical event unless the event itself is not persisted and the architecture decision explicitly assigns the new field as its durable owner.

#### Content utilization gate

Every survey objective and prose variant must have a runtime consumer. Codex visibility, catalog loading, or a world-bible mention is insufficient. The implementation packet should name the loader, runtime query, host route, observable player output, and disposition for a missing reference. An optional variant may be omitted safely; a mandatory missing destination must become a clue, delay, or explicit invalid-content diagnostic instead of a silent dead end.
### Pass 23A — Source-free rumor provenance boundary (DRAFT)

The world-bible prompt permits kernel-less ambient chatter only at explicitly noise-dominant stations. The current RumorSystem does not encode that distinction. WastelandRumor carries origin location/day, subject type/ID, headline/description, truthfulness, decay, propagation speed, interception state, and reached hub IDs. RumorSystem.GenerateRumor accepts those values and allocates a sequential ID. It does not accept a source event, provenance class, or hub policy. InformationHub stores credibility, bias, and location; the catalog schema additionally carries daily rumor capacity. No reviewed source marks an existing hub noise-dominant.

#### Non-equivalent states

“Source not yet found,” “source not recorded,” “source known but unverified,” “deliberate deception,” and “ambient-only noise” are different states. Do not encode all of them as low truthfulness. RumorSystem currently decays truthfulness by a configured amount and removes a rumor when truth reaches zero or its age exceeds thirty days. A low numeric value can therefore mean aging, not source quality. A high value can make CreateBriefingReport mark an item verified at the 0.80 threshold. Neither behavior by itself expresses epistemic provenance.

Likewise, a rumor classified as Event or Faction is counted as a threat in the current briefing report. Source-free ambience must not accidentally appear as a threat/opportunity or as a verified event. Do not use an existing enum member or overload the `bias` string to hide a new semantic class; any schema change needs an explicit owner review.

#### Candidate authored contract

A future proposal may add a typed origin policy to an information hub, with carefully enumerated values such as event-backed only, attributed-report allowed, and ambient-only. A rumor record may then refer to a provenance class and optionally to a source event or testimony ID. Ambient-only means there is intentionally no event kernel; it does not mean the rumor is false, harmless, or low quality. Attribution remains “heard at hub X,” not “caused by event Y.”

The exact field names, enum values, defaults, migration, and UI semantics are undecided. Legacy hubs must default to the current behavior or to a safely restrictive policy approved by the information-flow owner; they must never silently become noise-dominant. Legacy rumor entries with no source metadata must not be retroactively labeled ambient-only. Unknown values should fail validation and be omitted from the ambient-only consumer rather than guessed.

#### Content versus runtime separation

Authored permanent content defines the permissible hub policy, approved rumor text, and any optional investigative dialogue. Runtime content records whether a rumor was generated, heard, propagated, intercepted, investigated, or source-checked. Generated text may vary wording inside an approved semantic band, but may not invent an underlying event or increase confidence. A player’s investigation conclusion is another fact and should remain in the quest owner, not be duplicated in the rumor record unless the rumor system explicitly owns correction status.

#### Save and integration questions

RumorNetworkState already persists rumors and hubs through its current save path. Adding fields requires capture/restore parity, schema migration, checksum/serialization review, and deterministic behavior. Before implementation, inspect the current information-flow host and save registration; decide whether hub policy is immutable loaded content or part of save state. Prefer static catalog policy if it does not change during play. If a live hub policy can change, identify the owning command and persistence contract. Do not add a second rumor ledger or a sidecar source registry.

#### Gate before any content batch

First inventory current rumor consumers and report projection, then assign a hub only if the authority owner approves its mode. Validate that every ambient rumor belongs to such a hub, has no required event subject ID, cannot be presented as verified, and cannot produce threat/opportunity counts by accident. Content usage must be observable through the existing utilization pipeline. Until these gates pass, the Third Bell story remains prose and architecture DRAFT, not an eligible authored rumor record.
### Pass 23B — Schema, migration, and briefing semantics (DRAFT)

The ambient-only proposal crosses authored catalog data, runtime rumor records, saved RumorNetworkState, and the briefing read model. All four surfaces need a compatible interpretation. A catalog field alone cannot make source-free rumor behavior safe if runtime records and briefings still treat the rumor as an ordinary high-confidence event.

#### Candidate semantics to decide before names

The information-flow owner should decide whether ambient-only is a **hub policy**, a **rumor provenance value**, or both. Hub policy restricts where source-free content may originate. Rumor provenance describes the individual record. Using both is more expressive and safer but adds schema and save complexity. The owner must define how an ambient report is interpreted at a non-ambient hub, whether it may propagate, how briefing verifies it, and which story consumers may query it. Field names are intentionally not prescribed here.

#### Compatibility matrix

| Saved/catalog record | Proposed handling | Reason |
|---|---|---|
| Legacy hub without policy | remain current-mode, never ambient by default | prevents accidental hub reclassification |
| Legacy rumor without provenance | preserve existing interpretation | avoids rewriting player history |
| New ambient rumor at approved hub | classify as source-free ambient | explicit authorship and policy gate |
| Ambient rumor referenced by unknown hub | reject or safely omit | unknown policy cannot authorize source-free entry |
| Unknown provenance enum | validator error or safe non-display | do not guess semantics |
| Missing event source on ordinary event rumor | retain current validation behavior or report integrity issue | do not relabel it ambient automatically |
| Expired ambient rumor in old save | preserve existing expiry rules | the story does not override rumor lifecycle |

#### Briefing contract

The current briefing projection sets `IsVerified` based on a numeric threshold and classifies Faction/Event subjects as threats. A future ambient presentation must explicitly prevent these derived values from misleading the player. Possible designs include a typed confidence status with values such as unverified, source-unknown, corroborated, and verified; or a separate ambient information category that the briefing renders without threat/opportunity classification. This is a proposal only. Do not reinterpret `IsVerified` globally or change ordinary rumor behavior without a bounded migration and compatibility review.

The briefing should show three separate facts: where the account was heard, whether it has a known source, and whether an investigation corroborated it. Hub credibility and rumor truthfulness remain independent data until the owner explicitly defines their relationship. A high-credibility hub can host honest uncertainty; a low-credibility hub can repeat a sourced report. Neither number alone is proof.

#### Content schema and utilization

Ambient rumor definitions should be validated against the approved hub policy and consumer path. Optional narrative text may be omitted if invalid, while a required production build should fail the content gate with the row and reason. No definition should use a synthetic location or event ID simply to satisfy the existing required fields. If current RumorSystem requires a subject type/ID, the owner must design a safe representation rather than passing a fake Event ID.

The content-utilization scanner should distinguish intentional ambient records from dead data only if a real runtime consumer exists. A file on disk, codex row, or self-test fixture is not a player route. Each entry needs a consuming surface, a hearing/discovery path, and a disposition on expiry.
### Pass 23C — Authoring validation and negative-claim discipline (DRAFT)

The content gate for ambient rumors should validate both presence and absence claims. Presence is straightforward only when the source is actually observed. Absence requires a bounded search scope. A record saying “no source exists” is too strong unless the world model can prove global absence; this design assumes it cannot.

#### Validation checks

1. Every ambient record names an approved hub policy and a real hub ID.
2. Its provenance says source-free/ambient using an approved typed value, not a free-text convention.
3. It does not include a fabricated event subject ID to satisfy ordinary rumor requirements.
4. Its text does not assert a faction action, location mutation, threat, trade opportunity, or route state.
5. The rumor's truthfulness/verification behavior is defined by the owner and cannot be mistaken for evidence-backed verification.
6. Its origin day and expiry behavior follow the existing rumor lifecycle or an approved migration.
7. Every dialogue outcome references a supported quest/dialogue fact or is explicitly cosmetic.
8. If a linked destination exists, it resolves against the current location catalog and is not inferred from prose.
9. The consumer surface can label the item as source-unknown; otherwise the row is not eligible for that surface.
10. Legacy records retain their old interpretation after schema evolution.

#### Safe conclusion vocabulary

Prefer: “No matching source was found in the checked log”; “One listener reports the sound”; “The report reached this hub”; “The signal was not captured”; “The origin remains unknown.” Avoid: “The sound never happened”; “The hub is lying”; “Nothing was there”; “The gate is safe”; “The report was false.” The first group encodes bounded knowledge; the second makes broad claims not supported by the proposal.

#### Documentation and release notes

Any implementation should update the rumor-system authority document, catalog schema reference, save migration record, and content-utilization mapping. The world bible seed remains a proposal until those owners sign off. Do not edit generated indexes by hand; use their owning generator. This plan itself may record the future acceptance contract but cannot confer implementation ownership.
