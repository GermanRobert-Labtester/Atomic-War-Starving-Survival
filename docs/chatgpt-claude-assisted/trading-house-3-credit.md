# TH-3 — Credit Exposure and Settlement

STATUS: DRAFT — proposal for review; depends on TH-1 and TH-2; no ownership claim, approval, or implementation authorization.

## 1. Objective

Give the Trading House a truthful view of credit obligations that its members can actually verify, and make a credit request available only where a canonical credit owner can accept it without changing the meaning of existing debt. The smallest useful first outcome is an exposure readout that points to its source contract. A request or repayment action is a gated follow-on, not an assumed capability.

This is one bounded economy integration, not a new loan product. It adds no currency, debt schedule, borrower identity, interest calculator, collection logic, collateral inventory, or credit score. It does not fold obligations from unlike systems into a fabricated grand total.

### Completion standard

The plan is ready to implement only after each P0 premise below is closed with a named public API, owner, save path, test target, and claimed path list. If identity or unit compatibility cannot be established, complete a read-only view for the proven source or stop the package. A green compile alone cannot close an ownership or transaction gap.

## 2. Current reality

The current tree has several adjacent but distinct credit authorities:

| Authority | Verified responsibility | State and current boundary |
|---|---|---|
| `LedgerDebtSystem` | Signed shelter debt contracts, two readings before signature, payment, forgiveness, renegotiation, weather delay, and forfeits | State contains open and closed contracts; `GetContract(debtorId)` is debtor-keyed and returns one contract. Contract principal is a float/item-like quantity, not proven equivalent to wallet chits. |
| `TradeCreditCoordinator` | Catalog-driven offer and acceptance after a failed trade; rechecks standing, existing debt, embargo, template activity; compensates principal item grant if sign fails | Composes ledger, catalog, embargo, campaign day, and item-grant callbacks. The caller supplies one fixed debtor ID. This is a coordinator, not an additional debt store. |
| `LoanSharkEnforcerEngine` | High-risk loan issue, daily compounding, delinquency/default, sanctions, bounty and repayment behavior | Owns `LoanDebtRecord` rows and can interact with `FundsLedger`, bounty state, and `LedgerDebtSystem`; exposed through its own `loan_shark` save section and day tick. Its `DebtId` and chits do not prove linkage to a signed ledger contract. |
| `BlackMarketSystem` + `BlackMarketSettlementService` | Syndicate loans and partial repayment; black-market buy/sell settlement | `BlackMarketState` itself includes underworld debts, syndicate identity, status, due day, interest basis points, fired event keys, and stock. The inspected debt record has no explicit borrower/member ID; `debtId` and `syndicateId` are present. The stateless settlement service composes this policy with `HoldfastTradeSession` funds and canonical inventory. This is a distinct saved owner under `black_market`. |
| `FundsLedger` / `HoldfastTradeSession` | Funds balance and movements / Holdfast trade value settlement | Do not infer that every debt amount is denominated in this balance. Movement history and wallet balance remain canonical in their owners. |

Consequently, “total debt” is not a supported number until the plan can define borrower equivalence, currency/unit conversion, duplicate-link semantics, and status mapping. An item principal of 4 filters, a balance of 50 chits, and a black-market settlement of 50 value units are three different facts. A UI sum would be false precision.

## 3. Required delta

Current owners can issue and manage credit in their specific contexts. The proposed delta is narrower:

1. Expose only the owner-backed obligations relevant to a House member.
2. Preserve source IDs and source status so an entry remains auditable.
3. Allow House-originated request/repayment only when a current owner supports the correct borrower identity and transaction semantics.
4. Let source owners continue to control aging, interest, due dates, eligibility, default and save/restore.

The House is a coordinating view and command router. It is not a second ledger.

## 4. Evidence and duplicate check

### Direct source evidence inspected

- `Assets/Ashfall.Core/Economy/TradeCreditCoordinator.cs`: `TryBuildCreditOffer`, `TryAcceptCredit`, same-creditor unpaid-debt gate, template-day and embargo/standing checks, two-reading ceremony, and compensating grant/sign behavior.
- `Assets/Ashfall.Core/LedgerDebtSystem.cs`: canonical `Contracts`, `ClosedContracts`, debtor lookup, `PresentContract`, `PayContract`, `CaptureState` and `RestoreState`.
- `src/Main.DebtCredit.cs`: live coordinator composition with the expansion-owned ledger/catalog/embargoes, player survivor debtor ID, campaign-day provider, canonical inventory grant/revoke delegates, and phase-4 `DebtLedgerDayOwner` tick.
- `Assets/Ashfall.Core/Economy/LoanSharkEnforcerEngine.cs`, `src/Main.LoanShark.cs`, and `src/Host/LoanSharkHostSession.cs`: separate debt rows, state capture/restore, dirty tracking, `loan_shark` save section and host tick entry point.
- `Assets/Ashfall.Core/Economy/BlackMarketSystem.cs` and `BlackMarketSettlementService.cs`: separate persisted underworld-debt state; side-effect-free previews and composed loan/repayment commands; wallet and inventory mutation remains with canonical owners.
- `src/Main.BlackMarket.cs` and `src/Host/BlackMarketHostSession.cs`: current black-market save and live bindings to market, trade wallet, inventory, item catalog and campaign day.
- `Ashfall.Core.Tests/Economy/TradeCreditCoordinatorTests.cs`, `FundsLedgerTests.cs`, `LoanSharkEnforcerEngineTests.cs`, `BlackMarketSettlementTests`/`Plan211BlackMarketSettlementTests.cs`, and `Plan155BlackMarketIntegrationTests.cs` exist as likely scoped targets. Recheck exact test paths and current test names at implementation time.
- `docs/plans/integrated/economy/INTEGRATED_PLAN_155_BLACK_MARKET.md`, `docs/plans/integrated/economy/INTEGRATED_PLAN_ECONOMY-LEDGER-TRUTH-96.md`, and `docs/plans/EXPANSION_PROGRAM_WAVE9_2026-09-21/PLAN-CONTRACT-BOARD-109.md` provide nearby implementation/coordination context. They are evidence of related work, not authority to duplicate it.

### Duplicate and overlap findings

The canonical structures already include credit, funds movements, underworld debt, loan-shark debt, and contract-board escrow proposals. Search found no existing plan titled “The Trading House” or TH-3 in `docs/` at the time of the parent task’s duplicate sweep; current economy plans must still be rechecked before implementation. The archived Black Market and Economy Ledger Truth plans are completed authority for their narrow systems. Plan 109 owns its contract-board/escrow proposal and must not be silently subsumed here. The Long Line: Freight and Underworld proposals named by the draft index need exact-path/source verification if reopened; their absence from the immediate `docs/plans` filename search does not prove they are absent from `.ai/plans/` or an archive.

### P0 VERIFY — premises blocking action design

1. **Borrower identity:** confirm whether a House member ID can map to the exact `LedgerDebtSystem` debtor ID or LoanShark debtor ID without creating aliases that merge people. The inspected black-market debt record has no explicit debtor/member field; establish the intended borrower scope from current callers and data. `TradeCreditCoordinator` in the current live host is composed for the player survivor, not a general House.
2. **Credit owner selection:** enumerate all public issue/repay verbs in current APIs and determine which one is eligible for this feature. A House may not issue through `LoanSharkEnforcerEngine` merely because it can create a loan row.
3. **Cross-owner link:** establish whether loan-shark `DebtId`, underworld `debtId`, and ledger `DebtContract` have stable, unique reciprocal references. No compatibility is assumed.
4. **Denomination:** verify whether each amount is an item principal, funds chits, Holdfast trade value, or another unit; do not sum or convert until an existing canonical conversion API proves it.
5. **Idempotency:** determine how a retried House command is rejected after save/load. Existing coordinator re-entry behavior is gated by creditor/debtor debt state, but it is not a documented arbitrary request-key API.
6. **Save composition:** trace live `SaveSectionRegistry`, capture/restore, dirty flush, reset, and migration behavior for each selected authority. Do not add a `trading_house_credit` section just to cache a projection.
7. **Creditor and sanctions semantics:** verify whether House participation changes faction, embargo, blacklist, bounty, labor-forfeit, or underworld heat behavior. It must not bypass the owner’s current consequences.
8. **Current plan/claim collision:** read current `INTEGRATION_PLANS.md` and `WORKTREE_OWNERSHIP.md` and claim only after the package is assigned. The evidence in this draft is a snapshot.

## 5. Existing extension seams

- `LedgerDebtSystem.Contracts` and `.ClosedContracts` are the read surface for that owner. Read them as source records and derive a view; do not mirror them into a saved House ledger.
- `TradeCreditCoordinator.TryBuildCreditOffer` and `TryAcceptCredit` are the existing offer/acceptance path. Reuse only when House request context truly matches their contract: creditor, item principal, active template, standing, embargo, debtor, and ceremony.
- `BlackMarketSettlementService` has preview/execute pairs for its own loan and repayment operations. Keep its black-market pricing and settlement policy there.
- `LoanSharkHostSession` is the adapter/save seam for high-risk chits loans. It is not a generic House-credit service.
- TH-1’s proposed derived House ledger and TH-2’s source references are prerequisites. TH-3 may append a typed source reference only through TH-1’s owning contract; it does not create a new transaction ID system.

No new public abstraction is justified until an actual second consumer requires it. If a small adapter is proposed, it should only translate a House command into one existing owner’s exact API and return that owner’s result unchanged.

## 6. Proposed architecture

### Default first slice: derived exposure projection

Build an ephemeral query result from exactly the confirmed source owners. Each row contains source-system key, stable source ID, original debtor/member ID, creditor ID, owner-provided principal/balance and unit label, due day when supplied, owner status, and a route to the existing action. Rows must not be persisted, normalized into another owner’s status enum, or recalculated from prose.

If an owner lacks a stable row identity or public read surface, omit that source with an unavailable reason. Do not make a best-effort duplicate by reading private serialized DTOs.

### Conditional second slice: owner-routed command

For an actionable source, a host command may call the canonical preview and execute methods. The preview is advisory; the owner must repeat eligibility checks at mutation time. The UI may show a confirmation summary based on the preview but may not cache approval or make the loan decision. No multi-owner “one click” transaction is planned. If a request would mutate credit owner, wallet, inventory and House state with no existing rollback-safe boundary, stop and request an integrator decision.

### Explicitly rejected architecture

- no `HouseDebtLedger`, House wallet, escrow, balance projection cache, master `DebtRecord`, extra daily tick, or interest snapshot;
- no reimplementation of `LedgerDebtSystem`, black-market loan policy, or Plan 96 loan-shark terms;
- no total-number arithmetic across item quantities, chits, and trade-value units;
- no direct manipulation of DTOs or `LedgerDebtSystem.State` to synthesize contracts;
- no side effect hidden behind panel open, `Preview`, or read-model construction.

## 7. Ownership matrix

| Concern | Sole owner | House responsibility |
|---|---|---|
| Signed shelter debt, forfeit, payment, forgiveness and weather pause | `LedgerDebtSystem` + its existing host composition | Display referenced facts; route eligible owner commands if P0 closes |
| Catalog credit offer eligibility and acceptance | `TradeCreditCoordinator` + debt catalog/embargo/faction owners | Request/confirm only through existing coordinator contract |
| Black-market loans, their status and repayments | `BlackMarketSystem` | Link/query; route through `BlackMarketSettlementService` when permitted |
| High-risk chits loan, interest and escalation | `LoanSharkEnforcerEngine` | Show its own records; do not create a second borrower/credit authority |
| Funds balance and movement | `FundsLedger`/`HoldfastTradeSession`, as applicable | Never cache or reconcile by independent arithmetic |
| Inventory principal | shared `Inventory` | Display owner receipt; do not create collateral or inventory mirror |
| House identity/membership | TH-1 owner, still P0 | Supply the source member key only; no debt ownership migration |
| UI projection | Godot presenter/adapter in `src/` | Format owner facts; no gameplay decision or tick |

If two sources must be represented together, the view keeps their rows separate and labels their source and unit. The read model is disposable and computed on demand.

## 8. Data flow

### Read path

`existing source save restore → canonical owner state → query by confirmed debtor/member mapping → derived TH credit rows → host/presenter → player`

The query must be side-effect-free. Sorting is deterministic: source key ordinal, creditor ID ordinal, then source record ID ordinal. It must not depend on hash/dictionary enumeration. The ordering is presentational, not a new status priority.

### Request path, if P0 permits

`player action → validate House permission → ask canonical owner preview/offer → present owner-authored terms → player confirms → owner repeats gates and mutates → receive owner result/event → mark existing owner dirty/save through its current path → refresh derived rows`

If an owner does not expose a preview, its current documented command contract must be reviewed before the House calls it. House logic must not simulate a preview from partial private facts.

### Repayment path, if P0 permits

`player chooses source row → source adapter calls canonical repayment preview → player confirms exact source amount/unit → source owner composes wallet/inventory mutation → source owner emits result → existing save owner captures it → projection refreshes`

The House cannot combine payments across debts or silently pick a debt order.

## 9. State model and invariants

The default design has no new persistent state. A row is a read model with:

- `source_owner_id` and `source_record_id` (both required; source record ID must be stable across save/load);
- source debtor/member reference, creditor reference, status text/key as supplied or mapped through an explicit source adapter;
- source-owned amount, denomination key, issue/due day when public;
- action capability (`read_only`, `repay`, `request`), derived from owner availability and permission rather than saved;
- unavailable reason when no current source command is safe.

