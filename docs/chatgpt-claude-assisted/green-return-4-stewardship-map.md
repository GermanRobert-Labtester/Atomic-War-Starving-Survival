# GR-4 — Stewardship Choices and Map Readout

STATUS: DRAFT — proposal for review; depends on GR-1 through GR-3 premise audits; no approval or ownership claim.

## 1. Objective

Give the player a truthful way to read the current land condition, understand uncertainty, and—only if an existing owner supports it—make a small stewardship choice whose cost and result are canonical. Keep the initial deliverable centered on a **read-only map/location report**. Do not invent a project system to force an action into existence.

The requested Green Return direction includes choices. The audit found no verified general field-restoration command in the inspected location owner. This plan therefore treats any action as conditional: either P0 identifies a real command that can be extended safely, or this package remains a map readout and submits a separate decision request for a new action owner. “Refuse harvest” and “allocate work detail” are examples to investigate, not approved mechanics.

## 2. Current Reality

`LocationEvolutionSystem` owns recorded location mutations and exposes methods for visit, clear, depletion, threat, owner, and daily environmental drift. Its fields are not a general work-order queue and `MarkCleared` means an expedition site was cleared; it is not land remediation. `LocationEvolutionSystem` has no observed general method to spend inventory and improve contamination or establish a protected area.

Inventory/resources already have canonical owners and transaction seams; an action must use the current consumer appropriate to its actual resource. A panel must not subtract counts locally. The map already has authored region/sector vocabulary and live danger escalation in `WastelandMapView`. It can potentially display GR-1/2's read model, but it does not own environmental condition.

Important collision discovered in the current source: managed greenhouse plots already expose soil contamination and fertility/production through the farming/soil-reclamation path (`Assets/Ashfall.Core/Farming/SoilReclamationProfileEngine.cs`, `src/Host/SoilReclamationProfileHostSession.cs`, and `src/UI/GreenhousePanel.cs`); `Assets/StreamingAssets/Data/narrative_questlines.json` contains `quest_the_irradiated_soil`. Those are adjacent plot and narrative owners, not an invitation to add a parallel wild-land soil cleanup mechanic. GR-4 is restricted to non-cultivated map land unless the existing soil-reclamation owner and foreman later approve a direct integration.

## 3. Required Delta

The smallest supported feature is a map/location readout whose display is assembled from GR-1 condition output, GR-2 evidence freshness, and GR-3 corridor reports. The player can see condition and uncertainty at a known location without creating state.

The optional action delta is only eligible if P0 finds a canonical player command with all of these properties: stable location ID, explicit preconditions, a single owner that can validate and commit the action, transaction-safe resource cost where applicable, deterministic result, event/refresh path, and persistence via its current save owner. The plan will not fill gaps by adding another inventory ledger, work detail system, stewardship task board, protected-zone registry, or site project queue.

## 4. Evidence

Snapshot checked 2026-09-29; repeat all ownership and command checks before implementation.

| Claim | Evidence | Consequence |
|---|---|---|
| Location mutations and clear/depletion facts have one existing owner | `LocationEvolutionSystem.cs` / `.Live.cs` | Read from it; new stewardship facts need its explicit acceptance. |
| Map already reads location threat/ruin for danger | `src/World/WastelandMapView.cs` `WorldEscalatedDanger` | Add condition detail separately; do not change danger meaning. |
| Resource inventory has a canonical owner and transaction path | Existing inventory and player-command owners; exact API must be found in P0 | Do not spend in panel or guess a method. |
| Agriculture and soil remediation already serve managed plots | `SoilReclamationProfileEngine`, `SoilReclamationProfileHostSession`, `GreenhousePanel` | Wild-land plan does not reproduce greenhouse soil state or fertility. |
| Narrative soil quest already frames remediation | `narrative_questlines.json`, `quest_the_irradiated_soil` | Do not create duplicate quest/reward/custodian arc. |
| Living Region owns settlement conditions and prices | `.ai/plans/living-region-2026-09-29.md` | No town governance, settlement benefit, refugee or price state. |
| Second Nature owns wildlife/crops/food webs | `docs/plans/story-expansion-batch-2/second-nature-and-ruins-of-the-before-2026-09-29.md` | No sanctuary population/reintroduction/farming changes. |
| Reconstruction Tree owns knowledge progression | `.ai/plans/reconstruction-tree-2026-09-29.md` | No technology unlock or research prerequisite added. |

Before claiming the UI surface, inspect current map controller, location details, resource transaction path, input command routing, save owner, and active worktree claims. Search for existing “restore”, “conservation”, “protected land”, “field work”, and cleanup commands beyond the first matching name; concept-level duplication matters.

## 5. Existing Extension Seams

Read-only map seam: selected node/detail provider in the existing map route, subject to exact audit. Environmental state seam: `LocationEvolutionSystem.TryGetRecord` and the GR-1 projection. Evidence seam: only real existing observation owner found by GR-2 P0. Corridor seam: existing sector graph/block/water state from GR-3.

Action seam: must be discovered, not assumed. Candidate command owners might include a field operation, expedition action, resource-consuming work system, or a genuinely applicable soil-reclamation command. An existing command is extendable only if its semantics match wild, non-cultivated land. Reusing greenhouse plot remediation on an unowned map location would be a semantic mismatch, even if it uses the word “soil.”

## 6. Proposed Architecture

### 6.1 Map report first

The view consumes immutable read projections for condition, evidence, and corridor status. It shows source/freshness and avoids conflating danger with recovery. Opening, selecting, refreshing, or closing the map performs no mutation.

### 6.2 Action only after ownership audit

At most two actions may be proposed for a first release, but the number is a ceiling, not a goal. P0 should prefer one well-owned, reversible action. Candidate intent vocabulary to investigate: (a) explicitly leave an extant resource/site undisturbed, or (b) commit a work effort through an existing owner. Neither action may be implemented from UI callbacks alone.

Before an action is accepted, define one atomic command that validates site eligibility, checks all costs, commits owner state, and emits success/failure. If costs span multiple owners and no atomic transaction seam exists, defer the action rather than risk partial consumption. If existing owners cannot own a protected designation or intervention record, stop for an architecture decision.

### 6.3 Outcome is reported only after owner commit

The map readout changes only after the authoritative owner records a result. A queued intention or paid cost is not evidence that contamination dropped or habitat returned. No map-side optimistic “recovered” badge.

## 7. Ownership Matrix

| Concern | Owner | GR-4 role |
|---|---|---|
| Land condition and location mutations | `LocationEvolutionSystem` | Read; possible action only if owner accepts contract |
| Condition/evidence/corridor projections | GR-1/2/3 contracts | Consume as read-only values |
| Resource balances and cost settlement | Existing inventory/resource owner | Command routes through its canonical transaction seam |
| Managed plot soil/fertility/yield | Soil reclamation/farming owners | Explicitly excluded from wild-land duplication |
| Wildlife/crops/food web | Second Nature's current owners | No mutation |
| Settlement health/economy/governance | Living Region/current settlement owners | No mutation |
| Research and recovered techniques | Reconstruction Tree/current research owner | No mutation |
| User input and presentation | Existing Godot route/controller | Bind command/result only; no domain rule |
| Stewardship history | None identified | Do not add until a canonical owner accepts it |

## 8. Data Flow

### Read path

Location/sector canonical catalogs → existing owners load/restore → GR-1 condition projection + GR-2 evidence projection + GR-3 corridor projection → map provider → selected-site report. Every fact retains its own owner and source date; the map does not flatten multiple dimensions into a synthetic recovery score.

### Conditional action path

Player selects an eligible site and explicit action → host command validates current selection and arguments → owning Core command validates rules and costs → canonical inventory transaction commits (if relevant) → canonical location/work owner commits its fact exactly once → success/failure result and existing event/refresh pathway → map rereads authoritative state → save captures through the existing owner.

