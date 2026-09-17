# D1 Flagship Integration Plan [22]
## Plan 213 — Survivor Barter & Informal Economy

> **Canonical filename:** `D1_planintegration[22].md`
>
> **Previous:** `D1_planintegration[21].md`
>
> **Next:** `D1_planintegration[23].md`
>
> **Purpose:** Introduce a survivor-to-survivor informal economy where individuals can exchange personal items,
> gifts, favors, and services through negotiated offers while preserving ASHFALL's existing shelter-wide
> inventory and external-market authorities. The system should create social and economic agency without turning
> communal resources into secretly private stock, spawning a second currency, or requiring the player to
> manually adjudicate hundreds of trivial transactions.
>
> **Primary source:** Plan 213 — Survivor Barter & Informal Economy.
>
> **Core repository problem:** `MarketSystem.Barter()` handles external equal-value trade, but there is no
> survivor-to-survivor barter authority, no personal pricing, no favor ledger, no pairwise trade reputation, no
> internal dispute lifecycle, and no informal price board. More importantly, the repository currently treats
> inventory as shared shelter inventory, so Plan 213 cannot safely transfer "personal items" until ownership or
> personal-belongings semantics are explicit.
>
> **Implementation posture:** transactional, consent-driven, sparse, deterministic, relationship-aware,
> ownership-safe, event-sourced for significant trades, data-driven for rules, and tightly bounded against
> resource duplication, wash trading, reputation farming, infinite favor generation, or bypass of shelter
> inventory policy.
>
> **Critical guardrail:** `SurvivorBarterSystem` must never silently privatize shared shelter stock. Item exchange
> is allowed only for items that a canonical ownership/belongings authority identifies as personally disposable,
> or through an explicit shelter-authorized allocation transaction. Until Plan 210 or equivalent personal
> ownership exists, item barter must remain disabled or limited to explicitly personal/allocated item instances.
---

## 1. Source Problem Statement

The source identifies the missing internal-economy layer:

- no `SurvivorBarter`, `InformalEconomy`, `InternalTrade`, `SurvivorTrade`, `PersonalTrade`, `BarterSystem`,
  `UndergroundMarket`, or `SurvivorEconomy` implementation exists;
- `MarketSystem` handles external trade only;
- all items currently live in shared shelter inventory;
- no survivor-to-survivor negotiation exists;
- no personal price board exists;
- no favor-trading exists;
- no pairwise trade reputation exists;
- no internal black-market/informal-market state exists;
- no dispute lifecycle exists.

The target architecture is:

```text
Personal ownership / allocation authority
                 ↓
        SurvivorBarterSystem
      ┌──────────┼───────────────┐
      ↓          ↓               ↓
   offers     favor ledger   trade reputation
      ↓          ↓               ↓
 counter / accept / reject / expire
                 ↓
         transactional settlement
       ┌─────────┼───────────────┐
       ↓         ↓               ↓
Inventory   Relations      Favor/Task system
       └─────────┼───────────────┘
                 ↓
          completed trade fact
                 ↓
   communication / journal / quests / reputation
```

The informal economy owns offer negotiation, settlement orchestration, pairwise trade-history facts, and favor
obligations. It does not own item existence, survivor identity, relationships, work scheduling, external market
prices, or shelter-wide economic policy.
---

## 2. Flagship Success Criteria

Implementation is complete only when all of the following are true:

1. `SurvivorBarterSystem.cs` exists with schema-versioned capture/restore.
2. Every offer has a stable ID and immutable offerer/target identity.
3. Every completed trade has a stable trade ID and references the accepted offer/counter chain.
4. Item terms reference stable item instance IDs or validated quantity/batch claims owned by an actual inventory authority.
5. Shared shelter inventory cannot be traded as personal property without an explicit allocation/permission path.
6. The personal-belongings dependency is audited before item barter is enabled.
7. If Plan 210 is unavailable, the system runs in a safe reduced mode rather than fabricating personal ownership.
8. Offer acceptance revalidates that all promised items/favor capacities remain available.
9. Settlement is atomic/idempotent.
10. A trade cannot transfer one item twice.
11. Save/load during offer settlement cannot duplicate or lose items.
12. Counter-offers form a bounded negotiation chain.
13. Offers expire deterministically on campaign time.
14. Negotiation does not reroll values merely because the UI is reopened.
15. Survivor preferences/value modifiers are deterministic from stable state.
16. No universal internal currency is created unless a separate shelter-economy plan explicitly adds one.
17. Informal prices are asking terms or valuation profiles, not authoritative market prices.
18. Favors use typed, fulfillable obligations rather than free-text descriptions as mechanical authority.
19. Favor completion is confirmed by the owning task/service system.
20. Favor debt cannot be repaid by merely toggling a flag in UI.
21. Pairwise trade reputation is separate from general relationship/trust state.
22. Trade reputation changes are bounded and evidence-based.
23. SurvivorRelationsSystem remains the relationship authority.
24. Gifts transfer real personal items and can emit a bounded relationship/social-capital event.
25. Repeated gift loops cannot farm unlimited relationship/reputation gains.
26. Disputes require a concrete disputed trade/favor and reason code.
27. Conflict/mediation systems own social escalation/resolution where applicable.
28. No trade can occur with dead, departed, incapacitated, or unavailable participants unless a specific inheritance/estate mechanic permits it.
29. Price-board listings reference actual available personal goods or typed services.
30. Listing expiration/removal is deterministic.
31. A listing cannot sell the same item into two accepted offers.
32. The informal market can be globally disabled without breaking saves.
33. Old saves start with no fabricated offers, trades, favors, prices, or pair reputations.
34. Existing shelter inventory behavior remains unchanged when barter is disabled.
35. UI uses read-only projection DTOs.
36. Headless behavior matches UI-driven behavior.
37. `--survivor-barter-selftest` validates offers, counters, acceptance, settlement, gifts, typed favors,
    reputation, disputes, expiration, migration, idempotency, inventory safety, and save round-trip.
