# TH-2 — Consignments and Exchange Clearing

STATUS: DRAFT — evidence-backed proposal for review; depends conceptually on TH-1; no ownership claim, path claim, implementation approval, or authorization to introduce escrow.

**Plan family:** The Trading House (TH-2 of 4). **Primary rule:** a consignment is a proposal/reference layer only until one canonical owner accepts and performs the transaction. The House does not become a warehouse, contract board, route operator, wallet, or second settlement ledger. **Drafted:** 2026-09-29. **Execution status:** premise audit required; every integration seam below is provisional and must be verified in current source before implementation.

## 1. Objective

Specify how a member could ask the Trading House to coordinate a proposed exchange and receive a truthful answer about whether an existing market, contract, route, production, or black-market owner can handle it. If a source owner offers an atomic command, the House may forward that command after the owner revalidates its own gates. The canonical owner owns acceptance, execution, refusal, and receipt.

The intended outcome is an exchange that can be followed from proposal to canonical source record, with no duplicate cargo, currency, escrow, deadline, or completion flag. A proposal may be useful even when no owner can currently fulfill it: “not routable,” “not supported,” and “owner unavailable” are informative outcomes, not errors to disguise.

This plan does not claim the current APIs support House consignments. The audited source proves substantial market, route, debt, and black-market mechanisms; it does not yet prove a generic atomic exchange contract that can accept an arbitrary House proposal. If that seam is absent, this plan remains a read-only feasibility/projection design and implementation stops pending an integrator decision.

## 2. Current Reality

The current architecture contains distinct economic paths rather than one universal exchange transaction:

- `MarketSystem` computes prices and records successful market activity in its `MarketState.ledger`. The inspected `LedgerEntry` has day, item, quantity, unit price, total value, and counterparty. The DTO does not show a unique transaction ID, offer ID, escrow ID, or generic consignment lifecycle.
- `PlayerTradeRouteSystem` stores scheduled route contracts and their cargo legs, reliability, cadence, tariff count, and outcome totals. `TradeRouteContract` is a recurring route commitment, not a general one-off consignment or atomic cargo handoff.
- `TradeRouteHostSession` loads authored caravan route definitions, exposes route contracts/census, provides route commands, risk projection, day tick, and save integration. It remains the route owner seam.
- `CaravanTradeNetworkSystem` and `CaravanAtomicTrader` cover caravan-network and caravan transaction concepts. Their verbs and state must be audited independently; neither should be assumed equivalent to player routes or the market.
- `Contract Board 109` explicitly covers posted work, deadlines, escrow, and failure consequences. It is the strongest overlap candidate for a promise to deliver goods by a deadline. A House plan must not add a competing board or escrow store.
- Long Line: Freight covers the player's freight company/runs. It may own company dispatch and carriage details; it is not a generic House clearing service.
- `TradeCreditCoordinator` offers credit after a failed trade context and revalidates eligibility at acceptance. It does not authorize House-level account balances or escrow.
- `BlackMarketSettlementService` demonstrates a tightly coordinated transaction path: it previews canonical quotes, checks wallet/inventory constraints, executes a wallet+inventory transaction through callbacks, and reports a result. This is a service-specific seam, not permission to add a generic transaction coordinator.

The new House documents are drafts only. There is no verified generic `ConsignmentSystem`, House escrow, stable transaction receipt crosswalk, or atomic cross-owner clearing method identified in this audit.

## 3. Required Delta

**Existing behavior:** several owners can accept and execute their own transactions or recurring commitments. Host APIs expose some of those commands. Their data models and save lifecycles differ.

**Requested capability:** a House member proposes an exchange and can track the result through a source owner without creating parallel obligations or goods state.

**Smallest delta:** support a source-qualified pointer from an existing House identity/coordination context to one canonical owner record, but only after that owner has created or accepted the record. Any pre-acceptance proposal remains ephemeral unless a canonical owner already persists it. Completion is never copied into House state.

The plan distinguishes three states deliberately:

1. **Proposal:** user intent, not binding and not a source owner record. Prefer UI-local/transient state; do not persist it by default.
2. **Canonical acceptance:** a supported owner has accepted its own contract or transaction and supplies a stable identifier/receipt. This is the earliest point a durable House reference might be useful.
3. **Canonical outcome:** the source owner reports completion, refusal, expiry, cancellation, failure, or settlement. The House may display that state but may not author it.

If the requested UX requires the House to persist proposals or enforce their expiry/escrow, that requirement collides with existing board/freight owners and must be re-scoped, not implemented as a fallback.

## 4. Evidence

Evidence checked on 2026-09-29:

| Source | Verified evidence | Consequence |
|---|---|---|
| `Assets/Ashfall.Core/Economy/MarketSystem.cs` | `MarketState` v3 owns a list of `LedgerEntry`; `Buy` / `Sell` append market activity and trade pressure. The inspected `Transact` method does not transfer items or funds. `Barter` records equal-value ledger legs/remainder but likewise writes market bookkeeping; it does not call inventory or wallet. A row lacks explicit immutable ID. | This is not a complete settlement command. A successful market `TransactionResult` alone is not proof of goods/currency custody transfer. House must not offer it as an atomic consignment or link rows durably without a distinct receipt owner. |
| `Assets/Ashfall.Core/Economy/PlayerTradeRouteSystem.cs` | Public contract registration, lookup, cancel/suspend/resume, run outcome, census, capture/restore. Contract registration replaces by case-insensitive route ID. | Route records are not a generic consignment. Reuse behavior means stable uniqueness/lifecycle must be checked before storing pointers. |
| `Assets/Ashfall.Core/Economy/TradeRouteContract.cs` | `TradeRouteContractSaveState` includes ID, counterparty, cadence, tariff, goods in/out, schedule, reliability, counters and status fields. `RecordRunOutcome` updates state and next run day. | Route owner alone controls recurring schedule/outcome. House must not record run status or advance it. |
| `Assets/Ashfall.Core/Economy/CaravanAtomicTrader.cs` | `Commit` rejects missing/previously committed quote IDs and invalid units/price, then appends a `CaravanCommittedTrade` to `CaravanTradeState`; capture clones that list. This type does not consume/grant inventory or debit/credit a wallet. | It proves a persisted once-per-quote commitment record, not complete resource settlement. Do not call it a cleared exchange. Audit its quote producer and host side-effect path separately. |
| `Assets/Ashfall.Core/Economy/CaravanTradeNetworkSystem.cs` | `ExecuteBarter` checks an arrived manifest, offered/requested goods and player inventory, computes values, then consumes offered inventory, adds manifest stock, removes requested stock and grants inventory; it updates profitable-trade/favored faction state and emits completion. The method has no compensating rollback block if a later inventory mutation throws after an earlier mutation. | It is a source-specific manifest barter path, not a generic consignment. Its exception atomicity needs proof or an owner-level repair before House forwarding. |
| `src/Host/TradeRouteHostSession.cs` | Host exposes canonical route reads and explicit establishment/cancel/suspend/resume and daily `TickDay`; risk evaluation is read-only. | A House command cannot call route creation as a side effect of “consign.” Only explicit existing route flows may be linked. |
| `src/Host/TradeRouteHostSession.cs`, `src/Main.CampaignOwners.cs`, `src/Main.TradeRoutes.cs` | `trade_routes` save store and phase-5 day owner are already wired. | No duplicate route save or House route tick. |
| `Assets/Ashfall.Core/Economy/TradeCreditCoordinator.cs` | Offer construction and acceptance methods use owner gates, two-read ledger ceremony, grant/revoke compensation, and ledger signing. | Credit is not a generic clearing fallback when cash/inventory is insufficient. Do not request debt automatically. |
| `Assets/Ashfall.Core/Economy/BlackMarketSettlementService.cs` | Buy/sell previews validate quote, item definition, inventory bill and wallet capacity. Commit delegates stock staging to `BlackMarketSystem.Buy` / `Sell` and supplies a callback coupling wallet settlement with inventory transaction; source stock rolls back if callback refuses/throws. A durable uniquely keyed settlement receipt still needs verification. | This is the strongest inspected source-specific settlement seam, limited to its own black-market catalog and wallet/inventory rules. Forward only as that service’s action; never wrap or copy settlement. |
| `Assets/Ashfall.Core/HoldfastTradeSession.cs` and `src/Host/HoldfastTradeSaveStore.cs` | `Buy`/`Sell` and `BuyWithFunds`/`SellWithFunds` validate actor/item/stock/inventory/funds and mutate the current Holdfast trading state; funds variants report a `HoldfastTradeResult` and write funds movements. Save state persists current value/held/stock, not a per-operation receipt ID/history. | Potential canonical terminal action if it is the intended route for the desired transaction; it is not a generic House receipt ledger. | Forward the existing command only through its owning host/session. Do not mirror its stock/value or claim reloadable transaction history without a source-level receipt plan. |
| `Assets/Ashfall.Core/Economy/BlackMarketSystem.cs` | Black-market state and preview/trade behaviors are canonical and separately persisted. | House must not maintain a shadow offer/stock/heat state. |
| `docs/plans/EXPANSION_PROGRAM_WAVE9_2026-09-21/PLAN-CONTRACT-BOARD-109.md` | Existing plan explicitly names posted work, deadlines, escrow and failure consequences. | Contract Board owns that problem space; re-audit current implementation/status before any future execution. |
| `.ai/plans/long-line-freight-2026-09-29.md` and `docs/expansions/expansion_long_line_freight_plan.md` | Freight company and road-run plan exists; it names a conceptual company/run path. | No House dispatch, cargo, freight rates, or duplicate run ledger. |
| `docs/chatgpt-claude-assisted/trading-house-1-charter-ledger.md` | TH-1 proposes House identity and derived source reads; no state owner selected yet. | TH-2 may depend on a reference contract but cannot assume one was implemented. |

Collision review also searched the plan family, trade route and contract plans, underworld/black-market audit, and other House drafts. It found adjacent substantial work but no existing document titled “Trading House Consignments” in the requested folder. This is not proof that a generic proposal seam is absent; exact source and test search is required at execution time.

## 5. Existing Extension Seams

Candidate seams, with intended safe use:

1. **Market:** read quotes/explanations from `MarketSystem`; its `Buy` / `Sell` / `Barter` methods update market-side ledger and pressure but do not settle inventory/wallet. Do not treat their `TransactionResult` as a complete handoff. Do not write `MarketState.ledger` or construct receipts in the House.
2. **Player route:** view `TradeRouteHostSession` and forward only existing route commands with their existing parameters, after confirming that the action semantically creates a route rather than a consignment. A route may be linked as an existing obligation; it is not created by clearing.
3. **Caravan network / atomic trader:** inspect public verbs, transaction callbacks, item custody, settlement resource, save contract, idempotency, and failure recovery. Treat those APIs as source-specific until proven to satisfy a more general interface.
4. **Contract Board 109:** verify whether its live owner already supports one-off goods commitments. If it does, use its contract ID/status/read model. If no stable command exists, stop rather than cloning its escrow design.
5. **Freight:** inspect Long Line: Freight's current implementation and plan; it may provide dispatch/receipt references. House remains a user/coordination surface and cannot own freight operations.
6. **Credit:** credit may only be offered by the current credit request path in its existing triggering context. House cannot intercept every insufficient-funds result or auto-accept.
7. **Black market:** only use canonical `Preview*` and transaction service methods for supported entries. Do not create House-side quote snapshots for later execution; source preview must be repeated at acceptance.
8. **Identity/reference:** rely on TH-1's eventual verified identity owner, if any. Until then, House proposals cannot be treated as signed member commitments.

At implementation time the integrator must inspect the exact current method contracts, ID stability, thread/lifecycle assumptions, and tests. No method name in this list guarantees an atomic multi-owner operation.

## 6. Proposed Architecture

### 6.1 Three-lane proposal design

**Lane A — Read and discover.** Render options from canonical owner APIs. A quote or preview is read-only and short-lived. Label its source owner, validity window if any, and whether later acceptance will revalidate it.

**Lane B — Submit to one owner.** On an explicit player action, call exactly one canonical owner command. The command must validate stock/custody, participant authorization, current price, wallet/funds, inventory capacity, route/contract eligibility, and current campaign day according to that owner's contract. It returns the owner's structured success/refusal result.

**Lane C — Track.** After a canonical record is returned, optionally retain a stable source-qualified pointer in the existing House identity/record owner. Re-read status from the owner. If no stable pointer or suitable owner exists, show the result in the immediate flow and do not promise persistence.

There is no “clearing engine” in this draft. A generic cross-owner two-phase commit would couple market, inventory, wallet, route, freight, credit, and contract systems and create a new high-risk authority. The safe pattern is one owner per supported operation; if the operation inherently requires multiple owners, only an existing source-specific transaction service may coordinate them.

### 6.2 Consignment semantics

For this plan, “consignment” is a presentation label for a member-submitted request. It is not a claim of custody transfer. Until a canonical owner accepts goods or records an offer, the House has no goods, stock, title, or escrow.

Valid terminal text must use source facts, for example “accepted by Market,” “route contract active,” “refused: inventory capacity,” or “source no longer available.” Do not reduce source-specific outcomes to a new House `Completed` enum unless that mapping has explicit evidence and preserves distinctions.

### 6.3 Supported source registry

No runtime registry is proposed. A compile-time explicit dispatch over a small set of supported owner commands is safer if more than one source is approved. If only one route exists, call it directly. Do not create plugin discovery, reflective owner registration, generic settlement interfaces, or dynamic authority routing for speculative future sources.

Every supported action must have a narrow contract card documenting: source owner, exact command, exact receipt/status API, atomicity guarantee, save owner, replay/idempotency rule, and refusal mapping. If any field is unknown, the action stays unavailable.

## 7. Ownership Matrix