Failure at validation or cost check produces no partial mutation. If an external cost owner cannot participate transactionally, the action is blocked before consuming anything. UI does not retry automatically.

## 9. State Model

Readout data is derived and transient. Candidate action state is not specified because no owner has been identified. Before any persisted action is proposed, its contract must define site key, action identity, start/end state, current status, day, actor/source, resources committed, idempotency key, cancellation/refund semantics, and versioning—only the minimal subset necessary to its existing owning feature.

Do not add a parallel `StewardshipState`, action list, protected-site list, reserve/material cache, or ecological progress field to a generic map DTO. Do not put a recovery percentage in the UI. If the site owner accepts exactly one durable fact such as a protection designation, prove it cannot be derived from an existing owner and define migration before review.

Action invariants, if approved:

- unknown/inaccessible/non-wild location cannot be mutated;
- validation occurs before resource reservation or deduction;
- one command ID cannot commit twice;
- failed/blocked command changes neither resource nor site state;
- refresh never replays an action;
- owner state can be captured/restored without the map;
- no action implies ecological effect outside its explicitly owned result.

## 10. API / Contracts

No command API is approved. P0 must inspect existing player-command conventions and transaction abstractions. A candidate command request needs explicit `locationId`, `actionId`, and stable request/idempotency key if the existing command host uses one. Avoid a generic project framework or extensible action plugin API.

The result should use existing `ActionResult` or current command-result type with precise refusal codes and localized keys. The UI can display costs and preconditions from owner-provided data but cannot recalculate costs or mutation effects. If no existing result/read-model pattern fits, ask the integrator to select one; do not create another.

## 11. Data Changes

No JSON additions are assumed. Existing `world_evolution_seeds.json`, `locations.json`, and map catalogs define locations and sectors; inspect whether data distinguishes wild sites from cultivated/greenhouse plots. Do not include greenhouse plot IDs or duplicate its soil profiles.

If an action requires a small authored eligibility/cost catalog, it must have a real loader, current integrity validation, duplicate ID and reference checks, range checks, and explicit missing-catalog behavior. Reuse an existing catalog if compatible. New data must be snake_case and authoritative under `Assets/StreamingAssets/Data/`; do not place mutable costs or success rules in Godot scene files.

No map prose may claim that one unit of resource produces a precise environmental outcome until owner behavior is approved and validated. Avoid line-per-location hand-authored claims that disagree with live owner state.

## 12. Save / Load

Map readout adds no save state. A future action must use the existing domain owner's save section, not create a Green Return save store by default. P0 identifies capture/restore, schema version, checksum, old-save handling, day restore order, and in-flight action behavior for each candidate owner.

If an action is instantaneous, atomic, and represented by existing location facts, no new action ledger may be needed. If it is multi-day, interrupted, refundable, or requires an audit history, the current owner may not support it; that is a material architecture decision and must stop the implementation until approved. Additive fields default to neutral/absent in old saves; absent must never read as a completed intervention. Generated save-store matrices are updated only by their generator and verified with `--check`.

## 13. Determinism

The report is deterministic and uses no RNG. Any action with a stochastic outcome must use the current campaign seeded RNG contract and stable action ordering; randomness is not necessary for the candidate choices and should be avoided. Do not seed by wall-clock time or map entry. Same inputs and command ID must produce the same accepted/refused outcome, with exactly-once mutation under retry.

Resource arithmetic uses owner-defined units and invariant rounding. The UI must not compute a parallel spend amount. Repeated map opens, save/restore, and command retries cannot change the map or spend resources.

## 14. System / Event Wiring

There is no new global day owner. If action is instantaneous, route through its current command owner and publish the existing event/result. If it has duration, use an existing work/project scheduler only if its current ownership actually covers this work; do not repurpose unrelated shelter construction. Define ordering relative to inventory, location mutation, day rollback, and save snapshot.

Rung/trend changes from GR-1/2 are generated by existing location facts; stewardship must not write a desired trajectory. Corridor display refresh can follow migration/world update, but action cannot force pack movement. No `OnLocationMutated` subscriber may infer a specific stewardship cause when that event does not carry command provenance.

## 15. Godot Integration

The existing map or selected-location surface is the only initial candidate. P0 records current scene, panel/controller, route, data refresh, focus restoration, action feedback, and disposal. Readout should separate: recorded condition; measurement date; known corridor/block state; danger status; and eligible actions. Unknown or stale remains visible and accessible.

If a command is approved, provide one explicit button per action with preconditions/cost and a clear result state. Disable or refuse stale selections using the owner command, not just UI state. Do not spend resources in signal handler, hide failure messages, or use color alone. Maintain keyboard/controller close/back behavior and focus after command results. If no legitimate action is found, a map-only report still constitutes GR-4's safe deliverable.

## 16. Narrative / Content Integration

Narrative content must read committed facts and must not award a trait, quest result, resource, research unlock, or wildlife change on a speculative map action. Existing `quest_the_irradiated_soil` has a specific cultivated soil remediation arc; do not retell or branch it as a second wild-land treatment without a separate narrative continuity review.

Keep map text reportorial: describe what is measured, when, and what remains uncertain. Do not label an intervention successful until the owner updates the measured dimension. Any new journal/radio line requires an existing event owner and exactly-once behavior; none is required for the readout.

## 17. Failure Modes

| Case | Required behavior |
|---|---|
| Site ID absent or unknown | Readout is unknown; command refuses without mutation. |
| Site is a cultivated/greenhouse plot | Defer to soil-reclamation/farming owner; do not offer duplicate wild-site action. |
| No valid action owner | Omit action; file a signed architecture request if action remains a requirement. |
| Insufficient resources | Refuse before mutation; report required/available using canonical owner. |
| Cost transaction partially fails | Roll back reservation/deduction atomically or block feature pending transaction seam. |
| Duplicate click/retry | Idempotency check; no double spend. |
| Stale selection after navigation | Revalidate location at command boundary; no action on previous row. |
| Map closes during request | Core action remains governed by command owner; no UI-owned cancellation assumption. |
| Save during multi-day work | Unsupported unless current owner persists/restores lifecycle exactly; stop if not. |
| Day rollback after action | Source owner restores both resource and site effects or action occurs outside rollback window by contract. |
| Owner update delayed | Show pending/last committed state; no optimistic recovered label. |
| Invalid/missing catalog | No new action availability; existing map still opens. |
| Corrupt save | Existing recovery path applies; no silently recreated local action cache. |

## 18. Test Strategy

This draft authorizes no testing or implementation. A future approved package runs focused targets with `bin/run-scoped-tests` after reviewing existing equivalent tests.

- provider tests: combines GR-1/2/3 projections without changing underlying owners;
- map selection/open/refresh does not mutate `CaptureState` or inventory;
- if an action is approved: happy path, refusal, zero-resource boundary, invalid site, stale target, duplicate request, exact-once spend, and rollback atomicity;
- save target verifies new action fact uses canonical owner and old saves remain neutral;
- no-data/invalid-data catalog test confirms current map still works and action stays unavailable;
- UI test verifies status text, costs, keyboard/controller focus, failure feedback, and panel disposal;
- narrow Godot headless route check only if active scene or runtime wiring changes.

Tests must prove behavior, not mirror every wording row. No full suite unless the user types exactly `RUN FULL TESTS`.

## 19. Dependency-Ordered Phases

### Phase 0 — command and collision audit (read only)

Inventory existing conservation, cleanup, protected-site, work detail, field operation, inventory transaction, location command, and map panel APIs. Confirm explicit separation from greenhouse soil reclamation and the irradiated-soil quest. Re-read Living Region / Second Nature / Reconstruction Tree scopes, GR-1/2/3 findings, ownership ledger, and dirty paths.

