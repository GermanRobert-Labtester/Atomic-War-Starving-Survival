# PLAN 48 / C2[21] — Release Craft Closeout

> **Status:** COMPLETE
> **Completed:** 2026-09-19
> **Branch:** `feat/unblock-cf-p28-and-plan-implementation`
> **Commits:** Phase 1+2 `c418e6f7`, docs index fix `3119b61f`,
>              Phase 3 `c2aa6a58`, Phase 4 `034cdffd`, Phase 5 `aa2eef72`,
>              Phase 6 (this commit)

---

## Summary

Plan 48 (C2[21]) — *Release Craft: Versioning, Changelog, and the Hotfix Path* —
has been fully integrated across all six phases.

---

## Phases Completed

### Phase 0 — Premise Audit (prior session)
- Verified all three version sources; fixed `export_presets.cfg` drift (`1.0.0` → `1.1.0`)
- Registered `claim-c2-21-release-craft-2026-09-19` in `WORKTREE_OWNERSHIP.md`

### Phase 1 — Version Policy + Truth
- `Assets/Ashfall.Core/ReleaseVersion.cs`: engine-free strict-semver parser
- `Directory.Build.props`: `<VersionPrefix>1.1.0</VersionPrefix>`
- `export_presets.cfg`: file/product version synced
- `src/Host/HostCli.cs`: `PrintVersion` uses `ReleaseVersion.TryParse`
- `docs/releases/VERSIONING.md`: three-axis version policy
- `Ashfall.Core.Tests/Release/ReleaseVersionContractTests.cs`: 32 tests — PASS

### Phase 2 — Drift Gates
- `scripts/ci/version-gate.py`: three-source + semver drift gate (fast, critical)
- `scripts/release/generate_changelog.py --check`: marker-region drift gate (fast)
- `docs/ci/CI_GATE_MANIFEST.json`: 53 → 55 fast gates (+ version_gate + changelog_drift)
- `docs/architecture/CLAIMS.json`: 24 → 28 claims (4 release-craft claims)

### Phase 3 — Save Support Window + Fixtures
- `Ashfall.Core.Tests/Save/SaveSupportWindowTests.cs`: 15 tests — all PASS
  - Pins 6 curated codec schema versions at game v1.1.0
  - Validates historical corpus manifest structure
  - Enforces no-regression invariant (INV-48.2)
- `artifacts/golden_saves/historical/manifest.json`: initial corpus index
- `artifacts/golden_saves/historical/schema_snapshot_v1.1.0.json`: v1.1.0 anchor
- `docs/ci/CI_GATE_MANIFEST.json`: + `save_support_window` (full tier): total 57 gates
- `docs/testing/FIXTURE_POLICY.md`: §7 addendum — historical corpus invariants

### Phase 4 — Release Pipeline
- `docs/releases/PROCESS.md`: branch/tag model, ceremony, retro tag decision
- `docs/releases/TEMPLATE.md`: release note template with generated-region markers
- `CHANGELOG.md`: `[1.1.0]` backfill with generated-region markers
- `scripts/release/prepare-release.sh`: full release ceremony automation
- `scripts/ci/release-gate.sh`: fast + release-specific + full-tier gate runner
- `.github/workflows/build.yml`: + `version_gate` step
- `.github/workflows/release.yml`: tag-triggered workflow

### Phase 5 — Hotfix Path
- `docs/releases/HOTFIX.md`: classification table, iron rule, emergency quarantine
- `scripts/release/hotfix.sh`: --classify mode + full gate runner
- `.github/workflows/hotfix.yml`: iron rule CI gate on `hotfix/*` branches
- `docs/releases/SUPPORT.md`: player triage kit, S1-S5 severity, save issue triage
- `docs/releases/POSTMORTEM_TEMPLATE.md`: structured post-incident template
- **Rehearsal proof**: `HOTFIX_GATE FAIL` correctly refused (40+ schema-constant files);
  iron rule enforcement proven.

### Phase 6 — Hygiene + Close
- `.agents/skills/ashfall-release-captain/SKILL.md`: `bit lane/snap` → git branch + `prepare-release.sh` vocabulary
- `docs/agents/AGENT_SKILLS_INDEX.md`: regenerated (35 skills)
- `docs/architecture/CLAIMS.json`: `rel_save_support_window_tested` updated to
  `TRUE / PROVEN_INTEGRATION`; `release_fixture_matrix` stale gate ref removed
- `docs/plans/PLAN_48_RELEASE_CRAFT_CLOSEOUT.md`: this document

---

## Gate Counts at Closeout

| Tier | Count |
|---|---|
| Fast | 53 |
| Full | 3 |
| Performance | 1 |
| **Total** | **57** |

New gates added by Plan 48: `version_gate` (fast), `changelog_drift` (fast),
`save_support_window` (full).

---

## Claims State at Closeout

| Claim ID | Status | Confidence |
|---|---|---|
| `rel_version_single_source` | TRUE | PROVEN_INTEGRATION |
| `rel_changelog_gated` | TRUE | PROVEN_INTEGRATION |
| `rel_save_support_window_tested` | TRUE | PROVEN_INTEGRATION |
| `rel_hotfix_never_migrates` | TRUE | PROVEN_INTEGRATION |

All 28 capability claims verified. 0 `UNVERIFIED`.

---

## Debt / Decisions

| Decision | Rationale |
|---|---|
| No retro `v1.0.0` tag | Absence doesn't affect support window; retro tags mislead tooling. Documented in PROCESS.md. |
| `release_fixture_matrix` gate not created | This was a Phase 3 aspirational gate. The `save_support_window` gate covers the same invariant; a separate release-tier matrix gate adds no verified value at this stage. |
| Historical corpus starts at v1.1.0 | The de-facto 1.0.0 state predates the corpus; 1.1.0 is the first formally tracked release. |

---

## Next Steps

The release infrastructure is complete. The next step is to run
`bash scripts/release/prepare-release.sh --version 1.1.0 --base <anchor-sha>`
as part of cutting the actual `v1.1.0` release tag when the branch merges to
`main` and all fast CI gates pass.