---

## 3. Repository Reconnaissance Before Editing

Create:

`docs/survivor_barter/SURVIVOR_BARTER_INTEGRATION_AUDIT.md`

Inspect at minimum:

- `Assets/Ashfall.Core/Economy/MarketSystem.cs`
- `Assets/Ashfall.Core/Inventory/Inventory.cs`
- `Assets/Ashfall.Core/Survivors/SurvivorRelationsSystem.cs`
- survivor lifecycle / survivor registry
- survivor scheduler / duty / task systems
- `PersonalBelongingsSystem` from Plan 210 if implemented
- item instance IDs / stack semantics
- gift/trade ownership semantics if any
- quest item reservation rules
- equipment assignment/reservation
- crafting/job reservations
- shelter resource policies
- external black-market plan/system from Plan 155 if implemented
- Plan 202 InterpersonalConflictSystem if implemented
- Plan 211 InternalCommunicationSystem if implemented
- shelter reputation system if present
- save schema/migrations
- deterministic RNG
- UI inventory/survivor interaction panels
- journal/achievement/epilogue consumers

Build an authority matrix:

| Concern | Canonical owner | Barter-system role |
|---|---|---|
| item existence | Inventory | reserve/transfer through transaction |
| personal ownership | PersonalBelongings/ownership authority | validate disposable ownership |
| survivor identity/lifecycle | Survivor registry | validate participants |
| general relationship | SurvivorRelations | emit bounded relationship facts |
| work/service completion | Scheduler/task system | confirm favor fulfillment |
| external prices | MarketSystem | optional reference only |
| external black market | Plan 155 | no ownership |
| interpersonal conflict | Plan 202 | consume dispute/escalation |
| internal notice board | Plan 211 | publish listing/notice facts |
| shelter reputation | shelter reputation authority | consume major informal-economy facts |

The audit must answer one blocking question before item barter ships:

> **What exactly does it mean for a survivor to own an item in a shelter where inventory is currently shared?**
---

## 4. Scope Boundary

### In scope

- survivor-to-survivor offers;
- counter-offers;
- accept/reject/expiry;
- personally owned item exchange;
- typed favor exchange;
- mixed trades;
- gifts;
- informal asking prices/listings;
- pairwise trade reputation;
- dispute records;
- negotiation/value projections;
- internal-market UI;
- save/load;
- migration;
- deterministic CI.

### Explicitly out of scope

- replacing `MarketSystem`;
- creating shelter currency;
- privatizing all shared resources;
- arbitrarily creating personal inventories;
- external black-market economy;
- wage/salary simulation;
- taxation;
- debt collection violence;
- prisoner/slave trading;
- gambling;
- speculative commodities;
- full contract law;
- autonomous real-time auction house;
- parsing free-text favor promises into executable gameplay.

The feature is an internal social economy, not a replacement for ASHFALL's resource-management model.