**Gate:** identify a matching command owner and transaction seam, or formally bound this release to the map readout. No implementation begins while the action owner is ambiguous.

### Phase 1 — read-model contract

Specify each displayed fact, owner, source day, known/unknown behavior, and refresh boundary. Confirm no overlapping danger, settlement, farm soil, wildlife, or research readout is being renamed.

**Gate:** map-only value remains useful and each label is traceable.

### Phase 2 — conditional action design

If and only if Phase 0 finds a compatible owner, write a narrow action contract for no more than two intents. Name authority, command, transaction, costs, state, idempotency, save, failure result, and rollback. If this implies a new general project/work system, stop and request a separate architecture decision.

**Gate:** foreman and affected owner sign the seam and exact path claims before editing.

### Phase 3 — Core command (conditional)

Implement validation and atomic effects in engine-free Core through the canonical owner. Preserve item/resource authority. No UI or authored content before command tests pass.

**Gate:** focused tests prove conservation/no partial mutation, retries, determinism, and restore behavior.

### Phase 4 — data and host adapter

Add only necessary validated authored values; route explicit UI action to Core. No domain logic in panel. If no action owner exists, skip this phase entirely.

**Gate:** catalog validation, host adapter result handling, and existing save owner verified.

### Phase 5 — map presentation

Bind GR-1/2/3 projections and any accepted command to the existing surface. Preserve accessibility, lifecycle, route, and exact current danger semantics.

**Gate:** focused source/UI test and narrow runtime check pass.

### Phase 6 — handoff and approval boundary

Document what is read-only, any accepted command owner, exact changed files, focused verification, remaining blockers, and untouched paths. Keep plan `STATUS: DRAFT` until explicit approval process.

**Gate:** no work proceeds into settlement, farming, wildlife, or research owner without its separate signed plan.

## 20. File Impact Map

Candidate areas only; P0 must validate each exact path and ownership before a package claim.

| File / area | Action | Reason | Risk |
|---|---|---|---|
| `Assets/Ashfall.Core/LocationEvolutionSystem.cs` | READ ONLY; conditional additive command only if owner accepts | Existing land mutation authority | High: semantic/save effects |
| Existing inventory transaction/resource owner | READ ONLY; conditional integration | Cost authority | High: conservation/partial-spend risk |
| `src/World/WastelandMapView.cs` and selected-location controller | READ ONLY; conditional MODIFY | Candidate report/action surface | Medium: UI/route/accessibility |
| `src/Main.CampaignOwners.cs` | READ ONLY unless integrator claims exact seam | Day order/rollback | Very high shared ownership |
| `Assets/Ashfall.Core/Farming/SoilReclamationProfileEngine.cs` | READ ONLY | Ensure wild-land feature does not duplicate managed plot owner | High if semantics cross |
| `src/Host/SoilReclamationProfileHostSession.cs`, `src/UI/GreenhousePanel.cs` | READ ONLY | Existing cultivated plot behavior and UI | Medium |
| `Assets/StreamingAssets/Data/narrative_questlines.json` | READ ONLY | Existing quest collision evidence | High narrative/content scope |
| `Assets/StreamingAssets/Data/world_evolution_seeds.json`, map data | READ ONLY; conditional MODIFY if existing catalog is correct | Canonical IDs/eligibility | Medium |
| Save registry / new GR save store | NO CHANGE | No new state approved | High duplicate authority |
| Living Region, Second Nature, Reconstruction Tree plans | READ ONLY | Scope boundaries | Governance risk |

## 21. Risks

- **Action without owner:** a map button can become fake operational theater. Mitigation: condition action on a real Core command, save owner, and transaction seam.
- **Partial resource spend:** cost and location effects may be owned by different systems. Mitigation: require atomic contract or do not ship action.
- **Wrong land domain:** greenhouse soil models cultivated plots, not wild-map ecosystems. Mitigation: explicitly exclude plots and reuse only with direct owner sign-off.
- **Optimistic recovery:** resource spend can be mistaken for a measured environmental result. Mitigation: wait for canonical updated fact.
- **Repeated clicks:** UI disables controls too late or command retries. Mitigation: owner-level idempotency.
- **Map semantics collision:** recovery color could override danger/locked state. Mitigation: separate text fields, keep navigation state untouched.
- **Scope creep:** protected land implies new governance, policing, land access and wildlife behavior. Mitigation: stop and create a separate approved architecture package.

## 22. Out of Scope

No generalized project system, work crew scheduler, protected-area enforcement, settlement charter/council, land ownership change, crop/fertility mechanic, greenhouse remediation, yield bonus, wildlife return/population effect, biological suitability, research gate, item sink, alternate inventory balance, map capture mechanic, new campaign mode, or second save system. No action is included just to meet the requested count.

## 23. Rollback Strategy

Read-only report rollback removes its map binding; saved world and inventory remain unchanged. For an approved atomic instantaneous command represented by existing owner state, remove the command surface and preserve already-committed canonical facts. Do not refund retroactively unless the command's approved transaction contract says to do so.

For newly persisted state, rollback requires a forward-compatible neutral reader and an explicit migration plan; never delete player decisions or state silently. If any feature requires an irreversible resource spend but no controlled rollback/failure contract exists, do not expose it in a draft implementation. Revert data and loader together, never generated outputs by hand.

## 24. Definition of Done

- P0 confirms the map surface, action search, inventory transaction, save owner, and all adjacent-plan boundaries.
- Readout is truthful, read-only, multi-dimensional, and explicit about source/freshness/unknown.
- Any action is optional, capped at two, routes through one canonical command and has atomic cost/effect semantics.
- Cultivated soil remediation and `quest_the_irradiated_soil` are not duplicated.
- No new generic work, project, stewardship, inventory, ecology, or save authority exists.
- Failures never partially spend or optimistically report recovery; repeated calls are idempotent.
- Focused tests for changed paths pass through `bin/run-scoped-tests`; exact command/result and limitations are recorded.
- Plan remains draft until explicit user/foreman approval; all non-target paths remain untouched.

## 25. Implementation Handoff

### MUST PRESERVE

The canonical location, resource, agriculture/soil, wildlife, settlement, and research owners; map danger/locked state; existing route/input/lifecycle; all current save and day rollback contracts.

### MUST ADD

First, a read-only map detail using GR-1/2/3 projections if those plans establish valid contracts. Add an action only when P0 identifies and receives approval for a compatible existing owner and transaction seam.

### MUST NOT DO

Build a parallel stewardship/project/work system; store resources or condition in UI; reuse greenhouse soil state for wild terrain; create duplicate quest content; promise a recovery effect before the owner records it; or modify wildlife, settlement, economy, agriculture, research, or shared day orchestration under this draft.

### VERIFY WITH

Changed-path scoped tests through `bin/run-scoped-tests`, plus targeted host/UI tests and Godot headless only if the active map route changes. No full test suite unless the user types exactly `RUN FULL TESTS`.

### FIRST SAFE IMPLEMENTATION STEP

Complete a read-only command/ownership map and draft a map-only mock state readout. Bring any candidate action back as a specific owner/transaction/save proposal before implementation.

## Appendix A — What qualifies as a stewardship action

An action is gameplay only if the player can make a choice and the canonical game state changes. A decorative “protect” button that only changes its own label is misleading; a button that subtracts stock but has no owner to record the site result is worse. Use this qualification matrix before promoting either example action.

