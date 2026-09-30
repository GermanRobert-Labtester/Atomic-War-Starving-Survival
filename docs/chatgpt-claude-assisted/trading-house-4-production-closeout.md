# TH-4 — Production Commitments and Closeout

STATUS: DRAFT — proposal for review; depends on TH-1 through TH-3; no ownership claim, approval, or implementation authorization.

## 1. Objective

Let the Trading House report whether an existing producer actually made goods that satisfy an existing exchange commitment, then close that commitment through its canonical contract, inventory and settlement owners. The first safe result is a read-only proof path for one named producer, not a general production market.

The plan does not authorize a new production simulator, reservation/warehouse store, production queue, stock count, recipe, item ID, inventory, escrow or settlement owner. “Committed,” “scheduled,” “recipe available,” “batch complete,” “output buffered,” “inventory delivered,” and “contract fulfilled” are distinct states. A House display must report which fact it has, and who owns it.

### Completion standard

This plan is implementation-ready only after Phase 0 names one existing producer with a traceable completion fact, one canonical item/inventory handoff, one existing contract/consignment completion verb, and one save path for each mutable fact. If no single producer satisfies those constraints, the package remains a read-only investigation and reports the gap as P0. It may not infer output from a recipe, completed event text, aggregate total, or resource forecast.

## 2. Current reality

Production in the repository is plural and system-specific. A generic type name does not mean one shared production authority.

| Observed seam | Evidence | What it proves / does not prove |
|---|---|---|
| `Assets/Ashfall.Core/Production/IOutputSink.cs` | `DeliveryBill` has `SourceSystemId`, `Reason`, `Day`, and item lines; `DeliveryResult` exposes status, delivered/rejected counts and undelivered lines; `IOutputSink` declares `Deliver` and `CanDeliver` | A contract exists. `rg` found no production implementation/use of `IOutputSink` in Core/host/tests beyond its declaration and a comment in `ResourceMassBalanceSimulator`. It is not evidence of a wired producer receipt path. |
| Silent Foundry | `SilentFoundrySystem` stores `FoundryProductionRecord` history (`productId`, `amount`, quality, `completedDay`, workers, provenance); `OnProductionCompleted` fires. Host composition is through the Expansion Hub, and foundry state is captured under existing save ownership | There is a completion fact and historical record. This audit did not establish that record amount equals delivered inventory quantity or that it carries a unique stable batch ID consumable by a House contract. |
| Powder metallurgy | `Assets/Ashfall.Core/Foundry/PowderMetallurgySystem.cs` calls `Inventory.TryExecuteTransaction` for input bill and later `Inventory.TryProduce` for its output | It reaches canonical inventory, but transaction sequence, batch ID, rollback, output receipt, host save/tick wiring and use as a contract deliverable require exact audit. |
| Pharmaceutical tablets | `PharmaceuticalTabletEngine` has an output buffer and `ClaimOutputs`; host has `PharmaceuticalTabletHostSession`, `pharmaceutical_tablet` save section and canonical inventory binding | Output can be buffered/claimed, and output has a persisted owner. A House needs to distinguish buffer quantity from claimed inventory; stable per-batch receipt semantics remain P0. |
| Oilseed pressing | `OilseedPressingEngine` is a yield evaluator; `OilseedPressingHostSession` is paired with canonical inventory and `oilseed_pressing` save section (Plan 118) | It has a real consumption/delivery integration, but the exact consumed inputs, output grant, receipt identity, and duplicate-call behavior need a fresh package audit. |
| Radio production | `RadioProgramProductionSystem` owns prep jobs and delivery facts for broadcasts, has its own save state and host wiring | This produces a broadcast/event, not trade goods; it is excluded as a cargo producer. |
| Shared inventory | `Assets/Ashfall.Core/Inventory/Inventory.cs`, `InventoryBill`, `InventoryTransaction`, and `InventoryHostSession` expose canonical item state, quotes/validation, atomic transaction support, and capture/restore | Inventory is the goods authority. A producer’s own historical count must not be added to inventory a second time. |
| Market / contract systems | Market owns prices/stock; route contracts and the TH-2 scope own contract references; Contract Board 109 discusses a separate contract lifecycle/escrow seam | The House cannot treat a market listing, authored recipe, or a proposed TH commitment as an accepted existing contract. Cross-plan ownership and overlap must be settled first. |

## 3. Required delta

The delta is to bind three already distinct facts for one supported, concrete path:

1. An existing contract/consignment identifies a deliverable using canonical item IDs and quantity.
2. One existing production system reports a real completed output with enough provenance to identify the batch or event exactly once.
3. Canonical inventory custody and the existing exchange owner confirm actual delivery/closeout.

If those facts cannot be joined through stable public references, this plan must not invent a reservation or receipt ledger to fill the gap. It should stop with an evidence-based interface gap for a separately authorized design.

## 4. Evidence and duplicate check

### Direct evidence inspected

- `Assets/Ashfall.Core/Production/IOutputSink.cs`: delivery bill/result/interface shape, with no identified implementation consumer.
- `Assets/Ashfall.Core/Inventory/Inventory.cs` and `InventoryTransaction.cs`: canonical inventory, `ValidateTransaction`, `Quote`, `BeginTransaction`, `TryExecuteTransaction`, `TryConsumeBill`, `AddById`/`TryProduce`, and capture/restore APIs.
- `src/Host/InventoryHostSession.cs`: the production inventory host, catalog-backed item access, transactions and save-state capture/restore.
- `Assets/Ashfall.Core/Foundry/SilentFoundryTypes.cs`, `SilentFoundrySystem.cs`, and `SilentFoundrySystem.Heat.cs`: saved production history and `OnProductionCompleted` event.
- `Assets/Ashfall.Core/Foundry/PowderMetallurgySystem.cs`: canonical input transaction and output through inventory; exact per-batch link needs reinspection.
- `Assets/Ashfall.Core/Medical/PharmaceuticalTabletEngine.cs`, `src/Host/PharmaceuticalTabletHostSession.cs`, and `src/Main.PharmaceuticalTablet.cs`: output buffer/claim API, canonical inventory host callbacks and `pharmaceutical_tablet` persistence.
- `Assets/Ashfall.Core/Farming/OilseedPressingEngine.cs`, `src/Host/OilseedPressingHostSession.cs`, and `src/Main.OilseedPressing.cs`: yield engine, host integration and `oilseed_pressing` save ownership.
- `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`: `silent_foundry`, `powder_metallurgy`, `pharmaceutical_tablet` and `oilseed_pressing` are existing source-specific keys. Their presence does not prove all backup/capture/restore routes are correct; implementation must trace each touched path.
- Existing focused targets found include `Ashfall.Core.Tests/Production/Plan35ProductionDeliveryTests.cs`, `Plan35_43ProductionGovernanceIntegrationTests.cs`, `Ashfall.Core.Tests/Foundry/FoundryPlan129IntegrationTests.cs`, `SilentFoundrySystemTests.cs`, `Ashfall.Core.Tests/PharmaceuticalTabletEngineTests.cs`, and `Ashfall.Core.Tests/Farming/OilseedPressingTests.cs`. Exact targets depend on the selected producer.
- `docs/plans/integrated/farming/INTEGRATED_PLAN_PRESERVATION-TRUTH-118.md`, `docs/plans/integrated/medical/INTEGRATED_PLAN_PHARMACEUTICAL-TRUTH-167.md`, `docs/plans/integrated/economy/INTEGRATED_PLAN_155_BLACK_MARKET.md`, `docs/plans/EXPANSION_PROGRAM_WAVE9_2026-09-21/PLAN-CONTRACT-BOARD-109.md`, and the Freight/Underworld proposal references in `docs/chatgpt-claude-assisted/README.md` are related boundaries, not permission to copy or rewrite their work.

### Duplicate and overlap findings

No existing “The Trading House”/TH-4 plan was found in the scoped documentation search; the parent duplicate sweep recorded the broader title/destination check. There are completed producer integrations, existing contracts, canonical inventory and market owners, and Plan 109’s proposed board lifecycle. Therefore the gap is not “make production happen.” It is the currently unproven join among an existing accepted obligation, unique produced output, canonical custody, and exactly-once contract settlement.

Plan 118 and Plan 167 are already integrated and must remain authoritative for oilseed and tablet outputs. Any later implementation must use current APIs after their closeout evidence is rechecked. It must not restart those plans to create House-specific outputs. The Contract Board plan remains a coordination dependency: House closeout cannot create a second posted-work/escrow lifecycle or silently assume Plan 109 is implemented.

### P0 VERIFY — premises blocking a commitment API

1. **Select one producer:** audit current host/API for a smallest candidate (for example, oilseed, tablets, powder metallurgy or foundry) and name one exact supported path. Do not create a generic registry covering every producer.
2. **Completion reference:** prove a stable unique batch/job/record ID exists and survives save/load. Foundry’s visible completed history lacks an established unique ID in the inspected record shape; do not synthesize a key from list position.
3. **Output authority:** determine whether output is buffered, in inventory, or only recorded historically. For tablet production, buffer and claimed inventory are separate; for Foundry, event/history and inventory grant must be reconciled; for oilseed/powder, trace all branches and failure paths.
4. **Atomicity:** establish whether input consumption, producer completion and output grant form one transaction. A successful recipe completion followed by failed inventory grant cannot count as delivered goods. A second grant after retry must be impossible or compensated through existing owner behavior.
5. **Contract owner:** identify the real accepted contract/consignment owner and its stable ID, completion verb, refusal behavior, expiry/shortfall rules and save path. TH-2’s concept is not proof that the owner exists.
6. **Quantity and quality:** prove compatible units and the rule for partial amounts, quality/provenance, contamination and accepted substitutes. No invisible rounding or automatic downgrade.
7. **Custody and reservation:** determine whether inventory already supports reservations. If not, House may issue a promise/reference but cannot promise exclusive goods or prevent other consumers taking stock. Any reservation proposal is a separate owner decision; no parallel warehouse ledger.
8. **Ownership collision:** audit current `INTEGRATION_PLANS.md` / `WORKTREE_OWNERSHIP.md`, the completed Plans 118/167 and Plan 109 status; claim exact paths before edits.
9. **Current path reachability:** prove the chosen production owner is composed in the campaign, has the correct day tick and restore order, and produces observable output in a real campaign route.
10. **Receipt permanence:** decide whether the canonical contract owner already stores the fulfillment fact and source reference. If it does, project it. If no owner can store it, stop for an approved receipt/contract state design rather than appending a duplicate House history.

## 5. Existing extension seams

- `Inventory` is the only inventory quantity authority. `InventoryBill` and `InventoryTransaction` provide preflight/atomic application seams; use them only through the owner path the producer already uses.
- Producer-specific completion events/history/output buffers are the source of truth for production. Do not reinterpret `CompletedCount`, recipe availability, cumulative output, or an event’s display string as a fresh delivery.
- TH-2 may hold an exchange reference. It cannot claim a completed contract unless its selected contract owner already exposes a completion verb and record.
- Existing producer host sessions/save stores compose source state and may be queried by the application owner. They should not be pulled into House UI as direct mutation dependencies.
- `IOutputSink` is only a possible future seam if Phase 0 proves its contract matches a real producer and canonical inventory transfer. The interface itself is not a safe shortcut or mandate to refactor every producer.

## 6. Proposed architecture

### Default slice: receipt-backed closeout read path

Choose one already-live output pipeline. The House shows a compact trace such as:

`commitment reference → producer source record → claimed/delivered canonical inventory → existing contract result`

Each step is read from its owner. If no step has a stable ID, the next step stays unavailable. No separate House production state is written.

### Conditional command slice

If an existing contract owner already has an exactly-once completion verb, the House may route the user action to it after its normal preconditions are revalidated. Production should complete through the producer’s own command/tick. Inventory transfer must use the existing canonical transaction. The House may not directly grant output, debit input, mutate the contract, or keep a second state machine to coordinate three non-atomic writes.

### Scope if owner APIs are incomplete

The smallest authorized next design, if later approved, should add the missing fact to the authority that owns it:

- unique output receipt ID belongs to the producer if its completed output otherwise cannot be referenced;
- custody/transfer receipt belongs to inventory transaction owner if absent;
- fulfillment status/reference belongs to the contract owner if absent.

This is a routing rule, not approval to modify any of these owners. Before implementation the integrator must decide which missing receipt is actually required, whether it can be derived, and which current owner is allowed to persist it. Never add all three copies to House.