These fields are a contract sketch, not approved DTO names. The implementation should reuse existing view DTOs if suitable. Do not persist duplicate amount, status, elapsed days, eligibility, or member mapping. If TH-1’s membership is persisted, that owner alone determines membership; TH-3 does not add a second copy.

Required invariants:

1. Every visible obligation has exactly one canonical owner and traceable source ID.
2. An obligation cannot appear twice under the same `(source_owner_id, source_record_id)` key.
3. A displayed amount preserves the source unit and precision; no lossy conversion or cross-owner addition.
4. A missing or corrupt owner produces “unavailable” state, never a zero-balance claim.
5. Querying does not mutate the source, tick time, grant principal, emit a debt event, or save.
6. Closed/paid/forgiven/defaulted status comes from the source, with no reclassification by TH rules.
7. A stale preview cannot authorize mutation; the owner rechecks.

## 10. API and contract sketch

Names below are conceptual only. P0 must map them to current APIs before a coder proposes new types.

| Operation | Preferred contract | Not allowed |
|---|---|---|
| enumerate | Read from a public owner collection/query and project source keys | Read private JSON files or clone DTOs to invent a common debt store |
| request | Call `TradeCreditCoordinator` or another specifically selected canonical owner after eligibility mapping | Call multiple credit engines until one accepts, or create debt by direct state writes |
| repay | Call source `PreviewRepay` then source `Repay`/equivalent for the same ID | Deduct funds and mutate debt separately without the existing composed transaction |
| refresh | Subscribe to source `StateChanged`/owner events where available; also refresh after command result | Add an hourly/day tick that scans and mutates obligations |
| unavailability | Return a stable reason code and source owner identity | Fall back to another owner with different terms |

Any new adapter should be stateless. It may own a source registry only if TH-1 already owns that composition and the registry holds references, not gameplay state. Avoid broad interfaces whose only consumer is this feature.

## 11. Data changes

Expected default: none. Existing debt templates remain the sole authored source for `TradeCreditCoordinator` offer terms. The black-market inventory catalog remains the black-market stock catalog. No “House loan” rows should be authored until a signed design selects an owner and identifies a compatible borrower, currency, terms model, and failure consequences.

If a catalog change is later proposed, implementation must identify its loader, schema/version expectations, consumer, stable IDs, integrity-validator rule, default behavior for legacy files, and focused catalog test. No field in `debt_templates` may be reinterpreted for House behavior without that consumer audit. No data duplication in TH-1’s ledger.

## 12. Save/load and migration

### Default projection

No save DTO, section key, migration, checksum, or restore routine is added. Existing debt owners continue to capture/restore their own state. Reopening the House rebuilds rows from restored sources; no stale cache survives.

### Conditional command wiring

The command must use existing owner `CaptureState`/`RestoreState`, dirty tracking, save-section registry key, checksummed store, and day owner unchanged. An adapter must not capture the same owner twice under a House section. The acceptance round trip verifies the source record, wallet/inventory effect where applicable, and source status all agree after save/reload.

No migration is authorized. If the design requires changing an owner save schema or migrating old debt records to a House member identity, stop and request a separate explicit schema/migration plan. In particular, do not infer a member from array order, current UI selection, current player, or creditor name.

## 13. Determinism

The read projection needs no RNG. It sorts by stable ordinal keys and formats numeric data with invariant culture. It uses source days and statuses verbatim. No wall clock, random tie-breaker, hash iteration, display text, locale-specific decimal conversion, or recomputed compound interest is permitted.

If a future credit offer uses an existing authored catalog, preserve that owner’s deterministic catalog order and RNG behavior. TH-3 must not add a second draw, random ID, wall-clock expiry, or day calculation. Query and preview calls must leave seeded sequences and owner state unchanged.

## 14. System and event wiring

Use owner state changes/events already available. The ledger exposes contract lifecycle events and state-change signals; black-market composition publishes completed action results/state changes; the loan-shark session raises state changes. Verify exact event names and subscription lifecycle at package start. Subscribe once when House presenter/session opens or is composed, unsubscribe at reset/teardown, and refresh after the owner command result. Do not wire a duplicate day tick.

Ledger debt already ages under `DebtLedgerDayOwner`; black-market has its own daily tick; loan shark has `TickLoanShark(day)` and its host day owner. Which clock each system uses and whether ordering is registered once must be re-audited. The House does not call any of these tick methods. Multiple source clocks may mean different day policies; show the owner’s day and document source limitations instead of forcing synchronization.

## 15. Godot host integration

Potential host touch points, pending ownership audit: TH-1 House presenter/session, current economy/contract route, and existing debt/black-market panels. The UI should expose a source-specific row, member/creditor, amount + unit, due information, status, and available canonical action. If a source cannot be safely mapped, show a clear unavailable state.

Accessibility requirements: keyboard/controller can focus each row and action; back closes through existing panel lifecycle; stale previews report a visible refusal; source updates refresh or mark the view stale; long IDs/details are readable and do not overflow. No rule or calculation lives in a panel. No panel constructs a `TradeCreditCoordinator`, mutates `LedgerDebtSystem`, calls wallet/inventory directly, or invents a retry policy.

## 16. Narrative and content integration

No new authored narrative is necessary for the first slice. Existing creditor-authored consequence summaries and source event messages are authoritative. A journal entry may be added only if its canonical event owner already provides a stable once-only event fact and the feature is approved to consume it. Never produce House flavor text that contradicts the exact interest, forfeit, due day, sanctions, or eligibility disclosed by the owner.

## 17. Failure modes and required outcomes

| Condition | Required behavior |
|---|---|
| Owner not composed or source save unavailable | Show that source as unavailable; do not infer no debt |
| Unknown debtor/member mapping | No row/action for that identity; preserve source record untouched |
| Duplicate source ID | Reject duplicate projection and surface diagnostic; do not merge balances |
| Same person appears in different credit systems | Keep separate rows and denomination labels; do not sum |
| Offer became stale after preview | Canonical owner refuses; display owner reason and refresh |
| Save/reload between preview and confirm | Rebuild preview; require confirmation again if terms changed |
| Grant or sign fails | Preserve coordinator compensation; no House-side patch or manual balance change |
| Repayment exceeds balance or funds | Source preview/command refuses; no partial House inference |
| Day is repeated or moves backward | House does not tick debt; source follows its own idempotency rules |
| Missing catalog/template | Hide request and show unavailable reason; never create generic fallback terms |
| Host reset/disposal | Detach event subscriptions; no callback to discarded UI/session |
| Unsupported status/unit | Keep source raw representation or hide row with a reason; never relabel as paid/defaulted |
| Extreme numeric values or NaN | Preserve source validation; formatted read model does not overflow or treat NaN as zero |

## 18. Test and verification strategy

This is a plan, not authorization to run tests now. At implementation, `TEST_POLICY.md` governs and all commands go through `bin/run-scoped-tests`; no full suite without the exact user phrase `RUN FULL TESTS`.

### First choose existing tests

Reconfirm exact paths and current ownership before running:

- `TradeCreditCoordinatorTests` for eligibility, stale offer, sign/grant compensation, and repeated acceptance;
- `LedgerDebtSystemTests` for signing, payment, closed state and capture/restore;
- `BlackMarketSettlementTests` and `Plan211BlackMarketSettlementTests` for composed preview/settlement, partial repayment and save behavior;
- `LoanSharkEnforcerEngineTests` and existing Plan 96 focused tests for day tick, debt stages and persistence;
- TH-specific view/adapter tests only if the feature adds a new observable contract not already asserted.

Do not add near-duplicate tests that restate owner behavior. Add only the missing House seam tests after a current equivalent search.

### Acceptance cases

| Case | Given | Action | Expected evidence |
|---|---|---|---|
| projection fidelity | one source contract with known ID, amount/unit, due day and status | query House | exact source values; no mutation; source ID carried |
| source separation | obligations in two owners and unlike denominations | query House | two source-labelled rows; no combined amount |
| no phantom zero | source is unavailable or restore fails | query House | unavailable state, not “no debt” or zero balance |
| deterministic ordering | same source state inserted in different dictionary order | query twice | identical ordered rows and invariant formatting |
| stale offer | eligible preview then standing/embargo/debt gate changes | accept | owner rejects; inventory and debt unchanged |
| one-time settlement | command accepted then same request retried | retry | owner refuses/returns existing result; no duplicated principal or payment |
| save round trip | action through owner then save/reload | query and inspect owner | exactly one source obligation and matching wallet/inventory result |
| source tick isolation | query/preview across day boundary | compare before/after | House made no tick; only canonical day owner advances debt |
| missing owner | optional owner not composed | open House | relevant source unavailable; other rows still truthful |
| disposal | subscribe, close/reset, then source changes | refresh path | no stale panel mutation or leaked callback |

No acceptance case permits reliance on a mock to prove the real host composition. Host wiring requires its focused source-level integration gate if a host route is added.

## 19. Dependency-ordered phases

### Phase 0 — premise and authority audit

**Why:** current economy is already multi-owner; API naming and apparent semantic similarity are insufficient.

**Read only:** inspect current plan queue/claims, `TradeCreditCoordinator`, ledger/debt save owner, black-market owner, funds/wallet owner, loan-shark owner, TH-1/2 contracts, and existing tests. Record actual caller, debtor identity, source ID, amount unit, day owner, save key, migration behavior, and completion event for each selected source.

**Gate:** all eight P0 items in §4 closed or scope reduced to the source with complete evidence. Exact path list claimed before edits. If no member identity/transaction seam exists, stop after documenting the read-only projection’s source coverage.

**Do not touch:** production code, data, save registry, ledger schemas, integration ledger, ownership ledger, and current source plan statuses.

### Phase 1 — projection contract

**Why:** agree on identity and traceability before host/UI.

Create or reuse the smallest ephemeral read model, with unit and source metadata; prove it is derived and side-effect-free. Do not introduce persistence.

**Gate:** duplicate identity test, source-value parity, stable ordering, no mutation, no invented totals. Existing owners’ public API is enough to read every included row.

### Phase 2 — host query wiring

**Why:** make the truthful model reachable to the House surface.

Compose references to existing owners in the existing application/session owner. Use event lifecycle already present. No new tick or save owner. Keep uncomposed sources visibly unavailable.

**Gate:** host integration test opens/query/refreshes after an owner event and proves source ID/value parity; close/reset unsubscribes.

### Phase 3 — read-only user surface

**Why:** validate the view before making money/debt commands reachable.

Render separate source rows and actions disabled unless the source adapter is proven. Use owner messages/terms and established panel lifecycle/focus/back behavior.

**Gate:** focused UI and accessibility checks demonstrate correct refresh, keyboard/controller operation, stale-state feedback, and no panel-side mutation.

### Phase 4 — one owner-routed command, only if signed off

**Why:** separate a command’s transaction proof from a view that already works.

Pick one precise owner and verb. Reuse owner preview, acceptance, repayment, save, event and idempotency. If an existing adapter is insufficient, stop for a new design decision rather than composing partial writes.

**Gate:** focused transaction and host journey proves preflight, confirmation, canonical mutation, compensation/refusal, save/load and repeated command behavior.

### Phase 5 — closeout

Update only the integration plan and state/ownership artifacts if the named foreman/integrator owns them. Keep this draft DRAFT unless explicitly approved and accepted through its governance process. Archive and mark integrated only after real integration is complete.

## 20. File impact map

The rows below are candidates, not current claims. Exact file list is a Phase 0 deliverable.

| File/area | Action | Reason | Risk |
|---|---|---|---|
| `Assets/Ashfall.Core/Economy/TradeCreditCoordinator.cs` | READ ONLY by default; MODIFY only if a signed design selects a missing reusable contract | Existing offer and acceptance seam | High: changing terms affects existing Holdfast trade |
| `Assets/Ashfall.Core/LedgerDebtSystem.cs` | READ ONLY | Canonical shelter contracts and lifecycle | High: state/semantics changes fan out to all debt users |
| `Assets/Ashfall.Core/Economy/BlackMarketSystem.cs` | READ ONLY | Existing black-market loans/debt and policy | High: changes underworld save and daily behavior |
| `Assets/Ashfall.Core/Economy/BlackMarketSettlementService.cs` | READ ONLY; use current commands if permitted | Atomic composed black-market settlement | High: funds/inventory rollback boundary |
| `Assets/Ashfall.Core/Economy/LoanSharkEnforcerEngine.cs` | READ ONLY | Existing separate loan-shark debt engine | High: interest/default and bounty consequences |
| `src/Main.DebtCredit.cs`, `src/Main.BlackMarket.cs`, `src/Main.LoanShark.cs` | READ ONLY; possible small composition change | Live host/save/day wiring evidence | High: duplicate ticks or session lifecycle |
| TH-1 House owner/presenter files | MODIFY only after TH-1 path claim | Derived source rows and UI | Medium: shared seam; needs integrator if claimed |
| Existing economy tests | READ ONLY then focused additions only for missing House-specific behavior | Use current coverage and protect selected seam | Low to medium |
| catalogs/save registry | NO CHANGE by default | No new terms or state needed | High if changed without schema/migration plan |

## 21. Risks