| Requirement | Evidence needed | Acceptable implementation basis | Disqualifier |
|---|---|---|---|
| Target identity | Canonical map location/site ID | Existing location ID accepted by domain owner | Display-name string or guessed sector-to-site mapping |
| Eligibility | Rule owned by a Core domain system | Existing command preconditions or an explicitly approved owner extension | UI decides from danger color or text search |
| Player intent | One explicit command with a result | Existing action/command routing convention | Passive panel refresh or map selection |
| Cost | Canonical resource list and units | Existing inventory transaction/consume seam | Panel-local count, direct serialized balance edit |
| Effect | Domain fact with defined meaning | Mutation recorded by current owner after command succeeds | “Recovered” badge stored only in view state |
| Atomicity | Cost/effect transaction boundary | Existing transaction or a single owner commit | Consume first, then hope location mutation succeeds |
| Exactly once | Stable request identity or current command idempotency | Existing event/action key contract | Disable button only; network/frame retry can double-spend |
| Persistence | Existing capture/restore owner | Existing owning save DTO/section | New GR save file without architecture approval |
| Visibility | Committed result and failure message | Existing event/result and map refresh path | Optimistic UI that survives a failed command |
| Rollback | Defined behavior after rejection, save restore, or day rollback | Owner's established transaction/snapshot contract | Informal promise to refund or manually repair state |

If any row cannot be filled with current source evidence, the action does not pass P0. It may be resubmitted as a separate architecture proposal, but this plan cannot pre-approve it.

## Appendix B — Worked map and stewardship cases

### Case 1: radiation event at Fallout Zone Alpha

The authored evolution catalog has a hazard event that targets `loc_cut_radiation_zone_alpha`, adds a threat, and authors contamination delta `0.35`. The map can eventually report its recorded condition and current threat according to each owner. It cannot offer a “remove radiation” action merely because the event prose describes a plume. Such an action requires a canonical treatment command, supported resource costs, a result field, and a safe measurement path. Until those exist, the map row communicates the known state and date only.

If a future action is proposed, the acceptance test must prove that the command does not silently clear the existing threat, remove the sticky ruined flag, or change route danger unless those are explicit, separately owned effects. Clearing a contamination value must not imply the map route is unlocked.

### Case 2: flooded/collapsed infrastructure location

`world_evolution_events.json` describes a water-station collapse with depletion and a destroyed pump. An action that says “restore land” cannot reverse a structural collapse or make water potable as an incidental side effect. The existing landmark/water/infrastructure owner, if any, would have to own those outcomes. The safe map report keeps structural condition, water service, site contamination, and ecological readout separate.

### Case 3: current map selection is not stewardship

The player highlights an unvisited location. No state is written and no sample is created. The display can show authored location description and danger, then show dynamic condition as unknown if no live record exists. The selection cannot create a new mutation record, record an observation, raise confidence, or reserve supplies.

### Case 4: candidate “leave it undisturbed” action

This example may be a no-op in the current model: if there is no existing harvest at a wild site and no owned location protection flag, the player has nothing canonical to refuse. Do not add a local flag solely to make the choice visible. P0 must find an existing harvest command that can honor a player protection intent, or reject this candidate. If a valid owner exists, tests must establish that refusal does not consume cost and does not interfere with an unrelated expedition/trapping action.

### Case 5: candidate “allocate a work detail” action

This example sounds like an ongoing project and likely requires worker assignment, resource reservations, duration, interruption, and completion. Unless an existing owner already represents that exact field work, it fails GR-4's minimality bar. Do not borrow settlement governance from Living Region, greenhouse labor from farming, or shelter construction scheduling. A new multi-day work owner would require a separate signed architecture decision and a different plan.

### Case 6: cultivated plot remediation

The soil reclamation engine evaluates salinity, radionuclide load, organic matter, pH, amendment, germination viability, crop mutation risk, yield multiplier, and cultivability. Its host keeps plot entries and has a dedicated checksummed save store. That is materially richer and more specific than location contamination. GR-4 must not copy its result into `LocationMutationRecord` or map land band. A cultivated greenhouse bed is not an unmanaged region, and the same word “soil” does not make the records interchangeable.

### Case 7: save closes between cost and effect

Suppose a future command spans inventory and a location owner. If the first write commits resource cost and the process stops before the owner records the work, reload loses resources without an action result. A UI-side repair or refund is not acceptable. The action is blocked until one owner or current transaction abstraction can commit the related effects atomically, or the operation has a durable, recoverable pending contract.

### Case 8: repeated input and day rollback

The player double-clicks; then the campaign day owner rolls back the day snapshot after a later owner fails. GR-4 requires the canonical command to define whether that action is outside the day transaction or restored with it. Replaying the same request cannot spend twice or leave a condition mutation behind. If the request identity is lost on rollback, a fresh retry must still have exactly-once behavior according to current command convention.

## Appendix C — Map readout acceptance matrix

This readout can stand alone even when no action owner is found. It should preserve detail rather than compressing incomparable facts into a single score.

| Readout row | Source | Example state | Acceptance condition |
|---|---|---|---|
| Authored site identity | Existing location/map catalog | `loc_cut_radiation_zone_alpha` | Existing selection and route behavior unchanged |
| Dynamic location record | `TryGetRecord` | Present / absent | Absent shows unknown and creates nothing |
| Contamination | Location owner | Value/band per GR-1 contract | Dimension and data freshness are explicit |
| Disturbance | `isCleared`, `lootDepletionFactor` | Cleared / salvage thinned | Does not claim ecological degradation |
| Ruin/threat | Location owner and existing danger presentation | Ruined, threat list, authored danger | Existing danger and locked semantics unchanged |
| Trend | GR-2 evidence contract | Unknown / direction over named dates | Never appears from one reading or visit timestamp |
| Corridor | GR-3 and migration owners | Known edge / current block / unknown suitability | Sector granularity; no pack spawn or location precision |
| Action affordance | Matching canonical command only | Available / refused / not supported | No fake button; feasibility revalidated in Core |
| Last updated | Source owner day | Campaign day N | Source date, not UI open time |

### Refresh and consistency rule

The controller should build all rows from a coherent committed snapshot. If GR-1, GR-2, and GR-3 owners update at different points, the UI must either show per-row source days or defer refresh until a known integration boundary. It must not imply that every row comes from one sensor sweep. On selection change, clear the previous location's transient rows before binding the new selection; do not leave stale action availability on screen.

## Appendix D — Command refusal and feedback contract

If and only if a command is approved, Core or its canonical host owner should determine feasibility. The view may hide an unavailable option for clarity, but the command must still reject invalid, stale, or malformed requests. A refused command should return a stable code and a user-readable localization key; it should not expose internal exceptions or silently refresh into a seeming success.

| Refusal cause | State effect | Player feedback | Retry rule |
|---|---|---|---|
| Unknown location | None | Site is unavailable/unknown | No automatic retry |
| Wrong site category | None | Action does not apply here | User may navigate elsewhere |
| Not enough resources | None | Show canonical required/available values if owner supports it | Re-evaluate after stock changes |
| Another operation owns the site | None | Existing owner reports current lock/use | No overwrite |
| Invalid authored action ID | None | Generic unavailable content result; log validation failure | Do not guess a fallback |
| Duplicate request | No second effect | Return original/idempotent outcome if supported | Safe to refresh |
| Transaction cannot commit | Full rollback | Clear failure state; preserve balances | Retry only after owner state recovers |
| Save/restore interrupted | Per owner contract | Show pending or last committed status | Never infer completion from spend |

This contract prevents failure feedback from becoming a new gameplay authority. Exact codes should reuse current conventions instead of adding a parallel error taxonomy.

## Appendix E — Cross-plan boundary examples