## 7. Ownership matrix

| Concern | Sole owner | Trading House responsibility |
|---|---|---|
| Recipe/catalog availability and production rules | selected producer’s Core engine and authored catalog | Display producer-authored capability only |
| Batch progress and completion event | selected producer | Reference real completion fact; never manufacture it |
| Output buffer/claim state | producer that owns the buffer (e.g. tablet engine) | Show unclaimed vs claimed distinctly |
| Item definitions, amount in shelter, canonical transfers | `Inventory`/`InventoryHostSession` | Query custody or route an existing transaction |
| Market price and market stock | `MarketSystem` | Use its quote/result only when actual exchange path requires it |
| Accepted obligation, delivery rule, expiry and completion | existing contract/consignment owner, to be selected | Carry its stable reference; route its command |
| House membership/authority | TH-1 owner, still P0 | Check permission; no production policy |
| Display and interaction | Godot presenter/host in `src/` | Show source state and command result |
| Persistent history | existing source owners | No House receipt/warehouse store by default |

## 8. Data flow

### Promise/read flow

`accepted existing contract → source item/amount requirement → producer capability/queue state → canonical producer completion record → inventory custody/transfer fact → derived House status`

No recipe unlock skips the completion step. No cumulative production counter substitutes for a batch receipt. No inventory quantity is attributed to a producer without a source owner reference.

### Completion flow, only if existing owner verbs support it

`player confirms → contract adapter validates current contract identity/status/due day → existing producer reports matching completed receipt → inventory owner validates and applies one bill → contract owner closes against that receipt → source owners emit state change → existing save owners capture each state → House refreshes`

The ordering above is a set of required invariants, not a proposed multi-owner transaction. Phase 0 must prove an existing transactional composition or the command is out of scope. Never implement partial cross-owner writes and rely on the House to retry.

### Error flow

`owner preflight/result → typed refusal with owner + source ID + reason → UI feedback → refresh derived state`

The House reports a shortfall without manufacturing it. It does not retry production or grant compensation goods on its own.

## 9. State model and invariants

Default state is entirely derived. A status projection may contain source references and display-only categories such as `offered`, `accepted`, `in_progress`, `output_ready`, `output_unclaimed`, `delivered`, `shortfall`, `expired`, and `unavailable` only if the source owner’s existing status map supports them. These labels are conceptual; no shared enum is authorized.

Do not persist:

- promised quantity duplicated from a contract;
- recipe yield prediction as actual output;
- stock reservations, warehouse balance, delivery queue, or spoilage clock;
- duplicate output receipt, completion flag, delivery counter or fulfilled bit;
- cached contract status, producer state, or inventory totals.

Invariants:

1. Recipe unlock, scheduled production, batch complete, output buffer, claimed output, inventory possession, shipment and contract closeout are separately represented facts.
2. One output receipt may satisfy at most one contract unless the canonical contract owner explicitly supports shared allocation.
3. A contract can close only through its owner and only once.
4. An amount delivered cannot exceed the owner-verified amount physically available and accepted by inventory/contract rules.
5. Quantity and units match canonical catalog/contract values; item aliases resolve through current canonical rules.
6. Unknown producer, missing source record, missing item definition, stale contract, insufficient output, capacity failure or unloaded owner blocks settlement without phantom success.
7. The derived House view is disposable and does not mutate its sources.

## 10. API and contract sketch

No new generic production interface is approved. Phase 0 should fill this mapping before any API proposal:

| Need | Current owner contract to prove | Stop if |
|---|---|---|
| production status | selected engine’s public job/batch query | only text, aggregate total or private save DTO is available |
| unique output fact | stable receipt/batch key with item, amount, completion day and status | key is positional, regenerates after restore or does not bind to actual output |
| inventory delivery | existing output claim or `InventoryTransaction` path | House must call `AddById` after an unrelated success callback |
| contract fulfillment | selected owner’s idempotent complete/fulfill command | the House must invent lifecycle, deadline or escrow behavior |
| failure/refusal | owner result with reason and no partial mutation | partial write has no compensation/restore boundary |

If `IOutputSink` becomes relevant, verify it has a real implementation, producer integration, inventory-backed behavior, save semantics, and tests before proposing to extend it. A contract interface with no live consumer is not an extension seam in production.

## 11. Data changes

Default: no JSON change. Existing product recipes and item catalogs remain authoritative; a House demand cannot add a product or alter its yield. A commitment references a canonical item ID and uses the existing owner’s unit semantics.

If data work becomes necessary, list the exact catalog and loader, schema version, item/product references, amount range and unit, contract consumers, integrity validator and compatibility rule. The producer output must resolve through the existing `items.json`/domain catalog path. No duplicate demand catalog or House recipe catalog is authorized. No prose-only output item may appear as physical inventory.

## 12. Save/load and migration

The House read model has no saved state. Contract, producer, inventory and market owners retain their existing checksummed save sections and restore ordering. The first candidate audit should inspect:

- producer save section and backup/store composition (`silent_foundry`, `powder_metallurgy`, `pharmaceutical_tablet`, `oilseed_pressing`, or another selected owner);
- inventory section and source restoration order;
- contract/consignment section and migration path;
- host dirty flags and SaveOrchestrator capture behavior;
- old-save defaults for absent producer receipts/output buffers.

After save/reload, a completion reference must resolve to one source receipt and match inventory and contract state. Old saves without a receipt remain unfulfilled/unavailable; they must not fabricate a receipt from cumulative counters. No new `trading_house_production` section or migration is authorized. If the existing owner needs a save schema change, open a separate design for that owner and preserve old saves before this plan proceeds.

## 13. Determinism

The House adds no production RNG. Producer output, yield, quality and job identity continue to use the producer’s existing seeded RNG and state. The House must not reroll yield on query, preview, panel reopen, save restore or contract closeout.

Derived rows sort on ordinal stable owner/source IDs. Amount formatting is invariant-culture. No IDs may use `Guid.NewGuid`, wall-clock time, dictionary iteration or UI label. If a producer’s output is seeded, its existing stream must not be shifted by an extra House sample. Repeated query/preview must leave the RNG and producer save checksum unchanged.

## 14. System/event wiring

Subscribe to the chosen producer’s already-live completion/state event and existing contract/inventory change events only if a stable source reference is provided. On each event, rebuild the derived view. Unsubscribe on panel/session close and campaign reset. Event ordering must not assert “contract fulfilled” before inventory commit succeeds.

Do not register a new daily producer tick. The producer’s established day owner remains responsible for generation; TH-4 must not progress a batch. If the producer has a claim-output action, its own engine/host handles it. The House may call its public owner command only after a user action and P0 confirms that is the intended route.

No generic retry queue: save/load or duplicate event delivery must not create output twice. Any retry or dedupe behavior belongs to the producer, inventory transaction, or contract owner that owns the underlying state.

## 15. Godot host integration

Potential UI is a House commitment detail/readout, connected through the current `Main`/host owner and whichever source presenter is already supported. It may display requirement, accepted amount/quality, producer identity, actual production state, output custody, due date and final contract state—each sourced and labelled.

The UI must keep distinct “ready for collection” from “in inventory” and “delivered.” If only a completion record is known, it cannot show shipment or settlement. It must present a bounded refusal such as output unavailable, owner not loaded, capacity failure, quantity shortfall, contract stale or delivery rejected. Do not provide a fake “complete” button backed by a local callback.

Use current panel lifecycle: initial focus, keyboard/controller focus movement, back close, visible feedback after commands, refresh after source events, disposal unsubscribe, and readable IDs/details at supported scale. UI handles input and presentation only; it does not calculate yields, match substitute goods, reserve quantity or mark contract fulfilled.

## 16. Narrative and content integration

No new narrative is required for the first receipt slice. If an existing contract provides authored text, preserve it and attach factual state from the owner. Any later notice about delay, shortage, spoilage, quality rejection or labor dispute must be tied to an actual event from the responsible system; do not invent spoilage or imply a producer broke a promise because no stock reservation exists.

Do not attach faction/treaty consequences from Foundry history to Trading House contracts unless the existing treaty owner explicitly binds that delivery. Existing product provenance and quality labels remain producer facts. Flavor text cannot act as a second contract term.

## 17. Failure modes and expected behavior

| Condition | Expected behavior |
|---|---|
| Recipe exists but no completed batch | In progress/not ready; no deliverable amount shown |
| Producer completed event but no stable receipt ID | Read-only completion fact or unavailable; cannot close contract |
| Output is buffered and not claimed | Show unclaimed buffer; do not count it as inventory or delivered |
| Output already claimed into inventory | Show source only if producer receipt can be joined; inventory still owns current custody |
| Another system consumes goods before House settlement | Revalidate inventory; report shortfall; no phantom contract completion |
| Inventory capacity/weight/type rejects delivery | Preserve owner failure result; no contract closeout |
| Partial production or quantity below commitment | Report exact owner-backed quantity; partial settlement only if contract owner supports it |
| Contract unknown, stale, expired or already closed | Owner refuses; no output or money mutation from House |
| Duplicate completion event or command retry | Existing owner idempotency must yield one output/closeout; otherwise stop before implementation |
| Save after production but before claim/delivery | Restore the exact owner state; no duplicated output on resume |
| Legacy save lacks receipt or buffer field | Apply owner’s documented legacy default; never reconstruct from narrative/history count |
| Missing producer/catalog/item definition | Mark source unavailable; no guessed output or fallback item |
| Quality/contamination mismatch | Follow producer and contract policy; refuse if no canonical compatibility rule |
| Day tick occurs twice or events arrive out of order | House does not tick or write; source owner’s idempotency governs, otherwise block the path |
| Producer host session disposed/reset | House detaches event handlers and invalidates the view |
| Very large amounts/float precision | use source precision; reject overflow; do not silently round to whole units |

## 18. Test and verification strategy

No tests are run while drafting. Before implementation, recheck the package’s exact target and use `bin/run-scoped-tests` only. Full suite requires the exact phrase `RUN FULL TESTS`.

### First targets after producer selection

- one producer’s existing Core tests (e.g. `OilseedPressingTests`, `PharmaceuticalTabletEngineTests`, `FoundryPlan129IntegrationTests`/`SilentFoundrySystemTests`, or Powder Metallurgy tests after locating exact current filename);
- existing `Plan35ProductionDeliveryTests` only if generic `IOutputSink`/delivery is truly in the chosen path;
- `InventoryTransactionTests`, `UnifiedInventoryOwnershipTests` or `InventorySystemTests` only for a changed inventory boundary;
- existing selected contract/consignment tests (must be identified at Phase 0; TH-2 concept alone gives no test path);
- host wiring/save test for exactly the touched integration.

Do not run all producer tests by keyword or expand broad regressions unless changed paths or a concrete risk require them. Do not add tests that duplicate producer output tests; new tests should prove the missing join/receipt/settlement contract.

### Acceptance matrix

| Case | Starting facts | Action | Pass condition |
|---|---|---|---|
| recipe is not output | recipe/catalog entry active, no production | query House | no completed/delivered quantity claimed |
| stable receipt | one real batch completes | query House | exact owner key, item, amount, quality/provenance and day displayed |
| output buffer | batch ready but not claimed | query/claim via owner if approved | buffer remains distinct; no duplicate inventory grant |
| canonical custody | producer’s existing handoff succeeds | query inventory/source | one item delta and one traceable receipt |
| failure compensation | output grant/capacity fails | execute path | no committed receipt or contract fulfillment, or canonical owner supplies safe compensating result |
| partial output | producer delivers less than commitment | settle attempt | owner refuses or returns supported partial result exactly once |
| duplicate reference | same receipt/contract command repeated | repeat after success/save restore | no second output, inventory delta or completion |
| stale/expired contract | deadline/status changes after view | attempt closeout | contract owner refuses with no mutation |
| unrelated inventory consumption | stock changes after preview | commit | source owner revalidates; shortage is visible |
| save/reload boundaries | save after batch, after claim, after settlement | restore and query | producer buffer, inventory count and contract status agree; no phantom row |
| deterministic repeat | identical seeded run and command sequence | compare receipts/state | same output facts and owner checksums; House queries do not shift RNG |
| missing optional source | producer/session not mounted | open House | unavailable source, unaffected other commitments |
| host lifecycle | open, subscribe, close/reset, source event | inspect UI/session | one refresh while open; no disposed-handler effects after close |