| Concern | Canonical owner | House responsibility | Must not own |
|---|---|---|---|
| Proposal text and immediate UX | UI/input flow | Collect explicit player intent; show preview | Persistent obligation by default |
| Member identity/role | TH-1-selected existing identity owner (unresolved) | Read the authorized actor reference | A parallel member registry |
| Market quote and market-side accounting | `MarketSystem`; inventory/payment settlement is a separate caller/owner to verify | Display quote/accounting facts; forward only a proven settlement command | House quote cache, stock, payment, receipts |
| Inventory custody/capacity | Inventory owner and source transaction service | Present owner refusal/success | Escrow, reserved goods, shadow warehouse |
| Currency/wallet/funds | Existing wallet or funds owner | Display source result | House balance, debt, settlement math |
| Scheduled routes | `PlayerTradeRouteSystem` / host | Link to an existing route record if relevant | Route schedule, route outcome, cargo run |
| Posted work / deadlines / escrow | Contract Board 109 owner (current status recheck required) | Forward or link only if that owner supports it | Competing board or default consequence |
| Freight dispatch and movement | Long Line: Freight owner (current status recheck required) | Link to canonical run | Dispatch, cargo, rates, route risk |
| Debt / credit | `LedgerDebtSystem` + `TradeCreditCoordinator` | Display or navigate to canonical owner | Automatic credit fallback or debt record |
| Black-market execution | `BlackMarketSettlementService` | Forward canonical action only | Quote freeze, escrow, wallet/inventory mutation |
| Source status/receipt | Same source owner | Project as a pointer/read | Duplicate outcome ledger |
| Day/tick/expiry | Existing source day owner | Read only | House deadline or daily expiration tick |

## 8. Data Flow

### Preview

`member intent → validate member through canonical identity owner → resolve supported source → request live source preview → render owner-returned price/capacity/eligibility and refusal reasons`

Previews are advisory. They must not hold goods, debit currency, create debt, reserve a route, accept a contract, increment market demand, or create a lasting House record.

### Submit

`player confirms explicit terms → rerun source preview/validation → source owner checks its state → source owner performs its transaction (or refuses) → source result/receipt returned → House displays source-owned outcome → optional stable pointer recorded by an approved existing identity/record owner`

If operation cannot be performed by one source owner or an existing transaction service, return “unsupported” and stop. Do not sequence separate `debit → grant → ledger append` calls inside the House and attempt compensating rollback unless that canonical service already owns and guarantees the transaction.

### Track

`House pointer (if any) → owner-qualified lookup → canonical current status → view projection`

Pointer absence, deletion, or ambiguity is handled as an unresolved reference. The House does not replay a command to make a missing item appear completed.

### Failure

Rejection returns owner taxonomy and leaves House/source state unchanged. If the source reports a partial commit or unknown outcome, the operation is treated as indeterminate and requires source-specific recovery; the House must not retry blindly.

## 9. State Model

No new durable consignment state is approved by default.

Transient proposal fields, if a UI requires them, may include requested source kind, item ID, quantity, recipient, and user-entered note. These remain in the active view and are discarded on cancel, panel disposal, campaign restart, or owner/session replacement. They are not a new economy DTO and cannot reserve resources.

After owner acceptance, the House may keep only a pointer if all of these conditions hold:

1. An existing owner accepts an additive reference field without changing its domain semantics, or a separately approved identity owner can hold the relation.
2. The source has a stable, immutable ID that survives capture/restore and is not reused after deletion.
3. The source provides lookup and canonical current status.
4. The pointer does not include copied item quantities, prices, money, due dates, cargo state, completion flags, or settlement result.
5. Removal and migration behavior are specified and tested.

If one condition fails, use immediate transient feedback and omit durable tracking. Do not create a `ConsignmentState` as an expedient.

## 10. API / Contracts

The following are acceptance criteria for the existing source contract, not proposed method names:

### Preview contract

- Side-effect free across all owners.
- Returns canonical source ID/type, item identity, quantity accepted/available, price/fee where applicable, current eligibility, and refusal reason.
- Marks which fields may change before execution and states that the source will revalidate.
- Does not reserve funds or inventory.

### Commit contract

- Explicit player confirmation; no implicit acceptance from preview.
- Idempotency/duplicate submission behavior is known. If the canonical owner has no idempotency key or naturally idempotent accepted record, the UI must prevent repeat submit and implementation must prove that retry after crash cannot double-settle. Otherwise stop.
- Canonical owner rechecks every mutable precondition at commit.
- Exactly one source owner or its existing service decides/coordinates mutation.
- Returns one canonical receipt or source key and an explicit outcome.
- A refusal causes no mutation. A partial/unknown result has a recovery path owned by that service.

### Tracking contract

- Pointer contains owner type and stable source ID.
- Lookup is read-only; missing/ambiguous status stays visible.
- No House command can directly transition a linked record.

Do not impose a new generic interface until at least two current consumers can implement it without altering their semantics. Avoid stringly typed commands and arbitrary callback chains.

## 11. Data Changes

No data change is planned. Existing authored catalogs remain authoritative for goods, prices, routes, debt templates, black-market entries, and contract offers.

If an implementation later adds a list of supported source kinds or authored restrictions, first test whether existing source catalogs already express them. A new catalog must specify owning loader, schema version, `snake_case` IDs, references, invalid-row behavior, catalog integrity hooks, and consumer proof. It may not copy prices, quantities, route state, credit terms, escrow values, or source eligibility.

User-supplied proposal text is untrusted display input and is not catalog authority. No narrative line or UI copy may promise execution before the owner returns a receipt.

## 12. Save / Load

**Default:** proposal state is ephemeral; accepted contract/transaction state remains in its canonical owner save. House tracking is reconstructed from a stable owner reference if an existing approved owner stores it; otherwise no persistent tracking is promised.

Never add a parallel `consignments` save section simply to make the House page look durable. Duplicating source status creates disagreement after source restore, contract cancellation, inventory mutation, or transaction compensation.

If a future decision approves a pointer field, the implementation plan must explicitly cover:

- owning section and DTO; no cross-owner hidden write;
- schema version compatibility and old save defaults;
- checksum/store registration through existing generator/registry;
- restore order after identity and source owners;
- missing source and ID reuse behavior;
- capture/restore round trip and deep-copy isolation;
- save taken during an in-flight canonical transaction;
- rollback from source persistence failure.

If the source commits but saving a pointer fails, the transaction remains committed at its owner and the UI reports “completed; House link unavailable.” Retrying the transaction is forbidden. This failure case is a key reason pointer persistence must remain optional.

## 13. Determinism

Proposal and projection logic contains no randomness. It uses source-generated IDs only. No local `Guid.NewGuid`, wall-clock deadline, hash-based id, or `System.Random` is permitted in Core.

If an existing source requires a request/idempotency token, reuse its deterministic/current contract or have the canonical host adapter supply an opaque request correlation value with no gameplay significance and a documented retry lifecycle. Do not derive business identity from process/time/hash order.

Read-model ordering is stable by owner type, source day, and source ID; if the source does not provide a stable tie-break key, the plan cannot guarantee canonical ordering of those rows. Comparisons and persisted values are culture invariant. House operations do not join day advance or consume RNG; enabled/disabled paired runs must retain identical owner checksums.

## 14. Event / System Wiring

TH-2 adds no daily owner, expiry clock, tariff tick, retry worker, queue processor, or settlement event. The House reacts to an explicit input, calls one source owner, and reads its result.

Event ordering and atomicity must be confirmed for each source:

- Market ledger row creation relative to inventory/wallet mutation.
- Caravan offer state relative to inventory/funds settlement.
- Contract acceptance relative to escrow and deadline creation.
- Freight dispatch relative to cargo custody and route activation.
- Credit signing relative to principal grant and compensation (already coordinated by `TradeCreditCoordinator`).
- Black-market stock/offer mutation relative to wallet/inventory commit (coordinated by `BlackMarketSettlementService`).

The House must not subscribe to low-level “attempted” events and infer acceptance. It tracks the canonical command result or queries canonical state. Event listeners are disposed with their host/panel owner. Unknown outcome after an exception must not trigger automatic retry.

## 15. Godot Integration

No panel or scene file is pre-approved. If a surface is later authorized, use the existing trade/economy navigation and lifecycle. Proposed view states:

- **Choose source:** display only owners proven able to support an explicit exchange type.
- **Preview:** show source identity, item, quantity, full current terms, and that acceptance will revalidate.
- **Confirm:** one explicit confirmation action, with a clear cancel/back path.
- **Outcome:** show canonical receipt/status or owner-returned refusal; preserve exact distinction between unsupported, stale preview, insufficiency, and commit failure.
- **Tracking:** show linked source record and read-only canonical status; missing pointer is visible and recoverable by returning to source owner, not by retrying the transaction.

Keyboard/controller focus must land on a safe control, return to the opener on close, and show refusal/commit feedback without relying only on color. Disable confirm while submission is in flight. A view callback cannot write market state, inventory, wallet, debt, route or contract DTOs directly.

Do not put operational details such as novel risk disclaimers into product UI. Show the actual decision terms the owner requires. Separate optional House narration from binding source terms.

## 16. Narrative / Content Integration

This system can support institutional drama about trust, custody, and the difference between a promise and a receipt. Narrative remains downstream of canonical events: a scene can react to “contract accepted” only if the owner emits or exposes that fact.

Potential diegetic artifacts, if later authored in a separate content pass, include a blank claim ticket, a returned request with a missing signature, or a receipt whose date is later than the promise. They should illustrate state distinctions without adding new currency, contract rules, flags, or completion semantics.

No new quest, faction reaction, event, radio line, black-market encounter, or campaign flag is required by this plan. Do not write a successful exchange scene for a path that is still unsupported by code.

## 17. Failure Modes

| Condition | Required behavior |
|---|---|
| No canonical owner supports this exchange type | Mark unsupported; no local fallback transaction. |
| TH-1 identity/member owner absent | Disable House submission; canonical direct owner flows still work as before. |
| Source unavailable | Show source unavailable; do not interpret this as empty market or success. |
| Preview becomes stale | Revalidate at commit; return source refusal, no commit. |
| Unknown item or alias | Let canonical source validation reject; do not canonicalize independently in UI. |
| Zero/negative/overflow quantity | Reject before display/command and retain source-side validation; use bounded types. |
| Insufficient inventory or capacity | Return owner result; no grant/debit or local reservation. |
| Insufficient funds | Use existing owner flow. Do not auto-open or auto-accept `TradeCreditCoordinator` offers. |
| Duplicate click or retry after timeout | Disable duplicate submit; query canonical request/receipt if supported; otherwise mark indeterminate and require source-owned recovery. |
| Transaction crosses two owners with no atomic service | Stop source support; do not compose raw mutations. |
| Contract/route ID missing or reused | Do not persist link; report untrackable/ambiguous. |
| Source canceled/deleted after linking | Display source status or missing pointer; do not revive it. |
| Save after source commit, before optional House link | Canonical transaction remains committed; show link unavailable; never resubmit. |
| Owner exception during commit | No automatic retry; use owner’s recovery semantics and explicit outcome. |
| Source outcome has finer statuses than House display | Preserve detailed status or label the mapped view as summary; never collapse failure into completion. |
| Campaign day changes during preview | Source revalidates with current canonical day. |
| Optional subsystem not loaded | Omit its action and explain availability; supported owners remain usable. |
| Host disposed with request in flight | Prevent stale callbacks from mutating a replacement campaign or panel. |
| Restore order presents empty owner | Do not infer failure/settlement before readiness is signaled. |

## 18. Test Strategy

Do not add tests until Phase 0 verifies source commands and the implementation boundary. Search for existing equivalent coverage before creating any target. Use only `bin/run-scoped-tests`; the full suite is prohibited unless the user types exactly `RUN FULL TESTS`.

Focused behavioral coverage should prove:

1. Preview calls are side-effect free across all bound owners.
2. A source rejection leaves canonical state/checksum unchanged.
3. Acceptance delegates to exactly one source owner or its existing atomic service.
4. Repeated submission cannot double-debit, grant, create a contract, or settle stock; the owner’s existing idempotency/recovery contract is exercised.
5. Canonical source receipts/status drive House output; no copied completion flag is authoritative.
6. Missing, expired, unsupported, ambiguous, and stale records fail closed and do not mutate source state.
7. House enabled/disabled same-seed behavior is identical for relevant owners.
8. If pointer persistence is approved, round-trip plus source-missing/ID-reuse behavior is verified without duplicating source state.
9. Host/UI: confirm is disabled in-flight, cancel/back works, focus lifecycle is correct, and stale callbacks cannot affect a new campaign.

Run the smallest relevant target through `bin/run-scoped-tests`. If no existing runner supports a proposed focused target, ask integrator/foreman to resolve the test invocation; do not bypass project policy by calling `dotnet test` directly.

## 19. Dependency-Ordered Phases

### Phase 0 — Re-audit current API, overlap, and claims

Read the active authority docs and ownership ledger. Verify current implementation/status of Contract Board 109 and Long Line: Freight, then inspect their exact APIs/tests and source owners. Inspect MarketSystem transaction callers, CaravanTradeNetworkSystem and CaravanAtomicTrader transaction boundaries, BlackMarketSettlementService, PlayerTradeRouteSystem, wallet/inventory, credit/ledger, day owners, save stores, and candidate UI routes. Search for existing consignment, escrow, receipt, idempotency, and retry behaviors. **Gate:** one supported owner path has stable source identity, canonical command, clear atomicity, and recovery behavior; otherwise stop with precise missing seam.

### Phase 1 — Source contract cards

For each candidate exchange type, record the preview method/result, commit method/result, owner, state mutation, receipt ID, save section, day owner, failure taxonomy, idempotency/retry rule, and tests. Reject paths that require the House to coordinate independent writes. **Gate:** no ambiguous ownership, no House mutation.

### Phase 2 — Minimal explicit forwarding path

Implement at most one source path first, chosen by the existing owner’s proven atomic command. Wire an explicit player command and preserve existing owner validation. Do not add generic interfaces or persistent House proposal data. **Gate:** refusal and success contracts verified; non-House direct path unchanged.

### Phase 3 — Optional canonical reference

Only if TH-1 selects an existing owner and the source ID is durable/non-reused, add an owner-qualified pointer through that owner. Do not store the source state. **Gate:** migration, restore order, deletion, and commit-before-pointer failure all covered.

### Phase 4 — User surface

Expose preview/confirm/outcome and read-only track view through current UI lifecycle. Use owner-provided terms/refusals. **Gate:** duplicate submit prevention, close/focus behavior, unavailable source path, and commit feedback.

### Phase 5 — Focused verification and handoff

Run changed-area tests via `bin/run-scoped-tests`; add a bounded Godot headless check only if runtime UI/wiring changed. Report exact commands/results and stop after gates pass. Do not broaden to another source owner in the same package without a separate review.

