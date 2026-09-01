# Black Flotilla Standing & Trade Matrix (Plan 23 / Task 23A)

## Standing (existing authority: `FactionStanceEngine` — one trust track)

Registration: `BlackFlotillaStanding.Register(engine)` — canonical thresholds
(`FactionThresholds`): raid −50, rob −20, minTrustToTrade 0, intelShare 40,
aggression 0.35, no trust inversion. Trust clamps to [−100, +100].

| Trust | Tier (`BlackFlotillaTier`) | Player outcome |
|---|---|---|
| < 0 | Hostile | Flagged traffic; boarding; claims suspended (radio raid_warning pool) |
| 0–29 | Tolerated | Hail answered; standard rates; bulletins; no privileges |
| 30–54 | Trading | Exchange access, claim-tag courtesy, specialist stock |
| ≥ 40 | (intel share) | Charts/coordinates/tide-table intel may be shared |
| ≥ 30 | SalvageTrusted | Claim cooperation; wreck-rights quests |
| ≥ 55 | DeepCooperation | Deep-dive cooperation, launch rights, kin berths |

Standing consumers (existing surfaces only): trade pricing (`FactionTradePreference`),
radio tone/pools, quest/NPC availability, deep-site gear/coords (23B/23D), NPC
disposition. Not every Flotilla feature is standing-gated — the drowned metro, ferry
terminal, and Verrill's wreck notes are reachable through risk, trade, or exploration.

## Trade specialty (existing authority: `hardcore_economy_tuning.json` faction_preferences)

| Field | Value |
|---|---|
| faction_id | `faction_black_flotilla` |
| buys_at_premium | `item_marine_sealant_kit`, `item_descent_line`, `item_sealed_dive_lamp`, `item_rebreather_canister`, `brass_fittings`, `scrap_mechanical`, `item_process_barrel`, `item_ro_resin`, `chart_*`, `paper_scrap` |
| refuses | jewelry, book, family_photograph, item_teddy_bear |
| trade_currency | "Dry cloth, medicine, fuel, and salvage with paper on it" |

Strengths (buys at premium): marine salvage, corrosion-resistant components, dive gear,
charts/paper. Needs (sold/asked): medicine, dry food, fuel, clean textiles — matching the
Flotilla `wants` in `holdfast_factions.json`.

Arbitrage guard: premium list contains only salvage/technical goods the player must
*recover* (wreck dives, deep-coast route) or craft; refuses pure-luxury items; no
Flotilla-owned seller table exists to buy back at inflated marks. Full loop audited in
Task 23E (`DIVE_RISK_REWARD_MATRIX`, economy loop audit).

## Tests pinning this matrix

- `Plan23FlotillaFactionDepthTests` (14 tests): roster grammar, standing
  registration/thresholds/tiers, 12 items in merged catalog, no duplicate ids,
  trade preference load, six NPC roles, radio band/pools/determinism, frequency
  separation, old-save behavior.
