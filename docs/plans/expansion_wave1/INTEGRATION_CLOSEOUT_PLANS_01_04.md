# Plans 1–4 Integration Closeout and Handoff

**Status:** DRAFT closeout; documentation-only; no production code or data changed.

This closeout converts the four large expansion plans into a dependency-ordered integration package. It records current reality, shared seams, ownership, persistence, determinism, UI boundaries, content gating, verification, rollout, rollback, and the remaining blockers. A closeout receipt is evidence that a plan can be safely reviewed; it is not proof that the game already contains every proposed feature.

## 1. Closeout objective

Close Plans 1–4 as design packages that are ready for premise audits and bounded implementation claims. Closure means the four plans have a common ownership map, dependency order, save and deterministic contracts, content boundaries, focused verification, and explicit blockers. It does not mean that the prose itself proves runtime integration. The closeout package freezes the assumptions that can be checked and identifies the smallest safe first slice for each plan. It records that Plans 1–4 are still drafts until current source, exact path claims, and focused evidence agree.

## 2. Current reality by plan

Plan 1 proposes wildland recovery content while existing weather cascade, shelter fire, disaster response, expedition, vehicle, wildlife, communications, cartography, and archive owners remain authoritative. Plan 2 proposes mobile care content while health, inventory, vehicle, expedition, relationships, consent, journal, and roster owners remain authoritative. Plan 3 proposes public works content while water, project, territory, settlement, reputation, press, archive, and colony owners remain authoritative. Plan 4 proposes an anthology library while narrative questline, campaign manifest, difficulty, identity, localization, save, and ModSupport boundaries remain authoritative. The closeout treats every proposal as an adapter, content packet, or bounded command over those realities.

## 3. Evidence and premise receipts

Before implementation, each plan receives a premise receipt containing the exact source files inspected, the data records found, the public API used, the save owner, the host route, and the focused verification command. A plan row without a receipt stays design-only. A receipt must distinguish present behavior, partially wired behavior, unreachable data, and an unowned proposal. Old audits and plan names are context, not proof. The integrator records the date of inspection and marks any premise that changed during implementation as superseded.

## 4. Cross-plan ownership

Recovery hazards remain with weather and environment owners; medical outcomes remain with health and consent owners; civic completion remains with project, water, territory, and reputation owners; scenario progression remains with narrative and campaign owners. Shared facts cross plan boundaries through events, read models, or existing providers. No plan may create a second inventory, patient, route, world, project, campaign, or profile ledger. Cross-plan links are references with source IDs, not copied mutable state. If two plans need a new fact, the integrator assigns one owner before either package is promoted.

## 5. Dependency-ordered integration

The closeout sequence is: premise audit, owner claim, Core contract, focused Core tests, data schema and validator, save and replay coverage, existing event/provider wiring, thin Godot presentation, authored content, headless/runtime check, and only then balance or volume. Plan 1 depends on verified weather and expedition seams; Plan 2 depends on health, consent, inventory, and travel seams; Plan 3 depends on project, water, settlement, and reputation seams; Plan 4 depends on manifest, narrative, campaign, and localization seams. Parallel authoring is allowed only after shared IDs and owner boundaries are recorded.

## 6. Data and migration closeout

All new records use snake_case IDs, explicit schema versions, bounded values, references that current validators can explain, and migration notes for old saves or catalogs. Authored descriptions belong in StreamingAssets/Data. Mutable progress belongs to its existing Core owner. A catalog row is not integrated until a reachable consumer exists. The closeout requires duplicate-ID checks, reference checks, range checks, empty-catalog behavior, and a report for records that are valid JSON but unreachable at runtime.

## 7. Save and determinism closeout

Each stateful slice must name CaptureState, RestoreState, version, old-save defaults, invalid-save behavior, checksum implications, and null or empty semantics. Random choices use the existing seeded RNG contract with stable ordering and invariant formatting. No wall-clock seed, hash iteration order, or UI callback may influence Core outcomes. Paired same-seed replays must compare state hashes and event order. A save during an interrupted action must restore the same state ladder and avoid duplicate rewards, penalties, messages, or archive entries.