## 19. Dependency-ordered phases

### Phase 0 — producer and contract receipt audit (blocking)

**Why:** the generic delivery interface has no identified live consumer and each production engine owns different output behavior.

Inspect candidate producers, exact host composition, day owner, save section, source record ID, inventory transfer, catalog ID/unit and relevant contract owner. Compare owner APIs and focused test cases. Select exactly one pipeline whose completion output can be traced to canonical inventory and one real accepted contract.

**Gate:** documented end-to-end data path, public methods, immutable/unique IDs, mutation order and failure behavior; current plan/claim check; exact files claimed. If any link is speculative, return P0 gap and stop.

**Do not touch:** producer Core state, generic `IOutputSink`, inventory schema, contract-board/escrow implementation, new catalog/data, save registry, generated docs, or production balance.

### Phase 1 — evidence-backed read model

**Why:** verify contract → producer → inventory references without making economic mutations.

Build a derived display projection using existing public DTOs/events and no persistence. Keep missing references explicit. The first acceptance proves actual one-batch output truth against source state.

**Gate:** projection carries stable source IDs and exact owner values; querying it does not change source state, save checksum, inventory, RNG or day tick; deterministic order and disposal are verified.

### Phase 2 — host composition and observable route

**Why:** ensure source owner is actually reachable in the campaign and the House surface displays live data.

Wire the presenter through the current application owner, respecting the producer’s existing startup/restore sequence and `Main` lifecycle. Use current state-change events. No new producer session or duplicate host composition.

**Gate:** focused host integration proves the real route from restored campaign through completion fact into House view; optional/unloaded owner is unavailable; no new save section/day tick.

### Phase 3 — closeout command, separately gated

**Why:** transaction correctness is a distinct risk from displaying a batch.

Proceed only if an existing contract owner and canonical inventory transaction already provide idempotent closeout. Route one explicit user action to that owner; revalidate all facts at mutation time. If three owners require independent writes without atomic coordination, do not proceed.

**Gate:** conservation test for inputs/output/inventory/settlement, repeated command, failure, partial amount, stale contract and save/reload passes in exact focused targets.

### Phase 4 — content/UX polish, only after facts are correct

Add no item or narrative content unless the chosen producer/contract schema requires it. Add accessible state explanation using established terms and facts. Confirm no source status is overstated.

**Gate:** focused UI/accessibility/runtime check and proof every label maps to source facts.

### Phase 5 — integration closeout

Only the named foreman/integrator updates shared ledgers. Keep the plan DRAFT until approved. After full implementation, follow the project’s mandatory integrated header and immediate archive procedure under the approved category; do not archive this proposal merely because the audit finished.

## 20. File impact map

All production rows remain READ ONLY until Phase 0 selects the single path and ownership claim exists.

| File/area | Action | Reason | Risk |
|---|---|---|---|
| `Assets/Ashfall.Core/Production/IOutputSink.cs` | READ ONLY; no change unless a real consumer is proven and integrator approves | Abstract bill/result contract | High: broad producer API could invite fan-out refactor with no proven benefit |
| selected producer Core engine + its state/catalog | READ ONLY first; narrowly MODIFY only if missing receipt field is proven necessary and owner decision signed | Authoritative output | High: determinism/save/old-save semantics |
| `Assets/Ashfall.Core/Inventory/Inventory.cs`, `InventoryTransaction.cs` | READ ONLY by default | Canonical custody and atomic bill support | High: all item consumers and save state |
| selected producer host/Main/session/save files | READ ONLY first; small integration change only if current route is missing | Composition, restore, events, save | High: duplicate session/tick/save ownership |
| selected contract owner | READ ONLY first; modify only under its own approved plan/claim | Acceptance and fulfillment status | Very high: deadline, escrow, lifecycle and migration |
| `Assets/StreamingAssets/Data/<selected catalog>` | NO CHANGE by default | Existing production/item authority | Medium/high: validator and balance dependencies |
| TH-1/TH-2 House host/presenter | possible derived query/presentation changes after ownership audit | Expose source facts | Medium: shared integration seam |
| producer/inventory/contract focused tests | READ ONLY then narrowly add missing integration assertion | Validate selected path | Medium: avoid duplicate coverage |

## 21. Risks

1. **Output history mistaken for stock:** completed records may outlive goods, or historical amounts may not reflect inventory. Use current inventory and source references.
2. **Buffered output duplicated:** claim already grants inventory, while House grant adds it again. Route only through owner claim/transfer.
3. **Non-atomic three-owner mutation:** production, inventory and contract each commit independently. Stop unless an existing coordinator owns compensation.
4. **Invented availability:** unlocked recipe, labor or projected yield is presented as guaranteed delivery. Label capability versus actual output.
5. **Parallel warehouse:** House reservation count diverges when other systems consume goods. No reservation unless canonical inventory owner already supports it or a separate signed decision is made.
6. **Wrong closeout authority:** House marks a TH-2 reference complete while Plan 109/other contract owner owns deadline and escrow. Resolve cross-plan boundary first.
7. **False receipt identity:** sequential list position or timestamp makes non-durable IDs. Reuse stable owner ID; add none without owner approval.
8. **Save ordering:** inventory restores before producer/contract state or vice versa and exposes duplicate claim. Verify actual restore dependencies and old saves.
9. **Quality mismatch:** amount alone misses output tier, contamination, or provenance. Preserve producer result; use only existing contract acceptance rules.
10. **Scope explosion:** generic adapter touches every production system. Limit initial integration to one complete path; each additional owner requires its own evidence and claim.

## 22. Out of scope

- a general factory/production queue or production manager;
- cross-producer batch registry, universal output receipt store, warehouse, stock ledger, reservations, shipping manifests or spoilage simulation;
- new production recipe, output item, resource, item alias, catalog, market price or product quality system;
- House-owned contract board, escrow, deadlines, penalty policy, cargo ownership or settlement;
- producer balancing, yield changes, unlock progression or day-tick changes;
- attaching radio broadcasts or abstract narrative outputs to physical inventory;
- direct mutation of inventory or source state from Godot UI;
- broad `IOutputSink` retrofit, save-schema migration or cross-owner event framework;
- unrelated integration of freight, railway, caravan or black-market systems.

## 23. Rollback and recovery

Keep any authorized package separable into read-only query/presentation and a later closeout command. The derived query can be removed without migrating saves. The producer and its inventory owner remain usable with the House closed.

For a later source schema change, preserve the previous serialized fields and write an explicit migration, restore-order test, and recovery note before enabling new commands. Never delete/rewind player inventory or contract state to hide a failed adapter. Recover only through existing owner backup/restore and explicit owner compensation. If duplicate production or item transfer is possible and cannot be reversed safely, disable the House command while leaving source producer operation intact, then return for a contract-owner correction.

## 24. Definition of done

- A current duplicate, plan-status and path-ownership audit is recorded.
- Exactly one real producer-to-inventory path and one real contract owner are selected from live evidence.
- Stable IDs, output units, quality rules, event order, host composition, save/restore and failure behavior are documented.
- House display distinguishes projected, started, completed, buffered, claimed, held, delivered and settled states according to the facts actually exposed.
- Query/presentation is derived, deterministic, disposable and mutation-free.
- A command exists only if canonical owners already provide safe transaction and idempotency boundaries; no local status bit or retry queue substitutes for them.
- No parallel output, inventory, stock, warehouse, contract, funds or settlement authority is added.
- Focused targets pass through `bin/run-scoped-tests`; exact commands/results, runtime limitation, and any P0 gap are reported. No full-suite run without exact authorization.
- Save/reload, duplicate command, failure compensation, ownership, lifecycle and accessibility gates apply to every modified path.
- Shared governance files are changed only by the authorized owner; this plan remains DRAFT until formal approval.

## 25. Implementation handoff

### MUST PRESERVE

- Producer-specific catalogs, seeded RNG, event order, output buffers and save ownership.
- Inventory as canonical physical custody; use existing bill/transaction/claim paths.
- Existing contract/consignment owner’s lifecycle, deadline, price, escrow and completion semantics.
- Separation between a historical output record and goods currently present in inventory.

### MUST ADD

- First: one audited source trace from accepted obligation through unique producer fact to inventory and contract result.
- If that trace exists: an ephemeral House readout and owner event refresh.
- If a canonical contract/inventory command safely closes the path: one thin action route with current-owner preflight and idempotency.

### MUST NOT DO

- Create House production state, receipt ledger, warehouse, reservation, retry queue, output grant, production tick, generic status machine or substitute contract owner.
- Retrofit every producer to an unused interface or call a recipe output “delivered.”

### VERIFY WITH

- Current source and archive audit; completion event/API, save section, host day owner, and exact current claim.
- One producer’s focused tests plus inventory/contract tests only where their seams changed.
- Host runtime test on the selected real path if host routing changes; focused save round-trip and deterministic replay when state/RNG changes.

### FIRST SAFE IMPLEMENTATION STEP

Run Phase 0 read-only on one candidate producer and one existing accepted contract. Prove its stable output receipt, canonical inventory handoff, save/reload behavior and exactly-once closeout. If any link is missing, return the precise P0 gap and stop before creating a House commitment API.

## Appendix A — Producer completion is not delivery

The source audit has found several different meanings behind “complete”: a
recipe completed, a historical batch was recorded, an inventory callback was
attempted, buffered output was claimed, or an event was emitted. These facts
are not interchangeable. A House commitment needs to know whether the named
quantity became available through canonical Inventory and whether the existing
contract owner accepted that quantity. “Output receipt” is therefore a source
question, not a new entity to add.

The minimum evidence chain is: (1) an accepted obligation in its current owner
with stable identity; (2) a producer batch/completion fact with stable identity
or a verified idempotent boundary; (3) accepted item and amount in canonical
Inventory, observable from the actual write result; (4) required quality,
shelf-life, rejection and byproduct semantics retained or explicitly outside
the obligation; (5) contract progress credited once from the delivery fact;
(6) host route displays owner state and invokes the actual command; and (7)
existing save sections restore consistent state. Every link needs code
evidence. A recipe result, a production history row, `CanAdd`, or a void
callback is not by itself proof of delivered goods.

Receipt fields to search for—not to pre-design—include a stable batch or
completion reference, output item and accepted quantity, partial/rejected
amount, quality metadata if promised, deterministic day/order data, destination
inventory identity, contract progress reference, duplicate guard, and save
representation. The owner may encode these through state transitions rather
than a receipt object. Conversely, do not call a history record a receipt if it
only proves recipe completion.

## Appendix B — Current producer trace findings and consequences

### B.1 Oilseed pressing

The audited host session checks press availability and inventory count, calls
the pure pressing evaluator, invokes a boolean `consumeItem` callback without
branching on its result, invokes void `addItem` callbacks for primary output
and byproduct, and increments `TotalPressed` by primary amount. No stable batch
ID or delivery receipt was visible. The host partial wires save/flush/reset;
repository search found the press command reachable in CLI but did not prove a
normal game route.

This sequence has concrete edges: if consumption returns false after the count
check, output callbacks may still run; if primary add fails, the counter may
still advance; if primary succeeds and byproduct fails, the session has no
visible return contract for partial delivery. Verify callback implementations
and all callers before drawing a runtime conclusion. Focused cases are:
sufficient preflight but failed consumption; consumption succeeds then output
capacity changes; primary accepted/byproduct rejected; both accepted; duplicate
call after a lost response; save/load after completion; and ordinary gameplay
reachability. Do not add a batch counter or change `TotalPressed` before an
atomic Inventory boundary and route are established.

### B.2 Pharmaceutical tablet output buffer

`PharmaceuticalBatchResult` records batch ID, canonical item, quantity,
quality, shelf life, rejected quantity and completion day. Claiming is a
separate operation. The inspected `ClaimOutputs` iterates buffered output,
calls `_canAdd`, calls a void `_addItem`, removes the buffered record and
counts it as claimed. `Main.PharmaceuticalTablet.cs` binds `_canAdd` to
unconditional true and discards the return from `Inventory.AddById` in its
action callback. This host seam does not prove Inventory accepted the result
before the producer clears its buffer: P0 output-handoff gap.

