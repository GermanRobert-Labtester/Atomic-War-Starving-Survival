# Plan 142 Implementation Log

## Phase 1 — Baseline and source map

Status: PASS

Changed:

- Added the Plan 142 baseline, schema, source, dedup, timestamp, author,
  producer, save, and regression documents.

Evidence:

- 189 Plan 142 records inventoried.
- Exact IDs and normalized bodies are unique.
- Focused journal tests: 110/110 PASS.

## Phase 2 — Canonical adapter

Status: PASS

Changed:

- Added `JournalCorpusCatalogLoader`, `JournalCorpusCatalog`, and
  `JournalCorpusAdapter` in Core.
- Bound the immutable catalog during `Main.SetupJournal`.
- Added the producer-bound `TryAddAuthoredEntry` seam and map discovery hook.
- Added utilization scanner/runtime evidence for all five Plan 142 files.
- Updated the narrative acceptance decision and utilization baseline.

Tests:

- Focused corpus/system tests: 34/34 PASS.
- Focused journal/utilization tests before the map hook: 166/166 PASS.
- Host build: PASS.
- Journal self-test: 23/23 PASS.
- Journal UI smoke test: PASS.
- Content utilization gate: PASS.

Result:

- 189 authored records normalize into one canonical insertion path.
- No authored records are inserted at boot.
- Exact IDs resolve to canonical survivors; unresolved ambient names stay
  display-only.
- Faction-war and voice-prose sources remain separate.

Divergences:

- Ambient title/type/tags remain read-only adapter metadata because the
  persisted/UI contract does not contain those fields.

## Phase 3 — Verification and closeout

Status: PASS for Plan 142; repository-wide closeout is blocked by unrelated
manifest data.

The scoped gates passed:

- Core corpus/system: 34/34.
- Journal/utilization subset: 166/166.
- Godot host build: 0 warnings, 0 errors.
- Journal domain self-test: 23/23.
- Journal save self-test.
- Journal UI smoke test.
- Content-utilization self-test.
- Catalog boot preflight: 299/299.
- Save/load UI failure-path: 8/8.
- Bridge self-test.

The full Core suite reached 10,045 passed and 7 failed. The data-integrity
self-test reports 79 errors. Both failures are confined to unresolved IDs and
channels in `narrative_discovery_manifest.json`, outside the Plan 142 source
set. No unrelated manifest edits were made.
