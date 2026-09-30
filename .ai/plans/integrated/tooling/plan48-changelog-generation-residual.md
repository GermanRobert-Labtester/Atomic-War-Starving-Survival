# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

## Plan 48 residual — real changelog generation

## Acceptance — 2026-09-30

- Existing `--version/--base/--repo-root` generation CLI now writes categorized git-range notes and changed-path review prompts through the existing release caller seam.
- Isolated local git fixture `/tmp/ashfall-plan48-changelog.lWuVqu`: two commit categories and authored-data path rendered; markerless insertion and `--check --version 1.2.0` passed; repeat generation byte-identical. After an empty-subject commit and CRLF fixture, repeats retained SHA-256 `da635dfec9d2e3cbe927f75b5b29758f5d109b07ea7a1e163f401e7de8a48ddd`; outside-region text and CRLF bytes preserved.
- Missing/option-like base refs and invalid canonical semver refused; duplicate target headings refused without changing SHA-256. Live `python3 scripts/release/generate_changelog.py --check` passed. Read-only auditor's four findings repaired and re-review returned no remaining must-fix findings. Scoped `git diff --check` passed.
- Verification used the actual generator CLI in isolated git fixtures; no xUnit/full suite, live CHANGELOG writes, tags, commits or pushes in the project. Fixture-only commits supply the input history. No engine/runtime change requires a Godot probe.
- Limitations: subject-derived categories require editorial review; save/mod path lists are bounded review prompts, not schema/compatibility proofs. This seals the generator residual only, not the historical Plan 48 release ceremony or its broader acceptance claims.

> STATUS: APPROVED BY USER
> Authorization: “continue with next plan 1 integration!” (2026-09-30).

## Bounded outcome

Replace the existing successful no-op generation mode in `scripts/release/generate_changelog.py` with deterministic git-range notes inside the existing generated markers. Preserve human text outside that region. This is a residual package, not certification of the entire historical Plan 48 release ceremony.

## Current evidence and ownership

Original Plan 48 is recorded DONE; its generator still prints “generation mode stub” and returns zero. `prepare-release.sh` already invokes this exact CLI with `--version` and `--base`. Existing `--check` validates markers. No active claim owns this generator.

Claim exact generator, this plan/archive, `WORKTREE_OWNERSHIP.md`, `.ai/state.md`, and a residual entry in `INTEGRATION_PLANS.md`. Existing generated CHANGELOG.md and all release/CI/gameplay/save sources are read-only for this package.

## Implementation

1. Validate strict version, mandatory base ref, git revision resolution, ancestry, and existing marker shape before writing.
2. Render commit subjects with hashes by conventional change category in stable chronological order; include changed save/data/mod paths as review prompts without inventing schema or compatibility claims.
3. Replace only the marker body; insert markers under an exact target heading or Unreleased when absent. Refuse an ambiguous/missing target. Preserve file mode and replace atomically.
4. Check with isolated local git fixture CLI runs: generation, idempotency, marker validation, invalid ref/version refusal with no write. No full suite or release execution.
5. Read-only audit; record exact evidence. Mark repeated FULLY INTEGRATED and immediately archive this residual plan under `.ai/plans/integrated/tooling/`.

## Done

Real CLI output is generated from a verified base..HEAD range; repeated output is byte-identical; invalid inputs fail without changing the file; outside text is preserved. No tags, commits, pushes, or live changelog writes.
