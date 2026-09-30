# E1C Plan Metadata Migration — Implementation Log

Date: 2026-09-30
Status: **FULLY INTEGRATED**

## Delivered

E1C adds a dry-run-first, one-time plan metadata migrator at
`scripts/ci/migrate-plan-metadata.py`. It captures an execution baseline,
requires the reviewed proposal digest before writing, preserves existing
unknown metadata and all body bytes, records pre/post and body hashes, and
proposes stable path-derived IDs when filename-derived IDs collide. It keeps
ambiguous metadata visible in the review queue and never infers `DONE` from
prose.

The current execution baseline contains 621 plan paths and their pre-migration
SHA-256 values. The E1A baseline remains unchanged at 609 paths. Its
reconciliation records 601 paths still present, eight historical root paths
missing with non-identical `shipped_to_chat` candidates (historical Git blob,
historical SHA-256, and current candidate SHA-256 are retained), and twenty
current paths added after E1A. These are documented as changed copies, not
claimed as byte-identical moves.

All 621 current paths now have schema-versioned metadata. Proposed statuses
are `PROPOSED` (592), `BLOCKED` (28), and `READY` (1); there are zero `DONE`
proposals. Because the corpus does not authoritatively classify categories,
all 621 are provisionally `PROCESS`, marked `INFERRED: true`, and listed for
human review. Duplicate filename identities use stable path IDs and remain in
that review queue. The migration report records all 621 decisions and the
final pre/post SHA-256 and body SHA-256 for every file.

The generated plan register covers 621/621 files with metadata state
`COMPLETE`, zero duplicate IDs, and zero validation errors. The docs index
generator now streams Markdown to avoid loading large files in memory and
ignores front matter when selecting titles and summaries; it regenerated the
5,609-document index without exposing YAML metadata as prose.

## Verification

- `python3 scripts/ci/migrate-plan-metadata.py --self-test` — PASS.
- `python3 scripts/ci/migrate-plan-metadata.py --report-out /tmp/e1c-final-noop.json` — PASS; 621 plans, changed 0, source drift 0.
- `python3 scripts/ci/generate-plan-register.py --write` and `--check` — PASS; 621 plans, zero validation errors.
- `python3 scripts/ci/generate-docs-index.py` and `--check` — PASS; 5,609 documents.
- `bin/run-scoped-tests Ashfall.Core.Tests/Tooling/PlanGovernanceContractTests.cs` — PASS 5/5.
- Report invariants — PASS: baseline path parity, no duplicate IDs, no inferred `DONE`, no body-hash mismatches, no source drift, and an idempotent second run.

The review queue is intentionally retained as follow-up human review rather
than silently presenting inferred category, title, or status values as
authoritative. No gameplay, save, UI, or runtime behavior changed. No full
test suite or commit was run.
