# Expansion-series Plan 18 — Expedition Location Selection

**Status:** Architecture proposal; documentation only.
**Numbering note:** This Expansion Planning Wave 1 Plan 18 is separate from global Plans 18, 32, and 49.
**Purpose:** Select a varied set of expedition locations while guaranteeing that accepted quests and critical progression remain possible, and make destinations and fallback behavior visible at dispatch.

## 1. Desired behavior

At every expedition start, the dispatch map presents the locations relevant to that trip. An accepted quest's required destination appears as a map target on every dispatch until the quest is resolved, abandoned, or explicitly blocked. Its presentation follows the player's knowledge: a known location uses its canonical marker; an unknown but discoverable destination uses the quest's clue or signal marker. The system must never reveal secret coordinates solely because the quest engine knows an ID.

The candidate set draws from permanent authored, mandatory quest, critical progression, character, faction, recently discovered, thematic, optional, secret, and temporary pools. Pools are labels over canonical locations, not independent registries. A location may belong to more than one pool but can appear only once in a selected map. Existing map and expedition owners remain authoritative for node identity, discovery, route reachability, and travel.

## 2. Current evidence and owner boundary

Current source shows ExpeditionSystem as the expedition state owner and WastelandMapSystem as the canonical map, route, discovery, marker, and tunnel state owner. Plan 32 already concerns expedition destination wiring; Plan 49 concerns micro-location discovery; Plan 133 concerns expedition discoveries and persistent consequences. This plan defines how a selector could coordinate those seams. It does not claim that the current expedition start currently performs every selection rule listed here.

Before implementation, inspect the live integration ledger, worktree claims, current map/expedition host entry points, quest location bindings, discovery owner, catalog loaders, and persistence. If there is already a selection owner, extend it. Do not add a second expedition, map, route, discovery, or quest-state authority.

## 3. Selection inputs and output

The pure selection decision needs a bounded, immutable input snapshot:

- expedition context: dispatch origin, theme, permitted travel range, and candidate-slot budget;
- quest requirements: active required locations, availability status, and a supported equivalent/clue/failover;
- critical progression gates: current mandatory location requirements;
- map facts: known/discovered nodes, open routes, exhausted or closed sites, and canonical location IDs;
- content traits: pool memberships, rarity band, faction/character gates, theme tags, and mutual exclusions;
- replay input: the existing deterministic RNG stream or stable deterministic ordering input.

The result should contain selected canonical location IDs, a map presentation state for each, unmet requirement explanations, and fallback decisions. It must not mutate map knowledge, quest status, faction standing, inventory, or campaign facts. The host or current owners apply any resulting route/discovery commands.

## 4. Priority order and capacity

Use the following ordered passes. The first two are hard requirements; later passes compete for remaining selection capacity.

1. **Mandatory active quest locations.** Include every accepted quest destination or a supported equivalent. Show it on each expedition dispatch map until resolved. If the exact site is secret, show the authored clue/signal marker at the knowledge level the quest has earned.
2. **Locations required for critical progression.** Include a reachable route or make the next progression gate explicitly unavailable with a clue and reason.
3. **Character or faction locations.** Include only when the relevant actor, faction, quest, and route are currently eligible.
4. **Recently discovered locations.** Prefer a recent discovery when it adds a follow-up action; reduce immediate repetition using existing discovery or encounter history if available.
5. **Thematic locations matching the expedition.** Rank by authored theme compatibility after hard requirements are satisfied.
6. **Optional exploration locations.** Fill remaining slots from reachable, non-exhausted sites.
7. **Rare or surprise locations.** Consider last, under an authored rarity ceiling and only when they do not displace a hard requirement.

Hard requirements are not dropped to satisfy a fixed map size. If the required set exceeds a proposed soft slot budget, expand the display budget if the existing UI can do so. If runtime constraints truly prevent inclusion, resolve the quest before dispatch into a visible blocked/delayed state or a validated equivalent/clue route. Do not dispatch an expedition that silently removes the only objective destination.

## 5. Exclusions, rarity, and deterministic variety

Exclude a candidate when its ID is invalid, its route is unreachable from the expedition origin, its authored gate is false, it is exhausted and one-time, a required actor/faction is absent, it conflicts with another selected encounter or location, or a critical prerequisite has not been met. Exclusion reasons should be inspectable by validation and available to player-facing fallback copy when the player needs to act.

Apply exclusions before weighting. Deduplicate by canonical location ID after pool membership is combined. Keep selection order stable by sorting IDs with ordinal comparison before seeded choice. No selection may depend on dictionary order, wall-clock time, system randomness, or a hidden UI callback.

Rarity should be an authored band and a selection cap, not an unexplained probability scattered through code. The MVP may use common, uncommon, and rare bands with at most one rare/surprise slot per expedition. Exact rates are a balance decision requiring seeded simulation and real candidate counts. Rare content may not contain the only critical quest route.

Avoid predictability by rotating thematic compatibility, choosing optional candidates without replacement within one dispatch, and suppressing immediate repeats when an existing owner can prove the last appearance. Do not add a new visit-history save section just to support variety. If no current owner records appearances, deterministic per-dispatch variety is acceptable for MVP and persistent anti-repeat behavior is deferred.

## 6. Map visibility and discovery rules

Map availability, map visibility, and physical reachability are three separate questions:

- **Available:** the location's authored and campaign gates allow it to be selected.
- **Visible:** the player has enough knowledge to see the location or its clue on the map.
- **Reachable:** the current route authority can plan travel from the expedition origin.

A quest accepted with a known target pins the exact marker at every expedition start. A quest accepted from a clue or environmental discovery pins the clue marker and labels the required action. Discovery changes visibility only through the current map/discovery owner. A secret pool candidate stays hidden until a current discovery condition is met. A visible but unreachable destination shows its route blocker and an authored next step.

Map UI displays status truthfully: required, optional, hinted, discovered, unavailable, or complete, using the panel's existing interaction patterns. Quest and map panels must not invent their own marker states or discover locations on hover.

### Quest-only location rule

A quest-only site may be selected only while its owning accepted quest is active or while an authored discovery explicitly reveals it. It uses a stable location identity and an existing map anchor/route contract. It appears on every expedition-start map while the quest requires it; the marker may describe a clue rather than expose the site when the player has not learned its exact location. When the quest resolves, remove the required marker but retain any discovery, journal, or world result through its existing owner. If the quest is abandoned, show the authored cleanup or continued clue state. Do not leave a permanent invisible candidate that can later appear as a surprise.

### Location prose cards

These short examples are provisional descriptions rather than approved catalog IDs.

- **The Sump Clerk's Landing:** A stairwell opens onto a concrete platform above a drained service channel. Someone has painted a tide line at shoulder height, then crossed it out and written the date of the last pump test. A discovery quest can ask whether the mark is a warning or a maintenance record.
- **The Cinder Parcel Room:** The door is swollen shut, but paper slips still pass under it. An accepted quest pins the room as a destination at every dispatch; before the route is known, the player sees a “parcel-room lead” marker tied to a clue.
- **The Night Vent:** The map shows a narrow connector with a blue access tag. It is optional for ordinary travel and mandatory only for one quest objective. If its route is closed, the map keeps the quest marker and offers the tested service-stair alternative or a visible delay.

At expedition start the brief can use one compact sentence: “A required lead remains open; the route is unconfirmed. Follow the marked service channel or return with another map.” This separates the player's map knowledge from the selector's internal candidate ID.

## 7. Fallback when a required location cannot spawn

Fallback resolution happens before the expedition begins and is visible in the quest/map read model:

1. **Equivalent location:** substitute only an authored location with the same objective capability, accepted by the quest definition and reachable from this dispatch origin.
2. **Explicit delay:** retain the quest as Blocked or the closest truthful current lifecycle/read-model state, name the reason, and state what must change.
3. **Clue route:** create or reveal an authored clue that leads to another reachable canonical site; the clue is a real content entry and has a consumer.
4. **Fail-forward route:** change the objective through the quest owner and provide an alternate route with its own success condition.

An unavailable required site cannot be silently removed, marked complete, or replaced by an arbitrary string ID. If no fallback is valid, prevent acceptance or dispatch and explain the blocker before the player commits resources.

## 8. MVP, expansions, and acceptance

**MVP:** One location pool over existing nodes, hard inclusion for active quest destinations, map pin at each expedition start, deterministic optional fill, one explicit unavailable-site fallback, and validator coverage for dangling and unreachable IDs.

**Optional layers:** faction-specific map views; weather and hazard themes; recently discovered follow-ups; temporary event sites; secret/rare sites; more nuanced anti-repeat; weighted candidate simulation; expanded map UI. Each layer depends on proven data consumers and owner state.

Acceptance requires every active required destination to remain possible, every required marker to appear on dispatch at a knowledge-appropriate level, optional variety to be deterministic, unavailable locations to produce a visible fallback, invalid routes to be excluded, and quest/map state to remain owned by existing authorities.

## 9. Integration course

**Phase 0 — Premise audit:** Compare current dispatch selection against Plans 32, 49, and 133. Inspect exact quest binding shape, map knowledge semantics, route reachability, existing selection randomness, and active path claims.

**Phase 1 — Selector contract:** Define the immutable input, selection result, hard requirements, soft capacity, and exclusion reason vocabulary. Keep the decision pure and add no durable state.

**Phase 2 — Data slice:** Choose a small set of existing locations and tag only valid memberships. Validate IDs, gates, objective capability, route reachability, rare-content limits, and fallback references.

**Phase 3 — Owner wiring:** Read requirements from the quest owner, resolve routes through the map owner, use current RNG, and expose the decision to the existing dispatch panel. Route any blocked/delayed state through the quest owner.

**Phase 4 — Verification:** With claimed paths, run focused selector, map, quest, expedition, and catalog validation. Verify multiple active quests, insufficient soft capacity, missing location, hidden destination, invalid route, duplicate pool membership, save/load of required state, and paired same-seed selection.

**Phase 5 — Handoff:** Report exact files, owner contracts, selected and excluded examples, focused commands, known constraints, and any unresolved capacity/architecture decision. A full test suite is outside this plan.

## Continuation pass 2 — deterministic selection and dispatch cases

### Selection procedure

The selector should have bounded work and a result that can be explained. This pseudocode defines ordering and safety, not a required class name:

~~~text
input := snapshot current expedition origin, quest requirements, map facts,
         authored candidate metadata, campaign gates, and seeded RNG
validate every referenced location ID against the canonical catalog
required := resolve active quest destinations and supported alternatives
required += resolve critical progression destinations
if required contains an invalid or unreachable target:
    resolve equivalent, explicit delay, clue, or fail-forward route
    if no route is valid, return a pre-dispatch blocker
selected := deduplicate required locations by canonical ID
for each lower-priority pool in fixed authored order:
    eligible := filter candidates by gate, reachability, exhaustion,
                mutual exclusion, and already-selected ID
    sort eligible by stable ordinal ID
    select within the remaining soft slot budget using the seeded RNG
return selected IDs, visibility states, exclusions, and fallback notices
~~~

The implementation must avoid an unbounded “reroll until a location fits” loop. Filter first, then select from the finite eligible list. If a category has no eligible candidates, continue to the next category and record the empty result for diagnostics. Selection output is stable for a fixed snapshot and RNG state.

### Candidate metadata

Candidate metadata can remain on the canonical location definition or in an existing pool catalog if current loader ownership supports it. Proposed fields are location ID, pool tags, theme tags, rarity band, required capability tags, actor/faction gates, minimum knowledge state, one-time/repeatable policy, exclusion group, and fallback reference. Pool membership cannot contain its own copy of route, discovery, hazard, or availability truth.

An objective capability tag describes what a location can support, such as “inspect evidence” or “deliver supplies.” Validate every tag against an objective consumer. Do not rely on free-form tags that no code or authoring tool understands.

### Worked dispatch cases

| Dispatch case | Selection result | Player view and fallback |
|---|---|---|
| One accepted quest, target reachable | Target is selected in hard pass; optional sites fill remaining slots. | Exact destination pinned on every dispatch until resolution. |
| Two accepted quests share one site | Canonical ID dedupes to one marker while both quest bindings remain attached. | Marker indicates both objectives without duplicating the location. |
| Two quests require separate sites and soft capacity is one | Both required sites enter the hard set; the soft cap expands. | Show both targets; optional content yields its slot. |
| Required site is closed by a canonical gate | Selector checks only authored equivalent or clue routes. | Show the alternative or a visible blocked reason before dispatch. |
| Secret site is required after a clue quest | Required quest is preserved but visibility follows the clue's earned knowledge. | Pin the clue/signal marker, not the secret's internal coordinates. |
| Recent optional site repeats | Existing discovery/encounter history may reduce its priority. | Another eligible site can win; no persistent anti-repeat state is fabricated. |
| All optional candidates are excluded | Return the hard set and an empty optional list. | Dispatch remains valid; the UI does not show fake map pins. |
| Rare pool is empty | Skip rare selection. | No placeholder or forced rare encounter. |

### Required quest map behavior

The map state at dispatch should identify the origin, selected candidates, quest binding IDs, visibility level, and whether a location is required, hinted, or optional. The UI may emphasize a target but must keep the map owner's canonical knowledge state. A dispatch refresh cannot mark a node discovered. When a quest becomes resolved or abandoned, the required highlight is recalculated from the quest owner before the next expedition.

For a quest accepted after an expedition begins, update the current dispatch only if the host and expedition lifecycle can safely accept a new destination. Otherwise, keep the quest active and show the target on the next dispatch. Never mutate a dispatched route in the background.

### Selector observability and performance

Diagnostic output should be bounded: candidate count, eligible count by pool, exclusion reason counts, chosen IDs, fallback kind, and seed/replay identifier if already available. Avoid logging private narrative text or dumping full campaign state. A normal dispatch should not rescan every catalog or rebuild all map art; validate immutable catalog data at load time and filter the already-loaded candidate list per dispatch.

The MVP complexity target is linear filtering of the finite candidate pool plus stable sorting of the eligible subset. If measurements show the candidate catalog grows beyond comfortable dispatch cost, profile the actual path before adding caches. Any cache is derived and invalidated by canonical owner events; it never persists a second location truth.

## Continuation pass 2 acceptance

The selector contract is ready for implementation audit when the hard set cannot be displaced by optional or rare content, the seven pool passes are stable, the no-valid-location result is explicit, required marker visibility respects player knowledge, and same-seed selection over the same snapshot returns the same result.

## Continuation pass 3 — pool semantics, dispatch trace, and recovery

### Pool membership semantics

The selector reads pool membership from authored metadata but computes eligibility from live owner facts. Pool tags are not mutually exclusive:

- **Permanent authored:** stable places available to ordinary exploration when their authored gate and route permit.
- **Quest-required:** active objective targets, accepted quest alternatives, and a clue-backed target.
- **Critical progression:** only locations that gate the current main campaign step.
- **Character/faction:** sites whose speaker, faction, or arc is currently available.
- **Recently discovered:** locations with a new authored follow-up; “recent” uses an existing event/day fact if one exists.
- **Thematic:** weather, travel intent, resource need, or expedition purpose supported by current inputs.
- **Optional:** reachable general-interest sites.
- **Secret:** hidden candidates with a clue or explicit discovery rule.
- **Temporary:** event-backed sites with a real start/end condition owned by an existing campaign/event clock.
- **Rare/surprise:** optional, authored, noncritical candidates selected after all other pools.

A site can be tagged both quest-required and secret. The hard quest requirement wins selection priority, while the visibility rule still controls what the player sees. A site tagged temporary is invalid if its expiry depends on an invented clock. A faction site is filtered when the current faction actor or route is absent.

### Priority and exclusion precedence

Resolve conflicts in this order:

1. Invalid ID or invalid definition: exclude and report a content error.
2. Closed hard requirement with authored fallback: resolve fallback before selection.
3. Closed hard requirement without fallback: return a pre-dispatch blocker; do not silently lower it.
4. Quest/critical hard set: preserve all compatible required destinations.
5. Contradictory one-time or mutually exclusive candidates: retain the higher-priority authored requirement and reject the conflicting optional candidate.
6. Route reachability and current party capability: use the canonical planner and current expedition inputs.
7. Optional pool ranking, thematic fit, recent-repeat suppression, and rarity.

The selector does not decide that a quest is no longer important because the map is crowded. Its hard set is derived from accepted quest state and critical progression facts before soft capacity is applied.

### Example deterministic trace

Assume the dispatch origin is the shelter map node. The player has accepted two quests: one requires the known waterworks site; the other requires an unknown workshop whose clue has already been discovered. Critical progression also points to a radio relay. The soft map budget is two, and a rare candidate is eligible.

The result contains all three required locations because the hard set exceeds the soft budget. The waterworks and relay appear by exact map identity. The workshop appears as its clue marker until the map owner reports the exact place discovered. The selector then considers character/faction, recent discovery, theme, optional, and rare pools. The rare candidate is skipped if the presentation or travel budget cannot support a fourth marker. The dispatch remains stable on replay with the same snapshot and seeded stream.

If the workshop route has closed, the quest's authored alternative is evaluated before optional selection. If that alternative is valid, replace only the workshop target and explain the change in the quest/map read model. If none exists, return a blocker with the quest ID and reason; the player can still cancel dispatch without spending supplies.

### Marker lifecycle

| Quest timing | Dispatch map behavior |
|---|---|
| Quest available but not accepted | No mandatory marker; a discoverable offer may appear through its ordinary board or authored clue. |
| Quest discovered but unknown destination | Show the authored clue/signal marker where the player can act on it. |
| Quest accepted before expedition | Show exact target if known or clue marker if not; repeat at every dispatch start. |
| Quest accepted while expedition is active | Do not rewrite a committed route unless current expedition owner supports a safe route change; show it at the next dispatch. |
| Quest blocked by a closed location | Retain the marker with blocked status and actionable reason or validated alternative. |
| Quest completed/resolved/abandoned | Remove required status; retain ordinary discovered map knowledge owned by the map system. |
| Quest expired or failed forward | Replace the objective marker only after the quest owner publishes the alternate objective. |

The map may visually group multiple active objectives at one location. It must not merge their quest states or mark all of them complete when one objective succeeds.

### Failure diagnostics and player recovery

Use a stable exclusion reason family for diagnostics: unknown ID, catalog-invalid, hidden-not-discovered, gate-closed, route-unreachable, party-ineligible, exhausted, conflict-group, event-expired, and no-safe-fallback. These reason codes are useful to validators and tests. Player copy should translate only reasons that support a useful action; do not show internal enum names.

The pre-dispatch recovery panel should say what is affected, whether the expedition can proceed, and what restores the required content. Example: “The north workshop route has flooded. The quest remains active. A maintenance note points to the dry stair; review the alternative before leaving.” A soft optional exclusion needs no modal. A hard requirement with no fallback prevents a misleading dispatch confirmation.

### Performance and reproducibility budgets

Selection work scales with the number of canonical candidates, not the number of pool tags. Deduplicate IDs once, validate immutable metadata at catalog load, and avoid loading every location description when the map only needs IDs and visibility state. If repeated dispatch profiling shows a hot path, cache only derived eligibility keyed to explicit owner revisions/events; invalidate when those facts change. Never use a cache as persistent map truth.

Replay diagnostics should capture the candidate-set version, stable ordered IDs, selected IDs, fallback decisions, and existing RNG stream state/sequence if that instrumentation already exists. Do not add broad campaign serialization to logs. A deterministic selector should have a bounded number of decisions and no retry loop whose number of draws depends on unrelated dictionary ordering.

## Continuation pass 3 acceptance

The pool contract is ready for integration when overlapping tags resolve without duplicate markers, the required hard set survives any soft budget, every active quest marker is re-evaluated at every dispatch, hidden locations retain fog rules, temporary content has a real clock owner, and missing targets produce a safe pre-dispatch result.

## Continuation pass 4 — dispatch transaction, pool interaction, and player-facing recovery

### Define the expedition selection transaction

Treat a new expedition as one selection transaction with a captured input view. The planner reads a stable seed/sequence from the existing deterministic RNG owner, current expedition constraints, active quest requirements, canonical location definitions, and visibility facts. It computes candidates, reserves hard requirements, fills remaining capacity by weighted categories, writes the resulting selection through the existing expedition/map owner, then exposes the map. Opening the map, moving a cursor, or refreshing the panel must not rerun selection.

The transaction has three distinct outputs: the immutable list of selected location IDs for this expedition; a derived map presentation that knows whether each selection is revealed, hinted, or hidden; and a diagnostic record sufficient to explain exclusions and fallbacks. Only the canonical owner decides whether a site was visited, discovered, cleared, or changed. A UI marker is not evidence of discovery, and generation of an ID is not evidence that the site is reachable.

### Make pool overlap predictable to the system and varied to the player

Each location may carry several eligibility tags, but candidate identity is unique. Resolve the candidate set once by stable ID before applying category quotas. Then evaluate hard quest obligations, critical progression, faction or character constraints, recent-discovery preference, expedition theme, optional exploration, and rare surprise in the priority order already established by this plan. Category membership can influence weighting after hard reservations; it cannot multiply a location's chance merely because that row has three tags.

Use a constrained preference rather than a rigid quota for soft pools. For example, when an expedition theme requests two water-adjacent sites but only one valid site remains after route exclusions, select that one, report the unmet soft preference, and fill the other slot from the next eligible pool. Do not duplicate a site to satisfy a count. A location excluded as already visited can still be eligible for a quest revisit only when the quest definition explicitly permits that return and the current owner can present a changed state.

Rare content is a bounded chance among remaining eligible slots. It must not displace a mandatory active quest target or critical progression site. Secret content has two independent conditions: it must be selected by valid rules and its reveal policy must remain hidden until the discovery fact or authored clue occurs. The player may see a silhouette, rumor, or clue marker only if the content author supplied that presentation.

### Map visibility and quest-only destinations

At expedition start, map visibility is computed from the selected set and current discovery facts. A mandatory location can be map-visible, clue-visible, or undisclosed depending on the quest's arrival design. If the player must know the destination before dispatch, the quest presentation must communicate that requirement and the destination must be guaranteed. If the quest is environmental-discovery content, the selector must not leak a precise map pin before its clue is found. After a clue is found, refresh the presentation from the discovery owner without rerolling the expedition.

Quest-only locations should be modeled as ordinary canonical location definitions with a restrictive eligibility predicate and explicit fallback. They should not be fabricated as display-only markers. If no valid quest-only site can enter the expedition, return one of three explicit decisions: equivalent site substitution; quest delay with a clue or changed objective; or dispatch refusal with a clear explanation. Which decision is valid belongs to quest content and its owners, not a generic selector guess.

### Recovery trace for several concurrent requirements

Suppose two active quests request distinct sites and a faction request accepts either of two sites. First reserve the two exact mandatory IDs if eligible. Next bind the faction requirement to the eligible alternative that introduces the least route conflict, using stable ID tie-breaking after deterministic weighting. Fill optional slots only after these reservations. If one exact ID is unavailable, consult that quest's declared equivalence set; if none is valid, mark the quest as delayed and provide its authored clue route. Do not silently substitute the other quest's location when that site cannot prove the missing objective.

The trace shown to a player should describe the decision in fiction and action terms, not expose internal pool scores. “The clinic yard is unreachable after the washout. A chalked service route leads to the pump annex; the medical request can continue there.” The journal records the route change only after the appropriate owner accepts the replacement. If the player declines, preserve the original quest and let the next dispatch reconsider it.

### Required focused verification cases

Exercise: two quests sharing one location; two quests requiring separate sites; a location carrying optional and rare tags; a secret site selected but unrevealed; a discovered site excluded by current route facts; missing required site with each allowed fallback; all soft candidates excluded; zero valid candidates overall; same seed and identical inputs; same seed with one changed exclusion fact; and reopening the map without changing selection. Assert stable ordered candidate processing, no duplicate IDs, no soft-pool displacement of hard requirements, and player-visible reason for a delayed task.

The integration receipt records the RNG contract actually used, maximum selected count from the current expedition owner, candidate revision, deterministic tie-break policy, visibility owner, quest equivalence authoring rule, fallback output, and focused replay/UI outcomes. If a required location cannot be represented by the current map or expedition owner, stop at the architecture boundary and request a new scoped design decision.

## Continuation pass 4 acceptance

The dispatch design is ready when selection is a one-time deterministic transaction, location selection and map visibility remain separate, quest-only content uses canonical reachable locations, soft preferences degrade cleanly, and a player can understand every required-location delay or alternate route.

## Continuation pass 5 — boundary with map and encounter owners, and expedition opportunity contract

### Select expedition opportunities without replacing the world map

Current source shows that ExpeditionSystem starts an expedition for a specific ExpeditionDefinition, rejects a destination locked by the damaged-map owner, and rejects a destination whose required discovery has not occurred. WastelandMapSystem already owns canonical nodes, routes, fog/knowledge, discovery, locks, completion, and route estimates. NarrativeEncounterSystem separately selects a micro-location encounter inside an expedition context. Plan 18 proposes a bounded layer that selects an expedition opportunity set from existing canonical destinations; it does not generate a parallel map graph, reroute a player silently, or replace micro-location encounter selection.

The visible result is a dispatch opportunity list over the existing map. It may mark a destination as required by an active quest, recommended by a character or faction, recently discovered, thematically relevant, optional, secret, or temporarily eligible. Each entry resolves to a canonical location definition and existing map node. A location without a canonical ID, map position, route, and ExpeditionDefinition cannot be offered as a dispatch target. A future authored hidden node must first be admitted to the current location/map catalogs and satisfy discovery rules.

F21, Discovery Selection-Context Extension, is a separate filed proposal: it adds season, drought, and skill weights to NarrativeEncounterSystem's micro-location candidate selector. Plan 18 must not reimplement those weights or change water, radio, or greenhouse encounter grants. It consumes location availability and quest obligations at the expedition-planning boundary; encounter weighting inside the trip remains with its existing selector and any separately authorized F21 integration.

### Request contract from quest content

Quest definitions express destination intent in a small vocabulary that the planning layer can validate:

| Requirement kind | Meaning | Selection behavior |
|---|---|---|
| Exact canonical destination | This specific site proves the objective. | Reserve it if unlocked, routeable, and compatible with expedition rules; otherwise invoke its authored fallback. |
| Equivalent destination set | Any member can prove the same objective, with possible authored evidence differences. | Choose one valid member deterministically; store the bound destination with the quest owner if the instance must remember it. |
| Clue-led destination | The player must first find or interpret a clue. | Select a reachable clue source, keep the destination hidden until its discovery fact, and never show a premature pin. |
| Optional lead | The location adds context or a reward but is not necessary for core progression. | Use only remaining capacity after hard requirements and critical progression. |
| Return-to-known-site | The canonical site is revisited to observe an owner-backed change. | Revisit only when the quest allows it and current location state can distinguish the result. |

Each requirement also says whether it is hard, deferrable, substitutable, or optional; its discovery policy; accepted evidence; and what happens when no candidate can spawn. Candidate legality is established first; weights rank only candidates that remain legal.

### Opportunity-set build procedure

At entry to each expedition-planning session:

1. Read the existing map graph and current map/route facts. Never construct map nodes from prose.
2. Gather active quest requirements through a read-only adapter, including exact targets and authored equivalence/clue sources.
3. Deduplicate by canonical location ID and evaluate lock, discovery, route, temporary expiry, actor/location, and critical-progression constraints.
4. Reserve mandatory active quest and critical progression opportunities in stable quest priority order. For conflicts, bind an explicitly authored equivalent or return a visible blocker.
5. Add character/faction and recent-discovery candidates, then thematic and optional candidates, then rare eligible candidates; enforce the opportunity-count limit owned by the current planning surface.
6. Resolve visibility through WastelandMapSystem's discovery/knowledge state. The selector can request a rumor or marker only through the map owner's available seam.
7. Present a stable result until its source revision changes or the player leaves the planning session. Recompute after a relevant owner event; do not reroll on redraw, focus movement, or panel reopen.
8. Pass the chosen canonical definition to the existing preview/start path. That path remains the final validation authority and rejects stale or invalid dispatch.

Selection inputs and output are a derived session view unless existing save ownership explicitly requires an opportunity set to persist across a saved mid-planning state. A completed selection becomes part of the expedition's existing target state through the current ExpeditionSystem; it does not become a second permanent location ledger.

### Example: mandatory quest location and optional shelf evidence

For the Unentered Shelf fixture, the shelter itself is already a valid interaction context and needs no expedition slot. If the player chooses to investigate the alleged neighbor delivery, the active quest can request an authored location or an equivalent evidence source. The selector first tries the exact known destination. If its route is flooded, it can select an authored alternate evidence site only when that site's evidence has equivalent quest semantics. A rumor may lead to the destination while preserving fog. If neither exact nor equivalent evidence exists, the expedition menu says the optional proof route is delayed; the local-hold resolution remains playable.

The selector must not manufacture the neighbor, a hidden site, a map coordinate, or a successful delivery. That would convert an unresolved story into unowned world state. This case demonstrates that optional content can be delayed without blocking the core quest and without making every expedition contain the same locations.

### Exclusion precedence and visibility contract

Hard exclusion conditions include invalid content ID, no canonical ExpeditionDefinition, permanent owner lock, unmet clue-only discovery, no valid route under current route rules, unavailable required actors when the objective depends on them, and an expired temporary definition. A hard quest requirement can only override an exclusion if its authored fallback specifically resolves it; the selector itself does not open locked sites.

Soft exclusions include recently visited preference, repeated-theme reduction, faction conflict when the quest owner allows neutrality, or rarity budget. Apply these after hard requirements. Do not use a hidden rarity draw to make a mandatory target disappear. Hidden means the selector may use a clue candidate internally while map presentation continues to honor fog. Rumored, surveyed, visited, locked, available, and completed presentations derive from current MapNodeKnowledgeState and map status, not from a second visibility enum owned by dialogue.

### Focused integration evidence

Acceptance cases include: mandatory exact target; two requirements competing for one node; equivalent target with deterministic stable tie; hidden clue source; rumored-but-not-discovered destination; route becomes flooded after planning preview; temporary candidate expires; quest is abandoned before dispatch; player opens the panel repeatedly; player changes one relevant map fact; no optional candidate survives; and no map node is legal. The run report records which owner supplied each fact, what the selector excluded, why a fallback was chosen, and whether existing PreviewStart/ExecuteStart accepted the final target.

## Continuation pass 5 acceptance

This location tranche is ready when the planning layer yields a derived opportunity set over canonical map nodes, F21's micro-encounter selector remains untouched, every hard quest request has a legal fallback or visible delay, and ExpeditionSystem's current dispatch validation remains authoritative.

## Continuation pass 6 — constraint resolution, stable selection, and edge-case behavior

### Selection contract as a derived opportunity set

Model one expedition's candidate locations as a short-lived opportunity set built from the current world snapshot. It combines the existing permanent expedition definitions with eligible quest-required, character/faction-relevant, recently discovered, thematic, optional, secret, and temporary entries. These names describe selection inputs; they do not require one mutable pool or registry per category. A definition may qualify for several reasons, but the output contains one canonical location entry with several provenance reasons.

Each candidate evaluation returns a typed disposition:

- **Eligible:** may be offered now.
- **Required:** needed for an active quest or critical progression and can be made available under an authored rule.
- **Substituted:** an explicitly equivalent definition satisfies the same requirement.
- **Clue-led:** the target cannot be pinned yet, but a valid discoverable clue is offered.
- **Deferred:** no valid opportunity exists this dispatch; the quest remains possible later.
- **Excluded:** a hard rule makes the location unavailable and no authored fallback resolves it.

Only Eligible, Required, Substituted, or Clue-led results enter the player-facing result, each with the correct map-visibility form. Deferred results must be surfaced to the relevant quest/journal view when the player needs to understand why progress waits. Excluded candidates should be absent unless the UI has an authored reason to explain them.

### Priority and constraints are separate passes

Do not collapse quest-required into a large random weight. Resolve constraints first, then rank the remaining optional candidates:

1. Load canonical expedition definitions and current map/quest facts.
2. Collect mandatory active quest requirements, preserving exact target, substitution permission, clue policy, and delay policy as authored data.
3. Resolve critical progression requirements through their own explicit route contract.
4. Apply hard exclusions: invalid reference, locked definition, unmet discovery rule, expired temporary content, invalid route, incompatible encounter/world condition, or unavailable required actor without an alternative.
5. Satisfy mandatory requirements with exact targets or explicitly equivalent targets. Never allow optional candidates to displace them.
6. Add eligible character/faction and recent-discovery opportunities within the configured content budget.
7. Select thematic and optional candidates using the existing deterministic RNG stream and a stable candidate ordering.
8. Admit rare/surprise locations only from the remaining legal set and only within the authored budget.
9. Deduplicate by canonical location ID, preserve all reason tags, and compute map visibility separately.
10. Freeze the result for this planning session and pass the selected target to the existing ExpeditionSystem preview/start validation.

Priority establishes whether an opportunity must be preserved; rarity and theme order selection only among legal optional candidates. This avoids a weight configuration accidentally assigning zero practical chance to an essential quest site.

### Stable ranking and replay behavior

Define deterministic candidate ordering before any random draw: stable content ID, then an authored tie-break key if distinct variants share a target. Feed optional selection from the game's existing seeded RNG contract. Do not use wall-clock values, object hash order, randomized string hashes, catalog file order, or a UI redraw as entropy. If eligible candidates are unchanged and the seed/snapshot is unchanged, the same opportunity set must be produced.

Avoid reroll loops. Evaluate each candidate once from a coherent planning snapshot; apply fixed inclusion quotas; draw only the remaining optional subset. Record selection reasons and relevant source revisions in the transient view for debugging, not as new campaign state. If a relevant owner change invalidates the view, rebuild it from the new snapshot and announce a material change before dispatch. If the player reopens the panel without a state change, retain the result.

For a saved mid-planning session, inspect the current save contract first. If the expedition target itself is already persisted, rehydrate that target through ExpeditionSystem rather than persisting an independent candidate list. If no target is committed, derive the view again from canonical facts and the appropriate deterministic stream.

### Exact, equivalent, clue-led, and delay requirements

The quest author marks the fallback semantics; the selector cannot infer equivalence:

| Requirement kind | Valid resolution | Invalid shortcut |
|---|---|---|
| Exact | Offer the named canonical target when its current route and unlock conditions pass. | Silently swap to a thematically similar site. |
| Equivalent evidence | Offer only a pre-authored location/objective that yields the same quest evidence kind. | Treat any site with the same biome or faction as equivalent. |
| Clue-led | Offer an authored clue interaction while preserving unknown map state until discovery is confirmed. | Reveal a precise marker merely because a hidden target is required. |
| Delay-allowed | Keep the quest accepted or active and show a player-readable reason for waiting. | Mark the task failed because the selector had no candidate. |
| Optional lead | Include when eligible and budget allows; absence does not block the primary route. | Promote an optional lead to an undocumented mandatory objective. |
| Return-only | Resolve through the current shelter/character interaction and consume no expedition slot. | Spawn a duplicate location for an interaction already in the current scene. |

Equivalent evidence must be semantically testable. If the objective is “verify that the neighboring shelter received the blankets,” a warehouse inspection is not equivalent unless it provides evidence of receipt. A copied note can prove that a promise was made but not that delivery occurred. Keep requirement purpose and evidence kind explicit in the content dossier.

### Exclusion precedence and no-valid-location policy

The selector reports why a required candidate failed resolution to the quest opportunity adapter. The adapter applies the authored policy in order: equivalent target, clue route, delayed availability, or a visible blocked state with a concrete condition. Critical progression requires a separately reviewed guarantee. A late-game story should never become impossible because a rare-location budget was zero or a thematic pool happened to be empty.

If every candidate is excluded:

1. Preserve the active quest and all confirmed progress.
2. Do not start an expedition with an invented/default target unless the current game explicitly supports that target.
3. Report a safe player-facing explanation through the current planning/journal UI.
4. Offer an authored clue or alternate interaction if one is valid.
5. Otherwise defer selection and state the real prerequisite or route failure.
6. Emit a concise diagnostic with quest ID, requirement ID, and reason code for development review.

If the required location definition itself is missing from a release bundle, treat it as a content-integrity failure during validation. At runtime, old saves resolve through a declared replacement or retired-content fallback. Do not search for a random location with a matching display name.

### Visibility is independent from eligibility

For each candidate, derive map presentation from current discovery knowledge:

- **Exact marker:** player has learned the target and current map rules permit a pin.
- **Regional/approximate marker:** an owner-backed clue supports a region but not a coordinate.
- **Rumor or clue card:** the target is unknown; an authored lead is visible.
- **No map entry:** hidden content has not been discovered and no clue is known.
- **Unavailable marker:** the target is known but currently locked, unsafe, expired, or unreachable, with an accurate reason where appropriate.

Eligibility to serve a quest and visibility to the player answer separate questions. The selector may internally preserve a hidden mandatory target without exposing its exact ID. A clue response becomes visible only after the discovery/quest owner confirms it. The map owner remains authoritative for fog, location identity, and route presentation.

### Opportunity-budget and fairness examples

Consider a dispatch with one mandatory evidence destination, one character-relevant visit, one recent discovery, and four optional slots. The mandatory destination occupies a guaranteed slot. The optional budget is then selected from remaining candidates; it cannot evict the required site. If a thematic event would normally weight a remote location but its path is invalid, remove it before selection and continue with the valid pool. If a character site is already selected as mandatory, it consumes one slot once, while preserving both selection reasons.

If the only valid candidate is the mandatory destination, offer that location and report the smaller expedition choice set honestly. Do not pad the map with ineligible nodes to preserve a target count. If there are no mandatory targets, use the normal pool and allow a different result on a later seed. A secret site can be included only if the discovery contract allows the player to encounter it without seeing its exact marker in advance.

### Test matrix for selection behavior

The implementation package should independently exercise:

- mandatory exact location available and unavailable;
- mandatory equivalent location explicitly permitted and disallowed;
- a quest with clue-led discovery and map fog preserved;
- a delayed requirement with no candidate;
- two quests requesting the same canonical site;
- a site qualifying for mandatory, faction, recent, and thematic reasons;
- hard exclusion versus soft novelty avoidance;
- all candidates excluded with at least one active quest;
- optional pool empty, secret pool empty, temporary definition expired;
- duplicate IDs and missing references rejected by content validation;
- deterministic output under identical seed and facts;
- stable output after panel redraw/reopen;
- changed owner facts causing one refresh before dispatch;
- a stale preview rejected by the existing ExpeditionSystem start path.

Keep selection tests separate from NarrativeEncounterSystem tests. The former decide which canonical expeditions are offered; the latter choose encounter content within a selected expedition or micro-location. Passing one selector's tests must not be counted as coverage for the other.

## Continuation pass 6 acceptance

The selection package is ready when mandatory requirements are resolved before optional ranking, exact/equivalent/clue/delay semantics come from quest content, candidate ordering and random draws are replayable, visibility remains map-owned, empty pools preserve active quests, and the existing expedition preview/start path remains the final dispatch authority.

