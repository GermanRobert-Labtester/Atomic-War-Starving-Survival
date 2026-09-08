# Foundry Treaty Consequence Regression Matrix

## Catalog checks

| Check | Expected |
|---|---|
| Policy count | exactly 15 |
| Added rows | exactly 9 |
| Duplicate `(treaty_id, outcome)` keys | 0 |
| Invalid treaty IDs | 0 |
| Invalid faction IDs | 0 |
| Invalid outcomes | 0 |
| Unsupported good IDs | 0 |
| Empty reasons | 0 |

## Coverage checks

| Treaty group | Expected |
|---|---:|
| Baseline policies preserved | 6 |
| Saltworks / Coal / Membrane / Crisis / Incident rows | 9 |
| Treaties with two policies | 7 |
| Treaties with one policy | 1 |
| Treaties intentionally without policies | 2 |

## Runtime contracts

- Baseline idempotency remains keyed by treaty and assessment day.
- Save/reload retains applied records and does not reapply effects.
- Standing remains clamped by the existing Foundry ledger.
- Market modifiers route through `MarketSystem`; no inventory mutator is
  authored in JSON.
- No access, production, contamination, war, dialogue, or epilogue runtime
  was added.

## Verification commands

```text
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --silent-foundry-selftest
godot --headless --path . -- --content-utilization-selftest
```

The repository-wide fast tier is reported separately because it is sensitive
to unrelated workspace whitespace findings.