## 8. Host, UI, and accessibility closeout

Godot hosts remain thin: input, binding, presentation, provider adaptation, lifecycle, and feedback. Panels read authoritative facts and route commands; they do not calculate health, inventory, project completion, route validity, or campaign progression. Every route has loading, empty, blocked, completed, error, focus, back, keyboard, controller, contrast, text-scale, reduced-motion, and long-localization cases. A visible marker or journal entry reinforces a result only after the owner emits it. Disposal and refresh behavior are part of the acceptance receipt.

## 9. Content and narrative closeout

Each plan has a content bank that can be authored independently from runtime authority. Side quests name entry facts, choices, costs, failure text, rewards, consequences, repeat policy, and follow-up hooks. Characters and locations must reference canonical IDs. Prose may describe an observed state but cannot imply an unimplemented mechanic. Content review checks tone, variation, privacy, consent, localization keys, and accessibility copy. A high-volume bank is enabled only after a small vertical slice proves the data route, save behavior, UI feedback, and replay stability.

## 10. Performance and diagnostics

Every package defines a work budget, expected frequency, worst-case record count, allocation target, event fan-out, save delta, and safe degradation. Caches have one owner and explicit invalidation. Operator diagnostics report blocked commands, missing references, stale saves, panel refresh count, and event latency without collecting private free text. Telemetry is read-only and can be disabled without changing gameplay. Dense content is lazy-loaded or batched while deterministic ordering remains stable.

## 11. Verification matrix

The closeout requires Core happy-path, boundary, failure, event, invariant, save round-trip, deep-copy, migration, invalid-version, catalog, duplicate-ID, and deterministic replay tests. Integration checks trace command to Core mutation, Core event to provider, provider to Godot panel, and panel feedback to player. Headless checks are added only where the runtime route is affected. Evidence includes commands, results, limitations, files touched, and intentionally untouched shared paths. A compile-green result alone never closes a row.

## 12. Blockers and decisions

Open blockers include any missing owner, unresolved canonical ID, absent host route, unsupported catalog field, save migration decision, unverified event order, or performance budget without a fixture. A blocker is written with its evidence and the decision required. The integrator does not fill a gap with a local cache, panel counter, speculative registry, or duplicate event bus. Decision-blocked items remain visible in the handoff and are excluded from the ready slice.

## 13. Rollback and recovery

Each implementation slice is reversible by package. Data additions are gated, migrations are tested against a copy, and feature flags can disable new presentation while preserving old saves. If a new field cannot be restored, the adapter rejects or defaults it according to the documented version rule and reports the reason. A failed runtime route rolls back the host binding and leaves the Core contract isolated. Rollback evidence is captured before the next slice begins.

## 14. Integration handoff

The integrator receives four owner packets, one for each plan, plus the cross-plan matrix. Each packet lists objective, premise, exact paths, command route, state owner, catalog rows, save section, seeded replay, UI route, accessibility cases, content IDs, performance fixture, rollback, and open blockers. The first safe action is a read-only premise audit for one bounded slice. Closure is complete when every claimed row has a receipt or is explicitly marked design-only, and no plan claims runtime behavior that current evidence cannot show.

## Cross-plan readiness matrix

| Package | Current owner receipt | Data contract | Save/replay | Host route | Content gate | Status |
|---|---|---|---|---|---|---|
| Plan 1 recovery | required per slice | required | required | weather/expedition/UI proof | report-first | READY FOR PREMISE AUDIT |
| Plan 2 outreach | required per slice | required | required | health/consent/travel proof | privacy-first | READY FOR PREMISE AUDIT |
| Plan 3 public works | required per slice | required | required | project/water/settlement proof | service-desk-first | READY FOR PREMISE AUDIT |
| Plan 4 anthology | required per package | required | required | narrative/manifest/UI proof | one-package-first | READY FOR PREMISE AUDIT |

## Implementation contract

### MUST PRESERVE

One authority per concern; engine-free Core; JSON data authority; deterministic seeded behavior; existing save ownership; accessible input and feedback; current integration ledger and path claims.

### MUST ADD