| Player-visible request | Correct owner family | GR-4 response |
|---|---|---|
| “Will this settlement gain food or residents?” | Living Region / canonical market and population owners | Do not calculate or promise it |
| “Will species return here?” | Second Nature / migration and ecosystem owners | Show current sector corridor facts; state suitability unknown if no provider |
| “Can I amend this greenhouse bed?” | Soil reclamation / farming | Route to its current panel and command, not a Green Return action |
| “Does this recovered book unlock remediation?” | Reconstruction Tree / research | Do not add unlock or prerequisites |
| “Can I protect this named wild map site?” | No owner verified yet | Record action-owner gap and request a narrow decision; do not fabricate a project system |
| “Is this road passable?” | Map/route infrastructure owner | Keep route passability separate from land condition |

These boundaries are acceptance constraints, not merely documentation. If the eventual design needs one of these outcomes, identify and approve that cross-owner seam before implementation.

## Appendix F — Conditional command request/result contract

No particular command exists in the evidence collected so far. The table below defines what must be found or approved before one can be drafted. Every name and schema shape is **PROPOSED / VERIFY**.

### F.1 Request inputs

| Candidate field | Why needed | Validation owner | Failure behavior |
|---|---|---|---|
| `location_id` | Stable target key | Current location catalog + domain owner | Unknown target refuses |
| `action_id` | Explicit intent, not a freeform UI string | Existing action catalog/command owner | Unknown action refuses; no fallback |
| `request_id` | Retry idempotency if existing command system needs it | Existing command boundary | Duplicate returns prior result or safe duplicate response |
| `actor_id` | Needed only if current owner records actor/capability | Existing survivor/command owner | Never fabricate an actor from selected row |
| `expected_state_version` or source revision | Optional stale selection protection | Use only if canonical owner exposes versioning | If absent, revalidate live preconditions in Core |
| `expected_cost` | Should generally not be caller-controlled | Resource owner | Never trust UI-supplied amount as authoritative |

The narrowest request is preferable. If the existing command contract already uses typed domain commands, follow it instead of adding JSON-shaped request DTOs. Do not expose internal inventory IDs, floating-point costs, or authorization booleans as panel-owned rules.

### F.2 Result

Use the existing `ActionResult` or command-result contract if it can represent the outcome. A successful result should identify the owner-committed fact or event key so host refresh can reread it. Failure codes should distinguish invalid target, ineligible site, unavailable action, missing resource, duplicate request, owner conflict, and transaction failure only where current conventions support those distinctions.

Result does not contain a promised ecological percentage unless the owner actually calculated and persisted that outcome. It does not carry a client-side “new contamination” value for the panel to install. A result is a notification that the command owner completed or refused a request; current owner state remains authoritative.

## Appendix G — Transaction and resource accounting cases

### G.1 No cost is not zero-cost gameplay by default

An action may require labor/time, special access, a tool, or a location claim rather than a stackable item. Absence of a resource field in a plan is not an instruction to make the action free. The existing owner must define whether cost is applicable and how it is represented. A zero-resource command is acceptable only if its action semantics and opportunity cost are defined by an existing system.

### G.2 Cost preview

If the surface displays a cost, the preview comes from the same authored/owner contract used for validation. It is explanatory and can become stale. The final command rechecks exact canonical inventory. The UI cannot reserve resources during preview and cannot assume the previewed balance remains current while the player changes screens.

### G.3 Resource removed, site mutation rejected

This is a prohibited partial commit. The implementation must either validate all site preconditions before the transaction and use an atomic commit, or use a durable reservation/commit protocol already owned by the system. A new one-off refund path would need its own exactly-once semantics and is not a smaller change than atomicity.

### G.4 Site mutation applied, resource owner rejects

Also prohibited. The target action must not modify `LocationMutationRecord` before cost commit succeeds. If the resource authority cannot cooperate with the site owner, the action does not ship in this package.

### G.5 Duplicate command after UI timeout

The UI may not know whether the Core command committed if its result callback is delayed. If the current host supports idempotent requests, re-submit the same request identity and retrieve the existing result. If it has no such contract, disable auto-retry and resolve status from owner state. A double-click debounce is useful presentation hygiene but cannot substitute for Core idempotency.

### G.6 Conservation check

Where item stacks are spent, a focused test compares pre/post canonical balance and committed action facts. `before = after + committed_cost` for the exact item IDs in one successful command. A refusal or rollback proves `before == after`. Avoid a second test ledger or UI mock that asserts only the rendered number; the canonical inventory owner is the evidence.

## Appendix H — Scenario interaction grid

The grid names other systems only to protect seams. It does not create combined gameplay behavior.

| Scenario | GR-1 condition | GR-2 trend | GR-3 corridor | GR-4 behavior |
|---|---|---|---|---|
| Unseeded authored site | Unknown record | No samples → unknown | No sector map → unknown | Show site identity plus explicit missing live evidence; no action |
| High current contamination, no prior sample | Current fact only | Unknown, one sample | Suitability unknown | Do not offer remediation unless matching owner and command exist |
| Contamination decreases after clear days | Current value may decrease by owner tick | No trend unless two comparable readings persist | No automatic corridor change | Refresh current state; no stewardship credit |
| Wildlife pack enters neighboring sector | Site state independent | No land trend from movement | Report sector-level movement | No new radio/journal/spawn; action availability unchanged |
| War blocks destination sector | Site state independent | No land trend from blockage | Current incoming block distinct from possible outbound escape | Map action cannot bypass blockade |
| Greenhouse plot near named location | Location record remains site-level | Plot sample cannot serve as wilderness history | Wildlife graph stays sector-level | Route cultivated work to soil/farming owner; no duplicate button |
| Expedition clears a site | `isCleared`/depletion facts update | Visit alone is not sample | Pack route unchanged unless owner changes it | Report clear state; no “restoration completed” |
| Site is locked for travel | Condition is still separate | Stale/unknown as evidence dictates | Wildlife corridor not human route | Readout respects lock and does not open the node |
| Cost stock changes after preview | Condition unchanged | Trend unchanged | Corridor unchanged | Revalidate Core command; no local spending |
| Save restore between selection and action | Rebind live site snapshot | Recompute from canonical samples | Rebind topology/block state | Stale target request refuses/rechecks; no duplicate action |

## Appendix I — Save and corruption decision tree

### I.1 Map-only release

There is no map-derived save field. After load, the current owners restore and seed according to existing policy; the provider reconstructs the report. If a saved map cache exists elsewhere for performance, P0 must prove it is presentation-only and invalidated on owner restore. Do not add one in this feature.

### I.2 Instantaneous action represented by existing field

If one existing location-owned field fully and truthfully represents the action result, the owner may need no schema change. This must be demonstrated. The action still needs an exactly-once command contract if it costs resources. Save round-trip proves both canonical resource and site state persist. Old saves retain their previous semantics; missing action field is not converted to success.

### I.3 New owner state required

If the proposal needs an ongoing work item, active duration, reserved material, actor assignment, progress or cancellation, the current evidence does not show a matching wild-land owner. Stop. A new owner would need a formal architecture decision, approved plan and exact claim; this GR-4 draft does not authorize it.

### I.4 Corrupt or partial state

Use the current save-store behavior, not a local repair. A malformed action fact must not be treated as completed or charged if owner contract says no commit. The UI may show unavailable/unknown while the recovery path resolves. It cannot zero an inventory cost, recreate site state, or write a new action record to make the map appear consistent.

## Appendix J — Exact route and accessibility walkthrough

The eventual presenter review should walk through a complete interaction, not just inspect a screenshot:

1. Open the existing map route from its normal parent surface; confirm initial focus follows current behavior.
2. Navigate by keyboard and controller to a location with no live record; confirm unknown state is textually clear.
3. Select a location with a current record and, where available, an old or missing sample; confirm each row names the source day/unknown reason.
4. Change selection; verify no prior location's condition, action availability, or cost remains visible for a frame in a misleading way.
5. If an action was explicitly approved, invoke one refusal case and verify no resource/site mutation; invoke a successful case only in its focused test fixture and verify owner result.
6. Navigate away during a pending result; verify provider unbind/disposal and no subscription leak.
7. Reopen the panel after day advancement or load; verify it refreshes from restored owner state rather than retained controls.
8. Check focus visibility, label contrast, font scale/overflow at fixed target resolution, and textual alternatives to status colors.

