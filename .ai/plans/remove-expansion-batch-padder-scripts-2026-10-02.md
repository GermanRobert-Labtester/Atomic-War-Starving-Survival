# Remove Expansion Batch Padder Scripts (2026-10-02)

> **STATUS: APPROVED BY USER** — user authorized deletion and commit in-session
> (2026-10-02, "the expand oldest plans yeah safe to delete those!" / "yes please commit").

## Goal

Remove the 61 completed one-shot plan-expansion padding scripts from
`scripts/tools/`, plus their stale `__pycache__` bytecode. These are finished
batch writers with no runtime or CI role.

## Scope (exact paths)

- `scripts/tools/expand_oldest_485_plans_batch*.py` — 61 files, ~570,040 lines
- `scripts/tools/__pycache__/expand_oldest_485_plans_batch*.pyc` — 61 files

## Non-goals

- No change to `scripts/ci/` (wired into 6 GitHub workflows — load-bearing).
- No change to the other 574 `scripts/tools` files.
- No change to any `docs/` markdown, including the padded outputs.
- No change to C#, Core, host, data, or save behaviour.

## Evidence (pre-change verification)

1. **Not tests.** `pyproject.toml` sets `testpaths = ["tests"]` and
   `python_files = ["test_*.py"]`. These files live in `scripts/tools/`, are not
   named `test_*.py`, and contain zero `def test_` definitions. The repo's entire
   pytest suite is one file: `tests/test_audio_pipeline.py`.
2. **Not invoked by CI.** No reference from `.github/workflows/*` or any tracked
   file outside the files themselves.
3. **Embed no unique content.** Each script reads an existing `.md` and appends
   templated filler up to a 1,280,000-character target. Prose lives in the
   markdown, not the Python.
4. **All referenced outputs survive.** 2,500 unique output paths referenced by
   the 61 scripts: 1,918 present at original path, 560 relocated to
   `docs/plans/integrated/`, 22 present under `docs/plans/integrated/<category>/`.
   Zero unaccounted.

## Result

Python: 1,024,536 lines / 829 files → 454,496 lines / 768 files.

## Known limitation carried forward (NOT addressed here)

The committed plan markdown still contains the templated filler these scripts
wrote, including formulaic references to `SupplementalBoundary###Test` xUnit
tests and `supplemental_###` save keys that were never verified. Some plan
files are multi-megabyte as a result (e.g.
`docs/expansions/prose_wave144/cw144_06_the_registrar_keeps_a_copy_plan.md`,
4.5 MB). This is a content-accuracy question for the docs tree, tracked
separately, and deliberately not in this package's scope.

## Recovery

Files were tracked and unmodified at deletion time, so `git restore
scripts/tools/` restores them. This commit is a normal forward commit; history
is not rewritten.
