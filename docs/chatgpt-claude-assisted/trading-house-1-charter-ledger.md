# TH-1 — Charter, Membership, and House Ledger

STATUS: DRAFT — evidence-backed proposal for review; no ownership claim, path claim, implementation approval, or commitment to create a new save owner.

**Plan family:** The Trading House (TH-1 of 4). **Dependency:** none; TH-2 through TH-4 may depend on the identity and reference contract defined here, but this document does not authorize their work. **Drafted:** 2026-09-29. **Planning target:** institution-level coordination over existing economic owners. **Primary rule:** the House may identify participants and summarize canonical facts; every transaction, balance, route, contract, debt, stock quantity, price, standing change, and production result remains with its present authority.

## 1. Objective

Define a durable institution concept that can coordinate a player's dealings with traders and settlements while preserving one owner per economic concern. The first useful version is intentionally modest: a charter identifies who may act for the House, and a ledger presents traceable references to facts recorded by existing systems.

The desired player-facing question is: “What has the House agreed to oversee, and which canonical record proves the current status?” It is not “How much wealth does the House own?” unless an existing owner can answer that question without the House copying or aggregating mutable balances as authority.

Success means a bounded proposal that can be audited before implementation: source facts are named, identity and membership ownership is unresolved until a current host/faction owner is verified, and every proposed interaction degrades safely when an owner or stable identifier is unavailable.

## 2. Current Reality

The economy is not an empty feature area. It contains several active owners with overlapping user-facing vocabulary but separate state and behavior:

- `MarketSystem` owns market price calculations, demand/index/shock state, and market-side activity/pressure bookkeeping in `MarketState.ledger`. Its `Buy`/`Sell`/`Barter` methods do not settle inventory or wallet value; a caller-level transaction owner must supply that evidence.
- `PlayerTradeRouteSystem` owns player route contracts, run counts, reliability, cooldown, and tariff totals. `TradeRouteContract` carries route and counterparty IDs, cadence, cargo legs, run state, and its own capture/restore contract.
- `CaravanTradeNetworkSystem` and authored caravan route data represent caravan-network behavior. They are not interchangeable with a player-established route contract.
- `TradeCreditCoordinator` is an adapter over `LedgerDebtSystem`, authored debt templates, embargo/war checks, campaign day, and item-grant callbacks. It projects offers and revalidates gates on acceptance; it does not provide a House loan portfolio.
- `BlackMarketSystem` and `BlackMarketSettlementService` own black-market offers and atomic settlement against inventory and the wallet. `TradeRouteMonopolyEngine`, `TradeEmbargoSystem`, and other economy modules have their own bounded concerns.
- `HoldfastTradeSession` is an existing player-facing trader that validates stock, funds, and inventory and returns a structured result; its save DTO records current value/held/stock rather than a durable per-operation receipt history. `FundsLedger` tracks the canonical integer chit balance with a bounded 128-entry movement log.
- Host wiring exists for the economy and trade routes. `EconomyHostSession` exposes market price explanations, ticks and save capture; `TradeRouteHostSession` exposes route contracts and census. Main/day/save orchestration already has routes for these systems.

The names “trading institution,” “charter,” and “House ledger” can tempt a duplicate faction registry, contract board, funds ledger, or market history. This plan explicitly declines those structures. It describes a derived coordination layer only, and leaves the exact durable identity owner as an unresolved P0 premise audit.

## 3. Required Delta

**Existing behavior:** canonical economic owners already record transactions, routes, credit, and black-market settlement. Their data can be inspected through their current APIs and host sessions. Existing plans separately address freight company operations, posted contract offers, credit, and production commitments.

**Requested capability:** one fictional institution presents its charter and a cross-owner view of authorized participants and linked activity.

**Smallest delta:** identify one existing owner suitable for House identity and membership, then add a read model that resolves stable source references on demand. If no suitable identity owner exists, the plan stops for an architecture decision. It does not create a registry by default. No new event history is justified until concrete consumers prove that a compact, non-derivable fact must survive the removal of its source.

The ledger is a query over sources, not an append-only shadow. It can group and sort results, but totals and status are recalculated from canonical owners. It must label unavailable, stale, and unsupported sources instead of converting them into “completed,” “failed,” or zero-valued facts.

## 4. Evidence

Evidence was checked in source on 2026-09-29:

| Evidence | Observed contract | Planning consequence |
|---|---|---|
| `Assets/Ashfall.Core/Economy/MarketSystem.cs` | `MarketState` v3 contains the market `ledger`, demand, indices, pressure, shocks, and nested policy state. `LedgerEntry` records (`day`, item, signed quantity, unit price/value, counterparty), not a general House journal. `Buy` / `Sell` call `Transact`, which adds a market history row and pressure and raises change events; this method does not mutate inventory or wallet. `Barter` writes its two market ledger legs and pressure. | Use canonical rows only as market-side bookkeeping/pressure facts. They do not prove goods/currency were actually exchanged by inventory/wallet owners. Do not label them “settled trade” without a caller-level receipt. No explicit immutable transaction ID exists in the inspected DTO; no durable row link until audited. |
| `Assets/Ashfall.Core/Economy/PlayerTradeRouteSystem.cs` | `Contracts`, `GetContract`, `GetCensus`, `CaptureState`, and `RestoreState` expose route-owned data. Dictionary lookup uses case-insensitive route IDs. | Route activity can be projected from the route owner, but route IDs must be qualified by owner and checked for reuse/removal. No route duplication. |
| `Assets/Ashfall.Core/Economy/TradeRouteContract.cs` | Contract fields include `RouteId`, `CounterpartyId`, schedule, reliability, cargo legs, and counters. Its persisted form is `TradeRouteContractSaveState`. | The House must not rewrite route contract behavior or treat route run counts as a general transaction log. |
| `Assets/Ashfall.Core/Economy/CaravanTradeNetworkSystem.cs` | `ExecuteBarter` validates arrived manifest, offered/requested items, and player inventory, then mutates inventory/manifest stocks and emits a completion event; no per-barter receipt ID is visible in the inspected save DTO. | Candidate only for its own caravan trade surface; atomicity on failure after partial mutation must be proven before any read surface labels the result complete. |
| `Assets/Ashfall.Core/Economy/CaravanAtomicTrader.cs` | `Commit` stores a quote-ID guarded `CaravanCommittedTrade` and supports capture/restore; it does not transfer goods or funds itself. | A committed quote is a durable commitment record, not settled property or transaction. |
| `Assets/Ashfall.Core/HoldfastTradeSession.cs` / `src/Host/HoldfastTradeSaveStore.cs` | Existing `Buy`/`Sell` and funds variants coordinate trader stock/value, inventory, and optionally `FundsLedger`; save captures current state but not per-operation receipt IDs. | Display an immediate owner result through its route, but do not derive a persistent House history from current snapshots. |
| `Assets/Ashfall.Core/Economy/FundsLedger.cs` | Integer balance with `TryDebit`/`TryCredit`, source/reason/day movement records, and 128-entry bounded retention. | Not a complete or immutable activity archive; avoid lifetime ledger claims. |
| `src/Host/EconomyHostSession.cs` | `Market`, `ExplainPrice`, `CaptureSave`, and `RestoreSave` are exposed by the host session; the session binds canonical catalog and change events. | If a panel is later warranted, bind through the existing host/owner seam. Avoid a new simulation host. |
| `src/Host/TradeRouteHostSession.cs` | Exposes `System`, `Census`, `Contracts`, contract reads and route commands; catalog loading and risk evaluation are host session responsibilities. | Route read projection may use existing session data; House cannot establish, tick, cancel, or rate a route outside this owner. |
| `src/Host/TradeRouteHostSession.cs` / `src/Main.CampaignOwners.cs` / `src/Main.TradeRoutes.cs` | Existing `trade_routes` save store and phase-5 day owner exist. Route contract registration replaces an existing case-insensitive route ID in the inspected Core API. | House must not add a duplicate daily update or route save; reference lifecycle must account for replacement/reuse. |
| `Assets/Ashfall.Core/Economy/TradeCreditCoordinator.cs` and `Assets/Ashfall.Core/LedgerDebtSystem.cs` | `TryBuildCreditOffer` and `TryAcceptCredit` re-evaluate canonical gates; acceptance uses the two-read ledger ceremony and compensating inventory grant/revoke callbacks. `DebtContract` uses `debtorId` as lookup key; the inspected state contains one current contract per debtor plus closed contracts, without a separate immutable contract ID. | House membership does not confer credit authority. A read-only exposure line may link only if a stable contract reference is proven; debtor ID alone may be too broad for historical linkage. Do not persist an inferred contract key. |
| `Assets/Ashfall.Core/Economy/BlackMarketSettlementService.cs` | Buy/sell previews validate market quote, wallet capacity, and inventory transaction; execution coordinates wallet and inventory settlement. | House presentation must not bypass or wrap this with a second settlement protocol. Black-market activity may be omitted if no stable receipt ID exists. |
| `docs/chatgpt-claude-assisted/README.md` and sibling TH-2…TH-4 drafts | The new family is described as coordination over existing owners; TH-2 defines the agreement/reference seam; TH-3 owns no credit product; TH-4 is production conditional. | Keep these plans consistent and do not broaden this plan into the next three packages. |

Duplicate and collision review found the named systems and adjacent plan families, including `.ai/plans/long-line-freight-2026-09-29.md`, `docs/expansions/expansion_long_line_freight_plan.md`, `docs/plans/EXPANSION_PROGRAM_WAVE9_2026-09-21/PLAN-CONTRACT-BOARD-109.md`, trade-route plans, and the underworld/black-market work. Repository search did not find an earlier plan titled “The Trading House”; that absence does not prove there is no reusable identity abstraction. The owner audit is still required before any proposed type or file is selected.

## 5. Existing Extension Seams

1. **Market reads:** `MarketSystem.State.ledger`, `GetPrice`, `ExplainPrice`, and the economy host session are candidates for projection. Inspect encapsulation and any later API changes before use; avoid directly mutating state collections.
2. **Route reads:** `TradeRouteHostSession.Contracts` / `System.GetContract` and `GetCensus` are current read seams. Existing route events and day owner remain authoritative.
3. **Credit reads:** inspect `LedgerDebtSystem`/debt contract public identifiers and its owning host before defining a reference. `TradeCreditCoordinator` is an offer/accept mediator, not a generic portfolio API.
4. **Black-market reads:** inspect `BlackMarketState`, settlement receipts/results, and their host owner. If a completed transaction lacks durable, unique identity, omit it from durable House references instead of adding an ad hoc ID.
5. **Identity:** search faction, settlement, player, role, and charter APIs at implementation time. Reuse the owner that already persists the relevant institution or character relationship. If it cannot represent a House without changing unrelated semantics, stop and request a signed decision.
6. **Presentation:** if the existing route to a visible surface is approved, add a read-only panel or section to the owning economy/institution navigation and refresh it from existing state-change signals. Avoid a second dashboard, event bus, or host coordinator.

No seam is approved merely because it is public. Stable IDs, lifecycle, ownership, restore order, and caller semantics must all be checked in the selected current source.

## 6. Proposed Architecture

The proposed conceptual feature has three parts, only two of which may require code:

- **Charter facts:** House identity, public charter text or authored charter identifier, and membership/role facts. The charter text belongs in authored content if it is intended to vary by scenario; identity and mutable membership belong to one verified existing state owner. No copied faction standing, settlement ownership, or player identity.
- **Source adapters/read model:** pure projections that accept current owner snapshots or host read interfaces and return rows containing source owner, source key, display label, source status, and read-time facts. Projection code performs no gameplay mutation and no persistence.
- **Presentation:** displays charter and resolved source rows; commands are omitted unless a canonical source command already exists and its authorization model can be forwarded unchanged. A button cannot infer permission from House membership alone.

The initial bounded release should favor static charter presentation plus derived reads. A persistent House activity index is not proposed. If UX needs a user-pinned “watch list,” that is a separate non-authoritative preference and requires its own product decision; it must not be described as ledger truth.

### Candidate read-row shape (illustrative, not an approved API)

`HouseActivityRow` could contain:

- `sourceOwnerId` — canonical subsystem, e.g. `economy_market` or `trade_routes`;
- `sourceRecordId` — stable source identifier, present only where one exists;
- `sourceDay` — copied for display from the source fact, not a second clock;
- `activityKind` — finite display category, not a new gameplay enum unless multiple consumers need it;
- `memberId` — included only if the canonical record supplies it or a verified identity crosswalk can be resolved;
- `status` — resolved, missing, unsupported, or ambiguous; no invented completion state;
- `summary` — presentation text derived from typed facts, not a decision input.

Before introducing the row type, test whether a typed existing DTO can be projected by the panel or host without an abstraction. Do not add generic provider frameworks for one consumer.

## 7. Ownership Matrix