This is a future narrow UI verification sequence. It does not authorize running the entire snapshot suite for a read-only provider-only change; use the current UI test policy and changed path selection.

## Appendix K — Go/no-go review for action candidates

For each candidate, record the following. An “unknown” answer means no-go for that candidate, not a reason to improvise.

| Question | Green answer | Red answer |
|---|---|---|
| Is this a map location or managed crop plot? | Domain is explicit and one owner serves it | Feature mixes both |
| Does a current command already represent this intent? | Exact API and caller identified | Intent exists only in prose |
| Does that owner persist its result? | Capture/restore is current and tested | Panel or static data holds result |
| Is inventory cost atomic with effect? | Existing transaction/owner seam | Separate writes without rollback |
| Does action have a meaningful outcome? | Owner state changes and is observable | Button changes only its own state |
| Does map display measure success? | Result reads back from owner | UI assumes success from cost paid |
| Can duplicate input be safe? | Owner idempotency or exact-once command | UI debounce only |
| Does it change wildlife/settlement/crops/research? | No, or separately signed cross-owner package | Hidden side effect under GR-4 |
| Can rollout reverse safely? | Additive surface; persisted facts retained | Needs deleting player state or arbitrary refund |

## Appendix L — Suggested approval package if a new action is demanded

If design review insists on an action and P0 finds none, a later architecture package should be no larger than:

- one specific player intent and one location category;
- one named Core owner and exact persistence contract;
- one command request/result and idempotency rule;
- one resource transaction contract, or an explicit zero-cost justification;
- one measurable, dimension-specific output owned by that system;
- one location/map surface with accessibility acceptance;
- old-save and rollback behavior;
- focused test selection under `TEST_POLICY.md`;
- named boundaries with Living Region, Second Nature, SoilReclamationProfile/Greenhouse, and Reconstruction Tree;
- stop conditions if the transaction, measurement, or ownership premise fails.

That package would be a new decision for review. This draft remains a truthful map and stewardship-readiness plan until such approval exists.

## Appendix M — Map knowledge, deformation warnings, and stewardship scope

### M.1 Existing map survey does not measure recovered land

`CartographySystem.ProjectCanonicalMap` in `Assets/Ashfall.Core/Exploration/CartographySystem.cs` produces `CanonicalMapSurvey` rows containing node ID, display name, fog state, survey quality and tier, last-confirmed day, provenance source, and knowledge-source kind. This is a useful candidate for map freshness and node identity. Its “survey” means map knowledge, not a soil or ecology instrument reading. A visited node, detailed map tier, or fresh confirmation cannot enable a remediation command, establish an ecological baseline, or certify a safe site.

The map report should therefore keep at least two independently sourced labels where both apply: “map knowledge confirmed on day D” from Cartography and “current contamination record updated on day E” from `LocationEvolutionSystem`. If one source is absent, preserve that absence. Do not copy Cartography discovery into a Green Return sample history, and do not make map selection record a survey.

### M.2 InSAR is an adjacent geophysical warning, not a stewardship result

The current InSAR owner is concrete and persisted: `InSarDeformationEngine` records bounded repeat `SurveyPass` rows (`MaxPasses = 256`), chooses the latest reference geometry, requires at least two compatible passes over a positive day span, and calculates coherence, relative displacement in millimeters, trend velocity, confidence, and classification. Low coherence is retained as low-confidence/inconclusive rather than represented as a clean map. The engine exposes travel and excavation warning queries. `InSarMappingHostSession` and `Main.InSarMapping.cs` route pass recording and processing; `insar_deformation` is a registered save section.

These facts can inform a separately labeled geophysical warning if the map already has an authorized consumer. They cannot confirm that a stewardship action worked. InSAR does not own contamination, soil fertility, species return, vegetation, crop yield, water safety, or intervention causality. A drop in deformation is not evidence that land remediation succeeded. Conversely, a deformation warning does not invalidate a measured contamination change; both dimensions may coexist and should remain separate.

| Map row | Source and valid claim | Action consequence GR-4 may take |
|---|---|---|
| Map knowledge | Cartography: node is rumored/surveyed/visited with provenance and confirmation day | May explain identity/freshness; never authorizes work by itself |
| Location condition | Location owner: current mutation values at canonical location | May display the actual available fields; does not grant action authority |
| InSAR deformation | InSAR: compatible repeat-pass result or explicit low confidence | May show the existing warning if current UI supports it; cannot show stewardship success |
| Wildlife corridor | Migration/ecology owners: current sector topology, projected block, pack/observation facts | May show route context; cannot turn an action into wildlife recovery |
| Managed plot soil | Soil reclamation/farming owners: plot-level fertility/remediation | Remains a cultivated-plot feature, outside wild-map actions |

### M.3 Duplicate-plan receipt: wildland fire plan

The duplicate audit inspected `docs/plans/expansion_wave1/PLAN_01_WILDLAND_FIRE_AND_BURN_RECOVERY.md`. The header marks it `DRAFT — premise and authority audit required`; its declared scope is report-to-recovery stories and optional outdoor incident mechanics subject to current-owner review. The closing receipt is documentation-only and `READY FOR PREMISE AUDIT`, explicitly with no implementation claim. It has no `FULLY INTEGRATED` status in the inspected text.

Pass 167, “Wildlife Corridor Journals,” is a generic ten-field template. It names no concrete Green Return command or record, and gives no specific corridor schema, production caller, map route, save section, or acceptance result. Nearby topicized passes reuse the same generic phrases. This is adjacent planning material, not a duplicate implementation or complete plan. It is still a mandatory duplicate-check citation: any future action audit must search the complete wildland plan for overlapping work, but must not treat its template wording as approval to add stewardship state. Its own body says wildlife population/migration remain with existing wildlife systems and forbids outdoor incident content from writing their saves.

### M.4 No-join decision cases

| Case | Display | Do not do |
|---|---|---|
| Map node known, no location condition record | Show map knowledge; condition unknown | Backfill condition from the survey tier |
| Location condition exists, no Cartography confirmation | Show location record with its real source/day; map knowledge remains unknown | Treat a location record as map exploration progress |
| InSAR summary is low coherence | Show the owner’s low-confidence warning if the UI can do so | Hide the result as “stable” or offer remediation as a fix for sensor confidence |
| InSAR displacement trend is stable | State deformation classification and unit | Label ecology recovered or disable all stewardship options |
| A paid action succeeds but measured condition is unchanged | Show owner-committed action outcome separately from condition | Optimistically set a green/recovered map badge |
| Current condition improves with no recorded action | Display the dimension-specific change only if its history is legitimate | Credit the player or imply intervention causality |

Each join needs a canonical ID relation and explicit source provenance. Shared display labels or approximate map geometry are not enough. These are review rules, not a new join registry or cache.

## Appendix N — Conditional action lifecycle and failure acceptance

This matrix is for a future owner-approved command only. It does not assert that any of the candidate actions exists today. A map-only delivery can pass without implementing any row in the command column.