1. **False consolidation:** unlike units or contract semantics appear as one debt total. Mitigation: source-scoped rows and no aggregation.
2. **Wrong borrower:** House member identity is silently coerced to player survivor ID. Mitigation: P0 member identity gate; refuse actions when unmapped.
3. **Bypass:** generic request bypasses standing, embargo, signed ceremony, or black-market eligibility. Mitigation: owner-only command routing and stale-state test.
4. **Duplicate tick:** House day owner reprocesses source debts. Mitigation: no TH tick; verify existing owner registration.
5. **Split transaction:** funds, item principal and debt mutate separately. Mitigation: reuse existing rollback-safe owner boundary; stop if absent.
6. **Save fork:** House caches a second debt copy and goes stale after reload. Mitigation: ephemeral projection, no new section.
7. **Scope conflict:** Plan 109 escrow or Plan 96 debt map is altered indirectly. Mitigation: explicit cross-plan check and integrator sign-off.
8. **Prose promises unsupported by code:** “House credit” suggests a single line of credit though there are different contracts. Mitigation: label sources and terms exactly.

## 22. Out of scope

- changing creditor template terms or adding House-specific loan products;
- consolidating or refinancing loans across owners;
- universal credit score, affordability rules, interest rate negotiation, collateral, guarantors, or credit limit;
- new currency, exchange-rate conversion, wallet, fund movement log, escrow, collection clock, default, sanction, faction standing or bounty behavior;
- market price, black-market stock, caravan, route, contract-board listing, production, inventory-capacity, or trade-goods changes;
- borrower identity migration or old-save rewriting;
- new narrative content that invents owner state;
- full-suite tests, broad refactors, and cleanup unrelated to the selected source.

## 23. Rollback and recovery

Keep any implementation in separable commits/packages: projection contract, host read wiring, presentation, and—only if approved—one owner command. Revert the House adapter/presenter without touching underlying source owner state. A read model has no save migration, so rollback removes only presentation and query wiring.

For an owner mutation package, checkpoint existing owner-specific focused tests before host work. Do not roll back or rewrite debt save files to compensate for an adapter defect. Recover through the owner’s existing restore/backup system. If an additive field or migration becomes necessary, stop and produce a separately reviewed migration and recovery plan before editing that authority.

## 24. Definition of done

- Current duplicate/claim audit is recorded; exact paths belong to one package owner.
- Selected source obligations and their APIs/IDs/units/status/day/save paths are proved from live source.
- Every row is traceable, source-labelled, accurately formatted and side-effect-free.
- No grand total crosses incompatible units; missing sources never imply zero exposure.
- No new debt, credit, wallet, escrow, schedule, stock, inventory or save authority is introduced.
- If a command is approved, the existing owner is the sole mutator and its ordinary eligibility, transaction, idempotency and restore rules remain in force.
- Focused tests are run through `bin/run-scoped-tests`, with exact command/result documented; no full suite absent exact user authorization.
- Host lifecycle, save/reload, day tick isolation and accessibility are verified for touched paths.
- Integration/ownership ledgers are updated only by their authorized owner; draft status changes only through explicit plan governance.

## 25. Implementation handoff

### MUST PRESERVE

- `LedgerDebtSystem` as the authority for its signed contracts and lifecycle.
- BlackMarketSystem, LoanSharkEnforcerEngine, FundsLedger/HoldfastTradeSession and Inventory as separate owners with their current units and save paths.
- `TradeCreditCoordinator`’s catalog terms, revalidation, two-reading ceremony and compensation behavior.
- Existing day owners, checksummed save sections, deterministic ordering and UI lifecycle.

### MUST ADD

- First: evidence table mapping each included obligation to public source API, stable source ID, debtor mapping, unit, due day, status, save owner and event.
- If evidence supports it: a non-persistent source-scoped projection and thin host query/refresh seam.
- A canonical command route only after a named owner and borrower semantics are approved.

### MUST NOT DO

- Add a parallel debt/stock/funds ledger, total across unlike denominations, clone debt DTOs, mutate owner state from UI, add a second day tick, or invent cross-owner settlement behavior.
- Treat current plans or filenames as proof that a proposed API exists.

### VERIFY WITH

- Current source premise inspection; current claim/plan audit.
- Existing focused debt, black-market, loan-shark and funds tests as relevant, selected with `bin/run-scoped-tests`.
- New tests only for a missing House-specific projection/wiring/command contract.
- Save/restore and runtime route checks only where the selected command is actually connected.

### FIRST SAFE IMPLEMENTATION STEP

Perform the read-only P0 audit. Return with the exact candidate source rows and identify a single first read-only slice. Do not implement credit issuance until borrower identity, units, idempotency, and source transaction/save ownership are settled.

---

## Appendix A — Source owner contracts in greater detail

This appendix records what the inspected APIs actually guarantee so a later package does not generalize from a filename or a user-facing label. It is a planning aid; Phase 0 must still inspect current source at the commit being integrated.

### A.1 `LedgerDebtSystem`: signed document lifecycle

The debt object is a document with an explicit lifecycle. `DebtContract` carries `debtorId`, `creditorId`, `templateId`, float `principal`, `termDays`, `rate`, named `forfeit`, `readCount`, `signed`, `signedDay`, `daysRemaining`, and the `paid`, `forfeited`, and `forgiven` facts. It also carries weather pause count and most recent weather gate ID. It does not, in this inspected DTO, carry a distinct contract ID or a physical item transaction receipt. `GetContract` takes a debtor ID and returns the first matching current contract. This makes the debtor identity an important keying invariant and makes “several concurrent House obligations per person” incompatible with the present API unless a separate approved migration changes that contract model.

`PresentContract` creates or edits one current draft. It refuses invalid terms; it refuses a second draft over live signed/unpaid ink or an unpaid forfeiture; and it archives paid or forgiven current ink into `closedContracts` before starting a new draft. A first call creates/updates a draft and increments its reading count. The coordinator calls it twice, then grants item principal, then signs. `CancelDraft` removes only an unsigned draft and checks optional creditor/template IDs before removing it. `SignContract` requires at least two readings, sets `signedDay`, initializes remaining days, emits `OnContractSigned`, and raises state changed.

Those mechanics imply several House constraints:

- A House panel cannot retain an offer and assume it remains valid. It must revalidate at the action boundary.
- The same debtor cannot receive a second open contract through this owner, even if the House labels it as a distinct membership account.
- A House request must carry the exact creditor and template selected by the coordinator; it cannot replace an item principal with funds or reinterpret the rate.
- A failure after first or second reading must call the existing compensation path. A direct state edit to remove a draft is prohibited.
- The principal grant is wired to canonical inventory in `Main.DebtCredit.cs`. The written terms may describe a named item quantity, while the signed ledger later computes `TotalOwed` as `principal × (1 + rate)`. The meaning and denomination of that numeric amount relative to item quantity must be verified before any House UI labels it “balance due.”

The daily tick processes only signed contracts that are not paid, forfeited or forgiven. A route-specific weather blocker can pause the term for at most three days. After the bounded grace is exhausted, the day owner counts down; at zero the contract becomes forfeited and emits `OnForfeitTriggered`. The event signals a named forfeit obligation; it is not by itself proof that a physical item has been collected. Host consequences are routed through the existing dispatcher/bridge. House display must distinguish “forfeit due” from “forfeit collected,” and that latter fact must come from the owner that actually consumes the item or applies the consequence.

`PayContract` is a full-contract state transition; its method signature accepts `debtorId` and `day`, but the inspected method does not itself debit `FundsLedger` or consume the item principal. It changes `paid`, clears `forfeited`, emits `OnContractPaid`, and raises state changed. Forgiveness is a separate source fact and does not move payment. Therefore any House “repay” button must not call `PayContract` until the host-side action that pays/returns the specific named goods is traced and its order/compensation is understood. Treat this as a P0, not a minor UI detail.

`RenegotiateContract` permits signed changes only when term is at end and refuses paid or forfeited contracts. Contested signed renegotiation requires a fresh standing callback. Unsigned draft rewrites reset read count to zero. A House readout should use only read-only contract facts; it must not offer renegotiation as a generic credit feature without the arbitration seam and same contract command.

Capture/restore makes a defensive deep copy of open and closed contract lists. This is a useful source behavior: the House can rebuild a projection without taking an aliased reference. It does not solve the missing unique-contract-ID issue; `(debtorId, creditorId, templateId, signedDay)` might appear derivable but collision safety across repeated seasons, migration and duplicate records has not been proven. Do not present that tuple as a stable key without a current evidence-backed decision.

### A.2 `TradeCreditCoordinator`: offer versus acceptance

`CreditOffer` is explicitly a presentation/transaction projection of a valid template, never contract state. It exposes template and creditor IDs, item principal ID/quantity, term, rate, forfeit text, authored consequence summary and display name. `CreditOfferResult` carries eligibility and a reason taxonomy. `CreditAcceptResult` carries success, debtor ID and reason. These are suitable for the existing Holdfast terminal’s insufficient-funds flow; they do not describe a general House account.

Offer selection is deterministic in catalog file order. It canonicalizes the requested item and template item through `ItemAliases.ToCanonical`; among matching creditor/item entries, it takes the first template whose gates all pass. If none passes, it distinguishes no matching template from the first gate rejection. The current gates are:

1. when the faction-war dependency is bound, the creditor standing must exceed `HostileStandingThreshold`;
2. the same ledger must not contain signed, unpaid, unforgiven debt from that creditor;
3. the creditor must not be embargoed on current campaign day;
4. the failed trade item must match the template principal item after alias canonicalization;
5. the template must be active and the day inside authored min/max bounds.

If the optional faction-war dependency is null, the source code skips that standing check. The House must not exploit a partially composed coordinator to display an eligible offer; host composition must be complete, or the source offer path must be disabled with an unavailable reason. Offer creation logs the day, creditor, template, item and quantity but does not mutate debt or inventory.

Acceptance looks up the template and checks creditor identity, reruns all gates, invokes the ledger’s two-read ceremony, grants the principal through the configured callback, then signs. If item grant fails it removes the draft. If sign fails it invokes item revocation and removes the draft. The coordinator’s compensation calls are the established behavior; the House must neither wrap them in an additional speculative inventory transaction nor repeat the grant. The constructor’s revoke delegate is nullable; package audit must confirm that live composition wires both grant and revoke exactly as intended. `Main.DebtCredit.cs` currently does.

There is no arbitrary idempotency request key on `TryAcceptCredit`. Existing protection is domain-specific: the open debt gate and contract state block another unresolved debt from the same creditor/debtor. For a House with different memberships, an action retry after a partial restore could still be ambiguous. A view refresh plus explicit current owner gate is safer than inventing `house_request_id` and persisting it elsewhere. If transaction retry must be stronger than current owner semantics, stop for an integrator decision on the owner contract.

### A.3 `BlackMarketSystem`: underworld credit stays separate

The black-market owner’s own state explicitly contains the underworld ledger, loans and stock snapshots. A debt row inspected in the current source carries `debtId`, `syndicateId`, principal/repaid units, interest basis points, issued and due day, status and reason. There is no explicit debtor/member identifier in that row. The current product is therefore a syndicate-scoped facility tied to the player settlement path, not a demonstrated House-member liability record.

`PreviewLoan` is side-effect-free and returns validity, syndicate ID, requested units, duration, due day, interest basis points and reason. The source checks amount/duration and profile credit cap plus active-loan conditions. `TakeLoan` reuses the preview, creates one `UnderworldDebtRecord`, credits the wallet through the composed settlement service and returns source result facts. `PreviewRepay` bounds applied amount against outstanding balance and identifies whether that amount completes the debt. `RepayDebt` applies partial repayment and updates source debt status; the settlement service coordinates the wallet debit and source state callback.

This is materially different from `LedgerDebtSystem`: it is denominated in settlement units, references a syndicate and due date, uses an interest basis point field, permits partial repayment, and the settlement service binds it to `HoldfastTradeSession`. The black-market state uses a fired-event key ledger for debt consequences and persists stock snapshots so reopen does not reroll. House integration may link to this owner’s debt IDs and use its preview/command pair; it may not replicate the debt rows or combine their values with item debt.

If TH-1 creates a House membership roster, membership must not alter who is liable for a black-market loan. Before exposing one in a member-by-member list, verify current UI and caller semantics: the absence of a borrower ID may mean the borrower is the global player wallet. The UI should then label the source as a player/syndicate obligation and omit it from a specific survivor account rather than assign it to the currently selected member.

### A.4 `LoanSharkEnforcerEngine`: loan issuance and risk clock

`LoanDebtRecordSaveState` is a rich source record: debt ID, creditor faction, debtor string, principal/current balance in integer chits, daily interest permille, issue/due days, grace days, escalation stage, last accrual day, cumulative interest/repaid, bounty facts, raid risk and last escalation day. `ActiveDebts` excludes only settled and forgiven rows. `IssueLoan` validates IDs and positive amount/term/rate/grace, derives a debt ID from creditor, debtor, current day and list count, appends the record, then calls `FundsLedger.TryCredit` if a ledger is attached. The inspected issuance method does not check the funds result before returning the new debt. This is a significant P0 for a House command: record creation and principal disbursement are not proven atomic here.

`ProcessDailyTick` is catch-up aware: it accrues for every elapsed day after `LastInterestAccrualDay`, compounds against the current balance using integer arithmetic, floors positive interest to at least one chit, and saturates current balance at `int.MaxValue`. It then advances current → grace → delinquent → defaulted based on current day relative to due day, grace and a five-day delinquency threshold. Default event emits once on stage change; bounty placement is attempted only when a bounty owner is bound and `BountyPlaced` is false. Raid risk is derived and capped. This is a different clock and consequence policy from the flat-rate debt document.

