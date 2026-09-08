# Holdfast Rejection Semantic Matrix

## 1. Composition Syntax
In `HoldfastDispatchLog.OnRejected(HoldfastTradeResult result, string factionId)`:
```
"Requisition refused: " + detail + " " + voice.rejected
```
Where `detail` is determined by `result.Failure`:
- `InvalidQuantity`: "Quantity must be at least one."
- `InsufficientFunds`: "Available value is below the listed worth."
- `InsufficientStock`: "The selected counterparty has no stock at that quantity."
- `InsufficientInventory`: "No holdings of this item are available for transfer."
- `InventoryCapacity`: "The inventory cannot hold that quantity."
- `InvalidPrice`: "The listed value cannot be represented safely."
- `UnknownItem`: "The selected item is not in the Holdfast catalog."
- `UnknownFaction`: "No valid Holdfast counterparty is selected."
- `UnavailableOrRestricted`: "This supply remains reserved under current Holdfast restrictions."
- default: "Transaction declined."

## 2. Rejection String Safety Audit

| Faction ID | Authored `rejected` Text | Mechanical Reason Avoidance | Safe Institutional Framing |
|---|---|---|---|
| `faction_the_office` | `Requisition denied — the authorising stamp is absent or the ledger balance does not cover the line item.` | Does not fabricate a single cause; provides alternate institutional grounds. | Bureaucratic procedure and balance requirements. |
| `faction_the_cutters` | `No stock to release and no credit to draw against. The Cutters do not float empty requisitions.` | Covers both buy (no stock) and sell/credit failure paths safely. | Rough salvage and debt accounting. |
| `faction_the_fleet` | `The manifest does not clear. Either the berth is closed or the hold cannot accept the transfer.` | Uses maritime berth/hold metaphor without asserting factual weather. | Naval logistical protocol. |
| `faction_black_flotilla` | `Claim unacknowledged. The Flotilla does not lower tackle or part with raised stock without verified barter in the net.` | Reason-agnostic; focuses on salvage claim verification and net barter. | Privateer salvage posture. |
| `faction_supply_corps` | `Requisition declined. Your allotment chit lacks valid quota authorization or the district issue window has expired.` | General administrative refusal compatible with fund, stock, or restriction failures. | Ration depot quota enforcement. |
| `faction_railway_guild` | `Waybill refused. The tonnage exceeds your line credit or the destination switch remains clamped.` | Bridges financial, volume, and clearance restrictions without hardcoded numbers. | Rail logistics enforcement. |
| `faction_hydro_barons` | `Tap closed. No discharge authorized until previous draw accounts are cleared or certified filter stock is provided.` | Severe monopoly refusal framed around account balance or filter media. | Water syndicate embargo. |
| `faction_ordnance_foundry` | `Batch declined. The forge cannot accept uncertified scrap or float requisitions against unproved alloy.` | Applies equally to trade-in scrap or credit requisition failures. | Industrial foundry quality discipline. |