| Lifecycle stage | Required owner behavior | Map behavior | Reject if |
|---|---|---|---|
| Preview | Reads a canonical site and current resource availability; makes no reservation unless current owner explicitly supports one | Shows source state, proposed cost, eligibility reason, and expiry/refresh rule | Preview spends, reserves invisibly, or caches cost past an inventory change |
| Submit | Typed intent includes stable location ID and duplicate-safe request key if owner contract requires it | Disables duplicate input only as presentation; sends exactly one owner request | Panel changes map state before owner result |
| Validate | Owner re-reads location eligibility and resource balance at commit point | Presents owner refusal text while preserving selection/context | Preview result is trusted after day/save changes |
| Commit | One canonical transaction settles costs and the named action result | Waits for committed event/result; refreshes from owner | Partial cost is charged while effect fails, absent a documented compensation contract |
| Save | Owner capture/restore persists the accepted result and any idempotency fact required | Rebuilds view from restored owner state | Map has a separate action ledger or save bit |
| Rollback | Existing day/command transaction restores the full committed unit | Discards transient optimistic state and refreshes | Resource/site save sections diverge after failure |
| Repeat | Owner decides whether repeat is blocked, idempotent, or a new action | Displays actual repeat policy and result | UI debounce is the only duplicate protection |
| Correction | Owner revises/cancels using an explicit policy with provenance | Shows corrected result and source state | UI rewrites history or refunds by local arithmetic |
| Decommission | Existing owner keeps player state readable; additive map view can be removed | Falls back to canonical owner details | Removal deletes or reinterprets saved facts |

### N.1 Refusal cases that must remain distinguishable

| Cause | Required response shape | No side effect |
|---|---|---|
| Location missing or ID stale | `site_unavailable` / equivalent owner reason | No resource debit or record creation |
| Condition source stale | Refresh/unknown status; do not submit if eligibility depends on it | No optimistic success |
| Not a wild-land site (managed plot or settlement asset) | Route to its owning UI or explain scope | No mutation through GR-4 |
| Cost unavailable | Owner refusal identifies resource/cost if safe to expose | No partial debit |
| Competing day transaction in progress | Existing command serialization/refusal behavior | No duplicate action |
| Save/restore not complete | Disable command until owner is ready | No empty-default assumption |
| No owner command found | Keep report read-only and record unresolved design request | No synthetic button/action state |

### N.2 Cross-owner acceptance rows

| Neighboring domain | Review question | Accepted boundary |
|---|---|---|
| Inventory | Is there an atomic cost API at the intended command owner? | Cost is settled exactly once with owner result, or action is deferred |
| Location evolution | Does its current command vocabulary actually mean the proposed land action? | No reuse of `MarkCleared` or a contamination field for an unrelated intervention |
| Soil reclamation/greenhouse | Is target a managed cultivated plot? | If yes, greenhouse/farming owner controls soil; Green Return map does not duplicate it |
| Wildlife migration/ecology | Does action influence population, migration, or habitat rules? | If yes, separate wildlife owner plan and approval required |
| Living Region | Does action change settlement, governance, price, food security, or resident status? | If yes, settlement owner and plan must own it |
| Reconstruction Tree | Does action unlock research or recovered knowledge? | If yes, research owner controls that progression |
| Cartography | Does action change map discovery/confirmation? | Only Cartography command may mutate its map knowledge |
| InSAR | Is action merely reacting to deformation warning? | InSAR remains a separately sourced warning; it does not become action history or proof of ecological recovery |

### N.3 Stop/continue review example

Example proposal: “Spend 3 units of scrap to stabilize the old embankment and mark the sector recovered.” The phrase bundles at least three concerns. Scrap consumption requires inventory transaction ownership; stabilization may concern deformation and belongs with the owner of that physical risk; “recovered” asserts an ecological/land measure not supplied by the proposed command. Unless an existing command already owns the exact stabilization operation and exposes a canonical measurable outcome, the entire action is a no-go. A smaller accepted scope might show the existing InSAR excavation warning and separately display current land condition, with no action. The plan must not rename a paid repair as ecological recovery.

## Appendix O — Worked regional map lifecycle and release boundary

Use `loc_grange_hall` as the selected site. Its authored seed names `sector_4_hinterlands`, owner `none`, contamination `0.1`, and no threats. `loc_ration_queue_plaza` also maps to that sector and is seeded with owner `faction_the_compact`, contamination `0.1`, and no threats. Wildlife seed data places `pack_hinterland_dogs` in the same sector with population 6. These facts support a useful regional display but do not prove that the two locations have identical condition or that the pack visits either site.

| Time | Owners read | Map composition | Permitted action/result |
|---|---|---|---|
| Fresh campaign before seeding completes | Existing map topology; location/world owner may not yet have records | Node identity plus loading/unknown condition | No command; empty default is not clean |
| Seed completes, before first day tick | Map seed and location record | Current location value, separate sector link, no trend | Selection stays read-only |
| Day 1 after clear-weather tick | Location owner current value | Updated value with source/date only as supported | No claim that player caused decay |
| Expedition visits site | `MarkVisited` and relevant expedition outcome | Visit context only if current map exposes it; condition is separate | Visit is not a sample or stewardship record |
| Hazard-weather day commits | Location owner tick and existing threat RNG | Refresh after owner commit; threat and condition remain separate | Map never ticks/retries campaign day |
| Dominant faction projection updates wildlife blockers | Host owner reprojects blockers after migration/ecology steps | Block status may appear after day commit, distinct from land condition | Map does not reproject or change movement |
| InSAR summary exists for same sector | InSAR owner | Separate deformation class, confidence, and units | No stewardship success/failure inferred |
| Future approved field command submitted | Named command owner validates target and cost | Owner result then canonical readback; no optimistic green badge | Command remains conditional until exact API is found and approved |
| Save/reload | Location, wildlife, Cartography, and InSAR restore through their owners | Provider re-queries each authority; absent blocker remains unknown until rebuilt | No Green Return shadow save/cache |

### O.1 Proposed read-only map card contract

| Section | Proposed field(s) | Authority | Required qualifier | Acceptance rule |
|---|---|---|---|---|
| Identity | Canonical node/location ID, display name | Existing map/catalog provider | Map/catalog source | Selected node resolves; never synthesize ID from display text |
| Map knowledge | Fog state, quality/tier, last-confirmed day | Cartography `CanonicalMapSurvey` | “Map knowledge” | Never presented as physical measurement |
| Site condition | Current contamination, ruin, cleared, threats, visit date | `LocationEvolutionSystem` | “Current location record”; source day only if exposed | Missing record is explicit unavailable |
| Trajectory | Dimension, direction, dates, source references | GR-2 projector over qualified evidence | “Measured over [interval]” | Comparable samples and accepted comparison rule required |
| Corridor | Topology, blocker, pack/observation facts | Wildlife owners and host projection | Sector scope / projection freshness | Location join is canonical; otherwise unknown |
| Geophysical warning | Deformation class, displacement unit, confidence | InSAR owner | “Sector deformation” | Same canonical sector and approved consumer required |
| Stewardship action | Availability, cost, result | Future named command owner only | Owner and commit day | Existing atomic command contract approved |
| Travel availability | Current route and expedition result | Travel/expedition owner | Existing route source | Condition panel never mutates route status |

Each section may be visually grouped, but facts retain separate provenance. A single green/red composite is not acceptable unless a future owner defines and persists its meaning. The panel does not translate the values into one score.

### O.2 Conditional command state transitions

These are review requirements if an existing wild-land command is found; they do not create a parallel project ledger.