## 20. File Impact Map

These are candidate paths/areas, not claims. Exact files and owners must be established in Phase 0 and claimed before edits.

| File/area | Action | Reason | Risk |
|---|---|---|---|
| `Assets/Ashfall.Core/Economy/MarketSystem.cs` | READ ONLY by default | Verify quote/ledger and transaction semantics | High if House writes its state |
| `Assets/Ashfall.Core/Economy/PlayerTradeRouteSystem.cs` / `TradeRouteContract.cs` | READ ONLY | Determine whether any supported route reference is stable | High; route semantics are recurring contracts |
| `Assets/Ashfall.Core/Economy/CaravanTradeNetworkSystem.cs`, `CaravanAtomicTrader.cs`, route/catalog data | READ ONLY first; conditional owner-specific MODIFY only if source owner plan allows | Check one-off exchange/atomicity | High overlap and transaction integrity |
| `Assets/Ashfall.Core/Economy/BlackMarketSettlementService.cs` | READ ONLY; no change planned | Existing atomic buy/sell settlement example | High custody and money conservation risk |
| `Assets/Ashfall.Core/Economy/TradeCreditCoordinator.cs` | READ ONLY; no change planned | Preserve credit gates and compensation | High debt and item duplication risk |
| Contract Board 109 source/tests (exact paths TBD) | READ ONLY in P0 | Verify live canonical offer/escrow owner and overlap | High; another package/claim may own it |
| Long Line: Freight source/tests (exact paths TBD) | READ ONLY in P0 | Verify company/run and receipt owners | High; separate feature scope |
| Existing inventory/wallet owners (exact paths TBD) | READ ONLY; MODIFY only their integrator if required | Confirm conservation and atomicity | High, shared economic seam |
| TH-1-selected identity owner (exact path TBD) | Conditional MODIFY for pointer only | House member/source relation if approved | Medium/high save ownership |
| Existing host command/panel files (exact paths TBD) | Conditional MODIFY | Route one explicit command and display canonical result | UI may accidentally own transaction rules |
| focused tests (exact paths TBD) | Conditional CREATE/MODIFY after duplicate search | Prove no double settlement and source behavior | Test duplication/policy risk |
| save registries/generators | No change by default | No new save section proposed | Shared generated authority |
| this document | DRAFT only | Proposal | Does not grant path ownership |

## 21. Risks

- **Double settlement:** split writes across inventory, funds, route/contract, and House records can mint or destroy value. Mitigation: one existing atomic source service or no support.
- **Retry after ambiguous outcome:** a timeout after commit can lead to duplicate submit. Mitigation: source-owned idempotency/receipt lookup; otherwise no automated retry and no claim of robust clearing.
- **Escrow duplication:** “consignment” wording can imply custody that no owner holds. Mitigation: call it a proposal until canonical acceptance; never mark goods reserved locally.
- **Status disagreement:** copied House status can outlive/countermand the source. Mitigation: status is live-resolved from source, not copied.
- **Unstable record identity:** inspected market ledger has no explicit ID. Mitigation: no durable link until a source owner exposes stable identity under its own plan.
- **Overlap with Contract Board 109:** House obligations could duplicate deadlines, failure, escrow. Mitigation: verify and route to Board owner or stop.
- **Overlap with Long Line: Freight:** a delivery request could become dispatch/road-war feature. Mitigation: link only, never own delivery operation.
- **Credit side effects:** treating insufficient funds as permission to borrow can sign debt unexpectedly. Mitigation: explicit existing offer/accept flow only.
- **Owner state restored late:** House can misreport source as absent/failed. Mitigation: readiness/lifecycle gate.
- **Over-generalization:** a universal clearing interface would force unrelated owners into a common transaction vocabulary and failure model. Mitigation: one explicit path and evidence-based reuse.
- **Host re-entrancy:** repeated UI callbacks, campaign replacement, or signals could execute twice. Mitigation: in-flight guard plus canonical idempotency, lifecycle tests.

## 22. Out of Scope

- A second contract board, escrow/wallet, warehouse, stock ledger, contract offer catalog, or completion history.
- Market pricing, barter, inventories, credit, repayment, tariffs, route scheduling, reliability, convoy losses, freight rates, or settlement formulas.
- Player route establishment/cancel/suspend/resume or caravan scheduling except in their current owner UI.
- Contract Board 109 deadlines, defaults, escrow terms, or failure consequences.
- Long Line: Freight dispatch, cargo, route risks, company ownership, earnings, insurance, and freight receipts.
- TradeCreditCoordinator behavior, debt templates, credit eligibility, automatic loans, or a House borrower account.
- Black-market syndicate, contraband, heat, fences, black-market loans, or settlement mechanics.
- Production chain, stock reservation, manufacturing promises, warehouse, delivery, spoilage, or quality control.
- Cross-owner transaction broker, two-phase commit, saga framework, compensation engine, retry queue, or generic provider registry.
- Narrative content, faction standing, reputation, diplomacy, governance, law, and new campaign flags.

## 23. Rollback Strategy

Keep the first implementation path isolated behind its existing owner command and House UI route. Removing the House entry point must not alter or migrate source owner state. Transient proposals disappear on close. Canonical accepted transactions remain owned and visible through their existing surface.

If a reference field is added to an existing owner, it must be optional and additive. Rollback may hide the House pointer display without removing canonical transaction data. Never “roll back” by refunding or reversing source transactions from a House adapter. Reversal is permitted only through the source owner's existing refund/cancel operation and its normal eligibility checks.

If the source service cannot distinguish no-commit from unknown/partial commit, implementation stops before UI launch. A rollback procedure cannot safely compensate an operation whose canonical outcome is unknown.

## 24. Definition of Done

- Current overlap/source audit proves one canonical owner path or documents a stop condition; Long Line: Freight and Contract Board 109 are rechecked against live source and claims.
- Preview is pure; commit is explicit; source revalidates; transaction and receipt are canonical.
- Exactly one owner or existing source-specific service coordinates every mutation.
- There is no new House escrow, stock, money, contract state, deadline, completion state, or day tick.
- Duplicate submit, timeout/unknown outcome, stale preview, source deletion, and save/restore behavior are specified and tested.
- Optional House tracking is only an owner-qualified pointer held by an approved existing owner; absent stable IDs mean tracking is omitted.
- Existing flows behave as before when House is absent or disabled.
- Scoped tests run only via `bin/run-scoped-tests`; exact command/result reported. Full suite requires the exact user text `RUN FULL TESTS`.
- Claims are verified, plan is approved before implementation, and no shared path is edited without integrator ownership.

## 25. Implementation Handoff

### MUST PRESERVE

- Existing source owner validation, transaction ordering, settlement result types, save stores, day owners, and failure semantics.
- Inventory/funds conservation, source-owned ID and idempotency rules, and the distinction between preview, accepted contract, and completed transaction.
- Long Line: Freight, Contract Board 109, `TradeCreditCoordinator`, market/routes, and black-market settlement as separate owners.

### MUST ADD

- Only after Phase 0: one narrow explicit action path to a verified owner and source-owned result display; optional stable pointer only through an approved existing owner.
- Focused proof for stale preview, refusal no-mutation, duplicate/retry behavior, and canonical receipt/status.

### MUST NOT DO

- Create a generic House consignment state machine, persistent offer board, escrow balance, copied cargo, stock, money, route, debt, or receipt history.
- Execute raw inventory+wallet+market mutations from the House, auto-accept a credit offer, retry unknown commits, or fabricate an ID/status.
- Treat a route contract, freight run, posted offer, or market ledger row as a one-off consignment without proving that source owner's actual semantics.

### VERIFY WITH

- Current code and tests for exact source preview/commit/receipt, save owner, stable IDs, idempotency and recovery.
- Collision/status review against Long Line: Freight and Contract Board 109 plus `WORKTREE_OWNERSHIP.md`.
- Focused `bin/run-scoped-tests` targets; bounded Godot headless check only when host/runtime paths change.

### FIRST SAFE IMPLEMENTATION STEP

Run Phase 0 and produce one source contract card for the narrowest candidate path. If no owner provides an explicit canonical commit plus stable result/recovery contract, stop and return the missing seam to the foreman; do not add House persistence or a transaction coordinator.

## Appendix A — Source Capability Triage

The names below identify audit targets, not automatic approvals. The “candidate outcome” is the safe conclusion if current source proves only the API evidence already inspected.

| Source path | Existing capability evidenced | Can it fulfill a one-off House consignment now? | Safe candidate outcome |
|---|---|---|---|
| `MarketSystem` | Price/market activity, demand pressure, quote/price explanation, and market ledger; `Buy` / `Sell` / `Barter` update these facts but not inventory/wallet. | No as a complete exchange. | Use for read-only quote context or market-side bookkeeping only; identify the separate caller/service that settles canonical custody before proposing any House path. |
| `PlayerTradeRouteSystem` | Recurring route registration, lookup, run outcome, schedule and persisted contract fields | No, not as a generic one-time consignment. | Allow navigation/link to an already active route only if source key lifecycle is stable. Keep route creation and day tick in route owner. |
| `TradeRouteHostSession` | Route commands, read projections, risk evaluation, day tick and catalog load | No evidence of a generic one-off handoff. | Use for read/route navigation; do not overload `EstablishContract` as consignment acceptance. |
| `CaravanTradeNetworkSystem` | Manifest stock, route lifecycle, and `ExecuteBarter`, which consumes/grants player inventory and mutates manifest stock after validation | Conditionally, only its specific arrived-manifest barter; method-level exception atomicity is not demonstrated by the inspected body. | Audit its caller/tests and repair only under the caravan owner’s approved scope; no House action until conservation holds under injected failure. |
| `CaravanAtomicTrader` | Quote-ID duplicate guard plus persisted `CaravanCommittedTrade` entry; no inventory or wallet mutation in inspected type | No as an exchange settlement by itself. | Treat as a commit-record owner only. Audit caller coupling and save owner; do not use “atomic” name as proof of goods movement. |
| Contract Board 109 | Plan scope includes posted work, deadlines, escrow, failure consequences | Strong collision. Live implementation status/API must be verified. | If active, its owner defines the contract path. If incomplete, do not recreate a subset in House; report the missing owner seam. |
| Long Line: Freight | Existing plan for player's freight company and runs | Strong collision around carriage/delivery. | Link to the freight owner when it has an accepted run/receipt; no House dispatch. |
| `TradeCreditCoordinator` | Credit offer/acceptance, gate revalidation and compensation around canonical debt/inventory | Not a generic settlement owner or payment fallback. | Keep credit flow explicit and separate. |
| `BlackMarketSettlementService` | Buy/sell preview and callback-coordinated stock, wallet, and inventory settlement; durable unique receipt identity still unverified | Yes only for its own existing black-market actions and catalog semantics. | Forward canonical black-market UI operation if approved; never wrap/copy settlement. |

## Appendix B — Exchange Invariants and Conservation Checks

Any future supported path should name the exact resource conservation equations its current source contract guarantees. The House must not supply its own equations after the source has already acted.

### B.1 Inventory

For a canonical sale of quantity `q`, the source owner’s transaction should enforce the existing inventory cost of `q` and one settlement result. The House may not separately subtract the same goods or use a cached preflight count to permit a second action. For a purchase, granted quantity must match the canonical transaction’s accepted result. A UI preview is never custody.

### B.2 Currency or value

The payer debit and recipient credit must be coordinated by the canonical transaction service. If the current transaction uses barter rather than currency, its whole-unit/remainder rule remains source-owned. The House must not normalize currencies, round a market value, or interpret chits/value units as interchangeable.

### B.3 Contracts and routes

An accepted recurring route remains one route record, with cadence and route outcomes advanced by the route owner. An accepted posted-work contract remains one Board record, with its deadline/escrow/failure rules owned there. A freight run remains one freight record. No House counter may increment independently when the same owner does.

### B.4 Credit

An offer is not an obligation. A signed debt must come from the canonical ledger ceremony. Principal disbursement and debt signing remain coupled by the current coordinator; no consignment path may create a second debt while also paying the source transaction.

### B.5 Failure and compensation

Compensation is not a general House privilege. A source-specific service may compensate an operation only through its defined inverse and only when its transaction contract establishes the first action occurred. If an exception leaves commit status unknown, stop and recover from the source owner; never issue the inverse based on speculation.

## Appendix C — State-Transition Table

| User-visible phase | Durable canonical fact? | House mutable state? | Allowed next step |
|---|---:|---:|---|
| Forming a draft request | No | None beyond transient controls | Edit, cancel, or request a read-only preview |
| Preview available | No | None; do not reserve goods/funds | Confirm explicitly, revise, or cancel |
| Preview stale | No | None | Ask source for a fresh preview; do not auto-commit |
| Commit in progress | Not yet known to House | In-flight UI guard only | Await one result; prevent repeat action |
| Source refuses | No accepted source record unless source says otherwise | None | Display canonical refusal; return to editable proposal |
| Source accepts and returns receipt | Yes, in source owner | Optional pointer only if authorized and durable | Display source status; do not repeat commit |
| Source outcome unknown | Unknown | No speculative status | Query owner recovery API or halt for source-specific recovery |
| Source completes or settles | Yes, in source owner | No copied completion bit | Display owner-reported outcome |
| Source cancels/expires | Yes, in source owner | No House expiry clock | Display owner-reported cancellation/expiry |
| House pointer missing or invalid | Source may still be valid | No repair by replay | Open canonical source list or show unavailable link |

## Appendix D — Retry and Crash Matrix

| Interruption point | Safe recovery | Unsafe recovery |
|---|---|---|
| Before source call | User may submit once after controls are restored | Persisting request as accepted |
| During preview | Recompute preview | Treat old quote/availability as reserved |
| Before source mutation | Source returns refusal/no commit; user may review fresh terms | Blindly retrying an unobserved request without request-id semantics |
| After source commit but before result reaches UI | Query source receipt/status if supported; otherwise report indeterminate and use source owner recovery | Submit again, debit again, re-grant, or synthesize a success row |
| After source receipt but before optional House pointer write | Keep source transaction; show tracking unavailable; pointer repair may be read-only | Reverse canonical transaction because pointer persistence failed |
| During save/restore | Restore canonical owner first, then resolve pointer | Restore a copied House status over source owner state |
| After source cancellation/expiry | Display current source status | Reopen or extend source record from House UI |