| Concern | Authority | House role | Prohibited duplicate |
|---|---|---|---|
| Player identity | Existing player/survivor authority (verify exact owner) | Display identity only | House-owned player profile |
| Faction/settlement identity and standing | Existing faction/settlement systems | Resolve visible name/relationship if available | New faction registry or standing score |
| Market quote, demand, pressure, market-side transaction record | `MarketSystem`; actual inventory/wallet settlement belongs to its distinct canonical caller/owner | Read market-side facts only; link settlement only through a verified receipt owner | House market simulation, stock cache, inventory/wallet mutation, price formula |
| Player route, cadence, reliability, outcome | `PlayerTradeRouteSystem` / `TradeRouteHostSession` | Link and display route record | House route scheduler or duplicated route contract |
| Posted offers, deadlines, escrow and failure | Contract Board 109 owner, subject to current re-audit | At most source-qualified link/read | Competing board, escrow, deadline owner |
| Freight-company runs | Long Line: Freight owner, subject to current re-audit | At most source-qualified link/read | Second freight company or run table |
| Credit debt and settlement | `LedgerDebtSystem`; `TradeCreditCoordinator` gates requests | Display owner-reported exposure if stable | House principal, interest, default clock |
| Black-market transaction | `BlackMarketSystem` + `BlackMarketSettlementService` | Display source receipt if available | House fence, escrow, inventory, wallet |
| House identity/membership | One currently existing owner, to be identified at P0 | Store only what owner authorizes | New registry/save section without signed decision |
| House activity | Derived projection | Query and label facts | Append-only duplicate economic history |
| UI | Godot panel/host adapter | Display and forward canonical commands | Gameplay rules in controls/callbacks |

## 8. Data Flow

### Read flow

`existing saved owners restore → current host sessions expose owner state → House read adapter requests bounded source facts → adapter resolves stable references or marks them unavailable → view model orders/paginates deterministically → UI renders source label, date, and status`

Refresh triggers should be tied to owner `StateChanged` events or established UI refresh lifecycle. The projection must not tick owners, refresh market prices, or mutate any owner. A missing event subscription may mean the view refreshes on open; do not create a polling loop.

### Charter flow

`authored/static charter identity → verified identity owner loads/restores membership → House surface resolves membership and role → player action (if any) forwards to the canonical owner → owner validates and mutates → canonical owner saves/emits change → view refreshes`

No command is accepted merely because a UI element is visible. Authorization is checked at the owner boundary. If the existing owner cannot validate House roles, membership is informational only in this plan.

### Error flow

Source absent, record deleted, record ID reused, type unknown, or restore incomplete → row is unavailable/ambiguous with source name and a bounded reason → no synthetic balance or terminal state is produced. UI should distinguish “source unavailable” from “no activity.”

## 9. State Model

No new mutable state is currently approved.

At most, the selected existing identity owner may need to persist House membership/role facts. That is a conditional design question, not authority to create a new `HouseState`. Before state changes can be proposed, the implementation preflight must identify:

- current authoritative object and save section;
- whether the House already exists as a faction, settlement, organization, or authored campaign entity;
- identity stability across new game, save/restore, death, faction changes, and scenario starts;
- membership entry uniqueness and removal behavior;
- role vocabulary and default/unknown-role semantics;
- how existing ownership and diplomacy gates interact;
- migration for old saves and unknown future versions;
- reset and campaign restart behavior.

The derived ledger has no lifecycle or state migration because it is reconstructed. Its references are not historical guarantees. If a source can disappear or reuse an identifier, that row becomes missing/ambiguous and must not silently bind to another record.

## 10. API / Contracts

The only currently supportable API contract is conceptual:

```text
ReadCharter() -> current charter projection, if a canonical identity owner exists
ReadActivity(filters, page) -> rows resolved from available canonical owners
```

These names are examples only. Do not add them until P0 demonstrates a caller and a stable provider contract. Prefer direct calls through an existing host session when the same view has one consumer.

If identity membership is already modeled, expose existing read accessors and commands. If not, stop. A new Core contract is justifiable only if multiple consumers need the same invariant and its owner is approved. UI event names, new status enums, IDs, and source crosswalks are not settled by this draft.

Source ID qualification should be `(owner type, source key)` rather than a naked string. A composite value does not solve ID reuse; source lifecycle and uniqueness must be verified. Do not assign a generated `Guid` or hash-derived key in deterministic Core.

## 11. Data Changes

No JSON file change is planned by default. First inspect existing faction, settlement, organization, and scenario catalogs and their loaders/validators. If a charter needs authored text, prefer an existing data owner/file and schema over a new parallel catalog. New content must use snake_case IDs and participate in current catalog integrity validation.

Data acceptance, if later justified:

- stable House ID points to a canonical entity;
- member IDs resolve to the existing person/agent authority;
- role IDs resolve to a closed authored vocabulary;
- optional/missing charter content degrades to a known neutral state;
- no price, stock, inventory, route, debt, credit, tariff, or production values are authored as House-owned copies;
- duplicate IDs and dangling references fail the owning validator with row-specific diagnostics.

The House must not become a second authored source for active factions, trader inventories, routes, or contract templates.

## 12. Save / Load

**Default:** no new House save section. The activity view is derived from canonical owners after restore. This avoids stale duplicated records and eliminates House-specific migration for transactions it does not own.

**Conditional identity:** if P0 proves House membership cannot fit an existing owner, the plan must return for a signed architecture decision before implementation. A future revision must specify section ownership, DTO version, capture/restore order, empty/null behavior, checksum envelope, forward-version rejection, old-save migration, rollback, and round-trip coverage. This draft does not authorize adding a `house` section or nesting membership in an unrelated section merely to avoid a decision.

Do not persist serialized market rows or route/contract copies. A source reference can be persisted only if a user-visible durable bookmark is separately approved and source IDs are stable; it remains a pointer, never the source fact.

## 13. Determinism

The read model performs no stochastic behavior. It must:

- sort by an explicit stable tuple such as source day, owner ID, and stable source ID;
- use ordinal/culture-invariant comparisons for machine IDs and output formatting;
- handle duplicate IDs by marking ambiguity, not relying on dictionary enumeration order;
- avoid current wall-clock timestamps as activity ordering or persisted identity;
- avoid `System.Random`, `Guid.NewGuid`, hash iteration, or UI-local pricing math.

Existing owners keep their RNG contracts and tick order. The House does not subscribe as a day owner and does not change deterministic state, so paired simulation hashes should be identical with the read surface enabled/disabled.

## 14. Event / System Wiring

No new day event, simulation tick, transaction event, or cross-owner command is proposed. Read refresh can listen to existing owner/session state events. If no consistent event exists, refresh-on-open is adequate for the first read surface.

Potential event order hazards: market row added before inventory/wallet post-commit; route outcome recorded before tariff settlement; debt principal grant compensated after failed signing; black-market inventory/wallet mutation coordinated with source offer state. The House must not infer a successful transaction from a preflight or offer alone. Display only a completed source fact or explicit source-owned status.

Lifecycle must unsubscribe on panel disposal or session replacement. Restore must complete before the view projects source state. Empty or unavailable sources yield no rows plus an availability label; they do not block other sources.

## 15. Godot Integration

No UI file is pre-approved. If implementation is later authorized, first identify the currently owned economy/trade navigation route and reuse its established panel lifecycle. The view should display:

- House name/charter text and resolved current membership;
- source-filtered activity, with clearly visible owner and source day;
- empty state distinct from missing-source state;
- a route to open the canonical source surface where that route already exists.

No input action should mutate an owner in the initial charter/ledger scope. If future actions are added, use one explicit button per canonical owner command, preserve back/close/focus behavior, show refusal reason from the owner, and disable commands when the owner is absent. Never simulate a contract accept by changing a House row.

The UI cannot calculate market price, credit owed, total assets, route success, freight revenue, debt status, or black-market settlement. It may format owner-provided fields only. Avoid presenting aggregate “House wealth” until there is a signed rule for valuation, currency conversion, liabilities, and temporal consistency.

## 16. Narrative / Content Integration

The House can support short charter language, membership ceremony, and institutional friction, but those are authored facts and choices, not proof that an economy event happened. Any reactive dialogue must read a canonical fact and state its source.

Candidate narrative hooks (not approved content additions): a member asks for a record correction; a trader disputes the name attached to a route; a ledger clerk refuses to write an exchange as completed before the receipt arrives. These beats can teach source-of-truth rules without inventing economic outcomes.

Do not add faction standing, quests, embargoes, rivalries, reputation, market demand, or endings in this package. The narrative continuity and authored IDs must be audited before a separate content plan is produced.

## 17. Failure Modes

| Condition | Required behavior |
|---|---|
| No suitable identity owner | Stop P0; no registry or save section is created. |
| No House exists in current campaign | Show unavailable/locked authored state or omit the view; do not create it implicitly. |
| Optional source host absent | Keep charter readable; label source unavailable; do not fabricate zero activity. |
| Source record deleted | Show missing reference only if a stable pointer was legitimately stored; never keep a phantom “active” transaction. |
| Source key reused | Mark ambiguous if detectable; do not bind silently to new activity. |
| Market transaction has no stable row key | Use a read-only session slice only while present or omit cross-session link; do not synthesize a persisted ID. |
| Duplicate source IDs | Surface ambiguity and block command forwarding for the ambiguous record. |
| Owner restore incomplete | Defer projection until owner signals ready; no eager empty state interpreted as settled. |
| Member/faction changed or disappeared | Resolve live identity; preserve owner facts; show unresolved actor, not a guessed replacement. |
| Corrupt/unknown source shape | Skip that source, record bounded diagnostics, keep other sources available. |
| Large source histories | Page or cap deterministically; never truncate silently or change canonical retention. |
| UI disposed/reopened | Unsubscribe/resubscribe without duplicate event handlers or cross-campaign cached rows. |
| Save during source mutation | Rely on source transaction/save guarantees; House adds no half-written mirror. |
| Culture or locale differs | IDs/order remain stable; formatting is presentation-only and does not affect checksums. |

## 18. Test Strategy

No tests should be added until implementation scope and exact current public seams are verified. If this plan is executed, first search for equivalent tests. Use `bin/run-scoped-tests` only; no full suite unless the user types exactly `RUN FULL TESTS`.

Expected focused checks, subject to the existing test harness and target names:

1. Market projection reports only rows from canonical `MarketSystem` data and does not mutate captured state.
2. Route projection reports owner values and changes when canonical contract state changes; it does not advance a run or write a route save.
3. Source references are qualified, deterministic, and stale/ambiguous refs fail closed.
4. Absent optional owner does not prevent the remaining projections from working.
5. Repeated read and event refresh do not duplicate rows or listeners.
6. Save/restore of source owners followed by projection yields the same ordered read model; House creates no extra save section.
7. Paired seeded campaign behavior is unchanged by constructing or omitting the read-only view, using the smallest existing deterministic target if available.

Host/UI checks are needed only if a Godot surface is added: route reachability, focus/close lifecycle, owner-unavailable state, and no UI-side rules. Do not add a headless probe solely to echo a simple projection unless current project policy requires it.

## 19. Dependency-Ordered Phases

### Phase 0 — Premise and collision audit (mandatory)