| State | Entry fact | Owner behavior | Player-visible result | Abort/recovery |
|---|---|---|---|---|
| Unavailable | No valid selection, seed pending, or restore unresolved | Read only | “Site information unavailable” with known reason | Wait for readiness or select a canonical node |
| Inspecting | Canonical ID selected | Read owners only | Current facts with individual provenance | Selection change invalidates previous detail |
| Eligible preview | Existing owner confirms target and cost | Preview only, unless owner supports reservation | Exact scope, cost, duration, uncertainty | Revalidate before submit; stale preview expires |
| Refused | Owner rejects stale target, missing cost, or wrong class | No domain mutation | Owner refusal, selection context retained | Refresh owner data; no retry loop |
| Submitted | Typed command accepted for processing | Existing owner transaction | Pending only if command is actually asynchronous | Cancel only if owner supports it |
| Committed | Owner settles cost and action result atomically | Canonical write and event | “Action recorded” with commit day | Read back owner state; repeat follows owner idempotency |
| Outcome awaiting evidence | Work result exists, no new measurement | No inferred ecological write | “Action recorded; land change not yet measured” | Remain so until a source produces comparable evidence |
| Measured follow-up | Later source records compatible sample | Measurement owner writes; GR projector reads | Dimension-specific trend and dates | Insufficient pair stays unknown |
| Corrected/superseded | Source owner revises a fact | Owner-managed lineage | Corrected indicator and effective source | Preserve lineage; do not silently overwrite |

### O.3 Save cohort and missing-data behavior

| Save/load condition | Read-only map behavior | If action is added later |
|---|---|---|
| Legacy save has location records | Show supported current owner fields | Missing action field means no recorded action |
| Selected location has no record | Unknown/unavailable; seeder follows existing policy | Not eligible until owner record is valid |
| Cartography migration yields unknown fog | Keep map knowledge unknown | Never backfill environment from fog state |
| InSAR method is missing/retired | Show absent or unresolved context per InSAR policy | Do not substitute new method or Cartography tier |
| Wildlife save restored before blocker projection | Current pack may be shown; block unknown | No action may exploit transient empty projection |
| Resource restore fails or is partial | Follow save-store recovery; action availability unavailable | No fake success or local refund arithmetic |
| Corrected sample lineage exists | Show source owner’s effective reading with correction marker | Correction cannot grant repeat reward |
| Partial/corrupt section | Affected facts unavailable; other valid owner facts remain separate | Green Return never repairs other owners’ state |

### O.4 Release acceptance matrix

| Acceptance area | Read-only release passes when | Command release additionally requires |
|---|---|---|
| Selection purity | Opening/selecting/refreshing leaves owner snapshots and RNG unchanged | Submit passes one typed request only |
| Missing data | Unknown remains explicit and no field is defaulted to zero | Missing owner record refuses with no debit |
| Provenance | Cartography, location, wildlife, and InSAR labels identify their own domains | Cost/effect result names committing owner |
| Trend | Only comparable measurements produce a direction | Committed action is not treated as measurement |
| Corridor | Sector-level facts retain day/block freshness | Command does not mutate pack or blocker unless separately approved |
| Save/load | Providers rebuild from canonical owner restore | Owner action round-trips once; old saves mean no action |
| Determinism | Stable output ordering and no RNG use from query | Retry/dedup/replay yields one committed result |
| Accessibility | Keyboard/controller focus and text alternatives work in the current route | Refusal/pending/success states are navigable and understandable |
| Rollback | No projection survives owner rollback as stale display | Resource, site, and action state roll back atomically |

A screenshot cannot prove selection purity, save ownership, or replay. Use owner snapshot tests and a narrow route probe only if UI code changes. A command release also requires the exact command owner/API, active claim, duplicate policy, refusal/no-debit case, save round trip, rollback/retry fixture, and measured-outcome wording. If any owner or test seam is missing, the accepted delivery remains map-only or stops for architecture review.

## Appendix P — Existing UI route evidence and interaction boundary

The current Godot presentation has several adjacent map/location surfaces; none should be mistaken for a ready-made Green Return panel.

| Surface | Current source evidence | Candidate reuse | Boundary to preserve |
|---|---|---|---|
| `WastelandMapView` (`src/World/WastelandMapView.cs`) | Binds `WastelandMapSystem`, creates markers from nodes/intel, derives marker danger via `WorldEscalatedDanger`, and emits `NodeSelected(nodeId)` | Potential selection signal to a separate read-only detail provider if the scene has a compatible consumer | Map marker still owns fog/status and authored danger; Green Return cannot mutate node state or alter locked semantics |
| `MapPanel` (`src/UI/MapPanel.cs`) | Projects canonical map knowledge into a cartography-confidence card with charted count, average survey quality, rumor/survey/visit progression, and latest provenance | Possible companion coverage row only if environmental completeness has its own meaning | Cartography values measure map knowledge, not land health |
| `Main.GetCartographyProjection` (`src/Main.Cartography.Integration.cs`) | Calls `SetupWorld`, then projects existing `WastelandMap.Nodes` and `Knowledge`; it is read-only | Pattern reference for a Core projection bridge if the condition report is equally read-only | Does not supply location condition or a second region graph |
| `ExpeditionPanel.BuildWorldStateLine` (`src/UI/ExpeditionPanel.cs`) | Reads the location record and renders owner/unclaimed, spoilage, ruin, threat count, and optional flavor | Reference for established location-detail vocabulary | Does not present contamination as ecology status; preserve meanings |
| `Main.EvolvingWorld.ComposeExpeditionDangerMultiplier` (`src/Main.EvolvingWorld.cs`) | Uses location threats and `contaminationLevel > 0.6f` in expedition-danger calculation | Must be re-audited if map danger copy is touched | Green Return output must not feed back into gameplay calculation |

`WastelandMapView.OnNodeSelected` logs and emits a string ID. This is a plausible event seam for selection, not proof that an existing compatible detail panel subscribes. P0 must trace the signal through scene wiring and parent lifecycle, verify the ID equals `LocationMutationRecord.locationId`, and record focus/close/dispose behavior. `MapPanel` overview is a different route from the map marker scene; do not assume they share a selection model.

### P.1 Route contract and review walk

| Route step | Existing or proposed event | Review assertion |
|---|---|---|
| Open overview map | Existing `MapPanel` route | Cartography card remains correct in its own semantic domain |
| Open world map | Existing scene/parent route | Focus follows current contract; opening performs no write |
| Select marker | Existing `NodeSelected(nodeId)` signal | One canonical ID is passed to provider |
| Resolve condition | Proposed `TryGetRecord(nodeId)` read | Missing row remains unknown; no `GetOrCreateRecord` call |
| Render detail | Proposed condition plus independent corridor/InSAR rows | Each row retains source, unit, and date if known |
| Change selection quickly | Existing signal may fire repeatedly | Stale A result cannot overwrite selected B |
| Advance day | Existing campaign coordinator commits | Refresh after commit; no second day tick |
| Day rollback | Phase owner restores location snapshot | Rebind from restored state; discard provisional view |
| Close detail | Existing parent route | Restore focus and dispose future listeners |
| Reopen after load | Owners restore before requery | Old cached fields do not mask restored data |

### P.2 Selection and accessibility acceptance cases

| Input path | Expected behavior | Rejection condition |
|---|---|---|
| Mouse selects marker | Detail matches marker ID after owner query | A similarly named location’s record appears |
| Keyboard activates marker | Same result and source rows as pointer | Focus vanishes or command route differs |
| Controller selects and backs out | Back closes detail and restores prior focus | New detail route traps focus or bypasses parent close |
| Rapid select A → B | B response owns final detail | Late A response appears as current |
| No location record | Condition unavailable; unrelated map facts remain | Blank region reads as zero/healthy |
| Long localized source label | Text wraps or exposes complete accessible label | Source or uncertainty qualifier is clipped |
| Color-blind palette | Text/icon preserves status meaning | Color alone communicates recovery |
| Increased text scale | Owner, unit, date, unknown reason remain visible/navigable | Scaling clips evidence qualifiers |
| Scene disposed mid-refresh | No stale callback touches removed controls | Subscription survives route closure |

No new animation or map tint is necessary. If the existing detail route cannot show provenance, date, and unknown state without changing owner semantics, use an existing textual report surface or defer the UI. Visual emphasis cannot compensate for missing evidence.