The buffer records quality and shelf-life, while the inspected binding accepts
item ID and amount. Verify whether another item-instance path preserves those
properties. If inventory is intentionally fungible and contracts do not depend
on quality, document that limit. Otherwise the metadata loss blocks closeout.
Tests after any owner change should prove failed/partial add retains only the
unaccepted remainder, duplicate claim does not mint twice, save restore keeps
buffer and inventory consistent, and reported claimed amount matches actual
Inventory delta.

### B.3 Powder metallurgy

The system owns Inventory, seeded RNG, process catalog and power provider.
`StartBatch` validates and consumes costs through an inventory transaction,
then creates an active batch with a deterministic-looking ID. At completion,
quality comes from seeded RNG; `TryProduce` runs before a
`PowderMetallurgyBatchRecord` is appended and completion is emitted. Output
failure sets maintenance-required while active fields remain; recovery needs
direct audit. The record carries ID, item, amount, day, quality, reliability
and wear, but the inspected latest-modifier query is aggregated by item rather
than bound to specific units.

Verify whether `TryProduce` can partially add, whether output failure is
retryable without consuming inputs twice or drawing quality again, whether the
batch identity survives restore/counter reset, and whether an event observer
can credit a commitment before save capture. Quality cannot be promised on a
contract if Inventory cannot retain it, unless the contract explicitly accepts
fungible quantity.

### B.4 Silent Foundry

On the inspected successful-heat path, the owner invokes `_addItem` only when
`_canAdd` succeeds. Regardless of that delivery branch, it appends a
production record, applies treaty quota, marks heat complete and emits a
completion event. The record had no inspected unique ID. A blocked add may
therefore coexist with completion history and quota progress. A preflight and
void add also leave a race unless callback invariants prove otherwise.

Prove whether failed capacity is intentionally recoverable and whether treaty
quota measures produced or delivered quantity. Those measures can differ; a
House delivery promise must use accepted goods. If `Completed` is terminal
while no output was accepted, stop until the current owner has a verified
recovery seam. Retrying must neither mint output nor advance quota twice.

### B.5 `IOutputSink` and commitment owner

The `IOutputSink` declaration and simulator comment are not evidence of a
production integration. The audited `DeliveryBill`/result has no visible batch,
commitment or idempotency reference and no quality; repository search did not
establish a production caller. Inspect implementations and active callers
before considering it.

The current `CommitmentSystem` is a likely Plan 38 owner. Its definition has
target item/quantity, condition, due day, counterparty and consequences.
`RecordProgress(id, qty)` advances quantity without inspecting Inventory or a
delivery receipt; `Settle` can mark a commitment Met directly. Terminal state
handling appears one-shot, and deadlines use its own clock. Host progress
passes commitment ID and quantity without a receipt. Source therefore shows a
quantity assertion API, not independent proof of physical delivery. Trace all
callers. If an existing caller gates progress on an authoritative delivery,
document it; otherwise do not call `Settle` after recipe completion. Any new
receipt-binding API is an owner-level design decision.

## Appendix C — Transaction boundaries, inventory and recovery

`InventoryBill`, `InventoryTransaction` and `TryExecuteTransaction` provide a
local inventory operation when the candidate path actually uses them. They do
not automatically create a transaction spanning producer history, contract
progress, an event listener, host save section or UI. For a chosen path, draw
the ordered source mutations and identify the actual commit point:

| Step | Mutation | Owner | Evidence needed |
|---|---|---|---|
| Validate recipe | no mutation on rejection | Producer | Stable command result. |
| Consume inputs | exact cost or no change | Inventory/producer | Atomic bill or equivalent. |
| Complete batch | stable result and RNG outcome | Producer | Retry/restore semantics. |
| Add output | accepted amount | Inventory | Observable authoritative result. |
| Credit obligation | one progress delta | Contract owner | Idempotency and target match. |
| Refresh UI | projection only | Host | Owner reread; no second mutation. |
| Capture save | coherent owner states | Save owners | Restore at supported boundary. |

This is an audit table, not prescribed call order. Record actual order and
judge replay behavior. A `CanAdd` check followed by an add is a stale preflight
unless an owner invariant prevents intervening mutation. Where Inventory
reports accepted amount, preserve any remainder only in an existing owner
buffer or retry seam. Where a callback is void, mark P0: success and partial
acceptance are unobservable. Do not invent a queue, escrow, warehouse or
receipt ledger to retain the remainder.

For every current save section on the selected producer and Inventory, test
representable boundaries: before batch start; after costs consumed but before
output; after producer completion before Inventory acceptance; after Inventory
acceptance before producer clears pending output; after output before contract
progress; after contract update before host refresh; and during codec
migration. Ask whether reload duplicates output, re-consumes inputs, loses a
remainder, credits progress twice, re-fires a gameplay event, or changes seeded
quality. If a boundary is unreachable due to synchronous order, cite that
order; do not assume it.

The safe recovery preference is reject before mutation. Once inputs are
consumed, failed output needs an existing atomic inventory rollback, a
producer-owned pending batch, or an existing explicit compensation. A UI retry
is not compensation. If output was accepted but commitment progress failed,
determine whether a stable reference allows later single credit without
re-issuing goods. Otherwise the integration is blocked pending owner decision.

## Appendix D — End-to-end scenarios for Phase 0

1. **Full destination on tablet claim.** Batch is buffered; Inventory fills
   before claim. Observe buffer and actual inventory delta. If buffer clears
   while inventory is unchanged, output loss is proven. A second claim must
   neither mint duplicate goods nor hide the loss.
2. **Accepted output, missed UI event.** Inventory accepts, but panel closes
   before refresh. Reopening must read Inventory and contract owner state; it
   must not require another claim. If a repeated claim can mint twice,
   exactly-once behavior is unproven.
3. **Metallurgy output blocked.** Inputs were consumed at start; completion
   samples seeded quality; Inventory rejects. Inspect active fields, status,
   next retry RNG, restore and eventual single batch record. A permanently
   abandoned batch cannot satisfy a delivery commitment.
4. **Foundry quota versus goods.** Preflight blocks output but history/quota
   advance. Establish whether quota means production capacity or actual
   delivery. House contract must follow its own obligation semantics and never
   infer delivery from quota.
5. **Oilseed consume callback rejects.** Initial count passes, then another
   owner removes seed. Callback returns false. Observe output callbacks and
   counter; verify no goods appear without consumed input unless that is an
   explicitly designed rule.
6. **Due day crosses while batch runs.** Start before deadline; complete after.
   Contract owner decides whether start grants grace, late delivery counts, or
   miss is terminal. House UI must not invent a different rule.
7. **Reload after accepted batch.** Capture at real save boundary; restore and
   invoke only ordinary refresh. Inventory, history, contract quantity and
   event behavior must agree. Presentation events must not be replayed as
   gameplay mutations unless explicitly replay-safe.
8. **Two obligations target same output.** A single accepted unit must not be
   credited to two contracts unless the existing owner explicitly supports
   that policy. Search caller ordering and owner APIs; do not decide via list
   iteration order.
9. **Quality threshold with fungible inventory.** Contract demands quality or
   condition. Trace exact metadata from result to inventory stack and delivery
   check. If it is not retained, fail closed rather than accept a quality
   assertion from producer history alone.

## Appendix E — Failure/retry acceptance matrix

| Failure | Required source contract | P0 stop condition |
|---|---|---|
| Input validation | Rejection occurs before mutation. | Partial cost is lost without rollback. |
| Output capacity | Producer remains recoverable or defines abandonment. | History says delivered while Inventory lacks goods. |
| Callback false/partial | Accepted amount is reported or queryable. | Void callback hides success. |
| Lost result | Existing command reference is idempotent. | Retry duplicates output or commitment progress. |
| Duplicate event | Owner deduplicates or event is presentation only. | Listener can mint/credit repeatedly. |
| Save restore | Existing sections recover coherent state. | House must rebuild state from a second ledger. |
| Quality/reject output | Consumer of metadata is traced. | Promise cannot be checked against delivered goods. |
| Expired obligation | Contract owner sets timing rule. | UI adds a grace policy. |
| No stable identity | Current owner reference exists or gap is recorded. | House creates local ID as authority. |
| Cross-owner commit | Atomicity or recovery is evidenced. | Silent one-sided commit is possible. |

## Appendix F — Owner/API checklist

**Producer:** Does start consume exact inputs atomically? Is batch identity
stable across save restore and counter reset? Is completion separate from
delivery? Are partial/rejected/byproduct outputs possible? Does failure retain
a retryable remainder? Does retry reuse the seeded result? Which event fires,
and can its listeners mutate gameplay? Which save section owns active batch,
buffer, history and counters?

**Inventory:** Is `AddById` all-or-nothing, partial or clamped? Does it return
an accepted delta or failure reason? Is the write atomic with inputs? Can it
represent quality/condition? Which restore is canonical, and can callbacks
retain a stale Inventory instance after host reconstruction? Is any duplicate
guard present?

**Contract/commitment:** Is obligation identity stable? Does `RecordProgress`
validate item and quantity? Can it bind/dedupe a delivery reference? Does
`Settle` only mutate status or also transfer goods? What happens to over/late/
partial/rejected deliveries and duplicate progress after terminal state? Which
save owner and day clock govern it?

**Host:** Which screen invokes the producer in ordinary gameplay? How are
callbacks rebound after load? Does an adapter discard an add result? Does
Inventory UI reread canonical state? Are duplicate clicks blocked only in
presentation or by an owner guard? When are save sections captured relative to
mutations?

Every unanswered item is P0 VERIFY. Type names, stale plan prose and filename
matches do not answer these questions.

## Appendix G — Evidence boundary for proposed trade documents

`docs/production/PRODUCTION_TRADE_FLOW.md` opens with claims that it is a
canonical regional trade authority, names `Assets/Ashfall.Core.Economy` and a
`ProductionTradeFlowSystem.cs`, names a `trade_flows.json` catalog, and states
“100% Pass” verification. The current source/data search performed for this
plan did not find those named runtime files or `Assets/StreamingAssets/Data/
trade_flows.json`; this is a documentation claim, not runtime evidence. The
document also contains extensive consignment language and trade-flow records.
Those proposed records do not establish that a producer output has been
accepted into Inventory or that the active commitment owner records it.

`docs/world/REGIONAL_CONTROL_MATRIX.md` contains regional text describing
consignment (including the R3 transit rule), but the mention is world/rule
documentation, not proof of a live production receipt API. Before any future
integration, audit both documents for current authority status, references,
duplicate tables and archived/speculative content. Do not import their
consignment model as a new authority or treat their status banners as a
verified build/test result. A real owner must be found in active source and
data before these texts can inform closeout behavior.

This boundary is included because these files contain superficially relevant
terms. It prevents a filename or assertive status paragraph from overriding
current code evidence. No change to either document is in this plan's scope.

## Appendix H — Focused verification and handoff

Inspect these existing targets before proposing new tests: `OilseedPressingTests.cs`,
`PharmaceuticalTabletEngineTests.cs`, `SilentFoundrySystemTests.cs`,
`FoundryPlan129IntegrationTests.cs`, `InventoryTransactionTests`,
`UnifiedInventoryOwnershipTests`, `InventorySystemTests`, and
`Plan35ProductionDeliveryTests.cs` only if the output sink path is actually
current. Search for metallurgy and CommitmentSystem coverage first. Avoid
near-duplicate tests; select a few assertions per behavior: exact Inventory
delta, retained remainder on failure, no duplicate on retry, save round-trip,
and same-seed output after restore.

Use `bin/run-scoped-tests` per repository policy only after identifying the
exact current target. Run a focused host/Godot check only if host routing
changes; run deterministic replay and save round-trip only if those owners
change. This documentation task ran no tests. A compile pass is not proof of
normal route reachability or delivery.

The next implementation handoff must include the selected producer/obligation
source citations, exact command-to-inventory-to-contract call graph, accepted
quantity observation, existing idempotency/retry contract, already-owned
stable references, file impact map, mutation and event order, RNG behavior,
save migration and rollback, focused test mapping, and precise stop
conditions. Do not propose House-level receipt, retry, warehouse, reservation,
or progress ledgers. If one link is missing, hand off the P0 gap and leave this
feature at no-code-change status.

## Appendix I — Field ownership and route contract questions

The file impact map for a later implementation must name one owner for every
mutable fact. The matrix below helps expose accidental double ownership. It is
not a recommendation to add the listed field where it is absent.

