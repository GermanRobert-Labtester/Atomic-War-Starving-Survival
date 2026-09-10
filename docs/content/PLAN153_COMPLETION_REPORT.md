# Plan 153 completion report — fringe cults runtime activation

**Date:** 2026-09-09
**Scope:** `FringeCultsCatalog` and its four authored narrative catalogs
**Status:** Plan 153 implementation complete; repository-wide verification remains blocked by five unrelated concurrent-plan contract failures and one fast-gate hygiene finding.

## Delivered behavior

The 30 authored records are now reachable through the existing Plan 135 narrative discovery seam:

| Family | Records | Discovery projections | Runtime treatment |
|---|---:|---:|---|
| Cobalt liturgies | 8 | 8 | authored doctrine / ritual record |
| Iron Synod canons | 8 | 8 | authored institutional rule |
| Geophone hymnals | 7 | 7 | authored hymnal / historical observation |
| Wasteland epitaphs | 7 | 7 | memorial testimony |
| **Total** | **30** | **30** | **codex knowledge only** |

`FringeCultsCatalog` remains the typed source loader. `FringeCultSourceAdapter` projects safe, immutable presentation records into `NarrativeDiscoveryCatalog`; `JournalSystem` owns one-time discovery and `JournalCodex` owns the reader projection. The source transcript is never parsed as a command or state update.

All 30 records have explicit producer contexts across 20 producer IDs, including settlement and ruin inspections, foundry and radio rooms, the shelter archive, the memorial wall and existing map locations. The host does not globally unlock records from `posted_day`; the day value is retained as authored chronology and only the manifest’s minimum-day eligibility is checked when an explicit producer runs.

## Authority and identity decisions

- No record creates or aliases a `faction_*` ID. Cobalt, Iron Synod chapters and Bedrock Ear remain authored sect, institutional or monastery labels because no exact canonical faction or subfaction mapping was proven.
- Epitaph names remain display identities. No exact canonical survivor/NPC match was found, and no epitaph calls `MemorialSystem` or changes mortality state.
- `sacred_rad_threshold_cpm`, `sacred_temperature_celsius` and `resonant_frequency_hz` are rendered with provenance labels identifying doctrine, canon and hymnal notation. They do not write to radiation, medical, foundry, acoustic, morale or faction systems.
- Epitaph causes of death are rendered as “Cause of death recorded on marker”; they are not treated as true current mortality data.
- No generated live template was introduced. No existing typed consumer justified parameterizing a current report, so the static catalog remains archival/authored content.
- No curated hymnal-to-Oral-Lore performance mapping was found. The catalogs remain separate until an explicit mapping exists.

## Graph and persistence

The explicit related-record graph preserves the authored family order: Cobalt records 1–8, Iron Synod records 1–8, and Geophone hymnal records 1–7. Epitaphs are independent terminal records. Related links appear only for records already discovered, and missing targets degrade to no link.

The implementation reuses the existing journal knowledge ledger. It persists stable `knowledge_narrative_discovered_*` keys through the existing journal save section; it adds no doctrine, faction, mortality or downstream simulation fields. Discovery is idempotent, panel rebinds are presentation-only, and old saves do not receive retroactive discoveries or effects.

## Content utilization

The content-utilization scanner now classifies the four Fringe Cult source files through `FringeCultSourceAdapter`, `FringeCultsCatalog`, `NarrativeDiscoveryCatalog`, `JournalSystem` and `JournalCodex`. The runtime collector loads all four typed catalogs and verifies all 30 projections.

The current content-utilization selftest reports 583 catalogs, 228 gameplay-consumed catalogs, 268 codex-only catalogs, 0 orphaned catalogs, 71 unresolved references and 1,216 runtime events. The CI content-utilization gate passes.

## Files and documentation

The Plan 153 audit and handoff are recorded in:

- `PLAN153_BASELINE.md`
- `FRINGE_CULTS_RUNTIME_MATRIX.md`
- `PLAN153_GROUP_IDENTITY_MATRIX.md`
- `PLAN153_DISCOVERY_PRODUCER_MATRIX.md`
- `PLAN153_SAVE_COMPATIBILITY.md`
- `PLAN153_REGRESSION_MATRIX.md`
- `PLAN153_NARRATIVE_ACCURACY_AUDIT.md`
- this completion report

Runtime changes are limited to the typed fringe-cult adapter/catalog boundary, the existing narrative manifest and discovery seam, Journal/Codex presentation metadata, explicit Godot producer calls, catalog validation and content-utilization coverage. The four authored source JSON files were not rewritten.

## Verification

Passing Plan 153 and supporting checks:

- `dotnet build Ashfall.Core/Ashfall.Core.csproj --no-restore --verbosity:minimal` — pass, 0 warnings/errors.
- `dotnet build Ashfall.csproj --no-restore --verbosity:minimal` — pass, 0 warnings/errors.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter FullyQualifiedName~FringeCultRuntimeActivationTests` — **7/7 pass**.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter FullyQualifiedName~NarrativeDiscoverySystemTests` — **7/7 pass**.
- `godot --headless --path . -- --data-integrity-selftest` — pass, 0 findings across 299 catalogs.
- `godot --headless --path . -- --journal-selftest` — **25/25 pass**.
- `godot --headless --path . -- --content-utilization-selftest` — pass; CI content-utilization gate pass.

The full xUnit run completed with **10,429 passed and 5 failed of 10,434**. The failures are outside Plan 153: a concurrent Plan 154 report contains machine-specific `file:///home/...` links; the save-section contract is 169 while two tests still expect 168/162; and the architecture test map is missing the concurrent `oral_lore` section. The fast gate also stops on the pre-existing blank EOF line at `Assets/Ashfall.Core/Narrative/BunkerCourtCatalog.cs:277`. No Plan 153 focused test failed.