## Continuation pass 7 — pool contracts, dispatch scenarios, and player map language

### Candidate pool contracts

Keep selection pools as categories of eligible definitions, not separately persisted arrays. Each candidate has one canonical location identity and may carry several inclusion reasons. The current owner supplies the definition and state; the opportunity calculation supplies a temporary disposition for this dispatch.

| Pool label | Inclusion source | Dispatch role | Visibility rule |
|---|---|---|---|
| Permanent | Existing authored ExpeditionDefinition or canonical map node. | Baseline exploration and repeat visits where supported. | Current map knowledge/status. |
| Quest-required | Active quest owner's location/evidence requirement. | Mandatory if exact or authored equivalent; otherwise clue/delay. | May remain hidden until discovery fact exists. |
| Critical progression | Story/campaign owner marks a non-optional destination requirement. | Must have a reviewed route guarantee. | Reveal only what the current narrative has taught. |
| Character/faction | Current relationship/faction owner plus authored location relation. | Contextual opportunity within remaining budget. | Access and marker use current owner facts. |
| Recent discovery | Map owner reports newly known site or clue. | Prefer timely follow-up without forcing every new lead immediately. | Exact, regional, rumor, or hidden as map owner directs. |
| Thematic | Authored expedition theme matches current biome, hazard, or campaign chapter. | Fills optional slots after mandatory constraints. | Never reveals more than the supporting clue. |
| Optional exploration | Valid permanent candidates without an active requirement. | Broadens choice and replay variety. | Exact marker only if known. |
| Secret | Authored secret eligibility and discovery rules permit an encounter. | Rare optional discovery, not a hidden required target. | Suppress precise marker until the discovery owner confirms it. |
| Temporary | Current temporary/event content says it is valid for this campaign interval. | Short-lived optional or explicitly required opportunity. | Show expiry/availability only through a truthful current status. |

A definition that belongs to two pools is selected once and keeps both reasons for explanation and QA. Pool labels do not change ownership of route locks, fog, actor presence, quest progress, or temporary eligibility.

### Dispatch scenario book

Use a small scenario book to validate the selector's behavior before adding weights or rarity tuning:

**Two active quests, one shared site.** Both quests request the same canonical site for different evidence. The opportunity set includes one destination with both requirement references. On return, each quest owner evaluates its own evidence; visiting the site does not automatically satisfy both.

**One active quest, several optional sites.** The mandatory target is preserved first. The remaining opportunities come from eligible character, recent, thematic, optional, and rare candidates. A new seed may vary optional choices but never removes the mandatory target.

**Known target, blocked route.** The map still shows a known but unavailable target with an accurate route condition. The quest can choose an authored equivalent only if evidence semantics match; otherwise it remains active and delayed.

**Unknown target, known rumor.** The map shows a clue or regional marker. The selector does not expose exact coordinates. After the clue interaction produces a discovery fact, a later opportunity-set refresh may show the exact site.

**Temporary site expires after preview.** The existing expedition preview/start validation rejects the stale target. The planner refreshes and communicates that availability changed; it does not start with the expired definition.

**All optional pools empty.** Show only mandatory eligible targets or a truthful empty-state message. No fabricated nodes are added to reach a visual quota.

**No valid mandatory route.** Preserve quest progress, display a real delay/blocked explanation, and surface a content diagnostic. This is a bug if critical progression claims a guarantee; it is not solved by random substitution.

### Candidate resolution details

For each active quest requirement, resolve a stable request record with:

- requesting quest/instance ID;
- canonical target ID, if already known;
- required evidence kind;
- exact-only or substitution-allowed policy;
- equivalent target IDs, authored by the quest/content owner;
- discovery level required for map presentation;
- actor presence requirement or alternate evidence source;
- delay condition and player-facing explanation;
- whether this consumes an expedition slot or is a return-only interaction.

The selector should not accept a free-form location name from dialogue. It validates stable IDs against current definitions. If a requirement is malformed or references a removed target, data validation reports the defect; runtime fallback uses a declared old-save replacement or a safe delayed state.

### Optional selection and predictability

After mandatory resolution, apply optional selection with explicit diversity goals instead of an opaque score soup. Useful soft preferences include avoiding an identical theme on consecutive expeditions, keeping one recently discovered lead visible, limiting repeated visits when alternatives exist, and allowing one rare opportunity when the pool supports it. These preferences must never invalidate a quest requirement or locked/discovered rule.

Selection should be deterministic for the same seed and same ordered candidate facts. When the campaign advances or new owner facts appear, a later dispatch may differ through the approved random stream. Do not reroll because the user navigated back, switched focus, resized a panel, or reopened the planning screen. A reroll affordance, if the game ever supports it, is a distinct gameplay command with explicit cost and deterministic state update; it is not a display refresh.

Avoid exact percentages until a focused balance simulation and content pool census justify them. A probability without a count of eligible candidates is misleading: a 10% rare weight can mean zero sites in one region or one site in another. Record pool size, inclusion reason, and result in test fixtures before proposing numerical tuning.

### Map presentation language and player feedback

The expedition/map interface should distinguish four questions in concise language: Is the place known? Can the player reach it now? Is it required for an active task? What will change if the expedition succeeds? It should not expose implementation classifications such as “candidate pool 2” or “weight 0.35”.

Suggested truthful labels:

- “Known route” when map knowledge and route readiness are confirmed.
- “Lead only” when the player has a clue but not an exact map destination.
- “Route blocked” when the destination is known and a current owner supplies the blocking condition.
- “Quest evidence site” when an active quest requests the site and visibility rules allow that explanation.
- “Opportunity unavailable” with a short reason for a known temporary/locked target.
- “No site selected” when no candidate is legal and no expedition should start.

For required hidden content, phrase the quest journal around the player's knowledge: “Find a record that confirms the destination,” rather than “Travel to East Depot” before the map owner has discovered it. On route failure, the UI preserves the quest objective and describes the next valid clue or wait condition.

### Selection result model for review

An opportunity-set entry needs only enough information to render and revalidate the current choice: canonical definition ID; eligible/required/substitute/clue/delay disposition; inclusion reason IDs; map visibility supplied by the map owner; relevant source revision markers; and the expedition preview token/contract already supported by the current path. Do not persist optional weights, rejected candidate text, or all map fog state in this derived result.

The reviewer should be able to answer why each displayed location is present and why a plausible alternative is absent. Diagnostics should be stable under catalog reorder. When two requests coalesce on one location, retain both requirement IDs but pass one target through the current ExpeditionSystem dispatch seam.

### Content expansion and release classification

Core pool changes include a new required destination or new rule that alters what base-game dispatch offers. Those need integration evidence and save/replay review. Expansion pool changes may add optional permanent sites, temporary event sites, clues, and character/faction locations, provided the base route remains complete when the bundle is disabled. Secret locations stay optional unless the main campaign explicitly teaches their discovery route and provides a fallback.

Do not create a new location for every quest objective. Reuse a canonical site when the location purpose and owner state already support it; add a distinct scene or evidence interaction if that is what the quest needs. A location definition is not a generic container for arbitrary dialogue: it must be reachable through current map/expedition consumers.

## Continuation pass 7 acceptance

The pool and presentation package is ready when all category labels resolve to canonical definitions and owner facts, dispatch scenarios prove mandatory guarantees and optional variety independently, no-valid-route feedback keeps the quest understandable, and every displayed marker reflects map knowledge rather than selector intent.

### Conflicting requirements and slot pressure

When two active tasks request incompatible exact destinations, preserve both task records and report the constraint to their quest owners. The selector must not choose one quest as “more important” through an undocumented global score. The content/quest contract should define whether both sites can fit in one expedition, whether the tasks may be dispatched separately, whether one can be delayed, or whether an authored equivalent satisfies one request.

If the opportunity budget has fewer slots than mandatory requirements, the budget is wrong or the quest content lacks a dispatch grouping rule. Do not silently drop the last requirement, merge two objectives with different evidence, or force an unsafe route. A valid result may present a larger choice set, a multi-leg expedition only if the current system supports it, or a clear delay for a quest whose owner permits waiting.

The decision should be deterministic and visible in diagnostics: list each requesting quest, target, evidence kind, and allowed fallback. Player copy can remain concise (“Two active tasks need separate routes”) while the journal preserves which task is waiting and why.

### Route changes during active planning

If a route owner reports a change after the player inspects a candidate but before dispatch, retain the player's selection context long enough to explain the change, then rebuild the legal set. Do not silently replace the destination. If the route becomes legal again later, the same canonical target can return according to current eligibility; a prior failed preview does not itself mark it visited, discovered, or exhausted.

## Continuation pass 8 — expedition planning UX, slot lifecycle, and route assurance

### Player flow from expedition entry to committed dispatch

Make the selection process understandable as a sequence of player decisions:

1. **Open planning:** show the current expedition context and active requirements using the existing panel/host path.
2. **Review opportunities:** present only candidates legal under current owner facts. Show exact markers only for known locations; show clue cards or regional hints for incomplete knowledge.
3. **Inspect a candidate:** disclose why it is relevant, route readiness, known risks, and any active quest requirement. An optional discovery is labeled optional.
4. **Choose a target:** record the player's intended canonical location in the current planning view. Do not yet mutate quest or map state.
5. **Preview dispatch:** ask the existing expedition owner to validate target, route, roster, and other current start conditions.
6. **Confirm or revise:** if validation succeeds, make the target and relevant consequence clear; if it fails, explain the changed condition and return to the refreshed opportunity set.
7. **Commit start:** call the current ExecuteStart path. A successful start consumes the existing target/dispatch state; a failure leaves the player in planning with a truthful message.

The UI should not show a selected target as “visited,” “discovered,” or “quest complete.” Those states have different owners and happen at different points. A selected candidate is a plan. A committed expedition is a start result. A visited location requires the actual travel/arrival behavior.

### Slot lifecycle and relevance

Selection budgets are per dispatch, not per campaign. Define a default opportunity budget only after observing the current expedition UI and content volume. Hard requirements can exceed the optional target count only through an explicit authored policy; if there are too many simultaneous mandatory tasks, use the conflict resolution from pass 7.

An opportunity can be:

- introduced at this dispatch because a clue was just discovered;
- retained because an active quest still requires it;
- omitted because it is optional and the dispatch budget is full;
- suppressed because its definition or route is invalid;
- repeated because a task requires a return visit;
- retired from new selection after an owner-confirmed completion.

Do not implement an arbitrary global cooldown based solely on “recently selected.” A location can be selected but never reached, and a quest may need a repeat. Novelty reduction should consider actual owner-backed visit/completion facts when available.

If a mandatory requirement remains active over several dispatches, its canonical ID and fallback contract remain stable. The optional opportunity set may change, but no refresh may silently forget the required target. If the quest is abandoned, failed, completed, or resolved, the quest owner removes or transforms its request; the selector does not infer that change from a dialogue scene.

### Map visibility state and content kind

Separate location kind from its current map presentation:

| Content kind | Possible map presentation | Selection behavior |
|---|---|---|
| Permanent known site | Exact marker, route/lock badge, visited/completed status. | Eligible if route and expedition definition allow it. |
| Permanent undiscovered site | Hidden or absent. | May be internally relevant to an authored clue but cannot be directly pinned. |
| Quest-only site | Exact only after sufficient discovery; otherwise clue/region. | Required request uses explicit exact/equivalent/clue/delay policy. |
| Temporary site | Exact or rumor while valid, then unavailable/expired if known. | Never eligible after the owner-defined window closes. |
| Secret site | Hidden until the discovery owner reveals enough information. | Optional unless critical progression has a separate accessible fallback. |
| Character/faction context site | Visible according to current relationship/faction and map facts. | Optional lead or required only by an authored, supported quest. |
| Encounter micro-location | May be selected inside a chosen expedition by the separate encounter system. | Does not become a selectable expedition map node merely because it appears in an encounter. |

The same location can be permanent and quest-required; content kind stays permanent while the current selection reason includes the quest requirement. Avoid changing its canonical class each time an active quest uses it.

### Route-assurance contract

Before a quest requiring expedition travel is considered release-ready, simulate its location request against:

- correct region and required expedition type;
- exact location currently known and reachable;
- exact location known but route blocked;
- location not yet discovered, but a valid clue exists;
- location unavailable in an expansion-disabled build;
- temporary location outside its availability window;
- actor required for objective absent at planning or arrival;
- multiple active tasks sharing or competing for the same site;
- player changes a world state after preview;
- saved campaign has a retired or replaced definition.

For every case, state the expected disposition and copy. “No valid location” must not be represented by an empty black map with no explanation when the player has an accepted mandatory task. If the task is intentionally waiting for a clue, the clue route itself must be available through another supported interaction.

### Planning feedback and accessibility

Map symbols need redundant cues: icon/shape plus text label; color may reinforce but cannot carry the only meaning. A locked location should communicate whether it is undiscovered, route-blocked, temporarily unavailable, or quest-ineligible when those distinctions are meant to be known. Screen-reader and controller focus should announce the same reason shown visually.

Keep the opportunity panel stable while the player moves between candidates. Focus movement must not consume a random draw or rebuild the set. On refresh, preserve the selected target if it remains valid; otherwise move to the nearest valid option in deterministic canonical-ID order and announce the reason. If no candidate remains, focus the explanation and safe back/close commands.

Avoid exposing hidden IDs or spoiler-sensitive titles in hover text, disabled labels, logs visible to players, or accessibility announcements. Use a clue phrase authored for the current knowledge level. Development diagnostics can expose stable IDs in a separate diagnostic view.

### Dispatch acceptance record

For a focused integration, record a dispatch example with the seed, owner facts, mandatory requests, filtered candidates, inclusion reasons, final displayed set, chosen target, preview result, and ExecuteStart result. Repeat the same fixture to prove stable selection and panel reopen behavior. Change exactly one owner fact and show the expected recomputation. Keep the record small enough to compare between revisions and never serialize it as new campaign authority.

This record distinguishes selector correctness from map correctness and expedition-start correctness. If a candidate is legal but the start owner rejects it, investigate the owner contract rather than weakening the selector's hard exclusions.

## Continuation pass 8 acceptance

The planning UX is ready when selecting a candidate cannot masquerade as discovery or arrival, required requests persist until their owner changes them, map visibility is independent of location class, focus/refresh behavior is deterministic and accessible, and dispatch records prove both selection and existing start validation.

### Per-dispatch refresh contract

Rebuild the opportunity set at each genuine expedition-planning entry from current owner facts, but keep one result stable for the duration of that planning session. This gives new discoveries, quest requests, and route changes a chance to matter on the next dispatch without making the map reshuffle whenever the panel is redrawn.

At planning entry, the selector should:

1. Read active quest requirements and critical progression constraints.
2. Read map discovery, route, lock, visit, and completion facts from the map owner.
3. Read current temporary-event/character/faction eligibility from their own owners.
4. Resolve mandatory requests and authored fallbacks.
5. Build the optional set and use deterministic ordering/selection.
6. Expose the current map presentation and dispatch preview.
7. Freeze the displayed set until a relevant owner change or planning exit.

The selected target itself is preserved through preview and confirmation. A fresh planning entry may yield a new optional set from the deterministic stream when campaign context or dispatch sequence has advanced; merely leaving and re-entering without any state change must follow the intended seed contract rather than create a free reroll.

### Fairness measurements before tuning weights

Before proposing numerical rarity or novelty values, collect a bounded content census: count legal candidates by region, expedition type, visibility level, active quest requirement, and optional pool. Then test whether mandatory requests always appear; optional candidates vary across seeds; recently discovered sites receive a fair opportunity without monopolizing the list; and repeated visits decrease only when another legal option exists.

Measure both inclusion frequency and route viability. A location appearing often but failing preview is not a healthy selector result. A rare site that never appears in a small pool may be statistically expected but still unusable for a quest. Quest-required candidates are governed by guarantees and fallback semantics, not frequency targets.

No numerical threshold is accepted from a plan alone. Balancing requires the current seeded simulation/tooling, a representative catalog census, and a focused integration claim. Until then, preserve priority ordering and deterministic behavior while leaving exact optional weights configurable through existing content/selection authorities.

### Location opportunity at each expedition stage

Before planning, a quest may know only a clue; the map can show a rumor and the selector can return clue-led. During planning, an exact known site can be selected if the route and expedition rules allow it. At preview, current actors, route constraints, and required resources are validated by their owners. At dispatch, ExecuteStart is authoritative. During the expedition, encounter micro-locations belong to the separate encounter selector. On return, map/quest owners record visit and evidence. On a later dispatch, the canonical site may be eligible again if its current status and quest requirement allow it.

This stage model prevents one selection result from being reused as discovery, travel, encounter entry, or completion. Each stage has its own fact producer and player feedback. A location can be selected but not started; started but not reached; reached but not investigated; investigated without proving the active objective. The quest's proof contract determines which of these advances progress.

## Continuation pass 9 — opportunity record, requirement ownership, and scale constraints

### Derived opportunity record

An opportunity entry is a planning view over canonical data. It should carry only the information needed to explain and revalidate one choice:

| View field | Meaning | Source |
|---|---|---|
| Canonical target ID | Stable expedition/location reference. | Existing authored definition/catalog. |
| Eligibility disposition | Eligible, required, substitute, clue-led, deferred, or excluded. | Selector result from owner facts. |
| Inclusion reasons | One or more quest, discovery, character, faction, theme, or optional reasons. | Current owners and authored request. |
| Discovery presentation | Exact, regional, rumor, hidden, or unavailable as appropriate. | Map/discovery authority. |
| Route status | Known current readiness/blocker. | Current route/map/expedition owner. |
| Actor/resource constraints | Current preconditions, not a permanent copy. | Roster/inventory/expedition owners. |
| Source markers | Revision/event markers for relevant facts where supported. | Contributing owners. |
| Preview/start result | Current existing preview token or typed failure. | ExpeditionSystem. |

Do not save the whole record as campaign truth. The target state may already be owned and persisted by the expedition system after commitment; before commitment, the opportunity record is derived and disposable. A test diagnostic can retain candidate IDs and rejection reasons, but it should not leak spoiler-sensitive data into player-facing UI.

### Requirement ownership protocol

Quest authors provide a location requirement reference with an evidence purpose and fallback policy. The quest owner decides whether the requirement remains active. The opportunity layer resolves it against canonical site definitions and current conditions. The map owner decides what the player knows. The expedition owner decides whether the selected target can start. On return, the quest owner accepts or rejects the objective evidence.

No layer can shortcut the chain:

- Dialogue cannot declare a location found.
- A generated candidate cannot unlock a map node.
- The map cannot mark a quest objective complete because a site was selected.
- The selector cannot bypass actor/resource/route checks.
- Arrival cannot satisfy an investigation objective without its required interaction.

If a quest requirement is malformed, the content validator reports the request ID and source. If the owner reports a legitimate delay, preserve the accepted task. If an implementation seam is absent, mark it as unbound and do not make it a critical progression dependency.

### Location availability classes

Use a stable authored availability rule plus current owner disposition:

- **Always eligible by definition:** valid whenever normal expedition requirements pass.
- **Unlocked by progression:** available after an owner-backed campaign/quest result.
- **Discovered by clue:** hidden until discovery fact, then eligible subject to route validation.
- **Faction contextual:** eligible only when the current faction owner exposes access or a supported escort/permission.
- **Temporary window:** eligible while the owning event/time condition is active.
- **Quest-only:** included for a specific request and otherwise omitted or undisclosed.
- **Repeat-visit eligible:** can return for a distinct objective or revisit after the owner confirms new relevance.
- **Retired/old-save only:** no new selection, but stable reference remains resolvable for restore.

Availability classes are content definitions; they are not a second mutable map status. Current discovery, lock, route, visit, and completion remain with their existing owners.

### High-volume candidate processing

If a future content wave introduces many locations, evaluate performance from actual profile data before adding indexes or caches. The first acceptable implementation should perform bounded work per planning entry: load/reuse canonical definitions through the current catalog path, evaluate each candidate once against the requested facts, deduplicate by stable ID, and avoid rescanning all dialogue graphs or quest prose for every map redraw.

Any optimization must preserve exact output for the same seed and owner snapshot. A cache may index immutable tags or prevalidated references, but dynamic facts must be re-read or invalidated through known owner events. Do not keep a mutable long-lived candidate pool that becomes another location authority. If a profile shows cost, record candidate count, evaluation time, allocation, and refresh frequency; optimize only the measured stage.

### Location/quest combinations for release QA

Test the selector with content combinations, not only isolated candidates: a main quest plus an optional location clue; two character tasks at one shared site; a faction-specific destination with no access; a hidden site required by an optional quest; a temporary location whose window closes; a return-only shelter task; and a retired location ID referenced by an old save.

For each combination, assert which quest requests remain active, what the player can see, which canonical site enters the opportunity set, what fallback was selected, and whether existing PreviewStart/ExecuteStart accepts the dispatch. This catches the subtle bug where a map looks correct but a quest becomes impossible, or a quest remains valid while the panel misleadingly hides every action.

### Dispatch rehearsal: selection, visibility, and commitment as separate outputs

Review the whole decision in one bounded rehearsal rather than judging only the final map. Create a fixture with two accepted quest requirements, one critical progression destination, one faction-contextual site, one recently discovered clue, several legal optional locations, and one rare surprise candidate. Give one mandatory site an invalid route and the other a valid authored substitute. These are fixture roles, not new canonical locations; integration must resolve them through current IDs and owners.

The selector first resolves quest requirements and critical progression. It then reports the invalid route with its reason, asks the responsible owner-approved fallback policy for an equivalent site or clue lead, and retains the original request as unresolved until the quest owner changes it. The valid mandatory destination stays in the dispatch list even when optional capacity is full. The faction location is included only when current faction access permits it; a missing access fact becomes an explicit unavailable reason, not a random exclusion. Recent discovery influences optional presentation only after hard requirements are satisfied.

Review the result in three views:

| View | What it may reveal | What it must not imply |
|---|---|---|
| Opportunity list | Known destinations, supported rumor leads, route state, and inclusion reasons. | Arrival, objective completion, or an unapproved reservation. |
| Map | Exact or approximate marker permitted by discovery authority. | Exact hidden location from an undiscovered quest requirement. |
| Preview/confirmation | Current route, actor, resource, and expedition readiness. | Guaranteed survival, encounter outcome, or objective proof. |

Selecting a row changes only panel focus. It cannot draw a new optional set, consume a rarity roll, or alter map knowledge. Preview can reject a candidate if owner facts changed; refresh the affected reason and offer a supported alternative while retaining mandatory requests. ExecuteStart remains authoritative. If it rejects after preview, do not silently substitute a new destination after the player committed. Return control with a clear re-preview or cancel path and preserve existing resource-consumption rules.

### Stable selection under refresh and restart

Define the selector input boundary as a stable dispatch snapshot: expedition origin/type, active requirement IDs, canonical candidate definitions, relevant owner facts, and the existing deterministic stream position or seed contract. A redraw over the same snapshot returns the same ordered opportunities and fallback decision. A meaningful owner change can invalidate the snapshot; the panel should say dispatch information changed and recompute from current facts. Moving focus, opening help, changing sort order, or closing and reopening the screen cannot count as a gameplay reroll.

For a replay fixture, capture ordered input IDs and resulting selected IDs with exclusion/fallback reason codes. Run the same snapshot twice and compare output. Then change exactly one input fact: remove faction access, close a route, reveal a clue, complete one quest, or advance the owning temporary window. Each output difference must be explained by that changed fact. Keep this in a focused verification artifact; do not add a persistent dispatch-history store solely for comparison.

If candidates exceed a soft UI capacity, preserve required and critical entries and let the current interface scroll or group optional choices. If that presentation is unsupported, stop dispatch with a named capacity limitation before resource commitment. Do not truncate the tail of a sorted list: that would hide some optional entries systematically and could conceal a required location after a catalog change. If the hard-required set itself is too large, surface the conflict to its content/quest owner before dispatch; never pick one requirement silently.

### Selection review metrics and author checklist

For each representative pool, measure the facts that reveal content problems: eligible candidate count, exclusions by reason, mandatory inclusion rate, fallback rate, optional appearance frequency over bounded seeds, and preview/start acceptance. Report zero-candidate contexts separately from ordinary low frequency. A rare location appearing once in a small run says little about balance; a mandatory location missing from a valid context is a correctness failure.

Each authored location row should answer: which pool can request it; what makes it legal; what the player knows; what route class it supports; whether it is one-time or revisit-eligible; which evidence can be produced there; what conflicts exclude it; and what approved fallback applies. A row with no objective capability cannot be required just because its title sounds suitable. A location that can be seen but cannot be travelled to needs an explicit clue or map-only classification.

Review diversity by player activity, not only biome labels. Several sites that all request the same inspection, provide identical evidence, and lead to the same conversation are one content function despite different scenery. Reuse their mechanics, but count them as distinct optional variety only when they contribute different route choices, evidence, hazards, character access, or consequences. Keep the selector's data model compact: pool tags and authored availability are enough unless current source evidence proves another field is required.

### Dispatch closeout record

End the dispatch record with the chosen canonical target, current visibility level, inclusion reason, resolved substitute, preview outcome, and final ExecuteStart result. List mandatory requests that remain pending and why, so reviewers can confirm the expedition did not imply that those objectives were satisfied. Report optional-selection variety separately from hard-requirement success. Any proposal to tune rarity or anti-repeat behavior must include catalog counts and bounded seeded observations; do not promote a numeric rate based on a tiny hand-picked example. Close the tranche only after player-facing feedback and the owner-level selection record describe the same result.

## Continuation pass 10 — temporal availability, risk disclosure, and cross-dispatch continuity

### Temporary locations have an owner-defined time window

A temporary pool row is eligible only while the owner of its opening/closing condition reports it as active. The selector must not calculate its own clock, infer time from expedition count, or keep a local “seen until tomorrow” flag. Content describes the window in player-facing terms; the time/event owner determines exact validity. If the relevant owner is not ready when the player opens dispatch planning, use its existing load/readiness path or show a neutral pending state rather than treating unknown as expired.

Review the full temporary-site lifecycle:

| Moment | Selector behavior | Player-facing state | Quest rule |
|---|---|---|---|
| Before opening condition | Site excluded from ordinary selection. | Hidden unless an approved foreshadowing clue exists. | No accepted quest may require the closed site without a fallback. |
| Opening event accepted | Add as an eligible candidate after re-reading owner facts. | Exact marker or rumor according to discovery state. | Existing request becomes possible; no automatic acceptance. |
| Window active | Include only after route, actor, and conflict checks. | Show remaining context when known and useful. | Active objective remains pending until its proof is accepted. |
| Window closes before dispatch | Exclude as a current destination. | Show unavailable/expired wording only if the player knows of it. | Owner selects delay, substitute, clue, or explicit quest expiry. |
| Window closes after preview | Invalidate stale preview and re-evaluate before commitment. | Explain that availability changed; preserve focus when possible. | Do not silently mark the objective expired or complete. |
| Old save references site | Resolve the canonical old ID or migration. | Show truthful retired/closed-site description. | Preserve accepted-instance continuity through its owner. |

This sequence handles an important difference between a temporary opportunity and a timed quest. The site may close without the quest failing if the quest owner allows a substitute. The quest may expire while a physical site remains available for ordinary exploration. The selector reports the current location opportunity; the quest and time owners decide objective/deadline meaning.

### Candidate stability across an expedition lifecycle

Give each planning view a bounded lifecycle: create a selection snapshot at a valid planning boundary, display its candidate set, preserve focus during ordinary UI navigation, request a fresh preview when relevant facts change, and hand one confirmed target to the existing expedition start route. The selection snapshot is not durable campaign truth. The confirmed expedition, once accepted by the current owner, is the durable operation.

| Lifecycle action | May rebuild optional candidates? | Must preserve |
|---|---:|---|
| Open panel with unchanged dispatch snapshot | No free reroll. | Same ordered candidate IDs and reasons. |
| Move focus or inspect location details | No. | Current opportunity ordering and quest pins. |
| Change sort/filter preference | Presentation only. | Eligibility, random stream, and canonical target set. |
| Quest owner changes active request | Yes, at a stable refresh boundary. | Other still-active required requests. |
| Temporary window or faction access changes | Yes after reading the new owner fact. | Any confirmed expedition commitment. |
| Preview rejects stale route/readiness | Revalidate current inputs. | No substituted destination after confirmation. |
| Cancel before ExecuteStart accepts | Return to planning under current owner rules. | No false arrival or completed objective. |
| Return from expedition | Re-read canonical visit/discovery/objective results. | Evidence already accepted; no repeat mutation. |

If an optional set is intended to vary by dispatch, variation occurs at the existing owner-defined new-dispatch boundary and uses its deterministic input. UI activity is not that boundary. A repeated preview for the same selected destination should remain stable; the user should not be able to cycle through rare locations by backing out of a panel unless the game's existing operation explicitly defines a new dispatch attempt.

### Risk and uncertainty disclosure

Location presentation should expose known operational constraints without leaking undiscovered content or promising safety. Separate **known hazard**, **reported hazard**, **unverified rumor**, and **unknown condition** in the authored wording. A weather or enemy tag can influence candidate compatibility only through its verified owner input. Generated flavor can make the same known risk sound different; it cannot upgrade an uncertain report to a fact.

| Knowledge quality | Map/dispatch presentation | What the player can reasonably infer |
|---|---|---|
| Owner-confirmed current hazard | State the hazard and its known scope. | The named risk is present in the inspected/current segment. |
| Credible but unverified report | Attribute it to a source and preserve uncertainty. | The report may justify preparation, not certainty. |
| Old or partial route observation | Name the checked segment/age if available. | Unseen route beyond that point remains unknown. |
| No evidence | Use a general uncertainty marker or no claim. | Absence of a report is not proof of safety. |
| Required clue destination | Show the clue/region at the player's earned knowledge level. | The hidden target's exact coordinate is not disclosed prematurely. |

The route preview should distinguish “not legal to start,” “legal but with a known cost,” and “legal with unresolved risk.” These can lead to different player decisions. A warning cannot substitute for a command owner rejection or for hazard simulation. Conversely, a hidden risk discovered during play should not be described in pre-dispatch text as though the game had verified it earlier.

### Adversarial selection fixtures

The selector review should include cases that try to break guarantees rather than only demonstrate normal variety:

| Fixture | Pressure applied | Required behavior |
|---|---|---|
| Every optional candidate excluded | Close routes or fail optional gates. | Required tasks remain listed; empty optional fill is valid and explained. |
| More hard requirements than soft slots | Stack active mandatory requests. | Expand presentation or return a named conflict before spending resources. |
| Equivalent targets conflict | Two tasks offer different substitutes. | Let each quest owner resolve its request; never apply an undocumented global score. |
| Fallback chain loops | Equivalent location A points to B and B back to A. | Validator rejects the cycle or resolves through an explicit terminal clue/delay. |
| Site expires between preview and start | Change its owning window after preview. | ExecuteStart rejects stale request; no silent substitution or completion. |
| A required site is secret | Quest fact exists but knowledge is insufficient. | Pin earned clue/region and preserve critical possibility without coordinate leak. |
| Old accepted quest uses retired ID | Remove it from new-generation pool. | Restore path retains or migrates its reference. |
| Revisit-only site has no new evidence | Keep the location legal but objective already settled. | Ordinary visit can occur, but no quest progress is invented. |

Record which layer detected each invalid case: catalog validator, quest requirement producer, selector, map disclosure owner, expedition preview/start, or return evidence consumer. “Selector failed” is too broad to locate the repair. A missing location ID is a content validation error; a known route that becomes blocked after preview is a stale-state revalidation case; a clue that fails to reveal must be fixed at the discovery/map owner seam.

### Distinguish exploration availability from quest assurance

The location selector has two legitimate jobs that should be reviewed separately. Exploration availability provides optional variety and supports ordinary travel. Quest assurance preserves active objective possibility. A site can be exploration-available without being required by any task; a quest request can be assured through a clue or equivalent even when its exact physical target cannot appear. Report these outputs separately in review data and UI language.

This separation prevents a common design trap: turning every interesting location into a mandatory expedition pin. Too many pins undermine player choice and overload map presentation. Keep only the objective's necessary destination/evidence request as hard. Additional lore rooms, alternate witnesses, and reward sites can remain optional if the base objective route is complete without them. Conversely, do not label the only path to a required proof as “optional” merely because the site is usually chosen from a pool.

For campaign continuity, each active task should declare whether its request is exact-site required, evidence-kind required, or informational-only. Exact-site requirements need a stable location and hard inclusion/fallback. Evidence-kind requirements can accept any authored producer that the quest owner recognizes. Informational hints can enrich the player's map without controlling task completion. This classification keeps the selection system maintainable as authored location volume grows.

### Multiple quest requests at one physical destination

When two active requests point to the same canonical site, combine the location entry while preserving each request binding. The candidate is deduplicated by location ID; its inclusion reasons may name several quests, but the selector must not merge objective evidence or report all requests complete on arrival. One trip can make several objectives possible without automatically satisfying any of them.

| Shared-site situation | Dispatch presentation | Evidence on return |
|---|---|---|
| Two quests require arrival only | One marker with both task associations where disclosure allows. | Each quest owner accepts its own arrival fact. |
| One quest needs inspection and another needs a conversation | Explain the location supports both interactions. | Inspection and conversation each emit their own evidence. |
| Faction task is confidential | Show only the marker information the player has earned. | Faction owner validates its own task result. |
| One quest can use the site as a substitute | Name the primary required request and record the substitute relation. | Substitute counts only after the requesting quest owner accepts it. |
| One request is no longer active | Remove that binding after its owner changes it. | Retain evidence already accepted by other owners. |

Fallback belongs to the request, not the physical location as a whole. One quest may accept a clue from a nearby site while another still requires the original destination. If the shared site becomes unavailable, resolve each binding independently and display the resulting plan. A single candidate-level fallback string can conceal that difference and may direct players to a location that satisfies none of the active objectives.

### Information layering for a shared marker

Where several quests refer to one location, compose map labels from what the player knows and what the selector is allowed to disclose. A public travel marker can be shared even when one associated quest is secret. Do not expose a hidden task title in the marker's tooltip, dispatch reason, or accessibility label. Conversely, do not obscure an ordinary known destination merely because an optional confidential clue also points there.

The selector's structured output can retain canonical requirement IDs for owner routing, while the presentation layer chooses an allowed summary for the player. This is not permission to store a second hidden-state copy in the panel. On refresh, query current knowledge and active quest owners. If the presentation layer cannot safely combine the reasons, show the public reason and keep the undisclosed binding internal to the current owner/host route.

## Continuation pass 11 — location content contracts, compatibility constraints, and map review kit

### Canonical location card and selector-facing fields

Every selectable location needs one canonical definition. The selector may consume a small set of metadata, while the map, expedition, encounter, and quest systems keep their own authoritative facts. The card below names content concerns for review; it does not require duplicating every value in a new pool file.

| Card section | Authoring requirement | Authority question |
|---|---|---|
| Stable identity | Canonical ID, revision/deprecation note, and bundle. | Which current catalog owns resolution and old-save references? |
| Arrival framing | Name, region, concise map summary, arrival paragraph, accessibility label. | Which localization and map presentation route renders it? |
| Pool eligibility | Permanent/quest/character/faction/discovery/thematic/optional/secret/temporary role. | Are pool tags metadata over this identity or a duplicate registry? |
| Knowledge disclosure | Exact, regional, clue, rumor, or hidden entry requirements. | Which discovery/map owner proves the player knows each detail? |
| Route and expedition | Origin/range compatibility and any current start requirements. | Which map/route/expedition owner validates reachability? |
| Playable affordance | Interaction/evidence types actually available at the site. | Which existing system can produce each proof or action? |
| Conflicts | Mutually exclusive route/encounter constraints, if supported. | Is there an existing host/expedition representation? |
| Repeat visit | What changes, what remains fixed, what can be revisited. | Which owner records visit or accepted outcome? |
| Fallback | Equivalent site, clue lead, delay, ordinary unavailability, or no fallback. | Which quest owner accepts the substitute or delay? |
| Retention | Whether a retired site must remain resolvable for old saves. | Which save/catalog migration path retains the ID? |

A location with no gameplay interaction can still be a valid map destination, atmosphere node, or lore stop, but it cannot be cited as an objective producer until an owner-backed action exists. A clue-only region should not be represented as an exact physical location in dispatch data. If there are multiple arrival variants, they share the same canonical identity and route/evidence contract.

### Minimum compatibility grammar

Content authors need a small, readable way to declare relationships among locations. Prefer existing expedition and route constraints. If current content has no expression for a needed relationship, record the gap instead of inventing an executable rule language.

| Relationship | Purpose | Safe boundary |
|---|---|---|
| Required-by request | Keep an active quest destination possible. | Request originates from quest owner and is revalidated. |
| Evidence alternative | Let a quest accept multiple authored sources. | Quest owner declares equivalence; selector never decides proof semantics. |
| Mutually exclusive | Avoid presenting incompatible locations/encounters together. | Use only a verified exclusion mechanism; no hidden global score. |
| Same-site dedupe | Merge pool membership and request presentation for one canonical ID. | Preserve individual quest bindings and evidence requirements. |
| Route prerequisite | Check path/actor/resource readiness. | Delegate to current route/expedition owner. |
| Temporary validity | Include only during an owner-defined time/event window. | Selector reads; it does not own time or event status. |
| Optional thematic fit | Prefer relevance to expedition intent. | Never displace mandatory/critical candidates. |
| No relation | Ordinary candidate with no quest dependency. | Omit extra tags rather than adding a meaningless “neutral” predicate. |

Do not let content authors create circular `requires` chains. If location A can only appear with B and B only with A, the data needs a single explicit group rule or an error; it should not rely on row ordering. A fallback reference also needs a terminal: an equivalent location, a clue route, an allowed delay, or a clear impossibility before commitment. Self-reference and fallback cycles are validation errors.

### Selection resolution across overlapping pools

One location can be permanent, recently discovered, faction-specific, and quest-required at once. Pool membership contributes reasons; it does not multiply the site's chance to appear. Deduplicate by canonical ID before optional weighting, retain all valid reason references internally, then choose one player-facing presentation based on disclosure. This prevents a location from winning the lottery four times because four content authors independently tagged it.

| Mixed memberships | Inclusion rule | Presentation policy |
|---|---|---|
| Quest-required + secret | Satisfy the active request or resolve its fallback. | Show only the clue/region the player knows. |
| Permanent + faction-contextual | Ordinary inclusion remains possible if it is truly public. | Do not imply faction access unless the owner confirms it. |
| Recently discovered + temporary | Verify both current discovery and owner time window. | A known marker can show as closed after expiry when appropriate. |
| Character + optional + rare | Apply the character/optional gate, then rarity among eligible candidates. | Do not make the rare choice the only way to contact the character. |
| Several quests + same ID | One physical entry with several request bindings. | Explain multiple uses without merging their objectives. |