`RepayDebt` caps the requested amount to current balance, debits the optional FundsLedger first, and only then mutates the loan. If no funds owner was supplied, the method will still reduce debt without a funds debit; the live `LoanSharkHostSession` shown in the inspected code constructs the engine with no FundsLedger/bounty/debt-system argument by default. This may be intentional for isolated tests or an incompletely bound production path. House must not call the host method until source composition proves funds and bounty dependencies are supplied. The issue/repayment methods also lack a general command idempotency key; source debt ID is the target for repayment, but issue retry behavior needs review.

The engine uses a `rollPermille` supplied to `CheckEnforcerRaidTrigger`; the House must not supply a new random roll. A daily host tick can be called with campaign day, and the session exposes `TickDay`, capture and restore. `src/Main.LoanShark.cs` loads and saves a checksummed `loan_shark` section, tracks dirty state through the session event, and calls `TickDay`; verify the actual registered day owner and dirty/save ordering before implementation. The source does not own a universal relationship to the signed document ledger; its optional `_debtSystem` field in the engine constructor is not proof of a cross-reference.

### A.5 `FundsLedger` and `HoldfastTradeSession`

`FundsLedger` stores a non-negative integer balance, bounded movement history of 128 records, reason key, source ID, result balance, and day. It rejects nonpositive transactions, insufficient debit, and overflowed credit. It captures a schema-versioned state and restores a capped movement suffix. Its movements are intentionally bounded; they are not a complete archival debt/House transaction log. A derived House view must not claim complete financial history from this collection.

The black-market settlement service’s documented pattern is a composed, rollback-safe command over black-market policy, `HoldfastTradeSession`, canonical inventory and item catalog. For buy/sell, it previews price and inventory validity, then reruns price and exact rounded settlement inside the source mutation callback while the wallet transaction applies. That is a better model to preserve than direct `TryDebit` plus later debt mutation. For credit, the `TradeCreditCoordinator` intentionally uses compensating grant/sign because inventory principal and ledger signature are separate state owners. Those are two existing transaction designs; the House should not abstract them into one fabricated “universal credit transaction.”

### A.6 Plan 109 and contract-board boundaries

Plan 109’s listing lifecycle includes accepted/fulfilled/failed/expired state, deadline, deposit, reward, penalty and funds escrow. Its stated non-goals prohibit new currency, parallel quest system and contract state in a panel; escrow is through an existing funds seam. Even if Plan 109 remains proposed or unimplemented, its concept conflicts directly with any TH implementation that creates another commitment ledger or places money under a House-owned escrow row. Phase 0 must inspect current plan status, code path, exact claim and live API. If Plan 109 is unimplemented, this draft still cannot claim its own board behavior; it must define the TH-3 credit interface as a consumer of the eventual canonical contract/board owner or stay at read-only source references.

## Appendix B — Canonical row schema and interpretation examples

The row described here is a projection, not a proposed persisted record. A concrete implementation may use a different existing DTO.

| Projection property | Source derivation | Null/unavailable behavior |
|---|---|---|
| owner key | fixed adapter constant (`ledger_debt`, `black_market`, `loan_shark`) | never inferred from creditor text |
| record key | existing stable owner key if one exists | omit actionable row if no stable key; do not hash arbitrary mutable fields |
| debtor scope | owner record or verified caller context | `unmapped` if the record has no borrower and cannot be tied safely |
| creditor | canonical creditor/syndicate/faction ID | display label may be resolved separately; keep source ID |
| principal/current amount | owner fields, never recomputed unless owner exposes canonical owed method | keep source precision and denomination |
| unit | explicit source-defined unit: item count, chits, settlement units | no amount conversion or summation without a canonical converter |
| date | signed/issued/due day from owner | do not derive due day from elapsed ticks when owner has a field |
| state | raw source status/flags translated by a total, tested mapper | unknown source state → unavailable, never silently active/paid |
| action | owner query and membership permission | disabled on absent owner, unmapped member, stale record or missing save composition |

### Example 1 — one signed shelter debt

Assume source state has one signed contract with debtor `survivor_17`, creditor `faction_waterline`, template `waterline_medical_kit`, principal quantity 2, rate 0.10, term day state 4 days remaining, and no forfeit. The offer projection may show the source template terms and the ledger’s own readout. It must not state “2 chits due” or calculate money from item quantity. If `TotalOwed("survivor_17")` returns 2.2 by its method, that represents the ledger’s float formula only; whether fractional quantity is meaningful or how to settle it with physical items is P0. The TH view should use a “terms per ledger” representation until the consuming owner defines the settlement language.

### Example 2 — same member, black-market loan

Assume one black-market debt with ID `underworld_...`, syndicate `salt_crows`, 40 units borrowed, 5 repaid, 250 basis points, and due day 100. The House may show a separate player-wallet obligation only if the black-market caller confirms that is its borrower scope. It may not add 35 units to Example 1, assign the loan to `survivor_17`, or interpret 250 basis points as the ledger document’s float rate.

### Example 3 — loan-shark default

Assume loan-shark debt ID `loan_faction_x_survivor_17_...`, 500 chits principal, balance 640, and stage `Delinquent`. This may be shown as a chits debt only after confirming the live engine has a FundsLedger wired and the player/member ID is the same debtor. It is not the same as an underworld `defaulted` status. The House can display the source stage and a source-owned “sanction applies” fact; it cannot derive the independent black-market heat or one of the ledger’s forfeiture flags.

### Example 4 — the same source referenced twice

TH-2 may create a consignment referencing a source contract; TH-3 may present that contract as exposure. They must share the source owner+source record key, not make two local IDs. Query must deduplicate on that identity, and only the owning contract may transition state. If the source cannot return a stable key, both feature rows should be non-actionable until the owner’s contract identity is clarified.

## Appendix C — Action preflight and mutation contracts

### C.1 Offer discovery sequence

1. Determine exact House actor and authorization from TH-1. A membership role cannot alter creditor standing.
2. Determine which existing transaction failed and its canonical item/creditor IDs. The existing `TradeCreditCoordinator` is explicitly keyed to a failed trade item context.
3. Ensure the live owner graph has catalog, day, embargo and faction standing dependencies; if any required dependency is missing, do not create an offer through a partial coordinator.
4. Call the owner’s side-effect-free offer builder. Display only fields returned by the template projection; no panel copy edits terms.
5. Do not persist that offer. On return to the panel or after a day change, rebuild it.

Any “view exposure” function must not call this flow implicitly: reading exposure must not create or surface debt solely because the user opened the House. Offer discovery belongs to an explicit context where the existing creditor/item relationship applies.

### C.2 Acceptance sequence and compensation audit

The known live sequence is deliberate: two `PresentContract` calls, principal grant, then `SignContract`; rollback removes draft on grant failure and revokes grant plus draft on sign failure. A package integrating this through the House must assert conservation at each boundary:

| Fault injected | Source result to prove | State after failure |
|---|---|---|
| template missing/disabled | coordinator refuses before draft | no new contract, no inventory mutation |
| standing hostile / embargo active / same creditor open | coordinator returns reason | no draft or grant |
| first contract presentation fails | acceptance refusal | no grant |
| second presentation fails | refusal and draft cleanup if a draft was made | no open unsigned residue |
| grant capacity/catalog failure | `credit_principal_transfer_failed` | draft canceled; inventory unchanged |
| sign fails after grant | `credit_sign_failed` | revoke amount; draft canceled; inventory returns to baseline |
| state change after preview | stale offer refusal | no grant, no signed contract |
| response lost after success | repeat current owner check | cannot mint second principal; source-state idempotency behavior confirmed |

Important test caveat: mocking a successful grant/revoke callback proves coordinator behavior only. It does not prove the real inventory delegate respects item aliases, capacity, quantity, or restore order. If the House invokes the current production coordinator, run the existing owner test plus one scoped host integration for the actual callback composition.

### C.3 Repayment and closure decision tree

No generic repayment API is approved. The later integrator should create a table like this with actual source code evidence:

| Source | User action phrase | Owner command | Physical/wallet mutation | Final persisted fact |
|---|---|---|---|---|
| signed ledger contract | TBD — “return named good,” “pay,” or “honor forfeit” must reflect canon | `PayContract` and any separately owned consequence callback, if proven | not performed by `PayContract` itself in inspected source | `paid=true`; forfeit cleared |
| black-market loan | “repay X settlement units” | settlement service `Repay` | coordinated wallet debit + BlackMarketSystem balance mutation | source debt status/amount updated |
| loan-shark loan | “repay X chits” | host/engine `RepayDebt` | conditional FundsLedger debit in engine | `CurrentBalanceChits`, `TotalRepaid`, possible Settled stage |

If a single command phrase cannot accurately describe all three, present owner-specific actions. Payment to the ledger contract may be a return of the named good rather than a wallet debit. Do not offer a one-button “settle all” or equal-priority repayment order.

## Appendix D — Detailed end-to-end scenarios

These are target cases for a future implementation. They model current source distinctions and describe observable proofs; they are not claims that the complete TH route already exists.

### D.1 Creditor offer from an insufficient-funds trade

**Setup:** existing Holdfast trade calls the current coordinator after a failed funds check, the failed item is aliased but maps to the catalog principal, catalog has one active template, and creditor is neutral with no embargo/open debt.

**Expected path:** canonicalize requested item; deterministic first eligible catalog template chosen; offer terms shown from `CreditOffer`; no debt or item changes until explicit confirmation; on confirm gates are re-evaluated; two readings occur; inventory callback grants principal; ledger signs on campaign day; current source records one contract and matching inventory increment.

**Proof:** existing coordinator focused tests prove gates and compensations; host integration checks actual campaign day/creditor/aliases/inventory and save; load shows exactly the source contract and inventory. The House merely links the resulting source ID according to the identity rule proven in Phase 0.

**Stop if:** the player clicks an old offer after a new day, a creditor embargo, standing change or new contract and source refuses without clear feedback; House must refresh and explain. Never “honor old quote.”

### D.2 Saved but unsigned contract draft

The existing coordinator completes two reads and normally signs within one operation. A crash or save during mutation could expose a draft if host save occurs between callbacks. The plan does not assume such a save boundary is reachable or impossible. Phase 0 must inspect whether saving can interleave with synchronous Core operations. If an unsigned draft survives restoration, it remains not debt and cannot be displayed as a signed obligation or be removed by House cleanup. Use source `CancelDraft` only through its guarded coordinator transaction on a subsequent attempt; do not delete restored state from UI.

### D.3 Shelter contract reaches a weather pause

**Setup:** signed contract; a route-specific blocking gate appears; remaining days positive; weather pause allowance below the three-day cap.

**Expected path:** `DebtLedgerDayOwner` installs route gate provider then ticks ledger. The ledger increments bounded weather delay, records gate ID, emits event, and skips countdown that day. The host writes a journal note with day, creditor, debtor and grace usage. TH should refresh the contract row and show paused/delayed using this source event/fact if the member mapping and owner state are available.

**After cap:** further days are not paused by this branch; remaining time advances. House cannot extend term again or interpret “route blocked” as indefinite payment suspension.

**Persistence proof:** save after the first delay; restore; confirm `weatherDelayDaysUsed`, `lastWeatherDelayGateId`, days remaining and journal idempotency match. TH row has no independent pause count.

### D.4 Forfeit due versus good delivered

At term expiry, `forfeited` becomes true and `OnForfeitTriggered` fires. A separate dispatcher/bridge may consume items, apply consequences, or route a bounty. These are separate writes and may have their own fired/once state. House cannot label the forfeit delivered merely because `forfeited=true`. A full implementation would add one read-only evidence row that cites the ledger contract and, if exposed, a consequence record or canonical inventory delta. If no consumer records a stable consequence receipt, display “forfeit due / consequence processing per ledger” and do not claim collection. A plan that wants a verified collection receipt must identify which current owner already stores it; adding one here is out of scope until approved.

### D.5 Debt payment after forfeit

`PayContract` allows full closure after a forfeit came due and clears the forfeit flag. The player experience requires knowing what payment entails. If the named forfeit was a physical item and has already been taken, “pay in full” might be represented by satisfying it, or the contract may require a separate goods return; code must establish canon. The House action must present exact owner language and not take the item again. Repeat click after success must be refused by the ledger as already settled, with no duplicate inventory loss.

### D.6 Forgiveness and a later new contract

Forgiveness archives the source contract only when a later `PresentContract` begins new ink; closed contracts are maintained separately. Same-creditor gate considers only current `Contracts`. A House readout that scans current only may no longer show an old forgiven contract unless TH-1 history intentionally includes closed records. Do not say “no previous debt” because the live gate is clear; display current exposure and source history separately. If House has no history owner, avoid building one. The source event and closed-contract collection remain ground truth.

### D.7 Black-market partial payment at the balance boundary

**Setup:** source loan has outstanding balance `B`; user requests `R`, where `R > B`; wallet has exactly `B` settlement units. `PreviewRepay` computes applied amount bounded by remaining exposure; settlement rounds exact owner value and checks wallet. `Repay` repeats quote and wallet/debt transaction. Expected source result is applied amount `B`, no negative debt, wallet delta `-B`, debt closed according to black-market status, source event once.