---
## 5. Ownership Is the Blocking Dependency
- All item barter depends on a canonical answer to personal ownership.
- Do not assume items in shared Inventory are personally disposable because a survivor currently carries/equips them.
- Audit Plan 210 PersonalBelongingsSystem or equivalent ownership metadata before enabling exchange.
- If no personal ownership exists, ship favor/gift-of-explicit-personal-item functionality only after a minimal allocation contract is added.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 6. Shared Inventory Boundary
- Shared shelter stock remains communal.
- A survivor cannot offer five medicines, ammunition, fuel, or food directly from shelter reserves merely because the UI can find those item IDs.
- Any communal→personal transfer must be a separate authorized allocation transaction.
- Barter settlement never mutates communal stock without that explicit authority.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 7. Personal Belongings Contract
- Preferred API: `IPersonalBelongingsAuthority.CanDispose(survivorId, itemInstanceId)` and `TransferOwnership(...)`.
- Ownership authority remains canonical over who owns the item before/after exchange.
- BarterSystem reserves and requests transfer; it does not maintain a shadow owner field.
- If Plan 210 uses keepsakes/assigned belongings rather than full personal inventory, barter respects those exact semantics.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 8. Personal Item Eligibility
- Tradeable item must be personal/disposable, not quest-locked, task-reserved, equipped where removal is illegal, destroyed, missing, or already reserved by another offer.
- Some personal items may be nontradeable by definition.
- Use item tags/policy, not hardcoded names.
- Return structured ineligibility reasons.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 9. Stackable Item Boundary
- Audit whether personal belongings can own quantities/batches.
- Do not represent a promised stack as a static `item_id` if quantity can change.
- Use item instance + quantity reservation or a canonical item-claim token.
- Settlement conserves quantities exactly.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 10. Offer Definition
- `BarterOffer` should contain stable offer ID, offerer, target or public-listing scope, term set, created/expiry time, status, parent offer/counter ID, and source event ID.
- Separate terms from rendered description.
- Do not persist `minimumAcceptance` as a magical hidden personality score without a valuation contract.
- Use a negotiation profile/value threshold derived from survivor preference/trust where possible.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 11. Offer Terms
- Represent terms as typed claims: item transfer, favor/service obligation, gift/no-return, or optional shelter-approved allocation.
- Each item claim includes item instance/quantity.
- Each favor claim references a registered favor definition plus parameters.
- No free-text mechanical term.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 12. Offer Targeting
- Direct offer targets one survivor.
- Public informal listing targets any eligible participant under seller terms.
- Do not create one copied offer per survivor for a price-board listing.
- Target eligibility is checked at acceptance.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 13. Offer Lifecycle
- Recommended: draft → pending → countered/accepted/rejected/expired/cancelled.
- Accepted enters settlement, then completed or failed/voided.
- Countering terminates/supersedes the prior active offer with a child counter offer.
- One branch must be canonical to prevent accepting stale terms.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 14. Offer Expiration
- Use campaign clock/day.
- Index next expiry rather than scanning all offers every frame.
- Expired offers release item reservations.
- Save/load at expiry boundary is deterministic.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 15. Offer Cancellation
- Offerer may cancel pending offer if no accepted settlement is underway.
- Cancellation releases reservations.
- Countered/superseded offers cannot be revived without a new offer.
- Cancellation is not a dispute.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 16. Counter-Offer Chain
- Counter offers reference parent and root negotiation IDs.
- Set a configurable maximum counter depth or total active negotiation age.
- Only latest active branch can be accepted.
- This prevents infinite ping-pong and save growth.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 17. Negotiation State
- Negotiation is deterministic from preferences, current need, item desirability, relationship/trade reputation, scarcity context, and offer terms.
- Do not roll acceptance each time the offer panel opens.
- If RNG is used for AI willingness variation, seed once by negotiation/root offer ID.
- Persist decision-relevant seed/result where required.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 18. Perceived Value
- `agreedValue` should not become a universal currency.
- Use normalized perceived utility/value units internal to negotiation.
- Different survivors may value the same item/favor differently.
- Do not expose value units as tradable money unless intentionally designed.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 19. External Market Reference
- `MarketSystem` may provide a baseline external value for items if available.
- Survivor barter modifies that baseline by personal need, scarcity, attachment, trust, and trade context.
- Do not let informal internal prices overwrite external market prices.
- MarketSystem remains complementary.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 20. Value Context
- Potential inputs: item baseline value, survivor need/utility, scarcity, personal attachment, condition/quality, relationship, trade reputation, favor burden, urgency.
- Only use inputs backed by existing systems.
- Do not invent hidden personality traits solely for barter unless a survivor preference system exists.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 21. Minimum Acceptance Refactor
- The source's `minimumAcceptance 0-100` is too opaque as persisted authority.
- Prefer a derived acceptance threshold from negotiation rules or a data-defined negotiator profile.
- UI may display flexibility/firmness bands.
- Do not let a raw threshold drift independently of the survivor's state.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 22. Fixed / Negotiable / Free Listings
- Retain fixed, negotiable, and free as listing policy.
- Fixed means AI seller will not counter below stated terms, subject to item availability.
- Negotiable allows counter terms.
- Free is a gift/claim listing and should route through gift safeguards.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 23. Informal Price Definition
- Treat `InformalPrice` as a listing/ask, not a market-clearing price.
- Reference seller-owned item(s) and requested term template.
- Price expires and becomes invalid if item is no longer available.
- Do not store a global price per item type unless adding market analytics separately.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 24. Public Price Board
- Price board is a UI/read model over active listings.
- Plan 211 InternalCommunicationSystem may publish notices if implemented.
- The communication system owns visibility/distribution; BarterSystem owns listing terms.
- Do not duplicate offers just to display them.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 25. Listing Availability
- Listing reserves the advertised item/quantity or uses a single canonical availability claim.
- Seller cannot simultaneously consume/sell it elsewhere.
- If reservation is intentionally soft, acceptance must atomically revalidate and may fail.
- Choose one policy and test races.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 26. Reservation Strategy
- Recommended hard reservation for personally owned physical items once an offer/listing is posted.
- Reservation blocks conflicting barter/gift/consumption/assignment.
- Cancellation/expiry releases it.
- Reservation authority ideally belongs to Inventory/ownership service.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 27. Atomic Settlement
- Settlement revalidates participants, ownership, item state, favor capacity, and offer validity.
- Then reserves all outgoing claims, creates obligations, transfers item ownership, emits relationship/reputation facts, and commits trade exactly once.
- On failure, no partial item exchange unless explicitly modeled as dispute/fraud.
- Use transaction ID.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 28. Two-Sided Item Exchange
- Both sides' item claims are reserved before transfer.
- Ownership changes atomically.
- Do not transfer A's item first and then discover B's item vanished.
- Use existing inventory transaction/batch semantics.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 29. Mixed Item + Favor Trade
- Item leg can complete immediately while favor leg becomes an explicit obligation.
- Trade status may be `completed_with_obligation` or completed trade plus active favor debt.
- Failure to perform later can create a dispute.
- Do not keep the physical item in limbo indefinitely unless escrow is explicitly implemented.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 30. Favor Definition Contract
- Create a finite registry of favor/service types.
- Examples may include help_with_task, repair_item, cover_shift, accompany_expedition, teach_skill, deliver_item, mediate, or other actions actually supported by scheduler/gameplay.
- Each favor definition specifies parameters, validation, fulfillment source event, deadline policy, and burden/value model.
- Free text is presentation only.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 31. No Free-Text Mechanical Favors
- The source's `favorDescription` cannot be sufficient authority.
- Free text can annotate a favor, but the mechanical obligation must be typed.
- Otherwise the game cannot determine whether it was repaid.
- This is a flagship correction.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 32. Favor Obligation DTO
- Recommended fields: favorId, definitionId, debtorId, creditorId, sourceTradeId, parameter refs, owedAt, dueAt, status, fulfilledEventId, resolvedAt, disputeId.
- Use open/fulfilled/waived/failed/disputed/cancelled states.
- `isRepaid` alone is insufficient.
- Stable IDs.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 33. Favor Fulfillment
- Owning scheduler/task/expedition/teaching system emits a matching fulfillment fact.
- BarterSystem verifies it against obligation parameters and marks fulfilled once.
- Player cannot click `Mark Repaid` without a real event unless a deliberate manual social-resolution mechanic exists.
- Partial fulfillment requires explicit favor definition support.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 34. Favor Deadline
- Use optional due time/day.
- Expiry does not automatically mean malicious default.
- At deadline, create overdue state and allow grace/renegotiation if policy supports.
- Trust/reputation changes on meaningful failure, not one missed minute.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 35. Favor Waiver
- Creditor can voluntarily waive an obligation.
- Waiver may improve relationship/social capital depending RelationsSystem.
- No negative default penalty.
- Record provenance.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 36. Favor Transfer
- Do not allow selling/transferring favors to third parties in v1.
- Keep obligations bilateral.
- Future debt-network plan could extend it.
- This prevents opaque debt markets.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 37. Favor Capacity
- Do not allow a survivor to owe dozens of impossible services.
- Use per-survivor open-favor cap or workload sanity check.
- Acceptance revalidates availability.
- Favors must not bypass canonical scheduler capacity.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 38. Trade Reputation Definition
- Pairwise `TradeReputation` measures economic reliability/fairness, separate from general relationship.
- Use bounded 0–100 or basis points.
- Store only pairs that have interacted.
- Do not precreate O(N²) records for every survivor pair.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 39. Sparse Pairwise State
- Create reputation row lazily on first barter-related interaction.
- Use canonical ordered pair key if reputation is symmetric, or directional key if trust differs by observer.
- Decide explicitly.
- The source implies pair reputation but traderA/traderB could still be directional.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 40. Directional Reputation
- Recommended: reputation is directional because A may trust B more than B trusts A after asymmetric favors/disputes.
- Store `(observer, counterparty)` records.
- Completed fair trade can update both directions, possibly differently.
- UI can show mutual summary.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 41. Reputation Inputs
- Fair completed trade, fulfilled favor, waived debt, verified dispute outcome, fraud/cheating, repeated defaults.
- Do not increase reputation merely because an offer was posted.
- Counter-offers/rejections are normal negotiation and should not be penalized by default.
- Relationship state can influence initial willingness but remains separate.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 42. Reputation Bounds
- Clamp values.
- Use diminishing returns at high trust.
- Large one-off fraud can cause meaningful decline.
- Routine tiny trades should have very small impact.
- This prevents grinding.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 43. Reputation Tags
- Source tags fair/dealing/honest/cheater should be derived from evidence, not manually toggled.
- Use registered tags such as reliable, generous, hard_bargainer, frequent_defaulter, disputed.
- Avoid morally loaded permanent labels from one event.
- Tags may decay/clear under explicit rules.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 44. Credit / Deferred Exchange
- High trust can permit favor/deferred obligations rather than creating monetary credit.
- Do not implement numeric debt currency in v1.
- Item-now/favor-later is sufficient informal credit.
- Future lending system can build separately.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 45. Survivor Relations Integration
- Trade emits bounded social events such as fair_trade, generous_gift, fulfilled_favor, broken_promise, mediated_dispute.
- SurvivorRelationsSystem owns relationship/trust changes.
- Do not write relationship score directly.
- Repeated low-value events should be rate-limited/diminishing.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 46. Trade Reputation vs Relationship
- Do not collapse trade reputation into friendship.
- Survivors may dislike each other but trade reliably, or be friends but poor traders.
- Negotiation can consult both.
- UI should label them separately.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 47. Gift Economy
- Gift is one-way transfer of an eligible personal item with no contractual return.
- Gift transaction uses the same ownership/settlement safeguards.
- Relationship/social-capital consequence is emitted once.
- Do not convert communal resources into gifts without allocation authority.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 48. Gift Value Diminishing Returns
- Relationship benefit depends on recipient utility, significance, relationship context, and recent gift history.
- Repeated trivial gifts should rapidly diminish.
- Same item cannot be gifted back and forth for unlimited social capital.
- Use pair/item/recent-history anti-farming.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 49. Gift Return
- If recipient later gifts the item back, that is a new real transaction but should not recreate the original relationship bonus at full strength.
- Item provenance from Plan 190 can preserve the chain for meaningful durable items.
- BarterSystem does not own item lore.
- Anti-loop test required.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 50. Gift Refusal
- Recipient may refuse based on item desirability, relationship, ownership capacity, or policy.
- Refusal should not automatically damage relationship.
- Do not force survivors to accept harmful/irrelevant items.
- Deterministic evaluation.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 51. Dispute Definition
- A dispute must reference a completed/accepted trade or favor obligation.
- Reason codes: item_not_delivered, invalid_item_condition_if_condition_was_promised, favor_overdue, favor_failed, fraud_confirmed, misunderstanding, other_system_verified_reason.
- Do not allow arbitrary free-text accusation to directly alter reputation.
- Stable dispute ID.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 52. Item Condition Disputes
- Only support 'not as described' if offer terms actually captured item condition/quality expectations.
- If item condition was visible and unchanged, no automatic fraud.
- Item system provides historical condition if needed.
- Do not invent deception mechanics.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 53. Fraud Boundary
- V1 barter should be consensual and truthful unless Plan 153/other deception systems explicitly support fraud.
- Do not randomly label a trade 'cheating'.
- Disputes can arise from failed favors or actual invalid settlement.
- Future deception can add intentionally misrepresented terms.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 54. Dispute State Machine
- Recommended: opened → mediation_requested/investigating → resolved/withdrawn/unresolved.
- Resolution includes responsible party/neutral/no-fault if another authority determines it.
- Do not auto-resolve by RNG unless a mediation system intentionally does so.
- Persist stable IDs.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 55. Mediation Boundary
- Leadership/conflict/mediation system owns mediation outcome if available.
- BarterSystem supplies trade/favor evidence.
- Do not create a generic judge simulation inside barter.
- If no mediation system exists, allow bilateral settlement/waiver only.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 56. InterpersonalConflict Integration
- Plan 202 can consume broken promise/dispute/escalation facts.
- Conflict owns arguments, hostility, reconciliation.
- Barter does not directly spawn fights.
- One dispute creates at most one conflict trigger unless escalated later.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 57. Dispute Reputation Effects
- Only apply after outcome or clear default event.
- Opening a dispute alone should not prove guilt.
- False/mutual/no-fault resolutions affect reputation differently.
- Relations/TradeReputation receive typed outcome.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 58. Informal Market Toggle
- Global barter-enabled setting can disable new offers while preserving historical trades/favors.
- Existing obligations still need resolution or explicit cancellation policy.
- Do not delete state when disabled.
- Useful accessibility/complexity setting.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 59. Market Activity Level
- Do not autonomously generate hundreds of offers every day.
- Use event/schedule cadence and per-survivor caps.
- Player can opt into manual offers or allow limited AI-driven informal market activity.
- Default activity should be sparse.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 60. AI Offer Generation
- If AI survivors autonomously create offers, use deterministic need/preference triggers at low cadence.
- Only offer personally disposable items/services.
- Do not scan every survivor×item×survivor combination each tick.
- Generate candidate offers from indexed needs/owned surplus.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 61. Offer Generation Cadence
- Evaluate at day boundaries or meaningful inventory/need changes, not per frame.
- Apply cooldown per survivor.
- Respect max active offers.
- Stable seed if selection among equivalent candidates is needed.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 62. Heavy Market Performance
- Stress 100 survivors, thousands of personal items, 500 active listings, and 1,000 historical trades.
- Offer lookup indexed by target/seller/item category.
- No O(N²×items) scan every tick.
- History pagination.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 63. Negotiation Fatigue
- Cap counter depth and concurrent negotiations.
- Do not create an endless dialogue tree.
- Expired/rejected negotiations close cleanly.
- UI can offer accept/reject/counter with concise rationale.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 64. Informal Price Board Expiration
- Listings expire based on campaign time.
- Expiry releases reservations.
- No per-frame scanning; use expiry queue/index.
- Expired history can be omitted or compacted.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 65. Personal Need Pricing
- Survivor may ask more for personally important/rare item and less for surplus.
- Need signals must come from actual Needs/preferences/ownership context.
- Do not generate arbitrary personality multipliers without source.
- Explain broad rationale in UI, not hidden exact formula.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 66. Scarcity Input
- Shelter-wide scarcity can influence perceived value, but beware communal-resource leakage.
- Use aggregate availability as context only.
- A survivor cannot claim ownership of scarce communal goods.
- Scarcity increases willingness/value for personal substitutes.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 67. Item Condition/Quality
- Use canonical item condition/quality if relevant to value.
- Offer snapshots promised condition/identity at posting if needed.
- Settlement revalidates item identity and acceptable condition delta.
- Do not create parallel condition state.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 68. Sentimental/Item-Lore Value
- Plan 190 item provenance may influence a survivor's willingness to trade a personal heirloom.
- ItemLore provides significance tags; barter applies valuation policy.
- Do not move item lore into barter state.
- Legendary/heirloom item may be nontradeable or highly valued.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 69. Quest/Critical Item Protection
- Quest-critical and system-critical items cannot enter informal barter unless explicitly permitted.
- Use canonical item reservation/protection flags.
- Prevent soft-locks.
- Data-integrity test protected categories.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 70. Weapon/Armory Boundary
- Personal weapons may be tradeable only if ownership policy permits.
- Shelter armory weapons are communal and not personal barter stock.
- Security/access control from Plan 209 may protect physical access but is not ownership.
- Do not bypass armory policy through barter UI.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 71. Medicine/Food Boundary
- Shared shelter medicine/food remains communal.
- Personal rations/medicine only tradeable if ownership/allocation system explicitly supports them.
- This is critical to preserve survival balance.
- Do not create private hoarding implicitly.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 72. Service/Favor Task Integration
- Favor definitions should map to real tasks/jobs where possible.
- Examples: repair a personal item, cover a duty shift, teach a skill, accompany a mission.
- Task system emits fulfillment.
- Barter does not schedule hidden background labor.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 73. Cover Shift Favor
- Requires DutyRoster/task authority.
- Favor parameter references shift/task ID or workload category.
- Fulfillment occurs only after the substitute actually completes it.
- Do not mark repaid at acceptance.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 74. Repair Favor
- References specific item/job.
- Crafting/repair system performs work and emits completion.
- Materials ownership/payment must be explicit.
- Barter does not directly repair.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 75. Teaching Favor
- Requires teaching/apprenticeship system.
- Favor references skill/topic/session requirement.
- Fulfillment on real teaching completion.
- No free XP.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 76. Expedition Help Favor
- Requires canonical ExpeditionSystem assignment.
- Favor can be satisfied by participating in a specified expedition or objective.
- Do not force survivor assignment outside normal eligibility.
- Completion follows expedition result policy.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 77. Item Delivery Favor
- Could overlap item exchange; avoid redundant modeling.
- Use favor only when delivery later is the service.
- Actual item transfer still uses Inventory.
- Do not create synthetic item state.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 78. Favor Value
- Value can depend on expected labor time, risk, skill, urgency, and relationship context.
- Use task metadata if available.
- Do not create arbitrary value from text length.
- Favor valuation is internal negotiation utility, not currency.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 79. Overdue Favor Handling
- At due time, mark overdue and notify involved parties/player if appropriate.
- Apply a grace window by favor type.
- Only after actual default should reputation/relations receive negative event.
- Allow renegotiation/waiver.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 80. Favor Renegotiation
- Creditor/debtor can extend deadline or substitute an equivalent registered favor if both consent.
- Create amendment linked to original obligation.
- Do not silently edit original terms.
- Keep audit trail sparse.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 81. Favor Default
- Default is a terminal failure after policy/grace or explicit refusal.
- Emit broken_favor fact once.
- Can open dispute.
- Do not repeatedly penalize every day overdue.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 82. Trade History
- Persist completed significant trades and compact low-value repetitive history if needed.
- Keep stable trade IDs, participants, term summaries, day, outcome, dispute links.
- Do not duplicate full item definitions.
- Use references.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 83. Trade History Retention
- Recent full details, older compact summaries, and permanent records for disputed/important trades.
- Plan 190 item history can preserve durable-item provenance separately.
- Do not retain expired rejected offer details forever unless needed for negotiation memory.
- Set explicit policy.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 84. Offer History Retention
- Rejected/expired offers can be dropped or compacted after short window.
- Accepted/counter chain can compact into final negotiation summary.
- Keep root negotiation ID and final terms.
- This prevents save bloat.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 85. Trade Reputation Persistence
- Persist only interacted directional pairs.
- Store score, counts, evidence tags/counters, last meaningful trade day, and maybe recent trend.
- Do not store every reputation change event indefinitely.
- Recompute tags from retained aggregates.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 86. Reputation Decay
- Optional: old trade reputation can slowly regress toward neutral if no interaction for long periods.
- Do not implement unless social systems use similar decay.
- If enabled, deterministic day-based rule.
- History remains.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 87. Initial Trade Reputation
- Do not default every pair to 50 persistent rows.
- Use implicit neutral baseline.
- Relationship may influence initial willingness separately.
- Create row lazily.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 88. Relationship Bootstrap
- Close friends may accept less favorable offers because of relationship, but that is not the same as trade reputation.
- Enemies may refuse even economically fair offers.
- Use bounded modifiers.
- Do not let relationship override consent/availability.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 89. Consent Guardrail
- Every trade acceptance is a real consent decision or explicit player-commanded survivor action consistent with autonomy rules.
- No forced extraction of personal goods through barter.
- Plan 144 autonomy may affect willingness.
- Shared shelter policy actions belong elsewhere.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 90. Player Agency Mode
- Decide whether player directly negotiates on behalf of survivors, merely observes AI barter, or can intervene.
- Support a clear default.
- Do not mix hidden autonomous trades with full player ownership expectations without notifications/settings.
- Settings can limit autonomy.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 91. Autonomous Barter Safety
- AI cannot barter away survival-critical personal equipment, quest items, or shelter-reserved gear.
- Respect survivor needs and future task reservations.
- Use disposable-item policy.
- Player can lock/favorite items if ownership system supports it.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 92. Notification Policy
- Notify significant/interesting offers, major gifts, disputes, defaulted favors, or high-value trades.
- Do not notify every tiny autonomous exchange.
- Price-board listings can remain passive.
- Use preferences/cooldowns.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 93. Internal Communication Integration
- Plan 211 can publish barter listings/requests/notices.
- BarterSystem owns offer/listing state.
- Communication owns delivery/visibility/read state.
- No duplicate listing lifecycle.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 94. Shelter Reputation Integration
- Source says pairwise trade reputation affects shelter reputation; apply only for genuinely shelter-level patterns/major scandals if such system exists.
- Ordinary private trades should not change external shelter reputation.
- ShelterReputationSystem owns score.
- Export aggregate/major facts only.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 95. External Black Market Boundary
- Plan 155 covers external underground economy.
- Internal barter should not become a route to external black-market inventory/price state.
- A survivor could later act as intermediary through a separate bridge.
- Keep ledgers distinct.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 96. Trade Route Boundary
- Plan 192 external routes remain settlement/player economy.
- Internal survivor price board is local.
- Do not copy trade-route prices into survivor offers as authority.
- External market reference may inform perceived value only.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 97. Gift/Barter Classification
- Gift is not a zero-price barter if social consequences differ.
- Keep explicit `gift` trade type.
- Mixed exchange contains at least one obligation/return term.
- Classification used for analytics/quests.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 98. Trade Status
- Recommended: negotiating/accepted_pending_settlement/completed/completed_with_open_favor/disputed/voided.
- A fully transferred item trade is completed immediately.
- Open favor status lives on obligation; trade can still be completed with debt.
- Do not leave every favor trade 'pending' until months later.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 99. Voided Trade
- Voided means settlement never validly completed or was reversed by explicit authoritative action.
- Do not use voided to silently undo a completed item exchange after a dispute.
- Dispute outcomes usually create compensation/new transfer, not time travel.
- Audit trail remains.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 100. Compensation Settlement
- Mediation may create a new compensation transfer/favor.
- Link it to dispute.
- Do not rewrite original trade terms.
- Settlement is again transactional and consensual/authority-backed.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 101. Barter Event Taxonomy
- Retain Offer, Trade, Counter, Dispute, Favor, Gift, Price, Reputation as presentation themes.
- Core typed events should be specific: offer_posted, offer_countered, trade_settled, favor_created, favor_fulfilled, favor_defaulted, dispute_opened/resolved, listing_posted/expired.
- Do not emit reputation_changed for every tiny recalculation.
- Use stable IDs.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 102. No Generic `TickSurvivorBarter` Hot Loop
- Use offer expiry queue, favor due checkpoints, low-cadence AI offer generation, and event-driven settlement.
- No per-frame survivor×survivor scanning.
- Daily tick may handle expiry/due/AI candidate generation.
- Headless deterministic.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 103. Deterministic RNG
- Core settlement/availability is deterministic and RNG-free.
- Negotiation AI variety may use `ISeededRng` keyed by negotiation root + participants + context revision.
- Dispute outcome is not randomly decided here.
- No wall clock.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 104. Offer Valuation Versioning
- Store rules/profile version or persist accepted terms so balance patches do not retroactively change existing offers unpredictably.
- Pending offers may preserve their quoted terms/value snapshot.
- Future offers use new rules.
- Document save compatibility.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 105. Barter Rules Catalog
- Create `Assets/StreamingAssets/Data/barter_rules.json` for policy: offer caps, expiry defaults, counter limits, valuation modifiers, favor definitions or references, reputation deltas, anti-farming cooldowns, AI cadence.
- Do not put arbitrary executable formulas in JSON.
- Use registered rule kinds.
- Schema-version it.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 106. Item Valuation Profiles
- Optional data profiles by item category/tag can define broad barter desirability.
- External item base values remain canonical if available.
- Personal need/attachment modifies.
- Do not duplicate the entire MarketSystem price catalog.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 107. Favor Catalog
- Strong recommendation: use a dedicated section/file for favor definitions if list becomes large.
- Every mechanical favor needs fulfillment adapter and test.
- Reject orphan favor types.
- Localize labels/descriptions.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 108. Data Integrity Validation
- Validate barter rule IDs, item/tag references, favor definitions, task adapters, reputation deltas, expiry/counter bounds, localization, and ownership-policy compatibility.
- Validate no base-game favor lacks a fulfillment source.
- Validate protected item policies.
- Fail CI on orphan references.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 109. Personal Belongings Coverage Report
- Generate `docs/survivor_barter/PERSONAL_BELONGINGS_BARTER_AUDIT.md`.
- List all item ownership categories and whether they are barterable.
- Identify shared-only, personal, assigned-but-not-owned, quest-locked, consumable, equipped, sentimental, and reserved semantics.
- This report is a release prerequisite.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 110. Favor Coverage Report
- Generate `docs/survivor_barter/FAVOR_COVERAGE.md`.
- Columns: favor definition, parameters, owning task system, fulfillment event, default policy, valuation profile, fixture.
- Remove any favor with no real fulfillment path.
- Do not count free-text placeholders.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 111. Valuation Report
- Generate `docs/survivor_barter/BARTER_VALUATION_CALIBRATION.md`.
- Test common item pairs, high scarcity, high/low relationship, high/low trade reputation, urgent need, sentimental item, and favor burden.
- Ensure no one modifier dominates absurdly.
- Document value bands rather than exposing hidden currency.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 112. Reputation Calibration
- Generate `docs/survivor_barter/TRADE_REPUTATION_CALIBRATION.md`.
- Simulate fair trades, defaults, resolved disputes, gifts, and wash-trade attempts.
- Target slow, meaningful change.
- High reputation should require history, not 10 cheap swaps.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 113. Anti-Wash-Trading
- Repeated exchange of the same item(s) between the same pair should yield sharply diminishing or zero reputation/relationship/quest progress.
- Use recent item instance IDs, pair, and trade-value band.
- Do not forbid legitimate return trades; only suppress farming rewards.
- Mandatory exploit test.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 114. Anti-Gift-Farming
- Same pair repeatedly gifting low-value items receives diminishing social benefit.
- Returning the same item does not reset cooldown.
- Quest `give 10 gifts` should require unique meaningful gifts or value threshold if intended.
- Define exact completion semantics.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 115. Anti-Price-Farming
- `set 30 informal prices` is vulnerable to listing spam.
- Quest should count unique legitimate listings/items or sustained market activity, not create/cancel same listing 30 times.
- Price posting itself should not alter reputation.
- Stable listing IDs.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 116. Anti-Counter-Farming
- `successfully counter 10 offers` should require distinct negotiations and eventual accepted counter, not infinite self-counter spam.
- Counter depth is capped.
- Same root negotiation counts once according to quest rule.
- QuestSystem owns progress.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 117. Anti-Favor-Farming
- Create/waive/recreate same trivial favor cannot farm trust.
- Fulfillment event must be real and value/burden above configured threshold for rewards.
- Unique obligation IDs and pair cooldown.
- Mandatory fuzz/property tests.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 118. Anti-Inventory-Duplication
- Reserve item once.
- Settlement transaction ID consumed once.
- Save/load during transfer cannot replay grants/removals.
- Split/merge item stacks preserve claim quantities.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 119. Anti-Shared-Stock-Theft
- No barter query can select communal item unless an ownership/allocation authority marks it personal/disposable.
- UI filters them out.
- Core still revalidates.
- Architecture test against direct raw Inventory transfer bypass.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 120. Anti-Task-Reservation Conflict
- An item reserved for expedition/crafting/medical task cannot be bartered.
- If barter reservation existed first, task assignment sees it unavailable.
- One canonical reservation service if possible.
- No last-write-wins races.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 121. Anti-Death/Departure Exploit
- Participant death/departure invalidates pending offers.
- Personal estate/inheritance handles owned items; barter does not auto-complete.
- Open favor debts close/resolve via policy without reputation farming.
- Completed trade remains historical.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 122. Participant Eligibility
- Validate alive/present/available and autonomy state.
- Incapacitated survivor may not negotiate unless design allows deferred/represented decision.
- Visitors/non-residents are excluded unless explicitly integrated.
- Canonical survivor IDs.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 123. New Survivor Defaults
- No pairwise trade reputation rows are precreated.
- New survivor can trade if personal ownership and settings permit.
- Initial willingness uses relationship/personality/need state if available.
- No hidden negative newcomer tax unless design says so.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 124. Old-Save Migration
- Initialize no active offers, listings, favors, disputes, or pair reputation rows.
- Do not infer past trades from item ownership.
- Personal belongings migrate through Plan 210/ownership authority separately.
- Barter disabled/enabled follows default setting without notifications.

Implementation consequence: this concern must resolve through a real ownership/availability source, stable transaction or obligation ID, deterministic rule path, bounded social/economic consequence, save-safe reconciliation, and projection-only UI. If any of those pieces is missing, the barter path should fail closed rather than infer ownership or completion.

---
## 125. Migration With Personal Belongings
- If old save gains personal belongings through Plan 210 migration, that does not mean those items were historically traded.
- Start barter history at activation.
- No reputation bootstrap from ownership.
- Future offers use normal rules.