If one membership is invalid, retain other valid membership only if the canonical location can still be reached and the surviving reason is sufficient. For example, a faction restriction can fail while the site remains a normal public place; an impossible mandatory quest request cannot be hidden by its optional tag. Report rejected memberships separately so the content owner can correct the data.

### Map and dispatch information hierarchy

Present three levels of information consistently: **where** the opportunity is, **why** it is relevant, and **what is known about going there**. The map marker can be exact, regional, clue-led, or absent. The reason can be a quest, discovery, faction, character, or ordinary exploration. The route state can be ready, blocked, unknown, expired, or invalid. A screen must not compress these into one icon state if doing so would imply certainty the owners do not provide.

| Player question | Minimum answer in dispatch planning |
|---|---|
| “Can I tell where it is?” | Exact/region/clue/hidden marker at current disclosure level. |
| “Why is it on my list?” | A safe inclusion reason, or a neutral description for hidden content. |
| “Can I travel there now?” | Current preview/start readiness from its owner, including named blocker. |
| “Will this finish a task?” | State whether arrival enables an objective; name interaction/proof still required. |
| “What happens if unavailable?” | Current fallback or explicit no-commit/delay outcome. |
| “Can I choose another location?” | Distinguish an optional alternative from a required request that needs resolution. |

When the player confirms a destination, preserve the chosen canonical target through preview and start. If owner facts change and invalidate it, explain the new blocker before dispatch. Never silently reroute after confirmation, because the player may have selected an actor, equipment, or risk plan for the original destination.

### Discovery depth and spoiler boundaries

Discovery can reveal different levels: a region exists, a landmark is likely there, an exact site is known, an entrance is found, or a route is currently open. The project may use a smaller current enum; these are conceptual questions, not a required new state machine. Ask which owner fact supports each disclosure and which interaction will reveal the next layer. A map clue should be actionable enough to reward exploration but need not expose an exact route before the player has earned it.

Do not make visibility monotonic if the current game can close a route or change a temporary site. The player may know a site existed while it becomes closed or dangerous; retain historical knowledge and refresh current availability separately. A rumor that was later disproven can remain part of the story record, but it must not remain a valid exact travel instruction. The map should distinguish “known location, unavailable now” from “location no longer known.”

### Location opportunity lifecycle and UI state

The opportunity card has a presentation lifecycle that must not become a gameplay state authority: discovered/listed, highlighted, details open, preview in progress, start accepted/rejected, and returned/reconciled. Focus and detail expansion do not alter the candidate set. A keyboard/controller user can inspect the reason and blocker, choose another valid row, or back out safely. After a stale-state rejection, focus returns to the explanation or a deterministic next valid option and the update is announced.

Disposal clears transient panel selections and callbacks. Reopening queries the current map, quest, faction, time/event, and expedition owners through their existing host route. If no current fact has changed, the same dispatch snapshot contract applies. If a new expedition begins or a relevant owner event occurs, rebuild at the appropriate boundary. The screen does not persist a private “visited this card” or “used this location” flag to achieve the effect.

### Content-volume and map-density review

Count legal canonical locations by region, route type, knowledge level, interaction capability, quest role, and repeated-use policy. Compare not only how many sites exist but how many distinct player activities they enable. Ten points that all resolve to “inspect a note” can create the appearance of variety without changing the expedition loop. One route site with several well-authored interactions may be more valuable than a larger set of cosmetic duplicates.

Keep optional content from crowding required opportunities. Measure how many candidates appear, how many are actionable, how many are visible at each disclosure tier, and how often dispatch has no optional fill. A low count in one region might be a content gap; a consistent optional surplus may be a UI scanning burden. Exact weights remain a balance question for representative seeded simulation and catalog data, not a value to set in this planning pass.

Before adding an index or cache, profile catalog size, candidate evaluations, route queries, panel refresh count, allocations, and location description loads. The first selector pass should work from IDs and compact immutable metadata, loading full text only for displayed entries. Optimize measured hotspots while keeping identical results for the same input snapshot. Never create a long-lived location cache that becomes an untracked copy of discovery, availability, or route state.

### Integration package for the location slice

The smallest valuable slice uses several existing location IDs across distinct pool memberships, one accepted quest requirement, one hidden/clue marker, one optional candidate, one unavailable-route fallback, and an ordinary location with no quest binding. The handoff reports the exact current selectors/owners used, map visibility for each, preview and start outcomes, owner evidence on return, old-save behavior, and the result of repeating the same deterministic snapshot.

Only after this slice works should the package add temporary events, faction-specific destinations, rare surprises, broader weights, multi-leg travel, or more detailed discovery depth. Each optional layer adds a data and review cost and must use a current owner seam. This staged order gives players a reliable minimum map while leaving room for meaningful campaign variety.

### Accessible map and dispatch language

Map availability and quest relevance must remain understandable without relying on color alone. Pair marker color with shape, icon, short label, or an inspectable description. Required, clue-led, optional, blocked, and unavailable entries should have distinct wording in the list and a focusable legend. A hidden site can remain undisclosed while its clue marker and task summary remain accessible at the player's earned knowledge level.

The dispatch panel should keep required and critical opportunities visible when optional filters are applied. Filters can narrow optional exploration candidates but cannot remove active quest guarantees or silently change the committed destination. Sorting is a view preference and should not alter deterministic selection. Screen readers and keyboard/controller focus need the same reason text available for map marker and list entry; duplicating important status only in a hover tooltip is insufficient.

At confirmation, summarize the selected target, known route state, selected actor/equipment constraints if the current UI supports them, and any active quest relevance. Clearly distinguish known risks from unknowns and avoid an overconfident success forecast. A safe back/cancel control remains available until the existing ExecuteStart owner accepts. After acceptance, the interface reports the true start result and moves focus to the active expedition/status view through the current host route.

### Map legend and update behavior

Keep the legend small enough for quick scanning and use consistent terms across map, journal, and expedition preview. A marker can be required, hinted, discovered, optional, inaccessible, or newly changed. Avoid separate icon colors for every quest family; the active task list already names the quest type and can provide detailed context. When a marker represents multiple public reasons, the details view can list them; when some reason is protected, the marker uses a safe public summary.

When a route or discovery fact changes while the map is open, announce the update and preserve focus on the currently relevant row if it still exists. If it disappeared because an owner closed its temporary window, explain that state and move focus to an available route or the cancellation control. Do not teleport focus to the first optional candidate. When a mandatory destination receives a fallback, keep a relationship between the original request and substitute visible in task details so players understand why the map changed.

### Review of information certainty

Use the same uncertainty vocabulary in map legend, dispatch card, location prose, and dialogue. “Known” means an owner-backed discovery/source; “reported” attributes the information; “unverified” means the player has not corroborated it; “blocked” names a current route/readiness issue; “unknown” is not a prediction. Content review should search for contradictions such as a map card calling a site open while the route owner says blocked, or dialogue promising an exact location when the map only has a rumor.

The location's appearance can be vivid while its status remains clear. Atmospheric adjectives do not need to encode route readiness. If a site is frightening but currently legal to start, describe the known risk and uncertainty separately from its dispatch action. If it is unavailable, use direct copy rather than relying on ominous prose to teach the player that a button cannot be selected.
## Continuation pass 12 — Tidemark location portfolio and expedition route recipes

This pass adds a provisional location set for the Tidemark Compact portfolio introduced in Plan 17. The location names and descriptions are editorial draft material; they do not claim that those sites already exist in the game world or authorize new map identifiers. Their purpose is to show how a location's authored role, expedition appearance, quest requirements, discovery path, and player-facing map language can be coordinated without confusing a permanent place with a generated expedition instance.

The location set carries one central practical tension: a damaged sluice must be inspected, but several nearby sites can provide partial information about access, maintenance, or previous measurements. The expedition director must guarantee a required route when a quest requires it, then use the remaining destination opportunities for optional sites that match the current theme. The same place can appear in different expeditions only where the current owner permits that lifecycle. Reappearance should not silently reset discovery, resource state, quest proof, or danger.

### Regional identity and authored map grammar

The proposed region is a chain of raised service paths above low ground, interrupted by water channels, old measuring posts, shelter additions, and damaged infrastructure. The high-level map should teach this geography through a few visual relationships: a raised path continues across low ground; a channel separates access points; a shelter or staging area is distinct from an exploration site; and a route report may be less certain than a discovered destination. The map does not need a dense grid of individual rooms to communicate that the area has multiple approaches.

Use consistent descriptions for route certainty:
- **Mapped** means the current map owner has a player-visible route or site record.
- **Reported** attributes a lead to its source and may be incomplete.
- **Found** means an exploration or discovery owner has recognized the location.
- **Required** means an active quest requests the location, but does not automatically reveal secret details.
- **Unavailable** means a current owner says the expedition cannot reach it now.
- **Substitute** explains that a compatible location is fulfilling a quest request under the declared fallback.
- **Unknown** describes missing information, not a negative forecast.

These terms must appear consistently in dispatch, quest journal, map markers, and authored arrival prose. Avoid using “nearby” as a promise about geography unless the map owner can support it. A rumor should not become an exact pinned marker merely because a quest author needs the player to see a clue.

### Location portfolio cards

| Draft place | Authored role and persistence class | Expedition pool/use | Quest suitability | Map/discovery contract |
|---|---|---|---|---|
| **Switchback Sluice** | Permanent authored anchor: the fixed gate, inspection platform, and service alcove. | Mandatory while an eligible active objective requires its unique mechanism; otherwise thematic or optional. | Core inspection, Sena's repair conversation, constrained test encounter. | The site can be requested as required after its source clue; hidden maintenance access requires separate evidence. Its identity persists across expeditions. |
| **Rillstep Intake** | Permanent authored anchor: downstream sample edge and visible pressure marks. | Corroboration site, optional exploration, or compatible substitute only where objective semantics allow. | Alternative evidence for flow direction; not an equivalent substitute for inspecting the actual gate. | A report can reveal the general area; exact sample access is discovered on arrival or through an earned clue. |
| **Mothglass Nursery** | Permanent authored shelter/growing space: a dry-roofed nursery with a sealed service channel. | Shelter, social encounter, crafting or recovery stop if those owners support the function. | Optional resource preparation, character conversation, no mandatory gate proof by default. | Its shelter role must not be confused with a rest mechanic until current shelter ownership confirms that function. |
| **Lintel Camp** | Semi-permanent staging anchor with a stable public identity and expedition-specific occupancy. | Staging or safe return point; can host a meeting when its availability owner says so. | Faction meeting, expedition briefing, recovery dialogue. | The camp persists as a named place; which characters are present is a current world/quest fact and may differ between visits. |
| **Sootstep Marker** | Discoverable survey point, potentially represented by a generated visit under a stable authored definition. | Secret or clue-led pool; rare without discovery, eligible after a supporting clue. | The Second Marker clue; context for old route reports. | Do not expose an exact map target before discovery. If no valid instance can be selected, preserve the clue and delay the site. |
| **High Meridian Perch** | Late-game authored lookout and signal point, package-gated by coalition storyline availability. | Faction-specific or late-game thematic pool; never a core mandatory location while the package is absent. | A Measure Both Crews Can Read, late coalition conversation, no requirement for the base sluice conclusion. | Package-off behavior hides the site cleanly. If discovered in an older save but the package is disabled, map text uses an approved neutral explanation and does not reference missing content. |

The cards are not six requests to create six map scenes. A production slice can use two anchors and one optional clue point. Reuse a common service-path arrival structure where appropriate, but author the unique navigation cue, hazard, proof opportunity, and return behavior for each site. Reusing a scene template must not make the sites mechanically identical or overwrite persistent identity.

### Location definition versus expedition visit

The location definition provides stable authored identity, semantic role, allowed use, data references, and presentation defaults. An expedition visit carries only the current visit-specific facts supported by existing owners: selected target reference, current dispatch context, applicable encounter/arrival setup, and current reported risk. The visit must not clone the definition into mutable world state or become a new source of truth for permanent discovery.

For Switchback Sluice, the authored anchor names the gate and its inspection features. One expedition may select the gate with a clean access route; another may arrive after a hazard report and show a blocked approach; a third can present it as a required objective with a safe alternative route. These variations are generated/selected scenario context, not new sluice identities. If the current world system does not support persistent physical change to the gate, prose must not imply that the player permanently repaired it.

When the Sootstep Marker is represented by a temporary visit, its stable definition and the selected visit must remain distinguishable. The quest points to the evidence meaning or authored request, while the expedition system selects a compatible visit. The resulting clue is accepted by the quest owner only through the supported proof route. The marker is not “found forever” merely because a temporary instance was selected; discovery ownership decides what persists.

### Route recipes and mandatory-slot behavior

These recipes show how to preserve active quest feasibility while maintaining expedition variety. They supplement the priority and exclusion rules in earlier passes; they do not replace the deterministic selection contract.

**Recipe A — main inspection active, corroboration unknown.**
- Guarantee Switchback Sluice as the mandatory quest destination if it is valid under current location availability and expedition constraints.
- Select at most one compatible thematic site from Rillstep Intake, Mothglass Nursery, or Lintel Camp, subject to remaining capacity and exclusion rules.
- A secret-marker candidate remains absent or clue-led until discovery permission is earned.
- The map shows the required gate as required, the optional site with its actual certainty, and no fabricated route for the secret point.
- If the gate cannot be selected, use the specific substitute/delay/clue policy authored for that objective. Rillstep is not equivalent proof of gate inspection.

**Recipe B — main inspection active, gate temporarily unavailable.**
- Keep the quest visible and report that the required site cannot currently be reached.
- If another site can satisfy the exact objective semantics, show it as a declared substitute tied to the original request. If it can provide only partial evidence, do not mark the gate objective complete.
- If no equivalent location exists, delay with a clue or safe report-only route. The selected optional sites should not give the impression that the player can complete the unavailable objective by visiting unrelated locations.
- The journal and map agree about whether the quest is blocked, deferred, or has a supported alternate path.

**Recipe C — player accepted a faction meeting and a character task.**
- Give the eligible meeting location a presence guarantee only if it is required for an active accepted task.
- Add no more than one optional site that competes for the same travel or interaction window.
- Do not make both requests expire in the same expedition unless that competition is the explicit, disclosed gameplay challenge.
- A character's absence changes dialogue eligibility, not the stable identity or permanent availability of the shelter.

**Recipe D — no active required location, late-game coalition enabled.**
- The selection pool may include High Meridian Perch as a rare faction-specific site if its prerequisites are satisfied.
- The site should appear only at the authored rarity and selection stage, not because a random draw bypassed an unmet story condition.
- If it is omitted, the late-game storyline remains available through its canonical offer route; absence in one expedition is not a failure.
- A second route may provide a clue or future opportunity, but it must be authored through current discovery/quest owners.

**Recipe E — core-only package, old save contains optional references.**
- Remove optional destinations from eligible selection.
- Required core objectives retain their normal destinations or documented substitutes.
- A persisted optional discovery is either safely ignored by the current map view or displayed with neutral, package-independent copy. No missing resource key blocks map rendering.
- The selection result remains deterministic under the configured package set and seed contract.

### Location-specific encounter and clue scaffolds

These encounter seeds provide enough structure for a content author to define playable visits without inventing new state owners.

**Switchback Sluice: the handwheel test.** On arrival, the player sees a chain of scratches where the wheel has been marked at different heights. The encounter distinguishes observation from operation: inspect the scale; ask who last turned it; or request a safe test if a current command/quest owner permits it. The test has no automatic safety guarantee. If an operation is unavailable, the site still offers the observational route, and the arrival description does not depict an unapproved world change.

**Rillstep Intake: the two-sound approach.** Water passes behind the screen wall, while the channel edge carries mineral staining. A careful player can inspect both from the public path; a risky close approach may require a supported expedition choice. The site can corroborate direction, contamination report, or recent flow only when the relevant world owner supports that inference. A sound or stain alone should not prove potable water.

**Mothglass Nursery: the shaded workbench.** The shelter keeps seedlings behind fogged panes and lends tools only through its existing inventory/interaction owner. The room's lore shows care practices: repaired labels, a rationed heat vent, a stool set aside for a tired worker. The location does not grant a free crafting bonus or rest action by description alone. If a crafting service is absent, the scene is descriptive and a different supported workshop route is needed.

**Lintel Camp: public notice rail.** Expedition crews leave route slips on a covered board. The display can summarize known opportunities from the current map/quest read models, with each notice attributed to a source and certainty. It must not become a second quest board that owns offers or task state. A notice can lead the player to a proper current offer or simply clarify a report.

**Sootstep Marker: the double notch.** A short clue in the landscape identifies two old measuring positions, one partly obscured. The player may discover the marker by environmental observation, a knowledge gate, or a report. The exact route is hidden until the appropriate discovery fact is accepted. If an expedition does not contain the visit, the player retains the hint but does not get a false location marker.

These scenes can be reused as clue/arrival templates only when their objectives remain distinct. The same double-notch text should not reappear at several unrelated sites with no explanation. A location card lists reusable elements (route-slip board, sheltered alcove, weathered measurement surface) separately from unique lore (why a mark was cut, who uses the workbench, what a public notice means).

### Required, quest-only, temporary, and secret visibility

Map visibility is determined by the appropriate current source, while expedition selection uses the requested pool and constraints. A quest-only location is not necessarily secret: an accepted main task may mark a known gate as required. A secret site is one whose exact destination has not been discovered, even when an active quest allows a clue about its region. A temporary visit can use a permanent anchor but has an expedition-specific opportunity window. These categories can overlap only when the content card defines how.

| Site type | Before discovery | After discovery | While required | After opportunity closes |
|---|---|---|---|---|
| Permanent known anchor | Region or named landmark according to map facts. | Exact named site with current routes. | Required marker and quest relevance. | Landmark remains, but its interaction state may change. |
| Quest-only destination | Clue, rumor, or no marker according to disclosure. | Exact marker if discovery owner confirms it. | Mandatory appearance or explicit blocked/fallback explanation. | Accepted quest receives delay/alternate/closure status from its owner. |
| Secret destination | No exact marker; clue source only. | Exact marker after supported discovery. | Quest may point to earned hint without leaking the coordinate. | Secret knowledge can persist even when a visit is not selected. |
| Temporary opportunity | Reported window and uncertainty. | Visible while current owner says available. | Guaranteed only when the accepted quest contract requires it. | Closed opportunity gets a clear reason and any defined later route. |
| Faction-specific site | Hidden from unqualified map view unless a public clue exists. | Visible after eligible access/discovery. | Required only for that active faction route. | Core map does not retain unusable references to disabled package content. |

Filtering may hide optional markers but cannot remove a mandatory active destination or falsify an unavailable result. When one site supports multiple public requests, its detail view can list them. Protected or secret requests should not be disclosed through a marker title, tooltip, accessibility label, route color, or sorting position before their gates are met.

### Density, repetition, and performance review

High content volume becomes unreadable when every expedition offers many technically valid places with little difference in purpose. Review the portfolio by expedition start, not only by the total number of location records. Each generated selection should have a legible pattern: one required goal or no required goal; a small number of optional sites with nonredundant functions; and a controlled chance for a rare/faction site. The exact counts should be measured against current UI capacity and map layout rather than fixed here.

A density worksheet for each start lists:
- number of mandatory requests and their compatibility class;
- number of optional candidates by semantic role;
- overlap between candidate quest purposes;
- which candidates share an anchor or map edge;
- which destinations are discoverable now versus merely reported;
- maximum marker count and text length on the active screen;
- whether any optional site duplicates another site's only useful reward;
- whether repeated runs present a meaningful alternative.

The location selector should avoid presenting several sites that all offer only the same inspection clue. A map full of duplicates gives apparent variety without player choice. Conversely, two sites can share a route if their encounter opportunities and consequences differ clearly. A resource source is not mechanically unique if another supported owner can fulfill the task, but a substitute must preserve the required proof semantics.

Performance review records location definition count, eligible candidate count, reference-resolution work, map marker generation, and repeated visit refresh cost using the existing instrumentation and project budgets. Avoid recomputing rich prose or resolving every irrelevant package on each map refresh. A stable view model can be cached only under current ownership and invalidation rules; it must not become a shadow availability database. If the source owners provide events, use the established refresh seam and dispose subscriptions with the panel lifecycle.

### Location release acceptance for this portfolio

A location packet is ready for future integration when:
- each card declares permanent anchor versus temporary visit behavior;
- every quest reference resolves to a compatible location role or has an authored fallback;
- location appearance is repeatable under the established deterministic selection rules;
- a location's map visibility matches discovery and disclosure facts;
- risk and route certainty are communicated separately;
- a temporary opportunity can expire without deleting a permanent place;
- the same location can host a safe optional encounter without satisfying a different proof incorrectly;
- package-off play has no required reference to High Meridian Perch;
- scene reuse does not erase unique site identity or imply unsupported mechanics;
- focus, descriptions, route labels, and map interactions remain usable at full content density.

The first production slice should integrate Switchback Sluice and one compatible corroboration site. Confirm that the current dispatch/selection owner can reserve a required location, that map presentation reads its actual state, and that quest proof uses the current quest owner. Claim exact paths only after checking the live worktree ledger. If no current location identity or availability owner can meet the requirement, stop at that seam and record the decision needed.

### Dispatch-screen review for the Tidemark portfolio

The same destination can carry a required quest request, a reported clue, an optional resource stop, and a temporary meeting opportunity. The expedition screen should group this information by player decision rather than expose the internal selection pools as a technical list.

| Screen moment | What to show | What remains hidden or qualified |
|---|---|---|
| Expedition opening | One clear active goal, required location if eligible, and a concise “why this site matters” summary. | Do not expose a secret site or imply that a generated candidate is a permanent landmark. |
| Candidate comparison | Optional destinations with certainty, distance/route information available from current owners, and whether they support an active optional task. | Do not label a rumor as a guarantee or imply that optional stops satisfy a different objective. |
| Confirmation | Final selected route and any declared compatible substitute. Explain a delayed required location and the reason. | Do not make a nonbinding faction trial look like a scheduled visit. |
| During expedition | Preserve the committed target and show supported route changes as explicit events. | Do not silently reroll a destination when a panel refreshes or a quest predicate changes. |
| Return/reopen | Reconstruct current opportunity and discovery facts from their owners. | Do not resurrect a temporary visit that expired or was consumed. |

If a mandatory destination becomes unavailable after the player opens the comparison screen but before dispatch acceptance, the UI refreshes and explains the changed selection. It does not move focus to an unrelated candidate or submit the old route under a new location identity. If the user has already accepted the dispatch, later panel disposal cannot cancel the committed expedition unless the current expedition owner explicitly supports cancellation.

The map legend, list entry, confirmation summary, and accessible text use the same words for required, discovered, reported, unavailable, and substitute. A user navigating by keyboard/controller should receive the same reason and risk information as a user hovering over a marker. Large prose stays in a details/transcript view so that the selection list remains scannable. These constraints should be reviewed at maximum supported candidate density, with focus restoration after a required marker changes and an available back/cancel action at each pre-acceptance stage.
## Continuation pass 13 — Farline Circuit regional location network and dispatch compositions

This pass adds a provisional geographic network for The Farline Circuit. It concentrates on what places mean in relation to one another, which quest families can use them, and how an expedition map can communicate a route without pretending that every site is present on every outing. Site names are draft content. A location definition remains permanent authored content; whether a selected expedition includes a visit remains the responsibility of current location and dispatch owners.

### Regional concept and spatial grammar

The Farline region is a set of separated terraces linked by old elevated paths. An obsolete signal can travel farther than a person can safely travel, which makes reports and physical access visibly different kinds of knowledge. The map should show three readable relationships: high paths connect some sites without entering low ground; shelters are safe staging anchors but not automatically quest destinations; and a signal line is a report about communication, not proof that the represented route is open.

The map need not simulate radio propagation or route physics in order to present this story. It can show known landmarks and owner-supported route connections, label reports by source, and reveal exact site markers only when discovery rules permit. If the source owners do not model a signal's range, the content describes a message as “heard from” a location rather than drawing an authoritative coverage field.

### Location network cards

| Provisional location | Stable authored role | Quest-family fit | Visibility and selection |
|---|---|---|---|
| **Siltglass Relay** | Permanent signal tower with a plate, service hatch, and accessible exterior inspection point. | Main investigation, faction review, location-only objective, possible hidden clue. | Required only after the main quest accepts the request; otherwise a report or regional clue can lead to it. Exact service hatch access is separately gated. |
| **Half-Span Shelter** | Permanent shelter constructed beside the approach to a missing span; current occupancy may vary. | Character conversation, escort staging, survival recovery only if existing shelter owner supports it. | Known anchor after discovery; quest presence is a current world/character fact. Does not imply a guaranteed rest or protection action. |
| **Crowstep Span** | Route landmark where the old crossing broke and the safe route bends away. | Environmental discovery, survival path choice, escort failure-forward. | Region-level clue before discovery; visible obstruction after a supported map fact. It is not a substitute for inspecting the relay plate. |
| **Lantern Yard** | Public work area where the Wardens prepare lamps and route notices. | Resource/crafting, faction meeting, repeatable job template if current quest owner supports repetition. | Optional or character-specific pool; a public notice can be read without accepting its task. |
| **Old Echo Cut** | Former route cut with a second signal mark and partial traces of an abandoned path. | Hidden/discovery quest, alternate evidence, location-specific side story. | Secret until an earned clue or report; selection can be deferred while preserving the clue. A generated visit must reference this stable definition. |
| **Powder Orchard** | Wind-sheltered line of weathered trees and discarded route tags; its name is a local description, not a resource guarantee. | Exploration, environmental prose, low-risk alternative vantage point. | Rare thematic site. Do not promise food, crafting components, or safe rest without corresponding owners. |
| **Farline Overlook** | Late-game signal observation point used for a coalition meeting and comparison of public notices. | Faction quest and late-game resolution scene. | Faction-specific optional pool, gated by package and supported prerequisites; never mandatory to the core route. |
| **Lower Walk Service Niche** | Small temporary visit role attached to an authored raised path. | Escort handoff, recovery of a missed route slip, short timed opportunity. | Temporary pool. Its visit lifetime follows the existing expedition owner; it does not become a new permanent location when selected. |

Each site card needs a semantic role and the exact compatibility features needed by its consumers. “Relay-like” is insufficient if one objective requires a fixed engraved plate and another only needs a place to hear a report. Define a small compatibility vocabulary that can be mapped to current data fields after source inspection; do not create an unbounded set of one-off tags.

### Route graph and expedition topology

The authored region graph identifies stable route relationships and known alternate connections. It is not a separate world simulation. The current map/travel owner remains authoritative for actual travel, route state, and costs. A conceptual graph for this narrative is:

- Half-Span Shelter connects to the known upper path and a public route notice.
- The upper path approaches Siltglass Relay without crossing Crowstep Span.
- Crowstep Span once connected to the lower walk; current availability is unknown until the route owner confirms it.
- Lantern Yard provides a work-area lead and a Warden contact, but does not guarantee that the contact is present.
- Old Echo Cut is discovered through a clue from either the relay plate or a supported character report.
- Powder Orchard provides a view of route tags but is not evidence that a route remains passable.
- Farline Overlook is introduced only after late-game eligibility and can host a meeting without changing the stable identities of the other places.

When a dispatch selection includes multiple destinations, the map should distinguish a requested route from a selected sequence. If the current game only selects one destination per expedition, the content must not imply that the player can visit all points in one trip. If multiple stops are supported, the sequence, return condition, risk, and quest evidence source must be explicit. The proposed map design adapts to the existing expedition shape instead of requiring a new route planner.

### Quest-family availability by place

| Family | Natural location role | Availability rule | Boundary |
|---|---|---|---|
| Main investigation | Siltglass Relay, with clue/report lead from shelter | A quest request may guarantee the relay only if the current selector can do so. | The core question remains answerable through an authored fallback if unavailable. |
| Character | Half-Span Shelter or Lantern Yard | Character/quest owner confirms who is present and whether conversation is eligible. | A stable shelter does not guarantee the same cast every visit. |
| Faction | Lantern Yard or Farline Overlook | Current faction offer and location access both qualify the scene. | A faction-specific location is not a new faction-state owner. |
| Discovery | Crowstep Span or Old Echo Cut | Discovery facts reveal clues in stages. | Marker visibility follows the map/discovery owner. |
| Escort/protection | Half-Span staging and Lower Walk handoff niche | A valid dispatch and supported actor availability are prerequisites. | Do not place a required escort at a site the selection contract can omit. |
| Survival | Crowstep approach or exposed route | Existing environment/expedition facts determine the known risk. | Do not calculate new damage or weather in location content. |
| Resource/crafting | Lantern Yard or current workbench role | Existing recipe, item, and crafting owners validate materials. | Location prose alone cannot grant a component or craft result. |
| Timed | Lower Walk temporary opportunity or meeting window | Existing time owner confirms the window. | Expiration changes the opportunity, not unrelated sites or the main quest. |
| Repeatable/rotating | Lantern Yard notice | Only if the current quest owner offers reset/repeat semantics. | The location does not rotate task state itself. |
| Hidden/environmental | Old Echo Cut marks | Clue source and disclosure policy authorize visibility. | Discovery is distinct from objective completion. |

This matrix avoids a common content error: one location being called “required” for every quest family simply because it is narratively convenient. A place can host several activities if each has a clear owner and compatible objective. The map view lists public purposes that are currently available; it does not list every dormant possible use.

### Expedition compositions and candidate outcomes

**Composition A — early chapter, report received but no relay discovery.** Half-Span Shelter appears as a known anchor. A public report points toward a regional area, not a precise hidden marker. Siltglass Relay can be selected as the quest-required destination if the quest owner and selector agree. Old Echo Cut remains absent; Powder Orchard is a possible optional site only if it does not crowd out the mandatory request.

**Composition B — main quest active, relay route blocked.** The dispatch preview names the relay request and the current owner-backed route problem. If an equivalent proof site exists, it is offered as an explicit alternate with limited proof scope. Otherwise the task is deferred or routes to a report-only conclusion. Crowstep Span may explain why a lower route is unavailable, but the map must not present it as a safe shortcut. The optional Yard visit cannot be misrepresented as solving the relay inspection.

**Composition C — escort accepted.** Half-Span Shelter is the staging anchor; the actual temporary handoff niche is selected only if compatible and available. The destination summary discloses the escort goal and any known route risk. If the current expedition supports only one location, the handoff is a quest scene at the chosen anchor; content does not invent a two-stop trip.

**Composition D — Old Echo clue earned.** Old Echo Cut enters the eligible secret pool. A map marker can be “hinted” until the player reaches the supported discovery threshold, then becomes exact. One visit can establish the second signal mark; later dispatches may show the permanent discovered location even when no visit is currently selected. The quest owner decides whether the discovery supplies proof.

**Composition E — late-game meeting eligible.** Farline Overlook becomes available as a faction-specific possibility. The selector still prioritizes any active mandatory quest request. If the meeting site is absent this run, the faction task remains eligible or visibly delayed, depending on its owner. The map does not imply the meeting happened offscreen.

### Fallback equivalence and substitutions

A substitute location can only satisfy a required objective when it supplies equivalent evidence. The content review uses a three-part equivalence test:
1. **Object equivalence:** does the place expose the same kind of authored object or observation?
2. **Authority equivalence:** can the same current owner accept the evidence produced there?
3. **Player-understanding equivalence:** does the journal explain why visiting the alternate location completes or partially advances the task?

Siltglass Relay and Old Echo Cut are not automatically interchangeable. The first can prove that a signal plate is old; the second can corroborate that another signal existed. One may support a partial conclusion, but it cannot silently stand in for the other. Half-Span Shelter may provide a report but not the physical inspection. Farline Overlook may host a faction conversation but cannot replace a local mechanism visit.

If equivalence fails, fallback means delay with a clue or accept a lower-confidence conclusion, not substitution. The player sees which route they received and why. The original request remains linked to the substitute for task details, but no second location registry is created to store that relationship.

### Map information and local atmosphere

The map's graphic vocabulary should show relation, not certainty inflation:
- a stable line shows only routes the travel/map owner supports;
- a dashed report line may show a source's claim when the player has earned it;
- a broken bridge symbol means unavailable only when the owner confirms that status;
- a lantern icon may denote a signal-related place only if symbols are consistently explained;
- a shaded area can represent unknown terrain without implying a hazard mechanic;
- a faction mark appears only when the player is allowed to know the group has a relevant presence;
- a required marker is distinguishable from an optional objective or secret clue.

Atmospheric site descriptions are localized content, not map-state labels. “A low route disappears in the dust” can be prose even while the UI says “route status unknown.” Avoid describing a road as broken before the location owner says it is. The story can contain rumor, but the interface attributes it.

### Location-only missions and reuse catalogue

Location-exclusive tasks should have authored entrance and closure behavior. A task whose only interaction is at Lantern Yard has a location-specific route. If the yard is not selected, the player may not lose the task unless the current time/opportunity owner closes it. If a short activity occurs at Lower Walk Service Niche, it must have a safe abort/re-entry rule for an interrupted expedition.

Reusable production elements include: notice-board interaction, sheltered route briefing, inspection boundary, visible obstruction, signal plate, service niche, and debrief conversation. Unique elements include the precise meaning of each mark, who placed it, the location's access rules, and which proof can be submitted. A shared scene kit must not copy a required clue from Old Echo Cut into every relay location.

For each reuse candidate, record:
- allowed location roles;
- scene and quest families supported;
- command or read owners;
- which details are generic and which are unique;
- what can be omitted when a profile disables the location;
- how generated visit context is presented;
- accessibility labels and route certainty;
- maximum number of similar sites shown in one selection.

### Regional narrative continuity

A player should not need to memorize the whole region graph to understand the next step. The dispatch summary identifies one actionable destination, one reason to go, and any mandatory/optional distinction. The journal describes outstanding dependencies; the map shows earned knowledge. Environmental discovery can change the map without forcing the player to accept a quest. If an expedition reveals a permanent anchor, current discovery ownership controls whether it remains visible.

The narrative makes the separation between communication and access part of its theme. Hearing a signal means the player encountered a report or cue through the current presentation owner. Seeing the relay means the player visited a location. Knowing the road remains open requires a current route fact. Having permission to enter a faction meeting requires the appropriate availability fact. These statements should never collapse into one “signal found” Boolean.

### Content/performance acceptance

Before adding a large site catalog, run an authoring review using a bounded representative subset. The report notes candidate count, package resolution cost, selection input size, map marker count, description/localization volume, and refresh behavior through existing instrumentation. Keep one representative of each difficult role: a permanent required anchor, a temporary visit, a secret site, a faction-gated meeting, and an optional scenery location.

The acceptance review confirms that:
- selection never drops an active required destination silently;
- selection does not select incompatible sites solely because their tags overlap;
- optional sites remain varied without crowding out mandatory requests;
- secret markers cannot leak through title, sorting, accessibility text, or map grouping;
- location appearance is deterministic under the established seed contract;
- a selected temporary visit does not become a permanent authored anchor;
- old and package-off profiles do not require Farline-specific content;
- map and journal wording agree on route certainty;
- repeated location use preserves unique narrative identity and current owner state;
- the maximum content set remains navigable and responsive under the project’s actual panel budget.

No fixed candidate count is imposed here. The current expedition architecture, not this content plan, determines the supported maximum and dispatch shape. The first slice integrates Siltglass Relay and Half-Span Shelter only, then adds the optional Old Echo Cut after proof ownership and discovery visibility are verified.

### Location visit dossiers and playable exploration loops

The regional cards need more than a role label to support implementation and narrative review. The following visit dossiers describe what the player can do, what information can be earned, which task families can use the place, and what a safe conclusion looks like. They are proposal scaffolds for content authors. Any object action, route lock, resource, or hazard requires a verified existing owner.

#### Siltglass Relay — required inspection anchor

**Arrival:** The tower is built into a slope of pale glassy grit. A signal plate is bolted beneath a roof lip where runoff cannot reach it. The old tone can be referenced in authored text or a current signal/audio route; no playback is guaranteed without an existing cue consumer.

**Exploration loop:** Orient at the exterior; inspect the public plate; request access to the service hatch if an owner-approved interaction exists; optionally compare the plate with a report; choose to submit or withhold a conclusion. There is no forced “activate signal” step.

**Possible content:** main investigation, faction review, environmental note, optional maintenance clue. It does not host crafting or safe rest merely because the scene has a roof. A required quest reference points to the stable authored anchor. If the site is unavailable, the selection system follows the declared delay or lower-confidence route; it must not generate a duplicate relay.

**Public information:** “Relay site known; plate inspection not confirmed.” After the objective is accepted, map/journal state can say the plate was inspected. The status is owner-backed, and the physical description does not change until a world owner supports that change.

#### Half-Span Shelter — character and staging anchor

**Arrival:** A shelter has been built against the remaining bridge abutment. Its interior shows different hands at work: some boards are numbered, others patched twice, and a low shelf carries a row of capped cups. A hanging cloth is used to signal whether a private conversation is in progress only if the scene interaction owner supports that convention.

**Exploration loop:** Read public notices; meet a currently eligible character; ask about the old crossing; request a staging action if supported; leave. The player can enter for atmosphere even with no active quest, but should not see a false objective prompt.

**Possible content:** Yara's character thread, escort staging, route report, optional shelter lore. A current owner decides occupancy, shelter services, and travel. A missing character produces a neutral shelter scene, not a broken quest or an invented alternate character.

**Reusability:** The shelter can host different chapters because its stable architecture gives it identity. A seasonal coat, changed public notice, or character presence can vary only when corresponding content/world facts support it. The repeated scene should not look reset if the player previously accepted a durable owner result.

#### Crowstep Span — route uncertainty and failure-forward site

**Arrival:** The old span ends at a cut edge. A newer footpath turns back toward higher ground, but its current state is unknown until travel or expedition owners report it. A loose cord hangs where someone once marked the lower way.

**Exploration loop:** Identify the old path; compare any earned report; select a supported route or return. If the route owner says the lower path is blocked, the encounter does not invite an impossible traversal. If route status is unknown, the UI says unknown rather than blocked or safe.

**Possible content:** survival route choice, environmental discovery, escort alternate, route-safety clue. This site can explain why a delivery failed, but it cannot prove the signal plate is obsolete by itself. If the player never reaches Crowstep, the main quest remains possible through Siltglass.

**Failure-forward use:** an aborted crossing can create a clue or partial result only if an existing quest/expedition owner records it. It does not automatically apply injury, item loss, or companion removal through scene text.

#### Lantern Yard — local faction contact and task notice

**Arrival:** The yard consists of two roofed work bays and a rail for public route slips. A set of mismatched lamp shades hangs above the table; each shade has a repair mark. This gives the Wardens a lived-in visual language without inventing a special lamp resource.

**Exploration loop:** inspect a public route slip; speak with a current contact; prepare an item using a supported recipe; accept, decline, or defer a posted request. Public reading is informational. Task acceptance is a separate command.

