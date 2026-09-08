# Holdfast Sold Semantic Matrix

## 1. Composition Syntax & Directionality
In `HoldfastDispatchLog.OnSale(string itemId, int quantity, long totalValue, string factionId)`:
```
{quantity} + " × " + ItemRef(itemId) + " accepted. " + {totalValue} + " credited. " + voice.sold
```
This method is invoked exclusively by `HoldfastTerminalPanel.PressSell()` when the player sells held inventory items to the merchant:
- The items move from player inventory into merchant stock.
- Player's available value increases by `totalValue`.
- The counterparty accepts the cargo/item into their custody/depot/pile.

## 2. Acceptance String Safety Audit

| Faction ID | Authored `sold` Text | Directionality Compatibility | Semantic Safety Guarantees |
|---|---|---|---|
| `faction_the_office` | `Accepted for inventory. The Office records the transfer and adjusts the manifest accordingly.` | Player sells → Office accepts. | Accurately describes transfer into office custody. |
| `faction_the_cutters` | `Taken to the pile. The cutter ledger shifts; your credit moves the other way.` | Player sells → Cutters accept onto scrap pile. | Accurately describes credit moving toward player. |
| `faction_the_fleet` | `Logged and cleared. The Fleet manifest now shows the item transferred to your custody.` | Baseline legacy phrasing. | Preserved verbatim for exact parity. |
| `faction_black_flotilla` | `Brought across the gunwale. The dive ledger records the exchange and the deck watch tallies the balance.` | Player sells → Flotilla brings item aboard. | Clear receipt across ship gunwale; balance tallied. |
| `faction_supply_corps` | `Delivered to the depot cage. The supply tally is marked off and credit is posted to the district ledger.` | Player sells → Supply Corps places in depot cage. | Receipt into warehouse and credit posted to account. |
| `faction_railway_guild` | `Loaded onto the freight flatcar. The waybill is countersigned and transit credit is logged to your siding.` | Player sells → Railway Guild loads onto rolling stock. | Waybill countersigned and credit issued to player siding. |
| `faction_hydro_barons` | `Poured into the cistern manifold. The intake meter ticks and clean-water allowance is credited to your valve.` | Player sells → Barons accept into manifold. | Safe receipt of chemicals/resins/filters; credit recorded. |
| `faction_ordnance_foundry` | `Weighed at the charging dock. The casting tally is notched and munitions credit is struck in the yard book.` | Player sells → Foundry weighs scrap/parts at dock. | Tally notched, munitions/credit recorded in book. |
