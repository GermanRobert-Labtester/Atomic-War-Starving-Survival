# ASHFALL — Placeholder Triage

Phase 14R — per-placeholder classification.

Categorisation key:

- `A.ACTIVE_PRODUCTION_PLACEHOLDER` — drives a runtime render path; candidate for replacement.
- `B.GENERIC_RUNTIME_UTILITY` — referenced by chrome code paths (`icon = ...pattern`). Keep, do not replace.
- `C.TEMPLATE_REFERENCE` — used by tools (catalog generator, Figma, exporter). Keep.
- `D.DEBUG` — debug/test artefacts. Move out-of-runtime in a later cleanup phase.
- `E.DEPRECATED` — historical/legacy. Delete after a separately verified cleanup pass.
- `F.UNKNOWN` — review needed.

## Counts

| Bucket | Count |
|---|---|
| `A.ACTIVE_PRODUCTION_PLACEHOLDER` | 0 |
| `B.GENERIC_RUNTIME_UTILITY` | 0 |
| `C.TEMPLATE_REFERENCE` | 0 |
| `D.DEBUG` | 0 |
| `E.DEPRECATED` | 0 |
| `F.UNKNOWN` | 0 |

## Active production placeholders → eligible for replacement queue

There are 0 placeholder files in the active class. They are NOT auto-replaced; they are documented as candidates. A future art-replacement batch may target them on a per-ID basis.

| Stem | Path | Rationale |
|---|---|---|

## B.GENERIC_RUNTIME_UTILITY

| Stem | Path | Rationale |
|---|---|---|

## C.TEMPLATE_REFERENCE

| Stem | Path | Rationale |
|---|---|---|

## D.DEBUG

| Stem | Path | Rationale |
|---|---|---|

## E.DEPRECATED

| Stem | Path | Rationale |
|---|---|---|

## F.UNKNOWN

| Stem | Path | Rationale |
|---|---|---|