Premise receipts, bounded Core contracts, validator-backed data, save and replay coverage, thin host routes, focused content slices, performance fixtures, and rollback evidence.

### MUST NOT DO

Do not create parallel ledgers, speculative registries, panel-owned gameplay, Unity gameplay logic, unverified IDs, broad refactors, or claims based on prose volume.

### VERIFY WITH

Focused source inspection, catalog checks, Core tests, save round trips, deterministic replay, bounded host/runtime checks, accessibility review, and an evidence-rich handoff.

### FIRST SAFE IMPLEMENTATION STEP

Select one bounded slice from one plan, re-audit its current premise, claim exact paths, and produce the owner receipt before editing production code.

## Appendix A — Plan 1 recovery closeout packet

The first safe recovery slice is a report-first route that reads a current weather, expedition, shelter, or communications fact and presents a bounded field report. It must not claim wildland spread, smoke exposure, ecological burn state, or outdoor firefighting until a named owner exists. The receipt lists the source fact, canonical location, deterministic timestamp, event route, journal or archive consumer, and the exact UI surface. A report can be acknowledged, deferred, or linked to an existing follow-up. It cannot create a new hazard by itself. Acceptance covers a missing location, a stale weather result, a restored interrupted report, and a same-seed replay. The integrator should stop after this slice if the source fact cannot be shown through the current host.

## Appendix B — Plan 2 outreach closeout packet

The first safe outreach slice is a consent-aware dispatch preview over existing health, inventory, vehicle, expedition, relationship, and journal facts. It shows what is known, what is missing, what the player may request, and which owner will decide whether the route succeeds. A shipment, message, or colony building is not treated as clinical care without a verified care action. The receipt names the consent scope, patient or household reference, inventory transfer owner, travel result, health result, and follow-up journal event. Acceptance covers withdrawn consent, unreachable destination, missing stock, interrupted travel, duplicate message, and reload during a pending dispatch. The panel must remain useful when no clinic route exists.

## Appendix C — Plan 3 civic closeout packet

The first safe public-works slice is a service request or inspection view over existing project, settlement, territory, water, reputation, press, and archive facts. It presents provenance, priority, dependency, and current owner without creating a civic ledger. A printed notice does not complete a work order; a holiday or anniversary does not become a civic calendar. The receipt names the accepted event, project result, reputation evidence, water or territory dependency, and archive entry. Acceptance covers duplicate requests, abandoned inspections, no available material, inaccessible location, stale project state, and a panel reopened after completion. The integrator should ship one request type before enabling a high-volume service desk.

## Appendix D — Plan 4 anthology closeout packet

The first safe anthology slice is one manifest-backed package with a read-only preview, one player activation route, one bounded branch, and one ending record. It uses the current narrative and campaign owners and does not assume a global scenario browser, arbitrary quest graph, cross-run profile, or ModSupport catalog. The receipt names manifest ID, narrative IDs, campaign flags, difficulty binding, localization keys, save revision, and retirement behavior. Acceptance covers missing branch data, a retired package, repeated activation, a save during a choice, a long localized label, and identical seeded replay. Package volume stays low until continuity and release checks pass.

## Appendix E — Cross-plan event order

Shared facts flow from owner to event to provider to presentation. Plan 1 may publish a recovery observation that Plan 3 can reference as a location fact only after the source owner confirms it. Plan 2 may publish an outreach result that Plan 6 can reflect in a relationship or journal entry only through the existing journal or relationship route. Plan 3 may publish a completed public work that Plan 4 can mention only if the campaign or narrative owner consumes it. Plan 4 may reference any of these facts by stable ID, but it cannot copy mutable state. Event order is tested with a recorded sequence and an idempotency fixture. Duplicate delivery must not duplicate rewards, archive rows, relationship changes, or notifications.

## Appendix F — Migration and compatibility cases

Each package prepares three compatibility cases: an old save with no new section, an old save with a stale section version, and a malformed section with an invalid reference. The owner supplies a default or rejection rule, the loader reports a reason, and the host shows a recoverable message. No migration silently invents a location, survivor, item, project, route, or campaign flag. Data catalog migrations run through the existing validator and preserve old records where they remain semantically valid. The closeout requires a fixture for deep-copy isolation so restoring one package does not mutate another in memory.