## Appendix E — Source Contract Card Template

Complete one card before enabling each source path. The card is part of the future implementation handoff and should cite code and tests, not merely plans or type names.

```text
Source name and owner:
Current status and ownership claim:
Preview API / result:
Commit API / result:
Canonical mutated state:
Inventory/funds/contract side effects:
Stable receipt / identifier:
ID uniqueness and reuse policy:
Idempotency key or retry semantics:
Partial commit / recovery behavior:
Save section and restore order:
Day owner / expiry behavior:
Events and event ordering:
Existing focused tests:
Additional minimum test if missing:
UI route and owner-unavailable behavior:
Explicitly unsupported variants:
```

An incomplete card keeps the source unavailable through the House. It is acceptable for the initial Trading House release to support no consignment commands while still providing a correct charter and owner-linked read surface.

## Appendix F — End-to-End Exchange Cases

The cases below make the difference between a proposal, a commitment record, and a settled exchange concrete. They are test-design candidates; they do not enable any path by themselves.

### F.1 Market quote is displayed, then request is cancelled

1. The user selects one catalog good and opens the quote view.
2. Host calls `EconomyHostSession.ExplainPrice`, which returns the Core explanation and typed price factors.
3. The UI displays current quote and its component factors. No House record is created.
4. User cancels. No call to `MarketSystem.Buy`, `Sell`, or `Barter` occurs.
5. Market ledger row count, pressure, wallet balance, and inventory snapshot remain unchanged.

This case tests the current safe use of the market read seam. It must not call a market mutation as a “reservation.”

### F.2 Market `Buy` reports accepted but custody is unproven

1. A caller invokes `MarketSystem.Buy(item, quantity, day, counterparty)`.
2. Core resolves item and positive quantity, calculates price, appends the positive market ledger line and trade pressure, and returns an accepted `TransactionResult`.
3. No inventory or wallet mutation is performed by this method.
4. If the caller has a separate inventory/wallet owner, it must provide its own success receipt and compensation/recovery behavior.
5. Until that caller contract is verified, House shows a market-side accounting fact only; it does not display “consignment delivered.”

The test should assert both sides of the boundary: market state changes as documented; inventory/funds do not change from this method alone. This avoids a false integration test that tests only the `Accepted` flag.

### F.3 Same quote submitted twice to `CaravanAtomicTrader`

1. The quote producer supplies a quote with stable `QuoteId`.
2. First `Commit` validates ID, positive offered/requested units, and a positive price multiplier, appends one `CaravanCommittedTrade`, and emits `OnCommitted`.
3. Duplicate `Commit` with the same ID returns `already_committed`, with no second record and no second event.
4. Capture/restore preserves the committed quote guard.
5. Inventory, manifest stock, and wallet remain unchanged by this type itself.

`Ashfall.Core.Tests/Economy/CaravanAtomicTraderTests.cs` covers commit, duplicate commit, input validation, capture/restore and event behavior. That is evidence for the record guard, not for end-to-end cargo settlement. The caller audit must identify any subsequent transfer and its idempotency key.

### F.4 Valid barter with an arrived caravan manifest

1. User selects an arrived manifest and builds offer/request dictionaries with positive quantities.
2. `CaravanTradeNetworkSystem.ExecuteBarter` resolves manifest, status, player inventory, and manifest stock; rejects insufficient stock or nonpositive quantities before mutation.
3. It calculates offered/sought values from canonical item values, route import/export factors, crisis multiplier, favored status and treaty relief; it rejects when offered value is below requested value.
4. It consumes offered player items and adds those items to manifest stock; it removes requested manifest goods and grants them to player inventory.
5. It records profitable trade count/favored status and emits `OnTradeCompleted`.
6. House displays the source result or a stable source record if one exists. The observed method returns values/status, but it does not append a per-barter immutable receipt to `CaravanTradeNetworkSave`.

**P0 gate:** the mutation sequence can throw after an earlier consume/stock change; the inspected body does not wrap each step in a rollback transaction. Even though preflight checks reduce expected failures, external inventory mutation, overflow, or exception behavior must be proven. If inventory can reject `TryProduce` after `TryConsume` succeeds, this source path is unsafe under a failure injection test until repaired by the caravan owner’s approved task. The House must not add compensating mutations around it.

### F.5 Black-market buy with insufficient wallet or inventory capacity

1. UI requests `BlackMarketSettlementService.PreviewBuy`.
2. Service asks canonical black-market owner to preview stock/access/price, resolves item definition, validates wallet debit capacity and inventory grant capacity.
3. If any gate fails, a structured unavailable result includes the source reason. No stock, wallet, or inventory mutation occurs.
4. If preview passes, user confirms. Service performs a fresh preview before commit, then invokes black-market `Buy` with a settlement callback.
5. Black-market owner stages stock decrement; callback debits wallet and applies the inventory transaction. If callback fails or throws, source stock is restored; if successful, service notifies wallet and returns a structured action result.
6. House may display that result immediately. Durable post-reload tracking still requires a verified persisted receipt ID; funds movement source ID or UI event text is not enough.

The existing `Plan211BlackMarketSettlementTests` target is the implementation-time starting point for focused source behavior. The House test should avoid duplicating those owner-level tests; it should only cover that its command forwards once and displays the returned result.

### F.6 Offer appears, then credit is declined

1. A canonical trade failure context produces a possible credit offer through `TradeCreditCoordinator.TryBuildCreditOffer`.
2. House only displays/navigates to the existing offer if explicitly supported; no debt has been signed.
3. Player declines or leaves. `LedgerDebtSystem` remains unchanged and no inventory is granted.
4. Player acceptance must continue through `TryAcceptCredit`, which repeats eligibility checks, presents terms twice, grants principal, signs, and compensates if the grant or sign fails.

Acceptance evidence should include ledger state and inventory snapshot, not only the message string. House is never a second caller that signs around the ceremony.

## Appendix G — Failure and Recovery Matrix by Owner

| Owner path | Failure point | Canonical expected state | House response/recovery |
|---|---|---|---|
| Market quote | Unknown item / missing catalog row | No market ledger append | Show unavailable; no retry loop |
| Market mutation | Valid `Buy`/`Sell` result but caller custody step not yet known | Market row/pressure may already have changed | Do not label settled; require caller’s own receipt contract |
| Market barter | Remainder / floor leaves unequal whole units | Core reports exchanged value and remainder | Preserve remainder field; do not redistribute or credit it |
| Route contract | Registration uses an existing ID | Existing dictionary entry can be replaced | Do not issue House replacement; re-resolve pointer and flag identity discontinuity |
| Route run | Unknown route ID passed to record outcome | No contract mutation (Core silently skips absent contract) | Do not infer failure; ask source for current record |
| Caravan atomic quote | Same quote ID committed again | Source returns `already_committed`; one record | Do not retry as a new quote unless canonical quote producer issues a fresh one |
| Caravan atomic quote | Commit record persisted but cargo transfer absent/failed elsewhere | Commit ledger may show quote while physical state differs | Treat as an unresolved source contract; House cannot call record “settled” |
| Caravan manifest barter | Preflight refusal | Inventory/manifest remain unchanged | Display exact source reason |
| Caravan manifest barter | Exception after partial resource mutation | Potential partial state; no in-method compensator evidenced | Halt House entry point; source owner recovery only; never auto-call barter again |
| Black-market preview | Quote/access/stock/funds/inventory validation refusal | No commit; no final stock or wallet change | Show source reason and allow a new preview after user changes terms |
| Black-market commit | Settlement callback returns false or throws | Black-market stock is restored; callback owner controls its own compensation | Show canonical failure; do not independently refund or re-credit |
| Black-market commit | Source commits but UI loses response | Source result may exist, receipt persistence unknown | Query canonical source if supported; otherwise mark indeterminate and block retry |
| Debt offer | Gate changes before acceptance | Offer stale | `TryAcceptCredit` refusal; no House-side contract write |
| Debt acceptance | Grant succeeds but ledger sign fails | Coordinator invokes revoke and cancels draft per its contract | Render failure; do not retry blindly until query confirms the canonical state |
| Debt acceptance | Same-creditor debt is already unpaid | Offer/acceptance refused by canonical gate | Display owner refusal; do not route through alternate House creditor |

This matrix is a contract review aid, not an instruction for the House to repair another subsystem. A failure case that lacks a source-owned recovery path is a no-go for that action.

## Appendix H — Migration and Rollback Scenarios

### H.1 No new save state

The preferred implementation submits directly to an existing owner, and any pending request remains transient. Existing saves need no migration. A newly added UI surface can be removed without touching economy saves. Canonical accepted records remain available through their existing route, contract, market, or black-market owner.

### H.2 Pointer field is approved in an existing owner

If a later decision authorizes a `sourceOwnerId` + `sourceRecordId` pointer in the selected identity owner:

1. Existing saves deserialize the new optional list as empty/null with a documented neutral default.
2. The existing owner captures/restores the pointer field as part of its own DTO and checksum; no second House save file is added.
3. Restore order loads identity and source owners before view resolution. Missing source records render unavailable and do not delete or recreate source state.
4. Unknown source-owner types are preserved only if the DTO’s forward-compatibility contract allows it; otherwise a migration decision is needed before shipping.
5. Source ID deletion/reuse yields unresolved/ambiguous view status; no fallback match uses display name, item, day, or amount.
6. Rollback removes only the UI projection. It leaves canonical source owner data and user save intact.

This pointer option remains conditional. A required migration record, status snapshot, or retry key must be separately justified; none is automatically folded into the pointer.

### H.3 Source transaction saved, pointer not saved

This split is possible even if the source operation is correctly atomic. Canonical transaction remains committed; optional House pointer write fails. On next load, House may not find the item in its watch list but canonical source owner still has it. The UI must say “source action succeeded; House tracking unavailable” during the immediate session if that is observable, then provide navigation to the source owner. It must not repeat the transaction to reconstruct the pointer.

### H.4 Source schema or content changes

If the source changes its ID format, route identifiers, status taxonomy, or save schema, the House adapter must fail closed until updated. It must not migrate by guessing from old summaries. The source owner supplies any proper migration; House updates only its pointer interpretation after versioned evidence is available.

## Appendix I — Acceptance Test Cases for a Future Narrow Adapter

The eventual implementation should tailor these to the one supported owner; do not create all rows as redundant tests when a canonical owner target already covers them.

| Test case | Setup / action | Required assertion | Existing owner coverage to reuse |
|---|---|---|---|
| Read-only preview | Query preview twice | Equal source result; no state/checksum delta | Owner preview tests |
| Cancel before submit | Open preview and cancel | No owner mutation, no saved House record | UI focused test |
| Stale preview | Change source stock/price before commit | Source revalidates and refuses or returns new canonical terms | Owner stale-preview contract if present |
| Double confirmation | Submit same UI request twice | One owner action; second action blocked/refused by canonical idempotency | Caravan quote or settlement owner tests |
| Owner unavailable | Unbind or omit optional session | Action disabled with availability result; no null crash or fallback owner | Host wiring test |
| Refusal | Force canonical gate failure | Returned reason preserved; inventory/wallet/source unchanged | Source refusal suite |
| Commit success | Execute one supported exchange | One receipt/source record and exact source-owned resource movement | Source owner test; House test only checks forwarded result |
| Crash after commit | Simulate result-loss boundary if harness permits | Query/recovery identifies committed outcome, or UI reports indeterminate and blocks replay | Source persistence/host test |
| Pointer write fails | Force optional projection persistence failure | Canonical source remains committed; transaction is not repeated | Host persistence seam test |
| Source deletion | Remove/cancel linked record | House shows missing/current cancellation; no old copied status | Projection test |
| Save/restore | Commit, capture source owner, restore, reopen | Source status authoritative; pointer absent is safe if unsupported | Existing source round-trip target |
| Campaign replacement | Submit, switch session before callback returns | Old callback cannot mutate/render into new campaign | Lifecycle test |

Test budget remains scoped: select a small number of high-signal tests; do not add one test per failure-text variant or re-run broad suites after each doc/implementation edit.

## Appendix J — Holdfast Terminal as a Candidate Owner

Holdfast trade is a more direct candidate for a player-facing transaction than raw `MarketSystem.Buy` / `Sell`, because it coordinates merchant state, player inventory, and either its internal value or the canonical `FundsLedger`. This makes it important to audit, while still leaving House-level settlement unapproved.

### J.1 Observed source contract

`HoldfastTradeSession.Buy` canonicalizes an item ID, checks catalog presence, optional faction validity/restrictions/embargo, positive quantity, merchant stock, quote bounds, available value, and inventory capacity. Its mutation path adds the item, then changes value, stock, and held state; its catch branch attempts to undo those mutations if an exception occurs inside the block. `Sell` checks player-held quantity, quote/balance bounds and merchant stock overflow, then increases value/stock, reduces held quantity and removes inventory. `BuyWithFunds` and `SellWithFunds` use `FundsLedger` with reason keys and source IDs; the buy path attempts a compensating credit if item insertion fails. These methods return `HoldfastTradeResult` with success, item, quantity, faction, total, and optional funds delta/message.

`HoldfastTradeSaveState` captures current `value`, `held`, and `stock`. `HoldfastTradeSaveStore` persists that state in a checksummed envelope and uses a backup file. No immutable trade operation ID or complete durable transaction list appears in the inspected result/save DTOs. `FundsLedger` movements are capped at 128, so they cannot be treated as an eternal transaction archive.

### J.2 Safe House usage

If P0 establishes that this is the correct canonical player trading surface, TH-2 can propose an explicit “open this trader” navigation or forward an existing user command where the host already exposes it. It may show the immediate `HoldfastTradeResult` returned by the existing operation. It must not wrap the call in a second debit, market-ledger append, inventory grant, or separate House status transition.

The House may not add a preflight path that diverges from the owner’s actual command. If preview is absent or there are separate code paths for quote and commit, the House can present the existing screen and let it own those steps. A successful result supports immediate feedback only. A reloadable House consignment history still requires a source-owned stable receipt or another approved ledger owner.

### J.3 Questions requiring verification

