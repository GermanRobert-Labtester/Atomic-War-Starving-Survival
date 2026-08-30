# Plan 03 — `schema_version` & Data-Authority Hygiene Sweep

## Goal (2 lines)
Bring every root-level JSON catalog in `Assets/StreamingAssets/Data/` up to the authority
standard (snake_case keys + `schema_version`) and gate regressions, before the content
expansion plans (04–08) add dozens of new entries.

## Scope
- ~280 JSON files; AGENTS.md reports only ~35 carry `schema_version` today.
- Exclude `narrative/*.json` documents if their loaders intentionally use a document schema —
  confirm per-family before editing (196 files; don't bulk-blind-edit).

## Files to touch
- `Assets/StreamingAssets/Data/*.json` (root catalogs missing `schema_version`)
- `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` or a new checker — add a
  "schema_version present on root catalogs" rule
- Loader DTOs only if a loader would break on the added field (most ignore unknown fields —
  verify per loader)

## Steps
1. Inventory: list root catalogs lacking `schema_version` (script the count; do not hand-edit
   200 files without a generator).
2. For each file, add `"schema_version": 1` (or the family's current version if the loader
   already migrates — check codec V1→V2→V3 usage first).
3. While in each file, flag camelCase keys for the snake_case migration note (do NOT rename
   keys in this pass unless the owning loader is updated in the same commit — naming drift
   breaks binding silently; see the A11 parity audit in `docs/GODOT_MIGRATION_STATUS.md`).
4. Add the validator rule + a regression test: new root catalog without `schema_version`
   fails the data-integrity selftest.
5. Use skill `ashfall-data-schema` for the sweep mechanics.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest   # 0 errors, rule active
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj # loader binding tests still green
./scripts/ci/godot-asset-gate.sh                          # full gate if data loaders changed
```

## Risk
LOW–MEDIUM — pure data edits, but a loader that *strictly* validates unknown fields would
break; the loader tests from Plan 02 catch this, so run Plan 02 first.

## Definition of Done
- Every root catalog carries `schema_version`; validator gate enforced in CI;
  camelCase→snake_case migration notes filed per file (not executed) for a follow-up task.
