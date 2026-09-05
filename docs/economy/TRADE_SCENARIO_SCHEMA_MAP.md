# Trade Scenario Schema Map

**Document Version:** 1.0.0
**Authority:** `Assets/StreamingAssets/Data/trade_screen_scenarios.json`
**Consumer:** `Assets/Ashfall.Core/Economy/TradeScreenScenarios.cs` (`TradeScreenScenarioLoader`)

---

## 1. Document Structure

```json
{
  "schema_version": 1,
  "$schema": "./schema/trade_screen_scenarios.schema.json",
  "version": 1,
  "description": "string",
  "scenarios": [
    {
      "id": "scenario_identifier",
      "faction_id": "faction_identifier",
      "faction_name": "Faction Display Name",
      "leader_name": "Leader Name",
      "succession_generation": 1,
      "stance": "Trade",
      "trust": 0.0,
      "aggression": 0.0,
      "consecutive_repels": 0,
      "has_surrendered": false,
      "can_demand_parley": false,
      "world_phase": "CivilWar",
      "world_day": 1,
      "price_shocks": [],
      "scarcity": [],
      "player_offers": [],
      "biological_offers": {},
      "faction_demands": [],
      "expected_fairness": "fair",
      "confirm_succeeds": true,
      "radio_ticker": "RADIO: ..."
    }
  ]
}
```

---

## 2. Field Specifications

| JSON Key | Type | Fallback | Runtime Destination | Live Consumer & Semantics |
|---|---|---|---|---|
| `id` | `string` | `""` | `TradeScreenScenario.Id` | Stable scenario key. Gated in tests; resolved in scenario lookups. |
| `faction_id` | `string` | `""` | `TradeScreenScenario.FactionId` | Authoritative faction key. Queries `faction_radio_corpus.json` and `FactionStanceEngine`. |
| `faction_name` | `string` | `FactionId` | `TradeScreenScenario.FactionName` | UI presentation label in header and quote summaries. |
| `leader_name` | `string` | `""` | `TradeScreenScenario.LeaderName` | UI presentation label for faction leader. |
| `succession_generation` | `int` | `1` | `TradeScreenScenario.SuccessionGeneration` | UI generation count indicator (`gen N`). |
| `stance` | `string` | `Refuse` | `TradeScreenScenario.Stance` | Parsed to `TradeStance` (`Trade`, `Refuse`, `ShareIntel`, `Rob`, `HostileRaid`). Controls `willTrade` and `CanConfirm`. |
| `trust` | `float` | `0.0f` | `TradeScreenScenario.Trust` | Table-edge trust meter (-100 to 100). Determines `TradeTellEngine` trust band (`hostile`, `wary`, `neutral`, `warm`). |
| `aggression` | `float` | `0.0f` | `TradeScreenScenario.Aggression` | Table-edge raid aggression meter (0.0 to 1.0). |
| `consecutive_repels` | `int` | `0` | `TradeScreenScenario.ConsecutiveRepels` | Faction presence meter on HUD. |
| `has_surrendered` | `bool` | `false` | `TradeScreenScenario.HasSurrendered` | Gating flag for surrender terms. |
| `can_demand_parley` | `bool` | `false` | `TradeScreenScenario.CanDemandParley` | Controls whether parley action is available. |
| `world_phase` | `string` | `""` | `TradeScreenScenario.WorldPhase` | Displayed in "news from outside" strip. |
| `world_day` | `int` | `1` | `TradeScreenScenario.WorldDay` | Campaign day context for news and radio intercept timing. |
| `price_shocks` | `array` | `[]` | `TradeScreenScenario.PriceShocks` | List of `ShockBadgeData` (kind, multiplier, note) rendered as badges in market strip. |
| `scarcity` | `array` | `[]` | `TradeScreenScenario.Scarcity` | List of `ScarcityBandData` (item_id, display_name, multiplier) rendered as scarcity multipliers. |
| `player_offers` | `array` | `[]` | `TradeScreenScenario.PlayerOffers` | List of `TradeLineData` (item_id, display_name, quantity, unit_price). Contributes to `PlayerOfferValue`. |
| `biological_offers` | `object` | `{}` | `TradeScreenScenario.BiologicalOffers` | Map of `BiologicalTradeItem` -> int quantity. Value computed via `TradePricing.BioUnitValue`. |
| `faction_demands` | `array` | `[]` | `TradeScreenScenario.FactionDemands` | List of `TradeLineData` (item_id, display_name, quantity, unit_price). Contributes to `FactionAskValue`. |
| `expected_fairness` | `string` | `EmptyTable` | `TradeScreenScenario.ExpectedFairness` | Parsed to `TradeFairness` (`fair` -> `Fair`, `short` -> `Short`, other -> `EmptyTable`). |
| `confirm_succeeds` | `bool` | `false` | `TradeScreenScenario.ConfirmSucceeds` | Determines outcome of `MockTradeIntentSink.TryConfirmTrade()`. |
| `radio_ticker` | `string` | `""` | `TradeScreenScenario.RadioTicker` | Diegetic atmospheric broadcast shown on the room's radio ticker. |

---

## 3. Sub-Object Schemas

### Price Shock Object
```json
{
  "kind": "PlumePassing" | "ConvoyAmbush" | "FactionWar" | "WinterDeepens",
  "multiplier": 1.5,
  "note": "brief atmospheric context"
}
```

### Scarcity Object
```json
{
  "item_id": "clean_water",
  "display_name": "Clean Water",
  "multiplier": 2.0
}
```

### Trade Line Object (Offers & Demands)
```json
{
  "item_id": "canned_food",
  "display_name": "Canned Food",
  "quantity": 3,
  "unit_price": 18.0
}
```

### Biological Offers Object
```json
{
  "PintOfBlood": 1,
  "BoneMarrow": 0,
  "Plasma": 0,
  "Organ": 0
}
```
Unit values: `PintOfBlood` = 25, `BoneMarrow` = 50, `Plasma` = 75, `Organ` = 100.
