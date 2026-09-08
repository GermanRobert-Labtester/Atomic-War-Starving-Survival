# Plan 99 — Hardcore Economy Tuning Expansion Closeout

**Status:** COMPLETE
**Authority:** `Assets/StreamingAssets/Data/hardcore_economy_tuning.json`

## Delivered

| Surface | Final count | Runtime proof |
|---|---:|---|
| Scarcity tiers | 8 | Core loader, day-range tests, prefix wildcard tests |
| Faction preferences | 8 | Unique-ID and collision tests |
| Price shocks | 6 | Enum parsing and duration-boundary tests |

The first two tiers and the `PlumePassing` rule retain their baseline values.
The sixth new tier is `Reconstruction`, covering Days 161–220.

## Authority and compatibility

- JSON remains the only tuning authority.
- `HardcoreEconomyTuning` remains an in-memory overlay.
- Existing `MarketSystem` demand and base-price calculations remain unchanged.
- Old saves need no migration because tuning definitions are not persisted.
- `central_garrison_remnants` is preserved as the legacy tuning key and resolves
  alongside `faction_central_garrison` at lookup time.
- Legacy `FactionWar`, `WinterDeepens`, and `ScarcityTier.Low` enum names remain
  as source-compatible aliases; the catalog uses the canonical Plan 99 names.

## Verification

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter FullyQualifiedName~HardcoreEconomyTuningExpansionTests`: **PASS, 13/13**
- Focused economy regressions: **PASS, 26/26**
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`: **PASS**
- `dotnet build Ashfall.csproj`: **PASS, 0 warnings, 0 errors**
- `godot --headless --path . -- --economy-selftest`: **PASS, 11/11**
- `godot --headless --path . -- --data-integrity-selftest`: **PASS, 0 errors across 298 catalogs**
- `godot --headless --path . -- --content-utilization-selftest`: **PASS, CI gate**

## Plan divergence

The original plan described a data-only change. The repository's live tests and
contracts required minimal runtime reconciliation for enum parsing, wildcard
matching, faction alias resolution, and host loading. No new economy authority
or save state was introduced.