The House should show the previewed applied amount, not requested `R`, and preserve the source units. If the balance changed after preview, owner revalidation controls the final applied amount and UI shows actual result. Test below-balance, exact-balance, above-balance, zero, negative, huge, and wallet-shortfall cases in the focused owner test (avoid duplicating these in TH tests).

### D.8 Loan-shark interest catch-up across a save gap

**Setup:** loan last accrued on day `D`; save; campaign resumes at day `D+N`. The engine loops elapsed days, compounds on each day and raises interest event for each amount. `ProcessDailyTick` sets `LastInterestAccrualDay=currentDay` after processing. Verify no duplicate accrual when the same day runs again; it should skip because current day is not greater than last accrual. Verify whether host owner is registered only once so same day isn't reached twice through two different owners. House query cannot call it or trigger interest.

With zero permille, no minimum interest applies; with a positive permille whose integer division produces zero, code increments one chit minimum. With very low rates this can dominate over a long time. House displays the owner’s actual `CurrentBalanceChits` and may not calculate an alternative rate formula. On overflow, current balance saturates but `TotalInterestAccrued` integer overflow behavior should be checked; this is a risk if that field overflows. It is source owner debt, not a House patch.

### D.9 Loan-shark issue with failed wallet credit

The inspected `IssueLoan` appends a debt record before trying `FundsLedger.TryCredit`; it ignores the returned `FundsResult`. If balance would overflow or amount invalid, this code path can return a debt whose principal may not have been credited, depending on supplied ledger and call values. The current validator rejects nonpositive principal, but overflow is still possible. House integration should add a focused issue only after the owning package resolves whether production composition ever binds this funds ledger and whether issuance must be compensated. Do not add a House-side second credit to “fix” the path; that would hide the owner defect and risk double credit. This is a production code finding to report to the named integrator if the feature considers this owner.

### D.10 Multiple sources with no shared borrower

One player may hold a shelter debt under survivor ID and a black-market loan attached to the global wallet/syndicate record. A loan-shark row could have a debtor ID string equal to that survivor, but equality of strings does not prove same legal borrower or currency. The correct view is three source sections (or fewer if evidence is incomplete), not a unified borrower total. Actions appear under each source only if its own scope supports it. House member selection filters only records with proven membership mapping.

### D.11 Black market unavailable after restore

If black-market catalog load fails, `BlackMarketHostSession.Create` records a catalog rejection event; system may have empty/default catalog. The House must not infer the player has no syndicate debt because a query against an unbound/empty catalog returns no rows. The save may still restore debt state but action availability and stock policy could be unavailable. Separate read availability from command availability. If the owner state restores but catalog is missing, display only reliable saved source debts if the public API permits it, disable buy/repayment actions if settlement owner cannot quote. No substitute market is consulted.

### D.12 Optional source is not composed

Open House without LoanSharkHostSession while black market and LedgerDebtSystem are live. Expected row set contains only proven source rows; loan-shark section says unavailable, not “clear.” Close and reopen after source composition; derived view refreshes and includes records without duplicate subscriptions. This protects lazy session composition from leaking a false state.

### D.13 Data template day-window boundary

Template availability requires `day >= minDay` and, when `maxDay>0`, `day <= maxDay`; inactive false blocks. Tests should cover day-1, first day, last day, day+1, `maxDay=0`, and missing legacy fields. House cannot cache an eligibility result over a day advance. It displays no invented “offer available” based on description text.

### D.14 Alias and exact ID preservation

The coordinator canonicalizes request and principal IDs for matching, but `CreditOffer` carries the template’s authored `PrincipalItemId`. The grant callback canonicalizes again in the host. House read model should preserve both the authored template ID and canonical inventory ID only if a current item API exposes that mapping; no custom alias table. Unknown alias means request refusal, not fallback to a similar name.

## Appendix E — Failure matrix with recovery ownership

| Failure | Detecting owner | House response | Recovery owner/action | Must not |
|---|---|---|---|---|
| source collection malformed/null | source restore/validator | mark source unavailable and log diagnostic via established channel | source save owner or restore pipeline | create empty source state and announce debt-free |
| duplicate contract source key | adapter integrity check + source validator | suppress action and flag duplicate evidence | canonical debt owner / integrator | combine or renumber source contracts |
| display amount invalid or nonfinite | owner validator/read adapter | render safe unavailable text; preserve diagnostic | owner data/save contract | convert NaN to zero or clamp to plausible amount silently |
| campaign clock mismatch | host owner | mark due date uncertain, disable commands | canonical day owner / restore ordering | use wall-clock day or `_simDay` as fallback without verifying source provider |
| `TradeCreditCoordinator` lacks faction war | host composition audit | disable request; read-only source rows remain | Main composition owner | allow skip of standing gate as a feature |
| debt catalog invalid | catalog loader | no offer; show source unavailable | authored catalog/integrity owner | invent default terms |
| embargo query unavailable | host composition | no new credit request | embargo authority owner | treat null as not embargoed |
| credit principal grant rejected | coordinator | show owner refusal; refresh inventory | coordinator compensation; verify draft removed | manually retry grant |
| credit sign fails after grant | coordinator | show owner refusal; refresh both owners | revocation delegate + ledger cleanup | set `signed=true` from panel |
| black-market wallet debit fails | SettlementService/wallet | report insufficient/blocked; refresh preview | source settlement owner | lower loan repayment amount without source quote |
| black-market state changed after preview | BlackMarketSystem | show new source refusal/quote | source owner recomputes | use cached `AppliedUnits` |
| loan-shark funds owner missing | host composition | mark loan action unavailable | integrator binds/decides canonical funds owner | issue through engine because null is accepted |
| loan-shark credit overflow | FundsLedger result | no House workaround; surface unavailable/failure | source loan owner should compensate or refuse | grant again through House |
| loan-shark bounty owner missing on default | engine/host source state | display default stage without claiming bounty exists | source owner when bounty is bound | infer bounty from default stage |
| same day tick repeated | each source owner | read current status only | canonical day owner; owner idempotence | add suppression state in House |
| user identity changes | House membership owner | refresh/filter rows; disable stale actions | TH-1 identity owner + source mapping | transfer debt between survivors |
| save flush not completed | canonical save owner | show stale indicator; do not promise durability | source save orchestration | persist only the House row as if source committed |
| restoration partially succeeds | SaveOrchestrator/source section owner | show per-source availability | save recovery system | cross-restore partial state by cloning another source |

### Recovery protocol

1. Stop action routing to the failing source; keep other source rows available if truthful.
2. Preserve source logs/results and canonical save sections; do not manually rewrite them.
3. Determine whether the owner command mutated before failure by reading canonical owner state and transaction result, not by UI `LastEvent` alone.
4. Use the source’s existing compensation or recovery path. If none exists, open a source-owner repair plan; TH-3 cannot invent compensation logic.
5. Rebuild the projection after recovery and reconcile against the source’s own stable references.
6. Record exact focused verification on the authorized package; do not broad-test unrelated credit owners.

## Appendix F — Data, API and migration questions to answer before estimates

### F.1 Source ID and borrower identity worksheet

For every candidate source, Phase 0 should fill one row per source schema version:

| Question | Required answer |
|---|---|
| Which exact DTO/object contains the open obligation? | Fully-qualified type and owning file |
| What is the stable key? | Field(s) and uniqueness enforcement |
| Is the obligation keyed to debtor, creditor, syndicate or wallet? | The source’s actual lookup semantics |
| Can more than one obligation be open per debtor? | Source invariant and tested boundary |
| How does a House member map to it? | Existing identity resolver or “not supported” |
| Does save restore preserve this mapping? | Exact section + round-trip test |
| What event means issue, due, settled, forgiven or default? | Owner event/API and status field |
| What value/unit is exposed? | Source type, precision, conversion source |
| What commands mutate it? | Exact public methods and host route |
| Can command replay create a duplicate? | Existing idempotency contract/test |
| How do failures compensate? | Canonical caller and exact inverse/atomic operation |

Blank cells are P0; “the UI already shows it” or “the identifier looks unique” is not an acceptable answer.

### F.2 Catalog questions

If a later decision allows authored House-specific credit terms, first answer:

- Is a current `DebtTemplate` capable of expressing the desired principal in item quantity and the expected settlement amount without semantic overload?
- Does it already express borrower class or member eligibility? If not, can current coordinator pass the correct person context while preserving all gates?
- Does a template’s `rate` have a defined unit, arithmetic, precision, and display contract consistent with the ledger’s `TotalOwed` and old content?
- Are consequences all routed by current consequence catalog/dispatcher, or would House need new forfeiture types?
- Which schema loader performs strict validation and how are unknown/inactive templates reported?
- Is `maxDay=0` the supported unbounded sentinel for this catalog, including legacy content?
- Would adding House templates cause offer selection to change for current Holdfast callers because first eligible in authored catalog order wins?
- Do `CatalogIntegrityValidator` checks establish creditor, item, consequence and range references? If not, a catalog plan must state required validator changes.

No data addition should precede the above consumer analysis. A row that validates structurally can still change current offer selection.

### F.3 Save and version questions

For any selected owner, verify:

1. Whether the dedicated checksummed file and canonical save-section envelope both capture equivalent state, as the current main partial may do.
2. What happens if the checksummed file succeeds but `CaptureSection` fails or is dirty-flushed later.
3. Whether restore uses canonical section or sidecar file as precedence.
4. If source state has a version field, how old missing values normalize. `LedgerDebtSystemState` has no explicit schema version in the inspected DTO; `BlackMarketState` does; loan-shark save state has schema version 1.
5. Whether a House-only optional section can be omitted from legacy saves. Default projection should require no section.
6. Whether event subscribers attach before or after restore emits `OnStateChanged` and can inadvertently mark state dirty.
7. How New Game resets the source session and unsubscribes events; House must not survive into the next campaign with stale references.
8. How multiple save slots scope static `SaveStore` state and file paths.

If the default is no new save state, the migration plan is “none” only after proving projection can rebuild from source after all valid load/backup paths. If a House ID-to-owner mapping is required and persisted, that becomes a separate schema decision because mapping changes could orphan existing debts.

### F.4 Status mapping questions

Do not force statuses into a universal five-state enum. The source states are richer and not equivalent:

| Ledger document flags | Black-market status | Loan-shark stage | Why they are not interchangeable |
|---|---|---|---|
| signed and days remaining | active | Current | Tick rules/rates differ |
| `forfeited` true | possibly overdue logic/event | Grace, Delinquent, or Defaulted | Forfeit due is not the same as monetary default |
| paid | repaid | Settled | Ledger `PayContract` does not itself evidence a funds movement |
| forgiven | source may not use same state | Forgiven | Consequence, loss and settlement rules differ |
| unsigned draft | no active debt yet | not applicable | Draft is not exposure |

If UI needs a sorting bucket, define it as display-only and preserve each source state in row detail. Never map “overdue” from due date comparison if the owner’s status is authoritative.

## Appendix G — Risk-based test inventory for a future focused package

Tests must prove new cross-owner contracts, not reassert every source rule. The table helps choose a minimal, non-redundant set after Phase 0.

| Proposed test | New behavior it would prove | Existing source coverage to reuse | Do not duplicate |
|---|---|---|---|
| House projection selects only records whose member scope is verified | TH-specific mapping | owner collection/capture tests | owner math or template rules |
| Projection keys remain stable after restore | composite reference contract | each source owner’s save round-trip | duplicate source save suites |
| Projection preserves unit and source amount | rendering/query fidelity | owner balance tests | recomputing interest formulas in TH test |
| Cross-source values never aggregate | no false total behavior | source type definitions | arbitrary UI snapshot of every source detail |
| Unavailable owner doesn’t show zero/no debt | composition failure UI truth | save/load failure tests | mocking an owner as empty and treating it as absent |
| House request hits only one canonical coordinator | command route | `TradeCreditCoordinatorTests` | second copy of all eligibility gate permutations |
| stale preview rejection does not mutate | House action refresh | existing stale acceptance test if present | reasserting every gate individually |
| source event refreshes once and disposal detaches | TH host lifecycle | `HostSession` event tests | unrelated day-owner tests |
| repayment uses same ID/unit preview and commit | typed action link | `BlackMarketSettlementTests` or LoanShark tests | reimplementing owner repayment assertions |
| save/reload path reaches current owners | real integration boundary | focused host wiring test | full campaign or full economy suite absent risk |

Candidate focused command selection should be written before the first test run and stay below the repo’s 10–15 step cap. If a test fails in another owner’s contract, stop and route that issue to its claim owner rather than broadening the House package opportunistically.

## Appendix H — Package boundaries and handoff evidence

Because source owners cross multiple plans, implementation should be split by accountable interface, not by apparent UI page:

1. **Audit package:** no code; exact source map and owner API contract; foreman decides whether to open a package.
2. **Read projection package:** one owner or read-only multi-owner adapter, only if public APIs and identifiers are proven. TH-1 path owner/integrator owns the shared House presentation seam.
3. **Single-owner command package:** one existing owner, one command type, one settlement path, and tests already selected. Its owning source package must approve any API extension.
4. **Cross-source read only:** may compose multiple owners, but cannot route a command to more than one of them in the same mutation.

Every handoff should include:

- source path and public API names that were inspected;
- claimant and exact file paths;
- member/borrrower map proof, or an explicit unsupported result;
- units and examples showing no mixed arithmetic;
- ordinary behavior unaffected when the House is absent;
- restore, reset, and event-disposal proof;
- exact focused command and result;
- remaining owner-level gaps with the correct owner named.

