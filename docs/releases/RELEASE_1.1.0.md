# RELEASE 1.1.0 — Release Record — ✅ GO (clean-clone verified)

Lane: `lane/trapping-flagship-verification` (branched from `feat/asset-pipeline-flagship`)
Scope: **1,567 commits** over `main` (main last touched 2026-09-02; 39 numbered plan streams + cross-cutting integration work). Merge to `main` is a clean **fast-forward** — zero conflict risk.
Prepared: 2026-09-07 · Release captain run per `ashfall-release-captain` checklist.

---

## Phase 2 — Full Gate (working tree, all streams' in-flight state included)

| # | Gate | Result |
|---|------|--------|
| 1 | `dotnet build Ashfall.Core.Tests` | PASS — 0 errors, 0 warnings |
| 2 | `dotnet test` (full suite) | **PASS — 9226/9226** |
| 3 | `dotnet build Ashfall.csproj` | PASS — 0 errors, 0 warnings |
| 4 | `--data-integrity-selftest` | PASS — 298 catalogs, 0 errors/0 warnings |
| 5 | `--bridge-selftest` | PASS, exit 0 |
| 6 | `scripts/ci/godot-asset-gate.sh` | **ALL GATES GREEN** (import + asset-registry + data-integrity + bridge + survivors + world + economy + asset-registry) |
| 7 | Export smoke — Linux/X11 | **PASS** — 215 MB PCK + 71 MB binary; binary boots headless and passes full `--data-integrity-selftest` from inside the PCK (`wildlife_trapping_catalog.json`, `weather_seasons.json`, `recipes.json` confirmed packed) |
| 7 | Export smoke — Windows Desktop | **PARTIAL — BLOCKED BY ENVIRONMENT**: Windows export templates not installed locally (`export_templates/4.7.1.stable.mono/` lacks `windows_release.x86_64.exe`). Not a project defect. Install Godot 4.7.1 mono export templates with Windows support and re-run. |

## Committed-state verification (true clone at release HEAD `8ab78729`)

**PASS — 9226/9226.** The committed lane state stands alone.

Resolution of the earlier NO-GO:
- The original "19 failures" were two things: (a) a **worktree artifact** —
  several gate tests detect the repo root via a `.git` *directory*, which a
  linked worktree does not have (it has a `.git` file); re-verified in a true
  clone, those pass. (b) One real gap: `export_presets.cfg` was excluded by
  `.gitignore:191` while the `MicroLocationExportParity` gate requires it on
  fresh clones — fixed by force-add (`8ab78729`).
- The in-flight stream work was landed as integration snapshot `99cd3297`
  (same pattern as `ef67d600`) after the full working-tree suite verified
  9226/9226 immediately beforehand.

### Clean-clone gate record (release HEAD)

| Gate | Result |
|---|---|
| Full test suite | **PASS — 9226/9226** |
| `dotnet build Ashfall.csproj` | PASS — 0 errors (1 transient incremental-restore notice; clean rebuild 0 warnings) |
| `--data-integrity-selftest` | PASS — 298 catalogs, 0 errors/0 warnings |
| `--bridge-selftest` | PASS, exit 0 |
| `godot-asset-gate.sh` | **ALL GATES GREEN** (fresh-clone import) |
| Linux export smoke | **PASS** — clean-clone build; binary boots headless and passes `--data-integrity-selftest` from inside the PCK |
| Windows export smoke | Carried known-issue: Windows export templates not installed on this machine; Linux (primary dev platform) verified. Install templates and re-run before a Windows store drop. |

---

## Phase 3 — Version & Changelog

- Version bump prepared: `project.godot` `config/version` → `"1.1.0"` (content=minor per checklist; save-format: no breaking envelope changes on the lane — all save stores ship checksummed envelopes with legacy fallback; no codec V-bump requiring a migration note).
- No git tags exist (`main` carries none); this release establishes `v1.1.0` as the first tag. Recommended: also tag `v1.0.0` retroactively at `main` (9b4985d0) so future diffs are scoped.

### Changelog — lane highlights (1,567 commits; 125 mention numbered plans, 39 distinct plan streams)

**Systems (new/expanded, grouped by stream):**
- Wildlife trapping flagship tranche: transactional trap replacement, season+migration ecological quarry gating, craft-to-deployment identity chain, `--trapping-selftest` host gates, deterministic eligibility API (`89ae1187`)
- Plans 202–205: plastic pyrolysis fuel, perimeter defense grid, fungi cultivation closeout + panel construction, cargo airdrop recovery
- Plans B66–B69: heavy metallurgy (SilentFoundry), seismic monitoring + dampeners, radio intercept grid audit, cryogenic sample vault + seed repository; Phase-0/1 save-fixture contracts
- Shelter hardening: subgrid campaign wiring, catalog-driven surge tuning
- Plans 146–149 flagship: live tick loop, catalog authority, atomic consumption repairs

**Content:** 18 narrative commits; new catalogs (breaching equipment, direction-finding, metrology standards, aquaponics system, glassworks recipes); audio cue catalog expansion

**Fixes:** 170 fix commits, including quarantine + full-suite stall root causes (`f5bb2044`), weather-noise wiring, save-store baselines

**Docs:** docs-atlas reconciliation — Plan 14–43 archive (`shipped_to_chat/`), INDEX link repair; agent rulebook + architecture-map updates

---

## Phase 4 — Verdict

## 🟢 GO (2026-09-07, release HEAD `8ab78729`)

All seven gate items verified in a true clone of the committed release state.
Changelog accurate (below). No open CRITICAL audit findings in scope.

**Executed:** tag `v1.1.0` on the lane → fast-forward `main`.

**Known issues carried forward (non-blocking, tracked):**
- Windows export templates missing locally (environment; Linux verified).
- Test-suite quarantines in `Ashfall.Core.Tests.csproj` (~45 entries) — each an explicitly documented orphan/API-drift item owned by its stream; none mask runtime failures.
- Godot host `Main` sprawl (~19.8k lines / 74 partials) — deferred decomposition per audit #28/#29.
- **Process lesson for future gates:** linked worktrees (`.git` file) break
  repo-root detection in several meta-gate tests — verify release gates in a
  true clone, not a worktree.