**Possible content:** faction offer, Hush Kit recipe if an existing crafting route consumes it, optional notice-board task, discussion about route signal ethics. The yard is not a new quest authority and its notice rail cannot persist a duplicate accepted-job list.

**Reusability:** Notice board interaction and roofed-work-bay layout can recur, but task text carries source, time, and certainty. A rotating offer appears only through the current quest/task owner if repeat behavior exists. Missing faction context falls back to public route information.

#### Old Echo Cut — secret discovery role

**Arrival after earned clue:** The path narrows between old stonework. Three shallow marks interrupt a row of weathered notches. The third is worn around the edge, as if someone kept returning to it with a different tool.

**Exploration loop:** Recognize a supported clue; identify that it concerns a second signal rather than the relay itself; collect an observation only through current proof ownership; leave the location. The visit should not play as a full puzzle lock unless a current interaction owner provides such a mechanic.

**Map and quest behavior:** Before the clue, no exact marker or accessible label reveals the site. After the clue, a hint can appear. After discovery, a stable marker can persist. If the expedition does not select the site, the clue remains unresolved rather than falsely discovered. No generated variation changes the meaning of the three marks.

**Optional use:** It can supply an alternate route to explain signal history, but the main investigation still works without it. A late-game faction may mention it only after the supporting fact is accepted and disclosure is permitted.

#### Farline Overlook — coalition site

**Arrival:** A covered ledge faces the raised paths. Two sighting bars point toward separate valleys, but neither is labeled as a current route. One has been cleaned recently; the other is still dusted with pale grit.

**Exploration loop:** Arrive only after an eligible late-game offer; participate in a meeting; inspect public reports; make a supported recommendation or leave. The site is not an ending room that automatically resolves faction choice.

**Profile behavior:** Under a core-only package, the location is not required and should not appear as a dangling marker. If an old save remembers a public report, show package-safe copy only through existing compatibility rules. If a meeting is unavailable, do not send the player to an empty overlook.

### Expedition selection trace samples

These traces test the interaction between authored roles and the selection owner without prescribing a new algorithm.

| Input condition | Required result | Optional result | Prohibited inference |
|---|---|---|---|
| Main relay quest active, Siltglass valid | Siltglass enters mandatory candidate set or a valid explicit delay is shown. | At most the configured number of compatible optional locations. | Do not treat Crowstep as equivalent relay proof. |
| Main quest active, Siltglass blocked by current route owner | Keep quest in an actionable blocked/deferred state. | A clue or report route may be offered. | Do not silently omit the request and fill every slot with optional sites. |
| Old Echo clue not earned | No exact Old Echo marker or mandatory selection. | A public rumor can appear if content authorizes it. | Do not leak the secret through accessibility name or sorting order. |
| Escort task active, actor/route supported | Select the required staging/target role consistent with existing dispatch shape. | A neutral optional location can fill remaining capacity. | Do not assume the generated visit will contain the actor. |
| Faction meeting offered but not accepted | Meeting location is optional or absent. | Other thematic sites can appear. | Do not force a faction visit because dialogue exists. |
| Faction meeting accepted and destination required | Provide supported guarantee or explicit delay. | Optional destinations respect the active slot. | Do not report adoption or meeting completion just from selection. |
| Package disabled | All core-required locations resolve. | No optional faction site. | No core quest may wait on Farline Overlook. |
| All optional candidates excluded | Required location remains, and remaining slots can stay empty under current design. | A neutral non-quest route may appear if supported. | Do not create a random location with no authored definition. |

The selection trace should be deterministic wherever the existing contract requires it. Candidate iteration is stable. Any variety draw uses the current seeded RNG, not a second selector, clock-derived seed, or hash order. Tests for exact selection are chosen later under the project's active policy.

### Route certainty and hazard communication

Risk descriptions should distinguish known hazard, reported hazard, unknown route, and unavailable location. A player may choose to explore an unknown route if the existing expedition system supports that action, but the copy cannot convert unknown into likely safe or likely fatal. The selection owner can provide the current risk representation; the content author explains it in plain language.

For each route:
- state the last known source when relevant;
- show whether the player has personally discovered the marker;
- tell the player if the destination is a required objective or optional lead;
- state if choosing it may consume an opportunity or expedition slot where the current UI supports that fact;
- avoid unsupported numeric danger percentages;
- offer a back route before the player commits;
- after dispatch acceptance, show only route changes actually published by the owner.

A blocked route is a world result only when the route owner says so. A missing location candidate is a selection failure, not a story event. A travel cost appears only when the current travel owner exposes it. These distinctions prevent the author from using atmosphere to conceal a real system failure.

### Visit lifecycle and generated-instance cleanup

The content package should define what persists after a selected visit ends, but only current owners can implement it. Permanent identity remains stable. Map discovery, accepted quest proof, and resource changes persist under their respective owners. A temporary visit can close when an expedition ends. The panel's display of that visit does not make it a permanent world object.

On interruption, the selected visit can be:
- **not started:** dispatch owner says the expedition never began;
- **active:** current expedition/session state owns it;
- **completed:** owner returns the accepted result;
- **aborted:** owner reports the abort and any supported partial facts;
- **unknown:** owner reconciliation must query current state.

The content should have copy for player-facing results but not persist local lifecycle flags. Reopening the map asks the owners for current locations; it does not restore stale candidate rows from a prior panel instance. If an existing save stores a visit snapshot, that format and migration remain with its current owner.

### Completion review for location-rich episodes

A large location package can be completed when each active quest has a required target or valid fallback, each secret location has an earned disclosure path, each visit has an ending/abort behavior, and each destination's story purpose differs enough to justify its selection cost. Reviews should include:
- a no-optional-site run;
- a high optional-density map;
- required and secret requests active at once;
- map reopened after a location becomes unavailable;
- a repeated permanent anchor with changed current owner facts;
- a generated visit selected but never reached;
- a saved/loaded expedition if current save ownership supports it;
- a profile where all Farline content is absent;
- a clue found while the corresponding destination is not selected.

The actual maximum list size, marker budget, and route layout are measured against the current UI and performance constraints. The plan provides cases, not invented thresholds.

## Continuation pass 13 — location selection rehearsal, fairness guarantees, and expedition composition

### Selection must preserve intent and uncertainty

The Farline Circuit supplies a practical stress case for expedition location selection. Several story beats prefer a relay, shelter, span, or lookout, but the player should not be able to predict an entire map from the quest title. The selection process must protect required progress first, reserve space for authored variety second, and allow optional discoveries to surprise the player. The user-facing map then communicates what is known at expedition start without pretending that unknown details have already been discovered.

This is a design contract for an existing expedition/location owner, not a proposed second picker. The first implementation step is to inspect the current candidate source, seeded selection call, map assembly, and persistence behavior. If those already solve any listed requirement, preserve them and update the content contract. If the current owner cannot guarantee active quest access or equivalent fallback, write the smallest missing behavior into its own integration plan and claim its paths before implementation. A new selection service is justified only if the current owner cannot be extended without violating its responsibility.

### Candidate pool roles

Pool labels describe authored eligibility. They need not become separate runtime registries.

- **Permanent pool:** ordinary sites available under region, campaign, and world-state rules. A permanent site may still be hidden until discovered.
- **Quest-required pool:** places needed to satisfy an active objective, with one or more authored equivalents if the exact site is unavailable.
- **Critical-progression pool:** places required to prevent the campaign from becoming impossible. This pool should be small and explicitly reviewed; it is not a way to force every favored side quest onto the map.
- **Character or faction pool:** places whose availability depends on a character arc or faction relationship. These candidates should not displace a mandatory objective.
- **Recently discovered pool:** places eligible for a clue-follow-up or return visit. Recent discovery affects weight, not guaranteed selection, unless a quest needs the location.
- **Thematic pool:** ordinary authored sites that fit current conditions such as winter exposure, damaged communications, medical scarcity, or a faction’s current activity.
- **Optional pool:** broadly valid sites used for exploration and resource variation.
- **Secret pool:** candidates whose discovery follows a clue, skill, prior visit, or hidden condition. Secret status controls eligibility and initial map visibility; it should not mean that a site is absent from all selection logic.
- **Temporary pool:** sites generated by or attached to a limited event, travelling group, weather aftermath, or short-lived situation. Each temporary candidate requires an expiry and post-expiry narrative state.

A candidate can have more than one role. The design data should not duplicate a place once for each pool. Instead, its eligibility contract identifies why it can appear, while selection uses a single candidate record and current world facts. Content tags are helpful only when they correspond to readable design meaning and an actual consumer.

### Priority, reservation, and remaining slots

At expedition assembly, first calculate whether each active quest has a valid completion route, considering authored equivalents. Reserve the smallest number of candidate slots needed to preserve those routes. A quest requiring one site should not silently consume three map slots because three possible equivalents all matched. If different active quests can share a location, share the candidate only when both stories permit co-location without contradictory temporal facts. Otherwise the picker should choose distinct locations or an authored substitute.

Next, reserve a critical-progression candidate only when the campaign state requires it and no current route already satisfies the dependency. Then consider one character/faction candidate when its availability is materially meaningful. Fill remaining positions from thematic and optional categories using seeded selection. Rare or secret candidates use a bounded chance after core coverage is satisfied. A rare candidate can replace a common optional site, but cannot evict a mandatory quest route.

The exact number of map positions belongs to current expedition rules. This plan avoids hardcoding a map size. The implementation should reason in terms of available slots and minimum guarantees. When the total capacity cannot hold every quest’s preferred site, attempt shared-site compatibility, then equivalent sites, then a clue leading to a future route, then delay the affected quest with a visible reason. Never solve capacity by silently making a quest impossible.

An example with four slots: an active investigation has Siltglass Relay as its preferred site and Old Echo Cut as a valid alternative; a character conversation can use Half-Span Shelter; the current winter theme makes Crowstep Span attractive; two ordinary sites remain optional. If the investigation’s exact relay is valid, reserve it. The character site may be chosen if it also serves a route objective and the content supports that co-location. A thematic span takes one free slot. An optional site fills the last slot. A hidden site can displace the optional candidate only if its clue has already made it eligible and rarity roll succeeds. If capacity shrinks to two, keep the mandatory route and critical progression; show the character quest as delayed or offer its already-authored alternate start.

### Exclusion rules

Exclusions should protect coherence, not manufacture sameness. Candidates may be excluded because the region is wrong, a prerequisite is absent, the site is destroyed or sealed, the expedition type cannot reach it, a one-time event is already resolved, a mutually exclusive location is selected, or a required character is unavailable. The exclusion reason should be available to diagnostics and, where player intent is affected, a concise journal/map explanation.

The following conditions should not exclude a site by accident:

- A prior visit, unless the location is authored as one-time or physically changed.
- A completed unrelated quest.
- A cosmetic discovery state.
- A different site sharing a broad thematic tag.
- A previous selection from another expedition if the site is revisitable.
- A rare-site cooldown applied to a quest-required site.
- A missing optional character conversation when environmental evidence can still be collected.

Mutual exclusion must be authored with an explanation. Two sites might represent the same convoy being in one place at one time; selecting both would create a timeline contradiction. In that case the second is excluded as a conflicting instance and its story consequence becomes a later report. Conversely, two compatible locations can appear together and strengthen a cross-check. Avoid blanket rules such as “only one Farline place per expedition” unless pacing evidence shows that several circuit sites would overload the map.

### Rarity and seeded variation

Rarity should be evaluated after mandatory and progression checks. A useful conceptual distribution is common, uncommon, rare, and exceptional, with no numeric weight assigned here because existing selection balance and map capacity need measurement first. Authors state a desired frequency band and narrative function; the current picker owner translates those into its existing deterministic weighting mechanism. Randomness should affect which valid optional sites appear, not whether a required route remains reachable.

A rare site should have meaningful rarity reasons: unusual timing, clue discovery, world-state change, or limited event window. Rarity cannot be synonymous with “better loot.” The Farline Overlook, for example, may appear rarely because weather and access align, and its value is a broader view of old route marks. It may yield no scarce resource at all. A repeatable clue should have a cooldown or diminishing informational return, while a unique discovery should never respawn in a way that implies the player forgot it.

Seeded selection must remain replayable. Candidate enumeration needs a stable order before weighted selection; ties cannot depend on hash iteration order. Selection should use the established RNG path and avoid wall-clock values. A testable trace should identify the seed, eligibility facts, exclusions, reserved candidates, and final draw in a redacted-safe diagnostic representation. The trace is for development and verification, not necessarily a player-facing log.

### Map visibility and discovery behavior

At expedition start, map visibility should distinguish at least four knowledge states in the presentation contract: confirmed location, approximate lead, discovered but currently inaccessible, and unknown. The actual UI may use icons, text, or another existing convention. Do not introduce a redundant location knowledge store if the current world map or journal already owns discovery.

Mandatory route information should be honest. If the player has a confirmed destination, show it as confirmed. If the quest gives only a rumor, show a lead region or uncertain marker and explain the basis. If the location appears because an active objective guarantees it, the map may reveal its name only when the player plausibly knows it. The picker’s private candidate list must not leak secret locations through map count, labels, empty pins, or a UI refresh that changes before a discovery event.

A clue can make a hidden place eligible before revealing its exact coordinates. The first expedition might contain a source note that narrows the route; the following expedition may place the site in a known area. If immediate access is necessary, show a search-zone marker and allow the player to spend expedition effort investigating. Discovery changes map knowledge through the existing owner. Visiting again can expose altered details but must not duplicate the discovery reward.

If an active quest location is replaced, the player should receive a clue or journal update that correctly explains the substitution. The map should not relabel a physically different site as the original. Equivalent function does not mean identical fiction. The quest text can say “compare the relay’s marks at another maintenance station” rather than claim that the player has found the original relay.

### Fallback ladder and visible outcomes

Each quest-required candidate supplies an ordered fallback ladder:

1. Use the preferred site when valid and capacity permits.
2. Use a named authored equivalent that preserves the objective’s meaning.
3. Place or reveal a clue at a currently selected compatible site.
4. Delay the step while keeping its reason visible and its other objectives usable.
5. Convert the objective into an explicit uncertainty or failure-forward closure if all physical evidence paths have been lost.
6. Escalate to a content defect if none of these outcomes is authored.

This ladder avoids quiet impossibility. It also prevents overuse of rescue behavior: an equivalent should preserve the narrative question, not merely produce a completion token. If the original asks who altered a message, an equivalent must provide evidence about the alteration’s provenance, not any unrelated note. A clue fallback should lead somewhere actionable within a reasonable number of expeditions or state why the lead is long-range.

The game may show a quest as blocked when its fallback requires a future state. It should state the relevant condition in ordinary terms, such as “the route is buried; wait for a survey report or find another maintenance record.” A blocked quest should not be repeatedly re-added to the map as if it were active. The map may show its approximate area only when that signal is useful and known to the player.

### Rehearsal cases

**Case A: several required objectives, ample space.** Reserve both required routes, deduplicate if both can use the same shelter, choose one faction site, fill remaining positions with theme and optional candidates, and roll for one secret site. Validate that each active quest’s next action is on the map or has a clear non-location action.

**Case B: capacity pressure.** The expedition has fewer location slots than active objectives. Prioritize critical progression, then choose a location that advances two compatible quests. Mark the other quest blocked with an authored fallback; do not report it failed. If the player can choose which quest to postpone, communicate that choice before departure.

**Case C: faction character absent.** A faction quest expected to use a courier is active, but the character is not available. Use an environmental notice, a later meeting location, or an alternate witness if authored. Do not select an empty site merely to satisfy a candidate tag.

**Case D: required site already resolved.** A one-time investigation is completed, but another quest refers to the same physical location. The location remains eligible as a revisit with altered interaction content. Selection should check the current site state rather than assume completion globally excludes it.

**Case E: temporary site expired.** A moving caravan departed before the next expedition. Preserve the player’s earlier knowledge and offer a consequence through a notice, rumor, or another route. Expiry changes access; it does not delete the past.

**Case F: no valid candidate.** The pool is empty after region, capacity, and world-state rules. Use an authored fallback route or report a validation failure during content review. In a running save, never crash or silently omit the quest. The safe player-facing state is delayed with a precise reason and a valid next step if one exists.

**Case G: hidden location discovered early.** The player’s skill or prior clue makes a secret candidate eligible before the associated quest starts. It can appear if its content is independently available, but the quest should recognize the location as already known and not force a second “discover” action.

**Case H: duplicated active quest starts.** Two conversations offer the same work. The selection layer sees a single active quest and a single required route. If the current quest owner allows multiple linked contracts, each contract must state whether it shares or duplicates objective proofs.

### Composition fairness and player agency

Location selection is fair when it protects authored intention while preserving player choice. It is not fair merely because a map marker appears. The player should have enough departure information to choose gear, companions, and risk posture without revealing every encounter. When a location is mandatory, the game may show the destination and hazard summary. When it is optional, its purpose should remain partly uncertain. When an objective can be completed through several routes, the map should reveal only the routes already known.

The selection system should not make the player feel punished for accepting many quests. If simultaneous active quests exceed practical map capacity, the campaign needs one of three explicit design choices: lower the number of concurrent starts, allow efficient co-location, or permit multi-expedition pacing with visible blocked states. A hidden capacity rule that delays random quests without explanation produces an unreliable journal. Conversely, allowing every active quest to force a map slot can drown out survival exploration and make each expedition a checklist. Content authors must therefore label a required site as hard mandatory, soft preferred, or optional support, and specify the maximum delay the story can tolerate.

### Verification expectations for a future implementation package

A future owned implementation should validate: mandatory route guarantee; capacity pressure; equivalent selection; no-valid-site fallback; map-knowledge honesty; secret-site non-leakage; repeatable site behavior; expired temporary site; compatible co-location; incompatible candidates; seeded repeatability; stable tie-breaking; save/load reconstruction; and no duplicate active identity. These checks should be placed in the existing targeted test owner after an approved work package identifies the exact runtime seam. This plan does not create new tests or name an assumed class.

During content validation, report each candidate’s eligibility contract and each quest’s route guarantee. A human-readable trace should distinguish exclusion from low probability: “excluded: wrong region” differs from “eligible but not selected.” Without that distinction, balance tuning and quest debugging conflate authoring errors with random outcomes.

### Integration order and acceptance

The safe order is: verify current location and quest authorities; map the existing candidate shape; author one Farline location and one quest fallback against that shape; rehearse map visibility; verify seeded selection and persistence owners; then expand the candidate set. Add no broad pool abstraction before proving the current owner cannot express the categories. The minimum playable slice should include one active mandatory location, one equivalent, one optional site, and one discovery-gated secret. This proves selection fairness and map honesty without requiring all story locations.

Acceptance requires a reproducible selection trace, a visible response when a preferred place is missing, no quest that becomes silently impossible, and no duplicate location catalog authority. The content can be large, but the runtime change remains bounded to the existing owner. Any mismatch between the plan’s guarantees and the current architecture becomes an explicit decision request with evidence, not a local workaround.


### Departure briefing and player-facing selection trace

Selection is only half of the expedition experience; the departure briefing tells the player what they can responsibly infer from the chosen map. It should separate confirmed destinations from leads, identify the reason a mandatory site is included, and summarize known hazards without revealing encounters that remain uncertain. A brief may say that the route crosses exposed ground and that a shelter is expected to be occupied. It should not promise an NPC will be present if availability can change during travel.

Each selected place should have a player-facing purpose phrase grounded in known information: “inspect the old timing board,” “check the span marker,” or “ask whether the copy came through the yard.” An optional place can use a broad clue such as “a dry structure near the cut.” Avoid a map full of unexplained named pins, which makes selection feel like a checklist and weakens discovery. If the player cannot know the exact function, the label should communicate uncertainty rather than expose internal pool tags.

The briefing also supports meaningful loadout choices. If the player knows a relay room may be flooded, they can decide whether to bring equipment suited to wet conditions. If they only know that the site is damaged, preserve uncertainty about the exact hazard. The content team should identify the minimum trustworthy information needed for each expedition type and verify that selection never hides a known critical hazard merely to produce surprise.

For debugging and review, preserve a compact selection explanation in development output: which candidates were required, which were excluded and why, which were eligible but not drawn, and which fallback was used. This explanation must not become a second gameplay database. It can be a transient trace produced by the existing selection owner. A reviewer can then distinguish bad weighting from broken eligibility, and a player-facing bug report can be reproduced from the seed and campaign conditions.

The map should remain usable if a selected optional location is unavailable after departure. If a route closes, update its state through the existing location owner, retain the player’s knowledge of why it was selected, and offer the authored alternate action. Do not silently remove the pin or substitute a different place behind the player’s back. If the current expedition model freezes locations at departure, the plan should honor that rule and make environmental change apply to the next selection instead.

Review the briefing against three player profiles: a cautious planner who reads every label, a returning player who knows the site, and a first-time player with only a vague clue. The first should have enough reliable information to choose, the second should see updated conditions rather than redundant introduction, and the third should preserve a sense of discovery without becoming unable to plan.

### No-candidate policy and deterministic review record

When selection produces no valid candidate, the runtime needs a deliberate fallback rather than an empty expedition assembled accidentally. First determine whether the expedition can proceed with fewer optional locations while preserving required content. If yes, keep the valid subset and communicate the smaller route. If a quest requires a location, consult its authored equivalent list and verify each candidate against current conditions. If none is valid, preserve the quest’s progress, label the reason, and provide a non-location action or delayed route where one exists. If critical progression has no candidate or equivalent, surface a development integrity failure and a safe in-game recovery state under the established error policy.

A selection review record should identify the campaign profile, seed, active quest set, region, expedition capacity, candidate eligibility, exclusion reasons, mandatory reservations, equivalent substitutions, optional draws, and final map. It should be concise enough to compare across seeds. This record distinguishes a candidate that was invalid from one that was valid but not selected. It also makes a reproducible bug report possible without exposing secret candidate names to ordinary players.

Deterministic selection must not be changed by catalog enumeration order. Sort or otherwise stabilize the candidate input using the existing owner’s established approach before consuming the seeded RNG. Do not consume random draws for candidates already reserved as mandatory if that would shift optional draws unpredictably across content additions. If the current algorithm already uses a different stable contract, preserve it and document the consequence. Adding a new optional location should not make an active quest disappear or alter a critical route.

The design acceptance case uses a campaign with more eligible optional sites than map capacity and an active quest whose preferred site is invalid. The chosen equivalent must be the same for the same seed and state, or any permitted seeded alternative must remain within the authored equivalent set. A second profile with the preferred site valid should select it while still varying optional locations. This demonstrates both reliability and discovery.

### Player-directed selection within guarantees

Where the current expedition flow permits player choice, allow the player to choose among valid optional candidates after mandatory routes are reserved. Do not expose internal rarity weights; present a short description of known purpose and risk. A player may select a repair stop, a shelter conversation, or an unexplored lead, while the system fills remaining space deterministically. If no player choice exists today, this remains a future UX option and does not justify changing the picker.

A player-directed choice must preserve the same fallback contract. If the chosen site becomes invalid before departure, explain the change and offer an equivalent or return the slot to the player. Never mark a choice as confirmed and then substitute silently. This keeps the expedition map a negotiated plan rather than a hidden random outcome.

### Expedition theme without forced uniformity

A thematic expedition should shape the candidate mix without making every selected place repeat the same mechanic. If the theme is damaged communication, one site can expose a physical relay fault, another can show how a paper message was copied, and a third can offer a human consequence. Add a survival or resource location only when it fits the map and pacing. The theme is a compositional hint, not an exclusion filter for all unrelated exploration.

The player should still encounter contrast. A quiet shelter alongside a risky span can give the expedition room to breathe; an ordinary supply site can make the route economically useful. If every map slot is devoted to the current quest, the expedition becomes a level-select menu. If no slot reflects the active story, the campaign feels indifferent. The selection review should therefore record thematic coverage and gameplay variety alongside quest guarantees.

Do not solve variety by allowing a selected location to spawn arbitrary encounters unrelated to its authored identity. Use the existing encounter and location owners. A location can support several authored interaction variants, but each must satisfy the same physical and continuity constraints. This keeps the map recognizable while making repeated trips meaningfully different.

## Continuation pass 14 — Empty Shift location circuit and expedition availability recipes

### Region composition

The Empty Shift uses a compact geography that lets the player compare a written roster with physical work evidence. The locations below are provisional content candidates. They should be reconciled with current canonical map and location catalogs before any IDs are proposed. They are not claims that these rooms exist in the game or that new location-selection pools are implemented.

The route is deliberately not a linear sequence of all sites. The player can begin with the work board, investigate the annex, speak to a witness, or follow a trace toward an alternate maintenance niche. The expedition picker should preserve a valid route for active critical objectives while leaving optional context subject to normal selection. A single expedition should not be required to contain every source of evidence.

### Candidate locations

**The Sleeve Board.** A sheltered notice wall where current work assignments are kept in a cloth sleeve to reduce moisture damage. It has strong introductory visibility because a character can point to it. The location supports roster inspection, correction, and a return callback. Its environmental history is shown through pinholes, replaced sheets, and a strip of clean wood where an older board was removed. If the board is inside an existing shelter hub, it may be an interaction within that location rather than a new map site.

**The Heat-Service Annex.** A small room meant for routine inspection of shelter heating equipment. The player’s first visit may show it locked or temporarily inaccessible. A locked door is not an objective failure; the exterior can still provide useful evidence. Later access may depend on a key, an escort, weather, or a character’s shift. If the game does not model interior entry as a separate location, all interactions can occur through the exterior service panel.

**The Dry Shelf Room.** A storage nook where older rosters and work notes are kept above the floor. It is mechanically distinct because paper handling, damage, and provenance matter here, while it offers few immediate survival resources. It should remain optional unless a quest explicitly reserves it. If selected, it can hold a side quest or character memory rather than become a compulsory archive system.

**The Lower Service Niche.** A narrow space adjacent to a pipe run, reachable through an alternate route. It may explain how work could have occurred without anyone entering the annex. The clue must not imply a hidden tunnel unless the physical map supports one. The location’s purpose is to give the player an alternative interpretation, not to add a general stealth route.

**The Repair Bench.** A visible work surface where tools are checked and returned. It may be an existing shelter interaction rather than a separate location. A worn tool, a loose fastener, or a missing mark can point toward work, but none proves who used the tool. The bench supports a short inspection and can reveal a practical repair job.

**The West Intake Walk.** An exposed route between shelter spaces that the player may cross during a safe escort or supply run. It introduces weather and timing costs without being an arbitrary danger gauntlet. A task here should have a shelter alternative when severe conditions make travel unreasonable.

**The Cook Room Passage.** A transition point where a meal roster and work roster overlap. Its distinct narrative use is to show how recorded shifts affect people’s daily routines. It must not imply that cooking or resource allocation data is controlled by the location picker. The area can be a short scene attached to an existing kitchen or communal room.

**The Empty Cot Bay.** A quiet row of sleeping spaces near the night shift area. Its emptiness is not evidence that a worker disappeared; the roster may not correspond to cot assignment. The player can find a repaired blanket, a folded work coat, or no trace at all. This site helps writers resist the false inference that every discrepancy hides an intimate tragedy.

### Availability contracts

Each location candidate receives an availability recipe with four elements: prerequisites, preferred window, exclusions, and fallback.

- The Sleeve Board is available whenever the relevant shelter is reachable. If the player has already inspected it, the return interaction changes to the current roster.
- The Heat-Service Annex appears as a named map point only when the player has plausible knowledge of it. If its interior is inaccessible, the outer panel remains a valid site for at least one objective.
- The Dry Shelf Room is optional and can be discovered through Mara, a paper handling clue, or a returning character. It must not be the only source of a main quest fact.
- The Lower Service Niche is eligible only after the player has a clue that narrows the route. It can be represented as an approximate search area until discovered.
- The Repair Bench is available as a hub interaction, not necessarily a map candidate. If no current host route supports a distinct interaction, fold its evidence into the annex or board.
- The West Intake Walk can be excluded under severe weather or injury pressure; when excluded, use a shelter conversation or written handoff.
- The Cook Room Passage is included only if the active expedition or shelter visit already exposes the kitchen. Do not force a location solely to present one line.
- The Empty Cot Bay is a secret or optional context site. Its absence never blocks the evidence chain.

These are conceptual contracts. Their condition names must be mapped to the live content and expedition owner. If a weather, injury, or shelter access fact does not exist in the current authority, the author must not invent it in JSON.

### Expedition recipes

**Evidence-focused dispatch.** Reserve the Annex or an authored equivalent and one independent source. Add a thematic site with a different narrative function, such as the Repair Bench or the West Intake Walk. Leave Dry Shelf and Cot Bay as optional discoveries. This mix provides enough proof opportunity without turning the expedition into a roster tour.

**Character-focused dispatch.** If Mara’s quest is active, include a viable meeting location or a note from her. The player can then investigate one physical location and retain an open optional slot. Do not select every site carrying the character’s name. Character presence is a content condition, not an excuse to overfill the map.

**Weather-constrained dispatch.** If the exposed walkway is invalid, replace its objective with a dry-shelf clue, a service-panel observation, or an authored report. A hazard exclusion must not remove the only route to progress. If no exact substitute fits, delay the optional escort and show a clear blocked state.

**Pre-discovery dispatch.** A player who has accepted the quest but lacks exact location knowledge receives a broad lead. The picker can select an eligible local site containing a clue that narrows the search. Do not expose every location name at departure. The first map should preserve uncertainty without hiding the presence of a meaningful objective.

**Return dispatch.** After the player chose a roster treatment, the next expedition may include a changed board or a character who can report the handoff. It should not include the whole original site set again unless the player chooses to revisit. The map can select one closure opportunity and use the remaining slots for other gameplay.

### Location exclusivity and concurrency

Some physical states cannot coexist. The annex can be locked or open, a single roster can be on the board or in storage, and one named worker can be present in only one place at a time. These mutually exclusive conditions must be explicit in the existing location/world owners. They should not be implemented as separate copies of one site whose contents drift.

Multiple compatible sites can coexist: an open board, an active repair bench, and a service niche can all appear in one campaign. The quest designer decides whether one visit can satisfy multiple objectives. The selection algorithm only enforces that authored contract; it should not infer that two sites are narratively interchangeable because they share a tag.

When multiple quests want the Annex, co-location is allowed only if the timing and access conditions align. A personal conversation with Oren and a structural inspection may share the visit. A night escort and an unrelated daytime meeting may conflict. If they cannot coexist, choose the main quest’s required route and delay the other with an explanation. Avoid generating two map pins for one physical room.

### Map states and discovery wording

A map point can be confirmed, approximate, discovered-but-closed, or unknown. The player’s knowledge should dictate the label:

- “Heat-service annex” if a named source provided that name.
- “Locked utility room” if the player has only seen it from the outside.
- “Possible work niche” if an uncertain clue suggests a location.
- No marker when the player has not received a meaningful lead.

A closed location remains on the map if the player knows where it is and has a reason to return. The UI can show a lock, a weather barrier, or a short text note, following existing visual conventions. A site that is only temporarily blocked should not disappear and reappear without explanation. If the map system has no separate approximate marker, use a quest note and avoid new icon semantics.

Discovery can precede acceptance. A player may find the roster and later accept Mara’s request. Quest start logic must recognize that the board was already inspected and should not ask the player to repeat the interaction. Conversely, accepting first should not reveal evidence that the player has not seen merely because the site is guaranteed to be selected.

### Failure and fallback by location

If the Annex cannot spawn, select a supported exterior service point or a witness route. If the Shelf Room is unavailable, Mara can summarize its role without giving the hidden character note. If the Lower Service Niche is removed from the final layout, change the mystery’s proof to an available exterior trace rather than rename another location as the niche. If the Repair Bench is only a hub prop, treat it as a local interaction. If severe weather blocks the Intake Walk, use a shelter-based escort or a report. If the Cot Bay is cut, remove only the optional personal context.

This explicit granularity helps production cut content safely. A required objective’s fallback preserves its narrative purpose. An optional site’s removal drops its bonus context without leaving dangling dialogue or map references. A generated expedition that has no candidate after all exclusions should proceed with fewer optional sites or surface the quest delay; it must not invent a replacement room.

### Production review

Before the location portfolio is integrated, map every proposed room to the existing location catalog and Godot scene or interaction surface. Determine which are new map sites, which are sub-interactions, and which are only prose. Check whether each selected site has an actual quest or gameplay consumer. Ensure that repeat visits reuse one location authority and update only through its established state path. Review sightlines, interaction focus, controller access, short map labels, long journal wording, and localization cost.

The minimum viable location slice uses the Sleeve Board, one Annex interaction, and one alternative evidence source. The first location should be available in a shelter hub; the Annex may be an expedition point only if the current map supports it. The third clue can be a character account. This keeps production small while proving that the player can investigate the discrepancy without visiting every candidate place.


### Selection pass: full expedition simulation cases

The location-selection layer must do more than pick a list of sites. It has to evaluate a campaign profile, protect quest reachability, use available map capacity, honor location state, preserve discovery rules, and explain outcomes at departure. The following cases deepen the Empty Shift route and act as a design rehearsal. They are not assertions about current runtime behavior.

#### Simulation 1 — Main quest accepted, all locations available

The player has accepted The Empty Shift and has no required skill. The preferred Annex interaction is valid; the Sleeve Board is in the accessible shelter hub; the Repair Bench is an ordinary local interaction; several optional sites qualify.

The selection trace begins by classifying the board as already accessible through the hub, so it does not spend a map slot. The Annex is reserved as the only hard-required destination for the next objective. A second evidence source is not forced onto this expedition if the player can return later. The picker selects one thematic optional location, such as the Shelf Room, only if the map has capacity. It then fills open positions with normal exploration candidates. The departure screen says the Annex has a service-panel inspection and that older shift notes may be available near storage, without revealing the optional hidden name clue.

Expected outcome: the quest remains possible; the map does not duplicate the shelter board; optional discovery can vary by seed; the roster’s meaning remains unknown until the player interacts.

#### Simulation 2 — Preferred Annex unavailable

The Annex is inaccessible because a current location condition blocks entry, but the outer panel can still be inspected. If that is a valid objective source, reserve the exterior interaction and mark the interior as closed. If the panel is also unavailable, use the alternate service niche only after the player has a clue. If neither is valid, delay the physical observation and offer a witness or log route.

The selection trace records each fallback attempt and its reason. The map says “utility room closed; exterior inspection available” or “site unavailable; another maintenance account can be sought.” It never silently substitutes the Shelf Room and claims that the player inspected the Annex.

Expected outcome: prior quest evidence remains; there is a visible next step; no invalid site is pinned as actionable.

#### Simulation 3 — Many active side quests compete

The player has accepted the main quest, the glove task, the escort, and the meal count. The map has fewer slots than preferred locations. The main quest’s required route is reserved. The escort can use an already-selected route if timeline-compatible; otherwise it becomes an optional choice at departure or waits for a later trip. The meal count occurs at the shelter hub and consumes no expedition slot. The glove task can be completed at the Repair Bench, which is available without adding a new site. Any remaining capacity supports one optional discovery.

The player is told that the escort window and main inspection cannot both be completed on this dispatch if that is a genuine constraint. They can choose which to prioritize. The journal does not automatically expire the other quest.

Expected outcome: the map communicates genuine tradeoffs while preserving progress and avoiding arbitrary random loss.

#### Simulation 4 — New game, no knowledge of the Annex

The player can accept the request based on the work board but does not yet know the Annex’s exact name or location. The first expedition selects a search area or a clue-producing site only if the fiction establishes how the player learned about it. A character can point to the utility row without revealing an exact building. After inspection, the location owner records discovery and later briefings can use the name.

Expected outcome: acceptance does not leak map knowledge; required progress remains visible at an appropriate level; discovery is not repeated.

#### Simulation 5 — Return after a report treatment

The player chose to annotate the roster. On the next expedition, the selection layer checks whether the board state actually changed. If the board has a supported persistent owner, show the revised copy or a related response. If only a quest outcome was recorded, the callback can happen in dialogue and the map need not claim a new board prop. The Annex may remain available as a revisitable location if its physical state is unchanged, but it should not be selected as mandatory again.

Expected outcome: return content is caused by the outcome and not by a false location reset.

#### Simulation 6 — Character leaves before evidence is complete

Oren has departed, but his testimony was not required for the objective. The selection layer does not force a location that exists only to stage his conversation. An authored note or Nessa’s report can provide the minimum information; the main quest records that Oren’s direct account was not obtained. Optional relationship context is lost, not critical progression.

Expected outcome: no empty map pin and no invisible dependency on a missing survivor.

#### Simulation 7 — Severe travel conditions

The exposed Intake Walk is excluded by a current weather or hazard owner. The player can choose the indoor route or defer the escort. The main investigation still has a valid site. The selection system must not disregard a safety exclusion just because a side quest prefers the location. If no environment authority supports the hazard, treat weather as authored context rather than a real exclusion.

Expected outcome: physical constraints produce a legible delay, not a dead quest.

#### Simulation 8 — Secret clue discovered before the main quest

The player previously found the old shelf and a folded name impression. The main quest later starts. The selection owner recognizes the existing discovery state and does not draw the Shelf Room solely to repeat that interaction. The quest can offer a new inspection or conversation that builds on the clue. If no such follow-up is authored, the secret stays optional and does not appear in the main quest’s objective chain.

Expected outcome: early exploration is respected; a hidden discovery is not replayed as new.

#### Simulation 9 — Temporary worker’s departure

A timed escort is available while a temporary worker is present. If the player is away, an authored departure event may close the escort opportunity. The selection system does not keep spawning the worker’s destination after they leave. It can instead select a written handoff or a later report. Expiration is visible in the quest state and does not erase the main investigation.

Expected outcome: expired temporary content is gone for a reason, with no dangling destination.

#### Simulation 10 — No valid locations

Every route-specific candidate is excluded: Annex and niche unavailable, witnesses absent, and the expedition map at capacity. The picker must not invent an unreviewed location. If ordinary gameplay can proceed without quest advancement, it selects valid optional sites and marks the investigation delayed with the known reason. If the campaign cannot proceed, the situation is an integrity defect with a safe recovery response in the current owner.

Expected outcome: no crash, silent quest deletion, or fake substitution. A development trace contains enough facts for an author to repair the missing fallback.

### Candidate scoring without hidden complexity

The picker should use existing weighted selection and deterministic RNG rather than layering a second score system on top. If the current owner ranks candidates, the design order is: satisfy active mandatory content; protect critical progression; honor explicit player choice; include at most the useful number of character/faction opportunities; account for newly discovered routes; match the current expedition theme; fill remaining slots with optional variety; then consider rare surprise locations.

The weighting should apply only among valid candidates at the same practical priority. A rare site cannot outrank a mandatory objective. A recently discovered site should receive a modest chance of return, not monopolize every dispatch. Theme match should increase eligibility or priority without erasing unrelated gameplay. The exact weights require balance evidence and belong to the picker owner. This plan does not prescribe numeric values.