| Fact | Expected existing owner to verify | Projection may do | Projection may not do |
|---|---|---|---|
| Recipe inputs/costs | Selected producer and Inventory transaction | Show current recipe cost. | Reserve or consume ingredients. |
| Active batch/job | Selected producer save owner | Show active state from source. | Maintain parallel House batch status. |
| Completed output | Producer completion record/buffer | Display source result. | Treat completed history as accepted inventory. |
| Inventory amount | Canonical Inventory | Query current custody. | Cache a second stock count as authority. |
| Quality/condition | Item/producer representation, if supported | Show only proven retained metadata. | Infer quality from an unrelated aggregate record. |
| Delivery fact | Existing transfer/contract owner if present | Link owner evidence. | Synthesize delivery from button success. |
| Contract target/deadline | Existing commitment/consignment owner | Show owner terms. | Create another deadline/status machine. |
| Progress/settlement | Existing contract owner | Refresh owner result. | Increment progress directly from UI or event text. |
| Retry eligibility | Existing producer/transfer owner | Surface retry affordance if real. | Create a retry queue or retry by reissuing output. |
| Save/migration | Each current save owner | Read restored state. | Copy values into `trading_house` save section. |
| Notification | Existing event/UI route | Present a source fact. | Treat notification delivery as transaction success. |

If the selected flow requires a fact without a canonical owner, implementation
must stop at that gap. The correct next deliverable is an owner decision with
the smallest proposed change and its migration/replay consequences, not a
House field added preemptively.

### I.1 Read-only query contract

An eventual read surface should be pure and reconstructible from current owners.
For each displayed commitment, it should be possible to answer: which source
contract is this; which producer output is associated; what exact inventory
delta proves delivery; what quantity remains; and what owner-defined status is
current? If one answer cannot be obtained, the UI should present that stage as
unverified/unavailable rather than inventing an aggregate percentage.

The read operation must not advance producer ticks, consume RNG, alter save
dirty state, reserve goods, create receipts, or settle commitments. Querying
twice in the same state must produce the same rows and order. Ordering should
be deterministic using owner order or a documented stable key; never depend on
dictionary/hash enumeration. A read-only view should not persist a last-seen
receipt or increment exposure counters.

### I.2 Command contract

If a real owner already supports a command such as claim, transfer or fulfill,
the host should call that owner once and report the owner's exact result. The
command route must specify:

- actor/settlement identity and how it is bound after load;
- contract and producer references validated by their owners;
- item and amount revalidated at commit time, not accepted from stale panel
  state;
- result type that distinguishes rejected/no mutation, successful accepted
  amount, partial result, and unknown outcome where that can occur;
- duplicate command behavior after an event re-entry, rapid repeated click or
  save restore;
- completion signal and refresh path that cannot invoke the command again;
- transaction boundary for input cost, producer state, output inventory and
  obligation status.

If current owner API returns only `void`, a host adapter cannot manufacture a
success result from a preflight. If it returns a boolean, inspect whether true
means “attempt accepted,” “all goods added,” “batch completed,” or “contract
settled.” Name the semantics in the handoff. A wrapper that converts an
ambiguous boolean to `Delivered` does not make the API stronger.

## Appendix J — Determinism and save-boundary probes

The proposed feature is read-only by default, but any accepted command can
touch seeded production behavior. The verification plan must pin exact order
and RNG use before integration.

For each selected producer, record whether RNG is consumed at start, per tick,
at completion, or on each output retry. Record seed source, fork label and
whether restore reconstructs the same stream. Two replays with the same seed,
inputs, power/day progression and save/restore schedule should produce the same
batch ID, quality, rejection quantity, accepted inventory delta and commitment
result. The test must compare owner state and inventory, not only event text.

Do not use a UI read/query to consume randomness or schedule completion. Do not
generate stable IDs from wall-clock values, `Guid.NewGuid`, dictionary order,
current panel selection or output list position. If existing IDs use counters,
test their persistence through reset and restore. If IDs use day plus batch
count, test multiple batches on the same day and counter restoration to detect
collisions.

Save/load checks are limited to actual registered owners and codecs. Confirm
capture order and restore order for producer, Inventory and commitment state;
confirm `SaveSectionRegistry` uses each extant section exactly once; and
inspect legacy fallback behavior before changing schemas. Any newly serialized
field needs a version/default policy, malformed-value handling, migration
fixture and idempotent load-save-load case. If no new field is required, do not
alter the save schema merely to preserve UI state.

## Appendix K — Implementation sequence with explicit gates

This sequence is intentionally narrower than a generic House roadmap:

1. **Refresh audit evidence.** Re-run targeted source searches for the chosen
   producer, Inventory methods, host composition, save registration, contract
   owner and existing tests. Update any moved path references.
2. **Choose one real route.** Demonstrate the player action path in ordinary
   campaign host code. CLI/simulator reachability is not enough for a gameplay
   claim.
3. **Trace mutation order.** Document costs, RNG, producer completion, accepted
   output, contract progress and save capture. Mark all callback results that
   are discarded.
4. **Prove item custody.** Show canonical Inventory before/after values and
   item metadata. Confirm partial add behavior and transaction semantics.
5. **Prove obligation acceptance.** Show the current accepted commitment ID,
   valid target item, exact due/quality rule and current progress owner. Trace
   all callers of progress/settle.
6. **Prove retry and replay.** Simulate duplicate command, missed response,
   event repetition and reload from a real save boundary. Do not implement a
   House retry queue to bridge an unproven result.
7. **Decide owner changes.** If a receipt field or transaction result is
   missing, write the smallest owner-specific API proposal and its effect on
   existing consumers, data, save and tests. Wait for authorized owner
   selection before coding.
8. **Implement one vertical slice.** Only after the previous gates, change the
   selected producer/owner route and the minimum host projection. No generic
   production registry or every-producer retrofit.
9. **Run focused verification.** Existing tests first; add only missing
   high-signal behavior. Use scoped tests, deterministic replay and host route
   check as required by actual changed seams.
10. **Review authority boundaries.** Confirm there is no new House stock,
    receipt, warehouse, reservation, progress or save ledger; update this plan
    from DRAFT only through its proper review owner.

Gate failure returns to read-only audit or a P0 owner decision. It must not be
silently translated into “best effort” output delivery.

## Appendix L — Review checklist for stale-document claims

The production trade-flow document is relevant to economics vocabulary, but
its opening asserts current authority and 100% verification. The active source
search did not find the named class or catalog file. This mismatch should be
recorded when that document is audited: search source and data, inspect project
compile inclusion and current authority map, and determine whether it is
proposed, archived, stale or accidentally presented as live. Do not use its
claim banner as test output. Likewise, the regional matrix consignment phrase
is not an API. It can inform narrative terminology only after the current
consignment owner is located.

The boundary matters for this plan because a trade-flow “consignment” may mean
shipping goods into a market, while a House production closeout may mean
fulfilling an obligation to a specific counterparty. Those may have different
identities, quantities, clocks and transfer semantics. A shared noun does not
prove a shared contract. If the current code has a matching owner, cite its
methods and tests. If not, label the text-only mechanism proposed and keep the
House plan blocked at the existing owner gap.

## Appendix M — Candidate selection scorecard

The first implementation should follow whichever current producer can prove a
real vertical path with the fewest owner boundaries. Feature appeal or output
volume is secondary. This scorecard is based on the inspected evidence and is
not a ranking of quality or gameplay value.

| Candidate | Evidence already present | Unresolved seam | Phase 0 disposition |
|---|---|---|---|
| Oilseed pressing | Pure evaluator, host session, input/output callbacks, counter, save host partial, focused engine tests. | `consumeItem` result ignored; output callbacks void; no batch receipt; ordinary route not proven. | Audit route and callback invariants first. Do not choose until exact acceptance and route are known. |
| Tablet batches | Rich batch result with ID, item, qty, quality, shelf life, rejected qty, day; persistent output buffer and claim API. | Host claims unconditionally preflight and discards `AddById` result; metadata handoff uncertain. | Strongest explicit source record, but unsafe claim binding is P0. Inspect if owner can return actual accepted amount. |
| Powder metallurgy | Producer directly owns Inventory and seeded RNG; input `InventoryBill` transaction; output precedes record/event. | Failed output recovery, stable ID lifetime, RNG retry and quality-to-unit provenance unproven. | Candidate for local atomicity audit; not ready for commitment integration. |
| Silent Foundry | Existing production system/history, host wiring and focused integration tests. | Completion/history/quota/event can advance when output not accepted; record unique ID absent. | Do not route a House obligation until production-vs-delivery semantics and failure recovery are explicit. |
| `IOutputSink` route | Shared-looking interface and existing test filename. | No production caller found; receipt/request fields incomplete; interface’s live owner unknown. | Treat as unproven/possibly dormant until references and concrete implementations are found. |

For each candidate, the auditor should answer these questions in order, to
avoid spending implementation effort on a path that cannot close:

1. Can ordinary campaign play invoke it through a current host route?
2. Does it use the campaign's actual canonical Inventory instance, including
   after restore?
3. Is the accepted output amount observable from the mutation itself?
4. Is there a stable source fact usable after save/load and repeated events?
5. Does an existing accepted contract have compatible target and timing rules?
6. Is there a supported progress/settle command whose duplicate behavior is
   clear?
7. Are metadata and units compatible without lossy conversion?

The first “no” or unresolved answer is the work boundary. For example, a path
with reachable gameplay but no observable output result is not rescued by a
better panel; a rich batch record is not rescued by a non-routable CLI command;
and a safe Inventory transaction is not enough if no contract owner accepts
the delivery fact.

## Appendix N — Transaction observation protocol

When Phase 0 captures a trace, use fixed before/after snapshots from the
canonical owners and record callback return values. Do not infer a delta from
the producer's summary record. The trace sheet should include:

| Observation | Record |
|---|---|
| Scenario setup | Seed, day, recipe, inventory capacities/contents, power and contract state. |
| Command | Host method, arguments, actor and producer reference. |
| Preflight | Each check and whether it is read-only. |
| Mutations in order | Owner, method, input, return value, exception/result. |
| RNG | Stream/fork and draw count before/after command. |
| Events | Exact event and subscribers that mutate or render. |
| Inventory | Item quantity and metadata immediately before/after. |
| Producer | Active batch, pending buffer, history/counters before/after. |
| Contract | ID, target, due, progress and status before/after. |
| Save | Capture boundary, section names, restore order and post-restore values. |
| Retry | Second command result and whether any delta repeats. |

Run the same trace with one controlled failure: reject input, reject output,
interrupt after completion if a supported boundary allows it, or restore before
refresh. Change one variable at a time so that the result identifies which
owner controls the behavior. If current instrumentation cannot observe an
accepted amount or callback result, mark that limitation rather than adding
debug state to production code. A read-only trace may use existing test fakes;
do not create a new general-purpose simulation tool for this plan.

## Appendix O — Data and content limits

The House theme can describe producers and counterparties in text, but prose
must not state an operational outcome the current system cannot support. A
contract page may say a batch is “running” only when the producer reports an
active batch. It may say “ready for pickup” only when output is actually
available in a source-owned buffer or inventory. It may say “delivered” only
when a contract owner records accepted fulfillment. Narrative receipts,
ledger pages, consignment manifests and radio notifications are presentation
assets; they do not establish custody or debt state.

For catalog/data additions, do not add production or consignment IDs until
Phase 0 proves a current schema and consumer. Validate item IDs against the
authoritative item catalog and target items against the selected contract
catalog. A syntactically valid JSON entry is not operational if no route reads
it. Avoid production-specific catalog duplicates in the House folder.

Quantity wording must preserve actual units: item count, weight, stack amount,
quality band, or contract unit. “Batch” is a producer concept and may contain
multiple items; “shipment” may include several batches; “consignment” could
refer to commercial ownership or transport. Do not silently equate these.
Text should display owner-provided values and not suggest that quality,
shortage, late fee, penalty, reserve or escrow semantics exist if no current
owner supplies them.

## Appendix P — Worked candidate trace: powder metallurgy

This trace is a source-based candidate walkthrough, not a claim that a powder
batch can currently fulfill a House contract. It deliberately carries the
candidate through the existing owners and marks the exact point where the
current contract seam refuses the premise.

### P.1 Concrete data and composition