- Does the active Godot player route construct one `HoldfastRuntimeSession` shared by every screen, or can separate sessions hold divergent stocks/value?
- Is Holdfast the intended user-facing endpoint for all House members, or only the player/settlement trade surface?
- Does a successful trade update `MarketSystem` price pressure or market history? The inspected Holdfast methods do not themselves show a MarketSystem call; verify host wiring before combining these facts.
- Which `_value` or `FundsLedger` mode is active at runtime? Can a call accidentally pass both or reach a legacy session with a copied balance?
- Does the existing `HoldfastTradeIntegrityTests` cover exception rollback for both `Buy` and `Sell`, particularly fund debit/credit followed by inventory failure?
- How is `FundsLedger.TryCredit` inverse movement represented during compensation? A debit then credit creates two movement rows; do not erase or reinterpret them as no operation.
- Does the outer command/session provide idempotency for UI re-entry? The `HoldfastTradeResult` has no request key in the inspected shape.
- Is stock shared across trader screens and restored once? If stock is per session or per faction, each House row needs exact owner scope.
- Does legacy held inventory still participate in active campaigns, or has `InventoryMigrator` made canonical inventory sole owner? Never count both representations.

Until these questions are answered, Holdfast is “candidate owner, route for existing operation,” not a new House adapter.

## Appendix K — Extended Source/Receipt Comparison

| Owner/result | Immediate success observable | Durable state observable | Stable per-action identifier observed | Full resource transfer in the inspected type? | House capability with present evidence |
|---|---|---|---|---|---|
| `MarketSystem.TransactionResult` | accepted/item/quantity/unit price/total/remainder | market ledger, trade pressure, indices | No explicit row ID | No, market method only | Quote/accounting context; no complete consignment |
| `CaravanTradeCommitResult` | `Succeeded`, reason, committed trade object | list of committed quote records | Quote ID (must inspect generation/reuse scope) | No, trader only appends commitment record | Link to committed quote as a promise record only if caller semantics are clear |
| `BarterTransactionResult` from `CaravanTradeNetworkSystem` | success, offered/requested values, favored-status unlock | manifest stocks, inventory, faction trade count, favored faction list | Manifest ID locates parent; no per-barter receipt ID observed | Attempts yes; exception rollback not demonstrated | Unavailable until source atomicity and durable receipt requirements are settled |
| `HoldfastTradeResult` | success/item/quantity/faction/total/funds delta | current Holdfast value/held/stock and canonical inventory/funds | No operation ID in inspected result | Yes, within the terminal owner’s path subject to exact mode/tests | Forward existing command and show immediate result; no permanent audit link yet |
| `BlackMarketActionResult` | action/status/reason/item/quantity/value fields | black-market stock/ledger state, inventory, wallet | Unique durable receipt not established here | Yes through service callback and staged stock rollback | Forward only canonical black-market operation; receipt tracking TBD |
| `DebtContract` / credit result | offer/accept result, contract status | current/closed debtor contract lists | Debtor ID is lookup key; immutable contract ID not observed | Coordinator grants item and signs debt with compensation | Show debt owner status only; never treat as merchandise exchange |
| `FundsMovementRecord` | success/failure from debit/credit | balance plus last 128 movement records | sourceId field but uniqueness is caller-defined | Currency movement only, not goods contract | Snapshot/account movement only; not a complete receipt archive |

The table distinguishes “success” from “auditable persisted transaction.” A boolean result is enough to show immediate feedback in its native flow; it is not automatically enough to create a cross-system permanent House ledger entry.

## Appendix L — Host Lifecycle and Campaign Replacement

Before a House screen can forward a transaction, its host must hold the same canonical session instances that the current trade panels use. At P0, trace creation, restore, disposal, and save capture for `HoldfastRuntimeSession`, `EconomyHostSession`, `TradeRouteHostSession`, black-market session/service, `FundsLedger`, inventory, and `LedgerDebtSystem`.

The required lifecycle constraints are:

1. There is one session instance per canonical save owner within a campaign; the House does not instantiate a convenience copy with independent balance or stock.
2. Save capture reads the canonical instance after pending owner operations finish or uses the project’s established snapshot boundary.
3. A new campaign disposes old subscriptions before binding the replacement owner instances.
4. An async UI callback, if any, carries a session generation token or equivalent lifecycle guard so a result from the old campaign cannot refresh or mutate the new one.
5. Owner restoration finishes before the first availability check. A nullable not-yet-restored session is not equivalent to an owner with zero stock.
6. Event subscriptions are removed on view/session disposal; reopening does not multiply updates or submit commands.

No new thread, task runner, polling loop, or event broker is warranted for this synchronous command flow. If an existing trade operation becomes asynchronous later, its cancellation/idempotency model must be handled by that owner, not added in this plan.

## Appendix M — Detailed Go / No-Go Gates

### Go only when all are true

- One named canonical source owner supports the exact proposed exchange variant.
- The selected source is active in the current host path and receives the same player inventory/funds instances used by other trade UI.
- The source revalidates mutable terms at commit or exposes a single command that performs its own validation.
- The House action calls that command once, and retry after a lost response has either an owner-provided receipt lookup/idempotency key or a defined stop-and-reconcile workflow.
- The resource mutation sequence has an explicit owner-tested conservation and failure contract.
- Refusal and unknown result are distinguishable from success.
- Persistent tracking is omitted unless the source provides a stable record ID and TH-1 identifies an approved pointer owner.
- The action does not cross a claimed file boundary or alter Contract Board 109, Long Line: Freight, TradeCreditCoordinator, market, route, or black-market owner behavior.

### No-go if any are true

- The House would call market bookkeeping and separately mutate inventory/currency without an existing transaction owner.
- Only an ephemeral success message or bounded funds log exists, but product acceptance demands a permanent consignment history.
- The required action requires a second stock, funds, escrow, promise, route, or production owner.
- A source failure can leave partial physical state with no source owner recovery path.
- The House must reinterpret source states (e.g. quote offered → contract accepted; manifest arrived → goods transferred; successful API return → durable receipt) to satisfy a UI label.
- A current builder claim overlaps required files, or source ownership cannot be established from the live ledger.

When no-go is reached, record the missing contract precisely—such as “Holdfast commit has no durable receipt ID” or “market transaction result does not settle inventory”—and leave the House action unavailable. A blocker with evidence is the expected outcome of this planning gate, not a reason to improvise a coordinator.

## Appendix N — Canonical Owner Contract Register

The following register expands the capability triage into a per-owner interaction contract. “Observed” is limited to the inspected source. A House command remains disabled until each required `VERIFY` item is closed using current code and focused tests.

### N.1 MarketSystem

**Observed read surfaces:** `GetPrice`, regional `GetPrice`, `ExplainPrice`, and market state/ledger reads. `PriceExplanation` returns factor records from the Core formula. The user-facing host has pass-through explanations and save methods.

**Observed mutating surfaces:** `Buy` and `Sell` funnel into `Transact`, which validates catalog goods and positive quantity, calculates a market price, appends market ledger row, records category pressure, and raises change events. `Barter` validates goods and quantity, computes a floored quantity of the requested good, records equal-value legs and a remainder, then adjusts pressure.

**Not part of these methods:** inventory consumption/grant, wallet debit/credit, merchant stock conservation, counterparty custody, reserve/escrow, durable per-operation identity. Therefore the word “accepted” in `TransactionResult` means accepted by this market bookkeeping call; it does not certify end-to-end fulfillment.

**House eligibility:** read-only quote context only unless Phase 0 identifies a separate existing caller that atomically couples this market path to custody and returns a stable result. The House must not call `Buy` to make a proposal visible, and cannot call it after another owner’s trade to “keep prices current” without the existing owner’s explicit architecture contract.

**VERIFY:** every call site, duplicate invocation behavior, whether market ledger row append can happen before a caller failure, whether market ledger retention is bounded by a separate policy, and how region/counterparty terms are resolved. If no owner can correlate market row to the actual transfer, leave it as a separate fact.

### N.2 CaravanAtomicTrader

**Observed contract:** `Commit(quote)` rejects null via exception, empty `QuoteId`, previously committed quote ID, nonpositive offered/requested quantities, or a nonpositive price multiplier. It appends a cloned record-shaped trade to `CaravanTradeState.Committed`, raises `OnCommitted`, and returns `CaravanTradeCommitResult`. Capture/restore deep-copies committed entries.

**Important boundary:** the commit method has no item inventory or wallet dependencies. Its duplicate guard is scoped to quote IDs currently in this state. It does not establish global uniqueness of quote IDs, quote expiration, freshness of price/stock, member identity, or resource transfer.

**House eligibility:** could only display it as a commitment record if the quote producer and host define exactly what was committed and how the items move. Do not map `Succeeded` directly to `settled` or `delivered`.

**VERIFY:** quote ID generation and collision scope, whether QuoteId is deterministic and stable across restore, state save owner and campaign lifecycle, content of quote builder, subsequent transfer side effects, stale quote rejection, and test coverage. Existing `CaravanAtomicTraderTests` prove duplicate commit behavior, not resource transfer.

### N.3 CaravanTradeNetworkSystem

**Observed contract:** route catalog definitions seed manifests; `ScheduleCaravan` creates a manifest ID using route/day/current list count and deterministic RNG for initial stocks. `FindManifest` and `FindActiveCaravanForRoute` resolve current manifests. `ExecuteBarter` only accepts an existing `Arrived` manifest, validates item quantities and inventory/manifest stock, calculates values, consumes player goods/adds them to manifest stock and removes/grants requested goods. It updates faction profitable-trade and favored status and emits a completion event. `TickDay` advances its own campaign behavior.

**Identity and history limits:** manifest ID describes the parent caravan, not an individual barter. The manifest's `stocks` represent current stock; no per-barter record list is visible in `CaravanTradeNetworkSave`. A House pointer to a manifest does not identify one exchange or prove it was settled.

**Failure boundary:** preflight is explicit, but mutation is a sequence; an exception after consumption or one inventory grant has no visible in-method rollback. The result’s `Success` is returned only after all updates complete; an exception may leave partial state. This is a source owner risk, never a House compensation task.

**House eligibility:** none until a current owner audit proves the exception boundary safe and a source-owned operation receipt exists or the product explicitly accepts immediate-only results. Any needed fix belongs to the caravan owner under a separate approved claim.

**VERIFY:** every use of `TryConsume` / `TryProduce`, exception guarantees and side effects of inventory; maximum quantities and dictionary overflow behavior; manifest removal/retention; save/restore crosswalk; deterministic count-based ID collision after pruning; test coverage for injected failure.

### N.4 HoldfastTradeSession

**Observed contract:** `Buy`/`Sell` and funds variants perform current stock/inventory/value mutations and return `HoldfastTradeResult`. It contains no request correlation key and its save DTO persists current value/held/stock. Fund-denominated actions write a `FundsLedger` movement with source/reason and day. The current result provides useful immediate feedback but does not have an immutable transaction identifier.

**House eligibility:** may link the player to the existing trader surface or forward a canonical command if the host already exposes exactly the same session instance and authorization. Immediate result can be displayed. Reloadable House history requires a new receipt contract owned by Holdfast, not a new House table.

**VERIFY:** active currency mode, runtime session singleton/duplication, legacy held items versus canonical Inventory migration, whether trade affects EconomyHostSession market pressure, stale preview/commit behavior, save owner generation, and exception/compensation semantics for both buy and sell. Explicitly inspect the code path where `SellWithFunds` credits before removing goods and ensure no later operation can fail without compensating.

### N.5 BlackMarketSettlementService

**Observed contract:** preview calls black-market owner, catalog, inventory and wallet checks. Commit obtains a new preview, then asks `BlackMarketSystem` to stage stock, and supplies a callback that performs the owner’s coupled settlement. `BlackMarketSystem.Buy`/`Sell` restores staged stock when settlement returns false or throws. Results include operation/action/reason and resource deltas.

**House eligibility:** if approved, call this canonical service for the source-specific operation and use its immediate result. Do not split callback, manipulate `BlackMarketSystem.State`, call a separate wallet path first, or preserve the preview result for later execution.

**VERIFY:** confirm state-save ownership of black-market ledger/stock/debt, whether `BlackMarketActionResult` itself is persisted, retry behavior after lost UI response, day parameter origin, and how the service is disposed/bound. Existing `Plan211BlackMarketSettlementTests` cover owner conservation and restore scenarios; House-specific test should focus only on forwarding once.

### N.6 Trade routes and Contract Board / Freight

`PlayerTradeRouteSystem` owns recurring scheduled obligations and counters. A route contract is not a single transfer receipt. The Route host day owner calls `TickDay` on its schedule and captures/restores under `trade_routes`. Contract Board 109's named lifecycle already covers posted work, escrow, deadlines, and failure. Long Line: Freight covers a company and runs. None is a generic House commit target unless its active source exposes a supported one-off operation, stable identity, exact status, and no conflicting current claim.

The House command must not register a route to satisfy a consignment, create a Board offer, stage escrow, create a freight run, or call the day owner. It may navigate to the current route/board/freight owner only. The owner of that package decides its acceptance and terms.

## Appendix O — Consignment Reference Schema Questions

The following is a **conditional interface sketch**, not a DTO proposal or save-section authorization. Its purpose is to expose which fields cannot safely be guessed. Default remains no persistent request or House record.

### O.1 Ephemeral request (UI-local only)

| Field concept | Type/constraint | Owner | Status |
|---|---|---|---|
| Source kind | Closed list of explicitly supported owners | House view dispatch | `VERIFY` actual supported source list; no generic plugin registry |
| Good ID | Canonical catalog ID | Existing item owner/catalog | `VERIFY` alias canonicalization occurs only in source owner |
| Offered quantity | Positive bounded integer | Form input; source rechecks | Maximum allowed value is owner-specific, `VERIFY` |
| Requested good/quantity | Source-specific; may be zero/absent for sale | Source command | Do not force market/barter/contract paths into same shape |
| Counterparty/manifest ID | Existing canonical source ID | Route/faction/caravan owner | `VERIFY` namespace and alias rules |
| Price / value | Owner-returned preview only | Source owner | Never user-editable if source controls price; units explicit |
| Requested day/deadline | Omit unless source command requires it | Existing day/contract owner | No House expiry clock; validation source-owned |
| Member ID | Current authenticated actor, not form input | TH-1 identity owner or existing player owner | `VERIFY` available principal and authorization seam |
| User note | Optional length-bounded text | UI state | Does not affect acceptance or save; escaping/content safety `VERIFY` |
| Preview fingerprint | Owner-defined, opaque; if none, no custom field | Source owner | Do not synthesize from price fields; stale detection `VERIFY` |

