# Plans 5–8 Integration Closeout and Handoff

**Status:** Documentation closeout and implementation plan; not an implementation claim.
**Scope:** Plans 5–8 regional supply and travel resilience, survivor relationship and memory continuity, shelter automation and power recovery, and exploration cartography and archive composition.
**Review rule:** Current source, `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, and the live data loaders outrank this document. A plan section is ready for implementation only after its premise receipt and path claim are accepted.

## 1. Closeout objective

The four plans now have complete expansion scaffolds, including owner-bound mechanics, player loops, authored-content banks, side-quest hooks, UI and UX requirements, save and replay contracts, performance budgets, migration cases, and rollback notes. This closeout turns that material into one dependency-ordered implementation path. It closes the planning wave by naming the first safe slice for each plan, the cross-plan event order, the evidence required before code changes, and the conditions that stop work.

The closeout does not promote prose into runtime behavior. The plans remain proposals over existing authorities. A catalog row is not integrated until a current consumer reaches it. A panel is not integrated until a host route exposes an existing command and reports a truthful owner state. A stateful feature is not integrated until capture, restore, migration, checksum, and deterministic replay are proven through the current owner.

## 2. Current plan boundaries

| Plan | Primary owner families | First safe slice | Explicit non-goal |
|---|---|---|---|
| 5 | route, cargo, settlement, weather, vehicle, communications, expedition | one arrival-proof read model over an existing route result | no second trade, cargo, route, or settlement ledger |
| 6 | relationship, journal, consent, identity, roster, time-capsule, archive | one consent-aware memory record projection | no second relationship graph, private-data store, or memory decay authority |
| 7 | power, water, air, building, schedule, duty, needs, accessibility | one demand/reserve read model over existing power and water facts | no second grid, needs simulation, or maintenance ledger |
| 8 | cartography, expedition, installation, fragment, codex, journal, archive | one provenance-aware survey projection | no second map, fragment inventory, or archive authority |

## 3. Dependency-ordered integration sequence

### Gate 0 — premise and claim audit

For the selected first slice, search current Core, host, data, save, and test paths. Record the exact existing owner, public API, event or query seam, data file, route, and save section. Search for retired names, duplicate concepts, stale plan claims, and active ownership claims. If the owner or consumer cannot be proven, stop and return the slice to premise review.

### Gate 1 — data and content contract

Author one representative row only after the loader and validator are identified. Use a snake_case ID, explicit schema version, bounded ranges, localization keys, references, lifecycle status, and source provenance. Validate duplicate IDs, missing references, unsupported effect keys, empty consumers, and corrected-content migration. Content may describe a future result, but it cannot mutate state or imply a consumer that has not been demonstrated.

### Gate 2 — Core owner seam

Extend the existing owner with the smallest command or read model. Facts leave Core through existing events or typed queries. Panels, caches, and adapters do not become authorities. Stateful changes name the mutation owner, reservation behavior, event ordering, error result, and release rule. A derived projection must be rebuildable from owner state.

### Gate 3 — save, migration, and deterministic replay

Before host wiring, exercise the owner through capture and restore. Pin old-save defaults, malformed-envelope refusal, checksum behavior, null semantics, duplicate commands, interrupted transitions, and content correction. Pair the same seed and ordered inputs before and after save, reload, host reopen, and duplicate event delivery. No wall-clock time, process identity, locale, random GUID, or hash iteration order may affect a replayable result.

### Gate 4 — host route and interaction

Bind the read model to the existing session and route. Define focus order, keyboard/controller actions, close/back behavior, loading, empty, stale, blocked, redacted, completed, and error states. Keep source age and uncertainty visible in text. Optional art or audio may fail safely without removing the command, feedback, or navigation path. Refresh and disposal must be idempotent.

### Gate 5 — cross-plan event order

Use one publisher per fact and one consumer per consequence. The recommended order is: weather and world facts → route/expedition or shelter owner result → relationship/journal or cartography/archive projection → narrative callback → UI refresh and audio/feedback. A downstream projection cannot write upstream state. Cross-plan listeners must ignore duplicates and preserve source age.

### Gate 6 — content and side-quest slice

Start with one location, one named person or crew, one concrete disturbance, one quiet success, one costly success, one refusal, and one unresolved outcome. The branch must change a reachable owner state or projection. Add repeat policy, retirement condition, localization keys, delayed callback, and content-correction behavior. Do not expand a bank until the representative slice passes the previous gates.

### Gate 7 — accessibility, performance, and diagnostics

Exercise long localization, focus retention, reduced motion, sound-off feedback, non-color cues, empty catalogs, stale rows, repeated refresh, high-volume pagination, save delta, allocation count, event fan-out, and first-page latency. Record deterministic fixture hashes and a bounded performance budget. A failure leaves the feature in a visible, recoverable state and emits a useful diagnostic without suppressing the underlying owner error.

### Gate 8 — handoff and rollback

The handoff packet lists exact files, owner claim, consumer, data rows, save impact, focused verification command, observed result, limitations, and rollback path. Rollback removes the new content or disables the adapter while preserving old save readability. A plan cannot be marked integrated from a compile result, a document count, or an unreachable catalog row.

## 4. Plan-specific first slices

### Plan 5 — arrival proof over an existing route result

Read the existing expedition or route outcome, expose source age, cargo reservation, arrival confidence, and recovery timing, and let the player acknowledge, defer, or review it. The slice must not create a route ledger. The first content sample is one waystation arrival with a weather delay, a shared-load decision, and a return callback into the journal or settlement projection. Acceptance requires route owner evidence, deterministic delay ordering, save round-trip, a focus-safe panel, and a duplicate-event test.

### Plan 6 — consent-aware memory projection

Read one existing relationship or journal fact with consent state and source provenance. Offer view, redact, pause contact, or request renewal through the existing owner command. The slice must not copy private text into a new store. The first content sample is a remembered promise that can be confirmed, deferred, or left unresolved and later appears as a journal projection. Acceptance requires consent ownership, redaction behavior, old-save defaults, deterministic ordering, and keyboard/controller parity.

### Plan 7 — demand and reserve projection

Read canonical power, water, warmth, and duty facts and present one demand threshold with a reserve explanation. Allow the player to inspect, defer, or route an existing maintenance or schedule command. The slice must not create another grid or maintenance queue. The first content sample is a cold-room demand spike that resolves through a current owner event. Acceptance requires source freshness, no double spending, save/reload parity, accessible warnings, and bounded refresh cost.

### Plan 8 — provenance-aware survey projection

Read one cartography or expedition observation and show coordinate confidence, instrument condition, provenance age, and archive custody. Offer verify, annotate, defer, or return through the current owner. The slice must not create a second map or fragment inventory. The first content sample is a survey mark corrected after a return trip and cited in an existing archive surface. Acceptance requires idempotent revision, deterministic observation ordering, migration behavior, focus-safe pagination, and a visible uncertainty state.

## 5. Cross-plan acceptance matrix

| Row | Evidence required | Stop condition |
|---|---|---|
| Premise | current owner, API, consumer, and path search | owner or consumer is inferred only from a plan |
| Data | representative schema-valid row and validator output | duplicate, unresolved, or unreachable row |
| State | transition table, mutation owner, release rule | panel or cache owns mutable state |
| Save | capture/restore, old-save default, malformed refusal | new durable fact lacks restore path |
| Replay | same-seed hash across save/reload and duplicate event | ordering varies by process or wall clock |
| Host | route, focus, close/back, loading and error states | command is decorative or unreachable |
| Content | one branch with refusal and delayed callback | branch changes prose only |
| Accessibility | keyboard/controller, text scale, non-color cue, reduced motion | single sensory channel required |
| Performance | first-page latency, allocations, fan-out, save delta | unbounded refresh or pagination |
| Rollback | reversible adapter/content switch and readable saves | rollback requires deleting player state |
| Handoff | exact files, command, result, limitation, next owner | “compile passed” is the only evidence |

## 6. Save and determinism contract

Plans 5–8 share no new save section by default. Pure projections rebuild from existing owner state. If a first slice proves that durable state is necessary, the owner must register the smallest section, version it, define migration defaults, and update the save-store matrix before implementation. Every stateful command is idempotent or explicitly rejects duplicates. Source timestamps are display metadata only; they never seed simulation. Stable ordering uses authored ordinal or deterministic tie-breakers owned by the subsystem.

## 7. UI and UX contract

The host exposes facts, commands, and outcomes; it does not hide uncertainty or invent progress. Panels retain focus after refresh, return focus on close, support controller and keyboard navigation, and expose text alternatives for audio or color. Redacted or consent-blocked content explains why it is unavailable and offers the next permitted action. High-volume views paginate through the existing session and degrade to a safe empty or stale state when optional data is unavailable.

## 8. Rollback and recovery

If a premise fails, remove the proposed row and adapter, preserve the prior catalog and save envelope, and leave a short audit note naming the missing owner or consumer. If runtime wiring fails after data authoring, disable the row through its lifecycle status rather than deleting evidence. If a migration fails, reject the malformed envelope without partially applying the new state. Every handoff records the stop line and the smallest next investigation.

## 9. Definition of integration close

Plans 5–8 reach integration close only when each selected first slice has: one proven owner; one reachable consumer; schema-valid data; a deterministic command/result path; save or explicit stateless proof; a tested host route; accessible feedback; bounded performance; one narrative callback; focused verification; a reversible rollout; and a handoff accepted by the current integrator. Word count, content volume, or a polished mockup never substitutes for these receipts.

## 10. Handoff packet

- **Scope:** exact first slice and non-goals.
- **Ownership:** Core owner, host adapter, data authority, save owner, and event publisher.
- **Evidence:** source paths, catalog rows, focused command, output, and unresolved questions.
- **Behavior:** state ladder, duplicate handling, replay fixture, accessibility, and performance budget.
- **Content:** representative location/person, branch outcomes, localization keys, and delayed callback.
- **Recovery:** rollback switch, old-save behavior, malformed-input behavior, and stop line.
- **Next action:** one bounded implementation task with claimed paths.