`Assets/StreamingAssets/Data/powder_metallurgy_catalog.json` defines two
processes. The first is `process_powder_press_structural_coupling`: costs four
`scrap_metal` and one `mechanical_parts`, yields one
`item_foundry_structural_coupling`, takes two days and requires 850 W. The
second is `process_powder_press_casing_blanks`: costs three `scrap_metal` and
one `item_foundry_replacement_die`, yields two `item_foundry_casing_blanks`,
takes one day and requires 650 W. Current item records exist for these named
outputs in `items.json` (also mirrored as foundry definitions in
`foundry_items.json`). This proves the current catalog references resolve in
the checked data; it does not prove a commitment target exists.

The host composes Powder Metallurgy over `_inventory.Inventory`, seeded from
`_campaignDay.Rng.Fork(CampaignStreamIds.Foundry, 0, 24)` when campaign day is
available, and a power provider reading the grid's `NetWatts`. It loads the
catalog, restores `powder_metallurgy_save.json`, creates a host session and
connects completion to a journal entry. The `Plans130To133Panel` exposes a
start button for the first process in catalog enumeration and reports
“Material batch reserved” after `StartBatch` succeeds. The existing host daily
orchestration calls `TickPlans130To133(day)`, which ticks the Powder system.
This is a reachable host route, unlike a CLI-only candidate, but it is a
manual start button inside the plan panel; it is not an accepted contract
delivery action.

### P.2 Happy path from start through delivery candidate

Assume for illustration that the player has enough feedstock, the station is
installed, status is not Processing, and available power is at least 850 W.
At day 10 the player selects the structural-coupling recipe. `StartBatch`
checks station, current status, process ID and power; builds an `InventoryBill`
from catalog costs; and calls `TryExecuteTransaction`. That transaction
validates the full cost against the shared Inventory and consumes four scrap
metal plus one mechanical part atomically. Only after it succeeds does the
producer assign active process, batch ID `pm_10_<completed_batches +
days_elapsed + 1>`, active day, duration=2, elapsed=0 and Processing status.
The state-change event dirties the host session. Current behavior is therefore
“inputs consumed and batch started,” not a delivery or reservation of output.

On day 11, with sufficient power, `TickDay` sets Processing, increments elapsed
to one, and returns before any output write. On day 12, with sufficient power,
it increments elapsed to two, computes quality with one seeded `NextFloat`,
interpolates wear, and sets output quantity to at least one. It calls
`Inventory.TryProduce(output_item_id, amount)`. On success it appends a batch
record with batch ID, process ID, output ID, units, completion day, quality,
reliability and wear; increments completed count and produced units; sets last
completed day; clears active fields; returns Ready; emits `OnBatchCompleted`
and `OnStateChanged`; and returns a successful `ActionResult` containing
quality and output count.

The observed Powder owner has no call to CommitmentSystem and no accepted
contract ID in its process or batch record. The panel displays batch status,
completed count and cumulative units; it does not record delivery. The journal
subscriber says that the press completed abstract material units. Therefore
the end-to-end trace currently terminates at “Inventory accepted output” plus
“producer recorded completion.” It does not continue to contract progress.

### P.3 Current settlement refusal and target mismatch

The authored commitment catalog currently contains a 50-unit grain-flour
tribute, a two-unit water-filter treaty, and a one-unit water-filter census
filing. It contains no structural coupling or casing blank target. Thus there
is no authored accepted obligation against which the worked powder example can
legitimately settle.

The generic `CommitmentDefinition` does store `target_id`, target quantity,
counterparty, due day and condition type. However, `CommitmentSystem.RecordProgress`
receives only commitment ID and positive quantity. In the inspected method it
does not compare the delivered item ID to `target_id`; it increments the
commitment progress and marks Met once count reaches target. `Settle(id, day)`
sets progress directly to target and marks Met without examining item, amount
transferred, producer receipt, due-day eligibility or inventory. It refuses an
unknown ID and any already-met/missed terminal ID. The host wrapper forwards
these operations and marks state dirty when met. The method comment describes
progress as delivery, but the API does not verify that delivery.

Consequently, a naive completion handler that calls
`Commitments.RecordProgress("commitment_warlord_grain_tribute", 50, day)` on
Powder completion would pass the wrong item and still mark a grain obligation
Met. A call to `Settle("commitment_water_filter_treaty", day)` would likewise
mark it Met without two filters. This is not a hypothetical data mismatch: it
follows from the arguments and current guards in the inspected code. The
correct current behavior for the candidate is **refuse House settlement** and
record P0 owner/API gap. Do not add a temporary rule in the Powder event
subscriber; the contract owner must own target validation and delivery proof.

### P.4 Shortfall and output failure

At day 12, if Inventory cannot accept the output because capacity or weight
limits reject it, `TryProduce` returns false. The producer has already
incremented `days_elapsed` to its required duration and already sampled RNG
quality. It sets `MaintenanceRequired`, emits state change and returns
`storage_full`. It does not append a completed batch record or increment
completed/produced counts, and does not clear active process/batch fields.

However, `TickDay` only runs the batch progression when status is Processing.
Calling it again in MaintenanceRequired returns the idle-success result before
examining the retained active fields; there is no visible retry method in the
system. `StartBatch` rejects only current status Processing, so a new start can
pass the status gate while maintenance is required, consume another bill, and
overwrite the prior active process/batch fields. Unless another route repairs
or reconstructs that state (none was found in the inspected class), this
candidate has a shortfall/loss risk. The retained active ID is not evidence
that it is retryable. Any implementation must stop until actual recovery
behavior is found or the Powder owner receives an approved correction.

### P.5 Duplicate output and re-entry

For an uninterrupted successful run, after the first completion the owner is
Ready and active process ID is cleared. A second `TickDay` does not re-run the
output path; it returns idle. A user can start another batch only through the
button and after satisfying inputs and power, which creates a new active batch
ID based on updated counters. The record and inventory output are therefore
not duplicated by simply repeating the same day tick after a completed state.

The wider host save boundary is less strong. `Inventory.Add` emits
`OnInventoryChanged`; `InventoryHostSession` converts this to `StateChanged`,
and `Main.SetupInventory` immediately calls `SaveInventory` on state change.
Powder state is captured through its separate `powder_metallurgy` section when
`PersistPlans130To133` runs. Inside `TickDay`, Inventory output is added before
the producer appends its batch record and clears active state. A process crash
or power loss between those synchronous operations could leave Inventory's
output durably saved while Powder's last persisted capture still says the
batch is Processing at its previous elapsed count. On reload, ticking the
restored batch to completion could add a second output. There is no shared
transaction spanning both save sections in the inspected path. The window may
be narrow, but exactly-once persistence is not proven.

### P.6 Capture, restore and daily progression

Powder save state version 1 includes installed/status, active process/batch IDs,
active day, duration and elapsed days, last completion day, completed/produced
counters and batch history. `CaptureState` JSON-clones this state; restore
clones it and repairs a null batch list. The current focused save test starts a
three-day job, ticks once, restores the active system state into a second
system, then asserts Processing, elapsed=1 and process ID. That test uses the
same Inventory object, so it proves active owner-state round-trip but not the
coordinated Inventory/Powder save transaction, output duplicate prevention or
host save ordering.

Inventory has its own `inventory` section/file and restored items are resolved
through the canonical ItemCatalog. The two stores are independent. Normal
composition does restore Inventory before Powder because `SetupPowderMetallurgy`
calls `SetupInventory` first, then loads Powder state. This is a useful normal
restore order. It does not close the interrupted-save mismatch above. Also
note the Powder tick is invoked within the expanded-shelter daily routine,
whereas commitments have a separately registered campaign day owner in phase
4. Current ordering and day advancement must be traced in the exact campaign
entry path before promising whether commitment expiry runs before or after
Powder completion on the same day. The plan does not assert cross-owner day
ordering from filenames or UI.

### P.7 Stale process output ID and catalog resolution

The current powder catalog loader parses JSON and returns an empty catalog on
missing/corrupt file; it does not validate process IDs or item references.
`LoadCatalog` copies non-null definitions with non-empty process IDs but does
not validate input/output IDs against ItemCatalog. Current output references
are present in `items.json`, as noted above. If a future edit misspells or
stales an output ID, `PowderMetallurgySystem` still calls `Inventory.TryProduce`.
That method creates a minimal `ItemDefinition` for a non-empty ID (stack max
99, default weight 1) and calls `Add`; it does not ask ItemCatalog whether the
ID exists. If capacity/weight allows, the item is accepted as a generic slot,
and Powder then records the same stale ID as a completed batch.

The item can be lost on reload: `Inventory.RestoreState` resolves saved slot IDs
through `InventoryHostSession.Catalog.Get`; when lookup returns null, the
restore loop does not add that slot. Thus a stale ID can appear as successful
production in-session, be serialized, then silently disappear from canonical
Inventory after restore. This source trace makes catalog reference validation
a P0 precondition for using production output as a contract delivery. Do not
assume the generic `CatalogIntegrityValidator` currently covers the powder
catalog until its input set and checks are verified.

### P.8 Worked acceptance matrix for this exact candidate

| Case | Observed/current owner result | House decision |
|---|---|---|
| Correct data, enough costs/power, storage free | Inputs consumed; two ticks; Inventory accepts; batch recorded; no contract hook. | Show producer completion/stock only. No settlement. |
| Insufficient feedstock | Atomic inventory bill fails; start blocked; no active job. | No progress and no commitment change. |
| Power below requirement at start | Start blocked before inventory transaction. | No progress or consumption. |
| Power missing on a production day | Status becomes PowerStarved; elapsed does not increment; active identifiers remain. | Do not report completed or delivered. Recovery policy after power returns requires test. |
| Inventory refuses output at completion | RNG was sampled; elapsed reached duration; MaintenanceRequired; active fields retained; record/counters unchanged. | No delivery. Existing retry not visible; block. |
| Tick again while MaintenanceRequired | Early idle result; no retry. | No settlement; flag stranded-output P0. |
| Start new batch while MaintenanceRequired | Status guard permits start; new cost can be consumed and active identity overwritten. | Stop; never expose new promise until recovery policy is resolved. |
| Re-tick after successful completion | Idle result, no second output. | Completion re-entry itself appears guarded in memory. |
| Correct catalog ID removed from item catalog | Production may add default generic item; restore catalog lookup omits it. | Reject content before runtime; do not count as deliverable. |
| Existing unrelated commitment ID passed to progress | Method accepts any positive quantity; does not inspect target item. | Current contract owner requires target-aware receipt/API before House use. |
| `Settle` called for active commitment | Sets progress to target and Met without inventory proof; refuses unknown/terminal only. | Never invoke for physical delivery; action/flag settlement semantics only. |
| Save after Inventory add but before Powder final state persists | Independent store captures may disagree; duplicate output possible after reload. | Exactly-once save boundary is unproven; P0. |

This matrix is the full candidate result: production is a real routed system,
its happy path creates canonical Inventory quantity, but output failure,
stale catalog references, independent saves and commitment target semantics
prevent a safe contract closeout today.

## Appendix Q — Other live production facts: route and custody are distinct

The candidate comparison below prevents the Powder trace from being mistaken
for a universal production model. These are source-level distinctions; no one
producer's semantics should be imposed on another.

### Q.1 Hydraulic extrusion: completed record without custody write

`HydraulicExtrusionEngine` owns machine and batch state, not an Inventory
reference. Its batch stores product-profile ID, machine ID, units, quality and
phase; the product profile has `result_item_id`. `CompleteBatch` requires QA,
looks up the profile/machine, rolls deterministic defect using the injected
campaign RNG (stable batch-ID fallback when none is injected), computes final
quality/class, marks the batch Completed, wears tooling and clears the machine
active ID. The source method does not add `result_item_id` to Inventory, does
not create an inventory-transfer record, and does not emit an inventory
accepted quantity. Its XML comment says Inventory owns produced stock after
completion, but the inspected system/host/panel call graph has no such write.

The live UI can start a profile/machine/units batch, advance phases and complete
at QA. The host supplies power/cooling availability and save/restore, but not
an Inventory output callback. This is therefore a production-quality fact,
not verified custody. A completed “rejected” quality class is also distinct
from transfer rejection: the panel note says rejected stock is scrap, while
the core completion code still marks the batch completed. The exact source
path that turns a rejected profile result into scrap or usable inventory was
not found. P0: do not count the profile's configured units as delivered.

### Q.2 Greenhouse harvest: plot reset precedes unchecked add result