On close/cancel or campaign switch, ephemeral request is destroyed. Its disappearance cannot cancel or erase a source contract because it was not source state.

### O.2 Optional durable pointer (only after signed decision)

| Field concept | Requirement | Unknown requiring `VERIFY` |
|---|---|---|
| Owning House/member identity | Stored only by selected existing owner | Which current owner persists House membership? TH-1 P0 unresolved |
| Source owner ID | Stable closed identifier, not runtime type name | Canonical IDs and future rename/migration policy |
| Source record ID | Owner-issued stable ID | Uniqueness scope, deletion and reuse policy |
| Link creation day | Source or campaign day at acceptance | Whether source returns accepted day or caller supplies it |
| Relation label | Small display-only vocabulary such as `participant` or `referrer` | Is this needed at all? Avoid a new permission contract |
| Source schema version | Only if source has public version semantics | Existing owner’s compatibility contract |

**Must not be stored in the pointer:** item quantity, source quote, price, due day, escrow, cargo status, “settled” boolean, debt balance, market pressure, copied completion/failure, House payout, or replayable transaction command. Those values change at the owner or are not currently verified.

### O.3 Unknown/invalid pointer behavior

- Unknown owner type → preserve only if owner schema supports forward-compatible opaque data; otherwise migration must explicitly drop the link with diagnostics. Do not direct it to a default owner.
- Unknown source ID → display unavailable; never search other owner tables for a matching string.
- Deleted source → mark missing, keep no copied terminal status.
- Reused source ID → ambiguity if source exposes generation/version; absent that, no permanent guarantee can be made, so do not create the pointer initially.
- Malformed owner-qualified pair → reject at data boundary, do not route a player action.
- Wrong campaign/session generation → drop view binding, not source state.

## Appendix P — Conservation Accounting by Transaction Shape

This appendix states the minimum accounting proof required before an owner can be exposed as a consignment action. It does not define new game economics.

### P.1 Purchase with integer chits

For an existing owner that accepts one purchase, compare before and after snapshots:

```text
player item quantity delta = +q
merchant canonical stock delta = -q
FundsLedger balance delta = -c
one owner result = success(item, q, c, counterparty)
```

The owner’s rounding/price function defines `c`. The House must not recalculate `c`, append a second funds movement, or independently alter merchant stock. On any refusal, all three state deltas are zero. If owner uses an internal value instead of FundsLedger, those units remain separately named and cannot be combined in the same equality.

### P.2 Sale with integer chits

Expected owner-level changes for sale of `q`:

```text
player item quantity delta = -q
merchant canonical stock delta = +q
FundsLedger balance delta = +p
one owner result = success(item, q, p, counterparty)
```

Before enabling the action, prove that stock overflow, wallet overflow, inventory mismatch, and failed credit leave all owners unchanged. The House does not “repair” a mismatch by copying goods or funds.

### P.3 Barter between two inventories

For player/caravan barter, track four quantity deltas independently: offered item leaves player and enters manifest; requested item leaves manifest and enters player. Verify item identities are canonical and quantities are exact. Value tests remain a separate constraint from custody. The offered/requested values do not constitute currency and cannot be added to FundsLedger balance.

For market `Barter`, the inspected MarketSystem method only books accounting legs and market pressure. Its equal-value accounting invariant does not prove the four custody deltas above occurred.

### P.4 Black-market purchase/sale

The black-market service owns price conversion and settlement units; the transaction must conserve stock, wallet, and inventory according to the same result. Tests already named include `Buy_RejectingSecondLeg_RollsBackStagedStockAndWallet`, capacity/stock/funds refusal, `Sell_RejectingSecondLeg_RollsBackStagedStock`, and a save round-trip through three owner stores. House verification should be a forwarding test and must not copy those owner assertions.

### P.5 Credit or loan interaction

A purchase funded by a new debt would combine commodity transfer, debt signature, creditor eligibility, and possibly funds movement. The existing `TradeCreditCoordinator` couples ledger signing to item grant with compensation, but it is a distinct credit flow and has a fixed debtor identity. TH-2 cannot treat debt as the payment leg of an arbitrary House consignment. If a future owner command invokes both a sale and credit, it must be a specific approved integration with the ledger owner, not a House-side composition.

## Appendix Q — Host Command, Route, and Lifecycle Cases

### Q.1 Open existing trader

The safest current House interaction may be a navigation command that opens an existing trader/board/route surface. It carries no request payload, cannot mutate, and inherits the source surface’s availability, focus, close, and save behavior. The destination should be chosen from current `PanelRegistryBootstrap`/Main routing and verified at implementation time. Do not add new hardcoded route IDs until the source panel ID and active route are confirmed.

### Q.2 Direct command forwarding

If the House surface submits an existing command directly, it must pass the same canonical host session and owner inputs as the native panel. A separate helper that creates a new `HoldfastTradeSession`, `FundsLedger`, `Inventory`, `MarketSystem`, or `BlackMarketSystem` is forbidden because the result would mutate a shadow instance. One shared command path is preferable; if existing command routing requires a request object, reuse its command envelope instead of a House-only command bus.

### Q.3 Daily route update

`src/Main.CampaignOwners.cs` registers a `TradeRouteDayOwner` at phase 5. Route host/day owner remains responsible for route cadence and outcomes. House view must never call `TradeRouteHostSession.TickDay` or count `NextRunDay` crossed as a completed run. The owner’s event/census/status are the only supported display facts.

### Q.4 Campaign reset while UI is active

1. Old view is closed/disposed and detaches owner events.
2. Old request is discarded if not submitted; if submitted, result is associated with its original source/session.
3. Main disposes/replaces source sessions per existing lifecycle.
4. House binds new owners only after their restore barrier.
5. A late old callback cannot change the new campaign’s House page or source owner.

If commands are synchronous, still verify event re-entry and UI close ordering; do not assume a callback can never be late merely because current implementation appears local.

### Q.5 Duplicate click protection

The UI disables confirmation while the synchronous operation is executing. This is only a presentation guard, not durable idempotency. Crash/retry safety must still come from the owner’s request ID/unique quote ID or source lookup. If owner has no stable retry contract, the operation should use its canonical panel and House should not provide a second submit surface.

## Appendix R — Detailed Refusal and Feedback Mapping

The House must preserve source failure categories and not convert them into a single message that invites unsafe retries.

| Source refusal class | Example owner evidence | User feedback meaning | Safe retry |
|---|---|---|---|
| Invalid request | Nonpositive quantity; unknown item | Correct the request fields | Only after editing request and obtaining new preview |
| Stock unavailable | Merchant or manifest stock insufficient | Source cannot fill this quantity now | New preview after source state changes |
| Actor not authorized | Wrong faction/access tier/member gate | This actor cannot use this source | No automatic retry; use only canonical authorization route |
| Insufficient player goods | Inventory cannot cover offered leg | Player does not currently possess offered goods | After inventory changes and fresh preview |
| Insufficient capacity | Inventory cannot accept requested goods | Destination cannot receive goods now | After capacity changes and fresh preview |
| Insufficient funds | Wallet/ledger refuses debit | No settlement has occurred | Canonical credit offer only if existing flow produces one; never auto-create debt |
| Embargo/hostility | Source returns current diplomacy gate | Source is unavailable due to current policy | Wait for canonical policy change; do not use alternate House owner |
| Stale quote | Canonical commit rejects old preview | Terms changed; no acceptance | Fresh canonical preview |
| Duplicate request | Owner says already committed/settled | Existing action may already exist | Query source receipt; do not submit new ID or infer success |
| Unknown outcome | Exception/timeout leaves commit state uncertain | The House cannot confirm | Stop and query source recovery; no automated retry |
| Owner unavailable/not restored | Host session not ready | Service cannot be called | Reopen after owner ready; do not reinterpret as refusal or empty stock |
| Unsupported transaction family | No canonical command exists | House cannot process this exchange | Navigate to supported owner or leave unavailable |

The source message can be translated to player-facing text, but the underlying error code should remain available for diagnostics and focused tests. Never tell the user “nothing happened” after an unknown partial commit.

## Appendix S — Focused Test Matrix with Evidence Targets

Existing owner tests discovered during this audit:

| Existing test file | Evidence covered or likely owner scope | House gap that remains |
|---|---|---|
| `Ashfall.Core.Tests/Economy/CaravanAtomicTraderTests.cs` | Commit adds record; same quote cannot duplicate; input validation; capture/restore; one event per accepted record | Does not prove item/funds transfer or stable receipt-generation contract |
| `Ashfall.Core.Tests/Economy/CaravanTradeNetworkTests.cs` | Standard manifest barter and network progression | Review exact injected-failure coverage; House must not duplicate ordinary valuation tests |
| `Ashfall.Core.Tests/Economy/Plan211BlackMarketSettlementTests.cs` | Stock/wallet/inventory conservation, refusal no-mutation, second-leg rollback, loan/repay, three-owner save round-trip | Verify current target contents/status; House needs at most a command-forwarding/no-second-call test |
| `Ashfall.Core.Tests/Economy/TradeRouteContractTests.cs` | Reliability, cadence, cooldown, contract save/restore | House projection must not re-test route math; only source-link and no-tick behavior |
| `Ashfall.Core.Tests/Economy/Plan192TradeRouteHostIntegrationTests.cs` | Route host census, commands, save/restore and corruption | House must not duplicate host lifecycle assertions |
| `Ashfall.Core.Tests/Economy/HoldfastFundsTradeTests.cs` | Buy/sell fund balance, stock and inventory, insufficient funds refusal | Exact purchase/sale exception and operation-ID gap remain |
| `Ashfall.Core.Tests/Economy/HoldfastTradeIntegrityTests.cs` | Overflow, unrepresentable quotes, embargo preview, stock/wallet overflow and preserving inventory/funds | Need House route-to-same-session/no-double-submit coverage only if direct forwarding is added |
| `Ashfall.Core.Tests/Economy/TradeCreditCoordinatorTests.cs` | Offer gates, acceptance, compensation and repeated creditor protection (inspect exact cases) | House must not create a second credit identity or debt transaction |
| `Ashfall.Core.Tests/Economy/LedgerDebtSystemTests.cs` | Debt lifecycle/state behavior | Stable historical debt ID is still absent in inspected DTO |

### S.1 Minimum future House-focused tests

If the House only navigates to canonical panels, no economic Core tests are necessary; one UI route test can prove navigation and route availability. If direct forwarding is implemented, the smallest useful set is:

1. one successful request reaches the exact active source instance exactly once and displays its returned result;
2. one refusal forwards the source reason and shows unchanged canonical before/after snapshots;
3. missing owner or incomplete restore disables action without constructing a replacement instance;
4. campaign replacement prevents old UI/session callbacks from writing into the new session;
5. optional pointer capture/restore works without copying owner terms or status, with missing-source behavior verified.

For a partial/unknown commit, prefer a source-owner test that proves the recovery contract. A House test cannot prove atomicity by asserting that its method returned `false` if source state was never inspected.

## Appendix T — Production Trade Flow Overlap Audit

`docs/production/PRODUCTION_TRADE_FLOW.md` looks like a potential umbrella owner for consignments because it describes regional buyers, export barter, regional pricing, hauling mass, and a single settlement ledger. Its header names `Assets/StreamingAssets/Data/trade_flows.json`, `ProductionTradeFlowSystem.cs`, `RegionalPriceCurveCalculator.cs`, and `FactionLedger`. The document labels these canonical and reports broad verification. Such declarations are not enough to make its API a live dependency.

The bounded current-tree scan searched `Assets/Ashfall.Core`, `src`, `Assets/StreamingAssets/Data`, and `Ashfall.Core.Tests` for `ProductionTradeFlow`, `RegionalPriceCurve`, `trade_flows`, and `ProductionTradeFlowSystemTests`; no corresponding current source, catalog, or test filenames were returned. That finding is limited to present paths and those names. It does not prove whether the document is stale, points to a renamed owner, or describes a future feature. Recheck the precise target branch before implementation and inspect any alternate-name owner before concluding absence.

The consignment layer cannot be used to instantiate that missing regional trade authority. If a live regional trade flow is found, a consignment may at most be an owner-issued pointer or a host navigation step. Price quotation, buyer demand, export eligibility, route/haulage capacity, stock, escrow, delivery receipts, settlement and standing updates stay with their proven owners. The House will not copy the production document’s price schema, anti-arbitrage rules, or regional flow state. If the owner is absent, existing canonical barter/trader surfaces remain the available path; unsupported consignment execution is explicitly unavailable.

| Production document declaration | Verification in this audit | Consequence for TH-2 |
|---|---|---|
| `ProductionTradeFlowSystem.cs` owns commerce | Named file not located in searched runtime trees | No compile-time dependency may be planned yet |
| `RegionalPriceCurveCalculator.cs` computes live prices | Named file not located | Do not place price fields or curve snapshots in consignment request |
| `trade_flows.json` is catalog authority | Named catalog not located | Do not add a substitute consignment catalog with buyers/prices |
| `FactionLedger` is settlement authority | Exact current type/path/API not verified in this scan | No assumed escrow/settlement contract |
| Regional haulage/export behavior is tested | Matching named test file not located | Document’s verification claim is not current test evidence |

This overlap audit adds a required source search to phase zero: search active code and data by concept as well as exact claimed names, inspect registrations and call sites, and reconcile results with `INTEGRATION_PLANS.md` / `WORKTREE_OWNERSHIP.md`. Record exact live type and file paths in the implementation plan. A prose plan never grants file ownership or architecture authority.

## Appendix U — Source-By-Source Consignment Capability Contract

The same player intent (“leave these goods with someone and receive value later”) can map to very different current source capabilities. This table prevents a future UI from conflating a quote, a committed record, a completed barter, or a settled exchange.