Re-read `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, and `AI_AGENT_WORKFLOW.md`; identify current package/claims; inspect exact faction, settlement, player, market, route, debt, contract-board, freight, black-market, save, and panel owners. Search tests and data for equivalent House, guild, trader association, or activity-history semantics. Record exact evidence and file paths. **Gate:** suitable identity owner exists or stop for a signed decision; stable source IDs are documented; overlapping packages are explicitly excluded.

### Phase 1 — Read-contract proof

Build a tiny design proof using current read APIs only; no production files are selected until paths are claimed. Verify that each source can be queried without mutation and determine which has stable identity. Exclude any source that cannot meet the reference contract. **Gate:** all displayed columns have a source field and lifecycle.

### Phase 2 — Identity-owner extension (conditional)

Only if an existing owner supports the charter/member facts, define its minimal additive DTO/API extension. Preserve owner, schema, defaults, old-save behavior, and restore lifecycle. Do not create a separate House store. **Gate:** ownership is approved by foreman and path claims do not overlap.

### Phase 3 — Derived read model

Add the narrowest projection in the current owning Core/host seam, or keep it host-only if it is pure presentation adaptation. Do not add a general event index. **Gate:** deterministic output, source-qualified rows, no source mutation.

### Phase 4 — Presentation

Bind a read-only surface to existing navigation and lifecycle. Do not add commands in this phase. **Gate:** unavailable/empty/ambiguous states are distinguishable and accessible.

### Phase 5 — Focused verification and handoff

Run only changed-area targets via `bin/run-scoped-tests`; if the change affects Godot runtime, use the bounded headless check selected by the integrator. Report exact source contracts, tests, save impact, and limitations. No full-suite run by default.

At every phase, do not touch Long Line: Freight, Contract Board 109, TradeCreditCoordinator semantics, market formulas, routes, black-market transaction ownership, trade content, or unrelated save registries.

## 20. File Impact Map

All paths below are **candidate areas only**, not current claims. Exact ownership and names require Phase 0; no listed path may be edited until the live ownership ledger is checked.

| File/area | Action | Reason | Risk |
|---|---|---|---|
| `Assets/Ashfall.Core/Economy/*` | READ ONLY first; MODIFY only a verified existing identity/read owner | Establish market/route/credit source DTO contracts | High if a parallel authority is introduced |
| Existing faction/settlement identity Core and data files (exact path TBD) | READ ONLY, then conditional MODIFY | Determine whether House charter/membership already has an owner | High; unknown owner is the central unresolved premise |
| Existing identity owner DTO/save store/registry (exact path TBD) | Conditional MODIFY only if approved | Persist only member/charter fields within that owner | High migration and ownership risk |
| `src/Host/EconomyHostSession.cs` | READ ONLY; conditional small MODIFY | Reuse current market session/read events if needed | Medium; do not grow into coordination owner |
| `src/Host/TradeRouteHostSession.cs` | READ ONLY; conditional small MODIFY | Reuse canonical route reads | Medium; avoid route behavior changes |
| Existing Main navigation/panel files (exact path TBD) | Conditional MODIFY | Expose read-only charter/ledger | Medium UI lifecycle/focus risk |
| `Ashfall.Core.Tests` focused economy/identity targets (exact path TBD) | Conditional CREATE/MODIFY after duplicate search | Verify projection invariants and source linkage | Low/medium; avoid duplicate tests |
| `Assets/StreamingAssets/Data` relevant catalog (exact path TBD) | No change by default | Authored charter only if current data authority supports it | Schema/catalog drift |
| `SaveSectionRegistry`, generated matrices, docs indexes | NO CHANGE by default | No new section or generated artifact proposed | Shared governance/generated ownership |
| `docs/chatgpt-claude-assisted/trading-house-1-charter-ledger.md` | This draft only | Architecture proposal | DRAFT; no plan approval |

## 21. Risks

- **False institutional ownership:** “House” can accidentally become a new faction or wealth authority. Mitigation: no feature mutation until identity owner is named; no generic registry.
- **Identity aliasing:** different systems may use faction, settlement, counterparty, debtor, and route IDs for related entities. Mitigation: source-qualified IDs and audited crosswalk; unresolved aliases remain unresolved.
- **False financial totals:** combining items, credits, route tariffs, contract promises, or currencies can imply a balance that no owner calculates. Mitigation: no aggregate assets/value metric in TH-1.
- **Unstable references:** market `LedgerEntry` has no visible explicit record ID in inspected DTO. Mitigation: no cross-save reference commitment until identity is proven.
- **Overlap with existing packages:** Long Line: Freight and Contract Board 109 have contract/run language. Mitigation: clear boundary and premise review at implementation start.
- **Restore order:** derived view queried too early can show empty/settled data. Mitigation: wait for owner readiness or refresh on open after restore.
- **Scope drift into permissions:** House membership can be mistaken for authority to accept contracts or trade. Mitigation: owner validates every action; initial release is read-only.
- **Overengineering:** generic adapters, event bus, index cache, migration, and new section could be built without evidence. Mitigation: direct read path first; add abstractions only for actual multiple consumers.

## 22. Out of Scope

- Freight company ownership, dispatch, convoy runs, cargo, rates, loss, road conditions, and reputation (Long Line: Freight).
- Posted offer creation, escrow, deadlines, failure consequences, or board lifecycle (Contract Board 109).
- Loans, principal, interest, credit eligibility, debt collection, default, repayment clock, or guarantees (`TradeCreditCoordinator` and canonical debt owner remain unchanged).
- Market prices, stock, demand, item availability, barter, inventory, wallet, tariffs, route creation, cadence, risk, or settlement mechanics.
- Black-market brokerage, fences, syndicate state, heat, contraband, loan settlement, or inventory custody.
- Production recipe chains, storage, output, warehouse ownership, delivery receipt, and quality control (TH-4 is conditional and separate).
- Faction diplomacy, standing, war, embargo, governance, law, education, faith, narrative outcomes, or scenario design.
- New save section, economy history store, cross-owner transaction broker, crosswalk database, background polling, or general-purpose ledger framework.

## 23. Rollback Strategy

The preferred initial change is presentation-only and reversible: remove the House surface/read adapter while leaving canonical owner data untouched. If a verified existing identity owner is extended, retain its old-save defaults and use an additive schema change that reads old versions before enabling the panel. Rollback must not delete canonical activity or rewrite market/route/debt saves.

If source compatibility cannot be preserved or identity migration would drop member facts, pause before implementation and return with a migration decision. Do not add fallback writes to a House mirror. Any generated indexes or manifests must be updated by their owner generator, not hand-edited.

## 24. Definition of Done

- Current evidence and all duplicate/overlap candidates have been rechecked against source, data, tests, live plan ledger, and path claims.
- One existing identity owner is named, or the implementation is explicitly stopped pending a signed architecture decision.
- Every view row has a canonical source owner; unsupported/ambiguous records fail closed.
- House reads cannot change economy, routes, credit, inventory, faction standing, or campaign time.
- The ledger is derived and creates no new save section or persistent duplicate history.
- UI refresh, owner absence, restore order, focus, close, and disposal behavior are specified and verified if a panel is shipped.
- Focused tests use `bin/run-scoped-tests`; exact command and result are reported. Full suite is not run unless the user types exactly `RUN FULL TESTS`.
- Relevant path claims and an approved execution plan exist before code edits; this DRAFT itself grants neither.
- Boundaries with Long Line: Freight, Contract Board 109, TradeCreditCoordinator, canonical market/route/black-market owners are preserved.

## 25. Implementation Handoff

### MUST PRESERVE

- One canonical owner for each market, route, debt, inventory, wallet, faction, settlement, and black-market fact.
- Existing transaction, save, deterministic RNG, and day-owner behavior.
- Source-specific status semantics; a House projection is never stronger evidence than its source.
- The explicit unresolved status of House identity until the owner audit is complete.

### MUST ADD

- Only if justified: the smallest projection or additive field in a verified existing owner; a readable, source-labelled House view may follow.
- Failure states for absent, stale, ambiguous, or unsupported sources.
- Focused tests for no mutation, traceability, deterministic ordering, and source restore behavior.

### MUST NOT DO

- Create `HouseSystem`, a second faction/member registry, House ledger/save section, mirrored contract rows, market/funds/credit balances, new event bus, transaction broker, or daily tick without a signed decision.
- Mutate Contract Board 109, Long Line: Freight, `TradeCreditCoordinator`, market or route semantics under this package.
- Infer “wealth,” “profit,” “completed,” “defaulted,” or “settled” from an offer or copied display field.

### VERIFY WITH

- Exact current owner APIs and save paths; duplicate-content and API search; current `WORKTREE_OWNERSHIP.md` claims.
- Focused Core/host tests through `bin/run-scoped-tests`; a Godot headless check only if the actual runtime surface changes.
- Deterministic comparison proving the read-only House view changes no canonical campaign state.

### FIRST SAFE IMPLEMENTATION STEP

Perform Phase 0 as a read-only audit. Name the existing identity owner and stable source keys in a short evidence note. If identity has no suitable owner, stop and request the foreman’s architecture decision before editing production files.

## Appendix A — Concrete Read-Model Cases

These cases make the source-of-truth rule testable. They do not assert that every row is currently supported; a case marked conditional stays out of the first implementation until the relevant source API passes Phase 0.

| Case | Source facts available | House result | Forbidden inference |
|---|---|---|---|
| A market purchase is present in `MarketSystem.State.ledger` | Day, item, quantity, unit/total value, counterparty | Display a market activity row with a source label. If no stable row ID exists, it is a current projection, not a durable link. | Do not infer present inventory, current market stock, or a balance owed. |
| A route is active | Route owner returns contract and current schedule/status | Show route name/ID and owner-reported activity counters. | Do not turn run counts into independent shipment receipts. |
| A route is cancelled | Canonical contract exposes suspension/cancellation facts | Show cancelled/suspended according to the owner contract. | Do not retain it as an open House obligation. |
| A credit offer is displayed but unaccepted | `TradeCreditCoordinator.TryBuildCreditOffer` produced a projection | No debt row; optionally link to the canonical credit flow. | Do not treat offer principal as borrowed goods or debt. |
| A credit contract is signed | Canonical ledger reports signed debt | Display debt only if stable canonical debt ID/status can be read from the ledger owner. | Do not compute a second interest or due-day clock. |
| Black-market preview succeeds | Preview confirms current quote and preconditions | Label as an available action only in that source surface. | Do not count it as a House transaction. |
| Black-market settlement succeeds | Service returns canonical transaction result | Display result, or source receipt if one is stable and durable. | Do not reconstruct a history from UI text. |
| Source owner has not loaded | Host readiness is false/unknown | Display source unavailable or wait until ready. | Do not display a truthful-looking zero. |
| Market ledger row cannot be uniquely addressed | Multiple rows share observable fields or no row key | Show aggregate source count only if directly available, otherwise omit the link. | Do not mint a key by concatenating day/item/quantity/value; legitimate repeated purchases collide. |
| Counterparty ID does not resolve | Source retains an ID but identity owner has no current match | Show unresolved counterparty with raw stable owner-qualified ID if safe. | Do not infer entity by matching display name. |

### A.1 Read-model invariants

An implementation should be reviewable against these invariants rather than broad UI screenshots alone:

1. **Projection purity:** calling the query leaves all captured owner state byte-equivalent or semantically equal under the owner’s existing snapshot comparator.
2. **Provenance:** every fact in a rendered row can be traced to a named owner field or an explicitly documented read-only mapping.
3. **No copied truth:** removing a source record removes or marks unavailable its projected row on the next query; no House state preserves completion by accident.
4. **Identity clarity:** source keys are owner-qualified. Display names never serve as keys.
5. **No deceptive totals:** a count labeled “activity rows” is a row count; it is not currency, wealth, volume, or obligations.
6. **Stable view:** the same owner snapshots produce the same ordered rows across hosts and locales.
7. **Partial availability:** one missing source cannot erase or prevent rows from other ready sources.

### A.2 Identity premise questions

The P0 identity audit should answer these questions with file and symbol citations in its execution record:

- Is the proposed House a faction, settlement, player organization, trader group, or solely a user interface? Which current type owns that concept?
- Are counterparties globally unique IDs or catalog IDs scoped to a catalog/faction? Can two owners use the same string safely?
- Does membership already exist in a faction/settlement membership list or survivor relationship? Is it authoritative or merely a read model?
- Can a character belong to more than one institution? If so, do current systems have semantics for that, or would “member” overload a single-owner affiliation field?
- What happens when a member dies, leaves, changes faction, is recruited, or becomes unavailable? Is House membership independent from those facts under the current owner?
- Which campaign starts should contain the House? Does authored scenario content already state this, and is the owner restored before UI exposure?
- Does the existing save envelope support additive membership without changing global faction/save semantics?
- Is the charter player-authored, authored by content, or fixed fiction? Which one source owns the wording?

If the answers require novel multi-membership, a new organization graph, or a second affiliation model, this draft has reached a decision boundary. The answer is not to silently add a registry.

## Appendix B — Review Checklist for Source Traceability

For every additional source adapter, reviewers should be able to complete one compact record:

```text
Source owner:
Source file and public API:
Source state and save owner:
Stable record key and uniqueness scope:
Record removal/reuse behavior:
Owner readiness/restore signal:
Projected fields and exact source fields:
Fields omitted because they are not authoritative:
Owner event/refresh seam:
Failure/ambiguity display:
Focused test target:
```

An unanswered key or lifecycle field means that source is not yet included. This prevents a future implementation from filling gaps with guessed joins or copied state.

## Appendix C — Owner-by-Owner Projection Contract

This section records the known limits of each likely source in enough detail to prevent implementation from treating similarly named records as interchangeable. “Observed” describes the inspected source on the draft date; “VERIFY” means it must be rechecked against the live branch and claims before implementation.

### C.1 MarketSystem

**Observed facts:** `MarketState` is versioned and carries its own ledger. `MarketSystem.Buy` and `Sell` resolve a catalog good and positive quantity, calculate the current buy-side price explanation, append a signed `LedgerEntry`, update category trade pressure, and raise economy/state events. `MarketSystem.Barter` computes an equal-value exchange with whole-unit flooring and an explicit remainder, then appends two accounting legs and pressure. These Core methods do not debit a wallet, consume player inventory, grant player inventory, or update a distinct settlement receipt.

**Projection promise:** the House may say “market book records a buy/sale/barter on day D.” It may not say “the player received Q units,” “the counterparty was paid,” “goods are now in House custody,” or “this request has cleared” on the basis of this row alone. In particular, a market ledger line is not a generic immutable transaction ID; equal same-day repeated rows can have identical visible values.

**VERIFY before adaptation:** enumerate all call sites that invoke MarketSystem methods and determine which adapters transact against inventory/wallet before or after the market row; check whether exceptions can leave a market row without a settlement; inspect any transaction result correlation or caller-owned receipt. If a caller performs separate mutable writes, that caller’s transaction owner—not the House—must define the evidence of success.

### C.2 PlayerTradeRouteSystem / TradeRouteHostSession

**Observed facts:** route contracts are keyed by route ID in a case-insensitive dictionary. `RegisterContract` assigns by key and therefore replaces any existing contract under the same normalized key. `RecordRunOutcome` mutates the contract’s completed/failed/reliability/next-run fields when a contract exists; it silently does nothing for an unknown key. Contract save state carries route/counterparty IDs and route terms, but there is no per-run receipt list in the inspected contract DTO. `TradeRouteHostSession` has a separate authored route catalog and a daily tick method.

**Projection promise:** `GetCensus` can support an aggregate “active contracts/completed runs/failed runs/tariff chits” display, but those counts are the route owner’s census, not an auditable per-exchange history. A route contract may be linked as an ongoing arrangement only when its owner-qualified ID and replacement behavior are accepted; individual run claims cannot be represented unless the route owner exposes stable run records.

**VERIFY before adaptation:** confirm IDs are canonicalized consistently at save/restore and input; determine if source route IDs can change by authored catalog revision; determine whether there is a safe removal/replacement event; inspect the host session lifecycle around contract restoration. Do not rely on dictionary iteration order for UI order.

### C.3 Faction standing and identity

**Observed facts:** `FactionWarSystem` exposes faction standing/control/hostility/alliance records. Those records express player-to-faction geopolitical relation; they are not member lists. `FactionStandingIdResolver` canonicalizes some IDs. `HoldfastFactionsCatalog` is an immutable-after-load catalog of authored faction identity, alignment, region, wants/offers, trust and presentation fields; catalog entries are not a mutable organization membership authority. `SettlementState` inspected in `SettlementCatalog.cs` holds quest cooldowns and completion counts rather than settlement governance or members.

**Projection promise:** a House surface may use the existing catalog/faction read to show a faction name and the standing owner to show a relationship, with explicit labels for those separate facts. It may not interpret allied standing as House membership, import the authored `trust` field as player trust without an owner contract, or turn settlement quest counts into settlement participation.

**VERIFY before membership proposal:** search current source for additional member/charter/organization owners beyond these likely catalogs. Inspect the current `FactionWarSystemState` save path, settlement/outpost save owners, Holdfast session ownership, and any “crew” or “faction branch” structures. A character’s branch, ideology, faction standing, contract counterparty, and membership are separate dimensions unless code explicitly models them as one.

### C.4 Debt and funds

**Observed facts:** `LedgerDebtSystem` stores a current contract lookup by debtor ID plus closed contracts. The contract DTO has debtor/creditor/template, principal, term, rate, signed/read/payment/forfeit state, but the inspected shape has no immutable contract record key. It exposes events for signed, paid, forgiven, renegotiated, and forfeit changes. `TradeCreditCoordinator` binds to a debtor ID, so its existing path is specifically a player-debtor request rather than a House account. `FundsLedger` stores a balance and bounded movement records; at 128 movements it evicts the oldest. Movement records include day, delta, resulting balance, reason key, and source ID, but source ID is not demonstrated to be a globally unique transaction key.

**Projection promise:** a “current exposure” view can only show canonical debt owner fields after its exact host ownership is known; it cannot reconstruct lifetime credit history from a capped funds movement list. It cannot sum nominal chits, item principal, market values, barter values, tariff chits, or debt obligations into one House net-worth number without a signed valuation model.

**VERIFY before adaptation:** identify the authoritative debtor identity for a House (the current coordinator has a fixed debtor ID); confirm save owner/restore order; find whether debt records can be referenced uniquely across paid/forgiven contract replacement; determine whether any aggregate debt read API exists. Do not infer due or unpaid state from `FundsLedger` movement names.

### C.5 Black-market settlement

**Observed facts:** the settlement service couples black-market quote/stock staging with wallet and inventory transaction callbacks. This remains a domain-specific adapter and produces a structured result. The inspected service call does not by itself establish that a globally durable, non-reused receipt key is stored for every action. `FundsLedger` movement retention is bounded.

**Projection promise:** display the immediate source result or a durable black-market row only after proving its stable ID and save lifecycle. Do not reconstruct past actions from wallet movement text or UI “LastEvent.”

**VERIFY before adaptation:** inspect `BlackMarketActionResult`, host session owner, save lifecycle, exact inventory/wallet compensation behavior, and tests that force refusal/exception. Avoid adding a second House result log to compensate for missing source receipts.

## Appendix D — End-to-End Read Scenarios

### D.1 A House member opens a ledger after save restore

1. Main restores economy, route, black-market, credit, and identity owners through their existing save sections.
2. The House view waits for those owners' ready states or opens after the current established restore barrier.
3. It queries the market owner and receives three ledger rows, then queries the route owner and receives one route contract summary.
4. It orders by source-defined day and qualified ID; when market row IDs are unavailable, it presents a non-linkable market entry in the current snapshot.
5. User sees “Market record” versus “Active route contract,” not a combined House obligation total.

**Acceptance evidence:** owner snapshots before/after query are unchanged; missing market row identity prevents persistence; per-source totals match owner-reported counts; the same restore yields the same order. **Failure path:** if identity owner did not restore, the view remains unavailable rather than treating all user actions as authorized.

### D.2 A user sees an available market quote

1. The host calls `EconomyHostSession.ExplainPrice` for the selected item and side.
2. The view renders typed price factors returned from Core.
3. It does not call `MarketSystem.Buy` just because a quote is visible; that call would append market-side history/pressure and still would not transfer inventory or wallet value.
4. If the player proceeds through a separate existing transaction panel, that owner performs its own transaction and House can later project only the canonical facts available.

**Acceptance evidence:** opening/refreshing the House view leaves market ledger count/pressure unchanged. **Failure path:** unknown item or absent catalog reports unavailable quote without a transaction record.

### D.3 A House row points to a route that is replaced

1. House reads route `R` from `PlayerTradeRouteSystem` and stores no duplicate contract.
2. A canonical owner later registers another contract under a case-insensitive ID equal to `R`.
3. Since registration replaces by key, the House cannot prove the new route is the historical contract previously seen.
4. The view marks an old durable pointer ambiguous unless owner contract semantics establish identity continuity.

**Acceptance evidence:** identity is not assumed from matching text; no mutation/cancellation occurs through the House row. **Stop condition:** if product requires permanent audit history of replaced route contracts, that requirement must be handled by the route owner in a separate approved scope.

### D.4 A black-market contract changes state while the House view is open

1. House renders an owner-qualified read-only status.
2. The canonical black-market surface performs its transaction and emits its existing state change.
3. House refreshes from the owner or becomes stale/unavailable until reopened.
4. The House never intercepts partial wallet/stock updates or attempts to reconcile them.

**Acceptance evidence:** two surfaces converge on the canonical owner after refresh; no event subscription is duplicated after close/open. **Failure path:** if no stable settlement record exists, House removes the live activity row after its source facts expire rather than preserving a guessed outcome.

## Appendix E — Migration and Recovery Decision Tree

| P0 finding | Action | State migration |
|---|---|---|
| Existing identity owner already contains House/organization identity and membership | Reuse it; add no identity save fields unless exact needed field is demonstrated | None by default |
| Existing owner contains faction membership but cannot represent multiple organizations | Stop; request product/architecture decision | No speculative array or shadow relationship graph |
| No House entity exists, but user only needs a read-only economy digest | Treat “House” as presentation grouping; author text through an existing content owner if possible | No state migration |
| No House entity exists and membership must be campaign-persistent | Stop for signed ownership/save decision | New section prohibited until owner is designated |
| Canonical source provides stable record ID and status | Use source-qualified reference if an existing owner can hold it | Additive optional field only under owner plan |
| Source has no stable ID or bounded/overwritten log | Show snapshot/read-only aggregate, or omit it | Do not synthesize IDs or migrate guessed links |
| Existing saves lack optional pointer field | Resolve to empty/no House link | Preserve all source saves exactly |
| Pointer refers to unknown source after update | Show unavailable; retain no inferred status | Migration cannot delete or recreate source facts |
| Source schema is newer than runtime can read | Surface the source owner’s load failure; do not synthesize fallback data | Forward-version behavior remains with source owner |

The migration strategy is intentionally conservative: new House code can be removed without rolling back economic state. If a proposed change cannot meet that property, its ownership and rollback contract is not yet sufficient.

## Appendix F — Holdfast Trade Boundary

`HoldfastTradeSession` is a significant neighboring owner and must be part of the P0 inventory, because it has transactional behavior that `MarketSystem` does not.

### F.1 Observed behavior

The class owns a catalog, merchant stock/held goods and a value balance. `Buy` validates item/faction/embargo, quantity, stock, price, funds, and inventory capacity; it adds inventory, reduces value and merchant stock, updates held state when no player inventory is bound, and returns a `HoldfastTradeResult`. Its exception handler removes the just-added item and restores value/stock/held quantities for exceptions inside its mutation block. `Sell` verifies held quantity and bounds, updates value/held/stock, removes inventory, and returns a result. Funds-denominated variants call `FundsLedger.TryDebit`/`TryCredit`; buy attempts to credit back if inventory add fails. The owner’s save DTO captures `value`, `held`, and `stock`; the `HoldfastTradeSaveStore` owns a checksummed envelope and backup behavior through the host.

The inspected save DTO does not contain a durable transaction history or per-operation receipt ID. A returned `HoldfastTradeResult` describes the immediate action, but the type has no unique operation key. The held/stock/value snapshot tells current state, not which transaction changed it.

### F.2 Projection consequence

The House may present a Holdfast trade action through its canonical terminal and show the immediate result. TH-1 cannot reconstruct an audited House ledger from current `HoldfastTradeSaveState`; it should not label current merchant stock/value as House-owned. If the owner later exposes a durable event/receipt under its own plan, the House may query that source. A non-persistent `LastEvent`, UI message, movement record source label, or saved inventory delta is not an authoritative transaction history.

### F.3 P0 checks

- Verify whether `HoldfastTradeSession` is currently the intended active source for the player-facing trading terminal and which `HoldfastRuntimeSession` instance owns it.
- Inspect all `Buy`/`Sell` call paths, including use of standalone versus canonical player inventory and legacy held inventory; do not merge their semantics in the House.
- Establish whether `_value` is still active campaign authority, how it relates to `FundsLedger`, and which exact currency mode applies to each surface.
- Inspect transaction exceptions and state callbacks: the current `BuyWithFunds` path debits before inventory add and compensates by credit; exact exception behavior of both components must be checked. `SellWithFunds` credits before mutating other state; the owner test contract should establish whether subsequent operations can fail.
- Confirm save/load/backup and legacy migration (`InventoryMigrator`) semantics before any linked UI reports post-restore activity.
- Search `HoldfastTradeSessionTests`, `HoldfastFundsTradeTests`, `HoldfastTradeIntegrityTests`, and host integration tests for duplicate-submit, exception, save round-trip and conservation coverage.

If the requested “ledger” is merely a current statement of balances and inventory, those values must be attributed to their own owners and may be shown as a snapshot. If the product requires historical explanations or per-consignment audit, the missing durable transaction identity is an owner-level gap and needs a separately approved Holdfast plan.

## Appendix G — Source Evidence Ledger (Planning Run)

| Planning question | Evidence found | Still VERIFY |
|---|---|---|
| Is there already one mutable House/faction-membership owner? | Obvious candidates are different concerns: `FactionWarSystem` stores geopolitical standing/control; `HoldfastFactionsCatalog` loads authored faction identity and wants/offers; `SettlementState` stores quest cooldown/completion counters. These inspected types do not provide the requested House membership contract. | Complete search across current branch for organization/settlement government/personnel owners. Confirm which paths are active in the current campaign. |
| Can the market ledger act as a complete transaction history? | Market rows lack immutable IDs and `MarketSystem.Transact` updates market ledger and pressure only. | Inspect any caller-specific wallet/inventory transaction and whether it has receipt correlation. |
| Can route contracts act as a permanent contract history? | Route dictionary is keyed by route ID, register can replace, and contract state tracks run counters without per-run records. | Confirm whether any lifecycle guarantees prevent replacement/removal in current host. |
| Can funds movements act as a complete ledger? | `FundsLedger.MaxMovementLogCapacity` is 128; oldest row is removed on append when full. | Identify all callers that can provide stable source IDs; history is intentionally bounded. |
| Can debt records support historical House pointers? | Current debt is looked up by debtor ID; settled/forgiven records are moved into closed list. No separate contract ID appears in the inspected DTO. | Inspect host save section, one-debtor/one-active-debt semantics and whether record identity is needed externally. |
| Is there a current House charter in data? | No separate Trading House plan was found in the initial duplicate sweep; Long Line has a distinct authored `house_charter.json` proposal for freight progression. | Do not copy or reuse freight charter semantics without an explicit design choice; inspect loaded data/catalogs for an existing institution. |

The evidence supports an institution read surface, not a persistent cross-owner history, at this time. If later source inspection changes that conclusion, revise the specific contract rather than expanding a parallel House model.

## Appendix H — Proposed Projection Contract Inventory

This inventory spells out what a first read-only version could resolve without introducing a transaction abstraction. Columns shown as “VERIFY” must not be filled from assumptions. An omitted field should remain omitted; a plan is not improved by inventing a synthetic source identity.

| Candidate owner | Read surface evidenced | Durable source state | Potential display data | Unknowns that block durable link |
|---|---|---|---|---|
| Market | `MarketSystem.State.ledger`; `ExplainPrice` | `MarketState` v3 in existing economy save | Day, item, signed market quantity, market-side unit/total value, counterparty | No explicit `LedgerEntry` ID; no player/custodian ID; market mutation does not move inventory/funds; retention policy and index behavior require audit |
| Player routes | `TradeRouteHostSession.Contracts`, `Census`, `GetContract` | `PlayerTradeRouteSaveState` in `trade_routes` save | Route ID, counterparty ID, schedule, goods in/out, reliability and aggregate outcomes | Route ID replacement/case behavior; contract removal; no individual run receipt collection; member who established route not present in inspected DTO |
| Caravan network | `CaravanTradeNetworkSystem.Caravans`, `FindManifest`, read-only `Caravans` | `CaravanTradeNetworkSave` with manifest IDs, routes, statuses, stock, days, hazard outcome, escort | Manifest status, expected/actual arrival, route/faction, current stock (source-owned) | Whether manifests are pruned/replaced; transaction-level barter ID absent; stock is current state rather than history; member attribution absent |
| Quote commit record | `CaravanAtomicTrader.Committed`, `IsCommitted` | `CaravanTradeState.Committed` | Quote ID, offered/requested items and quantities, day, region, stance | Quote ID generator/namespace and collision/reuse policy; whether record means accepted commitment or merely quote commit; downstream inventory/funds transfer not in this type |
| Holdfast trade | Owning `HoldfastTradeSession` host/session | `HoldfastTradeSaveState` current balance, held, stock; inventory and FundsLedger have separate owners | Immediate action result and current state snapshot | No durable per-operation ID/history; active value mode; shared session instance/lifecycle; no verified link back to each market row |
| Black market | `BlackMarketSystem.State` and settlement service result | BlackMarket save plus Inventory/Wallet owners | Catalog entry, syndicate, current stock/access, immediate action outcome | Settlement receipt key and retention; action result persistence; actor/member attribution |
| Debt | `LedgerDebtSystem.Contracts` / `ClosedContracts`; coordinator results | LedgerDebt state save owner (exact host section `VERIFY`) | Debtor, creditor/template, principal, term/days, signed/paid/forgiven/forfeit facts | No immutable debt ID in inspected DTO; House-as-debtor unsupported by current fixed debtor adapter; current vs historical contract identity |
| Funds | `FundsLedger.Balance`, `Movements` | Funds save owner (exact section `VERIFY`) | Current chit balance and recent movement rows | Bounded 128-row history; caller source IDs may be reused; no cross-resource conversion |
| Faction identity/standing | `HoldfastFactionsCatalog`, `FactionWarSystem` | Catalog is authored; faction standing lives in FactionWar state | Display identity and separately labeled relationship | No membership relation; id resolver behavior differs by domain; settlement/faction alias not necessarily one entity |
| Settlement identity | `SettlementCatalog`; `SettlementState` | Authored definitions plus quest cooldown/completion counters | Location name, authored economy/society metadata | Not governance/membership; current owner of settlement political control must be found independently |

### H.1 Query contract (candidate)

The query accepts a caller-provided set of already-constructed, canonical source readers. It does not create those owners, restore them, or load a second copy. The result must distinguish each source's state:

```text
OwnerRead<T> = Ready(rows) | Empty | NotAvailable(reason) | NotReady | Invalid(reason)
```

This union is an explanatory shape only. Before creating a DTO, check whether the application already has an equivalent result/status contract. `Empty` means the owner is ready and reports no rows. `NotAvailable` means source host/content is not bound. `NotReady` means the existing restore lifecycle has not completed. `Invalid` means a source refused to provide valid data. The House view must never collapse these into the same empty list.

Rows should use the narrowest source schema possible. For sources without a stable key, a row can be displayed in the current page but cannot be selected for persistent bookmarking. Do not create a value tuple key by combining user-facing strings, floating-point prices, counterparty labels, or dates; those fields may change or collide.

### H.2 Proposed type field rules

If a future implementation creates a projection DTO, use the following rules and verify the repo's serialization conventions before choosing field names:

- IDs are source-qualified, stable strings supplied by owners. Do not generate ids at read time.
- Display labels are not keys and can be localized; comparison uses canonical ID fields.
- Quantities retain source signedness and units; market ledger `quantity` sign must not be normalized away.
- Monetary values retain the source unit and source owner label. No implicit addition across chits, settlement value, item principal, or barter value.
- Dates/day numbers are copied from source snapshots. They are not recalculated or advanced by the projection.
- Status fields preserve source enum/boolean distinctions. If a text-only display is needed, map exhaustively and retain a fallback `unknown_source_status`.
- Unsupported fields remain absent rather than defaulted to zero/false, since zero balance and unknown balance mean different things.
- If one value is unavailable, the row reports that value unavailable; it does not invalidate unrelated facts from the same source unless the source's contract says those facts are inseparable.

## Appendix I — Ledger Query and Ordering Cases

### I.1 Stable ordering

Order source-local rows only where the source provides a stable key. A deterministic view can sort by `(sourceOwnerId ordinal, sourceDay ascending/descending, sourceRecordId ordinal)`. Ties in a source without row ID remain in source-provided order only for this live snapshot; the House must not promise the order is stable after save/restore. If UI pagination needs stable cursor semantics, the required key is missing and must be supplied by the source owner or the rows must not be paginated across sessions.

### I.2 Large and bounded histories

The sources have different retention shapes: market ledger may be a list in `MarketState`; `FundsLedger` intentionally keeps at most 128 movements; route system holds current contract state and aggregate run counters; the Holdfast save stores balances/stock rather than a trade history. A House “last 30 days” filter cannot imply that every owner retained 30 days of complete data. The view must state per-source coverage or avoid a unified time-range control.

Pagination must not slice the `MarketState` list by mutating it or reorder canonical owner state. If a view caps rendered rows for performance, show the cutoff and source count when available. Never silently trim canonical data in the House query.

### I.3 Counterparty resolution

Counterparty labels are domain-specific: market `counterparty` string, route `CounterpartyId`, debt `creditorId`, black-market `syndicateId`, Holdfast `factionId`, and caravan manifest `faction_id`. Even if two IDs currently resolve to one display name, they are not interchangeable automatically. Each data path needs a typed resolver or an explicit unresolved label. Name equality must not merge entities.

Potential resolution outcomes:

| Resolver state | Display behavior | Link behavior |
|---|---|---|
| Canonical ID resolves to one current entity | Show canonical display name and owner-qualified ID on detail/diagnostic view | Link only to that entity's owner surface |
| ID is a supported alias with unique canonical target | Show resolved name and retain original source ID in diagnostics | Link to canonical target only if resolver contract is stable |
| ID is unknown | Show “Unknown counterparty” plus safe source identifier | No entity link |
| Multiple targets resolve | Mark ambiguous; do not choose first enumeration result | Disable linked command |
| Resolver absent | Show source-provided label only | No cross-owner relationship claim |

## Appendix J — Focused Verification Matrix

This plan is documentation only; no test has been run or added. The following matrix maps one possible future change to existing targets and expected responsibility. At implementation time, re-check target names and the scoped runner catalog.

| Behavior | Existing target to inspect first | House-specific assertion if needed | Do not duplicate |
|---|---|---|---|
| Market quote and ledger behavior | `EconomyHostSessionTests`, `PriceExplanationTests`, `EconomySystemTests` | House read leaves ledger/pressure unchanged; price factors are owner-returned | Core market pricing/ledger formulas |
| Player route contract save/read | `TradeRouteContractTests`, `Plan192TradeRouteHostIntegrationTests` | Projection uses owner values and never ticks/overwrites contract | Reliability tier and cadence math |
| Caravan barter and manifest | `CaravanTradeNetworkTests` | House display uses only canonical result and fails unavailable on unproven exception atomicity | Existing barter valuation/route stock tests |
| Quote commit dedupe | `CaravanAtomicTraderTests` | A House retry delegates only once or queries the source idempotency result | Quote-ID validation and capture/restore |
| Holdfast transaction conservation | `HoldfastFundsTradeTests`, `HoldfastTradeIntegrityTests`, `HoldfastTradeSessionTests` | House forwards one command; no second debit/grant; operation result is displayed as returned | Current balance, stock and price arithmetic |
| Black-market settlement | `Plan211BlackMarketSettlementTests`, `Plan211BlackMarketHostWiringTests` | House entry point does not call service twice; source refusal remains source-owned | Existing wallet/inventory/stock atomic transaction cases |
| Debt offer/accept | `TradeCreditCoordinatorTests`, `LedgerDebtSystemTests` | No House offer is treated as signed debt; user command reaches existing ceremony once | Eligibility, creditor gates, schedule and loan mechanics |
| Faction/settlement identity | `HoldfastFactionIdentityContractTests`, `FactionDisplayNameCatalogTests`, `SettlementCatalogTests` | Name resolution does not infer membership; duplicate aliases fail closed | Existing catalog serialization and ID map assertions |
| Read-model determinism | Find nearest canonical deterministic/read-model gate before adding test | Same snapshots produce same ordered rows across culture; no input state mutation | Generic sorting library behavior |
| Save/no-new-section assertion | Existing save-registry or source owner tests | House projections reconstruct after source restore without an extra store | Entire save registry if unchanged |
| UI lifecycle | Existing player-panel/focus/accessibility targets if view is added | Open/close/restore/owner unavailable do not duplicate listeners or actions | Framework focus behavior |

Select the smallest affected target set. If a source-specific invariant lacks any focused test, add one test at that source owner only in its approved package; do not paper over a missing owner contract with a House adapter test.

## Appendix K — Charter Rules and Permissions Without a Shadow Faction

“Charter” can mean authored declaration, membership rules, faction law, or executable permission policy. Those meanings must not be collapsed into one new `HouseSystem`. This plan permits only the first as narrative content until a current owner is identified for the others.

| Charter concern | Safe read-only treatment | What would require an existing owner | What this plan does not authorize |
|---|---|---|---|
| Institution name and emblem | Read from the existing faction/organization catalog if the House already exists; otherwise display only approved static title text | Stable entity ID, catalog loader, asset registry | Creating new faction IDs or a second faction catalog |
| Charter text | Authored narrative copy with source ID and localization if supported | Scenario-specific selection or mutable amendment | Treating prose clauses as executable law |
| Members | Resolve current member facts from the owning roster/relationship system | Join/leave events, membership persistence, role hierarchy | New membership table, `HashSet<string>` cache, or parallel survivor roster |
| Roles | Display owner-defined role labels | Permission checks and role transitions | UI hiding controls as a substitute for command authorization |
| Signature/consent | Show a canonical confirmation record if an existing ceremony owner provides it | Persisted signers and validation | Treating button click or dialogue choice as a contract signature |
| Counterparty standing | Read the appropriate diplomacy/faction owner and label it separately | Standing changes, treaty or embargo actions | Mapping House membership to alliance, trust, credit eligibility, or market access |
| Audit access | Display source-qualified activity facts where available | Permissions to reveal private debt/black-market records | House-wide access to all owner-private data |
| Succession | Omit unless a current owner exposes successor facts | Authority transfer and persistence | Naming a successor from narrative order or UI selection |
| Dispute handling | Navigate to existing arbitration/contract owner | Hearings, judgments, fine or suspension | New court, fine ledger, or member discipline system |

### K.1 Identity and authority should be separate axes

A faction identifier can answer “which group does this record name?” It does not necessarily answer “which survivor may command it?” The player character, campaign faction, creditor ID, counterparty ID, route owner, and House-member ID may be different identifiers. Before any member action is proposed, trace the identity handoff from current player command context to source API and ensure the source owner validates it. If the existing command is implicitly player-only and takes no member ID, the House cannot claim that another member executed it.

The UI may reflect source authorization results but may not create them by comparing string IDs. A visible `memberId` field in an informational row is not a grant of permission. Do not grant every member access to records merely because the House view can render them; each source may have privacy or faction boundaries.

### K.2 Member lifecycle resolution

When a member exits the House through a canonical system, the projection must reflect the new canonical roster. The House cannot keep a member “active” because an old row remains in a cached collection. Conversely, leaving the House does not delete the person or rewrite their existing route/debt/market facts. Historical actor attribution, if present in a canonical record, remains a source fact; missing actor attribution is never backfilled by current membership.

Events to inspect for any future roster binding include recruited, dismissed, deceased, imprisoned, away/on expedition, transferred, replaced by a survivor migration, and faction membership changed. Only events actually produced by the existing owner can drive refresh. An absent event requires refresh-on-open, not a second day owner.

## Appendix L — Read Surface State Machine

The view should represent owner readiness and user navigation without creating economic states. This state machine is presentation-only; it does not belong in Core unless the owner contract requires it.

| View state | Entry condition | Allowed operation | Exit condition |
|---|---|---|---|
| Unbound | Campaign/UI owner not available | Close/back | Main binds the existing identity and source readers |
| WaitingForRestore | Existing restore barrier is incomplete | Show loading/readiness text; allow back | Owners report ready or restore failure |
| CharterOnly | House identity/text resolves but optional activity sources do not | Read charter; open available owner surfaces | Source becomes ready, owner changes, or close |
| Reading | Snapshot queries execute synchronously from ready readers | Render source-tagged rows | Query completion or failure |
| PartialRead | Some owners ready, some not available/invalid | Show ready owner rows and per-source availability | New refresh, source bind, or close |
| Detail | User selected a row | Show source fields and owner link if stable | Return to list/back |
| Unavailable | Identity owner missing, unknown House, or current actor unauthorized | Show neutral unavailable state, close/back | Canonical identity/permission changes |
| Disposed | Panel/session closes or campaign is replaced | No callbacks/listeners allowed | A new panel instance is created |

All row models in `Reading` / `PartialRead` are throwaway projections. The view must not transition to `ActiveContract` or `Settled` solely because a row was selected. If source lookup fails while detail is open, the UI switches to “record unavailable” and disables any forwarded action.

## Appendix M — Source Snapshot Consistency

The owners are not necessarily captured in a single global atomic snapshot. A market row and inventory snapshot may reflect different points in an operation; a funds movement can be written before a UI state change; route counters update on their own day owner. The House must not promise a cross-owner instant ledger unless an existing campaign snapshot or transaction boundary gives it one.

### M.1 Read consistency levels

| Level | Meaning | Acceptable view claim |
|---|---|---|
| Single-owner snapshot | One canonical owner returns a coherent state/read model | “Current route owner reports …” |
| Host-turn snapshot | Multiple synchronous reads occur in one UI/host turn with no intervening mutation contract | “Current at the time this page refreshed,” not a financial statement |
| Eventual view | Owner changes may occur between source reads; UI updates on next event/open | “Sources refreshed at different times” if values could be mistaken as a total |
| Saved campaign snapshot | Existing save coordinator captures all relevant owners at a defined boundary | Only claim atomicity if current save coordinator explicitly guarantees it |

Do not hold locks across multiple Core owners from the House. Do not read one owner, issue a command, then assume another owner snapshot is from the same transaction. If the game is single-threaded today, that still does not prove save callbacks, host re-entry, or future threading preserve the same assumption.

### M.2 Consequences for aggregates

No unified totals should be added at TH-1. If a future product decision requests totals, it must define units, sign conventions, freshness, duplicate treatment, pending vs completed obligations, realized vs quoted value, debts/forfeits, and whether a source’s bounded retention creates missing history. A total could be a read-only analytic estimate only if visibly labeled and excluded from gameplay gates. This draft does not authorize that calculation.

## Appendix N — Identity/Data Collision Ledger

The repository contains multiple related concepts that are likely to collide if names are chosen casually. They must remain separate until a source owner establishes a canonical mapping.

| Existing concept | Current authority direction | Similar word likely to cause confusion | Rule for House draft |
|---|---|---|---|
| `FactionWarSystem` standing | Dynamic geopolitical relation, standing/control/hostility/alliance | “Faction member”, “House loyalist” | Standing is not membership; do not add House to this record implicitly |
| `HoldfastFactionsCatalog` | Authored faction identity and trade preference/presentation | “Trading House faction” | Determine if a House row already exists; do not duplicate its catalog entry |
| `faction_lore.json` / display-name catalog | Presentation and authored faction lore | “House charter” | Lore display text does not grant role/permission semantics |
| `SettlementCatalog` | Settlement definitions, NPCs, repeatable quests | “Settlement governance” | Definition/quest state does not prove current elected/appointed member roster |
| Faction branch systems | Player’s progression/ideological/military branch state | “House branch” | Branch identity is not organizational membership or authority to trade |
| `HoldfastTradeSession` trader state | Current trade value, goods held, merchant stock | “House account / stock” | Do not rename these values as House-owned assets |
| Long Line `house_charter.json` proposal | Freight company progression/house marks in its own expansion plan | Shared term “house charter” | Distinct plan and domain; no artifact reuse without approval |
| Contract Board 109 | Posted offers, escrow/deadline and fulfillment plan | “House consignment agreement” | Link to owner only after live status check; no competing schema |

If a new data ID is later proposed, search exact ID, normalized title, previous migration aliases, and similar concepts in active and archived plan folders. “Search did not find a title” is weaker evidence than “no conflicting runtime owner”; both are required.

## Appendix O — Backward-Compatible Change Paths

TH-1 does not authorize persistence, but this matrix prepares a later integrator decision without selecting a new owner prematurely.

| Future need | Lowest-risk path | Migration requirement | Rollback impact |
|---|---|---|---|
| Static House title, no membership | Existing localization/content slot or owner panel constant if that is current project practice | None if no authored schema changes | Remove surface/text only |
| Current member display | Read an existing roster owner | None; re-resolve every open | Remove projection only |
| Persistent House membership | Extend the actual identity owner only after signature and path claim | Existing DTO optional field/default; version bump only if owner contract calls for it; old saves restore no House members | Hide House membership UI; preserve owner state |
| Member role | Extend same identity owner if role is truly its concern | Unknown role fallback and migration from absent role | UI can suppress role labels; no effect on other owners |
| Pinned activity references | Add pointer collection to a selected existing owner only if source keys are stable | Source-owner type versioning and deletion behavior | Hide pins; do not delete source records |
| Historical transaction journal | Must be owned by canonical transaction system | Source-specific stable IDs, retention/version policy, migration from existing facts only when unambiguous | Cannot be reconstructed safely from missing rows; stop before implementation if rollback would lose authority |
| Unified statement/valuation | Requires signed financial semantics and consistent snapshot contract | Versioned valuation rules with deterministic conversion | Disable display; never feed gameplay decisions |

Never migrate by inventing historical attribution. If an old market row cannot name a member, the House cannot assign it to whoever currently holds the matching faction or route.

## Appendix P — Source Contract Inventory by Read Obligation

This appendix turns the evidence ledger into a contract checklist. “Read” below means an eventual House projection may request facts from the named owner. It does not assert a currently public aggregate API. Any proposed method signature is illustrative and must be reconciled with the live source before implementation.

| Source owner | Current stored or emitted fact evidenced | House question it could answer | Missing contract detail | Safe fallback |
|---|---|---|---|---|
| `MarketSystem` | Versioned market state and ledger entries with day, item ID, signed quantity, unit price, total value and counterparty; transaction records category pressure and events | What market transactions has this campaign owner recorded? | Stable row identity, actor attribution, retention, query-by-counterparty API, and whether UI can safely retain a pointer | Show only currently enumerable rows with source and freshness; do not promise complete lifetime history |
| `HoldfastTradeSession` | Current trader value, goods held and stock; buy/sell result APIs; `FundsLedger` movement records with reason/source/day and bounded movement log | What happened in this trader session, and what is its current state? | Durable per-action receipt, stable merchant ID contract, whether direct host path also updates market owner | Navigate to the active session; treat absence of row as “not exposed here,” not “never traded” |
| `CaravanAtomicTrader` | Commit record keyed by caller quote ID; cloned capture/restore; emitted accepted-record event; no observed settlement mutation | Which quote commit facts are recorded? | Relationship to actual barter completion, ID namespace, retention, actor identity | Never describe commit as delivery/payment; label “quote commit record” if surfaced at all |
| Caravan network owner | Manifest-aware barter validates and mutates manifest/player inventory, updates progress, emits completion | Did this barter complete against the arrived manifest? | Durable per-barter receipt, rollback behavior under second-leg exception, stable manifest/action IDs | Use owner’s present result only; an exception with uncertain state is “outcome unknown” |
| `BlackMarketSystem` + settlement service | Source-specific preview; coordinated stock, wallet and inventory callback; stock rollback on callback refusal/throw | Was this specific black-market request settled? | Durable unique request ID and cross-save recovery protocol; current API visibility | Link to canonical black-market interaction; House never retries uncertain action |
| Player trade-route owner/host | Contract definitions, cadence/reliability and aggregate execution counters; host tick at registered day phase; dedicated save section | What route contracts exist and what aggregate execution outcome does owner report? | Run-level receipt/history and actor identity; explicit fact distinguishing planned, attempted, delivered and lost legs | Display contract counters with their owner terminology; never derive per-run history |
| `TradeCreditCoordinator` + debt owner | Canonical acceptance/gates, compensation behavior, debtor-oriented current/closed debt contracts | What debt state does the creditor/debt owner currently report? | Stable debt contract ID, full repayment history and House membership boundary | Route to canonical creditor/debt view; no House offer, interest, ledger or acceptance substitute |
| `FactionWarSystem` | Dynamic standing, control, hostility and alliance facts | What is current geopolitical standing? | No membership contract; display-name and relation freshness | Do not use standing to authorize membership or record ownership |
| `SettlementState` / settlement owner | Inspected state tracks quest cooldown/completion counts | Does the settlement owner currently expose a House roster? | Roster, governance, official membership events were not evidenced | Stop roster feature pending an actual owner and approved path |

### P.1 Consumer obligations at the boundary

Every consuming component needs three explicit answers before a row is rendered: which owner owns the fact; which call obtains it and whether that call is supported by a public API; and what “not available” means. A nullable field alone cannot distinguish “owner has no such rows,” “owner was not restored,” “owner has no query surface,” “campaign save predates this source,” or “source call failed.” The eventual projection should carry a source status enum or equivalent discriminant in memory, even if it persists nothing.

The House should not combine same-looking amounts from different sources. A market ledger `totalValue` is recorded transaction value, a Holdfast trader `value` may be session balance/state, a caravan trade’s value may be quote/barter basis, and a route’s aggregate count is not money. No common currency or sign convention was proven across these APIs. Before arithmetic, a consumer must establish a shared unit, day boundary, and completed-vs-planned status from owners. Without that contract, display separate facts and labels.

### P.2 Query shape proposal (VERIFY)

If a later integration needs one read model, keep it a read-time adapter that returns `SourceStatus`, `SourceOwner`, `ObservedAtCampaignDay?`, `CanonicalRecordKey?`, `DisplayFacts`, and `OwnerRoute?`. These are candidate fields only; confirm the project’s DTO and UI conventions. Do not persist this adapter. `ObservedAtCampaignDay` is an observation label, not a transaction day. `CanonicalRecordKey` is omitted when an owner cannot guarantee stable identity. `OwnerRoute` is a UI navigation intent resolved by the host, never a string URL interpreted by a domain system.

The query must accept the already-bound campaign owner references and a current access context. It must not resolve singletons globally, create missing owners, or silently substitute a faction-wide owner for a player-scoped one. If one read fails, return a source-specific unavailable result and preserve the other owners’ valid rows; do not report an empty House ledger.

## Appendix Q — Candidate Ledger Presentation Rules

These rules constrain interpretation of facts if a future panel is approved. They are not a new accounting policy.

| Owner fact | Presentation phrase allowed | Phrase to avoid without more evidence | Why |
|---|---|---|---|
| Market `LedgerEntry` with signed quantity and total value | “Market owner recorded a trade on day D: Q of item I, recorded value V, counterparty C.” | “The House earned/spent V.” | No membership, actor, wallet transfer or House ownership proved |
| Holdfast buy/sell result | “Trader operation returned accepted/refused, reason R.” | “Market history confirms the transaction.” | Direct operation path may not update `MarketSystem`; verify host wiring |
| Caravan atomic trader commit | “Quote Q was recorded as committed.” | “Goods were delivered” / “payment completed” | Source class records commit fact only in inspected implementation |
| Caravan network completion event | “Network owner reported barter completion for this interaction.” | “The campaign ledger contains every exchange.” | Durable unique receipt/history was not found in inspected state |
| Trade-route counters | “Owner reports N successful / M failed aggregate executions.” | “The House delivered N contracts.” | Route owner is not necessarily House-owned and counters are aggregates |
| Debt contract current/closed view | “Debt owner reports this debtor state.” | “House member owes the House.” | House creditor, member identity, debt ID and owner relation not established |

Do not normalize missing or malformed owner values to zero. Zero says the source has a measured zero; unknown says no supported fact was obtained. If an entry has an unrecognized item ID, render a stable fallback item label while preserving the raw key in diagnostics; do not drop the row and thereby alter totals. If a source provides untrusted display text, escape it under existing UI conventions and use the catalog for authored labels where available.

Pagination and sorting should preserve the owner’s stable order where possible. Sorting by display name or value is a presentation operation and cannot become transaction order or tie-break behavior. If the canonical owner’s sequence order is not guaranteed, the projection may sort only after obtaining stable row keys; otherwise retain source order and avoid asserting recency. Current day is not a sufficient tie-break for multiple operations on the same day.

## Appendix R — Charter and Ledger Interaction Cases

| Case | Initial condition | User intent | Required owner interaction | Visible result | Prohibited side effect |
|---|---|---|---|---|---|
| Open charter during new campaign initialization | Campaign session exists; restore barrier not complete | Review House rules | Wait for UI’s existing campaign-ready signal; text may be static | Charter-only or waiting state | Starting ticks, constructing an owner, seeding roster |
| Open after complete restore with unsupported sources | Market owner restored; no roster owner found | Review account activity | Query only source APIs proven available; label source status | Market rows only, no implication they belong to House | “House balance” total or empty-house conclusion |
| Open with two owners, one throws on query | One read succeeds, one source unavailable | Review all activity | Isolate per-owner read failure | Partial view with failed source named | Discarding successful rows or retrying a mutation |
| Select row after source has advanced | Row pointer resolves to stale or missing record | Inspect detail | Re-query owner by stable ID if supported | Current detail or record unavailable | Reconstructing details from stale copied row |
| Open from unauthorized actor | Canonical access context refuses | View activity | Use the source owner’s authorization result | Unavailable/unauthorized message | String-compare faction/member IDs as substitute permission |
| Back to active trader | Host can resolve current Holdfast trade session | Continue a trade | Navigate to existing owner panel/session | Active canonical trader screen | House panel issuing a hidden buy/sell |
| Close during source event | UI subscribed to owner event | Leave view | Dispose/unsubscribe per existing lifecycle | No further row refresh callbacks | Listener remains attached to replacement campaign |
| Replace campaign while view is open | Old UI instance remains during scene/session switch | Avoid stale callback | Host invalidates/disposes old binding | View closes or becomes unbound | Old session mutates or refreshes new campaign UI |

The charter itself should never assert that a request is guaranteed to be honored. It may state institution-level principles—record provenance, readable obligations, dispute escalation, non-retaliation, and transparency—while execution, eligibility, and settlement remain canonical owner decisions. If those principles conflict with authored world canon, the plan pauses for narrative review instead of adding enforcement flags.

## Appendix S — Implementation Readiness Decision Table

This is a stop/go table for a later integrator. A row marked “stop” is a blocker, not an invitation to create the missing subsystem.

| Readiness question | Go evidence | Stop evidence / missing premise | Next action if stop |
|---|---|---|---|
| Which exact objects own House identity and membership? | Existing owner API and save section found; exact paths claimed | Only catalog, faction standing, or UI labels found | Keep TH-1 charter-only; ask for canonical owner decision |
| Can an activity row be attributed to a member? | Canonical source stores/returns actor and stable actor identity | Caller receives only a global market/route/debt fact | Show unattributed source fact or omit member attribution |
| Can rows be deduplicated across source displays? | Stable globally unique transaction key or explicit single-owner route | Similar date/item/value fields only | Keep source-separated; never hash mutable fields into durable IDs |
| Is a unified total meaningful? | Signed units, completion state, duplication rules, and snapshot policy documented | Different owners expose quotes, counters and bounded rows | Do not aggregate |
| Can the House submit activity? | Explicit approved host command path to canonical owner with idempotency/recovery | No stable action key or source receipt | Navigation only; no direct action |
| Is persistence needed? | Canonical owner already persists required state and House stores only references if needed | New ledger seems required because source facts are absent | Stop and request architectural decision; no shadow save store |
| Can old campaign saves load? | Existing owner restore is backward compatible; optional House fields default safely | Missing-owner behavior unclear or historical backfill ambiguous | No migration until source owner specifies contract |
| Can focused verification prove this change? | One narrow host route/projection test can inspect exact source binding | Test would only assert mock output or duplicate owner math | Redesign acceptance around owner boundary before implementation |

“Go” does not mean this draft approves implementation. It means the premise can be discussed with the foreman and path owner. User or foreman approval and ownership claims remain external gates. Do not turn this readiness matrix into a work claim or save-format commitment.

## Appendix T — Production Trade Flow Documentation Audit

`docs/production/PRODUCTION_TRADE_FLOW.md` is a relevant near-duplicate risk because its header calls itself a canonical regional trade-flow and commerce authority. Its opening claims catalog authority at `Assets/StreamingAssets/Data/trade_flows.json`, runtime types `ProductionTradeFlowSystem.cs` and `RegionalPriceCurveCalculator.cs`, and a unified settlement seam through `FactionLedger`. It also describes regional prices, barter, transport mass, and anti-arbitrage constraints. Those claims require source verification and cannot be accepted from prose alone.

### T.1 Current-path verification result

The bounded source check searched the current `Assets/Ashfall.Core`, `src`, `Assets/StreamingAssets/Data`, and `Ashfall.Core.Tests` trees for filenames matching `ProductionTradeFlow`, `RegionalPriceCurve`, `trade_flows`, and `ProductionTradeFlowSystemTests`. It found no matching current source, catalog, or test files. The current files audited in this draft remain the extant market, Holdfast, caravan, black-market, trade-route, and credit/debt owners documented in the main evidence section. The production document may be stale, aspirational, archived by omission, or point to a removed implementation; the current search alone cannot establish which.

Therefore, TH-1 must not depend on the production document’s named classes or schema. Before a later implementation, repeat the check on the exact branch/commit and inspect generated catalogs, rename aliases, loader registration, and source control history only if relevant. Do not recreate the named authority to make the plan fit. If a current canonical regional trade owner is found outside the searched trees or through a different type name, update the boundary with exact path, DTO and save-owner evidence, then stop for overlap review.

### T.2 Overlap disposition

The production document’s purported authority and the House’s proposed institution layer answer different questions only if that owner exists: the former would govern regional commodity exchange/pricing; the latter is intended to coordinate identity, charter text and read-only references. The House must not own price curves, inventory movement, wallet settlement, barter valuation, hauling capacity, merchant stock, faction trade pressure or transaction history. If a live production-trade owner is verified, all such requests route through it, and the House may only navigate to it or read a supported source projection. If no such owner exists, the House still cannot fill the gap; missing implementation is not permission to introduce a second commerce system.

The document’s test claims (including 100-test verification) also remain unverified against current test files. Test names and reported green summaries in a prose document do not establish present behavior. No tests are run for this documentation-only draft. A future integrator should compare the exact production document revision to current tests and decide whether it is an active authority, stale historical record, or a separate planned feature. Until then, mark all references to production trade flow `VERIFY` and exclude it from any House dependency graph.

| Claim in production document | Current audit status | TH-1 handling |
|---|---|---|
| Canonical source file `ProductionTradeFlowSystem.cs` | Not found by current path/name scan | Do not call, recreate or cite as implemented |
| Regional curve calculator | Not found by current path/name scan | No House-side price calculation |
| `trade_flows.json` catalog and schema | Not found by current path/name scan | No duplicated catalog or authored price source |
| `FactionLedger` is the settlement owner | Type/API/path was not proven by the bounded scan | Verify exact current authority before any link |
| Extensive anti-arbitrage and haulage tests pass | No corresponding tests found by exact filename scan | Treat verification level as stale/unconfirmed |

## Appendix U — Evidence Acquisition Protocol for a Later Implementation

This protocol keeps a future builder from treating today’s draft inventory as an eternal API guarantee. It is limited to TH-1’s read-side claims.

### U.1 Establish exact revision and ownership

1. Record branch, commit hash, and worktree state before any source re-audit. Search the current authority files, ownership ledger, integration plan, and test policy in the required order.
2. Resolve exact plan path claims before editing. This draft is not itself a code-path claim; if a future plan touches `Assets/Ashfall.Core/Economy`, host panels, or save composition, ask the integrator to claim the exact files first.
3. Search implementation, callers, registrations, serializers, save sections, tests, and data by exact type name and by concept. For the production trade-flow declaration, inspect potential alternate names such as regional buyers, export commodities, faction ledger and price curve; do not assume the absent exact filename proves feature absence.
4. Trace each public method from its owner definition to the actual campaign binding. A class that exists but is not registered in the current host is not a usable campaign surface. A registered adapter that creates a private owner is not the canonical one.
5. Trace persistence from capture to restore to session replacement. A `CaptureState` method alone does not show that the active save coordinator calls it, and a section registration alone does not show old-save defaults.
6. Check tests by reading their assertions, not titles. Classify each as owner unit, host integration, save/restore, replay/determinism, or UI route coverage. Record uncovered claims explicitly.

### U.2 Build a source contract card

For each source owner selected, write one contract card with: source path; owning type; public query method; current host binding; state DTO fields; transaction mutation behavior; save section/store; event names and order; action/record key guarantees; actor and counterparty identity; ordering/retention; old-save defaults; observed tests; and unresolved questions. “Not inspected” is a valid result. Guessing a field from a similar class is not.

This contract card makes a useful distinction between a fact and a derived view. For example, a source’s exact `LedgerEntry.day` is a fact; a UI-derived “recent” grouping is a presentation rule; a sum of values from two owners is an analytic proposal; “House profit” is a new accounting claim requiring a separate signed decision. Keep these layers separate in review.

### U.3 Evidence labels

Use `PRESENT` only when a current path, call site or test assertion directly demonstrates the behavior. Use `ABSENT IN INSPECTED TYPE` only when the inspected DTO/API surface is known. Use `NOT SEARCHED` for alternate-name/runtime discovery not completed. Use `VERIFY` for behavior dependent on execution order or host wiring. Use `STALE DOC CLAIM` for the production trade-flow assertions until reconciled. Avoid global statements such as “no system exists” from one filename search.

## Appendix V — Read-Only Projection Contract and View Model

This appendix describes a safe candidate projection for TH-1 if and only if each source supports the query. It is not a new domain DTO or saved ledger.

### V.1 Projection row responsibilities

| Projection element | Must originate from | Can be derived by view | Must not be inferred |
|---|---|---|---|
| Source owner kind | Host binding/typed adapter | Local enum to localized owner label | From similar field names or panel title |
| Raw source key | Canonical source | None; can be omitted if no stable key | Hash of mutable transaction fields |
| Source status | Canonical source result/state | Human wording | “Settled” from a commit event or positive ledger value |
| Day/date | Canonical source campaign clock | Relative wording such as “day 12” | Wall-clock timestamp or UI-open time |
| Item identity | Canonical item catalog/source key | Display label/icon from catalog | Drop or rewrite unresolved IDs |
| Quantity/value | Source fields with documented unit/sign | Formatting and grouping by exact unit | Conversion, cross-owner netting, or inferred fee |
| Actor/counterparty | Source owner plus canonical identity lookup | Display-name localization | Current roster lookup as historical attribution |
| Owner link | Host route resolver | Button label | Direct domain URL or stringly typed command |
| Availability/freshness | Reader call outcome and binding state | Neutral explanation | Empty list when query is unsupported or failed |

Candidate in-memory record:

```text
HouseActivityRow (ephemeral; proposal, VERIFY project conventions)
  ownerKind: enum
  stableSourceKey: optional opaque key
  rawStatus: optional source enum/string
  observedCampaignDay: optional integer
  sourceTransactionDay: optional integer
  itemKey: optional canonical item ID
  signedQuantity: optional source quantity
  recordedValue: optional source value
  actorKey: optional canonical actor ID
  counterpartyKey: optional canonical endpoint ID
  sourceFreshness: fresh | stale | unknown
  sourceReadStatus: available | empty | unsupported | notRestored | failed
  hostRouteIntent: optional typed route token
```

Every identifier type and number representation above is provisional. If the actual API uses `long`, decimal, integer chits, or immutable record structs, keep that type across the adapter. Do not use `double` to unify commerce values. If no stable source key exists, the UI may display a source-owned row for that refresh, but must not bookmark it or promise that it can be reopened later.

### V.2 Query and refresh algorithm constraints

The view model should obtain one owner snapshot at a time using campaign-bound references. Record read start/end only for diagnostics if permitted; do not turn elapsed wall-clock into gameplay ordering. Deduplicate only inside an owner that promises uniqueness. When one owner returns `unsupported`, do not probe a guessed alternative owner by mutating calls. When one owner throws, preserve other owner snapshots and report partial availability.

Refresh triggers may be: panel open after restore; canonical owner event that the host already publishes; campaign-day end if the existing panel already refreshes then; or explicit user refresh. Avoid adding another global day subscriber for a read-only view. Coalesce multiple invalidations into one UI refresh using the host’s existing lifecycle mechanism. Dispose all subscriptions on close and when campaign ownership changes.

If two events arrive during a snapshot, the projection may receive an old row followed by invalidation. The next refresh is authoritative. Do not update one field in a rendered row from an event and leave other fields from an older snapshot unless the owner event contract declares a complete delta. This is especially important for balance-like values: no optimistic “balance after trade” is needed in a read-only House view.

## Appendix W — Ledger Trust, Dispute, and Retention Semantics

The charter may promise clarity about evidence without pretending that every owner supplies a courtroom-grade journal. These distinctions should shape both copy and scope.

| User expectation | Evidence needed | Current limit from inspected owners | Safe House response |
|---|---|---|---|
| “Show me every exchange ever made.” | Unbounded or policy-defined source history, stable IDs, archive strategy | Some state is aggregate or bounded; `FundsLedger` movement log retains at most 128 records | Explain that this view is a current owner projection with source retention limits; do not claim full history |
| “Who made this exchange?” | Canonical actor key written at transaction time | Actor attribution not present in several inspected DTO/API surfaces | Show “actor not recorded” only when source confirms missing field; otherwise omit attribution |
| “Can I challenge a transaction?” | Canonical dispute case owner and immutable linked source receipt | No House dispute owner established | Link to source’s existing support path if one exists; otherwise report no current dispute workflow |
| “Was that value a final price?” | Owner status and quote-vs-settlement semantic | Market and quote-record APIs have differing behavior | State the exact source fact and its status; avoid “final” absent settlement result |
| “Can this record be edited?” | Owner correction/amendment API with audit | No such API proven | House view is read-only; correction requires canonical owner support |
| “Will this survive campaign save?” | Active save coordinator writes and restores owner state | Save evidence differs per source; not all have transaction receipts | Only promise restoration of the exact fields covered by verified owner save path |

A read projection must not be sold as an audit ledger if it omits rows due to retention, does not include wallet/inventory legs, or cannot identify actors. Its name should use “activity” or “owner records” unless the source contract proves the stronger term. The institution charter can state that disputes refer back to the originating owner and retain its original source key; if no stable key exists, that principle cannot be operationally guaranteed and must remain aspirational copy.

### W.1 Retention is source-owned

The House should not persist a copy “for convenience” to work around a bounded log. That would create an alternate record with different start dates, save failure modes and deletion rules. If product design later requires a durable House archive, it is a new architecture decision: name one transaction owner, specify immutable IDs, retention/compaction, privacy and actor attribution, migration from existing sources, data volume limits, save integrity and rollback. Until then, the UI should indicate the source’s available window where known and avoid making absence mean no event occurred.

## Appendix X — Member-to-Action Authorization Cases

TH-1 names a charter and possible membership concept, but membership cannot be inferred from trade facts. These cases expose the missing actor contract that must be resolved before the House can present member-specific permissions or history.

| Case | Canonical context required | Current evidence | House behavior until resolved |
|---|---|---|---|
| Player acts directly at a trader | Active actor key and trader’s accepted actor context | Holdfast transaction methods inspected; actor persistence in trade result/history not established | Keep transaction at existing trader surface; do not label it as member action |
| Player selects another survivor to conduct a trade | Assignment/command owner validates survivor availability, skill and permissions | No source call chain for delegated House trade established | Do not offer delegation; showing survivor portrait is not authorization |
| Survivor joins or leaves a faction | Membership owner emits canonical event/state and stable faction/member key | `FactionWarSystem` is geopolitical standing; not membership | No membership sync from relation changes |
| Survivor dies or is dismissed after historical trade | Source row stores actor at time of action | Actor key missing in some inspected surfaces | Preserve source fact without historical attribution; never attach current roster name |
| New campaign inherits survivor legacy | Campaign transfer owner specifies persistent identities | No New Game+ handoff established by this plan | Treat new campaign as separate until canonical identity contract says otherwise |
| House officer loses role while view is open | Identity owner changes authorization, source owner revalidates command | No House role owner found | Refresh/close on canonical invalidation; direct action remains disabled |
| Read-only member opens another member’s activity | Access policy and source permission gate | No privacy contract established | Do not expose actor-private or faction-private records by broad enumeration |

If the eventual game uses a single player actor for every economic command, make that restriction explicit in the user-facing design. A “member” may remain narrative flavor or a roster reference until a canonical assignment/identity system exists. Do not add a surrogate `HouseMemberId` solely to fill the actor field in a read row.

### X.1 Authorization is evaluated at commit time

A UI can display an authorization result from its latest refresh, but that result can become stale after a role change, campaign reset, faction change or source closure. The source command must re-check all relevant gates when invoked. The House may disable the button early for usability, but that is not a gate. A command that accepts only a quote ID and quantity should not be assumed to validate membership unless the implementation shows how actor context reaches it.

No permission is broadened by a read API. It is possible for all players to see an aggregate route counter but only the owner to issue commands; it is possible for a faction market row to be visible while the individual actor is unrecorded. Keep read visibility, action permission, and historical attribution as three separate policy questions.

## Appendix Y — Host Integration Contract Questions

The main plan names a panel and source references only conditionally. Before any host code is edited, answer these operational questions from current `Main` partials and campaign composition, then write exact paths into the approved package.

| Host concern | Evidence to trace | Acceptance condition | Failure response |
|---|---|---|---|
| Binding | Constructor/field that supplies active owner instances | House receives same objects already used by existing panels | Missing dependency disables affected read surface; no `new MarketSystem()` fallback |
| Restore barrier | Save manager order and owner-ready signal | First query occurs only after required owners restore | Waiting state; do not interpret defaults as zero/empty |
| Route registration | Current panel route enum/map and open/close command | Existing back/close/focus rules are reused | No ad-hoc hotkey or disconnected route |
| Event subscription | Source events and host forwarding | Listener receives owner facts on existing bus | Poll-on-open if no event; no duplicate global day service |
| Session replacement | Campaign/session generation token or disposal lifecycle | Old view invalidated before new binding is exposed | Old callback ignored/disposed, never redirected to new owner |
| Error boundary | Existing logger and UI status pattern | Read failure is source-scoped and nonfatal | Partial view with source status; preserve rest of panel |
| Input lifecycle | Focus, keyboard/controller close, pause behavior | Existing accessibility conventions hold | Panel remains closable even when a source hangs/fails |
| Localization | Current string-key source | Charter and errors resolve through existing localization path | No new hard-coded dynamic source error text without project convention |
| Save registration | Existing owner section registry | House adds none for read-only projection | If pointer persistence is approved later, use the named owner serializer path |

The view should remain read-only by default. If direct forwarding is added after a separate signature, the source command must be callable from a stable host service rather than panel-local lambdas, and command results must remain source-specific. A generic “perform transaction” callback obscures which owner performed the work and complicates test setup. Avoid blocking the UI while querying or mutating; current synchronous behavior should be verified, and asynchronous work needs a session-generation check before applying its result.

## Appendix Z — Migration and Removal Scenarios for the Read Surface

Even without its own save file, a read projection can fail across application evolution. Handle owner schema and route changes without converting missing data into false financial facts.

| Change event | Expected projection response | Migration requirement | Stop condition |
|---|---|---|---|
| Source owner type renamed but save section unchanged | Adapter updates at compile-time to new owner type after source review | No House data migration if projection remains ephemeral | Stop if mapping changes semantics or removes stable key |
| Source save section version changes | Existing owner migrates its own DTO before House read | House does not run parallel version conversion | Stop if old restore silently drops relevant fields |
| Item catalog ID is aliased | Canonical catalog resolver supplies current label while preserving source key | Use owner’s established alias/migration path | Do not rewrite source transaction IDs in a display adapter |
| Counterparty removed from catalog | Show unresolved endpoint key with neutral label | No archival record fabrication | Stop action navigation if current owner cannot resolve endpoint |
| Source retention policy shortens | Old rows disappear or become unavailable per owner policy | No House-side archive backfill | Update explanatory text; do not restore expired row from stale cache |
| House UI route is removed | Remove navigation surface and subscriptions | None for ephemeral projection | Never leave a dead action entry in other panels |
| House identity owner is retired | Close roster-dependent surfaces; preserve unrelated canonical transactions | Identity migration belongs to owner and signed package | Do not make activity rows look House-owned after identity disappears |
| A source no longer persists transaction detail | Show only remaining canonical aggregate/current state | Source owner decides migration/archive | Stop historical-ledger promises; do not copy data before removal |

Rollback of a presentation feature should require only unregistering its route, disposing listeners, removing projection code and restoring tests. If rollback would require deleting a House-owned history because it contains copied source state, the feature has crossed the no-shadow boundary and should not have been implemented under this plan. The safe rollback criterion is that all surviving economic facts remain readable from their canonical owners after the House view disappears.

## Appendix AA — Charter Content Boundaries and Review Prompts

The charter is the one portion of TH-1 that can deliver value without a new economic state owner. It still needs careful wording because institutional rules can imply mechanics the runtime does not implement.

| Charter topic | Safe declarative principle | Mechanical promise requiring separate evidence |
|---|---|---|
| Provenance | Every displayed statement names the source that recorded it | Every exchange appears in one complete House ledger |
| Member agency | Members may ask to inspect available records under existing access rules | Every survivor can issue transactions or review private records |
| Consent | A request is not presented as accepted until its canonical owner accepts it | An escrow or cooling-off period protects goods automatically |
| Dispute | A concern should be brought to the originating owner and cite its record key when available | House officers can reverse, freeze, or arbitrate a source transaction |
| Privacy | Views show only data permitted by current source/identity policy | House membership grants universal access to faction or debtor records |
| Correction | Errors should be corrected by the owner that created the fact | House can edit, delete, or replace canonical records |
| Retention | The interface should disclose limits known from its source | House permanently archives transactions that owners evict |
| Fair dealing | Authored rules may ask members to disclose obligations | Automatic fair-price enforcement, standing changes, or blacklist gates |
| Governance | The charter may describe nomination, review and amendment as narrative | Elections, votes, roles, removal or quorum are mechanically enforced |

Before finalizing diegetic copy, ask narrative review whether the House is meant to be a merchant guild, a settlement cooperative, an informal ledger circle, or another institution. These identities affect voice and canon but do not change system ownership. Do not use labels such as “treasury,” “escrow,” “credit desk,” or “official clearinghouse” unless matching runtime authority exists; those words create player expectations of durable balances and remedies.

### AA.1 Versioned text vs versioned rules

If the charter is static authored text, its content version may follow the existing localization/content workflow. A text revision must not alter save interpretation. If the design later adds mechanically enforced clauses, those are not merely a text version: they need an owner, rule IDs, migration behavior for old campaigns, effective-day semantics, deterministic evaluation, and tests. The current draft has no policy evaluator or clause state. The UI may show a signed/read acknowledgement only if an existing narrative journal or choice owner is selected to own that fact; do not invent a `CharterAccepted` flag in panel state.

### AA.2 Resolve contradictions without silently changing law

If source terms conflict with charter copy—e.g., charter claims an exchange can be challenged but the owner provides no correction mechanism—the user-facing view should report the source limitation and the charter text should be revised through content review. Do not add an exception button whose effect is unknown. If owners disagree about counterparty, price, or status, render source-specific accounts side by side and escalate to source owners; the House must not choose one as final based on timestamp or display order.

## Appendix AB — Worked Charter Revision Cases

These examples are design exercises, not approved world canon or a proposal for a rule engine. They show how wording can evolve while runtime claims stay exact.

### AB.1 Correcting an overclaim about complete records

**Draft text under review:** “The House keeps every trade made by every member.”

**Evidence check:** Market entries have day, item, signed quantity, recorded unit price/value and counterparty, but actor attribution and unlimited retention were not proven. Trade-route state exposes aggregate execution counts rather than a durable per-run chronology in the inspected DTO. `FundsLedger` keeps a capped movement log. The House has no roster owner yet.

**Content revision candidate:** “The House keeps a reading of the records each trade owner makes available. Where a record is incomplete or no longer retained, the source is named and the missing detail is left unresolved.”

**Runtime change:** None required for a static text revision. The source availability indicators belong in the read surface proposal, and only after each reader is implemented against its actual API. No `allTrades` flag or archive gets added.

**Acceptance question:** Can a player reasonably infer completeness, actor attribution or permanence from the revised text? If yes, the copy remains too strong.

### AB.2 Adding a fairness clause without price enforcement

**Player-facing intent:** Members should avoid knowingly exploiting a settlement during famine.

**Safe authored clause:** “When a settlement is under visible scarcity, members should record why an exchange is urgent and accept the local owner’s stated terms.”

**Evidence check:** The current plan has not found a canonical “settlement famine” policy input or House transaction actor. No owner contract establishes that the House can change prices, reject a barter or apply standing penalties.

**Allowed implementation under this draft:** Narrative text only, perhaps surfaced through an existing authored-content route if the project’s content conventions support it. No transaction preview is altered by this clause.

**Explicitly unsupported interpretation:** The clause does not prove price gouging detection, an embargo, dynamic tax, faction reputation penalty or mandatory explanation prompt. Any of those would require a separate owner decision and test contract.

### AB.3 Revising a dispute promise after a market row is challenged

Suppose the view displays a market row with source day, item key, quantity, value and counterparty. A player says the quantity is wrong. If the row has no stable key or actor ID and the market owner offers no correction endpoint, the House cannot decide which lower-level inventory event to reverse. It should display the raw source fact, provide the canonical source route if available, and avoid an “appeal accepted” status. The charter should say the House will direct disputes to the source owner where one can be identified, rather than promising that the House can adjudicate them.

If a later market owner adds stable receipt IDs and a correction mechanism, the content may be strengthened only after the integration shows that the active host uses that API and that corrections preserve original provenance. An edit-in-place that erases the first value may be inadequate for audit; that is for the owner to resolve, not this plan.

## Appendix AC — Worked Ledger Attribution Examples

### AC.1 Market row with no actor field

**Canonical fact received:** day `D`; item `I`; signed quantity `-Q`; unit price `P`; total value `V`; counterparty key `C`. The inspected entry shape provides these facts, not an identified House member.

**Safe row text:** “Market owner recorded `Q` units of `I` with `C` on day `D` (recorded value `V`). Actor: not supplied by this record.” The exact sign label should be selected from the MarketSystem contract; if negative quantity denotes a sale or outgoing leg, confirm that from `Transact` and existing callers before translating it.

**Unsafe row text:** “Member Mara sold `Q` units to `C` and the House earned `V`.” This invents historical attribution, direction semantics, House ownership, and profit arithmetic.

**Refresh behavior:** Re-query the market owner when the screen opens or on its existing event. Do not retain this as a durable row unless `LedgerEntry` has a stable lookup key. If two records share all displayed fields, show both source entries only if the owner returns both distinctly; never dedupe by date/item/quantity/value.

### AC.2 Route aggregate next to a market transaction

Assume the route owner reports three successful aggregate executions while MarketSystem returns one matching-day entry. The UI may show two source cards, each with its own labels. It may not say “three shipments paid for this market sale,” because the route owner’s count is aggregate and no per-run stable receipt/foreign key was found. A matching campaign day is temporal coincidence until a canonical link exists.

If the player filters by counterparty, only owners whose API explicitly provides a matching field can be filtered. A route without counterparty attribution should remain visible under “counterparty not available” or be omitted from that filter with an explanation; it cannot be joined by the string name shown in a different owner.

### AC.3 Current faction name differs from historical endpoint

Suppose a source row contains a counterparty ID whose display name has changed in the catalog. Resolve the label with the current catalog only if the ID mapping is canonical and stable. Preserve the raw ID in details. Do not rewrite the row to a different faction because current geopolitical control now maps the location to another controller. Faction standing/control and commercial counterparty identity answer different questions.

### AC.4 Owner read fails between list and detail

The list can render a source row at panel-open. Before the user opens the detail, the source owner may be unavailable, restore may be underway, or the row may have been evicted under owner retention. On selection, request details by stable source ID only if supported. If lookup returns missing, show “record no longer available from [owner]” and clear action controls. Do not show the old copied fields as current detail, and do not interpret a missing row as a reversed transaction.
| Regional commerce is canonical | Document asserts it; runtime proof absent | Keep question open; don't replace it with House behavior |
