# Plan 61 Baseline — Trade Screen Scenarios (recorded 2026-02, pre-expansion)

> **Provenance (Plan 61).** This document supersedes the 2026-09-03 v1.0.0 planning
> version of the same deliverable (authored before the catalog expansion landed).
> The planning roster was never landed in `trade_screen_scenarios.json` (baseline
> remained 3 scenarios) and its table design referenced item IDs absent from
> `items.json` with per-item prices violating the global unit-price consistency
> gate. This version is the implementation record for the landed 15-scenario
> catalog; unique correct authority facts from the planning version are merged.

Repository truth recorded before any scenario authoring. Every plan assumption was
re-checked against source; where the plan's provisional schema disagreed with the
runtime, **repository reality won**.

## 1. Catalog baseline

- File: `Assets/StreamingAssets/Data/trade_screen_scenarios.json`
- Scenario count: **3** — `fair_deal`, `offer_short`, `empty_table`
- Root shape: `{ schema_version: 1, $schema, version: 1, description, scenarios: [...] }`
- Baseline gates (all green before expansion):
  - `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — 0 errors
  - `dotnet test ... --filter TradeScreen` — 30/30 pass
  - `godot --headless --path . -- --data-integrity-selftest` — PASS, 298 catalogs, 0 findings
  - `dotnet build Ashfall.csproj` — 0 errors, 0 warnings
  - `godot --headless --path . -- --bridge-selftest` — PASS (shim-removal verb)

## 2. Actual scenario schema (runtime contract)

Source: `Assets/Ashfall.Core/Economy/TradeScreenScenarios.cs` (`TradeScreenScenarioLoader.LoadFromJson`).

A scenario is a **data-defined faction snapshot at the Negotiation Table**, not a
merchant-config object. Fields (all consumed by the loader / `CreateBinding`):

| JSON field | Type | Consumer behavior |
|---|---|---|
| `id` | string | scenario identity; lookups by ID |
| `faction_id` / `faction_name` | string | VM faction identity |
| `leader_name` | string | VM leader line |
| `succession_generation` | int (default 1) | VM generation |
| `stance` | enum key | `Trade`/`ShareIntel` → table opens (`willTrade`); `Refuse` default; `HostileRaid`/`Rob` accepted |
| `trust` | float | drives tell trust-band selection (`hostile ≤ -40`, `wary -39..0`, `neutral 1..40`, `warm 41..100`) |
| `aggression` | float | VM meter |
| `consecutive_repels`, `has_surrendered`, `can_demand_parley` | int/bool | VM presence |
| `world_phase` / `world_day` | string/int | news-strip world context |
| `price_shocks[]` | `{kind, multiplier, note}` | news-strip badges only; **no price math** |
| `scarcity[]` | `{item_id, display_name, multiplier}` | news-strip bands only; **no price math** |
| `player_offers[]` | `{item_id, display_name, quantity, unit_price}` | player edge of the table |
| `faction_demands[]` | `{item_id, display_name, quantity, unit_price}` | faction edge of the table |
| `biological_offers{}` | `PintOfBlood/BoneMarrow/Plasma/Organ → count` | "the drawer"; priced by `TradePricing.BioUnitValue = ((int)+1)*25` |
| `expected_fairness` | `fair`/`short`/empty default | data-defined expectation; VM computes from table values |
| `confirm_succeeds` | bool | mock intent sink `ConfirmResult` |
| `radio_ticker` | string | room-radio line |

### Answers to the plan's baseline exit gate

- **Exact fields of one scenario:** table above. Required in practice: `id`, `faction_id`; everything else has a loader default.
- **Scenario-ID prefix:** no prefix is enforced. Existing IDs are plain snake_case (`fair_deal`). New IDs follow the same convention; `trade_*`/`scenario_*` prefixes were **not** introduced.
- **`available_goods`:** **does not exist.** There is no stock list, stock generator, or stock authority. Scenario tables are fixed authored barter lines.
- **`price_modifier`:** **does not exist.** There is no scenario-level multiplier composed into prices. Unit prices are authored **per line**. `price_shocks`/`scarcity` multipliers are presentation badges in the mock binding (they never alter `unit_price`). The plan's "desperate = 1.5 / bulk = 0.8" examples are therefore **not implementable as written**; price posture is expressed by authored unit prices per table.
- **Other price layers:** none in this seam. Base "value" is the authored `unit_price`; bio value is the `TradePricing` core rule. No faction discount, scarcity math, or negotiation math is composed into scenario prices.
- **Negotiation options:** **not a scenario field.** Tell lines are auto-selected by `TradeTellEngine` from `stance × trust-band` pools. Scenarios influence negotiation only by choosing stance/trust.
- **Special conditions:** **none supported.** There is no eligibility predicate model — a scenario is a static authored encounter. No min-day, reputation, flag, item, debt, settlement, or patrol predicates exist in this seam.
- **Compound conditions:** not supported; none authored.
- **Missing reference behavior:** loader silently defaults (unknown stance → `Refuse`, unknown fairness → `empty`, unknown shock kind → `PlumePassing`). Data-contract tests must carry the validation load (the `CatalogIntegrityValidator` does not scan this file).
- **Multiple eligible scenarios / selection:** not applicable — there is no eligibility or selection engine. A consumer requests a scenario by ID. Determinism requirement reduces to: stable IDs + seed-deterministic tell selection (`ISeededRng`).
- **Active scenario persistence:** none. Scenarios are static content; no trade-session state persists a scenario ID. Old saves are unaffected by additive catalog growth.
- **Reroll on reopen:** impossible — no stock generation, no selection RNG over scenarios.
- **Plans 40/43/45/56/62 status at implementation time:**
  - Plan 62 **tell lines: LIVE** (`trade_tell_lines.json` + `TradeTellEngine`; pools exist for all 5 stances × 4 bands). Integration = stance/trust choices; scenarios never reference tell IDs directly.
  - Plan 40 debt / 43 settlements / 45 patrols: **no producer or consumer exists in the trade-screen seam** → documented as deferred; no fabricated references.
  - Plan 56 economy goods: no `economy_goods.json`-driven stock model in this seam → scenario goods reference `items.json` directly (all verified).

## 3. Existing scenario inventory (preserved verbatim)

| Field | fair_deal | offer_short | empty_table |
|---|---|---|---|
| faction_id | scavenger_camp | upland_militia | rot_farmers |
| leader | Varek | Sergeant Oduya | Mother Ilde |
| succession | 1 | 3 | 2 |
| stance | Trade | Trade | Refuse |
| trust | 22 (neutral) | -5 (wary) | -25 (wary) |
| aggression | 0.35 | 0.6 | 0.2 |
| repels / parley | 0 / false | 2 / false | 0 / false |
| world | CivilWar, day 14 | CivilWar, day 27 | LongWinter, day 41 |
| shocks | PlumePassing ×2.5 | ConvoyAmbush ×1.8 | — |
| scarcity | clean_water ×2.0 | fuel ×1.8 | — |
| player offers | 3× canned_food @18, 1× duct_tape @15 | 1× canned_food @18 | — |
| bio offers | PintOfBlood ×1 | — | — |
| demands | 2× clean_water @22 | 2× fuel @40 | — |
| fairness | fair (94 ≥ 44) | short (18 < 80) | empty |
| confirm_succeeds | true | false | false |

Characterization: `Ashfall.Core.Tests/TradeScreenSeamTests.cs` (30 tests) pins load,
computed-vs-expected fairness, value math (94/44), tell presence, and intent routing.

## 4. Faction ID authority

`faction_radio_corpus.json` defines the canonical set: `cult_of_the_glow`,
`custodians`, `doomsday_preppers`, `echo_bats`, `faction_black_flotilla`,
`faction_silent_foundry`, `hydro_barons`, `military_remnants`, `rot_farmers`,
`safe_haven_community`, `scavenger_camp`, `sump_dredgers`, `upland_militia`,
`wire_heads`. The three existing scenarios use three of these; all new scenarios
use **only** IDs from this set.

## 5. Item ID authority

`items.json` (621 entries) is the item authority. Every `item_id` used in scenario
tables must resolve there; verified mechanically by the new data-contract tests.

## 6. Known constraints for expansion

1. No new Core code is required or permitted (plan §6.1) — the 12 new scenarios must be expressible in the existing schema.
2. The contextual-prose carrier is `radio_ticker` (consumed); adding a per-scenario `description` field would be dead JSON and was rejected.
3. `world_phase` vocabulary in data is `CivilWar` / `LongWinter`; reused only.
4. Price posture must be authored per line with **global unit-price consistency** for the same `item_id` (existing data convention: `canned_food` is 18 in every table).
5. `CatalogIntegrityValidator` does not cover this file; new tests provide uniqueness, reference, fairness, consistency, and determinism gates.