### Integration review questions

Before a reviewer accepts a later package, ask:

- Is the data visible a projection of source state or did a duplicate field slip in?
- Can a stale House object still issue credit after campaign reset or save-slot switch?
- Are event subscriptions detached when panel/session closes?
- Do different credits keep their own units, due rules and status semantics?
- Could UI confirmation text imply a settled amount that the owner does not collect?
- Can same-day campaign ticks or events duplicate interest, consequences, or history?
- Did the package modify shared economic owners while their exact path belonged to another claim?
- Does the save restore test use the real owner sessions and inventory/wallet, not only serialized DTOs?

Any “yes/unknown” on duplicate state, borrower mapping, transaction atomicity or shared-path ownership blocks the mutation package until resolved.
# Appendix I — Lifecycle edges for any future credit integration

This appendix records the places where a product-facing credit offer can look
simple while crossing several independently owned states. It is a verification
map, not a proposal to normalize those states into a new account ledger.

## I.1 Creditor and borrower must be explicit at every boundary

The currently inspected debt owners do not share one party model. The ledger
contract path is creditor-scoped and debtor keyed; the underworld record carries
syndicate/debt identifiers but not a debtor/member identity; the loan-shark path
uses its own debt record and an externally supplied FundsLedger. Those shapes
cannot be safely joined by assuming that `debtorId`, `memberId`, a character ID,
or the active shelter means the same thing.

For each path, the Phase 0 audit should answer: who is the legal creditor; who
is the borrower; whether the borrower is a person, crew, settlement or session;
whether a creditor can hold multiple agreements against the same borrower;
whether creditor identity survives faction changes; and which party receives a
payment. These are semantic questions because the existing `GetContract`
lookup keyed by debtor cannot represent two simultaneous creditor contracts
for one debtor without an additional discriminator. The answer is not to add a
House-wide ID registry before determining whether the owning system already has
one.

Use a party mapping table in the eventual implementation handoff. Every cell
must cite the owner field or current host binding. A blank is a P0 VERIFY, not
permission to use the player's current faction as a fallback. Old saves with no
party identity must retain their existing interpretation; migrations must not
guess a creditor from current world state.

| Boundary | Question to prove from source | Unsafe shortcut |
|---|---|---|
| Offer creation | Which canonical owner creates the agreement and captures creditor identity? | House panel constructs a second contract object. |
| Acceptance | Which owner checks capacity, existing debt and eligibility? | UI checks one debt list, coordinator checks another. |
| Disbursement | Which wallet/item owner receives value and reports failure? | Assume an attempted `TryCredit` succeeded. |
| Accrual | Which owner advances interest and its clock? | Add calendar-day interest in House update code. |
| Payment | Which owner computes payoff and moves funds/items? | House subtracts a displayed amount from a wallet. |
| Default | Which owner changes status and applies consequences? | Treat a missed UI deadline as a second debt state. |
| Save | Which owner serializes each field and migrates old records? | Copy owner state into House saves for convenience. |

## I.2 Contract with no stable agreement ID

Current source evidence records the ledger path's `GetContract` lookup by
debtor ID and shows no exposed unique agreement ID in the inspected contract
shape. `PayContract` can mutate state, but the source trace did not show a
wallet transfer or item transfer in that method. This creates several distinct
edge cases:

1. **Two creditors, one debtor.** If the owner permits only one contract per
   debtor, the second offer must be rejected by that owner. If it permits two,
   a debtor-only lookup is ambiguous. The UI must not choose the first result
   from iteration order.
2. **Same creditor, amended terms.** Editing a displayed principal could mutate
   the live agreement or accidentally create a duplicate. Verify whether terms
   are immutable after acceptance and whether the owner has amendment history.
3. **Load after acceptance.** If save/load restores a contract without a stable
   key, a House view may only query it by the owner's supported key. The view
   cannot persist a synthesized key and treat it as canonical.
4. **Duplicate command.** Replayed accept or payment UI events must not create
   duplicate agreements or subtract funds twice. The existing owner must expose
   a guard, transition state, or transaction boundary before a UI route is
   called safe.
5. **Borrower identity change.** A player taking control of a new crew must
   not redirect an old obligation. Resolve the party from the saved owner
   record, not current selection.

Expected Phase 0 result: either prove the current owner already forbids
ambiguous contracts and exposes a supported stable reference, or mark the
proposed House contract route blocked. A House-local sequence counter or hash
is not a substitute because it cannot survive old saves, owner mutation, or
cross-system replay unless the owner adopts it.

## I.3 No-ID debt and FundsLedger retention are separate evidence problems

Debt identity and movement history answer different questions. A debt record
can exist without an immutable agreement key. A FundsLedger can retain only a
bounded recent movement history while still reporting current balances. The
observed movement cap is 128 entries; therefore movement records are not a
complete audit journal. Do not reconstruct lifetime payments, debt principal,
or receipts by summing the retained tail.

At the 129th movement, establish from implementation whether the oldest row is
dropped before or after balance mutation, whether the retained records are
ordered, and whether a save contains only retained movement evidence. These
questions affect diagnostics and migration, not the canonical balance owner.
Any UI that says “paid all time” from that bounded history would be false after
eviction. A House readout should label balance as current owner state, and any
recent-movement list as bounded history with its actual retained range.

If payment moves funds and then the process crashes before a debt owner records
the reduction, the source must establish whether the operation is atomic,
compensated, or replay-safe. If the debt is reduced first and wallet mutation
fails, it must establish the inverse. Without a cross-owner transaction or
idempotent payment reference, the plan cannot promise exactly-once payment.
Manual retry must not issue a second debit solely because the first result is
missing from the UI.

| Crash point | Evidence required | Safe UI outcome before proof |
|---|---|---|
| Before funds validation | Validation has no mutation side effects. | Keep offer/payment pending; allow owner refresh. |
| After funds debit, before debt reduction | Durable debit reference and owner query/reconcile route. | Show unresolved transaction; disable duplicate payment. |
| After debt reduction, before funds debit | Owner transition is compensated or transaction has not committed. | Do not report payment complete. |
| After both owners commit, before UI refresh | Idempotent command key or authoritative reread. | Refresh from both owners; do not reissue. |
| During save migration | Old and new codecs cannot both create the same obligation. | Fail closed, preserve backup, report migration error. |

No row above authorizes a reconciliation ledger. The gap may require a new
architecture decision, but until then it is a P0 stop condition.

## I.4 Payment failure, retry and save migration cases

The audit must distinguish a rejected payment from a failed payment and an
unknown outcome. Rejected means the owner proved no mutation (insufficient
funds, invalid borrower, closed contract). Failed means a mutation began and a
compensation/result is known. Unknown means the host lost the response and must
query the current owner before retry. Conflating the last two risks a double
debit.

Payment scenarios to run against the existing owners before any UI command is
added:

- exact payoff; overpayment; partial payment; zero payment; negative request;
  and value larger than principal plus accrued interest;
- borrower has less cash than displayed because an intervening purchase
  committed after the quote;
- caller supplies funds but the FundsLedger rejects due to overflow or invalid
  currency/amount;
- debtor key resolves to no agreement, one agreement or multiple agreements;
- agreement became completed/defaulted between quote and command;
- loaded save contains a debt but no retained matching movement row;
- save has legacy debt fields but current codec expects different version;
- retry follows host timeout after one owner already committed;
- day boundary occurs after quote but before payment, changing accrued amount;
- capture/restore happens between staged debit and contract update.

For each, record exact current result, state before and after, emitted event,
save section, and whether the caller can determine mutation success. If a case
cannot be exercised without inventing an API, document the missing API and
stop. Keep monetary units owner-native; do not add unlike currencies or treat
item value as a currency conversion absent a current rule.

Migration must be owner driven. Before changing any debt codec, inventory all
current and legacy save samples/fixtures, identify version tags and fallback
branches, and confirm what happens to unknown creditor IDs, missing debtor
identity, zero balances, already settled/defaulted records, and duplicated
records. Preserve owner-defined ordering if it affects deterministic capture.
Do not derive interest twice when migrating an accrued balance: verify whether
stored principal, stored accrued interest, last-accrual day, or an equivalent
field is the source. A migration should be idempotent under repeated load/save
and should preserve checksum/replay behavior. No House section should copy
these fields.

## I.5 Worked examples, kept deliberately owner-local

### Example A: quote is stale

At day 12 the owner reports a payoff quote. A shelter purchase then reduces
funds. The player confirms the old quote. The payment operation must ask the
funds owner to validate the current amount at commit time. If the current owner
API accepts a caller-computed amount without revalidation, mark P0: the route
cannot safely use the quote as a debit instruction. A disabled button is not a
transaction guard.

### Example B: partial payment and pause

Suppose an agreement has principal `P` and the existing owner computes a flat
payoff from its current rate. A partial amount `x` is entered. The inspected
`PayContract` path reduces debt state, while a weather-related path pauses
contract progression. These are two independent behaviors: pause does not
establish payment settlement, and partial payment does not prove funds moved.
Expected test captures owner state before and after, verifies caller-visible
result, then advances a paused/unpaused cycle to ensure no duplicate accrual or
duplicate debit. Numerical examples must be filled from the actual current
rate and formula when the test is written; this plan does not invent balance
rounding or payment allocation rules.

### Example C: bounded movement history

After more than 128 wallet movements, a current balance remains authoritative
while an old loan disbursement may no longer be in retained movement history.
An interface must not label the 128 rows as a lifetime statement. Tests should
capture/restore the ledger around the eviction boundary and ensure balance
matches exactly; whether the tail is persisted must be confirmed from the
codec. This does not justify a second House history store.

### Example D: no debtor identity in syndicate debt

An underworld syndicate record can report amount and due/status but lacks a
debtor/member ID in the audited shape. A player-facing screen cannot safely
attribute it to the currently selected survivor. Before exposing it, trace the
creator, caller and save owner to determine whether debtor association is
external and stable. If not, show nothing at House level and record the
attribution gap as P0 VERIFY.

## I.6 Owner/API verification checklist

Record source paths and method names during Phase 0; answers must come from
code, not the type name or plan title.

| Owner | Verify directly | Acceptance consequence |
|---|---|---|
| `LedgerDebtSystem` | Contract fields, uniqueness, quote/pay mutation, clock, weather pause, save codec, error result. | No offer/payment route without a stable reference and explicit no-mutation failure. |
| `TradeCreditCoordinator` | Offer source/order, eligibility gates, duplicate prevention, creditor assignment and effects. | Panel may request a canonical offer; may not reproduce eligibility. |
| Black-market settlement | Debt key, borrower binding, partial payment, item/fund settlement, due handling, persistence. | No syndicate balance aggregation without identity and currency semantics. |
| Loan-shark engine | Issue debit, failed `TryCredit`, interest catch-up, enforcer status, capture/restore, host wallet binding. | Unchecked credit return and missing binding remain hard blockers. |
| `FundsLedger` | Balance owner, movement units/order/cap, invalid amount behavior, restore and overflow. | Display current canonical balance; do not infer full history. |
| Host adapter | Current player/session binding, event routing, save ordering, reload and stale UI behavior. | No host callback can claim success without owner result. |

If source moved since the last audit, update citations and conclusions. This
appendix does not claim the current branch has passed any of these checks.

## Appendix J — Do not flatten lifecycle semantics into one balance

The House projection may compare sources for navigation, but it cannot imply
that their lifecycle fields have the same meaning. “Open,” “due,” “paused,”
“defaulted,” “enforced,” “closed,” and “settled” belong to their respective
owners. A row-level normalized label can be added only if its mapping is
lossless for every displayed behavior; otherwise display the source label and
source owner. No House-level status should drive interest, collection,
eligibility or availability.

### J.1 Ledger contract lifecycle

The inspected `LedgerDebtSystem` exposes active and closed collections,
`PresentContract`, `PayContract`, and capture/restore. The current contract
shape is accessed through debtor lookup in the reviewed path. Source evidence
records a flat `principal * (1 + rate)` total calculation and weather pause.
That calculation should be treated as the current owner contract for display
only after verifying its currency/rounding and callsites. The UI must not add
calendar accrual because it sees a rate. A displayed “total” is a current owner
quote, not a guaranteed debit amount unless the payment verb accepts and
revalidates it atomically.

The owner-facing questions are: Is principal reduced or is the whole flat
amount cleared? What happens to excess payment? Can partial payment occur? Does
the rate apply once, on every day, or only at presentation? Does weather pause
only the due clock, or also interest? Are contract terms fixed after signing?
Do closed records persist indefinitely or are they pruned? Until answered,
the projection should keep principal, quoted total and paid amount separate
only when the owner actually supplies each value. It must not derive “paid” by
subtracting current balance from principal if the owner’s formula includes
interest or non-cash consideration.

### J.2 Trade credit ceremony and compensation

`TradeCreditCoordinator` has an offer-building path and acceptance path with
catalog/template-day checks, embargo/standing conditions, a same-creditor
unpaid-debt gate, principal item context, two-reading ceremony and
compensating grant/sign behavior. This is stronger than a bare UI form but
remains player-survivor scoped in the live host composition. A House role or
survivor borrower cannot be substituted without proving the coordinator's
borrower semantics and ownership.