Tie-breaking must be stable. Candidate IDs or another existing stable order should be used before deterministic draws. Adding a new optional location should not consume random draws in a way that reshuffles every unrelated candidate unless that behavior is intended and replay-tested. If the current design deliberately changes draw order as catalogs grow, document it and assess migration/replay expectations before adding content.

### Weighted pools and fairness constraints

The pool vocabulary can be implemented as tags or derived sets, but only one candidate record may represent a physical location. Candidate categories are not independent copies. A site can be optional and faction-specific, or thematic and recently discovered. The selector should deduplicate before filling slots.

Fairness is measured in outcomes:

- Every active mandatory quest has a valid route or a truthful delayed state.
- A single optional theme cannot exclude all ordinary survival locations.
- Secret content remains undisclosed until player knowledge permits it.
- The same unique location cannot be selected twice under separate pool tags.
- A quest equivalent preserves the objective’s evidence meaning.
- Random selection cannot grant a branch choice the player has not encountered.
- Exclusion and low draw probability are diagnosable separately.
- Player choice is respected when offered, unless a real condition changes.

When several hard-required candidates are mutually exclusive, the content author must define which active objective wins or which clue can be shared. The selector should not guess narrative priority from a numeric score. The player can be offered a choice where both tasks are feasible but cannot fit in one trip.

### Selection trace schema for review

A transient diagnostic row can be described as:

- expedition seed and profile;
- region and capacity;
- active mandatory objective keys;
- eligible candidates and category reasons;
- invalid candidates with exclusion reason;
- candidates reserved to guarantee progress;
- chosen substitutes and equivalence reason;
- optional draw order;
- final map visibility state;
- any quest delayed and its fallback;
- whether a candidate was chosen by player action or seeded draw.

The row is useful only if emitted from the current selector. A hand-maintained log is not reliable evidence. It should avoid hidden narrative content in player logs and follow current privacy/log hygiene rules. An automated integrity report can compare candidates with references if an existing validator can be extended under an approved package.

### Accessibility and UX behavior

The departure screen should distinguish “must visit now,” “can visit,” and “known lead” with text in addition to icon. Screen readers and controller focus should move through sites in a stable order. If a route is unavailable, announce the reason in concise wording. Do not rely on red/green color to indicate mandatory state. If the map uses a limited viewport, the journal must still expose the quest fallback and any player-selected priority.

Location descriptions should mention only known hazards and intended actions. A mandatory quest location cannot be visually obscured as a secret if the player already accepted a clear objective. A secret site cannot be revealed through an empty slot count, rare icon silhouette, or changed location list before discovery. Map behavior should match dialogue: a character cannot say “meet me at the shelf room” while the map hides the site as unknown unless the game intentionally asks the player to navigate by clue.

### Implementation dependency order

1. Inventory the current location candidate owner and map assembly route.
2. Verify how active quests expose location requirements today.
3. Determine whether substitutions or delayed objectives are currently represented.
4. Map the proposed location roles to existing catalog records and sub-location interactions.
5. Author one hard-required candidate, one equivalent, and one optional site.
6. Verify current seeded RNG and stable candidate ordering.
7. Review map visibility and departure UX.
8. Add focused content validation or runtime checks only under an approved work package.
9. Expand remaining location candidates after the first slice succeeds.

This order avoids building a generalized pool abstraction before a real content case proves it necessary. It also keeps the team from editing the shared selector without a live path claim.


### Route topology and reachable-location graph

A candidate map is more than a bag of location cards. Routes imply travel cost, safety, and narrative order. The Empty Shift circuit should be authored as a small connectivity graph so the picker cannot select places that are individually valid but mutually unreachable in one expedition. The graph is conceptual until matched to the current travel system; no new route authority is proposed.

Nodes represent the shelter hub, Annex exterior, Shelf Room, Service Niche, Repair Bench, West Intake Walk, Cook Passage, and optional Cot Bay. Edges describe whether a player can travel directly, needs a clue, needs a character to accompany them, or must wait for a safer condition. The edge is not an extra resource cost unless an existing travel owner can represent that cost. A site reached through a search area can have uncertain coordinates; after discovery, the graph exposes the exact route only if the current map supports it.

The base topology has a strong hub connection: the Sleeve Board and Cook Passage are close enough to appear as shelter interactions; the Annex exterior is one plausible expedition destination; and the Shelf Room can be an interior interaction or a secondary location. The Service Niche is reached from an exterior clue. The West Intake Walk connects a shelter edge to a remote maintenance point. The optional Cot Bay is in a shelter space and does not consume a distinct travel route if it is simply a scene. Production must verify geography; a plausible story does not establish a valid map coordinate.

**Reachability rule.** Every active quest path must contain at least one reachable next interaction, or the journal must show a truthful blocked/delayed status. A site is not reachable merely because it is in the candidate catalog. It must fit the current region, map capacity, travel constraints, character availability, and expedition type. If a route edge is conditional, its condition is explicit and has a fallback.

**Travel cost and exposure.** If travel time or exposure is an existing mechanic, candidate selection can favor nearby sites when the player is injured or weather is severe. The selector must use current state rather than reproduce a local risk score. If no such travel owner exists, keep the graph descriptive and do not attach numeric costs.

### Route composition recipes

**Short shelter loop.** Board → Cook Passage → Repair Bench. This is a low-risk visit suitable for the initial quest offer and resource side task. It should not require an expedition transition if the game’s shelter hub already hosts these interactions.

**Evidence loop.** Annex exterior → Service Niche or Shelf Room → return to Board. This requires at least one physical clue plus a report. The second location is optional if the player can use a different witness route.

**Safe handoff loop.** Shelter hub → covered route if known → destination board. If the only route is the exposed Walk, the player can delay, request help, or accept the risk with adequate warning. The location selector cannot secretly route the player through a hazard.

**Discovery loop.** Approximate utility row → clue interaction → exact Annex or Niche discovery. The first expedition can reveal a region rather than the location’s display name. Once a site is known, repeat visits are ordinary revisits, not rediscoveries.

**Return loop.** Board outcome → next milestone location or character → closure. The earlier Annex should not be forced into the map again unless the result itself needs a physical inspection. A later callback may happen in the hub, preserving expedition capacity.

**Failure-forward loop.** Closed Annex → alternative maintenance note → unresolved/partial conclusion. The fallback remains within the same investigative question. It must not turn into a random unrelated supply run.

A route recipe states whether the locations must appear together, may be split across expeditions, or can be substituted. The quest portfolio should avoid requiring a three-stop route on a map that can only hold two locations. If one site is a shelter interaction, state that it consumes no map slot only after the current architecture confirms this.

### Dynamic accessibility without location duplication

A physical place can have changing access states: open, closed, hazardous, repurposed, or known only approximately. Represent these as states or conditions on one canonical location if that is supported. Do not create a different location record for the open Annex and locked Annex unless the current data architecture expressly uses authored variants as independent records. The player’s discovery belongs to one identity; visual and interaction details vary around it.

A place may have one permanent identity with several entry methods. The player can inspect the door from outside, enter the room after obtaining access, or find a maintenance note at another location. These are different interactions and evidence classes, not new location identities. The map can present a single pin with a short access note.

If a location changes role permanently, such as a storage room becoming a clinic, the world owner should define whether the identity remains or a new site is created. The dialogue and quest records should refer to the current canonical identity. Plans do not determine canonical location IDs.

### Cross-quest occupancy constraints

When characters occupy locations, selection must respect one-person/one-place-at-a-time constraints only if the current character simulation owns that information. The content design can declare that Mara cannot simultaneously be at the board and the Annex during the same scene window. It should not create a shadow occupancy map.

Two quests may use the same location at different times. If the current system cannot represent time-of-day, schedule the scenes across quest milestones rather than simulate clock-based occupancy. If it can, use its current event contract. An absent worker’s note may provide a fallback, but it must not create an additional copy of the same character in a generated instance.

A temporary scene location can appear only when its character or event exists. If the world state cannot guarantee this, the scene should use an existing hub. Avoid dangling pins that point to no interaction.

### Revisit and map memory

A revisitable location can have multiple interaction phases: first discovery; evidence acquired; route changed; task completed; optional return. These phases should be derived from the current location/quest owner. Do not reset the site at every expedition to make it appear fresh. A quest’s discovery proof should be satisfied once; repeat visits can supply new optional details if authored.

The map must preserve knowledge while accurately updating access. A player who discovered the Annex should not lose its name when the site is temporarily closed. The marker can show the closure, and the journal can identify an alternate source. An unresolved clue remains approximate until the player narrows it. Discovery and availability are separate concepts.

A location can be known but not currently selected. The UI should not erase it from the world map unless existing design hides off-expedition points. If only expedition candidates are shown, the player-facing policy needs to state that the departure map is a route plan, not a total world map. Repeated selection of an optional site can be reduced by the current picker’s cooldown or weight logic, if one exists. Do not remove a location because it was selected once unless the content is one-time.

### High-risk and special pools

**Quest-required candidates** are guaranteed or have a fallback. They are not necessarily visible as exact map pins until the player knows the location.

**Progression-critical candidates** exist only when campaign flow otherwise cannot continue. They receive a stronger guarantee and validation than optional jobs.

**Character/faction candidates** can support a conversation, but missing a character does not automatically eliminate the site if environmental evidence is valid.

**Recently discovered candidates** gain a chance of a follow-up visit where useful. The selector should avoid showing the same optional clue in back-to-back expeditions unless continuity or player choice warrants it.

**Thematic candidates** increase coherence but should preserve exploration contrast and survival utility.

**Optional candidates** fill remaining slots and maintain the game’s ordinary expedition loop.

**Secret candidates** are gated by knowledge and should not leak through count, label, or hidden marker. Their rarity is subordinate to eligibility.

**Temporary candidates** have clear expiration and an after-state. When expired, the game shows what happened or closes the opportunity.

A single candidate may have several tags. Selection deduplicates by canonical location identity. The current owner decides whether category tags are data or derived logic; a content author must not maintain two mutable pool lists.

### Selection order under player choice

If a player chooses locations, present choices after mandatory reservations. Show one or more valid optional choices with their known purpose and risks. The player can prioritize the evidence site, the safe route, or a resource stop. If there is a meaningful conflict, explain which task waits. The choice is not a hidden probability boost. The system may then fill remaining capacity with appropriate optional candidates.

If a player-selected site becomes unavailable before departure, tell them why and offer a replacement from its authored equivalence group or return control to them. If the site becomes unavailable after departure, follow the current expedition policy: either the plan is fixed and the player encounters a truthful obstruction, or the route can be revised. Never quietly substitute another location after the player committed gear and companions.

### Location selection balance data

After implementation is authorized, gather selection traces across a bounded set of representative profiles. Track mandatory coverage rate, fallback rate, optional variety, repeated-site rate, secret-site leak rate, expired-quest pin rate, and map capacity pressure. These metrics help determine whether the candidate set is too narrow or the selection weight is skewed. Do not add telemetry without the project’s existing instrumentation and privacy policy.

A content-facing review can use seeded fixture maps rather than a large simulation. Include: all candidates eligible; hard candidate unavailable; every optional candidate invalid; two quests share a site; two quests conflict; secret site not discovered; secret site discovered; temporary location expired; and player chooses a valid optional location. The intended trace is written down before testing.

### Scene and asset production budget

A new map node costs more than a location description. Estimate terrain or background art, interaction hotspots, pathfinding/travel links, map label, environmental audio, localization, quest hooks, revisitable state, and accessibility focus. A sub-location interaction may be cheap when it reuses a hub scene. Choose that approach if the player benefit is equivalent.

The Annex can be conveyed with a single exterior interaction and a service panel. Do not commission an interior room unless gameplay requires entry. The Shelf Room can be a short scene at an existing storage area. The Service Niche should be added only if the visual route clue and alternate interpretation justify its production. The optional Cot Bay can be a dialogue backdrop and need not be a new expedition location.


### Location availability by quest type

A location is selected because an authored player action needs it, not merely because the quest uses its name. This matrix gives content authors a shared way to describe availability without forcing every quest into the same expedition pattern.

| Quest type | Location relationship | Map behavior | If unavailable |
|---|---|---|---|
| Main quest | Hard required only when no other proof route exists | Confirmed marker if known; otherwise honest lead | Equivalent evidence or visible delay |
| Character quest | Preferred meeting place, often replaceable by note or hub | Pin only when the character is expected there | Alternate conversation surface or optional miss |
| Faction quest | Candidate location tied to current faction access | Access note reflects known standing/authorization | Another contact route or delay |
| Discovery quest | Hidden location is the objective | Approximate clue before discovery; exact marker after | Discovery remains optional or another clue |
| Investigation quest | One or more evidence sites | Show actionable known sites; do not expose hidden source | Failure-forward evidence path |
| Escort/protection | Route endpoints and connected path | Show destination and known hazard | Alternate route, safe wait, or closure |
| Survival quest | Place that materially changes survival decision | Hazard and preparation context | Another survival action or defer |
| Resource/crafting | Workbench, source, or delivery endpoint | Mark only if travel is needed | Existing recipe/material alternative |
| Location-based | Site interaction is central | Required/optional status visible | Authored equivalent or quest delay |
| Timed quest | Location with a real expiry cause | Show urgency and expected window | After-state is explicit |
| Repeatable/rotating | Locations vary by authored job | Avoid repeating last optional site without cause | Skip one rotation; do not create fake site |
| Hidden quest | Gated place or clue-bearing subarea | No exact pin until discovered | Never blocks main progress |
| Environmental start | Object or place is the start event | Clue is discoverable by ordinary play | Alternate entry only if authored |
| Choice-reactive | Location reflects prior outcome | State/label matches previous choice | Use dialogue/notice if place cannot change |
| Failure-forward | Substitute evidence location | Explain why it substitutes | Unresolved closure if every route is lost |

The matrix does not require every quest type to create a new map marker. A hub conversation or paper interaction may be enough. A physical location can support multiple quest types if time, character presence, and world state are compatible.

### Location lifecycle independent of quest lifecycle

A location can be unknown, discovered, accessible, visited, changed, blocked, or retired. A quest can be active or resolved. These are related but independent dimensions. The Annex can remain discovered after the main quest resolves. A quest can be blocked even though its location is accessible because the required witness is absent. The map should not derive location identity directly from quest status.

This separation matters for generated expeditions. The selector may choose a discovered location for ordinary exploration after its quest is complete. It may choose an undiscovered optional candidate only if the player has a legitimate discovery clue. Quest resolution should not automatically remove the site from the world or alter its physical state.

If current architecture combines location availability and quest markers, inspect the actual implications before proposing changes. A minimal UI may still work if its rules are clear. The plan should not demand distinct persisted states when existing content can represent the necessary behavior.

### Location-only and quest-only content

Some places exist primarily because a quest needs them. Others are ordinary locations with one quest interaction. Treat these cases differently during production review.

A **quest-only location** is justified when its geography, route, and repeated visit create gameplay that cannot be achieved through a scene at an existing site. It needs enough content to feel physical: arrival reason, interaction, exit, potential return, and a fallback when unavailable. If it has no gameplay after its quest, closure should remove or repurpose it honestly.

A **location with a quest** remains in the ordinary exploration pool and supports its normal resources or hazards. The quest adds an interaction rather than making the location exist solely for narrative convenience. Its quest state must not reset the site’s normal content.

A **quest-only interaction** is a prop or subarea inside an existing location. It should not consume a map slot or create a new location ID unless the current map model requires it. The Board, Bench, and Cook Passage may fit this category.

A **generated encounter location** is a bounded instance assembled from authored pieces. It cannot claim to be the permanent Annex unless it references that canonical identity. If a generic utility room is generated for optional exploration, its content must not satisfy the Annex objective without an explicit equivalence rule.

### Expedition start and world-state snapshot

At expedition start, determine the context snapshot used for candidate eligibility: active quests, known locations, current route/region, relevant character availability, world conditions, selection capacity, and the random seed through the existing deterministic owner. This is a conceptual list. The current code may gather some of these facts differently.

The chosen set should be internally coherent for the duration the current expedition model defines. If world state changes mid-expedition, follow the established policy: either the map is fixed, or selected locations can update. Avoid recomputing the full map on every UI refresh. Repeated recomputation can cause a pin to move after the player opened the panel and makes seeded behavior difficult to explain.

Persist only the state already required by the current expedition save contract. If the selected list is currently reconstructed from a seed and state, adding a new selection rule can alter older saves. The implementation owner must inspect restore behavior and preserve compatibility or provide an explicit migration. This plan does not declare that selected locations need new save fields.

### Compositional examples

**Main investigation plus ordinary survival.** The required Annex is guaranteed, but the map keeps one slot for food, medicine, or rest according to normal rules. The quest does not monopolize every expedition.

**Character story plus location discovery.** A worker’s note points to the Service Niche. The player may discover it through a map clue or postpone it and still use a witness route. Character content and spatial discovery reinforce each other without requiring both.

**Escort plus timed repair.** The player can either escort the worker with the repair bundle or deliver the bundle separately. The route plan communicates that the repair may wait if no safe crossing exists. The selection engine does not spawn a traveler beyond their authored time window.

**Hidden clue after resolution.** The player finishes the main case, then later returns to the Shelf Room and finds an optional impression. The map selects the location only if it is revisitable and the player has a reason to return. The hidden chain adds context but does not reopen the main investigation.

**Faction-specific site.** An existing faction may use the board as a contact surface, but the location does not become faction-locked unless the existing faction access system supports it. A local character can still discuss the record if faction access is unavailable.

### Selection fairness scorecard

A future location owner can be reviewed on:
- hard quest coverage per valid expedition;
- how often fallback substitutes are used;
- whether optional variety remains across repeated dispatches;
- whether two similar sites repeatedly appear together;
- whether the player can understand why a place is on the map;
- whether hidden sites leak;
- whether player-selected routes are respected;
- whether blocked sites remain accurately represented;
- whether a missing catalog row is caught;
- whether the same seed/profile yields expected reproducibility.

This scorecard is for owner acceptance. It does not require telemetry or a simulation tool. A handful of focused deterministic fixtures may be sufficient.

### Accessibility and map language examples

Map and journal text should differentiate a place’s known identity from its quest role. “Annex — check the outer service panel” is actionable and concise. “Possible service niche — location uncertain” is a lead. “Shelf room closed — Mara’s note may provide another account” identifies a fallback. “Cot Bay — optional record fragment” may disclose too much; instead use an ordinary descriptive label until discovery.

All symbols need a text equivalent. Focus order should match the route plan or a stable map order. A controller user must be able to move between pins without hover-only details. If capacity pressure forces the game to postpone a task, the message must be reachable through the journal and not only as a transient tooltip.

### Acceptance sequence

The first selector integration should prove one mandatory site, one equivalent, one hidden optional site, one expired temporary site, and one ordinary survival location. It should show exclusion reasons in development output and not introduce new canonical pool registries. The next slice can add character and faction candidates after verifying their actual owners. A full location expansion follows only after the candidate set can be audited and the map UX has been reviewed.


### Selection decision table for production

At expedition assembly, content design can present the following ordered questions to the current selector owner. This is a decision aid, not pseudocode for a new service.

1. Which active objectives have a hard location requirement?
2. Does each hard requirement have a valid preferred candidate?
3. If not, which authored equivalent preserves the objective?
4. If no equivalent is available, is there a clue, non-location action, or visible delay?
5. Is any campaign progression site required to avoid a soft lock?
6. Did the player explicitly choose an optional destination?
7. Are character or faction meetings eligible and compatible with the same timeline?
8. Which recently discovered sites add value without repeating the last trip?
9. Which thematic candidates fit the current expedition conditions?
10. How many optional slots remain?
11. Which common and rare candidates can fill those slots without duplication?
12. What does the player know about each selected place?
13. What fallback is shown if a selected route becomes blocked?

The order protects mandatory access first, but it does not automatically promote every side quest. Quest authors should mark whether a location is hard-required, preferred, or optional. These are design labels and should map to current content fields only after schema inspection.

### Compatibility and exclusion examples

Two quest objectives can share the Annex visit if both refer to the same interaction and there is no timing conflict. The main investigation may use the panel scratch while the repair ticket uses the loose fastener. A single visit can complete both if the player performs both actions. Selection should not automatically award both if the player only looked at the door.

The Cook Passage and Sleeve Board can both be present if they are hub interactions. The player can have a meal-count task without forcing the expedition picker to add a kitchen map site. Conversely, an optional Shelf Room should not be selected merely because it shares the “paper” theme if its access condition is not satisfied.

A temporary worker cannot be selected at two locations for the same time window. If the selection data includes character presence, the current character owner must supply it. A quest should not solve the conflict by creating a duplicate worker instance.

### Release review of unavailable destinations

When a location cannot appear, determine whether the cause is content eligibility, map capacity, travel constraints, story timing, or a data defect. The player sees only the explanation they need. “The route is not on this dispatch” is appropriate for capacity; “the site is closed” describes access; “no reliable lead yet” describes knowledge; and “the location record is missing” is a development error, never player-facing copy.

For each mandatory destination, review the full exclusion list. The exception should be based on a real world rule. If every condition can exclude the preferred site, the alternate route is not optional paperwork; it is required content.

### Seed and catalog-change compatibility

Before a content expansion adds candidates, inspect whether selected maps are persisted or reconstructed from seed and state. If old saves store only a seed, adding a candidate could alter the reconstructed map. The implementation plan must decide whether that is acceptable, whether a content version gates the candidate set, or whether the existing save owner already freezes the selection.

Do not invent a location-version field in this plan. Record the observed behavior and use the current migration/save process. A deterministic same-seed result must be defined for the current catalog version, not assumed to remain identical after all future content additions.

### Selector acceptance profiles

The first focused verification package should cover: ordinary map with spare slots; full map with a hard quest site; preferred site excluded but an equivalent available; no equivalent available; hidden site undiscovered; hidden site discovered; one temporary site expired; two compatible quests sharing a location; two incompatible tasks requiring the same character; and a player-selected optional stop. The expected candidate set, map visibility, and quest status are written down before the run.


### Site-equivalence acceptance worksheet

A substitute site must preserve the player’s question, not merely share a tag. For every mandatory location, list: physical fact sought, preferred site, valid equivalent, information lost by substitution, and text explaining the change. The Annex exterior can substitute for the interior only if the objective concerns the panel or access mark. A meal-room conversation cannot substitute for a repair observation, though it may support a separate testimony objective.

The selector’s acceptance review verifies each equivalence pair under valid and invalid conditions. If the substitute provides weaker evidence, the journal says so and the outcome confidence changes only through the quest owner’s supported result. If equivalence is complete, the player should not receive a lower reward solely because the preferred building was unavailable. This prevents world selection from quietly penalizing players for a random map composition.

Keep equivalence groups small and authored. A large generic “maintenance location” pool can return a site that does not support the clue. An explicit pair with an author-written rationale is easier to review and maintain.

### Candidate deduplication and physical identity

Pool membership never creates a second copy of a site. If the Annex is both quest-required and thematic, it appears once in the candidate set and carries both reasons for eligibility. A second authored entry with the same physical identity can produce duplicate map pins, contradictory access states, and separate discovery facts. The current catalog’s identity rules decide how location variants are represented.

Before selecting, deduplicate by the canonical identity the existing owner recognizes. If a generated encounter takes place at that location, it must reference the same identity or be clearly a separate generic instance. Revisit, hidden, faction, and temporary labels are selection reasons, not new places.

When two candidates share an interaction but not a physical site, they can only be treated as equivalents through a quest-authored evidence contract. Similar tags alone do not establish that one can substitute for the other.

### Rare-site budget

A rare location should be rare because its timing, access, or clue is unusual, not because a random roll withholds essential information. Set a small budget for rare and secret candidates per expedition so they cannot crowd out ordinary survival sites. The exact cap belongs to the current map owner and should be tuned against actual capacity. If the game has no rare-site weighting, use authored eligibility rather than introducing a new probability system.

Review several seeds and player histories. A rare site should feel surprising when it appears, but its absence should not make the expedition feel incomplete. Once discovered, it can remain known even when not selected.

### Map-result confirmation

After selection, compare the final map with the quest journal. Every visible destination has a player-known reason; every required action is reachable or explicitly delayed; optional sites remain optional. This final comparison catches mismatches between selector output and presentation before content is treated as complete.

## Pass 15 — Quest-safe expedition destination selection

**Status: PROPOSAL, premise-gated.** This selection policy elaborates the master bible's data-first narrative and destination-bound micro-location lanes. It is a proposal for an extension around the current WastelandMap, Expedition, expedition catalog, location catalog, encounter catalog, and quest owners. It does not authorize a second map graph, location registry, route ledger, or quest-state authority.

### 15.1 Previous-wave collision note

The Empty Shift route circuit, “roster” destinations, and location names from earlier passes are provisional content only. The repository already has a large location corpus, a map graph owner, micro_locations.json with a dedicated loader, and duty-roster locations. Treat every prior candidate as a possible duplicate until IDs and behaviors are compared. Reuse, rename only through the normal migration policy, or drop it. This plan creates no location ID by writing a name in prose.

### 15.2 Selection responsibilities

Separate four questions that are easy to conflate:

1. **Eligibility:** Can the location be reached and is its content valid for this expedition's time, route, party, equipment, and world state?
2. **Reservation:** Does an active required objective need this location or a valid equivalent now?
3. **Selection:** Which eligible optional destinations make this expedition varied and thematically coherent?
4. **Presentation:** Which selected locations are visible at expedition start, hinted, or held as undiscovered map entries?

The current map owner decides topology and the current expedition owner decides expedition execution. The proposed selector should receive read-only snapshots from those owners, return a bounded selection result, and let each owner apply only facts it already owns. A UI panel must not reroll or mutate availability.

### 15.3 Candidate pools

Build candidate views from canonical location definitions, not copies. The pool label is a derived selection role:

| Pool | Inclusion rule | Default treatment |
|---|---|---|
| Permanent authored | Existing location definition and route node are valid. | Baseline candidates; authored identity and history remain stable. |
| Mandatory active quest | A currently active objective explicitly binds this location or an audited equivalent group. | Reserve before optional selection. |
| Critical progression | A required campaign transition needs a location in this expedition window. | Reserve if the owning progression system says the transition is ready. |
| Character or faction | A verified character/faction arc exposes a reachable location. | Prioritize after mandatory and critical candidates; respect access state. |
| Recently discovered | Existing discovery authority reports the node found, but it has not received a fair opportunity in the chosen route context. | Apply a modest recency boost, never override hard eligibility. |
| Thematic | Fits the authored expedition brief, region, route, hazard, or season. | Weighted optional candidate. |
| General optional | Eligible, reachable, non-duplicate content. | Fill the remaining capacity. |
| Secret | Eligible only after a clue, skill, signal, or prior action owned by a current system. | Keep hidden until the discovery rule is met. |
| Temporary | Authored event content with a valid time/world-state window. | Enforce an explicit expiry or replacement route. |

A location may satisfy several pool labels, but it must appear once in the candidate set. Pool membership is metadata for scoring and presentation, not persistent truth. No class may bypass location integrity checks.

### 15.4 Priority, quotas, and deterministic ordering

Recommended priority is lexicographic rather than one opaque giant weight: (a) hard validity, (b) unresolved mandatory objective, (c) critical progression, (d) required character/faction continuity, (e) authored expedition rules, (f) recently discovered relevance, and (g) thematic/optional/rare variety. Within an equal priority tier, use authored weights and the existing seeded RNG contract. A “must always spawn” objective does not compete probabilistically with a rare optional site.

Capacity is an explicit expedition parameter. First reserve distinct mandatory targets. Deduplicate aliases that resolve to the same location ID. If mandatory reservations exceed the capacity, increase capacity only if the current expedition UI and travel path safely support it; otherwise partition the objectives across sequential expeditions or ask the owner to defer non-critical objectives. Never silently drop a mandatory target to keep the map visually tidy.

For optional choice, sort candidates by stable location ID before consuming RNG; use a named stream derived from the expedition instance/seed and selector version. Do not depend on dictionary order, wall-clock time, or the order that JSON files happened to load. The selector should record enough provenance in the expedition result to reproduce the choice. It must not write random candidates into the authored catalog.

Rarity means “less often chosen among valid peers,” not “allowed to starve required progress.” Rare candidates are considered only after the base route is satisfiable. Use rarity bands as design constraints, not promised percentages until a deterministic replay and representative seed sweep measures actual exposure.

### 15.5 Exclusion and de-duplication rules

Reject a candidate when its authored definition is invalid; its canonical map node is missing; an edge is inaccessible under the current route owner; the party cannot satisfy a truly mandatory entry rule; its time window is closed; the location is already occupied in this expedition under a single-visit rule; a mutually exclusive location already won the slot; or its content is explicitly retired/disabled by an accepted authority. A soft condition such as “interesting if the party has a mechanic” is a score, not an exclusion.

Do not exclude a location just because it has appeared recently if it is required, critical, or the only truthful continuation. Do not use a hidden location as the only route to a core objective unless its discovery clue has already been made reliably available. Do not surface mutually exclusive faction interiors as neutral optional destinations when faction access is currently denied.

Prior-expedition history is an input only if its owner already persists it. If no current owner can prove recency, omit that factor rather than adding a selector-only visit ledger.

### 15.6 Map visibility and discovery

Visibility has four proposed presentation categories: visible at expedition start, visible with a clue but not yet reachable, physically reachable but undiscovered, and discovered/revisited. These are UI projections from existing map knowledge, objective bindings, and discovery records. The selector must not own a parallel “known location” boolean.

A required quest destination may be revealed as an explicit objective marker, a legible clue, or an already known node. If its discovery is meant to be part of the challenge, the clue must precede the travel decision and the quest must retain a fallback. Secret sites should not be disclosed by a generic all-destinations map panel. Repeated expeditions can alter the clue phrasing only when the relevant narrative owner reports a meaningful new observation.

The map communicates why a site is present: “required by active request,” “possible lead,” “known route,” or “unconfirmed signal.” Avoid presenting weighted optional candidates as guaranteed discoveries. Color is not the sole signal; use text, icon shape, and focus-readable details. Keyboard/controller focus must stay stable as the map list refreshes.

### 15.7 Required-location fallback ladder

If a required location is invalid, missing, unreachable, or unavailable, process the failure explicitly:

1. Confirm the ID against the authoritative catalog and current validator findings.
2. If the content defines an equivalent route, substitute only an equivalent whose objective semantics, risk, and reward remain truthful.
3. If a physical target is not essential, issue a clue through an existing verified channel and retarget the objective only after the owning quest system accepts the change.
4. If neither path exists, delay the objective with a visible reason and a concrete eligibility reevaluation trigger.
5. If the story becomes impossible, close it with a failure-forward alternative owned by the quest domain.
6. Emit a diagnostic that identifies the missing reference and owning content record; do not silently remove it from the candidate set.

Fallback equivalence is stricter than “same biome.” Compare what evidence is found, what player action is required, travel cost, hazard, party capability, faction access, reward, and branch consequences. A substituted node that gives a different fact is a new branch and should be authored as such.

### 15.8 Mid-winter encounter set

Use the Days 90–180 pacing gap as the theme constraint, not as a blanket day gate. Candidate content categories include: a route made unreliable by weather; a field report whose measurements disagree; an optional salvage micro-location that offers a repair choice; a faction delivery with two valid destinations; a community request that can be answered through a route, a radio clue, or a safe refusal; and a Chronicle-relevant discovery with no exclusive ending key. Existing locations and hazards must be selected only after a catalog comparison.

Each proposed destination card states required/optional/secret, visible-by rule, first and repeat visit copy, entry condition, valid route, hazard owner, quest objective link, invalid-location fallback, map affordance, expected expedition duration, and the player-facing consequence of skipping it.

### 15.9 Minimum slice and bounded performance

Minimum slice: one required target, one equivalent fallback, two optional candidates from different existing content families, one secret candidate gated by a verified clue, and one temporarily eligible destination. The output must stay stable for a fixed seed/world snapshot; a saved expedition cannot reroll on reopening the map. Measure candidate count, selector time, allocation count, invalid-reference diagnostics, and selection fairness before considering optimization. Pre-indexing catalog metadata is acceptable only as an immutable derived lookup, not as a second location authority.

Acceptance evidence: all required quests remain possible; no duplicate locations appear; every result resolves to a canonical map node; optional variation is reproducible; secret visibility is truthful; invalid content yields explicit fallback diagnostics; party and access restrictions are respected; UI labels explain selection; and repeated open/close cannot change the route. Testing targets should be selected later under TEST_POLICY after ownership and API premises are confirmed. This documentation pass runs no tests and claims no implementation.

### 15.10 Selector walkthroughs

The selector should be reviewable as a deterministic decision trace. These walkthroughs use role labels rather than new catalog IDs.

**Walkthrough A — required investigation target is available.** The active quest owner provides one required target. The map owner confirms a valid route. The location definition passes schema/reference checks. The party and expedition type meet hard access requirements. The selector reserves this candidate before optional scoring. It then fills remaining slots from the authored expedition theme and general optional pools. A secret clue can be shown only if its discovery predicate is already true. The result records selected location IDs and selection reasons; it does not change the quest.

**Walkthrough B — required location is absent from the current catalog.** The selector emits a diagnostic tied to the quest definition and missing reference. It asks the quest owner for an authored equivalent set. If an equivalent is available and valid, it reserves that node and reports the substitution. If there is no equivalent, the quest owner can delay the objective if the objective is non-urgent, or surface an existing clue leading to an eligible place. If neither recovery is supported, the quest remains explicitly blocked for design review; no optional site is relabeled as the target.

**Walkthrough C — route exists but is faction-closed.** Map topology alone does not imply access. The faction owner reports an access denial; the candidate is excluded from immediate travel, but its existence may remain known on the map if the player has already discovered it. If the quest requires the site, the content must have an alternate: negotiate, obtain a permission token through an existing owner, take another route, or defer. Do not expose a clickable marker that fails only after party selection.

**Walkthrough D — three optional sites fit, only two slots remain.** All hard requirements are already reserved. The candidates have stable IDs, each passes validity, and thematic weights are applied from the authored expedition brief. Recently discovered status contributes only the agreed soft tie-breaker. A named seeded stream selects two after stable sorting. Re-running with identical inputs produces the same pair; a new expedition seed may produce a different pair. Reopening the same expedition preserves the original pair.

**Walkthrough E — a secret site becomes eligible mid-expedition.** The discovery owner emits the clue/knowledge fact. The expedition can add the site only if the current travel/map contract permits dynamic additions; otherwise it queues visibility for the next expedition and tells the player why. The selector must not add the location behind the player's back or invalidate the selected route.

**Walkthrough F — all optional candidates fail validation.** The required sites remain intact. The expedition starts with a smaller optional set or a safe empty optional category, with a diagnostic for each invalid record. Do not fabricate a location, repeat a unique site contrary to its contract, or fail expedition startup when only optional content is malformed.

### 15.11 Rarity and anti-starvation controls

A rarity setting must distinguish three concepts: authored narrative rarity, selector weight, and observed run frequency. A rare story can have an ordinary selection weight but a narrow authored condition; a frequent site can have a low weight when few peers exist. Avoid hand-tuning a single percentage without counting eligible candidates.

Before balancing, produce a deterministic simulation report from a representative range of seeds and snapshots. Report eligibility counts, selections per ID, required-location success, duplicate count, secret reveal count, empty optional slots, and rare-location wait distribution. Do not compare raw appearances when candidates were eligible for different numbers of seeds. A location that is eligible only for one weather/region combination should be measured within that eligible subset.

Use explicit anti-starvation only for optional content whose repeat opportunity is a real design goal. A recency adjustment can gently promote content that has not appeared, but it cannot override quest priorities, valid access, authored exclusions, or the player's last result. If no current visit history is available, do not invent it. If a recency ledger is later approved, it needs one owner, deterministic decay/order, capture/restore, bounded size, and migration; it should not be embedded in the UI or quest record.

### 15.12 Pool contract record

For every proposed pool, maintain a review table with: owner catalog, selection tag, hard/soft classification, eligible expedition types, region and route constraints, party capability requirements, faction access rule, weather/season window, quest binding, map visibility, discovery source, expected number of slots, rarity policy, conflict group, repeat rule, fallback group, and validator. Empty cells are unresolved questions, not wildcards.

Pool membership should not be physically duplicated into several catalog files. Prefer the current location/encounter schema if it already supports metadata; otherwise stage an extension proposal for the one catalog that owns the property. The same destination can carry optional-story and faction-story utility, but its canonical identity remains one location ID and its physical adjacency remains in the map owner.

### 15.13 Selector result and replay envelope

A result needs enough explanation for debugging and player-facing feedback, but it should not become a second save authority. The conceptual result includes expedition instance key, seed/content version, selected stable location IDs, required reservations, reason codes, substitutions, excluded candidate diagnostics, visible/hinted/hidden presentation, and a checksum or stable comparison string if the current replay infrastructure supports one. Store it through the existing expedition owner only if a selected route is committed and must survive load. Otherwise recompute from the same inputs and prove that recomputation is byte-stable.

When content changes between releases, old saves need defined behavior. A retired optional location can be removed from the next expedition candidate set. A selected-but-not-visited destination needs a compatibility policy: preserve the committed target if its data remains available, migrate to an explicitly equivalent target, or close with an owner-approved fallback. Do not silently bind an old quest to a new location with different evidence.

### 15.14 Map-facing prose packet

Use three short text fields for a destination card: what is known, why it is relevant now, and what remains uncertain. The site title and region name come from canonical data. A required card can say “Requested by an active field report” only if that request exists. A hint can say “The line points east of the old conduit” only if the clue literally establishes that direction. A secret card must not reveal the site's identity before discovery. An inaccessible destination states whether the issue is route, weather, access, or party capability and points to an available next action.

A map can show optional locations at expedition start without implying that the player has discovered their interiors. Keep the legend readable at high zoom and in screen-reader order. If multiple locations share an icon, a text qualifier distinguishes their route, faction, or quest role.

### 15.15 Handoff and integration gate

The selector is ready for implementation planning only after a source-to-source map identifies the current map graph, expedition start command, active quest inputs, location loader, micro-location encounter loader, discovery owner, visibility read model, deterministic RNG adapter, save owner, and UI map route. Then claim exact paths through WORKTREE_OWNERSHIP and sequence work through INTEGRATION_PLANS. The first implementation should handle one required quest target and one fallback; optional rarity and temporary sites follow only after that path is observable. This proposal changes no source, data, claims, or tests.


## Pass 16 — Bible-seeded destinations, evidence classes, and safe selection

The master world bible suggests a seasonal numbers-station arc, a hydrophone-coast mystery, and folklore whose meaning changes as a shelter community grows. This addition specifies how those story threads should use expedition selection without claiming that every narrative record already has a map node. It is a selection contract for a future premise-checked integration.

### A location is not the same thing as evidence

