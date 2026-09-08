# Trade Scenario Schema Map — actual runtime contract (Plan 61)

> **Provenance (Plan 61).** This document supersedes the 2026-09-03 v1.0.0 planning
> version of the same deliverable (authored before the catalog expansion landed).
> The planning roster was never landed in `trade_screen_scenarios.json` (baseline
> remained 3 scenarios) and its table design referenced item IDs absent from
> `items.json` with per-item prices violating the global unit-price consistency
> gate. This version is the implementation record for the landed 15-scenario
> catalog; unique correct authority facts from the planning version are merged.

Authoritative source: `Assets/Ashfall.Core/Economy/TradeScreenScenarios.cs`.
This map documents the schema as implemented; it intentionally omits any field the
loader does not read (no dead JSON is shipped).

## Catalog root

```json
{
  "schema_version": 1,
  "$schema": "./schema/trade_screen_scenarios.schema.json",
  "version": 1,
  "description": "...",
  "scenarios": [ { ...scenario... } ]
}
```

Only `scenarios` is read by `TradeScreenScenarioLoader.LoadFromJson`. The other keys
are established catalog metadata convention and are preserved unchanged.

## Scenario record

| Field | Kind | Default if missing | Consumed by |
|---|---|---|---|
| `id` | string | `""` | identity / test lookups |
| `faction_id` | string | `""` | `SetFaction` |
| `faction_name` | string | `""` | `SetFaction` (falls back to faction_id in VM) |
| `leader_name` | string | `""` | `SetFaction` |
| `succession_generation` | int | `1` | `SetFaction` |
| `stance` | enum key: `hostile_raid` / `rob` / `trade` / `share_intel` / `refuse` (PascalCase also accepted) | `Refuse` | `SetStance`; `Trade`/`ShareIntel` ⇒ `willTrade` ⇒ table can confirm |
| `trust` | float | `0` | `SetMeters`; selects tell band via `TradeTellEngine` |
| `aggression` | float | `0` | `SetMeters` |
| `consecutive_repels` | int | `0` | `SetFactionPresence` |
| `has_surrendered` | bool | `false` | `SetFactionPresence` |
| `can_demand_parley` | bool | `false` | `SetFactionPresence` |
| `world_phase` | string | `""` | `SetWorld` |
| `world_day` | int | `1` | `SetWorld` (clamped ≥ 1) |
| `price_shocks[]` | `{kind, multiplier, note}`; kind ∈ `PlumePassing` (default) / `ConvoyAmbush` / `FactionWar` / `WinterDeepens` | — | `SetShockBadges` (news strip) |
| `scarcity[]` | `{item_id, display_name, multiplier}` | — | `SetScarcityBands` (news strip) |
| `player_offers[]` | `{item_id, display_name, quantity, unit_price}` | — | player edge of table |
| `faction_demands[]` | `{item_id, display_name, quantity, unit_price}` | — | faction edge of table |
| `biological_offers{}` | keys `PintOfBlood` / `BoneMarrow` / `Plasma` / `Organ` → int count | — | "the drawer"; value priced by `TradePricing.BioUnitValue` |
| `expected_fairness` | `fair` / `short` / anything-else ⇒ `empty` | `EmptyTable` | data-defined expectation (tests pin computed == expected) |
| `confirm_succeeds` | bool | `false` | `MockTradeIntentSink.ConfirmResult` |
| `radio_ticker` | string | `""` | `SetRadioTicker` — the scenario's contextual prose carrier |

## Derived semantics (not authored)

- **Fairness (computed):** table empty on both edges ⇒ `EmptyTable`; otherwise
  `PlayerOfferValue >= FactionAskValue` ⇒ `Fair`, else `Short`.
  `PlayerOfferValue = Σ(quantity × unit_price) + Σ BioUnitValue(kind) × count`.
- **CanConfirm (computed):** `Fairness == Fair && stance ∈ {Trade, ShareIntel}`.
- **Tell line:** auto-selected by `TradeTellEngine` from `stance × trust band`
  (`hostile ≤ -40`, `wary -39..0`, `neutral 1..40`, `warm 41..100`), seed-deterministic
  through `ISeededRng`. Scenarios never store tell IDs.

## Rejected field proposals (plan §5.2 discipline)

`trader_type`, `available_goods`, `price_modifier`, `negotiation_options`,
`special_condition`, per-scenario `description` — **none exist in the loader**;
none were authored. See `PLAN61_BASELINE.md` §2 for how each concern is expressed
in repository-native semantics instead.