`GreenhouseSystem.Harvest` checks maturity and crop definition, computes clean
or tainted yield, increments harvest count, resets the plot, emits the
harvested event and returns the yield. `GreenhouseHostSession.Harvest` then
applies pollination bonus, calls `InventoryHost?.Add(yieldItemId, totalAmount)`
and discards the returned string. `InventoryHostSession.Add` can return an
“Unknown item” or “Cannot add” result; it is not a boolean success contract.
Because the plot has already reset, a failed item resolution/capacity add can
lose a harvest while the plot and harvest count report completion. The host
then sets a success-shaped LastEvent and returns true based on the Core
harvest result, not on item acceptance. This is a direct custody discrepancy.

This path has live day ownership: `GreenhouseFoundryDayOwner` runs in campaign
phase 2, ticks the canonical greenhouse through Agriculture when available (or
the host default tick otherwise), then ticks Silent Foundry. The user-facing
panel has a harvest command. Save ownership is greenhouse section plus
Inventory section. The exact order is different from Powder and the acceptance
issue differs: crop state resets before the host attempts a non-boolean add.
No House commitment should be based on `totalHarvests` or the successful Core
harvest event alone.

Apiculture is another distinct path inside `GreenhouseHostSession`: harvest
returns honey/wax quantities, which the host translates into `food_rations`
and `crafting_parts`, respectively, using rounded unit conversions; it calls
`Add` and discards the result. This is not a transferable exact honey/wax
stack receipt. A future contract target must name the actual resulting item
and conversion rule or select a different producer.

### Q.3 Oilseed: host session supports callback semantics, game route unproven

The active oilseed host session's `Press` takes injected count, consume,
and add callbacks. It preflights count, evaluates yield, then ignores the
boolean consume result and ignores the void output callbacks before incrementing
`TotalPressed`. The reviewed host partial initializes, captures/restores and
flushes the session; the only concrete `.Press(...)` callers found were in the
host CLI selftest. A normal player route was not found in current `src/` search.
That makes Oilseed an existing owner API but not a proven ordinary-campaign
command. Even if its callbacks are correctly bound in a future host route, the
present signature cannot prove add success or partial byproduct delivery.

### Q.4 Silent Foundry: host precondition checks catalog identity only

The prior source summary stated that output is conditional on `_canAdd`; the
concrete live host binding narrows what that means. `SilentFoundryHostSession`
binds `_canAdd` as `id => _inventoryCatalog.Get(id) != null`. It checks that an
item definition exists, not `Inventory.CanAdd(id, amount)` or the current
capacity/weight. Its `_addItem` delegate looks up the definition and calls
`_inventory.Add(def, amount)`, discarding the boolean result. In `CompleteCast`,
the engine invokes add when `_canAdd` passes, then records completed output,
applies quota, completes heat and emits events. Thus a valid catalog ID with
full inventory can fail the real add while production history/quota still
advance. A missing ID skips add entirely but still allows the later completion
record in the inspected non-Scrap quality branch. Earlier descriptions of
`_canAdd` as an output-capacity preflight should be read with this exact host
binding caveat; it is only an item-definition existence guard.

### Q.5 Candidate route matrix

| Owner | Normal gameplay route | Inventory write | Write result used? | Save/custody limitation |
|---|---|---|---|---|
| Powder metallurgy | Plans 130–133 panel start + daily tick. | Core directly calls `Inventory.TryProduce`. | Yes, boolean gates producer completion. | Separate producer and inventory save sections; completion write precedes producer-final state. |
| Hydraulic extrusion | Dedicated host/panel start/advance/complete. | None found in inspected call graph. | No output receipt exists. | Saved completed batch metadata is not stock custody. |
| Greenhouse harvest | Campaign phase 2 tick and panel harvest. | Host `InventoryHost.Add`. | Returned status string discarded after plot reset. | Harvest count/plot state can diverge from inventory acceptance. |
| Apiculture | Greenhouse/APIary panel and day tick. | Host maps honey/wax to rations/parts. | Returned status strings discarded. | Conversion is rounded and not an exact raw-product receipt. |
| Oilseed | Host session plus CLI selftest caller. | Injected callbacks only. | Consume bool ignored; add is void. | Normal game invocation not proven; no receipt. |
| Silent Foundry | Campaign phase 2 foundry tick and host session. | Host callback calls `Inventory.Add`. | Add bool discarded; precheck only definition existence. | Completion/history/quota can diverge from accepted inventory. |

This comparison changes the selection question. Powder currently has the most
direct Inventory mutation seam, but it also has a failed-output recovery gap
and split-save duplicate window. Greenhouse and Foundry have reachable paths
but ignore add failure after destructive progress. Hydraulic extrusion has no
custody write. Oilseed has no verified normal route and callbacks with no
accepted amount. No candidate currently closes the full accepted-obligation
to-exact-inventory-to-contract chain without an owner-level correction or
policy decision.

## Appendix R — Exact refusal rules and corrected P0 list

The trace supports a more precise division between what the current owners can
refuse and what they do not validate:

| Owner command | Current refusal/acceptance guard observed | Missing physical-delivery guard |
|---|---|---|
| Powder `StartBatch` | Not installed, already Processing, unknown process, low power, failed inventory bill. | No contract association; catalog item reference not validated at load. |
| Powder `TickDay` | Non-Processing returns idle; missing process sets maintenance; low power sets PowerStarved; `TryProduce` false sets maintenance. | No recovery method for storage failure was observed; new start can overwrite stranded active fields. |
| Extrusion `CompleteBatch` | Unknown ID, completed batch, not at QA, missing profile/machine state. | No Inventory addition or settlement guard. |
| Greenhouse `Harvest` | Invalid plot, immature crop or missing crop definition returns failed result (missing definition resets plot). | Host ignores Inventory add result after plot reset. |
| Oilseed `Press` | Not installed, insufficient preflight seed count, below minimum output. | Consumed boolean and output success are ignored. |
| Foundry `CompleteCast` | No product resets active heat; scrap/zero-quality records failed cast. | Host precheck does not test capacity; add boolean discarded; quota/completion still follow. |
| Commitment `RecordProgress` | Unknown/empty ID, nonpositive amount, or terminal Met/Missed. | Does not validate target item, transfer, receipt, due date or inventory. |
| Commitment `Settle` | Unknown ID or terminal Met/Missed. | Sets full target progress and Met without item/custody/due checks. |

After this refresh, the plan's P0 acceptance conditions are: a) no status
claim uses completion history as delivered stock; b) no route trusts a void or
discarded add result; c) no obligation gets progress without target-aware
custody evidence; d) no one-sided save boundary can reissue output; e) stale
catalog IDs are rejected before production or remain recoverable through a
current owner; and f) all day ordering around due dates is proven from the
campaign coordinator path.

## Appendix S — Commitment catalog, time and save contract

The existing commitment owner has a narrower contract than the word
“settlement” suggests. Authored definitions come from
`Assets/StreamingAssets/Data/commitments.json`; `Main.EnsureCommitments` loads
that catalog, restores `CommitmentSaveStore`, then configures observability.
The current file has three entries: a 50-unit grain-flour tribute due day 25,
a two-unit water-filter treaty due day 45, and a one-unit water-filter census
filing due day 65. None is a Powder Metallurgy output.

`CommitmentSystem.RecordProgress` receives only commitment ID and positive
quantity. It rejects empty/unknown IDs, nonpositive quantity, and terminal
Met/Missed state. Otherwise it adds quantity without overflow protection in
this method, does not compare delivered item ID to `target_id`, and has no
current-day argument. The host's day argument is only used for a buffered day
event if progress transitions to Met. Consequently, current source can accept
late progress after due day but before the campaign tick marks the obligation
Missed. After the due-day-plus-one tick marks Missed, further progress is
refused. Actual race/order depends on the campaign call graph and command
timing.

`TickDay` skips before `start_day`, emits one warning at/after the computed
warning day through the due day, and marks Missed only when `day > due_day`.
It routes consequences and appends semantic day events once because the
terminal flag prevents repeats. Campaign registration places
`shelter_commitments` in phase 4; Powder is ticked from the separate
expanded-shelter daily routine. The reviewed materials do not prove their
relative order in every `TickSimDay` entry path, so a future implementation
must trace the actual coordinator call graph before promising same-day
deadline acceptance.

Pre-day rollback snapshots met/missed IDs, progress and warning IDs. Save
state v1 persists those four collections. Restore copies nonempty keys but
does not validate each against current registered definitions. A removed or
renamed catalog entry can leave orphaned saved progress: it may remain in the
dictionary while no read model is built for it. No migration can infer whether
an orphan key represented valid physical delivery; never remap it automatically.

| Case | Current commitment result | Needed for production closeout |
|---|---|---|
| Wrong item, known ID, positive quantity | Progress can increase; target not checked. | Target-aware receipt check in the owning contract API. |
| Correct item, partial quantity before due | Progress increments; no source/item argument. | Stable delivery reference and explicit partial/overage policy. |
| Progress after due, before missed tick | May still increment and mark Met. | Owner must define deadline check at command time. |
| Progress after Missed terminal state | Refused. | Show missed status; no House override. |
| `Settle` with no goods delivered | Sets progress to target and Met. | Do not use for physical delivery. |
| `Settle` after terminal state | Refused. | Reread owner; do not retry mutation. |
| Catalog entry removed with saved progress | Orphan key can restore without matching definition. | Preserve/diagnose; do not guess a new ID mapping. |

No implementation route should be drafted until the Plan 38 owner semantics
are either confirmed as intentionally abstract or changed by an explicit owner
decision. Physical delivery requires the owning contract to validate target,
accepted quantity and deadline.

## Appendix T — Candidate execution and crash worksheets

These worksheets turn the Powder walkthrough into bounded evidence requests;
they describe current-source expectations and unrun tests.

### Start boundary

Before start, capture Inventory and Powder state. With insufficient one-item
feedstock, `TryExecuteTransaction` should reject without removing the other
costs, assigning active IDs, or changing producer counters. The existing
focused tests cover atomic feedstock success and missing feedstock, but the
multi-cost partial-failure assertion should be verified in the exact test
before adding coverage. On a successful start, the four/one costs are consumed
and Powder becomes Processing with elapsed zero; output is absent. The
Inventory host translates `OnInventoryChanged` to `StateChanged`, and Main
subscribes to save Inventory immediately. Powder host state is separately
captured by `PersistPlans130To133`. Therefore the input debit and active job
do not share a save transaction; coordinated save is normal path, crash
consistency is not yet proven.

### Midpoint and power boundary

For the two-day structural process, a tick at D+1 with adequate power raises
elapsed to one and does not output. Capture and restore both Inventory and
Powder sections, then tick D+2. Existing `Plans130To133CoreTests` demonstrates
active Powder state round-trip with the same Inventory object; it does not
cover both host stores or output duplication. At insufficient power, the
producer sets PowerStarved without advancing elapsed. On a later powered tick,
it sets Processing and increments one elapsed count. The method guards by
status but not by comparing `day` to a last-tick field, so duplicate calls
while Processing can advance twice even if a host accidentally calls twice
for the same day. The host daily routine's exactly-once registration/order is
part of the integration contract.

### Completion boundary and save mismatch

At duration, output `TryProduce` calls Inventory `Add`, which emits the change
event before Powder appends its batch record, increments counters, clears
active fields and sets Ready. The Main subscriber saves Inventory on that
change; Powder capture occurs through a separate section. A process fault
between these synchronous steps could persist Inventory output while the last
Powder snapshot still says Processing at the prior elapsed count. On reload,
another powered tick could add the output again. This is an inferred
crash-consistency risk from verified call order and separate stores, not an
observed runtime incident. Confirm with a bounded host test/fault injection
before describing it as reproduced.

For a fully consistent save, restore should show one output plus one completed
batch, with active IDs empty and status Ready. For the mismatch capture, the
expected risk is one saved item plus a restored active batch that can mint
again. There is no shared transaction spanning Inventory and Powder in the
inspected code. Do not claim exactly-once persistence from the in-memory
repeated-tick guard alone.

### Shortfall, stale item and settlement boundaries

Fill Inventory so output add rejects only at completion. Inputs remain spent;
quality RNG has been consumed; state becomes MaintenanceRequired with active
IDs retained. A following `TickDay` returns idle before retry logic. A new
`StartBatch` is allowed because the start guard rejects only Processing; it can
consume a second cost bill and replace stranded active IDs. The current owner
has no visible recovery method. This is a concrete stranded-work P0.