A cipher record, hydrophone observation, discovery-manifest row, destination, map-visible marker, and quest-required site are different entities. The source review confirms existing cipher target-location reveal behavior for authored chains. It also confirms hydrophone records and discovery references, but a record does not prove a corresponding expedition destination. Folklore entries have discovery consumers, while a cohort-age callback into the map is not established. The integrator must resolve these references against the active destination and location catalogs before selection weights are assigned.

Maintain an explicit mapping proposal with four outcomes: existing dispatchable site; existing non-dispatchable site that can host a shelter-only scene; record that only provides a clue; or unresolved entry deferred from the expedition pool. Never treat an unresolved string as a required location.

### Selection stages

1. **Build an eligibility snapshot.** From the canonical quest and world state, collect only active requirements with a verified destination binding. Keep clue-only tasks and shelter conversations out of the destination reservation list.
2. **Reserve mandatory destinations.** Add verified active quest targets and critical progression locations first. De-duplicate by canonical destination identity, not by display text or aliases.
3. **Reserve faction or character sites.** Add only sites whose access gates are currently satisfiable. A secret faction destination may remain hidden while still reserved if its quest is active and the route can reveal it.
4. **Apply campaign recency limits.** Keep recently visited locations from crowding out the required route unless they are the only valid return point. A quest-required revisit wins over a generic recency exclusion.
5. **Fill thematic slots.** Match season, weather, current region, and authored expedition tone using a deterministic score/tie-break derived from the existing seeded RNG contract. Do not use wall-clock time or collection iteration order.
6. **Fill optional and rare slots.** Roll optional, secret, and temporary candidates only after mandatory and progression reservations. Rarity may change presentation and slot probability, never the chance that a required active quest remains possible.
7. **Publish a visibility state.** Required known sites appear with a clear reason. Unrevealed sites can be represented by a clue or signal rather than a misleading map pin. Optional destinations can remain undiscovered until their current evidence path fires.

The exact slot count and probability distribution require current ExpeditionSystem and map-owner inspection. They are intentionally not fixed here.

### Exclusion and conflict rules

A destination cannot be selected twice under two names. A location requiring a consumed one-use item is ineligible unless the quest owner has a recoverable alternative. A faction-only location is ineligible when the required reputation gate cannot be met and no route to that gate is available. A temporary site with an expired world condition is removed before sampling. A mandatory site cannot be removed by optional rarity, regional theme, or a “recently visited” penalty.

If multiple active quests require the same destination, reserve it once and expose the several authored reasons in the journal. Resolve conflicting local states through quest-context composition; do not fork physical copies of one place to make quest markers fit.

### Bible seed application

**Winter Count.** The current cipher engine can reveal target locations when the authored hearing/key/decoding conditions are satisfied. At expedition start, the selector should honor the revealed target as mandatory while the chain is active. Before reveal, the station can be a known radio clue without exposing the target marker. If a save contains a revealed target that no longer resolves, preserve quest progress and use a verified equivalent, an authored clue route, or an explicit delayed state.

**Shelf That Answers.** Hydrophone evidence may make a coast or shelf destination thematically eligible only after a destination binding is proven. A sound log alone is not sufficient to force a location into the dispatch list. Where no matching site exists, keep the investigation as a shelter or discovery scene and record a deferred destination proposal.

**Rhyme After the Door.** Folklore is likely to resolve in a shelter scene. Do not convert every rhyme into an expedition marker. A physical clue may point to an existing room or destination after source validation. Cohort changes must not spawn map locations unless the owner explicitly reports a verified event or unlock.

### Missing-target recovery and visibility

Use the following priority when a required target is unavailable: first, resolve an already-authored equivalent destination with the same semantic purpose; second, keep the quest active but delayed and expose a useful action or expected condition; third, publish an authored clue that leads to a valid alternative; fourth, make the destination unavailable only when the quest contract explicitly allows a failure-forward resolution. Log which recovery was selected so support and journal text can explain it.

Recovery must not silently teleport an expedition, fabricate a permanent map location, or discard a quest flag. If the target is removed by a content revision, the migration layer should preserve the old target identity long enough to map it or report the delay.

### Acceptance probes for the eventual implementation

Reviewers should walk at least these state combinations: an active cipher before hearing; after hearing but before key acquisition; after decode and target reveal; a hydrophone record discovered without a verified site; an active quest whose target was already visited; two quests sharing one site; a temporarily unavailable target; a failed-forward branch; and restore from a save captured between selection and dispatch. Each case must show map visibility, expedition eligibility, fallback, and the message explaining the outcome. Selection should be deterministic for the same seed and state and should never let optional content consume the last required slot. These are proposed acceptance cases, not permission to add new save fields or a second map registry.

## Pass 17 — Treat destination-bound micro-locations as encounters, not map nodes

This pass applies two master-world-bible questions to the location-selection architecture: which micro-locations lack narrative encounters, and how can new cemetery or archive clues enter expeditions without inflating the map graph? Current evidence shows micro_locations.json is already loaded by NarrativeEncounterCatalogLoader into EncounterDefinition entries. Each definition can carry requiredLocationId and isMicroLocation metadata. EncounterDefinition.GetEffectiveWeight returns zero when its required location differs from the current expedition site. NarrativeEncounterSystem filters depleted encounters before weighted selection. Three visible examples in the corpus bind an encounter to a named location, including a hospital, flooded subway depot, and garrison checkpoint. Therefore a micro-location encounter is generally an arrival/encounter opportunity at a parent destination; it is not automatically a new world-map node or an expedition destination.

### Two-stage selection contract

Keep the distinction between selecting a destination for an expedition and selecting an encounter after the expedition reaches a location.

**Stage A: expedition destination.** The current map, expedition catalog, quest state, region, and weather constraints decide which destinations can be dispatched. An active investigation may reserve its verified parent destination if that location is a real map node and dispatchable. The registry record alone cannot reserve a destination. The location-selection policy should expose the reason for reservation to the journal or expedition summary.

**Stage B: local encounter.** Once the current location is known, the encounter selector evaluates the ordinary eligible candidate list. A destination-bound micro-location becomes eligible only when its requiredLocationId matches. A quest-critical micro-location needs an integration seam that protects its required encounter opportunity from optional competition and prior depletion. Do not add a second micro-location scheduler or a competing encounter pool. If the existing selector cannot express the guarantee, record that as an architecture decision required before implementation.

### Candidate selection rules

1. Resolve each authored micro-location to its canonical parent location and confirm that parent in the locations catalog.
2. Resolve the parent against the map graph and current expedition dispatch surface; record whether the site is guaranteed, optional, hidden, shelter-only, or unavailable.
3. Resolve encounter-level gates such as danger minimum, weather gate, depletion, and quest prerequisites before weighting.
4. Reserve a mandatory quest opportunity only when the quest is active and the exact encounter is reachable at the selected parent destination.
5. Exclude optional candidates that would consume or permanently deplete a quest-required encounter before the quest begins.
6. Preserve ordinary seeded selection among all remaining candidates. Keep mandatory routing deterministic and independent of iteration order.
7. If there is no eligible local encounter, do not fabricate a micro-location. Continue the expedition with a truthful no-encounter outcome and leave the active quest delayed with a clue or alternate route.

The order is a proposal around existing owners; it does not change the core weighted-selection contract by itself.

### The cemetery investigation application

The registry corpus references location_ash_dune_cemetery, and locations.json currently contains that ID. The next check is whether it resolves through the map graph and expedition dispatch surface, and whether its location state supports a cemetery visit in the active campaign. If it is valid and dispatchable, an optional physical-inspection beat may run there after arrival. If it is not dispatchable, the quest remains a shelter/archive chain or routes to a verified clue-bearing parent site. Do not add “Ash Dune Cemetery Annex” or another node merely to create an encounter slot.

A micro-location representing an existing grave, plaque, or small shelter feature should normally be an encounter tied to its parent. Its visibility can be “known location, undisclosed local detail” until an authored discovery condition fires. This preserves map truth while still allowing exploration to reveal a hidden object or story beat.

### Visibility, rarity, and fairness

Use map fog for whether a parent site is known. Use quest/discovery state for whether a specific encounter is known. Use encounter weights for optional repeatable or exploratory content. These are separate visibility questions. Avoid displaying a quest marker for an encounter that has no reachable parent destination or whose content is depleted. If the quest is active and the encounter is mandatory, the journal should explain whether the player needs to travel to the parent, wait for an authored condition, or follow another clue.

Rarity can tune optional micro-locations but cannot determine a required quest outcome. A rare environmental encounter may appear only in an eligible subset of expeditions; if an active quest depends on it, either guarantee its parent and encounter window or make the quest wait in an explicit blocked state. Map selection should never imply that a local encounter is guaranteed simply because the parent destination is selected.

### Exclusion and lifecycle cases

Check the following collision cases in the eventual integrator packet: the parent site is selected but the required encounter is weather-gated; the encounter has been depleted before its associated quest is accepted; two quests refer to the same local encounter; a quest becomes active after the destination selection snapshot; the parent is known but not dispatchable; a content update removes or renames the encounter; and an optional encounter competes for a forced-on-arrival slot. The recovery may be reselecting the local encounter, returning on a later expedition, replacing it with an equivalent authored clue, or delaying the quest. It must not duplicate the location, reset unrelated depletion, or promise an encounter from data whose consumer is absent.

### Performance and review evidence

Profile before adding an index. If candidate evaluation is shown to be costly, prefer a deterministic cache built by the current catalog owner and invalidated when catalogs or relevant state change. Do not introduce a per-frame JSON scan, shadow map graph, or another selection registry. The review packet should include parent-location resolution, dispatchability, encounter eligibility, depletion policy, map visibility, and fallback for each proposed micro-location. That packet answers the bible's coverage question with live data and lets content authors add encounters only where the actual narrative gap exists.

### Pass 17B — Reservation walkthroughs and local encounter budgets

The following walkthroughs show how an eventual selector should reason about the difference between a reserved expedition destination and a locally eligible encounter. They are examples for a future source-backed integration review, not fixed slot counts or probabilities.

**Required cemetery visit.** An active investigation requires a physical look at the cemetery. First prove the cemetery is a current map node and dispatchable destination. If yes, reserve it before optional destinations and attach the quest reason to the visible destination card. After arrival, evaluate the linked micro-location only if the authored definition requires this parent. If a weather, danger, or depletion gate suppresses it, keep the quest active and expose the alternate evidence step. Do not claim that a reserved parent alone guarantees the encounter.

**Optional grave discovery.** The player has not accepted the investigation but discovers an optional gravestone encounter. The encounter may appear through ordinary local weighting. If its choice permanently depletes that encounter, record the existing depletion state and ensure later quest acceptance has an alternative such as a journal copy or archive testimony. The optional action must not make a future required objective impossible.

**Shared destination.** Two quests point at the same verified site. Reserve the destination once, attach both active reasons, and let each quest evaluate its own evidence state after arrival. Do not duplicate the destination to guarantee two encounters. If the current encounter system can select only one event per arrival, one quest may retain a clear revisit condition; otherwise both can be composed only through an approved owner seam.

**Late quest activation.** If a quest activates after the expedition's destination snapshot is already committed, it cannot rewrite the active route. The quest should select the next eligible expedition, identify a current nearby clue, or remain visibly delayed. This prevents state changes in dialogue from silently invalidating a route selection already shown to the player.

**No valid destination.** If the parent is missing from the map, no equivalent may be guessed from a similar display name. Consult canonical location aliases or a human-authored fallback. If none exists, delay the quest and provide an archive or witness path. A missing destination is a content-integrity problem even when the game can safely finish the expedition.

For each location-selection decision, show three independent outputs: whether the parent is dispatchable; whether the local encounter is currently eligible; and whether the clue is visible to the player. A single “available” boolean hides important reasons. The expedition summary should explain a required route in player language, while internal diagnostics can record the precise exclusion reason.

**Budget rules.** The selector should not allow optional content to occupy a hard capacity reserved for critical progression. However, this plan does not prescribe a numeric expedition slot count because the active system and UI need to be inspected together. Measure current destination volume, dispatch performance, travel costs, and UI card space before changing capacity. If the set of required destinations exceeds the display or expedition limit, combine compatible objectives at one parent site, defer low-priority quests, or provide an explicit multi-trip chain. Do not silently truncate the required set.

**Operational acceptance.** The review owner should be able to trace one selected destination from quest state through map visibility to dispatch, then from location arrival through encounter eligibility to journal acknowledgement. It should also trace the failure route from the same start. This walkthrough is the unit of confidence; a probability table by itself cannot prove the quest remains playable.


## Pass 18 — Expedition placement for instrument evidence without map inflation

### Design purpose

The Lane B Geiger calibration seed in the master world bible can enrich expeditions, but the current calibration owner describes a shelter-side, one-day station procedure. It does not establish a field-calibration station, a new destination, or an expedition interaction. This extension connects the subject to expedition selection without inventing a parallel location pool: expeditions may carry authored evidence or a location-bound encounter, while actual device calibration remains available through its existing owner after return.

### Selection contract

An expedition candidate is eligible only when its permanent authored definition satisfies the active quest's location, faction, time-window, and encounter constraints. Resolve eligibility before weighted selection. A weight may rank valid candidates; it must never defeat a hard quest requirement. The current plan's selection stack is refined as follows:

1. Mandatory active quest parent destination, if the quest truly requires travel and the quest catalog declares the existing destination ID.
2. Critical progression destination, under the game's current progression owner.
3. Character- or faction-bound destination already authorized by the current expedition content.
4. Recently discovered destination that remains reachable and has not been excluded by campaign state.
5. Thematic destination matching the expedition brief and season.
6. Optional exploration destinations.
7. Rare or surprise destinations, selected only from the remaining valid set.

For The Needle's Margin, the mandatory travel set is empty. A calibration return action is a shelter interaction, not an expedition location. If the quest has an authored report clue at a currently selected destination, the clue attaches as a child encounter under that parent. It must not create an extra map pin or consume a second destination slot.

### Parent and micro-location relationship

Treat the expedition destination as the map-visible parent. A destination-bound micro-location or encounter is a nested content opportunity with its own eligibility and visibility rules. It cannot independently satisfy a quest's travel requirement unless the quest data explicitly names the parent and required encounter. Selecting the parent reserves the opportunity; the encounter can still be omitted when weather, route, faction control, or content capacity invalidates it.

A field report about instrument drift could appear at a radio hut, survey shelter, or other authored parent only when that existing location's content is compatible. Those examples are content roles, not new catalog IDs. The report gives the player provenance and a lead. It does not claim that the shelter's calibration station is physically present at the remote site.

### Exclusion, rarity, and budget rules

- Exclude a candidate when it is already selected for the same expedition, unavailable under authoritative campaign state, incompatible with a required faction or season, or below the minimum encounter budget.
- Avoid two encounters that deliver the same clue, reward, or quest transition in one expedition. Repeated ambience may vary, but repeated objective credit must be idempotent.
- Rare encounters draw only after mandatory and progression needs are satisfied. Rarity is a selection preference, not a probability guarantee that can strand a quest.
- Quest-required parents are reserved before optional weights are drawn. If more mandatory parents are eligible than the expedition supports, the selection contract must either expand the valid destination budget through the existing owner or sequence the quest objectives. Never drop the lowest-priority active quest without an explicit delay response.
- Seeded selection uses the existing expedition random stream and a stable candidate order. Do not use wall-clock time, hash iteration order, or a new private random generator. An identical campaign state and seed must produce the same selection and map visibility.

### Map visibility and discovery

A selected parent appears at expedition start according to the current expedition map contract. Its unresolved nested encounter may remain hidden until the player explores the parent. A quest clue already received at the shelter can reveal a destination only when the authored quest explicitly supplies that lead; generic calibration status should not reveal a remote location.

Do not mark a destination permanently discovered just because it was selected. Selection, visibility, arrival, and discovery are distinct facts. Persist them through the current owners. A clue encounter can report that a technician once worked there without granting a permanent map unlock unless the existing discovery authority accepts that effect.

### No-valid-location fallback

When an active travel objective has no valid parent:
- Substitute an authored equivalent only when the quest declares the equivalence and the replacement can fulfill the same objective.
- Otherwise delay the objective with a visible status and preserve its clues.
- If a previously selected reachable parent exists, place a clue there only if the quest permits a clue-based alternate route.
- If none of these are valid, retain the quest in Blocked with a clear reason and a route to continue other play.
- Never label shelter calibration as complete merely because the expedition produced a calibration report.

### Walkthrough and acceptance

Walkthrough A: the player accepts an expedition unrelated to calibration. The existing destination selector chooses its normal parent set; The Needle's Margin adds no forced destination. If a compatible selected parent contains the authored report encounter, it can appear once.

Walkthrough B: a future quest variant explicitly requires a remote source. Its catalog declares one mandatory parent and an equivalent fallback. The parent is reserved before optional sites. If both are invalid, the quest moves to visible Blocked/Delayed state and remains completable later.

Walkthrough C: the clue is found but the party returns with low battery or a damaged sensor. The clue is recorded by its existing discovery owner; the calibration quest remains blocked on the real device condition. No location selection mutates equipment.

Implementation gates: confirm current expedition data represents nested encounters under parent destinations; confirm the quest selector accepts required-parent constraints; confirm map selection and discovery have distinct owners; and claim only those files needed for that verified seam. Acceptance includes deterministic replay of the candidate order, no duplicate map node, no silently impossible quest, no secret encounter appearing as a mandatory visible marker, and no expedition clue counted twice.


## Pass 19 — Location selection for documentary investigations

### Do not turn an intercept channel into a map node

The wiretap transcript schema includes an intercept channel, target-faction string, clarity score, speaker-identities string, relative timestamp, tags, and prose. It does not contain a verified location ID, route edge, access rule, or player discovery state. Names such as a junction, a carrier line, or a relay are authored channel labels. They must not become map pins by parsing prose or channel text. The source is currently indexed as CODEX_ONLY; the first gating work is a real presentation and discovery route.

The Unplayed Side has no required expedition destination in its minimum version. The player can investigate the record in shelter. A field trip is a separate optional branch that must declare a valid parent location using the current authoritative location catalog and expedition route rules.

### Candidate pools and reservation order

At expedition start, construct the candidate set from existing permanent locations, active-quest requirements, optional destinations, secret destinations, faction-specific destinations, and valid temporary events. The selector consumes these pools; it does not create a parallel transcript-destination registry.

Apply hard filters before ranking:
- remove locations that do not exist in the authoritative location data;
- remove locations already selected for the expedition;
- remove locations inaccessible under current campaign, route, season, or faction state;
- remove locations whose required encounter is incompatible with the current expedition;
- preserve a required destination only when the quest has an explicit, validated location reference;
- retain the current exclusion state for destinations already exhausted or intentionally unavailable.

Then allocate slots in this order:
1. Mandatory locations named by active quests and required now.
2. Critical progression destinations.
3. Character or faction destinations whose current owner says they are available.
4. Recently discovered, still-reachable destinations.
5. Thematic destinations that fit the expedition brief.
6. Optional exploration candidates.
7. Rare and secret candidates from the remaining valid set.

Selection weights rank candidates inside a priority band. They do not let a rare site displace a mandatory quest location. A seed and stable ID ordering determine ties. No wall-clock source, unordered dictionary traversal, or second RNG is introduced.

### Quest attachment and map visibility

A transcript clue can attach to an already selected parent location only when the authored encounter names that parent and the quest's condition is true. It remains a child encounter: it does not occupy another map slot or create an independent travel destination. The current source does not establish that any wiretap channel is a place, so location attachment requires a content mapping audit.

Keep the following states distinct:
- eligible: the destination can be selected under current constraints;
- selected: it is in this expedition's location set;
- visible: the map chooses to show it at expedition start;
- discovered: the player has learned the location through an accepted discovery action;
- visited: the expedition records arrival through its current travel owner;
- clue-present: a child encounter can be triggered at the parent.

A confidential or secret clue can remain hidden until discovered. A required active quest parent must be visible or otherwise clearly reachable according to the existing map contract. A generic wiretap reading must not reveal a remote destination.

### Fallback behavior

If an active quest requires a site and that site fails validation:
- use an equivalent only if the quest data explicitly declares the same objective can be fulfilled there;
- otherwise delay the travel step and tell the player why;
- place a clue at another already selected parent only when the quest declares the alternate route;
- offer a non-travel investigation branch when it is authored and valid;
- preserve all previously recorded evidence and quest progress;
- never mark the field objective complete because the selector returned no location.

If no valid candidate remains for an expedition, the existing expedition owner should return a truthful empty/shortened selection or a supported fallback. The selector should not loop trying random candidates forever. A quest can remain Blocked or Delayed while other expeditions proceed.

### Walkthroughs

Walkthrough A, shelter investigation: the player reads the Office ammunition intercept, accepts The Unplayed Side, and selects the evidence-comparison branch. There is no travel requirement, so expedition selection is unchanged.

Walkthrough B, optional corroboration: the quest has a verified parent location and marks it optional. If it is selected, the child encounter appears once. If it is not selected, the player retains the report and can request another expedition or choose the no-travel resolution.

Walkthrough C, mandatory corroboration added later: the quest declares a single current parent and one author-approved equivalent. The selector reserves the primary site first. If both are invalid, it delays the quest visibly; it does not silently remove the objective.

Walkthrough D, secret content: a hidden audio trace is an optional clue under a secret site only after another content source has discovered that site. It can increase context, but it cannot be the sole way to submit the base case.

### Acceptance and production order

First make the record reachable in its current content surface. Next prove the quest can express a location-free state. Only then consider optional location bindings. Before authoring new location content, verify an existing parent and its dispatchability, then verify the expedition map distinguishes selection from discovery.

Acceptance covers deterministic candidate ordering, hard exclusions, quest reservation, duplicate parent suppression, child-encounter capacity, secret visibility, empty-pool fallback, and a save/load case in which an accepted quest survives a delayed expedition. These are proposed checks for a later owned implementation; this pass adds no map data or route.


### Pass 19B — Worked selector scenarios for evidence-led quests

| Situation at expedition start | Location decision | Player-facing result |
|---|---|---|
| Player has read a wiretap but accepted no travel objective | Do not reserve a map slot for the document. | Quest remains investigable at shelter; map selection is unchanged. |
| Accepted case has an authored optional corroboration parent that passes all hard filters | Select the parent only if capacity and the existing destination selector allow it. Attach the evidence encounter under the parent. | Map shows the existing destination; the journal labels corroboration as optional. |
| Quest declares a mandatory parent and the location is reachable | Reserve the parent before weighted optional picks. | Destination appears through the current expedition map contract. |
| Parent is faction-locked and the player lacks access | Keep the candidate excluded. Use a declared equivalent or delay. | State the faction/access blocker without exposing a secret route. |
| Parent has already been selected by another active quest | Reuse the parent once and attach both compatible encounters if capacity permits. Do not duplicate the map node. | Each quest gets its own progress result; encounter execution is idempotent. |
| Every candidate is invalid | Return an empty/shortened selection or the selector's supported fallback. Keep the quest pending. | Explain that evidence work is delayed; offer the no-travel branch if authored. |

The selector should report a small result envelope to its current caller: selected parent IDs in stable order, deferred quest requirement IDs with reason codes, and child encounters admitted under each parent. This is a design shape only. Reuse the current expedition result type if it carries the same information; do not add a second map-selection DTO merely for the wiretap feature.

The content author declares whether a location is required, optional, secret, faction-specific, or temporary. A single destination can satisfy more than one quest, but its runtime visit should not grant the same evidence milestone twice. A temporary location can serve a case only if the underlying objective remains possible after its window closes; otherwise it must be explicitly timed and failure-forward.

Attribution and secrecy affect visibility. A public route rumor may reveal a parent before the expedition starts; a sealed transcript cannot. The map label may reflect known information (“old relay site”) but cannot expose an unknown parent through a hidden quest pin. If the player later discovers the parent through another system, the quest may use that discovery without claiming the transcript revealed it.

### Capacity and fairness rules

A quest requirement is feasible only when it survives both route eligibility and expedition capacity checks. Validate feasibility after hard filters and before assigning optional slots. If a required parent cannot be allocated because the expedition budget is full, the caller should either reserve a slot through its existing capacity policy or defer the objective with an explicit reason. Do not use a high weight to approximate reservation.

A discovery-only or interview-only completion must remain available when map content is absent unless the quest's authored premise truly depends on that trip. This lets players progress across different route seeds without lowering the meaning of evidence. The same-seed replay should select the same parent and defer the same requirement when campaign state is identical, including faction access, route hazards, and quest statuses.

The final selector review records, for each candidate, its authoritative ID, source catalog, hard eligibility predicates, selection band, map visibility rule, child encounter ID, discovery effect, quest credit, and fallback. No site can be approved from a wiretap's intercept-channel text alone.


## Pass 20A — Expedition availability for a serialized radio case (DRAFT)

### Selection contract

This continuation applies the Part 43 Verdict Radio Theater seed to the existing expedition-location selection design. It does not create a permanent “radio theater” map node or promise that every broadcast is physically heard during an expedition. VerdictRadioSystem publishes authored radio events from a day/phase scheduler; Plan 94 already supplied 30 Machine-Register entries. The radio program production owner prepares player programs and follow-up hooks, but does not own reception, location selection, or scripted episode prose. The current player-facing reception path and the expedition selector’s exact production owner must be verified before implementation.

The Quiet Hours case should reuse a currently valid shelter, relay, or community-interaction anchor. Treat an episode as a quest requirement with a location affinity, not an unconditional map pin. The location data record continues to describe the place; the quest record describes why the player may need to visit; the expedition selector decides which valid instance is available; the map presentation reports actual availability. Do not let dialogue or UI fabricate a route that the expedition owner did not select.

### Priority and reservation rules

Apply the expedition’s existing hard capacity and exclusion rules first. Then choose required entries in a stable priority order: active mandatory quest objective with no non-location fallback; critical progression; character/faction appointment already made; a discovered lead whose return window is open; thematic fit for the expedition; optional exploration; rare surprise content. A radio-theater clue alone is not a mandatory location. Once the quest explicitly accepts a visit objective, reserve one eligible target or mark the objective as delayed before presenting expedition choices.

Within each priority tier, filter candidates by authored availability, route reachability, previous visit/exhaustion rules, quest and faction conditions, and duplicate/overlap exclusions. Rank remaining candidates with the existing seeded selection stream and stable IDs; never use wall-clock time, hash enumeration order, or a fresh untracked RNG. A mandatory target may displace an optional or surprise target, but it cannot exceed expedition capacity or violate a route gate. If two mandatory quests compete for one slot, resolve by explicit progression priority and postpone the other with a visible reason.

For this arc, candidate anchors can be described by roles, pending a catalog audit: the currently reachable relay site that surfaces the follow-up; a shelter common area where a witness is available; and an authored archive or notice surface that can supply the clue when travel is blocked. These are role labels only. The plan must not invent a stable ID, assume the old radio room exists in every campaign, or add the same physical place under several catalog names.

### Availability and map visibility

The broadcast scheduler’s fired flag is a world-system fact, not a map-discovery fact. The map may show a question-mark lead only after a player-visible clue exists, such as a note, a conversation, or a surfaced radio log. It may show a named location after the player has discovered it under the existing map rules. If the quest accepts a visit, show its requirement in the expedition briefing and preserve the location’s ordinary exploration description. Hiding the exact target can be a valid investigation choice only when the player has another meaningful clue and the objective cannot become impossible.

Visibility should have four distinguishable states in the planning model: unavailable, rumored, discovered, and selected for this expedition. Do not confuse these with quest lifecycle statuses or persist a duplicate map-discovery flag in the quest. The map reads its canonical discovery owner; the quest reads objective status; the expedition snapshot reports selected entries. A panel may summarize all three but owns none.

### Fallbacks and edge cases

If the required relay site cannot be selected, use one of three explicit behaviors. Substitute another authored, semantically equivalent site that satisfies the same objective contract; defer the visit and offer a credible clue that advances investigation without falsely marking the visit complete; or keep the objective Blocked with a specific condition and guaranteed retry. The fallback is chosen in data or by a reviewed deterministic rule. Never silently delete the requirement, mark it complete from a broadcast that was not heard, or create an unsaved temporary location.

If there are no valid locations at all, the selector returns a typed no-candidate result to its existing caller. The caller can preserve the quest and explain that no expedition route is currently open. It must not loop forever, lower a critical gate, or select an excluded location. If every optional candidate is excluded, fill the remaining capacity according to the current ordinary fallback policy; if there is no such policy, preserve an empty slot rather than fabricate a location.

Production cost is low for a reuse-only core slice and medium for an expansion if episode-specific map art, route variants, or new location content is approved. The core slice consists of requirement reservation, deterministic candidate filtering, map truth, and a testable fallback decision. Optional layers include a second witness site, a secret staging room, faction-specific listening access, and temporary carrier locations. Each optional layer adds content-review and availability-matrix cost; none should be bundled into the minimum viable contract.

## Pass 20B — Worked selector scenarios and no-stranding proof

**Scenario A: the player has discovered the relay but not accepted the quest.** The relay follows normal thematic and discovered-location weighting. It is never reserved as mandatory. A broadcast can remain atmosphere; the quest stays Available or undiscovered according to its verified trigger. The expedition can still select its usual mix. This guards against accidentally turning every scheduled corpus entry into a compulsory map visit.

**Scenario B: the player accepts the witness visit and the relay is eligible.** The target is reserved ahead of optional exploration, subject to normal capacity. If the witness is in a different eligible site, the quest objective names that site or a clue makes the substitution legible. The map does not show both as simultaneously guaranteed if only one can spawn. The briefing describes what will be possible on this expedition, and the quest remains In Progress until the player actually visits and completes the interaction.

**Scenario C: route conditions exclude the preferred target.** If the authored equivalent site is available, choose it through the stable seeded tie-break and attach a short line explaining why the witness is there. If no equivalent is available, keep the objective Blocked or Delayed, identify the missing route condition, and place an alternate non-location clue only if that clue has its own consumer. The expedition is still playable; the story cannot claim the meeting occurred.

**Scenario D: two required visits exceed capacity.** The selector preserves the higher-ranked critical progression visit and identifies which objective moves to the next expedition. A fair policy should order by authored urgency, explicit accepted appointment, and stable quest ID as the final tie-break. Never use plan-file order or dictionary order. If the player’s last choice caused the conflict, show that cause clearly; do not disguise a consequence as randomness.

**Scenario E: every eligible target is visited or exhausted.** Reuse is allowed only if the location’s current authored repeat-visit contract permits a revisitable scene. Otherwise use a valid substitute, a deferred appointment, or the authored clue fallback. A clue fallback can advance the investigation’s knowledge but cannot satisfy an objective explicitly requiring an in-person witness. If no fallback exists, report a content-data failure and keep the quest visibly delayed.

The acceptance record should include a matrix of quest status × candidate class × route availability × map state × expedition capacity. Minimum cases: available-but-not-accepted, accepted-and-selectable, accepted-but-excluded, critical progression conflict, no valid site, saved selection restored, location visited but interaction interrupted, and fallback clue found before the primary site. For every row, record whether the player sees a map marker, whether the objective can progress, and which authority owns the persisted fact.

Rarity should operate only inside a priority class. It must not make an active mandatory objective rare. The theater’s optional secret callback can be weighted low after all guarantees are satisfied; it is excluded if the player already heard or resolved the callback, if an incompatible scene occupies the same location slot, or if the authored availability window is closed. Use the current seeded selection API if verified. If none exists for this selector, that is an architecture decision to surface rather than permission to add an RNG manager.

An expedition reload must preserve the already-selected list if the current expedition snapshot owns that list. Re-running selection from a changed day or a new RNG draw after reload can swap out a promised objective. If selections are currently recomputed, the implementation package must identify the existing deterministic snapshot boundary and prove stable replay; do not add a parallel location-selection save section as a shortcut.

No-stranding proof: each active quest requirement has at least one of (a) a selectable target, (b) an explicitly equivalent target, (c) an alternate clue with a reachable consumer, or (d) a visible delayed state and guaranteed reevaluation condition. The proof is per objective and must survive map exclusions, route damage, NPC absence, save/restore, and full expedition capacity. A content author’s statement that “the scene should appear” is not sufficient evidence.



## Pass 20C — Expedition selector contract and content budget

### Selection record

At the start of each expedition, represent candidate sites as references to current canonical location entries, with eligibility reasons derived from current owners. A proposed selection result needs: expedition identifier; required quest references; eligible candidate references; selected site references; exclusion reasons for debugging; deterministic seed/stream reference; and a player-visible availability summary. These are conceptual contract fields; confirm the existing expedition snapshot shape before proposing a schema change. Persist only through its current owner. A panel may render the result but may not reserve, choose, or restore locations itself.

Apply the seven-pool model as a priority policy, not as seven mutable registries: mandatory active quest locations; critical progression locations; character/faction appointment locations; recently discovered locations; thematic candidates; optional exploration; rare surprises. Required quest content may reference any compatible pool. The selector should merge candidates by stable location ID and preserve a set of reasons, so a site that is both discovered and quest-required is selected once. The player sees the strongest valid reason, usually the active objective, while map lore and ordinary description remain attached to the canonical location.

### Sampling and fallback pseudocode in words

1. Read the expedition request and current quest/location facts from their owners.
2. Reject candidates that fail hard route, story, visit, capacity, or exclusion constraints.
3. Reserve mandatory candidates in the authored progression order; detect slot conflicts before choosing optional sites.
4. Resolve unavailable requirements through an authored equivalent, a clue route, or a visible delayed state. Record which route was selected so the briefing is honest.
5. Fill remaining slots from the next eligible priority class. Use the existing seeded stream and stable ID ordering for tie-breaking within a class.
6. Return the chosen set and any unresolved requirement result to the current caller. Do not repeat selection silently on UI refresh.
7. Present map markers and expedition briefing from the returned selection plus canonical discovery state.

### Exclusion and rarity policy

Exclude duplicate canonical IDs, mutually exclusive scene variants, faction content whose access gate is false, exhausted one-time scenes, locations outside the expedition’s route radius, and temporary content after its expiry. Exclusions need a diagnostic reason to make failed content review actionable, but that reason is not automatically player-facing. The player receives a plain-language explanation only when an active accepted objective is affected.

Rarity is a tie-break preference for optional or secret entries after all hard requirements and capacity are satisfied. A rare surprise cannot displace a required character appointment. A secret location may remain unmarked until discovery, but its prerequisites and fallback must be testable from content data. Temporary pools should provide an explicit expiry and recovery rule; an expired temp site cannot leave a quest pointing nowhere.

### Test matrix and production estimate

A focused selector target should cover: duplicate-role merge, no candidates, one mandatory slot, conflicting mandatory slots, deterministic same-seed output, different-seed optional variety, save/restore of a chosen list, unavailable target with equivalent, unavailable target with clue, unavailable target with delay, secret map visibility before/after discovery, and an optional rare site at full capacity. Each case should report selected IDs and exclusions rather than only a count.

Core production cost is one selector contract extension only if the existing owner has an identified seam, plus a narrow expedition briefing projection. The theater content itself should reuse existing locations and add no new location assets for the first release. An expansion layer may add one new relay interior or a faction-exclusive performance venue after map content review, cost estimate, and travel data validation. New sites need an authored description, map footprint, route eligibility, return behavior, required items/encounters if any, and a visit-state consumer. A sentence naming a place is not sufficient to create a location.

A no-valid-location response should be an ordinary typed outcome, not an exception and not an endlessly retried selection loop. If no route can satisfy a required objective, preserve the quest’s current progress, present the named blocker, and set a deterministic next evaluation point such as the next expedition start. Never convert an unsatisfied visit into completion simply because all alternatives were absent.


## Pass 20D — Location availability for the optional response story

The Second Margin is intentionally conversation-led, so its MVP should not force a new expedition. A listener response can be delivered at the same currently eligible hub as the parent arc, provided that the canonical location supports repeat visits and the dialogue owner can present the follow-up. If that route is unavailable, the content remains optional and is not shown as an accepted visit objective. A teaser cannot create a map marker without a verified location reference.

If an expansion adds a separate place to hear several interpretations—such as an existing common room, archive corner, workshop, or radio site—describe its role and eligibility without adding a new location ID until the location catalog is audited. The location selector should only add that candidate when the follow-up is accepted, the parent episode has a valid outcome, the place is reachable, the scene is not already resolved, and expedition capacity remains after mandatory objectives. A second listening location is lower priority than active quest requirements and character/faction appointments.

**Selection walkthrough:**

- Parent quest unresolved: do not select a response-only site. The player cannot respond to an outcome that does not yet exist.
- Parent resolved privately: allow a quiet follow-up at an existing hub; do not reserve a public venue.
- Parent resolved publicly and delivery confirmed: a public response scene may be thematically eligible, but remains optional. Use the exact delivered version as context.
- Parent resolved by silence: offer no station response. A private conversation remains available only if the authored character actually has a reason to speak again.
- Follow-up appointment accepted but no eligible site: preserve the accepted state as Blocked/Delayed with a clear retry, or use a reviewed equivalent. Do not respawn the original parent location under an alias.

The map should distinguish a general conversation opportunity from a new expedition destination. If the site is already discovered, show its ordinary icon and an optional callback indicator driven from quest state. If undiscovered, reveal it only through the location owner’s normal clue rule. The quest panel must not set discovered state. If an interaction can happen only while a location is selected for an expedition, the briefing must say so before deployment and preserve that selection on reload.

For repeatability, the follow-up may choose one of two authored listener interpretations based on the parent resolution, but selection is semantic, not random: public correction selects public response, private correction selects private response, and silence selects no public response. If a future expansion offers multiple listeners, a seeded tie-break may choose among eligible authored voices only after stable sorting. Do not add variety that changes the truth of what was broadcast.

Acceptance checks add: no optional-site reservation while the parent remains open; no site spawned solely by a passive broadcast; no false map marker after player declines; no mandatory capacity displacement; a fallback when the repeated-visit hub is unavailable; and persistence of the chosen follow-up location if the expedition snapshot owns it. Because this optional arc can be deferred without loss of core progression, its absence must not enter critical-path no-stranding calculations.


## Pass 20E — Location pool examples for the full case portfolio

### Location roles, not invented map entries

The case portfolio uses semantic roles until a map/catalog audit establishes real IDs. A “relay conversation anchor” is a role for an existing site that has a radio or program interaction. A “witness anchor” is an existing shelter or expedition location where a relevant character can be present. A “program-page clue anchor” is an authored searchable place that can carry environmental discovery. A “public response anchor” is a suitable place for an optional audience follow-up. One canonical location can satisfy several roles if its current data and scene slots support them; do not add duplicate locations just to meet the role count.

| Expedition pool | Case use | Selection rule | If unavailable |
|---|---|---|---|
| Mandatory quest | Accepted visit objective for The Quiet Hours | Reserve if no equivalent non-location completion exists | Equivalent site, clue plus delay, or visible Blocked state |
| Critical progression | Any current campaign objective | Preserve its existing priority and slot contract | Existing progression fallback takes precedence |
| Character/faction | Accepted appointment or eligible character visit | Select only when its current actor/route gate passes | Reschedule under that owner; no theater bypass |
| Recently discovered | A player-known clue site | Weight after required sites | Use a different discovered clue or keep clue unavailable |
| Thematic | Relay, archive, shelter common area | Use only when it satisfies route and scene constraints | Drop without changing quest state |
| Optional | Second Margin or Page Left Face Down | Fill remaining capacity | Side quest stays Available or is delayed |
| Rare/surprise | Secret version of an already-valid scene | Select last, after exclusions and capacity | Omit safely |

