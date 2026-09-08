# Plan 139 — Trade Voice Activation Closeout

Status: **implemented**

## Goal

Activate `Assets/StreamingAssets/Data/trade_texts.json` as a deterministic,
presentation-only voice layer for the live trade and traveling-caravan surfaces.
The layer must not own or modify prices, stock, standing, debt, scenario
eligibility, tell selection, save state, or transaction results.

## Baseline and implementation

Before this pass, `trade_texts.json` was authored content without a production
loader or consumer. The live trade screen was composed from `Main.OpenTradeScreen`
and the caravan screen was composed from `Main.ShelterBatch3`; neither projected
the catalog.

The implementation adds:

- `TradeTextCatalogLoader` with optional-file fallback and schema validation.
- `TradeVoiceResolver` with stable precedence:
  explicit profile, scenario, caravan ID, faction, caravan origin, specialty,
  then generic merchant.
- Nine authored producer-backed profiles:
  scavenger, merchant, faction trader, wanderer, quartermaster, bulk dealer,
  medical supplier, Foundry broker, and flotilla salvager.
- A separate trader voice projection on `TradeScreenViewModel`.
- `TradeScreenPresenter` voice projection that leaves Plan 62 stance tells intact.
- Main-composed loading once per campaign composition.
- `TradeScreenGodotPanel` and `TravelingCaravanPanel` consumers.
- Runtime content-utilization evidence for catalog load, lookup, selection, and
  display.

## Schema and placeholder contract

| Field | Contract |
|---|---|
| `schema_version` | `1` |
| `collection_id` | `trade_texts` |
| `traders` | Unique profiles with all ten required line families |
| `trade_scenarios` | Presentation-only trader/player text keyed by scenario ID |
| Placeholder | `[item]` only |
| Placeholder limits | Offers: at most two; negotiated/desperate lines: at most one; other families: none |
| Failure behavior | Missing or invalid optional data uses the generic fallback profile; trade remains available |

Display names are sanitized before substitution. Unknown bracket or brace
tokens reject the authored catalog, and unresolved `[item]` tokens never reach
the view.

## Producer map and archetype matrix

| Live producer context | Stable input | Resolved voice |
|---|---|---|
| Silent Foundry trade screen | `faction_silent_foundry` | Foundry broker |
| Medical caravan | `caravan_medic_syndicate` | Medical supplier |
| Foundry caravan | `caravan_foundry_coal_iron` or `industrial_belt` | Foundry broker |
| Flotilla caravan | `caravan_flotilla_salt_run` or `deep_coast` | Flotilla salvager |
| Bulk caravan | `caravan_verge_grain_convoy` or `ash_flats` | Bulk dealer |
| Other stable caravan/faction | canonical ID, faction, or region | Scavenger, merchant, or wanderer fallback |

Debt collector, smuggler, and refugee profiles were not invented: no current
live producer contract supplies those contexts. Scenario text remains available
through `ResolveScenarioTraderText` for a future producer without changing the
scenario authority.

## Tell and voice boundary

Plan 62 remains the authority for stance × trust tell lines. The new layer
renders a separate trader voice line and never replaces or mutates
`StanceTellLine`, `TradeStance`, trust, aggression, or radio output.

## Caravan integration

`TravelingCaravanPanel` resolves voice from the persisted caravan ID, faction,
and origin region. Rebinding the same stable caravan produces the same profile
and line. The panel only adds labels; `TravelingCaravanSystem` remains the
authority for route movement, inventory, rations, completed trades, and saves.

## Determinism and non-mutation

- Profile resolution is keyed lookup with explicit precedence, not gameplay RNG.
- Line selection is keyed by family and trust band; it does not consume the
  economy or campaign RNG stream.
- The presenter tests compare the voice-enabled and voice-disabled read models.
- Execution still crosses `ITradeExecutionSink`; voice data is never passed to
  or read by transaction execution.
- Missing, malformed, duplicate, or unsafe content falls back without blocking
  a trade surface.

## Regression matrix

| Gate | Result |
|---|---|
| `dotnet test Ashfall.Core.Tests --filter FullyQualifiedName~TradeTextCatalogTests` | PASS, 15/15 |
| `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --no-restore` | PASS, 0 errors; existing xUnit analyzer warnings only |
| `dotnet build Ashfall.csproj --no-restore` | PASS, 0 errors, 0 warnings |
| `godot --headless --path . -- --content-utilization-selftest` | PASS |
| `godot --headless --path . -- --caravan-selftest` | PASS, 16/16 |
| `godot --headless --path . -- --economy-selftest` | PASS, 11/11 |
| `godot --headless --path . -- --data-integrity-selftest` | PASS, 0 errors across 300 catalogs |
| `godot --headless --path . -- --bridge-selftest` | PASS |

The full Core suite and final host/UI ladder remain the final release checks
after this implementation pass.