If grant succeeds but signing fails, inspect whether the coordinator revokes
the exact principal item and whether revoke failure is observable. If the
contract is signed but grant callback partially succeeds, inspect whether the
owner compensates, and whether the compensating call is idempotent after host
exception/retry. A future action should return one of accepted, rejected with
no mutation, compensated, or unknown; if the current API collapses these
outcomes, a caller cannot safely offer automatic retry. The ceremony is not a
reason to route around the coordinator when terms fit; it is also not proof
that House-specific obligations fit its principal-item model.

### J.3 Black-market debt semantics

The black-market path consists of a persisted `UnderworldDebtRecord` and a
settlement service with preview/execute pairs. Audited record identity includes
syndicate/debt keys but not a debtor/member ID. The service's preview should be
read-only, and settlement should flow through its own wallet/inventory owners.
Do not convert black-market debt to chits or House credit merely to make a
single table. Verify whether principal represents item goods, currency, or
settlement value before displaying a numeric total next to other owners.

The caller-to-save trace must prove who holds the debtor association if it is
not on the record, what repayment resource is consumed, and what happens when
inventory removal succeeds but wallet settlement fails (or the inverse).
Partial repayment must update the exact underworld debt row, not an aggregate
syndicate total. A preview can go stale between display and execution; the
execute method must recompute/refuse if current state changed. No P0 is cleared
by a successful preview test alone.

### J.4 Loan-shark debt and clock catch-up

The separate loan-shark engine owns debt rows, interest/day behavior,
enforcement status and capture/restore. Its host adapter is the `loan_shark`
save-section path and day-tick entry point. The audited issue path appears to
ignore the return from `FundsLedger.TryCredit`; a default `LoanSharkHostSession`
constructor was not visibly bound to the wallet in the reviewed evidence.
Both are P0 until all callsites prove that another guard makes failure
impossible.

Long offline/catch-up intervals need particular care. If interest advances one
day at a time, a catch-up loop must be bounded or prove that all missed days
are applied once in stable order. If it jumps directly to current day, establish
whether the formula is equivalent and whether enforcement sees intermediate
thresholds. Save after day N, restore, and tick through N+k; same-seed runs
must yield the same balance, status and event order. The House must not
maintain a second “last credit day” to correct or supplement this owner.

For issue: if ledger row creation succeeds but wallet credit fails, determine
whether the row is rolled back. If wallet credit succeeds but row creation
fails, determine whether funds are revoked. Test both directions using the
real owner result seam. If current system cannot distinguish them, there is no
safe House-facing retry. A display-only exposure projection can still omit the
source with “not attributable” status; it cannot advertise an action.

### J.5 Event sequence and host ownership

For each source owner, list the event sequence at acceptance, accrual, payment,
default and restore. Events may drive UI refresh, journal, reputation, market
stock, or settlement. The plan must classify each observed subscriber:

| Subscriber effect | Projection safety question |
|---|---|
| UI refresh | Does it reread owner state or cache a stale amount? |
| Reputation/standing | Is it emitted exactly once with canonical creditor? |
| Inventory/wallet change | Is it gameplay mutation already included in owner command? |
| Journal/notification | Can restore re-fire it and duplicate user-facing claims? |
| Enforcement encounter | Does it rely on owner status transition order? |
| Save dirty flag | Does the adapter flush all owner changes on the same boundary? |

No broad event bus should be added to compensate for missing source-specific
signals. If no event is available, the host can refresh on the existing owner
command result or read on panel open, provided those paths are current and
bounded. Use source owner events as facts; do not treat event delivery itself
as proof that save state was captured.

## Appendix K — Additional acceptance cases for credit owners

The original matrix should be exercised by source owner, not by a synthetic
House-wide debt model. Add these cases when the selected source is confirmed:

1. **Same debtor, distinct creditors:** prove refusal or unambiguous selection;
   there must be no first-row/iteration-order behavior.
2. **Same creditor, settled then new agreement:** verify closed history does
   not block a legitimate new offer and old actions cannot target the new row.
3. **Principal item plus fees:** distinguish item grant from numeric debt
   amount; do not render both as the same denomination.
4. **Two-reading interrupted:** reload after the first reading; verify whether
   pending ceremony state is discarded, resumed or replay-safe. Do not let a
   stale panel complete the ceremony against changed terms.
5. **Embargo changes after offer:** acceptance must revalidate current standing
   and embargo, or clearly use the captured offer-time rule from its owner.
6. **Partial repayment at due boundary:** event order must establish whether
   repayment is applied before or after day accrual/default checks.
7. **Overpayment:** owner must return/refuse excess or define credit carry; House
   cannot choose to discard, refund or apply it to a second agreement.
8. **Debt with missing debtor metadata:** source is omitted from action routes;
   current selected survivor is not a fallback.
9. **Funds history eviction:** after 128 retained movements, current balance
   remains exact while UI labels history as bounded and does not claim a
   complete statement.
10. **Restore and duplicate command:** replay accept/payment after load and
    assert both owner balance and debt amount change no more than once.

Acceptance evidence should include pre-state, returned result, post-state,
emitted events, serialized owner state and user-visible amount label. A test
that asserts only a method returned true is insufficient for cross-owner
credit movement.

## Appendix L — Exposure projection and wording invariants

TH-1 and TH-2 are prerequisites, but they cannot turn incomplete source facts
into complete debt rows. Once those plans have current implementation evidence,
the projection should use source-specific optional fields rather than a
mandatory normalized record that encourages guesses. A source with no stable
ID can still be omitted with an owner-level diagnostic; it must not be included
under a generated ID. A source with no debtor association must not be displayed
as the selected survivor's debt. A source with a different denomination must
not be totaled with the rest.

For each row, field provenance should be reviewable:

| Display fact | Acceptable provenance | Disallowed derivation |
|---|---|---|
| Creditor | Saved owner record or explicit owner result. | Current faction leader or last dialogue target. |
| Borrower | Explicit owner identity resolved through current stable mapping. | Currently selected survivor/crew. |
| Principal | Owner-supplied amount and unit. | Reconstructed from current payoff. |
| Balance/quote | Current owner query at render or command time. | Locally accumulated rate × day count. |
| Due day | Owner-provided deadline with its calendar semantics. | Generic campaign day added in UI. |
| Status | Owner enum/text. | A global “open/late/default” mapping without proof. |
| Recent movements | Bounded owner history, labeled range. | Claim of full statement or full repayment timeline. |
| Action result | Owner command response plus refreshed owner state. | Button animation, toast or attempted callback. |

The view should state the denomination beside every figure. If the source has
an amount but no denomination, that row is unavailable for aggregate display
until clarified. If the same currency is held by a different wallet owner,
that does not prove it can settle the debt. Payment eligibility and balance
readout may come from separate owners and must be shown as separate facts.

### L.1 Quote freshness

A quote may become stale when time advances, weather changes, embargo or
standing changes, another transaction changes available funds, or an owner
closes/defaults the agreement. Each action control should therefore be backed
by execution-time validation in the canonical owner. UI freshness indicators
can explain the displayed quote's timestamp/day, but they are not guards.
Disabling an old button is useful feedback and is not a safety guarantee.

If the owner exposes only a getter and separate mutator, trace where its
preconditions are rechecked. If they are not rechecked, do not wrap both calls
in a House-level transaction that has no lock or rollback semantics. The right
handoff is an owner API question with a minimal required invariant: stale quote
cannot commit an invalid or duplicate payment.

### L.2 Privacy and party scoping

Debt details can reveal personal, factional or syndicate relationships. The
current owner’s borrower and creditor scope determines who can see the row.
Before a House-wide listing is built, establish whether the source record is
private to one survivor, available to all crew, or owned by the settlement.
Do not widen visibility simply because the House UI can enumerate records.
Missing access policy is P0 VERIFY; a hidden row is preferable to leaking
another party’s obligation. This rule does not create an authorization system;
it requires the existing host and source owner to define the audience.

## Appendix M — Decision table for action eligibility

An implementation reviewer can use this decision table to prevent a read-only
screen from drifting into a second economic authority:

| Evidence state | Projection | Action |
|---|---|---|
| Owner and row identity proven; borrower/creditor resolved; current read works. | Display owner fields with source label/unit. | Only existing owner action, after its command contract is proven. |
| Stable row but denomination unknown. | Display source/status only if useful; suppress total/amount or label unavailable. | Disabled; no conversion guess. |
| Debt exists but borrower identity missing. | Omit from player-specific exposure or label unattributed at a diagnostic level. | Disabled; do not use current actor fallback. |
| Multiple rows share a debtor-only lookup key. | Display only if source provides an unambiguous enumerator and IDs. | Disabled until target selection is unique. |
| Payment API mutates debt but not wallet. | Show debt as current owner state, not paid transaction history. | Disabled; no House debit/debt mutation pair. |
| Wallet debit path has unknown outcome or ignored result. | Current balance can still be read from FundsLedger. | Disabled; do not retry or claim receipt. |
| Source record is terminal/closed. | Display according to existing owner visibility and retention rules. | No payment route unless owner explicitly supports it. |
| Save migration unresolved. | Avoid fabricating migrated row data. | Disabled until owner codec behavior is known. |

This table is intentionally conservative. A hidden or disabled control is a
valid result for unsupported owner semantics; it is not a reason to create
parallel balance, debt, history or request-key state.

## Appendix N — Ledger debt verbs and terminal state matrix

`LedgerDebtSystem` is a document/forfeit authority with exact existing verbs;
it is not a funds ledger. This distinction should be explicit in every future
file map and command trace.

| Verb | Accepted state/guards observed | Mutations and side effects | What caller must not infer |
|---|---|---|---|
| `PresentContract` | Nonempty debtor, principal and term positive, nonempty named forfeit; refuses a signed unpaid contract and unresolved forfeit. | Creates/updates unsigned draft; captures creditor/template strings; increments read count; raises state changed. Paid/forgiven old draft is archived into `ClosedContracts` before new draft. | No principal transferred; one read is not signed credit. |
| `SignContract` | Existing draft; not signed; `readCount >= 2`. | Freezes signed flag/day and initializes days remaining; emits signed event. | Does not issue currency or item in this method. |
| `CancelDraft` | Existing unsigned contract; optional creditor/template match. | Removes draft. | Does not cancel signed debt or reverse a transfer. |
| `TickDaily` | Signed, unpaid, not forfeited, not forgiven. | Weather route may consume at most three grace days; otherwise decrements remaining days, triggers forfeit at <=0. Raises changed. | No funds accrual/payment; forfeit does not imply debt is terminally paid. |
| `PayContract` | Existing signed contract; not paid/forgiven. It remains allowed after forfeit. | Sets paid, clears forfeited, emits paid event; keeps record until next new draft archives it. | No FundsLedger debit or inventory transfer appears in this verb. |
| `ForgiveContract` | Existing signed contract; not paid/forgiven; allowed after forfeit. | Sets forgiven/day, clears forfeit, emits forgiveness. Record remains. | Forgiveness is not a payment movement; balance clearing is a contract fact. |
| `RenegotiateContract` | Valid new terms; refuses paid/forfeited. Signed contracts require term end (`daysRemaining <= 1`); contested path requires fresh standing callback. | Signed contract terms change in place, days reset; unsigned draft is rewritten and must be read twice again. | Not a generic edit path while terms are active. |
| `TotalOwed` | No signed contract, paid or forgiven returns zero. | Computes flat `principal * (1 + rate)`; no compounding. | Not proof the wallet can pay this amount or that amount is in chits. |

The `DebtContract` data has debtor ID, creditor ID, template ID, principal,
term, rate, named forfeit, read/signed fields, remaining days, paid/forfeited/
forgiven flags, signed/forgiven day, weather delay count and last gate ID. There
is no stable contract ID. `GetContract(debtorId)` returns the first active-list
row matching debtor; `PresentContract` itself prevents a second signed unpaid
row for that debtor, but the API cannot address more than one active draft by
debtor if corruption or old state creates duplicates. The displayed creditor
must come from the found record, and action must use that owner lookup without
inventing another key. If a future use case requires multiple simultaneous
creditors per debtor, this owner needs an explicit extension decision.

### N.1 Lifecycle worked example (Ledger owner only)

At day D an owner caller presents a valid principal, term, rate and named
forfeit. The first presentation creates a draft with read count one; second
presentation increments to two; `SignContract` sets signed day and days
remaining. No funds movement is shown in these three methods. Each later
`TickDaily` either consumes one bounded weather grace day or decrements the
term. When days reach zero, `forfeited` becomes true and an event fires. The
borrower can still call `PayContract` after that event; it marks paid and
clears forfeited without debiting funds in this owner. Alternatively,
forgiveness marks forgiven and is explicitly not payment. When a new contract
is next presented for that debtor, paid/forgiven old ink moves to
`ClosedContracts` and a new unsigned record is created.

A House action cannot honestly label `PayContract` “debit paid in full” without
another source owner and an existing coordinated settlement path. If it calls
funds debit then `PayContract` and the second step refuses, the wallet can
change while debt remains open. If it calls `PayContract` then debit rejects,
the debt owner closes while funds remain unchanged. A two-call UI wrapper has
no rollback primitive in the inspected method. This is a hard action stop,
even if the current game has narrative or barter paths that use the contract
without a wallet.

## Appendix O — Black-market settlement's compensation boundary

The black-market path does more cross-owner coordination than bare Ledger
debt, but its callback is not automatically a durable idempotency mechanism.
`BlackMarketSettlementService.Repay` performs a read-only preview; validates
that rounded applied units match its settlement quote; calls
`BlackMarketSystem.RepayDebt` with a settlement callback; and on success
notifies the wallet of external settlement. `RepayDebt` itself previews,
updates `repaidUnits` and possibly status/trust, then invokes the callback. If
the callback returns false or throws, it restores the debt amount/status/trust
snapshot. In the current service callback, the wallet `TryDebitValueForSettlement`
result gates that rollback. This is a useful synchronous compensation seam.