### Capacity and fair scheduling

Quest urgency should not be guessed from prose. The author sets a bounded priority class and an optional window; the selector’s owner applies the live capacity and route rules. If two accepted visits conflict, show which one remains available this expedition and which one is delayed. Use authored urgency, appointment commitment, and stable quest ID as tie-breaks only if the current selector permits them. A faction-specific meeting must not displace a critical progression objective unless the existing design already says it can.

A player should not be forced to choose between two story lines without being told. Before expedition start, show the visit that can happen now, the visit that must wait, and whether waiting changes anything. If neither visit is urgent, the player may select which one to pursue. This choice belongs in the expedition request, not in a map panel that directly changes the selected list. If the player changes the request, rerun the current selector once at the approved command boundary and persist the final selection through the current snapshot owner.

### Return and interrupted-scene rules

Entering a location does not complete a visit objective unless the authored quest contract says arrival is sufficient. If the player arrives but leaves before speaking, preserve the objective as In Progress and make the same location available again under existing revisit rules. If a character is absent after arrival, return a clear unmet condition and do not mark a visit complete. A player who found a program page but saved before reading it should see the same clue at the same selected site if the location’s persistence owner preserves the expedition. If it does not, the implementation package must define the existing deterministic regeneration boundary.

Map visibility is derived separately from expedition availability: an undiscovered site can be selected only if the current game supports hidden selected destinations; otherwise its clue must reveal it first. A selected-but-undiscovered site should not appear as a fully named icon. A rumor marker may be appropriate only under the existing map vocabulary. Revisited locations should not show a fresh “new” indicator. The UI reads these facts and presents them; it does not synthesize extra availability.

### Tuning policy

Do not balance the story through invisible spawn percentages. Mandatory content is guaranteed by valid reservation or surfaced delay. Optional callbacks can be tuned by tier weight after verifying they do not starve ordinary exploration. Track selection frequency across seeded test scenarios, but do not add persistent analytics merely for this feature. A content author can define thematic tags; the location selector should use only tags already understood by its contract or receive an explicitly reviewed schema extension. Unknown tags must be ignored or rejected consistently, not treated as a hidden priority boost.


## Pass 21A — Expedition and location plan for The Yellow Lamp Has a Shadow

### Location roles and map truth

The source catalog’s origin_sector is narrative provenance: examples include education, dormitory, corridor, library-corner, and shelter-yard labels. Those strings are not proof of canonical map locations. The location selector must resolve them through an existing location catalog or treat them as descriptive metadata only. No new map node should be created just because a folklore entry mentions a sector. A codex row is a reading surface, not an expedition destination.

The MVP quest is conversation-led and should complete without an expedition. An optional field-observation beat can use an existing eligible surface location only when the player has a route and the location data confirms its identity. The point is to let the adult compare an old indoor description with a present observation; the scene should not force the player to go outside or expose them to a hazard simply to finish a dialogue quest. A safe alternate account may come from an existing traveler or field report if the data source is confirmed.

### Pool ordering for the story

At expedition start, process existing mandatory active quest locations first, then critical progression, character/faction appointments, recently discovered sites, thematic sites, optional exploration, and rare/surprise entries. This arc normally contributes no mandatory destination. If the player accepts the optional field-observation objective, attach its required role to a valid existing destination and only then reserve it. A hidden location remains secret until the current discovery owner reveals it. A candidate labeled by origin_sector can gain thematic affinity but cannot bypass route, capacity, safety, or discovery rules.

Deduplicate a site that qualifies for both field observation and another quest. Keep the mandatory reason as the reservation priority and list all compatible scene opportunities for the location owner. If the site cannot host both scenes, select an authored compatible variant, postpone the optional folklore scene, or show a delayed state. Do not spawn two aliases to force both contents into the same expedition.

### Fallback and failure cases

- Preferred site is unavailable: use a different existing location with a reviewed surface/sky observation scene, if one exists; otherwise keep the objective delayed and complete the core conversation in shelter.
- Player does not select the expedition: no time-based failure unless an actual authored deadline applies. The quest stays Available/In Progress with a clear next step.
- Site is discovered but not selected: show that the observation is optional and can be pursued on a later expedition.
- Site is selected but the player exits before the scene: preserve In Progress and make the visit retryable under current location revisit rules.
- The player cannot or will not travel outside: provide a grounded account from an existing traveler or let them resolve through the adult characters’ competing interpretations. Do not imply that staying inside makes their choice inferior.
- A journal reference points to a source sector with no location record: keep the source label as lore metadata and omit the map pin; surface a content-integrity warning for the broken optional cross-reference.

Map visibility follows the established map owner: unavailable, rumored, discovered, selected-for-expedition. Quest state may request a marker after acceptance, but the map owner validates whether that marker is truthful. If only a codex clue exists, show it in the codex, not on the geographic map. The expedition briefing should distinguish the optional field observation from critical progression and make the opportunity cost visible.

### Acceptance and costs

The core selector test matrix should include: no expedition needed; valid optional site; unavailable source sector; duplicate location qualification; max capacity; player declines travel; restored expedition snapshot; and a visit interrupted before interaction. Verify that seed variation affects only optional sites and never makes the narrative’s central interaction unavailable. The lore arc is a low-to-medium production cost when it reuses locations, and a high-cost expansion if it demands a new interior or expedition scene. A location asset is justified only if it offers repeatable exploration or multiple authored uses beyond this one story.
### Pass 21B — Selection cases for the folklore lead (DRAFT)

The narrative tag origin_sector is metadata about an authored tradition. It is not automatically a map-location ID, spawn request, or proof that a location can appear. The selector must first resolve an explicit content link owned by the location catalog. If no such link exists, the quest uses a shelter conversation and does not pretend a field trip occurred.

#### Selection case 1: mandatory destination already valid

When a currently active quest explicitly requires a location and the location is eligible for the expedition's biome, phase, capacity, and safety constraints, reserve it before optional draws. Mandatory means the quest cannot make progress without that location in this expedition; an optional folklore context visit does not qualify. Reserve only the actual required count. The selector should then fill remaining slots from eligible pools without displacing reserved content.

#### Selection case 2: mandatory destination invalid

Evaluate, in order: authored equivalent location with the same gameplay contract; clue-only fallback that supports shelter investigation; delay until the site is eligible. Record which resolution was selected so the quest objective matches reality. Never place an incompatible site on the map simply to satisfy a quest, and never leave an accepted objective that cannot be completed or postponed.

#### Selection case 3: optional context site

If capacity remains after required and critical progression content, the folklore-origin site may enter the optional pool. Give it a low but nonzero rarity weight and a deterministic selection key derived from the expedition seed and stable candidate IDs. The exact weight belongs in the selector's tuning authority, not in quest prose. If it is not selected, the player still has a complete core route through shelter scenes.

#### Selection case 4: recently discovered clue

A discovered clue may raise eligibility or visibility priority only if the current selector supports that concept. It must not silently turn an optional site into a mandatory spawn. The map can show a known lead as unavailable, approximate, or not selected, according to established map language. Do not display a precise marker for a location the player has not discovered unless the clue justifies it.

#### Selection case 5: no valid expedition this cycle

The quest pauses at a legible state such as “awaiting a safe route” or offers a conversation fallback. The journal should distinguish “not selected” from “visited and empty.” On later expedition starts, eligibility is reevaluated from current state. A retry must use the normal deterministic selector, not a hidden forced reroll that changes unrelated locations.

#### Exclusion and capacity rules

1. A single location instance cannot satisfy two mutually exclusive quest obligations unless the authored location explicitly supports both scenes and its capacity model allows them.
2. Mandatory story locations outrank optional variants, but active mandatory locations still obey hard safety and world-phase exclusions.
3. Secret content must not consume the only slot available to a required destination.
4. Temporary locations require an explicit lifetime and cleanup rule; their disappearance must not erase a durable quest fact.
5. Faction-specific locations require the appropriate access condition and must not leak onto the map as available before that condition is met.
6. Recently visited content may receive a repetition penalty only after required content has been protected.
7. Selector tie-breaking must be deterministic and independent of dictionary iteration order.

#### Visibility and discovery contract

The location-selection result and the player-facing map are related but distinct. A selected hidden site can remain undiscovered until the player finds a clue or enters the relevant exploration radius. A discovered but unselected site can remain in the journal as a lead without appearing as an expedition destination. A selected quest-required site can be marked with the same visual language used for other required objectives. The quest system owns objective truth; the map adapter renders it.

#### Review cases for the eventual selector owner

Use fixed seeds to verify: active required content survives optional-pool pressure; optional folklore context appears only when eligible; an invalid origin-sector link takes the configured fallback; a blocked location is not falsely reported as visited; two quests sharing a site do not duplicate the same instance; no-valid-location behavior preserves a recoverable quest; and repeated runs with equal state and seed produce equal selections. This is a design acceptance list, not an instruction to add tests in this documentation pass.
### Pass 22A — Expedition selection under an inconclusive survey (DRAFT)

The blank-edge quest creates a selector requirement: an active quest may ask for an authored survey destination, yet neither its existence in a catalog nor a previous failed approach proves that it is safe, reachable, or suitable for the next expedition. Selection must protect valid mandatory objectives while retaining legitimate uncertainty.

#### Candidate classes and reservation order

1. **Hard safety and phase filters:** reject candidates incompatible with current weather, route state, world phase, vehicle or party requirements, and any current safety gate. No narrative priority overrides these constraints.
2. **Valid mandatory quest destination:** reserve only when the quest predicate truly cannot advance without an expedition to that exact eligible destination. An optional follow-up after an inconclusive return is not automatically mandatory.
3. **Critical progression destination:** reserve canonical progression sites only when the live quest owner confirms that the relevant stage is active and the site is eligible now.
4. **Character or faction destination:** include only if an authored access condition is met and the current faction/location owner confirms visibility and availability.
5. **Previously discovered route alternatives:** offer a known alternate approach as a candidate only if it exists in authored location/route data. A clue can nominate, not instantiate, a destination.
6. **Thematic and optional destinations:** select from valid non-reserved candidates with deterministic weights and a bounded repetition penalty.
7. **Secret and surprise destinations:** sample after required obligations have been protected; keep their discovery semantics separate from map visibility.

#### Survey-specific algorithm sketch

At expedition start, the host asks the canonical quest projection for active requirements. The selector resolves each requested ID against the current location/route authority, evaluates normal eligibility, and returns a typed result: selected, equivalent-selected, delayed, clue-fallback, or unavailable-with-reason. A quest-facing adapter translates the result into objective text. It does not mutate quest state from inside the selector.

If the same destination remains blocked, the selector must not repeatedly roll it as though it were a fresh optional candidate. It can present an authored alternative only when that alternative has equivalent proof semantics. For example, an alternate approach may prove access to a lower ridge without proving the original crossing is safe. The objective wording must name the narrower claim.

#### Worked cases

**A. Route known and eligible:** reserve the route destination when it is a true mandatory requirement; fill other slots from optional pools. The map may identify the mandatory route using established objective language.

**B. Route known but season- or hazard-blocked:** return delayed with the reason. Preserve the objective and allow the player to pursue another quest. Do not include the destination and then fail the expedition at launch.

**C. Original location blocked, equivalent authored site available:** select the equivalent only if its location metadata declares compatible survey evidence. The quest changes to a narrower alternate objective before dispatch. Do not silently substitute a different place under the original name.

**D. No eligible site or equivalent:** offer a debrief, archive, or clue-only conversation path. The player can end the quest as inconclusive; the selector does not force a location, consume an optional slot, or strand the journal objective.

**E. Player has a clue but not discovery:** keep the clue in the journal. Show an approximate or no-map-marker treatment according to existing map conventions. Do not set the location's known flag.

**F. Destination selected but not entered:** selection is not visitation. If the player retreats before reaching it, no local discovery, site dialogue, or visit reward may be emitted.

#### Exclusion, rarity, and repeat behavior

Do not allow a single location instance to satisfy incompatible mandatory quest steps at once. Permit a shared site only if its authored capacity and encounter contract support both. A recently attempted destination may receive a repetition penalty for optional draws, never for an eligible hard requirement. Apply rarity after exclusion and reservation, using the existing seeded RNG contract and stable IDs. If valid candidates tie, use an explicit stable tie-break rather than collection iteration order.

#### Map visibility and result explanation

Distinguish four states in the projection: unknown destination, known but unavailable, selected for this expedition, and actually visited. A fifth state, completed objective, belongs to quest truth and must not be inferred from map selection. The debrief should say “route not reached,” “site visited but route unverified,” or “route disproved” only when the result facts support those claims. A location can remain on the map after it is disproved; the map communicates knowledge, not an automatic deletion command.

#### Fallback behavior and verification design

Fallback order: authored equivalent with explicit evidence equivalence; clue that leads to a later valid site; delayed objective with an intelligible reason; optional inconclusive debrief. Never silently make the quest impossible. A future selector verification pass should cover deterministic same-seed selection, required-pool protection, weather ineligibility, no-candidate behavior, location selected but unvisited, alternate evidence semantics, and multiple quests sharing one destination. This document pass adds no tests or selector implementation.
### Pass 22B — Stable selection walkthrough and draw accounting (DRAFT)

To keep the selector explainable, a selected quest destination should have a small decision record available to diagnostics: request ID, candidate ID, eligibility result, priority class, and terminal selection/fallback reason. This diagnostic output must not become a second saved map state or disclose hidden candidates to the player. It exists to explain selector decisions to maintainers and to support bounded acceptance review.

#### Draw accounting

First build the eligible candidate set from canonical catalogs. Apply hard exclusions. Resolve mandatory reservations in stable quest-ID order, preserving authored quest priority where one exists. If reservations exceed available slots, do not arbitrarily drop a quest: return a capacity conflict to the quest/expedition host so it can delay one objective or present a clue fallback. Remove reserved instances from optional pools. Apply optional rarity/repetition policy to the remaining stable list, then draw with the existing seeded expedition stream. Finally, project map visibility from discovery facts; do not derive discovery from the draw.

#### Deterministic walkthrough

Given the same campaign state, active quest facts, candidate catalogs, tuning values, and seed, the output set and order must be identical. Candidate sorting uses stable ordinal IDs before consuming random values. Excluded candidates consume no random draw unless the established RNG contract explicitly requires fixed-slot consumption. If that contract differs, preserve it and document the sequence. A clue-fallback response should be deterministic from the same inputs and should not reroll the optional pool.

#### Slot pressure examples

**One mandatory, five optional slots:** reserve the mandatory eligible destination first; draw five optional valid sites. **Two quests, one destination slot:** determine whether one site can satisfy both authored contracts; otherwise report a conflict and let the quest scheduler resolve priority. **No mandatory candidate, optional room available:** do not promote an unrelated location and claim it satisfies the objective. Return a safe fallback. **All candidates hard-excluded:** produce a valid empty/short expedition selection according to existing product rules and surface why the quest is waiting. **Secret site plus one mandatory site and one slot:** the mandatory site wins; the secret site remains eligible for a later expedition.

#### Performance and observability

Candidate evaluation should be linear or bounded by the loaded candidate count and active requirements. Avoid per-frame selection, repeated catalog scans from UI refresh, or storing large prose records in the selection result. Cache only immutable catalog lookups under existing cache ownership. Diagnostics can report counts by pool and exclusion reason; they must not include private player narrative or create an extra telemetry persistence layer.
### Pass 23A — Ambient-rumor hub eligibility and expedition separation (DRAFT)

An ambient rumor can create an investigation lead, but a rumor location is not automatically an expedition destination. The information hub and location catalog must be joined through an existing stable location ID. If that link is absent or invalid, the rumor may not spawn a map marker, and the player should receive only the presentation that the current information-flow owner supports.

#### Three independent eligibility questions

1. **Can this hub host ambient-only rumors?** This is a static content-policy question. Current rumor hub definitions expose credibility, bias, location, and daily rumor capacity; they do not expose an explicit noise-dominant classification. Do not infer this mode from low credibility, neutral bias, a cave location, or low capacity.
2. **Can the player hear this rumor now?** This is a runtime availability question owned by the rumor and dialogue surfaces. A hub’s authored existence does not prove the player visited, intercepted, or heard anything.
3. **Can the player investigate at an expedition site?** This is a separate destination-selection question. It requires a real, eligible, discoverable location and an authored interaction. A rumored origin is not itself a valid destination unless catalog data explicitly says so.

#### Eligibility proposal

If a future schema adds a hub mode, only hubs explicitly marked ambient-only may publish source-free rumor entries. The selector for rumor presentation should query the hub owner, not classify hubs by string matching. A quest-specific optional trip may be nominated only when its site is a normal location ID, is eligible under season/weather/route rules, and the quest requires physical evidence that cannot be obtained through conversation. Otherwise the investigation remains in the listening room.

#### Selection priority when the optional trip is approved

Mandatory progression destinations remain first. An ambient-rumor site is an optional discovery destination and receives no reservation priority merely because a player heard a rumor. If an accepted quest makes the visit mandatory, its content must state why and the selector must confirm that the destination is valid before the quest promises it. Under slot pressure, the site may be delayed while the player can still resolve the quest from the hub. It may not displace a required main-story location or secret content that has a stronger authored rule.

#### Map visibility rules

Before the player hears the rumor: no marker. After hearing an attributed account: a journal lead may appear; a precise map pin is allowed only if the account supplies a justified location. After a location becomes a valid candidate but is not selected: show no claim that the player can go there this expedition. After selection but before entry: mark it as a planned destination only using existing map language. After entry and observation: record the actual visit through the location owner. An absent or contradicted rumor never erases a real discovered location.

#### Fallback cases

If no ambient-only hub is configured, no ambient-only rumor is eligible. If the configured hub lacks a valid location ID, surface an authoring diagnostic and omit the physical map route. If the site is known but seasonally blocked, present the delay and allow a hub-based resolution. If the player has only a secondhand account, show an approximate lead rather than a precise pin. If all eligible destinations are excluded, do not force a reroll; preserve the lead as unavailable or close it as uninvestigated according to player choice.

#### Determinism and pool isolation

Ambient rumor selection must not consume the expedition location RNG stream unless the current architecture explicitly makes rumor presentation an expedition draw. Keep authored rumor scheduling and location selection distinct. If an optional location is selected, use the established seeded selector and stable candidate ordering. Equal state and seed must produce equal expedition candidates; rumor propagation timing remains with RumorSystem. This plan proposes no map-location generation and no new destination pool until the existing owner contract is reviewed.
### Pass 23B — Source-aware pool model and scenario matrix (DRAFT)

If ambient rumor presentation later requires an optional field location, the pool item must distinguish narrative eligibility from gameplay availability. A static origin hub can be valid for rumor display while having no expedition destination at all. The location selector should receive only actual location candidates; it should not parse rumor prose, choose a destination from a headline, or infer a direction from a witness's words.

#### Candidate contract

An optional site request should name a stable destination ID, requesting quest ID, required evidence type, access condition, season/world-phase condition, and whether the visit is required or optional. The candidate resolves against the authoritative location catalog. If the quest cannot provide a real ID and proof requirement, there is no candidate. Do not persist this request as a second quest record inside ExpeditionSystem.

#### Scenario matrix

| Hub/rumor state | Candidate state | Selector result | Quest/map result |
|---|---|---|---|
| no approved ambient hub | none | no ambient pool entry | no quest offer, no marker |
| approved hub, rumor heard, no field proof needed | none | no expedition request | hub conversation only |
| approved hub, clue suggests an authored site | site valid but undiscovered | do not force candidate | clue remains approximate |
| site discovered and eligible; visit optional | optional pool | may appear after mandatory reservations | journal says optional lead |
| site required by an accepted quest and eligible | mandatory pool | reserve under current priority contract | objective marks planned destination |
| site required but hard-excluded | invalid now | delay or clue fallback | journal identifies the block |
| site catalog link invalid | malformed content | reject with authoring diagnostic | never display a false map marker |
| site selected but player retreats before arrival | selected, unvisited | selector has no post-selection success | no discovery or evidence fact |
| rumor expired before expedition selection | no active narrative requirement | omit rumor-tied optional site | preserve only already durable quest facts |

#### Repetition and surprise control

Ambient information can feel repetitive if the same sound lead is rolled every expedition. Apply any repetition control at the authored-content presentation layer or through an existing encounter frequency policy. Do not add a rumor-specific expedition RNG stream. Secret locations remain secret under current discovery policy; ambient chatter may hint at them only if the authored clue justifies that hint. A surprise site cannot be introduced as the factual source of a rumor after the player reaches it unless the site evidence is authored to support that conclusion.

#### Fallback ownership

The quest owner decides whether “no destination” means wait, conversation, or inconclusive resolution. The location selector reports eligibility and reason. The map projects known and selected state. The rumor system owns propagation. Keeping these responsibilities separate makes the recovery legible: no valid expedition location is not a rumor failure, and an expired rumor is not a map deletion. Any host glue must preserve these typed outcomes rather than flattening them into a boolean `available` flag.
### Pass 23C — Selection and visibility stress cases (DRAFT)

This case bank protects the distinction among hearing an ambient account, selecting a site, visiting a site, and discovering evidence. These states should remain separate even when a future feature adds more generated content.

| Case | Pool pressure | Expected selector behavior | Player-facing statement |
|---|---|---|---|
| ambient report, no location link | any | no expedition candidate | “The report names no place.” |
| report gives approximate ridge, no valid catalog ID | optional slots available | no synthetic destination | “The direction is approximate.” |
| valid optional site, mandatory main-story site uses final slot | full | preserve mandatory site | ambient lead waits |
| valid mandatory evidence site, hazardous weather | any | reject now and return reason | “No safe route is available today.” |
| selected site, party withdraws before entry | any | selected state only; no visit | “The team turned back before arrival.” |
| site visited, no source evidence | any | preserve visit, no rumor verification | “The location was reached; the source remains unknown.” |
| rumor expired before site draw | optional | remove rumor-only candidate | prior witnessed record remains historical |
| two quests reference same hub, separate evidence IDs | limited capacity | share only if encounter supports both | objectives remain separately tracked |

If any existing selector lacks typed reasons, document the smallest adapter contract needed. Do not have the dialogue panel infer pool pressure. Do not expose the full hidden candidate list through map UI merely to explain why a site was omitted. A concise reason can be shown for a player-known candidate; undiscovered content remains undisclosed.

## Pass 24A — Expedition eligibility for authored field-guide triggers (DRAFT)

### Decision frame

The world-bible Part 46 field-guide question intersects expedition selection only when an authored travel encounter can award a guide entry. Current Core resolution returns `UnlocksFieldGuideId`; the inspected `ExpeditionEncounterBridge` patrol branch constructs a narrative resolution with morale and guilt but not that ID, and the combat host overload discards the ID output. Treat this as a candidate loss at the consequence boundary. Do not respond by adding field-guide locations to the selector or by forcing an encounter to appear every expedition. Plan28's ecology signal and the current expedition pools remain separate authorities.

### Pool and priority rules

Retain the existing expedition selection order and guarantee rules. A field-guide reward does not create a new mandatory pool. First satisfy active critical quest destinations and other established selector obligations; then satisfy character/faction and thematic content under current rules; then optional and rare entries. A field-guide encounter belongs to its existing authored location pool and its existing rarity. It can be selected only if the parent location and encounter pass their current eligibility checks. A quest that mentions an entry may reserve the authored encounter only if the quest already owns a valid reservation contract; this proposal does not add one.

**Exclusion rules:** exclude malformed choice data, unknown field-guide IDs, encounters whose parent location is not selected, and content blocked by campaign state. A previously unlocked entry may still allow the encounter when narratively appropriate, but repeated selection must not repeatedly grant a reward. If authored content is one-shot, its current resolved-state owner decides whether it can recur. Do not use field-guide completion as a hidden eligibility predicate unless explicit design and data say so.

### Scenario matrix

1. Valid optional encounter selected; resolution grants a valid locked entry: route the identifier to the existing field-guide owner, show local feedback, and record provenance through existing available channels.
2. Same encounter selected after entry is already unlocked: preserve encounter consequences; guide update is an idempotent no-op and does not duplicate the journal reward.
3. Encounter exists in data but parent location is not selected: do not mark it as failed or discovered; it remains unavailable this expedition.
4. Encounter choice grants an unknown ID: resolve ordinary valid choice costs/effects according to current Core contract, but reject only the guide grant and emit a content-integrity diagnostic. Whether invalid content should block the choice is a separate contract decision requiring owner review.
5. A quest requires a guide clue but no eligible location exists: keep the quest active and postpone the objective, provide a clue to an already selected equivalent only if an authored equivalent is validated, or report an explicit blocker. Never silently strand the quest.
6. A required location cannot spawn: follow the existing equivalent-location/delay/clue fallback contract; never replace with an arbitrary encounter solely to satisfy a Codex counter.

### Visibility and discovery

A map marker represents a selected and discoverable location, not an unlock ID. An entry’s Codex availability is a separate view over `FieldGuideCatalog`. The expedition map should not reveal a hidden destination because a travel choice references a guide reward. When the location is already visible, ordinary location naming and fog rules apply. Discovery should occur through the authored event, not from catalog load, expedition selection, or a save restore.

### Selection acceptance

A future implementation must trace the same valid choice through direct travel resolution, the bridge's patrol path, and the combat-added path. All paths should preserve the choice’s guide ID and call one existing owner once. For each path record: chosen encounter, selected location, quest constraints, resulting guide state, map state, and journal output. Seeded replay must produce the same selected content and outcome. The focused test plan should cover idempotence, invalid-ID rejection, location-not-selected behavior, and save/reload; select only the target owner tests under `TEST_POLICY.md`. No test is run by this planning edit.

## Pass 24B — Fallback behavior and unpredictability constraints (DRAFT)

Field-guide availability must not make expedition selection predictable. Do not promote all entries with outstanding unlocks into the mandatory pool. Instead, guarantee only content that an active critical quest explicitly depends on and that already has an eligible location contract. An optional Codex entry stays optional. Where content is rare, retries should be bounded by existing selector policy; deterministic fallback should preserve theme and difficulty, not simply inject the same encounter at every expedition start.

When no valid location remains, selection should: (a) verify whether an active critical objective actually requires the content; (b) use a data-authored equivalent location only when its tags and encounter are compatible; (c) otherwise delay the objective and surface a truthful journal/map state; (d) preserve any separate quest clue path; and (e) log the unmet content contract for authoring review. It must never award a field-guide entry because a location failed to spawn.

Review rarity at the encounter definition level, not through a second guide-specific weight table. A source event can be observed in more than one context only if all contexts reference the same stable entry ID and are narratively truthful. Add a per-entry trigger audit column to the existing catalog-integrity workflow only after verifying the current validator owner; do not create an independent validator in Plan18. The proposal's release gate is that each active quest objective has at least one reachable valid producer under its location and campaign constraints, while optional entries may legitimately have no producer until a later content wave.

## Pass 25A — Route eligibility through a changing storm window (DRAFT)

### Separate dispatch closure from in-progress risk

Part 46’s mid-route storm prompt intersects two distinct contracts already visible in source: dispatch can be blocked or forced through a `WeatherGateBlock`, and dispatch weather inputs provide speed/encounter multipliers to the estimate and expedition start. Separately, Year of Ash windows are queried by day, while the host’s weather-cascade route can adjust an active front’s encounter multiplier. A future audit must determine whether and how those authorities meet an already-active `ExpeditionState`. Do not reinterpret a destination gate as an in-route gate; do not silently retime an active sortie based on a newly loaded forecast.

### Selection and commitment model

At dispatch, keep the current candidate-location pools and priorities. Before allowing a weather-sensitive expedition, show the player: selected destination, route/gate status, forecast interval used, whether the estimate sampled weather for the whole sortie or only departure, and the currently supported response if conditions worsen. Do not promise a hold, camp, reroute, or abort action until an owner and UI command exist. A forecast-based warning is explanatory; it must not secretly alter destination weighting.

At an in-progress weather transition, if a current event seam is verified, evaluate only active expeditions whose route and travel phase satisfy an authored predicate. Affects-outbound, affects-looting, and affects-inbound are separate authoring choices. A target storm may delay one route, increase encounter hazard, close a gate to future dispatch, or merely expose the team; these are not interchangeable effects. The same transition should not both close a gate and force an unchosen detour unless the data explicitly describes both.

### Pool and fallback scenarios

1. **Storm forecast overlaps route, but dispatch remains legal:** show estimate and uncertainty; do not remove the location from selection.
2. **Gate blocks before dispatch:** preserve the existing reason-carrying block and force-cost decision. An equivalent location is valid only when it has an authored equivalence and supports the same quest requirement.
3. **Weather changes after dispatch:** keep the sortie’s current state under its existing owner until an approved dynamic update contract exists. A new encounter or route option cannot be injected from UI code.
4. **Quest objective requires a blocked destination:** delay it, reveal an authored clue to a currently selected/known alternate, or use the current plan’s equivalent-location contract; never mark it impossible without notice.
5. **Storm closes a destination while an expedition is already looting:** do not erase the target or force an instant return unless the owner defines a mid-route transition. Preserve player-visible objective state and record the blocked/changed condition at the correct time.
6. **No safe alternate is eligible:** keep the main expedition selector unpredictable and let the quest wait; do not spawn a free safehouse, deterministic rescue, or an unbounded retry loop.

### Map visibility and timing

A route or location can be selected only through current destination/map authorities. Weather projection may add an authored “expected conditions” annotation to an already visible destination; it must not reveal unknown nodes. If a storm causes a gate to close, future-dispatch visibility and in-progress sortie information must be separately represented. A marker should not imply the player can dispatch through a gate that the command owner will reject. Any temporary location requires a stable selection seed, explicit visibility state, expiry, and save contract before it becomes a quest dependency.

## Pass 25B — Deterministic interruption and resume cases (DRAFT)

If a future architecture introduces a mid-route storm checkpoint, it must not roll on every frame or on an unordered active-expedition dictionary. Use a stable campaign event identity and existing seeded RNG stream only if the current campaign RNG owner authorizes it. A resumed save must not reroll whether the team received the warning or whether the storm effect applied. One storm window can affect multiple sorties, but each affected expedition needs a stable per-window application identity to prevent duplicate effect on repeated day ticks.

The current hourly expedition tick advances stamina, rolls an encounter, handles vehicle breakdown, and advances route phase. Any intervention must define ordering relative to those operations. Example ordering for review—not a committed order—is: campaign day/weather transition; determine eligible active routes; record one warning; accept player command at a safe input boundary; apply effect through expedition owner; then process ordinary travel tick. If the event reaches the host after that tick, the warning text must not claim the player could have prevented an already committed outcome.

Selector acceptance requires paired cases: same seed/same initial state yields same affected routes and warning IDs; different route/phase eligibility does not receive the effect; gate force remains distinct from storm consequence; an already-surfaced encounter stays associated with its existing location; no valid alternate triggers the documented delay; save/restore between storm trigger and player response applies the transition exactly once. This proposal does not add implementation/tests yet and remains gated on a source-level integration map.

## Pass 25C — Phase eligibility matrix and no-stranding outcomes (DRAFT)

| Active phase at transition | Candidate handling | Player-facing truth | Do not do |
| --- | --- | --- | --- |
| Not dispatched | Gate/forecast can affect availability under current dispatch owner | “Blocked,” “forceable,” or current forecast wording | Treat a blocked target as selected already |
| Outbound | Only apply a validated active-route effect; keep destination identity | “The route condition changed” if the owner records it | Replace the destination behind the player’s back |
| Looting | Preserve current objective/result; present only supported continuation choices | State whether the team has secured anything | Despawn the location or erase loot after entry |
| Inbound | Apply only effects that the return route is authored to receive | Give truthful return ETA/state if current owner has it | Send the team back outbound automatically |
| Camp | Use the existing camp owner and its own rules | Show current camp condition | Treat `Camp` as ordinary travel ticking |
| Completed/failed | No new route effect; report stored outcome | Existing terminal feedback | Rewrite the result after the fact |

A storm transition must not strand a quest that can only be completed at a location the system now blocks. Before reserving a required location, the quest contract needs one recovery route: (a) a compatible alternate destination already present in authored data; (b) delay until the current block expires; or (c) a clue/reconstruction route that uses a location the player can still reach. The selector must not repeatedly retry the same invalid destination every expedition start. If the delay is the selected fallback, expose when the player can reconsider only if the weather owner can compute that fact; otherwise say the route is currently closed and let ordinary status refresh handle it.

The expedition remains unpredictable because storm warnings do not grant the player a deterministic safe location. Optional locations retain ordinary weighted selection. Critical active-quest guarantees stay first, but a guaranteed destination can be satisfied by a verified alternate only when the quest’s required evidence is equivalent. Recent discoveries and thematic choices continue under existing weights once hard obligations are met.

For a storm-related hidden location, discovery may be triggered by a genuine environmental clue or returning report. A storm alone cannot reveal it. A quest-only encounter inside a known parent location should not create an additional map icon unless the content contract explicitly makes it a separate destination. Its expiry must be based on a real campaign clock and deterministic save state. If it cannot be selected because another mandatory objective consumes the slot, preserve a later opportunity instead of flagging the clue as consumed.

### Selection review checklist

For every candidate storm scene, reviewers answer: Is the parent location selectable now? Which phase permits it? Is the content optional or critical? Is a gate closed, forceable, or simply hazardous? Can a surviving team return to the same route? Does a successful quest choice consume the only available location slot? Is a fallback truly equivalent? Does the map disclose more than the player knows? Can save/load reproduce the selection? Any “no” answer must have an explicit postponement, alternate clue, or accepted optional miss.

## Pass 26A — Severity-aware destination availability and retry matrix (DRAFT)

The World Bible Part 47 severity ladder gives the selector a disciplined way to separate narrative intensity from route eligibility. A storm message may be ominous while the target remains dispatchable; a closed weather gate is a mechanical denial even if the prose is calm. Never use dramatic wording to imply a stronger block than the route owner returns.

| Severity / source result | Selector response | Player option | Fallback eligibility |
| --- | --- | --- | --- |
| Forecast only; route open | Keep destination in its ordinary pool and show estimate context | Dispatch or decline | No alternate needed |
| Encounter hazard projected | Keep route available; estimate may warn | Choose ordinary stance/gear | Alternative route only if current catalog supports it |
| Gate blocks dispatch | Exclude from currently dispatchable pool | Wait, choose a real alternate, or force through at authored cost | Only a validated equivalent destination satisfies an active objective |
| Active sortie receives a verified delay effect | Keep original target reserved; update ETA only through its owner | Respond only if an active command exists | Do not silently substitute target |
| Route result becomes terminal | Remove the sortie from active pool | Resolve/debrief | A later quest retry follows current cooldown/window rules |
| No consumer confirms a storm effect | Do not alter availability | Preserve standard route behavior | Report as a premise gap, not a player-facing blocker |

### Retry and guarantee budget

For a critical active quest, guarantee one reachable way to continue the objective across the content window, but do not guarantee that the original destination appears in every expedition. A valid alternate must preserve the objective’s semantic requirement: a return report can substitute for a site visit only if the quest is about testimony; it cannot substitute for a physical sample or repair. When a route is closed, retries should be scheduled by the actual gate/weather owner, not a hard-coded quest counter. If the selector cannot know when a closure ends, it should keep the quest blocked with an explicit reason and leave the rest of the expedition pool intact.

Optional, secret, faction-specific, and thematic pools retain their existing priorities after mandatory progression checks. Storm scenes draw only from selected parent locations. A hidden encounter on a known node does not become a new map icon. An unavailable scene remains unconsumed, so the next eligible expedition can surface it. Add no special storm weight that makes all players encounter the same scene on Day N.

### Route type and authored storm class

- **Black blizzard:** a visibility report may matter only at a location with an authored landmark/marker; no global map loss is implied.
- **Ash fallout:** dose consequences require the existing radiation/exposure route; do not infer gear damage from a storm label alone.
- **Ice fog / thaw flood:** access restriction follows a real gate or ice-road owner. Future dispatch and already-active sorties are separate eligibility checks.
- **Thermal inversion:** shelter radon or expedition exposure is applied only by the verified atmospheric consumer; the selector does not add a separate hazard.
- **Faction dust / siege weather:** danger and faction presence use existing territory and encounter predicates; no new faction is spawned to fill a location slot.

These are content audit prompts, not a claim that every weather class currently reaches every subsystem. Before each location can be called a storm destination, verify its route, owner, data ID, map state, and persistence source.

## Pass 26B — Selector arbitration, determinism, and map contract (DRAFT)

The expedition selector is a content-planning boundary, not a second world simulation. It receives an immutable request containing campaign identity, expedition sequence or seed, relevant quest requirements, current eligibility facts, and the requested location budget. It returns a selection result with chosen locations, visibility classes, exclusion reasons, and unmet requirements. The existing campaign and expedition owners remain authoritative for time, resources, character state, and quest progress. If those owners cannot supply a fact, the selector treats it as unknown and does not synthesize it.

### Priority is policy, not a numeric lottery

Priority tiers are evaluated in order: mandatory active-quest locations; critical progression requirements; character and faction obligations; recently discovered locations with a still-valid follow-up; thematic locations; optional exploration; rare surprise locations. Within a tier, eligible candidates can be selected by the established deterministic RNG contract using a stable candidate ordering. A weighted draw must not allow a lower tier to displace an eligible mandatory item. A tier may yield fewer items than its budget, allowing the next tier to fill the remaining capacity. The result records why each candidate entered or was skipped so that QA can distinguish legitimate absence from selector drift.

The user-facing request specifies the expedition start as the map-refresh point. The selector therefore produces one coherent map snapshot at dispatch; it does not reshuffle every time the panel is opened, after a save is loaded, or when an unrelated dialogue is read. A reroll is a new expedition request and consumes only the approved seed/sequence input. Repeated calls for the same request must return the same selection and visibility result. Candidate enumeration is ordinal and independent of hash iteration order.

### Eligibility and exclusion contract

A candidate is eligible only if its campaign phase, prerequisites, location type, faction context, temporary lifetime, and content availability all agree. Exclusions include duplicate location IDs; locations already consumed by a one-time quest; mutually exclusive chapter variants; inaccessible map edges; unmet safety requirements that are explicitly mandatory; and temporary locations whose expiry has passed. Optional hazards may make an eligible location risky, but must not be misrepresented as a hard exclusion. Quest-required content cannot be excluded by a novelty cap or thematic preference.