| Current source | Actual supported action in audited code | Capability classification for consignment | Required handoff if used |
|---|---|---|---|
| `MarketSystem` | `Buy`, `Sell`, `Barter` append market ledger facts, record pressure/events; audited methods do not move inventory or wallet | Ledger recording only; cannot fulfill delivery | Use only as a separate recorded fact after an owner that actually exchanges goods acts; confirm whether duplicate market and settlement records occur |
| `CaravanAtomicTrader` | Commit quote ID and persist a commit record; no inventory/funds movement | Quote-commit journal only | Never render “delivered”; do not retry an already committed ID; a separate owner must prove actual exchange |
| Caravan trade network | Validates arrived manifest/player stock, mutates inventory and manifest stock, applies progression, emits completion | Completed barter interaction, but stable durable receipt and exception recovery need verification | Preserve the network owner as command/settlement boundary; query current source after any ambiguous outcome |
| Holdfast trader session | Validates stock/inventory/value, returns operation result; funds-aware API checks balance; session save carries trader state but no observed receipt history | Current merchant interaction, source-scoped funds settlement | Route to same live session; no background action from House; capture receipt in presentation only for current interaction |
| Black market settlement service | Coordinates source preview with callback for wallet/inventory and source stock staging/rollback | Source-specific coordinated settlement | Call only its canonical host route; on unknown outcome require stable receipt before any retry |
| Trade route system | Runs route contracts on day cadence, reports aggregate success/failure counts | Autonomous scheduled delivery authority, not an on-demand consignment form | House may link to a route contract only if source ID and owner API are stable; cannot create/tick route itself |
| Trade credit/debt | Canonical creditor offer/acceptance and debtor state | Separate credit obligation | Never translate an unpaid consignment into debt unless canonical credit owner explicitly executes such a contract |

### U.1 Capability truth in user-facing language

An interaction should use the source owner’s verb. For MarketSystem, “recorded” is accurate for the inspected mutation; “paid,” “delivered,” and “accepted by buyer” need additional owner evidence. For `CaravanAtomicTrader`, “quote committed” is accurate; it is not proof of inventory or funds. For the network owner, the completion event is evidence that its barter path reported completion, but that does not automatically supply durable deduplication after reload. For a trade route, its cadence and aggregate counters are not a one-time consignment acceptance receipt. Keep these distinctions visible in logs and tests.

### U.2 Owner-specific command gate

Any proposed command dispatcher should be exhaustive over supported source types and return `UnsupportedSource` for a source without a proven command. Avoid a generic `ExecuteTrade(request)` facade that assumes all operations have the same quote, transaction, save and rollback semantics. A narrow host adapter may map one specific UI action into one specific source API, but it must preserve source result types, IDs, event timing and exception behavior. If callers need a new union result DTO, it should be an ephemeral host presentation DTO and carry the unmodified source status, not a parallel business contract.

## Appendix V — End-to-End Request Lifecycles by Existing Owner

These sequences state the minimum facts the UI would need to show truthfully; they do not prescribe implementation details for the owners.

### V.1 Holdfast trader: success and reload

1. Bind the active campaign’s existing `HoldfastTradeSession`; block input until its save restoration and campaign binding are complete.
2. Player selects a current stock item and quantity. The active session validates that stock, inventory/capacity, quote value and funds cover the operation.
3. On accepted result, the source session owns the resulting stock/value/funds/inventory mutations according to the specific API used. Host presents the returned result once.
4. If the campaign saves afterward, its established save path restores `value`, `held` and `stock`; no per-operation receipt list was identified in the inspected state. After reload, the UI can show current trader state, not a reconstructed historical operation row.
5. If the player says “I clicked twice,” the House cannot infer whether the first call committed from its own panel state. It must ask the source owner for a stable operation receipt; absent that support, it can only show current state and must not resubmit automatically.

### V.2 Caravan quote record: commit then separately settle

1. Existing quote owner creates a quote and supplies its ID. The ID must be stable for that quote; House cannot mint a replacement on retry.
2. `CaravanAtomicTrader.Commit` validates request and duplicate ID, then appends the canonical commit record and emits its event.
3. The application must distinguish that append from downstream barter. The inspected method does not alter inventory or funds.
4. A separate caravan network call may later validate an arrived manifest and complete barter. Only that owner’s returned completion status and mutations describe exchange.
5. Save/restore of the atomic trader commit record allows that specific record to be recovered, but it does not prove whether a separate barter operation committed. The source network needs its own stable receipt or the interaction remains ambiguous across a crash.

### V.3 Black-market coordinated exchange: refusal and compensation

1. Route the request through the established black-market host owner; preview source-specific constraints.
2. On commit, the settlement service invokes the coupled callback while the system stages source stock. Callback rejection or exception triggers observed source-stock rollback.
3. Verify that the callback restores both wallet and inventory on each failure point, and verify whether the exception is propagated or normalized. Existing focused tests cover multiple failure/round-trip cases, but current exact scope should be checked before relying on individual assertions.
4. Once accepted, show the exact source result. If a reload follows, rely only on the three owners’ existing save contract; the House cannot provide distributed transaction recovery.
5. For a lost response after mutation, retry only if the source exposes a durable unique action receipt and returns the prior outcome. Otherwise stop and report unknown outcome for recovery.

### V.4 Route contract: planned run vs aggregate outcome

1. The player opens the route owner through its current host panel. A route contract remains owned by `PlayerTradeRouteSystem` and host registration/day order.
2. The House can show owner-provided contract fields or aggregate counters if they are available through supported reads. Do not make a second `TickDay`, create a route schedule, or derive run-level deliveries from totals.
3. At the registered day phase, host ticks the owner once; its state is persisted by the `trade_routes` section / `trade_routes_save.json` route. House view refreshes after owner events or next open.
4. After restore, compare the owner’s aggregate counters and contract state only. If a specific run receipt does not exist, do not display a fabricated delivery chronology.

## Appendix W — Exception and Recovery Decision Table

| Observed condition | Source-specific recovery question | House-side action | Persisted House data allowed |
|---|---|---|---|
| Validation rejects before mutation | Did owner guarantee rejection is no-op? | Display refusal; let player edit and obtain fresh preview | None; optional transient error detail only |
| Duplicate quote/action ID | Can owner retrieve prior canonical result? | Query owner once; otherwise halt as unresolved | Never change ID to bypass duplicate gate |
| Exception before source invocation | Did host prove command was not dispatched? | A fresh player action may be offered after checking source state | No permanent request journal without approved owner |
| Exception during single-owner mutation | Does owner roll back atomically? | Use owner recovery contract; if absent label unknown | Do not store copied assumed result |
| Failure after one of multiple owners mutated | Is there a compensation or idempotent replay contract? | Stop automated retry; route to recovery | Never persist a second escrow/debt/stock ledger |
| Save write interrupted after successful action | Can owner reconcile its save envelope/checksum/backup? | Reload canonical owner via existing recovery path | House references only if owner key survives and semantics approved |
| Source owner absent after campaign restore | Is absence valid for old save or corruption? | Keep action disabled; show source unavailable | Do not create an empty replacement owner to make UI operational |
| Source receipt absent due to bounded retention | Can current state still resolve the action? | State history not available; do not guess | Do not extend retention locally |

The most important recovery boundary is one-way: a House plan can orchestrate a call, but it cannot undo canonical owner mutations unless the owning API explicitly offers compensation. Host-side exception handling must not swallow a failure and return a generic refusal where state may already have changed.

## Appendix X — Consignment Field Contract: Required Evidence per Candidate

The earlier candidate schema intentionally stayed small. This appendix records what each tempting field would mean, its source, and the proof needed before it can exist. No candidate below is a committed schema. For a UI-only workflow, these values should remain transient request state and disappear when the interaction ends.

| Candidate field | Intended meaning | Current source or authority | Status and verification required | Reject/omit rule |
|---|---|---|---|---|
| `requestKey` | Stable idempotency key for one user-requested operation | Must be generated/issued by the canonical transaction owner or an approved host command | VERIFY whether each owner accepts client keys and whether the key survives save/restore | Omit for owners lacking a dedupe contract; never generate a different key after unknown outcome |
| `sourceKind` | Exact canonical command owner family | Host route registration and owner type | VERIFY exact active route and owner; production trade flow docs are not proof | Unknown value returns unsupported; no default owner |
| `sourceRecordId` | Stable ID of a source-issued quote, contract, manifest or operation | Source owner | VERIFY existence, uniqueness scope, case sensitivity, retention and lookup | Null if owner has no stable ID; never derive from day/item/price |
| `campaignId` | Campaign identity in which the request was issued | Campaign/save owner | VERIFY current campaign ID API, rename/recovery semantics | Do not substitute global singleton or faction ID |
| `actorId` | Authorized survivor/player actor | Canonical player/identity owner | VERIFY source accepts and checks actor identity | Do not infer from UI selection or House membership |
| `counterpartyId` | Canonical merchant/faction/manifest endpoint | Source owner/catalog | VERIFY canonical namespace and case rules; display names are not keys | Reject unresolved IDs; do not fuzzy-match display names for settlement |
| `itemId` | Catalog key for requested goods | Item catalog and source acceptance validator | VERIFY valid item and source-compatible units | Preserve unknown raw key for diagnostics; source must refuse unsupported item |
| `quantity` | Positive units requested for the named item | Source owner validation; inventory/manifest owner | VERIFY integer vs fractional quantity, maximum and overflow semantics | No silent clamping or float-to-int rounding |
| `offeredLeg` | Goods/funds offered by the actor | Exact source API and canonical inventory/funds authority | VERIFY whether owner models one or multiple legs and whether quote includes transport/fees | Do not locally decrement or reserve inventory |
| `requestedLeg` | Goods/funds requested from counterparty | Exact source API | VERIFY direction, sign convention and whether source already values it | Never call this “profit” without canonical settlement facts |
| `quotedValue` | Non-binding or binding source quote | Source preview API | VERIFY unit, expiry, deterministic inputs and whether preview mutates pressure/history | Label preview/quote; never persist as settled value |
| `quoteExpiresAt` / `quoteDay` | Quote validity boundary | Source owner/campaign clock | VERIFY exact clock domain and exclusive/inclusive expiry semantics | Do not compute expiry from wall-clock or UI timer |
| `routeId` | Existing route that may transport cargo | Route owner | VERIFY whether source supports linking consignments and stable IDs | A descriptive route name is not transport allocation |
| `manifestId` | Existing caravan manifest involved in barter | Caravan network owner | VERIFY stable ID, arrival state, capacity ownership and save path | No new shadow manifest if absent |
| `status` | Current source lifecycle state | Source owner enum/status | VERIFY authoritative transitions and whether state is restored or derived | No House-authored `Delivered`, `Escrowed`, or `Settled` state |
| `acceptedAtDay` | Source acceptance campaign day | Canonical clock as written by source | VERIFY owner records it and save roundtrip preserves it | Never use panel-open day as acceptance date |
| `fulfilledAtDay` | Actual owner-reported fulfillment time | Source completion fact | VERIFY stable completion receipt and time semantics | No computed delivery date from route cadence |
| `receiptId` | Durable unique operation result | Source settlement owner | VERIFY uniqueness, query API, retention and crash consistency | Without it, mark ambiguous result unknown and prevent retry |
| `failureCode` | Source refusal/exception classification | Source result enum/exception mapping | VERIFY stable code values and localization mapping | Preserve unknown code; never convert to success/empty |
| `actorDisplayName` / `counterpartyName` | Human labels | Authored catalogs/current identity owner | VERIFY safe fallback and locale behavior | Never use labels for authorization or dedupe |
| `createdDay` / `updatedDay` | Canonical campaign-day timestamps | Source owner and campaign clock | VERIFY that state owner stores them and day ordering is stable | No filesystem timestamp or wall clock |
| `version` | Persisted DTO schema version | Selected canonical owner’s serializer | VERIFY owner convention; House must not version independently if it stores no DTO | No guessed version default for future state |

If an approved design ultimately needs an immutable consignment record, the source owner must name the minimum sufficient fields and own status transitions. The House may store only an opaque pointer when the source confirms identity durability and deletion behavior. Copying quote terms into a House-owned save object creates divergent truth even if the copy is described as a cache.

## Appendix Y — Per-Owner Conservation and Attribution Limits

The conservation equations in Appendix P are conditions to validate inside the canonical settlement owner, not equations for House to enforce. This appendix makes explicit which quantities are visible in audited owners and which are not.

| Owner/action | Observed before/after resources | Conservation proof supported by present evidence | Missing quantity / owner boundary |
|---|---|---|---|
| Market `Buy`/`Sell`/`Barter` | Market history, pressure, events | No wallet/inventory conservation proof because audited methods do not mutate those balances | Any actual exchange must be another owner; linkage/deduplication unproven |
| Caravan atomic commit | Commit record keyed by quote ID | Only one record per accepted quote ID is proven in its own state/tests | No resource legs mutate here |
| Caravan network barter | Player inventory and arrived manifest stock are mutated; requested items granted; faction progress/event updated | Normal path appears to exchange the two item legs | Fee/funds leg, exception after first mutation, durable action receipt need verification |
| Holdfast direct trade | Trader stock/value/inventory and optional funds through funds-aware methods | Existing tests target successful buy/sell and insufficient-funds refusal | Exception atomicity and operation receipt absent from inspected session DTO |
| Black-market coordinated settlement | Source stock, wallet, inventory callbacks; rollback indicated for stock on callback failure | Existing focused tests target stock/wallet/inventory conservation and compensation | Exact crash consistency and unique receipt need source-level verification |
| Trade route execution | Aggregate success/failure counters and route state | Counts can be restored with owner save path | Per-run physical stock/funds legs and receipts were not proven by inspected owner DTO |
| Trade credit | Debt contract and coordinator lifecycle | Owner tests cover acceptance/compensation and debt state | Not a consignment escrow; House-to-debt mapping forbidden |

The UI must never “repair” a conservation mismatch by changing the other owner’s counter or adding a compensating inventory mutation. If a source returns success but observed resource balances disagree, capture the source/result and fail the acceptance gate; the path owner investigates atomicity. If a save restore loses one side of a multi-owner settlement, that is a cross-owner recovery defect and must be owned by the transaction integrator. House-level logs can aid diagnostics only if they do not become an alternate transaction record.

## Appendix Z — State Transition Provenance Checklist

Use this checklist for every proposed lifecycle label in the future UI. It prevents status terms from being inferred from partial source facts.

