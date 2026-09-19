# ASHFALL Release Process

> **Authority:** Plan 48 / C2[21] Release Craft
> **Script:** `scripts/release/prepare-release.sh`
> **Status:** ACTIVE
> **Last Updated:** 2026-09-19

---

## Overview

ASHFALL uses a tag-based release model. Every public release is represented
by a `vX.Y.Z` git tag. The release script automates the ceremony; humans
approve the version string and review the generated changelog section before
tagging.

Version bump classification follows [VERSIONING.md](VERSIONING.md):
- **Patch** (`vX.Y.Z+1`): bug fixes, content fixes, no schema changes
- **Minor** (`vX.Y+1.0`): new content, new optional features, no breaking changes
- **Major** (`vX+1.0.0`): breaking save migration, major feature overhaul

---

## Pre-Release Checklist

Before running `prepare-release.sh`, confirm:

- [ ] `main` branch is green on `bash scripts/ci/verify-fast.sh` (all 53 fast gates)
- [ ] `bash scripts/ci/release-gate.sh` passes on a clean checkout
- [ ] `python3 scripts/ci/version-gate.py` PASS (three-source agreement)
- [ ] `python3 scripts/release/generate_changelog.py --check` PASS
- [ ] Target version string is agreed (bump type decided per VERSIONING.md)
- [ ] All open PRs for this release merged to `main`
- [ ] No uncommitted work in the tree

---

## Release Ceremony

### Step 1 — Prepare

```bash
# Run from repo root on a clean main branch
bash scripts/release/prepare-release.sh --version 1.2.0 --base v1.1.0
```

The script will:
1. Verify three-source version agreement (aborts on mismatch)
2. Write the three version sites (`project.godot`, `Directory.Build.props`, `export_presets.cfg`)
3. Generate the `[1.2.0]` CHANGELOG.md section (git-log from `v1.1.0..HEAD`)
4. Capture a `schema_snapshot_v1.2.0.json` into `artifacts/golden_saves/historical/`
5. Commit: `release: prepare v1.2.0`
6. Print the tag command to run after review

### Step 2 — Review

Read the generated CHANGELOG section. Human-edit the machine-generated region
if needed (use the markers to identify it).

### Step 3 — Tag

```bash
git tag -a v1.2.0 -m "ASHFALL v1.2.0"
git push origin main v1.2.0
```

Tags **must never** be pushed without passing `release-gate.sh`.

### Step 4 — Verify

```bash
bash scripts/ci/release-gate.sh
```

---

## Branch Model

| Branch pattern | Purpose |
|---|---|
| `main` | Stable integration target. All fast gates must be green. |
| `feat/*` | Feature branches. Merged via PR with CI gate approval. |
| `hotfix/v*` | Hotfix branches cut from a release tag. See HOTFIX.md. |

**Never push version bumps or release tags manually to main.** Use `prepare-release.sh`.

---

## Retro v1.0.0 Tag Note

As of v1.1.0, there is no `v1.0.0` tag in the repository. The commit
`9b4985d0` (or equivalent pre-1.1.0 anchor) is the de-facto 1.0.0 state.
The recommendation is to **formally decline** creating a retro tag — the
absence of `v1.0.0` does not affect the support window (the historical corpus
records v1.1.0 as the earliest schema anchor), and retro-tagging could mislead
automated tools. Document the decision; do not tag retroactively.

---

## Related Documents

- [VERSIONING.md](VERSIONING.md) — version policy, three-axis model, semver mapping
- [HOTFIX.md](HOTFIX.md) — hotfix classification and iron rule
- [TEMPLATE.md](TEMPLATE.md) — changelog/release note template
- [SUPPORT.md](SUPPORT.md) — triage kit
- `docs/testing/FIXTURE_POLICY.md` §7 — historical corpus ceremony
