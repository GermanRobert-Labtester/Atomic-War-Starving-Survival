# Trade Negotiation & Tell Integration

**Document Version:** 1.0.0
**Authority:** `Assets/StreamingAssets/Data/trade_tell_lines.json`
**Engine:** `Assets/Ashfall.Core/Economy/TradeTellEngine.cs`
**Integration Status:** `LIVE` (Operational across all 15 scenarios)

---

## 1. Tell Selection Mechanism

In ASHFALL, trade negotiation tells are not arbitrary strings hardcoded in UI views. They are data-defined in `trade_tell_lines.json` and selected deterministically by `TradeTellEngine.TrySelectTell`:

```csharp
public bool TrySelectTell(TradeStance stance, float trust, ISeededRng rng, out TradeTell tell)
```

The tell engine maps:
1. **Scenario Stance** (`HostileRaid`, `Rob`, `Refuse`, `Trade`, `ShareIntel`)
2. **Scenario Trust** into four discrete trust bands:
   - `hostile`: Trust $\in [-100, -40]$
   - `wary`: Trust $\in [-39, 0]$
   - `neutral`: Trust $\in [1, 40]$
   - `warm`: Trust $\in [41, 100]$
3. **Deterministic Selection:** `ISeededRng.Next(pool.Count)` picks the exact posture line.

---

## 2. Scenario-to-Tell Mapping Across All 15 Scenarios

| Scenario ID | Stance | Trust | Resulting Trust Band | Representative Selected Tell Line |
|---|---|---|---|---|
| `fair_deal` | `Trade` | +22 | `neutral` | *"They count the crates twice. Standard procedure, no malice in it."* |
| `offer_short` | `Trade` | -5 | `wary` | *"The sentry's hand stays resting on the holster flap."* |
| `empty_table` | `Refuse` | -25 | `wary` | *"The shutters are drawn. A cardboard sign says 'Gone south'."* |
| `last_vials` | `Trade` | +10 | `neutral` | *"Their hands tremble slightly as they set the scale weights."* |
| `winter_cart` | `Trade` | -15 | `wary` | *"They wrap their coat tighter, eyeing the horizon, not your face."* |
| `depot_window` | `Trade` | +45 | `warm` | *"The clerk stamps the docket without looking up. You're on the list."* |
| `emergency_requisition` | `Trade` | +15 | `neutral` | *"An armed escort stands two paces behind the logistics officer."* |
| `back_room_exchange` | `Trade` | +5 | `neutral` | *"Lantern turned low; shadows hide whatever is under the counter tarp."* |
| `ledgerless_broker` | `Rob` | -35 | `wary` | *"Their eyes do the arithmetic on everything you carry."* |
| `long_road_caravan` | `Trade` | +18 | `neutral` | *"Water bottles clink on the pack mules. Business as usual."* |
| `salvage_caravan` | `Trade` | +12 | `neutral` | *"Grease on every finger. They weigh the iron, not the words."* |
| `settlement_of_accounts`| `Refuse` | -45 | `hostile` | *"The debt ledger is open on the table with a red cross through your mark."* |
| `crate_lot` | `Trade` | +25 | `neutral` | *"Forklift idling outside. They want pallets moved before dark."* |
| `border_runner` | `ShareIntel` | +20 | `neutral` | *"They spread a greaseproof map under the edge of the battery pack."* |
| `road_knowledge` | `Trade` | +10 | `neutral` | *"Children peek out from behind the handcart canvas. Anxious silence."* |

---

## 3. Plan 62 Tell Integration Status

- **Status:** `LIVE`.
- `trade_tell_lines.json` contains 4 trust bands and complete tell sets for all 5 stances.
- `TradeScreenPresenter` and `TradeScreenScenarioLoader.CreateBinding` seamlessly bind tells onto `ITradeScreenViewModel.StanceTellLine` and `StanceTellId`.
- No dangling tell IDs exist; tell line selection is fully verified in `TradeScreenSeamTests`.