| Proposed label | Required source event or state | Current proof | Decision pending |
|---|---|---|---|
| Draft request | Local editable form exists | Presentation-only; no source command yet | No persistence implied |
| Quoted | Exact owner returned terms with quote key/expiry | Owner-specific; not universal across all sources | Verify whether preview is pure |
| Accepted | Owner accepted command and returned a stable record/status | Present for some source commands; no universal consignment API | Never map quote commit automatically to accepted delivery |
| In transit | Canonical route/manifest owner has an explicit in-transit status | Route cadence/contracts exist, but per-consignment status not proven | Do not infer from existence of route |
| Delivered | Source reports actual leg completion with stable receipt | Caravan network emits completion event for barter; durable per-action lookup unproven | Source-bound only; no cross-owner aggregate |
| Settled | Source confirms all resource legs and receipt | Black market coordinated settlement has coupled path; receipt durability VERIFY | Not equivalent to ledger row or quote commit |
| Refused | Owner rejects before mutation and exposes reason | Multiple owners return refusal/result; uncertainty depends on source | Distinguish refusal from exception/unknown |
| Cancelled | Canonical owner accepted cancellation and returned final status | No universal cancellation API evidenced | Hide cancel action until owner exists |
| Disputed | Canonical dispute system establishes a case | None in the audited consignment owners | Do not add a House dispute status as authority |
| Expired | Owner clock/status reports expiry | Quote/contract-specific only | Do not expire from panel timer |

Every status displayed in a list should retain provenance: owner type, raw source status/code, and read timestamp/day if the source supplies one. Translation to readable wording occurs at the view edge. A stale cached row should show its age or refresh before action; it cannot be treated as a current command precondition.

## Appendix AA — Consignment Review Outputs and Stop Gates

A future engineering review should produce concrete artifacts before implementation: one verified owner contract card per supported command, a state transition diagram taken from source enums/events, a request field list agreed with the transaction owner, a call-path trace from user action through host to canonical owner, a save/restore table, and focused acceptance cases. This draft intentionally supplies questions and candidate cases, not fabricated answers.

### AA.1 What a ready proposal must name

| Review item | Required exact answer | Evidence expected |
|---|---|---|
| Supported source | Which existing owner handles this kind of exchange? | Current type/file, registration and live call site |
| Command | Which public operation is called, with which canonical input type? | Method signature and caller validation path |
| Quote semantics | Whether preview is pure, whether it reserves stock, and expiry boundary | Implementation and focused owner test |
| Commit gate | Which exact owner revalidates stock, actor, funds, route and item constraints? | Current commit method/call tree |
| Idempotency | Stable key namespace and behavior for duplicate/replayed command | DTO/API plus duplicate/reload tests |
| Atomicity | Which owner mutates each leg, in what order, and how partial failure is compensated | Mutation order and failure injection tests |
| Save recovery | Which save stores carry each committed fact and when they capture | Host save registration/capture/restore path |
| Result truth | Which return/event/status proves quote, commit, delivery and settlement | Source result type and event call sites |
| Host lifecycle | How stale UI callbacks are invalidated on campaign replacement | Existing session lifecycle and one focused route test |
| Stop/rollback | How to disable path without losing canonical state | Reversible route removal or owner migration plan |

If any of the command, idempotency, or save-recovery answers is “the House will keep a copy,” the proposal is not ready: it has moved authority. Ask the canonical owner’s integrator for a supported extension or reduce scope to navigation. If the command uses an existing general-purpose owner that lacks a required guarantee, specify the owner change in that owner’s own plan and claim; TH-2 cannot smuggle that change into a UI feature.

### AA.2 Staged acceptance by interaction mode

**Navigation-only mode** is acceptable when the House shows authored charter copy, lists only source rows supported by read APIs, and opens the canonical trader/route/contract panel. Tests need prove route resolution and lifecycle only. It must not imply consignment acceptance.

**Forwarded-command mode** requires explicit owner confirmation that the host may call the command, source-side validation, a stable action key or proven safe duplicate semantics, truthful result mapping, and a focused test that confirms one action reaches the intended owner exactly once. If no stable receipt exists, the UI must not offer blind retry after an uncertain result.

**Durable-reference mode** requires a signed choice of existing save owner to store opaque references, stable source keys, source deletion behavior, old-save defaults, retention policy and migration. The pointer collection cannot store item quantities, value, status or counterparties as an alternate contract. Tests must prove pointers survive restore while missing/deleted source records render unavailable and do not mutate anything.

**Escrow/consignment-contract mode** is out of scope for this draft. It requires a separately approved canonical settlement owner with resource reservation, release/cancel/dispute behavior, deterministic timing, multi-owner rollback/recovery and save schema. A label change or UI workflow cannot stand in for these contracts.

## Appendix AB — End-to-End Acceptance Scenarios for a Later Host Test

These scenarios should be reduced to the smallest focused tests that cover the active feature mode; they are not a request to add all rows as tests now.

| Scenario | Setup | Action | Required assertions | Must not assert |
|---|---|---|---|---|
| Navigation to existing owner | Real host composition binds the existing trader panel and owner | Activate “Open trader” from House | Same session is shown; existing close/back returns to House or caller route | New trader/session created by House |
| Valid forwarded operation | Canonical source owner has valid quote/stock and a stable request key | Confirm once | Owner reports accepted; canonical before/after state matches owner result; one call only | House independently edits wallet/stock |
| Stale preview | Source state changes between preview and confirm | Confirm old preview | Owner refusal is shown; new preview required | House reuses old value or retries with changed quantity silently |
| Duplicate request | Source already has accepted key | Submit same canonical key | Prior result is queried/returned or explicit duplicate refusal; no second mutation | Minted alternate key as “retry” |
| Save after success | Operation completed through source | Capture, reload, reopen source/House | Owner’s documented fields restore; House displays only durable source fact | Reconstruct transaction from UI memory |
| Save before command | Preview exists but not accepted | Save/reload | No accepted transaction appears; preview expiry follows source policy | House treats saved local form as escrow |
| Missing source on restore | Owner missing or old save omits section | Open House | Read surface marks unavailable and action disabled | Construct default owner and let it trade |
| Partial source exception | First resource leg mutates, later leg fails in injected owner test | Trigger action | Source recovery/compensation behavior proven; House reports exact resulting state | Generic “failed, nothing changed” message without state proof |
| Campaign replaced mid-request | Old callback resolves after new campaign binding | Complete old async result | Old generation ignored; new owner balances unchanged | Apply old result to new campaign UI |
| Unsupported mode | User chooses source without proven command | Submit/open | Explicit unsupported state or navigation to canonical surface | Silent fallback to market or contract board |

Host tests should use the real composition where needed to prove source identity, then narrow fakes for isolated result translation. A fake that returns “accepted” cannot prove save ownership or transaction conservation. Conversely, a full campaign playthrough is unnecessary if one route test and one owner contract test cover the behavior.

## Appendix AC — Worked Source-Owner Outcome Examples

The examples below use descriptive placeholders (`item_A`, `counterparty_B`, `quote_Q`) so they do not invent current catalog IDs or API fields. Their purpose is to demonstrate the UI result language and recovery path using the audited distinctions.

### AC.1 Holdfast sale succeeds, then the app restarts

1. The player opens an existing Holdfast trade session and selects quantity `q` of `item_A` for the current merchant. The House is not holding the goods or calculating value.
2. The active session validates the operation using its current stock/value and the funds-aware path where applicable. It returns an accepted `HoldfastTradeResult`.
3. The host displays that source result once, with the current trader and exact operation direction. If the result API has no durable receipt key, do not create one in the House view.
4. The game saves and restarts. `HoldfastTradeSaveStore` restores its documented `value`, `held`, and `stock` fields, and `FundsLedger` may restore bounded movement history as its owner specifies. The House must not promise that a line-item history will reappear because the inspected session DTO lacks per-operation receipt history.
5. After restart, the player reopens the canonical merchant to inspect current state. If the merchant state agrees, that is current-state evidence; it is not proof of which exact click changed it. The House must avoid backfilling a consignment receipt from the difference.

**Outcome wording:** “The trader session reports the current stock and value. This screen does not retain an operation receipt for the earlier sale.” If the owner later exposes a stable history API, replace this phrasing only after restore behavior is verified.

### AC.2 Holdfast sale is refused for insufficient funds on requested goods

1. Player requests an operation whose requested leg would exceed current wallet balance or inventory/capacity limits.
2. The owner returns its refusal result. Existing focused tests cover insufficient-funds behavior; exact failure code and no-partial-mutation assertions should be read before mapping text.
3. The House displays that refusal and allows the user to reopen/refresh the canonical quote. It does not call `TradeCreditCoordinator` to create an offer, because credit is a distinct explicit owner transaction.
4. If the player later receives funds, the old quote is not reused unless the owner explicitly says it remains valid. Request a fresh quote and let the owner revalidate.

**Outcome wording:** “This trader refused the request: insufficient funds.” Do not say “consignment accepted but awaiting payment.” No goods should be described as held unless the source state confirms a reservation.

### AC.3 Caravan atomic quote commit is mistaken for delivery

1. Existing quote `quote_Q` is committed through `CaravanAtomicTrader`; its state appends the accepted commit record. Duplicate `quote_Q` is rejected in the inspected implementation/tests.
2. The application crashes immediately after the event or before any separate network barter executes.
3. On restore, the atomic trader can restore its commit record, but the inspected owner does not move goods or funds and does not prove a separate network receipt.
4. The view labels the state “quote committed.” It does not show “in transit,” “delivered,” “paid,” or “settled.” The separate caravan network owner must be queried by its own key, if it supports that lookup.
5. If the network has no stable barter receipt, the application cannot safely replay a delivery call after restart. It should present an unresolved source outcome or return the player to the network’s existing owner flow for manual inspection.

**Outcome wording:** “The quote commit is recorded. Delivery and settlement have not been confirmed by this record.” This distinction holds even if the record amount equals the expected value.

### AC.4 Caravan network barter mutates and then throws

Assume an injected failure occurs after the caravan network removes offered player inventory or decrements manifest stock, but before it grants the requested items. The inspected path performs sequential mutations and does not show an obvious compensating rollback around every operation. This is a source-owner atomicity concern, not an occasion for the House to restore a guessed quantity.

The source-focused test must inject failure at each mutation boundary, capture player inventory and manifest before/after, and establish whether the call is atomic or has a recoverable partial result. Until that test and implementation contract exist, a thrown exception is `OutcomeUnknown`; it cannot be translated into a clean refusal. The UI should block same-key retry if one exists, preserve diagnostic context and direct recovery to the source owner. If no idempotency key exists, do not manufacture a duplicate key and resubmit.

### AC.5 Black-market commit succeeds but response is lost

1. The canonical settlement service stages source stock and invokes its coordinated callback.
2. Wallet/inventory and stock mutations complete; the application fails before the UI receives the final result.
3. The service’s normal callback-failure path can roll stock back when the callback refuses or throws, but that does not alone prove recovery for a failure after all owners committed and before response delivery.
4. On restart, inspect the source service, wallet, inventory and stock save owners. If the service exposes a stable request ID that resolves the prior result, return that result; if not, the state may be observable but cannot be linked confidently to the lost request.
5. Do not retry automatically just because the panel did not receive success. A second accepted action could duplicate settlement.

**Required gate:** A focused owner test must distinguish failure before mutation, failure during compensation, and response loss after full commit. If the existing service has no durable receipt lookup, the safe interface is to stop and show that the outcome requires owner inspection.

### AC.6 Saved pointer resolves to a stale or deleted source record

Suppose a later approved feature stores an opaque pointer to a source record. After a version upgrade, the source owner prunes old history or changes its key namespace. On load, resolve the pointer against the current owner. If it returns missing, show “source record unavailable” while preserving the pointer only if the selected owner’s retention rules permit it. Never deserialize copied quantity/value/status from the pointer and present it as live state. If the source key format changed, use only the source owner’s migration function; no House alias table should be invented.

### AC.7 A player tries a source named in stale production documentation

The player sees “regional export consignment” in an older design note and expects `ProductionTradeFlowSystem`. The current bounded tree search did not find the named source/catalog/test files, so the UI must not expose an action that calls a type presumed from `PRODUCTION_TRADE_FLOW.md`. Search by live concepts at implementation time. If an actual alternate owner is found, route to it only after verifying current registration, transaction semantics and save path. If no owner is found, return `UnsupportedSource`; never fall back to `MarketSystem.Barter`, whose audited behavior records market legs without inventory/funds movement.

## Appendix AD — Ambiguous Outcome Resolution Walkthrough

An ambiguous response is the most dangerous retry case because the UI knows less than the owner. Use the following order whenever a command throws, times out, loses its host callback, or crosses campaign replacement during commit:

1. **Freeze the original request identity.** Retain the same source-issued quote/action key in transient diagnostics if one exists. Do not generate a new key to make the request pass.
2. **Determine dispatch certainty.** If the host proves the call never reached the owner, the user may retry after a fresh preview. If it may have reached the owner, continue as unknown.
3. **Resolve against the same source instance.** Query its receipt/result API by stable key after restore completion. Do not query a similarly named source or a new default instance.
4. **Interpret only source results.** `Accepted`/receipt may prove completion as defined by that owner; `Duplicate` may mean prior action exists; `NotFound` proves no action only if owner guarantees complete retention and lookup consistency. Otherwise it is unresolved.
5. **Check resource owners only as corroboration.** Balances and inventories can show current state but rarely identify the causative operation. Never infer a receipt from balance deltas alone.
6. **Stop on unresolved state.** Disable retry/cancel controls that lack owner semantics, report the owner and unresolved request key, and follow the owner’s recovery workflow.

| Lookup answer | Safe interpretation | Next UI action |
|---|---|---|
| Receipt exists and says completed | Owner-defined completion is confirmed | Display receipt and owner route |
| Receipt exists and says refused | Owner-defined no-commit refusal is confirmed only if documented | Display refusal; fresh preview for a new user action |
| Duplicate key, prior result query supported | Existing result is recoverable | Show prior result; do not resubmit |
| Key not found with authoritative complete lookup | No recorded action for key, per owner contract | Offer a fresh request; never reuse stale quote blindly |
| Key not found but history/retention is bounded | Absence is inconclusive | Remain unresolved; no retry |
| Owner unavailable/not restored | No evidence either way | Wait for owner-ready signal or stop session |
| Receipt conflicts with resource snapshots | Cross-owner consistency issue | Preserve evidence; stop automated repair and escalate |