For stale output ID, load a test process with an unknown nonempty ID. The
catalog loader does not validate references; `TryProduce` creates a generic
item definition for the ID and Inventory may accept it. On restore, Inventory
resolves through `ItemCatalog.Get`; unknown IDs are skipped. This verifies the
need for catalog-integrity coverage before output can count as durable stock.

For settlement, current content has no matching commitment target. Independently,
the generic API can progress a known ID without checking item or call
`Settle` without checking goods. The current correct closeout result is
therefore a refusal, not a synthetic commitment added to the catalog. Keep the
no-target finding separate from the unsafe API semantics.

## Appendix U — Focused next-test order

1. Inspect `Ashfall.Core.Tests/Plans130To133CoreTests.cs`: it already covers
   catalog counts, atomic costs/quality record, missing feedstock and active
   job state round-trip. Do not duplicate those assertions.
2. If Powder owner behavior is changed, add only missing cases: output full
   retry/recovery, failed-output new-start refusal, and repeated host-day tick.
3. Inspect `InventoryTransactionTests.cs` and
   `UnifiedInventoryOwnershipTests.cs` for capacity, acceptance and transaction
   semantics before adding Inventory coverage.
4. Host test must compose the canonical inventory and Powder save stores,
   save/reload both, and compare cost/output/batch facts. Core-only round-trip
   does not cover split host capture.
5. After a delivery API decision, add a commitment test for wrong target item,
   partial accepted quantity, deadline command timing and duplicate receipt.
6. Same-seed replay should compare batch record, output delta and commitment
   result across a save between D+1 and D+2.
7. A host route test should invoke the ordinary panel route and prove failed
   Inventory add is not reported as delivered.

Run only the scoped target after checking current equivalent coverage. No tests
were run during this documentation pass.

## Appendix V — Data-integrity boundary for target and output references

The presence of a structurally valid commitment does not establish a valid
item target. `CommitmentCatalogLoader` validates JSON shape, schema version,
ID prefix/uniqueness, date windows, warning lead, positive quantity and
nonempty consequence fields. `CatalogIntegrityValidator.ValidateCommitments`
delegates to that loader; the inspected method does not cross-check
`target_id` against the item catalog. Similarly, `PowderMetallurgyCatalogLoader`
deserializes the process file but does not validate feedstock/output IDs
against ItemCatalog, and `LoadCatalog` only checks that process definitions
and process IDs are nonempty. The current two Powder outputs were confirmed in
`items.json`; that is a checked current fact, not a future catalog guarantee.

| Join | Current checked behavior | Future verification |
|---|---|---|
| Powder process → input/output item | Catalog file has current IDs; loader accepts arbitrary deserialized references. | Confirm integrity pipeline includes Powder references; enforce at owning catalog boundary if approved. |
| Commitment → target item | Loader checks general shape, not item resolution. | Confirm target exists and has compatible quantity/condition semantics. |
| Producer result → Inventory definition | Powder `TryProduce` may create a generic fallback; host `InventoryHostSession.Add` returns a string failure for unknown IDs. | Resolve canonical definition and use observable add result. |
| Inventory save → restored definition | Restore omits slots when catalog lookup returns null. | Define stale-stock migration/rejection before saving it as accepted stock. |

A nonempty string is not a valid item; a real output item is not automatically a
valid contract target; and a valid target is not proof of custody. These joins
belong in Phase 0 and focused tests. Do not create a TH catalog that duplicates
either reference set.

## Appendix W — Status projection truth table

The existing producer panel can display useful owner facts without claiming
contract closeout. Keep output history and current stock separate.

| Powder state | Producer truth | Inventory truth | Defensible wording |
|---|---|---|---|
| Ready before start | No active batch. | Read stock from Inventory. | “Ready” and “On hand: N” are separate facts. |
| Processing | Active batch; no completed output record yet; input bill was consumed at start. | No output from this batch yet. | “In process”; not reserved or delivered. |
| PowerStarved | Active state remains; this tick did not advance. | No output from this tick. | “Paused by power”; no ETA unless owner supplies one. |
| MaintenanceRequired after storage failure | No completed batch record; active IDs remain. | `TryProduce` rejected output. | “Output blocked; recovery unresolved.” |
| Ready after successful completion | Batch record exists; active fields cleared. | Count increased in current Inventory. | “Produced”; “delivered” still needs contract receipt. |
| Completed batch restored, item missing | Producer history says completed. | Inventory disagrees or stale ID was omitted. | Show mismatch; never issue a repair grant from UI. |
| Commitment Met, goods absent | Commitment owner may have abstract progress/settlement. | Inventory does not corroborate custody. | Show contract status and custody as separate facts. |

Current Powder UI reports completed batches/output units but not current stock.
The extrusion panel reports batch quality/phases without a stock write.
Greenhouse and Foundry call Inventory but have the unchecked-result behavior
documented above. A later House card should query each canonical owner and
present independent facts; it must not combine counters into a synthetic
“delivered” state.

## Appendix X — Producer comparison by exactly-once evidence

The matrix separates five questions often collapsed into the word “output”:
where production completes, whether canonical Inventory accepts stock, what
fact survives save/load, what a retry can repeat, and whether an existing
commitment can consume the fact once. Findings below are from the audited
Core/host paths above; “not proven” means no source contract was found, not
that a particular playthrough has failed.

| Producer | Completion owner/fact | Inventory boundary | Durable source reference | Retry/duplicate behavior | Commitment join |
|---|---|---|---|---|---|
| Powder metallurgy | Core state record with batch ID, item, amount, quality/day after `TryProduce` succeeds. | Direct `Inventory.TryProduce`; bool checked. | Powder section records batch; Inventory is a separate section. | Ready-state duplicate tick is idle; storage failure leaves MaintenanceRequired and active fields; next start can overwrite them. Split save can leave item without final producer state. | No call or contract ID; current commitments target grain/water filters, not these outputs. |
| Hydraulic extrusion | Core marks batch Completed with quality class and profile data. | No Inventory write found in Engine, host or panel route. | Extrusion save records completed metadata. | Repeated completion is explicitly refused once Completed. | No custody quantity exists to join. |
| Greenhouse | Core computes yield, increments harvest count and resets plot before host add. | Host calls `InventoryHostSession.Add`, which returns status text that caller discards. | Greenhouse and Inventory save separately. | Reharvest is unavailable after plot reset; failed host add has no visible retained remainder. | No receipt/batch reference or contract call. |
| Apiculture | APIary returns float honey/wax amounts. | Host rounds/converts to food rations/crafting parts; Add status is discarded. | Greenhouse state includes apiculture; Inventory remains separate. | Harvest state likely consumes ready yield before host grant; exact recovery on failed Add not found. | Conversion units differ from raw output; no contract reference. |
| Oilseed pressing | Host session evaluates result and increments cumulative `TotalPressed`. | Consume bool ignored; primary/byproduct Add callbacks are void. | Press state saves installed/tool/total only; inventory is external. | Duplicate command may reconsume/readd; host session cannot detect partial callback success. | No normal game Press caller found; no batch ID/receipt. |
| Silent Foundry | Core records completed cast, quota and event after output branch. | Host `_canAdd` checks definition exists only; `_addItem` discards `Inventory.Add` result. | Foundry and Inventory owners have separate saves. | Completion/quota can advance despite rejected inventory add; no unique output record ID in inspected shape. | No receipt reference; quota is not evidence of accepted stock. |

Across these paths, none supplies all of: stable completion identity, accepted
Inventory delta linked to that identity, durable retry state, and contract
owner deduplication by the same identity. Some producers prove selected parts;
no current audited chain proves exactly-once physical delivery to an accepted
House commitment.

## Appendix Y — End-to-end attempted delivery and branch outcomes

This worked case uses only current authored data and current owner calls. It
shows why the right present result is read-only status, even though the domain
has both production and commitment systems.

### Y.1 Start with an existing obligation

At day 44, read `commitment_water_filter_treaty` through the commitment owner.
The authored target is two `item_water_filter` due day 45. The House has no
separate contract record. Current `CommitmentReadModel` can expose this authored
target/status from its definition and saved progress; the current Inventory
can report how many water filters are on hand. Those facts are suitable for a
read-only panel.

The Powder catalog contains `process_powder_press_structural_coupling` and
`process_powder_press_casing_blanks`; neither outputs a water filter. The
Hydraulic Extrusion profile output is also not written to Inventory in the
audited route. Greenhouse, Oilseed and Silent Foundry outputs do not match this
target in the inspected catalogs. Therefore the obligation has no audited
producer path that fulfills its item requirement. Starting a structural batch
cannot advance a water-filter contract.

### Y.2 Successful production branch still stops before closeout

For an independent production example, at day 10 start a structural coupling
batch with sufficient power and feedstock. Day 11 progresses; day 12 produces
one coupling and records the batch. Inventory count increases, and the batch
record remains available in Powder state. No contract ID or delivery fact is
emitted. A read-only projection may now show three separate truths: the batch
record says one unit completed; Inventory says current coupling stock is N;
the water-filter commitment remains at its prior progress/status. It must not
credit the wrong target or mark delivered.

Even if a future catalog authored a coupling commitment, current generic
`CommitmentSystem.RecordProgress(id, qty)` accepts only an ID and quantity; it
cannot validate item identity or prove the batch caused the inventory delta.
`Settle(id, day)` can mark the full target Met without a physical transfer.
Neither is safe for automated closeout as currently shaped.

### Y.3 Rejection and recovery branches

| Branch at the same candidate flow | Current owner outcome | Closeout outcome |
|---|---|---|
| Insufficient feedstock before start | Atomic bill rejects; Powder does not start. | No output, no progress. Read active obligation only. |
| Power absent at start | Start rejects before input transaction. | No output, no progress. |
| Power absent on D+2 | Powder becomes PowerStarved; elapsed does not advance that tick. | No output, no progress; current deadline policy remains with commitment owner. |
| Output capacity rejects at completion | `TryProduce` false; Powder enters MaintenanceRequired after consuming its completion RNG roll. | No accepted goods; no progress. Existing recovery route not found. |
| Retry tick after storage rejection | Returns idle because state is no longer Processing. | No retry receipt; no progress. Starting again can overwrite retained batch fields, so stop. |
| Inventory accepts output then host crashes before Powder capture | Inventory may have saved new stock while Powder save remains at pre-completion state. | On restore a second output may be minted; exactly-once durability is not proven. |
| Output item ID is stale | Powder loader does not validate it; Inventory may accept generic fallback ID. Restore drops unknown catalog item. | Do not count as durable stock; catalog/reference gate blocks this path. |
| User clicks fulfill on current water-filter commitment | No producer/fulfillment action is shown by the audited candidate flow. | The current contract owner may be read; no mutation route is supported here. |
| Future callback calls `RecordProgress` with the wrong item | API accepts known ID plus positive quantity without `target_id` check. | Potential false Met; prohibit this route pending owner API design. |
| Callback calls `Settle` after production | Method marks target quantity Met without checking item, output, day or receipt. | This is not physical closeout; use is refused by this plan. |
| Commitment is already Missed | Progress and Settle both reject terminal ID. | Display source Missed state; no House override. |

### Y.4 Supported read-only fallback

Until owner APIs change under an approved package, a House surface may query
each existing owner without persisting a second state:

1. Read `CommitmentHostSession.GetCommitment(id, currentDay)` or
   `GetCommitments(currentDay)` for the current authored obligation read model.
2. Read canonical Inventory count for the target item through the existing
   inventory owner. Treat it as current stock, not as the history of a
   particular producer batch.
3. Read producer state/history independently: Powder batch records and status;
   Extrusion completed phases/quality; Greenhouse crop/harvest facts; Foundry
   completed record/quota; Oilseed cumulative counter only where the current
   session is reachable.
4. Show source-specific facts and an unavailable/missing-link marker where a
   stable cross-reference does not exist. Do not create a receipt, reservation,
   delivery state, or progress mutation while rendering.
5. Refresh by rereading owners after ordinary commands or panel reopen. Do not
   replay producer output or call commitment progress as a UI refresh step.

This fallback lets the player inspect commitment status, target inventory and
production history without claiming they are causally linked. It is fully
consistent with the current one-owner-per-concern rule and is the only
supported House behavior demonstrated by the audited paths. A future exact
delivery route must add its missing evidence to the owner that owns that fact,
then test failure, retry and coordinated save behavior end to end.