## Appendix G — Operator evidence bundle

Every implementation handoff contains a premise receipt, source search notes, exact files, owner claim, schema diff, save contract, deterministic replay result, focused test result, runtime screenshot or log, accessibility checklist, performance measurement, rollback step, and known limitation. The bundle uses stable names and dates and excludes secrets or private free text. A failed check remains visible with its owner and decision required. Operators can compare bundles between slices without treating a larger document as stronger evidence. The bundle is the release boundary for the four plans.

## Appendix H — File impact map

| Area | Action | Reason | Risk | Gate |
|---|---|---|---|---|
| `docs/plans/expansion_wave1/PLAN_01...` | READ/UPDATE | recovery design and receipts | stale premise | current-source audit |
| `docs/plans/expansion_wave1/PLAN_02...` | READ/UPDATE | outreach design and consent | privacy drift | owner receipt |
| `docs/plans/expansion_wave1/PLAN_03...` | READ/UPDATE | civic request and water boundaries | duplicate ledger | project/water proof |
| `docs/plans/expansion_wave1/PLAN_04...` | READ/UPDATE | package manifest and continuity | parallel campaign | manifest proof |
| `Assets/Ashfall.Core/` | MODIFY only after approval | domain contracts and state | Core coupling | focused tests |
| `src/` | MODIFY only after approval | thin provider and UI route | host gameplay logic | headless route |
| `Assets/StreamingAssets/Data/` | MODIFY only after schema proof | authored records | unreachable IDs | validator |
| save owners | MODIFY only after contract | Capture/Restore | old-save breakage | round trip |
| tests | CREATE/MODIFY only for claimed slice | evidence | false coverage | focused target |

## Appendix I — Risk register

The highest risks are duplicate state, a guessed canonical ID, an event that fires before persistence, a panel that displays inferred progress, a catalog row with no consumer, nondeterministic ordering, and content that implies an unimplemented mechanic. Each risk has the same mitigation: stop, verify current source, name the owner, add a focused fixture, and record the decision. Broad cleanup, mass formatting, unrelated localization changes, and retired Unity paths remain out of scope. A risk is closed only when a receipt demonstrates the mitigation.

## Appendix J — Definition of integration close

Plans 1–4 are integration-close when each has one accepted vertical slice, one owner packet, one validated data path, one save or explicit stateless decision, one deterministic replay where relevant, one host-visible result, one accessibility pass, one performance measurement, one rollback, and one documented list of remaining design-only rows. The closeout document is then updated with commit or handoff references. Until those receipts exist, the plans remain DRAFT and no wording in the expansion files should be read as a claim that production behavior is already present.

## Closeout refresh — 2026-09-22

This refresh rechecks the closeout against the current integration ledger and the active project rules. The four plans remain documentation-only packages. Their large word counts provide design coverage, not runtime proof, and no proposal is promoted merely because it has a data-shaped paragraph, a named quest, or an acceptance row. The live queue, path claims, current source, save owners, and focused evidence continue to control implementation.

### Refresh outcome

The closeout still supports four separate premise audits followed by one bounded vertical slice per plan. Plan 1 begins with a report-first recovery observation over an existing weather, expedition, shelter, communications, or archive fact. Plan 2 begins with a consent-aware outreach dispatch preview over existing health, inventory, vehicle, expedition, relationship, and journal facts. Plan 3 begins with one public-works request or inspection over existing project, settlement, territory, water, reputation, press, and archive facts. Plan 4 begins with one manifest-backed anthology package using current narrative, campaign, identity, difficulty, localization, save, and ModSupport boundaries. None of these first slices establishes a new route, patient, civic, scenario, or profile authority.

### Current integration conditions