The plan distinguishes four map states. **Visible** locations appear as named destinations. **Rumored** entries surface as a clue, not a navigable pin. **Discovered** entries have been encountered but may not currently be selected. **Hidden** content has no map marker until its declared discovery condition is satisfied. A location may be available but hidden; map visibility never proves accessibility. The journal and expedition map must use the same returned selection snapshot.

### Requirement repair and fallback

If a mandatory candidate fails validation, emit a structured requirement failure before finalizing the selection. Repair follows a declared fallback chain: choose a semantically equivalent location from the same quest packet; select a clue location that can reveal a supported alternative; or postpone that quest objective and make the delay visible. Equivalent means it can satisfy the same authored objective and downstream facts, not merely that it has similar art or biome tags. If no fallback is authored, preserve the rest of the expedition and report the objective as delayed/blocked with a recovery condition. Never silently mark the quest complete, drop it from the map, or substitute an unrelated site.

### Capacity and conflict example

Suppose three active objectives require two named sites and one clue site, while the optional map budget is four. Both named sites reserve capacity first; the clue site is included if its fallback is needed or the clue itself is the only legal continuation. A recently discovered shelter may fill one remaining slot. The thematic pool then fills any remaining capacity, followed by optional and rare candidates. If the total distinct eligible set is smaller than the requested budget, return fewer locations and a reason. Do not repeat a site under a different label to make the map look full.

### Data and diagnostic requirements

Each candidate definition needs a stable ID, pool membership, availability predicate, map visibility policy, rarity band if applicable, repeat policy, quest/faction links, and fallback references. Diagnostics should expose request seed/sequence, ordered candidate IDs, exclusions with reasons, requirement repairs, chosen IDs, and final visibility. Logs must avoid dumping unnecessary player text. These fields are design requirements until matched against the actual catalog schema; this plan does not authorize a parallel location registry or new persistent selection store.

## Pass 27 — Evidence-site selection for “The Last Dry Strike” (DRAFT)

This extension converts the match-manufacturing story into a routeable content packet without claiming that an assay clue deserves a new map system. It contributes at most two required evidence roles—an original work record and a later storage-test record—and one optional domestic witness. Each role must be bound to a valid current destination or an approved in-shelter interaction before it can become a quest requirement. New location names below are role labels, not catalog IDs.

### Location roles and physical logic

| Role label | Narrative use | Access class | Required evidence | Fallback |
| --- | --- | --- | --- | --- |
| Batch-room archive | First assay card and author attribution | Existing shelter/workshop surface if verified | Original dated test record | Discover through existing archive or journal route |
| Dry-store or printroom annex | Storage-condition comparison, not a recipe | Optional expedition destination only after a real map reference is found | Second sheet or a traceable copy | Witness statement that narrows what can be known |
| Clinic transfer shelf | Optional consequence: where the carton was held | Character/shelter visit; never a mandatory expedition | Delivery notation or remembered date | Keep the delivery chain unresolved |
| Route-side notice board | Late callback showing the report is being used | Existing map destination only if verified | Published correction ID | A journal acknowledgement at return |

The name “dry-store” must not create a navigable site by itself. The map owner supplies stable nodes, the expedition owner supplies eligibility and route cost, and the quest owner supplies required roles. If no available destination hosts the second record, keep the objective local or use the clue fallback. Do not add an invented temporary node, duplicate an existing workshop, or turn a shelter room into a surface expedition destination.

### Candidate-pool rules

An active mandatory evidence role is selected before optional exploration only if an eligible destination is confirmed. A canonical site does not become newly available merely because the quest is active. Character/faction variants follow their own valid prerequisites. Recently discovered sites may provide a clue only when their authored content contains the relevant record. Thematic industrial sites and optional exploration sites fill remaining map capacity after required roles are safe. Surprise selection may add texture but never remove a mandatory clue.

Rarity is not an appropriate control for required evidence. Use authored eligibility and objective state. If the second evidence destination competes with another mandatory quest, the result either includes both under the existing capacity contract, selects a declared alternative, or reports a visible delay. It must not silently replace the assay site with a visually similar but semantically empty building.

### Expedition timing and repeat behavior

The assay mystery does not require a storm modifier, a new travel clock, or an expedition-only item. A standard route result may expose the document if the existing scavenging/narrative bridge can do so; otherwise the record is a fixed location interaction, not an invented loot roll. A failed-but-survived sortie cannot secretly destroy a required record without a declared quest transition. Reopening the map during one expedition returns the same selected sites. Reload resumes the same record availability and does not reroll the evidence to hide it.

### Visibility and player expectations

The map can show a visible named destination, a rumor with a clue, or nothing until discovery. A journal objective must use the same visibility contract: “find the dry-store note” is misleading if the player only has a rumor and no pin. If the location is known but not eligible, say it is unavailable and identify a recovery condition only when the owner provides one. The surface map and shelter/archive panel must not disagree about whether the site was found.

### Acceptance scenarios

- The required first record has a valid in-shelter route; no expedition is forced just to begin the story.
- The second record has exactly one eligible destination; it is reserved for this objective if the existing map capacity permits.
- The second record has zero valid destinations; the player receives a witness/clue fallback and a visible unresolved conclusion.
- Another quest reserves the same location; both requirements remain possible, or one is transparently delayed.
- A destination is discovered before the quest; the content can be read or reread without duplicating progress.
- A location is already consumed by a one-time encounter; selector returns its authored substitute or clue route.
- The player refuses the expedition; the quest remains resumable or resolves with uncertainty according to the packet.

The production estimate is low for a fixed in-shelter evidence scene and moderate for a new expedition destination because it entails map, danger, route, art, and discovery validation. This plan recommends the local-first candidate. Adding a dedicated industrial map node belongs to an expansion only after the existing map census finds a real density gap and an exact canonical ID can be registered.

### Pass 27B — Selection traces, reservations, and post-return copy (DRAFT)

These deterministic traces exercise the earlier role matrix. They are selector specifications, not current guarantees that these sites or hooks exist.

**Trace A — Local-first opening.** The player accepts the investigation at the shelter archive. The archive record is available immediately. An optional expedition candidate would lead to the printroom annex, but that location has not been verified in the map catalog. Result: no expedition destination is reserved or fabricated; the quest remains in progress and gives a clue to check the transfer ledger during the next shelter visit.

**Trace B — Eligible second record.** A premise audit finds a canonical workshop location that contains the dated storage sheet and is already expedition-eligible. An active quest now requires this evidence. The selector places it in the mandatory tier, records the quest reservation, and fills remaining slots from eligible thematic and optional tiers. The map shows the real location name and objective hint. A rarity roll cannot displace it.

**Trace C — Shared destination.** Two quests require the same actual depot but different documents. The selector chooses the site once, attaches both objective references, and prevents duplicate map pins. If both documents are physically available in one visit under existing interaction rules, both objectives can progress. If the second document requires a distinct encounter instance, the selector requests another legal slot or delays that objective visibly; it does not treat one document as both.

**Trace D — Destination unavailable.** The site is already consumed, the route is blocked by a verified hard gate, or its content is no longer eligible. The selector first tries the exact authored equivalent. If none exists, it adds the declared clue alternative or returns a delayed requirement with recovery criteria. The quest journal reports the reason at the player’s next safe interaction. A lower-tier surprise site is not a substitute.

**Trace E — Save/resume.** The expedition map has already been generated and saved by the current owner. Loading and reopening the map reuses that selection. If the save schema has no selected-site snapshot, the plan cannot assume replay safety; the integration must first determine whether the existing seed/sequence reproduces the same result without consuming unrelated RNG.

### Output contract and diagnostics

A selector result for this story should include request identity, ordered candidate IDs, eligible/ineligible status, each exclusion reason, quest requirement reservations, fallback chain taken, map visibility class, and any unresolved requirement. Do not write a new save file or selector registry to hold these fields; they are design diagnostics until an existing route exposes the needed data. UI copy is generated from the result’s facts: “Archive copy available here,” “The transfer shelf is not accessible today,” or “No second record is currently located.” Avoid “location missing” language for a hidden site the player has not discovered.

### Content-density and budget rules

Do not reserve more than one mandatory expedition site for the first quest chapter. Keep one location role local so a low-resource campaign can begin the story without a costly trip. If the authored world has no appropriate map destination, prefer a shelter scene plus a witness clue over a new node. A later expansion can create a dedicated annex if a location census shows a real gap and its route, danger, resource yield, encounter structure, and visual identity are distinct. The quest must not inflate destination count with a room that has no independent gameplay purpose.

### Reuse, replay, and exclusion examples

The selector can reuse the evidence-pair packet for other craft mysteries, but only those with a real location and distinct evidence roles. It cannot reuse this story’s “storage failure” label for every industrial record. Exclude sites that merely contain generic workshop props, already-consumed once-only evidence, or a contradicted map identity. Optional ambient tools may vary; evidence-bearing object placement and wording remain authored and stable. On repeat campaigns, the same deterministic request may produce another optional location, but required records and their fallback do not disappear randomly.

## Pass 28 — Shelter-bound quests and honest location selection

This pass adapts Part 46’s caregiving prompt to the selector architecture while respecting the story’s actual scope. “The Cup on the Rail” is a shelter character quest, not an expedition. Its required scene is a shelter interaction, with optional bedside and duty-roster conversations where supported. It must not consume a surface destination slot, add a hidden map icon, or manufacture an expedition just to make a quest fit this plan.

### Location role assignment

| Role | Story use | Selector behavior | Visibility |
|---|---|---|---|
| Permanent shelter hub | Quest offer, conversation, journal return | Existing route; outside expedition pool | Already known; never a newly discovered tile |
| Bedside scene anchor | Optional patient conversation | Existing ward/shelter context only if host supports it | No expedition marker |
| Duty-roster board | Optional context about the caregiver’s prior role | Query current roster at visit time | No new destination |
| Expedition location | None required | No selection or reservation | No map change |
| Future clue site | Not part of this packet | Add only for a separately audited evidence objective | Ordinary discovery and fallback rules |

The location service should accept a quest requirement only after content declares it as a typed requirement. A narrative mention of the bedside is not sufficient to create a destination. Here the requirement is “shelter interaction available,” not “surface location spawned.” The selector therefore has no row for this quest in any expedition pool. This guards against treating every conversation as a destination and then solving the resulting overpopulation with ranking rules.

### Eligibility, priority, and guarantees

The general expedition selection order remains: mandatory active quest destinations; critical progression sites; character/faction destinations; recently discovered candidates; theme-matched locations; optional locations; rare surprises. A shelter-only quest contributes nothing to the expedition-required tier. It remains playable during an expedition only if the existing host supports that concurrency. If the shelter panel is unavailable during a sortie, the quest remains pending and explains when it can be revisited; do not relocate a bedside scene outdoors as fallback.

For a future quest that genuinely requires travel to support a care handoff—such as retrieving an authored medical record—the location requirement must name its evidence role and explain why expedition access is necessary. Reserve the candidate before optional selection, provide a deterministic clue if hidden, and offer an equivalent evidence path or explicit delay if invalid. Do not guarantee that a patient’s condition is worsening, because care assignment does not encode that state.

### Exclusion and fallback rules

- Exclude shelter interactions from map generation unless the existing map authority represents the shelter as an expedition destination.
- Exclude a quest location when its only rationale is dialogue flavor, imagined travel, or a missing event bridge.
- If the required host scene cannot render, keep the quest available in an existing journal/hub surface if that route exists; otherwise retain it unresolved and report the integration gap.
- If a real evidence location becomes invalid, use an authored equivalent only when both locations provide the same objective evidence; otherwise delay and surface an explicit clue. Never silently remove the objective.
- If optional slots are occupied, drop the optional location, never the shelter quest. The selector owns neither assignment nor dialogue outcome.

### Replayable selector traces

**Trace S — shelter-only:** “The Cup on the Rail” is active; an expedition starts. Candidate building sees no required destination from this quest and selects the ordinary expedition set. The journal still lists a shelter conversation. Returning permits the scene. Expected invariant: candidate set and selector random stream are unchanged by this quest.

**Trace T — future evidence objective:** a separately audited quest requires one record site. Dispatch reserves one eligible record site and records the authored requirement ID. Theme and optional pools fill remaining slots. If the site is absent from catalog, deterministic fallback selects a documented equivalent; if none is valid, delay only if the quest owner supports that state. Expected invariant: no silent dead end and no duplicated destination.

**Trace U — host contention:** the player returns from an expedition while shelter dialogue is inaccessible. The selector invents no substitute. The quest remains visible with a truthful access note; availability is re-evaluated on the next eligible visit.

### Map and UX requirements

A quest panel must distinguish “available at shelter” from “appears on the expedition map.” The latter is false here. Do not display a stale marker from a prior sortie. A bedside scene uses an ordinary conversation affordance and accessible subtitle/interaction feedback. Selector explainability should say “no expedition location required,” rather than listing a fabricated null destination or generic fallback. Hidden-location rules apply only to actual map requirements.

**Reusability:** the role table and shelter-only trace apply to character conversations, shelter mediation, relationship callbacks, and roster debriefs. They also identify when the selector should not be involved. **Production cost:** low if hub dialogue routes exist; medium if quest availability cannot be queried from shelter UI; high if a map-backed surrogate is proposed. **Core/expansion:** the shelter scene is core scale; a field-evidence objective belongs in an expansion and requires a separate content/selector audit.

**Acceptance gate:** demonstrate that starting this quest changes no expedition candidates, reserved locations, map visibility, random draws, or expedition save state. Then prove that any later travel objective is guaranteed or explicitly delayed by current quest and selector owners. No selector implementation or map content is authorized here.
## Pass 29 — Micro-location binding census, eligibility and map guarantees

The Bible’s location-coverage prompt intersects two distinct selection stages that this plan must keep separate: selecting an expedition destination and selecting an encounter after a destination is known. The current static inventory is 179 location records and 28 micro encounter definitions. Three encounters require exact destination IDs and match records in locations.json; 25 are unbound. Micro definitions are loaded into NarrativeEncounterSystem’s common catalog. GetEffectiveWeight rejects a bound encounter unless the caller’s location ID exactly equals requiredLocationId, while an empty requirement has no site restriction. These facts do not prove that every catalog location is expedition-eligible or that each micro encounter is actually reachable in all runtime routes.

### Two-stage selection contract

**Stage A — destination selection.** The expedition/location authority chooses a valid destination, respecting active quest requirements, critical progression, faction/character needs, prior discoveries, theme, optional pool, rarity, exclusions, and fallback. Do not add an encounter record to this pool. If an active quest requires one of the three exact sites, only its canonical location ID can satisfy that objective; an alias or similarly named location is not equivalent without an audited mapping.

**Stage B — encounter selection.** Once the host supplies the chosen location context, NarrativeEncounterSystem constructs eligible encounter candidates. Exact-site micro records require exact ID equality. Global micro records compete in the same weighted candidate set as other eligible definitions; they are not guaranteed because they have no requiredLocationId. Weather gates, stance multipliers, danger minimums, depletion, and available seeded RNG also affect selection. Do not promise an encounter merely because its definition exists in JSON.

### Binding coverage matrix

| Census class | Count in reviewed static files | Meaning | Action |
|---|---:|---|---|
| Exact binding to a known catalog location | 3 | Two-sided ID link exists in locations.json | Verify that location is expedition-reachable and selector can pass the exact ID |
| Empty requiredLocationId | 25 | No exact-site restriction in EncounterDefinition | Treat as global candidate content, not location coverage |
| Nonempty reference absent from location catalog | 0 among these 28 | No static dangling requiredLocationId in the reviewed set | Keep this as a validator invariant |
| Destination without a narrative encounter | Not determined | Requires definition of “narrative encounter,” all-catalog join, route reachability, and consumer trace | Run a complete location × encounter consumer census before adding content |

The last row is intentionally “not determined.” 179 minus 3 would be a false gap count because the 25 global encounters can be eligible at multiple sites, and other encounter catalogs may also bind to locations. Conversely, a global candidate does not establish thematic adequacy at a particular destination.

### Guarantees, exclusions, rarity, and fallback

A mandatory active-quest site is guaranteed only at Stage A, by the location owner, while an encounter at that site remains a separate question. If the quest requires a specific micro record rather than merely the site, the quest must reserve or guarantee the encounter through a verified mechanism; current JSON presence alone is insufficient. Prefer an equivalent evidence path when the site is unavailable. Otherwise delay visibly through the quest owner or provide a clue. Never silently declare the objective impossible.

Exclude exact-bound encounters at every nonmatching site. Do not canonicalize case, aliases, or punctuation inside the weighting method unless the authoritative location resolver already does so and a focused API audit confirms that caller behavior. Empty-bound encounters remain eligible only under all other filters. A depleting choice removes the encounter according to current saved state; do not treat it as a rerollable rare item. If no eligible encounter remains, return the existing no-encounter outcome rather than force a micro definition that violates its location contract.

To prevent global micro records from crowding destination-specific narrative, measure their selection frequency and per-location candidate counts in a deterministic replay. Only change weights or add a pool tier after evidence shows a quality or starvation problem. A “featured local micro encounter” selection tier would be a new arbitration policy and needs its own owner decision, fairness rule, seed stream, and migration assessment.

### Replay examples

1. **Hospital chapel:** Stage A selects the canonical abandoned_hospital record. Stage B may include micro_hospital_chapel_ledger because its required ID matches. At any other location its weight is zero.
2. **Flooded depot:** location_flooded_subway_depot admits micro_depot_undertow_raft_line. Selection remains weighted; exact binding does not imply forced arrival.
3. **Checkpoint Gamma:** loc_garrison_checkpoint_gamma admits micro_gamma_levy_board under exact matching.
4. **Generic roadside expedition:** the three bound encounters are excluded. Up to 25 unbound micro records may be candidates, subject to their other weights and filters; do not report this as 25 guaranteed events.
5. **Unknown location string:** bound records fail closed. Global records can still be eligible unless other conditions exclude them. Instrument the mismatch as a diagnostic in a future audit rather than silently correcting IDs.
6. **Quest reservation with missing site:** choose a verified equivalent site only if it satisfies the same authored objective; else reveal an alternate clue or delay the quest. Preserve a deterministic selection order.

**UX and cost:** show the destination marker only when Stage A selects or reserves that site. An encounter title should not appear as a map pin before selection. The low-cost first slice is a static join report; medium cost is a selector explanation panel and deterministic frequency telemetry; high cost is guaranteed encounter reservation. One location census belongs in core validation; additional encounter families can be expansion content after the census reveals authored need.
## Pass 30 — Region-to-destination crosswalk before selection weights

### Verified shapes

The region catalog has eight named records and sixteen POI labels, two per region. The canonical travel map has 22 nodes and 68 routes; locations.json has 179 rows. Neither node definitions nor location rows carry a region field. The validator explicitly treats region POIs as catalog-local labels, so none may be passed to destination selection as a known node ID. The literal intersection between these labels and the reviewed node/location IDs is zero. This is not evidence of zero destinations per region; it means the current data contract has no proven join. Cartography UI projects WastelandMap knowledge, while the separate mutable region-survey API is not the host route found in this review.

### Three selection stages, one canonical location owner

**Stage 0 — authored regional context.** The region catalog supplies display name, terrain, hazard labels, scouting difficulty, and its own POI labels. These describe regional identity; they are not travel edges or expedition destinations.

**Stage 1 — campaign map graph.** WastelandMap supplies canonical nodes, routes, fog/knowledge, discovery, and travel reachability. Route choice follows this graph and existing lock rules. A quest-required map destination needs a real node ID and reachable route or a quest-owned fallback.

**Stage 2 — expedition destination and encounter.** Select a reachable node by active quest requirement, critical progression, character/faction need, recent discovery, theme, optional content, and rarity. Once dispatched, encounter selection receives the canonical node ID. Exact-bound encounters may appear only when IDs match; global encounters remain subject to their own weights and filters. The region catalog cannot bypass either stage.

### Density report contract

Until a crosswalk exists, publish distinct quantities:
1. Region metadata: eight region records and sixteen authored labels.
2. Travel graph: 22 nodes and 68 route records.
3. Verified regional destination density: not computable from the current direct fields.
4. Narrative coverage: computed only after matching encounter eligibility and location context across catalogs.
5. Player-facing access: reachable destinations by campaign phase; a locked or unconnected node is not currently selectable.

Do not count two labels per region as two reachable destinations. Do not claim 179 minus 22 is a regional gap. “Low density” thresholds require a design authority; equal-sized label lists do not justify a threshold.

### Binding decision options

**Option A — canonical travel-node region ID:** add a region_id to each WastelandMap node. The graph owns destination membership; map_regions remains the regional descriptor. Validate that region IDs resolve and that each travel node is assigned or explicitly global. This answers density for travel nodes, not all 179 location records.

**Option B — location-catalog region ID:** add a region reference to locations.json and explicitly declare which records are expedition destinations. This risks duplicating membership for the 22 canonical travel nodes and falsely classifying off-map sites. Use only if locations.json is ratified as regional destination authority.

**Option C — explicit relation catalog:** map region IDs to existing destination IDs. This avoids changing records but creates a second crosswalk authority that needs one owner and validator. It is not a default workaround.

Keep these as unresolved alternatives until the foreman/architecture owner selects one and claims exact files. No weights, rarity, map pins, or region-required quest can proceed before that decision.

### Guarantee and fallback

Guarantee a quest location only after its canonical node ID is known and reachable. Replace it only with a node proven to satisfy the same objective; otherwise give a clue or delay visibly through the quest owner. “A POI in Ash Valley” is underspecified until the mapping identifies eligible nodes. A region name cannot serve as fallback node. Optional sites may be dropped when slots are full. Order candidates by stable ID before existing seeded selection.

**Acceptance:** all membership IDs resolve; route connectivity checked from valid starts; regional quests cannot select unreachable nodes; map visibility agrees with selection; empty candidate sets recover explicitly; same-seed runs choose the same destinations. Build the join once at catalog load or in an offline report, not on every UI refresh. Static reporting is low cost; adding a single node-owned crosswalk is medium; broad ownership/topology changes and phase-aware rarity are high. No data or selector change is authorized here.
### Pass 30B — Selector traces after a region crosswalk is approved

These traces define expected behavior without pretending a crosswalk currently exists.

**Trace A — no mapping yet:** an active quest asks about the Ash Valley chart. Stage 0 returns region metadata and local POI labels. Stage 1 has no node membership relation to query. The selector does not guess from the loc_ prefix, display-name similarity, hazard, or nearest coordinates. The quest remains an archive investigation; no route slot is reserved and no map pin is created.

**Trace B — one approved node mapping:** after an explicit decision, one canonical node has region_id reg_ash_valley. Player fog state says unknown. The node is not offered as a guaranteed quest destination unless ordinary map discovery/access rules permit a clue or route. The selector may include a rumor clue under the existing authored information path; a hidden mandatory objective needs a deterministic discovery path or must remain delayed.

**Trace C — reachable regional quest:** the mapped node is discovered and reachable from the active origin. The quest’s canonical node ID enters the mandatory quest tier. Selection reserves it before optional/theme candidates. If there are multiple valid nodes, use stable ordering and the current seeded substream to choose among equivalent nodes; never randomize between nodes that have different story evidence.

**Trace D — invalidated route:** map knowledge, weather lock, or progression changes before expedition dispatch. Revalidate the reservation at dispatch. If invalid, choose another equivalent node if one exists; otherwise return a clue/delay to the quest owner. Do not silently substitute a location from a neighboring region.

**Trace E — selection succeeds, encounter does not:** the destination is guaranteed, but the encounter remains subject to existing encounter filters. If the quest requires a specific encounter, destination selection alone cannot satisfy it. Add a verified encounter guarantee or redesign the objective to accept another canonical evidence source.

**Trace F — region has zero reachable nodes:** report zero only after the crosswalk, graph reachability, and player-phase filters have run. Optional content may be withheld. A mandatory quest must be delayed or given an equivalent clue; no selector algorithm can manufacture a route that the map owner does not contain.

### Explainability output

For each candidate, an internal trace should include region relation source, canonical node ID, route reachability, fog/lock state, quest priority, exclusion reason, encounter requirement, and deterministic tie-break rank. The player sees only useful status (“not chart-linked,” “identified but unreachable,” “route available”), never internal scoring or pseudo-destinations. Trace storage belongs in bounded diagnostics or test output, not the save file. Performance must remain proportional to the bounded node set; precompute region membership on catalog load after validation and invalidate only when static catalogs change.

**Rollback:** if the region relation is withdrawn, remove region-based ranking and restore ordinary map/quest candidate construction. Existing map nodes, fog, route state, and saves remain unchanged because the mapping is static content. This rollback is feasible only if no quest has written region IDs into durable player state.

## Pass 31 — Arrival Windows, Stops, and Clue Substitutions (DRAFT)

### Selector contract

A caravan stop should not be modeled as an expedition destination merely because it is a location in a route catalog. The route network and expedition selector answer different questions. The route system owns scheduled movement, route risk, and arrivals; expedition location selection owns the set of places available to a player at expedition start. The proposed integration keeps this distinction. A quest may request a clue or visit opportunity, but the selector must return an eligible authored location through its existing contract or report that the quest is delayed. It must not fabricate a POI or make a route stop visible before the map owner says it is discovered.

For a future quest-aware expedition offer, define these candidate classes as selection annotations, not new world pools: (1) hard-required active objective locations; (2) critical progression locations; (3) faction/character opportunities; (4) recently discovered locations; (5) thematic matches; (6) ordinary optional sites; and (7) rare surprise sites. Each class is evaluated against the current expedition's region, route availability, already visited/locked/expired state, player map knowledge, and any explicit quest constraints. Hard-required means “must be possible,” not necessarily “always visible.” The plan must preserve the user's unpredictability goal by fixing eligibility and guarantees while allowing seeded choice among eligible optional content.

Priority and fairness:
- Resolve hard requirements first. If several active quests compete, use authored criticality and stable quest/location IDs as tie-break inputs; never rely on dictionary enumeration order.
- Reserve only locations that can be reached in the selected expedition envelope. Avoid showing mutually exclusive distant stops as simultaneous options if travel rules permit only one.
- Add at most a bounded number of optional thematic and rare sites after guarantees. When the map has few valid candidates, return fewer options rather than duplicating a site or inserting an invalid substitute.
- A location already visited may reappear only where its authored revisit policy allows it. Repeatable encounters require explicit cooldown/rotation data from their owner.
- A quest-only location stays hidden until its discover condition; if discovered, visibility follows the canonical map knowledge rule rather than an expedition-local reveal cache.
- Quest-specific place availability must be rechecked at expedition start and when the player commits, because the caravan may have departed or the relevant objective may have been resolved.

Fallback matrix:
1. Equivalent location: use only an authored replacement with matching objective capability, access constraints, and reward class. A textually similar place is not sufficient.
2. Clue substitution: emit or expose an authored clue at a currently reachable scene, then advance the investigation to a supported alternate target.
3. Delay: keep the quest active or blocked with an explicit reason and a retry condition tied to a later eligible arrival.
4. Clean expiry: use only when the quest design promises a time-limited window and tells the player the consequence.
Never mark a required step complete because selection failed. Never let a missing optional pool suppress the full expedition.

Illustrative, non-canonical scenario: a route-bound trader is expected at a stop but the actual arrival has not been confirmed. The expedition selector must not place “caravan camp” on the map based only on rumor. It may offer a survey site containing an authored route clue if already eligible. After the route owner confirms an arrival and the map/quest owners expose a valid visit opportunity, a trade scene can appear. If no stop is reachable, the quest can wait for the next supported visit window; the player receives a truthful journal update.

Rarity must be data-authored and bounded. A surprise encounter is a weighted option only after hard requirements, with a per-expedition cap and no repeated selection on a stable seed unless the encounter's repeatability contract explicitly allows it. Stable seed inputs should include campaign seed, expedition sequence, and canonical candidate IDs through the existing RNG contract. Do not seed from time, route traversal hash order, or a new private random stream.

Visibility states: hidden, rumor/clue-known, map-discovered, available this expedition, unavailable with reason, visited. These are design labels; implement by projection from existing map and expedition state, not by adding a parallel discovery ledger. UI should distinguish “not known,” “known but not available,” and “available now.” Avoid exposing the exact rare pool through a probability readout.

Minimum viable version: one active quest request, one required candidate, one equivalent or clue fallback, one optional candidate, deterministic tie breaking, and visible unavailability feedback. Defer multi-quest optimization, weighted pity timers, route-conditioned regional ecology, and player-configurable expedition decks. Dependencies: confirm the live selector API, the canonical map discovery owner, route-arrival event exposure, quest state representation, and save/replay contract before implementation. No code or data changes are authorized by this design pass.


### Pass 31B — Candidate audit rows and selector output

Before tuning rarity or adding candidates, produce one reviewable row per proposed place:
- canonical location ID and owning catalog;
- whether it is a route destination, expedition site, shelter encounter, or map rumor;
- existing availability/discovery rule and revisit policy;
- quest capability (which objective can actually be completed there);
- route/region vocabulary required to reach it;
- visibility state before and after discovery;
- exclusion reason codes (unreachable, already resolved, expired, incompatible expedition, duplicate);
- replacement candidate and proof of equivalent capability;
- fallback when no substitute qualifies.

This table prevents a common category error: a route's destination_region_id is not automatically an expedition location ID, and a label in a regional map catalog is not a canonical destination. Resolve the owning catalog and consumer before joining IDs. If the current data cannot express the relation, the selector plan should state the missing relation as an architecture decision rather than inventing a string convention.

Recommended result contract for a future selector: return ordered visible candidates plus required objective coverage, exclusions with stable reason codes, and an unresolved-required list. It should be a pure projection from the active quest snapshot, map knowledge, route/expedition options, and seeded selector inputs. A caller can then show “The route is not available this trip” or delay a quest; it cannot quietly report success. Candidate ordering should be stable for identical inputs, while an existing seeded choice may vary optional content across expedition sequence numbers.

Rarity policy must be validated against small-world cases. If the optional pool has one candidate, it can appear without being labeled rare. If no optional candidate is valid, return only the guaranteed set. If two quests require the same location, coalesce the location once and report both objective links; if they require mutually exclusive locations, expose both as an unresolved conflict and let quest policy determine whether one is delayed. Do not resolve conflict by silently dropping the lower-sort quest.

A later expansion layer may add themes such as winter convoy, damaged wheel, river crossing, or market day, but themes filter candidates and select authored variants; they do not create a new simulation event. Likewise, quest-only places may become map-visible through canonical discovery, but the selector must not persist discovery as a side effect of merely listing a candidate.

## Pass 32A — Maintenance Evidence Sites and Shelter/Expedition Boundaries

### Location pools for a maintenance-story arc

The source question concerns machine wear and its maintenance record corpus. Most maintenance work belongs inside the shelter and should not be forced into the expedition selector. The first selector decision is therefore “does this objective genuinely require a surface trip?” Default answer: no. The shelter service card, archive shelf, machine room, and shift handoff can be scene anchors or UI contexts without becoming map destinations. A required expedition destination is justified only by a specific clue or component already represented by a reachable canonical location and a real player action there.

Use distinct candidate annotations over existing owners:
- Shelter context: machine display, workbench, archive/codex, shift handoff. These are not expedition pool members unless the owning location catalog already represents them as expedition sites.
- Historical evidence: a static maintenance or failure record. It is selectable as readable content only through its actual narrative discovery/codex owner; its filename alone is not a location.
- On-site evidence: an authored encounter or POI tied to a canonical location ID, with confirmed availability and discovery rules.
- Return evidence: a live machine reading after expedition return or a repair action. It is a fresh projection from its owner, not an expedition reward.
- Optional memory: a rumor or conversation that can be reached from a current shelter/faction location only if that content owner exposes it.

### Selection rule and anti-stranding behavior

The expedition selector should never add “Maintenance Depot” as a substitute because the quest mentions a damaged pump. Instead, objective resolution asks the relevant domain owners whether the needed action is possible at shelter, whether a known surface location contains a required part/evidence, or whether a clue route is available. If the objective is shelter-solvable, keep it out of expedition selection. If travel is truly required, rank the exact authored location as quest-required only after proving location ID, availability, map knowledge, and action consumer. Then apply current selector guarantees: no silent loss of mandatory objectives, no duplicate location, no fabricated visibility.

If a required service location cannot spawn:
1. If an existing shelter action can satisfy the objective, offer that capability and report the substitution explicitly.
2. If an authored clue at a reachable site can redirect to a different canonical location, surface the clue and keep the objective active.
3. If the service is temporarily unavailable, set a truthful blocked/delayed state with a retry condition tied to the actual inventory, schedule, or location owner.
4. If the design has a published time window, expire only that optional branch and preserve the investigation route.
5. If no supported alternative exists, do not claim the quest is implementation-ready.

Candidate precedence for this arc: active required surface evidence; critical progression site already valid; relevant faction/character location; already discovered clue site; theme-compatible optional site; ordinary exploration; rare surprise site. This ordering is an architecture proposal to reuse Plan 18's selector contract, not a new map authority. Required locations should guarantee possibility without guaranteeing the player will always see the same optional layout. Seeded selection can vary optional records only after all active requirements remain satisfiable.

### Location examples and availability

Example A, shelter-only: the live tell is seen on the machine panel; the player opens the maintenance archive in the journal. No map marker or expedition slot should appear. Example B, prior discovery: a known workshop site contains an authored failed part or work order. It can appear in an expedition only if the existing location and encounter consumers make that item/evidence available; the quest may not spawn it from dialogue. Example C, unknown site: a log mentions a “north service shed” but has no canonical location reference. The text can motivate a later authored clue, but it cannot create an expedition candidate at runtime. Example D, unavailable witness: no character can be found; the site selector should not compensate by choosing a random mechanic.

Map visibility: “record known” does not mean “place discovered.” A codex entry can reveal a place only if a supported map-discovery action records that fact. Likewise, a site being in the candidate pool is not equivalent to player awareness. Keep selector outputs and UI labels separate: eligible this trip, known on map, rumor only, visited, and unavailable with reason. If the current map owner cannot express one of these distinctions, display only its real states and defer richer UI.

### Minimum viable selector layer

MVP needs no new regional density or machine-condition weighting. It only needs a quest requirement expressed against a currently valid site, stable exclusion reasons, one substitution route, and a no-candidate response that delays without breaking the quest. Optional expansion can later add trip themes (winter repair run, spare-parts search, inspection escort), but those themes filter authored locations and encounters; they cannot imply the repair happened. Dependencies to verify before implementation: current expedition candidate API, map discovery authority, encounter-to-location bindings, item source authority, quest status vocabulary, and which service actions are shelter-only. Production cost: low for a shelter-only quest; high if it needs new surface locations, map linkage, and persistence.


### Pass 32B — Deterministic selection walk-throughs and scarcity cases

The maintenance arc has a useful way to test selector guarantees because a quest writer may be tempted to turn every record into a site. Run these selection walk-throughs before any implementation:

**Case 1: purely shelter evidence.** The quest needs a current tell and an already-discovered log. Expedition list remains unchanged; the quest card names shelter as the action context. Selector result reports no required surface candidate. This is a valid successful selection, not missing content.

**Case 2: location-bound evidence exists and is undiscovered.** The active objective asks for one clue at a known canonical site. The selector may include it as a required candidate if it is in scope and reachable, but it may not mark it discovered merely by selecting it. If the required site is omitted by a pool cap, selector must report unresolved requirement and invoke a documented fallback before the expedition begins.

**Case 3: two maintenance quests need the same site.** Include the site once and associate two objective references. Quest outcomes remain separate: collecting one report cannot silently complete both unless each objective explicitly accepts the same evidence ID.

**Case 4: incompatible locations.** One quest requires an inaccessible clinic, another a factory beyond the selected travel envelope. Do not pretend both are guaranteed by showing a non-reachable card. Apply quest-level delay policy with per-quest reason codes; optionally select a clue location that is reachable and can advance one branch without erasing the other.

**Case 5: location exists but its clue content was removed or invalid.** Fail closed with an authored “no current record at this site” result only if the encounter can return that outcome. Otherwise do not expose the location as a required resolution site. Log a validation defect for the content owner.

**Case 6: same seed after save/load.** The required set, eligibility reasons, and optional ordering must reconstruct identically from canonical IDs and the existing RNG stream. If a repair or quest completion changes the required set, compute again from new source state; do not replay a stale candidate cache.

Fallback quality is measured by retained player agency. Equivalent substitution must preserve objective capability, cost, and risk; clue substitution can defer diagnosis but cannot award the conclusion; delay must name the retry signal; expiry should affect only the branch whose window was disclosed. “No valid location” must be a first-class result, never an empty list that looks like a crash or silently strands a quest.

A future themed selector can group winter repair, water-pump outage, or radio-transmitter service expeditions. It should filter by authored weather and destination compatibility through current systems. A thematic match does not override safety gates, active critical progression, or map knowledge. Re-run examples at low map knowledge, depleted supplies, absent witnesses, maximum active quest count, and all optional pools empty. This gives meaningful coverage without inventing a probabilistic content director.


### Pass 32C — Selector output contract and scarcity cases

A selector response for this arc should expose read-only: expedition sequence, eligible canonical location IDs, reserved objectives, map-derived visibility, exclusion reasons, optional selections, unresolved requirements, and fallback type. Do not persist the whole result when it can be recomputed from restored owners and seeded inputs.

A maintenance quest should not displace critical progression when selector capacity is constrained. Delay the optional quest with a reason. If two maintenance leads compete and neither is critical, prefer the reachable supported location; stable tie-break by authored priority then ordinal quest ID. Do not use random selection to decide which quest becomes impossible. If both quests need one site, include that location once and preserve separate objective links. One clue may advance both only when each objective explicitly accepts that same clue ID.

Distinguish no valid location cases: no expedition required because shelter action suffices; objective is shelter-based; temporary block has an owner-defined retry; or unresolved design defect. An empty location list alone cannot tell the caller which copy to show. Rare surprise locations are not fallbacks because their appearance is not guaranteed. Any equivalent replacement must preserve objective capability, cost, and risk.

The audit must cover all-excluded and zero-pool scenarios, low map knowledge, depleted supplies, absent witnesses, maximum active quests, and optional pools empty. Same seed plus same source state must reconstruct required set and optional order; a quest genuinely resolved must change the selection. If current selector cannot report objective coverage, propose a bounded API improvement for the approved owner package rather than adding a second selector or cache.


### Pass 32D — Expedition UX and operational diagnostics

The expedition panel should show a required maintenance clue only when a real trip can make progress. If the objective is shelter-based, place it in the shelter/quest view and do not consume map space. When it is travel-bound, state the destination's known/rumored/available status and why it was selected. An unavailable site should show a retry or clue route, not an empty card.

For implementation diagnostics, report counts of required candidates before filtering, after reachability filtering, and after visibility filtering; enumerate exclusion reasons by stable location ID. These diagnostics help distinguish a selector bug from a valid delay. Keep them in existing development diagnostics and do not expose hidden rarity weights to the player. UI acceptance includes keyboard/controller focus after the candidate list changes, accessible reason text, and back navigation that does not cancel or complete the quest.
