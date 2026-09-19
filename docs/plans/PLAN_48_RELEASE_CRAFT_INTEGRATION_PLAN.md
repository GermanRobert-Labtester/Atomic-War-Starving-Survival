# PLAN 48 (C2[21]) — Release Craft: Versioning, Changelog & Hotfix Path — Integration Plan

> **Package:** `C2[21]/Plan 48` — Release Craft: Versions, Tags, Changelog, and the Hotfix Path
> **Source document:** `Next-steps-plans/shipped_to_chat/Plan_48_Release_Craft_Versioning_Changelog_Hotfix.md` (Continuity Wave 7, tasks 48A/48B/48C)
> **Census status:** `AUDIT-PENDING` → promoted to available by `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md` (row 8: "Declared prerequisite C1[7]/Plan 29 is sealed; standard premise audit first")
> **Plan type:** Tooling/process plan. Zero gameplay, zero simulation, zero new runtime state authority.
> **Author:** senior systems architect / integration planner (ashfall-plan role)
> **Date of evidence:** 2026-09-19, HEAD `65357b8a` (branch `Zcode_Branch`), all citations re-verified against current source
> **⚠ Numbering collision:** this is **NOT** the weather/route-gates Plan 48. See §4.0 before citing "Plan 48" anywhere.

---

# 1. Objective

Make an ASHFALL release a **computed, gated, reproducible event** instead of a manual act of
discipline, and make "will my save survive this update?" a **tested answer** instead of an
unwritten hope. Concretely, this package delivers four outcomes:

1. **One versioning policy across the three independently-versioned surfaces** that already
   exist — game/build version (`project.godot` `config/version`), data schema versions
   (per-catalog `schema_version`), and save schema versions (per-codec `CurrentSaveVersion` /
   envelope `manifestVersion`) — with the relations between them written down and enforced by
   gates, not by memory.
2. **A release pipeline as scripts**: a version gate, a generated-and-checkable changelog, a
   release gate composing the existing 53-gate manifest plus export smoke, and a
   `prepare-release.sh --check` that fails on every failure mode the manual 1.1.0 release had
   to catch by hand.
3. **A rehearsed, save-safe hotfix path**: written classification rules ("a hotfix may never
   require a migration"), automated scope enforcement, fixture-based checksum-stability proof,
   a rollback procedure anchored on the existing `.bak`/envelope-migration machinery, and a
   support-triage kit assembled from reports that already exist.
4. **Closure of the remaining instruction drift**: the one live file still carrying
   `bit`-era vocabulary (the `ashfall-release-captain` skill), the stale Windows export-preset
   version fields, and the `HostCli` `"unknown"` version fallback.

**Non-goals** (explicit): no save-format changes, no codec changes, no data-catalog changes, no
new gameplay systems, no telemetry/opt-in metrics (that is C2[20]/Plan 46, unexecuted), no
long-run soak gate (that is C2[16]/Plan 39, unexecuted), no store-page/public-release
logistics, no modding API promises beyond citing the already-sealed Plan 47 contract, no
rewrite of the CI manifest beyond additive gate registration, and no tagging or pushing
executed by the builder without explicit user/foreman approval at the phase checkpoint.

**Bounded outcome statement (restated per workflow rule 1):** at package close,
`bash scripts/ci/release-gate.sh` is the entire pre-release verification; one dry-run release
has been executed end-to-end producing a tag candidate, a generated changelog section, a gate
report artifact, and a `docs/releases/RELEASE_<version>.md` record in the existing release-record
format; and `docs/releases/HOTFIX.md` plus one rehearsed dry-run hotfix prove a fix can ship to
a release line with byte-identical save checksums on every committed fixture.

---

# 2. Current Reality

Everything in this section was re-verified against current source on 2026-09-19 at HEAD
`65357b8a`. Where the source plan's evidence inventory (audited at `ccac926e`) disagrees with
current reality, the row is marked **STALE** or **PARTIALLY STALE** — that divergence is the
premise audit this package's entry gate requires, and it is recorded here so no builder
re-implements an already-landed piece or trusts an already-fixed gap.

## 2.1 The three version surfaces, as they exist today

**Surface 1 — game/build version.**

- `project.godot:15` → `config/version="1.1.0"`. This is the runtime authority: Godot bakes it
  into exports and `ProjectSettings.GetSetting("application/config/version")` reads it at
  runtime.
- `src/Host/HostCli.cs:772-782` (`PrintVersion`) still contains the silent fallback the source
  plan flagged:

  ```csharp
  public static void PrintVersion(string dataDir)
  {
      string gameVersion = "unknown";                                   // :774
      var setting = ProjectSettings.GetSetting("application/config/version");
      if (setting.VariantType == Variant.Type.String)
          gameVersion = setting.AsString();
      if (string.IsNullOrEmpty(gameVersion))
          gameVersion = "unknown";                                      // :779
      GD.Print($"\n{VersionReport.Compose(gameVersion, dataDir)}");
      GD.Print($"data resolution: {CatalogPath.ResolveDataDir()} [source: {CatalogPath.LastResolutionSource}]");
  }
  ```

  An exported build with a missing/empty version setting prints `unknown` and exits 0. The
  fallback is **live today**.
- `export_presets.cfg` preset 1 (Windows Desktop) carries **stale duplicate version fields**:
  `application/file_version="1.0.0"` and `application/product_version="1.0.0"` — the
  `project.godot` bump to `1.1.0` (commit `47e93fb7`, 2026-09-07) did not update them. This is
  a *new* drift finding the source plan could not have seen at `ccac926e` (version was `1.0.0`
  everywhere then): the repo has **three** game-version write sites, and two of them already
  disagree.
- `Directory.Build.props` (root, applies to all three csproj files) sets `LangVersion`,
  `Nullable`, `Deterministic`, and the shared `NoWarn` policy — **no `<Version>` /
  `<VersionPrefix>` / assembly version anywhere**. `Ashfall.csproj` likewise has no version
  property. Assembly metadata is therefore the SDK default (`1.0.0`), a fourth implicit value
  nobody reads.
- `scripts/ci/export-build.sh` §5 already writes a `builds/linux/RELEASE_STAMP.txt` with
  `commit=`, `configuration=`, `godot=`, `exported_at=` — artifact provenance machinery exists
  but does not include the game version and is not wired into CI (`build.yml` inlines its own
  export commands instead of calling this script).

**Surface 2 — data schema versions.**

- `Assets/Ashfall.Core/VersionReport.cs` `ScanDataSchemas(dataDir)` recursively inventories
  every `*.json` under the data authority and tallies top-level `schema_version` (missing or
  unparseable → version 0). The pinned contract test
  (`VersionReportContractTests.ScanDataSchemas_LiveDataAuthority_IsNonTrivialAndCurrent`)
  asserts ≥100 catalogs and ≥100 with `schema_version`; the class doc-comment example reads
  "403 catalogs — v1: 401, v2: 2 (max v2)". The Wave 7 continuity index counted 411 files
  carrying `schema_version`. The scanner is an inventory, not a gate: nothing today rejects a
  catalog whose `schema_version` is *newer than the build understands* — there is no
  "accepted range" concept anywhere in a loader; the floor gate
  (`scripts/ci/json-schema-policy-gate.sh`) checks presence/shape, not compatibility.
- `Assets/StreamingAssets/Data/` is the sole data authority (AGENTS.md rule 3);
  `src/Host/CatalogPath.cs` resolves it with the `ASHFALL_DATA` environment override at
  precedence #1 (Wave 7 audit, row 7; Plan 47's `docs/mods/MOD_CONTRACT.md` is the sealed
  statement of what a pack may rely on).

**Surface 3 — save schema versions.** This is the strongest surface and the one the release
plan must *integrate with, not compete with*:

- Per-store codec constants (grep-verified, current): `HoldfastSave.CurrentSaveVersion = 5`,
  `YearOfAshSave.CurrentSaveVersion = 5`, `DoseLedgerSave.CurrentSaveVersion = 2`
  (+ `MigrationFromVersion = 1`), `ExpansionHubSave.CurrentSaveVersion = 6`,
  `ExpansionQuestSaveEnvelope.CurrentVersion = 1` (+ `MigrationFromVersion = 1`),
  `WeightOfChoicesSave.CurrentSaveVersion = 2`, `RadioSaveCodec.CurrentSaveVersion = 6`
  (+ `MigrationFromVersion = 1`), plus version constants on at least
  `CombatState`(4), `VerdictSave`(4), `DutyRosterSave`(3), `MemorialSave`(1),
  `CampaignDaySave`(1), `DailyBriefingSave`(1), `ShelterAssignmentSave`(1), `PowerGridSave`(1),
  `MedicalWardSave`(1), `MoraleContagionSaveCodec`(1), `SubterraneanSaveCodec`(1),
  `PathogenStrainSaveCodec`(1), `PsyOpsSaveCodec`(1), `ThirdonarySaveEnvelope`(1) and ~20 more
  `CurrentVersion` state constants across Core.
