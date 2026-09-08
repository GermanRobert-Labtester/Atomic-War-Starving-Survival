# Plan 142 Regression Matrix

| Area | Required proof | Status |
|---|---|---|
| schema loader | both ambient and canonical wrappers normalize to one authored shape | PASS, 189-record loader test |
| required fields | missing ID/body/key and invalid day/hour are rejected | PASS, invalid timestamp fixture |
| duplicate guard | duplicate source ID/key fails deterministically | PASS, catalog unit test |
| author safety | exact survivor IDs resolve; unknown names remain display-only | PASS, 28/28 + 0/40 ambient match tests |
| timestamp | explicit timestamps validate; day-only entries remain day-only | PASS, source census + adapter tests |
| producer activation | authored body replaces only a matching producer submission | PASS, map-key activation seam |
| generated compatibility | unmatched producer body and sequence ID remain unchanged | PASS, existing JournalSystem suite |
| no boot flood | setup/catalog load creates no authored entries | PASS, adapter bind test |
| idempotency | retry produces no second entry, event, ping, or codex unlock | PASS, save/restore retry test |
| save round trip | authored entry and existing UI state survive capture/restore | PASS, JournalSystem save path + corpus test |
| restore suppression | restore emits no insertion or notification events | PASS, corpus test |
| UI/rebind | existing canonical body/author/timestamp rendering remains valid | PASS, `--journal-uitest` |
| faction-war boundary | faction-war journal is not generically ingested | PASS, loader source list excludes it |
| content utilization | classifications change only after runtime consumption evidence | PASS, content-utilization gate |
| full regression | Core tests, host build, Godot gates, data integrity | BLOCKED by unrelated baseline content: Core 10045/10052 passed, 7 failed; data-integrity reports 79 unresolved IDs in `narrative_discovery_manifest.json` |

## Final gate record

PASS:

- Plan 142 focused corpus/system tests: 34/34.
- Journal/utilization regression subset: 166/166.
- `dotnet build Ashfall.csproj --no-restore`: 0 warnings, 0 errors.
- `--journal-selftest`: 23/23.
- `--journal-uitest`.
- `--journal-save-selftest`.
- `--content-utilization-selftest`.
- `--catalog-boot-preflight`: 299/299 catalogs.
- `--save-load-ui-failure-selftest`: 8/8 gates.
- `--bridge-selftest`.

BLOCKED outside Plan 142:

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --no-restore`:
  10,045 passed, 7 failed, 0 skipped, 10,052 total.
- `--data-integrity-selftest`: 79 errors across 299 catalogs.

All seven Core failures and all 79 integrity errors identify unresolved
`source_record_id` or `channel` values in
`Assets/StreamingAssets/Data/narrative_discovery_manifest.json`. No Plan 142
source or implementation file is implicated. The manifest remains untouched.
