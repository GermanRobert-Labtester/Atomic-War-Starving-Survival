# Year of Ash Regression Matrix

## Data facts

- Questlines before: 8
- New questlines: 7
- Questlines after: 15
- Total stages: 89
- Unique stage IDs: 89
- Total choices: 134
- Unique choice IDs: 134
- Invalid new faction/item/encounter references: 0 in the targeted Plan 114 suite

## Verified targeted coverage

The targeted command below passed 9 tests, including count/parity, graph integrity, reference
integrity, availability boundaries, and live one-shot choice behavior:

```text
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter 'FullyQualifiedName~YearOfAshPlan114Expansion|FullyQualifiedName~YearOfAshQuestJsonParity' --no-restore
Passed: 9, Failed: 0, Skipped: 0
```

## Runtime limits recorded by design

- `unlockOnDay` is currently metadata; `TakeChoice` does not enforce it.
- `conditions` are loaded but not evaluated in `TakeChoice`.
- the host applies faction standing and inventory rewards, but does not consume
  `unlockedEncounterId` into expedition state.
- no verified direct treaty, epilogue, or echo bridge exists in this data path.

The final integrity, full test, build, content-utilization, and fast-gate outputs must be appended
here after the repository-wide verification sweep. Any unrelated pre-existing failure remains
separately identified rather than repaired as part of Plan 114.

## Final repository sweep

```text
godot --headless --path . -- --data-integrity-selftest
PASS — 298 catalogs, 0 errors, 0 warnings

dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --no-restore
PASS — 9,925 passed, 0 failed, 0 skipped

dotnet build Ashfall.csproj --no-restore
PASS — 0 warnings, 0 errors

godot --headless --path . -- --content-utilization-selftest
PASS — CI content-utilization gate; 0 orphaned catalogs, 0 invalid exemptions

godot --headless --path . -- --real-campaign-journey-selftest
PASS — host selftest exit code 0

godot --headless --path . -- --year-of-ash-save-selftest
PASS — save write/reload/restore/checksum/tamper gate
```

The canonical fast gate stopped at its first unrelated hygiene failure. It reported trailing
whitespace in `artifacts/asset_registry.md` and an existing blank line at EOF in
`docs/narrative/PLAN_89_MUSTER_EPILOGUES_EXPANSION_CLOSEOUT.md`; no Plan 114 file was named. The
fast gate result is therefore `FAIL (pre-existing working-tree hygiene)`, not a Plan 114 data or
build failure. The content-utilization run also emitted pre-existing duplicate-item diagnostics in
several catalogs, while still passing its gate.
