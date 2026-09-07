# RELEASE 1.1.0 — Release-Readiness Record (DRAFT — NO-GO for merge)

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

## Committed-state verification (clean worktree at lane HEAD `1fc16acb`)

**FAIL — 19/9116.** The committed lane state does not stand alone: five in-flight
streams have working-tree-only files their own gate tests require.

| Owning stream (evidence) | Failing gates at committed state | Missing uncommitted files |
|---|---|---|
| Metrology / Aquaponics (B86–B89 wave, files staged not committed) | `PersistentFilenameRegistryGateTests` — sections `precision_metrology`, `aquaponics` have no SaveStore files | `src/Host/PrecisionMetrologySaveStore.cs`, `src/Host/AquaponicsSaveStore.cs` (staged `A`) |
| Export/stream tooling | `MicroLocationExportParityTests` — `export_presets.cfg` absent from commit | `export_presets.cfg` (untracked) |
| Docs/rulebook stream | `AgentRulebookSyncGateTests`, `ArchitectureTestMapGateTests`, `DocLinkValidationGateTests` (committed state) | ~30 modified `docs/**` + `ANTIGRAVITY.md` |
| Production-art stream | `ProductionArtManifestTests` (13 tests — manifest file uncommitted) | art manifest + source catalog files |
| misc | remaining meta-gate deltas | balance of the 150 uncommitted files |

Working-tree full suite is 9226/9226 **because** those uncommitted files exist.
Per release discipline ("ship green-proven, not green-you-hope"), the merge is
blocked until the owning streams commit their files or explicitly hand them over.

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

## 🔴 NO-GO for merge-to-main (2026-09-07)

**Blocking condition (single):** the committed lane state fails 19 of its own
meta-gate tests because ~150 files from 5 in-flight streams are uncommitted.
Merging today ships a tree that fails its own test suite.

**Unblock sequence (in order):**
1. Each owning stream commits its staged/untracked files (list above; `git status` gate count → 0 in scope).
2. Re-run this checklist's Phase 2 on the committed state — all 7 gates must pass in a clean worktree.
3. Windows export templates installed → re-run export smoke for both presets.
4. Then: bump to `1.1.0`, tag `v1.1.0` on the lane, fast-forward `main`, and delete this draft's DRAFT marker.

**Known issues carried forward (non-blocking, tracked):**
- Windows export templates missing locally (environment).
- Test-suite quarantines in `Ashfall.Core.Tests.csproj` (~45 entries) — each is an explicitly documented orphan/API-drift item owned by its stream; none mask runtime failures.
- Godot host `Main` sprawl (~19.8k lines / 74 partials) — deferred decomposition per audit #28/#29.

*Draft prepared by the release-captain run of 2026-09-07. Do not tag until the unblock sequence completes.*
