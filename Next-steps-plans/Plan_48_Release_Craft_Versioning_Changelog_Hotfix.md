# Plan 48 — Release Craft: Versions, Tags, Changelog, and the Hotfix Path

> **Wave:** Continuity Wave 7 — *Content on Rails & the Measurement Layer*
> **Depends on:** 39A (release gate), 46A/46B (what a release report contains), 47C (breaking data
> changes feed the notes), 29A (the record must be green before anything is promised).
>
> **Theme:** the repo has **zero git tags**, a version string that reads `"1.0.0"` in
> `project.godot` and falls back to `"unknown"` in the CLI, no `CHANGELOG` anywhere, and no written
> statement of what a release is. Meanwhile the version-report *contract* is genuinely good —
> game/data/save schema versions pinned by tests — and there are two export presets and 46 CI gates.
> The engineering is release-grade; the release practice is absent. Also on the books: `AGENTS.md`
> still instructs lane/snap/`bit export` version-control discipline for a project that versions
> components with git and Godot exports.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | No releases have been tagged | `git tag \| wc -l` → **0** |
| 2 | Version exists as a bare string | `project.godot:11 → config/version="1.0.0"`; `src/Host/HostCli.cs:521–527` reads the project setting and defaults to `"unknown"` |
| 3 | No changelog, no release notes, no release doc | `find . -maxdepth 2 -iname "*changelog*" -o -iname "*VERSION*"` → only `Ashfall.Core.Tests/VersionReportContractTests.cs`; `ls docs \| grep -iE "release\|changelog"` → **0** |
| 4 | The version *report* contract is real | `Assets/Ashfall.Core/VersionReport.cs:60` composes game version + data schemas + save-schema versions; `VersionReportContractTests` asserts the render shape and that **every** save store version appears |
| 5 | Gates are numerous but untied to releases | `docs/ci/CI_GATE_MANIFEST.json` → 46 gates / 45 fast; `scripts/ci/release-gate.sh` does not exist (proposed in Wave 5's 39A); two of the fast gates were red at audit time (Wave 3's 29A) |
| 6 | Export exists but ships unproven | `.github/workflows/build.yml` exports raw `godot --export-release` (not `scripts/ci/godot-export-linux.sh`), verification steps can't fail, and no build is booted (Wave 3's 26B) |
| 7 | Save compatibility across versions is unspecified | codecs support V1→V2→V3 migration and the envelope carries `manifestVersion` 2 (Initiatives #41/#42), but no policy states which game versions may load which saves, and no test suite names a support window |
| 8 | Instructions describe another project's VCS | `AGENTS.md`'s "Saving and Publishing Changes" mandates `bit lane create` / `bit snap` / `bit export` and "never push to the main lane" — the workspace is a git/Godot repo (branch `main`, 95 uncommitted paths); the mismatch means there is effectively *no* versioned-artifact policy in force |
| 9 | The skills imply a process nobody runs | `ashfall-release-captain` (version bump, changelog from git history, pre-release gate) and `ashfall-hotfix-rollback` (cherry-pick, checksum-preserving migration, PCK smoke, rollback) exist as guidance with no repo counterpart |
| 10 | Content/data drift is untracked per release | `artifacts/content-utilization-baseline.json` and `artifacts/balance/*.csv` change without any release record (Wave 7's 46A/47C give them a home — this plan gives them a *when*) |

---

## Task 48A — One versioning scheme for three independently-versioned surfaces

**Goal:** state and automate how the game version, data schema versions, and save-schema versions
relate — so "compatible" is a computed answer, not a forum guess.

**Files:** `project.godot`, `Assets/Ashfall.Core/VersionReport.cs`,
`Ashfall.Core.Tests/VersionReportContractTests.cs`, `SaveSectionRegistry.cs` (`SchemaVersions`),
`src/Host/CatalogPath.cs` (data dir), `Assets/Ashfall.Core/Save/*Codec*.cs`
(`HoldfastSaveCodec`, `YearOfAshSaveCodec`, `DoseLedgerSaveCodec` …), `Directory.Build.props`,
new `docs/release/VERSIONING.md`, new `scripts/ci/version-gate.sh`, `.github/workflows/ci.yml`.

### Substeps

1. **Name the three axes** in one page: `game` (product semver), `data` (per-catalog
   `schema_version` + the game's accepted ranges), `save` (per-store `CurrentVersion` + envelope
   `manifestVersion`) — all three are already reported by `VersionReport`; none has a *policy*.
2. **Adopt semver with a documented rule** for what counts as major/minor/patch here:
   save-schema or mod-contract break = major; new reachability/content = minor; fixes = patch.
   Write the mapping down because this project's "breaking change" is a lost campaign.
3. **Single source of truth for the game version**: `project.godot`'s `config/version` plus
   `Directory.Build.props`/assembly version, asserted equal by a test, and **never** falling back to
   `"unknown"` (`HostCli.cs:521–527`) — an unknown version in a report is an unanswerable support
   ticket.
4. **Encode accepted data ranges** so a build states which catalog versions it accepts, with the
   existing `schema_version` presence gate as the floor and an explicit "newer than me" rejection
   message that names the file.
5. **Encode the save support window** per store: which `CurrentVersion` values load, which migrate,
   which are refused, and how far back a release promises to read — then assert it in a test that
   enumerates the codecs (the pattern `VersionReportContractTests` already uses for store coverage).
6. **Migration-path fixtures**: commit one real save file per supported historical version (Wave 5's
   27A step 7) and gate loading each on every push — this is the test that makes a version policy
   more than prose.
7. **Automate the bump**: a scripted release step that increments version(s), regenerates
   `VersionReport` expectations, and refuses to proceed if the contract tests were hand-edited to
   pass (the classic way a version policy dies).
8. **Fail CI on drift**: `scripts/ci/version-gate.sh` — version changed without a changelog entry; a
   store version changed without a migration test; a catalog `schema_version` bumped without a note.
9. **Publish the compatibility matrix** the mod contract needs (47A step 9) from the same source.
10. **Fix the instruction drift**: replace `AGENTS.md`'s `bit lane/snap/export` section with the
    actual git discipline for this repo (branching, tags, commit size, one system per commit), keeping
    every engine/architecture invariant byte-identical (Wave 3's 29 guardrail) — then regenerate the
    12 rulebook copies (29A).
11. **Tests**: version equality across sources, no `unknown` path, migration fixtures, drift gate
    self-proof (an un-bumped changelog fails).
12. **Run the checklist**.

**DoD:** "can build X load save Y with data Z" is answered by a test, not by archaeology.

---

## Task 48B — Releases as events: tags, changelog, artifacts, and the gate report

**Goal:** a release is one command producing a tagged commit, generated notes, tested artifacts, and
a machine-readable manifest of what shipped.

**Files:** new `scripts/release/prepare-release.sh`, new `CHANGELOG.md`, new
`docs/release/PROCESS.md`, `.github/workflows/build.yml`, Wave 5's
`scripts/ci/release-gate.sh` / `export-smoke-boot.sh`, `docs/ci/CI_GATE_MANIFEST.json`,
`artifacts/` reports, new `docs/release/TEMPLATE.md` (patch notes).

### Substeps

1. **Cut the first release properly** — tag the current state (`v0.x.0` with the version policy from
   48A) so there is a baseline to diff against; zero tags today means no known-good point exists.
2. **Generate the changelog from history**: conventional-commit prefixes → sections, with Waves'
   plan/task ids cited where the commit references them (the repo's commit style already does
   `feat(scope): … (Plan NN Task NX)`, which is unusually good material — use it).
3. **Record the three release-critical diffs** in every entry: save-schema changes,
   data-`schema_version` changes, and mod-contract changes (47C step 3 feeds this automatically).
4. **Balance and metrics lines** come from 46A's `DECISIONS.md` and 46B's funnel report — patch notes
   that say "rations are tighter" must be able to name the sweep that proved it.
5. **Artifacts must be complete and self-describing**: binary + PCK + deployed data + `report/`
   (version, gate list, counts, content-utilisation snapshot) — the failure mode Wave 5's 26B
   step 9 documented (an artifact missing its loose data).
6. **Attach a signed gate report** to every release: the manifest's 46 gates with pass/fail plus the
   added ones from 26B/39A; a release without its report doesn't exist.
7. **Pre-release checklist as a script, not a doc**: `prepare-release.sh --check` fails on an untagged
   HEAD, dirty tree, missing changelog section, red gate, or unbooted exported build.
8. **Define the tagging/branch model** concretely: `main` for development, `release/x.y` for
   stabilisation, `hotfix/*` from a tag, back-merge policy, and *never* a force-push past a tag — and
   delete the `bit` lane vocabulary from the rulebooks in the same commit (48A step 10).
9. **Publish patch notes from the template** (`docs/release/TEMPLATE.md`): player-facing language,
   no internal jargon, honest about known issues — with the known-issues list generated from
   `docs/CURRENT_AUTHORITY.md`'s register so it can't drift.
10. **Post-release verification**: run the exported-build boot + load-fixture + 7-day smoke against
    the *uploaded* artifacts, not the CI workspace.
11. **Retention**: keep every release's report and gate results in `docs/archive/releases/` so a
    regression can be bisected across releases rather than across commits.
12. **Tests**: `prepare-release.sh --check` against a deliberately broken candidate (missing changelog
    section, untagged HEAD) and a passing dry run.
13. **Run the checklist**, then execute one real (dry-run) release end to end and paste the report.

**DoD:** `prepare-release.sh` is the entire release process, and its output is the artifact players
get.

---

## Task 48C — Hotfixes, rollbacks, and the save you can't lose

**Goal:** a written, rehearsed path for shipping a fix to a release line without breaking a single
saved campaign — the scenario every one of Waves 1–6 could have caused.

**Files:** new `docs/release/HOTFIX.md`, `scripts/release/hotfix.sh`, save codecs +
`Ashfall.Core/Save/*`, `SaveChecksum`/`SaveEnvelopeHelper`, `SaveStore<T>` (`.bak` rotation),
`artifacts/` save fixtures (48A step 6), `.github/workflows/` (a backport job),
`Next-steps-plans/` wave ledger (29C) for decision records.

### Substeps

1. **Write the playbook first**: classify a fix as `patch` (no schema change) / `data` /
   `save-affecting` / `breaking`, and require the classification in the PR title so the release
   automation knows which tests to run.
2. **Rehearse the happy path on a branch cut from the last tag**: cherry-pick, rebuild, re-run the
   exported-build boot, publish, and back-merge to `main` — documented *and* executed at least once
   before it's needed.
3. **The save-compat rule**: a hotfix may never require a migration; if it must, it isn't a hotfix.
   Enforce with a diff test: fixture saves from the release load unchanged and produce identical
   checksums.
4. **Rehearse the rollback path**: how to revert a release while keeping saves readable (the
   checksums, envelope `manifestVersion`, and `.bak` rotation are the machinery) and how to tell
   players what happened.
5. **Define the data-only hotfix route**: with Wave 7's content packs and the `ASHFALL_DATA`
   override, a broken catalog may be fixable without a binary rebuild — state whether that's
   supported, because it changes the emergency response time from days to minutes.
6. **Emergency content quarantine**: a procedure to disable a broken content family by
   configuration/exemption rather than code change (safe because the acceptance ladder already knows
   what depends on what).
7. **Support triage kit**: the artifact bundle a player must send (version report, effective content
   report, day record, save header) — assembled by a command, redaction-checked, since 31C/46B already
   define what's safe to include.
8. **Post-mortem template** with a required field: which gate *would* have caught this, and add that
   gate if it doesn't exist — the loop that turns an incident into a permanent fix.
9. **Backport automation**: a workflow that runs the hotfix branch's gates against the release's
   fixtures (not `main`'s), because the common failure of a hotfix is testing the wrong tree.
10. **Version discipline under pressure**: hotfix increments are patch-level only, and the changelog
    entry is generated (48B) — no silent schema drift in an emergency, which is 48A step 7's
    "refuse if hand-edited" rule earning its keep.
11. **Tests**: a CI job that loads release-line fixtures against a candidate build and asserts
    checksum stability, plus a rollback rehearsal test.
12. **Docs**: `docs/release/HOTFIX.md` + `docs/ARCHIVE_INDEX.md` pointers; record the rehearsal run in
    the wave ledger (29C).
13. **Run the checklist** + a full rehearsed dry-run hotfix against the tag cut in 48B step 1.

**DoD:** a rehearsed path exists from "bug found on a release" to "fixed, shipped, saves intact".

---

## Cross-Task Dependencies

```
29A (green gates) ──► 48A (a version policy nobody trusts if CI is red)
39A (release gate) ─► 48B steps 6–7               46A/46B ──► 48B steps 4,7 (patch notes content)
47A/47C (mod contract, breaking diff) ──► 48A step 9, 48B step 3, 48C step 5
48A (versioning) ──► 48B (release) ──► 48C (hotfix)
     AGENTS.md lane/bit rewrite (48A step 10) ──► makes the VCS policy real
```

**Execution order:** 29A → 39A → 48A → 48B → 48C. Do not tag a release before 48A's compatibility
tests exist — an untested tag is a promise with no evidence behind it.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. bash scripts/ci/version-gate.sh                               # (48A step 8)
7. bash scripts/ci/release-gate.sh                               # all gates + report artifact
8. bash scripts/ci/export-smoke-boot.sh                          # boots the shipped artifact
9. bash scripts/release/prepare-release.sh --check --dry-run     # (48B step 7)
10. fixture-save compatibility: every supported version loads + checksum-stable
11. git tag --points-at HEAD                                     # after the first real cut
12. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Code | Scripts/Docs | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|
| 48A | 2–3 (version sources, codecs) | 2 docs + 1 gate | 8–12 | Medium | LOW (additive; one `bit`-era doc rewrite) |
| 48B | 0 | 3 scripts + 3 docs | 4–6 | Low–Med | LOW (CI-side) |
| 48C | 0–1 | 2 docs + 1 workflow | 5–8 | Medium (rehearsal-heavy) | LOW if rehearsed, HIGH if invented during an incident |

**Guardrails:** never hand-edit a generated report or a contract test to make a release pass; no
force-push past a tag; no save-schema change in a hotfix; no version claim without a fixture that
proves it; no player-facing promise (mod support, backwards compatibility, patch cadence) that a gate
doesn't enforce — Waves 3 and 5 both ended with the same lesson: *undocumented promises rot, and
untested claims ship*.