- `Assets/Ashfall.Core/VersionReport.cs:60+` publishes **a curated subset of six** stores
  (`holdfast, year_of_ash, dose_ledger, expansion_hub, expansion_quest, weight_of_choices`) as
  `SaveSchemaVersions`, sourced from the codec constants themselves. The pinned contract test
  asserts exactly these six (`SaveSchemaVersions_ListsEveryVersionedSaveCodec`) and pins the
  inventory at **194 sections = 6 versioned codecs + 188 checksum envelopes**
  (`AllPersistenceFormats_DistinguishesVersionedCodecsAndChecksumEnvelopes`). Consequence the
  plan must state plainly: stores that *do* carry version constants but are not in the curated
  six (e.g. `radio` at codec v6, `combat` at v4, `verdict` at v4) are inventoried by the report
  as "checksum envelope". The taxonomy of "versioned codec" is currently a hand-curated list
  in `VersionReport`, not derived from the codecs or the registry. The contract test comments
  document the established pin-bump practice ("When a new codec with CurrentSaveVersion ships,
  add it here (and to VersionReport.SaveSchemaVersions)"), so extending curation is a
  sanctioned, test-first operation — not an ad-hoc edit.
- The aggregate envelope (`Assets/Ashfall.Core/Save/CampaignSaveEnvelope.cs`,
  `CampaignEnvelopeBuilder.cs`) carries `manifestVersion` with
  `CampaignEnvelopeBuilder.CurrentEnvelopeVersion = 2`; `SaveSlotService` accepts
  `manifestVersion` 1 (migrated in memory via `MigrateToCurrent`, rewritten as current on the
  next successful save — `SaveSlotService.cs:639-644`) and 2 (current), and **refuses anything
  else** with `"Unsupported manifest version: {n}"` (`SaveSlotService.cs:308`, and
  `MigrateToCurrent` throws `"No migration path from envelope manifestVersion {n} to 2"` for
  n≠1). "Future formats are never truncated" is an implemented invariant, not a hope.
- **`SaveManifest` already stamps provenance into every aggregate save**
  (`CampaignSaveEnvelope.cs:14-18`): `manifestVersion`, `gameVersion` ("Game version string
  that wrote this manifest"), and `buildId`. The "which build wrote this save" question is
  answerable today — what is missing is any policy that *reads* it.
- Per-file save resilience exists and is tested: `.bak` rotation on save plus `.corrupt-*`
  quarantine on checksum failure (`src/Main.UiTests.Holdfast.cs:279-296`,
  `Ashfall.Core.Tests/Holdfast/HoldfastTradeSaveStoreTests.cs:88-119`). This is the rollback
  anchor §12 builds on.
- Golden save fixtures exist from Plan 27 (Wave 10 Part 1): `artifacts/golden_saves/` contains
  `early_campaign.json`, `mid_campaign.json`, `late_campaign.json`, `manifest.json`, governed
  by `docs/testing/FIXTURE_POLICY.md` and pinned by `GoldenSaveFixtureTests.cs`. They are all
  **current-version** fixtures; no per-historical-version corpus exists.

## 2.2 Tags, changelog, and release records today

- **Exactly one git tag exists**: `v1.1.0` (annotated tag object, dated 2026-09-07, pointing at
  `a5da264f` "docs(release): 1.1.0 verdict GO — clean-clone gate record"). The source plan's
  "0 tags" premise is **STALE**. The 1.1.0 release record explicitly recommended retroactively
  tagging `v1.0.0` at `main` commit `9b4985d0` "so future diffs are scoped" — **never
  executed** (`git tag -l | wc -l` → 1).
- **94 commits** sit between `v1.1.0` and HEAD, of which **37 touch
  `Assets/StreamingAssets/Data/`**. None of them is chronicled anywhere per-release.
- **`CHANGELOG.md` exists at repo root** (first touched by commit `1426438e`, Plan 76 era) —
  the source plan's "no CHANGELOG anywhere" premise is **STALE**. But its actual discipline is
  thin: the entire file is one `## [Unreleased]` section containing the Plan 76/76.1/76.2/76.3
  series entries, plus a "Notes" block. There is **no `[1.1.0]` section** — the only tagged
  release in repository history has no changelog entry. The header declares "Format: Keep a
  Changelog". Nothing generates it, nothing checks it, nothing links a version bump to it.
- **`docs/releases/RELEASE_1.1.0.md` exists** — a manual release record produced by the
  `ashfall-release-captain` skill checklist: 7 manually-run gates (build, full test suite
  9226/9226, data-integrity, bridge, asset gate, Linux export smoke booting the PCK, Windows
  export blocked by missing templates), a go/no-go verdict table, a hand-written lane
  changelog ("1,567 commits; 125 mention numbered plans, 39 distinct plan streams"), known
  issues carried forward, and one recorded process lesson ("linked worktrees break repo-root
  detection in several meta-gate tests — verify release gates in a true clone"). This file
  establishes the repo's de facto release-record convention: **`docs/releases/` (plural),
  `RELEASE_<version>.md`**. The source plan proposed creating `docs/release/` (singular) —
  this plan corrects that to extend the existing directory (EXTEND over DUPLICATE).
- No `scripts/release/` directory exists. No `scripts/ci/version-gate.sh`,
  `release-gate.sh`, or `export-smoke-boot.sh` exists (verified against the full `scripts/ci/`
  listing). No `docs/archive/releases/` retention directory exists.

## 2.3 CI and gates today

- `docs/ci/CI_GATE_MANIFEST.json`: **53 gates** (`total_gates: 53`, `fast_tier_count: 50`;
  classifications: 50 `fast`, 2 `full` (`test_core_suite`, `export_parity`), 1 `performance`),
  manifest `schema_version: 1.0.0`. The source plan's "46 gates" premise is **STALE** (the
  manifest grew). Gates are executed locally and in CI by the same runner,
  `scripts/ci/run-gates.py`, with a quarantine registry (`scripts/ci/quarantine.json`, 14-day
  expiry, protected Core-invariant gates).
- **The release tier already exists and is empty.** `run-gates.py:263` accepts
  `--tier {fast,full,performance,release,all}` — `"release"` is a pre-provisioned choice with
  **zero gates classified `release`** in the manifest. This is the single cleanest extension
  seam in the whole package (§5).
- `.github/workflows/ci.yml` runs `--tier fast` + the `test_core_suite` full gate on
  push/PR to `main,master`. `.github/workflows/build.yml` exports Linux/X11 and Windows on
  push to `main` using inline `run-godot-bounded.sh --export-release` commands; its
  "Verify export includes data authority" step **cannot fail meaningfully** (a `test -f` on the
  binary plus informational echoes; no boot, no packaged selftest). Meanwhile the repo already
  owns `scripts/ci/export-build.sh`, which does the full deterministic pipeline — preflight,
  dotnet build, import, export via `godot-export-linux.sh` (PCK staging + loose Data
  mirror-deploy + representative-catalog checks), `RELEASE_STAMP.txt`, packaged parity gate,
  and **exported-artifact boot + bridge + data-integrity + research-catalog + parity selftests
  run against the packaged artifact with `ASHFALL_DATA=` cleared** — exactly the "boot the
  shipped artifact" step the source plan asked CI to gain. CI simply does not call it. The
  1.1.0 record proves the underlying capability works (Linux PCK booted and passed
  `--data-integrity-selftest` from inside the PCK).
- No workflow triggers on tags; no workflow knows what a hotfix branch is.

## 2.4 Governance and instruction surfaces today

- **AGENTS.md is clean**: the `bit lane/snap/export` vocabulary the source plan's Task 48A
  step 10 targeted is gone from the canonical rulebook (grep-verified; the foreman rewrite
  landed with Plan 29's Wave 3/10 work), and gate `agent_rulebooks_sync`
  (`sync-agent-rulebooks.py --check`, 13 client rulebooks byte-identical) keeps it gone.
  Surviving `bit` references are confined to historical plan corpus files
  (`C-integration-plans/C2_planintegration[21].md`, `Next-steps-plans/…`), which are historical
  evidence, not active instructions. **Residual live drift (new finding):**
  `.agents/skills/ashfall-release-captain/SKILL.md` line 3 still advertises "lane/snap
  discipline" and line 13 still says "`bit`-style lane discipline here means: branch, commit…".
  The skill is the *operational* release checklist — the drift lives exactly where a releasing
  agent reads.
- **No `ashfall-hotfix-rollback` skill exists** (find-verified; the source plan's evidence row 9
  named two skills). Only `ashfall-release-captain` exists. That premise row is **PARTIALLY
  STALE**.
- Plan 29 outputs are live and are the correct registry seams: `docs/architecture/CLAIMS.json`
  (24 claims, `schema_version: 1.0.0`) + `scripts/ci/verify-capability-claims.py` (validates
  schema, evidence paths, test files, and gate IDs against the CI manifest; a TRUE claim must
  cite source/doc and test/gate evidence), `docs/roadmap/README.md` (numbering policy: <100
  continuity, ≥100 expansion; collision handling routes through the census), and
  `docs/roadmap/WAVE_LEDGER.md` (wave-by-wave executed/proposed history — a *governance*
  chronology, not a player-facing changelog; §6.4 keeps these separate on purpose).
- `docs/CURRENT_AUTHORITY.md` is a documentation map; it contains **no known-issues register**.
  The source plan's "known-issues list generated from `docs/CURRENT_AUTHORITY.md`'s register"
  premise is **STALE** — the actual known-issues surfaces today are `KNOWN_DEBT.md`
  (non-RETIRED rows) and the previous release record's "Known issues carried forward" section.
- Dependency realities vs the source plan's declared depends-on list: **C1[7]/Plan 29 is SEALED**
  (the census DAG edge `C1[7] -> C2[21]` is the satisfied prerequisite). **Plan 39
  (`C2[16]`, session durability/release gate) is NOT executed** (`AUDIT-PENDING`, census line
  59) — the "39A release gate" the source plan defers to does not exist, so this package builds
  `release-gate.sh` itself by composing existing manifest gates, and leaves long-run soak to
  Plan 39. **Plan 46 (`C2[20]`, playable metrics) is NOT executed** — there is no
  `DECISIONS.md`/funnel report to source patch-note balance lines from; existing
  `docs/balance/*.md` (e.g. `BALANCE_SIM_EXPEDITION_DESTINATIONS.md`) are the interim evidence
  base. **Plan 47 (mod/pack contract) IS sealed** (Wave 11 Part 1 A5, 44/44 tests;
  `docs/mods/MOD_CONTRACT.md` exists), so the mod-contract breaking-change feed is real and
  citable.
- Commit-style reality for the changelog generator: of the last 200 commits, **48** match a
  conventional-commit prefix (`feat|fix|chore|docs|test|refactor|perf|ci|build|style|revert`).
  Recent history mixes `feat(scope): …`, `feat: …`, `Plan NNN: …`, `Merge …`, and free-form
  messages. The 1.1.0 record counted "125 mention numbered plans" across 1,567 commits. Any
  generator must be **tolerant by design** (§6.4) — the source plan's "the repo's commit style
  already does `feat(scope): … (Plan NN Task NX)`" is true of a minority stream, not the
  median commit.

## 2.5 Premise audit summary (entry-gate record)

| Source-plan evidence row | Claim @ `ccac926e` | Verdict @ `65357b8a` | Consequence for this plan |
|---|---|---|---|
| 1 | 0 git tags | **STALE** — `v1.1.0` exists (2026-09-07); retro `v1.0.0` recommendation unexecuted | Tag *policy* and tag-triggered CI still missing; baseline-tag question becomes a foreman decision (§19 Phase 4) |
| 2 | Bare version string `1.0.0`, `unknown` fallback | **PARTIALLY STALE** — `project.godot` now `1.1.0`; `unknown` fallback live (`HostCli.cs:774,779`); export presets still `1.0.0` (new drift) | Multi-source equality gate + fallback hardening remain required |
| 3 | No changelog anywhere | **STALE** — root `CHANGELOG.md` exists ([Unreleased] only); `docs/releases/RELEASE_1.1.0.md` exists | Work shifts from "create" to "discipline + generation + link to tags" |
| 4 | Version-report contract is real | **CURRENT** — `VersionReport` + 12-case contract suite, 194-section inventory pinned | Extend; never duplicate |
| 5 | 46 gates, untied to releases, no `release-gate.sh` | **PARTIALLY STALE** — 53 gates now; still no release-scoped gate; `release` tier exists in the runner, empty | Populate the existing tier; add `release-gate.sh` |
| 6 | Export ships unproven in CI | **CURRENT** — `build.yml` cannot fail on a broken artifact; `export-build.sh` (the real pipeline) is not called by CI | Wire CI to the existing script; add release-tier export smoke |
| 7 | Save compatibility across versions unspecified | **CURRENT** — envelope v1→v2 ladder + refusal + `.bak`/quarantine exist; no policy doc, no support-window test, no historical fixtures | The core of 48A/48C remains fully outstanding |
| 8 | AGENTS.md mandates `bit` workflow | **RESOLVED** in AGENTS.md + 13 rulebooks; **residual drift** in `ashfall-release-captain` SKILL.md | Replace step 10 with the skill-file fix + skills-catalog regen |
| 9 | Two skills imply a process nobody runs | **PARTIALLY STALE** — only `ashfall-release-captain` exists; it was executed once (1.1.0) | Update the one real skill; do not reference a nonexistent hotfix skill |
| 10 | Content/data drift untracked per release | **CURRENT** — 94 commits / 37 data-touching since `v1.1.0`, zero release records | Changelog generation + per-release data-schema diff sections |

---

# 3. Required Delta

From current reality to the objective, the minimum delta decomposes into six deltas. Each is
stated as *what must become true*, with the owner artifact named.

**D1 — Version truth.** One written policy (`docs/releases/VERSIONING.md`, new) naming the
three axes (game/data/save), the project's semver mapping (what major/minor/patch *mean* when
"a breaking change is a lost campaign"), the save support window per store, and the data
acceptance rule. One machine enforcement: the game version string lives in exactly one runtime
authority (`project.godot`), is mirrored to `Directory.Build.props` (`<VersionPrefix>`) and the
Windows export preset fields **by script only**, and the three are asserted equal by a Core
contract test so no future bump can repeat the `1.0.0`-vs-`1.1.0` preset drift. `HostCli`
stops printing a soft `unknown`: an absent/invalid version renders as a loud, greppable
`INVALID` marker and a fast gate fails CI if `project.godot`'s value is not strict semver.

**D2 — Drift gates.** A new fast-tier gate (`version_gate`) that fails when: the version
sources disagree; a version bump lands without a CHANGELOG section/bullet in the same change;
a save-schema constant (`CurrentSaveVersion`, `CurrentVersion`, `manifestVersion`,
`MigrationFromVersion`) changes in `Assets/Ashfall.Core/**` without a changelog
migration note; a `schema_version` bump lands in `Assets/StreamingAssets/Data/**` without a
changelog note. This is the "documented promises rot" guardrail made executable.

**D3 — Save compatibility made testable.** A `SaveSupportWindowTests` suite that enumerates the
versioned codecs and the envelope ladder and asserts each one's declared migration floor
actually loads; a committed historical-fixture corpus under the existing
`artifacts/golden_saves/` + `FIXTURE_POLICY.md` seam (one fixture per supported migration
source, starting with envelope `manifestVersion: 1` and the curated six codecs'
`MigrationFromVersion`); and a release-tier fixture-matrix gate that loads every committed
fixture on the candidate build and asserts checksum-stable round-trip. This converts the
support window from prose into the thing a hotfix gate can diff against.

**D4 — Release as one command.** `scripts/release/prepare-release.sh` (with `--check` and
`--dry-run`), `scripts/release/generate_changelog.py` (regenerate + `--check`, per the repo's
generator convention), `scripts/ci/release-gate.sh` (fast + full + release tiers + export
smoke + machine-readable report), CI wiring (`build.yml` adopts `export-build.sh`; new
tag-triggered `release.yml`), and retention under the existing `docs/releases/` +
`artifacts/` conventions. The 1.1.0 manual flow (7 gates by hand, changelog by hand, verdict by
hand) becomes the scripted flow with the same checklist content.

**D5 — Hotfix path.** `docs/releases/HOTFIX.md` (classification: `patch` / `data` /
`save-affecting` / `breaking`; the iron rule "a hotfix may never require a migration — if it
must, it isn't a hotfix"), `scripts/release/hotfix.sh` (branch-from-tag, classify, check),
a hotfix CI workflow that runs the release-line fixtures (the tag's fixtures, not `main`'s),
a rollback procedure anchored on the existing `.bak` rotation + envelope migration refusal
semantics, a data-only hotfix route statement built on the sealed `ASHFALL_DATA`/MOD_CONTRACT
machinery, a support-triage kit doc assembling reports that already exist (`--version`,
manifest `gameVersion`/`buildId`, Plan 31C day-record), and a post-mortem template whose
required field is "which gate would have caught this".

**D6 — Instruction/registry hygiene.** Fix the residual `bit`-era vocabulary in
`.agents/skills/ashfall-release-captain/SKILL.md` (and regenerate the skills catalog through
its generator), register the new release-craft claims in `docs/architecture/CLAIMS.json` (the
existing verifier validates them), bump the CI manifest's `schema_version` with the additive
gates, and record the package in the governance ledgers (integrator-owned paths only).

---

# 4. Evidence

## 4.0 Numbering collision — read before citing "Plan 48"

There are **two unrelated Plan 48s** in this repository:

1. **Weather/route gates** — `piagentsplans/48-weather-route-gates.md` (historical evidence
   backlog), implemented and closed as `docs/weather/PLAN_48_WEATHER_ROUTE_GATES_CLOSEOUT.md`
   (plus GAP-48A/GAP-48B follow-ups already chronicled in `CHANGELOG.md`'s [Unreleased]
   section: "Plan 76.3 — destination-level seams sealed (GAP-48A / GAP-49B)"). **Done.
   Unrelated to this package.**
2. **Release craft** — `Next-steps-plans/shipped_to_chat/Plan_48_Release_Craft_Versioning_Changelog_Hotfix.md`
   = census row `C2[21]`. **This document plans that one.**

The collision is structural, not accidental: `docs/roadmap/README.md` §2 assigns `<100` to
continuity plans and `>= 100` to expansion plans, while `piagentsplans/00–129` is a historical
backlog occupying the same <100 space — 42 corpus baselines share a number with a closeout
filename and only 3 survive subject matching (census §1 finding). The census already flags
this exact pair with its standing formula (line 64):

> `C2[21]` | Plan 48 — Release Craft: Versions, Tags, Changelog, and the Hotfix Path | … |
> closeout exists for Plan 48 but subject differs → numbering collision
> (`docs/weather/PLAN_48_WEATHER_ROUTE_GATES_CLOSEOUT.md`)

**Citation rule for all artifacts of this package** (commit messages, changelog entries, gate
IDs, claims, ledger rows): always write `C2[21]` or "Plan 48 (Release Craft)". Never write a
bare "Plan 48" in a new file. Any future closeout for this package must be named
`PLAN_48_RELEASE_CRAFT_CLOSEOUT.md`-style (subject-qualified), exactly as this plan file is
named `PLAN_48_RELEASE_CRAFT_INTEGRATION_PLAN.md`, so the next census sweep can subject-match
without archaeology.

## 4.1 Primary citations (all re-verified 2026-09-19 @ `65357b8a`)

| Claim | Evidence |
|---|---|
| Game version runtime authority | `project.godot:15` → `config/version="1.1.0"` |
| Silent `unknown` fallback live | `src/Host/HostCli.cs:774` (`string gameVersion = "unknown";`), `:779` (re-assignment) |
| Stale duplicate version fields | `export_presets.cfg` preset.1: `application/file_version="1.0.0"`, `application/product_version="1.0.0"` |
| No assembly version | `Directory.Build.props` (no `Version*` property); `Ashfall.csproj` (no `Version*`) |
| Version report contract | `Assets/Ashfall.Core/VersionReport.cs` (`Compose`, `SaveSchemaVersions`, `ScanDataSchemas`, `FormatPersistenceInventory`); `Ashfall.Core.Tests/VersionReportContractTests.cs` (12 cases; pins 6 versioned / 188 envelope / 194 total) |
| Curated-six taxonomy gap | `VersionReport.SaveSchemaVersions` lists 6 stores; `RadioSaveCodec.CurrentSaveVersion = 6` (`Radio/RadioSave.cs:220`) is *not* among them and the `radio` section inventories as "envelope" |
| Codec versions (current) | `HoldfastSave.cs:24` v5; `YearOfAshSave.cs:21` v5; `DoseLedgerSave.cs:26-27` v2 from 1; `ExpansionHubSave.cs:30` v6; `ExpansionQuestSave.cs:14-15` v1 from 1; `WeightOfChoicesSave.cs:23` v2 |
| Envelope ladder + refusal | `CampaignEnvelopeBuilder.cs:26` `CurrentEnvelopeVersion = 2`; `SaveSlotService.cs:303-309` (accept 1-migrate, 2-current, else "Unsupported manifest version"), `:639-644` (in-memory migrate, rewrite on next save), `:775-777` (throws for n≠1) |
| Save provenance stamp exists | `CampaignSaveEnvelope.cs:14-18` (`manifestVersion`, `gameVersion`, `buildId`) |
| `.bak` + quarantine machinery | `src/Main.UiTests.Holdfast.cs:279-296`; `Ashfall.Core.Tests/Holdfast/HoldfastTradeSaveStoreTests.cs:88-119` |
| One tag | `git tag -l` → `v1.1.0` (annotated; `git cat-file -t v1.1.0` → `tag`; date 2026-09-07; target `a5da264f`) |
| Retro-tag recommendation unexecuted | `docs/releases/RELEASE_1.1.0.md` Phase 3: "Recommended: also tag `v1.0.0` retroactively at `main` (9b4985d0)"; tag list has no `v1.0.0` |
| Churn since tag | `git log v1.1.0..HEAD --oneline` → 94 commits; `-- Assets/StreamingAssets/Data` → 37 |
| CHANGELOG exists, release-blind | `CHANGELOG.md` (whole file: header + `## [Unreleased]` Plan 76 series + Notes; zero `## [x.y.z]` sections) |
| Release-record convention | `docs/releases/RELEASE_1.1.0.md` (7-gate manual record, verdict, lane changelog, known issues, worktree lesson) |
| Gate manifest | `docs/ci/CI_GATE_MANIFEST.json`: `total_gates: 53`, `fast_tier_count: 50`, `schema_version: "1.0.0"`; classifications fast×50/full×2/performance×1 |
| Empty release tier pre-provisioned | `scripts/ci/run-gates.py:263` `--tier choices=["fast","full","performance","release","all"]`; zero manifest gates carry `classification: "release"` |
| CI wiring | `.github/workflows/ci.yml` (fast tier + `test_core_suite`); `.github/workflows/build.yml` (inline exports; verification step is `test -f` + echoes; never boots) |
| Real export pipeline un-wired | `scripts/ci/export-build.sh` (preflight → build → import → export via `godot-export-linux.sh` → `RELEASE_STAMP.txt` → packaged parity → exported-artifact boot/bridge/data-integrity/research/parity selftests); `scripts/ci/godot-export-linux.sh` (PCK staging + loose Data mirror-deploy + representative catalog checks) |
| Fixture seam | `artifacts/golden_saves/{early,mid,late}_campaign.json + manifest.json`; `docs/testing/FIXTURE_POLICY.md`; `Ashfall.Core.Tests/Save/GoldenSaveFixtureTests.cs` |
| Claims registry | `docs/architecture/CLAIMS.json` (24 claims, `schema_version: "1.0.0"`); `scripts/ci/verify-capability-claims.py` (statuses, evidence-path and gate-ID validation) |
| Wave ledger | `docs/roadmap/WAVE_LEDGER.md` (Waves 1–10; governance chronology) |
| Numbering policy + collision rule | `docs/roadmap/README.md` §2; `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` line 64 + §1 (42 collisions, 3 subject matches) |
| Unblock basis | `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md` row 8 + census DAG edge `C1[7] -> C2[21]` (line 174) |
| Mod contract (Plan 47, sealed) | `docs/mods/MOD_CONTRACT.md`; Wave 11 Part 1 closeout (44/44) |
| Residual `bit` drift | `.agents/skills/ashfall-release-captain/SKILL.md:3` ("lane/snap discipline"), `:13` ("`bit`-style lane discipline"); AGENTS.md itself grep-clean; gate `agent_rulebooks_sync` active |
| No hotfix skill | `find . -iname "*hotfix*"` → only the source plan document itself |
| Commit-style distribution | `git log --oneline -200`: 48 conventional-prefixed; `RELEASE_1.1.0.md`: "125 mention numbered plans" of 1,567 |
| Data schema ceiling (observed) | `VersionReport` doc example "v1: 401, v2: 2 (max v2)"; live floor pinned by `ScanDataSchemas_LiveDataAuthority_IsNonTrivialAndCurrent` (≥100/≥100/max≥1) |
| Content-utilization baseline | `artifacts/content-utilization-baseline.json` exists (consumed by `--content-utilization-selftest`) |
| Test policy constraints | `TEST_POLICY.md` (focused targets; full-suite/release checks need explicit reason + dedicated window; `scripts/run_test.sh` 180s cap) |
| Ownership constraints | `WORKTREE_OWNERSHIP.md` active claims: `claim-xp-wave1-difficulty-2026-09-18`, `claim-wave11-part2-execution-2026-09-18` (both hold `WORKTREE_OWNERSHIP.md`/`INTEGRATION_PLANS.md`; the latter also holds `docs/plans/wave11_part2/` and endgame paths) — no overlap with this package's paths |

---

# 5. Existing Extension Seams

Every piece of this package attaches to an existing seam. None requires a new parallel
registry, ledger, save store, or manager (AGENTS.md rule 5).

1. **Empty `release` classification tier in the gate runner** — `run-gates.py:263` already
   accepts `--tier release`; the manifest simply has no such gates. Adding
   `classification: "release"` gates to `CI_GATE_MANIFEST.json` is the designed-for extension;
   no runner changes.
2. **`docs/releases/RELEASE_<version>.md` convention** — established by the 1.1.0 record and
   named as the `ashfall-release-captain` skill's OUTPUT. Retention, patch-notes template,
   hotfix playbook, and versioning policy all live here (correcting the source plan's
   singular `docs/release/`).
3. **`VersionReport` + `VersionReportContractTests`** — the pinned report is the single
   rendering authority for all three version axes. The plan extends its *consumers* (gates,
   release report, support kit) and, only via the sanctioned pin-bump practice documented in
   the test comments, its curated codec list. It does not add a second version report.
4. **`SaveManifest.gameVersion` / `buildId`** — every aggregate save already records its
   writer. The support kit and the hotfix triage read this; nothing new is stamped.
5. **Envelope migration ladder + refusal semantics in `SaveSlotService`** — the save support
   window is *derived from the code that already enforces it* (`manifestVersion` 1 migrates,
   2 current, others refused; future never truncated). The policy document cites the code;
   the window test enumerates it.
6. **`artifacts/golden_saves/` + `FIXTURE_POLICY.md` + `GoldenSaveFixtureTests`** — the
   historical-fixture corpus is a new *subdirectory class* under the existing fixture
   authority (`artifacts/golden_saves/historical/…` with manifest entries), not a new fixture
   system. Category rules (authority-backed vs explicit synthetic) already govern it.
7. **`scripts/ci/export-build.sh` + `godot-export-linux.sh`** — the export+smoke authority.
   Release gating *calls* it; CI `build.yml` is re-pointed at it. No second exporter.
8. **Generator `--check` convention** — `generate-docs-index.py`, `generate-architecture-map.py`,
   `generate-port-contract.py`, `sync-agent-rulebooks.py`, etc. all ship "regenerate /
   `--check` verify" pairs registered as drift gates. The changelog generator follows this
   exact pattern (`generate_changelog.py` + `--check`), including the repo rule "do not
   modify generated outputs by hand" (enforced via generated-region markers, §6.4).
9. **`docs/architecture/CLAIMS.json` + `verify-capability-claims.py`** — release-craft
   capability claims (version single-source, changelog gated, support window tested, hotfix
   never migrates) are added as ordinary claims; the existing verifier checks their evidence
   paths and gate IDs against the manifest in the same commit (ordering matters — §19).
10. **`ashfall-release-captain` skill** — the operational checklist. It is *edited* (vocabulary
    + point at scripts) and the skills catalog is regenerated through
    `generate-agent-skills-catalog.py --check`; no new skill is authored.
11. **Quarantine registry** (`scripts/ci/quarantine.json`) — the pre-existing, policy-bound
    escape hatch (14-day expiry, protected gates) if a new gate flakes. Rollback §23 uses it.
12. **`scripts/run_test.sh`** — focused xUnit execution with the 180s cap; all new test files
    run through it alone first, per policy.

---

# 6. Proposed Architecture

## 6.1 The three-axis version model (policy content of `VERSIONING.md`)

The policy page names three axes and states, for each, where the value lives, who may change
it, and what "compatible" means. Nothing here invents new state; it *names* existing state.

| Axis | Value lives in | Changed by | "Compatible" means |
|---|---|---|---|
| **game** (product semver `MAJOR.MINOR.PATCH`) | `project.godot` `application/config/version` (runtime authority); mirrored to `Directory.Build.props` `<VersionPrefix>` and `export_presets.cfg` Windows `file_version`/`product_version` by script only | `prepare-release.sh` / `hotfix.sh` (never hand-edited piecemeal — the version gate fails on partial bumps) | A save written by game version X loads on Y per the save axis rules; see mapping below |
| **data** (per-catalog `schema_version` int) | Each `Assets/StreamingAssets/Data/*.json` top-level field; inventoried live by `VersionReport.ScanDataSchemas` | Content packages, with a changelog note (gated) | Build B accepts catalogs whose `schema_version` ≤ the max B's shipped loaders understand; a catalog *newer* than the build is rejected with a message naming the file |
| **save** (per-store codec version + envelope `manifestVersion`) | Codec constants (`CurrentSaveVersion` / `MigrationFromVersion`), `CampaignEnvelopeBuilder.CurrentEnvelopeVersion` | Core packages with migration + changelog migration note (gated) | Store loads iff its on-disk version ∈ [migration floor … current]; envelope loads iff `manifestVersion` ∈ {1 (migrates), 2 (current)}; future versions are refused, never truncated |

**Semver mapping for this project** (written into `VERSIONING.md` — the project's "breaking
change" is a lost campaign, so the mapping is spelled out):

- **MAJOR**: any change that makes a previously loadable save unloadable (envelope
  `manifestVersion` bump *without* an in-place migration; dropping a codec migration source;
  a data-schema change that rejects catalogs a shipped build accepted — i.e. a MOD_CONTRACT
  breaking change per `docs/mods/MOD_CONTRACT.md`); removal of any published CLI/route
  contract.
- **MINOR**: new content/system reachability; *additive* save-schema movement (codec
  `CurrentSaveVersion` bump **with** migration from every previously supported version; new
  save section); additive data `schema_version` bump.
- **PATCH**: none of the above. Zero save-schema, zero data-schema, zero contract change.
  This is the hotfix class, and §12's enforcement keys off exactly this definition.

## 6.2 Version authority and the "never unknown" rule

- `project.godot` stays the single runtime source (Godot bakes it into exports; changing that
  would fight the engine for no gain). `Directory.Build.props` gains one property,
  `<VersionPrefix>`, so `dotnet` artifacts carry the same value; `export_presets.cfg`'s two
  Windows fields are the third write site. **One script owns all three writes**
  (`scripts/release/set_version.py`, invoked by `prepare-release.sh`/`hotfix.sh`), and one
  Core test owns the equality assertion.
- New engine-free Core helper `Assets/Ashfall.Core/ReleaseVersion.cs` (≈80 lines, zero
  dependencies, `netstandard2.1`-legal): `TryParse(string, out ReleaseVersionValue)` (strict
  `X.Y.Z` + optional `-suffix` rejected on purpose), `Format`, and `ClassifyBump(old, new) →
  Major|Minor|Patch|Invalid`. No Godot, no `System.Random`, no I/O beyond the text the caller
  hands it — Rule 2 preserved; the test project reads the repo files as *text* using the same
  repo-root discovery `VersionReportContractTests.FindRepoRoot()` already uses.
- `HostCli.PrintVersion` (bounded host change, 4 lines): if the project setting is missing,
  empty, or fails `ReleaseVersion.TryParse`, render `game : INVALID (config/version missing or
  not semver)` instead of `unknown`. An exported build with a broken version now fails loudly
  in every log and in the version gate's exported-artifact check, instead of minting an
  unanswerable support ticket. The render shape stays otherwise byte-identical, so the pinned
  `VersionReportContractTests` lines are untouched (they pass explicit strings; the
  `unknown`→`INVALID` path is additive and covered by a new case in the *new* test file).

## 6.3 Gates

All gates are registered in `docs/ci/CI_GATE_MANIFEST.json` (additive; manifest
`schema_version` 1.0.0 → 1.1.0 with the change noted in the manifest's own changelog
convention — a one-line header note, as the manifest has no per-entry history).

**Fast tier (run on every push/PR, local parity via `verify-fast.sh`):**

- `version_gate` → `python3 scripts/ci/version-gate.py`
  - *Sources agree:* `project.godot` == `Directory.Build.props` `VersionPrefix` ==
    `export_presets.cfg` Windows fields; value is strict semver.
  - *Bump-changelog link:* within the change range (`VERSION_GATE_BASE` env, default
    merge-base vs `main`, fallback `HEAD~1`), if the game version changed, `CHANGELOG.md`
    changed too and contains either an `[Unreleased]` bullet or a `## [X.Y.Z]` section
    matching the new value.
  - *Save-schema discipline:* if the range touches `CurrentSaveVersion|CurrentVersion
    =|manifestVersion|MigrationFromVersion` in `Assets/Ashfall.Core/**`, the changelog carries
    a migration note (matched by section header `Save & data compatibility` or an
    `[Unreleased]` bullet containing `migration`/`schema`).
  - *Data-schema discipline:* same range rule for `schema_version` in
    `Assets/StreamingAssets/Data/**`.
  - *Self-proof:* `--self-test` builds a temp fixture tree (bump without changelog → must
    fail; codec constant touch without note → must fail; clean change → must pass), the same
    failure-test pattern `verify-capability-claims.py` documents. On `main` after merge the
    range is empty and the gate passes vacuously — documented, deliberate.
- `changelog_drift` → `python3 scripts/release/generate_changelog.py --check` — the generated
  regions of `CHANGELOG.md` match regeneration from git history (§6.4).

**Full tier (dedicated windows only, per TEST_POLICY):**

- `save_support_window` → `dotnet test … --filter SaveSupportWindowTests` — enumerates every
  versioned codec (curated six + every Core type exposing `CurrentSaveVersion`/
  `MigrationFromVersion`) and the envelope ladder; asserts each declared migration floor
  actually migrates and each future version is refused. Bounded; one file; ~30 cases.

**Release tier (the pre-provisioned empty tier; run by `release-gate.sh` and `release.yml`):**

- `release_fixture_matrix` → host-side: new bounded selftest `--release-fixture-selftest`
  loads every committed fixture (golden + historical corpus), re-saves in memory, and asserts
  checksum-stable round-trip per fixture; prints one line per fixture. Registered through the
  existing `HostCliRegistry` selftest seam (the `selftest_manifest_drift` generator picks it
  up; regenerate, don't hand-edit).
- `export_smoke_boot` → `bash scripts/ci/export-build.sh` (the existing full pipeline:
  export, stamp, packaged parity, exported-artifact boot + selftests with `ASHFALL_DATA=`
  cleared). The release tier simply *calls the authority*.

**`scripts/ci/release-gate.sh`** (new, thin): runs `verify-fast.sh`, `run-gates.py --tier
full`, `run-gates.py --tier release`, then `export-build.sh`; aggregates the per-tier
`--report-json` outputs plus `--version` output and the content-utilization snapshot into
`build/reports/release-gate-report.json` (machine) and prints the verdict table (human). This
is the "signed gate report" of 48B step 6 — a release without this artifact does not exist.

## 6.4 Changelog: generation with a human margin

`CHANGELOG.md` remains the single player/product-facing chronology (Keep a Changelog format,
already declared in its header). `docs/roadmap/WAVE_LEDGER.md` remains the *governance*
chronology (wave status, claims, acceptance). They are **not merged**: the ledger answers
"what was authorized and sealed when"; the changelog answers "what changed for a player
between builds". The generator cross-links (plan/task IDs it extracts get emitted as plain
text; no coupling).

`scripts/release/generate_changelog.py` (Python, mirroring the header/usage-docstring style
of `generate-docs-index.py`):

- **Source of truth:** `git log <base>..<head>` with `--no-merges` (merge commits contribute
  their subjects only when nothing else exists in range).
- **Mapping:** `feat*`→`### Added`; `fix*`→`### Fixed`; `perf*`→`### Performance`;
  `docs|chore|ci|build|refactor|test|style|revert`→`### Maintenance`; non-conventional
  subjects (the majority today — 48/200 conventional) → `### Other changes` verbatim. Plan
  references (`Plan N+`, `C2[..]`, task IDs) are extracted and appended per entry as
  `(refs: …)`. Scope tags like `feat(release):` are preserved.
- **Release-critical diffs always surface:** the generator diffs, between base and head, (a)
  save-schema constants (same grep class as the version gate), (b) data `schema_version`
  values, (c) `docs/mods/MOD_CONTRACT.md`, and renders a `### Save & data compatibility`
  subsection from the *diff result* (not from prose), so a release's compatibility statement
  is computed. Until Plan 46 lands, balance lines cite `docs/balance/*.md` files touched in
  range by name.
- **Generated-region markers:** each `## [X.Y.Z] — date` section wraps machine content in
  `<!-- generated:begin … -->` / `<!-- generated:end -->`. Human curation (a `### Highlights`
  block, known-issues notes) lives **outside** the markers and survives regeneration — this
  is how the repo rule "never hand-edit generated output" coexists with "patch notes are
  human prose". `--check` regenerates marked regions and diffs; drift fails the
  `changelog_drift` gate.
- **`[Unreleased]` stays hand-written between releases** (current practice, already working);
  at release cut, `prepare-release.sh` freezes `[Unreleased]` content into the new section's
  human region and regenerates the machine region around it.
- **Backfill:** one-time generation of the missing `## [1.1.0] — 2026-09-07` section from
  `9b4985d0..v1.1.0` (or repo-root..v1.1.0 if the foreman declines the retro-tag), reviewed by
  a human before commit. This is the only generation that targets the past.

## 6.5 Release flow (`prepare-release.sh`)

Modes: `--check` (read-only verdict), `--dry-run` (full run, no tag, artifacts to
`build/release-dry-run/`), and the real run. The real run, in order: (1) preflight — true
working tree state, `core.ignorecase=false` (the 1.1.0 skill rule), not a detached HEAD, no
existing tag `vX.Y.Z`; (2) `set_version.py X.Y.Z` — writes all three version sites; (3)
changelog freeze + regenerate; (4) `release-gate.sh` — must pass; (5) write
`docs/releases/RELEASE_X.Y.Z.md` from `docs/releases/TEMPLATE.md` with the gate report
embedded and `artifacts/release/X.Y.Z/report.json` (version report, gate JSON, fixture matrix
results, content-utilization snapshot — the "self-describing artifact manifest"); (6) create
the **annotated** tag locally; (7) print the exact push commands for a human (pushing is
never scripted — see §17). `--check` performs 1–4 read-only and fails on: dirty tree without
an explicit `--allow-dirty` paths file (the 1.1.0 in-flight-streams lesson), missing changelog
section, red gate, missing fixture, or un-booted export.

## 6.6 Hotfix flow

`docs/releases/HOTFIX.md` owns the playbook; `scripts/release/hotfix.sh` owns the mechanics;
`.github/workflows/hotfix.yml` owns CI on `hotfix/*` branches. Classification (required in the
PR title as `hotfix(patch):` / `hotfix(data):` / `hotfix(save):` / `breaking:` — `breaking` is
refused on a hotfix branch by definition): **patch** = no schema change of any axis; **data**
= catalog value fix, `schema_version` unchanged; **save-affecting** = touches code on a
save/load path but no version constant and no DTO shape change (allowed, but triggers the
full fixture matrix); **breaking** = any version-constant or DTO-shape movement → not a
hotfix, must become a minor/major release. Enforcement: `version-gate.py --hotfix
--base vX.Y.Z` diffs the branch against its release tag and fails on any constant/DTO-shape
movement under Core save paths or any `schema_version` movement under Data; the release-tier
fixture matrix runs **against the tag's fixtures** (the release line's save corpus, not
`main`'s — the source plan's "testing the wrong tree" failure mode). Version discipline under
pressure: `hotfix.sh` bumps patch only, generates the changelog entry, re-runs the release
gate, tags `vX.Y.(Z+1)`, and hands back-merge instructions to a human. Rollback and the
data-only route are specified in §12.

## 6.7 What is deliberately not built

No new registries: the support window is derived from codec constants + registry, the
changelog from git, the release report from the manifest runner, claims live in CLAIMS.json,
fixtures live under the existing fixture policy. No Unity anything. No runtime gameplay state
of any kind.

---

# 7. Ownership Matrix

Checked against `WORKTREE_OWNERSHIP.md` active claims (`claim-xp-wave1-difficulty-2026-09-18`;
`claim-wave11-part2-execution-2026-09-18`): **zero path overlap**. Both active claims hold the
governance ledgers (`WORKTREE_OWNERSHIP.md`, `INTEGRATION_PLANS.md`), so all ledger edits in
this package route through the foreman/integrator — the builder never writes them.

| Artifact group | Paths | Owner during package | Notes |
|---|---|---|---|
| Release docs (new) | `docs/releases/VERSIONING.md`, `docs/releases/PROCESS.md`, `docs/releases/HOTFIX.md`, `docs/releases/TEMPLATE.md`, `docs/releases/SUPPORT.md`, `docs/releases/POSTMORTEM_TEMPLATE.md` | Builder (new claim `claim-c2-21-release-craft-…`) | Extends the existing `docs/releases/` directory; no `docs/release/` (singular) is created |
| Release scripts (new) | `scripts/release/prepare-release.sh`, `scripts/release/generate_changelog.py`, `scripts/release/hotfix.sh`, `scripts/release/set_version.py` | Builder | New directory `scripts/release/` |
| Gate script + manifest | `scripts/ci/version-gate.py` (new); `docs/ci/CI_GATE_MANIFEST.json` (additive entries + `schema_version` bump) | Builder | Runner untouched |
| Core (new, engine-free) | `Assets/Ashfall.Core/ReleaseVersion.cs` | Builder | Rule 2: no engine refs |
| Core (existing, read-mostly) | `Assets/Ashfall.Core/VersionReport.cs` | Integrator-only if touched | Only change contemplated: curated codec list extension, via the sanctioned pin-bump practice with test update in the same commit; default plan leaves it untouched |
| Host (bounded) | `src/Host/HostCli.cs` (`PrintVersion` fallback hardening); `src/Host/HostCliRegistry.cs` (one selftest descriptor) | Builder | 4-line change + 1 descriptor; no panel/UI work |
| Version write sites | `project.godot`, `Directory.Build.props`, `export_presets.cfg` | Script-owned after Phase 1; hand edits become gate failures | Initial sync (presets 1.0.0→1.1.0) is a Phase 1 fix commit |
| Tests (new) | `Ashfall.Core.Tests/Release/ReleaseVersionContractTests.cs`; `Ashfall.Core.Tests/Save/SaveSupportWindowTests.cs` | Builder | New files run alone first (TEST_POLICY) |
| Fixtures | `artifacts/golden_saves/historical/` + `manifest.json` entries | Builder | Under existing `FIXTURE_POLICY.md` |
| Changelog | `CHANGELOG.md` (backfill `[1.1.0]`; markers) | Builder (content), human-reviewed | Generated regions script-owned thereafter |
| CI workflows | `.github/workflows/build.yml` (adopt `export-build.sh`); `.github/workflows/release.yml` (new); `.github/workflows/hotfix.yml` (new) | Builder | `ci.yml` untouched (new fast gates flow through the manifest automatically) |
| Claims registry | `docs/architecture/CLAIMS.json` (additive claims) | Builder | Verifier gate must pass in the same commit |
| Skill | `.agents/skills/ashfall-release-captain/SKILL.md` (vocabulary + script pointers); regenerate `AGENT_SKILLS_INDEX.md` via generator | Builder | Generator output never hand-edited |
| Generated indexes | `docs/INDEX.md` | Regenerated via `generate-docs-index.py` at package close | Convention from prior packages |
| Governance ledgers | `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `KNOWN_DEBT.md`, `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` (status), `docs/roadmap/WAVE_LEDGER.md` (wave row) | **Foreman / integrator only** | Builder supplies the exact rows/diffs; never edits directly |
| Explicitly untouched | Active-claim paths (difficulty Core/data/tests; endgame completion-history set; `docs/plans/wave11_part2/`); all gameplay Core systems; all Data catalogs; all save codecs | — | Collision check result: disjoint |

---

# 8. Data Flow

Release cut (happy path):

```
human: prepare-release.sh 2.0.0 --dry-run
  ├─ set_version.py ── writes ──► project.godot · Directory.Build.props · export_presets.cfg
  ├─ generate_changelog.py ── reads ──► git log v1.1.0..HEAD (+ save/data/mod diffs)
  │                          ── writes ──► CHANGELOG.md [2.0.0] generated region
  ├─ release-gate.sh
  │    ├─ verify-fast.sh ──► run-gates.py --tier fast        (incl. version_gate, changelog_drift)
  │    ├─ run-gates.py --tier full                          (incl. save_support_window)
  │    ├─ run-gates.py --tier release                       (release_fixture_matrix, export_smoke_boot)
  │    │                                 └─ export-build.sh ─► builds/linux/* + RELEASE_STAMP.txt + packaged selftests
  │    └─ --version + content-utilization snapshot
  │                          ── writes ──► build/reports/release-gate-report.json
  ├─ TEMPLATE.md ──► docs/releases/RELEASE_2.0.0.md  (verdict table, changelog, known issues)
  ├─ artifacts/release/2.0.0/report.json             (machine manifest)
  └─ git tag -a v2.0.0   (local only; push commands printed for a human)
```

Hotfix flow:

```
bug on release line v2.0.0
  └─ hotfix.sh v2.0.0 --class patch
       ├─ branch hotfix/2.0.1-<slug> from tag v2.0.0
       ├─ (human lands the fix; PR titled hotfix(patch): …)
       ├─ version-gate.py --hotfix --base v2.0.0   (refuses schema movement)
       ├─ release_fixture_matrix against TAG fixtures (checksum-stable)
       ├─ set_version.py 2.0.1 · generate_changelog.py · release-gate.sh
       ├─ tag v2.0.1 (local) → human pushes, human back-merges to main
       └─ record row appended to docs/releases/RELEASE_2.0.1.md + post-mortem if Sev-1
```

Support flow (triage kit): player runs shipped binary `--version` → `VersionReport.Compose`
output (game/data/save axes) + save file header (`manifest.gameVersion`, `manifest.buildId`,
`manifestVersion`) + optional day-record JSONL (`ASHFALL_DAY_RECORD=1`, Plan 31C — already
privacy-shaped: route strings + durations, no free text). `docs/releases/SUPPORT.md` lists
exactly these four artifacts and the redaction rule; nothing new is collected.

---

# 9. State Model

This package introduces **no runtime game state** — no save section, no Core state type, no
campaign mutation, no RNG. The only state machines involved are repository-side artifacts,
each with a single owner:

- **Version triple** (`project.godot`, `Directory.Build.props`, `export_presets.cfg`): one
  logical value, three write sites, one writer (`set_version.py`), one reader-gate
  (`version_gate`), one test pin (`ReleaseVersionContractTests`). Illegal states: any
  divergence; non-semver value; both are CI-fatal.
- **CHANGELOG.md sections**: `[Unreleased]` (hand-written, append-only between releases) →
  frozen into `## [X.Y.Z] — date` at cut (generated region machine-owned, human region
  preserved). Illegal states: release section without matching tag after cut; generated-region
  drift (both gated).
- **Tag set**: append-only; annotated tags only; `v*` pattern; never moved, never force-pushed
  past (repo-history rule, stated in PROCESS.md, socialized through the skill).
- **Fixture corpus**: `artifacts/golden_saves/` (current-version, existing) +
  `historical/` (one per supported migration source). Each fixture immutable once committed —
  a fixture that needs "updating" is actually a *new* fixture plus a window-policy edit;
  stated in FIXTURE_POLICY addendum.
- **Gate manifest**: additive entries only; `schema_version` bump documented; quarantine only
  via `quarantine.json` policy (14-day, non-protected).
- **Claims**: new rows `TRUE` only with gate+test evidence present in the same commit
  (verifier enforces).

---

# 10. API/Contracts

**`ReleaseVersion` (new, Core, engine-free):**

```csharp
namespace Ashfall.Core
{
    public enum ReleaseBumpKind { Invalid, Patch, Minor, Major }

    /// Strict semver X.Y.Z for the GAME axis only. Data/save axes are ints
    /// owned by their codecs/catalogs and are out of scope here.
    public static class ReleaseVersion
    {
        public static bool TryParse(string text, out string normalized); // "1.2.3" ok; "1.2", "v1.2.3", "1.2.3.4", "" fail
        public static ReleaseBumpKind ClassifyBump(string oldVersion, string newVersion);
    }
}
```

Public-contract surface added to Core: exactly these two methods. Nothing in Core reads
files, Godot, or environment variables.

**`version-gate.py` CLI contract:** exit 0 pass / 1 fail with per-rule one-line diagnostics;
`--base <ref>` (default merge-base `main`, fallback `HEAD~1`), `--hotfix` (schema-movement
refusal mode), `--self-test` (fixture-tree proof). Deterministic: same refs → same verdict,
no wall-clock reads.

**`generate_changelog.py` CLI contract:** default mode regenerates marked regions for a given
`--version`/`--base`; `--check` verifies without writing; `--print` emits to stdout for review.
Date is a required argument on cut (`--date YYYY-MM-DD`) — never `datetime.now()` inside
generated content (determinism, §13).

**`prepare-release.sh` / `hotfix.sh` contracts:** `--check` never writes; `--dry-run` writes
only under `build/`; real mode's only out-of-tree effects are the three version files,
`CHANGELOG.md`, `docs/releases/RELEASE_X.Y.Z.md`, `artifacts/release/X.Y.Z/`, and the local
annotated tag. Push is always printed, never executed.

**Gate registrations (manifest additions):** `version_gate` (fast, critical), `changelog_drift`
(fast), `save_support_window` (full), `release_fixture_matrix` (release), `export_smoke_boot`
(release). Each carries `command`, `timeout_seconds`, `expected_summary`, `classification`,
matching the existing entry schema.

**Claim additions (CLAIMS.json):** `rel_version_single_source` (gate `version_gate` + test
`ReleaseVersionContractTests`), `rel_changelog_gated` (gates `changelog_drift`, `version_gate`),
`rel_save_support_window_tested` (test `SaveSupportWindowTests` + gate `release_fixture_matrix`),
`rel_hotfix_never_migrates` (gate `version_gate --hotfix` + `HOTFIX.md` doc), each with
`source`/`doc`/`test`/`gate` evidence arrays per the existing claim schema.

---

# 11. Data Changes

**No game data changes.** `Assets/StreamingAssets/Data/` is untouched — no new catalogs, no
`schema_version` bumps, no content edits. (This package *gates* future data changes; it makes
none itself.)

Repository-metadata changes only:

- `docs/ci/CI_GATE_MANIFEST.json`: +5 gate entries; `total_gates` 53→58, `fast_tier_count`
  50→52; header `schema_version` "1.0.0"→"1.1.0" with a one-line change note.
- `docs/architecture/CLAIMS.json`: +4 claim rows; `total_claims` 24→28; `verified_at` updated
  by the verifying run.
- `artifacts/golden_saves/manifest.json`: +historical fixture entries (hashes per the existing
  manifest format), plus the new `historical/` files themselves.
- `CHANGELOG.md`: `[1.1.0]` backfill + marker scaffolding — content generated from git
  history, human-reviewed.

---

# 12. Save/Load

This section is the package's center of gravity: the hotfix path exists to protect saved
campaigns, and every rule here keys off machinery that already exists.

## 12.1 The compatibility ladder as-implemented (the policy's factual basis)

- **Envelope axis:** `manifestVersion` 2 is current; 1 migrates in memory
  (`SaveSlotService.MigrateToCurrent`, `SaveSlotService.cs:770+`) and the on-disk file is
  rewritten as current on the next successful save (`:639-644`); any other value is refused
  with `Unsupported manifest version` (`:308`) and `MigrateToCurrent` throws for n≠1
  (`:775-777`). Future formats are never truncated.
- **Store axis:** each versioned codec carries `CurrentSaveVersion` (+ optional
  `MigrationFromVersion`); the curated six report through `VersionReport.SaveSchemaVersions`
  (holdfast v5, year_of_ash v5, dose_ledger v2-from-1, expansion_hub v6, expansion_quest
  v1-from-1, weight_of_choices v2); checksum-envelope sections carry no version and load via
  `SaveChecksum` verification.
- **Provenance:** every aggregate save records `manifest.gameVersion` / `manifest.buildId` —
  the support kit and any compatibility dispute start here.
- **Resilience:** `.bak` rotation (previous file preserved on save) and `.corrupt-*`
  quarantine (checksum failure never destroys the file) are implemented and tested
  (`HoldfastTradeSaveStoreTests`, `Main.UiTests.Holdfast`).

## 12.2 The support window policy (content of `VERSIONING.md` §save)

A release promises: **(a)** it loads every save its own fixtures prove — concretely, envelope
v1 and v2, and each versioned codec from its `MigrationFromVersion` (or v1 where absent)
through current; **(b)** it never silently drops a section it cannot read (checksum envelope
verification failure → quarantine path, never truncation — as implemented); **(c)** the window
only ever *narrows* at a MAJOR release, with the narrowing named in the changelog's
`Save & data compatibility` section and proven by removing the corresponding fixture in the
same commit. The window is *computed* by `SaveSupportWindowTests` enumerating codecs and the
ladder — the doc cites the test, the test cites the constants; there is no third copy.

## 12.3 What a hotfix may and may not change (the iron rule, made executable)

| Change class | Hotfix (`patch`/`data`) | Minor/Major release |
|---|---|---|
| Core gameplay logic, no save-path contact | allowed | allowed |
| Code on save/load paths, no DTO-shape or constant movement | `save-affecting` class: allowed, full fixture matrix mandatory | allowed |
| Codec `CurrentSaveVersion`/`MigrationFromVersion` movement | **forbidden** (gate fails) | allowed with migration + note |
| Save DTO field add/remove/rename (any section, incl. checksum envelopes) | **forbidden** | allowed with version bump + migration + note |
| Envelope `manifestVersion` movement | **forbidden** | MAJOR only, with in-place migration |
| Data catalog value fix, `schema_version` unchanged | allowed (`data` class) | allowed |
| Data `schema_version` bump | **forbidden** | allowed with note |

Enforcement is layered: (1) `version-gate.py --hotfix --base <tag>` diffs constants, DTO
shapes (field-line diff under `Assets/Ashfall.Core/**Save*.cs` and registered section state
types), and data `schema_version` values against the release tag — any movement fails; (2) the
release-tier fixture matrix loads the **tag's** fixtures and asserts **byte-identical
checksums** after an in-memory save round-trip — "loads unchanged and produces identical
checksums" is the exact property, per the source plan's 48C step 3; (3) classification in the
PR title is matched against the gate's computed class — a `hotfix(patch):` PR that the gate
computes as `save-affecting` fails until retitled, so the label can never lie.

## 12.4 Rollback semantics (anchored on existing machinery)

- **Patch-class rollback is always save-safe** by construction: no schema moved, so the older
  binary reads everything the newer one wrote. Procedure: restore previous artifact; no save
  action needed.
- **Release-line rollback after a migrating release (minor/major) is the dangerous case**, and
  the policy states it honestly: a save already *rewritten* as current by the newer build
  (`:639-644`) may be refused by the older binary. Mitigations, all existing: the `.bak`
  rotation preserves one pre-rewrite generation; the support playbook's first instruction for
  any migrating release is "keep a copy of the save directory before first launch" (player
  messaging in the patch-notes template); and rollback of a migrating release is defined as
  *re-issue a fixed newer release*, not *revert the binary* — written down so nobody
  improvises the wrong one during an incident.
- **Corruption path:** checksum failure → quarantine + `.bak` reload (as tested); the hotfix
  playbook references this instead of inventing a recovery flow.

## 12.5 Data-only hotfix route

A broken catalog whose fix changes **values only** (no `schema_version` movement, no id
removals) can ship as a data drop: the `ASHFALL_DATA` override (CatalogPath resolver
precedence #1) and the sealed pack mechanics (`docs/mods/MOD_CONTRACT.md`) make a corrected
Data tree deliverable without a binary rebuild. Policy statement (written into HOTFIX.md):
supported, with three conditions — the fix validates clean through
`--data-integrity-selftest` on the release line; the changelog entry still generates (48B
discipline does not relax under pressure); and the next binary release absorbs the same fix so
the override is temporary. Emergency content *quarantine* (disabling a broken family) routes
through the existing content-acceptance/exemption machinery (Plan 45, sealed) rather than a
new switch — the playbook links `scripts/ci/content-acceptance-gate.sh` and the acceptance
ladder docs.

## 12.6 Fixture corpus commitment

One real save per supported migration source, committed under
`artifacts/golden_saves/historical/`: initially (i) one envelope `manifestVersion: 1`
aggregate (constructible from the V1-shape fixtures the `SaveSlotService` migration tests
already build, exported once through the real writer), and (ii) one fixture at each curated
codec's `MigrationFromVersion` (dose_ledger v1, expansion_quest v1) plus one at a mid-ladder
version for the two longest ladders (holdfast v3 of 5, year_of_ash v3 of 5) to pin the
multi-step path. Fixtures are synthesized *through the production codecs* (write with an old
writer captured at that version's commit, or hand-trimmed from a current fixture with the
frozen V-shape constants the codecs keep — the repo already preserves frozen shapes, e.g.
RadioSave V4/V5), and every fixture's provenance is recorded in the manifest (source commit,
construction method), per FIXTURE_POLICY's explicit-fixture rules.

---

# 13. Determinism

No Core deterministic simulation behavior is affected — this package ships no gameplay code.
The determinism-relevant surfaces are the new tooling, and they are held to the same standard:
`generate_changelog.py` and `version-gate.py` are pure functions of (repo state, base ref,
arguments) — generated regions contain only commit-derived content (the release date is a
required CLI argument, never a wall-clock read), entries are sorted stably (by commit date,
then hash), and `--check` is byte-exact. `SaveSupportWindowTests` and the fixture matrix
assert checksum-*identical* round-trips, which is the existing `SaveChecksum`
culture-invariant contract doing the work; the new tests add no new nondeterminism surface.
Two consecutive `generate_changelog.py --print` runs on the same refs must be byte-identical —
pinned by a case in the script's `--self-test`.

---

# 14. System/Event Wiring

Minimal by design. (a) `HostCli.PrintVersion` — the four-line fallback hardening (§6.2); no
new events, no new subscriptions. (b) One new selftest descriptor for
`--release-fixture-selftest` registered through the existing `HostCliRegistry` seam, with
`generate-selftest-manifest.py --check` regenerated (never hand-edited) — same pattern every
selftest in `HostCli.cs:495-554` already follows. (c) No Core events added; `VersionReport`
stays a pure renderer. No panel, no UI route, no day-owner, no campaign tick contact.

---

# 15. Godot Integration

- `project.godot`: one line ever changes (`config/version`), written only by
  `set_version.py`. Godot bakes it into exports automatically — that is why it stays the
  runtime authority.
- `export_presets.cfg`: the two Windows version fields join the script-owned set (fixing the
  current 1.0.0/1.1.0 drift). The file is commit-tracked since `8ab78729` (the
  `MicroLocationExportParity` gate requires it); both presets otherwise untouched.
- `src/Host/HostCli.cs`: `PrintVersion` hardening only.
- CI: `build.yml`'s inline export steps are replaced by `bash scripts/ci/export-build.sh`
  (plus a Windows-preset export kept as-is, since `export-build.sh` is Linux-scoped — the
  Windows template gap recorded as a 1.1.0 known issue persists and stays recorded honestly);
  new `release.yml` triggers on `v*` tag pushes and runs `release-gate.sh` + artifact upload;
  new `hotfix.yml` triggers on `hotfix/*` pushes and runs fast tier + the tag-fixture matrix.
  Godot version/setup steps copy the existing `chickensoft-games/setup-godot@v2` 4.7.1-mono
  block verbatim.
- Engine-free boundary: `ReleaseVersion.cs` and both new test files contain zero Godot
  references; the `forbidden_core_apis` gate proves it on every run.

---

# 16. Narrative/Content Integration

Not applicable — a tooling/process package with zero narrative or catalog content. The single
tone-bearing artifact it does produce is the patch-notes template
(`docs/releases/TEMPLATE.md`), which inherits the project tone rule (AGENTS.md: restrained,
human, fictional; no real countries/wars/people): the template's `### Highlights` guidance
explicitly bans internal jargon (gate IDs, plan numbers stay in the refs line, not in prose)
and requires known issues to be stated plainly.

---

# 17. Failure Modes

| # | Failure mode | Detection | Consequence if undetected | Mitigation in this design |
|---|---|---|---|---|
| F1 | Hotfix changes a save-schema constant or DTO shape | `version-gate.py --hotfix` constant/shape diff + fixture checksum matrix | Released hotfix silently strands or migrates player campaigns — the "unforgivable bug" | Both layers are merge-blocking on `hotfix/*`; classification label must match computed class |
| F2 | Version bump without changelog entry | `version_gate` bump-changelog link rule | Release notes rot; support cannot reconstruct what shipped | Fast-tier gate; self-test proves the negative case |
| F3 | Tag pushed for a version the tree doesn't contain | `prepare-release.sh --check` (tag-vs-version equality pre-tag) + `release.yml` re-verify on the tag | Tag points at a lie; all downstream diffs mis-scope | Tag created only after gate-green; re-verified post-push by CI |
| F4 | Export preset / assembly version drift from `project.godot` | `version_gate` sources-agree rule (would have caught today's 1.0.0/1.1.0 preset drift) | Windows artifact metadata contradicts the game; support confusion | One writer script; equality test pin |
| F5 | `HostCli` prints `unknown`/empty version in a shipped build | `ReleaseVersionContractTests` (project.godot parses as strict semver) + `INVALID` loud render + export smoke checks `--version` output | Unanswerable support tickets (source plan's exact warning) | Fallback hardened; smoke boot asserts the version line |
| F6 | Changelog hand-edited inside generated region | `changelog_drift` gate (`--check` regeneration diff) | History and document diverge silently | Generated markers; drift is CI-fatal |
| F7 | Fixture corpus rots (fixtures "updated" to pass) | Fixture immutability rule + manifest hashes + a fixture edit requires a window-policy edit in the same commit | Support window narrows silently; F1 becomes possible | Policy in FIXTURE_POLICY addendum; reviewer checklist item in TEMPLATE.md |
| F8 | Release gate run in a linked worktree gives false failures (1.1.0 lesson) | `release-gate.sh` preflight detects `.git`-file worktrees and refuses with the recorded explanation | Repeated 2026-09-07 NO-GO confusion | The 1.1.0 record's process lesson becomes a preflight check |
| F9 | Codec curated list drifts from reality (e.g. radio v6 unlisted) | `SaveSupportWindowTests` enumerates *all* version-carrying codecs and reports any not in `VersionReport.SaveSchemaVersions` | `--version` under-reports the save axis | Test forces an explicit per-codec curation decision (extend list via sanctioned pin-bump, or document exclusion) |
| F10 | Data-only hotfix ships a `schema_version` bump by accident | `version-gate.py --hotfix` data rule | Override pack becomes a silent mod-contract break | Same gate, data axis |
| F11 | Push automation goes wrong (wrong remote/branch, force-push past tag) | — | Published-history damage — hard to recover | Scripts never push; they print commands. PROCESS.md states the no-force-push-past-a-tag rule; tags are annotated and append-only |
| F12 | New gate flakes and gets quarantined into meaninglessness | `quarantine.json` policy (14-day expiry, owner, reason; protected list) | The release net silently reopens | Quarantine entries expire; release-gate refuses to run with any release-class gate quarantined |
| F13 | Backfilled `[1.1.0]` changelog mis-attributes changes | Human review of generated section before commit (required step, not optional) | Permanent historical misrecord | Generation from real history + review + the lane record (`RELEASE_1.1.0.md`) as cross-check |
| F14 | `run-gates.py` manifest edits desync from runner expectations | Existing manifest consumers (`verify-capability-claims.py` gate-ID validation, `verify-fast.sh`) run in the same commit | CI red on the package itself | Additive-only edits; schema_version bump documented |

---

# 18. Test Strategy

Per `TEST_POLICY.md`: focused targets, new files run alone first through
`scripts/run_test.sh` (180s cap), no full-suite runs by builders, aggregation only for
homogeneous static mappings, and release/full-tier runs confined to the release window (the
release *is* the explicit reason the policy requires).

**New test files (both engine-free, both run alone first):**

1. `Ashfall.Core.Tests/Release/ReleaseVersionContractTests.cs` (~14 cases):
   `TryParse` accept/reject table (`1.2.3` ✓; `1.2`, `v1.2.3`, `1.2.3.4`, `1.2.3-rc`, empty,
   null ✗); `ClassifyBump` table (1.1.0→1.1.1 patch, →1.2.0 minor, →2.0.0 major, →1.0.9
   invalid, garbage invalid); **live-repo pins** (the `VersionReportContractTests`
   `FindRepoRoot()` pattern): `project.godot` version parses strict; equals
   `Directory.Build.props` `VersionPrefix`; equals both `export_presets.cfg` Windows fields;
   `VersionReport.Compose` with an `INVALID` marker string renders the pinned shape (the
   existing suite's shape cases stay green — additive coverage, no edits to the old file).
2. `Ashfall.Core.Tests/Save/SaveSupportWindowTests.cs` (~30 cases):
   envelope v1→v2 migration succeeds and v3 refuses (through `SaveSlotService.MigrateToCurrent`
   semantics, via the public load path); every versioned codec discovered by scanning Core for
   `CurrentSaveVersion`/`MigrationFromVersion` constants has its declared floor migrate to
   current (the `VersionReportContractTests` enumeration pattern, generalized); every
   committed historical fixture loads and round-trips checksum-stable; the curation-drift case
   (F9) reports any versioned codec absent from `VersionReport.SaveSchemaVersions` as an
   explicit, named failure. Save/load, migration, and state-transition cases stay independent
   per the aggregation policy — no aggregation here.

**Script-level proof:** `version-gate.py --self-test` and `generate_changelog.py --self-test`
(temp fixture trees; the repo's established failure-test pattern).

**Gate-level verification:** after manifest registration, `python3 scripts/ci/run-gates.py
--list` shows the new gates; `bash scripts/ci/verify-fast.sh` runs them in parity with CI.

**Release-window verification (dedicated, explicit):** one full `release-gate.sh` dry run,
one `prepare-release.sh --dry-run`, one rehearsed hotfix dry run against `v1.1.0` with a
deliberately-broken candidate (changelog section deleted → must fail; a save-constant touched
→ must fail) and one clean candidate (must pass). The 1.1.0 tag is the rehearsal target
precisely because it is the only tag: rehearsing against it exercises every path without
needing a new release to exist first.

**Focused commands (builder default set):**
`bash scripts/run_test.sh Ashfall.Core.Tests/Release/ReleaseVersionContractTests.cs` ·
`bash scripts/run_test.sh Ashfall.Core.Tests/Save/SaveSupportWindowTests.cs` ·
`dotnet build Ashfall.csproj --nologo` (host change) ·
`python3 scripts/ci/version-gate.py --self-test` ·
`python3 scripts/release/generate_changelog.py --self-test` ·
`python3 scripts/ci/verify-capability-claims.py` · `python3 scripts/ci/run-gates.py --list`.

---

# 19. Dependency-Ordered Phases

Execution order honors the source plan's 48A→48B→48C chain, premise-corrected per §2.5
(29A sealed; 39A and 46A/46B absent — their contributions are scoped out or built minimal
here). Each phase ends green on its focused verification before the next begins; phases 1–2
must land before anything tag-shaped is attempted ("an untested tag is a promise with no
evidence").

- **Phase 0 — Premise audit + claims (no production change).** Builder re-verifies the §2.5
  table against source (spot-check: `git tag -l`; `grep -n unknown src/Host/HostCli.cs`;
  `grep version project.godot export_presets.cfg`; manifest counts), records deltas, and the
  foreman files the package claim (`claim-c2-21-release-craft-…`) with §7's paths. *Exit:*
  claim row exists; no code touched.
- **Phase 1 — Version policy + truth (48A core).** Write `docs/releases/VERSIONING.md` (three
  axes, semver mapping, support window citing the ladder); add `ReleaseVersion.cs`;
  `Directory.Build.props` `<VersionPrefix>1.1.0</VersionPrefix>`; sync `export_presets.cfg`
  Windows fields to 1.1.0 (the drift fix); harden `PrintVersion`; new
  `ReleaseVersionContractTests`. *Verify:* focused test file alone; `dotnet build
  Ashfall.csproj` 0/0; `--version` output eyeballed once via headless run.
- **Phase 2 — Drift gates.** `scripts/ci/version-gate.py` (+`--self-test`);
  `generate_changelog.py` skeleton with `--check` (machine regions only); register
  `version_gate` + `changelog_drift` fast gates (manifest 53→55, schema_version note);
  CLAIMS.json rows for version/changelog (same commit as the gates they cite — verifier
  ordering). *Verify:* self-tests; `verify-fast.sh` green; verifier green.
- **Phase 3 — Save support window + fixtures (48A finish).** `SaveSupportWindowTests`;
  historical fixture corpus + manifest entries + FIXTURE_POLICY addendum;
  `--release-fixture-selftest` descriptor + selftest-manifest regeneration; register
  `save_support_window` (full) and `release_fixture_matrix` (release) gates; the F9 curation
  report runs and its outcome (extend curated list or document exclusions) is decided with
  the integrator — default: document, do not expand `VersionReport` scope in this package.
  *Verify:* focused test file alone; selftest headless run; `run-gates.py --list`.
- **Phase 4 — Release pipeline (48B).** `PROCESS.md`, `TEMPLATE.md`; full changelog generator
  (mapping, refs, save/data/mod diff sections); `[1.1.0]` backfill generated and
  human-reviewed; `release-gate.sh`; `prepare-release.sh`; `build.yml` adopts
  `export-build.sh`; `release.yml`; `export_smoke_boot` gate registration. **Foreman
  checkpoint:** the retro-`v1.0.0`-tag recommendation from the 1.1.0 record is presented for
  decision (execute at `9b4985d0` or formally decline); tag operations themselves are
  user-approved acts. *Verify:* `prepare-release.sh --dry-run` end-to-end; deliberately-broken
  candidate fails; `release-gate.sh` green in the dedicated window.
- **Phase 5 — Hotfix path (48C).** `HOTFIX.md` (classification table, iron rule, rollback
  semantics, data-only route, emergency quarantine link); `hotfix.sh` (+`--classify`,
  `--check`); `hotfix.yml`; `SUPPORT.md` triage kit; `POSTMORTEM_TEMPLATE.md`; rehearsal:
  branch from `v1.1.0`, land a trivial patch-class change, full flow to local tag
  `v1.1.1-rehearsal` (deleted after), fixture matrix checksum-identical proof recorded in
  `docs/releases/` as the rehearsal record. *Verify:* rehearsal record exists; gate refusal
  cases proven.
- **Phase 6 — Hygiene + close.** `ashfall-release-captain` SKILL.md vocabulary fix
  (`lane/snap`/`bit`-style → actual git discipline: branch, reviewable commits, tag only via
  the script, never push scripted) + pointer to `prepare-release.sh`; regenerate
  `AGENT_SKILLS_INDEX.md`; regenerate `docs/INDEX.md`; builder hands the foreman/integrator
  the exact ledger rows (INTEGRATION_PLANS batch note, census `C2[21]` → status change,
  WAVE_LEDGER row, KNOWN_DEBT — expected: no new debt). *Verify:* `sync-agent-rulebooks.py
  --check`, `generate-docs-index.py --check`, `generate-agent-skills-catalog.py --check`,
  `verify-capability-claims.py`, `verify-fast.sh` — all green; closeout
  `docs/plans/PLAN_48_RELEASE_CRAFT_CLOSEOUT.md` (subject-qualified name, §4.0 rule).

---

# 20. File Impact Map

Every change, with its reason. **New files (15):**

| Path | Reason |
|---|---|
| `docs/plans/PLAN_48_RELEASE_CRAFT_INTEGRATION_PLAN.md` | This plan |
| `docs/releases/VERSIONING.md` | The three-axis policy + support window (D1) |
| `docs/releases/PROCESS.md` | Branch/tag model, release checklist as procedure (D4) |
| `docs/releases/HOTFIX.md` | Classification, iron rule, rollback, data-only route (D5) |
| `docs/releases/TEMPLATE.md` | Patch-notes/release-record template (D4) |
| `docs/releases/SUPPORT.md` | Triage kit from existing reports (D5) |
| `docs/releases/POSTMORTEM_TEMPLATE.md` | "Which gate would have caught this" loop (D5) |
| `scripts/release/set_version.py` | Single writer for the three version sites (D1) |
| `scripts/release/generate_changelog.py` | Changelog generation + `--check` (D4) |
| `scripts/release/prepare-release.sh` | Release as one command (D4) |
| `scripts/release/hotfix.sh` | Hotfix mechanics (D5) |
| `scripts/ci/version-gate.py` | Drift enforcement (D2) |
| `Assets/Ashfall.Core/ReleaseVersion.cs` | Engine-free semver parse/classify (D1) |
| `Ashfall.Core.Tests/Release/ReleaseVersionContractTests.cs` | Version truth pins (D1) |
| `Ashfall.Core.Tests/Save/SaveSupportWindowTests.cs` | Support window + fixtures (D3) |

**Modified files (11):** `project.godot` (script-owned version line; initial no-op sync),
`Directory.Build.props` (+`<VersionPrefix>`), `export_presets.cfg` (Windows fields 1.0.0→1.1.0
drift fix, then script-owned), `src/Host/HostCli.cs` (PrintVersion hardening),
`src/Host/HostCliRegistry.cs` (+1 selftest descriptor), `CHANGELOG.md` ([1.1.0] backfill +
markers), `docs/ci/CI_GATE_MANIFEST.json` (+5 gates, schema_version note),
`docs/architecture/CLAIMS.json` (+4 claims), `.github/workflows/build.yml` (adopt
`export-build.sh`), `.agents/skills/ashfall-release-captain/SKILL.md` (vocabulary + script
pointers), `docs/testing/FIXTURE_POLICY.md` (fixture-immutability addendum).

**New workflows (2):** `.github/workflows/release.yml` (tag-triggered release gate),
`.github/workflows/hotfix.yml` (hotfix branch gates against tag fixtures).

**New fixtures:** `artifacts/golden_saves/historical/*` + `manifest.json` entries (D3).

**Regenerated, never hand-edited:** `docs/INDEX.md`, `AGENT_SKILLS_INDEX.md`,
`docs/ci/SELFTEST_MANIFEST.json`.

**Governance (foreman/integrator-written, builder supplies rows):** `INTEGRATION_PLANS.md`,
`WORKTREE_OWNERSHIP.md`, `docs/plans/UNCLAIMED_CORPUS_CENSUS.md`, `docs/roadmap/WAVE_LEDGER.md`,
`KNOWN_DEBT.md` (only if debt emerges).

**Explicitly untouched (collision check):** all `Assets/StreamingAssets/Data/**`; all save
codecs and `SaveSectionRegistry.cs`; `VersionReport.cs` (unless the F9 decision extends the
curated list — integrator call, sanctioned pin-bump only); both active-claim path sets;
`docs/weather/PLAN_48_WEATHER_ROUTE_GATES_CLOSEOUT.md` and all weather Plan 48 material;
`.github/workflows/ci.yml` (new fast gates flow through the manifest); `scripts/ci/run-gates.py`
(release tier already exists); `scripts/ci/export-build.sh` / `godot-export-linux.sh`
(called, not modified).

---

# 21. Risks

| Risk | Likelihood | Impact | Handling |
|---|---|---|---|
| Changelog generation quality poor on the 76% non-conventional commit majority | High | Medium | `Other changes` bucket verbatim + human `Highlights` margin + [1.1.0] backfill is human-reviewed before commit; generator improves signal over time as plan-id citation discipline spreads (already rising in Wave 10/11 commits) |
| Historical fixture synthesis through frozen V-shapes misrepresents a real old save | Medium | High | Provenance recorded per fixture (source commit + construction method); where possible prefer writing with the actual old binary/writer at that commit; fixtures prove the *migration code path*, which is the contract being pinned |
| Gate-range heuristics (merge-base) misfire in unusual topologies (e.g. current `Zcode_Branch` vs `main` divergence) | Medium | Low | `VERSION_GATE_BASE` env override; vacuous-pass documented; release-line runs always pass explicit refs |
| Release-tier gates extend CI wall time | Low | Low | Release tier never runs on PR; fast additions are two sub-second scripts |
| `docs/releases/` vs source plan's `docs/release/` naming confuses future agents | Low | Low | This plan + PROCESS.md state the correction; docs-index regeneration picks up the new files |
| Foreman declines retro-tag; base-ref logic then spans repo root | Low | Low | Generator accepts `--base <ref>`; backfill uses `9b4985d0` explicitly regardless of whether it carries a tag |
| Skill edit desyncs skills catalog | Low | Low | Same-commit regeneration + existing drift gate |
| Rehearsal tag `v1.1.1-rehearsal` pollutes history | Low | Low | Local-only, deleted in the same phase; push is never scripted (F11) |

---

# 22. Out of Scope

- Long-run soak / session-durability release criteria — `C2[16]`/Plan 39, unexecuted; this
  package's release gate composes *existing* manifest gates only.
- Play-metrics/funnel-informed patch notes — `C2[20]`/Plan 46, unexecuted; balance lines cite
  existing `docs/balance/*.md` evidence until then.
- Mod-contract changes themselves — Plan 47 is sealed; this package only *reads* its breaking-
  change signals. No public modding promises.
- Any Unity artifact, any `Assets/_Game/` structure, any gameplay/UI/audio/content work.
- Store deployment, signing, itch/Steam logistics, player-facing distribution of artifacts.
- Renumbering or reconciling the historical `piagentsplans/` backlog (census-owned, Rule: no
  silent renumbering).
- Expanding `VersionReport`'s curated codec list beyond the F9 documented-decision path.
- Fixing the pre-existing Windows export-template environment gap (recorded 1.1.0 known
  issue; environment, not repo).

---

# 23. Rollback Strategy

The package is additive by construction; rollback is subtraction in reverse phase order, and
no step strands anything:

1. **Gates:** new manifest entries are non-protected by default — a flaky new gate can be
   quarantined via the existing `quarantine.json` policy (owner, reason, 14-day expiry)
   while it is fixed; removing the five entries returns the manifest to 53 gates
   (`schema_version` note reverted with them).
2. **Scripts/docs:** all `scripts/release/*`, `version-gate.py`, and `docs/releases/*` (except
   the pre-existing `RELEASE_1.1.0.md`) delete cleanly; nothing references them except the
   manifest entries and claims removed in the same revert.
3. **Core/host:** `ReleaseVersion.cs` + its tests delete cleanly (no Core consumer outside
   tests); the `PrintVersion` hardening reverts as a 4-line diff; the selftest descriptor
   removal + manifest regeneration restores the registry.
4. **Version files:** `Directory.Build.props` `<VersionPrefix>` removal restores SDK defaults
   (no behavioral dependency is created on it); `export_presets.cfg` keeps its corrected
   1.1.0 values regardless — that sync is a bug fix, not package baggage.
5. **CHANGELOG.md:** the `[1.1.0]` backfill is *historical record* — a package rollback does
   not delete it (it documents a real tag); generated markers are inert comments without the
   generator.
6. **Tags:** the package creates none by default (foreman checkpoint, Phase 4); if the
   rehearsal tag exists it is local and deleted in-phase. Nothing is ever force-pushed.

---

# 24. Definition of Done

Binding per `docs/roadmap/README.md` §3 (this package's deliverable classes are **Build** and
**Plan**):

1. `bash scripts/ci/release-gate.sh` is the entire pre-release verification: it composes
   fast + full + release tiers + `export-build.sh`, emits
   `build/reports/release-gate-report.json`, and has run green once in a dedicated window.
2. `bash scripts/release/prepare-release.sh --dry-run` executed end-to-end exactly once,
   producing: version triple written by script, generated changelog section, gate report,
   `docs/releases/RELEASE_<dryrun>.md` from the template, and a local (unpushed) tag —
   with a deliberately-broken candidate (missing changelog section; touched save constant)
   proven to fail first.
3. "Can build X load save Y with data Z?" is answered by `SaveSupportWindowTests` +
   `release_fixture_matrix`, not prose: every committed fixture (golden + historical) loads
   and round-trips checksum-identical on the candidate.
4. The hotfix rehearsal record exists in `docs/releases/`: branch from `v1.1.0`, patch-class
   fix, gates against the tag's fixtures, local tag, checksum-identical proof, refusal proof
   for a schema-touching candidate.
5. `CHANGELOG.md` carries a human-reviewed `[1.1.0]` section; `changelog_drift` and
   `version_gate` are green fast-tier gates; the export-preset version drift is fixed.
6. `verify-capability-claims.py` passes with the four new claims; all generator `--check`
   gates (rulebooks, docs-index, skills catalog, selftest manifest) are green after
   regeneration; `docs/plans/PLAN_48_RELEASE_CRAFT_CLOSEOUT.md` (subject-qualified) records
   commands, results, and limitations.
7. Governance rows (claim status, census `C2[21]`, wave ledger) written by the
   foreman/integrator — not the builder.
8. No production gameplay, data, or save-codec diff exists in the package (the only Core
   addition is the engine-free `ReleaseVersion`; the only host diffs are `PrintVersion` + one
   selftest descriptor).

---

# 25. Implementation Handoff

**Bounded outcome:** release craft as gated tooling — version truth, drift gates, tested save
support window, one-command release, rehearsed save-safe hotfix. **Non-goals:** §22 (soak,
metrics-driven notes, mod-contract edits, distribution, gameplay). **Current evidence:** §4.1's
citation table; the builder re-verifies §2.5's ten rows in Phase 0 before the first edit.
**Contract:** §6 (three axes; script-owned version triple; generated-region changelog; iron
hotfix rule; existing-seam-only). **Commands/results expected:** §18's focused set per phase,
with the release-window runs confined to Phase 4/5's dedicated windows. **Limitations:**
changelog quality bounded by commit-message reality; fixture synthesis provenance-dependent;
39A/46A absences scoped around, not solved. **Shared paths intentionally untouched:**
`VersionReport.cs` (pending the F9 integrator decision), `run-gates.py`, `export-build.sh`,
`ci.yml`, both active claims' path sets, all governance ledgers (rows supplied, not written).

## MUST PRESERVE

- Godot authority and the engine-free Core boundary (`ReleaseVersion.cs` carries zero engine
  references; `forbidden_core_apis` stays green).
- `VersionReport` as the single version-rendering authority and its pinned contract shape;
  codec curation changes only via the sanctioned test-comment pin-bump practice.
- The envelope migration ladder and refusal semantics (`SaveSlotService`), the `.bak` /
  `.corrupt-*` resilience machinery, and every existing save fixture's immutability.
- `docs/releases/RELEASE_1.1.0.md` and the `v1.1.0` tag as historical fact; Keep-a-Changelog
  format of `CHANGELOG.md`; the existing 53-gate manifest behavior for all current gates.
- TEST_POLICY focused-verification discipline; the quarantine policy's constraints; the
  foreman/integrator ownership of all governance ledgers; both active worktree claims.
- The repo rule that generated outputs (docs index, skills catalog, selftest manifest) are
  regenerated through their generators, never hand-edited.

## MUST ADD

- `docs/releases/VERSIONING.md` (three-axis policy + semver mapping + save support window),
  `PROCESS.md`, `HOTFIX.md`, `TEMPLATE.md`, `SUPPORT.md`, `POSTMORTEM_TEMPLATE.md`.
- `scripts/release/{set_version.py, generate_changelog.py, prepare-release.sh, hotfix.sh}`
  and `scripts/ci/version-gate.py`, each with the repo-conventional `--check`/`--self-test`
  discipline.
- `Assets/Ashfall.Core/ReleaseVersion.cs` (engine-free) + `ReleaseVersionContractTests.cs`
  and `SaveSupportWindowTests.cs` (run alone first, focused).
- Five manifest gates (`version_gate`, `changelog_drift` fast; `save_support_window` full;
  `release_fixture_matrix`, `export_smoke_boot` release) filling the pre-provisioned release
  tier; four CLAIMS.json rows in the same commit as their evidence gates.
- Historical fixture corpus under `artifacts/golden_saves/historical/` with manifest
  provenance; the `[1.1.0]` changelog backfill; the export-preset version sync; the
  `PrintVersion` `INVALID` hardening; the release-captain skill vocabulary fix.
- One dry-run release and one rehearsed hotfix, both recorded in `docs/releases/`.

## MUST NOT DO

- Never create `docs/release/` (singular), a second version report, a parallel fixture
  system, a new registry/ledger, or any save-section/state change; never touch gameplay Core,
  data catalogs, or save codecs.
- Never use `System.Random`, wall-clock-seeded behavior, or nondeterministic output in the
  generators (dates are arguments); never hand-edit generated regions/outputs to force a pass
  (the exact death of a version policy, per the source plan's guardrail).
- Never push, force-push, move a tag, or script a push; never tag before Phase 1–2 gates
  exist; never let a hotfix carry a schema change; never execute the retro-`v1.0.0` tag
  without the foreman checkpoint decision.
- Never conflate this package with the weather Plan 48 — always cite `C2[21]`/"Plan 48
  (Release Craft)"; never renumber any historical file.
- Never run the full suite outside the dedicated Phase 4/5 windows; never quarantine a new
  gate without the policy fields; never edit the governance ledgers as the builder.

## VERIFY WITH

- Phase-local (focused): `bash scripts/run_test.sh Ashfall.Core.Tests/Release/ReleaseVersionContractTests.cs`;
  `bash scripts/run_test.sh Ashfall.Core.Tests/Save/SaveSupportWindowTests.cs`;
  `python3 scripts/ci/version-gate.py --self-test`;
  `python3 scripts/release/generate_changelog.py --self-test`;
  `dotnet build Ashfall.csproj --nologo` (0/0).
- Package gates: `bash scripts/ci/verify-fast.sh` (incl. the two new fast gates);
  `python3 scripts/ci/verify-capability-claims.py`;
  `python3 scripts/ci/sync-agent-rulebooks.py --check`;
  `python3 scripts/ci/generate-docs-index.py --check`;
  `python3 scripts/ci/generate-agent-skills-catalog.py --check`;
  `python3 scripts/ci/generate-selftest-manifest.py --check`;
  `godot --headless --path . -- --release-fixture-selftest` (via `run-godot-bounded.sh`).
- Release window (dedicated, explicit reason per TEST_POLICY):
  `bash scripts/ci/release-gate.sh`; `bash scripts/release/prepare-release.sh --dry-run`;
  `bash scripts/release/hotfix.sh v1.1.0 --class patch --check`; broken-candidate refusal
  proofs; `git tag --points-at HEAD` inspected by a human.

## FIRST SAFE IMPLEMENTATION STEP

Phase 0, read-only: re-verify the §2.5 premise table against current source (`git tag -l`;
`grep -n 'unknown' src/Host/HostCli.cs`; `grep -n version project.godot export_presets.cfg
Directory.Build.props`; the `CI_GATE_MANIFEST.json` counts; the census row), record any delta
found, and hand the foreman the claim row for
`claim-c2-21-release-craft-<date>` covering exactly the §7 builder paths — then stop for the
claim before writing anything else.