The boundary still requires source verification for retries. If a callback
debits wallet and then throws before returning true, the debt method rolls its
own state back but cannot restore the wallet. If an event subscriber throws
after `RepayDebt` has accepted, the debt and wallet may already be committed
while the service caller sees an exception rather than a result. Those paths
depend on callback/event behavior and save timing. There is no visible caller
idempotency key in the service method signature. Before House exposure, trace
`TryDebitValueForSettlement`, `NotifyExternalValueSettlement`, all
`OnDebtRepaid` subscribers, and the service host's exception boundary; verify
whether each can mutate or throw.

The settlement amount also has a precise rounding rule: `TryRoundRepay` rounds
the owner's applied float upward (ceiling) to integer settlement value. Preview
and execute compare the same rounded amount so a stale quote that changes
between calls returns false. Test fractional outstanding values around integer
boundaries and near completion tolerance (`1e-4f`). Display the quote's
settlement units exactly as returned; do not recompute rounding in the UI.

`OutstandingOnDebt` is currently principal minus repaid units for active debt;
at default it applies a one-time interest basis-point multiplier to the
remaining amount; non-active, non-default statuses return zero. `PreviewRepay`
accepts only active status; a defaulted record's computed outstanding amount
does not by itself make that debt repayable through this verb. This mismatch
between amount projection and action eligibility is source-defined and must be
kept visible to a projection: “not repayable by this command” differs from
“no amount due.”

## Appendix P — Cross-owner command test table

These tests should be selected only for the exact owner the future action uses.
They are not authorization to test every economy path or add duplicate tests.

| Test case | Required source-state assertion | User-visible result if current API cannot prove it |
|---|---|---|
| Ledger draft first/second read then sign | read count and signed day; no FundsLedger delta. | “Draft/signing state” rather than borrowed funds. |
| Ledger pay before sign | false; draft remains; no wallet mutation. | Rejected without payment claim. |
| Ledger pay after due/forfeit | true if signed and not paid/forgiven; forfeit clears. | Owner status refresh; do not invent debit. |
| Ledger forgive after forfeit | true, forgiveness day recorded, no wallet movement. | Label forgiven, not repaid. |
| Ledger second payment/repeated `PayContract` | false after paid; stable closed record retained. | Already settled; no repeat debit. |
| Black Market preview then stale funds | execute callback rejects; debt state rolls back; wallet remains as before. | Refresh quote; no blind retry. |
| Black Market fractional partial | upward rounding exact; debt repaid amount matches callback settlement. | Display owner-returned integer unit. |
| Black Market subscriber exception | state/result semantics are captured; determine if mutation committed despite exception. | Unknown outcome; query current owner before retry. |
| Loan-shark funds overflow | `IssueLoan` currently appends debt then ignores failed `TryCredit`; expose exact mismatch. | P0: disable issue action. |
| Funds movement 129th operation | oldest-first eviction; balance unchanged by truncation; capture/restore keeps at most 128. | Label recent bounded movements only. |
| Loan-shark catch-up across save | exact interest/status after D→D+k matches uninterrupted processing. | Avoid local estimated payoff. |

Tests should assert full tuple (funds balance, debt fields, active/closed
placement, emitted events and captured state), not just a boolean return. Existing
focused targets named in §18 are where equivalent behavior must be inspected
before adding any missing case.

## Appendix Q — Loan-shark host composition and issuance boundary

The current Main composition is more specific than “wallet binding uncertain.”
`Main.SetupLoanShark` constructs `new LoanSharkHostSession()`, restores the
`loan_shark` save if present, and subscribes dirty tracking. The no-argument
host-session constructor creates `new LoanSharkEnforcerEngine()` without a
FundsLedger argument. Its `IssueLoan` forwards creditor faction, debtor,
principal chits, term and current day to the engine. In `LoanSharkEnforcerEngine.IssueLoan`, validated values produce a debt ID
`loan_<creditor>_<debtor>_<day>_<count+1>`, a record is appended, and the
FundsLedger credit is attempted only if `_fundsLedger != null`. The result of
`TryCredit` is discarded. The host path does not visibly attach a ledger.

Therefore an issue through this host seam can produce a loan debt record
without disbursing chits at all. In the currently composed default session,
the inner branch is skipped because no funds ledger is attached. If a future
caller injects a ledger into the engine, overflow or invalid amount can still
leave the debt row issued because the `TryCredit` result is ignored. These are
two distinct failures: absent disbursement in current composition, and
unchecked disbursement failure in a possible injected composition. Both block
House loan offers; do not infer a transfer from `OnLoanIssued` or `LastEvent`.

The record does preserve a broader loan contract than the Ledger debt type:
debt ID, creditor faction ID, debtor ID, principal/current balance in chits,
daily interest permille, issued/due day, grace-period days, escalation stage,
last accrual day, total interest accrued, total repaid, bounty state/link,
enforcer raid risk, and last escalation day. Capture/restore preserves this
record list. `ProcessDailyTick` computes elapsed days from `LastInterestAccrualDay`
and accrues each elapsed day in a loop. This owner is capable of representing
actual chits and borrowers, but the live host composition currently lacks the
wallet connection needed to honor its own “issue” presentation.

The accurate Phase 0 test is not simply “overflow returns failure.” First
inspect the path reachable in normal gameplay and establish whether any caller
uses an alternate constructor or replaces the Engine property. Then assert
funds delta and debt record for that route. For the currently reviewed default
composition, expected evidence is: debt record exists; no FundsLedger was
attached; no wallet delta can occur through the engine. For a specifically
injected engine fixture, test balance overflow: record still exists and credit
failure is not surfaced. If a later code change corrects either contract, plan
and tests must be refreshed before enabling House UI.

## Appendix R — Black-market debt identity, quote and retry example

The underworld debt ID is generated as `debt_<syndicate>_<day>_<debt-count+1>`
and persisted with syndicate ID, principal units, repaid units, interest basis
points, issue/due days, status and reason. It contains no debtor/member ID.
Because ID uses list count, verify whether debt rows are ever pruned or
reordered before assuming no collision after migration. The current state
capture/restore copies debt rows as a list; the entry-generation path adds
records rather than updating a House ledger.

`TakeLoan` gets a preview and rejects if invalid; then appends debt and
increases syndicate trust before invoking the settlement callback. A false or
throwing callback causes removal of that exact newly added row and restoration
of prior trust. The settlement service callback checks the wallet can credit
and calls its external-value settlement method; successful result is followed
by notification. This is more complete than LoanShark's host path, but there is
still no actor binding in debt record. A House may only display this debt when
some separate persisted owner mapping proves the borrower; until then, do not
attribute it.

`RepayDebt` previews, snapshots repaid amount/status/trust, applies the
min(request, active outstanding), marks Repaid at a tolerance, and then asks
the settlement callback to debit. If it returns false, the debt fields and
trust roll back. The SettlementService performs its own preview and exact
rounded-amount equality check before debit. It uses ceiling rounding for
repayment, so e.g. a fractional owner quote of 10.01 units requires 11 integer
settlement units. This example illustrates rounding behavior only; it does not
assert that 10.01 exists in shipped authored data.

At due processing, the owner marks active debt defaulted and event path applies
the default consequence. `OutstandingOnDebt` applies interest to remaining
principal only in Defaulted status; the standard `PreviewRepay` rejects any
non-active status, including Defaulted. Therefore the current source can
report a positive computed amount due while that repayment verb refuses the
same debt. Do not show the amount as directly payable without tracing a
different current resolution route. This status/verb distinction should be in
the UI contract if the owner ever exposes it.

| Example state | Preview result | Execute/retry implication |
|---|---|---|
| Active, amount positive, wallet can debit rounded amount | Available with capped `AppliedUnits` and rounded settlement units. | Call once; reread debt and wallet after result. |
| Active, wallet falls below quote before execute | Execute callback should refuse and `RepayDebt` rolls debt fields back. | Refresh; no automatic second debit. |
| Debt already Repaid | `PreviewRepay` rejects because status is not Active. | No duplicate action. |
| Debt Defaulted, positive post-interest remainder | Outstanding helper may return positive value; preview rejects `debt_not_active`. | Escalate to existing default-resolution owner, not House repayment. |
| Callback debits and then throws | Debt method restores debt fields; wallet rollback is not shown by this method. | Unknown outcome; query canonical balance and debt before retry. |
| Repeated debt ID after save restore | Depends on persisted list count and no-prune invariant. | Verify unique ID and retain owner reference; never regenerate. |

This path is a candidate for a source-linked read view, not a general House
credit API. It has stronger transaction callbacks but unresolved borrower
identity, exception window, status-specific repayment and exact host behavior.

## Appendix S — Funds movements are bounded deltas, not receipts

Each `FundsMovementRecord` contains day, signed delta, resulting balance,
reason key and source ID. It has no unique movement ID, settlement-party ID,
contract ID field distinct from `SourceId`, idempotency token or reversal link.
`TryDebit` and `TryCredit` mutate balance only for positive amounts; debit
rejects insufficient balance; credit checks `int.MaxValue` overflow. Each
successful mutation evicts the oldest row when count has reached 128, then
appends the new record. Capture serializes balance and a copy of retained rows;
restore clamps a provided history to its last 128 records and clamps balance
nonnegative.

The bounded history is useful for recent attribution, but it cannot establish
that one debt was ever disbursed or repaid: records may be evicted; reason and
source strings are not a foreign-key constraint; callers can provide arbitrary
source IDs; and contract methods may not invoke FundsLedger at all. Current
balance can still be exact while historical evidence is partial. UI language
should say “recent movements” and show only the rows retained. Do not label it
a statement, complete debt history, or proof of lifetime net settlement.

Reconciliation after a failed cross-owner action must query each owner. Never
attempt to recreate an absent FundsMovementRecord by changing balance; never
subtract movement rows to reconstruct a debt balance; and never add a House
movement row as a substitute receipt. If source identity is absent from one
side of a cross-owner transaction, flag the exact gap for that owner/API.

## Appendix T — Late settlement walkthrough across unlike owners

This comparison uses one late moment to show why the House must preserve each
owner's decision instead of converting “default” into a universal payable
status.

### T.1 Ledger contract after the named forfeit is due

Start with one signed `DebtContract` whose debtor ID is `survivor_a`, whose
term has expired, and whose `TickDaily` call set `forfeited = true`. The current
`TotalOwed` still returns the flat principal/rate calculation because the
contract is signed and is neither paid nor forgiven. `PayContract` remains
allowed after forfeit as the documented honored path: it sets `paid = true`,
clears `forfeited`, emits `OnContractPaid` and raises state change. It does not
consult FundsLedger in this method. No cash debit can be attributed to this
call from current source evidence. On a later new `PresentContract` for the
debtor, the paid ink is moved from active contracts to `ClosedContracts` and a
fresh read-twice draft begins.

House-safe projection: show the owner's forfeit status and flat quote if those
fields are current; after the owner verb succeeds, show “paid in contract
ledger” or the source's exact terminal state. Do not show “chits paid” unless
an already existing coordinated path also proves wallet debit and owner
mutation. If a future command tries debit-first, a `PayContract` failure would
need refund; if it marks paid first, failed debit leaves no money moved. No
compensation API spans these operations in the inspected class, so House must
not compose them itself.

### T.2 Defaulted underworld debt with computed remainder

Consider an underworld debt with principal units `P`, partial repayments `R`,
positive interest basis points, and `status = defaulted`. The helper
`OutstandingOnDebt` computes `(P-R) * (1 + interestBp/10000)`. Despite that
positive figure, `PreviewRepay` accepts only `status == active`; it returns
`debt_not_active` for defaulted debt. The amount calculation is not an
authorization to repay through the active-debt verb. A House table can show
the debt as defaulted and explain that the active repayment route refuses it;
it cannot switch the status back to active, debit a wallet, or invoke the
callback itself.

### T.3 Loan-shark delinquency with no current wallet binding

Loan-shark records explicitly carry current chits balance, due day, grace,
interest accrual day and escalation stage. But normal host setup constructs a
no-argument `LoanSharkHostSession` whose engine has no FundsLedger attached.
An issue command still emits a loan-issued row; current source composition
does not show disbursement. A later `RepayDebt` result might mutate the loan
record according to engine rules, but it cannot prove a wallet debit from this
composition. The visible late balance and enforcement stage do not close the
wallet path.

### T.4 Comparison outcome

| Owner | Late-state amount | Late action result | Meaning of success |
|---|---|---|---|
| Ledger Debt | Flat formula remains while forfeit is due. | `PayContract` allowed after forfeit. | Contract record paid; no wallet mutation shown. |
| Underworld | Defaulted remainder includes default interest. | Active-only `PreviewRepay` refuses defaulted status. | No repayment via that verb; another owner path is needed. |
| Loan shark | Chit balance and escalation stage are in debt record. | Host session has no wallet bound in inspected construction. | Debt-owner result cannot certify cash movement. |

This late-settlement example keeps numbers and statuses source-local. No
cross-owner aggregate payoff can be calculated without denomination and actor
mapping. A House action remains disabled whenever its selected completion
verb does not include the required canonical funds or item movement.
