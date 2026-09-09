# CONTRABAND TRADE AND ARBITRAGE AUDIT — Plan 147 Task A.7 (prices) / Task C.2–C.3

## 1. `market_price_scrip` is not authoritative — reconciliation result

**Finding:** the repo has **no scrip currency runtime**. "Scrip" appears in
narrative prose and a few catalogs, but no wallet, mint or scrip-denominated
transaction exists. `market_price_scrip` therefore cannot be reconciled to a
live currency and is classified **DESCRIPTIVE** in the authority matrix.

**Consequence (enforced):** the contraband layer never converts scrip to
anything. The executable trade value of an activated contraband record is the
**canonical item `tradeValue`**, owned by items.json and consumed by the
existing barter/economy systems:

| Activated record | market_price_scrip (descriptive) | Canonical item tradeValue (authoritative) |
|---|---|---|
| card deck | 15 | `item_playing_cards` = 12 |
| sugar brick | 60 | `sugar` = 2 × 8 units = 16 |
| wheat seed vial | 200 | `item_seed_wheat` (greenhouse_items.json) |

Note the scrip column does not even rank-correlate perfectly with canonical
value — more proof that treating it as a price would fork pricing authority.

## 2. No second pricing authority (Plan Task A.7)

`trade_value_bonus` (condenser coil) and `trade_multiplier` (coffee) are
DESCRIPTIVE. `ShelterBarterSystem` valuation is per-transaction:
`GetBaseItemValue(item) × count × bp-multipliers (demand premium, winter
scarcity, caravan price multiplier)`. There is deliberately **no hook** for a
global "holds contraband → better prices" modifier; adding one would be a new
economy mechanic requiring its own plan, not a contraband field.

## 3. Arbitrage analysis for the three activated slices

- **Stash route is acquisition, not minting.** Each activated stash is claimable
  **exactly once per campaign**. The canonical items granted enter the same
  economy as any other items — they can be bartered away, at which point the
  player no longer holds them. There is no buy-low/sell-high loop because there
  is no repeatable purchase: no contraband merchant stock, no refresh.
- **No free conversion:** the claim grants items; it never grants currency,
  and never converts scrip→value. A player cannot mint scrip through the
  stash (there is no scrip).
- **Sell-side:** if the granted canonical item is offered to a caravan, the
  caravan pays by its normal base-value table. `item_playing_cards` is not in
  `ShelterBarterSystem._itemBaseValues` and falls to the neutral 5.0 fallback —
  cheaper than the item's own tradeValue 12, so no arbitrage vs. the item
  authority; the fallback is an existing economy behavior, unchanged by this
  plan.
- **Composition check (Task C.2):** the granted items do not stack with any
  stance/reputation/scarcity modifier in a way that doubles value: barter
  modifiers are caravan-side, not player-inventory-side; contraband possession
  adds no modifier.

## 4. Barter acquisition route (IMPLEMENTED — Plan 147 follow-up session 4)

The deferred barter route is now implemented on **`ShelterBarterSystem`** (the
Plan-54 Core barter authority, Plan-147 host wiring):

- **The contraband broker** (`ContrabandBrokerCaravan.Build`,
  `caravan_contraband_broker` / "The Quiet Counter") is built FROM the
  reviewed activation map — the single gate authority. Each activation becomes
  one stock line: canonical item, activation quantity, activation day gate.
- **Canonical pricing, no second authority:** the broker is flagged
  `use_canonical_item_values`; ShelterBarterSystem resolves its base values
  from the canonical item `tradeValue` (via the injected item lookup) instead
  of the legacy table. A visible **25% scarcity premium** (12500 bp) is added
  on top: contraband costs strictly MORE than its trade value, so every
  buy→sell round trip loses value — pinned by
  `Broker_NoBuySellArbitrage_RoundTripLosesValue` (per-line and end-to-end)
  and the selftest. Existing caravans' prices are byte-identical (table
  first; the flag is broker-only) — pinned by
  `LegacyCaravans_Pricing_UnchangedByBrokerPath`.
- **High-tier availability from real campaign state:** stock lines carry
  `available_from_day` (mirroring the activation day gates; parity pinned by
  `Broker_BuildsFromActivations_Deterministically`). The gate is evaluated
  once per restock (caravan arrival); stock is then pinned for the whole
  stay — `Broker_StockPinnedDuringStay_NoRerollByReopen`,
  `Broker_SaveRoundTrip_PreservesStockAndGates`,
  `Broker_Deterministic_IdenticalSequencesProduceIdenticalStock`.
- **scrip remains descriptive** — the broker trades goods for goods through
  the atomic `InventoryBill` pipeline; no currency is minted or converted.