The integrator must read `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, and the applicable owner source before claiming a path. A plan row becomes actionable only when the receipt names an existing public API, the canonical data record or an explicit data-only decision, the mutable state owner, the event or provider route, and the focused verification target. A stale audit statement is retained as context and marked superseded when current source disagrees. Shared paths remain read-only to unclaimed builders; shared seams belong to the named integrator.

The closeout records five gates for every slice. First, the premise gate proves that the proposed fact or command exists and is reachable. Second, the ownership gate assigns mutation, events, persistence, and presentation to one owner each. Third, the data gate validates IDs, schema, ranges, references, empty catalogs, and migration. Fourth, the behavior gate proves transitions, failure, interruption, save/restore, and deterministic replay where the slice is stateful. Fifth, the handoff gate records UI focus, accessibility, performance, rollback, evidence, and the exact next decision. A failed gate leaves the row design-only.

### Plan 1 closeout refresh — recovery evidence

The recovery plan may reference current weather-cascade closure, expedition delay, vehicle condition, communications, wildlife, cartography, shelter fire, disaster, and archive facts only through their owners. The first slice should render a field report with source timestamp, location ID, confidence, acknowledgement state, and a follow-up link to an existing command. It must not infer landscape-fire spread, smoke exposure, ecological burn state, or outdoor firefighting from a catalog name. A missing source fact produces an explicit unavailable state. The acceptance packet includes a stale weather result, a missing location, duplicate acknowledgement, save during a pending report, and a paired same-seed replay.

### Plan 2 closeout refresh — outreach evidence

The outreach plan may compose health, consent, inventory, vehicle, expedition, relationship, roster, journal, and time-capsule facts while leaving clinical outcome, item ownership, travel completion, and privacy decisions with their existing owners. The first slice shows a consent-aware dispatch preview and makes clear that a shipment, transmission, colony building, or destination listing is not clinical care by itself. The receipt includes withdrawn consent, missing stock, unreachable destination, interrupted travel, duplicate message, and a reload during a pending dispatch. No plan-local expiry, patient ledger, or hidden diagnosis cache is permitted.

### Plan 3 closeout refresh — civic evidence

The civic plan may present project, settlement, territory, water, reputation, press, archive, and colony facts through an existing service or inspection route. A request is not complete until its current owner emits the accepted event and records the result. Printing a notice, holding a celebration, or discovering a water source does not complete a work order. The first slice proves duplicate request handling, abandoned inspection, missing material, inaccessible destination, stale project state, and panel reopen after completion. A high-volume service desk remains deferred until one request type passes the full gates.

### Plan 4 closeout refresh — anthology evidence

The anthology plan may package authored scenario content behind the current manifest, narrative, campaign, identity, difficulty, localization, save, and ModSupport contracts. The first slice contains one package, one activation path, one bounded branch, and one ending record. It does not assume a global scenario browser, arbitrary quest graph, cross-run profile, or campaign catalog in ModSupport. The receipt names manifest ID, narrative IDs, flags, difficulty binding, localization keys, save revision, and retirement behavior. Missing branch data, retired package, repeated activation, save during a choice, long localization, and same-seed replay remain required cases.

### Cross-plan closeout refresh

Shared facts flow from the owner to a fact event or read provider, then to a thin host route and a visible result. Plan 1 recovery observations can become Plan 3 location context only after the source owner confirms them. Plan 2 outreach results can become Plan 6 relationship or journal callbacks only through existing routes. Plan 3 completed work can become Plan 4 narrative context only through the campaign or narrative owner. Plan 4 may reference stable IDs from the other plans but never copy mutable state. The event-order fixture proves that duplicate delivery does not duplicate rewards, archive entries, relationship changes, project completion, or notifications.

### Evidence bundle and stop lines

The refreshed handoff bundle contains source search notes, exact paths, owner claim, schema or stateless decision, save contract, replay result, focused test command, runtime evidence when applicable, accessibility checklist, performance measurement, rollback step, and known limitation. It contains no secrets or private free text. The stop line is reached when a canonical ID, consumer, event order, save owner, migration rule, deterministic input, or UI route cannot be demonstrated. At that point the row stays DRAFT and the missing decision is recorded in the ledger rather than filled with a local cache or speculative registry.

### Refreshed integration-close definition

Plans 1–4 are integration-close only when each has one accepted vertical slice, one owner packet, one validated data path, one save or explicit stateless decision, one deterministic replay where relevant, one host-visible result, one accessibility pass, one performance measurement, one rollback procedure, and one list of remaining design-only rows. This refresh confirms that the closeout package contains the gates, first slices, event order, migration cases, and handoff structure required to reach that state. It does not claim that any of the four slices has been implemented or accepted.

## Closeout addendum — synchronized review packet

This addendum keeps the four-plan closeout usable as one review packet while the larger expansion documents continue to grow. It does not reopen retired work, grant ownership, or convert a design row into an implementation claim. The integrator should treat the closeout as a gate map: every proposed slice enters through a current-source premise receipt and exits through an evidence-rich handoff.

### Package split and review order

Review Plan 1 recovery first where a current environmental, weather, expedition, communications, shelter, or archive fact can be observed without inventing a new hazard state. Review Plan 2 outreach after the health, consent, inventory, travel, and relationship owners are identified for one dispatch preview. Review Plan 3 civic work after a single project or inspection command can be traced to an existing water, settlement, territory, reputation, press, or archive result. Review Plan 4 anthology last for the first shared package because it may reference the other plans but must not own their mutable facts. This order minimizes speculative cross-plan dependencies.

Each package has a separate owner receipt, but the four receipts share one event-order fixture. The fixture records the source fact, event name, provider refresh, host feedback, save boundary, and later callback. It proves that a delayed subscriber, duplicate event, host close, or reload cannot duplicate a reward, relationship change, project completion, archive row, or notification. The fixture also records which facts are intentionally not available to a package, preventing accidental access through a convenient panel callback.

### Closeout acceptance rows

The refreshed packet accepts a plan slice only when the premise, owner, data, state, host, content, replay, accessibility, performance, and rollback rows each have current evidence. The premise row names the exact file and API. The owner row names the mutating system and event publisher. The data row names the schema, validator, and migration default. The state row covers interruption, failure, expiry, and duplicate input. The host row covers focus, close/back behavior, empty states, long text, and refresh disposal. The content row ties authored IDs to reachable consumers. The replay row compares seeded outcomes and save hashes. The accessibility and performance rows contain bounded fixtures. The rollback row restores the previous owner state and preserves evidence.

A row can be marked **ready for premise audit**, **blocked pending decision**, **design-only**, or **superseded by current source**. It cannot be marked integrated from prose review alone. The closeout ledger should keep the evidence link beside the status so a later agent can distinguish an old assumption from a current result. If a source path is claimed by another package, the builder stops and reports the collision rather than editing around it.

### Shared data and content rules

The four plans may share stable canonical IDs, source references, and read-only projections. They may not copy mutable inventory, health, route, relationship, project, campaign, archive, or profile state. New authored rows live in the current data authority and pass the existing schema and integrity pipeline. A row with no reachable consumer remains a content candidate. A field that needs persistence names CaptureState, RestoreState, versioning, old-save defaults, malformed-save behavior, and checksum impact before it is accepted.

Narrative content follows the same rule. A scene can describe an observed shortage, disagreement, closure, or recovery, but it cannot imply an effect that no owner applies. Side quests must name entry fact, player choice, cost, refusal, failure, reward, consequence, repeat policy, and delayed callback. Character and location references use verified canonical IDs. The closeout review rejects generic fetch loops, duplicate factions, unowned currencies, hidden panel counters, and irreversible content that lacks a rollback or retirement path.

### Operational handoff

The final handoff for Plans 1–4 contains the closeout version, date, source paths, owner packets, data and save decisions, focused commands, runtime evidence where applicable, accessibility notes, performance figures, rollback procedure, open blockers, and intentionally untouched shared paths. It excludes secrets and private free text. A bounded implementation slice may proceed only after the named integrator accepts the receipt and the live ledger records the claim. Until then, all four plans remain DRAFT and the closeout remains a review instrument rather than a release statement.

## Closeout refresh — threshold and handoff review

This refresh carries the Plans 1–4 closeout into the next expansion threshold without changing its authority. The four packages still require one bounded vertical slice each, one owner packet each, and one shared event-order fixture. The expansion files may grow, but their status remains DRAFT until source, data, host, save, replay, accessibility, performance, and rollback evidence is attached.

The threshold review adds one explicit question to every closeout receipt: does the proposed slice depend on a plan that has crossed a documentation volume milestone without gaining a verified runtime seam? If so, the dependency remains a design reference only. A larger field bank cannot authorize a new save section, catalog, event, route, or panel. The integrator must still claim exact paths, inspect current APIs, and record the owner decision before editing production code.

The cross-plan fixture now checks delayed delivery as well as duplicate delivery. It sends a source fact through its owner event, pauses before provider refresh, saves and reloads, delivers the event again, and then opens the host route. The expected result is one durable outcome, one visible notification, one journal or archive callback where explicitly owned, and no copied mutable state. A missing subscriber is reported as a bounded limitation rather than patched with a second bus.

The first-slice order remains recovery observation, consent-aware outreach preview, civic request or inspection, and one manifest-backed anthology package. Each slice must prove its own empty, blocked, stale, interrupted, and rollback states before the next plan consumes its result. Plan 4 may cite a stable fact from Plans 1–3 only through the narrative or campaign owner, and the other plans may not depend on a scenario package to make their core state valid.

The handoff bundle now records the threshold status of every referenced plan, the reserved field layer if applicable, current source evidence, exact path ownership, schema and migration decision, save or stateless decision, seeded replay, UI focus and accessibility checks, performance budget, rollback, open blockers, and deliberately untouched shared paths. This is a review artifact; it does not claim that the four plans are implemented.

## Closeout refresh — Wave 10 integration boundary

This refresh keeps Plans 1–4 reviewable while Plans 5–8 enter another large documentation wave. The closeout boundary remains unchanged: a plan is ready for a premise audit, not automatically ready for implementation. The integrator must still verify current source, exact owner paths, data reachability, save or stateless behavior, deterministic ordering, host feedback, accessibility, performance, and rollback.

The new review question is whether added content has a real consumer and a durable consequence. A side quest, operator card, field bank, or prose packet may be authored only when its entry fact, player command, owner result, failure state, and later callback are named. A high-volume bank is accepted only after a small sample proves catalog validation, pagination, focus restoration, save boundaries, replay stability, and safe degradation. Volume cannot replace a missing seam.

The four first slices continue to be recovery observation, consent-aware outreach preview, civic request or inspection, and one manifest-backed anthology package. The shared fixture now records a delayed callback and a reloaded host, then checks idempotency and source provenance. Any cross-plan reference remains a stable read reference; mutable state stays with the source owner. A missing subscriber or stale source is reported as a bounded limitation.

The handoff packet records the current closeout version, package order, owner receipts, threshold status, schema and migration decisions, focused verification, runtime evidence where applicable, accessibility and performance results, rollback, open blockers, and intentionally untouched shared paths. The closeout remains documentation-only and does not claim that any plan has been implemented.

## Closeout refresh — Wave 11 package discipline

This addendum keeps the Plans 1–4 integration closeout aligned with the current documentation wave. The closeout still describes a reviewable dependency package rather than an implementation result. Every proposed slice must enter through a premise receipt and exit through a handoff that shows owner evidence, data reachability, persistence or a stateless decision, deterministic behavior, host feedback, accessibility, performance, and rollback.

The package discipline is now explicit for dense content. A high-volume catalog, side-quest packet, or field layer is reviewed in a small sample first. The sample must prove one command, one owner result, one visible route, one save or stateless outcome, one replay comparison, and one failure recovery. Only then can the remaining records be batched. Duplicate IDs, unsupported effects, stale assumptions, private text, and shared-path collisions remain blockers.

The cross-plan event fixture follows source fact, owner event, provider refresh, host presentation, save/reload, delayed callback, and duplicate delivery. It verifies one durable outcome and one visible notification, while preserving stable source IDs and refusing to copy mutable state. Plan 4 may consume a verified fact through narrative or campaign ownership; it cannot become the source for recovery, outreach, civic, route, relationship, or archive state.

The handoff bundle lists the closeout version, package order, current source evidence, exact paths, owner claims, schema and migration decisions, focused checks, runtime evidence where applicable, accessibility and performance results, rollback, blockers, and untouched shared paths. The documents remain DRAFT until those receipts exist.
