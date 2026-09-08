# Foundry Treaty Consequence Bindings

**Treaty authority:** `Assets/StreamingAssets/Data/foundry_accords.json`

**Policy authority:** `Assets/StreamingAssets/Data/foundry_treaty_consequences.json`

## Policy seam

The live catalog is a static lookup from `(treaty_id, outcome)` to a policy.
The policy carries the affected signatory, a bounded standing delta, and
market demand modifiers. `SilentFoundrySystem` owns application and records
the result in `SilentFoundryConsequenceState`.

| Treaty | Live outcomes | Affected faction | Runtime trigger |
|---|---|---|---|
| `treaty_brine_pipe_and_iodine_exchange` | `met`, `missed` | `faction_silent_foundry` | live quota cycle |
| `treaty_cluster_labour_schedule` | `met`, `violated` | `faction_silent_foundry` | live labour cycle |
| `treaty_road_iron_charter` | `met`, `missed` | `faction_silent_foundry` | live quota cycle |
| `treaty_saltworks_access` | `met`, `violated` | `faction_silent_foundry` | data-ready; trigger deferred |
| `treaty_membrane_repair` | `met`, `violated` | `faction_silent_foundry` | data-ready; trigger deferred |
| `treaty_coal_window` | `met`, `missed` | `faction_silent_foundry` | data-ready; trigger deferred |
| `treaty_crisis_mutual_aid` | `met`, `violated` | `faction_silent_foundry` | data-ready; trigger deferred |
| `treaty_the_incident_book` | `met` | `faction_silent_foundry` | data-ready; reporting trigger deferred |

`treaty_apprentice_exchange` and `treaty_the_cluster_charter` intentionally
have no policy rows. The current Core has no typed trainee-cycle or charter
outcome trigger, and this plan does not create one.

## Consequence authority separation

- Treaty definitions remain in `foundry_accords.json`.
- Policy lookup and one-shot application remain in Core.
- Standing remains mirrored through the existing Foundry stance host.
- Market demand remains owned by `MarketSystem`.
- Access, production, contamination, war, dialogue, and epilogue systems are
  downstream consumers only; no unsupported policy fields or fake flags were
  added.

The nine new policies are therefore stable, validated data seams. A future
typed assessment trigger can call the existing `Find`/`ApplyConsequence`
path without changing the catalog contract.
