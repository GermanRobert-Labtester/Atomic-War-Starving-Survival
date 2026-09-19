# ASHFALL Hotfix Guide

> **Authority:** Plan 48 / C2[21] Release Craft
> **Script:** `scripts/release/hotfix.sh`
> **Status:** ACTIVE
> **Last Updated:** 2026-09-19

---

## Hotfix Classification

Not every urgent fix is a hotfix. Classify first.

| Class | Criteria | Allowed schema change? | Route |
|---|---|---|---|
| **Patch hotfix** | Bug/crash in released version, no new features | NO | `hotfix/v*` branch |
| **Data-only fix** | Typo/balance correction in JSON only, no code | NO | `hotfix/v*` branch |
| **Content fix** | Missing audio/art asset in exported build | NO | `hotfix/v*` branch |
| **Feature work** | Any new functionality, even small | — | Regular `feat/*` PR to `main` |
| **Schema migration** | Any codec `SchemaVersion` bump | NEVER | Cannot be a hotfix |

### Iron Rule

> **A hotfix MUST NOT alter any save codec `SchemaVersion` constant.**

Save schema changes require migration logic, full regression testing, and a
proper minor/major version bump. Hotfixes skip these safeguards. A hotfix that
migrates saves is a data-corruption risk for players with saves from the patched
version.

`version-gate.py --hotfix` enforces this rule automatically. The `hotfix.yml`
CI workflow runs it on every push to `hotfix/*` branches.

---

## Emergency Quarantine

If a released version is causing data corruption or save loss:

1. Do NOT push a hotfix until the root cause is confirmed.
2. Open an issue with `[QUARANTINE]` label.
3. Communicate to players: "Save often. Do not update until advised."
4. Run `scripts/ci/version-gate.py --hotfix --base vX.Y.Z` on every candidate fix commit.
5. Document findings in `docs/releases/POSTMORTEM_TEMPLATE.md`.

See `docs/testing/FIXTURE_POLICY.md` §7 for the historical corpus ceremony after a hotfix lands.

---

## Hotfix Mechanics

### Step 1 — Branch from the release tag

```bash
# ALWAYS branch from the release tag, not from main
git checkout -b hotfix/v1.1.1 v1.1.0
```

### Step 2 — Apply the minimal fix

```bash
# Fix ONLY the bug. No feature changes. No schema bumps.
# Run the hotfix gate after every change:
python3 scripts/ci/version-gate.py --hotfix --base v1.1.0
```

### Step 3 — Use hotfix.sh

```bash
bash scripts/release/hotfix.sh --version 1.1.1 --base-tag v1.1.0 --classify
```

The `--classify` flag runs the classification check and tells you if your
branch is eligible for a hotfix (no schema changes, no new features).

### Step 4 — Verify

```bash
bash scripts/ci/release-gate.sh
```

### Step 5 — Tag and merge back

```bash
# Tag the hotfix
git tag -a v1.1.1 -m "ASHFALL v1.1.1 (hotfix)"
git push origin hotfix/v1.1.1 v1.1.1

# Merge back to main (do NOT rebase — preserve the hotfix history)
git checkout main
git merge --no-ff hotfix/v1.1.1 -m "merge: hotfix v1.1.1 back to main"
git push origin main
```

### Step 6 — Historical corpus ceremony

After a hotfix lands, add the schema snapshot (even if schema versions are
identical — the corpus records every release tag):

```bash
bash scripts/release/prepare-release.sh --version 1.1.1 --base v1.1.0
# Schema map will be identical to v1.1.0 — that is expected and correct.
```

---

## Rollback Semantics

If a hotfix introduces a regression:

1. Revert to the previous tag: `git checkout v1.1.0`
2. The reverted build has known-good saves — no migration needed.
3. The corrupt hotfix tag may be deleted from the remote if no players have updated.
4. File a postmortem using `docs/releases/POSTMORTEM_TEMPLATE.md`.

---

## Data-Only Route

For JSON-only fixes (no C# changes):

1. All the same rules apply.
2. `version-gate.py --hotfix` will pass (no schema constants changed).
3. Verify with `godot --headless --path . -- --data-integrity-selftest`.
4. The schema snapshot for the hotfix version will be identical to the base.

---

## Related Documents

- [PROCESS.md](PROCESS.md) — full release process
- [VERSIONING.md](VERSIONING.md) — version policy
- [SUPPORT.md](SUPPORT.md) — player triage kit
- [POSTMORTEM_TEMPLATE.md](POSTMORTEM_TEMPLATE.md) — post-incident analysis
