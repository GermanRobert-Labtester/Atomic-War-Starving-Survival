---
name: ashfall-hotfix-rollback
description: Coordinates hotfix branching, cherry-pick, checksum-preserving save migration, PCK export smoke, and rollback plan. Use when patching release, cherry-picking to main, or handling save-breaking fixes.
---

# ASHFALL Hotfix & Rollback

## ROLE
`ashfall-release-captain` ships the happy path; you handle the incident. A hotfix must cherry-pick cleanly, preserve or explicitly migrate saves, smoke both export presets, and have a tested rollback.

## RULES
1. Hotfix branches from release tag, not `main` head.
2. Save wire compatibility is contractual (`SaveWireContract` 7 tests + `SaveStoreChecksumSweepTests` 12 per store). Breaking change needs `V(n)→V(n+1)` codec + legacy fallback, not silent shape change.
3. Verification is `dotnet` + `godot --headless` + export smoke — never assume cherry-pick is clean.

## WORKFLOW
### PHASE 1 — Triage & Branch
- Record tag/SHA, cherry-pick list, data vs code change, save-wire impact (none / additive / breaking).
- Branch `hotfix/<issue>` from tag; `git cherry-pick -x` each commit; resolve with minimal diff.

### PHASE 2 — Save Contract
- If DTO shape changed: bump `schema_version`, add `MigrationV{n}` in codec (`HoldfastSaveCodec`, `YearOfAshSaveCodec`, etc.), keep bare-state fallback for pre-checksum saves.
- Run `dotnet test --filter SaveWireContract` + `SaveStoreChecksumSweepTests` — mutated-state must change hash, null checksum rejected.

### PHASE 3 — Export Smoke
- `godot --headless --export-release "Linux" build/hotfix/` + `"Windows"`; verify PCK contains `StreamingAssets/Data/` (count JSON baked).
- Boot smoke: `godot --headless --path . -- --data-integrity-selftest` + `--bridge-selftest` + relevant `--*selftest` verbs.

### PHASE 4 — Rollback Plan
- Tag `rollback/<prev>`; document forward-rollback (revert commit vs restore DB) and save downgrade path (new saves loading on old build? must fail cleanly with `future version` error, not corrupt).
- `dotnet test` + `godot-asset-gate` green on both hotfix and rollback branch.

## OUTPUT
`docs/release/HOTFIX_<id>.md` — branch, picks, save impact, migration notes, export sizes, smoke logs, rollback steps, checklist sign-off.

## QUALITY GATE
- Cherry-pick clean, save contract tests 0 fails, both PCKs smoke, rollback tag exists and boots.
