# Plan 149 completion report

Date: 2026-09-09

Plan 149 is complete for the current 27-record bureaucratic document corpus. The records are discoverable through the existing Journal Events surface and remain presentation-only institutional memory. Static transcripts cannot mutate or overwrite live inventory, medical, population, staffing, room, ration, quest, faction or infrastructure state.

## Delivered result

| Measure | Result |
|---|---:|
| Authored documents | 27 |
| Distinct document types | 14 |
| Runtime-map records | 27 |
| Choices or effect verbs | 0; this corpus has no executable choice schema |
| Historical canonical records | 10 |
| Contemporaneous authored records | 6 |
| Template-compatible archival forms | 11 |
| Flavor-only artifacts | 0 |
| Unsafe/unresolved shipped records | 0 |
| Stable location IDs added | 0 |
| Identity aliases added | 0 |
| Explicit related-document source records | 15 |

## Runtime contract

- `BureaucraticDocumentCatalogLoader` validates schema version 1, required display fields, positive integer `posted_day`, duplicate IDs, mapping parity, truth class, producer IDs and related IDs.
- `BureaucraticDocumentDiscoverySystem` accepts only a known document ID, an explicit producer ID, a valid day and a non-unsafe truth class.
- Discovery is idempotent and writes only `KnowledgeKeys.BureaucraticDocument(doc_id)` through `JournalSystem.UnlockBureaucraticDocument`.
- `JournalCodex` adds locked/unlocked rows to the existing Events tab. The unlocked header shows truth class, authored day, poster, source location and material before the verbatim transcript.
- Audited related IDs render as links in `JournalBookUI`. Links switch to the Events tab and focus the target by stable ID; missing targets are omitted by the loader.
- No static quantity is parsed, projected or applied. No live template path was activated.

## Producer and presentation decisions

Real surface routes are wired for `archive_desk`, `medical_office`, `shelter_records` and `duty_roster`. The runtime map also records explicit future handoff contexts such as `maintenance_office`, `quartermaster_desk`, `gate_house_notice_board` and `school_corridor`; these tokens do not run a universal day unlock.

The representative slice is real: shelter records expose ration, assignment and notice records; medical office exposes medical paperwork; archive desk exposes requisitions, inspections and maintenance records; duty roster exposes roster records. All use `JournalBookUI` / `JournalCodex`.

All 11 template-compatible candidates remain archival. A future current report must use a separate typed generated-document schema, a current-report provenance badge and typed values from the owning system. The static transcript must not be edited in place.

## Canon and reference audit

The source's authored names and short roles remain display identities. Exact canonical catalog matches were recorded for `suki_tanaka` and `elena_rostov`; no runtime binding was required. `Bram`, `Tomas`, `Mira`, `Petr`, `Yelena`, the Loma family, the river woman and the unidentified adult were not fuzzy-mapped. Human-readable locations remain provenance strings because exact stable room mappings were not proven.

Authored numbers such as fuel litres, medicine courses, headcounts, filter percentages, generator hours, ration counts, ages and dates were classified in `DOCUMENT_TRUTH_CLASSIFICATION_MATRIX.md`. Form/report codes such as `IR-3-11`, `Req 7-L-12` and `Transfer Order 3-T-05` remain diegetic references; only explicit runtime-map IDs create navigation edges.

## Persistence policy

No new save section or document blob was added. Discovery uses the existing checksummed Journal knowledge ledger and campaign envelope. Old saves receive no retroactive document keys, including late-day saves. Restore reconstructs rows from the current catalog and saved keys without firing `OnCodexUnlocked` or replaying any consequence.

## Negative fixtures and regressions

`BureaucraticDocumentCatalogTests` covers the complete inventory, deterministic ordering, day gating, idempotence, save/restore without replay, unknown document/producer, duplicate authored ID, duplicate runtime-map ID, malformed fields, missing mapping and unresolved related target. The current focused suite has 7 passing tests.

## Content-utilization evidence

The static scanner recognizes both the authored catalog and runtime map as loaded and registered by `BureaucraticDocumentCatalogLoader` / `BureaucraticDocumentCatalog`, consumed by the discovery and Journal systems, and displayed by `JournalPanel`. The runtime collector exercised all 27 definitions at their authored days through the bounded discovery path.

The content-utilization self-test moved the repository snapshot from 222 to 224 gameplay-consumed catalogs, with 583 catalogs discovered, 0 orphaned, 71 unresolved, 1,114 runtime evidence events, 96 queried definitions, 48 selected definitions and 49 consumed definitions. The generated utilization report contains both Plan-149 source paths and the Journal surface edges.

## Verification commands

| Command | Result |
|---|---|
| `dotnet build Ashfall.Core/Ashfall.Core.csproj --no-restore --verbosity:minimal` | PASS; 0 warnings, 0 errors |
| `dotnet build Ashfall.csproj --no-restore --verbosity:minimal` | PASS; 0 warnings, 0 errors |
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --no-restore` | PASS; 10,341 / 10,341 |
| Focused `BureaucraticDocumentCatalogTests` | PASS; 7 / 7 |
| `godot --headless --path . -- --data-integrity-selftest` | PASS; 299 catalogs, 0 errors, 0 warnings |
| `godot --headless --path . -- --content-utilization-selftest` | PASS; 0 orphaned catalogs |
| `godot --headless --path . -- --journal-selftest` | PASS; 23 / 23 |
| `godot --headless --path . -- --journal-save-selftest` | PASS |
| `godot --headless --path . -- --narrative-selftest` | PASS; 10 / 10 |
| `godot --headless --path . -- --medical-selftest` | PASS; 15 / 15 |
| `godot --headless --path . -- --real-campaign-journey-selftest` | PASS |
| `python3 scripts/ci/run-gates.py --tier fast` | BLOCKED at pre-existing `BunkerCourtCatalog.cs:277` blank line; fail-fast stopped before Plan-149 checks |

The fast-gate block is unrelated to the Plan-149 files and was left untouched to avoid modifying concurrent work.
